Imports System.IO
Imports System
Imports log4net
Imports System.Web.Security

Namespace ANAGRAFICAWEB
    ''' <summary>
    ''' Classe generale che eredita Page.
    ''' Viene inclusa in tutti i form perché contiene le funzioni basi che tutti i form devono avere.
    ''' </summary>
    ''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    Public Class BasePage
        Inherits Page
        Private Shared Log As ILog = LogManager.GetLogger(GetType(BasePage))
        Private Const CONCURRENCY_OBJECT As String = "ConcurrencyObject"
        Public Shared authCookie As HttpCookie
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub BasePage_Init(sender As Object, e As EventArgs) Handles Me.Init
            Try
                Dim isExpired As Boolean = False
                'get cookie
                authCookie = HttpContext.Current.Request.Cookies.Get("aplckute")
                If Not authCookie Is Nothing Then
                    Dim authTicket As FormsAuthenticationTicket = FormsAuthentication.Decrypt(authCookie.Value)
                    If Not authTicket Is Nothing Then
                        If authTicket.Expired = False Then
                            Dim myData As String() = authTicket.UserData.Split("|")
                            If Not myData Is Nothing Then
                                If myData.GetUpperBound(0) <> 1 Then
                                    Log.Debug("sessione scaduta perchè non ho userdata nel cookie")
                                    isExpired = True
                                Else
                                    'If myData(0) <> ConstSession.UserName Then
                                    Log.Debug("utente cookie->" + myData(0) + "  utente sessione->" + ConstSession.UserName)
                                    If ConstSession.UserName <> "" Then
                                        'Log.Debug("sessione scaduta perchè utente cookie diverso da utente sessione")
                                        'isExpired = True
                                    Else
                                        Session("username") = myData(0)
                                        End If
                                    'End If
                                End If
                            Else
                                Log.Debug("sessione scaduta perchè userdata cookie null")
                                isExpired = True
                            End If
                        Else
                            Log.Debug("sessione scaduta perchè cookie scaduto")
                            isExpired = True
                        End If
                    Else
                        Log.Debug("sessione scaduta perchè non ticket cookie")
                        isExpired = True
                    End If
                Else
                    Log.Debug("sessione scaduta perchè non cookie")
                    isExpired = True
                End If
                If ConstSession.UserName = "" Then
                    Log.Debug("sessione scaduta perchè sessione operatore vuoto")
                    isExpired = True
                End If
                If isExpired = True Then
                    HttpContext.Current.Session("username") = ""
                    RegisterScript("GestAlert('a', 'warning', 'CmdLogOut', '', 'Sessione scaduta rieffettuare LOGIN');", Me.GetType)
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - Anagrafica.BasePage_Init.errore: ", ex)
            End Try
        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="script"></param>
        ''' <param name="type"></param>
        Protected Sub RegisterScript(ByVal script As String, ByVal type As Type)
            ConstSession.CountScript = (ConstSession.CountScript + 1)
            Dim uniqueId As String = ("spc_" _
                    + (ConstSession.CountScript.ToString _
                    + (DateTime.Now.ToString + ("." + DateTime.Now.Millisecond.ToString))))
            Dim sScript As String = "<script language='javascript'>"
            sScript = (sScript + script)
            sScript = (sScript + "</script>")
            ClientScript.RegisterStartupScript(type, uniqueId, sScript)
        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="obj"></param>
        Protected Sub SetConcurrencyObject(ByVal obj As AnagInterface.DettaglioAnagrafica)
            ViewState.Add(CONCURRENCY_OBJECT, obj)
        End Sub
    End Class

    Public Class clsFncAnag
        Private Shared Log As ILog = LogManager.GetLogger(GetType(clsFncAnag))

        ''' <summary>
        ''' '
        ''' </summary>
        ''' <param name="myApplicationsEnabled">string</param>
        ''' <param name="myAmbiente">string</param>
        ''' <param name="idente">string</param>
        ''' <param name="myCognome">string</param>
        ''' <param name="myNome">string</param>
        ''' <param name="myCodiceFiscale">string</param>
        ''' <param name="myPartitaIva">string</param>
        ''' <param name="myCodContribuente">string</param>
        ''' <param name="myComuneResidenza">string</param>
        ''' <param name="myProvinciaResidenza">string</param>
        ''' <param name="myDataNascita">string</param>
        ''' <param name="myDataMorte">string</param>
        ''' <param name="myViaResidenza">string</param>
        ''' <param name="myCodViaResidenza">string</param>
        ''' <param name="myTributoInvio">string</param>
        ''' <param name="myTributoPresente">string</param>
        ''' <param name="myParametroRicerca">string</param>
        ''' <param name="myTipoContatto">string</param>
        ''' <param name="nCol">int</param>
        ''' <returns>DataTable</returns>
        ''' <revisionHistory>
        ''' <revision date="04/07/2012">
        ''' <strong>IMU</strong>
        ''' passaggio tributo da ICI a IMU
        ''' </revision>
        ''' </revisionHistory>
        ''' <revisionHistory>
        ''' <revision date="11/2015">
        '''  Funzioni Sovracomunali
        ''' </revision>
        ''' </revisionHistory>
        ''' <revisionHistory>
        ''' <revision date="12/04/2019">
        ''' <strong>Qualificazione AgID-analisi_rel01</strong>
        ''' <em>Esportazione completa dati</em>
        ''' </revision>
        ''' </revisionHistory>
        Public Function Stampa(myApplicationsEnabled As String, myAmbiente As String, idente As String, myCognome As String, myNome As String, myCodiceFiscale As String, myPartitaIva As String, myCodContribuente As String, myComuneResidenza As String, myProvinciaResidenza As String, myDataNascita As String, myDataMorte As String, myViaResidenza As String, myCodViaResidenza As String, myTributoInvio As String, myTributoPresente As String, myParametroRicerca As String, myTipoContatto As String, ByVal nCol As Integer) As DataTable
            Dim DtDatiStampa As New DataTable
            Dim dr As DataRow
            Dim x As Integer
            Dim dvDati As DataView

            Try
                Dim ListAnagraficaExcel As ANAGRAFICAWEB.ListAnagrafica
                Dim objAnagraficaExcel As New ANAGRAFICAWEB.AnagraficaDB(myParametroRicerca)
                Dim dsAnagrafica As New DataSet

                ListAnagraficaExcel = objAnagraficaExcel.GetListAnagragrafica(ConstSession.StringConnectionAnagrafica, True, myCognome, myNome, myCodiceFiscale, myPartitaIva, myAmbiente, idente, myCodContribuente, 0, myComuneResidenza, myProvinciaResidenza, myDataNascita, myDataMorte, myViaResidenza, myCodViaResidenza, 0, myTipoContatto, myTributoInvio, myTributoPresente)
                dsAnagrafica = ListAnagraficaExcel.p_dsItemsANAGRAFICA
                dvDati = dsAnagrafica.Tables(0).DefaultView
                dsAnagrafica = New DataSet
                dsAnagrafica.Tables.Add("STAMPA_ANAGRAFICHE")
                For x = 1 To nCol + 1
                    dsAnagrafica.Tables("STAMPA_ANAGRAFICHE").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
                Next

                DtDatiStampa = dsAnagrafica.Tables("STAMPA_ANAGRAFICHE")
                dr = DtDatiStampa.NewRow
                dr(0) = "Prospetto Anagrafiche"
                dr(2) = "Data Stampa:" & DateTime.Now.Date
                DtDatiStampa.Rows.Add(dr)
                dr = DtDatiStampa.NewRow
                DtDatiStampa.Rows.Add(dr)
                dr = DtDatiStampa.NewRow
                DtDatiStampa.Rows.Add(dr)
                dr = DtDatiStampa.NewRow
                x = 0
                '*** 201511 - Funzioni Sovracomunali ***
                If idente = "" Then
                    dr(x) = "Ente"
                    x += 1
                End If
                dr(x) = "Cognome"
                x += 1
                dr(x) = "Nome"
                x += 1
                dr(x) = "Codice Fiscale/Partita IVA"
                x += 1
                dr(x) = "Sesso"
                x += 1
                dr(x) = "Data Nascita"
                x += 1
                dr(x) = "Luogo Nascita"
                x += 1
                dr(x) = "Data Morte"
                x += 1
                dr(x) = "Indirizzo"
                x += 1
                dr(x) = "CAP"
                x += 1
                dr(x) = "Città"
                x += 1
                dr(x) = "Provincia"
                '*** 20140723 ***
                x += 1
                dr(x) = "Tributo Invio"
                x += 1
                dr(x) = "Nominativo Invio"
                x += 1
                dr(x) = "Indirizzo Invio"
                x += 1
                dr(x) = "CAP Invio"
                x += 1
                dr(x) = "Città Invio"
                x += 1
                dr(x) = "Provincia Invio"
                x += 1
                dr(x) = "Contatto"
                '*** 20120704 - IMU ***
                x += 1
                dr(x) = "ICI/IMU"
                '*** ***
                x += 1
                dr(x) = "TARSU"
                If myApplicationsEnabled.IndexOf("0453") > 0 Then
                    x += 1
                    dr(x) = "OSAP"
                End If
                If myApplicationsEnabled.IndexOf("9253") > 0 Then
                    x += 1
                    dr(x) = "SCUOLA"
                End If
                If myApplicationsEnabled.IndexOf("9000") > 0 Then
                    x += 1
                    dr(x) = "H2O"
                End If
                x += 1
                dr(x) = "PROV"
                DtDatiStampa.Rows.Add(dr)
                For Each myRow As DataRowView In dvDati
                    dr = DtDatiStampa.NewRow
                    x = 0
                    '*** 201511 - Funzioni Sovracomunali ***
                    If idente = "" Then
                        dr(x) = CStr(myRow("DESCRIZIONE_ENTE"))
                        x += 1
                    End If
                    '*** ***
                    dr(x) = CStr(myRow("COGNOME_DENOMINAZIONE"))
                    x += 1
                    dr(x) = CStr(myRow("NOME"))
                    x += 1
                    dr(x) = "'" & CStr(myRow("CFPIVA"))
                    x += 1
                    dr(x) = CStr(myRow("SESSO"))
                    x += 1
                    If CStr(myRow("DATA_NASCITA")) <> "" Then
                        dr(x) = CStr(myRow("DATA_NASCITA"))
                    Else
                        dr(x) = ""
                    End If
                    x += 1
                    dr(x) = CStr(myRow("LUOGO_NASCITA"))
                    x += 1
                    If CStr(myRow("DATA_MORTE")) <> "" Then
                        dr(x) = CStr(myRow("DATA_MORTE"))
                    Else
                        dr(x) = ""
                    End If
                    x += 1
                    dr(x) = (CStr(myRow("VIA_RES")) + " " + CStr(myRow("CIVICO_RES")) + " " + CStr(myRow("ESPONENTE_CIVICO_RES")) + " " + CStr(myRow("INTERNO_CIVICO_RES"))).Trim
                    x += 1
                    dr(x) = CStr(myRow("CAP_RES"))
                    x += 1
                    dr(x) = CStr(myRow("COMUNE_RES"))
                    x += 1
                    dr(x) = CStr(myRow("PROVINCIA_RES"))
                    '*** 20140723 ***
                    x += 1
                    If Not IsDBNull(myRow("tributo_invio")) Then
                        dr(x) = CStr(myRow("tributo_invio"))
                    Else
                        dr(x) = ""
                    End If
                    x += 1
                    If Not IsDBNull(myRow("nome_invio")) Then
                        dr(x) = CStr(myRow("nome_invio"))
                    Else
                        dr(x) = ""
                    End If
                    x += 1
                    If Not IsDBNull(myRow("via_invio")) Then
                        dr(x) = CStr(myRow("via_invio"))
                    Else
                        dr(x) = ""
                    End If
                    x += 1
                    If Not IsDBNull(myRow("cap_invio")) Then
                        dr(x) = CStr(myRow("cap_invio"))
                    Else
                        dr(x) = ""
                    End If
                    x += 1
                    If Not IsDBNull(myRow("comune_invio")) Then
                        dr(x) = CStr(myRow("comune_invio"))
                    Else
                        dr(x) = ""
                    End If
                    x += 1
                    If Not IsDBNull(myRow("pv_invio")) Then
                        dr(x) = CStr(myRow("pv_invio"))
                    Else
                        dr(x) = ""
                    End If
                    x += 1
                    If Not IsDBNull(myRow("descrcontatto")) Then
                        dr(x) = CStr(myRow("descrcontatto"))
                    Else
                        dr(x) = ""
                    End If
                    x += 1
                    If CStr(myRow("ICI")) = "1" Then
                        dr(x) = "X"
                    Else
                        dr(x) = ""
                    End If
                    x += 1
                    If CStr(myRow("TARSU")) = "1" Then
                        dr(x) = "X"
                    Else
                        dr(x) = ""
                    End If
                    If myApplicationsEnabled.IndexOf("0453") > 0 Then
                        x += 1
                        If CStr(myRow("OSAP")) = "1" Then
                            dr(x) = "X"
                        Else
                            dr(x) = ""
                        End If
                    End If
                    If myApplicationsEnabled.IndexOf("9253") > 0 Then
                        x += 1
                        If CStr(myRow("SCUOLA")) = "1" Then
                            dr(x) = "X"
                        Else
                            dr(x) = ""
                        End If
                    End If
                    If myApplicationsEnabled.IndexOf("9000") > 0 Then
                        x += 1
                        If CStr(myRow("H2O")) = "1" Then
                            dr(x) = "X"
                        Else
                            dr(x) = ""
                        End If
                    End If
                    x += 1
                    If CStr(myRow("PROVVEDIMENTI")) = "1" Then
                        dr(x) = "X"
                    Else
                        dr(x) = ""
                    End If
                    DtDatiStampa.Rows.Add(dr)
                Next

                dr = DtDatiStampa.NewRow
                DtDatiStampa.Rows.Add(dr)
                dr = DtDatiStampa.NewRow
                DtDatiStampa.Rows.Add(dr)
                dr = DtDatiStampa.NewRow
                dr(0) = "Totale Contribuenti: " & (dvDati.Count)
                DtDatiStampa.Rows.Add(dr)
                Return DtDatiStampa
            Catch Err As Exception
                Log.Debug(idente + " - Anagrafica.clsFncAnag.Stampa.errore: ", Err)
                Return Nothing
            End Try
        End Function
    End Class
End Namespace