Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports Anagrafica
Imports System.Web.SessionState.HttpSessionState
Imports AnagInterface
Imports log4net
''' <summary>
''' Pagina per la ricerca anagrafica.
''' Contiene i parametri di ricerca e le funzioni della comandiera.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class RicercaAnagrafica
    Inherits ANAGRAFICAWEB.BasePage
    Public UrlPopComuni As String = ""
    Public UrlStradario As String = ANAGRAFICAWEB.ConstSession.UrlStradario
    Public oAnagrafica As DLL.GestioneAnagrafica
    Public oDettaglioAnagrafica As DettaglioAnagrafica
    Public sessionName As Object
    Private Log As ILog = LogManager.GetLogger(GetType(RicercaAnagrafica))
    Private Const SEARCH_PARAMETRES As String = "SEARCHPARAMETRES"
    Private Const FIRST_TIME As String = "1"

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
        hdEnteAppartenenza.Value = ANAGRAFICAWEB.ConstSession.IdEnte
    End Sub

#End Region
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="30/08/2007">
    ''' Fabiana
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="23/07/2014">
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="11/2015">
    ''' Funzioni Sovracomunali
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="10/11/2008">
    ''' lo stradario non deve essere obbligatorio
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim sScript As String = ""
            If Not ConfigurationManager.AppSettings("UrlPopUpComuni") Is Nothing Then UrlPopComuni = ConfigurationManager.AppSettings("UrlPopUpComuni")
            If Not Page.IsPostBack Then
                sessionName = Request.Item("sessionName")
                ViewState("sessionName") = sessionName
                Session("TributiAssociati") = Nothing
                LoadCombo()
                If ANAGRAFICAWEB.ConstSession.IdEnte <> "" Then
                    ddlEnti.SelectedValue = ANAGRAFICAWEB.ConstSession.IdEnte
                    sScript = "document.getElementById ('lblEnti').style.display='none';"
                    sScript += "document.getElementById ('ddlEnti').style.display='none';"
                    RegisterScript(sScript, Me.GetType())
                End If
                Dim htSearchParametres As Hashtable = New Hashtable
                htSearchParametres = CType(Session(SEARCH_PARAMETRES), Hashtable)

                If Not htSearchParametres Is Nothing Then
                    ddlEnti.SelectedValue = htSearchParametres("CodEnte")
                    txtCognome.Text = htSearchParametres("Cognome")
                    txtNome.Text = htSearchParametres("Nome")
                    txtCodiceFiscale.Text = htSearchParametres("CodiceFiscale")
                    txtPartitaIva.Text = htSearchParametres("PartitaIva")
                    txtCodContribuente.Text = htSearchParametres("CodContribuente")
                    txtComuneResidenza.Text = htSearchParametres("strComuneResidenza")
                    txtProvinciaResidenza.Text = htSearchParametres("strProvinciaResidenza")
                    txtDataNascita.Text = htSearchParametres("strDataNascita")
                    txtDataMorte.Text = htSearchParametres("strDataMorte")
                    chkDaRicontrollare.Checked = htSearchParametres("DaRicontrollare")
                    chkNonAgganciate.Checked = htSearchParametres("NONAGGANCIATE")
                    ddlContatti.SelectedValue = htSearchParametres("ddlContatti")
                    ddlTributoInvio.SelectedValue = htSearchParametres("nSearchInvio")
                    ddlTributoPresente.SelectedValue = htSearchParametres("nSearchModulo")
                    If chkDaRicontrollare.Checked Or chkNonAgganciate.Checked Or txtComuneResidenza.Text <> "" Or txtProvinciaResidenza.Text <> "" Or ddlContatti.SelectedValue > -1 Or ddlTributoPresente.SelectedValue > -1 Or ddlTributoInvio.SelectedValue > -1 Or txtDataNascita.Text <> "" Or txtDataMorte.Text <> "" Then
                        sScript = "VisualizzaParAvanzati();"
                        RegisterScript(sScript, Me.GetType())
                    End If
                End If

                oDettaglioAnagrafica = New DettaglioAnagrafica
                If Request.Item("popup") = "1" Then
                    oDettaglioAnagrafica = Session(ViewState("sessionName"))
                    If oDettaglioAnagrafica Is Nothing Then
                        Log.Debug("Anagrafica::RicercaAnagrafica::Load::oDettaglioAnagrafica è nullo")
                    End If
                End If

                sScript = ""
                If Request.Item("popup") = "1" Then
                    If Not oDettaglioAnagrafica Is Nothing Then
                        If Not oDettaglioAnagrafica.COD_CONTRIBUENTE = -1 Then
                            Modifica()
                        Else
                            sScript += "tabEsterna.style.display = '';"
                            sScript += "parent.Comandi.location.href='./Comandi/ComandiRicercaAnagraficaGenerale.aspx';"
                            sScript += "parent.Visualizza.document.getElementById('txtCognome').focus();"
                        End If
                    Else
                        sScript += "tabEsterna.style.display = '';"
                        sScript += "parent.Comandi.location.href='./Comandi/ComandiRicercaAnagraficaGenerale.aspx';"
                        sScript += "parent.Visualizza.document.getElementById('txtCognome').focus();"
                    End If
                Else
                    Session("modificaDiretta") = "False"
                    sScript += "tabEsterna.style.display = '';"
                    sScript += "document.getElementById('txtCognome').focus();"
                    sScript += "parent.Comandi.location.href='./Comandi/ComandiRicercaAnagrafica.aspx';"
                End If
                RegisterScript(sScript, Me.GetType())

                If Not htSearchParametres Is Nothing Then
                    Dim myPage As String = ""
                    Dim Parametri As String = "?cognome=" + txtCognome.Text + "&nome=" + txtNome.Text
                    Parametri += "&codicefiscale=" + txtCodiceFiscale.Text + "&partitaiva=" + txtPartitaIva.Text
                    Parametri += "&codcontribuente=" + txtCodContribuente.Text + "&DARICONTROLLARE=" + chkDaRicontrollare.Checked.ToString
                    Parametri += "&Via=" + txtViaResidenza.Text + "&CodVia=" + txtCodViaResidenza.Text
                    Parametri += "&NONAGGANCIATE=" + chkNonAgganciate.Checked.ToString
                    Parametri += "&comuneresidenza=" + txtComuneResidenza.Text
                    Parametri += "&provinciaresidenza=" + txtProvinciaResidenza.Text
                    Parametri += "&datanascita=" + txtDataNascita.Text
                    Parametri += "&datamorte=" + txtDataMorte.Text
                    Parametri += "&ddlContatti=" + ddlContatti.SelectedValue
                    Parametri += "&ddlTributoInvio=" + ddlTributoInvio.SelectedValue
                    Parametri += "&ddlTributoPresente=" + ddlTributoPresente.SelectedValue
                    Parametri += "&IdEnte=" + ddlEnti.SelectedValue
                    If (Request.Item("popup") = "1") Then
                        Parametri += "&sessionName=" + sessionName
                        myPage = "SearchResultsAnagraficaGenerale.aspx"
                    Else
                        myPage = "SearchResultsAnagrafica.aspx"
                    End If
                    sScript = "$('#DivAttesa').show();"
                    sScript += "document.getElementById('loadGrid').src='" + myPage + Parametri + "';"
                    RegisterScript(sScript, Me.GetType())
                End If
                txtCognome.Attributes.Add("onkeydown", "keyPress(" & Request.Item("popup") & " );")
                txtNome.Attributes.Add("onkeydown", "keyPress(" & Request.Item("popup") & " );")
                txtCodiceFiscale.Attributes.Add("onkeydown", "keyPress(" & Request.Item("popup") & " );")
                txtPartitaIva.Attributes.Add("onkeydown", "keyPress(" & Request.Item("popup") & " );")
                txtCodContribuente.Attributes.Add("onkeydown", "keyPress(" & Request.Item("popup") & " );")
                txtViaResidenza.Attributes.Add("onkeydown", "document.getElementById('txtCodViaResidenza').value = '';")

                ' se il comune è abilitato alla ricerca delle strade, abilito il collegamento.
                lnkOpenStradario.Attributes.Add("onclick", "return ApriStradario('RibaltaStrada', '" & ANAGRAFICAWEB.ConstSession.IdEnte & "')")
                If RicercaStradario() Then
                    lnkOpenStradario.Visible = True
                Else
                    lnkOpenStradario.Visible = False
                End If
                Dim fncActionEvent As New Utility.DBUtility(ANAGRAFICAWEB.ConstSession.DBType, ANAGRAFICAWEB.ConstSession.StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, ANAGRAFICAWEB.ConstSession.UserName, New Utility.Costanti.LogEventArgument().Anagrafica, "Ricerca", Utility.Costanti.AZIONE_LETTURA.ToString, "", ANAGRAFICAWEB.ConstSession.IdEnte, -1)
            End If
        Catch ex As Exception
            Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.RicercaAnagrafica.Page_Load.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnNuovo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNuovo.Click
        RegisterScript("parent.Visualizza.location.href='FormAnagrafica.aspx?sessionName=" & ViewState("sessionName") & "&popup=" & Request.Item("popup") & "&COD_CONTRIBUENTE=" & Utility.Costanti.INIT_VALUE_NUMBER & "&ID_DATA_ANAGRAFICA=" & Utility.Costanti.INIT_VALUE_NUMBER & "&STORICO=" & Utility.Costanti.INIT_VALUE_NUMBER & "'", Me.GetType())
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Esportazione completa dati</em>
    ''' </revision>
    ''' </revisionHistory>
    Private Sub btnStampaExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStampaExcel.Click
        Dim sNameXLS As String
        Dim aListColonne As ArrayList
        Dim aMyHeaders As String()
        Dim DtDatiStampa As New DataTable
        Dim x As Integer
        '*** 20140723 ***'*** 201511 - Funzioni Sovracomunali ***
        'Dim nCol As Integer = 14
        Dim nCol As Integer = 23
        '*** ***
        DtDatiStampa = New ANAGRAFICAWEB.clsFncAnag().Stampa(ANAGRAFICAWEB.ConstSession.ApplicationsEnabled, ANAGRAFICAWEB.ConstSession.Ambiente, ddlEnti.SelectedValue, txtCognome.Text, txtNome.Text, txtCodiceFiscale.Text, txtPartitaIva.Text, txtCodContribuente.Text, txtComuneResidenza.Text, txtProvinciaResidenza.Text, txtDataNascita.Text, txtDataMorte.Text, txtViaResidenza.Text, txtCodViaResidenza.Text, ddlTributoInvio.SelectedValue, ddlTributoPresente.SelectedValue, ViewState("SortKey") & " " & ViewState("OrderBy"), ddlContatti.SelectedValue, nCol)
        If Not IsNothing(DtDatiStampa) Then
            RegisterScript("DivAttesa.style.display='none';", Me.GetType)
            'valorizzo il nome del file
            sNameXLS = ANAGRAFICAWEB.ConstSession.IdEnte & "_ANAGRAFICHE_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
            'definisco le colonne
            aListColonne = New ArrayList
            For x = 0 To nCol
                aListColonne.Add("")
            Next
            aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

            Dim MyCol() As Integer = New Integer(nCol) {}
            For x = 0 To nCol
                MyCol(x) = x
            Next
            Dim objExport As New RKLib.ExportData.Export("Web")
            objExport.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
        Else
            Dim sScript As String = "GestAlert('a', 'danger', '', '', 'Errore in stampa');"
            RegisterScript(sScript, Me.GetType())
        End If
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub Modifica()

        Session("modificaDiretta") = "True"

        Dim COD_CONTRIB As String
        Dim ID_DATA_ANAG As String
        Try
            oDettaglioAnagrafica = New DettaglioAnagrafica
            oDettaglioAnagrafica = Session(ViewState("sessionName"))

            COD_CONTRIB = oDettaglioAnagrafica.COD_CONTRIBUENTE
            ID_DATA_ANAG = oDettaglioAnagrafica.ID_DATA_ANAGRAFICA

            RegisterScript("parent.Visualizza.location.href='FormAnagrafica.aspx?sessionName=" & ViewState("sessionName") & "&popup=1&COD_CONTRIBUENTE=" & COD_CONTRIB & "&ID_DATA_ANAGRAFICA=" & ID_DATA_ANAG & "';", Me.GetType())
        Catch ex As Exception
            Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.RicercaAnagrafica.Modifica.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Private Function RicercaStradario() As Boolean
        '***10/11/2008 - lo stradario non deve essere obbligatorio***
        Dim bret As Boolean = False
        If UrlStradario <> "" Then
            Try
                Dim objEnte As New OggettiComuniStrade.OggettoEnte
                Dim ArrObjEnte As OggettiComuniStrade.OggettoEnte()

                objEnte.Cap = ""
                objEnte.CodBelfiore = ""
                objEnte.CodCNC = ""
                '*** 201511 - Funzioni Sovracomunali ***
                objEnte.CodIstat = ddlEnti.SelectedValue 'Session("CodEnte")
                '*** ***
                objEnte.Denominazione = ""
                objEnte.Provincia = ""
                objEnte.Stradario = False

                ArrObjEnte = New WsStradario.Stradario().GetEnti(objEnte)
                If ArrObjEnte.Length = 1 Then
                    If ArrObjEnte(0).Stradario = True Then
                        bret = True
                    End If
                End If
                Return bret
            Catch ex As Exception
                Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.RicercaAnagrafica.RicercaStradario.errore: ", ex)
                Return False
            End Try
        End If
        '**************************************************
    End Function

    '*** 20140723 ***
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub LoadCombo()
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myDataReader As SqlClient.SqlDataReader
        Try

            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandTimeout = 0
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ANAGRAFICAWEB.ConstSession.StringConnectionAnagrafica)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            cmdMyCommand.CommandText = "prc_GetParamContatti"
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myDataReader = cmdMyCommand.ExecuteReader
            Try
                ddlContatti.Items.Clear()
                ddlContatti.Items.Add("...")
                ddlContatti.Items(0).Value = "-1"
                If Not myDataReader Is Nothing Then
                    Do While myDataReader.Read
                        If Not IsDBNull(myDataReader(0)) Then
                            ddlContatti.Items.Add(myDataReader(0))
                            ddlContatti.Items(ddlContatti.Items.Count - 1).Value = myDataReader(1)
                        End If
                    Loop
                End If
            Catch ex As Exception
                Log.Debug("loadCombo CONTATTI::si è verificato il seguente errore::" + ex.Message)
            Finally
                myDataReader.Close()
            End Try
        Catch ex As Exception
            Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.RicercaAnagrafica.LoadCombo.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
            Throw New Exception("Problemi nell'esecuzione di LoadComboGenerale " + ex.Message)
        Finally
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ANAGRAFICAWEB.ConstSession.StringConnectionOPENgov)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_GetTributi"
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPORICERCA", "INVIO"))
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myDataReader = cmdMyCommand.ExecuteReader
            Try
                ddlTributoInvio.Items.Clear()
                ddlTributoInvio.Items.Add("...")
                ddlTributoInvio.Items(0).Value = "-1"
                If Not myDataReader Is Nothing Then
                    Log.Debug("loadCombo --> myDataReader::valorizzato")
                    Do While myDataReader.Read
                        If Not IsDBNull(myDataReader(0)) Then
                            ddlTributoInvio.Items.Add(myDataReader(0))
                            ddlTributoInvio.Items(ddlTributoInvio.Items.Count - 1).Value = myDataReader(1)
                        End If
                    Loop
                End If
            Catch ex As Exception
                Log.Debug("loadCombo TRIBUTI INVIO::si è verificato il seguente errore::" + ex.Message)
            Finally
                myDataReader.Close()
            End Try

            cmdMyCommand.CommandText = "prc_GetTributi"
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPORICERCA", "MODULO"))
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myDataReader = cmdMyCommand.ExecuteReader
            Try
                ddlTributoPresente.Items.Clear()
                ddlTributoPresente.Items.Add("...")
                ddlTributoPresente.Items(0).Value = "-1"
                If Not myDataReader Is Nothing Then
                    Do While myDataReader.Read
                        If Not IsDBNull(myDataReader(0)) Then
                            ddlTributoPresente.Items.Add(myDataReader(0))
                            ddlTributoPresente.Items(ddlTributoPresente.Items.Count - 1).Value = myDataReader(1)
                        End If
                    Loop
                End If
            Catch ex As Exception
                Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.RicercaAnagrafica.LoadCombo.errore: ", ex)
                Response.Redirect("../PaginaErrore.aspx")
            Finally
                myDataReader.Close()
            End Try
        Catch ex As Exception
            Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.RicercaAnagrafica.LoadCombo.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
            Throw New Exception("Problemi nell'esecuzione di LoadComboGenerale " + ex.Message)
        Finally
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
        '*** 201511 - Funzioni Sovracomunali ***
        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ANAGRAFICAWEB.ConstSession.StringConnectionOPENgov)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "ENTI_S"
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add("@AMBIENTE", SqlDbType.NVarChar).Value = ANAGRAFICAWEB.ConstSession.Ambiente
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myDataReader = cmdMyCommand.ExecuteReader
            Try
                ddlEnti.Items.Clear()
                ddlEnti.Items.Add("...")
                ddlEnti.Items(0).Value = ""
                If Not myDataReader Is Nothing Then
                    Do While myDataReader.Read
                        If Not IsDBNull(myDataReader(0)) Then
                            ddlEnti.Items.Add(myDataReader(1))
                            ddlEnti.Items(ddlEnti.Items.Count - 1).Value = myDataReader(0)
                        End If
                    Loop
                End If
            Catch ex As Exception
                Log.Debug("loadCombos ENTI::si è verificato il seguente errore::" + ex.Message)
            Finally
                myDataReader.Close()
            End Try
        Catch ex As Exception
            Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.RicercaAnagrafica.LoadCombo.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
        Finally
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
        '*** ***
    End Sub
    '*** ***
End Class


