Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports log4net

Namespace TabelleDiDecodifica
    Public Class DBImpianti
        Inherits TabelleDiDecodifica.Impianti
        ''' <summary>
        ''' Gestione impianto di appartenenza contatore
        ''' </summary>
        ''' <remarks></remarks>
        Dim iDB As New DBAccess.getDBobject()
        Dim _Const As New Costanti()
        Private Shared Log As ILog = LogManager.GetLogger(GetType(DBImpianti))

        ''' <summary>
        ''' Estrae l'elenco degli oggetti configurati
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetListaImpianti() As DataView
            Dim cmdMyCommand As New SqlCommand
            Dim myDv As DataView = Nothing
            Try
                cmdMyCommand.CommandType = CommandType.StoredProcedure
                cmdMyCommand.CommandText = "sp_ReturnTipiImpianto"

                myDv = iDB.GetDataView(cmdMyCommand)
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBImpianti.GetListaImpianti.errore: ", ex)
                Throw New Exception("GetListaImpianti[DBImpianti]." & "Errore caricamento Impianti")
            End Try
            Return myDv
        End Function
        'Public Function GetListaImpianti() As GetLista
        '    Try
        '        Dim GetLista As New GetLista()

        '        Dim oConn As New SqlConnection()
        '        Dim oCmd As New SqlCommand()

        '        GetLista.lngRecodvMyDatiCount = iDB.RunSPReturnToGrid("sp_ReturnTipiImpianto", oConn, oCmd)

        '        GetLista.oConn = oConn
        '        GetLista.oComm = oCmd


        '        Return GetLista
        '    Catch ex As Ex
        'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBImpianti.GetListaImpianti.errore: ", ex)ception

        '        Throw New Exception("GetListaImpianti[DBImpianti]." & "Errore caricamento Impianti")
        '    End Try
        'End Function
        ''' <summary>
        ''' Estrae l'oggetto in base al codice passato
        ''' </summary>
        ''' <param name="IDIMPIANTO">intero</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetImpianti(ByVal IDIMPIANTO As Integer) As TabelleDiDecodifica.Impianti

            Dim lngTipoOperazione As Long
            Dim sSQL As String

            Dim DetailImpianti As New TabelleDiDecodifica.Impianti()
            Try
                If IDIMPIANTO = -1 Then lngTipoOperazione = DecEnum.DBOperation.DB_INSERT

                If lngTipoOperazione = DecEnum.DBOperation.DB_UPDATE Then
                    Dim dvMyDati As New DataView
                    sSQL = ""
                    sSQL = "SELECT * FROM TP_IMPIANTO WHERE IDIMPIANTO = " & Utility.StringOperation.FormatString(IDIMPIANTO)

                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            DetailImpianti.Descrizione = Utility.StringOperation.FormatString(myRow("DESCRIZIONE"))
                            DetailImpianti.CodiceImpianto = Utility.StringOperation.FormatString(myRow("CODICEIMPIANTO"))
                            DetailImpianti.Note = Utility.StringOperation.FormatString(myRow("NOTE"))
                        Next
                    End If

                    dvMyDati.Dispose()

                End If

                If lngTipoOperazione = DecEnum.DBOperation.DB_INSERT Then

                    DetailImpianti.IDImpianto = -1
                    DetailImpianti.CodiceImpianto = ""
                    DetailImpianti.Descrizione = ""
                    DetailImpianti.Note = ""

                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBImpianti.GetImpianti.errore: ", ex)
            End Try
            Return DetailImpianti
        End Function
        ''' <summary>
        ''' Inserisce un oggetto
        ''' </summary>
        ''' <param name="myDetail">oggetto di tipo TabelleDiDecodifica.Impianti</param>
        ''' <remarks></remarks>
        Public Sub SetImpianto(ByVal myDetail As TabelleDiDecodifica.Impianti)

            Dim lngTipoOperazione As Long
            Dim sSQL As String
            Dim dvMyDati As New DataView

            lngTipoOperazione = DecEnum.DBOperation.DB_UPDATE
            Try
                If myDetail.IDImpianto = -1 Then
                    lngTipoOperazione = DecEnum.DBOperation.DB_INSERT
                End If

                If lngTipoOperazione = DecEnum.DBOperation.DB_INSERT Then
                    ''''Verifica esistenza descrizione 

                    sSQL = ""
                    sSQL = "SELECT DESCRIZIONE FROM TP_IMPIANTO WHERE DESCRIZIONE=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.Descrizione), "'", "''"))

                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            Throw New Exception("La descrizione Inserita è già presente  in tabella !")
                        Next
                    End If
                    dvMyDati.Dispose()

                    sSQL = ""
                    sSQL = "SELECT CODICEIMPIANTO FROM TP_IMPIANTO WHERE CODICEIMPIANTO=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.CodiceImpianto), "'", "''")) & vbCrLf
                    sSQL += "AND" & vbCrLf
                    sSQL += "(CODICEIMPIANTO   IS NOT NULL  AND CODICEIMPIANTO <>'')" & vbCrLf


                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            Throw New Exception("Il Codice Impianto inserito è già presente in Tabella !")
                        Next
                    End If
                    dvMyDati.Dispose()

                    sSQL = ""
                    sSQL = "INSERT INTO TP_IMPIANTO"
                    sSQL += "(CODICEIMPIANTO,DESCRIZIONE,NOTE)" & vbCrLf
                    sSQL += "VALUES ( " & vbCrLf
                    sSQL += Utility.StringOperation.FormatString(UCase(myDetail.CodiceImpianto)) & "," & vbCrLf
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
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBImpianti.SetImpianti.errore: ", ex)
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
                    sSQL = "DELETE FROM TP_IMPIANTO WHERE IDIMPIANTO=" & myDetail.IDImpianto

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
                    sSQL = "SELECT CODICEIMPIANTO FROM TP_IMPIANTO WHERE CODICEIMPIANTO=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.CodiceImpianto), "'", "''")) & vbCrLf
                    sSQL += "AND" & vbCrLf
                    sSQL += "(CODICEIMPIANTO   IS NOT NULL  AND CODICEIMPIANTO <>'')" & vbCrLf
                    sSQL += "AND" & vbCrLf
                    sSQL += "IDIMPIANTO <> " & myDetail.IDImpianto & vbCrLf


                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            Throw New Exception("Il Codice Impianto inserito è già presente in Tabella !")
                        Next
                    End If
                    dvMyDati.Dispose()

                    sSQL = "UPDATE TP_IMPIANTO SET "
                    sSQL += "CODICEIMPIANTO =" & Utility.StringOperation.FormatString(myDetail.CodiceImpianto) & vbCrLf
                    sSQL += ",DESCRIZIONE =" & Utility.StringOperation.FormatString(UCase(myDetail.Descrizione)) & vbCrLf
                    sSQL += ",NOTE=" & Utility.StringOperation.FormatString(UCase(myDetail.Note)) & vbCrLf
                    sSQL += "WHERE" & vbCrLf
                    sSQL += "IDIMPIANTO=" & myDetail.IDImpianto

                    Try
                        If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                            Throw New Exception("errore in::" & sSQL)
                        End If
                    Catch ex As SqlException
                        Throw ex
                    Catch ex As Exception
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBImpianti.SetImpianti.errore: ", ex)
                        Throw ex
                    End Try

                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBImpianti.SetImpianti.errore: ", ex)
            End Try
        End Sub
        ''' <summary>
        ''' Elimina un oggetto
        ''' </summary>
        ''' <param name="IDIMPIANTO">intero</param>
        ''' <remarks></remarks>
        Public Sub EliminaImpianto(ByVal IDIMPIANTO As Integer)
            Dim sSQL As String
            Try
                sSQL = "DELETE FROM TP_IMPIANTO WHERE IDIMPIANTO=" & IDIMPIANTO
                If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                    Throw New Exception("errore in::" & sSQL)
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBImpianti.EliminaImpianto.errore: ", ex)
                Throw ex
            End Try
        End Sub
        ''' <summary>
        ''' Modifica un oggetto
        ''' </summary>
        ''' <param name="myDetail">oggetto di tipo TabelleDiDecodifica.Impianti</param>
        ''' <remarks></remarks>
        Public Sub UpdateForzato(ByVal myDetail As TabelleDiDecodifica.Impianti)
            Dim sSQL As String
            Dim dvMyDati As New DataView

            sSQL = ""
            sSQL = "SELECT CODICEIMPIANTO FROM TP_IMPIANTO WHERE CODICEIMPIANTO=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.CodiceImpianto), "'", "''")) & vbCrLf
            sSQL += "AND" & vbCrLf
            sSQL += "(CODICEIMPIANTO   IS NOT NULL  AND CODICEIMPIANTO <>'')" & vbCrLf
            sSQL += "AND" & vbCrLf
            sSQL += "IDIMPIANTO <> " & myDetail.IDImpianto & vbCrLf


            dvMyDati = iDB.GetDataView(sSQL)
            Try
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        Throw New Exception("Il Codice Impianto inserito è già presente in Tabella !")
                    Next
                End If
                dvMyDati.Dispose()

                sSQL = "UPDATE TP_IMPIANTO SET "
                sSQL += "CODICEIMPIANTO =" & utility.stringoperation.formatstring(myDetail.CodiceImpianto) & vbCrLf
                sSQL += ",DESCRIZIONE =" & utility.stringoperation.formatstring(UCase(myDetail.Descrizione)) & vbCrLf
                sSQL += ",NOTE=" & utility.stringoperation.formatstring(UCase(myDetail.Note)) & vbCrLf
                sSQL += "WHERE" & vbCrLf
                sSQL += "IDIMPIANTO=" & myDetail.IDImpianto

                Try
                    If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                        Throw New Exception("errore in::" & sSQL)
                    End If
                Catch ex As SqlException
                    Throw ex
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBImpianti.UpdateForzato.errore: ", ex)
                    Throw ex
                End Try
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBImpianti.UpdateForzato.errore: ", ex)
            End Try
        End Sub
    End Class
End Namespace
