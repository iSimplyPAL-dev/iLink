Imports System.IO
Imports System.Threading
Imports AnagInterface
Imports ComPlusInterface
Imports ICSharpCode.SharpZipLib.Zip
Imports log4net
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
Imports Utility

''' <summary>
''' Classe per la generazione del ruolo coattivo.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class clsCoattivo
    Private Shared Log As ILog = LogManager.GetLogger(GetType(clsCoattivo))
    Private _DBType As String
    Private _StringConnection As String
    Private _IdEnte As String
    Private _Operatore As String
    Private _Anno As String
    Private _ListTributi As String
    Private _Dal As Date
    Private _Al As Date
    Private _Provenienza As String
    Private _cmdMyCommand As SqlClient.SqlCommand
    Private _cmdMyCommandNoTrans As SqlClient.SqlCommand
    Private _myTrans As SqlClient.SqlTransaction
    Private _CalcInteressi As Boolean
    Private _CalcSpese As Boolean
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="TypeDB"></param>
    ''' <param name="myStringConnection"></param>
    ''' <param name="IdEnte"></param>
    ''' <param name="Operatore"></param>
    ''' <param name="Anno"></param>
    ''' <param name="Tributi"></param>
    ''' <param name="Dal"></param>
    ''' <param name="Al"></param>
    ''' <param name="Provenienza"></param>
    ''' <param name="Interessi"></param>
    ''' <param name="Spese"></param>
    Public Sub StartCoattivo(TypeDB As String, ByVal myStringConnection As String, ByVal IdEnte As String, Operatore As String, Anno As String, Tributi As String, Dal As Date, Al As Date, Provenienza As String, Interessi As Boolean, Spese As Boolean)
        Dim threadDelegate As ThreadStart = New ThreadStart(AddressOf StartCoattivoThreadEntryPoint)
        Dim t As Thread = New Thread(threadDelegate)
        _DBType = TypeDB
        _StringConnection = myStringConnection
        _IdEnte = IdEnte
        _Operatore = Operatore
        _Anno = Anno
        _ListTributi = Tributi
        _Dal = Dal
        _Al = Al
        _Provenienza = Provenienza
        _CalcInteressi = Interessi
        _CalcSpese = Spese

        ' Inizio la transazione e recupero l'istanza della connessione al db
        ' Devo farlo qui visto che sto eseguendo codice ancora nel thread della PostBack da FE, per cui ho il HttpContext disponibile con tutti i suoi oggetti (DichiarazioneSession, HttpSession, ecc.), mentre nel thread non ho queste informazioni
        _cmdMyCommand = StartCalcoloCoattivoTransaction()
        _cmdMyCommandNoTrans = OpenCalcoloCoattivoConnection()
        _myTrans = _cmdMyCommand.Transaction
        t.Start()
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub StartCoattivoThreadEntryPoint()
        CacheManager.SetCalcoloCoattivoInCorso(_ListTributi)
        Dim MyError As Boolean = False
        Dim ListAtti() As ObjCoattivo
        Dim nAvanzamento As Integer = 0

        Try
            Log.Debug("StartCoattivoThreadEntryPoint::inizio")
            ListAtti = GetAttiInCoattivo(_StringConnection, _IdEnte, _Anno, _ListTributi, _Dal, _Al, _Provenienza)
            If Not IsNothing(ListAtti) Then
                nAvanzamento = 0
                For Each myItem As ObjCoattivo In ListAtti
                    nAvanzamento += 1
                    CacheManager.SetAvanzamentoCalcoloCoattivo("Calcolo posizione " & nAvanzamento & " su " & ListAtti.GetUpperBound(0) + 1)
                    If _CalcInteressi Or _CalcSpese Then
                        If _CalcInteressi Then
                            If CalcolaInteressi(myItem) = False Then
                                MyError = True
                                Exit For
                            End If
                        End If
                        If _CalcSpese And myItem.SpeseCoattivo <= 0 Then
                            If CalcolaSpese(myItem) = False Then
                                MyError = True
                                Exit For
                            End If
                        End If
                    End If
                    myItem.ArrotondamentoCoattivo = 0 'FormatNumber(myItem.ImportoCoattivo + myItem.InteressiCoattivo, 0) - (myItem.ImportoCoattivo + myItem.InteressiCoattivo)
                    myItem.TotaleCoattivo = myItem.ImportoCoattivo + myItem.InteressiCoattivo + myItem.ArrotondamentoCoattivo + myItem.SpeseCoattivo
                Next
                If MyError = False Then
                    Dim myRuolo As New ObjRuolo
                    myRuolo.sEnte = _IdEnte
                    myRuolo.sAnno = _Anno
                    myRuolo.sDescrRuolo = _ListTributi
                    myRuolo.tDataInizioConf = _Dal
                    myRuolo.tDataFineConf = _Al
                    myRuolo.sDescrTipoRuolo = _Provenienza
                    myRuolo.HasMaggiorazione = _CalcInteressi
                    myRuolo.HasConferimenti = _CalcSpese
                    myRuolo.sOperatore = _Operatore
                    myRuolo.tDataCreazione = DateTime.Now
                    myRuolo.tDataOKMinuta = DateTime.MaxValue
                    myRuolo.tDataElabDoc = DateTime.MaxValue
                    myRuolo.IdFlusso = SetRuolo(Utility.Costanti.AZIONE_NEW, myRuolo, _StringConnection, _cmdMyCommand)
                    If myRuolo.IdFlusso > 0 Then
                        nAvanzamento = 0
                        For Each myItem As ObjCoattivo In ListAtti
                            nAvanzamento += 1
                            CacheManager.SetAvanzamentoCalcoloCoattivo("Inserimento posizione " & nAvanzamento & " su " & ListAtti.GetUpperBound(0) + 1)
                            Dim Soglia As Double = 0
                            Soglia = New DBPROVVEDIMENTI.ProvvedimentiDB().GetSogliaMinima(myItem.ANNO, myItem.COD_TRIBUTO, _IdEnte, OggettoAtto.Provvedimento.Coattivo)
                            If myItem.TotaleCoattivo >= Soglia Then
                                myItem.IdFlusso = myRuolo.IdFlusso
                                If SetCoattivo(Utility.Costanti.AZIONE_NEW, myItem, _StringConnection, _cmdMyCommand) = False Then
                                    MyError = True
                                    Exit For
                                End If
                            End If
                        Next
                    Else
                        MyError = True
                    End If
                End If
            Else
                MyError = True
            End If
            If MyError = True Then
                RollbackCalcoloCoattivoTransaction(_myTrans, _cmdMyCommand)
            Else
                CommitCalcoloCoattivoTransaction(_myTrans, _cmdMyCommand)
            End If
        Catch Err As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgovPROVVEDIMENTI.clsCoattivo.StartCoattivoThreadEntryPoint.errore: ", Err)
            RollbackCalcoloCoattivoTransaction(_myTrans, _cmdMyCommand)
        Finally
            CacheManager.RemoveCalcoloCoattivoInCorso()
            CacheManager.RemoveAvanzamentoCalcoloCoattivo()
            Log.Debug("StartCoattivoThreadEntryPoint::fine")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myStringConnection"></param>
    ''' <param name="IdEnte"></param>
    ''' <param name="Anno"></param>
    ''' <param name="Tributi"></param>
    ''' <param name="Dal"></param>
    ''' <param name="Al"></param>
    ''' <param name="Provenienza"></param>
    ''' <returns></returns>
    Private Function GetAttiInCoattivo(ByVal myStringConnection As String, ByVal IdEnte As String, Anno As String, Tributi As String, Dal As Date, Al As Date, Provenienza As String) As ObjCoattivo()
        Dim myList As New ArrayList
        Dim myItem As New ObjCoattivo
        Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
        Dim dtMyDati As New DataTable()
        Dim dtMyRow As DataRow
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim nAvanzamento As Integer = 0

        Try
            cmdMyCommand = New SqlClient.SqlCommand
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0

            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.AddWithValue("@IdEnte", IdEnte)
            cmdMyCommand.Parameters.AddWithValue("@Anno", Anno)
            cmdMyCommand.Parameters.AddWithValue("@Tributi", Tributi)
            cmdMyCommand.Parameters.AddWithValue("@Dal", New OPENgovTIA.generalClass.generalFunction().FormattaData(Dal.ToString(), "A"))
            cmdMyCommand.Parameters.AddWithValue("@Al", New OPENgovTIA.generalClass.generalFunction().FormattaData(Al.ToString(), "A"))
            cmdMyCommand.Parameters.AddWithValue("@Provenienza", Provenienza)
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_GetAttiInCoattivo"
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(dtMyDati)
            For Each dtMyRow In dtMyDati.Rows
                nAvanzamento += 1
                CacheManager.SetAvanzamentoCalcoloCoattivo("Lettura posizione " & nAvanzamento & " su " & dtMyDati.Rows.Count)
                myItem = New ObjCoattivo
                myItem.ID_PROVVEDIMENTO = CInt(dtMyRow("id_provvedimento"))
                myItem.COD_CONTRIBUENTE = CInt(dtMyRow("cod_contribuente"))
                myItem.ANNO = CStr(dtMyRow("anno"))
                myItem.COD_TRIBUTO = CStr(dtMyRow("cod_tributo"))
                myItem.ImportoCoattivo = CDbl(dtMyRow("importocoattivo"))
                myItem.DATA_NOTIFICA_AVVISO = CStr(dtMyRow("data_notifica_avviso"))
                myItem.DataInserimento = DateTime.Now
                myList.Add(myItem)
            Next
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgovPROVVEDIMENTI.clsCoattivo.GetAttiInCoattivo.errore: ", ex)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myList = Nothing
        Finally
            dtMyDati.Dispose()
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
        Return CType(myList.ToArray(GetType(ObjCoattivo)), ObjCoattivo())
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myAtto"></param>
    ''' <returns></returns>
    ''' <revisionHistory><revision date="12/11/2019">il calcolo interessi 8852/TASI deve essere fatto in acconto/saldo o in unica soluzione in base alla configurazione di TP_GENERALE_ICI</revision></revisionHistory>
    Private Function CalcolaInteressi(ByRef myAtto As ObjCoattivo) As Boolean
        Dim myRet As Boolean = False
        Dim ListBaseCalc As New ArrayList
        Try
            Dim objCOMDichiarazioniAccertamenti As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
            Dim myItem As New ObjBaseIntSanz
            myItem.Anno = myAtto.ANNO
            myItem.COD_TIPO_PROVVEDIMENTO = OggettoAtto.Provvedimento.Coattivo
            myItem.DifferenzaImposta = myAtto.ImportoCoattivo
            myItem.IdContribuente = myAtto.COD_CONTRIBUENTE
            myItem.IdEnte = _IdEnte
            ListBaseCalc.Add(myItem)

            objCOMDichiarazioniAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
            myAtto.ListInteressi = objCOMDichiarazioniAccertamenti.getInteressi(_IdEnte, myAtto.COD_TRIBUTO, "0", OggettoAtto.Provvedimento.Coattivo, OggettoAtto.Procedimento.Liquidazione, OggettoAtto.Fase.VersamentiTardivi, myAtto.DataInserimento, "", myAtto.DATA_NOTIFICA_AVVISO, 0, CType(ListBaseCalc.ToArray(GetType(ObjBaseIntSanz)), ObjBaseIntSanz()), _StringConnection)
            If myAtto.ListInteressi Is Nothing Then
                myRet = False
            Else
                For Each myInt As ObjInteressiSanzioni In myAtto.ListInteressi
                    myAtto.InteressiCoattivo += myInt.IMPORTO_GIORNI
                Next
                myRet = True
            End If
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgovPROVVEDIMENTI.clsCoattivo.CalcolaInteressi.errore: ", ex)
            myRet = False
        End Try
        Return myRet
    End Function
    'Private Function CalcolaInteressi(ByRef myAtto As ObjCoattivo) As Boolean
    '    Dim myRet As Boolean = False
    '    Dim ListBaseCalc As New ArrayList
    '    Try
    '        Dim objCOMDichiarazioniAccertamenti As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
    '        Dim myItem As New ObjBaseIntSanz
    '        myItem.Anno = myAtto.ANNO
    '        myItem.COD_TIPO_PROVVEDIMENTO = OggettoAtto.Provvedimento.Coattivo
    '        myItem.DifferenzaImposta = myAtto.ImportoCoattivo
    '        myItem.IdContribuente = myAtto.COD_CONTRIBUENTE
    '        myItem.IdEnte = _IdEnte
    '        ListBaseCalc.Add(myItem)

    '        objCOMDichiarazioniAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
    '        myAtto.ListInteressi = objCOMDichiarazioniAccertamenti.getInteressi(_IdEnte, myAtto.COD_TRIBUTO, "0", OggettoAtto.Provvedimento.Coattivo, OggettoAtto.Procedimento.Liquidazione, OggettoAtto.Fase.VersamentiTardivi, myAtto.DataInserimento, myAtto.DATA_NOTIFICA_AVVISO, 0, CType(ListBaseCalc.ToArray(GetType(ObjBaseIntSanz)), ObjBaseIntSanz()), _StringConnection)
    '        If myAtto.ListInteressi Is Nothing Then
    '            myRet = False
    '        Else
    '            For Each myInt As ObjInteressiSanzioni In myAtto.ListInteressi
    '                myAtto.InteressiCoattivo += myInt.IMPORTO_GIORNI
    '            Next
    '            myRet = True
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(_IdEnte  +"."+ _operatore + " - OPENgovPROVVEDIMENTI.clsCoattivo.CalcolaInteressi.errore: ", ex)
    '        myRet = False
    '    End Try
    '    Return myRet
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myAtto"></param>
    ''' <returns></returns>
    Private Function CalcolaSpese(ByRef myAtto As ObjCoattivo) As Boolean
        Dim myRet As Boolean = False
        Dim Spese As Double
        Try
            Spese = New DBPROVVEDIMENTI.ProvvedimentiDB().GetSpese(myAtto.ANNO, myAtto.COD_TRIBUTO, _IdEnte, OggettoAtto.Provvedimento.Coattivo)
            myAtto.SpeseCoattivo = Spese
            myRet = True
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgovPROVVEDIMENTI.clsCoattivo.CalcolaSpese.errore: ", ex)
            myRet = False
        End Try
        Return myRet
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="IdEnte"></param>
    ''' <param name="History"></param>
    ''' <param name="myStringConnection"></param>
    ''' <returns></returns>
    Public Function GetRuolo(ByVal IdEnte As String, History As Integer, myStringConnection As String) As ObjRuolo()
        Dim sSQL As String
        Dim myRuolo As New ObjRuolo
        Dim ListRuoli As New ArrayList
        Dim myDataView As New DataView

        Try
            Using ctx As New DBModel(ConstSession.DBType, myStringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetRuoliCoattivi", "IDENTE", "History")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", IdEnte) _
                            , ctx.GetParam("History", History)
                        )
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsCoattivo.GetRuolo.erroreQuery: ", ex)
                    Return Nothing
                Finally
                    ctx.Dispose()
                End Try
                For Each myRow As DataRowView In myDataView
                    myRuolo = New ObjRuolo
                    myRuolo.IdFlusso = StringOperation.FormatInt(myRow("idflusso"))
                    myRuolo.sEnte = StringOperation.FormatString(myRow("idente"))
                    myRuolo.sAnno = StringOperation.FormatString(myRow("anno"))
                    myRuolo.sDescrRuolo = StringOperation.FormatString(myRow("listtributi"))
                    myRuolo.sDescrTipoRuolo = StringOperation.FormatString(myRow("provenienza"))
                    myRuolo.sTipoCalcolo = StringOperation.FormatString(myRow("descranno"))
                    myRuolo.sNomeRuolo = StringOperation.FormatString(myRow("descrlisttributi"))
                    myRuolo.sTipoRuolo = StringOperation.FormatString(myRow("descrprovenienza"))
                    myRuolo.tDataInizioConf = StringOperation.FormatDateTime(myRow("inizio_notifica"))
                    myRuolo.tDataFineConf = StringOperation.FormatDateTime(myRow("fine_notifica"))
                    myRuolo.HasMaggiorazione = StringOperation.FormatBool(myRow("hasinteressi"))
                    myRuolo.HasConferimenti = StringOperation.FormatBool(myRow("hasspese"))
                    myRuolo.tDataCreazione = StringOperation.FormatDateTime(myRow("data_creazione"))
                    myRuolo.tDataOKMinuta = StringOperation.FormatDateTime(myRow("data_approvazione"))
                    myRuolo.tDataElabDoc = StringOperation.FormatDateTime(myRow("data_estrazione"))
                    myRuolo.sOperatore = StringOperation.FormatString(myRow("operatore"))
                    myRuolo.nAvvisi = StringOperation.FormatInt(myRow("navvisi"))
                    myRuolo.ImpAvvisi = StringOperation.FormatDouble(myRow("impavvisi"))
                    ListRuoli.Add(myRuolo)
                Next
            End Using
            Return CType(ListRuoli.ToArray(GetType(ObjRuolo)), ObjRuolo())
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsCoattivo.GetRuolo.errore: ", Err)
            Return Nothing
        End Try
    End Function
    'Public Function GetRuolo(ByVal IdEnte As String, History As Integer, myStringConnection As String) As ObjRuolo()
    '    Dim myRuolo As New ObjRuolo
    '    Dim ListRuoli As New ArrayList
    '    Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
    '    Dim dtMyDati As New DataTable()
    '    Dim dtMyRow As DataRow
    '    Dim myAdapter As New SqlClient.SqlDataAdapter

    '    Try
    '        cmdMyCommand = New SqlClient.SqlCommand
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0

    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.AddWithValue("@IdEnte", IdEnte)
    '        cmdMyCommand.Parameters.AddWithValue("@History", History)
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = "prc_GetRuoliCoattivi"
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(dtMyDati)
    '        For Each dtMyRow In dtMyDati.Rows
    '            myRuolo = New ObjRuolo
    '            myRuolo.IdFlusso = CInt(dtMyRow("idflusso"))
    '            myRuolo.sEnte = CStr(dtMyRow("idente"))
    '            myRuolo.sAnno = CStr(dtMyRow("anno"))
    '            myRuolo.sDescrRuolo = CStr(dtMyRow("listtributi"))
    '            myRuolo.sDescrTipoRuolo = CStr(dtMyRow("provenienza"))
    '            myRuolo.sTipoCalcolo = CStr(dtMyRow("descranno"))
    '            myRuolo.sNomeRuolo = CStr(dtMyRow("descrlisttributi"))
    '            myRuolo.sTipoRuolo = CStr(dtMyRow("descrprovenienza"))
    '            If Not IsDBNull(dtMyRow("inizio_notifica")) Then
    '                myRuolo.tDataInizioConf = New OPENgovTIA.generalClass.generalFunction().FormattaData(dtMyRow("inizio_notifica").ToString(), "G")
    '            Else
    '                myRuolo.tDataInizioConf = DateTime.MaxValue
    '            End If
    '            If Not IsDBNull(dtMyRow("fine_notifica")) Then
    '                myRuolo.tDataFineConf = New OPENgovTIA.generalClass.generalFunction().FormattaData(dtMyRow("fine_notifica").ToString(), "G")
    '            Else
    '                myRuolo.tDataFineConf = DateTime.MaxValue
    '            End If
    '            myRuolo.HasMaggiorazione = CBool(dtMyRow("hasinteressi"))
    '            myRuolo.HasConferimenti = CBool(dtMyRow("hasspese"))
    '            myRuolo.tDataCreazione = CDate(dtMyRow("data_creazione"))
    '            If Not IsDBNull(dtMyRow("data_approvazione")) Then
    '                myRuolo.tDataOKMinuta = CDate(dtMyRow("data_approvazione"))
    '            Else
    '                myRuolo.tDataOKMinuta = Date.MinValue
    '            End If
    '            If Not IsDBNull(dtMyRow("data_estrazione")) Then
    '                myRuolo.tDataElabDoc = CDate(dtMyRow("data_estrazione"))
    '            Else
    '                myRuolo.tDataElabDoc = DateTime.MinValue
    '            End If
    '            myRuolo.sOperatore = CStr(dtMyRow("operatore"))
    '            myRuolo.nAvvisi = CInt(dtMyRow("navvisi"))
    '            myRuolo.ImpAvvisi = CDbl(dtMyRow("impavvisi"))
    '            ListRuoli.Add(myRuolo)
    '        Next

    '        Return CType(ListRuoli.ToArray(GetType(ObjRuolo)), ObjRuolo())
    '    Catch Err As Exception
    '        Log.Debug(IdEnte + " - OPENgovPROVVEDIMENTI.clsCoattivo.GetRuolo.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return Nothing
    '    Finally
    '        dtMyDati.Dispose()
    '        cmdMyCommand.Dispose()
    '        cmdMyCommand.Connection.Close()
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myStringConnection"></param>
    ''' <param name="IdFlusso"></param>
    ''' <returns></returns>
    Public Function DeleteRuolo(ByVal myStringConnection As String, ByVal IdFlusso As Integer) As Boolean
        Dim cmdMyCommand As SqlClient.SqlCommand = Nothing

        Try
            cmdMyCommand = New SqlClient.SqlCommand
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0

            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.AddWithValue("@IdFlusso", IdFlusso)
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_TBLRUOLICOATTIVI_D"
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            cmdMyCommand.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgovPROVVEDIMENTI.clsCoattivo.DeleteRuolo.errore:  ", ex)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return False
        Finally
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="DBOperation"></param>
    ''' <param name="myRuolo"></param>
    ''' <param name="myStringConnection"></param>
    ''' <param name="cmdMyCommandOut"></param>
    ''' <returns></returns>
    Public Function SetRuolo(ByVal DBOperation As Integer, ByVal myRuolo As ObjRuolo, myStringConnection As String, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As Integer
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim MyProcedure As String = "prc_TBLRUOLICOATTIVO_IU"

        Try
            If (Not (cmdMyCommandOut) Is Nothing) Then
                cmdMyCommand = cmdMyCommandOut
            Else
                cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
                cmdMyCommand.Connection.Open()
            End If
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.Parameters.Clear()
            Select Case DBOperation
                Case Utility.Costanti.AZIONE_NEW, Utility.Costanti.AZIONE_UPDATE
                    cmdMyCommand.Parameters.AddWithValue("@IDFLUSSO", myRuolo.IdFlusso)
                    cmdMyCommand.Parameters.AddWithValue("@IDENTE", myRuolo.sEnte)
                    cmdMyCommand.Parameters.AddWithValue("@ANNO", myRuolo.sAnno)
                    cmdMyCommand.Parameters.AddWithValue("@LISTTRIBUTI", myRuolo.sDescrRuolo)
                    cmdMyCommand.Parameters.AddWithValue("@PROVENIENZA", myRuolo.sDescrTipoRuolo)
                    cmdMyCommand.Parameters.AddWithValue("@INIZIONOTIFICA", New OPENgovTIA.generalClass.generalFunction().FormattaData(myRuolo.tDataInizioConf.ToString(), "A"))
                    cmdMyCommand.Parameters.AddWithValue("@FINENOTIFICA", New OPENgovTIA.generalClass.generalFunction().FormattaData(myRuolo.tDataFineConf.ToString(), "A"))
                    cmdMyCommand.Parameters.AddWithValue("@HASINTERESSI", CInt(myRuolo.HasMaggiorazione))
                    cmdMyCommand.Parameters.AddWithValue("@HASSPESE", CInt(myRuolo.HasConferimenti))
                    cmdMyCommand.Parameters.AddWithValue("@DATA_CREAZIONE", myRuolo.tDataCreazione)
                    cmdMyCommand.Parameters.AddWithValue("@DATA_APPROVAZIONE", myRuolo.tDataOKMinuta)
                    cmdMyCommand.Parameters.AddWithValue("@DATA_ESTRAZIONE", myRuolo.tDataElabDoc)
                    cmdMyCommand.Parameters.AddWithValue("@OPERATORE", myRuolo.sOperatore)
                    MyProcedure = "prc_TBLRUOLICOATTIVI_IU"
                    cmdMyCommand.CommandText = MyProcedure
                    cmdMyCommand.Parameters("@IDFLUSSO").Direction = ParameterDirection.InputOutput
                    Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                    cmdMyCommand.ExecuteNonQuery()
                    myRuolo.IdFlusso = cmdMyCommand.Parameters("@IDFLUSSO").Value
                Case Utility.Costanti.AZIONE_DELETE
                    cmdMyCommand.Parameters.AddWithValue("@IDFLUSSO", myRuolo.IdFlusso)
                    MyProcedure = "prc_TBLRUOLICOATTIVI_D"
                    cmdMyCommand.CommandText = MyProcedure
                    Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                    cmdMyCommand.ExecuteNonQuery()
            End Select

            Return myRuolo.IdFlusso
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsCoattivo.SetRuolo.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return 0
        Finally
            If (cmdMyCommandOut Is Nothing) Then
                cmdMyCommand.Dispose()
                cmdMyCommand.Connection.Close()
            End If
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="DBOperation"></param>
    ''' <param name="myItem"></param>
    ''' <param name="myStringConnection"></param>
    ''' <param name="cmdMyCommandOut"></param>
    ''' <returns></returns>
    Private Function SetCoattivo(ByVal DBOperation As Integer, ByVal myItem As ObjCoattivo, myStringConnection As String, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As Integer
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Try
            If (Not (cmdMyCommandOut) Is Nothing) Then
                cmdMyCommand = cmdMyCommandOut
            Else
                cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
                cmdMyCommand.Connection.Open()
            End If
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.Parameters.Clear()
            Select Case DBOperation
                Case Utility.Costanti.AZIONE_NEW, Utility.Costanti.AZIONE_UPDATE
                    cmdMyCommand.Parameters.AddWithValue("@ID", myItem.Id)
                    cmdMyCommand.Parameters.AddWithValue("@IDFLUSSO", myItem.IdFlusso)
                    cmdMyCommand.Parameters.AddWithValue("@IDPROVVEDIMENTO", myItem.ID_PROVVEDIMENTO)
                    cmdMyCommand.Parameters.AddWithValue("@IMPORTO", myItem.ImportoCoattivo)
                    cmdMyCommand.Parameters.AddWithValue("@INTERESSI", myItem.InteressiCoattivo)
                    cmdMyCommand.Parameters.AddWithValue("@SPESE", myItem.SpeseCoattivo)
                    cmdMyCommand.Parameters.AddWithValue("@ARROTONDAMENTO", myItem.ArrotondamentoCoattivo)
                    cmdMyCommand.Parameters.AddWithValue("@TOTALE", myItem.TotaleCoattivo)
                    cmdMyCommand.Parameters.AddWithValue("@DATAINSERIMENTO", myItem.DataInserimento)
                    cmdMyCommand.Parameters.AddWithValue("@DATAVARIAZIONE", myItem.DataVariazione)
                    cmdMyCommand.CommandText = "prc_TBLCOATTIVO_IU"
                    cmdMyCommand.Parameters("@ID").Direction = ParameterDirection.InputOutput
                    Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                    cmdMyCommand.ExecuteNonQuery()
                    myItem.Id = cmdMyCommand.Parameters("@ID").Value
                    If myItem.Id > 0 Then
                        For Each myInt As ObjInteressiSanzioni In myItem.ListInteressi
                            If myInt.IMPORTO_GIORNI > 0 Then
                                cmdMyCommand.Parameters.Clear()
                                cmdMyCommand.Parameters.AddWithValue("@ID", myInt.ID)
                                cmdMyCommand.Parameters.AddWithValue("@IDCOATTIVO", myItem.Id)
                                cmdMyCommand.Parameters.AddWithValue("@IDTIPOINTERESSE", myInt.COD_VOCE)
                                cmdMyCommand.Parameters.AddWithValue("@DATAINIZIO", myInt.DATA_INIZIO)
                                cmdMyCommand.Parameters.AddWithValue("@DATAFINE", myInt.DATA_FINE)
                                cmdMyCommand.Parameters.AddWithValue("@GIORNI", myInt.N_GIORNI_TOTALI)
                                cmdMyCommand.Parameters.AddWithValue("@IMPORTO", myInt.IMPORTO_GIORNI)
                                cmdMyCommand.CommandText = "prc_TBLCOATTIVOINTERESSI_IU"
                                cmdMyCommand.Parameters("@ID").Direction = ParameterDirection.InputOutput
                                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                                cmdMyCommand.ExecuteNonQuery()
                                myInt.ID = cmdMyCommand.Parameters("@ID").Value
                                If myInt.ID < 0 Then
                                    Throw New Exception("Errore in inserimento interessi")
                                End If
                            End If
                        Next
                    End If
                Case Utility.Costanti.AZIONE_DELETE
                    cmdMyCommand.Parameters.AddWithValue("@ID", myItem.Id)
                    cmdMyCommand.CommandText = "prc_TBLCOATTIVO_D"
                    Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                    cmdMyCommand.ExecuteNonQuery()
            End Select

            Return myItem.Id
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsCoattivo.SetCoattivo.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return 0
        Finally
            If (cmdMyCommandOut Is Nothing) Then
                cmdMyCommand.Dispose()
                cmdMyCommand.Connection.Close()
            End If
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="IdRuolo"></param>
    ''' <param name="myStringConnection"></param>
    ''' <returns></returns>
    Public Function SetDataCoattivo(IdRuolo As Integer, myStringConnection As String) As Integer
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.AddWithValue("@ID", -1)
            cmdMyCommand.Parameters.AddWithValue("@IDFLUSSO", IdRuolo)
            cmdMyCommand.CommandText = "prc_SetDataCoattivo"
            cmdMyCommand.Parameters("@ID").Direction = ParameterDirection.InputOutput
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            cmdMyCommand.ExecuteNonQuery()
            Return cmdMyCommand.Parameters("@ID").Value
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsCoattivo.SetDataCoattivo.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return 0
        Finally
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myStringConnection"></param>
    ''' <param name="IdFlusso"></param>
    ''' <returns></returns>
    Public Function GetListCoattivi(ByVal myStringConnection As String, ByVal IdFlusso As Integer) As ObjCoattivo()
        Dim myList As New ArrayList
        Dim myItem As New ObjCoattivo
        Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
        Dim dtMyDati As New DataTable()
        Dim dtMyRow As DataRow
        Dim myAdapter As New SqlClient.SqlDataAdapter

        Try
            cmdMyCommand = New SqlClient.SqlCommand
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0

            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.AddWithValue("@IdFlusso", IdFlusso)
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_GetListCoattivi"
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(dtMyDati)
            For Each dtMyRow In dtMyDati.Rows
                myItem = New ObjCoattivo
                myItem.Id = CInt(dtMyRow("id"))
                myItem.ID_PROVVEDIMENTO = CInt(dtMyRow("idprovvedimento"))
                myItem.COD_CONTRIBUENTE = CInt(dtMyRow("cod_contribuente"))
                myItem.ANNO = CStr(dtMyRow("anno"))
                myItem.COD_TRIBUTO = CStr(dtMyRow("cod_tributo"))
                myItem.ImportoCoattivo = CDbl(dtMyRow("importo"))
                myItem.DataInserimento = CDate(dtMyRow("data_inserimento"))
                myItem.DATA_ELABORAZIONE = CDate(dtMyRow("data_elaborazione"))
                myItem.DATA_NOTIFICA_AVVISO = CDate(dtMyRow("data_notifica_avviso"))
                myItem.COGNOME = CStr(dtMyRow("Cognome"))
                myItem.NOME = CStr(dtMyRow("Nome"))
                myItem.CODICE_FISCALE = CStr(dtMyRow("CODICE_FISCALE"))
                myItem.PARTITA_IVA = CStr(dtMyRow("PARTITA_IVA"))
                myItem.NUMERO_ATTO = CStr(dtMyRow("NUMERO_ATTO"))
                myItem.DescrTributo = CStr(dtMyRow("DESCRTRIBUTO"))
                myItem.IMPORTO_SANZIONI = CDbl(dtMyRow("IMPORTO_SANZIONI"))
                myItem.InteressiCoattivo = CDbl(dtMyRow("Interessi"))
                myItem.SpeseCoattivo = CDbl(dtMyRow("Spese"))
                myItem.TotaleCoattivo = CDbl(dtMyRow("Totale"))
                myList.Add(myItem)
            Next
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgovPROVVEDIMENTI.clsCoattivo.GetListCoattivi.errore:  ", ex)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myList = Nothing
        Finally
            dtMyDati.Dispose()
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
        Return CType(myList.ToArray(GetType(ObjCoattivo)), ObjCoattivo())
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myStringConnection"></param>
    ''' <param name="Id"></param>
    ''' <returns></returns>
    Public Function DeleteCoattivo(ByVal myStringConnection As String, ByVal Id As Integer) As Boolean
        Dim cmdMyCommand As SqlClient.SqlCommand = Nothing

        Try
            cmdMyCommand = New SqlClient.SqlCommand
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0

            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.AddWithValue("@Id", Id)
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_TBLCOATTIVO_D"
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            cmdMyCommand.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            Log.Debug(_IdEnte + "." + _Operatore + " - OPENgovPROVVEDIMENTI.clsCoattivo.DeleteCoattivo.errore:  ", ex)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return False
        Finally
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function
#Region "Transaction"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Function StartCalcoloCoattivoTransaction() As SqlClient.SqlCommand
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
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Function OpenCalcoloCoattivoConnection() As SqlClient.SqlCommand
        Dim cmdMyCommand As New SqlClient.SqlCommand
        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
        cmdMyCommand.Connection.Open()
        cmdMyCommand.CommandTimeout = 0
        Return cmdMyCommand
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myTrans"></param>
    ''' <param name="cmdMyCommand"></param>
    Public Sub CommitCalcoloCoattivoTransaction(ByRef myTrans As SqlClient.SqlTransaction, ByRef cmdMyCommand As SqlClient.SqlCommand)
        myTrans.Commit()
        cmdMyCommand.Dispose()
        cmdMyCommand.Connection.Close()
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myTrans"></param>
    ''' <param name="cmdMyCommand"></param>
    Public Sub RollbackCalcoloCoattivoTransaction(ByRef myTrans As SqlClient.SqlTransaction, ByVal cmdMyCommand As SqlClient.SqlCommand)
        myTrans.Rollback()
        cmdMyCommand.Dispose()
        cmdMyCommand.Connection.Close()
    End Sub
#End Region
#Region "Stampa"
    ''' <summary>
    ''' 
    ''' </summary>
    Public Class clsStampa
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="ListItem"></param>
        ''' <param name="sIntestazioneEnte"></param>
        ''' <param name="nCampi"></param>
        ''' <param name="Dal"></param>
        ''' <param name="Al"></param>
        ''' <returns></returns>
        Public Function PrintMinuta(ByVal ListItem() As ObjCoattivo, ByVal sIntestazioneEnte As String, nCampi As Integer, Dal As Date, Al As Date) As DataTable
            Dim sDatiStampa As String
            Dim DsStampa As New DataSet
            Dim DtStampa As New DataTable
            Dim x As Integer
            Dim nPosizioni As Integer = 0
            Dim impImposta, impSanzioni, impInteressi, impSpese, impTotale As Double

            Try
                impImposta = 0 : impSanzioni = 0 : impInteressi = 0 : impSpese = 0 : impTotale = 0
                'carico il dataset
                DsStampa.Tables.Add("STAMPA")
                'carico le colonne nel dataset
                For x = 0 To nCampi + 1
                    DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
                Next
                'carico il datatable
                DtStampa = DsStampa.Tables("STAMPA")
                'inserisco l'intestazione dell'ente
                sDatiStampa = sIntestazioneEnte
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                'inserisco una riga vuota
                sDatiStampa = ""
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                'inserisco l'intestazione del report
                sDatiStampa = "Minuta Coattivi Notificati dal " + New Formatta.FunctionGrd().FormattaDataGrd(Dal) + " al " + New Formatta.FunctionGrd().FormattaDataGrd(Al)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                'inserisco una riga vuota
                sDatiStampa = ""
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                'inserisco le intestazioni di colonna
                sDatiStampa = ""
                sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.Iva"
                sDatiStampa += "|N.Atto|Descrizione|Data Emissione Coattivo|Data Notifica"
                sDatiStampa += "|Imposta|Sanzioni|Interessi|Spese|Totale"
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                'ciclo sui dati da stampare
                For Each myItem As ObjCoattivo In ListItem
                    sDatiStampa = ""
                    nPosizioni += 1
                    sDatiStampa += myItem.COGNOME
                    sDatiStampa += "|" + myItem.NOME
                    sDatiStampa += "|'" + New Formatta.FunctionGrd().FormattaCFPIVA(myItem.CODICE_FISCALE, myItem.PARTITA_IVA)
                    sDatiStampa += "|" + myItem.NUMERO_ATTO
                    sDatiStampa += "|" + myItem.DescrTributo
                    sDatiStampa += "|" + New Formatta.FunctionGrd().FormattaDataGrd(myItem.DataInserimento)
                    sDatiStampa += "|" + myItem.DATA_NOTIFICA_AVVISO
                    sDatiStampa += "|" + FormatNumber(myItem.ImportoCoattivo, 2)
                    sDatiStampa += "|" + FormatNumber(myItem.IMPORTO_SANZIONI, 2)
                    sDatiStampa += "|" + FormatNumber(myItem.InteressiCoattivo, 2)
                    sDatiStampa += "|" + FormatNumber(myItem.SpeseCoattivo, 2)
                    sDatiStampa += "|" + FormatNumber(myItem.TotaleCoattivo, 2)
                    impImposta += myItem.ImportoCoattivo
                    impSanzioni += myItem.IMPORTO_SANZIONI
                    impInteressi += myItem.InteressiCoattivo
                    impSpese += myItem.SpeseCoattivo
                    impTotale += myItem.TotaleCoattivo
                    If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                        Return Nothing
                    End If
                Next
                'inserisco una riga vuota
                sDatiStampa = ""
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                'inserisco i totalizzatori
                sDatiStampa = "Tot.Atti |" & nPosizioni.ToString
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                sDatiStampa = "Tot.Imposta |" & FormatNumber(impImposta, 2)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                sDatiStampa = "Tot.Sanzioni |" & FormatNumber(impSanzioni, 2)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                sDatiStampa = "Tot.Interessi |" & FormatNumber(impInteressi, 2)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                sDatiStampa = "Tot.Spese |" & FormatNumber(impSpese, 2)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                sDatiStampa = "Totale |" & FormatNumber(impTotale, 2)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                Return DtStampa
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsCoattivo.clsStampa.PrintMinuta.errore: ", Err)
                Return Nothing
            End Try
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="DtAddRow"></param>
        ''' <param name="sValueRow"></param>
        ''' <returns></returns>
        Private Function AddRowStampa(ByRef DtAddRow As DataTable, ByVal sValueRow As String) As Integer
            Dim sTextRow() As String
            Dim DrAddRow As DataRow
            Dim x As Integer = 0

            Try
                'aggiungo una nuova riga nel datarow
                DrAddRow = DtAddRow.NewRow
                'controllo se la riga e\' scritta
                If sValueRow <> "" Then
                    sTextRow = sValueRow.Split(CChar("|"))
                    For x = 0 To UBound(sTextRow)
                        'popolo la riga nel datarow
                        DrAddRow(x) = sTextRow(x)
                    Next
                End If
                'aggiorno la riga al datatable
                DtAddRow.Rows.Add(DrAddRow)

                Return 1
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsCoattivo.clsStampa.AddRowStampa.errore: ", Err)
                Return 0
            End Try
        End Function
    End Class
#End Region
#Region "290"
    ''' <summary>
    ''' 
    ''' </summary>
    Public Class cls290
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="myStringConnection"></param>
        ''' <param name="IdEnteCNC"></param>
        ''' <param name="CoattivoDal"></param>
        ''' <param name="CoattivoAl"></param>
        ''' <param name="IdRuolo"></param>
        ''' <param name="sIdEnteCredBen"></param>
        ''' <param name="nRate"></param>
        ''' <param name="sPathFile"></param>
        ''' <param name="sNameFile"></param>
        ''' <param name="ErrAnag"></param>
        ''' <param name="ListFile"></param>
        ''' <returns></returns>
        Public Function Crea290(ByVal myStringConnection As String, ByVal IdEnteCNC As String, CoattivoDal As String, CoattivoAl As String, IdRuolo As Integer, ByVal sIdEnteCredBen As String, ByVal nRate As Integer, sPathFile As String, ByVal sNameFile As String, ByRef ErrAnag As String, ByRef ListFile As ArrayList) As Integer
            Dim oListArticoli() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoArticoloRuolo
            Dim x As Integer
            Dim oMyN0 As New Importer290.N0
            Dim oListN1() As Importer290.N1
            Dim oMyN1 As Importer290.N1
            Dim nListN1 As Integer = -1
            Dim oMyN2 As Importer290.N2
            Dim nListN2 As Integer = -1
            Dim oListN4() As Importer290.N4
            Dim oMyN4 As Importer290.N4
            Dim nListN4 As Integer = -1
            Dim oMyN5 As Importer290.N5
            Dim oListN5() As Importer290.N5
            Dim nListN5 As Integer = -1
            Dim oMyN9 As Importer290.N9
            Dim nTotN0 As Integer = 0
            Dim nTotN1 As Integer = 0
            Dim nTotN2 As Integer = 0
            Dim nTotN4 As Integer = 0
            Dim nTotN5 As Integer = 0
            Dim nTotN9 As Integer = 0
            Dim sIdEntePrec As String = ""
            Dim nIdContribPrec As Integer = -1
            Dim nProgPartita As Integer = 0
            Dim impTot290 As Double = 0
            Dim TipoPrec As String = ""

            Try
                ListFile = New ArrayList
                'prelevo gli articoli da estrarre
                oListArticoli = GetPosizioni290(myStringConnection, IdEnteCNC, CoattivoDal, CoattivoAl, IdRuolo)
                If Not oListArticoli Is Nothing Then
                    'popolo il record N0
                    oMyN0 = PopolaN0(sIdEnteCredBen)
                    If Not oMyN0 Is Nothing Then
                        'incremento il totale dei record
                        nTotN0 += 1
                    Else
                        Return -1
                    End If
                    'ciclo su tutti gli articoli trovati e preparo il 290
                    For x = 0 To oListArticoli.GetUpperBound(0)
                        'i flussi devono essere suddivisi per tributo quindi controllo se devo creare un nuovo flusso
                        If oListArticoli(x).TipoRuolo <> TipoPrec And TipoPrec <> "" Then
                            'incremento il totale dei record
                            nTotN5 += 1
                            nListN5 += 1
                            ReDim Preserve oListN5(nListN5)
                            oMyN5 = New Importer290.N5
                            'popolo il record N5
                            oMyN5 = PopolaN5(IdEnteCNC, nTotN1, nTotN2, 0, nTotN4, nTotN5, impTot290)
                            If Not oMyN5 Is Nothing Then
                                oListN5(nListN5) = oMyN5
                            Else
                                Return -1
                            End If
                            'incremento il totale dei record
                            nTotN9 += 1
                            'popolo il record N9
                            oMyN9 = PopolaN9(sIdEnteCredBen, nTotN0, nTotN1, nTotN2, 0, nTotN4, nTotN5, nTotN9)
                            If oMyN9 Is Nothing Then
                                Return -1
                            End If

                            'scrivo il file
                            If Create290(sPathFile & sNameFile & TipoPrec & ".001", oMyN0, oListN1, oListN4, oListN5, oMyN9) < 1 Then
                                Return -1
                            End If
                            ListFile.Add(sPathFile & sNameFile & TipoPrec & ".001")
                            'svuoto le variabili
                            nTotN0 = 0 : nTotN1 = 0 : nTotN2 = 0 : nTotN4 = 0 : nTotN5 = 0 : nTotN9 = 0
                            nListN1 = -1 : nListN2 = -1 : nListN4 = -1 : nListN5 = -1
                            oMyN0 = New Importer290.N0 : oListN1 = Nothing : oListN4 = Nothing : oListN5 = Nothing : oMyN9 = New Importer290.N9
                            sIdEntePrec = "" : nIdContribPrec = -1 : nProgPartita = 0 : impTot290 = 0
                            'popolo il record N0
                            oMyN0 = PopolaN0(sIdEnteCredBen)
                            If Not oMyN0 Is Nothing Then
                                'incremento il totale dei record
                                nTotN0 += 1
                            Else
                                Return -1
                            End If
                        End If
                        'controllo se devo creare un nuovo N1
                        If oListArticoli(x).Ente <> sIdEntePrec Then
                            'controllo se devo chiudere l'N1 precedente
                            If sIdEntePrec <> "" Then
                                'incremento il totale dei record
                                nTotN5 += 1
                                nListN5 += 1
                                ReDim Preserve oListN5(nListN5)
                                oMyN5 = New Importer290.N5
                                'popolo il record N5
                                oMyN5 = PopolaN5(IdEnteCNC, nTotN1, nTotN2, 0, nTotN4, nTotN5, impTot290)
                                If Not oMyN5 Is Nothing Then
                                    oListN5(nListN5) = oMyN5
                                Else
                                    Return -1
                                End If
                            End If
                            'popolo il record N1
                            nListN1 += 1
                            ReDim Preserve oListN1(nListN1)
                            oMyN1 = New Importer290.N1
                            oMyN1 = PopolaN1(IdEnteCNC, "1", nRate)
                            If Not oMyN1 Is Nothing Then
                                oListN1(nListN1) = oMyN1
                                'incremento il totale dei record
                                nTotN1 += 1
                            Else
                                Return -1
                            End If
                        End If
                        'aggiungo un nuovo N4
                        nListN4 += 1
                        ReDim Preserve oListN4(nListN4)
                        oMyN4 = New Importer290.N4
                        'controllo se devo creare un nuovo N2
                        If oListArticoli(x).IdContribuente <> nIdContribPrec Then
                            nProgPartita += 1
                            oMyN2 = PopolaN2(oListArticoli(x).IdContribuente, oListArticoli(x).DescrDiffImposta, IdEnteCNC, nProgPartita, ErrAnag)
                            If Not oMyN2 Is Nothing Then
                                'incremento il totale dei record
                                nTotN2 += 1
                            Else
                                ErrAnag = oListArticoli(x).COGNOME + " " + oListArticoli(x).NOME
                                Return -2
                            End If
                        End If
                        oMyN4 = PopolaN4(oListArticoli(x), IdEnteCNC, nProgPartita, oListArticoli(x).DescrDiffImposta, oListArticoli(x).ImportoNetto, oMyN2)
                        If Not oMyN4 Is Nothing Then
                            oListN4(nListN4) = oMyN4
                            'incremento il totale dei record
                            nTotN4 += 1
                            impTot290 += oListArticoli(x).ImportoNetto
                        Else
                            Return -1
                        End If

                        nIdContribPrec = oListArticoli(x).IdContribuente
                        sIdEntePrec = oListArticoli(x).Ente
                        TipoPrec = oListArticoli(x).TipoRuolo
                    Next
                    'incremento il totale dei record
                    nTotN5 += 1
                    nListN5 += 1
                    ReDim Preserve oListN5(nListN5)
                    oMyN5 = New Importer290.N5
                    'popolo il record N5
                    oMyN5 = PopolaN5(IdEnteCNC, nTotN1, nTotN2, 0, nTotN4, nTotN5, impTot290)
                    If Not oMyN5 Is Nothing Then
                        oListN5(nListN5) = oMyN5
                    Else
                        Return -1
                    End If
                    'incremento il totale dei record
                    nTotN9 += 1
                    'popolo il record N9
                    oMyN9 = PopolaN9(sIdEnteCredBen, nTotN0, nTotN1, nTotN2, 0, nTotN4, nTotN5, nTotN9)
                    If oMyN9 Is Nothing Then
                        Return -1
                    End If

                    'scrivo il file
                    If Create290(sPathFile & sNameFile & TipoPrec & ".001", oMyN0, oListN1, oListN4, oListN5, oMyN9) < 1 Then
                        Return -1
                    End If
                    ListFile.Add(sPathFile & sNameFile & TipoPrec & ".001")
                    If ZipFile(sPathFile, sNameFile & ".zip", ListFile) = False Then
                        Return 0
                    End If
                Else
                    Return 0
                End If

                Return 1
            Catch Err As Exception
                Log.Debug(IdEnteCNC + " - OPENgovPROVVEDIMENTI.clsCoattivo.Crea290.errore::" & Err.Message)
                Return -1
            End Try
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="myStringConnection"></param>
        ''' <param name="IdEnte"></param>
        ''' <param name="CoattivoDal"></param>
        ''' <param name="CoattivoAl"></param>
        ''' <param name="IdRuolo"></param>
        ''' <returns></returns>
        Public Function GetPosizioni290(ByVal myStringConnection As String, IdEnte As String, CoattivoDal As String, CoattivoAl As String, IdRuolo As Integer) As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoArticoloRuolo()
            Dim oMyAvviso As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoArticoloRuolo
            Dim oListAvvisi As New ArrayList
            Dim oReplace As New OPENgovTIA.generalClass.generalFunction
            Dim sSQL As String
            Dim myDataView As New DataView

            Try
                Using ctx As New DBModel(ConstSession.DBType, myStringConnection)
                    Try
                        sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetPosizioni290", "IDENTE", "CoattivoDal", "CoattivoAl", "IdRuolo")
                        myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", IdEnte) _
                            , ctx.GetParam("CoattivoDal", oReplace.FormattaData(CoattivoDal, "A")) _
                            , ctx.GetParam("CoattivoAl", oReplace.FormattaData(CoattivoAl, "A")) _
                            , ctx.GetParam("IdRuolo", IdRuolo)
                        )
                    Catch ex As Exception
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsCoattivo.GetPosizioni290.erroreQuery: ", ex)
                        Return Nothing
                    Finally
                        ctx.Dispose()
                    End Try
                    For Each myRow As DataRowView In myDataView
                        oMyAvviso = New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoArticoloRuolo
                        oMyAvviso.Id = StringOperation.FormatInt(myRow("id"))
                        oMyAvviso.IdContribuente = StringOperation.FormatInt(myRow("idcontribuente"))
                        oMyAvviso.Ente = StringOperation.FormatString(myRow("idente"))
                        oMyAvviso.Anno = StringOperation.FormatString(myRow("anno"))
                        oMyAvviso.DescrDiffImposta = StringOperation.FormatString(myRow("cod_tributo"))
                        oMyAvviso.DataInizio = StringOperation.FormatDateTime(myRow("datainizio"))
                        oMyAvviso.ImportoNetto = StringOperation.FormatDouble(myRow("IMPORTO_DIFFERENZA_IMPOSTA"))
                        oMyAvviso.InformazioniCartella = StringOperation.FormatString(myRow("informazioni"))
                        oMyAvviso.TipoRuolo = StringOperation.FormatString(myRow("nfile"))
                        oListAvvisi.Add(oMyAvviso)
                    Next
                End Using
                Return CType(oListAvvisi.ToArray(GetType(RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoArticoloRuolo)), RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoArticoloRuolo())
            Catch Err As Exception
                Log.Debug(IdEnte + " - OPENgovPROVVEDIMENTI.clsCoattivo.GetPosizioni290.errore: ", Err)
                Return Nothing
            End Try
        End Function
        'Public Function GetPosizioni290(ByVal myStringConnection As String, IdEnte As String, CoattivoDal As String, CoattivoAl As String, IdRuolo As Integer) As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoArticoloRuolo()
        '    Dim cmdMyCommand As New SqlClient.SqlCommand
        '    Dim oMyAvviso As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoArticoloRuolo
        '    Dim oListAvvisi As New ArrayList
        '    Dim dtMyDati As New DataTable()
        '    Dim dtMyRow As DataRow
        '    Dim myAdapter As New SqlClient.SqlDataAdapter
        '    Dim oReplace As New OPENgovTIA.generalClass.generalFunction

        '    Try
        '        cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
        '        cmdMyCommand.Connection.Open()
        '        cmdMyCommand.CommandTimeout = 0
        '        cmdMyCommand.CommandType = CommandType.StoredProcedure
        '        cmdMyCommand.CommandText = "prc_GetPosizioni290"
        '        cmdMyCommand.Parameters.Clear()
        '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdEnte", SqlDbType.VarChar)).Value = IdEnte
        '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CoattivoDal", SqlDbType.VarChar)).Value = oReplace.FormattaData(CoattivoDal, "A")
        '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CoattivoAl", SqlDbType.VarChar)).Value = oReplace.FormattaData(CoattivoAl, "A")
        '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdRuolo", SqlDbType.Int)).Value = IdRuolo
        '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
        '        myAdapter.SelectCommand = cmdMyCommand
        '        myAdapter.Fill(dtMyDati)
        '        myAdapter.Dispose()
        '        For Each dtMyRow In dtMyDati.Rows
        '            oMyAvviso = New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoArticoloRuolo
        '            oMyAvviso.Id = CInt(dtMyRow("id"))
        '            oMyAvviso.IdContribuente = CInt(dtMyRow("idcontribuente"))
        '            oMyAvviso.Ente = CStr(dtMyRow("idente"))
        '            oMyAvviso.Anno = CStr(dtMyRow("anno"))
        '            oMyAvviso.DescrDiffImposta = CStr(dtMyRow("cod_tributo"))
        '            If Not IsDBNull(dtMyRow("datainizio")) Then
        '                oMyAvviso.DataInizio = CDate(dtMyRow("datainizio"))
        '            End If
        '            If Not IsDBNull(dtMyRow("IMPORTO_DIFFERENZA_IMPOSTA")) Then
        '                oMyAvviso.ImportoNetto = CDbl(dtMyRow("IMPORTO_DIFFERENZA_IMPOSTA"))
        '            End If
        '            If Not IsDBNull(dtMyRow("informazioni")) Then
        '                oMyAvviso.InformazioniCartella = CStr(dtMyRow("informazioni"))
        '            End If
        '            If Not IsDBNull(dtMyRow("nfile")) Then
        '                oMyAvviso.TipoRuolo = CStr(dtMyRow("nfile"))
        '            End If
        '            oListAvvisi.Add(oMyAvviso)
        '        Next

        '        Return CType(oListAvvisi.ToArray(GetType(RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoArticoloRuolo)), RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoArticoloRuolo())
        '    Catch Err As Exception
        '        Log.Debug(IdEnte + " - OPENgovPROVVEDIMENTI.clsCoattivo.GetPosizioni290.errore: ", Err)
        '        Return Nothing
        '    Finally
        '        dtMyDati.Dispose()
        '        cmdMyCommand.Dispose()
        '        cmdMyCommand.Connection.Close()
        '    End Try
        'End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sIdEnteCredBen"></param>
        ''' <returns></returns>
        Public Function PopolaN0(ByVal sIdEnteCredBen As String) As Importer290.N0
            Dim oN0 As New Importer290.N0

            Try
                oN0.CodiceEnteImpositore = New MyUtility().ReplaceCharForFile(sIdEnteCredBen)
                oN0.DataInvioFornitura = Now
                oN0.Filler = Space(275)

                Return oN0
            Catch Err As Exception
                Log.Debug(sIdEnteCredBen + " - OPENgovPROVVEDIMENTI.clsCoattivo.Crea290::PopolaN0.errore::" & Err.Message)
                Return Nothing
            End Try
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sIdEnteCNC"></param>
        ''' <param name="sProgRuolo"></param>
        ''' <param name="nRate"></param>
        ''' <returns></returns>
        Public Function PopolaN1(ByVal sIdEnteCNC As String, ByVal sProgRuolo As String, ByVal nRate As Integer) As Importer290.N1
            Dim oN1 As New Importer290.N1

            Try
                oN1.CodiceComuneIscrizioneRuolo = New MyUtility().ReplaceCharForFile(sIdEnteCNC)
                oN1.ProgressivoMinuta = "01"
                oN1.TipoRuolo = "1"
                oN1.NumeroRuolo = sProgRuolo
                oN1.NumeroRate = nRate
                oN1.ControlloRuolo = 0
                oN1.CodiceSede = 0
                oN1.TipoCompenso = 0
                oN1.Filler1 = "0"
                oN1.RuoloIciap = ""
                oN1.NumeroConvenzione = ""
                oN1.FlagArt89 = ""
                oN1.Filler2 = Space(245)

                Return oN1
            Catch Err As Exception
                Log.Debug(sIdEnteCNC + " - OPENgovPROVVEDIMENTI.clsCoattivo.Crea290::PopolaN1.errore::" & Err.Message)
                Return Nothing
            End Try
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="nIdContribuente"></param>
        ''' <param name="sCodTributo"></param>
        ''' <param name="sIdEnteCNC"></param>
        ''' <param name="nPartita"></param>
        ''' <param name="sNomeErr"></param>
        ''' <returns></returns>
        Public Function PopolaN2(ByVal nIdContribuente As Integer, ByVal sCodTributo As String, ByVal sIdEnteCNC As String, ByVal nPartita As Integer, ByRef sNomeErr As String) As Importer290.N2
            Dim oN2 As New Importer290.N2
            Dim DatiContribuente As DettaglioAnagrafica
            'Dim WFErrore As String
            'Dim WFSessione As CreateSessione
            Dim sNaturaGiud As Integer

            Try
                Dim oMyAnagrafica As New Anagrafica.DLL.GestioneAnagrafica()
                DatiContribuente = New DettaglioAnagrafica
                Log.Debug("Esportazione290::PopolaN2::inizializzo l'anagrafica::nIdContribuente::" & nIdContribuente & "::sCodTributo::" & sCodTributo)
                DatiContribuente = oMyAnagrafica.GetAnagrafica(nIdContribuente, -1, String.Empty, ConstSession.DBType, ConstSession.StringConnectionAnagrafica, False)
                If Not DatiContribuente Is Nothing Then
                    oN2.CodiceComuneIscrizioneRuolo = New MyUtility().ReplaceCharForFile(sIdEnteCNC)
                    oN2.ProgressivoMinuta = "01"
                    oN2.CodicePartita = nPartita
                    If DatiContribuente.CodiceFiscale <> "" Then
                        oN2.CodiceFiscaleIntestatarioRuolo = New MyUtility().ReplaceCharForFile(DatiContribuente.CodiceFiscale)
                    Else
                        oN2.CodiceFiscaleIntestatarioRuolo = New MyUtility().ReplaceCharForFile(DatiContribuente.PartitaIva)
                    End If
                    oN2.NumeroContribuente = 0
                    oN2.CodiceControlloNumeroContribuente = 0
                    'prelevo il codice del comune
                    If DatiContribuente.ComuneResidenza <> "" Then
                        oN2.CodiceIndirizzoReperimentoIntestatario = DatiContribuente.CodiceComuneResidenza
                    Else
                        oN2.CodiceIndirizzoReperimentoIntestatario = ""
                    End If

                    If IsNumeric(DatiContribuente.CivicoResidenza) Then
                        oN2.IndirizzoReperimentoIntestatario = New MyUtility().ReplaceCharForFile(DatiContribuente.ViaResidenza)
                        If DatiContribuente.CivicoResidenza = -1 Then DatiContribuente.CivicoResidenza = 0
                        oN2.NumeroCivicoReperimentoIntestatario = DatiContribuente.CivicoResidenza
                    Else
                        oN2.NumeroCivicoReperimentoIntestatario = 0
                        oN2.IndirizzoReperimentoIntestatario = Left(New MyUtility().ReplaceCharForFile(DatiContribuente.ViaResidenza + " " + Replace(DatiContribuente.CivicoResidenza, "/", "")), 30)
                    End If
                    oN2.LetteraNumeroCivicoReperimentoIntestatario = New MyUtility().ReplaceCharForFile(DatiContribuente.EsponenteCivicoResidenza)

                    oN2.KmReperimentoIntestatario = 0
                    oN2.CapReperimentoIntestatario = New MyUtility().ReplaceCharForFile(DatiContribuente.CapResidenza)
                    'prelevo il codice del comune
                    If DatiContribuente.ComuneResidenza <> "" Then
                        oN2.CodiceBelfioreReperimentoIntestatario = DatiContribuente.CodiceComuneResidenza
                    Else
                        oN2.CodiceBelfioreReperimentoIntestatario = ""
                    End If
                    oN2.LocalitaReperimentoIntestatario = New MyUtility().ReplaceCharForFile(DatiContribuente.FrazioneResidenza)
                    oN2.KmDomicilioFiscale = 0
                    Select Case DatiContribuente.Sesso
                        Case "F"
                            sNaturaGiud = 1
                        Case "G"
                            sNaturaGiud = 2
                        Case "M"
                            sNaturaGiud = 1
                        Case Else
                            If DatiContribuente.CodiceFiscale <> "" Then
                                sNaturaGiud = 1
                            ElseIf DatiContribuente.PartitaIva <> "" Then
                                sNaturaGiud = 2
                            Else
                                sNomeErr = "Utente::" + DatiContribuente.Cognome + " " + DatiContribuente.Nome
                                Log.Debug(sIdEnteCNC + " - OPENgovPROVVEDIMENTI.clsCoattivo.Crea290.Anagrafica con sesso errato - COD_CONTRIBUENTE::" & nIdContribuente)
                                Return Nothing
                            End If
                    End Select
                    oN2.NaturaGiuridicaIntestatario = sNaturaGiud
                    oN2.Cognome = New MyUtility().ReplaceCharForFile(DatiContribuente.Cognome)
                    oN2.Nome = New MyUtility().ReplaceCharForFile(DatiContribuente.Nome)
                    oN2.Sesso = DatiContribuente.Sesso
                    oN2.DataNascita = DatiContribuente.DataNascita
                    'prelevo il codice del comune
                    If DatiContribuente.ComuneNascita <> "" Then
                        oN2.CodiceBelfioreComuneNascita = DatiContribuente.CodiceComuneNascita
                    Else
                        oN2.CodiceBelfioreComuneNascita = ""
                    End If
                    oN2.IndicatorePartitaCointestata = ""
                    oN2.Filler = Space(4)
                    'prelevo il codice del comune
                    For Each mySped As ObjIndirizziSpedizione In DatiContribuente.ListSpedizioni
                        If mySped.CodTributo = Utility.Costanti.TRIBUTO_TARSU Then
                            If mySped.ComuneRCP <> "" Then
                                oN2.CodiceIndirizzoDomicilioFiscale = mySped.CodComuneRCP
                            Else
                                oN2.CodiceIndirizzoDomicilioFiscale = ""
                            End If
                            oN2.IndirizzoDomicilioFiscale = New MyUtility().ReplaceCharForFile(mySped.ViaRCP)
                            If mySped.CivicoRCP <> "" Then
                                oN2.NumeroCivicoDomicilioFiscale = mySped.CivicoRCP
                            End If
                            oN2.LetteraNumeroCivicoDomicilioFiscale = New MyUtility().ReplaceCharForFile(mySped.EsponenteCivicoRCP)
                            oN2.CapDomicilioFiscale = New MyUtility().ReplaceCharForFile(mySped.CapRCP)
                            'prelevo il codice del comune
                            If mySped.ComuneRCP <> "" Then
                                oN2.CodiceBelfioreDomicilioFiscale = mySped.CodComuneRCP
                            Else
                                oN2.CodiceBelfioreDomicilioFiscale = ""
                            End If
                            oN2.LocalitaDomicilioFiscale = New MyUtility().ReplaceCharForFile(mySped.FrazioneRCP)
                        End If
                    Next
                    '*** ***
                Else
                    sNomeErr = "Anagrafica non trovata x codice::" + nIdContribuente.ToString()
                    Log.Debug(sIdEnteCNC + " - OPENgovPROVVEDIMENTI.clsCoattivo.Crea290::Anagrafica non trovata - COD_CONTRIBUENTE::" & nIdContribuente)
                    Return Nothing
                End If

                Return oN2
            Catch Err As Exception
                Log.Debug(sIdEnteCNC + " - OPENgovPROVVEDIMENTI.clsCoattivo.Crea290.PopolaN2.errore::" & Err.Message & " - COD_CONTRIBUENTE::" & nIdContribuente)
                Return Nothing
            End Try
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="oArticolo"></param>
        ''' <param name="sIdEnteCNC"></param>
        ''' <param name="nPartita"></param>
        ''' <param name="sCodTributo"></param>
        ''' <param name="impImposta"></param>
        ''' <param name="oN2"></param>
        ''' <returns></returns>
        Public Function PopolaN4(ByVal oArticolo As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoArticoloRuolo, ByVal sIdEnteCNC As String, ByVal nPartita As Integer, ByVal sCodTributo As String, ByVal impImposta As Double, ByVal oN2 As Importer290.N2) As Importer290.N4
            Dim oN4 As New Importer290.N4

            Try
                oN4.CodiceComuneIscrizioneRuolo = New MyUtility().ReplaceCharForFile(sIdEnteCNC)
                oN4.ProgressivoMinuta = "01"
                oN4.CodicePartita = nPartita
                oN4.AnnoRiferimentoTributo = oArticolo.Anno
                oN4.CodiceTributo = sCodTributo
                oN4.Imponibile = 0
                oN4.Imposta = impImposta
                oN4.NumeroSemestriInteressi = 0
                oN4.DataDecorrenzaInteressi = oArticolo.DataInizio
                oN4.CodiceReparto = ""
                oN4.InformazioniCartella = oArticolo.InformazioniCartella
                oN4.ValoreIscrizioneRuolo = ""
                oN4.TitoloIscrizioneRuolo = ""
                oN4.DescrIscrizioneRuolo = ""
                oN4.FillerIscrizioneRuolo = ""
                oN4.DataSanzione = ""
                oN4.TargaAutomobilistica = ""
                oN4.Matricola = ""
                oN4.Filler = Space(112)
                oN4.oN2 = oN2

                Return oN4
            Catch Err As Exception
                Log.Debug(sIdEnteCNC + " - OPENgovPROVVEDIMENTI.clsCoattivo.Crea290::PopolaN4.errore::" & Err.Message)
                Return Nothing
            End Try
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sIdEnteCNC"></param>
        ''' <param name="nTotN1"></param>
        ''' <param name="nTotN2"></param>
        ''' <param name="nTotN3"></param>
        ''' <param name="nTotN4"></param>
        ''' <param name="nTotN5"></param>
        ''' <param name="impTotale"></param>
        ''' <returns></returns>
        Public Function PopolaN5(ByVal sIdEnteCNC As String, ByVal nTotN1 As Integer, ByVal nTotN2 As Integer, ByVal nTotN3 As Integer, ByVal nTotN4 As Integer, ByVal nTotN5 As Integer, ByVal impTotale As Double) As Importer290.N5
            Dim oN5 As New Importer290.N5

            Try
                oN5.CodiceComuneIscrizioneRuolo = New MyUtility().ReplaceCharForFile(sIdEnteCNC)
                oN5.ProgressivoMinuta = "01"
                oN5.TotaleRecordsN1N5 = nTotN1 + nTotN2 + nTotN3 + nTotN4 + nTotN5
                oN5.TotaleRecordsN2 = nTotN2
                oN5.TotaleRecordsN3 = nTotN3
                oN5.TotaleRecordsN4 = nTotN4
                oN5.TotaleImposta = impTotale * 100
                oN5.Filler = Space(237)

                Return oN5
            Catch Err As Exception
                Log.Debug(sIdEnteCNC + " - OPENgovPROVVEDIMENTI.clsCoattivo.Crea290::PopolaN5.errore::" & Err.Message)
                Return Nothing
            End Try
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sIdEnteCredBen"></param>
        ''' <param name="nTotN0"></param>
        ''' <param name="nTotN1"></param>
        ''' <param name="nTotN2"></param>
        ''' <param name="nTotN3"></param>
        ''' <param name="nTotN4"></param>
        ''' <param name="nTotN5"></param>
        ''' <param name="nTotN9"></param>
        ''' <returns></returns>
        Public Function PopolaN9(ByVal sIdEnteCredBen As String, ByVal nTotN0 As Integer, ByVal nTotN1 As Integer, ByVal nTotN2 As Integer, ByVal nTotN3 As Integer, ByVal nTotN4 As Integer, ByVal nTotN5 As Integer, ByVal nTotN9 As Integer) As Importer290.N9
            Dim oN9 As New Importer290.N9

            Try
                oN9.CodiceEnteImpositore = New MyUtility().ReplaceCharForFile(sIdEnteCredBen)
                oN9.TotaleRecordN0N9 = nTotN0 + nTotN1 + nTotN2 + nTotN3 + nTotN4 + nTotN5 + nTotN9
                oN9.TotaleRecordN1 = nTotN1
                oN9.TotaleRecordN2 = nTotN2
                oN9.TotaleRecordN3 = nTotN3
                oN9.TotaleRecordN4 = nTotN4
                oN9.TotaleRecordN5 = nTotN5
                oN9.Filler = Space(241)

                Return oN9
            Catch Err As Exception
                Log.Debug(sIdEnteCredBen + " - OPENgovPROVVEDIMENTI.clsCoattivo.Crea290::PopolaN9.errore::" & Err.Message)
                Return Nothing
            End Try
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sPathName290"></param>
        ''' <param name="oN0"></param>
        ''' <param name="oN1"></param>
        ''' <param name="oN4"></param>
        ''' <param name="oN5"></param>
        ''' <param name="oN9"></param>
        ''' <returns></returns>
        Public Function Create290(ByVal sPathName290 As String, ByVal oN0 As Importer290.N0, ByVal oN1() As Importer290.N1, ByVal oN4() As Importer290.N4, ByVal oN5() As Importer290.N5, ByVal oN9 As Importer290.N9) As Integer
            Dim x, y As Integer
            Dim nPartitaPrec As Integer

            Try
                'elimino il file se già presente
                If New MyUtility().DeleteFile(sPathName290) = False Then
                End If
                'scrivo il file
                If PrintLineN0(sPathName290, oN0) <= 0 Then
                    Return 0
                End If
                For x = 0 To oN1.GetUpperBound(0)
                    If PrintLineN1(sPathName290, oN1(x)) <= 0 Then
                        Return 0
                    End If
                    For y = 0 To oN4.GetUpperBound(0)
                        If oN4(y).CodicePartita <> nPartitaPrec Then
                            If PrintLineN2(sPathName290, oN4(y).oN2) <= 0 Then
                                Return 0
                            End If
                        End If
                        If PrintLineN4(sPathName290, oN4(y)) <= 0 Then
                            Return 0
                        End If
                        nPartitaPrec = oN4(y).CodicePartita
                    Next
                    If PrintLineN5(sPathName290, oN5(x)) <= 0 Then
                        Return 0
                    End If
                Next
                If PrintLineN9(sPathName290, oN9) <= 0 Then
                    Return 0
                End If

                Return 1
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsCoattivo.Crea290::Create290.errore::" & Err.Message)
                Return -1
            End Try
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sPathNameFile"></param>
        ''' <param name="oN0"></param>
        ''' <returns></returns>
        Public Function PrintLineN0(ByVal sPathNameFile As String, ByVal oN0 As Importer290.N0) As Integer
            Dim sPrintLine As String = ""
            Try
                'Campo Posizione 1-2 Lungh. 2 <<Record di inizio file>>
                sPrintLine += oN0.TipoRecord
                'Campo Posizione 3-7 Lungh. 5 <<Codice ente impositore>>
                sPrintLine += oN0.CodiceEnteImpositore.PadLeft(5, "0").Substring(0, 5)
                'Campo Posizione 8-15 Lungh. 8 <<Data di Invio fornitura>>
                sPrintLine += Format(CDate(oN0.DataInvioFornitura), "yyyyMMdd").PadLeft(8, "0").Substring(0, 8)
                'Campo Posizione 16-18 Lungh. 3 <<Release fisso R02>>
                sPrintLine += "R02"
                'Campo Posizione 19-290 Lungh. 272 <<Filler>>
                sPrintLine += oN0.Filler.PadRight(272, " ").Substring(0, 272)

                sPrintLine = New MyUtility().ReplaceCharForFile(sPrintLine)
                If New MyUtility().WriteFile(sPathNameFile, sPrintLine) <= 0 Then
                    Return 0
                End If

                Return 1
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsCoattivo.Crea290::PrintLineN0.errore::" & Err.Message)
                Return -1
            End Try
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sPathNameFile"></param>
        ''' <param name="oN1"></param>
        ''' <returns></returns>
        Public Function PrintLineN1(ByVal sPathNameFile As String, ByVal oN1 As Importer290.N1) As Integer
            Dim sPrintLine As String = ""
            Try
                'Campo Posizione 1-2 Lungh. 2 <<Codice Record Tipo N1>>
                sPrintLine += oN1.TipoRecord
                'Campo Posizione 3-8 Lungh. 6 <<Codice Comune di iscrizione>>
                sPrintLine += oN1.CodiceComuneIscrizioneRuolo.PadLeft(6, "0").Substring(0, 6)
                'Campo Posizione 9-10 Lungh. 2 <<Progressivo minuta>>
                sPrintLine += oN1.ProgressivoMinuta.ToString.PadLeft(2, "0").Substring(0, 2)
                'Campo Posizione 11-11 Lingh. 1 <<Tipo Ruolo>>
                sPrintLine += oN1.TipoRuolo.ToString.PadLeft(1, "0").Substring(0, 1)
                'Campo Posizione 12-15 Lungh. 4 <<Numero del ruolo>>
                sPrintLine += oN1.NumeroRuolo.PadLeft(4, "0").Substring(0, 4)
                'Campo Posizione 16-17 Lungh. 2 <<Numero delle rate>>
                sPrintLine += oN1.NumeroRate.ToString.PadLeft(2, "0").Substring(0, 2)
                'Campo Posizione 18-18 Lungh. 1 <<Tipo Ruolo 0-Ruolo Normale, 1-Ruolo Coattivo>>
                sPrintLine += oN1.ControlloRuolo.ToString.PadLeft(1, "0").Substring(0, 1)
                'Campo Posizione 19-22 Lungh. 4 <<Codice sede>>
                sPrintLine += oN1.CodiceSede.ToString.PadLeft(4, "0").Substring(0, 4)
                'Campo Posizione 23-23 Lungh. 1 <<Tipo compenso>>
                sPrintLine += oN1.TipoCompenso.ToString.PadLeft(1, "0").Substring(0, 1)
                'Campo Posizione 24-41 Lungh. 18 <<Filler>>
                sPrintLine += oN1.Filler1.PadLeft(18, " ").Substring(0, 18)
                'Campo Posizione 42-42 Lungh. 1 <<1=Ruolo ICIAP – spazio negli altri casi>>
                sPrintLine += oN1.RuoloIciap.PadLeft(1, " ").Substring(0, 1)
                'Campo Posizione 43-44 Lungh. 2 <<Numero convenzione>>
                sPrintLine += oN1.NumeroConvenzione.ToString.PadLeft(2, " ").Substring(0, 2)
                'Campo Posizione 45-45 Lungh. 1 <<Flag ART.89/64/65>>
                sPrintLine += oN1.FlagArt89.ToString.PadLeft(1, " ").Substring(0, 1)
                'Campo Posizione 46-290 Lungh. 245 <<Filler>>
                sPrintLine += oN1.Filler2.PadRight(245, " ").Substring(0, 245)

                sPrintLine = New MyUtility().ReplaceCharForFile(sPrintLine)
                If New MyUtility().WriteFile(sPathNameFile, sPrintLine) <= 0 Then
                    Return 0
                End If

                Return 1
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsCoattivo.Crea290::PrintLineN1.errore::" & Err.Message)
                Return -1
            End Try
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sPathNameFile"></param>
        ''' <param name="oN2"></param>
        ''' <returns></returns>
        Public Function PrintLineN2(ByVal sPathNameFile As String, ByVal oN2 As Importer290.N2) As Integer
            Dim sPrintLine As String = ""
            Try
                'Campo Posizione 1-2 Lungh. 2 <<Codice Record N2>>
                sPrintLine += oN2.TipoRecord
                'Campo Posizione 3-8  Lungh. 6 <<Codice Comune di Iscrizione>>
                sPrintLine += oN2.CodiceComuneIscrizioneRuolo.PadLeft(6, "0").Substring(0, 6)
                'Campo Posizione 9-10 Lungh. 2 <<Progressivo Minuta>>
                sPrintLine += oN2.ProgressivoMinuta.ToString.PadLeft(2, "0").Substring(0, 2)
                'Campo Posizione 11-24 Lungh. 14 <<Codice Partita Identificatore intestatario>>
                sPrintLine += oN2.CodicePartita.PadRight(14, " ").Substring(0, 14)
                'Campo Posizione 25-40  Lungh. 16 <<Codice Fiscale>>
                sPrintLine += oN2.CodiceFiscaleIntestatarioRuolo.PadRight(16, " ").Substring(0, 16)
                'Campo Posizione 41-48 Lungh. 8 <<Numero di Contribuente>> inserire Codice Esattoriale
                sPrintLine += oN2.NumeroContribuente.ToString.PadLeft(8, "0").Substring(0, 8)
                'Campo Posizione 49-50 Lungh. 2 <<Codice Controllo del codice Contribuente>>
                sPrintLine += oN2.CodiceControlloNumeroContribuente.ToString.PadLeft(2, "0").Substring(0, 2)
                'Campo Posizione 51-56 Lungh. 6 <<Codice CNC indirizzo residenza>>
                sPrintLine += oN2.CodiceIndirizzoReperimentoIntestatario.PadLeft(6, "0").Substring(0, 6)
                'Campo Posizione 57-86 Lungh. 30 <<Indirizzo>>
                sPrintLine += oN2.IndirizzoReperimentoIntestatario.PadRight(30, " ").Substring(0, 30)
                'Campo Posizione 87-91 Lungh. 5 <<Numero civico>>
                sPrintLine += oN2.NumeroCivicoReperimentoIntestatario.ToString.PadLeft(5, "0").Substring(0, 5)
                'Campo Posizione 92-93 Lungh. 2 <<Lettera numero civico>>
                sPrintLine += oN2.LetteraNumeroCivicoReperimentoIntestatario.PadRight(2, " ").Substring(0, 2)
                'Campo Posizione 94-99 Lungh. 6 <<Km (3interi, 3decimali>>
                sPrintLine += oN2.KmReperimentoIntestatario.PadLeft(6, "0").Substring(0, 6)
                'Campo Posizione 100-104 Lungh. 5 <<CAP>>
                sPrintLine += oN2.CapReperimentoIntestatario.PadLeft(5, "0").Substring(0, 5)
                'Campo Posizione 105-108 Lungh. 4 <<Codice Belfiore comune>>
                sPrintLine += oN2.CodiceBelfioreReperimentoIntestatario.PadRight(4, " ").Substring(0, 4)
                'Campo Posizione 109-129 Lungh. 21 <<località o frazione>>
                sPrintLine += oN2.LocalitaReperimentoIntestatario.PadRight(21, " ").Substring(0, 21)
                'Campo Posizione 130-135 Lungh. 6 <<Codice CNC indirizzo domicilio fiscale>>
                If oN2.CodiceIndirizzoDomicilioFiscale Is Nothing Then
                    oN2.CodiceIndirizzoDomicilioFiscale = ""
                End If
                sPrintLine += oN2.CodiceIndirizzoDomicilioFiscale.PadLeft(6, "0").Substring(0, 6)
                'Campo Posizione 136-165 Lungh. 30 <<Indirizzo>>
                If oN2.IndirizzoDomicilioFiscale Is Nothing Then
                    oN2.IndirizzoDomicilioFiscale = ""
                End If
                sPrintLine += oN2.IndirizzoDomicilioFiscale.PadRight(30, " ").Substring(0, 30)
                'Campo Posizione 166-170 Lungh. 5 <<Numero civico>>
                sPrintLine += oN2.NumeroCivicoDomicilioFiscale.ToString.PadLeft(5, "0").Substring(0, 5)
                'Campo Posizione 171-172 Lungh. 2 <<Lettera numero civico>>
                If oN2.LetteraNumeroCivicoDomicilioFiscale Is Nothing Then
                    oN2.LetteraNumeroCivicoDomicilioFiscale = ""
                End If
                sPrintLine += oN2.LetteraNumeroCivicoDomicilioFiscale.PadRight(2, " ").Substring(0, 2)
                'Campo Posizione 173-178 Lungh. 6 <<Km (3interi, 3decimali>>
                If oN2.KmDomicilioFiscale Is Nothing Then
                    oN2.KmDomicilioFiscale = ""
                End If
                sPrintLine += oN2.KmDomicilioFiscale.PadLeft(6, "0").Substring(0, 6)
                'Campo Posizione 179-183 Lungh. 5 <<CAP>>
                If oN2.CapDomicilioFiscale Is Nothing Then
                    oN2.CapDomicilioFiscale = ""
                End If
                sPrintLine += oN2.CapDomicilioFiscale.PadLeft(5, "0").Substring(0, 5)
                'Campo Posizione 184-187 Lungh. 4 <<Codice Belfiore comune>>
                If oN2.CodiceBelfioreDomicilioFiscale Is Nothing Then
                    oN2.CodiceBelfioreDomicilioFiscale = ""
                End If
                sPrintLine += oN2.CodiceBelfioreDomicilioFiscale.PadRight(4, " ").Substring(0, 4)
                'Campo Posizione 188-208 Lungh. 21 <<località o frazione>>
                If oN2.LocalitaDomicilioFiscale Is Nothing Then
                    oN2.LocalitaDomicilioFiscale = ""
                End If
                sPrintLine += oN2.LocalitaDomicilioFiscale.PadRight(21, " ").Substring(0, 21)
                'Campo Posizione 209-209 Lungh. 1 <<natura giuridica intestatario>>
                sPrintLine += oN2.NaturaGiuridicaIntestatario.ToString.PadLeft(1, "0").Substring(0, 1)
                If oN2.NaturaGiuridicaIntestatario = 1 Then
                    'Campo Posizione 210-233 Lungh. 24 <<Cognome>>
                    sPrintLine += oN2.Cognome.PadRight(24, " ").Substring(0, 24)
                    'Campo Posizione 234-253 Lungh. 20 <<Nome>>
                    sPrintLine += oN2.Nome.PadRight(20, " ").Substring(0, 20)
                    'Campo Posizione 254-254 Lungh. 1 <<Sesso F=femmina M=maschio>>
                    sPrintLine += oN2.Sesso.PadRight(1, " ").Substring(0, 1)
                    'Campo Posizione 255-262 Lungh. 8 <<Data di nascita>>
                    If oN2.DataNascita <> "" And IsDate(oN2.DataNascita) Then
                        sPrintLine += Format(CDate(oN2.DataNascita), "ddMMyyyy").PadLeft(8, "0").Substring(0, 8)
                    Else
                        sPrintLine += oN2.DataNascita.PadLeft(8, "0").Substring(0, 8)
                    End If
                    'Campo Posizione 263-266 Lungh. 4 <<Codice Belfiore comune di nascita>>
                    sPrintLine += oN2.CodiceBelfioreComuneNascita.PadRight(4, " ").Substring(0, 4)
                    'Campo Posizione 267-285 Lungh. 19 <<Filler>>
                    sPrintLine += oN2.Filler.PadRight(19, " ").Substring(0, 19)
                Else
                    'Campo Posizione 210-285 Lungh. 76 <<Denominazione della società>>
                    sPrintLine += oN2.Cognome.PadRight(76, " ").Substring(0, 76)
                End If
                'Campo Posizione 286-286 Lungh. 1 <<Indicatore Partita Cointestata>>
                sPrintLine += oN2.IndicatorePartitaCointestata.PadRight(1, " ").Substring(0, 1)
                'Campo Posizione 287-290 Lungh. 4 <<Filler>>
                sPrintLine += oN2.Filler.PadRight(4, " ").Substring(0, 4)

                sPrintLine = New MyUtility().ReplaceCharForFile(sPrintLine)
                If New MyUtility().WriteFile(sPathNameFile, sPrintLine) <= 0 Then
                    Return 0
                End If

                Return 1
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsCoattivo.Crea290::PrintLineN2.errore::" & Err.Message)
                Return -1
            End Try
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sPathNameFile"></param>
        ''' <param name="oN4"></param>
        ''' <returns></returns>
        Public Function PrintLineN4(ByVal sPathNameFile As String, ByVal oN4 As Importer290.N4) As Integer
            Dim sPrintLine As String = ""
            Try
                'Campo Posizione 1-2 Lungh. 2 <<Codice Record N4>>
                sPrintLine += oN4.TipoRecord
                'Campo Posizione 3-8  Lungh. 6 <<Codice Comune di Iscrizione>>
                sPrintLine += oN4.CodiceComuneIscrizioneRuolo.PadLeft(6, "0").Substring(0, 6)
                'Campo Posizione 9-10 Lungh. 2 <<Progressivo Minuta>>
                sPrintLine += oN4.ProgressivoMinuta.ToString.PadLeft(2, "0").Substring(0, 2)
                'Campo Posizione 11-24 Lungh. 14 <<Codice Partita Identificatore intestatario>>
                sPrintLine += oN4.CodicePartita.PadRight(14, " ").Substring(0, 14)
                'Campo Posizione 25-28 Lungh. 4 <<Anno di riferimento>>
                sPrintLine += oN4.AnnoRiferimentoTributo.PadLeft(4, "0").Substring(0, 4)
                'Campo Posizione 29-32 Lungh. 4 <<Codice del tributo>>
                sPrintLine += oN4.CodiceTributo.PadLeft(4, "0").Substring(0, 4)
                'Campo Posizione 33-45 Lungh. 13 <<Imponibile>>
                sPrintLine += FormatNumber(oN4.Imponibile, 2).Replace(".", "").Replace(",", "").PadLeft(13, "0").Substring(0, 13)
                'Campo Posizione 46-58 Lungh. 13 <<Importo>>
                sPrintLine += FormatNumber(oN4.Imposta, 2).Replace(".", "").Replace(",", "").PadLeft(13, "0").Substring(0, 13)
                'Campo Posizione 59-60 Lungh. 2 <<Numero semestri interessi>>
                sPrintLine += oN4.NumeroSemestriInteressi.ToString.PadLeft(2, "0").Substring(0, 2)
                'Campo Posizione 61-68 Lungh. 8 <<Data decorrenza interessi>>
                If oN4.DataDecorrenzaInteressi <> "" And CDate(oN4.DataDecorrenzaInteressi) <> DateTime.MaxValue.ToShortDateString Then
                    sPrintLine += Format(CDate(oN4.DataDecorrenzaInteressi), "ddMMyyyy").PadLeft(8, "0").Substring(0, 8)
                Else
                    sPrintLine += "".PadLeft(8, "0")
                End If
                'Campo Posizione 69-70 Lungh. 2 <<Codice del reparto>>
                sPrintLine += oN4.CodiceReparto.PadRight(2, " ").Substring(0, 2)
                'Campo Posizione 71-145 Lungh. 75 <<Informazioni da riportare sulla cartella>>
                sPrintLine += oN4.InformazioniCartella.PadRight(75, " ").Substring(0, 75)
                '<<Informazioni da riportare sul ruolo>>
                Select Case oN4.ValoreIscrizioneRuolo
                    Case "E"
                        'Campo Posizione 146-146 Lungh. 1 <<Valore iscrizione a ruolo>>
                        sPrintLine += oN4.ValoreIscrizioneRuolo.PadRight(1, " ").Substring(0, 1)
                        'Campo Posizione 147-157 Lungh. 11 <<Titolo>>
                        sPrintLine += oN4.TitoloIscrizioneRuolo.PadRight(11, " ").Substring(0, 11)
                        'Campo Posizione 158-172 Lungh. 15 <<Descrizione>>
                        sPrintLine += oN4.DescrIscrizioneRuolo.PadRight(15, " ").Substring(0, 15)
                        'Campo Posizione 173-178 Lungh. 6 <<Impostare a spazi>>
                        sPrintLine += oN4.FillerIscrizioneRuolo.PadRight(6, " ").Substring(0, 6)
                    Case "S"
                        'Campo Posizione 146-146 Lungh. 1 <<Valore iscrizione a ruolo>>
                        sPrintLine += oN4.ValoreIscrizioneRuolo.PadRight(1, " ").Substring(0, 1)
                        'Campo Posizione 147-148 Lungh. 1 <<VE=verbale - OR=ordinanza – IN=ingiunzione – DM= decreto ministeriale>>
                        sPrintLine += oN4.DescrIscrizioneRuolo.PadRight(1, " ").Substring(0, 1)
                        'Campo Posizione 149-160 Lungh. 12 <<Titolo>>
                        sPrintLine += oN4.TitoloIscrizioneRuolo.PadRight(12, " ").Substring(0, 12)
                        'Campo Posizione 161-166 Lungh. 6 <<Data>>
                        If oN4.DataSanzione <> "" Then
                            sPrintLine += Format(CDate(oN4.DataSanzione), " ddMMyy").PadLeft(8, "0").Substring(0, 8)
                        Else
                            sPrintLine += oN4.DataSanzione.PadLeft(8, "0").Substring(0, 8)
                        End If
                        'Campo Posizione 167-178 Lungh. 12 <<Targa>>
                        sPrintLine += oN4.TargaAutomobilistica.PadRight(12, " ").Substring(0, 12)
                    Case "M"
                        'Campo Posizione 146-146 Lungh. 1 <<Valore iscrizione a ruolo>>
                        sPrintLine += oN4.ValoreIscrizioneRuolo.PadRight(1, " ").Substring(0, 1)
                        'Campo Posizione 149-156 Lungh. 10 <<Matricola>>
                        sPrintLine += oN4.Matricola.PadRight(10, " ").Substring(0, 10)
                        'Campo Posizione 157-178 Lungh. 22 <<Impostare a spazi>>
                        sPrintLine += oN4.FillerIscrizioneRuolo.PadRight(22, " ").Substring(0, 22)
                    Case Else
                        'Campo Posizione 146-178 Lungh. 33 <<Imformazioni da riportare sul ruolo>>
                        sPrintLine += oN4.FillerIscrizioneRuolo.PadRight(33, " ").Substring(0, 33)
                End Select
                'Campo Posizione 179-290 Lungh. 112 <<Filler>>
                If oN4.InformazioniCartella.Length > 75 Then
                    sPrintLine += oN4.InformazioniCartella.Substring(75, oN4.InformazioniCartella.Length - 75).PadRight(112, " ").Substring(0, 112)
                Else
                    sPrintLine += oN4.Filler.PadRight(112, " ").Substring(0, 112)
                End If

                sPrintLine = New MyUtility().ReplaceCharForFile(sPrintLine)
                If New MyUtility().WriteFile(sPathNameFile, sPrintLine) <= 0 Then
                    Return 0
                End If

                Return 1
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsCoattivo.Crea290::PrintLineN4.errore::" & Err.Message)
                Return -1
            End Try
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sPathNameFile"></param>
        ''' <param name="oN5"></param>
        ''' <returns></returns>
        Public Function PrintLineN5(ByVal sPathNameFile As String, ByVal oN5 As Importer290.N5) As Integer
            Dim sPrintLine As String = ""
            Try
                'Campo Posizione 1-2 Lungh. 2 <<Codice Record Tipo N5>>
                sPrintLine += oN5.TipoRecord
                'Campo Posizione 3-8 Lungh. 6 <<Codice Comune di iscrizione>>
                sPrintLine += oN5.CodiceComuneIscrizioneRuolo.PadLeft(6, "0").Substring(0, 6)
                'Campo Posizione 9-10 Lungh. 2 <<Progressivo minuta>>
                sPrintLine += oN5.ProgressivoMinuta.ToString.PadLeft(2, "0").Substring(0, 2)
                'Campo Posizione 11-17 Lungh. 7 <<Totale del Totale di tutti i records>>
                sPrintLine += oN5.TotaleRecordsN1N5.ToString.PadLeft(7, "0").Substring(0, 7)
                'Campo Posizione 18-24 Lungh. 7 <<Totale dei record N2>>
                sPrintLine += oN5.TotaleRecordsN2.ToString.PadLeft(7, "0").Substring(0, 7)
                'Campo Posizione 25-31 Lungh. 7 <<Totale record N3>>
                sPrintLine += oN5.TotaleRecordsN3.ToString.PadLeft(7, "0").Substring(0, 7)
                'Campo Posizione 32-38 Lungh. 7 <<Totale dei record N4>>
                sPrintLine += oN5.TotaleRecordsN4.ToString.PadLeft(7, "0").Substring(0, 7)
                'Campo Posizione 39-53 Lungh. 15 <<Totale importo complessivo>>
                sPrintLine += oN5.TotaleImposta.ToString.PadLeft(15, "0").Substring(0, 15)
                'Campo Posizione 54-290 Lungh. 237 <<Filler>>
                sPrintLine += oN5.Filler.PadRight(237, " ").Substring(0, 237)

                sPrintLine = New MyUtility().ReplaceCharForFile(sPrintLine)
                If New MyUtility().WriteFile(sPathNameFile, sPrintLine) <= 0 Then
                    Return 0
                End If

                Return 1
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsCoattivo.Crea290::PrintLineN5.errore::" & Err.Message)
                Return -1
            End Try
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sPathNameFile"></param>
        ''' <param name="oN9"></param>
        ''' <returns></returns>
        Public Function PrintLineN9(ByVal sPathNameFile As String, ByVal oN9 As Importer290.N9) As Integer
            Dim sPrintLine As String = ""
            Try
                'Campo Posizione 1-2 Lungh. 2 << Codice Record N9>>
                sPrintLine += oN9.TipoRecord
                'Campo Posizione 3-7 Lungh. 5 <<Codice ente impositore>>
                sPrintLine += oN9.CodiceEnteImpositore.PadLeft(5, "0").Substring(0, 5)
                'Campo Posizione 8-14 Lungh. 7 <<Totale del totale di tutti i records>>
                sPrintLine += oN9.TotaleRecordN0N9.ToString.PadLeft(7, "0").Substring(0, 7)
                'Campo Posizione 15-21 Lungh. 7 <<Totale record N1>>
                sPrintLine += oN9.TotaleRecordN1.ToString.PadLeft(7, "0").Substring(0, 7)
                'Campo Posizione 22-28 Lungh. 7 <<Totale Record N2>>
                sPrintLine += oN9.TotaleRecordN2.ToString.PadLeft(7, "0").Substring(0, 7)
                'Campo Posizione 29-35 Lungh. 7 <<Totale record N3>>
                sPrintLine += oN9.TotaleRecordN3.ToString.PadLeft(7, "0").Substring(0, 7)
                'Campo Posizione 36-42 Lungh. 7 <<Totale Record N4>>
                sPrintLine += oN9.TotaleRecordN4.ToString.PadLeft(7, "0").Substring(0, 7)
                'Campo Posizione 43-49 Lungh. 7 <<Totale Record N5>>
                sPrintLine += oN9.TotaleRecordN5.ToString.PadLeft(7, "0").Substring(0, 7)
                'Campo Posizione 50-290 Lungh. 241 <<Filler>>
                sPrintLine += oN9.Filler.PadRight(241, " ").Substring(0, 241)

                sPrintLine = New MyUtility().ReplaceCharForFile(sPrintLine)
                If New MyUtility().WriteFile(sPathNameFile, sPrintLine) <= 0 Then
                    Return 0
                End If

                Return 1
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsCoattivo.Crea290.PrintLineN9.errore::" & Err.Message)
                Return -1
            End Try
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
#End Region
End Class
