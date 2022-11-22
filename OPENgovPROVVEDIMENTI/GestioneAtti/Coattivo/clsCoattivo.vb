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
        ''' <param name="ListTributi"></param>
        ''' <param name="Anno"></param>
        ''' <returns></returns>
        Public Function PrintMinuta(ByVal ListItem() As ObjCoattivo, ByVal sIntestazioneEnte As String, nCampi As Integer, Dal As Date, Al As Date, ListTributi As String, Anno As Integer) As DataTable
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
                sDatiStampa = "Minuta Coattivi "
                sDatiStampa += ListTributi.Replace(Utility.Costanti.TRIBUTO_ICI, "IMU").Replace(Utility.Costanti.TRIBUTO_TARSU, "TARI")
                sDatiStampa += " Notificati dal " + New Formatta.FunctionGrd().FormattaDataGrd(Dal) + " al " + New Formatta.FunctionGrd().FormattaDataGrd(Al)
                If Anno > 0 Then
                    sDatiStampa += " per l'anno " + Anno.ToString
                End If
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
        ''' <revisionHistory><revision date="20200524">Gli avvisi notificati dal 2020 in poi devono essere estratti secondo il nuovo tracciato</revision></revisionHistory>
        Public Function Crea290(ByVal myStringConnection As String, ByVal IdEnteCNC As String, CoattivoDal As String, CoattivoAl As String, IdRuolo As Integer, ByVal sIdEnteCredBen As String, ByVal nRate As Integer, sPathFile As String, ByRef sNameFile As String, ByRef ErrAnag As String, ByRef ListFile As ArrayList) As Integer
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
                    If oListArticoli.GetLength(0) > 0 Then
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
                        sNameFile = sNameFile + IdRuolo.ToString.PadLeft(3, "0")
                        If ZipFile(sPathFile, sNameFile & ".zip", ListFile) = False Then
                            Return 0
                        End If
                    Else
                        sNameFile = sIdEnteCredBen + Now.Year.ToString.PadLeft(4, "0") + IdRuolo.ToString.PadLeft(3, "0") + ".txt"
                        Return New clsAccertamentiEsecutivi().CreaAccertamentiEsecutivi(myStringConnection, IdRuolo, sPathFile, sNameFile, ListFile)
                    End If
                Else
                    sNameFile = sIdEnteCredBen + Now.Year.ToString.PadLeft(4, "0") + IdRuolo.ToString.PadLeft(3, "0") + ".txt"
                    Return New clsAccertamentiEsecutivi().CreaAccertamentiEsecutivi(myStringConnection, IdRuolo, sPathFile, sNameFile, ListFile)
                End If

                Return 1
            Catch Err As Exception
                Log.Debug(IdEnteCNC + " - OPENgovPROVVEDIMENTI.clsCoattivo.Crea290.errore::" + Err.Message)
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
                Log.Debug(sIdEnteCredBen + " - OPENgovPROVVEDIMENTI.clsCoattivo.Crea290::PopolaN0.errore::" + Err.Message)
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
                Log.Debug(sIdEnteCNC + " - OPENgovPROVVEDIMENTI.clsCoattivo.Crea290::PopolaN1.errore::" + Err.Message)
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
                        If mySped.CodTributo = sCodTributo.Replace(Utility.Costanti.TRIBUTO_TASI, Utility.Costanti.TRIBUTO_ICI) Then 'Utility.Costanti.TRIBUTO_TARSU Then
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
                    If oN2.CodiceBelfioreDomicilioFiscale = "" And DatiContribuente.Sesso = "G" Then
                        If DatiContribuente.ComuneResidenza <> "" Then
                            oN2.CodiceIndirizzoDomicilioFiscale = DatiContribuente.CodiceComuneResidenza
                        Else
                            oN2.CodiceIndirizzoDomicilioFiscale = ""
                        End If
                        If IsNumeric(DatiContribuente.CivicoResidenza) Then
                            oN2.IndirizzoDomicilioFiscale = New MyUtility().ReplaceCharForFile(DatiContribuente.ViaResidenza)
                            If DatiContribuente.CivicoResidenza = -1 Then DatiContribuente.CivicoResidenza = 0
                            oN2.NumeroCivicoDomicilioFiscale = DatiContribuente.CivicoResidenza
                        Else
                            oN2.NumeroCivicoDomicilioFiscale = 0
                            oN2.IndirizzoDomicilioFiscale = Left(New MyUtility().ReplaceCharForFile(DatiContribuente.ViaResidenza + " " + Replace(DatiContribuente.CivicoResidenza, "/", "")), 30)
                        End If
                        oN2.LetteraNumeroCivicoDomicilioFiscale = New MyUtility().ReplaceCharForFile(DatiContribuente.EsponenteCivicoResidenza)
                        oN2.CapDomicilioFiscale = New MyUtility().ReplaceCharForFile(DatiContribuente.CapResidenza)
                        'prelevo il codice del comune
                        If DatiContribuente.ComuneResidenza <> "" Then
                            oN2.CodiceBelfioreDomicilioFiscale = DatiContribuente.CodiceComuneResidenza
                        Else
                            oN2.CodiceBelfioreDomicilioFiscale = ""
                        End If
                        oN2.LocalitaDomicilioFiscale = New MyUtility().ReplaceCharForFile(DatiContribuente.FrazioneResidenza)
                    End If
                Else
                    sNomeErr = "Anagrafica non trovata x codice::" + nIdContribuente.ToString()
                    Log.Debug(sIdEnteCNC + " - OPENgovPROVVEDIMENTI.clsCoattivo.Crea290::Anagrafica non trovata - COD_CONTRIBUENTE::" & nIdContribuente)
                    Return Nothing
                End If

                Return oN2
            Catch Err As Exception
                Log.Debug(sIdEnteCNC + " - OPENgovPROVVEDIMENTI.clsCoattivo.Crea290.PopolaN2.errore::" + Err.Message & " - COD_CONTRIBUENTE::" & nIdContribuente)
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
                Log.Debug(sIdEnteCNC + " - OPENgovPROVVEDIMENTI.clsCoattivo.Crea290::PopolaN4.errore::" + Err.Message)
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
                Log.Debug(sIdEnteCNC + " - OPENgovPROVVEDIMENTI.clsCoattivo.Crea290::PopolaN5.errore::" + Err.Message)
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
                Log.Debug(sIdEnteCredBen + " - OPENgovPROVVEDIMENTI.clsCoattivo.Crea290::PopolaN9.errore::" + Err.Message)
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
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsCoattivo.Crea290::Create290.errore::" + Err.Message)
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
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsCoattivo.Crea290::PrintLineN0.errore::" + Err.Message)
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
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsCoattivo.Crea290::PrintLineN1.errore::" + Err.Message)
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
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsCoattivo.Crea290::PrintLineN2.errore::" + Err.Message)
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
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsCoattivo.Crea290::PrintLineN4.errore::" + Err.Message)
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
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsCoattivo.Crea290::PrintLineN5.errore::" + Err.Message)
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
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsCoattivo.Crea290.PrintLineN9.errore::" + Err.Message)
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
#Region "AccertamentiEsecutivi"
    Public Function GetDelibera(ByVal IdEnte As String, myStringConnection As String, ByRef Numero As String, ByRef Dal As String, ByRef Al As String) As Boolean
        Dim sSQL As String
        Dim myDataView As New DataView

        Try
            Using ctx As New DBModel(ConstSession.DBType, myStringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetDelibera", "IDENTE")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", IdEnte))
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsCoattivo.GetDelibera.erroreQuery: ", ex)
                    Return Nothing
                Finally
                    ctx.Dispose()
                End Try
                For Each myRow As DataRowView In myDataView
                    Numero = StringOperation.FormatString(myRow("ndelibera"))
                    Dal = StringOperation.FormatDateTime(myRow("datadelibera"))
                    Al = StringOperation.FormatDateTime(myRow("datafinedelibera"))
                Next
            End Using
            Return True
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsCoattivo.GetDelibera.errore: ", Err)
            Return False
        End Try
    End Function
    Public Function SetDelibera(ByVal IdEnte As String, myStringConnection As String, Numero As String, Dal As Date, Al As Date) As Boolean
        Dim sSQL As String
        Dim myDataView As New DataView

        Try
            Using ctx As New DBModel(ConstSession.DBType, myStringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_SetDelibera", "IDENTE", "NDELIBERA", "DATADELIBERA", "DATAFINEDELIBERA")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", IdEnte) _
                        , ctx.GetParam("NDELIBERA", Numero) _
                        , ctx.GetParam("DATADELIBERA", Dal) _
                        , ctx.GetParam("DATAFINEDELIBERA", Al))
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsCoattivo.SetDelibera.erroreQuery: ", ex)
                    Return Nothing
                Finally
                    ctx.Dispose()
                End Try
            End Using
            Return True
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsCoattivo.SetDelibera.errore: ", Err)
            Return False
        End Try
    End Function
    Public Class objAccertamentiEsecutivi
        Public Class Type
            Public Const Stringa As String = "AN"
            Public Const Numero As String = "N"
            Public Const Data As String = "D"
        End Class
        Public Class IdentificativoFlusso
            Private _CodiceEnteCreditore As Integer
            Private _TipoUfficio As String
            Private _CodiceUfficio As String
            Public Class Length
                Public Const CodiceEnteCreditore As Integer = 5
                Public Const TipoUfficio As Integer = 1
                Public Const CodiceUfficio As Integer = 6
            End Class
            Public Property CodiceEnteCreditore() As String
                Get
                    Return _CodiceEnteCreditore
                End Get
                Set(ByVal Value As String)
                    _CodiceEnteCreditore = Value
                End Set
            End Property
            Public Property TipoUfficio() As String
                Get
                    Return _TipoUfficio
                End Get
                Set(ByVal Value As String)
                    _TipoUfficio = Value
                End Set
            End Property
            Public Property CodiceUfficio() As String
                Get
                    Return _CodiceUfficio
                End Get
                Set(ByVal Value As String)
                    _CodiceUfficio = Value
                End Set
            End Property
        End Class
        Public Class IdentificativoAtto
            Public Const TipologiaAtto As Integer = 5
            Public Const ProgressivoPartita As Integer = 1
            Public Const CodiceTipoAtto As String = "AA"
            Public Const FillerIdentificativoAtto As String = ""
            Private _IdentificativoFlusso As IdentificativoFlusso
            Private _AnnoEmissioneAtto As Integer
            Private _NumeroPartita As Integer
            Private _DataEmissioneAtto As DateTime
            Private _NumeroAtto As String
            Private _DataNotificaAtto As DateTime
            Public Class Length
                Public Const AnnoEmissioneAtto As Integer = 4
                Public Const TipologiaAtto As Integer = 3
                Public Const NumeroPartita As Integer = 9
                Public Const ProgressivoPartita As Integer = 3
                Public Const CodiceTipoAtto As Integer = 2
                Public Const DataEmissioneAtto As Integer = 8
                Public Const NumeroAtto As Integer = 12
                Public Const DataNotificaAtto As Integer = 8
                Public Const FillerIdentificativoAtto As Integer = 40
            End Class
            Public Property IdentificativoFlusso() As IdentificativoFlusso
                Get
                    Return _IdentificativoFlusso
                End Get
                Set(ByVal Value As IdentificativoFlusso)
                    _IdentificativoFlusso = Value
                End Set
            End Property
            Public Property AnnoEmissioneAtto() As String
                Get
                    Return _AnnoEmissioneAtto
                End Get
                Set(ByVal Value As String)
                    _AnnoEmissioneAtto = Value
                End Set
            End Property
            Public Property NumeroPartita() As String
                Get
                    Return _NumeroPartita
                End Get
                Set(ByVal Value As String)
                    _NumeroPartita = Value
                End Set
            End Property
            Public Property DataEmissioneAtto() As DateTime
                Get
                    Return _DataEmissioneAtto
                End Get
                Set(ByVal Value As DateTime)
                    _DataEmissioneAtto = Value
                End Set
            End Property
            Public Property NumeroAtto() As String
                Get
                    Return _NumeroAtto
                End Get
                Set(ByVal Value As String)
                    _NumeroAtto = Value
                End Set
            End Property
            Public Property DataNotificaAtto() As DateTime
                Get
                    Return _DataNotificaAtto
                End Get
                Set(ByVal Value As DateTime)
                    _DataNotificaAtto = Value
                End Set
            End Property
        End Class
        Public Class E00
            Public Const TipoRecord As String = "E00"
            Public Const Filler As String = ""
            Private _IdentificativoFlusso As IdentificativoFlusso
            Private _DataCreazioneFlusso As DateTime = DateTime.MaxValue
            Private _EstremiFlusso As Integer
            Public Class Length
                Public Const DataCreazioneFlusso As Integer = 8
                Public Const EstremiFlusso As Integer = 10
                Public Const Filler As Integer = 567
            End Class
            Public Property IdentificativoFlusso() As IdentificativoFlusso
                Get
                    Return _IdentificativoFlusso
                End Get
                Set(ByVal Value As IdentificativoFlusso)
                    _IdentificativoFlusso = Value
                End Set
            End Property
            Public Property DataCreazioneFlusso() As DateTime
                Get
                    Return _DataCreazioneFlusso
                End Get
                Set(ByVal Value As DateTime)
                    _DataCreazioneFlusso = Value
                End Set
            End Property
            Public Property EstremiFlusso() As Integer
                Get
                    Return _EstremiFlusso
                End Get
                Set(ByVal Value As Integer)
                    _EstremiFlusso = Value
                End Set
            End Property
        End Class
        Public Class E20
            Public Const TipoRecord As String = "E20"
            Public Const PresenzaUlterioriDestinatariAtto As String = ""
            Public Const Filler As String = ""
            Private _ProgressivoRecord As Integer
            Private _IdentificativoAtto As IdentificativoAtto
            Private _PresenzaCoobligati As String
            Private _NaturaSoggetto As String
            Private _CodiceFiscale As String
            Private _CognomeDenominazione As String
            Private _Nome As String
            Private _Sesso As String
            Private _DataNascita As DateTime
            Private _CodiceCatastaleComuneNascita As String
            Private _ProvinciaNascita As String
            Private _CodiceCatastaleComuneDomicilioFiscale As String
            Private _ComuneDomicilioFiscale As String
            Private _ProvinciaDomicilioFiscale As String
            Private _CAPDomicilioFiscale As Integer
            Private _Indirizzo As String
            Private _NumeroCivico As Integer
            Private _LetteraNumeroCivico As String
            Private _Chilometro As Integer
            Private _Palazzina As String
            Private _Scala As String
            Private _Piano As String
            Private _Interno As String
            Private _LocalitaFrazione As String
            Public Class Length
                Public Const ProgressivoRecord As Integer = 7
                Public Const PresenzaUlterioriDestinatariAtto As Integer = 1
                Public Const PresenzaCoobligati As Integer = 1
                Public Const NaturaSoggetto As Integer = 1
                Public Const CodiceFiscale As Integer = 16
                Public Const CognomeDenominazione As Integer = 80
                Public Const Nome As Integer = 40
                Public Const Sesso As Integer = 1
                Public Const DataNascita As Integer = 8
                Public Const CodiceCatastaleComuneNascita As Integer = 4
                Public Const ProvinciaNascita As Integer = 2
                Public Const CodiceCatastaleComuneDomicilioFiscale As Integer = 4
                Public Const ComuneDomicilioFiscale As Integer = 45
                Public Const ProvinciaDomicilioFiscale As Integer = 2
                Public Const CAPDomicilioFiscale As Integer = 5
                Public Const Indirizzo As Integer = 80
                Public Const NumeroCivico As Integer = 5
                Public Const LetteraNumeroCivico As Integer = 2
                Public Const Chilometro As Integer = 6
                Public Const Palazzina As Integer = 3
                Public Const Scala As Integer = 3
                Public Const Piano As Integer = 3
                Public Const Interno As Integer = 4
                Public Const LocalitaFrazione As Integer = 25
                Public Const Filler As Integer = 148
            End Class
            Public Property ProgressivoRecord() As Integer
                Get
                    Return _ProgressivoRecord
                End Get
                Set(ByVal Value As Integer)
                    _ProgressivoRecord = Value
                End Set
            End Property
            Public Property IdentificativoAtto() As IdentificativoAtto
                Get
                    Return _IdentificativoAtto
                End Get
                Set(ByVal Value As IdentificativoAtto)
                    _IdentificativoAtto = Value
                End Set
            End Property
            Public Property PresenzaCoobligati() As String
                Get
                    Return _PresenzaCoobligati
                End Get
                Set(ByVal Value As String)
                    _PresenzaCoobligati = Value
                End Set
            End Property
            Public Property NaturaSoggetto() As String
                Get
                    Return _NaturaSoggetto
                End Get
                Set(ByVal Value As String)
                    _NaturaSoggetto = Value
                End Set
            End Property
            Public Property CodiceFiscale() As String
                Get
                    Return _CodiceFiscale
                End Get
                Set(ByVal Value As String)
                    _CodiceFiscale = Value
                End Set
            End Property
            Public Property CognomeDenominazione() As String
                Get
                    Return _CognomeDenominazione
                End Get
                Set(ByVal Value As String)
                    _CognomeDenominazione = Value
                End Set
            End Property
            Public Property Nome() As String
                Get
                    Return _Nome
                End Get
                Set(ByVal Value As String)
                    _Nome = Value
                End Set
            End Property
            Public Property Sesso() As String
                Get
                    Return _Sesso
                End Get
                Set(ByVal Value As String)
                    _Sesso = Value
                End Set
            End Property
            Public Property DataNascita() As DateTime
                Get
                    Return _DataNascita
                End Get
                Set(ByVal Value As DateTime)
                    _DataNascita = Value
                End Set
            End Property
            Public Property CodiceCatastaleComuneNascita() As String
                Get
                    Return _CodiceCatastaleComuneNascita
                End Get
                Set(ByVal Value As String)
                    _CodiceCatastaleComuneNascita = Value
                End Set
            End Property
            Public Property ProvinciaNascita() As String
                Get
                    Return _ProvinciaNascita
                End Get
                Set(ByVal Value As String)
                    _ProvinciaNascita = Value
                End Set
            End Property
            Public Property CodiceCatastaleComuneDomicilioFiscale() As String
                Get
                    Return _CodiceCatastaleComuneDomicilioFiscale
                End Get
                Set(ByVal Value As String)
                    _CodiceCatastaleComuneDomicilioFiscale = Value
                End Set
            End Property
            Public Property ComuneDomicilioFiscale() As String
                Get
                    Return _ComuneDomicilioFiscale
                End Get
                Set(ByVal Value As String)
                    _ComuneDomicilioFiscale = Value
                End Set
            End Property
            Public Property ProvinciaDomicilioFiscale() As String
                Get
                    Return _ProvinciaDomicilioFiscale
                End Get
                Set(ByVal Value As String)
                    _ProvinciaDomicilioFiscale = Value
                End Set
            End Property
            Public Property CAPDomicilioFiscale() As Integer
                Get
                    Return _CAPDomicilioFiscale
                End Get
                Set(value As Integer)
                    _CAPDomicilioFiscale = value
                End Set
            End Property
            Public Property Indirizzo() As String
                Get
                    Return _Indirizzo
                End Get
                Set(ByVal Value As String)
                    _Indirizzo = Value
                End Set
            End Property
            Public Property NumeroCivico() As Integer
                Get
                    Return _NumeroCivico
                End Get
                Set(ByVal Value As Integer)
                    _NumeroCivico = Value
                End Set
            End Property
            Public Property LetteraNumeroCivico() As String
                Get
                    Return _LetteraNumeroCivico
                End Get
                Set(ByVal Value As String)
                    _LetteraNumeroCivico = Value
                End Set
            End Property
            Public Property Chilometro() As Integer
                Get
                    Return _Chilometro
                End Get
                Set(ByVal Value As Integer)
                    _Chilometro = Value
                End Set
            End Property
            Public Property Palazzina() As String
                Get
                    Return _Palazzina
                End Get
                Set(ByVal Value As String)
                    _Palazzina = Value
                End Set
            End Property
            Public Property Scala() As String
                Get
                    Return _Scala
                End Get
                Set(ByVal Value As String)
                    _Scala = Value
                End Set
            End Property
            Public Property Piano() As String
                Get
                    Return _Piano
                End Get
                Set(ByVal Value As String)
                    _Piano = Value
                End Set
            End Property
            Public Property Interno() As String
                Get
                    Return _Interno
                End Get
                Set(ByVal Value As String)
                    _Interno = Value
                End Set
            End Property
            Public Property LocalitaFrazione() As String
                Get
                    Return _LocalitaFrazione
                End Get
                Set(ByVal Value As String)
                    _LocalitaFrazione = Value
                End Set
            End Property
        End Class
        Public Class E23
            Public Const TipoRecord As String = "E23"
            Public Const Filler As String = ""
            Private _ProgressivoRecord As Integer
            Private _IdentificativoAtto As IdentificativoAtto
            Private _CodiceFiscaleSoggettoIntestatario As String
            Private _CodiceFiscaleUlterioreSoggettoNotifica As String
            Private _Cognome As String
            Private _Nome As String
            Private _Sesso As String
            Private _DataNascita As DateTime
            Private _CodiceCatastaleComuneNascita As String
            Private _ProvinciaNascita As String
            Private _CodiceCatastaleComuneDomicilioFiscale As String
            Private _ComuneDomicilioFiscale As String
            Private _ProvinciaDomicilioFiscale As String
            Private _CAPDomicilioFiscale As Integer
            Private _Indirizzo As String
            Private _NumeroCivico As Integer
            Private _LetteraNumeroCivico As String
            Private _Chilometro As Integer
            Private _Palazzina As String
            Private _Scala As String
            Private _Piano As String
            Private _Interno As String
            Private _LocalitaFrazione As String
            Public Class Length
                Public Const ProgressivoRecord As Integer = 7
                Public Const CodiceFiscaleSoggettoIntestatario As Integer = 16
                Public Const CodiceFiscaleUlterioreSoggettoNotifica As Integer = 16
                Public Const Cognome As Integer = 50
                Public Const Nome As Integer = 40
                Public Const Sesso As Integer = 1
                Public Const DataNascita As Integer = 8
                Public Const CodiceCatastaleComuneNascita As Integer = 4
                Public Const ProvinciaNascita As Integer = 2
                Public Const CodiceCatastaleComuneDomicilioFiscale As Integer = 4
                Public Const ComuneDomicilioFiscale As Integer = 45
                Public Const ProvinciaDomicilioFiscale As Integer = 2
                Public Const CAPDomicilioFiscale As Integer = 5
                Public Const Indirizzo As Integer = 80
                Public Const NumeroCivico As Integer = 5
                Public Const LetteraNumeroCivico As Integer = 2
                Public Const Chilometro As Integer = 6
                Public Const Palazzina As Integer = 3
                Public Const Scala As Integer = 3
                Public Const Piano As Integer = 3
                Public Const Interno As Integer = 4
                Public Const LocalitaFrazione As Integer = 25
                Public Const Filler As Integer = 165
            End Class
            Public Property ProgressivoRecord() As Integer
                Get
                    Return _ProgressivoRecord
                End Get
                Set(ByVal Value As Integer)
                    _ProgressivoRecord = Value
                End Set
            End Property
            Public Property IdentificativoAtto() As IdentificativoAtto
                Get
                    Return _IdentificativoAtto
                End Get
                Set(ByVal Value As IdentificativoAtto)
                    _IdentificativoAtto = Value
                End Set
            End Property
            Public Property CodiceFiscaleSoggettoIntestatario() As String
                Get
                    Return _CodiceFiscaleSoggettoIntestatario
                End Get
                Set(ByVal Value As String)
                    _CodiceFiscaleSoggettoIntestatario = Value
                End Set
            End Property
            Public Property CodiceFiscaleUlterioreSoggettoNotifica() As String
                Get
                    Return _CodiceFiscaleUlterioreSoggettoNotifica
                End Get
                Set(ByVal Value As String)
                    _CodiceFiscaleUlterioreSoggettoNotifica = Value
                End Set
            End Property
            Public Property Cognome() As String
                Get
                    Return _Cognome
                End Get
                Set(ByVal Value As String)
                    _Cognome = Value
                End Set
            End Property
            Public Property Nome() As String
                Get
                    Return _Nome
                End Get
                Set(ByVal Value As String)
                    _Nome = Value
                End Set
            End Property
            Public Property Sesso() As String
                Get
                    Return _Sesso
                End Get
                Set(ByVal Value As String)
                    _Sesso = Value
                End Set
            End Property
            Public Property DataNascita() As DateTime
                Get
                    Return _DataNascita
                End Get
                Set(ByVal Value As DateTime)
                    _DataNascita = Value
                End Set
            End Property
            Public Property CodiceCatastaleComuneNascita() As String
                Get
                    Return _CodiceCatastaleComuneNascita
                End Get
                Set(ByVal Value As String)
                    _CodiceCatastaleComuneNascita = Value
                End Set
            End Property
            Public Property ProvinciaNascita() As String
                Get
                    Return _ProvinciaNascita
                End Get
                Set(ByVal Value As String)
                    _ProvinciaNascita = Value
                End Set
            End Property
            Public Property CodiceCatastaleComuneDomicilioFiscale() As String
                Get
                    Return _CodiceCatastaleComuneDomicilioFiscale
                End Get
                Set(ByVal Value As String)
                    _CodiceCatastaleComuneDomicilioFiscale = Value
                End Set
            End Property
            Public Property ComuneDomicilioFiscale() As String
                Get
                    Return _ComuneDomicilioFiscale
                End Get
                Set(ByVal Value As String)
                    _ComuneDomicilioFiscale = Value
                End Set
            End Property
            Public Property ProvinciaDomicilioFiscale() As String
                Get
                    Return _ProvinciaDomicilioFiscale
                End Get
                Set(ByVal Value As String)
                    _ProvinciaDomicilioFiscale = Value
                End Set
            End Property
            Public Property CAPDomicilioFiscale() As Integer
                Get
                    Return _CAPDomicilioFiscale
                End Get
                Set(ByVal Value As Integer)
                    _CAPDomicilioFiscale = Value
                End Set
            End Property
            Public Property Indirizzo() As String
                Get
                    Return _Indirizzo
                End Get
                Set(ByVal Value As String)
                    _Indirizzo = Value
                End Set
            End Property
            Public Property NumeroCivico() As Integer
                Get
                    Return _NumeroCivico
                End Get
                Set(ByVal Value As Integer)
                    _NumeroCivico = Value
                End Set
            End Property
            Public Property LetteraNumeroCivico() As String
                Get
                    Return _LetteraNumeroCivico
                End Get
                Set(ByVal Value As String)
                    _LetteraNumeroCivico = Value
                End Set
            End Property
            Public Property Chilometro() As Integer
                Get
                    Return _Chilometro
                End Get
                Set(ByVal Value As Integer)
                    _Chilometro = Value
                End Set
            End Property
            Public Property Palazzina() As String
                Get
                    Return _Palazzina
                End Get
                Set(ByVal Value As String)
                    _Palazzina = Value
                End Set
            End Property
            Public Property Scala() As String
                Get
                    Return _Scala
                End Get
                Set(ByVal Value As String)
                    _Scala = Value
                End Set
            End Property
            Public Property Piano() As String
                Get
                    Return _Piano
                End Get
                Set(ByVal Value As String)
                    _Piano = Value
                End Set
            End Property
            Public Property Interno() As String
                Get
                    Return _Interno
                End Get
                Set(ByVal Value As String)
                    _Interno = Value
                End Set
            End Property
            Public Property LocalitaFrazione() As String
                Get
                    Return _LocalitaFrazione
                End Get
                Set(ByVal Value As String)
                    _LocalitaFrazione = Value
                End Set
            End Property
        End Class
        Public Class E50
            Public Const TipoRecord As String = "E50"
            Public Const PeriodoRiferimento As Integer = 99
            Public Const Filler As String = ""
            Private _ProgressivoRecord As Integer
            Private _IdentificativoAtto As IdentificativoAtto
            Private _ProgressivoArticolo As Integer
            Private _CodiceEntrata As String
            Private _TipoCodiceEntrata As String
            Private _ImportoArticolo As Integer
            Public Class Length
                Public Const ProgressivoRecord As Integer = 7
                Public Const ProgressivoArticolo As Integer = 3
                Public Const PeriodoRiferimento As Integer = 6
                Public Const CodiceEntrata As Integer = 4
                Public Const TipoCodiceEntrata As Integer = 1
                Public Const ImportoArticolo As Integer = 15
                Public Const Filler As Integer = 460
            End Class
            Public Property ProgressivoRecord() As Integer
                Get
                    Return _ProgressivoRecord
                End Get
                Set(ByVal Value As Integer)
                    _ProgressivoRecord = Value
                End Set
            End Property
            Public Property IdentificativoAtto() As IdentificativoAtto
                Get
                    Return _IdentificativoAtto
                End Get
                Set(ByVal Value As IdentificativoAtto)
                    _IdentificativoAtto = Value
                End Set
            End Property
            Public Property ProgressivoArticolo() As Integer
                Get
                    Return _ProgressivoArticolo
                End Get
                Set(ByVal Value As Integer)
                    _ProgressivoArticolo = Value
                End Set
            End Property
            Public Property CodiceEntrata() As String
                Get
                    Return _CodiceEntrata
                End Get
                Set(ByVal Value As String)
                    _CodiceEntrata = Value
                End Set
            End Property
            Public Property TipoCodiceEntrata() As String
                Get
                    Return _TipoCodiceEntrata
                End Get
                Set(ByVal Value As String)
                    _TipoCodiceEntrata = Value
                End Set
            End Property
            Public Property ImportoArticolo() As Integer
                Get
                    Return _ImportoArticolo
                End Get
                Set(ByVal Value As Integer)
                    _ImportoArticolo = Value
                End Set
            End Property
        End Class
        Public Class E60
            Public Const TipoRecord As String = "E60"
            Public Const TipologiaSospensione As Integer = 3
            Public Const FlagEnteTerzo As String = "N"
            Public Const DenominazioneEnteTerzo As String = ""
            Public Const Filler As String = ""
            Private _ProgressivoRecord As Integer
            Private _IdentificativoAtto As IdentificativoAtto
            Private _NumeroDelibera As String
            Private _DataDelibera As DateTime
            Private _DataFineValiditaDelibera As DateTime
            Private _ImportoTotaleAtto As Integer
            Private _TotaleArticoliAtto As Integer
            Private _DataTermineUltimoPagamentoAtto As DateTime
            Public Class Length
                Public Const ProgressivoRecord As Integer = 7
                Public Const NumeroDelibera As Integer = 10
                Public Const DataDelibera As Integer = 8
                Public Const DataFineValiditaDelibera As Integer = 8
                Public Const TipologiaSospensione As Integer = 1
                Public Const ImportoTotaleAtto As Integer = 15
                Public Const TotaleArticoliAtto As Integer = 7
                Public Const DataTermineUltimoPagamentoAtto As Integer = 8
                Public Const FlagEnteTerzo As Integer = 1
                Public Const DenominazioneEnteTerzo As Integer = 60
                Public Const Filler As Integer = 371
            End Class
            Public Property ProgressivoRecord() As Integer
                Get
                    Return _ProgressivoRecord
                End Get
                Set(ByVal Value As Integer)
                    _ProgressivoRecord = Value
                End Set
            End Property
            Public Property IdentificativoAtto() As IdentificativoAtto
                Get
                    Return _IdentificativoAtto
                End Get
                Set(ByVal Value As IdentificativoAtto)
                    _IdentificativoAtto = Value
                End Set
            End Property
            Public Property NumeroDelibera() As String
                Get
                    Return _NumeroDelibera
                End Get
                Set(ByVal Value As String)
                    _NumeroDelibera = Value
                End Set
            End Property
            Public Property DataDelibera() As DateTime
                Get
                    Return _DataDelibera
                End Get
                Set(ByVal Value As DateTime)
                    _DataDelibera = Value
                End Set
            End Property
            Public Property DataFineValiditaDelibera() As DateTime
                Get
                    Return _DataFineValiditaDelibera
                End Get
                Set(ByVal Value As DateTime)
                    _DataFineValiditaDelibera = Value
                End Set
            End Property
            Public Property ImportoTotaleAtto() As Integer
                Get
                    Return _ImportoTotaleAtto
                End Get
                Set(ByVal Value As Integer)
                    _ImportoTotaleAtto = Value
                End Set
            End Property
            Public Property TotaleArticoliAtto() As Integer
                Get
                    Return _TotaleArticoliAtto
                End Get
                Set(ByVal Value As Integer)
                    _TotaleArticoliAtto = Value
                End Set
            End Property
            Public Property DataTermineUltimoPagamentoAtto() As DateTime
                Get
                    Return _DataTermineUltimoPagamentoAtto
                End Get
                Set(ByVal Value As DateTime)
                    _DataTermineUltimoPagamentoAtto = Value
                End Set
            End Property
        End Class
        Public Class E99
            Public Const TipoRecord As String = "E99"
            Public Const Filler As String = ""
            Private _IdentificativoFlusso As IdentificativoFlusso
            Private _DataCreazioneFlusso As DateTime
            Private _EstremiFlusso As Integer
            Private _TotaleRecord As Integer
            Private _TotaleRecordE20 As Integer
            Private _TotaleRecordE23 As Integer
            Private _TotaleRecordE50 As Integer
            Private _TotaleRecordE60 As Integer
            Private _TotaleCaricoFlusso As Integer
            Public Class Length
                Public Const DataCreazioneFlusso As Integer = 8
                Public Const EstremiFlusso As Integer = 10
                Public Const TotaleRecord As Integer = 7
                Public Const TotaleRecordE20 As Integer = 7
                Public Const TotaleRecordE23 As Integer = 7
                Public Const TotaleRecordE50 As Integer = 7
                Public Const TotaleRecordE60 As Integer = 7
                Public Const TotaleCaricoFlusso As Integer = 15
                Public Const Filler As Integer = 517
            End Class
            Public Property IdentificativoFlusso() As IdentificativoFlusso
                Get
                    Return _IdentificativoFlusso
                End Get
                Set(ByVal Value As IdentificativoFlusso)
                    _IdentificativoFlusso = Value
                End Set
            End Property
            Public Property DataCreazioneFlusso() As DateTime
                Get
                    Return _DataCreazioneFlusso
                End Get
                Set(ByVal Value As DateTime)
                    _DataCreazioneFlusso = Value
                End Set
            End Property
            Public Property EstremiFlusso() As Integer
                Get
                    Return _EstremiFlusso
                End Get
                Set(ByVal Value As Integer)
                    _EstremiFlusso = Value
                End Set
            End Property
            Public Property TotaleRecord() As Integer
                Get
                    Return _TotaleRecord
                End Get
                Set(ByVal Value As Integer)
                    _TotaleRecord = Value
                End Set
            End Property
            Public Property TotaleRecordE20() As Integer
                Get
                    Return _TotaleRecordE20
                End Get
                Set(ByVal Value As Integer)
                    _TotaleRecordE20 = Value
                End Set
            End Property
            Public Property TotaleRecordE23() As Integer
                Get
                    Return _TotaleRecordE23
                End Get
                Set(ByVal Value As Integer)
                    _TotaleRecordE23 = Value
                End Set
            End Property
            Public Property TotaleRecordE50() As Integer
                Get
                    Return _TotaleRecordE50
                End Get
                Set(ByVal Value As Integer)
                    _TotaleRecordE50 = Value
                End Set
            End Property
            Public Property TotaleRecordE60() As Integer
                Get
                    Return _TotaleRecordE60
                End Get
                Set(ByVal Value As Integer)
                    _TotaleRecordE60 = Value
                End Set
            End Property
            Public Property TotaleCaricoFlusso() As Integer
                Get
                    Return _TotaleCaricoFlusso
                End Get
                Set(ByVal Value As Integer)
                    _TotaleCaricoFlusso = Value
                End Set
            End Property
        End Class
    End Class
    Public Class clsAccertamentiEsecutivi
        ''' <summary>
        '''1 Caratteristiche
        '''1.1 Caratteristiche del file
        '''Il flusso di carico (tracciato lunghezza record 600) da accertamenti esecutivi con cui l’ente può
        '''affidare l’attività di riscossione forzata al soggetto legittimato alla riscossione deve rispettare le
        '''seguenti caratteristiche:
        ''' - Tipo di codifica ASCII
        ''' - Bloccato L.R. 600
        ''' - Nome del File: CCCCCSSAA_NNN.txt dove
        '''CCCCC = Codice ente creditore
        '''SSAA = Anno emissione su 4 posizioni
        '''NNN = Numero progressivo del file all’interno dell’anno
        '''.txt = Estensione file di testo
        '''1.2 Rappresentazione dati e controlli
        '''Tutti i dati previsti dalla procedura vengono illustrati nelle specifiche sezioni del presente documento;
        '''in particolare, vengono descritti i vari tipi record che compongono il flusso dati, fornendo per ciascun
        '''campo i seguenti elementi:
        '''• Id. Identificativo del campo progressivo numerico;
        '''• Da Posizione dell’inizio del campo nell’ambito del record;
        '''• A Posizione della fine del campo nell’ambito del record;
        '''• Lungh. Numero dei byte del campo;
        '''• Descr. Contiene le informazioni del campo, il suo significato e la sua valorizzazione;
        '''• Tipo Tipo di rappresentazione del campo, assume la seguente codifica:
        '''i. N per campo numerico,
        '''ii. A per campo alfabetico,
        '''iii. AN per campo alfanumerico;
        '''• Obbl. Indica se il campo è ritenuto obbligatorio o meno, assume la seguente
        '''codifica:
        '''i. S per campo obbligatorio;
        '''ii. N per campo facoltativo;
        '''• Errore Indica il codice con cui viene segnalato l’errore riscontrato;
        '''il significato dell’errore viene riportato nella Tabella codici di errore –
        '''APPENDICE A . Il codice – NC – significa che il campo NON è controllato.
        '''A livello generale valgono inoltre le seguenti convenzioni:
        '''• Nei campi di tipo A e AN, le lettere sono rappresentate con caratteri maiuscoli;
        '''• I filler sono valorizzati con spazio;
        '''• I campi numerici, se non significativi, sono valorizzati con “0”;
        '''• I campi numerici, se significativi, sono allineati a destra con i rimanenti caratteri valorizzati
        '''con “0”;
        '''• i campi non numerici, se non significativi, sono valorizzati con spazio;
        '''• i campi non numerici, se significativi, sono allineati a sinistra con i rimanenti caratteri
        '''valorizzati con spazio;
        '''• i campi vengono sottoposti a controlli formali secondo il tipo di rappresentazione; tale
        '''tipo di controllo, per brevità non indicato su ciascun singolo campo, può dare luogo alle
        '''segnalazioni di errore contenute nell’appendice A;
        '''• valgono inoltre le seguenti convenzioni per gli errori:
        '''o SF = Scarto Flusso – prevede lo scarto dell’intero flusso dati;
        '''o SP= Scarto Atto – prevede lo scarto del singolo atto;
        '''1.3 Rappresentazione del Flusso
        '''Il flusso logico è composto dai seguenti tipi di record aventi tutti lunghezza pari a 600 byte.
        '''Seq. Tipo Record Descrizione Tipo record
        '''1 E00 Record Inizio Flusso
        '''2 E20 Record Anagrafica Intestatario Atto
        '''3 E23 Record Anagrafica Ulteriore Soggetto
        '''4 E50 Record Dati Contabili Atto
        '''5 E60 Record Ulteriori Dati Atto
        '''6 E99 Record Fine Flusso
        '''2 Sui flussi ricevuti a seguito di accertamento enti, si effettua, oltre ai controlli riportati nei singoli campi
        '''dei vari tipi record, anche i controlli riportati nei seguenti paragrafi.
        '''2.1 Controlli di struttura
        '''La struttura del flusso è logicamente suddivisa in quattro parti: la prima relativa alle informazioni iniziali
        '''del flusso, la seconda che riguarda la parte anagrafica, la terza la parte contabile e l’ultima riporta
        '''le informazioni finali.
        '''La codifica dei tipi records segue questa logica: E00 e E99 sono le informazioni iniziali e finali, E20 ed
        '''E23 riguardano le informazioni anagrafiche. Nel tipo record E50 sono previste le informazioni
        '''contabili. Nel tipo record E60 sono le informazioni aggiuntive sull’atto.
        '''I tipi records ammessi sono:
        '''• E00 Inizio file;
        '''• E20 Anagrafica – Intestatario Atto/Coobbligato;
        '''• E23 Anagrafica – Rappresentante, Erede, Curatore ecc.;
        '''• E50 Contabile – Dati Contabili Atto (Articolo);
        '''• E60 Ulteriori dati atto;
        '''• E99 Fine file;
        '''2.2 Controlli di sequenza dei tipi records
        '''I controlli di sequenza dei tipi records all’interno del flusso riguardano:
        '''• il flusso deve cominciare con un record di Inizio E00;
        '''• il flusso deve terminare con un record di Fine E99;
        '''• il flusso deve contenere almeno un’Anagrafica Intestatario Atto E20;
        '''• il flusso deve contenere almeno un Contabile Atto E50;
        '''• il flusso deve contenere almeno un ulteriore dato atto E60;
        '''• Il record di testa E00 deve essere seguito da un record E20;
        '''• Il record E20 (intestatario) deve essere seguito da:
        '''o Se flag presenza ulteriori intestatari impostato
        '''Segue record E23, altrimenti
        '''o Se flag presenza coobbligati = 2
        '''Segue record E20 (coobbligato), altrimenti
        ''' Segue E50
        '''• Il record E50 deve essere seguito da un record E50-E60;
        '''• Il record E60 deve essere seguito da un record E20;
        '''• Ogni flusso logico deve finire con un record di Coda “E99”;
        '''2.3 Controlli di quadratura
        '''Sono effettuati anche controlli di quadratura, sia in ordine al numero dei tipi records presenti nel
        '''flusso che relativi al carico presente nel record Contabile Atto.
        '''• I totali dei record E20-E23-E50-E60 devono essere congruenti con quelli presenti
        '''sull’archivio;
        '''• I totali del carico devono essere congruenti con quelli presenti sull’archivio;
        '''2.4 Controlli di congruenza
        '''Dati intero flusso:
        '''• L’estremo della fornitura deve essere univoco nell’ambito dell’Ente;
        '''• L’estremo della fornitura deve essere uguale sui record di testa e di coda;
        '''• La data di creazione del file deve essere uguale sui record di testa e di coda;
        '''• Il progressivo record deve essere strettamente crescente;
        '''• Il codice Ente/Tipo Ufficio/Ufficio deve essere uguale per tutta la fornitura;
        '''• Per ogni soggetto la somma degli importi dei singoli atti deve essere maggiore di 10 €;
        '''Dati singolo atto:
        '''• Il numero partita deve essere crescente;
        '''• Anagrafica:
        '''o Codice fiscale intestatario/coobbligato non validato da Anagrafe Tributaria;
        '''o Codice fiscale non corrispondente con anagrafica;
        '''o Codice fiscale non corrispondente con tipologia debitore (persona fisica/ non fisica);
        '''o Codice fiscale valido intestato a soggetto minore alla data di affidamento del carico,
        '''privo dell’anagrafica del tutore;
        '''o Codice fiscale valido ma privo di indirizzo fiscale in Anagrafe Tributaria, in quanto:
        '''- in Anagrafe Tributaria è presente esclusivamente un indirizzo estero;
        '''- in sede di «validazione» in Anagrafe Tributaria nel campo relativo all’indirizzo
        '''di domicilio fiscale, compare la dizione «irreperibile» o altra dizione che non
        '''consentono di individuare l’indirizzo fisico;
        '''• Carico singolo atto: l’Importo totale del carico affidato per singolo atto deve essere superiore
        '''a zero;
        '''• DATE singolo atto:
        '''o La data notifica atto e data termine ultimo pagamento devono essere non
        '''antecedenti al 01 gennaio 2020;
        '''o La data del termine ultimo di pagamento e la data di efficacia esecutiva dell’atto
        '''devono essere successive alla data di notifica atto;
        '''o La data di notifica atto deve essere inferiore di almeno 60 gg rispetto alla data di
        '''trasmissione del flusso di carico; 
        ''' </summary>
        ''' <param name="myStringConnection"></param>
        ''' <param name="IdRuolo"></param>
        ''' <param name="sPathFile"></param>
        ''' <param name="sNameFile"></param>
        ''' <param name="ListFile"></param>
        ''' <returns></returns>
        Public Function CreaAccertamentiEsecutivi(ByVal myStringConnection As String, IdRuolo As Integer, sPathFile As String, ByVal sNameFile As String, ByRef ListFile As ArrayList) As Integer
            Dim myDataView As New DataView
            Dim myIdFlusso As New objAccertamentiEsecutivi.IdentificativoFlusso
            Dim myIdAtto As New objAccertamentiEsecutivi.IdentificativoAtto
            Dim myE00 As New objAccertamentiEsecutivi.E00
            Dim myE20 As New objAccertamentiEsecutivi.E20
            Dim myE23 As New objAccertamentiEsecutivi.E23
            Dim myE50 As New objAccertamentiEsecutivi.E50
            Dim myE60 As New objAccertamentiEsecutivi.E60
            Dim myE99 As New objAccertamentiEsecutivi.E99
            Dim ListE20 As New ArrayList
            Dim ListE23 As New ArrayList
            Dim ListE50 As New ArrayList
            Dim ListE60 As New ArrayList
            Dim nTotRC As Integer = 0
            Dim nTotE20 As Integer = 0
            Dim nTotE23 As Integer = 0
            Dim nTotE50 As Integer = 0
            Dim nTotE60 As Integer = 0
            Dim impTotAccertamentiEsecutivi As Double = 0
            Dim AttoPrec As String = ""

            Try
                ListFile = New ArrayList
                'prelevo gli articoli da estrarre
                myDataView = GetPosizioniAccertamentiEsecutivi(myStringConnection, IdRuolo)
                If Not myDataView Is Nothing Then
                    'ciclo su tutti gli articoli trovati e preparo il AccertamentiEsecutivi
                    For Each myRow As DataRowView In myDataView
                        If myIdFlusso.CodiceEnteCreditore = "" Or myIdFlusso.CodiceEnteCreditore = "0" Then
                            'popolo i dati identificativi del flusso
                            myIdFlusso = PopolaIdFlusso(myRow)
                            'popolo il record E00
                            myE00 = PopolaE00(myRow, myIdFlusso, nTotRC)
                            If myE00 Is Nothing Then
                                Return -1
                            End If
                        End If
                        If myRow("numeroatto") <> AttoPrec Then
                            If myE20.ProgressivoRecord > 0 Then
                                ListE20.Add(myE20)
                                If Not myE20 Is Nothing Then
                                    nTotE20 += 1
                                Else
                                    Return -1
                                End If
                                ListE23.Add(myE23)
                                If Not myE23 Is Nothing Then
                                    nTotE23 += 1
                                Else
                                    Return -1
                                End If
                                nTotRC += 1
                                myE60.ProgressivoRecord = nTotRC
                                ListE60.Add(myE60)
                                nTotE60 += 1
                                impTotAccertamentiEsecutivi += myRow("ImportoTotaleAtto")
                            End If
                            myIdAtto = PopolaIdentificativoAtto(myRow, myIdFlusso)
                            nTotRC += 1
                            myE20 = PopolaE20(myRow, myIdAtto, nTotRC)
                            nTotRC += 1
                            myE23 = PopolaE23(myRow, myIdAtto, nTotRC)
                        End If
                        nTotRC += 1
                        myE50 = PopolaE50(myRow, myIdAtto, nTotRC)
                        If Not myE50 Is Nothing Then
                            nTotE50 += 1
                        Else
                            Return -1
                        End If
                        ListE50.Add(myE50)
                        myE60 = PopolaE60(myRow, myIdAtto, nTotRC)
                        If myE60 Is Nothing Then
                            Return -1
                        End If
                        AttoPrec = myRow("numeroatto")
                    Next
                    myE99 = PopolaE99(myIdFlusso, myE00, nTotRC, nTotE20, nTotE23, nTotE50, nTotE60, impTotAccertamentiEsecutivi)

                    'scrivo il file
                    If CreateAccertamentiEsecutivi(sPathFile & sNameFile, myE00, ListE20, ListE23, ListE50, ListE60, myE99) < 1 Then
                        Return -1
                    End If
                    ListFile.Add(sPathFile & sNameFile)
                    If New cls290().ZipFile(sPathFile, sNameFile.Replace(".txt", ".zip"), ListFile) = False Then
                        Return 0
                    End If
                Else
                    Return 0
                End If

                Return 1
            Catch Err As Exception
                Log.Debug(IdRuolo.ToString() + " - OPENgovPROVVEDIMENTI.clsCoattivo.CreaobjAccertamentiEsecutivi.errore::" + Err.Message)
                Return -1
            End Try
        End Function
        Public Function GetPosizioniAccertamentiEsecutivi(ByVal myStringConnection As String, IdRuolo As Integer) As DataView
            Dim sSQL As String
            Dim myDataView As New DataView

            Try
                Using ctx As New DBModel(ConstSession.DBType, myStringConnection)
                    Try
                        sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetPosizioniAccertamentiEsecutivi", "IdRuolo")
                        myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IdRuolo", IdRuolo)
                        )
                    Catch ex As Exception
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsCoattivo.GetPosizioniobjAccertamentiEsecutivi.erroreQuery: ", ex)
                        Return Nothing
                    Finally
                        ctx.Dispose()
                    End Try
                End Using
                Return myDataView
            Catch Err As Exception
                Log.Debug(IdRuolo.ToString() + " - OPENgovPROVVEDIMENTI.clsCoattivo.GetPosizioniobjAccertamentiEsecutivi.errore: ", Err)
                Return Nothing
            End Try
        End Function
        Public Function PopolaIdFlusso(myRow As DataRowView) As objAccertamentiEsecutivi.IdentificativoFlusso
            Dim myItem As New objAccertamentiEsecutivi.IdentificativoFlusso

            Try
                myItem.CodiceEnteCreditore = New MyUtility().ReplaceCharForFile(myRow("CodiceEnteCreditore"))
                myItem.TipoUfficio = New MyUtility().ReplaceCharForFile(myRow("TipoUfficio"))
                myItem.CodiceUfficio = New MyUtility().ReplaceCharForFile(myRow("CodiceUfficio"))

                Return myItem
            Catch Err As Exception
                Log.Debug(myRow("CodiceEnteCreditore").ToString() + " - OPENgovPROVVEDIMENTI.clsCoattivo.CreaAccertamentiEsecutivi::PopolaIdFlusso.errore::" + Err.Message)
                Return Nothing
            End Try
        End Function
        Public Function PopolaIdentificativoAtto(myRow As DataRowView, ByVal myIdFlusso As objAccertamentiEsecutivi.IdentificativoFlusso) As objAccertamentiEsecutivi.IdentificativoAtto
            Dim myItem As New objAccertamentiEsecutivi.IdentificativoAtto

            Try
                myItem.IdentificativoFlusso = myIdFlusso
                myItem.AnnoEmissioneAtto = New MyUtility().ReplaceCharForFile(myRow("AnnoEmissioneAtto"))
                myItem.NumeroPartita = New MyUtility().ReplaceCharForFile(myRow("NumeroPartita"))
                myItem.DataEmissioneAtto = myRow("DataEmissioneAtto")
                myItem.NumeroAtto = New MyUtility().ReplaceCharForFile(myRow("NumeroAtto"))
                myItem.DataNotificaAtto = myRow("DataNotificaAtto")

                Return myItem
            Catch Err As Exception
                Log.Debug(myIdFlusso.CodiceEnteCreditore + " - OPENgovPROVVEDIMENTI.clsCoattivo.CreaAccertamentiEsecutivi::PopolaIdentificativoAtto.errore::" + Err.Message)
                Return Nothing
            End Try
        End Function
        Public Function PopolaE00(myRow As DataRowView, ByVal myIdFlusso As objAccertamentiEsecutivi.IdentificativoFlusso, nRC As Integer) As objAccertamentiEsecutivi.E00
            Dim myItem As New objAccertamentiEsecutivi.E00

            Try
                myItem.IdentificativoFlusso = myIdFlusso
                myItem.DataCreazioneFlusso = Now
                myItem.EstremiFlusso = New MyUtility().ReplaceCharForFile(myRow("EstremiFlusso"))
                nRC += 1
                Return myItem
            Catch Err As Exception
                Log.Debug(myIdFlusso.CodiceEnteCreditore + " - OPENgovPROVVEDIMENTI.clsCoattivo.CreaAccertamentiEsecutivi::PopolaE00.errore::" + Err.Message)
                Return Nothing
            End Try
        End Function
        Public Function PopolaE20(myRow As DataRowView, myIdAtto As objAccertamentiEsecutivi.IdentificativoAtto, nRC As Integer) As objAccertamentiEsecutivi.E20
            Dim myItem As New objAccertamentiEsecutivi.E20

            Try
                myItem.IdentificativoAtto = myIdAtto
                myItem.ProgressivoRecord = nRC
                myItem.PresenzaCoobligati = New MyUtility().ReplaceCharForFile(myRow("PresCoobligati"))
                myItem.NaturaSoggetto = New MyUtility().ReplaceCharForFile(myRow("NaturaSoggetto"))
                myItem.CodiceFiscale = New MyUtility().ReplaceCharForFile(myRow("CodiceFiscale"))
                myItem.CognomeDenominazione = New MyUtility().ReplaceCharForFile(myRow("CognomeDenominazione"))
                myItem.Nome = New MyUtility().ReplaceCharForFile(myRow("Nome"))
                myItem.Sesso = New MyUtility().ReplaceCharForFile(myRow("Sesso"))
                myItem.DataNascita = New MyUtility().ReplaceCharForFile(myRow("DataNascita"))
                myItem.CodiceCatastaleComuneNascita = New MyUtility().ReplaceCharForFile(myRow("CodCatastaleNascita"))
                myItem.ProvinciaNascita = New MyUtility().ReplaceCharForFile(myRow("PVNascita"))
                myItem.CodiceCatastaleComuneDomicilioFiscale = New MyUtility().ReplaceCharForFile(myRow("CodCatastaleDomicilio"))
                myItem.ComuneDomicilioFiscale = New MyUtility().ReplaceCharForFile(myRow("ComuneDomicilio"))
                myItem.ProvinciaDomicilioFiscale = New MyUtility().ReplaceCharForFile(myRow("PVDomicilio"))
                myItem.CAPDomicilioFiscale = New MyUtility().ReplaceCharForFile(myRow("CAPDomicilio"))
                myItem.Indirizzo = New MyUtility().ReplaceCharForFile(myRow("Indirizzo"))
                myItem.NumeroCivico = New MyUtility().ReplaceCharForFile(myRow("Civico"))
                myItem.LetteraNumeroCivico = New MyUtility().ReplaceCharForFile(myRow("Lettera"))
                myItem.Chilometro = New MyUtility().ReplaceCharForFile(myRow("Chilometro"))
                myItem.Palazzina = New MyUtility().ReplaceCharForFile(myRow("Palazzina"))
                myItem.Scala = New MyUtility().ReplaceCharForFile(myRow("Scala"))
                myItem.Piano = New MyUtility().ReplaceCharForFile(myRow("Piano"))
                myItem.Interno = New MyUtility().ReplaceCharForFile(myRow("Interno"))
                myItem.LocalitaFrazione = New MyUtility().ReplaceCharForFile(myRow("Frazione"))
                Return myItem
            Catch Err As Exception
                Log.Debug(myIdAtto.IdentificativoFlusso.CodiceEnteCreditore + " - OPENgovPROVVEDIMENTI.clsCoattivo.CreaAccertamentiEsecutivi::PopolaE20.errore::" + Err.Message + " - COD_CONTRIBUENTE::" + myItem.ProgressivoRecord.ToString() + "|" + myIdAtto.NumeroAtto.ToString())
                Return Nothing
            End Try
        End Function
        Public Function PopolaE23(myRow As DataRowView, myIdAtto As objAccertamentiEsecutivi.IdentificativoAtto, nRC As Integer) As objAccertamentiEsecutivi.E23
            Dim myItem As New objAccertamentiEsecutivi.E23

            Try
                myItem.IdentificativoAtto = myIdAtto
                myItem.ProgressivoRecord = nRC
                myItem.CodiceFiscaleSoggettoIntestatario = New MyUtility().ReplaceCharForFile(myRow("CodFiscaleIntestatario"))
                myItem.CodiceFiscaleUlterioreSoggettoNotifica = New MyUtility().ReplaceCharForFile(myRow("CodFiscaleUltSog"))
                myItem.Cognome = New MyUtility().ReplaceCharForFile(myRow("CognomeUltSog"))
                myItem.Nome = New MyUtility().ReplaceCharForFile(myRow("NomeUltSog"))
                myItem.Sesso = New MyUtility().ReplaceCharForFile(myRow("SessoUltSog"))
                myItem.DataNascita = New MyUtility().ReplaceCharForFile(myRow("DataNascitaUltSog"))
                myItem.CodiceCatastaleComuneNascita = New MyUtility().ReplaceCharForFile(myRow("CodCatastaleNascitaUltSog"))
                myItem.ProvinciaNascita = New MyUtility().ReplaceCharForFile(myRow("PVNascitaUltSog"))
                myItem.CodiceCatastaleComuneDomicilioFiscale = New MyUtility().ReplaceCharForFile(myRow("CodCatastaleDomicilioUltSog"))
                myItem.ComuneDomicilioFiscale = New MyUtility().ReplaceCharForFile(myRow("ComuneDomicilioUltSog"))
                myItem.ProvinciaDomicilioFiscale = New MyUtility().ReplaceCharForFile(myRow("PVDomicilioUltSog"))
                myItem.CAPDomicilioFiscale = New MyUtility().ReplaceCharForFile(myRow("CAPDomicilioUltSog"))
                myItem.Indirizzo = New MyUtility().ReplaceCharForFile(myRow("IndirizzoUltSog"))
                myItem.NumeroCivico = New MyUtility().ReplaceCharForFile(myRow("CivicoUltSog"))
                myItem.LetteraNumeroCivico = New MyUtility().ReplaceCharForFile(myRow("LetteraUltSog"))
                myItem.Chilometro = New MyUtility().ReplaceCharForFile(myRow("ChilometroUltSog"))
                myItem.Palazzina = New MyUtility().ReplaceCharForFile(myRow("PalazzinaUltSog"))
                myItem.Scala = New MyUtility().ReplaceCharForFile(myRow("ScalaUltSog"))
                myItem.Piano = New MyUtility().ReplaceCharForFile(myRow("PianoUltSog"))
                myItem.Interno = New MyUtility().ReplaceCharForFile(myRow("InternoUltSog"))
                myItem.LocalitaFrazione = New MyUtility().ReplaceCharForFile(myRow("FrazioneUltSog"))
                Return myItem
            Catch Err As Exception
                Log.Debug(myIdAtto.IdentificativoFlusso.CodiceEnteCreditore + " - OPENgovPROVVEDIMENTI.clsCoattivo.CreaobjAccertamentiEsecutivi.PopolaE23.errore::" + Err.Message + " - COD_CONTRIBUENTE::" + myItem.ProgressivoRecord.ToString() + "|" + myIdAtto.NumeroAtto.ToString())
                Return Nothing
            End Try
        End Function
        Public Function PopolaE50(myRow As DataRowView, myIdAtto As objAccertamentiEsecutivi.IdentificativoAtto, nRC As Integer) As objAccertamentiEsecutivi.E50
            Dim myItem As New objAccertamentiEsecutivi.E50

            Try
                myItem.IdentificativoAtto = myIdAtto
                myItem.ProgressivoRecord = nRC
                myItem.ProgressivoArticolo = New MyUtility().ReplaceCharForFile(myRow("ProgressivoArticolo"))
                myItem.CodiceEntrata = New MyUtility().ReplaceCharForFile(myRow("CodiceEntrata"))
                myItem.TipoCodiceEntrata = New MyUtility().ReplaceCharForFile(myRow("TipoCodiceEntrata"))
                myItem.ImportoArticolo = New MyUtility().ReplaceCharForFile(myRow("ImpArticolo"))
                Return myItem
            Catch Err As Exception
                Log.Debug(myIdAtto.IdentificativoFlusso.CodiceEnteCreditore + " - OPENgovPROVVEDIMENTI.clsCoattivo.CreaAccertamentiEsecutivi::PopolaE50.errore::" + Err.Message + " - COD_CONTRIBUENTE::" + myItem.ProgressivoRecord.ToString() + "|" + myIdAtto.NumeroAtto.ToString())
                Return Nothing
            End Try
        End Function
        Public Function PopolaE60(myRow As DataRowView, myIdAtto As objAccertamentiEsecutivi.IdentificativoAtto, nRC As Integer) As objAccertamentiEsecutivi.E60
            Dim myItem As New objAccertamentiEsecutivi.E60

            Try
                myItem.IdentificativoAtto = myIdAtto
                myItem.ProgressivoRecord = nRC
                myItem.NumeroDelibera = New MyUtility().ReplaceCharForFile(myRow("NDelibera"))
                myItem.DataDelibera = New MyUtility().ReplaceCharForFile(myRow("DataDelibera"))
                myItem.DataFineValiditaDelibera = New MyUtility().ReplaceCharForFile(myRow("DataFineDelibera"))
                myItem.ImportoTotaleAtto = New MyUtility().ReplaceCharForFile(myRow("ImportoTotaleAtto"))
                myItem.TotaleArticoliAtto = New MyUtility().ReplaceCharForFile(myRow("ProgressivoArticolo"))
                myItem.DataTermineUltimoPagamentoAtto = New MyUtility().ReplaceCharForFile(myRow("DataTerminePagamentoAtto"))
                Return myItem
            Catch Err As Exception
                Log.Debug(myIdAtto.IdentificativoFlusso.CodiceEnteCreditore + " - OPENgovPROVVEDIMENTI.clsCoattivo.CreaAccertamentiEsecutivi::PopolaE60.errore::" + Err.Message + " - COD_CONTRIBUENTE::" + myItem.ProgressivoRecord.ToString() + "|" + myIdAtto.NumeroAtto.ToString())
                Return Nothing
            End Try
        End Function
        Public Function PopolaE99(myIdFlusso As objAccertamentiEsecutivi.IdentificativoFlusso, E00 As objAccertamentiEsecutivi.E00, nRC As Integer, nE20 As Integer, nE23 As Integer, nE50 As Integer, nE60 As Integer, impFlusso As Double) As objAccertamentiEsecutivi.E99
            Dim myItem As New objAccertamentiEsecutivi.E99

            Try
                nRC += 2 'Testa+Coda
                myItem.IdentificativoFlusso = myIdFlusso
                myItem.DataCreazioneFlusso = E00.DataCreazioneFlusso
                myItem.EstremiFlusso = E00.EstremiFlusso
                myItem.TotaleRecord = nRC
                myItem.TotaleRecordE20 = nE20
                myItem.TotaleRecordE23 = nE23
                myItem.TotaleRecordE50 = nE50
                myItem.TotaleRecordE60 = nE60
                myItem.TotaleCaricoFlusso = impFlusso

                Return myItem
            Catch Err As Exception
                Log.Debug(myIdFlusso.CodiceEnteCreditore + " - OPENgovPROVVEDIMENTI.clsCoattivo.CreaAccertamentiEsecutivi::PopolaE99.errore::" + Err.Message)
                Return Nothing
            End Try
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sPathNameAccertamentiEsecutivi"></param>
        ''' <param name="oE00"></param>
        ''' <param name="oE20"></param>
        ''' <param name="oE50"></param>
        ''' <param name="oE60"></param>
        ''' <param name="oE99"></param>
        ''' <returns></returns>
        Public Function CreateAccertamentiEsecutivi(ByVal sPathNameAccertamentiEsecutivi As String, ByVal oE00 As objAccertamentiEsecutivi.E00, ByVal oE20 As ArrayList, oE23 As ArrayList, ByVal oE50 As ArrayList, ByVal oE60 As ArrayList, ByVal oE99 As objAccertamentiEsecutivi.E99) As Integer

            Try
                'elimino il file se già presente
                If New MyUtility().DeleteFile(sPathNameAccertamentiEsecutivi) = False Then
                End If
                'scrivo il file
                If PrintLineE00(sPathNameAccertamentiEsecutivi, oE00) <= 0 Then
                    Return 0
                End If
                For Each myE20 As objAccertamentiEsecutivi.E20 In oE20
                    If PrintLineE20(sPathNameAccertamentiEsecutivi, myE20) <= 0 Then
                        Return 0
                    End If
                    For Each myE23 As objAccertamentiEsecutivi.E23 In oE23
                        If myE23.IdentificativoAtto.NumeroAtto = myE20.IdentificativoAtto.NumeroAtto Then
                            If PrintLineE23(sPathNameAccertamentiEsecutivi, myE23) <= 0 Then
                                Return 0
                            End If
                        End If
                    Next
                    For Each myE50 As objAccertamentiEsecutivi.E50 In oE50
                        If myE50.IdentificativoAtto.NumeroAtto = myE20.IdentificativoAtto.NumeroAtto Then
                            If PrintLineE50(sPathNameAccertamentiEsecutivi, myE50) <= 0 Then
                                Return 0
                            End If
                        End If
                    Next
                    For Each myE60 As objAccertamentiEsecutivi.E60 In oE60
                        If myE60.IdentificativoAtto.NumeroAtto = myE20.IdentificativoAtto.NumeroAtto Then
                            If PrintLineE60(sPathNameAccertamentiEsecutivi, myE60) <= 0 Then
                                Return 0
                            End If
                        End If
                    Next
                Next
                If PrintLineE99(sPathNameAccertamentiEsecutivi, oE99) <= 0 Then
                    Return 0
                End If

                Return 1
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsCoattivo.CreaAccertamentiEsecutivi::CreateobjAccertamentiEsecutivi.errore::" + Err.Message)
                Return -1
            End Try
        End Function
        ''' <summary>
        ''' 
		''' </summary>
        ''' <param name="sPathNameFile"></param>
        ''' <param name="oE00"></param>
        ''' <returns></returns>
        Public Function PrintLineE00(ByVal sPathNameFile As String, ByVal oE00 As objAccertamentiEsecutivi.E00) As Integer
            Dim sPrintLine As String = ""
            Try
                'Campo Posizione 1-3 Lungh. 3<<TIPO RECORD Vale Sempre "E00">> Tipo:AN Obbl.S Errore:004-SF
                sPrintLine += oE00.TipoRecord
                'Campo Posizione 4-8 Lungh. 5<<CODICE ENTE CREDITORE Deve essere presente nella "Tabella Enti Creditori e Beneficiari" APPENDICE B>> Tipo:N Obbl.S Errore:504-SF
                sPrintLine += PadLine(oE00.IdentificativoFlusso.CodiceEnteCreditore, oE00.IdentificativoFlusso.Length.CodiceEnteCreditore, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 9-9 Lungh. 1<<TIPO UFFICIO Tipologia ufficio ente creditore APPENDICE B>> Tipo:AN Obbl.S Errore:504-SF
                sPrintLine += PadLine(oE00.IdentificativoFlusso.TipoUfficio, oE00.IdentificativoFlusso.Length.TipoUfficio, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 10-15 Lungh. 6<<CODICE UFFICIO Codice ufficio dell'ente creditore APPENDICE B>> Tipo:AN Obbl.S Errore:504-SF
                sPrintLine += PadLine(oE00.IdentificativoFlusso.CodiceUfficio, oE00.IdentificativoFlusso.Length.CodiceUfficio, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 16-23 Lungh. 8<<DATA CREAZIONE FLUSSO Data in cui l'ente crea il file di carico espressa nel formato (AAAAMMGG).Deve essere minore o uguale alla data trasmissione e non deve essere antecedente alla data di trasmissione di oltre 15 giorni>> Tipo:N Obbl.S Errore:045-SF 096-SF
                sPrintLine += PadLine(oE00.DataCreazioneFlusso, oE00.Length.DataCreazioneFlusso, objAccertamentiEsecutivi.Type.Data)
                'Campo Posizione 24-33 Lungh. 10<<ESTREMI FLUSSO Identificativo del file trasmesso, espresso nella forma AAAA7NNNNN dove AAAA=Anno flusso, 7=fisso per indicare la tipologia accertamento esecutivo NNNNN=Progressivo crescente L’anno AAAA coincide con l’anno di creazione del flusso>> Tipo:N Obbl.S Errore:501-SF
                sPrintLine += PadLine(oE00.EstremiFlusso, oE00.Length.EstremiFlusso, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 34-600 Lungh. 567<<FILLER>> Tipo:AN Obbl.N
                sPrintLine += PadLine(oE00.Filler, oE00.Length.Filler, objAccertamentiEsecutivi.Type.Stringa)

                sPrintLine = New MyUtility().ReplaceCharForFile(sPrintLine)
                If New MyUtility().WriteFile(sPathNameFile, sPrintLine) <= 0 Then
                    Return 0
                End If

                Return 1
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsCoattivo.CreaAccertamentiEsecutivi::PrintLineE00.errore::" + Err.Message)
                Return -1
            End Try
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sPathNameFile"></param>
        ''' <param name="oE20"></param>
        ''' <returns></returns>
        Public Function PrintLineE20(ByVal sPathNameFile As String, ByVal oE20 As objAccertamentiEsecutivi.E20) As Integer
            Dim sPrintLine As String = ""
            Try
                'Campo Posizione 1-3 Lungh. 3<<TIPO RECORD Vale Sempre "E20".>> Tipo:AN Obbl.S Errore:Q20-SP	
                sPrintLine += oE20.TipoRecord
                'Campo Posizione 4-10 Lungh. 7<<PROGRESSIVO RECORD Numero progressivo del record. Deve essere maggiore di una unità rispetto al valore contenuto nel medesimo campo del record immediatamente precedente, sul primo rec all’interno del file il campo deve valere 1>> Tipo:N Obbl.S Errore:013-SF	
                sPrintLine += PadLine(oE20.ProgressivoRecord, oE20.Length.ProgressivoRecord, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 11-15 Lungh. 5<<CODICE ENTE CREDITORE Deve essere presente nella "Tabella Enti Creditori e Beneficiari" APPENDICE B Deve assumere lo stesso valore dell’analogo campo del record “E00”.>> Tipo:N Obbl.S Errore:502-SF	
                sPrintLine += PadLine(oE20.IdentificativoAtto.IdentificativoFlusso.CodiceEnteCreditore, oE20.IdentificativoAtto.IdentificativoFlusso.Length.CodiceEnteCreditore, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 16-16 Lungh. 1<<TIPO UFFICIO Assume lo stesso valore dell'analogo campo del record E00>> Tipo:AN Obbl.S Errore:E230-SP	
                sPrintLine += PadLine(oE20.IdentificativoAtto.IdentificativoFlusso.TipoUfficio, oE20.IdentificativoAtto.IdentificativoFlusso.Length.TipoUfficio, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 17-22 Lungh. 6<<CODICE UFFICIO Assume lo stesso valore dell'analogo campo del record E00>> Tipo:AN Obbl.S Errore:E230-SP	
                sPrintLine += PadLine(oE20.IdentificativoAtto.IdentificativoFlusso.CodiceUfficio, oE20.IdentificativoAtto.IdentificativoFlusso.Length.CodiceUfficio, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 23-26 Lungh. 4<<ANNO DI EMISSIONE ATTO Indica l'anno in cui è stato emesso l’atto di accertamento, non può essere maggiore dell’anno corrente, e coincide con l’anno del campo data emissione atto.>> Tipo:N Obbl.S Errore:E230-SP	
                sPrintLine += PadLine(oE20.IdentificativoAtto.AnnoEmissioneAtto, oE20.IdentificativoAtto.Length.AnnoEmissioneAtto, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 27-29 Lungh. 3<<TIPOLOGIA ATTO Vale sempre 005>> Tipo:N Obbl.S Errore:E230-SP	
                sPrintLine += PadLine(oE20.IdentificativoAtto.TipologiaAtto, oE20.IdentificativoAtto.Length.TipologiaAtto, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 30-38 Lungh. 9<<NUMERO PARTITA Deve essere numerico e crescente nell'ambito della fornitura.>> Tipo:N Obbl.S Errore:E230-SP	
                sPrintLine += PadLine(oE20.IdentificativoAtto.NumeroPartita, oE20.IdentificativoAtto.Length.NumeroPartita, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 39-41 Lungh. 3<<PROGRESSIVO PARTITA Vale sempre 001.>> Tipo:N Obbl.S Errore:E230-SP	
                sPrintLine += PadLine(oE20.IdentificativoAtto.ProgressivoPartita, oE20.IdentificativoAtto.Length.ProgressivoPartita, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 42-43 Lungh. 2<<CODICE TIPO ATTO Codice che indica la tipologia dell'atto. Vedi NOTA 3 Fare riferimento alla tabella presente in APPENDICE P>> Tipo:AN Obbl.N Errore:E230-SP	
                sPrintLine += PadLine(oE20.IdentificativoAtto.CodiceTipoAtto, oE20.IdentificativoAtto.Length.CodiceTipoAtto, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 44-51 Lungh. 8<<DATA EMISSIONE ATTO Data in cui l'ente ha emesso l'atto nel formato (AAAAMMGG) Deve essere maggiore del 01/01/2020, minore della data di creazione flusso e minore della data di notifica atto>> Tipo:N Obbl.S Errore:E230-SP	
                sPrintLine += PadLine(oE20.IdentificativoAtto.DataEmissioneAtto, oE20.IdentificativoAtto.Length.DataEmissioneAtto, objAccertamentiEsecutivi.Type.Data)
                'Campo Posizione 52-63 Lungh. 12<<NUMERO ATTO Estremi dell'atto notificato Non può essere nullo>> Tipo:AN Obbl.S Errore:E230-SP	
                sPrintLine += PadLine(oE20.IdentificativoAtto.NumeroAtto, oE20.IdentificativoAtto.Length.NumeroAtto, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 64-71 Lungh. 8<<DATA NOTIFICA ATTO Data di notifica dell'atto al debitore, nella forma AAAAMMGG. Deve essere maggiore del 01/01/2020, maggiore o uguale della data emmissione atto e inferiore alla data creazione file presente sul record E00 di almeno 60 gg L'anno (AAAA) deve essere maggiore o uguale del valore indicato nel campo anno di emessione atto>> Tipo:N Obbl.S Errore:E230-SP T20-SP U20-SP	
                sPrintLine += PadLine(oE20.IdentificativoAtto.DataNotificaAtto, oE20.IdentificativoAtto.Length.DataNotificaAtto, objAccertamentiEsecutivi.Type.Data)
                'Campo Posizione 72-111 Lungh. 40<<FILLER >> Tipo:AN Obbl.N	
                sPrintLine += PadLine(oE20.Filler, oE20.Length.Filler, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 112-112 Lungh. 1<<PRESENZA ULTERIORI SOGGETTI DESTINATARI DELL'ATTO Indica la presenza di ulteriori destinatari per la notifica dell'atto. Deve essere presente nella "Tabella Codici altri soggetti per notifica – APPENDICE O" Deve assumere il valore ‘T’/’R’ in presenza di soggetto minore. Non può assumere il valore ‘T’ in presenza di soggetto giuridico>> Tipo:AN Obbl.S Errore:H20-SP R20-SP	
                sPrintLine += PadLine(oE20.PresenzaUlterioriDestinatariAtto, oE20.Length.PresenzaUlterioriDestinatariAtto, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 113-113 Lungh. 1<<PRESENZA COOBLIGATI Indica la presenza di soggetti coobbligati. Assume il valore: "1" = assenza di coobbligati "2" = presenza di coobbligati "C" = coobbligati Deve assumere i valori "1", "2" per i record E20 relativi all’intestatario Deve assumere il valore "C" per i record E20 relativi ai coobbligati>> Tipo:AN Obbl.S Errore:I20-SP O20-SP	
                sPrintLine += PadLine(oE20.PresenzaCoobligati, oE20.Length.PresenzaCoobligati, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 114-114 Lungh. 1<<NATURA SOGGETTO Indica la natura giuridica del soggetto. 1=PERSONA FISICA 2=PERSONA GIURIDICA>> Tipo:AN Obbl.S Errore:055-SP	
                sPrintLine += PadLine(oE20.NaturaSoggetto, oE20.Length.NaturaSoggetto, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 115-130 Lungh. 16<<CODICE FISCALE Codice Fiscale dell'intestatario, del coobligato. Se Natura Giuridica del soggetto è uguale a 1 (P.F.): •  deve essere formalmente corretto, (cfr. APPENDICE N) •  deve essere congruente con i dati anagrafici del soggetto. Se Natura Giuridica del soggetto è uguale a 2 (P.G.): •  deve essere numerico di 11 caratteri seguito da spazio. Per le causali di scarto verificare APPENDICE A>> Tipo:AN Obbl.S Errore:096-SP A-SP B-SP C-SP D-SP E-SP F-SP G-SP H-SP I-SP L-SP M-SP N-SP O-SP Q-SP R-SP A20-SP B20-SP	
                sPrintLine += PadLine(oE20.CodiceFiscale, oE20.Length.CodiceFiscale, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 131-210 Lungh. 80<<COGNOME/DENOMINAZIONE Cognome/Denominazione del soggetto. Deve essere sempre valorizzato. Se il campo Natura giuridica è uguale a "1" conterrà il cognome del soggetto Se il campo Natura giuridica è uguale a "2" conterrà la denominazione societaria. Puo’ contenere solo lettere maiuscole, trattino e apice.>> Tipo:AN Obbl.S Errore:J20-SP M20-SP S20-SP	
                sPrintLine += PadLine(oE20.CognomeDenominazione, oE20.Length.CognomeDenominazione, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 211-250 Lungh. 40<<NOME Nome del soggetto. Deve essere valorizzato se il campo Natura giuridica è uguale a "1". Può contenere solo lettere maiuscole, trattino e apice>> Tipo:AN Obbl.S Errore:L20-SP S20-SP	
                sPrintLine += PadLine(oE20.Nome, oE20.Length.Nome, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 251-251 Lungh. 1<<SESSO Sesso del soggetto. Deve essere valorizzato se il campo Natura giuridica è uguale a "1". Se valorizzato può assumere i valori "F" o "M". Assume il valore: "F" = femmina "M" = maschio>> Tipo:AN Obbl.S Errore:S20-SP	
                sPrintLine += PadLine(oE20.Sesso, oE20.Length.Sesso, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 252-259 Lungh. 8<<DATA DI NASCITA Data di nascita. Deve essere sempre valorizzata se il campo Natura giuridica è uguale a "1". Deve essere espressa nella forma AAAAMMGG e deve essere formalmente corretta. Se la data di nascita individua un soggetto minore deve essere sempre presente il rec.E23 relativo all’ulteriore destinatario della notifica con tipologia ‘T’/’R’>> Tipo:N Obbl.S Errore:S20-SP	
                sPrintLine += PadLine(oE20.DataNascita, oE20.Length.DataNascita, objAccertamentiEsecutivi.Type.Data)
                'Campo Posizione 260-263 Lungh. 4<<CODICE CATASTALE DEL COMUNE DI NASCITA Codice Catastale del comune di nascita Deve essere sempre valorizzata se il campo Natura giuridica è uguale a "1".>> Tipo:AN Obbl.S Errore:S20-SP	
                sPrintLine += PadLine(oE20.CodiceCatastaleComuneNascita, oE20.Length.CodiceCatastaleComuneNascita, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 264-265 Lungh. 2<<PROVINCIA DI NASCITA Sigla provincia del comune di nascita. Deve essere sempre valorizzata se il campo Natura giuridica è uguale a "1".>> Tipo:AN Obbl.S Errore:S20-SP	
                sPrintLine += PadLine(oE20.ProvinciaNascita, oE20.Length.ProvinciaNascita, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 266-269 Lungh. 4<<CODICE CATASTALE DEL COMUNE DI DOMICILIO FISCALE Contiene il codice catastale del domicilio fiscale ultimo alla data di notifica dell'atto da parte dell'ente.>> Tipo:AN Obbl.S Errore:039-SP 504-SP	
                sPrintLine += PadLine(oE20.CodiceCatastaleComuneDomicilioFiscale, oE20.Length.CodiceCatastaleComuneDomicilioFiscale, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 270-314 Lungh. 45<<COMUNE DEL DOMICILIO FISCALE Contiene la denominazione del comune del domicilio fiscale ultimo alla data di notifica dell'atto da parte dell'ente.>> Tipo:AN Obbl.S Errore:039-SP 504-SP	
                sPrintLine += PadLine(oE20.ComuneDomicilioFiscale, oE20.Length.ComuneDomicilioFiscale, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 315-316 Lungh. 2<<PROVINCIA DEL DOMICILIO FISCALE Contiene la provincia del domicilio fiscale ultimo alla data di notifica dell'atto da parte dell'ente.>> Tipo:AN Obbl.S Errore:039-SP 504-SP	
                sPrintLine += PadLine(oE20.ProvinciaDomicilioFiscale, oE20.Length.ProvinciaDomicilioFiscale, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 317-321 Lungh. 5<<C.A.P. DEL DOMICILIO FISCALE Contiene il cap del domicilio fiscale ultimo alla data di notifica dell'atto da parte dell'ente.>> Tipo:N Obbl.S Errore:039-SP 504-SP	
                sPrintLine += PadLine(oE20.CAPDomicilioFiscale, oE20.Length.CAPDomicilioFiscale, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 322-401 Lungh. 80<<INDIRIZZO Contiene l'indirizzo del domicilio fiscale ultimo alla data di notifica dell'atto da parte dell'ente>> Tipo:AN Obbl.S Errore:039-SP	
                sPrintLine += PadLine(oE20.Indirizzo, oE20.Length.Indirizzo, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 402-406 Lungh. 5<<NUMERO CIVICO Contiene il numero civico dell'indirizzo del domicilio fiscale ultimo alla data di notifica dell'atto da parte dell'ente>> Tipo:N Obbl.N	
                sPrintLine += PadLine(oE20.NumeroCivico, oE20.Length.NumeroCivico, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 407-408 Lungh. 2<<LETTERA NUMERO CIVICO Se valorizzato contiene la lettera associato al numero civico del'indirizzo di domicilio fiscale.>> Tipo:AN Obbl.N	
                sPrintLine += PadLine(oE20.LetteraNumeroCivico, oE20.Length.LetteraNumeroCivico, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 409-414 Lungh. 6<<CHILOMETRO Se valorizzato contiene il chilometro dell'indirizzo del domicilio fiscale e deve essere espresso nella forma KKKMMM, dove: "KKK"= parte intera del chilometro; "MMM"= parte decimale del chilometro.>> Tipo:N Obbl.N	
                sPrintLine += PadLine(oE20.Chilometro, oE20.Length.Chilometro, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 415-417 Lungh. 3<<PALAZZINA Se valorizzato contiene l’identificativo della palazzina relativa all’indirizzo del domicilio fiscale>> Tipo:AN Obbl.N	
                sPrintLine += PadLine(oE20.Palazzina, oE20.Length.Palazzina, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 418-420 Lungh. 3<<SCALA Se valorizzato contiene l’identificativo della scala relativo all’indirizzo del domicilio fiscale>> Tipo:AN Obbl.N	
                sPrintLine += PadLine(oE20.Scala, oE20.Length.Scala, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 421-423 Lungh. 3<<PIANO Se valorizzato contiene l’identificativo del piano relativo all’indirizzo del domicilio fiscale>> Tipo:AN Obbl.N	
                sPrintLine += PadLine(oE20.Piano, oE20.Length.Piano, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 424-427 Lungh. 4<<INTERNO Se valorizzato contiene l’identificativo dell’interno relativo all’indirizzo del domicilio fiscale>> Tipo:AN Obbl.N	
                sPrintLine += PadLine(oE20.Interno, oE20.Length.Interno, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 428-452 Lungh. 25<<LOCALITA’ – FRAZIONE Se valorizzato contiene la descrizione della frazione o della località del domicilio fiscale.>> Tipo:AN Obbl.N	
                sPrintLine += PadLine(oE20.LocalitaFrazione, oE20.Length.LocalitaFrazione, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 453-600 Lungh. 148<<FILLER >> Tipo:AN Obbl.S	
                sPrintLine += PadLine(oE20.Filler, oE20.Length.Filler, objAccertamentiEsecutivi.Type.Stringa)

                sPrintLine = New MyUtility().ReplaceCharForFile(sPrintLine)
                If New MyUtility().WriteFile(sPathNameFile, sPrintLine) <= 0 Then
                    Return 0
                End If

                Return 1
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsCoattivo.CreaAccertamentiEsecutivi::PrintLineE20.errore::" + Err.Message)
                Return -1
            End Try
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sPathNameFile"></param>
        ''' <param name="oE23"></param>
        ''' <returns></returns>
        Public Function PrintLineE23(ByVal sPathNameFile As String, ByVal oE23 As objAccertamentiEsecutivi.E23) As Integer
            Dim sPrintLine As String = ""
            Try
                'Campo Posizione 1-3 Lungh. 3<<TIPO RECORD TIPO RECORD Vale Sempre "E23".>> Tipo:AN Obbl.S Errore:Q20-SP Q20-SP	
                sPrintLine += oE23.TipoRecord
                'Campo Posizione 4-10 Lungh. 7<<PROGRRESSIVO RECORD PROGRRESSIVO RECORD Numero progressivo del record. Deve essere maggiore di una unità rispetto al valore contenuto nel medesimo campo del record immediatamente precedente>> Tipo:N Obbl.S Errore:004-SF 004-SF	
                sPrintLine += PadLine(oE23.ProgressivoRecord, oE23.Length.ProgressivoRecord, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 11-15 Lungh. 5<<CODICE ENTE CREDITORE CODICE ENTE CREDITORE Deve essere presente nella "Tabella Enti Creditori e Beneficiari" APPENDICE B Assume lo stesso valore dell'analogo campo del record E00>> Tipo:N Obbl.S Errore:504-SF 504-SF	
                sPrintLine += PadLine(oE23.IdentificativoAtto.IdentificativoFlusso.CodiceEnteCreditore, oE23.IdentificativoAtto.IdentificativoFlusso.Length.CodiceEnteCreditore, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 16-16 Lungh. 1<<TIPO UFFICIO TIPO UFFICIO Assume lo stesso valore dell'analogo campo del record E00>> Tipo:AN Obbl.S Errore:N20-SP N20-SP	
                sPrintLine += PadLine(oE23.IdentificativoAtto.IdentificativoFlusso.TipoUfficio, oE23.IdentificativoAtto.IdentificativoFlusso.Length.TipoUfficio, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 17-22 Lungh. 6<<CODICE UFFICIO CODICE UFFICIO Assume lo stesso valore dell'analogo campo del record E00>> Tipo:AN Obbl.S Errore:N20-SP N20-SP	
                sPrintLine += PadLine(oE23.IdentificativoAtto.IdentificativoFlusso.CodiceUfficio, oE23.IdentificativoAtto.IdentificativoFlusso.Length.CodiceUfficio, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 23-26 Lungh. 4<<ANNO DI EMISSIONE ATTO ANNO DI EMISSIONE ATTO Indica l'anno in cui è stato emesso l’atto di accertamento, non può essere maggiore dell’anno corrente, e coincide con l’anno del campo data emissione atto. TIPOLOGIA ATTO>> Tipo:N Obbl.S Errore:N20-SP N20-SP N20-SP	
                sPrintLine += PadLine(oE23.IdentificativoAtto.AnnoEmissioneAtto, oE23.IdentificativoAtto.Length.AnnoEmissioneAtto, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 27-29 Lungh. 0<<TIPOLOGIA ATTO Vale sempre 005>> Tipo:0 Obbl.N Errore:0	
                sPrintLine += PadLine(oE23.IdentificativoAtto.TipologiaAtto, oE23.IdentificativoAtto.Length.TipologiaAtto, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 30-38 Lungh. 9<<NUMERO PARTITA NUMERO PARTITA Deve essere numerico e crescente nell'ambito della fornitura.>> Tipo:N Obbl.S Errore:N20-SP N20-SP	
                sPrintLine += PadLine(oE23.IdentificativoAtto.NumeroPartita, oE23.IdentificativoAtto.Length.NumeroPartita, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 39-41 Lungh. 3<<PROGRESSIVO PARTITA PROGRESSIVO PARTITA Vale sempre 001.>> Tipo:N Obbl.S Errore:N20-SP N20-SP	
                sPrintLine += PadLine(oE23.IdentificativoAtto.ProgressivoPartita, oE23.IdentificativoAtto.Length.ProgressivoPartita, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 42-43 Lungh. 2<<CODICE TIPO ATTO CODICE TIPO ATTO Codice che indica la tipologia dell'atto. Vedi NOTA 3. Fare riferimento alla tabella presente in APPENDICE P>> Tipo:AN Obbl.N Errore:N20-SP N20-SP	
                sPrintLine += PadLine(oE23.IdentificativoAtto.CodiceTipoAtto, oE23.IdentificativoAtto.Length.CodiceTipoAtto, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 44-51 Lungh. 8<<DATA EMISSIONE ATTO DATA EMISSIONE ATTO Data in cui l'ente ha emesso l'atto nel formato (AAAAMMGG) Deve essere maggiore del 01/01/2020, minore della data di creazione flusso e minore della data di notifica atto>> Tipo:N Obbl.S Errore:N20-SP N20-SP	
                sPrintLine += PadLine(oE23.IdentificativoAtto.DataEmissioneAtto, oE23.IdentificativoAtto.Length.DataEmissioneAtto, objAccertamentiEsecutivi.Type.Data)
                'Campo Posizione 52-63 Lungh. 12<<NUMERO ATTO NUMERO ATTO Estremi dell'atto notificato Non può essere nullo>> Tipo:AN Obbl.S Errore:N20-SP N20-SP	
                sPrintLine += PadLine(oE23.IdentificativoAtto.NumeroAtto, oE23.IdentificativoAtto.Length.NumeroAtto, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 64-71 Lungh. 8<<DATA NOTIFICA ATTO DATA NOTIFICA ATTO Data di notifica dell'atto al debitore, nella forma AAAAMMGG. Deve essere maggiore del 01/01/2020, maggiore o uguale della data emmissione atto e inferiore alla data creazione file presente sul record E00 di almeno 60 gg. L'anno (AAAA) deve essere maggiore o uguale del valore indicato nel campo anno di emessione atto>> Tipo:N Obbl.S Errore:N20-SP N20-SP	
                sPrintLine += PadLine(oE23.IdentificativoAtto.DataNotificaAtto, oE23.IdentificativoAtto.Length.DataNotificaAtto, objAccertamentiEsecutivi.Type.Data)
                'Campo Posizione 72-111 Lungh. 40<<FILLER  FILLER>> Tipo:AN Obbl.S Errore:SPAZI SPAZI	
                sPrintLine += PadLine(oE23.Filler, oE23.Length.Filler, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 112-127 Lungh. 16<<CODICE FISCALE DEL SOGGETTO INTESTATARIO CODICE FISCALE DEL SOGGETTO INTESTATARIO Riporta lo stesso codice fiscale del record E20 dell'intestatario o del cooblicato dell’atto a cui l'ulteriore soggetto riferisce.>> Tipo:AN Obbl.S Errore:506-SF 506-SF	
                sPrintLine += PadLine(oE23.CodiceFiscaleSoggettoIntestatario, oE23.Length.CodiceFiscaleSoggettoIntestatario, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 128-143 Lungh. 16<<CODICE FISCALE ULTERIORE SOGGETTO NOTIFICA CODICE FISCALE ULTERIORE SOGGETTO NOTIFICA Codice fiscale dell’ulteriore soggetto. Deve essere formalmente corretto (cfr. APPENDICE N). Deve essere obbligatoriamente un soggetto fisico. Per le causali di scarto verificare APPENDICE A>> Tipo:AN Obbl.S Errore:096-SP A-SP B-SP C-SP D-SP E-SP F-SP G-SP H-SP I-SP L-SP M-SP N-SP O-SP Q-SP R-SP A23-SP E23-SP 096-SP A-SP B-SP C-SP D-SP E-SP F-SP G-SP H-SP I-SP L-SP M-SP N-SP O-SP Q-SP R-SP A23-SP E23-SP F23-SP	
                sPrintLine += PadLine(oE23.CodiceFiscaleUlterioreSoggettoNotifica, oE23.Length.CodiceFiscaleUlterioreSoggettoNotifica, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 144-193 Lungh. 50<<COGNOME COGNOME Cognome del soggetto. Puo’ contenere solo lettere maiuscole, trattino e apice>> Tipo:AN Obbl.S Errore:B23-SP B23-SP	
                sPrintLine += PadLine(oE23.Cognome, oE23.Length.Cognome, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 194-233 Lungh. 40<<NOME NOME Nome del soggetto. Può contenere solo lettere maiuscole, trattino e apice>> Tipo:AN Obbl.S Errore:C23-SP C23-SP	
                sPrintLine += PadLine(oE23.Nome, oE23.Length.Nome, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 234-234 Lungh. 1<<SESSO SESSO Sesso del soggetto. Assume il valore: "F" = femmina "M" = maschio>> Tipo:AN Obbl.S Errore:039-SP 039-SP 055-SP	
                sPrintLine += PadLine(oE23.Sesso, oE23.Length.Sesso, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 235-242 Lungh. 8<<DATA DI NASCITA DATA DI NASCITA Data di nascita. Deve essere espressa nella forma AAAAMMGG.>> Tipo:N Obbl.S Errore:045-SP 045-SP 055-SP	
                sPrintLine += PadLine(oE23.DataNascita, oE23.Length.DataNascita, objAccertamentiEsecutivi.Type.Data)
                'Campo Posizione 243-246 Lungh. 4<<CODICE CATASTALE DEL COMUNE DI NASCITA CODICE CATASTALE DEL COMUNE DI NASCITA Codice Catastale del comune di nascita>> Tipo:AN Obbl.S Errore:039-SP 039-SP 055-SP	
                sPrintLine += PadLine(oE23.CodiceCatastaleComuneNascita, oE23.Length.CodiceCatastaleComuneNascita, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 247-248 Lungh. 2<<PROVINCIA DI NASCITA PROVINCIA DI NASCITA Sigla provincia del comune di nascita.>> Tipo:AN Obbl.S Errore:039-SP 039-SP 055-SP	
                sPrintLine += PadLine(oE23.ProvinciaNascita, oE23.Length.ProvinciaNascita, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 249-252 Lungh. 4<<CODICE CATASTALE DEL COMUNE DI DOMICILIO FISCALE CODICE CATASTALE DEL COMUNE DI DOMICILIO FISCALE Contiene il codice catastale del domicilio fiscale ultimo alla data di notifica dell'atto da parte dell'ente.>> Tipo:AN Obbl.S Errore:039-SP 039-SP 504-SP	
                sPrintLine += PadLine(oE23.CodiceCatastaleComuneDomicilioFiscale, oE23.Length.CodiceCatastaleComuneDomicilioFiscale, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 253-297 Lungh. 45<<COMUNE DEL DOMICILIO FISCALE COMUNE DEL DOMICILIO FISCALE Contiene la denominazione del comune del domicilio fiscale ultimo alla data di notifica dell'atto da parte dell'ente.>> Tipo:AN Obbl.S Errore:039-SP 039-SP 504-SP	
                sPrintLine += PadLine(oE23.ComuneDomicilioFiscale, oE23.Length.ComuneDomicilioFiscale, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 298-299 Lungh. 2<<PROVINCIA DEL DOMICILIO FISCALE PROVINCIA DEL DOMICILIO FISCALE Contiene la provincia del domicilio fiscale ultimo alla data di notifica dell'atto da parte dell'ente.>> Tipo:AN Obbl.S Errore:039-SP 039-SP 504-SP	
                sPrintLine += PadLine(oE23.ProvinciaDomicilioFiscale, oE23.Length.ProvinciaDomicilioFiscale, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 300-304 Lungh. 5<<C.A.P. DEL DOMICILIO FISCALE C.A.P. DEL DOMICILIO FISCALE Contiene il cap del domicilio fiscale ultimo alla data di notifica dell'atto da parte dell'ente.>> Tipo:N Obbl.S Errore:039-SP 039-SP 504-SP	
                sPrintLine += PadLine(oE23.CAPDomicilioFiscale, oE23.Length.CAPDomicilioFiscale, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 305-384 Lungh. 80<<INDIRIZZO INDIRIZZO Contiene l'indirizzo del domicilio fiscale ultimo alla data di notifica dell'atto da parte dell'ente>> Tipo:AN Obbl.S Errore:039-SP 039-SP	
                sPrintLine += PadLine(oE23.Indirizzo, oE23.Length.Indirizzo, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 385-389 Lungh. 5<<NUMERO CIVICO NUMERO CIVICO Contiene il numero civico dell'indirizzo del domicilio fiscale ultimo alla data di notifica dell'atto da parte dell'ente>> Tipo:N Obbl.N Errore:0	
                sPrintLine += PadLine(oE23.NumeroCivico, oE23.Length.NumeroCivico, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 390-391 Lungh. 2<<LETTERA NUMERO CIVICO LETTERA NUMERO CIVICO Se valorizzato contiene la lettera associato al numero civico del'indirizzo di domicilio fiscale.>> Tipo:AN Obbl.N Errore:0	
                sPrintLine += PadLine(oE23.LetteraNumeroCivico, oE23.Length.LetteraNumeroCivico, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 392-397 Lungh. 6<<CHILOMETRO CHILOMETRO Se valorizzato contiene il chilometro dell'indirizzo del domicilio fiscale e deve essere espresso nella forma KKKMMM, dove: "KKK"= parte intera del chilometro; "MMM"= parte decimale del chilometro.>> Tipo:N Obbl.N Errore:0	
                sPrintLine += PadLine(oE23.Chilometro, oE23.Length.Chilometro, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 398-400 Lungh. 3<<PALAZZINA PALAZZINA Se valorizzato contiene l’identificativo della palazzina relativa all’indirizzo del domicilio fiscale>> Tipo:AN Obbl.N Errore:0	
                sPrintLine += PadLine(oE23.Palazzina, oE23.Length.Palazzina, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 401-403 Lungh. 3<<SCALA SCALA Se valorizzato contiene l’identificativo della scala relativo all’indirizzo del domicilio fiscale>> Tipo:AN Obbl.N Errore:0	
                sPrintLine += PadLine(oE23.Scala, oE23.Length.Scala, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 404-406 Lungh. 3<<PIANO PIANO Se valorizzato contiene l’identificativo del piano relativo all’indirizzo del domicilio fiscale>> Tipo:AN Obbl.N Errore:0	
                sPrintLine += PadLine(oE23.Piano, oE23.Length.Piano, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 407-410 Lungh. 4<<INTERNO INTERNO Se valorizzato contiene l’identificativo dell’interno relativo all’indirizzo del domicilio fiscale>> Tipo:AN Obbl.N Errore:0	
                sPrintLine += PadLine(oE23.Interno, oE23.Length.Interno, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 411-435 Lungh. 25<<LOCALITA’ – FRAZIONE LOCALITA’ – FRAZIONE Se valorizzato contiene la descrizione della frazione o della località del domicilio fiscale.>> Tipo:AN Obbl.N Errore:0	
                sPrintLine += PadLine(oE23.LocalitaFrazione, oE23.Length.LocalitaFrazione, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 436-600 Lungh. 165<<FILLER>> Tipo:AN Obbl.N Errore:SPAZI	
                sPrintLine += PadLine(oE23.Filler, oE23.Length.Filler, objAccertamentiEsecutivi.Type.Stringa)

                sPrintLine = New MyUtility().ReplaceCharForFile(sPrintLine)
                If New MyUtility().WriteFile(sPathNameFile, sPrintLine) <= 0 Then
                    Return 0
                End If

                Return 1
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsCoattivo.CreaAccertamentiEsecutivi::PrintLineE23.errore::" + Err.Message)
                Return -1
            End Try
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sPathNameFile"></param>
        ''' <param name="oE50"></param>
        ''' <returns></returns>
        Public Function PrintLineE50(ByVal sPathNameFile As String, ByVal oE50 As objAccertamentiEsecutivi.E50) As Integer
            Dim sPrintLine As String = ""
            Try
                'Campo Posizione 1-3 Lungh. 3<<TIPO RECORD TIPO RECORD Vale Sempre "E50".>> Tipo:AN Obbl.S Errore:004-SF 004-SF	
                sPrintLine += oE50.TipoRecord
                'Campo Posizione 4-10 Lungh. 7<<PROGRRESSIVO RECORD PROGRRESSIVO RECORD Numero progressivo del record. Deve essere maggiore di una unità rispetto al valore contenuto nel medesimo campo del record immediatamente precedente>> Tipo:N Obbl.S Errore:004-SF 004-SF	
                sPrintLine += PadLine(oE50.ProgressivoRecord, oE50.Length.ProgressivoRecord, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 11-15 Lungh. 5<<CODICE ENTE CREDITORE CODICE ENTE CREDITORE Deve essere presente nella "Tabella Enti Creditori e Beneficiari"APPENDICE B Assume lo stesso valore dell'analogo campo del record E00>> Tipo:N Obbl.S Errore:504-SF 504-SF	
                sPrintLine += PadLine(oE50.IdentificativoAtto.IdentificativoFlusso.CodiceEnteCreditore, oE50.IdentificativoAtto.IdentificativoFlusso.Length.CodiceEnteCreditore, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 16-16 Lungh. 1<<TIPO UFFICIO TIPO UFFICIO Assume lo stesso valore dell'analogo campo del record E00>> Tipo:AN Obbl.S Errore:N20-SP N20-SP	
                sPrintLine += PadLine(oE50.IdentificativoAtto.IdentificativoFlusso.TipoUfficio, oE50.IdentificativoAtto.IdentificativoFlusso.Length.TipoUfficio, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 17-22 Lungh. 6<<CODICE UFFICIO CODICE UFFICIO Assume lo stesso valore dell'analogo campo del record E00>> Tipo:AN Obbl.S Errore:N20-SP N20-SP	
                sPrintLine += PadLine(oE50.IdentificativoAtto.IdentificativoFlusso.CodiceUfficio, oE50.IdentificativoAtto.IdentificativoFlusso.Length.CodiceUfficio, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 23-26 Lungh. 4<<ANNO DI EMISSIONE ATTO ANNO DI EMISSIONE ATTO Indica l'anno in cui è stato emesso l’atto di accertamento, non può essere maggiore dell’anno corrente, e coincide con l’anno del campo data emissione atto.>> Tipo:N Obbl.S Errore:N20-SP N20-SP	
                sPrintLine += PadLine(oE50.IdentificativoAtto.AnnoEmissioneAtto, oE50.IdentificativoAtto.Length.AnnoEmissioneAtto, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 27-29 Lungh. 3<<TIPOLOGIA ATTO TIPOLOGIA ATTO Vale sempre 005>> Tipo:N Obbl.S Errore:N20-SP N20-SP	
                sPrintLine += PadLine(oE50.IdentificativoAtto.TipologiaAtto, oE50.IdentificativoAtto.Length.TipologiaAtto, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 30-38 Lungh. 9<<NUMERO PARTITA NUMERO PARTITA Deve essere numerico e crescente nell'ambito della fornitura.>> Tipo:N Obbl.S Errore:N20-SP N20-SP	
                sPrintLine += PadLine(oE50.IdentificativoAtto.NumeroPartita, oE50.IdentificativoAtto.Length.NumeroPartita, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 39-41 Lungh. 3<<PROGRESSIVO PARTITA PROGRESSIVO PARTITA Vale sempre 001.>> Tipo:N Obbl.S Errore:N20-SP N20-SP	
                sPrintLine += PadLine(oE50.IdentificativoAtto.ProgressivoPartita, oE50.IdentificativoAtto.Length.ProgressivoPartita, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 42-43 Lungh. 2<<CODICE TIPO ATTO CODICE TIPO ATTO Codice che indica la tipologia dell'atto. Vedi NOTA 3. Fare riferimento alla tabella presente in APPENDICE P>> Tipo:AN Obbl.N Errore:N20-SP N20-SP	
                sPrintLine += PadLine(oE50.IdentificativoAtto.CodiceTipoAtto, oE50.IdentificativoAtto.Length.CodiceTipoAtto, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 44-51 Lungh. 8<<DATA EMISSIONE ATTO DATA EMISSIONE ATTO Data in cui l'ente ha emesso l'atto nel formato (AAAAMMGG) Deve essere maggiore del 01/01/2020, minore della data di creazione flusso e minore della data di notifica atto.>> Tipo:N Obbl.S Errore:N20-SP N20-SP	
                sPrintLine += PadLine(oE50.IdentificativoAtto.DataEmissioneAtto, oE50.IdentificativoAtto.Length.DataEmissioneAtto, objAccertamentiEsecutivi.Type.Data)
                'Campo Posizione 52-63 Lungh. 12<<NUMERO ATTO NUMERO ATTO Estremi dell'atto notificato Non può essere nullo>> Tipo:AN Obbl.S Errore:N20-SP N20-SP	
                sPrintLine += PadLine(oE50.IdentificativoAtto.NumeroAtto, oE50.IdentificativoAtto.Length.NumeroAtto, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 64-71 Lungh. 8<<DATA NOTIFICA ATTO DATA NOTIFICA ATTO Data di notifica dell'atto al debitore, nella forma AAAAMMGG. Deve essere maggiore del 01/01/2020, maggiore o uguale della data emmissione atto e inferiore alla data creazione file presente sul record E00 di almeno 60 gg. L'anno (AAAA) deve essere maggiore o uguale del valore indicato nel campo anno di emessione atto>> Tipo:N Obbl.S Errore:N20-SP N20-SP	
                sPrintLine += PadLine(oE50.IdentificativoAtto.DataNotificaAtto, oE50.IdentificativoAtto.Length.DataNotificaAtto, objAccertamentiEsecutivi.Type.Data)
                'Campo Posizione 72-111 Lungh. 40<<FILLER  FILLER>> Tipo:AN Obbl.S Errore:0	
                sPrintLine += PadLine(oE50.Filler, oE50.Length.Filler, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 112-114 Lungh. 3<<PROGRESSIVO ARTICOLO PROGRESSIVO ARTICOLO Indica il progressivo del Codice Entrata nell'ambito della stessa partita. Deve essere in stretta sequenza a partire da 1.>> Tipo:N Obbl.S Errore:013-SF 013-SF	
                sPrintLine += PadLine(oE50.ProgressivoArticolo, oE50.Length.ProgressivoArticolo, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 115-120 Lungh. 6<<PERIODO DI RIFERIMENTO PERIODO DI RIFERIMENTO Indica il periodo di riferimento dell'entrata. Il campo è obbligatorio e deve essere espresso nella forma AAAAPP dove: AAAA indica l'anno, non può essere maggiore dell’anno di emissione atto. PP indica il periodo •  Il campo PP deve essere presente nella "Tabella Periodo infrazione - APPENDICE K" Nel caso in cui non ci sia un periodo, valorizzare PP con "99">> Tipo:N Obbl.S Errore:045-SP 045-SP	
                sPrintLine += PadLine(oE50.PeriodoRiferimento, oE50.Length.PeriodoRiferimento, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 121-124 Lungh. 4<<CODICE ENTRATA CODICE ENTRATA Codice dell'obbligazione tributaria del debitore. Deve essere presente nella "Tabella Codice Entrata – APPENDICE D">> Tipo:AN Obbl.S Errore:A50-SP A50-SP	
                sPrintLine += PadLine(oE50.CodiceEntrata, oE50.Length.CodiceEntrata, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 125-125 Lungh. 1<<TIPO CODICE ENTRATA TIPO CODICE ENTRATA Indica la tipologia del Codice entrata. Deve essere presente nella "Tabella Tipo Codice entrata APPENDICE E">> Tipo:AN Obbl.S Errore:055-SP 055-SP	
                sPrintLine += PadLine(oE50.TipoCodiceEntrata, oE50.Length.TipoCodiceEntrata, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 126-140 Lungh. 15<<IMPORTO ARTICOLO IMPORTO ARTICOLO Importo dovuto dal debitore per l'entrata, espresso in centesimi di Euro. Deve essere maggiore di zero.>> Tipo:N Obbl.S Errore:039-SP 039-SP	
                sPrintLine += PadLine(oE50.ImportoArticolo, oE50.Length.ImportoArticolo, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 141-600 Lungh. 460<<FILLER>> Tipo:AN Obbl.S Errore:SPAZI	
                sPrintLine += PadLine(oE50.Filler, oE50.Length.Filler, objAccertamentiEsecutivi.Type.Stringa)

                sPrintLine = New MyUtility().ReplaceCharForFile(sPrintLine)
                If New MyUtility().WriteFile(sPathNameFile, sPrintLine) <= 0 Then
                    Return 0
                End If

                Return 1
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsCoattivo.CreaAccertamentiEsecutivi::PrintLineE50.errore::" + Err.Message)
                Return -1
            End Try
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sPathNameFile"></param>
        ''' <param name="oE60"></param>
        ''' <returns></returns>
        Public Function PrintLineE60(ByVal sPathNameFile As String, ByVal oE60 As objAccertamentiEsecutivi.E60) As Integer
            Dim sPrintLine As String = ""
            Try
                'Campo Posizione 1-3 Lungh. 3<<TIPO RECORD TIPO RECORD Vale Sempre "E60".>> Tipo:AN Obbl.S Errore:004-SF 004-SF	
                sPrintLine += oE60.TipoRecord
                'Campo Posizione 4-10 Lungh. 7<<PROGRESSIVO RECORD PROGRESSIVO RECORD Numero progressivo del record. Deve essere maggiore di una unità rispetto al valore contenuto nel medesimo campo del record immediatamente precedente>> Tipo:N Obbl.S Errore:013-SF 013-SF	
                sPrintLine += PadLine(oE60.ProgressivoRecord, oE60.Length.ProgressivoRecord, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 11-15 Lungh. 5<<CODICE ENTE CREDITORE CODICE ENTE CREDITORE Deve essere presente nella "Tabella Enti Creditori e Beneficiari"APPENDICE B. Assume lo stesso valore dell'analogo campo del record E00>> Tipo:N Obbl.S Errore:502-SF 502-SF	
                sPrintLine += PadLine(oE60.IdentificativoAtto.IdentificativoFlusso.CodiceEnteCreditore, oE60.IdentificativoAtto.IdentificativoFlusso.Length.CodiceEnteCreditore, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 16-16 Lungh. 1<<TIPO UFFICIO TIPO UFFICIO Assume lo stesso valore dell'analogo campo del record E00>> Tipo:AN Obbl.S Errore:502-SP 502-SP	
                sPrintLine += PadLine(oE60.IdentificativoAtto.IdentificativoFlusso.TipoUfficio, oE60.IdentificativoAtto.IdentificativoFlusso.Length.TipoUfficio, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 17-22 Lungh. 6<<CODICE UFFICIO CODICE UFFICIO Assume lo stesso valore dell'analogo campo del record E00>> Tipo:AN Obbl.S Errore:502-SP 502-SP	
                sPrintLine += PadLine(oE60.IdentificativoAtto.IdentificativoFlusso.CodiceUfficio, oE60.IdentificativoAtto.IdentificativoFlusso.Length.CodiceUfficio, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 23-26 Lungh. 4<<ANNO DI EMISSIONE ATTO ANNO DI EMISSIONE ATTO Indica l'anno in cui è stato emesso l’atto di accertamento, non può essere maggiore dell’anno corrente, e coincide con l’anno del campo data emissione atto.>> Tipo:N Obbl.S Errore:033-SP 033-SP	
                sPrintLine += PadLine(oE60.IdentificativoAtto.AnnoEmissioneAtto, oE60.IdentificativoAtto.Length.AnnoEmissioneAtto, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 27-29 Lungh. 3<<TIPOLOGIA ATTO TIPOLOGIA ATTO Vale sempre 005>> Tipo:N Obbl.S Errore:039-SP 039-SP	
                sPrintLine += PadLine(oE60.IdentificativoAtto.TipologiaAtto, oE60.IdentificativoAtto.Length.TipologiaAtto, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 30-38 Lungh. 9<<NUMERO PARTITA NUMERO PARTITA Deve essere numerico e crescente nell'ambito della fornitura.>> Tipo:N Obbl.S Errore:501-SF 501-SF	
                sPrintLine += PadLine(oE60.IdentificativoAtto.NumeroPartita, oE60.IdentificativoAtto.Length.NumeroPartita, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 39-41 Lungh. 3<<PROGRESSIVO PARTITA PROGRESSIVO PARTITA Vale sempre 001.>> Tipo:N Obbl.S Errore:501-SF 501-SF	
                sPrintLine += PadLine(oE60.IdentificativoAtto.ProgressivoPartita, oE60.IdentificativoAtto.Length.ProgressivoPartita, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 42-43 Lungh. 2<<CODICE TIPO ATTO CODICE TIPO ATTO Codice che indica la tipologia dell'atto. Vedi NOTA 3 Fare riferimento alla tabella presente in APPENDICE P>> Tipo:AN Obbl.N Errore:055-SP 055-SP	
                sPrintLine += PadLine(oE60.IdentificativoAtto.CodiceTipoAtto, oE60.IdentificativoAtto.Length.CodiceTipoAtto, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 44-51 Lungh. 8<<DATA EMISSIONE ATTO DATA EMISSIONE ATTO Data in cui l'ente ha emesso l'atto nel formato (AAAAMMGG) Deve essere maggiore del 01/01/2020, minore della data di creazione flusso e minore della data di notifica atto.>> Tipo:N Obbl.S Errore:045-SP 045-SP	
                sPrintLine += PadLine(oE60.IdentificativoAtto.DataEmissioneAtto, oE60.IdentificativoAtto.Length.DataEmissioneAtto, objAccertamentiEsecutivi.Type.Data)
                'Campo Posizione 52-63 Lungh. 12<<NUMERO ATTO NUMERO ATTO Estremi dell'atto notificato Non può essere nullo>> Tipo:AN Obbl.S Errore:039-SP 039-SP	
                sPrintLine += PadLine(oE60.IdentificativoAtto.NumeroAtto, oE60.IdentificativoAtto.Length.NumeroAtto, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 64-71 Lungh. 8<<DATA NOTIFICA ATTO DATA NOTIFICA ATTO Data di notifica dell'atto al debitore, nella forma AAAAMMGG. Deve essere maggiore del 01/01/2020, maggiore o uguale della data emmissione atto e inferiore alla data creazione file presente sul record E00 di almeno 60 gg. L'anno (AAAA) deve essere maggiore o uguale del valore indicato nel campo anno di emessione atto>> Tipo:N Obbl.S Errore:045-SP 045-SP	
                sPrintLine += PadLine(oE60.IdentificativoAtto.DataNotificaAtto, oE60.IdentificativoAtto.Length.DataNotificaAtto, objAccertamentiEsecutivi.Type.Data)
                'Campo Posizione 72-111 Lungh. 40<<FILLER  FILLER>> Tipo:AN Obbl.N Errore:0	
                sPrintLine += PadLine(oE60.Filler, oE60.Length.Filler, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 112-121 Lungh. 10<<NUMERO DELIBERA NUMERO DELIBERA Estremi della delibera con cui l'ente affida la riscossione ai sensi dell'art.2,comma2,D.Lgs n.193 del 2016>> Tipo:AN Obbl.S Errore:039-SP 039-SP	
                sPrintLine += PadLine(oE60.NumeroDelibera, oE60.Length.NumeroDelibera, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 122-129 Lungh. 8<<DATA DELIBERA DATA DELIBERA Nel formato (AAAAMMGG) Deve essere inferiore alla data di trasmissione del flusso. Deve essere maggiore del 01/10/2016>> Tipo:N Obbl.S Errore:045-SP 045-SP	
                sPrintLine += PadLine(oE60.DataDelibera, oE60.Length.DataDelibera, objAccertamentiEsecutivi.Type.Data)
                'Campo Posizione 130-137 Lungh. 8<<DATA FINE VALIDITA' DELIBERA DATA FINE VALIDITA' DELIBERA Nel formato (AAAAMMGG) Se valorizzata deve essere una data valida>> Tipo:N Obbl.N Errore:045-SP 045-SP	
                sPrintLine += PadLine(oE60.DataFineValiditaDelibera, oE60.Length.DataFineValiditaDelibera, objAccertamentiEsecutivi.Type.Data)
                'Campo Posizione 138-138 Lungh. 1<<TIPOLOGIA SOSPENSIONE TIPOLOGIA SOSPENSIONE Identifica la tipologia di sospensione applicabile alla riscossione forzata. Può assumere i valori contenuti nell'APPENDICE Q>> Tipo:N Obbl.S Errore:055-SP 055-SP	
                sPrintLine += PadLine(oE60.TipologiaSospensione, oE60.Length.TipologiaSospensione, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 139-153 Lungh. 15<<IMPORTO TOTALE ATTO IMPORTO TOTALE ATTO Importo totale del carico affidato con l'atto. Somma IMPORTO ARTICOLO, record E50. Deve essere uguale o superiore ad Euro 10.>> Tipo:N Obbl.S Errore:039-SP A60-SP 039-SP A60-SP	
                sPrintLine += PadLine(oE60.ImportoTotaleAtto, oE60.Length.ImportoTotaleAtto, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 154-160 Lungh. 7<<TOTALE ARTICOLI ATTO TOTALE ARTICOLI ATTO Numero totale degli articoli, record E50, presenti nell'atto affidato.>> Tipo:N Obbl.S Errore:039-SP 039-SP	
                sPrintLine += PadLine(oE60.TotaleArticoliAtto, oE60.Length.TotaleArticoliAtto, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 161-168 Lungh. 8<<DATA TERMINE ULTIMO PAGAMENTO ATTO DATA TERMINE ULTIMO PAGAMENTO ATTO Corrisponde alla data termine ultimo per il pagamento delle somme intimate con l'atto. Deve essere maggiore della data notifica atto dal parte dell’ente ed espressa nel formato (AAAAMMGG). Non può essere maggiore della data di trasmissione flusso.>> Tipo:N Obbl.S Errore:045-SP B60-SP C60-SP 045-SP B60-SP C60-SP	
                sPrintLine += PadLine(oE60.DataTermineUltimoPagamentoAtto, oE60.Length.DataTermineUltimoPagamentoAtto, objAccertamentiEsecutivi.Type.Data)
                'Campo Posizione 169-169 Lungh. 1<<FLAG ENTE TERZO FLAG ENTE TERZO Flag che indica se è presente un’ente terzo diverso dall'ente impositore che ha emesso e notificato l’atto per contro dell’ente impositore S=PRESENTE N=NON PRESENTE Deve essere valorizzato con "S" solo se l'ente terzo è diverso dall'ente impositore>> Tipo:AN Obbl.S Errore:096-SP 096-SP	
                sPrintLine += PadLine(oE60.FlagEnteTerzo, oE60.Length.FlagEnteTerzo, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 170-229 Lungh. 60<<DENOMINAZIONE ENTE TERZO DENOMINAZIONE ENTE TERZO Denominazione dell’ente terzo che ha notificato l'atto. Se il flag ente terzo è valorizzato ad "S" la denominazione è obbligatoria.>> Tipo:AN Obbl.N Errore:039-SP 039-SP	
                sPrintLine += PadLine(oE60.DenominazioneEnteTerzo, oE60.Length.DenominazioneEnteTerzo, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 230-600 Lungh. 371<<FILLER>> Tipo:AN Obbl.S Errore:SPAZI	
                sPrintLine += PadLine(oE60.Filler, oE60.Length.Filler, objAccertamentiEsecutivi.Type.Stringa)

                sPrintLine = New MyUtility().ReplaceCharForFile(sPrintLine)
                If New MyUtility().WriteFile(sPathNameFile, sPrintLine) <= 0 Then
                    Return 0
                End If

                Return 1
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsCoattivo.CreaAccertamentiEsecutivi::PrintLineE60.errore::" + Err.Message)
                Return -1
            End Try
        End Function
        Public Function PrintLineE99(ByVal sPathNameFile As String, ByVal oE99 As objAccertamentiEsecutivi.E99) As Integer
            Dim sPrintLine As String = ""
            Try
                'Campo Posizione 1-3 Lungh. 3<<TIPO RECORD TIPO RECORD Vale Sempre "E99".>> Tipo:AN Obbl.S Errore:004-SF 004-SF	
                sPrintLine += oE99.TipoRecord
                'Campo Posizione 4-8 Lungh. 5<<CODICE ENTE CREDITORE CODICE ENTE CREDITORE Deve essere presente nella "Tabella Enti Creditori e Beneficiari" APPENDICE B Assume lo stesso valore dell'analogo campo del record E00>> Tipo:N Obbl.S Errore:502-SF 502-SF 504-SF	
                sPrintLine += PadLine(oE99.IdentificativoFlusso.CodiceEnteCreditore, oE99.IdentificativoFlusso.Length.CodiceEnteCreditore, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 9-9 Lungh. 1<<TIPO UFFICIO TIPO UFFICIO Tipologia ufficio ente creditore APPENDICE B. Assume lo stesso valore dell'analogo campo del record E00>> Tipo:AN Obbl.S Errore:502-SF 502-SF 504-SF	
                sPrintLine += PadLine(oE99.IdentificativoFlusso.TipoUfficio, oE99.IdentificativoFlusso.Length.TipoUfficio, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 10-15 Lungh. 6<<CODICE UFFICIO CODICE UFFICIO Codice ufficio dell'ente creditore APPENDICE B. Assume lo stesso valore dell'analogo campo del record E00>> Tipo:AN Obbl.S Errore:502-SF 502-SF 504-SF	
                sPrintLine += PadLine(oE99.IdentificativoFlusso.CodiceUfficio, oE99.IdentificativoFlusso.Length.CodiceUfficio, objAccertamentiEsecutivi.Type.Stringa)
                'Campo Posizione 16-23 Lungh. 8<<DATA CREAZIONE FLUSSO DATA CREAZIONE FLUSSO Data in cui l'ente crea il file di carico espressa nel formato (AAAAMMGG). Deve essere minore o uguale alla data trasmissione e non deve essere antecedente alla data di trasmissione di oltre 15 giorni. Assume lo stesso valore dell'analogo campo del record E00>> Tipo:N Obbl.S Errore:045-SF 045-SF	
                sPrintLine += PadLine(oE99.DataCreazioneFlusso, oE99.Length.DataCreazioneFlusso, objAccertamentiEsecutivi.Type.Data)
                'Campo Posizione 24-33 Lungh. 10<<ESTREMI FLUSSO ESTREMI FLUSSO Identificativo del file trasmesso, espresso nella forma AAAA7NNNNN dove AAAA=Anno flusso, 7=fisso per indicare la tipologia accertamento esecutivo NNNNN=Progressivo crescente L’anno AAAA coincide con l’anno di creazione del flusso Assume lo stesso valore dell'analogo campo del record E00>> Tipo:N Obbl.S Errore:501-SF 501-SF	
                sPrintLine += PadLine(oE99.EstremiFlusso, oE99.Length.EstremiFlusso, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 34-40 Lungh. 7<<TOTALE RECORD TOTALE RECORD Numero di record presenti nel flusso>> Tipo:N Obbl.S Errore:503-SF 503-SF	
                sPrintLine += PadLine(oE99.TotaleRecord, oE99.Length.TotaleRecord, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 41-47 Lungh. 7<<TOTALE RECORD E20 TOTALE RECORD E20 Numero di record E20 presenti nel flusso>> Tipo:N Obbl.S Errore:503-SF 503-SF	
                sPrintLine += PadLine(oE99.TotaleRecordE20, oE99.Length.TotaleRecordE20, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 48-54 Lungh. 7<<TOTALE RECORD E23 TOTALE RECORD E23 Numero di record E23 presenti nel flusso>> Tipo:N Obbl.S Errore:503-SF 503-SF	
                sPrintLine += PadLine(oE99.TotaleRecordE23, oE99.Length.TotaleRecordE23, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 55-61 Lungh. 7<<TOTALE RECORD E50 TOTALE RECORD E50 Numero di record E50 presenti nel flusso>> Tipo:N Obbl.S Errore:503-SF 503-SF	
                sPrintLine += PadLine(oE99.TotaleRecordE50, oE99.Length.TotaleRecordE50, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 62-68 Lungh. 7<<TOTALE RECORD E60 TOTALE RECORD E60 Numero di record E60 presenti nel flusso>> Tipo:N Obbl.S Errore:503-SF 503-SF	
                sPrintLine += PadLine(oE99.TotaleRecordE60, oE99.Length.TotaleRecordE60, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 69-83 Lungh. 15<<TOTALE CARICO FLUSSO TOTALE CARICO FLUSSO Contiene la somma totale dei carichi dei singoli atti>> Tipo:N Obbl.S Errore:503-SF 503-SF	
                sPrintLine += PadLine(oE99.TotaleCaricoFlusso, oE99.Length.TotaleCaricoFlusso, objAccertamentiEsecutivi.Type.Numero)
                'Campo Posizione 84-600 Lungh. 517<<FILLER>> Tipo:AN Obbl.N Errore:SPAZI	
                sPrintLine += PadLine(oE99.Filler, oE99.Length.Filler, objAccertamentiEsecutivi.Type.Stringa)

                sPrintLine = New MyUtility().ReplaceCharForFile(sPrintLine)
                If New MyUtility().WriteFile(sPathNameFile, sPrintLine) <= 0 Then
                    Return 0
                End If

                Return 1
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsCoattivo.CreaobjAccertamentiEsecutivi.PrintLineE99.errore::" + Err.Message)
                Return -1
            End Try
        End Function
        Public Function PadLine(sValLine As String, LenValLine As Integer, TypeValLine As String) As String
            Dim sPadLine As String = ""
            Try
                Select Case TypeValLine
                    Case objAccertamentiEsecutivi.Type.Numero
                        sPadLine = sValLine.PadLeft(LenValLine, "0").Substring(0, LenValLine)
                    Case objAccertamentiEsecutivi.Type.Data
                        sPadLine = Format(CDate(sValLine), "yyyyMMdd").PadLeft(LenValLine, "0").Substring(0, LenValLine)
                    Case Else
                        sPadLine = sValLine.PadRight(LenValLine, " ").Substring(0, LenValLine)
                End Select
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsCoattivo.CreaobjAccertamentiEsecutivi.PadLine.errore::" + Err.Message)
                sPadLine = ""
            End Try
            Return sPadLine
        End Function
    End Class
#End Region
End Class
