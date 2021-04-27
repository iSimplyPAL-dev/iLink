Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports log4net

Namespace TabelleDiDecodifica
    Public Class DBAnomalie
        'Inherits TabelleDiDecodifica.DetailFognatura
        ''' <summary>
        ''' Gestione delle Anomalie delle letture
        ''' </summary>
        ''' <remarks></remarks>
        Dim iDB As New DBAccess.getDBobject()
        Dim _Const As New Costanti()
        Private Shared Log As ILog = LogManager.GetLogger(GetType(DBAnomalie))

        'Public Function GetListaAnomalie() As GetLista
        '  Try
        '	Dim GetLista As New GetLista()

        '	Dim oConn As New SqlConnection()
        '	Dim oCmd As New SqlCommand()

        '	GetLista.lngRecodvMyDatiCount = DBAccess.RunSPReturnToGrid("sp_ReturnAnomalie ", oConn, oCmd)

        '	GetLista.oConn = oConn
        '	GetLista.oComm = oCmd


        '	Return GetLista
        '  Catch ex As Exception
        'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBAnomalie.GetListaAnomalie.errore: ", ex)
        '	Throw New Exception("GetListaAnomalie[DBAnomalie]." & "Errore caricamento tabella Anomalie")
        '  End Try

        'End Function
        ''' <summary>
        ''' Estrae l'elenco degli oggetti configurati
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetListaAnomalie() As DataView
            Dim cmdMyCommand As New SqlCommand
            Dim myDv As DataView = Nothing
            Try
                cmdMyCommand.CommandType = CommandType.StoredProcedure
                cmdMyCommand.CommandText = "sp_ReturnAnomalie"
                myDv = iDB.GetDataView(cmdMyCommand)
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBAnomalie.GetListaAnomalie.errore: ", ex)
            End Try
            Return myDv
        End Function
        ''' <summary>
        ''' Estrae l'oggetto in base al codice passato
        ''' </summary>
        ''' <param name="CODANOMALIA">intero</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetAnomalie(ByVal CODANOMALIA As Integer) As TabelleDiDecodifica.DetailAnomalie
            Dim lngTipoOperazione As Long
            Dim sSQL As String
            Dim DetailAnomalie As New TabelleDiDecodifica.DetailAnomalie()
            Try
                If CODANOMALIA = -1 Then lngTipoOperazione = DecEnum.DBOperation.DB_INSERT

                If lngTipoOperazione = DecEnum.DBOperation.DB_UPDATE Then
                    Dim dvMyDati As New DataView
                    sSQL = "SELECT * FROM TP_ANOMALIE WHERE CODANOMALIA = " & Utility.StringOperation.FormatString(CODANOMALIA)
                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            DetailAnomalie.Descrizione = Utility.StringOperation.FormatString(myRow("DESCRIZIONE"))
                            DetailAnomalie.CodiceAnomalia = Utility.StringOperation.FormatString(myRow("CODICEANOMALIA"))
                            DetailAnomalie.Note = Utility.StringOperation.FormatString(myRow("NOTE"))
                        Next
                    End If
                    dvMyDati.Dispose()
                End If

                If lngTipoOperazione = DecEnum.DBOperation.DB_INSERT Then
                    DetailAnomalie.CodAnomalia = -1
                    DetailAnomalie.CodiceAnomalia = ""
                    DetailAnomalie.Descrizione = ""
                    DetailAnomalie.Note = ""
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBAnomalie.GetAnomalie.errore: ", ex)
            End Try
            Return DetailAnomalie
        End Function
        ''' <summary>
        ''' Inserisce un oggetto
        ''' </summary>
        ''' <param name="myDetail">oggetto di tipo TabelleDiDecodifica.DetailAnomalie</param>
        ''' <remarks></remarks>
        Public Sub SetANOMALIE(ByVal myDetail As TabelleDiDecodifica.DetailAnomalie)

            Dim lngTipoOperazione As Long
            Dim sSQL As String
            Dim dvMyDati As New DataView

            lngTipoOperazione = DecEnum.DBOperation.DB_UPDATE
            Try
                If myDetail.CodAnomalia = -1 Then
                    lngTipoOperazione = DecEnum.DBOperation.DB_INSERT
                End If

                If lngTipoOperazione = DecEnum.DBOperation.DB_INSERT Then
                    ''''Verifica esistenza descrizione 
                    sSQL = "SELECT DESCRIZIONE FROM TP_ANOMALIE WHERE DESCRIZIONE=" & Replace(Trim(myDetail.Descrizione), "'", "''")
                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            Throw New Exception("La descrizione Inserita è già presente  in tabella !")
                        Next
                    End If
                    dvMyDati.Dispose()

                    sSQL = "SELECT CODICEANOMALIA FROM TP_ANOMALIE "
                    sSQL += " WHERE CODICEANOMALIA=" & Replace(Trim(myDetail.CodiceAnomalia), "'", "''")
                    sSQL += " AND(CODICEANOMALIA IS NOT NULL  AND CODICEANOMALIA <>'')"
                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            Throw New Exception("Il Codice Anomalia inserito è già presente in Tabella !")
                        Next
                    End If
                    dvMyDati.Dispose()
                    sSQL = "INSERT INTO TP_ANOMALIE"
                    sSQL += "(CODICEANOMALIA,DESCRIZIONE,NOTE)"
                    sSQL += " VALUES ( "
                    sSQL += Utility.StringOperation.FormatString(UCase(myDetail.CodiceAnomalia)) & ","
                    sSQL += Utility.StringOperation.FormatString(UCase(myDetail.Descrizione)) & ","
                    sSQL += Utility.StringOperation.FormatString(UCase(myDetail.Note))
                    sSQL += " )"
                    Try
                        If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                            Throw New Exception("errore in::" & sSQL)
                        End If
                    Catch ex As SqlException
                        Throw ex
                    Catch ex As Exception
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBAnomalie.SetANOMALIE.errore: ", ex)
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
                    sSQL = "DELETE FROM TP_ANOMALIE WHERE CODANOMALIA=" & myDetail.CodAnomalia

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
                    sSQL = "SELECT CODICEANOMALIA"
                    sSQL += " FROM TP_ANOMALIE"
                    sSQL += " WHERE CODICEANOMALIA=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.CodiceAnomalia), "'", "''"))
                    sSQL += " AND (CODICEANOMALIA IS NOT NULL  AND CODICEANOMALIA <>'')"
                    sSQL += " AND CODANOMALIA <> " & myDetail.CodAnomalia

                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            Throw New Exception("Il Codice Anomalia inserito è già presente in Tabella !")
                        Next
                    End If
                    dvMyDati.Dispose()

                    sSQL = "UPDATE TP_ANOMALIE SET "
                    sSQL += "CODICEANOMALIA =" & Utility.StringOperation.FormatString(UCase(myDetail.CodiceAnomalia))
                    sSQL += ",DESCRIZIONE =" & Utility.StringOperation.FormatString(UCase(myDetail.Descrizione))
                    sSQL += ",NOTE=" & Utility.StringOperation.FormatString(UCase(myDetail.Note))
                    sSQL += " WHERE CODANOMALIA=" & myDetail.CodAnomalia

                    Try
                        If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                            Throw New Exception("errore in::" & sSQL)
                        End If
                    Catch ex As SqlException
                        Throw ex
                    Catch ex As Exception
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBAnomalie.SetANOMALIE.errore: ", ex)
                        Throw ex
                    End Try

                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBAnomalie.SetANOMALIE.errore: ", ex)
            End Try
        End Sub
        ''' <summary>
        ''' Elimina un oggetto
        ''' </summary>
        ''' <param name="CODANOMALIA">intero</param>
        ''' <remarks></remarks>
        Public Sub EliminaAnomalia(ByVal CODANOMALIA As Integer)
            Dim sSQL As String
            Try
                sSQL = "DELETE FROM TP_ANOMALIE WHERE CODANOMALIA=" & CODANOMALIA
                If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                    Throw New Exception("errore in::" & sSQL)
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBAnomalie.EliminaAnomalia.errore: ", ex)
                Throw ex
            End Try
        End Sub
        ''' <summary>
        ''' Modifica un oggetto
        ''' </summary>
        ''' <param name="myDetail">oggetto di tipo TabelleDiDecodifica.DetailAnomalie</param>
        ''' <remarks></remarks>
        Public Sub UpdateForzato(ByVal myDetail As TabelleDiDecodifica.DetailAnomalie)
            Dim sSQL As String
            Dim dvMyDati As New DataView
            Try
                sSQL = "SELECT CODICEANOMALIA"
                sSQL += " FROM TP_ANOMALIE"
                sSQL += " WHERE CODICEANOMALIA=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.CodiceAnomalia), "'", "''"))
                sSQL += " AND (CODICEANOMALIA IS NOT NULL  AND CODICEANOMALIA <>'')"
                sSQL += " AND CODANOMALIA <> " & myDetail.CodAnomalia
                dvMyDati = iDB.GetDataView(sSQL)
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        Throw New Exception("Il Codice Anomalia inserito è già presente in Tabella !")
                    Next
                End If
                dvMyDati.Dispose()

                Try
                    sSQL = "UPDATE TP_ANOMALIE SET "
                    sSQL += "CODICEANOMALIA =" & Utility.StringOperation.FormatString(UCase(myDetail.CodiceAnomalia))
                sSQL += ",DESCRIZIONE =" & Utility.StringOperation.FormatString(UCase(myDetail.Descrizione))
                sSQL += ",NOTE=" & Utility.StringOperation.FormatString(UCase(myDetail.Note))
                sSQL += " WHERE CODANOMALIA=" & myDetail.CodAnomalia
                    If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                        Throw New Exception("errore in::" & sSQL)
                    End If
                Catch ex As SqlException
                    Throw ex
                Catch ex As Exception
                    Throw ex
                End Try
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBAnomalie.UpdateForzato.errore: ", ex)
            End Try
        End Sub

    End Class
End Namespace
