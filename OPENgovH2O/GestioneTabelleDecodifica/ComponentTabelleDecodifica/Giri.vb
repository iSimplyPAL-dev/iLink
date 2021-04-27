Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports log4net
Imports Utility

Namespace TabelleDiDecodifica
    Public Class DBGiri
        Inherits TabelleDiDecodifica.Giri
        ''' <summary>
        ''' Gestione giri di lettura
        ''' </summary>
        ''' <remarks></remarks>
        Dim iDB As New DBAccess.getDBobject()
        Dim _Const As New Costanti()
        Private Shared Log As ILog = LogManager.GetLogger(GetType(DBGiri))

        ''' <summary>
        ''' Estrae l'elenco degli oggetti configurati
        ''' </summary>
        ''' <param name="strEnte">intero</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetListaGiri(ByVal strEnte As Integer) As DataView
            Dim Ssql As String
            Dim dvMyDati As DataView = Nothing
            Try
                Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                    Ssql = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"sp_ReturnGiri", "CODENTE", "IDGIRO", "DESCRIZIONE", "NOTE", "GIROEST")
                    dvMyDati = ctx.GetDataView(Ssql, "TBL", ctx.GetParam("CODENTE", strEnte), ctx.GetParam("IDGIRO", -1) _
                                    , ctx.GetParam("DESCRIZIONE", "") _
                                    , ctx.GetParam("NOTE", "") _
                                    , ctx.GetParam("GIROEST", ""))
                    ctx.Dispose()
                End Using
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBGiri.GetListaGiri.errore: ", ex)
                Throw New Exception("GetListaGiri[DBGiri]." & "Errore caricamento Giri")
            End Try
            Return dvMyDati
        End Function
        'Public Function GetListaGiri(ByVal strEnte As Integer) As GetLista
        '    Try
        '        Dim GetLista As New GetLista()

        '        Dim oConn As New SqlConnection()
        '        Dim oCmd As New SqlCommand()

        '        GetLista.lngRecordCount = iDB.RunSPReturnToGrid("sp_ReturnGiri", oConn, oCmd, _
        '        New SqlParameter("@CodEnte", strEnte))

        '        GetLista.oConn = oConn
        '        GetLista.oComm = oCmd


        '        Return GetLista
        '    Catch ex As Exception
        ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBGiri.GetListaGiri.errore: ", ex)
        '        Throw New Exception("GetListaGiri[DBGiri]." & "Errore caricamento Giri")
        '    End Try
        'End Function
        ''' <summary>
        ''' Estrae l'oggetto in base al codice passato
        ''' </summary>
        ''' <param name="IDGIRO">intero</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetGiri(ByVal IDGIRO As Integer) As TabelleDiDecodifica.Giri

            Dim lngTipoOperazione As Long

            Dim DetailGiri As New TabelleDiDecodifica.Giri()
            Try
                If IDGIRO = -1 Then lngTipoOperazione = DecEnum.DBOperation.DB_INSERT

                If lngTipoOperazione = DecEnum.DBOperation.DB_UPDATE Then
                    Dim sSQL As String = ""
                    Dim dvMyDati As New DataView
                    Try
                        Dim oDbManagerRepository As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                        Using ctx As DBModel = oDbManagerRepository
                            sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"sp_ReturnGiri", "CODENTE", "IDGIRO", "DESCRIZIONE", "NOTE", "GIROEST")
                            dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("CODENTE", ConstSession.IdEnte), ctx.GetParam("IDGIRO", IDGIRO) _
                                        , ctx.GetParam("DESCRIZIONE", "") _
                                        , ctx.GetParam("NOTE", "") _
                                        , ctx.GetParam("GIROEST", ""))
                            ctx.Dispose()
                        End Using
                        If Not dvMyDati Is Nothing Then
                            For Each myRow As DataRowView In dvMyDati
                                DetailGiri.CODENTE = Utility.StringOperation.FormatInt(myRow("CODENTE"))
                                DetailGiri.Descrizione = Utility.StringOperation.FormatString(myRow("DESCRIZIONE"))
                                DetailGiri.CodiceGiro = Utility.StringOperation.FormatString(myRow("COD_GIRO_EST"))
                                DetailGiri.Note = Utility.StringOperation.FormatString(myRow("NOTE"))
                            Next
                        End If
                    Catch ex As Exception
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBGiri.GetGiri.query.errore: ", ex)
                    Finally
                        dvMyDati.Dispose()
                    End Try
                End If

                If lngTipoOperazione = DecEnum.DBOperation.DB_INSERT Then
                    DetailGiri.IDGIRO = -1
                    DetailGiri.CODENTE = -1
                    DetailGiri.Descrizione = ""
                    DetailGiri.CodiceGiro = ""
                    DetailGiri.Note = ""
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBGiri.GetGiri.errore: ", ex)
            End Try
            Return DetailGiri
        End Function
        ''' <summary>
        ''' Inserisce un oggetto
        ''' </summary>
        ''' <param name="myDetail">oggetto di tipo TabelleDiDecodifica.Giri</param>
        ''' <remarks></remarks>
        Public Sub SetGiri(ByVal myDetail As TabelleDiDecodifica.Giri)
            Dim lngTipoOperazione As Long
            Dim sSQL As String = ""
            Dim dvMyDati As New DataView

            lngTipoOperazione = DecEnum.DBOperation.DB_UPDATE
            Try
                Dim oDbManagerRepository As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                If myDetail.IDGIRO = -1 Then
                    lngTipoOperazione = DecEnum.DBOperation.DB_INSERT
                End If

                If lngTipoOperazione = DecEnum.DBOperation.DB_INSERT Then
                    'Verifica esistenza descrizione 
                    Try
                        Using ctx As DBModel = oDbManagerRepository
                            sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"sp_ReturnGiri", "CODENTE", "IDGIRO", "DESCRIZIONE", "NOTE", "GIROEST")
                            dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("CODENTE", myDetail.CODENTE), ctx.GetParam("IDGIRO", -1) _
                                        , ctx.GetParam("DESCRIZIONE", myDetail.Descrizione.Trim) _
                                        , ctx.GetParam("NOTE", "") _
                                        , ctx.GetParam("GIROEST", ""))
                            ctx.Dispose()
                        End Using
                        If Not dvMyDati Is Nothing Then
                            If dvMyDati.Count > 0 Then
                                Throw New Exception("La descrizione Inserita è già presente in tabella!")
                            End If
                        End If
                    Catch ex As Exception
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBGiri.SetGiri.insertdesc.errore: ", ex)
                    Finally
                        dvMyDati.Dispose()
                    End Try

                    Try
                        Using ctx As DBModel = oDbManagerRepository
                            sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"sp_ReturnGiri", "CODENTE", "IDGIRO", "DESCRIZIONE", "NOTE", "GIROEST")
                            dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("CODENTE", myDetail.CODENTE), ctx.GetParam("IDGIRO", myDetail.CodiceGiro) _
                                        , ctx.GetParam("DESCRIZIONE", "") _
                                        , ctx.GetParam("NOTE", "") _
                                        , ctx.GetParam("GIROEST", ""))
                            ctx.Dispose()
                        End Using
                        If Not dvMyDati Is Nothing Then
                            If dvMyDati.Count > 0 Then
                                Throw New Exception("Il Codice Giro inserito è già presente in Tabella !")
                            End If
                        End If
                    Catch ex As Exception
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBGiri.SetGiriinsertest.errore: ", ex)
                    Finally
                        dvMyDati.Dispose()
                    End Try

                    Try
                        Using ctx As DBModel = oDbManagerRepository
                            sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_TP_GIRI_IU", "IDENTE", "DESCRIZIONE", "NOTE", "GIROEST")
                            ctx.ExecuteNonQuery(sSQL, "TBL", ctx.GetParam("IDENTE", myDetail.CODENTE) _
                                        , ctx.GetParam("DESCRIZIONE", myDetail.Descrizione) _
                                        , ctx.GetParam("NOTE", myDetail.Note) _
                                        , ctx.GetParam("GIROEST", myDetail.CodiceGiro)
                                    )
                            ctx.Dispose()
                        End Using
                    Catch ex As Exception
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBGiri.SetGiri.insert.errore: ", ex)
                    End Try
                ElseIf lngTipoOperazione = DecEnum.DBOperation.DB_UPDATE Then
                    Try
                        Using ctx As DBModel = oDbManagerRepository
                            sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_TP_GIRI_D", "IDGIRO")
                            ctx.ExecuteNonQuery(sSQL, "TBL", ctx.GetParam("IDGIRO", myDetail.IDGIRO))
                            ctx.Dispose()
                        End Using
                    Catch ex As Exception
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBGiri.SetGiri.delete.errore: ", ex)
                    End Try

                    'Try
                    '    Using ctx As DBModel = oDbManagerRepository
                    '        sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"sp_ReturnGiri", "CODENTE", "IDGIRO", "DESCRIZIONE", "NOTE", "GIROEST")
                    '        dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("CODENTE", myDetail.CODENTE), ctx.GetParam("IDGIRO", myDetail.IDGIRO) _
                    '                    , ctx.GetParam("DESCRIZIONE", myDetail.Descrizione.Trim) _
                    '                    , ctx.GetParam("NOTE", "") _
                    '                    , ctx.GetParam("GIROEST", ""))
                    '        ctx.Dispose()
                    '    End Using
                    '    If Not dvMyDati Is Nothing Then
                    '        If dvMyDati.Count > 0 Then
                    '            Throw New Exception("La descrizione Inserita è già presente  in tabella !")
                    '        End If
                    '    End If
                    'Catch ex As Exception
                    '    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBGiri.SetGiri.updatequeryins.errore: ", ex)
                    'Finally
                    '    dvMyDati.Dispose()
                    'End Try


                    'Try
                    '    sSQL = "SELECT COD_GIRO_EST"
                    '    sSQL += " FROM TP_GIRI"
                    '    sSQL += " WHERE (COD_GIRO_EST  IS NOT NULL AND COD_GIRO_EST <>'')"
                    '    sSQL += " AND CODENTE=@IDENTE"
                    '    sSQL += " AND COD_GIRO_EST=@GIROEST"
                    '    sSQL += " AND IDGIRO<>@IDGIRO"
                    '    Using ctx As DBModel = oDbManagerRepository
                    '        sSQL = ctx.getsql( sSQL, "IDENTE", "GIROEST", "IDGIRO")
                    '        dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", myDetail.CODENTE) _
                    '                , ctx.GetParam("GIROEST", myDetail.CodiceGiro) _
                    '                , ctx.GetParam("IDGIRO", myDetail.IDGIRO)
                    '            )
                    '        ctx.Dispose()
                    '    End Using
                    '    If Not dvMyDati Is Nothing Then
                    '        If dvMyDati.Count > 0 Then
                    '            Throw New Exception("Il Codice Giro inserito è già presente in Tabella ")
                    '        End If
                    '    End If
                    'Catch ex As Exception
                    '    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBGiri.SetGiri.updatequeryest.errore: ", ex)
                    'Finally
                    '    dvMyDati.Dispose()
                    'End Try

                    Try
                        Using ctx As DBModel = oDbManagerRepository
                            sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_TP_GIRI_IU", "IDENTE", "DESCRIZIONE", "NOTE", "GIROEST")
                            ctx.ExecuteNonQuery(sSQL, "TBL", ctx.GetParam("IDENTE", myDetail.CODENTE) _
                                        , ctx.GetParam("DESCRIZIONE", myDetail.Descrizione) _
                                        , ctx.GetParam("NOTE", myDetail.Note) _
                                        , ctx.GetParam("GIROEST", myDetail.CodiceGiro)
                                    )
                            ctx.Dispose()
                        End Using
                    Catch ex As Exception
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBGiri.SetGiri.update.errore: ", ex)
                    End Try
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBGiri.SetGiri.errore: ", ex)
            End Try
        End Sub
        'Public Sub SetGiri(ByVal myDetail As TabelleDiDecodifica.Giri)

        '    Dim lngTipoOperazione As Long
        '    Dim sSQL As String
        '    Dim rd As SqlDataReader

        '    lngTipoOperazione = _Enum.DBOperation.DB_UPDATE
        '    Try
        '        If myDetail.IDGIRO = -1 Then
        '            lngTipoOperazione = _Enum.DBOperation.DB_INSERT
        '        End If

        '        If lngTipoOperazione = _Enum.DBOperation.DB_INSERT Then
        '            'Verifica esistenza descrizione 

        '            sSQL = ""
        '            sSQL = "SELECT DESCRIZIONE FROM TP_GIRI WHERE DESCRIZIONE=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.Descrizione), "'", "''")) & vbCrLf
        '            sSQL += "AND" & vbCrLf
        '            sSQL += "CODENTE =" & myDetail.CODENTE

        '            rd = iDB.GetDataReader(sSQL)
        '            If rd.Read Then
        '                Throw New Exception("La descrizione Inserita è già presente  in tabella !")
        '            End If
        '            rd.Close()

        '            sSQL = ""
        '            sSQL = "SELECT COD_GIRO_EST FROM TP_GIRI WHERE COD_GIRO_EST=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.CodiceGiro), "'", "''")) & vbCrLf
        '            sSQL += "AND" & vbCrLf
        '            sSQL += "(COD_GIRO_EST IS NOT NULL  AND COD_GIRO_EST <>'')" & vbCrLf
        '            sSQL += "AND" & vbCrLf
        '            sSQL += "CODENTE =" & myDetail.CODENTE


        '            rd = iDB.GetDataReader(sSQL)
        '            If rd.Read Then
        '                Throw New Exception("Il Codice Giro inserito è già presente in Tabella !")
        '            End If
        '            rd.Close()

        '            sSQL = ""
        '            sSQL = "INSERT INTO TP_GIRI"
        '            sSQL += "(CODENTE,DESCRIZIONE,NOTE,COD_GIRO_EST)" & vbCrLf
        '            sSQL += "VALUES ( " & vbCrLf
        '            sSQL += Utility.StringOperation.FormatInt(UCase(myDetail.CODENTE)) & "," & vbCrLf
        '            sSQL += Utility.StringOperation.FormatString(UCase(myDetail.Descrizione)) & "," & vbCrLf
        '            sSQL += Utility.StringOperation.FormatString(UCase(myDetail.Note)) & "," & vbCrLf
        '            sSQL += Utility.StringOperation.FormatString(UCase(myDetail.CodiceGiro)) & vbCrLf
        '            sSQL += " )"
        '            Try
        '                If iDB.ExecuteNonQuery(sSQL) <> 1 Then
        '                    Throw New Exception("errore in::" & sSQL)
        '                End If
        '            Catch ex As SqlException
        '                Throw ex
        '            Catch ex As Exception
        '                Throw ex
        '            End Try

        '        End If

        '        If lngTipoOperazione = _Enum.DBOperation.DB_UPDATE Then

        '            Dim sqlTrans As SqlTransaction
        '            Dim sqlConn As New SqlConnection
        '            Dim sqlCmdInsert As SqlCommand


        '            sqlConn.ConnectionString = ConstSession.StringConnection

        '            sqlConn.Open()
        '            sqlTrans = sqlConn.BeginTransaction
        '            sSQL = "DELETE FROM TP_GIRI WHERE IDGIRO=" & myDetail.IDGIRO & vbCrLf



        '            sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
        '            Try
        '                sqlCmdInsert.ExecuteNonQuery()

        '            Catch ex As SqlException
        '                Select Case ex.Number
        '                    Case 547
        '                        sqlTrans.Rollback()
        '                        sqlConn.Close()
        '                        sqlConn.Dispose()
        '                        sqlCmdInsert.Dispose()
        '                        Throw ex
        '                End Select
        '            End Try
        '            sqlTrans.Rollback()
        '            sqlConn.Close()
        '            sqlConn.Dispose()
        '            sqlCmdInsert.Dispose()

        '            sSQL = ""
        '            sSQL = "SELECT DESCRIZIONE FROM TP_GIRI WHERE DESCRIZIONE=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.Descrizione), "'", "''"))
        '            sSQL += "AND" & vbCrLf
        '            sSQL += "IDGIRO<>" & myDetail.IDGIRO & vbCrLf
        '            sSQL += "AND" & vbCrLf
        '            sSQL += "CODENTE =" & myDetail.CODENTE

        '            rd = iDB.GetDataReader(sSQL)
        '            If rd.Read Then
        '                Throw New Exception("La descrizione Inserita è già presente  in tabella !")
        '            End If
        '            rd.Close()

        '            sSQL = ""
        '            sSQL = "SELECT COD_GIRO_EST FROM TP_GIRI WHERE COD_GIRO_EST=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.CodiceGiro), "'", "''")) & vbCrLf
        '            sSQL += "AND" & vbCrLf
        '            sSQL += "(COD_GIRO_EST  IS NOT NULL  AND COD_GIRO_EST <>'')" & vbCrLf
        '            sSQL += "AND" & vbCrLf
        '            sSQL += "IDGIRO<>" & myDetail.IDGIRO & vbCrLf
        '            sSQL += "AND" & vbCrLf
        '            sSQL += "CODENTE =" & myDetail.CODENTE


        '            rd = iDB.GetDataReader(sSQL)
        '            If rd.Read Then
        '                Throw New Exception("Il Codice Giro inserito è già presente in Tabella !")
        '            End If
        '            rd.Close()

        '            sSQL = "UPDATE TP_GIRI SET "
        '            sSQL += "CODENTE =" & Utility.StringOperation.FormatInt(myDetail.CODENTE) & vbCrLf
        '            sSQL += ",DESCRIZIONE =" & Utility.StringOperation.FormatString(UCase(myDetail.Descrizione)) & vbCrLf
        '            sSQL += ",NOTE=" & Utility.StringOperation.FormatString(UCase(myDetail.Note)) & vbCrLf
        '            sSQL += ",COD_GIRO_EST=" & Utility.StringOperation.FormatString(UCase(myDetail.CodiceGiro)) & vbCrLf
        '            sSQL += "WHERE" & vbCrLf
        '            sSQL += "IDGIRO=" & myDetail.IDGIRO

        '            Try
        '                If iDB.ExecuteNonQuery(sSQL) <> 1 Then
        '                    Throw New Exception("errore in::" & sSQL)
        '                End If
        '            Catch ex As SqlException
        '                Throw ex
        '            Catch ex As Exception
        '                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBGiri.SetGiri.errore: ", ex)
        '                Throw ex
        '            End Try

        '        End If
        '    Catch ex As Exception
        '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBGiri.SetGiri.errore: ", ex)
        '    End Try
        'End Sub
        ''' <summary>
        ''' Elimina un oggetto
        ''' </summary>
        ''' <param name="IDGIRO">intero</param>
        ''' <remarks></remarks>
        Public Sub EliminaGiro(ByVal IDGIRO As Integer)
            Try
                Dim sSQL As String = ""
                Try
                    Dim oDbManagerRepository As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                    Using ctx As DBModel = oDbManagerRepository
                        sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_TP_GIRI_D", "IDGIRO")
                        ctx.ExecuteNonQuery(sSQL, "TBL", ctx.GetParam("IDGIRO", IDGIRO))
                        ctx.Dispose()
                    End Using
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBGiri.EliminaGiro.query.errore:", ex)
                End Try
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBGiri.EliminaGiro.errore: ", ex)
                Throw ex
            End Try
        End Sub
    End Class
End Namespace
