Imports System.Web.HttpContext
Imports OPENUtility
Imports log4net
Imports Utility
''' <summary>
''' Classe per la gestione dell'accesso all'ente
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class SelectEnti
    Private Shared Log As ILog = LogManager.GetLogger(GetType(SelectEnti))
    'Public Function GetEntiByUser(ByVal UserName As String) As DataSet


    '    Dim WFErrore As String
    '    Dim WFSessione As CreateSessione

    '    Try

    '        WFSessione = New CreateSessione(HttpContext.Current.Session("PARAMETROENV"), UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))

    '        If Not WFSessione.CreaSessione(UserName, WFErrore) Then
    '            Log.Fatal("GetEntiByUser 1: Errore durante l'apertura della sessione di WorkFlow: " & WFErrore.ToString)
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        Dim SQL As String

    '        Log.Debug("Ricerca Enti per utente " & UserName)
    '        'SQL = "SELECT ENTI.DESCRIZIONE_ENTE, ENTI.DENOMINAZIONE, ENTI.COD_ISTAT, ENTI.CONTO_CORRENTE, ENTI.INTESTAZIONE_BOLLETTINO, ENTI.COD_BELFIORE,IDENTE_CREDBEN, COD_ENTE_CNC"
    '        SQL += "SELECT ENTI.*"
    '        SQL += " FROM ENTI"
    '        SQL += " INNER JOIN UTENTI_ENTI ON ENTI.COD_ENTE = UTENTI_ENTI.COD_ENTE"
    '        SQL += " INNER JOIN GESTIONE_UTENTI ON UTENTI_ENTI.ID_UTENTE = GESTIONE_UTENTI.ID_UTENTE"
    '        SQL += " WHERE (GESTIONE_UTENTI.USERNAME = '" & UserName & "')"
    '        SQL += " ORDER BY ENTI.DESCRIZIONE_ENTE"

    '        Dim adoRecEnteUtente As DataSet
    '        adoRecEnteUtente = WFSessione.oSession.oAppDB.GetPrivateDataSet(SQL)

    '        Return adoRecEnteUtente

    '    Catch ex As Exception
    '        Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.SelectEnti.GetEntiByUser.errore: ", ex)
    '        Throw New Exception("Problemi nell'esecuzione di GetEntiByUser " + ex.Message)

    '    Finally

    '        If Not IsNothing(WFSessione) Then
    '            WFSessione.Kill()
    '        End If
    '        WFSessione = Nothing

    '    End Try

    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="UserName"></param>
    ''' <returns></returns>
    Public Function GetEntiByUser(ByVal UserName As String) As DataSet
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dsMyDati As New DataSet
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            Log.Debug("Ricerca Enti per utente " & UserName)
            cmdMyCommand.Connection = New SqlClient.SqlConnection(COSTANTValue.ConstSession.StringConnectionOPENgov)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_GetEntiByUser"
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.AddWithValue("@LOGNAME", UserName)
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(dsMyDati)
            Return dsMyDati
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte + "." + COSTANTValue.ConstSession.UserName + " - OPENgov.SelectEnti.GetEntiByUser.errore: ", ex)
            Throw New Exception("Problemi nell'esecuzione di GetEntiByUser " + ex.Message)
        Finally
            myAdapter.Dispose()
            dsMyDati.Dispose()
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Username"></param>
    ''' <param name="ElencoEnti"></param>
    ''' <returns></returns>
    Public Function GetEntiByUser(ByVal Username As String, ByVal ElencoEnti As String()) As DataSet
        'Dim WFErrore As String
        'Dim WFSessione As CreateSessione
        Dim SQL As String
        Dim strElencoEnti As String
        Dim i As Integer
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dsMyDati As New DataSet
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            For i = 0 To ElencoEnti.Length - 1
                strElencoEnti = strElencoEnti & ElencoEnti(i).ToString() & ","
            Next
            strElencoEnti = strElencoEnti.Substring(0, (strElencoEnti.Length - 1))

            SQL = "SELECT DISTINCT ENTI.*"
            SQL = SQL + " FROM ENTI INNER JOIN"
            SQL = SQL + " UTENTI_ENTI ON ENTI.COD_ENTE = UTENTI_ENTI.COD_ENTE INNER JOIN"
            SQL = SQL + " GESTIONE_UTENTI ON UTENTI_ENTI.ID_UTENTE = GESTIONE_UTENTI.ID_UTENTE"
            SQL = SQL + " WHERE ENTI.COD_ISTAT IN (@ElencoEnti)"
            Log.Debug("GetEntiByUser da ANATER::query::" & SQL)
            cmdMyCommand.Connection = New SqlClient.SqlConnection(COSTANTValue.ConstSession.StringConnectionOPENgov)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.CommandText = SQL
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.AddWithValue("@ElencoEnti", strElencoEnti.ToString())
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(dsMyDati)
            Return dsMyDati
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte + "." + COSTANTValue.ConstSession.UserName + " - OPENgov.SelectEnti.GetEntiByUser.errore: ", ex)
            Throw New Exception("Problemi nell'esecuzione di GetEntiByUser " + ex.Message)
        Finally
            'If Not IsNothing(WFSessione) Then
            '    WFSessione.Kill()
            'End If
            'WFSessione = Nothing
        End Try

    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="CodEnte"></param>
    ''' <param name="Belfiore"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Function LoginEnte(ByVal CodEnte As String, ByVal Belfiore As String) As Integer
        Dim dv As New DataView

        Try
            HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE") = "OPENGOVG"
            ClearVarEnte()
            dv = GetEnte(CodEnte, Belfiore)
            If dv.Count = 1 Then
                HttpContext.Current.Session("COD_ENTE") = dv.Item(0)("COD_ISTAT")
                HttpContext.Current.Session("CODENTE") = dv.Item(0)("COD_ISTAT")
                HttpContext.Current.Session("DESCRIZIONE_ENTE") = dv.Item(0)("DENOMINAZIONE")
                HttpContext.Current.Session("NOME_ENTE") = dv.Item(0)("DESCRIZIONE_ENTE")
                HttpContext.Current.Session("AMBIENTE") = dv.Item(0)("AMBIENTE")

                ' variabili utilizzate per i versamenti
                HttpContext.Current.Session("COMUNE_UBICAZIONE_IMMOBILE") = dv.Item(0)("DENOMINAZIONE")
                HttpContext.Current.Session("INTESTAZIONE_BOLLETTINO") = dv.Item(0)("INTESTAZIONE_BOLLETTINO")
                HttpContext.Current.Session("CONTO_CORRENTE") = dv.Item(0)("CONTO_CORRENTE")

                HttpContext.Current.Session("COD_BELFIORE") = dv.Item(0)("COD_BELFIORE")

                'variabili per il 290
                If Not IsDBNull(dv.Item(0)("idente_credben")) Then
                    HttpContext.Current.Session("idente_credben") = dv.Item(0)("idente_credben")
                Else
                    HttpContext.Current.Session("idente_credben") = ""
                End If
                If Not IsDBNull(dv.Item(0)("cod_ente_cnc")) Then
                    HttpContext.Current.Session("idente_CNC") = dv.Item(0)("cod_ente_cnc")
                Else
                    HttpContext.Current.Session("idente_CNC") = ""
                End If
                '*** 20130228 - gestione categoria Ateco per TARES ***
                If Not IsDBNull(dv.Item(0)("fk_IdTypeAteco")) Then
                    HttpContext.Current.Session("IdTypeAteco") = dv.Item(0)("fk_IdTypeAteco")
                Else
                    HttpContext.Current.Session("IdTypeAteco") = "2"
                End If
                '*** ***
                '*** 20140923 - GIS ***
                If Not IsDBNull(dv.Item(0)("HASGIS")) Then
                    HttpContext.Current.Session("VisualGIS") = CBool(dv.Item(0)("HASGIS"))
                Else
                    HttpContext.Current.Session("VisualGIS") = False
                End If
                Log.Debug("HttpContext.Current.SESSION(VisualGIS)::" & HttpContext.Current.Session("VisualGIS").ToString)
                '*** ***
                '**** 201809 - Cartelle Insoluti ***
                If Not IsDBNull(dv.Item(0)("hasruoloinsoluti")) Then
                    HttpContext.Current.Session("hasruoloinsoluti") = CBool(dv.Item(0)("hasruoloinsoluti"))
                Else
                    HttpContext.Current.Session("hasruoloinsoluti") = False
                End If
                '*** ***
                HttpContext.Current.Session("TributiBollettinoF24") = dv.Item(0)("TributiBollettinoF24")
                If Not IsDBNull(dv.Item(0).Item("IsolaEcologicaPathFile")) Then
                    HttpContext.Current.Session("IsolaEcologicaPathFile") = dv.Item(0)("IsolaEcologicaPathFile").ToString
                End If
                If Not IsDBNull(dv.Item(0).Item("IsolaEcologicaFTPUser")) Then
                    HttpContext.Current.Session("IsolaEcologicaFTPUser") = dv.Item(0)("IsolaEcologicaFTPUser").ToString
                End If
                If Not IsDBNull(dv.Item(0).Item("IsolaEcologicaFTPPwd")) Then
                    HttpContext.Current.Session("IsolaEcologicaFTPPwd") = dv.Item(0)("IsolaEcologicaFTPPwd").ToString
                End If
                If Not IsDBNull(dv.Item(0).Item("IsolaEcologicaFTP")) Then
                    HttpContext.Current.Session("IsolaEcologicaFTP") = dv.Item(0)("IsolaEcologicaFTP").ToString
                End If
                Log.Debug("SelectEnti per::Session('COD_ENTE')::" & HttpContext.Current.Session("COD_ENTE").ToString & "::Session('DESCRIZIONE_ENTE')::" & HttpContext.Current.Session("DESCRIZIONE_ENTE").ToString & "::Session('NOME_ENTE')::" & HttpContext.Current.Session("NOME_ENTE").ToString & "::Session('COD_BELFIORE')::" & HttpContext.Current.Session("COD_BELFIORE").ToString & "::Session('VisualGIS')::" & HttpContext.Current.Session("VisualGIS").ToString)

                ' Variabili stradario
                HttpContext.Current.Session("UrlPopUpStradario") = COSTANTValue.ConstSession.UrlStradario
                HttpContext.Current.Session("StileStradario") = COSTANTValue.ConstSession.StileStradario
                HttpContext.Current.Session("URLPOPCOMUNI") = COSTANTValue.ConstSession.UrlComuni

                'varibili dati attuali
                HttpContext.Current.Session("DATI_ATTUALI") = False

                'variabili gestione atti
                HttpContext.Current.Session.Remove("SELEZIONE_PROVVEDIMENTI_SEMPLICE")
            End If
            Return dv.Count
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte + "." + COSTANTValue.ConstSession.UserName + " - OPENgov.SelectEnti.LoginEnte.errore: ", ex)
            Log.Debug("LoginEnte::si è verificato il seguente errore::", ex)
            Return -1
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="COD_ENTE"></param>
    ''' <param name="Belfiore"></param>
    ''' <returns></returns>
    Public Function GetEnte(ByVal COD_ENTE As String, ByVal Belfiore As String) As DataView
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dtMyDati As New DataTable()
        Dim dvMyDati As New DataView
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(COSTANTValue.ConstSession.StringConnectionOPENgov)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            'cmdMyCommand.CommandType = CommandType.Text
            'cmdMyCommand.CommandText = "SELECT *"
            'cmdMyCommand.CommandText += " FROM ENTI"
            'cmdMyCommand.CommandText += " WHERE 1=1"
            'cmdMyCommand.CommandText += " AND (@IDENTE='' OR COD_ENTE=@IDENTE)"
            'cmdMyCommand.CommandText += " AND (@BELFIORE='' OR (@IDENTE='' AND COD_BELFIORE=@BELFIORE)) "
            'cmdMyCommand.CommandText += " ORDER BY DESCRIZIONE_ENTE"
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "ENTI_S"
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add("@AMBIENTE", SqlDbType.NVarChar).Value = COSTANTValue.ConstSession.Ambiente
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", COD_ENTE))
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@BELFIORE", Belfiore))
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(dtMyDati)
            dvMyDati = dtMyDati.DefaultView
            Return dvMyDati
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte + "." + COSTANTValue.ConstSession.UserName + " - OPENgov.SelectEnti.GetEnte.errore: ", ex)
            Throw New Exception("Problemi nell'esecuzione di GetEnte " + ex.Message)
        Finally
            myAdapter.Dispose()
            dtMyDati.Dispose()
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    Public Sub ClearVarEnte()
        Try
            HttpContext.Current.Session("COD_ENTE") = ""
            HttpContext.Current.Session("CODENTE") = ""
            HttpContext.Current.Session("DESCRIZIONE_ENTE") = ""
            HttpContext.Current.Session("NOME_ENTE") = ""
            'variabili utilizzate per i versamenti
            HttpContext.Current.Session("COMUNE_UBICAZIONE_IMMOBILE") = ""
            HttpContext.Current.Session("INTESTAZIONE_BOLLETTINO") = ""
            HttpContext.Current.Session("CONTO_CORRENTE") = ""
            HttpContext.Current.Session("COD_BELFIORE") = ""
            HttpContext.Current.Session("TributiBollettinoF24") = ""
            'variabili per il 290
            HttpContext.Current.Session("idente_credben") = ""
            HttpContext.Current.Session("idente_CNC") = ""
            HttpContext.Current.Session("IdTypeAteco") = "2"
            HttpContext.Current.Session("VisualGIS") = False
            'varibili dati attuali
            HttpContext.Current.Session("DATI_ATTUALI") = False
            'variabili gestione atti
            HttpContext.Current.Session.Remove("SELEZIONE_PROVVEDIMENTI_SEMPLICE")
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte + "." + COSTANTValue.ConstSession.UserName + " - OPENgov.SelectEnti.ClearVarEnte.errore: ", ex)
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myConnectionString"></param>
    ''' <param name="oEnte"></param>
    ''' <returns></returns>
    Public Function SetEnte(ByVal myConnectionString As String, ByVal oEnte As Ente) As Boolean
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Try
            'Valorizzo la connessione
            Using ctx As New DBModel(COSTANTValue.ConstSession.DBType, myConnectionString)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_ENTI_IU", "COD_ENTE", "COD_ISTAT", "DESCRIZIONE_ENTE", "AMBIENTE", "DENOMINAZIONE", "COD_BELFIORE", "COD_ENTE_CNC", "IDENTE_CREDBEN", "INDIRIZZO_CIVICO", "CAP", "LOCALITA", "PROVINCIA_SIGLA", "PROVINCIA_ESTESA", "TELEFONO", "FAX", "E_MAIL", "NUM_ABITANTI", "NUM_NUCLEI_FAM", "fk_IdTypeAteco", "POSIZIONEGEOGRAFICA", "HASGIS", "PERCCONTRIBUTOANCICNC", "CF_PIVA", "PIVA", "COGNOME", "NOME", "SESSO", "DATA_NASCITA", "COMUNE_NASCITASEDE", "PV_NASCITASEDE", "NOTE_ENTE", "COD_CATASTO", "COD_PROVINCIA", "ID_ENTE", "COD_TRIBUTO", "CONTO_CORRENTE", "INTESTAZIONE_BOLLETTINO", "COD_UBICAZCAT", "COMUNECAT", "HASRUOLOINSOLUTI", "TRIBUTIBOLLETTINOF24")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("COD_ENTE", oEnte.IdEnte) _
                    , ctx.GetParam("COD_ISTAT", oEnte.IdEnte) _
                    , ctx.GetParam("DESCRIZIONE_ENTE", oEnte.DescrizioneEnte) _
                    , ctx.GetParam("AMBIENTE", oEnte.Ambiente) _
                    , ctx.GetParam("DENOMINAZIONE", oEnte.Denominazione) _
                    , ctx.GetParam("COD_BELFIORE", oEnte.CodBelfiore) _
                    , ctx.GetParam("COD_ENTE_CNC", oEnte.CodEnteCNC) _
                    , ctx.GetParam("IDENTE_CREDBEN", oEnte.IdEnteCredBen) _
                    , ctx.GetParam("INDIRIZZO_CIVICO", oEnte.Indirizzo) _
                    , ctx.GetParam("CAP", oEnte.CAP) _
                    , ctx.GetParam("LOCALITA", oEnte.Localita) _
                    , ctx.GetParam("PROVINCIA_SIGLA", oEnte.PVSigla) _
                    , ctx.GetParam("PROVINCIA_ESTESA", oEnte.PVEstesa) _
                    , ctx.GetParam("TELEFONO", oEnte.Telefono) _
                    , ctx.GetParam("FAX", oEnte.Fax) _
                    , ctx.GetParam("E_MAIL", oEnte.EMail) _
                    , ctx.GetParam("NUM_ABITANTI", oEnte.NumAbitanti) _
                    , ctx.GetParam("NUM_NUCLEI_FAM", oEnte.NumNucleiFam) _
                    , ctx.GetParam("fk_IdTypeAteco", oEnte.IdTypeATECO) _
                    , ctx.GetParam("POSIZIONEGEOGRAFICA", oEnte.PosizioneGeografica) _
                    , ctx.GetParam("HASGIS", oEnte.hasGIS) _
                    , ctx.GetParam("PERCCONTRIBUTOANCICNC", oEnte.PercContributoANCICNC) _
                    , ctx.GetParam("CF_PIVA", oEnte.CFPIva) _
                    , ctx.GetParam("PIVA", oEnte.PIva) _
                    , ctx.GetParam("COGNOME", oEnte.Cognome) _
                    , ctx.GetParam("NOME", oEnte.Nome) _
                    , ctx.GetParam("SESSO", oEnte.Sesso) _
                    , ctx.GetParam("DATA_NASCITA", oEnte.DataNascita) _
                    , ctx.GetParam("COMUNE_NASCITASEDE", oEnte.ComuneNascita) _
                    , ctx.GetParam("PV_NASCITASEDE", oEnte.PVNascita) _
                    , ctx.GetParam("NOTE_ENTE", oEnte.Note) _
                    , ctx.GetParam("COD_CATASTO", oEnte.CodCatasto) _
                    , ctx.GetParam("COD_PROVINCIA", oEnte.CodProvincia) _
                    , ctx.GetParam("ID_ENTE", oEnte.IdEnteNumerico) _
                    , ctx.GetParam("COD_TRIBUTO", oEnte.CodTributo) _
                    , ctx.GetParam("CONTO_CORRENTE", oEnte.ContoCorrente) _
                    , ctx.GetParam("INTESTAZIONE_BOLLETTINO", oEnte.IntestazioneBollettino) _
                    , ctx.GetParam("COD_UBICAZCAT", oEnte.CodUbicazCat) _
                    , ctx.GetParam("COMUNECAT", oEnte.ComuneCat) _
                    , ctx.GetParam("HASRUOLOINSOLUTI", oEnte.HasRuoloInsoluti) _
                    , ctx.GetParam("TRIBUTIBOLLETTINOF24", oEnte.TributiBollettinoF24)
                )
                If dvMyDati.Table.Rows.Count() = 1 Then
                    If StringOperation.FormatInt(dvMyDati(0)("id")) <= 0 Then
                        Return False
                    End If
                End If
                ctx.Dispose()
            End Using
            Return True
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte + "." + COSTANTValue.ConstSession.UserName + " - OPENgov.SelectEnti.SetEnte.errore: ", ex)
            Return False
        End Try
    End Function
End Class
''' <summary>
''' Definizione oggetto Ente
''' </summary>
Public Class Ente
    Public IdEnte As String = ""
    Public DescrizioneEnte As String = ""
    Public Ambiente As String = ""
    Public Denominazione As String = ""
    Public CodBelfiore As String = ""
    Public CodEnteCNC As String = ""
    Public IdEnteCredBen As String = ""
    Public Indirizzo As String = ""
    Public CAP As String = ""
    Public Localita As String = ""
    Public PVSigla As String = ""
    Public PVEstesa As String = ""
    Public Telefono As String = ""
    Public Fax As String = ""
    Public EMail As String = ""
    Public NumAbitanti As Integer = 0
    Public NumNucleiFam As Integer = 0
    Public IdTypeATECO As Integer = 0
    Public PosizioneGeografica As String = ""
    Public hasGIS As Boolean = False
    Public PercContributoANCICNC As Double = 0
    Public CFPIva As String = ""
    Public PIva As String = ""
    Public Cognome As String = ""
    Public Nome As String = ""
    Public Sesso As String = ""
    Public DataNascita As String = ""
    Public ComuneNascita As String = ""
    Public PVNascita As String = ""
    Public Note As String = ""

    Public CodCatasto As String = ""
    Public CodProvincia As String = ""
    Public IdEnteNumerico As Integer = 0
    Public CodTributo As String = ""
    Public ContoCorrente As String = ""
    Public IntestazioneBollettino As String = ""
    Public CodUbicazCat As String = ""
    Public ComuneCat As String = ""
    '**** 201809 - Cartelle Insoluti ***
    Public HasRuoloInsoluti As Integer = 0
    Public TributiBollettinoF24 As String = ""
End Class
''' <summary>
''' Definizione oggetto dettagli rilascio
''' </summary>
<Serializable(), System.Xml.Serialization.XmlRoot("ReleaseHistory"), System.Xml.Serialization.XmlType("ReleaseHistory")>
Public Class ReleaseHistory
    Public ItemID As String = ""
    Public DateRel As String = ""
    Public Title As String = ""
    Public Description As String = ""
    Public DocumentRef As String = ""
End Class