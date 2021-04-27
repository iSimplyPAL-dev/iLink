Imports System
Imports System.Data
Imports System.Data.SqlClient

Namespace TabelleDiDecodifica
  Public Class DBTipoStrade
	Inherits TabelleDiDecodifica.TIPO_STRADA

        Dim iDB As New DBAccess.getDBobject()
        Dim _Const As New Costanti()

        'Public Function GetListaTipo_Strade() As GetLista
        '    Try
        '        Dim GetLista As New GetLista()

        '        Dim oConn As New SqlConnection()
        '        Dim oCmd As New SqlCommand()

        '        GetLista.lngRecordCount = iDB.RunSPReturnToGrid("sp_ReturnTipoStrade", oConn, oCmd)

        '        GetLista.oConn = oConn
        '        GetLista.oComm = oCmd


        '        Return GetLista
        '    Catch ex As Exception
        '        Throw New Exception("GetListaTipo_Strade[DBTipoStrade]." & "Errore caricamento Tipo Strade")
        '    End Try
        'End Function
        Public Function GetListaTipo_Strade() As DataView
            Dim cmdMyCommand As New SqlCommand
            Dim myDv As DataView = Nothing
            Try
                cmdMyCommand.CommandType = CommandType.StoredProcedure
                cmdMyCommand.CommandText = "sp_ReturnTipoStrade"

                myDv = iDB.GetDataView(cmdMyCommand)
            Catch ex As Exception
                Throw New Exception("GetListaTipo_Strade[DBTipoStrade]." & "Errore caricamento Tipo Strade")
            End Try
            Return myDv
        End Function

        Public Function GetTipoStrada(ByVal TIPO_STRADA As String) As TabelleDiDecodifica.TIPO_STRADA

            Dim lngTipoOperazione As Long
            Dim _enum As _Enum
            Dim strSql As String

            Dim DetailTipoStrade As New TabelleDiDecodifica.TIPO_STRADA()
            If TIPO_STRADA = _Const.INIT_VALUE_STRING Then lngTipoOperazione = _enum.DBOperation.DB_INSERT

            If lngTipoOperazione = _enum.DBOperation.DB_UPDATE Then
                Dim drDetail As SqlDataReader
                strSql = ""
                strSql = "SELECT * FROM TIPO_STRADE WHERE TIPO_STRADA = " & MyUtility.CStrToDB(Replace(Trim(TIPO_STRADA), "'", "''"))

                drDetail = iDB.GetDataReader(strSql)

                If drDetail.Read Then


                    DetailTipoStrade.Tipo_Strada = MyUtility.GetParametro(drDetail("TIPO_STRADA"))


                End If

                drDetail.Close()

            End If

            If lngTipoOperazione = _enum.DBOperation.DB_INSERT Then


                DetailTipoStrade.Tipo_Strada = _Const.INIT_VALUE_STRING


            End If
            Return DetailTipoStrade

        End Function
        Public Sub SetTipoStrade(ByVal oDatail As TabelleDiDecodifica.TIPO_STRADA)

            Dim lngTipoOperazione As Long
            Dim strSql As String
            Dim rd As SqlDataReader

            lngTipoOperazione = _Enum.DBOperation.DB_UPDATE

            If oDatail.CODTipo_Strada = _Const.INIT_VALUE_STRING Then
                lngTipoOperazione = _Enum.DBOperation.DB_INSERT
            End If

            If lngTipoOperazione = _Enum.DBOperation.DB_INSERT Then
                '///Verifica esistenza descrizione 

                strSql = ""
                strSql = "SELECT TIPO_STRADA FROM TIPO_STRADE WHERE TIPO_STRADA=" & MyUtility.CStrToDB(Replace(Trim(oDatail.Tipo_Strada), "'", "''")) & vbCrLf



                rd = iDB.GetDataReader(strSql)
                If rd.Read Then
                    Throw New Exception("Il Tipo strada Inserito è già presente  in tabella !")
                End If
                rd.Close()

                strSql = ""
                strSql = "INSERT INTO TIPO_STRADE"
                strSql = strSql & "(TIPO_STRADA)" & vbCrLf
                strSql = strSql & "VALUES ( " & vbCrLf
                strSql = strSql & MyUtility.CStrToDB(UCase(oDatail.Tipo_Strada)) & vbCrLf
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

                strSql = "SELECT  * FROM STRADARIO WHERE TIPO_STRADA=" & MyUtility.CStrToDB(Replace(Trim(oDatail.CODTipo_Strada), "'", "''"))

                rd = iDB.GetDataReader(strSql)
                If rd.Read Then
                    Throw New DBConcurrencyException("")
                End If
                rd.Close()


                strSql = ""
                strSql = "SELECT TIPO_STRADA FROM TIPO_STRADE WHERE TIPO_STRADA=" & MyUtility.CStrToDB(Replace(Trim(oDatail.Tipo_Strada), "'", "''"))
                strSql = strSql & "AND" & vbCrLf
                strSql = strSql & "TIPO_STRADA<>" & MyUtility.CStrToDB(Replace(Trim(oDatail.CODTipo_Strada), "'", "''"))

                rd = iDB.GetDataReader(strSql)
                If rd.Read Then
                    Throw New Exception("La descrizione Inserita è già presente  in tabella !")
                End If
                rd.Close()

                strSql = "UPDATE TIPO_STRADE SET "
                strSql = strSql & "TIPO_STRADA =" & MyUtility.CStrToDB(oDatail.Tipo_Strada) & vbCrLf
                strSql = strSql & "WHERE TIPO_STRADA=" & MyUtility.CStrToDB(Replace(Trim(oDatail.CODTipo_Strada), "'", "''"))

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


        Public Sub EliminaTipoStrada(ByVal TIPO_STRADA As String)

            Dim strSql As String
            Dim rd As SqlDataReader
            strSql = "SELECT  * FROM STRADARIO WHERE TIPO_STRADA=" & MyUtility.CStrToDB(Replace(Trim(TIPO_STRADA), "'", "''"))

            rd = iDB.GetDataReader(strSql)
            If rd.Read Then
                Throw New Exception("Il Tipo strada è usato nello stradario impossibile eliminare!")
            End If
            rd.Close()
            Try
                strSql = "DELETE FROM TIPO_STRADE WHERE TIPO_STRADA=" & MyUtility.CStrToDB(Replace(Trim(TIPO_STRADA), "'", "''"))
                If iDB.ExecuteNonQuery(strSql) <> Costanti.VALUE_NUMBER_UNO Then
                    Throw New Exception("errore in::" & strSql)
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub


        Public Sub UpdateForzato(ByVal oDatail As TabelleDiDecodifica.TIPO_STRADA)
            Dim strSql As String
            Dim rd As SqlDataReader

            strSql = ""
            strSql = "SELECT TIPO_STRADA FROM TIPO_STRADE WHERE TIPO_STRADA=" & MyUtility.CStrToDB(Replace(Trim(oDatail.Tipo_Strada), "'", "''"))
            strSql = strSql & "AND" & vbCrLf
            strSql = strSql & "TIPO_STRADA<>" & MyUtility.CStrToDB(Replace(Trim(oDatail.CODTipo_Strada), "'", "''"))

            rd = iDB.GetDataReader(strSql)
            If rd.Read Then
                Throw New Exception("La descrizione Inserita è già presente  in tabella !")
            End If
            rd.Close()

            strSql = "UPDATE TIPO_STRADE SET "
            strSql = strSql & "TIPO_STRADA =" & MyUtility.CStrToDB(oDatail.Tipo_Strada) & vbCrLf
            strSql = strSql & "WHERE TIPO_STRADA=" & MyUtility.CStrToDB(Replace(Trim(oDatail.CODTipo_Strada), "'", "''"))


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
