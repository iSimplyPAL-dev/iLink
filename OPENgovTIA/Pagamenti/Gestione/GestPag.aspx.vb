'*** 20120921 - pagamenti ***
'Imports RemotingInterfaceMotoreTarsu.RemotingInterfaceMotoreTarsu
Imports RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti
Imports AnagInterface
Imports log4net
Imports System.Configuration
Imports System.Globalization
''' <summary>
''' Pagina per la visualizzazione/gestione del pagamento.
''' Contiene i parametri di dettaglio e le funzioni della comandiera.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
''' <revisionHistory>
''' <revision date="26/11/2018">
''' <strong>Insoluti e Coattivo</strong>
''' <list type="bullet">
'''   <item>
'''   <em>Abbinamento Pagamenti</em>
''' La query di ricerca emesso filtrerà anche per data pagamento con le stesse logiche dell'importazione.
'''   </item>
'''   <item>
'''   <em>Data Entry Pagamenti</em>
'''   La query di ricerca emesso filtrerà con le stesse logiche dell'importazione, utilizzando come data di pagamento la data odierna. Al momento del salvataggio il sistema controllerà l’effettiva data di pagamento inserita e applicherà i ragionamenti fatti in fase di importazione, quindi se la data di pagamento sarà entro i termini il sistema si abbinerà sull’ordinario cancellando data di accertamento ed ingiunzione.
'''   </item>
''' </list>
''' </revision>
''' </revisionHistory>
''' <revisionHistory>
''' <revision date="12/04/2019">
''' <strong>Qualificazione AgID-analisi_rel01</strong>
''' <em>Analisi eventi</em>
''' </revision>
''' </revisionHistory>
Partial Class GestPag
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger("GesPagamenti")
    Private IdTributo As String = ""
    Private myStringConnection As String = ""
    Protected FncGrd As New Formatta.FunctionGrd

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
        lblTitolo.Text = ConstSession.DescrizioneEnte
        Dim IdTributo As String = Request.Item("TRIBUTO")
        Try
            If IdTributo = Utility.Costanti.TRIBUTO_OSAP Then
                info.InnerText = "TOSAP/COSAP "
            ElseIf IdTributo = Utility.Costanti.TRIBUTO_SCUOLE Then
                info.InnerText = "SCUOLA "
            Else
                If ConstSession.IsFromTARES = "1" Then
                    info.InnerText = "TARI "
                Else
                    info.InnerText = "TARSU "
                End If
            End If
            If ConstSession.IsFromVariabile() = "1" Then
                info.InnerText += "Variabile"
            End If
            info.InnerText += " - Pagamenti - Gestione"
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestPag.Page_Init.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub

#End Region
    ''' <summary>
    ''' Caricamento della pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim sScript As String = ""
            IdTributo = Request.Item("TRIBUTO")
            If IdTributo = Utility.Costanti.TRIBUTO_OSAP Or IdTributo = Utility.Costanti.TRIBUTO_SCUOLE Then
                myStringConnection = ConstSession.StringConnectionOSAP
                LblInfoMagg.Style.Add("display", "none")
                LblMaggiorazione.Style.Add("display", "none")
                txtImportoMagg.Style.Add("display", "none")
            Else
                myStringConnection = ConstSession.StringConnection
                LblInfoMagg.Style.Add("display", "")
                LblMaggiorazione.Style.Add("display", "")
                txtImportoMagg.Style.Add("display", "")
            End If
            txtNAvviso.Attributes.Add("onkeydown", "keyPress();")
            txtAnnoVersamento.Attributes.Add("onkeydown", "keyPress();")
            LnkPulisciContr.Attributes.Add("onclick", "return ClearDatiContrib();")
            txtRicCFPIva.Attributes.Add("onkeydown", "keyPressAbb();")
            txtRicCognome.Attributes.Add("onkeydown", "keyPressAbb();")
            txtRicNome.Attributes.Add("onkeydown", "keyPressAbb();")
            txtRicCodCartella.Attributes.Add("onkeydown", "keyPressAbb();")
            txtRicIdOperazione.Attributes.Add("onkeydown", "keyPressAbb();")
            txtRicImpPagato.Attributes.Add("onkeydown", "keyPressAbb();")
            'sScript += "$('#Modifica').addClass('DisableBtn');"
            'sScript += "$('#Cancella').addClass('DisableBtn');"
            'sScript += "$('#Salva').addClass('DisableBtn');"
            If Page.IsPostBack = False Then
                If hdIdContribuente.Value = "-1" Then
                    Session("oAnagrafe") = Nothing
                End If
                If Not Request.Item("NonAbb") Is Nothing Then
                    If Request.Item("NonAbb") = True Then
                        LoadNonAbb(IdTributo, myStringConnection)
                    Else
                        Log.Debug("load pag")
                        LoadPag(myStringConnection)
                    End If
                Else
                    Log.Debug("no nonabb load pag")
                    LoadPag(myStringConnection)
                End If
                Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Pagamento, "Gestione", Utility.Costanti.AZIONE_LETTURA.ToString, ConstSession.CodTributo, ConstSession.IdEnte, Request.Item("IdListPagamento"))
            Else
                Log.Debug("carico anag")
                If ConstSession.HasPlainAnag Then
                    ifrmAnag.Attributes.Add("src", "../../../Generali/asp/VisualAnag.aspx?IdContribuente=" & hdIdContribuente.Value & "&Azione=" & Utility.Costanti.AZIONE_NEW)
                End If
                controlloSessione(rdbDataEntry.Checked, rdbDaCartellazione.Checked)
            End If
            '*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI & DIVERSA GESTIONE QUOTA VARIABILE***
            If ConstSession.HasPlainAnag Then
                sScript += "document.getElementById('TRSpecAnag').style.display='none';"
            Else
                sScript += "document.getElementById('TRPlainAnag').style.display='none';"
            End If
            RegisterScript(sScript, Me.GetType)
            '*** ***
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPag.Page_Load.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Pulsante per il caricamento delle rate per il contribuente/avviso
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnTrovaRate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTrovaRate.Click
        GetCartellePag(IdTributo, myStringConnection)
    End Sub
    ''' <summary>
    ''' Pulsante per il salvataggio dei dati in memoria
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="06/2018">
    ''' <strong>miglior gestione inserimento più pagamenti</strong>
    ''' </revision>
    ''' </revisionHistory>
    Private Sub btnSalvaDati_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalvaDati.Click
        Dim FncGestPag As New ClsGestPag
        Dim oListPag() As OggettoPagamenti
        Dim bCheck As Boolean = True
        Dim ListPag As New ArrayList
        Dim oMyPagamento As New OggettoPagamenti
        Dim sScript As String = ""

        Try
            '*** da cartellazione
            controlloSessione(rdbDataEntry.Checked, rdbDaCartellazione.Checked)

            If Session("TipoOpzione") = "DaCartellazione" Then
                'controllo che ci siano tutti i dati obbligatori
                For Each myRow As GridViewRow In GrdPagamenti.Rows
                    If CType(myRow.FindControl("ckbSelezione"), CheckBox).Checked = True Then
                        If CType(myRow.FindControl("txtDataPagamento"), TextBox).Text = "" Then
                            sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario valorizzare il campo Data Pagamento!');"
                            RegisterScript(sScript, Me.GetType)
                            Exit Sub
                        End If
                        If CType(myRow.FindControl("txtTotalePagamento"), TextBox).Text = "" Then
                            sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario valorizzare il campo Importo Pagamento!');"
                            RegisterScript(sScript, Me.GetType)
                            Exit Sub
                        End If
                        'ciclo sulla griglia per popolarmi i dati da inserire
                        oMyPagamento = New OggettoPagamenti
                        oMyPagamento.sAnno = myRow.Cells(1).Text.ToString()
                        oMyPagamento.sNumeroAvviso = myRow.Cells(2).Text.ToString()
                        oMyPagamento.IdEnte = ConstSession.IdEnte
                        oMyPagamento.IdContribuente = CType(myRow.FindControl("hfidcontribuente"), HiddenField).Value
                        oMyPagamento.sNumeroRata = myRow.Cells(3).Text.ToString()
                        If CType(myRow.FindControl("txtDataScadenza"), TextBox).Text <> "" Then
                            oMyPagamento.tDataScadenza = CType(myRow.FindControl("txtDataScadenza"), TextBox).Text
                        End If
                        oMyPagamento.tDataPagamento = CType(myRow.FindControl("txtDataPagamento"), TextBox).Text
                        oMyPagamento.dImportoPagamento = CType(myRow.FindControl("txtTotalePagamento"), TextBox).Text
                        If CType(myRow.FindControl("txtPagMagg"), TextBox).Text <> "" Then
                            oMyPagamento.dImportoStat = CType(myRow.FindControl("txtPagMagg"), TextBox).Text
                        End If
                        oMyPagamento.sProvenienza = TxtProvenienza.Text
                        oMyPagamento.sTipoPagamento = ""
                        oMyPagamento.sNote = ""
                        oMyPagamento.ID = CType(myRow.FindControl("hfid"), HiddenField).Value
                        oMyPagamento.IDFlusso = CType(myRow.FindControl("hfIdFlusso"), HiddenField).Value
                        oMyPagamento.sOperatore = ConstSession.UserName
                        oMyPagamento.tDataInsert = Now
                        If CType(myRow.FindControl("txtDataAccredito"), TextBox).Text <> "" Then
                            oMyPagamento.tDataAccredito = CType(myRow.FindControl("txtDataAccredito"), TextBox).Text
                        Else
                            oMyPagamento.tDataAccredito = Date.MinValue
                        End If
                        ListPag.Add(oMyPagamento)
                        'controllo la congruenza dell'importo rata
                        If FncGestPag.CheckPagVSCartella(myStringConnection, 0, IdTributo, oMyPagamento) = False Then
                            sScript = "if (confirm('Attenzione!\nImporto rata diverso da quello emesso\nSi vuole continuare?'))"
                            sScript += "{document.getElementById('CmdSalvaPag').click()}"
                        Else
                            'controllo la congruenza del totale pagato da fare solo se nuovo inserimento
                            If oMyPagamento.ID = -1 Then
                                bCheck = FncGestPag.CheckPagVSCartella(myStringConnection, 1, IdTributo, oMyPagamento)
                            End If
                            If bCheck = False Then
                                sScript = "if (confirm('Attenzione!\nImporto maggiore da quello emesso\nSi vuole continuare?'))"
                                sScript += "{document.getElementById('CmdSalvaPag').click()}"
                            End If
                        End If
                    End If
                Next
                If ListPag.Count = 0 Then
                    sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario selezionare una rata!');"
                    RegisterScript(sScript, Me.GetType)
                    Exit Sub
                Else
                    If sScript = "" Then
                        sScript = "document.getElementById('CmdSalvaPag').click()"
                    End If
                    RegisterScript(sScript, Me.GetType)
                End If
            Else
                '*** data entry manuale
                If hdIdContribuente.Value <> -1 And TxtAnno.Text <> "" And txtDataPag.Text <> "" And txtImportoPag.Text <> "" And txtNAvvisoDE.Text <> "" Then
                    oListPag = FncGestPag.GetCartellePagamenti(myStringConnection, ConstSession.IdEnte, IdTributo, CInt(hdIdContribuente.Value), txtNAvvisoDE.Text, TxtAnno.Text)
                    If oListPag Is Nothing Then
                        sScript = "GestAlert('a', 'warning', '', '', 'Numero avviso errato, insesistente o associato ad un altro contribuente/anno!');"
                        RegisterScript(sScript, Me.GetType)
                        Exit Sub
                    End If
                    oMyPagamento = New OggettoPagamenti
                    oMyPagamento.sAnno = TxtAnno.Text
                    oMyPagamento.sNumeroAvviso = txtNAvvisoDE.Text
                    oMyPagamento.IdEnte = ConstSession.IdEnte
                    oMyPagamento.IdContribuente = hdIdContribuente.Value
                    oMyPagamento.sNumeroRata = txtNRata.Text
                    oMyPagamento.dImportoPagamento = txtImportoPag.Text
                    If txtImportoMagg.Text <> "" Then
                        oMyPagamento.dImportoStat = txtImportoMagg.Text
                    End If
                    oMyPagamento.tDataPagamento = txtDataPag.Text
                    oMyPagamento.sProvenienza = TxtProvenienza.Text
                    oMyPagamento.ID = TxtIdPag.Text
                    oMyPagamento.sOperatore = ConstSession.UserName
                    If txtDataAccreditoDE.Text <> "" Then
                        oMyPagamento.tDataAccredito = txtDataAccreditoDE.Text
                    Else
                        oMyPagamento.tDataAccredito = Date.MinValue
                    End If
                    oMyPagamento.tDataInsert = Now
                    ListPag.Add(oMyPagamento)
                    'controllo la congruenza del totale pagato da fare solo se nuovo inserimento
                    If oMyPagamento.ID = -1 Then
                        bCheck = FncGestPag.CheckPagVSCartella(myStringConnection, 1, IdTributo, oMyPagamento)
                    End If
                    If bCheck = False Then
                        sScript = "if (confirm('Attenzione!\nImporto maggiore da quello emesso\nSi vuole continuare?'))"
                        sScript += "{document.getElementById('CmdSalvaPag').click()}"
                    Else
                        sScript += "document.getElementById('CmdSalvaPag').click()"
                    End If
                    RegisterScript(sScript, Me.GetType)
                Else
                    sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario valorizzare i campi Anno, Numero Avviso, Data Pagamento, Importo Pagato e selezionare un Contribuente');"
                    RegisterScript(sScript, Me.GetType)
                    Exit Sub
                End If
            End If
            Session("oMyPagamento") = ListPag
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPag.btnSalvaDati_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub btnSalvaDati_click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalvaDati.Click
    '    Dim IsChecked As Integer = 0
    '    'Dim TabellaTotali As New DataTable("RATE_PAGAMENTI")
    '    Dim FncPagamenti As New ClsGestPag
    '    Dim idPagamento As Integer = 0
    '    'Dim nSalvati As Integer = 0
    '    'Dim nDaSalvare As Integer = 0
    '    Dim ModPagamento As String = ""
    '    Dim FncGestPag As New ClsGestPag
    '    Dim oListPag() As OggettoPagamenti
    '    Dim bCheck As Boolean = True

    '    Try
    '        ''inizializzo la connessione
    '        'WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        'If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '        '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        'End If
    '        '*** da cartellazione
    '        controlloSessione(rdbDataEntry.Checked, rdbDaCartellazione.Checked)

    '        If Session("TipoOpzione") = "DaCartellazione" Then
    '            'controllo che ci siano tutti i dati obbligatori
    '            For Each myRow As GridViewRow In GrdPagamenti.Rows
    '                If CType(myRow.FindControl("ckbSelezione"), CheckBox).Checked = True Then
    '                    IsChecked = 1
    '                    If CType(myRow.FindControl("txtDataPagamento"), TextBox).Text = "" Then
    '                        sScript = "alert('E\' necessario valorizzare il campo Data Pagamento!');"
    '                        RegisterScript(sScript, Me.GetType)
    '                        Exit Sub
    '                    End If
    '                    If CType(myRow.FindControl("txtTotalePagamento"), TextBox).Text = "" Then
    '                        sScript = "alert('E\' necessario valorizzare il campo Importo Pagamento!');"
    '                        RegisterScript(sScript, Me.GetType)
    '                        Exit Sub
    '                    End If
    '                    'ciclo sulla griglia per popolarmi i dati da inserire
    '                    oMyPagamento = New OggettoPagamenti
    '                    oMyPagamento.sAnno = myRow.Cells(1).Text.ToString()
    '                    oMyPagamento.sNumeroAvviso = myRow.Cells(2).Text.ToString()
    '                    oMyPagamento.IdEnte = ConstSession.IdEnte
    '                    oMyPagamento.IdContribuente = CType(myRow.FindControl("hfidcontribuente"), HiddenField).Value
    '                    oMyPagamento.sNumeroRata = myRow.Cells(3).Text.ToString()
    '                    'oMyPagamento.dImportoRata = GrdPagamenti.Items(x).Cells(4).Text.ToString()
    '                    If CType(myRow.FindControl("txtDataScadenza"), TextBox).Text <> "" Then
    '                        oMyPagamento.tDataScadenza = CType(myRow.FindControl("txtDataScadenza"), TextBox).Text
    '                    End If
    '                    oMyPagamento.tDataPagamento = CType(myRow.FindControl("txtDataPagamento"), TextBox).Text
    '                    oMyPagamento.dImportoPagamento = CType(myRow.FindControl("txtTotalePagamento"), TextBox).Text
    '                    If CType(myRow.FindControl("txtPagMagg"), TextBox).Text <> "" Then
    '                        oMyPagamento.dImportoStat = CType(myRow.FindControl("txtPagMagg"), TextBox).Text
    '                    End If
    '                    oMyPagamento.sProvenienza = TxtProvenienza.Text
    '                    oMyPagamento.sTipoPagamento = ""
    '                    oMyPagamento.sNote = ""
    '                    oMyPagamento.ID = CType(myRow.FindControl("hfid"), HiddenField).Value
    '                    oMyPagamento.sOperatore = ConstSession.UserName
    '                    oMyPagamento.tDataInsert = Now

    '                    If CType(myRow.FindControl("txtDataAccredito"), TextBox).Text <> "" Then
    '                        oMyPagamento.tDataAccredito = CType(myRow.FindControl("txtDataAccredito"), TextBox).Text
    '                    Else
    '                        oMyPagamento.tDataAccredito = Date.MinValue
    '                    End If
    '                    'controllo la congruenza dell'importo rata
    '                    If FncGestPag.CheckPagVSCartella(myStringConnection, 0, IdTributo, oMyPagamento) = False Then
    '                        sScript = "if (confirm('Attenzione!\nImporto rata diverso da quello emesso\nSi vuole continuare?'))"
    '                        sScript += "{document.formRicercaAnagrafica.CmdSalvaPag.click()}"
    '                    Else
    '                        'controllo la congruenza del totale pagato da fare solo se nuovo inserimento
    '                        If oMyPagamento.ID = -1 Then
    '                            bCheck = FncGestPag.CheckPagVSCartella(myStringConnection, 1, IdTributo, oMyPagamento)
    '                        End If
    '                        If bCheck = False Then
    '                            sScript = "if (confirm('Attenzione!\nImporto maggiore da quello emesso\nSi vuole continuare?'))"
    '                            sScript += "{document.formRicercaAnagrafica.CmdSalvaPag.click()}"
    '                        Else
    '                            sScript += "document.formRicercaAnagrafica.CmdSalvaPag.click()"
    '                        End If
    '                    End If
    '                    RegisterScript(sScript, Me.GetType)
    '                End If
    '            Next
    '            If IsChecked = 0 Then
    '                sScript = "alert('E\' necessario selezionare una rata!');"
    '                RegisterScript(sScript, Me.GetType)
    '                Exit Sub
    '            End If
    '        Else
    '            '*** data entry manuale
    '            If hdIdContribuente.Value <> -1 And TxtAnno.Text <> "" And txtDataPag.Text <> "" And txtImportoPag.Text <> "" And txtNAvvisoDE.Text <> "" Then
    '                oListPag = FncGestPag.GetCartellePagamenti(myStringConnection, ConstSession.IdEnte, IdTributo, CInt(hdIdContribuente.Value), txtNAvvisoDE.Text, TxtAnno.Text)
    '                If oListPag Is Nothing Then
    '                    sScript = "alert('Numero avviso errato, insesistente o associato ad un altro contribuente/anno!');"
    '                    RegisterScript(sScript, Me.GetType)
    '                    Exit Sub
    '                End If
    '                oMyPagamento = New OggettoPagamenti
    '                oMyPagamento.sAnno = TxtAnno.Text
    '                oMyPagamento.sNumeroAvviso = txtNAvvisoDE.Text
    '                oMyPagamento.IdEnte = ConstSession.IdEnte
    '                oMyPagamento.IdContribuente = hdIdContribuente.Value
    '                oMyPagamento.sNumeroRata = txtNRata.Text
    '                oMyPagamento.dImportoPagamento = txtImportoPag.Text
    '                If txtImportoMagg.Text <> "" Then
    '                    oMyPagamento.dImportoStat = txtImportoMagg.Text
    '                End If
    '                oMyPagamento.tDataPagamento = txtDataPag.Text
    '                oMyPagamento.sProvenienza = TxtProvenienza.Text
    '                oMyPagamento.ID = TxtIdPag.Text
    '                oMyPagamento.sOperatore = ConstSession.UserName
    '                If txtDataAccreditoDE.Text <> "" Then
    '                    oMyPagamento.tDataAccredito = txtDataAccreditoDE.Text
    '                Else
    '                    oMyPagamento.tDataAccredito = Date.MinValue
    '                End If
    '                oMyPagamento.tDataInsert = Now
    '                'controllo la congruenza del totale pagato da fare solo se nuovo inserimento
    '                If oMyPagamento.ID = -1 Then
    '                    bCheck = FncGestPag.CheckPagVSCartella(myStringConnection, 1, IdTributo, oMyPagamento)
    '                End If
    '                If bCheck = False Then
    '                    sScript = "if (confirm('Attenzione!\nImporto maggiore da quello emesso\nSi vuole continuare?'))"
    '                    sScript += "{document.formRicercaAnagrafica.CmdSalvaPag.click()}"
    '                Else
    '                    sScript += "document.formRicercaAnagrafica.CmdSalvaPag.click()"
    '                End If
    '                RegisterScript(sScript, Me.GetType)
    '            Else
    '                sScript = "alert('E\' necessario valorizzare i campi Anno, Numero Avviso, Data Pagamento, Importo Pagato e selezionare un Contribuente');"
    '                RegisterScript(sScript, Me.GetType)
    '                Exit Sub
    '            End If
    '        End If
    '        Session("oMyPagamento") = oMyPagamento
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPag.btnSalvaDati_Click.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    Finally
    '        'If Not IsNothing(WFSessione) Then
    '        '    WFSessione.Kill()
    '        '    WFSessione = Nothing
    '        'End If
    '    End Try
    'End Sub
    'Private Sub CmdSalvaPag_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdSalvaPag.Click
    '    'Dim x As Integer ', k
    '    Dim IsChecked As Integer = 0
    '    'Dim TabellaTotali As New DataTable("RATE_PAGAMENTI")
    '    Dim FncPagamenti As New ClsGestPag
    '    Dim idPagamento As Integer = 0
    '    'Dim nSalvati As Integer = 0
    '    'Dim nDaSalvare As Integer = 0
    '    Dim ModPagamento As String = ""
    '    Dim FncGestPag As New ClsGestPag
    '    'Dim oListPag() As OggettoPagamenti

    '    Try
    '        ''inizializzo la connessione
    '        'WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        'If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '        '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        'End If
    '        oMyPagamento = Session("oMyPagamento")
    '        If oMyPagamento.ID = -1 Then
    '            'Nuovo inserimento
    '            idPagamento = FncPagamenti.SetPagamento(myStringConnection, oMyPagamento, 0)
    '        Else
    '            'controllo se sono stati modificati dei dati
    '            ModPagamento = hdIdContribuente.Value & "|" & TxtAnno.Text & "|" & txtNAvvisoDE.Text & "|" & txtNRata.Text & "|" & txtDataPag.Text & "|" & txtImportoPag.Text & "|" & txtImportoMagg.Text & "|" & txtDataAccreditoDE.Text
    '            If ModPagamento.CompareTo(Session("sOldMyPagamento")) <> 0 Then
    '                'dati modificati -> Salvataggio delle modifiche
    '                idPagamento = FncPagamenti.SetPagamento(myStringConnection, oMyPagamento, 1)
    '            Else
    '                Session("oAnagrafe") = Nothing
    '                sScript = ""
    '                sScript += "alert('Inserimento effettuato correttamente!');"
    '                sScript += "parent.Comandi.location.href='ComandiRicGestPag.aspx?IsFromVariabile=" & ConstSession.IsFromVariabile & "&TRIBUTO=" & Request.Item("TRIBUTO") & "';"
    '                sScript += "parent.Visualizza.location.href='RicPag.aspx?IsFromVariabile=" & ConstSession.IsFromVariabile & "&TRIBUTO=" & Request.Item("TRIBUTO") & "';"
    '                sScript += ";"
    '                RegisterScript(sScript, Me.GetType)
    '                Exit Sub
    '            End If
    '        End If
    '        Session("sOldMyPagamento") = Nothing
    '        If idPagamento < 1 Then
    '            sScript = "alert('Si sono verificati errori durante il salvataggio.\nNon tutti i pagamenti sono stati salvati.');"
    '            RegisterScript(sScript, Me.GetType)
    '            Exit Sub
    '        Else
    '            'elimino il vecchio dettaglio
    '            Dim oMyDettaglio As New ObjDettaglioPagamento
    '            oMyDettaglio.IDPagamento = oMyPagamento.ID
    '            If oMyPagamento.ID > 0 Then
    '                If FncPagamenti.SetDettaglioPagamento(myStringConnection, oMyDettaglio, 2) < 0 Then
    '                    Log.Debug("GestPagamenti::CmdSalvaPag:: Si è verificato un'errore nella cancellazione del dettaglio!")
    '                End If
    '            End If
    '            If FncPagamenti.SetPagamento(myStringConnection, oMyPagamento, 4) < 0 Then
    '                'sScript = "alert('Si è verificato un\'errore nell\'aggiornamento del pagamento!');"
    '                Log.Debug("GestPagamenti::CmdSalvaPag:: Si è verificato un'errore nell'aggiornamento del pagamento!")
    '                'RegisterScript( sScript,Me.GetType)
    '            End If
    '            'se il richiamo al dettaglio dei pagamenti attraverso la funzione DettagliaPagamenti(sEnteImport, sOperatore, WFSessione) è negativo:
    '            'If oMyPagamento.dImportoStat <> 0 Then
    '            If oMyPagamento.ID <= 0 Then
    '                oMyPagamento.ID = idPagamento
    '            End If
    '            If FncPagamenti.DettagliaPagamenti(myStringConnection, oMyPagamento) <= 0 Then
    '                sScript = "alert('Si è verificato un\'errore in inserimento dettaglio!');"
    '                RegisterScript(sScript, Me.GetType)
    '            Else
    '                Session("oAnagrafe") = Nothing
    '                sScript = ""
    '                sScript += "alert('Salvataggio effettuato correttamente!');"
    '                sScript += "parent.Comandi.location.href='ComandiRicGestPag.aspx?IsFromVariabile=" & ConstSession.IsFromVariabile & "&TRIBUTO=" & Request.Item("TRIBUTO") & "';"
    '                sScript += "parent.Visualizza.location.href='RicPag.aspx?IsFromVariabile=" & ConstSession.IsFromVariabile & "&TRIBUTO=" & Request.Item("TRIBUTO") & "';"
    '            End If
    '            'Else
    '            '    If FncPagamenti.DettagliaPagamenti(ConstSession.IdEnte, ConstSession.UserName) <= 0 Then
    '            '        sScript = "alert('Si è verificato un\'errore in inserimento dettaglio!');"
    '            '        RegisterScript(me.gettype(),"msg", sScript)
    '            '    Else
    '            '        sScript = ""
    '            '        sScript += "alert('Salvataggio effettuato correttamente!');"
    '            '        sScript += "parent.Visualizza.location.href='RicPag.aspx';"
    '            '        sScript += "parent.Comandi.location.href='ComandiRicGestPag.aspx';"
    '            '        sScript += ";"
    '            '    End If
    '            'End If
    '        End If
    '        Session("oAnagrafe") = Nothing
    '        sScript = ""
    '        sScript += "alert('Inserimento effettuato correttamente!');"
    '        sScript += "parent.Comandi.location.href='ComandiRicGestPag.aspx?IsFromVariabile=" & ConstSession.IsFromVariabile & "&TRIBUTO=" & Request.Item("TRIBUTO") & "';"
    '        sScript += "parent.Visualizza.location.href='RicPag.aspx?IsFromVariabile=" & ConstSession.IsFromVariabile & "&TRIBUTO=" & Request.Item("TRIBUTO") & "';"
    '        RegisterScript(sScript, Me.GetType)
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPag.CmdSalvaPag_Click.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    Finally
    '        'If Not IsNothing(WFSessione) Then
    '        '    WFSessione.Kill()
    '        '    WFSessione = Nothing
    '        'End If
    '    End Try
    'End Sub
    ''' <summary>
    ''' Pulsante per il salvataggio fisico dei dati
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Analisi eventi</em>
    ''' </revision>
    ''' </revisionHistory>
    Private Sub CmdSalvaPag_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdSalvaPag.Click
        Dim ListPag() As OggettoPagamenti
        Dim oMyPagamento As New OggettoPagamenti
        Dim OperationDB As Integer = -1
        Dim FncPagamenti As New ClsGestPag
        Dim idPagamento As Integer = 0
        Dim ModPagamento As String = ""
        Dim bErrPag As Boolean = False
        Dim sScript As String = ""

        Try
            Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
            ListPag = CType(Session("oMyPagamento").ToArray(GetType(OggettoPagamenti)), OggettoPagamenti())
            For Each oMyPagamento In ListPag
                'controllo se sono stati modificati dei dati
                If oMyPagamento.ID = -1 Then
                    'Nuovo inserimento
                    OperationDB = OggettoPagamenti.TypeOperation.Insert
                Else
                    ModPagamento = hdIdContribuente.Value & "|" & TxtAnno.Text & "|" & txtNAvvisoDE.Text & "|" & txtNRata.Text & "|" & txtDataPag.Text & "|" & txtImportoPag.Text & "|" & txtImportoMagg.Text & "|" & txtDataAccreditoDE.Text
                    If ModPagamento.CompareTo(Session("sOldMyPagamento")) <> 0 Then
                        'dati modificati -> Salvataggio delle modifiche
                        OperationDB = OggettoPagamenti.TypeOperation.Update
                    End If
                End If
                If OperationDB >= 0 Then
                    idPagamento = FncPagamenti.SetPagamento(myStringConnection, oMyPagamento, OperationDB)
                    If idPagamento < 1 Then
                        bErrPag = True
                    Else
                        'elimino il vecchio dettaglio
                        Dim oMyDettaglio As New ObjDettaglioPagamento
                        oMyDettaglio.IDPagamento = oMyPagamento.ID
                        If oMyPagamento.ID > 0 Then
                            If FncPagamenti.SetDettaglioPagamento(myStringConnection, oMyDettaglio, 2) < 0 Then
                                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPag.CmdSalvaPag.Si è verificato un'errore nella cancellazione del dettaglio!")
                            End If
                        End If
                        FncPagamenti.SetPagamento(myStringConnection, oMyPagamento, OggettoPagamenti.TypeOperation.NotSplitted)
                        'se il richiamo al dettaglio dei pagamenti attraverso la funzione DettagliaPagamenti(sEnteImport, sOperatore, WFSessione) è negativo:
                        If oMyPagamento.ID <= 0 Then
                            oMyPagamento.ID = idPagamento
                        End If
                        If FncPagamenti.DettagliaPagamenti(myStringConnection, oMyPagamento) <= 0 Then
                            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPag.CmdSalvaPag.Si è verificato un'errore in inserimento dettaglio!');")
                        End If
                        If OperationDB = 0 Then
                            fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Pagamento, "CmdSalvaPag", Utility.Costanti.AZIONE_NEW, ConstSession.CodTributo, ConstSession.IdEnte, oMyPagamento.ID)
                        Else
                            fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Pagamento, "CmdSalvaPag", Utility.Costanti.AZIONE_UPDATE, ConstSession.CodTributo, ConstSession.IdEnte, oMyPagamento.ID)
                        End If
                    End If
                End If
            Next
            Session("sOldMyPagamento") = Nothing
            Session("oAnagrafe") = Nothing
            If bErrPag = True Then
                sScript = "GestAlert('a', 'danger', '', '', 'Si sono verificati errori durante il salvataggio.\nNon tutti i pagamenti sono stati salvati.');"
                RegisterScript(sScript, Me.GetType)
            Else
                hdIdContribuente.Value = "-1"
                sScript = "GestAlert('a', 'success', '', '', 'Inserimento effettuato correttamente!');"
                If Request.Item("IdListPagamento") Is Nothing Or Request.Item("IdListPagamento") = "-1" Then 'sono in nuovo inserimento
                    sScript += "if (confirm('Si vuole proseguire nell\'inserimento?'))"
                    sScript += "{"
                    sScript += "parent.Nascosto.location.href='../../../aspSvuota.asxp';"
                    'sScript += "parent.Comandi.location.href='ComandiGestPag.aspx?TRIBUTO=" & Request.Item("TRIBUTO") & "';"
                    sScript += "parent.Visualizza.location.href='GestPag.aspx?TRIBUTO=" & Request.Item("TRIBUTO") & "&IdListPagamento=-1';"
                    sScript += "}"
                    sScript += "else{"
                    sScript += "parent.Comandi.location.href='ComandiRicGestPag.aspx?IsFromVariabile=" & ConstSession.IsFromVariabile & "&TRIBUTO=" & Request.Item("TRIBUTO") & "';"
                    sScript += "parent.Visualizza.location.href='RicPag.aspx?IsFromVariabile=" & ConstSession.IsFromVariabile & "&TRIBUTO=" & Request.Item("TRIBUTO") & "';"
                    sScript += "parent.Nascosto.location.href='../../../aspVuota.aspx';"
                    sScript += "}"
                Else
                    sScript += "parent.Comandi.location.href='ComandiRicGestPag.aspx?IsFromVariabile=" & ConstSession.IsFromVariabile & "&TRIBUTO=" & Request.Item("TRIBUTO") & "';"
                    sScript += "parent.Visualizza.location.href='RicPag.aspx?IsFromVariabile=" & ConstSession.IsFromVariabile & "&TRIBUTO=" & Request.Item("TRIBUTO") & "';"
                    sScript += "parent.Nascosto.location.href='../../../aspVuota.aspx';"
                End If
                RegisterScript(sScript, Me.GetType)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPag.CmdSalvaPag_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub CmdSalvaPag_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdSalvaPag.Click
    '    Dim ListPag() As OggettoPagamenti
    '    Dim oMyPagamento As New OggettoPagamenti
    '    Dim OperationDB As Integer = -1
    '    Dim FncPagamenti As New ClsGestPag
    '    Dim idPagamento As Integer = 0
    '    Dim ModPagamento As String = ""
    '    Dim FncGestPag As New ClsGestPag
    '    Dim bErrPag As Boolean = False
    '    Dim sScript As String = ""

    '    Try
    '        ListPag = CType(Session("oMyPagamento").ToArray(GetType(OggettoPagamenti)), OggettoPagamenti())
    '        For Each oMyPagamento In ListPag
    '            'controllo se sono stati modificati dei dati
    '            If oMyPagamento.ID = -1 Then
    '                'Nuovo inserimento
    '                OperationDB = 0
    '            Else
    '                ModPagamento = hdIdContribuente.Value & "|" & TxtAnno.Text & "|" & txtNAvvisoDE.Text & "|" & txtNRata.Text & "|" & txtDataPag.Text & "|" & txtImportoPag.Text & "|" & txtImportoMagg.Text & "|" & txtDataAccreditoDE.Text
    '                If ModPagamento.CompareTo(Session("sOldMyPagamento")) <> 0 Then
    '                    'dati modificati -> Salvataggio delle modifiche
    '                    OperationDB = 1
    '                End If
    '            End If
    '            If OperationDB >= 0 Then
    '                idPagamento = FncPagamenti.SetPagamento(myStringConnection, oMyPagamento, OperationDB)
    '                If idPagamento < 1 Then
    '                    bErrPag = True
    '                Else
    '                    'elimino il vecchio dettaglio
    '                    Dim oMyDettaglio As New ObjDettaglioPagamento
    '                    oMyDettaglio.IDPagamento = oMyPagamento.ID
    '                    If oMyPagamento.ID > 0 Then
    '                        If FncPagamenti.SetDettaglioPagamento(myStringConnection, oMyDettaglio, 2) < 0 Then
    '                            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPag.CmdSalvaPag.Si è verificato un'errore nella cancellazione del dettaglio!")
    '                        End If
    '                    End If
    '                    FncPagamenti.SetPagamento(myStringConnection, oMyPagamento, 4)
    '                    'se il richiamo al dettaglio dei pagamenti attraverso la funzione DettagliaPagamenti(sEnteImport, sOperatore, WFSessione) è negativo:
    '                    If oMyPagamento.ID <= 0 Then
    '                        oMyPagamento.ID = idPagamento
    '                    End If
    '                    If FncPagamenti.DettagliaPagamenti(myStringConnection, oMyPagamento) <= 0 Then
    '                        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPag.CmdSalvaPag.Si è verificato un'errore in inserimento dettaglio!');")
    '                    End If
    '                End If
    '            End If
    '        Next
    '        Session("sOldMyPagamento") = Nothing
    '        Session("oAnagrafe") = Nothing
    '        If bErrPag = True Then
    '            sScript = "GestAlert('a', 'danger', '', '', 'Si sono verificati errori durante il salvataggio.\nNon tutti i pagamenti sono stati salvati.');"
    '            RegisterScript(sScript, Me.GetType)
    '        Else
    '            hdIdContribuente.Value = "-1"
    '            sScript = "GestAlert('a', 'success', '', '', 'Inserimento effettuato correttamente!');"
    '            If Request.Item("IdListPagamento") Is Nothing Or Request.Item("IdListPagamento") = "-1" Then 'sono in nuovo inserimento
    '                sScript += "if (confirm('Si vuole proseguire nell\'inserimento?'))"
    '                sScript += "{"
    '                sScript += "parent.Nascosto.location.href='../../../aspSvuota.asxp';"
    '                sScript += "parent.Comandi.location.href='ComandiGestPag.aspx?TRIBUTO=" & Request.Item("TRIBUTO") & "';"
    '                sScript += "parent.Visualizza.location.href='GestPag.aspx?TRIBUTO=" & Request.Item("TRIBUTO") & "&IdListPagamento=-1';"
    '                sScript += "}"
    '                sScript += "else{"
    '                sScript += "parent.Comandi.location.href='ComandiRicGestPag.aspx?IsFromVariabile=" & ConstSession.IsFromVariabile & "&TRIBUTO=" & Request.Item("TRIBUTO") & "';"
    '                sScript += "parent.Visualizza.location.href='RicPag.aspx?IsFromVariabile=" & ConstSession.IsFromVariabile & "&TRIBUTO=" & Request.Item("TRIBUTO") & "';"
    '                sScript += "}"
    '            Else
    '                sScript += "parent.Comandi.location.href='ComandiRicGestPag.aspx?IsFromVariabile=" & ConstSession.IsFromVariabile & "&TRIBUTO=" & Request.Item("TRIBUTO") & "';"
    '                sScript += "parent.Visualizza.location.href='RicPag.aspx?IsFromVariabile=" & ConstSession.IsFromVariabile & "&TRIBUTO=" & Request.Item("TRIBUTO") & "';"
    '            End If
    '            RegisterScript(sScript, Me.GetType)
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPag.CmdSalvaPag_Click.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    Private Sub btnModPagamenti_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnModPagamenti.Click
        Dim sScript As String = ""
        Dim oListOldPag As New OggettoPagamenti
        Dim nuovoPagamento As String = ""
        Try
            If TxtProvenienza.Text = "DEMANUALE" Or TxtProvenienza.Text.ToUpper.IndexOf("ENTRY") > 0 Then
                oListOldPag = Session("Pagamenti")

                Abilita(True, 0, oListOldPag)
                Abilita(False, 1, oListOldPag)

                Session("oPagamentiOrg") = Session("Pagamenti")
                nuovoPagamento = hdIdContribuente.Value & "|" & TxtAnno.Text & "|" & txtNAvvisoDE.Text & "|" & txtNRata.Text & "|" & txtDataPag.Text & "|" & txtImportoPag.Text & "|" & txtImportoMagg.Text & "|" & txtDataAccreditoDE.Text
                Session("sOldMyPagamento") = nuovoPagamento
            Else
                sScript = "GestAlert('a', 'warning', '', '', 'Pagamento da scarico postale.\nImpossibile fare modifiche!');"
                RegisterScript(sScript, Me.GetType)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPag.btnModPagamenti_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    Private Sub btnClearDatiPag_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearDatiPag.Click
        Try
            'ripulisco tutti i dati di sessione dei pagamenti
            ClearDatiPagamenti()
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPag.btnClearDatiPag_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    Private Sub btnRibalta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRibalta.Click
        Dim oDettaglioAnagrafica As DettaglioAnagrafica
        Try
            Dim IdTributo As String = Request.Item("TRIBUTO")
            If Not Session(ViewState("sessionName")) Is Nothing Then
                oDettaglioAnagrafica = Session(ViewState("sessionName"))
                Session("oAnagrafe") = oDettaglioAnagrafica
                TxtCodFiscale.Text = oDettaglioAnagrafica.CodiceFiscale
                TxtPIva.Text = oDettaglioAnagrafica.PartitaIva
                TxtCognome.Text = oDettaglioAnagrafica.Cognome
                TxtNome.Text = oDettaglioAnagrafica.Nome
                Select Case oDettaglioAnagrafica.Sesso
                    Case "F"
                        F.Checked = True
                    Case "G"
                        G.Checked = True
                    Case "M"
                        M.Checked = True
                End Select
                TxtDataNascita.Text = oDettaglioAnagrafica.DataNascita
                TxtLuogoNascita.Text = oDettaglioAnagrafica.ComuneNascita
                TxtResVia.Text = oDettaglioAnagrafica.ViaResidenza
                TxtResCivico.Text = oDettaglioAnagrafica.CivicoResidenza
                TxtResEsponente.Text = oDettaglioAnagrafica.EsponenteCivicoResidenza
                TxtResInterno.Text = oDettaglioAnagrafica.InternoCivicoResidenza
                TxtResScala.Text = oDettaglioAnagrafica.ScalaCivicoResidenza
                TxtResCAP.Text = oDettaglioAnagrafica.CapResidenza
                TxtResComune.Text = oDettaglioAnagrafica.ComuneResidenza
                TxtResPv.Text = oDettaglioAnagrafica.ProvinciaResidenza

                hdIdContribuente.Value = oDettaglioAnagrafica.COD_CONTRIBUENTE
                TxtIdDataAnagrafica.Text = oDettaglioAnagrafica.ID_DATA_ANAGRAFICA
                ViewState("sessionName") = ""
                Session("SEARCHPARAMETRES") = Nothing

                If rdbDaCartellazione.Checked = True Then
                    GetCartellePag(IdTributo, myStringConnection)
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPag.btnRibalta_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' pulsante per la cancellazione di un pagamento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Analisi eventi</em>
    ''' </revision>
    ''' </revisionHistory>
    Private Sub btnCancellaPag_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancellaPag.Click
        Dim FncPagamenti As New ClsGestPag
        Dim IsNonAbb As Boolean = False
        Dim sScript As String = ""

        Try
            Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
            IdTributo = Request.Item("TRIBUTO")
            If Not Request.Item("NonAbb") Is Nothing Then
                If Request.Item("NonAbb") = True Then
                    IsNonAbb = True
                End If
            End If
            If IsNonAbb = True Then
                Dim oMyPagamento As New OggettoPagamenti
                oMyPagamento.IdEnte = ConstSession.IdEnte
                oMyPagamento.sNote = IdTributo
                oMyPagamento.sAnno = CStr(Request.Item("Anno")).Trim
                oMyPagamento.sCFPIVA = Request.Item("CFPIVA")
                oMyPagamento.sNumeroAvviso = CStr(Request.Item("CodCartella")).Trim
                oMyPagamento.tDataPagamento = Request.Item("DataPagamento")
                'elimino il pagamento
                If FncPagamenti.SetPagamento(myStringConnection, oMyPagamento, OggettoPagamenti.TypeOperation.ToIgnore) <= 0 Then
                    sScript = "GestAlert('a', 'danger', '', '', 'Si è verificato un\'errore nella cancellazione del pagamento!');"
                    RegisterScript(sScript, Me.GetType)
                Else
                    fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Pagamento, "btnCancelllaPag", Utility.Costanti.AZIONE_DELETE, ConstSession.CodTributo, ConstSession.IdEnte, oMyPagamento.ID)
                    sScript = "GestAlert('a', 'success', '', '', 'Cancellazione effettuata correttamente!');"
                    sScript += "parent.Comandi.location.href='ComandiRicGestPag.aspx?IsFromVariabile=" & ConstSession.IsFromVariabile & "&TRIBUTO=" & Request.Item("TRIBUTO") & "';"
                    sScript += "parent.Visualizza.location.href='RicPag.aspx?IsFromVariabile=" & ConstSession.IsFromVariabile & "&TRIBUTO=" & Request.Item("TRIBUTO") & "';"
                    sScript += "parent.Nascosto.location.href='../../../aspVuota.aspx';"
                    RegisterScript(sScript, Me.GetType)
                End If
            Else
                If hdIdContribuente.Value <> -1 And TxtAnno.Text <> "" And txtDataPag.Text <> "" And txtImportoPag.Text <> "" Then
                    Dim oMyPagamento As New OggettoPagamenti
                    oMyPagamento.sAnno = TxtAnno.Text
                    oMyPagamento.sNumeroAvviso = txtNAvvisoDE.Text
                    oMyPagamento.IdEnte = ConstSession.IdEnte
                    oMyPagamento.IdContribuente = hdIdContribuente.Value
                    oMyPagamento.sNumeroRata = txtNRata.Text
                    oMyPagamento.dImportoPagamento = txtImportoPag.Text
                    If txtImportoMagg.Text <> "" Then
                        oMyPagamento.dImportoStat = txtImportoMagg.Text
                    End If
                    oMyPagamento.tDataPagamento = txtDataPag.Text
                    oMyPagamento.ID = TxtIdPag.Text
                    oMyPagamento.sOperatore = ConstSession.UserName
                    If txtDataAccreditoDE.Text <> "" Then
                        oMyPagamento.tDataAccredito = txtDataAccreditoDE.Text
                    Else
                        oMyPagamento.tDataAccredito = Date.MinValue
                    End If
                    oMyPagamento.tDataInsert = Now

                    If oMyPagamento.ID <> -1 Then
                        'elimino il dettaglio
                        Dim oMyDettaglio As New ObjDettaglioPagamento
                        oMyDettaglio.IDPagamento = oMyPagamento.ID
                        If FncPagamenti.SetDettaglioPagamento(myStringConnection, oMyDettaglio, 2) <= 0 Then
                            Log.Debug("GestPagamenti::btnCancellaPag:: Si è verificato un'errore nella cancellazione del dettaglio!")
                        End If
                        'elimino il pagamento
                        If FncPagamenti.SetPagamento(myStringConnection, oMyPagamento, OggettoPagamenti.TypeOperation.DelById) <= 0 Then
                            sScript = "GestAlert('a', 'danger', '', '', 'Si è verificato un\'errore nella cancellazione del pagamento!');"
                            RegisterScript(sScript, Me.GetType)
                        Else
                            fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Pagamento, "btnCancellaPag", Utility.Costanti.AZIONE_DELETE, ConstSession.CodTributo, ConstSession.IdEnte, oMyPagamento.ID)
                            sScript = ""
                            sScript += "GestAlert('a', 'success', '', '', 'Cancellazione effettuata correttamente!');" & vbCrLf
                            sScript += "parent.Comandi.location.href='ComandiRicGestPag.aspx?IsFromVariabile=" & ConstSession.IsFromVariabile & "&TRIBUTO=" & Request.Item("TRIBUTO") & "';"
                            sScript += "parent.Visualizza.location.href='RicPag.aspx?IsFromVariabile=" & ConstSession.IsFromVariabile & "&TRIBUTO=" & Request.Item("TRIBUTO") & "';"
                            sScript += "parent.Nascosto.location.href='../../../aspVuota.aspx';"
                            RegisterScript(sScript, Me.GetType)
                        End If
                    End If
                Else
                    sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario valorizzare i campi Anno, Data Pagamento, Importo Pagato e selezionare un Contribuente');"
                    RegisterScript(sScript, Me.GetType)
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPag.btnCancellaPag_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub btnCancellaPag_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancellaPag.Click
    '    'Dim x, k As Integer
    '    'Dim IsChecked As Integer = 0
    '    'Dim TabellaTotali As New DataTable("RATE_PAGAMENTI")
    '    Dim FncPagamenti As New ClsGestPag
    '    'Dim idPagamento As Integer = 0
    '    'Dim nSalvati As Integer = 0
    '    'Dim nDaSalvare As Integer = 0
    '    'Dim ModPagamento As String = ""
    '    Dim IsNonAbb As Boolean = False
    '    Dim sScript As String = ""

    '    Try
    '        ''inizializzo la connessione
    '        'WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        'If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '        '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        'End If

    '        'If TxtProvenienza.Text = "DEMANUALE" Then
    '        IdTributo = Request.Item("TRIBUTO")
    '        If Not Request.Item("NonAbb") Is Nothing Then
    '            If Request.Item("NonAbb") = True Then
    '                IsNonAbb = True
    '            End If
    '        End If
    '        If IsNonAbb = True Then
    '            Dim oMyPagamento As New OggettoPagamenti
    '            oMyPagamento.IdEnte = ConstSession.IdEnte
    '            oMyPagamento.sNote = IdTributo
    '            oMyPagamento.sAnno = CStr(Request.Item("Anno")).Trim
    '            oMyPagamento.sCFPIVA = Request.Item("CFPIVA")
    '            oMyPagamento.sNumeroAvviso = CStr(Request.Item("CodCartella")).Trim
    '            oMyPagamento.tDataPagamento = Request.Item("DataPagamento")
    '            'elimino il pagamento
    '            If FncPagamenti.SetPagamento(myStringConnection, oMyPagamento, 6) <= 0 Then
    '                sScript = "GestAlert('a', 'danger', '', '', 'Si è verificato un\'errore nella cancellazione del pagamento!');"
    '                RegisterScript(sScript, Me.GetType)
    '            Else
    '                sScript = "GestAlert('a', 'success', '', '', 'Cancellazione effettuata correttamente!');"
    '                sScript += "parent.Comandi.location.href='ComandiRicGestPag.aspx?IsFromVariabile=" & ConstSession.IsFromVariabile & "&TRIBUTO=" & Request.Item("TRIBUTO") & "';"
    '                sScript += "parent.Visualizza.location.href='RicPag.aspx?IsFromVariabile=" & ConstSession.IsFromVariabile & "&TRIBUTO=" & Request.Item("TRIBUTO") & "';"
    '                RegisterScript(sScript, Me.GetType)
    '            End If
    '        Else
    '            If hdIdContribuente.Value <> -1 And TxtAnno.Text <> "" And txtDataPag.Text <> "" And txtImportoPag.Text <> "" Then
    '                Dim oMyPagamento As New OggettoPagamenti
    '                oMyPagamento.sAnno = TxtAnno.Text
    '                oMyPagamento.sNumeroAvviso = txtNAvvisoDE.Text
    '                oMyPagamento.IdEnte = ConstSession.IdEnte
    '                oMyPagamento.IdContribuente = hdIdContribuente.Value
    '                oMyPagamento.sNumeroRata = txtNRata.Text
    '                oMyPagamento.dImportoPagamento = txtImportoPag.Text
    '                If txtImportoMagg.Text <> "" Then
    '                    oMyPagamento.dImportoStat = txtImportoMagg.Text
    '                End If
    '                oMyPagamento.tDataPagamento = txtDataPag.Text
    '                oMyPagamento.ID = TxtIdPag.Text
    '                oMyPagamento.sOperatore = ConstSession.UserName
    '                If txtDataAccreditoDE.Text <> "" Then
    '                    oMyPagamento.tDataAccredito = txtDataAccreditoDE.Text
    '                Else
    '                    oMyPagamento.tDataAccredito = Date.MinValue
    '                End If
    '                oMyPagamento.tDataInsert = Now

    '                If oMyPagamento.ID <> -1 Then
    '                    'elimino il dettaglio
    '                    Dim oMyDettaglio As New ObjDettaglioPagamento
    '                    oMyDettaglio.IDPagamento = oMyPagamento.ID
    '                    If FncPagamenti.SetDettaglioPagamento(myStringConnection, oMyDettaglio, 2) <= 0 Then
    '                        Log.Debug("GestPagamenti::btnCancellaPag:: Si è verificato un'errore nella cancellazione del dettaglio!")
    '                    End If
    '                    'elimino il pagamento
    '                    If FncPagamenti.SetPagamento(myStringConnection, oMyPagamento, 5) <= 0 Then
    '                        sScript = "GestAlert('a', 'danger', '', '', 'Si è verificato un\'errore nella cancellazione del pagamento!');"
    '                        RegisterScript(sScript, Me.GetType)
    '                    Else
    '                        sScript = ""
    '                        sScript += "GestAlert('a', 'success', '', '', 'Cancellazione effettuata correttamente!');" & vbCrLf
    '                        sScript += "parent.Comandi.location.href='ComandiRicGestPag.aspx?IsFromVariabile=" & ConstSession.IsFromVariabile & "&TRIBUTO=" & Request.Item("TRIBUTO") & "';"
    '                        sScript += "parent.Visualizza.location.href='RicPag.aspx?IsFromVariabile=" & ConstSession.IsFromVariabile & "&TRIBUTO=" & Request.Item("TRIBUTO") & "';"
    '                        sScript += ";"
    '                        RegisterScript(sScript, Me.GetType)
    '                    End If
    '                End If
    '            Else
    '                sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario valorizzare i campi Anno, Data Pagamento, Importo Pagato e selezionare un Contribuente');"
    '                RegisterScript(sScript, Me.GetType)
    '                Exit Sub
    '            End If
    '        End If
    '        'Else
    '        'sScript = "alert('Pagamento da scarico postale.\nImpossibile fare modifiche!')"
    '        'RegisterScript(me.gettype(),"del", "" & sScript & ";")
    '        'End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPag.btnCancellaPag_Click.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    Finally
    '        'If Not IsNothing(WFSessione) Then
    '        '    WFSessione.Kill()
    '        '    WFSessione = Nothing
    '        'End If
    '    End Try
    'End Sub
    Private Sub CmdRicercaEmesso_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdRicercaEmesso.Click
        Dim oListPagamenti() As OggettoPagamenti
        Dim FncGestPag As New ClsGestPag
        Dim oMySearchPag As New ObjSearchPagamenti
        Dim cmdMyCommand As SqlClient.SqlCommand = Nothing

        Try
            Dim IdTributo As String = Request.Item("TRIBUTO")
            If Not Request.Item("NonAbb") Is Nothing Then
                If Request.Item("NonAbb") = True Then
                    Try
                        cmdMyCommand = New SqlClient.SqlCommand
                        cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
                        cmdMyCommand.Connection.Open()
                        cmdMyCommand.CommandTimeout = 0
                        cmdMyCommand.CommandType = CommandType.StoredProcedure

                        oMySearchPag.sEnte = ConstSession.IdEnte
                        oMySearchPag.IdTributo = IdTributo
                        oMySearchPag.bRicPag = True
                        oMySearchPag.sCFPIVA = txtRicCFPIva.Text
                        oMySearchPag.sCognome = txtRicCognome.Text
                        oMySearchPag.sNome = txtRicNome.Text
                        oMySearchPag.sNAvviso = txtRicCodCartella.Text
                        oMySearchPag.sCodBollettino = txtRicIdOperazione.Text
                        If IsNumeric(txtRicImpPagato.Text) Then
                            oMySearchPag.impPagato = CDbl(txtRicImpPagato.Text.Replace(".", ","))
                        End If
                        oListPagamenti = FncGestPag.GetListEmessoPag(oMySearchPag, myStringConnection, cmdMyCommand)
                        If Not IsNothing(oListPagamenti) Then
                            If oListPagamenti.Length > 0 Then
                                GrdAvvisi.Visible = True
                                Session.Add("oListPagamenti", oListPagamenti)
                                GrdAvvisi.DataSource = oListPagamenti
                                GrdAvvisi.DataBind()
                                LblResultRicNonAbb.Style.Add("display", "none")
                            Else
                                GrdAvvisi.Visible = False
                                LblResultRicNonAbb.Text = "La ricerca non ha prodotto risultati."
                                LblResultRicNonAbb.Style.Add("display", "")
                            End If
                        Else
                            GrdAvvisi.Visible = False
                            LblResultRicNonAbb.Text = "La ricerca non ha prodotto risultati."
                            LblResultRicNonAbb.Style.Add("display", "")
                        End If
                        txtRicCFPIva.Focus()
                    Catch ex As Exception
                        Log.Debug("Si è verificato un errore in GestPag::CmdRicercaEmesso_Click::" & ex.Message)
                        Response.Redirect("../../../PaginaErrore.aspx")
                    Finally
                        cmdMyCommand.Dispose()
                        cmdMyCommand.Connection.Close()
                    End Try
                Else
                    If rdbDaCartellazione.Checked = True Then
                        GetCartellePag(IdTributo, myStringConnection)
                    End If
                End If
            Else
                If rdbDaCartellazione.Checked = True Then
                    GetCartellePag(IdTributo, myStringConnection)
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPag.CmdRicercaEmesso_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    Private Sub btnBack_Click(sender As Object, e As System.EventArgs) Handles btnBack.Click
        Dim sScript As String
        sScript = "parent.Comandi.location.href='ComandiRicGestPag.aspx?IsFromVariabile=" & ConstSession.IsFromVariabile & "&TRIBUTO=" & Request.Item("TRIBUTO") & "';"
        sScript += "parent.Visualizza.location.href='RicPag.aspx?IsFromVariabile=" & ConstSession.IsFromVariabile & "&TRIBUTO=" & Request.Item("TRIBUTO") & "';"
        sScript += "parent.Nascosto.location.href='../../../aspVuota.aspx';"
        RegisterScript(sScript, Me.GetType)
    End Sub

#Region "Griglie"
    Protected Sub GrdRowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                'Gestione Riga Tabella
                CType(e.Row.FindControl("txtDataScadenza"), TextBox).Enabled = False
                If CType(e.Row.FindControl("hfIdContribuente"), HiddenField).Value <> "-1" Then
                    If CType(e.Row.FindControl("txtDataPagamento"), TextBox).Text <> "" And CType(e.Row.FindControl("txtTotalePagamento"), TextBox).Text <> "" Then
                        e.Row.Enabled = False
                        'CType(e.Row.FindControl("txtDataPagamento"), TextBox).Enabled = False
                        'CType(e.Row.FindControl("txtDataAccredito"), TextBox).Enabled = False
                        'CType(e.Row.FindControl("txtTotalePagamento"), TextBox).Enabled = False
                    End If
                End If
            End If
            If Request.Item("TRIBUTO") = Utility.Costanti.TRIBUTO_TARSU Then
                GrdPagamenti.Columns(9).Visible = True
            Else
                GrdPagamenti.Columns(9).Visible = False
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPag.GrdRowDataBound.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Dim sScript As String = ""
        Dim FncGestPag As New ClsGestPag
        Dim cmdMyCommand As SqlClient.SqlCommand = Nothing

        Try
            Dim IDRow As String = e.CommandArgument.ToString()
            Select Case e.CommandName
                Case "RowAbbRata"
                    For Each myRow As GridViewRow In GrdAvvisi.Rows
                        If IDRow = CType(myRow.FindControl("hfIDRATA"), HiddenField).Value Then
                            Try
                                If FncGestPag.AbbinaPagamento(myStringConnection, Request.Item("CFPIVA"), CStr(Request.Item("CodCartella")).Trim, Request.Item("Anno"), Request.Item("DataPagamento"), CType(myRow.FindControl("hfIdContribuente"), HiddenField).Value, myRow.Cells(2).Text, myRow.Cells(5).Text, myRow.Cells(4).Text, myRow.Cells(3).Text, CDate(CType(myRow.FindControl("hfDataScadenza"), HiddenField).Value), cmdMyCommand) = False Then
                                    sScript = "GestAlert('a', 'danger', '', '', 'Errore in abbinamento pagamento!');"
                                    RegisterScript(sScript, Me.GetType)
                                Else
                                    sScript = "GestAlert('a', 'success', '', '', 'Abbinamento effettuato correttamente!');"
                                    sScript += "parent.Comandi.location.href='ComandiRicGestPag.aspx?IsFromVariabile=" & ConstSession.IsFromVariabile & "&TRIBUTO=" & Request.Item("TRIBUTO") & "';"
                                    sScript += "parent.Visualizza.location.href='RicPag.aspx?IsFromVariabile=" & ConstSession.IsFromVariabile & "&TRIBUTO=" & Request.Item("TRIBUTO") & "';"
                                    sScript += "parent.Nascosto.location.href='../../../aspVuota.aspx';"
                                    RegisterScript(sScript, Me.GetType)
                                End If
                            Catch ex As Exception
                                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPag.GrdRowCommand.errore: ", ex)
                                Response.Redirect("../../../PaginaErrore.aspx")
                            End Try
                        End If
                    Next
                Case "RowAbbAvviso"
                    For Each myRow As GridViewRow In GrdAvvisi.Rows
                        If IDRow = CType(myRow.FindControl("hfIDAVVISO"), HiddenField).Value Then
                            Try
                                If FncGestPag.AbbinaPagamento(myStringConnection, Request.Item("CFPIVA"), CStr(Request.Item("CodCartella")).Trim, CStr(Request.Item("Anno")).Trim, Request.Item("DataPagamento"), CType(myRow.FindControl("hfIdContribuente"), HiddenField).Value, myRow.Cells(2).Text, "", "", myRow.Cells(3).Text, CDate(CType(myRow.FindControl("hfDataScadenza"), HiddenField).Value), cmdMyCommand) = False Then
                                    sScript = "GestAlert('a', 'danger', '', '', 'Errore in abbinamento pagamento!');"
                                    RegisterScript(sScript, Me.GetType)
                                Else
                                    sScript = "GestAlert('a', 'success', '', '', 'Abbinamento effettuato correttamente!');"
                                    sScript += "parent.Comandi.location.href='ComandiRicGestPag.aspx?IsFromVariabile=" & ConstSession.IsFromVariabile & "&TRIBUTO=" & Request.Item("TRIBUTO") & "';"
                                    sScript += "parent.Visualizza.location.href='RicPag.aspx?IsFromVariabile=" & ConstSession.IsFromVariabile & "&TRIBUTO=" & Request.Item("TRIBUTO") & "';"
                                    sScript += "parent.Nascosto.location.href='../../../aspVuota.aspx';"
                                    RegisterScript(sScript, Me.GetType)
                                End If
                            Catch ex As Exception
                                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPag.GrdRowCommand.errore: ", ex)
                                Response.Redirect("../../../PaginaErrore.aspx")
                            End Try
                        End If
                    Next
            End Select
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPag.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub GrdPagamenti_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdPagamenti.ItemDataBound
    'Try
    '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '        'Gestione Riga Tabella
    '        If e.Item.Cells(11).Text <> -1 Then
    '            e.Item.Enabled = False
    '            If CType(e.Item.Cells(6).FindControl("txtDataPagamento"), TextBox).Text <> "" And CType(e.Item.Cells(8).FindControl("txtTotalePagamento"), TextBox).Text <> "" Then
    '                CType(e.Item.Cells(6).FindControl("txtDataPagamento"), TextBox).Enabled = False
    '                CType(e.Item.Cells(7).FindControl("txtDataAccredito"), TextBox).Enabled = False
    '                CType(e.Item.Cells(8).FindControl("txtTotalePagamento"), TextBox).Enabled = False
    '            End If
    '        End If
    '    End If
    ' Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPag.GrdPagamenti_ItemDataBound.errore: ", ex)
    '       Response.Redirect("../../../PaginaErrore.aspx")
    'End Try
    'End Sub

    'Private Sub GrdAvvisi_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GrdAvvisi.DeleteCommand
    '    Dim FncGestPag As New ClsGestPag
    '    Dim cmdMyCommand As SqlClient.SqlCommand = Nothing

    '    Try
    '        If FncGestPag.AbbinaPagamento(myStringConnection, Request.Item("CFPIVA"), CStr(Request.Item("CodCartella")).Trim, Request.Item("Anno"), Request.Item("DataPagamento"), e.Item.Cells(12).Text, e.Item.Cells(2).Text, e.Item.Cells(5).Text, e.Item.Cells(4).Text, e.Item.Cells(3).Text, cmdMyCommand) = False Then
    '            sScript = ""
    '            sScript += "GestAlert('a', 'danger', '', '', 'Errore in abbinamento pagamento!');" & vbCrLf
    '            sScript += ";"
    '            RegisterScript( sScript,Me.GetType)
    '        Else
    '            sScript = ""
    '            sScript += "GestAlert('a', 'success', '', '', 'Abbinamento effettuato correttamente!');" & vbCrLf
    '            sScript += "parent.Comandi.location.href='ComandiRicGestPag.aspx?IsFromVariabile=" & ConstSession.IsFromVariabile & "&TRIBUTO=" & Request.Item("TRIBUTO") & "';"
    '            sScript += "parent.Visualizza.location.href='RicPag.aspx?IsFromVariabile=" & ConstSession.IsFromVariabile & "&TRIBUTO=" & Request.Item("TRIBUTO") & "';"
    '            sScript += ";"
    '            RegisterScript( sScript,Me.GetType)
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPag.GrdAvvisi_DeleteCommand.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Private Sub GrdAvvisi_EditCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GrdAvvisi.EditCommand
    '    Dim FncGestPag As New ClsGestPag
    '    Dim cmdMyCommand As SqlClient.SqlCommand = Nothing

    '    Try
    '        If FncGestPag.AbbinaPagamento(myStringConnection, Request.Item("CFPIVA"), CStr(Request.Item("CodCartella")).Trim, CStr(Request.Item("Anno")).Trim, Request.Item("DataPagamento"), e.Item.Cells(12).Text, e.Item.Cells(2).Text, "", "", e.Item.Cells(3).Text, cmdMyCommand) = False Then
    '            sScript = ""
    '            sScript += "GestAlert('a', 'danger', '', '', 'Errore in abbinamento pagamento!');" & vbCrLf
    '            sScript += ";"
    '            RegisterScript( sScript,Me.GetType)
    '        Else
    '            sScript = ""
    '            sScript += "GestAlert('a', 'success', '', '', 'Abbinamento effettuato correttamente!');" & vbCrLf
    '            sScript += "parent.Comandi.location.href='ComandiRicGestPag.aspx?IsFromVariabile=" & ConstSession.IsFromVariabile & "&TRIBUTO=" & Request.Item("TRIBUTO") & "';"
    '            sScript += "parent.Visualizza.location.href='RicPag.aspx?IsFromVariabile=" & ConstSession.IsFromVariabile & "&TRIBUTO=" & Request.Item("TRIBUTO") & "';"
    '            sScript += ";"
    '            RegisterScript( sScript,Me.GetType)
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPag.GrdAvvisi_EditCommand.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Private Sub GrdAvvisi_UpdateCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GrdAvvisi.UpdateCommand
    '    Dim oListPagamenti() As OggettoPagamenti
    '    Dim FncGestPag As New ClsGestPag
    '    Dim oMySearchPag As New ObjSearchPagamenti
    '    Dim cmdMyCommand As SqlClient.SqlCommand = Nothing

    '    Try
    '        Dim IdTributo As String = Request.Item("TRIBUTO")
    '        oMySearchPag.sEnte = ConstSession.IdEnte
    '        oMySearchPag.IdTributo = IdTributo
    '        oMySearchPag.bRicPag = True
    '        oMySearchPag.sCFPIVA = txtRicCFPIva.Text
    '        oMySearchPag.sCognome = txtRicCognome.Text
    '        oMySearchPag.sNome = txtRicNome.Text
    '        oMySearchPag.sNAvviso = txtRicCodCartella.Text
    '        oMySearchPag.sCodBollettino = txtRicIdOperazione.Text
    '        If IsNumeric(txtRicImpPagato.Text) Then
    '            oMySearchPag.impPagato = CDbl(txtRicImpPagato.Text.Replace(".", ","))
    '        End If
    '        oListPagamenti = FncGestPag.GetListEmessoPag(oMySearchPag, myStringConnection, cmdMyCommand)
    '        If Not IsNothing(oListPagamenti) Then
    '            If oListPagamenti.Length > 0 Then
    '                GrdAvvisi.Visible = True
    '                Session.Add("oListPagamenti", oListPagamenti)
    '                GrdAvvisi.DataSource = oListPagamenti
    '                GrdAvvisi.DataBind()
    '                LblResultRicNonAbb.Style.Add("display", "none")
    '            Else
    '                GrdAvvisi.Visible = False
    '                LblResultRicNonAbb.Text = "La ricerca non ha prodotto risultati."
    '                LblResultRicNonAbb.Style.Add("display", "")
    '            End If
    '        Else
    '            GrdAvvisi.Visible = False
    '            LblResultRicNonAbb.Text = "La ricerca non ha prodotto risultati."
    '            LblResultRicNonAbb.Style.Add("display", "")
    '        End If
    '        txtRicCFPIva.Focus()
    '    Catch ex As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPag.GrdAvvisi_UpdateCommand.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    End Try
    'End Sub
#End Region

    Private Sub rdbDaCartellazione_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbDaCartellazione.CheckedChanged
        If Page.IsPostBack = True Then
            controlloSessione(False, True)
        End If
    End Sub
    Private Sub rdbDataEntry_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdbDataEntry.CheckedChanged
        If Page.IsPostBack = True Then
            controlloSessione(True, False)
        End If
    End Sub
    'Private Sub LnkAnagrafica_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles LnkAnagrafica.Click
    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Dim WFErrore As String

    '    Try
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        Session.Remove(ViewState("sessionName"))

    '        Dim oAnagrafica As New Anagrafica.DLL.GestioneAnagrafica(WFSessione.oSession, Session("Anagrafica"))
    '        Dim oDettaglioAnagrafica As New DettaglioAnagrafica
    '        oDettaglioAnagrafica.COD_CONTRIBUENTE = TxtCodContribuente.Text
    '        oDettaglioAnagrafica.ID_DATA_ANAGRAFICA = TxtIdDataAnagrafica.Text
    '        ViewState("sessionName") = "codContribuente"
    '        Session(ViewState("sessionName")) = oDettaglioAnagrafica
    '        writeJavascriptAnagrafica(ViewState("sessionName"))
    '        ''/* S.T. DEBUG */
    '        'TxtCodContribuente.Text = 45094
    '        'GetCartellePag()
    '        ''/* */
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPag.LknAnagrafica_Click.errore: ", Err)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    Finally
    '        If Not IsNothing(WFSessione) Then
    '            WFSessione.Kill()
    '            WFSessione = Nothing
    '        End If
    '    End Try
    'End Sub
    Private Sub LnkAnagrafica_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles LnkAnagrafica.Click
        Try
            Session.Remove(ViewState("sessionName"))

            Dim oDettaglioAnagrafica As New DettaglioAnagrafica
            oDettaglioAnagrafica.COD_CONTRIBUENTE = hdIdContribuente.Value
            oDettaglioAnagrafica.ID_DATA_ANAGRAFICA = TxtIdDataAnagrafica.Text
            ViewState("sessionName") = "codContribuente"
            Session(ViewState("sessionName")) = oDettaglioAnagrafica
            writeJavascriptAnagrafica(ViewState("sessionName"))
            ''/* S.T. DEBUG */
            'TxtCodContribuente.Text = 45094
            'GetCartellePag()
            ''/* */
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPag.LknAnagrafica_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    Private Sub LnkPulisciContr_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles LnkPulisciContr.Click
        Dim TabellaTotali As New DataTable("RATE_PAGAMENTI")

        TxtNome.Text = ""
        TxtCognome.Text = ""
        TxtCodFiscale.Text = ""
        TxtPIva.Text = ""
        TxtDataNascita.Text = ""
        TxtLuogoNascita.Text = ""
        TxtResVia.Text = ""
        TxtResCivico.Text = ""
        TxtResInterno.Text = ""
        TxtResEsponente.Text = ""
        TxtResScala.Text = ""
        TxtResCAP.Text = ""
        TxtResComune.Text = ""
        TxtResPv.Text = ""
        F.Checked = False
        G.Checked = False
        M.Checked = False
        hdIdContribuente.Value = "-1"

        TabellaTotali.Columns.Add("SELEZIONE")
        TabellaTotali.Columns.Add("ANNO")
        TabellaTotali.Columns.Add("CODICE_CARTELLA")
        TabellaTotali.Columns.Add("NUMERO_RATA")
        TabellaTotali.Columns.Add("IMPORTO_RATA")
        TabellaTotali.Columns.Add("DATA_SCADENZA")
        TabellaTotali.Columns.Add("DATA_PAGAMENTO")
        TabellaTotali.Columns.Add("DATA_ACCREDITO")
        TabellaTotali.Columns.Add("IMPORTO_PAGATO")
        TabellaTotali.Columns.Add("COD_CONTRIBUENTE")
        TabellaTotali.Columns.Add("ID_PAGAMENTO")

        GrdPagamenti.DataSource = TabellaTotali
        GrdPagamenti.DataBind()
        GrdPagamenti.Visible = False
        lblRisultato.Text = "Selezionare un contribuente e effettuare la ricerca delle rate"
    End Sub
    Private Sub txtNAvviso_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNAvviso.TextChanged
        If rdbDaCartellazione.Checked = True And txtNAvviso.Text.Length = 17 Then
            GetCartellePag(IdTributo, myStringConnection)
        End If
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="bTypeAbilita"></param>
    ''' <param name="IsSoloContrib"></param>
    ''' <param name="oPagamento"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Private Sub Abilita(ByVal bTypeAbilita As Boolean, ByVal IsSoloContrib As Integer, Optional ByVal oPagamento As OggettoPagamenti = Nothing)
        Dim sScript As String = ""
        Dim oListCmd() As Object
        Dim x As Integer

        'disabilito i dati del contribuente
        TxtCodFiscale.Enabled = bTypeAbilita : TxtPIva.Enabled = bTypeAbilita : F.Enabled = bTypeAbilita : M.Enabled = bTypeAbilita : G.Enabled = bTypeAbilita
        TxtCognome.Enabled = bTypeAbilita : TxtNome.Enabled = bTypeAbilita
        TxtDataNascita.Enabled = bTypeAbilita : TxtLuogoNascita.Enabled = bTypeAbilita
        TxtResVia.Enabled = bTypeAbilita : TxtResCivico.Enabled = bTypeAbilita : TxtResEsponente.Enabled = bTypeAbilita : TxtResInterno.Enabled = bTypeAbilita : TxtResScala.Enabled = bTypeAbilita
        TxtResCAP.Enabled = bTypeAbilita : TxtResComune.Enabled = bTypeAbilita : TxtResPv.Enabled = bTypeAbilita
        Try
            If IsSoloContrib <> 1 Then
                lblAnno.Enabled = bTypeAbilita
                lblNumAvvisoDE.Enabled = bTypeAbilita
                TxtAnno.Enabled = bTypeAbilita
                txtNAvviso.Enabled = bTypeAbilita
                txtAnnoVersamento.Enabled = bTypeAbilita
                txtNAvvisoDE.Enabled = bTypeAbilita
                txtNRata.Enabled = bTypeAbilita
                txtDataPag.Enabled = bTypeAbilita
                txtImportoPag.Enabled = bTypeAbilita
                txtImportoMagg.Enabled = bTypeAbilita
                txtDataAccreditoDE.Enabled = bTypeAbilita

                controlloSessione(rdbDataEntry.Checked, rdbDaCartellazione.Checked)

                rdbDaCartellazione.Enabled = False
                rdbDataEntry.Enabled = False

                If Not IsNothing(oPagamento) Then
                    If oPagamento.sNumeroAvviso <> "" And Not IsDBNull(oPagamento.sNumeroAvviso) Then
                        TxtAnno.Enabled = False
                        txtNAvvisoDE.Enabled = False
                        lblAnno.Enabled = False
                        lblNumAvvisoDE.Enabled = False
                    Else
                        TxtAnno.Enabled = True
                        txtNAvvisoDE.Enabled = True
                        lblAnno.Enabled = True
                        lblNumAvvisoDE.Enabled = True
                    End If
                End If

                'se passo da modifica a variazione devo disabilitare il pulsante di modifica/cancellazione e abilitare il pulsante di salvataggio
                If bTypeAbilita = True Then
                    ReDim Preserve oListCmd(1)
                    oListCmd(0) = "Modifica"
                    oListCmd(1) = "Cancella"
                    For x = 0 To oListCmd.Length - 1
                        sScript += "$('#" + oListCmd(x).ToString() + "').addClass('DisableBtn');"
                    Next
                    ReDim Preserve oListCmd(2)
                    oListCmd(2) = "Salva"
                    For x = 2 To oListCmd.Length - 1
                        sScript += "$('#" + oListCmd(x).ToString() + "').removeClass('DisableBtn');"
                    Next
                Else
                    sScript += "$('#Modifica').removeClass('DisableBtn');"
                    sScript += "$('#Cancella').removeClass('DisableBtn');"
                    sScript += "$('#Salva').addClass('DisableBtn');"
                End If
                RegisterScript(sScript, Me.GetType)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPag.Abilita.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub Abilita(ByVal bTypeAbilita As Boolean, ByVal IsSoloContrib As Integer, Optional ByVal oPagamento As OggettoPagamenti = Nothing)
    '    Dim sScript As String = ""
    '    Dim oListCmd() As Object
    '    Dim x As Integer
    '    Dim strscript As String = ""

    '    'disabilito i dati del contribuente
    '    TxtCodFiscale.Enabled = bTypeAbilita : TxtPIva.Enabled = bTypeAbilita : F.Enabled = bTypeAbilita : M.Enabled = bTypeAbilita : G.Enabled = bTypeAbilita
    '    TxtCognome.Enabled = bTypeAbilita : TxtNome.Enabled = bTypeAbilita
    '    TxtDataNascita.Enabled = bTypeAbilita : TxtLuogoNascita.Enabled = bTypeAbilita
    '    TxtResVia.Enabled = bTypeAbilita : TxtResCivico.Enabled = bTypeAbilita : TxtResEsponente.Enabled = bTypeAbilita : TxtResInterno.Enabled = bTypeAbilita : TxtResScala.Enabled = bTypeAbilita
    '    TxtResCAP.Enabled = bTypeAbilita : TxtResComune.Enabled = bTypeAbilita : TxtResPv.Enabled = bTypeAbilita
    '    Try
    '        If IsSoloContrib <> 1 Then
    '            lblAnno.Enabled = bTypeAbilita
    '            lblNumAvvisoDE.Enabled = bTypeAbilita
    '            TxtAnno.Enabled = bTypeAbilita
    '            txtNAvviso.Enabled = bTypeAbilita
    '            txtAnnoVersamento.Enabled = bTypeAbilita
    '            txtNAvvisoDE.Enabled = bTypeAbilita
    '            txtNRata.Enabled = bTypeAbilita
    '            txtDataPag.Enabled = bTypeAbilita
    '            txtImportoPag.Enabled = bTypeAbilita
    '            txtImportoMagg.Enabled = bTypeAbilita
    '            txtDataAccreditoDE.Enabled = bTypeAbilita

    '            controlloSessione(rdbDataEntry.Checked, rdbDaCartellazione.Checked)

    '            rdbDaCartellazione.Enabled = False
    '            rdbDataEntry.Enabled = False

    '            If Not IsNothing(oPagamento) Then
    '                If oPagamento.sNumeroAvviso <> "" And Not IsDBNull(oPagamento.sNumeroAvviso) Then
    '                    TxtAnno.Enabled = False
    '                    txtNAvvisoDE.Enabled = False
    '                    lblAnno.Enabled = False
    '                    lblNumAvvisoDE.Enabled = False
    '                Else
    '                    TxtAnno.Enabled = True
    '                    txtNAvvisoDE.Enabled = True
    '                    lblAnno.Enabled = True
    '                    lblNumAvvisoDE.Enabled = True
    '                End If
    '            End If

    '            'se passo da modifica a variazione devo disabilitare il pulsante di modifica/cancellazione e abilitare il pulsante di salvataggio
    '            If bTypeAbilita = True Then
    '                ReDim Preserve oListCmd(1)
    '                oListCmd(0) = "Modifica"
    '                oListCmd(1) = "Delete"
    '                For x = 0 To oListCmd.Length - 1
    '                    sScript += "parent.Comandi.document.getElementById('" & oListCmd(x).ToString() & "').disabled=" & True.ToString.ToLower & ";"
    '                Next
    '                ReDim Preserve oListCmd(2)
    '                oListCmd(2) = "Salva"
    '                For x = 2 To oListCmd.Length - 1
    '                    sScript += "parent.Comandi.document.getElementById('" & oListCmd(x).ToString() & "').disabled=" & False.ToString.ToLower & ";"
    '                Next
    '                RegisterScript(sScript, Me.GetType)
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPag.Abilita.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    Private Sub controlloSessione(ByVal radioDataEntry As Boolean, ByVal radioDaCartellazione As Boolean)
        Dim sScript As String = ""
        Try
            If radioDataEntry = True Then
                Session("TipoOpzione") = "DataEntry"
                txtAnnoVersamento.Enabled = True
                txtNAvviso.Enabled = True
                sScript = "DivDataEntry();"
                RegisterScript(sScript, Me.GetType)
            Else
                Session("TipoOpzione") = "DaCartellazione"
                txtAnnoVersamento.Enabled = False
                txtNAvviso.Enabled = False
                rdbDaCartellazione.Checked = True
                sScript = "DivCartellazione();"
                sScript += "$('#lblRisultato').text('Selezionare tutti i pagamenti che si desiderano inserire e compilarne i campi mancanti');"
                RegisterScript(sScript, Me.GetType)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPag.controlloSessione.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    Private Sub ClearDatiPagamenti()
        'ripulisco tutti i dati di sessione dei pagamenti
        Session.Remove("oAnagrafe")
        Session.Remove("Pagamenti")
        Session.Remove("Anagrafica")
        Session.Remove("TipoOpzione")
    End Sub
    Private Sub writeJavascriptAnagrafica(ByVal nomeSessione As String)
        Dim sScript As String

        sScript = "ApriRicercaAnagrafe('" & nomeSessione & "');"
        RegisterScript(sScript, Me.GetType)
    End Sub
    Private Sub GetCartellePag(IdTributo As String, myStringConnection As String)
        Dim FncGestPag As New ClsGestPag
        'Dim stringa As String = ""
        Dim TabellaTotali As New DataTable
        'Dim x As Integer
        Dim sScript As String = ""
        Dim oListPag() As OggettoPagamenti

        Try
            controlloSessione(rdbDataEntry.Checked, rdbDaCartellazione.Checked)

            If Session("TipoOpzione") = "DaCartellazione" And (hdIdContribuente.Value <> "-1" Or txtNAvviso.Text <> "") Then
                oListPag = FncGestPag.GetCartellePagamenti(myStringConnection, ConstSession.IdEnte, IdTributo, CInt(hdIdContribuente.Value), txtNAvviso.Text, txtAnnoVersamento.Text)
                If oListPag Is Nothing Then
                    lblRisultato.Text = "La ricerca non ha prodotto risultati."
                    GrdPagamenti.Visible = False
                Else
                    If oListPag.Length > 0 Then
                        GrdPagamenti.DataSource = oListPag
                        GrdPagamenti.DataBind()

                        GrdPagamenti.Visible = True
                        lblRisultato.Text = "Dati Cartellazione"
                        sScript += "$('#Salva').removeClass('DisableBtn');"
                        RegisterScript(sScript, Me.GetType)
                    Else
                        lblRisultato.Text = "La ricerca non ha prodotto risultati."
                        GrdPagamenti.DataSource = TabellaTotali
                        GrdPagamenti.DataBind()
                        GrdPagamenti.Visible = False
                    End If
                End If
            Else
                RegisterScript("GestAlert('a', 'warning', '', '', 'Necessario selezionare un contribuente o inserire un Avviso!');", Me.GetType)
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestPag.GetCartellaPag.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        Finally
            'WFSessione.Kill()
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myStringConnection"></param>
    ''' <revisionHistory>
    ''' <revision date="27/10/2014">
    ''' <strong>visualizzazione tutti indirizzi spedizione</strong>
    ''' Nuova gestione indirizzi spedizione
    ''' </revision>
    ''' </revisionHistory>
    Private Sub LoadPag(myStringConnection As String)
        Dim oListPag() As OggettoPagamenti
        Dim FncGestPag As New ClsGestPag
        Dim DatiContribuente As New DettaglioAnagrafica
        Dim oMySearchPag As New ObjSearchPagamenti

        Try
            DivDE.Visible = True : DivNonAbb.Visible = False

            If Not Request.Item("IdListPagamento") Is Nothing And Request.Item("IdListPagamento") <> "-1" Then
                oMySearchPag.sEnte = ConstSession.IdEnte
                oMySearchPag.IdPagamento = Request.Item("IdListPagamento")
                oMySearchPag.bRicPag = True
                rdbDataEntry.Checked = True
                '*** Pagamento già esistente -> visualizzato in modalità data entry
                oListPag = FncGestPag.GetListPagamenti(oMySearchPag, myStringConnection)
                If oListPag Is Nothing Then
                    Response.Redirect("../../../PaginaErrore.aspx")
                    Exit Sub
                End If

                Session("oMyPag") = oListPag(0)

                rdbDataEntry.Checked = True
                controlloSessione(rdbDataEntry.Checked, rdbDaCartellazione.Checked)

                '*** 201504 - Nuova Gestione anagrafica con form unico ***
                hdIdContribuente.Value = oListPag(0).IdContribuente
                If ConstSession.HasPlainAnag Then
                    ifrmAnag.Attributes.Add("src", "../../../Generali/asp/VisualAnag.aspx?IdContribuente=" & oListPag(0).IdContribuente & "&Azione=" & Utility.Costanti.AZIONE_LETTURA)
                Else
                    'Dim oMyAnagrafica As New Anagrafica.DLL.GestioneAnagrafica(WFSessione.oSession, ConfigurationManager.AppSettings("ParametroAnagrafica"))
                    'DatiContribuente = oMyAnagrafica.GetAnagrafica(oListPag(0).IdContribuente, ConstSession.CodTributo)
                    Dim oMyAnagrafica As New Anagrafica.DLL.GestioneAnagrafica()
                    '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
                    'DatiContribuente = oMyAnagrafica.GetAnagrafica(oListPag(0).IdContribuente, ConstSession.CodTributo, Costanti.INIT_VALUE_NUMBER, ConstSession.StringConnectionAnagrafica)
                    DatiContribuente = oMyAnagrafica.GetAnagrafica(oListPag(0).IdContribuente, Utility.Costanti.INIT_VALUE_NUMBER, String.Empty, ConstSession.DBType, ConstSession.StringConnectionAnagrafica, False)
                    '*** Caricamento Anagrafica
                    hdIdContribuente.Value = DatiContribuente.COD_CONTRIBUENTE
                    TxtIdDataAnagrafica.Text = DatiContribuente.ID_DATA_ANAGRAFICA
                    TxtCodFiscale.Text = DatiContribuente.CodiceFiscale
                    TxtPIva.Text = DatiContribuente.PartitaIva
                    TxtCognome.Text = DatiContribuente.Cognome
                    TxtNome.Text = DatiContribuente.Nome
                    TxtResVia.Text = DatiContribuente.ViaResidenza
                    TxtResCivico.Text = DatiContribuente.CivicoResidenza
                    TxtResEsponente.Text = DatiContribuente.EsponenteCivicoResidenza
                    TxtResInterno.Text = DatiContribuente.InternoCivicoResidenza
                    TxtResScala.Text = DatiContribuente.ScalaCivicoResidenza
                    TxtResCAP.Text = DatiContribuente.CapResidenza
                    TxtResComune.Text = DatiContribuente.ComuneResidenza
                    TxtResPv.Text = DatiContribuente.ProvinciaResidenza
                    If DatiContribuente.DataNascita <> "" Then
                        TxtDataNascita.Text = DatiContribuente.DataNascita
                    End If
                    TxtLuogoNascita.Text = DatiContribuente.ComuneNascita
                    Select Case DatiContribuente.Sesso
                        Case "F"
                            F.Checked = True
                        Case "M"
                            M.Checked = True
                        Case "G"
                            G.Checked = True
                        Case Else
                            F.Checked = False
                            M.Checked = False
                            G.Checked = False
                    End Select
                End If
                '*** ***

                '*** popolo i dati del versamento
                TxtIdPag.Text = oListPag(0).ID
                'txtAnnoVersamento.Text = oListPag(0).sAnno
                If oListPag(0).sNumeroAvviso <> "" And Not (oListPag(0).sNumeroAvviso Is DBNull.Value) Then
                    'txtNAvviso.Text = oListPag(0).sNumeroAvviso
                    txtNAvvisoDE.Text = oListPag(0).sNumeroAvviso
                    txtNAvvisoDE.Enabled = False
                    TxtAnno.Enabled = False
                    lblAnno.Enabled = False
                    lblNumAvvisoDE.Enabled = False
                Else
                    TxtAnno.Enabled = True
                    txtNAvvisoDE.Enabled = True
                    lblAnno.Enabled = True
                    lblNumAvvisoDE.Enabled = True
                End If

                If oListPag(0).sProvenienza <> "" Then
                    TxtProvenienza.Text = oListPag(0).sProvenienza
                    LblProvenienza.Text = FncGrd.FormattaProvPagGrd(oListPag(0).sProvenienza)
                End If
                If oListPag(0).sAnno <> "" Then
                    TxtAnno.Text = oListPag(0).sAnno
                End If
                If oListPag(0).sNumeroRata <> "" Then
                    txtNRata.Text = oListPag(0).sNumeroRata
                End If

                If oListPag(0).tDataPagamento <> Date.MinValue Then
                    txtDataPag.Text = oListPag(0).tDataPagamento
                End If

                If oListPag(0).dImportoPagamento <> 0 Then
                    txtImportoPag.Text = FormatNumber(oListPag(0).dImportoPagamento, 2)
                End If
                txtImportoMagg.Text = FormatNumber(oListPag(0).dImportoStat, 2)

                If oListPag(0).tDataAccredito <> Date.MinValue Then
                    txtDataAccreditoDE.Text = oListPag(0).tDataAccredito
                End If

                rdbDataEntry.Checked = True
                Abilita(False, 0)
            Else
                '*** Nuovo inserimento
                Abilita(False, 1)
                If ConstSession.HasPlainAnag Then
                    ifrmAnag.Attributes.Add("src", "../../../Generali/asp/VisualAnag.aspx?IdContribuente=" & hdIdContribuente.Value & "&Azione=" & Utility.Costanti.AZIONE_NEW)
                End If
                rdbDaCartellazione.Checked = True
                controlloSessione(rdbDataEntry.Checked, rdbDaCartellazione.Checked)
            End If
            'sScript = "parent.Comandi.document.getElementById('Search').style.display='none';"
            'RegisterScript(Me.GetType(), "nna", "" & sScript & ";")
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPag.LoadPag.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        Finally
            'WFSessione.Kill()
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="IdTributo"></param>
    ''' <param name="myStringConnection"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Private Sub LoadNonAbb(IdTributo As String, myStringConnection As String)
        Dim oListPag() As OggettoPagamenti
        Dim FncGestPag As New ClsGestPag
        Dim oMySearchPag As New ObjSearchPagamenti
        Dim sScript As String = ""
        Dim oListCmd() As Object
        Dim x As Integer

        Try
            DivDE.Visible = False : DivNonAbb.Visible = True

            oMySearchPag.sEnte = ConstSession.IdEnte
            oMySearchPag.IdTributo = IdTributo
            oMySearchPag.bRicPag = True
            oMySearchPag.sCFPIVA = Request.Item("CFPIVA")
            oMySearchPag.sNAvviso = CStr(Request.Item("CodCartella")).Trim
            oMySearchPag.sAnnoRif = CStr(Request.Item("Anno")).Trim
            If Request.Item("DataPagamento") <> "" Then
                oMySearchPag.tDataPagamentoDal = Request.Item("DataPagamento")
            End If
            oMySearchPag.IsNonAbbinati = Request.Item("NonAbb")
            '*** Pagamento già esistente -> visualizzato in modalità data entry
            oListPag = FncGestPag.GetListPagamenti(oMySearchPag, myStringConnection)
            If oListPag Is Nothing Then
                Response.Redirect("../../../PaginaErrore.aspx")
                Exit Sub
            End If
            Session("oMyPag") = oListPag(0)
            LblNonAbb.Text = "Provenienza: " + oListPag(0).sProvenienza
            If oListPag(0).sTipoBollettino <> "" Then
                LblNonAbb.Text += "&emsp;&emsp;Tipo Bollettino:" + oListPag(0).sTipoBollettino
            End If
            LblNonAbb.Text += "<br>Cod.Fiscale/P.IVA: " + oListPag(0).sCFPIVA + "&emsp;&emsp;" + oListPag(0).sCognome + " " + oListPag(0).sNome
            LblNonAbb.Text += "<br>Id.Operazione: " + oListPag(0).sNumeroAvviso
            LblNonAbb.Text += "&emsp;&emsp;Anno: " + oListPag(0).sAnno
            LblNonAbb.Text += "<br>Data Pagamento: " + FncGrd.FormattaDataGrd(oListPag(0).tDataPagamento)
            LblNonAbb.Text += "&emsp;&emsp;Importo: " + FormatNumber(oListPag(0).dImportoPagamento, 2)
            'blocco i pulsanti che non servono
            ReDim Preserve oListCmd(1) '(2)
            oListCmd(0) = "Modifica"
            oListCmd(1) = "Salva"
            For x = 0 To oListCmd.Length - 1
                sScript += "$('#" + oListCmd(x).ToString() + "').addClass('DisableBtn');"
            Next
            RegisterScript(sScript, Me.GetType)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPag.LoadNonAbb.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub LoadNonAbb(IdTributo As String, myStringConnection As String)
    '    Dim oListPag() As OggettoPagamenti
    '    Dim FncGestPag As New ClsGestPag
    '    Dim oMySearchPag As New ObjSearchPagamenti
    '    Dim sScript As String = ""
    '    Dim oListCmd() As Object
    '    Dim x As Integer

    '    Try
    '        DivDE.Visible = False : DivNonAbb.Visible = True

    '        oMySearchPag.sEnte = ConstSession.IdEnte
    '        oMySearchPag.IdTributo = IdTributo
    '        oMySearchPag.bRicPag = True
    '        oMySearchPag.sCFPIVA = Request.Item("CFPIVA")
    '        oMySearchPag.sNAvviso = CStr(Request.Item("CodCartella")).Trim
    '        oMySearchPag.sAnnoRif = CStr(Request.Item("Anno")).Trim
    '        If Request.Item("DataPagamento") <> "" Then
    '            oMySearchPag.tDataPagamentoDal = Request.Item("DataPagamento")
    '        End If
    '        oMySearchPag.IsNonAbbinati = Request.Item("NonAbb")
    '        '*** Pagamento già esistente -> visualizzato in modalità data entry
    '        oListPag = FncGestPag.GetListPagamenti(oMySearchPag, myStringConnection)
    '        If oListPag Is Nothing Then
    '            Response.Redirect("../../../PaginaErrore.aspx")
    '            Exit Sub
    '        End If
    '        Session("oMyPag") = oListPag(0)
    '        LblNonAbb.Text = "Provenienza: " + oListPag(0).sProvenienza
    '        If oListPag(0).sTipoBollettino <> "" Then
    '            LblNonAbb.Text += "&emsp;&emsp;Tipo Bollettino:" + oListPag(0).sTipoBollettino
    '        End If
    '        LblNonAbb.Text += "<br>Cod.Fiscale/P.IVA: " + oListPag(0).sCFPIVA + "&emsp;&emsp;" + oListPag(0).sCognome + " " + oListPag(0).sNome
    '        LblNonAbb.Text += "<br>Id.Operazione: " + oListPag(0).sNumeroAvviso
    '        LblNonAbb.Text += "&emsp;&emsp;Anno: " + oListPag(0).sAnno
    '        LblNonAbb.Text += "<br>Data Pagamento: " + FncGrd.FormattaDataGrd(oListPag(0).tDataPagamento)
    '        LblNonAbb.Text += "&emsp;&emsp;Importo: " + FormatNumber(oListPag(0).dImportoPagamento, 2)
    '        'blocco i pulsanti che non servono
    '        ReDim Preserve oListCmd(1) '(2)
    '        oListCmd(0) = "Modifica"
    '        oListCmd(1) = "Salva"
    '        'oListCmd(2) = "Cancella"
    '        For x = 0 To oListCmd.Length - 1
    '            sScript += "parent.Comandi.document.getElementById('" & oListCmd(x).ToString() & "').style.display='none';"
    '        Next
    '        RegisterScript(sScript, Me.GetType)
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPag.LoadNonAbb.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    End Try
    'End Sub
End Class
'*** ***