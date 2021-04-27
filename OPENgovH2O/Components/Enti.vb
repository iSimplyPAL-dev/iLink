Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports log4net

Public Class GetListEnti
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
    Public oConn As SqlConnection
    Public oComm As SqlCommand
    Public lngRecordCount As Integer
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
End Class
Public Class DetailsEnti
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
    Public dsEnti As DataSet
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
End Class

Public Class DBEnti
    Dim iDB As New DBAccess.getDBobject()
    Private Shared Log As ILog = LogManager.GetLogger(GetType(DBEnti))
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
    '
    ' GetListaComuni  Function
    '
    ' Ritorna i dati necessari per poplare il DataGrid
    ' 
    '   
    'Pagina Chiamante: Selezioni/ListaBanche.aspx
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
    'Public Function GetListaEnti(ByVal CodAmbiente As Integer) As GetListEnti

    '    Dim GetListEnti As New GetListEnti()
    '    Dim oConn As New SqlConnection()
    '    Dim oCmd As New SqlCommand()

    '    GetListEnti.lngRecordCount = iDB.RunSPReturnToGrid("sp_ENTI", oConn, oCmd, New SqlParameter("@CODAMBIENTE", CodAmbiente))


    '    GetListEnti.oConn = oConn
    '    GetListEnti.oComm = oCmd

    '    Return GetListEnti

    'End Function
    Public Function GetListaEnti(ByVal CodAmbiente As Integer) As DataView
        Dim cmdMyCommand As New SqlCommand
        Dim myDv As DataView = Nothing
        Try
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "sp_ENTI"
            cmdMyCommand.Parameters.Add(New SqlParameter("@CODAMBIENTE", CodAmbiente))
            myDv = iDB.GetDataView(cmdMyCommand)
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBEnti.GetListaEnti.errore: ", ex)
            Throw New Exception("GetListaEnti::", ex)
        End Try
        Return myDv
    End Function

    Public Function GetDetailsEnti(ByVal CodAmbiente As Integer, ByVal CodEnte As String) As DetailsEnti
        Dim DetailEnti As New DetailsEnti
        DetailEnti.dsEnti = iDB.RunSPReturnDataSet("sp_ENTI", "TP_ENTI", New SqlParameter("@CODAMBIENTE", CodAmbiente), New SqlParameter("@IDENTE", CodEnte))
        Return DetailEnti
    End Function

    'Public Function GetDetailsEnte() As DetailsEnti
    '    Dim DetailEnte As New DetailsEnti
    '    dim sSQL as string = "SELECT * FROM TP_ENTE WHERE CODENTE = " & CodEnte

    '    DetailEnte.dsEnti = DBAccess.RunSQLReturnDataSet(sSQL)
    '    Return DetailEnte
    'End Function
End Class
