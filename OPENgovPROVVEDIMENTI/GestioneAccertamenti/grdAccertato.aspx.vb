Imports System.Threading
Imports ComPlusInterface
Imports log4net
Imports AnagInterface
''' <summary>
''' Pagina per la generazione dei provvedimenti IMU.
''' Contiene le funzioni della comandiera e la griglia per la gestione dell'accertato. 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
''' <revisionHistory>
''' <revision date="12/04/2019">
''' <strong>Qualificazione AgID-analisi_rel01</strong>
''' <em>Analisi eventi</em>
''' </revision>
''' </revisionHistory>
Partial Class grdAccertato
    Inherits BaseEnte
    Protected FncGrd As New Formatta.FunctionGrd

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents btnTerritorio As System.Web.UI.WebControls.Button
    Protected WithEvents btnInsManuale As System.Web.UI.WebControls.Button

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    'Private idCelle As New DataGridIndex
    Private glbmese_inizio_p, glbmese_fine_p, glbmese_inizio_s, glbmese_fine_s As Integer

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    'Private objICI As DataSet

    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(grdAccertato))
    Private FncGen As New ClsGestioneAccertamenti
    Private TipoCalcolo As Integer = DichiarazioniICI.CalcoloICI.CalcoloICI.TIPOCalcolo_STANDARD
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Session("COD_TRIBUTO") = Utility.Costanti.TRIBUTO_ICI
        Dim ListUI() As objUIICIAccert
        Dim sScript As String
        Try
            Dim X As Integer = 0
            X += 1
            Log.Debug(X.TOSTRING)
            If Not Session("HashTableDichiarazioniAccertamenti") Is Nothing Then
                If CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable).ContainsKey("ANNOACCERTAMENTO") Then
                    X += 1 * 100
                    Log.Debug(X.ToString)
                    hfAnno.Value = CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("ANNOACCERTAMENTO")
                End If
                If CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable).ContainsKey("CODCONTRIBUENTE") Then
                    X += 1 * 200
                    Log.Debug(X.ToString)
                    hdIdContribuente.Value = CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("CODCONTRIBUENTE")
                End If
            End If
            X += 1
            Log.Debug(X.TOSTRING)
            btnInsManuale.Attributes.Add("onclick", "return ApriInserimentoImmobile();return false;")
            X += 1
            Log.Debug(X.ToString)
            btnAnater.Attributes.Add("onclick", "return ApriRicercaAnater(); return false;")
            btnAnater.Text = ConstSession.NameSistemaTerritorio
            btnAnater.ToolTip = "Ricerca e Selezione Immobile da " & ConstSession.NameSistemaTerritorio
            X += 1
            Log.Debug(X.ToString)
            btnTerritorio.Attributes.Add("onclick", "return ApriRicercaTerritorio(); return false;")
            btnTerritorio.Text = ConstSession.NameSistemaTerritorio
            btnTerritorio.ToolTip = "Ricerca e Selezione Immobile da " & ConstSession.NameSistemaTerritorio
            X += 1
            Log.Debug(X.ToString)
            btnCatasto.Attributes.Add("onclick", "return msg();")
            X += 1
            Log.Debug(X.ToString)
            btnAccertato.Attributes.Add("onclick", "return ApriRicercaAccertato();return false;")

            X += 1
            Log.Debug(X.ToString)
            If Not Page.IsPostBack Then
                txtControlloLegame.Text = "0"
                If Not Session("DataTableImmobiliDaAccertare") Is Nothing Then
                    'CalcolaAccertato()
                    ListUI = Session("DataTableImmobiliDaAccertare")
                    GrdAccertato.DataSource = ListUI
                    GrdAccertato.DataBind()

                    sScript = "visBottoneAccerta()"
                    RegisterScript(sScript, Me.GetType)
                End If
            Else
                GrdAccertato.Visible = True
            End If

            X += 1
            Log.Debug(X.ToString)
            sScript = "getCalcolaSpese()"
            RegisterScript(sScript, Me.GetType)

            X += 1
            Log.Debug(X.ToString)
            sScript = "parent.document.getElementById('attesaCarica').style.display='none';"
            sScript += "parent.document.getElementById('loadGridAccertato').style.display='' ;"
            RegisterScript(sScript, Me.GetType)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.grdAccertato.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '**** 201809 - Cartelle Insoluti ***
    'Private Sub btnAccertamento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccertamento.Click
    '    'Dim objSessione As CreateSessione
    '    Try
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - Inizio Accertamento ICI")
    '        Dim FncGest As New ClsGestioneAccertamenti
    '        Dim objICI() As objSituazioneFinale
    '        Dim objDSDichiaratoIci() As objUIICIAccert
    '        Dim objDTAccertato() As objUIICIAccert
    '        Dim objhashtableRIEPILOGO As New Hashtable
    '        'controllo 
    '        'NON SONO VALIDI immobili dichiarati NON LEGATI a immobili accertati
    '        'mentre SONO VALIDI immobili accertati NON LEGATI a immobili dichiarati 
    '        Dim blnRetValControlloLEGAME As Boolean
    '        Dim TotImpICIDichiarato As Double = 0
    '        Dim objHashTable As New Hashtable
    '        Dim TotImpICIACCERTAMENTO As Double = 0
    '        'Dim AnnoAccertamento As String
    '        Dim iTIPOPROVV_PREACC As Integer
    '        Dim objDSCalcoloSanzioniInteressi As DataSet

    '        Dim TotDiffImpostaDICHIARATO As Double 'Importo Totale Differenza di imposta immobili dichiarati

    '        Dim TotImportoSanzioniACCERTAMENTO As Double 'Totale Sanzioni atto di accertamento
    '        Dim TotImportoSanzioniRidottoACCERTAMENTO As Double 'Totale Sanzioni atto di accertamento
    '        Dim TotImportoInteressiACCERTAMENTO As Double 'Totale Interessi atto di accertamento
    '        Dim TotDiffImpostaACCERTAMENTO As Double 'Importo Totale Differenza di imposta atto di accertamento

    '        Dim dsRiepilogoFase2 As New DataSet
    '        'Dim objDSDichiarazioni As New DataSet
    '        Dim objDSSanzioni As New DataSet
    '        Dim objDSInteressi As New DataSet
    '        Dim dsSanzioniFase2 As New DataSet
    '        Dim dsInteressiFase2 As New DataSet
    '        'Dim objDSImmobiliIci As New DataSet
    '        'Dim objDSContitolariIci As New DataSet
    '        Dim dsVersamenti As New DataSet
    '        Dim TipoAvviso As Integer
    '        Dim DescrTipoAvviso As String = ""
    '        Dim lngNewID_PROVVEDIMENTO As Long

    '        Dim soglia As Double
    '        Dim TotVersamenti As Double
    '        Dim ImportoTotaleAvviso As Double
    '        Dim dblImportoTotaleF2 As Double
    '        Dim blnResult As Boolean = False
    '        Dim TipoAvvisoRimborso As Integer = -1
    '        Dim spese As Double
    '        'Dim strConnectionStringAnagrafica As String = String.Empty
    '        'Dim strConnectionStringOPENgovICI As String = String.Empty
    '        'Dim strConnectionStringOPENgovProvvedimenti As String = String.Empty
    '        'Dim strWFErrore As String

    '        Dim DATA_RETTIFICA_ANNULLAMENTO As String
    '        Dim blnTIPO_OPERAZIONE_RETTIFICA As Boolean = CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("TIPO_OPERAZIONE_RETTIFICA")

    '        Dim dsDettaglioAnagrafica As DataSet = Nothing
    '        Dim oDettaglioAnagrafica As DettaglioAnagrafica
    '        oDettaglioAnagrafica = New DettaglioAnagrafica

    '        oDettaglioAnagrafica = Session("codContribuente")
    '        dsDettaglioAnagrafica = FncGest.addRowsObjAnagrafica(oDettaglioAnagrafica)
    '        If Not IsNothing(oDettaglioAnagrafica) Then
    '            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - CodContribuente:" & oDettaglioAnagrafica.COD_CONTRIBUENTE & " - Cognome: " & oDettaglioAnagrafica.Cognome & " - Nome:" & oDettaglioAnagrafica.Nome)
    '        Else
    '            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - CodContribuente: Non disponibile")
    '        End If

    '        objHashTable = CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - Anno Accertamento:" & CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("ANNOACCERTAMENTO"))
    '        'Calcola ICI su singolo Immobile del dichiarato.
    '        objDSDichiaratoIci = CType(Session("DataSetDichiarazioni"), objUIICIAccert())
    '        If objDSDichiaratoIci Is Nothing Then
    '            RegisterScript("alert('Impossibile effettuare un accertamento. Il contribuente non è stato pre-accertato\nma ha delle dichiarazioni!');", Me.GetType)
    '            Exit Sub
    '        End If
    '        objICI = Session("DATASET_CALCOLO_ICI_IMMOBILI_DICHIARATI")
    '        objDTAccertato = CType(Session("DataTableImmobiliDaAccertare"), objUIICIAccert())
    '        If objDTAccertato Is Nothing Then
    '            RegisterScript("alert('Impossibile effettuare un accertamento. Il contribuente non è stato pre-accertato\nma ha delle dichiarazioni!');", Me.GetType)
    '            Exit Sub
    '        End If

    '        'verifico che i legami siano tutti progressivi
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - verifico che i legami siano tutti progressivi")
    '        If FncGest.CheckProgLegame(objDSDichiaratoIci) = False Then
    '            RegisterScript("alert('Attenzione! I legami non sono consecutivi!');", Me.GetType)
    '            Exit Sub
    '        End If
    '        '08/02/2008 commentato alep come indicato da giulia
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - controllo i legami")
    '        blnRetValControlloLEGAME = FncGest.ControlloLEGAME(objDSDichiaratoIci, objDTAccertato)
    '        If blnRetValControlloLEGAME = False And txtControlloLegame.Text = "0" Then
    '            'presenti immobili dichiarati NON LEGATI a immobili accertati
    '            'Impossibile effettuare un accertamento
    '            RegisterScript("ControlloLegame();", Me.GetType)
    '            Exit Sub
    '        End If
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - controllo legame doppio accertato")
    '        blnRetValControlloLEGAME = FncGest.ControlloLEGAMEdoppioAccertato(objDSDichiaratoIci)
    '        If blnRetValControlloLEGAME = True Then
    '            'presenti immobili accertati doppi per id legame
    '            'Impossibile effettuare un accertamento
    '            RegisterScript("alert('Impossibile effettuare un accertamento. Il sistema ha rilevato più immobili accertati con lo stesso LEGAME!');", Me.GetType)
    '            Exit Sub
    '        End If
    '        'ricalcolo la fase 2 del pre accertamento 

    '        'Session("VALORE_RITORNO_ACCERTAMENTO")=0
    '        ' è stato effettuato un accertamento definitivo, accerto con fase 2

    '        'Session("VALORE_RITORNO_ACCERTAMENTO")=1
    '        'è stato effettuato un pre-accertamento definitivo, accerto senza fase 2

    '        'Session("VALORE_RITORNO_ACCERTAMENTO")=2
    '        'è stato effettuato un pre-accertamento - atto potenziale, accerto con fase 2

    '        'Session("VALORE_RITORNO_ACCERTAMENTO")=3
    '        'non ho effettuato nè ACCERTAMENTO nè PRE-ACCERTAMENTO accerto con fase 2

    '        'Session("VALORE_RITORNO_ACCERTAMENTO")=4
    '        'se per contrib e anno è stato effettuato un accertamento 
    '        'con data di conferma NON presente (NON definitivo)
    '        'accerto senza fase 2 e elimino i dati dell'accertamento precedente

    '        'Session("VALORE_RITORNO_ACCERTAMENTO")=5
    '        'se per contrib e anno è stato effettuato un accertamento 
    '        'con data di conferma NON presente (NON definitivo) 
    '        'e un preaccertamento con data di conferma NON presente  (atto potenziale)
    '        'accerto con fase 2 e elimino i dati dell'accertamento precedente

    '        'Session("VALORE_RITORNO_ACCERTAMENTO")=6
    '        'sto effettuando un accertamento di autotutela

    '        If Not Session("ESCLUDI_PREACCERTAMENTO") Then
    '            'Calcolo Fase 2 PreAccertamento
    '            '*** 201810 - Generazione Massiva Atti ***
    '            Dim ImpDichAcconto, ImpDichSaldo, ImpDichTotale As Double
    '            ImpDichAcconto = 0 : ImpDichSaldo = 0 : ImpDichTotale = 0
    '            For Each myUI As objUIICIAccert In objDSDichiaratoIci
    '                If myUI.Anno = CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("ANNOACCERTAMENTO") Then
    '                    ImpDichAcconto += myUI.AccDovuto
    '                    ImpDichSaldo += myUI.SalDovuto
    '                    ImpDichTotale += myUI.TotDovuto
    '                End If
    '            Next
    '            'If FncGest.CalcoloPreAccertamento(ConstSession.IdEnte, ConstSession.CodTributo, ConstSession.CodTributo, -1, CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("ANNOACCERTAMENTO"), CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("CODCONTRIBUENTE"), CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("ID_PROVVEDIMENTO_RETTIFICA"), CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("DATA_ELABORAZIONE_PER_RETTIFICA"), dsDettaglioAnagrafica, dsRiepilogoFase2, TotVersamenti, dsSanzioniFase2, dsInteressiFase2, objDSDichiaratoIci, dsVersamenti) = False Then
    '            If FncGest.CalcoloPreAccertamento(ConstSession.IdEnte, ConstSession.CodTributo, ConstSession.CodTributo, CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("ANNOACCERTAMENTO"), CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("CODCONTRIBUENTE"), CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("ID_PROVVEDIMENTO_RETTIFICA"), CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("DATA_ELABORAZIONE_PER_RETTIFICA"), dsDettaglioAnagrafica, ImpDichAcconto, ImpDichSaldo, ImpDichTotale, dsRiepilogoFase2, TotVersamenti, dsSanzioniFase2, dsInteressiFase2, dsVersamenti) = False Then
    '                Throw New Exception("errore in calcolopreaccertamento")
    '            End If
    '        End If
    '        '******************************************************************************
    '        Dim controllaAccertato As New DBPROVVEDIMENTI.ProvvedimentiDB
    '        Dim objHashTable1 As Hashtable

    '        objHashTable1 = Session("HashTableDichiarazioniAccertamenti")

    '        'Calcolo la differenza di ICI 
    '        'Se la differenza è > 0 Calcolo Sanzioni e interessi
    '        'Gli immobili sono agganciati da txtLegame 

    '        '**********************************************************************
    '        'Determino il Tipo di Avviso (Accertamento o Ufficio)
    '        'Se trovo 1 immobile di accertato che non è in dichiarato
    '        'ho avviso di Tipo = 4 altrimenti avviso di tipo 3
    '        'se sto effettuando una rettifica dell'avviso dovrò forzare in seguito il tipoavviso
    '        '**********************************************************************
    '        If FncGest.GetTipoAvviso(CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("TIPO_OPERAZIONE_RETTIFICA"), True, objDTAccertato, objDSDichiaratoIci, ImportoTotaleAvviso, dblImportoTotaleF2, iTIPOPROVV_PREACC, TipoAvviso, DescrTipoAvviso) = False Then
    '            Throw New Exception("errore in gettipoavviso")
    '        End If
    '        Session("TipoAvviso") = DescrTipoAvviso
    '        '**************************************************************************
    '        'Fine Tipo avviso
    '        '**************************************************************************

    '        '**************************************************************************
    '        'Calcolo importi/sanzioni/interessi per singoli immobili
    '        '**************************************************************************
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - Calcolo importi/sanzioni/interessi per singoli immobili")
    '        If FncGest.CalcoloImpSanzIntSingleUI(objHashTable, CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("ANNOACCERTAMENTO"), Session("DataSetSanzioni"), objDSDichiaratoIci, TipoAvviso _
    '                , objDSCalcoloSanzioniInteressi, objDSSanzioni, objDSInteressi, objDTAccertato _
    '                , TotDiffImpostaACCERTAMENTO, TotImpICIDichiarato, TotDiffImpostaDICHIARATO, TotImpICIACCERTAMENTO, TotImportoInteressiACCERTAMENTO, TotImportoSanzioniACCERTAMENTO, TotImportoSanzioniRidottoACCERTAMENTO, TotVersamenti) = False Then
    '            Throw New Exception("errore in calcoloimpsanzintsingleui")
    '        End If
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - Terminato ciclo Calcolo importi/sanzioni/interessi per singoli immobili")
    '        Log.Debug("calcolo ImportoTotaleAvviso::" & (TotDiffImpostaACCERTAMENTO + TotImportoSanzioniACCERTAMENTO + TotImportoInteressiACCERTAMENTO))
    '        ImportoTotaleAvviso = TotDiffImpostaACCERTAMENTO + TotImportoSanzioniACCERTAMENTO + TotImportoInteressiACCERTAMENTO

    '        'inserisco in riepilogo i dati relativi alla fase accertamento
    '        If FncGest.LoadRiepilogo(TotImpICIACCERTAMENTO, TotDiffImpostaACCERTAMENTO, TotImportoSanzioniACCERTAMENTO, TotImportoInteressiACCERTAMENTO, ImportoTotaleAvviso, TotImportoSanzioniRidottoACCERTAMENTO, TotVersamenti, TotImpICIDichiarato, objhashtableRIEPILOGO) = False Then
    '            Throw New Exception("errore in load riepilogo")
    '        End If
    '        'agli importi scaturiti dall'accertamento, devo sommare gli importi scaturiti dalla fase 2 (se effettuata)
    '        'If GetImpFinali(objHashTable("ANNOACCERTAMENTO"), objHashTable("CODCONTRIBUENTE"), dsRiepilogoFase2, ImportoTotaleAvviso, TotDiffImpostaACCERTAMENTO, TotDiffImpostaACCERTAMENTO, TotImportoSanzioniRidottoACCERTAMENTO, TotImportoInteressiACCERTAMENTO, TotDiffImpostaDICHIARATO, objDSCalcoloSanzioniInteressi, objhashtableRIEPILOGO, iTIPOPROVV_PREACC, dblImportoTotaleF2) = False Then
    '        If FncGest.GetImpFinali(objHashTable("ANNOACCERTAMENTO"), objHashTable("CODCONTRIBUENTE"), dsRiepilogoFase2, ImportoTotaleAvviso, TotDiffImpostaACCERTAMENTO, TotImportoSanzioniACCERTAMENTO, TotImportoSanzioniRidottoACCERTAMENTO, TotImportoInteressiACCERTAMENTO, TotDiffImpostaDICHIARATO, objDSCalcoloSanzioniInteressi, objhashtableRIEPILOGO, iTIPOPROVV_PREACC, dblImportoTotaleF2) = False Then
    '            Throw New Exception("errore in getimpfinali")
    '        End If
    '        Session("HTRIEPILOGO") = objhashtableRIEPILOGO
    '        If FncGest.GetTipoAvviso(CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("TIPO_OPERAZIONE_RETTIFICA"), False, objDTAccertato, objDSDichiaratoIci, ImportoTotaleAvviso, dblImportoTotaleF2, iTIPOPROVV_PREACC, TipoAvviso, DescrTipoAvviso) = False Then
    '            Throw New Exception("errore in gettipoavviso")
    '        End If

    '        'If objHashTable.ContainsKey("VALORE_RITORNO_ACCERTAMENTO") = True Then
    '        '    objHashTable.Remove("VALORE_RITORNO_ACCERTAMENTO")
    '        'End If
    '        'objHashTable.Add("VALORE_RITORNO_ACCERTAMENTO", Session("VALORE_RITORNO_ACCERTAMENTO"))
    '        Dim objCOMUpdateDBAccertamenti As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
    '        Dim sCodContibuente As String
    '        sCodContibuente = CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("CODCONTRIBUENTE")

    '        '*****************************************************
    '        'reperisco soglia
    '        '*****************************************************
    '        Dim objProvvedimentiDB As New DBPROVVEDIMENTI.ProvvedimentiDB
    '        soglia = 0
    '        soglia = objProvvedimentiDB.GetSogliaMinima(objHashTable("ANNOACCERTAMENTO"), Utility.Costanti.TRIBUTO_ICI, objHashTable("CODENTE"), TipoAvviso)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - Soglia:" & soglia & " €")
    '        '******************************************************

    '        '******************************************************
    '        'Calcolo le spese
    '        '******************************************************
    '        If hfCalcolaSpese.Value = "0" Then
    '            spese = objProvvedimentiDB.GetSpese(objHashTable("ANNOACCERTAMENTO"), Utility.Costanti.TRIBUTO_ICI, objHashTable("CODENTE"), TipoAvviso)
    '        Else
    '            spese = 0
    '        End If
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - Spese:" & spese & " €")
    '        '******************************************************

    '        '***************************************************************************
    '        ' Aggiorno il DB dopo procedura di accertamento
    '        '***************************************************************************
    '        'determino il tipo di provvedimento finale (accertamento + pre accertamento fase 2)
    '        If objHashTable.Contains("TIPOPROVVEDIMENTO") = False Then
    '            objHashTable.Add("TIPOPROVVEDIMENTO", TipoAvviso)
    '        Else
    '            objHashTable("TIPOPROVVEDIMENTO") = TipoAvviso
    '        End If

    '        If blnTIPO_OPERAZIONE_RETTIFICA = True Then
    '            DATA_RETTIFICA_ANNULLAMENTO = DateTime.Now.ToString("yyyyMMdd")
    '            If ImportoTotaleAvviso < soglia Then
    '                TipoAvviso = oggettoatto.provvedimento.autotutelaANNULLAMENTO
    '                objHashTable("TIPOPROVVEDIMENTO") = TipoAvviso
    '                objHashTable.Add("DATA_ANNULLAMENTO", DATA_RETTIFICA_ANNULLAMENTO)
    '                objHashTable.Add("DATA_RETTIFICA", "")
    '            Else
    '                objHashTable.Add("DATA_RETTIFICA", DATA_RETTIFICA_ANNULLAMENTO)
    '                objHashTable.Add("DATA_ANNULLAMENTO", "")
    '            End If
    '        End If
    '        If TipoAvviso <> oggettoatto.provvedimento.noavviso Then
    '            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - Chiamo updateDBAccertamenti")
    '            'Inserisco i dati dell'accertamento nel database
    '            objHashTable("CODTRIBUTO") = ConstSession.CodTributo
    '            'queryResult = objCOMUpdateDBAccertamenti.updateDBAccertamenti(objHashTable, objDSCalcoloSanzioniInteressi, objDSSanzioni, objDSInteressi, spese, objICI, objDSDichiarazioni, objDSAccertato, dsSanzioniFase2, dsInteressiFase2, objDSDichiaratoIci, objDSImmobiliIci, objDSContitolariIci, objDSVersamentiF2)
    '            lngNewID_PROVVEDIMENTO = objCOMUpdateDBAccertamenti.updateDBAccertamenti(objHashTable, objDSCalcoloSanzioniInteressi, objDSSanzioni, objDSInteressi, spese, objICI, objDSDichiaratoIci, objDTAccertato, dsSanzioniFase2, dsInteressiFase2, dsVersamenti)
    '            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - Terminata updateDBAccertamenti")
    '            If lngNewID_PROVVEDIMENTO < 1 Then
    '                Throw New Exception("Errore in inserimento avviso")
    '            End If
    '        Else
    '            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - NON chiamo updateDBAccertamenti perchè NESSUN_AVVISO")
    '        End If

    '        '*******************************************************************************
    '        If ImportoTotaleAvviso < 0 Then
    '            If ImportoTotaleAvviso * (-1) < soglia Then

    '                'Non emetto Avviso
    '                Dim str1 As String
    '                str1 = "parent.document.getElementById('attesaElabAccertamento').style.display='none';"
    '                str1 = str1 & "parent.document.getElementById('attesaCarica').style.display='none';"
    '                If blnTIPO_OPERAZIONE_RETTIFICA = True Then
    '                    'Atto di autotutela in rettifica.
    '                    str1 = str1 & "alert('Importo Avviso inferiore alla soglia.\nE\' stato elaborato un ATTO DI AUTOTUTELA IN RETTIFICA');"
    '                    str1 = str1 & "RiepilogoAccertato('1'," & lngNewID_PROVVEDIMENTO & ");"
    '                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - Importo Avviso inferiore alla soglia. Elaborato ATTO DI AUTOTUTELA IN RETTIFICA")
    '                Else
    '                    'Effettuo il rientro dell'accertato
    '                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - caso 1 - Effettuo il rientro dell'accertato ID_PROVVEDIMENTO=" & lngNewID_PROVVEDIMENTO)
    '                    '*** RIENTRO DA SISTEMARE ***
    '                    'Dim Rientro As New BE_RientroDaAccertamento.ICI
    '                    'Rientro.pathApplication = ConfigurationManager.AppSettings("PATH_LOG_IMMOBILI")
    '                    'Rientro.PARAMETROENV = ConfigurationManager.AppSettings("PARAMETROENV")
    '                    'Rientro.username = ConstSession.UserName
    '                    'Rientro.IdentificativoApplicazione = ConfigurationManager.AppSettings("OPENGOVP")
    '                    'Rientro.CODICE_ENTE = ConstSession.IdEnte
    '                    'If Rientro.Elabora(lngNewID_PROVVEDIMENTO) Then
    '                    '    str1 = str1 & "alert('Importo Avviso inferiore alla soglia.\nL\'accertato è stato inserito come nuova dichiarazione');"
    '                    '    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - Effettuato il rientro dell'accertato")
    '                    'Else
    '                    '    str1 = str1 & "alert('Importo Avviso inferiore alla soglia.\nSi è verificato un errore nel rientro dell'accertato');"
    '                    '    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - Errore nel rientro dell'accertato")
    '                    'End If
    '                End If
    '                RegisterScript(str1, Me.GetType)

    '            End If
    '        Else
    '            'If TipoAvviso = Costanti.PROVVEDIMENTO_NESSUN_AVVISO Then
    '            If ImportoTotaleAvviso = 0 Then
    '                'Non emetto Avviso
    '                Dim str1 As String
    '                str1 = "parent.document.getElementById('attesaElabAccertamento').style.display='none';"
    '                str1 = str1 & "parent.document.getElementById('attesaCarica').style.display='none';"

    '                If blnTIPO_OPERAZIONE_RETTIFICA = True Then
    '                    'Atto di autotutela di annullamento
    '                    str1 = str1 & "alert('La posizione è corretta.\nE\' stato elaborato un ATTO DI AUTOTUTELA DI ANNULLAMENTO');"
    '                    str1 = str1 & "RiepilogoAccertato('1'," & lngNewID_PROVVEDIMENTO & ");"
    '                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - La posizione è corretta. Elaborato ATTO DI AUTOTUTELA DI ANNULLAMENTO")
    '                Else
    '                    'Nessun avviso emesso
    '                    'Effettuo il rientro dell'accertato
    '                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - caso 2 - Effettuo il rientro dell'accertato ID_PROVVEDIMENTO=" & lngNewID_PROVVEDIMENTO)
    '                    '*** RIENTRO DA SISTEMARE ***
    '                    'Dim Rientro As New BE_RientroDaAccertamento.ICI
    '                    'Rientro.pathApplication = ConfigurationManager.AppSettings("PATH_LOG_IMMOBILI")
    '                    'Rientro.PARAMETROENV = ConfigurationManager.AppSettings("PARAMETROENV")
    '                    'Rientro.username = ConstSession.UserName
    '                    'Rientro.IdentificativoApplicazione = ConfigurationManager.AppSettings("OPENGOVP")
    '                    'Rientro.CODICE_ENTE = ConstSession.IdEnte
    '                    'If Rientro.Elabora(lngNewID_PROVVEDIMENTO) Then
    '                    '    str1 = str1 & "alert('La posizione è corretta.\nL\'accertato è stato inserito come nuova dichiarazione');"
    '                    '    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - Effettuato il rientro dell'accertato")
    '                    'Else
    '                    '    str1 = str1 & "alert('La posizione è corretta.\nSi è verificato un errore nel rientro dell'accertato');"
    '                    '    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - Errore nel rientro dell'accertato")
    '                    'End If
    '                    str1 = str1 & "RiepilogoAccertato('0'," & lngNewID_PROVVEDIMENTO & ");"
    '                End If
    '                RegisterScript(str1, Me.GetType)
    '            ElseIf ImportoTotaleAvviso < soglia Then
    '                'Non emetto Avviso
    '                Dim str1 As String
    '                str1 = "parent.document.getElementById('attesaCarica').style.display='none';"
    '                If blnTIPO_OPERAZIONE_RETTIFICA = True Then
    '                    'Atto di autotutela di annullamento. Importo inferiore alla soglia
    '                    str1 = str1 & "alert('La posizione è corretta.\nE\' stato elaborato un ATTO DI AUTOTUTELA DI ANNULLAMENTO');"
    '                    str1 = str1 & "RiepilogoAccertato('1'," & lngNewID_PROVVEDIMENTO & ");"
    '                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - La posizione è corretta. Elaborato ATTO DI AUTOTUTELA DI ANNULLAMENTO")
    '                Else
    '                    'Effettuo il rientro dell'accertato
    '                    'Nessun avviso emesso. Importo inferiore alla soglia
    '                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - caso 3 - Effettuo il rientro dell'accertato ID_PROVVEDIMENTO=" & lngNewID_PROVVEDIMENTO)
    '                    '*** RIENTRO DA SISTEMARE ***
    '                    'Dim Rientro As New BE_RientroDaAccertamento.ICI
    '                    'Rientro.pathApplication = ConfigurationManager.AppSettings("PATH_LOG_IMMOBILI")
    '                    'Rientro.PARAMETROENV = ConfigurationManager.AppSettings("PARAMETROENV")
    '                    'Rientro.username = ConstSession.UserName
    '                    'Rientro.IdentificativoApplicazione = ConfigurationManager.AppSettings("OPENGOVP")
    '                    'Rientro.CODICE_ENTE = ConstSession.IdEnte
    '                    'If Rientro.Elabora(lngNewID_PROVVEDIMENTO) Then
    '                    '    str1 = str1 & "alert('Importo Avviso inferiore alla soglia.\nL\'accertato è stato inserito come nuova dichiarazione');"
    '                    '    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - Effettuato il rientro dell'accertato")
    '                    'Else
    '                    '    str1 = str1 & "alert('Importo Avviso inferiore alla soglia.\nSi è verificato un errore nel rientro dell'accertato');"
    '                    '    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - Errore nel rientro dell'accertato")
    '                    'End If
    '                    str1 = str1 & "RiepilogoAccertato('0'," & lngNewID_PROVVEDIMENTO & ");"
    '                End If
    '                RegisterScript(str1, Me.GetType)
    '            End If
    '        End If
    '        '*******************************************************************************
    '        'Aggiorno la Griglia con le sanzioni e interessi
    '        ''***** SOLO PER DEBUG *********
    '        'GrdAccertato.DataSource = objDTAccertato
    '        'GrdAccertato.DataBind()
    '        ''******************************
    '        objDSSanzioni.Dispose()
    '        objDSInteressi.Dispose()
    '        objDSSanzioni.Dispose()

    '        'If queryResult = True Then
    '        If lngNewID_PROVVEDIMENTO > 0 Then
    '            Dim str As String
    '            str = ""
    '            If blnTIPO_OPERAZIONE_RETTIFICA = True Then
    '                str = str & "FineElaborazioneAccertamento();" & vbCrLf
    '                str = str & "RiepilogoAccertato('1'," & lngNewID_PROVVEDIMENTO & ");"
    '            Else
    '                str = str & "RiepilogoAccertato('0'," & lngNewID_PROVVEDIMENTO & ");"
    '            End If
    '            RegisterScript(str, Me.GetType)
    '            'Elimino la varibile che mi dice che ho cercato in dichiarato
    '            Session.Remove("cercaDichiarato")
    '            txtRiaccerta.Text = "0"
    '        End If
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - Fine Accertamento")
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertato.btnAccertamento_Click.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' Pulsante per la creazione dell'accertamento
    ''' controllo:NON SONO VALIDI immobili dichiarati NON LEGATI a immobili accertati mentre SONO VALIDI immobili accertati NON LEGATI a immobili dichiarati 
    ''' ricalcolo la fase 2 del pre accertamento 
    ''' Session("VALORE_RITORNO_ACCERTAMENTO")=0 è stato effettuato un accertamento definitivo, accerto con fase 2
    ''' Session("VALORE_RITORNO_ACCERTAMENTO")=1 è stato effettuato un pre-accertamento definitivo, accerto senza fase 2
    ''' Session("VALORE_RITORNO_ACCERTAMENTO")=2 è stato effettuato un pre-accertamento - atto potenziale, accerto con fase 2
    ''' Session("VALORE_RITORNO_ACCERTAMENTO")=3 non ho effettuato nè ACCERTAMENTO nè PRE-ACCERTAMENTO accerto con fase 2
    ''' Session("VALORE_RITORNO_ACCERTAMENTO")=4 se per contrib e anno è stato effettuato un accertamento  con data di conferma NON presente (NON definitivo) accerto senza fase 2 e elimino i dati dell'accertamento precedente
    ''' Session("VALORE_RITORNO_ACCERTAMENTO")=5 se per contrib e anno è stato effettuato un accertamento  con data di conferma NON presente (NON definitivo)  e un preaccertamento con data di conferma NON presente  (atto potenziale) accerto con fase 2 e elimino i dati dell'accertamento precedente
    ''' Session("VALORE_RITORNO_ACCERTAMENTO")=6 sto effettuando un accertamento di autotutela
    ''' Calcolo la differenza di ICI Se la differenza è > 0 Calcolo Sanzioni e interessi
    ''' Gli immobili sono agganciati da txtLegame 
    ''' Determino il Tipo di Avviso (Accertamento o Ufficio)
    ''' Se trovo 1 immobile di accertato che non è in dichiarato ho avviso di Tipo = 4 altrimenti avviso di tipo 3; se sto effettuando una rettifica dell'avviso dovrò forzare in seguito il tipoavviso
    ''' Calcolo importi/sanzioni/interessi per singoli immobili
    ''' inserisco in riepilogo i dati relativi alla fase accertamento agli importi scaturiti dall'accertamento, devo sommare gli importi scaturiti dalla fase 2 (se effettuata)
    ''' reperisco soglia, Calcolo le spese
    ''' determino il tipo di provvedimento finale (accertamento + pre accertamento fase 2)
    ''' Inserisco i dati dell'accertamento nel database
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Analisi eventi</em>
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory><revision date="12/11/2019">il calcolo interessi 8852/TASI deve essere fatto in acconto/saldo o in unica soluzione in base alla configurazione di TP_GENERALE_ICI</revision></revisionHistory>
    Private Sub btnAccertamento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccertamento.Click
        Try
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - Inizio Accertamento ICI")
            Dim FncGest As New ClsGestioneAccertamenti
            Dim objICI() As objSituazioneFinale
            Dim ListDichiarato() As objUIICIAccert
            Dim objDTAccertato() As objUIICIAccert
            Dim objhashtableRIEPILOGO As New Hashtable
            Dim blnRetValControlloLEGAME As Boolean
            Dim TotImpICIDichiarato As Double = 0
            Dim objHashTable As New Hashtable
            Dim TotImpICIACCERTAMENTO As Double = 0
            Dim iTIPOPROVV_PREACC As Integer
            Dim oCalcoloSanzioniInteressi As ObjBaseIntSanz

            Dim TotDiffImpostaDICHIARATO As Double 'Importo Totale Differenza di imposta immobili dichiarati

            Dim TotImportoSanzioniACCERTAMENTO As Double 'Totale Sanzioni atto di accertamento
            Dim TotImportoSanzioniRidottoACCERTAMENTO As Double 'Totale Sanzioni atto di accertamento
            Dim TotImportoInteressiACCERTAMENTO As Double 'Totale Interessi atto di accertamento
            Dim TotDiffImpostaACCERTAMENTO As Double 'Importo Totale Differenza di imposta atto di accertamento

            Dim dsRiepilogoFase2 As New ObjBaseIntSanz
            Dim objDSSanzioni As New DataSet
            Dim ListInteressi() As ObjInteressiSanzioni
            Dim dsSanzioniFase2 As New DataSet
            Dim ListInteressiFase2() As ObjInteressiSanzioni
            Dim dsVersamenti As New DataSet
            Dim TipoAvviso As Integer
            Dim DescrTipoAvviso As String = ""
            Dim lngNewID_PROVVEDIMENTO As Long

            Dim soglia As Double
            Dim TotVersamenti As Double
            Dim ImportoTotaleAvviso As Double
            Dim dblImportoTotaleF2 As Double
            Dim spese As Double

            Dim DATA_RETTIFICA_ANNULLAMENTO As String
            Dim blnTIPO_OPERAZIONE_RETTIFICA As Boolean = CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("TIPO_OPERAZIONE_RETTIFICA")

            Dim dsDettaglioAnagrafica As DataSet = Nothing
            Dim oDettaglioAnagrafica As DettaglioAnagrafica
            oDettaglioAnagrafica = New DettaglioAnagrafica

            oDettaglioAnagrafica = Session("codContribuente")
            dsDettaglioAnagrafica = FncGest.addRowsObjAnagrafica(oDettaglioAnagrafica)
            If Not IsNothing(oDettaglioAnagrafica) Then
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - CodContribuente:" & oDettaglioAnagrafica.COD_CONTRIBUENTE & " - Cognome: " & oDettaglioAnagrafica.Cognome & " - Nome:" & oDettaglioAnagrafica.Nome)
            Else
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - CodContribuente: Non disponibile")
            End If

            objHashTable = CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - Anno Accertamento:" & hfAnno.Value)
            'Calcola ICI su singolo Immobile del dichiarato.
            objICI = Session("DATASET_CALCOLO_ICI_IMMOBILI_DICHIARATI")
            objDTAccertato = CType(Session("DataTableImmobiliDaAccertare"), objUIICIAccert())
            If objDTAccertato Is Nothing Then
                RegisterScript("GestAlert('a', 'danger', '', '', 'Impossibile effettuare un accertamento. Il contribuente non ha Accertato!');", Me.GetType)
                Exit Sub
            End If
            ListDichiarato = CType(Session("DataSetDichiarazioni"), objUIICIAccert())
            'verifico che i legami siano tutti progressivi
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - verifico che i legami siano tutti progressivi")
            If FncGest.CheckProgLegame(ListDichiarato) = False Then
                RegisterScript("GestAlert('a', 'warning', '', '', 'Attenzione! I legami non sono consecutivi!');", Me.GetType)
                Exit Sub
            End If
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - controllo legame doppio accertato")
            blnRetValControlloLEGAME = FncGest.ControlloLEGAMEdoppioAccertato(ListDichiarato)
            If blnRetValControlloLEGAME = True Then
                'presenti immobili accertati doppi per id legame Impossibile effettuare un accertamento
                RegisterScript("GestAlert('a', 'warning', '', '', 'Impossibile effettuare un accertamento. Il sistema ha rilevato più immobili accertati con lo stesso LEGAME!');", Me.GetType)
                Exit Sub
            End If
            '08/02/2008 commentato alep come indicato da giulia
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - controllo i legami")
            blnRetValControlloLEGAME = FncGest.ControlloLEGAME(ListDichiarato, objDTAccertato)
            If blnRetValControlloLEGAME = False And txtControlloLegame.Text = "0" Then
                'presenti immobili dichiarati NON LEGATI a immobili accertati Impossibile effettuare un accertamento
                RegisterScript("ControlloLegame();", Me.GetType)
                Exit Sub
            End If
            If Not Session("ESCLUDI_PREACCERTAMENTO") Then
                'Calcolo Fase 2 PreAccertamento
                '*** 201810 - Generazione Massiva Atti ***
                Dim ImpDichAcconto, ImpDichSaldo, ImpDichTotale As Double
                ImpDichAcconto = 0 : ImpDichSaldo = 0 : ImpDichTotale = 0
                If Not ListDichiarato Is Nothing Then

                    For Each myUI As objUIICIAccert In ListDichiarato
                        If myUI.Anno = hfAnno.Value Then
                            ImpDichAcconto += myUI.AccDovuto
                            ImpDichSaldo += myUI.SalDovuto
                            ImpDichTotale += myUI.TotDovuto
                        End If
                    Next
                End If
                If FncGest.CalcoloPreAccertamento(ConstSession.IdEnte, ConstSession.CodTributo, ConstSession.CodTributo, hfAnno.Value, oDettaglioAnagrafica.COD_CONTRIBUENTE, CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("ID_PROVVEDIMENTO_RETTIFICA"), CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("DATA_ELABORAZIONE_PER_RETTIFICA"), dsDettaglioAnagrafica, ImpDichAcconto, ImpDichSaldo, ImpDichTotale, dsRiepilogoFase2, TotVersamenti, dsSanzioniFase2, ListInteressiFase2, dsVersamenti) = False Then
                    Throw New Exception("errore in calcolopreaccertamento")
                End If
            End If
            '******************************************************************************
            'Tipo avviso
            If FncGest.GetTipoAvviso(CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("TIPO_OPERAZIONE_RETTIFICA"), True, objDTAccertato, ListDichiarato, ImportoTotaleAvviso, dblImportoTotaleF2, iTIPOPROVV_PREACC, TipoAvviso, DescrTipoAvviso) = False Then
                Throw New Exception("errore in gettipoavviso")
            End If
            Session("TipoAvviso") = DescrTipoAvviso
            'Calcolo importi/sanzioni/interessi per singoli immobili
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - Calcolo importi/sanzioni/interessi per singoli immobili")
            If FncGest.CalcoloImpSanzIntSingleUI(objHashTable, oDettaglioAnagrafica.COD_CONTRIBUENTE, hfAnno.Value, Session("DataSetSanzioni"), ListDichiarato, TipoAvviso, OggettoAtto.Fase.DichiaratoAccertato, oDettaglioAnagrafica.DataMorte _
                    , oCalcoloSanzioniInteressi, objDSSanzioni, ListInteressi, objDTAccertato _
                    , TotDiffImpostaACCERTAMENTO, TotImpICIDichiarato, TotDiffImpostaDICHIARATO, TotImpICIACCERTAMENTO, TotImportoInteressiACCERTAMENTO, TotImportoSanzioniACCERTAMENTO, TotImportoSanzioniRidottoACCERTAMENTO, TotVersamenti) = False Then
                Throw New Exception("errore in calcoloimpsanzintsingleui")
            End If
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - Terminato ciclo Calcolo importi/sanzioni/interessi per singoli immobili")
            Log.Debug("calcolo ImportoTotaleAvviso::" & (TotDiffImpostaACCERTAMENTO + TotImportoSanzioniACCERTAMENTO + TotImportoInteressiACCERTAMENTO))
            ImportoTotaleAvviso = TotDiffImpostaACCERTAMENTO + TotImportoSanzioniACCERTAMENTO + TotImportoInteressiACCERTAMENTO

            'inserisco in riepilogo i dati relativi alla fase accertamento
            If FncGest.LoadRiepilogo(TotImpICIACCERTAMENTO, TotDiffImpostaACCERTAMENTO, TotImportoSanzioniACCERTAMENTO, TotImportoInteressiACCERTAMENTO, ImportoTotaleAvviso, TotImportoSanzioniRidottoACCERTAMENTO, TotVersamenti, TotImpICIDichiarato, objhashtableRIEPILOGO) = False Then
                Throw New Exception("errore in load riepilogo")
            End If
            'agli importi scaturiti dall'accertamento, devo sommare gli importi scaturiti dalla fase 2 (se effettuata)
            If FncGest.GetImpFinali(hfAnno.Value, oDettaglioAnagrafica.COD_CONTRIBUENTE, dsRiepilogoFase2, ImportoTotaleAvviso, TotDiffImpostaACCERTAMENTO, TotImportoSanzioniACCERTAMENTO, TotImportoSanzioniRidottoACCERTAMENTO, TotImportoInteressiACCERTAMENTO, TotDiffImpostaDICHIARATO, oCalcoloSanzioniInteressi, objhashtableRIEPILOGO, iTIPOPROVV_PREACC, dblImportoTotaleF2) = False Then
                Throw New Exception("errore in getimpfinali")
            End If
            Session("HTRIEPILOGO") = objhashtableRIEPILOGO
            If FncGest.GetTipoAvviso(CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("TIPO_OPERAZIONE_RETTIFICA"), False, objDTAccertato, ListDichiarato, ImportoTotaleAvviso, dblImportoTotaleF2, iTIPOPROVV_PREACC, TipoAvviso, DescrTipoAvviso) = False Then
                Throw New Exception("errore in gettipoavviso")
            End If

            Dim objCOMUpdateDBAccertamenti As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)

            'reperisco soglia
            Dim objProvvedimentiDB As New DBPROVVEDIMENTI.ProvvedimentiDB
            soglia = 0
            soglia = objProvvedimentiDB.GetSogliaMinima(hfAnno.Value, Utility.Costanti.TRIBUTO_ICI, objHashTable("CODENTE"), TipoAvviso)
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - Soglia:" & soglia & " €")
            'Calcolo le spese
            If hfCalcolaSpese.Value = "0" Then
                spese = objProvvedimentiDB.GetSpese(hfAnno.Value, Utility.Costanti.TRIBUTO_ICI, objHashTable("CODENTE"), TipoAvviso)
            Else
                spese = 0
            End If
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - Spese:" & spese & " €")
            ' Aggiorno il DB dopo procedura di accertamento
            'determino il tipo di provvedimento finale (accertamento + pre accertamento fase 2)
            If objHashTable.Contains("TIPOPROVVEDIMENTO") = False Then
                objHashTable.Add("TIPOPROVVEDIMENTO", TipoAvviso)
            Else
                objHashTable("TIPOPROVVEDIMENTO") = TipoAvviso
            End If

            If blnTIPO_OPERAZIONE_RETTIFICA = True Then
                DATA_RETTIFICA_ANNULLAMENTO = DateTime.Now.ToString("yyyyMMdd")
                If ImportoTotaleAvviso < soglia Then
                    TipoAvviso = OggettoAtto.Provvedimento.AutotutelaAnnullamento
                    objHashTable("TIPOPROVVEDIMENTO") = TipoAvviso
                    objHashTable.Add("DATA_ANNULLAMENTO", DATA_RETTIFICA_ANNULLAMENTO)
                    objHashTable.Add("DATA_RETTIFICA", "")
                Else
                    objHashTable.Add("DATA_RETTIFICA", DATA_RETTIFICA_ANNULLAMENTO)
                    objHashTable.Add("DATA_ANNULLAMENTO", "")
                End If
            End If
            If TipoAvviso <> OggettoAtto.Provvedimento.NoAvviso Then
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - Chiamo updateDBAccertamenti")
                'Inserisco i dati dell'accertamento nel database
                objHashTable("CODTRIBUTO") = ConstSession.CodTributo
                lngNewID_PROVVEDIMENTO = objCOMUpdateDBAccertamenti.updateDBAccertamenti(ConstSession.DBType, ConstSession.StringConnection, ConstSession.IdEnte, oDettaglioAnagrafica.COD_CONTRIBUENTE, objHashTable, oCalcoloSanzioniInteressi, objDSSanzioni, ListInteressi, spese, objICI, ListDichiarato, objDTAccertato, dsSanzioniFase2, ListInteressiFase2, dsVersamenti, ConstSession.UserName)
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - Terminata updateDBAccertamenti")
                If lngNewID_PROVVEDIMENTO < 1 Then
                    Throw New Exception("Errore in inserimento avviso")
                End If
                Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Provvedimenti, "btnAccertamento", Utility.Costanti.AZIONE_NEW, ConstSession.CodTributo, ConstSession.IdEnte, lngNewID_PROVVEDIMENTO)
            Else
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - NON chiamo updateDBAccertamenti perchè NESSUN_AVVISO")
            End If

            '*******************************************************************************
            If ImportoTotaleAvviso < 0 Then
                If ImportoTotaleAvviso * (-1) < soglia Then
                    'Non emetto Avviso
                    Dim str1 As String
                    str1 = "parent.document.getElementById('attesaElabAccertamento').style.display='none';"
                    str1 = str1 & "parent.document.getElementById('attesaCarica').style.display='none';"
                    If blnTIPO_OPERAZIONE_RETTIFICA = True Then
                        'Atto di autotutela in rettifica.
                        str1 = str1 & "GestAlert('a', 'success', '', '', 'Importo Avviso inferiore alla soglia.\nE\' stato elaborato un ATTO DI AUTOTUTELA IN RETTIFICA');"
                        str1 = str1 & "RiepilogoAccertato('1'," & lngNewID_PROVVEDIMENTO & ");"
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - Importo Avviso inferiore alla soglia. Elaborato ATTO DI AUTOTUTELA IN RETTIFICA")
                    Else
                        'Effettuo il rientro dell'accertato
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - caso 1 - Effettuo il rientro dell'accertato ID_PROVVEDIMENTO=" & lngNewID_PROVVEDIMENTO)
                    End If
                    RegisterScript(str1, Me.GetType)

                End If
            Else
                If ImportoTotaleAvviso = 0 Then
                    'Non emetto Avviso
                    Dim str1 As String
                    str1 = "parent.document.getElementById('attesaElabAccertamento').style.display='none';"
                    str1 = str1 & "parent.document.getElementById('attesaCarica').style.display='none';"

                    If blnTIPO_OPERAZIONE_RETTIFICA = True Then
                        'Atto di autotutela di annullamento
                        str1 = str1 & "GestAlert('a', 'success', '', '', 'La posizione è corretta.\nE\' stato elaborato un ATTO DI AUTOTUTELA DI ANNULLAMENTO');"
                        str1 = str1 & "RiepilogoAccertato('1'," & lngNewID_PROVVEDIMENTO & ");"
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - La posizione è corretta. Elaborato ATTO DI AUTOTUTELA DI ANNULLAMENTO")
                    Else
                        'Nessun avviso emesso
                        'Effettuo il rientro dell'accertato
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - caso 2 - Effettuo il rientro dell'accertato ID_PROVVEDIMENTO=" & lngNewID_PROVVEDIMENTO)
                        str1 = str1 & "RiepilogoAccertato('0'," & lngNewID_PROVVEDIMENTO & ");"
                    End If
                    RegisterScript(str1, Me.GetType)
                ElseIf ImportoTotaleAvviso < soglia Then
                    'Non emetto Avviso
                    Dim str1 As String
                    str1 = "parent.document.getElementById('attesaCarica').style.display='none';"
                    If blnTIPO_OPERAZIONE_RETTIFICA = True Then
                        'Atto di autotutela di annullamento. Importo inferiore alla soglia
                        str1 = str1 & "GestAlert('a', 'success', '', '', 'La posizione è corretta.\nE\' stato elaborato un ATTO DI AUTOTUTELA DI ANNULLAMENTO');"
                        str1 = str1 & "RiepilogoAccertato('1'," & lngNewID_PROVVEDIMENTO & ");"
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - La posizione è corretta. Elaborato ATTO DI AUTOTUTELA DI ANNULLAMENTO")
                    Else
                        'Effettuo il rientro dell'accertato
                        'Nessun avviso emesso. Importo inferiore alla soglia
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - caso 3 - Effettuo il rientro dell'accertato ID_PROVVEDIMENTO=" & lngNewID_PROVVEDIMENTO)
                        str1 = str1 & "RiepilogoAccertato('0'," & lngNewID_PROVVEDIMENTO & ");"
                    End If
                    RegisterScript(str1, Me.GetType)
                End If
            End If
            objDSSanzioni.Dispose()
            objDSSanzioni.Dispose()

            If lngNewID_PROVVEDIMENTO > 0 Then
                Dim str As String
                str = ""
                If blnTIPO_OPERAZIONE_RETTIFICA = True Then
                    str = str & "FineElaborazioneAccertamento();" & vbCrLf
                    str = str & "RiepilogoAccertato('1'," & lngNewID_PROVVEDIMENTO & ");"
                Else
                    str = str & "RiepilogoAccertato('0'," & lngNewID_PROVVEDIMENTO & ");"
                End If
                RegisterScript(str, Me.GetType)
                'Elimino la varibile che mi dice che ho cercato in dichiarato
                Session.Remove("cercaDichiarato")
                txtRiaccerta.Text = "0"
            End If
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - Fine Accertamento")
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertato.btnAccertamento_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub btnAccertamento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccertamento.Click
    '    Try
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - Inizio Accertamento ICI")
    '        Dim FncGest As New ClsGestioneAccertamenti
    '        Dim objICI() As objSituazioneFinale
    '        Dim ListDichiarato() As objUIICIAccert
    '        Dim objDTAccertato() As objUIICIAccert
    '        Dim objhashtableRIEPILOGO As New Hashtable
    '        Dim blnRetValControlloLEGAME As Boolean
    '        Dim TotImpICIDichiarato As Double = 0
    '        Dim objHashTable As New Hashtable
    '        Dim TotImpICIACCERTAMENTO As Double = 0
    '        Dim iTIPOPROVV_PREACC As Integer
    '        Dim objDSCalcoloSanzioniInteressi As DataSet

    '        Dim TotDiffImpostaDICHIARATO As Double 'Importo Totale Differenza di imposta immobili dichiarati

    '        Dim TotImportoSanzioniACCERTAMENTO As Double 'Totale Sanzioni atto di accertamento
    '        Dim TotImportoSanzioniRidottoACCERTAMENTO As Double 'Totale Sanzioni atto di accertamento
    '        Dim TotImportoInteressiACCERTAMENTO As Double 'Totale Interessi atto di accertamento
    '        Dim TotDiffImpostaACCERTAMENTO As Double 'Importo Totale Differenza di imposta atto di accertamento

    '        Dim dsRiepilogoFase2 As New DataSet
    '        Dim objDSSanzioni As New DataSet
    '        Dim ListInteressi() As ObjInteressiSanzioni
    '        Dim dsSanzioniFase2 As New DataSet
    '        Dim ListInteressiFase2() As ObjInteressiSanzioni
    '        Dim dsVersamenti As New DataSet
    '        Dim TipoAvviso As Integer
    '        Dim DescrTipoAvviso As String = ""
    '        Dim lngNewID_PROVVEDIMENTO As Long

    '        Dim soglia As Double
    '        Dim TotVersamenti As Double
    '        Dim ImportoTotaleAvviso As Double
    '        Dim dblImportoTotaleF2 As Double
    '        Dim spese As Double

    '        Dim DATA_RETTIFICA_ANNULLAMENTO As String
    '        Dim blnTIPO_OPERAZIONE_RETTIFICA As Boolean = CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("TIPO_OPERAZIONE_RETTIFICA")

    '        Dim dsDettaglioAnagrafica As DataSet = Nothing
    '        Dim oDettaglioAnagrafica As DettaglioAnagrafica
    '        oDettaglioAnagrafica = New DettaglioAnagrafica

    '        oDettaglioAnagrafica = Session("codContribuente")
    '        dsDettaglioAnagrafica = FncGest.addRowsObjAnagrafica(oDettaglioAnagrafica)
    '        If Not IsNothing(oDettaglioAnagrafica) Then
    '            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - CodContribuente:" & oDettaglioAnagrafica.COD_CONTRIBUENTE & " - Cognome: " & oDettaglioAnagrafica.Cognome & " - Nome:" & oDettaglioAnagrafica.Nome)
    '        Else
    '            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - CodContribuente: Non disponibile")
    '        End If

    '        objHashTable = CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)

    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - Anno Accertamento:" & CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("ANNOACCERTAMENTO"))
    '        'Calcola ICI su singolo Immobile del dichiarato.
    '        objICI = Session("DATASET_CALCOLO_ICI_IMMOBILI_DICHIARATI")
    '        objDTAccertato = CType(Session("DataTableImmobiliDaAccertare"), objUIICIAccert())
    '        If objDTAccertato Is Nothing Then
    '            RegisterScript("GestAlert('a', 'danger', '', '', 'Impossibile effettuare un accertamento. Il contribuente non ha Accertato!');", Me.GetType)
    '            Exit Sub
    '        End If
    '        ListDichiarato = CType(Session("DataSetDichiarazioni"), objUIICIAccert())
    '        'verifico che i legami siano tutti progressivi
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - verifico che i legami siano tutti progressivi")
    '        If FncGest.CheckProgLegame(ListDichiarato) = False Then
    '            RegisterScript("GestAlert('a', 'warning', '', '', 'Attenzione! I legami non sono consecutivi!');", Me.GetType)
    '            Exit Sub
    '        End If
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - controllo legame doppio accertato")
    '        blnRetValControlloLEGAME = FncGest.ControlloLEGAMEdoppioAccertato(ListDichiarato)
    '        If blnRetValControlloLEGAME = True Then
    '            'presenti immobili accertati doppi per id legame Impossibile effettuare un accertamento
    '            RegisterScript("GestAlert('a', 'warning', '', '', 'Impossibile effettuare un accertamento. Il sistema ha rilevato più immobili accertati con lo stesso LEGAME!');", Me.GetType)
    '            Exit Sub
    '        End If
    '        '08/02/2008 commentato alep come indicato da giulia
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - controllo i legami")
    '        blnRetValControlloLEGAME = FncGest.ControlloLEGAME(ListDichiarato, objDTAccertato)
    '        If blnRetValControlloLEGAME = False And txtControlloLegame.Text = "0" Then
    '            'presenti immobili dichiarati NON LEGATI a immobili accertati Impossibile effettuare un accertamento
    '            RegisterScript("ControlloLegame();", Me.GetType)
    '            Exit Sub
    '        End If
    '        If Not Session("ESCLUDI_PREACCERTAMENTO") Then
    '            'Calcolo Fase 2 PreAccertamento
    '            '*** 201810 - Generazione Massiva Atti ***
    '            Dim ImpDichAcconto, ImpDichSaldo, ImpDichTotale As Double
    '            ImpDichAcconto = 0 : ImpDichSaldo = 0 : ImpDichTotale = 0
    '            If Not ListDichiarato Is Nothing Then

    '                For Each myUI As objUIICIAccert In ListDichiarato
    '                    If myUI.Anno = CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("ANNOACCERTAMENTO") Then
    '                        ImpDichAcconto += myUI.AccDovuto
    '                        ImpDichSaldo += myUI.SalDovuto
    '                        ImpDichTotale += myUI.TotDovuto
    '                    End If
    '                Next
    '            End If
    '            If FncGest.CalcoloPreAccertamento(ConstSession.IdEnte, ConstSession.CodTributo, ConstSession.CodTributo, CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("ANNOACCERTAMENTO"), CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("CODCONTRIBUENTE"), CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("ID_PROVVEDIMENTO_RETTIFICA"), CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("DATA_ELABORAZIONE_PER_RETTIFICA"), dsDettaglioAnagrafica, ImpDichAcconto, ImpDichSaldo, ImpDichTotale, dsRiepilogoFase2, TotVersamenti, dsSanzioniFase2, ListInteressiFase2, dsVersamenti) = False Then
    '                Throw New Exception("errore in calcolopreaccertamento")
    '            End If
    '        End If
    '        '******************************************************************************
    '        'Tipo avviso
    '        If FncGest.GetTipoAvviso(CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("TIPO_OPERAZIONE_RETTIFICA"), True, objDTAccertato, ListDichiarato, ImportoTotaleAvviso, dblImportoTotaleF2, iTIPOPROVV_PREACC, TipoAvviso, DescrTipoAvviso) = False Then
    '            Throw New Exception("errore in gettipoavviso")
    '        End If
    '        Session("TipoAvviso") = DescrTipoAvviso
    '        'Calcolo importi/sanzioni/interessi per singoli immobili
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - Calcolo importi/sanzioni/interessi per singoli immobili")
    '        If FncGest.CalcoloImpSanzIntSingleUI(objHashTable, CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("ANNOACCERTAMENTO"), Session("DataSetSanzioni"), ListDichiarato, TipoAvviso, OggettoAtto.Fase.DichiaratoAccertato, oDettaglioAnagrafica.DataMorte _
    '                , objDSCalcoloSanzioniInteressi, objDSSanzioni, ListInteressi, objDTAccertato _
    '                , TotDiffImpostaACCERTAMENTO, TotImpICIDichiarato, TotDiffImpostaDICHIARATO, TotImpICIACCERTAMENTO, TotImportoInteressiACCERTAMENTO, TotImportoSanzioniACCERTAMENTO, TotImportoSanzioniRidottoACCERTAMENTO, TotVersamenti) = False Then
    '            Throw New Exception("errore in calcoloimpsanzintsingleui")
    '        End If
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - Terminato ciclo Calcolo importi/sanzioni/interessi per singoli immobili")
    '        Log.Debug("calcolo ImportoTotaleAvviso::" & (TotDiffImpostaACCERTAMENTO + TotImportoSanzioniACCERTAMENTO + TotImportoInteressiACCERTAMENTO))
    '        ImportoTotaleAvviso = TotDiffImpostaACCERTAMENTO + TotImportoSanzioniACCERTAMENTO + TotImportoInteressiACCERTAMENTO

    '        'inserisco in riepilogo i dati relativi alla fase accertamento
    '        If FncGest.LoadRiepilogo(TotImpICIACCERTAMENTO, TotDiffImpostaACCERTAMENTO, TotImportoSanzioniACCERTAMENTO, TotImportoInteressiACCERTAMENTO, ImportoTotaleAvviso, TotImportoSanzioniRidottoACCERTAMENTO, TotVersamenti, TotImpICIDichiarato, objhashtableRIEPILOGO) = False Then
    '            Throw New Exception("errore in load riepilogo")
    '        End If
    '        'agli importi scaturiti dall'accertamento, devo sommare gli importi scaturiti dalla fase 2 (se effettuata)
    '        If FncGest.GetImpFinali(objHashTable("ANNOACCERTAMENTO"), objHashTable("CODCONTRIBUENTE"), dsRiepilogoFase2, ImportoTotaleAvviso, TotDiffImpostaACCERTAMENTO, TotImportoSanzioniACCERTAMENTO, TotImportoSanzioniRidottoACCERTAMENTO, TotImportoInteressiACCERTAMENTO, TotDiffImpostaDICHIARATO, objDSCalcoloSanzioniInteressi, objhashtableRIEPILOGO, iTIPOPROVV_PREACC, dblImportoTotaleF2) = False Then
    '            Throw New Exception("errore in getimpfinali")
    '        End If
    '        Session("HTRIEPILOGO") = objhashtableRIEPILOGO
    '        If FncGest.GetTipoAvviso(CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("TIPO_OPERAZIONE_RETTIFICA"), False, objDTAccertato, ListDichiarato, ImportoTotaleAvviso, dblImportoTotaleF2, iTIPOPROVV_PREACC, TipoAvviso, DescrTipoAvviso) = False Then
    '            Throw New Exception("errore in gettipoavviso")
    '        End If

    '        Dim objCOMUpdateDBAccertamenti As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)

    '        'reperisco soglia
    '        Dim objProvvedimentiDB As New DBPROVVEDIMENTI.ProvvedimentiDB
    '        soglia = 0
    '        soglia = objProvvedimentiDB.GetSogliaMinima(objHashTable("ANNOACCERTAMENTO"), Utility.Costanti.TRIBUTO_ICI, objHashTable("CODENTE"), TipoAvviso)
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - Soglia:" & soglia & " €")
    '        'Calcolo le spese
    '        If hfCalcolaSpese.Value = "0" Then
    '            spese = objProvvedimentiDB.GetSpese(objHashTable("ANNOACCERTAMENTO"), Utility.Costanti.TRIBUTO_ICI, objHashTable("CODENTE"), TipoAvviso)
    '        Else
    '            spese = 0
    '        End If
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - Spese:" & spese & " €")
    '        ' Aggiorno il DB dopo procedura di accertamento
    '        'determino il tipo di provvedimento finale (accertamento + pre accertamento fase 2)
    '        If objHashTable.Contains("TIPOPROVVEDIMENTO") = False Then
    '            objHashTable.Add("TIPOPROVVEDIMENTO", TipoAvviso)
    '        Else
    '            objHashTable("TIPOPROVVEDIMENTO") = TipoAvviso
    '        End If

    '        If blnTIPO_OPERAZIONE_RETTIFICA = True Then
    '            DATA_RETTIFICA_ANNULLAMENTO = DateTime.Now.ToString("yyyyMMdd")
    '            If ImportoTotaleAvviso < soglia Then
    '                TipoAvviso = OggettoAtto.Provvedimento.AutotutelaAnnullamento
    '                objHashTable("TIPOPROVVEDIMENTO") = TipoAvviso
    '                objHashTable.Add("DATA_ANNULLAMENTO", DATA_RETTIFICA_ANNULLAMENTO)
    '                objHashTable.Add("DATA_RETTIFICA", "")
    '            Else
    '                objHashTable.Add("DATA_RETTIFICA", DATA_RETTIFICA_ANNULLAMENTO)
    '                objHashTable.Add("DATA_ANNULLAMENTO", "")
    '            End If
    '        End If
    '        If TipoAvviso <> OggettoAtto.Provvedimento.NoAvviso Then
    '            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - Chiamo updateDBAccertamenti")
    '            'Inserisco i dati dell'accertamento nel database
    '            objHashTable("CODTRIBUTO") = ConstSession.CodTributo
    '            lngNewID_PROVVEDIMENTO = objCOMUpdateDBAccertamenti.updateDBAccertamenti(ConstSession.DBType, objHashTable, objDSCalcoloSanzioniInteressi, objDSSanzioni, ListInteressi, spese, objICI, ListDichiarato, objDTAccertato, dsSanzioniFase2, ListInteressiFase2, dsVersamenti, ConstSession.UserName)
    '            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - Terminata updateDBAccertamenti")
    '            If lngNewID_PROVVEDIMENTO < 1 Then
    '                Throw New Exception("Errore in inserimento avviso")
    '            End If
    '            Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
    '            fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Provvedimenti, "btnAccertamento", Utility.Costanti.AZIONE_NEW, ConstSession.CodTributo, ConstSession.IdEnte, lngNewID_PROVVEDIMENTO)
    '        Else
    '            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - NON chiamo updateDBAccertamenti perchè NESSUN_AVVISO")
    '        End If

    '        '*******************************************************************************
    '        If ImportoTotaleAvviso < 0 Then
    '            If ImportoTotaleAvviso * (-1) < soglia Then
    '                'Non emetto Avviso
    '                Dim str1 As String
    '                str1 = "parent.document.getElementById('attesaElabAccertamento').style.display='none';"
    '                str1 = str1 & "parent.document.getElementById('attesaCarica').style.display='none';"
    '                If blnTIPO_OPERAZIONE_RETTIFICA = True Then
    '                    'Atto di autotutela in rettifica.
    '                    str1 = str1 & "GestAlert('a', 'success', '', '', 'Importo Avviso inferiore alla soglia.\nE\' stato elaborato un ATTO DI AUTOTUTELA IN RETTIFICA');"
    '                    str1 = str1 & "RiepilogoAccertato('1'," & lngNewID_PROVVEDIMENTO & ");"
    '                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - Importo Avviso inferiore alla soglia. Elaborato ATTO DI AUTOTUTELA IN RETTIFICA")
    '                Else
    '                    'Effettuo il rientro dell'accertato
    '                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - caso 1 - Effettuo il rientro dell'accertato ID_PROVVEDIMENTO=" & lngNewID_PROVVEDIMENTO)
    '                End If
    '                RegisterScript(str1, Me.GetType)

    '            End If
    '        Else
    '            If ImportoTotaleAvviso = 0 Then
    '                'Non emetto Avviso
    '                Dim str1 As String
    '                str1 = "parent.document.getElementById('attesaElabAccertamento').style.display='none';"
    '                str1 = str1 & "parent.document.getElementById('attesaCarica').style.display='none';"

    '                If blnTIPO_OPERAZIONE_RETTIFICA = True Then
    '                    'Atto di autotutela di annullamento
    '                    str1 = str1 & "GestAlert('a', 'success', '', '', 'La posizione è corretta.\nE\' stato elaborato un ATTO DI AUTOTUTELA DI ANNULLAMENTO');"
    '                    str1 = str1 & "RiepilogoAccertato('1'," & lngNewID_PROVVEDIMENTO & ");"
    '                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - La posizione è corretta. Elaborato ATTO DI AUTOTUTELA DI ANNULLAMENTO")
    '                Else
    '                    'Nessun avviso emesso
    '                    'Effettuo il rientro dell'accertato
    '                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - caso 2 - Effettuo il rientro dell'accertato ID_PROVVEDIMENTO=" & lngNewID_PROVVEDIMENTO)
    '                    str1 = str1 & "RiepilogoAccertato('0'," & lngNewID_PROVVEDIMENTO & ");"
    '                End If
    '                RegisterScript(str1, Me.GetType)
    '            ElseIf ImportoTotaleAvviso < soglia Then
    '                'Non emetto Avviso
    '                Dim str1 As String
    '                str1 = "parent.document.getElementById('attesaCarica').style.display='none';"
    '                If blnTIPO_OPERAZIONE_RETTIFICA = True Then
    '                    'Atto di autotutela di annullamento. Importo inferiore alla soglia
    '                    str1 = str1 & "GestAlert('a', 'success', '', '', 'La posizione è corretta.\nE\' stato elaborato un ATTO DI AUTOTUTELA DI ANNULLAMENTO');"
    '                    str1 = str1 & "RiepilogoAccertato('1'," & lngNewID_PROVVEDIMENTO & ");"
    '                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - La posizione è corretta. Elaborato ATTO DI AUTOTUTELA DI ANNULLAMENTO")
    '                Else
    '                    'Effettuo il rientro dell'accertato
    '                    'Nessun avviso emesso. Importo inferiore alla soglia
    '                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - caso 3 - Effettuo il rientro dell'accertato ID_PROVVEDIMENTO=" & lngNewID_PROVVEDIMENTO)
    '                    str1 = str1 & "RiepilogoAccertato('0'," & lngNewID_PROVVEDIMENTO & ");"
    '                End If
    '                RegisterScript(str1, Me.GetType)
    '            End If
    '        End If
    '        objDSSanzioni.Dispose()
    '        objDSSanzioni.Dispose()

    '        If lngNewID_PROVVEDIMENTO > 0 Then
    '            Dim str As String
    '            str = ""
    '            If blnTIPO_OPERAZIONE_RETTIFICA = True Then
    '                str = str & "FineElaborazioneAccertamento();" & vbCrLf
    '                str = str & "RiepilogoAccertato('1'," & lngNewID_PROVVEDIMENTO & ");"
    '            Else
    '                str = str & "RiepilogoAccertato('0'," & lngNewID_PROVVEDIMENTO & ");"
    '            End If
    '            RegisterScript(str, Me.GetType)
    '            'Elimino la varibile che mi dice che ho cercato in dichiarato
    '            Session.Remove("cercaDichiarato")
    '            txtRiaccerta.Text = "0"
    '        End If
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - Fine Accertamento")
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertato.btnAccertamento_Click.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnRiaccerta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRiaccerta.Click
        txtRiaccerta.Text = "1"

        'Cancello il vecchio accertamento
        Dim objDeleteOldAccertamento As New DBPROVVEDIMENTI.ProvvedimentiDB
        Dim objHashTable1 As Hashtable

        objHashTable1 = Session("HashTableDichiarazioniAccertamenti")
        objDeleteOldAccertamento.deleteOldAccertamento(objHashTable1("CODCONTRIBUENTE"), objHashTable1("ANNOACCERTAMENTO"), ConstSession.IdEnte, Utility.Costanti.TRIBUTO_ICI)

        btnAccertamento_Click(Nothing, Nothing)


    End Sub
#Region "Griglie"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdRowDataBound(sender As Object, e As GridViewRowEventArgs)
        Dim nLegame As Integer = 1
        Try
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                Log.Debug("ICI.grdAccertato.GrdRowDataBound")
                Dim chkSanzioni As CheckBox
                Dim idSanzioni, idInteressi As String
                chkSanzioni = e.Row.FindControl("chkSanzioni")

                idSanzioni = CType(e.Row.FindControl("hfIDSANZIONI"), HiddenField).Value
                idInteressi = CType(e.Row.FindControl("hfInteressi"), HiddenField).Value
                If idSanzioni <> "" Then
                    chkSanzioni.Checked = True
                End If
                Dim a As ImageButton
                a = CType(e.Row.FindControl("imgInteressi"), ImageButton)
                If idInteressi.ToUpper = "TRUE" Then
                    a.ImageUrl = "..\..\images\Bottoni\visto.gif"
                Else
                    a.ImageUrl = "..\..\images\Bottoni\trasparente.png"
                End If

                e.Row.Cells(0).BackColor = Color.PaleGoldenrod
                e.Row.Cells(0).Font.Bold = True

                If Not IsNothing(e.Row.FindControl("lblLegame")) Then
                    nLegame = CType(e.Row.FindControl("lblLegame"), Label).Text
                ElseIf Not IsNothing(e.Row.FindControl("txtLegame")) Then
                    Integer.TryParse(CType(e.Row.FindControl("txtLegame"), TextBox).Text, nLegame)
                End If

                CType(e.Row.FindControl("chkSanzioni"), CheckBox).Attributes.Add("onClick", "return ApriDettaglioSanzioni('" & nlegame & "','" & chkSanzioni.ClientID.ToString & "','" & idSanzioni & "')")
                e.Row.Cells(19).ToolTip = "Premere questo pulsante per associare le sanzioni all'immobile, gestire le motivazioni e configurare la possibilità di calcolare gli interessi"
                e.Row.Cells(22).ToolTip = "Premere questo pulsante per modificare il legame dell'immobile"
                e.Row.Cells(23).ToolTip = "Premere questo pulsante per eliminare l'immobile"
                e.Row.Cells(24).ToolTip = "Premere questo pulsante per entrare in modifica sull'immobile"
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.grdAccertato.GrdRowDataBound.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim IDRow As String = e.CommandArgument.ToString()
            Dim annoAccertamento As String = Session("HashTableDichiarazioniAccertamenti")("ANNOACCERTAMENTO")
            Dim CodContribuente As String = Session("HashTableDichiarazioniAccertamenti")("CODCONTRIBUENTE")
            Dim CodTributo As String = ConstSession.CodTributo
            Select Case e.CommandName
                Case "RowOpen"
                    RegisterScript("apriModificaImmobileAnater('" & IDRow & "','" & annoAccertamento & "','" & CodContribuente & "','" & CodTributo & "')", Me.GetType)
                Case "RowInteressi"
                    Dim workTable() As objUIICIAccert = Nothing
                    For Each myRow As GridViewRow In GrdAccertato.Rows
                        If IDRow = myRow.Cells(0).Text Then
                            'eseguo le stesse operazioni che faceva prima della modifica
                            If Not Session("DataTableImmobiliDaAccertare") Is Nothing Then
                                workTable = CType(Session("DataTableImmobiliDaAccertare"), objUIICIAccert())
                                For Each myItem As objUIICIAccert In workTable
                                    If myItem.IdLegame = IDRow Then
                                        If myItem.CalcolaInteressi = True Then
                                            myItem.CalcolaInteressi = False
                                        Else
                                            myItem.CalcolaInteressi = True
                                        End If
                                    End If
                                Next
                                Session("DataTableImmobiliDaAccertare") = workTable
                            End If
                        End If
                    Next
                    GrdAccertato.DataSource = workTable
                    GrdAccertato.DataBind()
                Case "RowDelete"
                    Dim dt() As objUIICIAccert
                    dt = Session("DataTableImmobiliDaAccertare")
                    Dim myArray As New ArrayList()
                    For Each myItem As objUIICIAccert In dt
                        If myItem.Progressivo <> IDRow Then
                            myArray.Add(myItem)
                        End If
                    Next
                    dt = CType(myArray.ToArray(GetType(objUIICIAccert)), objUIICIAccert())
                    Session("DataTableImmobiliDaAccertare") = dt
                    If dt.GetLength(0) <= 0 Then
                        GrdAccertato.Visible = False
                    Else
                        GrdAccertato.Visible = True
                        GrdAccertato.DataSource = dt
                        GrdAccertato.DataBind()
                    End If
                Case "RowEdit"
                    For Each myRow As GridViewRow In GrdAccertato.Rows
                        If IDRow = myRow.Cells(0).Text Then
                            GrdAccertato.EditIndex = myRow.RowIndex
                            GrdAccertato.DataSource = Session("DataTableImmobiliDaAccertare")
                            GrdAccertato.DataBind()
                        End If
                    Next
                Case "RowUpdate"
                    For Each myRow As GridViewRow In GrdAccertato.Rows
                        If IDRow = myRow.Cells(0).Text Then
                            'Prendo l'idLegame
                            Dim idLegame As TextBox = myRow.Cells(25).FindControl("txtLegame")

                            If idLegame.Text = "" Then
                                Dim sScript As String
                                sScript = "msgLegameVuoto();" & vbCrLf
                                RegisterScript(sScript, Me.GetType)
                            Else
                                Dim Progressivo As String
                                Dim dt() As objUIICIAccert
                                dt = Session("DataTableImmobiliDaAccertare")
                                Progressivo = myRow.Cells(0).Text
                                For Each myItem As objUIICIAccert In dt
                                    If myItem.Progressivo = Progressivo Then
                                        myItem.IdLegame = idLegame.Text
                                    End If
                                Next
                                GrdAccertato.EditIndex = -1
                                Array.Sort(dt, New Utility.Comparatore(New String() {"IdLegame"}, New Boolean() {Utility.TipoOrdinamento.Crescente}))
                                GrdAccertato.DataSource = dt
                                GrdAccertato.DataBind()
                                Session("DataTableImmobiliDaAccertare") = dt
                            End If
                        End If
                    Next
                Case "RowCancel"
                    For Each myRow As GridViewRow In GrdAccertato.Rows
                        If IDRow = myRow.Cells(0).Text Then
                            GrdAccertato.EditIndex = -1
                            GrdAccertato.DataSource = Session("DataTableImmobiliDaAccertare")
                            GrdAccertato.DataBind()
                        End If
                    Next
            End Select
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.grdAccertato.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Gestione del cambio pagina della griglia.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdPageIndexChanging(ByVal sender As Object, e As GridViewPageEventArgs)
        LoadSearch(e.NewPageIndex)
    End Sub
    'Protected Sub GrdAccertato_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAccertato.ItemDataBound
    '    Try
    '        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '            Dim lblLegame As Label
    '            Dim chkSanzioni, chkInteressi As CheckBox
    '            Dim idSanzioni, txtSanzioni, idInteressi As String
    '            Dim check As CheckBox
    '            lblLegame = e.Item.Cells(idCelle.grdSanzioniSanzioni.cellLegame).FindControl("lblLegame")
    '            lblLegame.Text = e.Item.Cells(idCelle.grdSanzioniSanzioni.cellIDLegame).Text
    '            chkSanzioni = e.Item.Cells(idCelle.grdSanzioniSanzioni.cellCheckSanzioni).FindControl("chkSanzioni")
    '            chkInteressi = e.Item.Cells(idCelle.grdSanzioniSanzioni.cellCalcolaInteressi).FindControl("chkInteressi")

    '            idSanzioni = e.Item.Cells(idCelle.grdSanzioniSanzioni.cellIDSanzioni).Text
    '            idInteressi = e.Item.Cells(17).Text
    '            If idSanzioni <> "-1" Then
    '                chkSanzioni.Checked = True
    '            End If

    '            If idInteressi > 0 Then
    '                chkInteressi.Checked = True
    '            End If

    '            e.Item.Cells(0).BackColor = Color.PaleGoldenrod
    '            e.Item.Cells(0).Font.Bold = True

    '            CType(e.Item.Cells(idCelle.grdSanzioniSanzioni.cellCheckSanzioni).FindControl("chkSanzioni"), CheckBox).Attributes.Add("onClick", "return ApriDettaglioSanzioni('" & e.Item.Cells(idCelle.grdSanzioniSanzioni.cellIDLegame).Text & "','" & chkSanzioni.ClientID.ToString & "','" & idSanzioni & "')")
    '            CType(e.Item.Cells(idCelle.grdSanzioniSanzioni.cellCheckSanzioni).FindControl("chkSanzioni"), CheckBox).ToolTip = "Premere questo pulsante per associare le sanzioni all'immobile, gestire le motivazioni e configurare la possibilità di calcolare gli interessi"
    '            CType(e.Item.Cells(idCelle.grdSanzioniSanzioni.cellBottoneCompleta).FindControl("ImageCompleta"), ImageButton).Attributes.Add("onClick", "return apriModificaImmobileAnater('" & e.Item.Cells(0).Text & "')")

    '            e.Item.Cells(idCelle.grdSanzioniSanzioni.cellBottoneModifica).ToolTip = "Premere questo pulsante per modificare il legame dell'immobile"
    '            e.Item.Cells(idCelle.grdSanzioniSanzioni.cellBottoneDelete).ToolTip = "Premere questo pulsante per eliminare l'immobile"
    '            e.Item.Cells(idCelle.grdSanzioniSanzioni.cellBottoneCompleta).ToolTip = "Premere questo pulsante per entrare in modifica sull'immobile"
    '        End If
    '    Catch ex As Exception
    '         Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.grdAccertato.GrdAccertato_ItemDataBound.errore: ", ex)
    '       Response.Redirect("../../PaginaErrore.aspx")
    '        Response.Write(GestErrore.GetHTMLError(ex, "../../../Styles.css"))
    '        Response.End()
    '    End Try
    'End Sub

    'Sub GrdAccertato_DeleteCommand(ByVal s As Object, ByVal e As DataGridCommandEventArgs) Handles GrdAccertato.DeleteCommand
    '    Dim dt As DataTable
    '    Dim row() As DataRow

    '    dt = Session("DataTableImmobili")
    '    row = dt.Select("PROGRESSIVO='" & e.Item.Cells(0).Text & "'")

    '    dt.Rows.Remove(row(0))
    '    dt.AcceptChanges()

    '    Session("DataTableImmobili") = dt
    'Try
    '    If dt.Rows.Count <= 0 Then
    '        GrdAccertato.Visible = False
    '    Else
    '        GrdAccertato.Visible = True
    '        GrdAccertato.DataSource = dt
    '        GrdAccertato.DataBind()
    '    End If
    '    Catch ex As Exception
    '         Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.grdAccertato.GrdAccertato_DeleteCommand.errore: ", ex)
    '       Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Public Sub GrdAccertato_Edit(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GrdAccertato.EditCommand
    '    GrdAccertato.EditItemIndex = e.Item.ItemIndex
    '    GrdAccertato.DataSource = Session("DataTableImmobili")
    '    GrdAccertato.DataBind()
    'End Sub

    'Protected Sub GrdAccertato_Update(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GrdAccertato.UpdateCommand
    '    Dim dw As DataView

    '    'Prendo l'idLegame
    '    Dim idLegame As TextBox = e.Item.Cells(idCelle.grdSanzioniSanzioni.cellLegame).FindControl("txtLegame")
    'Try
    '    If idLegame.Text = "" Then
    '        dim sScript as string=""
    '        sscript+= "msgLegameVuoto();" & vbCrLf
    '        RegisterScript(sScript , Me.GetType())

    '    Else

    '        Dim Progressivo As String
    '        Dim dt As DataTable
    '        dt = Session("DataTableImmobili")
    '        Progressivo = e.Item.Cells(0).Text

    '        dt.DefaultView.RowFilter = "PROGRESSIVO='" & Progressivo & "'"
    '        If dt.DefaultView.Count > 0 Then
    '            dt.DefaultView.Item(0).Item("IDLegame") = idLegame.Text
    '            dt.AcceptChanges()
    '        End If
    '        dt.DefaultView.RowFilter = ""

    '        GrdAccertato.EditItemIndex = -1
    '        dw = dt.DefaultView
    '        dw.Sort = "IDLEGAME ASC, FOGLIO, NUMERO, SUBALTERNO"
    '        GrdAccertato.DataSource = dw
    '        dt.DefaultView.Sort = "IDLEGAME ASC, FOGLIO, NUMERO, SUBALTERNO"
    '        dt.AcceptChanges()
    '        GrdAccertato.DataSource = dt
    '        GrdAccertato.DataBind()
    '        Session("DataTableImmobili") = dt
    '    End If
    '    Catch ex As Exception
    '         Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.grdAccertato.GrdAccertato_Update.errore: ", ex)
    '       Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Protected Sub GrdAccertato_Cancel(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GrdAccertato.CancelCommand
    '    GrdAccertato.EditItemIndex = -1
    '    Dim dt As DataTable
    '    dt = Session("DataTableImmobili")
    '    GrdAccertato.DataSource = dt
    '    GrdAccertato.DataBind()
    'End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdAccertato.DataSource = CType(Session("DataTableImmobiliDaAccertare"), objUIICIAccert())
            If page.HasValue Then
                GrdAccertato.PageIndex = page.Value
            End If
            GrdAccertato.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.grdAccertato.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub

    'Private Function ControlloLEGAMEdoppioAccertato() As Boolean
    '    Dim objDTAccertato As DataTable = CType(Session("DataTableImmobiliDaAccertare"), DataTable)
    '    Dim Trovato As Boolean = False
    '    Dim ID_IMMOBILE_ORIGINALE_ACCERTATO As String
    '    Dim i As Integer = 0

    '    'Cerco l'immobile dichiarato in tutti gli immobili in accertato
    '    'Se lo trovo esco dal ciclo e proseguo a cercare con immobili
    '    'successivo di accertamento
    '    Try
    '        For i = 0 To objDTAccertato.DefaultView.Count - 1
    '            ID_IMMOBILE_ORIGINALE_ACCERTATO = objDTAccertato.Rows(i).Item("idLegame")
    '            If objDTAccertato.Select("idLegame='" & ID_IMMOBILE_ORIGINALE_ACCERTATO & "'").Length > 1 Then
    '                Trovato = True
    '                Exit For
    '            End If
    '        Next
    '        'Se trovato = TRUE vuol dire che ho trovato l'immobile doppio, quindi blocco l'accertamento

    '        Return Trovato
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.grdAccertato.ControlloLEGAMEdoppioAccertato.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Function

    'Private Function VerificaValore(ByVal AnnoAccertamento As String)
    '    Dim objDTAccertato As DataTable = CType(Session("DataTableImmobiliDaAccertare"), DataTable)
    '    Dim y As Integer
    '    Dim Rendita_Valore As Double

    '    Dim dblRendita As Double
    '    Dim strTipoImm, strCateg As String
    '    Dim Dal As String
    '    Dim iAnnoImmobile As Integer

    '    Dim objDBProvv As New DBPROVVEDIMENTI.ProvvedimentiDB

    '    Try
    '        For y = 0 To objDTAccertato.Rows.Count - 1

    '            iAnnoImmobile = CType(objDTAccertato.Rows(y).Item("Dal"), Date).Year
    '            If iAnnoImmobile <= 1997 Then
    '                dblRendita = objDTAccertato.Rows(y).Item("Rendita_Valore")
    '                strCateg = objDTAccertato.Rows(y).Item("Categoria")
    '                strTipoImm = objDTAccertato.Rows(y).Item(8)
    '                If strTipoImm <> "AF" Then
    '                    Rendita_Valore = dblRendita * 1.05
    '                    'Rendita_Valore = objDBProvv.CalcoloValoredaRendita(dblRendita, strTipoImm, strCateg, AnnoAccertamento)
    '                    objDTAccertato.Rows(y).Item("Rendita_Valore") = Rendita_Valore
    '                End If

    '            End If


    '        Next
    '        objDTAccertato.AcceptChanges()
    '        Session("DataTableImmobiliDaAccertare") = objDTAccertato
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.grdAccertato.VerificaValore.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Function

    'Private Function CheckProgLegame() As Boolean
    '    Dim x, nProg As Integer
    '    Dim dtAccertato As DataTable = CType(Session("DataTableImmobiliDaAccertare"), DataTable)
    '    Dim dvAccertato As New DataView

    '    Try
    '        nProg = 0
    '        dvAccertato = dtAccertato.DefaultView
    '        dvAccertato.Sort = "IDLEGAME"
    '        For x = 0 To dvAccertato.Count - 1
    '            nProg += 1
    '            If dvAccertato(x)("idlegame") <> nProg Then
    '                Return False
    '            End If
    '        Next
    '        Return True
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.grdAccertato.CheckProgLegame.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '        Return False
    '    End Try
    'End Function

    'Private Function ControlloLEGAME() As Boolean

    '    Dim y As Integer

    '    Dim objDSDichiarazioni As DataSet = CType(Session("DataSetDichiarazioni"), DataSet)
    '    Dim objDTAccertato As DataTable = CType(Session("DataTableImmobiliDaAccertare"), DataTable)

    '    Dim Trovato As Boolean = True
    '    Dim iTrovato As Integer = 0

    '    Dim ID_IMMOBILE_ORIGINALE_DICHIARATO As String
    '    Dim IDLegameDich, IDLegameDich_old, j As Integer
    '    Dim drFiltroDich(), drFiltroAcc() As DataRow
    '    IDLegameDich_old = -1
    '    Try
    '        For y = 0 To objDSDichiarazioni.Tables(0).DefaultView.Count - 1

    '            Dim i As Integer = 0


    '            IDLegameDich = objDSDichiarazioni.Tables(0).Rows(y).Item("idLegame")
    '            If IDLegameDich <> IDLegameDich_old Then

    '                'Dipe 30/11/2009
    '                ID_IMMOBILE_ORIGINALE_DICHIARATO = ""

    '                drFiltroDich = objDSDichiarazioni.Tables(0).Select("idLegame='" & IDLegameDich & "'")
    '                For j = 0 To drFiltroDich.Length - 1
    '                    ID_IMMOBILE_ORIGINALE_DICHIARATO = ID_IMMOBILE_ORIGINALE_DICHIARATO & drFiltroDich(j).Item("id") & ","
    '                Next
    '                ID_IMMOBILE_ORIGINALE_DICHIARATO = Mid(ID_IMMOBILE_ORIGINALE_DICHIARATO, 1, Len(ID_IMMOBILE_ORIGINALE_DICHIARATO) - 1)

    '                drFiltroAcc = objDTAccertato.Select("idLegame='" & IDLegameDich & "'")
    '                For j = 0 To drFiltroAcc.Length - 1
    '                    'dipe 09/06/2010
    '                    drFiltroAcc(j).Item("ID_IMMOBILE_ORIGINALE_DICHIARATO") = ID_IMMOBILE_ORIGINALE_DICHIARATO
    '                    drFiltroAcc(j).AcceptChanges()
    '                    iTrovato += 1
    '                Next
    '            Else
    '                iTrovato += 1
    '            End If

    '            IDLegameDich_old = IDLegameDich
    '        Next

    '        If iTrovato < objDSDichiarazioni.Tables(0).DefaultView.Count Then
    '            Trovato = False
    '        End If
    '        'For y = 0 To objDSDichiarazioni.Tables(0).DefaultView.Count - 1

    '        '    Dim i As Integer = 0
    '        '    'Cerco l'immobile dichiarato in tutti gli immobili in accertato
    '        '    'Se lo trovo esco dal ciclo e proseguo a cercare con immobili
    '        '    'successivo di accertamento
    '        '    For i = 0 To objDTAccertato.DefaultView.Count - 1
    '        '        Trovato = False
    '        '        If objDSDichiarazioni.Tables(0).Rows(y).Item("idLegame") = objDTAccertato.Rows(i).Item("idLegame") Then
    '        '            ID_IMMOBILE_ORIGINALE_DICHIARATO = objDSDichiarazioni.Tables(0).Rows(y).Item("id")
    '        '            objDTAccertato.Rows(i).Item("ID_IMMOBILE_ORIGINALE_DICHIARATO") = ID_IMMOBILE_ORIGINALE_DICHIARATO
    '        '            Trovato = True
    '        '            Exit For
    '        '        End If
    '        '    Next
    '        '    'Se trovato = False vuol dire che nn ho trovato l'immobile, quindi blocco l'accertamento
    '        '    If Trovato = False Then
    '        '        Exit For
    '        '    End If
    '        'Next

    '        'If Trovato = True Then
    '        '    objDTAccertato.AcceptChanges()
    '        'End If

    '        Return Trovato
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.grdAccertato.ControlloLEGAME.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Function

    'Private Function CalcolaAccertato(objICI() As objSituazioneFinale) As Boolean
    '    'workTable è ordinato per progressivo
    '    Dim maxLeg As Integer
    '    Dim i As Integer = 0
    '    Dim rowsArray() As DataRow
    '    Dim workTable As DataTable
    '    maxLeg = maxLegame("idLegame")
    '    If Not Session("DataTableImmobiliDaAccertare") Is Nothing Then
    '        workTable = CType(Session("DataTableImmobiliDaAccertare"), DataTable)
    '    End If

    '    'objICI = FncGen.CreateDSperCalcoloICI()
    '    Dim objHashTableICI As Hashtable
    '    'La session viene creata quando ricerco in territorio gli immobili
    '    objHashTableICI = Session("HashTableDichiarazioniAccertamenti")

    '    Try
    '        objHashTableICI("IDSOTTOAPPLICAZIONETERRITORIO") = ConfigurationManager.AppSettings("OPENGOVT")
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.grdAccertato.CalcoloAccertato.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '        Dim strConnTerritorio As String
    '        objHashTableICI = New Hashtable
    '        strConnTerritorio = ConfigurationManager.AppSettings("OPENGOVT")
    '        objHashTableICI.Add("IDSOTTOAPPLICAZIONETERRITORIO", strConnTerritorio)
    '    End Try

    '    Try
    '        For i = 1 To maxLeg
    '            'trasformo l'oggetto in ingresso con quello da passare al servizio di calcolo (il datatable da 54item in tp_situazione_finale_ici contenuto in objICI) N.B. è popolato in ordine di legame
    '            rowsArray = workTable.Select("IDLEGAME='" & i & "'")
    '            If rowsArray.Length > 0 Then
    '                '*** 20110506 - se non calcolo non funziona il resto perchè ho objICI vuoto *** 
    '                'If rowsArray(0).Item("ICICalcolato") = "-1" Then
    '                'rowsArray(0).Item("ICICalcolato") = CalcolaICI(objHashTableICI, rowsArray, objICI) 'la funzione non serve a nulla perchè richiama solo l'altra e ne restituisce il valore
    '                rowsArray(0).Item("ICICalcolato") = addRowsCalcoloICI(objICI, objHashTableICI, rowsArray)
    '                If rowsArray(0).Item("ICICalcolato") < 0 Then
    '                    '*** 20120704 - IMU ***
    '                    Throw New Exception("grdAccertato::CalcolaAccertato::Errore in calcolo ICI/IMU")
    '                End If
    '                rowsArray(0).AcceptChanges()
    '                workTable.AcceptChanges()
    '                'End If
    '            End If
    '        Next
    '        'Lancio il calcolo ICI richiamando il servizio
    '        If CalcolaICITotale(objICI, objHashTableICI, TipoCalcolo) = False Then
    '            Return False
    '        End If

    '        'Aggiorno Colonna ICI Calcolato nella Griglia dell'accertato
    '        If objICI.GetLength(0) > 0 Then
    '            '*** 20110506 è sbagliato perchè si aggiorna un'oggetto ordinato per progressivo con il valore di quello ordinato per legame senza assicurarsi di essere effettivamente sulla stessa posizione ***
    '            'For i = 0 To workTable.Rows.Count - 1
    '            '    Dim rows() As DataRow
    '            '    rows = objICI.Tables("TP_SITUAZIONE_FINALE_ICI").Select("ID_SITUAZIONE_FINALE='" & i + 1 & "'")
    '            '    If rows.Length > 0 Then
    '            '        workTable.Rows(i).Item("ICICalcolato") = rows(0).Item("ICI_TOTALE_DOVUTA")
    '            '        workTable.Rows(i).Item("ICICalcolatoACCONTO") = rows(0).Item("ICI_DOVUTA_ACCONTO")
    '            '        workTable.Rows(i).Item("ICICalcolatoSALDO") = rows(0).Item("ICI_DOVUTA_SALDO")
    '            '        workTable.Rows(i).Item("ICI_VALORE_ALIQUOTA") = rows(0).Item("ICI_VALORE_ALIQUOTA")
    '            '    Else
    '            '        workTable.Rows(i).Item("ICICalcolato") = 0
    '            '        workTable.Rows(i).Item("ICICalcolatoACCONTO") = 0
    '            '        workTable.Rows(i).Item("ICICalcolatoSALDO") = 0
    '            '        workTable.Rows(i).Item("ICI_VALORE_ALIQUOTA") = 0
    '            '    End If
    '            '    workTable.AcceptChanges()
    '            'Next
    '            'ciclo sugli immobili della videata
    '            For i = 0 To workTable.Rows.Count - 1
    '                'prelevo l'immobile della videata per id legame
    '                rowsArray = workTable.Select("IDLEGAME='" & i + 1 & "'")
    '                If rowsArray.Length > 0 Then
    '                    rowsArray(0).Item("ICICalcolato") = 0
    '                    rowsArray(0).Item("ICICalcolatoACCONTO") = 0
    '                    rowsArray(0).Item("ICICalcolatoSALDO") = 0
    '                    rowsArray(0).Item("ICI_VALORE_ALIQUOTA") = 0
    '                    'prelevo l'immobile calcolato per id legame
    '                    For Each mySitFinale As objSituazioneFinale In objICI
    '                        If mySitFinale.Id = i + 1 Then
    '                            'aggiorno i valori corrispondenti
    '                            rowsArray(0).Item("ICICalcolato") = mySitFinale.TotDovuto
    '                            rowsArray(0).Item("ICICalcolatoACCONTO") = mySitFinale.AccDovuto
    '                            rowsArray(0).Item("ICICalcolatoSALDO") = mySitFinale.SalDovuto
    '                            rowsArray(0).Item("ICI_VALORE_ALIQUOTA") = mySitFinale.Aliquota
    '                            'aggiorno l'oggetto iniziale
    '                            rowsArray(0).AcceptChanges()
    '                            workTable.AcceptChanges()
    '                        End If
    '                    Next
    '                End If
    '            Next
    '        End If
    '        Session("DataTableImmobiliDaAccertare") = workTable
    '        Return True
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.grdAccertato.CalcoloAccertato.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '        Return False
    '    End Try
    'End Function

    'Private Function maxLegame(ByVal campo As String) As Integer
    '    Dim workTable As DataTable
    '    Try
    '        If Not Session("DataTableImmobiliDaAccertare") Is Nothing Then
    '            workTable = CType(Session("DataTableImmobiliDaAccertare"), DataTable)
    '        End If
    '        Dim i As Integer = 0
    '        If workTable.DefaultView.Count = 0 Then
    '            maxLegame = 0
    '        Else
    '            Dim dw As DataView
    '            dw = workTable.DefaultView
    '            'alerole: bug in ordinamento per idlegame
    '            'dw.Sort = "idLegame desc"
    '            'maxLegame = dw.Item(0).Item("idLegame")

    '            For i = 0 To dw.Table.Rows.Count - 1
    '                Dim legame As Integer = dw.Table.Rows(i).Item("idLegame")
    '                If legame > maxLegame Then
    '                    maxLegame = legame
    '                End If
    '            Next
    '        End If

    '        Return maxLegame
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.grdAccertato.maxLegame.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Function

    'Private Function CalcolaICI(ByVal objHashTable As Hashtable, ByVal rowICI() As DataRow, ByVal objICI() As objSituazioneFinale) As Double
    '    'CalcolaICI = 0
    '    'Dim rnd As Random
    '    'rnd = New Random
    '    'CalcolaICI = CDbl(rnd.Next(10000)) / 100
    '    'System.Threading.Thread.Sleep(500)
    '    Return addRowsCalcoloICI(objICI, objHashTable, rowICI)
    'End Function

    'Private Function addRowsCalcoloICI(ByRef objICI() As objSituazioneFinale, ByVal objHashTable As Hashtable, ByVal rowICI() As DataRow) As Double
    '    'serve per trasformare l'oggetto in videata con quello da passare al servizio di calcolo
    '    Dim row As New objSituazioneFinale
    '    'Dim newTable As DataTable
    '    'Dim objDSImmobili As DataSet
    '    Dim clsGeneralFunction As New MyUtility

    '    '******************************************************************
    '    'Cerco  gli immobili dalla Griglia
    '    '******************************************************************

    '    'Determino numero di utilizzatori
    '    'Dim objDBTerritorio As New DBTERRITORIO.TerritoriDB
    '    Dim nUtilizzatori As Integer
    '    Dim rowImmobile() As DataRow
    '    Dim i As Integer = 0
    '    Dim objDBProvv As New DBPROVVEDIMENTI.ProvvedimentiDB
    '    Dim Valore, ValoreCalcolato As Double

    '    Try
    '        rowImmobile = rowICI

    '        If IsDBNull(rowImmobile(0).Item("NUMEROUTILIZZATORI")) Then
    '            nUtilizzatori = 0
    '        ElseIf rowImmobile(0).Item("NUMEROUTILIZZATORI") = "" Then
    '            nUtilizzatori = 0
    '        Else
    '            nUtilizzatori = rowImmobile(0).Item("NUMEROUTILIZZATORI")
    '        End If

    '        If rowImmobile(0).Item("RENDITA_VALORE") <> "" Then
    '            Valore = rowImmobile(0).Item("RENDITA_VALORE")
    '        Else
    '            Valore = 0
    '        End If

    '        '**********************************************************************
    '        'Determino Mesi Acconto e Totali
    '        '**********************************************************************
    '        Dim nMesiAcconto, nMeseTotali As Integer
    '        Dim DataInizio, DataFine As String
    '        'DAL
    '        'If Mid(rowImmobile(0).Item("DAL"), 1, 4) < objHashTable("ANNOACCERTAMENTO") Then
    '        If CDate(rowImmobile(0).Item("DAL")).Year < objHashTable("ANNOACCERTAMENTO") Then
    '            DataInizio = "01/01/" & objHashTable("ANNOACCERTAMENTO")
    '        Else
    '            DataInizio = annoBarra(rowImmobile(0).Item("DAL"))
    '        End If

    '        'AL
    '        If IsDBNull(rowImmobile(0).Item("AL")) Then
    '            DataFine = "31/12/" & objHashTable("ANNOACCERTAMENTO")
    '        ElseIf rowImmobile(0).Item("AL") = "" Then
    '            DataFine = "31/12/" & objHashTable("ANNOACCERTAMENTO")
    '        Else
    '            DataFine = annoBarra(rowImmobile(0).Item("AL"))
    '        End If

    '        'nGiorni = DateDiff(DateInterval.DayOfYear, DataInizio, DataFine)
    '        'nMeseTotali = Int((nGiorni / 30))
    '        'nMesiAcconto = DateDiff(DateInterval.Month, DataInizio, "01/01/" & objHashTable("ANNOACCERTAMENTO"))

    '        mesi_possesso(nMeseTotali, DataInizio, DataFine, 1, objHashTable("ANNOACCERTAMENTO"))

    '        Dim MeseAcconto As Integer
    '        If glbmese_fine_p > 6 Then
    '            MeseAcconto = 6
    '        Else
    '            MeseAcconto = glbmese_fine_p
    '        End If

    '        nMesiAcconto = (MeseAcconto - glbmese_inizio_p) + 1

    '        'giro le date per il calcolo ici Immobili B
    '        'DataInizio = clsGeneralFunction.GiraData(DataInizio)
    '        'DataFine = clsGeneralFunction.GiraData(DataFine)

    '        '**********************************************************************
    '        'Fine Calcolo Mesi
    '        '**********************************************************************

    '        row.Provenienza = "A"
    '        row.IdContribuente = objHashTable("CODCONTRIBUENTE")
    '        row.Anno = objHashTable("ANNOACCERTAMENTO")
    '        If nMesiAcconto < 0 Then
    '            row.AccMesi = 0
    '        Else
    '            row.AccMesi = nMesiAcconto
    '        End If
    '        row.Mesi = nMeseTotali
    '        row.NUtilizzatori = nUtilizzatori

    '        If rowImmobile(0).Item("FLAG_PRINCIPALE") = "False" Then
    '            row.FlagPrincipale = 0
    '        ElseIf rowImmobile(0).Item("FLAG_PRINCIPALE") = "0" Then
    '            row.FlagPrincipale = 0
    '        End If

    '        If rowImmobile(0).Item("FLAG_PRINCIPALE") = "True" Then
    '            row.FlagPrincipale = 1
    '        ElseIf rowImmobile(0).Item("FLAG_PRINCIPALE") = "1" Then
    '            row.FlagPrincipale = 1
    '        End If

    '        If rowImmobile(0).Item("ID_IMMOBILE_ORIGINALE_DICHIARATO").ToString() = "" Or rowImmobile(0).Item("ID_IMMOBILE_ORIGINALE_DICHIARATO").ToString() = "-1" Then
    '            row.IdImmobile = rowImmobile(0).Item("PROGRESSIVO")
    '        Else
    '            row.IdImmobile = CLng(rowImmobile(0).Item("ID_IMMOBILE_ORIGINALE_DICHIARATO"))
    '        End If
    '        '*************************************************************
    '        'PERTINENZA
    '        '*************************************************************
    '        'Se è una pertinenza metto 2 a il flag principale
    '        If rowImmobile(0).Item("TR") <> "AF" And rowImmobile(0).Item("TR") <> "TA" And CStr(rowImmobile(0).Item("CATEGORIA")).StartsWith("C") Then
    '            If rowImmobile(0).Item("FLAG_PERTINENZA") = "True" Then
    '                row.FlagPrincipale = 2
    '            ElseIf rowImmobile(0).Item("FLAG_PERTINENZA") = "1" Then
    '                row.FlagPrincipale = 2
    '            End If
    '            'Dipe altrimenti non viene trovatro l'immobile corretto per il calcolo dell'ici sulla pertinenza
    '            'row.COD_IMMOBILE") = rowImmobile(0).Item("PROGRESSIVO")
    '            If rowImmobile(0).Item("ID_IMMOBILE_ORIGINALE_DICHIARATO").ToString() = "" Or rowImmobile(0).Item("ID_IMMOBILE_ORIGINALE_DICHIARATO").ToString() = "-1" Then
    '                row.IdImmobile = rowImmobile(0).Item("PROGRESSIVO")
    '            Else
    '                row.IdImmobile = CLng(rowImmobile(0).Item("ID_IMMOBILE_ORIGINALE_DICHIARATO"))
    '            End If

    '            Dim objUtility As New MyUtility

    '            'GIULIA 17082005
    '            'If Len(objUtility.CToStr(rowImmobile(0).Item("IDIMMOBILEPERTINENZA"))) = 0 Then
    '            '    row.COD_IMMOBILE_PERTINENZA") = 0
    '            'Else
    '            '    row.COD_IMMOBILE_PERTINENZA") = rowImmobile(0).Item("IDIMMOBILEPERTINENZA")
    '            'End If
    '            If objUtility.CToStr(rowImmobile(0).Item("IDIMMOBILEPERTINENZA")) = "" Then
    '                row.IdImmobilePertinenza = 0
    '            Else
    '                row.IdImmobilePertinenza = rowImmobile(0).Item("IDIMMOBILEPERTINENZA")
    '            End If
    '        End If

    '        'row.ID_SITUAZIONE_FINALE") = rowImmobile(0).Item("PROGRESSIVO")
    '        row.Id = rowImmobile(0).Item("IDLEGAME")
    '        '*************************************************************
    '        'FINE PERTINENZA
    '        '*************************************************************

    '        row.PercPossesso = rowImmobile(0).Item("PERCPOSSESSO")
    '        row.MesiRiduzione = rowImmobile(0).Item("MESIRIDUZIONE")
    '        'vale sempre la regola valori invertiti perchè in dichiarato dati possesso
    '        '0=SI
    '        '1=NO
    '        '2=non compilato
    '        If rowImmobile(0).Item("FLAG_RIDOTTO") = "False" Or rowImmobile(0).Item("FLAG_RIDOTTO") = "0" Then
    '            'row.RIDUZIONE") = 0
    '            row.FlagRiduzione = 1
    '            'ElseIf rowImmobile(0).Item("FLAG_RIDOTTO") = "0" Then
    '            '    'row.RIDUZIONE") = rowImmobile(0).Item("FLAG_RIDOTTO")
    '            '    row.FLAG_RIDUZIONE") = rowImmobile(0).Item("FLAG_RIDOTTO")
    '        End If

    '        If rowImmobile(0).Item("FLAG_RIDOTTO") = "True" Or rowImmobile(0).Item("FLAG_RIDOTTO") = "1" Then
    '            'row.RIDUZIONE") = 1
    '            row.FlagRiduzione = 0
    '            'ElseIf rowImmobile(0).Item("FLAG_RIDOTTO") = "1" Then
    '            '    'row.RIDUZIONE") = rowImmobile(0).Item("FLAG_RIDOTTO")
    '            '    row.FLAG_RIDUZIONE") = rowImmobile(0).Item("FLAG_RIDOTTO")
    '        End If

    '        row.IdEnte = ConstSession.IdEnte
    '        'Altri Dati Relativi agli Immobili
    '        row.Foglio = rowImmobile(0).Item("FOGLIO")
    '        row.Numero = rowImmobile(0).Item("NUMERO")
    '        row.Subalterno = rowImmobile(0).Item("SUBALTERNO")
    '        row.Categoria = UCase(rowImmobile(0).Item("CATEGORIA"))
    '        row.Classe = rowImmobile(0).Item("CLASSE")
    '        row.Dal = clsGeneralFunction.GiraData(DataInizio)   'rowImmobile(0).Item("DAL")
    '        row.Al = clsGeneralFunction.GiraData(DataFine) ' rowImmobile(0).Item("AL")
    '        row.TipoRendita = rowImmobile(0).Item("TR")
    '        row.Sezione = rowImmobile(0).Item("SEZIONE")
    '        'row.INDIRIZZO") = rowImmobile(0).Item("INDIRIZZO")
    '        row.Via = ""

    '        row.TipoOperazione = "A"

    '        If rowImmobile(0).Item("AL") <> "" Then
    '            If rowImmobile(0).Item("AL") < objHashTable("ANNOACCERTAMENTO") & "1231" Then
    '                row.FlagPosseduto = "0"
    '            Else
    '                row.FlagPosseduto = "1"
    '            End If
    '        Else
    '            row.FlagPosseduto = "1"
    '        End If
    '        '*** 20140509 - TASI ***
    '        'row.TIPO_POSSESSO") = rowImmobile(0).Item("CODTITPOSSESSO")
    '        row.IdTipoUtilizzo = rowImmobile(0).Item("CODTITPOSSESSO")
    '        row.DataInizio = clsGeneralFunction.ReplaceDataForDB(rowImmobile(0).Item("dal"))
    '        row.Tributo = rowImmobile(0).Item("CODTRIBUTO")
    '        '*** ***
    '        'vale sempre la regola valori invertiti perchè in dichiarato dati possesso
    '        '0=SI
    '        '1=NO
    '        '2=non compilato
    '        If rowImmobile(0).Item("CODTITPOSSESSO") = "7" Then 'Or rowImmobile(0).Item("mesiEsclusioneEsenzione") = "0" Then
    '            row.FlagEsente = 1
    '        Else
    '            row.FlagEsente = rowImmobile(0).Item("FLAG_ESENTE")
    '        End If
    '        Log.Debug("grdAccertato.addRowsCalcoloICI.sono su: " + row.Foglio + "|" + row.Numero + "|" + row.Subalterno + " esenzione->" + row.FlagEsente)
    '        If rowImmobile(0).Item("mesiEsclusioneEsenzione") > 0 Then
    '            'row.MESI_ESCL_ESENZIONE") = rowImmobile(0).Item("mesiEsclusioneEsenzione")
    '            mesi_possesso(row.MesiEsenzione, DataInizio, DataFine, 1, objHashTable("ANNOACCERTAMENTO"))
    '        End If
    '        If CStr(rowImmobile(0).Item("RENDITA")) <> "" Then
    '            row.Rendita = rowImmobile(0).Item("RENDITA")
    '        Else
    '            row.Rendita = 0
    '        End If

    '        'Valore = objDBProvv.CalcoloValoredaRendita(rowImmobile(0).Item("RENDITA_VALORE"), rowImmobile(0).Item("TR"), rowImmobile(0).Item("CATEGORIA"), objHashTable("ANNOACCERTAMENTO"))
    '        '*** 20120530 - IMU ***
    '        'devo ricalcolare il valore aggiornato
    '        Dim FncValore As New ComPlusInterface.FncICI
    '        '*** ***
    '        Try
    '            If Not IsDBNull(rowImmobile(0).Item("COLTIVATOREDIRETTO")) Then
    '                row.IsColtivatoreDiretto = CBool(rowImmobile(0).Item("COLTIVATOREDIRETTO"))
    '            End If
    '        Catch ex As Exception
    '            row.IsColtivatoreDiretto = False
    '        End Try
    '        Try
    '            If Not IsDBNull(rowImmobile(0).Item("NUMEROFIGLI")) Then
    '                row.NumeroFigli = rowImmobile(0).Item("NUMEROFIGLI")
    '            End If
    '        Catch ex As Exception
    '            row.NumeroFigli = 0
    '        End Try
    '        Try
    '            If Not IsDBNull(rowImmobile(0).Item("PERCENTCARICOFIGLI")) Then
    '                row.PercentCaricoFigli = rowImmobile(0).Item("PERCENTCARICOFIGLI")
    '            End If
    '        Catch ex As Exception
    '            row.PercentCaricoFigli = 0
    '        End Try
    '        '*** 20120709 - IMU per AF e LC devo usare il campo valore ***
    '        Dim ValoreDich As Double = 0
    '        If rowImmobile(0).Item("RENDITA_VALORE") <> "" Then
    '            ValoreDich = rowImmobile(0).Item("RENDITA_VALORE")
    '        End If
    '        ValoreCalcolato = FncValore.CalcoloValore(ConstSession.StringConnectionOPENgov, ConstSession.StringConnectionICI, ConstSession.IdEnte, Year(rowImmobile(0).Item("dal")), rowImmobile(0).Item("TR"), row.Categoria, row.Classe, row.Zona, row.Rendita, ValoreDich, row.Consistenza, CDate(rowImmobile(0).Item("dal")), row.IsColtivatoreDiretto)
    '        '*** ***
    '        If ValoreCalcolato <> Valore And ValoreCalcolato > Valore Then
    '            Valore = ValoreCalcolato
    '        End If
    '        row.Valore = Valore

    '        'row("PROVENIENZA") 
    '        'row("CARATTERISTICA")
    '        'row("PROTOCOLLO") 
    '        'row("FLAG_STORICO") 
    '        'row("FLAG_PROVVISORIO") 
    '        'row("MESI_POSSESSO") 
    '        'row("MESI_ESCL_ESENZIONE") 
    '        'row("MESI_RIDUZIONE") 
    '        'row("IMPORTO_DETRAZIONE") xxx ma non usato
    '        'row("FLAG_ESENTE") xxx
    '        'row("MESE_INIZIO") 
    '        'row("DATA_SCADENZA")
    '        'row("TIPO_POSSESSO") xxx


    '        'Mi serve per calcolare l'ICI solo su 1 Immobile. Viene passato alla procedura di calcolo
    '        'dell'ICI. Viene poi distrutto all'uscita del metodo
    '        'Dim objDSImmobiliAppoggio As DataSet
    '        ''Creo la struttura 
    '        'objDSImmobiliAppoggio = FncGen.CreateDSperCalcoloICI()
    '        'objDSImmobiliAppoggio.Tables("TP_SITUAZIONE_FINALE_ICI").ImportRow(row)
    '        'objDSImmobiliAppoggio.AcceptChanges()

    '        'Aggiungo Riga a mio DS x Situazione Finali ICI
    '        Dim myArray As New ArrayList(objICI)
    '        myArray.Add(row)
    '        objICI = CType(myArray.ToArray(GetType(objSituazioneFinale)), objSituazioneFinale())
    '        Return 1
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.grdAccertato.addRowsCalcoloICI.errore:  ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '        Return -1
    '    End Try
    'End Function

    'Private Function CalcolaICITotale(ByRef objICI() As objSituazioneFinale, ByVal objHashTable As Hashtable, ByVal TipoCalcolo As Integer) As Boolean
    '    'Calcolo ICI
    '    Try
    '        '*** 20140509 - TASI ***            
    '        'Dim objCOMCalcoloICI As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
    '        'objICI = objCOMCalcoloICI.GetICI(objICI, objHashTable, TipoCalcolo)
    '        Dim objCOMCalcoloICI As IFreezer = Activator.GetObject(GetType(ComPlusInterface.IFreezer), ConstSession.URLServiziFreezer)
    '        Log.Debug("richiamo calcoloici da grdaccertato.CalcolaICITotale")
    '        objICI = objCOMCalcoloICI.CalcoloICI(objICI, objHashTable, TipoCalcolo)
    '        '*** ***
    '        Return True
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.grdAccertato.CalcoloICITotale.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '        Return False
    '    End Try
    'End Function

    'Private Function SetObjDSAppoggioSanzioni(ByVal anno As Integer, ByVal DiffImposta As Double, ByVal ImportoVersato As Double) As DataSet

    '    Try

    '        Dim objDS As New DataSet


    '        Dim newTableAppoggio As DataTable
    '        newTableAppoggio = New DataTable("TP_APPOGGIO_CALCOLO_SANZIONI")

    '        Dim NewColumn As New DataColumn
    '        NewColumn.ColumnName = "ANNO"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTableAppoggio.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "IVA"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = System.DBNull.Value
    '        newTableAppoggio.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "IVS"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = System.DBNull.Value
    '        newTableAppoggio.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "IVUS"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = System.DBNull.Value
    '        newTableAppoggio.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "IV"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = System.DBNull.Value
    '        newTableAppoggio.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DI"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = System.DBNull.Value
    '        newTableAppoggio.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "GG"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = System.DBNull.Value
    '        newTableAppoggio.Columns.Add(NewColumn)


    '        Dim row1 As DataRow

    '        row1 = newTableAppoggio.NewRow()

    '        row1.Item("ANNO") = anno
    '        row1.Item("DI") = DiffImposta
    '        row1.Item("IV") = ImportoVersato

    '        newTableAppoggio.Rows.Add(row1)

    '        objDS.Tables.Add(newTableAppoggio)

    '        Return objDS

    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.grdAccertato.SetObjDSAppoggioSanzioni.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try


    'End Function

    'Private Function CreateDatasetPerSanzInt(ByVal anno As Integer, ByVal codContribuente As String, ByVal DiffImposta As Double, ByVal DiffImpostaACCONTO As Double, ByVal DiffImpostaSALDO As Double) As DataSet

    '    Dim objDS As New DataSet

    '    Dim newTable As DataTable
    '    newTable = New DataTable("TABLE")
    '    Dim NewColumn As New DataColumn
    '    Try
    '        NewColumn = New DataColumn

    '        NewColumn.ColumnName = "COD_CONTRIBUENTE"
    '        NewColumn.DataType = System.Type.GetType("System.Int64")
    '        NewColumn.DefaultValue = 0
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ANNO"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)


    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DIFFERENZA_IMPOSTA_ACCONTO"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = 0
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DIFFERENZA_IMPOSTA_SALDO"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = 0
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DIFFERENZA_IMPOSTA_TOTALE"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "IMPORTO_SANZIONI"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "IMPORTO_SANZIONI_RIDOTTO"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        'dipe
    '        'NewColumn = New DataColumn
    '        'NewColumn.ColumnName = "IMPORTO_TOT_SANZIONI_RIDUCIBILI"
    '        'NewColumn.DataType = System.Type.GetType("System.String")
    '        'NewColumn.DefaultValue = "0"
    '        'newTable.Columns.Add(NewColumn)

    '        'NewColumn = New DataColumn
    '        'NewColumn.ColumnName = "IMPORTO_TOT_SANZIONI_RIDOTTE"
    '        'NewColumn.DataType = System.Type.GetType("System.String")
    '        'NewColumn.DefaultValue = "0"
    '        'newTable.Columns.Add(NewColumn)

    '        'NewColumn = New DataColumn
    '        'NewColumn.ColumnName = "IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI"
    '        'NewColumn.DataType = System.Type.GetType("System.String")
    '        'NewColumn.DefaultValue = "0"
    '        'newTable.Columns.Add(NewColumn)

    '        '************

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "IMPORTO_INTERESSI"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = "0"
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "FLAG_MODALITA_UNICA_SOLUZIONE"
    '        NewColumn.DataType = System.Type.GetType("System.Boolean")
    '        NewColumn.DefaultValue = False
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "IMPORTO_TOTALE_DICHIARATO"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = 0
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "IMPORTO_TOTALE_VERSATO"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = 0
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "IMPORTO_TOTALE_ACCERTATO"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = 0
    '        newTable.Columns.Add(NewColumn)
    '        '*** 20140701 - IMU/TARES ***
    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "QUOTARIDUZIONESANZIONI"
    '        NewColumn.DataType = System.Type.GetType("System.Int64")
    '        NewColumn.DefaultValue = 1
    '        newTable.Columns.Add(NewColumn)
    '        '*** ***

    '        Dim row1 As DataRow

    '        row1 = newTable.NewRow()
    '        row1.Item("COD_CONTRIBUENTE") = codContribuente
    '        row1.Item("ANNO") = anno
    '        row1.Item("IMPORTO_SANZIONI") = 0
    '        row1.Item("IMPORTO_SANZIONI_RIDOTTO") = 0
    '        'dipe
    '        'row1.Item("IMPORTO_TOT_SANZIONI_RIDUCIBILI") = 0
    '        'row1.Item("IMPORTO_TOT_SANZIONI_RIDOTTE") = 0
    '        'row1.Item("IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI") = 0
    '        '*****
    '        row1.Item("IMPORTO_INTERESSI") = 0
    '        row1.Item("DIFFERENZA_IMPOSTA_TOTALE") = DiffImposta
    '        row1.Item("DIFFERENZA_IMPOSTA_ACCONTO") = DiffImpostaACCONTO
    '        row1.Item("DIFFERENZA_IMPOSTA_SALDO") = DiffImpostaSALDO

    '        row1.Item("IMPORTO_TOTALE_DICHIARATO") = 0
    '        row1.Item("IMPORTO_TOTALE_VERSATO") = 0
    '        row1.Item("IMPORTO_TOTALE_ACCERTATO") = 0

    '        row1.Item("FLAG_MODALITA_UNICA_SOLUZIONE") = False
    '        '*** 20140701 - IMU/TARES ***
    '        If anno >= 2012 Then
    '            row1.Item("QUOTARIDUZIONESANZIONI") = 3
    '        Else
    '            row1.Item("QUOTARIDUZIONESANZIONI") = 4
    '        End If
    '        '*** ***
    '        newTable.Rows.Add(row1)

    '        objDS.Tables.Add(newTable)

    '        Return objDS
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.grdAccertato.CreateDatasetPerSanzInt.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Function

    'Protected Function annoBarra(ByVal objtemp As Object) As String
    '    Dim clsGeneralFunction As New MyUtility
    '    Dim strTemp As String = ""
    '    Try
    '        If Not IsDBNull(objtemp) Then
    '            If CStr(objtemp).CompareTo("") <> 0 Then
    '                If CDate(objtemp).Date = DateTime.MinValue.Date Or CDate(objtemp).Date = DateTime.MaxValue.Date Then
    '                    strTemp = ""
    '                Else
    '                    Dim MiaData As String = CType(objtemp, DateTime).ToString("yyyy/MM/dd")
    '                    '                strTemp = clsGeneralFunction.GiraDataFromDB(objtemp)
    '                    strTemp = clsGeneralFunction.GiraDataCompletaFromDB(MiaData)
    '                End If
    '            Else
    '                strTemp = ""
    '            End If
    '        End If
    '        Return strTemp
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.grdAccertato.annoBarra.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Function

    'Protected Function checkFlag(ByVal objtemp As Object) As String
    '    Dim strTemp As String = ""
    '    Try
    '        If Not IsDBNull(objtemp) Then
    '            If objtemp = "1" Or UCase(objtemp) = "TRUE" Then
    '                Return "True"
    '            Else
    '                Return "False"
    '            End If
    '        Else
    '            Return "False"
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.grdAccertato.checkFlag.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Function

    'Private Sub mesi_possesso(ByRef mesipossesso As Integer, ByVal dal As String, ByVal al As String, ByVal tipo_periodo As Integer, ByVal annoAccertamento As Integer)
    '    '********************************************************
    '    'Input:
    '    '       tipo_periodo: 1 Possesso, 2 Catasto_s, 3 Catasto_p
    '    '       dal : Data partenza periodo
    '    '       al : data chiusura periodo
    '    '
    '    'Output:
    '    '       mesipossesso: mesi di possesso del periodo in input rispetto
    '    'all()		'anno in esame
    '    '       impostazioni variabili globali
    '    '
    '    '********************************************************

    '    Dim mese As Integer
    '    Dim aggiunta_mese As Integer
    '    Dim data_ultimo_gg_mese As String


    '    'Azzero le variabili globali
    '    Try
    '        Select Case tipo_periodo
    '            Case 1    'Periodo di possesso
    '                glbmese_inizio_p = 0
    '                glbmese_fine_p = 0
    '            Case 2    'Periodo di classamento
    '                glbmese_inizio_s = 0
    '                glbmese_fine_s = 0
    '        End Select

    '        If Year(dal) < annoAccertamento Then
    '            If al = "" Then
    '                mesipossesso = 12
    '            Else
    '                If Year(al) > annoAccertamento Then
    '                    mesipossesso = 12
    '                Else
    '                    If Year(al) < annoAccertamento Then
    '                        mesipossesso = 0
    '                    Else
    '                        'Forzo la data Dal all'inizio anno
    '                        dal = "01/01" + "/" + Trim(annoAccertamento)
    '                        'Verifico quanti gg di possosseso ci sono nel mese della Data(al)
    '                        If Day(al) > 14 Then
    '                            aggiunta_mese = aggiunta_mese + 1
    '                        End If
    '                        'Calcolo i mesi di possesso per l'anno in esame
    '                        mesipossesso = DateDiff("M", dal, al) + aggiunta_mese
    '                    End If
    '                End If
    '            End If
    '            Select Case tipo_periodo
    '                Case 1     'Periodo di possesso
    '                    glbmese_inizio_p = 1
    '                    glbmese_fine_p = mesipossesso
    '                Case 2     'Periodo di classamento
    '                    glbmese_inizio_s = 1
    '                    glbmese_fine_s = mesipossesso
    '            End Select
    '            'glbmese_inizio = 1
    '            'glbmese_fine = mesipossesso
    '        Else
    '            If Year(dal) = annoAccertamento Then
    '                'Determino quanti giorni di possesso ci sono nel mese
    '                'del dal
    '                mese = Month(dal)
    '                data_ultimo_gg_mese = Trim(Str(FncGrd.giorni_mese(mese))) + "/" + Trim(Str(mese)) + "/" + Trim(Str(annoAccertamento))
    '                'Imposto i mesi di inizio/fine del periodo
    '                Select Case tipo_periodo
    '                    Case 1      'Periodo di possesso
    '                        glbmese_inizio_p = mese
    '                        glbmese_fine_p = 12
    '                    Case 2      'Periodo di classamento
    '                        glbmese_inizio_s = mese
    '                        glbmese_fine_s = 12
    '                End Select
    '                'glbmese_inizio = mese
    '                'glbmese_fine = 12
    '                'Verfico se, in presenza di febbraio, ho 28 o 29 gg
    '                If mese = 2 Then
    '                    Try
    '                        data_ultimo_gg_mese = DateValue(data_ultimo_gg_mese)
    '                    Catch ex As Exception
    '                        data_ultimo_gg_mese = "28" + "/" + Trim(Str(mese)) + "/" + Trim(Str(annoAccertamento))
    '                    End Try
    '                End If
    '                'Verifico quanti gg di possosseso ci sono nel mese della data Dal
    '                If mese = 2 Then
    '                    If DateDiff("d", dal, data_ultimo_gg_mese) < 14 Then     'modifica 8.1
    '                        'aggiunto = perchè in caso di un rif. cat
    '                        aggiunta_mese = -1
    '                        'secondario dal 16/11/1993 al ........ già di un secondo
    '                        'Sposto di uno il mese di inizio
    '                        'proprietario mi calcolava un mese per il primo
    '                        Select Case tipo_periodo
    '                            Case 1       'Periodo di possesso
    '                                glbmese_inizio_p = Month(dal) + 1
    '                            Case 2       'Periodo di classamento
    '                                glbmese_inizio_s = Month(dal) + 1
    '                        End Select
    '                        'glbmese_inizio = Month(dal) + 1
    '                    End If
    '                Else
    '                    If DateDiff("d", dal, data_ultimo_gg_mese) <= 14 Then     'modifica 8.1
    '                        'aggiunto = perchè in caso di un rif. cat
    '                        aggiunta_mese = -1
    '                        'secondario dal 16/11/1993 al ........ già di un secondo
    '                        'Sposto di uno il mese di inizio
    '                        'proprietario mi calcolava un mese per il primo
    '                        Select Case tipo_periodo
    '                            Case 1       'Periodo di possesso
    '                                glbmese_inizio_p = Month(dal) + 1
    '                            Case 2       'Periodo di classamento
    '                                glbmese_inizio_s = Month(dal) + 1
    '                        End Select
    '                        'glbmese_inizio = Month(dal) + 1
    '                    End If
    '                End If

    '                'Verifico data Al
    '                If al = "" Then
    '                    al = "31/12" + "/" + Trim(Str(annoAccertamento))
    '                Else
    '                    If Year(al) > annoAccertamento Then
    '                        al = "31/12" + "/" + Trim(Str(annoAccertamento))
    '                    End If
    '                End If
    '                'Verifico quanti gg di possosseso ci sono nel mese della data Al
    '                If Day(al) > 14 Then
    '                    aggiunta_mese = aggiunta_mese + 1
    '                    'Inposto il mese di fine nel caso non fosse Dicembre (12)
    '                    Select Case tipo_periodo
    '                        Case 1       'Periodo di possesso
    '                            glbmese_fine_p = Month(al)
    '                        Case 2       'Periodo di classamento
    '                            glbmese_fine_s = Month(al)
    '                    End Select
    '                    'glbmese_fine = Month(al)
    '                Else
    '                    'Sposto indietro di uno il mese di fine nel caso il mese in	esame
    '                    'non avesse i giorni sufficenti da essere considerato
    '                    Select Case tipo_periodo
    '                        Case 1       'Periodo di possesso
    '                            glbmese_fine_p = Month(al) - 1
    '                        Case 2       'Periodo di classamento
    '                            glbmese_fine_s = Month(al) - 1
    '                    End Select
    '                    'glbmese_fine = Month(al) - 1
    '                End If
    '                'Calcolo i mesi di possesso per l'anno in esame
    '                mesipossesso = DateDiff("M", dal, al) + aggiunta_mese
    '            Else
    '                mesipossesso = 0
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.grdAccertato.mesi_possesso.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub


    'Private Function FormatStringToEmpty(ByVal objInput As Object) As String

    '    Dim strOutput As String
    '    Try
    '        If (objInput Is Nothing) Then
    '            strOutput = ""
    '        ElseIf IsDBNull(objInput) Then
    '            strOutput = ""
    '        Else
    '            If CStr(objInput) = "" Or CStr(objInput) = "0" Or CStr(objInput) = "-1" Then
    '                strOutput = ""
    '            Else
    '                strOutput = objInput.ToString()
    '            End If

    '        End If
    '        Return strOutput
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.grdAccertato.FormatStringToEmpty.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Function

    'Protected Function FormattaNumero(ByVal NumeroDaFormattareParam As Object, ByVal numDec As Integer) As String
    '    Try
    '        If IsDBNull(NumeroDaFormattareParam) Then
    '            FormattaNumero = ""
    '        Else
    '            If NumeroDaFormattareParam.ToString() = "" Or NumeroDaFormattareParam.ToString() = "-1" Or NumeroDaFormattareParam.ToString() = "-1,00" Then
    '                FormattaNumero = ""
    '            Else
    '                FormattaNumero = FormatNumber(NumeroDaFormattareParam, numDec)
    '            End If

    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.grdAccertato.FormattaNumero.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Function

    'Public Function IntForGridView(ByVal iInput As Object) As String

    '    Dim ret As String = String.Empty
    '    Try
    '        If iInput.ToString() = "-1" Or iInput.ToString() = "-1,00" Then
    '            ret = String.Empty
    '        Else
    '            ret = Convert.ToString(iInput)
    '        End If

    '        IntForGridView = ret
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.grdAccertato.IntForGridView.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Function

    'Private Sub btnAssociaAccertato_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAssociaAccertato.Click
    '    Dim dw As DataView
    '    Try
    '        If Not Session("Immobili_accertati") Is Nothing Then
    '            dw = Session("Immobili_accertati")
    '            GrdAccertato.DataSource = dw
    '            GrdAccertato.DataBind()
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.grdAccertato.btnAssociaAccertato_Click.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Private Function addRowsObjAnagrafica(ByVal Anagrafica As DettaglioAnagrafica) As DataSet
    '    Dim row As DataRow
    '    Dim newTable As DataTable
    '    Dim objAnagrafica As DataSet = Nothing
    '    Try
    '        objAnagrafica = CreateDsPerAnagrafica()
    '        newTable = objAnagrafica.Tables(0).Copy

    '        row = newTable.NewRow()
    '        'row.Item("cod_contribuente") = Anagrafica.COD_CONTRIBUENTE

    '        row.Item("COD_CONTRIBUENTE") = Anagrafica.COD_CONTRIBUENTE
    '        row.Item("ID_DATA_ANAGRAFICA") = Anagrafica.ID_DATA_ANAGRAFICA
    '        row.Item("Cognome") = Anagrafica.Cognome
    '        row.Item("RappresentanteLegale") = Anagrafica.RappresentanteLegale
    '        row.Item("Nome") = Anagrafica.Nome
    '        row.Item("CodiceFiscale") = Anagrafica.CodiceFiscale
    '        row.Item("PartitaIva") = Anagrafica.PartitaIva
    '        row.Item("CodiceComuneNascita") = Anagrafica.CodiceComuneNascita
    '        row.Item("ComuneNascita") = Anagrafica.ComuneNascita
    '        row.Item("ProvinciaNascita") = Anagrafica.ProvinciaNascita
    '        row.Item("DataNascita") = Anagrafica.DataNascita
    '        row.Item("NazionalitaNascita") = Anagrafica.NazionalitaNascita
    '        row.Item("Sesso") = Anagrafica.Sesso
    '        row.Item("CodiceComuneResidenza") = Anagrafica.CodiceComuneResidenza
    '        row.Item("ComuneResidenza") = Anagrafica.ComuneResidenza
    '        row.Item("ProvinciaResidenza") = Anagrafica.ProvinciaResidenza
    '        row.Item("CapResidenza") = Anagrafica.CapResidenza
    '        row.Item("CodViaResidenza") = Anagrafica.CodViaResidenza
    '        row.Item("ViaResidenza") = Anagrafica.ViaResidenza
    '        row.Item("PosizioneCivicoResidenza") = Anagrafica.PosizioneCivicoResidenza
    '        row.Item("CivicoResidenza") = Anagrafica.CivicoResidenza
    '        row.Item("EsponenteCivicoResidenza") = Anagrafica.EsponenteCivicoResidenza
    '        row.Item("ScalaCivicoResidenza") = Anagrafica.ScalaCivicoResidenza
    '        row.Item("InternoCivicoResidenza") = Anagrafica.InternoCivicoResidenza
    '        row.Item("FrazioneResidenza") = Anagrafica.FrazioneResidenza
    '        row.Item("NazionalitaResidenza") = Anagrafica.NazionalitaResidenza
    '        row.Item("NucleoFamiliare") = Anagrafica.NucleoFamiliare
    '        row.Item("DATA_MORTE") = Anagrafica.DataMorte
    '        row.Item("Professione") = Anagrafica.Professione
    '        row.Item("Note") = Anagrafica.Note
    '        row.Item("DaRicontrollare") = Anagrafica.DaRicontrollare
    '        row.Item("DataInizioValidita") = Anagrafica.DataInizioValidita
    '        row.Item("DataFineValidita") = Anagrafica.DataFineValidita
    '        row.Item("DataUltimaModifica") = Anagrafica.DataUltimaModifica
    '        row.Item("Operatore") = Anagrafica.Operatore
    '        row.Item("CodContribuenteRappLegale") = Anagrafica.CodContribuenteRappLegale
    '        row.Item("CodEnte") = Anagrafica.CodEnte
    '        row.Item("CodIndividuale") = Anagrafica.CodIndividuale
    '        row.Item("CodFamiglia") = Anagrafica.CodFamiglia
    '        row.Item("NCTributari") = Anagrafica.NCTributari
    '        row.Item("DataUltimoAggTributi") = Anagrafica.DataUltimoAggTributi
    '        row.Item("NCAnagraficaRes") = Anagrafica.NCAnagraficaRes
    '        row.Item("DataUltimoAggAnagrafe") = Anagrafica.DataUltimoAggAnagrafe
    '        row.Item("TipoRiferimento") = Anagrafica.TipoRiferimento
    '        row.Item("DatiRiferimento") = Anagrafica.DatiRiferimento
    '        '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
    '        'row.Item("ID_DATA_SPEDIZIONE") = Anagrafica.ID_DATA_SPEDIZIONE
    '        'row.Item("CodTributo") = Anagrafica.CodTributo
    '        'row.Item("CognomeInvio") = Anagrafica.CognomeInvio
    '        'row.Item("NomeInvio") = Anagrafica.NomeInvio
    '        'row.Item("CodComuneRCP") = Anagrafica.CodComuneRCP
    '        'row.Item("ComuneRCP") = Anagrafica.ComuneRCP
    '        'row.Item("LocRCP") = Anagrafica.LocRCP
    '        'row.Item("ProvinciaRCP") = Anagrafica.ProvinciaRCP
    '        'row.Item("CapRCP") = Anagrafica.CapRCP
    '        'row.Item("CodViaRCP") = Anagrafica.CodViaRCP
    '        'row.Item("ViaRCP") = Anagrafica.ViaRCP
    '        'row.Item("PosizioneCivicoRCP") = Anagrafica.PosizioneCivicoRCP
    '        'row.Item("CivicoRCP") = Anagrafica.CivicoRCP
    '        'row.Item("EsponenteCivicoRCP") = Anagrafica.EsponenteCivicoRCP
    '        'row.Item("ScalaCivicoRCP") = Anagrafica.ScalaCivicoRCP
    '        'row.Item("InternoCivicoRCP") = Anagrafica.InternoCivicoRCP
    '        'row.Item("FrazioneRCP") = Anagrafica.FrazioneRCP
    '        'row.Item("DataInizioValiditaSpedizione") = Anagrafica.DataInizioValiditaSpedizione
    '        'row.Item("DataFineValiditaSpedizione") = Anagrafica.DataFineValiditaSpedizione
    '        'row.Item("DataUltimaModificaSpedizione") = Anagrafica.DataUltimaModificaSpedizione
    '        'row.Item("OperatoreSpedizione") = Anagrafica.OperatoreSpedizione
    '        For Each MySped As ObjIndirizziSpedizione In Anagrafica.ListSpedizioni
    '            If MySped.CodTributo = Utility.Costanti.TRIBUTO_ICI Then
    '                row.Item("ID_DATA_SPEDIZIONE") = MySped.ID_DATA_SPEDIZIONE
    '                row.Item("CodTributo") = MySped.CodTributo
    '                row.Item("CognomeInvio") = MySped.CognomeInvio
    '                row.Item("NomeInvio") = MySped.NomeInvio
    '                row.Item("CodComuneRCP") = MySped.CodComuneRCP
    '                row.Item("ComuneRCP") = MySped.ComuneRCP
    '                row.Item("LocRCP") = MySped.LocRCP
    '                row.Item("ProvinciaRCP") = MySped.ProvinciaRCP
    '                row.Item("CapRCP") = MySped.CapRCP
    '                row.Item("CodViaRCP") = MySped.CodViaRCP
    '                row.Item("ViaRCP") = MySped.ViaRCP
    '                row.Item("PosizioneCivicoRCP") = MySped.PosizioneCivicoRCP
    '                row.Item("CivicoRCP") = MySped.CivicoRCP
    '                row.Item("EsponenteCivicoRCP") = MySped.EsponenteCivicoRCP
    '                row.Item("ScalaCivicoRCP") = MySped.ScalaCivicoRCP
    '                row.Item("InternoCivicoRCP") = MySped.InternoCivicoRCP
    '                row.Item("FrazioneRCP") = MySped.FrazioneRCP
    '                row.Item("DataInizioValiditaSpedizione") = MySped.DataInizioValiditaSpedizione
    '                row.Item("DataFineValiditaSpedizione") = MySped.DataFineValiditaSpedizione
    '                row.Item("DataUltimaModificaSpedizione") = MySped.DataUltimaModificaSpedizione
    '                row.Item("OperatoreSpedizione") = MySped.OperatoreSpedizione
    '            End If
    '        Next
    '        '*** ***
    '        newTable.Rows.Add(row)
    '        newTable.AcceptChanges()

    '        objAnagrafica.Tables(0).ImportRow(row)
    '        objAnagrafica.Tables(0).AcceptChanges()


    '        addRowsObjAnagrafica = objAnagrafica

    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.grdAccertato.addRowsObjAnagrafica.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try

    'End Function

    'Private Function CreateDsPerAnagrafica() As DataSet
    '    Dim objDS As New DataSet

    '    Dim newTable As DataTable
    '    newTable = New DataTable("ANAGRAFICA")

    '    Dim NewColumn As New DataColumn
    '    Try
    '        NewColumn.ColumnName = "COD_CONTRIBUENTE"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ID_DATA_SPEDIZIONE"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ID_DATA_ANAGRAFICA"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "Cognome"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "RappresentanteLegale"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "Nome"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "CodiceFiscale"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "INDIRIZZO"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "PartitaIva"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "CodiceComuneNascita"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ComuneNascita"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ProvinciaNascita"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DataNascita"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "NazionalitaNascita"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "Sesso"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "CodiceComuneResidenza"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ComuneResidenza"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ProvinciaResidenza"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "CapResidenza"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "CodViaResidenza"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ViaResidenza"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "PosizioneCivicoResidenza"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "CivicoResidenza"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "EsponenteCivicoResidenza"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ScalaCivicoResidenza"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "InternoCivicoResidenza"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "FrazioneResidenza"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "NazionalitaResidenza"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "NucleoFamiliare"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DATA_MORTE"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "Professione"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "Note"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DaRicontrollare"
    '        NewColumn.DataType = System.Type.GetType("System.Boolean")
    '        NewColumn.DefaultValue = False
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DataInizioValidita"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DataFineValidita"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DataUltimaModifica"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "Operatore"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "CodContribuenteRappLegale"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "CodEnte"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "CodIndividuale"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "CodFamiglia"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "NCTributari"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DataUltimoAggTributi"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "NCAnagraficaRes"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DataUltimoAggAnagrafe"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "CodTributo"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "CognomeInvio"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "NomeInvio"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "CodComuneRCP"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ComuneRCP"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "LocRCP"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ProvinciaRCP"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "CapRCP"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "CodViaRCP"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ViaRCP"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "PosizioneCivicoRCP"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "CivicoRCP"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "EsponenteCivicoRCP"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ScalaCivicoRCP"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "InternoCivicoRCP"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "FrazioneRCP"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DataInizioValiditaSpedizione"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DataFineValiditaSpedizione"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DataUltimaModificaSpedizione"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "OperatoreSpedizione"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "TipoRiferimento"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DatiRiferimento"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)


    '        objDS.Tables.Add(newTable)

    '        Return objDS
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.grdAccertato.CreateDsPerAnagrafica.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Function

    'Protected Function checkMesiRiduzione(ByVal objtemp As Object) As String
    '    Try
    '        If Not IsDBNull(objtemp) Then
    '            If Int(objtemp) > 0 Then
    '                Return "True"
    '            Else
    '                Return "False"
    '            End If
    '        Else
    '            Return "False"
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.grdAccertato.checkMesiRiduzione.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Function
    'Private Function CalcoloImpSanzIntSingleUI(objHashTable As Hashtable, objDSDichiarazioni As DataSet, TipoAvviso As Integer, ByRef objDSCalcoloSanzioniInteressi As DataSet, ByRef objDSSanzioni As DataSet, ByRef objDSInteressi As DataSet, ByRef objDTAccertato As DataTable, ByRef TotDiffImpostaACCERTAMENTO As Double, ByRef TotImpICIDichiarato As Double, ByRef TotDiffImpostaDICHIARATO As Double, ByRef TotImpICIACCERTAMENTO As Double, ByRef TotInteressiACCERTAMENTO As Double, ByRef TotSanzioniACCERTAMENTO As Double, ByRef TotImportoSanzioniRidottoACCERTAMENTO As Double, ByRef TotVersamenti As Double) As Boolean
    '    Dim blnResult As Boolean
    '    Dim blnCalcolaInteressi As Boolean
    '    Dim objDSCalcoloSanzioniInteressiAppoggio As DataSet
    '    Dim TotDiffImposta As Double
    '    Dim TotDiffImpostaACCONTO As Double
    '    Dim TotDiffImpostaSALDO As Double
    '    Dim TotDiffImpostaAccontoDICHIARATO As Double 'Importo Totale Differenza di imposta ACCONTO immobili dichiarati
    '    Dim TotDiffImpostaSaldoDICHIARATO As Double 'Importo Totale Differenza di imposta SALDO immobili dichiarati
    '    Dim TotDiffImpostaAccontoACCERTAMENTO As Double 'Importo Totale Differenza di imposta atto ACCONTO di accertamento
    '    Dim TotDiffImpostaSaldoACCERTAMENTO As Double 'Importo Totale Differenza di imposta SALDO atto di accertamento
    '    Dim TotImpostaICIDichiaratoIMMOBILE As Double = 0
    '    Dim TotImpostaICIDichiaratoACCONTOIMMOBILE As Double = 0
    '    Dim TotImpostaICIDichiaratoSALDOIMMOBILE As Double = 0
    '    Dim TotImpostaICIAccertato As Double = 0
    '    Dim TotImpostaICIAccertatoACCONTO As Double = 0
    '    Dim TotImpostaICIAccertatoSALDO As Double = 0
    '    Dim ImportoSanzioneImmobile As Double = 0 'sanzioni singolo Immobile
    '    Dim ImportoSanzioneImmobileRidotto As Double = 0 'sanzioni singolo Immobile
    '    Dim ImportoInteresseImmobile As Double = 0 'interessi singolo Immobile
    '    'Dim DiffTotaleSanzioni As Double     'Somma algebrica fra sanzioni e interessi x det. tipo avviso
    '    'Dim DiffTotaleInteressi As Double     'Somma algebrica fra sanzioni e interessi x det. tipo avviso
    '    'Dim DiffTotaleSanzInt As Double     'Somma algebrica fra sanzioni e interessi x det. tipo avviso
    '    Dim ssanz As String
    '    Dim nElementiArrayAccert As Integer
    '    Dim nElementiArrayInteressi As Integer
    '    Dim nElementiArraySanzioni As Integer
    '    Dim intCount As Integer
    '    Dim i As Integer
    '    Dim y As Integer
    '    Dim maxLeg As Integer
    '    Dim copyRow As DataRow
    '    Dim objDSMotivazioniSanzioni As DataSet
    '    Dim dt As DataTable
    '    Dim dtInteressi As New DataTable
    '    Dim dtSanzioni As New DataTable
    '    Dim rowsArray(), rowsArraySanz() As DataRow
    '    Dim TipoAvvisoRimborso As Integer

    '    Try
    '        maxLeg = maxLegame("idLegame")

    '        For i = 1 To maxLeg
    '            TotDiffImposta = 0
    '            TotImpostaICIAccertato = 0
    '            TotImpostaICIAccertatoACCONTO = 0
    '            TotImpostaICIAccertatoSALDO = 0
    '            TotImpostaICIDichiaratoIMMOBILE = 0
    '            TotImpostaICIDichiaratoACCONTOIMMOBILE = 0
    '            TotImpostaICIDichiaratoSALDOIMMOBILE = 0

    '            ImportoSanzioneImmobile = 0 'sanzioni singolo Immobile
    '            ImportoSanzioneImmobileRidotto = 0 'sanzioni ridotte singolo Immobile
    '            ImportoInteresseImmobile = 0 'interessi singolo Immobile

    '            '*******************************************************************
    '            'Calcolo ICI Totale per dichiarato per ogni gruppo di legame
    '            'Se non ho dichiarato niente posso andare comunque in accertamento
    '            '*******************************************************************
    '            If objDSDichiarazioni.Tables(0).DefaultView.Count > 0 Then
    '                rowsArray = objDSDichiarazioni.Tables(0).Select("IDLEGAME='" & i & "'")
    '                If rowsArray.Length > 0 Then
    '                    'Calcolo l'ICI dichiarato per gruppo
    '                    For y = 0 To rowsArray.Length - 1
    '                        TotImpostaICIDichiaratoIMMOBILE = rowsArray(y).Item("ICICALCOLATO") + TotImpostaICIDichiaratoIMMOBILE
    '                        TotImpostaICIDichiaratoACCONTOIMMOBILE = rowsArray(y).Item("ICICalcolatoACCONTO") + TotImpostaICIDichiaratoACCONTOIMMOBILE
    '                        TotImpostaICIDichiaratoSALDOIMMOBILE = rowsArray(y).Item("ICICalcolatoSALDO") + TotImpostaICIDichiaratoSALDOIMMOBILE
    '                    Next
    '                End If
    '            End If
    '            TotImpICIDichiarato += TotImpostaICIDichiaratoIMMOBILE
    '            TotDiffImpostaDICHIARATO += TotImpostaICIDichiaratoIMMOBILE
    '            TotDiffImpostaAccontoDICHIARATO = TotDiffImpostaAccontoDICHIARATO + TotImpostaICIDichiaratoACCONTOIMMOBILE
    '            TotDiffImpostaSaldoDICHIARATO = TotDiffImpostaSaldoDICHIARATO + TotImpostaICIDichiaratoSALDOIMMOBILE
    '            '*******************************************************************

    '            '*******************************************************************
    '            'Calcolo ICI Totale per accertato per ogni legame
    '            '*******************************************************************
    '            rowsArray = objDTAccertato.Select("IDLEGAME='" & i & "'")

    '            'prelevo dall'array di immobili accertati l'informazione sul fatto
    '            'di calcolare o no gli INTERESSI
    '            'Calcolo Interessi su singolo Immobile
    '            If CBool(rowsArray(0)("CALCOLA_INTERESSI")) = False Then
    '                blnCalcolaInteressi = False
    '            Else
    '                blnCalcolaInteressi = True
    '            End If
    '            'blnCalcolaInteressi = rowsArray(0)("CALCOLA_INTERESSI")

    '            If rowsArray.Length > 0 Then
    '                'Calcolo l'ICI dichiarato per gruppo
    '                For y = 0 To rowsArray.Length - 1
    '                    Dim iciCalc As Double = 0
    '                    Dim iciCalcAcc As Double = 0
    '                    Dim iciCalcSald As Double = 0
    '                    If rowsArray(y).Item("ICICALCOLATO").ToString().CompareTo("") <> 0 Then
    '                        iciCalc = rowsArray(y).Item("ICICALCOLATO")
    '                    End If
    '                    If rowsArray(y).Item("ICICALCOLATOACCONTO").ToString().CompareTo("") <> 0 Then
    '                        iciCalcAcc = rowsArray(y).Item("ICICALCOLATOACCONTO")
    '                    End If
    '                    If rowsArray(y).Item("ICICALCOLATOSALDO").ToString().CompareTo("") <> 0 Then
    '                        iciCalcSald = rowsArray(y).Item("ICICALCOLATOSALDO")
    '                    End If
    '                    TotImpostaICIAccertato = iciCalc + TotImpostaICIAccertato
    '                    TotImpostaICIAccertatoACCONTO = iciCalcAcc + TotImpostaICIAccertatoACCONTO
    '                    TotImpostaICIAccertatoSALDO = iciCalcSald + TotImpostaICIAccertatoSALDO
    '                Next
    '            End If
    '            '*******************************************************************
    '            'Totale imposta accertato per riepilogo
    '            TotImpICIACCERTAMENTO += TotImpostaICIAccertato
    '            '*******************************************************************
    '            'Calcolo il Totale della differenza di imposta
    '            '*******************************************************************
    '            TotDiffImposta = TotImpostaICIAccertato - TotImpostaICIDichiaratoIMMOBILE
    '            TotDiffImpostaACCONTO = TotImpostaICIAccertatoACCONTO - TotImpostaICIDichiaratoACCONTOIMMOBILE
    '            TotDiffImpostaSALDO = TotImpostaICIAccertatoSALDO - TotImpostaICIDichiaratoSALDOIMMOBILE

    '            TotDiffImpostaACCERTAMENTO += TotDiffImposta
    '            TotDiffImpostaAccontoACCERTAMENTO = TotDiffImpostaAccontoACCERTAMENTO + TotDiffImpostaACCONTO
    '            TotDiffImpostaSaldoACCERTAMENTO = TotDiffImpostaSaldoACCERTAMENTO + TotDiffImpostaSALDO
    '            '*******************************************************************
    '            'Fine calcolo differenza di imposta
    '            '*******************************************************************

    '            'Aggiorno il objDSDichiarazioni con l'importo delle sanzioni calcolate-------------
    '            For nElementiArrayAccert = 0 To rowsArray.Length - 1
    '                rowsArray(nElementiArrayAccert).Item("diffimposta") = TotDiffImposta
    '                rowsArray(nElementiArrayAccert).AcceptChanges()

    '                objDTAccertato.AcceptChanges()
    '            Next
    '            '----------------------------------------------------------------------------------
    '            TipoAvvisoRimborso = -1
    '            '*******************************************************************
    '            'Rimborso. Calcolo gli interessi Attivi sul singolo immobile.
    '            'Al giro dopo la var viene azzerrata a -1.
    '            '*******************************************************************
    '            If TotDiffImposta < 0 Then
    '                TipoAvvisoRimborso = oggettoatto.provvedimento.RIMBORSO '"5"
    '            End If
    '            ''*******************************************************************
    '            'HashTable per calcolo Sanzioni e interessi
    '            '*******************************************************************
    '            'objSessione = New CreateSessione(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))
    '            'If Not objSessione.CreaSessione(ConstSession.UserName, strWFErrore) Then
    '            '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '            'End If
    '            objHashTable = Session("HashTableDichiarazioniAccertamenti")
    '            objHashTable("IDSOTTOAPPLICAZIONEANAGRAFICA") = ConfigurationManager.AppSettings("OPENGOVA")
    '            'strConnectionStringAnagrafica = objSessione.oSession.GetPrivateDBManager(objHashTable("IDSOTTOAPPLICAZIONEANAGRAFICA")).GetConnection.ConnectionString
    '            'objHashTable("CONNECTIONSTRINGANAGRAFICA") = strConnectionStringAnagrafica
    '            objHashTable("CONNECTIONSTRINGANAGRAFICA") = ConstSession.StringConnectionAnagrafica 'ConfigurationManager.AppSettings("connectionStringSQLOPENAnagrafica")
    '            objHashTable("CODTRIBUTO") = Utility.Costanti.TRIBUTO_ICI
    '            If objHashTable.ContainsKey("TIPOPROVVEDIMENTO") = True Then
    '                objHashTable.Remove("TIPOPROVVEDIMENTO")
    '            End If
    '            'TipoAvvisoRimborso = -1 == False ----> Non è un rimborso
    '            'Devo effettuare il calcolo guardando se è un rimborso sull'immobile oppure no
    '            'Se è rimborso ho solo interessi attivi
    '            If TipoAvvisoRimborso = -1 Then
    '                objHashTable.Add("TIPOPROVVEDIMENTO", TipoAvviso)
    '            Else
    '                objHashTable.Add("TIPOPROVVEDIMENTO", TipoAvvisoRimborso)
    '            End If
    '            If objHashTable.ContainsKey("COD_TIPO_PROCEDIMENTO") = True Then
    '                objHashTable("COD_TIPO_PROCEDIMENTO") = oggettoatto.procedimento.ACCERTAMENTO
    '            Else
    '                objHashTable.Add("COD_TIPO_PROCEDIMENTO", oggettoatto.procedimento.ACCERTAMENTO)
    '            End If
    '            If objHashTable.Contains("ANNOACCERTAMENTO") = False Then
    '                objHashTable.Add("ANNOACCERTAMENTO", CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("ANNOACCERTAMENTO"))
    '            End If
    '            '*******************************************************************
    '            'Fine HashTable
    '            '*******************************************************************

    '            '*******************************************************************
    '            'Calcolo Sanzioni e Interessi su singolo Immobile (la procedura di calcolo
    '            'delle sanzioni le calcola solo	se l'importo è positivo)
    '            '*******************************************************************
    '            If TotDiffImposta <> 0 Or TotDiffImposta = 0 Then
    '                blnResult = False
    '                Dim objHashTableDati As New Hashtable
    '                objHashTableDati.Add("IDSANZIONI", rowsArray(0).Item("IDSANZIONI"))
    '                'L'Id Immobile è il Progressivo
    '                objHashTableDati.Add("IDIMMOBILE", rowsArray(0).Item("PROGRESSIVO"))
    '                objHashTableDati.Add("IDLEGAME", rowsArray(0).Item("IDLEGAME"))
    '                '******************************************************************
    '                'Calcolo le sanzioni per i singoli Immobili
    '                '******************************************************************
    '                objDSCalcoloSanzioniInteressi = CreateDatasetPerSanzInt(objHashTable("ANNOACCERTAMENTO"), objHashTable("CODCONTRIBUENTE"), TotDiffImposta, TotDiffImpostaACCONTO, TotDiffImpostaSALDO)
    '                objDSCalcoloSanzioniInteressiAppoggio = SetObjDSAppoggioSanzioni(objHashTable("ANNOACCERTAMENTO"), TotDiffImposta, TotVersamenti)

    '                'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - GetSanzioni")
    '                Dim objCOMDichiarazioniAccertamenti As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
    '                objDSSanzioni = objCOMDichiarazioniAccertamenti.getSanzioni(objHashTable, objHashTableDati, objDSCalcoloSanzioniInteressi, objDSCalcoloSanzioniInteressiAppoggio, False)
    '                'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - Terminato GetSanzioni")

    '                'Creo una copia della struttura
    '                If dtSanzioni.Rows.Count = 0 And objDSSanzioni.Tables.Count > 0 Then
    '                    dtSanzioni = objDSSanzioni.Tables("SANZIONI").Copy
    '                    dtSanzioni.Clear()
    '                End If
    '                For intCount = 0 To objDSSanzioni.Tables("SANZIONI").Rows.Count - 1
    '                    objDSSanzioni.Tables("SANZIONI").Rows(intCount)("IMPORTO_GIORNI") = 0
    '                    copyRow = objDSSanzioni.Tables("SANZIONI").Rows(intCount)
    '                    copyRow("motivazioni") = rowsArray(0)("desc_sanzione")
    '                    dtSanzioni.ImportRow(copyRow)
    '                Next
    '                objDSMotivazioniSanzioni = Session("DataSetSanzioni")

    '                If Not objDSMotivazioniSanzioni Is Nothing Then
    '                    For intCount = 0 To objDSMotivazioniSanzioni.Tables(0).Rows.Count - 1
    '                        Dim rows() As DataRow
    '                        rows = dtSanzioni.Select("ID_LEGAME='" & i & "'")
    '                        rowsArraySanz = objDTAccertato.Select("IDLEGAME='" & i & "'")
    '                        If rowsArraySanz.Length > 0 Then
    '                            For y = 0 To rowsArraySanz.Length - 1
    '                                If rowsArraySanz(y).Item("DESC_SANZIONE").ToString().CompareTo("") <> 0 Then
    '                                    ssanz = rowsArraySanz(y).Item("DESC_SANZIONE")
    '                                End If
    '                            Next
    '                        End If
    '                        For y = 0 To UBound(rows)
    '                            rows(y).Item("Motivazioni") = ssanz
    '                        Next
    '                        dtSanzioni.AcceptChanges()
    '                    Next
    '                End If

    '                'Aggiorno il DataSet con le sanzioni

    '                dt = objDSSanzioni.Tables(1)
    '                objDSCalcoloSanzioniInteressi.Dispose()
    '                objDSCalcoloSanzioniInteressi = New DataSet
    '                objDSCalcoloSanzioniInteressi.Tables.Add(dt.Copy)

    '                'Aggiorno il DS con l'importo delle sanzioni calcolate
    '                rowsArray = objDTAccertato.Select("IDLEGAME='" & i & "'")

    '                For nElementiArraySanzioni = 0 To rowsArray.Length - 1
    '                    'Log.Debug("objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item(IMPORTO_SANZIONI)::" & objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI").ToString)
    '                    'Log.Debug("objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item(IMPORTO_SANZIONI_RIDOTTO)::" & objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI_RIDOTTO").ToString)
    '                    rowsArray(nElementiArraySanzioni).Item("SANZIONI") = objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI").ToString.Replace(".", ",")
    '                    rowsArray(nElementiArraySanzioni).Item("SANZIONI_RIDOTTO") = objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI_RIDOTTO").ToString.Replace(".", ",")

    '                    rowsArray(nElementiArraySanzioni).AcceptChanges()
    '                    ImportoSanzioneImmobile = ImportoSanzioneImmobile + CDbl(objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI").ToString.Replace(".", ","))
    '                    ImportoSanzioneImmobileRidotto = ImportoSanzioneImmobileRidotto + CDbl(objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI_RIDOTTO").ToString.Replace(".", ","))
    '                    'Log.Debug("ImportoSanzioneImmobile::" & ImportoSanzioneImmobile.ToString)
    '                    'Log.Debug("ImportoSanzioneImmobileRidotto::" & ImportoSanzioneImmobileRidotto.ToString)
    '                    objDTAccertato.AcceptChanges()
    '                Next

    '                'totale sanzione dell'atto di accertamento
    '                TotSanzioniACCERTAMENTO += ImportoSanzioneImmobile
    '                TotImportoSanzioniRidottoACCERTAMENTO += ImportoSanzioneImmobileRidotto

    '                'Somma algebrica per determinare Tipo Avviso
    '                'DiffTotaleSanzioni = ImportoSanzioneImmobile 'objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI")
    '                'DiffTotaleSanzInt = DiffTotaleSanzInt + DiffTotaleSanzioni


    '                '******************************************************************
    '                'CALCOLO INTERESSI
    '                If blnCalcolaInteressi = True Then
    '                    '''objDSCalcoloSanzioni = PrepareVersamentiPerSanzInt(objHashTable("ANNOACCERTAMENTO"), objHashTable("CODCONTRIBUENTE"), TotDiffImposta)
    '                    objCOMDichiarazioniAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
    '                    objDSInteressi = objCOMDichiarazioniAccertamenti.getInteressi(objHashTable, objHashTableDati, objDSCalcoloSanzioniInteressi, rowsArray(0).Item("PROGRESSIVO"), rowsArray(0).Item("IDLEGAME"))
    '                    If Not IsNothing(objDSInteressi.Tables("INTERESSI")) Then
    '                        If dtInteressi.Rows.Count = 0 And objDSInteressi.Tables.Count > 0 Then
    '                            dtInteressi = objDSInteressi.Tables("INTERESSI").Copy
    '                            dtInteressi.Clear()
    '                        End If
    '                        For intCount = 0 To objDSInteressi.Tables("INTERESSI").Rows.Count - 1
    '                            copyRow = objDSInteressi.Tables("INTERESSI").Rows(intCount)
    '                            dtInteressi.ImportRow(copyRow)
    '                        Next

    '                        'Aggiorno il DataSet con gli interessi
    '                        dt = objDSInteressi.Tables(1)
    '                        objDSCalcoloSanzioniInteressi.Dispose()
    '                        objDSCalcoloSanzioniInteressi = New DataSet
    '                        objDSCalcoloSanzioniInteressi.Tables.Add(dt.Copy)

    '                        'Aggiorno il DS con l'importo delle interessi calcolati
    '                        rowsArray = objDTAccertato.Select("IDLEGAME='" & i & "'")
    '                        For nElementiArrayInteressi = 0 To rowsArray.Length - 1
    '                            rowsArray(nElementiArrayInteressi).Item("INTERESSI") = objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_INTERESSI")
    '                            rowsArray(nElementiArrayInteressi).AcceptChanges()
    '                            objDTAccertato.AcceptChanges()
    '                            ImportoInteresseImmobile = ImportoInteresseImmobile + CDbl(objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_INTERESSI"))
    '                        Next
    '                    End If

    '                    'totale interessi dell'atto di accertamento
    '                    TotInteressiACCERTAMENTO += ImportoInteresseImmobile

    '                    'Somma algebrica per determinare se Rimborso o Avviso di accertamento
    '                    'DiffTotaleInteressi = objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_INTERESSI")
    '                    'DiffTotaleSanzInt = DiffTotaleSanzInt + DiffTotaleInteressi
    '                Else
    '                    'Aggiorno il DataSet con gli interessi
    '                    objDSCalcoloSanzioniInteressi = New DataSet
    '                    objDSCalcoloSanzioniInteressi.Tables.Add(dt.Copy)

    '                    For nElementiArrayInteressi = 0 To rowsArray.Length - 1
    '                        rowsArray(nElementiArrayInteressi).Item("INTERESSI") = 0
    '                        rowsArray(nElementiArrayInteressi).AcceptChanges()
    '                        objDTAccertato.AcceptChanges()
    '                        ImportoInteresseImmobile = ImportoInteresseImmobile + 0
    '                    Next

    '                    'totale interessi dell'atto di accertamento
    '                    TotInteressiACCERTAMENTO += ImportoInteresseImmobile

    '                    'Somma algebrica per determinare se Rimborso o Avviso di accertamento
    '                    'DiffTotaleInteressi = 0
    '                    'DiffTotaleSanzInt = DiffTotaleSanzInt + DiffTotaleInteressi
    '                End If
    '            Else
    '                '*******************************************************************
    '                'Se sono qui l'ici dichiarato è uguale a quello accertato
    '                'Sanzioni e Interessi a 0
    '                '*******************************************************************
    '                rowsArray = objDTAccertato.Select("IDLEGAME='" & i & "'")
    '                rowsArray(0).Item("SANZIONI") = "0"
    '                rowsArray(0).Item("INTERESSI") = 0
    '                rowsArray(0).AcceptChanges()
    '                objDTAccertato.AcceptChanges()
    '            End If

    '            'valorizzo totale imposta per il singolo immobile
    '            rowsArray = objDTAccertato.Select("IDLEGAME='" & i & "'")
    '            rowsArray(0).Item("totale") = CDbl(rowsArray(0).Item("diffimposta")) + CDbl(rowsArray(0).Item("SANZIONI")) + CDbl(rowsArray(0).Item("INTERESSI"))
    '            rowsArray(0).AcceptChanges()
    '            objDTAccertato.AcceptChanges()

    '            'l'avviso è dato dalla somma di tutte le voci di ogni singolo immobile accertato
    '            'differenza di imposta, sanzioni, interessi
    '        Next

    '        If Not objDSSanzioni Is Nothing Then
    '            objDSSanzioni.Dispose()
    '        End If
    '        If Not objDSInteressi Is Nothing Then
    '            objDSInteressi.Dispose()
    '        End If

    '        objDSSanzioni = New DataSet
    '        objDSInteressi = New DataSet

    '        objDSSanzioni.Tables.Add(dtSanzioni.Copy)
    '        objDSInteressi.Tables.Add(dtInteressi.Copy)
    '        Log.Debug("TotSanzioni=" + TotSanzioniACCERTAMENTO.ToString() + ", TotInteressi=" + TotInteressiACCERTAMENTO.ToString())
    '        Return True
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.grdAccertato.CalcoloImpSanzIntSingleUI.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '        Return False
    '    End Try
    'End Function
    'Private Function GetTipoAvviso(IsPrimoGiro As Boolean, objDTAccertato As DataTable, objDSDichiarazioni As DataSet, ImportoTotaleAvviso As Double, dblImportoTotaleF2 As Double, ByRef iTIPOPROVV_PREACC As Integer, ByRef TipoAvviso As Integer) As Boolean
    '    Dim i As Integer
    '    Dim Trovato As Boolean = False

    '    Try
    '        Dim blnTIPO_OPERAZIONE_RETTIFICA As Boolean = CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("TIPO_OPERAZIONE_RETTIFICA")
    '        If IsPrimoGiro Then
    '            TipoAvviso = oggettoatto.provvedimento.accertamentoRETTIFICA   '4
    '            'Prendo immobili di accertato
    '            For i = 0 To objDTAccertato.DefaultView.Count - 1
    '                Trovato = False
    '                Dim y As Integer
    '                'Cerco l'immobili in tutti gli immobili di dichiarato'
    '                'Se lo trovo esco dal ciclo e proseguo a cercare con immobili
    '                'successivo di accertamento
    '                For y = 0 To objDSDichiarazioni.Tables(0).DefaultView.Count - 1
    '                    If objDTAccertato.Rows(i).Item("idLegame") = objDSDichiarazioni.Tables(0).Rows(y).Item("idLegame") Then
    '                        Trovato = True
    '                        Exit For
    '                    End If
    '                Next
    '                'Se trovato = False vuol dire che non ho trovato l'immobile
    '                If Trovato = False Then
    '                    'Avviso D'ufficio
    '                    TipoAvviso = oggettoatto.provvedimento.accertamentoUFFICIO    '3
    '                    Exit For
    '                End If
    '            Next
    '        Else
    '            If ImportoTotaleAvviso > 0 Or dblImportoTotaleF2 > 0 Then
    '                If blnTIPO_OPERAZIONE_RETTIFICA Then
    '                    If ((ImportoTotaleAvviso + dblImportoTotaleF2) >= -2 And (ImportoTotaleAvviso + dblImportoTotaleF2) <= 2) Then
    '                        TipoAvviso = oggettoatto.provvedimento.autotutelaANNULLAMENTO
    '                    Else
    '                        TipoAvviso = oggettoatto.provvedimento.autotutelaRETTIFICA
    '                    End If
    '                Else
    '                    'AVVISO
    '                    'per determinare il tipo di avviso si prende "lo stato" più grave tra lo stato del pre accertamento e lo stato dell'accertamento
    '                    If (iTIPOPROVV_PREACC = oggettoatto.provvedimento.accertamentoUFFICIO) Or (TipoAvviso = oggettoatto.provvedimento.accertamentoUFFICIO) Then
    '                        'se PREACCERTAMENTO o ACCERTAMENTO hanno scaturito un avviso di accertamento d'ufficio
    '                        TipoAvviso = oggettoatto.provvedimento.accertamentoUFFICIO 'AVVISO DI ACCERTAMENTO D'UFFICIO                                    
    '                    ElseIf (iTIPOPROVV_PREACC = oggettoatto.provvedimento.accertamentoRETTIFICA) Or (TipoAvviso = oggettoatto.provvedimento.accertamentoRETTIFICA) Then
    '                        'se PREACCERTAMENTO o ACCERTAMENTO hanno scaturito un avviso di accertamento in rettifica
    '                        TipoAvviso = oggettoatto.provvedimento.accertamentoRETTIFICA  'AVVISO DI ACCERTAMENTO IN RETTIFICA                                    
    '                    Else
    '                        TipoAvviso = oggettoatto.provvedimento.accertamentoRETTIFICA 'AVVISO DI ACCERTAMENTO IN RETTIFICA
    '                    End If
    '                End If
    '            ElseIf dblImportoTotaleF2 < 0 Then
    '                TipoAvviso = oggettoatto.provvedimento.RIMBORSO  'AVVISO DI ACCERTAMENTO IN RETTIFICA                                
    '            ElseIf dblImportoTotaleF2 = 0 Then
    '                If blnTIPO_OPERAZIONE_RETTIFICA Then
    '                    TipoAvviso = oggettoatto.provvedimento.autotutelaANNULLAMENTO
    '                Else
    '                    'PREACCERTAMENTO E ACCERTAMENTO HANNO DATO ESITO POSITIVO
    '                    'NO AVVISO - NON CREO IL PROVVEDIMENTO
    '                    TipoAvviso = oggettoatto.provvedimento.noavviso
    '                End If
    '            End If
    '            Select Case (TipoAvviso)
    '                Case oggettoatto.provvedimento.RIMBORSO
    '                    Session("TipoAvviso") = "Avviso di rimborso"
    '                Case oggettoatto.provvedimento.accertamentoRETTIFICA
    '                    Session("TipoAvviso") = "Avviso di accertamento in rettifica"
    '                Case oggettoatto.provvedimento.accertamentoUFFICIO
    '                    Session("TipoAvviso") = "Avviso di accertamento d'ufficio"
    '                Case oggettoatto.provvedimento.autotutelaRETTIFICA
    '                    Session("TipoAvviso") = "Avviso di Autotutela in rettifica"
    '                Case oggettoatto.provvedimento.autotutelaANNULLAMENTO
    '                    Session("TipoAvviso") = "Avviso di Autotutela di annullamento"
    '                Case oggettoatto.provvedimento.noavviso
    '                    Session("TipoAvviso") = "Nessun avviso emesso"
    '                Case Else
    '                    Session("TipoAvviso") = "Avviso non determinato"
    '            End Select
    '            'Rimborso
    '            If ImportoTotaleAvviso < 0 Then
    '                'If DiffTotaleSanzInt < 0 Then 'GIULIA 09082005
    '                TipoAvviso = oggettoatto.provvedimento.RIMBORSO '5
    '                Session("TipoAvviso") = "Avviso di rimborso"
    '            End If

    '            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - ImportoTotaleAvviso:" & ImportoTotaleAvviso & " €")
    '            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - TipoAvviso:" & TipoAvviso & " - " & Session("TipoAvviso"))
    '        End If
    '        Return True
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.grdAccertato.GetTipoAvviso.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '        Return False
    '    End Try
    'End Function
    'Private Function LoadRiepilogo(TotImpICIACCERTAMENTO As Double, TotDiffImpostaACCERTAMENTO As Double, TotImportoSanzioniACCERTAMENTO As Double, TotImportoInteressiACCERTAMENTO As Double, ImportoTotaleAvviso As Double, TotImportoSanzioniRidottoACCERTAMENTO As Double, TotVersamenti As Double, TotImpICIDichiarato As Double, ByRef objhashtableRIEPILOGO As Hashtable) As Boolean
    '    Try
    '        objhashtableRIEPILOGO.Add("TotImpICIACCERTAMENTO", TotImpICIACCERTAMENTO)
    '        objhashtableRIEPILOGO.Add("TotDiffImpostaACCERTAMENTO", TotDiffImpostaACCERTAMENTO)
    '        objhashtableRIEPILOGO.Add("TotImportoSanzioniACCERTAMENTO", TotImportoSanzioniACCERTAMENTO)
    '        objhashtableRIEPILOGO.Add("TotImportoInteressiACCERTAMENTO", TotImportoInteressiACCERTAMENTO)
    '        objhashtableRIEPILOGO.Add("ImportoTotaleAvviso", ImportoTotaleAvviso)
    '        objhashtableRIEPILOGO.Add("TotImportoSanzioniRidottoACCERTAMENTO", TotImportoSanzioniRidottoACCERTAMENTO)
    '        'inserisco in riepilogo i dati relativi al totale dei versamenti 
    '        objhashtableRIEPILOGO.Add("TotVersamenti", TotVersamenti)
    '        objhashtableRIEPILOGO.Add("TotImpICIDichiarato", TotImpICIDichiarato)

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - TotImpICIACCERTAMENTO=" & TotImpICIACCERTAMENTO)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - TotDiffImpostaACCERTAMENTO=" & TotDiffImpostaACCERTAMENTO)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - TotImportoSanzioniACCERTAMENTO=" & TotImportoSanzioniACCERTAMENTO)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - TotImportoInteressiACCERTAMENTO=" & TotImportoInteressiACCERTAMENTO)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - ImportoTotaleAvviso=" & ImportoTotaleAvviso)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - TotImportoSanzioniRidottoACCERTAMENTO=" & TotImportoSanzioniRidottoACCERTAMENTO)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - TotVersamenti=" & TotVersamenti)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - TotImpICIDichiarato=" & TotImpICIDichiarato)
    '        Return True
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.grdAccertato.LoadRiepilogo.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '        Return False
    '    End Try
    'End Function
    'Private Function GetImpFinali(Anno As String, IdContribuente As String, dsRiepilogoFase2 As DataSet, ByRef ImportoTotaleAvviso As Double, TotDiffImpostaACCERTAMENTO As Double, TotImportoSanzioniACCERTAMENTO As Double, TotImportoSanzioniRidottoACCERTAMENTO As Double, TotImportoInteressiACCERTAMENTO As Double, TotDiffImpostaDICHIARATO As Double, objDSCalcoloSanzioniInteressi As DataSet, ByRef objhashtableRIEPILOGO As Hashtable, ByRef iTIPOPROVV_PREACC As Integer, ByRef dblImportoTotaleF2 As Double) As Boolean
    '    Dim dblImportoDiffImpTotaleF2 As Double
    '    Dim dblImportoSanzTotaleF2 As Double
    '    Dim dblImportoSanzRidottoTotaleF2 As Double
    '    Dim dblImportoIntTotaleF2 As Double
    '    Dim strTIPOPROVV_PREACC As String

    '    Try
    '        dblImportoDiffImpTotaleF2 = 0
    '        dblImportoSanzTotaleF2 = 0
    '        dblImportoSanzRidottoTotaleF2 = 0
    '        dblImportoIntTotaleF2 = 0
    '        dblImportoTotaleF2 = 0
    '        iTIPOPROVV_PREACC = oggettoatto.provvedimento.accertamentoUFFICIO

    '        If Not dsRiepilogoFase2 Is Nothing Then
    '            If dsRiepilogoFase2.Tables.Count > 0 Then
    '                If dsRiepilogoFase2.Tables(0).Rows.Count > 0 Then
    '                    strTIPOPROVV_PREACC = CType(dsRiepilogoFase2.Tables(0).Rows(0)("TIPO_PROVVEDIMENTO"), String)
    '                    iTIPOPROVV_PREACC = dsRiepilogoFase2.Tables(0).Rows(0)("TIPO_PROVVEDIMENTO")

    '                    Log.Debug("difimpf2:: " & dsRiepilogoFase2.Tables(0).Rows(0)("DIFFERENZA_IMPOSTA_TOTALE").ToString)
    '                    Log.Debug("sanzf2::" & dsRiepilogoFase2.Tables(0).Rows(0)("IMPORTO_SANZIONI").ToString)
    '                    Log.Debug("sanzridf2::" & dsRiepilogoFase2.Tables(0).Rows(0)("IMPORTO_SANZIONI_RIDOTTO").ToString)
    '                    Log.Debug("intf2::" & dsRiepilogoFase2.Tables(0).Rows(0)("IMPORTO_INTERESSI").ToString)

    '                    dblImportoDiffImpTotaleF2 = dsRiepilogoFase2.Tables(0).Rows(0)("DIFFERENZA_IMPOSTA_TOTALE").ToString.Replace(".", ",")
    '                    dblImportoSanzTotaleF2 = dsRiepilogoFase2.Tables(0).Rows(0)("IMPORTO_SANZIONI").ToString.Replace(".", ",")
    '                    dblImportoSanzRidottoTotaleF2 = dsRiepilogoFase2.Tables(0).Rows(0)("IMPORTO_SANZIONI_RIDOTTO").ToString.Replace(".", ",")
    '                    dblImportoIntTotaleF2 = dsRiepilogoFase2.Tables(0).Rows(0)("IMPORTO_INTERESSI").ToString.Replace(".", ",")
    '                    objhashtableRIEPILOGO("DIFASE2") = dblImportoDiffImpTotaleF2
    '                    objhashtableRIEPILOGO("SANZFASE2") = dblImportoSanzTotaleF2
    '                    objhashtableRIEPILOGO("SANZRIDOTTOFASE2") = dblImportoSanzRidottoTotaleF2
    '                    objhashtableRIEPILOGO("INTFASE2") = dblImportoIntTotaleF2

    '                    dblImportoTotaleF2 = dblImportoDiffImpTotaleF2 + dblImportoSanzTotaleF2 + dblImportoIntTotaleF2
    '                    objhashtableRIEPILOGO("TOTFASE2") = dblImportoTotaleF2

    '                End If
    '            Else
    '                iTIPOPROVV_PREACC = oggettoatto.provvedimento.noavviso
    '            End If
    '        Else
    '            iTIPOPROVV_PREACC = oggettoatto.provvedimento.noavviso
    '        End If
    '        Log.Debug("dblImportoDiffImpTotaleF2::" & dblImportoDiffImpTotaleF2.ToString)
    '        Log.Debug("dblImportoSanzTotaleF2::" & dblImportoSanzTotaleF2.ToString)
    '        Log.Debug("dblImportoSanzRidottoTotaleF2::" & dblImportoSanzRidottoTotaleF2.ToString)
    '        Log.Debug("dblImportoIntTotaleF2::" & dblImportoIntTotaleF2.ToString)
    '        Log.Debug("dblImportoTotaleF2::" & dblImportoTotaleF2.ToString)
    '        '************************************************************************************************************************************
    '        'somma algebrica tra i totali dell'accertamento e quelli della fase2 del preaccertamento
    '        ImportoTotaleAvviso += dblImportoTotaleF2
    '        TotDiffImpostaACCERTAMENTO += dblImportoDiffImpTotaleF2
    '        TotImportoSanzioniACCERTAMENTO += dblImportoSanzTotaleF2
    '        TotImportoSanzioniRidottoACCERTAMENTO += dblImportoSanzRidottoTotaleF2
    '        TotImportoInteressiACCERTAMENTO += dblImportoIntTotaleF2
    '        '************************************************************************************************************************************

    '        objhashtableRIEPILOGO.Add("TOTAVVISO", ImportoTotaleAvviso)
    '        objhashtableRIEPILOGO.Add("DIAVVISO", TotDiffImpostaACCERTAMENTO)
    '        objhashtableRIEPILOGO.Add("SANZAVVISO", TotImportoSanzioniACCERTAMENTO)
    '        objhashtableRIEPILOGO.Add("SANZRIDOTTOAVVISO", TotImportoSanzioniRidottoACCERTAMENTO)
    '        objhashtableRIEPILOGO.Add("INTAVVISO", TotImportoInteressiACCERTAMENTO)

    '        Session("HTRIEPILOGO") = objhashtableRIEPILOGO

    '        If Not objDSCalcoloSanzioniInteressi Is Nothing Then
    '            If objDSCalcoloSanzioniInteressi.Tables.Count > 0 Then
    '                objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI") = TotImportoSanzioniACCERTAMENTO
    '                objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI_RIDOTTO") = TotImportoSanzioniRidottoACCERTAMENTO
    '                objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_INTERESSI") = TotImportoInteressiACCERTAMENTO
    '                objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("DIFFERENZA_IMPOSTA_TOTALE") = TotDiffImpostaACCERTAMENTO
    '                objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_TOTALE_DICHIARATO") = TotDiffImpostaDICHIARATO
    '                objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_TOTALE_ACCERTATO") = ImportoTotaleAvviso
    '            End If
    '        ElseIf Not dsRiepilogoFase2 Is Nothing Then
    '            objDSCalcoloSanzioniInteressi = CreateDatasetPerSanzInt(Anno, IdContribuente, 0, 0, 0)
    '            objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI") = TotImportoSanzioniACCERTAMENTO
    '            objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_SANZIONI_RIDOTTO") = TotImportoSanzioniRidottoACCERTAMENTO
    '            objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_INTERESSI") = TotImportoInteressiACCERTAMENTO
    '            objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("DIFFERENZA_IMPOSTA_TOTALE") = TotDiffImpostaACCERTAMENTO
    '            objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_TOTALE_DICHIARATO") = TotDiffImpostaDICHIARATO
    '            objDSCalcoloSanzioniInteressi.Tables(0).Rows(0).Item("IMPORTO_TOTALE_ACCERTATO") = ImportoTotaleAvviso
    '        End If

    '        Return True
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.grdAccertato.GetImpFinali.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '        Return False
    '    End Try
    'End Function
    'Private Sub AddColDTAccertato(ByRef objDTAccertato As DataTable)
    '    Try
    '        objDTAccertato = CType(Session("DataTableImmobiliDaAccertare"), DataTable)
    '        'AGGIUNGO LA COLONNA IMPORTO_SANZIONI_RIDOTTO AL DATASET DEGLI IMMOBILI ACCERTATI -------------------------------------------------------------------------------------
    '        Dim NewColumn1 As New DataColumn
    '        NewColumn1.ColumnName = "SANZIONI_RIDOTTO"
    '        NewColumn1.DataType = System.Type.GetType("System.Double")
    '        NewColumn1.DefaultValue = 0
    '        objDTAccertato.Columns.Add(NewColumn1)
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.grdAccertato.AddColDTAccertato.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
End Class
