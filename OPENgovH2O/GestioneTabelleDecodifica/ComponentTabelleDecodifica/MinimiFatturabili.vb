Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports log4net
Imports Utility

Namespace TabelleDiDecodifica
    Public Class DBMinimiFatturabili
        Inherits TabelleDiDecodifica.MinimiFatturabili
        ''' <summary>
        ''' Gestione del minimo fatturabile
        ''' </summary>
        ''' <remarks></remarks>
        Dim iDB As New DBAccess.getDBobject()
        Dim _Const As New Costanti()
        Private Shared Log As ILog = LogManager.GetLogger(GetType(DBMinimiFatturabili))

        ''' <summary>
        ''' Estrae l'elenco degli oggetti configurati
        ''' </summary>
        ''' <returns></returns>
        ''' <revisionHistory>
        ''' <revision date="12/04/2019">
        ''' Modifiche da revisione manuale
        ''' </revision>
        ''' </revisionHistory>
        Public Function GetListaMinimiFatturabili(IDTipoUtenza As Integer) As DataView
            Dim sSQL As String
            Dim dvMyDati As DataView = Nothing

            Try
                Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"sp_ReturnMinimiFatturabili", "IDTIPOUTENZA")
                    dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDTIPOUTENZA", IDTipoUtenza))
                    ctx.Dispose()
                End Using
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBMinimiFatturabili.GetListaMinimiFatturabili.errore: ", ex)
                Throw New Exception("GetListaMinimiFatturabili[DBMinimiFatturabili]." & "Errore caricamento Minimi Fatturabili")
            End Try
            Return dvMyDati
        End Function
        'Public Function GetListaMinimiFatturabili() As DataView
        '    Dim cmdMyCommand As New SqlCommand
        '    Dim myDv As DataView = Nothing
        '    Try
        '        cmdMyCommand.CommandType = CommandType.StoredProcedure
        '        cmdMyCommand.CommandText = "sp_ReturnMinimiFatturabili"

        '        myDv = iDB.GetDataView(cmdMyCommand)
        '    Catch ex As Exception
        '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBMinimiFatturabili.GetListaMinimiFatturabili.errore: ", ex)
        '        Throw New Exception("GetListaMinimiFatturabili[DBMinimiFatturabili]." & "Errore caricamento Minimi Fatturabili")
        '    End Try
        '    Return myDv
        'End Function

        'Public Function GetListaMinimiFatturabili() As GetLista
        '    Try
        '        Dim GetLista As New GetLista()

        '        Dim oConn As New SqlConnection()
        '        Dim oCmd As New SqlCommand()

        '        GetLista.lngRecodvMyDatiCount = iDB.RunSPReturnToGrid("sp_ReturnMinimiFatturabili", oConn, oCmd)

        '        GetLista.oConn = oConn
        '        GetLista.oComm = oCmd


        '        Return GetLista
        '    Catch ex As Exception
        'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBMinimiFatturabili.GetListaMinimiFatturabili.errore: ", ex)
        '        Throw New Exception("GetListaMinimiFatturabili[DBMinimiFatturabili]." & "Errore caricamento Minimi Fatturabili")
        '    End Try

        'End Function
        ''' <summary>
        ''' Estrae l'oggetto in base al codice passato
        ''' </summary>
        ''' <param name="IDMINIMO">intero</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetMinimiFattiurabile(ByVal IDMINIMO As Integer) As TabelleDiDecodifica.MinimiFatturabili

            Dim lngTipoOperazione As Long
            'Dim _enum As _Enum
            Dim sSQL As String

            Dim MinimiFatturabili As New TabelleDiDecodifica.MinimiFatturabili()
            Try
                If IDMINIMO = -1 Then lngTipoOperazione = DecEnum.DBOperation.DB_INSERT
                MinimiFatturabili.dsTipoUtenza = iDB.RunSPReturnDataSet("sp_ReturnTipiUtenza", "TP_TIPIUTENZA", New SqlParameter("@COD_ENTE", ConstSession.IdEnte))


                If lngTipoOperazione = DecEnum.DBOperation.DB_UPDATE Then
                    Dim dvMyDati As New DataView
                    sSQL = "SELECT * FROM TP_MININIMIFATTURABILI WHERE IDMINIMO = " & Utility.StringOperation.FormatString(IDMINIMO)
                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            MinimiFatturabili.Descrizione = Utility.StringOperation.FormatString(myRow("DESCRIZIONE"))
                            MinimiFatturabili.Minimo = Utility.StringOperation.FormatString(myRow("MINIMO"))
                            MinimiFatturabili.TipoUtenza = MyUtility.CIdFromDB(myRow("IDTIPOUTENZA"))
                            MinimiFatturabili.Note = Utility.StringOperation.FormatString(myRow("NOTE"))
                        Next
                    End If

                    dvMyDati.Dispose()

                End If

                If lngTipoOperazione = DecEnum.DBOperation.DB_INSERT Then

                    MinimiFatturabili.Descrizione = ""
                    MinimiFatturabili.Minimo = ""
                    MinimiFatturabili.TipoUtenza = -1
                    MinimiFatturabili.Note = ""

                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBMinimiFatturabili.GetMinimiFatturabili.errore: ", ex)
            End Try
            Return MinimiFatturabili
        End Function
        ''' <summary>
        ''' Inserisce un oggetto
        ''' </summary>
        ''' <param name="myDetail">oggetto di tipo TabelleDiDecodifica.MinimiFatturabili</param>
        ''' <remarks></remarks>
        Public Sub SetMinimiFatturabili(ByVal myDetail As TabelleDiDecodifica.MinimiFatturabili)

            Dim lngTipoOperazione As Long
            Dim sSQL As String

            Dim dvMyDati As New DataView

            lngTipoOperazione = DecEnum.DBOperation.DB_UPDATE
            Try
                If myDetail.IDMinimo = -1 Then
                    lngTipoOperazione = DecEnum.DBOperation.DB_INSERT
                End If

                If lngTipoOperazione = DecEnum.DBOperation.DB_INSERT Then
                    ''''Verifica esistenza descrizione 

                    sSQL = ""
                    sSQL = "SELECT DESCRIZIONE FROM TP_MININIMIFATTURABILI WHERE DESCRIZIONE=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.Descrizione), "'", "''")) & vbCrLf
                    sSQL += "AND" & vbCrLf
                    sSQL += "IDTIPOUTENZA = " & myDetail.TipoUtenza

                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            Throw New Exception("La descrizione Inserita è già presente  in tabella !")
                        Next
                    End If
                    dvMyDati.Dispose()


                    sSQL = ""
                    sSQL = "INSERT INTO TP_MININIMIFATTURABILI"
                    sSQL += "(IDTIPOUTENZA,DESCRIZIONE,MINIMO,NOTE)" & vbCrLf
                    sSQL += "VALUES ( " & vbCrLf
                    sSQL += Utility.StringOperation.FormatInt(UCase(myDetail.TipoUtenza)) & "," & vbCrLf
                    sSQL += Utility.StringOperation.FormatString(UCase(myDetail.Descrizione)) & "," & vbCrLf
                    sSQL += Utility.StringOperation.FormatString(UCase(myDetail.Minimo)) & "," & vbCrLf
                    sSQL += Utility.StringOperation.FormatString(UCase(myDetail.Note)) & vbCrLf
                    sSQL += " )"
                    Try
                        If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                            Throw New Exception("errore in::" & sSQL)
                        End If
                    Catch ex As SqlException
                        Throw ex
                    Catch ex As Exception
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBMinimiFatturabili.SetMinimiFatturabili.errore: ", ex)
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
                    sSQL = "DELETE FROM TP_MININIMIFATTURABILI WHERE IDMINIMO=" & myDetail.IDMinimo

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
                    sSQL = "SELECT DESCRIZIONE FROM TP_MININIMIFATTURABILI WHERE DESCRIZIONE=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.Descrizione), "'", "''")) & vbCrLf
                    sSQL += "AND" & vbCrLf
                    sSQL += "IDMINIMO <>" & myDetail.IDMinimo
                    sSQL += "AND" & vbCrLf
                    sSQL += "IDTIPOUTENZA = " & myDetail.TipoUtenza


                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            Throw New Exception("La descrizione Inserita è già presente  in tabella !")
                        Next
                    End If
                    dvMyDati.Dispose()



                    sSQL = "UPDATE TP_MININIMIFATTURABILI SET "
                    sSQL += "IDTIPOUTENZA =" & Utility.StringOperation.FormatInt(UCase(myDetail.TipoUtenza)) & vbCrLf
                    sSQL += ",DESCRIZIONE =" & Utility.StringOperation.FormatString(UCase(myDetail.Descrizione)) & vbCrLf
                    sSQL += ",MINIMO =" & Utility.StringOperation.FormatString(UCase(myDetail.Minimo)) & vbCrLf
                    sSQL += ",NOTE =" & Utility.StringOperation.FormatString(UCase(myDetail.Note)) & vbCrLf
                    sSQL += "WHERE" & vbCrLf
                    sSQL += "IDMINIMO=" & myDetail.IDMinimo

                    Try
                        If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                            Throw New Exception("errore in::" & sSQL)
                        End If
                    Catch ex As SqlException
                        Throw ex
                    Catch ex As Exception
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBMinimiFatturabili.SetMinimiFatturabili.errore: ", ex)
                        Throw ex
                    End Try

                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBMinimiFatturabili.SetMinimiFatturabili.errore: ", ex)
            End Try
        End Sub
        ''' <summary>
        ''' Modifica un oggetto
        ''' </summary>
        ''' <param name="myDetail">oggetto di tipo TabelleDiDecodifica.MinimiFatturabili</param>
        ''' <remarks></remarks>
        Public Sub UpdateForzatoMinimoFatturabile(ByVal myDetail As TabelleDiDecodifica.MinimiFatturabili)

            Dim sSQL As String
            Dim dvMyDati As New DataView

            sSQL = ""
            sSQL = "SELECT DESCRIZIONE FROM TP_MININIMIFATTURABILI WHERE DESCRIZIONE=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.Descrizione), "'", "''")) & vbCrLf
            sSQL += "AND" & vbCrLf
            sSQL += "IDMINIMO <>" & myDetail.IDMinimo
            sSQL += "AND" & vbCrLf
            sSQL += "IDTIPOUTENZA = " & myDetail.TipoUtenza


            dvMyDati = iDB.GetDataView(sSQL)
            Try
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        Throw New Exception("La descrizione Inserita è già presente  in tabella !")
                    Next
                End If
                dvMyDati.Dispose()



                sSQL = "UPDATE TP_MININIMIFATTURABILI SET "
                sSQL += "IDTIPOUTENZA =" & Utility.StringOperation.FormatInt(UCase(myDetail.TipoUtenza)) & vbCrLf
                sSQL += ",DESCRIZIONE =" & Utility.StringOperation.FormatString(UCase(myDetail.Descrizione)) & vbCrLf
                sSQL += ",MINIMO =" & Utility.StringOperation.FormatString(UCase(myDetail.Minimo)) & vbCrLf
                sSQL += ",NOTE =" & Utility.StringOperation.FormatString(UCase(myDetail.Note)) & vbCrLf
                sSQL += "WHERE" & vbCrLf
                sSQL += "IDMINIMO=" & myDetail.IDMinimo

                Try
                    If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                        Throw New Exception("errore in::" & sSQL)
                    End If
                Catch ex As SqlException
                    Throw ex
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBMinimiFatturabili.UpdateForzatoMinimoFatturabile.errore: ", ex)
                    Throw ex
                End Try
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBMinimiFatturabili.UpdateForzatoMinimoFatturabile.errore: ", ex)
            End Try
        End Sub
        ''' <summary>
        ''' Elimina un oggetto
        ''' </summary>
        ''' <param name="IDMINIMO">intero</param>
        ''' <remarks></remarks>
        Public Sub EliminaMinimoFatturabile(ByVal IDMINIMO As Integer)
            Dim sSQL As String
            Try
                sSQL = "DELETE FROM TP_MININIMIFATTURABILI WHERE IDMINIMO=" & IDMINIMO
                If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                    Throw New Exception("errore in::" & sSQL)
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBMinimiFatturabili.EliminaMinimoFatturabile.errore: ", ex)
                Throw ex
            End Try
        End Sub
    End Class
End Namespace