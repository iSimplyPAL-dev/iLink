Imports log4net
Imports OPENUtility
Imports System.IO
Imports Anagrafica.DLL
Imports Utility
''' <summary>
''' Classe per la gestione dei residenti
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class ClsAnagrafeResidenti
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ClsAnagrafeResidenti))
    '*** richiamando il ws ****
    'Private Stradario As New WsStradario.Stradario
    '*** ***
    '*** richiamando direttamente il servizio ***
    Dim TypeOfRI As Type = GetType(RemotingInterfaceOpenGovStradario.IRemotingInterfaceOpenGovStradario)
    Dim RemStradario As RemotingInterfaceOpenGovStradario.IRemotingInterfaceOpenGovStradario
    '*** ***


    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Ricerca i dati anagrafici partendo dal codice contribuente
    ''' </summary>
    ''' <param name="COD_CONTRIBUENTE">Codice contribuente</param>
    ''' <returns>Data set valorizzato con i dati anagrafici</returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[danielediperna]	01/04/2009	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Function GetDatiAnagrafici(ByVal COD_CONTRIBUENTE As String, ByVal ParametroEnv As String, ByVal user As String) As DataSet ', ByVal WFSessione As CreateSessione
        'Dim WFSessioneAnagrafica As CreateSessione
        'Dim strWFErrore As String
        Dim sSQL As String
        Dim objContext As HttpContext = HttpContext.Current
        Dim cmdMyCommand As SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim DsAnag As New DataSet

        'WFSessioneAnagrafica = New CreateSessione(ParametroEnv, user, "OPENGOVA")
        'If Not WFSessioneAnagrafica.CreaSessione(user, errore) Then
        '    Throw New Exception("getAnagrafeResidentiTributi ::" & "Errore durante l'apertura della sessione di WorkFlow")
        '    Exit Function
        'End If

        Try
            sSQL = "SELECT DISTINCT * "
            sSQL += " FROM ANAGRAFICA"
            sSQL += " WHERE DATA_FINE_VALIDITA IS NULL"
            sSQL += " AND COD_CONTRIBUENTE=@CONTRIBUENTE"

            cmdMyCommand = New SqlClient.SqlCommand
            cmdMyCommand.Connection = New SqlClient.SqlConnection(COSTANTValue.ConstSession.StringConnectionAnagrafica) 'WFSessioneAnagrafica.oSession.oAppDB.GetConnection()
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.CommandText = sSQL
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.AddWithValue("@CONTRIBUENTE", COD_CONTRIBUENTE)

            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(DsAnag)
            Return DsAnag 'WFSessioneAnagrafica.oSession.oAppDB.GetPrivateDataSet(SelectCommand)

        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.ClsAnagrafeResidenti.GetDatiAnagrafici.errore: ", ex)
            'Return Nothing
            Throw New Exception("Si è verificato un errore in GetDatiAnagrafici::" & ex.Message)
        Finally
            'WFSessioneAnagrafica.Kill()
        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sEnteImport"></param>
    ''' <param name="COGNOME"></param>
    ''' <param name="NOME"></param>
    ''' <param name="CF"></param>
    ''' <param name="nTrattato"></param>
    ''' <param name="nVSTributo"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Function getAnagrafeResidentiTributi(ByVal sEnteImport As String, ByVal COGNOME As String, ByVal NOME As String, ByVal CF As String, NumFamiglia As String, ByVal nTrattato As Integer, ByVal nVSTributo As Integer) As DataSet
        Dim sSQL As String = ""
        Dim myDataSet As DataSet
        Try
            'Valorizzo la connessione
            Dim oDbManagerRepository As New DBModel(COSTANTValue.ConstSession.DBType, COSTANTValue.ConstSession.StringConnectionAnagrafica)
            Using ctx As DBModel = oDbManagerRepository
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"sp_GetAnagResTributi", "IDENTE", "COGNOME", "NOME", "CF", "NUMFAMIGLIA", "ISTRATTATO", "VSTRIBUTO")
                myDataSet = ctx.GetDataSet(sSQL, "TBL", ctx.GetParam("IDENTE", sEnteImport) _
                        , ctx.GetParam("COGNOME", COGNOME) _
                        , ctx.GetParam("NOME", NOME) _
                        , ctx.GetParam("CF", CF) _
                        , ctx.GetParam("NUMFAMIGLIA", NumFamiglia) _
                        , ctx.GetParam("ISTRATTATO", nTrattato) _
                        , ctx.GetParam("VSTRIBUTO", nVSTributo)
                    )
                ctx.Dispose()
            End Using

            Return myDataSet
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.ClsAnagrafeResidenti.GetAnagrafeResidentiTributi.errore: ", ex)
            Return Nothing
        End Try
    End Function
    'Public Function getAnagrafeResidentiTributi(ByVal sEnteImport As String, ByVal COGNOME As String, ByVal NOME As String, ByVal CF As String, ByVal nTrattato As Integer, ByVal nVSTributo As Integer) As DataSet
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim myDataSet As New DataSet

    '    Try
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(COSTANTValue.ConstSession.StringConnectionAnagrafica)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.Parameters.Add("@IDENTE", SqlDbType.NVarChar).Value = sEnteImport
    '        cmdMyCommand.Parameters.Add("@COGNOME", SqlDbType.NVarChar).Value = COGNOME
    '        cmdMyCommand.Parameters.Add("@NOME", SqlDbType.NVarChar).Value = NOME
    '        cmdMyCommand.Parameters.Add("@CF", SqlDbType.NVarChar).Value = CF
    '        cmdMyCommand.Parameters.Add("@ISTRATTATO", SqlDbType.Int).Value = nTrattato
    '        cmdMyCommand.Parameters.Add("@VSTRIBUTO", SqlDbType.Int).Value = nVSTributo
    '        cmdMyCommand.CommandText = "sp_GetAnagResTributi"
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(myDataSet, "Create DataView")
    '        myAdapter.Dispose()
    '        Return myDataSet
    '    Catch ex As Exception
    '        Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.ClsAnagrafeResidenti.GetAnagrafeResidentiTributi.errore: ", ex)
    '        Return Nothing
    '    Finally
    '        cmdMyCommand.Connection.Close()
    '        cmdMyCommand.Dispose()
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="IdMovimento"></param>
    ''' <param name="nTrattato"></param>
    ''' <returns></returns>
    Public Function updateAnagrafeResidenti(ByVal IdMovimento As Integer, ByVal nTrattato As Integer) As Integer
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim myDataSet As New DataSet

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(COSTANTValue.ConstSession.StringConnectionAnagrafica)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.CommandText = "prc_ANAGRAFEMOVIMENTI_TRATTATO"
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDMovimento", SqlDbType.Int)).Value = IdMovimento
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TRATTATO", SqlDbType.Int)).Value = nTrattato
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            If cmdMyCommand.ExecuteNonQuery() < 0 Then
                'DEVO LOGGARE ERRORE
                Log.Debug("Si è verificato un errore ::updateAnagrafeResidenti::IDMOVIMENTO NON TRATTATO=" & IdMovimento)
            End If
            Return 1
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.ClsAnagrafeResidenti.updateAnagrafeResidenti.errore: ", ex)
            Return -1
        Finally
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nCodIndividuale"></param>
    ''' <param name="nCodContribuente"></param>
    ''' <returns></returns>
    Public Function SetResidentiVSTributi(ByVal nCodIndividuale As Integer, ByVal nCodContribuente As Integer) As Boolean
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(COSTANTValue.ConstSession.StringConnectionAnagrafica)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            'valorizzo i parameters:
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COD_INDIVIDUALE", SqlDbType.Int)).Value = nCodIndividuale
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COD_CONTRIBUENTE", SqlDbType.Int)).Value = nCodContribuente
            'Valorizzo il commandtext:
            cmdMyCommand.CommandText = "CL_ANAGRAFICA_ANAGRAFERESIDENTI_IU"
            'eseguo la query
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            cmdMyCommand.ExecuteNonQuery()

            Return True
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.ClsAnagrafeResidenti.SetResidentiVSTributi.errore: ", Err)
            Log.Debug("Si è verificato un errore in SetResidentiVSTributi::" & Err.Message & vbCrLf & "::COD_INDIVIDUALE::" & nCodIndividuale & "::COD_CONTRIBUENTE::" & nCodContribuente)
            Return False
        Finally
            cmdMyCommand.Connection.Close()
        End Try
    End Function
    '*** ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="DBType"></param>
    ''' <param name="myConnectionString"></param>
    ''' <param name="sEnte"></param>
    ''' <param name="COGNOME"></param>
    ''' <param name="NOME"></param>
    ''' <param name="COD_FISCALE"></param>
    ''' <returns></returns>
    Public Function SearchAnagrafica(DBType As String, ByVal myConnectionString As String, ByVal sEnte As String, ByVal COGNOME As String, ByVal NOME As String, ByVal COD_FISCALE As String) As DataSet
        Try
            Dim dsListaPersone As DataSet

            Dim oGA As New GestioneAnagrafica()

            dsListaPersone = oGA.GetListaPersone(-1, COGNOME, NOME, COD_FISCALE, "", sEnte, DBType, myConnectionString)
            If dsListaPersone.Tables(0).Rows.Count > 0 Then
                Return dsListaPersone
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.ClsAnagrafeResidenti.SearchAnagrafica.errore: ", ex)
            'Return Nothing
            Throw New Exception("Si è verificato un errore in GestioneAnagrafeResidenti::SearchAnagrafica::" & ex.Message)
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sEnteImport"></param>
    ''' <param name="ParametroEnv"></param>
    ''' <param name="user"></param>
    ''' <param name="objFlusso"></param>
    Public Sub GetFlussoAnagrafeResidenti(ByVal sEnteImport As String, ByVal ParametroEnv As String, ByVal user As String, ByRef objFlusso As OggettoFlussoAnagrafeResidenti) ', ByVal WFSessione As CreateSessione
        'Dim WFSessioneAnagrafica As CreateSessione
        'Dim errore As String
        Dim cmdMyCommand As SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim myDataSet As New DataSet

        Try
            'WFSessioneAnagrafica = New CreateSessione(ParametroEnv, user, "OPENGOVA")
            'If Not WFSessioneAnagrafica.CreaSessione(user, errore) Then
            '    Throw New Exception("getAnagrafeResidentiTributi ::" & "Errore durante l'apertura della sessione di WorkFlow")
            '    Exit Sub
            'End If

            cmdMyCommand = New SqlClient.SqlCommand
            cmdMyCommand.Connection = New SqlClient.SqlConnection(COSTANTValue.ConstSession.StringConnectionAnagrafica) 'WFSessioneAnagrafica.oSession.oAppDB.GetConnection()
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.Parameters.Add("@CODENTE", SqlDbType.NVarChar).Value = sEnteImport
            cmdMyCommand.CommandText = "sp_GetLastFlussoAnag"

            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(myDataSet, "Create DataView")

            If myDataSet.Tables(0).Rows.Count > 0 Then
                objFlusso.Esito = CStr(myDataSet.Tables(0).Rows(0)("Esito"))
                objFlusso.ID = CStr(myDataSet.Tables(0).Rows(0)("ID"))
                objFlusso.NOMEFILE = CStr(myDataSet.Tables(0).Rows(0)("NOMEFILE"))
                objFlusso.FINITO = CStr(myDataSet.Tables(0).Rows(0)("FINITO"))
            Else
                objFlusso.ID = 0
            End If


        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.ClsAnagrafeResidenti.GetFlussoAnagrafeResidenti.errore: ", ex)
            'Return Nothing
            Throw New Exception("Si è verificato un errore in GetFlussoAnagrafeResidenti::" & ex.Message)
        Finally
            'WFSessioneAnagrafica.Kill()
            myAdapter.Dispose()
            cmdMyCommand.Dispose()
        End Try

    End Sub

    '*** 20130128 - gestione numero occupanti per TARES ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sIdEnte"></param>
    ''' <param name="sParametroENV"></param>
    ''' <param name="sUsername"></param>
    ''' <param name="sIdentificativoApplicazione"></param>
    ''' <param name="bResNoTrib"></param>
    ''' <param name="bTribNoCN"></param>
    ''' <returns></returns>
    Public Function GetAnagResVSTributi(ByVal sIdEnte As String, ByVal sParametroENV As String, ByVal sUsername As String, ByVal sIdentificativoApplicazione As String, ByVal bResNoTrib As Boolean, ByVal bTribNoCN As Boolean) As DataSet
        Dim myDataSet As New DataSet
        Try
            'Dim WFSessione As CreateSessione
            'Dim WFErrore As String
            Dim cmdMyCommand As New SqlClient.SqlCommand
            Dim myAdapter As New SqlClient.SqlDataAdapter

            cmdMyCommand = New SqlClient.SqlCommand
            cmdMyCommand.Connection = New SqlClient.SqlConnection(COSTANTValue.ConstSession.StringConnectionOPENgov) 'WFSessioneAnagrafica.oSession.oAppDB.GetConnection()
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.CommandText = "SELECT COGNOME, NOME, CFPIVA"
            If bResNoTrib = True Then
                cmdMyCommand.CommandText += " FROM V_RESIDENTINOTRIBUTI"
            Else
                cmdMyCommand.CommandText += " FROM V_TARESSENZANC"
            End If
            cmdMyCommand.CommandText += " WHERE COD_ENTE=@ENTE"
            cmdMyCommand.CommandText += " ORDER BY COGNOME, NOME, CFPIVA"
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ENTE", SqlDbType.NVarChar)).Value = sIdEnte
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(myDataSet, "Create DataView")
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.ClsAnagrafeResidenti.GetAnagResVSTributi.errore: ", ex)
        End Try
        Return myDataSet
    End Function
    '*** ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myConnectionString"></param>
    ''' <param name="nIdContribuente"></param>
    ''' <returns></returns>
    Public Function GetFamigliaResidenti(ByVal myConnectionString As String, ByVal nIdContribuente As Integer) As DataView
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dtMyDati As New DataTable()
        Dim dvMyDati As New DataView

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_GetResidenti"
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = nIdContribuente
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(dtMyDati)
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.ClsAnagrafeResidenti.GetFamigliaResidenti.errore: ", ex)
        Finally
            myAdapter.Dispose()
            dtMyDati.Dispose()
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
        Return dtMyDati.DefaultView
    End Function
    '*** 201507 - Residenti da Tributi ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myStringConnection"></param>
    ''' <param name="CodEnte"></param>
    ''' <param name="IdContribuente"></param>
    ''' <returns></returns>
    Public Function SetResidentiFromTributi(ByVal myStringConnection As String, ByVal CodEnte As String, ByVal IdContribuente As Integer) As Boolean
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_ANAGRAFE_RESIDENTI_FROMTRIBUTI"
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COD_ENTE", SqlDbType.VarChar)).Value = CodEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_AZIONE", SqlDbType.DateTime)).Value = Now
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSO", SqlDbType.Int)).Value = -1
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODCONTRIBUENTE", SqlDbType.Int)).Value = IdContribuente
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            cmdMyCommand.ExecuteNonQuery()
            Return True
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.ClsAnagrafeResidenti.SetResidentiFromTributi.errore: ", ex)
            Return False
        Finally
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
    End Function
    '*** ***
End Class
''' <summary>
''' Definizione oggetto per anagrafe residenti
''' </summary>
Public Class OggettoAnagrafeResidenti
    Public COD_ENTE As String
    Public COD_INDIVIDUALE As Integer
    Public COGNOME As String
    Public NOME As String
    Public SESSO As String
    Public DATA_NASCITA As String
    Public DATA_MORTE As String
    Public LUOGO_NASCITA As String
    Public COD_FISCALE As String
    Public COD_VIA As Integer
    Public NUMERO As String
    Public LETTERA As String
    Public INTERNO As String
    Public NUMERO_FAMIGLIA As Integer
    Public CODICE_POSIZIONE_FAMIGLIA As Integer
    Public CODICE_AZIONE As Integer
    Public IDFLUSSO As Integer
End Class
''' <summary>
''' Definizione oggetto flusso anagrafe residenti
''' </summary>
Public Class OggettoFlussoAnagrafeResidenti
    Public ID As Integer
    Public NOMEFILE As String
    Public FINITO As Integer
    Public Operazione As String
    Public Esito As String
    Public ENTE As String
End Class

'Public Class FileAnagrafeResidenti
'    Private objAnagrafeResidenti() As OggettoAnagrafeResidenti
'End Class

''' <summary>
''' Classe per la consultazione vani
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class StradarioVani
    Private Shared Log As ILog = LogManager.GetLogger(GetType(StradarioVani))
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="TipoElaborazione"></param>
    ''' <param name="cod_ente"></param>
    ''' <returns></returns>
    Public Function selectStradarioVani(ByVal TipoElaborazione As String, ByVal cod_ente As String) As DataTable
        Dim dtMyDati As New DataTable
        Dim myDataReader As SqlClient.SqlDataReader
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim StringConnectionAnater As String
        Dim myTableName As String

        Try
            If Not (ConfigurationManager.AppSettings("connStrSQLANATER") Is Nothing) Then
                If TipoElaborazione = "S" Then
                    myTableName = "Modifiche_Stradario"
                Else
                    myTableName = "Modifiche_Composizione_UIU"
                End If

                StringConnectionAnater = ConfigurationManager.AppSettings("connStrSQLANATER")
                cmdMyCommand.CommandType = CommandType.Text
                cmdMyCommand.CommandTimeout = 0
                'Valorizzo la connessione
                cmdMyCommand.Connection = New SqlClient.SqlConnection(StringConnectionAnater)
                If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                    cmdMyCommand.Connection.Open()
                End If

                cmdMyCommand.CommandText = "SELECT * FROM @MYTABLE"
                cmdMyCommand.CommandText += " WHERE DATARECEPIMENTO IS NULL"
                cmdMyCommand.CommandText += " AND CODICEISTATCOMUNE=@IDENTE"

                'valorizzo i parameters:
                cmdMyCommand.Parameters.Clear()
                cmdMyCommand.Parameters.AddWithValue("@MYTABLE", myTableName)
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = cod_ente
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                myDataReader = cmdMyCommand.ExecuteReader
                dtMyDati.Load(myDataReader)
                Return dtMyDati
            Else
                Return Nothing
            End If
        Catch Ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.StradarioVani.selectStradarioVani.errore: ", Ex)
            Return Nothing
        Finally
            dtMyDati.Dispose()
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function
End Class