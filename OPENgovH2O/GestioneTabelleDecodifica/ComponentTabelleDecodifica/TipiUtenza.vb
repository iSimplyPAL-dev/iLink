Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports log4net
Imports Utility

Namespace TabelleDiDecodifica
    Public Class DBTipiUtenza
        Inherits TabelleDiDecodifica.DetailTipiUtenza
        ''' <summary>
        ''' Gestione delle tipologie di utenze
        ''' </summary>
        ''' <remarks></remarks>
        Private iDB As New DBAccess.getDBobject()
        Private _Const As New Costanti
        Private ModDate As New ClsGenerale.Generale
        Private Shared Log As ILog = LogManager.GetLogger(GetType(DBTipiUtenza))

        ''' <summary>
        ''' Estrae l'elenco degli oggetti configurati
        ''' </summary>
        ''' <param name="sIdEnte">stringa</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <revisionHistory>
        ''' <revision date="12/04/2019">
        ''' Modifiche da revisione manuale
        ''' </revision>
        ''' </revisionHistory>
        Public Function GetListaTipiUtenza(ByVal sIdEnte As String, IdTipoUtenza As Integer, IDContatore As Integer) As DataView
            Dim sSQL As String
            Dim dvMyDati As DataView = Nothing

            Try
                Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"sp_ReturnTipiUtenza", "COD_ENTE", "IDTIPOUTENZA", "CODCONTRATTO")
                    dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("COD_ENTE", sIdEnte) _
                                , ctx.GetParam("IDTIPOUTENZA", IdTipoUtenza) _
                                , ctx.GetParam("CODCONTRATTO", IDContatore)
                            )
                    ctx.Dispose()
                End Using
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBtipiUtenza.GetListaTipiUtenza.errore: ", ex)
                Throw New Exception("GetListaTipiUtenza[DBTipiUtenza]." & "Errore caricamento Tipi Utenza")
            End Try
            Return dvMyDati
        End Function
        'Public Function GetListaTipiUtenza(ByVal sIdEnte As String, IdTipoUtenza As Integer) As DataView
        '    Dim cmdMyCommand As New SqlCommand
        '    Dim myDv As DataView = Nothing
        '    Try
        '        cmdMyCommand.CommandType = CommandType.StoredProcedure
        '        cmdMyCommand.CommandText = "sp_ReturnTipiUtenza"
        '        cmdMyCommand.Parameters.Add(New SqlParameter("@COD_ENTE", sIdEnte))
        '        cmdMyCommand.Parameters.Add(New SqlParameter("@IDTIPOUTENZA", IdTipoUtenza))

        '        myDv = iDB.GetDataView(cmdMyCommand)
        '    Catch ex As Exception
        '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBtipiUtenza.GetListaTipiUtenza.errore: ", ex)
        '        Throw New Exception("GetListaTipiUtenza[DBTipiUtenza]." & "Errore caricamento Tipi Utenza")
        '    End Try
        '    Return myDv
        'End Function
        'Public Function GetListaTipiUtenza(ByVal sIdEnte As String) As GetLista
        '    Try
        '        Dim GetLista As New GetLista
        '        Dim oConn As New SqlConnection
        '        Dim oCmd As New SqlCommand

        '        GetLista.lngRecodvMyDatiCount = iDB.RunSPReturnToGrid("sp_ReturnTipiUtenza", oConn, oCmd, New SqlParameter("@COD_ENTE", sIdEnte))
        '        GetLista.oConn = oConn
        '        GetLista.oComm = oCmd
        '        Return GetLista
        '    Catch ex As Exception
        'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBtipiUtenza.GetListaTipiUtenza.errore: ", ex)
        '        Throw New Exception("GetListaTipiUtenza[DBTipiUtenza]." & "Errore caricamento Tipi Utenza")
        '    End Try
        'End Function
        ''' <summary>
        ''' Estrae l'oggetto in base al codice passato
        ''' </summary>
        ''' <param name="IDTIPOUTENZA">intero</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetTipoUtenza(ByVal IDTIPOUTENZA As Integer) As TabelleDiDecodifica.DetailTipiUtenza

            Dim lngTipoOperazione As Long
            Dim sSQL As String

            Dim DetailTipiUtenza As New TabelleDiDecodifica.DetailTipiUtenza
            Try
                If IDTIPOUTENZA = -1 Then lngTipoOperazione = DecEnum.DBOperation.DB_INSERT

                If lngTipoOperazione = DecEnum.DBOperation.DB_UPDATE Then
                    Dim dvMyDati As New DataView
                    sSQL = ""
                    sSQL = "SELECT * FROM TP_TIPIUTENZA WHERE IDTIPOUTENZA = " & Utility.StringOperation.FormatString(IDTIPOUTENZA)
                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            DetailTipiUtenza.Descrizione = Utility.StringOperation.FormatString(myRow("DESCRIZIONE"))
                            DetailTipiUtenza.CodiceEsterno = Utility.StringOperation.FormatString(myRow("CODICE_EST"))
                            DetailTipiUtenza.ConsumoMinimoAnnuo = Utility.StringOperation.FormatString(myRow("CONSUMOMINIMOANNUO"))
                            DetailTipiUtenza.SogliaPositiva = Utility.StringOperation.FormatString(myRow("SOGLIAPOSITIVA"))
                            DetailTipiUtenza.SogliaNegativa = Utility.StringOperation.FormatString(myRow("SOGLIANEGATIVA"))
                            DetailTipiUtenza.Note = Utility.StringOperation.FormatString(myRow("NOTE"))
                            DetailTipiUtenza.Dal = DateTime.Parse(Utility.StringOperation.FormatString(myRow("DAL"))).ToShortDateString
                            DetailTipiUtenza.Al = DateTime.Parse(Utility.StringOperation.FormatString(myRow("AL"))).ToShortDateString
                        Next
                    End If
                    dvMyDati.Dispose()
                End If

                If lngTipoOperazione = DecEnum.DBOperation.DB_INSERT Then
                    DetailTipiUtenza.IDTipiUtenza = -1
                    DetailTipiUtenza.CodiceEsterno = ""
                    DetailTipiUtenza.Descrizione = ""
                    DetailTipiUtenza.ConsumoMinimoAnnuo = ""
                    DetailTipiUtenza.SogliaPositiva = ""
                    DetailTipiUtenza.SogliaNegativa = ""
                    DetailTipiUtenza.Note = ""
                    DetailTipiUtenza.Dal = ""
                    DetailTipiUtenza.Al = ""
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBtipiUtenza.GetTipoUtenza.errore: ", ex)
            End Try
            Return DetailTipiUtenza
        End Function
        ''' <summary>
        ''' Inserisce un oggetto
        ''' </summary>
        ''' <param name="myDetail">oggetto di tipo TabelleDiDecodifica.DetailTipiUtenza</param>
        ''' <param name="sIdEnte">stringa</param>
        ''' <remarks></remarks>
        Public Sub SetTipiUtenza(ByVal myDetail As TabelleDiDecodifica.DetailTipiUtenza, ByVal sIdEnte As String)

            Dim lngTipoOperazione As Long
            Dim sSQL As String
            Dim dvMyDati As New DataView

            lngTipoOperazione = DecEnum.DBOperation.DB_UPDATE
            Try
                If myDetail.IDTipiUtenza = -1 Then
                    lngTipoOperazione = DecEnum.DBOperation.DB_INSERT
                End If

                If lngTipoOperazione = DecEnum.DBOperation.DB_INSERT Then
                    ''''Verifica esistenza descrizione 
                    sSQL = "SELECT DESCRIZIONE FROM TP_TIPIUTENZA WHERE COD_ENTE='" & sIdEnte & "' AND DESCRIZIONE='" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.Descrizione), "'", "''")) & "'"
                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            Throw New Exception("La descrizione Inserita è già presente  in tabella !")
                        Next
                    End If
                    dvMyDati.Dispose()

                    sSQL = "SELECT CODICE_EST FROM TP_TIPIUTENZA WHERE CODICE_EST='" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.CodiceEsterno), "'", "''")) & "'"
                    sSQL += " AND (CODICE_EST IS NOT NULL  AND CODICE_EST <>'')"
                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            Throw New Exception("Il Codice Tariffa  inserito è già presente in Tabella !")
                        Next
                    End If
                    dvMyDati.Dispose()

                    sSQL = "INSERT INTO TP_TIPIUTENZA"
                    sSQL += "(CODICE_EST,DESCRIZIONE,CONSUMOMINIMOANNUO,SOGLIAPOSITIVA,SOGLIANEGATIVA,COD_ENTE,NOTE,DAL,AL)"
                    sSQL += " VALUES ('" & Utility.StringOperation.FormatString(UCase(myDetail.CodiceEsterno)) & "','"
                    sSQL += Utility.StringOperation.FormatString(UCase(myDetail.Descrizione)) & "','"
                    sSQL += Utility.StringOperation.FormatString(UCase(myDetail.ConsumoMinimoAnnuo)) & "','"
                    sSQL += Utility.StringOperation.FormatString(UCase(myDetail.SogliaPositiva)) & "','"
                    sSQL += Utility.StringOperation.FormatString(UCase(myDetail.SogliaNegativa)) & "','"
                    sSQL += sIdEnte & "','"
                    sSQL += Utility.StringOperation.FormatString(UCase(myDetail.Note))
                    If myDetail.Dal <> "" Then
                        sSQL += "','" & ModDate.GiraData(CType(myDetail.Dal, Date)) & "'"
                    Else
                        sSQL += ", NULL"
                    End If
                    If myDetail.Al <> "" Then
                        sSQL += ",'" & ModDate.GiraData(CType(myDetail.Al, Date)) & "'"
                    Else
                        sSQL += ", NULL"
                    End If
                    sSQL += " )"
                    Try
                        If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                            Throw New Exception("errore in::" & sSQL)
                        End If
                    Catch ex As SqlException
                        Throw ex
                    Catch ex As Exception
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBtipiUtenza.SetTipiUtenza.errore: ", ex)
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
                    sSQL = "DELETE FROM TP_TIPIUTENZA WHERE IDTIPOUTENZA=" & myDetail.IDTipiUtenza
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

                    sSQL = "SELECT DESCRIZIONE"
                    sSQL += " FROM TP_TIPIUTENZA"
                    sSQL += " WHERE (DESCRIZIONE = '" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.Descrizione), ") '", "''")) & "')"
                    sSQL += " AND (IDTIPOUTENZA <>" & myDetail.IDTipiUtenza & ")"
                    sSQL += " AND (COD_ENTE='" & sIdEnte & "')"
                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            Throw New Exception("La descrizione Inserita è già presente  in tabella !")
                        Next
                    End If
                    dvMyDati.Dispose()

                    sSQL = "SELECT CODICE_EST FROM TP_TIPIUTENZA WHERE CODICE_EST='" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.CodiceEsterno), "'", "''")) & "'"
                    sSQL += " AND (CODICE_EST IS NOT NULL  AND CODICE_EST <>'')"
                    sSQL += " AND IDTIPOUTENZA <>" & myDetail.IDTipiUtenza
                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            Throw New Exception("Il Codice Tariffa  inserito è già presente in Tabella !")
                        Next
                    End If
                    dvMyDati.Dispose()

                    sSQL = "UPDATE TP_TIPIUTENZA SET "
                    sSQL += "DESCRIZIONE ='" & Utility.StringOperation.FormatString(UCase(myDetail.Descrizione)) & "'"
                    sSQL += ",CODICE_EST ='" & Utility.StringOperation.FormatString(UCase(myDetail.CodiceEsterno)) & "'"
                    sSQL += ",CONSUMOMINIMOANNUO ='" & Utility.StringOperation.FormatString(UCase(myDetail.ConsumoMinimoAnnuo)) & "'"
                    sSQL += ",SOGLIAPOSITIVA ='" & Utility.StringOperation.FormatString(UCase(myDetail.SogliaPositiva)) & "'"
                    sSQL += ",SOGLIANEGATIVA '=" & Utility.StringOperation.FormatString(UCase(myDetail.SogliaPositiva)) & "'"
                    sSQL += ",NOTE='" & Utility.StringOperation.FormatString(UCase(myDetail.Note)) & "'"
                    If myDetail.Dal <> "" Then
                        sSQL += ",DAL='" & ModDate.GiraData(CType(myDetail.Dal, Date)) & "'"
                    Else
                        sSQL += ", DAL=NULL"
                    End If
                    If myDetail.Al <> "" Then
                        sSQL += ",AL='" & ModDate.GiraData(CType(myDetail.Al, Date)) & "'"
                    Else
                        sSQL += ", AL=NULL"
                    End If
                    sSQL += " WHERE IDTIPOUTENZA=" & myDetail.IDTipiUtenza
                    Try
                        If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                            Throw New Exception("errore in::" & sSQL)
                        End If
                    Catch ex As SqlException
                        Throw ex
                    Catch ex As Exception
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBtipiUtenza.SetTipiUtenza.errore: ", ex)
                        Throw ex
                    End Try
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBtipiUtenza.SetTipiUtenza.errore: ", ex)
            End Try
        End Sub
        ''' <summary>
        ''' Modifica un oggetto
        ''' </summary>
        ''' <param name="myDetail">oggetto di tipo TabelleDiDecodifica.DetailTipiUtenza</param>
        ''' <remarks></remarks>
        Public Sub UpdateForzatoTipiUtenze(ByVal myDetail As TabelleDiDecodifica.DetailTipiUtenza)
            Dim sSQL As String
            Dim dvMyDati As New DataView
            sSQL = ""
            sSQL = "SELECT DESCRIZIONE FROM TP_TIPIUTENZA WHERE DESCRIZIONE=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.Descrizione), "'", "''")) & vbCrLf
            sSQL += "AND" & vbCrLf
            sSQL += "IDTIPOUTENZA <>" & myDetail.IDTipiUtenza

            dvMyDati = iDB.GetDataView(sSQL)
            Try
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        Throw New Exception("La descrizione Inserita è già presente  in tabella !")
                    Next
                End If
                dvMyDati.Dispose()

                sSQL = ""
                sSQL = "SELECT CODICE_EST FROM TP_TIPIUTENZA WHERE CODICE_EST=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.CodiceEsterno), "'", "''")) & vbCrLf
                sSQL += "AND" & vbCrLf
                sSQL += "(CODICE_EST IS NOT NULL  AND CODICE_EST <>'')" & vbCrLf
                sSQL += "AND" & vbCrLf
                sSQL += "IDTIPOUTENZA <>" & myDetail.IDTipiUtenza

                dvMyDati = iDB.GetDataView(sSQL)
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        Throw New Exception("Il Codice Tariffa  inserito è già presente in Tabella !")
                    Next
                End If
                dvMyDati.Dispose()

                sSQL = "UPDATE TP_TIPIUTENZA SET "
                sSQL += "DESCRIZIONE =" & Utility.StringOperation.FormatString(UCase(myDetail.Descrizione)) & vbCrLf
                sSQL += ",CODICE_EST =" & Utility.StringOperation.FormatString(UCase(myDetail.CodiceEsterno)) & vbCrLf
                sSQL += ",CONSUMOMINIMOANNUO =" & Utility.StringOperation.FormatString(UCase(myDetail.ConsumoMinimoAnnuo)) & vbCrLf
                sSQL += ",SOGLIAPOSITIVA =" & Utility.StringOperation.FormatString(UCase(myDetail.SogliaPositiva)) & vbCrLf
                sSQL += ",SOGLIANEGATIVA =" & Utility.StringOperation.FormatString(UCase(myDetail.SogliaPositiva)) & vbCrLf
                sSQL += ",NOTE=" & Utility.StringOperation.FormatString(UCase(myDetail.Note)) & vbCrLf
                sSQL += "WHERE" & vbCrLf
                sSQL += "IDTIPOUTENZA=" & myDetail.IDTipiUtenza

                Try
                    If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                        Throw New Exception("errore in::" & sSQL)
                    End If
                Catch ex As SqlException
                    Throw ex
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBtipiUtenza.UpdateForzatoTipiUtenze.errore: ", ex)
                    Throw ex
                End Try
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBtipiUtenza.UpdateForzatoTipiUtenze.errore: ", ex)
            End Try
        End Sub
        ''' <summary>
        ''' Elimina un oggetto
        ''' </summary>
        ''' <param name="IDTIPOUTENZA">intero</param>
        ''' <remarks></remarks>
        Public Sub EliminaTipoUtenza(ByVal IDTIPOUTENZA As Integer)
            Dim sSQL As String
            Try
                sSQL = "DELETE FROM TP_TIPIUTENZA WHERE IDTIPOUTENZA=" & IDTIPOUTENZA
                If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                    Throw New Exception("errore in::" & sSQL)
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBtipiUtenza.EliminaTipoUtenza.errore: ", ex)
                Throw ex
            End Try
        End Sub
    End Class
End Namespace