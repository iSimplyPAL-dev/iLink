Imports log4net
Imports OPENUtility
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports System.IO
''' <summary>
''' Pagina per la generazione del ruolo ordinario. La generazione di un ruolo passa attraverso un iter procedurale composto dai seguenti passaggi:
''' - Creazione
''' - Stampa Minuta
''' - Approva Minuta
''' - Configura Rate
''' - Calcolo Rate
''' - Elabora Documenti
''' - Approva Documenti
''' Per ogni ruolo è inoltre possibile consultarne le posizioni tramite il pulsante "Visualizza Calcoli".
''' Ad ogni ruolo è possibile associare un template di stampa specifico; la gestione del template personalizzato è resa possibile dai pulsanti "Upload Template" e "Download Template.''' 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
''' <revisionHistory>
''' <revision date="05/09/2018">
''' <strong>Bollettazione Vigliano</strong>
''' Per portare la bollettazione di Vigliano dentro al sistema OPENgov si rendono necessarie alcune modifiche alle funzioni di elaborazione avvisi come di seguito dettagliato.
''' <list type="bullet">
''' <item>
''' <em>Importazione Flussi</em>
''' <c>Avvio importazione</c>
''' Nella videata di Elaborazione Avvisi, tra le opzioni di Generazione, sarà aggiunta l'opzione di Generazione da Flusso Esterno; questa opzione permetterà all’operatore di selezionare i flussi di carico da importare. 
''' Per procedere all'import l’operatore dovrà quindi:
''' 1.	Selezionare l'opzione di Generazione corretta
''' 2.	Selezionare i flussi di carico
''' 3.	Cliccare sul solito pulsante di “Calcola”
''' Anno e Tipologia Calcolo saranno dedotti dal file, ciò vuol dire che per ogni file selezionato il sistema genererà un Ruolo.
''' I flussi selezionati saranno copiati su apposita cartella sul server; a livello di database, nella tabella TBLRUOLI_GENERATI, sarà aggiunto il campo TIPOGENERAZIONE {D=Da Dichiarazione, M=Manuale, F=Da Flusso} ed il campo FILEIMPORT che conterrà percorso e nome del flusso.
''' Sarà possibile importare solo flussi In formato 290-SEAB; i flussi potranno essere singoli con estensione TXT o all'interno di uno zip.
''' <c>Importazione</c>
''' I dati del flusso saranno letti e caricati negli oggetti già utilizzati per la generazione del ruolo e successivamente salvati In banca dati; In questo modo si potranno riutilizzare le query di inserimento.
''' Fatta eccezione per l'anagrafica che sarà anche inserita o aggiornata nella banca dati ANAGRAFICA.
''' Dovranno solo essere configurati i Parametri Calcolo; le categorie e le tariffe saranno direttamente lette dal flusso e non configurate In quanto non serve effettuare calcoli.
''' Nella tabella TBLCARTELLE:
''' •	Il campo LOTTO_CARTELLAZIONE sarà vuoto
''' •	Il campo PERIODO sarà salvato nel campo TBLRUOLI_GENERATI.DESCRIZIONE
''' •	Il campo CODICE_PARTITA non sarà salvato perché non utilizzato in stampa
''' •	Il campo CODICE_CLIENTE sarà salvato nel campo ANAGRAFICA.COD_INDIVIDUALE
''' Nella tabella TBLRUOLOTARSU:
''' •	Il campo IDDETTAGLIOTESTATA sarà vuoto
''' •	Il campo IDTARIFFA sarà vuoto
''' •	I campi DATA_INIZIO e DATA_FINE saranno vuoti
''' •	Le informazioni del “già emesso” saranno registrate come nuova partita di tipo GF con UBICAZIONE uguale alla formula che dovrà uscire in stampa
''' •	Il campo TIPO_UTENZA sarà inserito nel campo TIPO_RUOLO
''' Nella tabella TBLCARTELLEDETTAGLIOVOCI saranno registrati i campi con appositi CODVOCE codificati
''' IMPORTO_NETTO_TARIFFA_CONGUAGLIO
''' IMPORTO_NETTO_TARIFFA_ANNUO
''' </item>
''' <item>
''' <em>Stampa Minuta</em>
''' A livello di stampa deve essere aggiunto il campo TIPOUTENZA (Domestica o Produttiva).
''' Sarà aggiunta la domanda sul tipo di minuta da produrre: normale o per stampatore; questa domanda sarà fatta solo se l'installazione prevede l’uscita verso lo stampatore (da attuale voce di INI), in questo modo a GC non sarà fatta la domanda ma sulla nostra installazione si.
''' </item>
''' </list>
''' </revision>
''' </revisionHistory>
''' <revisionHistory>
''' <revision date="11/10/2018">
''' <strong>Gestione della data di fine conferimenti </strong>
''' Nella videata di elaborazione avvisi sarà aggiunta al fianco della data inizio conferimenti la data fine. Il nuovo campo sarà aggiunto nella tabella delle elaborazioni effettuate.
''' In fase di calcolo se il campo data fine non sarà valorizzato il sistema lo valorizzerà automaticamente pari a fine anno ruolo, in questo modo si eviteranno di mandare a ruolo conferimenti di competenza di anni successivi.
''' Il sistema quindi prenderà In considerazione per il calcolo i soli immobili attivi nell'anno di ruolo in elaborazione e le sole tessere aventi conferimenti compresi tra le date indicate.
''' </revision>
''' </revisionHistory>
''' <revisionHistory>
''' <revision date="26/11/2018">
''' <strong>Insoluti e Coattivo</strong>
''' <list type="bullet">
'''   <item>
''' <em>Calcolo ruolo</em>
''' Nella videata di calcolo ruolo sarà aggiunta la tipologia RUOLO INSOLUTI, questa scelta chiederà anche il numero di giorni (default 60) che devono essere trascorsi dalla data di scadenza per considerare l’emesso come insoluto.
''' <c>Generazione Avviso ordinario</c>
''' Saranno selezionati tutti gli avvisi rispondenti ai criteri impostati; su questi avvisi sarà ricalcolato l'importo dovuto detraendone il già pagato, saranno calcolate le addizionali configurate per la tipologia ruolo e sarà ricalcolato l’arrotondamento al netto delle eventuali spese di notifica. 
''' Codice cartella e data emissione dovranno rimanere gli stessi dell'avviso originale quindi, fintanto che il ruolo non è approvato, il codice cartella sarà temporaneamente modificato facendolo iniziare con 999.
''' <c>Generazione Atto di Ingiunzione</c>
''' Sarà aggiunto un nuovo tipo di provvedimento ATTO DI INGIUNZIONE per ora attivo solo per il tributo TARES/TARI; dovrà essere configurata la tipologia voce per Omesso/Parziale Versamento da abbinare alla nuova tipologia di atto.
''' Le attuali funzioni di creazione accertamenti saranno adeguate per essere anche richiamate da una procedura che analizzerà il contribuente per l'anno in ogni fase (le fasi saranno calcolate solo se ne sono state configurate delle voci), la somma delle fasi darà l’atto finale.
''' Il salvataggio In banca dati dell'atto sarà spostata esternamente alla funzione di creazione, in modo da poter forzare, nel caso di generazione da RUOLO INSOLUTI, la tipologia atto (INGIUNZIONE), il numero atto ed il numero ingiunzione (CODICE CARTELLA), la data di elaborazione (DATA EMISSIONE CARTELLA), la data di conferma (DATA ELABORAZIONE ATTO) e la provenienza (NOTIFICA CARTELLA).
''' Nel caso di generazione da RUOLO INSOLUTI l'accertato sarà sempre uguale al dichiarato quindi sarà sempre presente la sola Fase 2, sempre in questo caso l’atto rimarrà “IN ATTESA” e quindi non visibile fino a quando non decadranno i termini di pagamento dell’avviso. Un atto è “IN ATTESA” se ha la provenienza NOTIFICA CARTELLA e non ha la data di conferma.
'''   </item>
'''   <item>
''' <em>Elaborazione Minuta</em>
''' Nella stampa saranno aggiunti i campi per le sanzioni e le spese di notifica con relativi totalizzatori.
'''   </item>
'''   <item>
'''   <em>Gestione Data Accertamento</em>
''' La data di accertamento dovrà essere valorizzata nel caso In cui l'avviso ordinario non sia pagato entro i termini <strong>(60g dalla data di notifica)</strong>; una volta impostata questa data l’avviso non potrà più essere trattato in ordinario né come sgravi né come pagamenti e neanche come insoluti, sarà quindi disponibile in sola visualizzazione.
''' La data sarà inserita tramite STORED PROCEDURE che girerà tutte le notti e andrà a valorizzare la data di accertamento, pari al termine massimo di pagamento, a tutti gli avvisi che hanno un'ingiunzione “IN ATTESA”, andrà inoltre a valorizzare la data conferma dell’ingiunzione in modo da renderla effettiva.
'''   </item>
''' </list>
''' </revision>
''' </revisionHistory>
Partial Class Calcolo
    Inherits BaseEnte
    Private Shared Log As ILog = LogManager.GetLogger(GetType(Calcolo))
    Protected FncGrd As New Formatta.FunctionGrd
    Private sScript As String = ""

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents LblResultRuoliPrec As System.Web.UI.WebControls.Label
    Protected WithEvents CmdRate As System.Web.UI.WebControls.Button
    Protected WithEvents FrmCalcolo As Global.System.Web.UI.HtmlControls.HtmlForm
    Protected WithEvents lblTitolo As Global.System.Web.UI.WebControls.Label
    Protected WithEvents optAutomatica As Global.System.Web.UI.WebControls.RadioButton
    Protected WithEvents optManuale As Global.System.Web.UI.WebControls.RadioButton
    Protected WithEvents Label5 As Global.System.Web.UI.WebControls.Label
    Protected WithEvents ddlAnno As Global.System.Web.UI.WebControls.DropDownList
    Protected WithEvents Label16 As Global.System.Web.UI.WebControls.Label
    Protected WithEvents ddlTipoCalcolo As Global.System.Web.UI.WebControls.DropDownList
    Protected WithEvents TxtPercentTariffe As Global.System.Web.UI.WebControls.TextBox
    Protected WithEvents Label10 As Global.System.Web.UI.WebControls.Label
    Protected WithEvents txtSogliaMinima As Global.System.Web.UI.WebControls.TextBox
    Protected WithEvents Label27 As Global.System.Web.UI.WebControls.Label
    Protected WithEvents DivRiepilogoDaElab As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents LblIntestNDom As Global.System.Web.UI.WebControls.Label
    Protected WithEvents LblNDom As Global.System.Web.UI.WebControls.Label
    Protected WithEvents LblIntestNNonDom As Global.System.Web.UI.WebControls.Label
    Protected WithEvents LblNNonDom As Global.System.Web.UI.WebControls.Label
    Protected WithEvents Label28 As Global.System.Web.UI.WebControls.Label
    Protected WithEvents LblNContribDaElab As Global.System.Web.UI.WebControls.Label
    Protected WithEvents LblIntestMQDom As Global.System.Web.UI.WebControls.Label
    Protected WithEvents LblMQDom As Global.System.Web.UI.WebControls.Label
    Protected WithEvents LblIntestMQNonDom As Global.System.Web.UI.WebControls.Label
    Protected WithEvents LblMQNonDom As Global.System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As Global.System.Web.UI.WebControls.Label
    Protected WithEvents LblMQDaElab As Global.System.Web.UI.WebControls.Label
    Protected WithEvents LblIntestTessereDaElab As Global.System.Web.UI.WebControls.Label
    Protected WithEvents LblNTessereDaElab As Global.System.Web.UI.WebControls.Label
    Protected WithEvents LblIntestTesNoUI As Global.System.Web.UI.WebControls.Label
    Protected WithEvents LblIntestTesBidone As Global.System.Web.UI.WebControls.Label
    Protected WithEvents LblTesNoUIDaElab As Global.System.Web.UI.WebControls.Label
    Protected WithEvents LblTesBidoneDaElab As Global.System.Web.UI.WebControls.Label
    Protected WithEvents LblIntestConf As Global.System.Web.UI.WebControls.Label
    Protected WithEvents LblConferimentiDaElab As Global.System.Web.UI.WebControls.Label
    Protected WithEvents Label26 As Global.System.Web.UI.WebControls.Label
    Protected WithEvents DivRiepilogoElab As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents Label24 As Global.System.Web.UI.WebControls.Label
    Protected WithEvents LblNContrib As Global.System.Web.UI.WebControls.Label
    Protected WithEvents Label23 As Global.System.Web.UI.WebControls.Label
    Protected WithEvents LblNDoc As Global.System.Web.UI.WebControls.Label
    Protected WithEvents Label18 As Global.System.Web.UI.WebControls.Label
    Protected WithEvents LblNScarti As Global.System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As Global.System.Web.UI.WebControls.Label
    Protected WithEvents LblImpPF As Global.System.Web.UI.WebControls.Label
    Protected WithEvents Label7 As Global.System.Web.UI.WebControls.Label
    Protected WithEvents LblImpPV As Global.System.Web.UI.WebControls.Label
    Protected WithEvents Label32 As Global.System.Web.UI.WebControls.Label
    Protected WithEvents LblIntestImpPC As Global.System.Web.UI.WebControls.Label
    Protected WithEvents LblImpPC As Global.System.Web.UI.WebControls.Label
    Protected WithEvents Label35 As Global.System.Web.UI.WebControls.Label
    Protected WithEvents LblImpAddiz As Global.System.Web.UI.WebControls.Label
    Protected WithEvents Label37 As Global.System.Web.UI.WebControls.Label
    Protected WithEvents LblIntestImpPM As Global.System.Web.UI.WebControls.Label
    Protected WithEvents LblImpPM As Global.System.Web.UI.WebControls.Label
    Protected WithEvents Label29 As Global.System.Web.UI.WebControls.Label
    Protected WithEvents LblImpTot As Global.System.Web.UI.WebControls.Label
    Protected WithEvents Label25 As Global.System.Web.UI.WebControls.Label
    Protected WithEvents LblNote As Global.System.Web.UI.WebControls.Label
    Protected WithEvents GrdDateElaborazione As Global.Ribes.OPENgov.WebControls.RibesGridView
    Protected WithEvents DivAttesa As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents DivDettaglio As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents Label4 As Global.System.Web.UI.WebControls.Label
    Protected WithEvents LblResultRuoliVsCat As Global.System.Web.UI.WebControls.Label
    Protected WithEvents GrdRuoliVsCat As Global.Ribes.OPENgov.WebControls.RibesGridView
    Protected WithEvents LbResultRuoliVsScaglioni As Global.System.Web.UI.WebControls.Label
    Protected WithEvents GrdRuoliVsScaglioni As Global.Ribes.OPENgov.WebControls.RibesGridView
    Protected WithEvents DivRuoliPrec As Global.System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents Label20 As Global.System.Web.UI.WebControls.Label
    Protected WithEvents GrdRuoliPrec As Global.Ribes.OPENgov.WebControls.RibesGridView
    Protected WithEvents LblIdElab As Global.System.Web.UI.WebControls.Label
    Protected WithEvents CmdCalcola As Global.System.Web.UI.WebControls.Button
    Protected WithEvents CmdStampaMinuta As Global.System.Web.UI.WebControls.Button
    Protected WithEvents CmdApprovaMinuta As Global.System.Web.UI.WebControls.Button
    Protected WithEvents CmdCartella As Global.System.Web.UI.WebControls.Button
    Protected WithEvents CmdDeleteElab As Global.System.Web.UI.WebControls.Button
    Protected WithEvents CmdDocumenti As Global.System.Web.UI.WebControls.Button
    Protected WithEvents CmdMinutaAvvisi As Global.System.Web.UI.WebControls.Button
    Protected WithEvents CmdDettaglio As Global.System.Web.UI.WebControls.Button
    Protected WithEvents LblTipoCalcolo As Global.System.Web.UI.WebControls.Label
    Protected WithEvents ChkConferimenti As Global.System.Web.UI.WebControls.CheckBox
    Protected WithEvents ChkMaggiorazione As Global.System.Web.UI.WebControls.CheckBox

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        Dim sScript As String
        InitializeComponent()
        lblTitolo.Text = ConstSession.DescrizioneEnte
        If ConstSession.IsFromTARES = "1" Then
            info.InnerText = "TARI "
        Else
            info.InnerText = "TARSU "
        End If
        If ConstSession.IsFromVariabile(Request.Item("IsFromVariabile")) = "1" Then
            info.InnerText += "Variabile "
        Else
            ChkConferimenti.Visible = False
            sScript = "$('#DataConf').hide();$('#RiepDaElabTessere').hide();$('#LblIntestImpPC').hide();$('#LblImpPC').hide();$('#RuoliVsCatPC').hide();"
            RegisterScript(sScript, Me.GetType())
        End If
        info.InnerText += " - Avvisi - Elaborazioni"
        sScript = "$('#PercentTariffe').hide();"
        '*** 201809 Bollettazione Vigliano in OPENgov***
        lblNoteFlussi.Text = "N.B. il nome file deve iniziare con il Codice Catastale dell'ente (" + ConstSession.Belfiore + ") e deve essere in formato 290-SEAB."
        sScript += "$('#lblUploadFiles').text('Template Documento di Stampa');$('#lblNoteFlussi').hide();"
        sScript += "$('#DivRiepilogoDaElab').hide();$('#DivRiepilogoElab').hide();$('#fsRuoliImportati').hide();" '$('#divUploadFiles').hide();
        RegisterScript(sScript, Me.GetType())
    End Sub
#End Region
    ''' <summary>
    ''' Al caricamento della pagina carica la combo con i possibili anni e le tipologie di ruolo da elaborare.
    ''' Viene inoltre controllato se c'è un'elaborazione in corso, in questo caso viene visualizzata la progressione di elaborazione; altrimenti viene visualizzato il riepilogo dei ruoli rispondenti ai criteri impostati.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="09/2018">
    ''' Bollettazione Vigliano in OPENgov
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim sSQL As String
        Dim oLoadCombo As New generalClass.generalFunction
        Dim FncRuolo As New ClsGestRuolo

        Try
            If ConstSession.Ambiente.ToUpper() = "CMGC" Then
                If ConstSession.IsFromVariabile("1") Then
                End If
            End If
            'Put user code to initialize the page here
            If Page.IsPostBack = False Then
                lblMessage.Visible = False
                hdMinutaStampatoreAllowed.Value = ConstSession.IsMinutaXStampatore.ToString
                hdPFPVUniqueRow.Value = ConstSession.bPFPVUniqueRow.ToString

                txtInizioConf.Text = Date.Now().ToShortDateString()

                FncRuolo.LoadTipoCalcolo(ConstSession.StringConnection, ConstSession.IdEnte, ddlAnno.SelectedValue, LblTipoCalcolo, LblTipoMQ, ChkConferimenti, ChkMaggiorazione)
                sSQL = "SELECT *"
                sSQL += " FROM V_GETANNIRUOLO"
                sSQL += " WHERE IDENTE='" & ConstSession.IdEnte & "'"
                sSQL += " ORDER BY DESCRIZIONE DESC"
                oLoadCombo.LoadComboGenerale(ddlAnno, sSQL, ConstSession.StringConnection, True, Costanti.TipoDefaultCmb.NUMERO)
                'popolo combo tiporuolo
                sSQL = "SELECT *"
                sSQL += " FROM V_GETTIPORUOLO"
                sSQL += " WHERE (TIPOTASSAZIONE='" & LblTipoCalcolo.Text & "')"
                sSQL += " AND IDTIPORUOLO<>'S'"
                If ConstSession.HasNotifiche = False Then
                    sSQL += " AND IDTIPORUOLO<>'I'"
                End If
                sSQL += " ORDER BY ORDINAMENTO"
                oLoadCombo.LoadComboGenerale(ddlTipoCalcolo, sSQL, ConstSession.StringConnection, True, Costanti.TipoDefaultCmb.STRINGA)
                'controllo se stro facendo un'elaborazione
                Dim AnnoCalcoloInCorso As Integer = CacheManager.GetCalcoloMassivoInCorso
                If (AnnoCalcoloInCorso <> -1) Then
                    ShowCalcoloInCorso()
                ElseIf (Not (Session("TARSUAnnoCalcoloInCorso")) Is Nothing) Then
                    ddlAnno.SelectedValue = CType(Session("TARSUAnnoCalcoloInCorso"), Integer)
                    ddlAnno_SelectedIndexChanged(Nothing, Nothing)
                End If
                Dim listRuoli() As ObjRuolo = CacheManager.GetRiepilogoCalcoloMassivo
                If Not IsNothing(listRuoli) Then
                    VisualizzaRiepilogo(listRuoli, 1)
                    CacheManager.RemoveRiepilogoCalcoloMassivo()
                    CacheManager.RemoveRiepilogoImportRuoli()
                End If
                Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Elaborazioni, "Calcolo", Utility.Costanti.AZIONE_LETTURA.ToString, ConstSession.CodTributo, ConstSession.IdEnte, LblIdElab.Text)
            End If
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
            If ddlTipoCalcolo.SelectedValue <> ObjRuolo.Ruolo.CartelleInsoluti Then
                sScript += "$('#DecorrenzaTermine').hide();"
            End If
            RegisterScript(sScript, Me.GetType)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.Calcolo.Page_Load.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    'Put user code to initialize the page here
    '    Dim sSQL As String
    '    Dim oLoadCombo As New generalClass.generalFunction
    '    Dim FncRuolo As New ClsGestRuolo

    '    Try
    '        If ConstSession.Ambiente.ToUpper() = "CMGC" Then
    '            If ConstSession.IsFromVariabile("1") Then
    '            End If
    '        End If
    '        'Put user code to initialize the page here
    '        If Page.IsPostBack = False Then
    '            lblMessage.Visible = False
    '            '*** 201809 Bollettazione Vigliano in OPENgov***
    '            hdMinutaStampatoreAllowed.Value = ConstSession.IsMinutaXStampatore.ToString
    '            hdPFPVUniqueRow.Value = ConstSession.bPFPVUniqueRow.ToString

    '            txtInizioConf.Text = Date.Now().ToShortDateString()

    '            FncRuolo.LoadTipoCalcolo(ConstSession.IdEnte, ddlAnno.SelectedValue, LblTipoCalcolo, LblTipoMQ, ChkConferimenti, ChkMaggiorazione, Nothing)
    '            '*** 201809 Bollettazione Vigliano in OPENgov***
    '            'popolo combo Anno
    '            'sSQL = "SELECT DISTINCT ANNO, ANNO AS IDANNO"
    '            'sSQL += " FROM TBLTARIFFE"
    '            'sSQL += " WHERE IDENTE='" & ConstSession.IdEnte & "'"
    '            'sSQL += " ORDER BY IDANNO DESC"
    '            sSQL = "SELECT *"
    '            sSQL += " FROM V_GETANNIRUOLO"
    '            sSQL += " WHERE IDENTE='" & ConstSession.IdEnte & "'"
    '            sSQL += " ORDER BY DESCRIZIONE DESC"
    '            oLoadCombo.LoadComboGenerale(ddlAnno, sSQL, ConstSession.StringConnection, True, Costanti.TipoDefaultCmb.NUMERO)
    '            'popolo combo tiporuolo
    '            'sSQL = "SELECT DESCRIZIONE, IDTIPORUOLO"
    '            'sSQL += " FROM TBLTIPORUOLO"
    '            ''*** 20131104 - TARES ***
    '            'sSQL += " WHERE (TIPOTASSAZIONE='" & LblTipoCalcolo.Text & "')"
    '            ''*** ***
    '            'sSQL += " ORDER BY ORDINAMENTO"
    '            sSQL = "SELECT *"
    '            sSQL += " FROM V_GETTIPORUOLO"
    '            sSQL += " WHERE (TIPOTASSAZIONE='" & LblTipoCalcolo.Text & "')"
    '            If ConstSession.HasNotifiche = False Then
    '                sSQL += " AND IDTIPORUOLO<>'I'"
    '            End If
    '            sSQL += " ORDER BY ORDINAMENTO"
    '            oLoadCombo.LoadComboGenerale(ddlTipoCalcolo, sSQL, ConstSession.StringConnection, True, Costanti.TipoDefaultCmb.STRINGA)
    '            'controllo se stro facendo un'elaborazione
    '            Dim AnnoCalcoloInCorso As Integer = CacheManager.GetCalcoloMassivoInCorso
    '            If (AnnoCalcoloInCorso <> -1) Then
    '                ShowCalcoloInCorso()
    '            ElseIf (Not (Session("TARSUAnnoCalcoloInCorso")) Is Nothing) Then
    '                ddlAnno.SelectedValue = CType(Session("TARSUAnnoCalcoloInCorso"), Integer)
    '                ddlAnno_SelectedIndexChanged(Nothing, Nothing)
    '            End If
    '            Dim listRuoli() As ObjRuolo = CacheManager.GetRiepilogoCalcoloMassivo
    '            If Not IsNothing(listRuoli) Then
    '                VisualizzaRiepilogo(listRuoli, 1)
    '                CacheManager.RemoveRiepilogoCalcoloMassivo()
    '                CacheManager.RemoveRiepilogoImportRuoli()
    '            End If
    '            Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
    '            fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Elaborazioni, "Calcolo", Utility.Costanti.AZIONE_LETTURA.ToString, ConstSession.CodTributo, ConstSession.IdEnte, LblIdElab.Text)
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.Calcolo.Page_Load.errore: ", Err)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    '*** 201809 Bollettazione Vigliano in OPENgov***
    ''' <summary>
    ''' Al cambio dell'anno vengono aggiornate le tipologie di ruolo selezionabili.
    ''' Vengono aggiornati a video i dati riepilogativi da elaborare e viene visualizzato il riepilogo dei ruoli rispondenti ai criteri impostati.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ddlAnno_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAnno.SelectedIndexChanged
        Dim FncRuolo As New ClsGestRuolo
        Dim oMyTotRuolo() As ObjRuolo

        Try
            Session("oRuoloTIA") = Nothing
            sScript = "$('#fsRuoliImportati').hide();$('#lblNoteFlussi').hide();"

            If ddlAnno.SelectedValue > 0 Then
                FncRuolo.LoadTipoCalcolo(ConstSession.StringConnection, ConstSession.IdEnte, ddlAnno.SelectedValue, LblTipoCalcolo, LblTipoMQ, ChkConferimenti, ChkMaggiorazione)
                'prelevo il ruolo da elaborare
                oMyTotRuolo = FncRuolo.GetDatiPerRuolo(ConstSession.StringConnection, ConstSession.IdEnte, ddlAnno.SelectedValue)
                If oMyTotRuolo.GetLength(0) > 0 Then
                    VisualizzaRiepilogo(oMyTotRuolo, 0)
                Else
                    sScript += "$('#DivRiepilogoDaElab').hide();"
                End If
                If ddlTipoCalcolo.SelectedValue <> "" Then
                    '*** 20130308 - devo memorizzare il conto corrente altrimenti non sarà valorizzato nella codeline e nel codice a barre ***
                    'prelevo il ruolo in elaborazione
                    oMyTotRuolo = FncRuolo.GetRuolo(ConstSession.StringConnection, ConstSession.IdEnte, -1, ddlAnno.SelectedValue, ddlTipoCalcolo.SelectedValue, 0, False, Nothing)
                    '*** ***
                    VisualizzaRiepilogo(oMyTotRuolo, 1)
                    'aggiorno la variabile di sessione
                    Session("oRuoloTIA") = oMyTotRuolo
                Else
                    'popolo combo tiporuolo
                    Dim oLoadCombo As New generalClass.generalFunction
                    Dim sSQL As String = "SELECT *"
                    sSQL += " FROM V_GETTIPORUOLO"
                    '*** 20131104 - TARES ***
                    sSQL += " WHERE (TIPOTASSAZIONE='" & LblTipoCalcolo.Text & "')"
                    '*** ***
                    sSQL += " AND IDTIPORUOLO<>'S'"
                    If ConstSession.HasNotifiche = False Then
                        sSQL += " AND IDTIPORUOLO<>'I'"
                    End If
                    sSQL += " ORDER BY ORDINAMENTO"
                    oLoadCombo.LoadComboGenerale(ddlTipoCalcolo, sSQL, ConstSession.StringConnection, True, Costanti.TipoDefaultCmb.STRINGA)
                    sScript += "$('#DivRiepilogoElab').hide();"
                    'sScript += "$('#divUploadFiles').hide();"
                End If
                '*** 20130308 - devo memorizzare il conto corrente altrimenti non sarà valorizzato nella codeline e nel codice a barre ***
                'prelevo i ruoli elaborati
                oMyTotRuolo = FncRuolo.GetRuolo(ConstSession.StringConnection, ConstSession.IdEnte, -1, ddlAnno.SelectedValue, "", 1, False, Nothing)
                '*** ***
                If Not IsNothing(oMyTotRuolo) Then
                    'popolo la griglia
                    GrdRuoliPrec.DataSource = oMyTotRuolo
                    GrdRuoliPrec.DataBind()
                    GrdRuoliPrec.SelectedIndex = -1
                    'visualizzo il pannello 
                    sScript += "DivRuoliPrec.style.display='';"
                Else
                    'nascondo il pannello
                    sScript += "DivRuoliPrec.style.display='none';"
                End If
                RegisterScript(sScript, Me.GetType)
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.Calcolo.SelectedIndexChanged.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** 201809 Bollettazione Vigliano in OPENgov***
    ''' <summary>
    ''' Al cambio del tipo ruolo vengono aggiornati a video i dati riepilogativi da elaborare e viene visualizzato il riepilogo dei ruoli rispondenti ai criteri impostati.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ddlTipoCalcolo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTipoCalcolo.SelectedIndexChanged
        Dim FncRuolo As New ClsGestRuolo
        Dim oMyTotRuolo() As ObjRuolo
        Dim sScript As String

        Try
            sScript = "$('#fsRuoliImportati').hide();$('#lblNoteFlussi').hide();"
            sScript += "$('#DecorrenzaTermine').hide();"
            If ddlTipoCalcolo.SelectedValue = ObjRuolo.Ruolo.APercentuale Then
                sScript += "$('#PercentTariffe').show();"
                If ConstSession.IsFromVariabile() = "1" Then
                    sScript += "$('#DataConf').show();"
                Else
                    sScript += "$('#DataConf').hide();"
                End If
                '**** 201809 - Cartelle Insoluti ***
            ElseIf ddlTipoCalcolo.SelectedValue = ObjRuolo.Ruolo.CartelleInsoluti Then
                sScript += "$('#PercentTariffe').hide();$('#DataConf').hide();"
                If ConstSession.HasNotifiche = True Then
                    sScript += "$('#DecorrenzaTermine').show();"
                End If
            Else
                sScript += "$('#PercentTariffe').hide();"
                If ConstSession.IsFromVariabile() = "1" Then
                    sScript += "$('#DataConf').show();"
                Else
                    sScript += "$('#DataConf').hide();"
                End If
            End If

            Session("oRuoloTIA") = Nothing

            If ddlAnno.SelectedValue <> "" Then
                If ddlTipoCalcolo.SelectedValue <> "" Then
                    '*** 20130308 - devo memorizzare il conto corrente altrimenti non sarà valorizzato nella codeline e nel codice a barre ***
                    'prelevo il ruolo in elaborazione
                    oMyTotRuolo = FncRuolo.GetRuolo(ConstSession.StringConnection, ConstSession.IdEnte, -1, ddlAnno.SelectedValue, ddlTipoCalcolo.SelectedValue, 0, False, Nothing)
                    '*** ***
                    VisualizzaRiepilogo(oMyTotRuolo, 1)
                    'aggiorno la variabile di sessione
                    Session("oRuoloTIA") = oMyTotRuolo
                    If oMyTotRuolo Is Nothing Then
                        sScript += "$('#DivRiepilogoDaElab').hide();"
                        sScript += "$('#DivRiepilogoElab').hide();"
                        'sScript += "$('#divUploadFiles').hide();"
                    End If
                Else
                    sScript += "$('#DivRiepilogoElab').hide();"
                    'sScript += "$('#divUploadFiles').hide();"
                    'prelevo i ruoli elaborati
                    oMyTotRuolo = FncRuolo.GetRuolo(ConstSession.StringConnection, ConstSession.IdEnte, -1, ddlAnno.SelectedValue, "", 1, False, Nothing)
                    If Not IsNothing(oMyTotRuolo) Then
                        'popolo la griglia
                        GrdRuoliPrec.DataSource = oMyTotRuolo
                        GrdRuoliPrec.DataBind()
                        GrdRuoliPrec.SelectedIndex = -1
                        'visualizzo il pannello 
                        RegisterScript("DivRuoliPrec.style.display='';", Me.GetType)
                    Else
                        'nascondo il pannello
                        RegisterScript("DivRuoliPrec.style.display='none';", Me.GetType)
                    End If
                End If
            End If
            RegisterScript(sScript, Me.GetType())
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.Calcolo.SelectedIndexChanged.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** 20181011 Dal/Al Conferimenti ***'*** 201809 Bollettazione Vigliano in OPENgov***
    ''' <summary>
    ''' Pulsante per la creazione del ruolo.
    ''' Controlli preventivi in base alla tipologia di generazione:
    ''' - Se è stata selezionata l'opzione "DaFlusso" deve essere stato selezionato un file di carico coerente; altrimenti messaggio di blocco.
    ''' - Se è stata selezionata l'opzione "DaDichiarazione" devono essere state configurate tutte le tariffe oggetto di calcolo; altrimenti messaggio di blocco.
    ''' 
    ''' Controllo se è già presente un ruolo approvato per gli stessi parametri; altrimenti messaggio di blocco.
    ''' 
    ''' Se vengono passati tutti i controlli richiama in modo asincrono ClsCalcoloRuolo.StartCalcolo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdCalcola_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdCalcola.Click
        'calcolo gli articoli per i vari anni
        Dim oMyRuolo() As ObjRuolo = Nothing
        Dim sTipoElab, sScript As String
        Dim FncRuolo As New ClsGestRuolo
        Dim FncCalcolo As New ClsElabRuolo
        Dim sErr As String = ""
        Dim nCheck As Integer
        Dim calcolo As New ClsCalcoloRuolo
        Dim ListAddizionali() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale
        Dim ListFlussi As New ArrayList
        Dim ImpSpeseIngiunzione As Double = 0

        Try
            sTipoElab = ObjRuolo.Generazione.DaDichiarazione
            CacheManager.RemoveRiepilogoCalcoloMassivo()
            CacheManager.RemoveAvanzamentoElaborazione()
            CacheManager.RemoveRiepilogoCalcoloMassivoArtVSCatPF()
            CacheManager.RemoveRiepilogoCalcoloMassivoArtVSCatPV()
            CacheManager.RemoveRiepilogoCalcoloMassivoArtVSCatPC()
            CacheManager.RemoveRiepilogoCalcoloMassivoArtVSCatPM()
            CacheManager.RemoveRiepilogoImportRuoli()
            'visualizzo il pannello di attesa
            DivAttesa.Style.Add("display", "")
            If optFlusso.Checked = True Then
                sTipoElab = ObjRuolo.Generazione.DaFlusso
                'carico i flussi selezionati nella cartella di lavoro
                Dim ListFiles As HttpFileCollection = Request.Files
                If Not ListFiles Is Nothing Then
                    Dim ErrorToImport As String = CheckFilesToImport(ListFiles, ListFlussi)
                    If ErrorToImport <> "" Then
                        sScript = "GestAlert('a', 'danger', '', '', '" + ErrorToImport + "');"
                        sScript += "$('#DivRiepilogoDaElab').hide();$('#lblUploadFiles').text('Flussi');$('#lblNoteFlussi').show();$('#divParamRuolo').hide();$('#DivRiepilogoElab').hide();$('#fsRuoliImportati').hide();"
                        RegisterScript(sScript, Me.GetType)
                        lblMessage.Text = ErrorToImport
                        lblMessage.Visible = True
                        DivAttesa.Style.Add("display", "none")
                        Exit Sub
                    End If
                Else
                    sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario selezionare un file!');"
                    sScript += "$('#DivRiepilogoDaElab').hide();$('#lblUploadFiles').text('Flussi');$('#lblNoteFlussi').show();$('#divParamRuolo').hide();$('#DivRiepilogoElab').hide();$('#fsRuoliImportati').hide();"
                    RegisterScript(sScript, Me.GetType)
                    lblMessage.Text = "E' necessario selezionare un file."
                    lblMessage.Visible = True
                    DivAttesa.Style.Add("display", "none")
                    Exit Sub
                End If
            Else
                If optAutomatica.Checked = True Then
                    sTipoElab = ObjRuolo.Generazione.DaDichiarazione
                    'controllo la presenza delle tariffe per tutte le categorie
                    nCheck = FncCalcolo.CheckTariffe(ConstSession.StringConnection, LblTipoCalcolo.Text, ConstSession.IdEnte, ddlAnno.SelectedValue, sErr)
                    If nCheck = 0 Then
                        sScript = "GestAlert('a', 'warning', '', '', 'Attenzione! le seguenti categorie non hanno una tariffa associata.\nImpossibile proseguire!\n" & sErr & "');"
                        RegisterScript(sScript, Me.GetType)
                        DivAttesa.Style.Add("display", "none")
                        Exit Sub
                    ElseIf nCheck = -1 Then
                        sScript = "GestAlert('a', 'warning', '', '', 'Attenzione!\nNon sono presenti dichiarazioni o articoli per la generazione del ruolo!');"
                        RegisterScript(sScript, Me.GetType)
                        DivAttesa.Style.Add("display", "none")
                        Exit Sub
                    End If
                Else
                    sTipoElab = ObjRuolo.Generazione.Manuale
                End If
                '*** 20130308 - devo memorizzare il conto corrente altrimenti non sarà valorizzato nella codeline e nel codice a barre ***
                'controllo se esiste già un ruolo della stessa tipologia per l'anno
                oMyRuolo = FncRuolo.GetRuolo(ConstSession.StringConnection, ConstSession.IdEnte, -1, ddlAnno.SelectedValue, ddlTipoCalcolo.SelectedValue, 0, False, Nothing)
                '*** ***
                If chkSimulazione.Checked = False Then
                    If Not IsNothing(oMyRuolo) Then
                        If oMyRuolo(0).sTipoCalcolo = ObjRuolo.Ruolo.AConguaglio And oMyRuolo(0).tDataOKMinuta <> Date.MaxValue Then
                            'se è già stata approvata la minuta non posso rifarlo
                            sScript = "GestAlert('a', 'warning', '', '', 'E\' già presente un ruolo approvato per gli stessi parametri.\nImpossibile proseguire!');"
                            RegisterScript(sScript, Me.GetType)
                            DivAttesa.Style.Add("display", "none")
                            Exit Sub
                        Else
                            '**** 201809 - Cartelle Insoluti ***
                            If ddlTipoCalcolo.SelectedValue <> ObjRuolo.Ruolo.CartelleInsoluti Then
                                If oMyRuolo(0).tDataOKDoc = Date.MaxValue Then
                                    If oMyRuolo(0).tDataOKMinuta <> Date.MaxValue Then
                                        'se è già stata approvata la minuta non posso rifarlo
                                        sScript = "GestAlert('a', 'warning', '', '', 'E\' già presente un ruolo approvato per gli stessi parametri.\nEliminarlo prima di poter proseguire!');"
                                        RegisterScript(sScript, Me.GetType)
                                        DivAttesa.Style.Add("display", "none")
                                        Exit Sub
                                    Else
                                        'ripulisco per fare il nuovo calcolo
                                        If FncCalcolo.DeleteRuolo(ConstSession.DBType, ConstSession.StringConnection, ConstSession.StringConnectionOPENgov, ConstSession.IsFromVariabile, oMyRuolo(0)) = False Then
                                            sScript = "GestAlert('a', 'warning', '', '', 'La cancellazione e\' stata terminata a causa di un errore!');"
                                            RegisterScript(sScript, Me.GetType)
                                            Exit Sub
                                        Else
                                            'aggiorno la variabile di sessione
                                            Session("oRuoloTIA") = Nothing
                                        End If
                                    End If
                                End If
                            Else
                                If oMyRuolo(0).tDataOKDoc = Date.MaxValue Then
                                    sScript = "GestAlert('a', 'warning', '', '', 'Il ruolo non e\' approvato.\nImpossibile proseguire!');"
                                    RegisterScript(sScript, Me.GetType)
                                    Exit Sub
                                End If
                            End If
                        End If
                    End If
                End If
            End If
            ListAddizionali = New GestAddizionali().GetAddizionale(ConstSession.StringConnection, ConstSession.IdEnte, ddlAnno.SelectedValue, "", ddlTipoCalcolo.SelectedValue)
            If ListAddizionali Is Nothing Then
                sScript = "GestAlert('a', 'warning', '', '', 'Errore in lettura addizionali.\nImpossibile proseguire!');"
                RegisterScript(sScript, Me.GetType)
                Exit Sub
            End If
            ImpSpeseIngiunzione = New GestAddizionali().GetSpeseIngiunzione(ConstSession.StringConnectionProvv, ConstSession.IdEnte, Utility.Costanti.TRIBUTO_TARSU, ddlAnno.SelectedValue, 2)
            ShowCalcoloInCorso()
            Dim bIsFromVariabile As Boolean = False
            If ConstSession.IsFromVariabile = "1" Then
                bIsFromVariabile = True
            End If

            If txtInizioConf.Text = "" Then
                txtInizioConf.Text = Date.MaxValue
            End If
            If txtFineConf.Text = "" Then
                txtFineConf.Text = New generalClass.generalFunction().FormattaData(ddlAnno.SelectedValue + "1231", "G") 'CDate(ddlAnno.SelectedValue + "1231")
            End If
            If IsNumeric(txtGGScadenza.Text) = False Then
                txtGGScadenza.Text = "60"
            End If

            calcolo.StartCalcolo(ConstSession.DBType, ConstSession.StringConnection, ConstSession.IdEnte, sTipoElab, LblTipoCalcolo.Text, ddlTipoCalcolo.SelectedValue, ddlTipoCalcolo.SelectedItem.Text, TxtPercentTariffe.Text, ChkMaggiorazione.Checked, ChkConferimenti.Checked, LblTipoMQ.Text.Substring(0, 1), ddlAnno.SelectedValue, txtSogliaMinima.Text, -1, chkSimulazione.Checked, bIsFromVariabile, CDate(txtInizioConf.Text), CDate(txtFineConf.Text), ListAddizionali, ListFlussi, ConstSession.Belfiore, ConstSession.StringConnectionAnagrafica, ConstSession.StringConnectionOPENgov, ConstSession.StringConnectionICI, ConstSession.StringConnectionProvv, ConstSession.UrlServiziLiquidazione, ConstSession.UrlServiziAccertamenti, ConstSession.UserName, CInt(txtGGScadenza.Text), impspeseingiunzione)
            'aggiorno la variabile di sessione
            Session("oRuoloTIA") = oMyRuolo
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.Calcolo.CmdCalcola_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub CmdCalcola_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdCalcola.Click
    '    'calcolo gli articoli per i vari anni
    '    Dim oMyRuolo() As ObjRuolo
    '    Dim sTipoElab, sScript As String
    '    Dim FncRuolo As New GestRuolo
    '    Dim FncCalcolo As New ClsElabRuolo
    '    Dim sErr As String = ""
    '    'Dim WFErrore As String = ""
    '    'Dim WFSessione As OPENUtility.CreateSessione
    '    Dim nCheck As Integer
    '    Dim calcolo As New CalcoloRuolo
    '    Dim ListAddizionali() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale

    '    Try
    '        CacheManager.RemoveRiepilogoCalcoloMassivo()
    '        CacheManager.RemoveAvanzamentoElaborazione()
    '        CacheManager.RemoveRiepilogoCalcoloMassivoArtVSCatPF()
    '        CacheManager.RemoveRiepilogoCalcoloMassivoArtVSCatPV()
    '        CacheManager.RemoveRiepilogoCalcoloMassivoArtVSCatPC()
    '        CacheManager.RemoveRiepilogoCalcoloMassivoArtVSCatPM()
    '        'WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        'If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '        '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        'End If
    '        'visualizzo il pannello di attesa
    '        DivAttesa.Style.Add("display", "")
    '        If optAutomatica.Checked = True Then
    '            sTipoElab = ObjRuolo.generazione.dadichiarazione
    '            'controllo la presenza delle tariffe per tutte le categorie
    '            nCheck = FncCalcolo.CheckTariffe(LblTipoCalcolo.Text, ConstSession.IdEnte, ddlAnno.SelectedValue, sErr, Nothing)
    '            If nCheck = 0 Then
    '                sScript = "alert('Attenzione! le seguenti categorie non hanno una tariffa associata.\nImpossibile proseguire!\n" & sErr & "');"
    '                RegisterScript(sScript, Me.GetType)
    '                DivAttesa.Style.Add("display", "none")
    '                Exit Sub
    '            ElseIf nCheck = -1 Then
    '                sScript = "alert('Attenzione!\nNon sono presenti dichiarazioni o articoli per la generazione del ruolo!');"
    '                RegisterScript(sScript, Me.GetType)
    '                DivAttesa.Style.Add("display", "none")
    '                Exit Sub
    '            End If
    '        Else
    '            sTipoElab = ObjRuolo.generazione.manuale
    '        End If
    '        '*** 20130308 - devo memorizzare il conto corrente altrimenti non sarà valorizzato nella codeline e nel codice a barre ***
    '        'controllo se esiste già un ruolo della stessa tipologia per l'anno
    '        oMyRuolo = FncRuolo.GetRuolo(ConstSession.IdEnte, -1, ddlAnno.SelectedValue, ddlTipoCalcolo.SelectedValue, 0, False, Nothing, Nothing)
    '        '*** ***
    '        If chkSimulazione.Checked = False Then
    '            If Not IsNothing(oMyRuolo) Then
    '                If oMyRuolo(0).sTipoCalcolo = ObjRuolo.CalcoloAConguaglio And oMyRuolo(0).tDataOKMinuta <> Date.MinValue Then
    '                    'se è già stata approvata la minuta non posso rifarlo
    '                    sScript = "alert('E\' già presente un ruolo approvato per gli stessi parametri.\nImpossibile proseguire!');"
    '                    RegisterScript(sScript, Me.GetType)
    '                    DivAttesa.Style.Add("display", "none")
    '                    Exit Sub
    '                Else
    '                    '**** 201809 - Cartelle Insoluti ***
    '                    If ddlTipoCalcolo.SelectedValue <> ObjRuolo.CalcoloCartelleInsoluti Then
    '                        If oMyRuolo(0).tDataOKDoc = Date.MinValue Then
    '                            If oMyRuolo(0).tDataOKMinuta <> Date.MinValue Then
    '                                'se è già stata approvata la minuta non posso rifarlo
    '                                sScript = "alert('E\' già presente un ruolo approvato per gli stessi parametri.\nEliminarlo prima di poter proseguire!');"
    '                                RegisterScript(sScript, Me.GetType)
    '                                DivAttesa.Style.Add("display", "none")
    '                                Exit Sub
    '                            Else
    '                                'ripulisco per fare il nuovo calcolo
    '                                If FncCalcolo.DeleteRuolo(ConstSession.DBType, ConstSession.StringConnection, ConstSession.IsFromVariabile, oMyRuolo(0)) = False Then
    '                                    sScript = "alert('La cancellazione e\' stata terminata a causa di un errore!');"
    '                                    RegisterScript(sScript, Me.GetType)
    '                                    Exit Sub
    '                                Else
    '                                    'aggiorno la variabile di sessione
    '                                    Session("oRuoloTIA") = Nothing
    '                                End If
    '                            End If
    '                        End If
    '                    Else
    '                        If oMyRuolo(0).tDataOKDoc = Date.MinValue Then
    '                            sScript = "alert('Il ruolo non e\' approvato.\nImpossibile proseguire!');"
    '                            RegisterScript(sScript, Me.GetType)
    '                            Exit Sub
    '                        End If
    '                        ListAddizionali = New GestAddizionali().GetAddizionale(ConstSession.StringConnection, ConstSession.IdEnte, ddlAnno.SelectedValue, "", ddlTipoCalcolo.SelectedValue, Nothing)
    '                        If ListAddizionali Is Nothing Then
    '                            sScript = "alert('Errore in lettura addizionali.\nImpossibile proseguire!');"
    '                            RegisterScript(sScript, Me.GetType)
    '                            Exit Sub
    '                        End If
    '                    End If
    '                End If
    '            End If
    '        End If
    '        'oMyRuolo = FncCalcolo.GeneraRuolo(ConstSession.IdEnte, sTipoElab, LblTipoCalcolo.Text, ddlTipoCalcolo.SelectedValue, ddlTipoCalcolo.SelectedItem.Text, TxtPercentTariffe.Text, ChkMaggiorazione.Checked, ChkConferimenti.Checked, LblTipoMQ.Text, ddlAnno.SelectedValue, txtSogliaMinima.Text, -1)
    '        'If Not oMyRuolo Is Nothing Then
    '        '    'eseguo la cartellazione
    '        '    oMyRuolo(0) = FncCalcolo.CartellaRuolo(WFSessione, oMyRuolo(0))
    '        '    If oMyRuolo(0) Is Nothing Then
    '        '        sScript = "alert('L\'elaborazione e\' stata terminata a causa di un errore!')"
    '        '        RegisterScript(me.gettype(),"", "<script language='javascript'>" & sScript & "</script>")
    '        '    Else
    '        '        If oMyRuolo(0).sNote.StartsWith("Errore") Then
    '        '            sScript = "alert('L\'elaborazione e\' stata terminata a causa di un errore!')"
    '        '            RegisterScript(me.gettype(),"", "<script language='javascript'>" & sScript & "</script>")
    '        '        Else
    '        '            'chiamo la pagina
    '        '            sScript = "alert('Elaborazione terminata correttamente!');"
    '        '            RegisterScript(me.gettype(),"", "<script language='javascript'>" & sScript & "</script>")
    '        '            VisualizzaRiepilogo(oMyRuolo, 1)
    '        '            DivAttesa.Style.Add("display", "none")
    '        '        End If
    '        '    End If
    '        'Else
    '        '    sScript = "alert('L\'elaborazione e\' stata terminata a causa di un errore!')"
    '        '    RegisterScript(me.gettype(),"", "<script language='javascript'>" & sScript & "</script>")
    '        '    DivAttesa.Style.Add("display", "none")
    '        'End If
    '        ShowCalcoloInCorso()
    '        Dim bIsFromVariabile As Boolean = False
    '        If ConstSession.IsFromVariabile = "1" Then
    '            bIsFromVariabile = True
    '        End If

    '        If txtInizioConf.Text = "" Then
    '            txtInizioConf.Text = Date.MaxValue
    '        End If

    '        calcolo.StartCalcolo(ConstSession.DBType, ConstSession.StringConnection, ConstSession.IdEnte, sTipoElab, LblTipoCalcolo.Text, ddlTipoCalcolo.SelectedValue, ddlTipoCalcolo.SelectedItem.Text, TxtPercentTariffe.Text, ChkMaggiorazione.Checked, ChkConferimenti.Checked, LblTipoMQ.Text.Substring(0, 1), ddlAnno.SelectedValue, txtSogliaMinima.Text, -1, chkSimulazione.Checked, bIsFromVariabile, CDate(txtInizioConf.Text), ListAddizionali)
    '        'aggiorno la variabile di sessione
    '        Session("oRuoloTIA") = oMyRuolo
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.Calcolo.CmdCalcola_Click.errore: ", Err)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    Finally
    '        'WFSessione.Kill()
    '    End Try
    'End Sub
    ''' <summary>
    ''' Pulsante per la visualizzazione degli avvisi del ruolo in elaborazione.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdVisualizza_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdVisualizza.Click
        Dim sScript As String

        Try
            If LblIdElab.Text = -1 Then
                sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario selezionare un Ruolo!');"
                RegisterScript(sScript, Me.GetType)
            Else
                sScript = "parent.Visualizza.location.href='../Gestione/RicAvvisi.aspx?IsFromVariabile=" & Request.Item("IsFromVariabile") & "';"
                sScript += "parent.Comandi.location.href='../Gestione/ComandiRicAvvisi.aspx?sProvenienza=C';"
                sScript += "parent.Basso.location.href='../../aspVuota.aspx';"
                RegisterScript(sScript, Me.GetType)
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.Calcolo.CmdVisualizza_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub CmdStampaMinuta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdStampaMinuta.Click
    '    Dim NameMinuta, sPathProspetti, Str As String
    '    Dim oMyRuolo() As ObjRuolo = Nothing
    '    Dim sScript As String
    '    Dim x As Integer
    '    Dim FncStampa As New ClsStampaXLS
    '    Dim oListAvvisi() As ObjAvviso
    '    Dim DtDatiStampa As New DataTable
    '    Dim aMyHeaders As String()
    '    Dim aListColonne As ArrayList
    '    Dim FncRuolo As New GestRuolo
    '    'Dim WFErrore As String = ""
    '    'Dim WFSessione As OPENUtility.CreateSessione
    '    NameMinuta = "" : sPathProspetti = ""
    '    Try
    '        'Dim bMinutaConDatiAvviso As Boolean = False
    '        'If Not (Session("MinutaConDatiAvviso")) Is Nothing Then
    '        '    bMinutaConDatiAvviso = Session("MinutaConDatiAvviso")
    '        'End If

    '        If LblIdElab.Text = -1 Then
    '            sScript = "alert('E\' necessario selezionare un Ruolo!')"
    '            RegisterScript(me.gettype(),"minuta", "<script language='javascript'>" & sScript & "</script>")
    '        Else
    '            'prelevo il ruolo
    '            oMyRuolo = Session("oRuoloTIA")
    '            ''*** 20130308 - devo memorizzare il conto corrente altrimenti non sarà valorizzato nella codeline e nel codice a barre ***
    '            oMyRuolo = FncRuolo.GetRuolo(ConstSession.IdEnte, oMyRuolo(0).IdFlusso, "", "", 0, True, Nothing, Nothing)
    '            ''*** ***
    '            'WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '            'If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '            'End If
    '            'oListAvvisi = oMyRuolo(0).oAvvisi
    '            'If Not oListAvvisi Is Nothing Then
    '            '    'valorizzo il nome del file
    '            '    sPathProspetti = System.Configuration.ConfigurationManager.AppSettings("PATH_PROSPETTI_EXCEL")
    '            sPathProspetti = ConstSession.PathProspetti
    '            NameMinuta = ConstSession.DescrizioneEnte & "_MINUTA_" & ConstSession.IdEnte & "_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
    '            '    DtDatiStampa = FncStampa.PrintMinutaRuolo(oListAvvisi, ConstSession.iDENTE & "-" & ConstSession.DESCRIZIONE_ENTE"))
    '            '    If Not DtDatiStampa Is Nothing Then
    '            '        'aggiorno la data di creazione minuta sul db
    '            '        If UpdateDateRuolo(oMyRuolo, 1, "I") = False Then
    '            '            Response.Redirect("../../../PaginaErrore.aspx")
    '            '        End If

    '            '        'definisco le colonne
    '            '        aListColonne = New ArrayList
    '            '        '*** 20130123 - aggiunti i riferimenti catastali ***
    '            '        For x = 0 To 24                  '21
    '            '            aListColonne.Add("")
    '            '        Next
    '            '        aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())
    '            '        'definisco l'insieme delle colonne da esportare
    '            '        Dim MyCol() As Integer = New Integer() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24}
    '            '        'esporto i dati in excel
    '            '        Dim MyStampa As New RKLib.ExportData.Export("Web")
    '            '        MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, NameMinuta)
    '            '        Str = sPathProspetti & NameMinuta
    '            '    End If
    '            'End If
    '            oListAvvisi = oMyRuolo(0).oAvvisi
    '            If Not oListAvvisi Is Nothing Then
    '                'valorizzo il nome del file
    '                sPathProspetti = ConstSession.PathProspetti
    '                NameMinuta = ConstSession.DescrizioneEnte & "_MINUTA_" & ConstSession.IdEnte & "_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
    '                DtDatiStampa = FncStampa.PrintMinutaRuolo(oListAvvisi, oMyRuolo(0).HasConferimenti, oMyRuolo(0).HasMaggiorazione, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte) ', bMinutaConDatiAvviso)
    '                'aggiorno la data di creazione minuta sul db
    '                If UpdateDateRuolo(oMyRuolo, 1, "I") = False Then
    '                    Response.Redirect("../../../PaginaErrore.aspx")
    '                End If
    '            End If
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.Calcolo.CmdStampaMinuta_Click.errore: ", Err)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    Finally
    '        CacheManager.RemoveAvanzamentoElaborazione()
    '    End Try
    '    If Not DtDatiStampa Is Nothing Then
    '        'definisco le colonne
    '        aListColonne = New ArrayList
    '        '*** 20130123 - aggiunti i riferimenti catastali ***
    '        For x = 0 To 54 '45 '39 '27                  '21
    '            aListColonne.Add("")
    '        Next
    '        aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())
    '        'definisco l'insieme delle colonne da esportare
    '        Dim MyCol() As Integer = New Integer() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 3, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54}
    '        'esporto i dati in excel
    '        Dim MyStampa As New RKLib.ExportData.Export("Win") '("Web") '
    '        MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sPathProspetti & NameMinuta) ' NameMinuta)
    '        Str = sPathProspetti & NameMinuta
    '    End If
    '    'Get the data from database into datatable
    '    'Dim MyDBEngine As DBEngine = Nothing
    '    'Dim dtMyDati As New DataTable()

    '    'MyDBEngine = Utility.DBEngineFactory.GetDBEngine(ConstSession.StringConnection)
    '    'MyDBEngine.OpenConnection()

    '    'MyDBEngine.ClearParameters()
    '    'MyDBEngine.AddParameter("@IdFlusso", oMyRuolo(0).IdFlusso, ParameterDirection.Input)
    '    'MyDBEngine.ExecuteQuery(dtMyDati, "prc_GetMinuta", CommandType.StoredProcedure)

    '    ''Create a dummy GridView
    '    'Dim GridView1 As New GridView()
    '    'GridView1.AllowPaging = False
    '    'GridView1.DataSource = dtMyDati
    '    'GridView1.DataBind()

    '    ''aggiorno la data di creazione minuta sul db
    '    'If UpdateDateRuolo(oMyRuolo, 1, "I") = False Then
    '    '    Response.Redirect("../../../PaginaErrore.aspx")
    '    'End If

    '    'Response.Clear()
    '    'Response.Buffer = True
    '    'Response.AddHeader("content-disposition", "attachment;filename=" & NameMinuta)
    '    'Response.Charset = ""
    '    'Response.ContentType = "application/vnd.ms-excel"
    '    'Dim sw As New StringWriter()
    '    'Dim hw As New HtmlTextWriter(sw)

    '    ''For i As Integer = 0 To GridView1.Rows.Count - 1
    '    ''    'Apply text style to each Row
    '    ''    GridView1.Rows(i).Attributes.Add("class", "textmode")
    '    ''Next
    '    'hw.AddStyleAttribute("border", "0")
    '    'GridView1.RenderControl(hw)

    '    ''style to format numbers to string
    '    'Dim style As String = "<style> .textmode{mso-number-format:\@;}</style>"
    '    'Response.Write(style)
    '    'Response.Output.Write(sw.ToString())
    '    'Response.Flush()
    '    'Response.End()
    'End Sub
    ''' <summary>
    ''' Pulsante per la stampa della minuta del ruolo in elaborazione.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="23/01/2013">
    ''' aggiunti i riferimenti catastali
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="11/2015">
    ''' <strong>PF e PV su unica riga</strong>
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="08/2018">
    ''' <strong>Bollettazione Vigliano in OPENgov</strong>
    ''' <strong>Cartelle Insoluti</strong>
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Esportazione completa dati</em>
    ''' </revision>
    ''' </revisionHistory>
    Private Sub CmdStampaMinuta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdStampaMinuta.Click
        Dim sNameXLS, sPathProspetti As String
        Dim oMyRuolo() As ObjRuolo = Nothing
        Dim sScript As String
        Dim x, nCol As Integer
        Dim FncStampa As New ClsStampaXLS
        Dim oListAvvisi() As ObjAvviso
        Dim DtDatiStampa As New DataTable
        Dim aMyHeaders As String()
        Dim aListColonne As ArrayList
        Dim FncRuolo As New ClsGestRuolo

        sNameXLS = "" : sPathProspetti = ""
        If LblIdElab.Text = -1 Then
            sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario selezionare un Ruolo!');"
            RegisterScript(sScript, Me.GetType)
        Else
            Try
                nCol = 68
                'prelevo il ruolo
                oMyRuolo = Session("oRuoloTIA")
                'valorizzo il nome del file
                sNameXLS = ConstSession.IdEnte + "_MINUTA_" + oMyRuolo(0).sAnno + "_" + Format(Now, "yyyyMMdd_hhmmss") & ".xls"
                If hdPFPVUniqueRow.Value = "1" Then
                    Dim DvDati As New DataView
                    DvDati = FncRuolo.GetMinuta(oMyRuolo(0).IdFlusso)
                    If Not DvDati Is Nothing Then
                        If DvDati.Count > 0 Then
                            DtDatiStampa = FncStampa.PrintMinutaRuolo(DvDati, oMyRuolo(0).HasConferimenti, oMyRuolo(0).HasMaggiorazione, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, hdMinutaAnagAllRow.Value, nCol, hdIsMinutaXStampatore.Value)
                            'aggiorno la data di creazione minuta sul db
                            If FncRuolo.UpdateDateRuolo(oMyRuolo, 1, "I") = False Then
                                Response.Redirect("../../../PaginaErrore.aspx")
                            End If
                            Session("oRuoloTIA") = oMyRuolo
                            VisualizzaRiepilogo(oMyRuolo, 1)
                        End If
                    End If
                Else
                    oListAvvisi = FncRuolo.GetRuoloMinuta(ConstSession.StringConnection, ConstSession.IdEnte, oMyRuolo(0).IdFlusso)
                    If Not oListAvvisi Is Nothing Then
                        DtDatiStampa = FncStampa.PrintMinutaRuolo(oListAvvisi, oMyRuolo(0).HasConferimenti, oMyRuolo(0).HasMaggiorazione, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, hdMinutaAnagAllRow.Value, nCol, hdIsMinutaXStampatore.Value, oMyRuolo(0).sTipoRuolo)
                        'aggiorno la data di creazione minuta sul db
                        If FncRuolo.UpdateDateRuolo(oMyRuolo, 1, "I") = False Then
                            Response.Redirect("../../../PaginaErrore.aspx")
                        End If
                        Session("oRuoloTIA") = oMyRuolo
                        VisualizzaRiepilogo(oMyRuolo, 1)
                    End If
                End If
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.Calcolo.CmdStampaMinuta_Click.errore: ", Err)
                Response.Redirect("../../../PaginaErrore.aspx")
            Finally
                CacheManager.RemoveAvanzamentoElaborazione()
            End Try
            If Not DtDatiStampa Is Nothing Then
                'definisco le colonne
                aListColonne = New ArrayList
                For x = 0 To nCol
                    aListColonne.Add("")
                Next
                aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())
                'definisco l'insieme delle colonne da esportare
                Dim MyCol() As Integer = New Integer(nCol) {}
                For x = 0 To nCol
                    MyCol(x) = x
                Next
                'esporto i dati in excel
                Dim MyStampa As New RKLib.ExportData.Export("Web")
                MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sPathProspetti & sNameXLS)
            End If
        End If
    End Sub
    'Private Sub CmdStampaMinuta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdStampaMinuta.Click
    '    Dim sNameXLS, sPathProspetti, Str As String
    '    Dim oMyRuolo() As ObjRuolo = Nothing
    '    Dim sScript As String
    '    Dim x, nCol As Integer
    '    Dim FncStampa As New ClsStampaXLS
    '    Dim oListAvvisi() As ObjAvviso
    '    Dim DtDatiStampa As New DataTable
    '    Dim aMyHeaders As String()
    '    Dim aListColonne As ArrayList
    '    Dim FncRuolo As New ClsGestRuolo

    '    sNameXLS = "" : sPathProspetti = ""
    '    If LblIdElab.Text = -1 Then
    '        sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario selezionare un Ruolo!');"
    '        RegisterScript(sScript, Me.GetType)
    '    Else
    '        Try
    '            '*** 201809 Bollettazione Vigliano in OPENgov***'**** 201809 - Cartelle Insoluti ***
    '            nCol = 68 '62 '61
    '            'prelevo il ruolo
    '            oMyRuolo = Session("oRuoloTIA")
    '            'valorizzo il nome del file
    '            'sPathProspetti = ConstSession.PathProspetti
    '            sNameXLS = ConstSession.IdEnte + "_MINUTA_" + oMyRuolo(0).sAnno + "_" + Format(Now, "yyyyMMdd_hhmmss") & ".xls"
    '            '*** 201511 - PF e PV su unica riga ***
    '            If hdPFPVUniqueRow.Value = "1" Then
    '                Dim DvDati As New DataView
    '                DvDati = FncRuolo.GetMinuta(oMyRuolo(0).IdFlusso)
    '                If Not DvDati Is Nothing Then
    '                    If DvDati.Count > 0 Then
    '                        DtDatiStampa = FncStampa.PrintMinutaRuolo(DvDati, oMyRuolo(0).HasConferimenti, oMyRuolo(0).HasMaggiorazione, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, hdMinutaAnagAllRow.Value, nCol, hdIsMinutaXStampatore.Value) ', bMinutaConDatiAvviso)
    '                        'aggiorno la data di creazione minuta sul db
    '                        If FncRuolo.UpdateDateRuolo(oMyRuolo, 1, "I") = False Then
    '                            Response.Redirect("../../../PaginaErrore.aspx")
    '                        End If
    '                        Session("oRuoloTIA") = oMyRuolo
    '                        VisualizzaRiepilogo(oMyRuolo, 1)
    '                    End If
    '                End If
    '            Else
    '                oListAvvisi = FncRuolo.GetRuoloMinuta(oMyRuolo(0).IdFlusso)
    '                If Not oListAvvisi Is Nothing Then
    '                    '*** 201809 Bollettazione Vigliano in OPENgov***
    '                    DtDatiStampa = FncStampa.PrintMinutaRuolo(oListAvvisi, oMyRuolo(0).HasConferimenti, oMyRuolo(0).HasMaggiorazione, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, hdMinutaAnagAllRow.Value, nCol, hdIsMinutaXStampatore.Value, oMyRuolo(0).sTipoRuolo) ', bMinutaConDatiAvviso)
    '                    'aggiorno la data di creazione minuta sul db
    '                    If FncRuolo.UpdateDateRuolo(oMyRuolo, 1, "I") = False Then
    '                        Response.Redirect("../../../PaginaErrore.aspx")
    '                    End If
    '                    Session("oRuoloTIA") = oMyRuolo
    '                    VisualizzaRiepilogo(oMyRuolo, 1)
    '                End If
    '            End If
    '        Catch Err As Exception
    '            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.Calcolo.CmdStampaMinuta_Click.errore: ", Err)
    '            Response.Redirect("../../../PaginaErrore.aspx")
    '        Finally
    '            CacheManager.RemoveAvanzamentoElaborazione()
    '        End Try
    '        If Not DtDatiStampa Is Nothing Then
    '            'definisco le colonne
    '            aListColonne = New ArrayList
    '            '*** 201511 - PF e PV su unica riga ***'*** 20130123 - aggiunti i riferimenti catastali ***
    '            For x = 0 To nCol '54 '45 '39 '27                  '21
    '                aListColonne.Add("")
    '            Next
    '            aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())
    '            'definisco l'insieme delle colonne da esportare
    '            'Dim MyCol() As Integer = New Integer() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60}
    '            Dim MyCol() As Integer = New Integer(nCol) {}
    '            For x = 0 To nCol
    '                MyCol(x) = x
    '            Next
    '            'esporto i dati in excel
    '            Dim MyStampa As New RKLib.ExportData.Export("Web") '("Win") '
    '            MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sPathProspetti & sNameXLS) ' NameMinuta)
    '            Str = sPathProspetti & sNameXLS
    '        End If
    '    End If
    'End Sub
    ''' <summary>
    ''' Pulsante per l'approvazione della minuta del ruolo in elaborazione.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdApprovaMinuta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdApprovaMinuta.Click
        Dim sScript As String
        Dim oMyRuolo() As ObjRuolo

        Try
            If LblIdElab.Text = -1 Then
                sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario selezionare un Ruolo!');"
                RegisterScript(sScript, Me.GetType)
            Else
                'prelevo il ruolo
                oMyRuolo = Session("oRuoloTIA")
                If oMyRuolo(0).tDataStampaMinuta = Date.MaxValue Then
                    sScript = "GestAlert('a', 'warning', '', '', 'Produrre la Minuta prima di fare l\'approvazione!');"
                    RegisterScript(sScript, Me.GetType)
                Else
                    If New ClsGestRuolo().UpdateDateRuolo(oMyRuolo, 2, "I") = False Then
                        Response.Redirect("../../PaginaErrore.aspx")
                        Exit Sub
                    End If
                    Session("oRuoloTIA") = oMyRuolo
                    VisualizzaRiepilogo(oMyRuolo, 1)
                    sScript = "GestAlert('a', 'success', '', '', 'Approvazione effettuata con succcesso!');"
                    RegisterScript(sScript, Me.GetType)
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.Calcolo.CmdApprovaMinuta_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    '**** 201809 - Cartelle Insoluti ***
    ''' <summary>
    ''' Pulsante per il calcolo delle rate del ruolo in elaborazione.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdCartella_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdCartella.Click
        Dim sScript As String
        Dim FncRuolo As New ClsGestRuolo
        Dim oMyRuolo() As ObjRuolo
        Dim FncCalcolo As New ClsElabRuolo
        Dim FncCC As New OPENUtility.ClsContiCorrenti
        Dim oContoCorrente As OPENUtility.objContoCorrente
        Dim x As Integer

        Try
            If LblIdElab.Text = -1 Then
                sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario selezionare un Ruolo!');"
                RegisterScript(sScript, Me.GetType)
            Else
                'prelevo il ruolo
                oMyRuolo = Session("oRuoloTIA")
                If oMyRuolo(0).tDataOKMinuta = Date.MaxValue Then
                    sScript = "GestAlert('a', 'warning', '', '', 'Approvare la Minuta prima di fare la cartellazione!');"
                    RegisterScript(sScript, Me.GetType)
                ElseIf oMyRuolo(0).tDataCartellazione <> Date.MaxValue Then
                    sScript = "GestAlert('a', 'warning', '', '', 'Cartellazione gia\' eseguita!');"
                    RegisterScript(sScript, Me.GetType)
                Else
                    'verifico la presenza del conto corrente
                    oContoCorrente = FncCC.GetContoCorrente(ConstSession.IdEnte, Utility.Costanti.TRIBUTO_TARSU, ConstSession.UserName, ConstSession.StringConnectionOPENgov)
                    If oContoCorrente Is Nothing Then
                        sScript = "GestAlert('a', 'warning', '', '', 'Non e\' presente il Conto Corrente.\nImpossibile proseguire!');"
                        RegisterScript(sScript, Me.GetType)
                        Exit Sub
                    End If
                    'verifico la presenza delle rate
                    If oMyRuolo(0).oRate Is Nothing Then
                        sScript = "GestAlert('a', 'warning', '', '', 'Non sono state configurate le rate!\nImpossibile proseguire!');"
                        RegisterScript(sScript, Me.GetType)
                        Exit Sub
                    Else
                        'valorizzo il conto sulle rate
                        For x = 0 To oMyRuolo(0).oRate.GetUpperBound(0)
                            oMyRuolo(0).oRate(x).NumeroContoCorrente = oContoCorrente.ContoCorrente
                        Next
                    End If
                    '*** 20131104 - TARES ***
                    'eseguo il calcolo delle rate
                    oMyRuolo(0) = FncCalcolo.CalcoloRate(ConstSession.StringConnection, oMyRuolo(0), -1)
                    '*** ***
                    If oMyRuolo(0) Is Nothing Then
                        sScript = "GestAlert('a', 'danger', '', '', 'L\'elaborazione e\' stata terminata a causa di un errore!');"
                        RegisterScript(sScript, Me.GetType)
                    Else
                        If oMyRuolo(0).sNote.StartsWith("Errore") Then
                            sScript = "GestAlert('a', 'danger', '', '', 'L\'elaborazione e\' stata terminata a causa di un errore!');"
                            RegisterScript(sScript, Me.GetType)
                        Else
                            If FncRuolo.UpdateDateRuolo(oMyRuolo, 3, "I") = False Then
                                Response.Redirect("../../PaginaErrore.aspx")
                                Exit Sub
                            End If
                            Session("oRuoloTIA") = oMyRuolo
                            VisualizzaRiepilogo(oMyRuolo, 1)
                            sScript = "GestAlert('a', 'success', '', '', 'Cartellazione effettuata con succcesso!');"
                            RegisterScript(sScript, Me.GetType)
                        End If
                    End If
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.Calcolo.CmdCartella_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        Finally
            'WFSessione.Kill()
            DivAttesa.Style.Add("display", "none")
        End Try
    End Sub
    ''' <summary>
    ''' Pulsante per il reindirizzamento alla pagina di elaborazione documenti.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdDocumenti_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdDocumenti.Click
        Dim sScript As String
        Dim oMyRuolo() As ObjRuolo

        Try
            If LblIdElab.Text = -1 Then
                sScript = "GestAlert('a', 'success', '', '', 'E\' necessario selezionare un Ruolo!')"
                RegisterScript(sScript, Me.GetType)
            Else

                'prelevo il ruolo
                oMyRuolo = Session("oRuoloTIA")
                If oMyRuolo(0).tDataCartellazione = Date.MaxValue Then
                    sScript = "GestAlert('a', 'warning', '', '', 'Cartellazione non eseguita.\nImpossibile proseguire!');"
                    RegisterScript(sScript, Me.GetType)
                Else
                    sScript = "parent.Visualizza.location.href='../Documenti/RicercaDoc.aspx?IdUniqueTestata=" & LblIdElab.Text & "&AzioneProv=" & Request.Item("AzioneProv") & "';"
                    sScript += "parent.Comandi.location.href='../Documenti/ComandiDoc.aspx';"
                    sScript += "parent.Basso.location.href='../../aspVuota.aspx';"
                    RegisterScript(sScript, Me.GetType)
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.Calcolo.CmdDocumenti_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Pulsante per la cancellazione del ruolo in elaborazione.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdDeleteElab_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdDeleteElab.Click
        Dim sScript As String
        Dim oMyRuolo() As ObjRuolo
        Dim FncCalcolo As New ClsElabRuolo

        Try
            If LblIdElab.Text = -1 Then
                sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario selezionare un Ruolo!')"
                RegisterScript(sScript, Me.GetType)
            Else
                'prelevo il ruolo
                oMyRuolo = Session("oRuoloTIA")
                If oMyRuolo(0).tDataElabDoc <> Date.MaxValue Then
                    sScript = "GestAlert('a', 'warning', '', '', 'Documenti gia\' elaborati.\nImpossibile proseguire!')"
                    RegisterScript(sScript, Me.GetType)
                Else
                    'visualizzo il pannello di attesa
                    DivAttesa.Style.Add("display", "")
                    'elimino l'elaborazione
                    If FncCalcolo.DeleteRuolo(ConstSession.DBType, ConstSession.StringConnection, ConstSession.StringConnectionOPENgov, ConstSession.IsFromVariabile, oMyRuolo(0)) = False Then
                        sScript = "GestAlert('a', 'danger', '', '', 'La cancellazione e\' stata terminata a causa di un errore!')"
                        RegisterScript(sScript, Me.GetType)
                    Else
                        'aggiorno la variabile di sessione
                        Session("oRuoloTIA") = Nothing
                        sScript = "GestAlert('a', 'success', '', '', 'Cancellazione effettuata con succcesso!')"
                        RegisterScript(sScript, Me.GetType)
                    End If
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.Calcolo.CmdDeleteElab_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        Finally
            DivAttesa.Style.Add("display", "none")
        End Try
    End Sub
    ''' <summary>
    ''' Pulsante per la visualizzazione del ruolo in elaborazione suddiviso per categoria.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdDettaglio_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdDettaglio.Click
        Try
            'visualizzo il pannello di dettaglio
            RegisterScript("DivDettaglio.style.display='';", Me.GetType)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.Calcolo.CmdDettaglio_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Pulsante per la stampa della minuta avvisi del ruolo selezionato.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdMinutaAvvisi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdMinutaAvvisi.Click
        'uso questo comando per ristampare la minuta
        Dim sNameXLS As String
        Dim sScript As String
        Dim x, nCol As Integer
        Dim FncStampa As New ClsStampaXLS
        Dim DtDatiStampa As DataTable = Nothing
        Dim aMyHeaders As String()
        Dim aListColonne As ArrayList
        Dim FncRuolo As New ClsGestRuolo

        Try
            nCol = 22
            If CInt(hfIdRuoloMinutaAvvisi.Value) <= 0 And LblIdElab.Text <= 0 Then
                sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario selezionare un Ruolo!');"
                RegisterScript(sScript, Me.GetType)
            Else
                '*** 20130308 - devo memorizzare il conto corrente altrimenti non sarà valorizzato nella codeline e nel codice a barre ***
                'prelevo il ruolo
                If CInt(hfIdRuoloMinutaAvvisi.Value) <= 0 Then
                    x = CInt(LblIdElab.Text)
                Else
                    x = CInt(hfIdRuoloMinutaAvvisi.Value)
                End If
                Dim DvDati As New DataView
                DtDatiStampa = New DataTable
                DvDati = FncRuolo.GetMinutaAvvisi(x)
                If Not DvDati Is Nothing Then
                    If DvDati.Count > 0 Then
                        DtDatiStampa = FncStampa.PrintMinutaAvvisi(DvDati, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, ChkMaggiorazione.Checked, ChkConferimenti.Checked, nCol)
                    Else
                        sScript = "GestAlert('a', 'danger', '', '', 'Si e\' verificato un\'errore durante la selezione dei dati.\nImpossibile proseguire nell\'estrazione!');"
                        RegisterScript(sScript, Me.GetType)
                        DtDatiStampa = Nothing
                    End If
                Else
                    sScript = "GestAlert('a', 'danger', '', '', 'Si e\' verificato un\'errore durante la selezione dei dati.\nImpossibile proseguire nell\'estrazione!');"
                    RegisterScript(sScript, Me.GetType)
                    DtDatiStampa = Nothing
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.Calcolo.CmdMinutaAvvisi_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
        If Not DtDatiStampa Is Nothing Then
            'valorizzo il nome del file
            sNameXLS = ConstSession.IdEnte & "_MINUTAAVVISI_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
            'definisco le colonne
            aListColonne = New ArrayList
            For x = 0 To nCol
                aListColonne.Add("")
            Next
            aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())
            'definisco l'insieme delle colonne da esportare
            Dim MyCol() As Integer = New Integer(nCol) {}
            For x = 0 To nCol
                MyCol(x) = x
            Next
            'esporto i dati in excel
            Dim MyStampa As New RKLib.ExportData.Export("Web")
            MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
        End If
    End Sub
    '*** 201511 - template documenti per ruolo ***
    ''' <summary>
    ''' Pulsante per l'upload a sistema del template specifico del ruolo in elaborazione.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub CmdUploadClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdUpload.Click
        Try
            If LblIdElab.Text = -1 Then
                Dim sScript As String = "GestAlert('a', 'warning', '', '', 'E\' necessario selezionare un Ruolo!');"
                RegisterScript(sScript, Me.GetType)
            Else
                If System.IO.Path.GetFileName(fuMyFiles.PostedFile.FileName) = "" Then
                    RegisterScript("GestAlert('a', 'warning', '', '', 'E\' necessario selezionare un file!');", Me.GetType)
                Else
                    Dim myTemplateDoc As New ElaborazioneDatiStampeInterface.ObjTemplateDoc
                    myTemplateDoc.myStringConnection = ConstSession.StringConnectionOPENgov
                    myTemplateDoc.IdEnte = ConstSession.IdEnte
                    myTemplateDoc.IdTributo = ConstSession.CodTributo
                    myTemplateDoc.IdRuolo = CInt(LblIdElab.Text)
                    myTemplateDoc.FileMIMEType = fuMyFiles.PostedFile.ContentType
                    myTemplateDoc.PostedFile = fuMyFiles.FileBytes
                    myTemplateDoc.FileName = System.IO.Path.GetFileName(fuMyFiles.PostedFile.FileName)
                    myTemplateDoc.IdTemplateDoc = myTemplateDoc.Save()
                    If myTemplateDoc.IdTemplateDoc <= 0 Then
                        lblMessage.Text = "Si sono verificati errori durante il salvataggio del file."
                    Else
                        lblMessage.Text = "File caricato con successo."
                        lblMessage.CssClass = "Input_Label_bold"
                    End If

                    lblMessage.Visible = True
                    fuMyFiles = New FileUpload
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.Calcolo.CmdUpload_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        Finally
            DivAttesa.Style.Add("display", "none")
        End Try
    End Sub
    ''' <summary>
    ''' Pulsante per il download del template specifico del ruolo in elaborazione.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub CmdDownloadClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdDownload.Click
        Dim myTemplateDoc As New ElaborazioneDatiStampeInterface.ObjTemplateDoc
        Try
            If LblIdElab.Text = -1 Then
                Dim sScript As String = "GestAlert('a', 'warning', '', '', 'E\' necessario selezionare un Ruolo!');"
                RegisterScript(sScript, Me.GetType)
            Else
                myTemplateDoc.myStringConnection = ConstSession.StringConnectionOPENgov
                myTemplateDoc.IdEnte = ConstSession.IdEnte
                myTemplateDoc.IdTributo = ConstSession.CodTributo
                myTemplateDoc.IdRuolo = CInt(LblIdElab.Text)
                myTemplateDoc.Load()
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.Calcolo.CmdDownload_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        Finally
            DivAttesa.Style.Add("display", "none")
        End Try
        If Not myTemplateDoc.PostedFile Is Nothing Then
            Response.ContentType = myTemplateDoc.FileMIMEType
            Response.AddHeader("content-disposition", String.Format("attachment;filename=""{0}""", myTemplateDoc.FileName))
            Response.BinaryWrite(myTemplateDoc.PostedFile)
            Response.End()
        End If
    End Sub
    '*** ***

#Region "Griglie"
    ''' <summary>
    ''' Gestione degli eventi sulla griglia. Con il comando RowOpen viene visualizzato il ruolo selezionato suddivo per categoria.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim IDRow As Integer = CInt(e.CommandArgument.ToString())
            If e.CommandName = "RowOpen" Then
                Dim listArticoloVSCat() As ObjArticolo
                listArticoloVSCat = CacheManager.GetRiepilogoCalcoloMassivoArtVSCatPF
                If Not IsNothing(listArticoloVSCat) Then
                    'popolo la griglia
                    GrdRuoliVsCatPF.DataSource = listArticoloVSCat
                    GrdRuoliVsCatPF.DataBind()
                    GrdRuoliVsCatPF.SelectedIndex = -1
                    LblResultRuoliVsCatPF.Text = "PARTE FISSA"
                Else
                    LblResultRuoliVsCatPF.Text = "PARTE FISSA - non ci sono risultati."
                End If
                'If LblTipoCalcolo.Text = ObjRuolo.TipoCalcolo.TARES Then
                listArticoloVSCat = CacheManager.GetRiepilogoCalcoloMassivoArtVSCatPV
                If Not IsNothing(listArticoloVSCat) Then
                    'popolo la griglia
                    GrdRuoliVsCatPV.DataSource = listArticoloVSCat
                    GrdRuoliVsCatPV.DataBind()
                    GrdRuoliVsCatPV.SelectedIndex = -1
                    LblResultRuoliVsCatPV.Text = "PARTE VARIABILE"
                Else
                    LblResultRuoliVsCatPV.Text = "PARTE VARIABILE - non ci sono risultati."
                End If
                'End If
                'If ChkConferimenti.Checked = True Then
                listArticoloVSCat = CacheManager.GetRiepilogoCalcoloMassivoArtVSCatPC
                If Not IsNothing(listArticoloVSCat) Then
                    'popolo la griglia
                    GrdRuoliVsCatPC.DataSource = listArticoloVSCat
                    GrdRuoliVsCatPC.DataBind()
                    GrdRuoliVsCatPC.SelectedIndex = -1
                    LblResultRuoliVsCatPC.Text = "CONFERIMENTI"
                Else
                    LblResultRuoliVsCatPC.Text = "CONFERIMENTI - non ci sono risultati."
                End If
                'End If
                'If ChkMaggiorazione.Checked = True Then
                listArticoloVSCat = CacheManager.GetRiepilogoCalcoloMassivoArtVSCatPM
                If Not IsNothing(listArticoloVSCat) Then
                    'popolo la griglia
                    GrdRuoliVsCatPM.DataSource = listArticoloVSCat
                    GrdRuoliVsCatPM.DataBind()
                    GrdRuoliVsCatPM.SelectedIndex = -1
                    LblResultRuoliVsCatPM.Text = "MAGGIORAZIONE"
                Else
                    LblResultRuoliVsCatPM.Text = "MAGGIORAZIONE - non ci sono risultati."
                End If
                'End If
                DivDettaglio.Style.Add("display", "")
            ElseIf e.CommandName = "RowPrint" Then
                hfIdRuoloMinutaAvvisi.Value = IDRow
                sScript = "if (confirm('Si vuole eseguire la stampa minuta avvisi?'))"
                sScript += "{DivAttesa.style.display='';document.getElementById('CmdMinutaAvvisi').click();}"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.Calcolo.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub

#End Region

    'Private Sub GrdRuoliPrec_UpdateCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GrdRuoliPrec.UpdateCommand
    '    Dim sScript As String
    '    hfIdRuoloMinutaAvvisi.Value = CInt(e.Item.Cells(9).Text)
    '    sScript = "if (confirm('Si vuole eseguire la stampa minuta avvisi?'))"
    '    sScript += "{DivAttesa.style.display='';FrmCalcolo.CmdMinutaAvvisi.click();}"
    '    RegisterScript(me.gettype, "GrdRuoliPrec", "<script language='javascript'>" & sScript & "</script>")
    'End Sub
    'Private Sub GrdDateElaborazione_UpdateCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GrdDateElaborazione.UpdateCommand
    '    Dim listArticoloVSCat() As ObjArticolo
    '    Try
    '        listArticoloVSCat = CacheManager.GetRiepilogoCalcoloMassivoArtVSCatPF
    '        If Not IsNothing(listArticoloVSCat) Then
    '            'popolo la griglia
    '            GrdRuoliVsCatPF.DataSource = listArticoloVSCat
    '            GrdRuoliVsCatPF.start_index = Convert.ToString(0)
    '            GrdRuoliVsCatPF.Rows.Count = listArticoloVSCat.Length
    '            GrdRuoliVsCatPF.DataBind()
    '            GrdRuoliVsCatPF.SelectedIndex = -1
    '            LblResultRuoliVsCatPF.Text = "PARTE FISSA"
    '        Else
    '            LblResultRuoliVsCatPF.Text = "PARTE FISSA - non ci sono risultati."
    '        End If
    '        'If LblTipoCalcolo.Text = ObjRuolo.TipoCalcolo.TARES Then
    '        listArticoloVSCat = CacheManager.GetRiepilogoCalcoloMassivoArtVSCatPV
    '        If Not IsNothing(listArticoloVSCat) Then
    '            'popolo la griglia
    '            GrdRuoliVsCatPV.DataSource = listArticoloVSCat
    '            GrdRuoliVsCatPV.start_index = Convert.ToString(0)
    '            GrdRuoliVsCatPV.Rows.Count = listArticoloVSCat.Length
    '            GrdRuoliVsCatPV.DataBind()
    '            GrdRuoliVsCatPV.SelectedIndex = -1
    '            LblResultRuoliVsCatPV.Text = "PARTE VARIABILE"
    '        Else
    '            LblResultRuoliVsCatPV.Text = "PARTE VARIABILE - non ci sono risultati."
    '        End If
    '        'End If
    '        'If ChkConferimenti.Checked = True Then
    '        listArticoloVSCat = CacheManager.GetRiepilogoCalcoloMassivoArtVSCatPC
    '        If Not IsNothing(listArticoloVSCat) Then
    '            'popolo la griglia
    '            GrdRuoliVsCatPC.DataSource = listArticoloVSCat
    '            GrdRuoliVsCatPC.start_index = Convert.ToString(0)
    '            GrdRuoliVsCatPC.Rows.Count = listArticoloVSCat.Length
    '            GrdRuoliVsCatPC.DataBind()
    '            GrdRuoliVsCatPC.SelectedIndex = -1
    '            LblResultRuoliVsCatPC.Text = "CONFERIMENTI"
    '        Else
    '            LblResultRuoliVsCatPC.Text = "CONFERIMENTI - non ci sono risultati."
    '        End If
    '        'End If
    '        'If ChkMaggiorazione.Checked = True Then
    '        listArticoloVSCat = CacheManager.GetRiepilogoCalcoloMassivoArtVSCatPM
    '        If Not IsNothing(listArticoloVSCat) Then
    '            'popolo la griglia
    '            GrdRuoliVsCatPM.DataSource = listArticoloVSCat
    '            GrdRuoliVsCatPM.start_index = Convert.ToString(0)
    '            GrdRuoliVsCatPM.Rows.Count = listArticoloVSCat.Length
    '            GrdRuoliVsCatPM.DataBind()
    '            GrdRuoliVsCatPM.SelectedIndex = -1
    '            LblResultRuoliVsCatPM.Text = "MAGGIORAZIONE"
    '        Else
    '            LblResultRuoliVsCatPM.Text = "MAGGIORAZIONE - non ci sono risultati."
    '        End If
    '        'End If
    '        DivDettaglio.Style.Add("display", "")
    '    Catch ex As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.Calcolo.GrdRuoliPrec_UpdateCommand.errore: ", ex)
    '       Response.Redirect("../../../PaginaErrore.aspx")
    '    End Try
    'End Sub



    'Private Sub CmdDeleteElab_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdDeleteElab.Click
    '    Dim sScript, WFErrore As String
    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Dim oMyRuolo() As ObjRuolo
    '    Dim FncCalcolo As New ClsElabRuolo

    '    Try
    '        'inizializzo la connessione
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If
    '        If LblIdElab.Text = -1 Then
    '            sScript = "alert('E\' necessario selezionare un Ruolo!')"
    '            RegisterScript(me.gettype(),"cartella", "<script language='javascript'>" & sScript & "</script>")
    '        Else
    '            'prelevo il ruolo
    '            oMyRuolo = Session("oRuoloTIA")
    '            If oMyRuolo(0).tDataElabDoc <> Date.MinValue Then
    '                sScript = "alert('Documenti gia\' elaborati.\nImpossibile proseguire!')"
    '                RegisterScript(me.gettype(),"cartella", "<script language='javascript'>" & sScript & "</script>")
    '            Else
    '                'visualizzo il pannello di attesa
    '                DivAttesa.Style.Add("display", "")
    '                'elimino l'elaborazione
    '                If FncCalcolo.DeleteRuolo(WFSessione, oMyRuolo(0)) = False Then
    '                    sScript = "alert('La cancellazione e\' stata terminata a causa di un errore!')"
    '                    RegisterScript(me.gettype(),"", "<script language='javascript'>" & sScript & "</script>")
    '                Else
    '                    'aggiorno la variabile di sessione
    '                    Session("oRuoloTIA") = Nothing
    '                    sScript = "alert('Cancellazione effettuata con succcesso!')"
    '                    RegisterScript(me.gettype(),"cartella", "<script language='javascript'>" & sScript & "</script>")
    '                End If
    '            End If
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.Calcolo.CmdDeleteElab_Click.errore: ", Err)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    Finally
    '        DivAttesa.Style.Add("display", "none")
    '        WFSessione.Kill()
    '    End Try
    'End Sub
    '*** 20181011 Dal/Al Conferimenti ***
    ''' <summary>
    ''' Funzione per il caricamento del ruolo in ingresso nella pagina.
    ''' </summary>
    ''' <param name="oRuolo"></param>
    ''' <param name="nTipo"></param>
    Private Sub VisualizzaRiepilogo(ByVal oRuolo() As ObjRuolo, ByVal nTipo As Integer)
        'nTipo {0= da elaborare, 1=in elaborazione}
        Dim sScript As String = ""
        Try
            If Not IsNothing(oRuolo) Then
                sScript += "$('#DivRiepilogoDaElab').show();"
                LblIdElab.Text = oRuolo(0).IdFlusso
                '*** 201809 Bollettazione Vigliano in OPENgov ***
                Select Case oRuolo(0).TipoGenerazione
                    Case ObjRuolo.Generazione.DaDichiarazione
                        optAutomatica.Checked = True
                    Case ObjRuolo.Generazione.DaFlusso
                        optFlusso.Checked = True
                        sScript += "$('#lblUploadFiles').text('Flussi');$('#lblNoteFlussi').show();$('#DivRiepilogoDaElab').hide();"
                    Case ObjRuolo.Generazione.Manuale
                        optManuale.Checked = True
                End Select
                Select Case nTipo
                    Case 0
                        'da elaborare
                        LblNDom.Text = oRuolo(0).nUtentiDom
                        LblNNonDom.Text = oRuolo(0).nUtentiNonDom
                        LblNContribDaElab.Text = oRuolo(0).nContribuenti
                        LblMQDom.Text = oRuolo(0).nMQDom
                        LblMQNonDom.Text = oRuolo(0).nMQNonDom
                        LblMQDaElab.Text = oRuolo(0).nMQ
                        LblNTessereDaElab.Text = oRuolo(0).nTessere
                        LblTesNoUIDaElab.Text = oRuolo(0).nTesNoUI
                        LblTesBidoneDaElab.Text = oRuolo(0).nTesBidone
                        LblConferimentiDaElab.Text = oRuolo(0).nConferimenti
                        If LblTipoCalcolo.Text = ObjRuolo.TipoCalcolo.TARSU Then
                            LblIntestNDom.Visible = False
                            LblNDom.Visible = False
                            LblIntestNNonDom.Visible = False
                            LblNNonDom.Visible = False
                            LblIntestMQDom.Visible = False
                            LblMQDom.Visible = False
                            LblIntestMQNonDom.Visible = False
                            LblMQNonDom.Visible = False
                        Else
                            LblIntestNDom.Visible = True
                            LblNDom.Visible = True
                            LblIntestNNonDom.Visible = True
                            LblNNonDom.Visible = True
                            LblIntestMQDom.Visible = True
                            LblMQDom.Visible = True
                            LblIntestMQNonDom.Visible = True
                            LblMQNonDom.Visible = True
                        End If
                        If ChkConferimenti.Checked = False Then
                            LblIntestTesBidoneDaElab.Visible = False
                            LblTesBidoneDaElab.Visible = False
                            LblIntestTessereDaElab.Visible = False
                            LblNTessereDaElab.Visible = False
                            LblIntestTesNoUI.Visible = False
                            LblTesNoUIDaElab.Visible = False
                            LblIntestConf.Visible = False
                            LblConferimentiDaElab.Visible = False
                        Else
                            LblIntestTesBidoneDaElab.Visible = True
                            LblTesBidoneDaElab.Visible = True
                            LblIntestTessereDaElab.Visible = True
                            LblNTessereDaElab.Visible = True
                            LblIntestTesNoUI.Visible = True
                            LblTesNoUIDaElab.Visible = True
                            LblIntestConf.Visible = True
                            LblConferimentiDaElab.Visible = True
                        End If
                    Case 1
                        sScript += "$('#DivRiepilogoElab').show();"
                        'elaborazione in corso
                        TxtPercentTariffe.Text = FormatNumber(oRuolo(0).PercentTariffe, 2)
                        txtSogliaMinima.Text = FormatNumber(oRuolo(0).ImpMinimo, 2)
                        txtInizioConf.Text = oRuolo(0).tDataInizioConf
                        txtFineConf.Text = oRuolo(0).tDataFineConf
                        LblNContrib.Text = oRuolo(0).nContribuenti
                        LblNDoc.Text = oRuolo(0).nAvvisi
                        LblNScarti.Text = oRuolo(0).nScarti
                        LblImpPF.Text = FormatNumber(oRuolo(0).ImpPF, 2)
                        LblImpPV.Text = FormatNumber(oRuolo(0).ImpPV, 2)
                        LblImpPM.Text = FormatNumber(oRuolo(0).impPM, 2)
                        LblImpPC.Text = FormatNumber(oRuolo(0).impPC, 2)
                        LblImpAddiz.Text = FormatNumber(oRuolo(0).impAddizionali, 2)
                        LblImpTot.Text = FormatNumber(oRuolo(0).ImpAvvisi, 2)
                        LblNote.Text = oRuolo(0).sNote
                        LblNote.Font.Bold = True
                        If LblTipoCalcolo.Text = ObjRuolo.TipoCalcolo.TARSU Then
                            Label7.Visible = False
                            LblImpPV.Visible = False
                        Else
                            Label7.Visible = True
                            LblImpPV.Visible = True
                        End If
                        If ChkConferimenti.Checked = False Then
                            LblIntestImpPC.Visible = False
                            LblImpPC.Visible = False
                        Else
                            LblIntestImpPC.Visible = True
                            LblImpPC.Visible = True
                        End If
                        If ChkMaggiorazione.Checked = False Then
                            LblIntestImpPM.Visible = False
                            LblImpPM.Visible = False
                        Else
                            LblIntestImpPM.Visible = True
                            LblImpPM.Visible = True
                        End If

                        'popolo la griglia
                        GrdDateElaborazione.Visible = True
                        GrdDateElaborazione.DataSource = oRuolo
                        GrdDateElaborazione.DataBind()
                        GrdDateElaborazione.SelectedIndex = -1

                        '*** 201809 Bollettazione Vigliano in OPENgov ***
                        Dim ListRuoli() As ObjRuolo = CacheManager.GetRiepilogoImportRuoli
                        If Not ListRuoli Is Nothing Then
                            sScript += "$('#fsRuoliImportati').show();$('#DivRiepilogoElab').hide();$('#DivRiepilogoDaElab').hide();"
                            GrdDateElaborazione.Visible = False
                            GrdRuoliImportati.Visible = True
                            GrdRuoliImportati.DataSource = ListRuoli
                            GrdRuoliImportati.DataBind()
                            GrdRuoliImportati.SelectedIndex = -1
                        End If
                End Select
            Else
                LblIdElab.Text = -1

                Select Case nTipo
                    Case 0
                        'da elaborare
                        LblNDom.Text = ""
                        LblNNonDom.Text = ""
                        LblNContribDaElab.Text = ""
                        LblMQDom.Text = ""
                        LblMQNonDom.Text = ""
                        LblMQDaElab.Text = ""
                        LblNTessereDaElab.Text = ""
                        LblTesNoUIDaElab.Text = ""
                        LblTesBidoneDaElab.Text = ""
                        LblConferimentiDaElab.Text = ""
                        If LblTipoCalcolo.Text = ObjRuolo.TipoCalcolo.TARSU Then
                            LblIntestNDom.Visible = False
                            LblNDom.Visible = False
                            LblIntestNNonDom.Visible = False
                            LblNNonDom.Visible = False
                            LblIntestMQDom.Visible = False
                            LblMQDom.Visible = False
                            LblIntestMQNonDom.Visible = False
                            LblMQNonDom.Visible = False
                        Else
                            LblIntestNDom.Visible = True
                            LblNDom.Visible = True
                            LblIntestNNonDom.Visible = True
                            LblNNonDom.Visible = True
                            LblIntestMQDom.Visible = True
                            LblMQDom.Visible = True
                            LblIntestMQNonDom.Visible = True
                            LblMQNonDom.Visible = True
                        End If
                        If ChkConferimenti.Checked = False Then
                            LblIntestTesBidoneDaElab.Visible = False
                            LblTesBidoneDaElab.Visible = False
                            LblIntestTessereDaElab.Visible = False
                            LblNTessereDaElab.Visible = False
                            LblIntestTesNoUI.Visible = False
                            LblTesNoUIDaElab.Visible = False
                            LblIntestConf.Visible = False
                            LblConferimentiDaElab.Visible = False
                        Else
                            LblIntestTesBidoneDaElab.Visible = True
                            LblTesBidoneDaElab.Visible = True
                            LblIntestTessereDaElab.Visible = True
                            LblNTessereDaElab.Visible = True
                            LblIntestTesNoUI.Visible = True
                            LblTesNoUIDaElab.Visible = True
                            LblIntestConf.Visible = True
                            LblConferimentiDaElab.Visible = True
                        End If
                    Case 1
                        'elaborazione in corso
                        TxtPercentTariffe.Text = "100"
                        txtSogliaMinima.Text = "0"
                        txtInizioConf.Text = Date.Now().ToShortDateString()
                        txtFineConf.Text = ""
                        LblNContrib.Text = ""
                        LblNDoc.Text = ""
                        LblNScarti.Text = ""
                        LblImpPF.Text = ""
                        LblImpPV.Text = ""
                        LblImpPM.Text = ""
                        LblImpPC.Text = ""
                        LblImpAddiz.Text = ""
                        LblImpTot.Text = ""
                        LblNote.Text = ""
                        LblNote.Font.Bold = True
                        If LblTipoCalcolo.Text = ObjRuolo.TipoCalcolo.TARSU Then
                            Label7.Visible = False
                            LblImpPV.Visible = False
                        Else
                            Label7.Visible = True
                            LblImpPV.Visible = True
                        End If
                        If ChkConferimenti.Checked = False Then
                            LblIntestImpPC.Visible = False
                            LblImpPC.Visible = False
                        Else
                            LblIntestImpPC.Visible = True
                            LblImpPC.Visible = True
                        End If
                        If ChkMaggiorazione.Checked = False Then
                            LblIntestImpPM.Visible = False
                            LblImpPM.Visible = False
                        Else
                            LblIntestImpPM.Visible = True
                            LblImpPM.Visible = True
                        End If

                        'popolo la griglia
                        GrdDateElaborazione.Visible = False
                End Select
            End If
            RegisterScript(sScript, Me.GetType)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.Calcolo.VisualizzaRiepilogo.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Funzione per la visualizzazione della progressione del calcolo in corso.
    ''' </summary>
    Private Sub ShowCalcoloInCorso()
        DivAttesa.Style.Add("display", "")
        optAutomatica.Enabled = False : optManuale.Enabled = False
        ddlAnno.Enabled = False
        ddlTipoCalcolo.Enabled = False
        TxtPercentTariffe.Enabled = False
        txtSogliaMinima.Enabled = False
        Response.AppendHeader("refresh", "5")
        Session("TARSUAnnoCalcoloInCorso") = ddlAnno.SelectedValue
        DivDettaglio.Style.Add("display", "none")
        LblAvanzamento.Text = CacheManager.GetAvanzamentoElaborazione
    End Sub
    ''' <summary>
    ''' Funzione per il controllo di congruenza dei flussi in ingresso.
    ''' Viene creata una directory d'appoggio per la gestione.
    ''' Ciclando su ogni oggetto viene testata l'estensione del file:
    ''' - ZIP: viene estratto l'archivio e per ogni file estratto viene richiamata la funzione CheckFile
    ''' - TXT: viene richiamata la funzione CheckFile
    ''' </summary>
    ''' <param name="ListFiles">HttpFileCollection flussi selezionati</param>
    ''' <param name="ListFlussi">ByRef ArrayList flussi da importare</param>
    ''' <returns>String descrizione errore</returns>
    Private Function CheckFilesToImport(ListFiles As HttpFileCollection, ByRef ListFlussi As ArrayList) As String
        Dim myError As String = ""
        Dim ListToImport As New ArrayList
        Dim Unzipped As New ICSharpCode.SharpZipLib.Zip.FastZip
        Dim PathCheckFiles As String = ""
        Dim PathUnzipped As String = ""

        Try
            ListFlussi = New ArrayList
            PathCheckFiles = ConstSession.PathImport + ConstSession.IdEnte + "\" + Format(Now, "yyyyMMdd") + "\"
            PathUnzipped = PathCheckFiles + "ZIP\"

            If Directory.Exists(PathCheckFiles) = False Then
                Directory.CreateDirectory(PathCheckFiles)
            End If

            For x As Integer = 0 To ListFiles.Count - 1
                ListToImport.Add(ListFiles(x))
            Next
            For Each myPostedFile As HttpPostedFile In ListToImport
                Try
                    If myPostedFile.ContentLength > 0 Then
                        If myPostedFile.FileName.ToUpper.StartsWith(ConstSession.Belfiore) Then
                            myPostedFile.SaveAs(PathCheckFiles + myPostedFile.FileName)
                            Select Case Path.GetExtension(myPostedFile.FileName.ToUpper)
                                Case ".ZIP"
                                    Try
                                        Unzipped.ExtractZip(PathCheckFiles + myPostedFile.FileName, PathUnzipped, Nothing)
                                        Dim Files() As String
                                        Files = Directory.GetFiles(PathUnzipped)
                                        For Each myItem As String In Files
                                            Dim nCheck As Integer = CheckFile(PathUnzipped, myItem.Replace(PathUnzipped, ""), ListFlussi)
                                            If nCheck = 2 Then
                                                myError = "Il File " + myPostedFile.FileName + " non può essere acquisito!"
                                                Exit For
                                            ElseIf nCheck = 0 Then
                                                myError = "Il File " + myPostedFile.FileName + " non può essere acquisito!"
                                                Exit For
                                            End If
                                        Next
                                    Catch ex As Exception
                                        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.Calcolo.CheckFilesToImport.unzip.errore: ", ex)
                                        myError = "Errore in estrazione dati dal file " + myPostedFile.FileName + "!"
                                        Exit For
                                    End Try
                                Case ".TXT"
                                    Dim nCheck As Integer = CheckFile(PathCheckFiles, myPostedFile.FileName, ListFlussi)
                                    If nCheck = 2 Then
                                        myError = "Il File " + myPostedFile.FileName + " non può essere acquisito!"
                                        Exit For
                                    ElseIf nCheck = 0 Then
                                        myError = "Il File " + myPostedFile.FileName + " non può essere acquisito!"
                                        Exit For
                                    End If
                                Case Else
                            End Select
                        End If
                    End If
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.Calcolo.CheckFilesToImport.carico flussi.errore: ", ex)
                    myError = "Errore in salvataggio files!"
                    Exit For
                End Try
            Next
            If ListFlussi.Count <= 0 Then
                myError = "I files selezionati non sono coerenti con l'ente in lavorazione! Si ricorda che il nome file deve iniziare con il codice catastale dell'ente."
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.Calcolo.CheckFilesToImport.errore: ", ex)
        Finally
            Try
                For Each myItem As String In Directory.GetDirectories(ConstSession.PathImport)
                    If myItem.EndsWith(ConstSession.IdEnte) Then
                        Directory.Delete(myItem, True)
                    End If
                Next
            Catch ex As Exception
            End Try
        End Try
        Return myError
    End Function
    ''' <summary>
    ''' Funzione che controlla la validità del file in ingresso; se conforme il file viene copiato nella cartella di importazione.
    ''' </summary>
    ''' <param name="myPathFile">String Percorso file</param>
    ''' <param name="myFileName">String Nome file</param>
    ''' <param name="ListFlussi">ByRef ArrayList flussi da importare</param>
    ''' <returns>Integer 0: errore, 1: file congruente, 2: non formato 290</returns>
    Private Function CheckFile(myPathFile As String, myFileName As String, ByRef ListFlussi As ArrayList) As Integer
        Try
            Dim myBuffer() As Byte = System.IO.File.ReadAllBytes(myPathFile + myFileName) 'New Byte((myPostedFile.ContentLength) - 1) {}
            Dim myStream As New System.IO.MemoryStream(myBuffer, 0, myBuffer.Length)
            Dim LenStreamReader As New IO.StreamReader(myStream)
            Try
                'se riga non è 290 non copio
                LenStreamReader = New IO.StreamReader(myPathFile + myFileName)
                If LenStreamReader.ReadLine.Length <> 290 Then
                    Return 2
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.Calcolo.CheckFile.errore: ", ex)
                Return 0
            Finally
                LenStreamReader.Close()
            End Try
            If File.Exists(ConstSession.PathImport + myFileName) Then
                File.Delete(ConstSession.PathImport + myFileName)
            End If
            File.Copy(myPathFile + myFileName, ConstSession.PathImport + myFileName)
            ListFlussi.Add(ConstSession.PathImport + myFileName)

            Return 1
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.Calcolo.CheckFile.errore: ", ex)
            Return 0
        End Try
    End Function
End Class
