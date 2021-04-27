Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports log4net
Imports Utility

Namespace DBAccess
    Public Class getDBobject
        ''' <summary>
        ''' classe per la gestione del dbg
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared Log As ILog = LogManager.GetLogger(GetType(getDBobject))
        Private _const As New Costanti()
        Public m_Connection As String
        Private m_Conn As SqlConnection
        Private m_Comm As SqlCommand
        Private m_Trans As SqlTransaction

        Public Sub New()
        End Sub
        Public Sub New(ByVal _ConnectionString As String)
            'Se non si usa la stringa di connessione di  default del WebConfig
            m_Connection = _ConnectionString
        End Sub

#Region "ACCESS"
        '        '================OLEDB====================================================
        '        'Connessione OLEDB
        '        Protected Function GetConnectionAccess() As OleDbConnection
        '            Dim ret_conn As OleDbConnection
        '            ret_conn = New OleDbConnection(m_Connection)
        '            ret_conn.Open()
        '            GetConnectionAccess = ret_conn
        '        End Function
        '        Public Function GetDataReaderAccess(ByVal sSQL As String) As OleDbDataReader
        '            Dim cn As OleDbConnection = GetConnectionAccess()
        '            Dim rdr As OleDbDataReader
        '            Dim cmd As New OleDbCommand(sSQL, cn)
        '            rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
        '            cmd.Dispose()
        '            Return rdr
        '        End Function
        '        Public Sub RunActionQueryAccess(ByVal sSQL As String)
        '            Dim cn As OleDbConnection = GetConnectionAccess()
        '            Dim cmd As New OleDbCommand(sSQL, cn)
        '            Try
        '                cmd.ExecuteNonQuery()
        '                cmd.Dispose()
        '            Finally
        '                CloseConnectionAccess(cn)
        '            End Try
        '        End Sub
        '        Protected Sub CloseConnectionAccess(ByVal conn As OleDbConnection)
        '            conn.Close()
        '            conn = Nothing
        '        End Sub
        '        '========================================================================
#End Region
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetConnection() As SqlConnection
            Dim ret_conn As New SqlConnection
            Try
                If m_Connection = String.Empty Then
                    ret_conn = New SqlConnection(ConstSession.StringConnection) 'ConfigurationManager.AppSettings("connString")
                Else
                    ret_conn = New SqlConnection(m_Connection)
                End If

                ret_conn.Open()
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.getDBobject.GetConnection.errore: ", ex)
            End Try
            GetConnection = ret_conn
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="conn"></param>
        ''' <remarks></remarks>
        Protected Sub CloseConnection(ByVal conn As SqlConnection)
            conn.Close()
            conn = Nothing
        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sSQL"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetDataReader(ByVal sSQL As String) As SqlDataReader
            Dim cmdMyCommand As New SqlCommand

            Try
                Dim myCn As SqlConnection = GetConnection()
                Dim myDr As SqlDataReader

                cmdMyCommand = New SqlCommand(sSQL, myCn)
                cmdMyCommand.CommandTimeout = 0
                myDr = cmdMyCommand.ExecuteReader(CommandBehavior.CloseConnection)
                Return myDr
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.getDBobject.GetDataReader.errore: ", ex)
                Log.Debug("GetDataReader::errore::query::" & sSQL, ex)
                Throw New Exception("GetDataReader::", ex)
            Finally
                cmdMyCommand.Dispose()
            End Try
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="cmdMyCommand"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetDataReader(ByVal cmdMyCommand As SqlCommand) As SqlDataReader
            Try
                Dim myCn As SqlConnection = GetConnection()
                Dim myDr As SqlDataReader

                cmdMyCommand.Connection = myCn
                cmdMyCommand.CommandTimeout = 0
                myDr = cmdMyCommand.ExecuteReader(CommandBehavior.CloseConnection)
                Return myDr
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.getDBobject.GetDataReader.errore: ", ex)
                Throw New Exception("GetDataReader::", ex)
            Finally
                cmdMyCommand.Dispose()
            End Try
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sSQL"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetDataView(ByVal sSQL As String) As DataView
            Dim dvMyDati As New DataView
            Try
                Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.View, sSQL)
                    dvMyDati = ctx.GetDataView(sSQL, "dati")
                    ctx.Dispose()
                End Using
                Return dvMyDati
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.getDBobject.GetDataView.errore: ", ex)
                Throw New Exception("getDBobject.GetDataView::", ex)
            End Try
        End Function
        'Public Function GetDataView(ByVal sSQL As String) As DataView
        '    Try
        '        Dim myCn As SqlConnection = GetConnection()
        '        Dim myDs As New DataSet
        '        Dim myAdapter As New SqlDataAdapter

        '        Dim cmdMyCommand As New SqlCommand(sSQL, myCn)
        '        cmdMyCommand.CommandTimeout = 0
        '        myAdapter.SelectCommand = cmdMyCommand
        '        myAdapter.Fill(myDs, "dati")
        '        CloseConnection(myCn)
        '        myAdapter.Dispose()
        '        Return myDs.Tables(0).DefaultView
        '    Catch ex As Exception
        '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.getDBobject.GetDataView.errore: ", ex)
        '        Throw New Exception("GetDataView::", ex)
        '    End Try
        'End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="cmdMyCommand"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetDataView(ByVal cmdMyCommand As SqlCommand) As DataView
            Dim myDs As New DataSet
            Dim myAdapter As New SqlDataAdapter
            Try
                Dim myCn As SqlConnection = GetConnection()

                cmdMyCommand.Connection = myCn
                cmdMyCommand.CommandTimeout = 0
                myAdapter.SelectCommand = cmdMyCommand
                myAdapter.Fill(myDs, "dati")
                Return myDs.Tables(0).DefaultView
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.getDBobject.GetDataView.errore: ", ex)
                Throw New Exception("GetDataView::", ex)
            Finally
                myAdapter.Dispose()
                If cmdMyCommand.Connection.State = ConnectionState.Open Then
                    cmdMyCommand.Connection.Close()
                End If
                cmdMyCommand.Dispose()
            End Try
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sSQL"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetDataSet(ByVal sSQL As String) As DataSet
            Dim dsMyDati As New DataSet
            Try
                Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.View, sSQL)
                    dsMyDati = ctx.GetDataSet(sSQL, "dati")
                    ctx.Dispose()
                End Using
                Return dsMyDati
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.getDBobject.GetDataSet.errore: ", ex)
                Throw New Exception("getDBobject.GetDataSet::", ex)
            End Try
        End Function
        'Public Function GetDataSet(ByVal sSQL As String) As DataSet
        '    Try
        '        Dim myCn As SqlConnection = GetConnection()
        '        Dim myDs As New DataSet
        '        Dim myAdapter As New SqlDataAdapter

        '        Dim cmdMyCommand As New SqlCommand(sSQL, myCn)
        '        cmdMyCommand.CommandTimeout = 0
        '        myAdapter.SelectCommand = cmdMyCommand
        '        myAdapter.Fill(myDs, "dati")
        '        CloseConnection(myCn)
        '        myAdapter.Dispose()
        '        Return myDs
        '    Catch ex As Exception
        '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.getDBobject.GetDataSet.errore: ", ex)
        '        Throw New Exception("GetDataSet::", ex)
        '    End Try
        'End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="cmdMyCommand"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetDataSet(ByVal cmdMyCommand As SqlCommand) As DataSet
            Try
                Dim myCn As SqlConnection = GetConnection()
                Dim myDs As New DataSet
                Dim myAdapter As New SqlDataAdapter

                cmdMyCommand.Connection = myCn
                cmdMyCommand.CommandTimeout = 0
                myAdapter.SelectCommand = cmdMyCommand
                myAdapter.Fill(myDs, "dati")
                CloseConnection(myCn)
                myAdapter.Dispose()
                Return myDs
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.getDBobject.GetDataSet.errore: ", ex)
                Throw New Exception("GetDataSet::", ex)
            Finally
            End Try
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="ProcedureName"></param>
        ''' <param name="commandParameters"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetDataTable(ByVal ProcedureName As String, ByVal ParamArray commandParameters() As SqlParameter) As DataTable
            Try
                Dim myCn As SqlConnection = GetConnection()
                Dim myDs As New DataSet
                Dim myAdapter As New SqlDataAdapter

                Dim cmdMyCommand As New SqlCommand(ProcedureName, myCn)
                cmdMyCommand.CommandType = CommandType.StoredProcedure
                cmdMyCommand.CommandTimeout = 0
                For Each myParam As SqlParameter In commandParameters
                    myParam = cmdMyCommand.Parameters.Add(myParam)
                    myParam.Direction = ParameterDirection.Input
                Next

                myAdapter.SelectCommand = cmdMyCommand
                myAdapter.Fill(myDs, "dati")
                CloseConnection(myCn)
                myAdapter.Dispose()
                Return myDs.Tables(0)
            Catch ex As Exception

                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.getDBobject.GetDataTable.errore: ", ex)
                Throw New Exception("GetDataTable::", ex)
            End Try
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sSQL"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ExecuteNonQuery(ByVal sSQL As String) As Integer
            Dim cn As SqlConnection = GetConnection()
            Dim cmd As New SqlCommand(sSQL, cn)
            Dim myRet As Integer = -1
            Try
                myRet = cmd.ExecuteNonQuery()
                cmd.Dispose()
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.getDBobject.ExecuteNonQuery.errore: ", ex)
                Log.Debug("ExecuteNonQuery::errore::SQL::" & sSQL, ex)
                Throw New Exception("ExecuteNonQuery::", ex)
            Finally
                CloseConnection(cn)
            End Try
            Return myRet
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="strSP"></param>
        ''' <param name="ParamOutput"></param>
        ''' <param name="commandParameters"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ExecuteNonQuery(ByVal strSP As String, ByVal ParamOutput As SqlParameter, ByVal ParamArray commandParameters() As SqlParameter) As Integer
            Dim cn As SqlConnection = GetConnection()
            Dim retVal As Integer = 0

            Try
                Dim cmd As New SqlCommand(strSP, cn)
                cmd.CommandType = CommandType.StoredProcedure

                Dim p As SqlParameter
                For Each p In commandParameters
                    p = cmd.Parameters.Add(p)
                    p.Direction = ParameterDirection.Input
                Next
                If Not ParamOutput Is Nothing Then
                    p = cmd.Parameters.Add(ParamOutput)
                    p.Direction = ParameterDirection.InputOutput
                    cmd.ExecuteNonQuery()
                    retVal = cmd.Parameters(ParamOutput.ParameterName).Value
                Else
                    retVal = cmd.ExecuteNonQuery()
                End If
                cmd.Dispose()
            Catch ex As Exception

                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.getDBobject.ExecuteNonQuery.errore: ", ex)
                retVal = -1
                Throw New Exception("ExecuteNonQuery::", ex)
            Finally
                CloseConnection(cn)
            End Try
            Return retVal
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="cmd"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ExecuteNonQuery(ByVal cmd As SqlCommand) As Integer
            Dim cn As SqlConnection = GetConnection()
            Dim retVal As Integer = 0

            Try
                cmd.Connection = cn
                retVal = cmd.ExecuteNonQuery()
                cmd.Dispose()
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.getDBobject.ExecuteNonQuery.errore: ", ex)
                Log.Debug("ExecuteNonQuery::errore::SQL::" & cmd.CommandText, ex)
                retVal = -1
                Throw New Exception("ExecuteNonQuery::", ex)
            Finally
                CloseConnection(cn)
            End Try
            Return retVal
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="strSP"></param>
        ''' <param name="commandParameters"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function RunSPReturnRS(ByVal strSP As String, ByVal ParamArray commandParameters() As SqlParameter) As SqlDataReader
            Dim cn As SqlConnection = GetConnection()
            Dim rdr As SqlDataReader
            rdr = Nothing
            Dim cmd As New SqlCommand(strSP, cn)
            cmd.CommandType = CommandType.StoredProcedure

            Dim p As SqlParameter
            Try
                For Each p In commandParameters
                    p = cmd.Parameters.Add(p)
                    p.Direction = ParameterDirection.Input
                Next

                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                cmd.Dispose()
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.getDBobject.RunSPReturnRS.errore: ", ex)
            End Try

            Return rdr
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="strSP"></param>
        ''' <param name="DataTableName"></param>
        ''' <param name="commandParameters"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function RunSPReturnDataSet(ByVal strSP As String, ByVal DataTableName As String, ByVal ParamArray commandParameters() As SqlParameter) As DataSet

            Dim cn As SqlConnection = GetConnection()

            Dim ds As New DataSet

            Dim da As New SqlDataAdapter(strSP, cn)
            da.SelectCommand.CommandType = CommandType.StoredProcedure

            Dim p As SqlParameter
            Try
                For Each p In commandParameters
                    da.SelectCommand.Parameters.Add(p)
                    p.Direction = ParameterDirection.Input
                Next

                da.Fill(ds, DataTableName)

                CloseConnection(cn)
                da.Dispose()

            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.getDBobject.RunSPReturnDataSet.errore: ", ex)
            End Try
            Return ds
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sSql"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function RunSQLReturnDataSet(ByVal sSQL As String) As DataSet

            Dim cn As SqlConnection = GetConnection()

            Dim ds As New DataSet

            Dim da As New SqlDataAdapter(sSQL, cn)
            da.SelectCommand.CommandType = CommandType.Text

            da.Fill(ds)

            CloseConnection(cn)
            da.Dispose()

            Return ds

        End Function

        '**************************************************************
        'Public Sub ConnectionTransaction()
        '    Try
        '        Dim cn As SqlConnection = GetConnection()
        '        m_Conn = cn
        '        Dim sqlTrans As SqlTransaction
        '        m_Trans = sqlTrans
        '        m_Trans = m_Conn.BeginTransaction
        '        m_Comm = New SqlCommand
        '    Catch ex As Exception
        ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.getDBobject.ConnectionTransaction.errore: ", ex)
        '        Throw ex
        '    End Try
        'End Sub

        'Public Sub ConnectionTransactionClose(ByVal blnCommit As Boolean)
        '    Try
        '        If blnCommit Then
        '            m_Trans.Commit()
        '        End If
        '        m_Comm.Dispose()
        '        m_Conn.Close()
        '    Catch ex As Exception
        ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.getDBobject.ConnectionTransactionClose.errore: ", ex)
        '        Throw ex
        '    End Try
        'End Sub
        '**************************************************************
        'Public Function RunActionQueryIdentiy(ByVal sSQL As String) As Integer
        '    Dim cn As SqlConnection = GetConnection()
        '    Dim cmd As New SqlCommand(sSQL, cn)
        '    Dim IDValue As Integer
        '    Try
        '        IDValue = cmd.ExecuteScalar()
        '        cmd.Dispose()
        '    Catch ex As Exception
        ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.getDBobject.RunActionQueryIdentity.errore: ", ex)
        '    Finally
        '        CloseConnection(cn)
        '    End Try
        '    Return IDValue
        'End Function

        'Public Sub RunActionQueryTransaction_ExecuteNonQuery(ByVal sSQL As String)

        '    m_Comm.Connection = m_Conn
        '    m_Comm.CommandText =sSQL
        '    m_Comm.Transaction = m_Trans
        '    Try
        '        m_Comm.ExecuteNonQuery()
        '    Catch ex As Exception
        ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.getDBobject.RunActionQueryTransaction_ExecuteNonQuery.errore: ", ex)
        '        m_Trans.Rollback()
        '        m_Comm.Dispose()
        '        m_Conn.Close()
        '        Throw ex
        '    End Try
        'End Sub

        'Public Function RunActionQueryTransaction_ExecuteScalar(ByVal sSQL As String) As Long
        '    Dim IDVALUE As Long

        '    m_Comm.Connection = m_Conn
        '    m_Comm.CommandText =sSQL
        '    m_Comm.Transaction = m_Trans
        '    Try
        '        IDVALUE = m_Comm.ExecuteScalar
        '        Return IDVALUE
        '    Catch ex As Exception
        ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.getDBobject.RunActionQueryTransaction_ExecuteScalar.errore: ", ex)
        '        m_Trans.Rollback()
        '        m_Comm.Dispose()
        '        m_Conn.Close()
        '        Throw ex
        '    End Try

        'End Function

        'Public Sub RunSP(ByVal strSP As String, ByVal ParamArray commandParameters() As SqlParameter)
        '    Dim cn As SqlConnection = GetConnection()
        '    Dim retVal As Integer

        '    Try
        '        Dim cmd As New SqlCommand(strSP, cn)
        '        cmd.CommandType = CommandType.StoredProcedure

        '        Dim p As SqlParameter
        '        For Each p In commandParameters
        '            p = cmd.Parameters.Add(p)
        '            p.Direction = ParameterDirection.Input
        '        Next
        '        cmd.ExecuteNonQuery()

        '        cmd.Dispose()
        '    Catch ex As Exception
        ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.getDBobject.RunSP.errore: ", ex)
        '    Finally
        '        CloseConnection(cn)
        '    End Try
        'End Sub

        'Public Function RunSPReturnInteger(ByVal strSP As String, ByVal ParamArray commandParameters() As SqlParameter) As Integer

        '    Dim cn As SqlConnection = GetConnection()
        '    Dim retVal As Integer

        '    Try

        '        Dim cmd As New SqlCommand(strSP, cn)
        '        cmd.CommandType = CommandType.StoredProcedure

        '        Dim p As SqlParameter
        '        For Each p In commandParameters
        '            p = cmd.Parameters.Add(p)
        '            p.Direction = ParameterDirection.Input
        '        Next

        '        p = cmd.Parameters.Add(New SqlParameter("@RetVal", SqlDbType.Int))
        '        p.Direction = ParameterDirection.Output

        '        cmd.ExecuteNonQuery()
        '        retVal = cmd.Parameters("@RetVal").Value
        '        cmd.Dispose()
        '    Catch ex As Exception
        ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.getDBobject.RunSPReturnInteger.errore: ", ex)
        '    Finally
        '        CloseConnection(cn)
        '    End Try

        '    Return retVal

        'End Function

        'Public Function RunSPReturnRowCount(ByVal strSP As String, ByVal ParamArray commandParameters() As SqlParameter) As Integer

        '    Dim cn As SqlConnection = GetConnection()
        '    Dim retVal As Integer

        '    Try

        '        Dim cmd As New SqlCommand(strSP, cn)
        '        cmd.CommandType = CommandType.StoredProcedure

        '        Dim p As SqlParameter
        '        For Each p In commandParameters
        '            p = cmd.Parameters.Add(p)
        '            p.Direction = ParameterDirection.Input
        '        Next

        '        p = cmd.Parameters.Add("@RowCount", SqlDbType.Int)
        '        p.Direction = ParameterDirection.ReturnValue



        '        cmd.ExecuteNonQuery()
        '        retVal = cmd.Parameters("@RowCount").Value
        '        cmd.Dispose()
        '    Catch ex As Exception
        ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.getDBobject.RunSPReturnRowCount.errore: ", ex)
        '    Finally
        '        CloseConnection(cn)
        '    End Try

        '    Return retVal

        'End Function
        'Public Function RunSPReturnToGrid(ByVal strSP As String, ByRef oConn As SqlConnection, ByRef oComm As SqlCommand, ByVal ParamArray commandParameters() As SqlParameter) As Integer
        '    ''''Utilizzata per popolare una griglia da una storedprocedure
        '    ''''Deve tornare un oggettocommand,e un  oggetto connection

        '    Dim cn As SqlConnection = GetConnection()
        '    Dim retVal As Integer
        'Try
        '    oConn = cn
        '    Dim cmd As New SqlCommand(strSP, cn)
        '    cmd.CommandType = CommandType.StoredProcedure

        '    Dim p As SqlParameter
        '    For Each p In commandParameters
        '        p = cmd.Parameters.Add(p)
        '        p.Direction = ParameterDirection.Input
        '    Next

        '    p = cmd.Parameters.Add("@RowCount", SqlDbType.Int)
        '    p.Direction = ParameterDirection.ReturnValue

        '    cmd.ExecuteNonQuery()
        '    oComm = cmd
        '    retVal = cmd.Parameters("@RowCount").Value
        '    Catch ex As Exception
        ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.getDBobject.RunSPReturnToGrid.errore: ", ex)
        'End Try
        '    Return retVal
        'End Function

        'Public Function GetNewId(ByRef strNomeTabella As String) As Long
        '    'ANTONELLO
        '    'funzione che estrae il nuovo ID
        '    dim sSQL as string
        '    Dim sqlTrans As SqlTransaction
        '    Dim lngMaxId As Long
        '    Dim oComm As SqlCommand
        '    Dim ret_conn As SqlConnection
        '    ret_conn = New SqlConnection(ConstSession.StringConnection)

        '    ret_conn.Open()
        '    sqlTrans = ret_conn.BeginTransaction(IsolationLevel.Serializable)
        '    Try

        '        sSQL="SELECT MAXID FROM CONTATORI  WHERE NOME_TABELLA ='" & strNomeTabella & "'"
        '        oComm = New SqlCommand(sSQL, ret_conn, sqlTrans)
        '        Dim dr As SqlDataReader = oComm.ExecuteReader
        '        If dr.Read Then
        '            lngMaxId = dr.Item("MAXID")
        '            lngMaxId = lngMaxId + _const.VALUE_INCREMENT
        '        End If
        '        dr.Close()
        '        sSQL="UPDATE CONTATORI SET MAXID=" & lngMaxId & " WHERE NOME_TABELLA ='" & strNomeTabella & "'"
        '        oComm = New SqlCommand(sSQL, ret_conn, sqlTrans)
        '        oComm.ExecuteNonQuery()
        '        sqlTrans.Commit()
        '    Catch ex As Exception
        ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.getDBobject.RunSQLReturnDataSet.errore: ", ex)
        '        sqlTrans.Rollback()
        '        Throw
        '    Finally
        '        oComm.Dispose()
        '        ret_conn.Close()
        '    End Try

        '    GetNewId = lngMaxId

        'End Function
    End Class
End Namespace
