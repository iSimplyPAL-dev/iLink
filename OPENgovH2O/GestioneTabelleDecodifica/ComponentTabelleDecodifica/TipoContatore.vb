Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports log4net

Namespace TabelleDiDecodifica
    Public Class DBTipoContatore
        Inherits TabelleDiDecodifica.TipoContatore
        ''' <summary>
        ''' Gestione tipologia contatore
        ''' </summary>
        ''' <remarks></remarks>
        Private iDB As New DBAccess.getDBobject()
        Private _Const As New Costanti()
        Private Shared Log As ILog = LogManager.GetLogger(GetType(DBTipoContatore))

        ''' <summary>
        ''' Estrae l'elenco degli oggetti configurati
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetListaTipoContatore() As DataView
            Dim cmdMyCommand As New SqlCommand
            Dim myDv As DataView = Nothing
            Try
                cmdMyCommand.CommandType = CommandType.StoredProcedure
                cmdMyCommand.CommandText = "sp_ReturnTipiContatore"

                myDv = iDB.GetDataView(cmdMyCommand)
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBTipoContatore.GetListaTipoContatore.errore: ", ex)
                Throw New Exception("GetListaTipoContatore[DBTipoContatore]." & "Errore caricamento Tipo Contatore")
            End Try
            Return myDv
        End Function
        'Public Function GetListaTipoContatore() As GetLista
        '    Try
        '        Dim GetLista As New GetLista()

        '        Dim oConn As New SqlConnection()
        '        Dim oCmd As New SqlCommand()

        '        GetLista.lngRecodvMyDatiCount = iDB.RunSPReturnToGrid("sp_ReturnTipiContatore", oConn, oCmd)

        '        GetLista.oConn = oConn
        '        GetLista.oComm = oCmd


        '        Return GetLista
        '    Catch ex As Exception
        '  Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBTipoContatore.GetListaTipoContatore.errore: ", ex)
        '        Throw New Exception("GetListaTipoContatore[DBTipoContatore]." & "Errore caricamento Tipo Contatore")
        '    End Try
        'End Function
        ''' <summary>
        ''' Estrae l'oggetto in base al codice passato
        ''' </summary>
        ''' <param name="IDTIPOCONTATORE">intero</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetTipoContatore(ByVal IDTIPOCONTATORE As Integer) As TabelleDiDecodifica.TipoContatore

            Dim lngTipoOperazione As Long
            Dim sSQL As String

            Dim DetailTipoContatore As New TabelleDiDecodifica.TipoContatore()
            Try
                If IDTIPOCONTATORE = -1 Then lngTipoOperazione = DecEnum.DBOperation.DB_INSERT

                If lngTipoOperazione = DecEnum.DBOperation.DB_UPDATE Then
                    Dim dvMyDati As New DataView
                    sSQL = ""
                    sSQL = "SELECT * FROM TP_TIPOCONTATORE WHERE IDTIPOCONTATORE = " & Utility.StringOperation.FormatString(IDTIPOCONTATORE)

                    dvMyDati = iDB.GetDataView(sSQL)

                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            DetailTipoContatore.Descrizione = Utility.StringOperation.FormatString(myRow("DESCRIZIONE"))
                            DetailTipoContatore.FondoScala = Utility.StringOperation.FormatInt(myRow("VALOREFONDOSCALA"))
                            DetailTipoContatore.Note = Utility.StringOperation.FormatString(myRow("NOTE"))
                        Next
                    End If

                    dvMyDati.Dispose()

                End If

                If lngTipoOperazione = DecEnum.DBOperation.DB_INSERT Then

                    DetailTipoContatore.IDTIPOCONTATORE = -1
                    DetailTipoContatore.FondoScala = -1
                    DetailTipoContatore.Descrizione = ""
                    DetailTipoContatore.Note = ""

                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBTipoContatore.GetTipoContatore.errore: ", ex)
            End Try
            Return DetailTipoContatore
        End Function
        ''' <summary>
        ''' Inserisce un oggetto
        ''' </summary>
        ''' <param name="myDetail">oggetto di tipo TabelleDiDecodifica.TipoContatore</param>
        ''' <remarks></remarks>
        Public Sub SetTipoContatore(ByVal myDetail As TabelleDiDecodifica.TipoContatore)

            Dim lngTipoOperazione As Long
            Dim sSQL As String
            Dim dvMyDati As New DataView

            lngTipoOperazione = DecEnum.DBOperation.DB_UPDATE
            Try
                If myDetail.IDTIPOCONTATORE = -1 Then
                    lngTipoOperazione = DecEnum.DBOperation.DB_INSERT
                End If

                If lngTipoOperazione = DecEnum.DBOperation.DB_INSERT Then
                    ''''Verifica esistenza descrizione 

                    sSQL = ""
                    sSQL = "SELECT DESCRIZIONE FROM TP_TIPOCONTATORE WHERE DESCRIZIONE=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.Descrizione), "'", "''"))

                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            Throw New Exception("La descrizione Inserita è già presente  in tabella !")
                        Next
                    End If
                    dvMyDati.Dispose()


                    sSQL = ""
                    sSQL = "INSERT INTO TP_TIPOCONTATORE"
                    sSQL += "(VALOREFONDOSCALA,DESCRIZIONE,NOTE)" & vbCrLf
                    sSQL += "VALUES ( " & vbCrLf
                    sSQL += Utility.StringOperation.FormatInt(UCase(myDetail.FondoScala)) & "," & vbCrLf
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
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBTipoContatore.SetTipoContatore.errore: ", ex)
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
                    sSQL = "DELETE FROM TP_TIPOCONTATORE WHERE IDTIPOCONTATORE=" & myDetail.IDTIPOCONTATORE

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
                    sSQL = "SELECT DESCRIZIONE FROM TP_TIPOCONTATORE WHERE DESCRIZIONE=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.Descrizione), "'", "''"))
                    sSQL += "AND" & vbCrLf
                    sSQL += "IDTIPOCONTATORE <>" & myDetail.IDTIPOCONTATORE

                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            Throw New Exception("La descrizione Inserita è già presente  in tabella !")
                        Next
                    End If
                    dvMyDati.Dispose()

                    sSQL = "UPDATE TP_TIPOCONTATORE SET "
                    sSQL += "VALOREFONDOSCALA =" & Utility.StringOperation.FormatInt(myDetail.FondoScala) & vbCrLf
                    sSQL += ",DESCRIZIONE =" & Utility.StringOperation.FormatString(UCase(myDetail.Descrizione)) & vbCrLf
                    sSQL += ",NOTE=" & Utility.StringOperation.FormatString(UCase(myDetail.Note)) & vbCrLf
                    sSQL += "WHERE" & vbCrLf
                    sSQL += "IDTIPOCONTATORE=" & myDetail.IDTIPOCONTATORE

                    Try
                        If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                            Throw New Exception("errore in::" & sSQL)
                        End If
                    Catch ex As SqlException
                        Throw ex
                    Catch ex As Exception
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBTipoContatore.SetTipoContatore.errore: ", ex)
                        Throw ex
                    End Try

                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBTipoContatore.SetTipoContatore.errore: ", ex)
            End Try
        End Sub
        ''' <summary>
        ''' Elimina un oggetto
        ''' </summary>
        ''' <param name="IDTIPOCONTATORE">intero</param>
        ''' <remarks></remarks>
        Public Sub EliminaTipoContatore(ByVal IDTIPOCONTATORE As Integer)
            Dim sSQL As String
            Try
                sSQL = "DELETE FROM TP_TIPOCONTATORE WHERE IDTIPOCONTATORE=" & IDTIPOCONTATORE
                If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                    Throw New Exception("errore in::" & sSQL)
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBTipoContatore.EliminaTipoContatore.errore: ", ex)
                Throw ex
            End Try
        End Sub
        ''' <summary>
        ''' Modifica un oggetto
        ''' </summary>
        ''' <param name="myDetail">oggetto di tipo TabelleDiDecodifica.TipoContatore</param>
        ''' <remarks></remarks>
        Public Sub UpdateForzato(ByVal myDetail As TabelleDiDecodifica.TipoContatore)
            Dim sSQL As String
            Dim dvMyDati As New DataView

            sSQL = ""
            sSQL = "SELECT DESCRIZIONE FROM TP_TIPOCONTATORE WHERE DESCRIZIONE=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.Descrizione), "'", "''"))
            sSQL += "AND" & vbCrLf
            sSQL += "IDTIPOCONTATORE <>" & myDetail.IDTIPOCONTATORE

            dvMyDati = iDB.GetDataView(sSQL)
            Try
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        Throw New Exception("La descrizione Inserita è già presente  in tabella !")
                    Next
                End If
                dvMyDati.Dispose()

                sSQL = "UPDATE TP_TIPOCONTATORE SET "
                sSQL += "VALOREFONDOSCALA =" & utility.stringoperation.formatint(myDetail.FondoScala) & vbCrLf
                sSQL += ",DESCRIZIONE =" & utility.stringoperation.formatstring(UCase(myDetail.Descrizione)) & vbCrLf
                sSQL += ",NOTE=" & utility.stringoperation.formatstring(UCase(myDetail.Note)) & vbCrLf
                sSQL += "WHERE" & vbCrLf
                sSQL += "IDTIPOCONTATORE=" & myDetail.IDTIPOCONTATORE

                Try
                    If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                        Throw New Exception("errore in::" & sSQL)
                    End If
                Catch ex As SqlException
                    Throw ex
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBTipoContatore.UpdateForzato.errore: ", ex)
                    Throw ex
                End Try
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBTipoContatore.UpdateForzato.errore: ", ex)
            End Try
        End Sub
    End Class
End Namespace
