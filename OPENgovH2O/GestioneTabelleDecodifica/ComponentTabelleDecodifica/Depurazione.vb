Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports log4net

Namespace TabelleDiDecodifica
    Public Class DetailDepurazione
        Public Descrizione As String
        Public CodiceDepurazione As String
        Public CodDepurazione As String
        Public Note As String
    End Class
    Public Class DBDepurazione
        Inherits TabelleDiDecodifica.DetailDepurazione

        Dim iDB As New DBAccess.getDBobject()
        Dim _Const As New Costanti()
        Private Shared Log As ILog = LogManager.GetLogger(GetType(DBDepurazione))

        'Public Function GetListaDepurazione() As GetLista
        '    Try
        '        Dim GetLista As New GetLista()

        '        Dim oConn As New SqlConnection()
        '        Dim oCmd As New SqlCommand()

        '        GetLista.lngRecodvMyDatiCount = iDB.RunSPReturnToGrid("sp_ReturnDepurazione", oConn, oCmd)

        '        GetLista.oConn = oConn
        '        GetLista.oComm = oCmd


        '        Return GetLista
        '    Catch ex As Exception
        ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBDepurazione.GetListaDepurazione.errore: ", ex)
        '        Throw New Exception("GetListaDepurazione[DBDepurazione]." & "Errore caricamento tabella depurazione")
        '    End Try

        'End Function
        Public Function GetListaDepurazione() As DataView
            Dim cmdMyCommand As New SqlCommand
            Dim myDv As DataView = Nothing
            Try
                cmdMyCommand.CommandType = CommandType.StoredProcedure
                cmdMyCommand.CommandText = "sp_ReturnDepurazione"

                myDv = iDB.GetDataView(cmdMyCommand)
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBDepurazione.GetListaDepurazione.errore: ", ex)
                Throw New Exception("GetListaDepurazione[DBDepurazione]." & "Errore caricamento tabella depurazione")
            End Try
            Return myDv
        End Function
        ''''''''''''''''/RITORNA IL DETTAGLIO DELLA TABELLA DEPURAZIONE''''''''''''''''''''''''''''''//
        Public Function GetDepurazione(ByVal CODDEPURAZIONE As Integer) As TabelleDiDecodifica.DetailDepurazione

            Dim lngTipoOperazione As Long
            Dim sSQL As String

            Dim DetailDepurazione As New TabelleDiDecodifica.DetailDepurazione()
            Try
                If CODDEPURAZIONE = -1 Then lngTipoOperazione = DecEnum.DBOperation.DB_INSERT

                If lngTipoOperazione = DecEnum.DBOperation.DB_UPDATE Then
                    Dim dvMyDati As New DataView
                    sSQL = "SELECT * FROM TP_DEPURAZIONE WHERE CODDEPURAZIONE = " & Utility.StringOperation.FormatString(CODDEPURAZIONE)
                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            DetailDepurazione.Descrizione = Utility.StringOperation.FormatString(myRow("DESCRIZIONE"))
                            DetailDepurazione.CodiceDepurazione = Utility.StringOperation.FormatString(myRow("CODICEDEPURAZIONE"))
                            DetailDepurazione.Note = Utility.StringOperation.FormatString(myRow("NOTE"))
                        Next
                    End If
                    dvMyDati.Dispose()
                End If

                If lngTipoOperazione = DecEnum.DBOperation.DB_INSERT Then
                    DetailDepurazione.CodDepurazione = -1
                    DetailDepurazione.Descrizione = ""
                    DetailDepurazione.CodiceDepurazione = ""
                    DetailDepurazione.Note = ""
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBDepurazione.GetDepurazione.errore: ", ex)
            End Try
            Return DetailDepurazione
        End Function
        ''''''''''''''''/RITORNA IL DETTAGLIO DELLA TABELLA DEPURAZIONE''''''''''''''''''''''''''''''//
        Public Sub SetDEPURAZIONE(ByVal myDetail As TabelleDiDecodifica.DetailDepurazione)

            Dim lngTipoOperazione As Long
            Dim sSQL As String
            Dim dvMyDati As New DataView

            lngTipoOperazione = DecEnum.DBOperation.DB_UPDATE
            Try
                If myDetail.CodDepurazione = -1 Then
                    lngTipoOperazione = DecEnum.DBOperation.DB_INSERT
                End If

                If lngTipoOperazione = DecEnum.DBOperation.DB_INSERT Then
                    ''''Verifica esistenza descrizione 
                    sSQL = "SELECT DESCRIZIONE FROM TP_DEPURAZIONE WHERE DESCRIZIONE=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.Descrizione), "'", "''"))
                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            Throw New Exception("La descrizione Inserita è già presente  in tabella !")
                        Next
                    End If
                    dvMyDati.Dispose()

                    sSQL = "SELECT CODICEDEPURAZIONE FROM TP_DEPURAZIONE WHERE CODICEDEPURAZIONE=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.CodiceDepurazione), "'", "''")) & vbCrLf
                    sSQL += " AND (CODICEDEPURAZIONE   IS NOT NULL  AND CODICEDEPURAZIONE <>'')" & vbCrLf
                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            Throw New Exception("Il Codice Depurazione inserito è già presente in Tabella !")
                        Next
                    End If
                    dvMyDati.Dispose()


                    sSQL = ""
                    sSQL = "INSERT INTO TP_DEPURAZIONE"
                    sSQL += "(CODICEDEPURAZIONE,DESCRIZIONE,NOTE)" & vbCrLf
                    sSQL += "VALUES ( " & vbCrLf
                    sSQL += Utility.StringOperation.FormatString(UCase(myDetail.CodiceDepurazione)) & "," & vbCrLf
                    sSQL += Utility.StringOperation.FormatString(UCase(myDetail.Descrizione)) & "," & vbCrLf
                    sSQL += Utility.StringOperation.FormatString(UCase(myDetail.Note)) & vbCrLf
                    sSQL += " )"
                    Try
                        If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                            Throw New Exception("errore in::" & sSQL)
                        End If
                    Catch ex As SqlException
                        Throw ex
                    Catch ex As Exception
                        Throw ex
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBDepurazione.SetDEPURAZIONE.errore: ", ex)
                    End Try

                End If

                If lngTipoOperazione = DecEnum.DBOperation.DB_UPDATE Then

                    Dim sqlTrans As SqlTransaction
                    Dim sqlConn As New SqlConnection
                    Dim sqlCmdInsert As SqlCommand


                    sqlConn.ConnectionString = ConstSession.StringConnection

                    sqlConn.Open()
                    sqlTrans = sqlConn.BeginTransaction
                    sSQL = "DELETE FROM TP_DEPURAZIONE WHERE CODDEPURAZIONE=" & myDetail.CodDepurazione

                    sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
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

                    sSQL = ""
                    sSQL = "SELECT CODICEDEPURAZIONE FROM TP_DEPURAZIONE WHERE CODICEDEPURAZIONE=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.CodiceDepurazione), "'", "''")) & vbCrLf
                    sSQL += " AND (CODICEDEPURAZIONE   IS NOT NULL  AND CODICEDEPURAZIONE <>'')" & vbCrLf
                    sSQL += " AND CODDEPURAZIONE <> " & myDetail.CodDepurazione & vbCrLf
                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            Throw New Exception("Il Codice Depurazione inserito è già presente in Tabella !")
                        Next
                    End If
                    dvMyDati.Dispose()

                    sSQL = "UPDATE TP_DEPURAZIONE SET "
                    sSQL += "CODICEDEPURAZIONE =" & Utility.StringOperation.FormatString(UCase(myDetail.CodiceDepurazione)) & vbCrLf
                    sSQL += ",DESCRIZIONE =" & Utility.StringOperation.FormatString(UCase(myDetail.Descrizione)) & vbCrLf
                    sSQL += ",NOTE=" & Utility.StringOperation.FormatString(UCase(myDetail.Note)) & vbCrLf
                    sSQL += "WHERE" & vbCrLf
                    sSQL += "CODDEPURAZIONE=" & myDetail.CodDepurazione

                    Try
                        If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                            Throw New Exception("errore in::" & sSQL)
                        End If
                    Catch ex As SqlException
                        Throw ex
                    Catch ex As Exception
                        Throw ex
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBDepurazione.SetDEPURAZIONE.errore: ", ex)
                    End Try

                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBDepurazione.SetDEPURAZIONE.errore: ", ex)
            End Try
        End Sub

        Public Sub EliminaDepurazione(ByVal CODDEPURAZIONE As Integer)
            Dim sSQL As String
            Try
                sSQL = "DELETE FROM TP_DEPURAZIONE WHERE CODDEPURAZIONE=" & CODDEPURAZIONE
                If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                    Throw New Exception("errore in::" & sSQL)
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBDepurazione.EliminaDepurazione.errore: ", ex)
                Throw ex
            End Try
        End Sub

        Public Sub UpdateForzato(ByVal myDetail As TabelleDiDecodifica.DetailDepurazione)
            Dim sSQL As String
            Dim dvMyDati As New DataView
            Try
                sSQL = ""
                sSQL = "SELECT CODICEDEPURAZIONE FROM TP_DEPURAZIONE WHERE CODICEDEPURAZIONE=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.CodiceDepurazione), "'", "''")) & vbCrLf
                sSQL += " AND (CODICEDEPURAZIONE   IS NOT NULL  AND CODICEDEPURAZIONE <>'')" & vbCrLf
                sSQL += " AND CODDEPURAZIONE <> " & myDetail.CodDepurazione & vbCrLf
                dvMyDati = iDB.GetDataView(sSQL)
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        Throw New Exception("Il Codice Depurazione inserito è già presente in Tabella !")
                    Next
                End If
                dvMyDati.Dispose()

                sSQL = "UPDATE TP_DEPURAZIONE SET "
                sSQL += "CODICEDEPURAZIONE =" & utility.stringoperation.formatstring(UCase(myDetail.CodiceDepurazione)) & vbCrLf
                sSQL += ",DESCRIZIONE =" & utility.stringoperation.formatstring(UCase(myDetail.Descrizione)) & vbCrLf
                sSQL += ",NOTE=" & utility.stringoperation.formatstring(UCase(myDetail.Note)) & vbCrLf
                sSQL += "WHERE" & vbCrLf
                sSQL += "CODDEPURAZIONE=" & myDetail.CodDepurazione

                Try
                    If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                        Throw New Exception("errore in::" & sSQL)
                    End If
                Catch ex As SqlException
                    Throw ex
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBDepurazione.UpdateForzato.errore: ", ex)
                    Throw ex
                End Try
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBDepurazione.UpdateForzato.errore: ", ex)
            End Try
        End Sub
    End Class
End Namespace
