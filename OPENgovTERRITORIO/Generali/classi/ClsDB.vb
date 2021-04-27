Imports log4net
Imports DAL
Imports Utility
Imports OggettiComuniStrade
''' <summary>
''' Classe di interfacciamento connessione databse
''' </summary>
Public Class DBEngineFactory
    Private Shared Log As ILog = LogManager.GetLogger(GetType(DBEngineFactory))

    Public Overloads Shared Function GetDBEngine() As DBEngine
        Dim connectionString_ As String
        Dim dbProvider_ As DALEnum.eDBProvider
        Dim dbEngine_ As DBEngine
        'Log.Debug("GetDBEngine::valorizzo stringa connessione::"+connectionString_);
        If Not ConfigurationSettings.AppSettings("ConnectionTERRITORIO") Is Nothing Then
            connectionString_ = ConfigurationSettings.AppSettings("ConnectionTERRITORIO")
        Else
            connectionString_ = ""
        End If
        dbProvider_ = DALEnum.eDBProvider.SqlClient
        dbEngine_ = New DBEngine(connectionString_, dbProvider_)
        Return dbEngine_
    End Function

    Public Overloads Shared Function GetDBEngine(ByVal StringConnection As String) As DBEngine
        Dim dbProvider_ As DALEnum.eDBProvider
        Dim dbEngine_ As DBEngine
        dbProvider_ = DALEnum.eDBProvider.SqlClient
        dbEngine_ = New DBEngine(StringConnection, dbProvider_)
        Return dbEngine_
    End Function
End Class
''' <summary>
''' Classe di interrogazioni database
''' </summary>
Public Class ClsDB
    Inherits Ribes.OPENgov.Utilities.ClsDatabase
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(ClsDB))
    Private FncDate As New ModificaDate
    Private FncGen As New ClsUtilities
    Public Const TYPEOPERATION_INSERT As Integer = 0
    Public Const TYPEOPERATION_UPDATE As Integer = 1
    Public Const TYPEOPERATION_DELETE As Integer = 2

    'Private Function GetValParamCmd(ByVal MyCMD As SqlClient.SqlCommand) As String
    '    Dim sReturn As String
    '    Dim x As Integer

    '    For x = 0 To MyCMD.Parameters.Count - 1
    '        sReturn += MyCMD.Parameters(x).ParameterName + "="
    '        If MyCMD.Parameters(x).DbType = DbType.String Or MyCMD.Parameters(x).DbType = DbType.DateTime Then
    '            sReturn += "'" + MyCMD.Parameters(x).Value & "',"
    '        ElseIf MyCMD.Parameters(x).DbType = DbType.Double Then
    '            sReturn += MyCMD.Parameters(x).Value.ToString.Replace(",", ".") & ","
    '        Else
    '            sReturn += MyCMD.Parameters(x).Value & ","
    '        End If
    '    Next
    '    Return sReturn
    'End Function
#Region "SELECT"
    Public Function GetSQLGestioneFabbricati(ByVal dbEngine_ As DAL.DBEngine, ByVal oMyFabbricato As ObjFabbricato) As DAL.DBEngine
        Try
            'valorizzo i parameters
            dbEngine_.ClearParameters()
            dbEngine_.AddParameter("@COD_ENTE", oMyFabbricato.sIdEnte, ParameterDirection.Input)
            dbEngine_.AddParameter("@IDAGGFABBRICATO", oMyFabbricato.nIdFabbricato, ParameterDirection.Input)
            dbEngine_.AddParameter("@IDNEWFABBRICATO", oMyFabbricato.nIdFabbricato, ParameterDirection.Output)
            dbEngine_.AddParameter("@COD_STRADA", oMyFabbricato.nCodVia, ParameterDirection.Input)
            dbEngine_.AddParameter("@NOME_FABBRICATO", oMyFabbricato.sNomeFabbricato, ParameterDirection.Input)
            dbEngine_.AddParameter("@POSIZIONE_CIVICO", oMyFabbricato.sPosizioneCivico, ParameterDirection.Input)
            dbEngine_.AddParameter("@NUM_CIVICO", oMyFabbricato.nNumeroCivico, ParameterDirection.Input)
            dbEngine_.AddParameter("@ESPONENTE_CIVICO", oMyFabbricato.sEsponenteCivico, ParameterDirection.Input)
            dbEngine_.AddParameter("@FLAG_SN", CBool(oMyFabbricato.bSenzaNumero), ParameterDirection.Input)
            dbEngine_.AddParameter("@COD_TIPO_FABBRICATO", oMyFabbricato.sCodTipoFab, ParameterDirection.Input)
            dbEngine_.AddParameter("@FLAG_CONDOMINIO", CBool(oMyFabbricato.bCondominio), ParameterDirection.Input)
            dbEngine_.AddParameter("@COD_CONDOMINIO", oMyFabbricato.oCondominio.nIdCondominio, ParameterDirection.Input)
            dbEngine_.AddParameter("@FLAG_PRESENZA_CORTILE", CBool(oMyFabbricato.bCortile), ParameterDirection.Input)
            dbEngine_.AddParameter("@INFO_SEMINTERRATO", oMyFabbricato.sCodSeminterrato, ParameterDirection.Input)
            dbEngine_.AddParameter("@INFO_SOTTOTETTO", oMyFabbricato.sCodSottotetto, ParameterDirection.Input)
            dbEngine_.AddParameter("@NUM_PIANI", oMyFabbricato.nPiani, ParameterDirection.Input)
            dbEngine_.AddParameter("@NUM_CAMPANELLI", oMyFabbricato.nCampanelli, ParameterDirection.Input)
            dbEngine_.AddParameter("@NUM_ALLOGGI", oMyFabbricato.nAlloggi, ParameterDirection.Input)
            dbEngine_.AddParameter("@NUM_AUTORIMESSE", oMyFabbricato.nAutorimesse, ParameterDirection.Input)
            dbEngine_.AddParameter("@NUM_DEPOSITI", oMyFabbricato.nDepositi, ParameterDirection.Input)
            dbEngine_.AddParameter("@NUM_TETTOIE", oMyFabbricato.nTettoie, ParameterDirection.Input)
            dbEngine_.AddParameter("@NUM_NEGOZI", oMyFabbricato.nNegozi, ParameterDirection.Input)
            dbEngine_.AddParameter("@NUM_LABORATORI", oMyFabbricato.nLaboratori, ParameterDirection.Input)
            dbEngine_.AddParameter("@NUM_UFFICI", oMyFabbricato.nUffici, ParameterDirection.Input)
            dbEngine_.AddParameter("@NUM_RESIDENTI", 0, ParameterDirection.Input)
            dbEngine_.AddParameter("@ID_UTENZA", 0, ParameterDirection.Input)
            dbEngine_.AddParameter("@ID_REFERENTE", 0, ParameterDirection.Input)
            dbEngine_.AddParameter("@CAT_CATASTALE_TEORICA", oMyFabbricato.sCatCatastale, ParameterDirection.Input)
            dbEngine_.AddParameter("@STATO_FABBRICATO", "", ParameterDirection.Input)
            'dbEngine_.AddParameter("@COD_LINK", "", ParameterDirection.Input)
            dbEngine_.AddParameter("@COD_STATO_CONSERVAZIONE", oMyFabbricato.nStatoConservazione, ParameterDirection.Input)
            dbEngine_.AddParameter("@NOTE_DATI_GENERALI", oMyFabbricato.sNote, ParameterDirection.Input)
            dbEngine_.AddParameter("@COD_AREA_A", oMyFabbricato.nAreeAmministrative, ParameterDirection.Input)
            dbEngine_.AddParameter("@COD_AREA_U", oMyFabbricato.nAreeUrbanistiche, ParameterDirection.Input)
            dbEngine_.AddParameter("@COD_ZONA", oMyFabbricato.nZona, ParameterDirection.Input)
            dbEngine_.AddParameter("@COD_MICROZONA", oMyFabbricato.nMicrozona, ParameterDirection.Input)
            dbEngine_.AddParameter("@COD_ECOGRAFICO", 0, ParameterDirection.Input)
            dbEngine_.AddParameter("@COD_STRADA_1", 0, ParameterDirection.Input)
            dbEngine_.AddParameter("@NUM_CIVICO_1", "", ParameterDirection.Input)
            dbEngine_.AddParameter("@NUMERO_CIVICO", 0, ParameterDirection.Input)
            dbEngine_.AddParameter("@STATO_ASCENSORE", 0, ParameterDirection.Input)
            dbEngine_.AddParameter("@STATO_BARRIERE", 0, ParameterDirection.Input)
            dbEngine_.AddParameter("@STATO_AMIANTO", 0, ParameterDirection.Input)
            dbEngine_.AddParameter("@COD_TIPO_COPERTURA", 0, ParameterDirection.Input)
            dbEngine_.AddParameter("@STATO_INTERESSE_STORICO", 0, ParameterDirection.Input)
            dbEngine_.AddParameter("@FLAG_SOGGETTO_CALAMITA", 0, ParameterDirection.Input)
            dbEngine_.AddParameter("@NOTE_CARATTERISTICHE", "", ParameterDirection.Input)
            dbEngine_.AddParameter("@DATA_COSTRUZIONE", "", ParameterDirection.Input)
            dbEngine_.AddParameter("@DATA_ULTIMA_RISTRUTTURAZIONE", "", ParameterDirection.Input)
            dbEngine_.AddParameter("@PROVENIENZA_CATASTO", "", ParameterDirection.Input)
            dbEngine_.AddParameter("@FABBRICATO_DEFINITO", "", ParameterDirection.Input)
            dbEngine_.AddParameter("@TARGA", 0, ParameterDirection.Input)

            Return dbEngine_
        Catch Err As Exception
            'Dim sValParametri As String = GetValParamCmd(dbEngine_)
            Log.Error("Si è verificato un errore in ClsDB::GetSQLFabbricato::" & Err.Message)           ' & vbCrLf & "SQL::" & dbEngine_.CommandText & vbCrLf & " VALUES " & sValParametri)
            Return Nothing
        End Try
    End Function

    Public Function GetSQLGestioneStrade(myStringConnection As String, ByVal nTypeOperation As Integer, ByVal sIdEnte As String, ByVal nIdVia As Integer, ByVal nIdToponimo As Integer, ByVal nIdFrazione As Integer, ByVal sDescrizione As String) As DataTable
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dtMyDati As New DataTable()
        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            Select Case nTypeOperation
                Case TYPEOPERATION_INSERT, TYPEOPERATION_UPDATE
                    cmdMyCommand.CommandText = "T_STRADE_IU"
                    'valorizzo i parameters
                    cmdMyCommand.Parameters.Clear()
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_TIPO_VIA", SqlDbType.Int)).Value = nIdToponimo
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DESCRIZIONE", SqlDbType.NVarChar)).Value = sDescrizione
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_FRAZIONE", SqlDbType.Int)).Value = nIdFrazione
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDVIADEMOGRAFICO", SqlDbType.Int)).Value = 0
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_VIA", SqlDbType.Int)).Value = nIdVia
                Case TYPEOPERATION_DELETE
                    cmdMyCommand.CommandText = "T_STRADE_D"
                    'valorizzo i parameters
                    cmdMyCommand.Parameters.Clear()
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_VIA", SqlDbType.Int)).Value = nIdVia
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
            End Select
            myAdapter.SelectCommand = cmdMyCommand
            myAdapter.Fill(dtMyDati)
            myAdapter.Dispose()

            Return dtMyDati
        Catch Err As Exception
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Log.Error("Si è verificato un errore in ClsDB::GetSQLGestioneStrade::" & Err.Message)
            Return New DataTable()
        Finally
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function

    Public Function GetSQLGestioneDettaglioStrade(myStringConnection As String, ByVal nTypeOperation As Integer, ByVal nIdDettaglio As Integer, ByVal nIdVia As Integer, ByVal sExDescrizione As String, ByVal sCAP As String, ByVal nLunghezza As Double, ByVal nLarghezza As Double, ByVal sNote As String) As DataTable
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dtMyDati As New DataTable()
        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            Select Case nTypeOperation
                Case TYPEOPERATION_INSERT, TYPEOPERATION_UPDATE
                    cmdMyCommand.CommandText = "DETTAGLIO_STRADE_IU"
                Case TYPEOPERATION_DELETE
                    cmdMyCommand.CommandText = "DETTAGLIO_STRADE_D"
            End Select
            'VALORIZZO I PARAMETERS
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDDETTAGLIOSTRADA", SqlDbType.Int)).Value = nIdDettaglio
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDVIA", SqlDbType.Int)).Value = nIdVia
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@EX_DENOMINAZIONE", SqlDbType.NVarChar)).Value = sExDescrizione
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CAP", SqlDbType.NVarChar)).Value = sCAP
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@LUNGHEZZA", SqlDbType.Float)).Value = nLunghezza
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@LARGHEZZA", SqlDbType.Float)).Value = nLarghezza
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOTE", SqlDbType.NVarChar)).Value = sNote
            myAdapter.SelectCommand = cmdMyCommand
            myAdapter.Fill(dtMyDati)
            myAdapter.Dispose()

            Return dtMyDati
        Catch Err As Exception
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Log.Error("Si è verificato un errore in ClsDB::GetSQLGestioneDettaglioStrade::" & Err.Message)
            Return New DataTable()
        Finally
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function

    Public Function GetSQLRicercaStrade(myStringConnection As String, ByVal sIdEnte As String, ByVal nIdVia As Integer, ByVal nIdToponimo As Integer, ByVal nIdFrazione As Integer, ByVal sDescrizione As String, ByVal sCAP As String) As DataTable
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dtMyDati As New DataTable()
        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.CommandText = "SELECT *"
            cmdMyCommand.CommandText += " FROM V_STRADE STRADE"
            cmdMyCommand.CommandText += " WHERE (COD_ENTE=@IDENTE)"
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
            If nIdVia > 0 Then
                cmdMyCommand.CommandText += " AND (ID_VIA=@IDVIA)"
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDVIA", SqlDbType.Int)).Value = nIdVia
            End If
            If nIdToponimo > 0 Then
                cmdMyCommand.CommandText += " AND (ID_TOPONIMO=@IDTOPONIMO)"
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTOPONIMO", SqlDbType.Int)).Value = nIdToponimo
            End If
            If nIdFrazione > 0 Then
                cmdMyCommand.CommandText += " AND (ID_FRAZIONE=@IDFRAZIONE)"
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFRAZIONE", SqlDbType.Int)).Value = nIdFrazione
            End If
            If sDescrizione <> "" Then
                cmdMyCommand.CommandText += " AND (DESCRIZIONE_VIA LIKE @STRADA)"
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@STRADA", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(sDescrizione) & "%"
            End If
            If sCAP <> "" Then
                cmdMyCommand.CommandText += " AND (CAP=@CAP)"
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CAP", SqlDbType.NVarChar)).Value = sCAP
            End If
            cmdMyCommand.CommandText += " ORDER BY TOPONIMO, DESCRIZIONE_VIA, FRAZIONE"
            myAdapter.SelectCommand = cmdMyCommand
            myAdapter.Fill(dtMyDati)
            myAdapter.Dispose()

            Return dtMyDati
        Catch Err As Exception
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Log.Error("Si è verificato un errore in ClsDB::GetSQLRicercaStrade::" & Err.Message)
            Return New DataTable()
        Finally
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function
    Public Function GetSQLRicercaFabbricato(myStringConnection As String, ByVal sIdEnte As String, ByVal nIdVia As Integer, ByVal bSenzaNumero As Boolean, ByVal sPosizione As String, ByVal nCivico As Integer, ByVal sEsponente As String) As SqlClient.SqlDataReader
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dtMyDati As New DataTable()

        Try
            'prelevo i dati della testata
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.CommandText = "SELECT *"
            cmdMyCommand.CommandText += " FROM V_RICFABBRICATI"
            cmdMyCommand.CommandText += " WHERE (COD_ENTE=@IDENTE)"
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
            If nIdVia > 0 Then
                cmdMyCommand.CommandText += " AND (COD_STRADA=@IDVIA)"
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDVIA", SqlDbType.Int)).Value = nIdVia
            End If
            If bSenzaNumero = True Then
                cmdMyCommand.CommandText += " AND (FLAG_SN=@SN)"
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SN", SqlDbType.Bit)).Value = bSenzaNumero
            End If
            If sPosizione <> "" Then
                cmdMyCommand.CommandText += " AND (POSIZIONE_CIVICO=@POSIZIONE)"
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@POSIZIONE", SqlDbType.NVarChar)).Value = sPosizione
            End If
            If nCivico > 0 Then
                cmdMyCommand.CommandText += " AND (NUM_CIVICO=@CIVICO)"
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CIVICO", SqlDbType.Int)).Value = nCivico
            End If
            If sEsponente <> "" Then
                cmdMyCommand.CommandText += " AND (ESPONENTE_CIVICO=@ESPONENTE)"
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ESPONENTE", SqlDbType.NVarChar)).Value = sEsponente
            End If

            Return cmdMyCommand.ExecuteReader
        Catch Err As Exception
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Log.Error("Si è verificato un errore in ClsDB::GetSQLRicercaFabbricato::" & Err.Message)
            Return Nothing
        End Try
    End Function

    Public Function GetSQLFabbricato(myStringConnection As String, ByVal sIdEnte As String, ByVal nIdStradario As Integer, ByVal nIdFab As Integer, ByVal nIdVia As Integer, ByVal nCivico As Integer, ByVal sPosizione As String, ByVal sEsponente As String, ByVal nFoglio As Integer, ByVal sNumero As String, ByVal nSubalterno As Integer) As DataTable
        Dim sSuffissoTab As String = ""
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dtMyDati As New DataTable()

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.CommandText = "SELECT *"
            cmdMyCommand.CommandText += " FROM V_FABBRICATI" + sSuffissoTab
            cmdMyCommand.CommandText += " WHERE (COD_ENTE=@IDENTE)"
            cmdMyCommand.CommandText += " AND (@IDFAB<=0 OR COD_FABBRICATO=@IDFAB)"
            cmdMyCommand.CommandText += " AND (@IDSTRADARIO<=0 OR COD_STRADARIO=@IDSTRADARIO)"
            cmdMyCommand.CommandText += " AND (@IDVIA<=0 OR COD_STRADA=@IDVIA)"
            cmdMyCommand.CommandText += " AND (@CIVICO<=0 OR NUM_CIVICO=@CIVICO)"
            cmdMyCommand.CommandText += " AND (@POSIZIONE='' OR POSIZIONE_CIVICO=@POSIZIONE)"
            cmdMyCommand.CommandText += " AND (@ESPONENTE='' OR ESPONENTE_CIVICO=@ESPONENTE)"
            cmdMyCommand.CommandText += " AND (@FOGLIO<=0 OR FOGLIO=@FOGLIO)"
            cmdMyCommand.CommandText += " AND (@NUMERO='' OR NUMERO=@NUMERO)"
            cmdMyCommand.CommandText += " AND (@SUB<=0 OR SUB=@SUB)"

            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFAB", SqlDbType.Int)).Value = nIdFab
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDSTRADARIO", SqlDbType.Int)).Value = nIdStradario
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDVIA", SqlDbType.Int)).Value = nIdVia
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CIVICO", SqlDbType.Int)).Value = nCivico
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@POSIZIONE", SqlDbType.NVarChar)).Value = sPosizione
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ESPONENTE", SqlDbType.NVarChar)).Value = sEsponente
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FOGLIO", SqlDbType.Int)).Value = nFoglio
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO", SqlDbType.NVarChar)).Value = sNumero
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SUB", SqlDbType.Int)).Value = nSubalterno

            myAdapter.SelectCommand = cmdMyCommand
            myAdapter.Fill(dtMyDati)
            myAdapter.Dispose()
            Return dtMyDati
        Catch Err As Exception
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Log.Error("Si è verificato un errore in ClsDB::GetSQLFabbricato::" & Err.Message)
            Return Nothing
        End Try
    End Function

    Public Function GetSQLGestioneFabbricato(ByVal nTypeOperation As Integer, ByVal oMyFab As ObjFabbricato) As SqlClient.SqlCommand
        'Try
        '	'VALORIZZO IL COMMANDTEXT;
        '	cmdMyCommand.Parameters.Clear()
        '	Select Case nTypeOperation
        '		Case TYPEOPERATION_INSERT
        '			cmdMyCommand.CommandType = CommandType.Text
        '			cmdMyCommand.CommandText = "INSERT INTO FABBRICATI (COD_ENTE"
        '			cmdMyCommand.CommandText += ",COD_STRADA"
        '			cmdMyCommand.CommandText += ", POSIZIONE_CIVICO, ESPONENTE_CIVICO"
        '			cmdMyCommand.CommandText += ",FLAG_SN, FLAG_PRESENZA_CORTILE"
        '			cmdMyCommand.CommandText += ",FLAG_CONDOMINIO, NOME_FABBRICATO"
        '			cmdMyCommand.CommandText += ",CAT_CATASTALE_TEORICA, NOTE_DATI_GENERALI"
        '			If oMyFab.nNumeroCivico > 0 Then
        '				cmdMyCommand.CommandText += ", NUM_CIVICO"
        '			End If
        '			If oMyFab.nAreeAmministrative > 0 Then
        '				cmdMyCommand.CommandText += ",COD_AREA_A"
        '			End If
        '			If oMyFab.nAreeUrbanistiche > 0 Then
        '				cmdMyCommand.CommandText += ", COD_AREA_U"
        '			End If
        '			If oMyFab.nZona > 0 Then
        '				cmdMyCommand.CommandText += ", COD_ZONA"
        '			End If
        '			If oMyFab.nMicrozona > 0 Then
        '				cmdMyCommand.CommandText += ", COD_MICROZONA"
        '			End If
        '			If oMyFab.sCodTipoFab <> "-1" Then
        '				cmdMyCommand.CommandText += ",COD_TIPO_FABBRICATO"
        '			End If
        '			If oMyFab.sCodSeminterrato <> "-1" Then
        '				cmdMyCommand.CommandText += ",INFO_SEMINTERRATO"
        '			End If
        '			If oMyFab.sCodSottotetto <> "-1" Then
        '				cmdMyCommand.CommandText += ", INFO_SOTTOTETTO"
        '			End If
        '			If oMyFab.nStatoConservazione > 0 Then
        '				cmdMyCommand.CommandText += ", COD_STATO_CONSERVAZIONE"
        '			End If
        '			If oMyFab.nAlloggi > 0 Then
        '				cmdMyCommand.CommandText += ",NUM_ALLOGGI"
        '			End If
        '			If oMyFab.nCampanelli > 0 Then
        '				cmdMyCommand.CommandText += ", NUM_CAMPANELLI"
        '			End If
        '			If oMyFab.nPiani > 0 Then
        '				cmdMyCommand.CommandText += ", NUM_PIANI"
        '			End If
        '			If oMyFab.nNegozi > 0 Then
        '				cmdMyCommand.CommandText += ", NUM_NEGOZI"
        '			End If
        '			If oMyFab.nDepositi > 0 Then
        '				cmdMyCommand.CommandText += ", NUM_DEPOSITI"
        '			End If
        '			If oMyFab.nLaboratori > 0 Then
        '				cmdMyCommand.CommandText += ", NUM_LABORATORI"
        '			End If
        '			If oMyFab.nAutorimesse > 0 Then
        '				cmdMyCommand.CommandText += ", NUM_AUTORIMESSE"
        '			End If
        '			If oMyFab.nUffici > 0 Then
        '				cmdMyCommand.CommandText += ", NUM_UFFICI"
        '			End If
        '			If oMyFab.nTettoie > 0 Then
        '				cmdMyCommand.CommandText += ", NUM_TETTOIE"
        '			End If
        '			If oMyFab.oCondominio.nIdCondominio > 0 Then
        '				cmdMyCommand.CommandText += ", COD_CONDOMINIO"
        '			End If
        '			'cmdMyCommand.CommandText += ",STATO_INTERESSE_STORICO, COD_TIPO_COPERTURA"
        '			'cmdMyCommand.CommandText += ",STATO_AMIANTO, STATO_ASCENSORE, STATO_BARRIERE, NOTE_CARATTERISTICHE, FLAG_SOGGETTO_CALAMITA"
        '			cmdMyCommand.CommandText += ")"
        '			cmdMyCommand.CommandText += " VALUES(@IDENTE"
        '			cmdMyCommand.CommandText += ",@IDVIA"
        '			cmdMyCommand.CommandText += ",@POSIZIONE,@ESPONENTE"
        '			cmdMyCommand.CommandText += ",@SENZANUMERO,@HASCORTILE"
        '			cmdMyCommand.CommandText += ",@ISCONDOMINIO"
        '			cmdMyCommand.CommandText += ",@NOMEFABBRICATO"
        '			cmdMyCommand.CommandText += ",@CATASTALETEORICA"
        '			cmdMyCommand.CommandText += ",@NOTE"
        '			If oMyFab.nNumeroCivico > 0 Then
        '				cmdMyCommand.CommandText += ",@CIVICO"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CIVICO", SqlDbType.Int)).Value = oMyFab.nNumeroCivico
        '			End If
        '			If oMyFab.nAreeAmministrative > 0 Then
        '				cmdMyCommand.CommandText += ",@AREAAMMINISTRATIVA"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@AREAAMMINISTRATIVA", SqlDbType.Int)).Value = oMyFab.nAreeAmministrative
        '			End If
        '			If oMyFab.nAreeUrbanistiche > 0 Then
        '				cmdMyCommand.CommandText += ",@AREAURBANISTICA"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@AREAURBANISTICA", SqlDbType.Int)).Value = oMyFab.nAreeUrbanistiche
        '			End If
        '			If oMyFab.nZona > 0 Then
        '				cmdMyCommand.CommandText += ",@ZONA"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ZONA", SqlDbType.Int)).Value = oMyFab.nZona
        '			End If
        '			If oMyFab.nMicrozona > 0 Then
        '				cmdMyCommand.CommandText += ",@MICROZONA"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@MICROZONA", SqlDbType.Int)).Value = oMyFab.nMicrozona
        '			End If
        '			If oMyFab.sCodTipoFab <> "-1" Then
        '				cmdMyCommand.CommandText += ",@TIPOFABBRICATO"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPOFABBRICATO", SqlDbType.NVarChar)).Value = oMyFab.sCodTipoFab
        '			End If
        '			If oMyFab.sCodSeminterrato <> "-1" Then
        '				cmdMyCommand.CommandText += ",@SEMINTERRATO"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SEMINTERRATO", SqlDbType.NVarChar)).Value = oMyFab.sCodSeminterrato
        '			End If
        '			If oMyFab.sCodSottotetto <> "-1" Then
        '				cmdMyCommand.CommandText += ",@SOTTOTETTO"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SOTTOTETTO", SqlDbType.NVarChar)).Value = oMyFab.sCodSottotetto
        '			End If
        '			If oMyFab.nStatoConservazione > 0 Then
        '				cmdMyCommand.CommandText += ",@STATOCONSERVAZIONE"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@STATOCONSERVAZIONE", SqlDbType.Int)).Value = oMyFab.nStatoConservazione
        '			End If
        '			If oMyFab.nAlloggi > 0 Then
        '				cmdMyCommand.CommandText += ",@NALLOGGI"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NALLOGGI", SqlDbType.Int)).Value = oMyFab.nAlloggi
        '			End If
        '			If oMyFab.nCampanelli > 0 Then
        '				cmdMyCommand.CommandText += ",@NCAMPANELLI"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NCAMPANELLI", SqlDbType.Int)).Value = oMyFab.nCampanelli
        '			End If
        '			If oMyFab.nPiani > 0 Then
        '				cmdMyCommand.CommandText += ",@NPIANI"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NPIANI", SqlDbType.Int)).Value = oMyFab.nPiani
        '			End If
        '			If oMyFab.nNegozi > 0 Then
        '				cmdMyCommand.CommandText += ",@NNEGOZI"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NNEGOZI", SqlDbType.Int)).Value = oMyFab.nNegozi
        '			End If
        '			If oMyFab.nDepositi > 0 Then
        '				cmdMyCommand.CommandText += ",@NDEPOSITI"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NDEPOSITI", SqlDbType.Int)).Value = oMyFab.nDepositi
        '			End If
        '			If oMyFab.nLaboratori > 0 Then
        '				cmdMyCommand.CommandText += ",@NLABORATORI"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NLABORATORI", SqlDbType.Int)).Value = oMyFab.nLaboratori
        '			End If
        '			If oMyFab.nAutorimesse > 0 Then
        '				cmdMyCommand.CommandText += ",@NAUTORIMESSE"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NAUTORIMESSE", SqlDbType.Int)).Value = oMyFab.nAutorimesse
        '			End If
        '			If oMyFab.nUffici > 0 Then
        '				cmdMyCommand.CommandText += ",@NUFFICI"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUFFICI", SqlDbType.Int)).Value = oMyFab.nUffici
        '			End If
        '			If oMyFab.nTettoie > 0 Then
        '				cmdMyCommand.CommandText += ",@NTETTOIE"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NTETTOIE", SqlDbType.Int)).Value = oMyFab.nTettoie
        '			End If
        '			If oMyFab.oCondominio.nIdCondominio > 0 Then
        '				cmdMyCommand.CommandText += ",@IDCONDOMINIO"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONDOMINIO", SqlDbType.Int)).Value = oMyFab.oCondominio.nIdCondominio
        '			End If
        '			'cmdMyCommand.CommandText += ",@INTERESSESTORICO,@TIPOCOPERTURA"
        '			'cmdMyCommand.CommandText += ",@STATOAMIANTO,@STATOASCENSORE,@STATOBARRIERE,@NOTECARATTERISTICHE,@ISSOGGETTOCALAMITA"
        '			cmdMyCommand.CommandText += " )"
        '			cmdMyCommand.CommandText += " SELECT @@IDENTITY AS COD_FABBRICATO"
        '			cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oMyFab.sIdEnte
        '			cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDVIA", SqlDbType.Int)).Value = oMyFab.nCodVia
        '			cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@POSIZIONE", SqlDbType.NVarChar)).Value = oMyFab.sPosizioneCivico
        '			cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ESPONENTE", SqlDbType.NVarChar)).Value = oMyFab.sEsponenteCivico
        '			cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SENZANUMERO", SqlDbType.Bit)).Value = oMyFab.bSenzaNumero
        '			cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@HASCORTILE", SqlDbType.Bit)).Value = oMyFab.bCortile
        '			cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ISCONDOMINIO", SqlDbType.Bit)).Value = oMyFab.bCondominio
        '			cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOMEFABBRICATO", SqlDbType.NVarChar)).Value = oMyFab.sNomeFabbricato
        '			cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CATASTALETEORICA", SqlDbType.NVarChar)).Value = oMyFab.sCatCatastale
        '			cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOTE", SqlDbType.NVarChar)).Value = oMyFab.sNote

        '		Case TYPEOPERATION_UPDATE
        '			cmdMyCommand.CommandText = "UPDATE FABBRICATI SET"
        '			cmdMyCommand.CommandText += " COD_ENTE=@IDENTE"
        '			cmdMyCommand.CommandText += ",COD_STRADA=@IDVIA"
        '			cmdMyCommand.CommandText += ",POSIZIONE_CIVICO=@POSIZIONE, ESPONENTE_CIVICO=@ESPONENTE"
        '			cmdMyCommand.CommandText += ",FLAG_SN=@SENZANUMERO, FLAG_PRESENZA_CORTILE=@HASCORTILE"
        '			cmdMyCommand.CommandText += ",FLAG_CONDOMINIO=@ISCONDOMINIO, NOME_FABBRICATO=@NOMEFABBRICATO"
        '			cmdMyCommand.CommandText += ",CAT_CATASTALE_TEORICA=@CATASTALETEORICA, NOTE_DATI_GENERALI=@NOTE"
        '			'cmdMyCommand.CommandText += ",STATO_INTERESSE_STORICO, COD_TIPO_COPERTURA"
        '			'cmdMyCommand.CommandText += ",STATO_AMIANTO, STATO_ASCENSORE, STATO_BARRIERE, NOTE_CARATTERISTICHE, FLAG_SOGGETTO_CALAMITA"
        '			If oMyFab.nNumeroCivico > 0 Then
        '				cmdMyCommand.CommandText += ", NUM_CIVICO=@CIVICO"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CIVICO", SqlDbType.Int)).Value = oMyFab.nNumeroCivico
        '			End If
        '			If oMyFab.nAreeAmministrative > 0 Then
        '				cmdMyCommand.CommandText += ",COD_AREA_A=@AREAAMMINISTRATIVA"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@AREAAMMINISTRATIVA", SqlDbType.Int)).Value = oMyFab.nAreeAmministrative
        '			End If
        '			If oMyFab.nAreeUrbanistiche > 0 Then
        '				cmdMyCommand.CommandText += ", COD_AREA_U=@AREAURBANISTICA"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@AREAURBANISTICA", SqlDbType.Int)).Value = oMyFab.nAreeUrbanistiche
        '			End If
        '			If oMyFab.nZona > 0 Then
        '				cmdMyCommand.CommandText += ", COD_ZONA=@ZONA"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ZONA", SqlDbType.Int)).Value = oMyFab.nZona
        '			End If
        '			If oMyFab.nMicrozona > 0 Then
        '				cmdMyCommand.CommandText += ", COD_MICROZONA=@MICROZONA"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@MICROZONA", SqlDbType.Int)).Value = oMyFab.nMicrozona
        '			End If
        '			If oMyFab.sCodTipoFab <> "-1" Then
        '				cmdMyCommand.CommandText += ",COD_TIPO_FABBRICATO=@TIPOFABBRICATO"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPOFABBRICATO", SqlDbType.NVarChar)).Value = oMyFab.sCodTipoFab
        '			End If
        '			If oMyFab.sCodSeminterrato <> "-1" Then
        '				cmdMyCommand.CommandText += ",INFO_SEMINTERRATO=@SEMINTERRATO"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SEMINTERRATO", SqlDbType.NVarChar)).Value = oMyFab.sCodSeminterrato
        '			End If
        '			If oMyFab.sCodSottotetto <> "-1" Then
        '				cmdMyCommand.CommandText += ", INFO_SOTTOTETTO=@SOTTOTETTO"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SOTTOTETTO", SqlDbType.NVarChar)).Value = oMyFab.sCodSottotetto
        '			End If
        '			If oMyFab.nStatoConservazione > 0 Then
        '				cmdMyCommand.CommandText += ", COD_STATO_CONSERVAZIONE=@STATOCONSERVAZIONE"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@STATOCONSERVAZIONE", SqlDbType.Int)).Value = oMyFab.nStatoConservazione
        '			End If
        '			If oMyFab.nAlloggi > 0 Then
        '				cmdMyCommand.CommandText += ",NUM_ALLOGGI=@NALLOGGI"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NALLOGGI", SqlDbType.Int)).Value = oMyFab.nAlloggi
        '			End If
        '			If oMyFab.nCampanelli > 0 Then
        '				cmdMyCommand.CommandText += ", NUM_CAMPANELLI=@NCAMPANELLI"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NCAMPANELLI", SqlDbType.Int)).Value = oMyFab.nCampanelli
        '			End If
        '			If oMyFab.nPiani > 0 Then
        '				cmdMyCommand.CommandText += ", NUM_PIANI=@NPIANI"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NPIANI", SqlDbType.Int)).Value = oMyFab.nPiani
        '			End If
        '			If oMyFab.nNegozi > 0 Then
        '				cmdMyCommand.CommandText += ", NUM_NEGOZI=@NNEGOZI"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NNEGOZI", SqlDbType.Int)).Value = oMyFab.nNegozi
        '			End If
        '			If oMyFab.nDepositi > 0 Then
        '				cmdMyCommand.CommandText += ", NUM_DEPOSITI=@NDEPOSITI"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NDEPOSITI", SqlDbType.Int)).Value = oMyFab.nDepositi
        '			End If
        '			If oMyFab.nLaboratori > 0 Then
        '				cmdMyCommand.CommandText += ", NUM_LABORATORI=@NLABORATORI"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NLABORATORI", SqlDbType.Int)).Value = oMyFab.nLaboratori
        '			End If
        '			If oMyFab.nAutorimesse > 0 Then
        '				cmdMyCommand.CommandText += ", NUM_AUTORIMESSE=@NAUTORIMESSE"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NAUTORIMESSE", SqlDbType.Int)).Value = oMyFab.nAutorimesse
        '			End If
        '			If oMyFab.nUffici > 0 Then
        '				cmdMyCommand.CommandText += ", NUM_UFFICI=@NUFFICI"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUFFICI", SqlDbType.Int)).Value = oMyFab.nUffici
        '			End If
        '			If oMyFab.nTettoie > 0 Then
        '				cmdMyCommand.CommandText += ", NUM_TETTOIE=@NTETTOIE"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NTETTOIE", SqlDbType.Int)).Value = oMyFab.nTettoie
        '			End If
        '			If oMyFab.oCondominio.nIdCondominio > 0 Then
        '				cmdMyCommand.CommandText += ", COD_CONDOMINIO=@IDCONDOMINIO"
        '				cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONDOMINIO", SqlDbType.Int)).Value = oMyFab.oCondominio.nIdCondominio
        '			End If
        '			'cmdMyCommand.CommandText += ",@INTERESSESTORICO,@TIPOCOPERTURA"
        '			'cmdMyCommand.CommandText += ",@STATOAMIANTO,@STATOASCENSORE,@STATOBARRIERE,@NOTECARATTERISTICHE,@ISSOGGETTOCALAMITA"
        '			cmdMyCommand.CommandText += " WHERE (COD_FABBRICATO=@IDFABBRICATO)"
        '			cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFABBRICATO", SqlDbType.Int)).Value = oMyFab.iCodFabbricatoReale
        '			cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oMyFab.sIdEnte
        '			cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDVIA", SqlDbType.Int)).Value = oMyFab.nCodVia
        '			cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@POSIZIONE", SqlDbType.NVarChar)).Value = oMyFab.sPosizioneCivico
        '			cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ESPONENTE", SqlDbType.NVarChar)).Value = oMyFab.sEsponenteCivico
        '			cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SENZANUMERO", SqlDbType.Bit)).Value = oMyFab.bSenzaNumero
        '			cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@HASCORTILE", SqlDbType.Bit)).Value = oMyFab.bCortile
        '			cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ISCONDOMINIO", SqlDbType.Bit)).Value = oMyFab.bCondominio
        '			cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOMEFABBRICATO", SqlDbType.NVarChar)).Value = oMyFab.sNomeFabbricato
        '			cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CATASTALETEORICA", SqlDbType.NVarChar)).Value = oMyFab.sCatCatastale
        '			cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOTE", SqlDbType.NVarChar)).Value = oMyFab.sNote
        '		Case TYPEOPERATION_DELETE
        '	End Select

        '	Return cmdMyCommand
        'Catch Err As Exception
        '	Dim sValParametri As String = GetValParamCmd(cmdMyCommand)
        '	Log.Error("Si è verificato un errore in ClsDB::GetSQLGestioneFabbricato::" & Err.Message )
        '	Return Nothing
        'End Try
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Try
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            Select Case nTypeOperation
                Case TYPEOPERATION_INSERT, TYPEOPERATION_UPDATE
                    cmdMyCommand.CommandText = "prc_FABBRICATI_IU"
                    'valorizzo i parameters
                    cmdMyCommand.Parameters.Clear()
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COD_FABBRICATO", SqlDbType.Int, ParameterDirection.InputOutput)).Value = oMyFab.iCodFabbricatoReale
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COD_ENTE", SqlDbType.NVarChar)).Value = oMyFab.sIdEnte
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COD_STRADA", SqlDbType.Int)).Value = oMyFab.nCodVia
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUM_CIVICO", SqlDbType.Int)).Value = oMyFab.nNumeroCivico
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@POSIZIONE_CIVICO", SqlDbType.NVarChar)).Value = oMyFab.sPosizioneCivico
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ESPONENTE_CIVICO", SqlDbType.NVarChar)).Value = oMyFab.sEsponenteCivico
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOMEFABBRICATO", SqlDbType.NVarChar)).Value = oMyFab.sNomeFabbricato
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FLAG_SN", SqlDbType.Bit)).Value = oMyFab.bSenzaNumero
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COD_TIPO_FABBRICATO", SqlDbType.NVarChar)).Value = oMyFab.sCodTipoFab
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FLAG_CONDOMINIO", SqlDbType.Bit)).Value = oMyFab.bCondominio
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COD_CONDOMINIO", SqlDbType.Int)).Value = oMyFab.oCondominio.nIdCondominio
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FLAG_PRESENZA_CORTILE", SqlDbType.Bit)).Value = oMyFab.bCortile
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@INFO_SEMINTERRATO", SqlDbType.NVarChar)).Value = oMyFab.sCodSeminterrato
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@INFO_SOTTOTETTO", SqlDbType.NVarChar)).Value = oMyFab.sCodSottotetto
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUM_PIANI", SqlDbType.Int)).Value = oMyFab.nPiani
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUM_CAMPANELLI", SqlDbType.Int)).Value = oMyFab.nCampanelli
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUM_ALLOGGI", SqlDbType.Int)).Value = oMyFab.nAlloggi
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUM_AUTORIMESSE", SqlDbType.Int)).Value = oMyFab.nAutorimesse
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUM_DEPOSITI", SqlDbType.Int)).Value = oMyFab.nDepositi
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUM_TETTOIE", SqlDbType.Int)).Value = oMyFab.nTettoie
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUM_NEGOZI", SqlDbType.Int)).Value = oMyFab.nNegozi
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUM_LABORATORI", SqlDbType.Int)).Value = oMyFab.nLaboratori
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUM_UFFICI", SqlDbType.Int)).Value = oMyFab.nUffici
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUM_RESIDENTI", SqlDbType.Int)).Value = 0
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_UTENZA", SqlDbType.Int)).Value = 0
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID_REFERENTE", SqlDbType.Int)).Value = 0
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CAT_CATASTALE_TEORICA", SqlDbType.NVarChar)).Value = oMyFab.sCatCatastale
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COD_STATO_CONSERVAZIONE", SqlDbType.Int)).Value = oMyFab.nStatoConservazione
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COD_AREA_A", SqlDbType.Int)).Value = oMyFab.nAreeAmministrative
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COD_AREA_U", SqlDbType.Int)).Value = oMyFab.nAreeUrbanistiche
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COD_ZONA", SqlDbType.Int)).Value = oMyFab.nZona
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COD_MICROZONA", SqlDbType.Int)).Value = oMyFab.nMicrozona
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COD_STRADA_1", SqlDbType.Int)).Value = DBNull.Value
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUM_CIVICO_1", SqlDbType.Int)).Value = DBNull.Value
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO_CIVICO", SqlDbType.Int)).Value = DBNull.Value
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@STATO_FABBRICATO", SqlDbType.Int)).Value = DBNull.Value
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOTE_DATI_GENERALI", SqlDbType.NVarChar)).Value = oMyFab.sNote
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@STATO_ASCENSORE", SqlDbType.Int)).Value = DBNull.Value
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@STATO_BARRIERE", SqlDbType.Int)).Value = DBNull.Value
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@STATO_AMIANTO", SqlDbType.Int)).Value = DBNull.Value
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COD_TIPO_COPERTURA", SqlDbType.Int)).Value = DBNull.Value
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@STATO_INTERESSE_STORICO", SqlDbType.Int)).Value = DBNull.Value
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FLAG_SOGGETTO_CALAMITA", SqlDbType.Int)).Value = DBNull.Value
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOTE_CARATTERISTICHE", SqlDbType.Int)).Value = DBNull.Value
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_COSTRUZIONE", SqlDbType.Int)).Value = DBNull.Value
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_ULTIMA_RISTRUTTURAZIONE", SqlDbType.Int)).Value = DBNull.Value
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PROVENIENZA_CATASTO", SqlDbType.Int)).Value = DBNull.Value
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FABBRICATO_DEFINITO", SqlDbType.Int)).Value = DBNull.Value
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TARGA", SqlDbType.Int)).Value = DBNull.Value
                Case TYPEOPERATION_DELETE
            End Select

            Return cmdMyCommand
        Catch Err As Exception
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Log.Error("Si è verificato un errore in ClsDB::GetSQLGestioneFabbricato::" & Err.Message)
            Return Nothing
        End Try
    End Function
    Public Function GetSQLGestioneCondominio(sMyConn As String, ByVal nTypeOperation As Integer, ByVal oMyFab As ObjFabbricato) As SqlClient.SqlCommand
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(sMyConn)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            'VALORIZZO IL COMMANDTEXT;
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.CommandType = CommandType.Text
            Select Case nTypeOperation
                Case TYPEOPERATION_INSERT
                    cmdMyCommand.CommandText = "INSERT INTO CONDOMINI_STRADARIO ("
                    cmdMyCommand.CommandText += "COD_ENTE, DENOMINAZIONE, DATI_AMMINISTRATORE, VIE_NUMERI_CIVICI"
                    cmdMyCommand.CommandText += ")"
                    cmdMyCommand.CommandText += " VALUES (@IDENTE"
                    cmdMyCommand.CommandText += ",@NOMEFABBRICATO,@INFOAMMINISTRATORE,@VIECIVICI"
                    cmdMyCommand.CommandText += ")"
                    cmdMyCommand.CommandText += " SELECT @@IDENTITY AS COD_CONDOMINIO"
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oMyFab.sIdEnte
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOMEFABBRICATO", SqlDbType.NVarChar)).Value = oMyFab.sNomeFabbricato
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@INFOAMMINISTRATORE", SqlDbType.NVarChar)).Value = oMyFab.oCondominio.sDatiAmministratore
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VIECIVICI", SqlDbType.NVarChar)).Value = oMyFab.oCondominio.sVieAmministrative
                Case TYPEOPERATION_DELETE
                    cmdMyCommand.CommandText = "DELETE"
                    cmdMyCommand.CommandText += " FROM CONDOMINI_STRADARIO"
                    cmdMyCommand.CommandText += " WHERE (COD_CONDOMINIO=@IDCONDOMINIO)"
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONDOMINIO", SqlDbType.Int)).Value = oMyFab.oCondominio.nIdCondominio
            End Select

            Return cmdMyCommand
        Catch Err As Exception
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Log.Error("Si è verificato un errore in ClsDB::GetSQLGestioneCondominio::" & Err.Message)
            Return Nothing
        End Try
    End Function

    Public Function GetSQLDescrizioneUI(myStringConnection As String, ByVal nIdUI As Integer) As SqlClient.SqlCommand
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.CommandText = "SELECT FABBRICATI.NOME_FABBRICATO"
            cmdMyCommand.CommandText += " , VIA+' '+CASE WHEN NOT FABBRICATI.NUM_CIVICO IS NULL THEN CAST(FABBRICATI.NUM_CIVICO AS NVARCHAR) ELSE '' END+' '+CASE WHEN NOT FABBRICATI.ESPONENTE_CIVICO IS NULL THEN FABBRICATI.ESPONENTE_CIVICO ELSE '' END AS INDIRIZZO"
            cmdMyCommand.CommandText += " , UNITA_IMMOBILIARI.DATA_INIZIO, UNITA_IMMOBILIARI.DATA_FINE"
            cmdMyCommand.CommandText += " , FOGLIO, NUMERO, SUB"
            cmdMyCommand.CommandText += " , TAB_PIANI.DESCRIZIONE AS PIANO, TAB_ZONE_CENSUARIE.DESCRIZIONE AS ZONACENS"
            cmdMyCommand.CommandText += " FROM FABBRICATI"
            cmdMyCommand.CommandText += " INNER JOIN V_STRADE ON FABBRICATI.COD_STRADA =V_STRADE.ID_VIA"
            cmdMyCommand.CommandText += " LEFT JOIN UNITA_IMMOBILIARI ON FABBRICATI.COD_FABBRICATO = UNITA_IMMOBILIARI.COD_FABBRICATO"
            cmdMyCommand.CommandText += " LEFT JOIN TAB_PIANI ON UNITA_IMMOBILIARI.COD_PIANO=TAB_PIANI.COD_PIANO"
            cmdMyCommand.CommandText += " LEFT JOIN TAB_ZONE_CENSUARIE ON UNITA_IMMOBILIARI.COD_ZONA_CENSUARIA=TAB_ZONE_CENSUARIE.COD_ZONA_CENSUARIA"
            cmdMyCommand.CommandText += " WHERE (COD_UNITA_IMMOBILIARE=@IDUI)"
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDUI", SqlDbType.Int)).Value = nIdUI

            Return cmdMyCommand '.ExecuteReader
        Catch Err As Exception
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Log.Error("Si è verificato un errore in ClsDB::GetSQLDescrizioneUI::" & Err.Message)
            Return Nothing
        End Try
    End Function

    Public Function GetSQLUnitaImmobiliare(myStringConnection As String, ByVal sIdEnte As String, ByVal nIdFab As Integer, ByVal nIdUI As Integer, ByVal nFoglio As Integer, ByVal sNumero As String, ByVal nSubalterno As Integer) As SqlClient.SqlCommand
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "SELECT *"
            cmdMyCommand.CommandText += " FROM V_UNITAIMMOBILIARI"
            cmdMyCommand.CommandText += " WHERE (1=1)"
            'VALORIZZO I PARAMETERS:
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.CommandText += " AND ((@IDUI<=0 AND COD_ENTE=@IDENTE) OR COD_UNITA_IMMOBILIARE=@IDUI)"
            cmdMyCommand.CommandText += " AND (@IDFAB<=0 OR COD_FABBRICATO=@IDFAB)"
            cmdMyCommand.CommandText += " AND (@FOGLIO<=0 OR FOGLIO=@FOGLIO)"
            cmdMyCommand.CommandText += " AND (@NUMERO='' OR NUMERO=@NUMERO)"
            cmdMyCommand.CommandText += " AND (@SUB<=0 OR SUB=@SUB)"
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFAB", SqlDbType.Int)).Value = nIdFab
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDUI", SqlDbType.Int)).Value = nIdUI
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FOGLIO", SqlDbType.Int)).Value = nFoglio
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO", SqlDbType.NVarChar)).Value = sNumero
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SUB", SqlDbType.Int)).Value = nSubalterno

            Return cmdMyCommand '.ExecuteReader
        Catch Err As Exception
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Log.Error("Si è verificato un errore in ClsDB::GetSQLUnitaImmobiliare::" & Err.Message)
            Return Nothing
        End Try
    End Function

    Public Function GetSQLGestioneUnitaImmobiliare(myStringConnection As String, ByVal nTypeOperation As Integer, ByVal oMyUI As ObjUnitaImmobiliare) As SqlClient.SqlDataReader
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.Parameters.Clear()
            Select Case nTypeOperation
                Case TYPEOPERATION_INSERT
                    cmdMyCommand.CommandText = "INSERT INTO UNITA_IMMOBILIARI(COD_ENTE"
                    cmdMyCommand.CommandText += ",COD_FABBRICATO"
                    cmdMyCommand.CommandText += ",FOGLIO, NUMERO"
                    cmdMyCommand.CommandText += ",DATA_INIZIO"
                    cmdMyCommand.CommandText += ",INTERNO,GRAFFATURA,COD_ECOGRAFO,PROV_CATASTALE"
                    cmdMyCommand.CommandText += ",NOTE"
                    If oMyUI.nSubalterno > 0 Then
                        cmdMyCommand.CommandText += ",SUB"
                    End If
                    If oMyUI.dAl <> Date.MinValue And oMyUI.dAl <> Date.MaxValue Then
                        cmdMyCommand.CommandText += ",DATA_FINE"
                    End If
                    If oMyUI.nCodZonaCensuaria > 0 Then
                        cmdMyCommand.CommandText += ",COD_ZONA_CENSUARIA"
                    End If
                    cmdMyCommand.CommandText += ")"
                    cmdMyCommand.CommandText += " VALUES(@IDENTE"
                    cmdMyCommand.CommandText += ",@IDFABBRICATO"
                    cmdMyCommand.CommandText += ",@FOGLIO,@NUMERO"
                    cmdMyCommand.CommandText += ",@DATAINIZIO"
                    cmdMyCommand.CommandText += ",@INTERNO,@GRAFFATURA,@CODECOGRAFICO,@PROVENIENZACATASTALE"
                    cmdMyCommand.CommandText += ",@NOTE"
                    If oMyUI.nSubalterno > 0 Then
                        cmdMyCommand.CommandText += ",@SUB"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SUB", SqlDbType.Int)).Value = oMyUI.nSubalterno
                    End If
                    If oMyUI.dAl <> Date.MinValue And oMyUI.dAl <> Date.MaxValue Then
                        cmdMyCommand.CommandText += ",@DATAFINE"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAFINE", SqlDbType.NVarChar)).Value = FncDate.GiraData(oMyUI.dAl)
                    End If
                    If oMyUI.nCodZonaCensuaria > 0 Then
                        cmdMyCommand.CommandText += ",@ZONACENSUARIA"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ZONACENSUARIA", SqlDbType.Int)).Value = oMyUI.nCodZonaCensuaria
                    End If
                    cmdMyCommand.CommandText += " )"
                    cmdMyCommand.CommandText += " SELECT @@IDENTITY AS COD_UI"
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oMyUI.sIdEnte
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFABBRICATO", SqlDbType.Int)).Value = oMyUI.nIdFabbricato
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FOGLIO", SqlDbType.Int)).Value = oMyUI.nFoglio
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO", SqlDbType.NVarChar)).Value = oMyUI.sNumero
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAINIZIO", SqlDbType.NVarChar)).Value = FncDate.GiraData(oMyUI.dDal)
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@INTERNO", SqlDbType.NVarChar)).Value = oMyUI.sInterno
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@GRAFFATURA", SqlDbType.NVarChar)).Value = oMyUI.sGraffatura
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODECOGRAFICO", SqlDbType.NVarChar)).Value = oMyUI.sCodEcografico
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PROVENIENZACATASTALE", SqlDbType.NVarChar)).Value = oMyUI.sProvCatastale
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOTE", SqlDbType.NVarChar)).Value = oMyUI.sNote

                Case TYPEOPERATION_UPDATE
                    cmdMyCommand.CommandText = "UPDATE UNITA_IMMOBILIARI SET"
                    cmdMyCommand.CommandText += " COD_ENTE=@IDENTE,COD_FABBRICATO=@IDFABBRICATO"
                    cmdMyCommand.CommandText += ",FOGLIO=@FOGLIO, NUMERO=@NUMERO"
                    cmdMyCommand.CommandText += ",DATA_INIZIO=@DATAINIZIO"
                    cmdMyCommand.CommandText += ",INTERNO=@INTERNO,GRAFFATURA=@GRAFFATURA,COD_ECOGRAFO=@CODECOGRAFICO,PROV_CATASTALE=@PROVENIENZACATASTALE"
                    cmdMyCommand.CommandText += ",NOTE=@NOTE"
                    If oMyUI.nSubalterno > 0 Then
                        cmdMyCommand.CommandText += ",SUB=@SUB"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SUB", SqlDbType.Int)).Value = oMyUI.nSubalterno
                    Else
                        cmdMyCommand.CommandText += ",SUB=NULL"
                    End If
                    If oMyUI.dAl <> Date.MinValue And oMyUI.dAl <> Date.MaxValue Then
                        cmdMyCommand.CommandText += ",DATA_FINE=@DATAFINE"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAFINE", SqlDbType.NVarChar)).Value = FncDate.GiraData(oMyUI.dAl)
                    Else
                        cmdMyCommand.CommandText += ",DATA_FINE=NULL"
                    End If
                    If oMyUI.nCodZonaCensuaria > 0 Then
                        cmdMyCommand.CommandText += ",COD_ZONA_CENSUARIA=@ZONACENSUARIA"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ZONACENSUARIA", SqlDbType.Int)).Value = oMyUI.nCodZonaCensuaria
                    Else
                        cmdMyCommand.CommandText += ",COD_ZONA_CENSUARIA=NULL"
                    End If
                    cmdMyCommand.CommandText += " WHERE (COD_UNITA_IMMOBILIARE=@IDUI)"
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDUI", SqlDbType.Int)).Value = oMyUI.nIdUnitaImmobiliare
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oMyUI.sIdEnte
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFABBRICATO", SqlDbType.Int)).Value = oMyUI.nIdFabbricato
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FOGLIO", SqlDbType.Int)).Value = oMyUI.nFoglio
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO", SqlDbType.NVarChar)).Value = oMyUI.sNumero
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAINIZIO", SqlDbType.NVarChar)).Value = FncDate.GiraData(oMyUI.dDal)
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@INTERNO", SqlDbType.NVarChar)).Value = oMyUI.sInterno
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@GRAFFATURA", SqlDbType.NVarChar)).Value = oMyUI.sGraffatura
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODECOGRAFICO", SqlDbType.NVarChar)).Value = oMyUI.sCodEcografico
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PROVENIENZACATASTALE", SqlDbType.NVarChar)).Value = oMyUI.sProvCatastale
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOTE", SqlDbType.NVarChar)).Value = oMyUI.sNote
                Case TYPEOPERATION_DELETE
            End Select

            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return cmdMyCommand.ExecuteReader
        Catch Err As Exception
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Log.Error("Si è verificato un errore in ClsDB::GetSQLGestioneunitaimmobiliare::" & Err.Message)
            Return Nothing
        End Try
    End Function

    Public Function GetSQLDescrizioneClassificazione(myStringConnection As String, ByVal nIdClassificazione As Integer) As SqlClient.SqlCommand
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.CommandText = "SELECT UNITA_IMMOBILIARI_CLASSIFICAZIONI.COD_CLASSIFICAZIONE , UNITA_IMMOBILIARI_CLASSIFICAZIONI.DATA_INIZIO, UNITA_IMMOBILIARI_CLASSIFICAZIONI.DATA_FINE, "
            cmdMyCommand.CommandText += " UNITA_IMMOBILIARI_CLASSIFICAZIONI.COD_CATEGORIA_CATASTALE AS CATEGORIA, UNITA_IMMOBILIARI_CLASSIFICAZIONI.CLASSE, "
            cmdMyCommand.CommandText += " UNITA_IMMOBILIARI_CLASSIFICAZIONI.CONSISTENZA, TAB_TIPO_RENDITA.DESCRIZIONE AS RENDITA, UNITA_IMMOBILIARI_CLASSIFICAZIONI.VALORE_RENDITA AS VALORE"
            cmdMyCommand.CommandText += " FROM UNITA_IMMOBILIARI_CLASSIFICAZIONI"
            cmdMyCommand.CommandText += " LEFT JOIN TAB_TIPO_RENDITA ON UNITA_IMMOBILIARI_CLASSIFICAZIONI.COD_TIPO_RENDITA = TAB_TIPO_RENDITA.COD_RENDITA"
            cmdMyCommand.CommandText += " WHERE (UNITA_IMMOBILIARI_CLASSIFICAZIONI.COD_CLASSIFICAZIONE=@IDCLASSIFICAZIONE)"
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCLASSIFICAZIONE", SqlDbType.Int)).Value = nIdClassificazione

            Return cmdMyCommand '.ExecuteReader
        Catch Err As Exception
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Log.Error("Si è verificato un errore in ClsDB::GetSQLDescrizioneClassificazione::" & Err.Message)
            Return Nothing
        End Try
    End Function

    Public Function GetSQLClassificazione(myStringConnection As String, ByVal nIdUI As Integer, ByVal nIdClas As Integer, ByVal sCatCatastale As String, ByVal sClasse As String, ByVal nConsistenza As Integer, ByVal nCodTipoRendita As Integer) As SqlClient.SqlCommand 'DataTable
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dtMyDati As New DataTable()

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.CommandText = "SELECT *"
            cmdMyCommand.CommandText += " FROM V_CLASSIFICAZIONI"
            cmdMyCommand.CommandText += " WHERE (1=1)"
            'VALORIZZO I PARAMETERS:
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.CommandText += " AND ((@IDCLAS<=0 AND COD_UI=@IDUI) OR COD_CLASSIFICAZIONE=@IDCLAS)"
            cmdMyCommand.CommandText += " AND (@CATCATASTALE='' OR COD_CATEGORIA_CATASTALE=@CATCATASTALE)"
            cmdMyCommand.CommandText += " AND (@CLASSE='' OR CLASSE=@CLASSE)"
            cmdMyCommand.CommandText += " AND (@CONSISTENZA<=0 OR CONSISTENZA=@CONSISTENZA)"
            cmdMyCommand.CommandText += " AND (@IDTIPORENDITA<=0 OR COD_TIPO_RENDITA=@IDTIPORENDITA)"
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDUI", SqlDbType.Int)).Value = nIdUI
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCLAS", SqlDbType.Int)).Value = nIdClas
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CATCATASTALE", SqlDbType.NVarChar)).Value = sCatCatastale
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CLASSE", SqlDbType.NVarChar)).Value = sClasse
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CONSISTENZA", SqlDbType.Int)).Value = nConsistenza
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTIPORENDITA", SqlDbType.Int)).Value = nCodTipoRendita
            'myAdapter.SelectCommand = cmdMyCommand
            'myAdapter.Fill(dtMyDati)
            'myAdapter.Dispose()
            Return cmdMyCommand 'dtMyDati
        Catch Err As Exception
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Log.Error("Si è verificato un errore in ClsDB::GetSQLClassificazione::" & Err.Message)
            Return Nothing
        End Try
    End Function

    Public Function GetSQLGestioneClassificazione(myStringConnection As String, ByVal nTypeOperation As Integer, ByVal oMyClas As ObjClassificazione) As SqlClient.SqlDataReader
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.Parameters.Clear()
            Select Case nTypeOperation
                Case TYPEOPERATION_INSERT
                    cmdMyCommand.CommandText = "INSERT INTO UNITA_IMMOBILIARI_CLASSIFICAZIONI(COD_UI"
                    cmdMyCommand.CommandText += ",DATA_INIZIO"
                    cmdMyCommand.CommandText += ",NOTE"
                    cmdMyCommand.CommandText += ",CONSISTENZA"
                    cmdMyCommand.CommandText += ",VALORE_RENDITA"
                    cmdMyCommand.CommandText += ",NUMERO_PROTOCOLLO"
                    cmdMyCommand.CommandText += ",FLAG_INAGIBILITA"
                    cmdMyCommand.CommandText += ",CLASSE"
                    If oMyClas.nSuperficieCatastale > 0 Then
                        cmdMyCommand.CommandText += ",SUPERFICIE_CATASTALE"
                    End If
                    If oMyClas.nSuperficieNetta > 0 Then
                        cmdMyCommand.CommandText += ",SUPERFICIE_NETTA"
                    End If
                    If oMyClas.nSuperficieLorda > 0 Then
                        cmdMyCommand.CommandText += ",SUPERFICIE_LORDA"
                    End If
                    If oMyClas.sCodCategoriaCatastale <> "-1" Then
                        cmdMyCommand.CommandText += ",COD_CATEGORIA_CATASTALE"
                    End If
                    If oMyClas.nCodTipoRendita > 0 Then
                        cmdMyCommand.CommandText += ",COD_TIPO_RENDITA"
                    End If
                    If oMyClas.nCodDestUso > 0 Then
                        cmdMyCommand.CommandText += ",COD_DESTINAZIONE_USO"
                    End If
                    If oMyClas.nCodInagibilita > 0 Then
                        cmdMyCommand.CommandText += ",COD_INAGIBILITA"
                    End If
                    If oMyClas.dAl <> Date.MinValue And oMyClas.dAl <> Date.MaxValue Then
                        cmdMyCommand.CommandText += ",DATA_FINE"
                    End If
                    If oMyClas.dDataAttribuzione <> Date.MinValue And oMyClas.dDataAttribuzione <> Date.MaxValue Then
                        cmdMyCommand.CommandText += ",DATA_ATTRIBUZIONE_RENDITA"
                    End If
                    If oMyClas.dDataEfficacia <> Date.MinValue And oMyClas.dDataEfficacia <> Date.MaxValue Then
                        cmdMyCommand.CommandText += ",DATA_EFFICACIA"
                    End If
                    If oMyClas.dDataProtocollo <> Date.MinValue And oMyClas.dDataProtocollo <> Date.MaxValue Then
                        cmdMyCommand.CommandText += ",DATA_PRESENTAZIONE_PROT"
                    End If
                    If oMyClas.dDataEffettivoUtilizzo <> Date.MinValue And oMyClas.dDataEffettivoUtilizzo <> Date.MaxValue Then
                        cmdMyCommand.CommandText += ",DATA_EFFETTIVO_UTILIZZO"
                    End If
                    If oMyClas.dDataFineLavori <> Date.MinValue And oMyClas.dDataFineLavori <> Date.MaxValue Then
                        cmdMyCommand.CommandText += ",DATA_FINE_LAVORI"
                    End If
                    cmdMyCommand.CommandText += ")"
                    cmdMyCommand.CommandText += " VALUES(@IDUI"
                    cmdMyCommand.CommandText += ",@DATAINIZIO"
                    cmdMyCommand.CommandText += ",@NOTE"
                    cmdMyCommand.CommandText += ",@CONSISTENZA"
                    cmdMyCommand.CommandText += ",@RENDITA"
                    cmdMyCommand.CommandText += ",@NPROTOCOLLO"
                    cmdMyCommand.CommandText += ",@ISINAGIBILITA"
                    cmdMyCommand.CommandText += ",@CLASSE"
                    If oMyClas.nSuperficieCatastale > 0 Then
                        cmdMyCommand.CommandText += ",@SUPCATASTALE"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SUPCATASTALE", SqlDbType.Int)).Value = oMyClas.nSuperficieCatastale
                    End If
                    If oMyClas.nSuperficieNetta > 0 Then
                        cmdMyCommand.CommandText += ",@SUPNETTA"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SUPNETTA", SqlDbType.Int)).Value = oMyClas.nSuperficieNetta
                    End If
                    If oMyClas.nSuperficieLorda > 0 Then
                        cmdMyCommand.CommandText += ",@SUPLORDA"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SUPLORDA", SqlDbType.Int)).Value = oMyClas.nSuperficieLorda
                    End If
                    If oMyClas.sCodCategoriaCatastale <> "-1" Then
                        cmdMyCommand.CommandText += ",@CATCATASTALE"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CATCATASTALE", SqlDbType.NVarChar)).Value = oMyClas.sCodCategoriaCatastale
                    End If
                    If oMyClas.nCodTipoRendita > 0 Then
                        cmdMyCommand.CommandText += ",@IDTIPORENDITA"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTIPORENDITA", SqlDbType.Int)).Value = oMyClas.nCodTipoRendita
                    End If
                    If oMyClas.nCodDestUso > 0 Then
                        cmdMyCommand.CommandText += ",@IDDESTUSO"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDDESTUSO", SqlDbType.Int)).Value = oMyClas.nCodDestUso
                    End If
                    If oMyClas.nCodInagibilita > 0 Then
                        cmdMyCommand.CommandText += ",@IDINAGIBILITA"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDINAGIBILITA", SqlDbType.Int)).Value = oMyClas.nCodInagibilita
                    End If
                    If oMyClas.dAl <> Date.MinValue And oMyClas.dAl <> Date.MaxValue Then
                        cmdMyCommand.CommandText += ",@DATAFINE"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAFINE", SqlDbType.NVarChar)).Value = FncDate.GiraData(oMyClas.dAl)
                    End If
                    If oMyClas.dDataAttribuzione <> Date.MinValue And oMyClas.dDataAttribuzione <> Date.MaxValue Then
                        cmdMyCommand.CommandText += ",@DATAATTRIBUZIONERENDITA"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAATTRIBUZIONERENDITA", SqlDbType.NVarChar)).Value = FncDate.GiraData(oMyClas.dDataAttribuzione)
                    End If
                    If oMyClas.dDataEfficacia <> Date.MinValue And oMyClas.dDataEfficacia <> Date.MaxValue Then
                        cmdMyCommand.CommandText += ",@DATAEFFICACIA"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAEFFICACIA", SqlDbType.NVarChar)).Value = FncDate.GiraData(oMyClas.dDataEfficacia)
                    End If
                    If oMyClas.dDataProtocollo <> Date.MinValue And oMyClas.dDataProtocollo <> Date.MaxValue Then
                        cmdMyCommand.CommandText += ",@DATAPROTOCOLLO"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAPROTOCOLLO", SqlDbType.NVarChar)).Value = FncDate.GiraData(oMyClas.dDataProtocollo)
                    End If
                    If oMyClas.dDataEffettivoUtilizzo <> Date.MinValue And oMyClas.dDataEffettivoUtilizzo <> Date.MaxValue Then
                        cmdMyCommand.CommandText += ",@DATAEFFETTIVOUTILIZZO"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAEFFETTIVOUTILIZZO", SqlDbType.NVarChar)).Value = FncDate.GiraData(oMyClas.dDataEffettivoUtilizzo)
                    End If
                    If oMyClas.dDataFineLavori <> Date.MinValue And oMyClas.dDataFineLavori <> Date.MaxValue Then
                        cmdMyCommand.CommandText += ",@DATAFINELAVORI"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAFINELAVORI", SqlDbType.NVarChar)).Value = FncDate.GiraData(oMyClas.dDataFineLavori)
                    End If
                    cmdMyCommand.CommandText += " )"
                    cmdMyCommand.CommandText += " SELECT @@IDENTITY AS COD_CLAS"
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDUI", SqlDbType.Int)).Value = oMyClas.nIdUI
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAINIZIO", SqlDbType.NVarChar)).Value = FncDate.GiraData(oMyClas.dDal)
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOTE", SqlDbType.NVarChar)).Value = oMyClas.sNote
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CONSISTENZA", SqlDbType.Int)).Value = oMyClas.nConsistenza
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@RENDITA", SqlDbType.NVarChar)).Value = oMyClas.sValoreRendita
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NPROTOCOLLO", SqlDbType.NVarChar)).Value = oMyClas.sNProtocollo
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ISINAGIBILITA", SqlDbType.Bit)).Value = oMyClas.bInagibilita
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CLASSE", SqlDbType.NVarChar)).Value = oMyClas.sCodClasse

                Case TYPEOPERATION_UPDATE
                    cmdMyCommand.CommandText = "UPDATE UNITA_IMMOBILIARI_CLASSIFICAZIONI SET"
                    cmdMyCommand.CommandText += " COD_UI=@IDUI,DATA_INIZIO=@DATAINIZIO"
                    cmdMyCommand.CommandText += ",NOTE=@NOTE"
                    cmdMyCommand.CommandText += ",CONSISTENZA=@CONSISTENZA"
                    cmdMyCommand.CommandText += ",VALORE_RENDITA=@RENDITA"
                    cmdMyCommand.CommandText += ",NUMERO_PROTOCOLLO=@NPROTOCOLLO"
                    cmdMyCommand.CommandText += ",FLAG_INAGIBILITA=@ISINAGIBILITA"
                    If oMyClas.nSuperficieCatastale > 0 Then
                        cmdMyCommand.CommandText += ",SUPERFICIE_CATASTALE=@SUPCATASTALE"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SUPCATASTALE", SqlDbType.Int)).Value = oMyClas.nSuperficieCatastale
                    Else
                        cmdMyCommand.CommandText += ",SUPERFICIE_CATASTALE=0"
                    End If
                    If oMyClas.nSuperficieNetta > 0 Then
                        cmdMyCommand.CommandText += ",SUPERFICIE_NETTA=@SUPNETTA"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SUPNETTA", SqlDbType.Int)).Value = oMyClas.nSuperficieNetta
                    Else
                        cmdMyCommand.CommandText += ",SUPERFICIE_NETTA=0"
                    End If
                    If oMyClas.nSuperficieLorda > 0 Then
                        cmdMyCommand.CommandText += ",SUPERFICIE_LORDA=@SUPLORDA"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SUPLORDA", SqlDbType.Int)).Value = oMyClas.nSuperficieLorda
                    Else
                        cmdMyCommand.CommandText += ",SUPERFICIE_LORDA=0"
                    End If
                    If oMyClas.sCodCategoriaCatastale <> "-1" Then
                        cmdMyCommand.CommandText += ",COD_CATEGORIA_CATASTALE=@CATCATASTALE"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CATCATASTALE", SqlDbType.NVarChar)).Value = oMyClas.sCodCategoriaCatastale
                    Else
                        cmdMyCommand.CommandText += ",COD_CATEGORIA_CATASTALE=NULL"
                    End If
                    If oMyClas.sCodClasse <> "-1" Then
                        cmdMyCommand.CommandText += ",CLASSE=@CLASSE"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CLASSE", SqlDbType.NVarChar)).Value = oMyClas.sCodClasse
                    Else
                        cmdMyCommand.CommandText += ",CLASSE=NULL"
                    End If
                    If oMyClas.nCodTipoRendita > 0 Then
                        cmdMyCommand.CommandText += ",COD_TIPO_RENDITA=@IDTIPORENDITA"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTIPORENDITA", SqlDbType.Int)).Value = oMyClas.nCodTipoRendita
                    Else
                        cmdMyCommand.CommandText += ",COD_TIPO_RENDITA=NULL"
                    End If
                    If oMyClas.nCodDestUso > 0 Then
                        cmdMyCommand.CommandText += ",COD_DESTINAZIONE_USO=@IDDESTUSO"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDDESTUSO", SqlDbType.Int)).Value = oMyClas.nCodDestUso
                    Else
                        cmdMyCommand.CommandText += ",COD_DESTINAZIONE_USO=NULL"
                    End If
                    If oMyClas.nCodInagibilita > 0 Then
                        cmdMyCommand.CommandText += ",COD_INAGIBILITA=@IDINAGIBILITA"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDINAGIBILITA", SqlDbType.Int)).Value = oMyClas.nCodInagibilita
                    Else
                        cmdMyCommand.CommandText += ",COD_INAGIBILITA=NULL"
                    End If
                    If oMyClas.dAl <> Date.MinValue And oMyClas.dAl <> Date.MaxValue Then
                        cmdMyCommand.CommandText += ",DATA_FINE=@DATAFINE"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAFINE", SqlDbType.NVarChar)).Value = FncDate.GiraData(oMyClas.dAl)
                    Else
                        cmdMyCommand.CommandText += ",DATA_FINE=NULL"
                    End If
                    If oMyClas.dDataAttribuzione <> Date.MinValue And oMyClas.dDataAttribuzione <> Date.MaxValue Then
                        cmdMyCommand.CommandText += ",DATA_ATTRIBUZIONE_RENDITA=@DATAATTRIBUZIONERENDITA"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAATTRIBUZIONERENDITA", SqlDbType.NVarChar)).Value = FncDate.GiraData(oMyClas.dDataAttribuzione)
                    Else
                        cmdMyCommand.CommandText += ",DATA_ATTRIBUZIONE_RENDITA=NULL"
                    End If
                    If oMyClas.dDataEfficacia <> Date.MinValue And oMyClas.dDataEfficacia <> Date.MaxValue Then
                        cmdMyCommand.CommandText += ",DATA_EFFICACIA=@DATAEFFICACIA"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAEFFICACIA", SqlDbType.NVarChar)).Value = FncDate.GiraData(oMyClas.dDataEfficacia)
                    Else
                        cmdMyCommand.CommandText += ",DATA_EFFICACIA=NULL"
                    End If
                    If oMyClas.dDataProtocollo <> Date.MinValue And oMyClas.dDataProtocollo <> Date.MaxValue Then
                        cmdMyCommand.CommandText += ",DATA_PRESENTAZIONE_PROT=@DATAPROTOCOLLO"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAPROTOCOLLO", SqlDbType.NVarChar)).Value = FncDate.GiraData(oMyClas.dDataProtocollo)
                    Else
                        cmdMyCommand.CommandText += ",DATA_PRESENTAZIONE_PROT=NULL"
                    End If
                    If oMyClas.dDataEffettivoUtilizzo <> Date.MinValue And oMyClas.dDataEffettivoUtilizzo <> Date.MaxValue Then
                        cmdMyCommand.CommandText += ",DATA_EFFETTIVO_UTILIZZO=@DATAEFFETTIVOUTILIZZO"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAEFFETTIVOUTILIZZO", SqlDbType.NVarChar)).Value = FncDate.GiraData(oMyClas.dDataEffettivoUtilizzo)
                    Else
                        cmdMyCommand.CommandText += ",DATA_EFFETTIVO_UTILIZZO=NULL"
                    End If
                    If oMyClas.dDataFineLavori <> Date.MinValue And oMyClas.dDataFineLavori <> Date.MaxValue Then
                        cmdMyCommand.CommandText += ",DATA_FINE_LAVORI=@DATAFINELAVORI"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAFINELAVORI", SqlDbType.NVarChar)).Value = FncDate.GiraData(oMyClas.dDataFineLavori)
                    Else
                        cmdMyCommand.CommandText += ",DATA_FINE_LAVORI=NULL"
                    End If
                    cmdMyCommand.CommandText += " WHERE (COD_CLASSIFICAZIONE=@IDCLAS)"
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCLAS", SqlDbType.Int)).Value = oMyClas.nIdClassificazione
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDUI", SqlDbType.Int)).Value = oMyClas.nIdUI
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAINIZIO", SqlDbType.NVarChar)).Value = FncDate.GiraData(oMyClas.dDal)
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOTE", SqlDbType.NVarChar)).Value = oMyClas.sNote
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CONSISTENZA", SqlDbType.Int)).Value = oMyClas.nConsistenza
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@RENDITA", SqlDbType.NVarChar)).Value = oMyClas.sValoreRendita
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NPROTOCOLLO", SqlDbType.NVarChar)).Value = oMyClas.sNProtocollo
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ISINAGIBILITA", SqlDbType.Bit)).Value = oMyClas.bInagibilita
                Case TYPEOPERATION_DELETE
            End Select

            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return cmdMyCommand.ExecuteReader
        Catch Err As Exception
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Log.Error("Si è verificato un errore in ClsDB::GetSQLGestioneClassificazione::" & Err.Message)
            Return Nothing
        End Try
    End Function

    Public Function GetSQLValoreRendita(myStringConnection As String, ByVal sIdEnte As String, ByVal sCatCatastale As String, ByVal sClasse As String) As SqlClient.SqlCommand
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.CommandText = "SELECT TARIFFA_EURO"
            cmdMyCommand.CommandText += " FROM V_TARIFFE_CATASTALI"
            cmdMyCommand.CommandText += " WHERE (COD_ENTE=@IDENTE)"
            cmdMyCommand.CommandText += " AND (CATEGORIA=@CATCATASTALE)"
            cmdMyCommand.CommandText += " AND (CLASSE=@CLASSE)"
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CATCATASTALE", SqlDbType.NVarChar)).Value = sCatCatastale
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CLASSE", SqlDbType.NVarChar)).Value = sClasse

            Return cmdMyCommand '.ExecuteReader
        Catch Err As Exception
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Log.Error("Si è verificato un errore in ClsDB::GetSQLUnitaImmobiliare::" & Err.Message)
            Return Nothing
        End Try
    End Function

    Public Function GetSQLProprieta(myStringConnection As String, ByVal nIdClass As Integer, ByVal nIdProprieta As Integer, ByVal nIdProprietario As Integer, ByVal dDataInizio As DateTime, ByVal dDataFine As DateTime, ByVal nIdAnagrafica As Integer) As SqlClient.SqlDataReader
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.CommandText = "SELECT *"
            cmdMyCommand.CommandText += " FROM V_PROPRIETA"
            cmdMyCommand.CommandText += " WHERE (1=1)"
            'VALORIZZO I PARAMETERS:
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.CommandText += " AND (@IDCLAS<=0 OR COD_CLASSIFICAZIONE=@IDCLAS)"
            cmdMyCommand.CommandText += " AND (@IDPROPRIETA<=0 OR COD_PROPRIETA=@IDPROPRIETA)"
            cmdMyCommand.CommandText += " AND (@IDPROPRIETARIO<=0 OR COD_PROPRIETARIO=@IDPROPRIETARIO)"
            cmdMyCommand.CommandText += " AND (YEAR(@DATAINIZIO)=9999 OR DATA_INIZIO=@DATAINIZIO)"
            cmdMyCommand.CommandText += " AND (YEAR(@DATAFINE)=9999 OR DATA_FINE=@DATAFINE)"
            cmdMyCommand.CommandText += " AND (@IDANAGRAFICA<=0 OR COD_ANAGRAFICA=@IDANAGRAFICA)"
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCLAS", SqlDbType.Int)).Value = nIdClass
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPROPRIETA", SqlDbType.Int)).Value = nIdProprieta
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPROPRIETARIO", SqlDbType.Int)).Value = nIdProprietario
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAINIZIO", SqlDbType.NVarChar)).Value = FncDate.GiraData(dDataInizio)
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAFINE", SqlDbType.NVarChar)).Value = FncDate.GiraData(dDataFine)
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDANAGRAFICA", SqlDbType.Int)).Value = nIdAnagrafica

            Return cmdMyCommand.ExecuteReader
        Catch Err As Exception
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Log.Error("Si è verificato un errore in ClsDB::GetSQLProprieta::" & Err.Message)
            Return Nothing
        End Try
    End Function

    Public Function GetSQLGestioneProprieta(myStringConnection As String, ByVal nTypeOperation As Integer, ByVal oMyProprieta As ObjProprieta) As SqlClient.SqlDataReader
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            'prelevo i dati della testata

            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.Parameters.Clear()
            Select Case nTypeOperation
                Case TYPEOPERATION_INSERT
                    cmdMyCommand.CommandText = "INSERT INTO UNITA_IMMOBILIARI_PROPRIETA(COD_CLASSIFICAZIONE"
                    cmdMyCommand.CommandText += ",DATA_INIZIO"
                    If oMyProprieta.dDataFine <> Date.MinValue And oMyProprieta.dDataFine <> Date.MaxValue Then
                        cmdMyCommand.CommandText += ",DATA_FINE"
                    End If
                    cmdMyCommand.CommandText += ")"
                    cmdMyCommand.CommandText += " VALUES(@IDCLAS"
                    cmdMyCommand.CommandText += ",@DATAINIZIO"
                    If oMyProprieta.dDataFine <> Date.MinValue And oMyProprieta.dDataFine <> Date.MaxValue Then
                        cmdMyCommand.CommandText += ",@DATAFINE"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAFINE", SqlDbType.NVarChar)).Value = FncDate.GiraData(oMyProprieta.dDataFine)
                    End If
                    cmdMyCommand.CommandText += " )"
                    cmdMyCommand.CommandText += " SELECT @@IDENTITY AS COD_PROPRIETA"
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCLAS", SqlDbType.Int)).Value = oMyProprieta.nIdClassificazione
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAINIZIO", SqlDbType.NVarChar)).Value = FncDate.GiraData(oMyProprieta.dDataInizio)

                Case TYPEOPERATION_UPDATE
                    cmdMyCommand.CommandText = "UPDATE UNITA_IMMOBILIARI_PROPRIETA SET"
                    cmdMyCommand.CommandText += " DATA_INIZIO=@DATAINIZIO"
                    If oMyProprieta.dDataFine <> Date.MinValue And oMyProprieta.dDataFine <> Date.MaxValue Then
                        cmdMyCommand.CommandText += ",DATA_FINE=@DATAFINE"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAFINE", SqlDbType.NVarChar)).Value = FncDate.GiraData(oMyProprieta.dDataFine)
                    Else
                        cmdMyCommand.CommandText += ",DATA_FINE=NULL"
                    End If
                    cmdMyCommand.CommandText += " WHERE (COD_PROPRIETA=@IDPROPRIETA)"
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPROPRIETA", SqlDbType.Int)).Value = oMyProprieta.nIdProprieta
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCLAS", SqlDbType.Int)).Value = oMyProprieta.nIdClassificazione
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAINIZIO", SqlDbType.NVarChar)).Value = FncDate.GiraData(oMyProprieta.dDataInizio)
                Case TYPEOPERATION_DELETE
            End Select

            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return cmdMyCommand.ExecuteReader
        Catch Err As Exception
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Log.Error("Si è verificato un errore in ClsDB::GetSQLGestioneProprieta::" & Err.Message)
            Return Nothing
        End Try
    End Function

    Public Function GetSQLGestioneProprietario(myStringConnection As String, ByVal nTypeOperation As Integer, ByVal oMyProprietario As ObjProprieta) As SqlClient.SqlDataReader
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.Parameters.Clear()
            Select Case nTypeOperation
                Case TYPEOPERATION_INSERT
                    cmdMyCommand.CommandText = "INSERT INTO UNITA_IMMOBILIARI_PROPRIETARI(COD_PROPRIETA"
                    cmdMyCommand.CommandText += ",COD_ANAGRAFICA"
                    cmdMyCommand.CommandText += ",NOTE"
                    cmdMyCommand.CommandText += ",PERCENTUALE_PROPRIETA"
                    cmdMyCommand.CommandText += ",PERCENTUALE_POSSESSO"
                    If oMyProprietario.nTipoProprieta > 0 Then
                        cmdMyCommand.CommandText += ",COD_TIPO_PROPRIETA"
                    End If
                    If oMyProprietario.nTipoPossesso > 0 Then
                        cmdMyCommand.CommandText += ",COD_TIPO_POSSESSO"
                    End If
                    If oMyProprietario.nTipoUtilizzo > 0 Then
                        cmdMyCommand.CommandText += ",COD_UTILIZZO"
                    End If
                    If oMyProprietario.nTipoParentela > 0 Then
                        cmdMyCommand.CommandText += ",COD_TIPO_PARENTELA"
                    End If
                    cmdMyCommand.CommandText += ")"
                    cmdMyCommand.CommandText += " VALUES(@IDPROPRIETA"
                    cmdMyCommand.CommandText += ",@IDANAGRAFICA"
                    cmdMyCommand.CommandText += ",@NOTE"
                    cmdMyCommand.CommandText += ",@PERCENTPROPRIETA"
                    cmdMyCommand.CommandText += ",@PERCENTPOSSESSO"
                    If oMyProprietario.nTipoProprieta > 0 Then
                        cmdMyCommand.CommandText += ",@TIPOPROPRIETA"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPOPROPRIETA", SqlDbType.Int)).Value = oMyProprietario.nTipoProprieta
                    End If
                    If oMyProprietario.nTipoPossesso > 0 Then
                        cmdMyCommand.CommandText += ",@TIPOPOSSESSO"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPOPOSSESSO", SqlDbType.Int)).Value = oMyProprietario.nTipoPossesso
                    End If
                    If oMyProprietario.nTipoUtilizzo > 0 Then
                        cmdMyCommand.CommandText += ",@TIPOUTILIZZO"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPOUTILIZZO", SqlDbType.Int)).Value = oMyProprietario.nTipoUtilizzo
                    End If
                    If oMyProprietario.nTipoParentela > 0 Then
                        cmdMyCommand.CommandText += ",@TIPOPARENTELA"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPOPARENTELA", SqlDbType.Int)).Value = oMyProprietario.nTipoParentela
                    End If
                    cmdMyCommand.CommandText += " )"
                    cmdMyCommand.CommandText += " SELECT @@IDENTITY AS COD_PROPRIETARIO"
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPROPRIETA", SqlDbType.Int)).Value = oMyProprietario.nIdProprieta
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDANAGRAFICA", SqlDbType.Int)).Value = oMyProprietario.nIdAnagrafica
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOTE", SqlDbType.NVarChar)).Value = oMyProprietario.sNote
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PERCENTPROPRIETA", SqlDbType.Float)).Value = oMyProprietario.nPercentProprieta
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PERCENTPOSSESSO", SqlDbType.Float)).Value = oMyProprietario.nPercentPossesso

                Case TYPEOPERATION_UPDATE
                Case TYPEOPERATION_DELETE
                    cmdMyCommand.CommandText = "DELETE"
                    cmdMyCommand.CommandText += " FROM UNITA_IMMOBILIARI_PROPRIETARI"
                    cmdMyCommand.CommandText += " WHERE (COD_PROPRIETARIO=@IDPROPRIETARIO)"
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPROPRIETARIO", SqlDbType.Int)).Value = oMyProprietario.nIdProprietario
            End Select

            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return cmdMyCommand.ExecuteReader
        Catch Err As Exception
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Log.Error("Si è verificato un errore in ClsDB::GetSQLGestioneProprietario::" & Err.Message)
            Return Nothing
        End Try
    End Function

    Public Function GetSQLConduzione(myStringConnection As String, ByVal nIdClas As Integer, ByVal nIdConduzione As Integer, ByVal nIdConduttore As Integer, ByVal dDataInizio As DateTime, ByVal dDataFine As DateTime, ByVal nIdAnagrafica As Integer) As SqlClient.SqlCommand
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.CommandText = "SELECT *"
            cmdMyCommand.CommandText += " FROM V_CONDUZIONE"
            cmdMyCommand.CommandText += " WHERE (1=1)"
            'VALORIZZO I PARAMETERS:
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.CommandText += " AND ((@IDCONDUZIONE<=0 AND COD_CLASSIFICAZIONE=@IDCLAS) OR COD_CONDUZIONE=@IDCONDUZIONE) "
            cmdMyCommand.CommandText += " AND (@IDCONDUZIONE<=0 OR COD_CONDUZIONE=@IDCONDUZIONE)"
            cmdMyCommand.CommandText += " AND (@IDCONDUTTORE<=0 OR COD_CONDUTTORE=@IDCONDUTTORE)"
            cmdMyCommand.CommandText += " AND (YEAR(@DATAINIZIO)=9999 OR DATA_INIZIO=@DATAINIZIO)"
            cmdMyCommand.CommandText += " AND (YEAR(@DATAFINE)=9999 OR DATA_FINE=@DATAFINE)"
            cmdMyCommand.CommandText += " AND (@IDANAGRAFICA<=0 OR COD_ANAGRAFICA=@IDANAGRAFICA)"
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCLAS", SqlDbType.Int)).Value = nIdClas
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONDUZIONE", SqlDbType.Int)).Value = nIdConduzione
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONDUTTORE", SqlDbType.Int)).Value = nIdConduttore
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAINIZIO", SqlDbType.NVarChar)).Value = FncDate.GiraData(dDataInizio)
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAFINE", SqlDbType.NVarChar)).Value = FncDate.GiraData(dDataFine)
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDANAGRAFICA", SqlDbType.Int)).Value = nIdAnagrafica

            Return cmdMyCommand '.ExecuteReader
        Catch Err As Exception
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Log.Error("Si è verificato un errore in ClsDB::GetSQLConduzione::" & Err.Message)
            Return Nothing
        End Try
    End Function

    Public Function GetSQLGestioneConduzione(sMyConn As String, ByVal nTypeOperation As Integer, ByVal oMyConduzione As ObjConduzione) As SqlClient.SqlCommand
        Dim cmdMyCommand As SqlClient.SqlCommand
        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(sMyConn)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            'VALORIZZO IL COMMANDTEXT;
            cmdMyCommand.Parameters.Clear()
            Select Case nTypeOperation
                Case TYPEOPERATION_INSERT
                    cmdMyCommand.CommandText = "INSERT INTO UNITA_IMMOBILIARI_CONDUZIONE(COD_CLASSIFICAZIONE"
                    cmdMyCommand.CommandText += ",DATA_INIZIO"
                    If oMyConduzione.dDataFine <> Date.MinValue And oMyConduzione.dDataFine <> Date.MaxValue Then
                        cmdMyCommand.CommandText += ",DATA_FINE"
                    End If
                    If oMyConduzione.nCodStatoOccupazione > 0 Then
                        cmdMyCommand.CommandText += ",COD_STATO_OCCUPAZIONE"
                    End If
                    If oMyConduzione.nCodTipoOccupazione > 0 Then
                        cmdMyCommand.CommandText += ",COD_TIPO_OCCUPAZIONE"
                    End If
                    cmdMyCommand.CommandText += ")"
                    cmdMyCommand.CommandText += " VALUES(@IDCLAS"
                    cmdMyCommand.CommandText += ",@DATAINIZIO"
                    If oMyConduzione.dDataFine <> Date.MinValue And oMyConduzione.dDataFine <> Date.MaxValue Then
                        cmdMyCommand.CommandText += ",@DATAFINE"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAFINE", SqlDbType.NVarChar)).Value = FncDate.GiraData(oMyConduzione.dDataFine)
                    End If
                    If oMyConduzione.nCodStatoOccupazione > 0 Then
                        cmdMyCommand.CommandText += ",@STATOOCCUPAZIONE"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@STATOOCCUPAZIONE", SqlDbType.Int)).Value = oMyConduzione.nCodStatoOccupazione
                    End If
                    If oMyConduzione.nCodTipoOccupazione > 0 Then
                        cmdMyCommand.CommandText += ",@TIPOOCCUPAZIONE"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPOOCCUPAZIONE", SqlDbType.Int)).Value = oMyConduzione.nCodTipoOccupazione
                    End If
                    cmdMyCommand.CommandText += " )"
                    cmdMyCommand.CommandText += " SELECT @@IDENTITY AS COD_CONDUZIONE"
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCLAS", SqlDbType.Int)).Value = oMyConduzione.nIdClassificazione
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAINIZIO", SqlDbType.NVarChar)).Value = FncDate.GiraData(oMyConduzione.dDataInizio)

                Case TYPEOPERATION_UPDATE
                    cmdMyCommand.CommandText = "UPDATE UNITA_IMMOBILIARI_CONDUZIONE SET"
                    cmdMyCommand.CommandText += " DATA_INIZIO=@DATAINIZIO"
                    If oMyConduzione.dDataFine <> Date.MinValue And oMyConduzione.dDataFine <> Date.MaxValue Then
                        cmdMyCommand.CommandText += ",DATA_FINE=@DATAFINE"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAFINE", SqlDbType.NVarChar)).Value = FncDate.GiraData(oMyConduzione.dDataFine)
                    Else
                        cmdMyCommand.CommandText += ",DATA_FINE=NULL"
                    End If
                    If oMyConduzione.nCodStatoOccupazione > 0 Then
                        cmdMyCommand.CommandText += ",COD_STATO_OCCUPAZIONE=@STATOOCCUPAZIONE"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@STATOOCCUPAZIONE", SqlDbType.Int)).Value = oMyConduzione.nCodStatoOccupazione
                    Else
                        cmdMyCommand.CommandText += ",COD_STATO_OCCUPAZIONE=NULL"
                    End If
                    If oMyConduzione.nCodTipoOccupazione > 0 Then
                        cmdMyCommand.CommandText += ",COD_TIPO_OCCUPAZIONE=@TIPOOCCUPAZIONE"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPOOCCUPAZIONE", SqlDbType.Int)).Value = oMyConduzione.nCodTipoOccupazione
                    Else
                        cmdMyCommand.CommandText += ",COD_TIPO_OCCUPAZIONE=NULL"
                    End If
                    cmdMyCommand.CommandText += " WHERE (COD_CONDUZIONE=@IDCONDUZIONE)"
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONDUZIONE", SqlDbType.Int)).Value = oMyConduzione.nIdConduzione
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCLAS", SqlDbType.Int)).Value = oMyConduzione.nIdClassificazione
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAINIZIO", SqlDbType.NVarChar)).Value = FncDate.GiraData(oMyConduzione.dDataInizio)
                Case TYPEOPERATION_DELETE
            End Select

            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return cmdMyCommand
        Catch Err As Exception
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Log.Error("Si è verificato un errore in ClsDB::GetSQLGestioneConduzione::" & Err.Message)
            Return Nothing
        End Try
    End Function

    Public Function GetSQLGestioneConduttore(myStringConnection As String, ByVal nTypeOperation As Integer, ByVal oMyConduttore As ObjConduzione) As SqlClient.SqlDataReader
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            'prelevo i dati della testata

            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.Parameters.Clear()
            Select Case nTypeOperation
                Case TYPEOPERATION_INSERT
                    cmdMyCommand.CommandText = "INSERT INTO UNITA_IMMOBILIARI_CONDUTTORI(COD_CONDUZIONE"
                    cmdMyCommand.CommandText += ",COD_ANAGRAFICA"
                    cmdMyCommand.CommandText += ",PERCENTUALE_UTILIZZO"
                    cmdMyCommand.CommandText += ",MOTIVO_UTILIZZO"
                    cmdMyCommand.CommandText += ",NUM_PERSONE_OCCUPANTI"
                    cmdMyCommand.CommandText += ",NOTE"
                    If oMyConduttore.nTipoUtilizzo > 0 Then
                        cmdMyCommand.CommandText += ",COD_TIPO_UTILIZZO"
                    End If
                    cmdMyCommand.CommandText += ")"
                    cmdMyCommand.CommandText += " VALUES(@IDCONDUZIONE"
                    cmdMyCommand.CommandText += ",@IDANAGRAFICA"
                    cmdMyCommand.CommandText += ",@PERCENTUTILIZZO"
                    cmdMyCommand.CommandText += ",@MOTIVOUTILIZZO"
                    cmdMyCommand.CommandText += ",@NOCCUPANTI"
                    cmdMyCommand.CommandText += ",@NOTE"
                    If oMyConduttore.nTipoUtilizzo > 0 Then
                        cmdMyCommand.CommandText += ",@TIPOUTILIZZO"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPOUTILIZZO", SqlDbType.Int)).Value = oMyConduttore.nTipoUtilizzo
                    End If
                    cmdMyCommand.CommandText += " )"
                    cmdMyCommand.CommandText += " SELECT @@IDENTITY AS COD_CONDUTTORE"
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONDUZIONE", SqlDbType.Int)).Value = oMyConduttore.nIdConduzione
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDANAGRAFICA", SqlDbType.Int)).Value = oMyConduttore.nIdAnagrafica
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOTE", SqlDbType.NVarChar)).Value = oMyConduttore.sNote
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PERCENTUTILIZZO", SqlDbType.Float)).Value = oMyConduttore.nPercentUtilizzo
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@MOTIVOUTILIZZO", SqlDbType.NVarChar)).Value = oMyConduttore.sMotivoUtilizzo
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOCCUPANTI", SqlDbType.Int)).Value = oMyConduttore.nOccupanti

                Case TYPEOPERATION_UPDATE
                Case TYPEOPERATION_DELETE
                    cmdMyCommand.CommandText = "DELETE"
                    cmdMyCommand.CommandText += " FROM UNITA_IMMOBILIARI_CONDUTTORI"
                    cmdMyCommand.CommandText += " WHERE (COD_CONDUTTORE=@IDCONDUTTORE)"
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONDUTTORE", SqlDbType.Int)).Value = oMyConduttore.nIdConduttore
            End Select

            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return cmdMyCommand.ExecuteReader
        Catch Err As Exception
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Log.Error("Si è verificato un errore in ClsDB::GetSQLGestioneConduttore::" & Err.Message)
            Return Nothing
        End Try
    End Function

    Public Function GetSQLVano(myStringConnection As String, ByVal nIdClas As Integer, ByVal nIdVano As Integer) As SqlClient.SqlCommand
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.CommandText = "SELECT *"
            cmdMyCommand.CommandText += " FROM V_VANI"
            cmdMyCommand.CommandText += " WHERE (1=1)"
            'VALORIZZO I PARAMETERS:
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.CommandText += " AND ((@IDVANO<=0 AND COD_CLASSIFICAZIONE=@IDCLAS) OR COD_VANO=@IDVANO)"
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCLAS", SqlDbType.Int)).Value = nIdClas
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDVANO", SqlDbType.Int)).Value = nIdVano

            Return cmdMyCommand '.ExecuteReader
        Catch Err As Exception
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Log.Error("Si è verificato un errore in ClsDB::GetSQLVano::" & Err.Message)
            Return Nothing
        End Try
    End Function

    Public Function GetSQLGestioneVano(myStringConnection As String, ByVal nTypeOperation As Integer, ByVal oMyVano As ObjVano) As SqlClient.SqlDataReader
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.Parameters.Clear()
            Select Case nTypeOperation
                Case TYPEOPERATION_INSERT
                    cmdMyCommand.CommandText = "INSERT INTO UNITA_IMMOBILIARI_VANI(COD_CLASSIFICAZIONE"
                    cmdMyCommand.CommandText += ",COD_TIPO_VANO"
                    cmdMyCommand.CommandText += ",MQ"
                    cmdMyCommand.CommandText += ",NOTE"
                    If oMyVano.nPesoCat > 0 Then
                        cmdMyCommand.CommandText += ",PESO_CATASTALE"
                    End If
                    If oMyVano.nPercentUso > 0 Then
                        cmdMyCommand.CommandText += ",PERCENTUALE_UTILIZZO"
                    End If
                    If oMyVano.nPiano > 0 Then
                        cmdMyCommand.CommandText += ",COD_PIANO"
                    End If
                    cmdMyCommand.CommandText += ")"
                    cmdMyCommand.CommandText += " VALUES(@IDCLAS"
                    cmdMyCommand.CommandText += ",@TIPOVANO"
                    cmdMyCommand.CommandText += ",@MQ"
                    cmdMyCommand.CommandText += ",@NOTE"
                    If oMyVano.nPesoCat > 0 Then
                        cmdMyCommand.CommandText += ",@PESOCAT"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PESOCAT", SqlDbType.Float)).Value = oMyVano.nPesoCat
                    End If
                    If oMyVano.nPercentUso > 0 Then
                        cmdMyCommand.CommandText += ",@PERCENTUSO"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PERCENTUSO", SqlDbType.Int)).Value = oMyVano.nPercentUso
                    End If
                    If oMyVano.nPiano > 0 Then
                        cmdMyCommand.CommandText += ",@PIANO"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PIANO", SqlDbType.Int)).Value = oMyVano.nPiano
                    End If
                    cmdMyCommand.CommandText += " )"
                    cmdMyCommand.CommandText += " SELECT @@IDENTITY AS COD_VANO"
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCLAS", SqlDbType.Int)).Value = oMyVano.nIdClassificazione
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPOVANO", SqlDbType.Int)).Value = oMyVano.nTipoVano
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@MQ", SqlDbType.Float)).Value = oMyVano.nMQ
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOTE", SqlDbType.NVarChar)).Value = oMyVano.sNote

                Case TYPEOPERATION_UPDATE
                    cmdMyCommand.CommandText = "UPDATE UNITA_IMMOBILIARI_VANI SET"
                    cmdMyCommand.CommandText += " COD_CLASSIFICAZIONE=@IDCLAS"
                    cmdMyCommand.CommandText += ",COD_TIPO_VANO=@TIPOVANO"
                    cmdMyCommand.CommandText += ",MQ=@MQ"
                    cmdMyCommand.CommandText += ",NOTE=@NOTE"
                    If oMyVano.nPesoCat > 0 Then
                        cmdMyCommand.CommandText += ",PESO_CATASTALE=@PESOCAT"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PESOCAT", SqlDbType.Float)).Value = oMyVano.nPesoCat
                    Else
                        cmdMyCommand.CommandText += ",PESO_CATASTALE=NULL"
                    End If
                    If oMyVano.nPercentUso > 0 Then
                        cmdMyCommand.CommandText += ",PERCENTUALE_UTILIZZO=@PERCENTUSO"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PERCENTUSO", SqlDbType.Int)).Value = oMyVano.nPercentUso
                    Else
                        cmdMyCommand.CommandText += ",PERCENTUALE_UTILIZZO=NULL"
                    End If
                    If oMyVano.nPiano > 0 Then
                        cmdMyCommand.CommandText += ",COD_PIANO=@PIANO"
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PIANO", SqlDbType.Int)).Value = oMyVano.nPiano
                    Else
                        cmdMyCommand.CommandText += ",COD_PIANO=NULL"
                    End If
                    cmdMyCommand.CommandText += " WHERE (COD_VANO=@IDVANO)"
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDVANO", SqlDbType.Int)).Value = oMyVano.nIdVano
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCLAS", SqlDbType.Int)).Value = oMyVano.nIdClassificazione
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPOVANO", SqlDbType.Int)).Value = oMyVano.nTipoVano
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@MQ", SqlDbType.Float)).Value = oMyVano.nMQ
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOTE", SqlDbType.NVarChar)).Value = oMyVano.sNote
                Case TYPEOPERATION_DELETE
            End Select

            Return cmdMyCommand.ExecuteReader
        Catch Err As Exception
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Log.Error("Si è verificato un errore in ClsDB::GetSQLGestioneVano::" & Err.Message)
            Return Nothing
        End Try
    End Function
#End Region
#Region "INSERT/UPDATE"
    Public Function SetFabbricato(sMyConn As String, ByVal oMyFabbricato As ObjFabbricato, ByRef cmdOutCommand As SqlClient.SqlCommand) As Integer ', ByVal oMyStradario As ObjStradario
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myDataReader As SqlClient.SqlDataReader = Nothing

        Try
            If cmdOutCommand Is Nothing Then
                Connect(sMyConn)
                cmdMyCommand = CreateCommand()
            Else
                cmdMyCommand = cmdOutCommand
            End If
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.Parameters.Add("@COD_ENTE", SqlDbType.NVarChar).Value = oMyFabbricato.sIdEnte
            cmdMyCommand.Parameters.Add("@COD_FABBRICATO", SqlDbType.Int).Value = oMyFabbricato.nIdFabbricato
            cmdMyCommand.Parameters.Add("@COD_STRADA", SqlDbType.Int).Value = oMyFabbricato.nCodVia
            cmdMyCommand.Parameters.Add("@NOME_FABBRICATO", SqlDbType.NVarChar).Value = oMyFabbricato.sNomeFabbricato
            cmdMyCommand.Parameters.Add("@POSIZIONE_CIVICO", SqlDbType.NVarChar).Value = oMyFabbricato.sPosizioneCivico
            cmdMyCommand.Parameters.Add("@NUM_CIVICO", SqlDbType.Int).Value = oMyFabbricato.nNumeroCivico
            cmdMyCommand.Parameters.Add("@ESPONENTE_CIVICO", SqlDbType.NVarChar).Value = oMyFabbricato.sEsponenteCivico
            cmdMyCommand.Parameters.Add("@FLAG_SN", SqlDbType.Int).Value = CInt(oMyFabbricato.bSenzaNumero)
            cmdMyCommand.Parameters.Add("@COD_TIPO_FABBRICATO", SqlDbType.NVarChar).Value = oMyFabbricato.sCodTipoFab
            cmdMyCommand.Parameters.Add("@FLAG_CONDOMINIO", SqlDbType.Bit).Value = oMyFabbricato.bCondominio
            cmdMyCommand.Parameters.Add("@COD_CONDOMINIO", SqlDbType.Int).Value = oMyFabbricato.oCondominio.nIdCondominio
            cmdMyCommand.Parameters.Add("@FLAG_PRESENZA_CORTILE", SqlDbType.Bit).Value = oMyFabbricato.bCortile
            cmdMyCommand.Parameters.Add("@INFO_SEMINTERRATO", SqlDbType.NVarChar).Value = oMyFabbricato.sCodSeminterrato
            cmdMyCommand.Parameters.Add("@INFO_SOTTOTETTO", SqlDbType.NVarChar).Value = oMyFabbricato.sCodSottotetto
            cmdMyCommand.Parameters.Add("@NUM_PIANI", SqlDbType.Int).Value = oMyFabbricato.nPiani
            cmdMyCommand.Parameters.Add("@NUM_CAMPANELLI", SqlDbType.Int).Value = oMyFabbricato.nCampanelli
            cmdMyCommand.Parameters.Add("@NUM_ALLOGGI", SqlDbType.Int).Value = oMyFabbricato.nAlloggi
            cmdMyCommand.Parameters.Add("@NUM_AUTORIMESSE", SqlDbType.Int).Value = oMyFabbricato.nAutorimesse
            cmdMyCommand.Parameters.Add("@NUM_DEPOSITI", SqlDbType.Int).Value = oMyFabbricato.nDepositi
            cmdMyCommand.Parameters.Add("@NUM_TETTOIE", SqlDbType.Int).Value = oMyFabbricato.nTettoie
            cmdMyCommand.Parameters.Add("@NUM_NEGOZI", SqlDbType.Int).Value = oMyFabbricato.nNegozi
            cmdMyCommand.Parameters.Add("@NUM_LABORATORI", SqlDbType.Int).Value = oMyFabbricato.nLaboratori
            cmdMyCommand.Parameters.Add("@NUM_UFFICI", SqlDbType.Int).Value = oMyFabbricato.nUffici
            cmdMyCommand.Parameters.Add("@NUM_RESIDENTI", SqlDbType.Int).Value = 0
            cmdMyCommand.Parameters.Add("@ID_UTENZA", SqlDbType.Int).Value = 0
            cmdMyCommand.Parameters.Add("@ID_REFERENTE", SqlDbType.Int).Value = 0
            cmdMyCommand.Parameters.Add("@CAT_CATASTALE_TEORICA", SqlDbType.NVarChar).Value = oMyFabbricato.sCatCatastale
            cmdMyCommand.Parameters.Add("@STATO_FABBRICATO", SqlDbType.NVarChar).Value = ""
            cmdMyCommand.Parameters.Add("@COD_STATO_CONSERVAZIONE", SqlDbType.Int).Value = oMyFabbricato.nStatoConservazione
            cmdMyCommand.Parameters.Add("@NOTE_DATI_GENERALI", SqlDbType.NVarChar).Value = oMyFabbricato.sNote
            cmdMyCommand.Parameters.Add("@COD_AREA_A", SqlDbType.Int).Value = oMyFabbricato.nAreeAmministrative
            cmdMyCommand.Parameters.Add("@COD_AREA_U", SqlDbType.Int).Value = oMyFabbricato.nAreeUrbanistiche
            cmdMyCommand.Parameters.Add("@COD_ZONA", SqlDbType.Int).Value = oMyFabbricato.nZona
            cmdMyCommand.Parameters.Add("@COD_MICROZONA", SqlDbType.Int).Value = oMyFabbricato.nMicrozona
            cmdMyCommand.Parameters.Add("@COD_ECOGRAFICO", SqlDbType.Int).Value = 0
            cmdMyCommand.Parameters.Add("@COD_STRADA_1", SqlDbType.Int).Value = 0
            cmdMyCommand.Parameters.Add("@NUM_CIVICO_1", SqlDbType.NVarChar).Value = ""
            cmdMyCommand.Parameters.Add("@NUMERO_CIVICO", SqlDbType.Int).Value = 0
            cmdMyCommand.Parameters.Add("@STATO_ASCENSORE", SqlDbType.Int).Value = 0
            cmdMyCommand.Parameters.Add("@STATO_BARRIERE", SqlDbType.Int).Value = 0
            cmdMyCommand.Parameters.Add("@STATO_AMIANTO", SqlDbType.Int).Value = 0
            cmdMyCommand.Parameters.Add("@COD_TIPO_COPERTURA", SqlDbType.Int).Value = 0
            cmdMyCommand.Parameters.Add("@STATO_INTERESSE_STORICO", SqlDbType.Int).Value = 0
            cmdMyCommand.Parameters.Add("@FLAG_SOGGETTO_CALAMITA", SqlDbType.Int).Value = 0
            cmdMyCommand.Parameters.Add("@NOTE_CARATTERISTICHE", SqlDbType.NVarChar).Value = ""
            cmdMyCommand.Parameters.Add("@DATA_COSTRUZIONE", SqlDbType.NVarChar).Value = ""
            cmdMyCommand.Parameters.Add("@DATA_ULTIMA_RISTRUTTURAZIONE", SqlDbType.NVarChar).Value = ""
            cmdMyCommand.Parameters.Add("@PROVENIENZA_CATASTO", SqlDbType.NVarChar).Value = ""
            cmdMyCommand.Parameters.Add("@FABBRICATO_DEFINITO", SqlDbType.NVarChar).Value = ""
            cmdMyCommand.Parameters.Add("@TARGA", SqlDbType.Bit).Value = False
            cmdMyCommand.Parameters("@COD_FABBRICATO").Direction = ParameterDirection.InputOutput
            cmdMyCommand.CommandText = "prc_FABBRICATI_IU"
            'eseguo la query
            Try
                cmdMyCommand.ExecuteNonQuery()
                oMyFabbricato.nIdFabbricato = cmdMyCommand.Parameters("@COD_FABBRICATO").Value
                If oMyFabbricato.nIdFabbricato <= 0 Then
                    Return 0
                End If
            Catch ex As Exception
                Log.Error("Si è verificato un errore in GestioneFabbricato::SetFabbricato::", ex)
                Return 0
            End Try
            'svuoto gli eventuali dati del condominio inseriti in precedenza
            cmdMyCommand = GetSQLGestioneCondominio(sMyConn, TYPEOPERATION_DELETE, oMyFabbricato)
            If cmdMyCommand.ExecuteNonQuery > 1 Then
                Return 0
            End If
            'se il fabbricato che sto inserendo è un condominio, inserimento record in CONDOMINI_STRADARIO
            If oMyFabbricato.bCondominio = True Then
                'eseguo la query
                cmdMyCommand = GetSQLGestioneCondominio(sMyConn, TYPEOPERATION_INSERT, oMyFabbricato)
                Try
                    myDataReader = cmdMyCommand.ExecuteReader
                    Do While myDataReader.Read
                        oMyFabbricato.oCondominio.nIdCondominio = myDataReader(0)
                    Loop
                Catch ex As Exception
                    Log.Error("Si è verificato un errore in GestioneFabbricato::SetFabbricato::", ex)
                Finally
                    myDataReader.Close()
                End Try
                If oMyFabbricato.oCondominio.nIdCondominio < 1 Then
                    Return -2
                End If
            End If

            Return oMyFabbricato.nIdFabbricato
        Catch Err As Exception
            Log.Error("Si è verificato un errore in GestioneFabbricato::SetFabbricato::" & Err.Message)
            Return -1
        Finally
            If cmdOutCommand Is Nothing Then
                Disconnect(cmdMyCommand)
            End If
        End Try
    End Function
    Public Function SetUI(sMyConn As String, ByVal oMyUI As ObjUnitaImmobiliare, ByRef cmdOutCommand As SqlClient.SqlCommand) As Integer
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            If cmdOutCommand Is Nothing Then
                Connect(sMyConn)
                cmdMyCommand = CreateCommand()
            Else
                cmdMyCommand = cmdOutCommand
            End If
            'Dim myDataReader As SqlClient.SqlDataReader = Nothing
            'Dim myIdentity As Integer = 0
            'eseguo la query
            'cmdMyCommand.CommandType = CommandType.Text
            'If oMyUI.nIdUnitaImmobiliare > 0 Then
            '    cmdMyCommand = GetSQLGestioneUnitaImmobiliare(cmdMyCommand, TYPEOPERATION_UPDATE, oMyUI)
            '    If cmdMyCommand.ExecuteNonQuery > 1 Then
            '        Return 0
            '    End If
            '    myIdentity = oMyUI.nIdUnitaImmobiliare
            'Else
            '    cmdMyCommand = GetSQLGestioneUnitaImmobiliare(cmdMyCommand, TYPEOPERATION_INSERT, oMyUI)
            '    myDataReader = cmdMyCommand.ExecuteReader
            '    Do While myDataReader.Read
            '        myIdentity = myDataReader(0)
            '    Loop
            '    myDataReader.Close()
            'End If
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_UNITA_IMMOBILIARI_IU"
            cmdMyCommand.Parameters.Add("@COD_UNITA_IMMOBILIARE", SqlDbType.Int).Value = -1
            cmdMyCommand.Parameters.Add("@COD_ENTE", SqlDbType.VarChar).Value = oMyUI.sIdEnte
            cmdMyCommand.Parameters.Add("@COD_FABBRICATO", SqlDbType.Int).Value = oMyUI.nIdFabbricato
            cmdMyCommand.Parameters.Add("@NUMERO_UI", SqlDbType.Int).Value = -1
            cmdMyCommand.Parameters.Add("@FOGLIO", SqlDbType.VarChar).Value = oMyUI.nFoglio
            cmdMyCommand.Parameters.Add("@NUMERO", SqlDbType.VarChar).Value = oMyUI.sNumero
            cmdMyCommand.Parameters.Add("@SUB", SqlDbType.Int).Value = oMyUI.nSubalterno
            cmdMyCommand.Parameters.Add("@FLAG_RIFERIMENTO_CATASTALE_CERTO", SqlDbType.Bit).Value = False
            cmdMyCommand.Parameters.Add("@INTERNO", SqlDbType.VarChar).Value = oMyUI.sInterno
            cmdMyCommand.Parameters.Add("@COD_ZONA_CENSUARIA", SqlDbType.Int).Value = oMyUI.nCodZonaCensuaria
            cmdMyCommand.Parameters.Add("@COD_PIANO", SqlDbType.Int).Value = 0
            cmdMyCommand.Parameters.Add("@GRAFFATURA", SqlDbType.VarChar).Value = oMyUI.sGraffatura
            cmdMyCommand.Parameters.Add("@PROV_CATASTALE", SqlDbType.VarChar).Value = oMyUI.sProvCatastale
            cmdMyCommand.Parameters.Add("@COD_ECOGRAFO", SqlDbType.VarChar).Value = oMyUI.sCodEcografico
            cmdMyCommand.Parameters.Add("@SEZIONE", SqlDbType.VarChar).Value = oMyUI.sSezione
            cmdMyCommand.Parameters.Add("@NOTE", SqlDbType.VarChar).Value = oMyUI.sNote
            cmdMyCommand.Parameters.Add("@DATA_INIZIO", SqlDbType.VarChar).Value = oMyUI.dDal
            cmdMyCommand.Parameters.Add("@DATA_FINE", SqlDbType.VarChar).Value = oMyUI.dAl
            cmdMyCommand.Parameters.Add("@MOTIVO_CESSAZIONE", SqlDbType.VarChar).Value = oMyUI.sCodCessazione
            cmdMyCommand.Parameters.Add("@NUMERO_UI_ORIGINE", SqlDbType.Int).Value = 0
            cmdMyCommand.Parameters.Add("@NUMERO_UI_DESTINAZIONE", SqlDbType.Int).Value = 0
            cmdMyCommand.Parameters.Add("@SCALA", SqlDbType.VarChar).Value = oMyUI.sScala
            cmdMyCommand.Parameters.Add("@INTERNO_CORTILE", SqlDbType.VarChar).Value = oMyUI.sInternoCortile
            cmdMyCommand.Parameters.Add("@INTERNO_GARAGE", SqlDbType.VarChar).Value = oMyUI.sInternoGarage
            cmdMyCommand.Parameters.Add("@IDORGFAB", SqlDbType.Int).Value = 0
            cmdMyCommand.Parameters.Add("@IDORGFABIMMO", SqlDbType.Int).Value = 0

            cmdMyCommand.Parameters("@COD_UNITA_IMMOBILIARE").Direction = ParameterDirection.InputOutput
            Try
                cmdMyCommand.ExecuteNonQuery()
                oMyUI.nIdUnitaImmobiliare = cmdMyCommand.Parameters("@COD_UNITA_IMMOBILIARE").Value
            Catch ex As Exception
                Log.Error("Si è verificato un errore in SetUI::", ex)
                Return 0
            End Try
            Return oMyUI.nIdUnitaImmobiliare
        Catch Err As Exception
            Log.Error("Si è verificato un errore in SetUI::" & Err.Message)
            Return -1
        Finally
            If cmdOutCommand Is Nothing Then
                Disconnect(cmdMyCommand)
            End If
        End Try
    End Function
    Public Function SetClassificazione(sMyConn As String, ByVal oMyClas As ObjClassificazione, ByRef cmdOutCommand As SqlClient.SqlCommand) As Integer
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            If cmdOutCommand Is Nothing Then
                Connect(sMyConn)
                cmdMyCommand = CreateCommand()
            Else
                cmdMyCommand = cmdOutCommand
            End If
            'Dim myDataReader As SqlClient.SqlDataReader = Nothing
            'Dim myIdentity As Integer = 0
            'cmdMyCommand.CommandType = CommandType.Text
            ''eseguo la query
            'If oMyClas.nIdClassificazione > 0 Then
            '    cmdMyCommand = GetSQLGestioneClassificazione(cmdMyCommand, TYPEOPERATION_UPDATE, oMyClas)
            '    If cmdMyCommand.ExecuteNonQuery > 1 Then
            '        Return 0
            '    End If
            '    myIdentity = oMyClas.nIdClassificazione
            'Else
            '    cmdMyCommand = GetSQLGestioneClassificazione(cmdMyCommand, TYPEOPERATION_INSERT, oMyClas)
            '    myDataReader = cmdMyCommand.ExecuteReader
            '    Do While myDataReader.Read
            '        myIdentity = myDataReader(0)
            '    Loop
            '    myDataReader.Close()
            'End If
            'Return myIdentity
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_UNITA_IMMOBILIARI_CLASSIFICAZIONI_IU"
            cmdMyCommand.Parameters.Add("@COD_CLASSIFICAZIONE", SqlDbType.Int).Value = oMyClas.nIdClassificazione
            cmdMyCommand.Parameters.Add("@COD_UI", SqlDbType.Int).Value = oMyClas.nIdUI
            cmdMyCommand.Parameters.Add("@DATA_INIZIO", SqlDbType.VarChar).Value = oMyClas.dDal
            cmdMyCommand.Parameters.Add("@DATA_FINE", SqlDbType.VarChar).Value = oMyClas.dAl
            cmdMyCommand.Parameters.Add("@COD_CATEGORIA_CATASTALE", SqlDbType.VarChar).Value = oMyClas.sCodCategoriaCatastale
            cmdMyCommand.Parameters.Add("@CLASSE", SqlDbType.VarChar).Value = oMyClas.sCodClasse
            cmdMyCommand.Parameters.Add("@CONSISTENZA", SqlDbType.Float).Value = oMyClas.nConsistenza
            cmdMyCommand.Parameters.Add("@COD_TIPO_RENDITA", SqlDbType.Int).Value = oMyClas.nCodTipoRendita
            cmdMyCommand.Parameters.Add("@SUPERFICIE_CATASTALE", SqlDbType.Decimal).Value = oMyClas.nSuperficieCatastale
            cmdMyCommand.Parameters.Add("@SUPERFICIE_NETTA", SqlDbType.Decimal).Value = oMyClas.nSuperficieNetta
            cmdMyCommand.Parameters.Add("@SUPERFICIE_LORDA", SqlDbType.Decimal).Value = oMyClas.nSuperficieLorda
            cmdMyCommand.Parameters.Add("@VALORE_RENDITA", SqlDbType.VarChar).Value = oMyClas.sValoreRendita
            cmdMyCommand.Parameters.Add("@DATA_ATTRIBUZIONE_RENDITA", SqlDbType.VarChar).Value = oMyClas.dDataAttribuzione
            cmdMyCommand.Parameters.Add("@DATA_EFFICACIA", SqlDbType.VarChar).Value = oMyClas.dDataEfficacia
            cmdMyCommand.Parameters.Add("@DATA_PRESENTAZIONE_PROT", SqlDbType.VarChar).Value = oMyClas.dDataProtocollo
            cmdMyCommand.Parameters.Add("@NUMERO_PROTOCOLLO", SqlDbType.VarChar).Value = oMyClas.sNProtocollo
            cmdMyCommand.Parameters.Add("@DATA_EFFETTIVO_UTILIZZO", SqlDbType.VarChar).Value = oMyClas.dDataEffettivoUtilizzo
            cmdMyCommand.Parameters.Add("@DATA_FINE_LAVORI", SqlDbType.VarChar).Value = oMyClas.dDataFineLavori
            cmdMyCommand.Parameters.Add("@FLAG_PERTINENZA", SqlDbType.Bit).Value = oMyClas.bFlagPertinenza
            cmdMyCommand.Parameters.Add("@COD_UI_PERTINENZA", SqlDbType.Int).Value = 0
            cmdMyCommand.Parameters.Add("@DESCRIZIONE_PERTINENZA", SqlDbType.VarChar).Value = ""
            cmdMyCommand.Parameters.Add("@COD_DESTINAZIONE_USO", SqlDbType.Int).Value = oMyClas.nCodDestUso
            cmdMyCommand.Parameters.Add("@FLAG_INAGIBILITA", SqlDbType.Bit).Value = oMyClas.bInagibilita
            cmdMyCommand.Parameters.Add("@COD_INAGIBILITA", SqlDbType.Int).Value = oMyClas.nCodInagibilita
            cmdMyCommand.Parameters.Add("@NOTE", SqlDbType.VarChar).Value = oMyClas.sNote
            cmdMyCommand.Parameters.Add("@DIFFORMITACAT", SqlDbType.Int).Value = oMyClas.nDifformitaCat

            cmdMyCommand.Parameters("@COD_CLASSIFICAZIONE").Direction = ParameterDirection.InputOutput
            Try
                cmdMyCommand.ExecuteNonQuery()
                oMyClas.nIdClassificazione = cmdMyCommand.Parameters("@COD_CLASSIFICAZIONE").Value
            Catch ex As Exception
                Log.Error("Si è verificato un errore in SetClassificazione::", ex)
                Return 0
            End Try
            Return oMyClas.nIdClassificazione
        Catch Err As Exception
            Log.Error("Si è verificato un errore in SetClassificazione::", Err)
            Return -1
        Finally
            If cmdOutCommand Is Nothing Then
                Disconnect(cmdMyCommand)
            End If
        End Try
    End Function

    Public Function SetVano(sMyConn As String, ByVal oMyVano As ObjVano, ByRef cmdOutCommand As SqlClient.SqlCommand) As Integer
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            If cmdOutCommand Is Nothing Then
                Connect(sMyConn)
                cmdMyCommand = CreateCommand()
            Else
                cmdMyCommand = cmdOutCommand
            End If
            'Dim myDataReader As SqlClient.SqlDataReader = Nothing
            'Dim myIdentity As Integer = 0
            'cmdMyCommand.CommandType = CommandType.Text
            ''eseguo la query
            'If oMyClas.nIdVano > 0 Then
            '    cmdMyCommand = GetSQLGestioneVano(cmdMyCommand, TYPEOPERATION_UPDATE, oMyVano)
            '    If cmdMyCommand.ExecuteNonQuery > 1 Then
            '        Return 0
            '    End If
            '    myIdentity = oMyClas.nIdVano
            'Else
            '    cmdMyCommand = GetSQLGestioneVano(cmdMyCommand, TYPEOPERATION_INSERT, oMyVano)
            '    myDataReader = cmdMyCommand.ExecuteReader
            '    Do While myDataReader.Read
            '        myIdentity = myDataReader(0)
            '    Loop
            '    myDataReader.Close()
            'End If
            'Return myIdentity
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_UNITA_IMMOBILIARI_VANI_IU"
            cmdMyCommand.Parameters.Add("@COD_VANO", SqlDbType.Int).Value = oMyVano.nIdVano
            cmdMyCommand.Parameters.Add("@COD_CLASSIFICAZIONE", SqlDbType.Int).Value = oMyVano.nIdClassificazione
            cmdMyCommand.Parameters.Add("@COD_PIANO", SqlDbType.Int).Value = oMyVano.nPiano
            cmdMyCommand.Parameters.Add("@COD_TIPO_VANO", SqlDbType.VarChar).Value = oMyVano.nTipoVano
            cmdMyCommand.Parameters.Add("@MQ", SqlDbType.Float).Value = oMyVano.nMQ
            cmdMyCommand.Parameters.Add("@PERCENTUALE_UTILIZZO", SqlDbType.Int).Value = oMyVano.nPercentUso
            cmdMyCommand.Parameters.Add("@PESO_CATASTALE", SqlDbType.Float).Value = oMyVano.nPesoCat
            cmdMyCommand.Parameters.Add("@NOTE", SqlDbType.VarChar).Value = oMyVano.sNote
            cmdMyCommand.Parameters.Add("@NOMERASTER", SqlDbType.VarChar).Value = oMyVano.sNomeRaster
            cmdMyCommand.Parameters("@COD_VANO").Direction = ParameterDirection.InputOutput
            Try
                cmdMyCommand.ExecuteNonQuery()
                oMyVano.nIdVano = cmdMyCommand.Parameters("@COD_VANO").Value
            Catch ex As Exception
                Log.Error("Si è verificato un errore in SetVano::", ex)
                Return 0
            End Try
            Return oMyVano.nIdVano
        Catch Err As Exception
            Log.Error("Si è verificato un errore in GestioneVano::SetVano::" & Err.Message)
            Return -1
        Finally
            If cmdOutCommand Is Nothing Then
                Disconnect(cmdMyCommand)
            End If
        End Try
    End Function
    Public Function SetConduzione(sMyConn As String, ByVal oMyConduzione As ObjConduzione, ByRef cmdOutCommand As SqlClient.SqlCommand, ByRef TxtIdConduttore As TextBox) As Integer
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            If cmdOutCommand Is Nothing Then
                Connect(sMyConn)
                cmdMyCommand = CreateCommand()
            Else
                cmdMyCommand = cmdOutCommand
            End If
            'Dim myDataReader As SqlClient.SqlDataReader = Nothing
            'Dim myIdentity As Integer = 0
            ''eseguo la query
            'cmdMyCommand.CommandType = CommandType.Text
            'If oMyConduzione.nIdConduzione > 0 Then
            '    cmdMyCommand = GetSQLGestioneConduzione(cmdMyCommand, TYPEOPERATION_UPDATE, oMyConduzione)
            '    If cmdMyCommand.ExecuteNonQuery > 1 Then
            '        Return 0
            '    End If
            '    'svuoto i Conduttori inseriti in precedenza
            '    cmdMyCommand = GetSQLGestioneConduttore(cmdMyCommand, TYPEOPERATION_DELETE, oMyConduzione)
            '    If cmdMyCommand.ExecuteNonQuery > 1 Then
            '        Return 0
            '    End If
            '    cmdMyCommand = GetSQLGestioneConduttore(cmdMyCommand, TYPEOPERATION_INSERT, oMyConduzione)
            '    myDataReader = cmdMyCommand.ExecuteReader
            '    Do While myDataReader.Read
            '        myIdentity = myDataReader(0)
            '    Loop
            '    myDataReader.Close()
            '    TxtIdConduttore.Text = myIdentity
            'Else
            '    cmdMyCommand = GetSQLGestioneConduzione(cmdMyCommand, TYPEOPERATION_INSERT, oMyConduzione)
            '    myDataReader = cmdMyCommand.ExecuteReader
            '    Do While myDataReader.Read
            '        myIdentity = myDataReader(0)
            '    Loop
            '    myDataReader.Close()
            '    oMyConduzione.nIdConduzione = myIdentity
            '    If myIdentity > 0 Then
            '        cmdMyCommand = GetSQLGestioneConduttore(cmdMyCommand, TYPEOPERATION_INSERT, oMyConduzione)
            '        myDataReader = cmdMyCommand.ExecuteReader
            '        Do While myDataReader.Read
            '            myIdentity = myDataReader(0)
            '        Loop
            '        myDataReader.Close()
            '        TxtIdConduttore.Text = myIdentity
            '    Else
            '        Return -2
            '    End If
            'End If
            'myIdentity = oMyConduzione.nIdConduzione
            'Return myIdentity
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_UNITA_IMMOBILIARI_CONDUZIONE_IU"
            cmdMyCommand.Parameters.Add("@COD_CONDUZIONE", SqlDbType.Int).Value = oMyConduzione.nIdConduzione
            cmdMyCommand.Parameters.Add("@COD_VANO", SqlDbType.VarChar).Value = -1
            cmdMyCommand.Parameters.Add("@COD_CLASSIFICAZIONE", SqlDbType.Int).Value = oMyConduzione.nIdClassificazione
            cmdMyCommand.Parameters.Add("@DATA_INIZIO", SqlDbType.VarChar).Value = oMyConduzione.dDataInizio
            cmdMyCommand.Parameters.Add("@DATA_FINE", SqlDbType.VarChar).Value = oMyConduzione.dDataFine
            cmdMyCommand.Parameters.Add("@COD_STATO_OCCUPAZIONE", SqlDbType.Int).Value = oMyConduzione.nCodStatoOccupazione
            cmdMyCommand.Parameters.Add("@COD_TIPO_OCCUPAZIONE", SqlDbType.Int).Value = oMyConduzione.nCodTipoOccupazione

            cmdMyCommand.Parameters("@COD_CONDUZIONE").Direction = ParameterDirection.InputOutput
            Try
                cmdMyCommand.ExecuteNonQuery()
                oMyConduzione.nIdConduzione = cmdMyCommand.Parameters("@COD_CONDUZIONE").Value
                If oMyConduzione.nIdConduzione > 0 Then
                    cmdMyCommand.Parameters.Clear()
                    cmdMyCommand.CommandText = "prc_UNITA_IMMOBILIARI_CONDUTTORI_IU"
                    cmdMyCommand.Parameters.Add("@COD_CONDUTTORE", SqlDbType.Int).Value = oMyConduzione.nIdConduttore
                    cmdMyCommand.Parameters.Add("@COD_ANAGRAFICA", SqlDbType.Int).Value = oMyConduzione.nIdAnagrafica
                    cmdMyCommand.Parameters.Add("@COD_CONDUZIONE", SqlDbType.Int).Value = oMyConduzione.nIdConduzione
                    cmdMyCommand.Parameters.Add("@PERCENTUALE_UTILIZZO", SqlDbType.Float).Value = oMyConduzione.nPercentUtilizzo
                    cmdMyCommand.Parameters.Add("@MOTIVO_UTILIZZO", SqlDbType.VarChar).Value = oMyConduzione.sMotivoUtilizzo
                    cmdMyCommand.Parameters.Add("@COD_TIPO_UTILIZZO", SqlDbType.Int).Value = oMyConduzione.nTipoUtilizzo
                    cmdMyCommand.Parameters.Add("@NUM_PERSONE_OCCUPANTI", SqlDbType.Int).Value = oMyConduzione.nOccupanti
                    cmdMyCommand.Parameters.Add("@NOTE", SqlDbType.VarChar).Value = oMyConduzione.sNote

                    cmdMyCommand.Parameters("@COD_CONDUTTORE").Direction = ParameterDirection.InputOutput
                    Try
                        cmdMyCommand.ExecuteNonQuery()
                        oMyConduzione.nIdConduttore = cmdMyCommand.Parameters("@COD_CONDUTTORE").Value
                        TxtIdConduttore.Text = oMyConduzione.nIdConduttore
                    Catch ex As Exception
                        Log.Error("Si è verificato un errore in SetConduttori::", ex)
                        Return -1
                    End Try
                Else
                    Return -2
                End If
            Catch ex As Exception
                Log.Error("Si è verificato un errore in SetConduzione::", ex)
                Return -1
            End Try
        Catch Err As Exception
            Log.Error("Si è verificato un errore in SetConduzione::", Err)
            Return -1
        Finally
            If cmdOutCommand Is Nothing Then
                Disconnect(cmdMyCommand)
            End If
        End Try
    End Function
    Public Function SetProprieta(sMyConn As String, ByVal oMyProprieta As ObjProprieta, ByRef cmdOutCommand As SqlClient.SqlCommand, ByRef TxtIdProprietario As TextBox) As Integer
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            If cmdOutCommand Is Nothing Then
                Connect(sMyConn)
                cmdMyCommand = CreateCommand()
            Else
                cmdMyCommand = cmdOutCommand
            End If
            'Dim myIdentity As Integer = 0
            'Dim myDataReader As SqlClient.SqlDataReader = Nothing
            'cmdMyCommand.CommandType = CommandType.Text
            ''eseguo la query
            'If oMyProprieta.nIdProprieta > 0 Then
            '    cmdMyCommand = GetSQLGestioneProprieta(cmdMyCommand, TYPEOPERATION_UPDATE, oMyProprieta)
            '    If cmdMyCommand.ExecuteNonQuery > 1 Then
            '        Return 0
            '    End If
            '    'svuoto i proprietari inseriti in precedenza
            '    cmdMyCommand = GetSQLGestioneProprietario(cmdMyCommand, TYPEOPERATION_DELETE, oMyProprieta)
            '    If cmdMyCommand.ExecuteNonQuery > 1 Then
            '        Return 0
            '    End If
            '    cmdMyCommand = GetSQLGestioneProprietario(cmdMyCommand, TYPEOPERATION_INSERT, oMyProprieta)
            '    myDataReader = cmdMyCommand.ExecuteReader
            '    Do While myDataReader.Read
            '        myIdentity = myDataReader(0)
            '    Loop
            '    myDataReader.Close()
            '    TxtIdProprietario.Text = myIdentity
            'Else
            '    cmdMyCommand = GetSQLGestioneProprieta(cmdMyCommand, TYPEOPERATION_INSERT, oMyProprieta)
            '    myDataReader = cmdMyCommand.ExecuteReader
            '    Do While myDataReader.Read
            '        myIdentity = myDataReader(0)
            '    Loop
            '    myDataReader.Close()
            '    oMyProprieta.nIdProprieta = myIdentity
            '    If myIdentity > 0 Then
            '        cmdMyCommand = GetSQLGestioneProprietario(cmdMyCommand, TYPEOPERATION_INSERT, oMyProprieta)
            '        myDataReader = cmdMyCommand.ExecuteReader
            '        Do While myDataReader.Read
            '            myIdentity = myDataReader(0)
            '        Loop
            '        myDataReader.Close()
            '        TxtIdProprietario.Text = myIdentity
            '    Else
            '         Return -2
            '    End If
            'End If
            'myIdentity = oMyProprieta.nIdProprieta
            'Return myIdentity
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_UNITA_IMMOBILIARI_PROPRIETA_IU"
            cmdMyCommand.Parameters.Add("@COD_PROPRIETA", SqlDbType.Int).Value = oMyProprieta.nIdProprieta
            cmdMyCommand.Parameters.Add("@COD_CLASSIFICAZIONE", SqlDbType.Int).Value = oMyProprieta.nIdClassificazione
            cmdMyCommand.Parameters.Add("@DATA_INIZIO", SqlDbType.VarChar).Value = oMyProprieta.dDataInizio
            cmdMyCommand.Parameters.Add("@DATA_FINE", SqlDbType.VarChar).Value = oMyProprieta.dDataFine

            cmdMyCommand.Parameters("@COD_PROPRIETA").Direction = ParameterDirection.InputOutput
            Try
                cmdMyCommand.ExecuteNonQuery()
                oMyProprieta.nIdProprieta = cmdMyCommand.Parameters("@COD_PROPRIETA").Value
                If oMyProprieta.nIdProprieta > 0 Then
                    cmdMyCommand.Parameters.Clear()
                    cmdMyCommand.CommandText = "prc_UNITA_IMMOBILIARI_PROPRIETARI_IU"
                    cmdMyCommand.Parameters.Add("@COD_PROPRIETARIO", SqlDbType.Int).Value = oMyProprieta.nIdProprietario
                    cmdMyCommand.Parameters.Add("@COD_ANAGRAFICA", SqlDbType.Int).Value = oMyProprieta.nIdAnagrafica
                    cmdMyCommand.Parameters.Add("@COD_PROPRIETA", SqlDbType.Int).Value = oMyProprieta.nIdProprieta
                    cmdMyCommand.Parameters.Add("@PERCENTUALE_PROPRIETA", SqlDbType.Float).Value = oMyProprieta.nPercentProprieta
                    cmdMyCommand.Parameters.Add("@COD_TIPO_PROPRIETA", SqlDbType.Int).Value = oMyProprieta.nTipoProprieta
                    cmdMyCommand.Parameters.Add("@PERCENTUALE_POSSESSO", SqlDbType.Float).Value = oMyProprieta.nPercentPossesso
                    cmdMyCommand.Parameters.Add("@COD_TIPO_POSSESSO", SqlDbType.Int).Value = oMyProprieta.nTipoPossesso
                    cmdMyCommand.Parameters.Add("@COD_UTILIZZO", SqlDbType.Int).Value = oMyProprieta.nTipoUtilizzo
                    cmdMyCommand.Parameters.Add("@COD_TIPO_PARENTELA", SqlDbType.Int).Value = oMyProprieta.nTipoParentela
                    cmdMyCommand.Parameters.Add("@NOTE", SqlDbType.VarChar).Value = oMyProprieta.sNote

                    cmdMyCommand.Parameters("@COD_PROPRIETARIO").Direction = ParameterDirection.InputOutput
                    Try
                        cmdMyCommand.ExecuteNonQuery()
                        oMyProprieta.nIdProprietario = cmdMyCommand.Parameters("@COD_PROPRIETARIO").Value
                        TxtIdProprietario.Text = oMyProprieta.nIdProprietario
                    Catch ex As Exception
                        Log.Error("Si è verificato un errore in SetProprietari::", ex)
                        Return -1
                    End Try
                Else
                    Return -2
                End If
            Catch ex As Exception
                Log.Error("Si è verificato un errore in SetProprietà::", ex)
                Return -1
            End Try
        Catch Err As Exception
            Log.Error("Si è verificato un errore in GestioneProprieta::SetProprieta::" & Err.Message)
            Return -1
        Finally
            If cmdOutCommand Is Nothing Then
                Disconnect(cmdMyCommand)
            End If
        End Try
    End Function
#End Region
#Region "Variazioni Tributarie"
    Public Function SearchVariazioniTributi(ByVal SearchParams As objVariazioniTributiSearch, ByVal sMyConn As String) As DataView
        Dim sSQL As String = ""
        Dim myCommandText As String = "prc_GetVariazioniTributi"
        Dim myDataView As New DataView

        Try
            If SearchParams.IdVariazione > 0 Then
                myCommandText = "prc_GetVariazioniTributiDettaglio"
            End If
            Using ctx As New DBModel(ConstSession.DBType, sMyConn)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, myCommandText, "IDENTE", "IdVariazione", "Tributo", "Foglio", "Numero", "Subalterno", "Dal", "Al", "Operatore", "IdCausale", "IsTrattato")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", SearchParams.IdEnte) _
                            , ctx.GetParam("IdVariazione", SearchParams.IdVariazione) _
                            , ctx.GetParam("Tributo", SearchParams.Tributo) _
                            , ctx.GetParam("Foglio", SearchParams.Foglio) _
                            , ctx.GetParam("Numero", SearchParams.Numero) _
                            , ctx.GetParam("Subalterno", SearchParams.Subalterno) _
                            , ctx.GetParam("Dal", SearchParams.Dal) _
                            , ctx.GetParam("Al", SearchParams.Al) _
                            , ctx.GetParam("Operatore", SearchParams.Operatore) _
                            , ctx.GetParam("IdCausale", SearchParams.IdCausale) _
                            , ctx.GetParam("IsTrattato", SearchParams.IsTrattato)
                        )
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTERRITORIO.ClsDB.SearchVariazioniTributi.erroreQuery: ", ex)
                    Return Nothing
                Finally
                    ctx.Dispose()
                End Try
            End Using
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTERRITORIO.ClsDB.SearchVariazioniTributi.errore: ", Err)
            myDataView = Nothing
        End Try
        Return myDataView
    End Function
    Public Function GetFabToInsert(ByVal IdVarTrib As Integer, ByVal sMyConn As String, myCommandText As String) As DataTable
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim myDataTable As DataTable = New DataTable

        Try
            Connect(sMyConn)
            cmdMyCommand = CreateCommand()
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = myCommandText
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdVariazione", SqlDbType.Int)).Value = IdVarTrib
            myAdapter.SelectCommand = cmdMyCommand
            myAdapter.Fill(myDataTable)
        Catch ex As Exception
            Log.Debug("GetFabToInsert::si è verificato il seguente errore::", ex)
            Throw
        Finally
            myAdapter.Dispose()
            Disconnect(cmdMyCommand)
        End Try
        Return myDataTable
    End Function
#End Region
#Region "Gestione Comuni"
    Public Function SetComuni(myStringConnection As String, myItem As OggettoEnte) As Boolean
        Dim sSQL As String = ""
        Dim myDataView As New DataView
        Dim bRet As Boolean = False
        Try
            Using ctx As New DBModel(ConstSession.DBType, myStringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_TBLCOMUNI_IU", "ID", "COMUNE", "PV", "CAP", "IDENTIFICATIVO", "COD_ISTAT")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("ID", myItem.ID) _
                        , ctx.GetParam("COMUNE", myItem.Denominazione) _
                        , ctx.GetParam("PV", myItem.Provincia) _
                        , ctx.GetParam("CAP", myItem.Cap) _
                        , ctx.GetParam("IDENTIFICATIVO", myItem.CodBelfiore) _
                        , ctx.GetParam("COD_ISTAT", myItem.CodIstat)
                    )
                Catch ex As Exception
                    Log.Debug(" - OPENgovTERRITORIO.ClsDB.SetComuni.erroreQuery: ", ex)
                    bRet = False
                Finally
                    ctx.Dispose()
                End Try
            End Using
            For Each dtMyRow As DataRowView In myDataView
                If StringOperation.FormatInt(dtMyRow("ID")) > 0 Then
                    bRet = True
                End If
            Next
        Catch Err As Exception
            Log.Debug(" - OPENgovTERRITORIO.ClsDB.SetComuni.errore: ", Err)
            bRet = False
        End Try
        Return bRet
    End Function
#End Region
End Class
