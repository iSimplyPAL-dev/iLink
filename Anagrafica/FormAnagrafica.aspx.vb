Imports System
Imports System.Xml
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Configuration
Imports Anagrafica
Imports AnagInterface
Imports log4net

Namespace PAGINA
    ''' <summary>
    ''' Pagina per la gestione dei dati anagrafici.
    ''' Vengono visualizzati i dati da anagrafe residenti se presenti.
    ''' Si possono gestire i dati anagrafici, di residenza, di invio ed i contatti.
    ''' Viene visualizzato l'elenco dei tributi per i quali è presente il soggetto con possibilità di consultazione diretta.
    ''' </summary>
    ''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    ''' <revisionHistory>
    ''' <revision date="27/10/2014">
    ''' <strong>visualizzazione tutti indirizzi spedizione</strong>
    ''' Nuova gestione indirizzi spedizione
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Analisi eventi</em>
    ''' </revision>
    ''' </revisionHistory>
    Partial Class FormAnagrafica
        Inherits ANAGRAFICAWEB.BasePage
        Private strURI As System.Uri
        Private PAG_PREC As String
        Private popup As String
        Private Log As ILog = LogManager.GetLogger(GetType(FormAnagrafica))
        Private _strXMLFileName As String
        Public UrlPopComuni As String = ANAGRAFICAWEB.ConstSession.UrlComuni
        Public UrlStradario As String = ANAGRAFICAWEB.ConstSession.UrlStradario
        Protected FncGrd As New ANAGRAFICAWEB.FunctionGrd
        Private GestError As New DLL.GestError
        Private strHTMLError As String
        Dim myFnc As New ANAGRAFICAWEB.AnagraficaDB()

#Region " Web Form Designer Generated Code "
        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        ''''''''''''''''''''''''''''''''''Dati Nascita'''''''''''''''''''''''''''''''''''''''''''/
        ''''''''''''''''''''''''''''''''''Fine Dati Nascita'''''''''''''''''''''''''''''''''''''''''''/
        ''''''''''''''''''''''''''''''''''Residenza''''''''''''''''''''''''''''''''''''''''''''''''
        ''''''''''''''''''''''''''''''''''Fine Residenza''''''''''''''''''''''''''''''''''''''''''''''''
        ''''''''''''''''''''''''''''''''''Spedizione'''''''''''''''''''''''''''''''''''''''''''''''''
        '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
        ''''''''''''''''''''''''''''''''''Fine Spedizione'''''''''''''''''''''''''''''''''''''''''''''''''

        '''''''''''''''''''''''''''''''''''TextBox UTENTE'''''''''''''''''''''''''''''''''''''''''''/
        'Protected WithEvents txtCFPIUtente As System.Web.UI.WebControls.TextBox
        'Protected WithEvents txtCognomeUtente As System.Web.UI.WebControls.TextBox
        'Protected WithEvents txtNomeUtente As System.Web.UI.WebControls.TextBox
        'Protected WithEvents txtComuneNascitaUtente As System.Web.UI.WebControls.TextBox
        'Protected WithEvents txtProvinciaNascitaUtente As System.Web.UI.WebControls.TextBox
        'Protected WithEvents txtDataNascitaUtente As System.Web.UI.WebControls.TextBox

        'Protected WithEvents txtIndirizzoResidenzaUtente As System.Web.UI.WebControls.TextBox
        'Protected WithEvents txtCivicoResidenzaUtente As System.Web.UI.WebControls.TextBox
        'Protected WithEvents txtFrazioneResidenzaUtente As System.Web.UI.WebControls.TextBox
        'Protected WithEvents txtComuneResidenzaUtente As System.Web.UI.WebControls.TextBox
        'Protected WithEvents txtCAPResidenzaUtente As System.Web.UI.WebControls.TextBox
        'Protected WithEvents txtProvinciaResidenzaUtente As System.Web.UI.WebControls.TextBox
        'Protected WithEvents txtTelefonoResidenzaUtente As System.Web.UI.WebControls.TextBox
        'Protected WithEvents txtNucleoFamiliareUtente As System.Web.UI.WebControls.TextBox
        'Protected WithEvents txtCellulareResidenzaUtente As System.Web.UI.WebControls.TextBox
        'Protected WithEvents txtCognomeSpedizioneUtente As System.Web.UI.WebControls.TextBox
        'Protected WithEvents txtNomeSpedizioneUtente As System.Web.UI.WebControls.TextBox
        'Protected WithEvents txtIndirizzoSpedizioneUtente As System.Web.UI.WebControls.TextBox
        'Protected WithEvents txtCivicoSpedizioneUtente As System.Web.UI.WebControls.TextBox
        'Protected WithEvents txtFrazioneSpedizioneUtente As System.Web.UI.WebControls.TextBox
        'Protected WithEvents txtComuneSpedizioneUtente As System.Web.UI.WebControls.TextBox
        'Protected WithEvents txtCAPSpedizioneUtente As System.Web.UI.WebControls.TextBox
        'Protected WithEvents txtProvinciaSpedizioneUtente As System.Web.UI.WebControls.TextBox
        'Protected WithEvents txtBancaUtente As System.Web.UI.WebControls.TextBox
        'Protected WithEvents txtAgenziaUtente As System.Web.UI.WebControls.TextBox
        'Protected WithEvents txtABIUtente As System.Web.UI.WebControls.TextBox
        'Protected WithEvents txtCABUtente As System.Web.UI.WebControls.TextBox
        'Protected WithEvents txtNumeroCCUtente As System.Web.UI.WebControls.TextBox
        'Protected WithEvents txtEMailUtente As System.Web.UI.WebControls.TextBox
        'Protected WithEvents txtFaxUtente As System.Web.UI.WebControls.TextBox
        '''''''''''''''''''''''''''''''''''Fine TextBox'''''''''''''''''''''''''''''''''''''''''''/
        '''''''''''''''''''''''''''''''''''CheckBox INTESTATARIO'''''''''''''''''''''''''''''''''''''''''''/
        'Protected WithEvents chkDomiciliazione As System.Web.UI.WebControls.CheckBox
        '''''''''''''''''''''''''''''''''''Fine ChecktBox INTESTATARIO'''''''''''''''''''''''''''''''''''''''''''/
        '''''''''''''''''''''''''''''''''''CheckBox UTENTE'''''''''''''''''''''''''''''''''''''''''''/
        'Protected WithEvents chkDomiciliazioneUtente As System.Web.UI.WebControls.CheckBox
        '''''''''''''''''''''''''''''''''''Fine ChecktBox UTENTE'''''''''''''''''''''''''''''''''''''''''''/
        ''''''''''''''''''''''''''''''''''' Radio Button INTESTATARIO'''''''''''''''''''''''''''''''''''''''''''/
        'Protected WithEvents Residenza As System.Web.UI.WebControls.RadioButtonList
        '''''''''''''''''''''''''''''''''''Fine Radio Button INTESTATARIO'''''''''''''''''''''''''''''''''''''''''''/
        ''''''''''''''''''''''''''''''''''' Radio Button UTENTE'''''''''''''''''''''''''''''''''''''''''''/
        'Protected WithEvents ResidenzaUtente As System.Web.UI.WebControls.RadioButtonList
        '''''''''''''''''''''''''''''''''''Fine Radio Button UTENTE'''''''''''''''''''''''''''''''''''''''''''/

        ''''''''''''''''''''''''''''''''''' DropDownList INTESTATARIO'''''''''''''''''''''''''''''''''''''''''''/
        'Protected WithEvents cboTitoloSoggetto As System.Web.UI.WebControls.DropDownList
        ''''''''''''''''''''''''''''''''''' fine DropDownList INTESTATARIO'''''''''''''''''''''''''''''''''''''''''''/
        ''''''''''''''''''''''''''''''''''' DropDownList UTENTE'''''''''''''''''''''''''''''''''''''''''''/
        'Protected WithEvents cboSessoUtente As System.Web.UI.WebControls.DropDownList
        'Protected WithEvents cboTitoloUtente As System.Web.UI.WebControls.DropDownList
        ''''''''''''''''''''''''''''''''''' fine DropDownList  UTENTE'''''''''''''''''''''''''''''''''''''''''''/
        ''''''''''''''''''''''''''''''''/GESTIONE PROCESSO''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ''''''''''''''''''''''''''''''''/GESTIONE PROCESSO''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ''Protected WithEvents txtDataInizioInvio As System.Web.UI.WebControls.TextBox
        ''Protected WithEvents chkInvioInformativeViaMail As System.Web.UI.WebControls.CheckBox


        Protected WithEvents btnConferma As System.Web.UI.WebControls.Button

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()
        End Sub
#End Region
        Public Sub New()

        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[antonello]	17/02/2005	Created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        ''' <revisionHistory>
        ''' <revision date="27/10/2014">
        ''' <strong>visualizzazione tutti indirizzi spedizione</strong>
        ''' Nuova gestione indirizzi spedizione
        ''' </revision>
        ''' </revisionHistory>
        ''' <revisionHistory>
        ''' <revision date="12/04/2019">
        ''' Modifiche da revisione manuale
        ''' </revision>
        ''' </revisionHistory>
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Dim sScript As String = ""

            Try
                CmdSaveSpedizione.Attributes.Add("OnClick", "return VerificaCampiObbligatoriSpedizione();")
                btnSalva.Attributes.Add("OnClick", "return VerificaCampiObbligatori();")
                btnDelete.Attributes.Add("OnClick", "return Conferma();")
                popup = Request.Item("popup")
                If Not Page.IsPostBack Then
                    If ViewState("sessionName") = False Then
                        ViewState("sessionName") = Request.Item("sessionName")
                    End If
                    LoadCombos()
                    'Caricamento Dati Anagrafica
                    LoadAnagrafica()
                    '*** caricamento dati da residenti ***
                    If ANAGRAFICAWEB.ConstSession.HasResidenti Then
                        GestResidenti.Style.Add("display", "")
                        LoadDatiResidenti()
                    Else
                        GestResidenti.Style.Add("display", "none")
                    End If
                    LoadTributiAssociati()
                    'Gestione Storico Anagrafica
                    If CInt(Request("STORICO")) <> Utility.Costanti.INIT_VALUE_NUMBER Then
                        SetEnabledAll(True, Nothing)
                    End If
                    ViewState.Add("PAGEFROM", Request.Item("PAGEFROM"))
                    Dim fncActionEvent As New Utility.DBUtility(ANAGRAFICAWEB.ConstSession.DBType, ANAGRAFICAWEB.ConstSession.StringConnectionOPENgov)
                    fncActionEvent.LogActionEvent(DateTime.Now, ANAGRAFICAWEB.ConstSession.UserName, New Utility.Costanti.LogEventArgument().Anagrafica, "DettaglioAnagrafica", Utility.Costanti.AZIONE_LETTURA.ToString, "", ANAGRAFICAWEB.ConstSession.IdEnte, hdIdContribuente.Value)
                End If
                If ANAGRAFICAWEB.ConstSession.IdEnte <> "" Then
                    ddlEnti.SelectedValue = ANAGRAFICAWEB.ConstSession.IdEnte
                    sScript += "document.getElementById('lblEnti').style.display='none';"
                    sScript += "document.getElementById('ddlEnti').style.display='none';"
                    sScript += "parent.Comandi.location.href='./Comandi/ComandiInsertSaveAnagrafica.aspx';"
                Else
                    If Request.Item("PAGEFROM") = "DOPP" Then
                        sScript += "parent.Comandi.location.href='./Comandi/ComandiAnagDoppieMancanti.aspx';"
                    Else
                        sScript += "parent.Comandi.location.href='./Comandi/ComandiDettaglio.aspx';"
                    End If
                End If
                RegisterScript(sScript, Me.GetType())
                ddlEnti.Enabled = False
            Catch ex As Exception
                Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.Page_Load.errore: ", ex)
                Response.Redirect("../PaginaErrore.aspx")
            End Try
        End Sub
        'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '    Dim sScript As String
        '    'Dim path As String
        '    'Dim ConfigAnagraficaXML As New XmlDocument

        '    Try
        '        '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
        '        CmdSaveSpedizione.Attributes.Add("OnClick", "return VerificaCampiObbligatoriSpedizione();")
        '        '*** ***
        '        btnSalva.Attributes.Add("OnClick", "return VerificaCampiObbligatori();")
        '        'btnControlloCINSalva.Attributes.Add("OnClick", "return VerificaCampiObbligatori();")
        '        btnDelete.Attributes.Add("OnClick", "return Conferma();")
        '        popup = Request.Item("popup")
        '        If Not Page.IsPostBack Then
        '            If ViewState("sessionName") = False Then
        '                ViewState("sessionName") = Request.Item("sessionName")
        '            End If

        '            If Request.Item("PAGEFROM") = "DOPP" Then 'Or Request.Item("PAGEFROM") = "MANC" Then
        '                sScript = "parent.Comandi.location.href='./Comandi/ComandiAnagDoppieMancanti.aspx'" & vbCrLf
        '            Else
        '                If popup = "1" Then
        '                    sScript = "parent.Comandi.location.href='./Comandi/ComandiInsertSaveAnagrafica.aspx'" & vbCrLf
        '                Else
        '                    sScript = "parent.Comandi.location.href='./Comandi/ComandiInsertSaveAnagrafica.aspx'" & vbCrLf
        '                End If
        '            End If
        '            RegisterScript(sScript, Me.GetType())

        '            'path = Server.MapPath("")
        '            '***************************************************************************************************
        '            '***************************************************************************************************
        '            LoadCombos()
        '            'ConfigAnagraficaXML.Load(DLL.Utility.AddBackSlashToPath(path) & "ConfigAnagrafica.xml")
        '            'Caricamento Dati Anagrafica
        '            LoadAnagrafica()
        '            '*** caricamento dati da residenti ***
        '            If ANAGRAFICAWEB.ConstSession.HasResidenti Then
        '                GestResidenti.Style.Add("display", "")
        '                LoadDatiResidenti()
        '            Else
        '                GestResidenti.Style.Add("display", "none")
        '            End If
        '            LoadTributiAssociati()
        '            'Gestione Storico Anagrafica
        '            If CInt(Request("STORICO")) <> Utility.Costanti.INIT_VALUE_NUMBER Then
        '                SetEnabledAll(True, Nothing)
        '            End If
        '            ViewState.Add("PAGEFROM", Request.Item("PAGEFROM"))
        '            Dim fncActionEvent As New Utility.DBUtility(ANAGRAFICAWEB.ConstSession.DBType, ANAGRAFICAWEB.ConstSession.StringConnectionOPENgov)
        '            fncActionEvent.LogActionEvent(DateTime.Now, ANAGRAFICAWEB.ConstSession.UserName, New Utility.Costanti.LogEventArgument().Anagrafica, "DettaglioAnagrafica", Utility.Costanti.AZIONE_LETTURA.ToString, "", ANAGRAFICAWEB.ConstSession.IdEnte, hdIdContribuente.Value)
        '        End If
        '        '*** 201511 - Funzioni Sovracomunali ***
        '        If ANAGRAFICAWEB.ConstSession.IdEnte <> "" Then
        '            ddlEnti.SelectedValue = ANAGRAFICAWEB.ConstSession.IdEnte
        '            sScript = "document.getElementById('lblEnti').style.display='none';"
        '            sScript += "document.getElementById('ddlEnti').style.display='none';"
        '        Else
        '            sScript += "$('#CANCELLA', parent.frames['Comandi'].document).addClass('DisableBtn');"
        '            sScript += "$('#Insert', parent.frames['Comandi'].document).addClass('DisableBtn');"
        '        End If
        '        RegisterScript(sScript, Me.GetType())
        '        ddlEnti.Enabled = False
        '        '*** ***
        '    Catch ex As Exception
        '        Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.Page_Load.errore: ", ex)
        '        Response.Redirect("../PaginaErrore.aspx")
        '        'strHTMLError = GestError.GetHTMLError(ex, Server.MapPath("/" & Application("nome_sito") & "/ERRORIANAGRAFICA.css"), "parent.Visualizza.location.href='RicercaAnagrafica.aspx?popup=" & popup & "&sessionName=" & ViewState("sessionName") & "';") & vbCrLf
        '        'strHTMLError = strHTMLError & "<script>" & vbCrLf
        '        'strHTMLError = strHTMLError & "parent.Comandi.location.href='" & "../aspError.aspx" & "';" & vbCrLf
        '        'strHTMLError = strHTMLError & "</script>"
        '        'Response.Write(strHTMLError)
        '        'Response.End()
        '    End Try
        'End Sub
#Region "NO RIBESFRAMEWORK"
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="prdStatus"></param>
        ''' <returns></returns>
        Protected Function DescRiferimento(ByVal prdStatus As Object) As String
            Dim oAnagrafica As New DLL.GestioneAnagrafica()
            DescRiferimento = oAnagrafica.DescrizioneTipoContatto(prdStatus, ANAGRAFICAWEB.ConstSession.StringConnectionAnagrafica)
            Return DescRiferimento
        End Function

        '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <revisionHistory>
        ''' <revision date="27/10/2014">
        ''' <strong>visualizzazione tutti indirizzi spedizione</strong>
        ''' Nuova gestione indirizzi spedizione
        ''' </revision>
        ''' <revision date="04/02/2020">
        ''' <strong>split payment</strong>
        ''' per la fatturazione elettronica bisogna gestire, per gli enti pubblici, lo split payment. 
        ''' Si implementa il campo Split Payment (si/no) nell'attuale campo COD_CONTRIBUENTE_RAPP_LEGALE.
        ''' Si implementa il campo Codice Univoco nell'attuale campo PROFESSIONE.
        ''' </revision>
        ''' </revisionHistory>
        Private Sub LoadAnagrafica()
            Try
                Dim COD_CONTRIBUENTE As Integer = Utility.Costanti.INIT_VALUE_NUMBER
                Dim ID_DATA_ANAGRAFICA As Integer = Utility.Costanti.INIT_VALUE_NUMBER

                lblConcurrencyMsg.Visible = False
                Try
                    COD_CONTRIBUENTE = Int32.Parse(Request.QueryString("COD_CONTRIBUENTE"))
                    ID_DATA_ANAGRAFICA = Int32.Parse(Request.QueryString("ID_DATA_ANAGRAFICA"))
                Catch
                    Throw New Exception("Errore Passaggio dei Parametri.I Parametri passati non sono corretti")
                End Try

                Dim oAnagrafica As New DLL.GestioneAnagrafica()
                Dim oDettaglioAnagrafica As New DettaglioAnagrafica
                oDettaglioAnagrafica = oAnagrafica.GetAnagrafica(COD_CONTRIBUENTE, ID_DATA_ANAGRAFICA, String.Empty, ANAGRAFICAWEB.ConstSession.DBType, ANAGRAFICAWEB.ConstSession.StringConnectionAnagrafica, True)
                '***************************************************************************************************
                If Not oDettaglioAnagrafica Is Nothing Then
                    '***************************************************************************************************
                    hdIdContribuente.Value = oDettaglioAnagrafica.COD_CONTRIBUENTE
                    hdIdAnagrafica.Value = oDettaglioAnagrafica.ID_DATA_ANAGRAFICA
                    hdCodComuneNascita.Value = oDettaglioAnagrafica.CodiceComuneNascita
                    hdCodComuneResidenza.Value = oDettaglioAnagrafica.CodiceComuneResidenza
                    hdCODViaResidenza.Value = oDettaglioAnagrafica.CodViaResidenza

                    If COD_CONTRIBUENTE > Utility.Costanti.INIT_VALUE_NUMBER Then
                        lblOperation.Text = "Modifica Anagrafica"
                    Else
                        lblOperation.Text = "Inserimento Anagrafica"
                    End If
                    '******************GESTIONE STRADARIO*********************
                    If oDettaglioAnagrafica.CodiceComuneResidenza <> "" Then
                        '***10/11/2008 - lo stradario non deve essere obbligatorio***
                        If Not UrlStradario Is Nothing Then
                            If UrlStradario <> "" Then
                                ComuneResidenzaStradario(oDettaglioAnagrafica.CodiceComuneResidenza, "Residenza")
                            End If
                        End If
                        '***************************************************
                    End If
                    '******************FINE GESTIONE STRADARIO****************
                    ddlEnti.SelectedValue = oDettaglioAnagrafica.CodEnte
                    '*** ***
                    txtCodiceFiscale.Text = oDettaglioAnagrafica.CodiceFiscale
                    txtPartitaIva.Text = oDettaglioAnagrafica.PartitaIva
                    txtCognome.Text = oDettaglioAnagrafica.Cognome
                    txtNome.Text = oDettaglioAnagrafica.Nome
                    myFnc.SelectIndexDropDownList(cboSesso, oDettaglioAnagrafica.Sesso)
                    txtLuogoNascita.Text = oDettaglioAnagrafica.ComuneNascita
                    '***************************************************************************************************************
                    txtProvinciaNascita.Text = oDettaglioAnagrafica.ProvinciaNascita
                    txtDataNascita.Text = oDettaglioAnagrafica.DataNascita
                    txtDataMorte.Text = oDettaglioAnagrafica.DataMorte
                    txtNazionalitaNascita.Text = oDettaglioAnagrafica.NazionalitaNascita
                    txtNucleoFamiliare.Text = oDettaglioAnagrafica.NucleoFamiliare
                    '***************************************************************************************************
                    chkSplitPayment.Checked = False
                    If Utility.StringOperation.FormatInt(oDettaglioAnagrafica.CodContribuenteRappLegale) > 0 Then
                        chkSplitPayment.Checked = True
                    End If
                    txtSplitCod.Text = oDettaglioAnagrafica.Professione
                    '***************************************************************************************************
                    txtComuneResidenza.Text = oDettaglioAnagrafica.ComuneResidenza
                    txtCAPResidenza.Text = oDettaglioAnagrafica.CapResidenza
                    txtProvinciaResidenza.Text = oDettaglioAnagrafica.ProvinciaResidenza
                    txtViaResidenza.Text = oDettaglioAnagrafica.ViaResidenza
                    txtPosizioneResidenza.Text = oDettaglioAnagrafica.PosizioneCivicoResidenza
                    txtNumeroCivicoResidenza.Text = oDettaglioAnagrafica.CivicoResidenza
                    txtEsponenteCivicoResidenza.Text = oDettaglioAnagrafica.EsponenteCivicoResidenza
                    txtScalaResidenza.Text = oDettaglioAnagrafica.ScalaCivicoResidenza
                    txtInternoResidenza.Text = oDettaglioAnagrafica.InternoCivicoResidenza
                    txtFrazioneResidenza.Text = oDettaglioAnagrafica.FrazioneResidenza
                    txtNazionalitaResidenza.Text = oDettaglioAnagrafica.NazionalitaResidenza
                    '*********************************************************************************************************************
                    'Caricamento combo Tipi Riferimenti
                    LoadDropDownList(cboTipoContatto, oDettaglioAnagrafica.dsTipiContatti, "IDTipoRiferimento", "DESCRIZIONE", -1)
                    Session("IndirizziSpedizione") = oDettaglioAnagrafica.ListSpedizioni
                    GrdInvio.DataSource = oDettaglioAnagrafica.ListSpedizioni.ToArray
                    GrdInvio.DataBind()
                    '*** ***
                    '*********************************************************************************************************************
                    txtNoteAnagrafica.Text = oDettaglioAnagrafica.Note
                    chkDaRicontrollare.Checked = oDettaglioAnagrafica.DaRicontrollare
                    '***************************************************************************************************
                    'Salvataggio del DataSet in Memoria di tipo dsContatti
                    Session("DataSetContatti") = oDettaglioAnagrafica.dsContatti
                    dgContatti.DataSource = oDettaglioAnagrafica.dsContatti
                    dgContatti.DataBind()

                    '''''''''Gestione Del LOCK'''''''''''''''''''''''''''''''
                    SetConcurrencyObject(oDettaglioAnagrafica)
                    '''''''''Gestione Del LOCK	'''''''''''''''''''''''''''''''
                End If
            Catch Err As Exception
                Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.LoadAnagrafica.errore: ", Err)
                Response.Redirect("../PaginaErrore.aspx")
            End Try
        End Sub
        'Private Sub LoadAnagrafica()
        '    Try
        '        Dim strDatiAnagrafica As String
        '        Dim strDatiSpedizione As String
        '        Dim strBuilderButton As New System.Text.StringBuilder
        '        Dim Constant As New Utility.Costanti
        '        Dim COD_CONTRIBUENTE As Integer = Utility.Costanti.INIT_VALUE_NUMBER
        '        Dim ID_DATA_ANAGRAFICA As Integer = Utility.Costanti.INIT_VALUE_NUMBER
        '        'Dim COD_TRIBUTO As Integer = Constant.INIT_VALUE_NUMBER

        '        lblConcurrencyMsg.Visible = False
        '        '/GESTIONE ERRORE

        '        Try
        '            COD_CONTRIBUENTE = Int32.Parse(Request.QueryString("COD_CONTRIBUENTE"))
        '            ID_DATA_ANAGRAFICA = Int32.Parse(Request.QueryString("ID_DATA_ANAGRAFICA"))
        '            'COD_TRIBUTO = Int32.Parse(Session("COD_TRIBUTO"))
        '        Catch
        '            '/Verifica Del Passaggio dei Parametri
        '            Throw New Exception("Errore Passaggio dei Parametri.I Parametri passati non sono corretti")
        '        End Try

        '        Dim oAnagrafica As New DLL.GestioneAnagrafica()
        '        Dim oDettaglioAnagrafica As New DettaglioAnagrafica
        '        oDettaglioAnagrafica = oAnagrafica.GetAnagrafica(COD_CONTRIBUENTE, ID_DATA_ANAGRAFICA, String.Empty, ANAGRAFICAWEB.ConstSession.DBType, ANAGRAFICAWEB.ConstSession.StringConnectionAnagrafica, True)
        '        '***************************************************************************************************
        '        If Not oDettaglioAnagrafica Is Nothing Then
        '            '***************************************************************************************************
        '            hdIdContribuente.Value = oDettaglioAnagrafica.COD_CONTRIBUENTE
        '            hdIdAnagrafica.Value = oDettaglioAnagrafica.ID_DATA_ANAGRAFICA
        '            hdCodComuneNascita.Value = oDettaglioAnagrafica.CodiceComuneNascita
        '            hdCODRappresentanteLegale.Value = oDettaglioAnagrafica.CodContribuenteRappLegale
        '            hdCodComuneResidenza.Value = oDettaglioAnagrafica.CodiceComuneResidenza
        '            hdCODViaResidenza.Value = oDettaglioAnagrafica.CodViaResidenza

        '            Dim ds As DataSet = oDettaglioAnagrafica.dsContatti

        '            If COD_CONTRIBUENTE > Utility.Costanti.INIT_VALUE_NUMBER Then
        '                lblOperation.Text = "Modifica Anagrafica"
        '                strDatiAnagrafica = ""
        '                strDatiAnagrafica = oDettaglioAnagrafica.CodiceFiscale
        '                strDatiAnagrafica += oDettaglioAnagrafica.PartitaIva
        '                strDatiAnagrafica += oDettaglioAnagrafica.Cognome
        '                strDatiAnagrafica += oDettaglioAnagrafica.Nome
        '                strDatiAnagrafica += oDettaglioAnagrafica.Sesso
        '                strDatiAnagrafica += oDettaglioAnagrafica.ComuneNascita
        '                strDatiAnagrafica += oDettaglioAnagrafica.ProvinciaNascita
        '                strDatiAnagrafica += oDettaglioAnagrafica.DataNascita
        '                strDatiAnagrafica += oDettaglioAnagrafica.DataMorte
        '                strDatiAnagrafica += oDettaglioAnagrafica.NazionalitaNascita
        '                strDatiAnagrafica += oDettaglioAnagrafica.Professione
        '                strDatiAnagrafica += oDettaglioAnagrafica.NucleoFamiliare

        '                strDatiAnagrafica += oDettaglioAnagrafica.RappresentanteLegale
        '                ' *** ALESSIO AGGIUNTO CAMPO hdCodComuneResidenza AL CONTROLLO
        '                strDatiAnagrafica += oDettaglioAnagrafica.CodiceComuneResidenza
        '                strDatiAnagrafica += oDettaglioAnagrafica.ComuneResidenza
        '                strDatiAnagrafica += oDettaglioAnagrafica.CapResidenza
        '                strDatiAnagrafica += oDettaglioAnagrafica.ProvinciaResidenza
        '                strDatiAnagrafica += oDettaglioAnagrafica.ViaResidenza
        '                strDatiAnagrafica += oDettaglioAnagrafica.PosizioneCivicoResidenza
        '                strDatiAnagrafica += oDettaglioAnagrafica.CivicoResidenza
        '                strDatiAnagrafica += oDettaglioAnagrafica.EsponenteCivicoResidenza
        '                strDatiAnagrafica += oDettaglioAnagrafica.ScalaCivicoResidenza
        '                strDatiAnagrafica += oDettaglioAnagrafica.InternoCivicoResidenza
        '                strDatiAnagrafica += oDettaglioAnagrafica.FrazioneResidenza
        '                strDatiAnagrafica += oDettaglioAnagrafica.NazionalitaResidenza

        '                strDatiSpedizione = ""
        '                For Each mySped As ObjIndirizziSpedizione In oDettaglioAnagrafica.ListSpedizioni
        '                    strDatiSpedizione += mySped.CognomeInvio
        '                    strDatiSpedizione += mySped.NomeInvio
        '                    strDatiSpedizione += mySped.ComuneRCP
        '                    strDatiSpedizione += mySped.CapRCP
        '                    strDatiSpedizione += mySped.ProvinciaRCP
        '                    strDatiSpedizione += mySped.ViaRCP
        '                    strDatiSpedizione += mySped.PosizioneCivicoRCP
        '                    strDatiSpedizione += mySped.CivicoRCP
        '                    strDatiSpedizione += mySped.EsponenteCivicoRCP
        '                    strDatiSpedizione += mySped.ScalaCivicoRCP
        '                    strDatiSpedizione += mySped.InternoCivicoRCP
        '                    strDatiSpedizione += mySped.FrazioneRCP
        '                Next

        '                Session("DatiInzioAnagrafica") = Replace(strDatiAnagrafica, vbCrLf, "")
        '                Session("DatiInzioSpedizione") = Replace(strDatiSpedizione, vbCrLf, "")
        '            Else
        '                lblOperation.Text = "Inserimento Anagrafica"
        '            End If

        '            RegisterScript(strBuilderButton.ToString(), Me.GetType())
        '            '******************FINE MODIFCA**************************************

        '            '******************GESTIONE STRADARIO*********************
        '            If oDettaglioAnagrafica.CodiceComuneResidenza <> "" Then
        '                '***10/11/2008 - lo stradario non deve essere obbligatorio***
        '                If Not UrlStradario Is Nothing Then
        '                    If UrlStradario <> "" Then
        '                        ComuneResidenzaStradario(oDettaglioAnagrafica.CodiceComuneResidenza, "Residenza")
        '                        'ComuneResidenzaStradario(oDettaglioAnagrafica.CodComuneRCP, "Spedizione")
        '                    End If
        '                End If
        '                '***************************************************
        '            End If
        '            '******************FINE GESTIONE STRADARIO****************
        '            '*** 201511 - Funzioni Sovracomunali ***
        '            ddlEnti.SelectedValue = oDettaglioAnagrafica.CodEnte
        '            '*** ***
        '            txtCodiceFiscale.Text = oDettaglioAnagrafica.CodiceFiscale
        '            txtPartitaIva.Text = oDettaglioAnagrafica.PartitaIva
        '            txtCognome.Text = oDettaglioAnagrafica.Cognome
        '            txtNome.Text = oDettaglioAnagrafica.Nome
        '            myFnc.SelectIndexDropDownList(cboSesso, oDettaglioAnagrafica.Sesso)
        '            txtLuogoNascita.Text = oDettaglioAnagrafica.ComuneNascita

        '            '***************************************************************************************************************
        '            txtProvinciaNascita.Text = oDettaglioAnagrafica.ProvinciaNascita
        '            txtDataNascita.Text = oDettaglioAnagrafica.DataNascita
        '            txtDataMorte.Text = oDettaglioAnagrafica.DataMorte
        '            txtNazionalitaNascita.Text = oDettaglioAnagrafica.NazionalitaNascita
        '            txtProfessione.Text = oDettaglioAnagrafica.Professione
        '            txtNucleoFamiliare.Text = oDettaglioAnagrafica.NucleoFamiliare
        '            'txtRappresentanteLegale.Text = oDettaglioAnagrafica.RappresentanteLegale
        '            'Caricamento combo Tipi Riferimenti
        '            '***************************************************************************************************
        '            '***************************************************************************************************
        '            LoadDropDownList(cboTipoContatto, oDettaglioAnagrafica.dsTipiContatti, "IDTipoRiferimento", "DESCRIZIONE", -1)
        '            '***************************************************************************************************
        '            '***************************************************************************************************
        '            'Salvataggio del DataSet in Memoria di tipo dsContatti
        '            '***************************************************************************************************
        '            '***************************************************************************************************
        '            'If ds.Tables(0).Rows.Count = 0 Then
        '            ' lblInfo.Text = "Non Sono Stati trovati contatti"
        '            'End If
        '            txtComuneResidenza.Text = oDettaglioAnagrafica.ComuneResidenza
        '            txtCAPResidenza.Text = oDettaglioAnagrafica.CapResidenza
        '            txtProvinciaResidenza.Text = oDettaglioAnagrafica.ProvinciaResidenza
        '            txtViaResidenza.Text = oDettaglioAnagrafica.ViaResidenza
        '            txtPosizioneResidenza.Text = oDettaglioAnagrafica.PosizioneCivicoResidenza
        '            txtNumeroCivicoResidenza.Text = oDettaglioAnagrafica.CivicoResidenza
        '            txtEsponenteCivicoResidenza.Text = oDettaglioAnagrafica.EsponenteCivicoResidenza
        '            txtScalaResidenza.Text = oDettaglioAnagrafica.ScalaCivicoResidenza
        '            txtInternoResidenza.Text = oDettaglioAnagrafica.InternoCivicoResidenza
        '            txtFrazioneResidenza.Text = oDettaglioAnagrafica.FrazioneResidenza
        '            txtNazionalitaResidenza.Text = oDettaglioAnagrafica.NazionalitaResidenza
        '            '*********************************************************************************************************************
        '            '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
        '            'txtCognomeSpedizioneOld.Text = oDettaglioAnagrafica.CognomeInvio
        '            'txtNomeSpedizioneOld.Text = oDettaglioAnagrafica.NomeInvio
        '            'txtComuneSpedizione.Text = oDettaglioAnagrafica.ComuneRCP
        '            'txtCAPSpedizioneOld.Text = oDettaglioAnagrafica.CapRCP
        '            'txtProvinciaSpedizioneOld.Text = oDettaglioAnagrafica.ProvinciaRCP
        '            'txtIndirizzoSpedizioneOld.Text = oDettaglioAnagrafica.ViaRCP
        '            'txtPosizioneSpedizioneOld.Text = oDettaglioAnagrafica.PosizioneCivicoRCP
        '            'txtNumeroCivicoSpedizioneOld.Text = oDettaglioAnagrafica.CivicoRCP
        '            'txtEsponenteSpedizioneOld.Text = oDettaglioAnagrafica.EsponenteCivicoRCP
        '            'txtScalaSpedizioneOld.Text = oDettaglioAnagrafica.ScalaCivicoRCP
        '            'txtInternoSpedizioneOld.Text = oDettaglioAnagrafica.InternoCivicoRCP
        '            'txtFrazioneSpedizioneOld.Text = oDettaglioAnagrafica.FrazioneRCP
        '            'GrdProva.start_index = Convert.ToString(GrdProva.CurrentPageIndex)
        '            'GrdProva.AllowCustomPaging = False
        '            'GrdProva.DataSource = oDettaglioAnagrafica.ListSpedizioni
        '            'GrdProva.DataBind()
        '            '*** ***
        '            Session("IndirizziSpedizione") = oDettaglioAnagrafica.ListSpedizioni
        '            GrdInvio.DataSource = oDettaglioAnagrafica.ListSpedizioni.ToArray
        '            GrdInvio.DataBind()
        '            '*** ***
        '            '*********************************************************************************************************************
        '            txtNoteAnagrafica.Text = oDettaglioAnagrafica.Note
        '            chkDaRicontrollare.Checked = oDettaglioAnagrafica.DaRicontrollare

        '            Session("DataSetContatti") = oDettaglioAnagrafica.dsContatti
        '            dgContatti.DataSource = oDettaglioAnagrafica.dsContatti
        '            dgContatti.DataBind()

        '            '''''''''Gestione Del LOCK'''''''''''''''''''''''''''''''
        '            SetConcurrencyObject(oDettaglioAnagrafica)
        '            '''''''''Gestione Del LOCK	'''''''''''''''''''''''''''''''
        '            '***************************************************************************************************
        '            '***************************************************************************************************
        '        End If
        '    Catch Err As Exception
        '        Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.LoadAnagrafica.errore: ", Err)
        '        Response.Redirect("../PaginaErrore.aspx")
        '    End Try
        'End Sub
#Region "CF"
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub btnCodiceFiscaleServer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCodiceFiscaleServer.Click
            Dim ControlloCodiceFiscale As New DLL.ControlliCFPI()
            Dim Cognome As String
            Dim Nome As String
            Dim Comune As String
            Dim Sesso As String
            Dim DataNascita As String
            Dim Giorno As String
            Dim Mese As String
            Dim Anno As String

            Log.Debug("calcolo codice fiscale")
            Try
                Cognome = UCase(txtCognome.Text)
                Nome = UCase(txtNome.Text)
                Comune = UCase(txtLuogoNascita.Text & txtProvinciaNascita.Text)
                Sesso = UCase(cboSesso.SelectedItem.Value)

                DataNascita = UCase(txtDataNascita.Text)

                Giorno = Mid(DataNascita, 1, 2)
                Mese = Mid(DataNascita, 4, 2)
                Anno = Mid(DataNascita, 7, 4)

                Session.LCID = 1040

                Dim dtDataNascita As DateTime = New DateTime(Anno, Mese, Giorno)

                DataNascita = dtDataNascita.ToString

                txtCodiceFiscale.Text = ControlloCodiceFiscale.Calcolo_Codice_Fiscale(Cognome, Nome, DataNascita, Sesso, Comune, "", ANAGRAFICAWEB.ConstSession.DBType, ANAGRAFICAWEB.ConstSession.StringConnectionAnagrafica)
            Catch ex As Exception
                Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.btnCodiceFiscaleServer_Click.errore: ", ex)
                Response.Redirect("../PaginaErrore.aspx")
            End Try
        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub btnDaCodiceFiscaleServer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDaCodiceFiscaleServer.Click
            Try
                Dim ControlloCodiceFiscale As New DLL.ControlliCFPI()

                Dim CodiceFiscale As String
                Dim Provincia As String = ""
                Dim Identificativo As String = ""

                CodiceFiscale = UCase(txtCodiceFiscale.Text)
                txtDataNascita.Text = ControlloCodiceFiscale.Data_Nascita_da_CodFiscale(CodiceFiscale, False, "")
                txtLuogoNascita.Text = ControlloCodiceFiscale.Luogo_Nascita_da_CodFiscale(CodiceFiscale, False, "", Identificativo, Provincia, ANAGRAFICAWEB.ConstSession.DBType, ANAGRAFICAWEB.ConstSession.StringConnectionAnagrafica)
                txtProvinciaNascita.Text = Provincia
                hdCodComuneNascita.Value = Identificativo

                myFnc.SelectIndexDropDownList(cboSesso, ControlloCodiceFiscale.Sesso_da_CodFiscale(CodiceFiscale, False, ""))

            Catch EX As Exception
                Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.btnDaCodiceFiscaleServer.errore: ", EX)
                Response.Redirect("../PaginaErrore.aspx")
            End Try
        End Sub
        ''' <summary>
        ''' Effettua il controllo sul cin del codice fiscale e della partita iva e avvisa se si vuole salvare o meno
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub btnControlloCINSalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnControlloCINSalva.Click
            Dim myWarning As String = ""
            Try

                If Len(txtCodiceFiscale.Text) > 0 Then
                    If Not New DLL.ControlliCFPI().ControlloCinCF(txtCodiceFiscale.Text.ToUpper) Then
                        myWarning += "Codice Fiscale"
                    End If
                End If
                If Len(txtPartitaIva.Text) > 0 Then
                    If myWarning <> "" Then
                        myWarning += " e "
                    End If
                    If Not New DLL.ControlliCFPI().ControlloCinPI(txtPartitaIva.Text.ToUpper) Then
                        myWarning += "Partita Iva"
                    End If
                End If
                If myWarning <> "" Then
                    RegisterScript("if (confirm('" + myWarning + " non corretto.\r\nSi vuole salvare ugualmente il dato?\r\nAltrimenti correggere il dato e salvare.')){document.getElementById('btnSalva').click();}", Me.GetType())
                Else
                    btnSalva_Click(Nothing, Nothing)
                End If
            Catch Ex As Exception
                Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.btnControlloCINSalva_Click.errore: ", Ex)
                Response.Redirect("../PaginaErrore.aspx")
            End Try
        End Sub
#End Region

        '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <revisionHistory>
        ''' <revision date="27/10/2014">
        ''' <strong>visualizzazione tutti indirizzi spedizione</strong>
        ''' Nuova gestione indirizzi spedizione
        ''' </revision>
        ''' </revisionHistory>
        ''' <revisionHistory>
        ''' <revision date="12/04/2019">
        ''' <strong>Qualificazione AgID-analisi_rel01</strong>
        ''' <em>Analisi eventi</em>
        ''' </revision>
        ''' </revisionHistory>
        Private Sub btnSalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalva.Click
            Dim blnUpdate As Boolean = True
            Dim blnReturnSearch As Boolean
            Dim lngCOD_CONTRIBUENTE As Long = Utility.Costanti.INIT_VALUE_NUMBER
            Dim oAnagrafica As New DLL.GestioneAnagrafica()
            Dim oDettaglioAnagrafica As New DettaglioAnagrafica
            Session.LCID = 1040

            Try
                Dim fncActionEvent As New Utility.DBUtility(ANAGRAFICAWEB.ConstSession.DBType, ANAGRAFICAWEB.ConstSession.StringConnectionOPENgov)

                oDettaglioAnagrafica.COD_CONTRIBUENTE = hdIdContribuente.Value
                oDettaglioAnagrafica.ID_DATA_ANAGRAFICA = hdIdAnagrafica.Value

                oDettaglioAnagrafica.CodiceFiscale = txtCodiceFiscale.Text
                oDettaglioAnagrafica.PartitaIva = txtPartitaIva.Text
                oDettaglioAnagrafica.Cognome = txtCognome.Text
                oDettaglioAnagrafica.Nome = txtNome.Text
                If cboSesso.SelectedItem.Value = CStr(Utility.Costanti.INIT_VALUE_NUMBER) Then
                    oDettaglioAnagrafica.Sesso = ""
                Else
                    oDettaglioAnagrafica.Sesso = cboSesso.SelectedItem.Value
                End If

                oDettaglioAnagrafica.ComuneNascita = txtLuogoNascita.Text

                oDettaglioAnagrafica.CodiceComuneNascita = hdCodComuneNascita.Value

                oDettaglioAnagrafica.ProvinciaNascita = txtProvinciaNascita.Text
                oDettaglioAnagrafica.DataNascita = txtDataNascita.Text
                oDettaglioAnagrafica.DataMorte = txtDataMorte.Text
                oDettaglioAnagrafica.NazionalitaNascita = txtNazionalitaNascita.Text
                oDettaglioAnagrafica.NucleoFamiliare = txtNucleoFamiliare.Text
                '********************************************************************************
                oDettaglioAnagrafica.CodContribuenteRappLegale = -1
                If chkSplitPayment.Checked Then
                    oDettaglioAnagrafica.CodContribuenteRappLegale = 1
                End If
                oDettaglioAnagrafica.Professione = txtSplitCod.Text
                '********************************************************************************
                oDettaglioAnagrafica.RappresentanteLegale = ""
                oDettaglioAnagrafica.ComuneResidenza = txtComuneResidenza.Text
                oDettaglioAnagrafica.CodiceComuneResidenza = hdCodComuneResidenza.Value
                oDettaglioAnagrafica.CapResidenza = txtCAPResidenza.Text
                oDettaglioAnagrafica.ProvinciaResidenza = txtProvinciaResidenza.Text
                oDettaglioAnagrafica.ViaResidenza = txtViaResidenza.Text
                oDettaglioAnagrafica.Operatore = ANAGRAFICAWEB.ConstSession.Operatore

                oDettaglioAnagrafica.CodViaResidenza = hdCODViaResidenza.Value

                oDettaglioAnagrafica.PosizioneCivicoResidenza = txtPosizioneResidenza.Text
                oDettaglioAnagrafica.CivicoResidenza = txtNumeroCivicoResidenza.Text
                oDettaglioAnagrafica.EsponenteCivicoResidenza = txtEsponenteCivicoResidenza.Text
                oDettaglioAnagrafica.ScalaCivicoResidenza = txtScalaResidenza.Text
                oDettaglioAnagrafica.InternoCivicoResidenza = txtInternoResidenza.Text
                oDettaglioAnagrafica.FrazioneResidenza = txtFrazioneResidenza.Text
                oDettaglioAnagrafica.NazionalitaResidenza = txtNazionalitaResidenza.Text

                oDettaglioAnagrafica.Note = txtNoteAnagrafica.Text
                oDettaglioAnagrafica.DaRicontrollare = chkDaRicontrollare.Checked
                '*** 201511 - Funzioni Sovracomunali ***
                If ANAGRAFICAWEB.ConstSession.IdEnte = "" Then
                    oDettaglioAnagrafica.CodEnte = ddlEnti.SelectedValue
                Else
                    oDettaglioAnagrafica.CodEnte = ANAGRAFICAWEB.ConstSession.IdEnte
                End If
                Log.Debug("SalvaAnagrafica::cod ente::" & oDettaglioAnagrafica.CodEnte)
                '*** ***
                oDettaglioAnagrafica.Concurrency = Date.Now()

                oDettaglioAnagrafica.ListSpedizioni = Session("IndirizziSpedizione")
                oDettaglioAnagrafica.dsContatti = Session("DataSetContatti")
                If oDettaglioAnagrafica.COD_CONTRIBUENTE = Utility.Costanti.INIT_VALUE_NUMBER Then
                    '*************************************************
                    'NUOVO
                    '*************************************************
                    lngCOD_CONTRIBUENTE = oAnagrafica.SetAnagraficaCompleta(oDettaglioAnagrafica, ANAGRAFICAWEB.ConstSession.DBType, ANAGRAFICAWEB.ConstSession.StringConnectionAnagrafica)
                    fncActionEvent.LogActionEvent(DateTime.Now, ANAGRAFICAWEB.ConstSession.UserName, New Utility.Costanti.LogEventArgument().Anagrafica, "btnSalva", Utility.Costanti.AZIONE_NEW, "", ANAGRAFICAWEB.ConstSession.IdEnte, lngCOD_CONTRIBUENTE)
                Else
                    '*************************************************
                    'MODIFICA
                    '************************************************
                    Try
                        'Se il record non e' Loccato Continua
                        oAnagrafica.UpdateForLock(oDettaglioAnagrafica, ANAGRAFICAWEB.ConstSession.StringConnectionAnagrafica)
                    Catch ex As DBConcurrencyException
                        If ConcurrencyException(ANAGRAFICAWEB.ConstSession.DBType, ANAGRAFICAWEB.ConstSession.StringConnectionAnagrafica) Then
                            blnUpdate = False
                        Else
                            blnUpdate = False
                            'Se non sono state trovate modifiche sui campi
                            blnReturnSearch = True
                        End If
                        Dim strPostForm As New System.Text.StringBuilder
                        strPostForm.Append("document.getElementById('btnCodiceFiscale').disabled=true;")
                        strPostForm.Append("document.getElementById('btnDaCodiceFiscale').disabled=true;")
                        strPostForm.Append("document.getElementById('btnCaricaTabella').disabled=true;")
                        RegisterScript(strPostForm.ToString(), Me.GetType())
                    Catch ex As DeletedRowInaccessibleException
                        Throw
                    Catch ex As Exception
                        Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.btnSalva_Click.errore: ", ex)
                        Response.Redirect("../PaginaErrore.aspx")
                        Throw
                    End Try

                    'Se non c'e' stata concorrenza sul record si eseguono operazioni sul Data Base Anagrafica
                    If blnUpdate Then
                        Dim oMyAnag As New DettaglioAnagraficaReturn
                        oMyAnag = oAnagrafica.GestisciAnagrafica(oDettaglioAnagrafica, ANAGRAFICAWEB.ConstSession.DBType, ANAGRAFICAWEB.ConstSession.StringConnectionAnagrafica, False, True)
                        lngCOD_CONTRIBUENTE = oMyAnag.COD_CONTRIBUENTE
                        fncActionEvent.LogActionEvent(DateTime.Now, ANAGRAFICAWEB.ConstSession.UserName, New Utility.Costanti.LogEventArgument().Anagrafica, "btnSalva", Utility.Costanti.AZIONE_UPDATE, "", ANAGRAFICAWEB.ConstSession.IdEnte, lngCOD_CONTRIBUENTE)
                    End If    'blnUpdate
                End If
                '************************************************
                '/Se non si sono verificati errori gestione della Pagina su cui tornare dopo il salvataggio
                If blnUpdate Then
                    GestReturnPage(oDettaglioAnagrafica, lngCOD_CONTRIBUENTE)
                End If
                If blnReturnSearch Then
                    GestReturnPage(oDettaglioAnagrafica, lngCOD_CONTRIBUENTE)
                End If
                DivAttesa.Style.Add("display", "none")
            Catch ex As Exception
                Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.btnSalva_Click.errore: ", ex)
                Response.Redirect("../PaginaErrore.aspx")
            End Try
        End Sub
        'Private Sub btnSalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalva.Click
        '    Dim GestError As New DLL.GestError
        '    Dim Costant As New Utility.Costanti
        '    Dim blnUpdate As Boolean = True
        '    Dim blnReturnSearch As Boolean
        '    Dim strSesso As String

        '    Dim lngCOD_CONTRIBUENTE As Long = Utility.Costanti.INIT_VALUE_NUMBER
        '    Dim oAnagrafica As New DLL.GestioneAnagrafica()
        '    Dim oDettaglioAnagrafica As New DettaglioAnagrafica
        '    Dim controlloCFPI As New DLL.ControlliCFPI()
        '    Session.LCID = 1040

        '    Try
        '        Dim fncActionEvent As New Utility.DBUtility(ANAGRAFICAWEB.ConstSession.DBType, ANAGRAFICAWEB.ConstSession.StringConnectionOPENgov)

        '        oDettaglioAnagrafica.COD_CONTRIBUENTE = hdIdContribuente.Value
        '        oDettaglioAnagrafica.ID_DATA_ANAGRAFICA = hdIdAnagrafica.Value

        '        oDettaglioAnagrafica.CodiceFiscale = txtCodiceFiscale.Text
        '        oDettaglioAnagrafica.PartitaIva = txtPartitaIva.Text
        '        oDettaglioAnagrafica.Cognome = txtCognome.Text
        '        oDettaglioAnagrafica.Nome = txtNome.Text
        '        If cboSesso.SelectedItem.Value = CStr(Utility.Costanti.INIT_VALUE_NUMBER) Then
        '            oDettaglioAnagrafica.Sesso = ""
        '            strSesso = ""
        '        Else
        '            oDettaglioAnagrafica.Sesso = cboSesso.SelectedItem.Value
        '            strSesso = cboSesso.SelectedItem.Value
        '        End If

        '        oDettaglioAnagrafica.ComuneNascita = txtLuogoNascita.Text

        '        oDettaglioAnagrafica.CodiceComuneNascita = hdCodComuneNascita.Value

        '        oDettaglioAnagrafica.ProvinciaNascita = txtProvinciaNascita.Text
        '        oDettaglioAnagrafica.DataNascita = txtDataNascita.Text
        '        oDettaglioAnagrafica.DataMorte = txtDataMorte.Text
        '        oDettaglioAnagrafica.NazionalitaNascita = txtNazionalitaNascita.Text
        '        oDettaglioAnagrafica.Professione = txtProfessione.Text
        '        oDettaglioAnagrafica.NucleoFamiliare = txtNucleoFamiliare.Text
        '        oDettaglioAnagrafica.RappresentanteLegale = ""
        '        oDettaglioAnagrafica.CodContribuenteRappLegale = hdCODRappresentanteLegale.Value
        '        oDettaglioAnagrafica.ComuneResidenza = txtComuneResidenza.Text
        '        oDettaglioAnagrafica.CodiceComuneResidenza = hdCodComuneResidenza.Value
        '        oDettaglioAnagrafica.CapResidenza = txtCAPResidenza.Text
        '        oDettaglioAnagrafica.ProvinciaResidenza = txtProvinciaResidenza.Text
        '        oDettaglioAnagrafica.ViaResidenza = txtViaResidenza.Text
        '        oDettaglioAnagrafica.Operatore = ANAGRAFICAWEB.ConstSession.Operatore

        '        oDettaglioAnagrafica.CodViaResidenza = hdCODViaResidenza.Value

        '        oDettaglioAnagrafica.PosizioneCivicoResidenza = txtPosizioneResidenza.Text
        '        oDettaglioAnagrafica.CivicoResidenza = txtNumeroCivicoResidenza.Text
        '        oDettaglioAnagrafica.EsponenteCivicoResidenza = txtEsponenteCivicoResidenza.Text
        '        oDettaglioAnagrafica.ScalaCivicoResidenza = txtScalaResidenza.Text
        '        oDettaglioAnagrafica.InternoCivicoResidenza = txtInternoResidenza.Text
        '        oDettaglioAnagrafica.FrazioneResidenza = txtFrazioneResidenza.Text
        '        oDettaglioAnagrafica.NazionalitaResidenza = txtNazionalitaResidenza.Text

        '        oDettaglioAnagrafica.Note = txtNoteAnagrafica.Text
        '        oDettaglioAnagrafica.DaRicontrollare = chkDaRicontrollare.Checked
        '        '*** 201511 - Funzioni Sovracomunali ***
        '        If ANAGRAFICAWEB.ConstSession.IdEnte = "" Then
        '            oDettaglioAnagrafica.CodEnte = ddlEnti.SelectedValue
        '        Else
        '            oDettaglioAnagrafica.CodEnte = ANAGRAFICAWEB.ConstSession.IdEnte
        '        End If
        '        Log.Debug("SalvaAnagrafica::cod ente::" & oDettaglioAnagrafica.CodEnte)
        '        '*** ***
        '        oDettaglioAnagrafica.Concurrency = Date.Now()

        '        oDettaglioAnagrafica.ListSpedizioni = Session("IndirizziSpedizione")
        '        oDettaglioAnagrafica.dsContatti = Session("DataSetContatti")
        '        If oDettaglioAnagrafica.COD_CONTRIBUENTE = Utility.Costanti.INIT_VALUE_NUMBER Then
        '            '*************************************************
        '            'NUOVO
        '            '*************************************************
        '            lngCOD_CONTRIBUENTE = oAnagrafica.SetAnagraficaCompleta(oDettaglioAnagrafica, ANAGRAFICAWEB.ConstSession.DBType, ANAGRAFICAWEB.ConstSession.StringConnectionAnagrafica)
        '            fncActionEvent.LogActionEvent(DateTime.Now, ANAGRAFICAWEB.ConstSession.UserName, New Utility.Costanti.LogEventArgument().Anagrafica, "btnSalva", Utility.Costanti.AZIONE_NEW, "", ANAGRAFICAWEB.ConstSession.IdEnte, lngCOD_CONTRIBUENTE)
        '        Else
        '            '*************************************************
        '            'MODIFICA
        '            '************************************************
        '            Try
        '                'Se il record non e' Loccato Continua
        '                oAnagrafica.UpdateForLock(oDettaglioAnagrafica, ANAGRAFICAWEB.ConstSession.StringConnectionAnagrafica)
        '            Catch ex As DBConcurrencyException
        '                If ConcurrencyException(ANAGRAFICAWEB.ConstSession.DBType, ANAGRAFICAWEB.ConstSession.StringConnectionAnagrafica) Then
        '                    blnUpdate = False
        '                Else
        '                    blnUpdate = False
        '                    'Se non sono state trovate modifiche sui campi
        '                    blnReturnSearch = True
        '                End If
        '                Dim strPostForm As New System.Text.StringBuilder
        '                strPostForm.Append("document.getElementById('btnCodiceFiscale').disabled=true;")
        '                strPostForm.Append("document.getElementById('btnDaCodiceFiscale').disabled=true;")
        '                strPostForm.Append("document.getElementById('btnCaricaTabella').disabled=true;")
        '                RegisterScript(strPostForm.ToString(), Me.GetType())
        '            Catch ex As DeletedRowInaccessibleException
        '                Throw
        '            Catch ex As Exception
        '                Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.btnSalva_Click.errore: ", ex)
        '                Response.Redirect("../PaginaErrore.aspx")
        '                Throw
        '            End Try

        '            'Se non c'e' stata concorrenza sul record si eseguono operazioni sul Data Base Anagrafica
        '            If blnUpdate Then
        '                Dim oMyAnag As New DettaglioAnagraficaReturn
        '                oMyAnag = oAnagrafica.GestisciAnagrafica(oDettaglioAnagrafica, ANAGRAFICAWEB.ConstSession.DBType, ANAGRAFICAWEB.ConstSession.StringConnectionAnagrafica, False, True)
        '                lngCOD_CONTRIBUENTE = oMyAnag.COD_CONTRIBUENTE
        '                fncActionEvent.LogActionEvent(DateTime.Now, ANAGRAFICAWEB.ConstSession.UserName, New Utility.Costanti.LogEventArgument().Anagrafica, "btnSalva", Utility.Costanti.AZIONE_UPDATE, "", ANAGRAFICAWEB.ConstSession.IdEnte, lngCOD_CONTRIBUENTE)
        '            End If    'blnUpdate
        '        End If
        '        '************************************************
        '        '/Se non si sono verificati errori gestione della Pagina su cui tornare dopo il salvataggio
        '        If blnUpdate Then
        '            GestReturnPage(oDettaglioAnagrafica, lngCOD_CONTRIBUENTE)
        '        End If
        '        If blnReturnSearch Then
        '            GestReturnPage(oDettaglioAnagrafica, lngCOD_CONTRIBUENTE)
        '        End If
        '        DivAttesa.Style.Add("display", "none")
        '    Catch ex As Exception
        '        Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.btnSalva_Click.errore: ", ex)
        '        Response.Redirect("../PaginaErrore.aspx")
        '    End Try
        'End Sub
        ''' <summary>
        ''' Pulsante per la cancellazione dell'anagrafica.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <revisionHistory>
        ''' <revision date="12/04/2019">
        ''' <strong>Qualificazione AgID-analisi_rel01</strong>
        ''' <em>Analisi eventi</em>
        ''' </revision>
        ''' </revisionHistory>
        Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
            Dim oAnagrafica As New DLL.GestioneAnagrafica()

            Try
                oAnagrafica.DeleteAnagrafica(hdIdContribuente.Value, hdIdAnagrafica.Value, ANAGRAFICAWEB.ConstSession.DBType, ANAGRAFICAWEB.ConstSession.StringConnectionAnagrafica)
                Dim fncActionEvent As New Utility.DBUtility(ANAGRAFICAWEB.ConstSession.DBType, ANAGRAFICAWEB.ConstSession.StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, ANAGRAFICAWEB.ConstSession.UserName, New Utility.Costanti.LogEventArgument().Anagrafica, "btnDelete", Utility.Costanti.AZIONE_DELETE, "", ANAGRAFICAWEB.ConstSession.IdEnte, hdIdContribuente.Value)
                DivAttesa.Style.Add("display", "none")
                RegisterScript("parent.Visualizza.location.href='RicercaAnagrafica.aspx?popup=" & popup & "';", Me.GetType())
            Catch ex As Exception
                Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.btnDelete_Click.errore: ", ex)
                If ex.Message.IndexOf("00000") > 0 Then
                    RegisterScript("GestAlert('a', 'warning', '', '', 'Impossibile cancellare la posizione anagrafica selezionata, è utilizzata all\'interno del sistema!')", Me.GetType())
                Else
                    Response.Redirect("../PaginaErrore.aspx")
                End If
            End Try
        End Sub
        'Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        '    'Dim hdCOD_CONTRIBUENTE As Integer = Request.Params("hdCOD_CONTRIBUENTE")
        '    'Dim ID_DATA_ANAGRAFICA As String = Request.Params("ID_DATA_ANAGRAFICA")
        '    Dim oAnagrafica As New DLL.GestioneAnagrafica()

        '    Try
        '        oAnagrafica.DeleteAnagrafica(hdIdContribuente.Value, hdIdAnagrafica.Value, ANAGRAFICAWEB.ConstSession.StringConnectionAnagrafica)
        '        DivAttesa.Style.Add("display", "none")
        '        RegisterScript("parent.Visualizza.location.href='RicercaAnagrafica.aspx?popup=" & popup & "';", Me.GetType())
        '    Catch ex As Exception
        '        Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.btnDelete_Click.errore: ", ex)
        '        If ex.Message.IndexOf("00000") > 0 Then
        '            RegisterScript("GestAlert('a', 'warning', '', '', 'Impossibile cancellare la posizione anagrafica selezionata, è utilizzata all\'interno del sistema!')", Me.GetType())
        '        Else
        '            Response.Redirect("../PaginaErrore.aspx")
        '        End If
        '    End Try
        'End Sub
        'Private Sub btnCancella_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancella.Click
        '    Dim GestError As New DLL.GestError

        '    Try
        '        Dim hdCOD_CONTRIBUENTE As Integer = Request.Params("hdCOD_CONTRIBUENTE")
        '        Dim ID_DATA_ANAGRAFICA As String = Request.Params("ID_DATA_ANAGRAFICA")
        '        Dim oAnagrafica As New DLL.GestioneAnagrafica()

        '        oAnagrafica.DeleteAnagrafica(hdCOD_CONTRIBUENTE, ID_DATA_ANAGRAFICA, ANAGRAFICAWEB.ConstSession.DBType, ANAGRAFICAWEB.ConstSession.StringConnectionAnagrafica)
        '        RegisterScript("parent.Visualizza.location.href='RicercaAnagrafica.aspx?popup=" & popup & "';", Me.GetType())
        '    Catch EX As Exception
        '        Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.btnCancella_Click.errore: ", EX)
        '        Response.Redirect("../PaginaErrore.aspx")
        '    End Try
        'End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub btnAnnulla_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAnnulla.Click
            Dim sScript As String = ""
            Try
                If StrComp(Session("modificaDiretta"), "True") = 0 Then
                    sScript += "parent.window.close();"
                Else
                    If ViewState("PAGEFROM") = "DOPP" Then
                        sScript += "parent.Visualizza.location.href='RicercaAnagraficaDoppia.aspx';"
                    ElseIf ViewState("PAGEFROM") = "MANC" Then
                        sScript += "parent.Comandi.location.href='./Comandi/ComandiRicercaAnagraficaManc.aspx';"
                        sScript += "parent.Visualizza.location.href='RicercaAnagraficaMancante.aspx';"
                    Else
                        sScript += "parent.Visualizza.location.href='RicercaAnagrafica.aspx?popup=" & popup & "&sessionName=" & ViewState("sessionName") & "';"
                    End If
                End If
                RegisterScript(sScript, Me.GetType)
            Catch EX As Exception
                Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.btnAnnulla_Click.errore: ", EX)
                Response.Redirect("../PaginaErrore.aspx")
            End Try
        End Sub
#End Region
#Region "Concurrency"
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="DBType"></param>
        ''' <param name="StringConnection"></param>
        ''' <returns></returns>
        Private Function ConcurrencyException(DBType As String, ByVal StringConnection As String) As Boolean
            Dim operatore As String = ""
            Dim message As String
            'Dim controls As Hashtable = GetControlsMap()
            Try
                ConcurrencyException = False

                'If ShowConcurrencyFields(controls, operatore, DBType, StringConnection) Then
                '    'Se sono state fatte delle Modifiche da un altro utente trova delle differenze e mostra le differenze dei campi
                '    ConcurrencyException = True
                'End If
                lblConcurrencyMsg.Visible = True
                message = "l' Utente " & operatore & " ha modificato questa anagrafica da quando avete cominciato le Vostre modifiche." & vbCrLf
                message = message & "I cambiamenti fatti dall'altro utente " & operatore & " sono indicati dal colore grigio." & vbCrLf
                message = message & "Prego Ricontrollare i cambiamenti." & vbCrLf
                message = message & "Per modificare nuovamente questa anagrafica,riselezionarla dalla pagina di ricerca."

                lblConcurrencyMsg.Text = message

                SetEnabledAll(False, Nothing)
            Catch EX As Exception
                Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.ConcurrencyException.errore: ", EX)
                Response.Redirect("../PaginaErrore.aspx")
            End Try
        End Function
        'Private Function GetControlsMap() As Hashtable
        '    Dim Controls As Hashtable = New Hashtable
        '    Try
        '        Controls.Add("CodiceFiscale", txtCodiceFiscale)
        '        Controls.Add("PartitaIva", txtPartitaIva)
        '        Controls.Add("Cognome", txtCognome)
        '        Controls.Add("Nome", txtNome)

        '        Controls.Add("ComuneNascita", txtLuogoNascita)
        '        Controls.Add("ProvinciaNascita", txtProvinciaNascita)
        '        Controls.Add("DataNascita", txtDataNascita)
        '        Controls.Add("DataMorte", txtDataMorte)
        '        Controls.Add("NazionalitaNascita", txtNazionalitaNascita)
        '        Controls.Add("Professione", txtProfessione)
        '        Controls.Add("NucleoFamiliare", txtNucleoFamiliare)
        '        Controls.Add("RappresentanteLegale", "") 'txtRappresentanteLegale
        '        Controls.Add("ComuneResidenza", txtComuneResidenza)
        '        Controls.Add("CapResidenza", txtCAPResidenza)
        '        Controls.Add("ProvinciaResidenza", txtProvinciaResidenza)
        '        Controls.Add("ViaResidenza", txtViaResidenza)
        '        Controls.Add("PosizioneCivicoResidenza", txtPosizioneResidenza)
        '        Controls.Add("CivicoResidenza", txtNumeroCivicoResidenza)
        '        Controls.Add("EsponenteCivicoResidenza", txtEsponenteCivicoResidenza)
        '        Controls.Add("ScalaCivicoResidenza", txtScalaResidenza)
        '        Controls.Add("InternoCivicoResidenza", txtInternoResidenza)
        '        Controls.Add("FrazioneResidenza", txtFrazioneResidenza)
        '        Controls.Add("NazionalitaResidenza", txtNazionalitaResidenza)
        '        'Controls.Add("CognomeInvio", txtCognomeSpedizione)
        '        'Controls.Add("NomeInvio", txtNomeSpedizione)
        '        'Controls.Add("ComuneRCP", txtComuneSpedizione)
        '        'Controls.Add("CapRCP", txtCAPSpedizione)
        '        'Controls.Add("ProvinciaRCP", txtProvinciaSpedizione)
        '        'Controls.Add("ViaRCP", txtIndirizzoSpedizione)
        '        'Controls.Add("PosizioneCivicoRCP", txtPosizioneSpedizione)
        '        'Controls.Add("CivicoRCP", txtNumeroCivicoSpedizione)
        '        'Controls.Add("EsponenteCivicoRCP", txtEsponenteSpedizione)
        '        'Controls.Add("ScalaCivicoRCP", txtScalaSpedizione)
        '        'Controls.Add("InternoCivicoRCP", txtInternoSpedizione)
        '        'Controls.Add("FrazioneRCP", txtFrazioneSpedizione)
        '        Controls.Add("Note", txtNoteAnagrafica)
        '        Controls.Add("DaRicontrollare", chkDaRicontrollare)
        '        Controls.Add("Sesso", cboSesso)
        '        Controls.Add("Contatti", dgContatti)

        '        Return Controls
        '    Catch EX As Exception
        '        Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.GetControlsMap.errore: ", EX)
        '        Response.Redirect("../PaginaErrore.aspx")
        '        Return Nothing
        '    End Try
        'End Function
#End Region
#Region "Caricamento Combo"
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="cboTemp"></param>
        ''' <param name="dsTemp"></param>
        ''' <param name="DataValueField"></param>
        ''' <param name="DataTextField"></param>
        ''' <param name="lngSelectedID"></param>
        Private Sub LoadDropDownList(ByVal cboTemp As DropDownList, ByVal dsTemp As DataSet, ByVal DataValueField As String, ByVal DataTextField As String, ByVal lngSelectedID As Long)
            cboTemp.DataSource = dsTemp
            cboTemp.DataValueField = DataValueField
            cboTemp.DataTextField = DataTextField
            cboTemp.DataBind()
        End Sub
#End Region
#Region "Gestione Contatti e griglia Contatti"
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub btnConfermaContatti_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfermaContatti.Click
            Try
                'Gestione Modifca DataSet 
                Dim hdIDRIFERIMENTO As Int32 = CInt(Request.Params("hdIDRIFERIMENTO"))

                Dim oAnagrafica As New DLL.GestioneAnagrafica()

                'Inserimento elementi nel Data Set 
                'Inserire il TipoRiferimento ed il Dato Riferimento
                '*************************************************************************************************************
                Dim ds As DataSet = oAnagrafica.SetContatti(Session("DataSetContatti"), cboTipoContatto.SelectedIndex, txtDatiRiferimento.Text, txtDataInizioInvio.Text, hdIDRIFERIMENTO, String.Empty)
                '*************************************************************************************************************
                lblInfo.Text = ""
                Session("DataSetContatti") = ds
                If hdIDRIFERIMENTO <> Utility.Costanti.INIT_VALUE_NUMBER Then
                    RegisterScript("document.getElementById('hdIDRIFERIMENTO').value='" & Utility.Costanti.INIT_VALUE_NUMBER & "';", Me.GetType())
                End If
                cboTipoContatto.SelectedIndex = -1
                txtDatiRiferimento.Text = ""
                chkInvioInformativeViaMail.Checked = False : txtDataInizioInvio.Text = ""

                '***************************************************************************************************
                dgContatti.DataKeyField = "IDRIFERIMENTO"
                dgContatti.DataSource = myFnc.RetDataView(ds)
                dgContatti.DataBind()
                '***************************************************************************************************
            Catch ex As Exception
                Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.btnConfermaContatti_Click.errore: ", ex)
                Response.Redirect("../PaginaErrore.aspx")
            End Try
        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub btnEliminaContatti_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEliminaContatti.Click
            Try
                Dim hdIDRIFERIMENTO As Int32 = CInt(Request.Params("hdIDRIFERIMENTO"))
                Dim oAnagrafica As New DLL.GestioneAnagrafica()
                'Inserimento elementi nel Data Set 
                'Inserire il TipoRiferimento ed il Dato Riferimento
                '*************************************************************************************************************
                Dim ds As DataSet = oAnagrafica.DeleteContatti(Session("DataSetContatti"), hdIDRIFERIMENTO)
                '*************************************************************************************************************
                lblInfo.Text = ""
                Session("DataSetContatti") = ds
                If hdIDRIFERIMENTO <> Utility.Costanti.INIT_VALUE_NUMBER Then
                    RegisterScript("document.getElementById('hdIDRIFERIMENTO')value='" & Utility.Costanti.INIT_VALUE_NUMBER & "';", Me.GetType())
                End If
                cboTipoContatto.SelectedIndex = -1
                txtDatiRiferimento.Text = ""
                chkInvioInformativeViaMail.Checked = False : txtDataInizioInvio.Text = ""
                '***************************************************************************************************
                dgContatti.DataKeyField = "IDRIFERIMENTO"
                dgContatti.DataSource = myFnc.RetDataView(ds)
                dgContatti.DataBind()
                '***************************************************************************************************
            Catch EX As Exception
                Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.btnEliminaContatti.errore: ", EX)
                Response.Redirect("../PaginaErrore.aspx")
            End Try
        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Public Sub dgContatti_OnItemDataBound(ByVal sender As Object, ByVal e As DataGridItemEventArgs)
            Dim lblSID As Label
            Dim lblDescrizione As Label
            Dim lblIDRIFERIMENTO As Label
            Dim lblDataInizioInvio As Label
            Dim strArgumentsID As String
            Dim strArgumentsDESC As String
            Dim strArgumentsIDRIFERIMENTO As String
            Dim strArgumentsDataInizioInvio As String
            Try
                Select Case e.Item.ItemType
                    Case ListItemType.Item
                        lblSID = CType(e.Item.FindControl("lblTipoRiferimento"), Label)
                        lblIDRIFERIMENTO = CType(e.Item.FindControl("lblIDRIFERIMENTO"), Label)
                        lblDescrizione = CType(e.Item.FindControl("DatiRiferimento"), Label)
                        lblDataInizioInvio = CType(e.Item.FindControl("DataInizioInvio"), Label)

                        strArgumentsID = "'" & lblSID.Text & "'"
                        strArgumentsDESC = "'" & lblDescrizione.Text & "'"
                        strArgumentsIDRIFERIMENTO = "'" & lblIDRIFERIMENTO.Text & "'"
                        strArgumentsDataInizioInvio = "'" & lblDataInizioInvio.Text.Replace("Data validità invio: ", "") & "'"

                        e.Item.Attributes.Add("OnClick", "ModificaContatti(" & strArgumentsID & "," & strArgumentsDESC & "," & strArgumentsIDRIFERIMENTO & "," & strArgumentsDataInizioInvio & ");")
                    Case ListItemType.AlternatingItem
                        lblSID = CType(e.Item.FindControl("lblTipoRiferimento"), Label)
                        lblIDRIFERIMENTO = CType(e.Item.FindControl("lblIDRIFERIMENTO"), Label)
                        lblDescrizione = CType(e.Item.FindControl("DatiRiferimento"), Label)
                        lblDataInizioInvio = CType(e.Item.FindControl("DataInizioInvio"), Label)

                        strArgumentsID = "'" & lblSID.Text & "'"
                        strArgumentsDESC = "'" & lblDescrizione.Text & "'"
                        strArgumentsIDRIFERIMENTO = "'" & lblIDRIFERIMENTO.Text & "'"
                        strArgumentsDataInizioInvio = "'" & lblDataInizioInvio.Text.Replace("Data validità invio: ", "") & "'"

                        e.Item.Attributes.Add("OnClick", "ModificaContatti(" & strArgumentsID & "," & strArgumentsDESC & "," & strArgumentsIDRIFERIMENTO & "," & strArgumentsDataInizioInvio & ");")
                End Select

                If e.Item.ItemType = ListItemType.Item Then
                    e.Item.Attributes.Add("bgcolor", "White")
                End If

                If e.Item.ItemType = ListItemType.AlternatingItem Then
                    e.Item.Attributes.Add("bgcolor", "WhiteSmoke")
                End If

                e.Item.Attributes.Add("onmouseover", "this.className='riga_tabella_mouse_over_Normal'")
                e.Item.Attributes.Add("onmouseout", "this.className='riga_tabella_Normal'")
            Catch EX As Exception
                Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.dgContatti_OnItemDataBound.errore: ", EX)
                Response.Redirect("../PaginaErrore.aspx")
            End Try
        End Sub
        '*** ***
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Public Sub dgContatti_OnDeleteCommand(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
            Try
                'Cancella un Contatto dal DataSet memorizzato
                Dim dataSet As DataSet = Session("DataSetContatti")
                Dim deletekey As String = dgContatti.DataKeys(CInt(e.Item.ItemIndex))
                Dim DBTable As DataTable = dataSet.Tables("CONTATTI")
                Dim DBRow As DataRow
                For Each DBRow In DBTable.Rows
                    If DBRow("IDRIFERIMENTO") = deletekey Then
                        DBRow.Delete()
                        Exit For
                    End If
                Next

                'Accetta i cambiamenti
                dataSet.AcceptChanges()
                Session("DataSetContatti") = dataSet
                cboTipoContatto.SelectedIndex = -1
                txtDatiRiferimento.Text = ""
                dgContatti.EditItemIndex = -1

                dgContatti.DataKeyField = "IDRIFERIMENTO"

                dgContatti.DataSource = myFnc.RetDataView(dataSet)
                dgContatti.DataBind()

            Catch ex As Exception
                Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.dgContatti_OnDeleteCommand.errore: ", ex)
                Response.Redirect("../PaginaErrore.aspx")
            End Try
        End Sub
#End Region
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="bEnabled"></param>
        ''' <param name="ctlPageForm"></param>
        Protected Sub SetEnabledAll(ByVal bEnabled As Boolean, ByVal ctlPageForm As System.Web.UI.Control)
            Dim ctlForm As Control
            Dim strType As String
            Try

                If ctlPageForm Is Nothing Then
                    ctlPageForm = Me.Page
                End If
                strType = ctlPageForm.ToString()

                If TypeOf ctlPageForm Is System.Web.UI.HtmlControls.HtmlForm Or TypeOf ctlPageForm Is System.Web.UI.WebControls.Panel Or strType.IndexOf("MultiPage") <> -1 Or strType.IndexOf("PageView") <> -1 Then
                    ctlForm = ctlPageForm
                Else
                    If TypeOf ctlPageForm Is System.Web.UI.Page Then
                        ctlForm = ctlPageForm
                        Dim ctlItem As Control
                        For Each ctlItem In ctlPageForm.Controls
                            If TypeOf ctlItem Is System.Web.UI.HtmlControls.HtmlForm Then
                                ctlForm = ctlItem
                                Exit For
                            End If
                        Next ctlItem
                    Else
                        Return
                    End If
                End If
                Dim ctl As Control
                For Each ctl In ctlForm.Controls
                    If TypeOf ctl Is System.Web.UI.WebControls.DataGrid Or TypeOf ctl Is System.Web.UI.WebControls.TextBox Or TypeOf ctl Is System.Web.UI.WebControls.DropDownList Or TypeOf ctl Is System.Web.UI.WebControls.ListBox Or TypeOf ctl Is System.Web.UI.WebControls.CheckBox Or TypeOf ctl Is System.Web.UI.WebControls.CheckBoxList Or TypeOf ctl Is System.Web.UI.WebControls.RadioButton Or TypeOf ctl Is System.Web.UI.WebControls.RadioButtonList Then
                        If CType(ctl, WebControl).ClientID <> "CommandAction" Then
                            CType(ctl, WebControl).Enabled = bEnabled
                        End If
                    Else
                        strType = ctl.ToString()
                        If TypeOf ctl Is System.Web.UI.WebControls.Panel Or strType.IndexOf("MultiPage") <> -1 Or strType.IndexOf("PageView") <> -1 Then
                            SetEnabledAll(bEnabled, ctl)
                        End If
                    End If
                Next ctl
            Catch ex As Exception
                Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.SetEnabledAll.errore: ", ex)
                Response.Redirect("../PaginaErrore.aspx")
            End Try
        End Sub


        ''' <summary>
        ''' Metodo che gestisce il ritorno di Pagina dopo il salvataggio dell'Anagrafica
        ''' </summary>
        ''' <param name="oDettaglioAnagrafica"></param>
        ''' <param name="lngCOD_CONTRIBUENTE"></param>
        Private Sub GestReturnPage(ByVal oDettaglioAnagrafica As DettaglioAnagrafica, ByVal lngCOD_CONTRIBUENTE As Long)
            Try
                If ViewState("PAGEFROM") = "DOPP" Then
                    RegisterScript("parent.Visualizza.location.href='RicercaAnagraficaDoppia.aspx';", Me.GetType())
                ElseIf ViewState("PAGEFROM") = "MANC" Then
                    Dim sScript As String = ""
                    sScript += "parent.Comandi.location.href='./Comandi/ComandiRicercaAnagraficaManc.aspx';"
                    sScript += "parent.Visualizza.location.href='RicercaAnagraficaMancante.aspx';"
                    RegisterScript(sScript, Me.GetType)
                Else
                    Dim buffScriptString As New System.Text.StringBuilder
                    'Dim Costant As New ANAGRAFICAWEB.Costanti
                    'Se non si sono verificati errori gestione della Pagina su cui tornare dopo il salvataggio

                    'Gestione dell'Anagrafica da parte del data Entry massivo dei contatori 
                    buffScriptString.Append("document.getElementById('hdIDDATAANAGRAFICA').value ='" & Replace(oDettaglioAnagrafica.ID_DATA_ANAGRAFICA, "'", "\'") & "';")
                    buffScriptString.Append("document.getElementById('hdCOD_CONTRIBUENTE').value ='" & Replace(oDettaglioAnagrafica.COD_CONTRIBUENTE, "'", "\'") & "';")
                    If (StrComp(Session("modificaDiretta"), "True") = 0) Then
                        Session(ViewState("sessionName")) = oDettaglioAnagrafica
                        buffScriptString.Append("parent.parent.opener.document.getElementById('btnRibalta').click();" & vbCrLf)
                        buffScriptString.Append("parent.parent.window.close();")
                        RegisterScript(buffScriptString.ToString(), Me.GetType())
                        Exit Sub
                    End If
                    buffScriptString.Append("parent.Visualizza.location.href='RicercaAnagrafica.aspx?sessionName=" & ViewState("sessionName") & "&popup=" & popup & "';")
                    RegisterScript(buffScriptString.ToString(), Me.GetType())
                End If
            Catch ex As Exception
                Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.GestReturnPage.errore: ", ex)
                Response.Redirect("../PaginaErrore.aspx")
            End Try
        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="CodBelfiore"></param>
        ''' <param name="sSuffisso"></param>
        Public Sub ComuneResidenzaStradario(ByVal CodBelfiore As String, ByVal sSuffisso As String)
            Try
                Dim objEnte As New OggettiComuniStrade.OggettoEnte
                Dim ArrObjEnte As OggettiComuniStrade.OggettoEnte()

                objEnte.Cap = ""
                objEnte.CodBelfiore = CodBelfiore
                objEnte.CodCNC = ""
                objEnte.CodIstat = ""
                objEnte.Denominazione = ""
                objEnte.Provincia = ""
                objEnte.Stradario = False

                ArrObjEnte = New WsStradario.Stradario().GetEnti(objEnte)

                If ArrObjEnte.Length = 1 Then
                    Dim strjs As String = String.Empty
                    If IsNumeric(ArrObjEnte(0).CodIstat) = True Then
                        strjs &= "document.getElementById('hdCodComStrada" & sSuffisso & "').value = " + Integer.Parse(ArrObjEnte(0).CodIstat.ToString).ToString + ";"
                    End If
                    If ArrObjEnte(0).Stradario = True Then
                        strjs &= "AbilitaRicercaStradario('linkStrada" & sSuffisso & "', '');"
                        strjs &= "AbilitaTxtStradario('txtVia" & sSuffisso & "', true);"
                    End If
                    RegisterScript(strjs, Me.GetType())
                End If

            Catch ex As Exception
                Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.ComuneResidenzaStradario.errore: ", ex)
            End Try
        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        Private Sub LoadDatiResidenti()
            Try
                Dim oAnagResidente As New DLL.AnagraficaResidente
                If Not IsNothing(Session("DatiResidenti")) Then
                    oAnagResidente = Session("DatiResidenti")
                Else
                    Dim DsAnag As DataSet = oAnagResidente.getAnagrafeResidentiTributi(ANAGRAFICAWEB.ConstSession.StringConnectionAnagrafica, ANAGRAFICAWEB.ConstSession.IdEnte, txtCognome.Text, txtNome.Text, txtCodiceFiscale.Text, 2, 2)
                    If Not IsNothing(DsAnag) Then
                        For Each myRow As DataRow In DsAnag.Tables(0).Rows
                            If Not IsDBNull(myRow("cod_individuale")) Then
                                oAnagResidente.CodIndividuale = myRow("cod_individuale")
                            End If
                            If Not IsDBNull(myRow("data_movimento")) Then
                                oAnagResidente.Azione = "il " & myRow("data_movimento") & " "
                            End If
                            If Not IsDBNull(myRow("descrizione")) Then
                                oAnagResidente.Azione += myRow("descrizione")
                            End If
                            If Not IsDBNull(myRow("cod_fiscale")) Then
                                oAnagResidente.CodiceFiscale = myRow("cod_fiscale")
                            End If
                            If Not IsDBNull(myRow("cognome")) Then
                                oAnagResidente.Cognome = myRow("cognome")
                            End If
                            If Not IsDBNull(myRow("nome")) Then
                                oAnagResidente.Nome = myRow("nome")
                            End If
                            If Not IsDBNull(myRow("sesso")) Then
                                oAnagResidente.Sesso = myRow("sesso")
                            End If
                            If Not IsDBNull(myRow("luogo_nascita")) Then
                                oAnagResidente.ComuneNascita = myRow("luogo_nascita")
                            End If
                            If Not IsDBNull(myRow("data_nascita")) Then
                                oAnagResidente.DataNascita = myRow("data_nascita")
                            End If
                            If Utility.StringOperation.FormatDateTime(myRow("data_morte")).Year <> 9999 Then
                                oAnagResidente.DataMorte = myRow("data_morte")
                            End If
                            If Not IsDBNull(myRow("via")) Then
                                oAnagResidente.ViaResidenza = myRow("via")
                            End If
                            If Not IsDBNull(myRow("numero")) Then
                                oAnagResidente.CivicoResidenza = myRow("numero")
                            End If
                            If Not IsDBNull(myRow("lettera")) Then
                                oAnagResidente.EsponenteCivicoResidenza = myRow("lettera")
                            End If
                            If Not IsDBNull(myRow("interno")) Then
                                oAnagResidente.InternoCivicoResidenza = myRow("interno")
                            End If
                            If Not IsDBNull(myRow("numero_famiglia")) Then
                                oAnagResidente.CodFamiglia = myRow("numero_famiglia")
                            End If
                            If Not IsDBNull(myRow("descrizione_pos")) Then
                                oAnagResidente.DescrParentela = myRow("descrizione_pos")
                            End If
                        Next
                        DsAnag.Dispose()
                    End If
                End If
                If oAnagResidente.CodIndividuale > 0 Then
                    LblAzione.Text = oAnagResidente.Azione
                    LblCodFiscaleRes.Text = oAnagResidente.CodiceFiscale
                    LblCognomeRes.Text = oAnagResidente.Cognome
                    LblNomeRes.Text = oAnagResidente.Nome
                    LblSessoRes.Text = oAnagResidente.Sesso
                    LblComuneNasRes.Text = oAnagResidente.ComuneNascita
                    LblDataNasRes.Text = oAnagResidente.DataNascita
                    LblDataMorteRes.Text = oAnagResidente.DataMorte
                    LblIndirizzoRes.Text = oAnagResidente.ViaResidenza & " " & oAnagResidente.CivicoResidenza & " " & oAnagResidente.EsponenteCivicoResidenza & " " & oAnagResidente.InternoCivicoResidenza
                    LblCodFamigliaRes.Text = oAnagResidente.CodFamiglia
                    LblParentelaRes.Text = oAnagResidente.DescrParentela
                    DivDatiResidenti.Style.Add("display", "")
                Else
                    DivDatiResidenti.Style.Add("display", "none")
                End If
            Catch ex As Exception
                Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.LoadDatiResidenti.errore: ", ex)
            End Try
        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        Private Sub LoadTributiAssociati()
            Dim cmdMyCommand As New SqlClient.SqlCommand
            Dim myAdapter As New SqlClient.SqlDataAdapter
            Dim myDataSet As New DataSet

            Try
                If Not IsNothing(Session("TributiAssociati")) Then
                    myDataSet = Session("TributiAssociati")
                Else
                    cmdMyCommand.Connection = New SqlClient.SqlConnection(ANAGRAFICAWEB.ConstSession.StringConnectionAnagrafica)
                    cmdMyCommand.Connection.Open()
                    cmdMyCommand.CommandTimeout = 0
                    cmdMyCommand.CommandType = CommandType.StoredProcedure
                    cmdMyCommand.Parameters.Add("@IDENTE", SqlDbType.NVarChar).Value = ANAGRAFICAWEB.ConstSession.IdEnte
                    cmdMyCommand.Parameters.Add("@IDCONTRIBUENTE", SqlDbType.Int).Value = hdIdContribuente.Value
                    cmdMyCommand.CommandText = "prc_GetTributiVSContrib"
                    myAdapter.SelectCommand = cmdMyCommand
                    Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                    myAdapter.Fill(myDataSet, "Create DataView")
                    myAdapter.Dispose()

                    Session("TributiAssociati") = myDataSet
                End If
                GrdTributi.DataSource = myDataSet.Tables(0)
                GrdTributi.DataBind()
            Catch ex As Exception
                Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.LoadTributiAssociati.errore: ", ex)
                Response.Redirect("../PaginaErrore.aspx")
            End Try
        End Sub
#Region "Spedizione"
        '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <revisionHistory>
        ''' <revision date="27/10/2014">
        ''' <strong>visualizzazione tutti indirizzi spedizione</strong>
        ''' Nuova gestione indirizzi spedizione
        ''' </revision>
        ''' </revisionHistory>
        ''' <revisionHistory>
        ''' <revision date="12/04/2019">
        ''' Modifiche da revisione manuale
        ''' </revision>
        ''' </revisionHistory>
        Protected Sub GrdRowDataBound(sender As Object, e As GridViewRowEventArgs)
            Try
                Dim MyBtn As ImageButton
                If (e.Row.RowType = DataControlRowType.DataRow) Then
                    If CType(e.Row.DataItem, ObjIndirizziSpedizione).ID_DATA_SPEDIZIONE = -1 Then
                        MyBtn = CType(e.Row.FindControl("imgUpd"), ImageButton)
                        MyBtn.CssClass = "BottoneGrd BottoneNewInsertGrd"
                        MyBtn.ToolTip = "Nuovo"
                        e.Row.Cells(5).Enabled = False

                        MyBtn = CType(e.Row.FindControl("imgDel"), ImageButton)
                        MyBtn.CssClass = "BottoneGrd"
                        MyBtn.ToolTip = ""
                        e.Row.Cells(6).Enabled = False
                    Else
                        MyBtn = CType(e.Row.FindControl("imgUpd"), ImageButton)
                        MyBtn.CssClass = "BottoneGrd BottoneApriGrd"
                        MyBtn.ToolTip = "Modifica"
                        e.Row.Cells(5).Enabled = True

                        MyBtn = CType(e.Row.FindControl("imgDel"), ImageButton)
                        MyBtn.CssClass = "BottoneGrd BottoneCancellaGrd"
                        MyBtn.ToolTip = "Cancella"
                        e.Row.Cells(6).Enabled = True
                    End If
                End If
            Catch ex As Exception
                Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.GrdRowDataBound.errore: ", ex)
                'Response.Redirect("../PaginaErrore.aspx")
            End Try
        End Sub
        'Protected Sub GrdRowDataBound(sender As Object, e As GridViewRowEventArgs)
        '    Try
        '        Dim MyBtn As ImageButton
        '        If (e.Row.RowType = DataControlRowType.DataRow) Then
        '            If CType(e.Row.DataItem, ObjIndirizziSpedizione).ID_DATA_SPEDIZIONE = -1 Then
        '                MyBtn = CType(e.Row.FindControl("imgUpd"), ImageButton)
        '                MyBtn.CssClass = "BottoneGrd BottoneNewInsertGrd"
        '                MyBtn.ToolTip = "Nuovo"
        '                e.Row.Cells(5).Enabled = False
        '            Else
        '                MyBtn = CType(e.Row.FindControl("imgUpd"), ImageButton)
        '                MyBtn.CssClass = "BottoneGrd BottoneApriGrd"
        '                MyBtn.ToolTip = "Modifica"
        '                e.Row.Cells(5).Enabled = True
        '            End If
        '        End If
        '    Catch ex As Exception
        '        Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.GrdRowDataBound.errore: ", ex)
        '        Response.Redirect("../PaginaErrore.aspx")
        '    End Try
        'End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
            Dim mySped As New ObjIndirizziSpedizione
            Dim ListSpedizione As New Generic.List(Of ObjIndirizziSpedizione)
            Try
                Dim IDRow As Integer = CInt(e.CommandArgument.ToString())
                If e.CommandName = "RowOpen" Then
                    DivIndSped.Style.Add("display", "")
                    For Each mySped In Session("IndirizziSpedizione")
                        If mySped.ID_DATA_SPEDIZIONE = IDRow Then
                            hdIdSpedizione.Value = mySped.ID_DATA_SPEDIZIONE
                            If mySped.CodTributo <> "" Then
                                ddlTributo.SelectedValue = mySped.CodTributo.PadLeft(4, "0")
                            End If
                            txtCognomeSpedizione.Text = mySped.CognomeInvio
                            txtNomeSpedizione.Text = mySped.NomeInvio
                            txtComuneSpedizione.Text = mySped.ComuneRCP
                            txtCAPSpedizione.Text = mySped.CapRCP
                            txtProvinciaSpedizione.Text = mySped.ProvinciaRCP
                            txtIndirizzoSpedizione.Text = mySped.ViaRCP
                            txtPosizioneSpedizione.Text = mySped.PosizioneCivicoRCP
                            txtNumeroCivicoSpedizione.Text = mySped.CivicoRCP
                            txtEsponenteSpedizione.Text = mySped.EsponenteCivicoRCP
                            txtScalaSpedizione.Text = mySped.ScalaCivicoRCP
                            txtInternoSpedizione.Text = mySped.InternoCivicoRCP
                            txtFrazioneSpedizione.Text = mySped.FrazioneRCP
                            hdCodComuneSpedizione.Value = mySped.CodComuneRCP
                            hdCODViaSpedizione.Value = mySped.CodViaRCP
                        End If
                    Next
                ElseIf e.CommandName = "RowDelete" Then
                    For Each mySped In Session("IndirizziSpedizione")
                        If mySped.ID_DATA_SPEDIZIONE <> IDRow Then
                            ListSpedizione.Add(mySped)
                        End If
                    Next
                    Session("IndirizziSpedizione") = ListSpedizione
                    GrdInvio.DataSource = ListSpedizione.ToArray
                    GrdInvio.DataBind()
                End If
            Catch ex As Exception
                Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.GrdRowCommand.errore: ", ex)
                Response.Redirect("../PaginaErrore.aspx")
            End Try
        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        Private Sub LoadCombos()
            Dim cmdMyCommand As New SqlClient.SqlCommand
            Dim myDataReader As SqlClient.SqlDataReader

            Try
                cmdMyCommand.Connection = New SqlClient.SqlConnection(ANAGRAFICAWEB.ConstSession.StringConnectionOPENgov)
                cmdMyCommand.Connection.Open()
                cmdMyCommand.CommandTimeout = 0
                cmdMyCommand.CommandType = CommandType.StoredProcedure
                cmdMyCommand.CommandText = "prc_GetTributi"
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                myDataReader = cmdMyCommand.ExecuteReader
                Try
                    ddlTributo.Items.Clear()
                    ddlTributo.Items.Add("...")
                    ddlTributo.Items(0).Value = "-1"
                    If Not myDataReader Is Nothing Then
                        Do While myDataReader.Read
                            If Not IsDBNull(myDataReader(0)) Then
                                ddlTributo.Items.Add(myDataReader(0))
                                ddlTributo.Items(ddlTributo.Items.Count - 1).Value = myDataReader(1)
                            End If
                        Loop
                    End If
                Catch ex As Exception
                    Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.LoadCombos.errore: ", ex)
                    Response.Redirect("../PaginaErrore.aspx")
                End Try
            Catch ex As Exception

                Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.LoadCombos.errore: ", ex)
                Response.Redirect("../PaginaErrore.aspx")
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
                    Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.LoadCombos.errore: ", ex)
                    Response.Redirect("../PaginaErrore.aspx")
                Finally
                    myDataReader.Close()
                End Try
            Catch ex As Exception
                Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.LoadCombos.errore: ", ex)
                Response.Redirect("../PaginaErrore.aspx")
            Finally
                cmdMyCommand.Connection.Close()
                cmdMyCommand.Dispose()
            End Try
            '*** ***
        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub CmdSaveSpedizione_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdSaveSpedizione.Click
            Dim mySped As New ObjIndirizziSpedizione
            Dim ListSpedizione As New Generic.List(Of ObjIndirizziSpedizione)
            Dim bTrovato As Boolean = False

            Try
                'carico i dati della videata
                mySped = LoadSpedFromForm()
                If mySped.ID_DATA_SPEDIZIONE = -1 Then
                    'carico un id fittizio per non sovrascrivermi se inserisco + di 1 indirizzo nuovo
                    mySped.ID_DATA_SPEDIZIONE = (Session("IndirizziSpedizione").Count * -100)
                End If
                For Each oSped As ObjIndirizziSpedizione In Session("IndirizziSpedizione")
                    If oSped.ID_DATA_SPEDIZIONE = mySped.ID_DATA_SPEDIZIONE Then
                        oSped = mySped
                        bTrovato = True
                    End If
                    If oSped.CodTributo <> "" Then
                        'il rigo vuoto si aggiunge in coda
                        ListSpedizione.Add(oSped)
                    End If
                Next
                If bTrovato = False Then
                    ListSpedizione.Add(mySped)
                End If
                ListSpedizione.Add(New ObjIndirizziSpedizione)
                'controllo che non ci sia un doppio tributo
                For Each oSped As ObjIndirizziSpedizione In ListSpedizione
                    If oSped.CodTributo.PadLeft(4, "0") = mySped.CodTributo And oSped.ID_DATA_SPEDIZIONE <> mySped.ID_DATA_SPEDIZIONE Then
                        SvuotaIndSped()
                        RegisterScript("GestAlert('a', 'warning', '', '', 'Indirizzo per Tributo gia\' presente!\nImpossibile inserirlo come nuovo!')", Me.GetType())
                        Exit Sub
                    End If
                Next
                'ricarico la griglia
                Session("IndirizziSpedizione") = ListSpedizione
                GrdInvio.DataSource = ListSpedizione.ToArray
                GrdInvio.DataBind()
                SvuotaIndSped()
            Catch ex As Exception
                Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.CmdSaveSpedizione_Click.errore: ", ex)
                Response.Redirect("../PaginaErrore.aspx")
            End Try
        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub CmdRibaltaSpedizione_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdRibaltaSpedizione.Click
            Dim mySped As New ObjIndirizziSpedizione
            Dim ListSpedizione As New Generic.List(Of ObjIndirizziSpedizione)

            Try
                'carico i dati della videata
                mySped = LoadSpedFromForm()
                If mySped.ID_DATA_SPEDIZIONE <= 0 Then
                    'carico un id fittizio per non sovrascrivermi se inserisco + di 1 indirizzo nuovo
                    mySped.ID_DATA_SPEDIZIONE = (Session("IndirizziSpedizione").Count * -100)
                End If
                ListSpedizione.Add(mySped)
                'carico l'indirizzo per tutti gli altri tributi
                For Each myItem As ListItem In ddlTributo.Items
                    If mySped.CodTributo <> myItem.Value And myItem.Value <> "-1" Then
                        Dim oSped As New ObjIndirizziSpedizione
                        oSped = LoadSpedFromForm()
                        oSped.ID_DATA_SPEDIZIONE = (ListSpedizione.Count * -100) + 1
                        oSped.CodTributo = myItem.Value
                        oSped.DescrTributo = myItem.Text
                        ListSpedizione.Add(oSped)
                    End If
                Next
                ListSpedizione.Add(New ObjIndirizziSpedizione)
                'controllo che non ci sia un doppio tributo
                For Each oSped As ObjIndirizziSpedizione In ListSpedizione
                    If oSped.CodTributo.PadLeft(4, "0") = mySped.CodTributo And oSped.ID_DATA_SPEDIZIONE <> mySped.ID_DATA_SPEDIZIONE Then
                        SvuotaIndSped()
                        RegisterScript("GestAlert('a', 'warning', '', '', 'Indirizzo per Tributo gia\' presente!\nImpossibile inserirlo come nuovo!')", Me.GetType())
                        Exit Sub
                    End If
                Next
                'ricarico la griglia
                Session("IndirizziSpedizione") = ListSpedizione
                GrdInvio.DataSource = ListSpedizione.ToArray
                GrdInvio.DataBind()
                SvuotaIndSped()
            Catch ex As Exception
                Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.CmdRibaltaSpedizione_Click.errore: ", ex)
                Response.Redirect("../PaginaErrore.aspx")
            End Try
        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Private Sub CmdUnloadSpedizione_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdUnloadSpedizione.Click
            Try
                SvuotaIndSped()
            Catch ex As Exception
                Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.CmdUndoladSpedizione_Click.errore: ", ex)
                Response.Redirect("../PaginaErrore.aspx")
            End Try
        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Private Function LoadSpedFromForm() As ObjIndirizziSpedizione
            Dim mySped As New ObjIndirizziSpedizione

            Try
                mySped.ID_DATA_SPEDIZIONE = hdIdSpedizione.Value
                mySped.CodTributo = ddlTributo.SelectedValue
                mySped.DescrTributo = ddlTributo.SelectedItem.Text
                mySped.CognomeInvio = txtCognomeSpedizione.Text
                mySped.NomeInvio = txtNomeSpedizione.Text
                mySped.CodComuneRCP = hdCodComuneSpedizione.Value
                mySped.ComuneRCP = txtComuneSpedizione.Text
                mySped.CapRCP = txtCAPSpedizione.Text
                mySped.ProvinciaRCP = txtProvinciaSpedizione.Text
                mySped.CodViaRCP = hdCODViaSpedizione.Value
                mySped.ViaRCP = txtIndirizzoSpedizione.Text
                mySped.PosizioneCivicoRCP = txtPosizioneSpedizione.Text
                mySped.CivicoRCP = txtNumeroCivicoSpedizione.Text
                mySped.EsponenteCivicoRCP = txtEsponenteSpedizione.Text
                mySped.ScalaCivicoRCP = txtScalaSpedizione.Text
                mySped.InternoCivicoRCP = txtInternoSpedizione.Text
                mySped.FrazioneRCP = txtFrazioneSpedizione.Text
                mySped.OperatoreSpedizione = ANAGRAFICAWEB.ConstSession.UserName
                Return mySped
            Catch ex As Exception
                Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.LoadSpedFromForm.errore: ", ex)
                Return Nothing
            End Try
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        Private Sub SvuotaIndSped()
            Try
                'svuoto le text
                hdCODViaSpedizione.Value = "-1" : hdCodComuneSpedizione.Value = "-1"
                ddlTributo.SelectedValue = -1
                txtCognomeSpedizione.Text = "" : txtNomeSpedizione.Text = ""
                txtCAPSpedizione.Text = "" : txtComuneSpedizione.Text = "" : txtProvinciaSpedizione.Text = ""
                txtIndirizzoSpedizione.Text = "" : txtPosizioneSpedizione.Text = "" : txtNumeroCivicoSpedizione.Text = "" : txtEsponenteSpedizione.Text = "" : txtScalaSpedizione.Text = "" : txtInternoSpedizione.Text = ""
                txtFrazioneSpedizione.Text = ""
                DivIndSped.Style.Add("display", "none")
            Catch ex As Exception
                Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.SvuotaIndSped.errore: ", ex)
                Response.Redirect("../PaginaErrore.aspx")
            End Try
        End Sub
        '*** ***
#End Region
#Region "Griglie"
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Protected Sub GrdTributiRowCommand(sender As Object, e As GridViewCommandEventArgs)
            Try
                Dim sScript As String = ""
                Dim IDRow As String = e.CommandArgument.ToString()
                If e.CommandName = "RowOpen" Then
                    If ANAGRAFICAWEB.ConstSession.IdEnte <> "" Then
                        Select Case IDRow
                            Case Utility.Costanti.TRIBUTO_ICI
                                sScript = "parent.Visualizza.location.href='.." & ANAGRAFICAWEB.ConstSession.Path_ICI & "/Gestione.aspx?IdEnte=" & ANAGRAFICAWEB.ConstSession.IdEnte & "&TipoRicerca=Persona&Cognome=" & txtCognome.Text & "&Nome=" & txtNome.Text & "&CodiceFiscale=" & txtCodiceFiscale.Text & "&PartitaIVA=" & txtPartitaIva.Text & "';"
                                sScript += "parent.Comandi.location.href='.." & ANAGRAFICAWEB.ConstSession.Path_ICI & "/CGestione.aspx';"
                                sScript += "parent.Basso.location.href='../aspVuota.aspx';"
                                sScript += "parent.Nascosto.location.href='../aspSvuota.aspx';"
                            Case Utility.Costanti.TRIBUTO_TARSU
                                sScript = "parent.Visualizza.location.href='.." & ANAGRAFICAWEB.ConstSession.Path_TARSU & "/Dichiarazioni/RicercaDichiarazione.aspx?IsFromVariabile=0&IdEnte=" & ANAGRAFICAWEB.ConstSession.IdEnte & "&Cognome=" & txtCognome.Text & "&Nome=" & txtNome.Text & "&CodiceFiscale=" & txtCodiceFiscale.Text & "&PartitaIVA=" & txtPartitaIva.Text & "';"
                                sScript += "parent.Comandi.location.href='.." & ANAGRAFICAWEB.ConstSession.Path_TARSU & "/Dichiarazioni/ComandiRicDichiarazione.aspx?IsFromVariabile=0';"
                                sScript += "parent.Basso.location.href='../aspVuota.aspx';"
                                sScript += "parent.Nascosto.location.href='../aspSvuota.aspx';"
                            Case Utility.Costanti.TRIBUTO_OSAP
                                sScript = "parent.Visualizza.location.href='.." & ANAGRAFICAWEB.ConstSession.Path_OSAPSCUOLE & "/Dichiarazioni/NewSearch=false&DichiarazioniSearch.aspx?IdEnte=" & ANAGRAFICAWEB.ConstSession.IdEnte & "&Cognome=" & txtCognome.Text & "&Nome=" & txtNome.Text & "&CodiceFiscale=" & txtCodiceFiscale.Text & "&PartitaIVA=" & txtPartitaIva.Text & "';"
                                sScript += "parent.Comandi.location.href='../aspVuotaRemoveComandi.aspx';"
                                sScript += "parent.Basso.location.href='../aspVuotaRemoveComandi.aspx';"
                                sScript += "parent.Nascosto.location.href='../aspSvuota.aspx';"
                            Case Utility.Costanti.TRIBUTO_SCUOLE
                                sScript = "parent.Visualizza.location.href='.." & ANAGRAFICAWEB.ConstSession.Path_OSAPSCUOLE & "/SituazioneContribuente/SituazioneAvvisiSearch.aspx?NewSearch=false&CodTributo=9253&IdEnte=" & ANAGRAFICAWEB.ConstSession.IdEnte & "&Cognome=" & txtCognome.Text & "&Nome=" & txtNome.Text & "&CodiceFiscale=" & txtCodiceFiscale.Text & "&PartitaIVA=" & txtPartitaIva.Text & "';"
                                sScript += "parent.Comandi.location.href='../aspVuotaRemoveComandi.aspx';"
                                sScript += "parent.Basso.location.href='../aspVuotaRemoveComandi.aspx';"
                                sScript += "parent.Nascosto.location.href='../aspSvuota.aspx';"
                            Case Utility.Costanti.TRIBUTO_H2O
                            Case "9999" 'provvedimenti
                                sScript = "parent.Visualizza.location.href='.." & ANAGRAFICAWEB.ConstSession.Path_Provvedimenti & "/GestioneAtti/RicercaSemplice/RicercaSemplice.aspx?IdEnte=" & ANAGRAFICAWEB.ConstSession.IdEnte & "&Cognome=" & txtCognome.Text & "&Nome=" & txtNome.Text & "&CodiceFiscale=" & txtCodiceFiscale.Text & "&PartitaIVA=" & txtPartitaIva.Text & "';"
                                sScript += "parent.Comandi.location.href='../aspVuota.aspx';"
                                sScript += "parent.Basso.location.href='../aspVuota.aspx';"
                                sScript += "parent.Nascosto.location.href='../aspSvuota.aspx';"
                            Case Else
                        End Select
                    Else
                        sScript = "GestAlert('a', 'warning', '', '', 'Impossibile entrare nel dettaglio dalle funzioni sovracomunali.');"
                    End If
                    RegisterScript(sScript, Me.GetType())
                End If
            Catch ex As Exception
                Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.FormAnagrafica.GrdRowCommand.errore: ", ex)
                Response.Redirect("../PaginaErrore.aspx")
            End Try
        End Sub
#End Region
    End Class
End Namespace
