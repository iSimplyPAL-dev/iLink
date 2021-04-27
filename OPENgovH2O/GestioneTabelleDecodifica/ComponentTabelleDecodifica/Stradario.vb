Imports System
Imports System.Data
Imports System.Data.SqlClient

Namespace TabelleDiDecodifica
    Public Class DBStradario
        Inherits TabelleDiDecodifica.Stradario

        Dim iDB As New DBAccess.getDBobject()
        Dim _Const As New Costanti()

        'Public Function GetListaStrade(ByVal CodiceStradaEst As String, _
        '  ByVal TIPO_STRADA As String, _
        'ByVal Strada As String, ByVal CodiceIstat As String) As GetLista

        '    Dim GetList As New GetLista()
        '    Dim oConn As New SqlConnection()
        '    Dim oCmd As New SqlCommand()

        '    GetList.lngRecordCount = iDB.RunSPReturnToGrid("sp_RicercaStrade", oConn, oCmd, _
        '   New SqlParameter("@CodiceStradaEst", CodiceStradaEst), _
        '   New SqlParameter("@TIPO_STRADA", TIPO_STRADA), _
        '   New SqlParameter("@Strada", Strada), _
        '   New SqlParameter("@CODICEISTAT", CodiceIstat))


        '    GetList.oConn = oConn
        '    GetList.oComm = oCmd

        '    Return GetList

        'End Function
        Public Function GetListaStrade(ByVal CodiceStradaEst As String, ByVal TIPO_STRADA As String, ByVal Strada As String, ByVal CodiceIstat As String) As DataView
            Dim cmdMyCommand As New SqlCommand
            Dim myDv As DataView = Nothing
            Try
                cmdMyCommand.CommandType = CommandType.StoredProcedure
                cmdMyCommand.CommandText = "sp_RicercaStrade"
                cmdMyCommand.Parameters.Add(New SqlParameter("@CodiceStradaEst", CodiceStradaEst))
                cmdMyCommand.Parameters.Add(New SqlParameter("@TIPO_STRADA", TIPO_STRADA))
                cmdMyCommand.Parameters.Add(New SqlParameter("@Strada", Strada))
                cmdMyCommand.Parameters.Add(New SqlParameter("@CODICEISTAT", CodiceIstat))

                myDv = iDB.GetDataView(cmdMyCommand)
            Catch ex As Exception
                Throw New Exception("GetListaStrade::", ex)
            End Try
            Return myDv
        End Function

        Public Function oTipoStrada() As TabelleDiDecodifica.Stradario
            Dim DetailStradario As New TabelleDiDecodifica.Stradario()

            DetailStradario.dsTipoStrada = iDB.RunSPReturnDataSet("sp_ReturnTipoStrada", "TIPO_STRADE")
            Return DetailStradario

        End Function

        Public Function GetStradario(ByVal COD_STRADA As Integer) As TabelleDiDecodifica.Stradario

            Dim lngTipoOperazione As Long
            Dim _enum As _Enum
            Dim strSql As String

            Dim DetailStradario As New TabelleDiDecodifica.Stradario()
            If COD_STRADA = _Const.INIT_VALUE_NUMBER Then lngTipoOperazione = _enum.DBOperation.DB_INSERT

            DetailStradario.dsTipoStrada = iDB.RunSPReturnDataSet("sp_ReturnTipoStrada", "TIPO_STRADE")

            If lngTipoOperazione = _enum.DBOperation.DB_UPDATE Then
                Dim drDetail As SqlDataReader
                strSql = ""
                strSql = "SELECT * FROM STRADARIO WHERE COD_STRADA = " & MyUtility.GetParametro(COD_STRADA)


                drDetail = iDB.GetDataReader(strSql)

                If drDetail.Read Then
                    DetailStradario.CodiceStrada = MyUtility.GetParametro(drDetail("CODICE_STRADA_EST"))
                    DetailStradario.Strada = MyUtility.GetParametro(drDetail("STRADA"))
                    DetailStradario.TipoStrada = MyUtility.GetParametro(drDetail("TIPO_STRADA"))
                    DetailStradario.CodiceCitta = MyUtility.GetParametro(drDetail("COD_CITTA"))
                End If

                drDetail.Close()

            End If

            If lngTipoOperazione = _enum.DBOperation.DB_INSERT Then

                DetailStradario.CODstrada = _Const.INIT_VALUE_NUMBER
                DetailStradario.CodiceStrada = _Const.INIT_VALUE_STRING
                DetailStradario.Strada = _Const.INIT_VALUE_STRING
                DetailStradario.TipoStrada = _Const.INIT_VALUE_STRING
                DetailStradario.CodiceCitta = _Const.INIT_VALUE_STRING

            End If

            Return DetailStradario

        End Function

        Public Sub SetStradario(ByVal oDatail As TabelleDiDecodifica.Stradario, ByVal CODICECITTA As String)

            Dim lngTipoOperazione As Long
            Dim strSql As String
            Dim rd As SqlDataReader

            lngTipoOperazione = _Enum.DBOperation.DB_UPDATE

            If oDatail.CODstrada = _Const.INIT_VALUE_NUMBER Then
                lngTipoOperazione = _Enum.DBOperation.DB_INSERT
            End If

            If lngTipoOperazione = _Enum.DBOperation.DB_INSERT Then

                '///Verifica esistenza descrizione 
                strSql = ""
                strSql = "SELECT CODICE_STRADA_EST FROM STRADARIO WHERE CODICE_STRADA_EST=" & MyUtility.CStrToDB(Replace(Trim(oDatail.CodiceStrada), "'", "''")) & vbCrLf
                strSql = strSql & "AND" & vbCrLf
                strSql = strSql & "(CODICE_STRADA_EST   IS NOT NULL  AND CODICE_STRADA_EST <>'')" & vbCrLf
                strSql = strSql & "AND" & vbCrLf
                strSql = strSql & "CODICE_ISTAT=" & MyUtility.CStrToDB(oDatail.CodiceISTAT) & vbCrLf

                If CODICECITTA = oDatail.CodiceCitta Then
                    strSql = strSql & "AND" & vbCrLf
                    strSql = strSql & "COD_CITTA=" & CODICECITTA & vbCrLf
                Else
                    strSql = strSql & "AND" & vbCrLf
                    strSql = strSql & "COD_CITTA=" & oDatail.CodiceCitta & vbCrLf
                End If



                rd = iDB.GetDataReader(strSql)
                If rd.Read Then
                    Throw New Exception("Il Codice	Strada  inserito è già presente in Tabella !")
                End If
                rd.Close()

                strSql = ""
                strSql = "SELECT TIPO_STRADA + STRADA FROM STRADARIO WHERE TIPO_STRADA + STRADA=" & MyUtility.CStrToDB(Replace(Trim(oDatail.TipoStrada), "'", "''") + Replace(Trim(oDatail.Strada), "'", "''")) & vbCrLf
                strSql = strSql & "AND" & vbCrLf
                strSql = strSql & "CODICE_ISTAT=" & MyUtility.CStrToDB(oDatail.CodiceISTAT) & vbCrLf

                If CODICECITTA = oDatail.CodiceCitta Then
                    strSql = strSql & "AND" & vbCrLf
                    strSql = strSql & "COD_CITTA=" & CODICECITTA & vbCrLf
                Else
                    strSql = strSql & "AND" & vbCrLf
                    strSql = strSql & "COD_CITTA=" & oDatail.CodiceCitta & vbCrLf
                End If


                rd = iDB.GetDataReader(strSql)
                If rd.Read Then
                    Throw New Exception("La strada  inserita è già presente in Tabella !")
                End If
                rd.Close()

                strSql = ""
                strSql = "INSERT INTO STRADARIO"
                strSql = strSql & "(TIPO_STRADA,STRADA,COD_COMUNE,CODICE_STRADA_EST,CODICE_ISTAT,COD_CITTA)" & vbCrLf
                strSql = strSql & "VALUES ( " & vbCrLf
                strSql = strSql & MyUtility.CStrToDB(UCase(oDatail.TipoStrada)) & "," & vbCrLf
                strSql = strSql & MyUtility.CStrToDB(UCase(oDatail.Strada)) & "," & vbCrLf
                strSql = strSql & MyUtility.CStrToDB(UCase(oDatail.CodComune)) & "," & vbCrLf
                strSql = strSql & MyUtility.CStrToDB(UCase(oDatail.CodiceStrada)) & "," & vbCrLf
                strSql = strSql & MyUtility.CStrToDB(UCase(oDatail.CodiceISTAT)) & "," & vbCrLf
                strSql = strSql & MyUtility.CStrToDB(UCase(oDatail.CodiceCitta)) & vbCrLf
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
                strSql = "DELETE FROM STRADARIO WHERE COD_STRADA=" & oDatail.CODstrada

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
                strSql = "SELECT CODICE_STRADA_EST FROM STRADARIO WHERE CODICE_STRADA_EST=" & MyUtility.CStrToDB(Replace(Trim(oDatail.CodiceStrada), "'", "''")) & vbCrLf
                strSql = strSql & "AND" & vbCrLf
                strSql = strSql & "(CODICE_STRADA_EST   IS NOT NULL  AND CODICE_STRADA_EST <>'')" & vbCrLf
                strSql = strSql & "AND" & vbCrLf
                strSql = strSql & "COD_STRADA <> " & oDatail.CODstrada & vbCrLf
                strSql = strSql & "AND" & vbCrLf
                strSql = strSql & "CODICE_ISTAT=" & MyUtility.CStrToDB(oDatail.CodiceISTAT) & vbCrLf

                If CODICECITTA = oDatail.CodiceCitta Then
                    strSql = strSql & "AND" & vbCrLf
                    strSql = strSql & "COD_CITTA=" & CODICECITTA & vbCrLf
                Else
                    strSql = strSql & "AND" & vbCrLf
                    strSql = strSql & "COD_CITTA=" & oDatail.CodiceCitta & vbCrLf

                End If

                rd = iDB.GetDataReader(strSql)
                If rd.Read Then
                    Throw New Exception("Il Codice Strada inserito è già presente in Tabella !")
                End If
                rd.Close()

                strSql = ""
                strSql = "SELECT TIPO_STRADA + STRADA FROM STRADARIO WHERE TIPO_STRADA + STRADA=" & MyUtility.CStrToDB(Replace(Trim(oDatail.TipoStrada), "'", "''") + Replace(Trim(oDatail.Strada), "'", "''")) & vbCrLf
                strSql = strSql & "AND" & vbCrLf
                strSql = strSql & "COD_STRADA <> " & oDatail.CODstrada & vbCrLf
                strSql = strSql & "AND" & vbCrLf
                strSql = strSql & "CODICE_ISTAT=" & MyUtility.CStrToDB(oDatail.CodiceISTAT) & vbCrLf

                If CODICECITTA = oDatail.CodiceCitta Then
                    strSql = strSql & "AND" & vbCrLf
                    strSql = strSql & "COD_CITTA=" & CODICECITTA & vbCrLf
                Else
                    strSql = strSql & "AND" & vbCrLf
                    strSql = strSql & "COD_CITTA=" & oDatail.CodiceCitta & vbCrLf

                End If

                rd = iDB.GetDataReader(strSql)
                If rd.Read Then
                    Throw New Exception("La strada  inserita è già presente in Tabella !")
                End If
                rd.Close()

                strSql = "UPDATE STRADARIO SET "
                strSql = strSql & "TIPO_STRADA =" & MyUtility.CStrToDB(UCase(oDatail.TipoStrada)) & vbCrLf
                strSql = strSql & ",STRADA =" & MyUtility.CStrToDB(UCase(oDatail.Strada)) & vbCrLf
                strSql = strSql & ",CODICE_STRADA_EST=" & MyUtility.CStrToDB(UCase(oDatail.CodiceStrada)) & vbCrLf
                strSql = strSql & ",COD_CITTA=" & MyUtility.CStrToDB(UCase(oDatail.CodiceCitta)) & vbCrLf
                strSql = strSql & "WHERE" & vbCrLf
                strSql = strSql & "COD_STRADA=" & oDatail.CODstrada

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
        Public Sub EliminaStrada(ByVal COD_STRADA As Integer)
            Dim strSql As String
            Try
                strSql = "DELETE FROM STRADARIO WHERE COD_STRADA=" & COD_STRADA
                If iDB.ExecuteNonQuery(strSql) <> Costanti.VALUE_NUMBER_UNO Then
                    Throw New Exception("errore in::" & strSql)
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Sub UpdateForzato(ByVal oDatail As TabelleDiDecodifica.Stradario, ByVal CodiceCitta As String)
            Dim strSql As String
            Dim rd As SqlDataReader

            strSql = ""
            strSql = "SELECT CODICE_STRADA_EST FROM STRADARIO WHERE CODICE_STRADA_EST=" & MyUtility.CStrToDB(Replace(Trim(oDatail.CodiceStrada), "'", "''")) & vbCrLf
            strSql = strSql & "AND" & vbCrLf
            strSql = strSql & "(CODICE_STRADA_EST   IS NOT NULL  AND CODICE_STRADA_EST <>'')" & vbCrLf
            strSql = strSql & "AND" & vbCrLf
            strSql = strSql & "COD_STRADA <> " & oDatail.CODstrada & vbCrLf
            strSql = strSql & "AND" & vbCrLf
            strSql = strSql & "CODICE_ISTAT=" & MyUtility.CStrToDB(oDatail.CodiceISTAT) & vbCrLf

            If CodiceCitta = oDatail.CodiceCitta Then
                strSql = strSql & "AND" & vbCrLf
                strSql = strSql & "COD_CITTA=" & CodiceCitta & vbCrLf
            Else
                strSql = strSql & "AND" & vbCrLf
                strSql = strSql & "COD_CITTA=" & oDatail.CodiceCitta & vbCrLf

            End If

            rd = iDB.GetDataReader(strSql)
            If rd.Read Then
                Throw New Exception("Il Codice Strada inserito è già presente in Tabella !")
            End If
            rd.Close()




            strSql = ""
            strSql = "SELECT TIPO_STRADA + STRADA FROM STRADARIO WHERE TIPO_STRADA + STRADA=" & MyUtility.CStrToDB(Replace(Trim(oDatail.TipoStrada), "'", "''") + Replace(Trim(oDatail.Strada), "'", "''")) & vbCrLf
            strSql = strSql & "AND" & vbCrLf
            strSql = strSql & "COD_STRADA <> " & oDatail.CODstrada & vbCrLf
            strSql = strSql & "AND" & vbCrLf
            strSql = strSql & "CODICE_ISTAT=" & MyUtility.CStrToDB(oDatail.CodiceISTAT) & vbCrLf

            If CodiceCitta = oDatail.CodiceCitta Then
                strSql = strSql & "AND" & vbCrLf
                strSql = strSql & "COD_CITTA=" & CodiceCitta & vbCrLf
            Else
                strSql = strSql & "AND" & vbCrLf
                strSql = strSql & "COD_CITTA=" & oDatail.CodiceCitta & vbCrLf
            End If

            rd = iDB.GetDataReader(strSql)
            If rd.Read Then
                Throw New Exception("La strada  inserita è già presente in Tabella !")
            End If
            rd.Close()

            strSql = "UPDATE STRADARIO SET "
            strSql = strSql & "TIPO_STRADA =" & MyUtility.CStrToDB(UCase(oDatail.TipoStrada)) & vbCrLf
            strSql = strSql & ",STRADA =" & MyUtility.CStrToDB(UCase(oDatail.Strada)) & vbCrLf
            strSql = strSql & ",CODICE_STRADA_EST=" & MyUtility.CStrToDB(UCase(oDatail.CodiceStrada)) & vbCrLf
            strSql = strSql & ",COD_CITTA=" & MyUtility.CStrToDB(UCase(oDatail.CodiceCitta)) & vbCrLf
            strSql = strSql & "WHERE" & vbCrLf
            strSql = strSql & "COD_STRADA=" & oDatail.CODstrada

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