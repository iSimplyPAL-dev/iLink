Imports log4net
Imports System.Web.HttpContext
Imports OPENUtility
Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Data.Odbc
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports AnagInterface
Imports Utility

Public Class ClsPagamenti
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ClsPagamenti))
    Private iDB As New DBAccess.getDBobject
    Private oReplace As New ClsGenerale.Generale
    Private cmdMyCommand As New SqlClient.SqlCommand
    Private FncGen As New ClsGenerale.Generale

    Public Class TypeStampa
        Public Shared Pagamenti As Integer = 0
        Public Shared Insoluti As Integer = 1
        Public Shared Rimborsi As Integer = 2
        Public Shared Riversamento As Integer = 3
    End Class

    'Public Function GetStampaPagamenti(ByVal sParametroENV As String, ByVal sUserName As String, ByVal sIdentificativoApplicazione As String, ByVal sIdEnte As String, ByVal oMyPagamentiForSearch As OggettoPagamento) As DataView
    '    Dim WFErrore As String
    '    Dim WFSession As CreateSessione
    '    Dim dvMyDati As New DataView

    '    Try
    '        'inizializzo la connessione
    '        WFSession = New CreateSessione(sParametroENV, sUserName, sIdentificativoApplicazione)
    '        If Not WFSession.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        cmdMyCommand.Connection = WFSession.oSession.oAppDB.GetConnection
    '        'valorizzo il CommandText;
    '        cmdMyCommand.CommandText = "SELECT ID, COD_UTENTE, COGNOME, NOME, CFPIVA, DATA_FATTURA, NUMERO_FATTURA, IMPEMESSO, DATA_ACCREDITO, DATA_PAGAMENTO, PROVENIENZA, COD_CAPITOLO, IDVOCE, SUM(IMPDET) AS IMPORTO"
    '        cmdMyCommand.CommandText += " FROM OPENGOV_FATTUREPAGAMENTI"
    '        cmdMyCommand.CommandText += " WHERE (IDENTE=@IDENTE)"
    '        If oMyPagamentiForSearch.sDataFattura <> "" Then
    '            cmdMyCommand.CommandText += " AND (DATA_FATTURA=@DATAFATTURA)"
    '        End If
    '        If oMyPagamentiForSearch.sNumeroFattura <> "" Then
    '            cmdMyCommand.CommandText += " AND (NUMERO_FATTURA=@NFATTURA)"
    '        End If
    '        If oMyPagamentiForSearch.sCognome <> "" Then
    '            cmdMyCommand.CommandText += " AND (COGNOME LIKE @COGNOME)"
    '        End If
    '        If oMyPagamentiForSearch.sNome <> "" Then
    '            cmdMyCommand.CommandText += " AND (NOME LIKE @NOME)"
    '        End If
    '        If oMyPagamentiForSearch.sCodFiscalePIva <> "" Then
    '            cmdMyCommand.CommandText += " AND (CFPIVA LIKE @CFPIVA)"
    '        End If
    '        If oMyPagamentiForSearch.sAnnoEmissioneFattura <> "" Then
    '            cmdMyCommand.CommandText += " AND (ANNO_FATTURA LIKE @ANNOFATTURA)"
    '        End If
    '        '*** 02122008 Fabi Modifica per data_accredito
    '        If oMyPagamentiForSearch.sDataAccredito <> "" Then
    '            cmdMyCommand.CommandText += " AND (DATA_ACCREDITO >=@ACCREDITODAL)"
    '        End If
    '        If oMyPagamentiForSearch.sDataAccreditoAl <> "" Then
    '            cmdMyCommand.CommandText += " AND (DATA_ACCREDITO <=@ACCREDITOAL)"
    '        End If
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAFATTURA", SqlDbType.NVarChar)).Value = oMyPagamentiForSearch.sDataFattura
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NFATTURA", SqlDbType.NVarChar)).Value = oMyPagamentiForSearch.sNumeroFattura
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COGNOME", SqlDbType.NVarChar)).Value = oMyPagamentiForSearch.sCognome & "%"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOME", SqlDbType.NVarChar)).Value = oMyPagamentiForSearch.sNome & "%"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CFPIVA", SqlDbType.NVarChar)).Value = oReplace.ReplaceCharsForSearch(oMyPagamentiForSearch.sCodFiscalePIva) & "%"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNOFATTURA", SqlDbType.NVarChar)).Value = oMyPagamentiForSearch.sAnnoEmissioneFattura
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ACCREDITODAL", SqlDbType.NVarChar)).Value = oMyPagamentiForSearch.sDataAccredito
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ACCREDITOAL", SqlDbType.NVarChar)).Value = oMyPagamentiForSearch.sDataAccreditoAl
    '        If oMyPagamentiForSearch.nPeriodo > 0 Then
    '            cmdMyCommand.CommandText += " AND (IDPERIODO=@IDPERIODO)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPERIODO", SqlDbType.Int)).Value = oMyPagamentiForSearch.nPeriodo
    '        End If
    '        cmdMyCommand.CommandText += " GROUP BY ID, COD_UTENTE, COGNOME, NOME, CFPIVA, DATA_FATTURA, NUMERO_FATTURA, IMPEMESSO, DATA_ACCREDITO, DATA_PAGAMENTO, PROVENIENZA, COD_CAPITOLO, IDVOCE"
    '        cmdMyCommand.CommandText += " ORDER BY COGNOME, NOME, CFPIVA, DATA_FATTURA, NUMERO_FATTURA,  DATA_ACCREDITO, DATA_PAGAMENTO, PROVENIENZA, ID, COD_CAPITOLO, IDVOCE"
    '        dvMyDati = WFSession.oSession.oAppDB.GetPrivateDataview(cmdMyCommand)
    '        Return dvMyDati
    '    Catch Err As Exception
    '        Dim sValParametri As String = FncGen.GetValParamCmd(cmdMyCommand)
    '  Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsPagamenti.GetStampaPagamenti.errore: ", Err) 
    '        Log.Debug("Si è verificato un errore in ClsPagamenti::GetStampaPagamenti::" & Err.Message )
    '        
    '        Return Nothing
    '    End Try
    'End Function

    'Public Function GetRiversamento(ByVal sParametroENV As String, ByVal sUserName As String, ByVal sIdentificativoApplicazione As String, ByVal sIdEnte As String, ByVal oMyPagamentiForSearch As OggettoPagamento) As DataView
    '    Dim WFErrore As String
    '    Dim WFSession As CreateSessione
    '    Dim dvMyDati As DataView

    '    Try
    '        'inizializzo la connessione
    '        WFSession = New CreateSessione(sParametroENV, sUserName, sIdentificativoApplicazione)
    '        If Not WFSession.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        cmdMyCommand.Connection = WFSession.oSession.oAppDB.GetConnection
    '        'valorizzo il CommandText;
    '        cmdMyCommand.CommandText = "SELECT ANNO_FATTURA, DATA_ACCREDITO, COD_CAPITOLO, IDVOCE, SUM(IMPDET) AS IMPORTO"
    '        cmdMyCommand.CommandText += " FROM OPENGOV_FATTUREPAGAMENTI"
    '        cmdMyCommand.CommandText += " WHERE (IDENTE=@IDENTE)"
    '        If oMyPagamentiForSearch.sDataFattura <> "" Then
    '            cmdMyCommand.CommandText += " AND (DATA_FATTURA=@DATAFATTURA)"
    '        End If
    '        If oMyPagamentiForSearch.sNumeroFattura <> "" Then
    '            cmdMyCommand.CommandText += " AND (NUMERO_FATTURA=@NFATTURA)"
    '        End If
    '        If oMyPagamentiForSearch.sCognome <> "" Then
    '            cmdMyCommand.CommandText += " AND (COGNOME LIKE @COGNOME)"
    '        End If
    '        If oMyPagamentiForSearch.sNome <> "" Then
    '            cmdMyCommand.CommandText += " AND (NOME LIKE @NOME)"
    '        End If
    '        If oMyPagamentiForSearch.sCodFiscalePIva <> "" Then
    '            cmdMyCommand.CommandText += " AND (CFPIVA LIKE @CFPIVA)"
    '        End If
    '        If oMyPagamentiForSearch.sAnnoEmissioneFattura <> "" Then
    '            cmdMyCommand.CommandText += " AND (ANNO_FATTURA LIKE @ANNOFATTURA)"
    '        End If
    '        '*** 02122008 Fabi Modifica per data_accredito
    '        If oMyPagamentiForSearch.sDataAccredito <> "" Then
    '            cmdMyCommand.CommandText += " AND (DATA_ACCREDITO >=@ACCREDITODAL)"
    '        End If
    '        If oMyPagamentiForSearch.sDataAccreditoAl <> "" Then
    '            cmdMyCommand.CommandText += " AND (DATA_ACCREDITO <=@ACCREDITOAL)"
    '        End If
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAFATTURA", SqlDbType.NVarChar)).Value = oMyPagamentiForSearch.sDataFattura
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NFATTURA", SqlDbType.NVarChar)).Value = oMyPagamentiForSearch.sNumeroFattura
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COGNOME", SqlDbType.NVarChar)).Value = oMyPagamentiForSearch.sCognome & "%"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOME", SqlDbType.NVarChar)).Value = oMyPagamentiForSearch.sNome & "%"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CFPIVA", SqlDbType.NVarChar)).Value = oReplace.ReplaceCharsForSearch(oMyPagamentiForSearch.sCodFiscalePIva) & "%"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNOFATTURA", SqlDbType.NVarChar)).Value = oMyPagamentiForSearch.sAnnoEmissioneFattura
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ACCREDITODAL", SqlDbType.NVarChar)).Value = oMyPagamentiForSearch.sDataAccredito
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ACCREDITOAL", SqlDbType.NVarChar)).Value = oMyPagamentiForSearch.sDataAccreditoAl
    '        If oMyPagamentiForSearch.nPeriodo > 0 Then
    '            cmdMyCommand.CommandText += " AND (IDPERIODO=@IDPERIODO)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPERIODO", SqlDbType.Int)).Value = oMyPagamentiForSearch.nPeriodo
    '        End If
    '        cmdMyCommand.CommandText += " GROUP BY ANNO_FATTURA, DATA_ACCREDITO, COD_CAPITOLO, IDVOCE"
    '        cmdMyCommand.CommandText += " ORDER BY ANNO_FATTURA, DATA_ACCREDITO, COD_CAPITOLO, IDVOCE"
    '        dvMyDati = WFSession.oSession.oAppDB.GetPrivateDataview(cmdMyCommand)
    '        Return dvMyDati
    '    Catch Err As Exception
    '        Dim sValParametri As String = FncGen.GetValParamCmd(cmdMyCommand)
    ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsPagamenti.GetRiversamento.errore: ", Err) 
    '        Log.Debug("Si è verificato un errore in ClsPagamenti::GetRiversamento::" & Err.Message )
    '        
    '        Return Nothing
    '    End Try
    'End Function

    'Public Function GetFatturaPagamenti(ByVal objFatturaPagamenti As OggettoPagamento) As OggettoPagamento()
    '    Dim culture As IFormatProvider
    '    culture = New System.Globalization.CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")
    '    Dim WFSessione As CreateSessione
    '    Dim DrDati As SqlClient.SqlDataReader
    '    Try
    '        Dim sSQL, WFErrore As String
    '        Dim oFatturaPagamenti As OggettoPagamento
    '        Dim oListFatturaPagamenti() As OggettoPagamento
    '        Dim nList As Integer = -1
    '        Dim oMyAnagrafe As DettaglioAnagrafica

    '        'inizializzo la connessione
    '        WFSessione = New CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If
    '        sSQL = "SELECT TP_FATTURE_NOTE.*, TP_PAGAMENTI.ID, TP_PAGAMENTI.DATA_PAGAMENTO, TP_PAGAMENTI.IMPORTO_PAGAMENTO, TP_PAGAMENTI.OPERATORE, TP_PAGAMENTI.NUMERO_RATA, TP_PAGAMENTI.DATA_ACCREDITO"
    '        sSQL += " FROM TP_FATTURE_NOTE"
    '        sSQL += " LEFT JOIN TP_PAGAMENTI ON TP_FATTURE_NOTE.IDENTE=TP_PAGAMENTI.IDENTE"
    '        sSQL += " AND TP_FATTURE_NOTE.NUMERO_FATTURA=TP_PAGAMENTI.NUMERO_FATTURA AND TP_FATTURE_NOTE.DATA_FATTURA=TP_PAGAMENTI.DATA_FATTURA"
    '        sSQL += " WHERE (TP_FATTURE_NOTE.DATA_VARIAZIONE IS NULL) AND TP_FATTURE_NOTE.IDENTE='" & ConstSession.IdEnte & "'"
    '        If objFatturaPagamenti.ID <> -1 Then
    '            sSQL += " AND (TP_PAGAMENTI.ID = " & objFatturaPagamenti.ID & ")"
    '        End If
    '        If objFatturaPagamenti.sAnnoEmissioneFattura <> "" Then
    '            sSQL += " AND (TP_FATTURE_NOTE.ANNO_RIFERIMENTO = '" & objFatturaPagamenti.sAnnoEmissioneFattura & "')"
    '        End If
    '        If objFatturaPagamenti.sNumeroFattura <> "" Then
    '            sSQL += " AND (TP_FATTURE_NOTE.NUMERO_FATTURA = '" & objFatturaPagamenti.sNumeroFattura & "')"
    '        End If
    '        If objFatturaPagamenti.sDataFattura <> "" Then
    '            sSQL += " AND (TP_FATTURE_NOTE.DATA_FATTURA = '" & objFatturaPagamenti.sDataFattura & "')"
    '        End If
    '        If objFatturaPagamenti.ImportoEmesso <> CDbl(0) Then
    '            sSQL += " AND (TP_FATTURE_NOTE.IMPORTO_FATTURANOTA = " & objFatturaPagamenti.ImportoEmesso.ToString().Replace(",", ".") & ")"
    '        End If
    '        If objFatturaPagamenti.sCognome.CompareTo("") <> 0 Then
    '            sSQL += " AND TP_FATTURE_NOTE.COGNOME_DENOMINAZIONE LIKE '" & oReplace.ReplaceCharsForSearch(objFatturaPagamenti.sCognome) & "%'"
    '        End If
    '        If objFatturaPagamenti.sNome.CompareTo("") <> 0 Then
    '            sSQL += " AND TP_FATTURE_NOTE.NOME LIKE '" & oReplace.ReplaceCharsForSearch(objFatturaPagamenti.sNome) & "%'"
    '        End If
    '        sSQL += " AND (TP_FATTURE_NOTE.IMPORTO_FATTURANOTA > 0) AND (TP_FATTURE_NOTE.NUMERO_FATTURA<>'' AND NOT TP_FATTURE_NOTE.NUMERO_FATTURA IS NULL)"

    '        'eseguo la query
    '        DrDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '        Do While DrDati.Read
    '            oFatturaPagamenti = New OggettoPagamento
    '            If Not IsDBNull(DrDati("id")) Then
    '                oFatturaPagamenti.ID = CInt(DrDati("id"))
    '            End If
    '            oFatturaPagamenti.IDEnte = CStr(DrDati("idente"))
    '            oFatturaPagamenti.IdFatturaNota = CInt(DrDati("idfatturanota"))
    '            oFatturaPagamenti.sAnnoEmissioneFattura = CStr(DrDati("anno_riferimento"))
    '            oFatturaPagamenti.nCodUtente = CInt(DrDati("cod_utente"))
    '            oFatturaPagamenti.sVia = CStr(DrDati("via_res"))
    '            oFatturaPagamenti.sCivico = CStr(DrDati("civico_res"))
    '            oFatturaPagamenti.sInterno = CStr(DrDati("interno_res"))
    '            oFatturaPagamenti.ImportoEmesso = CDbl(DrDati("importo_fatturanota"))
    '            oFatturaPagamenti.sNome = CStr(DrDati("NOME"))
    '            oFatturaPagamenti.sCognome = CStr(DrDati("COGNOME_DENOMINAZIONE"))
    '            oFatturaPagamenti.sNumeroFattura = CStr(DrDati("NUMERO_FATTURA"))
    '            If CStr(DrDati("PARTITA_IVA")) <> "" Then
    '                oFatturaPagamenti.sCodFiscalePIva = CStr(DrDati("PARTITA_IVA"))
    '            Else
    '                oFatturaPagamenti.sCodFiscalePIva = CStr(DrDati("cod_fiscale"))
    '            End If
    '            If Not IsDBNull(DrDati("Data_Pagamento")) Then
    '                If CStr(DrDati("Data_Pagamento")) <> "" Then
    '                    oFatturaPagamenti.sDataPagamento = CStr(DrDati("Data_Pagamento"))
    '                End If
    '            End If
    '            If Not IsDBNull(DrDati("data_fattura")) Then
    '                If CStr(DrDati("data_fattura")) <> "" Then
    '                    oFatturaPagamenti.sDataFattura = CStr(DrDati("data_fattura"))
    '                End If
    '            End If
    '            If Not IsDBNull(DrDati("Importo_Pagamento")) Then
    '                oFatturaPagamenti.ImportoPagamento = CDbl(DrDati("Importo_Pagamento"))
    '            End If
    '            If Not IsDBNull(DrDati("OPERATORE")) Then
    '                oFatturaPagamenti.sOperatore = CStr(DrDati("OPERATORE"))
    '            End If
    '            If Not IsDBNull(DrDati("NUMERO_RATA")) Then
    '                oFatturaPagamenti.sNRata = CStr(DrDati("NUMERO_RATA"))
    '            End If

    '            If Not IsDBNull(DrDati("data_accredito")) Then
    '                If CStr(DrDati("data_accredito")) <> "" Then
    '                    oFatturaPagamenti.sDataAccredito = CStr(DrDati("data_accredito"))
    '                End If
    '            End If

    '            'ridimensiono l'array
    '            nList += 1
    '            ReDim Preserve oListFatturaPagamenti(nList)
    '            oListFatturaPagamenti(nList) = oFatturaPagamenti
    '        Loop
    '        DrDati.Close()
    '        Return oListFatturaPagamenti
    '    Catch Err As Exception
    '         Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsPagamenti.GetFatturaPagamenti.errore: ", Err) 
    '        Return Nothing
    '    Finally

    '        WFSessione.Kill()
    '    End Try
    'End Function

    'Public Function GetRimborsi(ByVal sParametroENV As String, ByVal sUserName As String, ByVal sIdentificativoApplicazione As String, ByVal sIdEnte As String, ByVal oMyPagamentiForSearch As OggettoPagamento) As DataView
    '    dim sSQL as string = ""
    '    Dim WFErrore As String = ""
    '    Dim WFSessione As CreateSessione
    '    Dim dvMyDati As New DataView

    '    Try
    '        '*** QUERY SBAGLIATA ***
    '        'sSQL = "SELECT TP_FATTURE_NOTE.NUMEROUTENTE, TP_FATTURE_NOTE.COD_UTENTE, TP_FATTURE_NOTE.IMPORTO_FATTURANOTA, SUM(TP_PAGAMENTI.IMPORTO_PAGAMENTO) AS IMPORTO_PAGATO,TP_FATTURE_NOTE.IDENTE,TP_FATTURE_NOTE.DATA_VARIAZIONE,"
    '        'sSQL += NomeDbAnag & ".ANAGRAFICA.DATA_FINE_VALIDITA, " & NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.COGNOME_INVIO, " & NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.NOME_INVIO, " & NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.VIA_RCP, "
    '        'sSQL += NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.CIVICO_RCP, " & NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.INTERNO_CIVICO_RCP, " & NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.COMUNE_RCP, "
    '        'sSQL += NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.PROVINCIA_RCP, " & NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.CAP_RCP, " & NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.DATA_FINE_VALIDITA,"
    '        'sSQL += NomeDbAnag & ".ANAGRAFICA.DATA_FINE_VALIDITA, " & NomeDbAnag & ".ANAGRAFICA.COGNOME_DENOMINAZIONE, " & NomeDbAnag & ".ANAGRAFICA.NOME, " & NomeDbAnag & ".ANAGRAFICA.COD_FISCALE, "
    '        'sSQL += NomeDbAnag & ".ANAGRAFICA.PARTITA_IVA, " & NomeDbAnag & ".ANAGRAFICA.VIA_RES, " & NomeDbAnag & ".ANAGRAFICA.CIVICO_RES, " & NomeDbAnag & ".ANAGRAFICA.INTERNO_CIVICO_RES, "
    '        'sSQL += NomeDbAnag & ".ANAGRAFICA.COMUNE_RES, " & NomeDbAnag & ".ANAGRAFICA.PROVINCIA_RES, " & NomeDbAnag & ".ANAGRAFICA.CAP_RES"
    '        'sSQL += " FROM TP_FATTURE_NOTE INNER JOIN TP_PAGAMENTI ON TP_FATTURE_NOTE.DATA_FATTURA = TP_PAGAMENTI.DATA_FATTURA AND TP_FATTURE_NOTE.NUMERO_FATTURA = TP_PAGAMENTI.NUMERO_FATTURA"
    '        'sSQL += " INNER JOIN " & NomeDbAnag & ".ANAGRAFICA ON TP_FATTURE_NOTE.COD_UTENTE= " & NomeDbAnag & ".ANAGRAFICA.COD_CONTRIBUENTE lEFT JOIN " & NomeDbAnag & ".INDIRIZZI_SPEDIZIONE ON " & NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.COD_CONTRIBUENTE= " & NomeDbAnag & ".ANAGRAFICA.COD_CONTRIBUENTE"
    '        'sSQL += " WHERE (IDENTE='" & oMyPagamentiForSearch.IDEnte & "')"
    '        'If oMyPagamentiForSearch.sDataFattura <> "" Then
    '        '    sSQL += " AND (DATA_FATTURA='" & oMyPagamentiForSearch.sDataFattura & "')"
    '        'End If
    '        'If oMyPagamentiForSearch.sNumeroFattura <> "" Then
    '        '    sSQL += " AND (NUMERO_FATTURA='" & oMyPagamentiForSearch.sNumeroFattura & "')"
    '        'End If
    '        'If oMyPagamentiForSearch.sCognome <> "" Then
    '        '    sSQL += " AND (COGNOME LIKE '" & oMyPagamentiForSearch.sCognome & "%')"
    '        'End If
    '        'If oMyPagamentiForSearch.sNome <> "" Then
    '        '    sSQL += " AND (NOME LIKE'" & oMyPagamentiForSearch.sNome & "%')"
    '        'End If
    '        'If oMyPagamentiForSearch.sCodFiscalePIva <> "" Then
    '        '    sSQL += " AND (CFPIVA LIKE '" & oReplace.ReplaceCharsForSearch(oMyPagamentiForSearch.sCodFiscalePIva) & "%')"
    '        'End If
    '        ''*** 02122008 Fabi Modifica per data_accredito
    '        'If oMyPagamentiForSearch.sDataAccredito <> "" Then
    '        '    sSQL += " AND (DATA_ACCREDITO >='" & oMyPagamentiForSearch.sDataAccredito & "')"
    '        'End If
    '        'If oMyPagamentiForSearch.sDataAccreditoAl <> "" Then
    '        '    sSQL += " AND (DATA_ACCREDITO <='" & oMyPagamentiForSearch.sDataAccreditoAl & "')"
    '        'End If
    '        'sSQL += " GROUP BY TP_FATTURE_NOTE.NUMEROUTENTE, TP_FATTURE_NOTE.COD_UTENTE, TP_FATTURE_NOTE.IMPORTO_FATTURANOTA,TP_FATTURE_NOTE.IDENTE,TP_FATTURE_NOTE.DATA_VARIAZIONE," & NomeDbAnag & ".ANAGRAFICA.DATA_FINE_VALIDITA,"
    '        'sSQL += NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.COGNOME_INVIO, " & NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.NOME_INVIO, " & NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.VIA_RCP, "
    '        'sSQL += NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.CIVICO_RCP, " & NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.INTERNO_CIVICO_RCP, " & NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.COMUNE_RCP, "
    '        'sSQL += NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.PROVINCIA_RCP, " & NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.CAP_RCP, " & NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.DATA_FINE_VALIDITA, " & NomeDbAnag & ".ANAGRAFICA.DATA_FINE_VALIDITA, " & NomeDbAnag & ".ANAGRAFICA.COGNOME_DENOMINAZIONE, "
    '        'sSQL += NomeDbAnag & ".ANAGRAFICA.NOME, " & NomeDbAnag & ".ANAGRAFICA.COD_FISCALE, " & NomeDbAnag & ".ANAGRAFICA.PARTITA_IVA, " & NomeDbAnag & ".ANAGRAFICA.VIA_RES, "
    '        'sSQL += NomeDbAnag & ".ANAGRAFICA.CIVICO_RES, " & NomeDbAnag & ".ANAGRAFICA.INTERNO_CIVICO_RES, " & NomeDbAnag & ".ANAGRAFICA.COMUNE_RES, " & NomeDbAnag & ".ANAGRAFICA.PROVINCIA_RES, "
    '        'sSQL += NomeDbAnag & ".ANAGRAFICA.CAP_RES"
    '        'sSQL += " HAVING (" & NomeDbAnag & ".ANAGRAFICA.DATA_FINE_VALIDITA IS NULL) AND (" & NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.DATA_FINE_VALIDITA IS NULL)"
    '        'sSQL += " AND TP_FATTURE_NOTE.DATA_VARIAZIONE IS NULL"
    '        'sSQL += " AND TP_FATTURE_NOTE.IMPORTO_FATTURANOTA < SUM(TP_PAGAMENTI.IMPORTO_PAGAMENTO)"
    '        'sSQL += " GROUP BY COD_UTENTE, COGNOME, NOME, CFPIVA, DATA_FATTURA, NUMERO_FATTURA, IMPEMESSO, DATA_ACCREDITO, DATA_PAGAMENTO, PROVENIENZA, COD_CAPITOLO, IDVOCE"
    '        'sSQL += " ORDER BY COGNOME, NOME, CFPIVA, DATA_FATTURA, NUMERO_FATTURA,  DATA_ACCREDITO, DATA_PAGAMENTO, PROVENIENZA, COD_CAPITOLO, IDVOCE"
    '        '*** ***
    '        sSQL = "SELECT NUMEROUTENTE"
    '        sSQL += " , COGNOME, NOME, CFPIVA"
    '        sSQL += " , VIA,CIVICO,ESPONENTE_CIVICO,CAP,COMUNE, PROVINCIA"
    '        sSQL += " , DATA_FATTURA, NUMERO_FATTURA, IMPEMESSO, IMPPAG AS PAGATO"
    '        sSQL += " , IMPEMESSO-IMPPAG AS DIF"
    '        sSQL += " FROM OPENGOV_FATTUREPAGAMENTIPERRIMBORSI"
    '        sSQL += " WHERE IMPEMESSO<IMPPAG"
    '        sSQL += " AND (IDENTE='" & oMyPagamentiForSearch.IDEnte & "')"
    '        If oMyPagamentiForSearch.sDataFattura <> "" Then
    '            sSQL += " AND (DATA_FATTURA='" & oMyPagamentiForSearch.sDataFattura & "')"
    '        End If
    '        If oMyPagamentiForSearch.sCognome <> "" Then
    '            sSQL += " AND (COGNOME LIKE '" & oMyPagamentiForSearch.sCognome & "%')"
    '        End If
    '        If oMyPagamentiForSearch.sNome <> "" Then
    '            sSQL += " AND (NOME LIKE'" & oMyPagamentiForSearch.sNome & "%')"
    '        End If
    '        If oMyPagamentiForSearch.sCodFiscalePIva <> "" Then
    '            sSQL += " AND (CFPIVA LIKE '" & oReplace.ReplaceCharsForSearch(oMyPagamentiForSearch.sCodFiscalePIva) & "%')"
    '        End If
    '        If oMyPagamentiForSearch.sAnnoEmissioneFattura <> "" Then
    '            sSQL += " AND (YEAR(DATA_FATTURA)=" & oMyPagamentiForSearch.sAnnoEmissioneFattura & ")"
    '        End If
    '        If oMyPagamentiForSearch.nPeriodo > 0 Then
    '            sSQL += " AND (IDPERIODO=" & oMyPagamentiForSearch.nPeriodo & ")"
    '        End If
    '        If oMyPagamentiForSearch.sNumeroFattura <> "" Then
    '            sSQL += " AND (NUMERO_FATTURA='" & oMyPagamentiForSearch.sNumeroFattura & "')"
    '        End If
    '        sSQL += " ORDER BY COGNOME, NOME, CFPIVA"
    '        sSQL += " , DATA_FATTURA, NUMERO_FATTURA"
    '        WFSessione = New CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataview(sSQL)
    '        Return dvMyDati

    '    Catch ex As Exception
    '  Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsPagamenti.GetRimborsi.errore: ", ex) 
    '        Throw New Exception("Errori nella stampa ClsPagamenti::GetRimborsi:: " + ex.Message)
    '    Finally
    '        WFSessione.oSession.oAppDB.DisposeConnection()
    '        WFSessione.oSession.oAppDB.Dispose()
    '        WFSessione.oSession.SecDB.Dispose()
    '        WFSessione.oSession.SecDB.DisposeConnection()

    '        WFSessione.Kill()

    '    End Try
    'End Function

    'Public Function GetInsoluti(ByVal sParametroENV As String, ByVal sUserName As String, ByVal sIdentificativoApplicazione As String, ByVal sIdEnte As String, ByVal oMyPagamentiForSearch As OggettoPagamento) As DataView
    '    dim sSQL as string = ""
    '    Dim WFErrore As String = ""
    '    Dim WFSessione As CreateSessione
    '    Dim dvMyDati As New DataView

    '    Try
    '        sSQL = "SELECT NUMEROUTENTE"
    '        sSQL += " , COGNOME, NOME, CFPIVA"
    '        sSQL += " , VIA,CIVICO,ESPONENTE_CIVICO,CAP,COMUNE, PROVINCIA"
    '        sSQL += " , DATA_FATTURA, NUMERO_FATTURA, IMPEMESSO, IMPPAG AS PAGATO"
    '        sSQL += " , IMPEMESSO-IMPPAG AS DIF"
    '        sSQL += " FROM OPENGOV_FATTUREPAGAMENTIPERINSOLUTI"
    '        sSQL += " WHERE IMPEMESSO>IMPPAG"
    '        sSQL += " AND (IDENTE='" & oMyPagamentiForSearch.IDEnte & "')"
    '        If oMyPagamentiForSearch.sDataFattura <> "" Then
    '            sSQL += " AND (DATA_FATTURA='" & oMyPagamentiForSearch.sDataFattura & "')"
    '        End If
    '        If oMyPagamentiForSearch.sCognome <> "" Then
    '            sSQL += " AND (COGNOME LIKE '" & oMyPagamentiForSearch.sCognome & "%')"
    '        End If
    '        If oMyPagamentiForSearch.sNome <> "" Then
    '            sSQL += " AND (NOME LIKE'" & oMyPagamentiForSearch.sNome & "%')"
    '        End If
    '        If oMyPagamentiForSearch.sCodFiscalePIva <> "" Then
    '            sSQL += " AND (CFPIVA LIKE '" & oReplace.ReplaceCharsForSearch(oMyPagamentiForSearch.sCodFiscalePIva) & "%')"
    '        End If
    '        If oMyPagamentiForSearch.sAnnoEmissioneFattura <> "" Then
    '            sSQL += " AND (YEAR(DATA_FATTURA)=" & oMyPagamentiForSearch.sAnnoEmissioneFattura & ")"
    '        End If
    '        If oMyPagamentiForSearch.nPeriodo > 0 Then
    '            sSQL += " AND (IDPERIODO=" & oMyPagamentiForSearch.nPeriodo & ")"
    '        End If
    '        If oMyPagamentiForSearch.sNumeroFattura <> "" Then
    '            sSQL += " AND (NUMERO_FATTURA='" & oMyPagamentiForSearch.sNumeroFattura & "')"
    '        End If
    '        sSQL += " ORDER BY COGNOME, NOME, CFPIVA"
    '        sSQL += " , DATA_FATTURA, NUMERO_FATTURA"
    '        WFSessione = New CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataview(sSQL)
    '        Return dvMyDati

    '    Catch ex As Exception
    ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsPagamenti.GetInsoluti.errore: ", ex) 
    '        Throw New Exception("Errori nella stampa ClsPagamenti::GetInsoluti:: " + ex.Message)
    '    Finally
    '        WFSessione.oSession.oAppDB.DisposeConnection()
    '        WFSessione.oSession.oAppDB.Dispose()
    '        WFSessione.oSession.SecDB.Dispose()
    '        WFSessione.oSession.SecDB.DisposeConnection()
    '        WFSessione.Kill()
    '    End Try
    'End Function

    'Public Function SetPagamento(ByVal oMyPagamento As OggettoPagamento, ByVal nDBOperation As Integer, ByVal WFSession As CreateSessione) As Integer
    '    '{0= non a buon fine, >0= id tabella}
    '    Dim nMyReturn As Integer

    '    Try
    '        'valorizzo il CommandText:
    '        Select Case nDBOperation
    '            Case 0
    '                cmdMyCommand.CommandText = "INSERT INTO TP_PAGAMENTI (IDIMPORTAZIONE , IDFLUSSO, IDENTE, IDCONTRIBUENTE, PROVENIENZA,"
    '                cmdMyCommand.CommandText += " DATA_FATTURA, NUMERO_FATTURA, CODICE_BOLLETTINO, NUMERO_RATA,"
    '                cmdMyCommand.CommandText += " DATA_PAGAMENTO, DATA_ACCREDITO, DIVISA, SEGNO, IMPORTO_PAGAMENTO,"
    '                cmdMyCommand.CommandText += " TIPO_BOLLETTINO, TIPO_PAGAMENTO,"
    '                cmdMyCommand.CommandText += " PROGRESSIVO_CARICAMENTO, PROGRESSIVO_SELEZIONE, CCBENEFICIARIO, UFFICIOSPORTELLO,"
    '                cmdMyCommand.CommandText += " NOTE, FLAG_DETTAGLIO, OPERATORE, DATA_INSERIMENTO)"
    '                cmdMyCommand.CommandText += " VALUES (@IDIMPORTAZIONE , @IDFLUSSO, @IDENTE, @IDCONTRIBUENTE, @PROVENIENZA,"
    '                cmdMyCommand.CommandText += " @DATAFATTURA, @NUMERO_FATTURA, @CODICE_BOLLETTINO, @NUMERO_RATA,"
    '                cmdMyCommand.CommandText += " @DATA_PAGAMENTO, @DATA_ACCREDITO, @DIVISA, @SEGNO, @IMPORTO_PAGAMENTO,"
    '                cmdMyCommand.CommandText += " @TIPO_BOLLETTINO, @TIPO_PAGAMENTO,"
    '                cmdMyCommand.CommandText += " @PROGRESSIVO_CARICAMENTO, @PROGRESSIVO_SELEZIONE, @CCBENEFICIARIO, @UFFICIOSPORTELLO,"
    '                cmdMyCommand.CommandText += " @NOTE, @FLAGDETTAGLIO, @OPERATORE, @DATA_INSERIMENTO)"
    '                cmdMyCommand.CommandText += " SELECT @@IDENTITY"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPAGAMENTO", SqlDbType.Int)).Value = oMyPagamento.ID
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDIMPORTAZIONE", SqlDbType.Int)).Value = oMyPagamento.IDImportazione
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSO", SqlDbType.Int)).Value = oMyPagamento.IDFlusso
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oMyPagamento.IDEnte
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = oMyPagamento.nCodUtente
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PROVENIENZA", SqlDbType.NVarChar)).Value = oMyPagamento.sProvenienza
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAFATTURA", SqlDbType.NVarChar)).Value = oReplace.GiraData(oMyPagamento.sDataFattura)
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO_FATTURA", SqlDbType.NVarChar)).Value = oMyPagamento.sNumeroFattura
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE_BOLLETTINO", SqlDbType.NVarChar)).Value = oMyPagamento.sCodBollettino
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO_RATA", SqlDbType.NVarChar)).Value = oMyPagamento.sNRata
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_PAGAMENTO", SqlDbType.NVarChar)).Value = oReplace.GiraData(oMyPagamento.sDataPagamento)
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_ACCREDITO", SqlDbType.NVarChar)).Value = oReplace.GiraData(oMyPagamento.sDataAccredito)
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DIVISA", SqlDbType.NVarChar)).Value = oMyPagamento.sDivisa
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SEGNO", SqlDbType.NVarChar)).Value = oMyPagamento.sSegno
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO_PAGAMENTO", SqlDbType.Float)).Value = oMyPagamento.ImportoPagamento
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPO_BOLLETTINO", SqlDbType.NVarChar)).Value = oMyPagamento.sTipoBollettino
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPO_PAGAMENTO", SqlDbType.NVarChar)).Value = oMyPagamento.sTipoPagamento
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PROGRESSIVO_CARICAMENTO", SqlDbType.NVarChar)).Value = oMyPagamento.sProgCaricamento
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PROGRESSIVO_SELEZIONE", SqlDbType.NVarChar)).Value = oMyPagamento.sProgSelezione
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CCBENEFICIARIO", SqlDbType.NVarChar)).Value = oMyPagamento.sCCBeneficiario
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@UFFICIOSPORTELLO", SqlDbType.NVarChar)).Value = oMyPagamento.sUfficioSportello
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOTE", SqlDbType.NVarChar)).Value = oMyPagamento.sNote
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FLAGDETTAGLIO", SqlDbType.Bit)).Value = CInt(oMyPagamento.bFlagDettaglio)
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@OPERATORE", SqlDbType.NVarChar)).Value = oMyPagamento.sOperatore
    '                If oMyPagamento.tDataInsert = Date.MinValue Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INSERIMENTO", SqlDbType.DateTime)).Value = Now
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INSERIMENTO", SqlDbType.DateTime)).Value = oMyPagamento.tDataInsert
    '                End If
    '                'eseguo la query()
    '                Dim DrReturn As SqlClient.SqlDataReader
    '                DrReturn = WFSession.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '                Do While DrReturn.Read
    '                    nMyReturn = DrReturn(0)
    '                Loop
    '                DrReturn.Close()
    '            Case 1
    '                cmdMyCommand.CommandText = "UPDATE TP_PAGAMENTI"
    '                cmdMyCommand.CommandText += " SET DATA_PAGAMENTO=@DATA_PAGAMENTO,"
    '                cmdMyCommand.CommandText += " IMPORTO_PAGAMENTO=@IMPORTO_PAGAMENTO,"
    '                cmdMyCommand.CommandText += " DATA_FATTURA=@DATAFATTURA,"
    '                cmdMyCommand.CommandText += " NUMERO_FATTURA=@NUMERO_FATTURA,"
    '                cmdMyCommand.CommandText += " NUMERO_RATA=@NUMERO_RATA,"
    '                cmdMyCommand.CommandText += " DATA_ACCREDITO =@DATA_ACCREDITO"
    '                cmdMyCommand.CommandText += " WHERE (ID=@IDPAGAMENTO)"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPAGAMENTO", SqlDbType.Int)).Value = oMyPagamento.ID
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAFATTURA", SqlDbType.NVarChar)).Value = oReplace.GiraData(oMyPagamento.sDataFattura)
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO_FATTURA", SqlDbType.NVarChar)).Value = oMyPagamento.sNumeroFattura
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO_RATA", SqlDbType.NVarChar)).Value = oMyPagamento.sNRata
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_PAGAMENTO", SqlDbType.NVarChar)).Value = oReplace.GiraData(oMyPagamento.sDataPagamento)
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_ACCREDITO", SqlDbType.NVarChar)).Value = oReplace.GiraData(oMyPagamento.sDataAccredito)
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO_PAGAMENTO", SqlDbType.Float)).Value = oMyPagamento.ImportoPagamento
    '                nMyReturn = CInt(WFSession.oSession.oAppDB.Execute(cmdMyCommand))
    '            Case 2
    '                cmdMyCommand.CommandText = "DELETE"
    '                cmdMyCommand.CommandText += " FROM TP_PAGAMENTI"
    '                cmdMyCommand.CommandText += " WHERE (IDIMPORTAZIONE=@IDIMPORTAZIONE)"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                nMyReturn = CInt(WFSession.oSession.oAppDB.Execute(cmdMyCommand))
    '            Case 3
    '                cmdMyCommand.CommandText = "UPDATE TP_PAGAMENTI SET FLAG_DETTAGLIO=1"
    '                cmdMyCommand.CommandText += " WHERE (ID=@IDPAGAMENTO)"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPAGAMENTO", SqlDbType.Int)).Value = oMyPagamento.ID
    '                nMyReturn = CInt(WFSession.oSession.oAppDB.Execute(cmdMyCommand))
    '            Case 4
    '                cmdMyCommand.CommandText = "UPDATE TP_PAGAMENTI SET FLAG_DETTAGLIO=0"
    '                cmdMyCommand.CommandText += " WHERE (ID=@IDPAGAMENTO)"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPAGAMENTO", SqlDbType.Int)).Value = oMyPagamento.ID
    '                nMyReturn = CInt(WFSession.oSession.oAppDB.Execute(cmdMyCommand))
    '        End Select
    '        Return nMyReturn
    '    Catch Err As Exception
    '        Dim sValParametri As String = FncGen.GetValParamCmd(cmdMyCommand)
    ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsPagamenti.SetPagamento.errore: ", Err) 
    '        Log.Debug("Si è verificato un errore in ClsPagamenti::SetPagamento::" & Err.Message )
    '       
    '        Return -1
    '    End Try
    'End Function

    'Public Function DeletePagamentiEnte(ByVal OBJ As OggettoPagamento, ByRef strError As String) As Boolean
    '    Dim WFErrore As String
    '    Dim WFSessione As CreateSessione
    '    Dim myIdentity As Integer

    '    Try

    '        DeletePagamentiEnte = False
    '        'inizializzo la connessione
    '        WFSessione = New CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))

    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        Dim SQL As String

    '        'SE NON è STATA ASSOCIATA LA CANCELLO
    '        SQL = "DELETE FROM TP_PAGAMENTI"
    '        SQL += " WHERE IDENTE='" & ConstSession.IdEnte & "' AND ID=" & OBJ.ID & ""

    '        'eseguo la query
    '        myIdentity = CInt(WFSessione.oSession.oAppDB.Execute(SQL))

    '        DeletePagamentiEnte = True
    '    Catch ex As Exception
    ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsPagamenti.DeletePagamentiEnte.errore :  ", ex) 
    '        Throw New Exception("Problemi nell'esecuzione di DeletePagamentiEnte " + ex.Message)
    '        DeletePagamentiEnte = False
    '    Finally
    '        'chiudo la connessione
    '        If Not WFSessione.oSession Is Nothing Then
    '            WFSessione.Kill()
    '        End If
    '    End Try
    'End Function
    '''' <summary>
    '''' 
    '''' </summary>
    '''' <param name="myStringConnection"></param>
    '''' <param name="sIdEnte"></param>
    '''' <param name="oMyPagamentiForSearch"></param>
    '''' <returns></returns>
    '''' <revisionHistory>
    '''' <revision date="02/12/2008">
    '''' Fabi Modifica per data_accredito
    '''' </revision>
    '''' </revisionHistory>
    '''' <revisionHistory>
    '''' <revision date="12/04/2019">
    '''' <strong>Qualificazione AgID-analisi_rel01</strong>
    '''' <em>Esportazione completa dati</em>
    '''' </revision>
    '''' </revisionHistory>
    'Public Function GetStampaPagamenti(myStringConnection As String, ByVal sIdEnte As String, ByVal oMyPagamentiForSearch As OggettoPagamento) As DataView
    '    Dim dvMyDati As New DataView

    '    Try
    '        iDB = New DBAccess.getDBobject(myStringConnection)
    '        'valorizzo il CommandText;
    '        cmdMyCommand.CommandText = "SELECT ID, COD_UTENTE, COGNOME, NOME, CFPIVA, DATA_FATTURA, NUMERO_FATTURA, IMPEMESSO, DATA_ACCREDITO, DATA_PAGAMENTO, PROVENIENZA, COD_CAPITOLO, IDVOCE, SUM(IMPDET) AS IMPORTO"
    '        cmdMyCommand.CommandText += " FROM OPENGOV_FATTUREPAGAMENTI"
    '        cmdMyCommand.CommandText += " WHERE (IDENTE=@IDENTE)"
    '        If oMyPagamentiForSearch.sDataFattura <> "" Then
    '            cmdMyCommand.CommandText += " AND (DATA_FATTURA=@DATAFATTURA)"
    '        End If
    '        If oMyPagamentiForSearch.sNumeroFattura <> "" Then
    '            cmdMyCommand.CommandText += " AND (NUMERO_FATTURA=@NFATTURA)"
    '        End If
    '        If oMyPagamentiForSearch.sCognome <> "" Then
    '            cmdMyCommand.CommandText += " AND (COGNOME LIKE @COGNOME)"
    '        End If
    '        If oMyPagamentiForSearch.sNome <> "" Then
    '            cmdMyCommand.CommandText += " AND (NOME LIKE @NOME)"
    '        End If
    '        If oMyPagamentiForSearch.sCodFiscalePIva <> "" Then
    '            cmdMyCommand.CommandText += " AND (CFPIVA LIKE @CFPIVA)"
    '        End If
    '        If oMyPagamentiForSearch.sAnnoEmissioneFattura <> "" Then
    '            cmdMyCommand.CommandText += " AND (ANNO_FATTURA LIKE @ANNOFATTURA)"
    '        End If
    '        If oMyPagamentiForSearch.sDataAccredito <> "" Then
    '            cmdMyCommand.CommandText += " AND (DATA_ACCREDITO >=@ACCREDITODAL)"
    '        End If
    '        If oMyPagamentiForSearch.sDataAccreditoAl <> "" Then
    '            cmdMyCommand.CommandText += " AND (DATA_ACCREDITO <=@ACCREDITOAL)"
    '        End If
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAFATTURA", SqlDbType.NVarChar)).Value = oMyPagamentiForSearch.sDataFattura
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NFATTURA", SqlDbType.NVarChar)).Value = oMyPagamentiForSearch.sNumeroFattura
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COGNOME", SqlDbType.NVarChar)).Value = oMyPagamentiForSearch.sCognome & "%"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOME", SqlDbType.NVarChar)).Value = oMyPagamentiForSearch.sNome & "%"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CFPIVA", SqlDbType.NVarChar)).Value = oReplace.ReplaceCharsForSearch(oMyPagamentiForSearch.sCodFiscalePIva) & "%"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNOFATTURA", SqlDbType.NVarChar)).Value = oMyPagamentiForSearch.sAnnoEmissioneFattura
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ACCREDITODAL", SqlDbType.NVarChar)).Value = oMyPagamentiForSearch.sDataAccredito
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ACCREDITOAL", SqlDbType.NVarChar)).Value = oMyPagamentiForSearch.sDataAccreditoAl
    '        If oMyPagamentiForSearch.nPeriodo > 0 Then
    '            cmdMyCommand.CommandText += " AND (IDPERIODO=@IDPERIODO)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPERIODO", SqlDbType.Int)).Value = oMyPagamentiForSearch.nPeriodo
    '        End If
    '        cmdMyCommand.CommandText += " GROUP BY ID, COD_UTENTE, COGNOME, NOME, CFPIVA, DATA_FATTURA, NUMERO_FATTURA, IMPEMESSO, DATA_ACCREDITO, DATA_PAGAMENTO, PROVENIENZA, COD_CAPITOLO, IDVOCE"
    '        cmdMyCommand.CommandText += " ORDER BY COGNOME, NOME, CFPIVA, DATA_FATTURA, NUMERO_FATTURA,  DATA_ACCREDITO, DATA_PAGAMENTO, PROVENIENZA, ID, COD_CAPITOLO, IDVOCE"
    '        dvMyDati = iDB.GetDataView(cmdMyCommand)
    '        Return dvMyDati
    '    Catch Err As Exception
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Log.Debug(sIdEnte + " - OPENgovH2O.ClsPagamenti.GetStampaPagamenti.errore: ", Err)
    '        Return Nothing
    '    End Try
    'End Function
    'Public Function GetStampaPagamenti(ByVal sParametroENV As String, ByVal sUserName As String, ByVal sIdentificativoApplicazione As String, ByVal sIdEnte As String, ByVal oMyPagamentiForSearch As OggettoPagamento) As DataView
    '    Dim dvMyDati As New DataView

    '    Try
    '        cmdMyCommand.Connection = New SqlConnection(ConstSession.StringConnection)
    '        'valorizzo il CommandText;
    '        cmdMyCommand.CommandText = "SELECT ID, COD_UTENTE, COGNOME, NOME, CFPIVA, DATA_FATTURA, NUMERO_FATTURA, IMPEMESSO, DATA_ACCREDITO, DATA_PAGAMENTO, PROVENIENZA, COD_CAPITOLO, IDVOCE, SUM(IMPDET) AS IMPORTO"
    '        cmdMyCommand.CommandText += " FROM OPENGOV_FATTUREPAGAMENTI"
    '        cmdMyCommand.CommandText += " WHERE (IDENTE=@IDENTE)"
    '        If oMyPagamentiForSearch.sDataFattura <> "" Then
    '            cmdMyCommand.CommandText += " AND (DATA_FATTURA=@DATAFATTURA)"
    '        End If
    '        If oMyPagamentiForSearch.sNumeroFattura <> "" Then
    '            cmdMyCommand.CommandText += " AND (NUMERO_FATTURA=@NFATTURA)"
    '        End If
    '        If oMyPagamentiForSearch.sCognome <> "" Then
    '            cmdMyCommand.CommandText += " AND (COGNOME LIKE @COGNOME)"
    '        End If
    '        If oMyPagamentiForSearch.sNome <> "" Then
    '            cmdMyCommand.CommandText += " AND (NOME LIKE @NOME)"
    '        End If
    '        If oMyPagamentiForSearch.sCodFiscalePIva <> "" Then
    '            cmdMyCommand.CommandText += " AND (CFPIVA LIKE @CFPIVA)"
    '        End If
    '        If oMyPagamentiForSearch.sAnnoEmissioneFattura <> "" Then
    '            cmdMyCommand.CommandText += " AND (ANNO_FATTURA LIKE @ANNOFATTURA)"
    '        End If
    '        '*** 02122008 Fabi Modifica per data_accredito
    '        If oMyPagamentiForSearch.sDataAccredito <> "" Then
    '            cmdMyCommand.CommandText += " AND (DATA_ACCREDITO >=@ACCREDITODAL)"
    '        End If
    '        If oMyPagamentiForSearch.sDataAccreditoAl <> "" Then
    '            cmdMyCommand.CommandText += " AND (DATA_ACCREDITO <=@ACCREDITOAL)"
    '        End If
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAFATTURA", SqlDbType.NVarChar)).Value = oMyPagamentiForSearch.sDataFattura
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NFATTURA", SqlDbType.NVarChar)).Value = oMyPagamentiForSearch.sNumeroFattura
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COGNOME", SqlDbType.NVarChar)).Value = oMyPagamentiForSearch.sCognome & "%"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOME", SqlDbType.NVarChar)).Value = oMyPagamentiForSearch.sNome & "%"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CFPIVA", SqlDbType.NVarChar)).Value = oReplace.ReplaceCharsForSearch(oMyPagamentiForSearch.sCodFiscalePIva) & "%"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNOFATTURA", SqlDbType.NVarChar)).Value = oMyPagamentiForSearch.sAnnoEmissioneFattura
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ACCREDITODAL", SqlDbType.NVarChar)).Value = oMyPagamentiForSearch.sDataAccredito
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ACCREDITOAL", SqlDbType.NVarChar)).Value = oMyPagamentiForSearch.sDataAccreditoAl
    '        If oMyPagamentiForSearch.nPeriodo > 0 Then
    '            cmdMyCommand.CommandText += " AND (IDPERIODO=@IDPERIODO)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPERIODO", SqlDbType.Int)).Value = oMyPagamentiForSearch.nPeriodo
    '        End If
    '        cmdMyCommand.CommandText += " GROUP BY ID, COD_UTENTE, COGNOME, NOME, CFPIVA, DATA_FATTURA, NUMERO_FATTURA, IMPEMESSO, DATA_ACCREDITO, DATA_PAGAMENTO, PROVENIENZA, COD_CAPITOLO, IDVOCE"
    '        cmdMyCommand.CommandText += " ORDER BY COGNOME, NOME, CFPIVA, DATA_FATTURA, NUMERO_FATTURA,  DATA_ACCREDITO, DATA_PAGAMENTO, PROVENIENZA, ID, COD_CAPITOLO, IDVOCE"
    '        dvMyDati = iDB.GetDataView(cmdMyCommand)
    '        Return dvMyDati
    '    Catch Err As Exception
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsPagamenti.GetStampaPagamenti.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in ClsPagamenti::GetStampaPagamenti::" & Err.Message )

    '        Return Nothing
    '    End Try
    'End Function

    'Public Function GetRiversamento(ByVal sIdEnte As String, ByVal oMyPagamentiForSearch As OggettoPagamento) As DataView
    '    Dim dvMyDati As DataView

    '    Try
    '        cmdMyCommand.Connection = New SqlConnection(ConstSession.StringConnection)
    '        'valorizzo il CommandText;
    '        cmdMyCommand.CommandText = "SELECT ANNO_FATTURA, DATA_ACCREDITO, COD_CAPITOLO, IDVOCE, SUM(IMPDET) AS IMPORTO"
    '        cmdMyCommand.CommandText += " FROM OPENGOV_FATTUREPAGAMENTI"
    '        cmdMyCommand.CommandText += " WHERE (IDENTE=@IDENTE)"
    '        If oMyPagamentiForSearch.sDataFattura <> "" Then
    '            cmdMyCommand.CommandText += " AND (DATA_FATTURA=@DATAFATTURA)"
    '        End If
    '        If oMyPagamentiForSearch.sNumeroFattura <> "" Then
    '            cmdMyCommand.CommandText += " AND (NUMERO_FATTURA=@NFATTURA)"
    '        End If
    '        If oMyPagamentiForSearch.sCognome <> "" Then
    '            cmdMyCommand.CommandText += " AND (COGNOME LIKE @COGNOME)"
    '        End If
    '        If oMyPagamentiForSearch.sNome <> "" Then
    '            cmdMyCommand.CommandText += " AND (NOME LIKE @NOME)"
    '        End If
    '        If oMyPagamentiForSearch.sCodFiscalePIva <> "" Then
    '            cmdMyCommand.CommandText += " AND (CFPIVA LIKE @CFPIVA)"
    '        End If
    '        If oMyPagamentiForSearch.sAnnoEmissioneFattura <> "" Then
    '            cmdMyCommand.CommandText += " AND (ANNO_FATTURA LIKE @ANNOFATTURA)"
    '        End If
    '        '*** 02122008 Fabi Modifica per data_accredito
    '        If oMyPagamentiForSearch.sDataAccredito <> "" Then
    '            cmdMyCommand.CommandText += " AND (DATA_ACCREDITO >=@ACCREDITODAL)"
    '        End If
    '        If oMyPagamentiForSearch.sDataAccreditoAl <> "" Then
    '            cmdMyCommand.CommandText += " AND (DATA_ACCREDITO <=@ACCREDITOAL)"
    '        End If
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAFATTURA", SqlDbType.NVarChar)).Value = oMyPagamentiForSearch.sDataFattura
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NFATTURA", SqlDbType.NVarChar)).Value = oMyPagamentiForSearch.sNumeroFattura
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COGNOME", SqlDbType.NVarChar)).Value = oMyPagamentiForSearch.sCognome & "%"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOME", SqlDbType.NVarChar)).Value = oMyPagamentiForSearch.sNome & "%"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CFPIVA", SqlDbType.NVarChar)).Value = oReplace.ReplaceCharsForSearch(oMyPagamentiForSearch.sCodFiscalePIva) & "%"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNOFATTURA", SqlDbType.NVarChar)).Value = oMyPagamentiForSearch.sAnnoEmissioneFattura
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ACCREDITODAL", SqlDbType.NVarChar)).Value = oMyPagamentiForSearch.sDataAccredito
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ACCREDITOAL", SqlDbType.NVarChar)).Value = oMyPagamentiForSearch.sDataAccreditoAl
    '        If oMyPagamentiForSearch.nPeriodo > 0 Then
    '            cmdMyCommand.CommandText += " AND (IDPERIODO=@IDPERIODO)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPERIODO", SqlDbType.Int)).Value = oMyPagamentiForSearch.nPeriodo
    '        End If
    '        cmdMyCommand.CommandText += " GROUP BY ANNO_FATTURA, DATA_ACCREDITO, COD_CAPITOLO, IDVOCE"
    '        cmdMyCommand.CommandText += " ORDER BY ANNO_FATTURA, DATA_ACCREDITO, COD_CAPITOLO, IDVOCE"
    '        dvMyDati = iDB.GetDataView(cmdMyCommand)
    '        Return dvMyDati
    '    Catch Err As Exception
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsPagamenti.GetRiversamento.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in ClsPagamenti::GetRiversamento::" & Err.Message)

    '        Return Nothing
    '    End Try
    'End Function

#Region "DATA ENTRY"
    ''' <summary>
    ''' Estrae i pagamenti presenti per l'Ente in esame.
    ''' Può estrarre tutti i pagamenti oppure uno o più pagamento in base ai parametri passati tramite l'oggetto 
    ''' di tipo OggettoPagamento (es. ID pagamento, data fattura, ID anagrafico, numero fattura, anno di riferimento, nominativo, codice fiscale/partita IVA).
    ''' </summary>
    ''' <param name="obj">oggetto di tipo OggettoPagamento</param>
    ''' <returns>array di oggetti di tipo OggettoPagamento</returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Function GetPagamenti(ByVal obj As OggettoPagamento) As OggettoPagamento()
        Dim sSQL As String
        Dim dvMyDati As DataView = Nothing
        Dim oListPagamenti As New ArrayList
        Dim oMyPagamento As OggettoPagamento

        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_GetPagamenti", "IDENTE", "ID", "CODUTENTE", "DATAFATTURA", "NUMEROFATTURA", "ANNOEMISSIONEFATTURA", "COGNOME", "NOME", "CODFISCALEPIVA", "NPERIODO", "DATAACCREDITO", "DATAACCREDITOAL")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", obj.IDEnte) _
                        , ctx.GetParam("ID", obj.ID) _
                        , ctx.GetParam("CODUTENTE", obj.nCodUtente) _
                        , ctx.GetParam("DATAFATTURA", obj.sDataFattura) _
                        , ctx.GetParam("NUMEROFATTURA", obj.sNumeroFattura) _
                        , ctx.GetParam("ANNOEMISSIONEFATTURA", obj.sAnnoEmissioneFattura) _
                        , ctx.GetParam("COGNOME", FncGen.ReplaceCharsForSearch(obj.sCognome) & "%") _
                        , ctx.GetParam("NOME", FncGen.ReplaceCharsForSearch(obj.sNome) & "%") _
                        , ctx.GetParam("CODFISCALEPIVA", FncGen.ReplaceCharsForSearch(obj.sCodFiscalePIva) & "%") _
                        , ctx.GetParam("NPERIODO", obj.nPeriodo) _
                        , ctx.GetParam("DATAACCREDITO", obj.sDataAccredito) _
                        , ctx.GetParam("DATAACCREDITOAL", obj.sDataAccreditoAl)
                    )
                ctx.Dispose()
            End Using
            For Each myRow As DataRowView In dvMyDati
                oMyPagamento = New OggettoPagamento
                oMyPagamento.ID = StringOperation.FormatInt(myRow("ID"))
                oMyPagamento.nCodUtente = StringOperation.FormatInt(myRow("COD_UTENTE"))
                If StringOperation.FormatString(myRow("PARTITA_IVA")) <> "" Then
                    oMyPagamento.sCodFiscalePIva = StringOperation.FormatString(myRow("PARTITA_IVA"))
                Else
                    oMyPagamento.sCodFiscalePIva = StringOperation.FormatString(myRow("COD_FISCALE"))
                End If
                oMyPagamento.sCognome = StringOperation.FormatString(myRow("COGNOME_DENOMINAZIONE"))
                oMyPagamento.sDataFattura = StringOperation.FormatString(myRow("DATA_FATTURA"))
                oMyPagamento.sDataPagamento = StringOperation.FormatString(myRow("DATA_PAGAMENTO"))
                oMyPagamento.IDEnte = StringOperation.FormatString(myRow("IDENTE"))
                oMyPagamento.ImportoPagamento = StringOperation.FormatDouble(myRow("IMPORTO_PAGAMENTO"))
                '*** se è Pagamento di Parini Importo emesso è 0
                If StringOperation.FormatString(myRow("PROVENIENZA")) = "Parini" Then
                    oMyPagamento.ImportoEmesso = 0
                Else
                    oMyPagamento.ImportoEmesso = StringOperation.FormatDouble(("ImportoEmesso"))
                End If
                oMyPagamento.sNome = StringOperation.FormatString(myRow("NOME"))
                oMyPagamento.sNRata = StringOperation.FormatString(myRow("NUMERO_RATA"))
                oMyPagamento.sNumeroFattura = StringOperation.FormatString(myRow("NUMERO_FATTURA"))
                oMyPagamento.sOperatore = StringOperation.FormatString(myRow("OPERATORE"))
                oMyPagamento.sVia = StringOperation.FormatString(myRow("VIA_RES"))
                oMyPagamento.sCivico = StringOperation.FormatString(myRow("CIVICO_RES"))
                oMyPagamento.sInterno = StringOperation.FormatString(myRow("INTERNO_RES"))
                oMyPagamento.sAnnoEmissioneFattura = StringOperation.FormatString(myRow("DATA_FATTURA")).Substring(0, 4)
                oMyPagamento.sProvenienza = StringOperation.FormatString(myRow("PROVENIENZA"))
                '*** 02122008 Fabi Modifica per data_accredito
                oMyPagamento.sDataAccredito = StringOperation.FormatString(myRow("DATA_ACCREDITO"))
                oListPagamenti.Add(oMyPagamento)
            Next
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsPagamenti.GetStampaPagamenti.errore: ", Err)
        Finally
            dvMyDati.Dispose()
        End Try
        Return CType(oListPagamenti.ToArray(GetType(OggettoPagamento)), OggettoPagamento())
    End Function
    'Public Function GetPagamenti(ByVal obj As OggettoPagamento) As OggettoPagamento()
    '    'Dim culture As IFormatProvider
    '    'culture = New System.Globalization.CultureInfo("it-IT", True)
    '    'System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")

    '    'dim sSQL as string
    '    'Dim WFErrore As String
    '    'Dim WFSessione As CreateSessione

    '    'Dim NomeDbAnag As String = ConfigurationManager.AppSettings("NomeDbAnagrafe").ToString()

    '    Dim myAdapter As New SqlDataAdapter
    '    Dim myDataSet As New DataSet
    '    Dim dtMyDati As New DataTable
    '    Dim dtMyRow As DataRow
    '    Dim oListPagamenti As New ArrayList
    '    Dim oMyPagamento As OggettoPagamento

    '    Try
    '        ''inizializzo la connessione
    '        'WFSessione = New CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))
    '        'If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '        '	Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        'End If

    '        'sSQL = "SELECT TP_PAGAMENTI.ID, TP_PAGAMENTI.COD_UTENTE, TP_PAGAMENTI.IDENTE, TP_PAGAMENTI.DATA_FATTURA, TP_PAGAMENTI.NUMERO_FATTURA,  TP_PAGAMENTI.NUMERO_RATA, TP_PAGAMENTI.DATA_PAGAMENTO, TP_PAGAMENTI.IMPORTO_PAGAMENTO, TP_PAGAMENTI.OPERATORE, TP_PAGAMENTI.PROVENIENZA, "
    '        'sSQL += " TP_FATTURE_NOTE.COGNOME_DENOMINAZIONE, TP_FATTURE_NOTE.NOME, TP_FATTURE_NOTE.COD_UTENTE AS COD_CONTRIBUENTE, TP_FATTURE_NOTE.COD_FISCALE, TP_FATTURE_NOTE.PARTITA_IVA, TP_FATTURE_NOTE.VIA_RES, TP_FATTURE_NOTE.CIVICO_RES, TP_FATTURE_NOTE.INTERNO_RES,  TP_FATTURE_NOTE.IMPORTO_FATTURANOTA AS IMPORTOEMESSO, TP_PAGAMENTI.DATA_ACCREDITO  "
    '        'sSQL += " FROM TP_PAGAMENTI "
    '        'sSQL += " LEFT JOIN TP_FATTURE_NOTE ON TP_PAGAMENTI.IDENTE=TP_FATTURE_NOTE.IDENTE AND TP_PAGAMENTI.DATA_FATTURA=TP_FATTURE_NOTE.DATA_FATTURA AND TP_PAGAMENTI.NUMERO_FATTURA=TP_FATTURE_NOTE.NUMERO_FATTURA"
    '        'sSQL += " WHERE (TP_FATTURE_NOTE.DATA_VARIAZIONE IS NULL) AND TP_PAGAMENTI.IDENTE='" & ConstSession.IdEnte & "'"
    '        'If obj.ID.CompareTo(-1) <> 0 And obj.ID.CompareTo(0) <> 0 Then
    '        '	sSQL += " AND TP_PAGAMENTI.ID=" & obj.ID & ""
    '        'End If
    '        'If obj.nCodUtente.CompareTo(-1) <> 0 And obj.nCodUtente.CompareTo(0) <> 0 Then
    '        '	sSQL += " AND TP_PAGAMENTI.COD_UTENTE=" & obj.nCodUtente & ""
    '        'End If
    '        'If obj.sDataFattura.CompareTo("") <> 0 Then
    '        '	sSQL += " AND TP_PAGAMENTI.DATA_FATTURA='" & obj.sDataFattura & "'"
    '        'End If
    '        'If obj.sNumeroFattura.CompareTo("") <> 0 Then
    '        '	sSQL += " AND TP_PAGAMENTI.NUMERO_FATTURA='" & obj.sNumeroFattura & "'"
    '        'End If
    '        'If obj.sAnnoEmissioneFattura.CompareTo("") <> 0 Then
    '        '	sSQL += " AND substring(TP_PAGAMENTI.DATA_FATTURA, 1, 4)='" & obj.sAnnoEmissioneFattura & "'"
    '        'End If
    '        'If obj.sCognome.CompareTo("") <> 0 Then
    '        '	sSQL += " AND TP_FATTURE_NOTE.COGNOME_DENOMINAZIONE LIKE '" & obj.sCognome & "%'"
    '        'End If
    '        'If obj.sNome.CompareTo("") <> 0 Then
    '        '	sSQL += " AND TP_FATTURE_NOTE.NOME LIKE '" & obj.sNome & "%'"
    '        'End If
    '        'If obj.sCodFiscalePIva.CompareTo("") <> 0 Then
    '        '	sSQL += " AND (TP_FATTURE_NOTE.COD_FISCALE LIKE '" & oReplace.ReplaceCharsForSearch(obj.sCodFiscalePIva) & "%' OR TP_FATTURE_NOTE.PARTITA_IVA LIKE '" & oReplace.ReplaceCharsForSearch(obj.sCodFiscalePIva) & "%')"
    '        'End If
    '        'If obj.nPeriodo > 0 Then
    '        '	sSQL += " AND (TP_FATTURE_NOTE.IDPERIODO=" & obj.nPeriodo & ")"
    '        'End If
    '        ''*** 02122008 Fabi Modifica per data_accredito
    '        'If obj.sDataAccredito.CompareTo("") <> 0 Then
    '        '	sSQL += " AND TP_PAGAMENTI.DATA_ACCREDITO >='" & obj.sDataAccredito & "'"
    '        'End If
    '        'If obj.sDataAccreditoAl.CompareTo("") <> 0 Then
    '        '	sSQL += " AND TP_PAGAMENTI.DATA_ACCREDITO <='" & obj.sDataAccreditoAl & "'"
    '        'End If

    '        ''eseguo la query
    '        'Dim dsPagamenti As DataSet
    '        'dsPagamenti = WFSessione.oSession.oAppDB.GetPrivateDataSet(sSQL)
    '        'If dsPagamenti.Tables(0).Rows.Count > 0 Then
    '        '	Dim oDatiPagamento As OggettoPagamento
    '        '	Dim arrayListoDatiPagamento As New ArrayList
    '        '	Dim iCount As Integer
    '        '	For iCount = 0 To dsPagamenti.Tables(0).Rows.Count - 1
    '        '		oDatiPagamento = New OggettoPagamento

    '        '		oDatiPagamento.ID = dsPagamenti.Tables(0).Rows(iCount)("ID")
    '        '		If Not IsDBNull(dsPagamenti.Tables(0).Rows(iCount)("COD_UTENTE")) Then
    '        '			oDatiPagamento.nCodUtente = dsPagamenti.Tables(0).Rows(iCount)("COD_UTENTE")
    '        '		End If
    '        '		If Not IsDBNull(dsPagamenti.Tables(0).Rows(iCount)("PARTITA_IVA")) Then
    '        '			If dsPagamenti.Tables(0).Rows(iCount)("PARTITA_IVA") <> "" Then
    '        '				oDatiPagamento.sCodFiscalePIva = dsPagamenti.Tables(0).Rows(iCount)("PARTITA_IVA")
    '        '			Else
    '        '				If Not IsDBNull(dsPagamenti.Tables(0).Rows(iCount)("COD_FISCALE")) Then
    '        '					oDatiPagamento.sCodFiscalePIva = dsPagamenti.Tables(0).Rows(iCount)("COD_FISCALE")
    '        '				End If
    '        '			End If
    '        '		End If
    '        '		If Not IsDBNull(dsPagamenti.Tables(0).Rows(iCount)("COGNOME_DENOMINAZIONE")) Then
    '        '			oDatiPagamento.sCognome = dsPagamenti.Tables(0).Rows(iCount)("COGNOME_DENOMINAZIONE")
    '        '		End If
    '        '		If Not IsDBNull(dsPagamenti.Tables(0).Rows(iCount)("DATA_FATTURA")) Then
    '        '			oDatiPagamento.sDataFattura = dsPagamenti.Tables(0).Rows(iCount)("DATA_FATTURA")
    '        '		End If
    '        '		If Not IsDBNull(dsPagamenti.Tables(0).Rows(iCount)("DATA_PAGAMENTO")) Then
    '        '			oDatiPagamento.sDataPagamento = dsPagamenti.Tables(0).Rows(iCount)("DATA_PAGAMENTO")
    '        '		End If
    '        '		If Not IsDBNull(dsPagamenti.Tables(0).Rows(iCount)("IDENTE")) Then
    '        '			oDatiPagamento.IDEnte = dsPagamenti.Tables(0).Rows(iCount)("IDENTE")
    '        '		End If
    '        '		If IsDBNull(dsPagamenti.Tables(0).Rows(iCount)("IMPORTO_PAGAMENTO")) Then
    '        '			oDatiPagamento.ImportoPagamento = 0
    '        '		Else
    '        '			oDatiPagamento.ImportoPagamento = dsPagamenti.Tables(0).Rows(iCount)("IMPORTO_PAGAMENTO")
    '        '		End If
    '        '		'*** se è Pagamento di Parini Importo emesso è 0
    '        '		If Not (IsDBNull(dsPagamenti.Tables(0).Rows(iCount)("PROVENIENZA"))) And dsPagamenti.Tables(0).Rows(iCount)("PROVENIENZA").ToString() = "Parini" Then
    '        '			oDatiPagamento.ImportoEmesso = 0
    '        '		Else
    '        '			If IsDBNull(dsPagamenti.Tables(0).Rows(iCount)("ImportoEmesso")) Then
    '        '				oDatiPagamento.ImportoEmesso = 0
    '        '			Else
    '        '				oDatiPagamento.ImportoEmesso = dsPagamenti.Tables(0).Rows(iCount)("ImportoEmesso")
    '        '			End If
    '        '		End If
    '        '		If Not IsDBNull(dsPagamenti.Tables(0).Rows(iCount)("NOME")) Then
    '        '			oDatiPagamento.sNome = dsPagamenti.Tables(0).Rows(iCount)("NOME")
    '        '		End If
    '        '		If Not IsDBNull(dsPagamenti.Tables(0).Rows(iCount)("NUMERO_RATA")) Then
    '        '			oDatiPagamento.sNRata = dsPagamenti.Tables(0).Rows(iCount)("NUMERO_RATA")
    '        '		End If
    '        '		If Not IsDBNull(dsPagamenti.Tables(0).Rows(iCount)("NUMERO_FATTURA")) Then
    '        '			oDatiPagamento.sNumeroFattura = dsPagamenti.Tables(0).Rows(iCount)("NUMERO_FATTURA")
    '        '		End If
    '        '		If Not IsDBNull(dsPagamenti.Tables(0).Rows(iCount)("OPERATORE")) Then
    '        '			oDatiPagamento.sOperatore = dsPagamenti.Tables(0).Rows(iCount)("OPERATORE")
    '        '		End If
    '        '		If Not IsDBNull(dsPagamenti.Tables(0).Rows(iCount)("VIA_RES")) Then
    '        '			oDatiPagamento.sVia = dsPagamenti.Tables(0).Rows(iCount)("VIA_RES")
    '        '		End If
    '        '		If Not IsDBNull(dsPagamenti.Tables(0).Rows(iCount)("CIVICO_RES")) Then
    '        '			oDatiPagamento.sCivico = dsPagamenti.Tables(0).Rows(iCount)("CIVICO_RES")
    '        '		End If
    '        '		If Not IsDBNull(dsPagamenti.Tables(0).Rows(iCount)("INTERNO_RES")) Then
    '        '			oDatiPagamento.sInterno = dsPagamenti.Tables(0).Rows(iCount)("INTERNO_RES")
    '        '		End If
    '        '		If Not IsDBNull(dsPagamenti.Tables(0).Rows(iCount)("DATA_FATTURA")) Then
    '        '			oDatiPagamento.sAnnoEmissioneFattura = dsPagamenti.Tables(0).Rows(iCount)("DATA_FATTURA").ToString().Substring(0, 4)
    '        '		End If
    '        '		If Not IsDBNull(dsPagamenti.Tables(0).Rows(iCount)("PROVENIENZA")) Then
    '        '			oDatiPagamento.sProvenienza = dsPagamenti.Tables(0).Rows(iCount)("PROVENIENZA").ToString()
    '        '		End If
    '        '		If Not IsDBNull(dsPagamenti.Tables(0).Rows(iCount)("DATA_ACCREDITO")) Then
    '        '			'*** 02122008 Fabi Modifica per data_accredito
    '        '			oDatiPagamento.sDataAccredito = dsPagamenti.Tables(0).Rows(iCount)("DATA_ACCREDITO").ToString()
    '        '		End If
    '        '		arrayListoDatiPagamento.Add(oDatiPagamento)
    '        '	Next
    '        '	Return CType(arrayListoDatiPagamento.ToArray(GetType(OggettoPagamento)), OggettoPagamento())
    '        'End If
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandTimeout = 0
    '        'Valorizzo la connessione
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '            cmdMyCommand.Connection.Open()
    '        End If

    '        cmdMyCommand.CommandText = "prc_GetPagamenti"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = obj.IDEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = obj.ID
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CodUtente", SqlDbType.Int)).Value = obj.nCodUtente
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DataFattura", SqlDbType.VarChar)).Value = obj.sDataFattura
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NumeroFattura", SqlDbType.VarChar)).Value = obj.sNumeroFattura
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@AnnoEmissioneFattura", SqlDbType.NVarChar)).Value = obj.sAnnoEmissioneFattura
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Cognome", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(obj.sCognome) & "%"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Nome", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(obj.sNome) & "%"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CodFiscalePIva", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(obj.sCodFiscalePIva) & "%"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NPeriodo", SqlDbType.Int)).Value = obj.nPeriodo
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DataAccredito", SqlDbType.VarChar)).Value = obj.sDataAccredito
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DataAccreditoAl", SqlDbType.VarChar)).Value = obj.sDataAccreditoAl
    '        myAdapter.SelectCommand = cmdMyCommand
    '        myAdapter.Fill(myDataSet)
    '        dtMyDati = myDataSet.Tables(0)
    '        For Each dtMyRow In dtMyDati.Rows
    '            oMyPagamento = New OggettoPagamento
    '            oMyPagamento.ID = dtMyRow("ID")
    '            If Not IsDBNull(dtMyRow("COD_UTENTE")) Then
    '                oMyPagamento.nCodUtente = dtMyRow("COD_UTENTE")
    '            End If
    '            If Not IsDBNull(dtMyRow("PARTITA_IVA")) Then
    '                If dtMyRow("PARTITA_IVA") <> "" Then
    '                    oMyPagamento.sCodFiscalePIva = dtMyRow("PARTITA_IVA")
    '                Else
    '                    If Not IsDBNull(dtMyRow("COD_FISCALE")) Then
    '                        oMyPagamento.sCodFiscalePIva = dtMyRow("COD_FISCALE")
    '                    End If
    '                End If
    '            End If
    '            If Not IsDBNull(dtMyRow("COGNOME_DENOMINAZIONE")) Then
    '                oMyPagamento.sCognome = dtMyRow("COGNOME_DENOMINAZIONE")
    '            End If
    '            If Not IsDBNull(dtMyRow("DATA_FATTURA")) Then
    '                oMyPagamento.sDataFattura = dtMyRow("DATA_FATTURA")
    '            End If
    '            If Not IsDBNull(dtMyRow("DATA_PAGAMENTO")) Then
    '                oMyPagamento.sDataPagamento = dtMyRow("DATA_PAGAMENTO")
    '            End If
    '            If Not IsDBNull(dtMyRow("IDENTE")) Then
    '                oMyPagamento.IDEnte = dtMyRow("IDENTE")
    '            End If
    '            If IsDBNull(dtMyRow("IMPORTO_PAGAMENTO")) Then
    '                oMyPagamento.ImportoPagamento = 0
    '            Else
    '                oMyPagamento.ImportoPagamento = dtMyRow("IMPORTO_PAGAMENTO")
    '            End If
    '            '*** se è Pagamento di Parini Importo emesso è 0
    '            If Not (IsDBNull(dtMyRow("PROVENIENZA"))) And dtMyRow("PROVENIENZA").ToString() = "Parini" Then
    '                oMyPagamento.ImportoEmesso = 0
    '            Else
    '                If IsDBNull(dtMyRow("ImportoEmesso")) Then
    '                    oMyPagamento.ImportoEmesso = 0
    '                Else
    '                    oMyPagamento.ImportoEmesso = dtMyRow("ImportoEmesso")
    '                End If
    '            End If
    '            If Not IsDBNull(dtMyRow("NOME")) Then
    '                oMyPagamento.sNome = dtMyRow("NOME")
    '            End If
    '            If Not IsDBNull(dtMyRow("NUMERO_RATA")) Then
    '                oMyPagamento.sNRata = dtMyRow("NUMERO_RATA")
    '            End If
    '            If Not IsDBNull(dtMyRow("NUMERO_FATTURA")) Then
    '                oMyPagamento.sNumeroFattura = dtMyRow("NUMERO_FATTURA")
    '            End If
    '            If Not IsDBNull(dtMyRow("OPERATORE")) Then
    '                oMyPagamento.sOperatore = dtMyRow("OPERATORE")
    '            End If
    '            If Not IsDBNull(dtMyRow("VIA_RES")) Then
    '                oMyPagamento.sVia = dtMyRow("VIA_RES")
    '            End If
    '            If Not IsDBNull(dtMyRow("CIVICO_RES")) Then
    '                oMyPagamento.sCivico = dtMyRow("CIVICO_RES")
    '            End If
    '            If Not IsDBNull(dtMyRow("INTERNO_RES")) Then
    '                oMyPagamento.sInterno = dtMyRow("INTERNO_RES")
    '            End If
    '            If Not IsDBNull(dtMyRow("DATA_FATTURA")) Then
    '                oMyPagamento.sAnnoEmissioneFattura = dtMyRow("DATA_FATTURA").ToString().Substring(0, 4)
    '            End If
    '            If Not IsDBNull(dtMyRow("PROVENIENZA")) Then
    '                oMyPagamento.sProvenienza = dtMyRow("PROVENIENZA").ToString()
    '            End If
    '            If Not IsDBNull(dtMyRow("DATA_ACCREDITO")) Then
    '                '*** 02122008 Fabi Modifica per data_accredito
    '                oMyPagamento.sDataAccredito = dtMyRow("DATA_ACCREDITO").ToString()
    '            End If
    '            'prelevo i dati della testata
    '            oListPagamenti.Add(oMyPagamento)
    '        Next

    '        Return CType(oListPagamenti.ToArray(GetType(OggettoPagamento)), OggettoPagamento())

    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsPagamenti.GetStampaPagamenti.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in ClsPagamenti::GetPagamenti::" & Err.Message & " SQL::" & cmdMyCommand.CommandText)
    '    Finally
    '        ''chiudo la connessione
    '        'If Not WFSessione.oSession Is Nothing Then
    '        '    WFSessione.Kill()
    '        'End If
    '        dtMyDati.Dispose()
    '        cmdMyCommand.Dispose()
    '        cmdMyCommand.Connection.Close()
    '    End Try
    'End Function

    ''' <summary>
    ''' Estrae le fatture presenti per l'Ente in esame a cui si possono associare i pagamenti.
    ''' Può estrarre tutte le fatture oppure uno o più fatture in base ai parametri passati tramite l'oggetto 
    ''' di tipo OggettoPagamento (es. ID pagamento, data fattura, ID anagrafico, numero fattura, anno di riferimento, nominativo, codice fiscale/partita IVA).
    ''' </summary>
    ''' <param name="objFatturaPagamenti">oggetto di tipo OggettoPagamento</param>
    ''' <returns>array di oggetti di tipo OggettoPagamento</returns>
    ''' <remarks>
    ''' </remarks>
    Public Function GetFatturaPagamenti(ByVal objFatturaPagamenti As OggettoPagamento) As OggettoPagamento()
        Dim sSQL As String
        Dim dvMyDati As DataView = Nothing
        Dim oFatturaPagamenti As OggettoPagamento
        Dim oListFatturaPagamenti() As OggettoPagamento = Nothing
        Dim nList As Integer = -1

        Try
            'inizializzo la connessione
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_GetFatturaPagamenti", "IDENTE", "ID", "ANNO", "NFATTURA", "DATAFATTURA", "IMPORTO", "COGNOME", "NOME")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", ConstSession.IdEnte) _
                        , ctx.GetParam("ID", objFatturaPagamenti.ID) _
                        , ctx.GetParam("ANNO", objFatturaPagamenti.sAnnoEmissioneFattura) _
                        , ctx.GetParam("NFATTURA", objFatturaPagamenti.sNumeroFattura) _
                        , ctx.GetParam("DATAFATTURA", objFatturaPagamenti.sDataFattura) _
                        , ctx.GetParam("IMPORTO", objFatturaPagamenti.ImportoEmesso) _
                        , ctx.GetParam("COGNOME", oReplace.ReplaceCharsForSearch(objFatturaPagamenti.sCognome)) _
                        , ctx.GetParam("NOME", oReplace.ReplaceCharsForSearch(objFatturaPagamenti.sNome))
                    )
                ctx.Dispose()
            End Using
            For Each myRow As DataRowView In dvMyDati
                oFatturaPagamenti = New OggettoPagamento
                oFatturaPagamenti.ID = StringOperation.FormatInt(myRow("id"))
                oFatturaPagamenti.IDEnte = StringOperation.FormatString(myRow("idente"))
                oFatturaPagamenti.IdFatturaNota = StringOperation.FormatInt(myRow("idfatturanota"))
                oFatturaPagamenti.sAnnoEmissioneFattura = StringOperation.FormatString(myRow("anno_riferimento"))
                oFatturaPagamenti.nCodUtente = StringOperation.FormatInt(myRow("cod_utente"))
                oFatturaPagamenti.sVia = StringOperation.FormatString(myRow("via_res"))
                oFatturaPagamenti.sCivico = StringOperation.FormatString(myRow("civico_res"))
                oFatturaPagamenti.sInterno = StringOperation.FormatString(myRow("interno_res"))
                oFatturaPagamenti.ImportoEmesso = StringOperation.FormatDouble(myRow("importo_fatturanota"))
                oFatturaPagamenti.sNome = StringOperation.FormatString(myRow("NOME"))
                oFatturaPagamenti.sCognome = StringOperation.FormatString(myRow("COGNOME_DENOMINAZIONE"))
                oFatturaPagamenti.sNumeroFattura = StringOperation.FormatString(myRow("NUMERO_FATTURA"))
                If StringOperation.FormatString(myRow("PARTITA_IVA")) <> "" Then
                    oFatturaPagamenti.sCodFiscalePIva = StringOperation.FormatString(myRow("PARTITA_IVA"))
                Else
                    oFatturaPagamenti.sCodFiscalePIva = StringOperation.FormatString(myRow("cod_fiscale"))
                End If
                If StringOperation.FormatString(myRow("Data_Pagamento")) <> "" Then
                    oFatturaPagamenti.sDataPagamento = StringOperation.FormatString(myRow("Data_Pagamento"))
                End If
                If StringOperation.FormatString(myRow("data_fattura")) <> "" Then
                    oFatturaPagamenti.sDataFattura = StringOperation.FormatString(myRow("data_fattura"))
                End If
                oFatturaPagamenti.ImportoPagamento = StringOperation.FormatDouble(myRow("Importo_Pagamento"))
                oFatturaPagamenti.sOperatore = StringOperation.FormatString(myRow("OPERATORE"))
                oFatturaPagamenti.sNRata = StringOperation.FormatString(myRow("NUMERO_RATA"))
                If StringOperation.FormatString(myRow("data_accredito")) <> "" Then
                    oFatturaPagamenti.sDataAccredito = StringOperation.FormatString(myRow("data_accredito"))
                End If

                'ridimensiono l'array
                nList += 1
                ReDim Preserve oListFatturaPagamenti(nList)
                oListFatturaPagamenti(nList) = oFatturaPagamenti
            Next
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsPagamenti.GetFatturaPagamenti.errore: ", Err)
        Finally
            dvMyDati.Dispose()
        End Try
        Return oListFatturaPagamenti
    End Function
    'Public Function GetFatturaPagamenti(ByVal objFatturaPagamenti As OggettoPagamento) As OggettoPagamento()
    '    Dim DrDati As SqlClient.SqlDataReader
    '    Try
    '        Dim sSQL As String
    '        Dim oFatturaPagamenti As OggettoPagamento
    '        Dim oListFatturaPagamenti() As OggettoPagamento
    '        Dim nList As Integer = -1

    '        'inizializzo la connessione
    '        sSQL = "SELECT TP_FATTURE_NOTE.*, TP_PAGAMENTI.ID, TP_PAGAMENTI.DATA_PAGAMENTO, TP_PAGAMENTI.IMPORTO_PAGAMENTO, TP_PAGAMENTI.OPERATORE, TP_PAGAMENTI.NUMERO_RATA, TP_PAGAMENTI.DATA_ACCREDITO"
    '        sSQL += " FROM TP_FATTURE_NOTE"
    '        sSQL += " LEFT JOIN TP_PAGAMENTI ON TP_FATTURE_NOTE.IDENTE=TP_PAGAMENTI.IDENTE"
    '        sSQL += " AND TP_FATTURE_NOTE.NUMERO_FATTURA=TP_PAGAMENTI.NUMERO_FATTURA AND TP_FATTURE_NOTE.DATA_FATTURA=TP_PAGAMENTI.DATA_FATTURA"
    '        sSQL += " WHERE (TP_FATTURE_NOTE.DATA_VARIAZIONE IS NULL) AND TP_FATTURE_NOTE.IDENTE='" & ConstSession.IdEnte & "'"
    '        If objFatturaPagamenti.ID <> -1 Then
    '            sSQL += " AND (TP_PAGAMENTI.ID = " & objFatturaPagamenti.ID & ")"
    '        End If
    '        If objFatturaPagamenti.sAnnoEmissioneFattura <> "" Then
    '            sSQL += " AND (TP_FATTURE_NOTE.ANNO_RIFERIMENTO = '" & objFatturaPagamenti.sAnnoEmissioneFattura & "')"
    '        End If
    '        If objFatturaPagamenti.sNumeroFattura <> "" Then
    '            sSQL += " AND (TP_FATTURE_NOTE.NUMERO_FATTURA = '" & objFatturaPagamenti.sNumeroFattura & "')"
    '        End If
    '        If objFatturaPagamenti.sDataFattura <> "" Then
    '            sSQL += " AND (TP_FATTURE_NOTE.DATA_FATTURA = '" & objFatturaPagamenti.sDataFattura & "')"
    '        End If
    '        If objFatturaPagamenti.ImportoEmesso <> CDbl(0) Then
    '            sSQL += " AND (TP_FATTURE_NOTE.IMPORTO_FATTURANOTA = " & objFatturaPagamenti.ImportoEmesso.ToString().Replace(",", ".") & ")"
    '        End If
    '        If objFatturaPagamenti.sCognome.CompareTo("") <> 0 Then
    '            sSQL += " AND TP_FATTURE_NOTE.COGNOME_DENOMINAZIONE LIKE '" & oReplace.ReplaceCharsForSearch(objFatturaPagamenti.sCognome) & "%'"
    '        End If
    '        If objFatturaPagamenti.sNome.CompareTo("") <> 0 Then
    '            sSQL += " AND TP_FATTURE_NOTE.NOME LIKE '" & oReplace.ReplaceCharsForSearch(objFatturaPagamenti.sNome) & "%'"
    '        End If
    '        sSQL += " AND (TP_FATTURE_NOTE.IMPORTO_FATTURANOTA > 0) AND (TP_FATTURE_NOTE.NUMERO_FATTURA<>'' AND NOT TP_FATTURE_NOTE.NUMERO_FATTURA IS NULL)"

    '        'eseguo la query
    '        DrDati = iDB.GetDataReader(sSQL)
    '        Do While DrDati.Read
    '            oFatturaPagamenti = New OggettoPagamento
    '            If Not IsDBNull(DrDati("id")) Then
    '                oFatturaPagamenti.ID = CInt(DrDati("id"))
    '            End If
    '            oFatturaPagamenti.IDEnte = CStr(DrDati("idente"))
    '            oFatturaPagamenti.IdFatturaNota = CInt(DrDati("idfatturanota"))
    '            oFatturaPagamenti.sAnnoEmissioneFattura = CStr(DrDati("anno_riferimento"))
    '            oFatturaPagamenti.nCodUtente = CInt(DrDati("cod_utente"))
    '            oFatturaPagamenti.sVia = CStr(DrDati("via_res"))
    '            oFatturaPagamenti.sCivico = CStr(DrDati("civico_res"))
    '            oFatturaPagamenti.sInterno = CStr(DrDati("interno_res"))
    '            oFatturaPagamenti.ImportoEmesso = CDbl(DrDati("importo_fatturanota"))
    '            oFatturaPagamenti.sNome = CStr(DrDati("NOME"))
    '            oFatturaPagamenti.sCognome = CStr(DrDati("COGNOME_DENOMINAZIONE"))
    '            oFatturaPagamenti.sNumeroFattura = CStr(DrDati("NUMERO_FATTURA"))
    '            If CStr(DrDati("PARTITA_IVA")) <> "" Then
    '                oFatturaPagamenti.sCodFiscalePIva = CStr(DrDati("PARTITA_IVA"))
    '            Else
    '                oFatturaPagamenti.sCodFiscalePIva = CStr(DrDati("cod_fiscale"))
    '            End If
    '            If Not IsDBNull(DrDati("Data_Pagamento")) Then
    '                If CStr(DrDati("Data_Pagamento")) <> "" Then
    '                    oFatturaPagamenti.sDataPagamento = CStr(DrDati("Data_Pagamento"))
    '                End If
    '            End If
    '            If Not IsDBNull(DrDati("data_fattura")) Then
    '                If CStr(DrDati("data_fattura")) <> "" Then
    '                    oFatturaPagamenti.sDataFattura = CStr(DrDati("data_fattura"))
    '                End If
    '            End If
    '            If Not IsDBNull(DrDati("Importo_Pagamento")) Then
    '                oFatturaPagamenti.ImportoPagamento = CDbl(DrDati("Importo_Pagamento"))
    '            End If
    '            If Not IsDBNull(DrDati("OPERATORE")) Then
    '                oFatturaPagamenti.sOperatore = CStr(DrDati("OPERATORE"))
    '            End If
    '            If Not IsDBNull(DrDati("NUMERO_RATA")) Then
    '                oFatturaPagamenti.sNRata = CStr(DrDati("NUMERO_RATA"))
    '            End If

    '            If Not IsDBNull(DrDati("data_accredito")) Then
    '                If CStr(DrDati("data_accredito")) <> "" Then
    '                    oFatturaPagamenti.sDataAccredito = CStr(DrDati("data_accredito"))
    '                End If
    '            End If

    '            'ridimensiono l'array
    '            nList += 1
    '            ReDim Preserve oListFatturaPagamenti(nList)
    '            oListFatturaPagamenti(nList) = oFatturaPagamenti
    '        Loop
    '        DrDati.Close()
    '        Return oListFatturaPagamenti
    '    Catch Err As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsPagamenti.GetFatturaPagamenti.errore: ", Err)
    '        Return Nothing
    '    End Try
    'End Function

    ''' <summary>
    ''' Estrae le fatture presenti per l'Ente in esame che risultano avere un importo totale pagato superiore all'emesso.
    ''' </summary>
    ''' <returns>Dataset</returns>
    ''' <remarks>
    ''' </remarks>


    Public Function GetStampaPagamenti(nType As Integer, ByVal oMyPagamentiForSearch As OggettoPagamento) As DataView
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView

        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_GetStampaPagamenti", "TYPESTAMPA", "IDENTE", "IDPERIODO", "ANNOEMISSIONE", "DATAFATTURA", "NFATTURA", "COGNOME", "NOME", "CFPIVA", "ACCREDITODAL", "ACCREDITOAL")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("TYPESTAMPA", nType) _
                        , ctx.GetParam("IDENTE", oMyPagamentiForSearch.IDEnte) _
                        , ctx.GetParam("IDPERIODO", oMyPagamentiForSearch.nPeriodo) _
                        , ctx.GetParam("ANNOEMISSIONE", oMyPagamentiForSearch.sAnnoEmissioneFattura) _
                        , ctx.GetParam("DATAFATTURA", oMyPagamentiForSearch.sDataFattura) _
                        , ctx.GetParam("NFATTURA", oMyPagamentiForSearch.sNumeroFattura) _
                        , ctx.GetParam("COGNOME", oReplace.ReplaceCharsForSearch(oMyPagamentiForSearch.sCognome)) _
                        , ctx.GetParam("NOME", oReplace.ReplaceCharsForSearch(oMyPagamentiForSearch.sNome)) _
                        , ctx.GetParam("CFPIVA", oReplace.ReplaceCharsForSearch(oMyPagamentiForSearch.sCodFiscalePIva)) _
                        , ctx.GetParam("ACCREDITODAL", oMyPagamentiForSearch.sDataAccredito) _
                        , ctx.GetParam("ACCREDITOAL", oMyPagamentiForSearch.sDataAccreditoAl)
                    )
                ctx.Dispose()
            End Using
            Return dvMyDati
        Catch ex As Exception
            Log.Debug(oMyPagamentiForSearch.IDEnte + " - OPENgovH2O.ClsPagamenti.GetInsoluti.errore: ", ex)
            Throw New Exception("Errori nella stampa ClsPagamenti::GetInsoluti:: " + ex.Message)
        End Try
    End Function
    'Public Function GetRimborsi(ByVal sIdEnte As String, ByVal oMyPagamentiForSearch As OggettoPagamento) As DataView
    '    Dim sSQL As String = ""
    '    Dim dvMyDati As New DataView

    '    Try
    '        '*** QUERY SBAGLIATA ***
    '        'sSQL = "SELECT TP_FATTURE_NOTE.NUMEROUTENTE, TP_FATTURE_NOTE.COD_UTENTE, TP_FATTURE_NOTE.IMPORTO_FATTURANOTA, SUM(TP_PAGAMENTI.IMPORTO_PAGAMENTO) AS IMPORTO_PAGATO,TP_FATTURE_NOTE.IDENTE,TP_FATTURE_NOTE.DATA_VARIAZIONE,"
    '        'sSQL += NomeDbAnag & ".ANAGRAFICA.DATA_FINE_VALIDITA, " & NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.COGNOME_INVIO, " & NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.NOME_INVIO, " & NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.VIA_RCP, "
    '        'sSQL += NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.CIVICO_RCP, " & NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.INTERNO_CIVICO_RCP, " & NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.COMUNE_RCP, "
    '        'sSQL += NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.PROVINCIA_RCP, " & NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.CAP_RCP, " & NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.DATA_FINE_VALIDITA,"
    '        'sSQL += NomeDbAnag & ".ANAGRAFICA.DATA_FINE_VALIDITA, " & NomeDbAnag & ".ANAGRAFICA.COGNOME_DENOMINAZIONE, " & NomeDbAnag & ".ANAGRAFICA.NOME, " & NomeDbAnag & ".ANAGRAFICA.COD_FISCALE, "
    '        'sSQL += NomeDbAnag & ".ANAGRAFICA.PARTITA_IVA, " & NomeDbAnag & ".ANAGRAFICA.VIA_RES, " & NomeDbAnag & ".ANAGRAFICA.CIVICO_RES, " & NomeDbAnag & ".ANAGRAFICA.INTERNO_CIVICO_RES, "
    '        'sSQL += NomeDbAnag & ".ANAGRAFICA.COMUNE_RES, " & NomeDbAnag & ".ANAGRAFICA.PROVINCIA_RES, " & NomeDbAnag & ".ANAGRAFICA.CAP_RES"
    '        'sSQL += " FROM TP_FATTURE_NOTE INNER JOIN TP_PAGAMENTI ON TP_FATTURE_NOTE.DATA_FATTURA = TP_PAGAMENTI.DATA_FATTURA AND TP_FATTURE_NOTE.NUMERO_FATTURA = TP_PAGAMENTI.NUMERO_FATTURA"
    '        'sSQL += " INNER JOIN " & NomeDbAnag & ".ANAGRAFICA ON TP_FATTURE_NOTE.COD_UTENTE= " & NomeDbAnag & ".ANAGRAFICA.COD_CONTRIBUENTE lEFT JOIN " & NomeDbAnag & ".INDIRIZZI_SPEDIZIONE ON " & NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.COD_CONTRIBUENTE= " & NomeDbAnag & ".ANAGRAFICA.COD_CONTRIBUENTE"
    '        'sSQL += " WHERE (IDENTE='" & oMyPagamentiForSearch.IDEnte & "')"
    '        'If oMyPagamentiForSearch.sDataFattura <> "" Then
    '        '    sSQL += " AND (DATA_FATTURA='" & oMyPagamentiForSearch.sDataFattura & "')"
    '        'End If
    '        'If oMyPagamentiForSearch.sNumeroFattura <> "" Then
    '        '    sSQL += " AND (NUMERO_FATTURA='" & oMyPagamentiForSearch.sNumeroFattura & "')"
    '        'End If
    '        'If oMyPagamentiForSearch.sCognome <> "" Then
    '        '    sSQL += " AND (COGNOME LIKE '" & oMyPagamentiForSearch.sCognome & "%')"
    '        'End If
    '        'If oMyPagamentiForSearch.sNome <> "" Then
    '        '    sSQL += " AND (NOME LIKE'" & oMyPagamentiForSearch.sNome & "%')"
    '        'End If
    '        'If oMyPagamentiForSearch.sCodFiscalePIva <> "" Then
    '        '    sSQL += " AND (CFPIVA LIKE '" & oReplace.ReplaceCharsForSearch(oMyPagamentiForSearch.sCodFiscalePIva) & "%')"
    '        'End If
    '        ''*** 02122008 Fabi Modifica per data_accredito
    '        'If oMyPagamentiForSearch.sDataAccredito <> "" Then
    '        '    sSQL += " AND (DATA_ACCREDITO >='" & oMyPagamentiForSearch.sDataAccredito & "')"
    '        'End If
    '        'If oMyPagamentiForSearch.sDataAccreditoAl <> "" Then
    '        '    sSQL += " AND (DATA_ACCREDITO <='" & oMyPagamentiForSearch.sDataAccreditoAl & "')"
    '        'End If
    '        'sSQL += " GROUP BY TP_FATTURE_NOTE.NUMEROUTENTE, TP_FATTURE_NOTE.COD_UTENTE, TP_FATTURE_NOTE.IMPORTO_FATTURANOTA,TP_FATTURE_NOTE.IDENTE,TP_FATTURE_NOTE.DATA_VARIAZIONE," & NomeDbAnag & ".ANAGRAFICA.DATA_FINE_VALIDITA,"
    '        'sSQL += NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.COGNOME_INVIO, " & NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.NOME_INVIO, " & NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.VIA_RCP, "
    '        'sSQL += NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.CIVICO_RCP, " & NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.INTERNO_CIVICO_RCP, " & NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.COMUNE_RCP, "
    '        'sSQL += NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.PROVINCIA_RCP, " & NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.CAP_RCP, " & NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.DATA_FINE_VALIDITA, " & NomeDbAnag & ".ANAGRAFICA.DATA_FINE_VALIDITA, " & NomeDbAnag & ".ANAGRAFICA.COGNOME_DENOMINAZIONE, "
    '        'sSQL += NomeDbAnag & ".ANAGRAFICA.NOME, " & NomeDbAnag & ".ANAGRAFICA.COD_FISCALE, " & NomeDbAnag & ".ANAGRAFICA.PARTITA_IVA, " & NomeDbAnag & ".ANAGRAFICA.VIA_RES, "
    '        'sSQL += NomeDbAnag & ".ANAGRAFICA.CIVICO_RES, " & NomeDbAnag & ".ANAGRAFICA.INTERNO_CIVICO_RES, " & NomeDbAnag & ".ANAGRAFICA.COMUNE_RES, " & NomeDbAnag & ".ANAGRAFICA.PROVINCIA_RES, "
    '        'sSQL += NomeDbAnag & ".ANAGRAFICA.CAP_RES"
    '        'sSQL += " HAVING (" & NomeDbAnag & ".ANAGRAFICA.DATA_FINE_VALIDITA IS NULL) AND (" & NomeDbAnag & ".INDIRIZZI_SPEDIZIONE.DATA_FINE_VALIDITA IS NULL)"
    '        'sSQL += " AND TP_FATTURE_NOTE.DATA_VARIAZIONE IS NULL"
    '        'sSQL += " AND TP_FATTURE_NOTE.IMPORTO_FATTURANOTA < SUM(TP_PAGAMENTI.IMPORTO_PAGAMENTO)"
    '        'sSQL += " GROUP BY COD_UTENTE, COGNOME, NOME, CFPIVA, DATA_FATTURA, NUMERO_FATTURA, IMPEMESSO, DATA_ACCREDITO, DATA_PAGAMENTO, PROVENIENZA, COD_CAPITOLO, IDVOCE"
    '        'sSQL += " ORDER BY COGNOME, NOME, CFPIVA, DATA_FATTURA, NUMERO_FATTURA,  DATA_ACCREDITO, DATA_PAGAMENTO, PROVENIENZA, COD_CAPITOLO, IDVOCE"
    '        '*** ***
    '        sSQL = "SELECT NUMEROUTENTE"
    '        sSQL += " , COGNOME, NOME, CFPIVA"
    '        sSQL += " , VIA,CIVICO,ESPONENTE_CIVICO,CAP,COMUNE, PROVINCIA"
    '        sSQL += " , DATA_FATTURA, NUMERO_FATTURA, IMPEMESSO, IMPPAG AS PAGATO"
    '        sSQL += " , IMPEMESSO-IMPPAG AS DIF"
    '        sSQL += " FROM OPENGOV_FATTUREPAGAMENTIPERRIMBORSI"
    '        sSQL += " WHERE IMPEMESSO<IMPPAG"
    '        sSQL += " AND (IDENTE='" & oMyPagamentiForSearch.IDEnte & "')"
    '        If oMyPagamentiForSearch.sDataFattura <> "" Then
    '            sSQL += " AND (DATA_FATTURA='" & oMyPagamentiForSearch.sDataFattura & "')"
    '        End If
    '        If oMyPagamentiForSearch.sCognome <> "" Then
    '            sSQL += " AND (COGNOME LIKE '" & oMyPagamentiForSearch.sCognome & "%')"
    '        End If
    '        If oMyPagamentiForSearch.sNome <> "" Then
    '            sSQL += " AND (NOME LIKE'" & oMyPagamentiForSearch.sNome & "%')"
    '        End If
    '        If oMyPagamentiForSearch.sCodFiscalePIva <> "" Then
    '            sSQL += " AND (CFPIVA LIKE '" & oReplace.ReplaceCharsForSearch(oMyPagamentiForSearch.sCodFiscalePIva) & "%')"
    '        End If
    '        If oMyPagamentiForSearch.sAnnoEmissioneFattura <> "" Then
    '            sSQL += " AND (YEAR(DATA_FATTURA)=" & oMyPagamentiForSearch.sAnnoEmissioneFattura & ")"
    '        End If
    '        If oMyPagamentiForSearch.nPeriodo > 0 Then
    '            sSQL += " AND (IDPERIODO=" & oMyPagamentiForSearch.nPeriodo & ")"
    '        End If
    '        If oMyPagamentiForSearch.sNumeroFattura <> "" Then
    '            sSQL += " AND (NUMERO_FATTURA='" & oMyPagamentiForSearch.sNumeroFattura & "')"
    '        End If
    '        sSQL += " ORDER BY COGNOME, NOME, CFPIVA"
    '        sSQL += " , DATA_FATTURA, NUMERO_FATTURA"
    '        dvMyDati = iDB.GetDataView(sSQL)
    '        Return dvMyDati

    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsPagamenti.GetRimborsi.errore: ", ex)
    '        Throw New Exception("Errori nella stampa ClsPagamenti::GetRimborsi:: " + ex.Message)
    '    End Try
    'End Function 
    'Public Function GetInsoluti(ByVal sIdEnte As String, ByVal oMyPagamentiForSearch As OggettoPagamento) As DataView
    '    Dim sSQL As String = ""
    '    Dim dvMyDati As New DataView

    '    Try
    '        sSQL = "SELECT NUMEROUTENTE"
    '        sSQL += " , COGNOME, NOME, CFPIVA"
    '        sSQL += " , VIA,CIVICO,ESPONENTE_CIVICO,CAP,COMUNE, PROVINCIA"
    '        sSQL += " , DATA_FATTURA, NUMERO_FATTURA, IMPEMESSO, IMPPAG AS PAGATO"
    '        sSQL += " , IMPEMESSO-IMPPAG AS DIF"
    '        sSQL += " FROM OPENGOV_FATTUREPAGAMENTIPERINSOLUTI"
    '        sSQL += " WHERE IMPEMESSO>IMPPAG"
    '        sSQL += " AND (IDENTE='" & oMyPagamentiForSearch.IDEnte & "')"
    '        If oMyPagamentiForSearch.sDataFattura <> "" Then
    '            sSQL += " AND (DATA_FATTURA='" & oMyPagamentiForSearch.sDataFattura & "')"
    '        End If
    '        If oMyPagamentiForSearch.sCognome <> "" Then
    '            sSQL += " AND (COGNOME LIKE '" & oMyPagamentiForSearch.sCognome & "%')"
    '        End If
    '        If oMyPagamentiForSearch.sNome <> "" Then
    '            sSQL += " AND (NOME LIKE'" & oMyPagamentiForSearch.sNome & "%')"
    '        End If
    '        If oMyPagamentiForSearch.sCodFiscalePIva <> "" Then
    '            sSQL += " AND (CFPIVA LIKE '" & oReplace.ReplaceCharsForSearch(oMyPagamentiForSearch.sCodFiscalePIva) & "%')"
    '        End If
    '        If oMyPagamentiForSearch.sAnnoEmissioneFattura <> "" Then
    '            sSQL += " AND (YEAR(DATA_FATTURA)=" & oMyPagamentiForSearch.sAnnoEmissioneFattura & ")"
    '        End If
    '        If oMyPagamentiForSearch.nPeriodo > 0 Then
    '            sSQL += " AND (IDPERIODO=" & oMyPagamentiForSearch.nPeriodo & ")"
    '        End If
    '        If oMyPagamentiForSearch.sNumeroFattura <> "" Then
    '            sSQL += " AND (NUMERO_FATTURA='" & oMyPagamentiForSearch.sNumeroFattura & "')"
    '        End If
    '        sSQL += " ORDER BY COGNOME, NOME, CFPIVA"
    '        sSQL += " , DATA_FATTURA, NUMERO_FATTURA"
    '        dvMyDati = iDB.GetDataView(sSQL)
    '        Return dvMyDati

    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsPagamenti.GetInsoluti.errore: ", ex)
    '        Throw New Exception("Errori nella stampa ClsPagamenti::GetInsoluti:: " + ex.Message)
    '    End Try
    'End Function

    Public Function SetPagamento(ByVal oMyPagamento As OggettoPagamento, ByVal nDBOperation As Integer) As Integer
        '{0= non a buon fine, >0= id tabella}
        Dim nMyReturn As Integer

        Try
            'valorizzo il CommandText:
            Select Case nDBOperation
                Case 0
                    cmdMyCommand.CommandText = "INSERT INTO TP_PAGAMENTI (IDIMPORTAZIONE , IDFLUSSO, IDENTE, IDCONTRIBUENTE, PROVENIENZA,"
                    cmdMyCommand.CommandText += " DATA_FATTURA, NUMERO_FATTURA, CODICE_BOLLETTINO, NUMERO_RATA,"
                    cmdMyCommand.CommandText += " DATA_PAGAMENTO, DATA_ACCREDITO, DIVISA, SEGNO, IMPORTO_PAGAMENTO,"
                    cmdMyCommand.CommandText += " TIPO_BOLLETTINO, TIPO_PAGAMENTO,"
                    cmdMyCommand.CommandText += " PROGRESSIVO_CARICAMENTO, PROGRESSIVO_SELEZIONE, CCBENEFICIARIO, UFFICIOSPORTELLO,"
                    cmdMyCommand.CommandText += " NOTE, FLAG_DETTAGLIO, OPERATORE, DATA_INSERIMENTO)"
                    cmdMyCommand.CommandText += " VALUES (@IDIMPORTAZIONE , @IDFLUSSO, @IDENTE, @IDCONTRIBUENTE, @PROVENIENZA,"
                    cmdMyCommand.CommandText += " @DATAFATTURA, @NUMERO_FATTURA, @CODICE_BOLLETTINO, @NUMERO_RATA,"
                    cmdMyCommand.CommandText += " @DATA_PAGAMENTO, @DATA_ACCREDITO, @DIVISA, @SEGNO, @IMPORTO_PAGAMENTO,"
                    cmdMyCommand.CommandText += " @TIPO_BOLLETTINO, @TIPO_PAGAMENTO,"
                    cmdMyCommand.CommandText += " @PROGRESSIVO_CARICAMENTO, @PROGRESSIVO_SELEZIONE, @CCBENEFICIARIO, @UFFICIOSPORTELLO,"
                    cmdMyCommand.CommandText += " @NOTE, @FLAGDETTAGLIO, @OPERATORE, @DATA_INSERIMENTO)"
                    cmdMyCommand.CommandText += " SELECT @@IDENTITY"
                    'valorizzo i parameters:
                    cmdMyCommand.Parameters.Clear()
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPAGAMENTO", SqlDbType.Int)).Value = oMyPagamento.ID
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDIMPORTAZIONE", SqlDbType.Int)).Value = oMyPagamento.IDImportazione
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSO", SqlDbType.Int)).Value = oMyPagamento.IDFlusso
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oMyPagamento.IDEnte
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = oMyPagamento.nCodUtente
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PROVENIENZA", SqlDbType.NVarChar)).Value = oMyPagamento.sProvenienza
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAFATTURA", SqlDbType.NVarChar)).Value = oReplace.GiraData(oMyPagamento.sDataFattura)
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO_FATTURA", SqlDbType.NVarChar)).Value = oMyPagamento.sNumeroFattura
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE_BOLLETTINO", SqlDbType.NVarChar)).Value = oMyPagamento.sCodBollettino
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO_RATA", SqlDbType.NVarChar)).Value = oMyPagamento.sNRata
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_PAGAMENTO", SqlDbType.NVarChar)).Value = oReplace.GiraData(oMyPagamento.sDataPagamento)
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_ACCREDITO", SqlDbType.NVarChar)).Value = oReplace.GiraData(oMyPagamento.sDataAccredito)
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DIVISA", SqlDbType.NVarChar)).Value = oMyPagamento.sDivisa
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SEGNO", SqlDbType.NVarChar)).Value = oMyPagamento.sSegno
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO_PAGAMENTO", SqlDbType.Float)).Value = oMyPagamento.ImportoPagamento
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPO_BOLLETTINO", SqlDbType.NVarChar)).Value = oMyPagamento.sTipoBollettino
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPO_PAGAMENTO", SqlDbType.NVarChar)).Value = oMyPagamento.sTipoPagamento
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PROGRESSIVO_CARICAMENTO", SqlDbType.NVarChar)).Value = oMyPagamento.sProgCaricamento
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PROGRESSIVO_SELEZIONE", SqlDbType.NVarChar)).Value = oMyPagamento.sProgSelezione
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CCBENEFICIARIO", SqlDbType.NVarChar)).Value = oMyPagamento.sCCBeneficiario
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@UFFICIOSPORTELLO", SqlDbType.NVarChar)).Value = oMyPagamento.sUfficioSportello
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOTE", SqlDbType.NVarChar)).Value = oMyPagamento.sNote
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FLAGDETTAGLIO", SqlDbType.Bit)).Value = CInt(oMyPagamento.bFlagDettaglio)
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@OPERATORE", SqlDbType.NVarChar)).Value = oMyPagamento.sOperatore
                    If oMyPagamento.tDataInsert = Date.MaxValue Then
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INSERIMENTO", SqlDbType.DateTime)).Value = Now
                    Else
                        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INSERIMENTO", SqlDbType.DateTime)).Value = oMyPagamento.tDataInsert
                    End If
                    'eseguo la query()
                    Dim DrReturn As SqlClient.SqlDataReader
                    DrReturn = iDB.GetDataReader(cmdMyCommand)
                    Do While DrReturn.Read
                        nMyReturn = DrReturn(0)
                    Loop
                    DrReturn.Close()
                Case 1
                    cmdMyCommand.CommandText = "UPDATE TP_PAGAMENTI"
                    cmdMyCommand.CommandText += " SET DATA_PAGAMENTO=@DATA_PAGAMENTO,"
                    cmdMyCommand.CommandText += " IMPORTO_PAGAMENTO=@IMPORTO_PAGAMENTO,"
                    cmdMyCommand.CommandText += " DATA_FATTURA=@DATAFATTURA,"
                    cmdMyCommand.CommandText += " NUMERO_FATTURA=@NUMERO_FATTURA,"
                    cmdMyCommand.CommandText += " NUMERO_RATA=@NUMERO_RATA,"
                    cmdMyCommand.CommandText += " DATA_ACCREDITO =@DATA_ACCREDITO"
                    cmdMyCommand.CommandText += " WHERE (ID=@IDPAGAMENTO)"
                    'valorizzo i parameters:
                    cmdMyCommand.Parameters.Clear()
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPAGAMENTO", SqlDbType.Int)).Value = oMyPagamento.ID
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAFATTURA", SqlDbType.NVarChar)).Value = oReplace.GiraData(oMyPagamento.sDataFattura)
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO_FATTURA", SqlDbType.NVarChar)).Value = oMyPagamento.sNumeroFattura
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO_RATA", SqlDbType.NVarChar)).Value = oMyPagamento.sNRata
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_PAGAMENTO", SqlDbType.NVarChar)).Value = oReplace.GiraData(oMyPagamento.sDataPagamento)
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_ACCREDITO", SqlDbType.NVarChar)).Value = oReplace.GiraData(oMyPagamento.sDataAccredito)
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO_PAGAMENTO", SqlDbType.Float)).Value = oMyPagamento.ImportoPagamento
                    nMyReturn = CInt(iDB.ExecuteNonQuery(cmdMyCommand))
                Case 2
                    cmdMyCommand.CommandText = "DELETE"
                    cmdMyCommand.CommandText += " FROM TP_PAGAMENTI"
                    cmdMyCommand.CommandText += " WHERE (IDIMPORTAZIONE=@IDIMPORTAZIONE)"
                    'valorizzo i parameters:
                    cmdMyCommand.Parameters.Clear()
                    nMyReturn = CInt(iDB.ExecuteNonQuery(cmdMyCommand))
                Case 3
                    cmdMyCommand.CommandText = "UPDATE TP_PAGAMENTI SET FLAG_DETTAGLIO=1"
                    cmdMyCommand.CommandText += " WHERE (ID=@IDPAGAMENTO)"
                    'valorizzo i parameters:
                    cmdMyCommand.Parameters.Clear()
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPAGAMENTO", SqlDbType.Int)).Value = oMyPagamento.ID
                    nMyReturn = CInt(iDB.ExecuteNonQuery(cmdMyCommand))
                Case 4
                    cmdMyCommand.CommandText = "UPDATE TP_PAGAMENTI SET FLAG_DETTAGLIO=0"
                    cmdMyCommand.CommandText += " WHERE (ID=@IDPAGAMENTO)"
                    'valorizzo i parameters:
                    cmdMyCommand.Parameters.Clear()
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPAGAMENTO", SqlDbType.Int)).Value = oMyPagamento.ID
                    nMyReturn = CInt(iDB.ExecuteNonQuery(cmdMyCommand))
            End Select
            Return nMyReturn
        Catch Err As Exception
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsPagamenti.SetPagamento.errore: ", Err)
            Log.Debug("Si è verificato un errore in ClsPagamenti::SetPagamento::" & Err.Message)

            Return -1
        End Try
    End Function
    ''' <summary>
    ''' Esegue l'eliminazione di un determinato pagamento.
    ''' </summary>
    ''' <param name="OBJ">oggetto di tipo OggettoPagamento</param>
    ''' <param name="strError">stringa</param>
    ''' <returns>Valore booleano (TRUE se l'inserimento è andato a buon fine, 
    ''' FALSE se l'inserimento non è andato a buon fine)
    ''' Se si verifica un errore viene restituito il messaggio tramite il parametro passato per referenza strError</returns>
    ''' <remarks>
    ''' </remarks>
    Public Function DeletePagamentiEnte(ByVal OBJ As OggettoPagamento, ByRef strError As String) As Boolean
        Dim myIdentity As Integer

        Try

            DeletePagamentiEnte = False
            Dim SQL As String

            'SE NON è STATA ASSOCIATA LA CANCELLO
            SQL = "DELETE FROM TP_PAGAMENTI"
            SQL += " WHERE IDENTE='" & ConstSession.IdEnte & "' AND ID=" & OBJ.ID & ""

            'eseguo la query
            myIdentity = CInt(iDB.ExecuteNonQuery(SQL))

            DeletePagamentiEnte = True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsPagamenti.DeletePagamentiEnte.errore: ", ex)
            Throw New Exception("Problemi nell'esecuzione di DeletePagamentiEnte " + ex.Message)
            DeletePagamentiEnte = False
        End Try
    End Function
#End Region


#Region "Importazione Flussi"
    'Public Function DettagliaPagamenti(ByVal sIdEnte As String, ByVal sOperatore As String, ByVal WFSessione As CreateSessione) As Integer
    '    '{-1= non a buon fine, 1= buon fine}
    '    Try
    '        Dim DvMyDati As DataView
    '        Dim DrMyEmesso As SqlClient.SqlDataReader
    '        Dim oMyDettaglioPag As ObjDettaglioPagamento
    '        Dim oListDettaglio() As ObjDettaglioPagamento
    '        Dim nPercentPag, nImpDettagliato, nImpDettaglio As Double
    '        Dim nList As Integer = -1
    '        Dim x As Integer

    '        'Prelevo tutti i pagamenti che non hanno il flag dettaglio uguale a 0 dalla tabella TBLPAGAMENTI richiamando la funzione GetPagamento(sIdEnte);
    '        DvMyDati = GetPagamento(sIdEnte, WFSessione)
    '        'Ciclo sui record trovati:
    '        For x = 0 To DvMyDati.Count - 1
    '            nImpDettagliato = 0 : nList = -1
    '            'calcolo la percentuale di pagato rispetto all’emesso tramite la proporzione (emesso:100=pagato:x)
    '            nPercentPag = ((DvMyDati.Item(x)("importo_pagamento") * 100) / DvMyDati.Item(x)("importo_carico"))
    '            'Prelevo il dettaglio dell’emesso in base a IDENTE e dati fattura
    '            DrMyEmesso = GetDettaglioEmesso(sIdEnte, DvMyDati.Item(x)("data_fattura"), DvMyDati.Item(x)("numero_fattura"), WFSessione)
    '            'Se il datareader è valorizzato:
    '            If Not IsNothing(DrMyEmesso) Then
    '                'ciclo sui record trovati:
    '                Do While DrMyEmesso.Read
    '                    'dichiaro un nuovo oggetto di tipo ObjDettaglioPagamento;
    '                    oMyDettaglioPag = New ObjDettaglioPagamento
    '                    'valorizzo l’oggetto di dettaglio pagamento;
    '                    oMyDettaglioPag.IDPagamento = DvMyDati.Item(x)("id")
    '                    oMyDettaglioPag.sCodCapitolo = DrMyEmesso("codice_capitolo")
    '                    oMyDettaglioPag.sCodVoce = DrMyEmesso("idvoce")
    '                    oMyDettaglioPag.sAnno = DrMyEmesso("anno_fattura")
    '                    oMyDettaglioPag.sDivisa = "E"
    '                    nImpDettaglio = FormatNumber(((DrMyEmesso("importo") * nPercentPag) / 100), 2)
    '                    If Math.Sign(nImpDettaglio) = -1 Then
    '                        oMyDettaglioPag.sSegno = "-"
    '                        'calcolo l’importo di scorporo del pagamento rispetto alla voce tramite la proporzione (dettaglio:100=x:percentuale di pagato);
    '                        oMyDettaglioPag.dImpDettaglio = Math.Abs(nImpDettaglio)
    '                    Else
    '                        oMyDettaglioPag.sSegno = "+"
    '                        oMyDettaglioPag.dImpDettaglio = nImpDettaglio
    '                    End If
    '                    oMyDettaglioPag.tDataInsert = Now
    '                    oMyDettaglioPag.sOperatore = sOperatore
    '                    'incremento l’array di dettaglio pagamento;
    '                    nList += 1
    '                    ReDim Preserve oListDettaglio(nList)
    '                    'inserisco nell’array la voce di dettaglio pagamento appena valorizzata;
    '                    oListDettaglio(nList) = oMyDettaglioPag
    '                    nImpDettagliato += FormatNumber(oMyDettaglioPag.dImpDettaglio, 2)
    '                Loop
    '                DrMyEmesso.Close()
    '                'se la somma delle voci dettagliate non corrisponde al pagato:
    '                If FormatNumber((DvMyDati.Item(x)("importo_pagamento") - nImpDettagliato), 2) <> "0,00" Then
    '                    'dichiaro un nuovo oggetto di tipo ObjDettaglioPagamento;
    '                    oMyDettaglioPag = New ObjDettaglioPagamento
    '                    'creo una voce di arrotondamento;
    '                    oMyDettaglioPag.IDPagamento = DvMyDati.Item(x)("id")
    '                    oMyDettaglioPag.sCodCapitolo = "9999"
    '                    oMyDettaglioPag.sCodVoce = ""
    '                    oMyDettaglioPag.sAnno = ""
    '                    oMyDettaglioPag.sDivisa = "E"
    '                    nImpDettaglio = FormatNumber((DvMyDati.Item(x)("importo_pagamento") - nImpDettagliato), 2)
    '                    If Math.Sign(nImpDettaglio) = -1 Then
    '                        oMyDettaglioPag.sSegno = "-"
    '                        'calcolo l’importo di scorporo del pagamento rispetto alla voce tramite la proporzione (dettaglio:100=x:percentuale di pagato);
    '                        oMyDettaglioPag.dImpDettaglio = Math.Abs(nImpDettaglio)
    '                    Else
    '                        oMyDettaglioPag.sSegno = "+"
    '                        oMyDettaglioPag.dImpDettaglio = nImpDettaglio
    '                    End If
    '                    oMyDettaglioPag.tDataInsert = Now
    '                    oMyDettaglioPag.sOperatore = sOperatore
    '                    'incremento l’array di dettaglio pagamento;
    '                    nList += 1
    '                    ReDim Preserve oListDettaglio(nList)
    '                    'inserisco nell’array la voce di dettaglio pagamento appena valorizzata;
    '                    oListDettaglio(nList) = oMyDettaglioPag
    '                End If
    '                'se l’inserimento del dettaglio pagamento sulla base dati richiamando la funzione SetPagamentoDettagliato(oListDettaglio) è andato a buon fine:
    '                If SetPagamentoDettagliato(oListDettaglio, WFSessione) > 0 Then
    '                    'registro l’avvenuto dettaglio del pagamento sulla base dati richiamando la funzione SetPagamento(2);
    '                    Dim oMyPag As New OggettoPagamento
    '                    oMyPag.ID = DvMyDati.Item(x)("id")
    '                    If SetPagamento(oMyPag, 3, WFSessione) <= 0 Then
    '                        Return 0
    '                    End If
    '                End If
    '            End If
    '        Next
    '        DvMyDati.Dispose()
    '        Return 1
    '    Catch Err As Exception
    '         Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsPagamenti.DettagliPagamenti.errore: ", ex) 
    '        Return -1
    '    End Try
    'End Function

    'Public Function GetPagamento(ByVal sIdEnte As String, ByVal WFSession As CreateSessione) As DataView
    '    Try
    '        Dim DvMyDati As DataView
    '        cmdMyCommand.Connection = WFSession.oSession.oAppDB.GetConnection
    '        'valorizzo il CommandText;
    '        cmdMyCommand.CommandText = "SELECT TP_PAGAMENTI.ID, TP_PAGAMENTI.IDENTE, TP_PAGAMENTI.DATA_FATTURA, TP_PAGAMENTI.NUMERO_FATTURA, (TP_PAGAMENTI.IMPORTO_PAGAMENTO*CAST(TP_PAGAMENTI.SEGNO+'1' AS NUMERIC)) AS IMPORTO_PAGAMENTO, TP_FATTURE_NOTE.IMPORTO_FATTURANOTA AS IMPORTO_CARICO"
    '        cmdMyCommand.CommandText += " FROM TP_PAGAMENTI INNER JOIN TP_FATTURE_NOTE ON TP_PAGAMENTI.IDENTE=TP_FATTURE_NOTE.IDENTE AND TP_PAGAMENTI.DATA_FATTURA=TP_FATTURE_NOTE.DATA_FATTURA AND TP_PAGAMENTI.NUMERO_FATTURA=TP_FATTURE_NOTE.NUMERO_FATTURA"
    '        cmdMyCommand.CommandText += " WHERE (FLAG_DETTAGLIO=0) AND (TP_PAGAMENTI.IDENTE=@IDENTE)"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE ", SqlDbType.NVarChar)).Value = sIdEnte
    '        DvMyDati = WFSession.oSession.oAppDB.GetPrivateDataview(cmdMyCommand)
    '        Return DvMyDati
    '    Catch Err As Exception
    '        Dim sValParametri As String = FncGen.GetValParamCmd(cmdMyCommand)
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsPagamenti.GetPagamento.errore: ", ex) 
    '        Log.Debug("Si è verificato un errore in ClsPagamenti::GetPagamento::" & Err.Message )
    '        
    '        Return Nothing
    '    End Try
    'End Function

    'Public Function GetDettaglioEmesso(ByVal sIdEnte As String, ByVal sDataFattura As String, ByVal sNumeroFattura As String, ByVal WFSession As CreateSessione) As SqlClient.SqlDataReader
    '    Try
    '        Dim DrMyDati As SqlClient.SqlDataReader
    '        'valorizzo il CommandText;
    '        cmdMyCommand.CommandText = "SELECT DATA_FATTURA, NUMERO_FATTURA, CODICE_CAPITOLO, IDVOCE, ANNO_FATTURA, IMPORTO"
    '        cmdMyCommand.CommandText += " FROM OPENgov_FATTURE_DETTAGLIOVOCI"
    '        cmdMyCommand.CommandText += " WHERE (IDENTE=@IDENTE) AND (DATA_FATTURA=@DATAFATTURA) AND (NUMERO_FATTURA=@NUMEROFATTURA)"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE ", SqlDbType.NVarChar)).Value = sIdEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAFATTURA ", SqlDbType.NVarChar)).Value = sDataFattura
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMEROFATTURA ", SqlDbType.NVarChar)).Value = sNumeroFattura
    '        DrMyDati = WFSession.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)

    '        Return DrMyDati
    '    Catch Err As Exception
    '         Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsPagamenti.GetDettagliEmessoerrore: ", ex) 
    '        Return Nothing
    '    End Try
    'End Function

    'Public Function SetPagamentoDettagliato(ByVal oListMyDettaglio() As ObjDettaglioPagamento, ByVal WFSession As CreateSessione) As Integer
    '    '{0= non a buon fine, >0= id tabella}
    '    Try
    '        Dim x As Integer
    '        'Ciclo sull'array di oggetti di dettaglio:
    '        For x = 0 To oListMyDettaglio.GetUpperBound(0)
    '            'Se l'inserimento su base dati richiamando la funzione SetDettaglioPagamento(oListDettaglio(x)) non è andato a buon fine:
    '            If SetDettaglioPagamento(oListMyDettaglio(x), 0, WFSession) <= 0 Then
    '                'Restituisco errore;
    '                Return 0
    '            End If
    '        Next
    '        Return 1
    '    Catch Err As Exception
    '        Dim sValParametri As String = FncGen.GetValParamCmd(cmdMyCommand)
    ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsPagamenti.SetPagamentoDettagliato.errore: ", ex) 
    '        Log.Debug("Si è verificato un errore in ClsPagamenti::SetPagamentoDettagliato::" & Err.Message )
    '        
    '        Return -1
    '    End Try
    'End Function

    'Public Function SetDettaglioPagamento(ByVal oMyDettaglio As ObjDettaglioPagamento, ByVal nDBOperation As Integer, ByVal WFSession As CreateSessione) As Integer
    '    Dim nMyReturn As Integer
    '    Try
    '        'valorizzo il CommandText;
    '        Select Case nDBOperation
    '            Case 0
    '                cmdMyCommand.CommandText = "INSERT INTO TP_PAGAMENTIDETTAGLIO"
    '                cmdMyCommand.CommandText += " (IDPAGAMENTO, COD_CAPITOLO, IDVOCE, SEGNO, IMPORTO, OPERATORE, DATA_INSERIMENTO)"
    '                cmdMyCommand.CommandText += " VALUES (@IDPAGAMENTO, @CODICECAPITOLO, @IDVOCE, @SEGNO, @IMPORTO, @OPERATORE, @DATAINSERIMENTO)"
    '                cmdMyCommand.CommandText += " SELECT @@IDENTITY"
    '                'Valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPAGAMENTO", SqlDbType.Int)).Value = oMyDettaglio.IDPagamento
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICECAPITOLO", SqlDbType.NVarChar)).Value = oMyDettaglio.sCodCapitolo
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDVOCE", SqlDbType.NVarChar)).Value = oMyDettaglio.sCodVoce
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SEGNO", SqlDbType.NVarChar)).Value = oMyDettaglio.sSegno
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO", SqlDbType.Float)).Value = oMyDettaglio.dImpDettaglio
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@OPERATORE", SqlDbType.NVarChar)).Value = oMyDettaglio.sOperatore
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAINSERIMENTO", SqlDbType.DateTime)).Value = oMyDettaglio.tDataInsert
    '                'eseguo la query
    '                Dim DrReturn As SqlClient.SqlDataReader
    '                DrReturn = WFSession.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '                Do While DrReturn.Read
    '                    nMyReturn = DrReturn(0)
    '                Loop
    '                DrReturn.Close()
    '            Case 2
    '                cmdMyCommand.CommandText = "DELETE"
    '                cmdMyCommand.CommandText += " FROM TP_PAGAMENTIDETTAGLIO"
    '                cmdMyCommand.CommandText += " WHERE (IDPAGAMENTO=@IDPAGAMENTO)"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPAGAMENTO", SqlDbType.Int)).Value = oMyDettaglio.IDPagamento
    '                nMyReturn = CInt(WFSession.oSession.oAppDB.Execute(cmdMyCommand))
    '        End Select
    '        Return nMyReturn
    '    Catch Err As Exception
    '        Dim sValParametri As String = FncGen.GetValParamCmd(cmdMyCommand)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsPagamenti.SetDettaglioPagamento.errore: ", ex) 
    '        Log.Debug("Si è verificato un errore in ClsPagamenti::SetDettaglioPagamento::" & Err.Message )
    '        
    '        Return -1
    '    End Try
    'End Function

    'Public Function MaxIdImport(ByVal sIdEnte As String, ByVal WFSession As CreateSessione) As Integer
    '    Dim DrReturn As SqlClient.SqlDataReader
    '    Dim myIdentity As Integer = -1

    '    Try
    '        cmdMyCommand.CommandText = "SELECT MAX(ID) AS MAXID"
    '        cmdMyCommand.CommandText += " FROM TBLIMPORTPAGAMENTI"
    '        cmdMyCommand.CommandText += " WHERE (TBLIMPORTPAGAMENTI.IDENTE = @IDENTE)"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE ", SqlDbType.NVarChar)).Value = sIdEnte
    '        DrReturn = WFSession.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '        Do While DrReturn.Read
    '            If Not IsDBNull(DrReturn("maxid")) Then
    '                myIdentity = CInt(DrReturn("maxid"))
    '            End If
    '        Loop
    '        Return myIdentity
    '    Catch Err As Exception
    '        Dim sValParametri As String = FncGen.GetValParamCmd(cmdMyCommand)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsPagamenti.MaxIdImport.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in ClsPagamenti::MaxIdImport::" & Err.Message )

    '        Return -1
    '    Finally
    '        DrReturn.Close()
    '    End Try
    'End Function

    'Public Function GetAcquisizione(ByVal sIDEnte As String, ByVal nStato As Integer, ByVal WFSession As CreateSessione) As ObjImportPagamenti
    '    'STATO_IMPORTAZIONE: {1=in corso, 0=finita correttamente, -1= finita con errori}
    '    Dim oMyImportPag As New ObjImportPagamenti
    '    Dim DrReturn As SqlClient.SqlDataReader

    '    Try
    '        'valorizzo il commandtext:
    '        cmdMyCommand.CommandText = "SELECT TOP 1 *"
    '        cmdMyCommand.CommandText += " FROM TBLIMPORTPAGAMENTI"
    '        cmdMyCommand.CommandText += " WHERE (TBLIMPORTPAGAMENTI.IDENTE = @IDENTE)"
    '        cmdMyCommand.CommandText += " ORDER BY ID DESC"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE ", SqlDbType.NVarChar)).Value = sIDEnte
    '        DrReturn = WFSession.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '        Do While DrReturn.Read
    '            If CInt(DrReturn("stato_importazione")) = nStato Or nStato <> 1 Then
    '                oMyImportPag.Id = CInt(DrReturn("id"))
    '                oMyImportPag.IdEnte = CStr(DrReturn("idente"))
    '                oMyImportPag.sFileAcq = CStr(DrReturn("file_import"))
    '                oMyImportPag.nStatoAcq = CInt(DrReturn("stato_importazione"))
    '                oMyImportPag.sEsito = CStr(DrReturn("esito"))
    '                oMyImportPag.sFileScarti = CStr(DrReturn("file_scarti"))
    '                oMyImportPag.nRcDaAcquisire = CInt(DrReturn("rcdaacquisire"))
    '                oMyImportPag.nRcAcquisiti = CInt(DrReturn("rcacquisiti"))
    '                oMyImportPag.nRcScarti = CInt(DrReturn("rcscartati"))
    '                oMyImportPag.impDaAcquisire = CDbl(DrReturn("impdaacquisire"))
    '                oMyImportPag.impAcquisiti = CDbl(DrReturn("impacquisiti"))
    '                oMyImportPag.impScarti = CDbl(DrReturn("impscartati"))
    '                oMyImportPag.tDataAcq = CDate(DrReturn("data_import"))
    '                oMyImportPag.sOperatore = CStr(DrReturn("operatore"))
    '            End If
    '        Loop
    '        Return oMyImportPag
    '    Catch Err As Exception
    '        Dim sValParametri As String = FncGen.GetValParamCmd(cmdMyCommand)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsPagamenti.GetAcquisizone.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in ClsPagamenti::GetAcquisizione::" & Err.Message )

    '        Return Nothing
    '    Finally
    '        DrReturn.Close()
    '    End Try
    'End Function

    'Public Function AbbinaBollettino(ByRef oMyPagamento As OggettoPagamento, ByVal WFSession As CreateSessione) As Boolean
    '    '{true= abbinamento trovato; false= abbinamento non trovato }
    '    Dim DrMyDati As SqlClient.SqlDataReader
    '    Dim bMyReturn As Boolean = False

    '    Try
    '        'valorizzo il commandtext:
    '        cmdMyCommand.CommandText = "SELECT TP_FATTURE_NOTE.COD_UTENTE, TP_FATTURE_NOTE.NUMERO_FATTURA, TP_FATTURE_NOTE.DATA_FATTURA, TP_FATTURE_RATE.NUMERO_RATA"
    '        cmdMyCommand.CommandText += " FROM TP_FATTURE_NOTE"
    '        cmdMyCommand.CommandText += " INNER JOIN TP_FATTURE_RATE ON TP_FATTURE_NOTE.IDENTE=TP_FATTURE_RATE.IDENTE AND TP_FATTURE_NOTE.DATA_FATTURA = TP_FATTURE_RATE.DATA_FATTURA AND TP_FATTURE_NOTE.NUMERO_FATTURA = TP_FATTURE_RATE.NUMERO_FATTURA"
    '        cmdMyCommand.CommandText += " WHERE (SUBSTRING(TP_FATTURE_RATE.CODICE_BOLLETTINO,1,16)=@CODBOLLETTINO)"
    '        cmdMyCommand.CommandText += " AND (TP_FATTURE_NOTE.IDENTE=@IDENTE)"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE ", SqlDbType.NVarChar)).Value = oMyPagamento.IDEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODBOLLETTINO ", SqlDbType.NVarChar)).Value = oMyPagamento.sCodBollettino
    '        DrMyDati = WFSession.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '        Do While DrMyDati.Read
    '            oMyPagamento.nCodUtente = CInt(DrMyDati("cod_utente"))
    '            oMyPagamento.sNumeroFattura = CStr(DrMyDati("numero_fattura"))
    '            oMyPagamento.sDataFattura = CStr(DrMyDati("data_fattura"))
    '            oMyPagamento.sNRata = CStr(DrMyDati("numero_rata"))
    '            bMyReturn = True
    '        Loop
    '    Catch Err As Exception
    '        Dim sValParametri As String = FncGen.GetValParamCmd(cmdMyCommand)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsPagamenti.AbbinaBollettino.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in ClsPagamenti::AbbinaBollettino::" & Err.Message )
    '    Finally
    '        DrMyDati.Close()
    '    End Try
    '    Return bMyReturn
    'End Function

    'Public Function SetAcquisizione(ByVal oMyImport As ObjImportPagamenti, ByVal nDBOperation As Integer, ByVal WFSession As CreateSessione) As Integer
    '    Dim nMyReturn As Integer

    '    Try
    '        'Valorizzo la connessione
    '        cmdMyCommand.Connection = WFSession.oSession.oAppDB.GetConnection
    '        'Valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDIMPORT", SqlDbType.NVarChar)).Value = oMyImport.Id
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oMyImport.IdEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FILE_IMPORT", SqlDbType.NVarChar)).Value = oMyImport.sFileAcq
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@STATO_IMPORTAZIONE", SqlDbType.Int)).Value = oMyImport.nStatoAcq
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ESITO", SqlDbType.NVarChar)).Value = oMyImport.sEsito
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FILE_SCARTI", SqlDbType.NVarChar)).Value = oMyImport.sFileScarti
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NPAGAMENTI_FILE", SqlDbType.Int)).Value = oMyImport.nRcDaAcquisire
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NPAGAMENTI_IMPORT", SqlDbType.Int)).Value = oMyImport.nRcAcquisiti
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NPAGAMENTI_SCARTATI", SqlDbType.Int)).Value = oMyImport.nRcScarti
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPPAGAMENTI_FILE", SqlDbType.Float)).Value = oMyImport.impDaAcquisire
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPPAGAMENTI_IMPORT", SqlDbType.Float)).Value = oMyImport.impAcquisiti
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPPAGAMENTI_SCARTATI", SqlDbType.Float)).Value = oMyImport.impScarti
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_IMPORT", SqlDbType.DateTime)).Value = oMyImport.tDataAcq
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@OPERATORE", SqlDbType.NVarChar)).Value = oMyImport.sOperatore
    '        'Valorizzo il commandtext:
    '        Select Case nDBOperation
    '            Case 0
    '                cmdMyCommand.CommandText = "INSERT INTO TBLIMPORTPAGAMENTI (IDENTE, FILE_IMPORT, STATO_IMPORTAZIONE, ESITO, FILE_SCARTI,"
    '                cmdMyCommand.CommandText += " RCDAACQUISIRE, RCACQUISITI, RCSCARTATI,"
    '                cmdMyCommand.CommandText += " IMPDAACQUISIRE, IMPACQUISITI, IMPSCARTATI,"
    '                cmdMyCommand.CommandText += " DATA_IMPORT, OPERATORE)"
    '                cmdMyCommand.CommandText += " VALUES (@IDENTE, @FILE_IMPORT, @STATO_IMPORTAZIONE, @ESITO, @FILE_SCARTI,"
    '                cmdMyCommand.CommandText += " @NPAGAMENTI_FILE, @NPAGAMENTI_IMPORT, @NPAGAMENTI_SCARTATI,"
    '                cmdMyCommand.CommandText += " @IMPPAGAMENTI_FILE, @IMPPAGAMENTI_IMPORT, @IMPPAGAMENTI_SCARTATI,"
    '                cmdMyCommand.CommandText += " @DATA_IMPORT, @OPERATORE)"
    '                cmdMyCommand.CommandText += " SELECT @@IDENTITY"
    '                'eseguo la query
    '                Dim DrReturn As SqlClient.SqlDataReader
    '                DrReturn = WFSession.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '                Do While DrReturn.Read
    '                    nMyReturn = DrReturn(0)
    '                Loop
    '                DrReturn.Close()
    '            Case 1
    '                cmdMyCommand.CommandText = "UPDATE TBLIMPORTPAGAMENTI SET IDENTE=@IDENTE, FILE_IMPORT=@FILE_IMPORT,"
    '                cmdMyCommand.CommandText += " STATO_IMPORTAZIONE=@STATO_IMPORTAZIONE, ESITO=@ESITO, FILE_SCARTI=@FILE_SCARTI, "
    '                cmdMyCommand.CommandText += " RCDAACQUISIRE=@NPAGAMENTI_FILE, RCACQUISITI=@NPAGAMENTI_IMPORT, RCSCARTATI=@NPAGAMENTI_SCARTATI,"
    '                cmdMyCommand.CommandText += " IMPDAACQUISIRE=@IMPPAGAMENTI_FILE, IMPACQUISITI=@IMPPAGAMENTI_IMPORT, IMPSCARTATI=@IMPPAGAMENTI_SCARTATI,"
    '                cmdMyCommand.CommandText += " DATA_IMPORT=@DATA_IMPORT, OPERATORE=@OPERATORE"
    '                cmdMyCommand.CommandText += " WHERE (TBLIMPORTPAGAMENTI.ID=@IDIMPORT)"
    '                nMyReturn = WFSession.oSession.oAppDB.Execute(cmdMyCommand)
    '            Case 2
    '                cmdMyCommand.CommandText = "DELETE"
    '                cmdMyCommand.CommandText += " FROM TBLIMPORTPAGAMENTI"
    '                cmdMyCommand.CommandText += " WHERE (TBLIMPORTPAGAMENTI.ID=@IDIMPORT)"
    '                nMyReturn = WFSession.oSession.oAppDB.Execute(cmdMyCommand)
    '        End Select

    '        Return nMyReturn
    '    Catch Err As Exception
    '        Dim sValParametri As String = FncGen.GetValParamCmd(cmdMyCommand)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsPagamenti.SetAcquisizione.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in ClsPagamenti::SetAcquisizione::" & Err.Message )

    '        Return -1
    '    End Try
    'End Function

    'Public Function GetCondizioniScarto(ByVal sContoCorrente As String, ByVal WFSession As CreateSessione) As ObjCondizioniScarto()
    '    Dim oListCondizioni() As ObjCondizioniScarto
    '    Dim oMyCondizione As ObjCondizioniScarto
    '    Dim DrMyDati As SqlClient.SqlDataReader
    '    Dim nList As Integer = -1

    '    Try
    '        'prelevo le condizioni di scarto dal database
    '        'valorizzo il commandtext:
    '        cmdMyCommand.CommandText = "SELECT *"
    '        cmdMyCommand.CommandText += " FROM TBLCONFIGURAZIONEIMPORTPAGAMENTI"
    '        cmdMyCommand.CommandText += " WHERE (TBLCONFIGURAZIONEIMPORTPAGAMENTI.NOME_FILE=@CONTOCORRENTE)"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CONTOCORRENTE ", SqlDbType.NVarChar)).Value = sContoCorrente
    '        DrMyDati = WFSession.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '        Do While DrMyDati.Read
    '            oMyCondizione = New ObjCondizioniScarto
    '            oMyCondizione.sTipoBollettino = CStr(DrMyDati("tipo_bollettino"))
    '            oMyCondizione.sTipoScarto = CStr(DrMyDati("tipo_scarto"))
    '            'incremento l’array
    '            nList += 1
    '            ReDim Preserve oListCondizioni(nList)
    '            oListCondizioni(nList) = oMyCondizione
    '        Loop
    '        Return oListCondizioni
    '    Catch Err As Exception
    '        Dim sValParametri As String = FncGen.GetValParamCmd(cmdMyCommand)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsPagamenti.GetCondizioniScarto.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in ClsPagamenti::GetCondizioniScarto::" & Err.Message )

    '        Return Nothing
    '    Finally
    '        DrMyDati.Close()
    '    End Try
    'End Function

    'Public Function GetFlussi(ByVal sFileImport As String, ByVal WFSession As CreateSessione) As ObjFlussoPagamenti()
    '    Dim oListFlussi() As ObjFlussoPagamenti
    '    Dim oMyFlusso As ObjFlussoPagamenti
    '    Dim DrMyDati As SqlClient.SqlDataReader
    '    Dim nList As Integer = -1

    '    Try
    '        'valorizzo il commandtext:
    '        cmdMyCommand.CommandText = "SELECT *"
    '        cmdMyCommand.CommandText += " FROM TBLDESCRIZIONEFLUSSIPAG"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        If sFileImport <> "" Then
    '            cmdMyCommand.CommandText += " WHERE (TBLDESCRIZIONEFLUSSIPAG.NOME_FILE=@NOMEFILE)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOMEFILE ", SqlDbType.NVarChar)).Value = sFileImport
    '        End If
    '        DrMyDati = WFSession.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '        Do While DrMyDati.Read
    '            oMyFlusso = New ObjFlussoPagamenti
    '            oMyFlusso.ID = CInt(DrMyDati("id"))
    '            oMyFlusso.IDEnte = CStr(DrMyDati("idente"))
    '            oMyFlusso.IDImportazione = CInt(DrMyDati("idimport"))
    '            oMyFlusso.impAcquisiti = CDbl(DrMyDati("totale_importi"))
    '            oMyFlusso.impPagamentiErrati = CDbl(DrMyDati("totale_importi_errati"))
    '            oMyFlusso.impPagamentiEsatti = CDbl(DrMyDati("totale_importi_esatti"))
    '            oMyFlusso.nPagamentiErrati = CInt(DrMyDati("totale_documenti_errati"))
    '            oMyFlusso.nPagamentiEsatti = CInt(DrMyDati("totale_documenti_esatti"))
    '            oMyFlusso.nRcAcquisiti = CInt(DrMyDati("numero_pagamenti"))
    '            oMyFlusso.sCodCUAS = CStr(DrMyDati("codice_cuas"))
    '            oMyFlusso.sContoCorrente = CStr(DrMyDati("numero_cc"))
    '            oMyFlusso.sDivisa = CStr(DrMyDati("divisa"))
    '            oMyFlusso.sFileAcq = CStr(DrMyDati("nome_file"))
    '            oMyFlusso.sOperatore = CStr(DrMyDati("operatore"))
    '            oMyFlusso.sProvenienza = CStr(DrMyDati("provenienza"))
    '            oMyFlusso.tDataAcq = CDate(DrMyDati("data_import"))
    '            'incremento l’array
    '            nList += 1
    '            ReDim Preserve oListFlussi(nList)
    '            oListFlussi(nList) = oMyFlusso
    '        Loop
    '        Return oListFlussi
    '    Catch Err As Exception
    '        Dim sValParametri As String = FncGen.GetValParamCmd(cmdMyCommand)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsPagamenti.GetFlussi.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in ClsPagamenti::GetFlussi::" & Err.Message )

    '        Return Nothing
    '    Finally
    '        DrMyDati.Close()
    '    End Try
    'End Function

    'Public Function GetNewIdFlusso(ByVal sMyIdEnte As String, ByVal WFSession As CreateSessione) As Integer
    '    Dim DrMyDati As SqlClient.SqlDataReader
    '    Dim nMyReturn As Integer = 0

    '    Try
    '        'valorizzo il commandtext:
    '        cmdMyCommand.CommandText = "SELECT TOP 1 ID"
    '        cmdMyCommand.CommandText += " FROM TBLDESCRIZIONEFLUSSIPAG"
    '        cmdMyCommand.CommandText += " WHERE (IDENTE =@IDENTE)"
    '        cmdMyCommand.CommandText += " ORDER BY ID DESC"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sMyIdEnte
    '        DrMyDati = WFSession.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '        Do While DrMyDati.Read
    '            nMyReturn = CInt(DrMyDati("id"))
    '        Loop
    '        nMyReturn += 1
    '        Return nMyReturn
    '    Catch Err As Exception
    '        Dim sValParametri As String = FncGen.GetValParamCmd(cmdMyCommand)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsPagamenti.GetNewIdFlusso.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in ClsPagamenti::GetNewIdFlusso::" & Err.Message )

    '        Return -1
    '    Finally
    '        DrMyDati.Close()
    '    End Try
    'End Function

    'Public Function SetFlusso(ByVal oMyFlusso As ObjFlussoPagamenti, ByVal nDBOperation As Integer, ByVal WFSession As CreateSessione) As Integer
    '    Dim nMyReturn As Integer

    '    Try
    '        'Valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSO", SqlDbType.Int)).Value = oMyFlusso.ID
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDIMPORT", SqlDbType.Int)).Value = oMyFlusso.IDImportazione
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oMyFlusso.IDEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOME_FILE", SqlDbType.NVarChar)).Value = oMyFlusso.sFileAcq
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PROVENIENZA", SqlDbType.NVarChar)).Value = oMyFlusso.sProvenienza
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DIVISA", SqlDbType.NVarChar)).Value = oMyFlusso.sDivisa
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO_PAGAMENTI", SqlDbType.Int)).Value = oMyFlusso.nRcAcquisiti
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TOTALE_IMPORTI", SqlDbType.Float)).Value = oMyFlusso.impAcquisiti
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE_CUAS", SqlDbType.NVarChar)).Value = oMyFlusso.sCodCUAS
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO_CC", SqlDbType.NVarChar)).Value = oMyFlusso.sContoCorrente
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TOTALE_DOCUMENTI_ESATTI", SqlDbType.Int)).Value = oMyFlusso.nPagamentiEsatti
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TOTALE_IMPORTI_ESATTI", SqlDbType.Float)).Value = oMyFlusso.impPagamentiEsatti
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TOTALE_DOCUMENTI_ERRATI", SqlDbType.Int)).Value = oMyFlusso.nPagamentiErrati
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TOTALE_IMPORTI_ERRATI", SqlDbType.Float)).Value = oMyFlusso.impPagamentiErrati
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_IMPORT", SqlDbType.DateTime)).Value = oMyFlusso.tDataAcq
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@OPERATORE", SqlDbType.NVarChar)).Value = oMyFlusso.sOperatore
    '        Select Case nDBOperation
    '            Case 0
    '                'valorizzo il commandtext:
    '                cmdMyCommand.CommandText = "INSERT INTO TBLDESCRIZIONEFLUSSIPAG"
    '                cmdMyCommand.CommandText += "(ID, IDIMPORT, IDENTE, NOME_FILE, PROVENIENZA, DIVISA,"
    '                cmdMyCommand.CommandText += " NUMERO_PAGAMENTI, TOTALE_IMPORTI, CODICE_CUAS, NUMERO_CC,"
    '                cmdMyCommand.CommandText += " TOTALE_DOCUMENTI_ESATTI, TOTALE_IMPORTI_ESATTI, TOTALE_DOCUMENTI_ERRATI, TOTALE_IMPORTI_ERRATI,"
    '                cmdMyCommand.CommandText += " DATA_IMPORT, OPERATORE)"
    '                cmdMyCommand.CommandText += " VALUES (@IDFLUSSO, @IDIMPORT, @IDENTE, @NOME_FILE, @PROVENIENZA, @DIVISA,"
    '                cmdMyCommand.CommandText += " @NUMERO_PAGAMENTI, @TOTALE_IMPORTI, @CODICE_CUAS, @NUMERO_CC,"
    '                cmdMyCommand.CommandText += " @TOTALE_DOCUMENTI_ESATTI, @TOTALE_IMPORTI_ESATTI, @TOTALE_DOCUMENTI_ERRATI, @TOTALE_IMPORTI_ERRATI,"
    '                cmdMyCommand.CommandText += " @DATA_IMPORT, @OPERATORE)"
    '                nMyReturn = WFSession.oSession.oAppDB.Execute(cmdMyCommand)
    '            Case 2
    '                'valorizzo il commandtext:
    '                cmdMyCommand.CommandText = "DELETE"
    '                cmdMyCommand.CommandText += " FROM TBLDESCRIZIONEFLUSSIPAG"
    '                cmdMyCommand.CommandText += " WHERE (TBLDESCRIZIONEFLUSSIPAG.IDIMPORT=@IDIMPORT)"
    '                nMyReturn = WFSession.oSession.oAppDB.Execute(cmdMyCommand)
    '        End Select
    '        Return nMyReturn
    '    Catch Err As Exception
    '        Dim sValParametri As String = FncGen.GetValParamCmd(cmdMyCommand)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsPagamenti.SetFlusso.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in ClsPagamenti::SetFlusso::" & Err.Message )

    '        Return -1
    '    End Try
    'End Function

    Public Function GetPagamento(ByVal sIdEnte As String) As DataView
        Try
            Dim DvMyDati As DataView
            cmdMyCommand.Connection = New SqlConnection(ConstSession.StringConnection)
            'valorizzo il CommandText;
            cmdMyCommand.CommandText = "SELECT TP_PAGAMENTI.ID, TP_PAGAMENTI.IDENTE, TP_PAGAMENTI.DATA_FATTURA, TP_PAGAMENTI.NUMERO_FATTURA, (TP_PAGAMENTI.IMPORTO_PAGAMENTO*CAST(TP_PAGAMENTI.SEGNO+'1' AS NUMERIC)) AS IMPORTO_PAGAMENTO, TP_FATTURE_NOTE.IMPORTO_FATTURANOTA AS IMPORTO_CARICO"
            cmdMyCommand.CommandText += " FROM TP_PAGAMENTI INNER JOIN TP_FATTURE_NOTE ON TP_PAGAMENTI.IDENTE=TP_FATTURE_NOTE.IDENTE AND TP_PAGAMENTI.DATA_FATTURA=TP_FATTURE_NOTE.DATA_FATTURA AND TP_PAGAMENTI.NUMERO_FATTURA=TP_FATTURE_NOTE.NUMERO_FATTURA"
            cmdMyCommand.CommandText += " WHERE (FLAG_DETTAGLIO=0) AND (TP_PAGAMENTI.IDENTE=@IDENTE)"
            'valorizzo i parameters:
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE ", SqlDbType.NVarChar)).Value = sIdEnte
            DvMyDati = iDB.GetDataView(cmdMyCommand)
            Return DvMyDati
        Catch Err As Exception
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsPagamenti.GetPagamento.errore: ", Err)
            Log.Debug("Si è verificato un errore in ClsPagamenti::GetPagamento::" & Err.Message)

            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' La funzione preleva tutti i pagamenti per l'ente in ingresso non ancora dettagliati e ne procedere allo scorporo proporzionale rispetto alle voci di dettaglio
    ''' dell'emesso.
    ''' </summary>
    ''' <param name="sIdEnte">stringa contiene l'identificativo dell'ente in esame.</param>
    ''' <param name="sOperatore">stringa contiene l'identificativo dell'operatore che sta eseguendo l'acquisizione.</param>
    ''' <returns>Integer<br/>
    ''' Il metodo restituisce <c>1</c> se l'importazione è terminata con successo; mentre restituisce <c>0</c> se si è verificato un errore.
    ''' </returns>
    ''' <remarks>
    ''' Lo scorporo del pagamento utilizza la seguente proporzione:
    ''' EMESSO : 100 = X : PERCENTUALE DI PAGATO
    ''' </remarks>
    Public Function DettagliaPagamenti(ByVal sIdEnte As String, ByVal sOperatore As String) As Integer
        '{-1= non a buon fine, 1= buon fine}
        Try
            Dim DvMyDati As DataView
            Dim DrMyEmesso As SqlClient.SqlDataReader
            Dim oMyDettaglioPag As ObjDettaglioPagamento
            Dim oListDettaglio() As ObjDettaglioPagamento
            Dim nPercentPag, nImpDettagliato, nImpDettaglio As Double
            Dim nList As Integer = -1
            Dim x As Integer

            oListDettaglio = Nothing
            'Prelevo tutti i pagamenti che non hanno il flag dettaglio uguale a 0 dalla tabella TBLPAGAMENTI richiamando la funzione GetPagamento(sIdEnte);
            DvMyDati = GetPagamento(sIdEnte)
            'Ciclo sui record trovati:
            For x = 0 To DvMyDati.Count - 1
                nImpDettagliato = 0 : nList = -1
                'calcolo la percentuale di pagato rispetto all’emesso tramite la proporzione (emesso:100=pagato:x)
                nPercentPag = ((DvMyDati.Item(x)("importo_pagamento") * 100) / DvMyDati.Item(x)("importo_carico"))
                'Prelevo il dettaglio dell’emesso in base a IDENTE e dati fattura
                DrMyEmesso = GetDettaglioEmesso(sIdEnte, DvMyDati.Item(x)("data_fattura"), DvMyDati.Item(x)("numero_fattura"))
                'Se il datareader è valorizzato:
                If Not IsNothing(DrMyEmesso) Then
                    'ciclo sui record trovati:
                    Do While DrMyEmesso.Read
                        'dichiaro un nuovo oggetto di tipo ObjDettaglioPagamento;
                        oMyDettaglioPag = New ObjDettaglioPagamento
                        'valorizzo l’oggetto di dettaglio pagamento;
                        oMyDettaglioPag.IDPagamento = DvMyDati.Item(x)("id")
                        oMyDettaglioPag.sCodCapitolo = DrMyEmesso("codice_capitolo")
                        oMyDettaglioPag.sCodVoce = DrMyEmesso("idvoce")
                        oMyDettaglioPag.sAnno = DrMyEmesso("anno_fattura")
                        oMyDettaglioPag.sDivisa = "E"
                        nImpDettaglio = FormatNumber(((DrMyEmesso("importo") * nPercentPag) / 100), 2)
                        If Math.Sign(nImpDettaglio) = -1 Then
                            oMyDettaglioPag.sSegno = "-"
                            'calcolo l’importo di scorporo del pagamento rispetto alla voce tramite la proporzione (dettaglio:100=x:percentuale di pagato);
                            oMyDettaglioPag.dImpDettaglio = Math.Abs(nImpDettaglio)
                        Else
                            oMyDettaglioPag.sSegno = "+"
                            oMyDettaglioPag.dImpDettaglio = nImpDettaglio
                        End If
                        oMyDettaglioPag.tDataInsert = Now
                        oMyDettaglioPag.sOperatore = sOperatore
                        'incremento l’array di dettaglio pagamento;
                        nList += 1
                        ReDim Preserve oListDettaglio(nList)
                        'inserisco nell’array la voce di dettaglio pagamento appena valorizzata;
                        oListDettaglio(nList) = oMyDettaglioPag
                        nImpDettagliato += FormatNumber(oMyDettaglioPag.dImpDettaglio, 2)
                    Loop
                    DrMyEmesso.Close()
                    'se la somma delle voci dettagliate non corrisponde al pagato:
                    If FormatNumber((DvMyDati.Item(x)("importo_pagamento") - nImpDettagliato), 2) <> "0,00" Then
                        'dichiaro un nuovo oggetto di tipo ObjDettaglioPagamento;
                        oMyDettaglioPag = New ObjDettaglioPagamento
                        'creo una voce di arrotondamento;
                        oMyDettaglioPag.IDPagamento = DvMyDati.Item(x)("id")
                        oMyDettaglioPag.sCodCapitolo = "9999"
                        oMyDettaglioPag.sCodVoce = ""
                        oMyDettaglioPag.sAnno = ""
                        oMyDettaglioPag.sDivisa = "E"
                        nImpDettaglio = FormatNumber((DvMyDati.Item(x)("importo_pagamento") - nImpDettagliato), 2)
                        If Math.Sign(nImpDettaglio) = -1 Then
                            oMyDettaglioPag.sSegno = "-"
                            'calcolo l’importo di scorporo del pagamento rispetto alla voce tramite la proporzione (dettaglio:100=x:percentuale di pagato);
                            oMyDettaglioPag.dImpDettaglio = Math.Abs(nImpDettaglio)
                        Else
                            oMyDettaglioPag.sSegno = "+"
                            oMyDettaglioPag.dImpDettaglio = nImpDettaglio
                        End If
                        oMyDettaglioPag.tDataInsert = Now
                        oMyDettaglioPag.sOperatore = sOperatore
                        'incremento l’array di dettaglio pagamento;
                        nList += 1
                        ReDim Preserve oListDettaglio(nList)
                        'inserisco nell’array la voce di dettaglio pagamento appena valorizzata;
                        oListDettaglio(nList) = oMyDettaglioPag
                    End If
                    'se l’inserimento del dettaglio pagamento sulla base dati richiamando la funzione SetPagamentoDettagliato(oListDettaglio) è andato a buon fine:
                    If SetPagamentoDettagliato(oListDettaglio) > 0 Then
                        'registro l’avvenuto dettaglio del pagamento sulla base dati richiamando la funzione SetPagamento(2);
                        Dim oMyPag As New OggettoPagamento
                        oMyPag.ID = DvMyDati.Item(x)("id")
                        If SetPagamento(oMyPag, 3) <= 0 Then
                            Return 0
                        End If
                    End If
                End If
            Next
            DvMyDati.Dispose()
            Return 1
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsPagamenti.DettagliPagamenti.errore: ", Err)
            Return -1
        End Try
    End Function

    Public Function GetDettaglioEmesso(ByVal sIdEnte As String, ByVal sDataFattura As String, ByVal sNumeroFattura As String) As SqlClient.SqlDataReader
        Try
            Dim DrMyDati As SqlClient.SqlDataReader
            'valorizzo il CommandText;
            cmdMyCommand.CommandText = "SELECT DATA_FATTURA, NUMERO_FATTURA, CODICE_CAPITOLO, IDVOCE, ANNO_FATTURA, IMPORTO"
            cmdMyCommand.CommandText += " FROM OPENgov_FATTURE_DETTAGLIOVOCI"
            cmdMyCommand.CommandText += " WHERE (IDENTE=@IDENTE) AND (DATA_FATTURA=@DATAFATTURA) AND (NUMERO_FATTURA=@NUMEROFATTURA)"
            'valorizzo i parameters:
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE ", SqlDbType.NVarChar)).Value = sIdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAFATTURA ", SqlDbType.NVarChar)).Value = sDataFattura
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMEROFATTURA ", SqlDbType.NVarChar)).Value = sNumeroFattura
            DrMyDati = iDB.GetDataReader(cmdMyCommand)

            Return DrMyDati
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsPagamenti.GetDettaglioEmesso.errore: ", Err)
            Return Nothing
        End Try
    End Function

    Public Function SetPagamentoDettagliato(ByVal oListMyDettaglio() As ObjDettaglioPagamento) As Integer
        '{0= non a buon fine, >0= id tabella}
        Try
            Dim x As Integer
            'Ciclo sull'array di oggetti di dettaglio:
            For x = 0 To oListMyDettaglio.GetUpperBound(0)
                'Se l'inserimento su base dati richiamando la funzione SetDettaglioPagamento(oListDettaglio(x)) non è andato a buon fine:
                If SetDettaglioPagamento(oListMyDettaglio(x), 0) <= 0 Then
                    'Restituisco errore;
                    Return 0
                End If
            Next
            Return 1
        Catch Err As Exception
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsPagamenti.SetPagamentoDettagliato.errore: ", Err)
            Log.Debug("Si è verificato un errore in ClsPagamenti::SetPagamentoDettagliato::" & Err.Message)

            Return -1
        End Try
    End Function

    Public Function SetDettaglioPagamento(ByVal oMyDettaglio As ObjDettaglioPagamento, ByVal nDBOperation As Integer) As Integer
        Dim nMyReturn As Integer
        Try
            'valorizzo il CommandText;
            Select Case nDBOperation
                Case 0
                    cmdMyCommand.CommandText = "INSERT INTO TP_PAGAMENTIDETTAGLIO"
                    cmdMyCommand.CommandText += " (IDPAGAMENTO, COD_CAPITOLO, IDVOCE, SEGNO, IMPORTO, OPERATORE, DATA_INSERIMENTO)"
                    cmdMyCommand.CommandText += " VALUES (@IDPAGAMENTO, @CODICECAPITOLO, @IDVOCE, @SEGNO, @IMPORTO, @OPERATORE, @DATAINSERIMENTO)"
                    cmdMyCommand.CommandText += " SELECT @@IDENTITY"
                    'Valorizzo i parameters:
                    cmdMyCommand.Parameters.Clear()
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPAGAMENTO", SqlDbType.Int)).Value = oMyDettaglio.IDPagamento
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICECAPITOLO", SqlDbType.NVarChar)).Value = oMyDettaglio.sCodCapitolo
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDVOCE", SqlDbType.NVarChar)).Value = oMyDettaglio.sCodVoce
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@SEGNO", SqlDbType.NVarChar)).Value = oMyDettaglio.sSegno
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO", SqlDbType.Float)).Value = oMyDettaglio.dImpDettaglio
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@OPERATORE", SqlDbType.NVarChar)).Value = oMyDettaglio.sOperatore
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAINSERIMENTO", SqlDbType.DateTime)).Value = oMyDettaglio.tDataInsert
                    'eseguo la query
                    Dim DrReturn As SqlClient.SqlDataReader
                    DrReturn = iDB.GetDataReader(cmdMyCommand)
                    Do While DrReturn.Read
                        nMyReturn = DrReturn(0)
                    Loop
                    DrReturn.Close()
                Case 2
                    cmdMyCommand.CommandText = "DELETE"
                    cmdMyCommand.CommandText += " FROM TP_PAGAMENTIDETTAGLIO"
                    cmdMyCommand.CommandText += " WHERE (IDPAGAMENTO=@IDPAGAMENTO)"
                    'valorizzo i parameters:
                    cmdMyCommand.Parameters.Clear()
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPAGAMENTO", SqlDbType.Int)).Value = oMyDettaglio.IDPagamento
                    nMyReturn = CInt(iDB.ExecuteNonQuery(cmdMyCommand))
            End Select
            Return nMyReturn
        Catch Err As Exception
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsPagamenti.SetDettaglioPagamento.errore: ", Err)
            Log.Debug("Si è verificato un errore in ClsPagamenti::SetDettaglioPagamento::" & Err.Message)

            Return -1
        End Try
    End Function
#End Region
End Class
