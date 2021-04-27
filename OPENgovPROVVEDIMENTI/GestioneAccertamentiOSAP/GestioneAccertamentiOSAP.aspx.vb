Imports AnagInterface
Imports Anagrafica.DLL
Imports ComPlusInterface
Imports log4net
''' <summary>
''' Pagina per la generazione dei provvedimenti OSAP.
''' Contiene i parametri di gestione, le funzioni della comandiera e la griglia per la visualizzazione del dichiarato.  
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class GestioneAccertamentiOSAP
	Inherits BasePage
	Private Shared Log As ILog = LogManager.GetLogger(GetType(GestioneAccertamentiOSAP))
	Private strConnectionStringOPENgovProvvedimenti As String
    Private FncMyUtility As New MyUtility
   
    Private WFErrore As String

    Private oAnagrafica As New GestioneAnagrafica
    Private oDettaglioAnagrafica As DettaglioAnagrafica
    Public anno As Integer

    Protected COD_CONTRIB_RETTIFICA, ID_PROVVEDIMENTO_RETTIFICA As Long
    Protected TIPO_OPERAZIONE_RETTIFICA As Boolean
    Protected NOMINATIVO_RETTIFICA, COD_TRIBUTO_RETTIFICA, ANNO_RETTIFICA, DATA_ELABORAZIONE_RETTIFICA, DATA_RETTIFICA As String

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Session("COD_TRIBUTO") = Utility.Costanti.TRIBUTO_OSAP
        Session("oAnagrafe") = Nothing
        Dim objGestOPENgovProvvedimenti As DBPROVVEDIMENTI.ProvvedimentiDB
        Dim sScript As String = ""

        Try
            btnFocus.Attributes.Add("onclick", "return false;")

            'Utility = New MyUtility

            'IMPOSTO MANUALMENTE IL CODICE TRIBUTO PERCHè SONO NELLE VIDEATE DELLA OSAP
            Session("COD_TRIBUTO") = Utility.Costanti.TRIBUTO_OSAP

            '---------------------------GESTIONE ACCERTAMENTI DA RETTIFICA-------------------------------------------
            Dim myHasTbl As New Hashtable
            Try
                If Not Session("HashTableRettificaAccertamenti") Is Nothing Then
                    myHasTbl = Session("HashTableRettificaAccertamenti")
                End If

                COD_CONTRIB_RETTIFICA = myHasTbl("CODCONTRIBUENTE")
                ID_PROVVEDIMENTO_RETTIFICA = myHasTbl("ID_PROVVEDIMENTO_RETTIFICA")
                COD_TRIBUTO_RETTIFICA = myHasTbl("COD_TRIBUTO")

                If myHasTbl("ID_PROVVEDIMENTO_RETTIFICA") = 0 Then
                    TIPO_OPERAZIONE_RETTIFICA = False
                Else
                    TIPO_OPERAZIONE_RETTIFICA = True
                End If
                Session("ID_PROVVEDIMENTO_RETTIFICA") = ID_PROVVEDIMENTO_RETTIFICA

                ANNO_RETTIFICA = myHasTbl("ANNOACCERTAMENTO")
                DATA_ELABORAZIONE_RETTIFICA = myHasTbl("DATA_ELABORAZIONE_PER_RETTIFICA")
            Catch err As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAccertamenti.Page_Load.errore: ", err)
                Response.Redirect("../../PaginaErrore.aspx")
            End Try
            '--------------------------------------------------------------------------------------------------------

            Session("modificaDiretta") = "False"
            Session("HashTableData") = ""
            Session("CalcolaAddizionali") = Nothing
            'If ChkAddizionali.Checked = True Then
            '    Session("CalcolaAddizionali") = True
            'Else
                Session("CalcolaAddizionali") = False
            'End If

            If Page.IsPostBack = False Then
                Session.Remove("oAccertatiGriglia")
                Session.Remove("DataSetDichiarazioni")

                ViewState("sessionName") = ""

                hdIdContribuente.Value = "-1"
                txtHiddenIdDataAnagrafica.Text = "-1"

                'Creo oggetto ProvvedementiDB per l'esecuzione delle query
                objGestOPENgovProvvedimenti = New DBPROVVEDIMENTI.ProvvedimentiDB

                'Il datareader viene chiuso all'interno della funzione FillDropDownSQLString

                FncMyUtility.FillDropDownSQLString(ddlAnno, objGestOPENgovProvvedimenti.GetAnniProvvedimenti(ConstSession.Ambiente, ConstSession.IdEnte, Session("COD_TRIBUTO")), -1, "...")

                sScript += "parent.Comandi.location.href='CGestioneAccertamentiOSAP.aspx';"
                RegisterScript(sScript, Me.GetType())
                If Not Request.Item("codcontribuente") Is Nothing Then
                    oDettaglioAnagrafica = New DettaglioAnagrafica
                    oAnagrafica = New GestioneAnagrafica
                    oDettaglioAnagrafica = oAnagrafica.GetAnagrafica(Request.Item("codcontribuente"), -1, String.Empty, ConstSession.DBType, ConstSession.StringConnectionAnagrafica, False) 'oAnagrafica.GetAnagrafica(Request.Item("codcontribuente"), ConstSession.CodTributo, -1, ConstSession.StringConnectionAnagrafica)
                    hdIdContribuente.Value = oDettaglioAnagrafica.COD_CONTRIBUENTE
                    txtHiddenIdDataAnagrafica.Text = oDettaglioAnagrafica.ID_DATA_ANAGRAFICA
                    '*** 201504 - Nuova Gestione anagrafica con form unico ***
                    If ConstSession.HasPlainAnag Then
                        ifrmAnag.Attributes.Add("src", "../../Generali/asp/VisualAnag.aspx?IdContribuente=" + hdIdContribuente.Value + "&Azione=" + Utility.Costanti.AZIONE_NEW.ToString())
                    Else
                        txtNominativo.Text = oDettaglioAnagrafica.Cognome & " " & oDettaglioAnagrafica.Nome
                    End If
                    ViewState.Add("COGNOME", oDettaglioAnagrafica.Cognome)
                    ViewState.Add("NOME", oDettaglioAnagrafica.Nome)

                    Session("codContribuente") = oDettaglioAnagrafica
                    ViewState("sessionName") = ""
                Else
                    '*** 201504 - Nuova Gestione anagrafica con form unico ***
                    If ConstSession.HasPlainAnag Then
                        ifrmAnag.Attributes.Add("src", "../../Generali/asp/VisualAnag.aspx?IdContribuente=" + hdIdContribuente.Value + "&Azione=" + Utility.Costanti.AZIONE_NEW.ToString())
                    End If
                End If
            Else
                '*** 201504 - Nuova Gestione anagrafica con form unico ***
                If ConstSession.HasPlainAnag Then
                    ifrmAnag.Attributes.Add("src", "../../Generali/asp/VisualAnag.aspx?IdContribuente=" + hdIdContribuente.Value + "&Azione=" + Utility.Costanti.AZIONE_NEW.ToString())
                End If
            End If
            If TIPO_OPERAZIONE_RETTIFICA = True Then
                GestisciRettifica()
            End If
            '*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI & DIVERSA GESTIONE QUOTA VARIABILE***
            If ConstSession.HasPlainAnag Then
                sScript += "document.getElementById('TRSpecAnag').style.display='none';"
            Else
                sScript += "document.getElementById('TRPlainAnag').style.display='none';"
            End If
            RegisterScript(sScript, Me.GetType())
            '*** ***
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAccertamentiOSAP.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ddlAnno_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlAnno.SelectedIndexChanged
        Try
            If ddlAnno.SelectedIndex <> 0 And hdIdContribuente.Value <> "" Then 'And txtNominativo.Text <> "" Then
                If CInt(hdIdContribuente.Value) > 0 Then
                    btnSearchDichiarazioni_Click(Nothing, Nothing)
                    txtCerca.Text = "1"
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAccertamentiOSAP.ddlAnno_SelectedIndexChanged.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnSearchDichiarazioni_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchDichiarazioni.Click
        Dim objHashTable As Hashtable = New Hashtable

        Try
            Session("ListArticoliDic") = Nothing
            strConnectionStringOPENgovProvvedimenti = ConfigurationManager.AppSettings("connectionStringOPENgovPROVVEDIMENTI")         'objSessione.oSession.oAppDB.GetConnection.ConnectionString
            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
            objHashTable.Add("ID_PROVVEDIMENTO_RETTIFICA", Session("ID_PROVVEDIMENTO_RETTIFICA"))

            Dim objCOMDichiarazioniAccertamenti As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
            Dim iRetValControllo As Integer

            'effettuato accertamento con data di conferma presente (definitivo) RETURN 0
            'msgbox impossibile proseguire, già presente un accertamento definitivo
            'effettuato un pre-accertamento con data di conferma presente (definitivo) RETURN 1
            'eseguo accertamento (senza ri-eseguire la fase 2 del preaccertamento)
            'effettuato un accertamento con data di conferma non presente (atto potenziale) RETURN 2
            'non effettuato nè ACCERTAMENTO nè PRE-ACCERTAMENTO RETURN 3
            ''sscript+=""
            iRetValControllo = objCOMDichiarazioniAccertamenti.GetControlliAccertamento(ConstSession.StringConnection, ddlAnno.SelectedValue, ConstSession.IdEnte, hdIdContribuente.Value, Session("COD_TRIBUTO"), objHashTable)

            'Session.Remove("VALORE_RITORNO_ACCERTAMENTO")
            'Session.Add("VALORE_RITORNO_ACCERTAMENTO", iRetValControllo)
            Session.Add("ESCLUDI_PREACCERTAMENTO", False)

            If TIPO_OPERAZIONE_RETTIFICA = False Then
                If iRetValControllo = 0 Then
                    'effettuato accertamento definitivo, impossibile proseguire
                    Session("ESCLUDI_PREACCERTAMENTO") = True
                    RegisterScript("FoundAccDefinitivo();", Me.GetType())
                    Exit Sub
                End If

                If iRetValControllo = 4 Then
                    'effettuato accertamento NON definitivo, Vuoi proseguire?
                    RegisterScript("FoundAccNONDefinitivo();", Me.GetType())
                    Exit Sub
                End If
            Else
                'Session.Remove("VALORE_RITORNO_ACCERTAMENTO")
                Session.Remove("DATA_ELABORAZIONE_PER_RETTIFICA")
                iRetValControllo = 6
                'Session.Add("VALORE_RITORNO_ACCERTAMENTO", iRetValControllo)
                Session.Add("DATA_ELABORAZIONE_PER_RETTIFICA", objHashTable("DATA_ELABORAZIONE_PER_RETTIFICA"))
            End If

            btnSearchDichiarazioniEffettivo_Click(Nothing, Nothing)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAccertamentiOSAP.btnSearchDichiarazioni_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnSearchDichiarazioniEffettivo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearchDichiarazioniEffettivo.Click
        Try
            dim sScript as string=""
            Dim objHashTable As Hashtable = New Hashtable

            If TIPO_OPERAZIONE_RETTIFICA = True Then
                'carico nell'oggetto hashtable i dati per gestire la rettifica

                Dim objUtility As New MyUtility

                If objHashTable.ContainsKey("DATA_ELABORAZIONE_PER_RETTIFICA") Then
                    objHashTable.Remove("DATA_ELABORAZIONE_PER_RETTIFICA")
                    objHashTable.Add("DATA_ELABORAZIONE_PER_RETTIFICA", objUtility.GiraDataFromDB(Session("DATA_ELABORAZIONE_PER_RETTIFICA")))
                Else
                    objHashTable.Add("DATA_ELABORAZIONE_PER_RETTIFICA", objUtility.GiraDataFromDB(Session("DATA_ELABORAZIONE_PER_RETTIFICA")))
                End If
                objHashTable.Add("DATA_ELABORAZIONE", objUtility.GiraData(DATA_ELABORAZIONE_RETTIFICA))
                objHashTable.Add("COD_TRIBUTO", COD_TRIBUTO_RETTIFICA)
                objHashTable.Add("ID_PROVVEDIMENTO_RETTIFICA", ID_PROVVEDIMENTO_RETTIFICA)
                DATA_RETTIFICA = DateTime.Now.ToString("yyyyMMdd")
                objHashTable.Add("DATA_RETTIFICA", DATA_RETTIFICA)
                objHashTable.Add("TIPO_OPERAZIONE_RETTIFICA", TIPO_OPERAZIONE_RETTIFICA)

                objHashTable.Add("CODCONTRIBUENTE", CStr(COD_CONTRIB_RETTIFICA))
                objHashTable.Add("ANNOACCERTAMENTO", ANNO_RETTIFICA)

            Else
                Session("ID_PROVVEDIMENTO_RETTIFICA") = ""
                objHashTable.Add("CODCONTRIBUENTE", hdIdContribuente.Value)
                objHashTable.Add("ANNOACCERTAMENTO", ddlAnno.SelectedValue)
            End If

            anno = ddlAnno.SelectedValue
            Session.Remove("oAccertatiGriglia")
            Session.Remove("oAccertato")
            Session("HashTableRettificaAccertamenti") = objHashTable
            Session("HashTableDichiarazioniAccertamenti") = objHashTable

            sScript +="document.getElementById('attesaCarica').style.display='';"
            sscript+= "document.getElementById('loadGridDichiarato').src='SearchDatiDichiaratoOSAP.aspx';" 'frames.item('loadGridDichiarato').location.href 
            sScript += "document.getElementById('loadGridAccertato').src='SearchDatiAccertatoOSAP.aspx';"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAccertamentiOSAP.btnSearchDichiarazioniEffettivo_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Imagebutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Imagebutton.Click
        txtNominativo.Text = ""
        Try
            ' Dim WFSessione As New OPENUtility.CreateSessione(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))
            ' If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
            ' Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
            ' End If

            Session.Remove(ViewState("sessionName"))

            'oAnagrafica = New GestioneAnagrafica(WFSessione.oSession, Session("Anagrafica"))
            oDettaglioAnagrafica = New DettaglioAnagrafica
            oDettaglioAnagrafica.COD_CONTRIBUENTE = hdIdContribuente.Value
            oDettaglioAnagrafica.ID_DATA_ANAGRAFICA = txtHiddenIdDataAnagrafica.Text
            ViewState("sessionName") = "codContribuente"
            Session(ViewState("sessionName")) = oDettaglioAnagrafica
            writeJavascriptAnagrafica(ViewState("sessionName"))
            'If Not IsNothing(WFSessione) Then
            'WFSessione.Kill()
            'WFSessione = Nothing
            'End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAccertamentiOSAP.Imagebutton_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnRibalta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRibalta.Click
        'Azzero la varibile di sessione dell'accertato se ribalto i dati
        'di un nuovo contribuente da accertare
        Try
            Session.Remove("DataTableImmobiliDaAccertare")       'rimuovo dichiarato
            Session.Remove("oAccertato")          'rimuovo accertato
            Session.Remove("oAccertatiGriglia")       'rimuovo accertato

            oDettaglioAnagrafica = Session(ViewState("sessionName"))

            txtNominativo.Text = oDettaglioAnagrafica.Cognome & " " & oDettaglioAnagrafica.Nome
            ViewState.Add("COGNOME", oDettaglioAnagrafica.Cognome)
            ViewState.Add("NOME", oDettaglioAnagrafica.Nome)

            hdIdContribuente.Value = oDettaglioAnagrafica.COD_CONTRIBUENTE
            txtHiddenIdDataAnagrafica.Text = oDettaglioAnagrafica.ID_DATA_ANAGRAFICA
            ViewState("sessionName") = ""

            If ddlAnno.SelectedIndex <> 0 And txtNominativo.Text <> "" Then       'And txtCerca.Text = "0" Then
                btnSearchDichiarazioni_Click(Nothing, Nothing)
                txtCerca.Text = "1"
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAccertamentiOSAP.btnRibalta_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btngotoVersContribuente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btngotoVersContribuente.Click
        dim sScript as string=""

        sScript += "ApriVisualizzaPagOSAP(" & hdIdContribuente.Value & ",'" & ddlAnno.SelectedValue & "');"
        RegisterScript(sScript, Me.GetType())
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnSvuotaSessionContribuente_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSvuotaSessionContribuente.Click
        'se svuotano il contribuente e avevano già inpostato le griglie
        'svuoto le variabili di sessione dell'accertato e del dichiarato
        txtNominativo.Text = ""
        Session("oAccertatiGriglia") = Nothing
        Session("oAccertato") = Nothing
        Session("DataSetDichiarazioni") = Nothing
        Session("HashTableRettificaAccertamenti") = Nothing
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnAnnulloNoAcc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAnnulloNoAcc.Click
        Dim oAccertato() As IRemInterfaceOSAP.Articolo
        Dim oDichiarato() As IRemInterfaceOSAP.Articolo
        Dim FncAnnulloNoAcc As New ClsGestioneAccertamenti
        Dim sDescTipoAvviso As String
        'Dim WFSessione As CreateSessione
        Dim sScript As String = ""

        Try
            oDettaglioAnagrafica = Session("oAnagrafe")
            FncAnnulloNoAcc.OSAPConfrontoAccertatoDichiarato(ConstSession.IdEnte, hdIdContribuente.Value, ddlAnno.SelectedValue, oDichiarato, oAccertato, CType(Session("HashTableRettificaAccertamenti"), Hashtable)("TIPO_OPERAZIONE_RETTIFICA"), Session("HashTableRettificaAccertamenti"), Session("DataSetSanzioni"), oDettaglioAnagrafica.DataMorte, sDescTipoAvviso, sScript)
            Session("TipoAvviso") = sDescTipoAvviso
            If sScript <> "" Then
                RegisterScript(sScript, Me.GetType())
            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAccertamentiOSAP.BtnAnnulloNoAcc_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nomeSessione"></param>
    Private Sub writeJavascriptAnagrafica(ByVal nomeSessione As String)
		dim sScript as string="" = ""
		sscript+="ApriRicercaAnagrafe('" & nomeSessione & "');" & vbCrLf
		RegisterScript(sScript , Me.GetType())
	End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Public Sub GestisciRettifica()
		Try
            oDettaglioAnagrafica = New DettaglioAnagrafica
            'Dim WFSessione As New OPENUtility.CreateSessione(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))
            'If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
            '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
            'End If

            'oAnagrafica = New GestioneAnagrafica(WFSessione.oSession, Session("Anagrafica"))
            'oDettaglioAnagrafica = oAnagrafica.GetAnagrafica(COD_CONTRIB_RETTIFICA, "0434", -1)
            oDettaglioAnagrafica = oAnagrafica.GetAnagrafica(COD_CONTRIB_RETTIFICA, -1, String.Empty, ConstSession.DBType, ConstSession.StringConnectionAnagrafica, False) 'oAnagrafica.GetAnagrafica(COD_CONTRIB_RETTIFICA, Costanti.TRIBUTO_TARSU, -1, ConstSession.DBType, ConstSession.StringConnectionAnagrafica)

            txtNominativo.Text = oDettaglioAnagrafica.Cognome & " " & oDettaglioAnagrafica.Nome

			ViewState.Add("COGNOME", oDettaglioAnagrafica.Cognome)
			ViewState.Add("NOME", oDettaglioAnagrafica.Nome)

			hdIdContribuente.Value = oDettaglioAnagrafica.COD_CONTRIBUENTE
			txtHiddenIdDataAnagrafica.Text = oDettaglioAnagrafica.ID_DATA_ANAGRAFICA
			ViewState("sessionName") = ""

			ddlAnno.SelectedValue = ANNO_RETTIFICA

			Imagebutton.Enabled = False
			ddlAnno.Enabled = False

			btnSearchDichiarazioni_Click(Nothing, Nothing)
			txtCerca.Text = "1"
            'If Not IsNothing(WFSessione) Then
            '	WFSessione.Kill()
            '	WFSessione = Nothing
            'End If
		Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAccertamentiOSAP.GestisciRettifica.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
		End Try
	End Sub
End Class
