Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports log4net

Namespace TabelleDiDecodifica
    Public Class DBIVA
        Inherits TabelleDiDecodifica.DetailIVA
        ''' <summary>
        ''' Gestione tipo di assoggettamento IVA
        ''' </summary>
        ''' <remarks></remarks>
        Dim iDB As New DBAccess.getDBobject()
        Dim _Const As New Costanti()

        Private Shared Log As ILog = LogManager.GetLogger(GetType(DBIVA))

        ''' <summary>
        ''' Estrae l'elenco degli oggetti configurati
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetListaIVA() As DataView
            Dim cmdMyCommand As New SqlCommand
            Dim myDv As DataView = Nothing
            Try
                cmdMyCommand.CommandType = CommandType.StoredProcedure
                cmdMyCommand.CommandText = "sp_ReturnIVA"

                myDv = iDB.GetDataView(cmdMyCommand)
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBIVA.GetListaIVA.errore: ", ex)
                Throw New Exception("GetListaIVA[DBIVA]." & "Errore caricamento tabella Iva")
            End Try
            Return myDv
        End Function
        'Public Function GetListaIVA() As GetLista
        '    Try
        '        Dim GetLista As New GetLista()

        '        Dim oConn As New SqlConnection()
        '        Dim oCmd As New SqlCommand()

        '        GetLista.lngRecodvMyDatiCount = iDB.RunSPReturnToGrid("sp_ReturnIVA ", oConn, oCmd)

        '        GetLista.oConn = oConn
        '        GetLista.oComm = oCmd


        '        Return GetLista
        '    Catch ex As Exception
        'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBIVA.GetListaIVA.errore: ", ex)
        '        Throw New Exception("GetListaIVA[DBIVA]." & "Errore caricamento tabella Iva")
        '    End Try

        'End Function
        ''' <summary>
        ''' Estrae l'oggetto in base al codice passato
        ''' </summary>
        ''' <param name="CODIVA">intero</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetIVA(ByVal CODIVA As Integer) As TabelleDiDecodifica.DetailIVA
            Dim myIVA As New TabelleDiDecodifica.DetailIVA()
            Try
                Dim lngTipoOperazione As Long
                'Dim _enum As _Enum
                Dim sSQL As String

                If CODIVA = -1 Then lngTipoOperazione = DecEnum.DBOperation.DB_INSERT

                If lngTipoOperazione = DecEnum.DBOperation.DB_UPDATE Then
                    Dim dvMyDati As New DataView
                    sSQL = ""
                    sSQL = "SELECT * FROM TP_IVA WHERE CODIVA = " & CODIVA
                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            myIVA.Descrizione = Utility.StringOperation.FormatString(myRow("DESCRIZIONE"))
                            myIVA.CodiceIVA = Utility.StringOperation.FormatString(myRow("CODICEIVA"))
                            myIVA.Note = Utility.StringOperation.FormatString(myRow("NOTE"))
                        Next
                    End If

                    dvMyDati.Dispose()

                End If

                If lngTipoOperazione = DecEnum.DBOperation.DB_INSERT Then

                    myIVA.CodIVA = -1
                    myIVA.CodiceIVA = ""
                    myIVA.Descrizione = ""
                    myIVA.Note = ""

                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBIVA.GetIVA.errore: ", ex)
            End Try
            Return myIVA
        End Function
        ''' <summary>
        ''' Inserisce un oggetto
        ''' </summary>
        ''' <param name="myDetail">oggetto di tipo TabelleDiDecodifica.DetailIVA</param>
        ''' <remarks></remarks>
        Public Sub SetIVA(ByVal myDetail As TabelleDiDecodifica.DetailIVA)
            Try
                Dim lngTipoOperazione As Long
                Dim sSQL As String
                Dim dvMyDati As New DataView

                lngTipoOperazione = DecEnum.DBOperation.DB_UPDATE

                If myDetail.CodIVA = -1 Then
                    lngTipoOperazione = DecEnum.DBOperation.DB_INSERT
                End If

                If lngTipoOperazione = DecEnum.DBOperation.DB_INSERT Then
                    ''''Verifica esistenza descrizione 

                    sSQL = ""
                    sSQL = "SELECT DESCRIZIONE FROM TP_IVA WHERE DESCRIZIONE=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.Descrizione), "'", "''"))

                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            Throw New Exception("La descrizione Inserita è già presente  in tabella !")
                        Next
                    End If
                    dvMyDati.Dispose()

                    sSQL = ""
                    sSQL = "SELECT CODICEIVA FROM TP_IVA WHERE CODICEIVA=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.CodiceIVA), "'", "''")) & vbCrLf
                    sSQL += "AND" & vbCrLf
                    sSQL += "(CODICEIVA IS NOT NULL  AND CODICEIVA <>'')" & vbCrLf


                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            Throw New Exception("Il Codice IVA inserito è già presente in Tabella !")
                        Next
                    End If
                    dvMyDati.Dispose()
                    sSQL = ""
                    sSQL = "INSERT INTO TP_IVA"
                    sSQL += "(CODICEIVA,DESCRIZIONE,NOTE)" & vbCrLf
                    sSQL += "VALUES ( " & vbCrLf
                    sSQL += Utility.StringOperation.FormatString(UCase(myDetail.CodiceIVA)) & "," & vbCrLf
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
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBIVA.SetIVA.errore: ", ex)
                    End Try

                End If

                If lngTipoOperazione = DecEnum.DBOperation.DB_UPDATE Then

                    Dim sqlTrans As SqlTransaction
                    Dim sqlConn As New SqlConnection
                    Dim sqlCmdInsert As SqlCommand


                    sqlConn.ConnectionString = ConstSession.StringConnection

                    sqlConn.Open()
                    sqlTrans = sqlConn.BeginTransaction
                    sSQL = "DELETE FROM TP_IVA WHERE CODIVA=" & myDetail.CodIVA

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
                    sSQL = "SELECT CODICEIVA FROM TP_IVA WHERE CODICEIVA=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.CodiceIVA), "'", "''")) & vbCrLf
                    sSQL += "AND" & vbCrLf
                    sSQL += "(CODICEIVA IS NOT NULL  AND CODICEIVA <>'')" & vbCrLf
                    sSQL += "AND" & vbCrLf
                    sSQL += "CODIVA <> " & myDetail.CodIVA & vbCrLf

                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            Throw New Exception("Il Codice Iva inserito è già presente in Tabella !")
                        Next
                    End If
                    dvMyDati.Dispose()

                    sSQL = "UPDATE TP_IVA SET "
                    sSQL += "CODICEIVA =" & Utility.StringOperation.FormatString(UCase(myDetail.CodiceIVA)) & vbCrLf
                    sSQL += ",DESCRIZIONE =" & Utility.StringOperation.FormatString(UCase(myDetail.Descrizione)) & vbCrLf
                    sSQL += ",NOTE=" & Utility.StringOperation.FormatString(UCase(myDetail.Note)) & vbCrLf
                    sSQL += "WHERE" & vbCrLf
                    sSQL += "CODIVA=" & myDetail.CodIVA

                    Try
                        If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                            Throw New Exception("errore in::" & sSQL)
                        End If
                    Catch ex As SqlException
                        Throw ex
                    Catch ex As Exception
                        Throw ex
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBIVA.SetIVA.errore: ", ex)
                    End Try

                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBIVA.SetIVA.errore: ", ex)
            End Try
        End Sub
        ''' <summary>
        ''' Elimina un oggetto
        ''' </summary>
        ''' <param name="CODIVA">intero</param>
        ''' <remarks></remarks>
        Public Sub EliminaIva(ByVal CODIVA As Integer)
            Dim sSQL As String
            Try
                sSQL = "DELETE FROM TP_IVA WHERE CODIVA=" & CODIVA
                If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                    Throw New Exception("errore in::" & sSQL)
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBIVA.EliminaIVA.errore: ", ex)
                Throw ex
            End Try
        End Sub
        ''' <summary>
        ''' Modifica un oggetto
        ''' </summary>
        ''' <param name="myDetail">oggetto di tipo TabelleDiDecodifica.DetailIVA</param>
        ''' <remarks></remarks>
        Public Sub UpdateForzato(ByVal myDetail As TabelleDiDecodifica.DetailIVA)
            Dim sSQL As String
            Dim dvMyDati As New DataView

            sSQL = ""
            sSQL = "SELECT CODICEIVA FROM TP_IVA WHERE CODICEIVA=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.CodiceIVA), "'", "''")) & vbCrLf
            sSQL += "AND" & vbCrLf
            sSQL += "(CODICEIVA IS NOT NULL  AND CODICEIVA <>'')" & vbCrLf
            sSQL += "AND" & vbCrLf
            sSQL += "CODIVA <> " & myDetail.CodIVA & vbCrLf


            dvMyDati = iDB.GetDataView(sSQL)
            Try
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        Throw New Exception("Il Codice IVA inserito è già presente in Tabella !")
                    Next
                End If
                dvMyDati.Dispose()

                sSQL ="UPDATE TP_IVA SET "
                sSQL+="CODICEIVA =" & utility.stringoperation.formatstring(UCase(myDetail.CodiceIVA)) & vbCrLf
                sSQL+=",DESCRIZIONE =" & utility.stringoperation.formatstring(UCase(myDetail.Descrizione)) & vbCrLf
                sSQL+=",NOTE=" & utility.stringoperation.formatstring(UCase(myDetail.Note)) & vbCrLf
                sSQL+="WHERE" & vbCrLf
                sSQL+="CODANOMALIA=" & myDetail.CodIVA

                Try
                    If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                        Throw New Exception("errore in::" & sSQL)
                    End If
                Catch ex As SqlException
                    Throw ex
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBIVA.UpdateForzato.errore: ", ex)
                    Throw ex
                End Try
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBIVA.UpdateForzato.errore: ", ex)
            End Try
        End Sub
    End Class
End Namespace
