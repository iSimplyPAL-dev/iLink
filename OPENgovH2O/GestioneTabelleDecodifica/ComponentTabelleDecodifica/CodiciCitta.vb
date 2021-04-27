Imports System
Imports System.Data
Imports System.Data.SqlClient

Namespace TabelleDiDecodifica
    Public Class DBCodiciCitta
        Inherits TabelleDiDecodifica.DetailCodiciCitta

        Dim iDB As New DBAccess.getDBobject()
        Dim _Const As New Costanti()

        'Public Function GetListaCodiciCitta() As GetLista
        '    Try
        '        Dim GetLista As New GetLista()

        '        Dim oConn As New SqlConnection()
        '        Dim oCmd As New SqlCommand()

        '        GetLista.lngRecordCount = iDB.RunSPReturnToGrid("sp_ReturnCodiciCitta", oConn, oCmd)

        '        GetLista.oConn = oConn
        '        GetLista.oComm = oCmd


        '        Return GetLista
        '    Catch ex As Exception
        '        Throw New Exception("GetListaCodiciCitta[DBCodiciCitta]." & "Errore caricamento tabella TP_CODICI_CITTA")
        '    End Try

        'End Function

        Public Function GetAttivita(ByVal IDTIPOATTIVITA As Integer) As TabelleDiDecodifica.DetailAttivita

            Dim lngTipoOperazione As Long
            Dim _enum As _Enum
            Dim strSql As String

            Dim DetailAttivita As New TabelleDiDecodifica.DetailAttivita()
            If IDTIPOATTIVITA = _Const.INIT_VALUE_NUMBER Then lngTipoOperazione = _enum.DBOperation.DB_INSERT

            If lngTipoOperazione = _enum.DBOperation.DB_UPDATE Then
                Dim drDetail As SqlDataReader
                strSql = ""
                strSql = "SELECT * FROM TP_TIPOATTIVITA WHERE IDTIPOATTIVITA = " & MyUtility.GetParametro(IDTIPOATTIVITA)

                drDetail = iDB.GetDataReader(strSql)

                If drDetail.Read Then

                    DetailAttivita.Descrizione = MyUtility.GetParametro(drDetail("DESCRIZIONE"))
                    DetailAttivita.CodiceAttivita = MyUtility.GetParametro(drDetail("CODICEATTIVITA"))
                    DetailAttivita.Note = MyUtility.GetParametro(drDetail("NOTE"))
                End If

                drDetail.Close()

            End If

            If lngTipoOperazione = _enum.DBOperation.DB_INSERT Then

                DetailAttivita.IDAttivita = _Const.INIT_VALUE_NUMBER
                DetailAttivita.CodiceAttivita = _Const.INIT_VALUE_STRING
                DetailAttivita.Descrizione = _Const.INIT_VALUE_STRING
                DetailAttivita.Note = _Const.INIT_VALUE_STRING

            End If
            Return DetailAttivita

        End Function

        Public Sub SetATTIVITA(ByVal oDatail As TabelleDiDecodifica.DetailAttivita)

            Dim lngTipoOperazione As Long
            Dim strSql As String
            Dim rd As SqlDataReader

            lngTipoOperazione = _Enum.DBOperation.DB_UPDATE

            If oDatail.IDAttivita = _Const.INIT_VALUE_NUMBER Then
                lngTipoOperazione = _Enum.DBOperation.DB_INSERT
            End If

            If lngTipoOperazione = _Enum.DBOperation.DB_INSERT Then
                '///Verifica esistenza descrizione 

                strSql = ""
                strSql = "SELECT DESCRIZIONE FROM TP_TIPOATTIVITA WHERE DESCRIZIONE=" & MyUtility.CStrToDB(Replace(Trim(oDatail.Descrizione), "'", "''"))

                rd = iDB.GetDataReader(strSql)
                If rd.Read Then
                    Throw New Exception("La descrizione Inserita è già presente  in tabella !")
                End If
                rd.Close()

                strSql = ""
                strSql = "INSERT INTO TP_TIPOATTIVITA"
                strSql = strSql & "(DESCRIZIONE,NOTE)" & vbCrLf
                strSql = strSql & "VALUES ( " & vbCrLf
                strSql = strSql & MyUtility.CStrToDB(UCase(oDatail.Descrizione)) & "," & vbCrLf
                strSql = strSql & MyUtility.CStrToDB(UCase(oDatail.Note)) & vbCrLf
                strSql = strSql & " )"
                Try
                    If iDB.ExecuteNonQuery(strSql) <> Costanti.VALUE_NUMBER_UNO Then
                        Throw New Exception("errore in::" & strSql)
                    End If
                Catch ex As SqlException
                    Throw ex
                Catch ex As Exception
                    Throw ex
                End Try

            End If

            If lngTipoOperazione = _Enum.DBOperation.DB_UPDATE Then

                Dim sqlTrans As SqlTransaction
                Dim sqlConn As New SqlConnection
                Dim sqlCmdInsert As SqlCommand


                sqlConn.ConnectionString = ConstSession.StringConnection

                sqlConn.Open()
                sqlTrans = sqlConn.BeginTransaction

                strSql = "DELETE FROM TP_TIPOATTIVITA WHERE IDTIPOATTIVITA=" & oDatail.IDAttivita

                sqlCmdInsert = New SqlCommand(strSql, sqlConn, sqlTrans)
                Try
                    sqlCmdInsert.ExecuteNonQuery()

                Catch ex As SqlException
                    Select Case ex.Number
                        Case 547
                            sqlTrans.Rollback()
                            sqlConn.Close()
                            sqlConn.Dispose()
                            sqlCmdInsert.Dispose()
                            Throw ex
                    End Select
                End Try
                sqlTrans.Rollback()
                sqlConn.Close()
                sqlConn.Dispose()
                sqlCmdInsert.Dispose()

                strSql = ""
                strSql = "SELECT DESCRIZIONE FROM TP_TIPOATTIVITA WHERE DESCRIZIONE=" & MyUtility.CStrToDB(Replace(Trim(oDatail.Descrizione), "'", "''"))

                rd = iDB.GetDataReader(strSql)
                If rd.Read Then
                    Throw New Exception("La descrizione Inserita è già presente  in tabella !")
                End If
                rd.Close()

                strSql = "UPDATE TP_TIPOATTIVITA SET "
                strSql = strSql & "DESCRIZIONE =" & MyUtility.CStrToDB(UCase(oDatail.Descrizione)) & vbCrLf
                strSql = strSql & ",NOTE=" & MyUtility.CStrToDB(UCase(oDatail.Note)) & vbCrLf
                strSql = strSql & "WHERE" & vbCrLf
                strSql = strSql & "IDTIPOATTIVITA=" & oDatail.IDAttivita

                Try
                    If iDB.ExecuteNonQuery(strSql) <> Costanti.VALUE_NUMBER_UNO Then
                        Throw New Exception("errore in::" & strSql)
                    End If
                Catch ex As SqlException
                    Throw ex
                Catch ex As Exception
                    Throw ex
                End Try

            End If

        End Sub
        Public Sub EliminaAttivita(ByVal IDTIPOATTIVITA As Integer)
            Dim strSql As String
            Try
                strSql = "DELETE FROM TP_TIPOATTIVITA WHERE IDTIPOATTIVITA=" & IDTIPOATTIVITA
                If iDB.ExecuteNonQuery(strSql) <> Costanti.VALUE_NUMBER_UNO Then
                    Throw New Exception("errore in::" & strSql)
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Sub UpdateForzato(ByVal oDatail As TabelleDiDecodifica.DetailAttivita)

            Dim strSql As String
            Dim rd As SqlDataReader

            strSql = ""
            strSql = "SELECT DESCRIZIONE FROM TP_TIPOATTIVITA WHERE DESCRIZIONE=" & MyUtility.CStrToDB(Replace(Trim(oDatail.Descrizione), "'", "''"))

            rd = iDB.GetDataReader(strSql)
            If rd.Read Then
                Throw New Exception("La descrizione Inserita è già presente  in tabella !")
            End If
            rd.Close()

            strSql = "UPDATE TP_TIPOATTIVITA SET "
            strSql = strSql & "DESCRIZIONE =" & MyUtility.CStrToDB(UCase(oDatail.Descrizione)) & vbCrLf
            strSql = strSql & ",NOTE=" & MyUtility.CStrToDB(UCase(oDatail.Note)) & vbCrLf
            strSql = strSql & "WHERE" & vbCrLf
            strSql = strSql & "IDTIPOATTIVITA=" & oDatail.IDAttivita

            Try
                If iDB.ExecuteNonQuery(strSql) <> Costanti.VALUE_NUMBER_UNO Then
                    Throw New Exception("errore in::" & strSql)
                End If
            Catch ex As SqlException
                Throw ex
            Catch ex As Exception
                Throw ex
            End Try

        End Sub

    End Class
End Namespace
