Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports log4net

Public Class LoadComune

    Public Function Load_Comune(ByVal sqlConn As SqlConnection, ByRef lngTotalRecord As Int32, ByVal strComune As String, ByVal strCAP As String, ByVal strPROV As String) As SqlDataReader


        dim sSQL as string
        Dim strOrderBy As String
        Dim strWhere As String
        Dim sqlCom As New SqlCommand()
        Dim strSqlCount As String

        'Definizione della stringa SQL

        sqlCom.CommandType = CommandType.Text

        strOrderBy = " Order By COMUNE"

        strWhere = ""

        If (strCAP <> "") Then
            If (strWhere <> "") Then
                strWhere = strWhere & " and CAP like '" & strCAP & "%'"
            Else
                strWhere = strWhere & " CAP like '" & strCAP & "%'"
            End If
        End If
        If (strComune <> "") Then
            If (strWhere <> "") Then
                strWhere = strWhere & " and COMUNE like '" & Replace(strComune, "'", "''") & "%'"
            Else
                strWhere = strWhere & " COMUNE like '" & Replace(strComune, "'", "''") & "%'"
            End If
        End If
        If (strPROV <> "") Then
            If (strWhere <> "") Then
                strWhere = strWhere & " and PV like '" & strPROV & "%'"
            Else
                strWhere = strWhere & " PV like '" & strPROV & "%'"
            End If
        End If
        If (strWhere <> "") Then
            sSQL="SELECT * FROM COMUNI WHERE " & strWhere & strOrderBy
            strSqlCount = "select Count(*) FROM comuni WHERE " & strWhere
        Else
            sSQL="SELECT * FROM comuni" & strOrderBy
            strSqlCount = "select Count(*) FROM comuni"

        End If
        On Error Resume Next
        Dim _cmdTotalRecord As SqlCommand = New SqlCommand(strSqlCount, sqlConn)
        sqlCom = New SqlCommand(sSQL, sqlConn)

        If Err.Number <> 0 Then
            'Errore
        End If
        Err.Clear()
        On Error GoTo 0
        lngTotalRecord = _cmdTotalRecord.ExecuteScalar
        Load_Comune = sqlCom.ExecuteReader()
        On Error GoTo Page_Load_ERR



Page_Load_ERR:
        'Gestione errore

    End Function
End Class
