Imports System
Imports System.Xml
Imports System.Data.SqlClient
Imports log4net

Public Class DBManager

    Private Const INDIRIZZO As Integer = 1
    Private Const TELEFONO As Integer = 2
    Private Const CELLULARE As Integer = 3
    Protected blnUPDATE As Boolean
    Protected blnTerminato As Boolean
    Private clsFile As New GestioneFile()
    Public strPath As String


    Private Shared Log As ILog = LogManager.GetLogger(GetType(DBManager))

    Public Sub SaveData(ByVal strIDDOC As String, ByVal strXMLFileName As String, _
       Optional ByVal blnTerminato As Boolean = False)
        'Determina la funzione da utilizzare per salvare i dati in base all'attivita'
        setDBOpenUtenzeAllData(strIDDOC, strXMLFileName, blnTerminato)
    End Sub
    Public Sub Modify_Stored_Data(ByVal strIDDOC As String, ByVal strIDDOC_OLD As String, ByVal strXMLFileName As String, _
              Optional ByVal blnTerminato As Boolean = False)
        'Se si modifica il Contratto aggiorno e inserisco nelle tabelle

        setModifyStoredData(strIDDOC, _
                  strIDDOC_OLD, _
                  strXMLFileName, _
                  blnTerminato)
    End Sub


    Public Sub SelectData(ByVal strIDDOC As String, _
      ByVal strXMLFileName As String)
        'Determina la funzione da utilizzare per Estrarre i dati Dal DataBase
        getANAINTEATATARIO(strIDDOC, strXMLFileName)
        getANACONTATORE(strIDDOC, strXMLFileName)
        getTP_CONTATORI(strIDDOC, strXMLFileName)
        getTP_LETTURE(strIDDOC, strXMLFileName)
        getTP_PREVENTIVO(strIDDOC, strXMLFileName)
        getTP_CONTRATTI(strIDDOC, strXMLFileName)
    End Sub


#Region "setDBOpenUtenzeAllData"
    Private Sub setDBOpenUtenzeAllData(ByVal strIDDOC As String, ByVal strXMLFileName As String, _
     ByVal blnTerminato As Boolean)
        'Utilizzo di StoreProcedure
        Dim sqlConn As New SqlConnection
        Dim sqlParm As New SqlParameter
        Dim lngRowCount As Int32
        Dim sqlTrans As SqlTransaction
        Dim blnOkDB As Boolean
        sqlConn.ConnectionString = ConstSession.StringConnection
        sqlConn.Open()

        Dim sqlCmd As SqlCommand = New SqlCommand("sp_SelectTP_CONTATORI", sqlConn)
        sqlCmd.CommandType = CommandType.StoredProcedure
        sqlParm = sqlCmd.Parameters.Add("@RowCount", SqlDbType.Int)
        sqlParm.Direction = ParameterDirection.ReturnValue
        sqlCmd.Parameters.Add("@IDCONTATORE", SqlDbType.Int, 4, "IDCONTATORE")
        sqlCmd.Parameters("@IDCONTATORE").Value = strIDDOC
        sqlCmd.ExecuteNonQuery()

        lngRowCount = sqlCmd.Parameters("@RowCount").Value
        Try
            If lngRowCount = 0 Then
                'INSERT 
                blnUPDATE = False
            Else
                'UPDATE 
                blnUPDATE = True
            End If
            'Inizio la transazione
            sqlTrans = sqlConn.BeginTransaction

            If Not blnUPDATE Then
                'Inserimento dei dati nel DataBase OpenUtenze
                blnOkDB = setTP_CONTATORI(sqlConn, sqlTrans, strXMLFileName, strIDDOC, "sp_InsertTP_CONTATORI")
                If blnOkDB Then blnOkDB = setTP_CONTRATTI(sqlConn, sqlTrans, strXMLFileName, strIDDOC, "sp_InsertTP_CONTRATTI", blnTerminato)
                If blnOkDB Then blnOkDB = setANAGRAFE_INTESTATARIO(sqlConn, sqlTrans, strXMLFileName, strIDDOC, "sp_InsertANAGRAFE_INTESTATARIO")
                If blnOkDB Then blnOkDB = setANAGRAFE_CONTATORE(sqlConn, sqlTrans, strXMLFileName, strIDDOC, "sp_InsertANAGRAFE_CONTATORE")
                If blnOkDB Then blnOkDB = setTP_LETTURE(sqlConn, sqlTrans, strXMLFileName, strIDDOC, "sp_InsertTP_LETTURE")
                If blnOkDB Then blnOkDB = setTP_PREVENTIVO(sqlConn, sqlTrans, strXMLFileName, strIDDOC, "sp_InsertTP_PREVENTIVO")

            Else
                blnOkDB = setTP_CONTRATTI(sqlConn, sqlTrans, strXMLFileName, strIDDOC, "sp_UpdateTP_CONTRATTI", blnTerminato)
                If blnOkDB Then blnOkDB = setTP_CONTATORI(sqlConn, sqlTrans, strXMLFileName, strIDDOC, "sp_UpdateTP_CONTATORI")
                If blnOkDB Then blnOkDB = setANAGRAFE_INTESTATARIO(sqlConn, sqlTrans, strXMLFileName, strIDDOC, "sp_UpdateANAGRAFE_INTESTATARIO")
                If blnOkDB Then blnOkDB = setANAGRAFE_CONTATORE(sqlConn, sqlTrans, strXMLFileName, strIDDOC, "sp_UpdateANAGRAFE_CONTATORE")
                If blnOkDB Then blnOkDB = setTP_LETTURE(sqlConn, sqlTrans, strXMLFileName, strIDDOC, "sp_UpdateTP_LETTURE")
                If blnOkDB Then blnOkDB = setTP_PREVENTIVO(sqlConn, sqlTrans, strXMLFileName, strIDDOC, "sp_UpdateTP_PREVENTIVO")
            End If
            If blnOkDB Then
                sqlTrans.Commit()
            Else
                sqlTrans.Rollback()
                'Sollevo un messaggio d'errore
                Throw New System.Exception("Si è verificato un errore durante il  Salvataggio dei dati nel DataBase:OpenUtenze-Verificare il file ErrorDb_logError.log !")
            End If
            sqlConn.Close()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBManager.setDBOpenUtenzaAllData.errore: ", ex)
        End Try
    End Sub
#End Region
#Region "setTP_CONTATORI"

    Protected Function setTP_CONTATORI(ByVal sqlConn As SqlConnection, _
     ByVal sqlTrans As SqlTransaction, _
     ByVal strXMLFileName As String, _
     ByVal strIDDOC As String, _
     ByVal strStoreName As String) As Boolean
        Try
            Dim xmlLoadDocument As New XmlDocument

            Dim sqlCmdInsert As SqlCommand = New SqlCommand(strStoreName, sqlConn, sqlTrans)
            setTP_CONTATORI = False

            sqlCmdInsert.CommandType = CommandType.StoredProcedure
            xmlLoadDocument.Load(strXMLFileName)

            sqlCmdInsert.Parameters.Add("@IDCONTATORE", SqlDbType.Int, 4, "IDCONTATORE")
            sqlCmdInsert.Parameters("@IDCONTATORE").Value = strIDDOC
            sqlCmdInsert.Parameters.Add("@IDTIPOUTENZA", SqlDbType.Int, 4, "IDTIPOUTENZA")
            If xmlLoadDocument.SelectSingleNode("//DATI_UTENZA/TIPOLOGIA_UTENZA").InnerText = "DOMESTICA" Then
                sqlCmdInsert.Parameters("@IDTIPOUTENZA").Value = 1
            Else
                sqlCmdInsert.Parameters("@IDTIPOUTENZA").Value = 2
            End If

            sqlCmdInsert.Parameters.Add("@NUMERO_UTENZE", SqlDbType.Int, 4, "NUMERO_UTENZE")
            sqlCmdInsert.Parameters("@NUMERO_UTENZE").Value = MyUtility.objToStr(xmlLoadDocument.SelectSingleNode("//DATI_UTENZA/N_UTENZE").InnerText(), MyUtility.TypeFieldXML.TypeInteger)
            sqlCmdInsert.Parameters.Add("@PUNTIUTILIZZO", SqlDbType.Int, 4, "PUNTIUTILIZZO")
            sqlCmdInsert.Parameters("@PUNTIUTILIZZO").Value = MyUtility.objToStr(xmlLoadDocument.SelectSingleNode("//DATI_UTENZA/PUNTI_UTILIZZO").InnerText(), MyUtility.TypeFieldXML.TypeInteger)
            sqlCmdInsert.Parameters.Add("@DATA_ATTIVAZIONE", SqlDbType.DateTime, 8, "DATA_ATTIVAZIONE")
            sqlCmdInsert.Parameters("@DATA_ATTIVAZIONE").Value = System.DBNull.Value
            sqlCmdInsert.Parameters.Add("@DATA_PAGAMENTO", SqlDbType.DateTime, 8, "DATA_PAGAMENTO")
            sqlCmdInsert.Parameters("@DATA_PAGAMENTO").Value = MyUtility.objToStr(xmlLoadDocument.SelectSingleNode("//REGISTRAZIONE_PAGAMENTI/DATA_PAGAMENTO_PREISTRUTTORIA").InnerText(), MyUtility.TypeFieldXML.TypeDate)
            sqlCmdInsert.Parameters.Add("@NUMERO_FATTURA ", SqlDbType.Int, 4, "NUMERO_FATTURA ")
            sqlCmdInsert.Parameters("@NUMERO_FATTURA ").Value = MyUtility.objToStr(xmlLoadDocument.SelectSingleNode("//REGISTRAZIONE_PAGAMENTI/NUMEROFATTURA_PREISTRUTTORIA").InnerText(), MyUtility.TypeFieldXML.TypeInteger)
            sqlCmdInsert.Parameters.Add("@DATA_REGISTRAZIONE ", SqlDbType.DateTime, 8, "DATA_REGISTRAZIONE ")
            sqlCmdInsert.Parameters("@DATA_REGISTRAZIONE ").Value = MyUtility.objToStr(xmlLoadDocument.SelectSingleNode("//FIRMA_CONTRATTO/DATA_SOTTOSCRIZIONE").InnerText(), MyUtility.TypeFieldXML.TypeDate)
            sqlCmdInsert.Parameters.Add("@NOTE ", SqlDbType.VarChar, 500, "NOTE ")
            sqlCmdInsert.Parameters("@NOTE ").Value = MyUtility.objToStr(xmlLoadDocument.SelectSingleNode("//FIRMA_CONTRATTO/NOTE").InnerText(), MyUtility.TypeFieldXML.TypeString)



            sqlCmdInsert.ExecuteNonQuery()

            setTP_CONTATORI = True

        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBManager.setTP_CONTATORI.errore: ", ex)

            setTP_CONTATORI = False
        End Try

    End Function
#End Region
#Region "setANAGRAFE_INTESTATARIO"
    Protected Function setANAGRAFE_INTESTATARIO(ByVal sqlConn As SqlConnection, ByVal sqlTrans As SqlTransaction, _
      ByVal strXMLFileName As String, _
      ByVal strIDDOC As String, _
      ByVal strStoreName As String) As Boolean
        Try

            Dim xmlLoadDocument As New XmlDocument
            Dim sqlParm As New SqlParameter
            Dim sqlCmdInsert As SqlCommand = New SqlCommand(strStoreName, sqlConn, sqlTrans)

            setANAGRAFE_INTESTATARIO = False

            sqlCmdInsert.CommandType = CommandType.StoredProcedure
            xmlLoadDocument.Load(strXMLFileName)

            Dim parameterItemID As New SqlParameter("@ItemID", SqlDbType.Int, 4)
            parameterItemID.Direction = ParameterDirection.Output

            If Not blnUPDATE Then
                sqlCmdInsert.Parameters.Add(parameterItemID)
            End If


            sqlCmdInsert.Parameters.Add("@COGNOME", SqlDbType.NVarChar, 100, "COGNOME")
            sqlCmdInsert.Parameters("@COGNOME").Value = xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/COGNOME").InnerText

            sqlCmdInsert.Parameters.Add("@NOME", SqlDbType.NVarChar, 50, "NOME")
            sqlCmdInsert.Parameters("@NOME").Value = xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/NOME").InnerText

            sqlCmdInsert.Parameters.Add("@COD_FISCALE", SqlDbType.NVarChar, 16, "COD_FISCALE")
            sqlCmdInsert.Parameters("@COD_FISCALE").Value = xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/CODICE_FISCALE").InnerText

            sqlCmdInsert.Parameters.Add("@PARTITA_IVA", SqlDbType.NVarChar, 11, "PARTITA_IVA")
            sqlCmdInsert.Parameters("@PARTITA_IVA").Value = System.DBNull.Value

            sqlCmdInsert.Parameters.Add("@SESSO", SqlDbType.NVarChar, 1, "SESSO")
            sqlCmdInsert.Parameters("@SESSO").Value = xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/SESSO").InnerText

            sqlCmdInsert.Parameters.Add("@DATA_NASCITA", SqlDbType.DateTime, 8, "DATA_NASCITA")
            sqlCmdInsert.Parameters("@DATA_NASCITA").Value = xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/DATA_NASCITA").InnerText

            sqlCmdInsert.Parameters.Add("@CAP_NASCITA", SqlDbType.NVarChar, 5, "CAP_NASCITA")
            sqlCmdInsert.Parameters("@CAP_NASCITA").Value = System.DBNull.Value

            sqlCmdInsert.Parameters.Add("@COMUNE_NASCITA", SqlDbType.NVarChar, 50, "COMUNE_NASCITA")
            sqlCmdInsert.Parameters("@COMUNE_NASCITA").Value = xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/COMUNE_NASCITA").InnerText

            sqlCmdInsert.Parameters.Add("@COD_COMUNE_NASCITA", SqlDbType.NVarChar, 50, "COD_COMUNE_NASCITA")
            sqlCmdInsert.Parameters("@COD_COMUNE_NASCITA").Value = xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/COD_COMUNE_NASCITA").InnerText

            sqlCmdInsert.Parameters.Add("@PROV_NASCITA", SqlDbType.NVarChar, 2, "PROV_NASCITA")
            sqlCmdInsert.Parameters("@PROV_NASCITA").Value = xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/PROVINCIA_NASCITA").InnerText

            sqlCmdInsert.Parameters.Add("@VIA_RES", SqlDbType.NVarChar, 50, "VIA_RES")
            sqlCmdInsert.Parameters("@VIA_RES").Value = xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/RESIDENZA/VIA_RES").InnerText

            sqlCmdInsert.Parameters.Add("@FRAZIONE_RES", SqlDbType.NVarChar, 50, "FRAZIONE_RES")
            sqlCmdInsert.Parameters("@FRAZIONE_RES").Value = xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/RESIDENZA/FRAZIONE_RES").InnerText

            sqlCmdInsert.Parameters.Add("@CIVICO_RES", SqlDbType.NVarChar, 10, "CIVICO_RES")
            sqlCmdInsert.Parameters("@CIVICO_RES").Value = xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/RESIDENZA/NUM_CIVICO_RES").InnerText

            sqlCmdInsert.Parameters.Add("@CAP_RES", SqlDbType.NVarChar, 5, "CAP_RES")
            sqlCmdInsert.Parameters("@CAP_RES").Value = xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/RESIDENZA/CAP_RES").InnerText

            sqlCmdInsert.Parameters.Add("@CITTA_RES", SqlDbType.NVarChar, 50, "CITTA_RES")
            sqlCmdInsert.Parameters("@CITTA_RES").Value = xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/RESIDENZA/COMUNE_RES").InnerText

            sqlCmdInsert.Parameters.Add("@COD_COMUNE_RES", SqlDbType.NVarChar, 50, "COD_COMUNE_RES")
            sqlCmdInsert.Parameters("@COD_COMUNE_RES").Value = xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/RESIDENZA/COD_COMUNE_RES").InnerText

            sqlCmdInsert.Parameters.Add("@PROVINCIA_RES", SqlDbType.NVarChar, 2, "PROVINCIA_RES")
            sqlCmdInsert.Parameters("@PROVINCIA_RES").Value = xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/RESIDENZA/PROVINCIA_RES").InnerText

            sqlCmdInsert.Parameters.Add("@TELEFONO", SqlDbType.NVarChar, 20, "TELEFONO")
            sqlCmdInsert.Parameters("@TELEFONO").Value = xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/TELEFONO").InnerText

            sqlCmdInsert.Parameters.Add("@CELLULARE", SqlDbType.NVarChar, 20, "CELLULARE")
            sqlCmdInsert.Parameters("@CELLULARE").Value = MyUtility.objToStr(xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/CELLULARE").InnerText, MyUtility.TypeFieldXML.TypeString)


            sqlCmdInsert.Parameters.Add("@IDTITOLOSOGGETTO", SqlDbType.Int, 4, "IDTITOLOSOGGETTO")
            sqlCmdInsert.Parameters("@IDTITOLOSOGGETTO").Value = xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/PROFESSIONE").InnerText

            If blnUPDATE Then
                sqlCmdInsert.Parameters.Add("@COD_CONTRIBUENTE", SqlDbType.Int, 4, "COD_CONTRIBUENTE")
                sqlCmdInsert.Parameters("@COD_CONTRIBUENTE").Value = xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/COD_CONTRIBUENTE").InnerText
            End If

            sqlCmdInsert.ExecuteScalar()

            Dim strID As String
            If Not blnUPDATE Then
                strID = CStr(parameterItemID.Value)
                xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/COD_CONTRIBUENTE").InnerText = strID
                xmlLoadDocument.Save(strXMLFileName)
                'AGGIORNAMENTO TABELLA DI RELAZIONE
                Dim sqlCmdInsertTR As SqlCommand = New SqlCommand("sp_TRInsertCommand", sqlConn, sqlTrans)

                sqlCmdInsertTR.CommandType = CommandType.StoredProcedure

                sqlCmdInsertTR.Parameters.Add("@IDCONTATORE", SqlDbType.Int, 4, "IDCONTATORE")
                sqlCmdInsertTR.Parameters("@IDCONTATORE").Value = strIDDOC

                sqlCmdInsertTR.Parameters.Add("@COD_CONTRIBUENTE", SqlDbType.Int, 4, "COD_CONTRIBUENTE")
                sqlCmdInsertTR.Parameters("@COD_CONTRIBUENTE").Value = strID

                sqlCmdInsertTR.ExecuteNonQuery()

            End If

            setANAGRAFE_INTESTATARIO = True

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBManager.setANAGRAFE_INTESTATARIO.errore: ", ex)
            setANAGRAFE_INTESTATARIO = False
            'scrittura nel file di log
        End Try

    End Function
#End Region
#Region "setTP_CONTRATTI"
    Protected Function setTP_CONTRATTI(ByVal sqlConn As SqlConnection, _
     ByVal sqlTrans As SqlTransaction, _
     ByVal strXMLFileName As String, _
     ByVal strIDDOC As String, _
     ByVal strStoreName As String, _
     ByVal blnTerminato As Boolean _
   ) As Boolean
        Try
            Dim xmlLoadDocument As New XmlDocument
            Dim sqlCmdInsert As SqlCommand = New SqlCommand(strStoreName, sqlConn, sqlTrans)

            setTP_CONTRATTI = False

            sqlCmdInsert.CommandType = CommandType.StoredProcedure
            xmlLoadDocument.Load(strXMLFileName)

            Dim parameterItemID As New SqlParameter("@ItemID", SqlDbType.Int, 4)
            parameterItemID.Direction = ParameterDirection.Output

            If Not blnUPDATE Then
                sqlCmdInsert.Parameters.Add(parameterItemID)
            End If

            sqlCmdInsert.Parameters.Add("@DATA_ATTIVAZIONE", SqlDbType.DateTime, 8, "DATA_ATTIVAZIONE")
            sqlCmdInsert.Parameters("@DATA_ATTIVAZIONE").Value = CType(Now, Date)

            sqlCmdInsert.Parameters.Add("@DATA_CHIUSURA", SqlDbType.DateTime, 8, "DATA_CHIUSURA")
            sqlCmdInsert.Parameters("@DATA_CHIUSURA").Value = System.DBNull.Value

            sqlCmdInsert.Parameters.Add("@IDCONTATORE", SqlDbType.Int, 4, "IDCONTATORI")
            sqlCmdInsert.Parameters("@IDCONTATORE").Value = strIDDOC
            sqlCmdInsert.Parameters.Add("@TERMINATO", SqlDbType.Bit, 1, "TERMINATO")
            If blnTerminato Then
                sqlCmdInsert.Parameters("@TERMINATO").Value = 1
            Else
                sqlCmdInsert.Parameters("@TERMINATO").Value = System.DBNull.Value
            End If

            If blnUPDATE Then
                sqlCmdInsert.Parameters.Add("@COD_CONTRATTO", SqlDbType.Int, 4, "COD_CONTRATTO")
                sqlCmdInsert.Parameters("@COD_CONTRATTO").Value = xmlLoadDocument.SelectSingleNode("//EMISSIONE_CONTRATTO/COD_CONTRATTO").InnerText
            End If

            sqlCmdInsert.ExecuteNonQuery()
            Dim strID As String

            If Not blnUPDATE Then
                strID = CStr(parameterItemID.Value)
                xmlLoadDocument.SelectSingleNode("//EMISSIONE_CONTRATTO/COD_CONTRATTO").InnerText = strID
                xmlLoadDocument.Save(strXMLFileName)
            End If

            setTP_CONTRATTI = True

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBManager.setTP_CONTRATTI.errore: ", ex)
            setTP_CONTRATTI = False
            'scrittura nel file di log
        End Try

    End Function

#End Region
#Region "setANAGRAFE_CONTATORE"
    Protected Function setANAGRAFE_CONTATORE(ByVal sqlConn As SqlConnection, _
      ByVal sqlTrans As SqlTransaction, _
      ByVal strXMLFileName As String, _
      ByVal strIDDOC As String, _
      ByVal strStoreName As String) As Boolean
        Try

            Dim xmlLoadDocument As New XmlDocument
            Dim sqlParm As New SqlParameter
            Dim sqlCmdInsert As SqlCommand = New SqlCommand(strStoreName, sqlConn, sqlTrans)

            setANAGRAFE_CONTATORE = False

            sqlCmdInsert.CommandType = CommandType.StoredProcedure
            xmlLoadDocument.Load(strXMLFileName)

            Dim parameterItemID As New SqlParameter("@ItemID", SqlDbType.Int, 4)
            parameterItemID.Direction = ParameterDirection.Output

            If Not blnUPDATE Then
                sqlCmdInsert.Parameters.Add(parameterItemID)
            End If

            sqlCmdInsert.Parameters.Add("@COGNOME_PRESSO", SqlDbType.NVarChar, 100, "COGNOME_PRESSO")
            sqlCmdInsert.Parameters("@COGNOME_PRESSO").Value = xmlLoadDocument.SelectSingleNode("//UBICAZIONE/COGNOME_SOPRALLUOGO").InnerText

            sqlCmdInsert.Parameters.Add("@NOME_PRESSO", SqlDbType.NVarChar, 100, "NOME_PRESSO")
            sqlCmdInsert.Parameters("@NOME_PRESSO").Value = xmlLoadDocument.SelectSingleNode("//UBICAZIONE/NOME_SOPRALLUOGO").InnerText

            sqlCmdInsert.Parameters.Add("@COMUNE_PRESSO", SqlDbType.NVarChar, 50, "COMUNE_PRESSO")
            sqlCmdInsert.Parameters("@COMUNE_PRESSO").Value = xmlLoadDocument.SelectSingleNode("//UBICAZIONE/COMUNE_SOPRALLUOGO").InnerText

            sqlCmdInsert.Parameters.Add("@COD_COMUNE_PRESSO", SqlDbType.NVarChar, 50, "COD_COMUNE_PRESSO")
            sqlCmdInsert.Parameters("@COD_COMUNE_PRESSO").Value = xmlLoadDocument.SelectSingleNode("//UBICAZIONE/COD_COMUNESOPR").InnerText

            sqlCmdInsert.Parameters.Add("@CAP_PRESSO", SqlDbType.NVarChar, 5, "CAP_PRESSO")
            sqlCmdInsert.Parameters("@CAP_PRESSO").Value = xmlLoadDocument.SelectSingleNode("//UBICAZIONE/CAP_SOPRALLUOGO").InnerText

            sqlCmdInsert.Parameters.Add("@PROVINCIA_PRESSO", SqlDbType.NVarChar, 2, "PROVINCIA_PRESSO")
            sqlCmdInsert.Parameters("@PROVINCIA_PRESSO").Value = xmlLoadDocument.SelectSingleNode("//UBICAZIONE/PROVINCIA_SOPRALLUOGO").InnerText

            sqlCmdInsert.Parameters.Add("@VIA_PRESSO", SqlDbType.NVarChar, 50, "VIA_PRESSO")
            sqlCmdInsert.Parameters("@VIA_PRESSO").Value = xmlLoadDocument.SelectSingleNode("//UBICAZIONE/INDIRIZZO_SOPRALLUOGO").InnerText

            sqlCmdInsert.Parameters.Add("@CIVICO_PRESSO", SqlDbType.NVarChar, 10, "CIVICO_PRESSO")
            sqlCmdInsert.Parameters("@CIVICO_PRESSO").Value = xmlLoadDocument.SelectSingleNode("//UBICAZIONE/CIVICO_SOPRALLUOGO").InnerText

            sqlCmdInsert.Parameters.Add("@FRAZIONE_PRESSO", SqlDbType.NVarChar, 100, "FRAZIONE_PRESSO")
            sqlCmdInsert.Parameters("@FRAZIONE_PRESSO").Value = xmlLoadDocument.SelectSingleNode("//UBICAZIONE/FRAZIONE_SOPRALLUOGO").InnerText

            sqlCmdInsert.Parameters.Add("@COMUNE_UBICAZIONE", SqlDbType.NVarChar, 50, "COMUNE_UBICAZIONE")
            sqlCmdInsert.Parameters("@COMUNE_UBICAZIONE").Value = xmlLoadDocument.SelectSingleNode("//UBICAZIONE/COMUNEFABBRICATO").InnerText

            sqlCmdInsert.Parameters.Add("@COD_COMUNE_UBICAZIONE", SqlDbType.NVarChar, 50, "COD_COMUNE_UBICAZIONE")
            sqlCmdInsert.Parameters("@COD_COMUNE_UBICAZIONE").Value = xmlLoadDocument.SelectSingleNode("//UBICAZIONE/COD_COMUNEFAB").InnerText

            sqlCmdInsert.Parameters.Add("@CAP_UBICAZIONE", SqlDbType.NVarChar, 5, "CAP_UBICAZIONE")
            sqlCmdInsert.Parameters("@CAP_UBICAZIONE").Value = xmlLoadDocument.SelectSingleNode("//UBICAZIONE/CAPFABBRICATO").InnerText

            sqlCmdInsert.Parameters.Add("@PROVINCIA_UBICAZIONE", SqlDbType.NVarChar, 2, "PROVINCIA_UBICAZIONE")
            sqlCmdInsert.Parameters("@PROVINCIA_UBICAZIONE").Value = xmlLoadDocument.SelectSingleNode("//UBICAZIONE/PROVINCIAFABBRICATO").InnerText

            sqlCmdInsert.Parameters.Add("@VIA_UBICAZIONE", SqlDbType.NVarChar, 50, "VIA_UBICAZIONE")
            sqlCmdInsert.Parameters("@VIA_UBICAZIONE").Value = xmlLoadDocument.SelectSingleNode("//UBICAZIONE/INDIRIZZOFABBRICATO").InnerText

            sqlCmdInsert.Parameters.Add("@CIVICO_UBICAZIONE", SqlDbType.NVarChar, 10, "CIVICO_UBICAZIONE")
            sqlCmdInsert.Parameters("@CIVICO_UBICAZIONE").Value = xmlLoadDocument.SelectSingleNode("//UBICAZIONE/CIVICOFABBRICATO").InnerText

            sqlCmdInsert.Parameters.Add("@FRAZIONE_UBICAZIONE", SqlDbType.NVarChar, 100, "FRAZIONE_UBICAZIONE")
            sqlCmdInsert.Parameters("@FRAZIONE_UBICAZIONE").Value = xmlLoadDocument.SelectSingleNode("//UBICAZIONE/FRAZIONEFABBRICATO").InnerText

            sqlCmdInsert.Parameters.Add("@COGNOME_UBICAZIONE", SqlDbType.NVarChar, 100, "COGNOME_UBICAZIONE")
            sqlCmdInsert.Parameters("@COGNOME_UBICAZIONE").Value = xmlLoadDocument.SelectSingleNode("//UBICAZIONE/COGNOMEPROPRIETARIO").InnerText

            sqlCmdInsert.Parameters.Add("@NOME_UBICAZIONE", SqlDbType.NVarChar, 100, "NOME_UBICAZIONE")
            sqlCmdInsert.Parameters("@NOME_UBICAZIONE").Value = xmlLoadDocument.SelectSingleNode("//UBICAZIONE/NOMEPROPRIETARIO").InnerText

            sqlCmdInsert.Parameters.Add("@IDCONTATORE", SqlDbType.Int, 4, "IDCONTATORE")
            sqlCmdInsert.Parameters("@IDCONTATORE").Value = strIDDOC

            If blnUPDATE Then
                sqlCmdInsert.Parameters.Add("@ID_ANAGRAFE_CONTATORE", SqlDbType.Int, 4, "COD_CONTRIBUENTE")
                sqlCmdInsert.Parameters("@ID_ANAGRAFE_CONTATORE").Value = xmlLoadDocument.SelectSingleNode("//UBICAZIONE/ID_ANAGRAFE_CONTATORE").InnerText
            End If
            '/CONTRATTO/UBICAZIONE/ID_ANAGRAFE_CONTATORE

            sqlCmdInsert.ExecuteScalar()
            Dim strID As String
            If Not blnUPDATE Then
                strID = CStr(parameterItemID.Value)
                xmlLoadDocument.SelectSingleNode("//UBICAZIONE/ID_ANAGRAFE_CONTATORE").InnerText = strID
                xmlLoadDocument.Save(strXMLFileName)
            End If
            setANAGRAFE_CONTATORE = True

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBManager.setANAGRAFE_CONTATORE.errore: ", ex)
            setANAGRAFE_CONTATORE = False
            'scrittura nel file di log
        End Try

    End Function
#End Region
#Region "setTP_LETTURE"
    Protected Function setTP_LETTURE(ByVal sqlConn As SqlConnection, ByVal sqlTrans As SqlTransaction, _
     ByVal strXMLFileName As String, _
     ByVal strIDDOC As String, _
     ByVal strStoreName As String) As Boolean

        Try

            Dim xmlLoadDocument As New XmlDocument
            Dim sqlParm As New SqlParameter
            Dim sqlCmdInsert As SqlCommand = New SqlCommand(strStoreName, sqlConn, sqlTrans)

            setTP_LETTURE = False

            sqlCmdInsert.CommandType = CommandType.StoredProcedure
            xmlLoadDocument.Load(strXMLFileName)
            Dim parameterItemID As New SqlParameter("@ItemID", SqlDbType.Int, 4)
            parameterItemID.Direction = ParameterDirection.Output

            If Not blnUPDATE Then
                sqlCmdInsert.Parameters.Add(parameterItemID)
            End If

            sqlCmdInsert.Parameters.Add("@NUMEROMATRICOLA", SqlDbType.NVarChar, 50, "NUMEROMATRICOLA")
            sqlCmdInsert.Parameters("@NUMEROMATRICOLA").Value = xmlLoadDocument.SelectSingleNode("//DATI_CONTATORE/N_MATRICOLA").InnerText

            sqlCmdInsert.Parameters.Add("@LETTURA", SqlDbType.NVarChar, 50, "LETTURA")
            sqlCmdInsert.Parameters("@LETTURA").Value = xmlLoadDocument.SelectSingleNode("//DATI_CONTATORE/DATO_LETTURA").InnerText

            sqlCmdInsert.Parameters.Add("@DATA", SqlDbType.DateTime, 8, "DATA")
            sqlCmdInsert.Parameters("@DATA").Value = MyUtility.objToStr(xmlLoadDocument.SelectSingleNode("//DATI_CONTATORE/DATA_LETTURA").InnerText, MyUtility.TypeFieldXML.TypeDate)

            sqlCmdInsert.Parameters.Add("@NOTE", SqlDbType.VarChar, 500, "NOTE")
            sqlCmdInsert.Parameters("@NOTE").Value = xmlLoadDocument.SelectSingleNode("//DATI_CONTATORE/NOTE").InnerText


            sqlCmdInsert.Parameters.Add("@IDCONTATORE", SqlDbType.Int, 4, "IDCONTATORE")
            sqlCmdInsert.Parameters("@IDCONTATORE").Value = strIDDOC

            If blnUPDATE Then
                sqlCmdInsert.Parameters.Add("@IDLETTURACONTATORE", SqlDbType.Int, 4, "IDLETTURACONTATORE")
                sqlCmdInsert.Parameters("@IDLETTURACONTATORE").Value = xmlLoadDocument.SelectSingleNode("//DATI_CONTATORE/IDLETTURACONTATORE").InnerText
            End If

            sqlCmdInsert.ExecuteScalar()


            Dim strID As String
            If Not blnUPDATE Then
                strID = CStr(parameterItemID.Value)
                xmlLoadDocument.SelectSingleNode("//DATI_CONTATORE/IDLETTURACONTATORE").InnerText = strID
                xmlLoadDocument.Save(strXMLFileName)
            End If
            setTP_LETTURE = True

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBManager.setTP_LETTURE.errore: ", ex)
            setTP_LETTURE = False
            'scrittura nel file di log
        End Try

    End Function
#End Region
#Region "setTP_PREVENTIVO"
    Protected Function setTP_PREVENTIVO(ByVal sqlConn As SqlConnection, _
    ByVal sqlTrans As SqlTransaction, _
    ByVal strXMLFileName As String, _
    ByVal strIDDOC As String, _
    ByVal strStoreName As String) As Boolean
        Try

            Dim xmlLoadDocument As New XmlDocument
            Dim sqlParm As New SqlParameter
            Dim sqlCmdInsert As SqlCommand = New SqlCommand(strStoreName, sqlConn, sqlTrans)

            setTP_PREVENTIVO = False

            sqlCmdInsert.CommandType = CommandType.StoredProcedure
            xmlLoadDocument.Load(strXMLFileName)
            Dim parameterItemID As New SqlParameter("@ItemID", SqlDbType.Int, 4)
            parameterItemID.Direction = ParameterDirection.Output

            If Not blnUPDATE Then
                sqlCmdInsert.Parameters.Add(parameterItemID)
            End If

            sqlCmdInsert.Parameters.Add("@DIAMETROPRESA", SqlDbType.NVarChar, 10, "DIAMETROPRESA")
            sqlCmdInsert.Parameters("@DIAMETROPRESA").Value = xmlLoadDocument.SelectSingleNode("//PREVENTIVO/DIAMETRO_PRESA").InnerText

            sqlCmdInsert.Parameters.Add("@DIAMETROCONTATORE", SqlDbType.NVarChar, 10, "DIAMETROCONTATORE")
            sqlCmdInsert.Parameters("@DIAMETROCONTATORE").Value = xmlLoadDocument.SelectSingleNode("//PREVENTIVO/DIAMETRO_CONTATORE").InnerText

            sqlCmdInsert.Parameters.Add("@IMPORTOPREVENTIVO", SqlDbType.Money, 8, "IMPORTOPREVENTIVO")
            If Len(xmlLoadDocument.SelectSingleNode("//PREVENTIVO/IMPORTO").InnerText) = 0 Then
                sqlCmdInsert.Parameters("@IMPORTOPREVENTIVO").Value = System.DBNull.Value
            Else
                sqlCmdInsert.Parameters("@IMPORTOPREVENTIVO").Value = xmlLoadDocument.SelectSingleNode("//PREVENTIVO/IMPORTO").InnerText
            End If

            sqlCmdInsert.Parameters.Add("@NOTE", SqlDbType.VarChar, 500, "NOTE")
            sqlCmdInsert.Parameters("@NOTE").Value = xmlLoadDocument.SelectSingleNode("//PREVENTIVO/NOTE").InnerText

            sqlCmdInsert.Parameters.Add("@DATAPAGAMENTOPREVENTIVO", SqlDbType.DateTime, 8, "DATAPAGAMENTOPREVENTIVO")

            If Len(xmlLoadDocument.SelectSingleNode("//REGISTRAZIONE_PAGAMENTI/DATA_PAGAMENTO_PREVENTIVO").InnerText) = 0 Then
                sqlCmdInsert.Parameters("@DATAPAGAMENTOPREVENTIVO").Value = System.DBNull.Value
            Else
                sqlCmdInsert.Parameters("@DATAPAGAMENTOPREVENTIVO").Value = xmlLoadDocument.SelectSingleNode("//REGISTRAZIONE_PAGAMENTI/DATA_PAGAMENTO_PREVENTIVO").InnerText
            End If

            sqlCmdInsert.Parameters.Add("@NUMEROFATTURA", SqlDbType.NVarChar, 50, "NUMEROFATTURA")
            sqlCmdInsert.Parameters("@NUMEROFATTURA").Value = xmlLoadDocument.SelectSingleNode("//REGISTRAZIONE_PAGAMENTI/NUMEROFATTURA_PREVENTIVO").InnerText

            sqlCmdInsert.Parameters.Add("@IDCONTATORE", SqlDbType.Int, 4, "IDCONTATORE")
            sqlCmdInsert.Parameters("@IDCONTATORE").Value = strIDDOC
            If blnUPDATE Then
                sqlCmdInsert.Parameters.Add("@IDPREVENTIVO", SqlDbType.Int, 4, "IDPREVENTIVO")
                sqlCmdInsert.Parameters("@IDPREVENTIVO").Value = xmlLoadDocument.SelectSingleNode("//PREVENTIVO/IDPREVENTIVO").InnerText
            End If

            sqlCmdInsert.ExecuteScalar()
            Dim strID As String
            If Not blnUPDATE Then
                strID = CStr(parameterItemID.Value)
                xmlLoadDocument.SelectSingleNode("//PREVENTIVO/IDPREVENTIVO").InnerText = strID
                xmlLoadDocument.Save(strXMLFileName)
            End If
            setTP_PREVENTIVO = True

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBManager.setTP_PREVENTIVO.errore: ", ex)
            setTP_PREVENTIVO = False
            'scrittura nel file di log
        End Try

    End Function
#End Region
#Region "getANAINTEATATARIO"
    Private Sub getANAINTEATATARIO(ByVal strIDDOC As String, _
    ByVal strXMLFileName As String)
        Try
            Dim cmd As SqlCommand
            Dim sqlConn As New SqlConnection
            Dim sparam As SqlParameter
            Dim Reader As SqlClient.SqlDataReader
            Dim xmlLoadDocument As New XmlDocument

            cmd = New SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_selectANAINTESTATARIO"

            sqlConn.ConnectionString = ConstSession.StringConnection

            cmd.Connection = sqlConn


            sparam = New SqlClient.SqlParameter
            sparam.ParameterName = "@IDCONTATORE"
            sparam.SqlDbType = SqlDbType.Int
            cmd.Parameters.Add(sparam)


            cmd.Parameters("@IDCONTATORE").Value = strIDDOC
            sqlConn.Open()
            Reader = cmd.ExecuteReader()
            xmlLoadDocument.Load(strXMLFileName)
            Do While Reader.Read()

                xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/COGNOME").InnerText = utility.stringoperation.formatstring(Reader.Item("COGNOME"))
                xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/NOME").InnerText = utility.stringoperation.formatstring(Reader.Item("NOME"))
                xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/CODICE_FISCALE").InnerText = utility.stringoperation.formatstring(Reader.Item("COD_FISCALE"))
                xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/SESSO").InnerText = utility.stringoperation.formatstring(Reader.Item("SESSO"))
                xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/DATA_NASCITA").InnerText = Utility.StringOperation.FormatDateTime(Reader.Item("DATA_NASCITA"))
                xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/COMUNE_NASCITA").InnerText = utility.stringoperation.formatstring(Reader.Item("COMUNE_NASCITA"))
                xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/COD_COMUNE_NASCITA").InnerText = utility.stringoperation.formatstring(Reader.Item("COD_COMUNE_NASCITA"))
                xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/PROVINCIA_NASCITA").InnerText = utility.stringoperation.formatstring(Reader.Item("PROV_NASCITA"))
                xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/RESIDENZA/VIA_RES").InnerText = utility.stringoperation.formatstring(Reader.Item("VIA_RES"))
                xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/RESIDENZA/FRAZIONE_RES").InnerText = utility.stringoperation.formatstring(Reader.Item("FRAZIONE_RES"))
                xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/RESIDENZA/NUM_CIVICO_RES").InnerText = utility.stringoperation.formatstring(Reader.Item("CIVICO_RES"))
                xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/RESIDENZA/CAP_RES").InnerText = utility.stringoperation.formatstring(Reader.Item("CAP_RES"))
                xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/RESIDENZA/COMUNE_RES").InnerText = utility.stringoperation.formatstring(Reader.Item("CITTA_RES"))
                xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/RESIDENZA/COD_COMUNE_RES").InnerText = utility.stringoperation.formatstring(Reader.Item("COD_COMUNE_RES"))
                xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/RESIDENZA/PROVINCIA_RES").InnerText = utility.stringoperation.formatstring(Reader.Item("PROVINCIA_RES"))
                xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/TELEFONO").InnerText = utility.stringoperation.formatstring(Reader.Item("TELEFONO"))
                xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/CELLULARE").InnerText = utility.stringoperation.formatstring(Reader.Item("CELLULARE"))
                xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/PROFESSIONE").InnerText = utility.stringoperation.formatstring(Reader.Item("IDTITOLOSOGGETTO"))
                xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/COD_CONTRIBUENTE").InnerText = utility.stringoperation.formatstring(Reader.Item("COD_CONTRIBUENTE"))
            Loop

            xmlLoadDocument.Save(strXMLFileName)
            Reader.Close()
            sqlConn.Close()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBManager.getANAINTEATATARIO.errore: ", ex)
            Throw New System.Exception(ex.Message & " [Name Function:getANAINTEATATARIO][Class: DBManager]")
        End Try
    End Sub
#End Region
#Region "getANACONTATORE"
    Private Sub getANACONTATORE(ByVal strIDDOC As String,
    ByVal strXMLFileName As String)
        Try
            Dim cmd As SqlCommand
            Dim sqlConn As New SqlConnection
            Dim sparam As SqlParameter
            Dim Reader As SqlClient.SqlDataReader
            Dim xmlLoadDocument As New XmlDocument

            cmd = New SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_selectANACONTATORE"

            sqlConn.ConnectionString = ConstSession.StringConnection

            cmd.Connection = sqlConn


            sparam = New SqlClient.SqlParameter
            sparam.ParameterName = "@IDCONTATORE"
            sparam.SqlDbType = SqlDbType.Int
            cmd.Parameters.Add(sparam)


            cmd.Parameters("@IDCONTATORE").Value = strIDDOC
            sqlConn.Open()
            Reader = cmd.ExecuteReader()
            xmlLoadDocument.Load(strXMLFileName)

            Do While Reader.Read()

                xmlLoadDocument.SelectSingleNode("//UBICAZIONE/COGNOME_SOPRALLUOGO").InnerText = Utility.StringOperation.FormatString(Reader.Item("COGNOME_PRESSO"))
                xmlLoadDocument.SelectSingleNode("//UBICAZIONE/NOME_SOPRALLUOGO").InnerText = Utility.StringOperation.FormatString(Reader.Item("NOME_PRESSO"))
                xmlLoadDocument.SelectSingleNode("//UBICAZIONE/COMUNE_SOPRALLUOGO").InnerText = Utility.StringOperation.FormatString(Reader.Item("COMUNE_PRESSO"))
                xmlLoadDocument.SelectSingleNode("//UBICAZIONE/COD_COMUNESOPR").InnerText = Utility.StringOperation.FormatString(Reader.Item("COD_COMUNE_PRESSO"))
                xmlLoadDocument.SelectSingleNode("//UBICAZIONE/CAP_SOPRALLUOGO").InnerText = Utility.StringOperation.FormatString(Reader.Item("CAP_PRESSO"))
                xmlLoadDocument.SelectSingleNode("//UBICAZIONE/PROVINCIA_SOPRALLUOGO").InnerText = Utility.StringOperation.FormatString(Reader.Item("PROVINCIA_PRESSO"))
                xmlLoadDocument.SelectSingleNode("//UBICAZIONE/INDIRIZZO_SOPRALLUOGO").InnerText = Utility.StringOperation.FormatString(Reader.Item("VIA_PRESSO"))
                xmlLoadDocument.SelectSingleNode("//UBICAZIONE/CIVICO_SOPRALLUOGO").InnerText = Utility.StringOperation.FormatString(Reader.Item("CIVICO_PRESSO"))
                xmlLoadDocument.SelectSingleNode("//UBICAZIONE/FRAZIONE_SOPRALLUOGO").InnerText = Utility.StringOperation.FormatString(Reader.Item("FRAZIONE_PRESSO"))
                xmlLoadDocument.SelectSingleNode("//UBICAZIONE/COMUNEFABBRICATO").InnerText = Utility.StringOperation.FormatString(Reader.Item("COMUNE_UBICAZIONE"))
                xmlLoadDocument.SelectSingleNode("//UBICAZIONE/COD_COMUNEFAB").InnerText = Utility.StringOperation.FormatString(Reader.Item("COD_COMUNE_UBICAZIONE"))
                xmlLoadDocument.SelectSingleNode("//UBICAZIONE/CAPFABBRICATO").InnerText = Utility.StringOperation.FormatString(Reader.Item("CAP_UBICAZIONE"))
                xmlLoadDocument.SelectSingleNode("//UBICAZIONE/PROVINCIAFABBRICATO").InnerText = Utility.StringOperation.FormatString(Reader.Item("PROVINCIA_UBICAZIONE"))
                xmlLoadDocument.SelectSingleNode("//UBICAZIONE/INDIRIZZOFABBRICATO").InnerText = Utility.StringOperation.FormatString(Reader.Item("VIA_UBICAZIONE"))
                xmlLoadDocument.SelectSingleNode("//UBICAZIONE/CIVICOFABBRICATO").InnerText = Utility.StringOperation.FormatString(Reader.Item("CIVICO_UBICAZIONE"))
                xmlLoadDocument.SelectSingleNode("//UBICAZIONE/FRAZIONEFABBRICATO").InnerText = Utility.StringOperation.FormatString(Reader.Item("FRAZIONE_UBICAZIONE"))
                xmlLoadDocument.SelectSingleNode("//UBICAZIONE/COGNOMEPROPRIETARIO").InnerText = Utility.StringOperation.FormatString(Reader.Item("COGNOME_UBICAZIONE"))
                xmlLoadDocument.SelectSingleNode("//UBICAZIONE/NOMEPROPRIETARIO").InnerText = Utility.StringOperation.FormatString(Reader.Item("NOME_UBICAZIONE"))
                xmlLoadDocument.SelectSingleNode("//UBICAZIONE/ID_ANAGRAFE_CONTATORE").InnerText = Utility.StringOperation.FormatString(Reader.Item("COD_CONTRIBUENTE"))

            Loop

            xmlLoadDocument.Save(strXMLFileName)
            Reader.Close()
            sqlConn.Close()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBManager.getANACONTATORE.errore: ", ex)
        End Try

    End Sub
#End Region
#Region "getTP_CONTATORI"
    Private Sub getTP_CONTATORI(ByVal strIDDOC As String,
    ByVal strXMLFileName As String)
        Try
            Dim cmd As SqlCommand
            Dim sqlConn As New SqlConnection
            Dim sparam As SqlParameter
            Dim Reader As SqlClient.SqlDataReader
            Dim xmlLoadDocument As New XmlDocument

            cmd = New SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_SelectTP_CONTATORI"

            sqlConn.ConnectionString = ConstSession.StringConnection

            cmd.Connection = sqlConn


            sparam = New SqlClient.SqlParameter
            sparam.ParameterName = "@IDCONTATORE"
            sparam.SqlDbType = SqlDbType.Int
            cmd.Parameters.Add(sparam)


            cmd.Parameters("@IDCONTATORE").Value = strIDDOC
            sqlConn.Open()
            Reader = cmd.ExecuteReader()
            xmlLoadDocument.Load(strXMLFileName)

            Do While Reader.Read()

                xmlLoadDocument.SelectSingleNode("//DATI_UTENZA/TIPOLOGIA_UTENZA").InnerText = Utility.StringOperation.FormatString(Reader.Item("IDTIPOUTENZA"))
                xmlLoadDocument.SelectSingleNode("//DATI_UTENZA/N_UTENZE").InnerText = Utility.StringOperation.FormatString(Reader.Item("NUMERO_UTENZE"))
                xmlLoadDocument.SelectSingleNode("//DATI_UTENZA/PUNTI_UTILIZZO").InnerText = Utility.StringOperation.FormatString(Reader.Item("PUNTIUTILIZZO"))
                xmlLoadDocument.SelectSingleNode("//REGISTRAZIONE_PAGAMENTI/DATA_PAGAMENTO_PREISTRUTTORIA").InnerText = Utility.StringOperation.FormatDateTime(Utility.StringOperation.FormatString(Reader.Item("DATA_PAGAMENTO")))
                xmlLoadDocument.SelectSingleNode("//REGISTRAZIONE_PAGAMENTI/NUMEROFATTURA_PREISTRUTTORIA").InnerText = Utility.StringOperation.FormatString(Reader.Item("NUMERO_FATTURA"))
                xmlLoadDocument.SelectSingleNode("//FIRMA_CONTRATTO/DATA_SOTTOSCRIZIONE").InnerText = Utility.StringOperation.FormatDateTime(Utility.StringOperation.FormatString(Reader.Item("DATA_REGISTRAZIONE")))
                xmlLoadDocument.SelectSingleNode("//FIRMA_CONTRATTO/NOTE").InnerText = Utility.StringOperation.FormatString(Reader.Item("NOTE"))

            Loop

            xmlLoadDocument.Save(strXMLFileName)
            Reader.Close()
            sqlConn.Close()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBManager.getTP_CONTATORI.errore: ", ex)
        End Try
    End Sub
#End Region
#Region "getTP_LETTURE"
    Private Sub getTP_LETTURE(ByVal strIDDOC As String,
    ByVal strXMLFileName As String)
        Try
            Dim cmd As SqlCommand
            Dim sqlConn As New SqlConnection
            Dim sparam As SqlParameter
            Dim Reader As SqlClient.SqlDataReader
            Dim xmlLoadDocument As New XmlDocument

            cmd = New SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_SelectTP_LETTURE"

            sqlConn.ConnectionString = ConstSession.StringConnection

            cmd.Connection = sqlConn


            sparam = New SqlClient.SqlParameter
            sparam.ParameterName = "@IDCONTATORE"
            sparam.SqlDbType = SqlDbType.Int
            cmd.Parameters.Add(sparam)


            cmd.Parameters("@IDCONTATORE").Value = strIDDOC
            sqlConn.Open()
            Reader = cmd.ExecuteReader()
            xmlLoadDocument.Load(strXMLFileName)

            Do While Reader.Read()
                xmlLoadDocument.SelectSingleNode("//DATI_CONTATORE/N_MATRICOLA").InnerText = Utility.StringOperation.FormatString(Reader.Item("NUMEROMATRICOLA"))
                xmlLoadDocument.SelectSingleNode("//DATI_CONTATORE/DATO_LETTURA").InnerText = Utility.StringOperation.FormatString(Reader.Item("LETTURA"))
                xmlLoadDocument.SelectSingleNode("//DATI_CONTATORE/DATA_LETTURA").InnerText = Utility.StringOperation.FormatDateTime(Utility.StringOperation.FormatString(Reader.Item("DATA")))
                xmlLoadDocument.SelectSingleNode("//DATI_CONTATORE/NOTE").InnerText = Utility.StringOperation.FormatString(Reader.Item("NOTE"))
                xmlLoadDocument.SelectSingleNode("//DATI_CONTATORE/IDLETTURACONTATORE").InnerText = Utility.StringOperation.FormatString(Reader.Item("IDLETTURACONTATORE"))
            Loop

            xmlLoadDocument.Save(strXMLFileName)
            Reader.Close()
            sqlConn.Close()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBManager.getTP_LETTURE.errore: ", ex)
        End Try
    End Sub
#End Region
#Region "getTP_PREVENTIVO"
    Private Sub getTP_PREVENTIVO(ByVal strIDDOC As String,
    ByVal strXMLFileName As String)
        Try
            Dim cmd As SqlCommand
            Dim sqlConn As New SqlConnection
            Dim sparam As SqlParameter
            Dim Reader As SqlClient.SqlDataReader
            Dim xmlLoadDocument As New XmlDocument

            cmd = New SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_SelectTP_PREVENTIVO"

            sqlConn.ConnectionString = ConstSession.StringConnection

            cmd.Connection = sqlConn


            sparam = New SqlClient.SqlParameter
            sparam.ParameterName = "@IDCONTATORE"
            sparam.SqlDbType = SqlDbType.Int
            cmd.Parameters.Add(sparam)


            cmd.Parameters("@IDCONTATORE").Value = strIDDOC
            sqlConn.Open()
            Reader = cmd.ExecuteReader()
            xmlLoadDocument.Load(strXMLFileName)

            Do While Reader.Read()

                xmlLoadDocument.SelectSingleNode("//PREVENTIVO/DIAMETRO_PRESA").InnerText = Utility.StringOperation.FormatString(Reader.Item("DIAMETROPRESA"))
                xmlLoadDocument.SelectSingleNode("//PREVENTIVO/DIAMETRO_CONTATORE").InnerText = Utility.StringOperation.FormatString(Reader.Item("DIAMETROCONTATORE"))
                xmlLoadDocument.SelectSingleNode("//PREVENTIVO/IMPORTO").InnerText = Utility.StringOperation.FormatString(Reader.Item("IMPORTOPREVENTIVO"))
                xmlLoadDocument.SelectSingleNode("//PREVENTIVO/NOTE").InnerText = Utility.StringOperation.FormatString(Reader.Item("NOTE"))
                xmlLoadDocument.SelectSingleNode("//REGISTRAZIONE_PAGAMENTI/DATA_PAGAMENTO_PREVENTIVO").InnerText = Utility.StringOperation.FormatDateTime(Utility.StringOperation.FormatString(Reader.Item("DATAPAGAMENTOPREVENTIVO")))
                xmlLoadDocument.SelectSingleNode("//REGISTRAZIONE_PAGAMENTI/NUMEROFATTURA_PREVENTIVO").InnerText = Utility.StringOperation.FormatString(Reader.Item("NUMEROFATTURA"))
                xmlLoadDocument.SelectSingleNode("//PREVENTIVO/IDPREVENTIVO").InnerText = Utility.StringOperation.FormatString(Reader.Item("IDPREVENTIVO"))

            Loop

            xmlLoadDocument.Save(strXMLFileName)
            Reader.Close()
            sqlConn.Close()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBManager.getTP_PREVENTITVO.errore: ", ex)
        End Try
    End Sub
#End Region
#Region "getTP_CONTRATTI"
    Private Sub getTP_CONTRATTI(ByVal strIDDOC As String,
    ByVal strXMLFileName As String)
        Try
            Dim cmd As SqlCommand
            Dim sqlConn As New SqlConnection
            Dim sparam As SqlParameter
            Dim Reader As SqlClient.SqlDataReader
            Dim xmlLoadDocument As New XmlDocument

            cmd = New SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "sp_SelectTP_CONTRATTI"

            sqlConn.ConnectionString = ConstSession.StringConnection

            cmd.Connection = sqlConn


            sparam = New SqlClient.SqlParameter
            sparam.ParameterName = "@IDCONTATORE"
            sparam.SqlDbType = SqlDbType.Int
            cmd.Parameters.Add(sparam)


            cmd.Parameters("@IDCONTATORE").Value = strIDDOC
            sqlConn.Open()
            Reader = cmd.ExecuteReader()
            xmlLoadDocument.Load(strXMLFileName)
            Do While Reader.Read()
                xmlLoadDocument.SelectSingleNode("//EMISSIONE_CONTRATTO/COD_CONTRATTO").InnerText = Utility.StringOperation.FormatString(Reader.Item("COD_CONTRATTO"))
            Loop
            xmlLoadDocument.Save(strXMLFileName)
            Reader.Close()
            sqlConn.Close()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBManager.getTP_CONTRATTI.errore: ", ex)
        End Try
    End Sub
#End Region


#Region "setModifyStoredData"

    Private Sub setModifyStoredData(ByVal strIDDOC As String,
     ByVal strIDDOCOLD As String,
     ByVal strXMLFileName As String,
     ByVal blnTerminato As Boolean)
        Try

            'Utilizzo di StoreProcedure

            Dim sqlConn As New SqlConnection
            Dim sqlParm As New SqlParameter
            Dim sqlTrans As SqlTransaction
            Dim blnOkDB As Boolean

            sqlConn.ConnectionString = ConstSession.StringConnection
            sqlConn.Open()
            'Inizio la transazione
            sqlTrans = sqlConn.BeginTransaction

            'Inserimento dei dati nel DataBase OpenUtenze
            'IN Questa fase vengono savti i dati anche per lo Storico Tabella 
            'TP_CONTRATTI_STO tutto questo sotto transazione

            blnOkDB = setTP_CONTATORI(sqlConn, sqlTrans, strXMLFileName, strIDDOC, "sp_InsertTP_CONTATORI")
            'Salvataggio dei dati Per Lo storico
            '*****************************************************************************************************
            If blnOkDB Then blnOkDB = setTP_CONTRATTI_STO(sqlConn, sqlTrans, strIDDOCOLD, "sp_InsertTP_CONTRATTI_STO")
            '*****************************************************************************************************
            blnUPDATE = True
            If blnOkDB Then blnOkDB = setTP_CONTRATTI(sqlConn, sqlTrans, strXMLFileName, strIDDOC, "sp_UpdateTP_CONTRATTI", blnTerminato)
            blnUPDATE = False
            If blnOkDB Then blnOkDB = setTR_CONTATORI_INTESTATARIO(sqlConn, sqlTrans, strXMLFileName, strIDDOC, "sp_TRInsertCommand")
            If blnOkDB Then blnOkDB = setANAGRAFE_CONTATORE(sqlConn, sqlTrans, strXMLFileName, strIDDOC, "sp_InsertANAGRAFE_CONTATORE")
            If blnOkDB Then blnOkDB = setTP_LETTURE(sqlConn, sqlTrans, strXMLFileName, strIDDOC, "sp_InsertTP_LETTURE")
            If blnOkDB Then blnOkDB = setTP_PREVENTIVO(sqlConn, sqlTrans, strXMLFileName, strIDDOC, "sp_InsertTP_PREVENTIVO")

            If blnOkDB Then
                sqlTrans.Commit()
            Else
                sqlTrans.Rollback()
                Throw New System.Exception("Si è verificato un errore durante il  Salvataggio dei dati nel DataBase:OpenUtenze-Verificare il file ErrorDb_logError.log !")
            End If
            sqlConn.Close()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBManager.setModifyStoredData.errore: ", ex)
        End Try
    End Sub
#End Region

#Region "setTR_CONTATORI_INTESTATARIO"
    Protected Function setTR_CONTATORI_INTESTATARIO(ByVal sqlConn As SqlConnection, ByVal sqlTrans As SqlTransaction,
      ByVal strXMLFileName As String,
      ByVal strIDDOC As String,
      ByVal strStoreName As String) As Boolean
        Try

            Dim xmlLoadDocument As New XmlDocument
            Dim sqlParm As New SqlParameter
            Dim sqlCmdInsertTR As SqlCommand = New SqlCommand(strStoreName, sqlConn, sqlTrans)

            setTR_CONTATORI_INTESTATARIO = False

            sqlCmdInsertTR.CommandType = CommandType.StoredProcedure

            xmlLoadDocument.Load(strXMLFileName)


            sqlCmdInsertTR.Parameters.Add("@IDCONTATORE", SqlDbType.Int, 4, "IDCONTATORE")
            sqlCmdInsertTR.Parameters("@IDCONTATORE").Value = strIDDOC
            sqlCmdInsertTR.Parameters.Add("@COD_CONTRIBUENTE", SqlDbType.Int, 4, "COD_CONTRIBUENTE")
            sqlCmdInsertTR.Parameters("@COD_CONTRIBUENTE").Value = xmlLoadDocument.SelectSingleNode("//ANA_INTESTATARIO/COD_CONTRIBUENTE").InnerText


            sqlCmdInsertTR.ExecuteScalar()


            setTR_CONTATORI_INTESTATARIO = True

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBManager.setTR_CONTATORI_INTESTATARIO.errore: ", ex)
            setTR_CONTATORI_INTESTATARIO = False
            'scrittura nel file di log
        End Try

    End Function
#End Region
#Region "getContrattiTerminati"
    Public Function getContrattiTerminati(ByVal strCognome As String, ByVal strNome As String, ByVal strComune As String) As Integer
        'Funzione che VIene chiamata dalla pagina VisualizzaContrattiTermina.aspx

        Dim sqlConn As New SqlConnection
        Dim sqlParm As New SqlParameter
        Dim lngRowCount As Int32
        'se trova record ritorna 0
        Try
            sqlConn.ConnectionString = ConstSession.StringConnection
            sqlConn.Open()

            Dim sqlCmd As SqlCommand = New SqlCommand("sp_ModifyRecord", sqlConn)

            sqlCmd.CommandType = CommandType.StoredProcedure

            sqlParm = sqlCmd.Parameters.Add("@RowCount", SqlDbType.Int)
            sqlParm.Direction = ParameterDirection.ReturnValue

            sqlCmd.Parameters.Add("@COGNOME", SqlDbType.VarChar, 500, "COGNOME")
            sqlCmd.Parameters("@COGNOME").Value = strCognome

            sqlCmd.Parameters.Add("@NOME", SqlDbType.VarChar, 500, "NOME")
            sqlCmd.Parameters("@NOME").Value = strNome

            sqlCmd.Parameters.Add("@COMUNE", SqlDbType.VarChar, 500, "CITTA_RES")
            sqlCmd.Parameters("@COMUNE").Value = strComune

            sqlCmd.ExecuteNonQuery()

            lngRowCount = sqlCmd.Parameters("@RowCount").Value
            sqlConn.Close()

            If lngRowCount = 0 Then
                Return -1
            End If

        Catch ex As Exception
            'Genero 'l'errore 
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBManager.getContrattiTerminati.errore: ", ex)
            Throw New System.Exception(ex.Message & " [Name Function:getContrattiTerminati][Class: DBManager]")
        End Try
    End Function
#End Region

#Region "setTP_CONTRATTI_STO"
    Protected Function setTP_CONTRATTI_STO(ByVal sqlConn As SqlConnection, _
   ByVal sqlTrans As SqlTransaction, _
   ByVal strIDDOCOLD As String, _
   ByVal strStoreName As String _
     ) As Boolean
        Try


            setTP_CONTRATTI_STO = False

            Dim sqlCmdInsert As SqlCommand = New SqlCommand(strStoreName, sqlConn, sqlTrans)
            sqlCmdInsert.CommandType = CommandType.StoredProcedure

            sqlCmdInsert.Parameters.Add("@IDCONTATORE", SqlDbType.Int, 4, "IDCONTATORE")
            sqlCmdInsert.Parameters("@IDCONTATORE").Value = CType(strIDDOCOLD, Integer)

            sqlCmdInsert.ExecuteNonQuery()

            setTP_CONTRATTI_STO = True

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBManager.setTP_CONTRATTI_STO.errore: ", ex)
            setTP_CONTRATTI_STO = False
            'scrittura nel file di log
        End Try

    End Function
#End Region

#Region "SOSTITUZIONE CONTRATTI"
    '********************************************************************************
    'Name : setTP_CONTATORISOSTITUITI
    '
    'Class :DBManager
    '
    'Calls: SostituzioneContratto.vb ----Class------
    '
    '
    'Author: Antonello Lo Bianco 30 luglio, 2003
    '*********************************************************************************
    Protected Function setTP_CONTATORISOSTITUITI(ByVal sqlConn As SqlConnection, _
     ByVal sqlTrans As SqlTransaction, _
     ByVal strXMLFileName As String, _
     ByVal strIDDOC As String, _
     ByVal strStoreName As String) As Boolean
        Try

            Dim xmlLoadDocument As New XmlDocument

            Dim sqlCmdInsert As SqlCommand = New SqlCommand(strStoreName, sqlConn, sqlTrans)
            setTP_CONTATORISOSTITUITI = False

            sqlCmdInsert.CommandType = CommandType.StoredProcedure
            xmlLoadDocument.Load(strXMLFileName)
            sqlCmdInsert.Parameters.Add("@IDCONTATORE", SqlDbType.Int, 4, "IDCONTATORE")
            sqlCmdInsert.Parameters("@IDCONTATORE").Value = strIDDOC

            sqlCmdInsert.Parameters.Add("@DATA_CESSAZIONE", SqlDbType.DateTime, 8, "DATA_CESSAZIONE")
            sqlCmdInsert.Parameters("@DATA_CESSAZIONE").Value = xmlLoadDocument.SelectSingleNode("//DATI_CONTATORE/DATA_CESSAZIONE").InnerText

            sqlCmdInsert.Parameters.Add("@DATA_INSTALLAZIONE", SqlDbType.DateTime, 8, "DATA_CESSAZIONE")
            sqlCmdInsert.Parameters("@DATA_INSTALLAZIONE").Value = xmlLoadDocument.SelectSingleNode("//DATI_CONTATORE/DATA_INSTALLAZIONE").InnerText

            sqlCmdInsert.Parameters.Add("@LETTURA_VECCHIO", SqlDbType.BigInt, 9, "LETTURA_VECCHIO")
            sqlCmdInsert.Parameters("@LETTURA_VECCHIO").Value = xmlLoadDocument.SelectSingleNode("//DATI_CONTATORE/LETTURA_VECCHIO").InnerText
            sqlCmdInsert.Parameters.Add("@LETTURA_NUOVO", SqlDbType.BigInt, 9, "LETTURA_NUOVO")
            sqlCmdInsert.Parameters("@LETTURA_NUOVO").Value = xmlLoadDocument.SelectSingleNode("//DATI_CONTATORE/LETTURA_NUOVO").InnerText

            sqlCmdInsert.Parameters.Add("@SOSTITUITO", SqlDbType.Bit, 1, "SOSTITUITO")
            sqlCmdInsert.Parameters("@SOSTITUITO").Value = 1

            sqlCmdInsert.Parameters.Add("@NOTE", SqlDbType.VarChar, 500, "NOTE")
            sqlCmdInsert.Parameters("@NOTE").Value = xmlLoadDocument.SelectSingleNode("//DATI_CONTATORE/NOTE").InnerText()

            sqlCmdInsert.ExecuteNonQuery()

            setTP_CONTATORISOSTITUITI = True

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBManager.setTP_CONTATORISOSTITUITI.errore: ", ex)
            setTP_CONTATORISOSTITUITI = False
            'clsFile.ScriviFileLog_Error("DBManager.vb--Err:" & ex.Message, "ContrattiTerminatiPendenti.aspx", "ErrorDB" & ConfigurationManager.AppSettings("FileName"), strPath & ConfigurationManager.AppSettings("FilePath"), "")
        End Try

    End Function

    '********************************************************************************
    'Name : setTP_PREVENTIVOSOSTITUZIONE
    '
    'Class :DBManager
    '
    'Calls: SostituzioneContratto.vb ----Class------
    '
    '
    'Author: Antonello Lo Bianco 30 luglio, 2003
    '*********************************************************************************
    Protected Function setTP_PREVENTIVOSOSTITUZIONE(ByVal sqlConn As SqlConnection, _
    ByVal sqlTrans As SqlTransaction, _
    ByVal strXMLFileName As String, _
    ByVal strIDDOC As String, _
    ByVal strStoreName As String) As Boolean
        Try

            Dim xmlLoadDocument As New XmlDocument
            Dim sqlParm As New SqlParameter
            Dim sqlCmdInsert As SqlCommand = New SqlCommand(strStoreName, sqlConn, sqlTrans)

            setTP_PREVENTIVOSOSTITUZIONE = False

            sqlCmdInsert.CommandType = CommandType.StoredProcedure
            xmlLoadDocument.Load(strXMLFileName)

            sqlCmdInsert.Parameters.Add("@DIAMETROPRESA", SqlDbType.NVarChar, 10, "DIAMETROPRESA")
            sqlCmdInsert.Parameters("@DIAMETROPRESA").Value = xmlLoadDocument.SelectSingleNode("//PREVENTIVO/DIAMETRO_PRESA").InnerText

            sqlCmdInsert.Parameters.Add("@DIAMETROCONTATORE", SqlDbType.NVarChar, 10, "DIAMETROCONTATORE")
            sqlCmdInsert.Parameters("@DIAMETROCONTATORE").Value = xmlLoadDocument.SelectSingleNode("//PREVENTIVO/DIAMETRO_CONTATORE").InnerText


            sqlCmdInsert.Parameters.Add("@IDPREVENTIVO", SqlDbType.Int, 4, "IDPREVENTIVO")
            sqlCmdInsert.Parameters("@IDPREVENTIVO").Value = xmlLoadDocument.SelectSingleNode("//PREVENTIVO/IDPREVENTIVO").InnerText


            sqlCmdInsert.ExecuteScalar()
            setTP_PREVENTIVOSOSTITUZIONE = True

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBManager.setTP_PREVENTIVOSOSTITUZIONE.errore: ", ex)
            setTP_PREVENTIVOSOSTITUZIONE = False
            'scrittura nel file di log
        End Try

    End Function
    '********************************************************************************
    'Name : setTP_CONTATORI_STO
    '
    'Class :DBManager
    '
    'Calls: SostituzioneContratto.vb ----Class------
    'Scope: Aggiorna o Inserisce I dati nella tebella dello storico  TP_CONTATORI_STO
    '
    'Author: Antonello Lo Bianco 30 luglio, 2003
    '*********************************************************************************
    Protected Function setTP_CONTATORI_STO(ByVal sqlConn As SqlConnection, _
   ByVal sqlTrans As SqlTransaction, _
   ByVal strIDDOCOLD As String, _
   ByVal strStoreName As String _
     ) As Boolean
        Try


            setTP_CONTATORI_STO = False

            Dim sqlCmdInsert As SqlCommand = New SqlCommand(strStoreName, sqlConn, sqlTrans)
            sqlCmdInsert.CommandType = CommandType.StoredProcedure

            sqlCmdInsert.Parameters.Add("@IDCONTATORE", SqlDbType.Int, 4, "IDCONTATORE")
            sqlCmdInsert.Parameters("@IDCONTATORE").Value = CType(strIDDOCOLD, Integer)

            sqlCmdInsert.ExecuteNonQuery()

            setTP_CONTATORI_STO = True

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DBManager.setTP_CONTATORI_STO.errore: ", ex)
            setTP_CONTATORI_STO = False
            'scrittura nel file di log
        End Try

    End Function
#End Region



End Class
