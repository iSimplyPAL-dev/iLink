Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports log4net
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports Utility

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Class DetailsSearchLetture
    Public dsGiro As DataSet
    Public drUbicazione As SqlDataReader
    Public lngIDGiro As Long
    Public lngIDUbicazione As Long
End Class
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''//
Public Class DetailsLetture

    Public dsModalitaLettura As DataSet
    Public dsStatoLettura As DataSet

    Public lngTipoContatore As Long
    Public lngCodImpianto As Long
    Public NomeUtente As String
    Public TipoUtenza As String
    Public MinimoFatturabile As String
    Public MinFattRim As String

End Class

'Public Class DettaglioLetture

'    Public dsModalitaLettura As DataSet
'    Public dsStatoLettura As DataSet
'    Public drAnomalie As SqlDataReader
'    Public drPeriodo As SqlDataReader

'    Public AnomalieApplicate As New ListBox
'    Public lngIdStatoLettura As Long
'    Public lngIdModalitaLettura As Long
'    Public lngIdPeriodo As Long


'    Public Fatturazione As Boolean
'    Public FatturazioneSospesa As Boolean
'    Public IncongruenteForzato As Boolean
'    Public DaRicontrollare As Boolean
'    Public LasciatoAvviso As Boolean
'    Public GiroContatore As Boolean

'    Public DataLettura As String
'    Public DataPassaggio As String

'    Public Lettura As String
'    Public Note As String
'    Public GiorniDiConsumo As String
'    Public ConsumoTeorico As String
'    Public LetturaTeorica As String
'    Public Consumo As String

'    Public oConn As SqlConnection

'    Public Query As String
'    Public QueryCount As String
'    Public RecordCount As Long

'End Class

'Public Class GetListContatori
'    Public oConn As SqlConnection

'    Public Query As String
'    Public QueryCount As String
'    Public RecordCount As Long

'End Class
'Public Class GetListaLetture
'    Public oConn As SqlConnection
'    Public Query As String
'    Public QueryCount As String
'    Public RecordCount As Long
'    Public TableName As String
'End Class

Public Class GestLetture
    Private Shared Log As ILog = LogManager.GetLogger(GetType(GestLetture))
    Private cmdMyCommand As New SqlClient.SqlCommand
    Private FncGen As New ClsGenerale.Generale
    Private iDB As New DBAccess.getDBobject
    Private _Const As New Costanti
    Private RaiseError As New GestioneFile
    Private clsLetture As New clsLetture
    Private NomeDBAnagrafe As String = ConfigurationManager.AppSettings("NOME_DATABASE_ANAGRAFICA") & ".dbo"

    Enum DBOperation
        DB_INSERT = 1
        DB_UPDATE = 0
    End Enum

    Public Function GetFondoScala(IdTipoContatore As Integer, ByVal IDContatore As Integer) As Integer
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Dim nMyFondoScala As Integer = 0

        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetFondoScala", "IDTIPO", "IDCONTATORE")
                    dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDTIPO", IdTipoContatore), ctx.GetParam("IDCONTATORE", IDContatore))
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            'prelevo i dati del contatore
                            nMyFondoScala = StringOperation.FormatInt(myRow("valorefondoscala"))
                        Next
                    End If
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.GetFondoScala.dv.errore: ", ex)
                Finally
                    dvMyDati.Dispose()
                End Try
                ctx.Dispose()
            End Using
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.GetFondoScala.errore: ", Err)
            nMyFondoScala = 0
        End Try
        Return nMyFondoScala
    End Function
    Public Function getListAnomalie() As DataView
        Try
            Return iDB.GetDataView("SELECT  *  FROM TP_ANOMALIE ORDER BY CODANOMALIA")
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.getListAnomalie.errore: ", ex)
            Return Nothing
        End Try
    End Function

    Public Function getListModalitaLettura() As DataView
        Try
            Return iDB.GetDataView("SELECT  * FROM TP_MODALITALETTURA ORDER BY CODMODALITALETTURA")
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.getListModalitaLettura.errore: ", ex)
            Return Nothing
        End Try
    End Function

    Public Function getListStatoLetture() As DataView
        Try
            Return iDB.GetDataView("SELECT  * FROM TP_STATOLETTURA ORDER BY IDSTATOLETTURA")
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.getListStatoLetture.errore: ", ex)
            Return Nothing
        End Try
    End Function

    Public Function getListPeriodo(ByVal sIdEnte As String) As DataView
        Try
            Return iDB.GetDataView("SELECT * FROM TP_PERIODO WHERE COD_ENTE='" & sIdEnte & "'")
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.getListPeriodo.errore: ", ex)
            Return Nothing
        End Try
    End Function
    'Public Function getListAnomalie() As SqlDataReader
    '    Try
    '        Return iDB.GetDataReader("SELECT  *  FROM TP_ANOMALIE ORDER BY CODANOMALIA")
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.getListAnomalie.errore: ", ex)
    '        Return Nothing
    '    End Try
    'End Function

    'Public Function getListModalitaLettura() As SqlDataReader
    '    Try
    '        Return iDB.GetDataReader("SELECT  * FROM TP_MODALITALETTURA ORDER BY CODMODALITALETTURA")
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.getListModalitaLettura.errore: ", ex)
    '        Return Nothing
    '    End Try
    'End Function

    'Public Function getListStatoLetture() As SqlDataReader
    '    Try
    '        Return iDB.GetDataReader("SELECT  * FROM TP_STATOLETTURA ORDER BY IDSTATOLETTURA")
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.getListStatoLetture.errore: ", ex)
    '        Return Nothing
    '    End Try
    'End Function

    'Public Function getListPeriodo(ByVal sIdEnte As String) As SqlDataReader
    '    Try
    '        Return iDB.GetDataReader("SELECT * FROM TP_PERIODO WHERE COD_ENTE='" & sIdEnte & "'")
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.getListPeriodo.errore: ", ex)
    '        Return Nothing
    '    End Try
    'End Function

    '''' <summary>
    '''' Estrapola tutti i documenti di tipo fattura (TIPO_DOCUMENTO = F) che non hanno NESSUN pagamento associato
    '''' </summary>
    '''' <returns>DataTable</returns>
    '''' <remarks>
    '''' </remarks>
    'Public Function getTableInsolutiTotali() As DataTable
    '    Try
    '        Dim sSQL As String
    '        Dim oconn As New SqlConnection

    '        oconn.ConnectionString = ConstSession.StringConnection

    '        sSQL = "Select "
    '        sSQL += " TP_FATTURE_NOTE.COD_UTENTE, TP_FATTURE_NOTE.COGNOME_DENOMINAZIONE, TP_FATTURE_NOTE.NOME, "
    '        sSQL += " TP_FATTURE_NOTE.COD_INTESTATARIO, TP_FATTURE_NOTE.IDENTE,TP_FATTURE_NOTE.NUMERO_FATTURA, "
    '        sSQL += " TP_FATTURE_NOTE.NUMEROUTENTE AS NUTENTE,TP_FATTURE_NOTE.COD_FISCALE AS CODICEFISCALE, TP_FATTURE_NOTE.PARTITA_IVA AS PARTITAIVA, "
    '        sSQL += " TP_FATTURE_NOTE.VIA_RES, TP_FATTURE_NOTE.CIVICO_RES,TP_FATTURE_NOTE.IMPORTO_FATTURANOTA AS IMPORTOFATTURANOTA, "
    '        sSQL += " TP_FATTURE_NOTE.FRAZIONE_RES, TP_FATTURE_NOTE.CAP_RES, TP_FATTURE_NOTE.ESPONENTE_RES , TP_FATTURE_NOTE.COMUNE_RES, TP_FATTURE_NOTE.PROVINCIA_RES, "
    '        sSQL += " TP_FATTURE_NOTE.VIA_RCP, TP_FATTURE_NOTE.CIVICO_RCP, TP_FATTURE_NOTE.FRAZIONE_RCP, TP_FATTURE_NOTE.CAP_RCP, "
    '        sSQL += " TP_FATTURE_NOTE.COMUNE_RCP, TP_FATTURE_NOTE.ESPONENTE_RCP,TP_FATTURE_NOTE.PROVINCIA_RCP,"
    '        sSQL += " TP_FATTURE_NOTE.VIA_CONTATORE,TP_FATTURE_NOTE.MATRICOLA,TP_FATTURE_NOTE.ID_TIPOLOGIA_UTENZA,TP_FATTURE_NOTE.NUTENZE, TP_FATTURE_NOTE.DATA_FATTURA,"
    '        sSQL += " TP_FATTURE_NOTE.CIVICO_CONTATORE, TP_FATTURE_NOTE.FRAZIONE_CONTATORE"
    '        sSQL += " FROM TP_FATTURE_NOTE LEFT OUTER JOIN "
    '        sSQL += " TP_PAGAMENTI ON TP_FATTURE_NOTE.IDENTE = TP_PAGAMENTI.IDENTE AND "
    '        sSQL += " TP_FATTURE_NOTE.NUMERO_FATTURA = TP_PAGAMENTI.NUMERO_FATTURA AND "
    '        sSQL += " TP_FATTURE_NOTE.DATA_FATTURA = TP_PAGAMENTI.DATA_FATTURA "
    '        sSQL += " WHERE (TP_FATTURE_NOTE.DATA_VARIAZIONE IS NULL) AND (TP_PAGAMENTI.IMPORTO_PAGAMENTO IS NULL) AND (TP_FATTURE_NOTE.IDENTE = " & ConstSession.IdEnte & ")"
    '        sSQL += " AND (TP_FATTURE_NOTE.TIPO_DOCUMENTO = 'f')"
    '        Dim ds As DataSet = iDB.RunSQLReturnDataSet(sSQL)
    '        Dim dt As DataTable = ds.Tables(0)

    '        Return dt
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestLetture.getTableInsolutiTotali.errore: ", ex)
    '    End Try
    'End Function

    '''' <summary>
    '''' 
    '''' </summary>
    '''' <param name="sParametroENV">Nome del file di ENVIRONMENT utilizzato dal workflow</param>
    '''' <param name="sUsername">Utente del workflow</param>
    '''' <param name="sApplicazione">Nome applicazione configurata nel workflow</param>
    '''' <param name="sIdEnte">stringa Codice Ente in lavorazione</param>
    '''' <param name="impSoglia">Importo soglia insoluto</param>
    '''' <param name="sAnno">Anno emissione</param>
    '''' <param name="IsInsoluti">Valorizzata a 1 se l'estrazione riguarda gli insoluti parziali
    '''' Valorizzata a 2 se l'estrazione riguarda gli insoluti totali</param>
    '''' <returns>Dataset utilizzato per caricare la griglia contenente i risultati della ricerca</returns>
    '''' <remarks>
    '''' </remarks>
    'Public Function getTableGridInsoluti(ByVal sParametroENV As String, ByVal sUsername As String, ByVal sApplicazione As String, ByVal sIdEnte As String, ByVal impSoglia As Double, ByVal sAnno As String, Optional ByVal IsInsoluti As Integer = 0) As DataSet
    '    'IsInsoluti: {1=Parziali, 2=Totali}
    '    Dim sSQL As String
    '    Dim DsDati As DataSet

    '    Try
    '        'prelevo tutte le fatture emesse
    '        sSQL = "SELECT TMPTBL_EMESSO.COD_UTENTE, TMPTBL_EMESSO.COGNOME_DENOMINAZIONE, TMPTBL_EMESSO.NOME, CFPIVA,"
    '        sSQL += " TP_FATTURE_NOTE.VIA_RES, TP_FATTURE_NOTE.CIVICO_RES, TP_FATTURE_NOTE.ESPONENTE_RES, TP_FATTURE_NOTE.INTERNO_RES, TP_FATTURE_NOTE.SCALA_RES, TP_FATTURE_NOTE.FRAZIONE_RES, TP_FATTURE_NOTE.CAP_RES, TP_FATTURE_NOTE.COMUNE_RES, TP_FATTURE_NOTE.PROVINCIA_RES,"
    '        sSQL += " NOME_INVIO, VIA_RCP, CIVICO_RCP, ESPONENTE_RCP, INTERNO_RCP, SCALA_RCP, FRAZIONE_RCP, CAP_RCP, COMUNE_RCP, PROVINCIA_RCP,"
    '        sSQL += NomeDBAnagrafe & ".ANAGRAFICA.COGNOME_DENOMINAZIONE AS COGNOME_INTEST, " & NomeDBAnagrafe & ".ANAGRAFICA.NOME AS NOME_INTEST,"
    '        sSQL += " NUMEROUTENTE, MATRICOLA, VIA_CONTATORE, CIVICO_CONTATORE, FRAZIONE_CONTATORE, DESCRIZIONE, NUTENZE,"
    '        sSQL += " TMPTBL_EMESSO.DATA_FATTURA, TMPTBL_EMESSO.NUMERO_FATTURA, RIF_NOTE_CREDITO=CASE WHEN RIF_NOTA IS NULL THEN 'NOTA NON ANCORA FATTURATA' ELSE RIF_NOTA END,"
    '        sSQL += " IMPEMESSO, SUM(CASE WHEN IMPORTO_PAGAMENTO IS NULL THEN 0 ELSE IMPORTO_PAGAMENTO END) AS IMPPAGATO,"
    '        sSQL += " (IMPEMESSO-SUM(CASE WHEN IMPORTO_PAGAMENTO IS NULL THEN 0 ELSE IMPORTO_PAGAMENTO END)) AS IMPDIFFERENZA"
    '        sSQL += " FROM ("
    '        sSQL += "   SELECT TP_FATTURE_NOTE.COD_UTENTE, COGNOME_DENOMINAZIONE, NOME, CFPIVA= CASE WHEN NOT PARTITA_IVA IS NULL AND PARTITA_IVA<>'' THEN PARTITA_IVA ELSE COD_FISCALE END,"
    '        sSQL += "   TP_FATTURE_NOTE.DATA_FATTURA, TP_FATTURE_NOTE.NUMERO_FATTURA, RIF_NOTA, IMPORTO_FATTURANOTA, IMPNOTA=CASE WHEN IMPORTO_NOTA IS NULL THEN 0 ELSE IMPORTO_NOTA END,"
    '        sSQL += "   (IMPORTO_FATTURANOTA+CASE WHEN IMPORTO_NOTA IS NULL THEN 0 ELSE IMPORTO_NOTA END) AS IMPEMESSO"
    '        sSQL += "   FROM TP_FATTURE_NOTE"
    '        sSQL += "   LEFT JOIN ("
    '        sSQL += "       SELECT IDENTE+DATA_FATTURA_RIFERIMENTO+NUMERO_FATTURA_RIFERIMENTO AS RIFERIMENTO_FATTURA, IMPORTO_FATTURANOTA AS IMPORTO_NOTA,"
    '        sSQL += "       MAX('NOTA N.'+NUMERO_FATTURA+' DEL '+SUBSTRING(DATA_FATTURA,7,2)+'/'+SUBSTRING(DATA_FATTURA,5,2)+'/'+SUBSTRING(DATA_FATTURA,1,4)) AS RIF_NOTA"
    '        sSQL += "       FROM TP_FATTURE_NOTE"
    '        sSQL += "       WHERE (TP_FATTURE_NOTE.DATA_VARIAZIONE IS NULL) AND (IDENTE='" & sIdEnte & "')"
    '        sSQL += "       AND (NOT IDENTE+DATA_FATTURA_RIFERIMENTO+NUMERO_FATTURA_RIFERIMENTO IS NULL)"
    '        sSQL += "       GROUP BY IDENTE+DATA_FATTURA_RIFERIMENTO+NUMERO_FATTURA_RIFERIMENTO, IMPORTO_FATTURANOTA"
    '        sSQL += "   ) TMPTABLE ON TP_FATTURE_NOTE.IDENTE+TP_FATTURE_NOTE.DATA_FATTURA+TP_FATTURE_NOTE.NUMERO_FATTURA=TMPTABLE.RIFERIMENTO_FATTURA"
    '        sSQL += "   WHERE (TP_FATTURE_NOTE.DATA_VARIAZIONE IS NULL) AND (TP_FATTURE_NOTE.IDENTE = '" & sIdEnte & "')"
    '        sSQL += "   AND (IDFLUSSO IN("
    '        sSQL += "       SELECT ID_FLUSSO"
    '        sSQL += "       FROM TP_FATTURAZIONI_GENERATE"
    '        sSQL += "       WHERE (IDENTE='" & sIdEnte & "')"
    '        sSQL += "       AND (NOT DATA_APPROVAZIONE_DOCUMENTI IS NULL AND DATA_APPROVAZIONE_DOCUMENTI<>'')"
    '        sSQL += "       AND (DATA_SCADENZA<=GETDATE())))"
    '        sSQL += "   GROUP BY TP_FATTURE_NOTE.COD_UTENTE, COGNOME_DENOMINAZIONE, NOME, CASE WHEN NOT PARTITA_IVA IS NULL AND PARTITA_IVA<>'' THEN PARTITA_IVA ELSE COD_FISCALE END,"
    '        sSQL += "   TP_FATTURE_NOTE.DATA_FATTURA, TP_FATTURE_NOTE.NUMERO_FATTURA, RIF_NOTA, IMPORTO_FATTURANOTA, IMPORTO_NOTA"
    '        sSQL += " ) TMPTBL_EMESSO INNER JOIN TP_FATTURE_NOTE ON TMPTBL_EMESSO.DATA_FATTURA=TP_FATTURE_NOTE.DATA_FATTURA AND TMPTBL_EMESSO.NUMERO_FATTURA=TP_FATTURE_NOTE.NUMERO_FATTURA"
    '        sSQL += " INNER JOIN " & NomeDBAnagrafe & ".ANAGRAFICA ON TP_FATTURE_NOTE.COD_INTESTATARIO=" & NomeDBAnagrafe & ".ANAGRAFICA.COD_CONTRIBUENTE"
    '        sSQL += " INNER JOIN TP_TIPIUTENZA ON TP_FATTURE_NOTE.ID_TIPOLOGIA_UTENZA=TP_TIPIUTENZA.IDTIPOUTENZA"
    '        sSQL += " LEFT JOIN TP_PAGAMENTI ON TMPTBL_EMESSO.DATA_FATTURA=TP_PAGAMENTI.DATA_FATTURA AND TMPTBL_EMESSO.NUMERO_FATTURA=TP_PAGAMENTI.NUMERO_FATTURA"
    '        If sAnno <> "" Then
    '            sSQL += " AND (SUBSTRING(TP_FATTURE_NOTE.DATA_FATTURA,1,4)='" & sAnno & "')"
    '        End If
    '        sSQL += " GROUP BY TMPTBL_EMESSO.COD_UTENTE, TMPTBL_EMESSO.COGNOME_DENOMINAZIONE, TMPTBL_EMESSO.NOME, CFPIVA,"
    '        sSQL += " TP_FATTURE_NOTE.VIA_RES, TP_FATTURE_NOTE.CIVICO_RES, TP_FATTURE_NOTE.ESPONENTE_RES, TP_FATTURE_NOTE.INTERNO_RES, TP_FATTURE_NOTE.SCALA_RES, TP_FATTURE_NOTE.FRAZIONE_RES, TP_FATTURE_NOTE.CAP_RES, TP_FATTURE_NOTE.COMUNE_RES, TP_FATTURE_NOTE.PROVINCIA_RES,"
    '        sSQL += " NOME_INVIO, VIA_RCP, CIVICO_RCP, ESPONENTE_RCP, INTERNO_RCP, SCALA_RCP, FRAZIONE_RCP, CAP_RCP, COMUNE_RCP, PROVINCIA_RCP,"
    '        sSQL += " " & NomeDBAnagrafe & ".ANAGRAFICA.COGNOME_DENOMINAZIONE, " & ConfigurationManager.AppSettings("NomeDbAnagrafe").ToString() & ".ANAGRAFICA.NOME,"
    '        sSQL += " NUMEROUTENTE, MATRICOLA, VIA_CONTATORE, CIVICO_CONTATORE, FRAZIONE_CONTATORE, DESCRIZIONE, NUTENZE,"
    '        sSQL += " TMPTBL_EMESSO.DATA_FATTURA, TMPTBL_EMESSO.NUMERO_FATTURA, CASE WHEN RIF_NOTA IS NULL THEN 'NOTA NON ANCORA FATTURATA' ELSE RIF_NOTA END, IMPEMESSO"
    '        If IsInsoluti = 1 Then
    '            sSQL += " HAVING (SUM(CASE WHEN IMPORTO_PAGAMENTO IS NULL THEN 0 ELSE IMPORTO_PAGAMENTO END)>0)"
    '            sSQL += " AND ((IMPEMESSO-SUM(CASE WHEN IMPORTO_PAGAMENTO IS NULL THEN 0 ELSE IMPORTO_PAGAMENTO END))>0)"
    '            If impSoglia > 0 Then
    '                sSQL += " AND ((IMPEMESSO-SUM(CASE WHEN IMPORTO_PAGAMENTO IS NULL THEN 0 ELSE IMPORTO_PAGAMENTO END))>=" & impSoglia & ")"
    '            End If
    '        ElseIf IsInsoluti = 2 Then
    '            sSQL += " HAVING (SUM(CASE WHEN IMPORTO_PAGAMENTO IS NULL THEN 0 ELSE IMPORTO_PAGAMENTO END)=0)"
    '            If impSoglia > 0 Then
    '                sSQL += " AND (IMPEMESSO>=" & impSoglia & ")"
    '            End If
    '        Else
    '            sSQL += " HAVING ((IMPEMESSO-SUM(CASE WHEN IMPORTO_PAGAMENTO IS NULL THEN 0 ELSE IMPORTO_PAGAMENTO END))>0)"
    '            If impSoglia > 0 Then
    '                sSQL += " AND ((IMPEMESSO-SUM(CASE WHEN IMPORTO_PAGAMENTO IS NULL THEN 0 ELSE IMPORTO_PAGAMENTO END))>=" & impSoglia & ")"
    '            End If
    '        End If
    '        sSQL += " ORDER BY TMPTBL_EMESSO.COGNOME_DENOMINAZIONE, TMPTBL_EMESSO.NOME, CFPIVA,"
    '        sSQL += " TMPTBL_EMESSO.DATA_FATTURA, TMPTBL_EMESSO.NUMERO_FATTURA"
    '        'eseguo la query
    '        DsDati = iDB.GetDataSet(sSQL)

    '        Return DsDati
    '    Catch Err As Exception
    '        Log.Debug("Si è verificato un errore in GestLetture::getTableGridInsoluti::" & Err.Message & " SQL::" & sSQL)

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestLetture.getTableGridInsoluti.errore: ", Err)
    '        Return Nothing
    '    End Try
    'End Function

    '''' <summary>
    '''' Estrapola tutti i documenti di tipo fattura (TIPO_DOCUMENTO = F) che sono pagate parzialmente
    '''' </summary>
    '''' <returns>DataTable</returns>
    '''' <remarks>
    '''' </remarks>
    'Public Function getTableInsolutiParziali() As DataTable
    '    Try
    '        Dim sSQL As String
    '        Dim oconn As New SqlConnection

    '        oconn.ConnectionString = ConstSession.StringConnection

    '        sSQL = ""
    '        sSQL = "SELECT COUNT(*) AS CONTEGGIO, TP_FATTURE_NOTE.IMPORTO_FATTURANOTA AS IMPORTOFATTURANOTA, SUM(TP_PAGAMENTI.IMPORTO_PAGAMENTO) "
    '        sSQL += " AS SOMMAPAGAMENTI, TP_FATTURE_NOTE.COGNOME_DENOMINAZIONE AS COGNOME, TP_FATTURE_NOTE.NOME AS NOME, "
    '        sSQL += " TP_FATTURE_NOTE.DATA_FATTURA AS DATAFATTURA, TP_FATTURE_NOTE.IMPORTO_FATTURANOTA - SUM(TP_PAGAMENTI.IMPORTO_PAGAMENTO) "
    '        sSQL += " AS DIFFERENZA, TP_FATTURE_NOTE.NUMEROUTENTE AS NUTENTE, TP_FATTURE_NOTE.COD_FISCALE AS CODFISCALE, TP_FATTURE_NOTE.PARTITA_IVA AS PARTITAIVA, "
    '        sSQL += " TP_FATTURE_NOTE.VIA_RES, TP_FATTURE_NOTE.CIVICO_RES, TP_FATTURE_NOTE.ESPONENTE_RES, TP_FATTURE_NOTE.FRAZIONE_RES, TP_FATTURE_NOTE.CAP_RES, TP_FATTURE_NOTE.COMUNE_RES, "
    '        sSQL += " TP_FATTURE_NOTE.PROVINCIA_RES, TP_FATTURE_NOTE.VIA_RCP,TP_FATTURE_NOTE.CIVICO_RCP, TP_FATTURE_NOTE.ESPONENTE_RCP, "
    '        sSQL += " TP_FATTURE_NOTE.FRAZIONE_RCP, TP_FATTURE_NOTE.CAP_RCP, TP_FATTURE_NOTE.COMUNE_RCP, TP_FATTURE_NOTE.PROVINCIA_RCP, "
    '        sSQL += " TP_FATTURE_NOTE.VIA_CONTATORE, TP_FATTURE_NOTE.CIVICO_CONTATORE, TP_FATTURE_NOTE.FRAZIONE_CONTATORE, "
    '        sSQL += " TP_FATTURE_NOTE.MATRICOLA,TP_FATTURE_NOTE.ID_TIPOLOGIA_UTENZA,TP_FATTURE_NOTE.NUTENZE,TP_FATTURE_NOTE.NUMERO_FATTURA, "
    '        sSQL += " TP_PAGAMENTI.DATA_FATTURA,TP_FATTURE_NOTE.COD_INTESTATARIO "
    '        sSQL += " FROM TP_PAGAMENTI INNER JOIN"
    '        sSQL += " TP_FATTURE_NOTE ON TP_PAGAMENTI.DATA_FATTURA = TP_FATTURE_NOTE.DATA_FATTURA AND "
    '        sSQL += " TP_PAGAMENTI.NUMERO_FATTURA = TP_FATTURE_NOTE.NUMERO_FATTURA"
    '        sSQL += " WHERE (TP_FATTURE_NOTE.DATA_VARIAZIONE IS NULL) AND (TP_PAGAMENTI.IDENTE = " & ConstSession.IdEnte & ")"
    '        'sSQL+=" GROUP BY TP_FATTURE_NOTE.IMPORTO_FATTURANOTA, TP_FATTURE_NOTE.COGNOME_DENOMINAZIONE, TP_FATTURE_NOTE.NOME, "
    '        'sSQL+=" TP_FATTURE_NOTE.DATA_FATTURA , TP_FATTURE_NOTE.NUMEROUTENTE, TP_FATTURE_NOTE.COD_FISCALE, TP_FATTURE_NOTE.PARTITA_IVA"
    '        sSQL += " GROUP BY TP_FATTURE_NOTE.IMPORTO_FATTURANOTA, TP_FATTURE_NOTE.COGNOME_DENOMINAZIONE, TP_FATTURE_NOTE.NOME, "
    '        sSQL += " TP_FATTURE_NOTE.DATA_FATTURA, TP_FATTURE_NOTE.NUMEROUTENTE, TP_FATTURE_NOTE.COD_FISCALE, TP_FATTURE_NOTE.PARTITA_IVA, "
    '        sSQL += " TP_FATTURE_NOTE.VIA_RES, TP_FATTURE_NOTE.CIVICO_RES, TP_FATTURE_NOTE.ESPONENTE_RES, TP_FATTURE_NOTE.FRAZIONE_RES, "
    '        sSQL += " TP_FATTURE_NOTE.CAP_RES, TP_FATTURE_NOTE.COMUNE_RES, TP_FATTURE_NOTE.PROVINCIA_RES,"
    '        sSQL += " TP_FATTURE_NOTE.VIA_RCP, TP_FATTURE_NOTE.CIVICO_RCP, "
    '        sSQL += " TP_FATTURE_NOTE.ESPONENTE_RCP, TP_FATTURE_NOTE.FRAZIONE_RCP, TP_FATTURE_NOTE.CAP_RCP, "
    '        sSQL += " TP_FATTURE_NOTE.COMUNE_RCP, TP_FATTURE_NOTE.PROVINCIA_RCP,"
    '        sSQL += " TP_FATTURE_NOTE.VIA_CONTATORE, TP_FATTURE_NOTE.CIVICO_CONTATORE, TP_FATTURE_NOTE.FRAZIONE_CONTATORE,"
    '        sSQL += " TP_FATTURE_NOTE.MATRICOLA,TP_FATTURE_NOTE.ID_TIPOLOGIA_UTENZA,TP_FATTURE_NOTE.NUTENZE,TP_FATTURE_NOTE.NUMERO_FATTURA,"
    '        sSQL += " TP_PAGAMENTI.DATA_FATTURA,TP_FATTURE_NOTE.COD_INTESTATARIO"
    '        Dim ds As DataSet
    '        ds = iDB.RunSQLReturnDataSet(sSQL)
    '        Dim dt As DataTable
    '        dt = ds.Tables(0)
    '        Return dt
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestLetture.getTableInsolutiParziali.errore: ", ex)
    '    End Try
    'End Function

    ''' <summary>
    ''' Estrae i contatori attivi che non hanno una lettura associata pert il periodo selezionato
    ''' </summary>
    ''' <returns>DataTable</returns>
    ''' <remarks>
    ''' </remarks>
    Public Function getTableLettureNonPresenti(ByVal sIdEnte As String, ByVal nIdPeriodo As Integer, ByVal sIntestatario As String, ByVal sUtente As String, ByVal sVia As String, ByVal sNumeroUtente As String, ByVal sMatricola As String, ByVal nIdGiro As Integer, ByVal bSub As Boolean) As DataView
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Dim IsSub As Integer = 0
        Try
            'determino i dati del periodo
            Dim oPeriodo As TabelleDiDecodifica.DetailPeriodo = New TabelleDiDecodifica.DBPeriodo().GetPeriodo(nIdPeriodo)
            If bSub = True Then
                IsSub = 1
            Else
                IsSub = 0
            End If
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetLettureNonPresenti", "IDENTE", "IDPERIODO", "DATAA", "INTESTATARIO", "UTENTE", "VIA", "NUTENTE", "MATRICOLA", "IDGIRO", "ISSUB")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", sIdEnte) _
                        , ctx.GetParam("IDPERIODO", nIdPeriodo) _
                        , ctx.GetParam("DATAA", FncGen.GiraData(oPeriodo.AData)) _
                        , ctx.GetParam("INTESTATARIO", FncGen.ReplaceCharsForSearch(sIntestatario)) _
                        , ctx.GetParam("UTENTE", FncGen.ReplaceCharsForSearch(sUtente)) _
                        , ctx.GetParam("VIA", sVia) _
                        , ctx.GetParam("NUTENTE", sNumeroUtente) _
                        , ctx.GetParam("MATRICOLA", sMatricola) _
                        , ctx.GetParam("IDGIRO", nIdGiro) _
                        , ctx.GetParam("ISSUB", IsSub)
                    )
                ctx.Dispose()
            End Using
            Return dvMyDati
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.getTableLettureNonPresenti.errore: ", Err)
            Return Nothing
        End Try
    End Function
    'Public Function getTableLettureNonPresenti(ByVal sIdEnte As String, ByVal nIdPeriodo As Integer, ByVal sIntestatario As String, ByVal sUtente As String, ByVal sVia As String, ByVal sNumeroUtente As String, ByVal sMatricola As String, ByVal nIdGiro As Integer, ByVal bSub As Boolean) As DataView
    '    Dim dvMyDati As DataView
    '    Dim clsPeriodo As New TabelleDiDecodifica.DBPeriodo
    '    Dim oPeriodo As TabelleDiDecodifica.DetailPeriodo

    '    Try
    '        'determino i dati del periodo
    '        oPeriodo = clsPeriodo.GetPeriodo(nIdPeriodo)

    '        cmdMyCommand.Connection = New SqlConnection(ConstSession.StringConnection)
    '        cmdMyCommand.Parameters.Clear()
    '        'valorizzo il CommandText;
    '        cmdMyCommand.CommandText = "SELECT *"
    '        cmdMyCommand.CommandText += " FROM OPENGOV_STAMPACONTATORILETTURE"
    '        cmdMyCommand.CommandText += " WHERE (DATAATTIVAZIONE IS NOT NULL)"
    '        cmdMyCommand.CommandText += " AND (DATAATTIVAZIONE <> '') "
    '        cmdMyCommand.CommandText += " AND (DATAATTIVAZIONE <=@DATAA) "
    '        cmdMyCommand.CommandText += " AND (DATACESSAZIONE IS NULL OR DATACESSAZIONE='')"
    '        cmdMyCommand.CommandText += " AND (CODCONTATORE NOT IN ("
    '        cmdMyCommand.CommandText += " 	SELECT DISTINCT TP_CONTATORI.CODCONTATORE "
    '        cmdMyCommand.CommandText += " 	FROM TP_LETTURE "
    '        cmdMyCommand.CommandText += " 	LEFT JOIN TP_CONTATORI ON TP_LETTURE.CODCONTATORE = TP_CONTATORI.CODCONTATORE "
    '        cmdMyCommand.CommandText += " 	LEFT JOIN TP_PERIODO ON TP_LETTURE.CODPERIODO = TP_PERIODO.CODPERIODO AND TP_CONTATORI.CODENTE = TP_PERIODO.COD_ENTE"
    '        cmdMyCommand.CommandText += " 	WHERE (TP_CONTATORI.CODENTE =@IDENTE) "
    '        cmdMyCommand.CommandText += " 	AND (TP_LETTURE.CODPERIODO=@IDPERIODO)"
    '        cmdMyCommand.CommandText += " 	AND (TP_LETTURE.PRIMALETTURA=0 OR TP_LETTURE.PRIMALETTURA IS NULL)"
    '        cmdMyCommand.CommandText += " ))"
    '        cmdMyCommand.CommandText += " AND (CODENTE=@IDENTE )"
    '        'cmdMyCommand.CommandText += " AND (CODPERIODO=@IDPERIODO)"
    '        If sIntestatario <> "" Then
    '            cmdMyCommand.CommandText += " AND (COGNOME_INT+' '+NOME_INT LIKE @INTESTATARIO)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@INTESTATARIO", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(sIntestatario) & "%"
    '        End If
    '        If sUtente <> "" Then
    '            cmdMyCommand.CommandText += " AND (COGNOME_UT+' '+NOME_UT LIKE @UTENTE)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@UTENTE", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(sUtente) & "%"
    '        End If
    '        If sVia <> "" Then
    '            cmdMyCommand.CommandText += " AND (VIA_UBICAZIONE=@VIA)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VIA", SqlDbType.NVarChar)).Value = sVia
    '        End If
    '        If sNumeroUtente <> "" Then
    '            cmdMyCommand.CommandText += " AND (NUMEROUTENTE=@NUTENTE)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUTENTE", SqlDbType.NVarChar)).Value = sNumeroUtente
    '        End If
    '        If sMatricola <> "" Then
    '            cmdMyCommand.CommandText += " AND (MATRICOLA=@MATRICOLA)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@MATRICOLA", SqlDbType.NVarChar)).Value = sMatricola
    '        End If
    '        If nIdGiro > 0 Then
    '            cmdMyCommand.CommandText += " AND (CODGIRO=@IDGIRO)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDGIRO", SqlDbType.Int)).Value = nIdGiro
    '        End If
    '        If bSub = True Then
    '            cmdMyCommand.CommandText += " AND (NOT CODCONTATORESUB IS NULL)"
    '        End If
    '        cmdMyCommand.CommandText += " ORDER BY COGNOME_INT,NOME_INT"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPERIODO", SqlDbType.Int)).Value = nIdPeriodo
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAA", SqlDbType.NVarChar)).Value = FncGen.GiraData(oPeriodo.AData)
    '        dvMyDati = iDB.GetDataView(cmdMyCommand)
    '        Return dvMyDati
    '    Catch Err As Exception
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))

    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.getTableLettureNonPresenti.errore: ", Err)

    '        Log.Debug("Si è verificato un errore in GestContatori::getTableLettureNonPresenti::" & Err.Message)
    '        Return Nothing
    '    End Try
    'End Function


    '

    ''' <summary>
    ''' Estrae le letture presenti per il periodo selezionato che hanno il campo PRIMALETTURA pari a 0
    ''' </summary>
    ''' <returns>DataTable</returns>
    ''' <remarks>
    ''' </remarks>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Esportazione completa dati</em>
    ''' </revision>
    ''' </revisionHistory>
    Public Function getTableLetture(myStringConnection As String, ByVal sIdEnte As String, ByVal nIdPeriodo As Integer, ByVal sIntestatario As String, ByVal sUtente As String, ByVal sVia As String, ByVal sNumeroUtente As String, ByVal sMatricola As String, ByVal nIdGiro As Integer, ByVal bSub As Boolean) As DataView
        Dim dvMyDati As DataView

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.Parameters.Clear()
            'valorizzo il CommandText;
            cmdMyCommand.CommandText = "SELECT *"
            cmdMyCommand.CommandText += " FROM OPENGOV_STAMPALETTURE"
            cmdMyCommand.CommandText += " WHERE (1=1)"
            cmdMyCommand.CommandText += " AND (CODENTE=@IDENTE )"
            cmdMyCommand.CommandText += " AND (@IDPERIODO<=0 OR CODPERIODO=@IDPERIODO)"
            If sIntestatario <> "" Then
                cmdMyCommand.CommandText += " AND (COGNOME_INT+' '+NOME_INT LIKE @INTESTATARIO)"
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@INTESTATARIO", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(sIntestatario) & "%"
            End If
            If sUtente <> "" Then
                cmdMyCommand.CommandText += " AND (COGNOME_UT+' '+NOME_UT LIKE @UTENTE)"
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@UTENTE", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(sUtente) & "%"
            End If
            If sVia <> "" Then
                cmdMyCommand.CommandText += " AND (VIA_UBICAZIONE=@VIA)"
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VIA", SqlDbType.NVarChar)).Value = sVia
            End If
            If sNumeroUtente <> "" Then
                cmdMyCommand.CommandText += " AND (NUMEROUTENTE=@NUTENTE)"
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUTENTE", SqlDbType.NVarChar)).Value = sNumeroUtente
            End If
            If sMatricola <> "" Then
                cmdMyCommand.CommandText += " AND (MATRICOLA=@MATRICOLA)"
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@MATRICOLA", SqlDbType.NVarChar)).Value = sMatricola
            End If
            If nIdGiro > 0 Then
                cmdMyCommand.CommandText += " AND (CODGIRO=@IDGIRO)"
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDGIRO", SqlDbType.Int)).Value = nIdGiro
            End If
            If bSub = True Then
                cmdMyCommand.CommandText += " AND (NOT CODCONTATORESUB IS NULL)"
            End If
            cmdMyCommand.CommandText += " ORDER BY "
            cmdMyCommand.CommandText += " COGNOME_INT,"
            cmdMyCommand.CommandText += " NOME_INT,"
            cmdMyCommand.CommandText += " NUMEROUTENTE,"
            cmdMyCommand.CommandText += " MATRICOLA"
            'valorizzo i parameters:
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPERIODO", SqlDbType.Int)).Value = nIdPeriodo
            dvMyDati = iDB.GetDataView(cmdMyCommand)
            Return dvMyDati
        Catch Err As Exception
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.getTableLetture.errore: ", Err)
            Return Nothing
        End Try
    End Function
    'Public Function getTableLetture(ByVal sIdEnte As String, ByVal nIdPeriodo As Integer, ByVal sIntestatario As String, ByVal sUtente As String, ByVal sVia As String, ByVal sNumeroUtente As String, ByVal sMatricola As String, ByVal nIdGiro As Integer, ByVal bSub As Boolean) As DataView
    '    Dim dvMyDati As DataView

    '    Try
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        cmdMyCommand.Parameters.Clear()
    '        'valorizzo il CommandText;
    '        cmdMyCommand.CommandText = "SELECT *"
    '        cmdMyCommand.CommandText += " FROM OPENGOV_STAMPALETTURE"
    '        cmdMyCommand.CommandText += " WHERE (1=1)"
    '        cmdMyCommand.CommandText += " AND (CODENTE=@IDENTE )"
    '        cmdMyCommand.CommandText += " AND (CODPERIODO=@IDPERIODO)"
    '        If sIntestatario <> "" Then
    '            cmdMyCommand.CommandText += " AND (COGNOME_INT+' '+NOME_INT LIKE @INTESTATARIO)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@INTESTATARIO", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(sIntestatario) & "%"
    '        End If
    '        If sUtente <> "" Then
    '            cmdMyCommand.CommandText += " AND (COGNOME_UT+' '+NOME_UT LIKE @UTENTE)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@UTENTE", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(sUtente) & "%"
    '        End If
    '        If sVia <> "" Then
    '            cmdMyCommand.CommandText += " AND (VIA_UBICAZIONE=@VIA)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VIA", SqlDbType.NVarChar)).Value = sVia
    '        End If
    '        If sNumeroUtente <> "" Then
    '            cmdMyCommand.CommandText += " AND (NUMEROUTENTE=@NUTENTE)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUTENTE", SqlDbType.NVarChar)).Value = sNumeroUtente
    '        End If
    '        If sMatricola <> "" Then
    '            cmdMyCommand.CommandText += " AND (MATRICOLA=@MATRICOLA)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@MATRICOLA", SqlDbType.NVarChar)).Value = sMatricola
    '        End If
    '        If nIdGiro > 0 Then
    '            cmdMyCommand.CommandText += " AND (CODGIRO=@IDGIRO)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDGIRO", SqlDbType.Int)).Value = nIdGiro
    '        End If
    '        If bSub = True Then
    '            cmdMyCommand.CommandText += " AND (NOT CODCONTATORESUB IS NULL)"
    '        End If
    '        cmdMyCommand.CommandText += " ORDER BY "
    '        cmdMyCommand.CommandText += " COGNOME_INT,"
    '        cmdMyCommand.CommandText += " NOME_INT,"
    '        cmdMyCommand.CommandText += " NUMEROUTENTE,"
    '        cmdMyCommand.CommandText += " MATRICOLA"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPERIODO", SqlDbType.Int)).Value = nIdPeriodo
    '        dvMyDati = iDB.GetDataView(cmdMyCommand)
    '        Return dvMyDati
    '    Catch Err As Exception
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestLetture.getTableLetture.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in GestContatori::GetTableContatoriAttivi::" & Err.Message)
    '        Return Nothing
    '    End Try
    'End Function

    '''' <summary>
    '''' In base a un ID contatore restituisce il numero utente, la tipologia dell'utenza, 
    '''' il numero delle utenze e il minimo fatturabile
    '''' </summary>
    '''' <param name="IDContatore">Chiave primaria della tabella TP_CONTATORI</param>
    '''' <returns>Un oggetto di tipo DetailsLetture
    '''' </returns>
    '''' <remarks>
    '''' </remarks>
    'Public Function GetDetailsLetture(ByVal IDContatore As Integer) As DetailsLetture
    '    Try
    '        Dim DetailsLetture As New DetailsLetture
    '        Dim sSQL As String
    '        Dim rdTemp As SqlDataReader
    '        DetailsLetture.dsStatoLettura = iDB.RunSPReturnDataSet("sp_StatoLettura", "TP_STATOLETTURA")
    '        DetailsLetture.dsModalitaLettura = iDB.RunSPReturnDataSet("sp_ModLettura", "TP_MODALITALETTURA")

    '        sSQL = ""
    '        sSQL = "SELECT  TP_CONTATORI.NUMEROUTENTE," & vbCrLf
    '        sSQL += "TP_TIPIUTENZA.DESCRIZIONE, TP_CONTATORI.NUMEROUTENZE, TP_CONTATORI.CONSUMOMINIMOFATTURABILE, " & vbCrLf
    '        sSQL += "TP_CONTATORI.CONSUMOMINIMOFATTURABILERIMTEMP" & vbCrLf
    '        sSQL += "FROM TP_CONTATORI INNER JOIN" & vbCrLf
    '        sSQL += "TR_CONTATORI_UTENTE ON TP_CONTATORI.CODCONTATORE = TR_CONTATORI_UTENTE.CODCONTATORE INNER JOIN" & vbCrLf
    '        sSQL += "TP_TIPIUTENZA ON TP_CONTATORI.IDTIPOUTENZA = TP_TIPIUTENZA.IDTIPOUTENZA" & vbCrLf
    '        sSQL += "WHERE TP_CONTATORI.CODCONTATORE =" & IDContatore

    '        rdTemp = iDB.GetDataReader(sSQL)

    '        If rdTemp.Read() Then

    '            DetailsLetture.NomeUtente = ""
    '            DetailsLetture.TipoUtenza = Utility.StringOperation.FormatString(rdTemp("DESCRIZIONE").ToString())
    '            DetailsLetture.MinimoFatturabile = Utility.StringOperation.FormatString(rdTemp("CONSUMOMINIMOFATTURABILE").ToString())
    '            DetailsLetture.MinFattRim = Utility.StringOperation.FormatString(rdTemp("CONSUMOMINIMOFATTURABILERIMTEMP").ToString())

    '        End If

    '        rdTemp.Close()


    '        Return DetailsLetture
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.GetDetailsLetture.errore: ", ex)
    '    End Try
    'End Function

    ''' <summary>
    ''' Restituisce i dati di una determinata lettura
    ''' </summary>
    ''' <param name="IDLettura">Chiave primaria della tabella TP_LETTURE</param>
    ''' <param name="IDcontatore">Chiave primaria della tabella TP_CONTATORI</param>
    ''' <returns>Un oggetto di tipo DettaglioLetture
    ''' </returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Function GetDettaglioLetture(ByVal IDLettura As Integer, ByVal IDcontatore As Integer, ByVal nPeriodo As Integer, ByVal tDataLetAtt As DateTime, ByVal nLetAtt As Integer, ByVal bLastLettura As Boolean) As ObjLettura
        Dim DettaglioLetture As New ObjLettura
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetLetture", "CODCONTATORE", "CODPERIODO", "IDLETTURA", "DATALETTURA", "LETTURA")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("CODCONTATORE", IDcontatore) _
                        , ctx.GetParam("CODPERIODO", nPeriodo) _
                        , ctx.GetParam("IDLETTURA", IDLettura) _
                        , ctx.GetParam("DATALETTURA", StringOperation.FormatDateTime(tDataLetAtt)) _
                        , ctx.GetParam("LETTURA", nLetAtt)
                    )
                ctx.Dispose()
            End Using
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    DettaglioLetture.IdLettura = StringOperation.FormatInt(myRow("CODLETTURA"))
                    DettaglioLetture.nIdContatore = StringOperation.FormatInt(myRow("CODCONTATORE"))
                    DettaglioLetture.nIdContatorePrec = StringOperation.FormatInt(myRow("CODCONTATOREPRECEDENTE"))
                    DettaglioLetture.nIdPeriodo = StringOperation.FormatInt(myRow("CODPERIODO"))
                    DettaglioLetture.tDataLetturaAtt = StringOperation.FormatDateTime(myRow("DATALETTURA"))
                    DettaglioLetture.nLetturaAtt = StringOperation.FormatInt(myRow("LETTURA"))
                    DettaglioLetture.nCodModoLett = StringOperation.FormatInt(myRow("CODMODALITALETTURA"))
                    DettaglioLetture.bIsFatturata = StringOperation.FormatBool(myRow("FATTURAZIONE"))
                    DettaglioLetture.nConsumo = StringOperation.FormatInt(myRow("CONSUMO"))
                    DettaglioLetture.sNote = StringOperation.FormatString(myRow("NOTE"))
                    DettaglioLetture.nGiorni = StringOperation.FormatInt(myRow("GIORNIDICONSUMO"))
                    DettaglioLetture.bIsIncongruente = StringOperation.FormatBool(myRow("INCONGRUENTE"))
                    DettaglioLetture.bIsIncongruenteForzato = StringOperation.FormatBool(myRow("INCONGRUENTEFORZATO"))
                    DettaglioLetture.nConsumoTeorico = StringOperation.FormatInt(myRow("CONSUMOTEORICO"))
                    DettaglioLetture.bFattSospesa = StringOperation.FormatBool(myRow("FATTURAZIONESOSPESA"))
                    DettaglioLetture.sNUtente = StringOperation.FormatString(myRow("NUMEROUTENTE"))
                    DettaglioLetture.tDataLetturaPrec = StringOperation.FormatDateTime(myRow("DATALETTURAPRECEDENTE"))
                    DettaglioLetture.nLetturaPrec = StringOperation.FormatInt(myRow("LETTURAPRECEDENTE"))
                    DettaglioLetture.bIsPrimaLettura = StringOperation.FormatBool(myRow("PRIMALETTURA"))
                    DettaglioLetture.nIdUtente = StringOperation.FormatInt(myRow("CODUTENTE"))
                    DettaglioLetture.bIsUltimaLettura = StringOperation.FormatBool(myRow("ULTIMALETTURA"))
                    DettaglioLetture.nConsumoPrec = StringOperation.FormatInt(myRow("CONSUMOTOTALEPREC"))
                    DettaglioLetture.nGiorniPrec = StringOperation.FormatInt(myRow("GIORNIDICONSUMOTOTPREC"))
                    DettaglioLetture.tDataFatt = StringOperation.FormatDateTime(myRow("DATAFATTURAZIONE"))
                    DettaglioLetture.nIdStatoLettura = StringOperation.FormatInt(myRow("IDSTATOLETTURA"))
                    DettaglioLetture.tDataPassaggio = StringOperation.FormatDateTime(myRow("DATADIPASSAGGIO"))
                    DettaglioLetture.nIdAnomalia1 = StringOperation.FormatInt(myRow("COD_ANOMALIA1"))
                    DettaglioLetture.nIdAnomalia2 = StringOperation.FormatInt(myRow("COD_ANOMALIA2"))
                    DettaglioLetture.nIdAnomalia3 = StringOperation.FormatInt(myRow("COD_ANOMALIA3"))
                    DettaglioLetture.sLetturaTeorica = StringOperation.FormatString(myRow("LETTURATEORICA"))
                    DettaglioLetture.bIsGiroContatore = StringOperation.FormatBool(myRow("GIROCONTATORE"))
                    DettaglioLetture.bIsStorica = StringOperation.FormatBool(myRow("STORICA"))
                    DettaglioLetture.bIsStoricizzata = StringOperation.FormatBool(myRow("STORICIZZATA"))
                    DettaglioLetture.sProvenienza = StringOperation.FormatString(myRow("PROVENIENZA"))
                    DettaglioLetture.nConsumoSubContatore = StringOperation.FormatInt(myRow("CONSUMOSUB"))
                    DettaglioLetture.tDataInserimento = StringOperation.FormatDateTime(myRow("DATA_INSERIMENTO"))
                    DettaglioLetture.tDataVariazione = StringOperation.FormatDateTime(myRow("DATA_VARIAZIONE"))
                    DettaglioLetture.sAzione = StringOperation.FormatString(myRow("AZIONE"))
                Next
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.GetDettaglioLetture.errore: ", ex)
            Return Nothing
        Finally
            dvMyDati.Dispose()
        End Try
        Return DettaglioLetture
    End Function
    'Public Function GetDettaglioLetture(ByVal IDLettura As Integer, ByVal IDcontatore As Integer, ByVal nPeriodo As Integer, ByVal tDataLetAtt As DateTime, ByVal nLetAtt As Integer, ByVal bLastLettura As Boolean) As ObjLettura
    '    Dim DettaglioLetture As New ObjLettura
    '    Dim sSQL As String
    '    Dim drDetailsLetture As SqlDataReader

    '    Try
    '        If bLastLettura = True Then
    '            sSQL = "SELECT *"
    '            sSQL += " FROM TP_LETTURE"
    '            sSQL += " WHERE 1=1"
    '            If IDcontatore > 0 Then
    '                sSQL += " AND (CODCONTATORE = " & IDcontatore & ")"
    '            End If
    '            If nPeriodo > 0 Then
    '                sSQL += " AND (CODPERIODO = " & nPeriodo & ")"
    '            End If
    '            sSQL += " ORDER BY DATALETTURA DESC"
    '            Log.Debug("GestLetture::GetDettaglioLetture::prelevo da TP_LETTURE::" & sSQL)
    '        Else
    '            sSQL = "SELECT *"
    '            sSQL += " FROM TP_LETTURE"
    '            sSQL += " WHERE 1=1"
    '            If IDLettura > 0 Then
    '                sSQL += " AND (CODLETTURA = " & IDLettura & ")"
    '            End If
    '            If IDcontatore > 0 Then
    '                sSQL += " AND (CODCONTATORE = " & IDcontatore & ")"
    '            End If
    '            If nPeriodo > 0 Then
    '                sSQL += " AND (CODPERIODO = " & nPeriodo & ")"
    '            End If
    '            If tDataLetAtt > Date.MinValue Then
    '                sSQL += " AND (DATALETTURA = '" & FncGen.FormattaData("A", "", tDataLetAtt, False) & "')"
    '            End If
    '            If nLetAtt > 0 Then
    '                sSQL += " AND (LETTURA = " & nLetAtt & ")"
    '            End If
    '        End If
    '        Log.Debug("GestLetture::GetDettaglioLetture::prelevo da TP_LETTURE::" & sSQL)
    '        drDetailsLetture = iDB.GetDataReader(sSQL)
    '        If drDetailsLetture.Read Then
    '            DettaglioLetture.IdLettura = MyUtility.CIdFromDB(drDetailsLetture("CODLETTURA"))
    '            DettaglioLetture.nIdContatore = MyUtility.CIdFromDB(drDetailsLetture("CODCONTATORE"))
    '            DettaglioLetture.nIdContatorePrec = MyUtility.CIdFromDB(drDetailsLetture("CODCONTATOREPRECEDENTE"))
    '            DettaglioLetture.nIdPeriodo = MyUtility.CIdFromDB(drDetailsLetture("CODPERIODO"))
    '            DettaglioLetture.tDataLetturaAtt = FncGen.GiraDataFromDB(Utility.StringOperation.FormatString(drDetailsLetture("DATALETTURA")))
    '            DettaglioLetture.nLetturaAtt = MyUtility.CIdFromDB(drDetailsLetture("LETTURA"))
    '            DettaglioLetture.nCodModoLett = MyUtility.CIdFromDB(drDetailsLetture("CODMODALITALETTURA"))
    '            DettaglioLetture.bIsFatturata = Utility.StringOperation.FormatBool(drDetailsLetture("FATTURAZIONE"))
    '            DettaglioLetture.nConsumo = MyUtility.CIdFromDB(drDetailsLetture("CONSUMO"))
    '            DettaglioLetture.sNote = Utility.StringOperation.FormatString(drDetailsLetture("NOTE"))
    '            DettaglioLetture.nGiorni = MyUtility.CIdFromDB(drDetailsLetture("GIORNIDICONSUMO"))
    '            DettaglioLetture.bIsIncongruente = Utility.StringOperation.FormatBool(drDetailsLetture("INCONGRUENTE"))
    '            DettaglioLetture.bIsIncongruenteForzato = Utility.StringOperation.FormatBool(drDetailsLetture("INCONGRUENTEFORZATO"))
    '            DettaglioLetture.nConsumoTeorico = MyUtility.CIdFromDB(drDetailsLetture("CONSUMOTEORICO"))
    '            DettaglioLetture.bFattSospesa = Utility.StringOperation.FormatBool(drDetailsLetture("FATTURAZIONESOSPESA"))
    '            DettaglioLetture.sNUtente = Utility.StringOperation.FormatString(drDetailsLetture("NUMEROUTENTE"))
    '            If Utility.StringOperation.FormatString(drDetailsLetture("DATALETTURAPRECEDENTE")) <> "" Then
    '                DettaglioLetture.tDataLetturaPrec = FncGen.GiraDataFromDB(Utility.StringOperation.FormatString(drDetailsLetture("DATALETTURAPRECEDENTE")))
    '            End If
    '            DettaglioLetture.nLetturaPrec = MyUtility.CIdFromDB(drDetailsLetture("LETTURAPRECEDENTE"))
    '            DettaglioLetture.bIsPrimaLettura = Utility.StringOperation.FormatBool(drDetailsLetture("PRIMALETTURA"))
    '            DettaglioLetture.nIdUtente = MyUtility.CIdFromDB(drDetailsLetture("CODUTENTE"))
    '            DettaglioLetture.bIsUltimaLettura = Utility.StringOperation.FormatBool(drDetailsLetture("ULTIMALETTURA"))
    '            DettaglioLetture.nConsumoPrec = MyUtility.CIdFromDB(drDetailsLetture("CONSUMOTOTALEPREC"))
    '            DettaglioLetture.nGiorniPrec = MyUtility.CIdFromDB(drDetailsLetture("GIORNIDICONSUMOTOTPREC"))
    '            If Utility.StringOperation.FormatString(drDetailsLetture("DATAFATTURAZIONE")) <> "" Then
    '                DettaglioLetture.tDataFatt = FncGen.GiraDataFromDB(Utility.StringOperation.FormatString(drDetailsLetture("DATAFATTURAZIONE")))
    '            End If
    '            DettaglioLetture.nIdStatoLettura = MyUtility.CIdFromDB(drDetailsLetture("IDSTATOLETTURA"))
    '            If Utility.StringOperation.FormatString(drDetailsLetture("DATADIPASSAGGIO")) <> "" Then
    '                DettaglioLetture.tDataPassaggio = FncGen.GiraDataFromDB(Utility.StringOperation.FormatString(drDetailsLetture("DATADIPASSAGGIO")))
    '            End If
    '            If Not IsDBNull(drDetailsLetture("COD_ANOMALIA1")) Then
    '                DettaglioLetture.nIdAnomalia1 = MyUtility.CIdFromDB(drDetailsLetture("COD_ANOMALIA1"))
    '            End If
    '            If Not IsDBNull(drDetailsLetture("COD_ANOMALIA2")) Then
    '                DettaglioLetture.nIdAnomalia2 = MyUtility.CIdFromDB(drDetailsLetture("COD_ANOMALIA2"))
    '            End If
    '            If Not IsDBNull(drDetailsLetture("COD_ANOMALIA3")) Then
    '                DettaglioLetture.nIdAnomalia3 = MyUtility.CIdFromDB(drDetailsLetture("COD_ANOMALIA3"))
    '            End If
    '            DettaglioLetture.sLetturaTeorica = Utility.StringOperation.FormatString(drDetailsLetture("LETTURATEORICA"))
    '            DettaglioLetture.bIsGiroContatore = Utility.StringOperation.FormatBool(drDetailsLetture("GIROCONTATORE"))
    '            DettaglioLetture.bIsStorica = Utility.StringOperation.FormatBool(drDetailsLetture("STORICA"))
    '            DettaglioLetture.bIsStoricizzata = Utility.StringOperation.FormatBool(drDetailsLetture("STORICIZZATA"))
    '            DettaglioLetture.sProvenienza = Utility.StringOperation.FormatString(drDetailsLetture("PROVENIENZA"))
    '            DettaglioLetture.nConsumoSubContatore = MyUtility.CIdFromDB(drDetailsLetture("CONSUMOSUB"))
    '            If Not IsDBNull(drDetailsLetture("DATA_INSERIMENTO")) Then
    '                DettaglioLetture.tDataInserimento = CDate(drDetailsLetture("DATA_INSERIMENTO"))
    '            End If
    '            If Not IsDBNull(drDetailsLetture("DATA_VARIAZIONE")) Then
    '                DettaglioLetture.tDataVariazione = CDate(drDetailsLetture("DATA_VARIAZIONE"))
    '            End If
    '            If Not IsDBNull(drDetailsLetture("AZIONE")) Then
    '                DettaglioLetture.sAzione = Utility.StringOperation.FormatString(drDetailsLetture("AZIONE"))
    '            End If
    '        End If
    '        drDetailsLetture.Close()
    '        Return DettaglioLetture
    '    Catch ex As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestLetture.GetDettaglioLetture.errore: ", ex)
    '        Return Nothing
    '    End Try
    'End Function

    Public Function GetLetSucIsFatturata(ByVal IDcontatore As Integer, ByVal tDataLetAtt As DateTime, ByRef sMatricola As String) As Boolean
        Dim IsFatturata As Boolean = False
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView

        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetElencoLetture", "IDCONTATORE", "DATALETTURA")
                    dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDCONTATORE", IDcontatore) _
                                        , ctx.GetParam("DATALETTURA", FncGen.FormattaData("A", "", tDataLetAtt, False))
                                    )
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            sMatricola = StringOperation.FormatString(myRow("matricola"))
                            If StringOperation.FormatInt(myRow("fatturata")) = 1 Then
                                IsFatturata = True
                            End If
                        Next
                    End If
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + " - OPENgovH2O.GestContratti.GetLetSucIsFattura.errore: ", ex)
                Finally
                    dvMyDati.Dispose()
                End Try
                ctx.Dispose()
            End Using
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.GetLetSucIsFattura.errore: ", ex)
        End Try
        Return IsFatturata
    End Function

    ''' <summary>
    ''' Restituisce il numero di letture di un determinato contatore considerando le ultime 5 ed escludendo la prima
    ''' </summary>
    ''' <param name="IDContatore">ID del contatore</param>
    ''' <returns>Un oggetto di tipo GetListaLetture</returns>
    ''' <remarks>
    ''' </remarks>
    Public Function BindData(ByVal IDContatore As Integer) As DataView
        Dim myDv As DataView = Nothing
        Try
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.CommandText = "SELECT TOP 5 * "
            cmdMyCommand.CommandText += " FROM dbo.OPENgov_ELENCO_LETTURE"
            cmdMyCommand.CommandText += " WHERE (CODCONTATORE = " & IDContatore & ")"
            cmdMyCommand.CommandText += " ORDER BY DATALETTURA DESC"

            myDv = iDB.GetDataView(cmdMyCommand)
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.BindData.errore: ", ex)
        End Try
        Return myDv
    End Function

    ''' <summary>
    ''' Verifica se ci sono delle letture oppure no associate a un determinato contatore.
    ''' </summary>
    ''' <param name="IDContatore">ID del contatore</param>
    ''' <param name="CodUtente">ID anagrafico</param>
    ''' <returns>Valore Bolean. 
    ''' FALSE = non sono presenti delle letture
    ''' TRUE = sono presenti delle letture</returns>
    ''' <remarks>
    ''' </remarks>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Function VerificaEsistenzaLettura(ByVal IDContatore As Integer, ByVal CodUtente As Integer) As Boolean
        Dim dvMyDati As New DataView

        Try
            dvMyDati = New clsLetture().GetTopLetture(ConstSession.DBType, ConstSession.StringConnection, 5, IDContatore, "", "")
            For Each myRow As DataRowView In dvMyDati
                If StringOperation.FormatBool(myRow("INCONGRUENTEFORZATO")) = True Then
                    VerificaEsistenzaLettura = False
                Else
                    VerificaEsistenzaLettura = True
                End If
            Next
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.VerificaEsistenzaLettura.errore: ", ex)
        Finally
            dvMyDati.Dispose()
        End Try
    End Function
    'Public Function VerificaEsistenzaLettura(ByVal IDContatore As Integer, ByVal CodUtente As Integer) As Boolean
    '    Dim sSQL As String
    '    Dim rdTemp As SqlDataReader
    '    VerificaEsistenzaLettura = False

    '    Try
    '        sSQL = "SELECT TOP 1 TP_LETTURE.*  " & vbCrLf
    '        sSQL += "FROM TP_LETTURE " & vbCrLf
    '        sSQL += "WHERE (1=1)" & vbCrLf
    '        'sSQL+="CODUTENTE=" & CodUtente & vbCrLf
    '        sSQL += "AND CODCONTATORE=" & IDContatore & vbCrLf
    '        sSQL += "AND (STORICIZZATA=0 OR STORICIZZATA IS NULL)" & vbCrLf
    '        sSQL += "AND (TP_LETTURE.DATADIPASSAGGIO IS NULL OR TP_LETTURE.DATADIPASSAGGIO='')"
    '        sSQL += "ORDER BY DATALETTURA "
    '        rdTemp = iDB.GetDataReader(sSQL)
    '        If rdTemp.Read Then
    '            If Utility.StringOperation.FormatBool(rdTemp("INCONGRUENTEFORZATO")) = True Then
    '                VerificaEsistenzaLettura = False
    '            Else
    '                VerificaEsistenzaLettura = True
    '            End If
    '        End If
    '        rdTemp.Close()
    '    Catch ex As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestLetture.VerificaEsistenzaLettura.errore: ", ex)
    '    End Try
    'End Function

    ''' <summary>
    ''' Restituisce l'elenco dei giri e lo stradario
    ''' </summary>
    ''' <param name="CodEnte"></param>
    ''' <param name="codiceIstat"></param>
    ''' <returns>Un oggetto di tipo DetailsSearchLetture
    ''' </returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Function GetDetailsSearchLetture(ByVal CodEnte As Integer, ByVal codiceIstat As String) As DetailsSearchLetture
        Dim mySearch As New DetailsSearchLetture
        Dim sSQL As String = ""
        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "DetailGiri", "CODENTE")
                mySearch.dsGiro = ctx.GetDataSet(sSQL, "TBL", ctx.GetParam("CODENTE", CodEnte))
                ctx.Dispose()
            End Using
        Catch ex As Exception
            Log.Debug(CodEnte + " - OPENgovH2O.GestLetture.GetDetailsSearchLetture.errore: ", ex)
        End Try
        mySearch.lngIDGiro = -1
        mySearch.lngIDUbicazione = -1
        Return mySearch
    End Function
    'Public Function GetDetailsSearchLetture(ByVal CodEnte As Integer, ByVal codiceIstat As String) As DetailsSearchLetture

    '    Dim DetailsSearchLetture As New DetailsSearchLetture

    '    DetailsSearchLetture.dsGiro = iDB.RunSPReturnDataSet("DetailGiri", "TP_GIRI", New SqlParameter("@CodEnte", CodEnte))

    '    DetailsSearchLetture.drUbicazione = iDB.GetDataReader("Select COD_STRADA,TIPO_STRADA,STRADA  From STRADARIO WHERE CODICE_ISTAT =" & codiceIstat)
    '    DetailsSearchLetture.lngIDGiro = -1

    '    DetailsSearchLetture.lngIDUbicazione = -1
    '    Return DetailsSearchLetture

    'End Function

    ''' <summary>
    ''' Restituisce la query e il numero di contatori validi (attivi all'interno del periodo in linea) in base ai parametri passati al metodo
    ''' </summary>
    ''' <param name="Intestatario">Intestatario del contatore</param>
    ''' <param name="Utente">Utente del contatore</param>
    ''' <param name="IDVia">codice VIA dello stradario</param>
    ''' <param name="Giro">giro di lettura</param>
    ''' <param name="NumeroUtente"></param>
    ''' <param name="Cessati">Paramentro non utilizzato</param>
    ''' <param name="sIdEnte"></param>
    ''' <param name="matricola">matricola del contatore</param>
    ''' <param name="Boolsub">se è un contatore di tipo sub</param>
    ''' <param name="IsLetturaPresente"></param>
    ''' <param name="IsLetturaMancante"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Function GetListaContatori(ByVal Intestatario As String, ByVal Utente As String, ByVal IDVia As Integer, ByVal Giro As Integer, ByVal NumeroUtente As String, ByVal Cessati As Integer, ByVal sIdEnte As String, ByVal matricola As String, ByVal Boolsub As Boolean, ByVal IsLetturaPresente As Boolean, ByVal IsLetturaMancante As Boolean) As DataView
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Try
            Dim oPeriodo As New TabelleDiDecodifica.DetailPeriodo
            oPeriodo = New TabelleDiDecodifica.DBPeriodo().GetPeriodo(ConstSession.IdPeriodo)

            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetContatoriLetture", "IDENTE", "IDPERIODO", "DAL", "AL", "GIRO", "NUMEROUTENTE", "IDVIA", "ISSUB", "INTESTATARIO", "UTENTE", "MATRICOLA", "LETTURAPRESENTE", "LETTURAMANCANTE")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", sIdEnte) _
                        , ctx.GetParam("IDPERIODO", ConstSession.IdPeriodo) _
                        , ctx.GetParam("DAL", StringOperation.FormatDateTime(oPeriodo.DaData)) _
                        , ctx.GetParam("AL", StringOperation.FormatDateTime(oPeriodo.AData)) _
                        , ctx.GetParam("GIRO", Giro) _
                        , ctx.GetParam("NUMEROUTENTE", NumeroUtente) _
                        , ctx.GetParam("IDVIA", IDVia) _
                        , ctx.GetParam("ISSUB", Boolsub) _
                        , ctx.GetParam("INTESTATARIO", Intestatario.Replace("'", "''").Replace("*", "%")) _
                        , ctx.GetParam("UTENTE", Utente.Replace("'", "''").Replace("*", "%")) _
                        , ctx.GetParam("MATRICOLA", matricola.Replace("'", "''").Replace("*", "%")) _
                        , ctx.GetParam("LETTURAPRESENTE", IsLetturaPresente) _
                        , ctx.GetParam("LETTURAMANCANTE", IsLetturaMancante)
                    )
                ctx.Dispose()
            End Using
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.GetListaContatori.errore: ", ex)
        End Try
        Return dvMyDati
    End Function
    'Public Function GetListaContatori(ByVal Intestatario As String, ByVal Utente As String, ByVal IDVia As Integer, ByVal Giro As Integer, ByVal NumeroUtente As String, ByVal Cessati As Integer, ByVal sIdEnte As String, ByVal matricola As String, ByVal Boolsub As Boolean, ByVal IsLetturaPresente As Boolean) As DataView
    '    Dim myDv As DataView = Nothing
    '    Try
    '        Dim SearchChar As String = "*"
    '        'determino i dati del periodo
    '        Dim clsPeriodo As New TabelleDiDecodifica.DBPeriodo
    '        Dim oPeriodo As TabelleDiDecodifica.DetailPeriodo = clsPeriodo.GetPeriodo(ConstSession.IdPeriodo)
    '        Dim ModDate As New ClsGenerale.Generale

    '        cmdMyCommand.CommandType = CommandType.Text
    '        cmdMyCommand.CommandText = "SELECT OPENgov_ELENCO_CONTATORI.COGNOME_INT, OPENgov_ELENCO_CONTATORI.NOME_INT"
    '        cmdMyCommand.CommandText += " ,OPENgov_ELENCO_CONTATORI.COGNOME_UT,OPENgov_ELENCO_CONTATORI.NOME_UT"
    '        cmdMyCommand.CommandText += " ,OPENgov_ELENCO_CONTATORI.VIA_UBICAZIONE,OPENgov_ELENCO_CONTATORI.CIVICO_UBICAZIONE"
    '        cmdMyCommand.CommandText += " ,OPENgov_ELENCO_CONTATORI.MATRICOLA,OPENgov_ELENCO_CONTATORI.NUMEROUTENTE,OPENgov_ELENCO_CONTATORI.CODCONTATORE"
    '        cmdMyCommand.CommandText += " , CASE WHEN NOT MAX(LETTURE.CODCONTATORE) IS NULL THEN 'X' ELSE NULL END AS LETTURA"

    '        cmdMyCommand.CommandText += " FROM OPENgov_ELENCO_CONTATORI"
    '        cmdMyCommand.CommandText += " LEFT JOIN ("
    '        cmdMyCommand.CommandText += " 	SELECT CODCONTATORE"
    '        cmdMyCommand.CommandText += " 	FROM TP_LETTURE "
    '        cmdMyCommand.CommandText += " 	WHERE (CODPERIODO =" & ConstSession.IdPeriodo & ")"
    '        cmdMyCommand.CommandText += " 	AND (PRIMALETTURA IS NULL OR PRIMALETTURA=0)"
    '        cmdMyCommand.CommandText += " 	) LETTURE ON OPENGOV_ELENCO_CONTATORI.CODCONTATORE=LETTURE.CODCONTATORE"

    '        cmdMyCommand.CommandText += " WHERE (1 = 1)"
    '        cmdMyCommand.CommandText += " AND (DATAATTIVAZIONE IS NOT NULL AND DATAATTIVAZIONE <> '' and DATAATTIVAZIONE <= " & ModDate.GiraData(oPeriodo.AData) & ") "
    '        cmdMyCommand.CommandText += " AND ((DATACESSAZIONE IS NULL OR DATACESSAZIONE ='') OR (DATACESSAZIONE >= " & ModDate.GiraData(oPeriodo.DaData) & "))"
    '        cmdMyCommand.CommandText += " AND (CODENTE=" & sIdEnte & ")"
    '        If Giro <> -1 Then
    '            cmdMyCommand.CommandText += " AND (IDGIRO=" & Giro & ")"
    '        End If
    '        If Len(Trim(NumeroUtente)) > 0 Then
    '            cmdMyCommand.CommandText += " AND (NUMEROUTENTE=" & NumeroUtente & ")"
    '        End If
    '        If IDVia <> -1 Then
    '            cmdMyCommand.CommandText += " AND (COD_STRADA=" & IDVia & ")"
    '        End If
    '        If Boolsub = True Then
    '            cmdMyCommand.CommandText += " AND (NOT CODCONTATORESUB IS NULL)"
    '        End If
    '        If Len(Trim(Intestatario)) > 0 Then
    '            cmdMyCommand.CommandText += " AND (COGNOME_INT+' '+NOME_INT like  '" & Replace(Replace(Trim(Intestatario), "'", "''"), "*", "%") & "%')"
    '        End If
    '        If Len(Trim(Utente)) > 0 Then
    '            cmdMyCommand.CommandText += " AND (COGNOME_UT+' '+NOME_UT like  '" & Replace(Replace(Trim(Utente), "'", "''"), "*", "%") & "%')"
    '        End If
    '        If Len(Trim(matricola)) > 0 Then
    '            If InStr(matricola, SearchChar) > 0 Then
    '                cmdMyCommand.CommandText += " AND (MATRICOLA like  '" & Replace(Replace(Trim(matricola), "'", "''"), "*", "%") & "%')"
    '            Else
    '                cmdMyCommand.CommandText += " AND (MATRICOLA =" & matricola & ")"
    '            End If
    '        End If
    '        If IsLetturaPresente = True Then
    '            cmdMyCommand.CommandText += " AND (NOT LETTURE.CODCONTATORE IS NULL)"
    '        End If

    '        cmdMyCommand.CommandText += " GROUP BY OPENgov_ELENCO_CONTATORI.COGNOME_INT, OPENgov_ELENCO_CONTATORI.NOME_INT"
    '        cmdMyCommand.CommandText += " ,OPENgov_ELENCO_CONTATORI.COGNOME_UT,OPENgov_ELENCO_CONTATORI.NOME_UT"
    '        cmdMyCommand.CommandText += " ,OPENgov_ELENCO_CONTATORI.VIA_UBICAZIONE,OPENgov_ELENCO_CONTATORI.CIVICO_UBICAZIONE"
    '        cmdMyCommand.CommandText += " ,OPENgov_ELENCO_CONTATORI.MATRICOLA,OPENgov_ELENCO_CONTATORI.NUMEROUTENTE,OPENgov_ELENCO_CONTATORI.CODCONTATORE"

    '        cmdMyCommand.CommandText += " ORDER BY "
    '        cmdMyCommand.CommandText += " COGNOME_INT+' '+NOME_INT,"
    '        cmdMyCommand.CommandText += " COGNOME_UT+' '+NOME_UT"
    '        cmdMyCommand.CommandText += " ,MATRICOLA"
    '        Log.Debug("GetListaContatori::query::" & cmdMyCommand.CommandText)
    '        myDv = iDB.GetDataView(cmdMyCommand)
    '    Catch ex As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestLetture.GetListaContatori.errore: ", ex)
    '    End Try
    '    Return myDv
    'End Function

    ''' <summary>
    ''' Dato l'ID restituisce la descrizione di un'anomalia
    ''' </summary>
    ''' <param name="IDAnomalia"></param>
    ''' <returns>Stringa</returns>
    ''' <remarks>
    ''' </remarks>
    Public Function DescrizioneAnomalie(ByVal IDAnomalia As Object) As String
        DescrizioneAnomalie = ""
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Try
            If Utility.StringOperation.FormatString(IDAnomalia) = "" Then Exit Function

            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "sp_Anomalie", "IDANOMALIA")
                    dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDANOMALIA", IDAnomalia))
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            DescrizioneAnomalie = Utility.StringOperation.FormatString(myRow("DESCRIZIONE"))
                        Next
                    End If
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + " - OPENgovH2O.GestLetture.DescrizioneAnomalie.errore: ", ex)
                Finally
                    dvMyDati.Dispose()
                End Try
                ctx.Dispose()
            End Using
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.DescrizioneAnomalie.errore: ", ex)
        End Try
        Return DescrizioneAnomalie
    End Function

    ''' <summary>
    ''' Esegue l'aggiornamento di una lettura esistente oppure un inserimento di una nuova lettura utilizzando i parametri che vengono passati al metodo 
    ''' </summary>
    ''' <param name="IDLettura"></param>
    ''' <param name="oDatiLettura"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Function SetLetture(ByVal IDLettura As Integer, ByVal oDatiLettura As ObjLettura) As Integer
        Try
            If oDatiLettura.nLetturaPrec > 0 Then
                oDatiLettura.bIsPrimaLettura = False
            Else
                If Not clsLetture.VerificaTolleranzaConsumo(oDatiLettura.nConsumo, oDatiLettura.nConsumoTeorico, oDatiLettura.nIdContatore) Then
                    oDatiLettura.bIsIncongruente = True
                End If
            End If

            IDLettura = SetLettura(oDatiLettura)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.SetLetture.errore: ", ex)
            Return IDLettura = -1
        End Try
        Return IDLettura
    End Function
    'Public Function SetLetture(ByVal IDLettura As Integer, ByVal oDatiLettura As ObjLettura) As Integer
    '    Dim sSQL As String
    '    Dim lngTipoOp As Long = DBOperation.DB_INSERT
    '    'Dim IDValue As Integer
    '    Dim sqlTrans As SqlTransaction
    '    Dim sqlConn As New SqlConnection
    '    'Dim Inconguente As Boolean = False
    '    'Dim Anomalie As String
    '    'Dim lngLetturaPrecedente As Long
    '    'Dim dataLetturaprecedente As String
    '    'Dim PrimaLettura As Boolean = True
    '    Dim sqlCmdInsert As SqlCommand
    '    Dim drMaxValue As SqlDataReader

    '    Try
    '        If IDLettura > 0 Then
    '            lngTipoOp = DBOperation.DB_UPDATE
    '        End If

    '        Try
    '            sqlConn.ConnectionString = ConstSession.StringConnection
    '            sqlConn.Open()
    '            sqlTrans = sqlConn.BeginTransaction

    '            If oDatiLettura.nLetturaPrec > 0 Then
    '                oDatiLettura.bIsPrimaLettura = False
    '            Else
    '                If Not clsLetture.VerificaTolleranzaConsumo(oDatiLettura.nConsumo, oDatiLettura.nConsumoTeorico, oDatiLettura.nIdContatore) Then
    '                    oDatiLettura.bIsIncongruente = True
    '                End If
    '            End If

    '            'eseguo l'insert in TP_LETTURE
    '            sSQL = GetSQLLetture(lngTipoOp, oDatiLettura)
    '            Log.Debug("GestLetture::SetLetture::eseguo l'insert in TP_LETTURE::" & sSQL)
    '            sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '            sqlCmdInsert.ExecuteNonQuery()

    '            sqlTrans.Commit()

    '            '***prelevo l'ultima lettura presente***
    '            sSQL = "SELECT MAX(TP_LETTURE.CODLETTURA) AS ID_MAX FROM TP_LETTURE WITH (NOLOCK)"
    '            drMaxValue = iDB.GetDataReader(sSQL)
    '            If drMaxValue.Read = True Then
    '                If Not IsDBNull(drMaxValue("ID_MAX")) Then
    '                    IDLettura = drMaxValue("ID_MAX")
    '                End If
    '            End If
    '            drMaxValue.Close()

    '        Catch ex As Exception
    '            sqlTrans.Rollback()
    '            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestLetture.SetLetture.errore: ", ex)
    '            Return -1
    '        Finally
    '            sqlConn.Close()
    '        End Try

    '        Return IDLettura
    '    Catch ex As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestLetture.SetLetture.errore: ", ex)
    '        Return -1
    '    End Try
    'End Function
    Public Function SetLettura(ByVal myLettura As ObjLettura) As Integer
        Dim sSQL As String
        Dim dvMyDati As New DataView
        Dim myID As Integer = -1

        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_TP_LETTURE_IU", "CODLETTURA", "CODCONTATORE", "CODCONTATOREPRECEDENTE", "CODPERIODO", "DATALETTURA", "LETTURA", "CODMODALITALETTURA", "CONSUMO", "NOTE", "GIORNIDICONSUMO", "CONSUMOTEORICO", "CODUTENTE", "IDSTATOLETTURA", "COD_ANOMALIA1", "COD_ANOMALIA2", "COD_ANOMALIA3", "INCONGRUENTE", "DATADIPASSAGGIO", "LETTURATEORICA", "DATALETTURAPRECEDENTE", "LETTURAPRECEDENTE", "PRIMALETTURA", "DATA_INSERIMENTO", "DATA_VARIAZIONE", "AZIONE", "GIROCONTATORE", "FATTURAZIONESOSPESA")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("CODLETTURA", myLettura.IdLettura) _
                        , ctx.GetParam("CODCONTATORE", myLettura.nIdContatore) _
                        , ctx.GetParam("CODCONTATOREPRECEDENTE", myLettura.nIdContatorePrec) _
                        , ctx.GetParam("CODPERIODO", myLettura.nIdPeriodo) _
                        , ctx.GetParam("DATALETTURA", myLettura.tDataLetturaAtt) _
                        , ctx.GetParam("LETTURA", myLettura.nLetturaAtt) _
                        , ctx.GetParam("CODMODALITALETTURA", myLettura.nCodModoLett) _
                        , ctx.GetParam("CONSUMO", myLettura.nConsumo) _
                        , ctx.GetParam("NOTE", FncGen.ReplaceChar(myLettura.sNote)) _
                        , ctx.GetParam("GIORNIDICONSUMO", myLettura.nGiorni) _
                        , ctx.GetParam("CONSUMOTEORICO", myLettura.nConsumoTeorico) _
                        , ctx.GetParam("CODUTENTE", myLettura.nIdUtente) _
                        , ctx.GetParam("IDSTATOLETTURA", myLettura.nIdStatoLettura) _
                        , ctx.GetParam("COD_ANOMALIA1", myLettura.nIdAnomalia1) _
                        , ctx.GetParam("COD_ANOMALIA2", myLettura.nIdAnomalia2) _
                        , ctx.GetParam("COD_ANOMALIA3", myLettura.nIdAnomalia3) _
                        , ctx.GetParam("INCONGRUENTE", myLettura.bIsIncongruente) _
                        , ctx.GetParam("DATADIPASSAGGIO", myLettura.tDataPassaggio) _
                        , ctx.GetParam("LETTURATEORICA", myLettura.sLetturaTeorica) _
                        , ctx.GetParam("DATALETTURAPRECEDENTE", myLettura.tDataLetturaPrec) _
                        , ctx.GetParam("LETTURAPRECEDENTE", myLettura.nLetturaPrec) _
                        , ctx.GetParam("PRIMALETTURA", myLettura.bIsPrimaLettura) _
                        , ctx.GetParam("DATA_INSERIMENTO", StringOperation.FormatDateTime(myLettura.tDataInserimento)) _
                        , ctx.GetParam("DATA_VARIAZIONE", StringOperation.FormatDateTime(myLettura.tDataVariazione)) _
                        , ctx.GetParam("AZIONE", myLettura.sAzione) _
                        , ctx.GetParam("GIROCONTATORE", myLettura.bIsGiroContatore) _
                        , ctx.GetParam("FATTURAZIONESOSPESA", myLettura.bFattSospesa)
                    )
                ctx.Dispose()
            End Using
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    myID = Utility.StringOperation.FormatString(myRow("id"))
                Next
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.SetLettura.errore: ", ex)
            Return -1
        Finally
            dvMyDati.Dispose()
        End Try
        Return myID
    End Function
    'Public Function SetLettura(ByVal myLettura As ObjLettura) As Integer
    '    Dim sSQL As String
    '    Dim dvMyDati As New DataView
    '    Dim myID As Integer = -1

    '    Try
    '        Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
    '            sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_TP_LETTURE_IU", "CODLETTURA", "CODCONTATORE", "CODCONTATOREPRECEDENTE", "CODPERIODO", "DATALETTURA", "LETTURA", "CODMODALITALETTURA", "CONSUMO", "NOTE", "GIORNIDICONSUMO", "CONSUMOTEORICO", "CODUTENTE", "IDSTATOLETTURA", "COD_ANOMALIA1", "COD_ANOMALIA2", "COD_ANOMALIA3", "INCONGRUENTE", "DATADIPASSAGGIO", "LETTURATEORICA", "DATALETTURAPRECEDENTE", "LETTURAPRECEDENTE", "PRIMALETTURA", "DATA_INSERIMENTO", "DATA_VARIAZIONE", "AZIONE", "GIROCONTATORE", "FATTURAZIONESOSPESA")
    '            dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("CODLETTURA", myLettura.IdLettura) _
    '                    , ctx.GetParam("CODCONTATORE", myLettura.nIdContatore) _
    '                    , ctx.GetParam("CODCONTATOREPRECEDENTE", myLettura.nIdContatorePrec) _
    '                    , ctx.GetParam("CODPERIODO", myLettura.nIdPeriodo) _
    '                    , ctx.GetParam("DATALETTURA", FncGen.GiraData(StringOperation.FormatDateTime(myLettura.tDataLetturaAtt))) _
    '                    , ctx.GetParam("LETTURA", myLettura.nLetturaAtt) _
    '                    , ctx.GetParam("CODMODALITALETTURA", myLettura.nCodModoLett) _
    '                    , ctx.GetParam("CONSUMO", myLettura.nConsumo) _
    '                    , ctx.GetParam("NOTE", FncGen.ReplaceChar(myLettura.sNote)) _
    '                    , ctx.GetParam("GIORNIDICONSUMO", myLettura.nGiorni) _
    '                    , ctx.GetParam("CONSUMOTEORICO", myLettura.nConsumoTeorico) _
    '                    , ctx.GetParam("CODUTENTE", myLettura.nIdUtente) _
    '                    , ctx.GetParam("IDSTATOLETTURA", myLettura.nIdStatoLettura) _
    '                    , ctx.GetParam("COD_ANOMALIA1", myLettura.nIdAnomalia1) _
    '                    , ctx.GetParam("COD_ANOMALIA2", myLettura.nIdAnomalia2) _
    '                    , ctx.GetParam("COD_ANOMALIA3", myLettura.nIdAnomalia3) _
    '                    , ctx.GetParam("INCONGRUENTE", myLettura.bIsIncongruente) _
    '                    , ctx.GetParam("DATADIPASSAGGIO", FncGen.GiraData(StringOperation.FormatDateTime(myLettura.tDataPassaggio))) _
    '                    , ctx.GetParam("LETTURATEORICA", myLettura.sLetturaTeorica) _
    '                    , ctx.GetParam("DATALETTURAPRECEDENTE", FncGen.GiraData(StringOperation.FormatDateTime(myLettura.tDataLetturaPrec))) _
    '                    , ctx.GetParam("LETTURAPRECEDENTE", myLettura.nLetturaPrec) _
    '                    , ctx.GetParam("PRIMALETTURA", myLettura.bIsPrimaLettura) _
    '                    , ctx.GetParam("DATA_INSERIMENTO", StringOperation.FormatDateTime(myLettura.tDataInserimento)) _
    '                    , ctx.GetParam("DATA_VARIAZIONE", StringOperation.FormatDateTime(myLettura.tDataVariazione)) _
    '                    , ctx.GetParam("AZIONE", myLettura.sAzione) _
    '                    , ctx.GetParam("GIROCONTATORE", myLettura.bIsGiroContatore) _
    '                    , ctx.GetParam("FATTURAZIONESOSPESA", myLettura.bFattSospesa)
    '                )
    '            ctx.Dispose()
    '        End Using
    '        If Not dvMyDati Is Nothing Then
    '            For Each myRow As DataRowView In dvMyDati
    '                myID = Utility.StringOperation.FormatString(myRow("id"))
    '            Next
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestLetture.SetLettura.errore: ", ex)
    '        Return -1
    '    Finally
    '        dvMyDati.Dispose()
    '    End Try
    '    Return myID
    'End Function

    'Private Function GetSQLLetture(ByVal nDBOperation As DBOperation, ByVal oMyLettura As ObjLettura) As String
    '    Dim sSQL As String

    '    Try
    '        Select Case nDBOperation
    '            Case DBOperation.DB_INSERT
    '                sSQL = "INSERT INTO TP_LETTURE"
    '                sSQL += " (CODCONTATORE, CODCONTATOREPRECEDENTE,CODPERIODO, DATALETTURA, LETTURA,"
    '                sSQL += " CODMODALITALETTURA , CONSUMO, NOTE, GIORNIDICONSUMO, "
    '                sSQL += " CONSUMOTEORICO,CODUTENTE,IDSTATOLETTURA,"
    '                sSQL += " COD_ANOMALIA1,COD_ANOMALIA2,COD_ANOMALIA3,"
    '                sSQL += " INCONGRUENTE,DATADIPASSAGGIO,LETTURATEORICA,"
    '                sSQL += " DATALETTURAPRECEDENTE,LETTURAPRECEDENTE,PRIMALETTURA,DATA_INSERIMENTO, AZIONE)"
    '                sSQL += " VALUES ( " & oMyLettura.nIdContatore & ","
    '                If oMyLettura.nIdContatorePrec <> -1 Then
    '                    sSQL += oMyLettura.nIdContatorePrec & ","
    '                Else
    '                    sSQL += "Null,"
    '                End If
    '                sSQL += oMyLettura.nIdPeriodo & ","
    '                sSQL += "'" & FncGen.GiraData(oMyLettura.tDataLetturaAtt) & "'," & oMyLettura.nLetturaAtt & ","
    '                If oMyLettura.nCodModoLett > 0 Then
    '                    sSQL += oMyLettura.nCodModoLett & ","
    '                Else
    '                    sSQL += "NULL,"
    '                End If
    '                sSQL += oMyLettura.nConsumo & ",'" & FncGen.ReplaceChar(oMyLettura.sNote) & "'," & oMyLettura.nGiorni & ","
    '                sSQL += oMyLettura.nConsumoTeorico & "," & oMyLettura.nIdUtente & ","
    '                If oMyLettura.nIdStatoLettura <> -1 Then
    '                    sSQL += oMyLettura.nIdStatoLettura & ","
    '                Else
    '                    sSQL += "NULL,"
    '                End If
    '                If oMyLettura.nIdAnomalia1 <> -1 Then
    '                    sSQL += oMyLettura.nIdAnomalia1 & ","
    '                Else
    '                    sSQL += "NULL,"
    '                End If
    '                If oMyLettura.nIdAnomalia2 <> -1 Then
    '                    sSQL += oMyLettura.nIdAnomalia2 & ","
    '                Else
    '                    sSQL += "NULL,"
    '                End If
    '                If oMyLettura.nIdAnomalia3 <> -1 Then
    '                    sSQL += oMyLettura.nIdAnomalia3 & ","
    '                Else
    '                    sSQL += "NULL,"
    '                End If
    '                If oMyLettura.bIsIncongruente = True Then
    '                    sSQL += " 1,"
    '                Else
    '                    sSQL += " NULL,"
    '                End If
    '                If oMyLettura.tDataPassaggio <> Date.MinValue Then
    '                    sSQL += " '" & FncGen.GiraData(oMyLettura.tDataPassaggio) & "',"
    '                Else
    '                    sSQL += "'',"
    '                End If
    '                sSQL += "'" & oMyLettura.sLetturaTeorica & "',"
    '                If oMyLettura.tDataLetturaPrec <> Date.MinValue Then
    '                    sSQL += " '" & FncGen.GiraData(oMyLettura.tDataLetturaPrec) & "',"
    '                Else
    '                    sSQL += "'',"
    '                End If
    '                sSQL += oMyLettura.nLetturaPrec & ","
    '                If oMyLettura.bIsPrimaLettura = True Then
    '                    sSQL += CInt(oMyLettura.bIsPrimaLettura) & ","
    '                Else
    '                    sSQL += " NULL,"
    '                End If
    '                sSQL += "'" & FncGen.ReplaceDataForDB(oMyLettura.tDataInserimento) & "','" & oMyLettura.sAzione & "')"

    '            Case DBOperation.DB_UPDATE
    '                sSQL = ""
    '                sSQL = "UPDATE TP_LETTURE SET "
    '                sSQL += " DATALETTURA = '" & FncGen.GiraData(oMyLettura.tDataLetturaAtt) & "',"
    '                sSQL += " LETTURA = " & oMyLettura.nLetturaAtt & ","
    '                If oMyLettura.tDataLetturaPrec <> Date.MinValue And oMyLettura.tDataLetturaPrec.ToShortDateString <> Date.MaxValue.ToShortDateString Then
    '                    sSQL += " DATALETTURAPRECEDENTE = '" & FncGen.GiraData(oMyLettura.tDataLetturaPrec) & "',"
    '                Else
    '                    sSQL += " DATALETTURAPRECEDENTE = '',"
    '                End If
    '                sSQL += " LETTURAPRECEDENTE = " & oMyLettura.nLetturaPrec & ","
    '                If oMyLettura.bIsIncongruente = True Then
    '                    sSQL += " INCONGRUENTE=1,"
    '                Else
    '                    sSQL += " INCONGRUENTE=0,"
    '                End If
    '                If oMyLettura.bIsGiroContatore = True Then
    '                    sSQL += " GIROCONTATORE=1,"
    '                Else
    '                    sSQL += " GIROCONTATORE=0,"
    '                End If
    '                sSQL += " CONSUMO = " & oMyLettura.nConsumo & ","
    '                sSQL += " GIORNIDICONSUMO = " & oMyLettura.nGiorni & ","
    '                sSQL += " CONSUMOTEORICO = " & oMyLettura.nConsumoTeorico & ","
    '                sSQL += " LETTURATEORICA = '" & oMyLettura.sLetturaTeorica & "',"
    '                If oMyLettura.nCodModoLett <> -1 Then
    '                    sSQL += " CODMODALITALETTURA = " & oMyLettura.nCodModoLett & ","
    '                Else
    '                    sSQL += "CODMODALITALETTURA =NULL,"
    '                End If
    '                If oMyLettura.nIdStatoLettura <> -1 Then
    '                    sSQL += " IDSTATOLETTURA= " & oMyLettura.nIdStatoLettura & ","
    '                Else
    '                    sSQL += "IDSTATOLETTURA=NULL,"
    '                End If
    '                If oMyLettura.bFattSospesa = True Then
    '                    sSQL += " FATTURAZIONESOSPESA = 1,"
    '                Else
    '                    sSQL += " FATTURAZIONESOSPESA = 0,"
    '                End If
    '                sSQL += " NOTE = '" & FncGen.ReplaceChar(oMyLettura.sNote) & "',"
    '                If oMyLettura.tDataPassaggio <> Date.MinValue Then
    '                    sSQL += " DATADIPASSAGGIO = '" & FncGen.GiraData(oMyLettura.tDataPassaggio) & "',"
    '                Else
    '                    sSQL += "DATADIPASSAGGIO ='',"
    '                End If
    '                If oMyLettura.nIdAnomalia1 <> -1 Then
    '                    sSQL += " COD_ANOMALIA1 = " & oMyLettura.nIdAnomalia1 & ","
    '                Else
    '                    sSQL += " COD_ANOMALIA1 = NULL,"
    '                End If
    '                If oMyLettura.nIdAnomalia2 <> -1 Then
    '                    sSQL += " COD_ANOMALIA2 = " & oMyLettura.nIdAnomalia2 & ","
    '                Else
    '                    sSQL += " COD_ANOMALIA2=NULL,"
    '                End If
    '                If oMyLettura.nIdAnomalia3 <> -1 Then
    '                    sSQL += " COD_ANOMALIA3 = " & oMyLettura.nIdAnomalia3 & ","
    '                Else
    '                    sSQL += " COD_ANOMALIA3=NULL,"
    '                End If
    '                'sSQL += " DATA_VARIAZIONE='" & CDate(oMyLettura.tDataVariazione).ToString("yyyy-dd-MM 00:00:00").Replace(".", ":") & "',"
    '                sSQL += " DATA_VARIAZIONE='" & FncGen.ReplaceDataForDB(oMyLettura.tDataVariazione) & "',"
    '                sSQL += " AZIONE='" & oMyLettura.sAzione & "'"
    '                sSQL += " WHERE (TP_LETTURE.CODLETTURA=" & oMyLettura.IdLettura & ")"
    '        End Select

    '        Return sSQL
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestLetture.GetSQLLetture.errore: ", Err)
    '        Return ""
    '    End Try
    'End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="IDLettura">Chiave primaria della tabella TP_LETTURE</param>
    ''' <param name="myErr"></param>
    ''' <remarks>
    ''' </remarks>
    Public Sub DelLetture(ByVal IDLettura As Integer, ByRef myErr As String)
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView

        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_TP_LETTURE_D", "CODLETTURA")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("CODLETTURA", IDLettura))
                ctx.Dispose()
            End Using
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    myErr = StringOperation.FormatString(myRow("esito"))
                Next
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.DeLLetture.errore: ", ex)
        Finally
            dvMyDati.Dispose()
        End Try
    End Sub
    'Public Sub DelLetture(ByVal IDLettura As Integer, ByVal IDContatore As Integer, ByVal IDUtente As Integer, ByVal DataLettura As String, ByVal Lettura As String, ByVal datapassaggio As String)

    '    Dim sSQL As String
    '    If Len(datapassaggio) = 0 Then
    '        Dim DettaglioLetture As New ObjLettura
    '        DettaglioLetture = GetDettaglioLetture(IDLettura, IDContatore, -1, Date.MaxValue, -1, False)
    '        clsLetture.AggiornaGiorniDiConsumoDelete(DataLettura, utility.stringoperation.formatint(FncGen.GiraData(DettaglioLetture.tDataLetturaAtt)), IDContatore, IDUtente, IDLettura)
    '        clsLetture.AggiornaConsumoDelete(Lettura, DettaglioLetture.tDataLetturaAtt, IDContatore, IDUtente, IDLettura)
    '    End If

    '    sSQL = "DELETE"
    '    sSQL += " FROM TP_LETTURE"
    '    sSQL += " WHERE TP_LETTURE.CODLETTURA=" & IDLettura

    '    Try
    '        If iDB.ExecuteNonQuery(sSQL) <> 1 Then
    '            Throw New Exception("errore in::" & sSQL)
    '        End If
    '    Catch er As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestLetture.DeLLetture.errore: ", er)
    '        RaiseError.trace(er, sSQL.Replace(vbCrLf, ""), ConfigurationManager.AppSettings("Filename"), ConfigurationManager.AppSettings("FilePath"), "")
    '        Throw
    '    End Try

    '    'dopo la cancellazione del record rieseguo il calcolo del consumo teorico

    '    clsLetture.AggiornaConsumoTeorico(IDContatore, IDUtente)

    'End Sub

    '''' <summary>
    '''' Verifica il cambio di contatore -  controlla la coerenza della date di sostituzione con la data di lettura
    '''' </summary>
    '''' <param name="IDContatore">Chiave primaria della tabella TP_CONTATORI</param>
    '''' <param name="IDUtente">ID anagrafico</param>
    '''' <param name="IDPeriodo">Chiave primaria della tabella TP_PERIODI</param>
    '''' <param name="IDPadre">ID contatore precedente</param>
    '''' <returns></returns>
    '''' <remarks>
    '''' </remarks>
    'Public Function VerificaVecchioContatore(ByVal IDContatore As Integer, ByVal IDUtente As Integer, ByVal IDPeriodo As Integer, ByRef IDPadre As Integer) As Boolean
    '    Try
    '        Dim sSQL As String
    '        Dim lngDataSostituzione, lngDataDa, lngDataA, lngDataUltimaLettura, lngIDPadre As Long

    '        VerificaVecchioContatore = False
    '        Dim sqlRdr As SqlDataReader

    '        sSQL = ""
    '        sSQL = "SELECT *" & vbCrLf
    '        sSQL += "FROM TP_CONTATORI " & vbCrLf
    '        sSQL += "WHERE TP_CONTATORI.CODCONTATORE = " & IDContatore

    '        sqlRdr = iDB.GetDataReader(sSQL)
    '        If sqlRdr.Read Then
    '            lngIDPadre = utility.stringoperation.formatint(sqlRdr("CODCONTATOREPRECEDENTE"))
    '        End If
    '        sqlRdr.Close()


    '        If lngIDPadre <> 0 Then

    '            sSQL = ""
    '            sSQL = "SELECT *" & vbCrLf
    '            sSQL += "FROM TP_CONTATORI " & vbCrLf
    '            sSQL += "WHERE TP_CONTATORI.CODCONTATORE = " & lngIDPadre & ";"
    '            sSQL += "SELECT * FROM TP_PERIODO WHERE CODPERIODO = " & IDPeriodo & ";"

    '            sSQL += "SELECT TOP 1 TP_LETTURE.*  FROM TP_LETTURE " & vbCrLf
    '            sSQL += "WHERE" & vbCrLf
    '            sSQL += "FATTURAZIONE=1" & vbCrLf
    '            'sSQL+="AND CODUTENTE=" & IDUtente & vbCrLf
    '            sSQL += "AND" & vbCrLf
    '            sSQL += "CODCONTATORE=" & IDContatore & vbCrLf

    '            sqlRdr = iDB.GetDataReader(sSQL)
    '            If sqlRdr.Read Then
    '                lngDataSostituzione = utility.stringoperation.formatint(sqlRdr("DATASOSTITUZIONE"))
    '            End If
    '            sqlRdr.NextResult()
    '            If sqlRdr.Read Then
    '                lngDataDa = utility.stringoperation.formatint(sqlRdr("DADATA"))
    '                lngDataA = utility.stringoperation.formatint(sqlRdr("ADATA"))
    '            End If
    '            'Determino la data dell' ultima lettura fattura per il Contatore selezionato
    '            sqlRdr.NextResult()
    '            If sqlRdr.Read Then
    '                lngDataUltimaLettura = utility.stringoperation.formatint(sqlRdr("DATALETTURA"))
    '            End If
    '            sqlRdr.Close()
    '            If (lngDataSostituzione > lngDataDa And lngDataSostituzione < lngDataA) Or (lngDataSostituzione > lngDataUltimaLettura) Then
    '                VerificaVecchioContatore = True
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestLetture.VerificaVecchioContatore.errore: ", ex)
    '    End Try
    'End Function

    ''' <summary>
    ''' Calcola i giorni di consumo, la lettura e il consumo teorico
    ''' </summary>
    ''' <param name="strCodContatore">Chiave primaria della tabella TP_CONTATORI</param>
    ''' <param name="strDataLettura"></param>
    ''' <param name="lngConsumoTeorico"></param>
    ''' <param name="lngGiorniDiConsumo"></param>
    ''' <param name="lngLetturaTeorica"></param>
    ''' <param name="strCodUtente"></param>
    ''' <param name="strCodContratto"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Sub ControllaDataLettura(ByVal strCodContatore As String, ByVal strDataLettura As String, ByRef lngConsumoTeorico As Long, ByRef lngGiorniDiConsumo As Long, ByRef lngLetturaTeorica As Long, ByVal strCodUtente As String, ByVal strCodContratto As String)
        Dim strDataLetturaPrecedente As String = ""
        Dim lngRecordCount, lngLetturaPrecedente As Long
        Dim dblConsumoTeorico, dblResult, dblMediaConsumo, dblRapportoCGG As Double
        Dim blnPrimaLettura As Boolean
        Dim dvMyDati As New DataView

        Try
            Dim myContatore As New objContatore
            myContatore = New GestContatori().GetDetailsContatori(strCodContatore, -1)

            'Ricavo se Esiste L'ultima Lettura Eseguita e Salvata in TP_Letture
            Try
                dvMyDati = New clsLetture().GetTopLetture(ConstSession.DBType, ConstSession.StringConnection, 1, strCodContatore, strDataLettura, "<")
                For Each myRow As DataRowView In dvMyDati
                    'Dati da Lettura Attuale - Lettura precedente
                    lngLetturaPrecedente = StringOperation.FormatInt(myRow("LETTURA"))
                    strDataLetturaPrecedente = FncGen.GiraDataFromDB(StringOperation.FormatString(myRow("DATALETTURA")))

                    'Determino i Giorni di Consumo dati dalla differenza delle due date
                    lngGiorniDiConsumo = getGiorniDiConsumo(strDataLetturaPrecedente, strDataLettura)

                    'Verifico se la lettura fatturata e la prima lettura del contatore (Es:---Nuovo Contatore---)
                    blnPrimaLettura = StringOperation.FormatBool(myRow("PRIMALETTURA"))
                Next
            Catch EX As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.ControlloDataLettura.GetTop1.errore: ", EX)
            Finally
                dvMyDati.Dispose()
            End Try
            'VERIFICA DELLA DATA
            If strDataLettura < myContatore.sDataAttivazione Then
                Exit Sub
            End If
            If strDataLettura <= strDataLetturaPrecedente Then
                Exit Sub
            End If
            Try
                If blnPrimaLettura Then
                    ConsumoLetturaTeorici_PrimaLettura(strCodContatore, dblConsumoTeorico, lngConsumoTeorico, lngLetturaTeorica, lngLetturaPrecedente, lngGiorniDiConsumo, strCodContratto)
                Else
                    dvMyDati = New clsLetture().GetTopLetture(ConstSession.DBType, ConstSession.StringConnection, 5, strCodContatore, strDataLettura, "<")
                    For Each myRow As DataRowView In dvMyDati
                        If StringOperation.FormatInt(myRow("GIORNIDICONSUMO")) = 0 Then
                            'Giorni di Consumo =0 situazione anomala
                            dblRapportoCGG = 0
                        Else
                            dblRapportoCGG = StringOperation.FormatInt(myRow("CONSUMO")) / StringOperation.FormatInt(myRow("GIORNIDICONSUMO"))
                        End If
                        dblResult += dblRapportoCGG
                        lngRecordCount += 1
                    Next
                    Try
                        dblMediaConsumo = dblResult / lngRecordCount
                    Catch ex As Exception When lngRecordCount = 0
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.ControlloDataLettura.GetTop5.Media.errore: ", ex)
                    Finally
                        dblConsumoTeorico = dblMediaConsumo * lngGiorniDiConsumo     ' -->GIORNI DI CONSUMO
                    End Try
                    'Approssimo per eccesso dblConsumoTeorico
                    lngConsumoTeorico = ApprossimaNumero(dblConsumoTeorico)
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.ControlloDataLettura.GetTop5.errore: ", ex)
            Finally
                dvMyDati.Dispose()
            End Try
            Try
                lngLetturaTeorica = CalcolaLetturaTeorica(lngLetturaPrecedente, lngConsumoTeorico, New GestLetture().GetFondoScala(-1, strCodContatore))
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.ControlloDataLettura.GetFondoScala.errore: ", ex)
            End Try
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.ControlloDataLettura.errore: ", ex)
        End Try
    End Sub
    'Public Sub ControllaDataLettura(ByVal strCodContatore As String, ByVal strDataLettura As String, ByVal DataLettura As String, ByRef lngConsumoTeorico As Long, ByRef lngGiorniDiConsumo As Long, ByRef lngLetturaTeorica As Long, ByVal strCodUtente As String, ByVal strCodContratto As String)
    '    Dim sSQL, strDataLetturaPrecedente As String
    '    Dim lngRecordCount, lngLetturaPrecedente As Long
    '    Dim dblConsumoTeorico, dblResult, dblMediaConsumo, dblRapportoCGG As Double
    '    Dim blnPrimaLettura As Boolean

    '    Try
    '        'Ricavo se Esiste L'ultima Lettura Eseguita e Salvata in TP_Letture
    '        sSQL = getTopOneLetture(strCodContatore, strCodUtente, strDataLettura, "DESC", ";") & vbCrLf

    '        sSQL += getTopFiveLetture(strCodContatore,
    '           strCodUtente, strDataLettura, "DESC", ";") & vbCrLf

    '        sSQL += clsLetture.getValoreFondoScala(strCodContatore)
    '        Dim sqlDataReader As SqlDataReader
    '        Try
    '            sqlDataReader = iDB.GetDataReader(sSQL)
    '        Catch EX As Exception
    '            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestLetture.ControlloDataLettura.errore: ", EX)
    '        End Try
    '        If sqlDataReader.Read() Then
    '            'Dati da Lettura Attuale - Lettura precedente
    '            lngLetturaPrecedente = utility.stringoperation.formatint(sqlDataReader.Item("LETTURA"))
    '            strDataLetturaPrecedente = FncGen.GiraDataFromDB(Utility.StringOperation.FormatString(sqlDataReader.Item("DATALETTURA")))

    '            'Determino i Giorni di Consumo dati dalla differenza delle due date
    '            lngGiorniDiConsumo = getGiorniDiConsumo(strDataLetturaPrecedente, DataLettura)

    '            'Verifico se la lettura fatturata e la prima lettura del contatore (Es:---Nuovo Contatore---)
    '            blnPrimaLettura = Utility.StringOperation.FormatBool(sqlDataReader.Item("PRIMALETTURA"))
    '            If blnPrimaLettura Then
    '                'Log.Debug("ControllaDataLettura::controlla prima lettura")
    '                sqlDataReader.Close()

    '                ConsumoLetturaTeorici_PrimaLettura(strCodContatore,
    '                 dblConsumoTeorico,
    '                 lngConsumoTeorico, lngLetturaTeorica,
    '                 lngLetturaPrecedente,
    '                 lngGiorniDiConsumo,
    '                 strCodContratto)
    '            Else

    '                sqlDataReader.NextResult()

    '                While sqlDataReader.Read

    '                    dblRapportoCGG = utility.stringoperation.formatint(sqlDataReader.Item("CONSUMO")) / utility.stringoperation.formatint(sqlDataReader.Item("GIORNIDICONSUMO"))

    '                    If utility.stringoperation.formatint(sqlDataReader.Item("GIORNIDICONSUMO")) = 0 Then
    '                        'Giorni di Consumo =0 situazione anomala
    '                        dblRapportoCGG = 0
    '                    End If

    '                    dblResult = dblResult + dblRapportoCGG

    '                    lngRecordCount = lngRecordCount + 1

    '                End While
    '                Try
    '                    dblMediaConsumo = dblResult / lngRecordCount
    '                Catch ex As Exception When lngRecordCount = 0

    '                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestLetture.ControlloDataLettura.errore: ", ex)
    '                Finally
    '                    dblConsumoTeorico = dblMediaConsumo * lngGiorniDiConsumo     ' -->GIORNI DI CONSUMO
    '                End Try
    '                'Approssimo per eccesso dblConsumoTeorico
    '                lngConsumoTeorico = ApprossimaNumero(dblConsumoTeorico)

    '                sqlDataReader.NextResult()

    '                If sqlDataReader.Read() Then
    '                    lngLetturaTeorica = CalcolaLetturaTeorica(lngLetturaPrecedente,
    '                    lngConsumoTeorico,
    '                    utility.stringoperation.formatint(sqlDataReader.Item("VALOREFONDOSCALA")))
    '                End If
    '            End If
    '        End If
    '        If Not sqlDataReader.IsClosed Then
    '            sqlDataReader.Close()
    '        End If
    '    Catch ex As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestLetture.ControlloDataLettura.errore: ", ex)
    '    End Try
    'End Sub

    ''' <summary>
    ''' Determina la lettura teorica
    ''' </summary>
    ''' <param name="lngLetturaPrecedente"></param>
    ''' <param name="lngConsumoTeorico"></param>
    ''' <param name="lngValoreFondoScala"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    Protected Function CalcolaLetturaTeorica(ByVal lngLetturaPrecedente As Long, ByVal lngConsumoTeorico As Long, ByVal lngValoreFondoScala As Long) As Long

        CalcolaLetturaTeorica = lngLetturaPrecedente + lngConsumoTeorico

        If CalcolaLetturaTeorica > lngValoreFondoScala Then

            CalcolaLetturaTeorica = CalcolaLetturaTeorica - lngValoreFondoScala

        End If

    End Function

    '''' <summary>
    '''' Restituisce la lettura più recente
    '''' </summary>
    '''' <param name="CodContatore"></param>
    '''' <param name="DataLettura"></param>
    '''' <returns></returns>
    '''' <revisionHistory>
    '''' <revision date="12/04/2019">
    '''' Modifiche da revisione manuale
    '''' </revision>
    '''' </revisionHistory>
    'Public Function getTopOneLetture(ByVal CodContatore As String, ByVal DataLettura As String, TipoLettura As String) As DataView
    '    Dim dvMyDati As New DataView
    '    Dim sSQL As String = ""
    '    Try
    '        Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
    '            sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"TopOneLetture", "CODCONTATORE", "DATALETTURA", "TIPO")
    '            dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("CODCONTATORE", CodContatore) _
    '                    , ctx.GetParam("DATALETTURA", DataLettura) _
    '                    , ctx.GetParam("TIPO", TipoLettura)
    '                )
    '            ctx.Dispose()
    '        End Using
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.MyUtility.GetMaxID.errore: ", ex)
    '    End Try
    '    Return dvMyDati
    'End Function
    'Public Function getTopOneLetture(ByVal CodContatore As String, ByVal DataLettura As String, TipoLettura As String) As DataTable
    '    Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim dtMyDati As New DataTable()

    '    Try
    '        cmdMyCommand = New SqlClient.SqlCommand
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.AddWithValue("@CODCONTATORE", CodContatore)
    '        cmdMyCommand.Parameters.AddWithValue("@DATALETTURA", DataLettura)
    '        cmdMyCommand.Parameters.AddWithValue("@TIPO", TipoLettura)
    '        cmdMyCommand.CommandText = "TopOneLetture"
    '        myAdapter.SelectCommand = cmdMyCommand
    '        myAdapter.Fill(dtMyDati)
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.MyUtility.GetMaxID.errore: ", ex)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '    Finally
    '        myAdapter.Dispose()
    '        cmdMyCommand.Dispose()
    '        cmdMyCommand.Connection.Close()
    '    End Try
    '    Return dtMyDati
    'End Function
    '''' <summary>
    '''' Restituisce la stringa sql per determinare la lettura più recente
    '''' </summary>
    '''' <param name="CodContatore">Chiave primaria della tabella TP_CONTATORI</param>
    '''' <param name="CodUtente">ID anagrafico</param>
    '''' <param name="DataLettura"></param>
    '''' <param name="strTypeOrder"></param>
    '''' <param name="strQueryDelimiter"></param>
    '''' <returns></returns>
    'Protected Function getTopOneLetture(ByVal CodContatore As String, ByVal CodUtente As String, ByVal DataLettura As String, Optional ByVal strTypeOrder As String = "", Optional ByVal strQueryDelimiter As String = "") As DataView
    '    Try
    '        DataLettura = FncGen.GiraData(DataLettura)

    '        Return New clsLetture().GetTopLetture(ConstSession.DBType, ConstSession.StringConnection, 1, CodContatore, DataLettura, "<")
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestLetture.getTopOneLetture.errore: ", ex)
    '    End Try
    'End Function
    'Protected Function getTopOneLetture(ByVal CodContatore As String, ByVal CodUtente As String, ByVal DataLettura As String, Optional ByVal strTypeOrder As String = "", Optional ByVal strQueryDelimiter As String = "") As String
    '    Dim sSQL As String
    '    Try
    '        DataLettura = FncGen.GiraData(DataLettura)

    '        sSQL = "SELECT TOP 1 TP_LETTURE.*  " & vbCrLf
    '        sSQL += "FROM TP_LETTURE " & vbCrLf
    '        sSQL += "WHERE (1=1)" & vbCrLf
    '        'sSQL+="CODUTENTE=" & CodUtente & vbCrLf
    '        sSQL += "AND CODCONTATORE=" & CodContatore & vbCrLf
    '        sSQL += "AND (STORICIZZATA=0 OR STORICIZZATA IS NULL)" & vbCrLf
    '        sSQL += "AND DATALETTURA <" & Utility.StringOperation.FormatString(DataLettura) & vbCrLf
    '        sSQL += "ORDER BY DATALETTURA "
    '        If Not strTypeOrder = String.Empty Then
    '            sSQL += strTypeOrder
    '        End If
    '        If Not strQueryDelimiter = String.Empty Then
    '            sSQL += strQueryDelimiter
    '        End If

    '        Return sSQL
    '    Catch ex As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestLetture.getTopOneLetture.errore: ", ex)
    '    End Try
    'End Function

    Protected Function ApprossimaNumero(ByVal dblNumber As Double) As Long
        ApprossimaNumero = System.Math.Ceiling(dblNumber)
    End Function

    ''' <summary>
    ''' Determina i giorni di consumo
    ''' </summary>
    ''' <param name="strDataPrecedente"></param>
    ''' <param name="strDataAttuale"></param>
    ''' <returns></returns>
    ''' <remarks>
    ''' </remarks>
    Protected Function getGiorniDiConsumo(ByVal strDataPrecedente As String, ByVal strDataAttuale As String) As Long

        Dim lngGiorniDiConsumo As Long

        lngGiorniDiConsumo = 0
        lngGiorniDiConsumo = DateDiff(DateInterval.Day, CDate(strDataPrecedente), CDate(strDataAttuale))
        Return lngGiorniDiConsumo

    End Function

    '''' <summary>
    '''' Restituisce la stringa SQL che permette l'estrazione delle ultime 5 letture eseguite 
    '''' su un determinato contatore
    '''' </summary>
    '''' <param name="CodContatore">Chiave primaria della tabella TP_CONTATORI</param>
    '''' <param name="CodUtente">ID anagrafico</param>
    '''' <param name="DataLettura"></param>
    '''' <param name="strTypeOrder"></param>
    '''' <param name="strQueryDelimiter"></param>
    '''' <returns></returns>
    '''' <remarks>
    '''' </remarks>
    'Protected Function getTopFiveLetture(ByVal CodContatore As String, ByVal CodUtente As String, ByVal DataLettura As String, Optional ByVal strTypeOrder As String = "", Optional ByVal strQueryDelimiter As String = "") As String
    '    Dim sSQL As String
    '    Try
    '        DataLettura = FncGen.GiraData(DataLettura)

    '        sSQL = "SELECT TOP 5 TP_LETTURE.*  FROM TP_LETTURE " & vbCrLf
    '        sSQL += "WHERE" & vbCrLf
    '        sSQL += "CODCONTATORE=" & CodContatore & vbCrLf
    '        'sSQL+="AND CODUTENTE=" & CodUtente & vbCrLf
    '        sSQL += "AND PRIMALETTURA IS NULL" & vbCrLf
    '        sSQL += "AND (STORICIZZATA=0 OR STORICIZZATA IS NULL)" & vbCrLf
    '        sSQL += "AND (INCONGRUENTEFORZATO IS NULL  OR INCONGRUENTEFORZATO =0)" & vbCrLf
    '        sSQL += "AND (DATADIPASSAGGIO IS NULL OR DATADIPASSAGGIO='')" & vbCrLf
    '        sSQL += "AND DATALETTURA <" & Utility.StringOperation.FormatString(DataLettura) & vbCrLf
    '        sSQL += "ORDER BY DATALETTURA "

    '        If Not strTypeOrder = String.Empty Then
    '            sSQL += strTypeOrder
    '        End If

    '        If Not strQueryDelimiter = String.Empty Then
    '            sSQL += strQueryDelimiter
    '        End If

    '        Return sSQL
    '    Catch ex As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestLetture.getTopFiveLetture.errore: ", ex)
    '    End Try
    'End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strCodContatore"></param>
    ''' <param name="dblConsumoTeorico"></param>
    ''' <param name="lngConsumoTeorico"></param>
    ''' <param name="lngLetturaTeorica"></param>
    ''' <param name="lngLetturaPrecedente"></param>
    ''' <param name="lngGiorniDiConsumo"></param>
    ''' <param name="strCodContratto"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Private Sub ConsumoLetturaTeorici_PrimaLettura(ByVal strCodContatore As String, ByRef dblConsumoTeorico As Double, ByRef lngConsumoTeorico As Long, ByRef lngLetturaTeorica As Long, ByRef lngLetturaPrecedente As Long, ByRef lngGiorniDiConsumo As Long, ByVal strCodContratto As String)
        Dim blnConsumoMinimoContrattuale As Boolean

        blnConsumoMinimoContrattuale = False

        Try
            Dim myContratto As New objContratto
            myContratto = New GestContratti().GetDetailsContratti(strCodContratto, ConstSession.IdEnte)
            dblConsumoTeorico = myContratto.oContatore.nConsumoMinimo * lngGiorniDiConsumo  '-->GIORNI DI CONSUMO
            dblConsumoTeorico = FormatNumber(dblConsumoTeorico, 2)
            blnConsumoMinimoContrattuale = True
            'Verifico se trovo nella tabella TIPIUTENZA il campo minimo fatturabile su base annua
            If Not blnConsumoMinimoContrattuale Then
                Dim dvMyDati As New DataView
                dvMyDati = New TabelleDiDecodifica.DBTipiUtenza().GetListaTipiUtenza(ConstSession.IdEnte, "", strCodContatore)
                For Each myRow As DataRowView In dvMyDati
                    myContratto.oContatore.nConsumoMinimo = StringOperation.FormatInt(myRow("CONSUMOMINIMOANNUO")) / 365    ' Consumo giornaliero
                    dblConsumoTeorico = myContratto.oContatore.nConsumoMinimo * lngGiorniDiConsumo  ' -->GIORNI DI CONSUMO
                Next
            End If
            lngConsumoTeorico = ApprossimaNumero(dblConsumoTeorico)
            lngLetturaTeorica = CalcolaLetturaTeorica(lngLetturaPrecedente, lngConsumoTeorico, GetFondoScala(-1, strCodContatore))
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.ConsumoLetturaTeorici_PrimaLettura.errore: ", ex)
        End Try
    End Sub
    'Private Function ConsumoLetturaTeorici_PrimaLettura(ByVal strCodContatore As String,
    ' ByRef dblConsumoTeorico As Double,
    ' ByRef lngConsumoTeorico As Long,
    ' ByRef lngLetturaTeorica As Long,
    ' ByRef lngLetturaPrecedente As Long,
    ' ByRef lngGiorniDiConsumo As Long,
    ' ByVal strCodContratto As String)
    '    Dim sSQL As String
    '    Dim dblResult As Double
    '    Dim blnConsumoMinimoContrattuale As Boolean

    '    blnConsumoMinimoContrattuale = False

    '    Try
    '        sSQL = "SELECT  TP_CONTRATTI.CONSUMOMINIMO  FROM TP_CONTRATTI " & vbCrLf
    '        sSQL += "WHERE" & vbCrLf
    '        sSQL += "CODCONTRATTO=" & strCodContratto & vbCrLf
    '        sSQL += "AND" & vbCrLf
    '        sSQL += "CONSUMOMINIMO IS NOT NULL " & vbCrLf
    '        'Log.Debug("ConsumoLetturaTeorici_PrimaLettura::query1::" & sSQL)
    '        Dim sqlDataReader As SqlDataReader = iDB.GetDataReader(sSQL)
    '        If sqlDataReader.Read Then
    '            dblResult = (Utility.StringOperation.FormatString(sqlDataReader.Item("CONSUMOMINIMO")) / 365) ' Consumo giornaliero
    '            dblConsumoTeorico = dblResult * lngGiorniDiConsumo  '-->GIORNI DI CONSUMO
    '            dblConsumoTeorico = FormatNumber(dblConsumoTeorico, 2)
    '            blnConsumoMinimoContrattuale = True
    '        End If
    '        If Not sqlDataReader.IsClosed Then
    '            sqlDataReader.Close()
    '        End If

    '        'Verifico se trovo nella tabella TIPIUTENZA il campo minimo fatturabile su base annua

    '        If Not blnConsumoMinimoContrattuale Then
    '            sSQL = "SELECT  TP_TIPIUTENZA.CONSUMOMINIMOANNUO" & vbCrLf
    '            sSQL += "FROM TP_CONTATORI INNER JOIN" & vbCrLf
    '            sSQL += "TP_TIPIUTENZA ON TP_CONTATORI.IDTIPOUTENZA = TP_TIPIUTENZA.IDTIPOUTENZA" & vbCrLf
    '            sSQL += "WHERE" & vbCrLf
    '            sSQL += "CODCONTATORE=" & strCodContatore & vbCrLf
    '            'Log.Debug("ConsumoLetturaTeorici_PrimaLettura::query2::" & sSQL)
    '            sqlDataReader = iDB.GetDataReader(sSQL)
    '            If sqlDataReader.Read Then
    '                dblResult = utility.stringoperation.formatint(sqlDataReader.Item("CONSUMOMINIMOANNUO")) / 365    ' Consumo giornaliero
    '                dblConsumoTeorico = dblResult * lngGiorniDiConsumo  ' -->GIORNI DI CONSUMO
    '            End If
    '        End If
    '        If Not sqlDataReader.IsClosed Then
    '            sqlDataReader.Close()
    '        End If
    '        lngConsumoTeorico = ApprossimaNumero(dblConsumoTeorico)
    '        'Log.Debug("ConsumoLetturaTeorici_PrimaLettura::getfondoscala")
    '        sqlDataReader = iDB.GetDataReader(clsLetture.getValoreFondoScala(strCodContatore))
    '        If sqlDataReader.Read() Then
    '            'Log.Debug("ConsumoLetturaTeorici_PrimaLettura::getletturateorica")
    '            lngLetturaTeorica = CalcolaLetturaTeorica(lngLetturaPrecedente,
    '           lngConsumoTeorico,
    '           utility.stringoperation.formatint(sqlDataReader.Item("VALOREFONDOSCALA")))
    '        End If
    '        If Not sqlDataReader.IsClosed Then
    '            sqlDataReader.Close()
    '        End If
    '    Catch ex As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestLetture.ConsumoLetturaTeorici_PrimaLettura.errore: ", ex)
    '    End Try
    'End Function

    ''' <summary>
    ''' Dato un periodo (ID) restituisce se è un periodo che fa parte dello storico
    ''' oppure è quello impostato come attuale 
    ''' </summary>
    ''' <param name="CodPeriodo">Chiave primaria della tabella TP_PERIODI</param>
    ''' <returns>
    ''' True se è impostato come periodo STORICO
    ''' False se è impostato come periodo ATTUALE
    ''' </returns>
    ''' <remarks>
    ''' </remarks>
    Public Function VerificaPeriodo(ByVal CodPeriodo As Integer) As Boolean
        Dim sSQL As String
        Dim dvMyDati As New DataView

        Try
            VerificaPeriodo = False

            sSQL = "SELECT STORICO FROM TP_PERIODO WHERE CodPeriodo = " & CodPeriodo
            dvMyDati = iDB.GetDataView(sSQL)
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    VerificaPeriodo = Utility.StringOperation.FormatBool(myRow("STORICO"))
                Next
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.VerificaPeriodo.errore: ", ex)
        Finally
            dvMyDati.Dispose()
        End Try
    End Function

    ''' <summary>
    ''' Dato un contatore e la data lettura, restituisce la lettura precedente
    ''' </summary>
    ''' <param name="DataLetturaAttuale"></param>
    ''' <param name="IDContatore">Chiave primaria della tabella TP_CONTATORI</param>
    ''' <param name="nLetturaPrec"></param>
    ''' <param name="sDataLetturaPrec"></param>
    ''' <returns>Long</returns>
    ''' <remarks>
    ''' </remarks>
    Public Function getDatiLetturaPrecedente(ByVal DataLetturaAttuale As String, ByVal IDContatore As Long, ByRef nLetturaPrec As Integer, ByRef sDataLetturaPrec As String) As Long
        Try
            Dim dvMyDati As New DataView
            nLetturaPrec = 0
            sDataLetturaPrec = Date.MaxValue.ToString
            dvMyDati = New clsLetture().GetTopLetture(ConstSession.DBType, ConstSession.StringConnection, 1, IDContatore, DataLetturaAttuale, "<")
            For Each myRow As DataRowView In dvMyDati
                nLetturaPrec = StringOperation.FormatInt(myRow("lettura"))
                sDataLetturaPrec = FncGen.GiraDataFromDB(StringOperation.FormatString(("DATALETTURA")))
            Next
            dvMyDati.Dispose()
            Return 1
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.getDatiLetturaPrecedente.errore: ", ex)
            Return -1
        End Try
    End Function

    '''' <summary>
    '''' Estrae la data della lattura precedente e la lettura partento da una data di lettura.
    '''' La funzione viene richiata dal metodo getLetturaPrecedente.
    '''' </summary>
    '''' <param name="IDContatore">Chiave primaria della tabella TP_CONTATORI</param>
    '''' <param name="IDUtente">ID anagrafico</param>
    '''' <param name="DatadiLettura"></param>
    '''' <returns>Stringa SQL per estrarre i dati</returns>
    '''' <remarks>
    '''' </remarks>
    'Public Function EstraiDatiLetturaPrecedente(ByVal IDContatore As Long, ByVal DatadiLettura As String) As String
    '    Dim sSQL As String

    '    sSQL = "SELECT TOP 1 LETTURA,DATALETTURA"
    '    sSQL += " FROM TP_LETTURE"
    '    sSQL += " WHERE 1=1"
    '    sSQL += " AND CODCONTATORE=" & IDContatore
    '    sSQL += " AND (STORICIZZATA=0 OR STORICIZZATA IS NULL)"
    '    sSQL += " AND DATALETTURA<" & FncGen.GiraData(DatadiLettura)
    '    sSQL += " ORDER BY DATALETTURA DESC"
    '    Return sSQL

    'End Function

    '''' <summary>
    '''' In base alle ultime 5 letture vengono determinati il consumo teorico e la data di lettura teorica
    '''' e aggiorna la tabella nel database.
    '''' </summary>
    '''' <param name="IDContatore">Chiave primaria della tabella TP_CONTATORI</param>
    '''' <param name="IDUtente">ID anagrafico</param>
    '''' <param name="IdLettura"></param>
    '''' <param name="DataLettura"></param>
    '''' <param name="DetailContatore"></param>
    '''' <remarks>
    '''' </remarks>
    'Protected Sub setConsumoTeorico(ByVal IDContatore As Integer, ByVal IDUtente As Integer, IdLettura As Integer, ByVal DataLettura As String, ByVal DetailContatore As objContatore)
    '    Try
    '        Dim strValoreFondoScala As String = ""
    '        Dim lngLetturaPrecedente, lngLetturaTeorica, lngCodLetturaSuccessiva, lngCifreContatore, lngIndex, lngValoreFondoScala As Integer
    '        Dim dblRapportoCGG, dblMediaConsumo As Double

    '        Dim arrIDLetture() As String
    '        Dim arrConsumoTeorico() As String
    '        Dim arrLetturaTeorica() As String

    '        lngLetturaPrecedente = 0

    '        'Verifico se esiste una data successiva

    '        Dim dblConsumoTeorico, dblResult As Double
    '        Dim lngRecordCount, lngConsumoTeorico As Integer

    '        Dim dvMyDati As New DataView
    '        Dim lngLastLettura As Long
    '        dvMyDati = New clsLetture().GetTopLetture(ConstSession.DBType, ConstSession.StringConnection, 5, IDContatore, DataLettura, "<")
    '        For Each myRow As DataRowView In dvMyDati
    '            lngLastLettura = StringOperation.FormatInt(myRow("lettura"))
    '            If StringOperation.FormatInt(myRow("GIORNIDICONSUMO")) = 0 Then
    '                'Giorni di Consumo =0 situazione anomala
    '                dblRapportoCGG = 0
    '            Else
    '                dblRapportoCGG = StringOperation.FormatInt(myRow("CONSUMO")) / StringOperation.FormatInt(myRow("GIORNIDICONSUMO"))
    '            End If

    '            dblResult = dblResult + dblRapportoCGG
    '            lngRecordCount = lngRecordCount + 1
    '        Next
    '        dvMyDati.Dispose()

    '        If lngRecordCount > 0 Then

    '            Dim lngCount, lngCountRecord As Long
    '            dblMediaConsumo = dblResult / lngRecordCount

    '            ReDim arrIDLetture(4)    '--> 0-4 cinque elementi
    '            ReDim arrConsumoTeorico(4)    '--> 0-4 cinque elementi
    '            ReDim arrLetturaTeorica(4)   '--> 0-4 cinque elementi

    '            If Len(Trim(DetailContatore.sCifreContatore)) > 0 Then
    '                lngCifreContatore = StringOperation.FormatInt(DetailContatore.sCifreContatore)
    '                For lngIndex = 1 To lngCifreContatore
    '                    strValoreFondoScala = strValoreFondoScala & "9"
    '                Next
    '                lngValoreFondoScala = StringOperation.FormatInt(strValoreFondoScala)
    '            End If

    '            dvMyDati = New clsLetture().GetTopLetture(ConstSession.DBType, ConstSession.StringConnection, 5, IDContatore, DataLettura, "<")
    '            For Each myRow As DataRowView In dvMyDati
    '                dblConsumoTeorico = dblMediaConsumo * StringOperation.FormatString(myRow("GIORNIDICONSUMO"))
    '                lngConsumoTeorico = ApprossimaNumero(dblConsumoTeorico)
    '                lngCodLetturaSuccessiva = StringOperation.FormatInt(myRow("CODLETTURA"))
    '                lngLetturaPrecedente = StringOperation.FormatInt(myRow("LETTURAPRECEDENTE"))
    '                lngLetturaTeorica = lngLetturaPrecedente + lngConsumoTeorico

    '                If lngLetturaTeorica > lngValoreFondoScala Then
    '                    lngLetturaTeorica = lngLetturaTeorica - lngValoreFondoScala
    '                End If

    '                arrLetturaTeorica(lngCount) = lngLetturaTeorica
    '                arrIDLetture(lngCount) = Utility.StringOperation.FormatString(lngCodLetturaSuccessiva)
    '                arrConsumoTeorico(lngCount) = Utility.StringOperation.FormatString(lngConsumoTeorico)

    '                lngCount = lngCount + 1
    '            Next
    '            dvMyDati.Dispose()
    '            lngCountRecord = lngCount - 1

    '            For lngCount = 0 To lngCountRecord
    '                Dim myLettura As New ObjLettura
    '                myLettura.IdLettura = arrIDLetture(lngCount)
    '                myLettura.nIdContatore = IDContatore
    '                myLettura.nConsumoTeorico = arrConsumoTeorico(lngCount)
    '                myLettura.sLetturaTeorica = arrLetturaTeorica(lngCount)
    '                myLettura.sAzione = "TEORICO"
    '                SetLettura(myLettura)
    '            Next
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.setConsumoTeorico.errore: ", ex)
    '    End Try
    'End Sub

    '''' <summary>
    '''' Determina i giorni di consumo della lettura che si sta inserendo e aggiorna la tabella nel database.
    '''' </summary>
    '''' <param name="DataLetturaAttuale"></param>
    '''' <param name="DataLetturaSalvata">Data lettura precedente</param>
    '''' <param name="IDContatore">Chiave primaria della tabella TP_CONTATORI</param>
    '''' <param name="IDUtente">ID anagrafico</param>
    '''' <param name="IDLettura"></param>
    '''' <remarks>
    '''' </remarks>
    'Protected Sub GiorniDiConsumo(ByVal DataLetturaAttuale As String, ByVal DataLetturaSalvata As String, ByVal IDContatore As Long, ByVal IDUtente As Long, ByVal IDLettura As Long)

    '    Try
    '        Dim sSQL, strCondition, strTemp As String

    '        Dim sqlDataReader As SqlDataReader
    '        Dim lngDataLetturaSuccessiva, lngDataLetturaPrecedente, lngCodLetturaSuccessiva, lngGiorniDiConsumoSuccessivi, lngGiorniDiConsumoPrecedenti As Integer
    '        lngDataLetturaSuccessiva = 0
    '        lngDataLetturaPrecedente = 0

    '        '*******************************************************************
    '        'Verifico se esiste una data successiva 
    '        'QUINDI UNA LETTURA SUCCESSIVA A QUELLA CHE SI STA MODIFICANDO
    '        '*******************************************************************

    '        sSQL = ""
    '        sSQL = "SELECT TOP 1 CODLETTURA,DATALETTURA" & vbCrLf
    '        sSQL += "FROM TP_LETTURE" & vbCrLf
    '        sSQL += "WHERE" & vbCrLf
    '        sSQL += "CODCONTATORE=" & IDContatore & vbCrLf
    '        sSQL += "AND (STORICIZZATA=0 OR STORICIZZATA IS NULL)" & vbCrLf
    '        'sSQL+="AND CODUTENTE=" & IDUtente & vbCrLf
    '        sSQL += "AND" & vbCrLf

    '        strCondition = "DATALETTURA > " & DataLetturaSalvata & vbCrLf
    '        strCondition = strCondition & "ORDER BY DATALETTURA"

    '        strTemp = sSQL & strCondition
    '        sqlCmdInsert = New SqlCommand(strTemp, sqlConn, sqlTrans)


    '        sqlDataReader = sqlCmdInsert.ExecuteReader

    '        If sqlDataReader.Read Then
    '            lngDataLetturaSuccessiva = utility.stringoperation.formatint(sqlDataReader.Item("DATALETTURA"))
    '            lngCodLetturaSuccessiva = utility.stringoperation.formatint(sqlDataReader.Item("CODLETTURA"))
    '            lngGiorniDiConsumoSuccessivi = DateDiff(DateInterval.Day, CDate(DataLetturaAttuale), CDate(FncGen.GiraDataFromDB(lngDataLetturaSuccessiva)))
    '        End If

    '        If Not sqlDataReader.IsClosed Then
    '            sqlDataReader.Close()
    '        End If
    '        '*******************************************************************
    '        'Verifico se esiste una data PRECEDENTE
    '        'QUINDI UNA LETTURA PRECEDENTE A QUELLA CHE SI STA MODIFICANDO
    '        '*******************************************************************
    '        strTemp = ""
    '        strCondition = ""
    '        strCondition = "DATALETTURA < " & DataLetturaSalvata & vbCrLf
    '        strCondition = strCondition & "ORDER BY DATALETTURA DESC"

    '        strTemp = sSQL & strCondition

    '        sqlCmdInsert = New SqlCommand(strTemp, sqlConn, sqlTrans)

    '        sqlDataReader = sqlCmdInsert.ExecuteReader


    '        If sqlDataReader.Read Then
    '            lngDataLetturaPrecedente = utility.stringoperation.formatint(sqlDataReader.Item("DATALETTURA"))
    '            lngGiorniDiConsumoPrecedenti = DateDiff(DateInterval.Day, CDate(FncGen.GiraDataFromDB(lngDataLetturaPrecedente)), CDate(DataLetturaAttuale))
    '        End If

    '        If Not sqlDataReader.IsClosed Then
    '            sqlDataReader.Close()
    '        End If




    '        Select Case lngDataLetturaSuccessiva

    '            Case Is > 0    '  E' PRESENTE UNA LETTURA SUCCESSIVA
    '                sSQL = ""
    '                sSQL = "UPDATE TP_LETTURE SET " & vbCrLf
    '                sSQL += "GIORNIDICONSUMO = " & Utility.StringOperation.FormatString(lngGiorniDiConsumoSuccessivi) & vbCrLf
    '                sSQL += "WHERE" & vbCrLf
    '                sSQL += "TP_LETTURE.CODLETTURA=" & lngCodLetturaSuccessiva & vbCrLf
    '                sSQL += "AND TP_LETTURE.CODCONTATORE=" & IDContatore
    '                'sSQL+="AND TP_LETTURE.CODUTENTE=" & IDUtente

    '                Try
    '                    sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '                    sqlCmdInsert.ExecuteNonQuery()

    '                Catch er As Exception
    '                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.GiorniDiConsumi.errore: ", er)
    '                    RaiseError.trace(er, sSQL.Replace(vbCrLf, ""), ConfigurationManager.AppSettings("Filename"), ConfigurationManager.AppSettings("FilePath"), "")
    '                    Throw er
    '                End Try

    '        End Select
    '        Select Case lngDataLetturaPrecedente

    '            Case Is > 0

    '                sSQL = ""
    '                sSQL = "UPDATE TP_LETTURE SET " & vbCrLf
    '                sSQL += "GIORNIDICONSUMO = " & Utility.StringOperation.FormatString(lngGiorniDiConsumoPrecedenti) & vbCrLf
    '                sSQL += "WHERE" & vbCrLf
    '                sSQL += "TP_LETTURE.CODLETTURA=" & IDLettura & vbCrLf
    '                sSQL += "AND TP_LETTURE.CODCONTATORE=" & IDContatore
    '                'sSQL+="AND TP_LETTURE.CODUTENTE=" & IDUtente

    '                Try
    '                    sqlCmdInsert = New SqlCommand(sSQL, sqlConn, sqlTrans)
    '                    sqlCmdInsert.ExecuteNonQuery()

    '                Catch er As Exception
    '                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.GiorniDiConsumi.errore: ", er)
    '                    RaiseError.trace(er, sSQL.Replace(vbCrLf, ""), ConfigurationManager.AppSettings("Filename"), ConfigurationManager.AppSettings("FilePath"), "")
    '                    Throw er
    '                End Try


    '        End Select

    '    Catch ex As Exception

    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.GiorniDiConsumi.errore: ", ex)
    '    End Try
    'End Sub

    '''' <summary>
    '''' Determina i metri cubi consumati della lettura che si sta inserendo e aggiorna la tabella nel database.
    '''' </summary>
    '''' <param name="LetturaAttuale"></param>
    '''' <param name="LetturaSalvata">Data lettura precedente</param>
    '''' <param name="IDContatore">Chiave primaria della tabella TP_CONTATORI</param>
    '''' <param name="IDUtente">ID anagrafico</param>
    '''' <param name="IDLettura"></param>
    '''' <param name="DetailContatore"></param>
    '''' <remarks>
    '''' </remarks>
    'Protected Sub Consumo(ByVal LetturaAttuale As String, ByVal LetturaSalvata As String, ByVal IDContatore As Long, ByVal IDUtente As Long, ByVal IDLettura As Long, ByVal DetailContatore As objContatore)
    '    Try

    '        Dim strCondition, strTemp, strValoreFondoScala As String
    '        Dim dvMyDati As New DataView
    '        Dim blnGiroContatore As Boolean = False
    '        Dim blnGiroContatorePrec As Boolean = False
    '        Dim blnLETTURAVUOTA As Boolean = False

    '        Dim lngLetturaSuccessiva, lngLetturaPrecedente, lngCodLetturaSuccessiva, lngConsumoEffettivoSuccessivo, lngConsumoEffettivoPrecedente, lngCifreContatore, lngValoreFondoScala, lngCount As Integer

    '        strValoreFondoScala = ""
    '        lngLetturaSuccessiva = 0
    '        lngLetturaPrecedente = 0


    '        '*************************************************************************************
    '        'VERIFICA ESISTENZA LETTURA SUCCESSIVA
    '        '*************************************************************************************
    '        dvMyDati = New clsLetture().GetTopLetture(ConstSession.DBType, ConstSession.StringConnection, 1, IDContatore, LetturaSalvata, ">")
    '        For Each myRow As DataRowView In dvMyDati
    '            lngLetturaSuccessiva = StringOperation.FormatInt(myRow("LETTURA"))
    '            lngCodLetturaSuccessiva = StringOperation.FormatInt(myRow("CODLETTURA"))
    '            '********************************************************************************************
    '            'LETTURA ATTUALE - LETTURA PRECEDENTE
    '            '********************************************************************************************
    '            If Len(LetturaAttuale.Trim) = 0 Then
    '                blnLETTURAVUOTA = True
    '            End If
    '            If Not blnLETTURAVUOTA Then
    '                lngConsumoEffettivoSuccessivo = lngLetturaSuccessiva - StringOperation.FormatInt(LetturaAttuale)
    '            Else
    '                lngConsumoEffettivoSuccessivo = 0
    '            End If

    '            If lngConsumoEffettivoSuccessivo < 0 Then
    '                lngCifreContatore = StringOperation.FormatInt(DetailContatore.sCifreContatore)
    '                If Len(Trim(DetailContatore.sCifreContatore)) > 0 Then
    '                    '*************************************************************
    '                    'Se non sono indicate le cifre del contatore il giro contatore deve essere digitato
    '                    'Dall'Utente e flaggare il flag Giro Contatore
    '                    '*************************************************************
    '                    For lngCount = 1 To lngCifreContatore
    '                        strValoreFondoScala = strValoreFondoScala & "9"
    '                    Next
    '                    lngValoreFondoScala = StringOperation.FormatInt(strValoreFondoScala)

    '                    lngConsumoEffettivoSuccessivo = lngValoreFondoScala - LetturaAttuale
    '                    lngConsumoEffettivoSuccessivo = (lngLetturaSuccessiva - 0) + lngConsumoEffettivoSuccessivo
    '                    blnGiroContatore = True
    '                End If

    '            End If
    '        Next
    '        dvMyDati.Dispose()

    '        dvMyDati = New clsLetture().GetTopLetture(ConstSession.DBType, ConstSession.StringConnection, 1, IDContatore, LetturaSalvata, "<")
    '        For Each myRow As DataRowView In dvMyDati
    '            lngLetturaPrecedente = StringOperation.FormatInt(myRow("LETTURA"))
    '            If Len(LetturaAttuale.Trim) = 0 Then
    '                blnLETTURAVUOTA = True
    '            End If
    '            If Not blnLETTURAVUOTA Then
    '                lngConsumoEffettivoPrecedente = LetturaAttuale - StringOperation.FormatInt(myRow("LETTURA"))
    '            Else
    '                lngConsumoEffettivoPrecedente = 0
    '            End If

    '            If lngConsumoEffettivoPrecedente < 0 Then
    '                lngCifreContatore = StringOperation.FormatInt(DetailContatore.sCifreContatore)
    '                If Len(Trim(DetailContatore.sCifreContatore)) > 0 Then
    '                    '*************************************************************
    '                    'Se non sono indicate le cifre del contatore il giro contatore deve essere digitato
    '                    'Dall'Utente e flaggare il flag Giro Contatore
    '                    '*************************************************************
    '                    For lngCount = 1 To lngCifreContatore
    '                        strValoreFondoScala = strValoreFondoScala & "9"
    '                    Next
    '                    lngValoreFondoScala = StringOperation.FormatInt(strValoreFondoScala)

    '                    lngConsumoEffettivoPrecedente = lngValoreFondoScala - lngLetturaPrecedente
    '                    lngConsumoEffettivoPrecedente = (LetturaAttuale - 0) + lngConsumoEffettivoPrecedente
    '                    blnGiroContatorePrec = True
    '                End If
    '            End If
    '        Next
    '        dvMyDati.Dispose()

    '        Dim myLettura As New ObjLettura
    '        If lngCodLetturaSuccessiva > 0 Then    '  E' PRESENTE UNA LETTURA SUCCESSIVA
    '            myLettura = New ObjLettura
    '            myLettura.IdLettura = lngCodLetturaSuccessiva
    '            myLettura.nIdContatore = IDContatore
    '            myLettura.nConsumo = lngConsumoEffettivoSuccessivo
    '            If blnGiroContatore Then
    '                myLettura.bIsGiroContatore = True
    '            Else
    '                myLettura.bIsGiroContatore = False
    '            End If
    '            myLettura.sAzione = "CONSUMO"
    '            SetLettura(myLettura)
    '        End If
    '        myLettura = New ObjLettura
    '        myLettura.IdLettura = IDLettura
    '        myLettura.nIdContatore = IDContatore
    '        myLettura.nConsumo = lngConsumoEffettivoPrecedente
    '        If blnGiroContatore Then
    '            myLettura.bIsGiroContatore = True
    '        Else
    '            myLettura.bIsGiroContatore = False
    '        End If
    '        myLettura.sAzione = "CONSUMO"
    '        SetLettura(myLettura)
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.Consumo.errore: ", ex)
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nIdContatore"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Function GetConsumoSubContatore(ByVal nIdContatore As Integer) As Integer
        Dim nConsumoSub As Integer = 0

        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetConsumoSubContatore", "IDCONTATORE")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDCONTATORE", nIdContatore))
                ctx.Dispose()
            End Using
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    nConsumoSub = Utility.StringOperation.FormatInt(myRow("totconsumo"))
                Next
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.GetConsumoSubContatore.errore: ", ex)
            nConsumoSub = -1
        Finally
            dvMyDati.Dispose()
        End Try
        Return nConsumoSub
    End Function
    'Public Function GetConsumoSubContatore(ByVal nIdContatore As Integer) As Integer
    '    Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim dtMyDati As New DataTable()
    '    Dim dtMyRow As DataRow
    '    Dim nConsumoSub As Integer = 0

    '    Try
    '        cmdMyCommand = New SqlClient.SqlCommand
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.AddWithValue("@DICONTATORE", nIdContatore)
    '        cmdMyCommand.CommandText = "prc_GetConsumoSubContatore"
    '        myAdapter.SelectCommand = cmdMyCommand
    '        myAdapter.Fill(dtMyDati)
    '        myAdapter.Dispose()
    '        For Each dtMyRow In dtMyDati.Rows
    '            If Not IsDBNull(dtMyRow("maxid")) Then
    '                nConsumoSub = CInt(dtMyRow("totconsumo"))
    '            End If
    '        Next
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestLetture.GetConsumoSubContatore.errore: ", ex)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        nConsumoSub = -1
    '    Finally
    '        dtMyDati.Dispose()
    '        cmdMyCommand.Dispose()
    '        cmdMyCommand.Connection.Close()
    '    End Try
    '    Return nConsumoSub
    'End Function
    Public Function GetInfoLettureContribuente(ByVal sIdEnte As String, ByVal sCodContribuente As String) As DataView
        Dim sSQL As String
        Dim dvMyDati As DataView
        Dim NOME_DATABASE_H2O As String

        Try
            NOME_DATABASE_H2O = ConfigurationManager.AppSettings("NOME_DATABASE_H20")

            sSQL = "SELECT * "
            sSQL += " FROM " & NOME_DATABASE_H2O & ".dbo.OPENgov_ELENCO_LETTURE"
            sSQL += " WHERE (COD_CONTRIBUENTE = " & sCodContribuente & ")"
            sSQL += " AND (CODENTE = '" & sIdEnte & "')"
            sSQL += " ORDER BY MATRICOLA ASC, PERIODO DESC, DATALETTURA DESC"
            dvMyDati = iDB.GetDataView(sSQL)

            Return dvMyDati
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.GestLetture.GetInfoLettureContribuente.errore: ", Err)
            Return Nothing
        End Try
    End Function
    'Public Function getTableLetture(ByVal sParametroENV As String, ByVal sUserName As String, ByVal sIdentificativoApplicazione As String, ByVal sIdEnte As String, ByVal nIdPeriodo As Integer, ByVal sIntestatario As String, ByVal sUtente As String, ByVal sVia As String, ByVal sNumeroUtente As String, ByVal sMatricola As String, ByVal nIdGiro As Integer, ByVal bSub As Boolean) As DataView
    '    Dim WFErrore As String
    '    Dim WFSession As OPENUtility.CreateSessione
    '    Dim dvMyDati As DataView

    '    Try
    '        'inizializzo la connessione
    '        WFSession = New OPENUtility.CreateSessione(sParametroENV, sUserName, sIdentificativoApplicazione)
    '        If Not WFSession.CreaSessione(sUserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        cmdMyCommand.Connection = WFSession.oSession.oAppDB.GetConnection
    '        cmdMyCommand.Parameters.Clear()
    '        'valorizzo il CommandText;
    '        cmdMyCommand.CommandText = "SELECT *"
    '        cmdMyCommand.CommandText += " FROM OPENGOV_STAMPALETTURE"
    '        cmdMyCommand.CommandText += " WHERE (1=1)"
    '        cmdMyCommand.CommandText += " AND (CODENTE=@IDENTE )"
    '        cmdMyCommand.CommandText += " AND (CODPERIODO=@IDPERIODO)"
    '        If sIntestatario <> "" Then
    '            cmdMyCommand.CommandText += " AND (COGNOME_INT+' '+NOME_INT LIKE @INTESTATARIO)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@INTESTATARIO", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(sIntestatario) & "%"
    '        End If
    '        If sUtente <> "" Then
    '            cmdMyCommand.CommandText += " AND (COGNOME_UT+' '+NOME_UT LIKE @UTENTE)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@UTENTE", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(sUtente) & "%"
    '        End If
    '        If sVia <> "" Then
    '            cmdMyCommand.CommandText += " AND (VIA_UBICAZIONE=@VIA)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VIA", SqlDbType.NVarChar)).Value = sVia
    '        End If
    '        If sNumeroUtente <> "" Then
    '            cmdMyCommand.CommandText += " AND (NUMEROUTENTE=@NUTENTE)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUTENTE", SqlDbType.NVarChar)).Value = sNumeroUtente
    '        End If
    '        If sMatricola <> "" Then
    '            cmdMyCommand.CommandText += " AND (MATRICOLA=@MATRICOLA)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@MATRICOLA", SqlDbType.NVarChar)).Value = sMatricola
    '        End If
    '        If nIdGiro > 0 Then
    '            cmdMyCommand.CommandText += " AND (CODGIRO=@IDGIRO)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDGIRO", SqlDbType.Int)).Value = nIdGiro
    '        End If
    '        If bSub = True Then
    '            cmdMyCommand.CommandText += " AND (NOT CODCONTATORESUB IS NULL)"
    '        End If
    '        cmdMyCommand.CommandText += " ORDER BY "
    '        cmdMyCommand.CommandText += " COGNOME_INT,"
    '        cmdMyCommand.CommandText += " NOME_INT,"
    '        cmdMyCommand.CommandText += " NUMEROUTENTE,"
    '        cmdMyCommand.CommandText += " MATRICOLA"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPERIODO", SqlDbType.Int)).Value = nIdPeriodo
    '        dvMyDati = WFSession.oSession.oAppDB.GetPrivateDataview(cmdMyCommand)
    '        Return dvMyDati
    '    Catch Err As Exception
    '        Dim sValParametri As String = FncGen.GetValParamCmd(cmdMyCommand)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestLetture.GetTableLetture.errore: ", ex)
    '        Log.Debug("Si è verificato un errore in GestContatori::GetTableContatoriAttivi::" & Err.Message )
    '        Return Nothing
    '    End Try
    'End Function

    'Public Function getTableGridInsoluti(ByVal sParametroENV As String, ByVal sUsername As String, ByVal sApplicazione As String, ByVal sIdEnte As String, ByVal impSoglia As Double, ByVal sAnno As String, Optional ByVal IsInsoluti As Integer = 0) As DataSet
    '    'IsInsoluti: {1=Parziali, 2=Totali}
    '    Dim culture As IFormatProvider
    '    culture = New System.Globalization.CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")
    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Dim sSQL, WFErrore As String
    '    Dim DsDati As DataSet

    '    Try
    '        'inizializzo la connessione
    '        WFSessione = New OPENUtility.CreateSessione(sParametroENV, sUsername, sApplicazione)
    '        If Not WFSessione.CreaSessione(sUsername, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        'prelevo tutte le fatture emesse
    '        sSQL = "SELECT TMPTBL_EMESSO.COD_UTENTE, TMPTBL_EMESSO.COGNOME_DENOMINAZIONE, TMPTBL_EMESSO.NOME, CFPIVA,"
    '        sSQL += " TP_FATTURE_NOTE.VIA_RES, TP_FATTURE_NOTE.CIVICO_RES, TP_FATTURE_NOTE.ESPONENTE_RES, TP_FATTURE_NOTE.INTERNO_RES, TP_FATTURE_NOTE.SCALA_RES, TP_FATTURE_NOTE.FRAZIONE_RES, TP_FATTURE_NOTE.CAP_RES, TP_FATTURE_NOTE.COMUNE_RES, TP_FATTURE_NOTE.PROVINCIA_RES,"
    '        sSQL += " NOME_INVIO, VIA_RCP, CIVICO_RCP, ESPONENTE_RCP, INTERNO_RCP, SCALA_RCP, FRAZIONE_RCP, CAP_RCP, COMUNE_RCP, PROVINCIA_RCP,"
    '        sSQL += NomeDBAnagrafe & ".ANAGRAFICA.COGNOME_DENOMINAZIONE AS COGNOME_INTEST, " & NomeDBAnagrafe & ".ANAGRAFICA.NOME AS NOME_INTEST,"
    '        sSQL += " NUMEROUTENTE, MATRICOLA, VIA_CONTATORE, CIVICO_CONTATORE, FRAZIONE_CONTATORE, DESCRIZIONE, NUTENZE,"
    '        sSQL += " TMPTBL_EMESSO.DATA_FATTURA, TMPTBL_EMESSO.NUMERO_FATTURA, RIF_NOTE_CREDITO=CASE WHEN RIF_NOTA IS NULL THEN 'NOTA NON ANCORA FATTURATA' ELSE RIF_NOTA END,"
    '        sSQL += " IMPEMESSO, SUM(CASE WHEN IMPORTO_PAGAMENTO IS NULL THEN 0 ELSE IMPORTO_PAGAMENTO END) AS IMPPAGATO,"
    '        sSQL += " (IMPEMESSO-SUM(CASE WHEN IMPORTO_PAGAMENTO IS NULL THEN 0 ELSE IMPORTO_PAGAMENTO END)) AS IMPDIFFERENZA"
    '        sSQL += " FROM ("
    '        sSQL += "   SELECT TP_FATTURE_NOTE.COD_UTENTE, COGNOME_DENOMINAZIONE, NOME, CFPIVA= CASE WHEN NOT PARTITA_IVA IS NULL AND PARTITA_IVA<>'' THEN PARTITA_IVA ELSE COD_FISCALE END,"
    '        sSQL += "   TP_FATTURE_NOTE.DATA_FATTURA, TP_FATTURE_NOTE.NUMERO_FATTURA, RIF_NOTA, IMPORTO_FATTURANOTA, IMPNOTA=CASE WHEN IMPORTO_NOTA IS NULL THEN 0 ELSE IMPORTO_NOTA END,"
    '        sSQL += "   (IMPORTO_FATTURANOTA+CASE WHEN IMPORTO_NOTA IS NULL THEN 0 ELSE IMPORTO_NOTA END) AS IMPEMESSO"
    '        sSQL += "   FROM TP_FATTURE_NOTE"
    '        sSQL += "   LEFT JOIN ("
    '        sSQL += "       SELECT IDENTE+DATA_FATTURA_RIFERIMENTO+NUMERO_FATTURA_RIFERIMENTO AS RIFERIMENTO_FATTURA, IMPORTO_FATTURANOTA AS IMPORTO_NOTA,"
    '        sSQL += "       MAX('NOTA N.'+NUMERO_FATTURA+' DEL '+SUBSTRING(DATA_FATTURA,7,2)+'/'+SUBSTRING(DATA_FATTURA,5,2)+'/'+SUBSTRING(DATA_FATTURA,1,4)) AS RIF_NOTA"
    '        sSQL += "       FROM TP_FATTURE_NOTE"
    '        sSQL += "       WHERE (TP_FATTURE_NOTE.DATA_VARIAZIONE IS NULL) AND (IDENTE='" & sIdEnte & "')"
    '        sSQL += "       AND (NOT IDENTE+DATA_FATTURA_RIFERIMENTO+NUMERO_FATTURA_RIFERIMENTO IS NULL)"
    '        sSQL += "       GROUP BY IDENTE+DATA_FATTURA_RIFERIMENTO+NUMERO_FATTURA_RIFERIMENTO, IMPORTO_FATTURANOTA"
    '        sSQL += "   ) TMPTABLE ON TP_FATTURE_NOTE.IDENTE+TP_FATTURE_NOTE.DATA_FATTURA+TP_FATTURE_NOTE.NUMERO_FATTURA=TMPTABLE.RIFERIMENTO_FATTURA"
    '        sSQL += "   WHERE (TP_FATTURE_NOTE.DATA_VARIAZIONE IS NULL) AND (TP_FATTURE_NOTE.IDENTE = '" & sIdEnte & "')"
    '        sSQL += "   AND (IDFLUSSO IN("
    '        sSQL += "       SELECT ID_FLUSSO"
    '        sSQL += "       FROM TP_FATTURAZIONI_GENERATE"
    '        sSQL += "       WHERE (IDENTE='" & sIdEnte & "')"
    '        sSQL += "       AND (NOT DATA_APPROVAZIONE_DOCUMENTI IS NULL AND DATA_APPROVAZIONE_DOCUMENTI<>'')"
    '        sSQL += "       AND (DATA_SCADENZA<=GETDATE())))"
    '        sSQL += "   GROUP BY TP_FATTURE_NOTE.COD_UTENTE, COGNOME_DENOMINAZIONE, NOME, CASE WHEN NOT PARTITA_IVA IS NULL AND PARTITA_IVA<>'' THEN PARTITA_IVA ELSE COD_FISCALE END,"
    '        sSQL += "   TP_FATTURE_NOTE.DATA_FATTURA, TP_FATTURE_NOTE.NUMERO_FATTURA, RIF_NOTA, IMPORTO_FATTURANOTA, IMPORTO_NOTA"
    '        sSQL += " ) TMPTBL_EMESSO INNER JOIN TP_FATTURE_NOTE ON TMPTBL_EMESSO.DATA_FATTURA=TP_FATTURE_NOTE.DATA_FATTURA AND TMPTBL_EMESSO.NUMERO_FATTURA=TP_FATTURE_NOTE.NUMERO_FATTURA"
    '        sSQL += " INNER JOIN " & NomeDBAnagrafe & ".ANAGRAFICA ON TP_FATTURE_NOTE.COD_INTESTATARIO=" & NomeDBAnagrafe & ".ANAGRAFICA.COD_CONTRIBUENTE"
    '        sSQL += " INNER JOIN TP_TIPIUTENZA ON TP_FATTURE_NOTE.ID_TIPOLOGIA_UTENZA=TP_TIPIUTENZA.IDTIPOUTENZA"
    '        sSQL += " LEFT JOIN TP_PAGAMENTI ON TMPTBL_EMESSO.DATA_FATTURA=TP_PAGAMENTI.DATA_FATTURA AND TMPTBL_EMESSO.NUMERO_FATTURA=TP_PAGAMENTI.NUMERO_FATTURA"
    '        If sAnno <> "" Then
    '            sSQL += " AND (SUBSTRING(TP_FATTURE_NOTE.DATA_FATTURA,1,4)='" & sAnno & "')"
    '        End If
    '        sSQL += " GROUP BY TMPTBL_EMESSO.COD_UTENTE, TMPTBL_EMESSO.COGNOME_DENOMINAZIONE, TMPTBL_EMESSO.NOME, CFPIVA,"
    '        sSQL += " TP_FATTURE_NOTE.VIA_RES, TP_FATTURE_NOTE.CIVICO_RES, TP_FATTURE_NOTE.ESPONENTE_RES, TP_FATTURE_NOTE.INTERNO_RES, TP_FATTURE_NOTE.SCALA_RES, TP_FATTURE_NOTE.FRAZIONE_RES, TP_FATTURE_NOTE.CAP_RES, TP_FATTURE_NOTE.COMUNE_RES, TP_FATTURE_NOTE.PROVINCIA_RES,"
    '        sSQL += " NOME_INVIO, VIA_RCP, CIVICO_RCP, ESPONENTE_RCP, INTERNO_RCP, SCALA_RCP, FRAZIONE_RCP, CAP_RCP, COMUNE_RCP, PROVINCIA_RCP,"
    '        sSQL += " " & NomeDBAnagrafe & ".ANAGRAFICA.COGNOME_DENOMINAZIONE, " & ConfigurationManager.AppSettings("NomeDbAnagrafe").ToString() & ".ANAGRAFICA.NOME,"
    '        sSQL += " NUMEROUTENTE, MATRICOLA, VIA_CONTATORE, CIVICO_CONTATORE, FRAZIONE_CONTATORE, DESCRIZIONE, NUTENZE,"
    '        sSQL += " TMPTBL_EMESSO.DATA_FATTURA, TMPTBL_EMESSO.NUMERO_FATTURA, CASE WHEN RIF_NOTA IS NULL THEN 'NOTA NON ANCORA FATTURATA' ELSE RIF_NOTA END, IMPEMESSO"
    '        If IsInsoluti = 1 Then
    '            sSQL += " HAVING (SUM(CASE WHEN IMPORTO_PAGAMENTO IS NULL THEN 0 ELSE IMPORTO_PAGAMENTO END)>0)"
    '            sSQL += " AND ((IMPEMESSO-SUM(CASE WHEN IMPORTO_PAGAMENTO IS NULL THEN 0 ELSE IMPORTO_PAGAMENTO END))>0)"
    '            If impSoglia > 0 Then
    '                sSQL += " AND ((IMPEMESSO-SUM(CASE WHEN IMPORTO_PAGAMENTO IS NULL THEN 0 ELSE IMPORTO_PAGAMENTO END))>=" & impSoglia & ")"
    '            End If
    '        ElseIf IsInsoluti = 2 Then
    '            sSQL += " HAVING (SUM(CASE WHEN IMPORTO_PAGAMENTO IS NULL THEN 0 ELSE IMPORTO_PAGAMENTO END)=0)"
    '            If impSoglia > 0 Then
    '                sSQL += " AND (IMPEMESSO>=" & impSoglia & ")"
    '            End If
    '        Else
    '            sSQL += " HAVING ((IMPEMESSO-SUM(CASE WHEN IMPORTO_PAGAMENTO IS NULL THEN 0 ELSE IMPORTO_PAGAMENTO END))>0)"
    '            If impSoglia > 0 Then
    '                sSQL += " AND ((IMPEMESSO-SUM(CASE WHEN IMPORTO_PAGAMENTO IS NULL THEN 0 ELSE IMPORTO_PAGAMENTO END))>=" & impSoglia & ")"
    '            End If
    '        End If
    '        sSQL += " ORDER BY TMPTBL_EMESSO.COGNOME_DENOMINAZIONE, TMPTBL_EMESSO.NOME, CFPIVA,"
    '        sSQL += " TMPTBL_EMESSO.DATA_FATTURA, TMPTBL_EMESSO.NUMERO_FATTURA"
    '        'eseguo la query
    '        DsDati = WFSessione.oSession.oAppDB.GetPrivateDataSet(sSQL)

    '        Return DsDati
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestLetture.GetTabelGridInsoluti.errore: ", ex)
    '        Log.Debug("Si è verificato un errore in GestLetture::getTableGridInsoluti::" & Err.Message & " SQL::" & sSQL)
    '       
    '        Return Nothing
    '    End Try
    'End Function
    '********************************************************************
    'commentato il 22/08/2008 - LAURA
    '********************************************************************
    'Public Function getTableGridInsoluti_MARCIA(ByVal dataFattura As String, ByVal soglia As String) As DataTable
    '    dim sSQL as string
    '    Dim oconn As New SqlConnection
    '    Dim ocmd As New SqlCommand
    '    Dim sqlCmdInsert As SqlCommand

    '    Try
    '        oConn.ConnectionString = ConstSession.StringConnection

    '        sSQL = "SELECT COUNT(*) AS CONTEGGIO, TP_FATTURE_NOTE.IMPORTO_FATTURANOTA AS IMPORTOFATTURANOTA, SUM(TP_PAGAMENTI.IMPORTO_PAGAMENTO) AS SOMMAPAGAMENTI,"
    '        sSQL+=" TP_FATTURE_NOTE.ANNO_RIFERIMENTO AS MIOANNO, TP_FATTURE_NOTE.COGNOME_DENOMINAZIONE AS COGNOME, TP_FATTURE_NOTE.NOME AS NOME, "
    '        sSQL+=" TP_FATTURE_NOTE.DATA_FATTURA AS DATAFATTURA, TP_FATTURE_NOTE.IMPORTO_FATTURANOTA - SUM(TP_PAGAMENTI.IMPORTO_PAGAMENTO) AS DIFFERENZA, "
    '        sSQL+=" TP_FATTURE_NOTE.NUMEROUTENTE AS NUTENTE, TP_FATTURE_NOTE.COD_FISCALE AS CODFISCALE, TP_FATTURE_NOTE.PARTITA_IVA AS PARTITAIVA, "
    '        sSQL+=" TP_FATTURE_NOTE.VIA_RES, TP_FATTURE_NOTE.CIVICO_RES, TP_FATTURE_NOTE.ESPONENTE_RES, TP_FATTURE_NOTE.FRAZIONE_RES, TP_FATTURE_NOTE.CAP_RES, TP_FATTURE_NOTE.COMUNE_RES, "
    '        sSQL+=" TP_FATTURE_NOTE.PROVINCIA_RES, TP_FATTURE_NOTE.VIA_RCP,TP_FATTURE_NOTE.CIVICO_RCP, TP_FATTURE_NOTE.ESPONENTE_RCP, "
    '        sSQL+=" TP_FATTURE_NOTE.FRAZIONE_RCP, TP_FATTURE_NOTE.CAP_RCP, TP_FATTURE_NOTE.COMUNE_RCP, TP_FATTURE_NOTE.PROVINCIA_RCP, "
    '        sSQL+=" TP_FATTURE_NOTE.VIA_CONTATORE, TP_FATTURE_NOTE.CIVICO_CONTATORE, TP_FATTURE_NOTE.FRAZIONE_CONTATORE, "
    '        sSQL+=" TP_FATTURE_NOTE.MATRICOLA,TP_FATTURE_NOTE.ID_TIPOLOGIA_UTENZA,TP_FATTURE_NOTE.NUTENZE,TP_FATTURE_NOTE.NUMERO_FATTURA AS MYNUMERO, "
    '        sSQL+=" TP_PAGAMENTI.DATA_FATTURA,TP_FATTURE_NOTE.COD_INTESTATARIO "
    '        sSQL+=" FROM TP_PAGAMENTI INNER JOIN"
    '        sSQL+=" TP_FATTURE_NOTE ON TP_PAGAMENTI.DATA_FATTURA = TP_FATTURE_NOTE.DATA_FATTURA AND "
    '        sSQL+=" TP_PAGAMENTI.NUMERO_FATTURA = TP_FATTURE_NOTE.NUMERO_FATTURA "
    '        sSQL+=" WHERE (TP_PAGAMENTI.IDENTE = " & ConstSession.IdEnte & ")"
    '        sSQL+=" GROUP BY TP_FATTURE_NOTE.IMPORTO_FATTURANOTA, TP_FATTURE_NOTE.COGNOME_DENOMINAZIONE, TP_FATTURE_NOTE.NOME, "
    '        sSQL+=" TP_FATTURE_NOTE.DATA_FATTURA, TP_FATTURE_NOTE.NUMEROUTENTE, TP_FATTURE_NOTE.COD_FISCALE, TP_FATTURE_NOTE.PARTITA_IVA, "
    '        sSQL+=" TP_FATTURE_NOTE.VIA_RES, TP_FATTURE_NOTE.CIVICO_RES, TP_FATTURE_NOTE.ESPONENTE_RES, TP_FATTURE_NOTE.FRAZIONE_RES, "
    '        sSQL+=" TP_FATTURE_NOTE.CAP_RES, TP_FATTURE_NOTE.COMUNE_RES, TP_FATTURE_NOTE.PROVINCIA_RES,"
    '        sSQL+=" TP_FATTURE_NOTE.VIA_RCP, TP_FATTURE_NOTE.CIVICO_RCP, "
    '        sSQL+=" TP_FATTURE_NOTE.ESPONENTE_RCP, TP_FATTURE_NOTE.FRAZIONE_RCP, TP_FATTURE_NOTE.CAP_RCP, "
    '        sSQL+=" TP_FATTURE_NOTE.COMUNE_RCP, TP_FATTURE_NOTE.PROVINCIA_RCP,"
    '        sSQL+=" TP_FATTURE_NOTE.VIA_CONTATORE, TP_FATTURE_NOTE.CIVICO_CONTATORE, TP_FATTURE_NOTE.FRAZIONE_CONTATORE,"
    '        sSQL+=" TP_FATTURE_NOTE.MATRICOLA,TP_FATTURE_NOTE.ID_TIPOLOGIA_UTENZA,TP_FATTURE_NOTE.NUTENZE,TP_FATTURE_NOTE.NUMERO_FATTURA,"
    '        sSQL+=" TP_PAGAMENTI.DATA_FATTURA,TP_FATTURE_NOTE.COD_INTESTATARIO, TP_FATTURE_NOTE.ANNO_RIFERIMENTO"
    '        Dim ds As DataSet
    '        ds = DBAccess.RunSQLReturnDataSet(sSQL)
    '        Dim dt As DataTable
    '        dt = ds.Tables(0)

    '        Dim i As Integer
    '        Dim dr As SqlDataReader

    '        For i = 0 To dt.Rows.Count - 1
    '            sSQL = "SELECT SUM(TP_FATTURE_NOTE.IMPORTO_FATTURANOTA) AS SOMMATORIA"
    '            sSQL += " FROM TP_FATTURE_NOTE "
    '            sSQL += " WHERE TIPO_DOCUMENTO='N' "
    '            sSQL += " AND NUMERO_FATTURA_RIFERIMENTO =" & dt.Rows(i)("MYNUMERO")
    '            sSQL += " AND DATA_FATTURA_RIFERIMENTO ='" & dt.Rows(i)("DATAFATTURA") & "'"
    '            sSQL += " AND IDENTE=" & ConstSession.IdEnte
    '            dr = DBAccess.GetDataReader(sSQL)
    '            Dim totale As Double = 0
    '            If dr.Read() Then
    '                If Not IsDBNull(dr("sommatoria")) Then
    '                    totale += dr("SOMMATORIA")
    '                End If
    '            End If
    '            dr.Close()

    '            If totale <> 0 Then
    '                dt.Rows(i)("IMPORTOFATTURANOTA") = dt.Rows(i)("IMPORTOFATTURANOTA") + totale
    '                dt.AcceptChanges()

    '                dt.Rows(i)("DIFFERENZA") = CDbl(dt.Rows(i)("IMPORTOFATTURANOTA")) - CDbl(dt.Rows(i)("SOMMAPAGAMENTI"))
    '                dt.AcceptChanges()
    '            End If
    '        Next

    '        If soglia <> "" Then
    '            Dim foundRows() As DataRow
    '            foundRows = dt.Select("differenza <= " & soglia)

    '            Dim b As Int16

    '            For b = 0 To foundRows.Length() - 1
    '                foundRows(b).Delete()
    '                dt.AcceptChanges()
    '            Next
    '        End If

    '        If dataFattura <> "" Then
    '            Dim foundRows2() As DataRow
    '            foundRows2 = dt.Select("MIOANNO <> '" & dataFattura & "'")

    '            Dim c As Int16
    '            For c = 0 To foundRows2.Length() - 1
    '                foundRows2(c).Delete()
    '                dt.AcceptChanges()
    '            Next
    '        End If

    '        Return dt
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestLetture.GetTabelGridInsoluti_MARCIA.errore: ", ex)
    '        Return Nothing
    '    End Try
    'End Function
    '********************************************************************
    'Public Function getTableLettureNonPresenti(ByVal sParametroENV As String, ByVal sUserName As String, ByVal sIdentificativoApplicazione As String, ByVal sIdEnte As String, ByVal nIdPeriodo As Integer, ByVal sIntestatario As String, ByVal sUtente As String, ByVal sVia As String, ByVal sNumeroUtente As String, ByVal sMatricola As String, ByVal nIdGiro As Integer, ByVal bSub As Boolean) As DataView
    '    Dim WFErrore As String
    '    Dim WFSession As OPENUtility.CreateSessione
    '    Dim dvMyDati As DataView
    '    Dim clsPeriodo As New TabelleDiDecodifica.DBPeriodo
    '    Dim oPeriodo As TabelleDiDecodifica.DetailPeriodo

    '    Try
    '        'inizializzo la connessione
    '        WFSession = New OPENUtility.CreateSessione(sParametroENV, sUserName, sIdentificativoApplicazione)
    '        If Not WFSession.CreaSessione(sUserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        'determino i dati del periodo
    '        oPeriodo = clsPeriodo.GetPeriodo(nIdPeriodo)

    '        cmdMyCommand.Connection = WFSession.oSession.oAppDB.GetConnection
    '        cmdMyCommand.Parameters.Clear()
    '        'valorizzo il CommandText;
    '        cmdMyCommand.CommandText = "SELECT *"
    '        cmdMyCommand.CommandText += " FROM OPENGOV_STAMPACONTATORILETTURE"
    '        cmdMyCommand.CommandText += " WHERE (DATAATTIVAZIONE IS NOT NULL)"
    '        cmdMyCommand.CommandText += " AND (DATAATTIVAZIONE <> '') "
    '        cmdMyCommand.CommandText += " AND (DATAATTIVAZIONE <=@DATAA) "
    '        cmdMyCommand.CommandText += " AND (DATACESSAZIONE IS NULL OR DATACESSAZIONE='')"
    '        cmdMyCommand.CommandText += " AND (CODCONTATORE NOT IN ("
    '        cmdMyCommand.CommandText += " 	SELECT DISTINCT TP_CONTATORI.CODCONTATORE "
    '        cmdMyCommand.CommandText += " 	FROM TP_LETTURE "
    '        cmdMyCommand.CommandText += " 	LEFT JOIN TP_CONTATORI ON TP_LETTURE.CODCONTATORE = TP_CONTATORI.CODCONTATORE "
    '        cmdMyCommand.CommandText += " 	LEFT JOIN TP_PERIODO ON TP_LETTURE.CODPERIODO = TP_PERIODO.CODPERIODO AND TP_CONTATORI.CODENTE = TP_PERIODO.COD_ENTE"
    '        cmdMyCommand.CommandText += " 	WHERE (TP_CONTATORI.CODENTE =@IDENTE) "
    '        cmdMyCommand.CommandText += " 	AND (TP_LETTURE.CODPERIODO=@IDPERIODO)"
    '        cmdMyCommand.CommandText += " 	AND (TP_LETTURE.PRIMALETTURA=0 OR TP_LETTURE.PRIMALETTURA IS NULL)"
    '        cmdMyCommand.CommandText += " ))"
    '        cmdMyCommand.CommandText += " AND (CODENTE=@IDENTE )"
    '        'cmdMyCommand.CommandText += " AND (CODPERIODO=@IDPERIODO)"
    '        If sIntestatario <> "" Then
    '            cmdMyCommand.CommandText += " AND (COGNOME_INT+' '+NOME_INT LIKE @INTESTATARIO)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@INTESTATARIO", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(sIntestatario) & "%"
    '        End If
    '        If sUtente <> "" Then
    '            cmdMyCommand.CommandText += " AND (COGNOME_UT+' '+NOME_UT LIKE @UTENTE)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@UTENTE", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(sUtente) & "%"
    '        End If
    '        If sVia <> "" Then
    '            cmdMyCommand.CommandText += " AND (VIA_UBICAZIONE=@VIA)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VIA", SqlDbType.NVarChar)).Value = sVia
    '        End If
    '        If sNumeroUtente <> "" Then
    '            cmdMyCommand.CommandText += " AND (NUMEROUTENTE=@NUTENTE)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUTENTE", SqlDbType.NVarChar)).Value = sNumeroUtente
    '        End If
    '        If sMatricola <> "" Then
    '            cmdMyCommand.CommandText += " AND (MATRICOLA=@MATRICOLA)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@MATRICOLA", SqlDbType.NVarChar)).Value = sMatricola
    '        End If
    '        If nIdGiro > 0 Then
    '            cmdMyCommand.CommandText += " AND (CODGIRO=@IDGIRO)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDGIRO", SqlDbType.Int)).Value = nIdGiro
    '        End If
    '        If bSub = True Then
    '            cmdMyCommand.CommandText += " AND (NOT CODCONTATORESUB IS NULL)"
    '        End If
    '        cmdMyCommand.CommandText += " ORDER BY COGNOME_INT,NOME_INT"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPERIODO", SqlDbType.Int)).Value = nIdPeriodo
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAA", SqlDbType.NVarChar)).Value = FncGen.GiraData(oPeriodo.AData)
    '        dvMyDati = WFSession.oSession.oAppDB.GetPrivateDataview(cmdMyCommand)
    '        Return dvMyDati
    '    Catch Err As Exception
    '        Dim sValParametri As String = FncGen.GetValParamCmd(cmdMyCommand)
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestLetture.GetTabelLettureNonPresenti.errore: ", ex)
    '        Log.Debug("Si è verificato un errore in GestContatori::getTableLettureNonPresenti::" & Err.Message )
    '        Return Nothing
    '    End Try
    'End Function
    '********************************************************************
    'commentato il 25/08/2008 - LAURA
    '********************************************************************
    'Public Function getListaInsoluti(ByVal dataFattura As String, ByVal soglia As String) As DataTable

    '    Try


    '        dim sSQL as string

    '        sSQL="SELECT COUNT(*) AS CONTEGGIO, TP_FATTURE_NOTE.IMPORTO_FATTURANOTA AS IMPORTOFATTURANOTA, SUM(TP_PAGAMENTI.IMPORTO_PAGAMENTO) "
    '        sSQL+=" AS SOMMAPAGAMENTI, TP_FATTURE_NOTE.COGNOME_DENOMINAZIONE AS COGNOME, TP_FATTURE_NOTE.NOME, "
    '        sSQL+=" TP_FATTURE_NOTE.DATA_FATTURA AS DATAFATTURA,TP_FATTURE_NOTE.NUMERO_FATTURA AS MYNUMERO, "
    '        sSQL+=" TP_FATTURE_NOTE.IMPORTO_FATTURANOTA - SUM(TP_PAGAMENTI.IMPORTO_PAGAMENTO) AS DIFFERENZA "
    '        sSQL+=" FROM TP_PAGAMENTI INNER JOIN "
    '        sSQL+=" TP_FATTURE_NOTE ON TP_PAGAMENTI.DATA_FATTURA = TP_FATTURE_NOTE.DATA_FATTURA AND "
    '        sSQL+=" TP_PAGAMENTI.NUMERO_FATTURA = TP_FATTURE_NOTE.NUMERO_FATTURA "
    '        sSQL+=" WHERE(TP_PAGAMENTI.IDENTE = " & ConstSession.IdEnte & ") "

    '        If dataFattura <> "" Then
    '            sSQL+=" AND TP_FATTURE_NOTE.DATA_FATTURA >= '" & dataFattura & "0101' "
    '            sSQL+=" AND TP_FATTURE_NOTE.DATA_FATTURA < '" & CInt(dataFattura) + 1 & "0101' "
    '        End If
    '        sSQL+=" GROUP BY TP_FATTURE_NOTE.IMPORTO_FATTURANOTA, TP_FATTURE_NOTE.COGNOME_DENOMINAZIONE, TP_FATTURE_NOTE.NOME,"
    '        sSQL+=" TP_FATTURE_NOTE.DATA_FATTURA,TP_FATTURE_NOTE.NUMERO_FATTURA"
    '        If soglia <> "" Then
    '            sSQL+=" HAVING SUM(TP_PAGAMENTI.IMPORTO_PAGAMENTO) > " & CDbl(soglia) & " "
    '        End If


    '        Dim ds As DataSet
    '        ds = DBAccess.RunSQLReturnDataSet(sSQL)
    '        Dim dt As DataTable = ds.Tables(0)
    '        Return dt

    '    Catch ex As Exception
    '     Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestLetture.GetListaInsoluti.errore: ", ex)
    '    End Try
    'End Function
    '********************************************************************
    'Public Function BindData(ByVal IDContatore As Integer, ByVal CodUtente As Integer, Optional ByVal conanomalie As Boolean = False, Optional ByVal lasciatoavviso As Boolean = False) As objDBListSQL
    'Try
    '    dim sSQL as string
    '    Dim lngRecordCount As Long
    '    Dim GetListaLetture As New objDBListSQL

    '    GetListaLetture.oConn = iDB.GetConnection

    '    sSQL = "SELECT TOP 5 * "
    '    sSQL += " FROM dbo.OPENgov_ELENCO_LETTURE"
    '    sSQL += " WHERE (CODCONTATORE = " & IDContatore & ")"
    '    sSQL += " ORDER BY DATALETTURA DESC"

    '    GetListaLetture.Query = sSQL
    '    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
    '    'Ritorno il Record Count
    '    Dim drTemp As SqlDataReader
    '    drTemp = iDB.GetDataReader(GetListaLetture.Query)
    '    Do While drTemp.Read
    '        lngRecordCount = lngRecordCount + 1
    '    Loop

    '    drTemp.Close()
    '    GetListaLetture.RecordCount = lngRecordCount

    '    Return GetListaLetture
    '    Catch ex As Exception
    '     Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestLetture.BindData.errore: ", ex)
    '    End Try
    'End Function

    'Public Function GetListaContatori(ByVal Intestatario As String, ByVal Utente As String, ByVal IDVia As Integer, ByVal Giro As Integer, ByVal NumeroUtente As String, ByVal Cessati As Integer, ByVal sIdEnte As String, ByVal matricola As String, ByVal Boolsub As Boolean, ByVal IsLetturaPresente As Boolean) As objDBListSQL
    'Try
    '    dim sSQL as string
    '    Dim sSQLCount As String
    '    Dim sSQLWhere As String
    '    Dim sSQLFrom As String
    '    Dim sSQLGroup As String
    '    Dim sSQLOrder As String
    '    Dim SearchChar As String = "*"
    '    Dim intPosString As Integer

    '    'determino i dati del periodo
    '    Dim clsPeriodo As New TabelleDiDecodifica.DBPeriodo
    '    Dim oPeriodo As TabelleDiDecodifica.DetailPeriodo
    '    oPeriodo = clsPeriodo.GetPeriodo(ConstSession.IdPeriodo)

    '    Dim ModDate As New ClsGenerale.Generale

    '    Dim GetListContatori As New objDBListSQL
    '    GetListContatori.oConn = iDB.GetConnection

    '    sSQL = "SELECT OPENgov_ELENCO_CONTATORI.COGNOME_INT, OPENgov_ELENCO_CONTATORI.NOME_INT"
    '    sSQL += " ,OPENgov_ELENCO_CONTATORI.COGNOME_UT,OPENgov_ELENCO_CONTATORI.NOME_UT"
    '    sSQL += " ,OPENgov_ELENCO_CONTATORI.VIA_UBICAZIONE,OPENgov_ELENCO_CONTATORI.CIVICO_UBICAZIONE"
    '    sSQL += " ,OPENgov_ELENCO_CONTATORI.MATRICOLA,OPENgov_ELENCO_CONTATORI.NUMEROUTENTE,OPENgov_ELENCO_CONTATORI.CODCONTATORE"
    '    sSQL += " , CASE WHEN NOT MAX(LETTURE.CODCONTATORE) IS NULL THEN 'X' ELSE NULL END AS LETTURA"

    '    sSQLFrom = " FROM OPENgov_ELENCO_CONTATORI"
    '    sSQLFrom += " LEFT JOIN ("
    '    sSQLFrom += " 	SELECT CODCONTATORE"
    '    sSQLFrom += " 	FROM TP_LETTURE "
    '    sSQLFrom += " 	WHERE (CODPERIODO =" & ConstSession.IdPeriodo & ")"
    '    sSQLFrom += " 	AND (PRIMALETTURA IS NULL OR PRIMALETTURA=0)"
    '    sSQLFrom += " 	) LETTURE ON OPENGOV_ELENCO_CONTATORI.CODCONTATORE=LETTURE.CODCONTATORE"

    '    sSQLWhere = " WHERE (1 = 1)"
    '    sSQLWhere = sSQLWhere & " AND (DATAATTIVAZIONE IS NOT NULL AND DATAATTIVAZIONE <> '' and DATAATTIVAZIONE <= " & ModDate.GiraData(oPeriodo.AData) & ") "
    '    sSQLWhere = sSQLWhere & " AND ((DATACESSAZIONE IS NULL OR DATACESSAZIONE ='') OR (DATACESSAZIONE >= " & ModDate.GiraData(oPeriodo.DaData) & "))"
    '    sSQLWhere = sSQLWhere & " AND (CODENTE=" & utility.stringoperation.formatstring(sIdEnte) & ")"
    '    If Giro <> -1 Then
    '        sSQLWhere += " AND (IDGIRO=" & Giro & ")"
    '    End If
    '    If Len(Trim(NumeroUtente)) > 0 Then
    '        sSQLWhere = sSQLWhere & " AND (NUMEROUTENTE=" & utility.stringoperation.formatstring(NumeroUtente) & ")"
    '    End If
    '    If IDVia <> -1 Then
    '        sSQLWhere = sSQLWhere & " AND (COD_STRADA=" & IDVia & ")"
    '    End If
    '    If Boolsub = True Then
    '        sSQLWhere = sSQLWhere & " AND (NOT CODCONTATORESUB IS NULL)"
    '    End If
    '    If Len(Trim(Intestatario)) > 0 Then
    '        sSQLWhere = sSQLWhere & " AND (COGNOME_INT+' '+NOME_INT like  '" & Replace(Replace(Trim(Intestatario), "'", "''"), "*", "%") & "%')"
    '    End If
    '    If Len(Trim(Utente)) > 0 Then
    '        sSQLWhere = sSQLWhere & " AND (COGNOME_UT+' '+NOME_UT like  '" & Replace(Replace(Trim(Utente), "'", "''"), "*", "%") & "%')"
    '    End If
    '    If Len(Trim(matricola)) > 0 Then
    '        intPosString = InStr(matricola, SearchChar)
    '        If intPosString > 0 Then
    '            sSQLWhere = sSQLWhere & " AND (MATRICOLA like  '" & Replace(Replace(Trim(matricola), "'", "''"), "*", "%") & "%')"
    '        Else
    '            sSQLWhere = sSQLWhere & " AND (MATRICOLA =" & matricola & ")"
    '        End If
    '    End If
    '    If IsLetturaPresente = True Then
    '        sSQLWhere += " AND (NOT LETTURE.CODCONTATORE IS NULL)"
    '    End If

    '    sSQLGroup = " GROUP BY OPENgov_ELENCO_CONTATORI.COGNOME_INT, OPENgov_ELENCO_CONTATORI.NOME_INT"
    '    sSQLGroup += " ,OPENgov_ELENCO_CONTATORI.COGNOME_UT,OPENgov_ELENCO_CONTATORI.NOME_UT"
    '    sSQLGroup += " ,OPENgov_ELENCO_CONTATORI.VIA_UBICAZIONE,OPENgov_ELENCO_CONTATORI.CIVICO_UBICAZIONE"
    '    sSQLGroup += " ,OPENgov_ELENCO_CONTATORI.MATRICOLA,OPENgov_ELENCO_CONTATORI.NUMEROUTENTE,OPENgov_ELENCO_CONTATORI.CODCONTATORE"

    '    sSQLOrder = " ORDER BY "
    '    sSQLOrder += " COGNOME_INT+' '+NOME_INT,"
    '    sSQLOrder += " COGNOME_UT+' '+NOME_UT"
    '    sSQLOrder += " ,MATRICOLA"

    '    GetListContatori.Query = sSQL & sSQLFrom & sSQLWhere & sSQLGroup & sSQLOrder
    '    GetListContatori.QueryCount = "Select Count(*) AS NUMERORECORD " & sSQLFrom & sSQLWhere

    '    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
    '    'Ritorno il Record Count
    '    Dim drTemp As SqlDataReader
    '    drTemp = iDB.GetDataReader(GetListContatori.QueryCount)
    '    If drTemp.Read Then
    '        GetListContatori.RecordCount = utility.stringoperation.formatint(drTemp.Item("NUMERORECORD"))
    '    End If
    '    drTemp.Close()
    '    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/'

    '    Return GetListContatori
    '    Catch ex As Exception
    '     Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestLetture.GetListaContatori.errore: ", ex)
    '    End Try
    'End Function

    'Protected Function getValoreFondoScala(ByVal strCodContatore As String) As String

    '    dim sSQL as string

    '    sSQL="SELECT TP_TIPOCONTATORE.VALOREFONDOSCALA" & vbCrLf
    '    sSQL+="FROM  TP_CONTATORI INNER JOIN" & vbCrLf
    '    sSQL+="TP_TIPOCONTATORE ON TP_CONTATORI.IDTIPOCONTATORE = TP_TIPOCONTATORE.IDTIPOCONTATORE" & vbCrLf
    '    sSQL+="WHERE TP_CONTATORI.CODCONTATORE = " & strCodContatore
    '    Return sSQL

    'End Function

    'Public Sub CalcolaConsumoEffettivo(ByVal strLettura As String, ByVal DataLettura As String, ByRef lngConsumoEffettivo As Long, ByVal strCodUtente As String, ByVal strCodContatore As String, Optional ByVal orderby As String = "DESC")
    '    Dim sqlDataReader As SqlDataReader
    '    Dim lngValoreFondoScala, lngLetturaPrecedente, nConsumoSub As Long
    'Try
    '    'Ricavo se Esiste L'ultima Lettura Eseguita e fatturata
    '    sqlDataReader = DBAccess.GetDataReader(getTopOneLetture(strCodContatore, strCodUtente, DataLettura, orderby))
    '    If sqlDataReader.Read() Then
    '        lngLetturaPrecedente = utility.stringoperation.formatint(sqlDataReader.Item("LETTURA"))
    '        lngConsumoEffettivo = utility.stringoperation.formatint(strLettura) - utility.stringoperation.formatint(sqlDataReader.Item("LETTURA"))
    '        If Not sqlDataReader.IsClosed Then
    '            sqlDataReader.Close()
    '        End If
    '        ' Si verifica quando la Lettura Attuale é minore di quella precedente
    '        If lngConsumoEffettivo < 0 Then
    '            'Verifico e considero  il Giro Contatore
    '            sqlDataReader = DBAccess.GetDataReader(getValoreFondoScala(strCodContatore))
    '            sqlDataReader.Read()
    '            lngValoreFondoScala = utility.stringoperation.formatint(sqlDataReader.Item("VALOREFONDOSCALA"))
    '            lngConsumoEffettivo = lngValoreFondoScala - lngLetturaPrecedente
    '            lngConsumoEffettivo = (utility.stringoperation.formatint(strLettura) - 0) + lngConsumoEffettivo
    '            sqlDataReader.Close()
    '        End If
    '    Else
    '        'Non Ci sono Letture Precedenti
    '        lngConsumoEffettivo = 0
    '    End If
    '    '***tolgo l'eventuale consumo del subcontatore associato***
    '    nConsumoSub = GetConsumoSubContatore(CInt(strCodContatore))
    '    If nConsumoSub < 0 Then
    '        Exit Sub
    '    End If
    '    lngConsumoEffettivo -= nConsumoSub
    '    '**********************************************************
    '    Catch ex As Exception
    '     Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestLetture.CalcolaConsumoEffettivo.errore: ", ex)
    '    End Try
    'End Sub

    'Public Function GetInfoLettureContribuente(ByVal sIdEnte As String, ByVal sCodContribuente As String, ByVal WFSessione As OPENUtility.CreateSessione) As DataView
    '    dim sSQL as string
    '    Dim dvMyDati As DataView
    '    Dim NOME_DATABASE_H2O As String

    '    Try
    '        NOME_DATABASE_H2O = ConfigurationManager.AppSettings("NOME_DATABASE_H20")

    '        sSQL = "SELECT * "
    '        sSQL += " FROM " & NOME_DATABASE_H2O & ".dbo.OPENgov_ELENCO_LETTURE"
    '        sSQL += " WHERE (COD_CONTRIBUENTE = " & sCodContribuente & ")"
    '        sSQL += " AND (CODENTE = '" & sIdEnte & "')"
    '        sSQL += " ORDER BY MATRICOLA ASC, PERIODO DESC, DATALETTURA DESC"
    '        dvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataview(sSQL)

    '        Return dvMyDati
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.GestLetture.GetInfoLettureContribuente.errore: ", Err)
    '        Return Nothing
    '    End Try
    'End Function
End Class
