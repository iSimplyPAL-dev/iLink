Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports log4net

Namespace TabelleDiDecodifica
    Public Class DBDiametroContatore
        Inherits TabelleDiDecodifica.DiametroContatore
        ''' <summary>
        ''' Gestione diametro contatore
        ''' </summary>
        ''' <remarks></remarks>
        Dim iDB As New DBAccess.getDBobject()
        Dim _Const As New Costanti()


        Private Shared Log As ILog = LogManager.GetLogger(GetType(DBDiametroContatore))

        ''' <summary>
        ''' Estrae l'elenco degli oggetti configurati
        ''' </summary>
        ''' <param name="CodiceIstat">stringa</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetListaDiametroContatore(ByVal CodiceIstat As String) As DataView
            Dim cmdMyCommand As New SqlCommand
            Dim myDv As DataView = Nothing
            Try
                cmdMyCommand.CommandType = CommandType.StoredProcedure
                cmdMyCommand.CommandText = "sp_ReturnDiametroContatori"
                cmdMyCommand.Parameters.Add(New SqlParameter("@CODICEISTAT", CodiceIstat))

                myDv = iDB.GetDataView(cmdMyCommand)
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBDiametroContatore.GetListaDiametroContatore.errore: ", ex)
                Throw New Exception("GetListaDiametroContatore[DBDiametroContatore]." & "Errore caricamento Diametro Contatore")
            End Try
            Return myDv
        End Function
        ''' <summary>
        ''' Estrae l'oggetto in base al codice passato
        ''' </summary>
        ''' <param name="CODDIAMETROCONTATORE">intero</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetDiametroContatore(ByVal CODDIAMETROCONTATORE As Integer) As TabelleDiDecodifica.DiametroContatore

            Dim lngTipoOperazione As Long
            Dim sSQL As String

            Dim DetailDiametroContatore As New TabelleDiDecodifica.DiametroContatore()
            Try
                If CODDIAMETROCONTATORE = -1 Then lngTipoOperazione = DecEnum.DBOperation.DB_INSERT

                DetailDiametroContatore.Importo = iDB.RunSPReturnDataSet("sp_TARIFFECONTATORI", "TP_TARIFFECONTATORI")

                If lngTipoOperazione = DecEnum.DBOperation.DB_UPDATE Then
                    Dim dvMyDati As New DataView
                    sSQL = "SELECT * FROM TP_DIAMETROCONTATORE WHERE CODDIAMETROCONTATORE = " & Utility.StringOperation.FormatString(CODDIAMETROCONTATORE)
                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            DetailDiametroContatore.Descrizione = Utility.StringOperation.FormatString(myRow("DESCRIZIONE"))
                            DetailDiametroContatore.CodTariffaContatore = MyUtility.CIdFromDB(myrow("CODTARIFFACONTATORE"))
                            DetailDiametroContatore.DiametroContatore = Utility.StringOperation.FormatString(myrow("DIAMETROCONTATORE"))
                            DetailDiametroContatore.Note = Utility.StringOperation.FormatString(myrow("NOTE"))
                            DetailDiametroContatore.Prevalente = Utility.StringOperation.FormatBool(myRow("PREVALENTE"))
                        Next
                    End If
                    dvMyDati.Dispose()

                End If

                If lngTipoOperazione = DecEnum.DBOperation.DB_INSERT Then
                    DetailDiametroContatore.CodDiametroContatore = -1
                    DetailDiametroContatore.CodTariffaContatore = -1
                    DetailDiametroContatore.Descrizione = ""
                    DetailDiametroContatore.DiametroContatore = ""
                    DetailDiametroContatore.Note = ""
                    DetailDiametroContatore.Prevalente = False
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBDiametroContatore.GetDiametroContatore.errore: ", ex)
            End Try
            Return DetailDiametroContatore
        End Function
        ''' <summary>
        ''' Inserisce un oggetto
        ''' </summary>
        ''' <param name="myDetail">oggetto di tipo TabelleDiDecodifica.DiametroContatore</param>
        ''' <remarks></remarks>
        Public Sub SetDiametroContatore(ByVal myDetail As TabelleDiDecodifica.DiametroContatore)
            Dim lngTipoOperazione As Long
            Dim sSQL As String
            Dim dvMyDati As New DataView

            lngTipoOperazione = DecEnum.DBOperation.DB_UPDATE
            Try
                If myDetail.CodDiametroContatore = -1 Then
                    lngTipoOperazione = DecEnum.DBOperation.DB_INSERT
                End If

                If lngTipoOperazione = DecEnum.DBOperation.DB_INSERT Then
                    ''''Verifica esistenza descrizione 

                    sSQL = ""
                    sSQL = "SELECT DESCRIZIONE FROM TP_DIAMETROCONTATORE WHERE DESCRIZIONE=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.Descrizione), "'", "''")) & vbCrLf
                    sSQL += "AND" & vbCrLf
                    sSQL += "CODICE_ISTAT=" & Utility.StringOperation.FormatString(myDetail.CodiceISTAT)

                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            Throw New Exception("La descrizione Inserita è già presente  in tabella !")
                        Next
                    End If
                    dvMyDati.Dispose()

                    If myDetail.Prevalente = True Then

                        sSQL = ""
                        sSQL = "SELECT PREVALENTE FROM TP_DIAMETROCONTATORE WHERE PREVALENTE=1" & vbCrLf
                        sSQL += "AND" & vbCrLf
                        sSQL += "CODICE_ISTAT=" & Utility.StringOperation.FormatString(myDetail.CodiceISTAT)

                        dvMyDati = iDB.GetDataView(sSQL)
                        If Not dvMyDati Is Nothing Then
                            For Each myRow As DataRowView In dvMyDati
                                Throw New Exception("Diametro Prevalente già esistente!")
                            Next
                        End If
                        dvMyDati.Dispose()

                    End If

                    sSQL = ""
                    sSQL = "INSERT INTO TP_DIAMETROCONTATORE"
                    sSQL += "(CODTARIFFACONTATORE,DESCRIZIONE,NOTE,DIAMETROCONTATORE,PREVALENTE,CODICE_ISTAT)" & vbCrLf
                    sSQL += "VALUES ( " & vbCrLf
                    sSQL += Utility.StringOperation.FormatInt(UCase(myDetail.CodTariffaContatore)) & "," & vbCrLf
                    sSQL += Utility.StringOperation.FormatString(UCase(myDetail.Descrizione)) & "," & vbCrLf
                    sSQL += Utility.StringOperation.FormatString(UCase(myDetail.Note)) & "," & vbCrLf
                    sSQL += Utility.StringOperation.FormatString(UCase(myDetail.DiametroContatore)) & "," & vbCrLf
                    sSQL += Utility.StringOperation.FormatBool(myDetail.Prevalente) & "," & vbCrLf
                    sSQL += Utility.StringOperation.FormatString(UCase(myDetail.CodiceISTAT)) & vbCrLf

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
                    sSQL = "DELETE FROM TP_DIAMETROCONTATORE WHERE CODDIAMETROCONTATORE=" & myDetail.CodDiametroContatore

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
                    sSQL = "SELECT DESCRIZIONE FROM TP_DIAMETROCONTATORE WHERE DESCRIZIONE=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.Descrizione), "'", "''")) & vbCrLf
                    sSQL += "AND" & vbCrLf
                    sSQL += "CODDIAMETROCONTATORE<>" & myDetail.CodDiametroContatore & vbCrLf
                    sSQL += "AND" & vbCrLf
                    sSQL += "CODICE_ISTAT=" & Utility.StringOperation.FormatString(myDetail.CodiceISTAT)

                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            Throw New Exception("La descrizione Inserita è già presente  in tabella!")
                        Next
                    End If
                    dvMyDati.Dispose()

                    If myDetail.Prevalente = True Then
                        sSQL = ""
                        sSQL = "SELECT PREVALENTE FROM TP_DIAMETROCONTATORE WHERE PREVALENTE=1" & vbCrLf
                        sSQL += "AND" & vbCrLf
                        sSQL += "CODDIAMETROCONTATORE<>" & myDetail.CodDiametroContatore & vbCrLf
                        sSQL += "AND" & vbCrLf
                        sSQL += "CODICE_ISTAT=" & Utility.StringOperation.FormatString(myDetail.CodiceISTAT)

                        dvMyDati = iDB.GetDataView(sSQL)
                        If Not dvMyDati Is Nothing Then
                            For Each myRow As DataRowView In dvMyDati
                                Throw New Exception("Diametro Prevalente già esistente!")
                            Next
                        End If
                        dvMyDati.Dispose()
                    End If

                    sSQL = "UPDATE TP_DIAMETROCONTATORE SET "
                    sSQL += "CODTARIFFACONTATORE =" & Utility.StringOperation.FormatInt(myDetail.CodTariffaContatore) & vbCrLf
                    sSQL += ",DESCRIZIONE =" & Utility.StringOperation.FormatString(UCase(myDetail.Descrizione)) & vbCrLf
                    sSQL += ",NOTE=" & Utility.StringOperation.FormatString(UCase(myDetail.Note)) & vbCrLf
                    sSQL += ",DIAMETROCONTATORE=" & Utility.StringOperation.FormatString(UCase(myDetail.DiametroContatore)) & vbCrLf
                    sSQL += ",PREVALENTE=" & Utility.StringOperation.FormatBool(myDetail.Prevalente) & vbCrLf
                    sSQL += "WHERE" & vbCrLf
                    sSQL += "CODDIAMETROCONTATORE=" & myDetail.CodDiametroContatore & vbCrLf
                    sSQL += "AND" & vbCrLf
                    sSQL += "CODICE_ISTAT=" & Utility.StringOperation.FormatString(myDetail.CodiceISTAT)

                    Try
                        If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                            Throw New Exception("errore in::" & sSQL)
                        End If
                    Catch ex As SqlException
                        Throw ex
                    Catch ex As Exception
                        Throw ex
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBDiametroContatore.SetDiametroContatore.errore: ", ex)

                    End Try

                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBDiametroContatore.SetDiametroContatore.errore: ", ex)
            End Try
        End Sub
        ''' <summary>
        ''' Elimina un oggetto
        ''' </summary>
        ''' <param name="CODDIAMETROCONTATORE">intero</param>
        ''' <remarks></remarks>
        Public Sub EliminaDiametroContatore(ByVal CODDIAMETROCONTATORE As Integer)
            Dim sSQL As String
            Try
                sSQL = "DELETE FROM TP_DIAMETROCONTATORE WHERE CODDIAMETROCONTATORE=" & CODDIAMETROCONTATORE
                If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                    Throw New Exception("errore in::" & sSQL)
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBDiametroContatore.EliminaDiametroContatore.errore: ", ex)
                Throw ex
            End Try
        End Sub
        ''' <summary>
        ''' Modifica un oggetto
        ''' </summary>
        ''' <param name="myDetail">oggetto di tipo TabelleDiDecodifica.DiametroContatore</param>
        ''' <remarks></remarks>
        Public Sub UpdateForzato(ByVal myDetail As TabelleDiDecodifica.DiametroContatore)
            Dim sSQL As String
            Dim dvMyDati As New DataView
            Try
                sSQL = ""
                sSQL = "SELECT DESCRIZIONE FROM TP_DIAMETROCONTATORE WHERE DESCRIZIONE=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.Descrizione), "'", "''")) & vbCrLf
                sSQL += "AND" & vbCrLf
                sSQL += "CODDIAMETROCONTATORE<>" & myDetail.CodDiametroContatore & vbCrLf
                sSQL += "AND" & vbCrLf
                sSQL += "CODICE_ISTAT=" & Utility.StringOperation.FormatString(myDetail.CodiceISTAT)

                dvMyDati = iDB.GetDataView(sSQL)
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        Throw New Exception("La descrizione Inserita è già presente  in tabella!")
                    Next
                End If
                dvMyDati.Dispose()

                If myDetail.Prevalente = True Then

                    sSQL = ""
                    sSQL = "SELECT PREVALENTE FROM TP_DIAMETROCONTATORE WHERE PREVALENTE=1" & vbCrLf
                    sSQL += "AND" & vbCrLf
                    sSQL += "CODDIAMETROCONTATORE<>" & myDetail.CodDiametroContatore & vbCrLf
                    sSQL += "AND" & vbCrLf
                    sSQL += "CODICE_ISTAT=" & Utility.StringOperation.FormatString(myDetail.CodiceISTAT)

                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            Throw New Exception("Diametro Prevalente già esistente!")
                        Next
                    End If
                    dvMyDati.Dispose()
                End If
                sSQL = "UPDATE TP_DIAMETROCONTATORE SET "
                sSQL += "CODTARIFFACONTATORE =" & Utility.StringOperation.FormatInt(myDetail.CodTariffaContatore) & vbCrLf
                sSQL += ",DESCRIZIONE =" & Utility.StringOperation.FormatString(UCase(myDetail.Descrizione)) & vbCrLf
                sSQL += ",NOTE=" & Utility.StringOperation.FormatString(UCase(myDetail.Note)) & vbCrLf
                sSQL += ",DIAMETROCONTATORE=" & Utility.StringOperation.FormatString(UCase(myDetail.DiametroContatore)) & vbCrLf
                sSQL += "WHERE" & vbCrLf
                sSQL += "CODDIAMETROCONTATORE=" & myDetail.CodDiametroContatore

                Try
                    If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                        Throw New Exception("errore in::" & sSQL)
                    End If
                Catch ex As SqlException
                    Throw ex
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBDiametroContatore.UpdateForzato.errore: ", ex)
                    Throw ex
                End Try
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBDiametroContatore.UpdateForzato.errore: ", ex)
            End Try
        End Sub

        'Public Function GetImporto(ByVal pdvMyDatiStatus As Object) As String
        '    dim sSQL as string
        '    Dim dr As new dataview
        '    GetImporto = ""
        '    sSQL=""
        '    sSQL="SELECT IMPORTO FROM TP_TARIFFECONTATORI WHERE CODTARIFFACONTATORE=" & pdvMyDatiStatus

        '    dr = iDB.getdataview(sSQL)
        '    If dr.Read Then
        '        GetImporto = utility.stringoperation.formatstring(dr("IMPORTO"))
        '    End If
        '    dr.Close()
        'End Function

    End Class

End Namespace