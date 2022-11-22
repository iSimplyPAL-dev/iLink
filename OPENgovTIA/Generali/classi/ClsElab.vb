Imports System.IO
Imports System.Threading
Imports System.Web.Caching
Imports ICSharpCode.SharpZipLib.Zip
Imports log4net
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
Imports Utility
''' <summary>
''' Classe per il calcolo del ruolo.
''' Contiene le funzioni che calcolo i diversi tipi di ruolo.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class ClsElabRuolo
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ClsElabRuolo))
    Private cmdMyCommand As New SqlClient.SqlCommand

#Region "Calcolo Ruolo"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myStringConnection"></param>
    ''' <param name="TipoRuolo"></param>
    ''' <param name="sCheckEnte"></param>
    ''' <param name="sCheckAnno"></param>
    ''' <param name="sErrCheck"></param>
    ''' <returns></returns>
    Public Function CheckTariffe(myStringConnection As String, ByVal TipoRuolo As String, ByVal sCheckEnte As String, ByVal sCheckAnno As String, ByRef sErrCheck As String) As Integer
        Dim MyProcedure As String
        Dim sSQL As String = ""
        Dim myDataView As New DataView
        Dim nMyReturn As Integer = -1

        Try
            If TipoRuolo = ObjRuolo.TipoCalcolo.TARSU Then
                MyProcedure = "prc_CheckTariffe_TARSU"
            Else
                MyProcedure = "prc_CheckTariffe_TARES"
            End If
            Using ctx As New DBModel(ConstSession.DBType, myStringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, MyProcedure, "IdEnte", "Anno")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IdEnte", sCheckEnte) _
                            , ctx.GetParam("Anno", sCheckAnno)
                        )
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsElabRuolo.CheckTariffe.erroreQuery: ", ex)
                    Return 0
                Finally
                    ctx.Dispose()
                End Try
                For Each myRow As DataRowView In myDataView
                    sErrCheck += "-" + myRow("idcategoria").ToString
                Next
            End Using
            If sErrCheck <> "" Then
                Return 0
            Else
                Return 1
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsElabRuolo.CheckTariffe.errore: ", Err)
            Return 0
        Finally
            myDataView.Dispose()
        End Try
    End Function
    'Public Function CheckTariffe(ByVal TipoRuolo As String, ByVal sCheckEnte As String, ByVal sCheckAnno As String, ByRef sErrCheck As String, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As Integer
    '    Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim dtMyDati As New DataTable()
    '    Dim dtMyRow As DataRow
    '    Dim MyProcedure As String

    '    Try
    '        If (Not (cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand = cmdMyCommandOut
    '        Else
    '            cmdMyCommand = New SqlClient.SqlCommand
    '            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure

    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.AddWithValue("@IdEnte", sCheckEnte)
    '        cmdMyCommand.Parameters.AddWithValue("@Anno", sCheckAnno)
    '        If TipoRuolo = ObjRuolo.TipoCalcolo.TARSU Then
    '            MyProcedure = "prc_CheckTariffe_TARSU"
    '        Else
    '            MyProcedure = "prc_CheckTariffe_TARES"
    '        End If
    '        cmdMyCommand.CommandText = MyProcedure
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(dtMyDati)
    '        myAdapter.Dispose()
    '        For Each dtMyRow In dtMyDati.Rows
    '            sErrCheck += "-" + dtMyRow("idcategoria").ToString
    '        Next

    '        If sErrCheck <> "" Then
    '            Return 0
    '        Else
    '            Return 1
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsElabRuolo.CheckTariffe.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return 0
    '    Finally
    '        dtMyDati.Dispose()
    '        If (cmdMyCommandOut Is Nothing) Then
    '            cmdMyCommand.Dispose()
    '            cmdMyCommand.Connection.Close()
    '        End If
    '    End Try
    'End Function
    Public Function CalcolaRuoloFromDich(ByVal myStringConnection As String, ByVal sEnte As String, ByVal nAnno As Integer, ByVal TipoTassazione As String, ByVal TipoCalcolo As String, ByVal sDescrTipoCalcolo As String, ByVal PercentTariffe As Double, ByVal HasMaggiorazione As Boolean, ByVal HasConferimenti As Boolean, ByVal TipoMq As String, ByVal impSogliaAvvisi As Double, ByVal nIdTestata As Integer, ByVal nIdContribuente As Long, tDataInizioConf As DateTime, tDataFineConf As DateTime) As ObjRuolo()
        Dim ListRuoli() As ObjRuolo = Nothing
        Dim myRuolo As ObjRuolo
        Dim FncDich As New ClsDichiarazione
        Dim nList As Integer = -1
        Dim oListAvvisi() As ObjAvviso
        Dim TypeOfRI As Type = GetType(RemotingInterfaceMotoreTarsu.RemotingInterfaceMotoreTarsu.IRuoloTARSUVariabile)
        Dim RemoRuolo As RemotingInterfaceMotoreTarsu.RemotingInterfaceMotoreTarsu.IRuoloTARSUVariabile
        Dim sSQL As String = ""
        Dim myDataView As New DataView
        Dim nMyReturn As Integer = -1

        Try
            Using ctx As New DBModel(ConstSession.DBType, myStringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GETANNICALCOLO", "IDENTE", "ANNO", "IDTESTATA", "IDCONTRIBUENTE")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", sEnte) _
                            , ctx.GetParam("ANNO", nAnno) _
                            , ctx.GetParam("IDTESTATA", nIdTestata) _
                            , ctx.GetParam("IDCONTRIBUENTE", nIdContribuente)
                        )
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsElabRuolo.CalcolaRuoloFromDich.erroreQuery: ", ex)
                    Return Nothing
                Finally
                    ctx.Dispose()
                End Try
                For Each myRow As DataRowView In myDataView
                    'incremento l'oggetto di riepilogo
                    nList += 1
                    ReDim Preserve ListRuoli(nList)
                    myRuolo = New ObjRuolo
                    'richiamo la funzione che genera gli articoli per l'anno e la testata
                    oListAvvisi = FncDich.GetDichPerCalcolo(myStringConnection, sEnte, TipoTassazione, TipoCalcolo, PercentTariffe, HasMaggiorazione, HasConferimenti, TipoMq, myRow("anno"), nIdTestata, nIdContribuente, tDataInizioConf, tDataFineConf, cmdMyCommand)
                    If Not oListAvvisi Is Nothing Then
                        Log.Debug("CalcolaRuoloFromDich::richiamo il servizio::" & ConstSession.UrlServizioRuolo)
                        'se ha trovato record genero gli articoli
                        CacheManager.SetAvanzamentoElaborazione("Calcolo avvisi")
                        'attivo il servizio
                        RemoRuolo = Activator.GetObject(TypeOfRI, ConstSession.UrlServizioRuolo)
                        oListAvvisi = RemoRuolo.CalcolaAvvisi(oListAvvisi)
                        'se ha generato degli avvisi genero il record per il ruolo
                        If Not oListAvvisi Is Nothing Then
                            'aggiungo la riga non calcolata
                            myRuolo.sAnno = myRow("anno").ToString
                            myRuolo.sTipoRuolo = TipoCalcolo 'TipoTassazione
                            myRuolo.sTipoCalcolo = TipoTassazione 'TipoCalcolo
                            myRuolo.sDescrTipoRuolo = sDescrTipoCalcolo
                            myRuolo.PercentTariffe = PercentTariffe
                            myRuolo.TipoMQ = TipoMq
                            myRuolo.HasMaggiorazione = HasMaggiorazione
                            myRuolo.HasConferimenti = HasConferimenti
                            myRuolo.oAvvisi = oListAvvisi
                            myRuolo.nTassazioneMinima = 0
                            myRuolo.ImpMinimo = impSogliaAvvisi
                            If HasConferimenti Then
                                myRuolo.tDataInizioConf = tDataInizioConf
                                myRuolo.tDataFineConf = tDataFineConf
                            Else
                                myRuolo.tDataInizioConf = DateTime.MaxValue
                                myRuolo.tDataFineConf = DateTime.MaxValue
                            End If
                        End If
                    Else
                        'aggiungo la riga non calcolata
                        myRuolo.sAnno = myRow("anno").ToString
                        myRuolo.sNote = "Non calcolato per mancanza configurazione tariffe/articoli."
                    End If
                    ListRuoli(nList) = myRuolo
                Next
            End Using
            Return ListRuoli
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsElabRuolo.CalcolaRuoloFromDich.errore: ", Err)
            Return Nothing
        Finally
            myDataView.Dispose()
        End Try
    End Function
    'Public Function CalcolaRuoloFromDich(ByVal myStringConnection As String, ByVal sEnte As String, ByVal nAnno As Integer, ByVal TipoTassazione As String, ByVal TipoCalcolo As String, ByVal sDescrTipoCalcolo As String, ByVal PercentTariffe As Double, ByVal HasMaggiorazione As Boolean, ByVal HasConferimenti As Boolean, ByVal TipoMq As String, ByVal impSogliaAvvisi As Double, ByVal nIdTestata As Integer, ByVal nIdContribuente As Long, tDataInizioConf As DateTime, tDataFineConf As DateTime) As ObjRuolo()
    '    Dim ListRuoli() As ObjRuolo = Nothing
    '    Dim myRuolo As ObjRuolo
    '    Dim FncDich As New ClsDichiarazione
    '    Dim nList As Integer = -1
    '    Dim oListAvvisi() As ObjAvviso
    '    Dim TypeOfRI As Type = GetType(RemotingInterfaceMotoreTarsu.RemotingInterfaceMotoreTarsu.IRuoloTARSUVariabile)
    '    Dim RemoRuolo As RemotingInterfaceMotoreTarsu.RemotingInterfaceMotoreTarsu.IRuoloTARSUVariabile
    '    'Dim FncTassazione As New ClsTassazione
    '    Dim DsDati As New DataSet
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim dtMyDati As New DataTable()
    '    Dim dtMyRow As DataRow

    '    Try
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure

    '        Log.Debug("CalcolaRuoloFromDich::devo eseguire prc_GETANNICALCOLO")
    '        cmdMyCommand.CommandText = "prc_GETANNICALCOLO"
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdEnte", SqlDbType.VarChar)).Value = sEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Anno", SqlDbType.Int)).Value = nAnno
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTESTATA", SqlDbType.Int)).Value = nIdTestata
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = nIdContribuente
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(dtMyDati)
    '        myAdapter.Dispose()
    '        For Each dtMyRow In dtMyDati.Rows
    '            'incremento l'oggetto di riepilogo
    '            nList += 1
    '            ReDim Preserve ListRuoli(nList)
    '            myRuolo = New ObjRuolo
    '            'richiamo la funzione che genera gli articoli per l'anno e la testata
    '            oListAvvisi = FncDich.GetDichPerCalcolo(myStringConnection, sEnte, TipoTassazione, TipoCalcolo, PercentTariffe, HasMaggiorazione, HasConferimenti, TipoMq, dtMyRow("anno"), nIdTestata, nIdContribuente, tDataInizioConf, tDataFineConf, cmdMyCommand)
    '            If Not oListAvvisi Is Nothing Then
    '                Log.Debug("CalcolaRuoloFromDich::richiamo il servizio::" & ConstSession.UrlServizioRuolo)
    '                'se ha trovato record genero gli articoli
    '                CacheManager.SetAvanzamentoElaborazione("Calcolo avvisi")
    '                'attivo il servizio
    '                RemoRuolo = Activator.GetObject(TypeOfRI, ConstSession.UrlServizioRuolo)
    '                oListAvvisi = RemoRuolo.CalcolaAvvisi(oListAvvisi)
    '                'se ha generato degli avvisi genero il record per il ruolo
    '                If Not oListAvvisi Is Nothing Then
    '                    'aggiungo la riga non calcolata
    '                    myRuolo.sAnno = dtMyRow("anno").ToString
    '                    myRuolo.sTipoRuolo = TipoCalcolo 'TipoTassazione
    '                    myRuolo.sTipoCalcolo = TipoTassazione 'TipoCalcolo
    '                    myRuolo.sDescrTipoRuolo = sDescrTipoCalcolo
    '                    myRuolo.PercentTariffe = PercentTariffe
    '                    myRuolo.TipoMQ = TipoMq
    '                    myRuolo.HasMaggiorazione = HasMaggiorazione
    '                    myRuolo.HasConferimenti = HasConferimenti
    '                    myRuolo.oAvvisi = oListAvvisi
    '                    myRuolo.nTassazioneMinima = 0
    '                    myRuolo.ImpMinimo = impSogliaAvvisi
    '                    If HasConferimenti Then
    '                        myRuolo.tDataInizioConf = tDataInizioConf
    '                        myRuolo.tDataFineConf = tDataFineConf
    '                    Else
    '                        myRuolo.tDataInizioConf = DateTime.MaxValue
    '                        myRuolo.tDataFineConf = DateTime.MaxValue
    '                    End If
    '                End If
    '            Else
    '                'aggiungo la riga non calcolata
    '                myRuolo.sAnno = dtMyRow("anno").ToString
    '                myRuolo.sNote = "Non calcolato per mancanza configurazione tariffe/articoli."
    '            End If
    '            ListRuoli(nList) = myRuolo
    '        Next

    '        Return ListRuoli
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsElabRuolo.CalcolaRuoloFromDich.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return Nothing
    '    Finally
    '        dtMyDati.Dispose()
    '        cmdMyCommand.Dispose()
    '        cmdMyCommand.Connection.Close()
    '    End Try
    'End Function
    '*** ***
    '*** 201809 Bollettazione Vigliano in OPENgov***
    Public Function ValorizzaTotRuolo(ByVal TipoTassazione As String, ByVal IdEnte As String, ByVal sGenTipoElab As String, ByVal sGenDescTipoRuolo As String, ByVal oMyRiepilogo As ObjRuolo) As ObjRuolo
        Dim oMyTotRuolo As New ObjRuolo
        Dim FncRuolo As New ClsGestRuolo

        Try
            oMyTotRuolo = oMyRiepilogo
            oMyTotRuolo.IdFlusso = -1
            oMyTotRuolo.sEnte = IdEnte
            oMyTotRuolo.tDataCreazione = Now
            If oMyTotRuolo.nNumeroRuolo <= 0 Then
                oMyTotRuolo.nNumeroRuolo = FncRuolo.GetNumeroRuolo(IdEnte, oMyRiepilogo.sTipoCalcolo, oMyRiepilogo.sAnno)
            End If
            If oMyRiepilogo.sNomeRuolo = "" Then
                oMyTotRuolo.sNomeRuolo = FncRuolo.GetNameRuolo(IdEnte, oMyRiepilogo.sAnno, oMyRiepilogo.sTipoCalcolo, Now)
            End If
            '*** 20131104 - TARES ***
            If oMyRiepilogo.sDescrRuolo = "" Then
                oMyTotRuolo.sDescrRuolo = FncRuolo.GetDescrizioneRuolo(TipoTassazione, sGenDescTipoRuolo, sGenTipoElab, oMyRiepilogo.nNumeroRuolo, oMyRiepilogo.sAnno)
            End If
            '*** ***

            Return oMyTotRuolo
        Catch Err As Exception
            Log.Debug(IdEnte + " - OPENgovTIA.ClsElabRuolo.ValorizzaTotRuolo.errore: ", Err)
            Log.Debug("Si è verificato un errore in CalcolaTotRuolo::" & Err.Message)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oRuoloCalcolato"></param>
    ''' <returns></returns>
    Public Function CheckSogliaRuolo(ByVal oRuoloCalcolato As ObjRuolo) As ObjRuolo
        Try
            Dim y As Integer
            Dim oNewRuolo As New ObjRuolo
            Dim nNewRc As Integer = -1
            Dim oNewListAvvisi() As ObjAvviso = Nothing
            Dim nUtenti, ContribPrec, nAvvisi, nArticoli, nScarti As Integer
            Dim nMQ, impAvvisi, impPF, impPV, impPM, impPC, impRid, impDet, impMinimo As Double

            nAvvisi = 0
            If Not oRuoloCalcolato.oAvvisi Is Nothing Then
                For Each myItem As ObjAvviso In oRuoloCalcolato.oAvvisi
                    impMinimo = oRuoloCalcolato.ImpMinimo
                    '*** 20140630 - SUPPLETIVO/CONGUAGLIO ***
                    'a SUPPLETIVO/CONGUAGLIO devono uscire solo le posizioni sopra soglia perchè tanto ho già emesso sgravio per i negativi
                    'quindi se la soglia è 0,00 la si forza a 0,1
                    If oRuoloCalcolato.ImpMinimo = 0 And (oRuoloCalcolato.sTipoCalcolo = ObjRuolo.Ruolo.AConguaglio) Then
                        impMinimo = 0.1 'uso una variabile così non rimane sporco quanto registrato in ruolo
                    End If
                    If ValRuoloPerSoglia(impMinimo, oRuoloCalcolato.nTassazioneMinima, myItem, nNewRc, oNewListAvvisi, nScarti) = 0 Then
                        Log.Debug("CheckSogliaRuolo::errore in ValRuoloPerSoglia per posizione::" & myItem.IdContribuente.ToString + "|" + myItem.impDovuto.ToString)
                        Return Nothing
                    End If
                    '*** ***
                Next
                'calcolo i totalizzatori sugli avvisi
                If Not oNewListAvvisi Is Nothing Then
                    nAvvisi = oNewListAvvisi.GetUpperBound(0) + 1
                    For Each myItem As ObjAvviso In oNewListAvvisi
                        If myItem.IdContribuente <> ContribPrec Then
                            nUtenti += 1
                        End If
                        impAvvisi += myItem.impTotale
                        If Not IsNothing(myItem.oArticoli) Then
                            For y = 0 To myItem.oArticoli.GetUpperBound(0)
                                nArticoli += 1
                                Select Case myItem.oArticoli(y).TipoPartita
                                    Case ObjArticolo.PARTEFISSA
                                        impPF += myItem.oArticoli(y).impNetto
                                        nMQ += myItem.oArticoli(y).nMQ
                                    Case ObjArticolo.PARTEVARIABILE
                                        impPV += myItem.oArticoli(y).impNetto
                                    Case ObjArticolo.PARTEMAGGIORAZIONE
                                        impPM += myItem.oArticoli(y).impNetto
                                    Case ObjArticolo.PARTECONFERIMENTI
                                        impPC += myItem.oArticoli(y).impNetto
                                End Select
                                impRid += myItem.oArticoli(y).impRiduzione
                                impDet += myItem.oArticoli(y).impDetassazione
                            Next
                        End If
                    Next
                Else
                    'se le note non sono valorizzate è perchè non ho calcolato articoli da supplettivo
                    If oRuoloCalcolato.sNote = "" Then
                        oRuoloCalcolato.sNote = "Ruolo non calcolato perchè non sono subentrare variazioni o perché sotto soglia."
                    End If
                End If
            Else
                'se le note non sono valorizzate è perchè non ho calcolato articoli da supplettivo
                If oRuoloCalcolato.sNote = "" Then
                    oRuoloCalcolato.sNote = "Ruolo non calcolato perchè non sono subentrare variazioni o perché sotto soglia."
                End If
            End If
            'creo il nuovo oggetto totalizzatore
            oNewRuolo.IdFlusso = oRuoloCalcolato.IdFlusso
            oNewRuolo.ImpPF = impPF
            oNewRuolo.ImpPV = impPV
            oNewRuolo.impPM = impPM
            oNewRuolo.impPC = impPC
            oNewRuolo.ImpNetto = impAvvisi
            oNewRuolo.ImpAvvisi = impAvvisi
            oNewRuolo.ImpDetassazione = impDet
            oNewRuolo.ImpRiduzione = impRid
            oNewRuolo.nArticoli = nArticoli
            oNewRuolo.nAvvisi = nAvvisi
            oNewRuolo.nContribuenti = nUtenti
            oNewRuolo.nMQ = nMQ
            oNewRuolo.ImpMinimo = oRuoloCalcolato.ImpMinimo
            oNewRuolo.nNumeroRuolo = oRuoloCalcolato.nNumeroRuolo
            oNewRuolo.nTassazioneMinima = oRuoloCalcolato.nTassazioneMinima
            oNewRuolo.oAvvisi = oNewListAvvisi
            oNewRuolo.sAnno = oRuoloCalcolato.sAnno
            oNewRuolo.sDescrTipoRuolo = oRuoloCalcolato.sDescrTipoRuolo
            oNewRuolo.sNote = oRuoloCalcolato.sNote
            oNewRuolo.sTipoRuolo = oRuoloCalcolato.sTipoRuolo
            oNewRuolo.sTipoCalcolo = oRuoloCalcolato.sTipoCalcolo
            oNewRuolo.tDataStampaMinuta = oRuoloCalcolato.tDataStampaMinuta
            oNewRuolo.nScarti = nScarti
            oNewRuolo.PercentTariffe = oRuoloCalcolato.PercentTariffe
            oNewRuolo.TipoMQ = oRuoloCalcolato.TipoMQ
            oNewRuolo.HasMaggiorazione = oRuoloCalcolato.HasMaggiorazione
            oNewRuolo.HasConferimenti = oRuoloCalcolato.HasConferimenti
            oNewRuolo.tDataInizioConf = oRuoloCalcolato.tDataInizioConf
            oNewRuolo.tDataFineConf = oRuoloCalcolato.tDataFineConf

            Return oNewRuolo
        Catch Err As Exception
            Log.Debug(oRuoloCalcolato.sEnte + " - OPENgovTIA.ClsElabRuolo.CheckSogliaRuolo.errore: ", Err)
            Return Nothing
        End Try
    End Function
    'Public Function CheckSogliaRuolo(ByVal oRuoloCalcolato As ObjRuolo) As ObjRuolo
    '    Try
    '        Dim x, y As Integer
    '        Dim oNewRuolo As New ObjRuolo
    '        Dim nNewRc As Integer = -1
    '        Dim oNewListAvvisi() As ObjAvviso = Nothing
    '        Dim nUtenti, ContribPrec, nArticoli, nScarti As Integer
    '        Dim nMQ, impAvvisi, impPF, impPV, impPM, impPC, impRid, impDet, impMinimo As Double

    '        If Not oRuoloCalcolato.oAvvisi Is Nothing Then
    '            For x = 0 To oRuoloCalcolato.oAvvisi.GetUpperBound(0)
    '                impMinimo = oRuoloCalcolato.ImpMinimo
    '                '*** 20140630 - SUPPLETIVO/CONGUAGLIO ***
    '                'a SUPPLETIVO/CONGUAGLIO devono uscire solo le posizioni sopra soglia perchè tanto ho già emesso sgravio per i negativi
    '                'quindi se la soglia è 0,00 la si forza a 0,1
    '                If oRuoloCalcolato.ImpMinimo = 0 And (oRuoloCalcolato.sTipoCalcolo = ObjRuolo.Ruolo.AConguaglio Or oRuoloCalcolato.sTipoCalcolo = ObjRuolo.Ruolo.Supplettivo) Then
    '                    impMinimo = 0.1 'uso una variabile così non rimane sporco quanto registrato in ruolo
    '                End If
    '                If ValRuoloPerSoglia(impMinimo, oRuoloCalcolato.nTassazioneMinima, oRuoloCalcolato.oAvvisi(x), nNewRc, oNewListAvvisi, nScarti) = 0 Then
    '                    Log.Debug("CheckSogliaRuolo::errore in ValRuoloPerSoglia per posizione::" & x)
    '                    Return Nothing
    '                End If
    '                '*** ***
    '            Next
    '            'calcolo i totalizzatori sugli avvisi
    '            For x = 0 To oNewListAvvisi.GetUpperBound(0)
    '                If oNewListAvvisi(x).IdContribuente <> ContribPrec Then
    '                    nUtenti += 1
    '                End If
    '                impAvvisi += oNewListAvvisi(x).impTotale
    '                If Not IsNothing(oNewListAvvisi(x).oArticoli) Then
    '                    For y = 0 To oNewListAvvisi(x).oArticoli.GetUpperBound(0)
    '                        nArticoli += 1
    '                        Select Case oNewListAvvisi(x).oArticoli(y).TipoPartita
    '                            Case ObjArticolo.PARTEFISSA
    '                                impPF += oNewListAvvisi(x).oArticoli(y).impNetto
    '                                nMQ += oNewListAvvisi(x).oArticoli(y).nMQ
    '                            Case ObjArticolo.PARTEVARIABILE
    '                                impPV += oNewListAvvisi(x).oArticoli(y).impNetto
    '                            Case ObjArticolo.PARTEMAGGIORAZIONE
    '                                impPM += oNewListAvvisi(x).oArticoli(y).impNetto
    '                            Case ObjArticolo.PARTECONFERIMENTI
    '                                impPC += oNewListAvvisi(x).oArticoli(y).impNetto
    '                        End Select
    '                        impRid += oNewListAvvisi(x).oArticoli(y).impRiduzione
    '                        impDet += oNewListAvvisi(x).oArticoli(y).impDetassazione
    '                    Next
    '                End If
    '            Next
    '        Else
    '            'se le note non sono valorizzate è perchè non ho calcolato articoli da supplettivo
    '            If oRuoloCalcolato.sNote = "" Then
    '                oRuoloCalcolato.sNote = "Ruolo non calcolato perchè non sono subentrare variazioni."
    '            End If
    '        End If
    '        'creo il nuovo oggetto totalizzatore
    '        oNewRuolo.IdFlusso = oRuoloCalcolato.IdFlusso
    '        oNewRuolo.ImpPF = impPF
    '        oNewRuolo.ImpPV = impPV
    '        oNewRuolo.impPM = impPM
    '        oNewRuolo.impPC = impPC
    '        oNewRuolo.ImpAvvisi = impAvvisi
    '        oNewRuolo.ImpDetassazione = impDet
    '        oNewRuolo.ImpRiduzione = impRid
    '        oNewRuolo.nArticoli = nArticoli
    '        oNewRuolo.nAvvisi = oNewListAvvisi.GetUpperBound(0) + 1
    '        oNewRuolo.nContribuenti = nUtenti
    '        oNewRuolo.nMQ = nMQ
    '        oNewRuolo.ImpMinimo = oRuoloCalcolato.ImpMinimo
    '        oNewRuolo.nNumeroRuolo = oRuoloCalcolato.nNumeroRuolo
    '        oNewRuolo.nTassazioneMinima = oRuoloCalcolato.nTassazioneMinima
    '        oNewRuolo.oAvvisi = oNewListAvvisi
    '        oNewRuolo.sAnno = oRuoloCalcolato.sAnno
    '        oNewRuolo.sDescrTipoRuolo = oRuoloCalcolato.sDescrTipoRuolo
    '        oNewRuolo.sNote = oRuoloCalcolato.sNote
    '        oNewRuolo.sTipoRuolo = oRuoloCalcolato.sTipoRuolo
    '        oNewRuolo.sTipoCalcolo = oRuoloCalcolato.sTipoCalcolo
    '        oNewRuolo.tDataStampaMinuta = oRuoloCalcolato.tDataStampaMinuta
    '        oNewRuolo.nScarti = nScarti
    '        oNewRuolo.PercentTariffe = oRuoloCalcolato.PercentTariffe
    '        oNewRuolo.TipoMQ = oRuoloCalcolato.TipoMQ
    '        oNewRuolo.HasMaggiorazione = oRuoloCalcolato.HasMaggiorazione
    '        oNewRuolo.HasConferimenti = oRuoloCalcolato.HasConferimenti
    '        oNewRuolo.tDataInizioConf = oRuoloCalcolato.tDataInizioConf
    '        oNewRuolo.tDataFineConf = oRuoloCalcolato.tDataFineConf

    '        Return oNewRuolo
    '    Catch Err As Exception
    '        Log.Debug(oRuoloCalcolato.sEnte + " - OPENgovTIA.ClsElabRuolo.CheckSogliaRuolo.errore: ", Err)
    '        Return Nothing
    '    End Try
    'End Function
    '*** ***
    '*** 20140630 - SUPPLETIVO/CONGUAGLIO ***
    Private Function ValRuoloPerSoglia(ByVal impMinimo As Double, ByVal nTassazioneMinima As Integer, ByVal oMyAvviso As ObjAvviso, ByRef nNewRc As Integer, ByRef oNewListAvvisi() As ObjAvviso, ByRef nScarti As Integer) As Integer
        Dim y As Integer
        Dim FncArticolo As New GestArticolo

        Try
            'devo confrontare il dovuto
            If oMyAvviso.impDovuto < impMinimo Then 'If oMyAvviso.impTotale < impMinimo Then 'If FormatNumber(oMyAvviso.impTotale, 2) < FormatNumber(impMinimo, 2) Then
                'se è presente tassazione minima devo aggiornare tutti gli articoli
                If nTassazioneMinima = 1 Then
                    For y = 0 To oMyAvviso.oArticoli.GetUpperBound(0)
                        oMyAvviso.oArticoli(y).bIsImportoForzato = True
                        oMyAvviso.oArticoli(y).impNetto = impMinimo
                    Next
                    'aggiorno il nuovo insieme di articoli
                    nNewRc += 1
                    ReDim Preserve oNewListAvvisi(nNewRc)
                    oNewListAvvisi(nNewRc) = oMyAvviso
                Else
                    nScarti += 1
                    ''inserisco gli articoli negli scarti
                    Log.Debug("ValRuoloPerSoglia::SCARTO::" & oMyAvviso.IdEnte & "|" & oMyAvviso.sCognome & " " & oMyAvviso.sNome & "|" & oMyAvviso.impTotale & "|" & oMyAvviso.impDovuto & "|" & impMinimo)
                End If
            Else
                'aggiorno il nuovo insieme di articoli
                nNewRc += 1
                ReDim Preserve oNewListAvvisi(nNewRc)
                oNewListAvvisi(nNewRc) = oMyAvviso
            End If
            Return 1
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsElabRuolo.ValRuoloPerSoglia.errore: ", Err)
            Return 0
        End Try
    End Function
#End Region
#Region "Cartellazione"
    Public Function CartellaRuolo(DBType As String, myConnectionString As String, ByVal oRuolo As ObjRuolo) As ObjRuolo
        Dim TypeOfRI As Type = GetType(RemotingInterfaceMotoreTarsu.RemotingInterfaceMotoreTarsu.IRuoloTARSUVariabile)
        Dim RemoRuolo As RemotingInterfaceMotoreTarsu.RemotingInterfaceMotoreTarsu.IRuoloTARSUVariabile
        Dim oMyRuolo As New ObjRuolo
        Dim oListAvvisi() As ObjAvviso
        Dim oLottoCartellazione As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.ObjLottoCartellazione
        Dim FncDB As New DichManagerTARSU(DBType, myConnectionString, "", oMyRuolo.sEnte)
        Dim FncLotto As New GestLottoCartellazione
        Dim FncAddiz As New GestAddizionali
        Dim impTotale, impAddizionali As Double

        Try
            impTotale = 0 : impAddizionali = 0
            oMyRuolo = oRuolo
            'prelevo le eventuali addizionali da calcolare
            oMyRuolo.oAddizionali = FncAddiz.GetAddizionale(myConnectionString, oMyRuolo.sEnte, oMyRuolo.sAnno, "", oMyRuolo.sTipoRuolo)
            'determino il lotto di cartellazione
            oLottoCartellazione = FncLotto.GetLottoCartellazione(myConnectionString, oMyRuolo.sAnno, "100", oMyRuolo.sEnte, oMyRuolo.oAvvisi.Length)
            If Not oLottoCartellazione Is Nothing Then
                'attivo il servizio
                RemoRuolo = Activator.GetObject(TypeOfRI, ConstSession.UrlServizioRuolo)
                'eseguo la creazione degli articoli
                oListAvvisi = RemoRuolo.CartellaAvvisi(oMyRuolo, oLottoCartellazione)
                'se ha cartellato aggiorno il record per il ruolo
                If Not oListAvvisi Is Nothing Then
                    'ciclo sugli avvisi cartellati
                    For Each myAvviso As ObjAvviso In oListAvvisi
                        'registro l'avviso nella tabella
                        If FncDB.SetCartella(myAvviso) = 0 Then
                            oMyRuolo.sNote = "Errore in inserimento Cartellazione"
                        End If
                        impTotale += myAvviso.impCarico
                        For Each myDet As ObjDetVoci In myAvviso.oDetVoci
                            Select Case myDet.sCapitolo
                                Case ObjDetVoci.Capitolo.AggioEnte, ObjDetVoci.Capitolo.ECA, ObjDetVoci.Capitolo.MECA, ObjDetVoci.Capitolo.Provinciale, ObjDetVoci.Capitolo.ProvincialeEnte
                                    impAddizionali += myDet.impDettaglio
                                Case Else
                            End Select
                        Next
                    Next
                    oMyRuolo.oAvvisi = oListAvvisi
                    'inserisco il lotto di cartellazione
                    If FncLotto.SetLottoCartellazione(myConnectionString, oLottoCartellazione) < 1 Then
                        oMyRuolo.sNote = "Errore in inserimento Lotto Cartellazione"
                    End If
                Else
                    oMyRuolo.sNote = "Errore in calcolo Cartellazione"
                End If
                oMyRuolo.ImpAvvisi = impTotale
                oMyRuolo.impAddizionali = impAddizionali
            Else
                oMyRuolo.sNote = "Errore in reperimento Lotto Cartellazione"
            End If

            Return oMyRuolo
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsElabRuolo.CartellaRuolo.errore: ", Err)
            Return Nothing
        End Try
    End Function
    'Public Function CartellaRuolo(DBType As String, myConnectionString As String, ByVal oRuolo As ObjRuolo, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As ObjRuolo
    '    Dim TypeOfRI As Type = GetType(RemotingInterfaceMotoreTarsu.RemotingInterfaceMotoreTarsu.IRuoloTARSUVariabile)
    '    Dim RemoRuolo As RemotingInterfaceMotoreTarsu.RemotingInterfaceMotoreTarsu.IRuoloTARSUVariabile
    '    Dim oMyRuolo As New ObjRuolo
    '    Dim oListAvvisi() As ObjAvviso
    '    Dim oLottoCartellazione As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.ObjLottoCartellazione
    '    Dim x As Integer
    '    Dim FncDB As New DichManagerTARSU(DBType, myConnectionString, "", oMyRuolo.sEnte)
    '    Dim FncLotto As New GestLottoCartellazione
    '    Dim FncAddiz As New GestAddizionali
    '    Dim impTotale As Double
    '    Dim cmdMyCommand As SqlClient.SqlCommand = Nothing

    '    Try
    '        If (Not (cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand = cmdMyCommandOut
    '        Else
    '            cmdMyCommand = New SqlClient.SqlCommand
    '            cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
    '            cmdMyCommand.Connection.Open()
    '            cmdMyCommand.CommandTimeout = 0
    '        End If
    '        oMyRuolo = oRuolo
    '        'prelevo le eventuali addizionali da calcolare
    '        oMyRuolo.oAddizionali = FncAddiz.GetAddizionale(myConnectionString, oMyRuolo.sEnte, oMyRuolo.sAnno, "", oMyRuolo.sTipoRuolo, cmdMyCommand)
    '        'determino il lotto di cartellazione
    '        oLottoCartellazione = FncLotto.GetLottoCartellazione(oMyRuolo.sAnno, "100", oMyRuolo.sEnte, oMyRuolo.oAvvisi.Length, cmdMyCommand)
    '        If Not oLottoCartellazione Is Nothing Then
    '            'attivo il servizio
    '            RemoRuolo = Activator.GetObject(TypeOfRI, ConstSession.UrlServizioRuolo)
    '            'eseguo la creazione degli articoli
    '            oListAvvisi = RemoRuolo.CartellaAvvisi(oMyRuolo, oLottoCartellazione)
    '            'se ha cartellato aggiorno il record per il ruolo
    '            If Not oListAvvisi Is Nothing Then
    '                'ciclo sugli avvisi cartellati
    '                For x = 0 To oListAvvisi.GetUpperBound(0)
    '                    'registro l'avviso nella tabella
    '                    If FncDB.SetCartella(oListAvvisi(x)) = 0 Then
    '                        oMyRuolo.sNote = "Errore in inserimento Cartellazione"
    '                    End If
    '                    impTotale += oListAvvisi(x).impCarico
    '                Next
    '                oMyRuolo.oAvvisi = oListAvvisi
    '                'inserisco il lotto di cartellazione
    '                If FncLotto.SetLottoCartellazione(oLottoCartellazione, cmdMyCommand) < 1 Then
    '                    oMyRuolo.sNote = "Errore in inserimento Lotto Cartellazione"
    '                End If
    '            Else
    '                oMyRuolo.sNote = "Errore in calcolo Cartellazione"
    '            End If
    '            oMyRuolo.ImpAvvisi = impTotale
    '        Else
    '            oMyRuolo.sNote = "Errore in reperimento Lotto Cartellazione"
    '        End If

    '        Return oMyRuolo
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsElabRuolo.CartellaRuolo.errore: ", Err)
    '        Return Nothing
    '    Finally
    '        If (cmdMyCommandOut Is Nothing) Then
    '            cmdMyCommand.Dispose()
    '            cmdMyCommand.Connection.Close()
    '        End If
    '    End Try
    'End Function
    Public Function CalcoloRate(myStringConnection As String, ByVal oRuolo As ObjRuolo, ByVal IdAvviso As Integer) As ObjRuolo
        Dim sSQL As String = ""
        Dim myDataView As New DataView
        Dim nMyReturn As Integer = -1

        Try
            Using ctx As New DBModel(ConstSession.DBType, myStringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_SetRateDettaglio", "ID", "IDFLUSSO", "IDAVVISO", "TRIBUTO", "SOGLIARATA", "SOGLIABOLLETTINO")

                    ''sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_SetRateDettaglio_old", "ID", "IDFLUSSO", "IDAVVISO", "TRIBUTO", "SOGLIARATA", "SOGLIABOLLETTINO")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("ID", 0) _
                            , ctx.GetParam("IDFLUSSO", oRuolo.IdFlusso) _
                            , ctx.GetParam("IDAVVISO", IdAvviso) _
                            , ctx.GetParam("TRIBUTO", ConstSession.TributoTARESF24) _
                            , ctx.GetParam("SOGLIARATA", 0) _
                            , ctx.GetParam("SOGLIABOLLETTINO", 0)
                        )

                    ''myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("ID", 0) _
                    ''        , ctx.GetParam("IDFLUSSO", oRuolo.IdFlusso) _
                    ''        , ctx.GetParam("IDAVVISO", IdAvviso) _
                    ''        , ctx.GetParam("TRIBUTO", ConstSession.TributoTARESF24) _
                    ''        , ctx.GetParam("SOGLIARATA", oRuolo.oRate(0).impSoglia) _
                    ''        , ctx.GetParam("SOGLIABOLLETTINO", 0)
                    ''    )


                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsElabRuolo.CalcoloRate.SetRateDettaglio.errore: ", ex)
                    nMyReturn = -1
                Finally
                    ctx.Dispose()
                End Try
                For Each myRow As DataRowView In myDataView
                    nMyReturn = Utility.StringOperation.FormatInt(myRow("id"))
                Next
                If nMyReturn <= 0 Then
                    oRuolo = Nothing
                Else
                    Try
                        sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_SetRate", "ID", "IDFLUSSO", "OPERATORE", "IDAVVISO")
                        myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("ID", 0) _
                            , ctx.GetParam("IDFLUSSO", oRuolo.IdFlusso) _
                            , ctx.GetParam("OPERATORE", ConstSession.UserName) _
                            , ctx.GetParam("IDAVVISO", IdAvviso)
                        )
                    Catch ex As Exception
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsElabRuolo.CalcoloRate.prc_SetRate.errore: ", ex)
                        nMyReturn = -1
                    Finally
                        ctx.Dispose()
                    End Try
                    For Each myRow As DataRowView In myDataView
                        nMyReturn = Utility.StringOperation.FormatInt(myRow("id"))
                    Next
                    If nMyReturn <= 0 Then
                        oRuolo = Nothing
                    End If
                End If
            End Using
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsElabRuolo.CalcoloRate.errore: ", Err)
            nMyReturn = 0
            oRuolo.sNote = "Errore in CalcoloRate::" & Err.Message
        Finally
            myDataView.Dispose()
        End Try
        Return oRuolo
    End Function
    'Public Function CalcoloRate(ByVal oRuolo As ObjRuolo, ByVal IdAvviso As Integer) As ObjRuolo
    '    'Dim MyDBEngine As DBEngine = Nothing
    '    'Dim MyParamCollection As IDataParameterCollection
    '    'Dim MyParam As IDataParameter
    '    'Dim MyProcedure As String = "prc_CalcoloRate"
    '    Dim nMyReturn As Integer

    '    Try
    '        cmdMyCommand = New SqlClient.SqlCommand
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        'Valorizzo la connessione
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        cmdMyCommand.CommandTimeout = 0
    '        If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = 0
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSO", SqlDbType.Int)).Value = oRuolo.IdFlusso
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDAVVISO", SqlDbType.Int)).Value = IdAvviso
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TRIBUTO", SqlDbType.NVarChar)).Value = ConstSession.TributoTARESF24
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SOGLIARATA", SqlDbType.Float)).Value = 0
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SOGLIABOLLETTINO", SqlDbType.Float)).Value = 0
    '        'Valorizzo il commandtext:
    '        cmdMyCommand.CommandText = "prc_SetRateDettaglio"
    '        cmdMyCommand.Parameters("@ID").Direction = ParameterDirection.InputOutput
    '        'eseguo la query
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        cmdMyCommand.ExecuteNonQuery()
    '        nMyReturn = cmdMyCommand.Parameters("@ID").Value
    '        If nMyReturn <= 0 Then
    '            oRuolo = Nothing
    '        Else
    '            'valorizzo i parameters:
    '            cmdMyCommand.Parameters.Clear()
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = 0
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSO", SqlDbType.Int)).Value = oRuolo.IdFlusso
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@OPERATORE", SqlDbType.NVarChar)).Value = ConstSession.UserName
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDAVVISO", SqlDbType.Int)).Value = IdAvviso
    '            'Valorizzo il commandtext:
    '            cmdMyCommand.CommandText = "prc_SetRate"
    '            cmdMyCommand.Parameters("@ID").Direction = ParameterDirection.InputOutput
    '            'eseguo la query
    '            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '            cmdMyCommand.ExecuteNonQuery()
    '            nMyReturn = cmdMyCommand.Parameters("@ID").Value
    '            If nMyReturn <= 0 Then
    '                oRuolo = Nothing
    '            End If
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsElabRuolo.CalcoloRate.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        oRuolo.sNote = "Errore in CalcoloRate::" & Err.Message
    '    Finally
    '        cmdMyCommand.Connection.Close()
    '    End Try

    '    'Try
    '    '    MyDBEngine = Utility.DBEngineFactory.GetDBEngine(ConstSession.StringConnection)
    '    '    MyDBEngine.OpenConnection()

    '    '    MyDBEngine.ClearParameters()
    '    '    MyDBEngine.AddParameter("@ID", 0, ParameterDirection.InputOutput)
    '    '    MyDBEngine.AddParameter("@IDFLUSSO", oRuolo.IdFlusso, ParameterDirection.Input)
    '    '    'MyDBEngine.AddParameter("@DBOPERATION", 4, ParameterDirection.Input) 'parametro per il calcolo da stored
    '    '    MyDBEngine.AddParameter("@SOGLIARATA", 0, ParameterDirection.Input)
    '    '    MyDBEngine.AddParameter("@SOGLIABOLLETTINO", 0, ParameterDirection.Input)
    '    '    MyDBEngine.AddParameter("@TRIBUTO", ConstSession.TributoTARESF24, ParameterDirection.Input)
    '    '    MyDBEngine.AddParameter("@IDAVVISO", idavviso, ParameterDirection.Input)
    '    '    MyProcedure = "prc_SetRateDettaglio"
    '    '    MyParamCollection = MyDBEngine.ExecuteNonQuery(MyProcedure, CommandType.StoredProcedure)
    '    '    MyParam = CType(MyParamCollection("@ID"), IDataParameter)
    '    '    If stringoperation.formatint(MyParam.Value) <= 0 Then
    '    '        oRuolo = Nothing
    '    '    Else
    '    '        MyDBEngine.ClearParameters()
    '    '        MyDBEngine.AddParameter("@ID", 0, ParameterDirection.InputOutput)
    '    '        MyDBEngine.AddParameter("@IDFLUSSO", oRuolo.IdFlusso, ParameterDirection.Input)
    '    '        'MyDBEngine.AddParameter("@DBOPERATION", 4, ParameterDirection.Input) 'parametro per il calcolo da stored
    '    '        MyDBEngine.AddParameter("@OPERATORE", ConstSession.UserName, ParameterDirection.Input)
    '    '        MyDBEngine.AddParameter("@IDAVVISO", IdAvviso, ParameterDirection.Input)
    '    '        MyProcedure = "prc_SetRate"
    '    '        MyParamCollection = MyDBEngine.ExecuteNonQuery(MyProcedure, CommandType.StoredProcedure)
    '    '        MyParam = CType(MyParamCollection("@ID"), IDataParameter)
    '    '        If stringoperation.formatint(MyParam.Value) <= 0 Then
    '    '            oRuolo = Nothing
    '    '        End If
    '    '    End If
    '    'Catch Err As Exception
    '    '    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsElabRuolo.CalcolorRate.errore: ", Err)
    '    '    oRuolo.sNote = "Errore in CalcoloRate::" & Err.Message
    '    'Finally
    '    '    MyDBEngine.CloseConnection()
    '    'End Try
    '    Return oRuolo
    'End Function
    '*** ***
#End Region
    Public Function DeleteRuolo(DBType As String, ByVal myStringConnection As String, ByVal myStringConnectionGOV As String, IsFromVariabile As String, ByVal oMyRuolo As ObjRuolo) As Boolean
        Dim FncLotto As New GestLottoCartellazione
        Dim FncAvviso As New GestAvviso
        Dim FncRuolo As New ClsGestRuolo

        Try
            'elimino il lotto di cartellazione
            If FncLotto.DeleteLottoCartellazione(myStringConnection, oMyRuolo.sEnte, oMyRuolo.IdFlusso) < 0 Then
                Return False
            End If
            If FncAvviso.DeleteAvvisoCompleto(DBType, myStringConnection, myStringConnectionGOV, IsFromVariabile, oMyRuolo.sEnte, oMyRuolo.IdFlusso) < 1 Then
                Return False
            End If
            If FncRuolo.SetRuolo(Utility.Costanti.AZIONE_DELETE, oMyRuolo, myStringConnection) < 1 Then
                Return False
            End If
            Return True
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsElabRuolo.DeleteRuolo.errore: ", Err)
            Return False
        End Try
    End Function
#Region "Ricalcolo Sgravi"
    Public Function RicalcoloAvviso(myConnectionString As String, ByVal oListArticoli() As ObjArticolo, ByRef sScript As String, ByVal sTipoCalcolo As String, ByVal bHasMaggiorazione As Boolean, ByVal bHasConferimenti As Boolean, TipoRuolo As String) As Boolean
        Dim TypeOfRI As Type = GetType(RemotingInterfaceMotoreTarsu.RemotingInterfaceMotoreTarsu.IRuoloTARSUVariabile)
        Dim RemoRuolo As RemotingInterfaceMotoreTarsu.RemotingInterfaceMotoreTarsu.IRuoloTARSUVariabile
        Dim MyAvviso As ObjAvviso

        Try
            MyAvviso = HttpContext.Current.Session("oMyAvviso")
            '*** 20140701 - IMU/TARES ***
            If MyAvviso Is Nothing Then
                MyAvviso = New ObjAvviso
                MyAvviso.IdContribuente = oListArticoli(0).IdContribuente
                MyAvviso.sAnnoRiferimento = oListArticoli(0).sAnno
            End If
            '*** ***
            'ORDINO PER partita+categoria+nc+forza pv
            Array.Sort(oListArticoli, New Utility.Comparatore(New String() {"TipoPartita", "sCategoria", "nComponenti", "nComponentiPV", "bForzaPV", "sOperatore", "tDataInizio", "tDataFine"}, New Boolean() {Utility.TipoOrdinamento.Crescente, Utility.TipoOrdinamento.Crescente, Utility.TipoOrdinamento.Crescente, Utility.TipoOrdinamento.Crescente, Utility.TipoOrdinamento.Crescente, Utility.TipoOrdinamento.Decrescente, Utility.TipoOrdinamento.Crescente, Utility.TipoOrdinamento.Crescente}))
            Dim myArticolo As New ObjArticolo
            Dim myPrecForPV As New ObjArticolo
            Dim myPrecForPM As New ObjArticolo
            Dim CurrentItem As New ObjUnitaImmobiliare
            Dim nMQ, nMQPM As Double
            Dim newArticoli As New ArrayList
            Dim ListPartitePrec As New ArrayList

            For Each myArticolo In oListArticoli
                '*** 20140630 - SUPPLETIVO/CONGUAGLIO ***
                If myArticolo.sVia = ObjArticolo.PARTEPRECEMESSO_DESCR Then
                    ListPartitePrec.Add(myArticolo)
                Else
                    'se non sono su variabile ricarico da oggetto
                    Select Case myArticolo.TipoPartita
                        Case ObjArticolo.PARTEFISSA
                            If myPrecForPV.Id > 0 Then
                                If sTipoCalcolo = "TARES" Then
                                    If CheckGenPV(myConnectionString, sTipoCalcolo, myPrecForPV, myArticolo, nMQ, nMQPM, newArticoli) = False Then
                                        sScript = "GestAlert('a', 'danger', '', '', 'ATTENZIONE! Errore nel calcolo dell\'importo.');"
                                        Return False
                                    End If
                                End If
                                If bHasMaggiorazione Then
                                    If CheckGenPM(myConnectionString, sTipoCalcolo, myPrecForPM, myArticolo, nMQPM, newArticoli) = False Then
                                        sScript = "GestAlert('a', 'danger', '', '', 'ATTENZIONE! Errore nel calcolo dell\'importo.');"
                                        Return False
                                    End If
                                End If
                            End If
                            nMQ += myArticolo.nMQ
                            nMQPM += myArticolo.nMQ
                            CurrentItem = GeneraPartita(myConnectionString, myArticolo, 0, sTipoCalcolo, Nothing, Nothing)
                            If Not CurrentItem Is Nothing Then
                                newArticoli.Add(CurrentItem)
                            Else
                                sScript = "GestAlert('a', 'danger', '', '', 'ATTENZIONE! Errore nel calcolo dell\'importo.');"
                                Return False
                            End If
                            'non ho generato new pv allora eventualmente associo rid prec a rid articolo altrimenti me le perdo
                            myPrecForPM = myArticolo
                            Dim myArticoloForPV As New ObjArticolo
                            myArticoloForPV.bForzaPV = myArticolo.bForzaPV
                            myArticoloForPV.bIsImportoForzato = myArticolo.bIsImportoForzato
                            myArticoloForPV.bIsTarsuGiornaliera = myArticolo.bIsTarsuGiornaliera
                            myArticoloForPV.Id = myArticolo.Id
                            myArticoloForPV.IdArticolo = myArticolo.IdArticolo
                            myArticoloForPV.IdAvviso = myArticolo.IdAvviso
                            myArticoloForPV.IdContribuente = myArticolo.IdContribuente
                            myArticoloForPV.IdDettaglioTestata = myArticolo.IdDettaglioTestata
                            myArticoloForPV.IdEnte = myArticolo.IdEnte
                            myArticoloForPV.impDetassazione = myArticolo.impDetassazione
                            myArticoloForPV.impNetto = myArticolo.impNetto
                            myArticoloForPV.impRiduzione = myArticolo.impRiduzione
                            myArticoloForPV.impRuolo = myArticolo.impRuolo
                            myArticoloForPV.impTariffa = myArticolo.impTariffa
                            myArticoloForPV.nBimestri = myArticolo.nBimestri
                            myArticoloForPV.nCodVia = myArticolo.nCodVia
                            myArticoloForPV.nComponenti = myArticolo.nComponenti
                            myArticoloForPV.nComponentiPV = myArticolo.nComponentiPV
                            myArticoloForPV.nIdAssenzaDatiCatastali = myArticolo.nIdAssenzaDatiCatastali
                            myArticoloForPV.nIdDestUso = myArticolo.nIdDestUso
                            myArticoloForPV.nIdFlussoRuolo = myArticolo.nIdFlussoRuolo
                            myArticoloForPV.nIdNaturaOccupaz = myArticolo.nIdNaturaOccupaz
                            myArticoloForPV.nIdTariffa = myArticolo.nIdTariffa
                            myArticoloForPV.nIdTitoloOccupaz = myArticolo.nIdTitoloOccupaz
                            myArticoloForPV.nMQ = myArticolo.nMQ
                            myArticoloForPV.oDetassazioni = myArticolo.oDetassazioni
                            myArticoloForPV.oRiduzioni = myArticolo.oRiduzioni
                            myArticoloForPV.sAnno = myArticolo.sAnno
                            myArticoloForPV.sCategoria = myArticolo.sCategoria
                            myArticoloForPV.sCivico = myArticolo.sCivico
                            myArticoloForPV.sDescrCategoria = myArticolo.sDescrCategoria
                            myArticoloForPV.sEsponente = myArticolo.sEsponente
                            myArticoloForPV.sEstensioneParticella = myArticolo.sEstensioneParticella
                            myArticoloForPV.sFoglio = myArticolo.sFoglio
                            myArticoloForPV.sIdTipoParticella = myArticolo.sIdTipoParticella
                            myArticoloForPV.sIdTipoUnita = myArticolo.sIdTipoUnita
                            myArticoloForPV.sInterno = myArticolo.sInterno
                            myArticoloForPV.sNote = myArticolo.sNote
                            myArticoloForPV.sNumero = myArticolo.sNumero
                            myArticoloForPV.sOperatore = myArticolo.sOperatore
                            myArticoloForPV.sScala = myArticolo.sScala
                            myArticoloForPV.sSezione = myArticolo.sSezione
                            myArticoloForPV.sSubalterno = myArticolo.sSubalterno
                            myArticoloForPV.sTipoRuolo = myArticolo.sTipoRuolo
                            myArticoloForPV.sVia = myArticolo.sVia
                            myArticoloForPV.tDataCessazione = myArticolo.tDataCessazione
                            myArticoloForPV.tDataFine = myArticolo.tDataFine
                            myArticoloForPV.tDataInizio = myArticolo.tDataInizio
                            myArticoloForPV.tDataInserimento = myArticolo.tDataInserimento
                            myArticoloForPV.tDataVariazione = myArticolo.tDataVariazione
                            myArticoloForPV.TipoPartita = myArticolo.TipoPartita
                            'BD 04/10/2021
                            myArticoloForPV.ImportoFissoRid = myArticolo.ImportoFissoRid
                            'BD 04/10/2021
                            If myPrecForPV.Id > 0 And (myArticolo.sCategoria = myPrecForPV.sCategoria And myArticolo.nComponentiPV = myPrecForPV.nComponentiPV And myArticolo.bForzaPV = False And myArticolo.sDescrCategoria.ToUpper.StartsWith("DOM") And myArticolo.nBimestri = myPrecForPV.nBimestri) Then
                                'assegno la data maggiore altrimento me la perdo
                                If myPrecForPV.tDataFine > myArticolo.tDataFine Then
                                    myArticoloForPV.tDataFine = myPrecForPV.tDataFine
                                End If
                                myArticoloForPV.oRiduzioni = myPrecForPV.oRiduzioni
                                myArticoloForPV.oDetassazioni = myPrecForPV.oDetassazioni
                            End If
                            '*** 20141211 - legami PF-PV ***
                            Dim myPFvsPV As New Generic.List(Of ObjLegamePFPV)
                            If Not myPrecForPV.ListPFvsPV Is Nothing Then
                                For Each myObj As ObjLegamePFPV In myPrecForPV.ListPFvsPV
                                    myPFvsPV.Add(myObj)
                                Next
                            End If
                            Dim myLegame As New ObjLegamePFPV
                            myLegame.IdPF = myArticolo.Id
                            myPFvsPV.Add(myLegame)
                            myArticoloForPV.ListPFvsPV = myPFvsPV.ToArray
                            '*** ***
                            myPrecForPV = myArticoloForPV
                        Case ObjArticolo.PARTECONFERIMENTI
                            If bHasConferimenti = True Then
                                CurrentItem = GeneraPartita(myConnectionString, myArticolo, 0, sTipoCalcolo, Nothing, Nothing)
                                If Not CurrentItem Is Nothing Then
                                    newArticoli.Add(CurrentItem)
                                Else
                                    sScript = "GestAlert('a', 'danger', '', '', 'ATTENZIONE! Errore nel calcolo dell\'importo.');"
                                    Return False
                                End If
                            End If
                    End Select
                End If
                '*** ***
            Next
            If sTipoCalcolo = "TARES" Then
                'devo creare nuova pv x ultime pf passate
                CurrentItem = GeneraPV(myConnectionString, myPrecForPV, nMQ, sTipoCalcolo)
                If Not CurrentItem Is Nothing Then
                    newArticoli.Add(CurrentItem)
                Else
                    sScript = "GestAlert('a', 'danger', '', '', 'ATTENZIONE! Errore nel calcolo dell\'importo.');"
                    Return False
                End If
            End If
            If bHasMaggiorazione Then
                'devo creare nuova pm x ultime pf passate
                CurrentItem = GeneraPM(myConnectionString, myPrecForPM, nMQPM, sTipoCalcolo)
                If Not CurrentItem Is Nothing Then
                    newArticoli.Add(CurrentItem)
                Else
                    sScript = "GestAlert('a', 'danger', '', '', 'ATTENZIONE! Errore nel calcolo dell\'importo.');"
                    Return False
                End If
            End If
            '*** 20140630 - SUPPLETIVO/CONGUAGLIO ***
            MyAvviso.oArticoliPrec = CType(ListPartitePrec.ToArray(GetType(ObjArticolo)), ObjArticolo())
            '*** ***
            nMQPM = 0
            MyAvviso.oUI = CType(newArticoli.ToArray(GetType(ObjUnitaImmobiliare)), ObjUnitaImmobiliare())
            MyAvviso.oArticoli = oListArticoli
            'attivo il servizio
            RemoRuolo = Activator.GetObject(TypeOfRI, ConstSession.UrlServizioRuolo)
            Dim oListAvvisi(0) As ObjAvviso
            oListAvvisi(0) = MyAvviso
            oListAvvisi = RemoRuolo.CalcolaAvvisi(oListAvvisi)
            'se ha generato degli avvisi genero il record per il ruolo
            If oListAvvisi Is Nothing Then
                sScript = "GestAlert('a', 'danger', '', '', 'ATTENZIONE! Errore nel calcolo dell\'importo.');"
                Return False
            End If
            oListAvvisi(0).ID = MyAvviso.ID
            oListAvvisi(0).IdFlussoRuolo = MyAvviso.IdFlussoRuolo
            oListAvvisi(0).nLottoCartellazione = MyAvviso.nLottoCartellazione
            oListAvvisi(0).oTessere = MyAvviso.oTessere
            oListAvvisi(0).sCodiceCartella = MyAvviso.sCodiceCartella
            oListAvvisi(0).tDataEmissione = MyAvviso.tDataEmissione
            oListAvvisi(0).impPagato = MyAvviso.impPagato
            oListAvvisi(0).impSaldo = FormatNumber(MyAvviso.impCarico - MyAvviso.impPagato, 2)
            'devo settare debito/credito prec uguale a pagato e dovuto al netto di pagato per avere le rate ricalcolateper il solo debito residuo
            oListAvvisi(0).impCreditoDebitoPrec = MyAvviso.impPagato
            oListAvvisi(0).impDovuto = FormatNumber(MyAvviso.impCarico - MyAvviso.impPagato, 2)
            oListAvvisi(0).oPagamenti = MyAvviso.oPagamenti
            'devo riprendere ncomponentipv
            For Each myArticolo In oListAvvisi(0).oArticoli
                For Each myPrecForPV In MyAvviso.oArticoli
                    If myArticolo.Id = myPrecForPV.Id And myArticolo.Id > 0 Then
                        myArticolo.nComponentiPV = myPrecForPV.nComponentiPV
                        myArticolo.bForzaPV = myPrecForPV.bForzaPV
                        myArticolo.oRiduzioni = myPrecForPV.oRiduzioni
                        myArticolo.oDetassazioni = myPrecForPV.oDetassazioni
                        '*** 20141211 - legami PF-PV ***
                        myArticolo.IdOggetto = myPrecForPV.IdOggetto
                        '*** ***
                    End If
                Next
            Next
            MyAvviso = oListAvvisi(0)
            'verifico la presenza delle addizionali
            Dim FncAddiz As New GestAddizionali
            Dim oAddizionali() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale
            oAddizionali = FncAddiz.GetAddizionale(ConstSession.StringConnection, ConstSession.IdEnte, MyAvviso.sAnnoRiferimento, "", TipoRuolo)
            If oAddizionali Is Nothing Then
                sScript = "GestAlert('a', 'warning', '', '', 'Non sono state configurate le addizionali!Impossibile proseguire!');"
                Return False
            End If
            'calcolo le addizionali
            Log.Debug("devo calcolare det voci")
            MyAvviso.oDetVoci = RemoRuolo.CalcoloDetVoci(oAddizionali, MyAvviso)
            HttpContext.Current.Session.Remove("MyAvvisoRicalcolato")
            HttpContext.Current.Session("MyAvvisoRicalcolato") = MyAvviso
            HttpContext.Current.Session("oListArticoli") = MyAvviso.oArticoli
            HttpContext.Current.Session("oListArticoliSgravi") = MyAvviso.oArticoli
            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsElabRuolo.RicalcoloAvviso.errore: ", ex)
            Return False
        End Try
    End Function
    'Public Function RicalcoloAvviso(ByVal oListArticoli() As ObjArticolo, ByRef sScript As String, ByVal sTipoCalcolo As String, ByVal bHasMaggiorazione As Boolean, ByVal bHasConferimenti As Boolean, TipoRuolo As String) As Boolean
    '    Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
    '    Dim TypeOfRI As Type = GetType(RemotingInterfaceMotoreTarsu.RemotingInterfaceMotoreTarsu.IRuoloTARSUVariabile)
    '    Dim RemoRuolo As RemotingInterfaceMotoreTarsu.RemotingInterfaceMotoreTarsu.IRuoloTARSUVariabile
    '    Dim MyAvviso As ObjAvviso

    '    Try
    '        MyAvviso = HttpContext.Current.Session("oMyAvviso")
    '        '*** 20140701 - IMU/TARES ***
    '        If MyAvviso Is Nothing Then
    '            MyAvviso = New ObjAvviso
    '            MyAvviso.IdContribuente = oListArticoli(0).IdContribuente
    '            MyAvviso.sAnnoRiferimento = oListArticoli(0).sAnno
    '        End If
    '        '*** ***
    '        'ORDINO PER partita+categoria+nc+forza pv
    '        Array.Sort(oListArticoli, New Utility.Comparatore(New String() {"TipoPartita", "sCategoria", "nComponenti", "nComponentiPV", "bForzaPV", "sOperatore", "tDataInizio", "tDataFine"}, New Boolean() {Utility.TipoOrdinamento.Crescente, Utility.TipoOrdinamento.Crescente, Utility.TipoOrdinamento.Crescente, Utility.TipoOrdinamento.Crescente, Utility.TipoOrdinamento.Crescente, Utility.TipoOrdinamento.Decrescente, Utility.TipoOrdinamento.Crescente, Utility.TipoOrdinamento.Crescente}))
    '        Dim myArticolo As New ObjArticolo
    '        Dim myPrecForPV As New ObjArticolo
    '        Dim myPrecForPM As New ObjArticolo
    '        Dim CurrentItem As New ObjUnitaImmobiliare
    '        Dim nMQ, nMQPM As Double
    '        Dim newArticoli As New ArrayList
    '        Dim ListPartitePrec As New ArrayList

    '        For Each myArticolo In oListArticoli
    '            '*** 20140630 - SUPPLETIVO/CONGUAGLIO ***
    '            If myArticolo.sVia = ObjArticolo.PARTEPRECEMESSO_DESCR Then
    '                ListPartitePrec.Add(myArticolo)
    '            Else
    '                'se non sono su variabile ricarico da oggetto
    '                Select Case myArticolo.TipoPartita
    '                    Case ObjArticolo.PARTEFISSA
    '                        If myPrecForPV.Id > 0 Then
    '                            If sTipoCalcolo = "TARES" Then
    '                                If CheckGenPV(sTipoCalcolo, myPrecForPV, myArticolo, nMQ, nMQPM, newArticoli) = False Then
    '                                    sScript = "GestAlert('a', 'danger', '', '', 'ATTENZIONE! Errore nel calcolo dell\'importo.');"
    '                                    Return False
    '                                End If
    '                            End If
    '                            If bHasMaggiorazione Then
    '                                If CheckGenPM(sTipoCalcolo, myPrecForPM, myArticolo, nMQPM, newArticoli) = False Then
    '                                    sScript = "GestAlert('a', 'danger', '', '', 'ATTENZIONE! Errore nel calcolo dell\'importo.');"
    '                                    Return False
    '                                End If
    '                            End If
    '                        End If
    '                        nMQ += myArticolo.nMQ
    '                        nMQPM += myArticolo.nMQ
    '                        'If myArticolo.Id = oArticoloSgravi.Id Then
    '                        '    CurrentItem = GeneraPartita(myArticolo, 0, stipocalcolo, oArticoloSgravi.oRiduzioni, oArticoloSgravi.oDetassazioni)
    '                        'Else
    '                        CurrentItem = GeneraPartita(myArticolo, 0, sTipoCalcolo, Nothing, Nothing)
    '                        'End If
    '                        If Not CurrentItem Is Nothing Then
    '                            newArticoli.Add(CurrentItem)
    '                        Else
    '                            sScript = "GestAlert('a', 'danger', '', '', 'ATTENZIONE! Errore nel calcolo dell\'importo.');"
    '                            Return False
    '                        End If
    '                        'non ho generato new pv allora eventualmente associo rid prec a rid articolo altrimenti me le perdo
    '                        myPrecForPM = myArticolo
    '                        Dim myArticoloForPV As New ObjArticolo
    '                        'myArticoloForPV = myArticolo
    '                        myArticoloForPV.bForzaPV = myArticolo.bForzaPV
    '                        myArticoloForPV.bIsImportoForzato = myArticolo.bIsImportoForzato
    '                        myArticoloForPV.bIsTarsuGiornaliera = myArticolo.bIsTarsuGiornaliera
    '                        myArticoloForPV.Id = myArticolo.Id
    '                        myArticoloForPV.IdArticolo = myArticolo.IdArticolo
    '                        myArticoloForPV.IdAvviso = myArticolo.IdAvviso
    '                        myArticoloForPV.IdContribuente = myArticolo.IdContribuente
    '                        myArticoloForPV.IdDettaglioTestata = myArticolo.IdDettaglioTestata
    '                        myArticoloForPV.IdEnte = myArticolo.IdEnte
    '                        myArticoloForPV.impDetassazione = myArticolo.impDetassazione
    '                        myArticoloForPV.impNetto = myArticolo.impNetto
    '                        myArticoloForPV.impRiduzione = myArticolo.impRiduzione
    '                        myArticoloForPV.impRuolo = myArticolo.impRuolo
    '                        myArticoloForPV.impTariffa = myArticolo.impTariffa
    '                        myArticoloForPV.nBimestri = myArticolo.nBimestri
    '                        myArticoloForPV.nCodVia = myArticolo.nCodVia
    '                        myArticoloForPV.nComponenti = myArticolo.nComponenti
    '                        myArticoloForPV.nComponentiPV = myArticolo.nComponentiPV
    '                        myArticoloForPV.nIdAssenzaDatiCatastali = myArticolo.nIdAssenzaDatiCatastali
    '                        myArticoloForPV.nIdDestUso = myArticolo.nIdDestUso
    '                        myArticoloForPV.nIdFlussoRuolo = myArticolo.nIdFlussoRuolo
    '                        myArticoloForPV.nIdNaturaOccupaz = myArticolo.nIdNaturaOccupaz
    '                        myArticoloForPV.nIdTariffa = myArticolo.nIdTariffa
    '                        myArticoloForPV.nIdTitoloOccupaz = myArticolo.nIdTitoloOccupaz
    '                        myArticoloForPV.nMQ = myArticolo.nMQ
    '                        myArticoloForPV.oDetassazioni = myArticolo.oDetassazioni
    '                        myArticoloForPV.oRiduzioni = myArticolo.oRiduzioni
    '                        myArticoloForPV.sAnno = myArticolo.sAnno
    '                        myArticoloForPV.sCategoria = myArticolo.sCategoria
    '                        myArticoloForPV.sCivico = myArticolo.sCivico
    '                        myArticoloForPV.sDescrCategoria = myArticolo.sDescrCategoria
    '                        myArticoloForPV.sEsponente = myArticolo.sEsponente
    '                        myArticoloForPV.sEstensioneParticella = myArticolo.sEstensioneParticella
    '                        myArticoloForPV.sFoglio = myArticolo.sFoglio
    '                        myArticoloForPV.sIdTipoParticella = myArticolo.sIdTipoParticella
    '                        myArticoloForPV.sIdTipoUnita = myArticolo.sIdTipoUnita
    '                        myArticoloForPV.sInterno = myArticolo.sInterno
    '                        myArticoloForPV.sNote = myArticolo.sNote
    '                        myArticoloForPV.sNumero = myArticolo.sNumero
    '                        myArticoloForPV.sOperatore = myArticolo.sOperatore
    '                        myArticoloForPV.sScala = myArticolo.sScala
    '                        myArticoloForPV.sSezione = myArticolo.sSezione
    '                        myArticoloForPV.sSubalterno = myArticolo.sSubalterno
    '                        myArticoloForPV.sTipoRuolo = myArticolo.sTipoRuolo
    '                        myArticoloForPV.sVia = myArticolo.sVia
    '                        myArticoloForPV.tDataCessazione = myArticolo.tDataCessazione
    '                        myArticoloForPV.tDataFine = myArticolo.tDataFine
    '                        myArticoloForPV.tDataInizio = myArticolo.tDataInizio
    '                        myArticoloForPV.tDataInserimento = myArticolo.tDataInserimento
    '                        myArticoloForPV.tDataVariazione = myArticolo.tDataVariazione
    '                        myArticoloForPV.TipoPartita = myArticolo.TipoPartita
    '                        If myPrecForPV.Id > 0 And (myArticolo.sCategoria = myPrecForPV.sCategoria And myArticolo.nComponentiPV = myPrecForPV.nComponentiPV And myArticolo.bForzaPV = False And myArticolo.sDescrCategoria.ToUpper.StartsWith("DOM") And myArticolo.nBimestri = myPrecForPV.nBimestri) Then
    '                            'assegno la data maggiore altrimento me la perdo
    '                            If myPrecForPV.tDataFine > myArticolo.tDataFine Then
    '                                myArticoloForPV.tDataFine = myPrecForPV.tDataFine
    '                            End If
    '                            myArticoloForPV.oRiduzioni = myPrecForPV.oRiduzioni
    '                            myArticoloForPV.oDetassazioni = myPrecForPV.oDetassazioni
    '                            'Else
    '                            '    myPrecForPV = myArticolo
    '                        End If
    '                        '*** 20141211 - legami PF-PV ***
    '                        Dim myPFvsPV As New Generic.List(Of ObjLegamePFPV)
    '                        If Not myPrecForPV.ListPFvsPV Is Nothing Then
    '                            For Each myObj As ObjLegamePFPV In myPrecForPV.ListPFvsPV
    '                                myPFvsPV.Add(myObj)
    '                            Next
    '                        End If
    '                        Dim myLegame As New ObjLegamePFPV
    '                        myLegame.IdPF = myArticolo.Id
    '                        myPFvsPV.Add(myLegame)
    '                        myArticoloForPV.ListPFvsPV = myPFvsPV.ToArray
    '                        '*** ***
    '                        myPrecForPV = myArticoloForPV

    '                    Case ObjArticolo.PARTECONFERIMENTI
    '                        If bHasConferimenti = True Then
    '                            CurrentItem = GeneraPartita(myArticolo, 0, sTipoCalcolo, Nothing, Nothing)
    '                            If Not CurrentItem Is Nothing Then
    '                                newArticoli.Add(CurrentItem)
    '                            Else
    '                                sScript = "GestAlert('a', 'danger', '', '', 'ATTENZIONE! Errore nel calcolo dell\'importo.');"
    '                                Return False
    '                            End If
    '                        End If
    '                End Select
    '            End If
    '            '*** ***
    '        Next
    '        If sTipoCalcolo = "TARES" Then
    '            'devo creare nuova pv x ultime pf passate
    '            'CurrentItem = GeneraPV(myPrec, nMQ, stipocalcolo, oArticoloSgravi.oRiduzioni, oArticoloSgravi.oDetassazioni)
    '            CurrentItem = GeneraPV(myPrecForPV, nMQ, sTipoCalcolo)
    '            If Not CurrentItem Is Nothing Then
    '                newArticoli.Add(CurrentItem)
    '            Else
    '                sScript = "GestAlert('a', 'danger', '', '', 'ATTENZIONE! Errore nel calcolo dell\'importo.');"
    '                Return False
    '            End If
    '        End If
    '        If bHasMaggiorazione Then
    '            'devo creare nuova pm x ultime pf passate
    '            'CurrentItem = GeneraPM(myPrec, nMQPM, stipocalcolo, oArticoloSgravi.oRiduzioni, oArticoloSgravi.oDetassazioni)
    '            CurrentItem = GeneraPM(myPrecForPM, nMQPM, sTipoCalcolo)
    '            If Not CurrentItem Is Nothing Then
    '                newArticoli.Add(CurrentItem)
    '            Else
    '                sScript = "GestAlert('a', 'danger', '', '', 'ATTENZIONE! Errore nel calcolo dell\'importo.');"
    '                Return False
    '            End If
    '        End If
    '        '*** 20140630 - SUPPLETIVO/CONGUAGLIO ***
    '        MyAvviso.oArticoliPrec = CType(ListPartitePrec.ToArray(GetType(ObjArticolo)), ObjArticolo())
    '        '*** ***
    '        nMQPM = 0
    '        MyAvviso.oUI = CType(newArticoli.ToArray(GetType(ObjUnitaImmobiliare)), ObjUnitaImmobiliare())
    '        MyAvviso.oArticoli = oListArticoli
    '        'attivo il servizio
    '        RemoRuolo = Activator.GetObject(TypeOfRI, ConstSession.UrlServizioRuolo)
    '        Dim oListAvvisi(0) As ObjAvviso
    '        oListAvvisi(0) = MyAvviso
    '        oListAvvisi = RemoRuolo.CalcolaAvvisi(oListAvvisi)
    '        'se ha generato degli avvisi genero il record per il ruolo
    '        If oListAvvisi Is Nothing Then
    '            sScript = "GestAlert('a', 'danger', '', '', 'ATTENZIONE! Errore nel calcolo dell\'importo.');"
    '            Return False
    '        End If
    '        oListAvvisi(0).ID = MyAvviso.ID
    '        oListAvvisi(0).IdFlussoRuolo = MyAvviso.IdFlussoRuolo
    '        oListAvvisi(0).nLottoCartellazione = MyAvviso.nLottoCartellazione
    '        oListAvvisi(0).oTessere = MyAvviso.oTessere
    '        oListAvvisi(0).sCodiceCartella = MyAvviso.sCodiceCartella
    '        oListAvvisi(0).tDataEmissione = MyAvviso.tDataEmissione
    '        oListAvvisi(0).impPagato = MyAvviso.impPagato
    '        oListAvvisi(0).impSaldo = FormatNumber(MyAvviso.impCarico - MyAvviso.impPagato, 2)
    '        'devo settare debito/credito prec uguale a pagato e dovuto al netto di pagato per avere le rate ricalcolateper il solo debito residuo
    '        oListAvvisi(0).impCreditoDebitoPrec = MyAvviso.impPagato
    '        oListAvvisi(0).impDovuto = FormatNumber(MyAvviso.impCarico - MyAvviso.impPagato, 2)
    '        oListAvvisi(0).oPagamenti = MyAvviso.oPagamenti
    '        'devo riprendere ncomponentipv
    '        For Each myArticolo In oListAvvisi(0).oArticoli
    '            For Each myPrecForPV In MyAvviso.oArticoli
    '                If myArticolo.Id = myPrecForPV.Id And myArticolo.Id > 0 Then
    '                    myArticolo.nComponentiPV = myPrecForPV.nComponentiPV
    '                    myArticolo.bForzaPV = myPrecForPV.bForzaPV
    '                    myArticolo.oRiduzioni = myPrecForPV.oRiduzioni
    '                    myArticolo.oDetassazioni = myPrecForPV.oDetassazioni
    '                    '*** 20141211 - legami PF-PV ***
    '                    myArticolo.IdOggetto = myPrecForPV.IdOggetto
    '                    '*** ***
    '                End If
    '            Next
    '        Next
    '        MyAvviso = oListAvvisi(0)
    '        'verifico la presenza delle addizionali
    '        Dim FncAddiz As New GestAddizionali
    '        Dim oAddizionali() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale
    '        oAddizionali = FncAddiz.GetAddizionale(ConstSession.StringConnection, ConstSession.IdEnte, MyAvviso.sAnnoRiferimento, "", TipoRuolo, cmdMyCommand)
    '        If oAddizionali Is Nothing Then
    '            sScript = "GestAlert('a', 'warning', '', '', 'Non sono state configurate le addizionali!Impossibile proseguire!');"
    '            Return False
    '        End If
    '        'calcolo le addizionali
    '        Log.Debug("devo calcolare det voci")
    '        MyAvviso.oDetVoci = RemoRuolo.CalcoloDetVoci(oAddizionali, MyAvviso)
    '        HttpContext.Current.Session.Remove("MyAvvisoRicalcolato")
    '        HttpContext.Current.Session("MyAvvisoRicalcolato") = MyAvviso
    '        ''Se l'importo Nuovo è uguale quello vecchio segnalo che non è un sgravio
    '        'If FormatNumber((oListArticoliOrg(0).impNetto - oArticoloSgravi.impNetto), 2) = 0 Then
    '        '    sScript = "alert('ATTENZIONE! Importo calcolato uguale all\'importo emesso.\nOperazione non consentita in quanto non è uno sgravio!');"
    '        '    RegisterScript(me.gettype(),"msg", sScript)
    '        '    Exit Sub
    '        'Else
    '        ''aggiorno gli articoli sull'avviso
    '        'oListArticoli = Session("oListArticoli")
    '        'For x = 0 To oListArticoli.GetUpperBound(0)
    '        '    If oListArticoli(x).Id = oArticoloSgravi.Id Then
    '        '        oListArticoli(x) = oArticoloSgravi
    '        '        Exit For
    '        '    End If
    '        'Next
    '        HttpContext.Current.Session("oListArticoli") = MyAvviso.oArticoli
    '        HttpContext.Current.Session("oListArticoliSgravi") = MyAvviso.oArticoli
    '        'End If
    '        Return True
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsElabRuolo.RicalcoloAvviso.errore: ", ex)
    '        Return False
    '    End Try
    'End Function
    Private Function CheckGenPV(myConnectionString As String, ByVal sTipoCalcolo As String, ByRef myPrec As ObjArticolo, ByVal myArticolo As ObjArticolo, ByRef nMQ As Double, ByRef nMQPM As Double, ByRef newArticoli As ArrayList) As Boolean
        Dim CurrentItem As New ObjUnitaImmobiliare
        Try
            'controllo se creare nuova pv
            If myPrec.Id > 0 And (myArticolo.sCategoria <> myPrec.sCategoria Or myArticolo.nComponentiPV <> myPrec.nComponentiPV Or myPrec.nBimestri <> myArticolo.nBimestri Or myArticolo.bForzaPV = True Or Not myArticolo.sDescrCategoria.ToUpper.StartsWith("DOM")) Then
                CurrentItem = GeneraPV(myConnectionString, myPrec, nMQ, sTipoCalcolo)
                myPrec = New ObjArticolo
                If Not CurrentItem Is Nothing Then
                    newArticoli.Add(CurrentItem)
                Else
                    Return False
                End If
                nMQ = 0
            End If

            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsElabRuolo.CheckGenPV.errore: ", ex)
            Return False
        End Try
    End Function
    'Private Function CheckGenPV(ByVal sTipoCalcolo As String, ByRef myPrec As ObjArticolo, ByVal myArticolo As ObjArticolo, ByRef nMQ As Double, ByRef nMQPM As Double, ByRef newArticoli As ArrayList) As Boolean
    '    'Private Function CheckGenPV(ByVal myPrec As ObjArticolo, ByVal myArticolo As ObjArticolo, ByVal oArticoloSgravi As ObjArticolo, ByRef nMQ As Double, ByRef nMQPM As Double, ByRef newArticoli As ArrayList) As Boolean
    '    Dim CurrentItem As New ObjUnitaImmobiliare
    '    Try
    '        'controllo se creare nuova pv
    '        If myPrec.Id > 0 And (myArticolo.sCategoria <> myPrec.sCategoria Or myArticolo.nComponentiPV <> myPrec.nComponentiPV Or myPrec.nBimestri <> myArticolo.nBimestri Or myArticolo.bForzaPV = True Or Not myArticolo.sDescrCategoria.ToUpper.StartsWith("DOM")) Then
    '            'If myPrec.nComponenti = oArticoloSgravi.nComponenti Then
    '            'CurrentItem = GeneraPV(myPrec, nMQ, stipocalcolo, oArticoloSgravi.oRiduzioni, oArticoloSgravi.oDetassazioni)
    '            'Else
    '            CurrentItem = GeneraPV(myPrec, nMQ, sTipoCalcolo)
    '            'End If
    '            myPrec = New ObjArticolo
    '            If Not CurrentItem Is Nothing Then
    '                newArticoli.Add(CurrentItem)
    '            Else
    '                Return False
    '            End If
    '            nMQ = 0
    '        Else
    '            'If (Not myArticolo.oRiduzioni Is Nothing And myPrec.oRiduzioni Is Nothing) Or (myArticolo.oRiduzioni Is Nothing And Not myPrec.oRiduzioni Is Nothing) Then
    '            '    CurrentItem = GeneraPV(myPrec, nMQ, stipocalcolo, oArticoloSgravi.oRiduzioni, oArticoloSgravi.oDetassazioni)
    '            '    If Not CurrentItem Is Nothing Then
    '            '        newArticoli.Add(CurrentItem)
    '            '    Else
    '            '        Return False
    '            '    End If
    '            '    nMQ = 0
    '            'Else
    '            '    If Not myArticolo.oRiduzioni Is Nothing Then
    '            '        If (myArticolo.oRiduzioni.GetUpperBound(0) <> myPrec.oRiduzioni.GetUpperBound(0)) Then
    '            '            CurrentItem = GeneraPV(myPrec, nMQ, stipocalcolo, oArticoloSgravi.oRiduzioni, oArticoloSgravi.oDetassazioni)
    '            '            If Not CurrentItem Is Nothing Then
    '            '                newArticoli.Add(CurrentItem)
    '            '            Else
    '            '                Return False
    '            '            End If
    '            '            nMQ = 0
    '            '        End If
    '            '    End If
    '            'End If
    '        End If

    '        Return True
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsElabRuolo.CheckGenPV.errore: ", ex)
    '        Return False
    '    End Try
    'End Function
    Private Function CheckGenPM(myConnectionString As String, ByVal sTipoCalcolo As String, ByVal myPrec As ObjArticolo, ByVal myArticolo As ObjArticolo, ByRef nMQPM As Double, ByRef newArticoli As ArrayList) As Boolean
        Dim CurrentItem As New ObjUnitaImmobiliare
        Try
            'controllo se creare nuova pm
            If myPrec.Id > 0 And (myArticolo.tDataInizio <> myPrec.tDataInizio Or myArticolo.tDataFine <> myPrec.tDataFine) Then
                CurrentItem = GeneraPM(myConnectionString, myPrec, nMQPM, sTipoCalcolo)
                If Not CurrentItem Is Nothing Then
                    newArticoli.Add(CurrentItem)
                Else
                    Return False
                End If
                nMQPM = 0
            Else
                If (Not myArticolo.oRiduzioni Is Nothing And myPrec.oRiduzioni Is Nothing) Or (myArticolo.oRiduzioni Is Nothing And Not myPrec.oRiduzioni Is Nothing) Then
                    CurrentItem = GeneraPM(myConnectionString, myPrec, nMQPM, sTipoCalcolo)
                    If Not CurrentItem Is Nothing Then
                        newArticoli.Add(CurrentItem)
                    Else
                        Return False
                    End If
                    nMQPM = 0
                Else
                    If Not myArticolo.oRiduzioni Is Nothing Then
                        CurrentItem = GeneraPM(myConnectionString, myPrec, nMQPM, sTipoCalcolo)
                        If Not CurrentItem Is Nothing Then
                            newArticoli.Add(CurrentItem)
                        Else
                            Return False
                        End If
                        nMQPM = 0
                    End If
                End If
                If (Not myArticolo.oDetassazioni Is Nothing And myPrec.oDetassazioni Is Nothing) Or (myArticolo.oDetassazioni Is Nothing And Not myPrec.oDetassazioni Is Nothing) Then
                    CurrentItem = GeneraPM(myConnectionString, myPrec, nMQPM, sTipoCalcolo)
                    If Not CurrentItem Is Nothing Then
                        newArticoli.Add(CurrentItem)
                    Else
                        Return False
                    End If
                    nMQPM = 0
                Else
                    If Not myArticolo.oDetassazioni Is Nothing Then
                        CurrentItem = GeneraPM(myConnectionString, myPrec, nMQPM, sTipoCalcolo)
                        If Not CurrentItem Is Nothing Then
                            newArticoli.Add(CurrentItem)
                        Else
                            Return False
                        End If
                        nMQPM = 0
                    End If
                End If
            End If
            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsElabRuolo.CheckGenPM.errore: ", ex)
            Return False
        End Try
    End Function
    Private Function GeneraPartita(myConnectionString As String, ByVal myArticolo As ObjArticolo, ByVal nMQ As Double, ByVal TipoCalcolo As String, ByVal RidSel() As ObjRidEseApplicati, ByVal DetSel() As ObjRidEseApplicati) As ObjUnitaImmobiliare
        Dim CurrentItem As New ObjUnitaImmobiliare
        Dim MaxGG As Integer = 365

        Try
            If DateTime.IsLeapYear(StringOperation.FormatInt(myArticolo.sAnno)) Then
                MaxGG = 365
            End If
            CurrentItem.TipoPartita = myArticolo.TipoPartita
            CurrentItem.Id = myArticolo.Id
            CurrentItem.IdDettaglioTestata = myArticolo.IdDettaglioTestata
            CurrentItem.tDataInizio = myArticolo.tDataInizio
            CurrentItem.tDataFine = myArticolo.tDataFine
            CurrentItem.sVia = myArticolo.sVia
            CurrentItem.sCivico = myArticolo.sCivico
            CurrentItem.sEsponente = myArticolo.sEsponente
            CurrentItem.sInterno = myArticolo.sInterno
            CurrentItem.sScala = myArticolo.sScala
            CurrentItem.sFoglio = myArticolo.sFoglio
            CurrentItem.sNumero = myArticolo.sNumero
            CurrentItem.sSubalterno = myArticolo.sSubalterno
            If nMQ > 0 Then
                CurrentItem.nMQ = nMQ
            Else
                CurrentItem.nMQ = myArticolo.nMQ
            End If
            CurrentItem.bIsImportoForzato = myArticolo.bIsImportoForzato
            CurrentItem.impRuolo = myArticolo.impRuolo
            CurrentItem.nNComponenti = myArticolo.nComponenti
            CurrentItem.IdCategoria = myArticolo.sCategoria
            If CurrentItem.tDataFine.Year > myArticolo.sAnno Then
                CurrentItem.tDataFine = StringOperation.FormatDateTime("31/12/" + myArticolo.sAnno)
            End If
            If TipoCalcolo = "TARES" Then
                CurrentItem.nGGTarsu = DateDiff(DateInterval.Day, CurrentItem.tDataInizio, CurrentItem.tDataFine) + 1
                If CurrentItem.nGGTarsu > MaxGG Then
                    CurrentItem.nGGTarsu = MaxGG
                ElseIf CurrentItem.nGGTarsu < 0 Then
                    CurrentItem.nGGTarsu = MaxGG
                End If
            Else
                If Year(CurrentItem.tDataFine) <> myArticolo.sAnno Then
                    CurrentItem.tDataFine = "31/12/" & myArticolo.sAnno
                End If
            End If
            'prelevo la tariffa
            Dim FncTariffe As New GestTariffe
            Dim oMyTariffe As New ObjTariffa
            Dim oListTariffe() As ObjTariffa
            Dim TypeTassazione As Integer = 1
            oMyTariffe.IdEnte = ConstSession.IdEnte
            oMyTariffe.sAnno = myArticolo.sAnno
            oMyTariffe.sCodice = CurrentItem.IdCategoria & "|" & CurrentItem.nNComponenti.ToString().PadLeft(2, "0")
            Select Case CurrentItem.TipoPartita
                Case ObjArticolo.PARTEFISSA
                    TypeTassazione = 1
                    oListTariffe = FncTariffe.GetTariffa(myConnectionString, oMyTariffe, TypeTassazione)
                    If Not IsNothing(oListTariffe) Then
                        CurrentItem.IdTariffa = oListTariffe(0).ID
                        CurrentItem.impTariffa = oListTariffe(0).sValore
                        CurrentItem.sCatAteco = oListTariffe(0).sDescrizione
                    End If
                Case ObjArticolo.PARTEMAGGIORAZIONE
                    TypeTassazione = 2
                    oListTariffe = FncTariffe.GetTariffa(myConnectionString, oMyTariffe, TypeTassazione)
                    If Not IsNothing(oListTariffe) Then
                        CurrentItem.IdTariffa = oListTariffe(0).ID
                        CurrentItem.impTariffa = oListTariffe(0).sValore
                        CurrentItem.sCatAteco = oListTariffe(0).sDescrizione
                    End If
                Case ObjArticolo.PARTECONFERIMENTI
                    CurrentItem.IdTariffa = myArticolo.nIdTariffa
                    CurrentItem.impTariffa = myArticolo.impTariffa
                    CurrentItem.sCatAteco = myArticolo.sDescrCategoria
                Case Else
                    TypeTassazione = -1
            End Select

            '***Agenzia Entrate***
            CurrentItem.sSezione = myArticolo.sSezione
            CurrentItem.sEstensioneParticella = myArticolo.sEstensioneParticella
            CurrentItem.sIdTipoParticella = myArticolo.sIdTipoParticella
            CurrentItem.nIdTitoloOccupaz = myArticolo.nIdTitoloOccupaz
            CurrentItem.nIdNaturaOccupaz = myArticolo.nIdNaturaOccupaz
            CurrentItem.nIdDestUso = myArticolo.nIdDestUso
            CurrentItem.sIdTipoUnita = myArticolo.sIdTipoUnita
            CurrentItem.nIdAssenzaDatiCatastali = myArticolo.nIdAssenzaDatiCatastali

            CurrentItem.oRiduzioni = PrelevaRidDetPerCalcolo(myConnectionString, myArticolo.oRiduzioni, ObjRidEse.TIPO_RIDUZIONI, myArticolo.sAnno, CurrentItem.TipoPartita)
            CurrentItem.oDetassazioni = PrelevaRidDetPerCalcolo(myConnectionString, myArticolo.oDetassazioni, ObjRidEse.TIPO_ESENZIONI, myArticolo.sAnno, CurrentItem.TipoPartita)
            CurrentItem.ImportoFissoRid = myArticolo.ImportoFissoRid
            'BD 04/10/2021
            For Each myRid As ObjRidEseApplicati In CurrentItem.oRiduzioni
                If myRid.sCodice.Equals("2021") And myArticolo.TipoPartita.Equals("PV") Then '' COVID 2021
                    myRid.sValore = myArticolo.ImportoFissoRid
                    Exit For
                End If
            Next
            'BD 04/10/2021

            Return CurrentItem
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsElabRuolo.GeneraPartita.errore: ", ex)
            Return Nothing
        End Try
    End Function
    'Private Function GeneraPartita(ByVal myArticolo As ObjArticolo, ByVal nMQ As Double, ByVal TipoCalcolo As String, ByVal RidSel() As ObjRidEseApplicati, ByVal DetSel() As ObjRidEseApplicati) As ObjUnitaImmobiliare
    '    Dim MyDBEngine As SqlClient.SqlCommand = Nothing
    '    Dim CurrentItem As New ObjUnitaImmobiliare
    '    Dim MaxGG As Integer = 365

    '    Try
    '        If DateTime.IsLeapYear(stringoperation.formatint(myArticolo.sAnno)) Then
    '            MaxGG = 365
    '        End If
    '        CurrentItem.TipoPartita = myArticolo.TipoPartita
    '        CurrentItem.Id = myArticolo.Id
    '        CurrentItem.IdDettaglioTestata = myArticolo.IdDettaglioTestata
    '        CurrentItem.tDataInizio = myArticolo.tDataInizio
    '        CurrentItem.tDataFine = myArticolo.tDataFine
    '        CurrentItem.sVia = myArticolo.sVia
    '        CurrentItem.sCivico = myArticolo.sCivico
    '        CurrentItem.sEsponente = myArticolo.sEsponente
    '        CurrentItem.sInterno = myArticolo.sInterno
    '        CurrentItem.sScala = myArticolo.sScala
    '        CurrentItem.sFoglio = myArticolo.sFoglio
    '        CurrentItem.sNumero = myArticolo.sNumero
    '        CurrentItem.sSubalterno = myArticolo.sSubalterno
    '        If nMQ > 0 Then
    '            CurrentItem.nMQ = nMQ
    '        Else
    '            CurrentItem.nMQ = myArticolo.nMQ
    '        End If
    '        CurrentItem.bIsImportoForzato = myArticolo.bIsImportoForzato
    '        CurrentItem.impRuolo = myArticolo.impRuolo
    '        CurrentItem.nNComponenti = myArticolo.nComponenti
    '        CurrentItem.IdCategoria = myArticolo.sCategoria
    '        If CurrentItem.tDataFine.Year > myArticolo.sAnno Then
    '            CurrentItem.tDataFine = stringoperation.formatdatetime("31/12/" + myArticolo.sAnno)
    '        End If
    '        If TipoCalcolo = "TARES" Then
    '            CurrentItem.nGGTarsu = DateDiff(DateInterval.Day, CurrentItem.tDataInizio, CurrentItem.tDataFine) + 1
    '            If CurrentItem.nGGTarsu > MaxGG Then
    '                CurrentItem.nGGTarsu = MaxGG
    '            ElseIf CurrentItem.nGGTarsu < 0 Then
    '                CurrentItem.nGGTarsu = MaxGG
    '            End If
    '        Else
    '            If Year(CurrentItem.tDataFine) <> myArticolo.sAnno Then
    '                CurrentItem.tDataFine = "31/12/" & myArticolo.sAnno
    '            End If
    '        End If
    '        'prelevo la tariffa
    '        Dim FncTariffe As New GestTariffe
    '        Dim oMyTariffe As New ObjTariffa
    '        Dim oListTariffe() As ObjTariffa
    '        Dim TypeTassazione As Integer = 1
    '        oMyTariffe.IdEnte = ConstSession.IdEnte
    '        oMyTariffe.sAnno = myArticolo.sAnno
    '        oMyTariffe.sCodice = CurrentItem.IdCategoria & "|" & CurrentItem.nNComponenti.ToString().PadLeft(2, "0")
    '        Select Case CurrentItem.TipoPartita
    '            Case ObjArticolo.PARTEFISSA
    '                TypeTassazione = 1
    '                oListTariffe = FncTariffe.GetTariffa(oMyTariffe, TypeTassazione, MyDBEngine)
    '                If Not IsNothing(oListTariffe) Then
    '                    CurrentItem.IdTariffa = oListTariffe(0).ID
    '                    CurrentItem.impTariffa = oListTariffe(0).sValore
    '                    CurrentItem.sCatAteco = oListTariffe(0).sDescrizione
    '                End If
    '            Case ObjArticolo.PARTEMAGGIORAZIONE
    '                TypeTassazione = 2
    '                oListTariffe = FncTariffe.GetTariffa(oMyTariffe, TypeTassazione, MyDBEngine)
    '                If Not IsNothing(oListTariffe) Then
    '                    CurrentItem.IdTariffa = oListTariffe(0).ID
    '                    CurrentItem.impTariffa = oListTariffe(0).sValore
    '                    CurrentItem.sCatAteco = oListTariffe(0).sDescrizione
    '                End If
    '            Case ObjArticolo.PARTECONFERIMENTI
    '                CurrentItem.IdTariffa = myArticolo.nIdTariffa
    '                CurrentItem.impTariffa = myArticolo.impTariffa
    '                CurrentItem.sCatAteco = myArticolo.sDescrCategoria
    '            Case Else
    '                TypeTassazione = -1
    '        End Select

    '        '***Agenzia Entrate***
    '        CurrentItem.sSezione = myArticolo.sSezione
    '        CurrentItem.sEstensioneParticella = myArticolo.sEstensioneParticella
    '        CurrentItem.sIdTipoParticella = myArticolo.sIdTipoParticella
    '        CurrentItem.nIdTitoloOccupaz = myArticolo.nIdTitoloOccupaz
    '        CurrentItem.nIdNaturaOccupaz = myArticolo.nIdNaturaOccupaz
    '        CurrentItem.nIdDestUso = myArticolo.nIdDestUso
    '        CurrentItem.sIdTipoUnita = myArticolo.sIdTipoUnita
    '        CurrentItem.nIdAssenzaDatiCatastali = myArticolo.nIdAssenzaDatiCatastali

    '        'CurrentItem.oRiduzioni = PrelevaRidDetPerCalcolo(myArticolo.oRiduzioni, RidSel, ObjRidEse.TIPO_RIDUZIONI, myArticolo.sAnno, CurrentItem.TipoPartita, MyDBEngine)
    '        'CurrentItem.oDetassazioni = PrelevaRidDetPerCalcolo(myArticolo.oDetassazioni, DetSel, ObjRidEse.TIPO_ESENZIONI, myArticolo.sAnno, CurrentItem.TipoPartita, MyDBEngine)
    '        CurrentItem.oRiduzioni = PrelevaRidDetPerCalcolo(myArticolo.oRiduzioni, ObjRidEse.TIPO_RIDUZIONI, myArticolo.sAnno, CurrentItem.TipoPartita, MyDBEngine)
    '        CurrentItem.oDetassazioni = PrelevaRidDetPerCalcolo(myArticolo.oDetassazioni, ObjRidEse.TIPO_ESENZIONI, myArticolo.sAnno, CurrentItem.TipoPartita, MyDBEngine)
    '        Return CurrentItem
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsElabRuolo.GeneraPartita.errore: ", ex)
    '        Return Nothing
    '    End Try
    'End Function
    Private Function GeneraPV(myConnectionString As String, ByVal myArticolo As ObjArticolo, ByVal nMQ As Double, ByVal TipoCalcolo As String) As ObjUnitaImmobiliare
        Dim CurrentItem As New ObjUnitaImmobiliare
        Dim MaxGG As Integer = 365

        Try
            If DateTime.IsLeapYear(StringOperation.FormatInt(myArticolo.sAnno)) Then
                MaxGG = 365
            End If
            CurrentItem = New ObjUnitaImmobiliare
            CurrentItem.TipoPartita = ObjArticolo.PARTEVARIABILE
            CurrentItem.tDataInizio = myArticolo.tDataInizio
            CurrentItem.tDataFine = myArticolo.tDataFine
            If CurrentItem.tDataFine.Year > myArticolo.sAnno Then
                CurrentItem.tDataFine = StringOperation.FormatDateTime("31/12/" + myArticolo.sAnno)
            End If
            If myArticolo.sDescrCategoria.ToUpper.StartsWith("DOM") Then
                CurrentItem.nMQ = 1
            Else
                CurrentItem.nMQ = nMQ
            End If
            CurrentItem.nNComponenti = myArticolo.nComponentiPV
            CurrentItem.IdCategoria = myArticolo.sCategoria
            If TipoCalcolo = "TARES" Then
                CurrentItem.nGGTarsu = DateDiff(DateInterval.Day, CurrentItem.tDataInizio, CurrentItem.tDataFine) + 1
                If CurrentItem.nGGTarsu > MaxGG Then
                    CurrentItem.nGGTarsu = MaxGG
                End If
            End If
            'prelevo la tariffa
            Dim FncTariffe As New GestTariffe
            Dim oMyTariffe As New ObjTariffa
            Dim oListTariffe() As ObjTariffa
            oMyTariffe.IdEnte = ConstSession.IdEnte
            oMyTariffe.sAnno = myArticolo.sAnno
            oMyTariffe.sCodice = CurrentItem.IdCategoria & "|" & CurrentItem.nNComponenti.ToString().PadLeft(2, "0")
            oListTariffe = FncTariffe.GetTariffa(myConnectionString, oMyTariffe, -1)
            If Not IsNothing(oListTariffe) Then
                CurrentItem.IdTariffa = oListTariffe(0).ID
                CurrentItem.impTariffa = oListTariffe(0).sValore
                '*** 20140701 - IMU/TARES ***
                CurrentItem.sCatAteco = oListTariffe(0).sDescrizione
                '*** ***
            End If

            CurrentItem.oRiduzioni = PrelevaRidDetPerCalcolo(myConnectionString, myArticolo.oRiduzioni, ObjRidEse.TIPO_RIDUZIONI, myArticolo.sAnno, CurrentItem.TipoPartita)
            'BD 04/01/2021
            For Each myRid As ObjRidEseApplicati In CurrentItem.oRiduzioni
                If myRid.sCodice.Equals("2021") Then '' COVID 2021
                    myRid.sValore = myArticolo.ImportoFissoRid
                    Exit For
                End If
            Next
            'BD 04/01/2021

            CurrentItem.oDetassazioni = PrelevaRidDetPerCalcolo(myConnectionString, myArticolo.oDetassazioni, ObjRidEse.TIPO_ESENZIONI, myArticolo.sAnno, CurrentItem.TipoPartita)
            '*** 20141211 - legami PF-PV ***
            CurrentItem.ListPFvsPV = myArticolo.ListPFvsPV
            '*** ***
            'qaz
            CurrentItem.ImportoFissoRid = myArticolo.ImportoFissoRid
            'qaz
            Return CurrentItem
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsElabRuolo.GeneraPV.errore: ", ex)
            Return Nothing
        End Try
    End Function
    'Private Function GeneraPV(ByVal myArticolo As ObjArticolo, ByVal nMQ As Double, ByVal TipoCalcolo As String) As ObjUnitaImmobiliare
    '    'Private Function GeneraPV(ByVal myArticolo As ObjArticolo, ByVal nMQ As Double, ByVal TipoCalcolo As String, ByVal RidSel() As ObjRidEseApplicati, ByVal DetSel() As ObjRidEseApplicati) As ObjUnitaImmobiliare
    '    Dim MyDBEngine As SqlClient.SqlCommand = Nothing
    '    Dim CurrentItem As New ObjUnitaImmobiliare
    '    Dim MaxGG As Integer = 365

    '    Try
    '        If DateTime.IsLeapYear(stringoperation.formatint(myArticolo.sAnno)) Then
    '            MaxGG = 365
    '        End If
    '        CurrentItem = New ObjUnitaImmobiliare
    '        CurrentItem.TipoPartita = ObjArticolo.PARTEVARIABILE
    '        CurrentItem.tDataInizio = myArticolo.tDataInizio
    '        CurrentItem.tDataFine = myArticolo.tDataFine
    '        If CurrentItem.tDataFine.Year > myArticolo.sAnno Then
    '            CurrentItem.tDataFine = stringoperation.formatdatetime("31/12/" + myArticolo.sAnno)
    '        End If
    '        If myArticolo.sDescrCategoria.ToUpper.StartsWith("DOM") Then
    '            CurrentItem.nMQ = 1
    '        Else
    '            CurrentItem.nMQ = nMQ
    '        End If
    '        CurrentItem.nNComponenti = myArticolo.nComponentiPV
    '        CurrentItem.IdCategoria = myArticolo.sCategoria
    '        If TipoCalcolo = "TARES" Then
    '            CurrentItem.nGGTarsu = DateDiff(DateInterval.Day, CurrentItem.tDataInizio, CurrentItem.tDataFine) + 1
    '            If CurrentItem.nGGTarsu > MaxGG Then
    '                CurrentItem.nGGTarsu = MaxGG
    '            End If
    '        End If
    '        'prelevo la tariffa
    '        Dim FncTariffe As New GestTariffe
    '        Dim oMyTariffe As New ObjTariffa
    '        Dim oListTariffe() As ObjTariffa
    '        oMyTariffe.IdEnte = ConstSession.IdEnte
    '        oMyTariffe.sAnno = myArticolo.sAnno
    '        oMyTariffe.sCodice = CurrentItem.IdCategoria & "|" & CurrentItem.nNComponenti.ToString().PadLeft(2, "0")
    '        oListTariffe = FncTariffe.GetTariffa(oMyTariffe, -1, MyDBEngine)
    '        If Not IsNothing(oListTariffe) Then
    '            CurrentItem.IdTariffa = oListTariffe(0).ID
    '            CurrentItem.impTariffa = oListTariffe(0).sValore
    '            '*** 20140701 - IMU/TARES ***
    '            CurrentItem.sCatAteco = oListTariffe(0).sDescrizione
    '            '*** ***
    '        End If

    '        'CurrentItem.oRiduzioni = PrelevaRidDetPerCalcolo(myArticolo.oRiduzioni, RidSel, ObjRidEse.TIPO_RIDUZIONI, myArticolo.sAnno, CurrentItem.TipoPartita, MyDBEngine)
    '        'CurrentItem.oDetassazioni = PrelevaRidDetPerCalcolo(myArticolo.oDetassazioni, DetSel, ObjRidEse.TIPO_ESENZIONI, myArticolo.sAnno, CurrentItem.TipoPartita, MyDBEngine)
    '        CurrentItem.oRiduzioni = PrelevaRidDetPerCalcolo(myArticolo.oRiduzioni, ObjRidEse.TIPO_RIDUZIONI, myArticolo.sAnno, CurrentItem.TipoPartita, MyDBEngine)
    '        CurrentItem.oDetassazioni = PrelevaRidDetPerCalcolo(myArticolo.oDetassazioni, ObjRidEse.TIPO_ESENZIONI, myArticolo.sAnno, CurrentItem.TipoPartita, MyDBEngine)
    '        '*** 20141211 - legami PF-PV ***
    '        CurrentItem.ListPFvsPV = myArticolo.ListPFvsPV
    '        '*** ***
    '        Return CurrentItem
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsElabRuolo.GeneraPV.errore: ", ex)
    '        Return Nothing
    '    End Try
    'End Function
    Private Function GeneraPM(myConnectionString As String, ByVal myArticolo As ObjArticolo, ByVal nMQ As Double, ByVal TipoCalcolo As String) As ObjUnitaImmobiliare
        Dim CurrentItem As New ObjUnitaImmobiliare
        Dim MaxGG As Integer = 365

        Try
            If DateTime.IsLeapYear(StringOperation.FormatInt(myArticolo.sAnno)) Then
                MaxGG = 365
            End If
            CurrentItem = New ObjUnitaImmobiliare
            CurrentItem.TipoPartita = ObjArticolo.PARTEMAGGIORAZIONE
            CurrentItem.tDataInizio = myArticolo.tDataInizio
            CurrentItem.tDataFine = myArticolo.tDataFine
            If CurrentItem.tDataFine.Year > myArticolo.sAnno Then
                CurrentItem.tDataFine = StringOperation.FormatDateTime("31/12/" + myArticolo.sAnno)
            End If
            CurrentItem.nMQ = nMQ
            CurrentItem.IdCategoria = "MAGST"
            If TipoCalcolo = "TARES" Then
                CurrentItem.nGGTarsu = DateDiff(DateInterval.Day, CurrentItem.tDataInizio, CurrentItem.tDataFine) + 1
                If CurrentItem.nGGTarsu > MaxGG Then
                    CurrentItem.nGGTarsu = MaxGG
                End If
            End If
            'prelevo la tariffa
            Dim FncTariffe As New GestTariffe
            Dim oMyTariffe As New ObjTariffa
            Dim oListTariffe() As ObjTariffa
            oMyTariffe.IdEnte = ConstSession.IdEnte
            oMyTariffe.sAnno = myArticolo.sAnno
            oMyTariffe.sCodice = CurrentItem.IdCategoria & "|00"
            oListTariffe = FncTariffe.GetTariffa(myConnectionString, oMyTariffe, 2)
            If Not IsNothing(oListTariffe) Then
                CurrentItem.IdTariffa = oListTariffe(0).ID
                CurrentItem.impTariffa = oListTariffe(0).sValore
            End If

            CurrentItem.oRiduzioni = PrelevaRidDetPerCalcolo(myConnectionString, myArticolo.oRiduzioni, ObjRidEse.TIPO_RIDUZIONI, myArticolo.sAnno, CurrentItem.TipoPartita)
            CurrentItem.oDetassazioni = PrelevaRidDetPerCalcolo(myConnectionString, myArticolo.oDetassazioni, ObjRidEse.TIPO_ESENZIONI, myArticolo.sAnno, CurrentItem.TipoPartita)
            Return CurrentItem
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsElabRuolo.GeneraPM.errore: ", ex)
            Return Nothing
        End Try
    End Function
    Private Function PrelevaRidDetPerCalcolo(myConnectionString As String, ByVal RidDetPrec() As ObjRidEseApplicati, ByVal Tipo As String, ByVal sAnno As String, ByVal TipoPartita As String) As ObjRidEseApplicati()
        Dim MyArrayRid As New ArrayList
        Dim CurrentRid As New ObjRidEseApplicati
        Dim FncRidEse As New GestRidEse
        Dim RidDetForCalcolo() As ObjRidEseApplicati

        Try
            If Not RidDetPrec Is Nothing Then
                Dim myArtRidDet As New ObjRidEseApplicati
                For Each myArtRidDet In RidDetPrec
                    Dim oRicRidEse As New ObjRidEse
                    oRicRidEse.IdEnte = ConstSession.IdEnte
                    oRicRidEse.sCodice = myArtRidDet.sCodice
                    oRicRidEse.sAnno = sAnno
                    RidDetForCalcolo = FncRidEse.GetRidEseApplicate(myConnectionString, oRicRidEse, Tipo, "", -1, TipoPartita)
                    If Not RidDetForCalcolo Is Nothing Then
                        CurrentRid = New ObjRidEseApplicati
                        CurrentRid.ID = RidDetForCalcolo(0).ID
                        CurrentRid.sCodice = RidDetForCalcolo(0).sCodice
                        CurrentRid.sTipoValore = RidDetForCalcolo(0).sTipoValore
                        CurrentRid.sValore = RidDetForCalcolo(0).sValore
                        If MyArrayRid.Contains(CurrentRid) = False Then
                            MyArrayRid.Add(CurrentRid)
                        End If
                    Else
                        'devo prelevare lo stesso per poter gestire sgravi successivi ma con valore a zero per non calcolarlo
                        If TipoPartita = ObjArticolo.PARTEFISSA Then
                            oRicRidEse = New ObjRidEse
                            oRicRidEse.IdEnte = ConstSession.IdEnte
                            oRicRidEse.sCodice = myArtRidDet.sCodice
                            oRicRidEse.sAnno = sAnno
                            RidDetForCalcolo = FncRidEse.GetRidEseApplicate(myConnectionString, oRicRidEse, Tipo, "", -1, "")
                            If Not RidDetForCalcolo Is Nothing Then
                                CurrentRid = New ObjRidEseApplicati
                                CurrentRid.ID = RidDetForCalcolo(0).ID
                                CurrentRid.sCodice = RidDetForCalcolo(0).sCodice
                                CurrentRid.sTipoValore = RidDetForCalcolo(0).sTipoValore
                                CurrentRid.sValore = "0"
                                If MyArrayRid.Contains(CurrentRid) = False Then
                                    MyArrayRid.Add(CurrentRid)
                                End If
                            End If
                        End If
                    End If
                Next
            End If
            Return CType(MyArrayRid.ToArray(GetType(ObjRidEseApplicati)), ObjRidEseApplicati())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsElabRuolo.PrelevaRidDetPerCalcolo.errore: ", ex)
            Return Nothing
        End Try
    End Function
    'Private Function PrelevaRidDetPerCalcolo(ByVal RidDetPrec() As ObjRidEseApplicati, ByVal Tipo As String, ByVal sAnno As String, ByVal TipoPartita As String, ByVal cmdMyCommand As SqlClient.SqlCommand) As ObjRidEseApplicati()
    '    'Private Function PrelevaRidDetPerCalcolo(ByVal RidDetPrec() As ObjRidEseApplicati, ByVal RidDetArticolo() As ObjRidEseApplicati, ByVal Tipo As String, ByVal sAnno As String, ByVal TipoPartita As String, ByVal MyDBEngine As DBEngine) As ObjRidEseApplicati()
    '    Dim MyArrayRid As New ArrayList
    '    Dim CurrentRid As New ObjRidEseApplicati
    '    Dim FncRidEse As New GestRidEse
    '    Dim RidDetForCalcolo() As ObjRidEseApplicati

    '    Try
    '        'If Not RidDetPrec Is Nothing Then
    '        '    For Each CurrentRid In RidDetPrec
    '        '        MyArrayRid.Add(CurrentRid)
    '        '    Next
    '        'End If
    '        'If Not RidDetArticolo Is Nothing Then
    '        '    Dim myArtRidDet As New ObjRidEseApplicati
    '        '    For Each myArtRidDet In RidDetArticolo
    '        '        Dim oRicRidEse As New ObjRidEse
    '        '        oRicRidEse.IdEnte = ConstSession.IdEnte
    '        '        oRicRidEse.sCodice = myArtRidDet.sCodice
    '        '        oRicRidEse.sAnno = sAnno
    '        '        RidDetForCalcolo = FncRidEse.GetRidEseApplicate(oRicRidEse, Tipo, "", -1, TipoPartita, MyDBEngine)
    '        '        If Not RidDetForCalcolo Is Nothing Then
    '        '            CurrentRid.ID = RidDetForCalcolo(0).ID
    '        '            CurrentRid.sCodice = RidDetForCalcolo(0).sCodice
    '        '            CurrentRid.sTipoValore = RidDetForCalcolo(0).sTipoValore
    '        '            CurrentRid.sValore = RidDetForCalcolo(0).sValore
    '        '            If MyArrayRid.Contains(CurrentRid) = False Then
    '        '                MyArrayRid.Add(CurrentRid)
    '        '            End If
    '        '        End If
    '        '    Next
    '        'End If
    '        If Not RidDetPrec Is Nothing Then
    '            Dim myArtRidDet As New ObjRidEseApplicati
    '            For Each myArtRidDet In RidDetPrec
    '                Dim oRicRidEse As New ObjRidEse
    '                oRicRidEse.IdEnte = ConstSession.IdEnte
    '                oRicRidEse.sCodice = myArtRidDet.sCodice
    '                oRicRidEse.sAnno = sAnno
    '                RidDetForCalcolo = FncRidEse.GetRidEseApplicate(oRicRidEse, Tipo, "", -1, TipoPartita, cmdMyCommand)
    '                If Not RidDetForCalcolo Is Nothing Then
    '                    CurrentRid = New ObjRidEseApplicati
    '                    CurrentRid.ID = RidDetForCalcolo(0).ID
    '                    CurrentRid.sCodice = RidDetForCalcolo(0).sCodice
    '                    CurrentRid.sTipoValore = RidDetForCalcolo(0).sTipoValore
    '                    CurrentRid.sValore = RidDetForCalcolo(0).sValore
    '                    If MyArrayRid.Contains(CurrentRid) = False Then
    '                        MyArrayRid.Add(CurrentRid)
    '                    End If
    '                Else
    '                    'devo prelevare lo stesso per poter gestire sgravi successivi ma con valore a zero per non calcolarlo
    '                    If TipoPartita = ObjArticolo.PARTEFISSA Then
    '                        oRicRidEse = New ObjRidEse
    '                        oRicRidEse.IdEnte = ConstSession.IdEnte
    '                        oRicRidEse.sCodice = myArtRidDet.sCodice
    '                        oRicRidEse.sAnno = sAnno
    '                        RidDetForCalcolo = FncRidEse.GetRidEseApplicate(oRicRidEse, Tipo, "", -1, "", cmdMyCommand)
    '                        If Not RidDetForCalcolo Is Nothing Then
    '                            CurrentRid = New ObjRidEseApplicati
    '                            CurrentRid.ID = RidDetForCalcolo(0).ID
    '                            CurrentRid.sCodice = RidDetForCalcolo(0).sCodice
    '                            CurrentRid.sTipoValore = RidDetForCalcolo(0).sTipoValore
    '                            CurrentRid.sValore = "0"
    '                            If MyArrayRid.Contains(CurrentRid) = False Then
    '                                MyArrayRid.Add(CurrentRid)
    '                            End If
    '                        End If
    '                    End If
    '                End If
    '            Next
    '        End If
    '        Return CType(MyArrayRid.ToArray(GetType(ObjRidEseApplicati)), ObjRidEseApplicati())
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsElabRuolo.PrelevaRidDetPerCalcolo.errore: ", ex)
    '        Return Nothing
    '    End Try
    'End Function
#End Region
    '**** 201809 - Cartelle Insoluti ***
#Region "Ruolo Cartelle Insoluti"
    Public Function CalcolaRuoloCartelleInsoluti(ByVal myStringConnection As String, ByVal sEnte As String, ByVal nAnno As Integer, GGScadenza As Integer, sOperatore As String, ByVal oAddizionali() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale) As ObjRuolo()
        Dim oMyRiepilogo As New ArrayList
        Dim oSingleRiepilogo As ObjRuolo
        Dim FncDich As New ClsDichiarazione
        Dim oListAvvisi As New ArrayList
        Dim TypeOfRI As Type = GetType(RemotingInterfaceMotoreTarsu.RemotingInterfaceMotoreTarsu.IRuoloTARSUVariabile)
        Dim RemoRuolo As RemotingInterfaceMotoreTarsu.RemotingInterfaceMotoreTarsu.IRuoloTARSUVariabile
        Dim nAvanzamento As Integer = 0
        Dim sAvanzamento As String = ""

        Dim sSQL As String = ""
        Dim myDataView As New DataView
        Dim nMyReturn As Integer = -1

        Try
            Using ctx As New DBModel(ConstSession.DBType, myStringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_RuoloCartelleInsolutiCrea", "IDENTE", "ANNO", "GGSCADENZA")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", sEnte) _
                            , ctx.GetParam("ANNO", nAnno) _
                            , ctx.GetParam("GGSCADENZA", GGScadenza)
                        )
                Catch ex As Exception
                    Log.Debug(sEnte + " - OPENgovTIA.ClsElabRuolo.CalcolaRuoloCartelleInsoluti.erroreQuery: ", ex)
                    Return Nothing
                Finally
                    ctx.Dispose()
                End Try
                'incremento l'oggetto di riepilogo
                oSingleRiepilogo = New ObjRuolo
                For Each myRow As DataRowView In myDataView
                    Dim myItem As New ObjAvviso
                    Dim ListVoci As New ArrayList
                    Dim myVoce As New ObjDetVoci
                    Dim impVoci As Double = 0
                    Dim impVociImponibile As Double = 0

                    nAvanzamento += 1
                    sAvanzamento = "Lettura posizione " & nAvanzamento & " su " & myDataView.Count
                    CacheManager.SetAvanzamentoElaborazione(sAvanzamento)

                    myItem = New ObjAvviso
                    myItem.ID = -1
                    myItem.sOperatore = sOperatore
                    myItem.tDataInserimento = DateTime.Now
                    myItem.IdEnte = StringOperation.FormatString(myRow("idente"))
                    myItem.IdContribuente = StringOperation.FormatInt(myRow("COD_CONTRIBUENTE"))
                    myItem.sCodFiscale = StringOperation.FormatString(myRow("cfpiva"))
                    myItem.sCognome = StringOperation.FormatString(myRow("COGNOME"))
                    myItem.sNome = StringOperation.FormatString(myRow("NOME"))

                    myItem.sIndirizzoRes = StringOperation.FormatString(myRow("via_res"))
                    myItem.sCivicoRes = StringOperation.FormatString(myRow("civico_res"))
                    myItem.sCAPRes = StringOperation.FormatString(myRow("cap_res"))
                    myItem.sComuneRes = StringOperation.FormatString(myRow("comune_res"))
                    myItem.sProvRes = StringOperation.FormatString(myRow("provincia_res"))

                    myItem.sNominativoCO = StringOperation.FormatString(myRow("nominativoco"))
                    myItem.sIndirizzoCO = StringOperation.FormatString(myRow("indirizzoco"))
                    myItem.sCivicoCO = StringOperation.FormatString(myRow("civicoco"))
                    myItem.sCAPCO = StringOperation.FormatString(myRow("capco"))
                    myItem.sComuneCO = StringOperation.FormatString(myRow("comuneco"))
                    myItem.sProvCO = StringOperation.FormatString(myRow("pvco"))

                    myItem.sAnnoRiferimento = StringOperation.FormatString(myRow("ANNO"))
                    myItem.sCodiceCartella = StringOperation.FormatString(myRow("CODICE_CARTELLA"))
                    myItem.IdFlussoRuolo = StringOperation.FormatInt(myRow("IDFLUSSO_RUOLO"))
                    myItem.tDataEmissione = StringOperation.FormatDateTime(myRow("DATA_EMISSIONE"))
                    myItem.impPF = StringOperation.FormatDouble(myRow("IMPORTO_PF"))
                    myItem.impPV = StringOperation.FormatDouble(myRow("IMPORTO_PV"))
                    myItem.impPC = StringOperation.FormatDouble(myRow("IMPORTO_PC"))
                    myItem.impPM = StringOperation.FormatDouble(myRow("IMPORTO_PM"))
                    myItem.impPagato = StringOperation.FormatDouble(myRow("PAGATO"))
                    myItem.impCreditoDebitoPrec = StringOperation.FormatDouble(myRow("PAGATO"))
                    myItem.nLottoCartellazione = StringOperation.FormatInt(myRow("LOTTO_CARTELLAZIONE"))

                    'prelevo gli articoli
                    myItem.oArticoli = New GestArticolo().GetArticoli(myStringConnection, -1, StringOperation.FormatInt(myRow("id")), -1, True)
                    If myItem.oArticoli Is Nothing Then
                        Log.Debug("Mancano gli articoli::oMyAvviso.ID::" & myItem.ID & "::oMyAvviso.sCodiceCartella::" & myItem.sCodiceCartella)
                        Return Nothing
                    End If

                    'prelevo il dettaglio voci
                    myItem.oDetVoci = New GestDetVoci().GetDetVoci(myStringConnection, StringOperation.FormatInt(myRow("id")), -1)
                    'aggiungo le voci precedenti
                    For Each myVoce In myItem.oDetVoci
                        myVoce.IdDettaglio = -1
                        myVoce.IdAvviso = myItem.ID
                        myVoce.sOperatore = myItem.sOperatore
                        myVoce.tDataInserimento = myItem.tDataInserimento
                        If myVoce.impDettaglio <> 0 Then
                            ListVoci.Add(myVoce)
                        End If
                    Next
                    'calcolo le addizionali
                    RemoRuolo = Activator.GetObject(TypeOfRI, ConstSession.UrlServizioRuolo)
                    Dim ListDetVoci() As ObjDetVoci = RemoRuolo.CalcoloDetVoci(oAddizionali, myItem)
                    For Each myVoce In ListDetVoci
                        For Each myAdd As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale In oAddizionali
                            If myVoce.sCapitolo = myAdd.CodiceCapitolo Then
                                myVoce.IdDettaglio = -1
                                myVoce.IdAvviso = myItem.ID
                                myVoce.sOperatore = myItem.sOperatore
                                myVoce.tDataInserimento = myItem.tDataInserimento
                                ListVoci.Add(myVoce)
                                If myVoce.sCapitolo <> ObjDetVoci.Capitolo.SpeseNotifica Then
                                    impVociImponibile += myVoce.impDettaglio
                                End If
                                impVoci += myVoce.impDettaglio
                                Exit For
                            End If
                        Next
                    Next

                    myItem.impTotale = StringOperation.FormatDouble(myRow("IMPORTO_TOTALE")) + impVociImponibile
                    myItem.impArrotondamento = FormatNumber(myItem.impTotale, 0) - myItem.impTotale
                    myItem.impCarico = FormatNumber(myItem.impTotale + myItem.impArrotondamento + impVoci, 2)
                    myItem.impDovuto = FormatNumber(myItem.impTotale + myItem.impArrotondamento + impVoci, 2) - myItem.impPagato
                    myItem.impSaldo = FormatNumber(myItem.impTotale + myItem.impArrotondamento + impVoci, 2) - myItem.impPagato
                    myItem.oDetVoci = ListVoci.ToArray(GetType(ObjDetVoci))
                    oListAvvisi.Add(myItem)
                Next
            End Using
            oSingleRiepilogo.oAvvisi = CType(oListAvvisi.ToArray(GetType(ObjAvviso)), ObjAvviso())
            oMyRiepilogo.Add(oSingleRiepilogo)

            Return CType(oMyRiepilogo.ToArray(GetType(ObjRuolo)), ObjRuolo())
        Catch Err As Exception
            Log.Debug(sEnte + " - OPENgovTIA.ClsElabRuolo.CalcolaRuoloCartelleInsoluti.errore: ", Err)
            Return Nothing
        Finally
            myDataView.Dispose()
        End Try
    End Function
    'Public Function CalcolaRuoloCartelleInsoluti(ByVal myStringConnection As String, ByVal sEnte As String, ByVal nAnno As Integer, GGScadenza As Integer, sOperatore As String, ByVal oAddizionali() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale) As ObjRuolo()
    '    Dim oMyRiepilogo As New ArrayList
    '    Dim oSingleRiepilogo As ObjRuolo
    '    Dim FncDich As New ClsDichiarazione
    '    Dim oListAvvisi As New ArrayList
    '    Dim TypeOfRI As Type = GetType(RemotingInterfaceMotoreTarsu.RemotingInterfaceMotoreTarsu.IRuoloTARSUVariabile)
    '    Dim RemoRuolo As RemotingInterfaceMotoreTarsu.RemotingInterfaceMotoreTarsu.IRuoloTARSUVariabile
    '    Dim DsDati As New DataSet
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim dtMyDati As New DataTable()
    '    Dim nAvanzamento As Integer = 0
    '    Dim sAvanzamento As String = ""

    '    Try
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure

    '        cmdMyCommand.CommandText = "prc_RuoloCartelleInsolutiCrea"
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdEnte", SqlDbType.VarChar)).Value = sEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Anno", SqlDbType.Int)).Value = nAnno
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@GGScadenza", SqlDbType.Int)).Value = GGScadenza
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(dtMyDati)
    '        myAdapter.Dispose()
    '        'incremento l'oggetto di riepilogo
    '        oSingleRiepilogo = New ObjRuolo
    '        For Each dtMyRow As DataRow In dtMyDati.Rows
    '            Dim myItem As New ObjAvviso
    '            Dim ListVoci As New ArrayList
    '            Dim myVoce As New ObjDetVoci
    '            Dim impVoci As Double = 0
    '            Dim impVociImponibile As Double = 0

    '            nAvanzamento += 1
    '            sAvanzamento = "Lettura posizione " & nAvanzamento & " su " & dtMyDati.Rows.Count
    '            CacheManager.SetAvanzamentoElaborazione(sAvanzamento)

    '            myItem = New ObjAvviso
    '            myItem.ID = -1
    '            myItem.sOperatore = sOperatore
    '            myItem.tDataInserimento = DateTime.Now
    '            myItem.IdEnte = stringoperation.formatstring(dtMyRow("idente"))
    '            myItem.IdContribuente = stringoperation.formatint(dtMyRow("COD_CONTRIBUENTE"))
    '            myItem.sCodFiscale = stringoperation.formatstring(dtMyRow("cfpiva"))
    '            If Not IsDBNull(dtMyRow("COGNOME")) Then
    '                myItem.sCognome = stringoperation.formatstring(dtMyRow("COGNOME"))
    '            End If
    '            If Not IsDBNull(dtMyRow("NOME")) Then
    '                myItem.sNome = stringoperation.formatstring(dtMyRow("NOME"))
    '            End If

    '            myItem.sIndirizzoRes = stringoperation.formatstring(dtMyRow("via_res"))
    '            myItem.sCivicoRes = stringoperation.formatstring(dtMyRow("civico_res"))
    '            myItem.sCAPRes = stringoperation.formatstring(dtMyRow("cap_res"))
    '            myItem.sComuneRes = stringoperation.formatstring(dtMyRow("comune_res"))
    '            myItem.sProvRes = stringoperation.formatstring(dtMyRow("provincia_res"))

    '            myItem.sNominativoCO = stringoperation.formatstring(dtMyRow("nominativoco"))
    '            myItem.sIndirizzoCO = stringoperation.formatstring(dtMyRow("indirizzoco"))
    '            myItem.sCivicoCO = stringoperation.formatstring(dtMyRow("civicoco"))
    '            myItem.sCAPCO = stringoperation.formatstring(dtMyRow("capco"))
    '            myItem.sComuneCO = stringoperation.formatstring(dtMyRow("comuneco"))
    '            myItem.sProvCO = stringoperation.formatstring(dtMyRow("pvco"))

    '            myItem.sAnnoRiferimento = stringoperation.formatstring(dtMyRow("ANNO"))
    '            myItem.sCodiceCartella = stringoperation.formatstring(dtMyRow("CODICE_CARTELLA"))
    '            myItem.IdFlussoRuolo = stringoperation.formatint(dtMyRow("IDFLUSSO_RUOLO"))
    '            If Not IsDBNull(dtMyRow("DATA_EMISSIONE")) Then
    '                myItem.tDataEmissione = stringoperation.formatdatetime(dtMyRow("DATA_EMISSIONE"))
    '            End If
    '            If Not IsDBNull(dtMyRow("IMPORTO_PF")) Then
    '                myItem.impPF = stringoperation.formatdouble(dtMyRow("IMPORTO_PF"))
    '            End If
    '            If Not IsDBNull(dtMyRow("IMPORTO_PV")) Then
    '                myItem.impPV = stringoperation.formatdouble(dtMyRow("IMPORTO_PV"))
    '            End If
    '            If Not IsDBNull(dtMyRow("IMPORTO_PC")) Then
    '                myItem.impPC = stringoperation.formatdouble(dtMyRow("IMPORTO_PC"))
    '            End If
    '            If Not IsDBNull(dtMyRow("IMPORTO_PM")) Then
    '                myItem.impPM = stringoperation.formatdouble(dtMyRow("IMPORTO_PM"))
    '            End If
    '            If Not IsDBNull(dtMyRow("PAGATO")) Then
    '                myItem.impPagato = stringoperation.formatdouble(dtMyRow("PAGATO"))
    '                myItem.impCreditoDebitoPrec = stringoperation.formatdouble(dtMyRow("PAGATO"))
    '            End If
    '            If Not IsDBNull(dtMyRow("LOTTO_CARTELLAZIONE")) Then
    '                myItem.nLottoCartellazione = stringoperation.formatint(dtMyRow("LOTTO_CARTELLAZIONE"))
    '            End If

    '            'prelevo gli articoli
    '            myItem.oArticoli = New GestArticolo().GetArticoli(myStringConnection, -1, stringoperation.formatint(dtMyRow("id")), -1, True, cmdMyCommand)
    '            If myItem.oArticoli Is Nothing Then
    '                Log.Debug("Mancano gli articoli::oMyAvviso.ID::" & myItem.ID & "::oMyAvviso.sCodiceCartella::" & myItem.sCodiceCartella)
    '                Return Nothing
    '            End If

    '            'prelevo il dettaglio voci
    '            myItem.oDetVoci = New GestDetVoci().GetDetVoci(stringoperation.formatint(dtMyRow("id")), -1, cmdMyCommand)
    '            'aggiungo le voci precedenti
    '            For Each myVoce In myItem.oDetVoci
    '                myVoce.IdDettaglio = -1
    '                myVoce.IdAvviso = myItem.ID
    '                myVoce.sOperatore = myItem.sOperatore
    '                myVoce.tDataInserimento = myItem.tDataInserimento
    '                If myVoce.impDettaglio <> 0 Then
    '                    ListVoci.Add(myVoce)
    '                End If
    '            Next
    '            'calcolo le addizionali
    '            RemoRuolo = Activator.GetObject(TypeOfRI, ConstSession.UrlServizioRuolo)
    '            Dim ListDetVoci() As ObjDetVoci = RemoRuolo.CalcoloDetVoci(oAddizionali, myItem)
    '            For Each myVoce In ListDetVoci
    '                For Each myAdd As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale In oAddizionali
    '                    If myVoce.sCapitolo = myAdd.CodiceCapitolo Then
    '                        myVoce.IdDettaglio = -1
    '                        myVoce.IdAvviso = myItem.ID
    '                        myVoce.sOperatore = myItem.sOperatore
    '                        myVoce.tDataInserimento = myItem.tDataInserimento
    '                        ListVoci.Add(myVoce)
    '                        If myVoce.sCapitolo <> ObjDetVoci.Capitolo.SpeseNotifica Then
    '                            impVociImponibile += myVoce.impDettaglio
    '                        End If
    '                        impVoci += myVoce.impDettaglio
    '                        Exit For
    '                    End If
    '                Next
    '            Next
    '            ''aggiungo la voce del pagato precedente
    '            'myVoce = New ObjDetVoci
    '            'myVoce.CodVoce = ObjDetVoci.Voce.Imposta
    '            'myVoce.IdAvviso = myItem.ID
    '            'myVoce.IdEnte = myItem.IdEnte
    '            'myVoce.impDettaglio = myItem.impPagato
    '            'myVoce.sAnno = myItem.sAnnoRiferimento
    '            'myVoce.sCapitolo = ObjDetVoci.Capitolo.CreditoPrec
    '            'myVoce.sOperatore = myItem.sOperatore
    '            'myVoce.tDataInserimento = myItem.tDataInserimento
    '            'If myVoce.impDettaglio <> 0 Then
    '            '    ListVoci.Add(myVoce)
    '            'End If

    '            If Not IsDBNull(dtMyRow("IMPORTO_TOTALE")) Then
    '                myItem.impTotale = stringoperation.formatdouble(dtMyRow("IMPORTO_TOTALE")) + impVociImponibile
    '            End If
    '            myItem.impArrotondamento = FormatNumber(myItem.impTotale, 0) - myItem.impTotale
    '            myItem.impCarico = FormatNumber(myItem.impTotale + myItem.impArrotondamento + impVoci, 2)
    '            myItem.impDovuto = FormatNumber(myItem.impTotale + myItem.impArrotondamento + impVoci, 2) - myItem.impPagato
    '            myItem.impSaldo = FormatNumber(myItem.impTotale + myItem.impArrotondamento + impVoci, 2) - myItem.impPagato
    '            ''aggiorno la voce dell'arrotondamento
    '            'Dim hasArrotondamento As Boolean = False
    '            'For Each myArr As ObjDetVoci In ListVoci
    '            '    If myArr.sCapitolo = ObjDetVoci.Capitolo.Arrotondamento Then
    '            '        myArr.impDettaglio += myItem.impArrotondamento
    '            '        hasArrotondamento = True
    '            '        Exit For
    '            '    End If
    '            'Next
    '            'If hasArrotondamento = False Then
    '            '    myVoce = New ObjDetVoci
    '            '    myVoce.CodVoce = ObjDetVoci.Voce.Imposta
    '            '    myVoce.IdAvviso = myItem.ID
    '            '    myVoce.IdEnte = myItem.IdEnte
    '            '    myVoce.impDettaglio = myItem.impArrotondamento
    '            '    myVoce.sAnno = myItem.sAnnoRiferimento
    '            '    myVoce.sCapitolo = ObjDetVoci.Capitolo.Arrotondamento
    '            '    myVoce.sOperatore = myItem.sOperatore
    '            '    myVoce.tDataInserimento = myItem.tDataInserimento
    '            '    If myVoce.impDettaglio <> 0 Then
    '            '        ListVoci.Add(myVoce)
    '            '    End If
    '            'End If
    '            myItem.oDetVoci = ListVoci.ToArray(GetType(ObjDetVoci))
    '            oListAvvisi.Add(myItem)
    '        Next
    '        oSingleRiepilogo.oAvvisi = CType(oListAvvisi.ToArray(GetType(ObjAvviso)), ObjAvviso())
    '        oMyRiepilogo.Add(oSingleRiepilogo)

    '        Return CType(oMyRiepilogo.ToArray(GetType(ObjRuolo)), ObjRuolo())
    '    Catch Err As Exception
    '        Log.Debug(sEnte + " - OPENgovTIA.ClsElabRuolo.CalcolaRuoloCartelleInsoluti.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return Nothing
    '    Finally
    '        dtMyDati.Dispose()
    '        cmdMyCommand.Dispose()
    '        cmdMyCommand.Connection.Close()
    '    End Try
    'End Function
#End Region
End Class
''' <summary>
''' Classe per la gestione del ruolo
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class ClsGestRuolo
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ClsGestRuolo))
    Private cmdMyCommand As New SqlClient.SqlCommand
    Private oReplace As New generalClass.generalFunction
    ''' <summary>
    ''' Funzione per prelevare la tipologia di calcolo Tassa Rifiuti da applicare.
    ''' Legge da db e scrive nei controlli in ingresso.
    ''' </summary>
    ''' <param name="myStringConnection"></param>
    ''' <param name="IdEnte"></param>
    ''' <param name="Anno"></param>
    ''' <param name="LblTipoCalcolo">ByRef</param>
    ''' <param name="LblTipoMQ">ByRef</param>
    ''' <param name="ChkConferimenti">ByRef</param>
    ''' <param name="ChkMaggiorazione">ByRef</param>
    Public Sub LoadTipoCalcolo(myStringConnection As String, ByVal IdEnte As String, ByVal Anno As String, ByRef LblTipoCalcolo As Label, ByRef LblTipoMQ As Label, ByRef ChkConferimenti As CheckBox, ByRef ChkMaggiorazione As CheckBox)
        Dim sSQL As String = ""
        Dim myDataView As New DataView
        Dim nMyReturn As Integer = -1

        Try
            LblTipoCalcolo.Text = " -- "
            LblTipoMQ.Text = " -- "
            ChkConferimenti.Checked = False
            ChkMaggiorazione.Checked = False
            Using ctx As New DBModel(ConstSession.DBType, myStringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetTipoCalcolo", "IdEnte", "Anno")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IdEnte", ConstSession.IdEnte) _
                            , ctx.GetParam("Anno", Anno)
                        )
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestRuolo.LoadTipoCalcolo.errore: ", ex)
                Finally
                    ctx.Dispose()
                End Try
                For Each myRow As DataRowView In myDataView
                    LblTipoCalcolo.Text = myRow("tipocalcolo")
                    Select Case myRow("tipomq").ToString()
                        Case "D"
                            LblTipoMQ.Text = "Dichiarate"
                        Case "C"
                            LblTipoMQ.Text = "Catastali"
                    End Select
                    ChkConferimenti.Checked = myRow("hasconferimenti")
                    ChkMaggiorazione.Checked = myRow("hasmaggiorazione")
                Next
            End Using
            ChkConferimenti.Enabled = False : ChkMaggiorazione.Enabled = False
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestRuolo.LoadTipoCalcolo.errore: ", Err)
        Finally
            myDataView.Dispose()
        End Try
    End Sub
    'Public Sub LoadTipoCalcolo(ByVal IdEnte As String, ByVal Anno As String, ByRef LblTipoCalcolo As Label, ByRef LblTipoMQ As Label, ByRef ChkConferimenti As CheckBox, ByRef ChkMaggiorazione As CheckBox, ByVal cmdMyCommandOut As SqlClient.SqlCommand)
    '    Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim dtMyDati As New DataTable()
    '    Dim dtMyRow As DataRow

    '    Try
    '        LblTipoCalcolo.Text = " -- "
    '        LblTipoMQ.Text = " -- "
    '        ChkConferimenti.Checked = False
    '        ChkMaggiorazione.Checked = False

    '        If (Not (cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand = cmdMyCommandOut
    '        Else
    '            cmdMyCommand = New SqlClient.SqlCommand
    '            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure

    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.AddWithValue("@IdEnte", ConstSession.IdEnte)
    '        cmdMyCommand.Parameters.AddWithValue("@Anno", Anno)
    '        cmdMyCommand.CommandText = "prc_GetTipoCalcolo"
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(dtMyDati)
    '        myAdapter.Dispose()
    '        For Each dtMyRow In dtMyDati.Rows
    '            LblTipoCalcolo.Text = dtMyRow("tipocalcolo")
    '            Select Case dtMyRow("tipomq").ToString()
    '                Case "D"
    '                    LblTipoMQ.Text = "Dichiarate"
    '                Case "C"
    '                    LblTipoMQ.Text = "Catastali"
    '            End Select
    '            ChkConferimenti.Checked = dtMyRow("hasconferimenti")
    '            ChkMaggiorazione.Checked = dtMyRow("hasmaggiorazione")
    '        Next
    '        ChkConferimenti.Enabled = False : ChkMaggiorazione.Enabled = False
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestRuolo.LoadTipoCalcolo.errore: ", ex)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '    Finally
    '        dtMyDati.Dispose()
    '        If (cmdMyCommandOut Is Nothing) Then
    '            cmdMyCommand.Dispose()
    '            cmdMyCommand.Connection.Close()
    '        End If
    '    End Try
    'End Sub
    ''' <summary>
    ''' Funzione per l'inserimento/aggiornamento dei dati del ruolo nel db
    ''' </summary>
    ''' <param name="DBOperation"></param>
    ''' <param name="oMyRuolo"></param>
    ''' <param name="myStringConnection"></param>
    ''' <returns></returns>
    Public Function SetRuolo(ByVal DBOperation As Integer, ByVal oMyRuolo As ObjRuolo, myStringConnection As String) As Integer
        Dim MyProcedure As String = "prc_TBLRUOLI_GENERATI_IU"
        Dim sSQL As String = ""
        Dim myDataView As New DataView
        Dim nMyReturn As Integer = -1

        Try
            If oMyRuolo.tDataInizioConf = DateTime.MinValue Then
                oMyRuolo.tDataInizioConf = DateTime.MaxValue
            End If
            Using ctx As New DBModel(ConstSession.DBType, myStringConnection)
                Try
                    If DBOperation = Utility.Costanti.AZIONE_DELETE Then
                        sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_TBLRUOLI_GENERATI_D", "IDFLUSSO")
                        myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDFLUSSO", oMyRuolo.IdFlusso))
                    Else
                        sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_TBLRUOLI_GENERATI_IU", "IDFLUSSO", "IDENTE", "ANNO", "NOME_RUOLO", "DESCRIZIONE", "TIPOTASSAZIONE", "TIPO_RUOLO", "NUMERO_RUOLO", "TOT_ARTICOLI", "TOT_CONTRIBUENTI", "TOT_AVVISI", "TOT_SCARTI", "TOT_MQ", "TOT_IMPORTO", "TOT_IMPORTO_RIDUZIONI", "TOT_IMPORTO_DETASSAZIONI", "TOT_IMPORTO_NETTO", "TOT_IMPORTO_SANZIONI", "TOT_IMPORTO_INTERESSI", "TOT_IMPORTO_SPESE_NOTIFICA", "TOT_IMPORTO_PF", "TOT_IMPORTO_PV", "TOT_CARTELLATO", "TASSAZIONEMINIMA", "IMPORTO_MINIMO", "DATA_CREAZIONE", "DATA_STAMPA_MINUTA", "DATA_OKMINUTA", "DATA_CARTELLAZIONE", "DATA_ESTRAZIONE_290", "DATA_ESTRAZIONE_POSTEL", "DATA_ELABDOC", "DATA_OKDOCUMENTI", "DATA_CANCELLAZIONE", "OPERATORE", "TOT_IMPORTO_PM", "TOT_IMPORTO_PC", "TOT_ADDIZIONALI", "PERCENTUALE_TARIFFE", "SOGLIARATE", "INIZIOCONF", "FINECONF", "TIPOGENERAZIONE")
                        myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDFLUSSO", oMyRuolo.IdFlusso) _
                                , ctx.GetParam("IDENTE", oMyRuolo.sEnte) _
                                , ctx.GetParam("ANNO", oMyRuolo.sAnno) _
                                , ctx.GetParam("NOME_RUOLO", oMyRuolo.sNomeRuolo) _
                                , ctx.GetParam("DESCRIZIONE", oMyRuolo.sDescrRuolo) _
                                , ctx.GetParam("TIPOTASSAZIONE", oMyRuolo.sTipoCalcolo) _
                                , ctx.GetParam("TIPO_RUOLO", oMyRuolo.sTipoRuolo) _
                                , ctx.GetParam("NUMERO_RUOLO", oMyRuolo.nNumeroRuolo) _
                                , ctx.GetParam("TOT_ARTICOLI", oMyRuolo.nArticoli) _
                                , ctx.GetParam("TOT_CONTRIBUENTI", oMyRuolo.nContribuenti) _
                                , ctx.GetParam("TOT_AVVISI", oMyRuolo.nAvvisi) _
                                , ctx.GetParam("TOT_SCARTI", oMyRuolo.nScarti) _
                                , ctx.GetParam("TOT_MQ", oMyRuolo.nMQ) _
                                , ctx.GetParam("TOT_IMPORTO", oMyRuolo.ImpArticoli) _
                                , ctx.GetParam("TOT_IMPORTO_RIDUZIONI", oMyRuolo.ImpRiduzione) _
                                , ctx.GetParam("TOT_IMPORTO_DETASSAZIONI", oMyRuolo.ImpDetassazione) _
                                , ctx.GetParam("TOT_IMPORTO_NETTO", oMyRuolo.ImpNetto) _
                                , ctx.GetParam("TOT_IMPORTO_SANZIONI", 0) _
                                , ctx.GetParam("TOT_IMPORTO_INTERESSI", 0) _
                                , ctx.GetParam("TOT_IMPORTO_SPESE_NOTIFICA", 0) _
                                , ctx.GetParam("TOT_IMPORTO_PF", oMyRuolo.ImpPF) _
                                , ctx.GetParam("TOT_IMPORTO_PV", oMyRuolo.ImpPV) _
                                , ctx.GetParam("TOT_CARTELLATO", oMyRuolo.ImpAvvisi) _
                                , ctx.GetParam("TASSAZIONEMINIMA", oMyRuolo.nTassazioneMinima) _
                                , ctx.GetParam("IMPORTO_MINIMO", oMyRuolo.ImpMinimo) _
                                , ctx.GetParam("DATA_CREAZIONE", oMyRuolo.tDataCreazione) _
                                , ctx.GetParam("DATA_STAMPA_MINUTA", DBNull.Value) _
                                , ctx.GetParam("DATA_OKMINUTA", DBNull.Value) _
                                , ctx.GetParam("DATA_CARTELLAZIONE", DBNull.Value) _
                                , ctx.GetParam("DATA_ESTRAZIONE_290", DBNull.Value) _
                                , ctx.GetParam("DATA_ESTRAZIONE_POSTEL", DBNull.Value) _
                                , ctx.GetParam("DATA_ELABDOC", DBNull.Value) _
                                , ctx.GetParam("DATA_OKDOCUMENTI", DBNull.Value) _
                                , ctx.GetParam("DATA_CANCELLAZIONE", DBNull.Value) _
                                , ctx.GetParam("OPERATORE", oMyRuolo.sOperatore) _
                                , ctx.GetParam("TOT_IMPORTO_PM", oMyRuolo.impPM) _
                                , ctx.GetParam("TOT_IMPORTO_PC", oMyRuolo.impPC) _
                                , ctx.GetParam("TOT_ADDIZIONALI", oMyRuolo.impAddizionali) _
                                , ctx.GetParam("PERCENTUALE_TARIFFE", oMyRuolo.PercentTariffe) _
                                , ctx.GetParam("SOGLIARATE", 0) _
                                , ctx.GetParam("INIZIOCONF", oMyRuolo.tDataInizioConf) _
                                , ctx.GetParam("FINECONF", oMyRuolo.tDataFineConf) _
                                , ctx.GetParam("TIPOGENERAZIONE", oMyRuolo.TipoGenerazione)
                            )
                    End If
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestRuolo.SetRuolo.erroreQuery: ", ex)
                    Return 0
                Finally
                    ctx.Dispose()
                End Try
                For Each myRow As DataRowView In myDataView
                    oMyRuolo.IdFlusso = StringOperation.FormatInt(myRow("ID"))
                Next
            End Using
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestRuolo.SetRuolo.errore: ", Err)
            Return 0
        Finally
            myDataView.Dispose()
        End Try
        Return oMyRuolo.IdFlusso
    End Function
    'Public Function SetRuolo(ByVal DBOperation As Integer, ByVal oMyRuolo As ObjRuolo, myStringConnection As String, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As Integer
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim MyProcedure As String = "prc_TBLRUOLI_GENERATI_IU"

    '    Try
    '        If (Not (cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand = cmdMyCommandOut
    '        Else
    '            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.Parameters.Clear()
    '        Select Case DBOperation
    '            Case Utility.Costanti.AZIONE_NEW
    '                cmdMyCommand.Parameters.AddWithValue("@IDFLUSSO", oMyRuolo.IdFlusso)
    '                cmdMyCommand.Parameters.AddWithValue("@IDENTE", oMyRuolo.sEnte)
    '                cmdMyCommand.Parameters.AddWithValue("@ANNO", oMyRuolo.sAnno)
    '                cmdMyCommand.Parameters.AddWithValue("@NOME_RUOLO", oMyRuolo.sNomeRuolo)
    '                cmdMyCommand.Parameters.AddWithValue("@DESCRIZIONE", oMyRuolo.sDescrRuolo)
    '                cmdMyCommand.Parameters.AddWithValue("@TIPOTASSAZIONE", oMyRuolo.sTipoCalcolo)
    '                cmdMyCommand.Parameters.AddWithValue("@TIPO_RUOLO", oMyRuolo.sTipoRuolo)
    '                cmdMyCommand.Parameters.AddWithValue("@NUMERO_RUOLO", oMyRuolo.nNumeroRuolo)
    '                cmdMyCommand.Parameters.AddWithValue("@TOT_ARTICOLI", oMyRuolo.nArticoli)
    '                cmdMyCommand.Parameters.AddWithValue("@TOT_CONTRIBUENTI", oMyRuolo.nContribuenti)
    '                cmdMyCommand.Parameters.AddWithValue("@TOT_AVVISI", oMyRuolo.nAvvisi)
    '                cmdMyCommand.Parameters.AddWithValue("@TOT_SCARTI", oMyRuolo.nScarti)
    '                cmdMyCommand.Parameters.AddWithValue("@TOT_MQ", oMyRuolo.nMQ)
    '                cmdMyCommand.Parameters.AddWithValue("@TOT_IMPORTO", oMyRuolo.ImpArticoli)
    '                cmdMyCommand.Parameters.AddWithValue("@TOT_IMPORTO_RIDUZIONI", oMyRuolo.ImpRiduzione)
    '                cmdMyCommand.Parameters.AddWithValue("@TOT_IMPORTO_DETASSAZIONI", oMyRuolo.ImpDetassazione)
    '                cmdMyCommand.Parameters.AddWithValue("@TOT_IMPORTO_NETTO", oMyRuolo.ImpNetto)
    '                cmdMyCommand.Parameters.AddWithValue("@TOT_IMPORTO_SANZIONI", 0)
    '                cmdMyCommand.Parameters.AddWithValue("@TOT_IMPORTO_INTERESSI", 0)
    '                cmdMyCommand.Parameters.AddWithValue("@TOT_IMPORTO_SPESE_NOTIFICA", 0)
    '                cmdMyCommand.Parameters.AddWithValue("@TOT_IMPORTO_PF", oMyRuolo.ImpPF)
    '                cmdMyCommand.Parameters.AddWithValue("@TOT_IMPORTO_PV", oMyRuolo.ImpPV)
    '                cmdMyCommand.Parameters.AddWithValue("@TOT_CARTELLATO", oMyRuolo.ImpAvvisi)
    '                cmdMyCommand.Parameters.AddWithValue("@TASSAZIONEMINIMA", oMyRuolo.nTassazioneMinima)
    '                cmdMyCommand.Parameters.AddWithValue("@IMPORTO_MINIMO", oMyRuolo.ImpMinimo)
    '                cmdMyCommand.Parameters.AddWithValue("@DATA_CREAZIONE", oMyRuolo.tDataCreazione)
    '                cmdMyCommand.Parameters.AddWithValue("@DATA_STAMPA_MINUTA", DBNull.Value)
    '                cmdMyCommand.Parameters.AddWithValue("@DATA_OKMINUTA", DBNull.Value)
    '                cmdMyCommand.Parameters.AddWithValue("@DATA_CARTELLAZIONE", DBNull.Value)
    '                cmdMyCommand.Parameters.AddWithValue("@DATA_ESTRAZIONE_290", DBNull.Value)
    '                cmdMyCommand.Parameters.AddWithValue("@DATA_ESTRAZIONE_POSTEL", DBNull.Value)
    '                cmdMyCommand.Parameters.AddWithValue("@DATA_ELABDOC", DBNull.Value)
    '                cmdMyCommand.Parameters.AddWithValue("@DATA_OKDOCUMENTI", DBNull.Value)
    '                cmdMyCommand.Parameters.AddWithValue("@DATA_CANCELLAZIONE", DBNull.Value)
    '                cmdMyCommand.Parameters.AddWithValue("@OPERATORE", oMyRuolo.sOperatore)
    '                cmdMyCommand.Parameters.AddWithValue("@TOT_IMPORTO_PM", oMyRuolo.impPM)
    '                cmdMyCommand.Parameters.AddWithValue("@TOT_IMPORTO_PC", oMyRuolo.impPC)
    '                cmdMyCommand.Parameters.AddWithValue("@TOT_ADDIZIONALI", 0)
    '                cmdMyCommand.Parameters.AddWithValue("@PERCENTUALE_TARIFFE", oMyRuolo.PercentTariffe)
    '                'MyDBEngine.AddParameter("@SOGLIARATE", oMyRuolo.SOGLIARATE)
    '                cmdMyCommand.Parameters.AddWithValue("@SOGLIARATE", 0)
    '                If oMyRuolo.tDataInizioConf = DateTime.MinValue Then
    '                    oMyRuolo.tDataInizioConf = DateTime.MaxValue
    '                End If
    '                cmdMyCommand.Parameters.AddWithValue("@INIZIOCONF", oMyRuolo.tDataInizioConf)
    '                cmdMyCommand.Parameters.AddWithValue("@FINECONF", oMyRuolo.tDataFineConf)
    '                '*** 201809 Bollettazione Vigliano in OPENgov ***
    '                cmdMyCommand.Parameters.AddWithValue("@TIPOGENERAZIONE", oMyRuolo.TipoGenerazione)
    '                MyProcedure = "prc_TBLRUOLI_GENERATI_IU"
    '                cmdMyCommand.CommandText = MyProcedure
    '                cmdMyCommand.Parameters("@IDFLUSSO").Direction = ParameterDirection.InputOutput
    '                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '                cmdMyCommand.ExecuteNonQuery()
    '                oMyRuolo.IdFlusso = cmdMyCommand.Parameters("@IDFLUSSO").Value
    '            Case Utility.Costanti.AZIONE_UPDATE
    '                cmdMyCommand.Parameters.AddWithValue("@IDFLUSSO", oMyRuolo.IdFlusso)
    '                cmdMyCommand.Parameters.AddWithValue("@IDENTE", oMyRuolo.sEnte)
    '                cmdMyCommand.Parameters.AddWithValue("@ANNO", oMyRuolo.sAnno)
    '                cmdMyCommand.Parameters.AddWithValue("@NOME_RUOLO", oMyRuolo.sNomeRuolo)
    '                cmdMyCommand.Parameters.AddWithValue("@DESCRIZIONE", oMyRuolo.sDescrRuolo)
    '                cmdMyCommand.Parameters.AddWithValue("@TIPOTASSAZIONE", oMyRuolo.sTipoCalcolo)
    '                cmdMyCommand.Parameters.AddWithValue("@TIPO_RUOLO", oMyRuolo.sTipoRuolo)
    '                cmdMyCommand.Parameters.AddWithValue("@NUMERO_RUOLO", oMyRuolo.nNumeroRuolo)
    '                cmdMyCommand.Parameters.AddWithValue("@TOT_ARTICOLI", oMyRuolo.nArticoli)
    '                cmdMyCommand.Parameters.AddWithValue("@TOT_CONTRIBUENTI", oMyRuolo.nContribuenti)
    '                cmdMyCommand.Parameters.AddWithValue("@TOT_AVVISI", oMyRuolo.nAvvisi)
    '                cmdMyCommand.Parameters.AddWithValue("@TOT_SCARTI", oMyRuolo.nScarti)
    '                cmdMyCommand.Parameters.AddWithValue("@TOT_MQ", oMyRuolo.nMQ)
    '                cmdMyCommand.Parameters.AddWithValue("@TOT_IMPORTO", oMyRuolo.ImpArticoli)
    '                cmdMyCommand.Parameters.AddWithValue("@TOT_IMPORTO_RIDUZIONI", oMyRuolo.ImpRiduzione)
    '                cmdMyCommand.Parameters.AddWithValue("@TOT_IMPORTO_DETASSAZIONI", oMyRuolo.ImpDetassazione)
    '                cmdMyCommand.Parameters.AddWithValue("@TOT_IMPORTO_NETTO", oMyRuolo.ImpNetto)
    '                cmdMyCommand.Parameters.AddWithValue("@TOT_IMPORTO_PF", oMyRuolo.ImpPF)
    '                cmdMyCommand.Parameters.AddWithValue("@TOT_IMPORTO_PV", oMyRuolo.ImpPV)
    '                cmdMyCommand.Parameters.AddWithValue("@TOT_CARTELLATO", oMyRuolo.ImpAvvisi)
    '                cmdMyCommand.Parameters.AddWithValue("@TASSAZIONEMINIMA", oMyRuolo.nTassazioneMinima)
    '                cmdMyCommand.Parameters.AddWithValue("@IMPORTO_MINIMO", oMyRuolo.ImpMinimo)
    '                cmdMyCommand.Parameters.AddWithValue("@DATA_CREAZIONE", oMyRuolo.tDataCreazione)
    '                cmdMyCommand.Parameters.AddWithValue("@TOT_IMPORTO_SANZIONI", 0)
    '                cmdMyCommand.Parameters.AddWithValue("@TOT_IMPORTO_INTERESSI", 0)
    '                cmdMyCommand.Parameters.AddWithValue("@TOT_IMPORTO_SPESE_NOTIFICA", 0)
    '                cmdMyCommand.Parameters.AddWithValue("@DATA_STAMPA_MINUTA", oMyRuolo.tDataStampaMinuta)
    '                cmdMyCommand.Parameters.AddWithValue("@DATA_OKMINUTA", oMyRuolo.tDataOKMinuta)
    '                cmdMyCommand.Parameters.AddWithValue("@DATA_CARTELLAZIONE", oMyRuolo.tDataCartellazione)
    '                cmdMyCommand.Parameters.AddWithValue("@DATA_ESTRAZIONE_290", DBNull.Value)
    '                cmdMyCommand.Parameters.AddWithValue("@DATA_ESTRAZIONE_POSTEL", DBNull.Value)
    '                cmdMyCommand.Parameters.AddWithValue("@DATA_ELABDOC", oMyRuolo.tDataElabDoc)
    '                cmdMyCommand.Parameters.AddWithValue("@DATA_OKDOCUMENTI", oMyRuolo.tDataOKDoc)
    '                cmdMyCommand.Parameters.AddWithValue("@DATA_CANCELLAZIONE", DBNull.Value)
    '                cmdMyCommand.Parameters.AddWithValue("@TOT_IMPORTO_PM", oMyRuolo.impPM)
    '                cmdMyCommand.Parameters.AddWithValue("@TOT_IMPORTO_PC", oMyRuolo.impPC)
    '                cmdMyCommand.Parameters.AddWithValue("@TOT_ADDIZIONALI", 0)
    '                cmdMyCommand.Parameters.AddWithValue("@PERCENTUALE_TARIFFE", oMyRuolo.PercentTariffe)
    '                'MyDBEngine.AddParameter("@SOGLIARATE", oMyRuolo.SOGLIARATE)
    '                cmdMyCommand.Parameters.AddWithValue("@SOGLIARATE", 0)
    '                cmdMyCommand.Parameters.AddWithValue("@OPERATORE", oMyRuolo.sOperatore)
    '                If oMyRuolo.tDataInizioConf = DateTime.MinValue Then
    '                    oMyRuolo.tDataInizioConf = DateTime.MaxValue
    '                End If
    '                cmdMyCommand.Parameters.AddWithValue("@INIZIOCONF", oMyRuolo.tDataInizioConf)
    '                cmdMyCommand.Parameters.AddWithValue("@FINECONF", oMyRuolo.tDataFineConf)
    '                '*** 201809 Bollettazione Vigliano in OPENgov ***
    '                cmdMyCommand.Parameters.AddWithValue("@TIPOGENERAZIONE", oMyRuolo.TipoGenerazione)
    '                MyProcedure = "prc_TBLRUOLI_GENERATI_IU"
    '                cmdMyCommand.CommandText = MyProcedure
    '                cmdMyCommand.Parameters("@IDFLUSSO").Direction = ParameterDirection.InputOutput
    '                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '                cmdMyCommand.ExecuteNonQuery()
    '                oMyRuolo.IdFlusso = cmdMyCommand.Parameters("@IDFLUSSO").Value
    '            Case Utility.Costanti.AZIONE_DELETE
    '                cmdMyCommand.Parameters.AddWithValue("@IDFLUSSO", oMyRuolo.IdFlusso)
    '                MyProcedure = "prc_TBLRUOLI_GENERATI_D"
    '                cmdMyCommand.CommandText = MyProcedure
    '                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '                cmdMyCommand.ExecuteNonQuery()
    '        End Select

    '        Return oMyRuolo.IdFlusso
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestRuolo.SetRuolo.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return 0
    '    Finally
    '        If (cmdMyCommandOut Is Nothing) Then
    '            cmdMyCommand.Dispose()
    '            cmdMyCommand.Connection.Close()
    '        End If
    '    End Try
    'End Function
    '** ***
    Public Function GetNameRuolo(ByVal sNameEnte As String, ByVal sNameAnno As String, ByVal sNameTipo As String, ByVal sNameData As String) As String
        Try
            Dim sNameRuolo As String

            sNameRuolo = sNameEnte.PadLeft(6, "0") & sNameAnno.PadLeft(4, "0") & sNameTipo.PadLeft(1, "0") & sNameData.PadLeft(15, "0")

            Return sNameRuolo
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestRuolo.GetNameRuolo.errore: ", Err)
            Return 0
        End Try
    End Function

    Public Function GetNumeroRuolo(ByVal sGetEnte As String, ByVal sGetTipoRuolo As String, ByVal sGetAnno As String) As Integer
        Dim nProgressivo As Integer = 0
        nProgressivo += 1

        Return nProgressivo
    End Function

    '*** 20131104 - TARES ***
    Public Function GetDescrizioneRuolo(ByVal TipoTassazione As String, ByVal sGetTipoRuolo As String, ByVal sGetTipoElabRuolo As String, ByVal nGetProgRuolo As String, ByVal sGetAnno As String) As String
        Dim sDescrRuolo As String = ""

        Try
            sDescrRuolo = "RUOLO " & TipoTassazione & " " & sGetTipoRuolo.ToUpper
            If sGetTipoElabRuolo = "A" Then
                sDescrRuolo += " AUTOMATICO"
            ElseIf sGetTipoElabRuolo = "M" Then
                sDescrRuolo += " MANUALE"
            End If
            sDescrRuolo += " PER L'ANNO " & sGetAnno
            If nGetProgRuolo > 0 Then
                sDescrRuolo += " N.PROGRESSIVO " & nGetProgRuolo
            End If
            sDescrRuolo += " CREATO IL " & Format(Now, "dd/MM/yyyy hh:mm:ss")

            Return sDescrRuolo
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestRuolo.GetDescrizioneRuolo.errore: ", Err)
            Return ""
        End Try
    End Function
    '*** 20181011 Dal/Al Conferimenti ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sIdEnte"></param>
    ''' <param name="nIdRuolo"></param>
    ''' <param name="sAnno"></param>
    ''' <param name="sTipoRuolo"></param>
    ''' <param name="nTipoEstraz">{0=in elaborazione, 1=elaborazione già terminata}</param>
    ''' <param name="bGetDetAvvisi"></param>
    ''' <param name="oMyConto"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Analisi eventi</em>
    ''' </revision>
    ''' </revisionHistory>
    Public Function GetRuolo(myStringConnection As String, ByVal sIdEnte As String, ByVal nIdRuolo As Integer, ByVal sAnno As String, ByVal sTipoRuolo As String, ByVal nTipoEstraz As Integer, ByVal bGetDetAvvisi As Boolean, ByVal oMyConto As OPENUtility.objContoCorrente) As ObjRuolo()
        Dim oMyRuolo As New ObjRuolo
        Dim nList As Integer = -1
        Dim oListRuolo() As ObjRuolo = Nothing
        Dim FncAvvisi As New GestAvviso
        Dim FncRate As New GestRata
        Dim FncAddiz As New GestAddizionali
        Dim sSQL As String = ""
        Dim myDataView As New DataView
        Dim nMyReturn As Integer = -1

        Try
            Using ctx As New DBModel(ConstSession.DBType, myStringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetElaborazioniEffettuate", "IDENTE", "IDFLUSSO", "ANNO", "TIPO_RUOLO", "TIPOESTRAZ")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", sIdEnte) _
                            , ctx.GetParam("IDFLUSSO", nIdRuolo) _
                            , ctx.GetParam("ANNO", sAnno) _
                            , ctx.GetParam("TIPO_RUOLO", sTipoRuolo) _
                            , ctx.GetParam("TIPOESTRAZ", nTipoEstraz)
                        )
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestRuolo.GetRuolo.erroreQuery: ", ex)
                    Return Nothing
                Finally
                    ctx.Dispose()
                End Try
                For Each myRow As DataRowView In myDataView
                    oMyRuolo = New ObjRuolo
                    oMyRuolo.IdFlusso = StringOperation.FormatInt(myRow("idflusso"))
                    oMyRuolo.sEnte = StringOperation.FormatString(myRow("idente"))
                    oMyRuolo.sDescrRuolo = StringOperation.FormatString(myRow("descrizione"))
                    oMyRuolo.sNomeRuolo = StringOperation.FormatString(myRow("nome_ruolo"))
                    oMyRuolo.sAnno = StringOperation.FormatString(myRow("anno"))
                    oMyRuolo.sTipoRuolo = StringOperation.FormatString(myRow("tipo_ruolo"))
                    oMyRuolo.TipoMQ = StringOperation.FormatString(myRow("tipomq"))
                    oMyRuolo.HasMaggiorazione = StringOperation.FormatBool(myRow("hasmaggiorazione"))
                    oMyRuolo.HasConferimenti = StringOperation.FormatBool(myRow("hasconferimenti"))
                    oMyRuolo.sTipoCalcolo = StringOperation.FormatString(myRow("tipotassazione"))
                    oMyRuolo.sDescrTipoRuolo = StringOperation.FormatString(myRow("descrtiporuolo"))
                    oMyRuolo.nContribuenti = StringOperation.FormatInt(myRow("tot_contribuenti"))
                    oMyRuolo.nArticoli = StringOperation.FormatInt(myRow("tot_articoli"))
                    oMyRuolo.nAvvisi = StringOperation.FormatInt(myRow("tot_avvisi"))
                    oMyRuolo.nScarti = StringOperation.FormatInt(myRow("tot_scarti"))
                    oMyRuolo.nMQ = StringOperation.FormatDouble(myRow("tot_mq"))
                    oMyRuolo.PercentTariffe = StringOperation.FormatDouble(myRow("Percentuale_Tariffe"))
                    oMyRuolo.ImpArticoli = StringOperation.FormatDouble(myRow("tot_importo"))
                    oMyRuolo.ImpDetassazione = StringOperation.FormatDouble(myRow("tot_importo_detassazioni"))
                    oMyRuolo.ImpNetto = StringOperation.FormatDouble(myRow("tot_importo_netto"))
                    oMyRuolo.ImpRiduzione = StringOperation.FormatDouble(myRow("tot_importo_riduzioni"))
                    oMyRuolo.ImpPF = StringOperation.FormatDouble(myRow("tot_importo_pf"))
                    oMyRuolo.ImpPV = StringOperation.FormatDouble(myRow("tot_importo_pv"))
                    oMyRuolo.impPM = StringOperation.FormatDouble(myRow("tot_importo_pm"))
                    oMyRuolo.impPC = StringOperation.FormatDouble(myRow("tot_importo_pc"))
                    oMyRuolo.impAddizionali = StringOperation.FormatDouble(myRow("tot_addizionali"))
                    oMyRuolo.ImpAvvisi = StringOperation.FormatDouble(myRow("tot_cartellato"))
                    oMyRuolo.tDataCreazione = StringOperation.FormatDateTime(myRow("data_creazione"))
                    oMyRuolo.nNumeroRuolo = StringOperation.FormatInt(myRow("NUMERO_RUOLO"))
                    oMyRuolo.ImpMinimo = StringOperation.FormatDouble(myRow("importo_minimo"))
                    oMyRuolo.nTassazioneMinima = StringOperation.FormatInt(myRow("tassazioneminima"))
                    oMyRuolo.tDataInizioConf = StringOperation.FormatDateTime(myRow("data_inizio_conferimenti"))
                    oMyRuolo.tDataFineConf = StringOperation.FormatDateTime(myRow("data_fine_conferimenti"))
                    oMyRuolo.tDataStampaMinuta = StringOperation.FormatDateTime(myRow("data_stampa_minuta"))
                    oMyRuolo.tDataCartellazione = StringOperation.FormatDateTime(myRow("data_cartellazione"))
                    oMyRuolo.tDataOKMinuta = StringOperation.FormatDateTime(myRow("data_okminuta"))
                    oMyRuolo.tDataElabDoc = StringOperation.FormatDateTime(myRow("data_elabdoc"))
                    oMyRuolo.tDataOKDoc = StringOperation.FormatDateTime(myRow("data_okdocumenti"))
                    '*** 201809 Bollettazione Vigliano in OPENgov ***
                    oMyRuolo.TipoGenerazione = StringOperation.FormatString(myRow("TIPOGENERAZIONE"))
                    If bGetDetAvvisi = True Then
                        oMyRuolo.oAvvisi = FncAvvisi.GetAvviso(ConstSession.StringConnection, -1, oMyRuolo.sEnte, oMyRuolo.IdFlusso, "", "", "", "", "", "", "", "", "", True, False, False, False, "", -1, Nothing)
                    End If
                    '*** 20130308 - devo memorizzare il conto corrente altrimenti non sarà valorizzato nella codeline e nel codice a barre ***
                    oMyRuolo.oRate = FncRate.GetRateConfigurate(ConstSession.StringConnection, oMyRuolo.IdFlusso, oMyConto)
                    '*** ***
                    oMyRuolo.oAddizionali = FncAddiz.GetAddizionale(ConstSession.StringConnection, oMyRuolo.sEnte, oMyRuolo.sAnno, "", oMyRuolo.sTipoRuolo)
                    nList += 1
                    ReDim Preserve oListRuolo(nList)
                    oListRuolo(nList) = oMyRuolo
                Next
            End Using
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestRuolo.GetRuolo.errore: ", Err)
            Return Nothing
        Finally
            myDataView.Dispose()
        End Try
        Return oListRuolo
    End Function
    'Public Function GetRuolo(ByVal sIdEnte As String, ByVal nIdRuolo As Integer, ByVal sAnno As String, ByVal sTipoRuolo As String, ByVal nTipoEstraz As Integer, ByVal bGetDetAvvisi As Boolean, ByVal oMyConto As OPENUtility.objContoCorrente, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As ObjRuolo()
    '    'nTipoEstraz {0=in elaborazione, 1=elaborazione già terminata}
    '    Dim oMyRuolo As New ObjRuolo
    '    Dim nList As Integer = -1
    '    Dim oListRuolo() As ObjRuolo = Nothing
    '    Dim FncAvvisi As New GestAvviso
    '    Dim FncRate As New GestRata
    '    Dim FncAddiz As New GestAddizionali
    '    Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
    '    Dim dtMyDati As New DataTable()
    '    Dim myRow As DataRow
    '    Dim myAdapter As New SqlClient.SqlDataAdapter

    '    Try
    '        If (Not (cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand = cmdMyCommandOut
    '        Else
    '            cmdMyCommand = New SqlClient.SqlCommand
    '            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '            cmdMyCommand.Connection.Open()
    '            cmdMyCommand.CommandTimeout = 0
    '        End If

    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.AddWithValue("@IdEnte", sIdEnte)
    '        cmdMyCommand.Parameters.AddWithValue("@Anno", sAnno)
    '        cmdMyCommand.Parameters.AddWithValue("@IDFLUSSO", nIdRuolo)
    '        cmdMyCommand.Parameters.AddWithValue("@TIPO_RUOLO", sTipoRuolo)
    '        cmdMyCommand.Parameters.AddWithValue("@TipoEstraz", nTipoEstraz)
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = "prc_GetElaborazioniEffettuate"
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(dtMyDati)
    '        For Each myRow In dtMyDati.Rows
    '            oMyRuolo = New ObjRuolo
    '            oMyRuolo.IdFlusso = StringOperation.FormatInt(myRow("idflusso"))
    '            oMyRuolo.sEnte = StringOperation.FormatString(myRow("idente"))
    '            oMyRuolo.sDescrRuolo = StringOperation.FormatString(myRow("descrizione"))
    '            oMyRuolo.sNomeRuolo = StringOperation.FormatString(myRow("nome_ruolo"))
    '            oMyRuolo.sAnno = StringOperation.FormatString(myRow("anno"))
    '            oMyRuolo.sTipoRuolo = StringOperation.FormatString(myRow("tipo_ruolo"))
    '            oMyRuolo.TipoMQ = StringOperation.FormatString(myRow("tipomq"))
    '            oMyRuolo.HasMaggiorazione = StringOperation.FormatBool(myRow("hasmaggiorazione"))
    '            oMyRuolo.HasConferimenti = StringOperation.FormatBool(myRow("hasconferimenti"))
    '            oMyRuolo.sTipoCalcolo = StringOperation.FormatString(myRow("tipotassazione"))
    '            oMyRuolo.sDescrTipoRuolo = StringOperation.FormatString(myRow("descrtiporuolo"))
    '            oMyRuolo.nContribuenti = StringOperation.FormatInt(myRow("tot_contribuenti"))
    '            oMyRuolo.nArticoli = StringOperation.FormatInt(myRow("tot_articoli"))
    '            oMyRuolo.nAvvisi = StringOperation.FormatInt(myRow("tot_avvisi"))
    '            oMyRuolo.nScarti = StringOperation.FormatInt(myRow("tot_scarti"))
    '            oMyRuolo.nMQ = StringOperation.FormatDouble(myRow("tot_mq"))
    '            oMyRuolo.PercentTariffe = StringOperation.FormatDouble(myRow("Percentuale_Tariffe"))
    '            oMyRuolo.ImpArticoli = StringOperation.FormatDouble(myRow("tot_importo"))
    '            oMyRuolo.ImpDetassazione = StringOperation.FormatDouble(myRow("tot_importo_detassazioni"))
    '            oMyRuolo.ImpNetto = StringOperation.FormatDouble(myRow("tot_importo_netto"))
    '            oMyRuolo.ImpRiduzione = StringOperation.FormatDouble(myRow("tot_importo_riduzioni"))
    '            oMyRuolo.ImpPF = StringOperation.FormatDouble(myRow("tot_importo_pf"))
    '            oMyRuolo.ImpPV = StringOperation.FormatDouble(myRow("tot_importo_pv"))
    '            oMyRuolo.impPM = StringOperation.FormatDouble(myRow("tot_importo_pm"))
    '            oMyRuolo.impPC = StringOperation.FormatDouble(myRow("tot_importo_pc"))
    '            oMyRuolo.impAddizionali = StringOperation.FormatDouble(myRow("tot_addizionali"))
    '            oMyRuolo.ImpAvvisi = StringOperation.FormatDouble(myRow("tot_cartellato"))
    '            oMyRuolo.tDataCreazione = StringOperation.FormatDateTime(myRow("data_creazione"))
    '            oMyRuolo.nNumeroRuolo = StringOperation.FormatInt(myRow("NUMERO_RUOLO"))
    '            oMyRuolo.ImpMinimo = StringOperation.FormatDouble(myRow("importo_minimo"))
    '            oMyRuolo.nTassazioneMinima = StringOperation.FormatInt(myRow("tassazioneminima"))
    '            oMyRuolo.tDataInizioConf = StringOperation.FormatDateTime(myRow("data_inizio_conferimenti"))
    '            oMyRuolo.tDataFineConf = StringOperation.FormatDateTime(myRow("data_fine_conferimenti"))
    '            oMyRuolo.tDataStampaMinuta = StringOperation.FormatDateTime(myRow("data_stampa_minuta"))
    '            oMyRuolo.tDataCartellazione = StringOperation.FormatDateTime(myRow("data_cartellazione"))
    '            oMyRuolo.tDataOKMinuta = StringOperation.FormatDateTime(myRow("data_okminuta"))
    '            oMyRuolo.tDataElabDoc = StringOperation.FormatDateTime(myRow("data_elabdoc"))
    '            oMyRuolo.tDataOKDoc = StringOperation.FormatDateTime(myRow("data_okdocumenti"))
    '            '*** 201809 Bollettazione Vigliano in OPENgov ***
    '            oMyRuolo.TipoGenerazione = StringOperation.FormatString(myRow("TIPOGENERAZIONE"))
    '            If bGetDetAvvisi = True Then
    '                'oMyRuolo.oAvvisi = FncAvvisi.GetAvviso(-1, oMyRuolo.sEnte, oMyRuolo.IdFlusso, "", "", "", "", "", "", "", "", "", True, False, False, False, MyDBEngine)
    '                oMyRuolo.oAvvisi = FncAvvisi.GetAvviso(ConstSession.StringConnection, -1, oMyRuolo.sEnte, oMyRuolo.IdFlusso, "", "", "", "", "", "", "", "", "", True, False, False, False, "", -1, Nothing)
    '            End If
    '            '*** 20130308 - devo memorizzare il conto corrente altrimenti non sarà valorizzato nella codeline e nel codice a barre ***
    '            oMyRuolo.oRate = FncRate.GetRateConfigurate(oMyRuolo.IdFlusso, oMyConto, cmdMyCommand)
    '            '*** ***
    '            oMyRuolo.oAddizionali = FncAddiz.GetAddizionale(ConstSession.StringConnection, oMyRuolo.sEnte, oMyRuolo.sAnno, "", oMyRuolo.sTipoRuolo, cmdMyCommand)
    '            nList += 1
    '            ReDim Preserve oListRuolo(nList)
    '            oListRuolo(nList) = oMyRuolo
    '        Next

    '        Return oListRuolo
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestRuolo.GetRuolo.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return Nothing
    '    Finally
    '        dtMyDati.Dispose()
    '        If (cmdMyCommandOut Is Nothing) Then
    '            cmdMyCommand.Dispose()
    '            cmdMyCommand.Connection.Close()
    '        End If
    '    End Try
    'End Function
    Public Function GetMinuta(ByVal nIdRuolo As Integer) As DataView
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dtMyDati As New DataTable()
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_GetMinutaUniqueRow"
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSO", SqlDbType.Int)).Value = nIdRuolo
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(dtMyDati)
            Return dtMyDati.DefaultView
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestRuolo.GetMinuta.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        Finally
            myAdapter.Dispose()
            dtMyDati.Dispose()
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myStringConnection"></param>
    ''' <param name="IdEnte"></param>
    ''' <param name="nIdRuolo"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Esportazione completa dati</em>
    ''' </revision>
    ''' </revisionHistory>
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
            Log.Debug(IdEnte + " - OPENgovTIA.GestRuolo.GetMinutaRate.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        Finally
            myAdapter.Dispose()
            dtMyDati.Dispose()
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function
    'Public Function GetMinutaRate(ByVal nIdRuolo As Integer) As DataView
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim dtMyDati As New DataTable()
    '    Dim cmdMyCommand As New SqlClient.SqlCommand

    '    Try
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = "prc_GetMinutaXStampatoreRate"
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSO", SqlDbType.Int)).Value = nIdRuolo
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(dtMyDati)
    '        Return dtMyDati.DefaultView
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestRuolo.GetMinutaRate.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return Nothing
    '    Finally
    '        myAdapter.Dispose()
    '        dtMyDati.Dispose()
    '        cmdMyCommand.Dispose()
    '        cmdMyCommand.Connection.Close()
    '    End Try
    'End Function
    Public Function GetMinutaAvvisi(ByVal nIdRuolo As Integer) As DataView
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dtMyDati As New DataTable()
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_GetMinutaAvvisi"
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSO", SqlDbType.Int)).Value = nIdRuolo
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(dtMyDati)
            Return dtMyDati.DefaultView
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestRuolo.GetMinutaAvvisi.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        Finally
            myAdapter.Dispose()
            dtMyDati.Dispose()
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myStringConnection"></param>
    ''' <param name="IdEnte"></param>
    ''' <param name="nIdRuolo"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="09/2018">
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
    Public Function GetRuoloMinuta(myStringConnection As String, IdEnte As String, ByVal nIdRuolo As Integer) As ObjAvviso()
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dtMyDati As New DataTable()
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim oMyAvviso As New ObjAvviso
        Dim oListAvvisi As New Generic.List(Of ObjAvviso)
        Dim oMyArticolo As New ObjArticolo
        Dim oListArticoli As New Generic.List(Of ObjArticolo)

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_GetMinuta"
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.AddWithValue("@IDENTE", IdEnte)
            cmdMyCommand.Parameters.AddWithValue("@IDFLUSSO", nIdRuolo)
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(dtMyDati)
            For Each dtMyRow As DataRow In dtMyDati.Rows
                If oMyAvviso.ID <> Utility.StringOperation.FormatInt(dtMyRow("ID")) Then
                    If oListArticoli.Count > 0 Then
                        oMyAvviso.oArticoli = oListArticoli.ToArray
                        oListAvvisi.Add(oMyAvviso)
                    End If
                    oListArticoli = New Generic.List(Of ObjArticolo)
                    oMyAvviso = New ObjAvviso
                    oMyAvviso.ID = Utility.StringOperation.FormatInt(dtMyRow("id"))
                    oMyAvviso.IdContribuente = Utility.StringOperation.FormatInt(dtMyRow("IdContribuente"))
                    oMyAvviso.sCognome = Utility.StringOperation.FormatString(dtMyRow("COGNOME"))
                    oMyAvviso.sNome = Utility.StringOperation.FormatString(dtMyRow("NOME"))
                    oMyAvviso.sCodFiscale = Utility.StringOperation.FormatString(dtMyRow("cfpiva"))
                    oMyAvviso.tDataNascita = Utility.StringOperation.FormatDateTime(dtMyRow("data_nascita"))
                    oMyAvviso.sComuneNascita = Utility.StringOperation.FormatString(dtMyRow("comune_nascita"))
                    oMyAvviso.sPVNascita = Utility.StringOperation.FormatString(dtMyRow("pv_nascita"))
                    oMyAvviso.sSesso = Utility.StringOperation.FormatString(dtMyRow("sesso"))
                    oMyAvviso.sIndirizzoRes = Utility.StringOperation.FormatString(dtMyRow("via_res"))
                    oMyAvviso.sCivicoRes = Utility.StringOperation.FormatString(dtMyRow("civico_res"))
                    oMyAvviso.sCAPRes = Utility.StringOperation.FormatString(dtMyRow("cap_res"))
                    oMyAvviso.sComuneRes = Utility.StringOperation.FormatString(dtMyRow("comune_res"))
                    oMyAvviso.sProvRes = Utility.StringOperation.FormatString(dtMyRow("provincia_res"))
                    oMyAvviso.sNominativoCO = Utility.StringOperation.FormatString(dtMyRow("nominativoco"))
                    oMyAvviso.sIndirizzoCO = Utility.StringOperation.FormatString(dtMyRow("indirizzoco"))
                    oMyAvviso.sCivicoCO = Utility.StringOperation.FormatString(dtMyRow("civicoco"))
                    oMyAvviso.sCAPCO = Utility.StringOperation.FormatString(dtMyRow("capco"))
                    oMyAvviso.sComuneCO = Utility.StringOperation.FormatString(dtMyRow("comuneco"))
                    oMyAvviso.sProvCO = Utility.StringOperation.FormatString(dtMyRow("pvco"))
                    oMyAvviso.sCodiceCartella = Utility.StringOperation.FormatString(dtMyRow("CODICE_CARTELLA"))
                    oMyAvviso.sAnnoRiferimento = Utility.StringOperation.FormatString(dtMyRow("ANNO"))
                    oMyAvviso.impPF = Utility.StringOperation.FormatDouble(dtMyRow("IMPORTO_PF"))
                    oMyAvviso.impPV = Utility.StringOperation.FormatDouble(dtMyRow("IMPORTO_PV"))
                    oMyAvviso.impPC = Utility.StringOperation.FormatDouble(dtMyRow("IMPORTO_PC"))
                    oMyAvviso.impPM = Utility.StringOperation.FormatDouble(dtMyRow("IMPORTO_PM"))
                    Dim oMyVoce As New ObjDetVoci
                    Dim ListVoci As New Generic.List(Of ObjDetVoci)
                    If Utility.StringOperation.FormatDouble(dtMyRow("impeca")) > 0 Then
                        oMyVoce = New ObjDetVoci
                        oMyVoce.impDettaglio = Utility.StringOperation.FormatDouble(dtMyRow("impeca"))
                        oMyVoce.sCapitolo = ObjDetVoci.Capitolo.ECA
                        ListVoci.Add(oMyVoce)
                    End If
                    If Utility.StringOperation.FormatDouble(dtMyRow("impmeca")) > 0 Then
                        oMyVoce = New ObjDetVoci
                        oMyVoce.impDettaglio = Utility.StringOperation.FormatDouble(dtMyRow("impmeca"))
                        oMyVoce.sCapitolo = ObjDetVoci.Capitolo.MECA
                        ListVoci.Add(oMyVoce)
                    End If
                    If Utility.StringOperation.FormatDouble(dtMyRow("impaggioperente")) > 0 Then
                        oMyVoce = New ObjDetVoci
                        oMyVoce.impDettaglio = Utility.StringOperation.FormatDouble(dtMyRow("impaggioperente"))
                        oMyVoce.sCapitolo = ObjDetVoci.Capitolo.AggioEnte
                        ListVoci.Add(oMyVoce)
                    End If
                    If Utility.StringOperation.FormatDouble(dtMyRow("impProvincialePerEnte")) > 0 Then
                        oMyVoce = New ObjDetVoci
                        oMyVoce.impDettaglio = Utility.StringOperation.FormatDouble(dtMyRow("impProvincialePerEnte"))
                        oMyVoce.sCapitolo = ObjDetVoci.Capitolo.ProvincialeEnte
                        ListVoci.Add(oMyVoce)
                    End If
                    If Utility.StringOperation.FormatDouble(dtMyRow("addprov")) > 0 Then
                        oMyVoce = New ObjDetVoci
                        oMyVoce.impDettaglio = Utility.StringOperation.FormatDouble(dtMyRow("addprov"))
                        oMyVoce.sCapitolo = ObjDetVoci.Capitolo.Provinciale
                        ListVoci.Add(oMyVoce)
                    End If
                    If Utility.StringOperation.FormatDouble(dtMyRow("impspesenotifica")) > 0 Then
                        oMyVoce = New ObjDetVoci
                        oMyVoce.impDettaglio = Utility.StringOperation.FormatDouble(dtMyRow("impspesenotifica"))
                        oMyVoce.sCapitolo = ObjDetVoci.Capitolo.SpeseNotifica
                        ListVoci.Add(oMyVoce)
                    End If
                    If Utility.StringOperation.FormatDouble(dtMyRow("impsanzioni")) > 0 Then
                        oMyVoce = New ObjDetVoci
                        oMyVoce.impDettaglio = Utility.StringOperation.FormatDouble(dtMyRow("impsanzioni"))
                        oMyVoce.sCapitolo = ObjDetVoci.Capitolo.Sanzione
                        ListVoci.Add(oMyVoce)
                    End If
                    oMyAvviso.oDetVoci = ListVoci.ToArray
                    oMyAvviso.impArrotondamento = Utility.StringOperation.FormatDouble(dtMyRow("IMPORTO_ARROTONDAMENTO"))
                    oMyAvviso.impCarico = Utility.StringOperation.FormatDouble(dtMyRow("IMPORTO_CARICO"))
                    If Utility.StringOperation.FormatDouble(dtMyRow("IMPORTO_PRE_SGRAVIO")) > 0 Then
                        oMyAvviso.impPRESgravio = Utility.StringOperation.FormatDouble(dtMyRow("IMPORTO_PRE_SGRAVIO"))
                    End If
                End If
                oMyArticolo = New ObjArticolo
                oMyArticolo.sVia = Utility.StringOperation.FormatString(dtMyRow("UBICAZIONE"))
                oMyArticolo.sCivico = Utility.StringOperation.FormatString(dtMyRow("CIVICO"))
                oMyArticolo.sEsponente = Utility.StringOperation.FormatString(dtMyRow("ESPONENTE"))
                oMyArticolo.sInterno = Utility.StringOperation.FormatString(dtMyRow("INTERNO"))
                oMyArticolo.sFoglio = Utility.StringOperation.FormatString(dtMyRow("FOGLIO"))
                oMyArticolo.sNumero = Utility.StringOperation.FormatString(dtMyRow("NUMERO"))
                oMyArticolo.sSubalterno = Utility.StringOperation.FormatString(dtMyRow("SUBALTERNO"))
                oMyArticolo.TipoPartita = Utility.StringOperation.FormatString(dtMyRow("TIPOPARTITA"))
                oMyArticolo.sDescrCategoria = Utility.StringOperation.FormatString(dtMyRow("CATEGORIA"))
                oMyArticolo.nComponenti = Utility.StringOperation.FormatInt(dtMyRow("NOCCUPANTI"))
                oMyArticolo.nMQ = Utility.StringOperation.FormatDouble(dtMyRow("MQ"))
                oMyArticolo.nBimestri = Utility.StringOperation.FormatInt(dtMyRow("TEMPO"))
                oMyArticolo.impTariffa = Utility.StringOperation.FormatDouble(dtMyRow("TARIFFA"))
                oMyArticolo.impRuolo = Utility.StringOperation.FormatDouble(dtMyRow("IMPORTO"))
                oMyArticolo.impRiduzione = Utility.StringOperation.FormatDouble(dtMyRow("IMPORTO_RIDUZIONI"))
                oMyArticolo.impDetassazione = Utility.StringOperation.FormatDouble(dtMyRow("IMPORTO_DETASSAZIONI"))
                oMyArticolo.impNetto = Utility.StringOperation.FormatDouble(dtMyRow("IMPORTO_NETTO"))
                oMyArticolo.sIdTipoUnita = Utility.StringOperation.FormatString(dtMyRow("TIPOUTENZA"))
                If Utility.StringOperation.FormatString(dtMyRow("RIDUZIONI")) <> "" Then
                    Dim oMyRid As New ObjRidEseApplicati
                    Dim ListRid As New Generic.List(Of ObjRidEseApplicati)
                    oMyRid.sDescrizione = Utility.StringOperation.FormatString(dtMyRow("RIDUZIONI"))
                    ListRid.Add(oMyRid)
                    oMyArticolo.oRiduzioni = ListRid.ToArray
                End If
                oListArticoli.Add(oMyArticolo)
            Next
            oMyAvviso.oArticoli = oListArticoli.ToArray
            oListAvvisi.Add(oMyAvviso)

            Return oListAvvisi.ToArray
        Catch Err As Exception
            Log.Debug(IdEnte + " - OPENgovTIA.GestRuolo.GetRuoloMinuta.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        Finally
            myAdapter.Dispose()
            dtMyDati.Dispose()
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function
    'Public Function GetRuoloMinuta(ByVal nIdRuolo As Integer) As ObjAvviso()
    '    'Dim oMyRuolo As New ObjRuolo
    '    'Dim nList As Integer = -1
    '    'Dim oListRuolo() As ObjRuolo
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim dtMyDati As New DataTable()
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim oMyAvviso As New ObjAvviso
    '    Dim oListAvvisi As New Generic.List(Of ObjAvviso)
    '    Dim oMyArticolo As New ObjArticolo
    '    Dim oListArticoli As New Generic.List(Of ObjArticolo)
    '    'Dim FncDetVoci As New GestDetVoci
    '    'Dim dtMyDati As New DataTable()
    '    'Dim dtMyRow As DataRow

    '    Try
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = "prc_GetMinuta"
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSO", SqlDbType.Int)).Value = nIdRuolo
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(dtMyDati)
    '        For Each dtMyRow As DataRow In dtMyDati.Rows
    '            If oMyAvviso.ID <> stringoperation.formatint(dtMyRow("ID")) Then
    '                If oListArticoli.Count > 0 Then
    '                    oMyAvviso.oArticoli = oListArticoli.ToArray
    '                    oListAvvisi.Add(oMyAvviso)
    '                End If
    '                oListArticoli = New Generic.List(Of ObjArticolo)
    '                oMyAvviso = New ObjAvviso
    '                oMyAvviso.ID = stringoperation.formatint(dtMyRow("id"))
    '                oMyAvviso.IdContribuente = stringoperation.formatint(dtMyRow("IdContribuente"))
    '                If Not IsDBNull(dtMyRow("COGNOME")) Then
    '                    oMyAvviso.sCognome = stringoperation.formatstring(dtMyRow("COGNOME"))
    '                End If
    '                If Not IsDBNull(dtMyRow("NOME")) Then
    '                    oMyAvviso.sNome = stringoperation.formatstring(dtMyRow("NOME"))
    '                End If
    '                If Not IsDBNull(dtMyRow("cfpiva")) Then
    '                    oMyAvviso.sCodFiscale = stringoperation.formatstring(dtMyRow("cfpiva"))
    '                End If
    '                If Not IsDBNull(dtMyRow("data_nascita")) Then
    '                    oMyAvviso.tDataNascita = stringoperation.formatdatetime(dtMyRow("data_nascita"))
    '                End If
    '                If Not IsDBNull(dtMyRow("comune_nascita")) Then
    '                    oMyAvviso.sComuneNascita = stringoperation.formatstring(dtMyRow("comune_nascita"))
    '                End If
    '                If Not IsDBNull(dtMyRow("pv_nascita")) Then
    '                    oMyAvviso.sPVNascita = stringoperation.formatstring(dtMyRow("pv_nascita"))
    '                End If
    '                If Not IsDBNull(dtMyRow("sesso")) Then
    '                    oMyAvviso.sSesso = stringoperation.formatstring(dtMyRow("sesso"))
    '                End If
    '                If Not IsDBNull(dtMyRow("via_res")) Then
    '                    oMyAvviso.sIndirizzoRes = stringoperation.formatstring(dtMyRow("via_res"))
    '                End If
    '                If Not IsDBNull(dtMyRow("civico_res")) Then
    '                    oMyAvviso.sCivicoRes = stringoperation.formatstring(dtMyRow("civico_res"))
    '                End If
    '                If Not IsDBNull(dtMyRow("cap_res")) Then
    '                    oMyAvviso.sCAPRes = stringoperation.formatstring(dtMyRow("cap_res"))
    '                End If
    '                If Not IsDBNull(dtMyRow("comune_res")) Then
    '                    oMyAvviso.sComuneRes = stringoperation.formatstring(dtMyRow("comune_res"))
    '                End If
    '                If Not IsDBNull(dtMyRow("provincia_res")) Then
    '                    oMyAvviso.sProvRes = stringoperation.formatstring(dtMyRow("provincia_res"))
    '                End If
    '                If Not IsDBNull(dtMyRow("nominativoco")) Then
    '                    oMyAvviso.sNominativoCO = stringoperation.formatstring(dtMyRow("nominativoco"))
    '                End If
    '                If Not IsDBNull(dtMyRow("indirizzoco")) Then
    '                    oMyAvviso.sIndirizzoCO = stringoperation.formatstring(dtMyRow("indirizzoco"))
    '                End If
    '                If Not IsDBNull(dtMyRow("civicoco")) Then
    '                    oMyAvviso.sCivicoCO = stringoperation.formatstring(dtMyRow("civicoco"))
    '                End If
    '                If Not IsDBNull(dtMyRow("capco")) Then
    '                    oMyAvviso.sCAPCO = stringoperation.formatstring(dtMyRow("capco"))
    '                End If
    '                If Not IsDBNull(dtMyRow("comuneco")) Then
    '                    oMyAvviso.sComuneCO = stringoperation.formatstring(dtMyRow("comuneco"))
    '                End If
    '                If Not IsDBNull(dtMyRow("pvco")) Then
    '                    oMyAvviso.sProvCO = stringoperation.formatstring(dtMyRow("pvco"))
    '                End If
    '                oMyAvviso.sCodiceCartella = stringoperation.formatstring(dtMyRow("CODICE_CARTELLA"))
    '                If Not IsDBNull(dtMyRow("ANNO")) Then
    '                    oMyAvviso.sAnnoRiferimento = stringoperation.formatstring(dtMyRow("ANNO"))
    '                End If
    '                If Not IsDBNull(dtMyRow("IMPORTO_PF")) Then
    '                    oMyAvviso.impPF = stringoperation.formatdouble(dtMyRow("IMPORTO_PF"))
    '                End If
    '                If Not IsDBNull(dtMyRow("IMPORTO_PV")) Then
    '                    oMyAvviso.impPV = stringoperation.formatdouble(dtMyRow("IMPORTO_PV"))
    '                End If
    '                If Not IsDBNull(dtMyRow("IMPORTO_PC")) Then
    '                    oMyAvviso.impPC = stringoperation.formatdouble(dtMyRow("IMPORTO_PC"))
    '                End If
    '                If Not IsDBNull(dtMyRow("IMPORTO_PM")) Then
    '                    oMyAvviso.impPM = stringoperation.formatdouble(dtMyRow("IMPORTO_PM"))
    '                End If
    '                Dim oMyVoce As New ObjDetVoci
    '                Dim ListVoci As New Generic.List(Of ObjDetVoci)
    '                If Not IsDBNull(dtMyRow("impeca")) Then
    '                    oMyVoce = New ObjDetVoci
    '                    oMyVoce.impDettaglio = stringoperation.formatdouble(dtMyRow("impeca"))
    '                    oMyVoce.sCapitolo = ObjDetVoci.Capitolo.ECA
    '                    ListVoci.Add(oMyVoce)
    '                End If
    '                If Not IsDBNull(dtMyRow("impmeca")) Then
    '                    oMyVoce = New ObjDetVoci
    '                    oMyVoce.impDettaglio = stringoperation.formatdouble(dtMyRow("impmeca"))
    '                    oMyVoce.sCapitolo = ObjDetVoci.Capitolo.MECA
    '                    ListVoci.Add(oMyVoce)
    '                End If
    '                If Not IsDBNull(dtMyRow("impaggioperente")) Then
    '                    oMyVoce = New ObjDetVoci
    '                    oMyVoce.impDettaglio = stringoperation.formatdouble(dtMyRow("impaggioperente"))
    '                    oMyVoce.sCapitolo = ObjDetVoci.Capitolo.AggioEnte
    '                    ListVoci.Add(oMyVoce)
    '                End If
    '                If Not IsDBNull(dtMyRow("impProvincialePerEnte")) Then
    '                    oMyVoce = New ObjDetVoci
    '                    oMyVoce.impDettaglio = stringoperation.formatdouble(dtMyRow("impProvincialePerEnte"))
    '                    oMyVoce.sCapitolo = ObjDetVoci.Capitolo.ProvincialeEnte
    '                    ListVoci.Add(oMyVoce)
    '                End If
    '                If Not IsDBNull(dtMyRow("addprov")) Then
    '                    oMyVoce = New ObjDetVoci
    '                    oMyVoce.impDettaglio = stringoperation.formatdouble(dtMyRow("addprov"))
    '                    oMyVoce.sCapitolo = ObjDetVoci.Capitolo.Provinciale
    '                    ListVoci.Add(oMyVoce)
    '                End If
    '                If Not IsDBNull(dtMyRow("impspesenotifica")) Then
    '                    oMyVoce = New ObjDetVoci
    '                    oMyVoce.impDettaglio = stringoperation.formatdouble(dtMyRow("impspesenotifica"))
    '                    oMyVoce.sCapitolo = ObjDetVoci.Capitolo.SpeseNotifica
    '                    ListVoci.Add(oMyVoce)
    '                End If
    '                If Not IsDBNull(dtMyRow("impsanzioni")) Then
    '                    oMyVoce = New ObjDetVoci
    '                    oMyVoce.impDettaglio = stringoperation.formatdouble(dtMyRow("impsanzioni"))
    '                    oMyVoce.sCapitolo = ObjDetVoci.Capitolo.Sanzione
    '                    ListVoci.Add(oMyVoce)
    '                End If
    '                oMyAvviso.oDetVoci = ListVoci.ToArray
    '                oMyAvviso.impArrotondamento = stringoperation.formatdouble(dtMyRow("IMPORTO_ARROTONDAMENTO"))
    '                oMyAvviso.impCarico = stringoperation.formatdouble(dtMyRow("IMPORTO_CARICO"))
    '                If Not IsDBNull(dtMyRow("IMPORTO_PRE_SGRAVIO")) Then
    '                    oMyAvviso.impPRESgravio = stringoperation.formatdouble(dtMyRow("IMPORTO_PRE_SGRAVIO"))
    '                End If
    '            End If
    '            oMyArticolo = New ObjArticolo
    '            If Not IsDBNull(dtMyRow("UBICAZIONE")) Then
    '                oMyArticolo.sVia = stringoperation.formatstring(dtMyRow("UBICAZIONE"))
    '            End If
    '            If Not IsDBNull(dtMyRow("CIVICO")) Then
    '                oMyArticolo.sCivico = stringoperation.formatstring(dtMyRow("CIVICO"))
    '            End If
    '            If Not IsDBNull(dtMyRow("ESPONENTE")) Then
    '                oMyArticolo.sEsponente = stringoperation.formatstring(dtMyRow("ESPONENTE"))
    '            End If
    '            If Not IsDBNull(dtMyRow("INTERNO")) Then
    '                oMyArticolo.sInterno = stringoperation.formatstring(dtMyRow("INTERNO"))
    '            End If
    '            If Not IsDBNull(dtMyRow("FOGLIO")) Then
    '                oMyArticolo.sFoglio = stringoperation.formatstring(dtMyRow("FOGLIO"))
    '            End If
    '            If Not IsDBNull(dtMyRow("NUMERO")) Then
    '                oMyArticolo.sNumero = stringoperation.formatstring(dtMyRow("NUMERO"))
    '            End If
    '            If Not IsDBNull(dtMyRow("SUBALTERNO")) Then
    '                oMyArticolo.sSubalterno = stringoperation.formatstring(dtMyRow("SUBALTERNO"))
    '            End If
    '            If Not IsDBNull(dtMyRow("TIPOPARTITA")) Then
    '                oMyArticolo.TipoPartita = stringoperation.formatstring(dtMyRow("TIPOPARTITA"))
    '            End If
    '            If Not IsDBNull(dtMyRow("CATEGORIA")) Then
    '                oMyArticolo.sDescrCategoria = stringoperation.formatstring(dtMyRow("CATEGORIA"))
    '            End If
    '            If Not IsDBNull(dtMyRow("NOCCUPANTI")) Then
    '                oMyArticolo.nComponenti = stringoperation.formatint(dtMyRow("NOCCUPANTI"))
    '            End If
    '            If Not IsDBNull(dtMyRow("MQ")) Then
    '                oMyArticolo.nMQ = stringoperation.formatdouble(dtMyRow("MQ"))
    '            End If
    '            If Not IsDBNull(dtMyRow("TEMPO")) Then
    '                oMyArticolo.nBimestri = stringoperation.formatint(dtMyRow("TEMPO"))
    '            End If
    '            If Not IsDBNull(dtMyRow("TARIFFA")) Then
    '                oMyArticolo.impTariffa = stringoperation.formatdouble(dtMyRow("TARIFFA"))
    '            End If
    '            If Not IsDBNull(dtMyRow("IMPORTO")) Then
    '                oMyArticolo.impRuolo = stringoperation.formatdouble(dtMyRow("IMPORTO"))
    '            End If
    '            If Not IsDBNull(dtMyRow("IMPORTO_RIDUZIONI")) Then
    '                oMyArticolo.impRiduzione = stringoperation.formatdouble(dtMyRow("IMPORTO_RIDUZIONI"))
    '            End If
    '            If Not IsDBNull(dtMyRow("IMPORTO_DETASSAZIONI")) Then
    '                oMyArticolo.impDetassazione = stringoperation.formatdouble(dtMyRow("IMPORTO_DETASSAZIONI"))
    '            End If
    '            If Not IsDBNull(dtMyRow("IMPORTO_NETTO")) Then
    '                oMyArticolo.impNetto = stringoperation.formatdouble(dtMyRow("IMPORTO_NETTO"))
    '            End If
    '            '*** 201809 Bollettazione Vigliano in OPENgov***
    '            If Not IsDBNull(dtMyRow("TIPOUTENZA")) Then
    '                oMyArticolo.sIdTipoUnita = stringoperation.formatstring(dtMyRow("TIPOUTENZA"))
    '            End If
    '            If Not IsDBNull(dtMyRow("RIDUZIONI")) Then
    '                Dim oMyRid As New ObjRidEseApplicati
    '                Dim ListRid As New Generic.List(Of ObjRidEseApplicati)
    '                oMyRid.sDescrizione = stringoperation.formatstring(dtMyRow("RIDUZIONI"))
    '                ListRid.Add(oMyRid)
    '                oMyArticolo.oRiduzioni = ListRid.ToArray
    '            End If
    '            oListArticoli.Add(oMyArticolo)
    '        Next
    '        oMyAvviso.oArticoli = oListArticoli.ToArray
    '        oListAvvisi.Add(oMyAvviso)

    '        Return oListAvvisi.ToArray
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestRuolo.GetRuoloMinuta.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return Nothing
    '    Finally
    '        myAdapter.Dispose()
    '        dtMyDati.Dispose()
    '        cmdMyCommand.Dispose()
    '        cmdMyCommand.Connection.Close()
    '    End Try
    'End Function
    '*** ***
    '*** 20131104 - TARES ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myStringConnection"></param>
    ''' <param name="sIdEnte"></param>
    ''' <param name="sAnno"></param>
    ''' <returns></returns>
    Public Function GetDatiPerRuolo(myStringConnection As String, ByVal sIdEnte As String, ByVal sAnno As String) As ObjRuolo()
        Dim oMyRuolo As New ObjRuolo
        Dim oListRuolo As New ArrayList
        Dim sSQL As String = ""
        Dim myDataView As New DataView
        Dim nMyReturn As Integer = -1

        Try
            Using ctx As New DBModel(ConstSession.DBType, myStringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetRiepilogoDaElaborare", "IdEnte", "Anno")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IdEnte", sIdEnte) _
                            , ctx.GetParam("Anno", sAnno)
                        )
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestRuolo.GetDatiPerRuolo.erroreQuery: ", ex)
                    Return Nothing
                Finally
                    ctx.Dispose()
                End Try
                For Each myRow As DataRowView In myDataView
                    oMyRuolo = New ObjRuolo
                    oMyRuolo.sEnte = sIdEnte
                    oMyRuolo.sAnno = sAnno
                    oMyRuolo.sTipoCalcolo = myRow("tipo_ruolo").ToString
                    oMyRuolo.nContribuenti = StringOperation.FormatInt(myRow("tot_contribuenti"))
                    oMyRuolo.nUtentiDom = StringOperation.FormatInt(myRow("contrib_dom"))
                    oMyRuolo.nUtentiNonDom = StringOperation.FormatInt(myRow("contrib_nondom"))
                    oMyRuolo.nMQ = StringOperation.FormatDouble(myRow("tot_mq"))
                    oMyRuolo.nMQDom = StringOperation.FormatDouble(myRow("mq_dom"))
                    oMyRuolo.nMQNonDom = StringOperation.FormatDouble(myRow("mq_nondom"))
                    oMyRuolo.nTessere = StringOperation.FormatInt(myRow("tot_tessere"))
                    oMyRuolo.nTesNoUI = StringOperation.FormatInt(myRow("tot_tesserenoui"))
                    oMyRuolo.nTesBidone = StringOperation.FormatInt(myRow("tot_tesserebidone"))
                    oMyRuolo.nConferimenti = StringOperation.FormatDouble(myRow("tot_conferimenti"))
                    oListRuolo.Add(oMyRuolo)
                Next
            End Using
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestRuolo.GetDatiPerRuolo.errore: ", Err)
            Return Nothing
        Finally
            myDataView.Dispose()
        End Try
        Return CType(oListRuolo.ToArray(GetType(ObjRuolo)), ObjRuolo())
    End Function
    'Public Function GetDatiPerRuolo(ByVal sIdEnte As String, ByVal nIdRuolo As Integer, ByVal sAnno As String, ByVal sTipoRuolo As String, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As ObjRuolo()
    '    'Dim dtmyrow As SqlClient.SqlDataReader
    '    Dim oMyRuolo As New ObjRuolo
    '    'Dim nList As Integer = -1
    '    Dim oListRuolo As New ArrayList
    '    Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim dtMyDati As New DataTable()
    '    Dim dtMyRow As DataRow

    '    Try
    '        If (Not (cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand = cmdMyCommandOut
    '        Else
    '            cmdMyCommand = New SqlClient.SqlCommand
    '            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure

    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.AddWithValue("@IdEnte", sIdEnte)
    '        cmdMyCommand.Parameters.AddWithValue("@Anno", sAnno)
    '        cmdMyCommand.CommandText = "prc_GetRiepilogoDaElaborare"
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(dtMyDati)
    '        myAdapter.Dispose()
    '        For Each dtMyRow In dtMyDati.Rows
    '            oMyRuolo = New ObjRuolo
    '            oMyRuolo.sEnte = sIdEnte
    '            oMyRuolo.sAnno = sAnno
    '            oMyRuolo.sTipoCalcolo = dtMyRow("tipo_ruolo").ToString
    '            If Not IsDBNull(dtMyRow("tot_contribuenti")) Then
    '                oMyRuolo.nContribuenti = stringoperation.formatint(dtMyRow("tot_contribuenti"))
    '            End If
    '            If Not IsDBNull(dtMyRow("contrib_dom")) Then
    '                oMyRuolo.nUtentiDom = stringoperation.formatint(dtMyRow("contrib_dom"))
    '            End If
    '            If Not IsDBNull(dtMyRow("contrib_nondom")) Then
    '                oMyRuolo.nUtentiNonDom = stringoperation.formatint(dtMyRow("contrib_nondom"))
    '            End If
    '            If Not IsDBNull(dtMyRow("tot_mq")) Then
    '                oMyRuolo.nMQ = stringoperation.formatdouble(dtMyRow("tot_mq"))
    '            End If
    '            If Not IsDBNull(dtMyRow("mq_dom")) Then
    '                oMyRuolo.nMQDom = stringoperation.formatdouble(dtMyRow("mq_dom"))
    '            End If
    '            If Not IsDBNull(dtMyRow("mq_nondom")) Then
    '                oMyRuolo.nMQNonDom = stringoperation.formatdouble(dtMyRow("mq_nondom"))
    '            End If
    '            If Not IsDBNull(dtMyRow("tot_tessere")) Then
    '                oMyRuolo.nTessere = stringoperation.formatint(dtMyRow("tot_tessere"))
    '            End If
    '            If Not IsDBNull(dtMyRow("tot_tesserenoui")) Then
    '                oMyRuolo.nTesNoUI = stringoperation.formatint(dtMyRow("tot_tesserenoui"))
    '            End If
    '            If Not IsDBNull(dtMyRow("tot_tesserebidone")) Then
    '                oMyRuolo.nTesBidone = stringoperation.formatint(dtMyRow("tot_tesserebidone"))
    '            End If
    '            If Not IsDBNull(dtMyRow("tot_conferimenti")) Then
    '                oMyRuolo.nConferimenti = stringoperation.formatdouble(dtMyRow("tot_conferimenti"))
    '            End If
    '            oListRuolo.Add(oMyRuolo)
    '        Next

    '        Return CType(oListRuolo.ToArray(GetType(ObjRuolo)), ObjRuolo())
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestRuolo.GetDatiPerRuolo.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return CType((New ArrayList).ToArray(GetType(ObjRuolo)), ObjRuolo())
    '    Finally
    '        dtMyDati.Dispose()
    '        If (cmdMyCommandOut Is Nothing) Then
    '            cmdMyCommand.Dispose()
    '            cmdMyCommand.Connection.Close()
    '        End If
    '    End Try
    'End Function
    Public Function UpdateDateRuolo(ByVal oMyRuolo() As ObjRuolo, ByVal nTypeData As Integer, ByVal sTypeUpdate As String) As Boolean
        'Dim WFErrore As String = ""
        'Dim WFSessione As OPENUtility.CreateSessione
        Dim FncRuolo As New ClsGestRuolo
        'Dim MyDBEngine As DBEngine = Nothing

        Try
            'WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
            'If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
            '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
            'End If
            If FncRuolo.SetDateRuolo(oMyRuolo(0).IdFlusso, nTypeData, sTypeUpdate) = 0 Then
                Return False
            End If
            If sTypeUpdate = "I" Then
                Select Case nTypeData
                    Case 0 '= imposta la data del calcolo
                        oMyRuolo(0).tDataCreazione = Now
                    Case 1 '= imposta la data della stampa della minuta
                        oMyRuolo(0).tDataStampaMinuta = Now
                    Case 2 '= imposta la data di approvazione della minuta
                        oMyRuolo(0).tDataOKMinuta = Now
                    Case 3 '= imposta la data di cartellazione
                        oMyRuolo(0).tDataCartellazione = Now
                    Case 4 '= imposta la data di elaborazione documenti
                        oMyRuolo(0).tDataElabDoc = Now
                    Case 5 '= imposta la data di approvazione documenti
                        oMyRuolo(0).tDataOKDoc = Now
                End Select
            ElseIf sTypeUpdate = "C" Then
                Select Case nTypeData
                    Case 0 '= imposta la data del calcolo
                        oMyRuolo(0).tDataCreazione = Date.MinValue
                    Case 1 '= imposta la data della stampa della minuta
                        oMyRuolo(0).tDataStampaMinuta = Date.MinValue
                    Case 2 '= imposta la data di approvazione della minuta
                        oMyRuolo(0).tDataOKMinuta = Date.MinValue
                    Case 3 '= imposta la data di cartellazione
                        oMyRuolo(0).tDataCartellazione = Date.MinValue
                    Case 4 '= imposta la data di elaborazione documenti
                        oMyRuolo(0).tDataElabDoc = Date.MinValue
                    Case 5 '= imposta la data di approvazione documenti
                        oMyRuolo(0).tDataElabDoc = Now
                End Select
            End If
            Return True
        Catch Err As Exception
            Log.Debug(oMyRuolo(0).sEnte + " - OPENgovTIA.GestRuolo.UpdateDateRuolo.errore: ", Err)
            Return False
        Finally
            'WFSessione.Kill()
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="IdSetRuolo"></param>
    ''' <param name="nDBOperation">
    '''0 = imposta la data del calcolo
    '''1 = imposta la data della stampa della minuta
    '''2 = imposta la data di approvazione della minuta
    '''3 = imposta la data di cartellazione
    '''4 = imposta la data di elaborazione documenti
    '''5 = imposta la data di approvazione documenti
    '''</param>
    ''' <param name="sTypeDBOperation">
    '''I = inserimento data
    '''C = cancellazione data
    '''</param>
    ''' <returns></returns>
    Public Function SetDateRuolo(ByVal IdSetRuolo As Integer, ByVal nDBOperation As Integer, ByVal sTypeDBOperation As String) As Integer
        Dim sSQL As String = ""
        Dim nMyReturn As Integer

        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_SetDateRuolo", "IDFLUSSO", "DBOPERATION", "TYPEDBOPERATION")
                    ctx.ExecuteNonQuery(sSQL, ctx.GetParam("IDFLUSSO", IdSetRuolo) _
                            , ctx.GetParam("DBOPERATION", nDBOperation) _
                            , ctx.GetParam("TYPEDBOPERATION", sTypeDBOperation)
                        )
                    nMyReturn = IdSetRuolo
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsElabRuolo.CalcoloRate.prc_SetRate.errore: ", ex)
                    nMyReturn = -1
                Finally
                    ctx.Dispose()
                End Try
            End Using
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestRuolo.SetDateRuolo.errore: ", ex)
            IdSetRuolo = 0
        End Try
        Return IdSetRuolo
    End Function
    'Public Function SetDateRuolo(ByVal IdSetRuolo As Integer, ByVal nDBOperation As Integer, ByVal sTypeDBOperation As String, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As Integer
    '    Dim sSQL As String = ""
    '    'Dim MyDBEngine As DBEngine = Nothing
    '    'Dim dtMyDati As New DataTable()
    '    'Dim dtMyRow As DataRow

    '    Try
    '        If (Not (cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand = cmdMyCommandOut
    '        Else
    '            cmdMyCommand = New SqlClient.SqlCommand
    '            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        '***nDBOperation****************************
    '        '0 = imposta la data del calcolo
    '        '1 = imposta la data della stampa della minuta
    '        '2 = imposta la data di approvazione della minuta
    '        '3 = imposta la data di cartellazione
    '        '4 = imposta la data di elaborazione documenti
    '        '5 = imposta la data di approvazione documenti
    '        '******************************************
    '        '***sTypeDBOperation************************
    '        'I = inserimento data
    '        'C = cancellazione data
    '        '******************************************
    '        'costruisco la query
    '        Select Case nDBOperation
    '            Case 0
    '                sSQL = "UPDATE TBLRUOLI_GENERATI SET DATA_CALCOLO="
    '                If sTypeDBOperation = "C" Then
    '                    sSQL += "NULL"
    '                Else
    '                    sSQL += "'" & oReplace.ReplaceDataForDB(Now) & "'"
    '                End If
    '                sSQL += " WHERE (IDFLUSSO=" & IdSetRuolo & ")"
    '            Case 1
    '                sSQL = "UPDATE TBLRUOLI_GENERATI SET DATA_STAMPA_MINUTA="
    '                If sTypeDBOperation = "C" Then
    '                    sSQL += "NULL"
    '                Else
    '                    sSQL += "'" & oReplace.ReplaceDataForDB(Now) & "'"
    '                End If
    '                sSQL += " WHERE (IDFLUSSO=" & IdSetRuolo & ")"
    '            Case 2
    '                If ConstSession.IsFromVariabile = "1" Then
    '                    sSQL = "UPDATE TBLRUOLI_GENERATI SET DATA_OKMINUTA="
    '                Else
    '                    sSQL = "UPDATE TBLRUOLI_GENERATI SET DATA_APPROVAZIONE="
    '                End If
    '                If sTypeDBOperation = "C" Then
    '                    sSQL += "NULL"
    '                Else
    '                    sSQL += "'" & oReplace.ReplaceDataForDB(Now) & "'"
    '                End If
    '                sSQL += " WHERE (IDFLUSSO=" & IdSetRuolo & ")"
    '            Case 3
    '                sSQL = "UPDATE TBLRUOLI_GENERATI SET DATA_CARTELLAZIONE="
    '                If sTypeDBOperation = "C" Then
    '                    sSQL += "NULL"
    '                Else
    '                    sSQL += "'" & oReplace.ReplaceDataForDB(Now) & "'"
    '                End If
    '                sSQL += " WHERE (IDFLUSSO=" & IdSetRuolo & ")"
    '            Case 4
    '                sSQL = "UPDATE TBLRUOLI_GENERATI SET DATA_ELABDOC="
    '                If sTypeDBOperation = "C" Then
    '                    sSQL += "NULL"
    '                Else
    '                    sSQL += "'" & oReplace.ReplaceDataForDB(Now) & "'"
    '                End If
    '                sSQL += " WHERE (IDFLUSSO=" & IdSetRuolo & ")"
    '            Case 5
    '                If ConstSession.IsFromVariabile = "1" Then
    '                    sSQL = "UPDATE TBLRUOLI_GENERATI SET DATA_OKDOCUMENTI="
    '                Else
    '                    sSQL = "UPDATE TBLRUOLI_GENERATI SET DATA_APPROVAZIONE_DOCUMENTI="
    '                End If
    '                If sTypeDBOperation = "C" Then
    '                    sSQL += "NULL"
    '                Else
    '                    sSQL += "'" & oReplace.ReplaceDataForDB(Now) & "'"
    '                End If
    '                sSQL += " WHERE (IDFLUSSO=" & IdSetRuolo & ")"
    '        End Select
    '        cmdMyCommand.CommandText = sSQL
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        cmdMyCommand.ExecuteNonQuery()

    '        Return IdSetRuolo
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestRuolo.SetDateRuolo.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return 0
    '    Finally
    '        If (cmdMyCommandOut Is Nothing) Then
    '            cmdMyCommand.Dispose()
    '            cmdMyCommand.Connection.Close()
    '        End If
    '    End Try
    'End Function
    '*** ***
    Public Function SetDocumentiElaborati(ByVal oMyRuolo As ObjRuolo) As Boolean
        Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dtMyDati As New DataTable()
        Dim myRet As Boolean = False

        Try
            cmdMyCommand = New SqlClient.SqlCommand
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure

            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.AddWithValue("@IDFLUSSO", oMyRuolo.IdFlusso)
            cmdMyCommand.CommandText = "prc_SetDocumentiElaborati"
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(dtMyDati)
            myAdapter.Dispose()
            For Each dtMyRow As DataRow In dtMyDati.Rows
                If Not IsDBNull(dtMyRow("id")) Then
                    If StringOperation.FormatInt(dtMyRow("id")) > 0 Then
                        myRet = True
                    Else
                        myRet = False
                    End If
                Else
                    myRet = False
                End If
            Next
        Catch Err As Exception
            Log.Debug(oMyRuolo.sEnte + " - OPENgovTIA.GestRuolo.SetDocumentiElaborati.errore: ", Err)
            myRet = False
        End Try
        Return myRet
    End Function
End Class
''' <summary>
''' Classe per l'elaborazione dei documenti
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class ClsGestDocumenti
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ClsGestDocumenti))
    Private cmdMyCommand As New SqlClient.SqlCommand
    Private oReplace As New generalClass.generalFunction
    Private Delegate Sub StampaMassivaAsyncTARSU(ByVal ConnessioneTARSU As String, ByVal ConnessioneAnagrafica As String, ByVal ConnessioneRepository As String, ByVal CodEnte As String, ByVal TipologiaElaborazione As String, ByVal DocumentiPerGruppo As Integer, ByVal TipoOrdinamento As String, ByVal IdFlussoRuolo As Integer, ByVal TipoFlussoRuolo As String, ByVal ArrayCodiciCartella As String(), ByVal ElaboraBollettini As Boolean, ByVal TipoBollettino As String, ByVal bCreaPDF As Boolean)
    '*** 201511 - template documenti per ruolo ***'*** 20140509 - TASI ***
    'Private Delegate Sub StampaMassivaAsync(ByVal Tributo As String, ByVal IdDocToElab(,) As Integer, ByVal sAnno As Integer, ByVal sIDEnte As String, ByVal IdFlussoRuolo As Integer, ByVal Esclusione() As String, ByVal ConnessioneICI As String, ByVal ConnessioneRepository As String, ByVal ConnessioneAnagrafica As String, ByVal ContribuentiPerGruppo As Integer, ByVal TipoElaborazione As String, ByVal ImpostazioniBollettini As String, ByVal TipoCalcolo As String, ByVal TipoBollettino As String, ByVal bStampaBollettino As Boolean, ByVal bCreaPDF As Boolean, ByVal nettoVersato As Boolean, ByVal nDecimal As Integer)
    'Private Delegate Sub StampaMassivaAsync(ByVal Tributo As String, ByVal IdDocToElab(,) As Integer, ByVal sAnno As Integer, ByVal sIDEnte As String, ByVal IdFlussoRuolo As Integer, ByVal Esclusione() As String, ByVal ConnessioneICI As String, ByVal ConnessioneRepository As String, ByVal ConnessioneAnagrafica As String, ByVal ContribuentiPerGruppo As Integer, ByVal TipoElaborazione As String, ByVal ImpostazioniBollettini As String, ByVal TipoCalcolo As String, ByVal TipoBollettino As String, ByVal bStampaBollettino As Boolean, ByVal bCreaPDF As Boolean, ByVal nettoVersato As Boolean, ByVal nDecimal As Integer, ByVal bSendByMail As Boolean)
    Private Delegate Sub StampaMassivaAsync(DBType As String, ByVal Tributo As String, ByVal IdDocToElab(,) As Integer, ByVal sAnno As Integer, ByVal sIDEnte As String, ByVal IdFlussoRuolo As Integer, ByVal Esclusione() As String, ByVal ConnessioneICI As String, ByVal ConnessioneRepository As String, ByVal ConnessioneAnagrafica As String, PathTemplate As String, PathTemplateVirtual As String, ByVal ContribuentiPerGruppo As Integer, ByVal TipoElaborazione As String, ByVal ImpostazioniBollettini As String, ByVal TipoCalcolo As String, ByVal TipoBollettino As String, ByVal bStampaBollettino As Boolean, ByVal bCreaPDF As Boolean, ByVal nettoVersato As Boolean, ByVal nDecimal As Integer, ByVal bSendByMail As Boolean, IsSoloBollettino As Boolean)
    Public Function InElaborazione(DBType As String, ByVal ConnessioneRepository As String, ByVal ANNO As String, ByVal COD_ENTE As String, ByVal COD_TRIBUTO As String, ByRef sNOTE As String, ByVal IDFLUSSORUOLO As Integer) As Integer
        Dim objDR As SqlClient.SqlDataReader
        Dim bELABORAZIONE As Boolean = False, bERRORI As Boolean = False

        Try
            Dim _oDbManagerRepository As New DBModel(DBType, ConnessioneRepository)
            Dim sSQL As String = ""
            Dim dvMyDati As New DataSet()
            Using ctx As DBModel = _oDbManagerRepository
                sSQL = "SELECT *" _
                    + " FROM TP_TASK_REPOSITORY" _
                    + " WHERE 1=1" _
                    + " AND COD_ENTE=@ENTE" _
                    + " AND COD_TRIBUTO=@COD_TRIBUTO" _
                    + " AND ANNO=@ANNO" _
                    + " AND IDFLUSSORUOLOTARSU=@IDFLUSSORUOLOTARSU" _
                    + " ORDER BY ID_TASK_REPOSITORY DESC,DATA_ELABORAZIONE DESC"
                objDR = ctx.GetDataReader(sSQL, ctx.GetParam("ENTE", COD_ENTE) _
                            , ctx.GetParam("COD_TRIBUTO", COD_TRIBUTO) _
                            , ctx.GetParam("ANNO", ANNO) _
                            , ctx.GetParam("IDFLUSSORUOLOTARSU", IDFLUSSORUOLO)
                        )
                If (objDR.HasRows) Then
                    objDR.Read()
                    bELABORAZIONE = Boolean.Parse(objDR("ELABORAZIONE").ToString())
                    bERRORI = Boolean.Parse(objDR("ERRORI").ToString())
                    If objDR("NOTE") Is DBNull.Value Then
                        sNOTE = ""
                    Else
                        sNOTE = objDR("NOTE").ToString()
                    End If
                End If
                objDR.Close()
                ctx.Dispose()
            End Using

            If Not bELABORAZIONE Then
                If (bERRORI) Then
                    Return 2
                Else
                    Return 0
                End If
            Else
                Return 1
            End If
        Catch Ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDocumenti.InElaborazione.errore: ", Ex)
            sNOTE = Ex.Message
            Return -1
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="DBType"></param>
    ''' <param name="Tributo"></param>
    ''' <param name="IdDocToElab"></param>
    ''' <param name="sAnno"></param>
    ''' <param name="sIDEnte"></param>
    ''' <param name="IdFlussoRuolo"></param>
    ''' <param name="Esclusione"></param>
    ''' <param name="ConnessioneICI"></param>
    ''' <param name="ConnessioneRepository"></param>
    ''' <param name="ConnessioneAnagrafica"></param>
    ''' <param name="PathTemplate"></param>
    ''' <param name="PathTemplateVirtual"></param>
    ''' <param name="ContribuentiPerGruppo"></param>
    ''' <param name="TipoElaborazione"></param>
    ''' <param name="ImpostazioniBollettini"></param>
    ''' <param name="TipoCalcolo"></param>
    ''' <param name="TipoBollettino"></param>
    ''' <param name="bStampaBollettino"></param>
    ''' <param name="bCreaPDF"></param>
    ''' <param name="nettoVersato"></param>
    ''' <param name="nDecimal"></param>
    ''' <param name="bSendByMail"></param>
    ''' <param name="IsSoloBollettino"></param>
    ''' <revisionHistory>
    ''' <revision date="05/11/2020">
    ''' devo aggiungere tributo F24 per poter gestire correttamente la stampa in caso di Ravvedimento IMU/TASI
    ''' </revision>
    ''' </revisionHistory>
    Private Sub ChiamaElaborazioneAsincrona(DBType As String, ByVal Tributo As String, ByVal IdDocToElab(,) As Integer, ByVal sAnno As Integer, ByVal sIDEnte As String, ByVal IdFlussoRuolo As Integer, ByVal Esclusione() As String, ByVal ConnessioneICI As String, ByVal ConnessioneRepository As String, ByVal ConnessioneAnagrafica As String, PathTemplate As String, PathTemplateVirtual As String, ByVal ContribuentiPerGruppo As Integer, ByVal TipoElaborazione As String, ByVal ImpostazioniBollettini As String, ByVal TipoCalcolo As String, ByVal TipoBollettino As String, ByVal bStampaBollettino As Boolean, ByVal bCreaPDF As Boolean, ByVal nettoVersato As Boolean, ByVal nDecimal As Integer, ByVal bSendByMail As Boolean, IsSoloBollettino As Boolean)
        Try
            '// faccio partire l'elaborazione
            '// chiamo il servizio di elaborazione delle stampe massive.
            Dim oElaborazioneDati As ElaborazioneDatiStampeInterface.IElaborazioneStampeICI
            oElaborazioneDati = Activator.GetObject(GetType(ElaborazioneDatiStampeInterface.IElaborazioneStampeICI), ConstSession.UrlServizioStampeICI)
            Log.Debug("ChiamaElaborazioneAsincronaChiamaElaborazioneAsincrona=" & ConstSession.UrlServizioStampeICI)
            '**** 201810 - Calcolo puntuale ****
            oElaborazioneDati.ElaborazioneMassivaDocumenti(ConstSession.DBType, Tributo, IdDocToElab, sAnno, sIDEnte, IdFlussoRuolo, Esclusione, ConnessioneICI, ConnessioneRepository, ConnessioneAnagrafica, PathTemplate, PathTemplateVirtual, ContribuentiPerGruppo, TipoElaborazione, ImpostazioniBollettini, TipoCalcolo, TipoBollettino, bStampaBollettino, bCreaPDF, nettoVersato, nDecimal, bSendByMail, IsSoloBollettino, "", Tributo)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDocumenti.ChaiamElaborazioneAsincrona.errore: ", Err)
            Throw Err
        End Try
    End Sub
    '*** ***

    '*** 20120917 - sgravi ***
    Public Function ConvAvvisi(ByVal oListAvvisi() As ObjAvviso) As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoCartella()
        Dim oConv() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoCartella = Nothing
        Dim oMyConv As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoCartella
        Dim x, nList As Integer

        Try
            nList = -1
            For x = 0 To oListAvvisi.GetUpperBound(0)
                oMyConv = New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoCartella
                oMyConv.Cognome = oListAvvisi(x).sCognome
                oMyConv.Nome = oListAvvisi(x).sNome
                oMyConv.idCartella = oListAvvisi(x).ID
                oMyConv.IdFlussoRuolo = oListAvvisi(x).IdFlussoRuolo
                oMyConv.AnnoRiferimento = oListAvvisi(x).sAnnoRiferimento
                oMyConv.CodiceCartella = oListAvvisi(x).sCodiceCartella
                oMyConv.ImportoTotale = oListAvvisi(x).impTotale
                oMyConv.ImportoArrotondamento = oListAvvisi(x).impArrotondamento
                oMyConv.ImportoCarico = oListAvvisi(x).impCarico
                oMyConv.IdContribuente = oListAvvisi(x).IdContribuente
                nList += 1
                ReDim Preserve oConv(nList)
                oConv(nList) = oMyConv
            Next
            Return oConv
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDocumenti.ConvAvvisi.errore: ", Err)
            Return Nothing
        End Try
    End Function
    '*** ***
    '*** 20131104 - TARES ***
    Public Sub GetNDoc(ByVal sIdEnte As String, ByVal nIdRuolo As Integer, ByRef nElab As Integer, ByRef nDaElab As Integer, ByRef cmdMyCommandOut As SqlClient.SqlCommand)
        Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dtMyDati As New DataTable()
        Dim dtMyRow As DataRow

        Try
            If (Not (cmdMyCommandOut) Is Nothing) Then
                cmdMyCommand = cmdMyCommandOut
            Else
                cmdMyCommand = New SqlClient.SqlCommand
                cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
                cmdMyCommand.Connection.Open()
            End If
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.AddWithValue("@IdEnte", sIdEnte)
            cmdMyCommand.Parameters.AddWithValue("@IDFLUSSO_RUOLO", nIdRuolo)
            cmdMyCommand.CommandText = "prc_GetNDoc"
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(dtMyDati)
            myAdapter.Dispose()
            For Each dtMyRow In dtMyDati.Rows
                nElab = StringOperation.FormatInt(dtMyRow("docelaborati"))
                nDaElab = StringOperation.FormatInt(dtMyRow("docdaelaborare"))
            Next
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDocumenti.GetNDoc.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
        Finally
            dtMyDati.Dispose()
            If (cmdMyCommandOut Is Nothing) Then
                cmdMyCommand.Dispose()
                cmdMyCommand.Connection.Close()
            End If
        End Try
    End Sub

    Public Function SetTabGuidaComunicoStorico(ByVal sIdEnte As String, ByVal nIdRuolo As Integer, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As Integer
        Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
        Dim MyRet As Integer = 0

        Try
            If (Not (cmdMyCommandOut) Is Nothing) Then
                cmdMyCommand = cmdMyCommandOut
            Else
                cmdMyCommand = New SqlClient.SqlCommand
                cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
                cmdMyCommand.Connection.Open()
            End If
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.AddWithValue("@ID", nIdRuolo)
            cmdMyCommand.Parameters.AddWithValue("@IdEnte", sIdEnte)
            cmdMyCommand.Parameters.AddWithValue("@IDFLUSSO_RUOLO", nIdRuolo)
            cmdMyCommand.Parameters("@ID").Direction = ParameterDirection.InputOutput
            cmdMyCommand.CommandText = "prc_SetTabGuidaComunicoStorico"
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            cmdMyCommand.ExecuteNonQuery()
            MyRet = cmdMyCommand.Parameters("@ID").Value
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDocumenti.SetTabGuidaComunicoStorico.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
        Finally
            If (cmdMyCommandOut Is Nothing) Then
                cmdMyCommand.Dispose()
                cmdMyCommand.Connection.Close()
            End If
        End Try
        Return MyRet
    End Function
    Public Function SetTabFilesComunicoElabStorico(ByVal sIdEnte As String, ByVal nIdRuolo As Integer, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As Integer
        Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
        Dim MyRet As Integer

        Try
            If (Not (cmdMyCommandOut) Is Nothing) Then
                cmdMyCommand = cmdMyCommandOut
            Else
                cmdMyCommand = New SqlClient.SqlCommand
                cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
                cmdMyCommand.Connection.Open()
            End If
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.AddWithValue("@ID", nIdRuolo)
            cmdMyCommand.Parameters.AddWithValue("@IdEnte", sIdEnte)
            cmdMyCommand.Parameters.AddWithValue("@IDFLUSSO_RUOLO", nIdRuolo)
            cmdMyCommand.Parameters("@ID").Direction = ParameterDirection.InputOutput
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_SetTabFilesComunicoElabStorico"
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            cmdMyCommand.ExecuteNonQuery()
            MyRet = cmdMyCommand.Parameters("@ID").Value
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDocumenti.SetTabFilesComunicoElabStorico.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
        Finally
            If (cmdMyCommandOut Is Nothing) Then
                cmdMyCommand.Dispose()
                cmdMyCommand.Connection.Close()
            End If
        End Try
        Return MyRet
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sIdEnte"></param>
    ''' <param name="sNomeTabella"></param>
    ''' <param name="nIdRuolo"></param>
    ''' <param name="Tributo"></param>
    ''' <returns></returns>
    Public Function DeleteTabGuidaComunico(ByVal sIdEnte As String, ByVal sNomeTabella As String, ByVal nIdRuolo As Integer, Tributo As String) As Integer
        Dim dvMyDati As New DataView
        Dim sSQL As String = ""
        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_" + sNomeTabella + "_D", "IdEnte", "TRIBUTO", "IDFLUSSO_RUOLO")
                ctx.ExecuteNonQuery(sSQL, ctx.GetParam("IdEnte", sIdEnte) _
                                , ctx.GetParam("TRIBUTO", Tributo) _
                                , ctx.GetParam("IDFLUSSO_RUOLO", nIdRuolo)
                            )
                ctx.Dispose()
            End Using
            Return 1
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDocumenti.DeleteTabGuidaComunico.errore: ", ex)
            Return 0
        End Try
    End Function
    'Public Function DeleteTabGuidaComunico(ByVal sIdEnte As String, ByVal sNomeTabella As String, ByVal nIdRuolo As Integer, Tributo As String, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As Integer
    '    Dim cmdMyCommand As SqlClient.SqlCommand = Nothing

    '    Try
    '        If (Not (cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand = cmdMyCommandOut
    '        Else
    '            cmdMyCommand = New SqlClient.SqlCommand
    '            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.AddWithValue("@IdEnte", sIdEnte)
    '        cmdMyCommand.Parameters.AddWithValue("@IDFLUSSO_RUOLO", nIdRuolo)
    '        cmdMyCommand.Parameters.AddWithValue("@TRIBUTO", Tributo)
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = "prc_" + sNomeTabella + "_D"
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        cmdMyCommand.ExecuteNonQuery()
    '        Return 1
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestDocumenti.DeleteTabGuidaComunico.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return 0
    '    Finally
    '        If (cmdMyCommandOut Is Nothing) Then
    '            cmdMyCommand.Dispose()
    '            cmdMyCommand.Connection.Close()
    '        End If
    '    End Try
    'End Function

    '*** 20140509 - TASI ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sIdEnte"></param>
    ''' <param name="nIdRuolo"></param>
    ''' <param name="sTipoRuolo"></param>
    ''' <param name="sAnno"></param>
    ''' <param name="nDocDaElaborare"></param>
    ''' <param name="nDocElaborati"></param>
    ''' <param name="nTipoElab"></param>
    ''' <param name="sTypeOrd"></param>
    ''' <param name="sNameModello"></param>
    ''' <param name="nMaxDocPerFile"></param>
    ''' <param name="bElabBollettini"></param>
    ''' <param name="oListAvvisi"></param>
    ''' <param name="oListDocStampati"></param>
    ''' <param name="bCreaPDF"></param>
    ''' <param name="nDecimal"></param>
    ''' <param name="TipoCalcolo"></param>
    ''' <param name="TipoStampaBollettini"></param>
    ''' <param name="TipoBollettino"></param>
    ''' <param name="bSendByMail"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="05/11/2020">
    ''' devo aggiungere tributo F24 per poter gestire correttamente la stampa in caso di Ravvedimento IMU/TASI
    ''' </revision>
    ''' </revisionHistory>
    Public Function ElaboraDocumenti(ByVal sIdEnte As String, ByVal nIdRuolo As Integer, ByVal sTipoRuolo As String, ByVal sAnno As String, ByVal nDocDaElaborare As Integer, ByVal nDocElaborati As Integer, ByVal nTipoElab As Integer, ByVal sTypeOrd As String, ByVal sNameModello As String, ByVal nMaxDocPerFile As Integer, ByVal bElabBollettini As Boolean, ByVal oListAvvisi() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoCartella, ByRef oListDocStampati() As RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL, ByVal bCreaPDF As Boolean, ByVal nDecimal As Integer, ByVal TipoCalcolo As String, ByVal TipoStampaBollettini As String, ByVal TipoBollettino As String, ByVal bSendByMail As Boolean) As Integer
        Dim nFileDaElab, x, nList As Integer
        Dim sListCodCartella() As String = Nothing
        Dim IdDocToElab(,) As Integer = Nothing
        Dim oListDocDaElab() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDocumentiElaborati
        Dim Esclusione() As String = Nothing

        Try
            Log.Debug("ElaboraDocumenti.inizio")
            '**************************************************************
            'devo risalire all'ultimo file usato per l'elaborazione effettiva in corso
            '**************************************************************
            nFileDaElab = GetNumFileDocDaElaborare(ConstSession.StringConnection, sIdEnte, nIdRuolo)
            Log.Debug("prelevato GetNumFileDocDaElaborare")
            If nFileDaElab <> -1 Then
                nFileDaElab += 1
            End If

            nList = -1
            If Not IsNothing(oListAvvisi) Then
                For x = 0 To oListAvvisi.GetUpperBound(0)
                    Log.Debug("carico documento n." & x)
                    If oListAvvisi(x).Selezionato = True Then
                        nList += 1
                        ReDim Preserve sListCodCartella(nList)
                        sListCodCartella(nList) = oListAvvisi(x).CodiceCartella
                        ReDim Preserve IdDocToElab(1, nList)
                        IdDocToElab(0, nList) = oListAvvisi(x).idCartella
                        IdDocToElab(1, nList) = oListAvvisi(x).IdContribuente
                    End If
                Next
            Else
                'se è già stata fatta un'elaborazione parziale devo solo elaborare le cartelle rimanenti
                If nDocDaElaborare > 0 And nDocElaborati > 0 Then
                    oListDocDaElab = GetDocDaElaborare(ConstSession.StringConnection, sIdEnte, nIdRuolo)
                    If Not IsNothing(oListDocDaElab) Then
                        For x = 0 To oListDocDaElab.GetUpperBound(0)
                            nList += 1
                            ReDim Preserve sListCodCartella(nList)
                            sListCodCartella(nList) = oListDocDaElab(x).CodiceCartella
                            ReDim Preserve IdDocToElab(nList, 1)
                            IdDocToElab(nList, 0) = oListDocDaElab(x).IdAvviso
                            IdDocToElab(nList, 1) = oListDocDaElab(x).IdContribuente
                        Next
                    End If
                End If
            End If

            ' recupero i dati per la chiamata al servizio di elaborazione delle stampe
            Dim strConnessioneTIA As String = ConstSession.StringConnection
            Dim strConnessioneRepository As String = ConstSession.StringConnectionOPENgov
            Dim strConnessioneAnagrafica As String = ConstSession.StringConnectionAnagrafica

            Log.Debug("GestDocumenti::ElaboraDocumenti::connessione" & strConnessioneTIA)
            If nTipoElab = 1 Then
                Log.Debug("elab prova")
                'elaborazione di prova
                Dim oElaborazioneDati As ElaborazioneDatiStampeInterface.IElaborazioneStampeICI
                oElaborazioneDati = Activator.GetObject(GetType(ElaborazioneDatiStampeInterface.IElaborazioneStampeICI), ConstSession.UrlServizioStampeICI)
                Log.Debug("UrlServizioStampeICI=" & ConstSession.UrlServizioStampeICI)
                '**** 201810 - Calcolo puntuale ****'*** 201511 - template documenti per ruolo ***'*** 20140509 - TASI ***
                oListDocStampati = oElaborazioneDati.ElaborazioneMassivaDocumenti(ConstSession.DBType, ConstSession.CodTributo, IdDocToElab, sAnno, sIdEnte, nIdRuolo, Esclusione, ConstSession.StringConnection, ConstSession.StringConnectionOPENgov, ConstSession.StringConnectionAnagrafica, ConstSession.PathStampe, ConstSession.PathVirtualStampe, nMaxDocPerFile, "PROVA", TipoStampaBollettini, TipoCalcolo, TipoBollettino, bElabBollettini, bCreaPDF, False, nDecimal, bSendByMail, False, "", ConstSession.CodTributo)
                '*** ***
            Else
                Log.Debug("elab effettiva")
                'elaborazione effettiva
                Dim del As New StampaMassivaAsync(AddressOf ChiamaElaborazioneAsincrona)
                '*** 201511 - template documenti per ruolo ***'*** 20140509 - TASI ***
                del.BeginInvoke(ConstSession.DBType, ConstSession.CodTributo, IdDocToElab, sAnno, sIdEnte, nIdRuolo, Esclusione, ConstSession.StringConnection, ConstSession.StringConnectionOPENgov, ConstSession.StringConnectionAnagrafica, ConstSession.PathStampe, ConstSession.PathVirtualStampe, nMaxDocPerFile, "EFFETTIVO", TipoStampaBollettini, TipoCalcolo, TipoBollettino, bElabBollettini, bCreaPDF, False, nDecimal, bSendByMail, False, Nothing, Nothing)
                '*** ***
                Return 2
            End If
            Return 1
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDocumenti.ElaboraDocumenti.errore: ", Err)
            Return 0
        End Try
    End Function
    'Public Function ElaboraDocumenti(ByVal sIdEnte As String, ByVal nIdRuolo As Integer, ByVal sTipoRuolo As String, ByVal sAnno As String, ByVal nDocDaElaborare As Integer, ByVal nDocElaborati As Integer, ByVal nTipoElab As Integer, ByVal sTypeOrd As String, ByVal sNameModello As String, ByVal nMaxDocPerFile As Integer, ByVal bElabBollettini As Boolean, ByVal oListAvvisi() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoCartella, ByRef oListDocStampati() As RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL, ByVal bCreaPDF As Boolean, ByVal nDecimal As Integer, ByVal TipoCalcolo As String, ByVal TipoStampaBollettini As String, ByVal TipoBollettino As String, ByVal bSendByMail As Boolean, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As Integer
    '    Dim nFileDaElab, x, nList As Integer
    '    Dim sListCodCartella() As String = Nothing
    '    Dim IdDocToElab(,) As Integer = Nothing
    '    Dim oListDocDaElab() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDocumentiElaborati
    '    Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
    '    Dim Esclusione() As String = Nothing

    '    Try
    '        Log.Debug("ElaboraDocumenti.inizio")
    '        If (Not (cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand = cmdMyCommandOut
    '        Else
    '            cmdMyCommand = New SqlClient.SqlCommand
    '            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        '**************************************************************
    '        'devo risalire all'ultimo file usato per l'elaborazione effettiva in corso
    '        '**************************************************************
    '        nFileDaElab = GetNumFileDocDaElaborare(sIdEnte, nIdRuolo, cmdMyCommand)
    '        Log.Debug("prelevato GetNumFileDocDaElaborare")
    '        If nFileDaElab <> -1 Then
    '            nFileDaElab += 1
    '        End If

    '        nList = -1
    '        If Not IsNothing(oListAvvisi) Then
    '            For x = 0 To oListAvvisi.GetUpperBound(0)
    '                Log.Debug("carico documento n." & x)
    '                If oListAvvisi(x).Selezionato = True Then
    '                    nList += 1
    '                    ReDim Preserve sListCodCartella(nList)
    '                    sListCodCartella(nList) = oListAvvisi(x).CodiceCartella
    '                    ReDim Preserve IdDocToElab(1, nList)
    '                    IdDocToElab(0, nList) = oListAvvisi(x).idCartella
    '                    IdDocToElab(1, nList) = oListAvvisi(x).IdContribuente
    '                End If
    '            Next
    '        Else
    '            'se è già stata fatta un'elaborazione parziale devo solo elaborare le cartelle rimanenti
    '            If nDocDaElaborare > 0 And nDocElaborati > 0 Then
    '                oListDocDaElab = GetDocDaElaborare(sIdEnte, nIdRuolo, cmdMyCommand)
    '                If Not IsNothing(oListDocDaElab) Then
    '                    For x = 0 To oListDocDaElab.GetUpperBound(0)
    '                        nList += 1
    '                        ReDim Preserve sListCodCartella(nList)
    '                        sListCodCartella(nList) = oListDocDaElab(x).CodiceCartella
    '                        ReDim Preserve IdDocToElab(nList, 1)
    '                        IdDocToElab(nList, 0) = oListDocDaElab(x).IdAvviso
    '                        IdDocToElab(nList, 1) = oListDocDaElab(x).IdContribuente
    '                    Next
    '                End If
    '            End If
    '        End If

    '        ' recupero i dati per la chiamata al servizio di elaborazione delle stampe
    '        Dim strConnessioneTIA As String = ConstSession.StringConnection
    '        Dim strConnessioneRepository As String = ConstSession.StringConnectionOPENgov
    '        Dim strConnessioneAnagrafica As String = ConstSession.StringConnectionAnagrafica

    '        Log.Debug("GestDocumenti::ElaboraDocumenti::connessione" & strConnessioneTIA)
    '        If nTipoElab = 1 Then
    '            Log.Debug("elab prova")
    '            'elaborazione di prova
    '            'Dim oElaborazioneDati As ElaborazioneDatiStampeInterface.IElaborazioneStampeTARSU
    '            'oElaborazioneDati = Activator.GetObject(GetType(ElaborazioneDatiStampeInterface.IElaborazioneStampeTARSU), ConfigurationManager.AppSettings("URLElaborazioneDatiStampeTARSU").ToString())
    '            'oListDocStampati = oElaborazioneDati.ElaborazioneMassivaDocumentiTARSU(strConnessioneTIA, strConnessioneAnagrafica, strConnessioneRepository, sIdEnte, nTipoElab, nMaxDocPerFile, sTypeOrd, nIdRuolo, sTipoRuolo, sListCodCartella, bElabBollettini, bCreaPDF)
    '            Dim oElaborazioneDati As ElaborazioneDatiStampeInterface.IElaborazioneStampeICI
    '            oElaborazioneDati = Activator.GetObject(GetType(ElaborazioneDatiStampeInterface.IElaborazioneStampeICI), ConstSession.UrlServizioStampeICI)
    '            Log.Debug("UrlServizioStampeICI=" & ConstSession.UrlServizioStampeICI)
    '            '**** 201810 - Calcolo puntuale ****'*** 201511 - template documenti per ruolo ***'*** 20140509 - TASI ***
    '            'oListDocStampati = oElaborazioneDati.ElaborazioneMassivaDocumenti(ConstSession.CodTributo, IdDocToElab, sAnno, sIdEnte, nIdRuolo, Esclusione, ConstSession.StringConnection, ConstSession.StringConnectionOPENgov, ConstSession.StringConnectionAnagrafica, nMaxDocPerFile, "PROVA", TipoStampaBollettini, TipoCalcolo, TipoBollettino, bElabBollettini, bCreaPDF, False, nDecimal)
    '            'oListDocStampati = oElaborazioneDati.ElaborazioneMassivaDocumenti(ConstSession.CodTributo, IdDocToElab, sAnno, sIdEnte, nIdRuolo, Esclusione, ConstSession.StringConnection, ConstSession.StringConnectionOPENgov, ConstSession.StringConnectionAnagrafica, nMaxDocPerFile, "PROVA", TipoStampaBollettini, TipoCalcolo, TipoBollettino, bElabBollettini, bCreaPDF, False, nDecimal, bSendByMail)
    '            oListDocStampati = oElaborazioneDati.ElaborazioneMassivaDocumenti(ConstSession.DBType, ConstSession.CodTributo, IdDocToElab, sAnno, sIdEnte, nIdRuolo, Esclusione, ConstSession.StringConnection, ConstSession.StringConnectionOPENgov, ConstSession.StringConnectionAnagrafica, ConstSession.PathStampe, ConstSession.PathVirtualStampe, nMaxDocPerFile, "PROVA", TipoStampaBollettini, TipoCalcolo, TipoBollettino, bElabBollettini, bCreaPDF, False, nDecimal, bSendByMail, False, "")
    '            '*** ***
    '        Else
    '            Log.Debug("elab effettiva")
    '            'elaborazione effettiva
    '            Dim del As New StampaMassivaAsync(AddressOf ChiamaElaborazioneAsincrona)
    '            '*** 201511 - template documenti per ruolo ***'*** 20140509 - TASI ***
    '            'del.BeginInvoke(ConstSession.CodTributo, IdDocToElab, sAnno, sIdEnte, nIdRuolo, Esclusione, ConstSession.StringConnection, ConstSession.StringConnectionOPENgov, ConstSession.StringConnectionAnagrafica, nMaxDocPerFile, "EFFETTIVO", TipoStampaBollettini, TipoCalcolo, TipoBollettino, bElabBollettini, bCreaPDF, False, nDecimal, Nothing, Nothing)
    '            'del.BeginInvoke(ConstSession.CodTributo, IdDocToElab, sAnno, sIdEnte, nIdRuolo, Esclusione, ConstSession.StringConnection, ConstSession.StringConnectionOPENgov, ConstSession.StringConnectionAnagrafica, nMaxDocPerFile, "EFFETTIVO", TipoStampaBollettini, TipoCalcolo, TipoBollettino, bElabBollettini, bCreaPDF, False, nDecimal, bSendByMail, Nothing, Nothing)
    '            del.BeginInvoke(ConstSession.DBType, ConstSession.CodTributo, IdDocToElab, sAnno, sIdEnte, nIdRuolo, Esclusione, ConstSession.StringConnection, ConstSession.StringConnectionOPENgov, ConstSession.StringConnectionAnagrafica, ConstSession.PathStampe, ConstSession.PathVirtualStampe, nMaxDocPerFile, "EFFETTIVO", TipoStampaBollettini, TipoCalcolo, TipoBollettino, bElabBollettini, bCreaPDF, False, nDecimal, bSendByMail, False, Nothing, Nothing)
    '            '*** ***
    '            Return 2
    '        End If
    '        Return 1
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDocumenti.ElaboraDocumenti.errore: ", Err)
    '        Return 0
    '    Finally
    '        If (cmdMyCommandOut Is Nothing) Then
    '            cmdMyCommand.Dispose()
    '            cmdMyCommand.Connection.Close()
    '        End If
    '    End Try
    'End Function

    Public Function GetNumFileDocDaElaborare(myStringConnection As String, ByVal sIdEnte As String, ByVal nIdRuolo As Integer) As Integer
        Dim sSQL As String = ""
        Dim myDataView As New DataView
        Dim nMyReturn As Integer = -1

        Try
            Using ctx As New DBModel(ConstSession.DBType, myStringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GeNumDocDaElaborare", "IDENTE", "IDFLUSSO_RUOLO")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", sIdEnte) _
                            , ctx.GetParam("IDFLUSSO_RUOLO", nIdRuolo)
                        )
                Catch ex As Exception
                    Log.Debug(sIdEnte + " - OPENgovTIA.GestAvviso.GetAvviso.erroreQuery: ", ex)
                    Return Nothing
                Finally
                    ctx.Dispose()
                End Try
                For Each myRow As DataRowView In myDataView
                    nMyReturn = StringOperation.FormatInt(myRow("nret"))
                Next
            End Using
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDocumenti.GetNumFileDocDaElaborare.errore: ", Err)
            nMyReturn = -1
        Finally
            myDataView.Dispose()
        End Try
        Return nMyReturn
    End Function
    'Public Function GetNumFileDocDaElaborare(ByVal sIdEnte As String, ByVal nIdRuolo As Integer, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As Integer
    '    Dim nRet As Integer = 0
    '    Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim dtMyDati As New DataTable()
    '    Dim dtMyRow As DataRow

    '    Try
    '        If (Not (cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand = cmdMyCommandOut
    '        Else
    '            cmdMyCommand = New SqlClient.SqlCommand
    '            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure

    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.AddWithValue("@IdEnte", sIdEnte)
    '        cmdMyCommand.Parameters.AddWithValue("@IDFLUSSO_RUOLO", nIdRuolo)
    '        cmdMyCommand.CommandText = "prc_GeNumDocDaElaborare"
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(dtMyDati)
    '        myAdapter.Dispose()
    '        For Each dtMyRow In dtMyDati.Rows
    '            nRet = StringOperation.FormatInt(dtMyRow("nret"))
    '        Next
    '        Return nRet
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDocumenti.GetNumFileDocDaElaborare.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return -1
    '    Finally
    '        dtMyDati.Dispose()
    '        If (cmdMyCommandOut Is Nothing) Then
    '            cmdMyCommand.Dispose()
    '            cmdMyCommand.Connection.Close()
    '        End If
    '    End Try
    'End Function

    Public Function GetDocDaElaborare(myStringConnection As String, ByVal sIdEnte As String, ByVal nIdRuolo As Integer) As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDocumentiElaborati()
        Dim nList As Integer
        Dim oListDocElab() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDocumentiElaborati = Nothing
        Dim oDocElab As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDocumentiElaborati
        Dim sSQL As String = ""
        Dim myDataView As New DataView

        Try
            Using ctx As New DBModel(ConstSession.DBType, myStringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetDocDaElaborare", "IDENTE", "IDFLUSSO_RUOLO")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", sIdEnte) _
                            , ctx.GetParam("IDFLUSSO_RUOLO", nIdRuolo)
                        )
                Catch ex As Exception
                    Log.Debug(sIdEnte + " - OPENgovTIA.GestDocumenti.GetDocDaElaborare.erroreQuery: ", ex)
                    Return Nothing
                Finally
                    ctx.Dispose()
                End Try
                For Each myRow As DataRowView In myDataView
                    oDocElab = New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDocumentiElaborati

                    oDocElab.IdFlusso = nIdRuolo
                    oDocElab.IdContribuente = -1
                    oDocElab.IdEnte = StringOperation.FormatString(myRow("IDENTE"))
                    oDocElab.CodiceCartella = StringOperation.FormatString(myRow("CODICE_CARTELLA"))
                    oDocElab.DataEmissione = StringOperation.FormatDateTime(myRow("DATA_EMISSIONE"))
                    oDocElab.IdModello = -1
                    oDocElab.CampoOrdinamento = ""
                    oDocElab.NumeroProgressivo = -1
                    oDocElab.NumeroFile = -1
                    oDocElab.Elaborato = False

                    nList += 1
                    ReDim Preserve oListDocElab(nList)
                    oListDocElab(nList) = oDocElab
                Next
            End Using
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDocumenti.GetDocDaElaborare.errore: ", Err)
            oListDocElab = Nothing
        Finally
            myDataView.Dispose()
        End Try
        Return oListDocElab
    End Function
    'Public Function GetDocDaElaborare(ByVal sIdEnte As String, ByVal nIdRuolo As Integer, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDocumentiElaborati()
    '    Dim nList As Integer
    '    Dim oListDocElab() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDocumentiElaborati = Nothing
    '    Dim oDocElab As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDocumentiElaborati
    '    Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim dtMyDati As New DataTable()
    '    Dim dtMyRow As DataRow

    '    Try
    '        If (Not (cmdMyCommandOut) Is Nothing) Then
    '            cmdMyCommand = cmdMyCommandOut
    '        Else
    '            cmdMyCommand = New SqlClient.SqlCommand
    '            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure

    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.AddWithValue("@IdEnte", sIdEnte)
    '        cmdMyCommand.Parameters.AddWithValue("@IDFLUSSO_RUOLO", nIdRuolo)
    '        cmdMyCommand.CommandText = "prc_GetDocDaElaborare"
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(dtMyDati)
    '        myAdapter.Dispose()
    '        For Each dtMyRow In dtMyDati.Rows
    '            oDocElab = New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDocumentiElaborati

    '            oDocElab.IdFlusso = nIdRuolo
    '            oDocElab.IdContribuente = -1
    '            oDocElab.IdEnte = StringOperation.FormatString(dtMyRow("IDENTE"))
    '            oDocElab.CodiceCartella = StringOperation.FormatString(dtMyRow("CODICE_CARTELLA"))
    '            oDocElab.DataEmissione = StringOperation.FormatDateTime(dtMyRow("DATA_EMISSIONE"))
    '            oDocElab.IdModello = -1
    '            oDocElab.CampoOrdinamento = ""
    '            oDocElab.NumeroProgressivo = -1
    '            oDocElab.NumeroFile = -1
    '            oDocElab.Elaborato = False

    '            nList += 1
    '            ReDim Preserve oListDocElab(nList)
    '            oListDocElab(nList) = oDocElab
    '        Next

    '        Return oListDocElab
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDocumenti.GetDocDaElaborare.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return Nothing
    '    Finally
    '        dtMyDati.Dispose()
    '        If (cmdMyCommandOut Is Nothing) Then
    '            cmdMyCommand.Dispose()
    '            cmdMyCommand.Connection.Close()
    '        End If
    '    End Try
    'End Function
    '*** ***
    '**** 201809 - Cartelle Insoluti ***
    Public Function GetDocElaborati(CodEnte As String, Tributo As String, idFlussoRuolo As Integer) As RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL()
        Dim oOggettoGruppoURL As RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL
        Dim oOggettoURL As RIBESElaborazioneDocumentiInterface.Stampa.oggetti.oggettoURL
        Dim ListDocElabEff As New ArrayList()
        Dim sSQL As String = ""
        Dim myDataView As New DataView

        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetDocEffettiviElaborati", "IDENTE", "TRIBUTO", "IDFLUSSORUOLO")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", CodEnte) _
                                , ctx.GetParam("TRIBUTO", Tributo) _
                                , ctx.GetParam("IDFLUSSORUOLO", idFlussoRuolo)
                            )
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDocumenti.GetDocElaborati.erroreQuery: ", ex)
                    Return Nothing
                Finally
                    ctx.Dispose()
                End Try
            End Using
            For Each myRow As DataRowView In myDataView
                oOggettoURL = New RIBESElaborazioneDocumentiInterface.Stampa.oggetti.oggettoURL
                oOggettoURL.Name = StringOperation.FormatString(myRow("NOME_FILE"))
                If oOggettoURL.Name = "Minuta Rate" Then
                    Exit For
                End If
                oOggettoURL.Path = StringOperation.FormatString(myRow("PATH"))
                oOggettoURL.Url = StringOperation.FormatString(myRow("PATH_WEB"))

                oOggettoGruppoURL = New RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL()
                oOggettoGruppoURL.URLComplessivo = oOggettoURL

                ListDocElabEff.Add(oOggettoGruppoURL)
            Next
            Return CType(ListDocElabEff.ToArray(GetType(RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL)), RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL())
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDocumenti.GetDocElaborati.errore: ", Err)
            Return Nothing
        End Try
    End Function
    Public Function ApprovaRuoloCartelleInsoluti(IdRuolo As Integer, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As Boolean
        Dim bRet As Boolean = True
        Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dtMyDati As New DataTable()
        Try
            If (Not (cmdMyCommandOut) Is Nothing) Then
                cmdMyCommand = cmdMyCommandOut
            Else
                cmdMyCommand = New SqlClient.SqlCommand
                cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
                cmdMyCommand.Connection.Open()
            End If
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.AddWithValue("@IDFLUSSO_RUOLO", IdRuolo)
            cmdMyCommand.CommandText = "prc_RuoloCartelleInsolutiApprova"
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(dtMyDati)
            myAdapter.Dispose()
            For Each dtMyRow As DataRow In dtMyDati.Rows
                If dtMyRow("ID") < 0 Then
                    bRet = False
                    Exit For
                End If
            Next
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDocumenti.ApprovaRuoloCartelleInsoluti.errore: ", ex)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            bRet = False
        Finally
            dtMyDati.Dispose()
            If (cmdMyCommandOut Is Nothing) Then
                cmdMyCommand.Dispose()
                cmdMyCommand.Connection.Close()
            End If
        End Try
        Return bRet
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="SourceDir"></param>
    ''' <param name="ZipFileName"></param>
    ''' <param name="ListFile"></param>
    ''' <returns></returns>
    Public Function ZipFile(SourceDir As String, ZipFileName As String, ListFile As ArrayList) As Boolean
        Try
            Dim astrFileNames() As String = Directory.GetFiles(SourceDir)
            Dim strmZipOutputStream As New ZipOutputStream(File.Create(SourceDir + ZipFileName))
            For Each myfile As String In astrFileNames
                For Each NameFile As String In ListFile
                    If myfile.ToLower = NameFile.ToLower Then
                        Dim entry As New ZipEntry(Path.GetFileName(myfile))
                        entry.DateTime = DateTime.Now
                        strmZipOutputStream.PutNextEntry(entry)
                        strmZipOutputStream.Write(File.ReadAllBytes(myfile), 0, File.ReadAllBytes(myfile).Length)
                        Exit For
                    End If
                Next
            Next
            strmZipOutputStream.Finish()
            strmZipOutputStream.Close()
            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsCoattivo.ZipFile.errore: ", ex)
            Return False
        End Try
    End Function
End Class

'*** 20131104 - TARES ***
''' <summary>
''' classe per la gestione del calcolo del ruolo.
''' Contiene la funzione asincrona che richiama i vari tipi di calcolo.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
''' <revisionHistory>
''' <revision date="12/04/2019">
''' <strong>Qualificazione AgID-analisi_rel01</strong>
''' <em>Analisi eventi</em>
''' </revision>
''' </revisionHistory>
Public Class ClsCalcoloRuolo
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ClsCalcoloRuolo))
    Private _IdEnte As String
    Private _Anno As Integer
    Private _TipoElaborazione As String
    Private _TipoCalcolo As String '{TARSU/TARI}
    Private _TipoRuolo As String '{P=percentuale,C=coattivo,I=insoluti}
    Private _DescrTipoCalcolo As String
    Private _PercentTariffe As Double
    Private _HasMaggiorazione As Boolean
    Private _HasConferimenti As Boolean
    Private _TipoMQ As String
    Private _impSogliaAvvisi As Double
    Private _IdTestata As Integer
    Private _IdContribuente As Integer
    Private _isSimula As Boolean
    Private _DBType As String
    Private _StringConnection As String
    Private _bIsFromVariabile As Boolean
    Private _tDataInizioConf As DateTime
    '*** 20181011 Dal/Al Conferimenti ***
    Private _tDataFineConf As DateTime
    Private _ListAddizionali() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale
    Private _cmdMyCommand As SqlClient.SqlCommand
    Private _cmdMyCommandNoTrans As SqlClient.SqlCommand
    Private _myTrans As SqlClient.SqlTransaction
    Private _ListFlussi As ArrayList
    Private _Belfiore As String
    Private _StringConnectionAnag As String
    Private _StringConnectionOPENgov As String
    Private _StringConnectionICI As String
    Private _StringConnectionProvv As String
    Private _URLServiziLiquidazione As String
    Private _URLServiziAccertamenti As String
    Private _Operatore As String
    '**** 201809 - Cartelle Insoluti ***
    Private _GGScadenza As Integer
    Private _DataScadenza As Date
    Private _impSpeseIngiunzione As Double

    Public Sub New()
        MyBase.New()
    End Sub

    '*** 20181011 Dal/Al Conferimenti ***'*** 201809 Bollettazione Vigliano in OPENgov*** '**** 201809 - Cartelle Insoluti ***
    Public Sub StartCalcolo(TypeDB As String, ByVal myStringConnection As String, ByVal IdEnte As String, ByVal sTipoElaborazione As String, ByVal TipoTassazione As String, ByVal TipoCalcolo As String, ByVal sDescrTipoCalcolo As String, ByVal PercentTariffe As Double, ByVal HasMaggiorazione As Boolean, ByVal HasConferimenti As Boolean, ByVal TipoMQ As String, ByVal Anno As Integer, ByVal impSogliaAvvisi As Double, ByVal nIdTestata As Integer, ByVal isSimula As Boolean, ByVal bIsFromVariabile As Boolean, tDataInizioConf As DateTime, tDataFineConf As DateTime, ListAddizionali() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale, ListFlussi As ArrayList, CodBelfiore As String, StringConnectionAnag As String, StringConnectionOPENgov As String, StringConnectionICI As String, StringConnectionProvv As String, URLServiziLiquidazione As String, URLServiziAccertamenti As String, Operatore As String, GGScadenza As Integer, impSpeseIngiunzione As Double)
        Dim threadDelegate As ThreadStart = New ThreadStart(AddressOf StartCalcoloThreadEntryPoint)
        Dim t As Thread = New Thread(threadDelegate)
        _IdEnte = IdEnte
        _Anno = Anno
        _TipoElaborazione = sTipoElaborazione
        _TipoCalcolo = TipoTassazione
        _TipoRuolo = TipoCalcolo
        _DescrTipoCalcolo = sDescrTipoCalcolo
        _PercentTariffe = PercentTariffe
        _HasMaggiorazione = HasMaggiorazione
        _HasConferimenti = HasConferimenti
        _TipoMQ = TipoMQ
        _impSogliaAvvisi = impSogliaAvvisi
        _IdTestata = nIdTestata
        _isSimula = isSimula
        _DBType = TypeDB
        _StringConnection = myStringConnection
        _bIsFromVariabile = bIsFromVariabile
        _tDataInizioConf = tDataInizioConf
        _tDataFineConf = tDataFineConf
        _ListAddizionali = ListAddizionali
        _ListFlussi = ListFlussi
        _Belfiore = CodBelfiore
        _StringConnectionAnag = StringConnectionAnag
        _StringConnectionOPENgov = StringConnectionOPENgov
        _StringConnectionICI = StringConnectionICI
        _StringConnectionProvv = StringConnectionProvv
        _URLServiziLiquidazione = URLServiziLiquidazione
        _URLServiziAccertamenti = URLServiziAccertamenti
        _Operatore = Operatore
        _GGScadenza = GGScadenza
        _impSpeseIngiunzione = impSpeseIngiunzione

        ' Inizio la transazione e recupero l'istanza della connessione al db
        ' Devo farlo qui visto che sto eseguendo codice ancora nel thread della PostBack da FE, per cui ho il HttpContext disponibile con tutti i suoi oggetti (DichiarazioneSession, HttpSession, ecc.), mentre nel thread non ho queste informazioni
        _cmdMyCommand = StartCalcoloMassivoTransaction()
        _cmdMyCommandNoTrans = OpenCalcoloMassivoConnection()
        _myTrans = _cmdMyCommand.Transaction
        t.Start()
    End Sub
    ''' <summary>
    ''' Funzione che richiama, in abse al tipo di ruolol e di elaborazione la specifica funzione di calcolo del ruolo
    ''' </summary>
    ''' <revisionHistory>
    ''' <revision date="09/2018">
    ''' <strong>Cartelle Insoluti</strong>
    ''' <strong>Bollettazione Vigliano in OPENgov</strong>
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Analisi eventi</em>
    ''' </revision>
    ''' </revisionHistory>
    Private Sub StartCalcoloThreadEntryPoint()
        CacheManager.SetCalcoloMassivoInCorso(_Anno)
        Dim MyError As Boolean = False

        Try
            Log.Debug("StartCalcoloThreadEntryPoint::inizio")
            '**** 201809 - Cartelle Insoluti ***
            If _TipoRuolo = ObjRuolo.Ruolo.CartelleInsoluti Then
                MyError = CalcoloCartelleInsoluti()
            Else
                Select Case _TipoElaborazione
                    Case ObjRuolo.Generazione.DaDichiarazione 'automatico
                        MyError = CalcoloCartelle()
                    Case ObjRuolo.Generazione.Manuale 'manuale
                            'andrà ad estrapolare tutte le posizioni ordinarie inserite a fronte della funzionalità di data-entry manuale posizioni a ruolo NON ancora entrare a far parte di un ruolo e ne genererà uno nuovo
                            '*** 201809 Bollettazione Vigliano in OPENgov***
                    Case ObjRuolo.Generazione.DaFlusso
                        CacheManager.SetCalcoloMassivoInCorso(0)
                        MyError = ImportRuolo()
                End Select
            End If

            If MyError = True Then
                RollbackCalcoloMassivoTransaction(_myTrans, _cmdMyCommand)
                CacheManager.RemoveRiepilogoImportRuoli()
            Else
                CommitCalcoloMassivoTransaction(_myTrans, _cmdMyCommand)
                Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, _StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, _Operatore, New Utility.Costanti.LogEventArgument().Elaborazioni, "StartCalcoloThreadEntryPoint", Utility.Costanti.AZIONE_NEW, Utility.Costanti.TRIBUTO_TARSU, _IdEnte, CType(CacheManager.GetRiepilogoCalcoloMassivo(), ObjRuolo())(0).IdFlusso)
            End If
        Catch Err As Exception
            Log.Debug(_IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.CalcolaRuolo.StartCalcoloThreadEntryPoint.errore: ", Err)
            RollbackCalcoloMassivoTransaction(_myTrans, _cmdMyCommand)
            CacheManager.RemoveRiepilogoImportRuoli()
        Finally
            CacheManager.RemoveCalcoloMassivoInCorso()
            CacheManager.RemoveAvanzamentoElaborazione()
        End Try
    End Sub
    'Private Sub StartCalcoloThreadEntryPoint()
    '    CacheManager.SetCalcoloMassivoInCorso(_Anno)
    '    Dim MyError As Boolean = False

    '    Try
    '        Log.Debug("StartCalcoloThreadEntryPoint::inizio")
    '        '**** 201809 - Cartelle Insoluti ***
    '        If _TipoRuolo = ObjRuolo.Ruolo.CartelleInsoluti Then
    '            MyError = CalcoloCartelleInsoluti()
    '        Else
    '            Select Case _TipoElaborazione
    '                Case ObjRuolo.Generazione.DaDichiarazione 'automatico
    '                    MyError = CalcoloCartelle()
    '                Case ObjRuolo.Generazione.Manuale 'manuale
    '                        'andrà ad estrapolare tutte le posizioni ordinarie inserite a fronte della funzionalità di data-entry manuale posizioni a ruolo NON ancora entrare a far parte di un ruolo e ne genererà uno nuovo
    '                        '*** 201809 Bollettazione Vigliano in OPENgov***
    '                Case ObjRuolo.Generazione.DaFlusso
    '                    CacheManager.SetCalcoloMassivoInCorso(0)
    '                    MyError = ImportRuolo()
    '            End Select
    '        End If

    '        If MyError = True Then
    '            RollbackCalcoloMassivoTransaction(_myTrans, _cmdMyCommand)
    '            CacheManager.RemoveRiepilogoImportRuoli()
    '        Else
    '            CommitCalcoloMassivoTransaction(_myTrans, _cmdMyCommand)
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.CalcolaRuolo.StartCalcoloThreadEntryPoint.errore: ", Err)
    '        RollbackCalcoloMassivoTransaction(_myTrans, _cmdMyCommand)
    '        CacheManager.RemoveRiepilogoImportRuoli()
    '    Finally
    '        CacheManager.RemoveCalcoloMassivoInCorso()
    '        CacheManager.RemoveAvanzamentoElaborazione()
    '    End Try


    '    'Dim elab As ElaborazioneEffettuata = New ElaborazioneEffettuata
    '    'Dim dao As CartelleDAO = New CartelleDAO
    '    'elab.IdEnte = _IdEnte
    '    'elab.Anno = _Anno
    '    ''*** 20130610 - ruolo supplettivo ***
    '    'elab.TipoRuolo = CType(_TipoRuolo, Integer)
    '    ''*** ***
    '    'elab.DataOraDocumentiStampati = DichiarazioneSession.MyDateMinValue
    '    'elab.DataOraDocumentiApprovati = DichiarazioneSession.MyDateMinValue
    '    'elab.DataOraMinutaApprovata = DichiarazioneSession.MyDateMinValue
    '    'elab.DataOraMinutaStampata = DichiarazioneSession.MyDateMinValue
    '    'elab.DataOraCalcoloRate = DichiarazioneSession.MyDateMinValue
    '    'elab.ImportoTotale = 0
    '    'elab.NArticoli = 0
    '    'elab.DataOraInizioElaborazione = DateTime.Now
    '    'elab.SogliaMinimaRate = _ImportoRate
    '    'Try
    '    '    Dim Error As Boolean = false
    '    '    Dim ErrorMessage As String = String.Empty
    '    '    Dim utenti As Hashtable = New Hashtable
    '    '    Dim dichiarazioniCalcolo As Hashtable = New Hashtable
    '    '    Dim bInserimento As Boolean = False
    '    '    ' Recupero gli ID di tutte le dichiarazioni da elaborare
    '    '    Dim ListaDichiarazioni() As Integer = DTO.MetodiDichiarazioneTosapCosap.GetIdDichiarazioniAnno(_Anno, _IdEnte, -1, _dbEngineNoTrans)
    '    '    '                WSMotoreOSAP.MotoreTOCO motore = new WSMotoreOSAP.MotoreTOCO ();
    '    '    '                motore.Url= ConfigurationManager.AppSettings["UrlMotoreTOCO"].ToString();
    '    '    Dim motore As IRemotingInterfaceOSAP = CType(Activator.GetObject(GetType(IRemotingInterfaceOSAP), ConfigurationManager.AppSettings("UrlMotoreTOCO").ToString), IRemotingInterfaceOSAP)
    '    '    Dim i As Integer = 0
    '    '    Do While ((i < ListaDichiarazioni.Length)  _
    '    '                AndAlso Not Error)
    '    '        Dim IdDichiarazione As Integer = ListaDichiarazioni(i)
    '    '        ' Ottengo la dichiarazione con tutti gli articoli
    '    '        Dim Dichiarazione As DichiarazioneTosapCosap = MetodiDichiarazioneTosapCosap.GetDichiarazioneForMotore(IdDichiarazione, _IdEnte, _CodTributo, _Anno, _dbEngineNoTrans)
    '    '        Dim cart As Cartella = CreateCartella(_Anno, Dichiarazione, Dichiarazione.ArticoliDichiarazione.Length)
    '    '        ' Scorro tutti gli articoli della dichiarazione e memorizzo i ruoli
    '    '        Dim j As Integer = 0
    '    '        Do While ((j < Dichiarazione.ArticoliDichiarazione.Length)  _
    '    '                    AndAlso Not Error)
    '    '            Dim a As Articolo = Dichiarazione.ArticoliDichiarazione(j)
    '    '            '*** 20130610 - ruolo supplettivo ***
    '    '            Dim ArticoloOrdinario As CalcoloResult = Nothing
    '    '            'se sono in supplettivo prelevo l'eventuale articolo giè generato per il dettagliotestata e anno
    '    '            If (_TipoRuolo = Ruolo.E_TIPO.SUPPLETIVO) Then
    '    '                If (bInserimento = False) Then
    '    '                    'Log.Debug("StartCalcoloThreadEntryPoint::richiamo notrans");
    '    '                    ArticoloOrdinario = MetodiArticolo.GetArticoloPrecedente(_IdEnte, Dichiarazione.ArticoliDichiarazione(j).IdArticoloPadre, _Anno, _dbEngineNoTrans)
    '    '                Else
    '    '                    'Log.Debug("StartCalcoloThreadEntryPoint::richiamo trans");
    '    '                    ArticoloOrdinario = MetodiArticolo.GetArticoloPrecedente(_IdEnte, Dichiarazione.ArticoliDichiarazione(j).IdArticoloPadre, _Anno, _dbEngine)
    '    '                End If
    '    '                'if (ArticoloOrdinario!=null)
    '    '                '{
    '    '                'Log.Debug("StartCalcoloThreadEntryPoint::importo precedente::"+ArticoloOrdinario.ImportoCalcolato.ToString());
    '    '                '}
    '    '                'else
    '    '                '{
    '    '                'Log.Debug("StartCalcoloThreadEntryPoint::importo precedente non trovato");
    '    '                '}
    '    '            End If
    '    '            ' Richiamo il motore di calcolo
    '    '            Dim result As CalcoloResult = motore.CalcolaOSAP(_TipoRuolo, a, _Categ, _TipiOcc, _Agev, _Tarif, ArticoloOrdinario)
    '    '            '*** ***
    '    '            If (result.Result = E_CALCOLORESULT.OK) Then
    '    '                'Log.Debug("StartCalcoloThreadEntryPoint::calcolato per idcontribuente::"+Dichiarazione.AnagraficaContribuente.COD_CONTRIBUENTE.ToString()+"::IdArticolo::"+ Dichiarazione.ArticoliDichiarazione[j].IdArticolo.ToString()+"::ImportoCalcolato::"+result.ImportoCalcolato.ToString());
    '    '                If (Double.Parse(String.Format("{0:0.00}", result.ImportoCalcolato)) > 0) Then
    '    '                    elab.NArticoli = (elab.NArticoli + 1)
    '    '                    Dim ImportoDueDecimali As Double = Double.Parse(String.Format("{0:0.00}", result.ImportoCalcolato))
    '    '                    elab.ImportoTotale = (elab.ImportoTotale + ImportoDueDecimali)
    '    '                    cart.ImportoTotale = (cart.ImportoTotale + ImportoDueDecimali)
    '    '                    'Log.Debug("StartCalcoloThreadEntryPoint::popolo articolo n."+ j.ToString() +" per idcontribuente::"+Dichiarazione.AnagraficaContribuente.COD_CONTRIBUENTE.ToString()+"::IdArticolo::"+ Dichiarazione.ArticoliDichiarazione[j].IdArticolo.ToString()+"::ImportoCalcolato::"+result.ImportoCalcolato.ToString());
    '    '                    cart.Ruoli(j) = CreateRuolo(result.ImportoCalcolato, a.IdArticolo)
    '    '                    cart.Ruoli(j).ImportoLordo = Double.Parse(String.Format("{0:0.00}", result.ImportoLordo))
    '    '                    cart.Ruoli(j).Tariffa = New Tariffe
    '    '                    cart.Ruoli(j).Tariffa.Valore = result.TariffaApplicata
    '    '                    cart.IdEnte = elab.IdEnte
    '    '                End If
    '    '            Else
    '    '                Error = true
    '    '                Select Case (result.Result)
    '    '                End Select
    '    '                ErrorMessage = "Si è verificato un errore di calcolo durante "
    '    '                Exit For
    '    '                ErrorMessage = "Non è stata trovata la categoria corretta per "
    '    '                Exit For
    '    '                ErrorMessage = "Non è stata trovata la tariffa corretta per "
    '    '                Exit For
    '    '                ErrorMessage = "Non è stata trovata la tipologia corretta per "
    '    '                Exit For
    '    '            End If
    '    '            ErrorMessage = (ErrorMessage + (" l'elaborazione dell'articolo n. " _
    '    '                        + ((j + 1) + (", tipologia \""" _
    '    '                        + (a.TipologiaOccupazione.Descrizione + ("\"" categoria " _
    '    '                        + (a.Categoria.Descrizione + ("\"" durata " _
    '    '                        + (a.TipoDurata.Descrizione + ("\"" della dichiarazione n. " + Dichiarazione.TestataDichiarazione.NDichiarazione))))))))))
    '    '            Exit For
    '    '            j = (j + 1)
    '    '        Loop
    '    '        i = (i + 1)
    '    '    Loop
    '    '    ' end for (j < Dichiarazione.ArticoliDichiarazione.Length)
    '    '    ' Creo la cartella con i ruoli
    '    '    If ((ErrorMessage = String.Empty) _
    '    '                AndAlso (cart.ImportoTotale > 0)) Then
    '    '        'Log.Debug("StartCalcoloThreadEntryPoint::inserisco cartella per idcontribuente::"+cart.CodContribuente.ToString());
    '    '        MetodiCartella.InsertCartella(cart, _dbEngine)
    '    '        bInserimento = True
    '    '        ' Salvo in una hashtable tutti gli utenti per memorizzare le
    '    '        ' statistiche di elaborazione alla fine
    '    '        If Not utenti.ContainsKey(Dichiarazione.AnagraficaContribuente.COD_CONTRIBUENTE) Then
    '    '            utenti.Add(Dichiarazione.AnagraficaContribuente.COD_CONTRIBUENTE, Dichiarazione.AnagraficaContribuente.COD_CONTRIBUENTE)
    '    '        End If
    '    '        If Not dichiarazioniCalcolo.ContainsKey(Dichiarazione.IdDichiarazione) Then
    '    '            dichiarazioniCalcolo.Add(Dichiarazione.IdDichiarazione, Dichiarazione.IdDichiarazione)
    '    '        End If
    '    '    End If
    '    '    'else
    '    '    '{
    '    '    'Log.Debug("StartCalcoloThreadEntryPoint::NON inserisco cartella per idcontribuente::"+cart.CodContribuente.ToString());
    '    '    '}
    '    ' Catch Err As Exception
    '    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.CalcolaRuolo.StartCalcoloThreadEntryPoint.errore: ", Err)
    '    'End Try
    '    'elab.DataOraFineElaborazione = DateTime.Now
    '    'elab.Note = ErrorMessage
    '    ''*** 20130610 - ruolo supplettivo ***
    '    ''elab.NDichiarazioni = ListaDichiarazioni.Length;
    '    'elab.NUtenti = utenti.Keys.Count
    '    'elab.NDichiarazioni = dichiarazioniCalcolo.Keys.Count
    '    ''*** ***
    '    'MetodiElaborazioneEffettuata.SetElaborazioneEffettuata(elab, _dbEngine)
    '    'If Not Error Then
    '    '    ' Inserimento lotto
    '    '    Dim l As Lotto = New Lotto
    '    '    l.IdEnte = _IdEnte
    '    '    l.Anno = _Anno
    '    '    MetodiLotto.InsertLotto(l, _dbEngine)
    '    '    'MetodiElaborazioneEffettuata.SetElaborazioneEffettuata (elab, ref _dbEngine);
    '    '    dao.CommitCalcoloMassivoTransaction(_dbEngine)
    '    'Else
    '    '    dao.RollbackCalcoloMassivoTransaction(_dbEngine)
    '    'End If
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="TypeDB"></param>
    ''' <param name="myStringConnection"></param>
    ''' <param name="IdEnte"></param>
    ''' <param name="sTipoElaborazione"></param>
    ''' <param name="TipoTassazione"></param>
    ''' <param name="TipoCalcolo"></param>
    ''' <param name="sDescrTipoCalcolo"></param>
    ''' <param name="PercentTariffe"></param>
    ''' <param name="HasMaggiorazione"></param>
    ''' <param name="HasConferimenti"></param>
    ''' <param name="TipoMQ"></param>
    ''' <param name="Anno"></param>
    ''' <param name="impSogliaAvvisi"></param>
    ''' <param name="nIdTestata"></param>
    ''' <param name="nIdContribuente"></param>
    ''' <param name="isSimula"></param>
    ''' <param name="bIsFromVariabile"></param>
    ''' <param name="tDataInizioConf"></param>
    ''' <param name="tDataFineConf"></param>
    ''' <param name="ListAddizionali"></param>
    ''' <param name="ListFlussi"></param>
    ''' <param name="CodBelfiore"></param>
    ''' <param name="StringConnectionAnag"></param>
    ''' <param name="StringConnectionOPENgov"></param>
    ''' <param name="StringConnectionICI"></param>
    ''' <param name="StringConnectionProvv"></param>
    ''' <param name="URLServiziLiquidazione"></param>
    ''' <param name="URLServiziAccertamenti"></param>
    ''' <param name="Operatore"></param>
    ''' <param name="GGScadenza"></param>
    ''' <param name="DataScadenza"></param>
    ''' <returns></returns>
    Public Function StartCalcoloPuntuale(TypeDB As String, ByVal myStringConnection As String, ByVal IdEnte As String, ByVal sTipoElaborazione As String, ByVal TipoTassazione As String, ByVal TipoCalcolo As String, ByVal sDescrTipoCalcolo As String, ByVal PercentTariffe As Double, ByVal HasMaggiorazione As Boolean, ByVal HasConferimenti As Boolean, ByVal TipoMQ As String, ByVal Anno As Integer, ByVal impSogliaAvvisi As Double, ByVal nIdTestata As Integer, nIdContribuente As Integer, ByVal isSimula As Boolean, ByVal bIsFromVariabile As Boolean, tDataInizioConf As DateTime, tDataFineConf As DateTime, ListAddizionali() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale, ListFlussi As ArrayList, CodBelfiore As String, StringConnectionAnag As String, StringConnectionOPENgov As String, StringConnectionICI As String, StringConnectionProvv As String, URLServiziLiquidazione As String, URLServiziAccertamenti As String, Operatore As String, GGScadenza As Integer, DataScadenza As Date) As ObjRuolo
        Dim MyError As Boolean = False
        Dim listRuoli() As ObjRuolo = Nothing
        Dim myRuolo As New ObjRuolo

        Try
            Log.Debug("StartCalcoloPuntuale::inizio")
            _IdEnte = IdEnte
            _Anno = Anno
            _TipoElaborazione = sTipoElaborazione
            _TipoCalcolo = TipoTassazione
            _TipoRuolo = TipoCalcolo
            _DescrTipoCalcolo = sDescrTipoCalcolo
            _PercentTariffe = PercentTariffe
            _HasMaggiorazione = HasMaggiorazione
            _HasConferimenti = HasConferimenti
            _TipoMQ = TipoMQ
            _impSogliaAvvisi = impSogliaAvvisi
            _IdTestata = nIdTestata
            _IdContribuente = nIdContribuente
            _isSimula = isSimula
            _DBType = TypeDB
            _StringConnection = myStringConnection
            _bIsFromVariabile = bIsFromVariabile
            _tDataInizioConf = tDataInizioConf
            _tDataFineConf = tDataFineConf
            _ListAddizionali = ListAddizionali
            _ListFlussi = ListFlussi
            _Belfiore = CodBelfiore
            _StringConnectionAnag = StringConnectionAnag
            _StringConnectionOPENgov = StringConnectionOPENgov
            _StringConnectionICI = StringConnectionICI
            _StringConnectionProvv = StringConnectionProvv
            _URLServiziLiquidazione = URLServiziLiquidazione
            _URLServiziAccertamenti = URLServiziAccertamenti
            _Operatore = Operatore
            _GGScadenza = GGScadenza
            _DataScadenza = DataScadenza

            ' Inizio la transazione e recupero l'istanza della connessione al db
            ' Devo farlo qui visto che sto eseguendo codice ancora nel thread della PostBack da FE, per cui ho il HttpContext disponibile con tutti i suoi oggetti (DichiarazioneSession, HttpSession, ecc.), mentre nel thread non ho queste informazioni
            _cmdMyCommand = StartCalcoloMassivoTransaction()
            _cmdMyCommandNoTrans = OpenCalcoloMassivoConnection()
            _myTrans = _cmdMyCommand.Transaction
            Try
                CacheManager.SetCalcoloMassivoInCorso(_Anno)

                If _TipoRuolo = ObjRuolo.Ruolo.CartelleInsoluti Then
                    MyError = CalcoloCartelleInsoluti()
                Else
                    Select Case _TipoElaborazione
                        Case ObjRuolo.Generazione.DaDichiarazione 'automatico
                            MyError = CalcoloCartelle()
                        Case ObjRuolo.Generazione.Manuale 'manuale
                            'andrà ad estrapolare tutte le posizioni ordinarie inserite a fronte della funzionalità di data-entry manuale posizioni a ruolo NON ancora entrare a far parte di un ruolo e ne genererà uno nuovo
                        Case ObjRuolo.Generazione.DaFlusso
                            CacheManager.SetCalcoloMassivoInCorso(0)
                            MyError = ImportRuolo()
                    End Select
                End If

                If MyError = True Then
                    RollbackCalcoloMassivoTransaction(_myTrans, _cmdMyCommand)
                    CacheManager.RemoveRiepilogoImportRuoli()
                Else
                    CommitCalcoloMassivoTransaction(_myTrans, _cmdMyCommand)
                    listRuoli = CacheManager.GetRiepilogoCalcoloMassivo
                    If Not listRuoli Is Nothing Then
                        Dim oListRate() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata = Nothing
                        Dim oMyConfigRata As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata
                        Dim x As Integer = 0
                        Try

                            'ciclo sulla griglia per popolarmi i dati da inserire
                            oMyConfigRata = New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata
                            oMyConfigRata.NumeroRata = "U"
                            oMyConfigRata.DescrizioneRata = "Unica Soluzione"
                            oMyConfigRata.DataScadenza = _DataScadenza
                            oMyConfigRata.Percentuale = 100
                            oMyConfigRata.HasImposta = True
                            oMyConfigRata.HasMaggiorazione = False
                            oMyConfigRata.sTipoBollettino = RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata.BOLLETTINO_F24
                            oMyConfigRata.impSoglia = 0
                            ReDim Preserve oListRate(x)
                            oListRate(x) = oMyConfigRata
                            'inserisco le rate
                            For x = 0 To oListRate.GetUpperBound(0)
                                If New GestRata().SetRataConfigurata(_StringConnection, oListRate(x), listRuoli(0).IdFlusso, Utility.Costanti.AZIONE_NEW, _Operatore) < 1 Then
                                    Throw New Exception("Errore in inserimento rate!")
                                End If
                            Next
                        Catch Err As Exception
                            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ConfRate.CmdSalva_Click.errore: ", Err)
                        End Try
                        'eseguo il calcolo delle rate
                        listRuoli(0) = New ClsElabRuolo().CalcoloRate(_StringConnection, listRuoli(0), -1)
                        myRuolo = listRuoli(0)
                    End If
                    If Not myRuolo Is Nothing Then
                        Dim fncActionEvent As New Utility.DBUtility(_DBType, _StringConnectionOPENgov)
                        fncActionEvent.LogActionEvent(DateTime.Now, _Operatore, New Utility.Costanti.LogEventArgument().Elaborazioni, "CalcoloPuntuale", Utility.Costanti.AZIONE_NEW, Utility.Costanti.TRIBUTO_TARSU, _IdEnte, CType(CacheManager.GetRiepilogoCalcoloMassivo(), ObjRuolo())(0).IdFlusso)
                    End If
                End If
            Catch Err As Exception
                Log.Debug(_IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.CalcolaRuolo.StartCalcoloPuntuale.errore: ", Err)
                RollbackCalcoloMassivoTransaction(_myTrans, _cmdMyCommand)
                CacheManager.RemoveRiepilogoImportRuoli()
                myRuolo = Nothing
            Finally
                CacheManager.RemoveCalcoloMassivoInCorso()
                CacheManager.RemoveAvanzamentoElaborazione()
                CacheManager.RemoveRiepilogoCalcoloMassivo()
                CacheManager.RemoveRiepilogoImportRuoli()
            End Try
        Catch Err As Exception
            Log.Debug(_IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.CalcolaRuolo.StartCalcoloPuntuale.errore: ", Err)
            myRuolo = Nothing
        End Try
        Return myRuolo
    End Function
    ''' <summary>
    ''' andrà ad estrapolare, per l’anno scelto, tutte le situazioni immobili attive o chiuse nell’anno e per ogni contribuente calcolerà l’importo totale dovuto in base ai metri registrati a sistema, basandosi sulle tariffe deliberate dal Comune e rapportando il tutto ai bimestri di occupazione.	
    ''' </summary>
    ''' <returns></returns>
    Private Function CalcoloCartelle() As Boolean
        Dim HasError As Boolean = False
        Dim x, nIdRuolo As Integer
        Dim oMyRuolo() As ObjRuolo
        Dim elab As New ClsElabRuolo
        Dim FncRuolo As New ClsGestRuolo
        Dim oSingleRiepilogo As New ObjRuolo
        Dim FncDB As New DichManagerTARSU(_DBType, _StringConnection, _StringConnectionOPENgov, _IdEnte)
        Dim dtMyDati As New DataTable()
        Dim sAvanzamento As String
        Dim nAvanzamento As Integer = 0

        Try
            'calcolo gli articoli di ruolo e il totalizzatore
            '*** 20181011 Dal/Al Conferimenti ***
            oMyRuolo = elab.CalcolaRuoloFromDich(_StringConnection, _IdEnte, _Anno, _TipoCalcolo, _TipoRuolo, _DescrTipoCalcolo, _PercentTariffe, _HasMaggiorazione, _HasConferimenti, _TipoMQ, _impSogliaAvvisi, _IdTestata, _IdContribuente, _tDataInizioConf, _tDataFineConf)
            'controllo se ho un riepilogo articoli
            If Not oMyRuolo Is Nothing Then
                'ciclo sugli anni calcolati
                For x = 0 To oMyRuolo.GetUpperBound(0)
                    oSingleRiepilogo = New ObjRuolo
                    'controllo i sotto soglia
                    Log.Debug("GeneraRuolo::devo controllare la soglia")
                    sAvanzamento = "Controllo soglia"
                    CacheManager.SetAvanzamentoElaborazione(sAvanzamento)
                    oSingleRiepilogo = elab.CheckSogliaRuolo(oMyRuolo(x))
                    If oSingleRiepilogo Is Nothing Then
                        HasError = True
                        Exit For
                    End If
                    'aggiorno l'oggetto che dovrò restituire
                    oMyRuolo(x) = oSingleRiepilogo
                    'registro il ruolo nella tabella dei totalizzatori
                    oSingleRiepilogo = elab.ValorizzaTotRuolo(_TipoCalcolo, _IdEnte, _TipoElaborazione, _DescrTipoCalcolo, oMyRuolo(x))
                    If _isSimula = False Then
                        If Not IsNothing(oSingleRiepilogo.oAvvisi) Then
                            nIdRuolo = FncRuolo.SetRuolo(Utility.Costanti.AZIONE_NEW, oSingleRiepilogo, _StringConnection)
                        End If
                        If nIdRuolo > 0 Then
                            'ciclo sugli avvisi calcolati
                            For Each myAvviso As ObjAvviso In oSingleRiepilogo.oAvvisi
                                nAvanzamento += 1
                                sAvanzamento = "Inserimento posizione " & nAvanzamento & " su " & oSingleRiepilogo.oAvvisi.GetUpperBound(0) + 1
                                CacheManager.SetAvanzamentoElaborazione(sAvanzamento)
                                'registro l'avviso nella tabella
                                myAvviso.IdFlussoRuolo = nIdRuolo
                                myAvviso = PulisciDate(myAvviso)
                                If FncDB.SetAvvisoCompleto(myAvviso, _bIsFromVariabile, False) = 0 Then
                                    HasError = True
                                    Exit For
                                End If
                            Next
                            'controllo che tutti gli articoli aventi importo riduzione abbiano la corrispettiva riga in articoliriduzioni
                            Dim sSQL As String = ""

                            Try
                                Using ctx As New DBModel(ConstSession.DBType, _StringConnection)
                                    Try
                                        sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_ControlloRidVSArticoli", "IDFLUSSO")
                                        ctx.ExecuteNonQuery(sSQL, ctx.GetParam("IDFLUSSO", nIdRuolo))
                                    Catch ex As Exception
                                        Log.Debug(_IdEnte + "." + _Operatore + " - OPENgovTIA.CalcolaRuolo.CalcoloCartelle.erroreQuery: ", ex)
                                        HasError = True
                                        Exit For
                                    Finally
                                        ctx.Dispose()
                                    End Try
                                End Using
                            Catch Err As Exception
                                Log.Debug(_IdEnte + "." + _Operatore + " - OPENgovTIA.CalcolaRuolo.CalcoloCartelle.errore: ", Err)
                                HasError = True
                                Exit For
                            End Try
                            'eseguo la cartellazione
                            sAvanzamento = "Cartellazione"
                            CacheManager.SetAvanzamentoElaborazione(sAvanzamento)
                            oSingleRiepilogo = elab.CartellaRuolo(_DBType, _StringConnection, oSingleRiepilogo)
                            If oSingleRiepilogo Is Nothing Then
                                HasError = True
                                Exit For
                            End If
                            'aggiorno con i totali cartellati
                            nIdRuolo = FncRuolo.SetRuolo(Utility.Costanti.AZIONE_UPDATE, oSingleRiepilogo, _StringConnection)
                        End If
                    End If
                    'popolo l'oggetto per il raffronto cat-importi
                    SetArticoliVSCat(oSingleRiepilogo)
                    CacheManager.SetRiepilogoCalcoloMassivo(oMyRuolo)
                Next
            Else
                HasError = True
            End If
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgovTIA.CalcolaRuolo.CalcoloCartelle.errore: ", ex)
            HasError = True
        End Try
        Return HasError
    End Function
    'Private Function CalcoloCartelle() As Boolean
    '    Dim HasError As Boolean = False
    '    Dim x, nIdRuolo As Integer
    '    Dim oMyRuolo() As ObjRuolo
    '    Dim elab As New ClsElabRuolo
    '    Dim FncRuolo As New ClsGestRuolo
    '    Dim oSingleRiepilogo As New ObjRuolo
    '    Dim FncDB As New DichManagerTARSU(_DBType, _StringConnection, _StringConnectionOPENgov, _IdEnte)
    '    Dim dtMyDati As New DataTable()
    '    Dim sAvanzamento As String
    '    Dim nAvanzamento As Integer = 0

    '    Try
    '        'andrà ad estrapolare, per l’anno scelto, tutte le situazioni immobili attive o chiuse nell’anno e per ogni contribuente calcolerà l’importo totale dovuto in base ai metri registrati a sistema, basandosi sulle tariffe deliberate dal Comune e rapportando il tutto ai bimestri di occupazione.	
    '        'calcolo gli articoli di ruolo e il totalizzatore
    '        'oMyRuolo = elab.CalcolaRuoloFromDich(_IdEnte, _Anno, _TipoTassazione, _TipoCalcolo, _DescrTipoCalcolo, _PercentTariffe, _HasMaggiorazione, _HasConferimenti, _TipoMQ, _impSogliaAvvisi, _IdTestata, -1, _dbEngineNoTrans)
    '        '*** 20181011 Dal/Al Conferimenti ***
    '        oMyRuolo = elab.CalcolaRuoloFromDich(_StringConnection, _IdEnte, _Anno, _TipoCalcolo, _TipoRuolo, _DescrTipoCalcolo, _PercentTariffe, _HasMaggiorazione, _HasConferimenti, _TipoMQ, _impSogliaAvvisi, _IdTestata, -1, _tDataInizioConf, _tDataFineConf)
    '        'controllo se ho un riepilogo articoli
    '        If Not oMyRuolo Is Nothing Then
    '            'ciclo sugli anni calcolati
    '            For x = 0 To oMyRuolo.GetUpperBound(0)
    '                oSingleRiepilogo = New ObjRuolo
    '                'controllo i sotto soglia
    '                Log.Debug("GeneraRuolo::devo controllare la soglia")
    '                sAvanzamento = "Controllo soglia"
    '                CacheManager.SetAvanzamentoElaborazione(sAvanzamento)
    '                oSingleRiepilogo = elab.CheckSogliaRuolo(oMyRuolo(x))
    '                If oSingleRiepilogo Is Nothing Then
    '                    HasError = True
    '                    Exit For
    '                End If
    '                'aggiorno l'oggetto che dovrò restituire
    '                oMyRuolo(x) = oSingleRiepilogo
    '                'registro il ruolo nella tabella dei totalizzatori
    '                oSingleRiepilogo = elab.ValorizzaTotRuolo(_TipoCalcolo, _IdEnte, _TipoElaborazione, _DescrTipoCalcolo, oMyRuolo(x))
    '                If _isSimula = False Then
    '                    If Not IsNothing(oSingleRiepilogo.oAvvisi) Then
    '                        nIdRuolo = FncRuolo.SetRuolo(Utility.Costanti.AZIONE_NEW, oSingleRiepilogo, _StringConnection, _cmdMyCommand)
    '                    End If
    '                    If nIdRuolo > 0 Then
    '                        'ciclo sugli avvisi calcolati
    '                        For Each myAvviso As ObjAvviso In oSingleRiepilogo.oAvvisi
    '                            nAvanzamento += 1
    '                            sAvanzamento = "Inserimento posizione " & nAvanzamento & " su " & oSingleRiepilogo.oAvvisi.GetUpperBound(0) + 1
    '                            CacheManager.SetAvanzamentoElaborazione(sAvanzamento)
    '                            'registro l'avviso nella tabella
    '                            myAvviso.IdFlussoRuolo = nIdRuolo
    '                            myAvviso = PulisciDate(myAvviso)
    '                            If FncDB.SetAvvisoCompleto(myAvviso, _bIsFromVariabile, False) = 0 Then
    '                                HasError = True
    '                                Exit For
    '                            End If
    '                        Next
    '                        'controllo che tutti gli articoli aventi importo riduzione abbiano la corrispettiva riga in articoliriduzioni
    '                        _cmdMyCommand.Parameters.Clear()
    '                        _cmdMyCommand.Parameters.AddWithValue("@IdFlusso", nIdRuolo)
    '                        _cmdMyCommand.CommandType = CommandType.StoredProcedure
    '                        _cmdMyCommand.CommandText = "prc_ControlloRidVSArticoli"
    '                        Log.Debug(Utility.Costanti.LogQuery(_cmdMyCommand))
    '                        _cmdMyCommand.ExecuteNonQuery()
    '                        'eseguo la cartellazione
    '                        sAvanzamento = "Cartellazione"
    '                        CacheManager.SetAvanzamentoElaborazione(sAvanzamento)
    '                        oSingleRiepilogo = elab.CartellaRuolo(_DBType, _StringConnection, oSingleRiepilogo, _cmdMyCommand)
    '                        If oSingleRiepilogo Is Nothing Then
    '                            HasError = True
    '                            Exit For
    '                        End If
    '                    End If
    '                End If
    '                'popolo l'oggetto per il raffronto cat-importi
    '                SetArticoliVSCat(oSingleRiepilogo)
    '                CacheManager.SetRiepilogoCalcoloMassivo(oMyRuolo)
    '            Next
    '        Else
    '            HasError = True
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(_IdEnte + "." + _Operatore + " - OPENgovTIA.CalcolaRuolo.CalcoloCartelle.errore: ", ex)
    '        HasError = True
    '    End Try
    '    Return HasError
    'End Function
    '**** 201809 - Cartelle Insoluti ***
    ''' <summary>
    ''' Funzione per la creazione solleciti e atti di ingiunzione
    ''' </summary>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Analisi eventi</em>
    ''' </revision>
    ''' </revisionHistory>
    Private Function CalcoloCartelleInsoluti() As Boolean
        Dim HasError As Boolean = False
        Dim x, nIdRuolo As Integer
        Dim ListRuoli() As ObjRuolo
        Dim elab As New ClsElabRuolo
        Dim FncRuolo As New ClsGestRuolo
        Dim myRuolo As New ObjRuolo
        Dim FncDB As New DichManagerTARSU(_DBType, _StringConnection, _StringConnectionOPENgov, _IdEnte)
        Dim dtMyDati As New DataTable()
        Dim sAvanzamento As String
        Dim nAvanzamento As Integer = 0
        Dim FncRate As New GestRata
        Dim ListAtti As New ArrayList
        Dim myAtto As ObjAppoggioIngiunzione
        Dim IdProvvedimento As Integer = 0
        Dim RemAccertamenti As ComPlusInterface.IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), _URLServiziAccertamenti)
        Dim ImpSpese As Double
        Dim myHashTable As New Hashtable

        Try
            ListRuoli = elab.CalcolaRuoloCartelleInsoluti(_StringConnection, _IdEnte, _Anno, _GGScadenza, _Operatore, _ListAddizionali)
            'controllo se ho un riepilogo articoli
            If Not ListRuoli Is Nothing Then
                myHashTable.Add("DBType", _DBType)
                myHashTable.Add("TipoTassazione", ObjRuolo.TipoCalcolo.TARES)
                myHashTable.Add("CONNECTIONSTRINGTARSU", _StringConnection)
                myHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", _StringConnectionProvv)
                myHashTable.Add("CONNECTIONSTRINGANAGRAFICA", _StringConnectionAnag)
                For Each myAdd As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale In _ListAddizionali
                    If myAdd.CodiceCapitolo = ObjDetVoci.Capitolo.SpeseNotifica Then
                        ImpSpese = myAdd.Valore
                        Exit For
                    End If
                Next
                ImpSpese = _impSpeseIngiunzione
                'ciclo sugli anni calcolati
                For x = 0 To ListRuoli.GetUpperBound(0)
                    myRuolo = New ObjRuolo
                    'controllo i sotto soglia
                    Log.Debug("GeneraRuolo::devo controllare la soglia")
                    sAvanzamento = "Controllo soglia"
                    CacheManager.SetAvanzamentoElaborazione(sAvanzamento)
                    ListRuoli(x).sAnno = _Anno
                    ListRuoli(x).sTipoRuolo = _TipoRuolo
                    ListRuoli(x).sTipoCalcolo = _TipoCalcolo
                    ListRuoli(x).sDescrTipoRuolo = _DescrTipoCalcolo
                    ListRuoli(x).PercentTariffe = _PercentTariffe
                    ListRuoli(x).TipoMQ = _TipoMQ
                    ListRuoli(x).HasMaggiorazione = _HasMaggiorazione
                    ListRuoli(x).HasConferimenti = _HasConferimenti
                    ListRuoli(x).nTassazioneMinima = 0
                    ListRuoli(x).ImpMinimo = _impSogliaAvvisi
                    '*** 20181011 Dal/Al Conferimenti ***
                    ListRuoli(x).tDataInizioConf = DateTime.MaxValue
                    ListRuoli(x).tDataFineConf = DateTime.MaxValue

                    myRuolo = elab.CheckSogliaRuolo(ListRuoli(x))
                    If myRuolo Is Nothing Then
                        Log.Debug(_IdEnte + "." + _Operatore + " - OPENgovTIA.CalcolaRuolo.CalcoloCartelleInsoluti.errore in CheckSogliaRuolo")
                        HasError = True
                        Exit For
                    End If
                    'creo gli atti di ingiunzione
                    nAvanzamento = 0
                    For Each myAvviso As ObjAvviso In myRuolo.oAvvisi
                        nAvanzamento += 1
                        sAvanzamento = "Creazione Ingiunzione " & nAvanzamento & " su " & myRuolo.oAvvisi.GetUpperBound(0) + 1
                        CacheManager.SetAvanzamentoElaborazione(sAvanzamento)
                        myAtto = CreateIngiunzione(myAvviso, ImpSpese)
                        If myAtto Is Nothing Then
                            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgovTIA.CalcolaRuolo.CalcoloCartelleInsoluti.errore in CreateIngiunzione.Avanzamento->" + nAvanzamento.ToString)
                            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgovTIA.CalcolaRuolo.CalcoloCartelleInsoluti.errore in CreateIngiunzione.IdContribuente->" + myAvviso.IdContribuente.ToString)
                            HasError = True
                            Exit For
                        Else
                            ListAtti.Add(myAtto)
                        End If
                    Next
                    'aggiorno l'oggetto che dovrò restituire
                    ListRuoli(x) = myRuolo
                    'registro il ruolo nella tabella dei totalizzatori
                    myRuolo = elab.ValorizzaTotRuolo(_TipoCalcolo, _IdEnte, _TipoElaborazione, _DescrTipoCalcolo, ListRuoli(x))
                    If _isSimula = False Then
                        If Not IsNothing(myRuolo.oAvvisi) Then
                            nIdRuolo = FncRuolo.SetRuolo(Utility.Costanti.AZIONE_NEW, myRuolo, _StringConnection)
                        End If
                        If nIdRuolo > 0 Then
                            'ciclo sugli avvisi calcolati
                            nAvanzamento = 0
                            For Each myAvviso As ObjAvviso In myRuolo.oAvvisi
                                nAvanzamento += 1
                                sAvanzamento = "Inserimento posizione " & nAvanzamento & " su " & myRuolo.oAvvisi.GetUpperBound(0) + 1
                                CacheManager.SetAvanzamentoElaborazione(sAvanzamento)
                                'registro l'avviso nella tabella
                                myAvviso.IdFlussoRuolo = nIdRuolo
                                myAvviso = PulisciDate(myAvviso)
                                If FncDB.SetAvvisoCompleto(myAvviso, _bIsFromVariabile, False) = 0 Then
                                    Log.Debug(_IdEnte + "." + _Operatore + " - OPENgovTIA.CalcolaRuolo.CalcoloCartelleInsoluti.errore in inserimento avviso.Avanzamento->" + nAvanzamento.ToString)
                                    myRuolo.sNote = "Errore in inserimento Avviso"
                                    HasError = True
                                    Exit For
                                End If
                                If FncDB.SetCartella(myAvviso) = 0 Then
                                    Log.Debug(_IdEnte + "." + _Operatore + " - OPENgovTIA.CalcolaRuolo.CalcoloCartelleInsoluti.errore in inserimento dati avviso.Avanzamento->" + nAvanzamento.ToString)
                                    myRuolo.sNote = "Errore in inserimento Dati Avviso"
                                    HasError = True
                                End If
                            Next
                            'ciclo sugli atti calcolati
                            nAvanzamento = 0
                            For Each myAtto In CType(ListAtti.ToArray(GetType(ObjAppoggioIngiunzione)), ObjAppoggioIngiunzione())
                                nAvanzamento += 1
                                sAvanzamento = "Inserimento Ingiunzione " & nAvanzamento & " su " & ListAtti.Count
                                CacheManager.SetAvanzamentoElaborazione(sAvanzamento)
                                myAtto.Atto.IDRUOLO = nIdRuolo
                                IdProvvedimento = RemAccertamenti.SetAtto(ConstSession.DBType, myHashTable, myAtto.ListSituazioneBase, myAtto.dsSanzioni, myAtto.ListInteressi, myAtto.Atto, myAtto.ListDettaglio, myAtto.ListDichiarato, myAtto.ListAccertato, ImpSpese, Nothing, _Operatore)
                                If IdProvvedimento < 1 Then
                                    HasError = True
                                    Log.Debug(_IdEnte + "." + _Operatore + " - OPENgovTIA.CalcolaRuolo.CalcoloCartelleInsoluti.errore in SetAtto.Avanzamento->" + nAvanzamento.ToString)
                                    Log.Debug(_IdEnte + "." + _Operatore + " - OPENgovTIA.CalcolaRuolo.CalcoloCartelleInsoluti.errore in CreateIngiunzione.IdContribuente->" + myAtto.Atto.COD_CONTRIBUENTE.ToString)
                                End If
                            Next
                        End If
                    End If
                    CacheManager.SetRiepilogoCalcoloMassivo(ListRuoli)
                Next
            Else
                HasError = True
            End If
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgovTIA.CalcolaRuolo.CalcoloCartelleInsoluti.errore: ", ex)
            HasError = True
        End Try
        Return HasError
    End Function
    'Private Function CalcoloCartelleInsoluti() As Boolean
    '    Dim HasError As Boolean = False
    '    Dim x, nIdRuolo As Integer
    '    Dim ListRuoli() As ObjRuolo
    '    Dim elab As New ClsElabRuolo
    '    Dim FncRuolo As New ClsGestRuolo
    '    Dim myRuolo As New ObjRuolo
    '    Dim FncDB As New DichManagerTARSU(_DBType, _StringConnection)
    '    Dim dtMyDati As New DataTable()
    '    Dim sAvanzamento As String
    '    Dim nAvanzamento As Integer = 0
    '    Dim FncRate As New GestRata
    '    Dim ListAtti As New ArrayList
    '    Dim myAtto As ObjAppoggioIngiunzione
    '    Dim IdProvvedimento As Integer = 0
    '    Dim RemAccertamenti As ComPlusInterface.IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), _URLServiziAccertamenti)
    '    Dim ImpSpese As Double
    '    Dim myHashTable As New Hashtable

    '    Try
    '        ListRuoli = elab.CalcolaRuoloCartelleInsoluti(_StringConnection, _IdEnte, _Anno, _GGScadenza, _Operatore, _ListAddizionali)
    '        'controllo se ho un riepilogo articoli
    '        If Not ListRuoli Is Nothing Then
    '            myHashTable.Add("DBType", _DBType)
    '            myHashTable.Add("TipoTassazione", ObjRuolo.TipoCalcolo.TARES)
    '            myHashTable.Add("CONNECTIONSTRINGTARSU", _StringConnection)
    '            myHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", _StringConnectionProvv)
    '            myHashTable.Add("CONNECTIONSTRINGANAGRAFICA", _StringConnectionAnag)
    '            For Each myAdd As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale In _ListAddizionali
    '                If myAdd.CodiceCapitolo = ObjDetVoci.Capitolo.SpeseNotifica Then
    '                    ImpSpese = myAdd.Valore
    '                    Exit For
    '                End If
    '            Next
    '            'ciclo sugli anni calcolati
    '            For x = 0 To ListRuoli.GetUpperBound(0)
    '                myRuolo = New ObjRuolo
    '                'controllo i sotto soglia
    '                Log.Debug("GeneraRuolo::devo controllare la soglia")
    '                sAvanzamento = "Controllo soglia"
    '                CacheManager.SetAvanzamentoElaborazione(sAvanzamento)
    '                ListRuoli(x).sAnno = _Anno
    '                ListRuoli(x).sTipoRuolo = _TipoRuolo
    '                ListRuoli(x).sTipoCalcolo = _TipoCalcolo
    '                ListRuoli(x).sDescrTipoRuolo = _DescrTipoCalcolo
    '                ListRuoli(x).PercentTariffe = _PercentTariffe
    '                ListRuoli(x).TipoMQ = _TipoMQ
    '                ListRuoli(x).HasMaggiorazione = _HasMaggiorazione
    '                ListRuoli(x).HasConferimenti = _HasConferimenti
    '                ListRuoli(x).nTassazioneMinima = 0
    '                ListRuoli(x).ImpMinimo = _impSogliaAvvisi
    '                '*** 20181011 Dal/Al Conferimenti ***
    '                ListRuoli(x).tDataInizioConf = DateTime.MaxValue
    '                ListRuoli(x).tDataFineConf = DateTime.MaxValue

    '                myRuolo = elab.CheckSogliaRuolo(ListRuoli(x))
    '                If myRuolo Is Nothing Then
    '                    Log.Debug(_IdEnte  +"."+ _operatore + " - OPENgovTIA.CalcolaRuolo.CalcoloCartelleInsoluti.errore in CheckSogliaRuolo")
    '                    HasError = True
    '                    Exit For
    '                End If
    '                'creo gli atti di ingiunzione
    '                nAvanzamento = 0
    '                For Each myAvviso As ObjAvviso In myRuolo.oAvvisi
    '                    nAvanzamento += 1
    '                    sAvanzamento = "Creazione Ingiunzione " & nAvanzamento & " su " & myRuolo.oAvvisi.GetUpperBound(0) + 1
    '                    CacheManager.SetAvanzamentoElaborazione(sAvanzamento)
    '                    myAtto = CreateIngiunzione(myAvviso, ImpSpese)
    '                    If myAtto Is Nothing Then
    '                        Log.Debug(_IdEnte  +"."+ _operatore + " - OPENgovTIA.CalcolaRuolo.CalcoloCartelleInsoluti.errore in CreateIngiunzione.Avanzamento->" + nAvanzamento.ToString)
    '                        Log.Debug(_IdEnte  +"."+ _operatore + " - OPENgovTIA.CalcolaRuolo.CalcoloCartelleInsoluti.errore in CreateIngiunzione.IdContribuente->" + myAvviso.IdContribuente.ToString)
    '                        HasError = True
    '                        Exit For
    '                    Else
    '                        ListAtti.Add(myAtto)
    '                    End If
    '                Next
    '                'aggiorno l'oggetto che dovrò restituire
    '                ListRuoli(x) = myRuolo
    '                'registro il ruolo nella tabella dei totalizzatori
    '                myRuolo = elab.ValorizzaTotRuolo(_TipoCalcolo, _IdEnte, _TipoElaborazione, _DescrTipoCalcolo, ListRuoli(x))
    '                If _isSimula = False Then
    '                    If Not IsNothing(myRuolo.oAvvisi) Then
    '                        nIdRuolo = FncRuolo.SetRuolo(Utility.Costanti.AZIONE_NEW, myRuolo, _StringConnection, _cmdMyCommand)
    '                    End If
    '                    If nIdRuolo > 0 Then
    '                        'ciclo sugli avvisi calcolati
    '                        nAvanzamento = 0
    '                        For Each myAvviso As ObjAvviso In myRuolo.oAvvisi
    '                            nAvanzamento += 1
    '                            sAvanzamento = "Inserimento posizione " & nAvanzamento & " su " & myRuolo.oAvvisi.GetUpperBound(0) + 1
    '                            CacheManager.SetAvanzamentoElaborazione(sAvanzamento)
    '                            'registro l'avviso nella tabella
    '                            myAvviso.IdFlussoRuolo = nIdRuolo
    '                            myAvviso = PulisciDate(myAvviso)
    '                            If FncDB.SetAvvisoCompleto(myAvviso, _bIsFromVariabile, False) = 0 Then
    '                                Log.Debug(_IdEnte  +"."+ _operatore + " - OPENgovTIA.CalcolaRuolo.CalcoloCartelleInsoluti.errore in inserimento avviso.Avanzamento->" + nAvanzamento.ToString)
    '                                myRuolo.sNote = "Errore in inserimento Avviso"
    '                                HasError = True
    '                                Exit For
    '                            End If
    '                            If FncDB.SetCartella(myAvviso) = 0 Then
    '                                Log.Debug(_IdEnte  +"."+ _operatore + " - OPENgovTIA.CalcolaRuolo.CalcoloCartelleInsoluti.errore in inserimento dati avviso.Avanzamento->" + nAvanzamento.ToString)
    '                                myRuolo.sNote = "Errore in inserimento Dati Avviso"
    '                                HasError = True
    '                            End If
    '                        Next
    '                        'ciclo sugli atti calcolati
    '                        nAvanzamento = 0
    '                        For Each myAtto In CType(ListAtti.ToArray(GetType(ObjAppoggioIngiunzione)), ObjAppoggioIngiunzione())
    '                            nAvanzamento += 1
    '                            sAvanzamento = "Inserimento Ingiunzione " & nAvanzamento & " su " & ListAtti.Count
    '                            CacheManager.SetAvanzamentoElaborazione(sAvanzamento)
    '                            myAtto.Atto.IDRUOLO = nIdRuolo
    '                            IdProvvedimento = RemAccertamenti.SetAtto(myHashTable, myAtto.ListSituazioneBase, myAtto.dsSanzioni, myAtto.ListInteressi, myAtto.Atto, myAtto.ListDettaglio, myAtto.ListDichiarato, myAtto.ListAccertato, ImpSpese, Nothing)
    '                            If IdProvvedimento < 1 Then
    '                                HasError = True
    '                                Log.Debug(_IdEnte  +"."+ _operatore + " - OPENgovTIA.CalcolaRuolo.CalcoloCartelleInsoluti.errore in SetAtto.Avanzamento->" + nAvanzamento.ToString)
    '                                Log.Debug(_IdEnte  +"."+ _operatore + " - OPENgovTIA.CalcolaRuolo.CalcoloCartelleInsoluti.errore in CreateIngiunzione.IdContribuente->" + myAtto.Atto.COD_CONTRIBUENTE.ToString)
    '                            End If
    '                        Next
    '                    End If
    '                End If
    '                CacheManager.SetRiepilogoCalcoloMassivo(ListRuoli)
    '            Next
    '        Else
    '            HasError = True
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(_IdEnte  +"."+ _operatore + " - OPENgovTIA.CalcolaRuolo.CalcoloCartelleInsoluti.errore: ", ex)
    '        HasError = True
    '    End Try
    '    Return HasError
    'End Function

    Private Sub SetArticoliVSCat(ByVal myRuolo As ObjRuolo)
        Dim listArticoloVSCatPF As New ArrayList
        Dim listArticoloVSCatPV As New ArrayList
        Dim listArticoloVSCatPC As New ArrayList
        Dim listArticoloVSCatPM As New ArrayList

        Try
            For Each myAvviso As ObjAvviso In myRuolo.oAvvisi
                For Each myArticolo As ObjArticolo In myAvviso.oArticoli
                    If myArticolo.TipoPartita = ObjArticolo.PARTEFISSA Then
                        ValArtVSCat(myArticolo, listArticoloVSCatPF)
                    ElseIf myArticolo.TipoPartita = ObjArticolo.PARTEVARIABILE Then
                        ValArtVSCat(myArticolo, listArticoloVSCatPV)
                    ElseIf myArticolo.TipoPartita = ObjArticolo.PARTECONFERIMENTI Then
                        ValArtVSCat(myArticolo, listArticoloVSCatPC)
                    ElseIf myArticolo.TipoPartita = ObjArticolo.PARTEMAGGIORAZIONE Then
                        ValArtVSCat(myArticolo, listArticoloVSCatPM)
                    End If
                Next
            Next
            CacheManager.SetRiepilogoCalcoloMassivoArtVSCatPF(CType(listArticoloVSCatPF.ToArray(GetType(ObjArticolo)), ObjArticolo()))
            CacheManager.SetRiepilogoCalcoloMassivoArtVSCatPV(CType(listArticoloVSCatPV.ToArray(GetType(ObjArticolo)), ObjArticolo()))
            CacheManager.SetRiepilogoCalcoloMassivoArtVSCatPC(CType(listArticoloVSCatPC.ToArray(GetType(ObjArticolo)), ObjArticolo()))
            CacheManager.SetRiepilogoCalcoloMassivoArtVSCatPM(CType(listArticoloVSCatPM.ToArray(GetType(ObjArticolo)), ObjArticolo()))
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.CalcolaRuolo.SetArticoliVSCat.errore: ", ex)
            CacheManager.RemoveRiepilogoCalcoloMassivoArtVSCatPF()
            CacheManager.RemoveRiepilogoCalcoloMassivoArtVSCatPV()
            CacheManager.RemoveRiepilogoCalcoloMassivoArtVSCatPC()
            CacheManager.RemoveRiepilogoCalcoloMassivoArtVSCatPM()
        End Try
    End Sub
    Private Sub ValArtVSCat(ByVal myArticolo As ObjArticolo, ByRef listArtVSCat As ArrayList)
        Dim bTrovato As Boolean = False
        Try
            If Not listArtVSCat Is Nothing Then
                For Each myArticoloVsCat As ObjArticolo In listArtVSCat
                    If myArticoloVsCat.nIdTariffa = myArticolo.nIdTariffa Then
                        myArticoloVsCat.nIdTariffa = myArticolo.nIdTariffa
                        myArticoloVsCat.TipoPartita = myArticolo.TipoPartita
                        myArticoloVsCat.sCategoria = myArticolo.sCategoria
                        myArticoloVsCat.sDescrCategoria = myArticolo.sDescrCategoria
                        myArticoloVsCat.impRuolo += myArticolo.impRuolo
                        myArticoloVsCat.impRiduzione += myArticolo.impRiduzione
                        myArticoloVsCat.impDetassazione += myArticolo.impDetassazione
                        myArticoloVsCat.impNetto += myArticolo.impNetto
                        bTrovato = True
                        Exit For
                    End If
                Next
            End If
            If bTrovato = False Then
                Dim myArticoloVSCat As New ObjArticolo
                myArticoloVSCat.nIdTariffa = myArticolo.nIdTariffa
                myArticoloVSCat.TipoPartita = myArticolo.TipoPartita
                myArticoloVSCat.sCategoria = myArticolo.sCategoria
                myArticoloVSCat.sDescrCategoria = myArticolo.sDescrCategoria
                myArticoloVSCat.impRuolo += myArticolo.impRuolo
                myArticoloVSCat.impRiduzione += myArticolo.impRiduzione
                myArticoloVSCat.impDetassazione += myArticolo.impDetassazione
                myArticoloVSCat.impNetto += myArticolo.impNetto
                listArtVSCat.Add(myArticoloVSCat)
            End If
            Array.Sort(CType(listArtVSCat.ToArray(GetType(ObjArticolo)), ObjArticolo()), New Utility.Comparatore(New String() {"sCategoria", "sDescrCategoria"}, New Boolean() {Utility.TipoOrdinamento.Crescente, Utility.TipoOrdinamento.Crescente}))
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.CalcolaRuolo.ValArtVSCat.errore: ", ex)
            Throw ex
        End Try
    End Sub
    Private Function PulisciDate(myAvviso As ObjAvviso) As ObjAvviso
        Try
            If (myAvviso.tDataEmissione = DateTime.MinValue) Then
                myAvviso.tDataEmissione = DateTime.MaxValue
            End If
            If (myAvviso.tDataNascita = DateTime.MinValue) Then
                myAvviso.tDataNascita = DateTime.MaxValue
            End If
            If (myAvviso.tDataCessazione = DateTime.MinValue) Then
                myAvviso.tDataCessazione = DateTime.MaxValue
            End If
            If (myAvviso.tDataInserimento = DateTime.MinValue) Then
                myAvviso.tDataInserimento = DateTime.Now
            End If
            If (myAvviso.tDataVariazione = DateTime.MinValue) Then
                myAvviso.tDataVariazione = DateTime.MaxValue
            End If
            For Each myArticolo As ObjArticolo In myAvviso.oArticoli
                If (myArticolo.tDataInizio = DateTime.MinValue) Then
                    myArticolo.tDataInizio = DateTime.MaxValue
                End If
                If (myArticolo.tDataFine = DateTime.MinValue) Then
                    myArticolo.tDataFine = DateTime.MaxValue
                End If
                If (myArticolo.tDataCessazione = DateTime.MinValue) Then
                    myArticolo.tDataCessazione = DateTime.MaxValue
                End If
                If (myArticolo.tDataInserimento = DateTime.MinValue) Then
                    myArticolo.tDataInserimento = DateTime.Now
                End If
                If (myArticolo.tDataVariazione = DateTime.MinValue) Then
                    myArticolo.tDataVariazione = DateTime.MaxValue
                End If
            Next
            Return myAvviso
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.CalcolaRuolo.PulisciDate.errore: ", ex)
            Throw New Exception("PulisciDate errore->" & ex.Message)
        End Try
    End Function
    '*** 201809 Bollettazione Vigliano in OPENgov***
    Private Function ImportRuolo() As Boolean
        Dim HasError As Boolean = False
        Dim nIdRuolo As Integer
        Dim FncRuolo As New ClsGestRuolo
        Dim myRuolo As New ObjRuolo
        Dim ListRuoli As New ArrayList
        Dim FncDB As New DichManagerTARSU(_DBType, _StringConnection, _StringConnectionOPENgov, _IdEnte)
        Dim dtMyDati As New DataTable()
        Dim sAvanzamento As String
        Dim nAvanzamento As Integer = 0
        Dim ListAnagrafiche As New ArrayList

        Try
            For Each myFile As String In _ListFlussi
                myRuolo = New ObjRuolo
                nAvanzamento = 0
                'controllo formale del flusso
                If New ClsImportRuolo().Controllo290(myFile, _IdEnte, _Belfiore, myRuolo) Then
                    If myRuolo.sNote = "" Then
                        Dim ListComuni() As OggettiComuniStrade.OggettoEnte = GetComuni()
                        'carico gli avvisi
                        myRuolo.oAvvisi = New ClsImportRuolo(ListComuni).Import290(myFile, _IdEnte, ListAnagrafiche, _StringConnection, _StringConnectionAnag)
                        'controllo se ho un riepilogo articoli
                        If Not myRuolo.oAvvisi Is Nothing Then
                            'registro il ruolo nella tabella dei totalizzatori
                            myRuolo = New ClsElabRuolo().ValorizzaTotRuolo(_TipoCalcolo, _IdEnte, _TipoElaborazione, _DescrTipoCalcolo, myRuolo)
                            If Not IsNothing(myRuolo.oAvvisi) Then
                                nIdRuolo = FncRuolo.SetRuolo(Utility.Costanti.AZIONE_NEW, myRuolo, _StringConnection)
                            End If
                            If nIdRuolo > 0 Then
                                'ciclo sugli avvisi 
                                For Each myAvviso As ObjAvviso In myRuolo.oAvvisi
                                    nAvanzamento += 1
                                    sAvanzamento = "Inserimento posizione " & nAvanzamento & " su " & myRuolo.oAvvisi.GetUpperBound(0) + 1
                                    CacheManager.SetAvanzamentoElaborazione(sAvanzamento)
                                    'registro l'avviso nella tabella
                                    For Each myAnag As AnagInterface.DettaglioAnagrafica In ListAnagrafiche
                                        If myAnag.CodiceFiscale = myAvviso.sCodFiscale And myAnag.PartitaIva = myAvviso.sPIVA Then
                                            Dim AnagRet As New AnagInterface.DettaglioAnagraficaReturn
                                            AnagRet = New Anagrafica.DLL.GestioneAnagrafica().GestisciAnagrafica(myAnag, _DBType, _StringConnectionAnag, True, True)
                                            myAvviso.IdContribuente = AnagRet.COD_CONTRIBUENTE
                                            myAvviso.IdFlussoRuolo = nIdRuolo
                                            myAvviso.sOperatore = _Operatore
                                            myAvviso = PulisciDate(myAvviso)
                                            For Each myArt As ObjArticolo In myAvviso.oArticoli
                                                myArt.IdContribuente = myAvviso.IdContribuente
                                                myArt.sOperatore = _Operatore
                                            Next
                                            If FncDB.SetAvvisoCompleto(myAvviso, _bIsFromVariabile, False) = 0 Then
                                                myRuolo.sNote = "Errore in inserimento Avviso"
                                                ListRuoli.Add(myRuolo)
                                                HasError = True
                                            End If
                                            If FncDB.SetCartella(myAvviso) = 0 Then
                                                myRuolo.sNote = "Errore in inserimento Dati Avviso"
                                                ListRuoli.Add(myRuolo)
                                                HasError = True
                                            End If
                                            Exit For
                                        End If
                                    Next
                                Next
                                'controllo che tutti gli articoli aventi importo riduzione abbiano la corrispettiva riga in articoliriduzioni
                                _cmdMyCommand.Parameters.Clear()
                                _cmdMyCommand.Parameters.AddWithValue("@IdFlusso", nIdRuolo)
                                _cmdMyCommand.CommandType = CommandType.StoredProcedure
                                _cmdMyCommand.CommandText = "prc_ControlloRidVSArticoli"
                                Log.Debug(Utility.Costanti.LogQuery(_cmdMyCommand))
                                _cmdMyCommand.ExecuteNonQuery()
                                ListRuoli.Add(myRuolo)
                            End If
                            'popolo l'oggetto per il raffronto cat-importi
                            SetArticoliVSCat(myRuolo)
                        Else
                            myRuolo.sNote = "Errore in lettura flusso"
                            ListRuoli.Add(myRuolo)
                            HasError = True
                            Exit For
                        End If
                    Else
                        nIdRuolo = FncRuolo.SetRuolo(Utility.Costanti.AZIONE_NEW, myRuolo, _StringConnection)
                    End If
                Else
                    ListRuoli.Add(myRuolo)
                    HasError = True
                    Exit For
                End If
            Next
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgovTIA.CalcolaRuolo.ImportRuolo.errore: ", ex)
            HasError = True
        End Try
        CacheManager.SetRiepilogoCalcoloMassivo(CType(ListRuoli.ToArray(GetType(ObjRuolo)), ObjRuolo()))
        CacheManager.SetRiepilogoImportRuoli(CType(ListRuoli.ToArray(GetType(ObjRuolo)), ObjRuolo()))
        Return HasError
    End Function
    Public Function GetComuni() As OggettiComuniStrade.OggettoEnte()
        Dim myList() As OggettiComuniStrade.OggettoEnte = (New ArrayList).ToArray(GetType(OggettiComuniStrade.OggettoEnte))

        Try
            Dim TypeOfRI As Type = GetType(RemotingInterfaceOpenGovStradario.IRemotingInterfaceOpenGovStradario)
            Dim RemStradario As RemotingInterfaceOpenGovStradario.IRemotingInterfaceOpenGovStradario
            RemStradario = Activator.GetObject(TypeOfRI, ConfigurationManager.AppSettings("URLServizioStradario"))

            myList = RemStradario.GetArrayEnti(ConfigurationManager.AppSettings("DBType"), ConfigurationManager.AppSettings("ConnessioneDBComuniStrade"), New OggettiComuniStrade.OggettoEnte())
            If myList Is Nothing Then
                myList = (New ArrayList).ToArray(GetType(OggettiComuniStrade.OggettoEnte))
            End If
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgovTIA.CalcolaRuolo.GetComuni.errore: ", ex)
        End Try
        Return myList
    End Function
#Region "Atti Ingiunzione"
    Private Function CreateIngiunzione(myAvviso As ObjAvviso, ImpSpese As Double) As ObjAppoggioIngiunzione
        Dim ListAccertato() As ObjArticoloAccertamento
        Dim ListDichiarato() As ObjArticoloAccertamento
        Dim myItem As New ObjAppoggioIngiunzione

        Try
            ListDichiarato = Nothing : ListAccertato = Nothing
            If ConvArtTOAcc(myAvviso, ListDichiarato, ListAccertato) = False Then
                Return Nothing
            End If
            If Not ListAccertato Is Nothing Then
                'controllo coerenza date Accertato
                For Each myAcc As ObjArticoloAccertamento In ListAccertato
                    'se l'anno della data fine<>anno accertamento restituisco l'errore
                    If myAcc.tDataInizio.Year < _Anno Then
                        myAcc.tDataInizio = StringOperation.FormatDateTime("01/01/" & myAcc.sAnno)
                    End If
                    'Forzo la data fine a fine anno accertamento 
                    If myAcc.tDataFine = Date.MinValue Or myAcc.tDataFine.ToString() = "" Then
                        myAcc.tDataFine = StringOperation.FormatDateTime("31/12/" & myAcc.sAnno)
                    End If
                Next
                myItem = ConfrontoAccertatoDichiarato(myAvviso.IdContribuente, myAvviso.sCodiceCartella, myAvviso.tDataEmissione, myAvviso.impCarico, _impSogliaAvvisi, ImpSpese, ListDichiarato, ListAccertato)
            End If
            Return myItem
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgovTIA.clsCalcoloRuolo.CreateIngiunzione.errore: ", ex)
            Return Nothing
        End Try
    End Function
    Private Function ConvArtTOAcc(myAvviso As ObjAvviso, ByRef ListDichiarato() As ObjArticoloAccertamento, ByRef ListAccertato() As ObjArticoloAccertamento) As Boolean
        Dim ListUI As New ArrayList
        Dim nLegame As Integer = 0
        Try
            For Each myArt As ObjArticolo In myAvviso.oArticoli
                Dim myDic As New ObjArticoloAccertamento
                nLegame += 1
                myDic.bForzaPV = myArt.bForzaPV
                myDic.bIsImportoForzato = myArt.bIsImportoForzato
                myDic.bIsTarsuGiornaliera = myArt.bIsTarsuGiornaliera
                myDic.Calcola_Interessi = True
                myDic.Id = myArt.Id
                myDic.IdArticolo = myArt.IdArticolo
                myDic.IdAvviso = myArt.IdAvviso
                myDic.IdContribuente = myArt.IdContribuente
                myDic.IdDettaglioTestata = myArt.IdDettaglioTestata
                myDic.IdEnte = myArt.IdEnte
                myDic.IdLegame = nLegame
                myDic.IdOggetto = myArt.IdOggetto
                myDic.impDetassazione = myArt.impDetassazione
                myDic.impNetto = myArt.impNetto
                myDic.impRiduzione = myArt.impRiduzione
                myDic.impRuolo = myArt.impRuolo
                myDic.impTariffa = myArt.impTariffa
                myDic.ListPFvsPV = myArt.ListPFvsPV
                myDic.nBimestri = myArt.nBimestri
                myDic.nCodVia = myArt.nCodVia
                myDic.nComponenti = myArt.nComponenti
                myDic.nComponentiPV = myArt.nComponentiPV
                myDic.nIdAssenzaDatiCatastali = myArt.nIdAssenzaDatiCatastali
                myDic.nIdDestUso = myArt.nIdDestUso
                myDic.nIdFlussoRuolo = myArt.nIdFlussoRuolo
                myDic.nIdNaturaOccupaz = myArt.nIdNaturaOccupaz
                myDic.nIdTariffa = myArt.nIdTariffa
                myDic.nIdTitoloOccupaz = myArt.nIdTitoloOccupaz
                myDic.nMQ = myArt.nMQ
                myDic.oDetassazioni = myArt.oDetassazioni
                myDic.oRiduzioni = myArt.oRiduzioni
                myDic.Progressivo = myDic.IdLegame
                myDic.sAnno = myArt.sAnno
                myDic.sCategoria = myArt.sCategoria
                myDic.sCivico = myArt.sCivico
                myDic.sDescrCategoria = myArt.sDescrCategoria
                myDic.sEsponente = myArt.sEsponente
                myDic.sEstensioneParticella = myArt.sEstensioneParticella
                myDic.sFoglio = myArt.sFoglio
                myDic.sIdTipoParticella = myArt.sIdTipoParticella
                myDic.sIdTipoUnita = myArt.sIdTipoUnita
                myDic.sInterno = myArt.sInterno
                myDic.sNote = myArt.sNote
                myDic.sNumero = myArt.sNumero
                myDic.sOperatore = myArt.sOperatore
                myDic.sScala = myArt.sScala
                myDic.sSezione = myArt.sSezione
                myDic.sSubalterno = myArt.sSubalterno
                myDic.sTipoRuolo = myArt.sTipoRuolo
                myDic.sVia = myArt.sVia
                myDic.tDataCessazione = myArt.tDataCessazione
                myDic.tDataFine = myArt.tDataFine
                myDic.tDataInizio = myArt.tDataInizio
                myDic.tDataInserimento = myArt.tDataInserimento
                myDic.tDataVariazione = myArt.tDataVariazione
                myDic.TipoPartita = myArt.TipoPartita
                ListUI.Add(myDic)
            Next
            ListDichiarato = CType(ListUI.ToArray(GetType(ObjArticoloAccertamento)), ObjArticoloAccertamento())
            ListAccertato = CType(ListUI.ToArray(GetType(ObjArticoloAccertamento)), ObjArticoloAccertamento())
            Return True
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgovTIA.clsCalcoloRuolo.ConvArtTOAcc.errore: ", ex)
            Return False
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="IdContribuente"></param>
    ''' <param name="CodiceCartella"></param>
    ''' <param name="DataEmissione"></param>
    ''' <param name="ImpDichTotale"></param>
    ''' <param name="ImpSoglia"></param>
    ''' <param name="ImpSpese"></param>
    ''' <param name="oDichiarato"></param>
    ''' <param name="oAccertato"></param>
    ''' <returns></returns>
    ''' <revisionHistory><revision date="10/12/2019">in caso di calcolo per Cartelle Insoluti devo prendere il pagato per singolo avviso</revision></revisionHistory>
    Private Function ConfrontoAccertatoDichiarato(IdContribuente As Integer, CodiceCartella As String, DataEmissione As DateTime, ImpDichTotale As Double, ImpSoglia As Double, ImpSpese As Double, ByVal oDichiarato() As ObjArticoloAccertamento, ByVal oAccertato() As ObjArticoloAccertamento) As ObjAppoggioIngiunzione
        Dim myHashTable As New Hashtable
        Dim ListBaseCalcoli() As ComPlusInterface.ObjBaseIntSanz = Nothing
        Dim dsSanzioniFase2 As New DataSet
        Dim ListInteressi() As ComPlusInterface.ObjInteressiSanzioni = Nothing
        Dim dsRiepilogoFase2 As New ComPlusInterface.ObjBaseIntSanz
        Dim dsVersamenti As New DataSet
        Dim oDettaglioAtto As New ArrayList
        Dim myAtto As New ComPlusInterface.OggettoAttoTARSU
        Dim myItem As New ObjAppoggioIngiunzione

        Try
            myHashTable.Add("ANNODA", _Anno)
            myHashTable.Add("ANNOA", _Anno)
            myHashTable.Add("CodContribuente", IdContribuente)
            myHashTable.Add("IdProvRett", -1)
            myHashTable.Add("CodENTE", _IdEnte)
            myHashTable.Add("COD_TRIBUTO", Utility.Costanti.TRIBUTO_TARSU)
            myHashTable.Add("TRIBUTOCALCOLO", Utility.Costanti.TRIBUTO_TARSU)
            myHashTable.Add("CONNECTIONSTRINGOPENGOV", _StringConnectionOPENgov)
            myHashTable.Add("CONNECTIONSTRINGOPENGOVICI", _StringConnectionICI)
            myHashTable.Add("CONNECTIONSTRINGANAGRAFICA", _StringConnectionAnag)
            myHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", _StringConnectionProvv)
            myHashTable.Add("PARAMETROORDINAMENTOGRIGLIA", "")
            myHashTable.Add("DA", "")
            myHashTable.Add("A", "")
            myHashTable.Add("DATA_ELABORAZIONE_PER_RETTIFICA", Date.Now)

            Dim dsDettaglioAnagrafica As DataSet = Nothing
            Dim oDettaglioAnagrafica As New AnagInterface.DettaglioAnagrafica
            oDettaglioAnagrafica = New Anagrafica.DLL.GestioneAnagrafica().GetAnagrafica(IdContribuente, -1, "", _DBType, _StringConnectionAnag, False)
            dsDettaglioAnagrafica = addRowsObjAnagrafica(oDettaglioAnagrafica)

            Dim RemLiquidazione As ComPlusInterface.IElaborazioneLiquidazioni = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneLiquidazioni), _URLServiziLiquidazione)
            If RemLiquidazione.ProcessFase2(_StringConnectionProvv, _StringConnectionICI, _IdEnte, IdContribuente, myHashTable, dsDettaglioAnagrafica, 0, 0, ImpDichTotale, CodiceCartella, ListBaseCalcoli, dsSanzioniFase2, ListInteressi, dsRiepilogoFase2, dsVersamenti) = False Then
                Log.Debug(_IdEnte + "." + _Operatore + " - OPENgovTIA.clsCalcoloRuolo.ConfrontoAccertatoDichiarato.errore in ProcessFase2.IdContribuente->" + IdContribuente.ToString)
                Return Nothing
            End If

            For Each myAcc As ObjArticoloAccertamento In oAccertato
                'sommo gli importi a parità di livello di legame
                Dim oListDettaglioAtto As New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDettaglioAtto
                'aggiungo un record
                oListDettaglioAtto.IdLegame = myAcc.IdLegame
                oListDettaglioAtto.Progressivo = myAcc.Progressivo
                oListDettaglioAtto.Sanzioni = myAcc.Sanzioni
                oListDettaglioAtto.Interessi = myAcc.Interessi
                oListDettaglioAtto.Calcola_Interessi = myAcc.Calcola_Interessi
                'sommo l'importo
                oListDettaglioAtto.ImpAccertato += myAcc.impNetto
                oDettaglioAtto.Add(oListDettaglioAtto)
            Next
            If Not dsRiepilogoFase2 Is Nothing Then
                Dim iTIPOPROVV_PREACC As String = dsRiepilogoFase2.COD_TIPO_PROVVEDIMENTO

                myAtto.IMPORTO_DIFFERENZA_IMPOSTA_F2 = dsRiepilogoFase2.DifferenzaImposta
                myAtto.IMPORTO_SANZIONI_F2 = dsRiepilogoFase2.Sanzioni
                myAtto.IMPORTO_SANZIONI_RIDOTTO = dsRiepilogoFase2.SanzioniRidotto
                myAtto.IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI = myAtto.IMPORTO_SANZIONI_F2 - myAtto.IMPORTO_SANZIONI_RIDOTTO
                myAtto.IMPORTO_INTERESSI_F2 = dsRiepilogoFase2.Interessi
            End If
            If Not dsVersamenti Is Nothing Then
                If dsVersamenti.Tables.Count > 0 Then
                    For Each myRow As DataRow In dsVersamenti.Tables(0).Rows
                        myAtto.IMPORTO_PAGATO += StringOperation.FormatDouble(myRow("IMPORTOPAGATO").ToString)
                    Next
                End If
            End If

            myAtto.TipoProvvedimento = 2 'FISSO=PROVVEDIMENTO_INGIUNZIONE
            myAtto.Provenienza = 2
            myAtto.COD_CONTRIBUENTE = IdContribuente
            myAtto.COD_ENTE = _IdEnte
            myAtto.COD_TRIBUTO = Utility.Costanti.TRIBUTO_TARSU
            myAtto.ANNO = _Anno
            myAtto.NUMERO_ATTO = CodiceCartella
            myAtto.NOTE_CASSAZIONE = CodiceCartella
            myAtto.DATA_ELABORAZIONE = DataEmissione
            myAtto.CAP_CO = ""
            myAtto.CAP_RES = ""
            myAtto.CITTA_CO = ""
            myAtto.CITTA_RES = ""
            myAtto.CIVICO_CO = ""
            myAtto.CIVICO_RES = ""
            myAtto.CO = ""
            myAtto.CODICE_FISCALE = ""
            myAtto.COGNOME = ""
            myAtto.ESPONENTE_CIVICO_CO = ""
            myAtto.ESPONENTE_CIVICO_RES = ""
            myAtto.FRAZIONE_CO = ""
            myAtto.FRAZIONE_RES = ""
            myAtto.NOME = ""
            myAtto.PARTITA_IVA = ""
            myAtto.POSIZIONE_CIVICO_CO = ""
            myAtto.POSIZIONE_CIVICO_RES = ""
            myAtto.PROVINCIA_CO = ""
            myAtto.PROVINCIA_RES = ""
            myAtto.VIA_CO = ""
            myAtto.VIA_RES = ""
            myAtto.IMPORTO_DIFFERENZA_IMPOSTA = myAtto.IMPORTO_DIFFERENZA_IMPOSTA_ACC + myAtto.IMPORTO_DIFFERENZA_IMPOSTA_F2
            myAtto.IMPORTO_SANZIONI = myAtto.IMPORTO_SANZIONI_ACC + myAtto.IMPORTO_SANZIONI_F2
            myAtto.IMPORTO_INTERESSI = myAtto.IMPORTO_INTERESSI_ACC + myAtto.IMPORTO_INTERESSI_F2
            myAtto.IMPORTO_SPESE = ImpSpese
            myAtto.IMPORTO_TOTALE = (myAtto.IMPORTO_DIFFERENZA_IMPOSTA + myAtto.IMPORTO_SANZIONI + myAtto.IMPORTO_INTERESSI + myAtto.IMPORTO_ALTRO)
            myAtto.IMPORTO_ARROTONDAMENTO = StringOperation.FormatDouble(ImportoArrotondato(myAtto.IMPORTO_TOTALE)) - myAtto.IMPORTO_TOTALE
            myAtto.IMPORTO_TOTALE = myAtto.IMPORTO_TOTALE + myAtto.IMPORTO_ARROTONDAMENTO + myAtto.IMPORTO_SPESE
            myAtto.IMPORTO_TOTALE_RIDOTTO = myAtto.IMPORTO_DIFFERENZA_IMPOSTA + myAtto.IMPORTO_SANZIONI_RIDOTTO + myAtto.IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI + myAtto.IMPORTO_INTERESSI + myAtto.IMPORTO_ALTRO
            myAtto.IMPORTO_ARROTONDAMENTO_RIDOTTO = StringOperation.FormatDouble(ImportoArrotondato(myAtto.IMPORTO_TOTALE_RIDOTTO)) - myAtto.IMPORTO_TOTALE_RIDOTTO
            myAtto.IMPORTO_TOTALE_RIDOTTO = myAtto.IMPORTO_TOTALE_RIDOTTO + myAtto.IMPORTO_ARROTONDAMENTO_RIDOTTO + myAtto.IMPORTO_SPESE
            For Each myAcc As ObjArticoloAccertamento In oAccertato
                If Year(myAcc.tDataInizio) < myAtto.ANNO Then
                    myAcc.tDataInizio = "01/01/" & myAtto.ANNO
                End If
                myAtto.IMPORTO_ACCERTATO_ACC += myAcc.impNetto
            Next
            For Each myDic As ObjArticoloAccertamento In oDichiarato
                If Year(myDic.tDataInizio) < myAtto.ANNO Then
                    myDic.tDataInizio = "01/01/" & myAtto.ANNO
                End If
                myAtto.IMPORTO_DICHIARATO_F2 += myDic.impNetto
            Next
            If myAtto.IMPORTO_TOTALE < ImpSoglia Then
                myAtto = New ComPlusInterface.OggettoAttoTARSU
            End If
            myItem.Atto = myAtto
            myItem.dsSanzioni = dsSanzioniFase2
            myItem.ListAccertato = oAccertato
            myItem.ListDettaglio = CType(oDettaglioAtto.ToArray(GetType(RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDettaglioAtto)), RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDettaglioAtto())
            myItem.ListDichiarato = oDichiarato
            myItem.ListInteressi = ListInteressi
            myItem.ListSituazioneBase = ListBaseCalcoli
            Return myItem
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgovTIA.clsCalcoloRuolo.ConfrontoAccertatoDichiarato.errore:IdContribuente->" + IdContribuente.ToString, ex)
            Return Nothing
        End Try
    End Function
    'Private Function ConfrontoAccertatoDichiarato(IdContribuente As Integer, CodiceCartella As String, DataEmissione As DateTime, ImpDichTotale As Double, ImpSoglia As Double, ImpSpese As Double, ByVal oDichiarato() As ObjArticoloAccertamento, ByVal oAccertato() As ObjArticoloAccertamento) As ObjAppoggioIngiunzione
    '    Dim myHashTable As New Hashtable
    '    Dim ListBaseCalcoli() As ComPlusInterface.ObjBaseIntSanz = Nothing
    '    Dim dsSanzioniFase2 As New DataSet
    '    Dim ListInteressi() As ComPlusInterface.ObjInteressiSanzioni = Nothing
    '    Dim dsRiepilogoFase2 As New DataSet
    '    Dim dsVersamenti As New DataSet
    '    Dim oDettaglioAtto As New ArrayList
    '    Dim myAtto As New ComPlusInterface.OggettoAttoTARSU
    '    Dim myItem As New ObjAppoggioIngiunzione

    '    Try
    '        myHashTable.Add("ANNODA", _Anno)
    '        myHashTable.Add("ANNOA", _Anno)
    '        myHashTable.Add("CodContribuente", IdContribuente)
    '        myHashTable.Add("IdProvRett", -1)
    '        myHashTable.Add("CodENTE", _IdEnte)
    '        myHashTable.Add("COD_TRIBUTO", Utility.Costanti.TRIBUTO_TARSU)
    '        myHashTable.Add("TRIBUTOCALCOLO", Utility.Costanti.TRIBUTO_TARSU)
    '        myHashTable.Add("CONNECTIONSTRINGOPENGOV", _StringConnectionOPENgov)
    '        myHashTable.Add("CONNECTIONSTRINGOPENGOVICI", _StringConnectionICI)
    '        myHashTable.Add("CONNECTIONSTRINGANAGRAFICA", _StringConnectionAnag)
    '        myHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", _StringConnectionProvv)
    '        myHashTable.Add("PARAMETROORDINAMENTOGRIGLIA", "")
    '        myHashTable.Add("DA", "")
    '        myHashTable.Add("A", "")
    '        myHashTable.Add("DATA_ELABORAZIONE_PER_RETTIFICA", Date.Now)

    '        Dim dsDettaglioAnagrafica As DataSet = Nothing
    '        Dim oDettaglioAnagrafica As New AnagInterface.DettaglioAnagrafica
    '        oDettaglioAnagrafica = New Anagrafica.DLL.GestioneAnagrafica().GetAnagrafica(IdContribuente, -1, "", _DBType, _StringConnectionAnag, False)
    '        dsDettaglioAnagrafica = addRowsObjAnagrafica(oDettaglioAnagrafica)

    '        Dim RemLiquidazione As ComPlusInterface.IElaborazioneLiquidazioni = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneLiquidazioni), _URLServiziLiquidazione)
    '        If RemLiquidazione.ProcessFase2(myHashTable, dsDettaglioAnagrafica, 0, 0, ImpDichTotale, CodiceCartella, ListBaseCalcoli, dsSanzioniFase2, ListInteressi, dsRiepilogoFase2, dsVersamenti) = False Then
    '            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgovTIA.clsCalcoloRuolo.ConfrontoAccertatoDichiarato.errore in ProcessFase2.IdContribuente->" + IdContribuente.ToString)
    '            Return Nothing
    '        End If

    '        For Each myAcc As ObjArticoloAccertamento In oAccertato
    '            'sommo gli importi a parità di livello di legame
    '            Dim oListDettaglioAtto As New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDettaglioAtto
    '            'aggiungo un record
    '            oListDettaglioAtto.IdLegame = myAcc.IdLegame
    '            oListDettaglioAtto.Progressivo = myAcc.Progressivo
    '            oListDettaglioAtto.Sanzioni = myAcc.Sanzioni
    '            oListDettaglioAtto.Interessi = myAcc.Interessi
    '            oListDettaglioAtto.Calcola_Interessi = myAcc.Calcola_Interessi
    '            'sommo l'importo
    '            oListDettaglioAtto.ImpAccertato += myAcc.impNetto
    '            oDettaglioAtto.Add(oListDettaglioAtto)
    '        Next
    '        If Not dsRiepilogoFase2 Is Nothing Then
    '            If dsRiepilogoFase2.Tables.Count > 0 Then
    '                If dsRiepilogoFase2.Tables(0).Rows.Count > 0 Then
    '                    Dim iTIPOPROVV_PREACC As String = dsRiepilogoFase2.Tables(0).Rows(0)("TIPO_PROVVEDIMENTO")

    '                    myAtto.IMPORTO_DIFFERENZA_IMPOSTA_F2 = stringoperation.formatdouble(dsRiepilogoFase2.Tables(0).Rows(0)("DIFFERENZA_IMPOSTA_TOTALE").ToString)
    '                    myAtto.IMPORTO_SANZIONI_F2 = stringoperation.formatdouble(dsRiepilogoFase2.Tables(0).Rows(0)("IMPORTO_SANZIONI").ToString)
    '                    myAtto.IMPORTO_SANZIONI_RIDOTTO = stringoperation.formatdouble(dsRiepilogoFase2.Tables(0).Rows(0)("IMPORTO_SANZIONI_RIDOTTO").ToString)
    '                    myAtto.IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI = myAtto.IMPORTO_SANZIONI_F2 - myAtto.IMPORTO_SANZIONI_RIDOTTO
    '                    myAtto.IMPORTO_INTERESSI_F2 = stringoperation.formatdouble(dsRiepilogoFase2.Tables(0).Rows(0)("IMPORTO_INTERESSI").ToString)
    '                End If
    '            End If
    '        End If
    '        If Not dsVersamenti Is Nothing Then
    '            If dsVersamenti.Tables.Count > 0 Then
    '                For Each myRow As DataRow In dsVersamenti.Tables(0).Rows
    '                    myAtto.IMPORTO_PAGATO += stringoperation.formatdouble(myRow("IMPORTOPAGATO").ToString)
    '                Next
    '            End If
    '        End If

    '        myAtto.TipoProvvedimento = 2 'FISSO=PROVVEDIMENTO_INGIUNZIONE
    '        myAtto.Provenienza = 2
    '        myAtto.COD_CONTRIBUENTE = IdContribuente
    '        myAtto.COD_ENTE = _IdEnte
    '        myAtto.COD_TRIBUTO = Utility.Costanti.TRIBUTO_TARSU
    '        myAtto.ANNO = _Anno
    '        myAtto.NUMERO_ATTO = CodiceCartella
    '        myAtto.NOTE_CASSAZIONE = CodiceCartella
    '        myAtto.DATA_ELABORAZIONE = DataEmissione
    '        myAtto.CAP_CO = ""
    '        myAtto.CAP_RES = ""
    '        myAtto.CITTA_CO = ""
    '        myAtto.CITTA_RES = ""
    '        myAtto.CIVICO_CO = ""
    '        myAtto.CIVICO_RES = ""
    '        myAtto.CO = ""
    '        myAtto.CODICE_FISCALE = ""
    '        myAtto.COGNOME = ""
    '        myAtto.ESPONENTE_CIVICO_CO = ""
    '        myAtto.ESPONENTE_CIVICO_RES = ""
    '        myAtto.FRAZIONE_CO = ""
    '        myAtto.FRAZIONE_RES = ""
    '        myAtto.NOME = ""
    '        myAtto.PARTITA_IVA = ""
    '        myAtto.POSIZIONE_CIVICO_CO = ""
    '        myAtto.POSIZIONE_CIVICO_RES = ""
    '        myAtto.PROVINCIA_CO = ""
    '        myAtto.PROVINCIA_RES = ""
    '        myAtto.VIA_CO = ""
    '        myAtto.VIA_RES = ""
    '        myAtto.IMPORTO_DIFFERENZA_IMPOSTA = myAtto.IMPORTO_DIFFERENZA_IMPOSTA_ACC + myAtto.IMPORTO_DIFFERENZA_IMPOSTA_F2
    '        myAtto.IMPORTO_SANZIONI = myAtto.IMPORTO_SANZIONI_ACC + myAtto.IMPORTO_SANZIONI_F2
    '        myAtto.IMPORTO_INTERESSI = myAtto.IMPORTO_INTERESSI_ACC + myAtto.IMPORTO_INTERESSI_F2
    '        myAtto.IMPORTO_SPESE = ImpSpese
    '        myAtto.IMPORTO_TOTALE = (myAtto.IMPORTO_DIFFERENZA_IMPOSTA + myAtto.IMPORTO_SANZIONI + myAtto.IMPORTO_INTERESSI + myAtto.IMPORTO_ALTRO)
    '        myAtto.IMPORTO_ARROTONDAMENTO = stringoperation.formatdouble(ImportoArrotondato(myAtto.IMPORTO_TOTALE)) - myAtto.IMPORTO_TOTALE
    '        myAtto.IMPORTO_TOTALE = myAtto.IMPORTO_TOTALE + myAtto.IMPORTO_ARROTONDAMENTO + myAtto.IMPORTO_SPESE
    '        myAtto.IMPORTO_TOTALE_RIDOTTO = myAtto.IMPORTO_DIFFERENZA_IMPOSTA + myAtto.IMPORTO_SANZIONI_RIDOTTO + myAtto.IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI + myAtto.IMPORTO_INTERESSI + myAtto.IMPORTO_ALTRO
    '        myAtto.IMPORTO_ARROTONDAMENTO_RIDOTTO = stringoperation.formatdouble(ImportoArrotondato(myAtto.IMPORTO_TOTALE_RIDOTTO)) - myAtto.IMPORTO_TOTALE_RIDOTTO
    '        myAtto.IMPORTO_TOTALE_RIDOTTO = myAtto.IMPORTO_TOTALE_RIDOTTO + myAtto.IMPORTO_ARROTONDAMENTO_RIDOTTO + myAtto.IMPORTO_SPESE
    '        For Each myAcc As ObjArticoloAccertamento In oAccertato
    '            If Year(myAcc.tDataInizio) < myAtto.ANNO Then
    '                myAcc.tDataInizio = "01/01/" & myAtto.ANNO
    '            End If
    '            myAtto.IMPORTO_ACCERTATO_ACC += myAcc.impNetto
    '        Next
    '        For Each myDic As ObjArticoloAccertamento In oDichiarato
    '            If Year(myDic.tDataInizio) < myAtto.ANNO Then
    '                myDic.tDataInizio = "01/01/" & myAtto.ANNO
    '            End If
    '            myAtto.IMPORTO_DICHIARATO_F2 += myDic.impNetto
    '        Next
    '        If myAtto.IMPORTO_TOTALE < ImpSoglia Then
    '            myAtto = New ComPlusInterface.OggettoAttoTARSU
    '        End If
    '        myItem.Atto = myAtto
    '        myItem.dsSanzioni = dsSanzioniFase2
    '        myItem.ListAccertato = oAccertato
    '        myItem.ListDettaglio = CType(oDettaglioAtto.ToArray(GetType(RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDettaglioAtto)), RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDettaglioAtto())
    '        myItem.ListDichiarato = oDichiarato
    '        myItem.ListInteressi = ListInteressi
    '        myItem.ListSituazioneBase = ListBaseCalcoli
    '        Return myItem
    '    Catch ex As Exception
    '        Log.Debug(_IdEnte + "." + _Operatore + " - OPENgovTIA.clsCalcoloRuolo.ConfrontoAccertatoDichiarato.errore:IdContribuente->" + IdContribuente.ToString, ex)
    '        Return Nothing
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Anagrafica"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="27/10/2014">
    ''' <strong>visualizzazione tutti indirizzi spedizione</strong>
    ''' Nuova gestione indirizzi spedizione
    ''' </revision>
    ''' </revisionHistory>
    Private Function addRowsObjAnagrafica(ByVal Anagrafica As AnagInterface.DettaglioAnagrafica) As DataSet
        Dim row As DataRow
        Dim newTable As DataTable
        Dim objAnagrafica As DataSet = Nothing
        Try
            objAnagrafica = CreateDsPerAnagrafica()
            newTable = objAnagrafica.Tables(0).Copy

            row = newTable.NewRow()

            row.Item("COD_CONTRIBUENTE") = Anagrafica.COD_CONTRIBUENTE
            row.Item("ID_DATA_ANAGRAFICA") = Anagrafica.ID_DATA_ANAGRAFICA
            row.Item("Cognome") = Anagrafica.Cognome
            row.Item("RappresentanteLegale") = Anagrafica.RappresentanteLegale
            row.Item("Nome") = Anagrafica.Nome
            row.Item("CodiceFiscale") = Anagrafica.CodiceFiscale
            row.Item("PartitaIva") = Anagrafica.PartitaIva
            row.Item("CodiceComuneNascita") = Anagrafica.CodiceComuneNascita
            row.Item("ComuneNascita") = Anagrafica.ComuneNascita
            row.Item("ProvinciaNascita") = Anagrafica.ProvinciaNascita
            row.Item("DataNascita") = Anagrafica.DataNascita
            row.Item("NazionalitaNascita") = Anagrafica.NazionalitaNascita
            row.Item("Sesso") = Anagrafica.Sesso
            row.Item("CodiceComuneResidenza") = Anagrafica.CodiceComuneResidenza
            row.Item("ComuneResidenza") = Anagrafica.ComuneResidenza
            row.Item("ProvinciaResidenza") = Anagrafica.ProvinciaResidenza
            row.Item("CapResidenza") = Anagrafica.CapResidenza
            row.Item("CodViaResidenza") = Anagrafica.CodViaResidenza
            row.Item("ViaResidenza") = Anagrafica.ViaResidenza
            row.Item("PosizioneCivicoResidenza") = Anagrafica.PosizioneCivicoResidenza
            row.Item("CivicoResidenza") = Anagrafica.CivicoResidenza
            row.Item("EsponenteCivicoResidenza") = Anagrafica.EsponenteCivicoResidenza
            row.Item("ScalaCivicoResidenza") = Anagrafica.ScalaCivicoResidenza
            row.Item("InternoCivicoResidenza") = Anagrafica.InternoCivicoResidenza
            row.Item("FrazioneResidenza") = Anagrafica.FrazioneResidenza
            row.Item("NazionalitaResidenza") = Anagrafica.NazionalitaResidenza
            row.Item("NucleoFamiliare") = Anagrafica.NucleoFamiliare
            row.Item("DATA_MORTE") = Anagrafica.DataMorte
            row.Item("Professione") = Anagrafica.Professione
            row.Item("Note") = Anagrafica.Note
            row.Item("DaRicontrollare") = Anagrafica.DaRicontrollare
            row.Item("DataInizioValidita") = Anagrafica.DataInizioValidita
            row.Item("DataFineValidita") = Anagrafica.DataFineValidita
            row.Item("DataUltimaModifica") = Anagrafica.DataUltimaModifica
            row.Item("Operatore") = Anagrafica.Operatore
            row.Item("CodContribuenteRappLegale") = Anagrafica.CodContribuenteRappLegale
            row.Item("CodEnte") = Anagrafica.CodEnte
            row.Item("CodIndividuale") = Anagrafica.CodIndividuale
            row.Item("CodFamiglia") = Anagrafica.CodFamiglia
            row.Item("NCTributari") = Anagrafica.NCTributari
            row.Item("DataUltimoAggTributi") = Anagrafica.DataUltimoAggTributi
            row.Item("NCAnagraficaRes") = Anagrafica.NCAnagraficaRes
            row.Item("DataUltimoAggAnagrafe") = Anagrafica.DataUltimoAggAnagrafe
            row.Item("TipoRiferimento") = Anagrafica.TipoRiferimento
            row.Item("DatiRiferimento") = Anagrafica.DatiRiferimento
            '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
            For Each MySped As AnagInterface.ObjIndirizziSpedizione In Anagrafica.ListSpedizioni
                If MySped.CodTributo = Utility.Costanti.TRIBUTO_ICI Then
                    row.Item("ID_DATA_SPEDIZIONE") = MySped.ID_DATA_SPEDIZIONE
                    row.Item("CodTributo") = MySped.CodTributo
                    row.Item("CognomeInvio") = MySped.CognomeInvio
                    row.Item("NomeInvio") = MySped.NomeInvio
                    row.Item("CodComuneRCP") = MySped.CodComuneRCP
                    row.Item("ComuneRCP") = MySped.ComuneRCP
                    row.Item("LocRCP") = MySped.LocRCP
                    row.Item("ProvinciaRCP") = MySped.ProvinciaRCP
                    row.Item("CapRCP") = MySped.CapRCP
                    row.Item("CodViaRCP") = MySped.CodViaRCP
                    row.Item("ViaRCP") = MySped.ViaRCP
                    row.Item("PosizioneCivicoRCP") = MySped.PosizioneCivicoRCP
                    row.Item("CivicoRCP") = MySped.CivicoRCP
                    row.Item("EsponenteCivicoRCP") = MySped.EsponenteCivicoRCP
                    row.Item("ScalaCivicoRCP") = MySped.ScalaCivicoRCP
                    row.Item("InternoCivicoRCP") = MySped.InternoCivicoRCP
                    row.Item("FrazioneRCP") = MySped.FrazioneRCP
                    row.Item("DataInizioValiditaSpedizione") = MySped.DataInizioValiditaSpedizione
                    row.Item("DataFineValiditaSpedizione") = MySped.DataFineValiditaSpedizione
                    row.Item("DataUltimaModificaSpedizione") = MySped.DataUltimaModificaSpedizione
                    row.Item("OperatoreSpedizione") = MySped.OperatoreSpedizione
                End If
            Next
            '*** ***
            newTable.Rows.Add(row)
            newTable.AcceptChanges()

            objAnagrafica.Tables(0).ImportRow(row)
            objAnagrafica.Tables(0).AcceptChanges()

            Return objAnagrafica
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgovTIA.clsCalcoloRuolo.addRowsObjAnagrafica.errore: ", ex)
            Return Nothing
        End Try
    End Function
    Private Function CreateDsPerAnagrafica() As DataSet
        Dim objDS As New DataSet

        Dim newTable As DataTable
        Try
            newTable = New DataTable("ANAGRAFICA")

            Dim NewColumn As New DataColumn
            NewColumn.ColumnName = "COD_CONTRIBUENTE"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "ID_DATA_SPEDIZIONE"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "ID_DATA_ANAGRAFICA"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "Cognome"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "RappresentanteLegale"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "Nome"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "CodiceFiscale"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "INDIRIZZO"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "PartitaIva"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "CodiceComuneNascita"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "ComuneNascita"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "ProvinciaNascita"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "DataNascita"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "NazionalitaNascita"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "Sesso"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "CodiceComuneResidenza"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "ComuneResidenza"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "ProvinciaResidenza"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "CapResidenza"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "CodViaResidenza"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "ViaResidenza"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "PosizioneCivicoResidenza"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "CivicoResidenza"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "EsponenteCivicoResidenza"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "ScalaCivicoResidenza"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "InternoCivicoResidenza"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "FrazioneResidenza"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "NazionalitaResidenza"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "NucleoFamiliare"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "DATA_MORTE"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "Professione"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "Note"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "DaRicontrollare"
            NewColumn.DataType = System.Type.GetType("System.Boolean")
            NewColumn.DefaultValue = False
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "DataInizioValidita"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "DataFineValidita"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "DataUltimaModifica"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "Operatore"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "CodContribuenteRappLegale"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "CodEnte"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "CodIndividuale"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "CodFamiglia"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "NCTributari"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "DataUltimoAggTributi"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "NCAnagraficaRes"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "DataUltimoAggAnagrafe"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "CodTributo"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "CognomeInvio"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "NomeInvio"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "CodComuneRCP"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "ComuneRCP"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "LocRCP"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "ProvinciaRCP"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "CapRCP"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "CodViaRCP"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "ViaRCP"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "PosizioneCivicoRCP"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "CivicoRCP"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "EsponenteCivicoRCP"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "ScalaCivicoRCP"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "InternoCivicoRCP"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "FrazioneRCP"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "DataInizioValiditaSpedizione"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "DataFineValiditaSpedizione"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "DataUltimaModificaSpedizione"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "OperatoreSpedizione"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "TipoRiferimento"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)

            NewColumn = New DataColumn
            NewColumn.ColumnName = "DatiRiferimento"
            NewColumn.DataType = System.Type.GetType("System.String")
            NewColumn.DefaultValue = ""
            newTable.Columns.Add(NewColumn)


            objDS.Tables.Add(newTable)

            Return objDS
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgovTIA.clsCalcoloRuolo.CreateDsPerAnagrafica.errore: ", ex)
            Return Nothing
        End Try
    End Function
    Private Function ImportoArrotondato(ByVal importo As Double) As String
        'Funzione che in base alla nuova finanziaria prevede
        'gli importi arrotondati
        'x= importo da arrotondare + 0.5
        'importo arrotondato = parte intera di x

        Dim X As Double
        Dim ImportoOut As Long
        Try
            If importo > 0 Then

                X = importo + 0.5
                If InStr(X, ",") > 0 Then
                    ImportoOut = Left(X, InStr(X, ",") - 1)
                Else
                    ImportoOut = X
                End If

            ElseIf importo < 0 Then

                X = importo - 0.5
                If InStr(X, ",") > 0 Then
                    ImportoOut = Left(X, InStr(X, ",") - 1)
                Else
                    ImportoOut = X
                End If

            End If

        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgovTIA.clsCalcoloRuolo.ImportoArrotondato.errore: ", ex)
        End Try
        Return ImportoOut
    End Function
    Private Class ObjAppoggioIngiunzione
        Dim _ListSituazioneBase() As ComPlusInterface.ObjBaseIntSanz
        Dim _dsSanzioni As New DataSet
        Dim _ListInteressi() As ComPlusInterface.ObjInteressiSanzioni
        Dim _Atto As New ComPlusInterface.OggettoAttoTARSU
        Dim _ListDettaglio() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDettaglioAtto = Nothing
        Dim _ListDichiarato() As ObjArticoloAccertamento
        Dim _ListAccertato() As ObjArticoloAccertamento
        Property ListSituazioneBase As ComPlusInterface.ObjBaseIntSanz()
            Get
                Return _ListSituazioneBase
            End Get
            Set(ByVal value As ComPlusInterface.ObjBaseIntSanz())
                _ListSituazioneBase = value
            End Set
        End Property
        Property dsSanzioni As DataSet
            Get
                Return _dsSanzioni
            End Get
            Set(ByVal value As DataSet)
                _dsSanzioni = value
            End Set
        End Property
        Property ListInteressi As ComPlusInterface.ObjInteressiSanzioni()
            Get
                Return _ListInteressi
            End Get
            Set(ByVal value As ComPlusInterface.ObjInteressiSanzioni())
                _ListInteressi = value
            End Set
        End Property
        Property Atto As ComPlusInterface.OggettoAttoTARSU
            Get
                Return _Atto
            End Get
            Set(ByVal value As ComPlusInterface.OggettoAttoTARSU)
                _Atto = value
            End Set
        End Property
        Property ListDettaglio As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDettaglioAtto()
            Get
                Return _ListDettaglio
            End Get
            Set(ByVal value As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDettaglioAtto())
                _ListDettaglio = value
            End Set
        End Property
        Property ListDichiarato As ObjArticoloAccertamento()
            Get
                Return _ListDichiarato
            End Get
            Set(ByVal value As ObjArticoloAccertamento())
                _ListDichiarato = value
            End Set
        End Property
        Property ListAccertato As ObjArticoloAccertamento()
            Get
                Return _ListAccertato
            End Get
            Set(ByVal value As ObjArticoloAccertamento())
                _ListAccertato = value
            End Set
        End Property
    End Class
#End Region
#Region "Transaction"
    Public Function StartCalcoloMassivoTransaction() As SqlClient.SqlCommand
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myTrans As SqlClient.SqlTransaction
        Dim myConnection As New SqlClient.SqlConnection(ConstSession.StringConnection)
        myConnection.Open()
        myTrans = myConnection.BeginTransaction
        cmdMyCommand.Connection = myConnection
        cmdMyCommand.CommandTimeout = 0
        cmdMyCommand.Transaction = myTrans
        Return cmdMyCommand
    End Function
    Public Function OpenCalcoloMassivoConnection() As SqlClient.SqlCommand
        Dim cmdMyCommand As New SqlClient.SqlCommand
        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
        cmdMyCommand.Connection.Open()
        cmdMyCommand.CommandTimeout = 0
        Return cmdMyCommand
    End Function
    Public Sub CommitCalcoloMassivoTransaction(ByRef myTrans As SqlClient.SqlTransaction, ByRef cmdMyCommand As SqlClient.SqlCommand)
        myTrans.Commit()
        cmdMyCommand.Dispose()
        cmdMyCommand.Connection.Close()
    End Sub
    Public Sub RollbackCalcoloMassivoTransaction(ByRef myTrans As SqlClient.SqlTransaction, ByVal cmdMyCommand As SqlClient.SqlCommand)
        myTrans.Rollback()
        cmdMyCommand.Dispose()
        cmdMyCommand.Connection.Close()
    End Sub
#End Region
End Class
''' <summary>
''' Classe per la gestione cache dell'elaborazione asincrona
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class CacheManager
    Private Shared IISCache As System.Web.Caching.Cache = HttpRuntime.Cache
    Private Shared CalcoloMassivoInCorsoKey As String = "TARSUAnnoCalcoloMassivoInCorso"
    Private Shared AvanzamentoElaborazioneKey As String = "TARSUAvanzamentoElaborazione"
    Private Shared RiepilogoCalcoloMassivoKey As String = "TARSURiepilogoCalcoloMassivo"
    Private Shared RiepilogoCalcoloMassivoArtVSCatPFKey As String = "TARSURiepilogoCalcoloMassivoArtVSCatPFKey"
    Private Shared RiepilogoCalcoloMassivoArtVSCatPVKey As String = "TARSURiepilogoCalcoloMassivoArtVSCatPVKey"
    Private Shared RiepilogoCalcoloMassivoArtVSCatPCKey As String = "TARSURiepilogoCalcoloMassivoArtVSCatPCKey"
    Private Shared RiepilogoCalcoloMassivoArtVSCatPMKey As String = "TARSURiepilogoCalcoloMassivoArtVSCatPMKey"
    '*** 201809 Bollettazione Vigliano in OPENgov ***
    Private Shared RiepilogoImportRuoliKey As String = "TARSURiepilogoImportRuoli"
    Private Shared Log As ILog = LogManager.GetLogger(GetType(CacheManager))

    Private Sub New()
        MyBase.New()
    End Sub

#Region "Calcolo massivo in corso"
    Public Shared Function GetCalcoloMassivoInCorso() As Integer
        Try
            If (Not (IISCache(CalcoloMassivoInCorsoKey)) Is Nothing) Then
                Return CType(IISCache(CalcoloMassivoInCorsoKey), Integer)
            Else
                Return -1
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.CacheManger.GetCalcoloMassivoInCorso.errore: ", ex)
        End Try
    End Function
    Public Shared Sub SetCalcoloMassivoInCorso(ByVal Anno As Integer)
        IISCache.Insert(CalcoloMassivoInCorsoKey, Anno, Nothing, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, Nothing)
    End Sub
    Public Shared Sub RemoveCalcoloMassivoInCorso()
        IISCache.Remove(CalcoloMassivoInCorsoKey)
    End Sub
#End Region
#Region "Riepilogo calcolo Massivo Articoli VS Categorie"
    Public Shared Function GetRiepilogoCalcoloMassivo() As ObjRuolo()
        If (Not (IISCache(RiepilogoCalcoloMassivoKey)) Is Nothing) Then
            Return CType(IISCache(RiepilogoCalcoloMassivoKey), ObjRuolo())
        Else
            Return Nothing
        End If
    End Function
    Public Shared Sub SetRiepilogoCalcoloMassivo(ByVal listRuoli() As ObjRuolo)
        IISCache.Insert(RiepilogoCalcoloMassivoKey, listRuoli, Nothing, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, Nothing)
    End Sub
    Public Shared Sub RemoveRiepilogoCalcoloMassivo()
        IISCache.Remove(RiepilogoCalcoloMassivoKey)
    End Sub

    Public Shared Function GetRiepilogoCalcoloMassivoArtVSCatPF() As ObjArticolo()
        Try
            If (Not (IISCache(RiepilogoCalcoloMassivoArtVSCatPFKey)) Is Nothing) Then
                Return CType(IISCache(RiepilogoCalcoloMassivoArtVSCatPFKey), ObjArticolo())
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.CacheManger.GetRiepilogoCalcoloMassivoArtVSCatPF.errore: ", ex)
            Return Nothing
        End Try
    End Function
    Public Shared Sub SetRiepilogoCalcoloMassivoArtVSCatPF(ByVal ArtVSCat As ObjArticolo())
        IISCache.Insert(RiepilogoCalcoloMassivoArtVSCatPFKey, ArtVSCat, Nothing, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, Nothing)
    End Sub
    Public Shared Sub RemoveRiepilogoCalcoloMassivoArtVSCatPF()
        IISCache.Remove(RiepilogoCalcoloMassivoArtVSCatPFKey)
    End Sub

    Public Shared Function GetRiepilogoCalcoloMassivoArtVSCatPV() As ObjArticolo()
        Try
            If (Not (IISCache(RiepilogoCalcoloMassivoArtVSCatPVKey)) Is Nothing) Then
                Return CType(IISCache(RiepilogoCalcoloMassivoArtVSCatPVKey), ObjArticolo())
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.CacheManger.GetRiepilogoCalcoloMassivoArtVSCatPV.errore: ", ex)
            Return Nothing
        End Try
    End Function
    Public Shared Sub SetRiepilogoCalcoloMassivoArtVSCatPV(ByVal ArtVSCat As ObjArticolo())
        IISCache.Insert(RiepilogoCalcoloMassivoArtVSCatPVKey, ArtVSCat, Nothing, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, Nothing)
    End Sub
    Public Shared Sub RemoveRiepilogoCalcoloMassivoArtVSCatPV()
        IISCache.Remove(RiepilogoCalcoloMassivoArtVSCatPVKey)
    End Sub

    Public Shared Function GetRiepilogoCalcoloMassivoArtVSCatPC() As ObjArticolo()
        Try
            If (Not (IISCache(RiepilogoCalcoloMassivoArtVSCatPCKey)) Is Nothing) Then
                Return CType(IISCache(RiepilogoCalcoloMassivoArtVSCatPCKey), ObjArticolo())
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.CacheManger.GetRiepilogoCalcoloMassivoArtVSCatPC.errore: ", ex)
            Return Nothing
        End Try
    End Function
    Public Shared Sub SetRiepilogoCalcoloMassivoArtVSCatPC(ByVal ArtVSCat As ObjArticolo())
        IISCache.Insert(RiepilogoCalcoloMassivoArtVSCatPCKey, ArtVSCat, Nothing, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, Nothing)
    End Sub
    Public Shared Sub RemoveRiepilogoCalcoloMassivoArtVSCatPC()
        IISCache.Remove(RiepilogoCalcoloMassivoArtVSCatPCKey)
    End Sub

    Public Shared Function GetRiepilogoCalcoloMassivoArtVSCatPM() As ObjArticolo()
        Try
            If (Not (IISCache(RiepilogoCalcoloMassivoArtVSCatPMKey)) Is Nothing) Then
                Return CType(IISCache(RiepilogoCalcoloMassivoArtVSCatPMKey), ObjArticolo())
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.CacheManger.GetRiepilogoCalcoloMassivoArtVSCatPM.errore: ", ex)
            Return Nothing
        End Try
    End Function
    Public Shared Sub SetRiepilogoCalcoloMassivoArtVSCatPM(ByVal ArtVSCat As ObjArticolo())
        IISCache.Insert(RiepilogoCalcoloMassivoArtVSCatPMKey, ArtVSCat, Nothing, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, Nothing)
    End Sub
    Public Shared Sub RemoveRiepilogoCalcoloMassivoArtVSCatPM()
        IISCache.Remove(RiepilogoCalcoloMassivoArtVSCatPMKey)
    End Sub
#End Region
#Region "Avanzamento Elaborazione"
    Public Shared Function GetAvanzamentoElaborazione() As String
        Try
            If (Not (IISCache(AvanzamentoElaborazioneKey)) Is Nothing) Then
                Return CType(IISCache(AvanzamentoElaborazioneKey), String)
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.CacheManger.GetAvanzamentoElaborazione.errore: ", ex)
            Return Nothing
        End Try
    End Function
    Public Shared Sub SetAvanzamentoElaborazione(ByVal sMyDati As String)
        IISCache.Insert(AvanzamentoElaborazioneKey, sMyDati, Nothing, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, Nothing)
    End Sub
    Public Shared Sub RemoveAvanzamentoElaborazione()
        IISCache.Remove(AvanzamentoElaborazioneKey)
    End Sub
#End Region
    '*** 201809 Bollettazione Vigliano in OPENgov ***
#Region "Riepilogo Import Ruoli"
    Public Shared Function GetRiepilogoImportRuoli() As ObjRuolo()
        If (Not (IISCache(RiepilogoImportRuoliKey)) Is Nothing) Then
            Return CType(IISCache(RiepilogoImportRuoliKey), ObjRuolo())
        Else
            Return Nothing
        End If
    End Function
    Public Shared Sub SetRiepilogoImportRuoli(ByVal ListRuoli() As ObjRuolo)
        IISCache.Insert(RiepilogoImportRuoliKey, ListRuoli, Nothing, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, Nothing)
    End Sub
    Public Shared Sub RemoveRiepilogoImportRuoli()
        IISCache.Remove(RiepilogoImportRuoliKey)
    End Sub
#End Region
End Class
'*** ***
'*** 201809 Bollettazione Vigliano in OPENgov ***
''' <summary>
''' Classe per l'importazione del ruolo
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
#Region "Import Ruolo da Flusso"
Public Class ClsImportRuolo
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ClsImportRuolo))
    Private ListComuni() As OggettiComuniStrade.OggettoEnte
    Private ListRid() As ObjCodDescr

    Public Sub New()
    End Sub
    Public Sub New(myComuni() As OggettiComuniStrade.OggettoEnte)
        ListComuni = myComuni
    End Sub

    Public Function Controllo290(myFlusso As String, IdEnte As String, Belfiore As String, ByRef oRuolo As ObjRuolo) As Boolean
        Dim myRuolo As New ObjRuolo
        Dim LineFile As String = ""
        Dim TypeRC As String
        Dim RcTotali As Integer = 0
        Dim RcN0 As Integer = 0
        Dim RcN1 As Integer = 0
        Dim RcN2 As Integer = 0
        Dim RcN3 As Integer = 0
        Dim RcN4 As Integer = 0
        Dim RcN5 As Integer = 0
        Dim RcN6 As Integer = 0
        Dim RcN7 As Integer = 0
        Dim RcN8 As Integer = 0
        Dim RcN9 As Integer = 0
        Dim CountN2 As Integer = 0
        Dim CountN3 As Integer = 0
        Dim CountN4 As Integer = 0
        Dim CountN5 As Integer = 0
        Dim CountN6 As Integer = 0
        Dim CountN7 As Integer = 0
        Dim sPartitaAnagrafica As String = ""
        Dim PartiteIncongruenti As Integer = 0
        Dim bContabileMancante As Boolean = False
        Dim bCespiteMancante As Boolean = False
        Dim CountContabileMancante As Integer = 0
        Dim CountCespiteMancante As Integer = 0
        Dim CountDettaglioMancante As Integer = 0
        Dim CodEnteImpositore As String = ""

        'Apertura del file Ascii contenente i dati da importare
        Dim StreamReader As IO.StreamReader = Nothing
        Try
            myRuolo.IdFlusso = -1
            myRuolo.sEnte = IdEnte
            myRuolo.TipoGenerazione = ObjRuolo.Generazione.DaFlusso
            myRuolo.sTipoCalcolo = ObjRuolo.TipoCalcolo.TARES
            myRuolo.tDataCreazione = Now
            myRuolo.sNomeRuolo = myFlusso
            StreamReader = New IO.StreamReader(myFlusso)
            Do
                LineFile = StreamReader.ReadLine
                If Not IsNothing(LineFile) Then
                    TypeRC = LineFile.Substring(0, 2)
                    Select Case TypeRC
                        Case "N0"
                            RcN0 += 1
                            CodEnteImpositore = LineFile.Substring(15, 4).Trim
                        Case "N1"
                            RcN1 += 1
                        Case "N2"
                            CountN2 += 1
                            sPartitaAnagrafica = LineFile.Substring(10, 14).Trim
                            bContabileMancante = True
                            bCespiteMancante = True
                        Case "N3"
                            CountN3 += 1
                            If sPartitaAnagrafica <> LineFile.Substring(10, 14).Trim Then
                                PartiteIncongruenti += 1
                            End If
                        Case "N4"
                            CountN4 += 1
                            bContabileMancante = False
                            If sPartitaAnagrafica <> LineFile.Substring(10, 14).Trim Then
                                PartiteIncongruenti += 1
                            End If
                            myRuolo.sAnno = LineFile.Substring(24, 4)
                            myRuolo.sDescrRuolo = LineFile.Substring(42, 30).Trim
                            Select Case LineFile.Substring(139, 1).Trim.ToUpper
                                Case "S"
                                    myRuolo.sTipoRuolo = ObjRuolo.Ruolo.AConguaglio
                                Case Else
                                    myRuolo.sTipoRuolo = ObjRuolo.Ruolo.APercentuale
                            End Select
                        Case "N5"
                            CountN5 += 1
                            If bContabileMancante = True Then
                                CountContabileMancante += 1
                            End If
                            bCespiteMancante = False
                            If sPartitaAnagrafica <> LineFile.Substring(10, 14).Trim Then
                                PartiteIncongruenti += 1
                            End If
                        Case "N6"
                            CountN6 += 1
                            If bCespiteMancante = True Then
                                CountCespiteMancante += 1
                            End If
                            If sPartitaAnagrafica <> LineFile.Substring(10, 14).Trim Then
                                PartiteIncongruenti += 1
                            End If
                        Case "N7"
                            CountN7 += 1
                            If bContabileMancante = True Then
                                CountContabileMancante += 1
                            End If
                            bCespiteMancante = False
                            If sPartitaAnagrafica <> LineFile.Substring(10, 14).Trim Then
                                PartiteIncongruenti += 1
                            End If
                        Case "N8"
                            RcN8 += 1
                            RcN2 = StringOperation.FormatInt(LineFile.Substring(17, 7))
                            RcN3 = StringOperation.FormatInt(LineFile.Substring(24, 7))
                            RcN4 = StringOperation.FormatInt(LineFile.Substring(31, 7))
                            RcN5 = StringOperation.FormatInt(LineFile.Substring(38, 7))
                            RcN6 = StringOperation.FormatInt(LineFile.Substring(45, 7))
                            RcN7 = StringOperation.FormatInt(LineFile.Substring(67, 7))
                            If IsNumeric(LineFile.Substring(52, 15).Trim) Then
                                myRuolo.ImpAvvisi = LineFile.Substring(52, 15) / 100
                            End If
                        Case "N9"
                            RcN9 += 1
                            RcTotali = StringOperation.FormatInt(LineFile.Substring(7, 7))
                    End Select
                End If
            Loop Until LineFile Is Nothing

            myRuolo.nNumeroRuolo = New ClsGestRuolo().GetNumeroRuolo(IdEnte, myRuolo.sTipoCalcolo, myRuolo.sAnno)
            'myRuolo.ImpMinimo = 0
            'myRuolo.nTassazioneMinima = 0
            'myRuolo.PercentTariffe = 100
            'myRuolo.tDataInizioConf = DateTime.MaxValue
            'myRuolo.nScarti = 0

            myRuolo.nContribuenti = CountN2
            myRuolo.nArticoli = CountN6
            myRuolo.nAvvisi = CountN4

            If CodEnteImpositore = "" Then
                myRuolo.sNote = "Manca il codice ente impositore. "
            Else
                If CodEnteImpositore <> Belfiore Then
                    myRuolo.sNote = "Codice Ente non coerente. "
                End If
            End If
            If RcN0 <> 1 Or RcN9 <> 1 Or RcN1 <> RcN8 Or RcN1 = 0 Or RcN8 = 0 Then
                myRuolo.sNote += "La sequenza dei record di apertura e chiusura del file è errata! "
            End If
            If CountN2 <> RcN2 Then
                myRuolo.sNote += "Il Totale Record Anagrafici analizzati con corrisponde con il totale presente sul record di chiusura! "
            End If
            If CountN3 <> RcN3 Then
                myRuolo.sNote += "Il Totale Record di Recapito analizzati con corrisponde con il totale presente sul record di chiusura! "
            End If
            If CountN4 <> RcN4 Then
                myRuolo.sNote += "Il Totale Record Contabili analizzati con corrisponde con il totale presente sul record di chiusura! "
            End If
            If CountN5 <> RcN5 Then
                myRuolo.sNote += "Il Totale Record dei Cespiti analizzati con corrisponde con il totale presente sul record di chiusura! "
            End If
            If CountN6 <> RcN6 Then
                myRuolo.sNote += "Il Totale Record di Dettaglio Tariffa analizzati con corrisponde con il totale presente sul record di chiusura! "
            End If
            If CountN7 <> RcN7 Then
                myRuolo.sNote += "Il Totale Record dei Cespiti analizzati con corrisponde con il totale presente sul record di chiusura! "
            End If
            If RcTotali <> (RcN0 + RcN1 + CountN2 + CountN3 + CountN4 + CountN5 + CountN6 + CountN7 + RcN8 + RcN9) Then
                myRuolo.sNote += "Il Totale Record analizzati con corrisponde con il totale presente sul record di chiusura! "
            End If
            If PartiteIncongruenti > 0 Then
                myRuolo.sNote += "Sono presenti n." & PartiteIncongruenti & " Partite Incongruenti! "
            End If
            If CountContabileMancante > 0 Then
                myRuolo.sNote += "Non sono presenti n." & CountContabileMancante & " Record Contabili! "
            End If
            If CountCespiteMancante > 0 Then
                myRuolo.sNote += "Non sono presenti n." & CountCespiteMancante & " Record di Cespiti! "
            End If
            If myRuolo.sNote = "" Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Log.Debug(IdEnte + " - OPENgovTIA.ClsImportRuolo.Controllo290.errore: ", ex)
            myRuolo.sNote += "Controllo290.errore::" + ex.Message
            Return False
        Finally
            StreamReader.Close()
            oRuolo = myRuolo
        End Try
    End Function
    Public Function Import290(myFlusso As String, IdEnte As String, ByRef ListAnagrafiche As ArrayList, myStringConnection As String, myStringConnectionAnag As String) As ObjAvviso()
        Dim StreamReader As IO.StreamReader = Nothing
        Dim LineFile As String
        Dim myAnagrafica As New AnagInterface.DettaglioAnagrafica
        Dim ListAvvisi As New ArrayList
        Dim myAvviso As New ObjAvviso
        Dim ListPartite As New ArrayList
        Dim myPartitaCont As New ObjArticolo

        Try
            StreamReader = New IO.StreamReader(myFlusso)
            Do
                LineFile = StreamReader.ReadLine
                If Not IsNothing(LineFile) Then
                    Select Case LineFile.Substring(0, 2).Trim
                        Case "N2" 'anagrafica
                            If myAvviso.IdEnte <> "" Then
                                myAvviso = CompleteAvviso(myAvviso, ListPartite)
                                If myAvviso Is Nothing Then
                                    Return Nothing
                                End If
                                ListAvvisi.Add(myAvviso)
                            End If
                            myAvviso = New ObjAvviso
                            ListPartite = New ArrayList
                            myAvviso.oDetVoci = (New ArrayList).ToArray(GetType(ObjDetVoci))
                            myAvviso.oArticoli = ListPartite.ToArray(GetType(ObjArticolo))
                            myAnagrafica = New AnagInterface.DettaglioAnagrafica
                            myAnagrafica = LoadAnagrafica(IdEnte, LineFile)
                            If myAnagrafica Is Nothing Then
                                Return Nothing
                            End If
                            ListAnagrafiche.Add(myAnagrafica)
                        Case "N3" 'indirizzo spedizione
                            myAnagrafica.ListSpedizioni = LoadSpedizione(IdEnte, LineFile)
                            If myAnagrafica.ListSpedizioni Is Nothing Then
                                Return Nothing
                            End If
                        Case "N4" 'fattura
                            myAvviso = New ObjAvviso
                            myAvviso = LoadAvviso(IdEnte, LineFile, myAnagrafica)
                            If myAvviso Is Nothing Then
                                Return Nothing
                            End If
                            myAvviso.IdEnte = IdEnte
                        Case "N7" 'estensione dati fattura
                            myAvviso = LoadAvvisoExt(IdEnte, LineFile, myAvviso)
                            If myAvviso Is Nothing Then
                                Return Nothing
                            End If
                        Case "N5" 'ubicazione
                            myPartitaCont = New ObjArticolo
                            myPartitaCont = LoadPartitaCont(IdEnte, LineFile)
                            If myPartitaCont Is Nothing Then
                                Return Nothing
                            End If
                            If ListRid Is Nothing Then
                                ListRid = GetRid(IdEnte, myStringConnection)
                            End If
                        Case "N6" 'partita contabile
                            ListPartite = LoadPartitaContExt(IdEnte, LineFile, myPartitaCont, ListPartite, myStringConnection, myStringConnectionAnag)
                            If ListPartite Is Nothing Then
                                Return Nothing
                            End If
                    End Select
                End If
            Loop Until LineFile Is Nothing
            myAvviso = CompleteAvviso(myAvviso, ListPartite)
            If myAvviso Is Nothing Then
                Return Nothing
            End If
            ListAvvisi.Add(myAvviso)

            Return ListAvvisi.ToArray(GetType(ObjAvviso))
        Catch ex As Exception
            Log.Debug(IdEnte + " - OPENgovTIA.ClsImportRuolo.Import290.errore: ", ex)
            Return Nothing
        Finally
            StreamReader.Close()
        End Try
    End Function
    Private Function LoadAnagrafica(IdEnte As String, LineFile As String) As AnagInterface.DettaglioAnagrafica
        Dim myItem As New AnagInterface.DettaglioAnagrafica
        Dim myStart, myLength As Integer
        Try
            myItem.CodEnte = IdEnte
            myStart = 0
            myLength = 2 'tipo record N2
            myStart += myLength
            myLength = 6 'codice comune CNC
            myStart += myLength
            myLength = 2 'progressivo minuta
            myStart += myLength
            myLength = 14 'codice partita
            myStart += myLength
            myLength = 16 'codice fiscale
            If LineFile.Substring(myStart, myLength).Trim.Length < 16 Then
                myItem.PartitaIva = LineFile.Substring(myStart, myLength).Trim
            Else
                myItem.CodiceFiscale = LineFile.Substring(myStart, myLength).Trim
            End If
            myStart += myLength
            myLength = 70 'indirizzo
            myItem.ViaResidenza = LineFile.Substring(myStart, myLength).Trim
            myStart += myLength
            myLength = 5 'civico
            myItem.CivicoResidenza = LineFile.Substring(myStart, myLength).Trim
            myStart += myLength
            myLength = 2 'lettera
            myItem.EsponenteCivicoResidenza = LineFile.Substring(myStart, myLength).Trim
            myStart += myLength
            myLength = 5 'CAP
            myItem.CapResidenza = LineFile.Substring(myStart, myLength).Trim
            myStart += myLength
            myLength = 4 'Belfiore comune
            myItem.CodiceComuneResidenza = LineFile.Substring(myStart, myLength).Trim
            myStart += myLength
            myLength = 54 'frazione
            myItem.FrazioneResidenza = LineFile.Substring(myStart, myLength).Trim
            myStart += myLength
            myLength = 18 'filler
            myStart += myLength
            myLength = 1 'natura giuridica
            If LineFile.Substring(myStart, myLength).Trim = "2" Then
                myItem.Sesso = "G"
            End If
            myStart += myLength
            myLength = 30 'cognome
            If myItem.Sesso = "G" Then
                myLength = 91 'denominazione società
                myItem.Cognome = LineFile.Substring(myStart, myLength).Trim
            Else
                myItem.Cognome = LineFile.Substring(myStart, myLength).Trim
                myStart += myLength
                myLength = 24 'nome
                myItem.Nome = LineFile.Substring(myStart, myLength).Trim
                myStart += myLength
                myLength = 1 'sesso
                myItem.Sesso = LineFile.Substring(myStart, myLength).Trim
                myStart += myLength
                myLength = 8 'data nascita
                If LineFile.Substring(myStart, myLength).Trim.Replace("0", "") <> "" Then
                    myItem.DataNascita = StringOperation.FormatDateTime(LineFile.Substring(myStart, 2).Trim + "/" + LineFile.Substring(myStart + 2, 2).Trim + "/" + LineFile.Substring(myStart + 4, 4).Trim)
                End If
                myStart += myLength
                myLength = 4 'belfiore nascita
                myItem.CodiceComuneNascita = LineFile.Substring(myStart, myLength).Trim
            End If
            myItem.ComuneNascita = GetComune(myItem.CodiceComuneNascita)
            myItem.ProvinciaNascita = GetProvincia(myItem.CodiceComuneNascita)
            myItem.ComuneResidenza = GetComune(myItem.CodiceComuneResidenza)
            myItem.ProvinciaResidenza = GetProvincia(myItem.CodiceComuneResidenza)
            If myItem.FrazioneResidenza = myItem.ComuneResidenza Then
                myItem.FrazioneResidenza = ""
            ElseIf myItem.ComuneResidenza = "" Then
                myItem.ComuneResidenza = myItem.FrazioneResidenza
            End If
        Catch ex As Exception
            Log.Debug(IdEnte + " - OPENgovTIA.ClsImportRuolo.LoadAnagrafica.errore: ", ex)
            myItem = Nothing
        End Try
        Return myItem
    End Function
    Private Function LoadSpedizione(IdEnte As String, LineFile As String) As Generic.List(Of AnagInterface.ObjIndirizziSpedizione)
        Dim ListItem As New Generic.List(Of AnagInterface.ObjIndirizziSpedizione)
        Dim myItem As New AnagInterface.ObjIndirizziSpedizione
        Dim myStart, myLength As Integer
        Try
            myStart = 0
            myLength = 2 'tipo record N3
            myStart += myLength
            myLength = 6 'codice comune CNC
            myStart += myLength
            myLength = 2 'progressivo minuta
            myStart += myLength
            myLength = 14 'codice partita
            myStart += myLength
            myLength = 16 'filler
            myStart += myLength
            If LineFile.Substring(myStart).Replace("0", "").Trim <> "" Then
                myLength = 70 'indirizzo
                myItem.ViaRCP = LineFile.Substring(myStart, myLength).Trim
                myStart += myLength
                myLength = 5 'civico
                myItem.CivicoRCP = LineFile.Substring(myStart, myLength).Trim
                myStart += myLength
                myLength = 2 'lettera
                myItem.EsponenteCivicoRCP = LineFile.Substring(myStart, myLength).Trim
                myStart += myLength
                myLength = 5 'CAP
                myItem.CapRCP = LineFile.Substring(myStart, myLength).Trim
                myStart += myLength
                myLength = 4 'belfiore comune
                myItem.CodComuneRCP = LineFile.Substring(myStart, myLength).Trim
                myStart += myLength
                myLength = 54 'frazione
                myItem.FrazioneRCP = LineFile.Substring(myStart, myLength).Trim
                myStart += myLength
                myLength = 19 'filler
                myStart += myLength
                myLength = 70 'nominativo
                myItem.CognomeInvio = LineFile.Substring(myStart, myLength).Trim
                myItem.ComuneRCP = GetComune(myItem.CodComuneRCP)
                myItem.ProvinciaRCP = GetProvincia(myItem.CodComuneRCP)
                If myItem.FrazioneRCP = myItem.ComuneRCP Then
                    myItem.FrazioneRCP = ""
                ElseIf myItem.ComuneRCP = "" Then
                    myItem.ComuneRCP = myItem.FrazioneRCP
                End If
                ListItem.Add(myItem)
            End If
        Catch ex As Exception
            Log.Debug(IdEnte + " - OPENgovTIA.ClsImportRuolo.LoadSpedizione.errore: ", ex)
            ListItem = Nothing
        End Try
        Return ListItem
    End Function
    Private Function LoadAvviso(IdEnte As String, LineFile As String, myAnagrafica As AnagInterface.DettaglioAnagrafica) As ObjAvviso
        Dim myItem As New ObjAvviso
        Dim myStart, myLength As Integer
        Dim myExt As New ObjDetVoci
        Dim ListDet As New ArrayList
        Try
            myStart = 0
            myLength = 2 'tipo record N4
            myStart += myLength
            myLength = 6 'codice comune CNC
            myStart += myLength
            myLength = 2 'progressivo minuta
            myStart += myLength
            myLength = 14 'codice partita
            myItem.sCodiceCliente = "74882" + (LineFile.Substring(myStart, myLength).Trim).Substring(5, 9).PadLeft(9, "0") + "14"
            myStart += myLength
            myLength = 4 'anno del tributo
            myItem.sAnnoRiferimento = LineFile.Substring(myStart, myLength).Trim
            myStart += myLength
            myLength = 4 'anno di emissione fattura
            myStart += myLength
            myLength = 10 'numero fattura
            myItem.sCodiceCartella = LineFile.Substring(myStart, myLength).Trim
            myStart += myLength
            myLength = 30 'periodo
            myStart += myLength
            myLength = 10 'importo netto 
            If StringOperation.FormatInt(LineFile.Substring(myStart, myLength).Trim) > 0 Then
                myExt = New ObjDetVoci
                myExt.sCapitolo = ObjDetVoci.Capitolo.Imposta
                If IsNumeric(LineFile.Substring(myStart, myLength).Trim) Then
                    myExt.impDettaglio = LineFile.Substring(myStart, myLength).Trim / 100
                End If
                myExt.IdEnte = myItem.IdEnte
                myExt.sAnno = myItem.sAnnoRiferimento
                myExt.sOperatore = myItem.sOperatore
                ListDet.Add(myExt)
            End If
            myStart += myLength
            myLength = 10 'importo iva
            If StringOperation.FormatInt(LineFile.Substring(myStart, myLength).Trim) > 0 Then
                myExt = New ObjDetVoci
                myExt.sCapitolo = ObjDetVoci.Capitolo.IVA
                If IsNumeric(LineFile.Substring(myStart, myLength).Trim) Then
                    myExt.impDettaglio = LineFile.Substring(myStart, myLength).Trim / 100
                End If
                myExt.IdEnte = myItem.IdEnte
                myExt.sAnno = myItem.sAnnoRiferimento
                myExt.sOperatore = myItem.sOperatore
                ListDet.Add(myExt)
            End If
            myStart += myLength
            myLength = 10 'importo provinciale
            If StringOperation.FormatInt(LineFile.Substring(myStart, myLength).Trim) > 0 Then
                myExt = New ObjDetVoci
                myExt.sCapitolo = ObjDetVoci.Capitolo.Provinciale
                If IsNumeric(LineFile.Substring(myStart, myLength).Trim) Then
                    myExt.impDettaglio = LineFile.Substring(myStart, myLength).Trim / 100
                End If
                myExt.IdEnte = myItem.IdEnte
                myExt.sAnno = myItem.sAnnoRiferimento
                myExt.sOperatore = myItem.sOperatore
                ListDet.Add(myExt)
            End If
            myStart += myLength
            myLength = 10 'totale
            If IsNumeric(LineFile.Substring(myStart, myLength).Trim) Then
                myItem.impTotale = LineFile.Substring(myStart, myLength).Trim / 100
            End If
            myItem.impCarico = myItem.impTotale
            myItem.impDovuto = myItem.impTotale
            myStart += myLength
            myLength = 8 'data emissione
            myItem.tDataEmissione = StringOperation.FormatDateTime(LineFile.Substring(myStart, 2).Trim + "/" + LineFile.Substring(myStart + 2, 2).Trim + "/" + LineFile.Substring(myStart + 4, 4).Trim)
            myStart += myLength
            myLength = 8 'data scadenza
            myStart += myLength
            myLength = 10 'importo netto tariffa per l'anno
            'If stringoperation.formatint(LineFile.Substring(myStart, myLength).Trim) > 0 Then
            '    myExt = New ObjDetVoci
            '    myExt.sCapitolo = ObjDetVoci.Capitolo.Imposta
            '    myExt.CodVoce = ObjDetVoci.Voce.NettoAnno
            '    If IsNumeric(LineFile.Substring(myStart, myLength).Trim) Then
            '        myExt.impDettaglio = LineFile.Substring(myStart, myLength).Trim / 100
            '    End If
            '    myExt.IdEnte = myItem.IdEnte
            '    myExt.sAnno = myItem.sAnnoRiferimento
            '    myExt.sOperatore = myItem.sOperatore
            '    If myExt.impDettaglio > 0 Then
            '        ListDet.Add(myExt)
            '    End If
            'End If
            myStart += myLength
            myLength = 1 'filler
            myStart += myLength
            myLength = 1 'tipo fatturazione
            myStart += myLength
            myLength = 10 'importo già conguagliato
            myStart += myLength
            Dim ListPrev As New ArrayList
            For myStart = myStart To 277
                myLength = 4 'anno acconto
                Dim myPrevAvv As New ObjArticolo
                myPrevAvv.TipoPartita = ObjArticolo.PARTEFISSA
                myPrevAvv.sVia = ObjArticolo.PARTEPRECEMESSO_DESCR + " per l'anno " + LineFile.Substring(myStart, myLength).Trim
                myStart += myLength
                myLength = 10 'fattura acconto
                myPrevAvv.sVia += " (Fattura N. " + LineFile.Substring(myStart, myLength).Trim
                myStart += myLength
                myLength = 8 'data acconto
                If LineFile.Substring(myStart, myLength).Trim.Replace("0", "") <> "" Then
                    myPrevAvv.sVia += " del " + LineFile.Substring(myStart, 2).Trim + "/" + LineFile.Substring(myStart + 2, 2).Trim + "/" + LineFile.Substring(myStart + 4, 4).Trim + ")"
                End If
                myStart += myLength
                myLength = 10 'importo acconto
                If IsNumeric(LineFile.Substring(myStart, myLength).Trim) Then
                    myPrevAvv.impNetto = LineFile.Substring(myStart, myLength).Trim / 100
                    myPrevAvv.impNetto *= -1
                End If
                myPrevAvv.impRuolo = myPrevAvv.impNetto
                myStart += myLength - 1
                If myPrevAvv.impNetto <> 0 Then
                    ListPrev.Add(myPrevAvv)
                End If
            Next
            myLength = 10 'importo netto tariffa a conguaglio
            'If LineFile.Substring(myStart, myLength).Trim.Replace("0", "") <> "" Then
            '    myExt = New ObjDetVoci
            '    myExt.sCapitolo = ObjDetVoci.Capitolo.Imposta
            '    myExt.CodVoce = ObjDetVoci.Voce.ConguaglioAnno
            '    If IsNumeric(LineFile.Substring(myStart, myLength).Trim) Then
            '        myExt.impDettaglio = LineFile.Substring(myStart, myLength).Trim / 100
            '    End If
            '    myExt.IdEnte = myItem.IdEnte
            '    myExt.sAnno = myItem.sAnnoRiferimento
            '    myExt.sOperatore = myItem.sOperatore
            '    If myExt.impDettaglio > 0 Then
            '        ListDet.Add(myExt)
            '    End If
            'End If
            myStart += myLength
            myLength = 2 'filler
            myItem.impArrotondamento = myItem.impCarico
            For Each myVoce As ObjDetVoci In ListDet
                myItem.impArrotondamento -= myVoce.impDettaglio
            Next
            If myItem.impArrotondamento <> 0 Then
                myExt = New ObjDetVoci
                myExt.sCapitolo = ObjDetVoci.Capitolo.Arrotondamento
                myExt.impDettaglio = myItem.impArrotondamento
                myExt.IdEnte = myItem.IdEnte
                myExt.sAnno = myItem.sAnnoRiferimento
                myExt.sOperatore = myItem.sOperatore
                ListDet.Add(myExt)
            End If
            If ListDet.Count > 0 Then
                myItem.oDetVoci = ListDet.ToArray(GetType(ObjDetVoci))
            End If
            If ListPrev.Count > 0 Then
                myItem.oArticoliPrec = ListPrev.ToArray(GetType(ObjArticolo))
            End If
            myItem.sCognome = myAnagrafica.Cognome
            myItem.sNome = myAnagrafica.Nome
            myItem.sCodFiscale = myAnagrafica.CodiceFiscale
            myItem.sPIVA = myAnagrafica.PartitaIva
            myItem.sIndirizzoRes = myAnagrafica.ViaResidenza
            myItem.sCivicoRes = myAnagrafica.CivicoResidenza
            myItem.sFrazRes = myAnagrafica.FrazioneResidenza
            myItem.sComuneRes = myAnagrafica.ComuneResidenza
            myItem.sCAPRes = myAnagrafica.CapResidenza
            myItem.sProvRes = myAnagrafica.ProvinciaResidenza
            For Each mySped As AnagInterface.ObjIndirizziSpedizione In myAnagrafica.ListSpedizioni
                myItem.sNominativoCO = mySped.CognomeInvio
                myItem.sIndirizzoCO = mySped.ViaRCP
                myItem.sCivicoCO = mySped.CivicoRCP
                myItem.sFrazCO = mySped.FrazioneRCP
                myItem.sCAPCO = mySped.CapRCP
                myItem.sComuneCO = mySped.ComuneRCP
                myItem.sProvCO = mySped.ProvinciaRCP
            Next
        Catch ex As Exception
            Log.Debug(IdEnte + " - OPENgovTIA.ClsImportRuolo.LoadAvviso.errore: ", ex)
            myItem = Nothing
        End Try
        Return myItem
    End Function
    Private Function LoadAvvisoExt(IdEnte As String, LineFile As String, myAvviso As ObjAvviso) As ObjAvviso
        Dim myItem As New ObjAvviso
        Dim ListItem As New ArrayList
        Dim myExt As New ObjDetVoci
        Dim myStart, myLength As Integer
        Try
            myItem = myAvviso
            myStart = 0
            myLength = 2 'tipo record N7
            myStart += myLength
            myLength = 6 'codice comune CNC
            myStart += myLength
            myLength = 2 'progressivo minuta
            myStart += myLength
            myLength = 14 'codice partita
            myStart += myLength
            If LineFile.Substring(myStart).Replace("0", "").Trim <> "" Then
                If Not myItem.oDetVoci Is Nothing Then
                    For Each myDet As ObjDetVoci In myItem.oDetVoci
                        ListItem.Add(myDet)
                    Next
                End If
                myExt = New ObjDetVoci
                myLength = 10 'sanzione totale
                myExt.sCapitolo = ObjDetVoci.Capitolo.Sanzione
                If IsNumeric(LineFile.Substring(myStart, myLength).Trim) Then
                    myExt.impDettaglio = LineFile.Substring(myStart, myLength).Trim / 100
                End If
                myExt.IdEnte = myItem.IdEnte
                myExt.sAnno = myItem.sAnnoRiferimento
                myExt.sOperatore = myItem.sOperatore
                If myExt.impDettaglio > 0 Then
                    ListItem.Add(myExt)
                End If
                myStart += myLength
                myExt = New ObjDetVoci
                myLength = 10 'interessi totali
                myExt.sCapitolo = ObjDetVoci.Capitolo.Interessi
                If IsNumeric(LineFile.Substring(myStart, myLength).Trim) Then
                    myExt.impDettaglio = LineFile.Substring(myStart, myLength).Trim / 100
                End If
                myExt.IdEnte = myItem.IdEnte
                myExt.sAnno = myItem.sAnnoRiferimento
                myExt.sOperatore = myItem.sOperatore
                If myExt.impDettaglio > 0 Then
                    ListItem.Add(myExt)
                End If
                myStart += myLength
                myExt = New ObjDetVoci
                myLength = 10 'spese di notifica totali
                myExt.sCapitolo = ObjDetVoci.Capitolo.SpeseNotifica
                If IsNumeric(LineFile.Substring(myStart, myLength).Trim) Then
                    myExt.impDettaglio = LineFile.Substring(myStart, myLength).Trim / 100
                End If
                myExt.IdEnte = myItem.IdEnte
                myExt.sAnno = myItem.sAnnoRiferimento
                myExt.sOperatore = myItem.sOperatore
                If myExt.impDettaglio > 0 Then
                    ListItem.Add(myExt)
                End If
                myStart += myLength
                myLength = 10 'sanzione anno
                myStart += myLength
                myLength = 10 'interessi anno
                myStart += myLength
                myLength = 10 'spese anno
                myStart += myLength
                myLength = 10 'sanzione conguaglio
                myStart += myLength
                myLength = 10 'interessi conguaglio
                myStart += myLength
                myLength = 10 'spese conguaglio
                myStart += myLength
                Dim nAcconto As Integer = 0
                For myStart = myStart To 234
                    nAcconto += 1
                    myExt = New ObjDetVoci
                    myLength = 10 'sanzioni
                    myExt.sCapitolo = ObjDetVoci.Capitolo.Sanzione
                    myExt.CodVoce = nAcconto
                    If IsNumeric(LineFile.Substring(myStart, myLength).Trim) Then
                        myExt.impDettaglio = LineFile.Substring(myStart, myLength).Trim / 100
                    End If
                    myExt.IdEnte = myItem.IdEnte
                    myExt.sAnno = myItem.sAnnoRiferimento
                    myExt.sOperatore = myItem.sOperatore
                    If myExt.impDettaglio > 0 Then
                        ListItem.Add(myExt)
                    End If
                    myStart += myLength
                    myExt = New ObjDetVoci
                    myLength = 10 'interessi
                    myExt.sCapitolo = ObjDetVoci.Capitolo.Interessi
                    myExt.CodVoce = nAcconto
                    If IsNumeric(LineFile.Substring(myStart, myLength).Trim) Then
                        myExt.impDettaglio = LineFile.Substring(myStart, myLength).Trim / 100
                    End If
                    myExt.IdEnte = myItem.IdEnte
                    myExt.sAnno = myItem.sAnnoRiferimento
                    myExt.sOperatore = myItem.sOperatore
                    If myExt.impDettaglio > 0 Then
                        ListItem.Add(myExt)
                    End If
                    myStart += myLength
                    myExt = New ObjDetVoci
                    myLength = 10 'spese di notifica
                    myExt.sCapitolo = ObjDetVoci.Capitolo.SpeseNotifica
                    myExt.CodVoce = nAcconto
                    If IsNumeric(LineFile.Substring(myStart, myLength).Trim) Then
                        myExt.impDettaglio = LineFile.Substring(myStart, myLength).Trim / 100
                    End If
                    myExt.IdEnte = myItem.IdEnte
                    myExt.sAnno = myItem.sAnnoRiferimento
                    myExt.sOperatore = myItem.sOperatore
                    If myExt.impDettaglio > 0 Then
                        ListItem.Add(myExt)
                    End If
                    myStart += myLength - 1
                Next
                myLength = 10 'sanzione saldo
                myStart += myLength
                myLength = 10 'interessi saldo
                myStart += myLength
                myLength = 10 'spese saldo
                myStart += myLength
                myLength = 26 'filler
                myItem.oDetVoci = ListItem.ToArray(GetType(ObjDetVoci))
            End If
        Catch ex As Exception
            Log.Debug(IdEnte + " - OPENgovTIA.ClsImportRuolo.LoadAvvisoExt.errore: ", ex)
            myItem = Nothing
        End Try
        Return myItem
    End Function
    Private Function LoadPartitaCont(IdEnte As String, LineFile As String) As ObjArticolo
        Dim myItem As New ObjArticolo
        Dim myStart, myLength As Integer
        Try
            myItem.IdEnte = IdEnte
            myStart = 0
            myLength = 2 'tipo record N5
            myStart += myLength
            myLength = 6 'codice comune CNC
            myStart += myLength
            myLength = 2 'progressivo minuta
            myStart += myLength
            myLength = 14 'codice partita
            myStart += myLength
            For myStart = myStart To 263
                myLength = 40 'indirizzo
                If LineFile.Substring(myStart, myLength).Trim <> "" Then
                    myItem.sVia += LineFile.Substring(myStart, myLength).Trim + ","
                End If
                myStart += myLength - 1
            Next
            If myItem.sVia <> "" Then
                myItem.sVia = myItem.sVia.Substring(0, myItem.sVia.Length - 1)
            End If
            myLength = 4 'anno
            myItem.sAnno = LineFile.Substring(myStart, myLength).Trim
            myStart += myLength
            myLength = 22 'filler
        Catch ex As Exception
            Log.Debug(IdEnte + " - OPENgovTIA.ClsImportRuolo.LoadPartitaCont.errore: ", ex)
            myItem = Nothing
        End Try
        Return myItem
    End Function
    Private Function LoadPartitaContExt(IdEnte As String, LineFile As String, myPartita As ObjArticolo, ListPartite As ArrayList, myStringConnection As String, myStringConnectionAnag As String) As ArrayList
        Dim ListItem As New ArrayList
        Dim myPF As New ObjArticolo
        Dim myPV As New ObjArticolo
        Dim ListRid As New ArrayList
        Dim myRid As New ObjRidEseApplicati
        Dim myStart, myLength As Integer
        Try
            For Each myItem As ObjArticolo In ListPartite
                ListItem.Add(myItem)
            Next
            myPF = myPartita
            myPV.TipoPartita = ObjArticolo.PARTEVARIABILE
            myPV.IdEnte = myPF.IdEnte
            myPV.sAnno = myPF.sAnno
            myRid.IdEnte = myPartita.IdEnte
            myRid.sAnno = myPartita.sAnno
            myStart = 0
            myLength = 2 'tipo record N6
            myStart += myLength
            myLength = 6 'codice comune CNC
            myStart += myLength
            myLength = 2 'progressivo minuta
            myStart += myLength
            myLength = 14 'codice partita
            myStart += myLength
            myLength = 1 'tipo utenza
            Select Case LineFile.Substring(myStart, myLength).Trim
                Case "D", "N"
                    myPF.TipoPartita = ObjArticolo.PARTEFISSA
                Case "S"
                    myPF.TipoPartita = "SA"
                Case "I"
                    myPF.TipoPartita = "IN"
                Case "P"
                    myPF.TipoPartita = "SP"
                Case "R"
                    myPF.TipoPartita = "RI"
            End Select
            myStart += myLength
            myLength = 10 'mq
            If IsNumeric(LineFile.Substring(myStart, myLength).Trim) Then
                myPF.nMQ = LineFile.Substring(myStart, myLength).Trim / 100
                myPV.nMQ = myPF.nMQ
            End If
            myStart += myLength
            myLength = 3 'n occupanti
            myPF.nComponenti = StringOperation.FormatInt(LineFile.Substring(myStart, myLength).Trim)
            myPV.nComponenti = myPF.nComponenti
            myStart += myLength
            myLength = 3 'mesi
            myPF.nBimestri = LineFile.Substring(myStart, myLength).Trim
            myPV.nBimestri = myPF.nBimestri
            myStart += myLength
            myLength = 13 'tariffa PF
            If IsNumeric(LineFile.Substring(myStart, myLength).Trim) Then
                myPF.impTariffa = LineFile.Substring(myStart, myLength).Trim / 100000
            End If
            myStart += myLength
            myLength = 10 'importo PF
            If IsNumeric(LineFile.Substring(myStart, myLength).Trim) Then
                myPF.impRuolo = LineFile.Substring(myStart, myLength).Trim / 100
                myPF.impNetto = LineFile.Substring(myStart, myLength).Trim / 100
            End If
            myStart += myLength
            myLength = 13 'tariffa PV
            If IsNumeric(LineFile.Substring(myStart, myLength).Trim) Then
                myPV.impTariffa = LineFile.Substring(myStart, myLength).Trim / 100000
            End If
            myStart += myLength
            myLength = 10 'importo PV
            If IsNumeric(LineFile.Substring(myStart, myLength).Trim) Then
                myPV.impRuolo = LineFile.Substring(myStart, myLength).Trim / 100
                myPV.impNetto = LineFile.Substring(myStart, myLength).Trim / 100
            End If
            myStart += myLength
            myLength = 10 'totale imposta
            myStart += myLength
            myLength = 3 'cod categoria
            If LineFile.Substring(myStart, myLength).Trim <> "" Then
                myPF.sCategoria = LineFile.Substring(myStart, myLength).Trim
            Else
                myPF.sCategoria = "DOM"
            End If
            myPV.sCategoria = myPF.sCategoria
            myStart += myLength
            myLength = 100 'descrizione categoria
            myPF.sNote = LineFile.Substring(myStart, myLength).Trim
            myStart += myLength
            myLength = 40 'descrizione riduzione
            myRid.sDescrizione = LineFile.Substring(myStart, myLength).Trim
            myStart += myLength
            myLength = 1 'indicazione di riduzione o maggiorazione
            myStart += myLength
            myLength = 5 '% per PF
            myStart += myLength
            myLength = 5 '% per PV
            myStart += myLength
            myLength = 1 'identificativo raggruppamento
            myStart += myLength
            myLength = 1 'applicazione calcolo
            myRid.sDescrApplicazione = LineFile.Substring(myStart, myLength).Trim '0=PF+PV,1=PF,2=PV
            myStart += myLength
            myLength = 4 'anno
            myRid.sAnno = LineFile.Substring(myStart, myLength).Trim
            myStart += myLength
            myLength = 10 'codici
            myRid.sCodice = LineFile.Substring(myStart, myLength).Trim
            myStart += myLength
            myLength = 10 'importo
            Dim impRid As Double = 0
            If IsNumeric(LineFile.Substring(myStart, myLength).Trim) Then
                impRid = LineFile.Substring(myStart, myLength).Trim / 100
                If myRid.sDescrApplicazione = "2" Then
                    myPV.impRiduzione = Math.Abs(impRid)
                    myPV.impNetto += impRid
                Else
                    myPF.impRiduzione = Math.Abs(impRid)
                    myPF.impNetto += impRid
                End If
                myRid.sValore = LineFile.Substring(myStart, myLength).Trim / 100
            End If
            myStart += myLength
            myLength = 1 'sottocategoria NonDom
            myStart += myLength
            myLength = 12 'filler
            If IsNumeric(LineFile.Substring(myStart, myLength).Trim) Then
                If StringOperation.FormatInt(LineFile.Substring(myStart, myLength).Trim) > 0 Then
                    Dim myPM As New ObjArticolo
                    myPM.TipoPartita = ObjArticolo.PARTEMAGGIORAZIONE
                    myPM.IdEnte = myPF.IdEnte
                    myPM.sAnno = myPF.sAnno
                    myPM.nMQ = myPF.nMQ
                    myPM.nBimestri = myPF.nBimestri
                    myPM.impRuolo = LineFile.Substring(myStart, myLength).Trim / 100
                    myPM.impNetto = LineFile.Substring(myStart, myLength).Trim / 100
                    myPM.sCategoria = "MAGST"
                    myPM.ListPFvsPV = (New ArrayList).ToArray(GetType(ObjLegamePFPV))
                    ListItem.Add(myPM)
                End If
            End If
            If impRid <> 0 Then
                myRid.ID = GetIdRid(myRid, myStringConnection)
                ListRid.Add(myRid)
            End If
            myPF.oRiduzioni = ListRid.ToArray(GetType(ObjRidEseApplicati))
            myPF.ListPFvsPV = (New ArrayList).ToArray(GetType(ObjLegamePFPV))
            myPV.ListPFvsPV = (New ArrayList).ToArray(GetType(ObjLegamePFPV))
            ListItem.Add(myPF)
            ListItem.Add(myPV)
        Catch ex As Exception
            Log.Debug(IdEnte + " - OPENgovTIA.ClsImportRuolo.LoadPartitaContExt.errore: ", ex)
            myPF = Nothing
        End Try
        Return ListItem
    End Function
    Private Function GetRid(IdEnte As String, StringConnection As String) As ObjCodDescr()
        Dim myList As New ArrayList
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim DrReturn As SqlClient.SqlDataReader = Nothing

        Try
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(StringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_GetIdRiduzioneImport"
            'VALORIZZO I PARAMETERS:
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = IdEnte
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            DrReturn = cmdMyCommand.ExecuteReader
            Do While DrReturn.Read
                Dim myCodDescr As New ObjCodDescr
                myCodDescr.Id = StringOperation.FormatInt(DrReturn("id"))
                myCodDescr.IdEnte = StringOperation.FormatString(DrReturn("idente"))
                myCodDescr.sCodice = StringOperation.FormatString(DrReturn("codice"))
                myCodDescr.sDescrizione = StringOperation.FormatString(DrReturn("descrizione"))
                myList.Add(myCodDescr)
            Loop
        Catch ex As Exception
            Log.Debug(IdEnte + " - OPENgovTIA.ClsImportRuolo.GetRid.errore: ", ex)
        Finally
            DrReturn.Close()
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
        Return myList.ToArray(GetType(ObjCodDescr))
    End Function
    Private Function GetIdRid(myRid As ObjRidEseApplicati, myStringConnection As String) As Integer
        Dim myIdRid As Integer = -1
        Try
            For Each myItem As ObjCodDescr In ListRid
                If myItem.sCodice = myRid.sCodice Then
                    myIdRid = myItem.Id
                    Exit For
                End If
            Next
            If myIdRid <= 0 Then
                Dim cmdMyCommand As New SqlClient.SqlCommand
                Try
                    cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
                    cmdMyCommand.Connection.Open()
                    cmdMyCommand.CommandTimeout = 0
                    cmdMyCommand.CommandType = CommandType.StoredProcedure

                    cmdMyCommand.Parameters.Clear()
                    cmdMyCommand.Parameters.AddWithValue("@IdRidEse", -1)
                    cmdMyCommand.Parameters.AddWithValue("@Codice", myRid.sCodice)
                    cmdMyCommand.Parameters.AddWithValue("@IDENTE", myRid.IdEnte)
                    cmdMyCommand.Parameters.AddWithValue("@Definizione", myRid.sDescrizione)
                    cmdMyCommand.Parameters("@IdRidEse").Direction = ParameterDirection.InputOutput
                    cmdMyCommand.CommandText = "prc_IMPORTRID_IU"
                    Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                    cmdMyCommand.ExecuteNonQuery()
                    myIdRid = cmdMyCommand.Parameters("@IdRidEse").Value
                Catch err As Exception
                    Log.Debug(myRid.IdEnte + " - OPENgovTIA.ClsImportRuolo.GetRid.SaveRid.errore: ", err)
                Finally
                    cmdMyCommand.Dispose()
                    cmdMyCommand.Connection.Close()
                End Try
                ListRid = GetRid(myRid.IdEnte, myStringConnection)
                myIdRid = GetIdRid(myRid, myStringConnection)
            End If
        Catch ex As Exception
            Log.Debug(myRid.IdEnte + " - OPENgovTIA.ClsImportRuolo.GetRid.errore: ", ex)
        End Try
        Return myIdRid
    End Function
    Private Function GetComune(myBelfiore As String) As String
        Dim myComune As String = ""
        Try
            If myBelfiore <> "" Then
                For Each myItem As OggettiComuniStrade.OggettoEnte In ListComuni
                    If myItem.CodBelfiore = myBelfiore Then
                        myComune = myItem.Denominazione
                        Exit For
                    End If
                Next
            End If
        Catch ex As Exception
            Log.Debug(myBelfiore + " - OPENgovTIA.ClsImportRuolo.GetComune.errore: ", ex)
        End Try
        Return myComune
    End Function
    Private Function GetProvincia(myBelfiore As String) As String
        Dim myComune As String = ""
        Try
            If myBelfiore <> "" Then
                For Each myItem As OggettiComuniStrade.OggettoEnte In ListComuni
                    If myItem.CodBelfiore = myBelfiore Then
                        myComune = myItem.Provincia
                        Exit For
                    End If
                Next
            End If
        Catch ex As Exception
            Log.Debug(myBelfiore + " - OPENgovTIA.ClsImportRuolo.GetProvincia.errore: ", ex)
        End Try
        Return myComune
    End Function
    Private Function CompleteAvviso(myAvviso As ObjAvviso, ListPartite As ArrayList) As ObjAvviso
        Dim myItem As New ObjAvviso
        Try
            myItem = myAvviso
            If Not myItem.oArticoliPrec Is Nothing Then
                For Each myPrev As ObjArticolo In myItem.oArticoliPrec
                    Try
                        myPrev.IdEnte = myItem.IdEnte
                        myPrev.sAnno = myItem.sAnnoRiferimento
                        myPrev.sCategoria = ListPartite.ToArray(GetType(ObjArticolo))(0).sCategoria
                    Catch err As Exception
                        Log.Debug(myItem.IdEnte + " - OPENgovTIA.ClsImportRuolo.CompleteAvviso.ValCatPrevArt.errore: ", err)
                    End Try
                    ListPartite.Add(myPrev)
                Next
            End If
            myItem.oArticoli = ListPartite.ToArray(GetType(ObjArticolo))
            For Each myVoce As ObjDetVoci In myItem.oDetVoci
                If myVoce.sCapitolo = ObjDetVoci.Capitolo.Imposta Then
                    If myItem.oArticoli(0).sCategoria = "DOM" Then
                        myVoce.CodVoce = ObjDetVoci.Voce.Domestica
                    Else
                        myVoce.CodVoce = ObjDetVoci.Voce.NonDomestica
                    End If
                End If
            Next
        Catch ex As Exception
            Log.Debug(myItem.IdEnte + " - OPENgovTIA.ClsImportRuolo.CompleteAvviso.errore: ", ex)
            myItem = Nothing
        End Try
        Return myItem
    End Function
End Class
#End Region
