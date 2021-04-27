Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports log4net
Imports Utility

Namespace TabelleDiDecodifica
    Public Class DBPeriodo
        Inherits TabelleDiDecodifica.DetailPeriodo
        Private Shared Log As ILog = LogManager.GetLogger(GetType(DBPeriodo))
        ''' <summary>
        ''' Gestione dei periodo di fatturazione
        ''' </summary>
        ''' <remarks></remarks>
        Dim iDB As New DBAccess.getDBobject()
        Dim ModDate As New ClsGenerale.Generale
        Dim _Const As New Costanti


        ''' <summary>
        ''' Estrae l'elenco degli oggetti configurati
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <revisionHistory>
        ''' <revision date="12/04/2019">
        ''' Modifiche da revisione manuale
        ''' </revision>
        ''' </revisionHistory>
        Public Function GetListaPeriodi() As DataView
            Dim sSQL As String = ""
            Dim dvMyDati As New DataView
            Try
                Dim oDbManagerRepository As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                Using ctx As DBModel = oDbManagerRepository
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"sp_ReturnPeriodi", "CODENTE")
                    dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("CODENTE", ConstSession.IdEnte))
                    ctx.Dispose()
                End Using
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBPeriodo.GetListaPeriodi.errore: ", ex)
                Throw New Exception("GetListaPeriodi[DBPeriodo]." & "Errore caricamento tabella Periodi")
            End Try
            Return dvMyDati
        End Function
        'Public Function GetListaPeriodi() As DataView
        '    Dim cmdMyCommand As New SqlCommand
        '    Dim myDv As DataView = Nothing
        '    Try
        '        cmdMyCommand.CommandType = CommandType.StoredProcedure
        '        cmdMyCommand.CommandText = "sp_ReturnPeriodi"
        '        cmdMyCommand.Parameters.Add(New SqlParameter("@CodEnte", ConstSession.IdEnte))
        '        myDv = iDB.GetDataView(cmdMyCommand)
        '    Catch ex As Exception
        '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBPeriodo.GetListaPeriodi.errore: ", ex)
        '        Throw New Exception("GetListaPeriodi[DBPeriodo]." & "Errore caricamento tabella Periodi")
        '    End Try
        '    Return myDv
        'End Function
        'Public Function GetListaPeriodi() As GetLista

        '    Try
        '        Dim GetLista As New GetLista

        '        Dim oConn As New SqlConnection
        '        Dim oCmd As New SqlCommand
        '        Dim oPar As New SqlParameter

        '        oPar.ParameterName = "@CodEnte"
        '        oPar.Value = ConstSession.IdEnte

        '        GetLista.lngRecodvMyDatiCount = iDB.RunSPReturnToGrid("sp_ReturnPeriodi", oConn, oCmd, oPar)

        '        GetLista.oConn = oConn
        '        GetLista.oComm = oCmd


        '        Return GetLista
        '    Catch ex As Exception
        '  Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBPeriodo.GetPeriodo.errore: ", ex)
        '        Throw New Exception("GetListaPeriodi[DBPeriodo]." & "Errore caricamento tabella Periodi")
        '    End Try

        'End Function
        ''' <summary>
        ''' Estrae l'oggetto in base al codice passato
        ''' </summary>
        ''' <param name="CODPERIODO">intero</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetPeriodo(ByVal CODPERIODO As Integer) As TabelleDiDecodifica.DetailPeriodo
            Dim lngTipoOperazione As Long
            Dim oMyPeriodo As New TabelleDiDecodifica.DetailPeriodo
            Try
                If CODPERIODO = -1 Then lngTipoOperazione = DecEnum.DBOperation.DB_INSERT

                If lngTipoOperazione = DecEnum.DBOperation.DB_UPDATE Then
                    Dim sSQL As String = ""
                    Dim dvMyDati As New DataView
                    Try
                        Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                            sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetPeriodo", "IDENTE", "PERIODO", "CODPERIODO")
                            dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", ConstSession.IdEnte) _
                            , ctx.GetParam("PERIODO", "") _
                            , ctx.GetParam("CODPERIODO", CODPERIODO)
                        )
                            ctx.Dispose()
                        End Using
                        If Not dvMyDati Is Nothing Then
                            For Each myRow As DataRowView In dvMyDati
                                '*** 20121213 - per nuova gestione analisi scaglioni ***
                                oMyPeriodo.CodPeriodo = CODPERIODO
                                '*** ***
                                oMyPeriodo.Periodo = Utility.StringOperation.FormatString(myRow("PERIODO"))
                                oMyPeriodo.DaData = ModDate.GiraDataFromDB(Utility.StringOperation.FormatString(myRow("DADATA")))
                                oMyPeriodo.AData = ModDate.GiraDataFromDB(Utility.StringOperation.FormatString(myRow("ADATA")))
                                oMyPeriodo.Storico = Utility.StringOperation.FormatBool(myRow("STORICO"))
                                oMyPeriodo.Attuale = Utility.StringOperation.FormatBool(myRow("ATTUALE"))
                                oMyPeriodo.Note = Utility.StringOperation.FormatString(myRow("NOTE"))
                                oMyPeriodo.nTipoArrotondamentoConsumo = Utility.StringOperation.FormatInt(myRow("tipo_arrotondamento_consumo"))
                            Next
                        End If
                    Catch ex As Exception
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBPeriodo.GetPeriodo.Update.errore: ", ex)
                    Finally
                        dvMyDati.Dispose()
                    End Try
                ElseIf lngTipoOperazione = DecEnum.DBOperation.DB_INSERT Then
                    oMyPeriodo.CodPeriodo = 0
                    oMyPeriodo.Periodo = ""
                    oMyPeriodo.DaData = ""
                    oMyPeriodo.AData = ""
                    oMyPeriodo.Note = ""
                    oMyPeriodo.Storico = False
                    oMyPeriodo.Attuale = False
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBPeriodo.GetPeriodo.errore: ", ex)
                oMyPeriodo = Nothing
            End Try
            Return oMyPeriodo
        End Function
        ''' <summary>
        ''' Estrae la tipologia di arrotondamento da applicare al consumo
        ''' </summary>
        ''' <param name="nPeriodo">intero</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetArrotondamentoConsumo(ByVal nPeriodo As Integer) As Integer
            Dim sSQL As String
            Dim dvMyDati As New DataView
            Dim nTipoArrotond As Integer = -1

            Try
                sSQL = "SELECT TP_PERIODO.TIPO_ARROTONDAMENTO_CONSUMO"
                sSQL += " FROM TP_PERIODO"
                sSQL += " WHERE (TP_PERIODO.CODPERIODO=" & nPeriodo & ")"
                'eseguo la query
                dvMyDati = iDB.GetDataView(sSQL)
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        nTipoArrotond = StringOperation.FormatInt(myRow("tipo_arrotondamento_consumo"))
                    Next
                End If
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBPeriodo.GetArrotondamentoConsumo.errore: ", Err)
            Finally
                dvMyDati.Dispose()
            End Try
            Return nTipoArrotond
        End Function
        'Public Function GetArrotondamentoConsumo(ByVal nPeriodo As Integer, ByVal WFSessione As OPENUtility.CreateSessione) As Integer
        '    dim sSQL as string
        '    Dim dvMyDati As SqlClient.new dataview
        '    Dim nTipoArrotond As Integer = -1

        '    Try
        '        sSQL = "SELECT TP_PERIODO.TIPO_ARROTONDAMENTO_CONSUMO"
        '        sSQL += " FROM TP_PERIODO"
        '        sSQL += " WHERE (TP_PERIODO.CODPERIODO=" & nPeriodo & ")"
        '        'eseguo la query
        '        dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
        '        Do While dvMyDati.Read
        '            If Not IsDBNull(dvMyDati("tipo_arrotondamento_consumo")) Then
        '                nTipoArrotond = StringOperation.FormatInt(myrow("tipo_arrotondamento_consumo"))
        '            End If
        '        Loop
        '    Catch Err As Exception
        '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBPeriodo.GetArrotondamentoConsumo.errore: ", ex)
        '    Finally
        '        dvmydati.dispose()
        '    End Try
        '    Return nTipoArrotond
        'End Function

        ''' <summary>
        ''' Inserisce un oggetto
        ''' </summary>
        ''' <param name="myItem">oggetto di tipo TabelleDiDecodifica.DetailPeriodo</param>
        ''' <param name="sMessaggioPeriodo">stringa</param>
        ''' <remarks></remarks>
        Public Sub SetPeriodo(ByVal myItem As TabelleDiDecodifica.DetailPeriodo, ByRef sMessaggioPeriodo As String)
            Dim lngTipoOperazione As Long
            Dim sSQL As String = ""
            Dim dvMyDati As New DataView

            lngTipoOperazione = DecEnum.DBOperation.DB_UPDATE
            Try
                Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                    If myItem.CodPeriodo <= 0 Then
                        lngTipoOperazione = DecEnum.DBOperation.DB_INSERT
                    End If

                    If lngTipoOperazione = DecEnum.DBOperation.DB_INSERT Then
                        'Verifica esistenza descrizione 
                        Try
                            sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetPeriodo", "IDENTE", "PERIODO", "CODPERIODO")
                            dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", ConstSession.IdEnte) _
                                    , ctx.GetParam("PERIODO", Replace(Trim(myItem.Periodo), ") '", "''")) _
                                    , ctx.GetParam("CODPERIODO", myItem.CodPeriodo)
                                )
                            If Not dvMyDati Is Nothing Then
                                If dvMyDati.Count > 0 Then
                                    sMessaggioPeriodo = "GestAlert('a', 'warning', '', '', 'Il Perido inserito e\' gia\' presente in Tabella!');"
                                    Exit Sub
                                End If
                            End If
                        Catch ex As Exception
                            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBPeriodo.SetPeriodo.Insert.errore: ", ex)
                        Finally
                            dvMyDati.Dispose()
                        End Try
                    End If
                    'modifica del 13/02/2007
                    'aggiornamento anche del campo PERIODOESTESO
                    Try
                        sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_TP_PERIODO_IU", "CODPERIODO", "PERIODO", "DADATA", "ADATA", "NOTE", "STORICO", "ATTUALE", "PERIODOESTESO", "IDENTE", "TIPOARROTONDAMENTOCONSUMO")
                        dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("CODPERIODO", myItem.CodPeriodo) _
                                , ctx.GetParam("PERIODO", myItem.Periodo) _
                                , ctx.GetParam("DADATA", ModDate.GiraData(myItem.DaData)) _
                                , ctx.GetParam("ADATA", ModDate.GiraData(myItem.AData)) _
                                , ctx.GetParam("NOTE", myItem.Note) _
                                , ctx.GetParam("STORICO", myItem.Storico) _
                                , ctx.GetParam("ATTUALE", myItem.Attuale) _
                                , ctx.GetParam("PERIODOESTESO", myItem.Periodo.ToUpper.Replace("/", "")) _
                                , ctx.GetParam("IDENTE", ConstSession.IdEnte) _
                                , ctx.GetParam("TIPOARROTONDAMENTOCONSUMO", myItem.nTipoArrotondamentoConsumo)
                            )
                        If Not dvMyDati Is Nothing Then
                            For Each myRow As DataRowView In dvMyDati
                                If Utility.StringOperation.FormatInt(myRow("codperiodo") <= 0) Then
                                    sMessaggioPeriodo = "GestAlert('a', 'danger', '', '', 'Errore in inserimento Periodo!');"
                                End If
                            Next
                        End If
                    Catch ex As Exception
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBPeriodo.GetPeriodo.IU.errore: ", ex)
                    Finally
                        dvMyDati.Dispose()
                    End Try

                    'modifica del 13/02/2007
                    'se il periodo che si sta andando a modificare ha come ATTUALE 1 tutti gli altri periodi di quell'ente devono avere ATTUALE a 0 devo estrarre il periodo di fatturazione attivo per vedere se è variato  e per valorizzare in maniera corretta la  variabile di sessione
                    HttpContext.Current.Session("PERIODOID") = -1
                    HttpContext.Current.Session("PERIODO") = ""
                    Try
                        sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetPeriodo", "IDENTE", "PERIODO", "CODPERIODO")
                        dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", ConstSession.IdEnte) _
                            , ctx.GetParam("PERIODO", Replace(Trim(myItem.Periodo), ") '", "''")) _
                            , ctx.GetParam("CODPERIODO", myItem.CodPeriodo)
                        )
                        If Not dvMyDati Is Nothing Then
                            For Each myRow As DataRowView In dvMyDati
                                HttpContext.Current.Session("PERIODOID") = Utility.StringOperation.FormatInt(myRow("codperiodo"))
                                HttpContext.Current.Session("PERIODO") = Utility.StringOperation.FormatString(myRow("periodo"))
                            Next
                        End If
                    Catch ex As Exception
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBPeriodo.GetPeriodo.Update.errore: ", ex)
                    Finally
                        dvMyDati.Dispose()
                    End Try
                    If ConstSession.IdPeriodo <= 0 Then
                        'è necessario dare un alert all'utente che deve sempre esserci un periodo di fatturazione con ATTUALE =1
                        sMessaggioPeriodo = "GestAlert('a', 'warning', '', '', 'Non e\' attivo alcun periodo di fatturazione. Impostare il periodo di fatturazione!');"
                    End If
                    ctx.Dispose()
                End Using
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBPeriodo.SetPERIODO.errore: ", ex)
            End Try
        End Sub
        ''' <summary>
        ''' Elimina un oggetto
        ''' </summary>
        ''' <param name="CODPERIODO">intero</param>
        ''' <remarks></remarks>
        Public Sub EliminaPERIODO(ByVal CODPERIODO As Integer)
            Dim sSQL As String
            Try
                sSQL = "DELETE FROM TP_PERIODO WHERE CODPERIODO=" & CODPERIODO
                If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                    Throw New Exception("errore in::" & sSQL)
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBPeriodo.EliminaPERIODO.errore: ", ex)
                Throw ex
            End Try
        End Sub
        ''' <summary>
        ''' Modifica un oggetto
        ''' </summary>
        ''' <param name="myDetail">oggetto di tipo TabelleDiDecodifica.DetailPeriodo</param>
        ''' <remarks></remarks>
        Public Sub UpdateForzato(ByVal myDetail As TabelleDiDecodifica.DetailPeriodo)
            Dim sSQL As String
            Dim dvMyDati As New DataView

            sSQL = "SELECT TP_PERIODO.PERIODO"
            sSQL += " FROM TP_PERIODO"
            sSQL += " WHERE (TP_PERIODO.PERIODO = " & Utility.StringOperation.FormatString(Replace(Trim(myDetail.Periodo), ") '", "''")) & ")"
            sSQL += " AND (TP_PERIODO.CODPERIODO <> " & myDetail.CodPeriodo & ")"
            sSQL += " AND (TP_PERIODO.COD_ENTE = " & ConstSession.IdEnte & ")"
            dvMyDati = iDB.GetDataView(sSQL)
            Try
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        Throw New Exception("Il Codice Anomalia inserito è già presente in Tabella !")
                    Next
                End If
                dvMyDati.Dispose()

                sSQL = "UPDATE TP_PERIODO SET "
                sSQL += "PERIODO =" & Utility.StringOperation.FormatString(UCase(myDetail.Periodo))
                sSQL += ",DADATA =" & Utility.StringOperation.FormatString(ModDate.GiraData(myDetail.DaData))
                sSQL += ",ADATA =" & Utility.StringOperation.FormatString(ModDate.GiraData(myDetail.AData))
                sSQL += ",NOTE=" & Utility.StringOperation.FormatString(UCase(myDetail.Note))
                sSQL += ",STORICO=" & Utility.StringOperation.FormatBool(UCase(myDetail.Storico))
                sSQL += ",ATTUALE=" & Utility.StringOperation.FormatBool(UCase(myDetail.Attuale))
                '************************************************
                'modifica del 13/02/2007
                'aggiornamento anche del campo PERIODOESTESO
                sSQL += ",PERIODOESTESO=" & Utility.StringOperation.FormatString(UCase(myDetail.Periodo)).Replace("/", "")
                '************************************************
                sSQL += ",TIPO_ARROTONDAMENTO_CONSUMO=" & myDetail.nTipoArrotondamentoConsumo
                sSQL += " WHERE (CODPERIODO <>" & myDetail.CodPeriodo & ")"
                sSQL += " AND (COD_ENTE = " & ConstSession.IdEnte & ")"
                Try
                    If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                        Throw New Exception("errore in::" & sSQL)
                    End If
                Catch ex As SqlException
                    Throw ex
                Catch ex As Exception
                    Throw ex
                End Try
                'modifica del 13/02/2007
                'se il periodo che si sta andando a modificare ha come ATTUALE 1 tutti gli altri periodi di quell'ente devono avere
                'ATTUALE a 0
                If Utility.StringOperation.FormatBool(UCase(myDetail.Attuale)) = 1 Then
                    sSQL = "UPDATE TP_PERIODO SET "
                    sSQL += "STORICO = 1, "
                    sSQL += "ATTUALE = 0 "
                    sSQL += "WHERE (CODPERIODO=" & myDetail.CodPeriodo & ")"
                End If

                Try
                    If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                        Throw New Exception("errore in::" & sSQL)
                    End If
                Catch ex As SqlException
                    Throw ex
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBPeriodo.UpdateForzato.errore: ", ex)
                    Throw ex
                End Try
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBPeriodo.UpdateForzato.errore: ", ex)
            End Try
        End Sub

    End Class
End Namespace
