Imports log4net

Public Class GestioneFabbricato
    Inherits Ribes.OPENgov.Utilities.ClsDatabase
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(GestioneFabbricato))
    Private FncDB As New ClsDB

    Public Function GetFabbricato(myStringConnection As String, ByVal sIdEnte As String, ByVal nIdStradario As Integer, ByVal nIdFabbricato As Integer, ByVal nFoglio As Integer, ByVal sNumero As String, ByVal nSubalterno As Integer) As ObjFabbricato
        Dim dtMyDati As New DataTable()
        Dim oMyFab As New ObjFabbricato
        Dim FncUI As New GestioneUnitaImmobiliare

        Try
            dtMyDati = FncDB.GetSQLFabbricato(myStringConnection, sIdEnte, nIdStradario, nIdFabbricato, -1, -1, "", "", nFoglio, sNumero, nSubalterno)
            For Each dtMyRow As DataRow In dtMyDati.Rows
                oMyFab = New ObjFabbricato
                oMyFab.sIdEnte = CStr(dtMyRow("COD_ENTE"))
                If Not IsDBNull(dtMyRow("COD_FABBRICATO")) Then
                    oMyFab.nIdFabbricato = CInt(dtMyRow("COD_FABBRICATO"))
                End If
                oMyFab.sNomeFabbricato = CStr(dtMyRow("NOME_FABBRICATO"))
                oMyFab.nCodVia = CInt(dtMyRow("COD_STRADA"))
                If Not IsDBNull(dtMyRow("POSIZIONE_CIVICO")) Then
                    oMyFab.sPosizioneCivico = CStr(dtMyRow("POSIZIONE_CIVICO"))
                End If
                If Not IsDBNull(dtMyRow("NUM_CIVICO")) Then
                    oMyFab.nNumeroCivico = CInt(dtMyRow("NUM_CIVICO"))
                End If
                If Not IsDBNull(dtMyRow("ESPONENTE_CIVICO")) Then
                    oMyFab.sEsponenteCivico = CStr(dtMyRow("ESPONENTE_CIVICO"))
                End If
                If Not IsDBNull(dtMyRow("NUM_PIANI")) Then
                    oMyFab.nPiani = CInt(dtMyRow("NUM_PIANI"))
                End If
                If Not IsDBNull(dtMyRow("NUM_CAMPANELLI")) Then
                    oMyFab.nCampanelli = CInt(dtMyRow("NUM_CAMPANELLI"))
                End If
                If Not IsDBNull(dtMyRow("NUM_ALLOGGI")) Then
                    oMyFab.nAlloggi = CInt(dtMyRow("NUM_ALLOGGI"))
                End If
                If Not IsDBNull(dtMyRow("NUM_AUTORIMESSE")) Then
                    oMyFab.nAutorimesse = CInt(dtMyRow("NUM_AUTORIMESSE"))
                End If
                If Not IsDBNull(dtMyRow("NUM_DEPOSITI")) Then
                    oMyFab.nDepositi = CInt(dtMyRow("NUM_DEPOSITI"))
                End If
                If Not IsDBNull(dtMyRow("NUM_TETTOIE")) Then
                    oMyFab.nTettoie = CInt(dtMyRow("NUM_TETTOIE"))
                End If
                If Not IsDBNull(dtMyRow("NUM_NEGOZI")) Then
                    oMyFab.nNegozi = CInt(dtMyRow("NUM_NEGOZI"))
                End If
                If Not IsDBNull(dtMyRow("NUM_LABORATORI")) Then
                    oMyFab.nLaboratori = CInt(dtMyRow("NUM_LABORATORI"))
                End If
                If Not IsDBNull(dtMyRow("NUM_UFFICI")) Then
                    oMyFab.nUffici = CInt(dtMyRow("NUM_UFFICI"))
                End If
                If Not IsDBNull(dtMyRow("COD_STATO_CONSERVAZIONE")) Then
                    oMyFab.nStatoConservazione = CInt(dtMyRow("COD_STATO_CONSERVAZIONE"))
                End If
                If Not IsDBNull(dtMyRow("TARGA")) Then
                    oMyFab.bTarga = CBool(dtMyRow("TARGA"))
                End If
                If Not IsDBNull(dtMyRow("COD_AREA_A")) Then
                    oMyFab.nAreeAmministrative = CInt(dtMyRow("COD_AREA_A"))
                End If
                If Not IsDBNull(dtMyRow("COD_AREA_U")) Then
                    oMyFab.nAreeUrbanistiche = CInt(dtMyRow("COD_AREA_U"))
                End If
                If Not IsDBNull(dtMyRow("COD_ZONA")) Then
                    oMyFab.nZona = CInt(dtMyRow("COD_ZONA"))
                End If
                If Not IsDBNull(dtMyRow("COD_MICROZONA")) Then
                    oMyFab.nMicrozona = CInt(dtMyRow("COD_MICROZONA"))
                End If
                If Not IsDBNull(dtMyRow("FLAG_SN")) Then
                    oMyFab.bSenzaNumero = CBool(dtMyRow("FLAG_SN"))
                End If
                If Not IsDBNull(dtMyRow("COD_TIPO_FABBRICATO")) Then
                    oMyFab.sCodTipoFab = CStr(dtMyRow("COD_TIPO_FABBRICATO"))
                End If
                If Not IsDBNull(dtMyRow("FLAG_CONDOMINIO")) Then
                    oMyFab.bCondominio = CBool(dtMyRow("FLAG_CONDOMINIO"))
                End If
                If Not IsDBNull(dtMyRow("FLAG_PRESENZA_CORTILE")) Then
                    oMyFab.bCortile = CBool(dtMyRow("FLAG_PRESENZA_CORTILE"))
                End If
                If Not IsDBNull(dtMyRow("INFO_SEMINTERRATO")) Then
                    oMyFab.sCodSeminterrato = CStr(dtMyRow("INFO_SEMINTERRATO"))
                End If
                If Not IsDBNull(dtMyRow("INFO_SOTTOTETTO")) Then
                    oMyFab.sCodSottotetto = CStr(dtMyRow("INFO_SOTTOTETTO"))
                End If
                If Not IsDBNull(dtMyRow("NOTE_DATI_GENERALI")) Then
                    oMyFab.sNote = CStr(dtMyRow("NOTE_DATI_GENERALI"))
                End If
                If Not IsDBNull(dtMyRow("CAT_CATASTALE_TEORICA")) Then
                    oMyFab.sCatCatastale = CStr(dtMyRow("CAT_CATASTALE_TEORICA"))
                End If
                If Not IsDBNull(dtMyRow("COD_FABBRICATO_REALE")) Then
                    oMyFab.iCodFabbricatoReale = CInt(dtMyRow("COD_FABBRICATO_REALE"))
                End If

                If oMyFab.iCodFabbricatoReale > 0 Then
                    oMyFab.oListUnitaImmobiliari = FncUI.GetUI(myStringConnection, oMyFab.iCodFabbricatoReale, -1, False)
                End If
            Next
            Return oMyFab
        Catch Err As Exception
            Log.Error("Si è verificato un errore in GestioneFabbricato::GetFabbricato::" & Err.Message)
            Return Nothing
        Finally
            dtMyDati.Dispose()
        End Try
    End Function

    Public Function CheckUniqueFabbricato(myStringConnection As String, ByVal oMyFabbricato As ObjFabbricato) As Boolean
        Dim drDati As SqlClient.SqlDataReader

        Try
            drDati = FncDB.GetSQLRicercaFabbricato(myStringConnection, oMyFabbricato.sIdEnte, oMyFabbricato.nCodVia, oMyFabbricato.bSenzaNumero, oMyFabbricato.sPosizioneCivico, oMyFabbricato.nNumeroCivico, oMyFabbricato.sEsponenteCivico)
            If drDati.Read = True Then
                Return False
            Else
                Return True
            End If

        Catch Err As Exception
            Log.Error("Si è verificato un errore in GestioneFabbricato::CheckUniqueFabbricato::" & Err.Message)
            Return False
        Finally
            drDati.Close()
        End Try
    End Function

    Public Function SetFabbricato(myStringConnection As String, ByVal oMyFabbricato As ObjFabbricato, ByVal oMyStradario As ObjStradario) As Integer
        Dim DrReturn As SqlClient.SqlDataReader
        Dim myVal As Integer

        Try
            'WFSessione.oSession.oAppDB.BeginTrans()
            Dim dbEngine_ As DAL.DBEngine = Nothing
            Dim parameterCollection_ As IDataParameterCollection
            Dim parameter_ As IDataParameter

            dbEngine_ = DBEngineFactory.GetDBEngine()
            dbEngine_.OpenConnection()
            dbEngine_.BeginTransaction()

            dbEngine_ = FncDB.GetSQLGestioneFabbricati(dbEngine_, oMyFabbricato)
            parameterCollection_ = dbEngine_.ExecuteNonQuery("prc_FABBRICATI_IU", CommandType.StoredProcedure)
            parameter_ = CType(parameterCollection_("@IDNEWFABBRICATO"), IDataParameter)
            oMyFabbricato.nIdFabbricato = parameter_.Value
            If oMyFabbricato.nIdFabbricato > 0 Then
                dbEngine_.CommitTransaction()
            Else
                dbEngine_.RollbackTransaction()
            End If

            '        If oMyStradario.nIdStradario > 0 Then
            'DrReturn = WFSessione.oSession.oAppDB.GetPrivateDataReader(FncDB.GetSQLGestioneFabbricatiStradario(ClsDB.TYPEOPERATION_UPDATE, oMyStradario))
            '            Do While DrReturn.Read
            '                oMyStradario.nIdStradario = DrReturn(0)
            '            Loop
            '            DrReturn.Close()
            '        Else
            'DrReturn = WFSessione.oSession.oAppDB.GetPrivateDataReader(FncDB.GetSQLGestioneFabbricatiStradario(ClsDB.TYPEOPERATION_INSERT, oMyStradario))
            '            Do While DrReturn.Read
            '                oMyStradario.nIdStradario = DrReturn(0)
            '            Loop
            '            DrReturn.Close()
            'End If
            '' nuovo fabbricato, collego fabbricati e fabbricati_stradario
            'If myIdentity > 0 Then
            '    If WFSessione.oSession.oAppDB.Execute(FncDB.GetSQLGestioneFabbricatoMyStradario(oMyFabbricato, myIdentity)) > 1 Then
            '        ''WFSessione.oSession.oAppDB.CommitTrans()
            '        Return 0
            '    End If
            'Else
            '    myIdentity = oMyFabbricato.iCodFabbricatoReale
            'End If
            'svuoto gli eventuali dati del condominio inseriti in precedenza
            DrReturn = FncDB.GetSQLGestioneCondominio(myStringConnection, FncDB.TYPEOPERATION_DELETE, oMyFabbricato).ExecuteReader
            Do While DrReturn.Read
                myVal = DrReturn(0)
            Loop
            DrReturn.Close()
            If myVal > 1 Then
                ''WFSessione.oSession.oAppDB.CommitTrans()
                Return 0
            End If
            'se il fabbricato che sto inserendo è un condominio, inserimento record in CONDOMINI_STRADARIO
            If oMyFabbricato.bCondominio = True Then
                'eseguo la query
                DrReturn = FncDB.GetSQLGestioneCondominio(myStringConnection, FncDB.TYPEOPERATION_INSERT, oMyFabbricato).ExecuteReader
                Do While DrReturn.Read
                    oMyFabbricato.oCondominio.nIdCondominio = DrReturn(0)
                Loop
                DrReturn.Close()
                If oMyFabbricato.oCondominio.nIdCondominio < 1 Then
                    ''WFSessione.oSession.oAppDB.CommitTrans()
                    Return -2
                End If
            End If

            '        'inserisco in fabbricati
            '        If oMyFabbricato.iCodFabbricatoReale > 0 Then
            '            If WFSessione.oSession.oAppDB.Execute(FncDB.GetSQLGestioneFabbricato(FncDB.TYPEOPERATION_UPDATE, oMyFabbricato)) > 1 Then
            '                ''WFSessione.oSession.oAppDB.CommitTrans()
            '                Return 0
            '            End If
            '        Else
            'DrReturn = WFSessione.oSession.oAppDB.GetPrivateDataReader(FncDB.GetSQLGestioneFabbricato(FncDB.TYPEOPERATION_INSERT, oMyFabbricato))
            '            Do While DrReturn.Read
            '                oMyFabbricato.nIdFabbricato = DrReturn(0)
            '            Loop
            '            DrReturn.Close()
            '        End If
            'aggiorno il legame tra fabbricati_stradario e fabbricati
            'If WFSessione.oSession.oAppDB.Execute(FncDB.GetSQLGestioneFabbricatoStradario(oMyStradario.nIdStradario, oMyFabbricato.nIdFabbricato)) > 1 Then
            '    ''WFSessione.oSession.oAppDB.CommitTrans()
            '    Return 0
            'End If

            'WFSessione.oSession.oAppDB.Execute(FncDB.GetSQLUpdateFabbricatiStradario(oMyStradario))

            'WFSessione.oSession.oAppDB.CommitTrans()
            Return oMyFabbricato.nIdFabbricato
        Catch Err As Exception
            ''WFSessione.oSession.oAppDB.CommitTrans()
            Log.Error("Si è verificato un errore in GestioneFabbricato::SetFabbricato::" & Err.Message)
            Return -1
        End Try
    End Function
End Class

Public Class GestioneUnitaImmobiliare
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(GestioneUnitaImmobiliare))
    Private FncDate As New ModificaDate
    Private FncDB As New ClsDB

    Public Function GetDescrizioneUI(myStringConnection As String, ByVal nIdUI As Integer, ByRef LblNomeFab As Label, ByRef LblIndirizzo As Label, ByRef LblFoglio As Label, ByRef LblNumero As Label, ByRef LblSub As Label, ByRef LblDal As Label, ByRef LblAl As Label, ByRef LblInterno As Label, ByRef LblPiano As Label, ByRef LblZonaCens As Label) As Boolean
        Dim drDati As SqlClient.SqlDataReader

        Try
            drDati = FncDB.GetSQLDescrizioneUI(myStringConnection, nIdUI).ExecuteReader
            Do While drDati.Read
                LblNomeFab.Text = CStr(drDati("Nome_Fabbricato"))
                LblIndirizzo.Text = CStr(drDati("Indirizzo"))
                If Not IsDBNull(drDati("foglio")) Then
                    LblFoglio.Text = CStr(drDati("foglio"))
                Else
                    LblFoglio.Text = ""
                End If
                If Not IsDBNull(drDati("numero")) Then
                    LblNumero.Text = CStr(drDati("numero"))
                Else
                    LblNumero.Text = ""
                End If
                If Not IsDBNull(drDati("sub")) Then
                    LblSub.Text = CStr(drDati("sub"))
                Else
                    LblSub.Text = ""
                End If
                If Not IsDBNull(drDati("DATA_INIZIO")) Then
                    LblDal.Text = FncDate.GiraDataFromDB(CStr(drDati("DATA_INIZIO")))
                Else
                    LblDal.Text = ""
                End If
                If Not IsDBNull(drDati("DATA_FINE")) Then
                    If CStr(drDati("DATA_FINE")) <> "" Then
                        LblAl.Text = FncDate.GiraDataFromDB(CStr(drDati("DATA_FINE")))
                    Else
                        LblAl.Text = ""
                    End If
                Else
                    LblAl.Text = ""
                End If
                If Not IsDBNull(drDati("piano")) Then
                    LblPiano.Text = CStr(drDati("piano"))
                Else
                    LblPiano.Text = ""
                End If
                If Not IsDBNull(drDati("zonacens")) Then
                    LblZonaCens.Text = CStr(drDati("zonacens"))
                Else
                    LblZonaCens.Text = ""
                End If
            Loop

            Return True
        Catch ex As Exception
            Log.Error("Si è verificato un errore in GestioneUnitaImmobiliare::GetDescrizioneUI::" & ex.Message)
            Return False
        Finally
            drDati.Close()
        End Try
    End Function

    Public Function GetUI(myStringConnection As String, ByVal nIdFab As Integer, ByVal nIdUI As Integer, ByVal bLoadDettaglio As Boolean) As ObjUnitaImmobiliare()
        Dim drDati As SqlClient.SqlDataReader
        Dim oMyUI As ObjUnitaImmobiliare
        Dim oListUI() As ObjUnitaImmobiliare
        Dim nList As Integer = -1
        Dim FncClass As New GestioneClassificazione

        Try
            drDati = FncDB.GetSQLUnitaImmobiliare(myStringConnection, "", nIdFab, nIdUI, -1, "", -1).ExecuteReader
            Do While drDati.Read
                oMyUI = New ObjUnitaImmobiliare

                oMyUI.sIdEnte = CStr(drDati("COD_ENTE"))
                oMyUI.sNomeFabbricato = CStr(drDati("nome_fabbricato"))
                oMyUI.sIndirizzo = CStr(drDati("indirizzo"))
                oMyUI.nIdFabbricato = CInt(drDati("COD_FABBRICATO"))
                oMyUI.nIdUnitaImmobiliare = CInt(drDati("COD_UNITA_IMMOBILIARE"))
                If Not IsDBNull(drDati("FOGLIO")) Then
                    oMyUI.nFoglio = CInt(drDati("FOGLIO"))
                End If
                If Not IsDBNull(drDati("NUMERO")) Then
                    oMyUI.sNumero = CStr(drDati("NUMERO"))
                End If
                If Not IsDBNull(drDati("SUB")) Then
                    oMyUI.nSubalterno = CInt(drDati("SUB"))
                End If
                If Not IsDBNull(drDati("INTERNO")) Then
                    oMyUI.sInterno = CStr(drDati("INTERNO"))
                End If
                If Not IsDBNull(drDati("COD_ZONA_CENSUARIA")) Then
                    oMyUI.nCodZonaCensuaria = CInt(drDati("COD_ZONA_CENSUARIA"))
                End If
                If Not IsDBNull(drDati("descrizione")) Then
                    oMyUI.sZonaCensuaria = CInt(drDati("descrizione"))
                End If
                If Not IsDBNull(drDati("SEZIONE")) Then
                    oMyUI.sSezione = CStr(drDati("SEZIONE"))
                End If
                If Not IsDBNull(drDati("DATA_INIZIO")) Then
                    oMyUI.dDal = FncDate.GiraDataFromDB(CStr(drDati("DATA_INIZIO")))
                End If
                If Not IsDBNull(drDati("DATA_FINE")) Then
                    If CStr(drDati("DATA_FINE")) <> "" Then
                        oMyUI.dAl = FncDate.GiraDataFromDB(CStr(drDati("DATA_FINE")))
                    End If
                End If
                If Not IsDBNull(drDati("SCALA")) Then
                    oMyUI.sScala = CStr(drDati("SCALA"))
                End If
                If Not IsDBNull(drDati("INTERNO_CORTILE")) Then
                    oMyUI.sInternoCortile = CStr(drDati("INTERNO_CORTILE"))
                End If
                If Not IsDBNull(drDati("INTERNO_GARAGE")) Then
                    oMyUI.sInternoGarage = CStr(drDati("INTERNO_GARAGE"))
                End If
                If Not IsDBNull(drDati("GRAFFATURA")) Then
                    oMyUI.sGraffatura = CStr(drDati("GRAFFATURA"))
                End If
                If Not IsDBNull(drDati("PROV_CATASTALE")) Then
                    oMyUI.sProvCatastale = CStr(drDati("PROV_CATASTALE"))
                End If
                If Not IsDBNull(drDati("COD_ECOGRAFO")) Then
                    oMyUI.sCodEcografico = CStr(drDati("COD_ECOGRAFO"))
                End If
                If Not IsDBNull(drDati("NOTE")) Then
                    oMyUI.sNote = CStr(drDati("NOTE"))
                End If
                If Not IsDBNull(drDati("MOTIVO_CESSAZIONE")) Then
                    oMyUI.sCodCessazione = CStr(drDati("MOTIVO_CESSAZIONE"))
                End If
                If bLoadDettaglio Then
                    oMyUI.oListClassificazioni = FncClass.GetClassificazioni(myStringConnection, oMyUI.nIdUnitaImmobiliare, -1)
                End If
                'oMyUI. = CInt(drDati("NUMERO_UI"))
                'oMyUI. = CBool(drDati("FLAG_RIFERIMENTO_CATASTALE_CERTO"))
                'omyui.=cint(drdati("COD_PIANO"))
                'omyui.=cint(drdati("NUMERO_UI_ORIGINE"))
                'omyui.=cint(drdati("NUMERO_UI_DESTINAZIONE"))
                'omyui.=cint(drdati("IDORGFAB"))
                'omyui.=cint(drdati("IDORGFABIMMO"))

                nList += 1
                ReDim Preserve oListUI(nList)
                oListUI(nList) = oMyUI
            Loop
            Return oListUI
        Catch Err As Exception
            Log.Error("Si è verificato un errore in GestioneUnitaImmobiliare::GetUI::" & Err.Message)
            Return Nothing
        Finally
            drDati.Close()
        End Try
    End Function

    Public Function CheckUniqueUI(myStringConnection As String, ByVal oMyUI As ObjUnitaImmobiliare) As Boolean
        Dim drDati As SqlClient.SqlDataReader

        Try
            drDati = FncDB.GetSQLUnitaImmobiliare(myStringConnection, oMyUI.sIdEnte, -1, -1, oMyUI.nFoglio, oMyUI.sNumero, oMyUI.nSubalterno).ExecuteReader
            If Not IsNothing(drDati) Then
                If drDati.Read = True Then
                    Return False
                Else
                    Return True
                End If
            Else
                Return True
            End If

        Catch Err As Exception
            Log.Error("Si è verificato un errore in GestioneUI::CheckUniqueUI::" & Err.Message)
            Return False
        Finally
            drDati.Close()
        End Try
    End Function

    Public Function SetUI(myStringConnection As String, ByVal oMyUI As ObjUnitaImmobiliare) As Integer
        Dim myIdentity As Integer = 0
        Dim DrReturn As SqlClient.SqlDataReader

        Try
            'eseguo la query
            If oMyUI.nIdUnitaImmobiliare > 0 Then
                DrReturn = FncDB.GetSQLGestioneUnitaImmobiliare(myStringConnection, FncDB.TYPEOPERATION_UPDATE, oMyUI)
                Do While DrReturn.Read
                    myIdentity = DrReturn(0)
                Loop
                DrReturn.Close()
                If myIdentity > 1 Then
                    Return 0
                End If
            Else
                DrReturn = FncDB.GetSQLGestioneUnitaImmobiliare(myStringConnection, FncDB.TYPEOPERATION_INSERT, oMyUI)
                Do While DrReturn.Read
                    myIdentity = DrReturn(0)
                Loop
                DrReturn.Close()
            End If

            Return myIdentity
        Catch Err As Exception
            Log.Error("Si è verificato un errore in GestioneUI::SetUI::" & Err.Message)
            Return -1
        End Try
    End Function
End Class

Public Class GestioneClassificazione
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(GestioneClassificazione))
    Private FncDate As New ModificaDate
    Private FncDB As New ClsDB

    Public Function GetDescrizioneClassificazione(myStringConnection As String, ByVal nIdClassificazione As Integer, ByRef LblDal As Label, ByRef LblAl As Label, ByRef LblCategoria As Label, ByRef LblClasse As Label, ByRef LblConsistenza As Label, ByRef LblRendita As Label, ByRef LblValore As Label) As Boolean
        Dim drDati As SqlClient.SqlDataReader

        Try
            drDati = FncDB.GetSQLDescrizioneClassificazione(myStringConnection, nIdClassificazione).ExecuteReader
            Do While drDati.Read
                If Not IsDBNull(drDati("DATA_INIZIO")) Then
                    LblDal.Text = FncDate.GiraDataFromDB(CStr(drDati("DATA_INIZIO")))
                Else
                    LblDal.Text = ""
                End If
                If Not IsDBNull(drDati("DATA_FINE")) Then
                    If CStr(drDati("DATA_FINE")) <> "" Then
                        LblAl.Text = FncDate.GiraDataFromDB(CStr(drDati("DATA_FINE")))
                    Else
                        LblAl.Text = ""
                    End If
                Else
                    LblAl.Text = ""
                End If
                If Not IsDBNull(drDati("categoria")) Then
                    LblCategoria.Text = CStr(drDati("categoria"))
                Else
                    LblCategoria.Text = ""
                End If
                If Not IsDBNull(drDati("classe")) Then
                    LblClasse.Text = CStr(drDati("classe"))
                Else
                    LblClasse.Text = ""
                End If
                If Not IsDBNull(drDati("consistenza")) Then
                    LblConsistenza.Text = CStr(drDati("consistenza"))
                Else
                    LblConsistenza.Text = ""
                End If
                If Not IsDBNull(drDati("rendita")) Then
                    LblRendita.Text = CStr(drDati("rendita"))
                Else
                    LblRendita.Text = ""
                End If
                If Not IsDBNull(drDati("valore")) Then
                    LblValore.Text = CStr(drDati("valore"))
                Else
                    LblValore.Text = ""
                End If
            Loop

            Return True
        Catch ex As Exception
            Log.Error("Si è verificato un errore in GestioneUnitaImmobiliare::GetDescrizioneClassificazione::" & ex.Message)
            Return False
        Finally
            drDati.Close()
        End Try
    End Function

    Public Function GetClassificazioni(myStringConnection As String, ByVal nIdUI As Integer, ByVal nIdClass As Integer) As ObjClassificazione()
        Dim dtMyDati As New DataTable
        Dim oMyClass As ObjClassificazione
        Dim oListClass() As ObjClassificazione
        Dim nList As Integer = -1
        Dim FncDB As New ClsDB
        Dim FncVani As New GestioneVano
        Dim FncProp As New GestioneProprieta
        Dim FncCond As New GestioneConduzione
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter

        Try
            cmdMyCommand = FncDB.GetSQLClassificazione(myStringConnection, nIdUI, nIdClass, "", "", -1, -1)
            myAdapter.SelectCommand = cmdMyCommand
            myAdapter.Fill(dtMyDati)
            myAdapter.Dispose()
            For Each myRow As DataRowView In dtMyDati.DefaultView
                oMyClass = New ObjClassificazione

                oMyClass.nIdClassificazione = CInt(myRow("COD_CLASSIFICAZIONE"))
                oMyClass.nIdUI = CInt(myRow("COD_UI"))
                If Not IsDBNull(myRow("DATA_INIZIO")) Then
                    oMyClass.dDal = FncDate.GiraDataFromDB(CStr(myRow("DATA_INIZIO")))
                End If
                If Not IsDBNull(myRow("DATA_FINE")) Then
                    If CStr(myRow("DATA_FINE")) <> "" Then
                        oMyClass.dAl = FncDate.GiraDataFromDB(CStr(myRow("DATA_FINE")))
                    End If
                End If
                If Not IsDBNull(myRow("COD_CATEGORIA_CATASTALE")) Then
                    oMyClass.sCodCategoriaCatastale = CStr(myRow("COD_CATEGORIA_CATASTALE"))
                End If
                If Not IsDBNull(myRow("CLASSE")) Then
                    oMyClass.sCodClasse = CStr(myRow("CLASSE"))
                End If
                If Not IsDBNull(myRow("CONSISTENZA")) Then
                    oMyClass.nConsistenza = CInt(myRow("CONSISTENZA"))
                End If
                If Not IsDBNull(myRow("COD_TIPO_RENDITA")) Then
                    oMyClass.nCodTipoRendita = CInt(myRow("COD_TIPO_RENDITA"))
                End If
                If Not IsDBNull(myRow("descrizione")) Then
                    oMyClass.sTipoRendita = CStr(myRow("descrizione"))
                End If
                If Not IsDBNull(myRow("SUPERFICIE_CATASTALE")) Then
                    oMyClass.nSuperficieCatastale = CDbl(myRow("SUPERFICIE_CATASTALE"))
                End If
                If Not IsDBNull(myRow("SUPERFICIE_NETTA")) Then
                    oMyClass.nSuperficieNetta = CDbl(myRow("SUPERFICIE_NETTA"))
                End If
                If Not IsDBNull(myRow("SUPERFICIE_LORDA")) Then
                    oMyClass.nSuperficieLorda = CDbl(myRow("SUPERFICIE_LORDA"))
                End If
                If Not IsDBNull(myRow("VALORE_RENDITA")) Then
                    oMyClass.sValoreRendita = CStr(myRow("VALORE_RENDITA"))
                End If
                oMyClass.oListProprieta = FncProp.GetProprieta(myStringConnection, oMyClass.nIdClassificazione, -1, -1)
                oMyClass.oListConduzioni = FncCond.GetConduzione(myStringConnection, oMyClass.nIdClassificazione, -1, -1)
                oMyClass.oListVani = FncVani.GetVano(myStringConnection, oMyClass.nIdClassificazione, -1)

                If Not IsDBNull(myRow("DATA_ATTRIBUZIONE_RENDITA")) Then
                    oMyClass.dDataAttribuzione = FncDate.GiraDataFromDB(CStr(myRow("DATA_ATTRIBUZIONE_RENDITA")))
                End If
                If Not IsDBNull(myRow("DATA_EFFICACIA")) Then
                    oMyClass.dDataEfficacia = FncDate.GiraDataFromDB(CStr(myRow("DATA_EFFICACIA")))
                End If
                If Not IsDBNull(myRow("DATA_PRESENTAZIONE_PROT")) Then
                    oMyClass.dDataProtocollo = FncDate.GiraDataFromDB(CStr(myRow("DATA_PRESENTAZIONE_PROT")))
                End If
                If Not IsDBNull(myRow("NUMERO_PROTOCOLLO")) Then
                    oMyClass.sNProtocollo = CStr(myRow("NUMERO_PROTOCOLLO"))
                End If
                If Not IsDBNull(myRow("DATA_EFFETTIVO_UTILIZZO")) Then
                    oMyClass.dDataEffettivoUtilizzo = FncDate.GiraDataFromDB(CStr(myRow("DATA_EFFETTIVO_UTILIZZO")))
                End If
                If Not IsDBNull(myRow("DATA_FINE_LAVORI")) Then
                    oMyClass.dDataFineLavori = FncDate.GiraDataFromDB(CStr(myRow("DATA_FINE_LAVORI")))
                End If
                If Not IsDBNull(myRow("COD_DESTINAZIONE_USO")) Then
                    oMyClass.nCodDestUso = CInt(myRow("COD_DESTINAZIONE_USO"))
                End If
                If Not IsDBNull(myRow("FLAG_INAGIBILITA")) Then
                    oMyClass.bInagibilita = CBool(myRow("FLAG_INAGIBILITA"))
                End If
                If Not IsDBNull(myRow("COD_INAGIBILITA")) Then
                    oMyClass.nCodInagibilita = CInt(myRow("COD_INAGIBILITA"))
                End If
                If Not IsDBNull(myRow("NOTE")) Then
                    oMyClass.sNote = CStr(myRow("NOTE"))
                End If
                'omyclass.=cbool(drdati("FLAG_PERTINENZA"))
                'omyclass.=cint(drdati("COD_UI_PERTINENZA"))
                'omyclass.=cstr(drdati("DESCRIZIONE_PERTINENZA"))
                'omyclass.=cint(drdati("DIFFORMITACAT"))

                nList += 1
                ReDim Preserve oListClass(nList)
                oListClass(nList) = oMyClass
            Next
            Return oListClass
        Catch Err As Exception
            Log.Error("Si è verificato un errore in GestioneClassificazione::GetClass::" & Err.Message)
            Return Nothing
        Finally
            dtMyDati.Dispose()
        End Try
    End Function

    Public Function CheckUniqueClas(myStringConnection As String, ByVal oMyClas As ObjClassificazione) As Boolean
        Dim drDati As SqlClient.SqlDataReader

        Try
            drDati = FncDB.GetSQLClassificazione(myStringConnection, oMyClas.nIdUI, -1, oMyClas.sCodCategoriaCatastale, oMyClas.sCodClasse, oMyClas.nConsistenza, oMyClas.nCodTipoRendita).ExecuteReader
            If drDati.Read = True Then
                Return False
            Else
                Return True
            End If

        Catch Err As Exception
            Log.Error("Si è verificato un errore in GestioneClas::CheckUniqueClas::" & Err.Message)
            Return False
        Finally
            drDati.Close()
        End Try
    End Function

    Public Function SetClassificazione(myStringConnection As String, ByVal oMyClas As ObjClassificazione) As Integer
        Dim myIdentity As Integer = 0
        Dim DrReturn As SqlClient.SqlDataReader

        Try

            'eseguo la query
            If oMyClas.nIdClassificazione > 0 Then
                DrReturn = FncDB.GetSQLGestioneClassificazione(myStringConnection, FncDB.TYPEOPERATION_UPDATE, oMyClas)
                Do While DrReturn.Read
                    myIdentity = DrReturn(0)
                Loop
                DrReturn.Close()
                If myIdentity > 1 Then
                    Return 0
                End If
            Else
                DrReturn = FncDB.GetSQLGestioneClassificazione(myStringConnection, FncDB.TYPEOPERATION_INSERT, oMyClas)
                Do While DrReturn.Read
                    myIdentity = DrReturn(0)
                Loop
                DrReturn.Close()
            End If
            Return myIdentity
        Catch Err As Exception
            Log.Error("Si è verificato un errore in GestioneClas::SetClassificazione::" & Err.Message)
            Return -1
        End Try
    End Function

    Public Function GetValoreRendita(myStringConnection As String, ByVal sIdEnte As String, ByVal sCatCatastale As String, ByVal sClasse As String, ByVal sConsistenza As String) As Double
        Dim drDati As SqlClient.SqlDataReader
        Dim MyReturn As Double = 0

        If IsNumeric(sConsistenza) Then
            Try
                drDati = FncDB.GetSQLValoreRendita(myStringConnection, sIdEnte, sCatCatastale, sClasse).ExecuteReader
                Do While drDati.Read
                    If Not IsDBNull(drDati("tariffa_euro")) Then
                        MyReturn = (drDati("tariffa_euro") * sConsistenza)
                    End If
                Loop
            Catch Err As Exception
                Log.Error("Si è verificato un errore in GestioneClas::GetValoreRendita::" & Err.Message)
                Return -1
            Finally
                drDati.Close()
            End Try
        End If
        Return MyReturn
    End Function
End Class
''' <summary>
''' Classe per la gestione del vano
''' </summary>
Public Class GestioneVano
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(GestioneVano))
    Private FncDate As New ModificaDate
    Private FncDB As New ClsDB

    Public Function GetVano(myStringConnection As String, ByVal nIdClass As Integer, ByVal nIdVano As Integer) As ObjVano()
        Dim drDati As SqlClient.SqlDataReader
        Dim oMyVano As ObjVano
        Dim oListVani() As ObjVano
        Dim nList As Integer = -1

        Try
            drDati = FncDB.GetSQLVano(myStringConnection, nIdClass, nIdVano).ExecuteReader
            Do While drDati.Read
                oMyVano = New ObjVano

                oMyVano.nIdVano = CInt(drDati("COD_VANO"))
                oMyVano.nIdClassificazione = CInt(drDati("COD_CLASSIFICAZIONE"))
                If Not IsDBNull(drDati("COD_PIANO")) Then
                    oMyVano.nPiano = CInt(drDati("COD_PIANO"))
                End If
                If Not IsDBNull(drDati("piano")) Then
                    oMyVano.sTipoPiano = CStr(drDati("piano"))
                End If
                If Not IsDBNull(drDati("COD_TIPO_VANO")) Then
                    oMyVano.nTipoVano = CInt(drDati("COD_TIPO_VANO"))
                End If
                If Not IsDBNull(drDati("descrizione")) Then
                    oMyVano.sTipoVano = CStr(drDati("descrizione"))
                End If
                If Not IsDBNull(drDati("MQ")) Then
                    oMyVano.nMQ = CDbl(drDati("MQ"))
                End If
                If Not IsDBNull(drDati("PERCENTUALE_UTILIZZO")) Then
                    oMyVano.nPercentUso = CInt(drDati("PERCENTUALE_UTILIZZO"))
                End If
                If Not IsDBNull(drDati("PESO_CATASTALE")) Then
                    oMyVano.nPesoCat = CDbl(drDati("PESO_CATASTALE"))
                End If
                If Not IsDBNull(drDati("note")) Then
                    oMyVano.sNote = CStr(drDati("NOTE"))
                End If
                'omyvano.=cstr(drdati("NOMERASTER"))

                nList += 1
                ReDim Preserve oListVani(nList)
                oListVani(nList) = oMyVano
            Loop
            Return oListVani
        Catch Err As Exception
            Log.Error("Si è verificato un errore in GestioneVano::GetVano::" & Err.Message)
            Return Nothing
        Finally
            drDati.Close()
        End Try
    End Function
End Class
''' <summary>
''' Classe per la gestione della proprietà
''' </summary>
Public Class GestioneProprieta
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(GestioneProprieta))
    Private FncDate As New ModificaDate
    Private FncDB As New ClsDB

    Public Function GetProprieta(myStringConnection As String, ByVal nIdClass As Integer, ByVal nIdProprieta As Integer, ByVal nIdProprietario As Integer) As ObjProprieta()
        Dim drDati As SqlClient.SqlDataReader
        Dim oMyProprieta As ObjProprieta
        Dim oListProprieta() As ObjProprieta
        Dim nList As Integer = -1
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            drDati = FncDB.GetSQLProprieta(myStringConnection, nIdClass, nIdProprieta, nIdProprietario, Date.MinValue, Date.MinValue, -1)
            Do While drDati.Read
                oMyProprieta = New ObjProprieta

                oMyProprieta.nIdProprieta = CInt(drDati("COD_PROPRIETA"))
                oMyProprieta.nIdProprietario = CInt(drDati("COD_PROPRIETArio"))
                oMyProprieta.nIdClassificazione = CInt(drDati("COD_CLASSIFICAZIONE"))
                If Not IsDBNull(drDati("DATA_INIZIO")) Then
                    oMyProprieta.dDataInizio = FncDate.GiraDataFromDB(CStr(drDati("DATA_INIZIO")))
                End If
                If Not IsDBNull(drDati("DATA_FINE")) Then
                    oMyProprieta.dDataFine = FncDate.GiraDataFromDB(CStr(drDati("DATA_FINE")))
                End If
                If Not IsDBNull(drDati("cod_anagrafica")) Then
                    oMyProprieta.nIdAnagrafica = CInt(drDati("cod_anagrafica"))
                End If
                If Not IsDBNull(drDati("cognome_denominazione")) Then
                    oMyProprieta.sCognome = CStr(drDati("cognome_denominazione"))
                End If
                If Not IsDBNull(drDati("note")) Then
                    oMyProprieta.sNote = CStr(drDati("note"))
                End If
                If Not IsDBNull(drDati("percentuale_proprieta")) Then
                    oMyProprieta.nPercentProprieta = CDbl(drDati("percentuale_proprieta"))
                End If
                If Not IsDBNull(drDati("cod_tipo_proprieta")) Then
                    oMyProprieta.nTipoProprieta = CInt(drDati("cod_tipo_proprieta"))
                End If
                If Not IsDBNull(drDati("descrizione_proprieta")) Then
                    oMyProprieta.sProprieta = CStr(drDati("descrizione_proprieta"))
                End If
                If Not IsDBNull(drDati("percentuale_possesso")) Then
                    oMyProprieta.nPercentPossesso = CDbl(drDati("percentuale_possesso"))
                End If
                If Not IsDBNull(drDati("cod_tipo_possesso")) Then
                    oMyProprieta.nTipoPossesso = CInt(drDati("cod_tipo_possesso"))
                End If
                If Not IsDBNull(drDati("descrizione_possesso")) Then
                    oMyProprieta.sPossesso = CStr(drDati("descrizione_possesso"))
                End If
                If Not IsDBNull(drDati("cod_utilizzo")) Then
                    oMyProprieta.nTipoUtilizzo = CInt(drDati("cod_utilizzo"))
                End If
                If Not IsDBNull(drDati("descrizione_utilizzo")) Then
                    oMyProprieta.sUtilizzo = CStr(drDati("descrizione_utilizzo"))
                End If
                If Not IsDBNull(drDati("cod_tipo_parentela")) Then
                    oMyProprieta.nTipoParentela = CInt(drDati("cod_tipo_parentela"))
                End If
                If Not IsDBNull(drDati("descrizione_parentela")) Then
                    oMyProprieta.sParentela = CStr(drDati("descrizione_parentela"))
                End If
                If Not IsDBNull(drDati("nome")) Then
                    oMyProprieta.sNome = CStr(drDati("nome"))
                End If

                nList += 1
                ReDim Preserve oListProprieta(nList)
                oListProprieta(nList) = oMyProprieta
            Loop
            Return oListProprieta
        Catch Err As Exception
            Log.Error("Si è verificato un errore in GestioneProprieta::GetProprieta::" & Err.Message)
            Return Nothing
        Finally
            drDati.Close()
        End Try
    End Function
End Class
''' <summary>
''' Classe per la gestione della conduzione
''' </summary>
Public Class GestioneConduzione
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(GestioneConduzione))
    Private FncDate As New ModificaDate
    Private FncDB As New ClsDB

    Public Function GetConduzione(myStringConnection As String, ByVal nIdClas As Integer, ByVal nIdConduzione As Integer, ByVal nIdConduttore As Integer) As ObjConduzione()
        Dim drDati As SqlClient.SqlDataReader
        Dim oMyConduzione As ObjConduzione
        Dim oListConduzione() As ObjConduzione
        Dim nList As Integer = -1

        Try
            drDati = FncDB.GetSQLConduzione(myStringConnection, nIdClas, nIdConduzione, nIdConduttore, Date.MinValue, Date.MinValue, -1).ExecuteReader
            Do While drDati.Read
                oMyConduzione = New ObjConduzione

                oMyConduzione.nIdClassificazione = CInt(drDati("cod_classificazione"))
                oMyConduzione.nIdConduzione = CInt(drDati("COD_Conduzione"))
                If Not IsDBNull(drDati("DATA_INIZIO")) Then
                    oMyConduzione.dDataInizio = FncDate.GiraDataFromDB(CStr(drDati("DATA_INIZIO")))
                End If
                If Not IsDBNull(drDati("DATA_FINE")) Then
                    oMyConduzione.dDataFine = FncDate.GiraDataFromDB(CStr(drDati("DATA_FINE")))
                End If
                If Not IsDBNull(drDati("cod_stato_occupazione")) Then
                    oMyConduzione.nCodStatoOccupazione = CInt(drDati("COD_STATO_OCCUPAZIONE"))
                End If
                If Not IsDBNull(drDati("cod_tipo_occupazione")) Then
                    oMyConduzione.nCodTipoOccupazione = CInt(drDati("COD_TIPO_OCCUPAZIONE"))
                End If
                If Not IsDBNull(drDati("cognome_denominazione")) Then
                    oMyConduzione.sCognome = CStr(drDati("cognome_denominazione"))
                End If
                If Not IsDBNull(drDati("nome")) Then
                    oMyConduzione.sNome = CStr(drDati("nome"))
                End If
                If Not IsDBNull(drDati("percentuale_utilizzo")) Then
                    oMyConduzione.nPercentUtilizzo = CDbl(drDati("percentuale_utilizzo"))
                End If
                If Not IsDBNull(drDati("stato_occupazione")) Then
                    oMyConduzione.sStatoOccupazione = CStr(drDati("stato_occupazione"))
                End If
                If Not IsDBNull(drDati("cod_conduttore")) Then
                    oMyConduzione.nIdConduttore = CInt(drDati("cod_conduttore"))
                End If
                If Not IsDBNull(drDati("cod_anagrafica")) Then
                    oMyConduzione.nIdAnagrafica = CInt(drDati("cod_anagrafica"))
                End If
                If Not IsDBNull(drDati("NUM_PERSONE_OCCUPANTI")) Then
                    oMyConduzione.nOccupanti = CInt(drDati("NUM_PERSONE_OCCUPANTI"))
                End If
                nList += 1
                ReDim Preserve oListConduzione(nList)
                oListConduzione(nList) = oMyConduzione
            Loop
            Return oListConduzione
        Catch Err As Exception
            Log.Error("Si è verificato un errore in GestioneConduzione::GetConduzione::" & Err.Message)
            Return Nothing
        Finally
            drDati.Close()
        End Try
    End Function
End Class