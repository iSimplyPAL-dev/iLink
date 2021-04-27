Imports AnagInterface
Imports ComPlusInterface
Imports log4net
''' <summary>
''' Pagina per la generazione dei provvedimenti TASI.
''' Contiene le funzioni della comandiera e la griglia per la gestione dell'accertato. 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
''' <revisionHistory>
''' <revision date="09/2018">
''' <strong>Cartelle Insoluti</strong>
''' </revision>
''' </revisionHistory>
''' <revisionHistory>
''' <revision date="12/04/2019">
''' <strong>Qualificazione AgID-analisi_rel01</strong>
''' <em>Analisi eventi</em>
''' </revision>
''' </revisionHistory>
Public Class SearchDatiAccertato
    Inherits BaseEnte
    Protected FncGrd As New Formatta.FunctionGrd
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(SearchDatiAccertato))

    Private glbmese_inizio_p, glbmese_fine_p, glbmese_inizio_s, glbmese_fine_s As Integer
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("COD_TRIBUTO") = Utility.Costanti.TRIBUTO_TASI
        Dim ListUI() As objUIICIAccert
        Dim sScript As String
        Try
            hfAnno.Value = CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("ANNOACCERTAMENTO")
            hfIdContribuente.Value = CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("CODCONTRIBUENTE")
            btnInsManuale.Attributes.Add("onclick", "return ApriInserimentoImmobile();return false;")
            btnAccertato.Attributes.Add("onclick", "return ApriRicercaAccertato();return false;")
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

            sScript = "getCalcolaSpese()"
            RegisterScript(sScript, Me.GetType)

            sScript = "parent.document.getElementById('attesaCarica').style.display='none';"
            sScript += "parent.document.getElementById('loadGridAccertato').style.display='' ;"
            RegisterScript(sScript, Me.GetType)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertato.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
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
    ''' <revisionHistory><revision date="12/11/2019">il calcolo interessi 8852/TASI deve essere fatto in acconto/saldo o in unica soluzione in base alla configurazione di TP_GENERALE_ICI</revision></revisionHistory>
    Private Sub btnAccertamento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccertamento.Click
        Try
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - Inizio Accertamento TASI")
            Dim FncGest As New ClsGestioneAccertamenti
            Dim objICI() As objSituazioneFinale
            Dim objDSDichiaratoIci() As objUIICIAccert
            Dim objDTAccertato() As objUIICIAccert
            Dim objhashtableRIEPILOGO As New Hashtable
            Dim blnRetValControlloLEGAME As Boolean
            Dim TotImpICIDichiarato As Double = 0
            Dim objHashTable As New Hashtable
            Dim TotImpICIACCERTAMENTO As Double = 0
            Dim iTIPOPROVV_PREACC As Integer
            Dim oCalcoloSanzioniInteressi As New ObjBaseIntSanz

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
            Dim objDSVersamentiF2 As New DataSet
            Dim TipoAvviso As Integer
            Dim DescrTipoAvviso As String
            Dim lngNewID_PROVVEDIMENTO As Long

            Dim soglia As Double
            Dim TotVersamenti As Double
            Dim ImportoTotaleAvviso As Double
            Dim dblImportoTotaleF2 As Double
            Dim spese As Double

            Dim DATA_RETTIFICA_ANNULLAMENTO As String
            Dim blnTIPO_OPERAZIONE_RETTIFICA As Boolean = CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("TIPO_OPERAZIONE_RETTIFICA")

            Dim dsDettaglioAnagrafica As DataSet = Nothing
            Dim oDettaglioAnagrafica As New DettaglioAnagrafica

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
            objDSDichiaratoIci = CType(Session("DataSetDichiarazioni"), objUIICIAccert())
            If objDSDichiaratoIci Is Nothing Then
                RegisterScript("GestAlert('a', 'warning', '', '', 'Impossibile effettuare un accertamento. Il contribuente non è stato pre-accertato\nma ha delle dichiarazioni!');", Me.GetType)
                Exit Sub
            End If
            objICI = Session("DATASET_CALCOLO_ICI_IMMOBILI_DICHIARATI")
            objDTAccertato = CType(Session("DataTableImmobiliDaAccertare"), objUIICIAccert())
            If objDTAccertato Is Nothing Then
                RegisterScript("GestAlert('a', 'warning', '', '', 'Impossibile effettuare un accertamento. Il contribuente non è stato pre-accertato\nma ha delle dichiarazioni!');", Me.GetType)
                Exit Sub
            End If

            'verifico che i legami siano tutti progressivi
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - verifico che i legami siano tutti progressivi")
            If FncGest.CheckProgLegame(objDSDichiaratoIci) = False Then
                RegisterScript("GestAlert('a', 'warning', '', '', 'Attenzione! I legami non sono consecutivi!');", Me.GetType)
                Exit Sub
            End If
            '08/02/2008 commentato alep come indicato da giulia
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - controllo i legami")
            blnRetValControlloLEGAME = FncGest.ControlloLEGAME(objDSDichiaratoIci, objDTAccertato)
            If blnRetValControlloLEGAME = False And txtControlloLegame.Text = "0" Then
                'presenti immobili dichiarati NON LEGATI a immobili accertati Impossibile effettuare un accertamento
                RegisterScript("ControlloLegame();", Me.GetType)
                Exit Sub
            End If
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - controllo legame doppio accertato")
            blnRetValControlloLEGAME = FncGest.ControlloLEGAMEdoppioAccertato(objDSDichiaratoIci)
            If blnRetValControlloLEGAME = True Then
                'presenti immobili accertati doppi per id legame Impossibile effettuare un accertamento
                RegisterScript("GestAlert('a', 'warning', '', '', 'Impossibile effettuare un accertamento. Il sistema ha rilevato più immobili accertati con lo stesso LEGAME!');", Me.GetType)
                Exit Sub
            End If
            'ricalcolo la fase 2 del pre accertamento 
            If Not Session("ESCLUDI_PREACCERTAMENTO") Then
                'Calcolo Fase 2 PreAccertamento
                '*** 201810 - Generazione Massiva Atti ***
                Dim ImpDichAcconto, ImpDichSaldo, ImpDichTotale As Double
                ImpDichAcconto = 0 : ImpDichSaldo = 0 : ImpDichTotale = 0
                For Each myUI As objUIICIAccert In objDSDichiaratoIci
                    If myUI.Anno = hfAnno.Value Then
                        ImpDichAcconto += myUI.AccDovuto
                        ImpDichSaldo += myUI.SalDovuto
                        ImpDichTotale += myUI.TotDovuto
                    End If
                Next
                If FncGest.CalcoloPreAccertamento(ConstSession.IdEnte, ConstSession.CodTributo, Utility.Costanti.TRIBUTO_TASI, hfAnno.Value, oDettaglioAnagrafica.COD_CONTRIBUENTE, CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("ID_PROVVEDIMENTO_RETTIFICA"), CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("DATA_ELABORAZIONE_PER_RETTIFICA"), dsDettaglioAnagrafica, ImpDichAcconto, ImpDichSaldo, ImpDichTotale, dsRiepilogoFase2, TotVersamenti, dsSanzioniFase2, ListInteressiFase2, objDSVersamentiF2) = False Then
                    Throw New Exception("errore in calcolopreaccertamento")
                End If
            End If

            'Tipo avviso
            If FncGest.GetTipoAvviso(CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("TIPO_OPERAZIONE_RETTIFICA"), True, objDTAccertato, objDSDichiaratoIci, ImportoTotaleAvviso, dblImportoTotaleF2, iTIPOPROVV_PREACC, TipoAvviso, DescrTipoAvviso) = False Then
                Throw New Exception("errore in gettipoavviso")
            End If
            Session("TipoAvviso") = DescrTipoAvviso
            'Calcolo importi/sanzioni/interessi per singoli immobili
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - Calcolo importi/sanzioni/interessi per singoli immobili")
            If FncGest.CalcoloImpSanzIntSingleUI(objHashTable, oDettaglioAnagrafica.COD_CONTRIBUENTE, hfAnno.Value, Session("DataSetSanzioni"), objDSDichiaratoIci, TipoAvviso, OggettoAtto.Fase.DichiaratoAccertato, oDettaglioAnagrafica.DataMorte _
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
            If FncGest.GetTipoAvviso(CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("TIPO_OPERAZIONE_RETTIFICA"), False, objDTAccertato, objDSDichiaratoIci, ImportoTotaleAvviso, dblImportoTotaleF2, iTIPOPROVV_PREACC, TipoAvviso, DescrTipoAvviso) = False Then
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
                    If objHashTable.Contains("DATA_ANNULLAMENTO") = False Then
                        objHashTable.Add("DATA_ANNULLAMENTO", DATA_RETTIFICA_ANNULLAMENTO)
                    Else
                        objHashTable("DATA_ANNULLAMENTO") = DATA_RETTIFICA_ANNULLAMENTO
                    End If
                    If objHashTable.Contains("DATA_RETTIFICA") = False Then
                        objHashTable.Add("DATA_RETTIFICA", "")
                    Else
                        objHashTable("DATA_RETTIFICA") = ""
                    End If
                Else
                    If objHashTable.Contains("DATA_RETTIFICA") = False Then
                        objHashTable.Add("DATA_RETTIFICA", DATA_RETTIFICA_ANNULLAMENTO)
                    Else
                        objHashTable("DATA_RETTIFICA") = DATA_RETTIFICA_ANNULLAMENTO
                    End If
                    If objHashTable.Contains("DATA_ANNULLAMENTO") = False Then
                        objHashTable.Add("DATA_ANNULLAMENTO", "")
                    Else
                        objHashTable("DATA_ANNULLAMENTO") = ""
                    End If
                End If
            End If

            If ImportoTotaleAvviso < 0 Then
                If ImportoTotaleAvviso * (-1) < soglia Then
                    If blnTIPO_OPERAZIONE_RETTIFICA = True Then
                        TipoAvviso = OggettoAtto.Provvedimento.AutotutelaRettifica
                    Else
                        TipoAvviso = OggettoAtto.Provvedimento.NoAvviso
                    End If
                End If
            Else
                If ImportoTotaleAvviso = 0 Then
                    If blnTIPO_OPERAZIONE_RETTIFICA = True Then
                        TipoAvviso = OggettoAtto.Provvedimento.AutotutelaAnnullamento
                    Else
                        TipoAvviso = OggettoAtto.Provvedimento.NoAvviso
                    End If
                ElseIf ImportoTotaleAvviso < soglia Then
                    If blnTIPO_OPERAZIONE_RETTIFICA = True Then
                        TipoAvviso = OggettoAtto.Provvedimento.AutotutelaRettifica
                    Else
                        TipoAvviso = OggettoAtto.Provvedimento.NoAvviso
                    End If
                End If
            End If

            If TipoAvviso <> OggettoAtto.Provvedimento.NoAvviso Or blnTIPO_OPERAZIONE_RETTIFICA = True Then
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - Chiamo updateDBAccertamenti")
                'Inserisco i dati dell'accertamento nel database
                objHashTable("CODTRIBUTO") = ConstSession.CodTributo
                lngNewID_PROVVEDIMENTO = objCOMUpdateDBAccertamenti.updateDBAccertamenti(ConstSession.DBType, ConstSession.StringConnection, ConstSession.IdEnte, oDettaglioAnagrafica.COD_CONTRIBUENTE, objHashTable, oCalcoloSanzioniInteressi, objDSSanzioni, ListInteressi, spese, objICI, objDSDichiaratoIci, objDTAccertato, dsSanzioniFase2, ListInteressiFase2, objDSVersamentiF2, ConstSession.UserName)
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - Terminata updateDBAccertamenti")
                If lngNewID_PROVVEDIMENTO < 1 Then
                    Throw New Exception("Errore in inserimento avviso")
                End If
                Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Provvedimenti, "btnAccertamento", Utility.Costanti.AZIONE_NEW, ConstSession.CodTributo, ConstSession.IdEnte, lngNewID_PROVVEDIMENTO)
            Else
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - NON chiamo updateDBAccertamenti perchè NESSUN_AVVISO")
            End If

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
                        'Nessun avviso emesso Effettuo il rientro dell'accertato
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - caso 2 - Effettuo il rientro dell'accertato ID_PROVVEDIMENTO=" & lngNewID_PROVVEDIMENTO)
                        str1 = str1 & "RiepilogoAccertato('0'," & lngNewID_PROVVEDIMENTO & ");"
                    End If
                    RegisterScript(str1, Me.GetType)
                ElseIf ImportoTotaleAvviso < soglia Then
                    Session("TipoAvviso") = "Avviso sotto soglia - non emesso"
                    'Non emetto Avviso
                    Dim str1 As String
                    str1 = "parent.document.getElementById('attesaCarica').style.display='none';"
                    If blnTIPO_OPERAZIONE_RETTIFICA = True Then
                        'Atto di autotutela di annullamento. Importo inferiore alla soglia
                        str1 = str1 & "GestAlert('a', 'success', '', '', 'La posizione è corretta.\nE\' stato elaborato un ATTO DI AUTOTUTELA DI ANNULLAMENTO');"
                        str1 = str1 & "RiepilogoAccertato('1'," & lngNewID_PROVVEDIMENTO & ");"
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - La posizione è corretta. Elaborato ATTO DI AUTOTUTELA DI ANNULLAMENTO")
                    Else
                        'Effettuo il rientro dell'accertato Nessun avviso emesso. Importo inferiore alla soglia
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
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - Inizio Accertamento TASI")
    '        Dim FncGest As New ClsGestioneAccertamenti
    '        Dim objICI() As objSituazioneFinale
    '        Dim objDSDichiaratoIci() As objUIICIAccert
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
    '        Dim objDSInteressi As New DataSet
    '        Dim dsSanzioniFase2 As New DataSet
    '        Dim ListInteressiFase2() As ObjInteressiSanzioni
    '        Dim objDSVersamentiF2 As New DataSet
    '        Dim TipoAvviso As Integer
    '        Dim DescrTipoAvviso As String
    '        Dim lngNewID_PROVVEDIMENTO As Long

    '        Dim soglia As Double
    '        Dim TotVersamenti As Double
    '        Dim ImportoTotaleAvviso As Double
    '        Dim dblImportoTotaleF2 As Double
    '        Dim blnResult As Boolean = False
    '        Dim TipoAvvisoRimborso As Integer = -1
    '        Dim spese As Double

    '        Dim DATA_RETTIFICA_ANNULLAMENTO As String
    '        Dim blnTIPO_OPERAZIONE_RETTIFICA As Boolean = CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("TIPO_OPERAZIONE_RETTIFICA")

    '        Dim dsDettaglioAnagrafica As DataSet = Nothing
    '        Dim oDettaglioAnagrafica As New DettaglioAnagrafica

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
    '            RegisterScript("GestAlert('a', 'warning', '', '', 'Impossibile effettuare un accertamento. Il contribuente non è stato pre-accertato\nma ha delle dichiarazioni!');", Me.GetType)
    '            Exit Sub
    '        End If
    '        objICI = Session("DATASET_CALCOLO_ICI_IMMOBILI_DICHIARATI")
    '        objDTAccertato = CType(Session("DataTableImmobiliDaAccertare"), objUIICIAccert())
    '        If objDTAccertato Is Nothing Then
    '            RegisterScript("GestAlert('a', 'warning', '', '', 'Impossibile effettuare un accertamento. Il contribuente non è stato pre-accertato\nma ha delle dichiarazioni!');", Me.GetType)
    '            Exit Sub
    '        End If

    '        'verifico che i legami siano tutti progressivi
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - verifico che i legami siano tutti progressivi")
    '        If FncGest.CheckProgLegame(objDSDichiaratoIci) = False Then
    '            RegisterScript("GestAlert('a', 'warning', '', '', 'Attenzione! I legami non sono consecutivi!');", Me.GetType)
    '            Exit Sub
    '        End If
    '        '08/02/2008 commentato alep come indicato da giulia
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - controllo i legami")
    '        blnRetValControlloLEGAME = FncGest.ControlloLEGAME(objDSDichiaratoIci, objDTAccertato)
    '        If blnRetValControlloLEGAME = False And txtControlloLegame.Text = "0" Then
    '            'presenti immobili dichiarati NON LEGATI a immobili accertati Impossibile effettuare un accertamento
    '            RegisterScript("ControlloLegame();", Me.GetType)
    '            Exit Sub
    '        End If
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - controllo legame doppio accertato")
    '        blnRetValControlloLEGAME = FncGest.ControlloLEGAMEdoppioAccertato(objDSDichiaratoIci)
    '        If blnRetValControlloLEGAME = True Then
    '            'presenti immobili accertati doppi per id legame Impossibile effettuare un accertamento
    '            RegisterScript("GestAlert('a', 'warning', '', '', 'Impossibile effettuare un accertamento. Il sistema ha rilevato più immobili accertati con lo stesso LEGAME!');", Me.GetType)
    '            Exit Sub
    '        End If
    '        'ricalcolo la fase 2 del pre accertamento 
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
    '            If FncGest.CalcoloPreAccertamento(ConstSession.IdEnte, ConstSession.CodTributo, Utility.Costanti.TRIBUTO_TASI, CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("ANNOACCERTAMENTO"), CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("CODCONTRIBUENTE"), CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("ID_PROVVEDIMENTO_RETTIFICA"), CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("DATA_ELABORAZIONE_PER_RETTIFICA"), dsDettaglioAnagrafica, ImpDichAcconto, ImpDichSaldo, ImpDichTotale, dsRiepilogoFase2, TotVersamenti, dsSanzioniFase2, ListInteressiFase2, objDSVersamentiF2) = False Then
    '                Throw New Exception("errore in calcolopreaccertamento")
    '            End If
    '        End If
    '        Dim controllaAccertato As New DBPROVVEDIMENTI.ProvvedimentiDB
    '        Dim objHashTable1 As Hashtable

    '        objHashTable1 = Session("HashTableDichiarazioniAccertamenti")

    '        'Tipo avviso
    '        If FncGest.GetTipoAvviso(CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("TIPO_OPERAZIONE_RETTIFICA"), True, objDTAccertato, objDSDichiaratoIci, ImportoTotaleAvviso, dblImportoTotaleF2, iTIPOPROVV_PREACC, TipoAvviso, DescrTipoAvviso) = False Then
    '            Throw New Exception("errore in gettipoavviso")
    '        End If
    '        Session("TipoAvviso") = DescrTipoAvviso
    '        'Calcolo importi/sanzioni/interessi per singoli immobili
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - Calcolo importi/sanzioni/interessi per singoli immobili")
    '        If FncGest.CalcoloImpSanzIntSingleUI(objHashTable, CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("ANNOACCERTAMENTO"), Session("DataSetSanzioni"), objDSDichiaratoIci, TipoAvviso, oDettaglioAnagrafica.DataMorte _
    '            , objDSCalcoloSanzioniInteressi, objDSSanzioni, objDSInteressi, objDTAccertato _
    '            , TotDiffImpostaACCERTAMENTO, TotImpICIDichiarato, TotDiffImpostaDICHIARATO, TotImpICIACCERTAMENTO, TotImportoInteressiACCERTAMENTO, TotImportoSanzioniACCERTAMENTO, TotImportoSanzioniRidottoACCERTAMENTO, TotVersamenti) = False Then
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
    '        If FncGest.GetImpFinali(objHashTable("ANNOACCERTAMENTO"), objHashTable("CODCONTRIBUENTE"), dsRiepilogoFase2, ImportoTotaleAvviso, TotDiffImpostaACCERTAMENTO, TotImportoSanzioniACCERTAMENTO, TotImportoSanzioniRidottoACCERTAMENTO, TotImportoInteressiACCERTAMENTO, TotDiffImpostaDICHIARATO, objDSCalcoloSanzioniInteressi, objhashtableRIEPILOGO, iTIPOPROVV_PREACC, dblImportoTotaleF2) = False Then
    '            Throw New Exception("errore in getimpfinali")
    '        End If
    '        Session("HTRIEPILOGO") = objhashtableRIEPILOGO
    '        If FncGest.GetTipoAvviso(CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("TIPO_OPERAZIONE_RETTIFICA"), False, objDTAccertato, objDSDichiaratoIci, ImportoTotaleAvviso, dblImportoTotaleF2, iTIPOPROVV_PREACC, TipoAvviso, DescrTipoAvviso) = False Then
    '            Throw New Exception("errore in gettipoavviso")
    '        End If
    '        Dim objCOMUpdateDBAccertamenti As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
    '        Dim sCodContibuente As String
    '        sCodContibuente = CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("CODCONTRIBUENTE")

    '        'reperisco soglia
    '        Dim objProvvedimentiDB As New DBPROVVEDIMENTI.ProvvedimentiDB
    '        soglia = 0
    '        soglia = objProvvedimentiDB.GetSogliaMinima(objHashTable("ANNOACCERTAMENTO"), Utility.Costanti.TRIBUTO_ICI, objHashTable("CODENTE"), TipoAvviso)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - Soglia:" & soglia & " €")
    '        'Calcolo le spese
    '        If hfCalcolaSpese.Value = "0" Then
    '            spese = objProvvedimentiDB.GetSpese(objHashTable("ANNOACCERTAMENTO"), Utility.Costanti.TRIBUTO_ICI, objHashTable("CODENTE"), TipoAvviso)
    '        Else
    '            spese = 0
    '        End If
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - Spese:" & spese & " €")
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
    '                If objHashTable.Contains("DATA_ANNULLAMENTO") = False Then
    '                    objHashTable.Add("DATA_ANNULLAMENTO", DATA_RETTIFICA_ANNULLAMENTO)
    '                Else
    '                    objHashTable.Add("DATA_ANNULLAMENTO", DATA_RETTIFICA_ANNULLAMENTO)
    '                End If
    '                If objHashTable.Contains("DATA_RETTIFICA") = False Then
    '                    objHashTable.Add("DATA_RETTIFICA", "")
    '                Else
    '                    objHashTable.Add("DATA_RETTIFICA", "")
    '                End If
    '            Else
    '                If objHashTable.Contains("DATA_RETTIFICA") = False Then
    '                    objHashTable.Add("DATA_RETTIFICA", DATA_RETTIFICA_ANNULLAMENTO)
    '                Else
    '                    objHashTable.Add("DATA_RETTIFICA", DATA_RETTIFICA_ANNULLAMENTO)
    '                End If
    '                If objHashTable.Contains("DATA_ANNULLAMENTO") = False Then
    '                    objHashTable.Add("DATA_ANNULLAMENTO", "")
    '                Else
    '                    objHashTable.Add("DATA_ANNULLAMENTO", "")
    '                End If
    '            End If
    '        End If

    '        If ImportoTotaleAvviso < 0 Then
    '            If ImportoTotaleAvviso * (-1) < soglia Then
    '                TipoAvviso = OggettoAtto.Provvedimento.NoAvviso
    '            End If
    '        Else
    '            If ImportoTotaleAvviso = 0 Then
    '                TipoAvviso = OggettoAtto.Provvedimento.NoAvviso
    '            ElseIf ImportoTotaleAvviso < soglia Then
    '                TipoAvviso = OggettoAtto.Provvedimento.NoAvviso
    '            End If
    '        End If

    '        If TipoAvviso <> OggettoAtto.Provvedimento.NoAvviso Then
    '            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - Chiamo updateDBAccertamenti")
    '            'Inserisco i dati dell'accertamento nel database
    '            objHashTable("CODTRIBUTO") = ConstSession.CodTributo
    '            lngNewID_PROVVEDIMENTO = objCOMUpdateDBAccertamenti.updateDBAccertamenti(ConstSession.DBType, objHashTable, objDSCalcoloSanzioniInteressi, objDSSanzioni, objDSInteressi, spese, objICI, objDSDichiaratoIci, objDTAccertato, dsSanzioniFase2, ListInteressiFase2, objDSVersamentiF2, ConstSession.UserName)
    '            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - Terminata updateDBAccertamenti")
    '            If lngNewID_PROVVEDIMENTO < 1 Then
    '                Throw New Exception("Errore in inserimento avviso")
    '            End If
    '            Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
    '            fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Provvedimenti, "btnAccertamento", Utility.Costanti.AZIONE_NEW, ConstSession.CodTributo, ConstSession.IdEnte, lngNewID_PROVVEDIMENTO)
    '        Else
    '            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - NON chiamo updateDBAccertamenti perchè NESSUN_AVVISO")
    '        End If

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
    '                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - Importo Avviso inferiore alla soglia. Elaborato ATTO DI AUTOTUTELA IN RETTIFICA")
    '                Else
    '                    'Effettuo il rientro dell'accertato
    '                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - caso 1 - Effettuo il rientro dell'accertato ID_PROVVEDIMENTO=" & lngNewID_PROVVEDIMENTO)
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
    '                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - La posizione è corretta. Elaborato ATTO DI AUTOTUTELA DI ANNULLAMENTO")
    '                Else
    '                    'Nessun avviso emesso Effettuo il rientro dell'accertato
    '                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - caso 2 - Effettuo il rientro dell'accertato ID_PROVVEDIMENTO=" & lngNewID_PROVVEDIMENTO)
    '                    str1 = str1 & "RiepilogoAccertato('0'," & lngNewID_PROVVEDIMENTO & ");"
    '                End If
    '                RegisterScript(str1, Me.GetType)
    '            ElseIf ImportoTotaleAvviso < soglia Then
    '                Session("TipoAvviso") = "Avviso sotto soglia - non emesso"
    '                'Non emetto Avviso
    '                Dim str1 As String
    '                str1 = "parent.document.getElementById('attesaCarica').style.display='none';"
    '                If blnTIPO_OPERAZIONE_RETTIFICA = True Then
    '                    'Atto di autotutela di annullamento. Importo inferiore alla soglia
    '                    str1 = str1 & "GestAlert('a', 'success', '', '', 'La posizione è corretta.\nE\' stato elaborato un ATTO DI AUTOTUTELA DI ANNULLAMENTO');"
    '                    str1 = str1 & "RiepilogoAccertato('1'," & lngNewID_PROVVEDIMENTO & ");"
    '                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - La posizione è corretta. Elaborato ATTO DI AUTOTUTELA DI ANNULLAMENTO")
    '                Else
    '                    'Effettuo il rientro dell'accertato Nessun avviso emesso. Importo inferiore alla soglia
    '                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - caso 3 - Effettuo il rientro dell'accertato ID_PROVVEDIMENTO=" & lngNewID_PROVVEDIMENTO)
    '                    str1 = str1 & "RiepilogoAccertato('0'," & lngNewID_PROVVEDIMENTO & ");"
    '                End If
    '                RegisterScript(str1, Me.GetType)
    '            End If
    '        End If
    '        objDSSanzioni.Dispose()
    '        objDSInteressi.Dispose()
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
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - Fine Accertamento")
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertato.btnAccertamento_Click.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    'Private Sub btnAccertamento_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccertamento.Click
    '    'Dim objSessione As CreateSessione
    '    Try
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - Inizio Accertamento TASI")
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
    '        Dim ListInteressiFase2() As ObjInteressiSanzioni
    '        'Dim objDSImmobiliIci As New DataSet
    '        'Dim objDSContitolariIci As New DataSet
    '        Dim objDSVersamentiF2 As New DataSet
    '        Dim TipoAvviso As Integer
    '        Dim DescrTipoAvviso As String
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
    '            RegisterScript("GestAlert('a', 'warning', '', '', 'Impossibile effettuare un accertamento. Il contribuente non è stato pre-accertato\nma ha delle dichiarazioni!');", Me.GetType)
    '            Exit Sub
    '        End If
    '        objICI = Session("DATASET_CALCOLO_ICI_IMMOBILI_DICHIARATI")
    '        objDTAccertato = CType(Session("DataTableImmobiliDaAccertare"), objUIICIAccert())
    '        If objDTAccertato Is Nothing Then
    '            RegisterScript("GestAlert('a', 'warning', '', '', 'Impossibile effettuare un accertamento. Il contribuente non è stato pre-accertato\nma ha delle dichiarazioni!');", Me.GetType)
    '            Exit Sub
    '        End If

    '        'verifico che i legami siano tutti progressivi
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - verifico che i legami siano tutti progressivi")
    '        If FncGest.CheckProgLegame(objDSDichiaratoIci) = False Then
    '            RegisterScript("GestAlert('a', 'warning', '', '', 'Attenzione! I legami non sono consecutivi!');", Me.GetType)
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
    '            RegisterScript("GestAlert('a', 'warning', '', '', 'Impossibile effettuare un accertamento. Il sistema ha rilevato più immobili accertati con lo stesso LEGAME!');", Me.GetType)
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
    '            'If FncGest.CalcoloPreAccertamento(ConstSession.IdEnte, ConstSession.CodTributo, Utility.Costanti.TRIBUTO_TASI, -1, CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("ANNOACCERTAMENTO"), CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("CODCONTRIBUENTE"), CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("ID_PROVVEDIMENTO_RETTIFICA"), CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("DATA_ELABORAZIONE_PER_RETTIFICA"), dsDettaglioAnagrafica, dsRiepilogoFase2, TotVersamenti, dsSanzioniFase2, dsInteressiFase2, objDSDichiaratoIci, objDSVersamentiF2) = False Then
    '            If FncGest.CalcoloPreAccertamento(ConstSession.IdEnte, ConstSession.CodTributo, Utility.Costanti.TRIBUTO_TASI, CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("ANNOACCERTAMENTO"), CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("CODCONTRIBUENTE"), CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("ID_PROVVEDIMENTO_RETTIFICA"), CType(Session("HashTableDichiarazioniAccertamenti"), Hashtable)("DATA_ELABORAZIONE_PER_RETTIFICA"), dsDettaglioAnagrafica, ImpDichAcconto, ImpDichSaldo, ImpDichTotale, dsRiepilogoFase2, TotVersamenti, dsSanzioniFase2, ListInteressiFase2, objDSVersamentiF2) = False Then
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
    '            , objDSCalcoloSanzioniInteressi, objDSSanzioni, objDSInteressi, objDTAccertato _
    '            , TotDiffImpostaACCERTAMENTO, TotImpICIDichiarato, TotDiffImpostaDICHIARATO, TotImpICIACCERTAMENTO, TotImportoInteressiACCERTAMENTO, TotImportoSanzioniACCERTAMENTO, TotImportoSanzioniRidottoACCERTAMENTO, TotVersamenti) = False Then
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
    '                If objHashTable.Contains("DATA_ANNULLAMENTO") = False Then
    '                    objHashTable.Add("DATA_ANNULLAMENTO", DATA_RETTIFICA_ANNULLAMENTO)
    '                Else
    '                    objHashTable.Add("DATA_ANNULLAMENTO", DATA_RETTIFICA_ANNULLAMENTO)
    '                End If
    '                If objHashTable.Contains("DATA_RETTIFICA") = False Then
    '                    objHashTable.Add("DATA_RETTIFICA", "")
    '                Else
    '                    objHashTable.Add("DATA_RETTIFICA", "")
    '                End If
    '            Else
    '                If objHashTable.Contains("DATA_RETTIFICA") = False Then
    '                    objHashTable.Add("DATA_RETTIFICA", DATA_RETTIFICA_ANNULLAMENTO)
    '                Else
    '                    objHashTable.Add("DATA_RETTIFICA", DATA_RETTIFICA_ANNULLAMENTO)
    '                End If
    '                If objHashTable.Contains("DATA_ANNULLAMENTO") = False Then
    '                    objHashTable.Add("DATA_ANNULLAMENTO", "")
    '                Else
    '                    objHashTable.Add("DATA_ANNULLAMENTO", "")
    '                End If
    '            End If
    '        End If

    '        If ImportoTotaleAvviso < 0 Then
    '            If ImportoTotaleAvviso * (-1) < soglia Then
    '                TipoAvviso = OggettoAtto.Provvedimento.NoAvviso
    '            End If
    '        Else
    '            If ImportoTotaleAvviso = 0 Then
    '                TipoAvviso = OggettoAtto.Provvedimento.NoAvviso
    '            ElseIf ImportoTotaleAvviso < soglia Then
    '                TipoAvviso = OggettoAtto.Provvedimento.NoAvviso
    '            End If
    '        End If

    '        If TipoAvviso <> OggettoAtto.Provvedimento.NoAvviso Then
    '            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - Chiamo updateDBAccertamenti")
    '            'Inserisco i dati dell'accertamento nel database
    '            objHashTable("CODTRIBUTO") = ConstSession.CodTributo
    '            'queryResult = objCOMUpdateDBAccertamenti.updateDBAccertamenti(objHashTable, objDSCalcoloSanzioniInteressi, objDSSanzioni, objDSInteressi, spese, objICI, objDSDichiarazioni, objDSAccertato, dsSanzioniFase2, dsInteressiFase2, objDSDichiaratoIci, objDSImmobiliIci, objDSContitolariIci, objDSVersamentiF2)
    '            lngNewID_PROVVEDIMENTO = objCOMUpdateDBAccertamenti.updateDBAccertamenti(objHashTable, objDSCalcoloSanzioniInteressi, objDSSanzioni, objDSInteressi, spese, objICI, objDSDichiaratoIci, objDTAccertato, dsSanzioniFase2, ListInteressiFase2, objDSVersamentiF2)
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
    '                    str1 = str1 & "GestAlert('a', 'success', '', '', 'Importo Avviso inferiore alla soglia.\nE\' stato elaborato un ATTO DI AUTOTUTELA IN RETTIFICA');"
    '                    str1 = str1 & "RiepilogoAccertato('1'," & lngNewID_PROVVEDIMENTO & ");"
    '                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - Importo Avviso inferiore alla soglia. Elaborato ATTO DI AUTOTUTELA IN RETTIFICA")
    '                Else
    '                    'Effettuo il rientro dell'accertato
    '                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - caso 1 - Effettuo il rientro dell'accertato ID_PROVVEDIMENTO=" & lngNewID_PROVVEDIMENTO)
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
    '                    str1 = str1 & "GestAlert('a', 'success', '', '', 'La posizione è corretta.\nE\' stato elaborato un ATTO DI AUTOTUTELA DI ANNULLAMENTO');"
    '                    str1 = str1 & "RiepilogoAccertato('1'," & lngNewID_PROVVEDIMENTO & ");"
    '                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - La posizione è corretta. Elaborato ATTO DI AUTOTUTELA DI ANNULLAMENTO")
    '                Else
    '                    'Nessun avviso emesso
    '                    'Effettuo il rientro dell'accertato
    '                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - caso 2 - Effettuo il rientro dell'accertato ID_PROVVEDIMENTO=" & lngNewID_PROVVEDIMENTO)
    '                    str1 = str1 & "RiepilogoAccertato('0'," & lngNewID_PROVVEDIMENTO & ");"
    '                End If
    '                RegisterScript(str1, Me.GetType)
    '            ElseIf ImportoTotaleAvviso < soglia Then
    '                Session("TipoAvviso") = "Avviso sotto soglia - non emesso"
    '                'Non emetto Avviso
    '                Dim str1 As String
    '                str1 = "parent.document.getElementById('attesaCarica').style.display='none';"
    '                If blnTIPO_OPERAZIONE_RETTIFICA = True Then
    '                    'Atto di autotutela di annullamento. Importo inferiore alla soglia
    '                    str1 = str1 & "GestAlert('a', 'success', '', '', 'La posizione è corretta.\nE\' stato elaborato un ATTO DI AUTOTUTELA DI ANNULLAMENTO');"
    '                    str1 = str1 & "RiepilogoAccertato('1'," & lngNewID_PROVVEDIMENTO & ");"
    '                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - La posizione è corretta. Elaborato ATTO DI AUTOTUTELA DI ANNULLAMENTO")
    '                Else
    '                    'Effettuo il rientro dell'accertato
    '                    'Nessun avviso emesso. Importo inferiore alla soglia
    '                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - caso 3 - Effettuo il rientro dell'accertato ID_PROVVEDIMENTO=" & lngNewID_PROVVEDIMENTO)
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

    'Protected Sub ChkInteressi_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
    '    Dim ck1 As CheckBox = CType(sender, CheckBox)
    '    Dim dgItem As GridViewRow = CType(ck1.NamingContainer, GridViewRow)
    '    Dim idlegame As Integer
    '    Dim workTable As New DataTable("IMMOBILI")
    '    Dim rowsArray() As DataRow
    '    Try
    '        'reperisco idlegame
    '        idlegame = CInt(dgItem.Cells(19).Text)
    '        If Not Session("DataTableImmobiliDaAccertare") Is Nothing Then

    '            workTable = CType(Session("DataTableImmobiliDaAccertare"), DataTable)
    '            rowsArray = workTable.Select("IDLEGAME='" & idlegame & "'")
    '            rowsArray(0)("CALCOLA_INTERESSI") = ck1.Checked
    '            rowsArray(0).AcceptChanges()

    '            Session("DataTableImmobiliDaAccertare") = workTable
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertato.ChkInteressi_CheckedChanged.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

#Region "Griglie"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdRowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                Log.Debug("ICI.SearchDatiAccertato.GrdRowDataBound")
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
                If idInteressi.ToLower = "true" Or idInteressi = "-1" Or idInteressi = "1" Then
                    'chkInteressi.Checked = True
                    a.ImageUrl = "..\..\images\Bottoni\visto.gif"
                Else
                    a.ImageUrl = "..\..\images\Bottoni\trasparente.png"
                End If

                e.Row.Cells(0).BackColor = Color.PaleGoldenrod
                e.Row.Cells(0).Font.Bold = True

                CType(e.Row.FindControl("chkSanzioni"), CheckBox).Attributes.Add("onClick", "return ApriDettaglioSanzioni('" & CType(e.Row.FindControl("lblLegame"), Label).Text & "','" & chkSanzioni.ClientID.ToString & "','" & idSanzioni & "')")
                e.Row.Cells(21).ToolTip = "Premere questo pulsante per associare le sanzioni all'immobile, gestire le motivazioni e configurare la possibilità di calcolare gli interessi"
                e.Row.Cells(24).ToolTip = "Premere questo pulsante per modificare il legame dell'immobile"
                e.Row.Cells(25).ToolTip = "Premere questo pulsante per eliminare l'immobile"
                e.Row.Cells(26).ToolTip = "Premere questo pulsante per entrare in modifica sull'immobile"
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertato.GrdRowDataBound.errore: ", ex)
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
                    Dim workTable() As objUIICIAccert
                    For Each myRow As GridViewRow In GrdAccertato.Rows
                        If IDRow = myRow.Cells(0).Text Then
                            'recupero idLegame da hiddenfield
                            Dim idLegame As String = CType(myRow.FindControl("hfIDLEGAME"), HiddenField).Value
                            'eseguo le stesse operazioni che faceva prima della modifica
                            If Not Session("DataTableImmobiliDaAccertare") Is Nothing Then
                                workTable = CType(Session("DataTableImmobiliDaAccertare"), objUIICIAccert())
                                For Each myItem As objUIICIAccert In workTable
                                    If myItem.IdLegame = idLegame Then
                                        Dim idInteressi As String
                                        idInteressi = CType(myRow.FindControl("hfInteressi"), HiddenField).Value
                                        If idInteressi > 0 Then
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
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertato.GrdRowCommand.errore: ", ex)
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
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertato.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub    'Private Function CalcolaAccertato() As Boolean
    '    'workTable è ordinato per progressivo
    '    Dim maxLeg As Integer
    '    Dim i As Integer = 0
    '    Dim rowsArray() As DataRow
    '    Dim workTable As New DataTable
    '    Dim FncGen As New ClsGestioneAccertamenti

    '    maxLeg = maxLegame("idLegame")
    '    If Not Session("DataTableImmobiliDaAccertare") Is Nothing Then
    '        workTable = CType(Session("DataTableImmobiliDaAccertare"), DataTable)
    '    End If

    '    Dim dsUI As objSituazioneFinale() ' = FncGen.CreateDSperCalcoloICI()
    '    Dim objHashTableICI As Hashtable
    '    'La session viene creata quando ricerco in territorio gli immobili
    '    objHashTableICI = Session("HashTableDichiarazioniAccertamenti")

    '    Try
    '        For i = 1 To maxLeg
    '            'trasformo l'oggetto in ingresso con quello da passare al servizio di calcolo (il datatable da 54item in tp_situazione_finale_ici contenuto in objICI) N.B. è popolato in ordine di legame
    '            rowsArray = workTable.Select("IDLEGAME='" & i & "'")
    '            If rowsArray.Length > 0 Then
    '                '*** 20110506 - se non calcolo non funziona il resto perchè ho objICI vuoto *** 
    '                rowsArray(0).Item("ICICalcolato") = addRowsCalcoloICI(dsUI, objHashTableICI, rowsArray)
    '                If rowsArray(0).Item("ICICalcolato") < 0 Then
    '                    '*** 20120704 - IMU ***
    '                    Throw New Exception("grdAccertato::CalcolaAccertato::Errore in calcolo ICI/IMU")
    '                End If
    '                rowsArray(0).AcceptChanges()
    '                workTable.AcceptChanges()
    '            End If
    '        Next
    '        'Lancio il calcolo ICI richiamando il servizio
    '        If CalcolaICITotale(dsUI, objHashTableICI, TipoCalcolo) = False Then
    '            Return False
    '        End If

    '        'Aggiorno Colonna ICI Calcolato nella Griglia dell'accertato
    '        If dsUI.GetLength(0) > 0 Then
    '            'If dsUI.Tables(0).Rows.Count > 0 Then
    '            '*** 20110506 è sbagliato perchè si aggiorna un'oggetto ordinato per progressivo con il valore di quello ordinato per legame senza assicurarsi di essere effettivamente sulla stessa posizione ***
    '            'ciclo sugli immobili della videata
    '            'Dim rows() As DataRow
    '            For i = 0 To workTable.Rows.Count - 1
    '                'prelevo l'immobile della videata per id legame
    '                rowsArray = workTable.Select("IDLEGAME='" & i + 1 & "'")
    '                If rowsArray.Length > 0 Then
    '                    rowsArray(0).Item("ICICalcolato") = 0
    '                    rowsArray(0).Item("ICICalcolatoACCONTO") = 0
    '                    rowsArray(0).Item("ICICalcolatoSALDO") = 0
    '                    rowsArray(0).Item("ICI_VALORE_ALIQUOTA") = 0
    '                    'prelevo l'immobile calcolato per id legame
    '                    For Each mySitFinale As objSituazioneFinale In dsUI
    '                        If mySitFinale.Id = i + 1 Then
    '                            rowsArray(0).Item("ICICalcolato") = mySitFinale.TotDovuto
    '                            rowsArray(0).Item("ICICalcolatoACCONTO") = mySitFinale.AccDovuto
    '                            rowsArray(0).Item("ICICalcolatoSALDO") = mySitFinale.SalDovuto
    '                            rowsArray(0).Item("ICI_VALORE_ALIQUOTA") = mySitFinale.Aliquota
    '                            'rows = dsUI.Tables("TP_SITUAZIONE_FINALE_ICI").Select("ID_SITUAZIONE_FINALE='" & i + 1 & "'")
    '                            'aggiorno i valori corrispondenti
    '                            '    If rows.Length > 0 Then
    '                            '    rowsArray(0).Item("ICICalcolato") = rows(0).Item("ICI_TOTALE_DOVUTA")
    '                            '    rowsArray(0).Item("ICICalcolatoACCONTO") = rows(0).Item("ICI_DOVUTA_ACCONTO")
    '                            '    rowsArray(0).Item("ICICalcolatoSALDO") = rows(0).Item("ICI_DOVUTA_SALDO")
    '                            '    rowsArray(0).Item("ICI_VALORE_ALIQUOTA") = rows(0).Item("ICI_VALORE_ALIQUOTA")
    '                            'Else
    '                            '    rowsArray(0).Item("ICICalcolato") = 0
    '                            '    rowsArray(0).Item("ICICalcolatoACCONTO") = 0
    '                            '    rowsArray(0).Item("ICICalcolatoSALDO") = 0
    '                            '    rowsArray(0).Item("ICI_VALORE_ALIQUOTA") = 0
    '                            'End If
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
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertato.CalcoloAccertato.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '        Return False
    '    End Try
    'End Function
    'Private Function maxLegame(ByVal campo As String) As Integer
    '    Try
    '        Dim workTable As New DataTable
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
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertato.maxLegame.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '        maxLegame = 0
    '    End Try
    '    Return maxLegame
    'End Function
    'Private Function addRowsCalcoloICI(ByRef objICI As objSituazioneFinale(), ByVal objHashTable As Hashtable, ByVal rowICI() As DataRow) As Double
    '    'serve per trasformare l'oggetto in videata con quello da passare al servizio di calcolo
    '    Dim row As objSituazioneFinale
    '    Dim FncGen As New ClsGestioneAccertamenti
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
    '        If CDate(rowImmobile(0).Item("DAL")).Year < objHashTable("ANNOACCERTAMENTO") Then
    '            DataInizio = "01/01/" & objHashTable("ANNOACCERTAMENTO")
    '        Else
    '            DataInizio = FncGrd.annoBarra(rowImmobile(0).Item("DAL"))
    '        End If

    '        'AL
    '        If IsDBNull(rowImmobile(0).Item("AL")) Then
    '            DataFine = "31/12/" & objHashTable("ANNOACCERTAMENTO")
    '        ElseIf rowImmobile(0).Item("AL") = "" Then
    '            DataFine = "31/12/" & objHashTable("ANNOACCERTAMENTO")
    '        Else
    '            DataFine = FncGrd.annoBarra(rowImmobile(0).Item("AL"))
    '        End If

    '        mesi_possesso(nMeseTotali, DataInizio, DataFine, 1, objHashTable("ANNOACCERTAMENTO"))

    '        Dim MeseAcconto As Integer
    '        If glbmese_fine_p > 6 Then
    '            MeseAcconto = 6
    '        Else
    '            MeseAcconto = glbmese_fine_p
    '        End If
    '        nMesiAcconto = (MeseAcconto - glbmese_inizio_p) + 1

    '        '**********************************************************************
    '        'Fine Calcolo Mesi
    '        '**********************************************************************

    '        row = New objSituazioneFinale

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
    '            If rowImmobile(0).Item("ID_IMMOBILE_ORIGINALE_DICHIARATO").ToString() = "" Or rowImmobile(0).Item("ID_IMMOBILE_ORIGINALE_DICHIARATO").ToString() = "-1" Then
    '                row.IdImmobile = rowImmobile(0).Item("PROGRESSIVO")
    '            Else
    '                row.IdImmobile = CLng(rowImmobile(0).Item("ID_IMMOBILE_ORIGINALE_DICHIARATO"))
    '            End If

    '            Dim objUtility As New MyUtility

    '            If objUtility.CToStr(rowImmobile(0).Item("IDIMMOBILEPERTINENZA")) = "" Then
    '                row.IdImmobilePertinenza = 0
    '            Else
    '                row.IdImmobilePertinenza = rowImmobile(0).Item("IDIMMOBILEPERTINENZA")
    '            End If
    '        End If

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
    '            row.FlagRiduzione = 1
    '        End If

    '        If rowImmobile(0).Item("FLAG_RIDOTTO") = "True" Or rowImmobile(0).Item("FLAG_RIDOTTO") = "1" Then
    '            row.FlagRiduzione = 0
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
    '        row.Indirizzo = ""

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
    '        row.IdTipoUtilizzo = rowImmobile(0).Item("CODTITPOSSESSO")
    '        row.DataInizio = clsGeneralFunction.ReplaceDataForDB(rowImmobile(0).Item("dal"))
    '        row.Tributo = rowImmobile(0).Item("CODTRIBUTO")
    '        '*** ***
    '        'vale sempre la regola valori invertiti perchè in dichiarato dati possesso
    '        '0=SI
    '        '1=NO
    '        '2=non compilato
    '        If rowImmobile(0).Item("CODTITPOSSESSO") = "7" Then 'Or rowImmobile(0).Item("mesiEsclusioneEsenzione= "0" Then
    '            row.FlagEsente = 1
    '        Else
    '            row.FlagEsente = rowImmobile(0).Item("FLAG_ESENTE")
    '        End If

    '        If rowImmobile(0).Item("mesiEsclusioneEsenzione") > 0 Then
    '            mesi_possesso(row.MesiEsenzione, DataInizio, DataFine, 1, objHashTable("ANNOACCERTAMENTO"))
    '        End If
    '        If CStr(rowImmobile(0).Item("RENDITA")) <> "" Then
    '            row.Rendita = rowImmobile(0).Item("RENDITA")
    '        Else
    '            row.Rendita = 0
    '        End If

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

    '        ''Mi serve per calcolare l'ICI solo su 1 Immobile. Viene passato alla procedura di calcolo
    '        ''dell'ICI. Viene poi distrutto all'uscita del metodo
    '        'Dim objDSImmobiliAppoggio As DataSet
    '        ''Creo la struttura 
    '        'objDSImmobiliAppoggio = FncGen.CreateDSperCalcoloICI()
    '        'objDSImmobiliAppoggio.Tables("TP_SITUAZIONE_FINALE_ICI").ImportRow(row)
    '        'objDSImmobiliAppoggio.AcceptChanges()

    '        'Aggiungo Riga a mio DS x Situazione Finali ICI
    '        ReDim Preserve objICI(objICI.GetLength(0) + 1)
    '        objICI(objICI.GetLength(0)) = row
    '        Return 1
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertato.addRowsCalcoloICI.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '        Return -1
    '    End Try
    'End Function
    'Private Function CalcolaICITotale(ByRef dsUIICI As objSituazioneFinale(), ByVal objHashTable As Hashtable, ByVal TipoCalcolo As Integer) As Boolean
    '    'Calcolo ICI
    '    Try
    '        'Dim objICICalcolato As New DataSet
    '        '*** 20140509 - TASI ***            
    '        Dim objCOMCalcoloICI As IFreezer = Activator.GetObject(GetType(ComPlusInterface.IFreezer), ConstSession.URLServiziFreezer)
    '        dsUIICI = objCOMCalcoloICI.CalcoloICI(dsUIICI, objHashTable, TipoCalcolo)
    '        '*** ***
    '        Return True
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertato.CalcolaICITotale.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '        Return False
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="mesipossesso">mesi di possesso del periodo in input rispetto</param>
    ''' <param name="dal"> Data partenza periodo</param>
    ''' <param name="al">data chiusura periodo</param>
    ''' <param name="tipo_periodo">tipo_periodo: 1 Possesso, 2 Catasto_s, 3 Catasto_p</param>
    ''' <param name="annoAccertamento">anno in esame</param>
    Private Sub mesi_possesso(ByRef mesipossesso As Integer, ByVal dal As String, ByVal al As String, ByVal tipo_periodo As Integer, ByVal annoAccertamento As Integer)

        Dim mese As Integer
        Dim aggiunta_mese As Integer
        Dim data_ultimo_gg_mese As String

        'Azzero le variabili globali
        Select Case tipo_periodo
            Case 1    'Periodo di possesso
                glbmese_inizio_p = 0
                glbmese_fine_p = 0
            Case 2    'Periodo di classamento
                glbmese_inizio_s = 0
                glbmese_fine_s = 0
        End Select
        Try
            If Year(dal) < annoAccertamento Then
                If al = "" Then
                    mesipossesso = 12
                Else
                    If Year(al) > annoAccertamento Then
                        mesipossesso = 12
                    Else
                        If Year(al) < annoAccertamento Then
                            mesipossesso = 0
                        Else
                            'Forzo la data Dal all'inizio anno
                            dal = "01/01" + "/" + Trim(annoAccertamento)
                            'Verifico quanti gg di possosseso ci sono nel mese della Data(al)
                            If Day(al) > 14 Then
                                aggiunta_mese = aggiunta_mese + 1
                            End If
                            'Calcolo i mesi di possesso per l'anno in esame
                            mesipossesso = DateDiff("M", dal, al) + aggiunta_mese
                        End If
                    End If
                End If
                Select Case tipo_periodo
                    Case 1     'Periodo di possesso
                        glbmese_inizio_p = 1
                        glbmese_fine_p = mesipossesso
                    Case 2     'Periodo di classamento
                        glbmese_inizio_s = 1
                        glbmese_fine_s = mesipossesso
                End Select
            Else
                If Year(dal) = annoAccertamento Then
                    'Determino quanti giorni di possesso ci sono nel mese
                    'del dal
                    mese = Month(dal)
                    data_ultimo_gg_mese = Trim(Str(FncGrd.giorni_mese(mese))) + "/" + Trim(Str(mese)) + "/" + Trim(Str(annoAccertamento))
                    'Imposto i mesi di inizio/fine del periodo
                    Select Case tipo_periodo
                        Case 1      'Periodo di possesso
                            glbmese_inizio_p = mese
                            glbmese_fine_p = 12
                        Case 2      'Periodo di classamento
                            glbmese_inizio_s = mese
                            glbmese_fine_s = 12
                    End Select
                    'Verfico se, in presenza di febbraio, ho 28 o 29 gg
                    If mese = 2 Then
                        Try
                            data_ultimo_gg_mese = DateValue(data_ultimo_gg_mese)
                        Catch ex As Exception
                            data_ultimo_gg_mese = "28" + "/" + Trim(Str(mese)) + "/" + Trim(Str(annoAccertamento))
                        End Try
                    End If
                    'Verifico quanti gg di possosseso ci sono nel mese della data Dal
                    If mese = 2 Then
                        If DateDiff("d", dal, data_ultimo_gg_mese) < 14 Then     'modifica 8.1
                            'aggiunto = perchè in caso di un rif. cat
                            aggiunta_mese = -1
                            'secondario dal 16/11/1993 al ........ già di un secondo
                            'Sposto di uno il mese di inizio
                            'proprietario mi calcolava un mese per il primo
                            Select Case tipo_periodo
                                Case 1       'Periodo di possesso
                                    glbmese_inizio_p = Month(dal) + 1
                                Case 2       'Periodo di classamento
                                    glbmese_inizio_s = Month(dal) + 1
                            End Select
                        End If
                    Else
                        If DateDiff("d", dal, data_ultimo_gg_mese) <= 14 Then     'modifica 8.1
                            'aggiunto = perchè in caso di un rif. cat
                            aggiunta_mese = -1
                            'secondario dal 16/11/1993 al ........ già di un secondo
                            'Sposto di uno il mese di inizio
                            'proprietario mi calcolava un mese per il primo
                            Select Case tipo_periodo
                                Case 1       'Periodo di possesso
                                    glbmese_inizio_p = Month(dal) + 1
                                Case 2       'Periodo di classamento
                                    glbmese_inizio_s = Month(dal) + 1
                            End Select
                        End If
                    End If

                    'Verifico data Al
                    If al = "" Then
                        al = "31/12" + "/" + Trim(Str(annoAccertamento))
                    Else
                        If Year(al) > annoAccertamento Then
                            al = "31/12" + "/" + Trim(Str(annoAccertamento))
                        End If
                    End If
                    'Verifico quanti gg di possosseso ci sono nel mese della data Al
                    If Day(al) > 14 Then
                        aggiunta_mese = aggiunta_mese + 1
                        'Inposto il mese di fine nel caso non fosse Dicembre (12)
                        Select Case tipo_periodo
                            Case 1       'Periodo di possesso
                                glbmese_fine_p = Month(al)
                            Case 2       'Periodo di classamento
                                glbmese_fine_s = Month(al)
                        End Select
                    Else
                        'Sposto indietro di uno il mese di fine nel caso il mese in	esame
                        'non avesse i giorni sufficenti da essere considerato
                        Select Case tipo_periodo
                            Case 1       'Periodo di possesso
                                glbmese_fine_p = Month(al) - 1
                            Case 2       'Periodo di classamento
                                glbmese_fine_s = Month(al) - 1
                        End Select
                    End If
                    'Calcolo i mesi di possesso per l'anno in esame
                    mesipossesso = DateDiff("M", dal, al) + aggiunta_mese
                Else
                    mesipossesso = 0
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertato.mesi_possesso.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class