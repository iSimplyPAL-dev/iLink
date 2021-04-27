Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Xml
Imports log4net
Imports Microsoft.VisualBasic

Public Class GetComuni
    '//La Classe Viene chiamata dalla pagina searchComuni.aspx
    Private Shared Log As ILog = LogManager.GetLogger(GetType(GetComuni))
    Dim sqlCmd As SqlCommand = New SqlCommand()
    Dim sqlConn As New SqlConnection()

    Public Function GetComuniRow(ByVal strCap As String, ByVal strComune As String,
                                                      ByVal strProvincia As String,
                                                      ByRef strError As String) As Long


        Dim sqlParm As New SqlParameter()
        Dim lngRowCount As Long

        '===========================================================
        'Apertura connessione 
        sqlConn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("connString")
        sqlConn.Open()

        'Strored Procedure  che estrae i Dati dall Tabella Comuni

        sqlCmd.CommandText = "sp_Comuni"
        sqlCmd.Connection = sqlConn
        sqlCmd.CommandType = CommandType.StoredProcedure
        sqlParm = sqlCmd.Parameters.Add("@RowCount", SqlDbType.Int)
        sqlParm.Direction = ParameterDirection.ReturnValue

        sqlCmd.Parameters.Add("@CAP", SqlDbType.NVarChar, 5, "CAP")
        If Len(strCap) = 0 Then
            sqlCmd.Parameters("@CAP").Value = System.DBNull.Value
        Else
            sqlCmd.Parameters("@CAP").Value = strCap
        End If
        sqlCmd.Parameters.Add("@COMUNE", SqlDbType.NVarChar, 35, "COMUNE")
        If Len(strComune) = 0 Then
            sqlCmd.Parameters("@COMUNE").Value = System.DBNull.Value
        Else
            sqlCmd.Parameters("@COMUNE").Value = strComune
        End If
        sqlCmd.Parameters.Add("@PROVINCIA", SqlDbType.NVarChar, 2, "PROVINCIA")
        If Len(strProvincia) = 0 Then
            sqlCmd.Parameters("@PROVINCIA").Value = System.DBNull.Value
        Else
            sqlCmd.Parameters("@PROVINCIA").Value = strProvincia
        End If
        Try
            Log.Debug(Utility.Costanti.LogQuery(sqlCmd))
            sqlCmd.ExecuteNonQuery()
        Catch ex As SqlException
            'Chiudo la connessione e segnalo l'errore
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GetComunu.GetComuniRow.errore: ", ex)
            CloseConnection()
            strError = ex.Message.ToString

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GetComunu.GetComuniRow.errore: ", ex)
            CloseConnection()
            strError = ex.Message.ToString()
        End Try

        'Parametro di ritorno
        lngRowCount = sqlCmd.Parameters("@RowCount").Value



        Return lngRowCount

    End Function
    Public Sub getComuni(ByRef strComune As String,
                                            ByRef strCap As String,
                                            ByRef strProvincia As String,
                                            ByRef strIDENTIFICATIVO As String,
                                            ByRef strError As String)

        'Restituisce un Datareader con lo stesso Oggetto command
        'caricato con la STOREPROCEDURE
        Try
            Dim rdr As SqlDataReader
            Log.Debug(Utility.Costanti.LogQuery(sqlCmd))
            rdr = sqlCmd.ExecuteReader
            While rdr.Read()
                strComune = rdr("COMUNE").ToString()
                strIDENTIFICATIVO = rdr("IDENTIFICATIVO").ToString()
                strProvincia = rdr("PV").ToString()
                strCap = rdr("CAP").ToString()
            End While
            rdr.Close()
            CloseConnection()
        Catch ex As SqlException
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GetComunu.getComuni.errore: ", ex)
            strError = ex.Message.ToString
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GetComunu.getComuni.errore: ", ex)
            strError = ex.Message.ToString
        End Try
    End Sub
    Private Sub CloseConnection()
        'Chiusura connessione aperta nella funzione GetComuniRow
        Try
            If sqlConn.State = ConnectionState.Open Then
                sqlConn.Close()
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.GetComunu.CloseConnection.errore: ", ex)
        End Try
    End Sub
    Public Function sqlCommand() As SqlCommand
        sqlCommand = sqlCmd
        Return sqlCommand
    End Function
    Public Function sqlConnect() As SqlConnection
        sqlConnect = sqlConn
        Return sqlConnect
    End Function
End Class



