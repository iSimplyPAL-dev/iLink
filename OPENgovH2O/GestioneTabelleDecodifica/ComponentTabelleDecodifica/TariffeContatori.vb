Imports System
Imports System.Data
Imports System.Data.SqlClient
Namespace TabelleDiDecodifica
  Public Class DBTariffeContatori
	Inherits TabelleDiDecodifica.TariffeContatore

        Dim iDB As New DBAccess.getDBobject()
        Dim _Const As New Costanti()


        'Public Function GetListaTariffeContatore() As GetLista
        '    Try
        '        Dim GetLista As New GetLista()

        '        Dim oConn As New SqlConnection()
        '        Dim oCmd As New SqlCommand()

        '        GetLista.lngRecordCount = iDB.RunSPReturnToGrid("sp_ReturnTitoloSoggetti", oConn, oCmd)

        '        GetLista.oConn = oConn
        '        GetLista.oComm = oCmd


        '        Return GetLista
        '    Catch ex As Exception
        '        Throw New Exception("GetListaTariffeContatore[DBTariffeContatori]." & "Errore caricamento Tariffe Contatore")
        '    End Try
        'End Function

        Public Function GetTariffeContatore(ByVal CODTARIFFACONTATORE As Integer) As TabelleDiDecodifica.TariffeContatore

            Dim lngTipoOperazione As Long
            Dim _enum As _Enum
            Dim strSql As String

            Dim DetailTariffeContatore As New TabelleDiDecodifica.TariffeContatore()
            If CODTARIFFACONTATORE = _Const.INIT_VALUE_NUMBER Then lngTipoOperazione = _enum.DBOperation.DB_INSERT

            If lngTipoOperazione = _enum.DBOperation.DB_UPDATE Then
                Dim drDetail As SqlDataReader
                strSql = ""
                strSql = "SELECT * FROM TP_TARIFFECONTATORI WHERE CODTARIFFACONTATORE = " & MyUtility.GetParametro(CODTARIFFACONTATORE)

                drDetail = iDB.GetDataReader(strSql)

                If drDetail.Read Then
                    DetailTariffeContatore.Descrizione = MyUtility.GetParametro(drDetail("DESCRIZIONE"))
                    DetailTariffeContatore.TariffaContatore = MyUtility.GetParametro(drDetail("IMPORTO"))
                    DetailTariffeContatore.Note = MyUtility.GetParametro(drDetail("NOTE"))
                End If

                drDetail.Close()

            End If

            If lngTipoOperazione = _enum.DBOperation.DB_INSERT Then

                DetailTariffeContatore.CODTARIFFACONTATORE = _Const.INIT_VALUE_NUMBER
                DetailTariffeContatore.TariffaContatore = _Const.INIT_VALUE_NUMBER
                DetailTariffeContatore.Descrizione = _Const.INIT_VALUE_STRING
                DetailTariffeContatore.Note = _Const.INIT_VALUE_STRING

            End If
            Return DetailTariffeContatore

        End Function


        Public Sub SetTariffeContatore(ByVal oDatail As TabelleDiDecodifica.TariffeContatore)

            Dim lngTipoOperazione As Long
            Dim strSql As String


            lngTipoOperazione = _Enum.DBOperation.DB_UPDATE

            If oDatail.CODTARIFFACONTATORE = _Const.INIT_VALUE_NUMBER Then
                lngTipoOperazione = _Enum.DBOperation.DB_INSERT
            End If

            If lngTipoOperazione = _Enum.DBOperation.DB_INSERT Then
                '///Verifica esistenza descrizione 

                strSql = ""
                strSql = "SELECT DESCRIZIONE FROM TP_TARIFFECONTATORI WHERE DESCRIZIONE=" & MyUtility.CStrToDB(Replace(Trim(oDatail.Descrizione), "'", "''"))
                Dim rd As SqlDataReader
                rd = iDB.GetDataReader(strSql)
                If rd.Read Then
                    Throw New Exception("La descrizione Inserita è già presente  in tabella !")
                End If
                rd.Close()

                strSql = ""
                strSql = "INSERT INTO TP_TARIFFECONTATORI"
                strSql = strSql & "(IMPORTO,DESCRIZIONE,NOTE)" & vbCrLf
                strSql = strSql & "VALUES ( " & vbCrLf
                strSql = strSql & MyUtility.CToStr(UCase(oDatail.TariffaContatore)) & "," & vbCrLf
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
                strSql = "DELETE FROM TP_TARIFFECONTATORI WHERE CODTARIFFACONTATORE=" & oDatail.CODTARIFFACONTATORE

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


                strSql = "UPDATE TP_TARIFFECONTATORI SET "
                strSql = strSql & "IMPORTO =" & MyUtility.CStrToDB(oDatail.TariffaContatore) & vbCrLf
                strSql = strSql & ",DESCRIZIONE =" & MyUtility.CStrToDB(UCase(oDatail.Descrizione)) & vbCrLf
                strSql = strSql & ",NOTE=" & MyUtility.CStrToDB(UCase(oDatail.Note)) & vbCrLf
                strSql = strSql & "WHERE" & vbCrLf
                strSql = strSql & "CODTARIFFACONTATORE=" & oDatail.CODTARIFFACONTATORE

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
        Public Sub EliminaTipoContatore(ByVal CODTARIFFACONTATORE As Integer)
            Dim strSql As String
            Try
                strSql = "DELETE FROM TP_TARIFFECONTATORI WHERE CODTARIFFACONTATORE=" & CODTARIFFACONTATORE
                If iDB.ExecuteNonQuery(strSql) <> Costanti.VALUE_NUMBER_UNO Then
                    Throw New Exception("errore in::" & strSql)
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Sub UpdateForzato(ByVal oDatail As TabelleDiDecodifica.TariffeContatore)
            Dim strSql As String


            strSql = "UPDATE TP_TARIFFECONTATORI SET "
            strSql = strSql & "IMPORTO =" & MyUtility.CStrToDB(oDatail.TariffaContatore) & vbCrLf
            strSql = strSql & ",DESCRIZIONE =" & MyUtility.CStrToDB(UCase(oDatail.Descrizione)) & vbCrLf
            strSql = strSql & ",NOTE=" & MyUtility.CStrToDB(UCase(oDatail.Note)) & vbCrLf
            strSql = strSql & "WHERE" & vbCrLf
            strSql = strSql & "CODTARIFFACONTATORE=" & oDatail.CODTARIFFACONTATORE

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