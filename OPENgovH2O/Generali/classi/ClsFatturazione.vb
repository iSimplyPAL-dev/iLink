Imports log4net
Imports RemotingInterfaceMotoreH2O.RemotingInterfaceMotoreH2O
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports AnagInterface
Imports Utility

Public Class ClsCreaFatture
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ClsCreaFatture))
    Private iDB As New DBAccess.getDBobject

    ''' <summary>
    ''' Procedura di calcolo documenti.
    ''' Se è già stata elaborata una fatturazione ma non è stata consolidata, vengono svuotate le tabelle prima di eseguire il calcolo.
    ''' prelevo il tipo di arrotondamento da applicare al consumo secondo configurazione dei periodi
    ''' prelevo le tariffe valide per il periodo
    ''' prelevo le letture valide per il periodo in esame ancora da fatturare
    ''' eseguo il calcolo delle fatture utilizzando un servizio esterno (motore H2O)
    ''' se il calcolo è andato a buon fine salvataggio in banca dati della riga di ruolo e di ogni singolo documento creato
    ''' inclusione nel ruolo dei documenti generati da note/variazioni non ancora emessi
    ''' </summary>
    ''' <param name="sEnte">stringa Codice Ente in lavorazione</param>
    ''' <param name="nPeriodo">ID periodo</param>
    ''' <param name="nIdFlusso">ID flusso</param>
    ''' <param name="sTributo">Codice tributo("9000")</param>
    ''' <param name="sOperatore">Operatore che esegue il calcolo</param>
    ''' <param name="sAnno">Anno</param>
    ''' <returns>un oggetto di tipo ObjTotRuoloFatture contenente il ruolo generato</returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Analisi eventi</em>
    ''' </revision>
    ''' </revisionHistory>
    Public Function CreaRuoloFatture(ByVal sEnte As String, ByVal nPeriodo As Integer, ByVal nIdFlusso As Integer, ByVal sTributo As String, ByVal sOperatore As String, ByVal sAnno As String) As RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti.ObjTotRuoloFatture
        Try
            Dim x As Integer
            Dim FunctionFatture As New ClsFatture
            Dim FunctionRuoloH2O As New ClsRuoloH2O
            Dim FunctionLetture As New ClsLettureFattura
            Dim FunctionTariffe As New ClsTariffe
            Dim TypeOfRI As Type = GetType(IH2O)
            Dim RemoRuoloH2O As IH2O
            Dim oListLettureFatt() As objContatore
            Dim oMyTariffe As New ObjTariffe
            Dim oTotRuoloH2O As New ObjTotRuoloFatture
            Dim FncPeriodo As New TabelleDiDecodifica.DBPeriodo
            Dim nArrotondConsumo As Integer

            'controllo se devo eliminare le fatture già inserite
            'elimino le fatture già inserite
            Log.Debug("ClsCreaFatture::CreaRuoloFatture::richiamo SetFattura")
            If FunctionFatture.SetFattura(Nothing, 2, -1, nIdFlusso) = 0 Then
                Log.Debug("ClsCreaFatture::CreaRuoloFatture::SetFattura ha dato errore")
                Return Nothing
            End If
            Log.Debug("ClsCreaFatture::CreaRuoloFatture::richiamo SetRuoloH2O")
            If FunctionRuoloH2O.SetRuoloH2O(Nothing, 2, nIdFlusso) = 0 Then
                Log.Debug("ClsCreaFatture::CreaRuoloFatture::SetRuoloH2O ha dato errore")
                Return Nothing
            End If
            'prelevo il tipo di arrotondamento da applicare al consumo
            Log.Debug("ClsCreaFatture::CreaRuoloFatture::richiamo GetArrotondamentoConsumo")
            nArrotondConsumo = FncPeriodo.GetArrotondamentoConsumo(nPeriodo)
            If nArrotondConsumo = -1 Then
                Return Nothing
            End If
            'prelevo le tariffe
            Log.Debug("ClsCreaFatture::CreaRuoloFatture::richiamo GetTariffe")
            oMyTariffe = FunctionTariffe.GetTariffe(sEnte, sAnno, oTotRuoloH2O)
            If oMyTariffe Is Nothing Then
                Log.Debug("ClsCreaFatture::CreaRuoloFatture::GetTariffe ha dato errore")
                Return oTotRuoloH2O
            End If
            'prelevo le letture
            Log.Debug("ClsCreaFatture::CreaRuoloFatture::richiamo GetLettureFatt")
            oListLettureFatt = FunctionLetture.GetLettureFatt(sEnte, nPeriodo, sTributo, nArrotondConsumo, oTotRuoloH2O)
            If oListLettureFatt Is Nothing Then
                Log.Debug("ClsCreaFatture::CreaRuoloFatture::GetLettureFatt ha dato errore")
                oTotRuoloH2O.sNote = "Non sono presenti Letture da Fatturare."
            Else
                'attivo il servizio
                RemoRuoloH2O = Activator.GetObject(TypeOfRI, ConfigurationManager.AppSettings("URLServizioRuoloH2O"))
                'eseguo il calcolo delle fatture
                Log.Debug("ClsCreaFatture::CreaRuoloFatture::richiamo il servizio CreaRuoloH2O")
                oTotRuoloH2O = RemoRuoloH2O.CreaRuoloH2O(ConfigurationManager.AppSettings("forzaquotafissa"), ConfigurationManager.AppSettings("TipoCalcoloFattura"), oListLettureFatt, oMyTariffe, sAnno, sOperatore)
                If oTotRuoloH2O Is Nothing Then
                    Log.Debug("ClsCreaFatture::CreaRuoloFatture::il servizio CreaRuoloH2O ha dato errore")
                    Return Nothing
                Else
                    oTotRuoloH2O.sIdEnte = sEnte
                    oTotRuoloH2O.nIdPeriodo = nPeriodo
                    Log.Debug("ClsCreaFatture::CreaRuoloFatture::richiamo SetRuoloH2O per salvataggio")
                    nIdFlusso = FunctionRuoloH2O.SetRuoloH2O(oTotRuoloH2O, 0)
                    If nIdFlusso = 0 Then
                        Log.Debug("ClsCreaFatture::CreaRuoloFatture::SetRuoloH2O per salvataggio ha dato errore")
                        Return Nothing
                    End If
                    oTotRuoloH2O.IdFlusso = nIdFlusso
                    If Not oTotRuoloH2O.oFatture Is Nothing Then
                        For x = 0 To oTotRuoloH2O.oFatture.GetUpperBound(0)
                            oTotRuoloH2O.oFatture(x).nIdFlusso = nIdFlusso
                            Log.Debug("ClsCreaFatture::CreaRuoloFatture::richiamo SetFatturaCompleta")
                            If FunctionFatture.SetFatturaCompleta(oTotRuoloH2O.oFatture(x)) = 0 Then
                                Log.Debug("ClsCreaFatture::CreaRuoloFatture::SetFatturaCompleta ha dato errore")
                                Return Nothing
                            End If
                        Next
                    End If
                End If
            End If

            'aggiorno anche i documenti generati da note/variazioni
            Dim oListVariazioni() As objRicercaVariazione = New ClsRibaltaVar().GetRicercaVariazioni(ConstSession.IdEnte, ClsRibaltaVar.sTypeRicerca.VARFATT_DAGESTIRE, -1, nPeriodo)
            If Not oListVariazioni Is Nothing Then
                If nIdFlusso <= 0 Then
                    oTotRuoloH2O.sIdEnte = sEnte
                    oTotRuoloH2O.nIdPeriodo = nPeriodo
                    oTotRuoloH2O.tDataCalcoli = Now
                    Log.Debug("ClsCreaFatture::CreaRuoloFatture::richiamo SetRuoloH2O per salvataggio solo doc generati da note/variazioni")
                    nIdFlusso = FunctionRuoloH2O.SetRuoloH2O(oTotRuoloH2O, 0)
                    If nIdFlusso = 0 Then
                        Log.Debug("ClsCreaFatture::CreaRuoloFatture::SetRuoloH2O per salvataggio ha dato errore")
                        Return Nothing
                    End If
                    oTotRuoloH2O.IdFlusso = nIdFlusso
                End If
                Log.Debug("ClsCreaFatture::CreaRuoloFatture::richiamo SetRuoloVariazioni")
                If FunctionFatture.SetRuoloVariazioni(sEnte, nPeriodo, nIdFlusso, oTotRuoloH2O) = 0 Then
                    Log.Debug("ClsCreaFatture::CreaRuoloFatture::SetRuoloVariazioni ha dato errore")
                    Return Nothing
                End If
            End If
            Return oTotRuoloH2O
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsCreaFatture.CreaRuoloFatture.errore: ", Err)
            Return Nothing
        End Try
    End Function
    'Public Function CreaRuoloFatture(ByVal sEnte As String, ByVal nPeriodo As Integer, ByVal nIdFlusso As Integer, ByVal sTributo As String, ByVal sOperatore As String, ByVal sAnno As String) As RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti.ObjTotRuoloFatture
    '    Try
    '        Dim x As Integer
    '        Dim FunctionFatture As New ClsFatture
    '        Dim FunctionRuoloH2O As New ClsRuoloH2O
    '        Dim FunctionLetture As New ClsLettureFattura
    '        Dim FunctionTariffe As New ClsTariffe
    '        Dim TypeOfRI As Type = GetType(IH2O)
    '        Dim RemoRuoloH2O As IH2O
    '        Dim oListLettureFatt() As objContatore
    '        Dim oMyTariffe As New ObjTariffe
    '        Dim oTotRuoloH2O As New ObjTotRuoloFatture
    '        Dim FncPeriodo As New TabelleDiDecodifica.DBPeriodo
    '        Dim nArrotondConsumo As Integer

    '        'controllo se devo eliminare le fatture già inserite
    '        'elimino le fatture già inserite
    '        Log.Debug("ClsCreaFatture::CreaRuoloFatture::richiamo SetFattura")
    '        If FunctionFatture.SetFattura(Nothing, 2, -1, nIdFlusso) = 0 Then
    '            Log.Debug("ClsCreaFatture::CreaRuoloFatture::SetFattura ha dato errore")
    '            Return Nothing
    '        End If
    '        Log.Debug("ClsCreaFatture::CreaRuoloFatture::richiamo SetRuoloH2O")
    '        If FunctionRuoloH2O.SetRuoloH2O(Nothing, 2, nIdFlusso) = 0 Then
    '            Log.Debug("ClsCreaFatture::CreaRuoloFatture::SetRuoloH2O ha dato errore")
    '            Return Nothing
    '        End If
    '        'prelevo il tipo di arrotondamento da applicare al consumo
    '        Log.Debug("ClsCreaFatture::CreaRuoloFatture::richiamo GetArrotondamentoConsumo")
    '        nArrotondConsumo = FncPeriodo.GetArrotondamentoConsumo(nPeriodo)
    '        If nArrotondConsumo = -1 Then
    '            Return Nothing
    '        End If
    '        'prelevo le tariffe
    '        Log.Debug("ClsCreaFatture::CreaRuoloFatture::richiamo GetTariffe")
    '        oMyTariffe = FunctionTariffe.GetTariffe(sEnte, sAnno, oTotRuoloH2O)
    '        If oMyTariffe Is Nothing Then
    '            Log.Debug("ClsCreaFatture::CreaRuoloFatture::GetTariffe ha dato errore")
    '            Return oTotRuoloH2O
    '        End If
    '        'prelevo le letture
    '        Log.Debug("ClsCreaFatture::CreaRuoloFatture::richiamo GetLettureFatt")
    '        oListLettureFatt = FunctionLetture.GetLettureFatt(sEnte, nPeriodo, sTributo, nArrotondConsumo, oTotRuoloH2O)
    '        If oListLettureFatt Is Nothing Then
    '            Log.Debug("ClsCreaFatture::CreaRuoloFatture::GetLettureFatt ha dato errore")
    '            oTotRuoloH2O.sNote = "Non sono presenti Letture da Fatturare."
    '            'Return oTotRuoloH2O
    '        End If
    '        'attivo il servizio
    '        RemoRuoloH2O = Activator.GetObject(TypeOfRI, ConfigurationManager.AppSettings("URLServizioRuoloH2O"))
    '        'eseguo il calcolo delle fatture
    '        Log.Debug("ClsCreaFatture::CreaRuoloFatture::richiamo il servizio CreaRuoloH2O")
    '        oTotRuoloH2O = RemoRuoloH2O.CreaRuoloH2O(ConfigurationManager.AppSettings("forzaquotafissa"), ConfigurationManager.AppSettings("TipoCalcoloFattura"), oListLettureFatt, oMyTariffe, sAnno, sOperatore)
    '        If oTotRuoloH2O Is Nothing Then
    '            Log.Debug("ClsCreaFatture::CreaRuoloFatture::il servizio CreaRuoloH2O ha dato errore")
    '            Return Nothing
    '        Else
    '            oTotRuoloH2O.sIdEnte = sEnte
    '            oTotRuoloH2O.nIdPeriodo = nPeriodo
    '            Log.Debug("ClsCreaFatture::CreaRuoloFatture::richiamo SetRuoloH2O per salvataggio")
    '            nIdFlusso = FunctionRuoloH2O.SetRuoloH2O(oTotRuoloH2O, 0)
    '            If nIdFlusso = 0 Then
    '                Log.Debug("ClsCreaFatture::CreaRuoloFatture::SetRuoloH2O per salvataggio ha dato errore")
    '                Return Nothing
    '            End If
    '            oTotRuoloH2O.IdFlusso = nIdFlusso
    '            If Not oTotRuoloH2O.oFatture Is Nothing Then
    '                For x = 0 To oTotRuoloH2O.oFatture.GetUpperBound(0)
    '                    oTotRuoloH2O.oFatture(x).nIdFlusso = nIdFlusso
    '                    Log.Debug("ClsCreaFatture::CreaRuoloFatture::richiamo SetFatturaCompleta")
    '                    If FunctionFatture.SetFatturaCompleta(oTotRuoloH2O.oFatture(x)) = 0 Then
    '                        Log.Debug("ClsCreaFatture::CreaRuoloFatture::SetFatturaCompleta ha dato errore")
    '                        Return Nothing
    '                    End If
    '                Next
    '            End If
    '        End If
    '        'aggiorno anche i documenti generati da note/variazioni
    '        Log.Debug("ClsCreaFatture::CreaRuoloFatture::richiamo SetRuoloVariazioni")
    '        If FunctionFatture.SetRuoloVariazioni(sEnte, nPeriodo, nIdFlusso, oTotRuoloH2O) = 0 Then
    '            Log.Debug("ClsCreaFatture::CreaRuoloFatture::SetRuoloVariazioni ha dato errore")
    '            Return Nothing
    '        End If
    '        Return oTotRuoloH2O
    '    Catch Err As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsCreaFatture.CreaRuoloFatture.errore: ", Err)
    '        Return Nothing
    '    End Try
    'End Function
    'Public Function CreaRuoloFatture(ByVal sEnte As String, ByVal nPeriodo As Integer, ByVal nIdFlusso As Integer, ByVal sTributo As String, ByVal sOperatore As String, ByVal sAnno As String) As RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti.ObjTotRuoloFatture
    '    Dim culture As IFormatProvider
    '    culture = New System.Globalization.CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")
    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Try
    '        Dim WFErrore As String
    '        Dim x As Integer
    '        Dim FunctionFatture As New ClsFatture
    '        Dim FunctionRuoloH2O As New ClsRuoloH2O
    '        Dim FunctionLetture As New ClsLettureFattura
    '        Dim FunctionTariffe As New ClsTariffe
    '        Dim TypeOfRI As Type = GetType(IH2O)
    '        Dim RemoRuoloH2O As IH2O
    '        Dim oListLettureFatt() As objContatore
    '        Dim oMyTariffe As New ObjTariffe
    '        Dim oTotRuoloH2O As New ObjTotRuoloFatture
    '        Dim FncPeriodo As New TabelleDiDecodifica.DBPeriodo
    '        Dim nArrotondConsumo As Integer

    '        'inizializzo la connessione
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If
    '        'controllo se devo eliminare le fatture già inserite
    '        'elimino le fatture già inserite
    '        Log.Debug("ClsCreaFatture::CreaRuoloFatture::richiamo SetFattura")
    '        If FunctionFatture.SetFattura(Nothing, 2, -1, WFSessione, nIdFlusso) = 0 Then
    '            Log.Debug("ClsCreaFatture::CreaRuoloFatture::SetFattura ha dato errore")
    '            Return Nothing
    '        End If
    '        Log.Debug("ClsCreaFatture::CreaRuoloFatture::richiamo SetRuoloH2O")
    '        If FunctionRuoloH2O.SetRuoloH2O(Nothing, 2, WFSessione, nIdFlusso) = 0 Then
    '            Log.Debug("ClsCreaFatture::CreaRuoloFatture::SetRuoloH2O ha dato errore")
    '            Return Nothing
    '        End If
    '        'prelevo il tipo di arrotondamento da applicare al consumo
    '        Log.Debug("ClsCreaFatture::CreaRuoloFatture::richiamo GetArrotondamentoConsumo")
    '        nArrotondConsumo = FncPeriodo.GetArrotondamentoConsumo(nPeriodo, WFSessione)
    '        If nArrotondConsumo = -1 Then
    '            Return Nothing
    '        End If
    '        'prelevo le tariffe
    '        Log.Debug("ClsCreaFatture::CreaRuoloFatture::richiamo GetTariffe")
    '        oMyTariffe = FunctionTariffe.GetTariffe(sEnte, sAnno, oTotRuoloH2O)
    '        If oMyTariffe Is Nothing Then
    '            Log.Debug("ClsCreaFatture::CreaRuoloFatture::GetTariffe ha dato errore")
    '            Return oTotRuoloH2O
    '        End If
    '        'prelevo le letture
    '        Log.Debug("ClsCreaFatture::CreaRuoloFatture::richiamo GetLettureFatt")
    '        oListLettureFatt = FunctionLetture.GetLettureFatt(sEnte, nPeriodo, sTributo, nArrotondConsumo, WFSessione, oTotRuoloH2O)
    '        If oListLettureFatt Is Nothing Then
    '            Log.Debug("ClsCreaFatture::CreaRuoloFatture::GetLettureFatt ha dato errore")
    '            oTotRuoloH2O.sNote = "Non sono presenti Letture da Fatturare."
    '            'Return oTotRuoloH2O
    '        End If
    '        'inizializzo la connessione
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If
    '        'attivo il servizio
    '        RemoRuoloH2O = Activator.GetObject(TypeOfRI, ConfigurationManager.AppSettings("URLServizioRuoloH2O"))
    '        'eseguo il calcolo delle fatture
    '        Log.Debug("ClsCreaFatture::CreaRuoloFatture::richiamo il servizio CreaRuoloH2O")
    '        oTotRuoloH2O = RemoRuoloH2O.CreaRuoloH2O(ConfigurationManager.AppSettings("forzaquotafissa"), ConfigurationManager.AppSettings("TipoCalcoloFattura"), oListLettureFatt, oMyTariffe, sAnno, sOperatore)
    '        If oTotRuoloH2O Is Nothing Then
    '            Log.Debug("ClsCreaFatture::CreaRuoloFatture::il servizio CreaRuoloH2O ha dato errore")
    '            Return Nothing
    '        Else
    '            oTotRuoloH2O.sIdEnte = sEnte
    '            oTotRuoloH2O.nIdPeriodo = nPeriodo
    '            Log.Debug("ClsCreaFatture::CreaRuoloFatture::richiamo SetRuoloH2O per salvataggio")
    '            nIdFlusso = FunctionRuoloH2O.SetRuoloH2O(oTotRuoloH2O, 0, WFSessione)
    '            If nIdFlusso = 0 Then
    '                Log.Debug("ClsCreaFatture::CreaRuoloFatture::SetRuoloH2O per salvataggio ha dato errore")
    '                Return Nothing
    '            End If
    '            oTotRuoloH2O.IdFlusso = nIdFlusso
    '            If Not oTotRuoloH2O.oFatture Is Nothing Then
    '                For x = 0 To oTotRuoloH2O.oFatture.GetUpperBound(0)
    '                    oTotRuoloH2O.oFatture(x).nIdFlusso = nIdFlusso
    '                    Log.Debug("ClsCreaFatture::CreaRuoloFatture::richiamo SetFatturaCompleta")
    '                    If FunctionFatture.SetFatturaCompleta(oTotRuoloH2O.oFatture(x), WFSessione) = 0 Then
    '                        Log.Debug("ClsCreaFatture::CreaRuoloFatture::SetFatturaCompleta ha dato errore")
    '                        Return Nothing
    '                    End If
    '                Next
    '            End If
    '        End If
    '        'aggiorno anche i documenti generati da note/variazioni
    '        Log.Debug("ClsCreaFatture::CreaRuoloFatture::richiamo SetRuoloVariazioni")
    '        If FunctionFatture.SetRuoloVariazioni(sEnte, nPeriodo, nIdFlusso, oTotRuoloH2O, WFSessione) = 0 Then
    '            Log.Debug("ClsCreaFatture::CreaRuoloFatture::SetRuoloVariazioni ha dato errore")
    '            Return Nothing
    '        End If
    '        Return oTotRuoloH2O
    '    Catch Err As Exception
    '         Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsCreaFatture.CreaRuoloFatture.errore: ", Err)
    '        Return Nothing
    '    Finally
    '        WFSessione.Kill()
    '    End Try
    'End Function

    Public Function NumeraDocumenti(ByVal oMyRuolo As ObjTotRuoloFatture, ByVal sNPartenzaDoc As String, ByVal sPrefissoNDoc As String, ByVal sSuffissoNDoc As String, ByVal sTributo As String, ByVal sUser As String) As Integer
        Dim oContoCorrente As New OPENUtility.objContoCorrente
        Dim FunctionConto As New OPENUtility.ClsContiCorrenti
        Dim FunctionFatture As New ClsFatture
        Dim FunctionRuolo As New ClsRuoloH2O

        Try
            oContoCorrente = FunctionConto.GetContoCorrente(oMyRuolo.sIdEnte, sTributo, sUser, ConstSession.StringConnectionOPENgov)
            If oContoCorrente Is Nothing Then
                oMyRuolo.sNote = "Conto Corrente non configurato."
                Return 0
            End If
            'controllo se il numero fattura è già presente per l'anno della data fattura
            Log.Debug("ClsCreaFatture::NumeraDocumenti::controllo se il numero fattura è già presente per l'anno della data fattura")
            If FunctionFatture.CheckIdentificativiDoc(oMyRuolo.sIdEnte, oMyRuolo.tDataEmissioneFattura, sNPartenzaDoc) = 0 Then
                Return 0
            End If
            'attribuisco la numerazione
            Log.Debug("ClsCreaFatture::NumeraDocumenti::attribuisco la numerazione")
            If FunctionFatture.SetIdentificativiDoc(oMyRuolo.sIdEnte, oMyRuolo.IdFlusso, sTributo, oMyRuolo.tDataEmissioneFattura, sNPartenzaDoc, sPrefissoNDoc, sSuffissoNDoc, oMyRuolo.tDataScadenza, oContoCorrente.ContoCorrente) = 0 Then
                Return 0
            End If
            'aggiorno la tabella dei totalizzatori
            Log.Debug("ClsCreaFatture::NumeraDocumenti::aggiorno la tabella dei totalizzatori")
            If FunctionRuolo.SetDateRuoliH2OGenerati(oMyRuolo.IdFlusso, 2) = 0 Then
                Return 0
            End If
            If FunctionRuolo.SetDateRuoliH2OGenerati(oMyRuolo.IdFlusso, 3, "I", oMyRuolo.tDataEmissioneFattura.ToShortDateString) = 0 Then
                Return 0
            End If
            If FunctionRuolo.SetDateRuoliH2OGenerati(oMyRuolo.IdFlusso, 4, "I", oMyRuolo.tDataScadenza.ToShortDateString) = 0 Then
                Return 0
            End If
            Return 1
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsCreaFatture.NumeraDocumenti.errore: ", Err)
            Return 0
        End Try
    End Function
    'Public Function NumeraDocumenti(ByVal oMyRuolo As ObjTotRuoloFatture, ByVal sNPartenzaDoc As String, ByVal sPrefissoNDoc As String, ByVal sSuffissoNDoc As String, ByVal sTributo As String, ByVal sUser As String) As Integer
    '    Dim culture As IFormatProvider
    '    culture = New System.Globalization.CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")
    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Try
    '        Dim WFErrore As String
    '        Dim oContoCorrente As New OPENUtility.objContoCorrente
    '        Dim FunctionConto As New OPENUtility.ClsContiCorrenti
    '        Dim FunctionFatture As New ClsFatture
    '        Dim FunctionRuolo As New ClsRuoloH2O

    '        oContoCorrente = FunctionConto.GetContoCorrente(oMyRuolo.sIdEnte, sTributo, sUser)
    '        If oContoCorrente Is Nothing Then
    '            oMyRuolo.sNote = "Conto Corrente non configurato."
    '            Return 0
    '        End If
    '        'inizializzo la connessione
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If
    '        'controllo se il numero fattura è già presente per l'anno della data fattura
    '        Log.Debug("ClsCreaFatture::NumeraDocumenti::controllo se il numero fattura è già presente per l'anno della data fattura")
    '        If FunctionFatture.CheckIdentificativiDoc(oMyRuolo.sIdEnte, oMyRuolo.tDataEmissioneFattura, sNPartenzaDoc, WFSessione) = 0 Then
    '            Return 0
    '        End If
    '        'attribuisco la numerazione
    '        Log.Debug("ClsCreaFatture::NumeraDocumenti::attribuisco la numerazione")
    '        If FunctionFatture.SetIdentificativiDoc(oMyRuolo.sIdEnte, oMyRuolo.IdFlusso, sTributo, oMyRuolo.tDataEmissioneFattura, sNPartenzaDoc, sPrefissoNDoc, sSuffissoNDoc, oMyRuolo.tDataScadenza, oContoCorrente.ContoCorrente, WFSessione) = 0 Then
    '            Return 0
    '        End If
    '        'aggiorno la tabella dei totalizzatori
    '        Log.Debug("ClsCreaFatture::NumeraDocumenti::aggiorno la tabella dei totalizzatori")
    '        If FunctionRuolo.SetDateRuoliH2OGenerati(oMyRuolo.IdFlusso, 2) = 0 Then
    '            Return 0
    '        End If
    '        If FunctionRuolo.SetDateRuoliH2OGenerati(oMyRuolo.IdFlusso, 3, "I", oMyRuolo.tDataEmissioneFattura.ToShortDateString) = 0 Then
    '            Return 0
    '        End If
    '        If FunctionRuolo.SetDateRuoliH2OGenerati(oMyRuolo.IdFlusso, 4, "I", oMyRuolo.tDataScadenza.ToShortDateString) = 0 Then
    '            Return 0
    '        End If
    '        Return 1
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsCreaFatture.NumeraDocumenti.errore: ", Err)
    '        Return 0
    '    Finally
    '        WFSessione.Kill()
    '    End Try
    'End Function

    'Public Function Crea290(ByVal nFlusso As Integer, ByVal sIdEnte As String, ByVal sIdEnteCredBen As String, ByVal sIdEnteCNC As String, ByVal nProgRuolo As String, ByVal nRate As Integer, ByVal sPathNameFile As String, ByRef sNomeErr As String) As Integer
    '    Dim oListFatture() As ObjFattura
    '    Dim FncFatture As New ClsFatture
    '    Dim FunctionRuolo As New ClsRuoloH2O
    '    Dim FncEstraz As New Esportazione290
    '    Dim x As Integer
    '    Dim oMyN0 As New Importer290.N0
    '    Dim oListN1() As Importer290.N1
    '    Dim oMyN1 As Importer290.N1
    '    Dim nListN1 As Integer = -1
    '    Dim oMyN2 As Importer290.N2
    '    Dim nListN2 As Integer = -1
    '    Dim oListN4() As Importer290.N4
    '    Dim oMyN4 As Importer290.N4
    '    Dim nListN4 As Integer = -1
    '    Dim oMyN5 As Importer290.N5
    '    Dim oListN5() As Importer290.N5
    '    Dim nListN5 As Integer = -1
    '    Dim oMyN9 As Importer290.N9
    '    Dim nTotN0 As Integer = 0
    '    Dim nTotN1 As Integer = 0
    '    Dim nTotN2 As Integer = 0
    '    Dim nTotN4 As Integer = 0
    '    Dim nTotN5 As Integer = 0
    '    Dim nTotN9 As Integer = 0
    '    Dim sIdEntePrec As String = ""
    '    Dim nIdPrec As Integer = -1
    '    Dim nProgPartita As Integer = 0
    '    Dim impTot290 As Double
    '    Dim WFSessione, WFSessione_ENTE As OPENUtility.CreateSessione
    '    Dim sCodTributo, WFErrore As String

    '    Try
    '        'Connessione principale per il reperimento dati 290
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If
    '        'Connessione per la ricerca dei dati relativi al comune
    '        WFSessione_ENTE = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConfigurationManager.AppSettings("OPENGOVG").ToString())
    '        If Not WFSessione_ENTE.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        oListFatture = FncFatture.GetFattura(sIdEnte, nFlusso, -1, False, WFSessione)
    '        If oListFatture Is Nothing Then
    '            Return 0
    '        End If
    '        If Not oListFatture Is Nothing Then
    '            'popolo il record N0
    '            oMyN0 = FncEstraz.PopolaN0(sIdEnteCredBen)
    '            If Not oMyN0 Is Nothing Then
    '                'incremento il totale dei record
    '                nTotN0 += 1
    '            Else
    '                Return -1
    '            End If
    '            'ciclo su tutti gli articoli trovati e preparo il 290
    '            For x = 0 To oListFatture.GetUpperBound(0)
    '                'controllo se devo creare un nuovo N1
    '                If oListFatture(x).sIdEnte <> sIdEntePrec Then
    '                    'controllo se devo chiudere l'N1 precedente
    '                    If sIdEntePrec <> "" Then
    '                        'incremento il totale dei record
    '                        nTotN5 += 1
    '                        nListN5 += 1
    '                        ReDim Preserve oListN5(nListN5)
    '                        oMyN5 = New Importer290.N5
    '                        'popolo il record N5
    '                        oMyN5 = FncEstraz.PopolaN5(sIdEnteCNC, nTotN1, nTotN2, 0, nTotN4, nTotN5, impTot290)
    '                        If Not oMyN5 Is Nothing Then
    '                            oListN5(nListN5) = oMyN5
    '                        Else
    '                            Return -1
    '                        End If
    '                    End If
    '                    'popolo il record N1
    '                    nListN1 += 1
    '                    ReDim Preserve oListN1(nListN1)
    '                    oMyN1 = New Importer290.N1
    '                    oMyN1 = FncEstraz.PopolaN1(sIdEnteCNC, nProgRuolo, nRate)
    '                    If Not oMyN1 Is Nothing Then
    '                        oListN1(nListN1) = oMyN1
    '                        'incremento il totale dei record
    '                        nTotN1 += 1
    '                    Else
    '                        Return -1
    '                    End If
    '                End If
    '                'aggiungo un nuovo N4
    '                nListN4 += 1
    '                ReDim Preserve oListN4(nListN4)
    '                oMyN4 = New Importer290.N4
    '                'controllo se devo creare un nuovo N2
    '                sCodTributo = MyUtility.TRIBUTO_H2O
    '                If oListFatture(x).Id <> nIdPrec Then
    '                    nProgPartita += 1
    '                    oMyN2 = FncEstraz.PopolaN2(oListFatture(x).nIdUtente, sCodTributo, sIdEnteCNC, nProgPartita, WFSessione, WFSessione_ENTE, sNomeErr)
    '                    If Not oMyN2 Is Nothing Then
    '                        'incremento il totale dei record
    '                        nTotN2 += 1
    '                    Else
    '                        Return -2
    '                    End If
    '                End If
    '                oMyN4 = FncEstraz.PopolaN4(oListFatture(x), sIdEnteCNC, nProgPartita, sCodTributo, oListFatture(x).impImponibile, oMyN2)
    '                If Not oMyN4 Is Nothing Then
    '                    oListN4(nListN4) = oMyN4
    '                    'incremento il totale dei record
    '                    nTotN4 += 1
    '                    impTot290 += oListFatture(x).impImponibile
    '                Else
    '                    Return -1
    '                End If
    '                'controllo se inserire il record per l'iva
    '                sCodTributo = MyUtility.TRIBUTO_IVA
    '                'aggiungo un nuovo N4
    '                nListN4 += 1
    '                ReDim Preserve oListN4(nListN4)
    '                oMyN4 = New Importer290.N4
    '                oMyN4.oN2 = oMyN2
    '                oMyN4 = FncEstraz.PopolaN4(oListFatture(x), sIdEnteCNC, nProgPartita, sCodTributo, oListFatture(x).impIva, oMyN2)
    '                If Not oMyN4 Is Nothing Then
    '                    oListN4(nListN4) = oMyN4
    '                    'incremento il totale dei record
    '                    nTotN4 += 1
    '                    impTot290 += oListFatture(x).impIva
    '                Else
    '                    Return -1
    '                End If

    '                nIdPrec = oListFatture(x).Id
    '                sIdEntePrec = oListFatture(x).sIdEnte
    '            Next
    '            'incremento il totale dei record
    '            nTotN5 += 1
    '            nListN5 += 1
    '            ReDim Preserve oListN5(nListN5)
    '            oMyN5 = New Importer290.N5
    '            'popolo il record N5
    '            oMyN5 = FncEstraz.PopolaN5(sIdEnteCNC, nTotN1, nTotN2, 0, nTotN4, nTotN5, impTot290)
    '            If Not oMyN5 Is Nothing Then
    '                oListN5(nListN5) = oMyN5
    '            Else
    '                Return -1
    '            End If
    '            'incremento il totale dei record
    '            nTotN9 += 1
    '            'popolo il record N9
    '            oMyN9 = FncEstraz.PopolaN9(sIdEnteCredBen, nTotN0, nTotN1, nTotN2, 0, nTotN4, nTotN5, nTotN9)
    '            If oMyN9 Is Nothing Then
    '                Return -1
    '            End If

    '            'scrivo il file
    '            If FncEstraz.Create290(sPathNameFile, oMyN0, oListN1, oListN4, oListN5, oMyN9) < 1 Then
    '                Return -1
    '            Else
    '                If FunctionRuolo.SetDateRuoliH2OGenerati(oListFatture(0).nIdFlusso, 5) = 0 Then
    '                    Return 0
    '                End If
    '            End If
    '        Else
    '            Return 0
    '        End If

    '        Return 1
    '    Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsCreaFatture.Crea290.errore: ", Err)
    '        Return -1
    '    Finally
    '        WFSessione.oOM.Terminate()
    '        WFSessione.oSession.Terminate()
    '        WFSessione.oSM.Terminate()
    '        WFSessione = Nothing

    '        WFSessione_ENTE.oOM.Terminate()
    '        WFSessione_ENTE.oSession.Terminate()
    '        WFSessione_ENTE.oSM.Terminate()
    '        WFSessione_ENTE = Nothing
    '    End Try
    'End Function
End Class

Public Class ClsLettureFattura
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ClsLettureFattura))
    Private iDB As New DBAccess.getDBobject
    Private oReplace As New ClsGenerale.Generale

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sEnte"></param>
    ''' <param name="nPeriodo"></param>
    ''' <param name="sTributo"></param>
    ''' <param name="nTipoArrotondConsumo"></param>
    ''' <param name="oRuoloH2O"></param>
    ''' <param name="nContatore"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetLettureFatt(ByVal sEnte As String, ByVal nPeriodo As Integer, ByVal sTributo As String, ByVal nTipoArrotondConsumo As Integer, ByRef oRuoloH2O As ObjTotRuoloFatture, Optional ByVal nContatore As Integer = -1) As objContatore()
        Dim sSQL As String = ""
        Dim nListCont As Integer = -1
        Dim nListLett As Integer = -1
        Dim nContatorePrec As Integer = -1
        Dim oListContatori() As objContatore = Nothing
        Dim oMyContatore As New objContatore
        Dim oMyLettura As New ObjLettura
        Dim FncAnagrafica As New Anagrafica.DLL.GestioneAnagrafica
        Dim myCultureInfo As New System.Globalization.CultureInfo("it-IT", True)

        Try
            Dim oDbManagerRepository As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
            Dim dvMyDati As New DataView
            Try
                Using ctx As DBModel = oDbManagerRepository
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_GetLettureFat", "IDENTE", "IDPERIODO", "IDCONTATORE")
                    dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", sEnte) _
                                , ctx.GetParam("IDPERIODO", nPeriodo) _
                                , ctx.GetParam("IDCONTATORE", nContatore)
                            )
                    ctx.Dispose()
                End Using
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        If StringOperation.FormatInt(myRow("codcontatore")) <> nContatorePrec And nContatorePrec <> -1 Then
                            'dimensiono l'array
                            nListCont += 1
                            ReDim Preserve oListContatori(nListCont)
                            'memorizzo i dati nell'array
                            oListContatori(nListCont) = oMyContatore
                            oMyContatore = New objContatore
                            nListLett = -1
                        End If

                        'incremento l'indice
                        nListLett += 1
                        oMyLettura = New ObjLettura
                        oMyLettura.IdLettura = StringOperation.FormatInt(myRow("codlettura"))
                        oMyLettura.nIdPeriodo = StringOperation.FormatInt(myRow("codperiodo"))
                        oMyLettura.nIdContatore = StringOperation.FormatInt(myRow("codcontatore"))
                        oMyLettura.nIdUtente = StringOperation.FormatInt(myRow("idutente"))
                        oMyLettura.sNUtente = StringOperation.FormatString(myRow("numeroutente"))
                        If StringOperation.FormatString(myRow("datalettura")) <> "" Then
                            oMyLettura.tDataLetturaAtt = DateTime.Parse(oReplace.GiraDataFromDB(myRow("datalettura")), myCultureInfo)
                        End If
                        oMyLettura.nLetturaAtt = StringOperation.FormatInt(myRow("lettura"))
                        If StringOperation.FormatString(myRow("dataletturaprecedente")) <> "" Then
                            oMyLettura.tDataLetturaPrec = DateTime.Parse(oReplace.GiraDataFromDB(myRow("dataletturaprecedente")), myCultureInfo)
                        Else
                            Log.Debug("ClsLettureFattura::GetLettureFatt::Data Lettura Precedente mancante::oMyLettura.IdLettura::" & oMyLettura.IdLettura)
                            oRuoloH2O.sNote = "Data Lettura Precedente mancante."
                            Return Nothing
                        End If
                        oMyLettura.nLetturaPrec = StringOperation.FormatInt(myRow("letturaprecedente"))
                        oMyLettura.nConsumoSubContatore = StringOperation.FormatInt(myRow("subconsumo"))
                        oMyLettura.nGiorni = StringOperation.FormatInt(myRow("giornidiconsumo"))

                        If nContatorePrec = -1 Then
                            oMyContatore = New objContatore
                        End If
                        oMyContatore.sIdEnte = StringOperation.FormatString(myRow("codente"))
                        oMyContatore.nIdIntestatario = StringOperation.FormatInt(myRow("idintestatario"))
                        oMyContatore.nIdUtente = StringOperation.FormatInt(myRow("idutente"))
                        oMyContatore.oAnagUtente = FncAnagrafica.GetAnagrafica(oMyLettura.nIdUtente, -1, String.Empty, ConstSession.DBType, ConstSession.StringConnectionAnagrafica, False)
                        oMyContatore.nTipoContatore = StringOperation.FormatInt(myRow("idtipocontatore"))
                        oMyContatore.nTipoUtenza = StringOperation.FormatInt(myRow("idtipoutenza"))
                        oMyContatore.nNumeroUtenze = StringOperation.FormatInt(myRow("numeroutenze"))
                        oMyContatore.sMatricola = StringOperation.FormatString(myRow("matricola"))
                        oMyContatore.sUbicazione = StringOperation.FormatString(myRow("via_ubicazione"))
                        oMyContatore.sCivico = StringOperation.FormatString(myRow("civico_ubicazione"))
                        oMyContatore.sFrazione = StringOperation.FormatString(myRow("frazione_ubicazione"))
                        oMyContatore.nCodDepurazione = StringOperation.FormatInt(myRow("coddepurazione"))
                        oMyContatore.bEsenteAcqua = StringOperation.FormatBool(myRow("esenteacqua"))
                        oMyContatore.bEsenteDepurazione = StringOperation.FormatBool(myRow("esentedepurazione"))
                        oMyContatore.nCodFognatura = StringOperation.FormatInt(myRow("codfognatura"))
                        oMyContatore.bEsenteFognatura = StringOperation.FormatBool(myRow("esentefognatura"))
                        oMyContatore.nFondoScala = StringOperation.FormatInt(myRow("valorefondoscala"))
                        oMyContatore.bEsenteAcquaQF = StringOperation.FormatBool(myRow("ESENTEACQUAQF"))
                        oMyContatore.bEsenteDepQF = StringOperation.FormatBool(myRow("ESENTEDEPURAZIONEQF"))
                        oMyContatore.bEsenteFogQF = StringOperation.FormatBool(myRow("ESENTEFOGNATURAQF"))
                        Log.Debug("ClsLettureFattura::GetLettureFatt::prelevato tutti i dati per fatturare la lettura::oMyLettura.IdLettura::" & oMyLettura.IdLettura)
                        ReDim Preserve oMyContatore.oListLetture(nListLett)
                        oMyContatore.oListLetture(nListLett) = oMyLettura
                        nContatorePrec = StringOperation.FormatInt(myRow("codcontatore"))
                    Next
                End If
            Catch ex As Exception
                Log.Debug(sEnte + " - OPENgovH2O.ClsLettureFattura.GetLettureFatt.query.errore: ", ex)
            Finally
                dvMyDati.Dispose()
            End Try
            If oMyContatore.sIdEnte <> "" Then
                'dimensiono l'array
                nListCont += 1
                ReDim Preserve oListContatori(nListCont)
                'memorizzo i dati nell'array
                oListContatori(nListCont) = oMyContatore
            End If

            Return oListContatori
        Catch Err As Exception
            Log.Debug(sEnte + " - OPENgovH2O.ClsLettureFattura.GetLettureFatt.errore: ", Err)
            Return Nothing
        End Try
    End Function
    'Public Function GetLettureFatt(ByVal sEnte As String, ByVal nPeriodo As Integer, ByVal sTributo As String, ByVal nTipoArrotondConsumo As Integer, ByRef oRuoloH2O As ObjTotRuoloFatture, Optional ByVal nContatore As Integer = -1) As objContatore()
    '    Dim sSQL As String=""
    '    Dim dvMyDati As new dataview=nothing
    '    Dim nListCont As Integer = -1
    '    Dim nListLett As Integer = -1
    '    Dim nContatorePrec As Integer = -1
    '    Dim oListContatori() As objContatore
    '    Dim oMyContatore As objContatore
    '    Dim oMyLettura As ObjLettura
    '    Dim FncAnagrafica As New Anagrafica.DLL.GestioneAnagrafica

    '    Try
    '        '*** 20111202 - a volte su prima lettura rimane data sporca e prima lettura non valorizzato, lo forzo altrimenti il calcolo da errore ***
    '        sSQL = "UPDATE TP_LETTURE SET PRIMALETTURA=1, DATALETTURAPRECEDENTE='' WHERE DATALETTURAPRECEDENTE='00.0.'"
    '        'eseguo la query
    '        Log.Debug("ClsLettureFattura::GetLettureFatt::20111202 - a volte su prima lettura rimane data sporca e prima lettura non valorizzato, lo forzo altrimenti il calcolo da errore::SQL::" & sSQL)
    '        iDB.ExecuteNonQuery(sSQL)
    '        '*** ***

    '        '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
    '        sSQL = "SELECT *"
    '        sSQL += " FROM OPENgov_LETTUREFAT"
    '        sSQL += " WHERE 1=1"
    '        sSQL += " AND (CODENTE='" & sEnte & "')"
    '        sSQL += " AND (CODPERIODO=" & nPeriodo & ") "
    '        If nContatore <> -1 Then
    '            sSQL += " A(TP_CONTATORI.CODCONTATORE =" & nContatore & ")"
    '        End If
    '        sSQL += " ORDER BY IDUTENTE, MATRICOLA, DATALETTURA"
    '        '*** ***
    '        'eseguo la query
    '        Log.Debug("ClsLettureFattura::GetLettureFatt::SQL::" & sSQL)
    '        dvMyDati = iDB.getdataview(sSQL)
    '        Do While dvMyDati.Read
    '            If StringOperation.Formatint(myrow("codcontatore")) <> nContatorePrec And nContatorePrec <> -1 Then
    '                'dimensiono l'array
    '                nListCont += 1
    '                ReDim Preserve oListContatori(nListCont)
    '                'memorizzo i dati nell'array
    '                oListContatori(nListCont) = oMyContatore
    '                oMyContatore = New objContatore
    '                nListLett = -1
    '            End If

    '            'incremento l'indice
    '            nListLett += 1
    '            oMyLettura = New ObjLettura
    '            oMyLettura.IdLettura = StringOperation.Formatint(myrow("codlettura"))
    '            'Log.Debug("GetLettureFatt::tratto codlettura::" & oMyLettura.IdLettura.ToString)
    '            oMyLettura.nIdPeriodo = StringOperation.Formatint(myrow("codperiodo"))
    '            oMyLettura.nIdContatore = StringOperation.Formatint(myrow("codcontatore"))
    '            If Not IsDBNull(myrow("idutente")) Then
    '                oMyLettura.nIdUtente = StringOperation.Formatint(myrow("idutente"))
    '            End If
    '            If Not IsDBNull(myrow("numeroutente")) Then
    '                oMyLettura.sNUtente = StringOperation.Formatstring(myrow("numeroutente"))
    '            End If
    '            If Not IsDBNull(myrow("datalettura")) Then
    '                If StringOperation.Formatstring(myrow("datalettura")) <> "" Then
    '                    oMyLettura.tDataLetturaAtt = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("datalettura")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("lettura")) Then
    '                oMyLettura.nLetturaAtt = StringOperation.Formatint(myrow("lettura"))
    '            End If
    '            If Not IsDBNull(myrow("dataletturaprecedente")) Then
    '                If StringOperation.Formatstring(myrow("dataletturaprecedente")) <> "" Then
    '                    oMyLettura.tDataLetturaPrec = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("dataletturaprecedente")))
    '                Else
    '                    Log.Debug("ClsLettureFattura::GetLettureFatt::Data Lettura Precedente mancante::oMyLettura.IdLettura::" & oMyLettura.IdLettura)
    '                    oRuoloH2O.sNote = "Data Lettura Precedente mancante."
    '                    Return Nothing
    '                End If
    '            Else
    '                Log.Debug("ClsLettureFattura::GetLettureFatt::Data Lettura Precedente mancante::oMyLettura.IdLettura::" & oMyLettura.IdLettura)
    '                oRuoloH2O.sNote = "Data Lettura Precedente mancante."
    '                Return Nothing
    '            End If
    '            If Not IsDBNull(myrow("letturaprecedente")) Then
    '                oMyLettura.nLetturaPrec = StringOperation.Formatint(myrow("letturaprecedente"))
    '            End If
    '            '*** 20100310 - non valorizzo mai il consumo ma lo faccio calcolare dal motore altrimenti rischio di togliere + volte il consumo del subcontatore associato ***
    '            'If Not IsDBNull(myrow("consumo")) Then
    '            '    If StringOperation.Formatint(myrow("consumo")) <> 0 Then
    '            '        If Not IsDBNull(myrow("subconsumo")) Then
    '            '            oMyLettura.nConsumo = StringOperation.Formatint(myrow("consumo")) - StringOperation.Formatint(myrow("subconsumo"))
    '            '        Else
    '            '            oMyLettura.nConsumo = StringOperation.Formatint(myrow("consumo"))
    '            '        End If
    '            '    End If
    '            '    If oMyLettura.nConsumo < -1 Then
    '            '        oMyLettura.nConsumo = 0
    '            '    End If
    '            'End If
    '            If Not IsDBNull(myrow("subconsumo")) Then
    '                oMyLettura.nConsumoSubContatore = StringOperation.Formatint(myrow("subconsumo"))
    '            End If
    '            '***********************************************************************************************************************************
    '            If Not IsDBNull(myrow("giornidiconsumo")) Then
    '                If StringOperation.Formatint(myrow("giornidiconsumo")) <> 0 Then
    '                    oMyLettura.nGiorni = StringOperation.Formatint(myrow("giornidiconsumo"))
    '                End If
    '            End If
    '            oMyLettura.nTipoArrotondConsumo = nTipoArrotondConsumo

    '            If nContatorePrec = -1 Then
    '                oMyContatore = New objContatore
    '            End If
    '            oMyContatore.sIdEnte = StringOperation.Formatstring(myrow("codente"))
    '            If Not IsDBNull(myrow("idintestatario")) Then
    '                oMyContatore.nIdIntestatario = StringOperation.Formatint(myrow("idintestatario"))
    '            End If
    '            If Not IsDBNull(myrow("idutente")) Then
    '                oMyContatore.nIdUtente = StringOperation.Formatint(myrow("idutente"))
    '            End If
    '            '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
    '            'oMyContatore.oAnagUtente = FncAnagrafica.GetAnagrafica(oMyLettura.nIdUtente, sTributo, -1, ConstSession.StringConnectionAnagrafica)
    '            oMyContatore.oAnagUtente = FncAnagrafica.GetAnagrafica(oMyLettura.nIdUtente, -1, String.Empty, ConstSession.DBType, ConstSession.StringConnectionAnagrafica, False)
    '            If Not IsDBNull(myrow("idtipocontatore")) Then
    '                oMyContatore.nTipoContatore = StringOperation.Formatint(myrow("idtipocontatore"))
    '            End If
    '            If Not IsDBNull(myrow("idtipoutenza")) Then
    '                oMyContatore.nTipoUtenza = StringOperation.Formatint(myrow("idtipoutenza"))
    '            End If
    '            If Not IsDBNull(myrow("numeroutenze")) Then
    '                oMyContatore.nNumeroUtenze = StringOperation.Formatint(myrow("numeroutenze"))
    '            End If
    '            If Not IsDBNull(myrow("matricola")) Then
    '                oMyContatore.sMatricola = StringOperation.Formatstring(myrow("matricola"))
    '            End If
    '            If Not IsDBNull(myrow("via_ubicazione")) Then
    '                oMyContatore.sUbicazione = StringOperation.Formatstring(myrow("via_ubicazione"))
    '            End If
    '            If Not IsDBNull(myrow("civico_ubicazione")) Then
    '                oMyContatore.sCivico = StringOperation.Formatstring(myrow("civico_ubicazione"))
    '            End If
    '            If Not IsDBNull(myrow("frazione_ubicazione")) Then
    '                oMyContatore.sFrazione = StringOperation.Formatstring(myrow("frazione_ubicazione"))
    '            End If
    '            If Not IsDBNull(myrow("coddepurazione")) Then
    '                oMyContatore.nCodDepurazione = StringOperation.Formatint(myrow("coddepurazione"))
    '            End If
    '            If Not IsDBNull(myrow("esenteacqua")) Then
    '                oMyContatore.bEsenteAcqua = StringOperation.Formatbool(myrow("esenteacqua"))
    '            End If
    '            If Not IsDBNull(myrow("esentedepurazione")) Then
    '                oMyContatore.bEsenteDepurazione = StringOperation.Formatbool(myrow("esentedepurazione"))
    '            End If
    '            If Not IsDBNull(myrow("codfognatura")) Then
    '                oMyContatore.nCodFognatura = StringOperation.Formatint(myrow("codfognatura"))
    '            End If
    '            If Not IsDBNull(myrow("esentefognatura")) Then
    '                oMyContatore.bEsenteFognatura = StringOperation.Formatbool(myrow("esentefognatura"))
    '            End If
    '            If Not IsDBNull(myrow("valorefondoscala")) Then
    '                If StringOperation.Formatstring(myrow("valorefondoscala")) <> "" Then
    '                    oMyContatore.nFondoScala = StringOperation.Formatint(myrow("valorefondoscala"))
    '                End If
    '            End If
    '            '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
    '            If Not IsDBNull(myrow("ESENTEACQUAQF")) Then
    '                oMyContatore.bEsenteAcquaQF = StringOperation.Formatbool(myrow("ESENTEACQUAQF"))
    '            End If
    '            If Not IsDBNull(myrow("ESENTEDEPURAZIONEQF")) Then
    '                oMyContatore.bEsenteDepQF = StringOperation.Formatbool(myrow("ESENTEDEPURAZIONEQF"))
    '            End If
    '            If Not IsDBNull(myrow("ESENTEFOGNATURAQF")) Then
    '                oMyContatore.bEsenteFogQF = StringOperation.Formatbool(myrow("ESENTEFOGNATURAQF"))
    '            End If
    '            '*** ***
    '            Log.Debug("ClsLettureFattura::GetLettureFatt::prelevato tutti i dati per fatturare la lettura::oMyLettura.IdLettura::" & oMyLettura.IdLettura)
    '            ReDim Preserve oMyContatore.oListLetture(nListLett)
    '            oMyContatore.oListLetture(nListLett) = oMyLettura
    '            nContatorePrec = StringOperation.Formatint(myrow("codcontatore"))
    '        Loop
    '        If Not IsNothing(oMyContatore) Then
    '            'dimensiono l'array
    '            nListCont += 1
    '            ReDim Preserve oListContatori(nListCont)
    '            'memorizzo i dati nell'array
    '            oListContatori(nListCont) = oMyContatore
    '        End If

    '        Return oListContatori
    '    Catch Err As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsLettureFattura.GetLettureFatt.errore: ", Err)
    '        Return Nothing
    '    Finally
    '        dvmydati.dispose()
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sIdEnte"></param>
    ''' <param name="nIdPeriodo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <revisionHistory>
    ''' <revision date="05/02/2020">
    ''' I contatori senza lettura devono essere riferiti al periodo in esame
    ''' </revision>
    ''' </revisionHistory>
    Public Function GetContatoriSenzaLettura(sIdEnte As String, ByVal nIdPeriodo As Integer) As Integer
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Dim nContatori As Integer = 0

        Try
            Try
                Dim oDbManagerRepository As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                Using ctx As DBModel = oDbManagerRepository
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetLettureNonPresenti", "IDENTE", "IDPERIODO", "DATAA", "INTESTATARIO", "UTENTE", "VIA", "NUTENTE", "MATRICOLA", "IDGIRO", "ISSUB")
                    dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", sIdEnte) _
                        , ctx.GetParam("IDPERIODO", nIdPeriodo) _
                        , ctx.GetParam("DATAA", "99991231") _
                        , ctx.GetParam("INTESTATARIO", "") _
                        , ctx.GetParam("UTENTE", "") _
                        , ctx.GetParam("VIA", "") _
                        , ctx.GetParam("NUTENTE", "") _
                        , ctx.GetParam("MATRICOLA", "") _
                        , ctx.GetParam("IDGIRO", -1) _
                        , ctx.GetParam("ISSUB", 0)
                    )
                    ctx.Dispose()
                End Using
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        nContatori += 1
                    Next
                End If
            Catch ex As Exception
                Log.Debug(sIdEnte + " - OPENgovH2O.ClsLettureFatture.GetContatoriSenzaLettura.query.errore: ", ex)
            Finally
                dvMyDati.Dispose()
            End Try
            Return nContatori
        Catch Err As Exception
            Log.Debug(sIdEnte + " - OPENgovH2O.ClsLettureFatture.GetContatoriSenzaLettura.errore: ", Err)
            Return 0
        End Try
    End Function
    'Public Function GetContatoriSenzaLettura(ByVal sEnte As String) As Integer
    '    Dim sSQL As String="" = ""
    '    Dim dvMyDati As New DataView
    '    Dim nContatori As Integer = 0

    '    Try
    '        Try
    '            Dim oDbManagerRepository As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
    '            Using ctx As DBModel = oDbManagerRepository
    '                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_ContatoriNoLettura", "IDENTE")
    '                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", sEnte))
    '                ctx.Dispose()
    '            End Using
    '            If Not dvMyDati Is Nothing Then
    '                For Each myRow As DataRowView In dvMyDati
    '                    nContatori = Utility.StringOperation.FormatInt(myRow("ncontatori"))
    '                Next
    '            End If
    '        Catch ex As Exception
    '            Log.Debug(sEnte + " - OPENgovH2O.ClsLettureFatture.GetContatoriSenzaLettura.query.errore: ", ex)
    '        Finally
    '            dvMyDati.Dispose()
    '        End Try
    '        Return nContatori
    '    Catch Err As Exception
    '        Log.Debug(sEnte + " - OPENgovH2O.ClsLettureFatture.GetContatoriSenzaLettura.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function
    'Public Function GetContatoriSenzaLettura(ByVal sEnte As String) As Integer
    '    Dim sSQL As String=""
    '    Dim dvMyDati As new dataview=nothing
    '    Dim nContatori As Integer = 0

    '    Try
    '        sSQL = "SELECT COUNT(TP_CONTATORI.CODCONTATORE) AS NCONTATORI"
    '        sSQL += " FROM TP_CONTATORI WITH (NOLOCK)"
    '        sSQL += " LEFT JOIN TP_LETTURE WITH (NOLOCK) ON TP_CONTATORI.CODCONTATORE = TP_LETTURE.CODCONTATORE"
    '        sSQL += " WHERE (TP_LETTURE.CODCONTATORE IS NULL) "
    '        sSQL += " AND (TP_CONTATORI.CODENTE='" & sEnte & "') "
    '        sSQL += " AND (NOT TP_CONTATORI.DATAATTIVAZIONE IS NULL AND TP_CONTATORI.DATACESSAZIONE IS NULL)"
    '        'eseguo la query
    '        dvMyDati = iDB.getdataview(sSQL)
    '        Do While dvMyDati.Read
    '            If Not IsDBNull(myrow("ncontatori")) Then
    '                nContatori = StringOperation.Formatint(myrow("ncontatori"))
    '            End If
    '        Loop

    '        Return nContatori
    '    Catch Err As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsLettureFatture.GetContatoriSenzaLettura.errore: ", Err)
    '        Return 0
    '    Finally
    '        dvmydati.dispose()
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sEnte"></param>
    ''' <param name="nPeriodo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetTotalizzatoriLetture(ByVal sEnte As String, ByVal nPeriodo As Integer) As ObjTotRuoloFatture
        Dim oTotLettureRuolo As New ObjTotRuoloFatture
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView

        Try
            Try
                Dim oDbManagerRepository As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                Using ctx As DBModel = oDbManagerRepository
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetTotaliLetture", "IDENTE", "IDPERIODO")
                    dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", sEnte) _
                                , ctx.GetParam("IDPERIODO", nPeriodo)
                            )
                    ctx.Dispose()
                End Using
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        oTotLettureRuolo.nNContribuenti = StringOperation.FormatInt(myRow("ncontribuenti"))
                        oTotLettureRuolo.nNDocumenti = StringOperation.FormatInt(myRow("nletture"))
                    Next
                End If
            Catch ex As Exception
                Log.Debug(sEnte + " - OPENgovH2O.ClsLettureFatture.GetTotalizzatoriLetture.query.errore: ", ex)
            Finally
                dvMyDati.Dispose()
            End Try
            'eseguo 
            Return oTotLettureRuolo
        Catch Err As Exception
            Log.Debug(sEnte + " - OPENgovH2O.ClsLettureFatture.GetTotalizzatoriLetture.errore: ", Err)
            Return Nothing
        End Try
    End Function
    'Public Function GetTotalizzatoriLetture(ByVal sEnte As String, ByVal nPeriodo As Integer) As ObjTotRuoloFatture
    '    Dim sSQL As String=""
    '    Dim dvMyDati As new dataview=nothing
    '    Dim oTotLettureRuolo As New ObjTotRuoloFatture

    '    Try
    '        sSQL = "SELECT COUNT(TMPCONTRIB.COD_CONTRIBUENTE) AS NCONTRIBUENTI, TMPLETTURE.NLETTURE"
    '        sSQL += " FROM ("
    '        sSQL += " SELECT DISTINCT TR_CONTATORI_INTESTATARIO.COD_CONTRIBUENTE"
    '        sSQL += " FROM TP_LETTURE INNER JOIN"
    '        sSQL += " TP_CONTATORI ON TP_LETTURE.CODCONTATORE = TP_CONTATORI.CODCONTATORE"
    '        sSQL += " INNER JOIN TR_CONTATORI_INTESTATARIO ON TP_CONTATORI.CODCONTATORE=TR_CONTATORI_INTESTATARIO.CODCONTATORE"
    '        sSQL += " WHERE (CODENTE='" & sEnte & "') AND (CODPERIODO=" & nPeriodo & ") AND (NOT DATALETTURAPRECEDENTE IS NULL AND DATALETTURAPRECEDENTE<>'')"
    '        sSQL += " AND (CODLETTURA NOT IN(SELECT DISTINCT IDLETTURA FROM TR_LETTURE_FATTURE))"
    '        sSQL += " ) TMPCONTRIB, ("
    '        sSQL += " SELECT COUNT(CODLETTURA) AS NLETTURE"
    '        sSQL += " FROM TP_LETTURE INNER JOIN"
    '        sSQL += " TP_CONTATORI ON TP_LETTURE.CODCONTATORE = TP_CONTATORI.CODCONTATORE"
    '        sSQL += " INNER JOIN TR_CONTATORI_INTESTATARIO ON TP_CONTATORI.CODCONTATORE=TR_CONTATORI_INTESTATARIO.CODCONTATORE"
    '        sSQL += " WHERE (CODENTE='" & sEnte & "') AND (CODPERIODO=" & nPeriodo & ") AND (NOT DATALETTURAPRECEDENTE IS NULL AND DATALETTURAPRECEDENTE<>'')"
    '        sSQL += "  AND (CODLETTURA NOT IN(SELECT DISTINCT IDLETTURA FROM TR_LETTURE_FATTURE))) TMPLETTURE"
    '        sSQL += " GROUP BY TMPLETTURE.NLETTURE"
    '        'eseguo la query
    '        dvMyDati = iDB.getdataview(sSQL)
    '        Do While dvMyDati.Read
    '            If Not IsDBNull(myrow("ncontribuenti")) Then
    '                oTotLettureRuolo.nNContribuenti = StringOperation.Formatint(myrow("ncontribuenti"))
    '            End If
    '            If Not IsDBNull(myrow("nletture")) Then
    '                oTotLettureRuolo.nNDocumenti = StringOperation.Formatint(myrow("nletture"))
    '            End If
    '        Loop
    '        Return oTotLettureRuolo
    '    Catch Err As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsLettureFatture.GetTotalizzatoriLetture.errore: ", Err)
    '        Return Nothing
    '    Finally
    '        dvmydati.dispose()
    '    End Try
    'End Function
End Class

Public Class ClsFatture
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ClsFatture))
    Private iDB As New DBAccess.getDBobject
    Private oReplace As New ClsGenerale.Generale
    Private NomeDBAnagrafica As String = ConfigurationManager.AppSettings("NOME_DATABASE_ANAGRAFICA") & ".dbo"


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myStringConnection"></param>
    ''' <param name="sEnte"></param>
    ''' <param name="nIdFlusso"></param>
    ''' <param name="nIdDoc"></param>
    ''' <param name="bIsVariazione"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <revisionHistory>
    ''' <revision date="17/12/2012">
    ''' calcolo quota fissa acqua+depurazione+fognatura
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="27/10/2014">
    ''' visualizzazione tutti indirizzi spedizione
    ''' </revision>
    ''' </revisionHistory>
    Public Function GetFattura(myStringConnection As String, ByVal sEnte As String, ByVal nIdFlusso As Integer, ByVal nIdDoc As Integer, ByVal bIsVariazione As Boolean) As ObjFattura()
        Dim oFattura As New ObjFattura
        Dim oListFatture() As ObjFattura
        Dim nList As Integer = -1
        Dim oMyAnagrafe As New DettaglioAnagrafica
        Dim x As Integer = 1

        oListFatture = Nothing
        Try
            Dim sSQL As String = ""
            Dim dvMyDati As New DataView
            Try
                Dim oDbManagerRepository As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                Using ctx As DBModel = oDbManagerRepository
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetFattura", "IDENTE", "VARIAZIONE", "IDFLUSSO", "IDDOC")
                    dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", sEnte) _
                                , ctx.GetParam("VARIAZIONE", bIsVariazione) _
                                , ctx.GetParam("IDFLUSSO", nIdFlusso) _
                                , ctx.GetParam("IDDOC", nIdDoc)
                            )
                    ctx.Dispose()
                End Using
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        Log.Debug("leggo::" & x)
                        x += 1
                        oFattura = New ObjFattura
                        oFattura.Id = StringOperation.FormatInt(myRow("idfatturanota"))
                        If Not IsDBNull(myRow("codcontatore")) Then
                            oFattura.nIdContatore = StringOperation.FormatInt(myRow("codcontatore"))
                        End If
                        oFattura.sIdEnte = StringOperation.FormatString(myRow("idente"))
                        oFattura.nIdFlusso = StringOperation.FormatInt(myRow("idflusso"))
                        oFattura.nIdPeriodo = StringOperation.FormatInt(myRow("idperiodo"))
                        oFattura.sTipoDocumento = StringOperation.FormatString(myRow("tipo_documento"))
                        oFattura.sDescrTipoDocumento = StringOperation.FormatString(myRow("descrtipodoc"))
                        oFattura.tDataDocumento = StringOperation.FormatDateTime(myRow("data_fattura"))
                        oFattura.sNDocumento = StringOperation.FormatString(myRow("numero_fattura"))
                        oFattura.sAnno = StringOperation.FormatString(myRow("anno_riferimento"))
                        oFattura.nIdIntestatario = StringOperation.FormatInt(myRow("cod_intestatario"))
                        'prelevo l'anagrafe dell'intestatario
                        oMyAnagrafe = New DettaglioAnagrafica
                        oMyAnagrafe.Cognome = StringOperation.FormatString(myRow("intest_cognome_denominazione"))
                        oMyAnagrafe.Nome = StringOperation.FormatString(myRow("intest_nome"))
                        oMyAnagrafe.CodiceFiscale = StringOperation.FormatString(myRow("intest_cod_fiscale"))
                        oMyAnagrafe.PartitaIva = StringOperation.FormatString(myRow("intest_partita_iva"))
                        oMyAnagrafe.ViaResidenza = StringOperation.FormatString(myRow("intest_via_res"))
                        oMyAnagrafe.CivicoResidenza = StringOperation.FormatString(myRow("intest_civico_res"))
                        oMyAnagrafe.EsponenteCivicoResidenza = StringOperation.FormatString(myRow("intest_esponente_res"))
                        oMyAnagrafe.InternoCivicoResidenza = StringOperation.FormatString(myRow("intest_interno_res"))
                        oMyAnagrafe.ScalaCivicoResidenza = StringOperation.FormatString(myRow("intest_scala_res"))
                        oMyAnagrafe.FrazioneResidenza = StringOperation.FormatString(myRow("intest_frazione_res"))
                        oMyAnagrafe.CapResidenza = StringOperation.FormatString(myRow("intest_cap_res"))
                        oMyAnagrafe.ComuneResidenza = StringOperation.FormatString(myRow("intest_comune_res"))
                        oMyAnagrafe.ProvinciaResidenza = StringOperation.FormatString(myRow("intest_provincia_res"))
                        oFattura.oAnagrafeIntestatario = oMyAnagrafe
                        'prelevo l'anagrafe dell'utente
                        oMyAnagrafe = New DettaglioAnagrafica
                        oFattura.nIdUtente = StringOperation.FormatInt(myRow("cod_utente"))
                        oMyAnagrafe.Cognome = StringOperation.FormatString(myRow("cognome_denominazione"))
                        oMyAnagrafe.Nome = StringOperation.FormatString(myRow("nome"))
                        oMyAnagrafe.CodiceFiscale = StringOperation.FormatString(myRow("cod_fiscale"))
                        oMyAnagrafe.PartitaIva = StringOperation.FormatString(myRow("partita_iva"))
                        oMyAnagrafe.ViaResidenza = StringOperation.FormatString(myRow("via_res"))
                        oMyAnagrafe.CivicoResidenza = StringOperation.FormatString(myRow("civico_res"))
                        oMyAnagrafe.EsponenteCivicoResidenza = StringOperation.FormatString(myRow("esponente_res"))
                        oMyAnagrafe.InternoCivicoResidenza = StringOperation.FormatString(myRow("interno_res"))
                        oMyAnagrafe.ScalaCivicoResidenza = StringOperation.FormatString(myRow("scala_res"))
                        oMyAnagrafe.FrazioneResidenza = StringOperation.FormatString(myRow("frazione_res"))
                        oMyAnagrafe.CapResidenza = StringOperation.FormatString(myRow("cap_res"))
                        oMyAnagrafe.ComuneResidenza = StringOperation.FormatString(myRow("comune_res"))
                        oMyAnagrafe.ProvinciaResidenza = StringOperation.FormatString(myRow("provincia_res"))
                        oMyAnagrafe.CodiceComuneResidenza = StringOperation.FormatString(myRow("codcomune_res"))
                        Dim mySped As New ObjIndirizziSpedizione
                        mySped.NomeInvio = StringOperation.FormatString(myRow("nome_invio"))
                        mySped.ViaRCP = StringOperation.FormatString(myRow("via_rcp"))
                        mySped.CivicoRCP = StringOperation.FormatString(myRow("civico_rcp"))
                        mySped.EsponenteCivicoRCP = StringOperation.FormatString(myRow("esponente_rcp"))
                        mySped.InternoCivicoRCP = StringOperation.FormatString(myRow("interno_rcp"))
                        mySped.ScalaCivicoRCP = StringOperation.FormatString(myRow("scala_rcp"))
                        mySped.FrazioneRCP = StringOperation.FormatString(myRow("frazione_rcp"))
                        mySped.CapRCP = StringOperation.FormatString(myRow("cap_rcp"))
                        mySped.ComuneRCP = StringOperation.FormatString(myRow("comune_rcp"))
                        mySped.ProvinciaRCP = StringOperation.FormatString(myRow("provincia_rcp"))
                        mySped.CodComuneRCP = StringOperation.FormatString(myRow("codcomune_rcp"))
                        oMyAnagrafe.ListSpedizioni.Add(mySped)
                        oFattura.oAnagrafeUtente = oMyAnagrafe
                        oFattura.sNUtente = StringOperation.FormatString(myRow("numeroutente"))
                        oFattura.sMatricola = StringOperation.FormatString(myRow("matricola"))
                        oFattura.sViaContatore = StringOperation.FormatString(myRow("via_contatore"))
                        oFattura.sCivicoContatore = StringOperation.FormatString(myRow("civico_contatore"))
                        oFattura.sFrazioneContatore = StringOperation.FormatString(myRow("frazione_contatore"))
                        oFattura.nConsumo = StringOperation.FormatInt(myRow("consumo"))
                        oFattura.nGiorni = StringOperation.FormatInt(myRow("giorni"))
                        oFattura.nTipoUtenza = StringOperation.FormatInt(myRow("id_tipologia_utenza"))
                        oFattura.sDescrTipoUtenza = StringOperation.FormatString(myRow("descrtipoutenza"))
                        oFattura.nTipoContatore = StringOperation.FormatInt(myRow("id_tipo_contatore"))
                        oFattura.sDescrTipoContatore = StringOperation.FormatString(myRow("descrtipocontatore"))
                        oFattura.nCodFognatura = StringOperation.FormatInt(myRow("codfognatura"))
                        oFattura.nCodDepurazione = StringOperation.FormatInt(myRow("coddepurazione"))
                        oFattura.bEsenteAcqua = StringOperation.FormatBool(myRow("esenteacqua"))
                        oFattura.bEsenteFognatura = StringOperation.FormatBool(myRow("esentefognatura"))
                        oFattura.bEsenteDepurazione = StringOperation.FormatBool(myRow("esentedepurazione"))
                        oFattura.nUtenze = StringOperation.FormatInt(myRow("nutenze"))
                        Log.Debug("letturaprec->" + StringOperation.FormatString(myRow("data_lettura_prec")))
                        oFattura.tDataLetturaPrec = StringOperation.FormatDateTime(myRow("data_lettura_prec"))
                        oFattura.nLetturaPrec = StringOperation.FormatInt(myRow("lettura_prec"))
                        Log.Debug("letturaatt->" + StringOperation.FormatString(myRow("data_lettura_att")))
                        oFattura.tDataLetturaAtt = StringOperation.FormatDateTime(myRow("data_lettura_att"))
                        oFattura.nLetturaAtt = StringOperation.FormatInt(myRow("lettura_att"))
                        oFattura.impScaglioni = StringOperation.FormatDouble(myRow("importo_scaglioni"))
                        oFattura.impCanoni = StringOperation.FormatDouble(myRow("importo_canoni"))
                        oFattura.impAddizionali = StringOperation.FormatDouble(myRow("importo_addizionali"))
                        oFattura.impNolo = StringOperation.FormatDouble(myRow("importo_nolo"))
                        oFattura.impQuoteFisse = StringOperation.FormatDouble(myRow("importo_quotafissa"))
                        oFattura.bEsenteAcquaQF = StringOperation.FormatBool(myRow("ESENTEACQUAQF"))
                        oFattura.bEsenteDepQF = StringOperation.FormatBool(myRow("ESENTEDEPURAZIONEQF"))
                        oFattura.bEsenteFogQF = StringOperation.FormatBool(myRow("ESENTEFOGNATURAQF"))
                        oFattura.impQuoteFisseDep = StringOperation.FormatDouble(myRow("importo_quotafissa_dep"))
                        oFattura.impQuoteFisseFog = StringOperation.FormatDouble(myRow("importo_quotafissa_fog"))
                        oFattura.impImponibile = StringOperation.FormatDouble(myRow("importo_imponibile"))
                        oFattura.impIva = StringOperation.FormatDouble(myRow("importo_iva"))
                        oFattura.impEsente = StringOperation.FormatDouble(myRow("importo_esente"))
                        oFattura.impTotale = StringOperation.FormatDouble(myRow("importo_totale"))
                        oFattura.impArrotondamento = StringOperation.FormatDouble(myRow("importo_arrotondamento"))
                        oFattura.impFattura = StringOperation.FormatDouble(myRow("importo_fatturanota"))
                        Log.Debug("data_fattura_riferimento->" + StringOperation.FormatString(myRow("data_fattura_riferimento")))
                        oFattura.tDataDocumentoRif = StringOperation.FormatDateTime(myRow("data_fattura_riferimento"))
                        oFattura.sNDocumentoRif = StringOperation.FormatString(myRow("numero_fattura_riferimento"))
                        oFattura.tDataInserimento = StringOperation.FormatDateTime(myRow("data_inserimento"))
                        oFattura.tDataVariazione = StringOperation.FormatDateTime(myRow("data_variazione"))
                        oFattura.tDataCessazione = StringOperation.FormatDateTime(myRow("data_cessazione"))
                        oFattura.sOperatore = StringOperation.FormatString(myRow("operatore"))
                        oFattura.oLetture = GetLettureVSFattura(oFattura.Id)
                        Log.Debug("GetLettureVSFattura fatto")
                        'devo caricare anche le tariffe
                        Dim FunctionTariffe As New ClsTariffe
                        oFattura.oScaglioni = FunctionTariffe.GetFatturaScaglioni(oFattura.Id)
                        Log.Debug("GetFatturaScaglioni fatto")
                        oFattura.oQuoteFisse = FunctionTariffe.GetFatturaQuoteFisse(oFattura.Id)
                        Log.Debug("GetFatturaQuoteFisse fatto")
                        oFattura.oNolo = FunctionTariffe.GetFatturaNolo(oFattura.Id)
                        Log.Debug("GetFatturaNolo fatto")
                        oFattura.oCanoni = FunctionTariffe.GetFatturaCanoni(oFattura.Id)
                        Log.Debug("GetFatturaCanoni fatto")
                        oFattura.oAddizionali = FunctionTariffe.GetFatturaAddizionali(oFattura.Id)
                        Log.Debug("GetFatturaAddizionali fatto")
                        oFattura.oDettaglioIva = FunctionTariffe.GetFatturaDettaglioIva(oFattura.Id)
                        Log.Debug("GetFatturaDettaglioIva fatto")
                        'ridimensiono l'array
                        nList += 1
                        ReDim Preserve oListFatture(nList)
                        oListFatture(nList) = oFattura
                    Next
                End If
            Catch ex As Exception
                Log.Debug(sEnte + " - OPENgovH2O.ClsFatture.GetFattura.query.errore: ", ex)
            Finally
                dvMyDati.Dispose()
            End Try

            Return oListFatture
        Catch Err As Exception
            Log.Debug(sEnte + " - OPENgovH2O.ClsFatture.GetFattura.errore: ", Err)
            Return Nothing
        End Try
    End Function
    'Public Function GetFattura(myStringConnection As String, ByVal sEnte As String, ByVal nIdFlusso As Integer, ByVal nIdDoc As Integer, ByVal bIsVariazione As Boolean) As ObjFattura()
    '    Dim sSQL As String=""
    '    Dim dvMyDati As new dataview=nothing
    '    Dim oFattura As ObjFattura
    '    Dim oListFatture() As ObjFattura
    '    Dim nList As Integer = -1
    '    Dim oMyAnagrafe As DettaglioAnagrafica
    '    Dim x As Integer = 1

    '    Try
    '        sSQL = "SELECT *"
    '        sSQL += " FROM OPENgov_FATTURE"
    '        sSQL += " WHERE 1=1"
    '        If sEnte <> "" Then
    '            sSQL += " AND (IDENTE='" & sEnte & "')"
    '            If bIsVariazione = True Then
    '                sSQL += " AND (IDFLUSSO=-1)"
    '            End If
    '        End If
    '        If nIdFlusso <> -1 Then
    '            sSQL += " AND (IDFLUSSO=" & nIdFlusso & ")"
    '        End If
    '        If nIdDoc <> -1 Then
    '            sSQL += " AND (IDFATTURANOTA=" & nIdDoc & ")"
    '        End If
    '        sSQL += " ORDER BY TIPO_DOCUMENTO DESC, COGNOME_DENOMINAZIONE,NOME,MATRICOLA"
    '        'eseguo la query
    '        Log.Debug("ClsFatture::GetFattura::SQL: " & sSQL)
    '        iDB = New DBAccess.getDBobject(myStringConnection)
    '        dvMyDati = iDB.getdataview(sSQL)
    '        Do While dvMyDati.Read
    '            Log.Debug("leggo::" & x)
    '            x += 1
    '            oFattura = New ObjFattura
    '            oFattura.Id = StringOperation.Formatint(myrow("idfatturanota"))
    '            If Not IsDBNull(myrow("codcontatore")) Then
    '                oFattura.nIdContatore = StringOperation.Formatint(myrow("codcontatore"))
    '            End If
    '            oFattura.sIdEnte = StringOperation.Formatstring(myrow("idente"))
    '            oFattura.nIdFlusso = StringOperation.Formatint(myrow("idflusso"))
    '            oFattura.nIdPeriodo = StringOperation.Formatint(myrow("idperiodo"))
    '            oFattura.sTipoDocumento = StringOperation.Formatstring(myrow("tipo_documento"))
    '            oFattura.sDescrTipoDocumento = StringOperation.Formatstring(myrow("descrtipodoc"))
    '            If Not IsDBNull(myrow("data_fattura")) Then
    '                If StringOperation.Formatstring(myrow("data_fattura")) <> "" Then
    '                    oFattura.tDataDocumento = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_fattura")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("numero_fattura")) Then
    '                If StringOperation.Formatstring(myrow("numero_fattura")) <> "" Then
    '                    oFattura.sNDocumento = StringOperation.Formatstring(myrow("numero_fattura"))
    '                End If
    '            End If
    '            oFattura.sAnno = StringOperation.Formatstring(myrow("anno_riferimento"))
    '            oFattura.nIdIntestatario = StringOperation.Formatint(myrow("cod_intestatario"))
    '            'prelevo l'anagrafe dell'intestatario
    '            oMyAnagrafe = New DettaglioAnagrafica
    '            oMyAnagrafe.Cognome = StringOperation.Formatstring(myrow("intest_cognome_denominazione"))
    '            oMyAnagrafe.Nome = StringOperation.Formatstring(myrow("intest_nome"))
    '            oMyAnagrafe.CodiceFiscale = StringOperation.Formatstring(myrow("intest_cod_fiscale"))
    '            oMyAnagrafe.PartitaIva = StringOperation.Formatstring(myrow("intest_partita_iva"))
    '            oMyAnagrafe.ViaResidenza = StringOperation.Formatstring(myrow("intest_via_res"))
    '            oMyAnagrafe.CivicoResidenza = StringOperation.Formatstring(myrow("intest_civico_res"))
    '            oMyAnagrafe.EsponenteCivicoResidenza = StringOperation.Formatstring(myrow("intest_esponente_res"))
    '            oMyAnagrafe.InternoCivicoResidenza = StringOperation.Formatstring(myrow("intest_interno_res"))
    '            oMyAnagrafe.ScalaCivicoResidenza = StringOperation.Formatstring(myrow("intest_scala_res"))
    '            oMyAnagrafe.FrazioneResidenza = StringOperation.Formatstring(myrow("intest_frazione_res"))
    '            oMyAnagrafe.CapResidenza = StringOperation.Formatstring(myrow("intest_cap_res"))
    '            oMyAnagrafe.ComuneResidenza = StringOperation.Formatstring(myrow("intest_comune_res"))
    '            oMyAnagrafe.ProvinciaResidenza = StringOperation.Formatstring(myrow("intest_provincia_res"))
    '            oFattura.oAnagrafeIntestatario = oMyAnagrafe
    '            'prelevo l'anagrafe dell'utente
    '            oMyAnagrafe = New DettaglioAnagrafica
    '            oFattura.nIdUtente = StringOperation.Formatint(myrow("cod_utente"))
    '            oMyAnagrafe.Cognome = StringOperation.Formatstring(myrow("cognome_denominazione"))
    '            oMyAnagrafe.Nome = StringOperation.Formatstring(myrow("nome"))
    '            oMyAnagrafe.CodiceFiscale = StringOperation.Formatstring(myrow("cod_fiscale"))
    '            oMyAnagrafe.PartitaIva = StringOperation.Formatstring(myrow("partita_iva"))
    '            oMyAnagrafe.ViaResidenza = StringOperation.Formatstring(myrow("via_res"))
    '            oMyAnagrafe.CivicoResidenza = StringOperation.Formatstring(myrow("civico_res"))
    '            oMyAnagrafe.EsponenteCivicoResidenza = StringOperation.Formatstring(myrow("esponente_res"))
    '            oMyAnagrafe.InternoCivicoResidenza = StringOperation.Formatstring(myrow("interno_res"))
    '            oMyAnagrafe.ScalaCivicoResidenza = StringOperation.Formatstring(myrow("scala_res"))
    '            oMyAnagrafe.FrazioneResidenza = StringOperation.Formatstring(myrow("frazione_res"))
    '            oMyAnagrafe.CapResidenza = StringOperation.Formatstring(myrow("cap_res"))
    '            oMyAnagrafe.ComuneResidenza = StringOperation.Formatstring(myrow("comune_res"))
    '            oMyAnagrafe.ProvinciaResidenza = StringOperation.Formatstring(myrow("provincia_res"))
    '            oMyAnagrafe.CodiceComuneResidenza = StringOperation.Formatstring(myrow("codcomune_res"))
    '            Dim mySped As New ObjIndirizziSpedizione
    '            mySped.NomeInvio = StringOperation.Formatstring(myrow("nome_invio"))
    '            mySped.ViaRCP = StringOperation.Formatstring(myrow("via_rcp"))
    '            mySped.CivicoRCP = StringOperation.Formatstring(myrow("civico_rcp"))
    '            mySped.EsponenteCivicoRCP = StringOperation.Formatstring(myrow("esponente_rcp"))
    '            mySped.InternoCivicoRCP = StringOperation.Formatstring(myrow("interno_rcp"))
    '            mySped.ScalaCivicoRCP = StringOperation.Formatstring(myrow("scala_rcp"))
    '            mySped.FrazioneRCP = StringOperation.Formatstring(myrow("frazione_rcp"))
    '            mySped.CapRCP = StringOperation.Formatstring(myrow("cap_rcp"))
    '            mySped.ComuneRCP = StringOperation.Formatstring(myrow("comune_rcp"))
    '            mySped.ProvinciaRCP = StringOperation.Formatstring(myrow("provincia_rcp"))
    '            mySped.CodComuneRCP = StringOperation.Formatstring(myrow("codcomune_rcp"))
    '            oMyAnagrafe.ListSpedizioni.Add(mySped)
    '            oFattura.oAnagrafeUtente = oMyAnagrafe
    '            oFattura.sNUtente = StringOperation.Formatstring(myrow("numeroutente"))
    '            oFattura.sMatricola = StringOperation.Formatstring(myrow("matricola"))
    '            oFattura.sViaContatore = StringOperation.Formatstring(myrow("via_contatore"))
    '            oFattura.sCivicoContatore = StringOperation.Formatstring(myrow("civico_contatore"))
    '            oFattura.sFrazioneContatore = StringOperation.Formatstring(myrow("frazione_contatore"))
    '            oFattura.nConsumo = StringOperation.Formatint(myrow("consumo"))
    '            oFattura.nGiorni = StringOperation.Formatint(myrow("giorni"))
    '            oFattura.nTipoUtenza = StringOperation.Formatint(myrow("id_tipologia_utenza"))
    '            If Not IsDBNull(myrow("descrtipoutenza")) Then
    '                oFattura.sDescrTipoUtenza = StringOperation.Formatstring(myrow("descrtipoutenza"))
    '            End If
    '            oFattura.nTipoContatore = StringOperation.Formatint(myrow("id_tipo_contatore"))
    '            If Not IsDBNull(myrow("descrtipocontatore")) Then
    '                oFattura.sDescrTipoContatore = StringOperation.Formatstring(myrow("descrtipocontatore"))
    '            End If
    '            If Not IsDBNull(myrow("codfognatura")) Then
    '                If StringOperation.Formatstring(myrow("codfognatura")) <> "" Then
    '                    oFattura.nCodFognatura = StringOperation.Formatint(myrow("codfognatura"))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("coddepurazione")) Then
    '                If StringOperation.Formatstring(myrow("coddepurazione")) <> "" Then
    '                    oFattura.nCodDepurazione = StringOperation.Formatint(myrow("coddepurazione"))
    '                End If
    '            End If
    '            oFattura.bEsenteAcqua = StringOperation.Formatbool(myrow("esenteacqua"))
    '            oFattura.bEsenteFognatura = StringOperation.Formatbool(myrow("esentefognatura"))
    '            oFattura.bEsenteDepurazione = StringOperation.Formatbool(myrow("esentedepurazione"))
    '            oFattura.nUtenze = StringOperation.Formatint(myrow("nutenze"))
    '            If Not IsDBNull(myrow("data_lettura_prec")) Then
    '                If StringOperation.Formatstring(myrow("data_lettura_prec")) <> "" Then
    '                    oFattura.tDataLetturaPrec = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_lettura_prec")))
    '                End If
    '            End If
    '            oFattura.nLetturaPrec = StringOperation.Formatint(myrow("lettura_prec"))
    '            If Not IsDBNull(myrow("data_lettura_att")) Then
    '                If StringOperation.Formatstring(myrow("data_lettura_att")) <> "" Then
    '                    oFattura.tDataLetturaAtt = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_lettura_att")))
    '                End If
    '            End If
    '            oFattura.nLetturaAtt = StringOperation.Formatint(myrow("lettura_att"))
    '            oFattura.impScaglioni = StringOperation.Formatdouble(myrow("importo_scaglioni"))
    '            oFattura.impCanoni = StringOperation.Formatdouble(myrow("importo_canoni"))
    '            oFattura.impAddizionali = StringOperation.Formatdouble(myrow("importo_addizionali"))
    '            oFattura.impNolo = StringOperation.Formatdouble(myrow("importo_nolo"))
    '            oFattura.impQuoteFisse = StringOperation.Formatdouble(myrow("importo_quotafissa"))
    '            If Not IsDBNull(myrow("ESENTEACQUAQF")) Then
    '                oFattura.bEsenteAcquaQF = StringOperation.Formatbool(myrow("ESENTEACQUAQF"))
    '            End If
    '            If Not IsDBNull(myrow("ESENTEDEPURAZIONEQF")) Then
    '                oFattura.bEsenteDepQF = StringOperation.Formatbool(myrow("ESENTEDEPURAZIONEQF"))
    '            End If
    '            If Not IsDBNull(myrow("ESENTEFOGNATURAQF")) Then
    '                oFattura.bEsenteFogQF = StringOperation.Formatbool(myrow("ESENTEFOGNATURAQF"))
    '            End If
    '            If Not IsDBNull(myrow("importo_quotafissa_dep")) Then
    '                oFattura.impQuoteFisseDep = StringOperation.Formatdouble(myrow("importo_quotafissa_dep"))
    '            End If
    '            If Not IsDBNull(myrow("importo_quotafissa_fog")) Then
    '                oFattura.impQuoteFisseFog = StringOperation.Formatdouble(myrow("importo_quotafissa_fog"))
    '            End If
    '            oFattura.impImponibile = StringOperation.Formatdouble(myrow("importo_imponibile"))
    '            oFattura.impIva = StringOperation.Formatdouble(myrow("importo_iva"))
    '            oFattura.impEsente = StringOperation.Formatdouble(myrow("importo_esente"))
    '            oFattura.impTotale = StringOperation.Formatdouble(myrow("importo_totale"))
    '            oFattura.impArrotondamento = StringOperation.Formatdouble(myrow("importo_arrotondamento"))
    '            oFattura.impFattura = StringOperation.Formatdouble(myrow("importo_fatturanota"))
    '            If Not IsDBNull(myrow("data_fattura_riferimento")) Then
    '                If StringOperation.Formatstring(myrow("data_fattura_riferimento")) <> "" Then
    '                    oFattura.tDataDocumentoRif = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_fattura_riferimento")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("numero_fattura_riferimento")) Then
    '                oFattura.sNDocumentoRif = StringOperation.Formatstring(myrow("numero_fattura_riferimento"))
    '            End If
    '            If Not IsDBNull(myrow("data_inserimento")) Then
    '                If StringOperation.Formatstring(myrow("data_inserimento")) <> "" Then
    '                    oFattura.tDataInserimento = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_inserimento")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("data_variazione")) Then
    '                If StringOperation.Formatstring(myrow("data_variazione")) <> "" Then
    '                    oFattura.tDataVariazione = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_variazione")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("data_cessazione")) Then
    '                If StringOperation.Formatstring(myrow("data_cessazione")) <> "" Then
    '                    oFattura.tDataCessazione = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_cessazione")))
    '                End If
    '            End If
    '            oFattura.sOperatore = StringOperation.Formatstring(myrow("operatore"))
    '            oFattura.oLetture = GetLettureVSFattura(oFattura.Id)
    '            Log.Debug("GetLettureVSFattura fatto")
    '            'devo caricare anche le tariffe
    '            Dim FunctionTariffe As New ClsTariffe
    '            oFattura.oScaglioni = FunctionTariffe.GetFatturaScaglioni(oFattura.Id)
    '            Log.Debug("GetFatturaScaglioni fatto")
    '            oFattura.oQuoteFisse = FunctionTariffe.GetFatturaQuoteFisse(oFattura.Id)
    '            Log.Debug("GetFatturaQuoteFisse fatto")
    '            oFattura.oNolo = FunctionTariffe.GetFatturaNolo(oFattura.Id)
    '            Log.Debug("GetFatturaNolo fatto")
    '            oFattura.oCanoni = FunctionTariffe.GetFatturaCanoni(oFattura.Id)
    '            Log.Debug("GetFatturaCanoni fatto")
    '            oFattura.oAddizionali = FunctionTariffe.GetFatturaAddizionali(oFattura.Id)
    '            Log.Debug("GetFatturaAddizionali fatto")
    '            oFattura.oDettaglioIva = FunctionTariffe.GetFatturaDettaglioIva(oFattura.Id)
    '            Log.Debug("GetFatturaDettaglioIva fatto")
    '            'ridimensiono l'array
    '            nList += 1
    '            ReDim Preserve oListFatture(nList)
    '            oListFatture(nList) = oFattura
    '        Loop

    '        Return oListFatture
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsFatture.GetFattura.errore: ", Err)
    '        Return Nothing
    '    Finally
    '        dvmydati.dispose()
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sEnte"></param>
    ''' <param name="tDataDoc"></param>
    ''' <param name="sNumDoc"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetNotaCreditoNewFattura(ByVal sEnte As String, ByVal tDataDoc As Date, ByVal sNumDoc As String) As ObjFattura
        Dim oFattura As New ObjFattura
        Dim oMyAnagrafe As New DettaglioAnagrafica

        Try
            Dim sSQL As String = ""
            Dim dvMyDati As New DataView
            Try
                Dim oDbManagerRepository As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                Using ctx As DBModel = oDbManagerRepository
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetNOTACREDITONEWFATTURA", "IDENTE", "NUMDOC", "DATADOC")
                    dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", sEnte) _
                    , ctx.GetParam("NUMDOC", sNumDoc) _
                    , ctx.GetParam("DATADOC", oReplace.GiraData(tDataDoc.ToString))
                )
                    ctx.Dispose()
                End Using
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        oFattura = New ObjFattura
                        oFattura.Id = StringOperation.FormatInt(myRow("idfatturanota"))
                        oFattura.sIdEnte = StringOperation.FormatString(myRow("idente"))
                        oFattura.nIdFlusso = StringOperation.FormatInt(myRow("idflusso"))
                        oFattura.nIdPeriodo = StringOperation.FormatInt(myRow("idperiodo"))
                        oFattura.sTipoDocumento = StringOperation.FormatString(myRow("tipo_documento"))
                        oFattura.sDescrTipoDocumento = StringOperation.FormatString(myRow("descrtipodoc"))
                        If StringOperation.FormatString(myRow("data_fattura")) <> "" Then
                            oFattura.tDataDocumento = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_fattura")))
                        End If
                        If StringOperation.FormatString(myRow("numero_fattura")) <> "" Then
                            oFattura.sNDocumento = StringOperation.FormatString(myRow("numero_fattura"))
                        End If
                        oFattura.sAnno = StringOperation.FormatString(myRow("anno_riferimento"))
                        oFattura.nIdIntestatario = StringOperation.FormatInt(myRow("cod_intestatario"))
                        'prelevo l'anagrafe dell'intestatario
                        oMyAnagrafe = New DettaglioAnagrafica
                        oMyAnagrafe.Cognome = StringOperation.FormatString(myRow("intest_cognome_denominazione"))
                        oMyAnagrafe.Nome = StringOperation.FormatString(myRow("intest_nome"))
                        oMyAnagrafe.CodiceFiscale = StringOperation.FormatString(myRow("intest_cod_fiscale"))
                        oMyAnagrafe.PartitaIva = StringOperation.FormatString(myRow("intest_partita_iva"))
                        oMyAnagrafe.ViaResidenza = StringOperation.FormatString(myRow("intest_via_res"))
                        oMyAnagrafe.CivicoResidenza = StringOperation.FormatString(myRow("intest_civico_res"))
                        oMyAnagrafe.EsponenteCivicoResidenza = StringOperation.FormatString(myRow("intest_esponente_res"))
                        oMyAnagrafe.InternoCivicoResidenza = StringOperation.FormatString(myRow("intest_interno_res"))
                        oMyAnagrafe.ScalaCivicoResidenza = StringOperation.FormatString(myRow("intest_scala_res"))
                        oMyAnagrafe.FrazioneResidenza = StringOperation.FormatString(myRow("intest_frazione_res"))
                        oMyAnagrafe.CapResidenza = StringOperation.FormatString(myRow("intest_cap_res"))
                        oMyAnagrafe.ComuneResidenza = StringOperation.FormatString(myRow("intest_comune_res"))
                        oMyAnagrafe.ProvinciaResidenza = StringOperation.FormatString(myRow("intest_provincia_res"))
                        oFattura.oAnagrafeIntestatario = oMyAnagrafe
                        'prelevo l'anagrafe dell'utente
                        oMyAnagrafe = New DettaglioAnagrafica
                        oFattura.nIdUtente = StringOperation.FormatInt(myRow("cod_utente"))
                        oMyAnagrafe.Cognome = StringOperation.FormatString(myRow("cognome_denominazione"))
                        oMyAnagrafe.Nome = StringOperation.FormatString(myRow("nome"))
                        oMyAnagrafe.CodiceFiscale = StringOperation.FormatString(myRow("cod_fiscale"))
                        oMyAnagrafe.PartitaIva = StringOperation.FormatString(myRow("partita_iva"))
                        oMyAnagrafe.ViaResidenza = StringOperation.FormatString(myRow("via_res"))
                        oMyAnagrafe.CivicoResidenza = StringOperation.FormatString(myRow("civico_res"))
                        oMyAnagrafe.EsponenteCivicoResidenza = StringOperation.FormatString(myRow("esponente_res"))
                        oMyAnagrafe.InternoCivicoResidenza = StringOperation.FormatString(myRow("interno_res"))
                        oMyAnagrafe.ScalaCivicoResidenza = StringOperation.FormatString(myRow("scala_res"))
                        oMyAnagrafe.FrazioneResidenza = StringOperation.FormatString(myRow("frazione_res"))
                        oMyAnagrafe.CapResidenza = StringOperation.FormatString(myRow("cap_res"))
                        oMyAnagrafe.ComuneResidenza = StringOperation.FormatString(myRow("comune_res"))
                        oMyAnagrafe.ProvinciaResidenza = StringOperation.FormatString(myRow("provincia_res"))
                        Dim mySped As New ObjIndirizziSpedizione
                        mySped.NomeInvio = StringOperation.FormatString(myRow("nome_invio"))
                        mySped.ViaRCP = StringOperation.FormatString(myRow("via_rcp"))
                        mySped.CivicoRCP = StringOperation.FormatString(myRow("civico_rcp"))
                        mySped.EsponenteCivicoRCP = StringOperation.FormatString(myRow("esponente_rcp"))
                        mySped.InternoCivicoRCP = StringOperation.FormatString(myRow("interno_rcp"))
                        mySped.ScalaCivicoRCP = StringOperation.FormatString(myRow("scala_rcp"))
                        mySped.FrazioneRCP = StringOperation.FormatString(myRow("frazione_rcp"))
                        mySped.CapRCP = StringOperation.FormatString(myRow("cap_rcp"))
                        mySped.ComuneRCP = StringOperation.FormatString(myRow("comune_rcp"))
                        mySped.ProvinciaRCP = StringOperation.FormatString(myRow("provincia_rcp"))
                        oMyAnagrafe.ListSpedizioni.Add(mySped)
                        oFattura.oAnagrafeUtente = oMyAnagrafe
                        oFattura.sNUtente = StringOperation.FormatString(myRow("numeroutente"))
                        oFattura.sMatricola = StringOperation.FormatString(myRow("matricola"))
                        oFattura.sViaContatore = StringOperation.FormatString(myRow("via_contatore"))
                        oFattura.sCivicoContatore = StringOperation.FormatString(myRow("civico_contatore"))
                        oFattura.sFrazioneContatore = StringOperation.FormatString(myRow("frazione_contatore"))
                        oFattura.nConsumo = StringOperation.FormatInt(myRow("consumo"))
                        oFattura.nGiorni = StringOperation.FormatInt(myRow("giorni"))
                        oFattura.nTipoUtenza = StringOperation.FormatInt(myRow("id_tipologia_utenza"))
                        oFattura.sDescrTipoUtenza = StringOperation.FormatString(myRow("descrtipoutenza"))
                        oFattura.nTipoContatore = StringOperation.FormatInt(myRow("id_tipo_contatore"))
                        oFattura.sDescrTipoContatore = StringOperation.FormatString(myRow("descrtipocontatore"))
                        oFattura.nCodFognatura = StringOperation.FormatInt(myRow("codfognatura"))
                        oFattura.nCodDepurazione = StringOperation.FormatInt(myRow("coddepurazione"))
                        oFattura.bEsenteAcqua = StringOperation.FormatBool(myRow("esenteacqua"))
                        oFattura.bEsenteFognatura = StringOperation.FormatBool(myRow("esentefognatura"))
                        oFattura.bEsenteDepurazione = StringOperation.FormatBool(myRow("esentedepurazione"))
                        oFattura.nUtenze = StringOperation.FormatInt(myRow("nutenze"))
                        If StringOperation.FormatString(myRow("data_lettura_prec")) <> "" Then
                            oFattura.tDataLetturaPrec = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_lettura_prec")))
                        End If
                        oFattura.nLetturaPrec = StringOperation.FormatInt(myRow("lettura_prec"))
                        If StringOperation.FormatString(myRow("data_lettura_att")) <> "" Then
                            oFattura.tDataLetturaAtt = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_lettura_att")))
                        End If
                        oFattura.nLetturaAtt = StringOperation.FormatInt(myRow("lettura_att"))
                        oFattura.impScaglioni = StringOperation.FormatDouble(myRow("importo_scaglioni"))
                        oFattura.impCanoni = StringOperation.FormatDouble(myRow("importo_canoni"))
                        oFattura.impAddizionali = StringOperation.FormatDouble(myRow("importo_addizionali"))
                        oFattura.impNolo = StringOperation.FormatDouble(myRow("importo_nolo"))
                        oFattura.impQuoteFisse = StringOperation.FormatDouble(myRow("importo_quotafissa"))
                        oFattura.bEsenteAcquaQF = StringOperation.FormatBool(myRow("ESENTEACQUAQF"))
                        oFattura.bEsenteDepQF = StringOperation.FormatBool(myRow("ESENTEDEPURAZIONEQF"))
                        oFattura.bEsenteFogQF = StringOperation.FormatBool(myRow("ESENTEFOGNATURAQF"))
                        oFattura.impQuoteFisseDep = StringOperation.FormatDouble(myRow("importo_quotafissa_dep"))
                        oFattura.impQuoteFisseFog = StringOperation.FormatDouble(myRow("importo_quotafissa_fog"))
                        oFattura.impImponibile = StringOperation.FormatDouble(myRow("importo_imponibile"))
                        oFattura.impIva = StringOperation.FormatDouble(myRow("importo_iva"))
                        oFattura.impEsente = StringOperation.FormatDouble(myRow("importo_esente"))
                        oFattura.impTotale = StringOperation.FormatDouble(myRow("importo_totale"))
                        oFattura.impArrotondamento = StringOperation.FormatDouble(myRow("importo_arrotondamento"))
                        oFattura.impFattura = StringOperation.FormatDouble(myRow("importo_fatturanota"))
                        If StringOperation.FormatString(myRow("data_fattura_riferimento")) <> "" Then
                            oFattura.tDataDocumentoRif = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_fattura_riferimento")))
                        End If
                        oFattura.sNDocumentoRif = StringOperation.FormatString(myRow("numero_fattura_riferimento"))
                        If StringOperation.FormatString(myRow("data_inserimento")) <> "" Then
                            oFattura.tDataInserimento = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_inserimento")))
                        End If
                        If StringOperation.FormatString(myRow("data_variazione")) <> "" Then
                            oFattura.tDataVariazione = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_variazione")))
                        End If
                        If StringOperation.FormatString(myRow("data_cessazione")) <> "" Then
                            oFattura.tDataCessazione = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_cessazione")))
                        End If
                        oFattura.sOperatore = StringOperation.FormatString(myRow("operatore"))
                        'devo caricare anche le tariffe
                        Dim FunctionTariffe As New ClsTariffe
                        oFattura.oScaglioni = FunctionTariffe.GetFatturaScaglioni(oFattura.Id)
                        oFattura.oQuoteFisse = FunctionTariffe.GetFatturaQuoteFisse(oFattura.Id)
                        oFattura.oNolo = FunctionTariffe.GetFatturaNolo(oFattura.Id)
                        oFattura.oCanoni = FunctionTariffe.GetFatturaCanoni(oFattura.Id)
                        oFattura.oAddizionali = FunctionTariffe.GetFatturaAddizionali(oFattura.Id)
                        oFattura.oDettaglioIva = FunctionTariffe.GetFatturaDettaglioIva(oFattura.Id)
                    Next
                End If
            Catch ex As Exception
                Log.Debug(sEnte + " - OPENgovH2O.ClsFatture.GetNotaCreditoNewFattura.query.errore: ", ex)
            Finally
                dvMyDati.Dispose()
            End Try

            Return oFattura
        Catch Err As Exception
            Log.Debug(sEnte + " - OPENgovH2O.ClsFatture.GetNotaCreditoNewFattura.errore: ", Err)
            Return Nothing
        End Try
    End Function
    'Public Function GetNotaCreditoNewFattura(ByVal sEnte As String, ByVal tDataDoc As Date, ByVal sNumDoc As String) As ObjFattura
    '    Dim sSQL As String=""
    '    Dim dvMyDati As new dataview=nothing
    '    Dim oFattura As ObjFattura
    '    Dim oMyAnagrafe As DettaglioAnagrafica

    '    Try
    '        sSQL = "SELECT *"
    '        sSQL += " FROM OPENgov_NOTACREDITONEWFATTURA WITH (NOLOCK)"
    '        sSQL += " WHERE (1=1)"
    '        sSQL += " AND (IDENTE='" & sEnte & "')"
    '        sSQL += " AND (NUMERO_FATTURA_RIFERIMENTO='" & sNumDoc & "')"
    '        sSQL += " AND (DATA_FATTURA_RIFERIMENTO='" & oReplace.GiraData(tDataDoc.ToString) & "')"
    '        'eseguo la query
    '        dvMyDati = iDB.getdataview(sSQL)
    '        Do While dvMyDati.Read
    '            oFattura = New ObjFattura
    '            oFattura.Id = StringOperation.FormatInt(myrow("idfatturanota"))
    '            oFattura.sIdEnte = StringOperation.Formatstring(myrow("idente"))
    '            oFattura.nIdFlusso = StringOperation.FormatInt(myrow("idflusso"))
    '            oFattura.nIdPeriodo = StringOperation.FormatInt(myrow("idperiodo"))
    '            oFattura.sTipoDocumento = StringOperation.Formatstring(myrow("tipo_documento"))
    '            oFattura.sDescrTipoDocumento = StringOperation.Formatstring(myrow("descrtipodoc"))
    '            If Not IsDBNull(myrow("data_fattura")) Then
    '                If StringOperation.Formatstring(myrow("data_fattura")) <> "" Then
    '                    oFattura.tDataDocumento = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_fattura")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("numero_fattura")) Then
    '                If StringOperation.Formatstring(myrow("numero_fattura")) <> "" Then
    '                    oFattura.sNDocumento = StringOperation.Formatstring(myrow("numero_fattura"))
    '                End If
    '            End If
    '            oFattura.sAnno = StringOperation.Formatstring(myrow("anno_riferimento"))
    '            oFattura.nIdIntestatario = StringOperation.FormatInt(myrow("cod_intestatario"))
    '            'prelevo l'anagrafe dell'intestatario
    '            oMyAnagrafe = New DettaglioAnagrafica
    '            oMyAnagrafe.Cognome = StringOperation.Formatstring(myrow("intest_cognome_denominazione"))
    '            oMyAnagrafe.Nome = StringOperation.Formatstring(myrow("intest_nome"))
    '            oMyAnagrafe.CodiceFiscale = StringOperation.Formatstring(myrow("intest_cod_fiscale"))
    '            oMyAnagrafe.PartitaIva = StringOperation.Formatstring(myrow("intest_partita_iva"))
    '            oMyAnagrafe.ViaResidenza = StringOperation.Formatstring(myrow("intest_via_res"))
    '            oMyAnagrafe.CivicoResidenza = StringOperation.Formatstring(myrow("intest_civico_res"))
    '            oMyAnagrafe.EsponenteCivicoResidenza = StringOperation.Formatstring(myrow("intest_esponente_res"))
    '            oMyAnagrafe.InternoCivicoResidenza = StringOperation.Formatstring(myrow("intest_interno_res"))
    '            oMyAnagrafe.ScalaCivicoResidenza = StringOperation.Formatstring(myrow("intest_scala_res"))
    '            oMyAnagrafe.FrazioneResidenza = StringOperation.Formatstring(myrow("intest_frazione_res"))
    '            oMyAnagrafe.CapResidenza = StringOperation.Formatstring(myrow("intest_cap_res"))
    '            oMyAnagrafe.ComuneResidenza = StringOperation.Formatstring(myrow("intest_comune_res"))
    '            oMyAnagrafe.ProvinciaResidenza = StringOperation.Formatstring(myrow("intest_provincia_res"))
    '            oFattura.oAnagrafeIntestatario = oMyAnagrafe
    '            'prelevo l'anagrafe dell'utente
    '            oMyAnagrafe = New DettaglioAnagrafica
    '            oFattura.nIdUtente = StringOperation.FormatInt(myrow("cod_utente"))
    '            oMyAnagrafe.Cognome = StringOperation.Formatstring(myrow("cognome_denominazione"))
    '            oMyAnagrafe.Nome = StringOperation.Formatstring(myrow("nome"))
    '            oMyAnagrafe.CodiceFiscale = StringOperation.Formatstring(myrow("cod_fiscale"))
    '            oMyAnagrafe.PartitaIva = StringOperation.Formatstring(myrow("partita_iva"))
    '            oMyAnagrafe.ViaResidenza = StringOperation.Formatstring(myrow("via_res"))
    '            oMyAnagrafe.CivicoResidenza = StringOperation.Formatstring(myrow("civico_res"))
    '            oMyAnagrafe.EsponenteCivicoResidenza = StringOperation.Formatstring(myrow("esponente_res"))
    '            oMyAnagrafe.InternoCivicoResidenza = StringOperation.Formatstring(myrow("interno_res"))
    '            oMyAnagrafe.ScalaCivicoResidenza = StringOperation.Formatstring(myrow("scala_res"))
    '            oMyAnagrafe.FrazioneResidenza = StringOperation.Formatstring(myrow("frazione_res"))
    '            oMyAnagrafe.CapResidenza = StringOperation.Formatstring(myrow("cap_res"))
    '            oMyAnagrafe.ComuneResidenza = StringOperation.Formatstring(myrow("comune_res"))
    '            oMyAnagrafe.ProvinciaResidenza = StringOperation.Formatstring(myrow("provincia_res"))
    '            '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
    '            'oMyAnagrafe.NomeInvio = StringOperation.Formatstring(myrow("nome_invio"))
    '            'oMyAnagrafe.ViaRCP = StringOperation.Formatstring(myrow("via_rcp"))
    '            'oMyAnagrafe.CivicoRCP = StringOperation.Formatstring(myrow("civico_rcp"))
    '            'oMyAnagrafe.EsponenteCivicoRCP = StringOperation.Formatstring(myrow("esponente_rcp"))
    '            'oMyAnagrafe.InternoCivicoRCP = StringOperation.Formatstring(myrow("interno_rcp"))
    '            'oMyAnagrafe.ScalaCivicoRCP = StringOperation.Formatstring(myrow("scala_rcp"))
    '            'oMyAnagrafe.FrazioneRCP = StringOperation.Formatstring(myrow("frazione_rcp"))
    '            'oMyAnagrafe.CapRCP = StringOperation.Formatstring(myrow("cap_rcp"))
    '            'oMyAnagrafe.ComuneRCP = StringOperation.Formatstring(myrow("comune_rcp"))
    '            'oMyAnagrafe.ProvinciaRCP = StringOperation.Formatstring(myrow("provincia_rcp"))
    '            Dim mySped As New ObjIndirizziSpedizione
    '            mySped.NomeInvio = StringOperation.Formatstring(myrow("nome_invio"))
    '            mySped.ViaRCP = StringOperation.Formatstring(myrow("via_rcp"))
    '            mySped.CivicoRCP = StringOperation.Formatstring(myrow("civico_rcp"))
    '            mySped.EsponenteCivicoRCP = StringOperation.Formatstring(myrow("esponente_rcp"))
    '            mySped.InternoCivicoRCP = StringOperation.Formatstring(myrow("interno_rcp"))
    '            mySped.ScalaCivicoRCP = StringOperation.Formatstring(myrow("scala_rcp"))
    '            mySped.FrazioneRCP = StringOperation.Formatstring(myrow("frazione_rcp"))
    '            mySped.CapRCP = StringOperation.Formatstring(myrow("cap_rcp"))
    '            mySped.ComuneRCP = StringOperation.Formatstring(myrow("comune_rcp"))
    '            mySped.ProvinciaRCP = StringOperation.Formatstring(myrow("provincia_rcp"))
    '            oMyAnagrafe.ListSpedizioni.Add(mySped)
    '            '*** ***
    '            oFattura.oAnagrafeUtente = oMyAnagrafe
    '            oFattura.sNUtente = StringOperation.Formatstring(myrow("numeroutente"))
    '            oFattura.sMatricola = StringOperation.Formatstring(myrow("matricola"))
    '            oFattura.sViaContatore = StringOperation.Formatstring(myrow("via_contatore"))
    '            oFattura.sCivicoContatore = StringOperation.Formatstring(myrow("civico_contatore"))
    '            oFattura.sFrazioneContatore = StringOperation.Formatstring(myrow("frazione_contatore"))
    '            oFattura.nConsumo = StringOperation.FormatInt(myrow("consumo"))
    '            oFattura.nGiorni = StringOperation.FormatInt(myrow("giorni"))
    '            oFattura.nTipoUtenza = StringOperation.FormatInt(myrow("id_tipologia_utenza"))
    '            If Not IsDBNull(myrow("descrtipoutenza")) Then
    '                oFattura.sDescrTipoUtenza = StringOperation.Formatstring(myrow("descrtipoutenza"))
    '            End If
    '            oFattura.nTipoContatore = StringOperation.FormatInt(myrow("id_tipo_contatore"))
    '            If Not IsDBNull(myrow("descrtipocontatore")) Then
    '                oFattura.sDescrTipoContatore = StringOperation.Formatstring(myrow("descrtipocontatore"))
    '            End If
    '            If Not IsDBNull(myrow("codfognatura")) Then
    '                If StringOperation.Formatstring(myrow("codfognatura")) <> "" Then
    '                    oFattura.nCodFognatura = StringOperation.FormatInt(myrow("codfognatura"))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("coddepurazione")) Then
    '                If StringOperation.Formatstring(myrow("coddepurazione")) <> "" Then
    '                    oFattura.nCodDepurazione = StringOperation.FormatInt(myrow("coddepurazione"))
    '                End If
    '            End If
    '            oFattura.bEsenteAcqua = StringOperation.Formatbool(myrow("esenteacqua"))
    '            oFattura.bEsenteFognatura = StringOperation.Formatbool(myrow("esentefognatura"))
    '            oFattura.bEsenteDepurazione = StringOperation.Formatbool(myrow("esentedepurazione"))
    '            oFattura.nUtenze = StringOperation.FormatInt(myrow("nutenze"))
    '            If Not IsDBNull(myrow("data_lettura_prec")) Then
    '                If StringOperation.Formatstring(myrow("data_lettura_prec")) <> "" Then
    '                    oFattura.tDataLetturaPrec = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_lettura_prec")))
    '                End If
    '            End If
    '            oFattura.nLetturaPrec = StringOperation.FormatInt(myrow("lettura_prec"))
    '            If Not IsDBNull(myrow("data_lettura_att")) Then
    '                If StringOperation.Formatstring(myrow("data_lettura_att")) <> "" Then
    '                    oFattura.tDataLetturaAtt = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_lettura_att")))
    '                End If
    '            End If
    '            oFattura.nLetturaAtt = StringOperation.FormatInt(myrow("lettura_att"))
    '            oFattura.impScaglioni = StringOperation.Formatdouble(myrow("importo_scaglioni"))
    '            oFattura.impCanoni = StringOperation.Formatdouble(myrow("importo_canoni"))
    '            oFattura.impAddizionali = StringOperation.Formatdouble(myrow("importo_addizionali"))
    '            oFattura.impNolo = StringOperation.Formatdouble(myrow("importo_nolo"))
    '            oFattura.impQuoteFisse = StringOperation.Formatdouble(myrow("importo_quotafissa"))
    '            '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
    '            If Not IsDBNull(myrow("ESENTEACQUAQF")) Then
    '                oFattura.bEsenteAcquaQF = StringOperation.Formatbool(myrow("ESENTEACQUAQF"))
    '            End If
    '            If Not IsDBNull(myrow("ESENTEDEPURAZIONEQF")) Then
    '                oFattura.bEsenteDepQF = StringOperation.Formatbool(myrow("ESENTEDEPURAZIONEQF"))
    '            End If
    '            If Not IsDBNull(myrow("ESENTEFOGNATURAQF")) Then
    '                oFattura.bEsenteFogQF = StringOperation.Formatbool(myrow("ESENTEFOGNATURAQF"))
    '            End If
    '            If Not IsDBNull(myrow("importo_quotafissa_dep")) Then
    '                oFattura.impQuoteFisseDep = StringOperation.Formatdouble(myrow("importo_quotafissa_dep"))
    '            End If
    '            If Not IsDBNull(myrow("importo_quotafissa_fog")) Then
    '                oFattura.impQuoteFisseFog = StringOperation.Formatdouble(myrow("importo_quotafissa_fog"))
    '            End If
    '            '*** ***
    '            oFattura.impImponibile = StringOperation.Formatdouble(myrow("importo_imponibile"))
    '            oFattura.impIva = StringOperation.Formatdouble(myrow("importo_iva"))
    '            oFattura.impEsente = StringOperation.Formatdouble(myrow("importo_esente"))
    '            oFattura.impTotale = StringOperation.Formatdouble(myrow("importo_totale"))
    '            oFattura.impArrotondamento = StringOperation.Formatdouble(myrow("importo_arrotondamento"))
    '            oFattura.impFattura = StringOperation.Formatdouble(myrow("importo_fatturanota"))
    '            If Not IsDBNull(myrow("data_fattura_riferimento")) Then
    '                If StringOperation.Formatstring(myrow("data_fattura_riferimento")) <> "" Then
    '                    oFattura.tDataDocumentoRif = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_fattura_riferimento")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("numero_fattura_riferimento")) Then
    '                oFattura.sNDocumentoRif = StringOperation.Formatstring(myrow("numero_fattura_riferimento"))
    '            End If
    '            If Not IsDBNull(myrow("data_inserimento")) Then
    '                If StringOperation.Formatstring(myrow("data_inserimento")) <> "" Then
    '                    oFattura.tDataInserimento = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_inserimento")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("data_variazione")) Then
    '                If StringOperation.Formatstring(myrow("data_variazione")) <> "" Then
    '                    oFattura.tDataVariazione = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_variazione")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("data_cessazione")) Then
    '                If StringOperation.Formatstring(myrow("data_cessazione")) <> "" Then
    '                    oFattura.tDataCessazione = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_cessazione")))
    '                End If
    '            End If
    '            oFattura.sOperatore = StringOperation.Formatstring(myrow("operatore"))
    '            'devo caricare anche le tariffe
    '            Dim FunctionTariffe As New ClsTariffe
    '            oFattura.oScaglioni = FunctionTariffe.GetFatturaScaglioni(oFattura.Id)
    '            oFattura.oQuoteFisse = FunctionTariffe.GetFatturaQuoteFisse(oFattura.Id)
    '            oFattura.oNolo = FunctionTariffe.GetFatturaNolo(oFattura.Id)
    '            oFattura.oCanoni = FunctionTariffe.GetFatturaCanoni(oFattura.Id)
    '            oFattura.oAddizionali = FunctionTariffe.GetFatturaAddizionali(oFattura.Id)
    '            oFattura.oDettaglioIva = FunctionTariffe.GetFatturaDettaglioIva(oFattura.Id)
    '        Loop

    '        Return oFattura
    '    Catch Err As Exception
    '        Log.Debug("Si è verificato un errore in ClsFatture::GetNotaCreditoNewFattura::" & Err.Message & " SQL: " & sSQL)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsFatture.GetNotaCreditoNewFattura.errore: ", Err)
    '        Return Nothing
    '    Finally
    '        dvmydati.dispose()
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nIdFattura"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetLettureVSFattura(ByVal nIdFattura As Integer) As ObjLettura()
        Dim oLettura As ObjLettura
        Dim oListLetture() As ObjLettura
        Dim nList As Integer = -1

        oListLetture = Nothing
        Try
            Dim sSQL As String = ""
            Dim dvMyDati As New DataView
            Try
                Dim oDbManagerRepository As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                Using ctx As DBModel = oDbManagerRepository
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetLettureVSFattura", "IDFATTURA")
                    dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDFATTURA", nIdFattura))
                    ctx.Dispose()
                End Using
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        oLettura = New ObjLettura
                        oLettura.IdLettura = StringOperation.FormatInt(myRow("idlettura"))
                        'ridimensiono l'array
                        nList += 1
                        ReDim Preserve oListLetture(nList)
                        oListLetture(nList) = oLettura
                    Next
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsFatture.GetLettureVSFattura.query.errore: ", ex)
            Finally
                dvMyDati.Dispose()
            End Try

            Return oListLetture
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsFatture.GetLettureVSFattura.errore: ", Err)
            Return Nothing
        End Try
    End Function
    'Public Function GetLettureVSFattura(ByVal nIdFattura As Integer) As ObjLettura()
    '    Dim sSQL As String=""
    '    Dim dvMyDati As new dataview=nothing
    '    Dim oLettura As ObjLettura
    '    Dim oListLetture() As ObjLettura
    '    Dim nList As Integer = -1

    '    Try
    '        sSQL = "SELECT *"
    '        sSQL += " FROM TR_LETTURE_FATTURE"
    '        sSQL += " WHERE (IDFATTURA=" & nIdFattura & ")"
    '        'eseguo la query
    '        dvMyDati = iDB.getdataview(sSQL)
    '        Do While dvMyDati.Read
    '            oLettura = New ObjLettura
    '            oLettura.IdLettura = StringOperation.FormatInt(myrow("idlettura"))
    '            'ridimensiono l'array
    '            nList += 1
    '            ReDim Preserve oListLetture(nList)
    '            oListLetture(nList) = oLettura
    '        Loop

    '        Return oListLetture
    '    Catch Err As Exception
    '        Log.Debug("Si è verificato un errore in ClsLetture::GetLettureVSFattura::" & Err.Message & " SQL: " & sSQL)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsFatture.GetLettureVSFattura.errore: ", Err)
    '        Return Nothing
    '    Finally
    '        dvmydati.dispose()
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sEnte"></param>
    ''' <param name="sAnno"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFirstNFattura(ByVal sEnte As String, ByVal sAnno As String) As Integer
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Try
            GetFirstNFattura = 1
            Try
                Dim oDbManagerRepository As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                Using ctx As DBModel = oDbManagerRepository
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetFirstNFattura", "IDENTE", "ANNO")
                    dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", sEnte) _
                    , ctx.GetParam("ANNO", sAnno)
                )
                    ctx.Dispose()
                End Using
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        GetFirstNFattura = StringOperation.FormatInt(myRow("nfattura")) + 1
                    Next
                End If
            Catch ex As Exception
                Log.Debug(sEnte + " - OPENgovH2O.ClsFatture.GetFirstNFattura.query.errore: ", ex)
            Finally
                dvMyDati.Dispose()
            End Try
        Catch Err As Exception
            Log.Debug(sEnte + " - OPENgovH2O.ClsFatture.GetFirstNFattura.errore: ", Err)
            Return Nothing
        End Try
    End Function
    'Public Function GetFirstNFattura(ByVal sEnte As String, ByVal sAnno As String) As Integer
    '    Dim dvMyDati As new dataview=nothing
    '    Dim sSQL As String=""
    '    Try

    '        GetFirstNFattura = 1
    '        sSQL = "SELECT TOP 1 NUMERO_DOCUMENTO AS NFATTURA"
    '        sSQL += " FROM TP_FATTURE_NOTE"
    '        sSQL += " WHERE (TP_FATTURE_NOTE.DATA_VARIAZIONE IS NULL) AND (IDENTE='" & sEnte & "') AND (SUBSTRING(DATA_FATTURA,1,4)='" & sAnno & "')"
    '        sSQL += " ORDER BY NUMERO_DOCUMENTO DESC"
    '        Log.Debug("ClsFatture::GetFirstNFattura::sql::" & sSQL)
    '        'eseguo la query
    '        dvMyDati = iDB.getdataview(sSQL)
    '        Do While dvMyDati.Read
    '            If Not IsDBNull(myrow("nfattura")) Then
    '                If StringOperation.FormatString(myrow("nfattura")) <> "" Then
    '                    GetFirstNFattura = StringOperation.FormatInt(myrow("nfattura")) + 1
    '                End If
    '            End If
    '        Loop
    '    Catch Err As Exception
    '        Log.Debug("Si è verificato un errore in ClsFatture::GetFirstNFattura::" & Err.Message & " SQL: " & sSQL)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsFatture.GetFirstNFattura.errore: ", Err)
    '        Return Nothing
    '    Finally
    '        dvmydati.dispose()
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oRicercaDoc"></param>
    ''' <param name="sTipoBlocco"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Function GetRicercaDocumenti(ByVal oRicercaDoc As ObjRicercaDoc, ByVal sTipoBlocco As String) As ObjAnagDocumenti()
        'STIPOBLOCCO= V - blocco per le variazioni, E - blocco per le elaborazioni 
        Dim sSQL As String = ""
        Dim dvMyDati As DataView = Nothing
        Dim oDocumento As New ObjAnagDocumenti
        Dim oListDocumenti() As ObjAnagDocumenti = Nothing
        Dim nList As Integer = -1

        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetRicercaDocumenti", "TIPOBLOCCO", "IDENTE", "PROVENIENZA", "IDPERIODO", "TIPODOCUMENTO", "COGNOME", "NOME", "CFPIVA", "DATADOCUMENTO", "NDOCUMENTO", "MATRICOLA")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("TIPOBLOCCO", sTipoBlocco) _
                        , ctx.GetParam("IDENTE", oRicercaDoc.sEnte) _
                        , ctx.GetParam("PROVENIENZA", oRicercaDoc.sProvenienza) _
                        , ctx.GetParam("IDPERIODO", oRicercaDoc.nPeriodo) _
                        , ctx.GetParam("TIPODOCUMENTO", oRicercaDoc.sTipoDocumento) _
                        , ctx.GetParam("COGNOME", oRicercaDoc.sCognome) _
                        , ctx.GetParam("NOME", oRicercaDoc.sNome) _
                        , ctx.GetParam("CFPIVA", oRicercaDoc.sCFPIva) _
                        , ctx.GetParam("DATADOCUMENTO", oRicercaDoc.tDataDocumento) _
                        , ctx.GetParam("NDOCUMENTO", oRicercaDoc.sNDocumento) _
                        , ctx.GetParam("MATRICOLA", oRicercaDoc.sMatricola)
                    )
                ctx.Dispose()
            End Using
            For Each myRow As DataRowView In dvMyDati
                oDocumento = New ObjAnagDocumenti
                oDocumento.nIdDocumento = StringOperation.FormatInt(myRow("idfatturanota"))
                oDocumento.sPeriodo = StringOperation.FormatString(myRow("periodo"))
                oDocumento.sCognome = StringOperation.FormatString(myRow("cognome_denominazione"))
                oDocumento.sNome = StringOperation.FormatString(myRow("nome"))
                oDocumento.sCodFiscalePIva = StringOperation.FormatString(myRow("cf_piva"))
                oDocumento.sMatricola = StringOperation.FormatString(myRow("matricola"))
                oDocumento.sTipoDocumento = StringOperation.FormatString(myRow("tipo_doc"))
                oDocumento.tDataDocumento = StringOperation.FormatDateTime(myRow("data_fattura"))
                oDocumento.sNDocumento = StringOperation.FormatString(myRow("numero_fattura"))
                oDocumento.impDocumento = StringOperation.FormatDouble(myRow("importo_fatturanota"))
                oDocumento.sVariato = StringOperation.FormatString(myRow("variato"))
                'ridimensiono l'array
                nList += 1
                ReDim Preserve oListDocumenti(nList)
                oListDocumenti(nList) = oDocumento
            Next

            Return oListDocumenti
        Catch Err As Exception
            Log.Debug(oRicercaDoc.sEnte + " - OPENgovH2O.ClsFatture.GetRicercaDocumenti.errore: ", Err)
            Return Nothing
        Finally
            dvMyDati.Dispose()
        End Try
    End Function
    'Public Function GetRicercaDocumenti(ByVal oRicercaDoc As ObjRicercaDoc, ByVal sTipoBlocco As String) As ObjAnagDocumenti()
    '    'STIPOBLOCCO= V - blocco per le variazioni, E - blocco per le elaborazioni 
    '    Dim sSQL As String=""
    '    Dim dvMyDati As new dataview=nothing
    '    Dim oDocumento As ObjAnagDocumenti
    '    Dim oListDocumenti() As ObjAnagDocumenti
    '    Dim nList As Integer = -1

    '    Try
    '        sSQL = "SELECT TP_FATTURE_NOTE.IDFATTURANOTA, PERIODO, " & NomeDBAnagrafica & ".ANAGRAFICA.COGNOME_DENOMINAZIONE, " & NomeDBAnagrafica & ".ANAGRAFICA.NOME,"
    '        sSQL += " CF_PIVA = CASE WHEN NOT " & NomeDBAnagrafica & ".ANAGRAFICA.PARTITA_IVA IS NULL AND " & NomeDBAnagrafica & ".ANAGRAFICA.PARTITA_IVA<>'' THEN " & NomeDBAnagrafica & ".ANAGRAFICA.PARTITA_IVA ELSE " & NomeDBAnagrafica & ".ANAGRAFICA.COD_FISCALE END,"
    '        sSQL += " MATRICOLA, TIPO_DOC= CASE WHEN TIPO_DOCUMENTO='N' THEN 'NOTA DI CREDITO' ELSE 'FATTURA' END, DATA_FATTURA, NUMERO_FATTURA, IMPORTO_FATTURANOTA, VARIATO"
    '        sSQL += " FROM TP_FATTURE_NOTE INNER JOIN " & NomeDBAnagrafica & ".ANAGRAFICA ON TP_FATTURE_NOTE.COD_UTENTE=" & NomeDBAnagrafica & ".ANAGRAFICA.COD_CONTRIBUENTE"
    '        sSQL += " INNER JOIN TP_PERIODO ON TP_FATTURE_NOTE.IDPERIODO=TP_PERIODO.CODPERIODO"
    '        sSQL += " INNER JOIN ("
    '        sSQL += "SELECT TP_FATTURE_NOTE.IDFATTURANOTA, VARIATO= CASE WHEN TMPTABLE.RIFERIMENTO IS NULL THEN '' ELSE 'X' END "
    '        sSQL += " FROM TP_FATTURE_NOTE"
    '        sSQL += " LEFT JOIN ("
    '        If sTipoBlocco = "V" Or sTipoBlocco = "" Then
    '            sSQL += "SELECT DISTINCT TP_FATTURE_NOTE.IDENTE+DATA_FATTURA_RIFERIMENTO+NUMERO_FATTURA_RIFERIMENTO AS RIFERIMENTO"
    '            sSQL += " FROM TP_FATTURE_NOTE"
    '            sSQL += " WHERE (TP_FATTURE_NOTE.DATA_VARIAZIONE IS NULL) AND (NOT DATA_FATTURA_RIFERIMENTO+NUMERO_FATTURA_RIFERIMENTO IS NULL)"
    '            sSQL += " AND (TP_FATTURE_NOTE.IDENTE='" & oRicercaDoc.sEnte & "')"
    '            sSQL += ") TMPTABLE ON TP_FATTURE_NOTE.IDENTE+TP_FATTURE_NOTE.DATA_FATTURA+TP_FATTURE_NOTE.NUMERO_FATTURA=TMPTABLE.RIFERIMENTO"
    '        Else
    '            sSQL += "SELECT DISTINCT DATA_APPROVAZIONE_MINUTA AS RIFERIMENTO, IDFATTURANOTA"
    '            sSQL += " FROM TP_FATTURAZIONI_GENERATE"
    '            sSQL += " INNER JOIN TP_FATTURE_NOTE ON TP_FATTURAZIONI_GENERATE.ID_FLUSSO=TP_FATTURE_NOTE.IDFLUSSO"
    '            sSQL += ") TMPTABLE ON TP_FATTURE_NOTE.IDFATTURANOTA=TMPTABLE.IDFATTURANOTA"
    '        End If
    '        sSQL += ") TMPTABLE2 ON TP_FATTURE_NOTE.IDFATTURANOTA=TMPTABLE2.IDFATTURANOTA"
    '        sSQL += " WHERE (DATA_FINE_VALIDITA IS NULL) AND (DATA_VARIAZIONE IS NULL) AND (TP_FATTURE_NOTE.IDENTE='" & oRicercaDoc.sEnte & "')"
    '        If oRicercaDoc.sProvenienza = "E" Then
    '            'arrivo dall'elaborazione, prendo tutto quello che è stato elaborato e non ancora inviato al cliente
    '            sSQL += " AND (IDFLUSSO IN (SELECT DISTINCT ID_FLUSSO FROM TP_FATTURAZIONI_GENERATE WHERE (DATA_APPROVAZIONE_DOCUMENTI IS NULL OR DATA_APPROVAZIONE_DOCUMENTI='')))"
    '        ElseIf oRicercaDoc.sProvenienza = "V" Or oRicercaDoc.sProvenienza = "C" Then
    '            'arrivo dalle variazioni, prendo tutto quello che è stato inviato al cliente
    '            sSQL += " AND (IDFLUSSO IN (SELECT DISTINCT ID_FLUSSO FROM TP_FATTURAZIONI_GENERATE WHERE (NOT DATA_APPROVAZIONE_DOCUMENTI IS NULL AND DATA_APPROVAZIONE_DOCUMENTI<>'')))"
    '            'sSQL += " AND (DATA_FATTURA+NUMERO_FATTURA NOT IN (SELECT DISTINCT DATA_FATTURA_RIFERIMENTO+NUMERO_FATTURA_RIFERIMENTO FROM TP_FATTURE_NOTE WHERE (NOT DATA_FATTURA_RIFERIMENTO+NUMERO_FATTURA_RIFERIMENTO IS NULL)))"
    '        End If
    '        If oRicercaDoc.nPeriodo <> -1 Then
    '            sSQL += " AND (IDPERIODO=" & oRicercaDoc.nPeriodo & ")"
    '        End If
    '        If oRicercaDoc.sTipoDocumento <> "" Then
    '            sSQL += " AND (TIPO_DOCUMENTO='" & oRicercaDoc.sTipoDocumento & "')"
    '        End If
    '        If oRicercaDoc.sCognome <> "" Then
    '            sSQL += " AND (" & NomeDBAnagrafica & ".ANAGRAFICA.COGNOME_DENOMINAZIONE LIKE '" & oReplace.ReplaceCharsForSearch(oRicercaDoc.sCognome) & "%')"
    '        End If
    '        If oRicercaDoc.sNome <> "" Then
    '            sSQL += " AND (" & NomeDBAnagrafica & ".ANAGRAFICA.NOME LIKE '" & oReplace.ReplaceCharsForSearch(oRicercaDoc.sNome) & "%')"
    '        End If
    '        If oRicercaDoc.sCFPIva <> "" Then
    '            sSQL += " AND (CASE WHEN NOT " & NomeDBAnagrafica & ".ANAGRAFICA.PARTITA_IVA IS NULL AND " & NomeDBAnagrafica & ".ANAGRAFICA.PARTITA_IVA<>'' THEN " & NomeDBAnagrafica & ".ANAGRAFICA.PARTITA_IVA ELSE " & NomeDBAnagrafica & ".ANAGRAFICA.COD_FISCALE END LIKE '" & oReplace.ReplaceCharsForSearch(oRicercaDoc.sCFPIva) & "%')"
    '        End If
    '        If oRicercaDoc.tDataDocumento.ToShortDateString <> Date.MaxValue.ToShortDateString Then
    '            sSQL += " AND (DATA_FATTURA='" & oReplace.GiraData(oRicercaDoc.tDataDocumento) & "')"
    '        End If
    '        If oRicercaDoc.sNDocumento <> "" Then
    '            sSQL += " AND (NUMERO_FATTURA='" & oRicercaDoc.sNDocumento & "')"
    '        End If
    '        If oRicercaDoc.sMatricola <> "" Then
    '            sSQL += " AND (MATRICOLA='" & oRicercaDoc.sMatricola & "')"
    '        End If
    '        sSQL += " ORDER BY " & NomeDBAnagrafica & ".ANAGRAFICA.COGNOME_DENOMINAZIONE, " & NomeDBAnagrafica & ".ANAGRAFICA.NOME, TIPO_DOCUMENTO, DATA_FATTURA, NUMERO_FATTURA"
    '        'eseguo la query
    '        dvMyDati = iDB.getdataview(sSQL)
    '        Do While dvMyDati.Read
    '            oDocumento = New ObjAnagDocumenti
    '            oDocumento.nIdDocumento = StringOperation.FormatInt(myrow("idfatturanota"))
    '            oDocumento.sPeriodo = StringOperation.FormatString(myrow("periodo"))
    '            oDocumento.sCognome = StringOperation.FormatString(myrow("cognome_denominazione"))
    '            oDocumento.sNome = StringOperation.FormatString(myrow("nome"))
    '            oDocumento.sCodFiscalePIva = StringOperation.FormatString(myrow("cf_piva"))
    '            oDocumento.sMatricola = StringOperation.FormatString(myrow("matricola"))
    '            oDocumento.sTipoDocumento = StringOperation.FormatString(myrow("tipo_doc"))
    '            If Not IsDBNull(myrow("data_fattura")) Then
    '                oDocumento.tDataDocumento = oReplace.GiraDataFromDB(StringOperation.FormatString(myrow("data_fattura")))
    '            End If
    '            oDocumento.sNDocumento = StringOperation.FormatString(myrow("numero_fattura"))
    '            oDocumento.impDocumento = StringOperation.FormatDouble(myrow("importo_fatturanota"))
    '            oDocumento.sVariato = StringOperation.FormatString(myrow("variato"))
    '            'ridimensiono l'array
    '            nList += 1
    '            ReDim Preserve oListDocumenti(nList)
    '            oListDocumenti(nList) = oDocumento
    '        Loop

    '        Return oListDocumenti
    '    Catch Err As Exception
    '        Log.Debug("Si è verificato un errore in ClsFatture::GetRicercaDocumenti::" & Err.Message & " SQL: " & sSQL)

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsFatture.GetRicercaDocumenti.errore: ", Err)
    '        Return Nothing
    '    Finally
    '        dvmydati.dispose()
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oRicercaDoc"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Function GetTotaliRicDoc(ByVal oRicercaDoc As ObjRicercaDoc) As ObjTotalizzatoriDocumenti
        Dim sSQL As String = ""
        Dim oTotaliRic As New ObjTotalizzatoriDocumenti
        Dim dvMyDati As DataView = Nothing

        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetTotaliRicDoc", "IDENTE", "PROVENIENZA", "IDFLUSSO", "IDPERIODO", "TIPODOC", "COGNOME", "NOME", "CFPIVA", "DATADOCUMENTO", "NDOCUMENTO", "MATRICOLA")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", oRicercaDoc.sEnte) _
                        , ctx.GetParam("PROVENIENZA", oRicercaDoc.sProvenienza) _
                        , ctx.GetParam("IDFLUSSO", oRicercaDoc.nFlusso) _
                        , ctx.GetParam("IDPERIODO", oRicercaDoc.nPeriodo) _
                        , ctx.GetParam("TIPODOC", oRicercaDoc.sTipoDocumento) _
                        , ctx.GetParam("COGNOME", oRicercaDoc.sCognome) _
                        , ctx.GetParam("NOME", oRicercaDoc.sNome) _
                        , ctx.GetParam("CFPIVA", oRicercaDoc.sCFPIva) _
                        , ctx.GetParam("DATADOCUMENTO", oRicercaDoc.tDataDocumento) _
                        , ctx.GetParam("NDOCUMENTO", oRicercaDoc.sNDocumento) _
                        , ctx.GetParam("MATRICOLA", oRicercaDoc.sMatricola)
                    )
                ctx.Dispose()
            End Using
            For Each myRow As DataRowView In dvMyDati
                oTotaliRic.nContribuenti = StringOperation.FormatInt(myRow("nutenti"))
                oTotaliRic.nFatture = StringOperation.FormatInt(myRow("nfatture"))
                oTotaliRic.nNote = StringOperation.FormatInt(myRow("nnote"))
                oTotaliRic.impFatture = StringOperation.FormatDouble(myRow("impfatture"))
                oTotaliRic.impNote = StringOperation.FormatDouble(myRow("impnote"))
            Next

            Return oTotaliRic
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsFatture.GetTotaliRicDoc.errore: ", Err)
            Return Nothing
        Finally
            dvMyDati.Dispose()
        End Try
    End Function
    'Public Function GetTotaliRicDoc(ByVal oRicercaDoc As ObjRicercaDoc) As ObjTotalizzatoriDocumenti
    '    Dim sSQL As String=""
    '    Dim dvMyDati As new dataview=nothing
    '    Dim oTotaliRic As New ObjTotalizzatoriDocumenti

    '    Try
    '        sSQL = "SELECT TMPUTENTI.NUTENTI, SUM(CASE WHEN TIPO_DOCUMENTO='N' THEN 1 ELSE 0 END) AS NNOTE, SUM(CASE WHEN TIPO_DOCUMENTO='F' THEN 1 ELSE 0 END) AS NFATTURE, "
    '        sSQL += " SUM(CASE WHEN TIPO_DOCUMENTO='N' THEN IMPORTO_FATTURANOTA ELSE 0 END) AS IMPNOTE, SUM(CASE WHEN TIPO_DOCUMENTO='F' THEN IMPORTO_FATTURANOTA ELSE 0 END) AS IMPFATTURE"
    '        sSQL += " FROM ("
    '        sSQL += " SELECT IDENTE, COUNT(CONTRIBUENTE) AS NUTENTI"
    '        sSQL += " FROM ("
    '        sSQL += " SELECT DISTINCT COD_UTENTE AS CONTRIBUENTE, TP_FATTURE_NOTE.IDENTE"
    '        sSQL += " FROM TP_FATTURE_NOTE"
    '        sSQL += " INNER JOIN " & NomeDBAnagrafica & ".ANAGRAFICA ON TP_FATTURE_NOTE.COD_UTENTE=" & NomeDBAnagrafica & ".ANAGRAFICA.COD_CONTRIBUENTE"
    '        sSQL += " INNER JOIN TP_PERIODO ON TP_FATTURE_NOTE.IDPERIODO=TP_PERIODO.CODPERIODO"
    '        sSQL += " WHERE (DATA_FINE_VALIDITA IS NULL) AND (DATA_VARIAZIONE IS NULL) AND (TP_FATTURE_NOTE.IDENTE='" & oRicercaDoc.sEnte & "')"
    '        If oRicercaDoc.sProvenienza = "E" Then
    '            'arrivo dall'elaborazione, prendo tutto quello che è stato elaborato e non ancora inviato al cliente
    '            sSQL += " AND (IDFLUSSO IN (SELECT DISTINCT ID_FLUSSO FROM TP_FATTURAZIONI_GENERATE WHERE (DATA_APPROVAZIONE_DOCUMENTI IS NULL OR DATA_APPROVAZIONE_DOCUMENTI='')))"
    '        ElseIf oRicercaDoc.sProvenienza = "V" Or oRicercaDoc.sProvenienza = "C" Then
    '            'arrivo dalle variazioni, prendo tutto quello che è stato inviato al cliente
    '            sSQL += " AND (IDFLUSSO IN (SELECT DISTINCT ID_FLUSSO FROM TP_FATTURAZIONI_GENERATE WHERE (NOT DATA_APPROVAZIONE_DOCUMENTI IS NULL AND DATA_APPROVAZIONE_DOCUMENTI<>'')))"
    '            'sSQL += " AND (DATA_FATTURA+NUMERO_FATTURA NOT IN (SELECT DISTINCT DATA_FATTURA_RIFERIMENTO+NUMERO_FATTURA_RIFERIMENTO FROM TP_FATTURE_NOTE WHERE (NOT DATA_FATTURA_RIFERIMENTO+NUMERO_FATTURA_RIFERIMENTO IS NULL)))"
    '        End If
    '        If oRicercaDoc.nFlusso <> -1 Then
    '            sSQL += " AND (IDFLUSSO=" & oRicercaDoc.nFlusso & ")"
    '        End If
    '        If oRicercaDoc.nPeriodo <> -1 Then
    '            sSQL += " AND (IDPERIODO=" & oRicercaDoc.nPeriodo & ")"
    '        End If
    '        If oRicercaDoc.sTipoDocumento <> "" Then
    '            sSQL += " AND (TIPO_DOCUMENTO='" & oRicercaDoc.sTipoDocumento & "')"
    '        End If
    '        If oRicercaDoc.sCognome <> "" Then
    '            sSQL += " AND (" & NomeDBAnagrafica & ".ANAGRAFICA.COGNOME_DENOMINAZIONE LIKE '" & oReplace.ReplaceCharsForSearch(oRicercaDoc.sCognome) & "%')"
    '        End If
    '        If oRicercaDoc.sNome <> "" Then
    '            sSQL += " AND (" & NomeDBAnagrafica & ".ANAGRAFICA.NOME LIKE '" & oReplace.ReplaceCharsForSearch(oRicercaDoc.sNome) & "%')"
    '        End If
    '        If oRicercaDoc.sCFPIva <> "" Then
    '            sSQL += " AND (CASE WHEN NOT " & NomeDBAnagrafica & ".ANAGRAFICA.PARTITA_IVA IS NULL AND " & NomeDBAnagrafica & ".ANAGRAFICA.PARTITA_IVA<>'' THEN " & NomeDBAnagrafica & ".ANAGRAFICA.PARTITA_IVA ELSE " & NomeDBAnagrafica & ".ANAGRAFICA.COD_FISCALE END LIKE '" & oReplace.ReplaceCharsForSearch(oRicercaDoc.sCFPIva) & "%')"
    '        End If
    '        If oRicercaDoc.tDataDocumento.ToShortDateString <> Date.MaxValue.ToShortDateString Then
    '            sSQL += " AND (DATA_FATTURA='" & oReplace.GiraData(oRicercaDoc.tDataDocumento) & "')"
    '        End If
    '        If oRicercaDoc.sNDocumento <> "" Then
    '            sSQL += " AND (NUMERO_FATTURA='" & oRicercaDoc.sNDocumento & "')"
    '        End If
    '        If oRicercaDoc.sMatricola <> "" Then
    '            sSQL += " AND (MATRICOLA='" & oRicercaDoc.sMatricola & "')"
    '        End If
    '        sSQL += " ) DERIVEDTABLE"
    '        sSQL += " GROUP BY IDENTE"
    '        sSQL += " ) TMPUTENTI "
    '        sSQL += " INNER JOIN TP_FATTURE_NOTE ON TMPUTENTI.IDENTE=TP_FATTURE_NOTE.IDENTE"
    '        sSQL += " INNER JOIN " & NomeDBAnagrafica & ".ANAGRAFICA ON TP_FATTURE_NOTE.COD_UTENTE=" & NomeDBAnagrafica & ".ANAGRAFICA.COD_CONTRIBUENTE"
    '        sSQL += " INNER JOIN TP_PERIODO ON TP_FATTURE_NOTE.IDPERIODO=TP_PERIODO.CODPERIODO"
    '        sSQL += " WHERE (DATA_FINE_VALIDITA IS NULL) AND (DATA_VARIAZIONE IS NULL) AND (TP_FATTURE_NOTE.IDENTE='" & oRicercaDoc.sEnte & "')"
    '        If oRicercaDoc.sProvenienza = "E" Then
    '            'arrivo dall'elaborazione, prendo tutto quello che è stato elaborato e non ancora inviato al cliente
    '            sSQL += " AND (IDFLUSSO IN (SELECT DISTINCT ID_FLUSSO FROM TP_FATTURAZIONI_GENERATE WHERE (DATA_APPROVAZIONE_DOCUMENTI IS NULL OR DATA_APPROVAZIONE_DOCUMENTI='')))"
    '        ElseIf oRicercaDoc.sProvenienza = "V" Then
    '            'arrivo dalle variazioni, prendo tutto quello che è stato inviato al cliente
    '            sSQL += " AND (IDFLUSSO IN (SELECT DISTINCT ID_FLUSSO FROM TP_FATTURAZIONI_GENERATE WHERE (NOT DATA_APPROVAZIONE_DOCUMENTI IS NULL AND DATA_APPROVAZIONE_DOCUMENTI<>'')))"
    '            'sSQL += " AND (DATA_FATTURA+NUMERO_FATTURA NOT IN (SELECT DISTINCT DATA_FATTURA_RIFERIMENTO+NUMERO_FATTURA_RIFERIMENTO FROM TP_FATTURE_NOTE WHERE (NOT DATA_FATTURA_RIFERIMENTO+NUMERO_FATTURA_RIFERIMENTO IS NULL)))"
    '        End If
    '        If oRicercaDoc.nFlusso <> -1 Then
    '            sSQL += " AND (IDFLUSSO=" & oRicercaDoc.nFlusso & ")"
    '        End If
    '        If oRicercaDoc.nPeriodo <> -1 Then
    '            sSQL += " AND (IDPERIODO=" & oRicercaDoc.nPeriodo & ")"
    '        End If
    '        If oRicercaDoc.sTipoDocumento <> "" Then
    '            sSQL += " AND (TIPO_DOCUMENTO='" & oRicercaDoc.sTipoDocumento & "')"
    '        End If
    '        If oRicercaDoc.sCognome <> "" Then
    '            sSQL += " AND (" & NomeDBAnagrafica & ".ANAGRAFICA.COGNOME_DENOMINAZIONE LIKE '" & oReplace.ReplaceCharsForSearch(oRicercaDoc.sCognome) & "%')"
    '        End If
    '        If oRicercaDoc.sNome <> "" Then
    '            sSQL += " AND (" & NomeDBAnagrafica & ".ANAGRAFICA.NOME LIKE '" & oReplace.ReplaceCharsForSearch(oRicercaDoc.sNome) & "%')"
    '        End If
    '        If oRicercaDoc.sCFPIva <> "" Then
    '            sSQL += " AND (CASE WHEN NOT " & NomeDBAnagrafica & ".ANAGRAFICA.PARTITA_IVA IS NULL AND " & NomeDBAnagrafica & ".ANAGRAFICA.PARTITA_IVA<>'' THEN " & NomeDBAnagrafica & ".ANAGRAFICA.PARTITA_IVA ELSE " & NomeDBAnagrafica & ".ANAGRAFICA.COD_FISCALE END LIKE '" & oReplace.ReplaceCharsForSearch(oRicercaDoc.sCFPIva) & "%')"
    '        End If
    '        If oRicercaDoc.tDataDocumento.ToShortDateString <> Date.MaxValue.ToShortDateString Then
    '            sSQL += " AND (DATA_FATTURA='" & oReplace.GiraData(oRicercaDoc.tDataDocumento) & "')"
    '        End If
    '        If oRicercaDoc.sNDocumento <> "" Then
    '            sSQL += " AND (NUMERO_FATTURA='" & oRicercaDoc.sNDocumento & "')"
    '        End If
    '        If oRicercaDoc.sMatricola <> "" Then
    '            sSQL += " AND (MATRICOLA='" & oRicercaDoc.sMatricola & "')"
    '        End If
    '        sSQL += " GROUP BY TMPUTENTI.NUTENTI"
    '        'eseguo la query
    '        dvMyDati = iDB.getdataview(sSQL)
    '        Do While dvMyDati.Read
    '            oTotaliRic.nContribuenti = StringOperation.FormatInt(myrow("nutenti"))
    '            oTotaliRic.nFatture = StringOperation.FormatInt(myrow("nfatture"))
    '            oTotaliRic.nNote = StringOperation.FormatInt(myrow("nnote"))
    '            oTotaliRic.impFatture = StringOperation.FormatDouble(myrow("impfatture"))
    '            oTotaliRic.impNote = StringOperation.FormatDouble(myrow("impnote"))
    '        Loop

    '        Return oTotaliRic
    '    Catch Err As Exception
    '        Log.Debug("Si è verificato un errore in ClsFatture::GetTotaliRicDoc::" & Err.Message & " SQL: " & sSQL)

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsFatture.GetTotaliRicDoc.errore: ", Err)
    '        Return Nothing
    '    Finally
    '        dvmydati.dispose()
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sEnte"></param>
    ''' <param name="tDataDocumento"></param>
    ''' <param name="sNPartenzaDocumento"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CheckIdentificativiDoc(ByVal sEnte As String, ByVal tDataDocumento As Date, ByVal sNPartenzaDocumento As String) As Integer
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView


        Try
            sSQL = "SELECT DISTINCT CAST(NUMERO_DOCUMENTO AS INTEGER) AS NFATTURA"
            sSQL += " FROM TP_FATTURE_NOTE"
            sSQL += " WHERE (IDENTE='" & sEnte & "') AND (SUBSTRING(DATA_FATTURA,1,4)='" & oReplace.GiraData(StringOperation.FormatString(tDataDocumento.ToShortDateString)).Substring(0, 4) & "')"
            sSQL += " ORDER BY CAST(NUMERO_DOCUMENTO AS INTEGER) DESC"
            'eseguo la query
            dvMyDati = iDB.GetDataView(sSQL)
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    If StringOperation.FormatInt(myRow("nfattura")) >= StringOperation.FormatInt(sNPartenzaDocumento) Then
                        Return 0
                    End If
                Next
            End If
            Return 1
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsFatture.CheckIdentificativiDoc.errore: ", Err)
            Return 0
        Finally
            dvMyDati.Dispose()
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sEnte"></param>
    ''' <param name="nFlusso"></param>
    ''' <param name="sTributo"></param>
    ''' <param name="tDataDoc"></param>
    ''' <param name="sNPartenzaDoc"></param>
    ''' <param name="sPrefissoNDoc"></param>
    ''' <param name="sSuffissoNDoc"></param>
    ''' <param name="tDataScadenza"></param>
    ''' <param name="sContoCorrente"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Esportazione completa dati</em>
    ''' </revision>
    ''' </revisionHistory>
    Public Function SetIdentificativiDoc(ByVal sEnte As String, ByVal nFlusso As Integer, ByVal sTributo As String, ByVal tDataDoc As Date, ByVal sNPartenzaDoc As String, ByVal sPrefissoNDoc As String, ByVal sSuffissoNDoc As String, ByVal tDataScadenza As Date, ByVal sContoCorrente As String) As Integer
        Dim nNumDoc, x As Integer
        Dim oListFatture() As ObjFattura
        Dim FncRate As New ClsRate
        Dim oListRate() As ObjConfiguraRata
        Dim oMyConfigRata As ObjConfiguraRata

        Try
            'prelevo le rate configurate
            Log.Debug("ClsFatture::SetIdentificativiDoc::prelevo prelevo le rate configurate")
            oListRate = FncRate.GetConfiguraRata(nFlusso)
            If oListRate Is Nothing Then
                'vuol dire che non ho configurato le rate perchè ho solo l'unica soluzione quindi le configuro adesso
                oMyConfigRata = New ObjConfiguraRata
                oMyConfigRata.nIdRuolo = nFlusso
                oMyConfigRata.sNRata = "U"
                oMyConfigRata.sIdEnte = sEnte
                oMyConfigRata.sDescrRata = "UNICA SOLUZIONE"
                oMyConfigRata.tDataScadenza = tDataScadenza
                ReDim Preserve oListRate(x)
                oListRate(x) = oMyConfigRata
                'inserisco le rate
                If FncRate.ConfiguraRata(oListRate, "") = False Then
                    Return 0
                End If
            End If
            'prelevo i documenti da numerare
            Log.Debug("ClsFatture::SetIdentificativiDoc::prelevo i documenti da numerare")
            oListFatture = GetFattura(ConstSession.StringConnection, sEnte, nFlusso, -1, False)
            If oListFatture Is Nothing Then
                Return 0
            End If
            'attribuisco la numerazione
            Log.Debug("ClsFatture::SetIdentificativiDoc::ciclo per attribuire la numerazione")
            nNumDoc = StringOperation.FormatInt(sNPartenzaDoc)
            For x = 0 To oListFatture.GetUpperBound(0)
                oListFatture(x).tDataDocumento = tDataDoc
                oListFatture(x).sNDocumento = sPrefissoNDoc + nNumDoc.ToString + sSuffissoNDoc
                Log.Debug("ClsFatture::SetIdentificativiDoc::setfattura progressivo::" & nNumDoc & ":: numero::" & sPrefissoNDoc + nNumDoc.ToString + sSuffissoNDoc)
                If SetFattura(oListFatture(x), 1, nNumDoc, -1) = 0 Then
                    Return 0
                End If
                If oListFatture(x).impFattura > 0 Then
                    Log.Debug("ClsFatture::SetIdentificativiDoc::creo rata")
                    If FncRate.CreaRateH2O(oListFatture(x), nNumDoc, oListRate, sContoCorrente) = 0 Then
                        Return 0
                    End If
                End If
                'incremento il numero fattura
                nNumDoc += 1
            Next
            Return 1
        Catch Err As Exception
            Log.Debug(sEnte + " - OPENgovH2O.ClsFatture.SetIdentificativiDoc.errore: ", Err)
            Return 0
        End Try
    End Function
    'Public Function SetIdentificativiDoc(ByVal sEnte As String, ByVal nFlusso As Integer, ByVal sTributo As String, ByVal tDataDoc As Date, ByVal sNPartenzaDoc As String, ByVal sPrefissoNDoc As String, ByVal sSuffissoNDoc As String, ByVal tDataScadenza As Date, ByVal sContoCorrente As String) As Integer
    '    Dim nNumDoc, x As Integer
    '    Dim oListFatture() As ObjFattura
    '    Dim FncRate As New ClsRate
    '    Dim oListRate() As ObjConfiguraRata
    '    Dim oMyConfigRata As ObjConfiguraRata

    '    Try
    '        'prelevo le rate configurate
    '        Log.Debug("ClsFatture::SetIdentificativiDoc::prelevo prelevo le rate configurate")
    '        oListRate = FncRate.GetConfiguraRata(nFlusso)
    '        If oListRate Is Nothing Then
    '            'vuol dire che non ho configurato le rate perchè ho solo l'unica soluzione quindi le configuro adesso
    '            oMyConfigRata = New ObjConfiguraRata
    '            oMyConfigRata.nIdRuolo = nFlusso
    '            oMyConfigRata.sNRata = "U"
    '            oMyConfigRata.sIdEnte = sEnte
    '            oMyConfigRata.sDescrRata = "UNICA SOLUZIONE"
    '            oMyConfigRata.tDataScadenza = tDataScadenza
    '            ReDim Preserve oListRate(x)
    '            oListRate(x) = oMyConfigRata
    '            'inserisco le rate
    '            If FncRate.ConfiguraRata(oListRate, "") = False Then
    '                Return 0
    '            End If
    '        End If
    '        'prelevo i documenti da numerare
    '        Log.Debug("ClsFatture::SetIdentificativiDoc::prelevo i documenti da numerare")
    '        oListFatture = GetFattura(sEnte, nFlusso, -1, False)
    '        If oListFatture Is Nothing Then
    '            Return 0
    '        End If
    '        'attribuisco la numerazione
    '        Log.Debug("ClsFatture::SetIdentificativiDoc::ciclo per attribuire la numerazione")
    '        nNumDoc = StringOperation.Formatint(sNPartenzaDoc)
    '        For x = 0 To oListFatture.GetUpperBound(0)
    '            oListFatture(x).tDataDocumento = tDataDoc
    '            oListFatture(x).sNDocumento = sPrefissoNDoc + nNumDoc.ToString + sSuffissoNDoc
    '            Log.Debug("ClsFatture::SetIdentificativiDoc::setfattura progressivo::" & nNumDoc & ":: numero::" & sPrefissoNDoc + nNumDoc.ToString + sSuffissoNDoc)
    '            If SetFattura(oListFatture(x), 1, nNumDoc) = 0 Then
    '                Return 0
    '            End If
    '            If oListFatture(x).impFattura > 0 Then
    '                Log.Debug("ClsFatture::SetIdentificativiDoc::creo rata")
    '                If FncRate.CreaRateH2O(oListFatture(x), nNumDoc, oListRate, sContoCorrente) = 0 Then
    '                    Return 0
    '                End If
    '            End If
    '            'incremento il numero fattura
    '            nNumDoc += 1
    '        Next
    '        Return 1
    '    Catch Err As Exception


    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsFatture.SetIdentificativiDoc.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oFattura"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetFatturaCompleta(ByVal oFattura As ObjFattura) As Integer
        Dim myIdentity, x As Integer
        Dim FunctionTariffe As New ClsTariffe

        Try
            If oFattura.sTipoDocumento = "N" Then
                Log.Debug("SetFatturaCompleta::inserisco la NOTA per " & oFattura.sMatricola)
                'inserisco la fattura
                myIdentity = SetNotaCredito(oFattura)
                If myIdentity = 0 Then
                    Return -1
                End If
            Else
                Log.Debug("SetFatturaCompleta::inserisco la fattura per " & oFattura.sMatricola)
                'inserisco la fattura
                myIdentity = SetFattura(oFattura, 0, -1, -1)
                If myIdentity = 0 Then
                    Return -1
                End If
                oFattura.Id = myIdentity
                Log.Debug("SetFatturaCompleta::inserito fattura id::" & myIdentity)
                'inserisco le tariffe
                If Not oFattura.oScaglioni Is Nothing Then
                    For x = 0 To oFattura.oScaglioni.GetUpperBound(0)
                        oFattura.oScaglioni(x).nIdFattura = oFattura.Id
                        myIdentity = FunctionTariffe.SetFatturaScaglioni(oFattura.oScaglioni(x), 0)
                        If myIdentity = 0 Then
                            SetFattura(oFattura, 2, -1, -1)
                            Return -1
                        End If
                    Next
                    Log.Debug("SetFatturaCompleta::inserito fattura scaglioni")
                End If
                If Not oFattura.oCanoni Is Nothing Then
                    For x = 0 To oFattura.oCanoni.GetUpperBound(0)
                        oFattura.oCanoni(x).nIdFattura = oFattura.Id
                        myIdentity = FunctionTariffe.SetFatturaCanoni(oFattura.oCanoni(x), 0)
                        If myIdentity = 0 Then
                            SetFattura(oFattura, 2, -1, -1)
                            Return -1
                        End If
                    Next
                    Log.Debug("SetFatturaCompleta::inserito fattura canoni")
                End If
                If Not oFattura.oAddizionali Is Nothing Then
                    For x = 0 To oFattura.oAddizionali.GetUpperBound(0)
                        oFattura.oAddizionali(x).nIdFattura = oFattura.Id
                        oFattura.oAddizionali(x).Id = -1
                        myIdentity = FunctionTariffe.SetFatturaAddizionali(oFattura.oAddizionali(x), 0)
                        If myIdentity = 0 Then
                            SetFattura(oFattura, 2, -1, -1)
                            Return -1
                        End If
                    Next
                    Log.Debug("SetFatturaCompleta::inserito fattura addizionali")
                End If
                If Not oFattura.oNolo Is Nothing Then
                    For x = 0 To oFattura.oNolo.GetUpperBound(0)
                        oFattura.oNolo(x).nIdFattura = oFattura.Id
                        myIdentity = FunctionTariffe.SetFatturaNolo(oFattura.oNolo(x), 0)
                        If myIdentity = 0 Then
                            SetFattura(oFattura, 2, -1, -1)
                            Return -1
                        End If
                    Next
                    Log.Debug("SetFatturaCompleta::inserito fattura nolo")
                End If
                If Not oFattura.oQuoteFisse Is Nothing Then
                    For x = 0 To oFattura.oQuoteFisse.GetUpperBound(0)
                        oFattura.oQuoteFisse(x).nIdFattura = oFattura.Id
                        myIdentity = FunctionTariffe.SetFatturaQuoteFisse(oFattura.oQuoteFisse(x), 0)
                        If myIdentity = 0 Then
                            SetFattura(oFattura, 2, -1, -1)
                            Return -1
                        End If
                    Next
                    Log.Debug("SetFatturaCompleta::inserito fattura quota fissa")
                End If
                If Not oFattura.oDettaglioIva Is Nothing Then
                    For x = 0 To oFattura.oDettaglioIva.GetUpperBound(0)
                        oFattura.oDettaglioIva(x).nIdFatturaNota = oFattura.Id
                        myIdentity = FunctionTariffe.SetFatturaDettaglioIva(oFattura.oDettaglioIva(x), 0)
                        If myIdentity = 0 Then
                            SetFattura(oFattura, 2, -1, -1)
                            Return -1
                        End If
                    Next
                    Log.Debug("SetFatturaCompleta::inserito fattura dettaglio iva")
                End If
                'blocco la lettura
                If oFattura.sTipoDocumento <> "N" Then
                    myIdentity = BloccoLettura(oFattura.Id, oFattura.oLetture, 1)
                    If myIdentity = 0 Then
                        SetFattura(oFattura, 2, -1, -1)
                        Return -1
                    End If
                    Log.Debug("SetFatturaCompleta::bloccato lettura")
                End If
            End If

            Return myIdentity
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsFatture.SetFatturaCompleta.errore: ", Err)
            Return 0
        End Try
    End Function
    ''' <summary>
    ''' Esegue l'inserimento, l'aggiornamento o eliminazione di una fattura
    ''' </summary>
    ''' <param name="oFattura">oggetto di tipo ObjFattura</param>
    ''' <param name="nDbOperation">indica il tipo di operazione che si esegue dul database:
    ''' 0 = inserimento
    ''' 1 = aggiornamento
    ''' 2 = eliminazione</param>
    ''' <param name="nNumDoc"></param>
    ''' <param name="nFlusso">ID flusso</param>
    ''' <returns>Integer
    ''' Se si è verificato un errore ritorna 0
    ''' Nel caso di inserimento/aggiornamento ritorna l'ID del record della tabella TP_FATTURE_NOTE
    ''' Nel caso di eliminazione ritorna 1
    ''' </returns>
    ''' <remarks>
    ''' </remarks>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Function SetFattura(ByVal oFattura As ObjFattura, ByVal nDbOperation As Integer, ByVal nNumDoc As Integer, ByVal nFlusso As Integer) As Integer
        Dim sSQL As String = ""
        Dim NomeInvio, ViaRCP, CivicoRCP, EsponenteRCP, InternoRCP, ScalaRCP, FrazioneRCP, CapRCP, ComuneRCP, PvRCP As String
        Dim dvMyDati As New DataView
        Try
            Dim myIdentity, IDFattura As Integer
            Dim oDbManagerRepository As New DBModel(ConstSession.DBType, ConstSession.StringConnection)

            NomeInvio = "" : ViaRCP = "" : CivicoRCP = "" : EsponenteRCP = "" : InternoRCP = "" : ScalaRCP = "" : FrazioneRCP = "" : CapRCP = "" : ComuneRCP = "" : PvRCP = ""
            If Not oFattura Is Nothing Then
                For Each mySped As ObjIndirizziSpedizione In oFattura.oAnagrafeUtente.ListSpedizioni
                    If mySped.CodTributo = "9000" Then
                        NomeInvio = mySped.NomeInvio
                        ViaRCP = mySped.ViaRCP
                        CivicoRCP = mySped.CivicoRCP
                        EsponenteRCP = mySped.EsponenteCivicoRCP
                        InternoRCP = mySped.InternoCivicoRCP
                        ScalaRCP = mySped.ScalaCivicoRCP
                        FrazioneRCP = mySped.FrazioneRCP
                        CapRCP = mySped.CapRCP
                        ComuneRCP = mySped.ComuneRCP
                        PvRCP = mySped.ProvinciaRCP
                    End If
                Next
                IDFattura = oFattura.Id
            End If
            'costruisco la query
            Select Case nDbOperation
                Case 0 'insert
                    Try
                        Using ctx As DBModel = oDbManagerRepository
                            sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_TP_FATTURE_NOTE_IU", "IDFATTURANOTA", "IDENTE", "IDFLUSSO", "IDPERIODO", "TIPO_DOCUMENTO", "DATA_FATTURA", "NUMERO_FATTURA", "ANNO_RIFERIMENTO", "COD_INTESTATARIO", "COD_UTENTE", "COGNOME_DENOMINAZIONE", "NOME", "COD_FISCALE", "PARTITA_IVA", "VIA_RES", "CIVICO_RES", "ESPONENTE_RES", "INTERNO_RES", "SCALA_RES", "FRAZIONE_RES", "CAP_RES", "COMUNE_RES", "PROVINCIA_RES", "NOME_INVIO", "VIA_RCP", "CIVICO_RCP", "ESPONENTE_RCP", "INTERNO_RCP", "SCALA_RCP", "FRAZIONE_RCP", "CAP_RCP", "COMUNE_RCP", "PROVINCIA_RCP", "NUMEROUTENTE", "MATRICOLA", "VIA_CONTATORE", "CIVICO_CONTATORE", "FRAZIONE_CONTATORE", "CONSUMO", "GIORNI", "ID_TIPOLOGIA_UTENZA", "ID_TIPO_CONTATORE", "CODFOGNATURA", "CODDEPURAZIONE", "ESENTEACQUA", "ESENTEFOGNATURA", "ESENTEDEPURAZIONE", "NUTENZE", "DATA_LETTURA_PREC", "LETTURA_PREC", "DATA_LETTURA_ATT", "LETTURA_ATT", "IMPORTO_SCAGLIONI", "IMPORTO_CANONI", "IMPORTO_ADDIZIONALI", "IMPORTO_NOLO", "IMPORTO_QUOTAFISSA", "IMPORTO_QUOTAFISSA_DEP", "IMPORTO_QUOTAFISSA_FOG", "ESENTEACQUAQF", "ESENTEDEPURAZIONEQF", "ESENTEFOGNATURAQF", "IMPORTO_IMPONIBILE", "IMPORTO_IVA", "IMPORTO_ESENTE", "IMPORTO_TOTALE", "IMPORTO_ARROTONDAMENTO", "IMPORTO_FATTURANOTA", "DATA_FATTURA_RIFERIMENTO", "NUMERO_FATTURA_RIFERIMENTO", "DATA_INSERIMENTO", "DATA_VARIAZIONE", "DATA_CESSAZIONE", "OPERATORE")
                            dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDFATTURANOTA", oFattura.Id) _
                                        , ctx.GetParam("IDENTE", oFattura.sIdEnte) _
                                        , ctx.GetParam("IDFLUSSO", oFattura.nIdFlusso) _
                                        , ctx.GetParam("IDPERIODO", oFattura.nIdPeriodo) _
                                        , ctx.GetParam("TIPO_DOCUMENTO", oFattura.sTipoDocumento) _
                                        , ctx.GetParam("DATA_FATTURA", oReplace.GiraData(oFattura.tDataDocumento.ToString)) _
                                        , ctx.GetParam("NUMERO_FATTURA", oFattura.sNDocumento) _
                                        , ctx.GetParam("ANNO_RIFERIMENTO", oFattura.sAnno) _
                                        , ctx.GetParam("COD_INTESTATARIO", oFattura.nIdIntestatario) _
                                        , ctx.GetParam("COD_UTENTE", oFattura.nIdUtente) _
                                        , ctx.GetParam("COGNOME_DENOMINAZIONE", oFattura.oAnagrafeUtente.Cognome) _
                                        , ctx.GetParam("NOME", oFattura.oAnagrafeUtente.Nome) _
                                        , ctx.GetParam("COD_FISCALE", oFattura.oAnagrafeUtente.CodiceFiscale) _
                                        , ctx.GetParam("PARTITA_IVA", oFattura.oAnagrafeUtente.PartitaIva) _
                                        , ctx.GetParam("VIA_RES", oFattura.oAnagrafeUtente.ViaResidenza) _
                                        , ctx.GetParam("CIVICO_RES", oFattura.oAnagrafeUtente.CivicoResidenza) _
                                        , ctx.GetParam("ESPONENTE_RES", oFattura.oAnagrafeUtente.EsponenteCivicoResidenza) _
                                        , ctx.GetParam("INTERNO_RES", oFattura.oAnagrafeUtente.InternoCivicoResidenza) _
                                        , ctx.GetParam("SCALA_RES", oFattura.oAnagrafeUtente.ScalaCivicoResidenza) _
                                        , ctx.GetParam("FRAZIONE_RES", oFattura.oAnagrafeUtente.FrazioneResidenza) _
                                        , ctx.GetParam("CAP_RES", oFattura.oAnagrafeUtente.CapResidenza) _
                                        , ctx.GetParam("COMUNE_RES", oFattura.oAnagrafeUtente.ComuneResidenza) _
                                        , ctx.GetParam("PROVINCIA_RES", oFattura.oAnagrafeUtente.ProvinciaResidenza) _
                                        , ctx.GetParam("NOME_INVIO", NomeInvio) _
                                        , ctx.GetParam("VIA_RCP", ViaRCP) _
                                        , ctx.GetParam("CIVICO_RCP", CivicoRCP) _
                                        , ctx.GetParam("ESPONENTE_RCP", EsponenteRCP) _
                                        , ctx.GetParam("INTERNO_RCP", InternoRCP) _
                                        , ctx.GetParam("SCALA_RCP", ScalaRCP) _
                                        , ctx.GetParam("FRAZIONE_RCP", FrazioneRCP) _
                                        , ctx.GetParam("CAP_RCP", CapRCP) _
                                        , ctx.GetParam("COMUNE_RCP", ComuneRCP) _
                                        , ctx.GetParam("PROVINCIA_RCP", PvRCP) _
                                        , ctx.GetParam("NUMEROUTENTE", oFattura.sNUtente) _
                                        , ctx.GetParam("MATRICOLA", oFattura.sMatricola) _
                                        , ctx.GetParam("VIA_CONTATORE", oFattura.sViaContatore) _
                                        , ctx.GetParam("CIVICO_CONTATORE", oFattura.sCivicoContatore) _
                                        , ctx.GetParam("FRAZIONE_CONTATORE", oFattura.sFrazioneContatore) _
                                        , ctx.GetParam("CONSUMO", oFattura.nConsumo) _
                                        , ctx.GetParam("GIORNI", oFattura.nGiorni) _
                                        , ctx.GetParam("ID_TIPOLOGIA_UTENZA", oFattura.nTipoUtenza) _
                                        , ctx.GetParam("ID_TIPO_CONTATORE", oFattura.nTipoContatore) _
                                        , ctx.GetParam("CODFOGNATURA", oFattura.nCodFognatura) _
                                        , ctx.GetParam("CODDEPURAZIONE", oFattura.nCodDepurazione) _
                                        , ctx.GetParam("ESENTEACQUA", oFattura.bEsenteAcqua) _
                                        , ctx.GetParam("ESENTEFOGNATURA", oFattura.bEsenteFognatura) _
                                        , ctx.GetParam("ESENTEDEPURAZIONE", oFattura.bEsenteDepurazione) _
                                        , ctx.GetParam("NUTENZE", oFattura.nUtenze) _
                                        , ctx.GetParam("DATA_LETTURA_PREC", oReplace.GiraData(oFattura.tDataLetturaPrec.ToString)) _
                                        , ctx.GetParam("LETTURA_PREC", oFattura.nLetturaPrec) _
                                        , ctx.GetParam("DATA_LETTURA_ATT", oReplace.GiraData(oFattura.tDataLetturaAtt.ToString)) _
                                        , ctx.GetParam("LETTURA_ATT", oFattura.nLetturaAtt) _
                                        , ctx.GetParam("IMPORTO_SCAGLIONI", oFattura.impScaglioni) _
                                        , ctx.GetParam("IMPORTO_CANONI", oFattura.impCanoni) _
                                        , ctx.GetParam("IMPORTO_ADDIZIONALI", oFattura.impAddizionali) _
                                        , ctx.GetParam("IMPORTO_NOLO", oFattura.impNolo) _
                                        , ctx.GetParam("IMPORTO_QUOTAFISSA", oFattura.impQuoteFisse) _
                                        , ctx.GetParam("IMPORTO_QUOTAFISSA_DEP", oFattura.impQuoteFisseDep) _
                                        , ctx.GetParam("IMPORTO_QUOTAFISSA_FOG", oFattura.impQuoteFisseFog) _
                                        , ctx.GetParam("ESENTEACQUAQF", oFattura.bEsenteAcquaQF) _
                                        , ctx.GetParam("ESENTEDEPURAZIONEQF", oFattura.bEsenteDepQF) _
                                        , ctx.GetParam("ESENTEFOGNATURAQF", oFattura.bEsenteFogQF) _
                                        , ctx.GetParam("IMPORTO_IMPONIBILE", oFattura.impImponibile) _
                                        , ctx.GetParam("IMPORTO_IVA", oFattura.impIva) _
                                        , ctx.GetParam("IMPORTO_ESENTE", oFattura.impEsente) _
                                        , ctx.GetParam("IMPORTO_TOTALE", oFattura.impTotale) _
                                        , ctx.GetParam("IMPORTO_ARROTONDAMENTO", oFattura.impArrotondamento) _
                                        , ctx.GetParam("IMPORTO_FATTURANOTA", oFattura.impFattura) _
                                        , ctx.GetParam("DATA_FATTURA_RIFERIMENTO", oReplace.GiraData(oFattura.tDataDocumentoRif.ToString)) _
                                        , ctx.GetParam("NUMERO_FATTURA_RIFERIMENTO", oFattura.sNDocumentoRif) _
                                        , ctx.GetParam("DATA_INSERIMENTO", oReplace.GiraData(oFattura.tDataInserimento.ToString)) _
                                        , ctx.GetParam("DATA_VARIAZIONE", oReplace.GiraData(oFattura.tDataVariazione.ToString)) _
                                        , ctx.GetParam("DATA_CESSAZIONE", oReplace.GiraData(oFattura.tDataCessazione.ToString)) _
                                        , ctx.GetParam("OPERATORE", oFattura.sOperatore)
                                    )
                            ctx.Dispose()
                        End Using
                        If Not dvMyDati Is Nothing Then
                            For Each myRow As DataRowView In dvMyDati
                                myIdentity = Utility.StringOperation.FormatString(myRow(0))
                            Next
                        End If
                    Catch ex As Exception
                        Log.Debug(oFattura.sIdEnte + " - OPENgovH2O.ClsFatture.SetFattura.Insert.errore:  ", ex)
                    Finally
                        dvMyDati.Dispose()
                    End Try
                Case 1 'update
                    Try
                        Using ctx As DBModel = oDbManagerRepository
                            sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_TP_FATTURE_NOTE_IU", "IDFATTURANOTA", "IDENTE", "IDFLUSSO", "IDPERIODO", "TIPO_DOCUMENTO", "DATA_FATTURA", "NUMERO_FATTURA", "ANNO_RIFERIMENTO", "COD_INTESTATARIO", "COD_UTENTE", "COGNOME_DENOMINAZIONE", "NOME", "COD_FISCALE", "PARTITA_IVA", "VIA_RES", "CIVICO_RES", "ESPONENTE_RES", "INTERNO_RES", "SCALA_RES", "FRAZIONE_RES", "CAP_RES", "COMUNE_RES", "PROVINCIA_RES", "NOME_INVIO", "VIA_RCP", "CIVICO_RCP", "ESPONENTE_RCP", "INTERNO_RCP", "SCALA_RCP", "FRAZIONE_RCP", "CAP_RCP", "COMUNE_RCP", "PROVINCIA_RCP", "NUMEROUTENTE", "MATRICOLA", "VIA_CONTATORE", "CIVICO_CONTATORE", "FRAZIONE_CONTATORE", "CONSUMO", "GIORNI", "ID_TIPOLOGIA_UTENZA", "ID_TIPO_CONTATORE", "CODFOGNATURA", "CODDEPURAZIONE", "ESENTEACQUA", "ESENTEFOGNATURA", "ESENTEDEPURAZIONE", "NUTENZE", "DATA_LETTURA_PREC", "LETTURA_PREC", "DATA_LETTURA_ATT", "LETTURA_ATT", "IMPORTO_SCAGLIONI", "IMPORTO_CANONI", "IMPORTO_ADDIZIONALI", "IMPORTO_NOLO", "IMPORTO_QUOTAFISSA", "IMPORTO_QUOTAFISSA_DEP", "IMPORTO_QUOTAFISSA_FOG", "ESENTEACQUAQF", "ESENTEDEPURAZIONEQF", "ESENTEFOGNATURAQF", "IMPORTO_IMPONIBILE", "IMPORTO_IVA", "IMPORTO_ESENTE", "IMPORTO_TOTALE", "IMPORTO_ARROTONDAMENTO", "IMPORTO_FATTURANOTA", "DATA_FATTURA_RIFERIMENTO", "NUMERO_FATTURA_RIFERIMENTO", "DATA_INSERIMENTO", "DATA_VARIAZIONE", "DATA_CESSAZIONE", "OPERATORE")
                            dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDFATTURANOTA", oFattura.Id) _
                                        , ctx.GetParam("IDENTE", oFattura.sIdEnte) _
                                        , ctx.GetParam("IDFLUSSO", oFattura.nIdFlusso) _
                                        , ctx.GetParam("IDPERIODO", oFattura.nIdPeriodo) _
                                        , ctx.GetParam("TIPO_DOCUMENTO", oFattura.sTipoDocumento) _
                                        , ctx.GetParam("DATA_FATTURA", oReplace.GiraData(oFattura.tDataDocumento.ToString)) _
                                        , ctx.GetParam("NUMERO_FATTURA", oFattura.sNDocumento) _
                                        , ctx.GetParam("ANNO_RIFERIMENTO", oFattura.sAnno) _
                                        , ctx.GetParam("COD_INTESTATARIO", oFattura.nIdIntestatario) _
                                        , ctx.GetParam("COD_UTENTE", oFattura.nIdUtente) _
                                        , ctx.GetParam("COGNOME_DENOMINAZIONE", oFattura.oAnagrafeUtente.Cognome) _
                                        , ctx.GetParam("NOME", oFattura.oAnagrafeUtente.Nome) _
                                        , ctx.GetParam("COD_FISCALE", oFattura.oAnagrafeUtente.CodiceFiscale) _
                                        , ctx.GetParam("PARTITA_IVA", oFattura.oAnagrafeUtente.PartitaIva) _
                                        , ctx.GetParam("VIA_RES", oFattura.oAnagrafeUtente.ViaResidenza) _
                                        , ctx.GetParam("CIVICO_RES", oFattura.oAnagrafeUtente.CivicoResidenza) _
                                        , ctx.GetParam("ESPONENTE_RES", oFattura.oAnagrafeUtente.EsponenteCivicoResidenza) _
                                        , ctx.GetParam("INTERNO_RES", oFattura.oAnagrafeUtente.InternoCivicoResidenza) _
                                        , ctx.GetParam("SCALA_RES", oFattura.oAnagrafeUtente.ScalaCivicoResidenza) _
                                        , ctx.GetParam("FRAZIONE_RES", oFattura.oAnagrafeUtente.FrazioneResidenza) _
                                        , ctx.GetParam("CAP_RES", oFattura.oAnagrafeUtente.CapResidenza) _
                                        , ctx.GetParam("COMUNE_RES", oFattura.oAnagrafeUtente.ComuneResidenza) _
                                        , ctx.GetParam("PROVINCIA_RES", oFattura.oAnagrafeUtente.ProvinciaResidenza) _
                                        , ctx.GetParam("NOME_INVIO", NomeInvio) _
                                        , ctx.GetParam("VIA_RCP", ViaRCP) _
                                        , ctx.GetParam("CIVICO_RCP", CivicoRCP) _
                                        , ctx.GetParam("ESPONENTE_RCP", EsponenteRCP) _
                                        , ctx.GetParam("INTERNO_RCP", InternoRCP) _
                                        , ctx.GetParam("SCALA_RCP", ScalaRCP) _
                                        , ctx.GetParam("FRAZIONE_RCP", FrazioneRCP) _
                                        , ctx.GetParam("CAP_RCP", CapRCP) _
                                        , ctx.GetParam("COMUNE_RCP", ComuneRCP) _
                                        , ctx.GetParam("PROVINCIA_RCP", PvRCP) _
                                        , ctx.GetParam("NUMEROUTENTE", oFattura.sNUtente) _
                                        , ctx.GetParam("MATRICOLA", oFattura.sMatricola) _
                                        , ctx.GetParam("VIA_CONTATORE", oFattura.sViaContatore) _
                                        , ctx.GetParam("CIVICO_CONTATORE", oFattura.sCivicoContatore) _
                                        , ctx.GetParam("FRAZIONE_CONTATORE", oFattura.sFrazioneContatore) _
                                        , ctx.GetParam("CONSUMO", oFattura.nConsumo) _
                                        , ctx.GetParam("GIORNI", oFattura.nGiorni) _
                                        , ctx.GetParam("ID_TIPOLOGIA_UTENZA", oFattura.nTipoUtenza) _
                                        , ctx.GetParam("ID_TIPO_CONTATORE", oFattura.nTipoContatore) _
                                        , ctx.GetParam("CODFOGNATURA", oFattura.nCodFognatura) _
                                        , ctx.GetParam("CODDEPURAZIONE", oFattura.nCodDepurazione) _
                                        , ctx.GetParam("ESENTEACQUA", oFattura.bEsenteAcqua) _
                                        , ctx.GetParam("ESENTEFOGNATURA", oFattura.bEsenteFognatura) _
                                        , ctx.GetParam("ESENTEDEPURAZIONE", oFattura.bEsenteDepurazione) _
                                        , ctx.GetParam("NUTENZE", oFattura.nUtenze) _
                                        , ctx.GetParam("DATA_LETTURA_PREC", oReplace.GiraData(oFattura.tDataLetturaPrec.ToString)) _
                                        , ctx.GetParam("LETTURA_PREC", oFattura.nLetturaPrec) _
                                        , ctx.GetParam("DATA_LETTURA_ATT", oReplace.GiraData(oFattura.tDataLetturaAtt.ToString)) _
                                        , ctx.GetParam("LETTURA_ATT", oFattura.nLetturaAtt) _
                                        , ctx.GetParam("IMPORTO_SCAGLIONI", oFattura.impScaglioni) _
                                        , ctx.GetParam("IMPORTO_CANONI", oFattura.impCanoni) _
                                        , ctx.GetParam("IMPORTO_ADDIZIONALI", oFattura.impAddizionali) _
                                        , ctx.GetParam("IMPORTO_NOLO", oFattura.impNolo) _
                                        , ctx.GetParam("IMPORTO_QUOTAFISSA", oFattura.impQuoteFisse) _
                                        , ctx.GetParam("IMPORTO_QUOTAFISSA_DEP", oFattura.impQuoteFisseDep) _
                                        , ctx.GetParam("IMPORTO_QUOTAFISSA_FOG", oFattura.impQuoteFisseFog) _
                                        , ctx.GetParam("ESENTEACQUAQF", oFattura.bEsenteAcquaQF) _
                                        , ctx.GetParam("ESENTEDEPURAZIONEQF", oFattura.bEsenteDepQF) _
                                        , ctx.GetParam("ESENTEFOGNATURAQF", oFattura.bEsenteFogQF) _
                                        , ctx.GetParam("IMPORTO_IMPONIBILE", oFattura.impImponibile) _
                                        , ctx.GetParam("IMPORTO_IVA", oFattura.impIva) _
                                        , ctx.GetParam("IMPORTO_ESENTE", oFattura.impEsente) _
                                        , ctx.GetParam("IMPORTO_TOTALE", oFattura.impTotale) _
                                        , ctx.GetParam("IMPORTO_ARROTONDAMENTO", oFattura.impArrotondamento) _
                                        , ctx.GetParam("IMPORTO_FATTURANOTA", oFattura.impFattura) _
                                        , ctx.GetParam("DATA_FATTURA_RIFERIMENTO", oReplace.GiraData(oFattura.tDataDocumentoRif.ToString)) _
                                        , ctx.GetParam("NUMERO_FATTURA_RIFERIMENTO", oFattura.sNDocumentoRif) _
                                        , ctx.GetParam("DATA_INSERIMENTO", oReplace.GiraData(oFattura.tDataInserimento.ToString)) _
                                        , ctx.GetParam("DATA_VARIAZIONE", oReplace.GiraData(oFattura.tDataVariazione.ToString)) _
                                        , ctx.GetParam("DATA_CESSAZIONE", oReplace.GiraData(oFattura.tDataCessazione.ToString)) _
                                        , ctx.GetParam("OPERATORE", oFattura.sOperatore)
                                    )
                            ctx.Dispose()
                        End Using
                        If Not dvMyDati Is Nothing Then
                            For Each myRow As DataRowView In dvMyDati
                                myIdentity = Utility.StringOperation.FormatString(myRow(0))
                            Next
                        End If
                    Catch ex As Exception
                        Log.Debug(oFattura.sIdEnte + " - OPENgovH2O.ClsFatture.SetFattura.Update.errore: ", ex)
                    Finally
                        dvMyDati.Dispose()
                    End Try
                    myIdentity = oFattura.Id
                Case 2
                    Try
                        Using ctx As DBModel = oDbManagerRepository
                            sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_TP_FATTURE_NOTE_D", "IDFLUSSO", "IDFATTURANOTA")
                            myIdentity = ctx.ExecuteNonQuery(sSQL, ctx.GetParam("IDFLUSSO", nFlusso), ctx.GetParam("IDFATTURANOTA", IDFattura))
                            ctx.Dispose()
                        End Using
                    Catch ex As Exception
                        Log.Debug(oFattura.sIdEnte + " - OPENgovH2O.ClsFatture.SetFattura.Delete.errore: ", ex)
                    Finally
                        dvMyDati.Dispose()
                    End Try
                    myIdentity = 1
            End Select

            Return myIdentity
        Catch Err As Exception
            Log.Debug(oFattura.sIdEnte + " - OPENgovH2O.ClsFatture.SetFattura.errore: ", Err)
            Return 0
        End Try
    End Function
    'Public Function SetFattura(ByVal oFattura As ObjFattura, ByVal nDbOperation As Integer, ByVal nNumDoc As Integer, Optional ByVal nFlusso As Integer = -1) As Integer
    '    Dim sSQL As String=""
    '    Try
    '        Dim myIdentity As Integer

    '        'costruisco la query
    '        Select Case nDbOperation
    '            Case 0
    '                sSQL = "INSERT INTO TP_FATTURE_NOTE (IDENTE, IDFLUSSO, IDPERIODO, TIPO_DOCUMENTO,"
    '                sSQL += "DATA_FATTURA, NUMERO_FATTURA, ANNO_RIFERIMENTO,"
    '                sSQL += "COD_INTESTATARIO, COD_UTENTE, COGNOME_DENOMINAZIONE, NOME, COD_FISCALE, PARTITA_IVA,"
    '                sSQL += "VIA_RES, CIVICO_RES, ESPONENTE_RES, INTERNO_RES, SCALA_RES, FRAZIONE_RES, CAP_RES, COMUNE_RES, PROVINCIA_RES,"
    '                sSQL += "NOME_INVIO, VIA_RCP, CIVICO_RCP, ESPONENTE_RCP, INTERNO_RCP, SCALA_RCP, FRAZIONE_RCP, CAP_RCP, COMUNE_RCP, PROVINCIA_RCP,"
    '                sSQL += "NUMEROUTENTE, MATRICOLA, VIA_CONTATORE, CIVICO_CONTATORE, FRAZIONE_CONTATORE, CONSUMO, GIORNI, ID_TIPOLOGIA_UTENZA, ID_TIPO_CONTATORE,"
    '                sSQL += "CODFOGNATURA, CODDEPURAZIONE, ESENTEACQUA, ESENTEFOGNATURA, ESENTEDEPURAZIONE,"
    '                sSQL += "NUTENZE, DATA_LETTURA_PREC, LETTURA_PREC, DATA_LETTURA_ATT, LETTURA_ATT,"
    '                sSQL += "IMPORTO_SCAGLIONI, IMPORTO_CANONI, IMPORTO_ADDIZIONALI, IMPORTO_NOLO, IMPORTO_QUOTAFISSA,"
    '                '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
    '                sSQL += "IMPORTO_QUOTAFISSA_DEP, IMPORTO_QUOTAFISSA_FOG, ESENTEACQUAQF, ESENTEDEPURAZIONEQF, ESENTEFOGNATURAQF,"
    '                '*** ***
    '                sSQL += "IMPORTO_IMPONIBILE, IMPORTO_IVA, IMPORTO_ESENTE, IMPORTO_TOTALE, IMPORTO_ARROTONDAMENTO, IMPORTO_FATTURANOTA,"
    '                sSQL += "DATA_FATTURA_RIFERIMENTO, NUMERO_FATTURA_RIFERIMENTO,"
    '                sSQL += "DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE, OPERATORE)"
    '                sSQL += "VALUES ('" & oFattura.sIdEnte & "'," & oFattura.nIdFlusso & "," & oFattura.nIdPeriodo & ",'" & oFattura.sTipoDocumento & "',"
    '                If oFattura.tDataDocumento <> Date.MinValue Then
    '                    sSQL += "'" & oReplace.GiraData(oFattura.tDataDocumento.ToString) & "',"
    '                Else
    '                    sSQL += "NULL,"
    '                End If
    '                sSQL += "'" & oFattura.sNDocumento & "','" & oFattura.sAnno & "',"
    '                sSQL += oFattura.nIdIntestatario & "," & oFattura.nIdUtente & ",'" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.Cognome) & "','" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.Nome) & "','" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.CodiceFiscale) & "','" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.PartitaIva) & "',"
    '                sSQL += "'" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.ViaResidenza) & "','" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.CivicoResidenza) & "','" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.EsponenteCivicoResidenza) & "','" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.InternoCivicoResidenza) & "','" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.ScalaCivicoResidenza) & "','" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.FrazioneResidenza) & "','" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.CapResidenza) & "','" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.ComuneResidenza) & "','" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.ProvinciaResidenza) & "',"
    '                '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
    '                'sSQL += "'" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.NomeInvio) & "','" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.ViaRCP) & "','" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.CivicoRCP) & "','" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.EsponenteCivicoRCP) & "','" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.InternoCivicoRCP) & "','" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.ScalaCivicoRCP) & "','" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.FrazioneRCP) & "','" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.CapRCP) & "','" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.ComuneRCP) & "','" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.ProvinciaRCP) & "',"
    '                Dim NomeInvio, ViaRCP, CivicoRCP, EsponenteRCP, InternoRCP, ScalaRCP, FrazioneRCP, CapRCP, ComuneRCP, PvRCP As String
    '                NomeInvio = "" : ViaRCP = "" : CivicoRCP = "" : EsponenteRCP = "" : InternoRCP = "" : ScalaRCP = "" : FrazioneRCP = "" : CapRCP = "" : ComuneRCP = "" : PvRCP = ""
    '                For Each mySped As ObjIndirizziSpedizione In oFattura.oAnagrafeUtente.ListSpedizioni
    '                    If mySped.CodTributo = "9000" Then
    '                        NomeInvio = mySped.NomeInvio
    '                        ViaRCP = mySped.ViaRCP
    '                        CivicoRCP = mySped.CivicoRCP
    '                        EsponenteRCP = mySped.EsponenteCivicoRCP
    '                        InternoRCP = mySped.InternoCivicoRCP
    '                        ScalaRCP = mySped.ScalaCivicoRCP
    '                        FrazioneRCP = mySped.FrazioneRCP
    '                        CapRCP = mySped.CapRCP
    '                        ComuneRCP = mySped.ComuneRCP
    '                        PvRCP = mySped.ProvinciaRCP
    '                    End If
    '                Next
    '                sSQL += "'" & oReplace.ReplaceChar(NomeInvio) & "','" & oReplace.ReplaceChar(ViaRCP) & "','" & oReplace.ReplaceChar(CivicoRCP) & "','" & oReplace.ReplaceChar(EsponenteRCP) & "','" & oReplace.ReplaceChar(InternoRCP) & "','" & oReplace.ReplaceChar(ScalaRCP) & "','" & oReplace.ReplaceChar(FrazioneRCP) & "','" & oReplace.ReplaceChar(CapRCP) & "','" & oReplace.ReplaceChar(ComuneRCP) & "','" & oReplace.ReplaceChar(PvRCP) & "',"
    '                '*** ***
    '                sSQL += "'" & oReplace.ReplaceChar(oFattura.sNUtente) & "','" & oReplace.ReplaceChar(oFattura.sMatricola) & "','" & oReplace.ReplaceChar(oFattura.sViaContatore) & "','" & oReplace.ReplaceChar(oFattura.sCivicoContatore) & "','" & oReplace.ReplaceChar(oFattura.sFrazioneContatore) & "'," & oFattura.nConsumo & "," & oFattura.nGiorni & "," & oFattura.nTipoUtenza & "," & oFattura.nTipoContatore & ","
    '                sSQL += oFattura.nCodFognatura & "," & oFattura.nCodDepurazione & ","
    '                If oFattura.bEsenteAcqua = False Then
    '                    sSQL += "0,"
    '                Else
    '                    sSQL += "1,"
    '                End If
    '                If oFattura.bEsenteFognatura = False Then
    '                    sSQL += "0,"
    '                Else
    '                    sSQL += "1,"
    '                End If
    '                If oFattura.bEsenteDepurazione = False Then
    '                    sSQL += "0,"
    '                Else
    '                    sSQL += "1,"
    '                End If
    '                sSQL += oFattura.nUtenze & ",'" & oReplace.GiraData(oFattura.tDataLetturaPrec.ToString) & "'," & oFattura.nLetturaPrec & ",'" & oReplace.GiraData(oFattura.tDataLetturaAtt.ToString) & "'," & oFattura.nLetturaAtt & ","
    '                sSQL += oReplace.ReplaceNumberForDB(oFattura.impScaglioni) & "," & oReplace.ReplaceNumberForDB(oFattura.impCanoni) & "," & oReplace.ReplaceNumberForDB(oFattura.impAddizionali) & "," & oReplace.ReplaceNumberForDB(oFattura.impNolo) & ""
    '                sSQL += "," & oReplace.ReplaceNumberForDB(oFattura.impQuoteFisse)
    '                '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
    '                sSQL += "," & oReplace.ReplaceNumberForDB(oFattura.impQuoteFisseDep)
    '                sSQL += "," & oReplace.ReplaceNumberForDB(oFattura.impQuoteFisseFog)
    '                If oFattura.bEsenteAcquaQF = False Then
    '                    sSQL += ",0"
    '                Else
    '                    sSQL += ",1"
    '                End If
    '                If oFattura.bEsenteDepQF = False Then
    '                    sSQL += ",0"
    '                Else
    '                    sSQL += ",1"
    '                End If
    '                If oFattura.bEsenteFogQF = False Then
    '                    sSQL += ",0"
    '                Else
    '                    sSQL += ",1"
    '                End If
    '                '*** ***
    '                sSQL += "," & oReplace.ReplaceNumberForDB(oFattura.impImponibile) & "," & oReplace.ReplaceNumberForDB(oFattura.impIva) & "," & oReplace.ReplaceNumberForDB(oFattura.impEsente) & "," & oReplace.ReplaceNumberForDB(oFattura.impTotale) & "," & oReplace.ReplaceNumberForDB(oFattura.impArrotondamento) & "," & oReplace.ReplaceNumberForDB(oFattura.impFattura) & ","
    '                If oFattura.tDataDocumentoRif <> Date.MinValue Then
    '                    sSQL += "'" & oReplace.GiraData(oFattura.tDataDocumentoRif.ToString) & "',"
    '                Else
    '                    sSQL += "NULL,"
    '                End If
    '                If oFattura.sNDocumentoRif <> "" Then
    '                    sSQL += "'" & oFattura.sNDocumentoRif & "',"
    '                Else
    '                    sSQL += "NULL,"
    '                End If
    '                sSQL += " '" & oReplace.GiraData(oFattura.tDataInserimento.ToString) & "',"
    '                If oFattura.tDataVariazione <> Date.MinValue Then
    '                    sSQL += "'" & oReplace.GiraData(oFattura.tDataVariazione.ToString) & "',"
    '                Else
    '                    sSQL += "NULL,"
    '                End If
    '                If oFattura.tDataCessazione <> Date.MinValue Then
    '                    sSQL += "'" & oReplace.GiraData(oFattura.tDataCessazione.ToString) & "',"
    '                Else
    '                    sSQL += "NULL,"
    '                End If
    '                sSQL += "'" & oFattura.sOperatore & "')"
    '                sSQL += " SELECT @@IDENTITY"
    '                'eseguo la query
    '                Dim dvMyDati As new dataview
    '                dvMyDati = iDB.getdataview(sSQL)
    '                Do While dvMyDati.Read
    '                    myIdentity = myrow(0)
    '                Loop
    '                dvmydati.dispose()
    '            Case 1
    '                sSQL = "UPDATE TP_FATTURE_NOTE SET IDENTE='" & oFattura.sIdEnte & "',"
    '                sSQL += "IDFLUSSO=" & oFattura.nIdFlusso & ","
    '                sSQL += "IDPERIODO=" & oFattura.nIdPeriodo & ","
    '                sSQL += "TIPO_DOCUMENTO='" & oFattura.sTipoDocumento & "',"
    '                If oFattura.tDataDocumento <> Date.MinValue Then
    '                    sSQL += "DATA_FATTURA='" & oReplace.GiraData(oFattura.tDataDocumento.ToString) & "',"
    '                Else
    '                    sSQL += "DATA_FATTURA=NULL,"
    '                End If
    '                sSQL += "NUMERO_FATTURA='" & oFattura.sNDocumento & "',"
    '                sSQL += "ANNO_RIFERIMENTO='" & oFattura.sAnno & "',"
    '                sSQL += "COD_INTESTATARIO=" & oFattura.nIdIntestatario & ","
    '                sSQL += "COD_UTENTE=" & oFattura.nIdUtente & ","
    '                sSQL += "COGNOME_DENOMINAZIONE='" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.Cognome) & "',"
    '                sSQL += "NOME='" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.Nome) & "',"
    '                sSQL += "COD_FISCALE='" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.CodiceFiscale) & "',"
    '                sSQL += "PARTITA_IVA='" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.PartitaIva) & "',"
    '                sSQL += "VIA_RES='" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.ViaResidenza) & "',"
    '                sSQL += "CIVICO_RES='" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.CivicoResidenza) & "',"
    '                sSQL += "ESPONENTE_RES='" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.EsponenteCivicoResidenza) & "',"
    '                sSQL += "INTERNO_RES='" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.InternoCivicoResidenza) & "',"
    '                sSQL += "SCALA_RES='" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.ScalaCivicoResidenza) & "',"
    '                sSQL += "FRAZIONE_RES='" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.FrazioneResidenza) & "',"
    '                sSQL += "CAP_RES='" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.CapResidenza) & "',"
    '                sSQL += "COMUNE_RES='" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.ComuneResidenza) & "',"
    '                sSQL += "PROVINCIA_RES='" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.ProvinciaResidenza) & "',"
    '                '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
    '                'sSQL += "NOME_INVIO='" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.NomeInvio) & "',"
    '                'sSQL += "VIA_RCP='" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.ViaRCP) & "',"
    '                'sSQL += "CIVICO_RCP='" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.CivicoRCP) & "',"
    '                'sSQL += "ESPONENTE_RCP='" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.EsponenteCivicoRCP) & "',"
    '                'sSQL += "INTERNO_RCP='" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.InternoCivicoRCP) & "',"
    '                'sSQL += "SCALA_RCP='" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.ScalaCivicoRCP) & "',"
    '                'sSQL += "FRAZIONE_RCP='" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.FrazioneRCP) & "',"
    '                'sSQL += "CAP_RCP='" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.CapRCP) & "',"
    '                'sSQL += "COMUNE_RCP='" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.ComuneRCP) & "',"
    '                'sSQL += "PROVINCIA_RCP='" & oReplace.ReplaceChar(oFattura.oAnagrafeUtente.ProvinciaRCP) & "',"
    '                For Each mySped As ObjIndirizziSpedizione In oFattura.oAnagrafeUtente.ListSpedizioni
    '                    If mySped.CodTributo = "9000" Then
    '                        sSQL += "NOME_INVIO='" & oReplace.ReplaceChar(mySped.NomeInvio) & "',"
    '                        sSQL += "VIA_RCP='" & oReplace.ReplaceChar(mySped.ViaRCP) & "',"
    '                        sSQL += "CIVICO_RCP='" & oReplace.ReplaceChar(mySped.CivicoRCP) & "',"
    '                        sSQL += "ESPONENTE_RCP='" & oReplace.ReplaceChar(mySped.EsponenteCivicoRCP) & "',"
    '                        sSQL += "INTERNO_RCP='" & oReplace.ReplaceChar(mySped.InternoCivicoRCP) & "',"
    '                        sSQL += "SCALA_RCP='" & oReplace.ReplaceChar(mySped.ScalaCivicoRCP) & "',"
    '                        sSQL += "FRAZIONE_RCP='" & oReplace.ReplaceChar(mySped.FrazioneRCP) & "',"
    '                        sSQL += "CAP_RCP='" & oReplace.ReplaceChar(mySped.CapRCP) & "',"
    '                        sSQL += "COMUNE_RCP='" & oReplace.ReplaceChar(mySped.ComuneRCP) & "',"
    '                        sSQL += "PROVINCIA_RCP='" & oReplace.ReplaceChar(mySped.ProvinciaRCP) & "',"
    '                    End If
    '                Next
    '                '*** ***
    '                sSQL += "CONSUMO=" & oFattura.nConsumo & ","
    '                sSQL += "ID_TIPOLOGIA_UTENZA=" & oFattura.nTipoUtenza & ","
    '                sSQL += "ID_TIPO_CONTATORE=" & oFattura.nTipoContatore & ","
    '                sSQL += "CODFOGNATURA=" & oFattura.nCodFognatura & ","
    '                sSQL += "CODDEPURAZIONE=" & oFattura.nCodDepurazione & ","
    '                If oFattura.bEsenteAcqua = False Then
    '                    sSQL += "ESENTEACQUA=0,"
    '                Else
    '                    sSQL += "ESENTEACQUA=1,"
    '                End If
    '                If oFattura.bEsenteFognatura = False Then
    '                    sSQL += "ESENTEFOGNATURA=0,"
    '                Else
    '                    sSQL += "ESENTEFOGNATURA=1,"
    '                End If
    '                If oFattura.bEsenteDepurazione = False Then
    '                    sSQL += "ESENTEDEPURAZIONE=0,"
    '                Else
    '                    sSQL += "ESENTEDEPURAZIONE=1,"
    '                End If
    '                sSQL += "NUTENZE=" & oFattura.nUtenze & ","
    '                sSQL += "DATA_LETTURA_PREC='" & oReplace.GiraData(oFattura.tDataLetturaPrec.ToString) & "',"
    '                sSQL += "LETTURA_PREC=" & oFattura.nLetturaPrec & ","
    '                sSQL += "DATA_LETTURA_ATT='" & oReplace.GiraData(oFattura.tDataLetturaAtt.ToString) & "',"
    '                sSQL += "LETTURA_ATT=" & oFattura.nLetturaAtt & ","
    '                sSQL += "IMPORTO_SCAGLIONI=" & oReplace.ReplaceNumberForDB(oFattura.impScaglioni) & ","
    '                sSQL += "IMPORTO_CANONI=" & oReplace.ReplaceNumberForDB(oFattura.impCanoni) & ","
    '                sSQL += "IMPORTO_ADDIZIONALI=" & oReplace.ReplaceNumberForDB(oFattura.impAddizionali) & ","
    '                sSQL += "IMPORTO_NOLO=" & oReplace.ReplaceNumberForDB(oFattura.impNolo) & ","
    '                sSQL += "IMPORTO_QUOTAFISSA=" & oReplace.ReplaceNumberForDB(oFattura.impQuoteFisse) & ","
    '                '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
    '                sSQL += "IMPORTO_QUOTAFISSA_DEP=" & oReplace.ReplaceNumberForDB(oFattura.impQuoteFisseDep) & ","
    '                sSQL += "IMPORTO_QUOTAFISSA_FOG=" & oReplace.ReplaceNumberForDB(oFattura.impQuoteFisseFog) & ","
    '                If oFattura.bEsenteAcquaQF = False Then
    '                    sSQL += "ESENTEACQUAQF=0,"
    '                Else
    '                    sSQL += "ESENTEACQUAQF=1,"
    '                End If
    '                If oFattura.bEsenteDepQF = False Then
    '                    sSQL += "ESENTEDEPURAZIONEQF=0,"
    '                Else
    '                    sSQL += "ESENTEDEPURAZIONEQF=1,"
    '                End If
    '                If oFattura.bEsenteFogQF = False Then
    '                    sSQL += "ESENTEFOGNATURAQF=0,"
    '                Else
    '                    sSQL += "ESENTEFOGNATURAQF=1,"
    '                End If
    '                '*** ***
    '                sSQL += "IMPORTO_IMPONIBILE=" & oReplace.ReplaceNumberForDB(oFattura.impImponibile) & ","
    '                sSQL += "IMPORTO_IVA=" & oReplace.ReplaceNumberForDB(oFattura.impIva) & ","
    '                sSQL += "IMPORTO_ESENTE=" & oReplace.ReplaceNumberForDB(oFattura.impEsente) & ","
    '                sSQL += "IMPORTO_TOTALE=" & oReplace.ReplaceNumberForDB(oFattura.impTotale) & ","
    '                sSQL += "IMPORTO_ARROTONDAMENTO=" & oReplace.ReplaceNumberForDB(oFattura.impArrotondamento) & ","
    '                sSQL += "IMPORTO_FATTURANOTA=" & oReplace.ReplaceNumberForDB(oFattura.impFattura) & ","
    '                If oFattura.tDataDocumentoRif <> Date.MinValue Then
    '                    sSQL += "DATA_FATTURA_RIFERIMENTO='" & oReplace.GiraData(oFattura.tDataDocumentoRif.ToString) & "',"
    '                Else
    '                    sSQL += "DATA_FATTURA_RIFERIMENTO=NULL,"
    '                End If
    '                If oFattura.sNDocumentoRif <> "" Then
    '                    sSQL += "NUMERO_FATTURA_RIFERIMENTO='" & oFattura.sNDocumentoRif & "',"
    '                Else
    '                    sSQL += "NUMERO_FATTURA_RIFERIMENTO=NULL,"
    '                End If
    '                sSQL += " NUMERO_DOCUMENTO=" & nNumDoc.ToString & ","
    '                sSQL += "DATA_INSERIMENTO='" & oReplace.GiraData(oFattura.tDataInserimento.ToString) & "',"
    '                If oFattura.tDataVariazione <> Date.MinValue Then
    '                    sSQL += "DATA_VARIAZIONE='" & oReplace.GiraData(oFattura.tDataVariazione.ToString) & "',"
    '                Else
    '                    sSQL += "DATA_VARIAZIONE=NULL,"
    '                End If
    '                If oFattura.tDataCessazione <> Date.MinValue Then
    '                    sSQL += "DATA_CESSAZIONE='" & oReplace.GiraData(oFattura.tDataCessazione.ToString) & "',"
    '                Else
    '                    sSQL += "DATA_CESSAZIONE=NULL,"
    '                End If
    '                sSQL += "OPERATORE='" & oFattura.sOperatore & "'"
    '                sSQL += " WHERE (IDFATTURANOTA = " & oFattura.Id & ")"
    '                'eseguo la query
    '                If iDB.ExecuteNonQuery(sSQL) <> 1 Then
    '                    Throw New Exception("errore in::" & sSQL)
    '                End If
    '                myIdentity = oFattura.Id
    '            Case 2
    '                sSQL = "DELETE"
    '                sSQL += " FROM TP_FATTURE_NOTE"
    '                sSQL += " WHERE"
    '                If nFlusso <> -1 Then
    '                    'devo cancellare le fatture create per il flusso
    '                    sSQL += " (TP_FATTURE_NOTE.IDFLUSSO =" & nFlusso & ") AND (TP_FATTURE_NOTE.DATA_FATTURA_RIFERIMENTO IS NULL)"
    '                    iDB.ExecuteNonQuery(sSQL)
    '                    'e svuotare il legame al flusso per le fatture da variazioni
    '                    sSQL = "UPDATE TP_FATTURE_NOTE SET IDFLUSSO=-1"
    '                    sSQL += " WHERE (TP_FATTURE_NOTE.IDFLUSSO =" & nFlusso & ")"
    '                    iDB.ExecuteNonQuery(sSQL)
    '                ElseIf Not oFattura Is Nothing Then
    '                    sSQL += " (TP_FATTURE_NOTE.IDFATTURANOTA=" & oFattura.Id & ")"
    '                    'eseguo la query
    '                    If iDB.ExecuteNonQuery(sSQL) <> 1 Then
    '                        Throw New Exception("errore in::" & sSQL)
    '                    End If
    '                End If
    '                myIdentity = 1
    '        End Select

    '        Return myIdentity
    '    Catch Err As Exception
    '        Log.Debug("Si è verificato un errore in ClsFatture::SetFattura::" & Err.Message & " SQL: " & sSQL)

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsFatture.SetFattura.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function
    Public Function SetNotaCredito(ByVal oMyFattura As ObjFattura) As Integer
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Dim myIdentity As Integer

        Try
            Dim oDbManagerRepository As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
            Using ctx As DBModel = oDbManagerRepository
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_SetNotaCredito", "IDNOTA", "IDFATTURA", "OPERATORE")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDNOTA", -1) _
                                    , ctx.GetParam("IDFATTURA", oMyFattura.Id) _
                                    , ctx.GetParam("OPERATORE", oMyFattura.sOperatore)
                                )
                ctx.Dispose()
            End Using
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    myIdentity = StringOperation.FormatInt(myRow("ID"))
                Next
            End If
            Log.Debug("executenonquery ha restituito->" + myIdentity.ToString)
            Return myIdentity
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsFatture.SetNotaCredito.errore: ", Err)
            Return 0
        Finally
            dvMyDati.Dispose()
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sEnte">stringa Codice Ente in lavorazione</param>
    ''' <param name="nPeriodo">ID periodo</param>
    ''' <param name="nFlusso">ID flusso</param>
    ''' <param name="oRuoloH2O">oggetto di tipo ObjTotRuoloFatture</param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    Public Function SetRuoloVariazioni(ByVal sEnte As String, ByVal nPeriodo As Integer, ByVal nFlusso As Integer, ByRef oRuoloH2O As ObjTotRuoloFatture) As Integer
        Dim sSQL As String = ""
        Dim myIdentity As Integer
        Dim oMyTot As New ObjTotalizzatoriDocumenti
        Dim oMyRic As New ObjRicercaDoc
        Dim FunctionRuolo As New ClsRuoloH2O

        Try
            'aggiorno il codice flusso sui documenti di variazione
            sSQL = "UPDATE TP_FATTURE_NOTE SET IDFLUSSO=" & nFlusso
            sSQL += " WHERE (IDFLUSSO=-1) AND (IDPERIODO =" & nPeriodo & ") AND (IDENTE='" & sEnte & "')"
            'eseguo la query
            iDB.ExecuteNonQuery(sSQL)
            'prelevo i totalizzatori per il flusso
            oMyRic.sEnte = sEnte
            oMyRic.nFlusso = nFlusso
            oMyTot = GetTotaliRicDoc(oMyRic)
            'aggiorno il ruolo
            oRuoloH2O.nNContribuenti = oMyTot.nContribuenti
            oRuoloH2O.nNDocumenti = oMyTot.nFatture + oMyTot.nNote
            oRuoloH2O.impPositivi = oMyTot.impFatture
            oRuoloH2O.impNegativi = oMyTot.impNote
            myIdentity = FunctionRuolo.SetRuoloH2O(oRuoloH2O, 1)
            If myIdentity = 0 Then
                Return Nothing
            End If

            Return myIdentity
        Catch Err As Exception
            Log.Debug("Si è verificato un errore in ClsFatture::SetRuoloVariazioni::" & Err.Message & " SQL: " & sSQL)

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsFatture.SetRuoloVariazioni.errore: ", Err)
            Return 0
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nIdFattura"></param>
    ''' <param name="oListLetture"></param>
    ''' <param name="nTypeBlocco"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function BloccoLettura(ByVal nIdFattura As Integer, ByVal oListLetture() As ObjLettura, ByVal nTypeBlocco As Integer) As Integer
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Dim myIdentity As Integer = 0
        Try

            For Each myLettura As ObjLettura In oListLetture
                Try
                    Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                        sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_TR_LETTURE_FATTURE_IU", "IDFATTURA", "IDLETTURA")
                        dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDFATTURA", nIdFattura) _
                                , ctx.GetParam("IDLETTURA", myLettura.IdLettura)
                            )
                        ctx.Dispose()
                    End Using
                    For Each myRow As DataRowView In dvMyDati
                        If Utility.StringOperation.FormatInt(myRow("id")) <= 0 Then
                            Throw New Exception("errore in blocco lettura " & myLettura.IdLettura)
                        End If
                    Next
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsFatture.BloccoLettura.errore: ", ex)
                    Return Nothing
                End Try
            Next
            myIdentity = 1
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsFatture.BloccoLettura.errore: ", Err)
        End Try
        Return myIdentity
    End Function
    '''' <summary>
    '''' 
    '''' </summary>
    '''' <param name="sIdEnte"></param>
    '''' <param name="sCodContribuente"></param>
    '''' <returns></returns>
    '''' <remarks></remarks>
    'Public Function GetInfoFattureContribuente(ByVal sIdEnte As String, ByVal sCodContribuente As String) As DataView
    '    Dim sSQL As String=""
    '    Dim dvMyDati As DataView
    '    Dim NOME_DATABASE_H2O As String

    '    Try
    '        NOME_DATABASE_H2O = ConfigurationManager.AppSettings("NOME_DATABASE_H20")

    '        sSQL = "SELECT DISTINCT IDFATTURANOTA, DATA_FATTURA, NUMERO_FATTURA, TIPO_DOCUMENTO, IMPEMESSO, SUM(IMPDET) AS IMPPAG, MATRICOLA "
    '        sSQL += " FROM " & NOME_DATABASE_H2O & ".dbo.OPENgov_FATTUREPAGAMENTI"
    '        sSQL += " WHERE (COD_UTENTE = " & sCodContribuente & ")"
    '        sSQL += " AND (IDENTE = '" & sIdEnte & "')"
    '        sSQL += " GROUP BY IDFATTURANOTA, DATA_FATTURA, NUMERO_FATTURA, TIPO_DOCUMENTO, IMPEMESSO, MATRICOLA"
    '        dvMyDati = iDB.GetDataView(sSQL)

    '        Return dvMyDati
    '    Catch Err As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsFatture.GetInfoFatturaContribuente.errore: ", Err)
    '        Return Nothing
    '    End Try
    'End Function
    Public Function GetMinutaRate(myStringConnection As String, IdEnte As String, ByVal nIdRuolo As Integer) As DataView
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dtMyDati As New DataTable()
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_GetMinutaXStampatoreRate"
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.AddWithValue("@IDENTE", IdEnte)
            cmdMyCommand.Parameters.AddWithValue("@IDFLUSSO", nIdRuolo)
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(dtMyDati)
            Return dtMyDati.DefaultView
        Catch Err As Exception
            Log.Debug(IdEnte + " - OPENgovH2O.ClsFatture.GetMinutaRate.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        Finally
            myAdapter.Dispose()
            dtMyDati.Dispose()
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function
End Class

Public Class ClsRuoloH2O
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ClsRuoloH2O))
    Private iDB As New DBAccess.getDBobject
    Private oReplace As New ClsGenerale.Generale

    'Public Function GetRuoloH2OAtt(ByVal sEnte As String, ByVal nPeriodo As Integer) As ObjTotRuoloFatture
    '    'preleva solo il ruolo in corso
    '    Dim culture As IFormatProvider
    '    culture = New System.Globalization.CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")
    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Dim sSQL, WFErrore As String
    '    Dim dvMyDati As new dataview=nothing
    '    Try
    '        Dim oMyRuoloH2O As New ObjTotRuoloFatture

    '        'inizializzo la connessione
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If
    '        sSQL = "SELECT TP_FATTURAZIONI_GENERATE.ID_FLUSSO, TP_FATTURAZIONI_GENERATE.IDPERIODO, TP_FATTURAZIONI_GENERATE.IDENTE,"
    '        sSQL += " TP_FATTURAZIONI_GENERATE.NUMERO_CONTRIBUENTI, TP_FATTURAZIONI_GENERATE.NUMERO_DOCUMENTI, TP_FATTURAZIONI_GENERATE.TOTALE_IMPORTI_POSITIVI, TP_FATTURAZIONI_GENERATE.TOTALE_IMPORTI_NEGATIVI,"
    '        sSQL += " TP_FATTURAZIONI_GENERATE.DATA_CALCOLI, TP_FATTURAZIONI_GENERATE.DATA_STAMPA_MINUTA, TP_FATTURAZIONI_GENERATE.DATA_APPROVAZIONE_MINUTA, TP_FATTURAZIONI_GENERATE.DATA_NUMERAZIONE, TP_FATTURAZIONI_GENERATE.DATA_EMISSIONE_FATTURA,"
    '        sSQL += " TP_FATTURAZIONI_GENERATE.DATA_APPROVAZIONE_DOCUMENTI, TP_FATTURAZIONI_GENERATE.DATA_SCADENZA,"
    '        sSQL += " TP_FATTURAZIONI_GENERATE.NOTE, TP_FATTURAZIONI_GENERATE.OPERATORE, TP_FATTURAZIONI_GENERATE.DATA_ESTRAZIONE_REGISTRO,"
    '        sSQL += " MIN(CAST(TP_FATTURE_NOTE.NUMERO_DOCUMENTO AS NUMERIC)) AS FIRSTNDOC, MAX(CAST(TP_FATTURE_NOTE.NUMERO_DOCUMENTO AS NUMERIC)) AS LASTNDOC"
    '        sSQL += " FROM TP_FATTURAZIONI_GENERATE"
    '        sSQL += " INNER JOIN TP_FATTURE_NOTE ON TP_FATTURAZIONI_GENERATE.ID_FLUSSO=TP_FATTURE_NOTE.IDFLUSSO"
    '        sSQL += " WHERE (TP_FATTURAZIONI_GENERATE.IDENTE='" & sEnte & "') AND (TP_FATTURAZIONI_GENERATE.IDPERIODO=" & nPeriodo & ") AND (TP_FATTURAZIONI_GENERATE.DATA_APPROVAZIONE_DOCUMENTI IS NULL)"
    '        sSQL += " GROUP BY TP_FATTURAZIONI_GENERATE.ID_FLUSSO, TP_FATTURAZIONI_GENERATE.IDPERIODO, TP_FATTURAZIONI_GENERATE.IDENTE,"
    '        sSQL += " TP_FATTURAZIONI_GENERATE.NUMERO_CONTRIBUENTI, TP_FATTURAZIONI_GENERATE.NUMERO_DOCUMENTI, TP_FATTURAZIONI_GENERATE.TOTALE_IMPORTI_POSITIVI, TP_FATTURAZIONI_GENERATE.TOTALE_IMPORTI_NEGATIVI,"
    '        sSQL += " TP_FATTURAZIONI_GENERATE.DATA_CALCOLI, TP_FATTURAZIONI_GENERATE.DATA_STAMPA_MINUTA, TP_FATTURAZIONI_GENERATE.DATA_APPROVAZIONE_MINUTA, TP_FATTURAZIONI_GENERATE.DATA_NUMERAZIONE, TP_FATTURAZIONI_GENERATE.DATA_EMISSIONE_FATTURA,"
    '        sSQL += " TP_FATTURAZIONI_GENERATE.DATA_APPROVAZIONE_DOCUMENTI, TP_FATTURAZIONI_GENERATE.DATA_SCADENZA,"
    '        sSQL += " TP_FATTURAZIONI_GENERATE.NOTE, TP_FATTURAZIONI_GENERATE.OPERATORE, TP_FATTURAZIONI_GENERATE.DATA_ESTRAZIONE_REGISTRO"
    '        'eseguo la query
    '        dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '        Do While dvMyDati.Read
    '            oMyRuoloH2O.sIdEnte = StringOperation.Formatstring(myrow("idente"))
    '            oMyRuoloH2O.IdFlusso = StringOperation.Formatint(myrow("id_flusso"))
    '            oMyRuoloH2O.nIdPeriodo = StringOperation.Formatint(myrow("idperiodo"))
    '            oMyRuoloH2O.nNContribuenti = StringOperation.Formatint(myrow("numero_contribuenti"))
    '            oMyRuoloH2O.nNDocumenti = StringOperation.Formatint(myrow("numero_documenti"))
    '            If Not IsDBNull(myrow("firstndoc")) Then
    '                oMyRuoloH2O.nFirstNDoc = StringOperation.Formatint(myrow("firstndoc"))
    '            End If
    '            If Not IsDBNull(myrow("lastndoc")) Then
    '                oMyRuoloH2O.nLastNDoc = StringOperation.Formatint(myrow("lastndoc"))
    '            End If
    '            oMyRuoloH2O.impPositivi = StringOperation.Formatdouble(myrow("totale_importi_positivi"))
    '            oMyRuoloH2O.impNegativi = StringOperation.Formatdouble(myrow("totale_importi_negativi"))
    '            If Not IsDBNull(myrow("data_calcoli")) Then
    '                If StringOperation.Formatstring(myrow("data_calcoli")) <> "" Then
    '                    oMyRuoloH2O.tDataCalcoli = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_calcoli")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("data_stampa_minuta")) Then
    '                If StringOperation.Formatstring(myrow("data_stampa_minuta")) <> "" Then
    '                    oMyRuoloH2O.tDataStampaMinuta = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_stampa_minuta")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("data_approvazione_minuta")) Then
    '                If StringOperation.Formatstring(myrow("data_approvazione_minuta")) <> "" Then
    '                    oMyRuoloH2O.tDataOkMinuta = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_approvazione_minuta")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("data_emissione_fattura")) Then
    '                If StringOperation.Formatstring(myrow("data_emissione_fattura")) <> "" Then
    '                    oMyRuoloH2O.tDataEmissioneFattura = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_emissione_fattura")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("data_scadenza")) Then
    '                If StringOperation.Formatstring(myrow("data_scadenza")) <> "" Then
    '                    oMyRuoloH2O.tDataScadenza = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_scadenza")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("data_numerazione")) Then
    '                If StringOperation.Formatstring(myrow("data_numerazione")) <> "" Then
    '                    oMyRuoloH2O.tDataNumerazione = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_numerazione")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("data_approvazione_documenti")) Then
    '                If StringOperation.Formatstring(myrow("data_approvazione_documenti")) <> "" Then
    '                    oMyRuoloH2O.tDataApprovazioneDOC = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_approvazione_documenti")))
    '                End If
    '            End If
    '            oMyRuoloH2O.sNote = StringOperation.Formatstring(myrow("note"))
    '            oMyRuoloH2O.sOperatore = StringOperation.Formatstring(myrow("operatore"))
    '        Loop

    '        Return oMyRuoloH2O
    '    Catch Err As Exception
    '        Log.Debug("Si è verificato un errore in ClsRuoloH2O::GetRuoloH2OAtt::" & Err.Message & " SQL: " & sSQL)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsRuoloH2O.GetRuoloH2OAtt.errore: ", Err)
    '        Return Nothing
    '    Finally
    '        dvmydati.dispose()
    '        WFSessione.Kill()
    '    End Try
    'End Function

    'Public Function GetRuoloH2OPrec(ByVal sEnte As String, ByVal nPeriodo As Integer) As ObjTotRuoloFatture()
    '    'preleva i ruoli precedenti
    '    Dim culture As IFormatProvider
    '    culture = New System.Globalization.CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")
    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Dim sSQL, WFErrore As String
    '    Dim dvMyDati As new dataview=nothing
    '    Dim oMyListRuoli() As ObjTotRuoloFatture
    '    Dim nList As Integer = -1
    '    Dim oMyRuoloH2O As ObjTotRuoloFatture
    '    Try

    '        'inizializzo la connessione
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If
    '        sSQL = "SELECT *"
    '        sSQL += " FROM TP_FATTURAZIONI_GENERATE R"
    '        sSQL += " LEFT JOIN ("
    '        sSQL += " 	SELECT IDENTE, IDPERIODO, IDFLUSSO, MIN(NUMERO_DOCUMENTO) AS FIRSTFATTURA, MAX(NUMERO_DOCUMENTO) AS LASTFATTURA"
    '        sSQL += " 	FROM TP_FATTURE_NOTE"
    '        sSQL += " 	GROUP BY IDENTE, IDPERIODO, IDFLUSSO"
    '        sSQL += " ) F ON R.IDENTE=F.IDENTE AND R.IDPERIODO=F.IDPERIODO AND R.ID_FLUSSO=F.IDFLUSSO "
    '        sSQL += " WHERE 1=1"
    '        sSQL += " AND (R.IDENTE='" & sEnte & "') AND (R.IDPERIODO=" & nPeriodo & ") AND (NOT R.DATA_APPROVAZIONE_DOCUMENTI IS NULL)"
    '        sSQL += " ORDER BY R.ID_FLUSSO"
    '        'eseguo la query
    '        dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '        Do While dvMyDati.Read
    '            oMyRuoloH2O = New ObjTotRuoloFatture
    '            oMyRuoloH2O.sIdEnte = StringOperation.Formatstring(myrow("idente"))
    '            oMyRuoloH2O.IdFlusso = StringOperation.Formatint(myrow("id_flusso"))
    '            oMyRuoloH2O.nIdPeriodo = StringOperation.Formatint(myrow("idperiodo"))
    '            oMyRuoloH2O.nNContribuenti = StringOperation.Formatint(myrow("numero_contribuenti"))
    '            oMyRuoloH2O.nNDocumenti = StringOperation.Formatint(myrow("numero_documenti"))
    '            oMyRuoloH2O.impPositivi = StringOperation.Formatdouble(myrow("totale_importi_positivi"))
    '            oMyRuoloH2O.impNegativi = StringOperation.Formatdouble(myrow("totale_importi_negativi"))
    '            If Not IsDBNull(myrow("data_calcoli")) Then
    '                If StringOperation.Formatstring(myrow("data_calcoli")) <> "" Then
    '                    oMyRuoloH2O.tDataCalcoli = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_calcoli")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("data_stampa_minuta")) Then
    '                If StringOperation.Formatstring(myrow("data_stampa_minuta")) <> "" Then
    '                    oMyRuoloH2O.tDataStampaMinuta = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_stampa_minuta")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("data_approvazione_minuta")) Then
    '                If StringOperation.Formatstring(myrow("data_approvazione_minuta")) <> "" Then
    '                    oMyRuoloH2O.tDataOkMinuta = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_approvazione_minuta")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("data_emissione_fattura")) Then
    '                If StringOperation.Formatstring(myrow("data_emissione_fattura")) <> "" Then
    '                    oMyRuoloH2O.tDataEmissioneFattura = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_emissione_fattura")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("data_scadenza")) Then
    '                If StringOperation.Formatstring(myrow("data_scadenza")) <> "" Then
    '                    oMyRuoloH2O.tDataScadenza = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_scadenza")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("data_numerazione")) Then
    '                If StringOperation.Formatstring(myrow("data_numerazione")) <> "" Then
    '                    oMyRuoloH2O.tDataNumerazione = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_numerazione")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("data_approvazione_documenti")) Then
    '                If StringOperation.Formatstring(myrow("data_approvazione_documenti")) <> "" Then
    '                    oMyRuoloH2O.tDataApprovazioneDOC = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_approvazione_documenti")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("firstfattura")) Then
    '                oMyRuoloH2O.nFirstNDoc = StringOperation.Formatint(myrow("firstfattura"))
    '            End If
    '            If Not IsDBNull(myrow("lastfattura")) Then
    '                oMyRuoloH2O.nLastNDoc = StringOperation.Formatint(myrow("lastfattura"))
    '            End If
    '            oMyRuoloH2O.sNote = StringOperation.Formatstring(myrow("note"))
    '            oMyRuoloH2O.sOperatore = StringOperation.Formatstring(myrow("operatore"))
    '            'incremento l'array
    '            nList += 1
    '            ReDim Preserve oMyListRuoli(nList)
    '            oMyListRuoli(nList) = oMyRuoloH2O
    '        Loop

    '        Return oMyListRuoli
    '    Catch Err As Exception
    '        Log.Debug("Si è verificato un errore in ClsRuoloH2O::GetRuoloH2OPrec::" & Err.Message & " SQL: " & sSQL)
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsRuoloH2O.GetRuoloH2OPrec.errore: ", Err)
    '        Return Nothing
    '    Finally
    '        dvmydati.dispose()
    '        WFSessione.Kill()
    '    End Try
    'End Function

    'Public Function SetRuoloH2O(ByVal oTotRuoloH2O As ObjTotRuoloFatture, ByVal nDbOperation As Integer, ByVal WFSessione As OPENUtility.CreateSessione, Optional ByVal nFlusso As Integer = -1) As Integer
    '    Dim culture As IFormatProvider
    '    culture = New System.Globalization.CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")
    '    Dim sSQL As String=""
    '    Try
    '        Dim myIdentity As Integer

    '        'costruisco la query
    '        Select Case nDbOperation
    '            Case 0
    '                sSQL = "INSERT INTO TP_FATTURAZIONI_GENERATE (IDPERIODO, IDENTE, DATA_CALCOLI,"
    '                sSQL += " NUMERO_CONTRIBUENTI, NUMERO_DOCUMENTI, TOTALE_IMPORTI_POSITIVI, TOTALE_IMPORTI_NEGATIVI,"
    '                sSQL += " DATA_STAMPA_MINUTA, DATA_APPROVAZIONE_MINUTA, DATA_NUMERAZIONE,"
    '                sSQL += " DATA_EMISSIONE_FATTURA, DATA_SCADENZA, NOTE, OPERATORE)"
    '                sSQL += "VALUES (" & oTotRuoloH2O.nIdPeriodo & ",'" & oTotRuoloH2O.sIdEnte & "','" & oReplace.GiraData(oTotRuoloH2O.tDataCalcoli.ToString) & "',"
    '                sSQL += oTotRuoloH2O.nNContribuenti & "," & oTotRuoloH2O.nNDocumenti & "," & oReplace.ReplaceNumberForDB(oTotRuoloH2O.impPositivi) & "," & oReplace.ReplaceNumberForDB(oTotRuoloH2O.impNegativi) & ","
    '                If oTotRuoloH2O.tDataStampaMinuta <> Date.MinValue Then
    '                    sSQL += "'" & oReplace.GiraData(oTotRuoloH2O.tDataStampaMinuta.ToString) & "',"
    '                Else
    '                    sSQL += "NULL,"
    '                End If
    '                If oTotRuoloH2O.tDataOkMinuta <> Date.MinValue Then
    '                    sSQL += "'" & oReplace.GiraData(oTotRuoloH2O.tDataOkMinuta.ToString) & "',"
    '                Else
    '                    sSQL += "NULL,"
    '                End If
    '                If oTotRuoloH2O.tDataNumerazione <> Date.MinValue Then
    '                    sSQL += "'" & oReplace.GiraData(oTotRuoloH2O.tDataNumerazione.ToString) & "',"
    '                Else
    '                    sSQL += "NULL,"
    '                End If
    '                If oTotRuoloH2O.tDataEmissioneFattura <> Date.MinValue Then
    '                    sSQL += "'" & oReplace.GiraData(oTotRuoloH2O.tDataEmissioneFattura.ToString) & "',"
    '                Else
    '                    sSQL += "NULL,"
    '                End If
    '                If oTotRuoloH2O.tDataScadenza <> Date.MinValue Then
    '                    sSQL += "'" & oReplace.GiraData(oTotRuoloH2O.tDataScadenza.ToString) & "',"
    '                Else
    '                    sSQL += "NULL,"
    '                End If
    '                sSQL += "'" & oReplace.ReplaceChar(oTotRuoloH2O.sNote) & "','" & oTotRuoloH2O.sOperatore & "')"
    '                sSQL += " SELECT @@IDENTITY"
    '                'eseguo la query
    '                Dim dvMyDati As new dataview
    '                dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '                Do While dvMyDati.Read
    '                    myIdentity = myrow(0)
    '                Loop
    '                dvmydati.dispose()
    '            Case 1
    '                sSQL = "UPDATE TP_FATTURAZIONI_GENERATE SET IDPERIODO=" & oTotRuoloH2O.nIdPeriodo & ","
    '                sSQL += "IDENTE='" & oTotRuoloH2O.sIdEnte & "',"
    '                sSQL += "DATA_CALCOLI='" & oReplace.GiraData(oTotRuoloH2O.tDataCalcoli.ToString) & "',"
    '                sSQL += "NUMERO_CONTRIBUENTI=" & oTotRuoloH2O.nNContribuenti & ","
    '                sSQL += "NUMERO_DOCUMENTI=" & oTotRuoloH2O.nNDocumenti & ","
    '                sSQL += "TOTALE_IMPORTI_POSITIVI=" & oReplace.ReplaceNumberForDB(oTotRuoloH2O.impPositivi) & ","
    '                sSQL += "TOTALE_IMPORTI_NEGATIVI=" & oReplace.ReplaceNumberForDB(oTotRuoloH2O.impNegativi) & ","
    '                If oTotRuoloH2O.tDataStampaMinuta <> Date.MinValue Then
    '                    sSQL += "DATA_STAMPA_MINUTA='" & oReplace.GiraData(oTotRuoloH2O.tDataStampaMinuta.ToString) & "',"
    '                End If
    '                If oTotRuoloH2O.tDataOkMinuta <> Date.MinValue Then
    '                    sSQL += "DATA_APPROVAZIONE_MINUTA='" & oReplace.GiraData(oTotRuoloH2O.tDataOkMinuta.ToString) & "',"
    '                End If
    '                If oTotRuoloH2O.tDataNumerazione <> Date.MinValue Then
    '                    sSQL += "DATA_NUMERAZIONE='" & oReplace.GiraData(oTotRuoloH2O.tDataNumerazione.ToString) & "',"
    '                End If
    '                If oTotRuoloH2O.tDataEmissioneFattura <> Date.MinValue Then
    '                    sSQL += "DATA_EMISSIONE_FATTURA='" & oReplace.GiraData(oTotRuoloH2O.tDataEmissioneFattura.ToString) & "',"
    '                End If
    '                If oTotRuoloH2O.tDataScadenza <> Date.MinValue Then
    '                    sSQL += "DATA_SCADENZA='" & oReplace.GiraData(oTotRuoloH2O.tDataScadenza.ToString) & "',"
    '                End If
    '                sSQL += "NOTE='" & oReplace.ReplaceChar(oTotRuoloH2O.sNote) & "',"
    '                sSQL += "OPERATORE='" & oTotRuoloH2O.sOperatore & "'"
    '                sSQL += " WHERE (ID_FLUSSO = " & oTotRuoloH2O.IdFlusso & ")"
    '                'eseguo la query
    '                If WFSessione.oSession.oAppDB.Execute(sSQL) <> 1 Then
    '                    Return 0
    '                End If
    '                myIdentity = oTotRuoloH2O.IdFlusso
    '            Case 2
    '                sSQL = "DELETE"
    '                sSQL += " FROM TP_FATTURAZIONI_GENERATE"
    '                If nFlusso <> -1 Then
    '                    sSQL += " WHERE (ID_FLUSSO =" & nFlusso & ")"
    '                    'eseguo la query
    '                    WFSessione.oSession.oAppDB.Execute(sSQL)
    '                ElseIf Not oTotRuoloH2O Is Nothing Then
    '                    sSQL += " WHERE (ID_FLUSSO =" & oTotRuoloH2O.IdFlusso & ")"
    '                    'eseguo la query
    '                    If WFSessione.oSession.oAppDB.Execute(sSQL) <> 1 Then
    '                        Return 0
    '                    End If
    '                End If
    '                myIdentity = 1
    '        End Select

    '        Return myIdentity
    '    Catch Err As Exception
    '        Log.Debug("Si è verificato un errore in ClsRuoloH2O::SetRuoloH2O::" & Err.Message & " SQL: " & sSQL)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsRuoloH2O.SetRuoloH2O.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function

    'Public Function SetDateRuoliH2OGenerati(ByVal IdSetRuolo As Integer, ByVal nDBOperation As Integer, Optional ByVal sTypeDBOperation As String = "I", Optional ByVal tDataRuoloH2O As String = "") As Integer
    '    Dim sSQL As String=""
    '    Dim WFErrore As String
    '    Dim WFSessione As OPENUtility.CreateSessione

    '    Try
    '        '***nDBOperation****************************
    '        '0 = imposta la data della stampa della minuta
    '        '1 = imposta la data di approvazione della minuta
    '        '2 = imposta la data di numerazione
    '        '3 = imposta la data di emissione fattura
    '        '4 = imposta la data di scadenza
    '        '5 = imposta la data di approvazione documenti
    '        '******************************************
    '        '***sTypeDBOperation************************
    '        'I = inserimento data
    '        'C = cancellazione data
    '        '******************************************
    '        'inizializzo la connessione
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If
    '        'costruisco la query
    '        Select Case nDBOperation
    '            Case 0
    '                sSQL = "UPDATE TP_FATTURAZIONI_GENERATE SET DATA_STAMPA_MINUTA="
    '                If sTypeDBOperation = "C" Then
    '                    sSQL += "NULL"
    '                Else
    '                    sSQL += "'" & oReplace.GiraData(Now) & "'"
    '                End If
    '                sSQL += " WHERE (ID_FLUSSO=" & IdSetRuolo & ")"
    '            Case 1
    '                sSQL = "UPDATE TP_FATTURAZIONI_GENERATE SET DATA_APPROVAZIONE_MINUTA="
    '                If sTypeDBOperation = "C" Then
    '                    sSQL += "NULL"
    '                Else
    '                    sSQL += "'" & oReplace.GiraData(Now) & "'"
    '                End If
    '                sSQL += " WHERE (ID_FLUSSO=" & IdSetRuolo & ")"
    '            Case 2
    '                sSQL = "UPDATE TP_FATTURAZIONI_GENERATE SET DATA_NUMERAZIONE="
    '                If sTypeDBOperation = "C" Then
    '                    sSQL += "NULL"
    '                Else
    '                    sSQL += "'" & oReplace.GiraData(Now) & "'"
    '                End If
    '                sSQL += " WHERE (ID_FLUSSO=" & IdSetRuolo & ")"
    '            Case 3
    '                sSQL = "UPDATE TP_FATTURAZIONI_GENERATE SET DATA_EMISSIONE_FATTURA="
    '                If sTypeDBOperation = "C" Then
    '                    sSQL += "NULL"
    '                Else
    '                    sSQL += "'" & oReplace.GiraData(tDataRuoloH2O) & "'"
    '                End If
    '                sSQL += " WHERE (ID_FLUSSO=" & IdSetRuolo & ")"
    '            Case 4
    '                sSQL = "UPDATE TP_FATTURAZIONI_GENERATE SET DATA_SCADENZA="
    '                If sTypeDBOperation = "C" Then
    '                    sSQL += "NULL"
    '                Else
    '                    sSQL += "'" & oReplace.GiraData(tDataRuoloH2O) & "'"
    '                End If
    '                sSQL += " WHERE (ID_FLUSSO=" & IdSetRuolo & ")"
    '            Case 5
    '                sSQL = "UPDATE TP_FATTURAZIONI_GENERATE SET DATA_APPROVAZIONE_DOCUMENTI="
    '                If sTypeDBOperation = "C" Then
    '                    sSQL += "NULL"
    '                Else
    '                    sSQL += "'" & oReplace.GiraData(Now) & "'"
    '                End If
    '                sSQL += " WHERE (ID_FLUSSO=" & IdSetRuolo & ")"
    '        End Select
    '        'eseguo la query
    '        If WFSessione.oSession.oAppDB.Execute(sSQL) <= 0 Then
    '            Return 0
    '        End If

    '        Return IdSetRuolo
    '    Catch Err As Exception
    '        Log.Debug("Si è verificato un errore in ClsRuoloH2O::SetDateRuoliH2OGenerati::" & Err.Message & " SQL: " & sSQL)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsRuoloH2O.SetDateRuoliH2OGenerati.errore: ", Err)
    '        Return 0
    '    Finally
    '        WFSessione.Kill()
    '    End Try
    'End Function

    'Public Function GetDatiDaFatturare(ByVal sEnte As String, ByVal nPeriodo As Integer, ByVal sTributo As String, ByVal sAnno As String, ByRef nContatoriNoLettura As Integer) As ObjTotRuoloFatture
    '    'preleva le letture da fatturare
    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Dim sSQL, WFErrore As String
    '    Dim FncLetture As New ClsLettureFattura
    '    Dim oMyListLetture() As ObjLettura
    '    Dim oRuoloH2O As New ObjTotRuoloFatture
    '    Dim x, nContribPrec As Integer
    '    Dim FncFatture As New ClsFatture
    '    Dim oListFatture() As ObjFattura

    '    Try
    '        'inizializzo la connessione
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If
    '        'prelevo i contatori senza lettura
    '        nContatoriNoLettura = FncLetture.GetContatoriSenzaLettura(sEnte, WFSessione)
    '        'prelevo le letture da fatturare
    '        oRuoloH2O = FncLetture.GetTotalizzatoriLetture(sEnte, nPeriodo, WFSessione)
    '        If oRuoloH2O Is Nothing Then
    '            Return Nothing
    '        End If
    '        'oMyListLetture = FncLetture.GetLetture(sEnte, nPeriodo, sTributo, sAnno, WFSessione, oRuoloH2O)
    '        'If Not oMyListLetture Is Nothing Then
    '        '    For x = 0 To oMyListLetture.GetUpperBound(0)
    '        '        If oMyListLetture(x).nIdIntestatario <> nContribPrec Then
    '        '            oRuoloH2O.nNContribuenti += 1
    '        '        End If
    '        '        oRuoloH2O.nNDocumenti += 1
    '        '        nContribPrec = oMyListLetture(x).nIdIntestatario
    '        '    Next
    '        'End If
    '        'prelevo le fatture da variazioni da fatturare
    '        oListFatture = FncFatture.GetFattura(sEnte, -1, -1, True, WFSessione)
    '        If Not oListFatture Is Nothing Then
    '            For x = 0 To oListFatture.GetUpperBound(0)
    '                If oListFatture(x).nIdIntestatario <> nContribPrec Then
    '                    oRuoloH2O.nNContribuenti += 1
    '                End If
    '                oRuoloH2O.nNDocumenti += 1
    '                nContribPrec = oListFatture(x).nIdIntestatario
    '            Next
    '        End If

    '        Return oRuoloH2O
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsRuoloH2O.GetDatiDaFatturare.errore: ", Err)
    '        Return Nothing
    '    Finally
    '        WFSessione.Kill()
    '    End Try
    'End Function

    'Public Function GetCartelleContribDaElaborare(ByVal idFlussoRuolo As Integer, ByVal idContribuente As Integer, ByVal NumeroFattura As String, ByVal DataFattura As String, ByVal sNominativoDa As String, ByVal sNominativoA As String) As ObjAnagDocumenti()
    '    Dim sSQL, WFErrore As String
    '    Dim WFSessione As New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))
    '    Dim WFSessioneEnte As New RIBESFrameWork.DBManager
    '    Dim CmDichiarazione As New SqlClient.SqlCommand
    '    Dim dvMyDati As new dataview=nothing
    '    Dim oReplace As New ClsGenerale.Generale
    '    Dim iDvDati As Integer
    '    Dim nListFatture As Integer
    '    Dim ObjFattura As ObjAnagDocumenti
    '    Dim ObjFatturaReturn() As ObjAnagDocumenti
    '    Dim culture As IFormatProvider
    '    culture = New System.Globalization.CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")
    '    Dim sNomeDbOpenGov As String = ConfigurationManager.AppSettings("NOME_DATABASE_OPENGOV")

    '    Try

    '        'inizializzo la connessione
    '        'apro la connessione al db
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("GetContribInDich::" & "Errore durante l'apertura della sessione di WorkFlow")
    '            Exit Function
    '        End If
    '        WFSessioneEnte = WFSessione.oSession.GetPrivateDBManager(HttpContext.Current.Session("IDENTIFICATIVOSOTTOAPPLICAZIONE"))

    '        sSQL = "SELECT TP_FATTURE_NOTE.*"
    '        sSQL += " FROM TP_FATTURE_NOTE"
    '        sSQL += " LEFT JOIN " & sNomeDbOpenGov & ".dbo.TBLGUIDA_COMUNICO ON TP_FATTURE_NOTE.IDFLUSSO = " & sNomeDbOpenGov & ".dbo.TBLGUIDA_COMUNICO.ID_FLUSSO_RUOLO AND"
    '        sSQL += " TP_FATTURE_NOTE.NUMERO_FATTURA = " & sNomeDbOpenGov & ".dbo.TBLGUIDA_COMUNICO.NUMERO_FATTURA AND TP_FATTURE_NOTE.DATA_FATTURA = " & sNomeDbOpenGov & ".dbo.TBLGUIDA_COMUNICO.DATA_FATTURA AND"
    '        sSQL += " TP_FATTURE_NOTE.IDENTE = " & sNomeDbOpenGov & ".dbo.TBLGUIDA_COMUNICO.IDENTE"
    '        sSQL += " WHERE (TP_FATTURE_NOTE.DATA_VARIAZIONE IS NULL)"
    '        sSQL += " AND (TP_FATTURE_NOTE.IDFLUSSO = " & idFlussoRuolo & ")"
    '        sSQL += " AND (TP_FATTURE_NOTE.IDENTE = '" & ConstSession.IdEnte & "')"
    '        sSQL += " AND (" & sNomeDbOpenGov & ".dbo.TBLGUIDA_COMUNICO.ID_FLUSSO_RUOLO IS NULL)"
    '        '***************************************
    '        'testo se bisogna ricercare per un determinato codice cartella
    '        '***************************************
    '        If NumeroFattura <> "" Then
    '            sSQL += " AND (TP_FATTURE_NOTE.NUMERO_FATTURA='" & NumeroFattura & "')"
    '        End If
    '        If DataFattura <> "" Then
    '            sSQL += " AND (TP_FATTURE_NOTE.DATA_FATTURA='" & oReplace.GiraData(DataFattura) & "')"
    '        End If
    '        If idContribuente > 0 Then
    '            sSQL += " AND (TBLCARTELLE.COD_CONTRIBUENTE = " & idContribuente & ")"
    '        End If
    '        If sNominativoA <> "" And sNominativoDa <> "" Then
    '            sSQL += " AND ((TP_FATTURE_NOTE.COGNOME_DENOMINAZIONE >='" & oReplace.ReplaceCharsForSearch(sNominativoDa) & "' AND TP_FATTURE_NOTE.COGNOME_DENOMINAZIONE <='" & oReplace.ReplaceCharsForSearch(sNominativoA) & "')"
    '            sSQL += " OR (TP_FATTURE_NOTE.COGNOME_DENOMINAZIONE LIKE '" & oReplace.ReplaceCharsForSearch(sNominativoA) & "%'))"
    '        ElseIf sNominativoDa <> "" Then
    '            sSQL += " AND (TP_FATTURE_NOTE.COGNOME_DENOMINAZIONE + ' ' + TP_FATTURE_NOTE.NOME  LIKE '" & oReplace.ReplaceCharsForSearch(sNominativoDa) & "%')"
    '        ElseIf sNominativoA <> "" Then
    '            sSQL += " AND (TP_FATTURE_NOTE.COGNOME_DENOMINAZIONE + ' ' + TP_FATTURE_NOTE.NOME LIKE '" & oReplace.ReplaceCharsForSearch(sNominativoA) & "%')"
    '        End If
    '        dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '        nListFatture = -1
    '        Do While dvMyDati.Read
    '            ObjFattura = New ObjAnagDocumenti
    '            ObjFattura.nIdDocumento = StringOperation.Formatint(myrow("idfatturanota"))
    '            ObjFattura.sCognome = StringOperation.Formatstring(myrow("cognome_denominazione"))
    '            ObjFattura.sNome = StringOperation.Formatstring(myrow("nome"))
    '            If StringOperation.Formatstring(myrow("COD_FISCALE")) <> "" Then
    '                ObjFattura.sCodFiscalePIva = StringOperation.Formatstring(myrow("COD_FISCALE"))
    '            ElseIf StringOperation.Formatstring(myrow("PARTITA_IVA")) <> "" Then
    '                ObjFattura.sCodFiscalePIva = StringOperation.Formatstring(myrow("PARTITA_IVA"))
    '            End If
    '            ObjFattura.sMatricola = StringOperation.Formatstring(myrow("matricola"))
    '            ObjFattura.sTipoDocumento = StringOperation.Formatstring(myrow("tipo_documento"))
    '            If Not IsDBNull(myrow("data_fattura")) Then
    '                ObjFattura.tDataDocumento = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_fattura")))
    '            End If
    '            ObjFattura.sNDocumento = StringOperation.Formatstring(myrow("numero_fattura"))
    '            ObjFattura.impDocumento = StringOperation.Formatdouble(myrow("importo_fatturanota"))
    '            'ridimensiono l'array
    '            nListFatture += 1
    '            ReDim Preserve ObjFatturaReturn(nListFatture)
    '            ObjFatturaReturn(nListFatture) = ObjFattura
    '        Loop

    '        Return ObjFatturaReturn
    '    Catch ex As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsRuoloH2O.GetCartelleContribDaElaborare.errore: ", ex)
    '    Finally
    '        'chiudo la connessione
    '        If Not WFSessione.oSession Is Nothing Then
    '            WFSessione.Kill()
    '        End If
    '    End Try
    'End Function

    'Public Function DeleteRuoloH2O(ByVal IdDelRuolo As Integer) As Integer
    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Dim sSQL, WFErrore As String

    '    Try
    '        'inizializzo la connessione
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If
    '        'costruisco la query
    '        sSQL = "DELETE "
    '        sSQL += " FROM TP_FATTURAZIONI_GENERATE "
    '        sSQL += " WHERE (ID_FLUSSO=" & IdDelRuolo & ")"
    '        'eseguo la query
    '        If WFSessione.oSession.oAppDB.Execute(sSQL) <> 1 Then
    '            Return 0
    '        End If

    '        sSQL = "UPDATE TP_LETTURE SET FATTURAZIONE=0"
    '        sSQL += " WHERE CODLETTURA IN ("
    '        sSQL += " 	SELECT IDLETTURA"
    '        sSQL += " 	FROM TP_FATTURE_NOTE"
    '        sSQL += " 	WHERE (IDFLUSSO=" & IdDelRuolo & ")"
    '        sSQL += " )"
    '        'eseguo la query
    '        If WFSessione.oSession.oAppDB.Execute(sSQL) < 0 Then
    '            Return 0
    '        End If

    '        sSQL = "DELETE"
    '        sSQL += " FROM TP_FATTURE_RATE"
    '        sSQL += " WHERE IDFATTURANOTA IN ("
    '        sSQL += " 	SELECT IDFATTURANOTA"
    '        sSQL += " 	FROM TP_FATTURE_NOTE"
    '        sSQL += " 	WHERE (IDFLUSSO=" & IdDelRuolo & ")"
    '        sSQL += " )"
    '        'eseguo la query
    '        If WFSessione.oSession.oAppDB.Execute(sSQL) < 0 Then
    '            Return 0
    '        End If

    '        sSQL = "DELETE"
    '        sSQL += " FROM TR_LETTURE_FATTURE"
    '        sSQL += " WHERE IDFATTURA IN ("
    '        sSQL += " 	SELECT IDFATTURANOTA"
    '        sSQL += " 	FROM TP_FATTURE_NOTE"
    '        sSQL += " 	WHERE (IDFLUSSO=" & IdDelRuolo & ")"
    '        sSQL += " )"
    '        'eseguo la query
    '        If WFSessione.oSession.oAppDB.Execute(sSQL) < 0 Then
    '            Return 0
    '        End If

    '        sSQL = "DELETE"
    '        sSQL += " FROM TP_FATTURE_NOTE"
    '        sSQL += " WHERE (IDFLUSSO=" & IdDelRuolo & ")"
    '        'eseguo la query
    '        If WFSessione.oSession.oAppDB.Execute(sSQL) < 0 Then
    '            Return 0
    '        End If

    '        Return 1
    '    Catch Err As Exception
    '        Log.Debug("Si è verificato un errore in ClsRuoloH2O::DeleteRuoloH2O::" & Err.Message & " SQL: " & sSQL)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsRuoloH2O.DeleteRuoloH2O.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sEnte"></param>
    ''' <param name="nPeriodo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetRuoloH2OAtt(ByVal sEnte As String, ByVal nPeriodo As Integer) As ObjTotRuoloFatture
        'preleva solo il ruolo in corso
        Dim oMyRuoloH2O As New ObjTotRuoloFatture
        Dim sSQL As String = ""
        Dim dvMyDati As DataView = Nothing

        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetRuoloAtt", "IDENTE", "IDPERIODO")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", sEnte) _
                        , ctx.GetParam("IDPERIODO", nPeriodo)
                    )
                ctx.Dispose()
            End Using
            For Each myRow As DataRowView In dvMyDati
                oMyRuoloH2O = New ObjTotRuoloFatture
                oMyRuoloH2O.sIdEnte = StringOperation.FormatString(myRow("idente"))
                oMyRuoloH2O.IdFlusso = StringOperation.FormatInt(myRow("id_flusso"))
                oMyRuoloH2O.nIdPeriodo = StringOperation.FormatInt(myRow("idperiodo"))
                oMyRuoloH2O.nNContribuenti = StringOperation.FormatInt(myRow("numero_contribuenti"))
                oMyRuoloH2O.nNDocumenti = StringOperation.FormatInt(myRow("numero_documenti"))
                oMyRuoloH2O.nFirstNDoc = StringOperation.FormatInt(myRow("firstndoc"))
                oMyRuoloH2O.nLastNDoc = StringOperation.FormatInt(myRow("lastndoc"))
                oMyRuoloH2O.impPositivi = StringOperation.FormatDouble(myRow("totale_importi_positivi"))
                oMyRuoloH2O.impNegativi = StringOperation.FormatDouble(myRow("totale_importi_negativi"))
                Log.Debug("date:" + StringOperation.FormatString(myRow("data_calcoli")) + "-" + StringOperation.FormatString(myRow("data_stampa_minuta")) + "-" + StringOperation.FormatString(myRow("data_approvazione_minuta")) + "-" + StringOperation.FormatString(myRow("data_emissione_fattura")) + "-" + StringOperation.FormatString(myRow("data_scadenza")) + "-" + StringOperation.FormatString(myRow("data_numerazione")) + "-" + StringOperation.FormatString(myRow("data_approvazione_documenti")))
                oMyRuoloH2O.tDataCalcoli = StringOperation.FormatDateTime(myRow("data_calcoli"))
                Log.Debug("tDataCalcoli")
                oMyRuoloH2O.tDataStampaMinuta = StringOperation.FormatDateTime(myRow("data_stampa_minuta"))
                Log.Debug("tDataStampaMinuta")
                oMyRuoloH2O.tDataOkMinuta = StringOperation.FormatDateTime(myRow("data_approvazione_minuta"))
                Log.Debug("tDataOkMinuta")
                oMyRuoloH2O.tDataEmissioneFattura = StringOperation.FormatDateTime(myRow("data_emissione_fattura"))
                Log.Debug("tDataEmissioneFattura")
                oMyRuoloH2O.tDataScadenza = StringOperation.FormatDateTime(myRow("data_scadenza"))
                Log.Debug("tDataScadenza")
                oMyRuoloH2O.tDataNumerazione = StringOperation.FormatDateTime(myRow("data_numerazione"))
                Log.Debug("tDataNumerazione")
                oMyRuoloH2O.tDataApprovazioneDOC = StringOperation.FormatDateTime(myRow("data_approvazione_documenti"))
                Log.Debug("tDataApprovazioneDOC")
                oMyRuoloH2O.sNote = StringOperation.FormatString(myRow("note"))
                oMyRuoloH2O.sOperatore = StringOperation.FormatString(myRow("operatore"))
            Next
            Return oMyRuoloH2O
        Catch Err As Exception
            Log.Debug(sEnte + " - OPENgovH2O.ClsRuoloH2O.GetRuoloH2OAtt.errore: ", Err)
            Return Nothing
        Finally
            dvMyDati.Dispose()
        End Try
    End Function
    'Public Function GetRuoloH2OAtt(ByVal sEnte As String, ByVal nPeriodo As Integer) As ObjTotRuoloFatture
    '    'preleva solo il ruolo in corso
    '    Dim sSQL As String=""
    '    Dim dvMyDati As new dataview=nothing
    '    Dim oMyRuoloH2O As New ObjTotRuoloFatture

    '    Try
    '        sSQL = "SELECT TP_FATTURAZIONI_GENERATE.ID_FLUSSO, TP_FATTURAZIONI_GENERATE.IDPERIODO, TP_FATTURAZIONI_GENERATE.IDENTE,"
    '        sSQL += " TP_FATTURAZIONI_GENERATE.NUMERO_CONTRIBUENTI, TP_FATTURAZIONI_GENERATE.NUMERO_DOCUMENTI, TP_FATTURAZIONI_GENERATE.TOTALE_IMPORTI_POSITIVI, TP_FATTURAZIONI_GENERATE.TOTALE_IMPORTI_NEGATIVI,"
    '        sSQL += " TP_FATTURAZIONI_GENERATE.DATA_CALCOLI, TP_FATTURAZIONI_GENERATE.DATA_STAMPA_MINUTA, TP_FATTURAZIONI_GENERATE.DATA_APPROVAZIONE_MINUTA, TP_FATTURAZIONI_GENERATE.DATA_NUMERAZIONE, TP_FATTURAZIONI_GENERATE.DATA_EMISSIONE_FATTURA,"
    '        sSQL += " TP_FATTURAZIONI_GENERATE.DATA_APPROVAZIONE_DOCUMENTI, TP_FATTURAZIONI_GENERATE.DATA_SCADENZA,"
    '        sSQL += " TP_FATTURAZIONI_GENERATE.NOTE, TP_FATTURAZIONI_GENERATE.OPERATORE, TP_FATTURAZIONI_GENERATE.DATA_ESTRAZIONE_REGISTRO,"
    '        sSQL += " MIN(CAST(TP_FATTURE_NOTE.NUMERO_DOCUMENTO AS NUMERIC)) AS FIRSTNDOC, MAX(CAST(TP_FATTURE_NOTE.NUMERO_DOCUMENTO AS NUMERIC)) AS LASTNDOC"
    '        sSQL += " FROM TP_FATTURAZIONI_GENERATE"
    '        sSQL += " INNER JOIN TP_FATTURE_NOTE ON TP_FATTURAZIONI_GENERATE.ID_FLUSSO=TP_FATTURE_NOTE.IDFLUSSO"
    '        sSQL += " WHERE (TP_FATTURAZIONI_GENERATE.IDENTE='" & sEnte & "') AND (TP_FATTURAZIONI_GENERATE.IDPERIODO=" & nPeriodo & ") AND (TP_FATTURAZIONI_GENERATE.DATA_APPROVAZIONE_DOCUMENTI IS NULL)"
    '        sSQL += " GROUP BY TP_FATTURAZIONI_GENERATE.ID_FLUSSO, TP_FATTURAZIONI_GENERATE.IDPERIODO, TP_FATTURAZIONI_GENERATE.IDENTE,"
    '        sSQL += " TP_FATTURAZIONI_GENERATE.NUMERO_CONTRIBUENTI, TP_FATTURAZIONI_GENERATE.NUMERO_DOCUMENTI, TP_FATTURAZIONI_GENERATE.TOTALE_IMPORTI_POSITIVI, TP_FATTURAZIONI_GENERATE.TOTALE_IMPORTI_NEGATIVI,"
    '        sSQL += " TP_FATTURAZIONI_GENERATE.DATA_CALCOLI, TP_FATTURAZIONI_GENERATE.DATA_STAMPA_MINUTA, TP_FATTURAZIONI_GENERATE.DATA_APPROVAZIONE_MINUTA, TP_FATTURAZIONI_GENERATE.DATA_NUMERAZIONE, TP_FATTURAZIONI_GENERATE.DATA_EMISSIONE_FATTURA,"
    '        sSQL += " TP_FATTURAZIONI_GENERATE.DATA_APPROVAZIONE_DOCUMENTI, TP_FATTURAZIONI_GENERATE.DATA_SCADENZA,"
    '        sSQL += " TP_FATTURAZIONI_GENERATE.NOTE, TP_FATTURAZIONI_GENERATE.OPERATORE, TP_FATTURAZIONI_GENERATE.DATA_ESTRAZIONE_REGISTRO"
    '        'eseguo la query
    '        dvMyDati = iDB.getdataview(sSQL)
    '        Do While dvMyDati.Read
    '            oMyRuoloH2O.sIdEnte = StringOperation.FormatString(myrow("idente"))
    '            oMyRuoloH2O.IdFlusso = StringOperation.FormatInt(myrow("id_flusso"))
    '            oMyRuoloH2O.nIdPeriodo = StringOperation.FormatInt(myrow("idperiodo"))
    '            oMyRuoloH2O.nNContribuenti = StringOperation.FormatInt(myrow("numero_contribuenti"))
    '            oMyRuoloH2O.nNDocumenti = StringOperation.FormatInt(myrow("numero_documenti"))
    '            oMyRuoloH2O.nFirstNDoc = StringOperation.FormatInt(myrow("firstndoc"))
    '            oMyRuoloH2O.nLastNDoc = StringOperation.FormatInt(myrow("lastndoc"))
    '            oMyRuoloH2O.impPositivi = StringOperation.FormatDouble(myrow("totale_importi_positivi"))
    '            oMyRuoloH2O.impNegativi = StringOperation.FormatDouble(myrow("totale_importi_negativi"))
    '            If StringOperation.FormatString(myrow("data_calcoli")) <> "" Then
    '                oMyRuoloH2O.tDataCalcoli = oReplace.GiraDataFromDB(StringOperation.FormatString(myrow("data_calcoli")))
    '            End If
    '            If StringOperation.FormatString(myrow("data_stampa_minuta")) <> "" Then
    '                oMyRuoloH2O.tDataStampaMinuta = oReplace.GiraDataFromDB(StringOperation.FormatString(myrow("data_stampa_minuta")))
    '            End If
    '            If StringOperation.FormatString(myrow("data_approvazione_minuta")) <> "" Then
    '                oMyRuoloH2O.tDataOkMinuta = oReplace.GiraDataFromDB(StringOperation.FormatString(myrow("data_approvazione_minuta")))
    '            End If
    '            If StringOperation.FormatString(myrow("data_emissione_fattura")) <> "" Then
    '                oMyRuoloH2O.tDataEmissioneFattura = oReplace.GiraDataFromDB(StringOperation.FormatString(myrow("data_emissione_fattura")))
    '            End If
    '            If StringOperation.FormatString(myrow("data_scadenza")) <> "" Then
    '                oMyRuoloH2O.tDataScadenza = oReplace.GiraDataFromDB(StringOperation.FormatString(myrow("data_scadenza")))
    '            End If
    '            If StringOperation.FormatString(myrow("data_numerazione")) <> "" Then
    '                oMyRuoloH2O.tDataNumerazione = oReplace.GiraDataFromDB(StringOperation.FormatString(myrow("data_numerazione")))
    '            End If
    '            If StringOperation.FormatString(myrow("data_approvazione_documenti")) <> "" Then
    '                oMyRuoloH2O.tDataApprovazioneDOC = oReplace.GiraDataFromDB(StringOperation.FormatString(myrow("data_approvazione_documenti")))
    '            End If
    '            oMyRuoloH2O.sNote = StringOperation.FormatString(myrow("note"))
    '            oMyRuoloH2O.sOperatore = StringOperation.FormatString(myrow("operatore"))
    '        Loop

    '        Return oMyRuoloH2O
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsRuoloH2O.GetRuoloH2OAtt.errore: " & sSQL, Err)
    '        Return Nothing
    '    Finally
    '        dvmydati.dispose()
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sEnte"></param>
    ''' <param name="nPeriodo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetRuoloH2OPrec(ByVal sEnte As String, ByVal nPeriodo As Integer) As ObjTotRuoloFatture()
        'preleva i ruoli precedenti
        Dim sSQL As String = ""
        Dim dvMyDati As DataView = Nothing
        Dim oMyListRuoli() As ObjTotRuoloFatture
        Dim nList As Integer = -1
        Dim oMyRuoloH2O As New ObjTotRuoloFatture

        oMyListRuoli = Nothing
        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetRuoloPrec", "IDENTE", "IDPERIODO")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", sEnte) _
                        , ctx.GetParam("IDPERIODO", nPeriodo)
                    )
                ctx.Dispose()
            End Using
            For Each myRow As DataRowView In dvMyDati
                oMyRuoloH2O = New ObjTotRuoloFatture
                oMyRuoloH2O.sIdEnte = StringOperation.FormatString(myRow("idente"))
                oMyRuoloH2O.IdFlusso = StringOperation.FormatInt(myRow("id_flusso"))
                oMyRuoloH2O.nIdPeriodo = StringOperation.FormatInt(myRow("idperiodo"))
                oMyRuoloH2O.nNContribuenti = StringOperation.FormatInt(myRow("numero_contribuenti"))
                oMyRuoloH2O.nNDocumenti = StringOperation.FormatInt(myRow("numero_documenti"))
                oMyRuoloH2O.impPositivi = StringOperation.FormatDouble(myRow("totale_importi_positivi"))
                oMyRuoloH2O.impNegativi = StringOperation.FormatDouble(myRow("totale_importi_negativi"))
                If StringOperation.FormatString(myRow("data_calcoli")) <> "" Then
                    oMyRuoloH2O.tDataCalcoli = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_calcoli")))
                End If
                If StringOperation.FormatString(myRow("data_stampa_minuta")) <> "" Then
                    oMyRuoloH2O.tDataStampaMinuta = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_stampa_minuta")))
                End If
                If StringOperation.FormatString(myRow("data_approvazione_minuta")) <> "" Then
                    oMyRuoloH2O.tDataOkMinuta = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_approvazione_minuta")))
                End If
                If StringOperation.FormatString(myRow("data_emissione_fattura")) <> "" Then
                    oMyRuoloH2O.tDataEmissioneFattura = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_emissione_fattura")))
                End If
                If StringOperation.FormatString(myRow("data_scadenza")) <> "" Then
                    oMyRuoloH2O.tDataScadenza = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_scadenza")))
                End If
                If StringOperation.FormatString(myRow("data_numerazione")) <> "" Then
                    oMyRuoloH2O.tDataNumerazione = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_numerazione")))
                End If
                If StringOperation.FormatString(myRow("data_approvazione_documenti")) <> "" Then
                    oMyRuoloH2O.tDataApprovazioneDOC = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_approvazione_documenti")))
                End If
                oMyRuoloH2O.nFirstNDoc = StringOperation.FormatInt(myRow("firstfattura"))
                oMyRuoloH2O.nLastNDoc = StringOperation.FormatInt(myRow("lastfattura"))
                oMyRuoloH2O.sNote = StringOperation.FormatString(myRow("note"))
                oMyRuoloH2O.sOperatore = StringOperation.FormatString(myRow("operatore"))
                'incremento l'array
                nList += 1
                ReDim Preserve oMyListRuoli(nList)
                oMyListRuoli(nList) = oMyRuoloH2O
            Next

            Return oMyListRuoli
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsRuoloH2O.GetRuoloH2OPrec.errore: ", Err)
            Return Nothing
        Finally
            dvMyDati.Dispose()
        End Try
    End Function
    'Public Function GetRuoloH2OPrec(ByVal sEnte As String, ByVal nPeriodo As Integer) As ObjTotRuoloFatture()
    '    'preleva i ruoli precedenti
    '    Dim sSQL As String=""
    '    Dim dvMyDati As new dataview=nothing
    '    Dim oMyListRuoli() As ObjTotRuoloFatture
    '    Dim nList As Integer = -1
    '    Dim oMyRuoloH2O As ObjTotRuoloFatture

    '    Try
    '        sSQL = "SELECT *"
    '        sSQL += " FROM TP_FATTURAZIONI_GENERATE R"
    '        sSQL += " LEFT JOIN ("
    '        sSQL += " 	SELECT IDENTE, IDPERIODO, IDFLUSSO, MIN(NUMERO_DOCUMENTO) AS FIRSTFATTURA, MAX(NUMERO_DOCUMENTO) AS LASTFATTURA"
    '        sSQL += " 	FROM TP_FATTURE_NOTE"
    '        sSQL += " 	GROUP BY IDENTE, IDPERIODO, IDFLUSSO"
    '        sSQL += " ) F ON R.IDENTE=F.IDENTE AND R.IDPERIODO=F.IDPERIODO AND R.ID_FLUSSO=F.IDFLUSSO "
    '        sSQL += " WHERE 1=1"
    '        sSQL += " AND (R.IDENTE='" & sEnte & "') AND (R.IDPERIODO=" & nPeriodo & ") AND (NOT R.DATA_APPROVAZIONE_DOCUMENTI IS NULL)"
    '        sSQL += " ORDER BY R.ID_FLUSSO"
    '        'eseguo la query
    '        dvMyDati = iDB.getdataview(sSQL)
    '        Do While dvMyDati.Read
    '            oMyRuoloH2O = New ObjTotRuoloFatture
    '            oMyRuoloH2O.sIdEnte = StringOperation.FormatString(myrow("idente"))
    '            oMyRuoloH2O.IdFlusso = StringOperation.FormatInt(myrow("id_flusso"))
    '            oMyRuoloH2O.nIdPeriodo = StringOperation.FormatInt(myrow("idperiodo"))
    '            oMyRuoloH2O.nNContribuenti = StringOperation.FormatInt(myrow("numero_contribuenti"))
    '            oMyRuoloH2O.nNDocumenti = StringOperation.FormatInt(myrow("numero_documenti"))
    '            oMyRuoloH2O.impPositivi = StringOperation.FormatDouble(myrow("totale_importi_positivi"))
    '            oMyRuoloH2O.impNegativi = StringOperation.FormatDouble(myrow("totale_importi_negativi"))
    '            If Not IsDBNull(myrow("data_calcoli")) Then
    '                If StringOperation.FormatString(myrow("data_calcoli")) <> "" Then
    '                    oMyRuoloH2O.tDataCalcoli = oReplace.GiraDataFromDB(StringOperation.FormatString(myrow("data_calcoli")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("data_stampa_minuta")) Then
    '                If StringOperation.FormatString(myrow("data_stampa_minuta")) <> "" Then
    '                    oMyRuoloH2O.tDataStampaMinuta = oReplace.GiraDataFromDB(StringOperation.FormatString(myrow("data_stampa_minuta")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("data_approvazione_minuta")) Then
    '                If StringOperation.FormatString(myrow("data_approvazione_minuta")) <> "" Then
    '                    oMyRuoloH2O.tDataOkMinuta = oReplace.GiraDataFromDB(StringOperation.FormatString(myrow("data_approvazione_minuta")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("data_emissione_fattura")) Then
    '                If StringOperation.FormatString(myrow("data_emissione_fattura")) <> "" Then
    '                    oMyRuoloH2O.tDataEmissioneFattura = oReplace.GiraDataFromDB(StringOperation.FormatString(myrow("data_emissione_fattura")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("data_scadenza")) Then
    '                If StringOperation.FormatString(myrow("data_scadenza")) <> "" Then
    '                    oMyRuoloH2O.tDataScadenza = oReplace.GiraDataFromDB(StringOperation.FormatString(myrow("data_scadenza")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("data_numerazione")) Then
    '                If StringOperation.FormatString(myrow("data_numerazione")) <> "" Then
    '                    oMyRuoloH2O.tDataNumerazione = oReplace.GiraDataFromDB(StringOperation.FormatString(myrow("data_numerazione")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("data_approvazione_documenti")) Then
    '                If StringOperation.FormatString(myrow("data_approvazione_documenti")) <> "" Then
    '                    oMyRuoloH2O.tDataApprovazioneDOC = oReplace.GiraDataFromDB(StringOperation.FormatString(myrow("data_approvazione_documenti")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("firstfattura")) Then
    '                oMyRuoloH2O.nFirstNDoc = StringOperation.FormatInt(myrow("firstfattura"))
    '            End If
    '            If Not IsDBNull(myrow("lastfattura")) Then
    '                oMyRuoloH2O.nLastNDoc = StringOperation.FormatInt(myrow("lastfattura"))
    '            End If
    '            oMyRuoloH2O.sNote = StringOperation.FormatString(myrow("note"))
    '            oMyRuoloH2O.sOperatore = StringOperation.FormatString(myrow("operatore"))
    '            'incremento l'array
    '            nList += 1
    '            ReDim Preserve oMyListRuoli(nList)
    '            oMyListRuoli(nList) = oMyRuoloH2O
    '        Loop

    '        Return oMyListRuoli
    '    Catch Err As Exception
    '        Log.Debug("Si è verificato un errore in ClsRuoloH2O::GetRuoloH2OPrec::" & Err.Message & " SQL: " & sSQL)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsRuoloH2O.GetRuoloH2OPrec.errore: ", Err)

    '        Return Nothing
    '    Finally
    '        dvmydati.dispose()
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oTotRuoloH2O"></param>
    ''' <param name="nDbOperation"></param>
    ''' <param name="nFlusso"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetRuoloH2O(ByVal oTotRuoloH2O As ObjTotRuoloFatture, ByVal nDbOperation As Integer, Optional ByVal nFlusso As Integer = -1) As Integer
        Dim sSQL As String = ""
        Try
            Dim myIdentity As Integer

            'costruisco la query
            Select Case nDbOperation
                Case 0
                    sSQL = "INSERT INTO TP_FATTURAZIONI_GENERATE (IDPERIODO, IDENTE, DATA_CALCOLI,"
                    sSQL += " NUMERO_CONTRIBUENTI, NUMERO_DOCUMENTI, TOTALE_IMPORTI_POSITIVI, TOTALE_IMPORTI_NEGATIVI,"
                    sSQL += " DATA_STAMPA_MINUTA, DATA_APPROVAZIONE_MINUTA, DATA_NUMERAZIONE,"
                    sSQL += " DATA_EMISSIONE_FATTURA, DATA_SCADENZA, NOTE, OPERATORE)"
                    sSQL += "VALUES (" & oTotRuoloH2O.nIdPeriodo & ",'" & oTotRuoloH2O.sIdEnte & "','" & oReplace.GiraData(oTotRuoloH2O.tDataCalcoli.ToString) & "',"
                    sSQL += oTotRuoloH2O.nNContribuenti & "," & oTotRuoloH2O.nNDocumenti & "," & oReplace.ReplaceNumberForDB(oTotRuoloH2O.impPositivi) & "," & oReplace.ReplaceNumberForDB(oTotRuoloH2O.impNegativi) & ","
                    If oTotRuoloH2O.tDataStampaMinuta.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                        sSQL += "'" & oReplace.GiraData(oTotRuoloH2O.tDataStampaMinuta.ToString) & "',"
                    Else
                        sSQL += "NULL,"
                    End If
                    If oTotRuoloH2O.tDataOkMinuta.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                        sSQL += "'" & oReplace.GiraData(oTotRuoloH2O.tDataOkMinuta.ToString) & "',"
                    Else
                        sSQL += "NULL,"
                    End If
                    If oTotRuoloH2O.tDataNumerazione.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                        sSQL += "'" & oReplace.GiraData(oTotRuoloH2O.tDataNumerazione.ToString) & "',"
                    Else
                        sSQL += "NULL,"
                    End If
                    If oTotRuoloH2O.tDataEmissioneFattura.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                        sSQL += "'" & oReplace.GiraData(oTotRuoloH2O.tDataEmissioneFattura.ToString) & "',"
                    Else
                        sSQL += "NULL,"
                    End If
                    If oTotRuoloH2O.tDataScadenza.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                        sSQL += "'" & oReplace.GiraData(oTotRuoloH2O.tDataScadenza.ToString) & "',"
                    Else
                        sSQL += "NULL,"
                    End If
                    sSQL += "'" & oReplace.ReplaceChar(oTotRuoloH2O.sNote) & "','" & oTotRuoloH2O.sOperatore & "')"
                    sSQL += " SELECT @@IDENTITY"
                    'eseguo la query
                    Dim dvMyDati As New DataView
                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            myIdentity = myRow(0)
                        Next
                    End If
                    dvMyDati.Dispose()
                Case 1
                    sSQL = "UPDATE TP_FATTURAZIONI_GENERATE SET IDPERIODO=" & oTotRuoloH2O.nIdPeriodo & ","
                    sSQL += "IDENTE='" & oTotRuoloH2O.sIdEnte & "',"
                    sSQL += "DATA_CALCOLI='" & oReplace.GiraData(oTotRuoloH2O.tDataCalcoli.ToString) & "',"
                    sSQL += "NUMERO_CONTRIBUENTI=" & oTotRuoloH2O.nNContribuenti & ","
                    sSQL += "NUMERO_DOCUMENTI=" & oTotRuoloH2O.nNDocumenti & ","
                    sSQL += "TOTALE_IMPORTI_POSITIVI=" & oReplace.ReplaceNumberForDB(oTotRuoloH2O.impPositivi) & ","
                    sSQL += "TOTALE_IMPORTI_NEGATIVI=" & oReplace.ReplaceNumberForDB(oTotRuoloH2O.impNegativi) & ","
                    If oTotRuoloH2O.tDataStampaMinuta.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                        sSQL += "DATA_STAMPA_MINUTA='" & oReplace.GiraData(oTotRuoloH2O.tDataStampaMinuta.ToString) & "',"
                    End If
                    If oTotRuoloH2O.tDataOkMinuta.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                        sSQL += "DATA_APPROVAZIONE_MINUTA='" & oReplace.GiraData(oTotRuoloH2O.tDataOkMinuta.ToString) & "',"
                    End If
                    If oTotRuoloH2O.tDataNumerazione.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                        sSQL += "DATA_NUMERAZIONE='" & oReplace.GiraData(oTotRuoloH2O.tDataNumerazione.ToString) & "',"
                    End If
                    If oTotRuoloH2O.tDataEmissioneFattura.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                        sSQL += "DATA_EMISSIONE_FATTURA='" & oReplace.GiraData(oTotRuoloH2O.tDataEmissioneFattura.ToString) & "',"
                    End If
                    If oTotRuoloH2O.tDataScadenza.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                        sSQL += "DATA_SCADENZA='" & oReplace.GiraData(oTotRuoloH2O.tDataScadenza.ToString) & "',"
                    End If
                    sSQL += "NOTE='" & oReplace.ReplaceChar(oTotRuoloH2O.sNote) & "',"
                    sSQL += "OPERATORE='" & oTotRuoloH2O.sOperatore & "'"
                    sSQL += " WHERE (ID_FLUSSO = " & oTotRuoloH2O.IdFlusso & ")"
                    'eseguo la query
                    If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                        Throw New Exception("errore in::" & sSQL)
                    End If
                    myIdentity = oTotRuoloH2O.IdFlusso
                Case 2
                    sSQL = "DELETE"
                    sSQL += " FROM TP_FATTURAZIONI_GENERATE"
                    If nFlusso <> -1 Then
                        sSQL += " WHERE (ID_FLUSSO =" & nFlusso & ")"
                        'eseguo la query
                        If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                            Throw New Exception("errore in::" & sSQL)
                        End If
                    ElseIf Not oTotRuoloH2O Is Nothing Then
                        sSQL += " WHERE (ID_FLUSSO =" & oTotRuoloH2O.IdFlusso & ")"
                        'eseguo la query
                        If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                            Throw New Exception("errore in::" & sSQL)
                        End If
                    End If
                    myIdentity = 1
            End Select

            Return myIdentity
        Catch Err As Exception
            Log.Debug("Si è verificato un errore in ClsRuoloH2O::SetRuoloH2O::" & Err.Message & " SQL: " & sSQL)
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsRuoloH2O.SetRuoloH2O.errore: ", Err)
            Return 0
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="IdSetRuolo"></param>
    ''' <param name="nDBOperation"></param>
    ''' <param name="sTypeDBOperation"></param>
    ''' <param name="tDataRuoloH2O"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetDateRuoliH2OGenerati(ByVal IdSetRuolo As Integer, ByVal nDBOperation As Integer, Optional ByVal sTypeDBOperation As String = "I", Optional ByVal tDataRuoloH2O As String = "") As Integer
        Dim sSQL As String = ""

        Try
            '***nDBOperation****************************
            '0 = imposta la data della stampa della minuta
            '1 = imposta la data di approvazione della minuta
            '2 = imposta la data di numerazione
            '3 = imposta la data di emissione fattura
            '4 = imposta la data di scadenza
            '5 = imposta la data di approvazione documenti
            '******************************************
            '***sTypeDBOperation************************
            'I = inserimento data
            'C = cancellazione data
            '******************************************
            'costruisco la query
            Select Case nDBOperation
                Case 0
                    sSQL = "UPDATE TP_FATTURAZIONI_GENERATE SET DATA_STAMPA_MINUTA="
                    If sTypeDBOperation = "C" Then
                        sSQL += "NULL"
                    Else
                        sSQL += "'" & oReplace.GiraData(Now) & "'"
                    End If
                    sSQL += " WHERE (ID_FLUSSO=" & IdSetRuolo & ")"
                Case 1
                    sSQL = "UPDATE TP_FATTURAZIONI_GENERATE SET DATA_APPROVAZIONE_MINUTA="
                    If sTypeDBOperation = "C" Then
                        sSQL += "NULL"
                    Else
                        sSQL += "'" & oReplace.GiraData(Now) & "'"
                    End If
                    sSQL += " WHERE (ID_FLUSSO=" & IdSetRuolo & ")"
                Case 2
                    sSQL = "UPDATE TP_FATTURAZIONI_GENERATE SET DATA_NUMERAZIONE="
                    If sTypeDBOperation = "C" Then
                        sSQL += "NULL"
                    Else
                        sSQL += "'" & oReplace.GiraData(Now) & "'"
                    End If
                    sSQL += " WHERE (ID_FLUSSO=" & IdSetRuolo & ")"
                Case 3
                    sSQL = "UPDATE TP_FATTURAZIONI_GENERATE SET DATA_EMISSIONE_FATTURA="
                    If sTypeDBOperation = "C" Then
                        sSQL += "NULL"
                    Else
                        sSQL += "'" & oReplace.GiraData(tDataRuoloH2O) & "'"
                    End If
                    sSQL += " WHERE (ID_FLUSSO=" & IdSetRuolo & ")"
                Case 4
                    sSQL = "UPDATE TP_FATTURAZIONI_GENERATE SET DATA_SCADENZA="
                    If sTypeDBOperation = "C" Then
                        sSQL += "NULL"
                    Else
                        sSQL += "'" & oReplace.GiraData(tDataRuoloH2O) & "'"
                    End If
                    sSQL += " WHERE (ID_FLUSSO=" & IdSetRuolo & ")"
                Case 5
                    sSQL = "UPDATE TP_FATTURAZIONI_GENERATE SET DATA_APPROVAZIONE_DOCUMENTI="
                    If sTypeDBOperation = "C" Then
                        sSQL += "NULL"
                    Else
                        sSQL += "'" & oReplace.GiraData(Now) & "'"
                    End If
                    sSQL += " WHERE (ID_FLUSSO=" & IdSetRuolo & ")"
            End Select
            'eseguo la query
            If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                Throw New Exception("errore in::" & sSQL)
            End If

            Return IdSetRuolo
        Catch Err As Exception
            Log.Debug("Si è verificato un errore in ClsRuoloH2O::SetDateRuoliH2OGenerati::" & Err.Message & " SQL: " & sSQL)

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsRuoloH2O.SetDateRuoliH2OGenerati.errore: ", Err)
            Return 0
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sEnte"></param>
    ''' <param name="nPeriodo"></param>
    ''' <param name="sTributo"></param>
    ''' <param name="sAnno"></param>
    ''' <param name="nContatoriNoLettura"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Esportazione completa dati</em>
    ''' </revision>
    ''' </revisionHistory>
    Public Function GetDatiDaFatturare(ByVal sEnte As String, ByVal nPeriodo As Integer, ByVal sTributo As String, ByVal sAnno As String, ByRef nContatoriNoLettura As Integer) As ObjTotRuoloFatture
        'preleva le letture da fatturare
        Dim FncLetture As New ClsLettureFattura
        Dim oRuoloH2O As New ObjTotRuoloFatture
        Dim x, nContribPrec As Integer
        Dim FncFatture As New ClsFatture
        Dim oListFatture() As ObjFattura

        Try
            'prelevo i contatori senza lettura
            nContatoriNoLettura = FncLetture.GetContatoriSenzaLettura(sEnte, nPeriodo)
            'prelevo le letture da fatturare
            oRuoloH2O = FncLetture.GetTotalizzatoriLetture(sEnte, nPeriodo)
            If oRuoloH2O Is Nothing Then
                Return Nothing
            End If
            'prelevo le fatture da variazioni da fatturare
            oListFatture = FncFatture.GetFattura(ConstSession.StringConnection, sEnte, -1, -1, True)
            If Not oListFatture Is Nothing Then
                For x = 0 To oListFatture.GetUpperBound(0)
                    If oListFatture(x).nIdIntestatario <> nContribPrec Then
                        oRuoloH2O.nNContribuenti += 1
                    End If
                    oRuoloH2O.nNDocumenti += 1
                    nContribPrec = oListFatture(x).nIdIntestatario
                Next
            End If

            Return oRuoloH2O
        Catch Err As Exception
            Log.Debug(sEnte + " - OPENgovH2O.ClsRuoloH2O.GetDatiDaFatturare.errore: ", Err)
            Return Nothing
        End Try
    End Function
    'Public Function GetDatiDaFatturare(ByVal sEnte As String, ByVal nPeriodo As Integer, ByVal sTributo As String, ByVal sAnno As String, ByRef nContatoriNoLettura As Integer) As ObjTotRuoloFatture
    '    'preleva le letture da fatturare
    '    Dim FncLetture As New ClsLettureFattura
    '    Dim oRuoloH2O As New ObjTotRuoloFatture
    '    Dim x, nContribPrec As Integer
    '    Dim FncFatture As New ClsFatture
    '    Dim oListFatture() As ObjFattura

    '    Try
    '        'prelevo i contatori senza lettura
    '        nContatoriNoLettura = FncLetture.GetContatoriSenzaLettura(sEnte)
    '        'prelevo le letture da fatturare
    '        oRuoloH2O = FncLetture.GetTotalizzatoriLetture(sEnte, nPeriodo)
    '        If oRuoloH2O Is Nothing Then
    '            Return Nothing
    '        End If
    '        'prelevo le fatture da variazioni da fatturare
    '        oListFatture = FncFatture.GetFattura(sEnte, -1, -1, True)
    '        If Not oListFatture Is Nothing Then
    '            For x = 0 To oListFatture.GetUpperBound(0)
    '                If oListFatture(x).nIdIntestatario <> nContribPrec Then
    '                    oRuoloH2O.nNContribuenti += 1
    '                End If
    '                oRuoloH2O.nNDocumenti += 1
    '                nContribPrec = oListFatture(x).nIdIntestatario
    '            Next
    '        End If

    '        Return oRuoloH2O
    '    Catch Err As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsRuoloH2O.GetDatiDaFatturare.errore: ", Err)
    '        Return Nothing
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="idFlussoRuolo"></param>
    ''' <param name="idContribuente"></param>
    ''' <param name="NumeroFattura"></param>
    ''' <param name="DataFattura"></param>
    ''' <param name="sNominativoDa"></param>
    ''' <param name="sNominativoA"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCartelleContribDaElaborare(ByVal idFlussoRuolo As Integer, ByVal idContribuente As Integer, ByVal NumeroFattura As String, ByVal DataFattura As String, ByVal sNominativoDa As String, ByVal sNominativoA As String) As ObjAnagDocumenti()
        Dim oReplace As New ClsGenerale.Generale
        Dim oDaElab As New ObjAnagDocumenti
        Dim ListDaElab As New Generic.List(Of ObjAnagDocumenti)

        Try
            Dim sSQL As String = ""
            Dim dvMyDati As New DataView
            Try
                Dim oDbManagerRepository As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                Using ctx As DBModel = oDbManagerRepository
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetFatture", "IDENTE", "IDFLUSSO", "NDOCUMENTO", "DATADOCUMENTO", "NOMINATIVODA", "NOMINATIVOA", "ISDOCTOELAB")
                    dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", ConstSession.IdEnte) _
                                , ctx.GetParam("IDFLUSSO", idFlussoRuolo) _
                                , ctx.GetParam("NDOCUMENTO", NumeroFattura) _
                                , ctx.GetParam("DATADOCUMENTO", oReplace.GiraData(DataFattura)) _
                                , ctx.GetParam("NOMINATIVODA", sNominativoDa.Replace("*", "%")) _
                                , ctx.GetParam("NOMINATIVOA", sNominativoA.Replace("*", "%")) _
                                , ctx.GetParam("ISDOCTOELAB", 1)
                            )
                    ctx.Dispose()
                End Using
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        oDaElab = New ObjAnagDocumenti
                        oDaElab.nIdDocumento = StringOperation.FormatInt(myRow("idfatturanota"))
                        oDaElab.sCognome = StringOperation.FormatString(myRow("cognome_denominazione"))
                        oDaElab.sNome = StringOperation.FormatString(myRow("nome"))
                        If StringOperation.FormatString(myRow("COD_FISCALE")) <> "" Then
                            oDaElab.sCodFiscalePIva = StringOperation.FormatString(myRow("COD_FISCALE"))
                        ElseIf StringOperation.FormatString(myRow("PARTITA_IVA")) <> "" Then
                            oDaElab.sCodFiscalePIva = StringOperation.FormatString(myRow("PARTITA_IVA"))
                        End If
                        oDaElab.sMatricola = StringOperation.FormatString(myRow("matricola"))
                        oDaElab.sTipoDocumento = StringOperation.FormatString(myRow("tipo_documento"))
                        If Not IsDBNull(myRow("data_fattura")) Then
                            oDaElab.tDataDocumento = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_fattura")))
                        End If
                        oDaElab.sNDocumento = StringOperation.FormatString(myRow("numero_fattura"))
                        oDaElab.impDocumento = StringOperation.FormatDouble(myRow("importo_fatturanota"))
                        'usato come appoggio per l'idcontribuente quando elaboro doc
                        oDaElab.sVariato = StringOperation.FormatString(myRow("idcontribuente"))
                        'ridimensiono l'array
                        ListDaElab.Add(oDaElab)
                    Next
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBPeriodo.GetPeriodo.Update.errore: ", ex)
            Finally
                dvMyDati.Dispose()
            End Try

            Return ListDaElab.ToArray
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsRuoloH2O.GetCartelleContribDaElaborare.errore: ", ex)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="IdDelRuolo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DeleteRuoloH2O(ByVal IdDelRuolo As Integer) As Integer
        Dim sSQL As String = ""

        Try
            'costruisco la query
            sSQL = "DELETE "
            sSQL += " FROM TP_FATTURAZIONI_GENERATE "
            sSQL += " WHERE (ID_FLUSSO=" & IdDelRuolo & ")"
            'eseguo la query
            iDB.ExecuteNonQuery(sSQL)

            sSQL = "UPDATE TP_LETTURE SET FATTURAZIONE=0"
            sSQL += " WHERE CODLETTURA IN ("
            sSQL += " 	SELECT IDLETTURA"
            sSQL += " 	FROM TP_FATTURE_NOTE"
            sSQL += " 	WHERE (IDFLUSSO=" & IdDelRuolo & ")"
            sSQL += " )"
            'eseguo la query
            iDB.ExecuteNonQuery(sSQL)

            sSQL = "DELETE"
            sSQL += " FROM TP_FATTURE_RATE"
            sSQL += " WHERE IDFATTURANOTA IN ("
            sSQL += " 	SELECT IDFATTURANOTA"
            sSQL += " 	FROM TP_FATTURE_NOTE"
            sSQL += " 	WHERE (IDFLUSSO=" & IdDelRuolo & ")"
            sSQL += " )"
            'eseguo la query
            iDB.ExecuteNonQuery(sSQL)

            sSQL = "DELETE"
            sSQL += " FROM TR_LETTURE_FATTURE"
            sSQL += " WHERE IDFATTURA IN ("
            sSQL += " 	SELECT IDFATTURANOTA"
            sSQL += " 	FROM TP_FATTURE_NOTE"
            sSQL += " 	WHERE (IDFLUSSO=" & IdDelRuolo & ")"
            sSQL += " )"
            'eseguo la query
            iDB.ExecuteNonQuery(sSQL)

            sSQL = "DELETE"
            sSQL += " FROM TP_FATTURE_NOTE"
            sSQL += " WHERE (IDFLUSSO=" & IdDelRuolo & ")"
            'eseguo la query
            iDB.ExecuteNonQuery(sSQL)

            Return 1
        Catch Err As Exception
            Log.Debug("Si è verificato un errore in ClsRuoloH2O::DeleteRuoloH2O::" & Err.Message & " SQL: " & sSQL)

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsRuoloH2O.DeleteRuoloH2O.errore: ", Err)
            Return 0
        End Try
    End Function
End Class

Public Class ClsTariffe
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ClsTariffe))
    Private iDB As New DBAccess.getDBobject
    Private oReplace As New ClsGenerale.Generale

    'Public Function GetFatturaCanoni(ByVal nIdFattura As Integer, ByVal WFSessione As OPENUtility.CreateSessione) As ObjTariffeCanone()
    '    Dim culture As IFormatProvider
    '    culture = New System.Globalization.CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")
    '    Dim sSQL As String=""
    '    Dim dvMyDati As new dataview=nothing
    '    Try
    '        Dim oFatturaCanone As ObjTariffeCanone
    '        Dim oListFatturaCanoni() As ObjTariffeCanone
    '        Dim nList As Integer = -1

    '        sSQL = "SELECT TP_FATTURE_NOTE_CANONI.ID, IDFATTURANOTA, TP_FATTURE_NOTE_CANONI.IDENTE, TP_FATTURE_NOTE_CANONI.ANNO, ID_CANONE,"
    '        sSQL += " DESCRIZIONE, TARIFFA, PERCENTUALE_SUL_CONSUMO, TP_FATTURE_NOTE_CANONI.ALIQUOTA, IMPORTO,"
    '        sSQL += " DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE, OPERATORE"
    '        sSQL += " FROM TP_FATTURE_NOTE_CANONI"
    '        sSQL += " INNER JOIN TP_CANONI ON TP_FATTURE_NOTE_CANONI.ID_CANONE=TP_CANONI.ID"
    '        sSQL += " INNER JOIN TP_TIPOLOGIE_CANONI ON TP_CANONI.ID_TIPO_CANONE=TP_TIPOLOGIE_CANONI.ID_TIPO_CANONE"
    '        sSQL += " WHERE (IDFATTURANOTA=" & nIdFattura & ")"
    '        'eseguo la query
    '        dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '        Do While dvMyDati.Read
    '            oFatturaCanone = New ObjTariffeCanone
    '            oFatturaCanone.Id = StringOperation.Formatint(myrow("id"))
    '            oFatturaCanone.nIdFattura = StringOperation.Formatint(myrow("idfatturanota"))
    '            oFatturaCanone.sIdEnte = StringOperation.Formatstring(myrow("idente"))
    '            oFatturaCanone.sAnno = StringOperation.Formatstring(myrow("anno"))
    '            oFatturaCanone.nIdCanone = StringOperation.Formatint(myrow("id_canone"))
    '            oFatturaCanone.sDescrizione = StringOperation.Formatstring(myrow("descrizione"))
    '            oFatturaCanone.impTariffa = StringOperation.Formatdouble(myrow("tariffa"))
    '            oFatturaCanone.nPercentSulConsumo = StringOperation.Formatdouble(myrow("percentuale_sul_consumo"))
    '            oFatturaCanone.nAliquota = StringOperation.Formatdouble(myrow("aliquota"))
    '            oFatturaCanone.impCanone = StringOperation.Formatdouble(myrow("importo"))
    '            If Not IsDBNull(myrow("data_inserimento")) Then
    '                If StringOperation.Formatstring(myrow("data_inserimento")) <> "" Then
    '                    oFatturaCanone.tDataInserimento = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_inserimento")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("data_variazione")) Then
    '                If StringOperation.Formatstring(myrow("data_variazione")) <> "" Then
    '                    oFatturaCanone.tDataVariazione = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_variazione")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("data_cessazione")) Then
    '                If StringOperation.Formatstring(myrow("data_cessazione")) <> "" Then
    '                    oFatturaCanone.tDataCessazione = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_cessazione")))
    '                End If
    '            End If
    '            oFatturaCanone.sOperatore = StringOperation.Formatstring(myrow("operatore"))
    '            'ridimensiono l'array
    '            nList += 1
    '            ReDim Preserve oListFatturaCanoni(nList)
    '            oListFatturaCanoni(nList) = oFatturaCanone
    '        Loop

    '        Return oListFatturaCanoni
    '    Catch Err As Exception
    '        Log.Debug("Si è verificato un errore in ClsFatturaScaglioni::GetFatturaCanoni::" & Err.Message & " SQL: " & sSQL)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsTariffe.GetFatturaCanoni.errore: ", Err)
    '        Return Nothing
    '    Finally
    '        dvmydati.dispose()
    '    End Try
    'End Function

    'Public Function GetFatturaScaglioni(ByVal nIdFattura As Integer, ByVal WFSessione As OPENUtility.CreateSessione) As ObjTariffeScaglione()
    '    Dim culture As IFormatProvider
    '    culture = New System.Globalization.CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")
    '    Dim sSQL As String=""
    '    Dim dvMyDati As new dataview=nothing
    '    Try
    '        Dim oFatturaScaglione As ObjTariffeScaglione
    '        Dim oListFatturaScaglioni() As ObjTariffeScaglione
    '        Dim nList As Integer = -1

    '        sSQL = "SELECT TP_FATTURE_NOTE_SCAGLIONI.ID, IDFATTURANOTA, TP_FATTURE_NOTE_SCAGLIONI.IDENTE, TP_FATTURE_NOTE_SCAGLIONI.ANNO, ID_SCAGLIONE,"
    '        sSQL += " DA, A, TARIFFA, MINIMO, TP_FATTURE_NOTE_SCAGLIONI.ALIQUOTA, QUANTITA, IMPORTO,"
    '        sSQL += " DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE, OPERATORE"
    '        sSQL += " FROM TP_FATTURE_NOTE_SCAGLIONI"
    '        sSQL += " INNER JOIN TP_SCAGLIONI ON TP_FATTURE_NOTE_SCAGLIONI.ID_SCAGLIONE=TP_SCAGLIONI.ID"
    '        sSQL += " WHERE (IDFATTURANOTA=" & nIdFattura & ")"
    '        'eseguo la query
    '        dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '        Do While dvMyDati.Read
    '            oFatturaScaglione = New ObjTariffeScaglione
    '            oFatturaScaglione.Id = StringOperation.Formatint(myrow("id"))
    '            oFatturaScaglione.nIdFattura = StringOperation.Formatint(myrow("idfatturanota"))
    '            oFatturaScaglione.sIdEnte = StringOperation.Formatstring(myrow("idente"))
    '            oFatturaScaglione.sAnno = StringOperation.Formatstring(myrow("anno"))
    '            oFatturaScaglione.nIdScaglione = StringOperation.Formatint(myrow("id_scaglione"))
    '            oFatturaScaglione.nDa = StringOperation.Formatint(myrow("da"))
    '            oFatturaScaglione.nA = StringOperation.Formatint(myrow("a"))
    '            oFatturaScaglione.nQuantita = StringOperation.Formatint(myrow("quantita"))
    '            oFatturaScaglione.impTariffa = StringOperation.Formatdouble(myrow("tariffa"))
    '            oFatturaScaglione.impMinimo = StringOperation.Formatdouble(myrow("minimo"))
    '            oFatturaScaglione.nAliquota = StringOperation.Formatdouble(myrow("aliquota"))
    '            oFatturaScaglione.impScaglione = StringOperation.Formatdouble(myrow("importo"))
    '            If Not IsDBNull(myrow("data_inserimento")) Then
    '                If StringOperation.Formatstring(myrow("data_inserimento")) <> "" Then
    '                    oFatturaScaglione.tDataInserimento = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_inserimento")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("data_variazione")) Then
    '                If StringOperation.Formatstring(myrow("data_variazione")) <> "" Then
    '                    oFatturaScaglione.tDataVariazione = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_variazione")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("data_cessazione")) Then
    '                If StringOperation.Formatstring(myrow("data_cessazione")) <> "" Then
    '                    oFatturaScaglione.tDataCessazione = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_cessazione")))
    '                End If
    '            End If
    '            oFatturaScaglione.sOperatore = StringOperation.Formatstring(myrow("operatore"))
    '            'ridimensiono l'array
    '            nList += 1
    '            ReDim Preserve oListFatturaScaglioni(nList)
    '            oListFatturaScaglioni(nList) = oFatturaScaglione
    '        Loop

    '        Return oListFatturaScaglioni
    '    Catch Err As Exception
    '        Log.Debug("Si è verificato un errore in ClsFatturaScaglioni::GetFatturaScaglioni::" & Err.Message & " SQL: " & sSQL)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsTariffe.GetFatturaScaglioni.errore: ", Err)
    '        Return Nothing
    '    Finally
    '        dvmydati.dispose()
    '    End Try
    'End Function

    'Public Function GetFatturaQuoteFisse(ByVal nIdFattura As Integer, ByVal WFSessione As OPENUtility.CreateSessione) As ObjTariffeQuotaFissa()
    '    Dim culture As IFormatProvider
    '    culture = New System.Globalization.CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")
    '    Dim sSQL As String=""
    '    Dim dvMyDati As new dataview=nothing
    '    Try
    '        Dim oFatturaQuotaFissa As ObjTariffeQuotaFissa
    '        Dim oListFatturaQuoteFisse() As ObjTariffeQuotaFissa
    '        Dim nList As Integer = -1

    '        sSQL = "SELECT TP_FATTURE_NOTE_QUOTA_FISSA.ID, IDFATTURANOTA, TP_FATTURE_NOTE_QUOTA_FISSA.IDENTE, TP_FATTURE_NOTE_QUOTA_FISSA.ANNO, ID_QUOTAFISSA,"
    '        sSQL += " DA, A, TP_FATTURE_NOTE_QUOTA_FISSA.ALIQUOTA, TP_FATTURE_NOTE_QUOTA_FISSA.IMPORTO,"
    '        '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
    '        'sSQL += " TP_QUOTA_FISSA.IMPORTO AS TARIFFA,"
    '        sSQL += " CASE WHEN TIPO_CANONE=2 THEN IMPORTOFOG WHEN TIPO_CANONE=1 THEN IMPORTODEP ELSE IMPORTOH2O END AS TARIFFA,"
    '        sSQL += " TIPO_CANONE,"
    '        '*** ***
    '        sSQL += " DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE, OPERATORE"
    '        sSQL += " FROM TP_FATTURE_NOTE_QUOTA_FISSA "
    '        sSQL += " INNER JOIN TP_QUOTA_FISSA ON TP_FATTURE_NOTE_QUOTA_FISSA.ID_QUOTAFISSA=TP_QUOTA_FISSA.ID"
    '        sSQL += " WHERE (IDFATTURANOTA=" & nIdFattura & ")"
    '        'eseguo la query
    '        dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '        Do While dvMyDati.Read
    '            oFatturaQuotaFissa = New ObjTariffeQuotaFissa
    '            oFatturaQuotaFissa.Id = StringOperation.Formatint(myrow("id"))
    '            oFatturaQuotaFissa.nIdFattura = StringOperation.Formatint(myrow("idfatturanota"))
    '            oFatturaQuotaFissa.sIdEnte = StringOperation.Formatstring(myrow("idente"))
    '            oFatturaQuotaFissa.sAnno = StringOperation.Formatstring(myrow("anno"))
    '            oFatturaQuotaFissa.nIdQuotaFissa = StringOperation.Formatint(myrow("id_quotafissa"))
    '            oFatturaQuotaFissa.nDa = StringOperation.Formatint(myrow("da"))
    '            oFatturaQuotaFissa.nA = StringOperation.Formatint(myrow("a"))
    '            oFatturaQuotaFissa.impTariffa = StringOperation.Formatdouble(myrow("tariffa"))
    '            oFatturaQuotaFissa.nAliquota = StringOperation.Formatdouble(myrow("aliquota"))
    '            oFatturaQuotaFissa.impQuotaFissa = StringOperation.Formatdouble(myrow("importo"))
    '            '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
    '            oFatturaQuotaFissa.nIdTipoCanone = StringOperation.Formatint(myrow("TIPO_CANONE"))
    '            '*** ***
    '            If Not IsDBNull(myrow("data_inserimento")) Then
    '                If StringOperation.Formatstring(myrow("data_inserimento")) <> "" Then
    '                    oFatturaQuotaFissa.tDataInserimento = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_inserimento")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("data_variazione")) Then
    '                If StringOperation.Formatstring(myrow("data_variazione")) <> "" Then
    '                    oFatturaQuotaFissa.tDataVariazione = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_variazione")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("data_cessazione")) Then
    '                If StringOperation.Formatstring(myrow("data_cessazione")) <> "" Then
    '                    oFatturaQuotaFissa.tDataCessazione = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_cessazione")))
    '                End If
    '            End If
    '            oFatturaQuotaFissa.sOperatore = StringOperation.Formatstring(myrow("operatore"))
    '            'ridimensiono l'array
    '            nList += 1
    '            ReDim Preserve oListFatturaQuoteFisse(nList)
    '            oListFatturaQuoteFisse(nList) = oFatturaQuotaFissa
    '        Loop

    '        Return oListFatturaQuoteFisse
    '    Catch Err As Exception
    '        Log.Debug("Si è verificato un errore in ClsFatturaScaglioni::GetFatturaQuoteFisse::" & Err.Message & " SQL: " & sSQL)
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsTariffe.GetFatturaQuoteFisse.errore: ", Err)
    '        Return Nothing
    '    Finally
    '        dvmydati.dispose()
    '    End Try
    'End Function

    'Public Function GetFatturaNolo(ByVal nIdFattura As Integer, ByVal WFSessione As OPENUtility.CreateSessione) As ObjTariffeNolo()
    '    Dim culture As IFormatProvider
    '    culture = New System.Globalization.CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")
    '    Dim sSQL As String=""
    '    Dim dvMyDati As new dataview=nothing
    '    Try
    '        Dim oFatturaNolo As ObjTariffeNolo
    '        Dim oListFatturaNolo() As ObjTariffeNolo
    '        Dim nList As Integer = -1

    '        sSQL = "SELECT TP_FATTURE_NOTE_NOLO.ID, IDFATTURANOTA, TP_FATTURE_NOTE_NOLO.IDENTE, TP_FATTURE_NOTE_NOLO.ANNO, ID_NOLO,"
    '        sSQL += " TP_NOLO.IMPORTO AS TARIFFA, TP_FATTURE_NOTE_NOLO.ALIQUOTA, TP_FATTURE_NOTE_NOLO.IMPORTO,"
    '        sSQL += " DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE, OPERATORE, TP_NOLO.ISUNATANTUM"
    '        sSQL += " FROM TP_FATTURE_NOTE_NOLO "
    '        sSQL += " INNER JOIN TP_NOLO ON TP_FATTURE_NOTE_NOLO.ID_NOLO=TP_NOLO.ID"
    '        sSQL += " WHERE (IDFATTURANOTA=" & nIdFattura & ")"
    '        'eseguo la query
    '        dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '        Do While dvMyDati.Read
    '            oFatturaNolo = New ObjTariffeNolo
    '            oFatturaNolo.Id = StringOperation.Formatint(myrow("id"))
    '            oFatturaNolo.nIdFattura = StringOperation.Formatint(myrow("idfatturanota"))
    '            oFatturaNolo.sIdEnte = StringOperation.Formatstring(myrow("idente"))
    '            oFatturaNolo.sAnno = StringOperation.Formatstring(myrow("anno"))
    '            oFatturaNolo.nIdNolo = StringOperation.Formatint(myrow("id_nolo"))
    '            oFatturaNolo.impTariffa = StringOperation.Formatdouble(myrow("tariffa"))
    '            oFatturaNolo.nAliquota = StringOperation.Formatdouble(myrow("aliquota"))
    '            oFatturaNolo.impNolo = StringOperation.Formatdouble(myrow("importo"))
    '            If Not IsDBNull(myrow("data_inserimento")) Then
    '                If StringOperation.Formatstring(myrow("data_inserimento")) <> "" Then
    '                    oFatturaNolo.tDataInserimento = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_inserimento")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("data_variazione")) Then
    '                If StringOperation.Formatstring(myrow("data_variazione")) <> "" Then
    '                    oFatturaNolo.tDataVariazione = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_variazione")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("data_cessazione")) Then
    '                If StringOperation.Formatstring(myrow("data_cessazione")) <> "" Then
    '                    oFatturaNolo.tDataCessazione = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_cessazione")))
    '                End If
    '            End If
    '            oFatturaNolo.sOperatore = StringOperation.Formatstring(myrow("operatore"))
    '            If Not IsDBNull(myrow("isunatantum")) Then
    '                oFatturaNolo.bIsUnaTantum = StringOperation.Formatbool(myrow("isunatantum"))
    '            End If
    '            'ridimensiono l'array
    '            nList += 1
    '            ReDim Preserve oListFatturaNolo(nList)
    '            oListFatturaNolo(nList) = oFatturaNolo
    '        Loop

    '        Return oListFatturaNolo
    '    Catch Err As Exception
    '        Log.Debug("Si è verificato un errore in ClsFatturaScaglioni::GetFatturaNolo::" & Err.Message & " SQL: " & sSQL)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsTariffe.GetFatturaNolo.errore: ", Err)
    '        Return Nothing
    '    Finally
    '        dvmydati.dispose()
    '    End Try
    'End Function

    'Public Function GetFatturaAddizionali(ByVal nIdFattura As Integer, ByVal WFSessione As OPENUtility.CreateSessione) As ObjTariffeAddizionale()
    '    Dim culture As IFormatProvider
    '    culture = New System.Globalization.CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")
    '    Dim sSQL As String=""
    '    Dim dvMyDati As new dataview=nothing
    '    Try
    '        Dim oFatturaAddizionale As ObjTariffeAddizionale
    '        Dim oListFatturaAddizionali() As ObjTariffeAddizionale
    '        Dim nList As Integer = -1

    '        sSQL = "SELECT TP_FATTURE_NOTE_ADDIZIONALI.ID, IDFATTURANOTA, TP_FATTURE_NOTE_ADDIZIONALI.IDENTE, TP_FATTURE_NOTE_ADDIZIONALI.ANNO, TP_FATTURE_NOTE_ADDIZIONALI.ID_ADDIZIONALE,"
    '        sSQL += " DESCRIZIONE, TP_ADDIZIONALI_ENTE.IMPORTO AS TARIFFA, TP_FATTURE_NOTE_ADDIZIONALI.ALIQUOTA, TP_FATTURE_NOTE_ADDIZIONALI.IMPORTO,"
    '        sSQL += " DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE, OPERATORE"
    '        sSQL += " FROM TP_FATTURE_NOTE_ADDIZIONALI"
    '        sSQL += " INNER JOIN TP_ADDIZIONALI_ENTE ON TP_FATTURE_NOTE_ADDIZIONALI.ID_ADDIZIONALE=TP_ADDIZIONALI_ENTE.ID"
    '        sSQL += " INNER JOIN TP_ADDIZIONALI ON TP_ADDIZIONALI_ENTE.ID_ADDIZIONALE=TP_ADDIZIONALI.ID_ADDIZIONALE"
    '        sSQL += " WHERE (IDFATTURANOTA=" & nIdFattura & ")"
    '        'eseguo la query
    '        dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '        Do While dvMyDati.Read
    '            oFatturaAddizionale = New ObjTariffeAddizionale
    '            oFatturaAddizionale.Id = StringOperation.Formatint(myrow("id"))
    '            oFatturaAddizionale.nIdFattura = StringOperation.Formatint(myrow("idfatturanota"))
    '            oFatturaAddizionale.sIdEnte = StringOperation.Formatstring(myrow("idente"))
    '            oFatturaAddizionale.sAnno = StringOperation.Formatstring(myrow("anno"))
    '            oFatturaAddizionale.nIdAddizionale = StringOperation.Formatint(myrow("id_addizionale"))
    '            oFatturaAddizionale.sDescrizione = StringOperation.Formatstring(myrow("descrizione"))
    '            oFatturaAddizionale.impTariffa = StringOperation.Formatdouble(myrow("tariffa"))
    '            oFatturaAddizionale.nAliquota = StringOperation.Formatdouble(myrow("aliquota"))
    '            oFatturaAddizionale.impAddizionale = StringOperation.Formatdouble(myrow("importo"))
    '            If Not IsDBNull(myrow("data_inserimento")) Then
    '                If StringOperation.Formatstring(myrow("data_inserimento")) <> "" Then
    '                    oFatturaAddizionale.tDataInserimento = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_inserimento")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("data_variazione")) Then
    '                If StringOperation.Formatstring(myrow("data_variazione")) <> "" Then
    '                    oFatturaAddizionale.tDataVariazione = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_variazione")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("data_cessazione")) Then
    '                If StringOperation.Formatstring(myrow("data_cessazione")) <> "" Then
    '                    oFatturaAddizionale.tDataCessazione = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_cessazione")))
    '                End If
    '            End If
    '            oFatturaAddizionale.sOperatore = StringOperation.Formatstring(myrow("operatore"))
    '            'ridimensiono l'array
    '            nList += 1
    '            ReDim Preserve oListFatturaAddizionali(nList)
    '            oListFatturaAddizionali(nList) = oFatturaAddizionale
    '        Loop

    '        Return oListFatturaAddizionali
    '    Catch Err As Exception
    '        Log.Debug("Si è verificato un errore in ClsFatturaScaglioni::GetFatturaAddizionali::" & Err.Message & " SQL: " & sSQL)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsTariffe.GetFatturaAddizionali.errore: ", Err)
    '        Return Nothing
    '    Finally
    '        dvmydati.dispose()
    '    End Try
    'End Function

    'Public Function GetFatturaDettaglioIva(ByVal nIdFattura As Integer, ByVal WFSessione As OPENUtility.CreateSessione) As ObjDettaglioIva()
    '    Dim culture As IFormatProvider
    '    culture = New System.Globalization.CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")
    '    Dim sSQL As String=""
    '    Dim dvMyDati As new dataview=nothing
    '    Try
    '        Dim oFatturaDettaglioIva As ObjDettaglioIva
    '        Dim oListFatturaDettaglioIva() As ObjDettaglioIva
    '        Dim nList As Integer = -1

    '        sSQL = "SELECT IDDETTAGLIOIVA, IDFATTURANOTA, IDENTE,"
    '        sSQL += " DESCRIZIONE= CASE WHEN COD_CAPITOLO='0000' THEN"
    '        sSQL += " CASE WHEN ALIQUOTA=0 THEN 'IMPONIBILE ESENTE' ELSE 'IMPONIBILE AL ' + CAST(ALIQUOTA AS NVARCHAR) +'%' END"
    '        sSQL += " WHEN COD_CAPITOLO='9996' THEN 'IVA AL ' + CAST(ALIQUOTA AS NVARCHAR) +'%'"
    '        sSQL += " ELSE 'ARROTONDAMENTO' END,"
    '        sSQL += " SUM(IMPORTO) AS IMPORTO,"
    '        sSQL += " DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE, OPERATORE"
    '        sSQL += " FROM TP_FATTURE_NOTE_DETTAGLIOIVA"
    '        sSQL += " WHERE (IDFATTURANOTA=" & nIdFattura & ")"
    '        sSQL += " GROUP BY IDDETTAGLIOIVA, IDFATTURANOTA, IDENTE,"
    '        sSQL += " CASE WHEN COD_CAPITOLO='0000' THEN"
    '        sSQL += " CASE WHEN ALIQUOTA=0 THEN 'IMPONIBILE ESENTE' ELSE 'IMPONIBILE AL ' + CAST(ALIQUOTA AS NVARCHAR) +'%' END"
    '        sSQL += " WHEN COD_CAPITOLO='9996' THEN 'IVA AL ' + CAST(ALIQUOTA AS NVARCHAR) +'%'"
    '        sSQL += " ELSE 'ARROTONDAMENTO' END,"
    '        sSQL += " DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE, OPERATORE"
    '        'eseguo la query
    '        dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '        Do While dvMyDati.Read
    '            oFatturaDettaglioIva = New ObjDettaglioIva
    '            oFatturaDettaglioIva.IdDettaglioIva = StringOperation.Formatint(myrow("iddettaglioiva"))
    '            oFatturaDettaglioIva.nIdFatturaNota = StringOperation.Formatint(myrow("idfatturanota"))
    '            oFatturaDettaglioIva.sIdEnte = StringOperation.Formatstring(myrow("idente"))
    '            oFatturaDettaglioIva.sDescrizione = StringOperation.Formatstring(myrow("descrizione"))
    '            oFatturaDettaglioIva.impDettaglio = StringOperation.Formatdouble(myrow("importo"))
    '            If Not IsDBNull(myrow("data_inserimento")) Then
    '                If StringOperation.Formatstring(myrow("data_inserimento")) <> "" Then
    '                    oFatturaDettaglioIva.tDataInserimento = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_inserimento")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("data_variazione")) Then
    '                If StringOperation.Formatstring(myrow("data_variazione")) <> "" Then
    '                    oFatturaDettaglioIva.tDataVariazione = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_variazione")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("data_cessazione")) Then
    '                If StringOperation.Formatstring(myrow("data_cessazione")) <> "" Then
    '                    oFatturaDettaglioIva.tDataCessazione = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_cessazione")))
    '                End If
    '            End If
    '            oFatturaDettaglioIva.sOperatore = StringOperation.Formatstring(myrow("operatore"))
    '            'ridimensiono l'array
    '            nList += 1
    '            ReDim Preserve oListFatturaDettaglioIva(nList)
    '            oListFatturaDettaglioIva(nList) = oFatturaDettaglioIva
    '        Loop

    '        Return oListFatturaDettaglioIva
    '    Catch Err As Exception
    '        Log.Debug("Si è verificato un errore in ClsFatturaScaglioni::GetFatturaDettaglioIva::" & Err.Message & " SQL: " & sSQL)
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsTariffe.GetFatturaDettaglioIva.errore: ", Err)
    '        Return Nothing
    '    Finally
    '        dvmydati.dispose()
    '    End Try
    'End Function

    'Public Function SetFatturaScaglioni(ByVal oScaglione As ObjTariffeScaglione, ByVal nDbOperation As Integer, ByVal WFSessione As OPENUtility.CreateSessione) As Integer
    '    Dim culture As IFormatProvider
    '    culture = New System.Globalization.CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")
    '    Dim sSQL As String=""
    '    Try
    '        Dim myIdentity As Integer

    '        'costruisco la query
    '        Select Case nDbOperation
    '            Case 0
    '                sSQL = "INSERT INTO TP_FATTURE_NOTE_SCAGLIONI (IDFATTURANOTA, IDENTE, ANNO, ID_SCAGLIONE, ALIQUOTA, QUANTITA, IMPORTO,"
    '                sSQL += " DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE, OPERATORE)"
    '                sSQL += "VALUES (" & oScaglione.nIdFattura & ",'" & oScaglione.sIdEnte & "','" & oScaglione.sAnno & "'," & oScaglione.nIdScaglione & "," & oReplace.ReplaceNumberForDB(oScaglione.nAliquota) & "," & oScaglione.nQuantita & "," & oReplace.ReplaceNumberForDB(oScaglione.impScaglione) & ","
    '                sSQL += "'" & oReplace.GiraData(oScaglione.tDataInserimento.ToString) & "',"
    '                If oScaglione.tDataVariazione <> Date.MinValue Then
    '                    sSQL += "'" & oReplace.GiraData(oScaglione.tDataVariazione.ToString) & "',"
    '                Else
    '                    sSQL += "NULL,"
    '                End If
    '                If oScaglione.tDataCessazione <> Date.MinValue Then
    '                    sSQL += "'" & oReplace.GiraData(oScaglione.tDataCessazione.ToString) & "',"
    '                Else
    '                    sSQL += "NULL,"
    '                End If
    '                sSQL += "'" & oScaglione.sOperatore & "')"
    '                sSQL += " SELECT @@IDENTITY"
    '                'eseguo la query
    '                Dim dvMyDati As new dataview
    '                dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '                Do While dvMyDati.Read
    '                    myIdentity = myrow(0)
    '                Loop
    '                dvmydati.dispose()
    '            Case 1
    '                sSQL = "UPDATE TP_FATTURE_NOTE_SCAGLIONI SET IDFATTURANOTA=" & oScaglione.nIdFattura & ","
    '                sSQL += "IDENTE='" & oScaglione.sIdEnte & "',"
    '                sSQL += "ANNO='" & oScaglione.sAnno & "',"
    '                sSQL += "ID_SCAGLIONE=" & oScaglione.nIdScaglione & ","
    '                sSQL += "ALIQUOTA=" & oReplace.ReplaceNumberForDB(oScaglione.nAliquota) & ","
    '                sSQL += "QUANTITA=" & oScaglione.nQuantita & ","
    '                sSQL += "IMPORTO=" & oReplace.ReplaceNumberForDB(oScaglione.impScaglione) & ","
    '                sSQL += "DATA_INSERIMENTO='" & oReplace.GiraData(oScaglione.tDataInserimento.ToString) & "',"
    '                If oScaglione.tDataVariazione <> Date.MinValue Then
    '                    sSQL += "DATA_VARIAZIONE='" & oReplace.GiraData(oScaglione.tDataVariazione.ToString) & "',"
    '                Else
    '                    sSQL += "DATA_VARIAZIONE=NULL,"
    '                End If
    '                If oScaglione.tDataCessazione <> Date.MinValue Then
    '                    sSQL += "DATA_CESSAZIONE='" & oReplace.GiraData(oScaglione.tDataCessazione.ToString) & "',"
    '                Else
    '                    sSQL += "DATA_CESSAZIONE=NULL,"
    '                End If
    '                sSQL += "OPERATORE='" & oScaglione.sOperatore & "'"
    '                sSQL += " WHERE (ID = " & oScaglione.Id & ")"
    '                'eseguo la query
    '                If WFSessione.oSession.oAppDB.Execute(sSQL) <> 1 Then
    '                    Return 0
    '                End If
    '                myIdentity = oScaglione.Id
    '            Case 2
    '                sSQL = "DELETE"
    '                sSQL += " FROM TP_FATTURE_NOTE_SCAGLIONI"
    '                sSQL += " WHERE (ID=" & oScaglione.Id & ")"
    '                'eseguo la query
    '                If WFSessione.oSession.oAppDB.Execute(sSQL) <> 1 Then
    '                    Return 0
    '                End If
    '                myIdentity = oScaglione.Id
    '        End Select

    '        Return myIdentity
    '    Catch Err As Exception
    '        Log.Debug("Si è verificato un errore in ClsTariffe::SetFatturaScaglioni::" & Err.Message & " SQL: " & sSQL)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsTariffe.SetFatturaScaglioni.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function

    'Public Function SetFatturaCanoni(ByVal oCanone As ObjTariffeCanone, ByVal nDbOperation As Integer, ByVal WFSessione As OPENUtility.CreateSessione) As Integer
    '    Dim culture As IFormatProvider
    '    culture = New System.Globalization.CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")
    '    Dim sSQL As String=""
    '    Try
    '        Dim myIdentity As Integer

    '        'costruisco la query
    '        Select Case nDbOperation
    '            Case 0
    '                sSQL = "INSERT INTO TP_FATTURE_NOTE_CANONI (IDFATTURANOTA, IDENTE, ANNO, ID_CANONE, ALIQUOTA, IMPORTO,"
    '                sSQL += " DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE, OPERATORE)"
    '                sSQL += "VALUES (" & oCanone.nIdFattura & ",'" & oCanone.sIdEnte & "','" & oCanone.sAnno & "'," & oCanone.nIdCanone & "," & oReplace.ReplaceNumberForDB(oCanone.nAliquota) & "," & oReplace.ReplaceNumberForDB(oCanone.impCanone) & ","
    '                sSQL += "'" & oReplace.GiraData(oCanone.tDataInserimento.ToString) & "',"
    '                If oCanone.tDataVariazione <> Date.MinValue Then
    '                    sSQL += "'" & oReplace.GiraData(oCanone.tDataVariazione.ToString) & "',"
    '                Else
    '                    sSQL += "NULL,"
    '                End If
    '                If oCanone.tDataCessazione <> Date.MinValue Then
    '                    sSQL += "'" & oReplace.GiraData(oCanone.tDataCessazione.ToString) & "',"
    '                Else
    '                    sSQL += "NULL,"
    '                End If
    '                sSQL += "'" & oCanone.sOperatore & "')"
    '                sSQL += " SELECT @@IDENTITY"
    '                'eseguo la query
    '                Dim dvMyDati As new dataview
    '                dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '                Do While dvMyDati.Read
    '                    myIdentity = myrow(0)
    '                Loop
    '                dvmydati.dispose()
    '            Case 1
    '                sSQL = "UPDATE TP_FATTURE_NOTE_CANONI SET IDFATTURANOTA=" & oCanone.nIdFattura & ","
    '                sSQL += "IDENTE='" & oCanone.sIdEnte & "',"
    '                sSQL += "ANNO='" & oCanone.sAnno & "',"
    '                sSQL += "ID_CANONE=" & oCanone.nIdCanone & ","
    '                sSQL += "ALIQUOTA=" & oReplace.ReplaceNumberForDB(oCanone.nAliquota) & ","
    '                sSQL += "IMPORTO=" & oReplace.ReplaceNumberForDB(oCanone.impCanone) & ","
    '                sSQL += "DATA_INSERIMENTO='" & oReplace.GiraData(oCanone.tDataInserimento.ToString) & "',"
    '                If oCanone.tDataVariazione <> Date.MinValue Then
    '                    sSQL += "DATA_VARIAZIONE='" & oReplace.GiraData(oCanone.tDataVariazione.ToString) & "',"
    '                Else
    '                    sSQL += "DATA_VARIAZIONE=NULL,"
    '                End If
    '                If oCanone.tDataCessazione <> Date.MinValue Then
    '                    sSQL += "DATA_CESSAZIONE='" & oReplace.GiraData(oCanone.tDataCessazione.ToString) & "',"
    '                Else
    '                    sSQL += "DATA_CESSAZIONE=NULL,"
    '                End If
    '                sSQL += "OPERATORE='" & oCanone.sOperatore & "'"
    '                sSQL += " WHERE (ID = " & oCanone.Id & ")"
    '                'eseguo la query
    '                If WFSessione.oSession.oAppDB.Execute(sSQL) <> 1 Then
    '                    Return 0
    '                End If
    '                myIdentity = oCanone.Id
    '            Case 2
    '                sSQL = "DELETE"
    '                sSQL += " FROM TP_FATTURE_NOTE_CANONI"
    '                sSQL += " WHERE (ID=" & oCanone.Id & ")"
    '                'eseguo la query
    '                If WFSessione.oSession.oAppDB.Execute(sSQL) <> 1 Then
    '                    Return 0
    '                End If
    '                myIdentity = oCanone.Id
    '        End Select

    '        Return myIdentity
    '    Catch Err As Exception
    '        Log.Debug("Si è verificato un errore in ClsTariffe::SetFatturaCanoni::" & Err.Message & " SQL: " & sSQL)
    '     Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsTariffe.SetFatturaCanoni.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function

    'Public Function SetFatturaAddizionali(ByVal oAddizionale As ObjTariffeAddizionale, ByVal nDbOperation As Integer, ByVal WFSessione As OPENUtility.CreateSessione) As Integer
    '    Dim culture As IFormatProvider
    '    culture = New System.Globalization.CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")
    '    Dim sSQL As String=""
    '    Try
    '        Dim myIdentity As Integer

    '        'costruisco la query
    '        Select Case nDbOperation
    '            Case 0
    '                sSQL = "INSERT INTO TP_FATTURE_NOTE_ADDIZIONALI (IDFATTURANOTA, IDENTE, ANNO, ID_ADDIZIONALE, ALIQUOTA, IMPORTO,"
    '                sSQL += " DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE, OPERATORE)"
    '                sSQL += "VALUES (" & oAddizionale.nIdFattura & ",'" & oAddizionale.sIdEnte & "','" & oAddizionale.sAnno & "'," & oAddizionale.nIdAddizionale & "," & oReplace.ReplaceNumberForDB(oAddizionale.nAliquota) & "," & oReplace.ReplaceNumberForDB(oAddizionale.impAddizionale) & ","
    '                sSQL += "'" & oReplace.GiraData(oAddizionale.tDataInserimento.ToString) & "',"
    '                If oAddizionale.tDataVariazione <> Date.MinValue Then
    '                    sSQL += "'" & oReplace.GiraData(oAddizionale.tDataVariazione.ToString) & "',"
    '                Else
    '                    sSQL += "NULL,"
    '                End If
    '                If oAddizionale.tDataCessazione <> Date.MinValue Then
    '                    sSQL += "'" & oReplace.GiraData(oAddizionale.tDataCessazione.ToString) & "',"
    '                Else
    '                    sSQL += "NULL,"
    '                End If
    '                sSQL += "'" & oAddizionale.sOperatore & "')"
    '                sSQL += " SELECT @@IDENTITY"
    '                'eseguo la query
    '                Dim dvMyDati As new dataview
    '                dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '                Do While dvMyDati.Read
    '                    myIdentity = myrow(0)
    '                Loop
    '                dvmydati.dispose()
    '            Case 1
    '                sSQL = "UPDATE TP_FATTURE_NOTE_ADDIZIONALI SET IDFATTURANOTA=" & oAddizionale.nIdFattura & ","
    '                sSQL += "IDENTE='" & oAddizionale.sIdEnte & "',"
    '                sSQL += "ANNO='" & oAddizionale.sAnno & "',"
    '                sSQL += "ID_ADDIZIONALE=" & oAddizionale.nIdAddizionale & ","
    '                sSQL += "ALIQUOTA=" & oReplace.ReplaceNumberForDB(oAddizionale.nAliquota) & ","
    '                sSQL += "IMPORTO=" & oReplace.ReplaceNumberForDB(oAddizionale.impAddizionale) & ","
    '                sSQL += "DATA_INSERIMENTO='" & oReplace.GiraData(oAddizionale.tDataInserimento.ToString) & "',"
    '                If oAddizionale.tDataVariazione <> Date.MinValue Then
    '                    sSQL += "DATA_VARIAZIONE='" & oReplace.GiraData(oAddizionale.tDataVariazione.ToString) & "',"
    '                Else
    '                    sSQL += "DATA_VARIAZIONE=NULL,"
    '                End If
    '                If oAddizionale.tDataCessazione <> Date.MinValue Then
    '                    sSQL += "DATA_CESSAZIONE='" & oReplace.GiraData(oAddizionale.tDataCessazione.ToString) & "',"
    '                Else
    '                    sSQL += "DATA_CESSAZIONE=NULL,"
    '                End If
    '                sSQL += "OPERATORE='" & oAddizionale.sOperatore & "'"
    '                sSQL += " WHERE (ID = " & oAddizionale.Id & ")"
    '                'eseguo la query
    '                If WFSessione.oSession.oAppDB.Execute(sSQL) <> 1 Then
    '                    Return 0
    '                End If
    '                myIdentity = oAddizionale.Id
    '            Case 2
    '                sSQL = "DELETE"
    '                sSQL += " FROM TP_FATTURE_NOTE_ADDIZIONALI"
    '                sSQL += " WHERE (ID=" & oAddizionale.Id & ")"
    '                'eseguo la query
    '                If WFSessione.oSession.oAppDB.Execute(sSQL) <> 1 Then
    '                    Return 0
    '                End If
    '                myIdentity = oAddizionale.Id
    '        End Select

    '        Return myIdentity
    '    Catch Err As Exception
    '        Log.Debug("Si è verificato un errore in ClsTariffe::SetFatturaAddizionali::" & Err.Message & " SQL: " & sSQL)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsTariffe.SetFatturaAddizionali.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function

    'Public Function SetFatturaAddizionali(ByVal oAddizionale As ObjTariffeAddizionale, ByVal nDbOperation As Integer) As Integer
    '    Dim sSQL As String=""
    '    Dim myIdentity As Integer

    '    Try
    '        'costruisco la query
    '        Select Case nDbOperation
    '            Case 0
    '                sSQL = "INSERT INTO TP_FATTURE_NOTE_ADDIZIONALI (IDFATTURANOTA, IDENTE, ANNO, ID_ADDIZIONALE, ALIQUOTA, IMPORTO,"
    '                sSQL += " DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE, OPERATORE)"
    '                sSQL += "VALUES (" & oAddizionale.nIdFattura & ",'" & oAddizionale.sIdEnte & "','" & oAddizionale.sAnno & "'," & oAddizionale.nIdAddizionale & "," & oReplace.ReplaceNumberForDB(oAddizionale.nAliquota) & "," & oReplace.ReplaceNumberForDB(oAddizionale.impAddizionale) & ","
    '                sSQL += "'" & oReplace.GiraData(oAddizionale.tDataInserimento.ToString) & "',"
    '                If oAddizionale.tDataVariazione <> Date.MinValue Then
    '                    sSQL += "'" & oReplace.GiraData(oAddizionale.tDataVariazione.ToString) & "',"
    '                Else
    '                    sSQL += "NULL,"
    '                End If
    '                If oAddizionale.tDataCessazione <> Date.MinValue Then
    '                    sSQL += "'" & oReplace.GiraData(oAddizionale.tDataCessazione.ToString) & "',"
    '                Else
    '                    sSQL += "NULL,"
    '                End If
    '                sSQL += "'" & oAddizionale.sOperatore & "')"
    '                sSQL += " SELECT @@IDENTITY"
    '                'eseguo la query
    '                Dim dvMyDati As new dataview
    '                dvMyDati = iDB.getdataview(sSQL)
    '                Do While dvMyDati.Read
    '                    myIdentity = myrow(0)
    '                Loop
    '                dvmydati.dispose()
    '            Case 1
    '                sSQL = "UPDATE TP_FATTURE_NOTE_ADDIZIONALI SET IDFATTURANOTA=" & oAddizionale.nIdFattura & ","
    '                sSQL += "IDENTE='" & oAddizionale.sIdEnte & "',"
    '                sSQL += "ANNO='" & oAddizionale.sAnno & "',"
    '                sSQL += "ID_ADDIZIONALE=" & oAddizionale.nIdAddizionale & ","
    '                sSQL += "ALIQUOTA=" & oReplace.ReplaceNumberForDB(oAddizionale.nAliquota) & ","
    '                sSQL += "IMPORTO=" & oReplace.ReplaceNumberForDB(oAddizionale.impAddizionale) & ","
    '                sSQL += "DATA_INSERIMENTO='" & oReplace.GiraData(oAddizionale.tDataInserimento.ToString) & "',"
    '                If oAddizionale.tDataVariazione <> Date.MinValue Then
    '                    sSQL += "DATA_VARIAZIONE='" & oReplace.GiraData(oAddizionale.tDataVariazione.ToString) & "',"
    '                Else
    '                    sSQL += "DATA_VARIAZIONE=NULL,"
    '                End If
    '                If oAddizionale.tDataCessazione <> Date.MinValue Then
    '                    sSQL += "DATA_CESSAZIONE='" & oReplace.GiraData(oAddizionale.tDataCessazione.ToString) & "',"
    '                Else
    '                    sSQL += "DATA_CESSAZIONE=NULL,"
    '                End If
    '                sSQL += "OPERATORE='" & oAddizionale.sOperatore & "'"
    '                sSQL += " WHERE (ID = " & oAddizionale.Id & ")"
    '                'eseguo la query
    '                If iDB.ExecuteNonQuery(sSQL) <> 1 Then
    '                    Throw New Exception("errore in::" & sSQL)
    '                End If
    '                myIdentity = oAddizionale.Id
    '            Case 2
    '                sSQL = "DELETE"
    '                sSQL += " FROM TP_FATTURE_NOTE_ADDIZIONALI"
    '                sSQL += " WHERE (ID=" & oAddizionale.Id & ")"
    '                'eseguo la query
    '                If iDB.ExecuteNonQuery(sSQL) <> 1 Then
    '                    Throw New Exception("errore in::" & sSQL)
    '                End If
    '                myIdentity = oAddizionale.Id
    '        End Select

    '        Return myIdentity
    '    Catch Err As Exception
    '        Log.Debug("Si è verificato un errore in ClsTariffe::SetFatturaAddizionali::" & Err.Message & " SQL: " & sSQL)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsTariffe.SetFatturaAddizionali.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function

    'Public Function SetFatturaNolo(ByVal oNolo As ObjTariffeNolo, ByVal nDbOperation As Integer, ByVal WFSessione As OPENUtility.CreateSessione) As Integer
    '    Dim culture As IFormatProvider
    '    culture = New System.Globalization.CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")
    '    Dim sSQL As String=""
    '    Try
    '        Dim myIdentity As Integer

    '        'costruisco la query
    '        Select Case nDbOperation
    '            Case 0
    '                sSQL = "INSERT INTO TP_FATTURE_NOTE_NOLO (IDFATTURANOTA, IDENTE, ANNO, ID_NOLO, ALIQUOTA, IMPORTO,"
    '                sSQL += " DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE, OPERATORE)"
    '                sSQL += "VALUES (" & oNolo.nIdFattura & ",'" & oNolo.sIdEnte & "','" & oNolo.sAnno & "'," & oNolo.nIdNolo & "," & oReplace.ReplaceNumberForDB(oNolo.nAliquota) & "," & oReplace.ReplaceNumberForDB(oNolo.impNolo) & ","
    '                sSQL += "'" & oReplace.GiraData(oNolo.tDataInserimento.ToString) & "',"
    '                If oNolo.tDataVariazione <> Date.MinValue Then
    '                    sSQL += "'" & oReplace.GiraData(oNolo.tDataVariazione.ToString) & "',"
    '                Else
    '                    sSQL += "NULL,"
    '                End If
    '                If oNolo.tDataCessazione <> Date.MinValue Then
    '                    sSQL += "'" & oReplace.GiraData(oNolo.tDataCessazione.ToString) & "',"
    '                Else
    '                    sSQL += "NULL,"
    '                End If
    '                sSQL += "'" & oNolo.sOperatore & "')"
    '                sSQL += " SELECT @@IDENTITY"
    '                'eseguo la query
    '                Dim dvMyDati As new dataview
    '                dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '                Do While dvMyDati.Read
    '                    myIdentity = myrow(0)
    '                Loop
    '                dvmydati.dispose()
    '            Case 1
    '                sSQL = "UPDATE TP_FATTURE_NOTE_NOLO SET IDFATTURANOTA=" & oNolo.nIdFattura & ","
    '                sSQL += "IDENTE='" & oNolo.sIdEnte & "',"
    '                sSQL += "ANNO='" & oNolo.sAnno & "',"
    '                sSQL += "ID_NOLO=" & oNolo.nIdNolo & ","
    '                sSQL += "ALIQUOTA=" & oReplace.ReplaceNumberForDB(oNolo.nAliquota) & ","
    '                sSQL += "IMPORTO=" & oReplace.ReplaceNumberForDB(oNolo.impNolo) & ","
    '                sSQL += "DATA_INSERIMENTO='" & oReplace.GiraData(oNolo.tDataInserimento.ToString) & "',"
    '                If oNolo.tDataVariazione <> Date.MinValue Then
    '                    sSQL += "DATA_VARIAZIONE='" & oReplace.GiraData(oNolo.tDataVariazione.ToString) & "',"
    '                Else
    '                    sSQL += "DATA_VARIAZIONE=NULL,"
    '                End If
    '                If oNolo.tDataCessazione <> Date.MinValue Then
    '                    sSQL += "DATA_CESSAZIONE='" & oReplace.GiraData(oNolo.tDataCessazione.ToString) & "',"
    '                Else
    '                    sSQL += "DATA_CESSAZIONE=NULL,"
    '                End If
    '                sSQL += "OPERATORE='" & oNolo.sOperatore & "'"
    '                sSQL += " WHERE (ID = " & oNolo.Id & ")"
    '                'eseguo la query
    '                If WFSessione.oSession.oAppDB.Execute(sSQL) <> 1 Then
    '                    Return 0
    '                End If
    '                myIdentity = oNolo.Id
    '            Case 2
    '                sSQL = "DELETE"
    '                sSQL += " FROM TP_FATTURE_NOTE_NOLO"
    '                sSQL += " WHERE (ID=" & oNolo.Id & ")"
    '                'eseguo la query
    '                If WFSessione.oSession.oAppDB.Execute(sSQL) <> 1 Then
    '                    Return 0
    '                End If
    '                myIdentity = oNolo.Id
    '        End Select

    '        Return myIdentity
    '    Catch Err As Exception
    '        Log.Debug("Si è verificato un errore in ClsTariffe::SetFatturaNolo::" & Err.Message & " SQL: " & sSQL)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsTariffe.SetFatturaNolo.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function

    'Public Function SetFatturaQuoteFisse(ByVal oQuotaFissa As ObjTariffeQuotaFissa, ByVal nDbOperation As Integer, ByVal WFSessione As OPENUtility.CreateSessione) As Integer
    '    Dim culture As IFormatProvider
    '    culture = New System.Globalization.CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")
    '    Dim sSQL As String=""
    '    Try
    '        Dim myIdentity As Integer

    '        'costruisco la query
    '        Select Case nDbOperation
    '            Case 0
    '                '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
    '                sSQL = "INSERT INTO TP_FATTURE_NOTE_QUOTA_FISSA (IDFATTURANOTA, IDENTE, ANNO, ID_QUOTAFISSA, ALIQUOTA, IMPORTO,"
    '                sSQL += " TIPO_CANONE,"
    '                sSQL += " DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE, OPERATORE)"
    '                sSQL += "VALUES (" & oQuotaFissa.nIdFattura & ",'" & oQuotaFissa.sIdEnte & "','" & oQuotaFissa.sAnno & "'," & oQuotaFissa.nIdQuotaFissa & "," & oReplace.ReplaceNumberForDB(oQuotaFissa.nAliquota) & "," & oReplace.ReplaceNumberForDB(oQuotaFissa.impQuotaFissa) & ","
    '                sSQL += oQuotaFissa.nIdTipoCanone & ","
    '                sSQL += "'" & oReplace.GiraData(oQuotaFissa.tDataInserimento.ToString) & "',"
    '                If oQuotaFissa.tDataVariazione <> Date.MinValue Then
    '                    sSQL += "'" & oReplace.GiraData(oQuotaFissa.tDataVariazione.ToString) & "',"
    '                Else
    '                    sSQL += "NULL,"
    '                End If
    '                If oQuotaFissa.tDataCessazione <> Date.MinValue Then
    '                    sSQL += "'" & oReplace.GiraData(oQuotaFissa.tDataCessazione.ToString) & "',"
    '                Else
    '                    sSQL += "NULL,"
    '                End If
    '                sSQL += "'" & oQuotaFissa.sOperatore & "')"
    '                sSQL += " SELECT @@IDENTITY"
    '                'eseguo la query
    '                Dim dvMyDati As new dataview
    '                dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '                Do While dvMyDati.Read
    '                    myIdentity = myrow(0)
    '                Loop
    '                dvmydati.dispose()
    '            Case 1
    '                sSQL = "UPDATE TP_FATTURE_NOTE_QUOTA_FISSA SET IDFATTURANOTA=" & oQuotaFissa.nIdFattura & ","
    '                sSQL += "IDENTE='" & oQuotaFissa.sIdEnte & "',"
    '                sSQL += "ANNO='" & oQuotaFissa.sAnno & "',"
    '                sSQL += "ID_QUOTAFISSA=" & oQuotaFissa.nIdQuotaFissa & ","
    '                sSQL += "ALIQUOTA=" & oReplace.ReplaceNumberForDB(oQuotaFissa.nAliquota) & ","
    '                sSQL += "IMPORTO=" & oReplace.ReplaceNumberForDB(oQuotaFissa.impQuotaFissa) & ","
    '                sSQL += "TIPO_CANONE=" & oQuotaFissa.nIdTipoCanone & ","
    '                sSQL += "DATA_INSERIMENTO='" & oReplace.GiraData(oQuotaFissa.tDataInserimento.ToString) & "',"
    '                If oQuotaFissa.tDataVariazione <> Date.MinValue Then
    '                    sSQL += "DATA_VARIAZIONE='" & oReplace.GiraData(oQuotaFissa.tDataVariazione.ToString) & "',"
    '                Else
    '                    sSQL += "DATA_VARIAZIONE=NULL,"
    '                End If
    '                If oQuotaFissa.tDataCessazione <> Date.MinValue Then
    '                    sSQL += "DATA_CESSAZIONE='" & oReplace.GiraData(oQuotaFissa.tDataCessazione.ToString) & "',"
    '                Else
    '                    sSQL += "DATA_CESSAZIONE=NULL,"
    '                End If
    '                sSQL += "OPERATORE='" & oQuotaFissa.sOperatore & "'"
    '                sSQL += " WHERE (ID = " & oQuotaFissa.Id & ")"
    '                'eseguo la query
    '                If WFSessione.oSession.oAppDB.Execute(sSQL) <> 1 Then
    '                    Return 0
    '                End If
    '                myIdentity = oQuotaFissa.Id
    '            Case 2
    '                sSQL = "DELETE"
    '                sSQL += " FROM TP_FATTURE_NOTE_QUOTA_FISSA"
    '                sSQL += " WHERE (ID=" & oQuotaFissa.Id & ")"
    '                'eseguo la query
    '                If WFSessione.oSession.oAppDB.Execute(sSQL) <> 1 Then
    '                    Return 0
    '                End If
    '                myIdentity = oQuotaFissa.Id
    '        End Select

    '        Return myIdentity
    '    Catch Err As Exception
    '        Log.Debug("Si è verificato un errore in ClsTariffe::SetFatturaQuoteFisse::" & Err.Message & " SQL: " & sSQL & "::connessione::" & WFSessione.oSession.oAppDB.GetConnection.ConnectionString)
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsTariffe.SetFatturaQuoteFisse.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function

    'Public Function SetFatturaDettaglioIva(ByVal oDettaglioIva As ObjDettaglioIva, ByVal nDbOperation As Integer, ByVal WFSessione As OPENUtility.CreateSessione) As Integer
    '    Dim culture As IFormatProvider
    '    culture = New System.Globalization.CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")
    '    Dim sSQL As String=""
    '    Try
    '        Dim myIdentity As Integer

    '        'costruisco la query
    '        Select Case nDbOperation
    '            Case 0
    '                sSQL = "INSERT INTO TP_FATTURE_NOTE_DETTAGLIOIVA (IDFATTURANOTA, IDENTE, COD_CAPITOLO, ALIQUOTA, IMPORTO,"
    '                sSQL += " DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE, OPERATORE)"
    '                sSQL += "VALUES (" & oDettaglioIva.nIdFatturaNota & ",'" & oDettaglioIva.sIdEnte & "','" & oDettaglioIva.sCapitolo & "'," & oReplace.ReplaceNumberForDB(oDettaglioIva.nAliquota) & "," & oReplace.ReplaceNumberForDB(oDettaglioIva.impDettaglio) & ","
    '                sSQL += "'" & oReplace.GiraData(oDettaglioIva.tDataInserimento.ToString) & "',"
    '                If oDettaglioIva.tDataVariazione <> Date.MinValue Then
    '                    sSQL += "'" & oReplace.GiraData(oDettaglioIva.tDataVariazione.ToString) & "',"
    '                Else
    '                    sSQL += "NULL,"
    '                End If
    '                If oDettaglioIva.tDataCessazione <> Date.MinValue Then
    '                    sSQL += "'" & oReplace.GiraData(oDettaglioIva.tDataCessazione.ToString) & "',"
    '                Else
    '                    sSQL += "NULL,"
    '                End If
    '                sSQL += "'" & oDettaglioIva.sOperatore & "')"
    '                sSQL += " SELECT @@IDENTITY"
    '                'eseguo la query
    '                Dim dvMyDati As new dataview
    '                dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '                Do While dvMyDati.Read
    '                    myIdentity = myrow(0)
    '                Loop
    '                dvmydati.dispose()
    '            Case 1
    '                sSQL = "UPDATE TP_FATTURE_NOTE_DETTAGLIOIVA SET IDFATTURANOTA=" & oDettaglioIva.nIdFatturaNota & ","
    '                sSQL += "IDENTE='" & oDettaglioIva.sIdEnte & "',"
    '                sSQL += "COD_CAPITOLO='" & oDettaglioIva.sCapitolo & "',"
    '                sSQL += "ALIQUOTA=" & oReplace.ReplaceNumberForDB(oDettaglioIva.nAliquota) & ","
    '                sSQL += "IMPORTO=" & oReplace.ReplaceNumberForDB(oDettaglioIva.impDettaglio) & ","
    '                sSQL += "DATA_INSERIMENTO='" & oReplace.GiraData(oDettaglioIva.tDataInserimento.ToString) & "',"
    '                If oDettaglioIva.tDataVariazione <> Date.MinValue Then
    '                    sSQL += "DATA_VARIAZIONE='" & oReplace.GiraData(oDettaglioIva.tDataVariazione.ToString) & "',"
    '                Else
    '                    sSQL += "DATA_VARIAZIONE=NULL,"
    '                End If
    '                If oDettaglioIva.tDataCessazione <> Date.MinValue Then
    '                    sSQL += "DATA_CESSAZIONE='" & oReplace.GiraData(oDettaglioIva.tDataCessazione.ToString) & "',"
    '                Else
    '                    sSQL += "DATA_CESSAZIONE=NULL,"
    '                End If
    '                sSQL += "OPERATORE='" & oDettaglioIva.sOperatore & "'"
    '                sSQL += " WHERE (IDDETTAGLIOIVA = " & oDettaglioIva.IdDettaglioIva & ")"
    '                'eseguo la query
    '                If WFSessione.oSession.oAppDB.Execute(sSQL) <> 1 Then
    '                    Return 0
    '                End If
    '                myIdentity = oDettaglioIva.IdDettaglioIva
    '            Case 2
    '                sSQL = "DELETE"
    '                sSQL += " FROM TP_FATTURE_NOTE_DETTAGLIOIVA"
    '                sSQL += " WHERE (IDDETTAGLIOIVA=" & oDettaglioIva.IdDettaglioIva & ")"
    '                'eseguo la query
    '                If WFSessione.oSession.oAppDB.Execute(sSQL) <> 1 Then
    '                    Return 0
    '                End If
    '                myIdentity = oDettaglioIva.IdDettaglioIva
    '        End Select

    '        Return myIdentity
    '    Catch Err As Exception
    '        Log.Debug("Si è verificato un errore in ClsTariffe::SetFatturaDettaglioIva::" & Err.Message & " SQL: " & sSQL)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsTariffe.SetFatturaDettaglioIva.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sEnte">stringa Codice Ente in lavorazione</param>
    ''' <param name="sAnno">stringa Anno di selezione</param>
    ''' <param name="oRuoloH2O">out oggetto di tipo ObjTotRuoloFatture aggiornato in caso di mancanza configurazioni</param>
    ''' <returns>oggetto di tipo ObjTariffe contenente tutte le tariffe valide per i parametri</returns>
    ''' <remarks></remarks>
    Public Function GetTariffe(ByVal sEnte As String, ByVal sAnno As String, ByRef oRuoloH2O As ObjTotRuoloFatture) As ObjTariffe
        Dim oMyTariffe As New ObjTariffe
        Dim FunctionScaglioni As New ClsScaglioni
        Dim oMyScaglione As New OggettoScaglione
        Dim FunctionCanoni As New ClsCanoni
        Dim oMyCanone As New OggettoCanone
        Dim FunctionAddizionali As New ClsAddizionali
        Dim oMyAddizionale As New OggettoAddizionaleEnte
        Dim FunctionNolo As New ClsNoliContatore
        Dim oMyNolo As New OggettoNoloContatore
        Dim FunctionQuote As New ClsQuotaFissa
        Dim oMyQuota As New OggettoQuotaFissa

        Try
            'prelevo le tariffe
            'SCAGLIONI - valorizzo i parametri di ricerca
            oMyScaglione.sIdEnte = sEnte
            oMyScaglione.sAnno = sAnno
            oMyTariffe.oScaglioni = FunctionScaglioni.GetScaglioniEnte(oMyScaglione)
            'CANONI - valorizzo i parametri di ricerca
            oMyCanone.sIdEnte = sEnte
            oMyCanone.sAnno = sAnno
            oMyTariffe.oCanoni = FunctionCanoni.GetCanoniEnte(oMyCanone)
            'ADDIZIONALI - valorizzo i parametri di ricerca
            oMyAddizionale.sIdEnte = sEnte
            oMyAddizionale.sAnno = sAnno
            oMyTariffe.oAddizionali = FunctionAddizionali.GetAddizionaliEnte(oMyAddizionale)
            'NOLO - valorizzo i parametri di ricerca
            oMyNolo.sIdEnte = sEnte
            oMyNolo.sAnno = sAnno
            oMyTariffe.oNolo = FunctionNolo.GetNoliContatoreEnte(oMyNolo)
            'QUOTE FISSE - valorizzo i parametri di ricerca
            oMyQuota.sIdEnte = sEnte
            oMyQuota.sAnno = sAnno
            oMyTariffe.oQuoteFisse = FunctionQuote.GetQuotaFissaEnte(oMyQuota)

            If oMyTariffe.oAddizionali Is Nothing And oMyTariffe.oCanoni Is Nothing And oMyTariffe.oNolo Is Nothing And oMyTariffe.oQuoteFisse Is Nothing And oMyTariffe.oScaglioni Is Nothing Then
                oRuoloH2O.sNote = "Tariffe non configurate."
                Return Nothing
            Else
                Return oMyTariffe
            End If
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsTariffe.GetTariffe.errore: ", Err)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nIdFattura"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFatturaCanoni(ByVal nIdFattura As Integer) As ObjTariffeCanone()
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Dim oFatturaCanone As ObjTariffeCanone
        Dim oListFatturaCanoni() As ObjTariffeCanone
        Dim nList As Integer = -1

        oListFatturaCanoni = Nothing
        Try
            sSQL = "SELECT TP_FATTURE_NOTE_CANONI.ID, IDFATTURANOTA, TP_FATTURE_NOTE_CANONI.IDENTE, TP_FATTURE_NOTE_CANONI.ANNO, ID_CANONE, TP_TIPOLOGIE_CANONI.ID_TIPO_CANONE, IDSERVIZIO"
            sSQL += ", DESCRIZIONE, TARIFFA, PERCENTUALE_SUL_CONSUMO, TP_FATTURE_NOTE_CANONI.ALIQUOTA, IMPORTO"
            sSQL += ", DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE, OPERATORE"
            sSQL += " FROM TP_FATTURE_NOTE_CANONI"
            sSQL += " INNER JOIN TP_CANONI ON TP_FATTURE_NOTE_CANONI.ID_CANONE=TP_CANONI.ID"
            sSQL += " INNER JOIN TP_TIPOLOGIE_CANONI ON TP_CANONI.ID_TIPO_CANONE=TP_TIPOLOGIE_CANONI.ID_TIPO_CANONE"
            sSQL += " WHERE (IDFATTURANOTA=" & nIdFattura & ")"
            'eseguo la query
            dvMyDati = iDB.GetDataView(sSQL)
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    oFatturaCanone = New ObjTariffeCanone
                    oFatturaCanone.Id = StringOperation.FormatInt(myRow("id"))
                    oFatturaCanone.nIdFattura = StringOperation.FormatInt(myRow("idfatturanota"))
                    oFatturaCanone.sIdEnte = StringOperation.FormatString(myRow("idente"))
                    oFatturaCanone.sAnno = StringOperation.FormatString(myRow("anno"))
                    oFatturaCanone.nIdCanone = StringOperation.FormatInt(myRow("id_canone"))
                    oFatturaCanone.idTipoCanone = StringOperation.FormatInt(myRow("ID_TIPO_CANONE"))
                    oFatturaCanone.idServizio = StringOperation.FormatInt(myRow("IDSERVIZIO"))
                    oFatturaCanone.sDescrizione = StringOperation.FormatString(myRow("descrizione"))
                    oFatturaCanone.impTariffa = StringOperation.FormatDouble(myRow("tariffa"))
                    oFatturaCanone.nPercentSulConsumo = StringOperation.FormatDouble(myRow("percentuale_sul_consumo"))
                    oFatturaCanone.nAliquota = StringOperation.FormatDouble(myRow("aliquota"))
                    oFatturaCanone.impCanone = StringOperation.FormatDouble(myRow("importo"))
                    If StringOperation.FormatString(myRow("data_inserimento")) <> "" Then
                        oFatturaCanone.tDataInserimento = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_inserimento")))
                    End If
                    If StringOperation.FormatString(myRow("data_variazione")) <> "" Then
                        oFatturaCanone.tDataVariazione = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_variazione")))
                    End If
                    If StringOperation.FormatString(myRow("data_cessazione")) <> "" Then
                        oFatturaCanone.tDataCessazione = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_cessazione")))
                    End If
                    oFatturaCanone.sOperatore = StringOperation.FormatString(myRow("operatore"))
                    'ridimensiono l'array
                    nList += 1
                    ReDim Preserve oListFatturaCanoni(nList)
                    oListFatturaCanoni(nList) = oFatturaCanone
                Next
            End If

            Return oListFatturaCanoni
        Catch Err As Exception
            Log.Debug("Si è verificato un errore in ClsFatturaScaglioni::GetFatturaCanoni::" & Err.Message & " SQL: " & sSQL)

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsTariffe.GetFatturaCanoni.errore: ", Err)
            Return Nothing
        Finally
            dvMyDati.Dispose()
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nIdFattura"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFatturaScaglioni(ByVal nIdFattura As Integer) As ObjTariffeScaglione()
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Dim oFatturaScaglione As ObjTariffeScaglione
        Dim oListFatturaScaglioni() As ObjTariffeScaglione
        Dim nList As Integer = -1

        oListFatturaScaglioni = Nothing
        Try
            sSQL = "SELECT TP_FATTURE_NOTE_SCAGLIONI.ID, IDFATTURANOTA, TP_FATTURE_NOTE_SCAGLIONI.IDENTE, TP_FATTURE_NOTE_SCAGLIONI.ANNO, ID_SCAGLIONE,"
            sSQL += " DA, A, TARIFFA, MINIMO, TP_FATTURE_NOTE_SCAGLIONI.ALIQUOTA, QUANTITA, IMPORTO,"
            sSQL += " DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE, OPERATORE"
            sSQL += " FROM TP_FATTURE_NOTE_SCAGLIONI"
            sSQL += " INNER JOIN TP_SCAGLIONI ON TP_FATTURE_NOTE_SCAGLIONI.ID_SCAGLIONE=TP_SCAGLIONI.ID"
            sSQL += " WHERE (IDFATTURANOTA=" & nIdFattura & ")"
            'eseguo la query
            dvMyDati = iDB.GetDataView(sSQL)
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    oFatturaScaglione = New ObjTariffeScaglione
                    oFatturaScaglione.Id = StringOperation.FormatInt(myRow("id"))
                    oFatturaScaglione.nIdFattura = StringOperation.FormatInt(myRow("idfatturanota"))
                    oFatturaScaglione.sIdEnte = StringOperation.FormatString(myRow("idente"))
                    oFatturaScaglione.sAnno = StringOperation.FormatString(myRow("anno"))
                    oFatturaScaglione.nIdScaglione = StringOperation.FormatInt(myRow("id_scaglione"))
                    oFatturaScaglione.nDa = StringOperation.FormatInt(myRow("da"))
                    oFatturaScaglione.nA = StringOperation.FormatInt(myRow("a"))
                    oFatturaScaglione.nQuantita = StringOperation.FormatInt(myRow("quantita"))
                    oFatturaScaglione.impTariffa = StringOperation.FormatDouble(myRow("tariffa"))
                    oFatturaScaglione.impMinimo = StringOperation.FormatDouble(myRow("minimo"))
                    oFatturaScaglione.nAliquota = StringOperation.FormatDouble(myRow("aliquota"))
                    oFatturaScaglione.impScaglione = StringOperation.FormatDouble(myRow("importo"))
                    If StringOperation.FormatString(myRow("data_inserimento")) <> "" Then
                        oFatturaScaglione.tDataInserimento = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_inserimento")))
                    End If
                    If StringOperation.FormatString(myRow("data_variazione")) <> "" Then
                        oFatturaScaglione.tDataVariazione = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_variazione")))
                    End If
                    If StringOperation.FormatString(myRow("data_cessazione")) <> "" Then
                        oFatturaScaglione.tDataCessazione = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_cessazione")))
                    End If
                    oFatturaScaglione.sOperatore = StringOperation.FormatString(myRow("operatore"))
                    'ridimensiono l'array
                    nList += 1
                    ReDim Preserve oListFatturaScaglioni(nList)
                    oListFatturaScaglioni(nList) = oFatturaScaglione
                Next
            End If

            Return oListFatturaScaglioni
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsTariffe.GetFatturaScaglioni.errore: ", Err)
            Return Nothing
        Finally
            dvMyDati.Dispose()
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nIdFattura"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFatturaQuoteFisse(ByVal nIdFattura As Integer) As ObjTariffeQuotaFissa()
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Dim oFatturaQuotaFissa As ObjTariffeQuotaFissa
        Dim oListFatturaQuoteFisse() As ObjTariffeQuotaFissa
        Dim nList As Integer = -1

        oListFatturaQuoteFisse = Nothing
        Try
            sSQL = "SELECT TP_FATTURE_NOTE_QUOTA_FISSA.ID, IDFATTURANOTA, TP_FATTURE_NOTE_QUOTA_FISSA.IDENTE, TP_FATTURE_NOTE_QUOTA_FISSA.ANNO, ID_QUOTAFISSA,"
            sSQL += " DA, A, TP_FATTURE_NOTE_QUOTA_FISSA.ALIQUOTA, TP_FATTURE_NOTE_QUOTA_FISSA.IMPORTO,"
            '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
            'sSQL += " TP_QUOTA_FISSA.IMPORTO AS TARIFFA,"
            sSQL += " CASE WHEN TIPO_CANONE=2 THEN IMPORTOFOG WHEN TIPO_CANONE=1 THEN IMPORTODEP ELSE IMPORTOH2O END AS TARIFFA,"
            sSQL += " TIPO_CANONE,"
            '*** ***
            sSQL += " DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE, OPERATORE"
            sSQL += " FROM TP_FATTURE_NOTE_QUOTA_FISSA "
            sSQL += " INNER JOIN TP_QUOTA_FISSA ON TP_FATTURE_NOTE_QUOTA_FISSA.ID_QUOTAFISSA=TP_QUOTA_FISSA.ID"
            sSQL += " WHERE (IDFATTURANOTA=" & nIdFattura & ")"
            'eseguo la query
            dvMyDati = iDB.GetDataView(sSQL)
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    oFatturaQuotaFissa = New ObjTariffeQuotaFissa
                    oFatturaQuotaFissa.Id = StringOperation.FormatInt(myRow("id"))
                    oFatturaQuotaFissa.nIdFattura = StringOperation.FormatInt(myRow("idfatturanota"))
                    oFatturaQuotaFissa.sIdEnte = StringOperation.FormatString(myRow("idente"))
                    oFatturaQuotaFissa.sAnno = StringOperation.FormatString(myRow("anno"))
                    oFatturaQuotaFissa.nIdQuotaFissa = StringOperation.FormatInt(myRow("id_quotafissa"))
                    oFatturaQuotaFissa.nDa = StringOperation.FormatInt(myRow("da"))
                    oFatturaQuotaFissa.nA = StringOperation.FormatInt(myRow("a"))
                    oFatturaQuotaFissa.impTariffa = StringOperation.FormatDouble(myRow("tariffa"))
                    oFatturaQuotaFissa.nAliquota = StringOperation.FormatDouble(myRow("aliquota"))
                    oFatturaQuotaFissa.impQuotaFissa = StringOperation.FormatDouble(myRow("importo"))
                    '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
                    oFatturaQuotaFissa.nIdTipoCanone = StringOperation.FormatInt(myRow("TIPO_CANONE"))
                    '*** ***
                    If StringOperation.FormatString(myRow("data_inserimento")) <> "" Then
                        oFatturaQuotaFissa.tDataInserimento = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_inserimento")))
                    End If
                    If StringOperation.FormatString(myRow("data_variazione")) <> "" Then
                        oFatturaQuotaFissa.tDataVariazione = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_variazione")))
                    End If
                    If StringOperation.FormatString(myRow("data_cessazione")) <> "" Then
                        oFatturaQuotaFissa.tDataCessazione = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_cessazione")))
                    End If
                    oFatturaQuotaFissa.sOperatore = StringOperation.FormatString(myRow("operatore"))
                    'ridimensiono l'array
                    nList += 1
                    ReDim Preserve oListFatturaQuoteFisse(nList)
                    oListFatturaQuoteFisse(nList) = oFatturaQuotaFissa
                Next
            End If

            Return oListFatturaQuoteFisse
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsTariffe.GetFatturaQuoteFisse.errore: ", Err)
            Return Nothing
        Finally
            dvMyDati.Dispose()
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nIdFattura"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFatturaNolo(ByVal nIdFattura As Integer) As ObjTariffeNolo()
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Dim oFatturaNolo As ObjTariffeNolo
        Dim oListFatturaNolo() As ObjTariffeNolo
        Dim nList As Integer = -1

        oListFatturaNolo = Nothing
        Try
            sSQL = "SELECT TP_FATTURE_NOTE_NOLO.ID, IDFATTURANOTA, TP_FATTURE_NOTE_NOLO.IDENTE, TP_FATTURE_NOTE_NOLO.ANNO, ID_NOLO,"
            sSQL += " TP_NOLO.IMPORTO AS TARIFFA, TP_FATTURE_NOTE_NOLO.ALIQUOTA, TP_FATTURE_NOTE_NOLO.IMPORTO,"
            sSQL += " DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE, OPERATORE, TP_NOLO.ISUNATANTUM"
            sSQL += " FROM TP_FATTURE_NOTE_NOLO "
            sSQL += " INNER JOIN TP_NOLO ON TP_FATTURE_NOTE_NOLO.ID_NOLO=TP_NOLO.ID"
            sSQL += " WHERE (IDFATTURANOTA=" & nIdFattura & ")"
            'eseguo la query
            dvMyDati = iDB.GetDataView(sSQL)
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    oFatturaNolo = New ObjTariffeNolo
                    oFatturaNolo.Id = StringOperation.FormatInt(myRow("id"))
                    oFatturaNolo.nIdFattura = StringOperation.FormatInt(myRow("idfatturanota"))
                    oFatturaNolo.sIdEnte = StringOperation.FormatString(myRow("idente"))
                    oFatturaNolo.sAnno = StringOperation.FormatString(myRow("anno"))
                    oFatturaNolo.nIdNolo = StringOperation.FormatInt(myRow("id_nolo"))
                    oFatturaNolo.impTariffa = StringOperation.FormatDouble(myRow("tariffa"))
                    oFatturaNolo.nAliquota = StringOperation.FormatDouble(myRow("aliquota"))
                    oFatturaNolo.impNolo = StringOperation.FormatDouble(myRow("importo"))
                    If StringOperation.FormatString(myRow("data_inserimento")) <> "" Then
                        oFatturaNolo.tDataInserimento = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_inserimento")))
                    End If
                    If StringOperation.FormatString(myRow("data_variazione")) <> "" Then
                        oFatturaNolo.tDataVariazione = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_variazione")))
                    End If
                    If StringOperation.FormatString(myRow("data_cessazione")) <> "" Then
                        oFatturaNolo.tDataCessazione = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_cessazione")))
                    End If
                    oFatturaNolo.sOperatore = StringOperation.FormatString(myRow("operatore"))
                    If Not IsDBNull(myRow("isunatantum")) Then
                        oFatturaNolo.bIsUnaTantum = StringOperation.FormatBool(myRow("isunatantum"))
                    End If
                    'ridimensiono l'array
                    nList += 1
                    ReDim Preserve oListFatturaNolo(nList)
                    oListFatturaNolo(nList) = oFatturaNolo
                Next
            End If

            Return oListFatturaNolo
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsTariffe.GetFatturaNolo.errore: ", Err)
            Return Nothing
        Finally
            dvMyDati.Dispose()
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nIdFattura"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFatturaAddizionali(ByVal nIdFattura As Integer) As ObjTariffeAddizionale()
        Dim oFatturaAddizionale As ObjTariffeAddizionale
        Dim oListFatturaAddizionali() As ObjTariffeAddizionale
        Dim nList As Integer = -1

        oListFatturaAddizionali = Nothing
        Try
            '*** 20141117 - voce di costo specifica per utente ***
            'sSQL = "SELECT TP_FATTURE_NOTE_ADDIZIONALI.ID, IDFATTURANOTA, TP_FATTURE_NOTE_ADDIZIONALI.IDENTE, TP_FATTURE_NOTE_ADDIZIONALI.ANNO, TP_FATTURE_NOTE_ADDIZIONALI.ID_ADDIZIONALE,"
            'sSQL += " DESCRIZIONE, TP_ADDIZIONALI_ENTE.IMPORTO AS TARIFFA, TP_FATTURE_NOTE_ADDIZIONALI.ALIQUOTA, TP_FATTURE_NOTE_ADDIZIONALI.IMPORTO,"
            'sSQL += " DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE, OPERATORE"
            'sSQL += " FROM TP_FATTURE_NOTE_ADDIZIONALI"
            'sSQL += " INNER JOIN TP_ADDIZIONALI_ENTE ON TP_FATTURE_NOTE_ADDIZIONALI.ID_ADDIZIONALE=TP_ADDIZIONALI_ENTE.ID"
            'sSQL += " INNER JOIN TP_ADDIZIONALI ON TP_ADDIZIONALI_ENTE.ID_ADDIZIONALE=TP_ADDIZIONALI.ID_ADDIZIONALE"
            'sSQL += " WHERE (IDFATTURANOTA=" & nIdFattura & ")"
            Dim sSQL As String = ""
            Dim dvMyDati As New DataView
            Try
                Dim oDbManagerRepository As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                Using ctx As DBModel = oDbManagerRepository
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "PRC_GETFATTUREADDIZIONALI", "IDFATTURA")
                    dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDFATTURA", nIdFattura))
                    ctx.Dispose()
                End Using
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        oFatturaAddizionale = New ObjTariffeAddizionale
                        oFatturaAddizionale.Id = StringOperation.FormatInt(myRow("id"))
                        oFatturaAddizionale.nIdFattura = StringOperation.FormatInt(myRow("idfatturanota"))
                        oFatturaAddizionale.sIdEnte = StringOperation.FormatString(myRow("idente"))
                        oFatturaAddizionale.sAnno = StringOperation.FormatString(myRow("anno"))
                        oFatturaAddizionale.nIdAddizionale = StringOperation.FormatInt(myRow("id_addizionale"))
                        oFatturaAddizionale.sDescrizione = StringOperation.FormatString(myRow("descrizione"))
                        oFatturaAddizionale.impTariffa = StringOperation.FormatDouble(myRow("tariffa"))
                        oFatturaAddizionale.nAliquota = StringOperation.FormatDouble(myRow("aliquota"))
                        oFatturaAddizionale.impAddizionale = StringOperation.FormatDouble(myRow("importo"))
                        If StringOperation.FormatString(myRow("data_inserimento")) <> "" Then
                            oFatturaAddizionale.tDataInserimento = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_inserimento")))
                        End If
                        If StringOperation.FormatString(myRow("data_variazione")) <> "" Then
                            oFatturaAddizionale.tDataVariazione = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_variazione")))
                        End If
                        If StringOperation.FormatString(myRow("data_cessazione")) <> "" Then
                            oFatturaAddizionale.tDataCessazione = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_cessazione")))
                        End If
                        oFatturaAddizionale.sOperatore = StringOperation.FormatString(myRow("operatore"))
                        'ridimensiono l'array
                        nList += 1
                        ReDim Preserve oListFatturaAddizionali(nList)
                        oListFatturaAddizionali(nList) = oFatturaAddizionale
                    Next
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsTariffe.GetFatturaAddizionali.sql.errore: ", ex)
            Finally
                dvMyDati.Dispose()
            End Try

            Return oListFatturaAddizionali
        Catch Err As Exception
            Log.Debug("Si è verificato un errore in ClsFatturaScaglioni::GetFatturaAddizionali::" & Err.Message & " SQL:prc_GETFATTUREADDIZIONALI @IDFATTURA=" & nIdFattura)
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsTariffe.GetFatturaAddizionali.errore: ", Err)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nIdFattura"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFatturaDettaglioIva(ByVal nIdFattura As Integer) As ObjDettaglioIva()
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Dim oFatturaDettaglioIva As ObjDettaglioIva
        Dim oListFatturaDettaglioIva() As ObjDettaglioIva
        Dim nList As Integer = -1

        oListFatturaDettaglioIva = Nothing
        Try
            sSQL = "SELECT IDDETTAGLIOIVA, IDFATTURANOTA, IDENTE,"
            sSQL += " DESCRIZIONE= CASE WHEN COD_CAPITOLO='0000' THEN"
            sSQL += " CASE WHEN ALIQUOTA=0 THEN 'IMPONIBILE ESENTE' ELSE 'IMPONIBILE AL ' + CAST(ALIQUOTA AS NVARCHAR) +'%' END"
            sSQL += " WHEN COD_CAPITOLO='9996' THEN 'IVA AL ' + CAST(ALIQUOTA AS NVARCHAR) +'%'"
            sSQL += " ELSE 'ARROTONDAMENTO' END,"
            sSQL += " SUM(IMPORTO) AS IMPORTO,"
            sSQL += " DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE, OPERATORE"
            sSQL += " FROM TP_FATTURE_NOTE_DETTAGLIOIVA"
            sSQL += " WHERE (IDFATTURANOTA=" & nIdFattura & ")"
            sSQL += " GROUP BY IDDETTAGLIOIVA, IDFATTURANOTA, IDENTE,"
            sSQL += " CASE WHEN COD_CAPITOLO='0000' THEN"
            sSQL += " CASE WHEN ALIQUOTA=0 THEN 'IMPONIBILE ESENTE' ELSE 'IMPONIBILE AL ' + CAST(ALIQUOTA AS NVARCHAR) +'%' END"
            sSQL += " WHEN COD_CAPITOLO='9996' THEN 'IVA AL ' + CAST(ALIQUOTA AS NVARCHAR) +'%'"
            sSQL += " ELSE 'ARROTONDAMENTO' END,"
            sSQL += " DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE, OPERATORE"
            'eseguo la query
            dvMyDati = iDB.GetDataView(sSQL)
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    oFatturaDettaglioIva = New ObjDettaglioIva
                    oFatturaDettaglioIva.IdDettaglioIva = StringOperation.FormatInt(myRow("iddettaglioiva"))
                    oFatturaDettaglioIva.nIdFatturaNota = StringOperation.FormatInt(myRow("idfatturanota"))
                    oFatturaDettaglioIva.sIdEnte = StringOperation.FormatString(myRow("idente"))
                    oFatturaDettaglioIva.sDescrizione = StringOperation.FormatString(myRow("descrizione"))
                    oFatturaDettaglioIva.impDettaglio = StringOperation.FormatDouble(myRow("importo"))
                    If StringOperation.FormatString(myRow("data_inserimento")) <> "" Then
                        oFatturaDettaglioIva.tDataInserimento = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_inserimento")))
                    End If
                    If StringOperation.FormatString(myRow("data_variazione")) <> "" Then
                        oFatturaDettaglioIva.tDataVariazione = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_variazione")))
                    End If
                    If StringOperation.FormatString(myRow("data_cessazione")) <> "" Then
                        oFatturaDettaglioIva.tDataCessazione = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_cessazione")))
                    End If
                    oFatturaDettaglioIva.sOperatore = StringOperation.FormatString(myRow("operatore"))
                    'ridimensiono l'array
                    nList += 1
                    ReDim Preserve oListFatturaDettaglioIva(nList)
                    oListFatturaDettaglioIva(nList) = oFatturaDettaglioIva
                Next
            End If

            Return oListFatturaDettaglioIva
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsTariffe.GetFatturaDettaglioIva.errore: ", Err)
            Return Nothing
        Finally
            dvMyDati.Dispose()
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oScaglione"></param>
    ''' <param name="nDbOperation"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetFatturaScaglioni(ByVal oScaglione As ObjTariffeScaglione, ByVal nDbOperation As Integer) As Integer
        Dim sSQL As String = ""
        Dim myIdentity As Integer

        Try
            'costruisco la query
            Select Case nDbOperation
                Case 0
                    sSQL = "INSERT INTO TP_FATTURE_NOTE_SCAGLIONI (IDFATTURANOTA, IDENTE, ANNO, ID_SCAGLIONE, ALIQUOTA, QUANTITA, IMPORTO,"
                    sSQL += " DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE, OPERATORE)"
                    sSQL += "VALUES (" & oScaglione.nIdFattura & ",'" & oScaglione.sIdEnte & "','" & oScaglione.sAnno & "'," & oScaglione.nIdScaglione & "," & oReplace.ReplaceNumberForDB(oScaglione.nAliquota) & "," & oScaglione.nQuantita & "," & oReplace.ReplaceNumberForDB(oScaglione.impScaglione) & ","
                    sSQL += "'" & oReplace.GiraData(oScaglione.tDataInserimento.ToString) & "',"
                    If oScaglione.tDataVariazione.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                        sSQL += "'" & oReplace.GiraData(oScaglione.tDataVariazione.ToString) & "',"
                    Else
                        sSQL += "NULL,"
                    End If
                    If oScaglione.tDataCessazione.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                        sSQL += "'" & oReplace.GiraData(oScaglione.tDataCessazione.ToString) & "',"
                    Else
                        sSQL += "NULL,"
                    End If
                    sSQL += "'" & oScaglione.sOperatore & "')"
                    sSQL += " SELECT @@IDENTITY"
                    'eseguo la query
                    Dim dvMyDati As New DataView
                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            myIdentity = myRow(0)
                        Next
                    End If
                    dvMyDati.Dispose()
                Case 1
                    sSQL = "UPDATE TP_FATTURE_NOTE_SCAGLIONI SET IDFATTURANOTA=" & oScaglione.nIdFattura & ","
                    sSQL += "IDENTE='" & oScaglione.sIdEnte & "',"
                    sSQL += "ANNO='" & oScaglione.sAnno & "',"
                    sSQL += "ID_SCAGLIONE=" & oScaglione.nIdScaglione & ","
                    sSQL += "ALIQUOTA=" & oReplace.ReplaceNumberForDB(oScaglione.nAliquota) & ","
                    sSQL += "QUANTITA=" & oScaglione.nQuantita & ","
                    sSQL += "IMPORTO=" & oReplace.ReplaceNumberForDB(oScaglione.impScaglione) & ","
                    sSQL += "DATA_INSERIMENTO='" & oReplace.GiraData(oScaglione.tDataInserimento.ToString) & "',"
                    If oScaglione.tDataVariazione.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                        sSQL += "DATA_VARIAZIONE='" & oReplace.GiraData(oScaglione.tDataVariazione.ToString) & "',"
                    Else
                        sSQL += "DATA_VARIAZIONE=NULL,"
                    End If
                    If oScaglione.tDataCessazione.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                        sSQL += "DATA_CESSAZIONE='" & oReplace.GiraData(oScaglione.tDataCessazione.ToString) & "',"
                    Else
                        sSQL += "DATA_CESSAZIONE=NULL,"
                    End If
                    sSQL += "OPERATORE='" & oScaglione.sOperatore & "'"
                    sSQL += " WHERE (ID = " & oScaglione.Id & ")"
                    'eseguo la query
                    If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                        Throw New Exception("errore in::" & sSQL)
                    End If
                    myIdentity = oScaglione.Id
                Case 2
                    sSQL = "DELETE"
                    sSQL += " FROM TP_FATTURE_NOTE_SCAGLIONI"
                    sSQL += " WHERE (ID=" & oScaglione.Id & ")"
                    'eseguo la query
                    If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                        Throw New Exception("errore in::" & sSQL)
                    End If
                    myIdentity = oScaglione.Id
            End Select

            Return myIdentity
        Catch Err As Exception
            Log.Debug("Si è verificato un errore in ClsTariffe::SetFatturaScaglioni::" & Err.Message & " SQL: " & sSQL)

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsTariffe.SetFatturaScaglioni.errore: ", Err)
            Return 0
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oCanone"></param>
    ''' <param name="nDbOperation"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetFatturaCanoni(ByVal oCanone As ObjTariffeCanone, ByVal nDbOperation As Integer) As Integer
        Dim sSQL As String = ""
        Dim myIdentity As Integer

        'costruisco la query
        Try
            Select Case nDbOperation
                Case 0
                    sSQL = "INSERT INTO TP_FATTURE_NOTE_CANONI (IDFATTURANOTA, IDENTE, ANNO, ID_CANONE, ALIQUOTA, IMPORTO,"
                    sSQL += " DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE, OPERATORE)"
                    sSQL += "VALUES (" & oCanone.nIdFattura & ",'" & oCanone.sIdEnte & "','" & oCanone.sAnno & "'," & oCanone.nIdCanone & "," & oReplace.ReplaceNumberForDB(oCanone.nAliquota) & "," & oReplace.ReplaceNumberForDB(oCanone.impCanone) & ","
                    sSQL += "'" & oReplace.GiraData(oCanone.tDataInserimento.ToString) & "',"
                    If oCanone.tDataVariazione.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                        sSQL += "'" & oReplace.GiraData(oCanone.tDataVariazione.ToString) & "',"
                    Else
                        sSQL += "NULL,"
                    End If
                    If oCanone.tDataCessazione.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                        sSQL += "'" & oReplace.GiraData(oCanone.tDataCessazione.ToString) & "',"
                    Else
                        sSQL += "NULL,"
                    End If
                    sSQL += "'" & oCanone.sOperatore & "')"
                    sSQL += " SELECT @@IDENTITY"
                    'eseguo la query
                    Dim dvMyDati As New DataView
                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            myIdentity = myRow(0)
                        Next
                    End If
                    dvMyDati.Dispose()
                Case 1
                    sSQL = "UPDATE TP_FATTURE_NOTE_CANONI SET IDFATTURANOTA=" & oCanone.nIdFattura & ","
                    sSQL += "IDENTE='" & oCanone.sIdEnte & "',"
                    sSQL += "ANNO='" & oCanone.sAnno & "',"
                    sSQL += "ID_CANONE=" & oCanone.nIdCanone & ","
                    sSQL += "ALIQUOTA=" & oReplace.ReplaceNumberForDB(oCanone.nAliquota) & ","
                    sSQL += "IMPORTO=" & oReplace.ReplaceNumberForDB(oCanone.impCanone) & ","
                    sSQL += "DATA_INSERIMENTO='" & oReplace.GiraData(oCanone.tDataInserimento.ToString) & "',"
                    If oCanone.tDataVariazione.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                        sSQL += "DATA_VARIAZIONE='" & oReplace.GiraData(oCanone.tDataVariazione.ToString) & "',"
                    Else
                        sSQL += "DATA_VARIAZIONE=NULL,"
                    End If
                    If oCanone.tDataCessazione.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                        sSQL += "DATA_CESSAZIONE='" & oReplace.GiraData(oCanone.tDataCessazione.ToString) & "',"
                    Else
                        sSQL += "DATA_CESSAZIONE=NULL,"
                    End If
                    sSQL += "OPERATORE='" & oCanone.sOperatore & "'"
                    sSQL += " WHERE (ID = " & oCanone.Id & ")"
                    'eseguo la query
                    If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                        Throw New Exception("errore in::" & sSQL)
                    End If
                    myIdentity = oCanone.Id
                Case 2
                    sSQL = "DELETE"
                    sSQL += " FROM TP_FATTURE_NOTE_CANONI"
                    sSQL += " WHERE (ID=" & oCanone.Id & ")"
                    'eseguo la query
                    If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                        Throw New Exception("errore in::" & sSQL)
                    End If
                    myIdentity = oCanone.Id
            End Select

            Return myIdentity
        Catch Err As Exception
            Log.Debug("Si è verificato un errore in ClsTariffe::SetFatturaCanoni::" & Err.Message & " SQL: " & sSQL)

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsTariffe.SetFatturaCanoni.errore: ", Err)
            Return 0
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oAddizionale"></param>
    ''' <param name="nDbOperation"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetFatturaAddizionali(ByVal oAddizionale As ObjTariffeAddizionale, ByVal nDbOperation As Integer) As Integer
        Dim sSQL As String = "" = "prc_TP_FATTURE_NOTE_ADDIZIONALI_IU"
        Dim myIdentity As Integer

        Try
            'costruisco la query
            Select Case nDbOperation
                Case 0, 1
                    'eseguo la query
                    myIdentity = iDB.ExecuteNonQuery("prc_TP_FATTURE_NOTE_ADDIZIONALI_IU", New SqlClient.SqlParameter("@ID", oAddizionale.Id) _
                                                     , New SqlClient.SqlParameter("@IDFATTURANOTA", oAddizionale.nIdFattura) _
                                                     , New SqlClient.SqlParameter("@IdEnte", oAddizionale.sIdEnte) _
                                                     , New SqlClient.SqlParameter("@ANNO", oAddizionale.sAnno) _
                                                     , New SqlClient.SqlParameter("@ID_ADDIZIONALE", oAddizionale.nIdAddizionale) _
                                                     , New SqlClient.SqlParameter("@ALIQUOTA", oAddizionale.nAliquota) _
                                                     , New SqlClient.SqlParameter("@IMPORTO", oAddizionale.impAddizionale) _
                                                     , New SqlClient.SqlParameter("@DESCRIZIONE", oAddizionale.sDescrizione) _
                                                     , New SqlClient.SqlParameter("@DATA_INSERIMENTO", oReplace.GiraData(oAddizionale.tDataInserimento.ToString)) _
                                                     , New SqlClient.SqlParameter("@OPERATORE", oAddizionale.sOperatore)
                                                   )
                Case 2
                    sSQL = "DELETE"
                    sSQL += " FROM TP_FATTURE_NOTE_ADDIZIONALI"
                    sSQL += " WHERE (ID=" & oAddizionale.Id & ")"
                    'eseguo la query
                    If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                        Throw New Exception("errore in::" & sSQL)
                    End If
                    myIdentity = oAddizionale.Id
            End Select

            Return myIdentity
        Catch Err As Exception
            Log.Debug("Si è verificato un errore in ClsTariffe::SetFatturaAddizionali::" & Err.Message & " SQL: " & sSQL)

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsTariffe.SetFatturaAddizionali.errore: ", Err)
            Return 0
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oNolo"></param>
    ''' <param name="nDbOperation"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetFatturaNolo(ByVal oNolo As ObjTariffeNolo, ByVal nDbOperation As Integer) As Integer
        Dim sSQL As String = ""
        Dim myIdentity As Integer

        'costruisco la query
        Try
            Select Case nDbOperation
                Case 0
                    sSQL = "INSERT INTO TP_FATTURE_NOTE_NOLO (IDFATTURANOTA, IDENTE, ANNO, ID_NOLO, ALIQUOTA, IMPORTO,"
                    sSQL += " DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE, OPERATORE)"
                    sSQL += "VALUES (" & oNolo.nIdFattura & ",'" & oNolo.sIdEnte & "','" & oNolo.sAnno & "'," & oNolo.nIdNolo & "," & oReplace.ReplaceNumberForDB(oNolo.nAliquota) & "," & oReplace.ReplaceNumberForDB(oNolo.impNolo) & ","
                    sSQL += "'" & oReplace.GiraData(oNolo.tDataInserimento.ToString) & "',"
                    If oNolo.tDataVariazione.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                        sSQL += "'" & oReplace.GiraData(oNolo.tDataVariazione.ToString) & "',"
                    Else
                        sSQL += "NULL,"
                    End If
                    If oNolo.tDataCessazione.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                        sSQL += "'" & oReplace.GiraData(oNolo.tDataCessazione.ToString) & "',"
                    Else
                        sSQL += "NULL,"
                    End If
                    sSQL += "'" & oNolo.sOperatore & "')"
                    sSQL += " SELECT @@IDENTITY"
                    'eseguo la query
                    Dim dvMyDati As New DataView
                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            myIdentity = myRow(0)
                        Next
                    End If
                    dvMyDati.Dispose()
                Case 1
                    sSQL = "UPDATE TP_FATTURE_NOTE_NOLO SET IDFATTURANOTA=" & oNolo.nIdFattura & ","
                    sSQL += "IDENTE='" & oNolo.sIdEnte & "',"
                    sSQL += "ANNO='" & oNolo.sAnno & "',"
                    sSQL += "ID_NOLO=" & oNolo.nIdNolo & ","
                    sSQL += "ALIQUOTA=" & oReplace.ReplaceNumberForDB(oNolo.nAliquota) & ","
                    sSQL += "IMPORTO=" & oReplace.ReplaceNumberForDB(oNolo.impNolo) & ","
                    sSQL += "DATA_INSERIMENTO='" & oReplace.GiraData(oNolo.tDataInserimento.ToString) & "',"
                    If oNolo.tDataVariazione.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                        sSQL += "DATA_VARIAZIONE='" & oReplace.GiraData(oNolo.tDataVariazione.ToString) & "',"
                    Else
                        sSQL += "DATA_VARIAZIONE=NULL,"
                    End If
                    If oNolo.tDataCessazione.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                        sSQL += "DATA_CESSAZIONE='" & oReplace.GiraData(oNolo.tDataCessazione.ToString) & "',"
                    Else
                        sSQL += "DATA_CESSAZIONE=NULL,"
                    End If
                    sSQL += "OPERATORE='" & oNolo.sOperatore & "'"
                    sSQL += " WHERE (ID = " & oNolo.Id & ")"
                    'eseguo la query
                    If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                        Throw New Exception("errore in::" & sSQL)
                    End If
                    myIdentity = oNolo.Id
                Case 2
                    sSQL = "DELETE"
                    sSQL += " FROM TP_FATTURE_NOTE_NOLO"
                    sSQL += " WHERE (ID=" & oNolo.Id & ")"
                    'eseguo la query
                    If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                        Throw New Exception("errore in::" & sSQL)
                    End If
                    myIdentity = oNolo.Id
            End Select

            Return myIdentity
        Catch Err As Exception
            Log.Debug("Si è verificato un errore in ClsTariffe::SetFatturaNolo::" & Err.Message & " SQL: " & sSQL)

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsTariffe.SetFatturaNolo.errore: ", Err)
            Return 0
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oQuotaFissa"></param>
    ''' <param name="nDbOperation"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetFatturaQuoteFisse(ByVal oQuotaFissa As ObjTariffeQuotaFissa, ByVal nDbOperation As Integer) As Integer
        Dim sSQL As String = ""
        Dim myIdentity As Integer

        Try
            'costruisco la query
            Select Case nDbOperation
                Case 0
                    '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
                    sSQL = "INSERT INTO TP_FATTURE_NOTE_QUOTA_FISSA (IDFATTURANOTA, IDENTE, ANNO, ID_QUOTAFISSA, ALIQUOTA, IMPORTO,"
                    sSQL += " TIPO_CANONE,"
                    sSQL += " DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE, OPERATORE)"
                    sSQL += "VALUES (" & oQuotaFissa.nIdFattura & ",'" & oQuotaFissa.sIdEnte & "','" & oQuotaFissa.sAnno & "'," & oQuotaFissa.nIdQuotaFissa & "," & oReplace.ReplaceNumberForDB(oQuotaFissa.nAliquota) & "," & oReplace.ReplaceNumberForDB(oQuotaFissa.impQuotaFissa) & ","
                    sSQL += oQuotaFissa.nIdTipoCanone & ","
                    sSQL += "'" & oReplace.GiraData(oQuotaFissa.tDataInserimento.ToString) & "',"
                    If oQuotaFissa.tDataVariazione.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                        sSQL += "'" & oReplace.GiraData(oQuotaFissa.tDataVariazione.ToString) & "',"
                    Else
                        sSQL += "NULL,"
                    End If
                    If oQuotaFissa.tDataCessazione.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                        sSQL += "'" & oReplace.GiraData(oQuotaFissa.tDataCessazione.ToString) & "',"
                    Else
                        sSQL += "NULL,"
                    End If
                    sSQL += "'" & oQuotaFissa.sOperatore & "')"
                    sSQL += " SELECT @@IDENTITY"
                    'eseguo la query
                    Dim dvMyDati As New DataView
                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            myIdentity = myRow(0)
                        Next
                    End If
                    dvMyDati.Dispose()
                Case 1
                    sSQL = "UPDATE TP_FATTURE_NOTE_QUOTA_FISSA SET IDFATTURANOTA=" & oQuotaFissa.nIdFattura & ","
                    sSQL += "IDENTE='" & oQuotaFissa.sIdEnte & "',"
                    sSQL += "ANNO='" & oQuotaFissa.sAnno & "',"
                    sSQL += "ID_QUOTAFISSA=" & oQuotaFissa.nIdQuotaFissa & ","
                    sSQL += "ALIQUOTA=" & oReplace.ReplaceNumberForDB(oQuotaFissa.nAliquota) & ","
                    sSQL += "IMPORTO=" & oReplace.ReplaceNumberForDB(oQuotaFissa.impQuotaFissa) & ","
                    sSQL += "TIPO_CANONE=" & oQuotaFissa.nIdTipoCanone & ","
                    sSQL += "DATA_INSERIMENTO='" & oReplace.GiraData(oQuotaFissa.tDataInserimento.ToString) & "',"
                    If oQuotaFissa.tDataVariazione.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                        sSQL += "DATA_VARIAZIONE='" & oReplace.GiraData(oQuotaFissa.tDataVariazione.ToString) & "',"
                    Else
                        sSQL += "DATA_VARIAZIONE=NULL,"
                    End If
                    If oQuotaFissa.tDataCessazione.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                        sSQL += "DATA_CESSAZIONE='" & oReplace.GiraData(oQuotaFissa.tDataCessazione.ToString) & "',"
                    Else
                        sSQL += "DATA_CESSAZIONE=NULL,"
                    End If
                    sSQL += "OPERATORE='" & oQuotaFissa.sOperatore & "'"
                    sSQL += " WHERE (ID = " & oQuotaFissa.Id & ")"
                    'eseguo la query
                    If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                        Throw New Exception("errore in::" & sSQL)
                    End If
                    myIdentity = oQuotaFissa.Id
                Case 2
                    sSQL = "DELETE"
                    sSQL += " FROM TP_FATTURE_NOTE_QUOTA_FISSA"
                    sSQL += " WHERE (ID=" & oQuotaFissa.Id & ")"
                    'eseguo la query
                    If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                        Throw New Exception("errore in::" & sSQL)
                    End If
                    myIdentity = oQuotaFissa.Id
            End Select

            Return myIdentity
        Catch Err As Exception
            Log.Debug("Si è verificato un errore in ClsTariffe::SetFatturaQuoteFisse::" & Err.Message & " SQL: " & sSQL & "::connessione::" & ConstSession.StringConnection)
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsTariffe.SetFatturaQuoteFisse.errore: ", Err)
            Return 0
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oDettaglioIva"></param>
    ''' <param name="nDbOperation"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetFatturaDettaglioIva(ByVal oDettaglioIva As ObjDettaglioIva, ByVal nDbOperation As Integer) As Integer
        Dim sSQL As String = ""
        Dim myIdentity As Integer

        Try
            'costruisco la query
            Select Case nDbOperation
                Case 0
                    sSQL = "INSERT INTO TP_FATTURE_NOTE_DETTAGLIOIVA (IDFATTURANOTA, IDENTE, COD_CAPITOLO, ALIQUOTA, IMPORTO,"
                    sSQL += " DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE, OPERATORE)"
                    sSQL += "VALUES (" & oDettaglioIva.nIdFatturaNota & ",'" & oDettaglioIva.sIdEnte & "','" & oDettaglioIva.sCapitolo & "'," & oReplace.ReplaceNumberForDB(oDettaglioIva.nAliquota) & "," & oReplace.ReplaceNumberForDB(oDettaglioIva.impDettaglio) & ","
                    sSQL += "'" & oReplace.GiraData(oDettaglioIva.tDataInserimento.ToString) & "',"
                    If oDettaglioIva.tDataVariazione.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                        sSQL += "'" & oReplace.GiraData(oDettaglioIva.tDataVariazione.ToString) & "',"
                    Else
                        sSQL += "NULL,"
                    End If
                    If oDettaglioIva.tDataCessazione.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                        sSQL += "'" & oReplace.GiraData(oDettaglioIva.tDataCessazione.ToString) & "',"
                    Else
                        sSQL += "NULL,"
                    End If
                    sSQL += "'" & oDettaglioIva.sOperatore & "')"
                    sSQL += " SELECT @@IDENTITY"
                    'eseguo la query
                    Dim dvMyDati As New DataView
                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            myIdentity = myRow(0)
                        Next
                    End If
                    dvMyDati.Dispose()
                Case 1
                    sSQL = "UPDATE TP_FATTURE_NOTE_DETTAGLIOIVA SET IDFATTURANOTA=" & oDettaglioIva.nIdFatturaNota & ","
                    sSQL += "IDENTE='" & oDettaglioIva.sIdEnte & "',"
                    sSQL += "COD_CAPITOLO='" & oDettaglioIva.sCapitolo & "',"
                    sSQL += "ALIQUOTA=" & oReplace.ReplaceNumberForDB(oDettaglioIva.nAliquota) & ","
                    sSQL += "IMPORTO=" & oReplace.ReplaceNumberForDB(oDettaglioIva.impDettaglio) & ","
                    sSQL += "DATA_INSERIMENTO='" & oReplace.GiraData(oDettaglioIva.tDataInserimento.ToString) & "',"
                    If oDettaglioIva.tDataVariazione.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                        sSQL += "DATA_VARIAZIONE='" & oReplace.GiraData(oDettaglioIva.tDataVariazione.ToString) & "',"
                    Else
                        sSQL += "DATA_VARIAZIONE=NULL,"
                    End If
                    If oDettaglioIva.tDataCessazione.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                        sSQL += "DATA_CESSAZIONE='" & oReplace.GiraData(oDettaglioIva.tDataCessazione.ToString) & "',"
                    Else
                        sSQL += "DATA_CESSAZIONE=NULL,"
                    End If
                    sSQL += "OPERATORE='" & oDettaglioIva.sOperatore & "'"
                    sSQL += " WHERE (IDDETTAGLIOIVA = " & oDettaglioIva.IdDettaglioIva & ")"
                    'eseguo la query
                    If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                        Throw New Exception("errore in::" & sSQL)
                    End If
                    myIdentity = oDettaglioIva.IdDettaglioIva
                Case 2
                    sSQL = "DELETE"
                    sSQL += " FROM TP_FATTURE_NOTE_DETTAGLIOIVA"
                    sSQL += " WHERE (IDDETTAGLIOIVA=" & oDettaglioIva.IdDettaglioIva & ")"
                    'eseguo la query
                    If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                        Throw New Exception("errore in::" & sSQL)
                    End If
                    myIdentity = oDettaglioIva.IdDettaglioIva
            End Select

            Return myIdentity
        Catch Err As Exception
            Log.Debug("Si è verificato un errore in ClsTariffe::SetFatturaDettaglioIva::" & Err.Message & " SQL: " & sSQL)

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsTariffe.SetFatturaDettaglioIva.errore: ", Err)
            Return 0
        End Try
    End Function
End Class

Public Class ClsRate
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ClsRate))
    Private iDB As New DBAccess.getDBobject
    Private oReplace As New ClsGenerale.Generale

#Region "Configurazione Rate"
    'Public Function GetConfiguraRata(ByVal nIdRuolo As Integer) As ObjConfiguraRata()
    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Dim sSQL, WFErrore As String
    '    Dim dvMyDati As new dataview=nothing
    '    Dim oRata As ObjConfiguraRata
    '    Dim oListRate() As ObjConfiguraRata
    '    Dim nList As Integer = -1

    '    Try
    '        'inizializzo la connessione
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If
    '        sSQL = "SELECT *"
    '        sSQL += " FROM TP_RATEENTE"
    '        sSQL += " WHERE (IDRUOLO=" & nIdRuolo & ")"
    '        sSQL += " ORDER BY NRATA, DATASCADENZA"
    '        'eseguo la query
    '        dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '        Do While dvMyDati.Read
    '            oRata = New ObjConfiguraRata
    '            oRata.Id = StringOperation.Formatint(myrow("id"))
    '            oRata.nIdRuolo = StringOperation.Formatint(myrow("idruolo"))
    '            oRata.sIdEnte = StringOperation.Formatstring(myrow("idente"))
    '            oRata.sNRata = StringOperation.Formatstring(myrow("nrata")).ToUpper
    '            oRata.sDescrRata = StringOperation.Formatstring(myrow("descrizionerata")).ToUpper
    '            oRata.tDataScadenza = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("datascadenza")))
    '            'ridimensiono l'array
    '            nList += 1
    '            ReDim Preserve oListRate(nList)
    '            oListRate(nList) = oRata
    '        Loop

    '        Return oListRate
    '    Catch Err As Exception
    '        Log.Debug("Si è verificato un errore in ClsRate::GetConfiguraRata::" & Err.Message & " SQL: " & sSQL)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsRate.GetConfiguraRata.errore: ", Err)
    '        Return Nothing
    '    Finally
    '        dvmydati.dispose()
    '        WFSessione.Kill()
    '    End Try
    'End Function

    'Public Function ConfiguraRata(ByVal oListRate() As ObjConfiguraRata, ByRef sError As String) As Boolean
    '    Dim WFErrore As String
    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Dim x As Integer
    '    Dim sNRataPrec As String
    '    Dim bPresenzaUS As Boolean = False
    '    Dim FncRuolo As New ClsRuoloH2O

    '    Try
    '        'inizializzo la connessione
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        For x = 0 To oListRate.GetUpperBound(0)
    '            If UCase(oListRate(x).sNRata) <> "U" Then
    '                If oListRate(x).sNRata <> StringOperation.Formatint(sNRataPrec) + 1 Then
    '                    'se è presente blocco l'operazione e do un messaggio
    '                    sError = "Inserire una Rata consecutiva all\'ultima configurata."
    '                    Return False
    '                ElseIf oListRate(x).sNRata = StringOperation.Formatint(sNRataPrec) Then
    '                    'se è presente blocco l'operazione e do un messaggio
    '                    sError = "Inserire una Rata consecutiva all\'ultima configurata."
    '                    Return False
    '                End If
    '            Else
    '                bPresenzaUS = True
    '            End If
    '            sNRataPrec = oListRate(x).sNRata
    '        Next
    '        If bPresenzaUS = False Then
    '            sError = "Inserire l'UNICA SOLUZIONE."
    '            Return False
    '        End If
    '        'elimino le rate precedentemente configurate
    '        If SetConfigRata(oListRate(0), 2, WFSessione) < 0 Then
    '            sError = "Errore in inserimento rata."
    '            Return False
    '        End If
    '        'inserisco le nuove rate
    '        For x = 0 To oListRate.GetUpperBound(0)
    '            If oListRate(x).sNRata <> "" Then
    '                If SetConfigRata(oListRate(x), 0, WFSessione) < 1 Then
    '                    Return False
    '                End If
    '                If oListRate(x).sNRata = "U" Then
    '                    'memorizzo l'unica soluzione sul ruolo
    '                    If FncRuolo.SetDateRuoliH2OGenerati(oListRate(x).nIdRuolo, 4, "I", oListRate(x).tDataScadenza.ToShortDateString) = 0 Then
    '                        Return 0
    '                    End If
    '                End If
    '                sNRataPrec = oListRate(x).sNRata
    '            End If
    '        Next
    '        Return True
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsRate.ConfiguraRata.errore: ", Err)
    '        Return False
    '    Finally
    '        If Not WFSessione.oSession Is Nothing Then
    '            WFSessione.Kill()
    '        End If
    '    End Try
    'End Function


    'Public Function SetConfigRata(ByVal oMyRata As ObjConfiguraRata, ByVal nDbOperation As Integer, ByVal WFSessione As OPENUtility.CreateSessione) As Integer
    '    Dim sSQL As String=""
    '    Dim myIdentity As Integer

    '    Try
    '        'costruisco la query
    '        Select Case nDbOperation
    '            Case 0
    '                sSQL = "INSERT INTO TP_RATEENTE"
    '                sSQL += " (IDENTE, IDRUOLO, NRATA, DESCRIZIONERATA, DATASCADENZA)"
    '                sSQL += " VALUES('" & oMyRata.sIdEnte & "'," & oMyRata.nIdRuolo & ",'" & oMyRata.sNRata & "','" & oReplace.ReplaceChar(oMyRata.sDescrRata) & "','" & oReplace.GiraData(oMyRata.tDataScadenza.ToString) & "')"
    '                sSQL += " SELECT @@IDENTITY"
    '                'eseguo la query
    '                Dim dvMyDati As new dataview
    '                dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '                Do While dvMyDati.Read
    '                    myIdentity = myrow(0)
    '                Loop
    '                dvmydati.dispose()
    '            Case 1
    '                sSQL = "UPDATE TP_RATEENTE SET IDENTE='" & oMyRata.sIdEnte & "',"
    '                sSQL += "IDRUOLO=" & oMyRata.nIdRuolo & ","
    '                sSQL += "NRATA='" & oMyRata.sNRata & "',"
    '                sSQL += "DESCRIZIONERATA='" & oReplace.ReplaceChar(oMyRata.sDescrRata) & "',"
    '                sSQL += "DATASCADENZA='" & oReplace.GiraData(oMyRata.tDataScadenza.ToString) & "',"
    '                sSQL += " WHERE (ID=" & oMyRata.Id & ")"
    '                'eseguo la query
    '                If WFSessione.oSession.oAppDB.Execute(sSQL) <> 1 Then
    '                    Return 0
    '                End If
    '                myIdentity = oMyRata.Id
    '            Case 2
    '                sSQL = "DELETE"
    '                sSQL += " FROM TP_RATEENTE"
    '                sSQL += " WHERE (IDRUOLO =" & oMyRata.nIdRuolo & ")"
    '                'eseguo la query
    '                If WFSessione.oSession.oAppDB.Execute(sSQL) < 0 Then
    '                    Return 0
    '                End If
    '                myIdentity = oMyRata.nIdRuolo
    '        End Select

    '        Return myIdentity
    '    Catch Err As Exception
    '        Log.Debug("Si è verificato un errore in ClsRate::SetConfigRata::" & Err.Message & " SQL: " & sSQL)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsRate.SetConfigRata.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nIdRuolo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetConfiguraRata(ByVal nIdRuolo As Integer) As ObjConfiguraRata()
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Dim oRata As ObjConfiguraRata
        Dim oListRate() As ObjConfiguraRata
        Dim nList As Integer = -1

        oListRate = Nothing
        Try
            sSQL = "SELECT *"
            sSQL += " FROM TP_RATEENTE"
            sSQL += " WHERE (IDRUOLO=" & nIdRuolo & ")"
            sSQL += " ORDER BY NRATA, DATASCADENZA"
            'eseguo la query
            dvMyDati = iDB.GetDataView(sSQL)
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    oRata = New ObjConfiguraRata
                    oRata.Id = StringOperation.FormatInt(myRow("id"))
                    oRata.nIdRuolo = StringOperation.FormatInt(myRow("idruolo"))
                    oRata.sIdEnte = StringOperation.FormatString(myRow("idente"))
                    oRata.sNRata = StringOperation.FormatString(myRow("nrata")).ToUpper
                    oRata.sDescrRata = StringOperation.FormatString(myRow("descrizionerata")).ToUpper
                    oRata.tDataScadenza = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("datascadenza")))
                    'ridimensiono l'array
                    nList += 1
                    ReDim Preserve oListRate(nList)
                    oListRate(nList) = oRata
                Next
            End If

            Return oListRate
        Catch Err As Exception
            Log.Debug("Si è verificato un errore in ClsRate::GetConfiguraRata::" & Err.Message & " SQL: " & sSQL)

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsRate.GetConfiguraRata.errore: ", Err)
            Return Nothing
        Finally
            dvMyDati.Dispose()
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oListRate"></param>
    ''' <param name="sError"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ConfiguraRata(ByVal oListRate() As ObjConfiguraRata, ByRef sError As String) As Boolean
        Dim x As Integer
        Dim sNRataPrec As String = ""
        Dim bPresenzaUS As Boolean = False
        Dim FncRuolo As New ClsRuoloH2O

        Try
            For x = 0 To oListRate.GetUpperBound(0)
                If UCase(oListRate(x).sNRata) <> "U" Then
                    If oListRate(x).sNRata <> StringOperation.FormatInt(sNRataPrec) + 1 Then
                        'se è presente blocco l'operazione e do un messaggio
                        sError = "Inserire una Rata consecutiva all\'ultima configurata."
                        Return False
                    ElseIf oListRate(x).sNRata = StringOperation.FormatInt(sNRataPrec) Then
                        'se è presente blocco l'operazione e do un messaggio
                        sError = "Inserire una Rata consecutiva all\'ultima configurata."
                        Return False
                    End If
                Else
                    bPresenzaUS = True
                End If
                sNRataPrec = oListRate(x).sNRata
            Next
            If bPresenzaUS = False Then
                sError = "Inserire l'UNICA SOLUZIONE."
                Return False
            End If
            'elimino le rate precedentemente configurate
            If SetConfigRata(oListRate(0), 2) < 0 Then
                sError = "Errore in inserimento rata."
                Return False
            End If
            'inserisco le nuove rate
            For x = 0 To oListRate.GetUpperBound(0)
                If oListRate(x).sNRata <> "" Then
                    If SetConfigRata(oListRate(x), 0) < 1 Then
                        Return False
                    End If
                    If oListRate(x).sNRata = "U" Then
                        'memorizzo l'unica soluzione sul ruolo
                        If FncRuolo.SetDateRuoliH2OGenerati(oListRate(x).nIdRuolo, 4, "I", oListRate(x).tDataScadenza.ToShortDateString) = 0 Then
                            Return 0
                        End If
                    End If
                    sNRataPrec = oListRate(x).sNRata
                End If
            Next
            Return True
        Catch Err As Exception
            Log.Debug("Si è verificato un errore in ClsRate::ConfiguraRata::" & Err.Message)

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsRate.ConfiguraRata.errore: ", Err)
            Return False
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oMyRata"></param>
    ''' <param name="nDbOperation"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetConfigRata(ByVal oMyRata As ObjConfiguraRata, ByVal nDbOperation As Integer) As Integer
        Dim sSQL As String = ""
        Dim myIdentity As Integer

        Try
            'costruisco la query
            Select Case nDbOperation
                Case 0
                    sSQL = "INSERT INTO TP_RATEENTE"
                    sSQL += " (IDENTE, IDRUOLO, NRATA, DESCRIZIONERATA, DATASCADENZA)"
                    sSQL += " VALUES('" & oMyRata.sIdEnte & "'," & oMyRata.nIdRuolo & ",'" & oMyRata.sNRata & "','" & oReplace.ReplaceChar(oMyRata.sDescrRata) & "','" & oReplace.GiraData(oMyRata.tDataScadenza.ToString) & "')"
                    sSQL += " SELECT @@IDENTITY"
                    'eseguo la query
                    Dim dvMyDati As New DataView
                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            myIdentity = myRow(0)
                        Next
                    End If
                    dvMyDati.Dispose()
                Case 1
                    sSQL = "UPDATE TP_RATEENTE SET IDENTE='" & oMyRata.sIdEnte & "',"
                    sSQL += "IDRUOLO=" & oMyRata.nIdRuolo & ","
                    sSQL += "NRATA='" & oMyRata.sNRata & "',"
                    sSQL += "DESCRIZIONERATA='" & oReplace.ReplaceChar(oMyRata.sDescrRata) & "',"
                    sSQL += "DATASCADENZA='" & oReplace.GiraData(oMyRata.tDataScadenza.ToString) & "',"
                    sSQL += " WHERE (ID=" & oMyRata.Id & ")"
                    'eseguo la query
                    If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                        Return 0
                    End If
                    myIdentity = oMyRata.Id
                Case 2
                    sSQL = "DELETE"
                    sSQL += " FROM TP_RATEENTE"
                    sSQL += " WHERE (IDRUOLO =" & oMyRata.nIdRuolo & ")"
                    'eseguo la query
                    If iDB.ExecuteNonQuery(sSQL) < 0 Then
                        Return 0
                    End If
                    myIdentity = oMyRata.nIdRuolo
            End Select

            Return myIdentity
        Catch Err As Exception
            Log.Debug("Si è verificato un errore in ClsRate::SetConfigRata::" & Err.Message & " SQL: " & sSQL)

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsRate.SetConfigRata.errore: ", Err)
            Return 0
        End Try
    End Function
#End Region

#Region "Rate su Fattura"
    'Public Function GetRata(ByVal nIdFattura As Integer) As ObjRata()
    '    Dim culture As IFormatProvider
    '    culture = New System.Globalization.CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")
    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Dim sSQL, WFErrore As String
    '    Dim dvMyDati As new dataview=nothing
    '    Try
    '        Dim oRata As ObjRata
    '        Dim oListRate() As ObjRata
    '        Dim nList As Integer = -1

    '        'inizializzo la connessione
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If
    '        sSQL = "SELECT *"
    '        sSQL += " FROM TP_FATTURE_RATE"
    '        sSQL += " WHERE (IDFATTURANOTA=" & nIdFattura & ")"
    '        sSQL += " ORDER BY DATA_SCADENZA"
    '        'eseguo la query
    '        dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '        Do While dvMyDati.Read
    '            oRata = New ObjRata
    '            oRata.Id = StringOperation.Formatint(myrow("idrata"))
    '            oRata.nIdFattura = StringOperation.Formatint(myrow("idfatturanota"))
    '            oRata.sIdEnte = StringOperation.Formatstring(myrow("idente"))
    '            oRata.tDataDocumento = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_fattura")))
    '            oRata.sNDocumento = StringOperation.Formatstring(myrow("numero_fattura"))
    '            oRata.nIdUtente = StringOperation.Formatint(myrow("cod_utente"))
    '            oRata.sNRata = StringOperation.Formatstring(myrow("numero_rata"))
    '            oRata.sDescrRata = StringOperation.Formatstring(myrow("descrizione_rata"))
    '            oRata.impRata = StringOperation.Formatdouble(myrow("importo_rata"))
    '            oRata.tDataScadenza = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_scadenza")))
    '            oRata.sCodBollettino = StringOperation.Formatstring(myrow("codice_bollettino"))
    '            oRata.sCodeline = StringOperation.Formatstring(myrow("codeline"))
    '            oRata.sContoCorrente = StringOperation.Formatstring(myrow("numero_conto_corrente"))
    '            If Not IsDBNull(myrow("data_inserimento")) Then
    '                If StringOperation.Formatstring(myrow("data_inserimento")) <> "" Then
    '                    oRata.tDataInserimento = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_inserimento")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("data_variazione")) Then
    '                If StringOperation.Formatstring(myrow("data_variazione")) <> "" Then
    '                    oRata.tDataVariazione = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_variazione")))
    '                End If
    '            End If
    '            If Not IsDBNull(myrow("data_cessazione")) Then
    '                If StringOperation.Formatstring(myrow("data_cessazione")) <> "" Then
    '                    oRata.tDataCessazione = oReplace.GiraDataFromDB(StringOperation.Formatstring(myrow("data_cessazione")))
    '                End If
    '            End If
    '            oRata.sOperatore = StringOperation.Formatstring(myrow("operatore"))
    '            'ridimensiono l'array
    '            nList += 1
    '            ReDim Preserve oListRate(nList)
    '            oListRate(nList) = oRata
    '        Loop

    '        Return oListRate
    '    Catch Err As Exception
    '        Log.Debug("Si è verificato un errore in ClsRate::GetRata::" & Err.Message & " SQL: " & sSQL)
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsRate.GetRata.errore: ", Err)
    '        Return Nothing
    '    Finally
    '        dvmydati.dispose()
    '        WFSessione.Kill()
    '    End Try
    'End Function

    Public Function GetRata(ByVal nIdFattura As Integer) As ObjRata()
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Dim oRata As ObjRata
        Dim oListRate() As ObjRata
        Dim nList As Integer = -1

        oListRate = Nothing
        Try
            sSQL = "SELECT *"
            sSQL += " FROM TP_FATTURE_RATE"
            sSQL += " WHERE (IDFATTURANOTA=" & nIdFattura & ")"
            sSQL += " ORDER BY DATA_SCADENZA"
            'eseguo la query
            dvMyDati = iDB.GetDataView(sSQL)
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    oRata = New ObjRata
                    oRata.Id = StringOperation.FormatInt(myRow("idrata"))
                    oRata.nIdFattura = StringOperation.FormatInt(myRow("idfatturanota"))
                    oRata.sIdEnte = StringOperation.FormatString(myRow("idente"))
                    oRata.tDataDocumento = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_fattura")))
                    oRata.sNDocumento = StringOperation.FormatString(myRow("numero_fattura"))
                    oRata.nIdUtente = StringOperation.FormatInt(myRow("cod_utente"))
                    oRata.sNRata = StringOperation.FormatString(myRow("numero_rata"))
                    oRata.sDescrRata = StringOperation.FormatString(myRow("descrizione_rata"))
                    oRata.impRata = StringOperation.FormatDouble(myRow("importo_rata"))
                    oRata.tDataScadenza = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_scadenza")))
                    oRata.sCodBollettino = StringOperation.FormatString(myRow("codice_bollettino"))
                    oRata.sCodeline = StringOperation.FormatString(myRow("codeline"))
                    oRata.sContoCorrente = StringOperation.FormatString(myRow("numero_conto_corrente"))
                    If StringOperation.FormatString(myRow("data_inserimento")) <> "" Then
                        oRata.tDataInserimento = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_inserimento")))
                    End If
                    If StringOperation.FormatString(myRow("data_variazione")) <> "" Then
                        oRata.tDataVariazione = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_variazione")))
                    End If
                    If StringOperation.FormatString(myRow("data_cessazione")) <> "" Then
                        oRata.tDataCessazione = oReplace.GiraDataFromDB(StringOperation.FormatString(myRow("data_cessazione")))
                    End If
                    oRata.sOperatore = StringOperation.FormatString(myRow("operatore"))
                    'ridimensiono l'array
                    nList += 1
                    ReDim Preserve oListRate(nList)
                    oListRate(nList) = oRata
                Next
            End If

            Return oListRate
        Catch Err As Exception
            Log.Debug("Si è verificato un errore in ClsRate::GetRata::" & Err.Message & " SQL: " & sSQL)

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsRate.GetRata.errore: ", Err)
            Return Nothing
        Finally
            dvMyDati.Dispose()
        End Try
    End Function

    'Public Function CreaRateH2O(ByVal oMyFattura As ObjFattura, ByVal nNumDoc As Integer, ByVal oListRate() As ObjConfiguraRata, ByVal sContoCorrente As String, ByVal WFSessione As OPENUtility.CreateSessione) As Integer
    '    Dim x As Integer
    '    Dim oMyRata As New ObjRata
    '    Dim FncGen As New Generale
    '    Dim impRataArrCent, impRataSingola, impUltimaRata, impRata As Double

    '    Try
    '        'calcolo gli importi delle rate
    '        If oListRate.GetUpperBound(0) <> 0 Then
    '            impRataArrCent = FncGen.ImportoArrotondato(oMyFattura.impFattura / oListRate.GetUpperBound(0))
    '            impRataSingola = FncGen.ImportoArrotondatoEuroIntero((impRataArrCent), 0)
    '            impUltimaRata = oMyFattura.impFattura - (impRataSingola * (oListRate.GetUpperBound(0) - 1))
    '        End If

    '        'genero le rate
    '        For x = 0 To oListRate.GetUpperBound(0)
    '            If oListRate(x).sNRata = "U" Then
    '                impRata = oMyFattura.impFattura
    '            ElseIf oListRate(x).sNRata = oListRate.GetUpperBound(0) Then
    '                impRata = impUltimaRata
    '            Else
    '                impRata = impRataSingola
    '            End If
    '            'creo la rata
    '            oMyRata.nIdFattura = oMyFattura.Id
    '            oMyRata.sIdEnte = oMyFattura.sIdEnte
    '            oMyRata.tDataDocumento = oMyFattura.tDataDocumento
    '            oMyRata.sNDocumento = oMyFattura.sNDocumento
    '            oMyRata.nIdUtente = oMyFattura.nIdUtente
    '            oMyRata.sNRata = oListRate(x).sNRata
    '            oMyRata.sDescrRata = oListRate(x).sDescrRata
    '            oMyRata.impRata = impRata
    '            oMyRata.tDataScadenza = oListRate(x).tDataScadenza
    '            oMyRata.sContoCorrente = sContoCorrente
    '            oMyRata.sCodBollettino = GetCodiceBollettino(oMyFattura.sIdEnte, oMyRata.sNRata, oReplace.GiraData(StringOperation.Formatstring(oMyFattura.tDataDocumento.ToShortDateString)).Substring(0, 4), nNumDoc)
    '            If oMyRata.sCodBollettino = "" Then
    '                Return 0
    '            End If
    '            oMyRata.sCodeline = GetCodeline(oMyRata.sCodBollettino, oMyRata.sContoCorrente, oMyRata.impRata)
    '            If oMyRata.sCodeline = "" Then
    '                Return 0
    '            End If
    '            oMyRata.tDataInserimento = Now
    '            oMyRata.sOperatore = oMyFattura.sOperatore
    '            'aggiorno il db
    '            If SetRata(oMyRata, 0, WFSessione) = 0 Then
    '                Return 0
    '            End If
    '        Next
    '        Return 1
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsRate.CreaRateH2O.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function

    Public Function CreaRateH2O(ByVal oMyFattura As ObjFattura, ByVal nNumDoc As Integer, ByVal oListRate() As ObjConfiguraRata, ByVal sContoCorrente As String) As Integer
        Dim x As Integer
        Dim oMyRata As New ObjRata
        Dim impRataArrCent, impRataSingola, impUltimaRata, impRata As Double

        Try
            'calcolo gli importi delle rate
            If oListRate.GetUpperBound(0) <> 0 Then
                impRataArrCent = New MyUtility().ImportoArrotondato(oMyFattura.impFattura / oListRate.GetUpperBound(0))
                impRataSingola = New MyUtility().ImportoArrotondatoEuroIntero((impRataArrCent), 0)
                impUltimaRata = oMyFattura.impFattura - (impRataSingola * (oListRate.GetUpperBound(0) - 1))
            End If

            'genero le rate
            For x = 0 To oListRate.GetUpperBound(0)
                If oListRate(x).sNRata = "U" Then
                    impRata = oMyFattura.impFattura
                ElseIf oListRate(x).sNRata = oListRate.GetUpperBound(0) Then
                    impRata = impUltimaRata
                Else
                    impRata = impRataSingola
                End If
                'creo la rata
                oMyRata.nIdFattura = oMyFattura.Id
                oMyRata.sIdEnte = oMyFattura.sIdEnte
                oMyRata.tDataDocumento = oMyFattura.tDataDocumento
                oMyRata.sNDocumento = oMyFattura.sNDocumento
                oMyRata.nIdUtente = oMyFattura.nIdUtente
                oMyRata.sNRata = oListRate(x).sNRata
                oMyRata.sDescrRata = oListRate(x).sDescrRata
                oMyRata.impRata = impRata
                oMyRata.tDataScadenza = oListRate(x).tDataScadenza
                oMyRata.sContoCorrente = sContoCorrente
                oMyRata.sCodBollettino = GetCodiceBollettino(oMyFattura.sIdEnte, oMyRata.sNRata, oReplace.GiraData(StringOperation.FormatString(oMyFattura.tDataDocumento.ToShortDateString)).Substring(0, 4), nNumDoc)
                If oMyRata.sCodBollettino = "" Then
                    Return 0
                End If
                oMyRata.sCodeline = GetCodeline(oMyRata.sCodBollettino, oMyRata.sContoCorrente, oMyRata.impRata)
                If oMyRata.sCodeline = "" Then
                    Return 0
                End If
                oMyRata.tDataInserimento = Now
                oMyRata.sOperatore = oMyFattura.sOperatore
                'aggiorno il db
                If SetRata(oMyRata, 0) = 0 Then
                    Return 0
                End If
            Next
            Return 1
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsRate.CreaRateH2O.errore: ", Err)
            Return 0
        End Try
    End Function

    'Public Function SetRata(ByVal oRata As ObjRata, ByVal nDbOperation As Integer, ByVal WFSessione As OPENUtility.CreateSessione) As Integer
    '    Dim culture As IFormatProvider
    '    culture = New System.Globalization.CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")
    '    Dim sSQL As String=""
    '    Try
    '        Dim myIdentity As Integer

    '        'costruisco la query
    '        Select Case nDbOperation
    '            Case 0
    '                sSQL = "INSERT INTO TP_FATTURE_RATE (IDFATTURANOTA, IDENTE, DATA_FATTURA, NUMERO_FATTURA, COD_UTENTE,"
    '                sSQL += " NUMERO_RATA, DESCRIZIONE_RATA, IMPORTO_RATA, DATA_SCADENZA,"
    '                sSQL += " CODICE_BOLLETTINO, CODELINE, NUMERO_CONTO_CORRENTE,"
    '                sSQL += " DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE, OPERATORE)"
    '                sSQL += "VALUES (" & oRata.nIdFattura & ",'" & oRata.sIdEnte & "','" & oReplace.GiraData(oRata.tDataDocumento.ToString) & "','" & oRata.sNDocumento & "'," & oRata.nIdUtente & ","
    '                sSQL += "'" & oRata.sNRata & "','" & oReplace.ReplaceChar(oRata.sDescrRata) & "'," & oReplace.ReplaceNumberForDB(oRata.impRata) & ",'" & oReplace.GiraData(oRata.tDataScadenza.ToString) & "',"
    '                sSQL += "'" & oRata.sCodBollettino & "','" & oRata.sCodeline & "','" & oRata.sContoCorrente & "',"
    '                sSQL += "'" & oReplace.GiraData(oRata.tDataInserimento.ToString) & "',"
    '                If oRata.tDataVariazione <> Date.MinValue Then
    '                    sSQL += "'" & oReplace.GiraData(oRata.tDataVariazione.ToString) & "',"
    '                Else
    '                    sSQL += "NULL,"
    '                End If
    '                If oRata.tDataCessazione <> Date.MinValue Then
    '                    sSQL += "'" & oReplace.GiraData(oRata.tDataCessazione.ToString) & "',"
    '                Else
    '                    sSQL += "NULL,"
    '                End If
    '                sSQL += "'" & oRata.sOperatore & "')"
    '                sSQL += " SELECT @@IDENTITY"
    '                'eseguo la query
    '                Dim dvMyDati As new dataview
    '                dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '                Do While dvMyDati.Read
    '                    myIdentity = myrow(0)
    '                Loop
    '                dvmydati.dispose()
    '            Case 1
    '                sSQL = "UPDATE TP_FATTURE_RATE SET IDENTE='" & oRata.sIdEnte & "',"
    '                sSQL += "IDFATTURANOTA=" & oRata.nIdFattura & ","
    '                sSQL += "DATA_FATTURA='" & oReplace.GiraData(oRata.tDataDocumento.ToString) & "',"
    '                sSQL += "NUMERO_FATTURA='" & oRata.sNDocumento & "',"
    '                sSQL += "COD_UTENTE=" & oRata.nIdUtente & ","
    '                sSQL += "NUMERO_RATA='" & oRata.sNRata & "',"
    '                sSQL += "DESCRIZIONE_RATA='" & oReplace.ReplaceChar(oRata.sDescrRata) & "',"
    '                sSQL += "IMPORTO_RATA=" & oReplace.ReplaceNumberForDB(oRata.impRata) & ","
    '                sSQL += "DATA_SCADENZA='" & oReplace.GiraData(oRata.tDataScadenza.ToString) & "',"
    '                sSQL += "CODICE_BOLLETTINO='" & oRata.sCodBollettino & "',"
    '                sSQL += "CODELINE='" & oRata.sCodeline & "',"
    '                sSQL += "NUMERO_CONTO_CORRENTE='" & oRata.sContoCorrente & "',"
    '                sSQL += "DATA_INSERIMENTO='" & oReplace.GiraData(oRata.tDataInserimento.ToString) & "',"
    '                If oRata.tDataVariazione <> Date.MinValue Then
    '                    sSQL += "DATA_VARIAZIONE='" & oReplace.GiraData(oRata.tDataVariazione.ToString) & "',"
    '                Else
    '                    sSQL += "DATA_VARIAZIONE=NULL,"
    '                End If
    '                If oRata.tDataCessazione <> Date.MinValue Then
    '                    sSQL += "DATA_CESSAZIONE='" & oReplace.GiraData(oRata.tDataCessazione.ToString) & "',"
    '                Else
    '                    sSQL += "DATA_CESSAZIONE=NULL,"
    '                End If
    '                sSQL += "OPERATORE='" & oRata.sOperatore & "'"
    '                sSQL += " WHERE (IDRATA  = " & oRata.Id & ")"
    '                'eseguo la query
    '                If WFSessione.oSession.oAppDB.Execute(sSQL) <> 1 Then
    '                    Return 0
    '                End If
    '                myIdentity = oRata.Id
    '            Case 2
    '                sSQL = "DELETE"
    '                sSQL += " FROM TP_FATTURE_RATE"
    '                sSQL += " WHERE (IDRATA =" & oRata.Id & ")"
    '                'eseguo la query
    '                If WFSessione.oSession.oAppDB.Execute(sSQL) <> 1 Then
    '                    Return 0
    '                End If
    '                myIdentity = oRata.Id
    '        End Select

    '        Return myIdentity
    '    Catch Err As Exception
    '        Log.Debug("Si è verificato un errore in ClsRate::SetRata::" & Err.Message & " SQL: " & sSQL)
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsRate.SetRata.errore: ", Err)
    '        Return 0
    '    End Try
    'End Function

    Public Function SetRata(ByVal oRata As ObjRata, ByVal nDbOperation As Integer) As Integer
        Dim sSQL As String = ""
        Dim myIdentity As Integer

        Try
            'costruisco la query
            Select Case nDbOperation
                Case 0
                    sSQL = "INSERT INTO TP_FATTURE_RATE (IDFATTURANOTA, IDENTE, DATA_FATTURA, NUMERO_FATTURA, COD_UTENTE,"
                    sSQL += " NUMERO_RATA, DESCRIZIONE_RATA, IMPORTO_RATA, DATA_SCADENZA,"
                    sSQL += " CODICE_BOLLETTINO, CODELINE, NUMERO_CONTO_CORRENTE,"
                    sSQL += " DATA_INSERIMENTO, DATA_VARIAZIONE, DATA_CESSAZIONE, OPERATORE)"
                    sSQL += "VALUES (" & oRata.nIdFattura & ",'" & oRata.sIdEnte & "','" & oReplace.GiraData(oRata.tDataDocumento.ToString) & "','" & oRata.sNDocumento & "'," & oRata.nIdUtente & ","
                    sSQL += "'" & oRata.sNRata & "','" & oReplace.ReplaceChar(oRata.sDescrRata) & "'," & oReplace.ReplaceNumberForDB(oRata.impRata) & ",'" & oReplace.GiraData(oRata.tDataScadenza.ToString) & "',"
                    sSQL += "'" & oRata.sCodBollettino & "','" & oRata.sCodeline & "','" & oRata.sContoCorrente & "',"
                    sSQL += "'" & oReplace.GiraData(oRata.tDataInserimento.ToString) & "',"
                    If oRata.tDataVariazione.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                        sSQL += "'" & oReplace.GiraData(oRata.tDataVariazione.ToString) & "',"
                    Else
                        sSQL += "NULL,"
                    End If
                    If oRata.tDataCessazione.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                        sSQL += "'" & oReplace.GiraData(oRata.tDataCessazione.ToString) & "',"
                    Else
                        sSQL += "NULL,"
                    End If
                    sSQL += "'" & oRata.sOperatore & "')"
                    sSQL += " SELECT @@IDENTITY"
                    'eseguo la query
                    Dim dvMyDati As New DataView
                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            myIdentity = myRow(0)
                        Next
                    End If
                    dvMyDati.Dispose()
                Case 1
                    sSQL = "UPDATE TP_FATTURE_RATE SET IDENTE='" & oRata.sIdEnte & "',"
                    sSQL += "IDFATTURANOTA=" & oRata.nIdFattura & ","
                    sSQL += "DATA_FATTURA='" & oReplace.GiraData(oRata.tDataDocumento.ToString) & "',"
                    sSQL += "NUMERO_FATTURA='" & oRata.sNDocumento & "',"
                    sSQL += "COD_UTENTE=" & oRata.nIdUtente & ","
                    sSQL += "NUMERO_RATA='" & oRata.sNRata & "',"
                    sSQL += "DESCRIZIONE_RATA='" & oReplace.ReplaceChar(oRata.sDescrRata) & "',"
                    sSQL += "IMPORTO_RATA=" & oReplace.ReplaceNumberForDB(oRata.impRata) & ","
                    sSQL += "DATA_SCADENZA='" & oReplace.GiraData(oRata.tDataScadenza.ToString) & "',"
                    sSQL += "CODICE_BOLLETTINO='" & oRata.sCodBollettino & "',"
                    sSQL += "CODELINE='" & oRata.sCodeline & "',"
                    sSQL += "NUMERO_CONTO_CORRENTE='" & oRata.sContoCorrente & "',"
                    sSQL += "DATA_INSERIMENTO='" & oReplace.GiraData(oRata.tDataInserimento.ToString) & "',"
                    If oRata.tDataVariazione.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                        sSQL += "DATA_VARIAZIONE='" & oReplace.GiraData(oRata.tDataVariazione.ToString) & "',"
                    Else
                        sSQL += "DATA_VARIAZIONE=NULL,"
                    End If
                    If oRata.tDataCessazione.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                        sSQL += "DATA_CESSAZIONE='" & oReplace.GiraData(oRata.tDataCessazione.ToString) & "',"
                    Else
                        sSQL += "DATA_CESSAZIONE=NULL,"
                    End If
                    sSQL += "OPERATORE='" & oRata.sOperatore & "'"
                    sSQL += " WHERE (IDRATA  = " & oRata.Id & ")"
                    'eseguo la query
                    If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                        Throw New Exception("errore in::" & sSQL)
                    End If
                    myIdentity = oRata.Id
                Case 2
                    sSQL = "DELETE"
                    sSQL += " FROM TP_FATTURE_RATE"
                    sSQL += " WHERE (IDRATA =" & oRata.Id & ")"
                    'eseguo la query
                    If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                        Throw New Exception("errore in::" & sSQL)
                    End If
                    myIdentity = oRata.Id
            End Select

            Return myIdentity
        Catch Err As Exception
            Log.Debug("Si è verificato un errore in ClsRate::SetRata::" & Err.Message & " SQL: " & sSQL)

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsRate.SetRata.errore: ", Err)
            Return 0
        End Try
    End Function

    Public Function GetCodiceBollettino(ByVal sEnte As String, ByVal sNRata As String, ByVal sAnnoDoc As String, ByVal sNDoc As String) As String
        '********************************************************************
        'logica di popolamento del codice bollettino
        '********************************************************************
        '5 chr	codice ente
        '2 chr	numero rata (Unica soluzione = ‘00', Prima rata = '01, Seconda rata = ‘02', ecc…')
        '2 chr	anno data documento(ultimi due caratteri dell'anno)
        '7 chr	numero documento
        '2 chr	caratteri di controllo
        'Algoritmo per la determinazione del CIN
        'n1 = 16 caratteri (determinati in base alla logica stabilita)
        'n2 = Int(n1 / 93) (risultato intero della divione tra dividendo e 93)
        'n3 = n2 * 93 (moltiplico il risultato della divisione per 93)
        'resto = n1 - n3 
        'formatto a due caratteri il resto.
        '********************************************************************
        Try
            Dim sCodiceBollettino As String
            Dim n1, n2, n3 As Double
            Dim cin As Integer

            sCodiceBollettino = sEnte.PadLeft(5, "0")
            If sNRata = "U" Then
                sCodiceBollettino += "00"
            Else
                sCodiceBollettino += sNRata.PadLeft(2, "0")
            End If
            sCodiceBollettino += sAnnoDoc.Substring(2, 2)
            sCodiceBollettino += sNDoc.PadLeft(7, "0")

            n1 = StringOperation.FormatDouble(sCodiceBollettino)
            n2 = Int(n1 / 93)
            n3 = n2 * 93
            cin = n1 - n3

            sCodiceBollettino += StringOperation.FormatString(cin)

            Return sCodiceBollettino
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsRate.GetCodiceBollettino.errore: ", Err)
            Return ""
        End Try
    End Function

    Public Function GetCodeline(ByVal sCodiceBollettino As String, ByVal sContoCorrente As String, ByVal impRata As Double) As String
        '********************************************************************
        'logica di popolamento della codeline
        '********************************************************************
        '<codice bollettino>
        '9:      spazi
        'parte intera dell'importo formattata a 8 caratteri
        '+
        'parte decimale dell'importo
        '>
        'conto corrente formattato a 11 caratteri (utilizzare spazi come riempimento)
        '<
        '2 spazi
        '896:
        '>
        'Esempio:<069920107000000177>         00000096+75>   73682981<  896>
        '********************************************************************
        Try
            Dim sCodeline As String

            sCodeline = "<" & sCodiceBollettino & ">"
            sCodeline += Space(9)
            sCodeline += StringOperation.FormatString(impRata).PadLeft(11, "0").Replace(",", "+").Replace(".", "+") + ">"
            sCodeline += sContoCorrente.PadLeft(11, " ")
            sCodeline += "<" + Space(2) + "896>"
            Return sCodeline
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsRate.GetCodeline.errore: ", Err)
            Return ""
        End Try
    End Function

    'Public Function GetTipologiaDocumento(ByVal TipoRuolo As String) As String
    '    Try
    '        If TipoRuolo = "F" Then
    '            Return "FATTURA"
    '        ElseIf TipoRuolo = "N" Then
    '            Return "NOTA DI CREDITO"
    '        End If
    '    Catch Err As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsRate.GetTipologiaDocumento.errore: ", Err)
    '        Return ""
    '    End Try
    'End Function
#End Region
End Class

