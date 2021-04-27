Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports log4net

Namespace TabelleDiDecodifica
    Public Class DBDiametroPresa
        Inherits TabelleDiDecodifica.DiametroPresa
        ''' <summary>
        ''' Gestione diametro presa contatore
        ''' </summary>
        ''' <remarks></remarks>
        Dim iDB As New DBAccess.getDBobject()
        Dim _Const As New Costanti()

        Private Shared Log As ILog = LogManager.GetLogger(GetType(DBDiametroPresa))

        ''' <summary>
        ''' Estrae l'elenco degli oggetti configurati
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetListaDiametroPresa() As DataView
            Dim cmdMyCommand As New SqlCommand
            Dim myDv As DataView = Nothing
            Try
                cmdMyCommand.CommandType = CommandType.StoredProcedure
                cmdMyCommand.CommandText = "sp_ReturnDiametroPresa"

                myDv = iDB.GetDataView(cmdMyCommand)
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBDiametroPresa.GetListaDiametroPresa.errore: ", ex)
                Throw New Exception("GetListaDiametroPresa[DBDiametroPresa]." & "Errore caricamento Diametro Presa")
            End Try
            Return myDv
        End Function
        ''' <summary>
        ''' Estrae l'oggetto in base al codice passato
        ''' </summary>
        ''' <param name="CODDIAMETROPRESA">intero</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetDiametroPresa(ByVal CODDIAMETROPRESA As Integer) As TabelleDiDecodifica.DiametroPresa

            Dim lngTipoOperazione As Long
            Dim sSQL As String

            Dim DetailDiametroPresa As New TabelleDiDecodifica.DiametroPresa()
            Try
                If CODDIAMETROPRESA = -1 Then lngTipoOperazione = DecEnum.DBOperation.DB_INSERT

                DetailDiametroPresa.Importo = iDB.RunSPReturnDataSet("sp_TARIFFECONTATORI", "TP_TARIFFECONTATORI")

                If lngTipoOperazione = DecEnum.DBOperation.DB_UPDATE Then
                    Dim dvMyDati As New DataView
                    sSQL = ""
                    sSQL = "SELECT * FROM TP_DIAMETROPRESA WHERE CODDIAMETROPRESA = " & Utility.StringOperation.FormatString(CODDIAMETROPRESA)
                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            DetailDiametroPresa.Descrizione = Utility.StringOperation.FormatString(myRow("DESCRIZIONE"))
                            DetailDiametroPresa.CodTariffaContatore = MyUtility.CIdFromDB(myrow("CODTARIFFACONTATORE"))
                            DetailDiametroPresa.DiametroPresa = Utility.StringOperation.FormatString(myrow("DIAMETROPRESA"))
                            DetailDiametroPresa.Note = Utility.StringOperation.FormatString(myRow("NOTE"))
                        Next
                    End If

                    dvMyDati.Dispose()

                End If

                If lngTipoOperazione = DecEnum.DBOperation.DB_INSERT Then

                    DetailDiametroPresa.CodDiametroPresa = -1
                    DetailDiametroPresa.CodTariffaContatore = -1
                    DetailDiametroPresa.Descrizione = ""
                    DetailDiametroPresa.DiametroPresa = ""
                    DetailDiametroPresa.Note = ""

                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBDiametroPresa.GetDiametroPresa.errore: ", ex)
            End Try
            Return DetailDiametroPresa
        End Function
        ''' <summary>
        ''' Inserisce un oggetto
        ''' </summary>
        ''' <param name="myDetail">oggetto di tipo TabelleDiDecodifica.DiametroPresa</param>
        ''' <remarks></remarks>
        Public Sub SetDiametroPresa(ByVal myDetail As TabelleDiDecodifica.DiametroPresa)

            Dim lngTipoOperazione As Long
            Dim sSQL As String
            Dim dvMyDati As New DataView

            lngTipoOperazione = DecEnum.DBOperation.DB_UPDATE
            Try
                If myDetail.CodDiametroPresa = -1 Then
                    lngTipoOperazione = DecEnum.DBOperation.DB_INSERT
                End If

                If lngTipoOperazione = DecEnum.DBOperation.DB_INSERT Then
                    ''''Verifica esistenza descrizione 

                    sSQL = ""
                    sSQL = "SELECT DESCRIZIONE FROM TP_DIAMETROPRESA WHERE DESCRIZIONE=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.Descrizione), "'", "''"))

                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            Throw New Exception("La descrizione Inserita è già presente  in tabella !")
                        Next
                    End If
                    dvMyDati.Dispose()

                    sSQL = ""
                    sSQL = "INSERT INTO TP_DIAMETROPRESA"
                    sSQL += "(CODTARIFFACONTATORE,DESCRIZIONE,NOTE,DIAMETROPRESA)" & vbCrLf
                    sSQL += "VALUES ( " & vbCrLf
                    sSQL += Utility.StringOperation.FormatInt(UCase(myDetail.CodTariffaContatore)) & "," & vbCrLf
                    sSQL += Utility.StringOperation.FormatString(UCase(myDetail.Descrizione)) & "," & vbCrLf
                    sSQL += Utility.StringOperation.FormatString(UCase(myDetail.Note)) & "," & vbCrLf
                    sSQL += Utility.StringOperation.FormatString(UCase(myDetail.DiametroPresa)) & vbCrLf
                    sSQL += " )"
                    Try
                        If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                            Throw New Exception("errore in::" & sSQL)
                        End If
                    Catch ex As SqlException
                        Throw ex
                    Catch ex As Exception
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBDiametroPresa.SetDiametroPresa.errore: ", ex)
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
                    sSQL = "DELETE FROM TP_DIAMETROPRESA WHERE CODDIAMETROPRESA=" & myDetail.CodDiametroPresa

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
                    sSQL = "SELECT DESCRIZIONE FROM TP_DIAMETROPRESA WHERE DESCRIZIONE=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.Descrizione), "'", "''")) & vbCrLf
                    sSQL += "AND" & vbCrLf
                    sSQL += "CODDIAMETROPRESA<>" & myDetail.CodDiametroPresa

                    dvMyDati = iDB.GetDataView(sSQL)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            Throw New Exception("La descrizione Inserita è già presente  in tabella !")
                        Next
                    End If
                    dvMyDati.Dispose()



                    sSQL = "UPDATE TP_DIAMETROPRESA SET "
                    sSQL += "CODTARIFFACONTATORE =" & Utility.StringOperation.FormatInt(myDetail.CodTariffaContatore) & vbCrLf
                    sSQL += ",DESCRIZIONE =" & Utility.StringOperation.FormatString(UCase(myDetail.Descrizione)) & vbCrLf
                    sSQL += ",NOTE=" & Utility.StringOperation.FormatString(UCase(myDetail.Note)) & vbCrLf
                    sSQL += ",DIAMETROPRESA=" & Utility.StringOperation.FormatString(UCase(myDetail.DiametroPresa)) & vbCrLf
                    sSQL += "WHERE" & vbCrLf
                    sSQL += "CODDIAMETROPRESA=" & myDetail.CodDiametroPresa

                    Try
                        If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                            Throw New Exception("errore in::" & sSQL)
                        End If
                    Catch ex As SqlException
                        Throw ex
                    Catch ex As Exception
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBDiametroPresa.SetDiametroPresa.errore: ", ex)
                        Throw ex
                    End Try

                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBDiametroPresa.SetDiametroPresa.errore: ", ex)
            End Try
        End Sub
        ''' <summary>
        ''' Elimina un oggetto
        ''' </summary>
        ''' <param name="CODDIAMETROPRESA">intero</param>
        ''' <remarks></remarks>
        Public Sub EliminaDiametroPresa(ByVal CODDIAMETROPRESA As Integer)
            Dim sSQL As String
            Try
                sSQL = "DELETE FROM TP_DIAMETROPRESA WHERE CODDIAMETROPRESA=" & CODDIAMETROPRESA
                If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                    Throw New Exception("errore in::" & sSQL)
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBDiametroPresa.EliminaDiametroPresa.errore: ", ex)
                Throw ex
            End Try
        End Sub
        ''' <summary>
        ''' Modifica un oggetto
        ''' </summary>
        ''' <param name="myDetail">oggetto di tipo TabelleDiDecodifica.DiametroPresa</param>
        ''' <remarks></remarks>
        Public Sub UpdateForzato(ByVal myDetail As TabelleDiDecodifica.DiametroPresa)
            Dim sSQL As String
            Dim dvMyDati As New DataView

            sSQL = ""
            sSQL = "SELECT DESCRIZIONE FROM TP_DIAMETROPRESA WHERE DESCRIZIONE=" & Utility.StringOperation.FormatString(Replace(Trim(myDetail.Descrizione), "'", "''")) & vbCrLf
            sSQL += "AND" & vbCrLf
            sSQL += "CODDIAMETROPRESA<>" & myDetail.CodDiametroPresa

            dvMyDati = iDB.GetDataView(sSQL)
            Try
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        Throw New Exception("La descrizione Inserita è già presente  in tabella !")
                    Next
                End If
                dvMyDati.Dispose()

                sSQL = "UPDATE TP_DIAMETROPRESA SET "
                sSQL += "CODTARIFFACONTATORE =" & Utility.StringOperation.FormatInt(myDetail.CodTariffaContatore) & vbCrLf
                sSQL += ",DESCRIZIONE =" & Utility.StringOperation.FormatString(UCase(myDetail.Descrizione)) & vbCrLf
                sSQL += ",NOTE=" & Utility.StringOperation.FormatString(UCase(myDetail.Note)) & vbCrLf
                sSQL += ",DIAMETROPRESA=" & Utility.StringOperation.FormatString(UCase(myDetail.DiametroPresa)) & vbCrLf
                sSQL += "WHERE" & vbCrLf
                sSQL += "CODDIAMETROPRESA=" & myDetail.CodDiametroPresa

                Try
                    If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                        Throw New Exception("errore in::" & sSQL)
                    End If
                Catch ex As SqlException
                    Throw ex
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBDiametroPresa.UpdateForzato.errore: ", ex)
                    Throw ex
                End Try
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBDiametroPresa.UpdateForzato.errore: ", ex)
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
