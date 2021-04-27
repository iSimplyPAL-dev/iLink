Imports System
Imports System.Data
Imports System.Data.SqlClient

Namespace TabelleDiDecodifica
    Public Class DBTitoloSoggetti
        Inherits TabelleDiDecodifica.DetailTitoloSoggetti

        Dim iDB As New DBAccess.getDBobject()
        Dim _Const As New Costanti()

        '/////////////RITORNA L'ELENCO DEI TITOLI SOGGETTI PRESNTI NEL DATBASE APPLICATIVO//////  
        'Public Function GetListaTitoloSoggetti() As GetLista
        '    Try
        '        Dim GetLista As New GetLista()

        '        Dim oConn As New SqlConnection()
        '        Dim oCmd As New SqlCommand()

        '        GetLista.lngRecordCount = iDB.RunSPReturnToGrid("sp_ReturnTitoloSoggetti", oConn, oCmd)

        '        GetLista.oConn = oConn
        '        GetLista.oComm = oCmd


        '        Return GetLista
        '    Catch ex As Exception
        '        Throw New Exception("GetListaTitoloSoggetti[DBTitoloSoggetti]." & "Errore caricamento Titolo Soggetti")
        '    End Try

        'End Function
        Public Function GetListaTitoloSoggetti() As DataView
            Dim cmdMyCommand As New SqlCommand
            Dim myDv As DataView = Nothing
            Try
                cmdMyCommand.CommandType = CommandType.StoredProcedure
                cmdMyCommand.CommandText = "sp_ReturnTitoloSoggetti"

                myDv = iDB.GetDataView(cmdMyCommand)
            Catch ex As Exception
                Throw New Exception("GetListaTitoloSoggetti[DBTitoloSoggetti]." & "Errore caricamento Titolo Soggetti")
            End Try
            Return myDv
        End Function

        Public Function GetTitoloSoggetti(ByVal IDTITOLOSOGGETTO As Integer) As TabelleDiDecodifica.DetailTitoloSoggetti

            Dim lngTipoOperazione As Long
            Dim _enum As _Enum
            Dim strSql As String

            Dim DetailTitoloSoggetti As New TabelleDiDecodifica.DetailTitoloSoggetti()
            If IDTITOLOSOGGETTO = _Const.INIT_VALUE_NUMBER Then lngTipoOperazione = _enum.DBOperation.DB_INSERT

            If lngTipoOperazione = _enum.DBOperation.DB_UPDATE Then
                Dim drDetail As SqlDataReader
                strSql = ""
                strSql = "SELECT * FROM TP_TITOLOSOGGETTI WHERE IDTITOLOSOGGETTO = " & MyUtility.GetParametro(IDTITOLOSOGGETTO)

                drDetail = iDB.GetDataReader(strSql)

                If drDetail.Read Then
                    DetailTitoloSoggetti.Descrizione = MyUtility.GetParametro(drDetail("DESCRIZIONE"))
                    DetailTitoloSoggetti.Note = MyUtility.GetParametro(drDetail("NOTE"))
                End If

                drDetail.Close()

            End If

            If lngTipoOperazione = _enum.DBOperation.DB_INSERT Then

                DetailTitoloSoggetti.IDTitoloSoggetto = _Const.INIT_VALUE_NUMBER
                DetailTitoloSoggetti.Descrizione = _Const.INIT_VALUE_STRING
                DetailTitoloSoggetti.Note = _Const.INIT_VALUE_STRING

            End If
            Return DetailTitoloSoggetti

        End Function


        Public Sub SetTitoloSoggetto(ByVal oDatail As TabelleDiDecodifica.DetailTitoloSoggetti)

            Dim lngTipoOperazione As Long
            Dim strSql As String
            Dim rd As SqlDataReader

            lngTipoOperazione = _Enum.DBOperation.DB_UPDATE

            If oDatail.IDTitoloSoggetto = _Const.INIT_VALUE_NUMBER Then
                lngTipoOperazione = _Enum.DBOperation.DB_INSERT
            End If

            If lngTipoOperazione = _Enum.DBOperation.DB_INSERT Then
                '///Verifica esistenza descrizione 

                strSql = ""
                strSql = "SELECT DESCRIZIONE FROM TP_TITOLOSOGGETTI WHERE DESCRIZIONE=" & MyUtility.CStrToDB(Replace(Trim(oDatail.Descrizione), "'", "''"))

                rd = iDB.GetDataReader(strSql)
                If rd.Read Then
                    Throw New Exception("La descrizione Inserita è già presente  in tabella !")
                End If
                rd.Close()

                strSql = ""
                strSql = "INSERT INTO TP_TITOLOSOGGETTI"
                strSql = strSql & "(Descrizione,Note)" & vbCrLf
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
                strSql = "DELETE FROM TP_TITOLOSOGGETTI WHERE IDTITOLOSOGGETTO=" & oDatail.IDTitoloSoggetto

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
                strSql = "SELECT DESCRIZIONE FROM TP_TITOLOSOGGETTI WHERE DESCRIZIONE=" & MyUtility.CStrToDB(Replace(Trim(oDatail.Descrizione), "'", "''")) & vbCrLf
                strSql = strSql & "AND" & vbCrLf
                strSql = strSql & "IDTITOLOSOGGETTO <>" & oDatail.IDTitoloSoggetto

                rd = iDB.GetDataReader(strSql)
                If rd.Read Then
                    Throw New Exception("La descrizione Inserita è già presente  in tabella !")
                End If
                rd.Close()

                strSql = "UPDATE TP_TITOLOSOGGETTI SET "
                strSql = strSql & "DESCRIZIONE =" & MyUtility.CStrToDB(UCase(oDatail.Descrizione)) & vbCrLf
                strSql = strSql & ",NOTE=" & MyUtility.CStrToDB(UCase(oDatail.Note)) & vbCrLf
                strSql = strSql & "WHERE" & vbCrLf
                strSql = strSql & "IDTITOLOSOGGETTO=" & oDatail.IDTitoloSoggetto

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

        Public Sub EliminaTitoloSoggetto(ByVal IDTITOLOSOGGETTO As Integer)
            Dim strSql As String
            Try
                strSql = "DELETE FROM TP_TITOLOSOGGETTI WHERE IDTITOLOSOGGETTO=" & IDTITOLOSOGGETTO
                If iDB.ExecuteNonQuery(strSql) <> Costanti.VALUE_NUMBER_UNO Then
                    Throw New Exception("errore in inserimento::" & strSql)
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Sub UpdateForzato(ByVal oDatail As TabelleDiDecodifica.DetailTitoloSoggetti)
            Dim strSql As String
            Dim rd As SqlDataReader

            strSql = ""
            strSql = "SELECT DESCRIZIONE FROM TP_TITOLOSOGGETTI WHERE DESCRIZIONE=" & MyUtility.CStrToDB(Replace(Trim(oDatail.Descrizione), "'", "''")) & vbCrLf
            strSql = strSql & "AND" & vbCrLf
            strSql = strSql & "IDTITOLOSOGGETTO <>" & oDatail.IDTitoloSoggetto

            rd = iDB.GetDataReader(strSql)
            If rd.Read Then
                Throw New Exception("La descrizione Inserita è già presente  in tabella !")
            End If
            rd.Close()

            strSql = "UPDATE TP_TITOLOSOGGETTI SET "
            strSql = strSql & "DESCRIZIONE =" & MyUtility.CStrToDB(UCase(oDatail.Descrizione)) & vbCrLf
            strSql = strSql & ",NOTE=" & MyUtility.CStrToDB(UCase(oDatail.Note)) & vbCrLf
            strSql = strSql & "WHERE" & vbCrLf
            strSql = strSql & "IDTITOLOSOGGETTO=" & oDatail.IDTitoloSoggetto

            Try
                If iDB.ExecuteNonQuery(strSql) <> Costanti.VALUE_NUMBER_UNO Then
                    Throw New Exception("errore in inserimento::" & strSql)
                End If
            Catch ex As SqlException
                Throw ex
            Catch ex As Exception
                Throw ex
            End Try

        End Sub
    End Class

End Namespace