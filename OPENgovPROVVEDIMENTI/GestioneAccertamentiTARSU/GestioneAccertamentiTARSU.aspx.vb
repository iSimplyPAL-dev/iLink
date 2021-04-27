Imports AnagInterface
Imports Anagrafica.DLL
Imports System
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Runtime.Remoting
Imports System.Runtime.Remoting.Channels
Imports System.Runtime.Remoting.Channels.Http
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters
'Imports RIBESFrameWork
'Imports ComPlusInterface.ProvvedimentiTarsu.Oggetti
Imports ComPlusInterface
'Imports OPENUtility
'Imports OPENgovTARSU
Imports RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
Imports log4net
''' <summary>
''' Pagina per la generazione dei provvedimenti TARI.
''' Contiene i parametri di gestione, le funzioni della comandiera e la griglia per la visualizzazione del dichiarato.  
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class GestioneAccertamentiTARSU
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(GestioneAccertamentiTARSU))
    Private strConnectionStringOPENgovProvvedimenti As String
    Private MyUtility As New myUtility
   
    Private WFErrore As String

    Private oAnagrafica As GestioneAnagrafica
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
        Session("COD_TRIBUTO") = Utility.Costanti.TRIBUTO_TARSU
        Session("oAnagrafe") = Nothing
        Dim sScript As String = ""
        Dim objGestOPENgovProvvedimenti As DBPROVVEDIMENTI.ProvvedimentiDB

        Try
            btnFocus.Attributes.Add("onclick", "return false;")

            MyUtility = New MyUtility

            'IMPOSTO MANUALMENTE IL CODICE TRIBUTO PERCHè SONO NELLE VIDEATE DELLA TARSU
            Session("COD_TRIBUTO") = Utility.Costanti.TRIBUTO_TARSU

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
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAccertamentiTARSU.Page_Load.LetturaHashtable_errore: ", err)
                Response.Redirect("../../PaginaErrore.aspx")
            End Try

            Session("modificaDiretta") = "False"
            Session("HashTableData") = ""
            Session("CalcolaAddizionali") = Nothing
            If ChkAddizionali.Checked = True Then
                Session("CalcolaAddizionali") = True
            Else
                Session("CalcolaAddizionali") = False
            End If

            If Page.IsPostBack = False Then
                Session.Remove("oAccertatiGriglia")
                Session.Remove("DataSetDichiarazioni")

                ViewState("sessionName") = ""

                hdIdContribuente.Value = "-1"
                txtHiddenIdDataAnagrafica.Text = "-1"

                'Creo oggetto ProvvedementiDB per l'esecuzione delle query
                objGestOPENgovProvvedimenti = New DBPROVVEDIMENTI.ProvvedimentiDB

                'Il datareader viene chiuso all'interno della funzione FillDropDownSQLString
                MyUtility.FillDropDownSQLString(ddlAnno, objGestOPENgovProvvedimenti.GetAnniProvvedimenti(ConstSession.Ambiente, ConstSession.IdEnte, Session("COD_TRIBUTO")), -1, "...")

                sScript = "parent.Comandi.location.href='CGestioneAccertamentiTARSU.aspx';"
                RegisterScript(sScript, Me.GetType())

                If Not myHasTbl("CODCONTRIBUENTE") Is Nothing Then
                    oDettaglioAnagrafica = New DettaglioAnagrafica
                    oAnagrafica = New GestioneAnagrafica
                    oDettaglioAnagrafica = oAnagrafica.GetAnagrafica(myHasTbl("CODCONTRIBUENTE"), -1, String.Empty, ConstSession.DBType, ConstSession.StringConnectionAnagrafica, False)
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

                    ViewState("sessionName") = ""
                Else
                    '*** 201504 - Nuova Gestione anagrafica con form unico ***
                    If ConstSession.HasPlainAnag Then
                        ifrmAnag.Attributes.Add("src", "../../Generali/asp/VisualAnag.aspx?IdContribuente=" + hdIdContribuente.Value + "&Azione=" + Utility.Costanti.AZIONE_NEW.ToString())
                    End If
                End If
            Else
                If CInt(hdIdContribuente.Value) <> -1 Then
                    '*** 201504 - Nuova Gestione anagrafica con form unico ***
                    If ConstSession.HasPlainAnag Then
                        ifrmAnag.Attributes.Add("src", "../../Generali/asp/VisualAnag.aspx?IdContribuente=" + hdIdContribuente.Value + "&Azione=" + Utility.Costanti.AZIONE_NEW.ToString())
                    End If
                    'eseguo lo script solo se c'è l'utente selezionato
                    If Not Session("oAccertatiGriglia") Is Nothing Then
                        Log.Debug("apro direttamente loadGridAccertato")
                        sScript = "document.getElementById('loadGridAccertato').src ='SearchAccertatiTARSU.aspx';"
                        RegisterScript(sScript, Me.GetType())
                    End If
                    If Not Session("DataSetDichiarazioni") Is Nothing Then
                        Log.Debug("apro direttamente loadGridDichiarato")
                        sScript = "document.getElementById('loadGridDichiarato').src='SearchDatiAccertamentiTARSU.aspx';" 'frames.item('loadGridDichiarato').location.href 
                        RegisterScript(sScript, Me.GetType())
                    End If
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
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAccertamentiTARSU.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Public Sub GestisciRettifica()
        Try
            oDettaglioAnagrafica = New DettaglioAnagrafica

            oAnagrafica = New GestioneAnagrafica
            oDettaglioAnagrafica = oAnagrafica.GetAnagrafica(COD_CONTRIB_RETTIFICA, -1, String.Empty, ConstSession.DBType, ConstSession.StringConnectionAnagrafica, False) 'oAnagrafica.GetAnagrafica(COD_CONTRIB_RETTIFICA, ConstSession.CodTributo, -1, ConstSession.DBType, ConstSession.StringConnectionAnagrafica)
            hdIdContribuente.Value = oDettaglioAnagrafica.COD_CONTRIBUENTE
            txtHiddenIdDataAnagrafica.Text = oDettaglioAnagrafica.ID_DATA_ANAGRAFICA

            '*** 201504 - Nuova Gestione anagrafica con form unico ***
            If ConstSession.HasPlainAnag Then
                ifrmAnag.Attributes.Add("src", "../../Generali/asp/VisualAnag.aspx?IdContribuente=" + hdIdContribuente.Value + "&Azione=" + Utility.Costanti.AZIONE_LETTURA.ToString())
            Else
                txtNominativo.Text = oDettaglioAnagrafica.Cognome & " " & oDettaglioAnagrafica.Nome
            End If
            ViewState.Add("COGNOME", oDettaglioAnagrafica.Cognome)
            ViewState.Add("NOME", oDettaglioAnagrafica.Nome)

            ViewState("sessionName") = ""

            ddlAnno.SelectedValue = ANNO_RETTIFICA
            '*** 20140701 - IMU/TARES ***
            Dim FncAcc As New ClsGestioneAccertamenti
            FncAcc.LoadTipoCalcolo(ConstSession.IdEnte, ddlAnno.SelectedValue, LblTipoCalcolo, LblTipoMQ, ChkConferimenti, ChkMaggiorazione, txtInizioConf)
            'se non selezionati nascondo
            If ChkMaggiorazione.Checked = False Then
                ChkMaggiorazione.Visible = False
            Else
                ChkMaggiorazione.Visible = True
            End If
            If ChkConferimenti.Checked = False Then
                ChkConferimenti.Visible = False
            Else
                ChkConferimenti.Visible = True
            End If
            '*** ***

            Imagebutton.Enabled = False
            ddlAnno.Enabled = False

            btnSearchDichiarazioni_Click(Nothing, Nothing)
            txtCerca.Text = "1"
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAccertamentiTARSU.GestisciRettifica.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Imagebutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Imagebutton.Click
        Try
            txtNominativo.Text = ""

            Session.Remove(ViewState("sessionName"))
            Log.Debug("Session(Anagrafica)::" & Session("Anagrafica").ToString)
            oDettaglioAnagrafica = New DettaglioAnagrafica
            oDettaglioAnagrafica.COD_CONTRIBUENTE = hdIdContribuente.Value
            oDettaglioAnagrafica.ID_DATA_ANAGRAFICA = txtHiddenIdDataAnagrafica.Text
            ViewState("sessionName") = "codContribuente"
            Session(ViewState("sessionName")) = oDettaglioAnagrafica
            writeJavascriptAnagrafica(ViewState("sessionName"))
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAccertamentiTARSU.Imagebutton_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Gestione Anagrafica
    ''' </summary>
    ''' <param name="nomeSessione"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[antonello]	09/03/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub writeJavascriptAnagrafica(ByVal nomeSessione As String)
        Dim sScript As String

        sScript = "ApriRicercaAnagrafe('" & nomeSessione & "');" & vbCrLf
        RegisterScript(sScript, Me.GetType())
    End Sub
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Gestione Anagrafica
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[antonello]	09/03/2005	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub btnRibalta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRibalta.Click
        'Azzero la varibile di sessione dell'accertato se ribalto i dati
        'di un nuovo contribuente da accertare
        Try
            Session.Remove("DataTableImmobiliDaAccertare") 'rimuovo dichiarato
            Session.Remove("oAccertato") 'rimuovo accertato
            Session.Remove("oAccertatiGriglia") 'rimuovo accertato

            oDettaglioAnagrafica = Session(ViewState("sessionName"))

            txtNominativo.Text = oDettaglioAnagrafica.Cognome & " " & oDettaglioAnagrafica.Nome
            ViewState.Add("COGNOME", oDettaglioAnagrafica.Cognome)
            ViewState.Add("NOME", oDettaglioAnagrafica.Nome)

            hdIdContribuente.Value = oDettaglioAnagrafica.COD_CONTRIBUENTE
            txtHiddenIdDataAnagrafica.Text = oDettaglioAnagrafica.ID_DATA_ANAGRAFICA
            ViewState("sessionName") = ""

            If ddlAnno.SelectedIndex <> 0 And txtNominativo.Text <> "" Then 'And txtCerca.Text = "0" Then
                btnSearchDichiarazioni_Click(Nothing, Nothing)
                txtCerca.Text = "1"
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAccertamentiTARSU.btnRibalta_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnSearchDichiarazioni_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchDichiarazioni.Click
        Dim sScript As String = ""
        Dim objHashTable As Hashtable = New Hashtable

        Try
            Log.Debug("btnSearchDichiarazioni_Click")
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

            ''DecorrenzaPresente = objCOMDichiarazioniAccertamenti.getPresenzaDecorrenzaInteressiTARSU(ddlAnno.SelectedValue, constsession.idente, objHashTable)
            ''If DecorrenzaPresente = False Then
            ''    sscript+="<script language='javascript'>"
            ''    sscript+= "alert('Attenzione! Data Decorrenza Scadenza Interessi NON configurata per l'Anno selezionato!\n Gli Interessi non verranno Calcolati')"
            ''    
            ''    ''sscript+=strscript)
            ''    ''RegisterScript("Decorrenza", strBuilder.ToString())
            ''End If

            iRetValControllo = objCOMDichiarazioniAccertamenti.GetControlliAccertamento(ConstSession.StringConnection, ddlAnno.SelectedValue, ConstSession.IdEnte, hdIdContribuente.Value, Utility.Costanti.TRIBUTO_TARSU, objHashTable)
            Log.Debug("iRetValControllo=" + iRetValControllo.ToString)
            Session.Add("ESCLUDI_PREACCERTAMENTO", False)

            If TIPO_OPERAZIONE_RETTIFICA = False Then
                If iRetValControllo = 0 Then
                    'effettuato accertamento definitivo, impossibile proseguire
                    Session("ESCLUDI_PREACCERTAMENTO") = True
                    sScript = "FoundAccDefinitivo();"
                    RegisterScript(sScript, Me.GetType())
                    Exit Sub
                End If

                If iRetValControllo = 4 Then
                    'effettuato accertamento NON definitivo, Vuoi proseguire?
                    sScript = "FoundAccNONDefinitivo();"
                    RegisterScript(sScript, Me.GetType())
                    Exit Sub
                End If
            Else
                Session.Remove("DATA_ELABORAZIONE_PER_RETTIFICA")
                iRetValControllo = 6
                Session.Add("DATA_ELABORAZIONE_PER_RETTIFICA", objHashTable("DATA_ELABORAZIONE_PER_RETTIFICA"))
            End If
            btnSearchDichiarazioniEffettivo_Click(Nothing, Nothing)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAccertamentiTARSU.btnSearchDichiarazioni_Click.errore: ", ex)
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
            Log.Debug("cambio anno")
            Session.Remove("DataSetDichiarazioni")
            '*** 20140701 - IMU/TARES ***
            Dim FncAcc As New ClsGestioneAccertamenti
            FncAcc.LoadTipoCalcolo(ConstSession.IdEnte, ddlAnno.SelectedValue, LblTipoCalcolo, LblTipoMQ, ChkConferimenti, ChkMaggiorazione, txtInizioConf)
            'se non selezionati nascondo
            If ChkMaggiorazione.Checked = False Then
                ChkMaggiorazione.Visible = False
            Else
                ChkMaggiorazione.Visible = True
            End If
            If ChkConferimenti.Checked = False Then
                ChkConferimenti.Visible = False
            Else
                ChkConferimenti.Visible = True
            End If
            '*** ***
            If ddlAnno.SelectedIndex <> 0 And LblTipoCalcolo.Text <> " -- " And hdIdContribuente.Value <> "" Then 'And txtNominativo.Text <> "" Then 'And txtCerca.Text = "0" Then
                If CInt(hdIdContribuente.Value) > 0 Then
                    Log.Debug("ricerco dich")
                    btnSearchDichiarazioni_Click(Nothing, Nothing)
                    txtCerca.Text = "1"
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAccertamentiTARSU.ddlAnno_SelectedIndexChanged.errore: ", Err)
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
            Dim sScript As String = ""
            Dim objHashTable As Hashtable = New Hashtable

            Log.Debug("btnSearchDichiarazioniEffettivo_Click")
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
                If objHashTable.ContainsKey("COD_TRIBUTO") Then
                    objHashTable.Remove("COD_TRIBUTO")
                End If
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
            '*** 20140701 - IMU/TARES ***
            objHashTable.Add("TipoTassazione", LblTipoCalcolo.Text)
            objHashTable.Add("TipoCalcolo", RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRuolo.Ruolo.APercentuale)
            objHashTable.Add("DescrTipoCalcolo", LblTipoCalcolo.Text)
            objHashTable.Add("PercentTariffe", 100)
            objHashTable.Add("HasMaggiorazione", ChkMaggiorazione.Checked)
            objHashTable.Add("HasConferimenti", ChkConferimenti.Checked)
            objHashTable.Add("TipoMQ", LblTipoMQ.Text.Substring(0, 1))
            objHashTable.Add("impSogliaAvvisi", 0)
            objHashTable.Add("IdTestata", -1)
            '*** 20181011 Dal/Al Conferimenti ***
            objHashTable.Add("tDataInizioConf", txtInizioConf.Text)
            objHashTable.Add("tDataFineConf", New OPENgovTIa.generalClass.generalFunction().FormattaData(ddlAnno.SelectedValue + "1231", "G"))
            Session.Remove("oAccertatiDaDichiarazione")
            Session.Remove("DataSetDichiarazioni")
            '*** ***
            anno = ddlAnno.SelectedValue
            'Session.Remove("DataTableImmobili")
            Session.Remove("oAccertatiGriglia")
            Session.Remove("oAccertato")
            Session("HashTableDichAccertamentiTARSU") = objHashTable
            Session("HashTableDichiarazioniAccertamenti") = objHashTable
            'Session("HashTableDichiarazioniAccertamenti") = objHashTable
            Session("HashTableRettificaAccertamenti") = objHashTable
            sScript += "document.getElementById('attesaCarica').style.display='';"
            sScript += "document.getElementById('loadGridDichiarato').src='SearchDatiAccertamentiTARSU.aspx';" 'frames.item('loadGridDichiarato').location.href 
            sScript += "document.getElementById('loadGridAccertato').src='SearchAccertatiTARSU.aspx';"
            Log.Debug("popolo loadGridDichiarato=SearchDatiAccertamentiTARSU e loadGridAccertato=SearchAccertatiTARSU")
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAccertamentiTARSU.btnSearchDichiarazioniEffettivo_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdRibaltaUIAnater_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdRibaltaUIAnater.Click
        Dim sScript As String = ""

        Try
            If Not Session("oDettaglioTestata") Is Nothing Then
                sScript += "ApriInserimentoImmobile();"
                RegisterScript(sScript, Me.GetType())
            End If
            'Session.Remove("oDettaglioTestata")
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAccertamentiTARSU.CmdRibaltaUIAnater_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btngotoVersContribuente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btngotoVersContribuente.Click
        'Session("path") = Application("nome_sito") + ConfigurationManager.AppSettings("PATH_OPENGOVTA").ToString()
        RegisterScript("ApriVisualizzaPagTARSU(" & hdIdContribuente.Value & ",'" & ddlAnno.SelectedValue & "');", Me.GetType())
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ChkAddizionali_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkAddizionali.CheckedChanged
        Session("CalcolaAddizionali") = Nothing
        Try
            If ChkAddizionali.Checked = True Then
                Session("CalcolaAddizionali") = True
            Else
                Session("CalcolaAddizionali") = False
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAccertamentiTARSU.ChkAddizionali_CheckedChanged.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
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
        'Session("oAcceratoXsanzioni") = Nothing
        Session("DataSetDichiarazioni") = Nothing
        Session("HashTableDichAccertamentiTARSU") = Nothing

    End Sub

    '*** 20140701 - IMU/TARES ***
    'Private Sub btnAnnulloNoAcc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAnnulloNoAcc.Click
    '    Dim oAccertato() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoArticoloRuolo
    '    Dim oDichiarato() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoArticoloRuolo
    '    Dim oMyAccertato As New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoArticoloRuolo
    '    Dim a As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAttoTARSU
    '    Dim objHashTable As New Hashtable
    '    objHashTable = Session("HashTableDichAccertamentiTARSU")
    '    Dim FncAnnulloNoAcc As New ClsGestioneAccertamenti
    '    Dim sDescTipoAvviso, sscript As String
    '    Dim WFSessione As CreateSessione
    '    Dim strWFErrore As String

    '    Try
    '        WFSessione = New CreateSessione(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, strWFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If
    '        oMyAccertato.Anno = ddlAnno.SelectedValue
    '        oMyAccertato.IdContribuente = txtHiddenCodContribuente.Text
    '        oMyAccertato.Ente = ConstSession.IdEnte
    '        oMyAccertato.ImportoNetto = 0
    '        ReDim Preserve oAccertato(0)
    '        oAccertato(0) = oMyAccertato
    '        a = FncAnnulloNoAcc.ConfrontoAccertatoDichiarato(ConstSession.IdEnte, WFSessione, ddlAnno.SelectedValue, oDichiarato, oAccertato, CType(Session("HashTableDichAccertamentiTARSU"), Hashtable)("TIPO_OPERAZIONE_RETTIFICA"), Session("HashTableDichAccertamentiTARSU"), Session("CalcolaAddizionali"), Session("DataSetSanzioni"), Session("VALORE_RITORNO_ACCERTAMENTO"), sDescTipoAvviso, sscript)
    '        Session("TipoAvviso") = sDescTipoAvviso
    '        If sscript <> "" Then
    '            RegisterScript(sScript , Me.GetType())
    '        End If

    '    Catch ex As Exception
    '         Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAccertamentiTARSU.btnAnnullaNoAcc_Click.errore: ", Err)
    '       Response.Redirect("../../PaginaErrore.aspx")
    '    Finally
    '        If Not IsNothing(WFSessione) Then
    '            WFSessione.Kill()
    '            WFSessione = Nothing
    '        End If
    '    End Try
    'End Sub
    ''' <summary>
    ''' Pulsante per la creazione di Autotutela di annullamento senza accertato
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnAnnulloNoAcc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnAnnulloNoAcc.Click
        Dim oAccertato() As ObjArticoloAccertamento
        Dim oDichiarato() As ObjArticoloAccertamento
        Dim oMyAccertato As New ObjArticoloAccertamento
        Dim myAtto As OggettoAttoTARSU
        Dim FncAnnulloNoAcc As New ClsGestioneAccertamenti
        Dim sDescTipoAvviso, sscript As String

        Try
            oDettaglioAnagrafica = Session("codContribuente")

            oMyAccertato.sAnno = ddlAnno.SelectedValue
            oMyAccertato.IdContribuente = hdIdContribuente.Value
            oMyAccertato.IdEnte = ConstSession.IdEnte
            oMyAccertato.impNetto = 0
            ReDim Preserve oAccertato(0)
            oAccertato(0) = oMyAccertato
            myAtto = FncAnnulloNoAcc.TARSUConfrontoAccertatoDichiarato(LblTipoCalcolo.Text, ConstSession.IdEnte, ddlAnno.SelectedValue, oDichiarato, oAccertato, CType(Session("HashTableDichAccertamentiTARSU"), Hashtable)("TIPO_OPERAZIONE_RETTIFICA"), Session("HashTableDichAccertamentiTARSU"), Session("CalcolaAddizionali"), Session("DataSetSanzioni"), False, oDettaglioAnagrafica.DataMorte, sDescTipoAvviso, sscript)
            If IsNothing(myAtto) Then
                Throw New Exception("Errore in calcolo accertamento")
            Else
                Session("TipoAvviso") = sDescTipoAvviso
                If sscript <> "" Then
                    RegisterScript(sscript, Me.GetType())
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAccertamentiTARSU.BtnAnnullaNoAcc_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** ***
End Class
