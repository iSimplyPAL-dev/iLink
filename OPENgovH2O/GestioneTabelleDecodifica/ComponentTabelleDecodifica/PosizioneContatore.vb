Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports log4net

Namespace TabelleDiDecodifica
    Public Class DBPosizioneContatore
        Inherits TabelleDiDecodifica.PosizioneContatore
        ''' <summary>
        ''' Gestione posizione fisica del contatore
        ''' </summary>
        ''' <remarks></remarks>
        Dim iDB As New DBAccess.getDBobject()
        Dim _Const As New Costanti()
        Private Shared Log As ILog = LogManager.GetLogger(GetType(DBPosizioneContatore))

        ''' <summary>
        ''' Estrae l'elenco degli oggetti configurati
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetListaPosizioneContatore() As DataView
            Dim cmdMyCommand As New SqlCommand
            Dim myDv As DataView = Nothing
            Try
                cmdMyCommand.CommandType = CommandType.StoredProcedure
                cmdMyCommand.CommandText = "sp_ReturnPosizioneContatore"

                myDv = iDB.GetDataView(cmdMyCommand)
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBPosizioneContatore.GetListaPosizioneContatore.errore: ", ex)
                Throw New Exception("GetListaPosizioneContatore[DBPosizioneContatore]." & "Errore caricamento Posizione Contatore")
            End Try
            Return myDv
        End Function
        'Public Function GetListaPosizioneContatore() As GetLista
        '    Try
        '        Dim GetLista As New GetLista()

        '        Dim oConn As New SqlConnection()
        '        Dim oCmd As New SqlCommand()

        '        GetLista.lngRecodvMyDatiCount = iDB.RunSPReturnToGrid("sp_ReturnPosizioneContatore", oConn, oCmd)

        '        GetLista.oConn = oConn
        '        GetLista.oComm = oCmd


        '        Return GetLista
        '    Catch ex As Exception
        ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBPosizioneContatore.GetListaPosizioneContatore.errore: ", ex)
        '        Throw New Exception("GetListaPosizioneContatore[DBPosizioneContatore]." & "Errore caricamento Posizione Contatore")
        '    End Try
        'End Function
        ''' <summary>
        ''' Estrae l'oggetto in base al codice passato
        ''' </summary>
        ''' <param name="CODPOSIZIONE">intero</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetPosizioneContatore(ByVal CODPOSIZIONE As Integer) As TabelleDiDecodifica.PosizioneContatore
            Dim myItem As New TabelleDiDecodifica.PosizioneContatore()
            Try
                Dim lngTipoOperazione As Long
                Dim sSQL As String

                If CODPOSIZIONE = -1 Then lngTipoOperazione = DecEnum.DBOperation.DB_INSERT

                If lngTipoOperazione = DecEnum.DBOperation.DB_UPDATE Then
                    Dim dvMyDati As New DataView
                    sSQL = ""
                    sSQL = "SELECT * FROM TP_POSIZIONECONTATORE WHERE CODPOSIZIONE = " & Utility.StringOperation.FormatString(CODPOSIZIONE)

                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            myItem.Descrizione = Utility.StringOperation.FormatString(myRow("DESCRIZIONE"))
                            myItem.Posizione = Utility.StringOperation.FormatInt(myRow("posizione"))
                            myItem.Note = Utility.StringOperation.FormatString(myRow("NOTE"))
                        Next
                    End If

                    dvMyDati.Dispose()

                End If

                If lngTipoOperazione = DecEnum.DBOperation.DB_INSERT Then

                    myItem.CODPOSIZIONE = -1
                    myItem.Posizione = -1
                    myItem.Descrizione = ""
                    myItem.Note = ""

                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBPosizioneContatore.GetPosizioneContatore.errore: ", ex)
            End Try
            Return myItem
        End Function
        ''' <summary>
        ''' Inserisce un oggetto
        ''' </summary>
        ''' <param name="myDetail">oggetto di tipo TabelleDiDecodifica.PosizioneContatore</param>
        ''' <remarks></remarks>
        Public Sub SetPosizioneContatore(ByVal myDetail As TabelleDiDecodifica.PosizioneContatore)
            Try
                Dim lngTipoOperazione As Long
                Dim sSQL As String

                Dim dvMyDati As New DataView

                lngTipoOperazione = DecEnum.DBOperation.DB_UPDATE

                If myDetail.CODPOSIZIONE = -1 Then
                    lngTipoOperazione = DecEnum.DBOperation.DB_INSERT
                End If

                If lngTipoOperazione = DecEnum.DBOperation.DB_INSERT Then
                    ''''Verifica esistenza descrizione 

                    sSQL = ""
                    sSQL = "SELECT DESCRIZIONE FROM TP_POSIZIONECONTATORE WHERE DESCRIZIONE=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.Descrizione), "'", "''"))
                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            Throw New Exception("La descrizione Inserita è già presente  in tabella !")
                        Next
                    End If
                    dvMyDati.Dispose()

                    sSQL = ""
                    sSQL = "SELECT POSIZIONE FROM TP_POSIZIONECONTATORE WHERE POSIZIONE=" & Utility.StringOperation.FormatString(myDetail.Posizione)
                    sSQL += "AND" & vbCrLf
                    sSQL += "(POSIZIONE  IS NOT NULL  AND POSIZIONE <>'')" & vbCrLf
                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            Throw New Exception("La Posizione Inserita è già presente  in tabella !")
                        Next
                    End If
                    dvMyDati.Dispose()

                    sSQL = ""
                    sSQL = "INSERT INTO TP_POSIZIONECONTATORE"
                    sSQL += "(POSIZIONE,DESCRIZIONE,NOTE)" & vbCrLf
                    sSQL += "VALUES ( " & vbCrLf
                    sSQL += Utility.StringOperation.FormatString(UCase(myDetail.Posizione)) & "," & vbCrLf
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
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBPosizioneContatore.SetPosizioneContatore.errore: ", ex)
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
                    sSQL = "DELETE FROM TP_POSIZIONECONTATORE WHERE CODPOSIZIONE=" & myDetail.CODPOSIZIONE

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
                    sSQL = "SELECT POSIZIONE FROM TP_POSIZIONECONTATORE WHERE POSIZIONE=" & Utility.StringOperation.FormatString(myDetail.Posizione) & vbCrLf
                    sSQL += "AND" & vbCrLf
                    sSQL += "(POSIZIONE  IS NOT NULL  AND POSIZIONE <>'')" & vbCrLf
                    sSQL += "AND" & vbCrLf
                    sSQL += "CODPOSIZIONE <> " & myDetail.CODPOSIZIONE & vbCrLf

                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            Throw New Exception("La Posizione Inserita è già presente  in tabella !")
                        Next
                    End If
                    dvMyDati.Dispose()

                    sSQL = "UPDATE TP_POSIZIONECONTATORE SET "
                    sSQL += "POSIZIONE =" & Utility.StringOperation.FormatString(myDetail.Posizione) & vbCrLf
                    sSQL += ",DESCRIZIONE =" & Utility.StringOperation.FormatString(UCase(myDetail.Descrizione)) & vbCrLf
                    sSQL += ",NOTE=" & Utility.StringOperation.FormatString(UCase(myDetail.Note)) & vbCrLf
                    sSQL += "WHERE" & vbCrLf
                    sSQL += "CODPOSIZIONE=" & myDetail.CODPOSIZIONE

                    Try
                        If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                            Throw New Exception("errore in::" & sSQL)
                        End If
                    Catch ex As SqlException
                        Throw ex
                    Catch ex As Exception
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBPosizioneContatore.SetPosizioneContatore.errore: ", ex)
                        Throw ex
                    End Try

                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBPosizioneContatore.SetPosizioneContatore.errore: ", ex)
            End Try
        End Sub
        ''' <summary>
        ''' Elimina un oggetto
        ''' </summary>
        ''' <param name="CODPOSIZIONE">intero</param>
        ''' <remarks></remarks>
        Public Sub EliminaPosizioneContatore(ByVal CODPOSIZIONE As Integer)
            Dim sSQL As String
            Try
                sSQL = "DELETE FROM TP_POSIZIONECONTATORE WHERE CODPOSIZIONE=" & CODPOSIZIONE
                If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                    Throw New Exception("errore in::" & sSQL)
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBPosizioneContatore.EliminaPosizioneContatore.errore: ", ex)
                Throw ex
            End Try
        End Sub
        ''' <summary>
        ''' Modifica un oggetto
        ''' </summary>
        ''' <param name="myDetail">oggetto di tipo TabelleDiDecodifica.PosizioneContatore</param>
        ''' <remarks></remarks>
        Public Sub UpdateForzato(ByVal myDetail As TabelleDiDecodifica.PosizioneContatore)
            Dim sSQL As String
            Dim dvMyDati As New DataView

            sSQL = ""
            sSQL = "SELECT POSIZIONE FROM TP_POSIZIONECONTATORE WHERE POSIZIONE=" & Utility.StringOperation.FormatString(myDetail.Posizione) & vbCrLf
            sSQL += "AND" & vbCrLf
            sSQL += "(POSIZIONE  IS NOT NULL  AND POSIZIONE <>'')" & vbCrLf
            sSQL += "AND" & vbCrLf
            sSQL += "CODPOSIZIONE <> " & myDetail.CODPOSIZIONE & vbCrLf

            dvMyDati = iDB.GetDataView(sSQL)
            Try
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        Throw New Exception("La Posizione Inserita è già presente  in tabella !")
                    Next
                End If
                dvMyDati.Dispose()

                sSQL = "UPDATE TP_POSIZIONECONTATORE SET "
                sSQL += "POSIZIONE =" & Utility.StringOperation.FormatString(myDetail.Posizione) & vbCrLf
                sSQL += ",DESCRIZIONE =" & Utility.StringOperation.FormatString(UCase(myDetail.Descrizione)) & vbCrLf
                sSQL += ",NOTE=" & Utility.StringOperation.FormatString(UCase(myDetail.Note)) & vbCrLf
                sSQL += "WHERE" & vbCrLf
                sSQL += "CODPOSIZIONE=" & myDetail.CODPOSIZIONE

                Try
                    If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                        Throw New Exception("errore in::" & sSQL)
                    End If
                Catch ex As SqlException
                    Throw ex
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBPosizioneContatore.UpdateForzato.errore: ", ex)
                    Throw ex
                End Try
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBPosizioneContatore.UpdateForzato.errore: ", ex)
            End Try
        End Sub

    End Class
End Namespace
