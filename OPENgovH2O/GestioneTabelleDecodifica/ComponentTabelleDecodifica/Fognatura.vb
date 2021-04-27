Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports log4net

Namespace TabelleDiDecodifica
    Public Class DetailFognatura
        Public Descrizione As String
        Public CodiceFognatura As String
        Public CodFognatura As String
        Public Note As String
    End Class
    Public Class DBFognatura
        Inherits TabelleDiDecodifica.DetailFognatura

        Dim iDB As New DBAccess.getDBobject()
        Dim _Const As New Costanti()
        Private Shared Log As ILog = LogManager.GetLogger(GetType(DBFognatura))
        ''''/Gestione lista da presentare a video'''''''''''''''''''''''''''''''''
        'Public Function GetListaFognatura() As GetLista
        '    Try
        '        Dim GetLista As New GetLista()

        '        Dim oConn As New SqlConnection()
        '        Dim oCmd As New SqlCommand()

        '        GetLista.lngRecodvMyDatiCount = iDB.RunSPReturnToGrid("sp_ReturnFognatura", oConn, oCmd)

        '        GetLista.oConn = oConn
        '        GetLista.oComm = oCmd


        '        Return GetLista
        '    Catch ex As Exception
        ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBFognatura.GetListaFognatura.errore: ", ex)
        '        Throw New Exception("GetListaFognatura[DBFognatura]." & "Errore caricamento tabella Fognatura")
        '    End Try

        'End Function
        Public Function GetListaFognatura() As DataView
            Dim cmdMyCommand As New SqlCommand
            Dim myDv As DataView
            Try
                cmdMyCommand.CommandType = CommandType.StoredProcedure
                cmdMyCommand.CommandText = "sp_ReturnFognatura"

                myDv = iDB.GetDataView(cmdMyCommand)
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBFognatura.GetListaFognatura.errore: ", ex)
                Throw New Exception("GetListaFognatura[DBFognatura]." & "Errore caricamento tabella Fognatura")
            End Try
            Return myDv
        End Function
        ''''/Fine Gestione lista da presentare a video'''''''''''''''''''''''''''''''''

        Public Function GetFognatura(ByVal CODFOGNATURA As Integer) As TabelleDiDecodifica.DetailFognatura

            Dim lngTipoOperazione As Long
            Dim sSQL As String

            Dim DetailFognatura As New TabelleDiDecodifica.DetailFognatura()
            Try
                If CODFOGNATURA = -1 Then lngTipoOperazione = DecEnum.DBOperation.DB_INSERT

                If lngTipoOperazione = DecEnum.DBOperation.DB_UPDATE Then
                    Dim dvMyDati As New DataView
                    sSQL = ""
                    sSQL = "SELECT * FROM TP_FOGNATURA WHERE CODFOGNATURA = " & Utility.StringOperation.FormatString(CODFOGNATURA)

                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            DetailFognatura.Descrizione = Utility.StringOperation.FormatString(myRow("DESCRIZIONE"))
                            DetailFognatura.CodiceFognatura = Utility.StringOperation.FormatString(myRow("CODICEFOGNATURA"))
                            DetailFognatura.Note = Utility.StringOperation.FormatString(myRow("NOTE"))
                        Next
                    End If

                    dvMyDati.Dispose()

                End If

                If lngTipoOperazione = DecEnum.DBOperation.DB_INSERT Then

                    DetailFognatura.CodFognatura = -1
                    DetailFognatura.Descrizione = ""
                    DetailFognatura.CodiceFognatura = ""
                    DetailFognatura.Note = ""

                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBFognatura.GetFognatura.errore: ", ex)
            End Try
            Return DetailFognatura
        End Function
        ''''''''''''''''/RITORNA IL DETTAGLIO DELLA TABELLA DEPURAZIONE''''''''''''''''''''''''''''''//
        Public Sub SetFOGNATURA(ByVal myDetail As TabelleDiDecodifica.DetailFognatura)

            Dim lngTipoOperazione As Long
            Dim sSQL As String
            Dim dvMyDati As New DataView

            lngTipoOperazione = DecEnum.DBOperation.DB_UPDATE
            Try
                If myDetail.CodFognatura = -1 Then
                    lngTipoOperazione = DecEnum.DBOperation.DB_INSERT
                End If

                If lngTipoOperazione = DecEnum.DBOperation.DB_INSERT Then
                    ''''Verifica esistenza descrizione 

                    sSQL = ""
                    sSQL = "SELECT DESCRIZIONE FROM TP_FOGNATURA WHERE DESCRIZIONE=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.Descrizione), "'", "''"))

                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            Throw New Exception("La descrizione Inserita è già presente  in tabella !")
                        Next
                    End If
                    dvMyDati.Dispose()

                    sSQL = ""
                    sSQL = "SELECT CODICEFOGNATURA FROM TP_FOGNATURA WHERE CODICEFOGNATURA=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.CodiceFognatura), "'", "''")) & vbCrLf
                    sSQL += "AND" & vbCrLf
                    sSQL += "(CODICEFOGNATURA IS NOT NULL  AND CODICEFOGNATURA <>'')" & vbCrLf


                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            Throw New Exception("Il Codice Fognatura inserito è già presente in Tabella !")
                        Next
                    End If
                    dvMyDati.Dispose()
                    sSQL = ""
                    sSQL = "INSERT INTO TP_FOGNATURA"
                    sSQL += "(CODICEFOGNATURA,DESCRIZIONE,NOTE)" & vbCrLf
                    sSQL += "VALUES ( " & vbCrLf
                    sSQL += Utility.StringOperation.FormatString(UCase(myDetail.CodiceFognatura)) & "," & vbCrLf
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
                    End Try

                End If

                If lngTipoOperazione = DecEnum.DBOperation.DB_UPDATE Then

                    Dim sqlTrans As SqlTransaction
                    Dim sqlConn As New SqlConnection
                    Dim sqlCmdInsert As SqlCommand


                    sqlConn.ConnectionString = ConstSession.StringConnection

                    sqlConn.Open()
                    sqlTrans = sqlConn.BeginTransaction
                    sSQL = "DELETE FROM TP_FOGNATURA WHERE CODFOGNATURA=" & myDetail.CodFognatura

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
                    sSQL = "SELECT CODICEFOGNATURA FROM TP_FOGNATURA WHERE CODICEFOGNATURA=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.CodiceFognatura), "'", "''")) & vbCrLf
                    sSQL += "AND" & vbCrLf
                    sSQL += "(CODICEFOGNATURA IS NOT NULL  AND CODICEFOGNATURA <>'')" & vbCrLf
                    sSQL += "AND" & vbCrLf
                    sSQL += "CODFOGNATURA <> " & myDetail.CodFognatura & vbCrLf

                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            Throw New Exception("Il Codice Fognatura inserito è già presente in Tabella !")
                        Next
                    End If
                    dvMyDati.Dispose()

                    sSQL = "UPDATE TP_FOGNATURA SET "
                    sSQL += "CODICEFOGNATURA =" & Utility.StringOperation.FormatString(UCase(myDetail.CodiceFognatura)) & vbCrLf
                    sSQL += ",DESCRIZIONE =" & Utility.StringOperation.FormatString(UCase(myDetail.Descrizione)) & vbCrLf
                    sSQL += ",NOTE=" & Utility.StringOperation.FormatString(UCase(myDetail.Note)) & vbCrLf
                    sSQL += "WHERE" & vbCrLf
                    sSQL += "CODFOGNATURA=" & myDetail.CodFognatura

                    Try
                        If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                            Throw New Exception("errore in::" & sSQL)
                        End If
                    Catch ex As SqlException
                        Throw ex
                    Catch ex As Exception
                        Throw ex
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBFognatura.SetFOGNATURA.errore: ", ex)
                    End Try

                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBFognatura.SetFOGNATURA.errore: ", ex)
            End Try
        End Sub

        Public Sub EliminaFognatura(ByVal CODFOGNATURA As Integer)
            Dim sSQL As String
            Try
                sSQL = "DELETE FROM TP_FOGNATURA WHERE CODFOGNATURA=" & CODFOGNATURA
                If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                    Throw New Exception("errore in::" & sSQL)
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBFognatura.EliminaFognatura.errore: ", ex)
                Throw ex
            End Try
        End Sub

        Public Sub UpdateForzato(ByVal myDetail As TabelleDiDecodifica.DetailFognatura)
            Dim sSQL As String
            Dim dvMyDati As New DataView
            sSQL = ""
            sSQL = "SELECT CODICEFOGNATURA FROM TP_FOGNATURA WHERE CODICEFOGNATURA=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.CodiceFognatura), "'", "''")) & vbCrLf
            sSQL += "AND" & vbCrLf
            sSQL += "(CODICEFOGNATURA IS NOT NULL  AND CODICEFOGNATURA <>'')" & vbCrLf
            sSQL += "AND" & vbCrLf
            sSQL += "CODFOGNATURA <> " & myDetail.CodFognatura & vbCrLf

            dvMyDati = iDB.GetDataView(sSQL)
            Try
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        Throw New Exception("Il Codice Fognatura inserito è già presente in Tabella !")
                    Next
                End If
                dvMyDati.Dispose()

                sSQL = "UPDATE TP_FOGNATURA SET "
                sSQL += "CODICEDEPURAZIONE =" & utility.stringoperation.formatstring(UCase(myDetail.CodiceFognatura)) & vbCrLf
                sSQL += ",DESCRIZIONE =" & utility.stringoperation.formatstring(UCase(myDetail.Descrizione)) & vbCrLf
                sSQL += ",NOTE=" & utility.stringoperation.formatstring(UCase(myDetail.Note)) & vbCrLf
                sSQL += "WHERE" & vbCrLf
                sSQL += "CODDEPURAZIONE=" & myDetail.CodFognatura

                Try
                    If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                        Throw New Exception("errore in::" & sSQL)
                    End If
                Catch ex As SqlException
                    Throw ex
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBFognatura.UpdateForzato.errore: ", ex)
                    Throw ex
                End Try
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBFognatura.UpdateForzato.errore: ", ex)
            End Try
        End Sub






    End Class




End Namespace