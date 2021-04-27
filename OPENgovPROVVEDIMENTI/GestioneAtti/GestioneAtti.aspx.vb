Imports System.Configuration
Imports System.Runtime.Remoting
Imports System.Runtime.Remoting.Channels
Imports System.Runtime.Remoting.Channels.Http
Imports System.IO
Imports System.Xml
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters
Imports System.Collections
Imports ComPlusInterface
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
Imports System.Net
Imports log4net
Imports Utility
''' <summary>
''' Pagina per la gestione del provvedimento.
''' Contiene i parametri di gestione e le funzioni della comandiera. 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
''' <revisionHistory>
''' <revision date="26/11/2018">
''' <strong>Insoluti e Coattivo</strong>
''' Dalla funzione sarà possibile agire solo sugli atti confermati e quindi non saranno visibili gli atti “IN ATTESA”.
''' </revision>
''' </revisionHistory>
''' <revisionHistory>
''' <revision date="12/04/2019">
''' <strong>Qualificazione AgID-analisi_rel01</strong>
''' <em>Analisi eventi</em>
''' </revision>
''' </revisionHistory>
Partial Class GestioneAtti
    Inherits BasePage
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(GestioneAtti))
    Const LIQUIDAZIONE As String = "L"
    Const ACCERTAMENTO As String = "A"
    Const QUESTIONARIO As String = "Q"

    Private objDS As DataSet
    Private objDSRate As DataSet
    Private objDSVers As DataSet
    Public IDTIPOPROVVEDIMENTO As String
    Public bRateizzato As Boolean
    Private objUtility As New MyUtility
    Private FncGestAccert As New ClsGestioneAccertamenti
    Protected FncForGrd As New Formatta.SharedGrd
    Protected FncGrd As New Formatta.FunctionGrd
    Private Shared myAtto As OggettoAtto

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents lblTotDich1 As System.Web.UI.WebControls.Label
    'Protected WithEvents lblTotVers As System.Web.UI.WebControls.Label
    'Protected WithEvents lblDIF2 As System.Web.UI.WebControls.Label
    'Protected WithEvents lblSANZF2 As System.Web.UI.WebControls.Label
    'Protected WithEvents lblINTF2 As System.Web.UI.WebControls.Label
    'Protected WithEvents lblTOTF2 As System.Web.UI.WebControls.Label
    'Protected WithEvents lbltotImpICI As System.Web.UI.WebControls.Label
    'Protected WithEvents lblTotDich2 As System.Web.UI.WebControls.Label
    'Protected WithEvents lbltotDiffImp As System.Web.UI.WebControls.Label
    'Protected WithEvents lbltotImpSanz As System.Web.UI.WebControls.Label
    'Protected WithEvents lbltotImpSanzRid As System.Web.UI.WebControls.Label
    'Protected WithEvents lbltotInteressi As System.Web.UI.WebControls.Label
    'Protected WithEvents lbltotTotale As System.Web.UI.WebControls.Label
    'Protected WithEvents lblDIAVVISO As System.Web.UI.WebControls.Label
    'Protected WithEvents lblSANZAVVISO As System.Web.UI.WebControls.Label
    'Protected WithEvents lblSANZRIDOTTOAVVISO As System.Web.UI.WebControls.Label
    'Protected WithEvents lblINTAVVISO As System.Web.UI.WebControls.Label
    'Protected WithEvents lblTOTALEAVVISO As System.Web.UI.WebControls.Label
    'Protected WithEvents lblDIAVVISOTARSU As System.Web.UI.WebControls.Label
    'Protected WithEvents lblSANZAVVISOTARSU As System.Web.UI.WebControls.Label
    'Protected WithEvents lblSANZRIDOTTOAVVISOTARSU As System.Web.UI.WebControls.Label
    'Protected WithEvents lblINTAVVISOTARSU As System.Web.UI.WebControls.Label
    'Protected WithEvents lblTOTALEAVVISOTARSU As System.Web.UI.WebControls.Label
    'Protected WithEvents tblRiepTotaliICI As System.Web.UI.HtmlControls.HtmlTable
    'Protected WithEvents tblRiepTotaliTARSU As System.Web.UI.HtmlControls.HtmlTable
    Protected WithEvents lblError As System.Web.UI.WebControls.Label


    'Protected WithEvents ID_PROVVEDIMENTO As System.Web.UI.WebControls.TextBox

    Protected WithEvents lblCodiceCliente As System.Web.UI.WebControls.Label
    Protected WithEvents lblCodZus As System.Web.UI.WebControls.Label
    Protected WithEvents lblDel As System.Web.UI.WebControls.Label
    Protected WithEvents lblPeriodo As System.Web.UI.WebControls.Label
    Protected WithEvents lblNFattura As System.Web.UI.WebControls.Label
    Protected WithEvents lblDomiciliato As System.Web.UI.WebControls.Label
    Protected WithEvents Textbox19 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Textbox20 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Textbox5 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Textbox6 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Textbox2 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Textbox3 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Textbox8 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Textbox10 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Textbox11 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Textbox12 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Textbox13 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Textbox14 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Textbox16 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Textbox17 As System.Web.UI.WebControls.TextBox
    Protected WithEvents Textbox18 As System.Web.UI.WebControls.TextBox

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
    ''' <revisionHistory>
    ''' <revision date="25/01/2013">
    ''' inserimento gestione pagato anche rispetto all'importo pieno
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="07/2015">
    ''' GESTIONE INCROCIATA RIFIUTI/ICI E DIVERSA GESTIONE QUOTA VARIABILE
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Analisi eventi</em>
    ''' </revision>
    ''' </revisionHistory>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sScript As String = ""

        Try
            IDTIPOPROVVEDIMENTO = Request("IDTIPOPROVVEDIMENTO")
            txtIDTIPOPROVVEDIMENTO.Text = IDTIPOPROVVEDIMENTO

            If Not Page.IsPostBack Then
                If Not Request("PROVENIENZA") Is Nothing Then
                    Session("PROVENIENZA") = Request("PROVENIENZA")
                Else
                    Session("PROVENIENZA") = Nothing
                End If
                If Not Request("CODCONTRIBUENTE") Is Nothing Then
                    Session("COD_CONTRIBUENTE") = Request("CODCONTRIBUENTE")
                Else
                    Session("COD_CONTRIBUENTE") = Nothing
                End If
                If Not Request("ANNO") Is Nothing Then
                    Session("ANNO") = Request("ANNO")
                Else
                    Session("ANNO") = Nothing
                End If

                ViewState.Add("TIPO_PROVVEDIMENTO", Utility.StringOperation.FormatString(Request("TIPO_PROVVEDIMENTO")))
                LoadAtto(Utility.StringOperation.FormatInt(Request("IDPROVVEDIMENTO")))
            Else
                LoadRateizzato(Session("IDPROVVEDIMENTO"))
            End If

            ' CONTROLLO SE CI SONO DATARUOLOCOATTIVOICI E IMPORTORUOLOCOATTIVOICI E MODIFICO LA COMANDIERA
            If txtDataRuoloCoattivoICI.Text.Length > 0 And txtImportoRuoloCoattivoICI.Text <> 0 Then
                sScript += "parent.Comandi.location.href='ComandiGestioneAttiRuoloCoattivo.aspx';"
            End If
            sScript += "LIQUIDAZIONE.style.display='';"
            sScript += "DISABILITA_IMPORTI.style.display='';"
            sScript += "parent.Comandi.location.href='ComandiGestioneAttiAccertamenti.aspx';"

            sScript += "IMPORTI.style.display='';"
            If ConstSession.TributiBollettinoF24.IndexOf(txtCOD_TRIBUTO.Text) >= 0 Then
                hfBollettinoF24.Value = "1"
            Else
                hfBollettinoF24.Value = "0"
            End If
            'Visualizza Riepilogo
            lblViewDettaglio.Attributes.Add("onclick", "RiepilogoAtto()")
            '*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI & DIVERSA GESTIONE QUOTA VARIABILE***
            If ConstSession.HasPlainAnag Then
                sScript += "document.getElementById('TRSpecAnag').style.display='none';"
            Else
                sScript += "document.getElementById('TRPlainAnag').style.display='none';"
            End If
            RegisterScript(sScript, Me.GetType())
            '*** ***
            Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
            fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Provvedimenti, "Gestione", Utility.Costanti.AZIONE_LETTURA, Utility.Costanti.TRIBUTO_Accertamento, ConstSession.IdEnte, Utility.StringOperation.FormatString(Request("IDPROVVEDIMENTO")))
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    Dim objHashTable As Hashtable = New Hashtable
    '    ''Dim objSessione As CreateSessione
    '    'Dim strConnectionStringOPENgovProvvedimenti As String
    '    Dim strTIPO_PROCEDIMENTO, strWFErrore As String
    '    Dim dt As DataView
    '    'dim sScript as string=""
    '    'Dim strBuilderimporti As New System.Text.StringBuilder
    '    '*** 20130125 - inserimento gestione pagato anche rispetto all'importo pieno ***
    '    Dim dblImpTotAvvisoRidotto, dblImpTotAvvisoPieno As Double
    '    '*** ***
    '    Dim strConnectionStringOPENgovICI As String = ""

    '    Dim ID_PROVVEDIMENTO As Integer
    '    Dim sScript As String = ""

    '    'objSessione = New CreateSessione(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))
    '    'If Not objSessione.CreaSessione(ConstSession.UserName, strWFErrore) Then
    '    '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '    'End If

    '    Try
    '        objHashTable.Add("IDPROVVEDIMENTO", CType(Request("IDPROVVEDIMENTO"), String))
    '        objHashTable.Add("CODENTE", ConstSession.IdEnte)
    '        objHashTable.Add("CODTRIBUTO", Session("COD_TRIBUTO"))
    '        objHashTable.Add("PARAMETROORDINAMENTOGRIGLIA", "")

    '        objHashTable.Add("IDSOTTOAPPLICAZIONEANAGRAFICA", ConfigurationManager.AppSettings("OPENGOVA"))
    '        objHashTable.Add("IDSOTTOAPPLICAZIONETERRITORIO", ConfigurationManager.AppSettings("OPENGOVT"))
    '        objHashTable.Add("IDSOTTOAPPLICAZIONEUTILITA", ConfigurationManager.AppSettings("OPENGOVU"))
    '        objHashTable.Add("IDSOTTOAPPLICAZIONEICI", ConfigurationManager.AppSettings("OPENGOVI"))

    '        objHashTable.Add("CONNECTIONSTRINGOPENGOVICI", ConfigurationManager.AppSettings("connectionStringOpenGovICI"))

    '        Session("IDENTIFICATIVOAPPLICAZIONE") = "OPENGOVP"

    '        IDTIPOPROVVEDIMENTO = Request("IDTIPOPROVVEDIMENTO")
    '        txtIDTIPOPROVVEDIMENTO.Text = IDTIPOPROVVEDIMENTO

    '        ''strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString
    '        objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
    '        Dim objCOMRicerca As IElaborazioneAtti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAtti), ConstSession.URLServiziElaborazioneAtti)
    '        If Not Page.IsPostBack Then
    '            If Not Request("PROVENIENZA") Is Nothing Then
    '                Session("PROVENIENZA") = Request("PROVENIENZA")
    '            Else
    '                Session("PROVENIENZA") = Nothing
    '            End If
    '            If Not Request("CODCONTRIBUENTE") Is Nothing Then
    '                Session("COD_CONTRIBUENTE") = Request("CODCONTRIBUENTE")
    '            Else
    '                Session("COD_CONTRIBUENTE") = Nothing
    '            End If
    '            If Not Request("ANNO") Is Nothing Then
    '                Session("ANNO") = Request("ANNO")
    '            Else
    '                Session("ANNO") = Nothing
    '            End If

    '            ViewState.Add("TIPO_PROVVEDIMENTO", utility.stringoperation.formatstring(Request("TIPO_PROVVEDIMENTO")))

    '            objDS = objCOMRicerca.getDATI_PROVVEDIMENTI(objHashTable)

    '            Session.Remove("PROVVEDIMENTI_DA_STAMPARE")
    '            Session.Add("PROVVEDIMENTI_DA_STAMPARE", objDS)

    '            txtTipoTributo.Text = Replace(CType(Request("DESCTRIBUTO"), String), """", "'")
    '            txtNumeroProvvedimento.Text = CType(Request("NUMEROATTO"), String)
    '            txtTipoProvvedimento.Text = CType(Request("TIPOPROVVEDIMENTO"), String)
    '            txtAnno.Text = CType(Request("ANNO"), String)
    '            strTIPO_PROCEDIMENTO = utility.stringoperation.formatstring(Request("TIPOPROCEDIMENTO"))
    '            Session("TIPOPROCEDIMENTO") = utility.stringoperation.formatstring(Request("TIPOPROCEDIMENTO"))
    '            lblTributo.Text = Replace(utility.stringoperation.formatstring(Request("DESCTRIBUTO")), """", "'")
    '            lblAnnoAvviso.Text = utility.stringoperation.formatstring(Request("ANNO"))
    '            'lblNumeroAvviso.Text = utility.stringoperation.formatstring(Request("NUMERO_ATTO"))
    '            If strTIPO_PROCEDIMENTO.CompareTo(LIQUIDAZIONE) = 0 Then
    '                txtOPEN_RETTIFICA.Text = "1"
    '            End If
    '            If strTIPO_PROCEDIMENTO.CompareTo(ACCERTAMENTO) = 0 Then
    '                txtOPEN_RETTIFICA.Text = "2"
    '            End If

    '            'For intCount = 0 To objDS.Tables("PROVVEDIMENTO").Rows.Count - 1
    '            If objDS.Tables("PROVVEDIMENTO").Rows.Count = 1 Then
    '                'Dim rowPROVVEDIMENTO As DataRow = objDS.Tables("PROVVEDIMENTO").Rows(intCount)
    '                Dim rowPROVVEDIMENTO As DataRow = objDS.Tables("PROVVEDIMENTO").Rows(0)

    '                objHashTable.Add("NUMERO_ATTO", utility.stringoperation.formatstring(rowPROVVEDIMENTO("NUMERO_ATTO")))
    '                objHashTable.Add("ANNO_AVVISO", utility.stringoperation.formatstring(Request("ANNO")))
    '                objHashTable.Add("COD_CONTRIBUENTE", utility.stringoperation.formatstring(rowPROVVEDIMENTO("COD_CONTRIBUENTE")))

    '                hdIdContribuente.Value = utility.stringoperation.formatstring(rowPROVVEDIMENTO("COD_CONTRIBUENTE"))
    '                ID_PROVVEDIMENTO = rowPROVVEDIMENTO("ID_PROVVEDIMENTO")
    '                Session("IDPROVVEDIMENTO") = ID_PROVVEDIMENTO
    '                hfidprovvedimento.value = utility.stringoperation.formatstring(rowPROVVEDIMENTO("ID_PROVVEDIMENTO"))
    '                txtCOD_TRIBUTO.Text = utility.stringoperation.formatstring(rowPROVVEDIMENTO("COD_TRIBUTO"))

    '                txtANNO_RETTIFICA.Text = txtAnno.Text
    '                lblNumeroAvviso.Text = utility.stringoperation.formatstring(rowPROVVEDIMENTO("NUMERO_ATTO"))
    '                txtTIPO_OPERAZIONE.Text = "RETTIFICA"
    '                TxtNomePDF.Text = utility.stringoperation.formatstring(rowPROVVEDIMENTO("nomepdf"))
    '                txtTIPO_PROCEDIMENTO.Text = utility.stringoperation.formatstring(Request("TIPOPROCEDIMENTO")).ToUpper

    '                '*** gestione anagrafica ***
    '                If LoadAnagrafica(rowPROVVEDIMENTO, strWFErrore) = False Then
    '                    Log.Debug("Errore in carico anagrafe:: " & strWFErrore)
    '                End If
    '                '*** ***
    '                '*** gestione importi ***
    '                If LoadImporti(rowPROVVEDIMENTO, dblImpTotAvvisoRidotto, dblImpTotAvvisoPieno, strWFErrore) = False Then
    '                    Log.Debug("Errore in carico importi:: " & strWFErrore)
    '                End If
    '                '*** ***
    '                '*** gestione date ***
    '                If LoadDateNote(rowPROVVEDIMENTO, strWFErrore) = False Then
    '                    Log.Debug("Errore in carico date:: " & strWFErrore)
    '                End If
    '                '*** ***
    '                '**** gestione pagamenti ****
    '                If LoadPagamenti(rowPROVVEDIMENTO, dblImpTotAvvisoRidotto, dblImpTotAvvisoPieno, strWFErrore) = False Then
    '                    Log.Debug("Errore in carico pagamenti:: " & strWFErrore)
    '                End If
    '                'objRicercaVers = Nothing
    '                '**** ****
    '                '*** gestione dettaglio atto ***
    '                Select Case txtCOD_TRIBUTO.Text
    '                    Case Utility.Costanti.TRIBUTO_ICI, Utility.Costanti.TRIBUTO_TASI
    '                        If LoadDettaglioICI(rowPROVVEDIMENTO, strWFErrore) = False Then
    '                            Log.Debug("Errore in carico dettaglio:: " & strWFErrore)
    '                        End If
    '                    Case Utility.Costanti.TRIBUTO_TARSU
    '                        If LoadDettaglioTARSU(rowPROVVEDIMENTO, strWFErrore) = False Then
    '                            Log.Debug("Errore in carico dettaglio:: " & strWFErrore)
    '                        End If
    '                    Case Utility.Costanti.TRIBUTO_OSAP
    '                        If LoadDettaglioOSAP(rowPROVVEDIMENTO, strWFErrore) = False Then
    '                            Log.Debug("Errore in carico dettaglio:: " & strWFErrore)
    '                        End If
    '                End Select
    '                'If Session("COD_TRIBUTO") = "8852" Then
    '                '    If LoadDettaglioICI(rowPROVVEDIMENTO, strWFErrore) = False Then
    '                '        Throw New Exception("Errore in carico dettaglio:: " & strWFErrore)
    '                '    End If
    '                'ElseIf Session("COD_TRIBUTO") = "0434" Then
    '                '    If LoadDettaglioTARSU(rowPROVVEDIMENTO, strWFErrore) = False Then
    '                '        Throw New Exception("Errore in carico dettaglio:: " & strWFErrore)
    '                '    End If
    '                'ElseIf Session("COD_TRIBUTO") = Costanti.TRIBUTO_OSAP Then
    '                '    If LoadDettaglioOSAP(rowPROVVEDIMENTO, strWFErrore) = False Then
    '                '        Throw New Exception("Errore in carico dettaglio:: " & strWFErrore)
    '                '    End If
    '                'End If
    '                '*** ***
    '                '*** Gestione Riepilogo atto ***
    '                If LoadRiepilogoImporti(rowPROVVEDIMENTO, strWFErrore) = False Then
    '                    Log.Debug("Errore in carico riepilogo:: " & strWFErrore)
    '                End If
    '                '*** ***

    '                'objDSRate = objCOMRicerca.getRateProvvedimenti(objHashTable, objHashTable("IDPROVVEDIMENTO"))
    '                '*** 20130125 - inserimento gestione pagato anche rispetto all'importo pieno ***
    '                LoadRateizzato(hfidprovvedimento.value)
    '                '*** ***
    '                '*** per la rettifica ***
    '                Dim myHasTbl As New Hashtable
    '                If myHasTbl.ContainsKey("DATA_ELABORAZIONE_PER_RETTIFICA") Then
    '                    myHasTbl.Remove("DATA_ELABORAZIONE_PER_RETTIFICA")
    '                    myHasTbl.Add("DATA_ELABORAZIONE_PER_RETTIFICA", objUtility.GiraDataFromDB(Session("DATA_ELABORAZIONE_PER_RETTIFICA")))
    '                Else
    '                    myHasTbl.Add("DATA_ELABORAZIONE_PER_RETTIFICA", objUtility.GiraDataFromDB(Session("DATA_ELABORAZIONE_PER_RETTIFICA")))
    '                End If
    '                myHasTbl.Add("DATA_ELABORAZIONE", objUtility.GiraData(txtDATA_ELABORAZIONE.Text))
    '                myHasTbl.Add("COD_TRIBUTO", txtCOD_TRIBUTO.Text)
    '                myHasTbl.Add("ID_PROVVEDIMENTO_RETTIFICA", hfidprovvedimento.value)
    '                myHasTbl.Add("DATA_RETTIFICA", DateTime.Now.ToString("yyyyMMdd"))
    '                myHasTbl.Add("TIPO_OPERAZIONE_RETTIFICA", True)
    '                myHasTbl.Add("CODCONTRIBUENTE", hdIdContribuente.Value)
    '                myHasTbl.Add("ANNOACCERTAMENTO", txtANNO_RETTIFICA.Text)
    '                Session("HashTableRettificaAccertamenti") = myHasTbl
    '                '*** ***
    '            End If
    '            'Next

    '            '**** gestione griglia pagamanti ****
    '            'Dim objRicercaVers As IElaborazioneAtti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAtti), ConstSession.URLServiziElaborazioneAtti)
    '            'If Session("COD_TRIBUTO") = "8852" And objHashTable("NUMERO_ATTO") <> "" Then
    '            '    objDSVers = objRicercaVers.getVersamentiICI(objHashTable)
    '            'End If
    '            'If Not objDSVers Is Nothing Then
    '            '    dtVers = objDSVers.Tables(0).DefaultView
    '            'End If
    '            'grdPagamenti.start_index = 0
    '            'grdPagamenti.DataSource = dtVers
    '            'grdPagamenti.DataBind()
    '            '**** gestione griglia pagamanti ****

    '            If Not objDS Is Nothing Then
    '                'viewstate("SortKey") = "ANNO"
    '                ' viewstate("OrderBy") = "ASC"
    '                dt = objDS.Tables("PROVVEDIMENTO").DefaultView
    '                ' dt.Sort = viewstate("SortKey") & " " & viewstate("OrderBy")
    '            End If
    '            GrdDettaglioAvviso.DataSource = dt
    '            GrdDettaglioAvviso.DataBind()
    '        Else
    '            'objDSRate = objCOMRicerca.getRateProvvedimenti(objHashTable, Session("IDPROVVEDIMENTO"))
    '            '*** 20130125 - inserimento gestione pagato anche rispetto all'importo pieno ***
    '            LoadRateizzato(Session("IDPROVVEDIMENTO"))
    '            '*** ***
    '        End If

    '        ' CONTROLLO SE CI SONO DATARUOLOCOATTIVOICI E IMPORTORUOLOCOATTIVOICI E MODIFICO LA COMANDIERA
    '        If txtDataRuoloCoattivoICI.Text.Length > 0 And txtImportoRuoloCoattivoICI.Text <> 0 Then
    '            sScript += "parent.Comandi.location.href='ComandiGestioneAttiRuoloCoattivo.aspx';"
    '        End If

    '        'Select Case utility.stringoperation.formatstring(Request("TIPOPROCEDIMENTO")).ToUpper
    '        '    Case "Q"
    '        '        sScript += "QUESTIONARIO.style.display='';"
    '        '        GrdDettaglioAvviso.Visible = False
    '        '        'If utility.stringoperation.formatstring(Request("PAGINAPRECEDENTE")).CompareTo("AVANZATA") = 0 Then
    '        '        '    sscript+= "parent.Comandi.location.href='ComandiGestioneAttiQuestionariAvanzata.aspx';"
    '        '        'End If
    '        '        'If utility.stringoperation.formatstring(Request("PAGINAPRECEDENTE")).CompareTo("SEMPLICE") = 0 Then
    '        '        sScript += "parent.Comandi.location.href='ComandiGestioneAttiQuestionari.aspx';"
    '        '        'End If

    '        '        If utility.stringoperation.formatstring(Request("PAGINAPRECEDENTE")).CompareTo("STATO") = 0 Then
    '        '            Dim strTipoProvvedimento As String
    '        '            Dim strUTENTE As String
    '        '            Dim strPARAMETRI As String

    '        '            strTipoProvvedimento = utility.stringoperation.formatstring(Request("TIPO_PROVVEDIMENTO"))

    '        '            strUTENTE = utility.stringoperation.formatstring(Request("UTENTE"))
    '        '            strPARAMETRI = "?TIPO_PROVVEDIMENTO=" & strTipoProvvedimento & "&UTENTE=" & strUTENTE & "&NOMEPAGINA=" & "StatoElaborazioneQuestionari.aspx"

    '        '            sScript += "parent.Comandi.location.href='ComandiGestioneAttiQuestionariStato.aspx" & strPARAMETRI & "';"
    '        '        End If
    '        '    Case "L"
    '        '        sScript += "LIQUIDAZIONE.style.display='';"
    '        '        sScript += "DISABILITA_IMPORTI.style.display='';"

    '        '        'If utility.stringoperation.formatstring(Request("PAGINAPRECEDENTE")).CompareTo("AVANZATA") = 0 Then
    '        '        '    sscript+= "parent.Comandi.location.href='ComandiGestioneAttiLiquidazioniAvanzata.aspx';"
    '        '        'End If
    '        '        'If utility.stringoperation.formatstring(Request("PAGINAPRECEDENTE")).CompareTo("SEMPLICE") = 0 Then
    '        '        sScript += "parent.Comandi.location.href='ComandiGestioneAttiLiquidazioni.aspx';"
    '        '        'End If

    '        '        If utility.stringoperation.formatstring(Request("PAGINAPRECEDENTE")).CompareTo("STATO") = 0 Then

    '        '            Dim strTipoProvvedimento As String
    '        '            Dim strUTENTE As String
    '        '            Dim strPARAMETRI As String

    '        '            strTipoProvvedimento = utility.stringoperation.formatstring(Request("TIPO_PROVVEDIMENTO"))
    '        '            strUTENTE = utility.stringoperation.formatstring(Request("UTENTE"))
    '        '            strPARAMETRI = "?TIPO_PROVVEDIMENTO=" & strTipoProvvedimento & "&UTENTE=" & strUTENTE & "&NOMEPAGINA=" & "StatoElaborazioneLiquidazione.aspx"

    '        '            sScript += "parent.Comandi.location.href='ComandiGestioneAttiLiquidazioniStato.aspx" & strPARAMETRI & "';"
    '        '        End If
    '        '        sScript += "IMPORTI.style.display='';"
    '        '    Case "A"
    '        sScript += "LIQUIDAZIONE.style.display='';"
    '        sScript += "DISABILITA_IMPORTI.style.display='';"

    '        'If CType(Request("PAGINAPRECEDENTE"), String).CompareTo("AVANZATA") = 0 Then
    '        '    sscript+= "parent.Comandi.location.href='ComandiGestioneAttiAccertamentiAvanzata.aspx';"
    '        'End If
    '        'If utility.stringoperation.formatstring(Request("PAGINAPRECEDENTE")).CompareTo("SEMPLICE") = 0 Or utility.stringoperation.formatstring(Request("PAGINAPRECEDENTE")).CompareTo("INTERGEN") = 0 Then
    '        sScript += "parent.Comandi.location.href='ComandiGestioneAttiAccertamenti.aspx';"
    '        'End If

    '        If utility.stringoperation.formatstring(Request("PAGINAPRECEDENTE")).CompareTo("STATO") = 0 Then
    '            Dim strTipoProvvedimento As String
    '            Dim strUTENTE As String

    '            strTipoProvvedimento = utility.stringoperation.formatstring(Request("TIPO_PROVVEDIMENTO"))
    '            strUTENTE = utility.stringoperation.formatstring(Request("UTENTE"))
    '            'strPARAMETRI = "?TIPO_PROVVEDIMENTO=" & strTipoProvvedimento & "&UTENTE=" & strUTENTE & "&NOMEPAGINA=" & "StatoElaborazioneQuestionari.aspx"
    '            'sscript+= "parent.Comandi.location.href='ComandiGestioneAttiAccertamentiStato.aspx" & strPARAMETRI & "'" & vbCrLf
    '            'RegisterScript(sScript , Me.GetType())
    '        End If
    '        sScript += "IMPORTI.style.display='';"
    '        'End Select
    '        If ConstSession.TributiBollettinoF24.IndexOf(txtCOD_TRIBUTO.Text) >= 0 Then
    '            hfBollettinoF24.Value = "1"
    '        Else
    '            hfBollettinoF24.Value = "0"
    '        End If
    '        'Visualizza Riepilogo
    '        lblViewDettaglio.Attributes.Add("onclick", "RiepilogoAtto()")
    '        '*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI & DIVERSA GESTIONE QUOTA VARIABILE***
    '        If ConstSession.HasPlainAnag Then
    '            sScript += "document.getElementById('TRSpecAnag').style.display='none';"
    '        Else
    '            sScript += "document.getElementById('TRPlainAnag').style.display='none';"
    '        End If
    '        RegisterScript(sScript, Me.GetType())
    '        '*** ***
    '    Catch ex As Exception
    '        'If Not IsNothing(objSessione) Then
    '        '    objSessione.Kill()
    '        '    objSessione = Nothing
    '        'End If
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.Page_Load.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    Finally
    '        'If Not IsNothing(objSessione) Then
    '        '    objSessione.Kill()
    '        '    objSessione = Nothing
    '        'End If
    '    End Try
    'End Sub

    '*** 20130125 - inserimento gestione pagato anche rispetto all'importo pieno ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nIdProvvedimento"></param>
    Private Sub LoadRateizzato(ByVal nIdProvvedimento As Integer)
        Try
            Dim FncPag As New clsPagamenti(ConstSession.StringConnection) 'clsPagamenti(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))
            '*** 20130125 - inserimento gestione pagato anche rispetto all'importo pieno ***
            objDSRate = FncPag.getRateProvvedimento(nIdProvvedimento, ConstSession.IdEnte, True)
            '*** ***
            If objDSRate.Tables.Count > 0 Then
                If objDSRate.Tables(0).Rows.Count > 0 Then
                    lblRateizzato.Text = "Il pagamento &egrave; stato rateizzato in " & objDSRate.Tables(0).Rows.Count() & " Rate"
                    lblRateizzato.Visible = True
                    Session("bRateizzato") = True
                Else
                    lblRateizzato.Visible = False
                    Session("bRateizzato") = False
                End If
            Else
                lblRateizzato.Visible = False
                Session("bRateizzato") = False
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.LoadRateizzato.errore: ", ex)
            'Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** ***
    ''' <summary>
    ''' Salvataggio Accertamenti
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Analisi eventi</em>
    ''' </revision>
    ''' </revisionHistory>
    Private Sub btnSalvaLiquidazioni_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalvaLiquidazioni.Click
        Dim objHashTable As Hashtable = New Hashtable
        Dim objUtility As New MyUtility
        Dim myResult As Integer = -1
        Dim flagAccertamento, flagConciliazioneG As Integer
        Dim sLogAtto As String = ""

        Try
            If ckAccertamento.Checked = False Then
                flagAccertamento = 0
            Else
                flagAccertamento = 1
            End If

            If ckConcGiudiz.Checked = False Then
                flagConciliazioneG = 0
            Else
                flagConciliazioneG = 1
            End If

            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
            objHashTable.Add("CONNECTIONSTRINGOPENGOVICI", ConstSession.StringConnectionICI)
            objHashTable.Add("IDPROVVEDIMENTO", hfIdProvvedimento.Value)
            Try
                For Each p As System.Reflection.PropertyInfo In myAtto.GetType().GetProperties()
                    If p.CanRead Then
                        If sLogAtto <> "" Then
                            sLogAtto += ","
                        End If
                        sLogAtto += p.Name + "->" + Utility.StringOperation.FormatString(p.GetValue(myAtto, Nothing))
                    End If
                Next
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.btnSalvaLiquidazioni.logattodb.errore: ", ex)
            End Try
            Log.Debug("GestioneAtti.btnSalvaLiquidazioni_Click.atto visualizzato=" + sLogAtto)
            'controllo che il numero avviso a video corrisponda a quello presente nell'oggetto
            If myAtto.NUMERO_ATTO <> lblNumeroAvviso.Text Then
                Log.Debug("GestioneAtti.btnSalvaLiquidazioni_Click.Numero Atto non coerente! Uscire e rientrare dalla videata (" + myAtto.NUMERO_ATTO + "-" + lblNumeroAvviso.Text + ")")
                Dim sScript As String = ""
                sScript = "GestAlert('a', 'warning', '', '', 'Numero Atto non coerente! Uscire e rientrare dalla videata.');"
                RegisterScript(sScript, Me.GetType())
            Else
                myAtto.NUMERO_ATTO = lblNumeroAvviso.Text
                myAtto.DATA_CONFERMA = objUtility.GiraData(txtDataConfermaAvviso.Text)
                myAtto.DATA_STAMPA = objUtility.GiraData(txtDataStampaAvviso.Text)

                myAtto.DATA_CONSEGNA_AVVISO = objUtility.GiraData(txtDataConsegnaAvviso.Text)
                myAtto.DATA_NOTIFICA_AVVISO = objUtility.GiraData(txtDataNotificaAvviso.Text)
                myAtto.DATA_SOSPENSIONE_AVVISO_AUTOTUTELA = objUtility.GiraData(txtDataSospensioneAvvisoAutotutela.Text)
                myAtto.DATA_PRESENTAZIONE_RICORSO = objUtility.GiraData(txtDataRicorsoProvinciale.Text)
                myAtto.DATA_SOSPENSIONE_DA_COMMISSIONE_TRIBUTARIA = objUtility.GiraData(txtSospensioneProvinciale.Text)
                myAtto.DATA_SENTENZA = objUtility.GiraData(txtSentenzaProvinciale.Text)
                myAtto.NOTE_PROVINCIALE = txtNoteProvinciale.Text
                myAtto.FLAG_CONCILIAZIONE_G = flagConciliazioneG
                myAtto.NOTE_CONCILIAZIONE_G = txtNoteConcGiudiz.Text
                myAtto.DATA_PRESENTAZIONE_RICORSO_REGIONALE = objUtility.GiraData(txtDataRicorsoRegionale.Text)
                myAtto.DATA_SOSPENSIONE_DA_COMMISSIONE_TRIBUTARIA_REGIONALE = objUtility.GiraData(txtSospensioneRegionale.Text)
                myAtto.DATA_SENTENZA_REGIONALE = objUtility.GiraData(txtSentenzaRegionale.Text)
                myAtto.NOTE_REGIONALE = txtNoteRegionale.Text
                myAtto.DATA_PRESENTAZIONE_RICORSO_CASSAZIONE = objUtility.GiraData(txtDataRicorsoCassazione.Text)
                myAtto.DATA_SOSPENSIONE_DA_COMMISSIONE_TRIBUTARIA_CASSAZIONE = objUtility.GiraData(txtSospensioneCassazione.Text)
                myAtto.DATA_SENTENZA_CASSAZIONE = objUtility.GiraData(txtSentenzaCassazione.Text)
                myAtto.NOTE_CASSAZIONE = txtNoteCassazione.Text
                myAtto.DATA_VERSAMENTO_SOLUZIONE_UNICA = objUtility.GiraData(txtDataVersamentoUnicaSoluzione.Text)
                myAtto.DATA_CONCESSIONE_RATEIZZAZIONE = objUtility.GiraData(txtDataConcessioneRateizzazione.Text)
                myAtto.FLAG_ACCERTAMENTO = flagAccertamento
                myAtto.ESITO_ACCERTAMENTO = ddlEsitoAccertamenti.SelectedValue
                myAtto.TERMINE_RICORSO_ACC = objUtility.GiraData(txtTermineRicorso.Text)
                myAtto.NOTE_ACCERTAMENTO = txtNoteAccertamenti.Text
                myAtto.NOTE_GENERALI_ATTO = txtNoteGenerali.Text
                myAtto.DATA_COATTIVO = objUtility.GiraData(txtDataRuoloCoattivoICI.Text)
                myAtto.DATA_IRREPERIBILE = objUtility.GiraData(txtDataIrreperibile.Text)
                Try
                    For Each p As System.Reflection.PropertyInfo In myAtto.GetType().GetProperties()
                        If p.CanRead Then
                            If sLogAtto <> "" Then
                                sLogAtto += ","
                            End If
                            sLogAtto += p.Name + "->" + Utility.StringOperation.FormatString(p.GetValue(myAtto, Nothing))
                        End If
                    Next
                    sLogAtto += ",operatore->" + ConstSession.UserName
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.btnSalvaLiquidazioni.logattovideo.errore: ", ex)
                End Try
                Log.Debug("GestioneAtti.btnSalvaLiquidazioni_Click.atto da salvare=" + sLogAtto)
                myResult = New DBPROVVEDIMENTI.ProvvedimentiDB().SetPROVVEDIMENTOATTO_LIQUIDAZIONE(myAtto)
                If myResult <= 0 Then
                    Throw New Exception("GestioneAtti.btnSalvaLiquidazioni_Click.Errore durante l'aggiornamento Atti")
                Else
                    LoadAtto(myResult)
                End If
                Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Provvedimenti, "btnSalvaLiquidazioni", Utility.Costanti.AZIONE_UPDATE, Utility.Costanti.TRIBUTO_Accertamento, ConstSession.IdEnte, myAtto.ID_PROVVEDIMENTO)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.btnSalvaLiquidazioni.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub btnSalvaLiquidazioni_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalvaLiquidazioni.Click
    '    Dim objHashTable As Hashtable = New Hashtable
    '    Dim objUtility As New MyUtility
    '    Dim blnResults As Boolean
    '    Dim flagAccertamento, flagConciliazioneG As Integer

    '    If ckAccertamento.Checked = False Then
    '        flagAccertamento = 0
    '    Else
    '        flagAccertamento = 1
    '    End If

    '    If ckConcGiudiz.Checked = False Then
    '        flagConciliazioneG = 0
    '    Else
    '        flagConciliazioneG = 1
    '    End If


    '    objHashTable.Add("IDSOTTOAPPLICAZIONEANAGRAFICA", ConfigurationManager.AppSettings("OPENGOVA"))
    '    objHashTable.Add("IDSOTTOAPPLICAZIONETERRITORIO", ConfigurationManager.AppSettings("OPENGOVT"))
    '    objHashTable.Add("IDSOTTOAPPLICAZIONEUTILITA", ConfigurationManager.AppSettings("OPENGOVU"))
    '    objHashTable.Add("IDSOTTOAPPLICAZIONEICI", ConfigurationManager.AppSettings("OPENGOVI"))

    '    objHashTable.Add("IDPROVVEDIMENTO", CType(Request("IDPROVVEDIMENTO"), String))
    '    objHashTable.Add("DATACONSEGNAAVVISO", objUtility.GiraData(txtDataConsegnaAvviso.Text))
    '    objHashTable.Add("DATANOTIFICAAVVISO", objUtility.GiraData(txtDataNotificaAvviso.Text))
    '    objHashTable.Add("DATASOSPENSIONEAVVISOAUTOTUTELA", objUtility.GiraData(txtDataSospensioneAvvisoAutotutela.Text))

    '    objHashTable.Add("DATARICORSO", objUtility.GiraData(txtDataRicorsoProvinciale.Text))
    '    objHashTable.Add("DATACOMMESSIONETRIBUTARIA", objUtility.GiraData(txtSospensioneProvinciale.Text))
    '    objHashTable.Add("DATASENTENZA", objUtility.GiraData(txtSentenzaProvinciale.Text))
    '    objHashTable.Add("NOTEPROVINCIALE", utility.stringoperation.formatstring(txtNoteProvinciale.Text))

    '    objHashTable.Add("FLAGCONCILIAZIONEG", flagConciliazioneG)
    '    objHashTable.Add("NOTECONCILIAZIONEG", utility.stringoperation.formatstring(txtNoteConcGiudiz.Text))

    '    objHashTable.Add("DATARICORSOREGIONALE", objUtility.GiraData(txtDataRicorsoRegionale.Text))
    '    objHashTable.Add("DATACOMMESSIONETRIBUTARIAREGIONALE", objUtility.GiraData(txtSospensioneRegionale.Text))
    '    objHashTable.Add("DATASENTENZAREGIONALE", objUtility.GiraData(txtSentenzaRegionale.Text))
    '    objHashTable.Add("NOTEREGIONALE", utility.stringoperation.formatstring(txtNoteRegionale.Text))

    '    objHashTable.Add("DATARICORSOCASSAZIONE", objUtility.GiraData(txtDataRicorsoCassazione.Text))
    '    objHashTable.Add("DATACOMMESSIONETRIBUTARIACASSAZIONE", objUtility.GiraData(txtSospensioneCassazione.Text))
    '    objHashTable.Add("DATASENTENZACASSAZIONE", objUtility.GiraData(txtSentenzaCassazione.Text))
    '    objHashTable.Add("NOTECASSAZIONE", utility.stringoperation.formatstring(txtNoteCassazione.Text))

    '    objHashTable.Add("DATAVERSAMENTOUNICASOLUZIONE", objUtility.GiraData(txtDataVersamentoUnicaSoluzione.Text))
    '    objHashTable.Add("DATARATEIZZAZIONE", objUtility.GiraData(txtDataConcessioneRateizzazione.Text))

    '    objHashTable.Add("FLAGACCERTAMENTO", flagAccertamento)
    '    objHashTable.Add("ESITOACCERTAMENTO", ddlEsitoAccertamenti.SelectedValue)
    '    objHashTable.Add("TERMINERICORSOACC", utility.stringoperation.formatstring(txtTermineRicorso.Text))
    '    objHashTable.Add("NOTEACCERTAMENTO", utility.stringoperation.formatstring(txtNoteAccertamenti.Text))

    '    '*** Fabi
    '    objHashTable.Add("NOTEGENERALIATTO", utility.stringoperation.formatstring(txtNoteGenerali.Text))
    '    '*** /Fabi
    '    '*** 20171218 ***
    '    objHashTable.Add("DATACOATTIVO", objUtility.GiraData(txtDataRuoloCoattivoICI.Text))
    '    '*** ***
    '    'objSessione = New CreateSessione(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))

    '    'If Not objSessione.CreaSessione(ConstSession.UserName, strWFErrore) Then
    '    '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '    'End If

    '    Try

    '        'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString
    '        'strConnectionStringOPENgovICI = objSessione.oSession.GetPrivateDBManager(objHashTable("IDSOTTOAPPLICAZIONEICI")).GetConnection.ConnectionString

    '        objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
    '        objHashTable.Add("CONNECTIONSTRINGOPENGOVICI", ConstSession.StringConnectionICI)

    '        'blnOk = CheckDataDataConsegnaAvviso()
    '        'If Not blnOk Then
    '        '  strErrorMessase = "Per salvare la data di consegna avviso deve essere presente la Data di stampa!"
    '        'End If
    '        'If blnOk Then
    '        '  blnOk = CheckDataDataNotificaAvviso()

    '        '  If Not blnOk Then
    '        '    strErrorMessase = strErrorMessase & "Per salvare la data di notifica avviso deve essere presente la Data di consegna avviso!" & vbCrLf
    '        '  End If

    '        'End If

    '        'If blnOk Then
    '        '  blnOk = CheckDataDataRettificaAvviso()
    '        '  If Not blnOk Then
    '        '    strErrorMessase = strErrorMessase & "Per rettificare l'avviso la data di notifica avviso e la data di stamap deveno essere presenti!" & vbCrLf
    '        '  End If

    '        'End If

    '        'If blnOk Then
    '        '  blnOk = CheckDataRicorso2()
    '        '  If Not blnOk Then
    '        '    strErrorMessase = strErrorMessase & "Per salvare la data di rimborso Regionale deve essere presente la data di ricorso Provinciale !" & vbCrLf
    '        '  End If

    '        'End If


    '        'If blnOk Then
    '        '  blnOk = CheckDataRicorso3()
    '        '  If Not blnOk Then
    '        '    strErrorMessase = strErrorMessase & "Per salvare la data di rimborso Cassazione deve essere presente la data di ricorso Regionale !" & vbCrLf
    '        '  End If

    '        'End If

    '        'If Not blnOk Then
    '        'lblError.Text = strErrorMessase
    '        'Else
    '        Dim objCOMSETAtti As New DBPROVVEDIMENTI.ProvvedimentiDB

    '        blnResults = objCOMSETAtti.SetPROVVEDIMENTOATTO_LIQUIDAZIONE(objHashTable)

    '        If Not blnResults Then
    '            Throw New Exception("Page GestioneAtti::btnSalvaLiquidazioni_Click::Errore durante l'aggiornamento Atti")
    '        End If

    '        'Dim a As Object
    '        'Dim b As System.EventArgs
    '        'Page_Load(a, b)

    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.btnSalvaLiquidazioni.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '        Response.Write("<script language='javascript'>")
    '        Response.Write("parent.attesaGestioneAtti.style.display='none';")
    '        Response.Write("</script>")
    '    Finally
    '        'If Not IsNothing(objSessione) Then
    '        '    objSessione.Kill()
    '        '    objSessione = Nothing
    '        'End If
    '    End Try

    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Private Function CheckDataDataConsegnaAvviso() As Boolean
        Dim blnChekOk As Boolean = True
        Try
            'SOLO SE DATA DI STAMPA PRESENTE
            If txtDataConsegnaAvviso.Text.Length > 0 Then
                Select Case txtDataStampaAvviso.Text.Length
                    Case 0
                        blnChekOk = False
                End Select
            End If
            If Not blnChekOk Then
                Return False
            End If
            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.CheckDataDataConsegnaAvviso.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Private Function CheckDataDataNotificaAvviso() As Boolean
        Dim blnChekOk As Boolean = True
        Try
            If txtDataConsegnaAvviso.Text.Length = 0 Then
                Select Case txtDataNotificaAvviso.Text.Length
                    Case Is > 0
                        blnChekOk = False
                End Select
            End If

            If Not blnChekOk Then
                Return False
            End If
            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.CheckDataDataNotificaAvviso.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Private Function CheckDataDataRettificaAvviso() As Boolean
        Dim blnChekOk As Boolean = True
        Try
            If txtDataStampaAvviso.Text.Length = 0 And txtDataNotificaAvviso.Text.Length = 0 Then
                Select Case txtDataRettificaAvviso.Text.Length
                    Case Is > 0
                        blnChekOk = False
                End Select
            End If
            If Not blnChekOk Then
                Return False
            End If
            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.CheckDataDataRettificaAvviso.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Private Function CheckDataRicorso2() As Boolean
        Dim blnChekOk As Boolean = True
        Try
            If txtDataRicorsoProvinciale.Text.Length = 0 Then
                Select Case txtDataRicorsoRegionale.Text.Length
                    Case Is > 0
                        blnChekOk = False
                End Select
            End If
            If Not blnChekOk Then
                Return False
            End If
            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.CheckDataRicorso2.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Private Function CheckDataRicorso3() As Boolean
        Dim blnChekOk As Boolean = True
        Try
            If txtDataRicorsoRegionale.Text.Length = 0 Then
                Select Case txtDataRicorsoCassazione.Text.Length
                    Case Is > 0
                        blnChekOk = False
                End Select
            End If
            If Not blnChekOk Then
                Return False
            End If
            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.CheckDataRicorso3.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Private Sub btnStampa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStampa.Click
        Dim objHashTable As Hashtable = New Hashtable
        Dim objUtility As New MyUtility
        Dim blnResults As Boolean
        Dim sScript As String = ""
        Dim NUMERO_ATTO As String
        Dim annoElaborazioneDoc As Integer

        Try
            annoElaborazioneDoc = Date.Now.Year

            If txtDataStampa.Text.CompareTo("") = 0 Then

                If lblNumeroAvviso.Text <> "" Then
                    NUMERO_ATTO = lblNumeroAvviso.Text
                Else
                    NUMERO_ATTO = "-1"
                End If
                objHashTable.Add("IDPROVVEDIMENTO", hfIdProvvedimento.Value)
                objHashTable.Add("ANNO", CType(Request("ANNO"), String))
                objHashTable.Add("COD_ENTE", CType(ConstSession.IdEnte, String))
                objHashTable.Add("ANNOELABORAZIONE", CType(annoElaborazioneDoc, String))

                objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
                objHashTable.Add("CONNECTIONSTRINGOPENGOVICI", ConstSession.StringConnectionICI)

                Dim objCOMSETAtti As New DBPROVVEDIMENTI.ProvvedimentiDB
                blnResults = objCOMSETAtti.SetPROVVEDIMENTOATTO_LIQUIDAZIONE_STAMPA(ConstSession.StringConnection, objHashTable, NUMERO_ATTO)
                If Not blnResults Then
                    Throw New Exception("Page GestioneAtti::btnStampa_Click::Errore durante l'aggiornamento Atti")
                End If
                txtDataStampaAvviso.Text = objUtility.GiraDataFromDB(DateTime.Now.ToString("yyyyMMdd"))
                txtDataConfermaAvviso.Text = txtDataStampaAvviso.Text
                lblNumeroAvviso.Text = NUMERO_ATTO
            End If

            sScript += "ApriStampaLiquidazione();"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.btnStampa_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub btnStampa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStampa.Click
    '    Dim objHashTable As Hashtable = New Hashtable
    '    'Dim objSessione As CreateSessione
    '    'Dim remCanale As HttpChannel
    '    'Dim strConnectionStringOPENgovICI As String
    '    'Dim strConnectionStringOPENgovProvvedimenti As String
    '    'Dim strConnectionStringAnagrafica As String
    '    'Dim strConnectionStringOPENgovTerritorio As String
    '    'Dim strConnectionStringOPENgovUTILITA As String
    '    Dim objUtility As New MyUtility
    '    Dim blnResults As Boolean
    '    Dim sScript As String = ""
    '    Dim NUMERO_ATTO As String
    '    Dim annoElaborazioneDoc As Integer

    '    Try
    '        annoElaborazioneDoc = Date.Now.Year

    '        If txtDataStampa.Text.CompareTo("") = 0 Then

    '            If lblNumeroAvviso.Text <> "" Then
    '                NUMERO_ATTO = lblNumeroAvviso.Text
    '            Else
    '                NUMERO_ATTO = "-1"
    '            End If
    '            objHashTable.Add("IDSOTTOAPPLICAZIONEANAGRAFICA", ConfigurationManager.AppSettings("OPENGOVA"))
    '            objHashTable.Add("IDSOTTOAPPLICAZIONETERRITORIO", ConfigurationManager.AppSettings("OPENGOVT"))
    '            objHashTable.Add("IDSOTTOAPPLICAZIONEUTILITA", ConfigurationManager.AppSettings("OPENGOVU"))
    '            objHashTable.Add("IDSOTTOAPPLICAZIONEICI", ConfigurationManager.AppSettings("OPENGOVI"))

    '            objHashTable.Add("IDPROVVEDIMENTO", CType(Request("IDPROVVEDIMENTO"), String))
    '            objHashTable.Add("ANNO", CType(Request("ANNO"), String))
    '            objHashTable.Add("COD_ENTE", CType(ConstSession.IdEnte, String))
    '            objHashTable.Add("ANNOELABORAZIONE", CType(annoElaborazioneDoc, String))

    '            'objSessione = New CreateSessione(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))

    '            'If Not objSessione.CreaSessione(ConstSession.UserName, strWFErrore) Then
    '            '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '            'End If

    '            'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString
    '            'strConnectionStringOPENgovICI = objSessione.oSession.GetPrivateDBManager(objHashTable("IDSOTTOAPPLICAZIONEICI")).GetConnection.ConnectionString

    '            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
    '            objHashTable.Add("CONNECTIONSTRINGOPENGOVICI", ConstSession.StringConnectionICI)

    '            Dim objCOMSETAtti As New DBPROVVEDIMENTI.ProvvedimentiDB
    '            blnResults = objCOMSETAtti.SetPROVVEDIMENTOATTO_LIQUIDAZIONE_STAMPA(objHashTable, NUMERO_ATTO)
    '            If Not blnResults Then
    '                Throw New Exception("Page GestioneAtti::btnStampa_Click::Errore durante l'aggiornamento Atti")
    '            End If
    '            txtDataStampaAvviso.Text = objUtility.GiraDataFromDB(DateTime.Now.ToString("yyyyMMdd"))
    '            txtDataConfermaAvviso.Text = txtDataStampaAvviso.Text
    '            lblNumeroAvviso.Text = NUMERO_ATTO
    '        End If

    '        sScript += "ApriStampaLiquidazione();"
    '        RegisterScript(sScript, Me.GetType())
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.btnStampa_Click.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '        Response.Write("<script language='javascript'>")
    '        Response.Write("parent.attesaGestioneAtti.style.display='none';")
    '        Response.Write("</script>")
    '    End Try
    'End Sub
    'Private Sub btnAnnullaAvviso_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAnnullaAvviso.Click
    '    Dim objHashTable As Hashtable = New Hashtable
    '    Dim objUtility As New MyUtility
    '    Dim blnResults As Boolean

    '    objHashTable.Add("IDSOTTOAPPLICAZIONEANAGRAFICA", ConfigurationManager.AppSettings("OPENGOVA"))
    '    objHashTable.Add("IDSOTTOAPPLICAZIONETERRITORIO", ConfigurationManager.AppSettings("OPENGOVT"))
    '    objHashTable.Add("IDSOTTOAPPLICAZIONEUTILITA", ConfigurationManager.AppSettings("OPENGOVU"))
    '    objHashTable.Add("IDSOTTOAPPLICAZIONEICI", ConfigurationManager.AppSettings("OPENGOVI"))

    '    objHashTable.Add("IDPROVVEDIMENTO", CType(Request("IDPROVVEDIMENTO"), String))
    '    objHashTable.Add("DATACONSEGNAAVVISO", objUtility.GiraData(txtDataConsegnaAvviso.Text))
    '    objHashTable.Add("DATANOTIFICAAVVISO", objUtility.GiraData(txtDataNotificaAvviso.Text))
    '    objHashTable.Add("DATASOSPENSIONEAVVISOAUTOTUTELA", objUtility.GiraData(txtDataSospensioneAvvisoAutotutela.Text))

    '    objHashTable.Add("DATARICORSO", objUtility.GiraData(txtDataRicorsoProvinciale.Text))
    '    objHashTable.Add("DATACOMMESSIONETRIBUTARIA", objUtility.GiraData(txtSospensioneProvinciale.Text))
    '    objHashTable.Add("DATASENTENZA", objUtility.GiraData(txtSentenzaProvinciale.Text))

    '    objHashTable.Add("DATARICORSOREGIONALE", objUtility.GiraData(txtDataRicorsoRegionale.Text))
    '    objHashTable.Add("DATACOMMESSIONETRIBUTARIAREGIONALE", objUtility.GiraData(txtSospensioneRegionale.Text))
    '    objHashTable.Add("DATASENTENZAREGIONALE", objUtility.GiraData(txtSentenzaRegionale.Text))

    '    objHashTable.Add("DATARICORSOCASSAZIONE", objUtility.GiraData(txtDataRicorsoCassazione.Text))
    '    objHashTable.Add("DATACOMMESSIONETRIBUTARIACASSAZIONE", objUtility.GiraData(txtSospensioneCassazione.Text))
    '    objHashTable.Add("DATASENTENZACASSAZIONE", objUtility.GiraData(txtSentenzaCassazione.Text))

    '    objHashTable.Add("DATAVERSAMENTOUNICASOLUZIONE", objUtility.GiraData(txtDataVersamentoUnicaSoluzione.Text))
    '    objHashTable.Add("DATARATEIZZAZIONE", objUtility.GiraData(txtDataConcessioneRateizzazione.Text))

    '    'objSessione = New CreateSessione(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))

    '    'If Not objSessione.CreaSessione(ConstSession.UserName, strWFErrore) Then
    '    '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '    'End If

    '    Try

    '        'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString
    '        'strConnectionStringOPENgovICI = objSessione.oSession.GetPrivateDBManager(objHashTable("IDSOTTOAPPLICAZIONEICI")).GetConnection.ConnectionString

    '        objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
    '        objHashTable.Add("CONNECTIONSTRINGOPENGOVICI", ConstSession.StringConnectionICI)


    '        Dim objCOMSETAtti As New DBPROVVEDIMENTI.ProvvedimentiDB

    '        blnResults = objCOMSETAtti.SetPROVVEDIMENTOATTO_LIQUIDAZIONE(objHashTable)

    '        If Not blnResults Then
    '            Throw New Exception("Page GestioneAtti::btnAnnullaAvviso_Click::Errore durante l'aggiornamento Atti")
    '        End If
    '        blnResults = objCOMSETAtti.SetPROVVEDIMENTOATTO_ANNULAMENTO_AVVISO(objHashTable)

    '        If Not blnResults Then
    '            Throw New Exception("Page GestioneAtti::btnAnnullaAvviso_Click::Errore durante l'aggiornamento Atti::Data Annullamento")
    '        End If

    '        txtDataAnnullamentoAvviso.Text = objUtility.GiraDataFromDB(DateTime.Now.ToString("yyyyMMdd"))
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.btnAnnullaAvviso_Click.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '        Response.Write("<script language='javascript'>")
    '        Response.Write("parent.attesaGestioneAtti.style.display='none';")
    '        Response.Write("</script>")
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Private Sub btnStampaQuestionari_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStampaQuestionari.Click
        Dim objHashTable As Hashtable = New Hashtable
        Dim objUtility As New MyUtility
        Dim blnResults As Boolean
        Dim sScript As String = ""
        Dim NUMERO_ATTO As String
        Dim annoElaborazioneDoc As Integer

        Try
            If lblNumeroAvviso.Text <> "" Then
                NUMERO_ATTO = lblNumeroAvviso.Text
            Else
                NUMERO_ATTO = "-1"
            End If

            annoElaborazioneDoc = Date.Now.Year

            If txtDataStampa.Text.CompareTo("") = 0 Then
                objHashTable.Add("IDPROVVEDIMENTO", hfIdProvvedimento.Value)

                objHashTable.Add("ANNO", CType(Request("ANNO"), String))
                objHashTable.Add("COD_ENTE", CType(ConstSession.IdEnte, String))

                objHashTable.Add("ANNOELABORAZIONE", CType(annoElaborazioneDoc, String))

                objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
                objHashTable.Add("CONNECTIONSTRINGOPENGOVICI", ConstSession.StringConnectionICI)

                Dim objCOMSETAtti As New DBPROVVEDIMENTI.ProvvedimentiDB

                blnResults = objCOMSETAtti.SetPROVVEDIMENTOATTO_LIQUIDAZIONE_STAMPA(ConstSession.StringConnection, objHashTable, NUMERO_ATTO)

                If Not blnResults Then
                    Throw New Exception("Page GestioneAtti::btnStampa_Click::Errore durante l'aggiornamento Atti")
                End If

                txtDataStampa.Text = objUtility.GiraDataFromDB(DateTime.Now.ToString("yyyyMMdd"))
                'ID_PROVVEDIMENTO.Text = utility.stringoperation.formatstring(Request("IDPROVVEDIMENTO"))
                lblNumeroAvviso.Text = NUMERO_ATTO
            End If

            sScript += "ApriStampaQuestionario();"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.btnStampaQuestionari_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub btnStampaQuestionari_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStampaQuestionari.Click
    '    'Dim blnOk As Boolean
    '    Dim objHashTable As Hashtable = New Hashtable
    '    'Dim objSessione As CreateSessione
    '    'Dim remCanale As HttpChannel
    '    'Dim strConnectionStringOPENgovICI As String
    '    'Dim strConnectionStringOPENgovProvvedimenti As String
    '    'Dim strConnectionStringAnagrafica As String
    '    'Dim strConnectionStringOPENgovTerritorio As String
    '    'Dim strConnectionStringOPENgovUTILITA As String
    '    Dim objUtility As New MyUtility
    '    Dim blnResults As Boolean
    '    Dim sScript As String = ""
    '    Dim NUMERO_ATTO As String
    '    Dim annoElaborazioneDoc As Integer

    '    Try
    '        If lblNumeroAvviso.Text <> "" Then
    '            NUMERO_ATTO = lblNumeroAvviso.Text
    '        Else
    '            NUMERO_ATTO = "-1"
    '        End If

    '        annoElaborazioneDoc = Date.Now.Year

    '        If txtDataStampa.Text.CompareTo("") = 0 Then
    '            objHashTable.Add("IDSOTTOAPPLICAZIONEANAGRAFICA", ConfigurationManager.AppSettings("OPENGOVA"))
    '            objHashTable.Add("IDSOTTOAPPLICAZIONETERRITORIO", ConfigurationManager.AppSettings("OPENGOVT"))
    '            objHashTable.Add("IDSOTTOAPPLICAZIONEUTILITA", ConfigurationManager.AppSettings("OPENGOVU"))
    '            objHashTable.Add("IDSOTTOAPPLICAZIONEICI", ConfigurationManager.AppSettings("OPENGOVI"))

    '            objHashTable.Add("IDPROVVEDIMENTO", CType(Request("IDPROVVEDIMENTO"), String))

    '            objHashTable.Add("ANNO", CType(Request("ANNO"), String))
    '            objHashTable.Add("COD_ENTE", CType(ConstSession.IdEnte, String))

    '            objHashTable.Add("ANNOELABORAZIONE", CType(annoElaborazioneDoc, String))

    '            'objSessione = New CreateSessione(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))

    '            'If Not objSessione.CreaSessione(ConstSession.UserName, strWFErrore) Then
    '            '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '            'End If

    '            'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString
    '            'strConnectionStringOPENgovICI = objSessione.oSession.GetPrivateDBManager(objHashTable("IDSOTTOAPPLICAZIONEICI")).GetConnection.ConnectionString

    '            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
    '            objHashTable.Add("CONNECTIONSTRINGOPENGOVICI", ConstSession.StringConnectionICI)

    '            Dim objCOMSETAtti As New DBPROVVEDIMENTI.ProvvedimentiDB

    '            blnResults = objCOMSETAtti.SetPROVVEDIMENTOATTO_LIQUIDAZIONE_STAMPA(objHashTable, NUMERO_ATTO)

    '            If Not blnResults Then
    '                Throw New Exception("Page GestioneAtti::btnStampa_Click::Errore durante l'aggiornamento Atti")
    '            End If

    '            txtDataStampa.Text = objUtility.GiraDataFromDB(DateTime.Now.ToString("yyyyMMdd"))
    '            'ID_PROVVEDIMENTO.Text = utility.stringoperation.formatstring(Request("IDPROVVEDIMENTO"))
    '            lblNumeroAvviso.Text = NUMERO_ATTO
    '        End If

    '        sScript += "ApriStampaQuestionario();"
    '        RegisterScript(sScript, Me.GetType())
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.btnStampaQuestionari_Click.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '        Response.Write("<script language='javascript'>")
    '        Response.Write("parent.attesaGestioneAtti.style.display='none';")
    '        Response.Write("</script>")
    '    End Try
    'End Sub

    'Protected Function ConvertABS(ByVal prdStatus As Object) As String
    '    Dim objUtility As New myUtility
    '    Dim dblImpTotAvviso As Double

    '    dblImpTotAvviso = objUtility.cToDbl(prdStatus)
    'Try
    '    'If dblImpTotAvviso > 0 Then
    '    ConvertABS = utility.stringoperation.formatstring(FormatNumber(dblImpTotAvviso, 2))
    '    ' Else
    '    'ConvertABS = utility.stringoperation.formatstring(FormatNumber(Math.Abs(dblImpTotAvviso), 2))
    '    '  ConvertABS = utility.stringoperation.formatstring(FormatNumber(dblImpTotAvviso, 2))
    '    ' End If

    '    Return ConvertABS
    'Catch ex As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.ConvertABS.errore: ", ex)
    '   Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Function

    'Protected Function FormattaData(ByVal data As String) As String
    '    Dim dataFormattata As String = String.Empty

    '    dataFormattata = CDate(data).ToShortDateString

    '    Return dataFormattata

    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Private Sub btnStampaAccertamenti_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStampaAccertamenti.Click
        Dim objHashTable As Hashtable = New Hashtable
        Dim blnResults As Boolean
        Dim sScript As String = ""
        Dim NUMERO_ATTO As String
        Dim annoElaborazioneDoc As Integer

        Try
            If lblNumeroAvviso.Text <> "" Then
                NUMERO_ATTO = lblNumeroAvviso.Text
            Else
                NUMERO_ATTO = "-1"
            End If

            annoElaborazioneDoc = Date.Now.Year

            If txtDataStampa.Text.CompareTo("") = 0 Then
                objHashTable.Add("IDPROVVEDIMENTO", hfIdProvvedimento.Value)
                objHashTable.Add("ANNO", CType(Request("ANNO"), String))
                objHashTable.Add("COD_ENTE", CType(ConstSession.IdEnte, String))

                objHashTable.Add("ANNOELABORAZIONE", CType(annoElaborazioneDoc, String))
                objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
                objHashTable.Add("CONNECTIONSTRINGOPENGOVICI", ConstSession.StringConnectionICI)

                Dim objCOMSETAtti As New DBPROVVEDIMENTI.ProvvedimentiDB

                blnResults = objCOMSETAtti.SetPROVVEDIMENTOATTO_LIQUIDAZIONE_STAMPA(ConstSession.StringConnection, objHashTable, NUMERO_ATTO)
                If Not blnResults Then
                    Throw New Exception("Page GestioneAtti::btnStampa_Click::Errore durante l'aggiornamento Atti")
                End If

                txtDataStampaAvviso.Text = objUtility.GiraDataFromDB(DateTime.Now.ToString("yyyyMMdd"))
                txtDataConfermaAvviso.Text = txtDataStampaAvviso.Text
                lblNumeroAvviso.Text = NUMERO_ATTO

                txtDataStampa.Text = objUtility.GiraDataFromDB(DateTime.Now.ToString("yyyyMMdd"))
            End If

            Select Case IDTIPOPROVVEDIMENTO
                Case OggettoAtto.Provvedimento.Rimborso
                    'Rimborso
                    sScript += "ApriStampaAccertamento('" & CostantiProvv.DOCUMENTO_RIMBORSO_ICI & "','0');"
                Case OggettoAtto.Provvedimento.AutotutelaAnnullamento
                    'Annullamento
                    sScript += "ApriStampaAccertamento('" & CostantiProvv.DOCUMENTO_ANNULLAMENTO & "','0');"
                Case Else
                    'accertamento
                    sScript += "ApriStampaAccertamento('" & CostantiProvv.DOCUMENTO_ACCERTAMENTO & "','0');"
            End Select
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.btnStampaAccertamenti_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub btnStampaAccertamenti_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStampaAccertamenti.Click
    '    'Dim blnOk As Boolean
    '    Dim objHashTable As Hashtable = New Hashtable
    '    'Dim objSessione As CreateSessione
    '    'Dim remCanale As HttpChannel
    '    'Dim strConnectionStringOPENgovICI As String
    '    'Dim strConnectionStringOPENgovProvvedimenti As String
    '    'Dim strConnectionStringAnagrafica As String
    '    'Dim strConnectionStringOPENgovTerritorio As String
    '    'Dim strConnectionStringOPENgovUTILITA As String
    '    Dim objUtility As New MyUtility
    '    Dim blnResults As Boolean
    '    Dim sScript As String = ""
    '    Dim NUMERO_ATTO As String
    '    Dim annoElaborazioneDoc As Integer

    '    Try
    '        'ID_PROVVEDIMENTO.Text = utility.stringoperation.formatstring(Request("IDPROVVEDIMENTO"))

    '        If lblNumeroAvviso.Text <> "" Then
    '            NUMERO_ATTO = lblNumeroAvviso.Text
    '        Else
    '            NUMERO_ATTO = "-1"
    '        End If

    '        annoElaborazioneDoc = Date.Now.Year

    '        If txtDataStampa.Text.CompareTo("") = 0 Then
    '            objHashTable.Add("IDSOTTOAPPLICAZIONEANAGRAFICA", ConfigurationManager.AppSettings("OPENGOVA"))
    '            objHashTable.Add("IDSOTTOAPPLICAZIONETERRITORIO", ConfigurationManager.AppSettings("OPENGOVT"))
    '            objHashTable.Add("IDSOTTOAPPLICAZIONEUTILITA", ConfigurationManager.AppSettings("OPENGOVU"))
    '            objHashTable.Add("IDSOTTOAPPLICAZIONEICI", ConfigurationManager.AppSettings("OPENGOVI"))

    '            objHashTable.Add("IDPROVVEDIMENTO", CType(Request("IDPROVVEDIMENTO"), String))
    '            objHashTable.Add("ANNO", CType(Request("ANNO"), String))
    '            objHashTable.Add("COD_ENTE", CType(ConstSession.IdEnte, String))

    '            objHashTable.Add("ANNOELABORAZIONE", CType(annoElaborazioneDoc, String))

    '            'objSessione = New CreateSessione(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))

    '            'If Not objSessione.CreaSessione(ConstSession.UserName, strWFErrore) Then
    '            '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '            'End If

    '            'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString
    '            'strConnectionStringOPENgovICI = objSessione.oSession.GetPrivateDBManager(objHashTable("IDSOTTOAPPLICAZIONEICI")).GetConnection.ConnectionString

    '            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
    '            objHashTable.Add("CONNECTIONSTRINGOPENGOVICI", ConstSession.StringConnectionICI)

    '            Dim objCOMSETAtti As New DBPROVVEDIMENTI.ProvvedimentiDB

    '            blnResults = objCOMSETAtti.SetPROVVEDIMENTOATTO_LIQUIDAZIONE_STAMPA(objHashTable, NUMERO_ATTO)
    '            If Not blnResults Then
    '                Throw New Exception("Page GestioneAtti::btnStampa_Click::Errore durante l'aggiornamento Atti")
    '            End If

    '            txtDataStampaAvviso.Text = objUtility.GiraDataFromDB(DateTime.Now.ToString("yyyyMMdd"))
    '            txtDataConfermaAvviso.Text = txtDataStampaAvviso.Text
    '            lblNumeroAvviso.Text = NUMERO_ATTO

    '            txtDataStampa.Text = objUtility.GiraDataFromDB(DateTime.Now.ToString("yyyyMMdd"))
    '        End If

    '        Select Case IDTIPOPROVVEDIMENTO
    '            Case OggettoAtto.Provvedimento.Rimborso
    '                'Rimborso
    '                sScript += "ApriStampaAccertamento('" & CostantiProvv.DOCUMENTO_RIMBORSO_ICI & "','0');"
    '            Case OggettoAtto.Provvedimento.AutotutelaAnnullamento
    '                'Annullamento
    '                sScript += "ApriStampaAccertamento('" & CostantiProvv.DOCUMENTO_ANNULLAMENTO & "','0');"
    '            Case Else
    '                'accertamento
    '                sScript += "ApriStampaAccertamento('" & CostantiProvv.DOCUMENTO_ACCERTAMENTO & "','0');"
    '        End Select
    '        RegisterScript(sScript, Me.GetType())
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.btnStampaAccertamenti_Click.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '        Response.Write("<script language='javascript'>")
    '        Response.Write("parent.attesaGestioneAtti.style.display='none';")
    '        Response.Write("</script>")
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Private Sub btnRateizzazioni_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRateizzazioni.Click
        Dim objHashTable As Hashtable = New Hashtable
        Try
            objHashTable.Add("IDPROVVEDIMENTO", hfIdProvvedimento.Value)
            objHashTable.Add("TIPO_PROVVEDIMENTO", txtTipoProvvedimento.Text)
            objHashTable.Add("TIPO_TRIBUTO", txtTipoProvvedimento.Text)
            objHashTable.Add("ANNO", CType(Request("ANNO"), String))
            objHashTable.Add("NUMERO_ATTO", Utility.StringOperation.FormatString(Request("NUMEROATTO")))
            objHashTable.Add("IDTIPOPROVVEDIMENTO", CType(Request("IDTIPOPROVVEDIMENTO"), String))
            objHashTable.Add("DESCRTRIBUTO", Replace(CType(Request("DESCTRIBUTO"), String), """", "'"))
            objHashTable.Add("TIPOPROCEDIMENTO", Utility.StringOperation.FormatString(Request("TIPOPROCEDIMENTO")))
            objHashTable.Add("COD_CONTRIBUENTE", hdIdContribuente.Value)
            objHashTable.Add("DATA_ELABORAZIONE", txtDATA_ELABORAZIONE.Text)
            objHashTable.Add("CODENTE", ConstSession.IdEnte)
            objHashTable.Add("CODTRIBUTO", Session("COD_TRIBUTO"))
            objHashTable.Add("IMPORTO", Utility.StringOperation.FormatDouble(txtImporto.Text))

            CallPageaspx(objHashTable("IDPROVVEDIMENTO"), objHashTable("TIPO_PROVVEDIMENTO"), objHashTable("TIPO_TRIBUTO"),
            objHashTable("ANNO"), objHashTable("NUMERO_ATTO"), objHashTable("IDTIPOPROVVEDIMENTO"),
            objHashTable("DESCRTRIBUTO"), objHashTable("TIPOPROCEDIMENTO"), objHashTable("COD_CONTRIBUENTE"),
            objHashTable("DATA_ELABORAZIONE"), objHashTable("CODENTE"), objHashTable("CODTRIBUTO"), objHashTable("IMPORTO"))
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.btnRateizzazioni_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub btnRateizzazioni_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRateizzazioni.Click
    '    Dim objHashTable As Hashtable = New Hashtable
    '    Dim objUtility As New MyUtility
    '    Try
    '        objHashTable.Add("IDPROVVEDIMENTO", hfidprovvedimento.value)
    '        'objHashTable.Add("TIPO_PROVVEDIMENTO", CType(Request("TIPOPROVVEDIMENTO"), String))
    '        objHashTable.Add("TIPO_PROVVEDIMENTO", txtTipoProvvedimento.Text)
    '        'objHashTable.Add("TIPO_TRIBUTO", CType(Request("TIPOPROVVEDIMENTO"), String)) 
    '        objHashTable.Add("TIPO_TRIBUTO", txtTipoProvvedimento.Text)
    '        objHashTable.Add("ANNO", CType(Request("ANNO"), String))
    '        objHashTable.Add("NUMERO_ATTO", utility.stringoperation.formatstring(Request("NUMEROATTO")))
    '        objHashTable.Add("IDTIPOPROVVEDIMENTO", CType(Request("IDTIPOPROVVEDIMENTO"), String))
    '        objHashTable.Add("DESCRTRIBUTO", Replace(CType(Request("DESCTRIBUTO"), String), """", "'"))
    '        objHashTable.Add("TIPOPROCEDIMENTO", utility.stringoperation.formatstring(Request("TIPOPROCEDIMENTO")))
    '        objHashTable.Add("COD_CONTRIBUENTE", hdIdContribuente.Value)
    '        objHashTable.Add("DATA_ELABORAZIONE", txtDATA_ELABORAZIONE.Text)
    '        objHashTable.Add("CODENTE", ConstSession.IdEnte)
    '        objHashTable.Add("CODTRIBUTO", Session("COD_TRIBUTO"))
    '        objHashTable.Add("IMPORTO", objUtility.cToDbl(txtImporto.Text))

    '        CallPageaspx(objHashTable("IDPROVVEDIMENTO"), objHashTable("TIPO_PROVVEDIMENTO"), objHashTable("TIPO_TRIBUTO"),
    '          objHashTable("ANNO"), objHashTable("NUMERO_ATTO"), objHashTable("IDTIPOPROVVEDIMENTO"),
    '          objHashTable("DESCRTRIBUTO"), objHashTable("TIPOPROCEDIMENTO"), objHashTable("COD_CONTRIBUENTE"),
    '          objHashTable("DATA_ELABORAZIONE"), objHashTable("CODENTE"), objHashTable("CODTRIBUTO"), objHashTable("IMPORTO"))
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.btnRateizzazioni_Click.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' stampa bollettini di violazione
    ''' controllo l'importo da pagare per vedere se è >0 se è negativo non produco nessun bollettino
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnStampaBollettini_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnStampaBollettini.Click
        Dim sScript As String = ""
        Try
            If CDbl(lblImpTotAvvisoRidotto.Text) > 0 Then
                If Session("bRateizzato") Then
                    sScript += "ApriStampaBollettiniViolazioneRate();"
                Else
                    sScript += "ApriStampaBollettiniViolazione();"
                End If
                RegisterScript(sScript, Me.GetType())
            Else
                sScript = "GestAlert('a', 'warning', '', '', 'L\'importo dovuto è negativo. Non verranno stampati i bollettini');"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.btnStampaBollettini_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnForzaDati_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnForzaDati.Click
        Dim strPARAMETRI As String
        Dim sScript As String = ""
        Try
            strPARAMETRI = "?IDPROVVEDIMENTO=" & hfIdProvvedimento.Value
            strPARAMETRI += "&CODCONTRIBUENTE=" & Replace(hdIdContribuente.Value, "'", "&quot;")
            strPARAMETRI += "&TIPOPROVVEDIMENTO=" & txtTipoProvvedimento.Text
            strPARAMETRI += "&IDTIPOPROVVEDIMENTO=" & txtIDTIPOPROVVEDIMENTO.Text
            strPARAMETRI += "&ANNO=" & txtAnno.Text
            strPARAMETRI += "&NUMEROATTO=" & txtNumeroProvvedimento.Text
            strPARAMETRI += "&PROVENIENZA=" & Session("PROVENIENZA")
            strPARAMETRI += "&DESCTRIBUTO=" & Replace(txtTipoTributo.Text, "'", "&quot;")
            strPARAMETRI += "&TIPOPROCEDIMENTO=" & Session("TIPOPROCEDIMENTO")
            strPARAMETRI += "&PAGINAPRECEDENTE=" & Request("PAGINAPRECEDENTE")

            Session.Add("ParamGestioneAtti", strPARAMETRI)

            sScript += "parent.Comandi.location.href='ForzaDati/CmdForzaDati.aspx';"
            sScript += "location.href='ForzaDati/GestForzaDati.aspx" & strPARAMETRI & "';"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.btnForzaDati_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub


    'Protected Function annoBarra(ByVal objtemp As Object) As String
    '    Dim clsGeneralFunction As New myUtility
    '    Dim strTemp As String = ""
    'Try
    '    If Not IsDBNull(objtemp) Then
    '        strTemp = clsGeneralFunction.GiraDataFromDB(objtemp)
    '    End If
    '    Return strTemp
    ' Catch ex As Exception
    '     Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.annoBarra.errore: ", ex)
    '    Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Function

    'Protected Function ParseDate(ByVal objtemp As Object) As String
    '    Dim strTemp As String = ""
    '    Dim objdate As Date
    'Try
    '    If Not IsDBNull(objtemp) Then
    '        If objtemp.ToString() <> "" Then
    '            objdate = CType(objtemp, Date)
    '            If objdate < Date.MaxValue.ToString("dd/MM/yyyy") Then
    '                strTemp = objdate.Parse(objtemp).ToString("dd/MM/yyyy")
    '            End If
    '            'strTemp = objtemp
    '        End If
    '    End If
    '    Return strTemp
    ' Catch ex As Exception
    '     Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.ParseDate.errore: ", ex)
    '    Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Function

    'Protected Function FormattaNumero(ByVal NumeroDaFormattareParam As Object, ByVal numDec As Integer) As String
    'Try
    '    If IsDBNull(NumeroDaFormattareParam) Then
    '        NumeroDaFormattareParam = FormatNumber(0, numDec)
    '    ElseIf NumeroDaFormattareParam.ToString() = "" Or NumeroDaFormattareParam.ToString() = "-1" Or NumeroDaFormattareParam.ToString() = "-1,00" Then
    '        NumeroDaFormattareParam = FormatNumber(0, numDec)
    '    Else
    '        FormattaNumero = FormatNumber(NumeroDaFormattareParam, numDec)
    '    End If
    ' Catch ex As Exception
    '     Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.FormattaNumero.errore: ", ex)
    '    Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Function

    'Public Function IntForGridView(ByVal iInput As Object) As String

    '    Dim ret As String = String.Empty
    'Try
    '    If iInput.ToString() = "-1" Or iInput.ToString() = "-1,00" Then
    '        ret = String.Empty
    '    Else
    '        ret = Convert.ToString(iInput)
    '    End If

    '    IntForGridView = ret
    ' Catch ex As Exception
    '     Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.IntForGridView.errore: ", ex)
    '    Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Function

    'Public Function FormattaDataGrd(ByVal tDataGrd As DateTime) As String
    'Try
    '    If tDataGrd = Date.MinValue Then
    '        Return ""
    '    Else
    '        Return tDataGrd.ToShortDateString
    '    End If
    ' Catch ex As Exception
    '     Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.FormattaDataGrd.errore: ", ex)
    '    Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Function

    'Public Function FormattaNumeriGrd(ByVal iNumGrd As Integer) As String
    'Try
    '    If iNumGrd <= 0 Then
    '        Return ""
    '    Else
    '        Return iNumGrd.ToString
    '    End If
    ' Catch ex As Exception
    '     Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.FormattaNumeriGrd.errore: ", ex)
    '    Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Function

    'Public Function CalcolaTotaliGrd(ByVal dDifferenzaImposta As Double, ByVal dInteressi As Double, ByVal dSanzioni As Double) As String
    '    Dim dTotale As Double

    '    dTotale = dDifferenzaImposta + dInteressi + dSanzioni
    '    Return FormatNumber(dTotale, 2)
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnVisualizzaDocPDF_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVisualizzaDocPDF.Click
        Try
            'logica di reperimento file pdf per avvisi TARSU
            'Ho reperito tutti i PDF degli avvisi TARSU elaborati dall'applicativo di Andreani:
            'tali files sono nominati per 
            '"Anno di elaborazione"_"Num.avviso(6numeri)"_"Tipo avviso(AV_RT o AV_OM)"_"Cognome"_"Nome".pdf
            Dim sScript As String = ""
            Dim sNomeFile, sAnno, sNumAvviso, sTipoAvviso, sCognome, sNome As String
            Dim sIndirizzoWeb As String = ConfigurationManager.AppSettings("HTTP_PDF_TARSU")
            Dim WC As New WebClient
            Dim bdati As Byte()
            Dim errweb As String = ""
            Dim bFound As Boolean

            If Not IsNothing(ConfigurationManager.AppSettings("HTTP_PDF_TARSU")) Then
                sIndirizzoWeb = ConfigurationManager.AppSettings("HTTP_PDF_TARSU")
                If Right(sIndirizzoWeb, 1) <> "/" Then sIndirizzoWeb += "/"
                sIndirizzoWeb += "Provvedimenti/" + ConstSession.IdEnte + "/"

                If TxtNomePDF.Text <> "" Then
                    sNomeFile = TxtNomePDF.Text
                    sIndirizzoWeb = sIndirizzoWeb & sNomeFile

                    WC.Credentials = CredentialCache.DefaultCredentials
                    Try
                        bFound = True
                    Catch wex As WebException
                        bFound = False
                        errweb = wex.Message
                    End Try
                Else
                    sNumAvviso = txtNumeroProvvedimento.Text
                    sNumAvviso = Right("000000" & Mid(sNumAvviso, InStr(sNumAvviso, "/") + 1), 6) & "_"
                    sAnno = txtAnno.Text & "_"
                    sTipoAvviso = lblTributo.Text
                    If InStr(sTipoAvviso, "UFFICIO") > 0 Then
                        sTipoAvviso = "AV_OM" & "_"
                    ElseIf InStr(sTipoAvviso, "RETTIFICA") > 0 Then
                        sTipoAvviso = "AV_RT" & "_"
                    Else
                        sTipoAvviso = "_"
                    End If
                    sCognome = txtCognome.Text & "_"
                    sNome = txtNome.Text
                    Dim ianno As Integer
                    For ianno = CInt(txtAnno.Text) To 2010
                        errweb = ""
                        sAnno = ianno.ToString() & "_"

                        sNomeFile = sAnno & sNumAvviso & sTipoAvviso & sCognome & sNome & ".pdf"

                        sIndirizzoWeb = sIndirizzoWeb & sNomeFile

                        WC.Credentials = CredentialCache.DefaultCredentials
                        Try
                            bdati = WC.DownloadData(sIndirizzoWeb)
                            bFound = True
                        Catch wex As WebException
                            bFound = False
                            errweb = wex.Message
                        End Try
                        If bFound Then Exit For
                    Next
                End If

                If errweb <> "" Then
                    If InStr(errweb, "404") Then
                        sScript += "GestAlert('a', 'warning', '', '', 'Il file richiesto \'" & sNomeFile & "\' non è stato trovato');"
                    ElseIf InStr(errweb, "500") Then
                        sScript += "GestAlert('a', 'danger', '', '', 'Si è verificato un errore nel server chiamato');"
                    Else
                        sScript += "GestAlert('a', 'danger', '', '', 'Si è verificato un errore non definito');"
                    End If
                Else
                    sScript += "ApriDocumentoPDF('" & sIndirizzoWeb & "');"
                End If
            Else
                sScript += "GestAlert('a', 'danger', '', '', 'Non è stato definito il server dal quale reperire i documenti');"
            End If
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.btnVisualizzaDocPDF_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdGoBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdGoBack.Click
        Try
            'aggiorno la pagina chiamante
            Dim sScript As String = ""
            Log.Debug("devo uscire da gestione atti")
            If Request.Item("Provenienza") = "" Then
                Log.Debug("non ho provenienza")
                If Session("PROVENIENZA") = "OPENGOVTARSU" Then
                    Log.Debug("ho session provenienza")
                    sScript = "parent.Visualizza.location.href='../../OPENgovTARSU/SituazioneContribuente/GestioneSituazione.aspx?COD_CONTRIBUENTE=" & Session("COD_CONTRIBUENTE") & "';"
                Else
                    sScript = "parent.Visualizza.location.href='RicercaSemplice/RicercaSemplice.aspx';"
                End If
                '*** 20150703 - INTERROGAZIONE GENERALE ***
            ElseIf Request.Item("Provenienza") = "INTERGEN" Then
                Log.Debug("ho provenienza intergen")
                sScript = "parent.Visualizza.location.href='../../Interrogazioni/DichEmesso.aspx?Ente=" & ConstSession.IdEnte & "';"
                sScript += "parent.Comandi.location.href='../../aspVuotaRemoveComandi.aspx';"
                sScript += "parent.Basso.location.href='../../aspVuotaRemoveComandi.aspx';"
                sScript += "parent.Nascosto.location.href='../../aspVuotaRemoveComandi.aspx';"
                '*** ***
            End If
            RegisterScript(sScript, Me.GetType())
            Log.Debug("registro script")
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.CmdGoBack_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Serve solo per far ricaricare la pagina da una popup
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnRicarica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRicarica.Click
        '
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory><revision date="24/03/2021">Inserito pulsante per eliminare un'ingiunzione e ripristinare l'avviso bonario</revision></revisionHistory>
    Private Sub CmdRipristinaOrdinario_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdRipristinaOrdinario.Click
        Dim sScript As String = ""
        Dim sSQL As String = ""
        Dim myDataView As New DataView
        Dim nRet As Integer = -1
        Try
            Try
                Dim oDbManagerRepository As New DBModel(ConstSession.DBType, ConstSession.StringConnectionTARSU)
                Using ctx As DBModel = oDbManagerRepository
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_RuoloCartelleInsolutiRipristinaOrdinario", "BELFIORE", "IDCONTRIBUENTE", "IDEMESSO", "ANNOEMISSIONE")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("BELFIORE", ConstSession.Belfiore) _
                        , ctx.GetParam("IDCONTRIBUENTE", hdIdContribuente.Value) _
                        , ctx.GetParam("IDEMESSO", txtNumeroProvvedimento.Text) _
                        , ctx.GetParam("ANNOEMISSIONE", txtAnno.Text)
                    )
                    For Each myRow As DataRowView In myDataView
                        nRet = StringOperation.FormatInt(myRow("id"))
                    Next
                    ctx.Dispose()
                End Using
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.CmdRipristinaOrdinario_Click.errorequery: ", ex)
                Throw New Exception(ex.Message, ex)
            Finally
                myDataView.Dispose()
            End Try

            If nRet <= 0 Then
                sScript += "GestAlert('a', 'success', '', '', 'Avviso ripristinato con successo!');"
            Else
                sScript += "GestAlert('a', 'danger', '', '', 'Si è verificato un\'errore durante il ripristino!');"
            End If
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.CmdRipristinaOrdinario_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

#Region "Griglie"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdRowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            'Dim idCelle As New DataGridIndex
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                Select Case CType(sender, Ribes.OPENgov.WebControls.RibesGridView).UniqueID
                    Case "GrdAccertatoICI"
                        Dim chkSanzioni As CheckBox
                        Dim idSanzioni, idSanzioniPar, DescSanzione, Motivazione As String
                        Dim arraySanzioni() As String

                        e.Row.Cells(0).BackColor = Color.PaleGoldenrod
                        e.Row.Cells(0).Font.Bold = True

                        chkSanzioni = e.Row.Cells(24).FindControl("chkSanzioni")
                        idSanzioni = e.Row.Cells(12).Text

                        'ApriDettaglioSanzioni(idLegame,idCheck, idSanzioni,bloccaCheck,id_provvedimento)

                        If idSanzioni <> "-1" Then
                            chkSanzioni.Checked = True
                            arraySanzioni = Split(idSanzioni, "#")
                            idSanzioniPar = arraySanzioni(0)

                            Dim objGestOPENgovProvvedimenti As DBPROVVEDIMENTI.ProvvedimentiDB
                            Dim objDS As DataSet
                            'Creo oggetto ProvvedementiDB per l'esecuzione delle query
                            objGestOPENgovProvvedimenti = New DBPROVVEDIMENTI.ProvvedimentiDB
                            objDS = objGestOPENgovProvvedimenti.GetTipologieSanzioni(ConstSession.CodTributo, idSanzioniPar)
                            DescSanzione = ""
                            If Not IsNothing(objDS) Then
                                If objDS.Tables.Count > 0 Then
                                    If objDS.Tables(0).Rows.Count > 0 Then
                                        DescSanzione = UCase(objDS.Tables(0).Rows(0)("DESCRIZIONE"))
                                    End If
                                End If
                            End If
                            DescSanzione = DescSanzione.Replace("'", "\'")
                            objDS.Dispose()
                            objGestOPENgovProvvedimenti = Nothing
                            Motivazione = e.Row.Cells(27).Text
                            Motivazione = Motivazione.Replace("'", "\'").Replace("&nbsp;", "").Replace(vbCrLf, "<br>")
                            chkSanzioni.Attributes.Add("onClick", "return VisualizzaMotivazione('" & DescSanzione & "', '" & Motivazione & "')")
                            chkSanzioni.ToolTip = "Premere questo pulsante per visualizzare la sanzione e la motivazione associata all'immobile"
                        Else
                            chkSanzioni.Checked = False
                            chkSanzioni.Enabled = False
                        End If
                    Case "GrdDichiaratoICI"
                        e.Row.Cells(0).BackColor = Color.PaleGoldenrod
                        e.Row.Cells(0).Font.Bold = True
                    Case "GrdAccertatoTARSU"
                        Dim chkSanzioni, chkRiduzioni, chkDetassazioni As CheckBox
                        Dim idSanzioni, idSanzioniPar, DescSanzione, Motivazione As String
                        Dim ImpRiduzione, ImpDetassazione As Double
                        Dim arraySanzioni() As String
                        Dim i As Integer


                        e.Row.Cells(23).BackColor = Color.PaleGoldenrod
                        e.Row.Cells(23).Font.Bold = True

                        chkSanzioni = e.Row.Cells(18).FindControl("chkSanzioniTarsu")
                        idSanzioni = e.Row.Cells(22).Text

                        If idSanzioni <> "-1" And idSanzioni <> "&nbsp;" Then
                            chkSanzioni.Checked = True
                            arraySanzioni = Split(idSanzioni, "#")
                            idSanzioniPar = arraySanzioni(0)

                            Dim objGestOPENgovProvvedimenti As DBPROVVEDIMENTI.ProvvedimentiDB
                            Dim objDS As DataSet
                            'Creo oggetto ProvvedementiDB per l'esecuzione delle query
                            objGestOPENgovProvvedimenti = New DBPROVVEDIMENTI.ProvvedimentiDB
                            objDS = objGestOPENgovProvvedimenti.GetTipologieSanzioni(ConstSession.CodTributo, idSanzioniPar)
                            DescSanzione = ""
                            If Not IsNothing(objDS) Then
                                If objDS.Tables.Count > 0 Then
                                    If objDS.Tables(0).Rows.Count > 0 Then
                                        DescSanzione = UCase(objDS.Tables(0).Rows(0)("DESCRIZIONE"))
                                    End If
                                End If
                            End If
                            DescSanzione = DescSanzione.Replace("'", "\'")
                            objDS.Dispose()
                            objGestOPENgovProvvedimenti = Nothing
                            '*** 20130308 - devo prelevare anche la motivazione ***
                            Motivazione = e.Row.Cells(21).Text             'e.Row.Cells(17).Text
                            '*** ***
                            Motivazione = Motivazione.Replace("'", "\'").Replace("&nbsp;", "").Replace(vbCrLf, "<br>")
                            chkSanzioni.Attributes.Add("onClick", "return VisualizzaMotivazione('" & DescSanzione & "', '" & Motivazione & "')")
                            chkSanzioni.ToolTip = "Premere questo pulsante per visualizzare la sanzione e la motivazione associata all'immobile"
                        Else
                            chkSanzioni.Checked = False
                            chkSanzioni.Enabled = False
                        End If

                        'Riduzioni
                        chkRiduzioni = e.Row.Cells(19).FindControl("chkRiduzioniTarsu")
                        ImpRiduzione = CDbl(e.Row.Cells(24).Text)
                        If ImpRiduzione > 0 Then
                            Dim oAccertato() As ObjArticoloAccertamento
                            Dim objRiduzioni() As ObjRidEseApplicati
                            Dim strRiduzioni As String = ""
                            oAccertato = Session("oAccertatoTARSU")
                            objRiduzioni = oAccertato(e.Row.RowIndex).oRiduzioni()
                            If Not IsNothing(objRiduzioni) Then
                                For i = 0 To objRiduzioni.Length - 1
                                    strRiduzioni += objRiduzioni(i).sDescrizione & "#" & objRiduzioni(i).sDescrTipo & "#" & objRiduzioni(i).sValore & "§"
                                Next
                                strRiduzioni = Mid(strRiduzioni, 1, Len(strRiduzioni) - 1)
                            End If
                            chkRiduzioni.Enabled = True
                            chkRiduzioni.Checked = True

                            chkRiduzioni.Attributes.Add("onClick", "return VisualizzaRiduzione('" & strRiduzioni & "')")
                            chkRiduzioni.ToolTip = "Premere questo pulsante per visualizzare le Riduzioni associate all'immobile"
                        Else
                            chkRiduzioni.Enabled = False
                            chkRiduzioni.Checked = False
                        End If

                        'detassazioni
                        chkDetassazioni = e.Row.Cells(20).FindControl("chkDetassazioniTarsu")
                        ImpDetassazione = CDbl(e.Row.Cells(25).Text)
                        If ImpDetassazione > 0 Then
                            Dim oAccertato() As ObjArticoloAccertamento
                            Dim objRiduzioni() As ObjRidEseApplicati
                            Dim strRiduzioni As String = ""
                            oAccertato = Session("oAccertatoTARSU")
                            objRiduzioni = oAccertato(e.Row.RowIndex).oDetassazioni()
                            If Not IsNothing(objRiduzioni) Then
                                For i = 0 To objRiduzioni.Length - 1
                                    strRiduzioni += objRiduzioni(i).sDescrizione & "#" & objRiduzioni(i).sDescrTipo & "#" & objRiduzioni(i).sValore & "§"
                                Next
                                strRiduzioni = Mid(strRiduzioni, 1, Len(strRiduzioni) - 1)
                            End If
                            chkDetassazioni.Enabled = True
                            chkDetassazioni.Checked = True

                            chkDetassazioni.Attributes.Add("onClick", "return VisualizzaRiduzione('" & strRiduzioni & "')")
                            chkDetassazioni.ToolTip = "Premere questo pulsante per visualizzare le Detassazioni associate all'immobile"
                        Else
                            chkDetassazioni.Enabled = False
                            chkDetassazioni.Checked = False
                        End If
                    Case "GrdDichiaratoTARSU"
                        Dim chkRiduzioni, chkDetassazioni As CheckBox
                        Dim ImpRiduzione, ImpDetassazione As Double
                        Dim i As Integer

                        e.Row.Cells(18).BackColor = Color.PaleGoldenrod
                        e.Row.Cells(18).Font.Bold = True

                        'Riduzioni
                        chkRiduzioni = e.Row.Cells(13).FindControl("chkRiduzioniTarsuDich")
                        ImpRiduzione = CDbl(e.Row.Cells(16).Text)
                        If ImpRiduzione > 0 Then
                            'Dim oDichiarato() As OggettoArticoloRuoloAccertamento
                            Dim oDichiarato() As ObjArticoloAccertamento
                            Dim objRiduzioni() As ObjRidEseApplicati
                            Dim strRiduzioni As String = ""
                            oDichiarato = Session("oDichiaratoTARSU")
                            objRiduzioni = oDichiarato(e.Row.RowIndex).oRiduzioni()
                            If Not IsNothing(objRiduzioni) Then
                                For i = 0 To objRiduzioni.Length - 1
                                    strRiduzioni += objRiduzioni(i).sDescrizione & "#" & objRiduzioni(i).sDescrTipo & "#" & objRiduzioni(i).sValore & "§"
                                Next
                                strRiduzioni = Mid(strRiduzioni, 1, Len(strRiduzioni) - 1)

                                chkRiduzioni.Enabled = True
                                chkRiduzioni.Checked = True

                                chkRiduzioni.Attributes.Add("onClick", "return VisualizzaRiduzione('" & strRiduzioni & "')")
                                chkRiduzioni.ToolTip = "Premere questo pulsante per visualizzare le Riduzioni associate all'immobile"
                            Else
                                chkRiduzioni.Enabled = False
                                chkRiduzioni.Checked = False
                            End If
                        Else
                            chkRiduzioni.Enabled = False
                            chkRiduzioni.Checked = False
                        End If

                        'detassazioni
                        chkDetassazioni = e.Row.Cells(14).FindControl("chkDetassazioniTarsuDich")
                        ImpDetassazione = CDbl(e.Row.Cells(17).Text)
                        If ImpDetassazione > 0 Then
                            Dim oDichiarato() As ObjArticoloAccertamento
                            Dim objRiduzioni() As ObjRidEseApplicati
                            Dim strRiduzioni As String = ""
                            oDichiarato = Session("oDichiaratoTARSU")
                            objRiduzioni = oDichiarato(e.Row.RowIndex).oDetassazioni()
                            If Not IsNothing(objRiduzioni) Then
                                For i = 0 To objRiduzioni.Length - 1
                                    strRiduzioni += objRiduzioni(i).sDescrizione & "#" & objRiduzioni(i).sDescrTipo & "#" & objRiduzioni(i).sValore & "§"
                                Next
                                strRiduzioni = Mid(strRiduzioni, 1, Len(strRiduzioni) - 1)

                                chkDetassazioni.Enabled = True
                                chkDetassazioni.Checked = True

                                chkDetassazioni.Attributes.Add("onClick", "return VisualizzaRiduzione('" & strRiduzioni & "')")
                                chkDetassazioni.ToolTip = "Premere questo pulsante per visualizzare le Detassazioni associate all'immobile"
                            Else
                                chkRiduzioni.Enabled = False
                                chkRiduzioni.Checked = False
                            End If
                        Else
                            chkDetassazioni.Enabled = False
                            chkDetassazioni.Checked = False
                        End If
                    Case "GrdAccertatoOSAP"
                        Dim chkSanzioni As CheckBox
                        Dim idSanzioni, idSanzioniPar, DescSanzione, Motivazione As String
                        Dim arraySanzioni() As String

                        e.Row.Cells(15).BackColor = Color.PaleGoldenrod
                        e.Row.Cells(15).Font.Bold = True

                        chkSanzioni = e.Row.Cells(13).FindControl("chkSanzioniOSAP")
                        idSanzioni = e.Row.Cells(14).Text

                        If idSanzioni <> "-1" And idSanzioni <> "&nbsp;" Then
                            chkSanzioni.Checked = True
                            arraySanzioni = Split(idSanzioni, "#")
                            idSanzioniPar = arraySanzioni(0)

                            Dim objGestOPENgovProvvedimenti As DBPROVVEDIMENTI.ProvvedimentiDB
                            Dim objDS As DataSet
                            'Creo oggetto ProvvedementiDB per l'esecuzione delle query
                            objGestOPENgovProvvedimenti = New DBPROVVEDIMENTI.ProvvedimentiDB
                            objDS = objGestOPENgovProvvedimenti.GetTipologieSanzioni(ConstSession.CodTributo, idSanzioniPar)
                            DescSanzione = ""
                            If Not IsNothing(objDS) Then
                                If objDS.Tables.Count > 0 Then
                                    If objDS.Tables(0).Rows.Count > 0 Then
                                        DescSanzione = UCase(objDS.Tables(0).Rows(0)("DESCRIZIONE"))
                                    End If
                                End If
                            End If
                            DescSanzione = DescSanzione.Replace("'", "\'")
                            objDS.Dispose()
                            objGestOPENgovProvvedimenti = Nothing
                            '*** 20130308 - devo prelevare anche la motivazione ***
                            Motivazione = e.Row.Cells(16).Text             'e.Row.Cells(17).Text
                            '*** ***
                            Motivazione = Motivazione.Replace("'", "\'").Replace("&nbsp;", "").Replace(vbCrLf, "<br>")
                            chkSanzioni.Attributes.Add("onClick", "return VisualizzaMotivazione('" & DescSanzione & "', '" & Motivazione & "')")
                            chkSanzioni.ToolTip = "Premere questo pulsante per visualizzare la sanzione e la motivazione associata all'immobile"
                        Else
                            chkSanzioni.Checked = False
                            chkSanzioni.Enabled = False
                        End If
                End Select
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.GrdRowDataBound.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub GrdAccertatoICI_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAccertatoICI.ItemDataBound
    '    Dim idCelle As New DataGridIndex
    'Try
    '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '        Dim chkSanzioni As CheckBox
    '        Dim idSanzioni, idSanzioniPar, DescSanzione, Motivazione As String
    '        Dim arraySanzioni() As String

    '        Dim ID_PROVVEDIMENTO As String = hfidprovvedimento.value

    '        e.Item.Cells(0).BackColor = Color.PaleGoldenrod
    '        e.Item.Cells(0).Font.Bold = True

    '        chkSanzioni = e.Item.Cells(idCelle.grdSanzioniSanzioni.cellCheckSanzioni).FindControl("chkSanzioni")
    '        idSanzioni = e.Item.Cells(12).Text

    '        'ApriDettaglioSanzioni(idLegame,idCheck, idSanzioni,bloccaCheck,id_provvedimento)

    '        If idSanzioni <> "-1" Then
    '            chkSanzioni.Checked = True
    '            arraySanzioni = Split(idSanzioni, "#")
    '            idSanzioniPar = arraySanzioni(0)

    '            Dim objGestOPENgovProvvedimenti As DBPROVVEDIMENTI.ProvvedimentiDB
    '            Dim objDS As DataSet
    '            'Creo oggetto ProvvedementiDB per l'esecuzione delle query
    '            objGestOPENgovProvvedimenti = New DBPROVVEDIMENTI.ProvvedimentiDB
    '            objDS = objGestOPENgovProvvedimenti.GetTipologieSanzioni(ConstSession.CodTributo, idSanzioniPar)
    '            DescSanzione = ""
    '            If Not IsNothing(objDS) Then
    '                If objDS.Tables.Count > 0 Then
    '                    If objDS.Tables(0).Rows.Count > 0 Then
    '                        DescSanzione = UCase(objDS.Tables(0).Rows(0)("DESCRIZIONE"))
    '                    End If
    '                End If
    '            End If
    '            DescSanzione = DescSanzione.Replace("'", "\'")
    '            objDS.Dispose()
    '            objGestOPENgovProvvedimenti = Nothing
    '            Motivazione = e.Item.Cells(27).Text
    '            Motivazione = Motivazione.Replace("'", "\'").Replace("&nbsp;", "").Replace(vbCrLf, "<br>")
    '            chkSanzioni.Attributes.Add("onClick", "return VisualizzaMotivazione('" & DescSanzione & "', '" & Motivazione & "')")
    '            chkSanzioni.ToolTip = "Premere questo pulsante per visualizzare la sanzione e la motivazione associata all'immobile"



    '            'e.Item.Cells(23).Attributes.Add("onClick", "return ApriDettaglioSanzioni('" & e.Item.Cells(26).Text & "','" & chkSanzioni.ClientID.ToString & "','" & idSanzioniPar & "','1','" & ID_PROVVEDIMENTO & "')")
    '            'e.Item.Cells(23).FindControl("viewSanzioni").Visible = True
    '            'e.Item.Cells(23).FindControl("imgtrasparente").Visible = False
    '        Else
    '            chkSanzioni.Checked = False
    '            chkSanzioni.Enabled = False
    '            'e.Item.Cells(idCelle.grdSanzioniSanzioni.cellCheckSanzioni).FindControl("viewSanzioni").Visible = False
    '            'e.Item.Cells(idCelle.grdSanzioniSanzioni.cellCheckSanzioni).FindControl("imgtrasparente").Visible = True
    '        End If




    '    End If
    ' Catch ex As Exception
    '      Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.GrdAccertatoICI_ItemDataBound.errore: ", ex)
    '     Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub

    'Private Sub GrdDichiaratoICI_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdDichiaratoICI.ItemDataBound
    '    Dim idCelle As New DataGridIndex
    'Try
    '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '        e.Item.Cells(0).BackColor = Color.PaleGoldenrod
    '        e.Item.Cells(0).Font.Bold = True
    '    End If
    ' Catch ex As Exception
    '      Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.GrdDichiarato_ItemDataBound.errore: ", ex)
    '     Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub

    'Private Sub GrdDichiaratoTARSU_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdDichiaratoTARSU.ItemDataBound
    '    Try
    '        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '            Dim chkSanzioni, chkRiduzioni, chkDetassazioni As CheckBox
    '            Dim idSanzioni, idSanzioniPar, DescSanzione, Motivazione As String
    '            Dim ImpRiduzione, ImpDetassazione As Double
    '            Dim arraySanzioni() As String
    '            Dim i As Integer

    '            Dim ID_PROVVEDIMENTO As String = hfidprovvedimento.value

    '            e.Item.Cells(18).BackColor = Color.PaleGoldenrod
    '            e.Item.Cells(18).Font.Bold = True

    '            'Riduzioni
    '            chkRiduzioni = e.Item.Cells(13).FindControl("chkRiduzioniTarsuDich")
    '            ImpRiduzione = CDbl(e.Item.Cells(16).Text)
    '            If ImpRiduzione > 0 Then
    '                'Dim oDichiarato() As OggettoArticoloRuoloAccertamento
    '                Dim oDichiarato() As ObjArticoloAccertamento
    '                Dim objRiduzioni() As ObjRidEseApplicati
    '                Dim strRiduzioni As String = ""
    '                oDichiarato = Session("oDichiaratoTARSU")
    '                objRiduzioni = oDichiarato(e.Item.ItemIndex).oRiduzioni()
    '                If Not IsNothing(objRiduzioni) Then
    '                    For i = 0 To objRiduzioni.Length - 1
    '                        strRiduzioni += objRiduzioni(i).sDescrizione & "#" & objRiduzioni(i).sDescrTipo & "#" & objRiduzioni(i).sValore & "§"
    '                    Next
    '                    strRiduzioni = Mid(strRiduzioni, 1, Len(strRiduzioni) - 1)

    '                    chkRiduzioni.Enabled = True
    '                    chkRiduzioni.Checked = True

    '                    chkRiduzioni.Attributes.Add("onClick", "return VisualizzaRiduzione('" & strRiduzioni & "')")
    '                    chkRiduzioni.ToolTip = "Premere questo pulsante per visualizzare le Riduzioni associate all'immobile"
    '                Else
    '                    chkRiduzioni.Enabled = False
    '                    chkRiduzioni.Checked = False
    '                End If
    '            Else
    '                chkRiduzioni.Enabled = False
    '                chkRiduzioni.Checked = False
    '            End If

    '            'detassazioni
    '            chkDetassazioni = e.Item.Cells(14).FindControl("chkDetassazioniTarsuDich")
    '            ImpDetassazione = CDbl(e.Item.Cells(17).Text)
    '            If ImpDetassazione > 0 Then
    '                Dim oDichiarato() As ObjArticoloAccertamento
    '                Dim objRiduzioni() As ObjRidEseApplicati
    '                Dim strRiduzioni As String = ""
    '                oDichiarato = Session("oDichiaratoTARSU")
    '                objRiduzioni = oDichiarato(e.Item.ItemIndex).oDetassazioni()
    '                If Not IsNothing(objRiduzioni) Then
    '                    For i = 0 To objRiduzioni.Length - 1
    '                        strRiduzioni += objRiduzioni(i).sDescrizione & "#" & objRiduzioni(i).sDescrTipo & "#" & objRiduzioni(i).sValore & "§"
    '                    Next
    '                    strRiduzioni = Mid(strRiduzioni, 1, Len(strRiduzioni) - 1)

    '                    chkDetassazioni.Enabled = True
    '                    chkDetassazioni.Checked = True

    '                    chkDetassazioni.Attributes.Add("onClick", "return VisualizzaRiduzione('" & strRiduzioni & "')")
    '                    chkDetassazioni.ToolTip = "Premere questo pulsante per visualizzare le Detassazioni associate all'immobile"
    '                Else
    '                    chkRiduzioni.Enabled = False
    '                    chkRiduzioni.Checked = False
    '                End If
    '            Else
    '                chkDetassazioni.Enabled = False
    '                chkDetassazioni.Checked = False
    '            End If
    '        End If
    '    Catch ex As Exception
    '      Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.GrdDichiaratoTARSU_ItemDataBound.errore: ", ex)
    '     Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Private Sub GrdAccertatoTARSU_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAccertatoTARSU.ItemDataBound
    '    Try
    '        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '            Dim chkSanzioni, chkRiduzioni, chkDetassazioni As CheckBox
    '            Dim idSanzioni, idSanzioniPar, DescSanzione, Motivazione As String
    '            Dim ImpRiduzione, ImpDetassazione As Double
    '            Dim arraySanzioni() As String
    '            Dim i As Integer

    '            Dim ID_PROVVEDIMENTO As String = hfidprovvedimento.value

    '            e.Item.Cells(23).BackColor = Color.PaleGoldenrod
    '            e.Item.Cells(23).Font.Bold = True

    '            chkSanzioni = e.Item.Cells(18).FindControl("chkSanzioniTarsu")
    '            idSanzioni = e.Item.Cells(22).Text

    '            If idSanzioni <> "-1" And idSanzioni <> "&nbsp;" Then
    '                chkSanzioni.Checked = True
    '                arraySanzioni = Split(idSanzioni, "#")
    '                idSanzioniPar = arraySanzioni(0)

    '                Dim objGestOPENgovProvvedimenti As DBPROVVEDIMENTI.ProvvedimentiDB
    '                Dim objDS As DataSet
    '                'Creo oggetto ProvvedementiDB per l'esecuzione delle query
    '                objGestOPENgovProvvedimenti = New DBPROVVEDIMENTI.ProvvedimentiDB
    '                objDS = objGestOPENgovProvvedimenti.GetTipologieSanzioni(ConstSession.CodTributo, idSanzioniPar)
    '                DescSanzione = ""
    '                If Not IsNothing(objDS) Then
    '                    If objDS.Tables.Count > 0 Then
    '                        If objDS.Tables(0).Rows.Count > 0 Then
    '                            DescSanzione = UCase(objDS.Tables(0).Rows(0)("DESCRIZIONE"))
    '                        End If
    '                    End If
    '                End If
    '                DescSanzione = DescSanzione.Replace("'", "\'")
    '                objDS.Dispose()
    '                objGestOPENgovProvvedimenti = Nothing
    '                '*** 20130308 - devo prelevare anche la motivazione ***
    '                Motivazione = e.Item.Cells(21).Text             'e.Item.Cells(17).Text
    '                '*** ***
    '                Motivazione = Motivazione.Replace("'", "\'").Replace("&nbsp;", "").Replace(vbCrLf, "<br>")
    '                chkSanzioni.Attributes.Add("onClick", "return VisualizzaMotivazione('" & DescSanzione & "', '" & Motivazione & "')")
    '                chkSanzioni.ToolTip = "Premere questo pulsante per visualizzare la sanzione e la motivazione associata all'immobile"
    '            Else
    '                chkSanzioni.Checked = False
    '                chkSanzioni.Enabled = False
    '            End If

    '            'Riduzioni
    '            chkRiduzioni = e.Item.Cells(19).FindControl("chkRiduzioniTarsu")
    '            ImpRiduzione = CDbl(e.Item.Cells(24).Text)
    '            If ImpRiduzione > 0 Then
    '                Dim oAccertato() As ObjArticoloAccertamento
    '                Dim objRiduzioni() As ObjRidEseApplicati
    '                Dim strRiduzioni As String = ""
    '                oAccertato = Session("oAccertatoTARSU")
    '                objRiduzioni = oAccertato(e.Item.ItemIndex).oRiduzioni()
    '                If Not IsNothing(objRiduzioni) Then
    '                    For i = 0 To objRiduzioni.Length - 1
    '                        strRiduzioni += objRiduzioni(i).sDescrizione & "#" & objRiduzioni(i).sDescrTipo & "#" & objRiduzioni(i).sValore & "§"
    '                    Next
    '                    strRiduzioni = Mid(strRiduzioni, 1, Len(strRiduzioni) - 1)
    '                End If
    '                chkRiduzioni.Enabled = True
    '                chkRiduzioni.Checked = True

    '                chkRiduzioni.Attributes.Add("onClick", "return VisualizzaRiduzione('" & strRiduzioni & "')")
    '                chkRiduzioni.ToolTip = "Premere questo pulsante per visualizzare le Riduzioni associate all'immobile"
    '            Else
    '                chkRiduzioni.Enabled = False
    '                chkRiduzioni.Checked = False
    '            End If

    '            'detassazioni
    '            chkDetassazioni = e.Item.Cells(20).FindControl("chkDetassazioniTarsu")
    '            ImpDetassazione = CDbl(e.Item.Cells(25).Text)
    '            If ImpDetassazione > 0 Then
    '                Dim oAccertato() As ObjArticoloAccertamento
    '                Dim objRiduzioni() As ObjRidEseApplicati
    '                Dim strRiduzioni As String = ""
    '                oAccertato = Session("oAccertatoTARSU")
    '                objRiduzioni = oAccertato(e.Item.ItemIndex).oDetassazioni()
    '                If Not IsNothing(objRiduzioni) Then
    '                    For i = 0 To objRiduzioni.Length - 1
    '                        strRiduzioni += objRiduzioni(i).sDescrizione & "#" & objRiduzioni(i).sDescrTipo & "#" & objRiduzioni(i).sValore & "§"
    '                    Next
    '                    strRiduzioni = Mid(strRiduzioni, 1, Len(strRiduzioni) - 1)
    '                End If
    '                chkDetassazioni.Enabled = True
    '                chkDetassazioni.Checked = True

    '                chkDetassazioni.Attributes.Add("onClick", "return VisualizzaRiduzione('" & strRiduzioni & "')")
    '                chkDetassazioni.ToolTip = "Premere questo pulsante per visualizzare le Detassazioni associate all'immobile"
    '            Else
    '                chkDetassazioni.Enabled = False
    '                chkDetassazioni.Checked = False
    '            End If
    '        End If
    '    Catch ex As Exception
    '      Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.GrdAccertatoTARSU_ItemDataBound.errore: ", ex)
    '     Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Private Sub GrdAccertatoOSAP_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAccertatoOSAP.ItemDataBound
    'Try
    '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '        Dim chkSanzioni, chkRiduzioni, chkDetassazioni As CheckBox
    '        Dim idSanzioni, idSanzioniPar, DescSanzione, Motivazione As String
    '        Dim ImpRiduzione, ImpDetassazione As Double
    '        Dim arraySanzioni() As String
    '        Dim i As Integer

    '        Dim ID_PROVVEDIMENTO As String = hfidprovvedimento.value

    '        e.Item.Cells(15).BackColor = Color.PaleGoldenrod
    '        e.Item.Cells(15).Font.Bold = True

    '        chkSanzioni = e.Item.Cells(13).FindControl("chkSanzioniOSAP")
    '        idSanzioni = e.Item.Cells(14).Text

    '        If idSanzioni <> "-1" And idSanzioni <> "&nbsp;" Then
    '            chkSanzioni.Checked = True
    '            arraySanzioni = Split(idSanzioni, "#")
    '            idSanzioniPar = arraySanzioni(0)

    '            Dim objGestOPENgovProvvedimenti As DBPROVVEDIMENTI.ProvvedimentiDB
    '            Dim objDS As DataSet
    '            'Creo oggetto ProvvedementiDB per l'esecuzione delle query
    '            objGestOPENgovProvvedimenti = New DBPROVVEDIMENTI.ProvvedimentiDB
    '            objDS = objGestOPENgovProvvedimenti.GetTipologieSanzioni(ConstSession.CodTributo, idSanzioniPar)
    '            DescSanzione = ""
    '            If Not IsNothing(objDS) Then
    '                If objDS.Tables.Count > 0 Then
    '                    If objDS.Tables(0).Rows.Count > 0 Then
    '                        DescSanzione = UCase(objDS.Tables(0).Rows(0)("DESCRIZIONE"))
    '                    End If
    '                End If
    '            End If
    '            DescSanzione = DescSanzione.Replace("'", "\'")
    '            objDS.Dispose()
    '            objGestOPENgovProvvedimenti = Nothing
    '            '*** 20130308 - devo prelevare anche la motivazione ***
    '            Motivazione = e.Item.Cells(16).Text             'e.Item.Cells(17).Text
    '            '*** ***
    '            Motivazione = Motivazione.Replace("'", "\'").Replace("&nbsp;", "").Replace(vbCrLf, "<br>")
    '            chkSanzioni.Attributes.Add("onClick", "return VisualizzaMotivazione('" & DescSanzione & "', '" & Motivazione & "')")
    '            chkSanzioni.ToolTip = "Premere questo pulsante per visualizzare la sanzione e la motivazione associata all'immobile"
    '        Else
    '            chkSanzioni.Checked = False
    '            chkSanzioni.Enabled = False
    '        End If
    '    End If
    '    Catch ex As Exception
    '      Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.GrdAccertatoOSAP_ItemDataBound.errore: ", ex)
    '     Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Private Sub GrdDichiaratoTARSU_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdDichiaratoTARSU.ItemDataBound
    'Try
    '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '        Dim chkSanzioni, chkRiduzioni, chkDetassazioni As CheckBox
    '        Dim idSanzioni, idSanzioniPar, DescSanzione, Motivazione As String
    '        Dim ImpRiduzione, ImpDetassazione As Double
    '        Dim arraySanzioni() As String
    '        Dim i As Integer

    '        Dim ID_PROVVEDIMENTO As String = hfidprovvedimento.value

    '        e.Item.Cells(15).BackColor = Color.PaleGoldenrod
    '        e.Item.Cells(15).Font.Bold = True

    '        'Riduzioni
    '        chkRiduzioni = e.Item.Cells(11).FindControl("chkRiduzioniTarsuDich")
    '        ImpRiduzione = CDbl(e.Item.Cells(13).Text)
    '        If ImpRiduzione > 0 Then
    '            'Dim oDichiarato() As OggettoArticoloRuoloAccertamento
    '            Dim oDichiarato() As OggettoArticoloRuolo
    '            Dim objRiduzioni() As OggettoRiduzione
    '            Dim strRiduzioni As String = ""
    '            oDichiarato = Session("oDichiaratoTARSU")
    '            objRiduzioni = oDichiarato(e.Item.ItemIndex).oRiduzioni()
    '            If Not IsNothing(objRiduzioni) Then
    '                For i = 0 To objRiduzioni.Length - 1
    '                    strRiduzioni += objRiduzioni(i).Descrizione & "#" & objRiduzioni(i).sTipo & "#" & objRiduzioni(i).sValore & "§"
    '                Next
    '                strRiduzioni = Mid(strRiduzioni, 1, Len(strRiduzioni) - 1)

    '                chkRiduzioni.Enabled = True
    '                chkRiduzioni.Checked = True

    '                chkRiduzioni.Attributes.Add("onClick", "return VisualizzaRiduzione('" & strRiduzioni & "')")
    '                chkRiduzioni.ToolTip = "Premere questo pulsante per visualizzare le Riduzioni associate all'immobile"
    '            Else
    '                chkRiduzioni.Enabled = False
    '                chkRiduzioni.Checked = False

    '            End If

    '        Else
    '            chkRiduzioni.Enabled = False
    '            chkRiduzioni.Checked = False
    '        End If

    '        'detassazioni
    '        chkDetassazioni = e.Item.Cells(12).FindControl("chkDetassazioniTarsuDich")
    '        ImpDetassazione = CDbl(e.Item.Cells(14).Text)
    '        If ImpDetassazione > 0 Then
    '            chkDetassazioni.Enabled = True
    '            chkDetassazioni.Checked = True
    '        Else
    '            chkDetassazioni.Enabled = False
    '            chkDetassazioni.Checked = False
    '        End If

    '    End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.GrdDicharatoTARSU_ItemDataBound.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Private Sub GrdAccertatoTARSU_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAccertatoTARSU.ItemDataBound
    'Try
    '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '        Dim chkSanzioni, chkRiduzioni, chkDetassazioni As CheckBox
    '        Dim idSanzioni, idSanzioniPar, DescSanzione, Motivazione As String
    '        Dim ImpRiduzione, ImpDetassazione As Double
    '        Dim arraySanzioni() As String
    '        Dim i As Integer

    '        Dim ID_PROVVEDIMENTO As String = hfidprovvedimento.value

    '        e.Item.Cells(20).BackColor = Color.PaleGoldenrod
    '        e.Item.Cells(20).Font.Bold = True

    '        chkSanzioni = e.Item.Cells(15).FindControl("chkSanzioniTarsu")
    '        idSanzioni = e.Item.Cells(19).Text

    '        If idSanzioni <> "-1" And idSanzioni <> "&nbsp;" Then
    '            chkSanzioni.Checked = True
    '            arraySanzioni = Split(idSanzioni, "#")
    '            idSanzioniPar = arraySanzioni(0)

    '            Dim objGestOPENgovProvvedimenti As DBPROVVEDIMENTI.ProvvedimentiDB
    '            Dim objDS As DataSet
    '            'Creo oggetto ProvvedementiDB per l'esecuzione delle query
    '            objGestOPENgovProvvedimenti = New DBPROVVEDIMENTI.ProvvedimentiDB
    '            objDS = objGestOPENgovProvvedimenti.GetTipologieSanzioni(idSanzioniPar)
    '            DescSanzione = ""
    '            If Not IsNothing(objDS) Then
    '                If objDS.Tables.Count > 0 Then
    '                    If objDS.Tables(0).Rows.Count > 0 Then
    '                        DescSanzione = UCase(objDS.Tables(0).Rows(0)("DESCRIZIONE"))
    '                    End If
    '                End If
    '            End If
    '            DescSanzione = DescSanzione.Replace("'", "\'")
    '            objDS.Dispose()
    '            objGestOPENgovProvvedimenti = Nothing
    '            '*** 20130308 - devo prelevare anche la motivazione ***
    '            Motivazione = e.Item.Cells(18).Text             'e.Item.Cells(17).Text
    '            '*** ***
    '            Motivazione = Motivazione.Replace("'", "\'").Replace("&nbsp;", "").Replace(vbCrLf, "<br>")
    '            chkSanzioni.Attributes.Add("onClick", "return VisualizzaMotivazione('" & DescSanzione & "', '" & Motivazione & "')")
    '            chkSanzioni.ToolTip = "Premere questo pulsante per visualizzare la sanzione e la motivazione associata all'immobile"

    '        Else
    '            chkSanzioni.Checked = False
    '            chkSanzioni.Enabled = False
    '        End If

    '        'Riduzioni
    '        chkRiduzioni = e.Item.Cells(16).FindControl("chkRiduzioniTarsu")
    '        ImpRiduzione = CDbl(e.Item.Cells(21).Text)
    '        If ImpRiduzione > 0 Then
    '            'Dim oAccertato() As OggettoArticoloRuoloAccertamento
    '            Dim oAccertato() As OggettoArticoloRuolo
    '            Dim objRiduzioni() As OggettoRiduzione
    '            Dim strRiduzioni As String = ""
    '            oAccertato = Session("oAccertatoTARSU")
    '            objRiduzioni = oAccertato(e.Item.ItemIndex).oRiduzioni()
    '            If Not IsNothing(objRiduzioni) Then
    '                For i = 0 To objRiduzioni.Length - 1
    '                    strRiduzioni += objRiduzioni(i).Descrizione & "#" & objRiduzioni(i).sTipo & "#" & objRiduzioni(i).sValore & "§"
    '                Next
    '                strRiduzioni = Mid(strRiduzioni, 1, Len(strRiduzioni) - 1)
    '            End If
    '            chkRiduzioni.Enabled = True
    '            chkRiduzioni.Checked = True

    '            chkRiduzioni.Attributes.Add("onClick", "return VisualizzaRiduzione('" & strRiduzioni & "')")
    '            chkRiduzioni.ToolTip = "Premere questo pulsante per visualizzare le Riduzioni associate all'immobile"
    '        Else
    '            chkRiduzioni.Enabled = False
    '            chkRiduzioni.Checked = False
    '        End If

    '        'detassazioni
    '        chkDetassazioni = e.Item.Cells(17).FindControl("chkDetassazioniTarsu")
    '        ImpDetassazione = CDbl(e.Item.Cells(22).Text)
    '        If ImpDetassazione > 0 Then
    '            chkDetassazioni.Enabled = True
    '            chkDetassazioni.Checked = True
    '        Else
    '            chkDetassazioni.Enabled = False
    '            chkDetassazioni.Checked = False
    '        End If
    '    End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.GrdAccertatoTARSU_ItemDataBound.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    '*** ***

    '*** ***
#End Region
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="intIDProvvedimento"></param>
    ''' <param name="strTipoProvvedimento"></param>
    ''' <param name="strTipoTributo"></param>
    ''' <param name="strAnno"></param>
    ''' <param name="strNumeroAtto"></param>
    ''' <param name="intCodTipoProvvedimento"></param>
    ''' <param name="strDescTipoTributo"></param>
    ''' <param name="strTipoProcedimento"></param>
    ''' <param name="codContribuente"></param>
    ''' <param name="dataElaborazione"></param>
    ''' <param name="codEnte"></param>
    ''' <param name="codTributo"></param>
    ''' <param name="importo"></param>
    Protected Sub CallPageaspx(ByVal intIDProvvedimento As Integer, ByVal strTipoProvvedimento As String, ByVal strTipoTributo As String, ByVal strAnno As String, ByVal strNumeroAtto As String, ByVal intCodTipoProvvedimento As Integer, ByVal strDescTipoTributo As String, ByVal strTipoProcedimento As String, ByVal codContribuente As String, ByVal dataElaborazione As String, ByVal codEnte As String, ByVal codTributo As String, ByVal importo As Double)
        Dim strPARAMETRI As String
        Dim sScript As String = ""
        Try
            strPARAMETRI = "?IDPROVVEDIMENTO=" & intIDProvvedimento
            strPARAMETRI = strPARAMETRI & "&TIPOTRIBUTO=" & Replace(strTipoTributo, "'", "&quot;")
            strPARAMETRI = strPARAMETRI & "&TIPOPROVVEDIMENTO=" & strTipoProvvedimento
            strPARAMETRI = strPARAMETRI & "&ANNO=" & strAnno
            strPARAMETRI = strPARAMETRI & "&NUMEROATTO=" & strNumeroAtto
            strPARAMETRI = strPARAMETRI & "&IDTIPOPROVVEDIMENTO=" & intCodTipoProvvedimento
            strPARAMETRI = strPARAMETRI & "&DESCTRIBUTO=" & Replace(strDescTipoTributo, "'", "\'")
            strPARAMETRI = strPARAMETRI & "&TIPOPROCEDIMENTO=" & strTipoProcedimento
            strPARAMETRI = strPARAMETRI & "&COD_CONTRIBUENTE=" & codContribuente
            strPARAMETRI = strPARAMETRI & "&DATA_ELABORAZIONE=" & dataElaborazione
            strPARAMETRI = strPARAMETRI & "&CODENTE=" & codEnte
            strPARAMETRI = strPARAMETRI & "&COD_TRIBUTO=" & codTributo
            strPARAMETRI = strPARAMETRI & "&IMPORTO=" & importo

            'Session("CODICE_CONTRIBUENTE_RICERCA_SEMPLICE") = utility.stringoperation.formatstring(Request("COD_CONTRIBUENTE"))

            '
            'sscript+="location.href='Rateizzazioni.aspx" & strPARAMETRI & "';")
            '

            '    RegisterScript(sScript , Me.GetType())
            sScript += "window.open(""FrameRateizzazioni.aspx" & strPARAMETRI & """,""ShowRateizzazioni"", ""width=700,height=550,top=50, left=162, status=yes, scrollbars=no,toolbar=no"");"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.CallPageaspx.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** 20130801 - accertamento OSAP ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="rowPROVVEDIMENTO"></param>
    ''' <param name="sError"></param>
    ''' <returns></returns>
    Private Function LoadAnagrafica(ByVal rowPROVVEDIMENTO As DataRow, ByRef sError As String) As Boolean
        Dim strIndirizzo, strCodFiscalePI As String
        Try
            txtNOME_OGGETTO.Text = "document.getElementById('txtDataRettificaAvviso')"

            '*** 201504 - Nuova Gestione anagrafica con form unico ***
            hdIdContribuente.Value = CInt(rowPROVVEDIMENTO("cod_contribuente"))
            If ConstSession.HasPlainAnag Then
                ifrmAnag.Attributes.Add("src", "../../Generali/asp/VisualAnag.aspx?IdContribuente=" & CInt(rowPROVVEDIMENTO("cod_contribuente")) & "&Azione=" & Utility.Costanti.AZIONE_LETTURA)
            Else
                lblCognomeNome.Text = Utility.StringOperation.FormatString(rowPROVVEDIMENTO("COGNOME")) & " " & Utility.StringOperation.FormatString(rowPROVVEDIMENTO("NOME"))

                If Len(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("PROVINCIA_RES"))) = 0 Then
                    strIndirizzo = Utility.StringOperation.FormatString(rowPROVVEDIMENTO("VIA_RES")) & " " & Utility.StringOperation.FormatString(rowPROVVEDIMENTO("CIVICO_RES")) & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & Utility.StringOperation.FormatString(rowPROVVEDIMENTO("CITTA_RES")) & " " & Utility.StringOperation.FormatString(rowPROVVEDIMENTO("CAP_RES"))
                Else
                    strIndirizzo = Utility.StringOperation.FormatString(rowPROVVEDIMENTO("VIA_RES")) & " " & Utility.StringOperation.FormatString(rowPROVVEDIMENTO("CIVICO_RES")) & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" & Utility.StringOperation.FormatString(rowPROVVEDIMENTO("CITTA_RES")) & " " & Utility.StringOperation.FormatString(rowPROVVEDIMENTO("CAP_RES")) & " " & "(" & Utility.StringOperation.FormatString(rowPROVVEDIMENTO("PROVINCIA_RES")) & ")"
                End If
                lblResidenza.Text = strIndirizzo

                If Len(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("PARTITA_IVA"))) = 0 Then
                    strCodFiscalePI = Utility.StringOperation.FormatString(rowPROVVEDIMENTO("CODICE_FISCALE"))
                Else
                    strCodFiscalePI = Utility.StringOperation.FormatString(rowPROVVEDIMENTO("PARTITA_IVA"))
                End If

                lblCfPiva.Text = strCodFiscalePI

                txtCognome.Text = Utility.StringOperation.FormatString(rowPROVVEDIMENTO("COGNOME"))
                txtNome.Text = Utility.StringOperation.FormatString(rowPROVVEDIMENTO("NOME"))
                txtCodiceFiscale.Text = Utility.StringOperation.FormatString(rowPROVVEDIMENTO("CODICE_FISCALE"))
                txtPartitaIVA.Text = Utility.StringOperation.FormatString(rowPROVVEDIMENTO("PARTITA_IVA"))
                txtIndirizzo.Text = Utility.StringOperation.FormatString(rowPROVVEDIMENTO("VIA_RES")) & " " & Utility.StringOperation.FormatString(rowPROVVEDIMENTO("CIVICO_RES"))
                txtComuneResidenza.Text = Utility.StringOperation.FormatString(rowPROVVEDIMENTO("CITTA_RES"))
            End If
            '*** ***
            txtNOMINATIVO_RETTIFICA.Text = Utility.StringOperation.FormatString(rowPROVVEDIMENTO("COGNOME")) & " " & Utility.StringOperation.FormatString(rowPROVVEDIMENTO("NOME"))
            Return True
        Catch ex As Exception
            sError = ex.Message
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.LoadAnagrafica.errore: ", ex)
            'Response.Redirect("../../PaginaErrore.aspx")
            Return False
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="rowPROVVEDIMENTO"></param>
    ''' <param name="dblImpTotAvvisoRidotto"></param>
    ''' <param name="dblImpTotAvvisoPieno"></param>
    ''' <param name="sError"></param>
    ''' <returns></returns>
    Private Function LoadImporti(ByVal rowPROVVEDIMENTO As DataRow, ByRef dblImpTotAvvisoRidotto As Double, ByRef dblImpTotAvvisoPieno As Double, ByRef sError As String) As Boolean
        Try
            If Utility.StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_DIFFERENZA_IMPOSTA")).Length = 0 Then
                txtImportoDifferenzaImposta.Text = "0"
            Else
                txtImportoDifferenzaImposta.Text = FormatNumber(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_DIFFERENZA_IMPOSTA")), 2)
            End If

            If Utility.StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_DIFFERENZA_IMPOSTA")).Length = 0 Then
                txtImportoDifferenzaImposta.Text = "0"
            Else
                txtImportoDifferenzaImposta.Text = FormatNumber(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_DIFFERENZA_IMPOSTA")), 2)
            End If

            If Utility.StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_INTERESSI")).Length = 0 Then
                txtImportoInteressi.Text = "0"
            Else
                txtImportoInteressi.Text = FormatNumber(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_INTERESSI")), 2)
            End If

            If Utility.StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_SANZIONI")).Length = 0 Then
                txtImportoSanzioni.Text = "0"
            Else
                txtImportoSanzioni.Text = FormatNumber(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_SANZIONI")), 2)
            End If

            If Utility.StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_SPESE")).Length = 0 Then
                txtImportoSpese.Text = "0"
            Else
                txtImportoSpese.Text = FormatNumber(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_SPESE")), 2)
            End If

            If Utility.StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_TOTALE_RIDOTTO")).Length <> 0 Then
                dblImpTotAvvisoRidotto = objUtility.cToDbl(rowPROVVEDIMENTO("IMPORTO_TOTALE_RIDOTTO"))
            End If
            txtImporto.Text = dblImpTotAvvisoRidotto
            lblImpTotAvvisoRidotto.Text = "€ " & Utility.StringOperation.FormatString(FormatNumber(dblImpTotAvvisoRidotto, 2))
            '*** 20130125 - inserimento gestione pagato anche rispetto all'importo pieno ***
            If Utility.StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_TOTALE")).Length <> 0 Then
                dblImpTotAvvisoPieno = objUtility.cToDbl(rowPROVVEDIMENTO("IMPORTO_TOTALE"))
            End If
            lblImpTotAvvisoPieno.Text = "€ " & Utility.StringOperation.FormatString(FormatNumber(dblImpTotAvvisoPieno, 2))
            '*** ***
            If Utility.StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_ALTRO")).Length = 0 Then
                txtAltroImporto.Text = "0"
            Else
                txtAltroImporto.Text = FormatNumber(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_ALTRO")), 2)
            End If
            If Utility.StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_RUOLO_COATTIVO")).Length = 0 Then
                txtImportoRuoloCoattivoICI.Text = "0,00"
            Else
                txtImportoRuoloCoattivoICI.Text = FormatNumber(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_RUOLO_COATTIVO")), 2)
            End If
            Return True
        Catch ex As Exception
            sError = ex.Message
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.LoadImporti.errore: ", ex)
            'Response.Redirect("../../PaginaErrore.aspx")
            Return False
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="rowPROVVEDIMENTO"></param>
    ''' <param name="sError"></param>
    ''' <returns></returns>
    Private Function LoadDateNote(ByVal rowPROVVEDIMENTO As DataRow, ByRef sError As String) As Boolean
        Try
            txtDATA_ELABORAZIONE.Text = Utility.StringOperation.FormatString(rowPROVVEDIMENTO("DATA_ELABORAZIONE"))
            lblDATAGENERAZIONE.Text = objUtility.GiraDataFromDB(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("DATA_ELABORAZIONE")))
            txtDataGenerazione.Text = objUtility.GiraDataFromDB(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("DATA_ELABORAZIONE")))
            '**************************************************************************
            'QUESTIONARI
            '**************************************************************************
            txtDataStampa.Text = objUtility.GiraDataFromDB(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("DATA_STAMPA")))
            txtDataConsegna.Text = objUtility.GiraDataFromDB(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("DATA_CONSEGNA_AVVISO")))
            txtDataNotifica.Text = objUtility.GiraDataFromDB(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("DATA_NOTIFICA_AVVISO")))

            If Utility.StringOperation.FormatString(rowPROVVEDIMENTO("DATA_PERVENUTO_IL")).Length > 0 Then
                txtPervenutoQuestionari.Enabled = False
                txtHiddenPERVENUTOIL.Text = "-1"
            End If

            txtPervenutoQuestionari.Text = objUtility.GiraDataFromDB(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("DATA_PERVENUTO_IL")))
            '**************************************************************************
            'LIQUIDAZIONI (PREACCERTAMENTI)
            '**************************************************************************
            'lblNumeroAvviso.Text = utility.stringoperation.formatstring(rowPROVVEDIMENTO("e"))

            txtDataConfermaAvviso.Text = objUtility.GiraDataFromDB(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("DATA_CONFERMA")))
            txtDataStampaAvviso.Text = objUtility.GiraDataFromDB(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("DATA_STAMPA")))
            txtDataConsegnaAvviso.Text = objUtility.GiraDataFromDB(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("DATA_CONSEGNA_AVVISO")))
            txtDataNotificaAvviso.Text = objUtility.GiraDataFromDB(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("DATA_NOTIFICA_AVVISO")))
            txtDataRettificaAvviso.Text = objUtility.GiraDataFromDB(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("DATA_RETTIFICA_AVVISO")))
            'M.B.
            '''''''''If (txtDataRettificaAvviso.Text = "") Then
            '''''''''    txtDataRettificaAvviso.Text = DateTime.Now()
            '''''''''End If

            txtDataAnnullamentoAvviso.Text = objUtility.GiraDataFromDB(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("DATA_ANNULLAMENTO_AVVISO")))
            txtDataSospensioneAvvisoAutotutela.Text = objUtility.GiraDataFromDB(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("DATA_SOSPENSIONE_AVVISO_AUTOTUTELA")))
            txtDataIrreperibile.Text = objUtility.GiraDataFromDB(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("DATA_IRREPERIBILE")))

            txtDataRicorsoProvinciale.Text = objUtility.GiraDataFromDB(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("DATA_PRESENTAZIONE_RICORSO")))
            txtSospensioneProvinciale.Text = objUtility.GiraDataFromDB(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("DATA_SOSPENSIONE_DA_COMMISSIONE_TRIBUTARIA")))
            txtSentenzaProvinciale.Text = objUtility.GiraDataFromDB(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("DATA_SENTENZA")))
            txtNoteProvinciale.Text = Utility.StringOperation.FormatString(rowPROVVEDIMENTO("NOTE_PROVINCIALE"))

            txtNoteConcGiudiz.Text = Utility.StringOperation.FormatString(rowPROVVEDIMENTO("NOTE_CONCILIAZIONE_G"))
            If IsNothing(rowPROVVEDIMENTO("FLAG_CONCILIAZIONE_G")) Or rowPROVVEDIMENTO("FLAG_CONCILIAZIONE_G") Is DBNull.Value Then
                ckConcGiudiz.Checked = False
            Else
                If rowPROVVEDIMENTO("FLAG_CONCILIAZIONE_G") = 0 Then
                    ckConcGiudiz.Checked = False
                Else
                    ckConcGiudiz.Checked = True
                End If
            End If

            txtDataRicorsoRegionale.Text = objUtility.GiraDataFromDB(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("DATA_PRESENTAZIONE_RICORSO_REGIONALE")))
            txtSospensioneRegionale.Text = objUtility.GiraDataFromDB(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("DATA_SOSPENSIONE_DA_COMMISSIONE_TRIBUTARIA_REGIONALE")))
            txtSentenzaRegionale.Text = objUtility.GiraDataFromDB(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("DATA_SENTENZA_REGIONALE")))
            txtNoteRegionale.Text = Utility.StringOperation.FormatString(rowPROVVEDIMENTO("NOTE_REGIONALE"))

            txtDataRicorsoCassazione.Text = objUtility.GiraDataFromDB(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("DATA_PRESENTAZIONE_RICORSO_CASSAZIONE")))
            txtSospensioneCassazione.Text = objUtility.GiraDataFromDB(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("DATA_SOSPENSIONE_DA_COMMISSIONE_TRIBUTARIA_CASSAZIONE")))
            txtSentenzaCassazione.Text = objUtility.GiraDataFromDB(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("DATA_SENTENZA_CASSAZIONE")))
            txtNoteCassazione.Text = Utility.StringOperation.FormatString(rowPROVVEDIMENTO("NOTE_CASSAZIONE"))

            If rowPROVVEDIMENTO("ESITO_ACCERTAMENTO") Is DBNull.Value Or IsNothing(rowPROVVEDIMENTO("ESITO_ACCERTAMENTO")) Then
                ddlEsitoAccertamenti.SelectedValue = -1
            Else
                ddlEsitoAccertamenti.SelectedValue = Utility.StringOperation.FormatInt(rowPROVVEDIMENTO("ESITO_ACCERTAMENTO"))
            End If
            txtTermineRicorso.Text = objUtility.GiraDataFromDB(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("TERMINE_RICORSO_ACC")))
            txtNoteAccertamenti.Text = Utility.StringOperation.FormatString(rowPROVVEDIMENTO("NOTE_ACCERTAMENTO"))
            If IsNothing(rowPROVVEDIMENTO("FLAG_ACCERTAMENTO")) Or rowPROVVEDIMENTO("FLAG_ACCERTAMENTO") Is DBNull.Value Then
                ckAccertamento.Checked = False
            Else
                If rowPROVVEDIMENTO("FLAG_ACCERTAMENTO") = 0 Then
                    ckAccertamento.Checked = False
                Else
                    ckAccertamento.Checked = True
                End If
            End If

            txtDataAttoDefinitivo.Text = objUtility.GiraDataFromDB(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("DATA_ATTO_DEFINITIVO")))
            txtDataVersamentoUnicaSoluzione.Text = objUtility.GiraDataFromDB(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("DATA_VERSAMENTO_SOLUZIONE_UNICA")))
            txtDataConcessioneRateizzazione.Text = objUtility.GiraDataFromDB(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("DATA_CONCESSIONE_RATEIZZAZIONE")))
            'txtDataSollecitoBonario.Text = objUtility.GiraDataFromDB(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("DATA_SOLLECITO_BONARIO")))
            txtDataRuoloOrdinario.Text = objUtility.GiraDataFromDB(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("DATA_RUOLO_ORDINARIO_TARSU")))
            txtDataRuoloCoattivoICI.Text = objUtility.GiraDataFromDB(Utility.StringOperation.FormatString(rowPROVVEDIMENTO("DATA_COATTIVO")))
            '*** Fabi
            txtNoteGenerali.Text = Utility.StringOperation.FormatString(rowPROVVEDIMENTO("NOTE_GENERALI_ATTO"))
            '*** /Fabi
            Return True
        Catch ex As Exception
            sError = ex.Message
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.LoadDateNote.errore: ", ex)
            'Response.Redirect("../../PaginaErrore.aspx")
            Return False
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="rowPROVVEDIMENTO"></param>
    ''' <param name="dblImpTotAvvisoRidotto"></param>
    ''' <param name="dblImpTotAvvisoPieno"></param>
    ''' <param name="sError"></param>
    ''' <returns></returns>
    Private Function LoadPagamenti(ByVal rowPROVVEDIMENTO As DataRow, ByVal dblImpTotAvvisoRidotto As Double, ByVal dblImpTotAvvisoPieno As Double, ByRef sError As String) As Boolean
        Dim dtVers As DataView
        Dim objRicercaVers As New clsPagamenti(ConstSession.StringConnection) 'clsPagamenti(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))
        Dim dblImportoPagato As Double = 0
        Dim dblResiduo As Double = 0
        Try
            'Dim objRicercaVers As IElaborazioneAtti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAtti), ConstSession.URLServiziElaborazioneAtti)
            objDSVers = objRicercaVers.getPagamenti(ConstSession.IdEnte, "", "", "", "", rowPROVVEDIMENTO("ID_PROVVEDIMENTO"), "", DateTime.MaxValue, DateTime.MaxValue)
            'If Session("COD_TRIBUTO") = "8852" And objHashTable("NUMERO_ATTO") <> "" Then
            '    objDSVers = objRicercaVers.getVersamentiICI(objHashTable)
            'End If
            If Not objDSVers Is Nothing Then
                dtVers = objDSVers.Tables(0).DefaultView
                Dim i As Integer
                For i = 0 To objDSVers.Tables(0).Rows.Count - 1
                    dblImportoPagato += objDSVers.Tables(0).Rows(i)("Importo_Pagato")
                Next
            End If
            'popolo griglia
            grdPagamenti.DataSource = dtVers
            grdPagamenti.DataBind()
            'popolo text
            If Utility.StringOperation.FormatString(dblImportoPagato).Length = 0 Then
                txtImportoPagato.Text = "0"
            Else
                txtImportoPagato.Text = FormatNumber(dblImportoPagato, 2)
            End If

            '*** gestione riepilogo importi *****

            'If txtDataVersamentoUnicaSoluzione.Text <> "" Then
            '    lblTotPagato.Text = lblImpTotAvviso.Text
            'Else
            lblTotPagato.Text = "€ " & FormatNumber(dblImportoPagato, 2)
            'End If

            '*** 20130125 - inserimento gestione pagato anche rispetto all'importo pieno ***
            dblResiduo = dblImpTotAvvisoRidotto - dblImportoPagato
            If dblResiduo <= 0 Then
                lblResiduo.ControlStyle.CssClass = "riga_menu_verde"
                lblResiduo.Text = "€ " & FormatNumber(dblResiduo, 2)
            Else
                dblResiduo = dblImpTotAvvisoPieno - dblImportoPagato
                If dblResiduo <= 0 Then
                    lblResiduo.ControlStyle.CssClass = "riga_menu_verde"
                    lblResiduo.Text = "€ " & FormatNumber(dblResiduo, 2)
                Else
                    lblResiduo.ControlStyle.CssClass = "riga_menu_mouse_over"
                    lblResiduo.Text = "€ " & FormatNumber(dblResiduo, 2)
                End If
            End If
            '*** ***
            'If objUtility.cToDbl(lblTotPagato.Text.Replace("€ ", "")) > 0 Then
            '    lblResiduo.ControlStyle.CssClass = "riga_menu_verde"
            '    lblResiduo.Text = "€ 0,00"
            'Else
            '    lblResiduo.ControlStyle.CssClass = "riga_menu_mouse_over"
            '    lblResiduo.Text = lblImpTotAvviso.Text
            'End If
            '******************************
            Return True
        Catch ex As Exception
            sError = ex.Message
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.LoadPagamenti.errore: ", ex)
            'Response.Redirect("../../PaginaErrore.aspx")
            Return False
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="rowPROVVEDIMENTO"></param>
    ''' <param name="sError"></param>
    ''' <returns></returns>
    Private Function LoadRiepilogoImporti(ByVal rowPROVVEDIMENTO As DataRow, ByRef sError As String) As Boolean
        Try
            'Riepilogo PreAccertamento FASE 2 {confronto pagato con dovuto}+FASE 1 {confronto data pagato con scadenza dovuto}
            LblImpDichPreAcc.Text = IIf(rowPROVVEDIMENTO("IMPORTO_DICHIARATO_F2") Is DBNull.Value, "ND", Utility.StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_DICHIARATO_F2")) & " €")
            LblImpVersPreAcc.Text = IIf(rowPROVVEDIMENTO("IMPORTO_VERSATO_F2") Is DBNull.Value, "ND", Utility.StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_VERSATO_F2")) & " €")
            LblImpDifImpPreAcc.Text = IIf(rowPROVVEDIMENTO("IMPORTO_DIFFERENZA_IMPOSTA_F2") Is DBNull.Value, "ND", Utility.StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_DIFFERENZA_IMPOSTA_F2")) & " €")
            LblImpSanzPreAcc.Text = IIf(rowPROVVEDIMENTO("IMPORTO_SANZIONI_F2") Is DBNull.Value, "ND", Utility.StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_SANZIONI_F2")) & " €")
            LblImpIntPreAcc.Text = IIf(rowPROVVEDIMENTO("IMPORTO_INTERESSI_F2") Is DBNull.Value, "ND", Utility.StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_INTERESSI_F2")) & " €")
            LblImpTotPreAcc.Text = IIf(rowPROVVEDIMENTO("IMPORTO_TOTALE_F2") Is DBNull.Value, "ND", Utility.StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_TOTALE_F2")) & " €")

            'Riepilogo Accertamento {confronto dichiarato con accertato}
            If rowPROVVEDIMENTO("COD_TRIBUTO") = Utility.Costanti.TRIBUTO_TARSU Then 'If Session("COD_TRIBUTO") = "0434" Then
                fldPreAcc.Visible = False
                fldAcc.Visible = False
            Else
                LblImpDich.Text = IIf(rowPROVVEDIMENTO("IMPORTO_DICHIARATO_F2") Is DBNull.Value, "ND", Utility.StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_DICHIARATO_F2")) & " €")
                LblImpAcc.Text = IIf(rowPROVVEDIMENTO("IMPORTO_ACCERTATO_ACC") Is DBNull.Value, "ND", Utility.StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_ACCERTATO_ACC")) & " €")
                LblImpDifImp.Text = IIf(rowPROVVEDIMENTO("IMPORTO_DIFFERENZA_IMPOSTA_ACC") Is DBNull.Value, "ND", Utility.StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_DIFFERENZA_IMPOSTA_ACC")) & " €")
                LblImpSanz.Text = IIf(rowPROVVEDIMENTO("IMPORTO_SANZIONI_ACC") Is DBNull.Value, "ND", Utility.StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_SANZIONI_ACC")) & " €")
                LblImpSanzRid.Text = IIf(rowPROVVEDIMENTO("IMPORTO_SANZIONI_RIDOTTE_ACC") Is DBNull.Value, "ND", Utility.StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_SANZIONI_RIDOTTE_ACC")) & " €")
                LblImpInt.Text = IIf(rowPROVVEDIMENTO("IMPORTO_INTERESSI_ACC") Is DBNull.Value, "ND", Utility.StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_INTERESSI_ACC")) & " €")
                LblImpTot.Text = IIf(rowPROVVEDIMENTO("IMPORTO_TOTALE_ACC") Is DBNull.Value, "ND", Utility.StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_TOTALE_ACC")) & " €")
            End If

            'Riepilogo totali
            LblImpDifImpAvviso.Text = IIf(rowPROVVEDIMENTO("IMPORTO_DIFFERENZA_IMPOSTA") Is DBNull.Value, "ND", Utility.StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_DIFFERENZA_IMPOSTA")) & " €")
            LblImpSanzAvviso.Text = IIf(rowPROVVEDIMENTO("IMPORTO_SANZIONI") Is DBNull.Value, "ND", Utility.StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_SANZIONI")) & " €")
            LblImpSanzRidAvviso.Text = IIf(rowPROVVEDIMENTO("IMPORTO_SANZIONI_RIDOTTO") Is DBNull.Value, "ND", Utility.StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_SANZIONI_RIDOTTO")) & " €")
            LblImpIntAvviso.Text = IIf(rowPROVVEDIMENTO("IMPORTO_INTERESSI") Is DBNull.Value, "ND", Utility.StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_INTERESSI")) & " €")
            LblImpTotAvviso.Text = IIf(rowPROVVEDIMENTO("IMPORTO_TOTALE") Is DBNull.Value, "ND", Utility.StringOperation.FormatString(rowPROVVEDIMENTO("IMPORTO_TOTALE")) & " €")

            Return True
        Catch ex As Exception
            sError = ex.Message
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.LoadRiepilogoImporti.errore: ", ex)
            'Response.Redirect("../../PaginaErrore.aspx")
            Return False
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="rowPROVVEDIMENTO"></param>
    ''' <param name="sError"></param>
    ''' <returns></returns>
    Private Function LoadDettaglioICI(ByVal rowPROVVEDIMENTO As DataRow, ByRef sError As String) As Boolean
        Dim objDataTable() As objUIICIAccert
        Try
            fldDicICI.Visible = True : fldAccICI.Visible = True : lblNFDichICI.Visible = False : lblNFAccICI.Visible = False

            fldDicTARSU.Visible = False : fldAccTARSU.Visible = False : lblNFDicTARSU.Visible = False : lblNFAccTARSU.Visible = False

            fldDicOSAP.Visible = False : fldAccOSAP.Visible = False : lblNFDicOSAP.Visible = False : lblNFAccOSAP.Visible = False

            fldPreAcc.Visible = True : fldAcc.Visible = True

            'Dichiarato ICI
            'objDataTable = FncGestAccert.RicercaDichiaratoICI(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"), constsession.idente, hfidprovvedimento.value)
            'objDataTable = FncGestAccert.RicercaDicAccICI("D", ConstSession.IdEnte, rowPROVVEDIMENTO("COD_CONTRIBUENTE"), Session("ANNO"), hfIdProvvedimento.Value, Session("ANNO"), ConstSession.StringConnection, "atto")
            objDataTable = FncGestAccert.RicercaDicAccICI("D", ConstSession.IdEnte, rowPROVVEDIMENTO("COD_CONTRIBUENTE"), rowPROVVEDIMENTO("ANNO"), hfIdProvvedimento.Value, rowPROVVEDIMENTO("ANNO"), ConstSession.StringConnection, "atto")
            If Not IsNothing(objDataTable) Then
                If objDataTable.GetLength(0) > 0 Then
                    GrdDichiaratoICI.DataSource = objDataTable
                    GrdDichiaratoICI.DataBind()
                    GrdDichiaratoICI.Visible = True
                Else
                    GrdDichiaratoICI.Visible = False
                    lblNFDichICI.Visible = True
                End If
            Else
                GrdDichiaratoICI.Visible = False
                lblNFDichICI.Visible = True
            End If
            'Accertato ICI
            'objDataTable = FncGestAccert.RicercaAccertatoICI(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"), constsession.idente, rowPROVVEDIMENTO("COD_CONTRIBUENTE"), Session("ANNO"), hfidprovvedimento.value, Session("ANNO"), "atto")
            objDataTable = FncGestAccert.RicercaDicAccICI("A", ConstSession.IdEnte, rowPROVVEDIMENTO("COD_CONTRIBUENTE"), Session("ANNO"), hfIdProvvedimento.Value, Session("ANNO"), ConstSession.StringConnection, "atto")
            If Not IsNothing(objDataTable) Then
                If objDataTable.GetLength(0) > 0 Then
                    Session("DataTableImmobili") = objDataTable
                    GrdAccertatoICI.DataSource = objDataTable
                    GrdAccertatoICI.DataBind()
                    GrdAccertatoICI.Visible = True
                Else
                    GrdAccertatoICI.Visible = False
                    lblNFAccICI.Visible = False
                End If
            Else
                GrdAccertatoICI.Visible = False
                lblNFAccICI.Visible = False
            End If
            Return True
        Catch ex As Exception
            sError = ex.Message
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.LoadDettaglioICI.errore: ", ex)
            'Response.Redirect("../../PaginaErrore.aspx")
            Return False
        End Try
    End Function
    '*** 20140701 - IMU/TARES ***
    'Private Function LoadDettaglioTARSU(ByVal rowPROVVEDIMENTO As DataRow, ByRef sError As String) As Boolean
    '    Try
    '        fldDicICI.Visible = False : fldAccICI.Visible = False : lblNFDichICI.Visible = False : lblNFAccICI.Visible = False

    '        fldDicTARSU.Visible = True : fldAccTARSU.Visible = True : lblNFDicTARSU.Visible = False : lblNFAccTARSU.Visible = False

    '        fldDicOSAP.Visible = False : fldAccOSAP.Visible = False : lblNFDicOSAP.Visible = False : lblNFAccOSAP.Visible = False

    '        fldPreAcc.Visible = True : fldAcc.Visible = True

    '        Dim oDichiarato() As OggettoArticoloRuolo
    '        Dim oAccertato() As OggettoArticoloRuolo
    '        'Dim oDichiarato() As OggettoArticoloRuoloAccertamento
    '        'Dim oAccertato() As OggettoArticoloRuoloAccertamento

    '        'Dichiarato TARSU
    '        oDichiarato = FncGestAccert.RicercaDichiaratoTARSU(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"), hfidprovvedimento.value)
    '        If Not IsNothing(oDichiarato) Then
    '            If oDichiarato(0).IdLegame <> 0 Then
    '                Session("oDichiaratoTARSU") = oDichiarato
    '                GrdDichiaratoTARSU.start_index = 0
    '                GrdDichiaratoTARSU.DataSource = oDichiarato
    '                GrdDichiaratoTARSU.DataBind()
    '            End If
    '        Else
    '            GrdDichiaratoTARSU.Visible = False
    '            lblNFDicTARSU.Visible = True
    '        End If

    '        'Accertato TARSU
    '        Dim bAnnoMod As Boolean
    '        oAccertato = FncGestAccert.RicercaAccertatoTARSU(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"), hfidprovvedimento.value, bAnnoMod, -1, True)
    '        If Not IsNothing(oAccertato) Then
    '            Session("oAccertatoTARSU") = oAccertato
    '            GrdAccertatoTARSU.start_index = 0
    '            GrdAccertatoTARSU.DataSource = oAccertato
    '            GrdAccertatoTARSU.DataBind()
    '        Else
    '            GrdAccertatoTARSU.Visible = False
    '            lblNFAccTARSU.Visible = True
    '        End If

    '        Return True
    '    Catch ex As Exception
    '        sError = ex.Message
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.LoadDettaglioTARSU.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '        Return False
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="rowPROVVEDIMENTO"></param>
    ''' <param name="sError"></param>
    ''' <returns></returns>
    Private Function LoadDettaglioTARSU(ByVal rowPROVVEDIMENTO As DataRow, ByRef sError As String) As Boolean
        Try
            fldDicICI.Visible = False : fldAccICI.Visible = False : lblNFDichICI.Visible = False : lblNFAccICI.Visible = False

            fldDicTARSU.Visible = True : fldAccTARSU.Visible = True : lblNFDicTARSU.Visible = False : lblNFAccTARSU.Visible = False

            fldDicOSAP.Visible = False : fldAccOSAP.Visible = False : lblNFDicOSAP.Visible = False : lblNFAccOSAP.Visible = False

            fldPreAcc.Visible = True : fldAcc.Visible = True

            Dim oDichiarato() As ObjArticoloAccertamento
            Dim oAccertato() As ObjArticoloAccertamento
            'Dim oDichiarato() As OggettoArticoloRuoloAccertamento
            'Dim oAccertato() As OggettoArticoloRuoloAccertamento

            'Dichiarato TARSU
            Log.Debug("carico dichiarato")
            oDichiarato = FncGestAccert.TARSU_RicercaDichiarato(ConstSession.StringConnection, hfIdProvvedimento.Value)
            If Not IsNothing(oDichiarato) Then
                If oDichiarato.GetUpperBound(0) >= 0 Then
                    Session("oDichiaratoTARSU") = oDichiarato
                    GrdDichiaratoTARSU.DataSource = oDichiarato
                    GrdDichiaratoTARSU.DataBind()
                    If oDichiarato(0).sNote <> ObjRuolo.TipoCalcolo.TARES Then
                        GrdDichiaratoTARSU.Columns(12).HeaderText = "Bim"
                        GrdDichiaratoTARSU.Columns(7).Visible = False
                        GrdDichiaratoTARSU.Columns(8).Visible = False
                        GrdDichiaratoTARSU.Columns(9).Visible = False
                    Else
                        GrdDichiaratoTARSU.Columns(12).HeaderText = "GG"
                    End If
                Else
                    GrdDichiaratoTARSU.Visible = False
                    lblNFDicTARSU.Visible = True
                End If
            Else
                GrdDichiaratoTARSU.Visible = False
                lblNFDicTARSU.Visible = True
            End If

            'Accertato TARSU
            Dim bAnnoMod As Boolean
            Log.Debug("carico accertato")
            oAccertato = FncGestAccert.TARSU_RicercaAccertato(ConstSession.StringConnection, hfIdProvvedimento.Value, bAnnoMod, -1, True, "", False, False)
            If Not IsNothing(oAccertato) Then
                Session("oAccertatoTARSU") = oAccertato
                GrdAccertatoTARSU.DataSource = oAccertato
                GrdAccertatoTARSU.DataBind()
                If oAccertato(0).sNote <> ObjRuolo.TipoCalcolo.TARES Then
                    GrdAccertatoTARSU.Columns(12).HeaderText = "Bim"
                    GrdAccertatoTARSU.Columns(7).Visible = False
                    GrdAccertatoTARSU.Columns(8).Visible = False
                    GrdAccertatoTARSU.Columns(9).Visible = False
                Else
                    GrdAccertatoTARSU.Columns(12).HeaderText = "GG"
                End If
            Else
                GrdAccertatoTARSU.Visible = False
                lblNFAccTARSU.Visible = True
            End If

            Return True
        Catch ex As Exception
            sError = ex.Message
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.LoadDettaglioTARSU.errore: ", ex)
            'Response.Redirect("../../PaginaErrore.aspx")
            Return False
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="rowPROVVEDIMENTO"></param>
    ''' <param name="sError"></param>
    ''' <returns></returns>
    Private Function LoadDettaglioOSAP(ByVal rowPROVVEDIMENTO As DataRow, ByRef sError As String) As Boolean
        Try
            fldDicICI.Visible = False : fldAccICI.Visible = False : lblNFDichICI.Visible = False : lblNFAccICI.Visible = False

            fldDicTARSU.Visible = False : fldAccTARSU.Visible = False : lblNFDicTARSU.Visible = False : lblNFAccTARSU.Visible = False

            fldDicOSAP.Visible = True : fldAccOSAP.Visible = True : lblNFDicOSAP.Visible = False : lblNFAccOSAP.Visible = False

            fldPreAcc.Visible = True : fldAcc.Visible = True

            Dim oListDichiarato() As OSAPAccertamentoArticolo
            Dim oListAccertato() As OSAPAccertamentoArticolo

            'Dichiarato OSAP
            oListDichiarato = FncGestAccert.OSAPRicercaArticoliDichAcc("D", hfIdProvvedimento.Value)
            If Not IsNothing(oListDichiarato) Then
                If oListDichiarato.Length > 0 Then
                    If oListDichiarato(0).IdLegame <> 0 Then
                        Session("oListDichiaratoOSAP") = oListDichiarato
                        GrdDichiaratoOSAP.DataSource = oListDichiarato
                        GrdDichiaratoOSAP.DataBind()
                    End If
                Else
                    GrdDichiaratoOSAP.Visible = False
                    lblNFDicOSAP.Visible = True
                End If
            Else
                GrdDichiaratoOSAP.Visible = False
                lblNFDicOSAP.Visible = True
            End If

            'Accertato OSAP
            oListAccertato = FncGestAccert.OSAPRicercaArticoliDichAcc("A", hfIdProvvedimento.Value)
            If Not IsNothing(oListAccertato) Then
                If oListAccertato.Length > 0 Then
                    Session("oListAccertatoOSAP") = oListAccertato
                    GrdAccertatoOSAP.DataSource = oListAccertato
                    GrdAccertatoOSAP.DataBind()
                Else
                    GrdAccertatoOSAP.Visible = False
                    lblNFAccOSAP.Visible = True
                End If
            Else
                GrdAccertatoOSAP.Visible = False
                lblNFAccOSAP.Visible = True
            End If
            Return True
        Catch ex As Exception
            sError = ex.Message
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.LoadDettaglioOSAP.errore: ", ex)
            'Response.Redirect("../../PaginaErrore.aspx")
            Return False
        End Try
    End Function
    Private Sub LoadAtto(IdProvvedimento As Integer)
        Dim objCOMRicerca As IElaborazioneAtti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAtti), ConstSession.URLServiziElaborazioneAtti)
        Dim strTIPO_PROCEDIMENTO, strWFErrore As String
        Dim dt As New DataView
        Dim dblImpTotAvvisoRidotto, dblImpTotAvvisoPieno As Double
        Dim objHashTable As New Hashtable
        Try
            objHashTable.Add("IDPROVVEDIMENTO", IdProvvedimento.ToString)
            objHashTable.Add("CODENTE", ConstSession.IdEnte)
            objHashTable.Add("CODTRIBUTO", ConstSession.CodTributo)
            objHashTable.Add("PARAMETROORDINAMENTOGRIGLIA", "")
            objHashTable.Add("CONNECTIONSTRINGOPENGOVICI", ConstSession.StringConnectionICI)
            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
            objDS = objCOMRicerca.GetDatiProvvedimenti(ConstSession.StringConnection, objHashTable, myAtto)

            Session.Remove("PROVVEDIMENTI_DA_STAMPARE")
            Session.Add("PROVVEDIMENTI_DA_STAMPARE", objDS)
            txtTipoTributo.Text = Replace(Utility.StringOperation.FormatString(Request("DESCTRIBUTO")), """", "'")
            txtNumeroProvvedimento.Text = Utility.StringOperation.FormatString(Request("NUMEROATTO"))
            txtTipoProvvedimento.Text = Utility.StringOperation.FormatString(Request("TIPOPROVVEDIMENTO"))
            txtAnno.Text = Utility.StringOperation.FormatString(Request("ANNO"))
            strTIPO_PROCEDIMENTO = Utility.StringOperation.FormatString(Request("TIPOPROCEDIMENTO"))
            Session("TIPOPROCEDIMENTO") = Utility.StringOperation.FormatString(Request("TIPOPROCEDIMENTO"))
            lblTributo.Text = Replace(Utility.StringOperation.FormatString(Request("DESCTRIBUTO")), """", "'")
            lblAnnoAvviso.Text = Utility.StringOperation.FormatString(Request("ANNO"))
            If strTIPO_PROCEDIMENTO.CompareTo(LIQUIDAZIONE) = 0 Then
                txtOPEN_RETTIFICA.Text = "1"
            End If
            If strTIPO_PROCEDIMENTO.CompareTo(ACCERTAMENTO) = 0 Then
                txtOPEN_RETTIFICA.Text = "2"
            End If
            If objDS.Tables("PROVVEDIMENTO").Rows.Count = 1 Then
                Dim rowPROVVEDIMENTO As DataRow = objDS.Tables("PROVVEDIMENTO").Rows(0)

                objHashTable.Add("NUMERO_ATTO", Utility.StringOperation.FormatString(rowPROVVEDIMENTO("NUMERO_ATTO")))
                objHashTable.Add("ANNO_AVVISO", Utility.StringOperation.FormatString(rowPROVVEDIMENTO("ANNO")))
                objHashTable.Add("COD_CONTRIBUENTE", Utility.StringOperation.FormatString(rowPROVVEDIMENTO("COD_CONTRIBUENTE")))

                hdIdContribuente.Value = Utility.StringOperation.FormatString(rowPROVVEDIMENTO("COD_CONTRIBUENTE"))
                Log.Debug("..." & hfIdProvvedimento.Value)
                hfIdProvvedimento.Value = Utility.StringOperation.FormatString(rowPROVVEDIMENTO("ID_PROVVEDIMENTO"))
                Session("IDPROVVEDIMENTO") = hfIdProvvedimento.Value
                txtCOD_TRIBUTO.Text = Utility.StringOperation.FormatString(rowPROVVEDIMENTO("COD_TRIBUTO"))

                txtANNO_RETTIFICA.Text = txtAnno.Text
                lblNumeroAvviso.Text = Utility.StringOperation.FormatString(rowPROVVEDIMENTO("NUMERO_ATTO"))
                txtTIPO_OPERAZIONE.Text = "RETTIFICA"
                TxtNomePDF.Text = Utility.StringOperation.FormatString(rowPROVVEDIMENTO("nomepdf"))
                txtTIPO_PROCEDIMENTO.Text = Utility.StringOperation.FormatString(Request("TIPOPROCEDIMENTO")).ToUpper
                '*** gestione anagrafica ***
                If LoadAnagrafica(rowPROVVEDIMENTO, strWFErrore) = False Then
                    Log.Debug("Errore in carico anagrafe:: " & strWFErrore)
                End If
                '*** gestione importi ***
                If LoadImporti(rowPROVVEDIMENTO, dblImpTotAvvisoRidotto, dblImpTotAvvisoPieno, strWFErrore) = False Then
                    Log.Debug("Errore in carico importi:: " & strWFErrore)
                End If
                '*** ***
                '*** gestione date ***
                If LoadDateNote(rowPROVVEDIMENTO, strWFErrore) = False Then
                    Log.Debug("Errore in carico date:: " & strWFErrore)
                End If
                '**** gestione pagamenti ****
                If LoadPagamenti(rowPROVVEDIMENTO, dblImpTotAvvisoRidotto, dblImpTotAvvisoPieno, strWFErrore) = False Then
                    Log.Debug("Errore in carico pagamenti:: " & strWFErrore)
                End If
                '*** gestione dettaglio atto ***
                Select Case txtCOD_TRIBUTO.Text
                    Case Utility.Costanti.TRIBUTO_ICI, Utility.Costanti.TRIBUTO_TASI
                        If LoadDettaglioICI(rowPROVVEDIMENTO, strWFErrore) = False Then
                            Log.Debug("Errore in carico dettaglio:: " & strWFErrore)
                        End If
                    Case Utility.Costanti.TRIBUTO_TARSU
                        If LoadDettaglioTARSU(rowPROVVEDIMENTO, strWFErrore) = False Then
                            Log.Debug("Errore in carico dettaglio:: " & strWFErrore)
                        End If
                    Case Utility.Costanti.TRIBUTO_OSAP
                        If LoadDettaglioOSAP(rowPROVVEDIMENTO, strWFErrore) = False Then
                            Log.Debug("Errore in carico dettaglio:: " & strWFErrore)
                        End If
                End Select
                '*** Gestione Riepilogo atto ***
                If LoadRiepilogoImporti(rowPROVVEDIMENTO, strWFErrore) = False Then
                    Log.Debug("Errore in carico riepilogo:: " & strWFErrore)
                End If
                LoadRateizzato(Utility.StringOperation.FormatInt(hfIdProvvedimento.Value))
                '*** per la rettifica ***
                Dim myHasTbl As New Hashtable
                If myHasTbl.ContainsKey("DATA_ELABORAZIONE_PER_RETTIFICA") Then
                    myHasTbl.Remove("DATA_ELABORAZIONE_PER_RETTIFICA")
                    myHasTbl.Add("DATA_ELABORAZIONE_PER_RETTIFICA", objUtility.GiraDataFromDB(Session("DATA_ELABORAZIONE_PER_RETTIFICA")))
                Else
                    myHasTbl.Add("DATA_ELABORAZIONE_PER_RETTIFICA", objUtility.GiraDataFromDB(Session("DATA_ELABORAZIONE_PER_RETTIFICA")))
                End If
                myHasTbl.Add("DATA_ELABORAZIONE", objUtility.GiraData(txtDATA_ELABORAZIONE.Text))
                myHasTbl.Add("COD_TRIBUTO", txtCOD_TRIBUTO.Text)
                myHasTbl.Add("ID_PROVVEDIMENTO_RETTIFICA", hfIdProvvedimento.Value)
                myHasTbl.Add("DATA_RETTIFICA", DateTime.Now.ToString("yyyyMMdd"))
                myHasTbl.Add("TIPO_OPERAZIONE_RETTIFICA", True)
                myHasTbl.Add("CODCONTRIBUENTE", hdIdContribuente.Value)
                myHasTbl.Add("ANNOACCERTAMENTO", txtANNO_RETTIFICA.Text)
                Session("HashTableRettificaAccertamenti") = myHasTbl
            End If
            Log.Debug("......")
            If Not objDS Is Nothing Then
                dt = objDS.Tables("PROVVEDIMENTO").DefaultView
            End If
            GrdDettaglioAvviso.DataSource = dt
            GrdDettaglioAvviso.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GestioneAtti.LoadAtto.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class

