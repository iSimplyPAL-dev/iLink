Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports log4net

Namespace TabelleDiDecodifica
    Public Class DBAttivita
        Inherits TabelleDiDecodifica.DetailAttivita
        ''' <summary>
        ''' Gestione delle tipologie di attività da associare ad un contatore
        ''' </summary>
        ''' <remarks></remarks>
        ''' 

        Private Shared Log As ILog = LogManager.GetLogger(GetType(DBAttivita))
        Dim iDB As New DBAccess.getDBobject()
        Dim _Const As New Costanti()


        ''' <summary>
        ''' Estrae l'elenco degli oggetti configurati
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetListaAttivita() As DataView
            Dim cmdMyCommand As New SqlCommand
            Dim myDv As DataView = Nothing
            Try
                cmdMyCommand.CommandType = CommandType.StoredProcedure
                cmdMyCommand.CommandText = "sp_ReturnAttivita"

                myDv = iDB.GetDataView(cmdMyCommand)
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBAttivita.GetListaAttivita.errore: ", ex)
                Throw New Exception("GetListaAttivita[DBAttivita]." & "Errore caricamento tabella TP_TIPOATTIVITA")
            End Try
            Return myDv
        End Function
        ''' <summary>
        ''' Estrae l'oggetto in base al codice passato
        ''' </summary>
        ''' <param name="IDTIPOATTIVITA">intero</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetAttivita(ByVal IDTIPOATTIVITA As Integer) As TabelleDiDecodifica.DetailAttivita
            Dim lngTipoOperazione As Long
            Dim sSQL As String

            Dim DetailAttivita As New TabelleDiDecodifica.DetailAttivita()
            Try
                If IDTIPOATTIVITA = -1 Then lngTipoOperazione = DecEnum.DBOperation.DB_INSERT

                If lngTipoOperazione = DecEnum.DBOperation.DB_UPDATE Then
                    Dim dvMyDati As New DataView
                    sSQL = "SELECT * FROM TP_TIPOATTIVITA WHERE IDTIPOATTIVITA = " & Utility.StringOperation.FormatString(IDTIPOATTIVITA)
                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            DetailAttivita.Descrizione = Utility.StringOperation.FormatString(myRow("DESCRIZIONE"))
                            DetailAttivita.CodiceAttivita = Utility.StringOperation.FormatString(myRow("CODICEATTIVITA"))
                            DetailAttivita.Note = Utility.StringOperation.FormatString(myRow("NOTE"))
                        Next
                    End If
                    dvMyDati.Dispose()
                End If

                If lngTipoOperazione = DecEnum.DBOperation.DB_INSERT Then
                    DetailAttivita.IDAttivita = -1
                    DetailAttivita.CodiceAttivita = ""
                    DetailAttivita.Descrizione = ""
                    DetailAttivita.Note = ""
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBAttivita.GetAttivita.errore: ", ex)
            End Try
            Return DetailAttivita
        End Function
        ''' <summary>
        ''' Inserisce un oggetto
        ''' </summary>
        ''' <param name="myDetail">oggetto di tipo TabelleDiDecodifica.DetailAttivita</param>
        ''' <remarks></remarks>
        Public Sub SetATTIVITA(ByVal myDetail As TabelleDiDecodifica.DetailAttivita)

            Dim lngTipoOperazione As Long
            Dim sSQL As String
            Dim dvMyDati As New DataView

            lngTipoOperazione = DecEnum.DBOperation.DB_UPDATE
            Try
                If myDetail.IDAttivita = -1 Then
                    lngTipoOperazione = DecEnum.DBOperation.DB_INSERT
                End If

                If lngTipoOperazione = DecEnum.DBOperation.DB_INSERT Then
                    ''''Verifica esistenza descrizione 
                    sSQL = "SELECT DESCRIZIONE FROM TP_TIPOATTIVITA WHERE DESCRIZIONE=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.Descrizione), "'", "''"))
                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            Throw New Exception("La descrizione Inserita è già presente  in tabella !")
                        Next
                    End If
                    dvMyDati.Dispose()

                    Try
                        sSQL = "INSERT INTO TP_TIPOATTIVITA(DESCRIZIONE,NOTE)"
                        sSQL += " VALUES ( "
                        sSQL += Utility.StringOperation.FormatString(UCase(myDetail.Descrizione)) & ","
                        sSQL += Utility.StringOperation.FormatString(UCase(myDetail.Note))
                        sSQL += " )"
                    If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                        Throw New Exception("errore in::" & sSQL)
                    End If
                    Catch ex As SqlException
                        Throw ex
                    Catch ex As Exception
                        Throw ex
                    End Try

                End If

                If lngTipoOperazione = DecEnum.DBOperation.DB_UPDATE Then

                    Dim sqlTrans As SqlTransaction
                    Dim sqlConn As New SqlConnection
                    Dim sqlCmdInsert As SqlCommand


                    sqlConn.ConnectionString = ConstSession.StringConnection

                    sqlConn.Open()
                    sqlTrans = sqlConn.BeginTransaction

                    sSQL = "DELETE FROM TP_TIPOATTIVITA WHERE IDTIPOATTIVITA=" & myDetail.IDAttivita

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

                    sSQL = "SELECT DESCRIZIONE FROM TP_TIPOATTIVITA WHERE DESCRIZIONE=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.Descrizione), "'", "''"))
                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            Throw New Exception("La descrizione Inserita è già presente  in tabella !")
                        Next
                    End If
                    dvMyDati.Dispose()

                    sSQL = "UPDATE TP_TIPOATTIVITA SET "
                    sSQL += "DESCRIZIONE =" & Utility.StringOperation.FormatString(UCase(myDetail.Descrizione))
                    sSQL += ",NOTE=" & Utility.StringOperation.FormatString(UCase(myDetail.Note))
                    sSQL += " WHERE IDTIPOATTIVITA=" & myDetail.IDAttivita

                    Try
                        If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                            Throw New Exception("errore in::" & sSQL)
                        End If
                    Catch ex As SqlException
                        Throw ex
                    Catch ex As Exception
                        Throw ex
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBAttivita.SetATTIVITA.errore: ", ex)
                    End Try

                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBAttivita.SetATTIVITA.errore: ", ex)
            End Try
        End Sub
        ''' <summary>
        ''' Elimina un oggetto
        ''' </summary>
        ''' <param name="IDTIPOATTIVITA">intero</param>
        ''' <remarks></remarks>
        Public Sub EliminaAttivita(ByVal IDTIPOATTIVITA As Integer)
            Dim sSQL As String
            Try
                sSQL = "DELETE FROM TP_TIPOATTIVITA WHERE IDTIPOATTIVITA=" & IDTIPOATTIVITA
                If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                    Throw New Exception("errore in::" & sSQL)
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBAttivita.EliminaAttivita.errore: ", ex)
                Throw ex
            End Try
        End Sub
        ''' <summary>
        ''' Modifica un oggetto
        ''' </summary>
        ''' <param name="myDetail">oggetto di tipo TabelleDiDecodifica.DetailAttivita</param>
        ''' <remarks></remarks>
        Public Sub UpdateForzato(ByVal myDetail As TabelleDiDecodifica.DetailAttivita)

            Dim sSQL As String
            Dim dvMyDati As New DataView
            Try
                sSQL = "SELECT DESCRIZIONE FROM TP_TIPOATTIVITA WHERE DESCRIZIONE=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.Descrizione), "'", "''"))
                dvMyDati = iDB.GetDataView(sSQL)
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        Throw New Exception("La descrizione Inserita è già presente  in tabella !")
                    Next
                End If
                dvMyDati.Dispose()

                Try
                    sSQL = "UPDATE TP_TIPOATTIVITA SET "
                    sSQL += "DESCRIZIONE =" & Utility.StringOperation.FormatString(UCase(myDetail.Descrizione))
                sSQL += ",NOTE=" & Utility.StringOperation.FormatString(UCase(myDetail.Note))
                sSQL += " WHERE IDTIPOATTIVITA=" & myDetail.IDAttivita
                    If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                        Throw New Exception("errore in::" & sSQL)
                    End If
                Catch ex As SqlException
                    Throw ex
                Catch ex As Exception
                    Throw ex
                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBAttivita.UpdateForzato.errore: ", ex)
                End Try
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBAttivita.UpdateForzato.errore: ", ex)
            End Try
        End Sub
    End Class
End Namespace
