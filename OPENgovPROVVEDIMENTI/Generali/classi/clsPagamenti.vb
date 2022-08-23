Imports log4net
Imports Utility
''' <summary>
''' Classe per la gestione di accorpamenti/pagamenti
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class clsPagamenti
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(clsPagamenti))

    Public Enum enumTIPO_VOCE
        ALTRO = 1
        ARROTONDAMENTO_PROVVEDIMENTO = 2
        DIFFERENZA_IMPOSTA = 3
        INTERESSI = 4
        SANZIONI_NON_RIDUCIBILI = 5
        SANZIONI_RIDOTTE = 6
        SPESE = 7
        ARROTONDAMENTO_PAGATO = 8
    End Enum

    Public Enum enumTIPO_CAPITOLO
        PROVVEDIMENTO = 1
        INTERESSI = 2
        ARROTONDAMENTO_PAGATO = 3
    End Enum

    Public sParametroENV As String
    Public sUserName As String
    Public sApplicazione As String
    Private strWFErrore As String
    'Private objSessione As CreateSessione
    'Private objDBManager As DBManager
    Private strConnectionStringOPENgovProvvedimenti As String
    Private sSQL As String
    Private iRecordAffected As Integer
    '
    ''' <summary>
    ''' Costruttore
    ''' </summary>
    ''' <param name="MyConn"></param>
    Public Sub New(ByVal MyConn As String)
        Try
            strConnectionStringOPENgovProvvedimenti = MyConn
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.New.errore: ", ex)
            Throw New Exception(ex.Message, ex)
        End Try
    End Sub
    'Distruttore
    Public Sub kill()
        Finalize()
    End Sub
    Protected Overrides Sub Finalize()
        'If Not IsNothing(myDataSet) Then myDataSet.Dispose()
        'objSessione.oOM.Terminate()
        'objSessione.oSM.Terminate()
        'objDBManager.Kill()
        MyBase.Finalize()
    End Sub

    ''' <summary>
    ''' Verifica se è gia stato effettuato un pagamento e preleva gli importi delle spese
    ''' </summary>
    ''' <param name="id_accorpamento">integer id accorpamento</param>
    ''' <param name="impSpese">ref double importo totale delle spese dell'acccorpamento</param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Function haSpesePagate(ByVal id_accorpamento As Integer, ByRef impSpese As Double) As Boolean
        Dim bRet As Boolean = False
        Dim myDataView As New DataView

        Try
            impSpese = 0
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_AccorpamentoSpesePagate", "IDACCORPAMENTO")
                myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDACCORPAMENTO", id_accorpamento))
                For Each myRow As DataRowView In myDataView
                    impSpese = StringOperation.FormatDouble(myRow("impspese"))
                    If StringOperation.FormatDouble(myRow("spesepag")) > 0 Then
                        bRet = True
                    End If
                Next
                ctx.Dispose()
            End Using
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.haSpesePagate.errore: ", ex)
        Finally
            myDataView.Dispose()
        End Try
        Return bRet
    End Function
    'Function haSpesePagate(ByVal id_accorpamento As Integer, ByRef impSpese As Double) As Boolean
    '    Dim bRet As Boolean = False
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim myDataSet As New DataSet

    '    Try
    '        impSpese = 0
    '        sSQL = "SELECT IMPSPESE, SUM(IMPORTO) AS SPESEPAG"
    '        sSQL += " FROM PGM_PAGATO PAG"
    '        sSQL += " RIGHT JOIN PGM_PAGATO_DETTAGLIO DET ON DET.ID_PAGATO = PAG.ID_PAGATO"
    '        sSQL += " RIGHT JOIN ("
    '        sSQL += " 	SELECT @IDACCORPAMENTO AS MIOID, SUM(IMPORTO_SPESE) AS IMPSPESE"
    '        sSQL += " 	FROM PROVVEDIMENTI"
    '        sSQL += " 	WHERE ID_PROVVEDIMENTO IN ("
    '        sSQL += " 		SELECT ID_PROVVEDIMENTO "
    '        sSQL += " 		FROM PGM_ACCORPAMENTO "
    '        sSQL += " 		WHERE ID_ACCORPAMENTO=@IDACCORPAMENTO"
    '        sSQL += " 	)"
    '        sSQL += " ) SPESE ON PAG.ID_ACCORPAMENTO=MIOID"
    '        sSQL += " WHERE 1=1 "
    '        sSQL += " AND (DET.ID_TIPO_VOCE=7 OR ID_TIPO_VOCE IS NULL)"
    '        sSQL += " AND MIOID=@IDACCORPAMENTO"
    '        sSQL += " GROUP BY IMPSPESE"
    '        cmdMyCommand = New SqlClient.SqlCommand
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        cmdMyCommand.CommandText = sSQL
    '        cmdMyCommand.Parameters.AddWithValue("@IDACCORPAMENTO", id_accorpamento)
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(myDataSet, "Create DataView")
    '        myAdapter.Dispose()
    '        If Not IsNothing(myDataSet) Then
    '            If myDataSet.Tables(0).Rows.Count > 0 Then
    '                If Not IsDBNull(myDataSet.Tables(0).Rows(0)("impspese")) Then
    '                    impSpese = CDbl(myDataSet.Tables(0).Rows(0)("impspese"))
    '                End If
    '                If Not IsDBNull(myDataSet.Tables(0).Rows(0)("spesepag")) Then
    '                    bRet = True
    '                End If
    '            End If
    '        End If
    '        Return bRet
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.haSpesePagate.errore: ", ex)
    '    Finally
    '        myDataSet.Dispose()
    '        cmdMyCommand.Connection.Close()
    '    End Try
    'End Function

    ''' <summary>
    ''' Seleziona i provvedimenti di un contribuente che non hanno ancora un pagamento
    ''' </summary>
    ''' <param name="cod_contribuente"></param>
    ''' <param name="sIdEnte"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="04/07/2012">
    ''' <strong>IMU</strong>
    ''' passaggio tributo da ICI a IMU
    ''' </revision>
    ''' </revisionHistory>
    Function getProvvedimenti(ByVal cod_contribuente As Integer, ByVal sIdEnte As String) As DataSet
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim myDataSet As New DataSet
        Try
            '*** 20130801 - accertamento OSAP ***
            'sSQL = "SELECT PROVVEDIMENTI.ID_PROVVEDIMENTO,DATA_ELABORAZIONE,"
            'sSQL += " PROVVEDIMENTI.COD_TRIBUTO, ANNO, "
            'sSQL += " NUMERO_ATTO=CASE WHEN NUMERO_ATTO IS NULL OR NUMERO_ATTO='' THEN '' ELSE NUMERO_ATTO END, "
            'sSQL += " IMPORTO_TOTALE_RIDOTTO, IMPORTO_TOTALE, "
            'sSQL += " DATA_NOTIFICA_AVVISO, DATA_STAMPA, DATA_CONSEGNA_AVVISO, DATA_ANNULLAMENTO_AVVISO,"
            'sSQL += " ID_ACCORPAMENTO= CASE WHEN ID_ACCORPAMENTO IS NULL THEN -1 ELSE ID_ACCORPAMENTO END, "
            ''*** 20120704 - IMU ***
            'sSQL += " PROVPROC = CASE WHEN PROVVEDIMENTI.COD_TRIBUTO='8852' THEN 'ICI/IMU - ' ELSE 'TARSU - ' END+CASE WHEN ID_ACCORPAMENTO IS NULL THEN 'PROVVEDIMENTO' ELSE 'RATEIZZAZIONE N.'+ CONVERT(NVARCHAR,ID_ACCORPAMENTO) END,"
            'sSQL += " TIPO = CASE WHEN PGM_ACCORPAMENTO.TIPO='P' OR ID_ACCORPAMENTO IS NULL THEN 'P' ELSE 'A' END,"
            'sSQL += " 0 AS SELEZIONATO"
            'sSQL += " FROM PROVVEDIMENTI "
            'sSQL += " INNER JOIN TAB_PROCEDIMENTI ON"
            'sSQL += " TAB_PROCEDIMENTI.ID_PROVVEDIMENTO = PROVVEDIMENTI.ID_PROVVEDIMENTO"
            'sSQL += " LEFT OUTER JOIN PGM_ACCORPAMENTO ON "
            'sSQL += " PGM_ACCORPAMENTO.ID_PROVVEDIMENTO = PROVVEDIMENTI.ID_PROVVEDIMENTO"
            'sSQL += " WHERE (PROVVEDIMENTI.COD_ENTE='" & sIdEnte & "')"
            'sSQL += " AND PROVVEDIMENTI.COD_CONTRIBUENTE=" & cod_contribuente
            ''SSQL += " AND DATA_STAMPA IS NOT NULL"
            ''SSQL += " AND DATA_CONSEGNA_AVVISO IS NOT NULL"
            'sSQL += " AND (DATA_ANNULLAMENTO_AVVISO IS NULL OR DATA_ANNULLAMENTO_AVVISO='')"
            'sSQL += " AND (DATA_RETTIFICA_AVVISO IS NULL OR DATA_RETTIFICA_AVVISO='')"
            ''*** 20130125 - inserimento gestione pagato anche rispetto all'importo pieno ***
            'sSQL += " AND (NOT DATA_NOTIFICA_AVVISO IS NULL AND DATA_NOTIFICA_AVVISO<>'')"
            ''*** ***
            'sSQL += " ORDER BY PROVVEDIMENTI.COD_TRIBUTO,ANNO DESC, DATA_NOTIFICA_AVVISO DESC"
            'objDS = objDBManager.GetPrivateDataSet(sSQL)
            cmdMyCommand = New SqlClient.SqlCommand
            'Log.Debug("connessione->" + ConstSession.StringConnection)
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.CommandText = "exec prc_GetProvVSPag @IdEnte,@IdContribuente"
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdEnte", SqlDbType.NVarChar)).Value = sIdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdContribuente", SqlDbType.Int)).Value = cod_contribuente
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(myDataSet, "Create DataView")
            myAdapter.Dispose()
            '*** ***

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.getProvvedimenti.errore: ", ex)
        Finally
            cmdMyCommand.Connection.Close()
        End Try
        Return myDataSet
    End Function
    '
    ''' <summary>
    ''' Seleziona gli importi relativi ad un determinato provvedimento
    ''' </summary>
    ''' <param name="id_provvedimento"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Function getProvvedimento(ByVal id_provvedimento As Integer) As Hashtable
        Dim myDataView As New DataView
        Dim htHashtable As New Hashtable

        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_GetProvvedimento", "IDPROVVEDIMENTO")
                myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDPROVVEDIMENTO", id_provvedimento))
                For Each myRow As DataRowView In myDataView
                    htHashtable = New Hashtable
                    htHashtable.Add("IMPORTO_TOTALE_RIDOTTO", StringOperation.FormatDouble(myRow("IMPORTO_TOTALE_RIDOTTO")))
                    htHashtable.Add("IMPORTO_DIFFERENZA_IMPOSTA", StringOperation.FormatDouble(myRow("IMPORTO_DIFFERENZA_IMPOSTA")))
                    htHashtable.Add("IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI", StringOperation.FormatDouble(myRow("IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI")))
                    htHashtable.Add("IMPORTO_SANZIONI_RIDOTTO", StringOperation.FormatDouble(myRow("IMPORTO_SANZIONI_RIDOTTO")))
                    htHashtable.Add("IMPORTO_INTERESSI", StringOperation.FormatDouble(myRow("IMPORTO_INTERESSI")))
                    htHashtable.Add("IMPORTO_ALTRO", StringOperation.FormatDouble(myRow("IMPORTO_ALTRO")))
                    htHashtable.Add("IMPORTO_ARROTONDAMENTO_RIDOTTO", StringOperation.FormatDouble(myRow("IMPORTO_ARROTONDAMENTO_RIDOTTO")))
                    htHashtable.Add("IMPORTO_SPESE", StringOperation.FormatDouble(myRow("IMPORTO_SPESE")))
                Next
                ctx.Dispose()
            End Using
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.getProvvedimento.errore: ", ex)
        Finally
            myDataView.Dispose()
        End Try
        Return htHashtable
    End Function
    'Function getProvvedimento(ByVal id_provvedimento As Integer) As Hashtable
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim myDataSet As New DataSet
    '    Dim htHashtable As New Hashtable

    '    Try
    '        sSQL = "select IMPORTO_TOTALE_RIDOTTO,IMPORTO_DIFFERENZA_IMPOSTA, IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI, IMPORTO_SANZIONI_RIDOTTO,"
    '        sSQL += " IMPORTO_INTERESSI, IMPORTO_ALTRO, IMPORTO_ARROTONDAMENTO_RIDOTTO, IMPORTO_SPESE"
    '        sSQL += " from provvedimenti"
    '        sSQL += " where id_provvedimento=" & id_provvedimento
    '        cmdMyCommand = New SqlClient.SqlCommand
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        cmdMyCommand.CommandText = sSQL
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(myDataSet, "Create DataView")
    '        myAdapter.Dispose()
    '        If Not IsNothing(myDataSet) Then
    '            If myDataSet.Tables(0).Rows.Count > 0 Then
    '                htHashtable = New Hashtable
    '                htHashtable.Add("IMPORTO_TOTALE_RIDOTTO", IIf(Not IsDBNull(myDataSet.Tables(0).Rows(0)("IMPORTO_TOTALE_RIDOTTO")), myDataSet.Tables(0).Rows(0)("IMPORTO_TOTALE_RIDOTTO"), 0))
    '                htHashtable.Add("IMPORTO_DIFFERENZA_IMPOSTA", IIf(Not IsDBNull(myDataSet.Tables(0).Rows(0)("IMPORTO_DIFFERENZA_IMPOSTA")), myDataSet.Tables(0).Rows(0)("IMPORTO_DIFFERENZA_IMPOSTA"), 0))
    '                htHashtable.Add("IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI", IIf(Not IsDBNull(myDataSet.Tables(0).Rows(0)("IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI")), myDataSet.Tables(0).Rows(0)("IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI"), 0))
    '                htHashtable.Add("IMPORTO_SANZIONI_RIDOTTO", IIf(Not IsDBNull(myDataSet.Tables(0).Rows(0)("IMPORTO_SANZIONI_RIDOTTO")), myDataSet.Tables(0).Rows(0)("IMPORTO_SANZIONI_RIDOTTO"), 0))
    '                htHashtable.Add("IMPORTO_INTERESSI", IIf(Not IsDBNull(myDataSet.Tables(0).Rows(0)("IMPORTO_INTERESSI")), myDataSet.Tables(0).Rows(0)("IMPORTO_INTERESSI"), 0))
    '                htHashtable.Add("IMPORTO_ALTRO", IIf(Not IsDBNull(myDataSet.Tables(0).Rows(0)("IMPORTO_ALTRO")), myDataSet.Tables(0).Rows(0)("IMPORTO_ALTRO"), 0))
    '                htHashtable.Add("IMPORTO_ARROTONDAMENTO_RIDOTTO", IIf(Not IsDBNull(myDataSet.Tables(0).Rows(0)("IMPORTO_ARROTONDAMENTO_RIDOTTO")), myDataSet.Tables(0).Rows(0)("IMPORTO_ARROTONDAMENTO_RIDOTTO"), 0))
    '                htHashtable.Add("IMPORTO_SPESE", IIf(Not IsDBNull(myDataSet.Tables(0).Rows(0)("IMPORTO_SPESE")), myDataSet.Tables(0).Rows(0)("IMPORTO_SPESE"), 0))
    '            End If
    '        End If

    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.getProvvedimento.errore: ", ex)
    '    Finally
    '        cmdMyCommand.Connection.Close()
    '    End Try
    '    Return htHashtable
    'End Function
    '
    ''' <summary>
    ''' Seleziona i provvedimenti legati ad un accorpamento
    ''' </summary>
    ''' <param name="id_accorpamento"></param>
    ''' <param name="id_provvedimento"></param>
    ''' <returns></returns>
    Function getProvvedimentiAccorpamento(ByVal id_accorpamento As Integer, ByVal id_provvedimento As Integer) As DataSet
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim myDataSet As New DataSet
        Try
            cmdMyCommand = New SqlClient.SqlCommand
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.CommandText = "exec prc_GetProvvedimentiAccorpamento @IdProvvedimento,@IdAccorpamento"
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdProvvedimento", SqlDbType.Int)).Value = id_provvedimento
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdAccorpamento", SqlDbType.Int)).Value = id_accorpamento
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(myDataSet, "Create DataView")
            myAdapter.Dispose()
            '*** ***
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.getProvvedimentiAccorpamento.errore: ", ex)
        Finally
            cmdMyCommand.Connection.Close()
        End Try
        Return myDataSet
    End Function
    '
    ''' <summary>
    ''' Seleziona le rate di un Provvedimento
    ''' </summary>
    ''' <param name="id_provvedimento"></param>
    ''' <param name="sIdEnte"></param>
    ''' <param name="bSoloRate"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="25/01/2013">
    ''' inserimento gestione pagato anche rispetto all'importo pieno
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="08/05/2019">
    ''' segnalazione 17/VI/19
    ''' </revision>
    ''' </revisionHistory>
    Function getRateProvvedimento(ByVal id_provvedimento As Integer, ByVal sIdEnte As String, ByVal bSoloRate As Boolean) As DataSet
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim myDataSet As New DataSet
        Try
            cmdMyCommand = New SqlClient.SqlCommand
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_GetRateProvvedimento"
            cmdMyCommand.Parameters.AddWithValue("@IDPROVVEDIMENTO", id_provvedimento)
            If bSoloRate = True Then
                cmdMyCommand.Parameters.AddWithValue("@SOLORATA", 1)
            Else
                cmdMyCommand.Parameters.AddWithValue("@SOLORATA", 0)
            End If
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(myDataSet, "Create DataView")
            myAdapter.Dispose()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.getRateProvvedimento.errore: ", ex)
        Finally
            cmdMyCommand.Connection.Close()
        End Try
        Return myDataSet
    End Function
    'Function getRateProvvedimento(ByVal id_provvedimento As Integer, ByVal sIdEnte As String, ByVal bSoloRate As Boolean) As DataSet
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim myDataSet As New DataSet
    '    Try
    '        sSQL = "SELECT PGM_RATA_PROVVEDIMENTO.*, "
    '        sSQL += " ID_PAGATO= CASE WHEN ID_PAGATO IS NULL THEN 0 ELSE ID_PAGATO END,"
    '        sSQL += " IMPORTO_PAGATO,DATA_PAGAMENTO,DATA_ACCREDITO,"
    '        sSQL += " PROVENIENZA, PGM_PROVENIENZA.DESCRIZIONE  AS DESCPROVENIENZA,"
    '        sSQL += " 0 AS ID_ACCORPAMENTO , ID_RATA_ACC "
    '        sSQL += " ,'P' AS TIPO"
    '        sSQL += " FROM PGM_RATA_PROVVEDIMENTO"
    '        sSQL += " INNER JOIN PGM_ACCORPAMENTO ON PGM_ACCORPAMENTO.ID_PROVVEDIMENTO=PGM_RATA_PROVVEDIMENTO.ID_PROVVEDIMENTO"
    '        sSQL += " LEFT JOIN PGM_PAGATO ON PGM_PAGATO.ID_ACCORPAMENTO=PGM_ACCORPAMENTO.ID_ACCORPAMENTO AND PGM_PAGATO.ID_RATA_PROVV=PGM_RATA_PROVVEDIMENTO.ID_RATA_PROVV"
    '        sSQL += " LEFT JOIN PGM_PROVENIENZA ON PGM_PROVENIENZA.ID_PROVENIENZA=PGM_PAGATO.PROVENIENZA"
    '        sSQL += " WHERE 1=1"
    '        sSQL += " AND PGM_RATA_PROVVEDIMENTO.ID_PROVVEDIMENTO = " & id_provvedimento
    '        '*** 20130125 - inserimento gestione pagato anche rispetto all'importo pieno ***
    '        If bSoloRate = True Then
    '            sSQL += " AND (PGM_ACCORPAMENTO.TIPO='A')"
    '        End If
    '        '*** ***
    '        sSQL += " ORDER BY PGM_RATA_PROVVEDIMENTO.N_RATA"
    '        cmdMyCommand = New SqlClient.SqlCommand
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        cmdMyCommand.CommandText = sSQL
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(myDataSet, "Create DataView")
    '        myAdapter.Dispose()

    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.getRateProvvedimento.errore: ", ex)
    '    Finally
    '        cmdMyCommand.Connection.Close()
    '    End Try
    '    Return myDataSet
    'End Function

    ''' <summary>
    ''' Seleziona le rate di un accorpamento
    ''' </summary>
    ''' <param name="id_accorpamento"></param>
    ''' <param name="sIdEnte"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="08/05/2019">
    ''' segnalazione 17/VI/19
    ''' </revision>
    ''' </revisionHistory>
    Function getRateAccorpamento(ByVal id_accorpamento As Integer, ByVal sIdEnte As String) As DataSet
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim myDataSet As New DataSet
        Try
            cmdMyCommand = New SqlClient.SqlCommand
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_GetRateAccorpamento"
            cmdMyCommand.Parameters.AddWithValue("@IDACCORPAMENTO", id_accorpamento)
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(myDataSet, "Create DataView")
            myAdapter.Dispose()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.getRateAccorpamento.errore: ", ex)
        Finally
            cmdMyCommand.Connection.Close()
        End Try
        Return myDataSet
    End Function
    'Function getRateAccorpamento(ByVal id_accorpamento As Integer, ByVal sIdEnte As String) As DataSet
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim myDataSet As New DataSet
    '    Try
    '        sSQL = "SELECT DISTINCT PGM_RATA_ACCORPAMENTO.*, "
    '        sSQL += " ID_PAGATO= CASE WHEN ID_PAGATO IS NULL THEN 0 ELSE ID_PAGATO END,"
    '        sSQL += " IMPORTO_PAGATO,DATA_PAGAMENTO,DATA_ACCREDITO,"
    '        sSQL += " PROVENIENZA, PGM_PROVENIENZA.DESCRIZIONE  AS DESCPROVENIENZA,"
    '        sSQL += " 0 AS ID_PROVVEDIMENTO, "
    '        sSQL += " ID_RATA_PROVV = CASE WHEN ID_RATA_PROVV IS NULL THEN 0 ELSE ID_RATA_PROVV END"
    '        sSQL += " ,'A' AS TIPO"
    '        sSQL += " FROM PGM_RATA_ACCORPAMENTO"
    '        sSQL += " LEFT JOIN PGM_PAGATO ON PGM_PAGATO.ID_ACCORPAMENTO = PGM_RATA_ACCORPAMENTO.ID_ACCORPAMENTO AND PGM_PAGATO.ID_RATA_ACC = PGM_RATA_ACCORPAMENTO.ID_RATA_ACC"
    '        sSQL += " LEFT JOIN PGM_PROVENIENZA ON PGM_PROVENIENZA.ID_PROVENIENZA = PGM_PAGATO.PROVENIENZA"
    '        sSQL += " WHERE 1=1"
    '        'sSQL += " AND (PGM_PROVENIENZA.COD_ENTE IS NULL OR PGM_PROVENIENZA.COD_ENTE='" & sIdEnte & "')"
    '        sSQL += " AND PGM_RATA_ACCORPAMENTO.ID_ACCORPAMENTO=" & id_accorpamento
    '        sSQL += " UNION"
    '        sSQL += " SELECT DISTINCT"
    '        sSQL += " -1 AS ID_RATA_ACC, PGM_PAGATO.ID_ACCORPAMENTO, PGM_PAGATO.N_RATA, NULL AS DATA_SCADENZA,"
    '        sSQL += " NULL AS VALORE_RATA, NULL AS VALORE_INTERESSE, NULL AS IMPORTO_TOTALE_RATA, PGM_PAGATO.ID_PAGATO,"
    '        sSQL += " IMPORTO_PAGATO,DATA_PAGAMENTO,DATA_ACCREDITO, PROVENIENZA, PGM_PROVENIENZA.DESCRIZIONE  AS DESCPROVENIENZA, 0 AS ID_PROVVEDIMENTO,  "
    '        sSQL += " ID_RATA_PROVV = CASE WHEN ID_RATA_PROVV IS NULL THEN 0 ELSE ID_RATA_PROVV END ,'A' AS TIPO "
    '        sSQL += " FROM PGM_PAGATO"
    '        sSQL += " LEFT JOIN PGM_PROVENIENZA ON  PGM_PROVENIENZA.ID_PROVENIENZA = PGM_PAGATO.PROVENIENZA "
    '        sSQL += " WHERE 1=1"
    '        'sSQL += " AND (PGM_PROVENIENZA.COD_ENTE IS NULL OR PGM_PROVENIENZA.COD_ENTE='" & sIdEnte & "')"
    '        sSQL += " AND PGM_PAGATO.ID_ACCORPAMENTO = " & id_accorpamento
    '        sSQL += " AND ID_RATA_PROVV=0 AND ID_RATA_ACC=0"
    '        sSQL += " ORDER BY N_RATA"
    '        cmdMyCommand = New SqlClient.SqlCommand
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        cmdMyCommand.CommandText = sSQL
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(myDataSet, "Create DataView")
    '        myAdapter.Dispose()


    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.getRateAccorpamento.errore: ", ex)
    '    Finally
    '        cmdMyCommand.Connection.Close()
    '    End Try
    '    Return myDataSet
    'End Function
    '
    ''' <summary>
    ''' Seleziona gli interessi di un accorpamento
    ''' </summary>
    ''' <param name="id_accorpamento"></param>
    ''' <param name="id_rata_acc"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Function getInteressiAccorpamento(ByVal id_accorpamento As Integer, ByVal id_rata_acc As Integer) As DataSet
        Dim myDataSet As New DataSet
        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_PGM_RATA_ACCORPAMENTO_S", "IDACCORPAMENTO", "IDRATAACC")
                myDataSet = ctx.GetDataSet(sSQL, "TBL", ctx.GetParam("IDACCORPAMENTO", id_accorpamento) _
                        , ctx.GetParam("IDRATAACC", id_rata_acc)
                    )
                ctx.Dispose()
            End Using
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.getInteressiAccorpamento.errore: ", ex)
        End Try
        Return myDataSet
    End Function
    'Function getInteressiAccorpamento(ByVal id_accorpamento As Integer, ByVal id_rata_acc As Integer) As DataSet
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim myDataSet As New DataSet
    '    Try
    '        sSQL = "sELECT *"
    '        sSQL += " FROM PGM_RATA_ACCORPAMENTO"
    '        sSQL += " WHERE ID_ACCORPAMENTO=" & id_accorpamento
    '        sSQL += " AND ID_RATA_ACC=" & id_rata_acc
    '        cmdMyCommand = New SqlClient.SqlCommand
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        cmdMyCommand.CommandText = sSQL
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(myDataSet, "Create DataView")
    '        myAdapter.Dispose()
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.getInteressiAccorpamento.errore: ", ex)
    '    Finally
    '        cmdMyCommand.Connection.Close()
    '    End Try
    '    Return myDataSet
    'End Function
    '
    ''' <summary>
    ''' Seleziona gli interessi di un accorpamento
    ''' </summary>
    ''' <param name="id_accorpamento"></param>
    ''' <param name="n_rata"></param>
    ''' <returns></returns>
    Function getIDRataAccorpamento(ByVal id_accorpamento As Integer, ByVal n_rata As Integer) As Integer
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim myDataSet As New DataSet
        Dim id_rata As Integer = -1
        Try
            sSQL = "sELECT *"
            sSQL += " FROM PGM_RATA_ACCORPAMENTO"
            sSQL += " WHERE ID_ACCORPAMENTO=" & id_accorpamento
            sSQL += " AND N_RATA=" & n_rata
            cmdMyCommand = New SqlClient.SqlCommand
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.CommandText = sSQL
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(myDataSet, "Create DataView")
            myAdapter.Dispose()
            If Not IsNothing(myDataSet) Then
                If myDataSet.Tables(0).Rows.Count > 0 Then
                    id_rata = myDataSet.Tables(0).Rows(0)("id_rata_acc")
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.getIDRataAccorpamento.errore: ", ex)
        Finally
            cmdMyCommand.Connection.Close()
        End Try
        Return id_rata
    End Function
    '
    ''' <summary>
    ''' Seleziona il totale di un accorpamento comprensivo degli interessi
    ''' </summary>
    ''' <param name="id_accorpamento"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Function getTotaleAccorpamento(ByVal id_accorpamento As Integer) As Decimal
        Dim myDataView As New DataView
        Dim Totale As Decimal = 0
        Try
            Dim oDbManagerRepository As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
            Using ctx As DBModel = oDbManagerRepository
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_GetTotaleAccorpamento", "IDACCORPAMENTO")
                myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDACCORPAMENTO", id_accorpamento))
                For Each myRow As DataRowView In myDataView
                    Totale = StringOperation.FormatDouble(myRow("Totale"))
                Next
                ctx.Dispose()
            End Using
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.getTotaleAccorpamento.errore: ", ex)
        Finally
            myDataView.Dispose()
        End Try
        Return Totale
    End Function
    'Function getTotaleAccorpamento(ByVal id_accorpamento As Integer) As Decimal
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim myDataSet As New DataSet
    '    Dim Totale As Decimal = 0
    '    Try
    '        sSQL = "selECT SUM(VALORE_RATA)+SUM(VALORE_INTERESSE) AS TOTALE "
    '        sSQL += " FROM PGM_RATA_ACCORPAMENTO"
    '        sSQL += " WHERE ID_ACCORPAMENTO= " & id_accorpamento
    '        cmdMyCommand = New SqlClient.SqlCommand
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        cmdMyCommand.CommandText = sSQL
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(myDataSet, "Create DataView")
    '        myAdapter.Dispose()
    '        If Not IsNothing(myDataSet) Then
    '            If myDataSet.Tables(0).Rows.Count > 0 Then
    '                Totale = myDataSet.Tables(0).Rows(0)("Totale")
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.getTotaleAccorpamento.errore: ", ex)
    '    Finally
    '        cmdMyCommand.Connection.Close()
    '    End Try
    '    Return Totale
    'End Function
    '
    ''' <summary>
    ''' Seleziona gli accorpamenti per nominativo
    ''' </summary>
    ''' <param name="sIdEnte"></param>
    ''' <param name="cognome"></param>
    ''' <param name="nome"></param>
    ''' <param name="cod_fiscale"></param>
    ''' <param name="partita_iva"></param>
    ''' <returns></returns>
    Function getAccorpamenti(ByVal sIdEnte As String, ByVal cognome As String, ByVal nome As String, ByVal cod_fiscale As String, ByVal partita_iva As String) As DataSet
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim myDataSet As New DataSet
        Try
            sSQL = "SELECT *"
            sSQL += " FROM V_GETACCORPAMENTI"
            sSQL += " WHERE 1=1"
            sSQL += " AND (IDENTE='" & sIdEnte & "')"
            If cognome <> "" Then
                sSQL += " AND COGNOME LIKE '" & cognome.Replace("'", "''") & "%'"
            End If
            If nome <> "" Then
                sSQL += " AND NOME LIKE '" & nome.Replace("'", "''") & "%'"
            End If
            If cod_fiscale <> "" Then
                sSQL += " AND COD_FISCALE LIKE '" & cod_fiscale & "%'"
            End If
            If partita_iva <> "" Then
                sSQL += " AND PARTITA_IVA LIKE '" & partita_iva & "%'"
            End If
            sSQL += " ORDER BY COGNOME, NOME, COD_FISCALE, PARTITA_IVA"
            cmdMyCommand = New SqlClient.SqlCommand
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.CommandText = sSQL
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(myDataSet, "Create DataView")
            myAdapter.Dispose()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.getAccorpamenti.errore: ", ex)
        Finally
            cmdMyCommand.Connection.Close()
        End Try
        Return myDataSet
    End Function
    ''' <summary>
    ''' ricerca i pagamenti in base ai parametri
    ''' </summary>
    ''' <param name="sIdEnte">string ente</param>
    ''' <param name="cognome">string cognome</param>
    ''' <param name="nome">string nome</param>
    ''' <param name="cod_fiscale">string codice fiscale</param>
    ''' <param name="partita_iva">string partita iva</param>
    ''' <param name="nIdProvvedimento">string id provvedimento</param>
    ''' <param name="NAtto">string numero atto</param>
    ''' <param name="Dal">DateTime dalla data pagamento</param>
    ''' <param name="Al">DateTime alla data pagamento </param>
    ''' <returns>DataSet risultato ricerca</returns>
    ''' <revisionHistory>
    ''' <revision date="13/03/2019">
    ''' <strong>ricerca per numero atto e date di pagamento</strong>
    ''' aggiunta la ricerca per anche per numero atto e date di pagamento
    ''' </revision>
    ''' </revisionHistory>    
    Function getPagamenti(ByVal sIdEnte As String, ByVal cognome As String, ByVal nome As String, ByVal cod_fiscale As String, ByVal partita_iva As String, ByVal nIdProvvedimento As Integer, NAtto As String, Dal As DateTime, Al As DateTime) As DataSet
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim myDataSet As New DataSet
        Try

            cmdMyCommand = New SqlClient.SqlCommand
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            cmdMyCommand.CommandTimeout = 0
            If nIdProvvedimento > 0 Then
                sSQL = "SELECT *"
                sSQL += " FROM V_GETPAGAMENTI"
                sSQL += " WHERE 1=1"
                sSQL += " AND (IDENTE=@IDENTE)"
                If cognome <> "" Then
                    sSQL += " AND COGNOME LIKE @COGNOME+'%'"
                End If
                If nome <> "" Then
                    sSQL += " AND NOME LIKE @NOME +'%'"
                End If
                If cod_fiscale <> "" Then
                    sSQL += " AND COD_FISCALE=@CODFISCALE"
                End If
                If partita_iva <> "" Then
                    sSQL += " AND PARTITA_IVA=@PIVA"
                End If
                sSQL += " And (ID_PROVVEDIMENTO=@IDPROVVEDIMENTO)"
                sSQL += " ORDER BY DOC, DATA_PAGAMENTO"
                cmdMyCommand.CommandType = CommandType.Text
                cmdMyCommand.CommandText = sSQL
                cmdMyCommand.Parameters.Clear()
                cmdMyCommand.Parameters.AddWithValue("@IDENTE", sIdEnte)
                cmdMyCommand.Parameters.AddWithValue("@COGNOME", cognome.Replace("'", "''"))
                cmdMyCommand.Parameters.AddWithValue("@NOME", nome.Replace("'", "''"))
                cmdMyCommand.Parameters.AddWithValue("@CODFISCALE", cod_fiscale)
                cmdMyCommand.Parameters.AddWithValue("@PIVA", partita_iva)
                cmdMyCommand.Parameters.AddWithValue("@IDPROVVEDIMENTO", nIdProvvedimento)
            Else
                cmdMyCommand.CommandType = CommandType.StoredProcedure
                cmdMyCommand.CommandText = "prc_GetPagamenti"
                cmdMyCommand.Parameters.Clear()
                cmdMyCommand.Parameters.AddWithValue("@IDENTE", sIdEnte)
                cmdMyCommand.Parameters.AddWithValue("@COGNOME", cognome.Replace("'", "''"))
                cmdMyCommand.Parameters.AddWithValue("@NOME", nome.Replace("'", "''"))
                cmdMyCommand.Parameters.AddWithValue("@CODFISCALE", cod_fiscale)
                cmdMyCommand.Parameters.AddWithValue("@PIVA", partita_iva)
                cmdMyCommand.Parameters.AddWithValue("@NATTO", NAtto)
                cmdMyCommand.Parameters.AddWithValue("@DAL", Dal)
                cmdMyCommand.Parameters.AddWithValue("@AL", Al)
            End If
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(myDataSet, "Create DataView")
            myAdapter.Dispose()

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.getPagamenti.errore: ", ex)
        Finally
            cmdMyCommand.Connection.Close()
        End Try
        Return myDataSet
    End Function
    ''' <summary>
    ''' stampa i pagamenti in base ai parametri
    ''' </summary>
    ''' <param name="sIdEnte">string ente</param>
    ''' <param name="cognome">string cognome</param>
    ''' <param name="nome">string nome</param>
    ''' <param name="cod_fiscale">string codice fiscale</param>
    ''' <param name="partita_iva">string partita iva</param>
    ''' <param name="NAtto">string numero atto</param>
    ''' <param name="Dal">DateTime dalla data pagamento</param>
    ''' <param name="Al">DateTime alla data pagamento </param>
    ''' <returns>DataSet risultato ricerca</returns>
    Function GetStampaPagamenti(ByVal sIdEnte As String, ByVal sCognome As String, ByVal sNome As String, ByVal sCFPIVA As String, NAtto As String, Dal As DateTime, Al As DateTime) As DataTable
        Dim myDataSet As New DataSet
        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetStampaPagamenti", "IDENTE", "COGNOME", "NOME", "CFPIVA", "NATTO", "DAL", "AL")
                    myDataSet = ctx.GetDataSet(sSQL, "TBL", ctx.GetParam("IDENTE", sIdEnte) _
                            , ctx.GetParam("COGNOME", New OPENgovTIA.generalClass.generalFunction().ReplaceCharsForSearch(sCognome) & "%") _
                            , ctx.GetParam("NOME", New OPENgovTIA.generalClass.generalFunction().ReplaceCharsForSearch(sNome) & "%") _
                            , ctx.GetParam("CFPIVA", New OPENgovTIA.generalClass.generalFunction().ReplaceCharsForSearch(sCFPIVA) & "%") _
                            , ctx.GetParam("NATTO", NAtto) _
                            , ctx.GetParam("DAL", New OPENgovTIA.generalClass.generalFunction().FormattaData(Dal, "A")) _
                            , ctx.GetParam("AL", New OPENgovTIA.generalClass.generalFunction().FormattaData(Al, "A"))
                        )
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.GetStampaPagamenti.erroreQuery: ", ex)
                    Return Nothing
                Finally
                    ctx.Dispose()
                End Try
            End Using
            Return myDataSet.Tables(0)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.GetStampaPagamenti.errore: ", ex)
        End Try
    End Function

    '
    ''' <summary>
    ''' Seleziona la provenienza del pagamento
    ''' </summary>
    ''' <param name="cod_ente"></param>
    ''' <returns></returns>
    Function getProvenienza(ByVal cod_ente As String) As DataSet
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim myDataSet As New DataSet
        Try
            sSQL = "SELECT PGM_PROVENIENZA.ID_PROVENIENZA, DESCRIZIONE "
            '*** 20130117 - ottimizzato struttura db per provenienza ***
            sSQL += " FROM PGM_PROVENIENZA"
            sSQL += " INNER JOIN PGM_PROVENIENZA_ENTI ON PGM_PROVENIENZA.ID_PROVENIENZA=PGM_PROVENIENZA_ENTI.ID_PROVENIENZA"
            '*** ***
            If cod_ente <> "" Then
                sSQL += " WHERE COD_ENTE='" & cod_ente & "'"
            End If
            sSQL += " ORDER BY DESCRIZIONE"
            cmdMyCommand = New SqlClient.SqlCommand
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.CommandText = sSQL
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(myDataSet, "Create DataView")
            myAdapter.Dispose()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.getProvenienza.errore: ", ex)
        Finally
            cmdMyCommand.Connection.Close()
        End Try
        Return myDataSet
    End Function
    '
    ''' <summary>
    ''' Seleziona il capitolo del pagamento
    ''' </summary>
    ''' <param name="cod_ente"></param>
    ''' <returns></returns>
    Function getCapitolo(ByVal cod_ente As String) As DataSet
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim myDataSet As New DataSet
        Try
            sSQL = "SELECT ID_TIPO_CAPITOLO, DESCRIZIONE FROM PGM_TIPO_CAPITOLO"
            If cod_ente <> "" Then
                sSQL += " WHERE COD_ENTE='" & cod_ente & "'"
            End If
            sSQL += " ORDER BY DESCRIZIONE"
            cmdMyCommand = New SqlClient.SqlCommand
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.CommandText = sSQL
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(myDataSet, "Create DataView")
            myAdapter.Dispose()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.getCapitolo.errore: ", ex)
        Finally
            cmdMyCommand.Connection.Close()
        End Try
        Return myDataSet
    End Function
    '
    ''' <summary>
    ''' Seleziona la voce del pagamento
    ''' </summary>
    ''' <param name="cod_ente"></param>
    ''' <returns></returns>
    Function getVoce(ByVal cod_ente As String) As DataSet
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim myDataSet As New DataSet
        Try
            sSQL = "SELECT ID_TIPO_VOCE, DESCRIZIONE FROM PGM_TIPO_VOCE"
            If cod_ente <> "" Then
                sSQL += " WHERE COD_ENTE='" & cod_ente & "'"
            End If
            sSQL += " ORDER BY DESCRIZIONE"
            cmdMyCommand = New SqlClient.SqlCommand
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.CommandText = sSQL
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(myDataSet, "Create DataView")
            myAdapter.Dispose()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.getVoce.errore: ", ex)
        Finally
            cmdMyCommand.Connection.Close()
        End Try
        Return myDataSet
    End Function
    '
    ''' <summary>
    ''' Reperimento Max accorpamento
    ''' </summary>
    ''' <returns></returns>
    Function GetMaxAccorpamento() As Integer
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim myDataSet As New DataSet
        Dim id_accorpamento As Integer = -1
        Try
            sSQL = "SELECT MAX(ID_ACCORPAMENTO)+1 AS NEW_ID_ACCORPAMENTO FROM PGM_ACCORPAMENTO"
            cmdMyCommand = New SqlClient.SqlCommand
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.CommandText = sSQL
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(myDataSet, "Create DataView")
            myAdapter.Dispose()
            If Not IsNothing(myDataSet) Then
                If myDataSet.Tables(0).Rows.Count > 0 Then
                    If Not IsDBNull(myDataSet.Tables(0).Rows(0)("new_id_accorpamento")) Then
                        id_accorpamento = myDataSet.Tables(0).Rows(0)("new_id_accorpamento")
                    Else
                        id_accorpamento = 1
                    End If
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.GetMaxAccorpamento.errore: ", ex)
            Return -1
        Finally
            cmdMyCommand.Connection.Close()
        End Try
        Return id_accorpamento
    End Function
    '
    ''' <summary>
    ''' Inserimento in accorpamento
    ''' </summary>
    ''' <param name="cod_contribuente"></param>
    ''' <param name="id_provvedimento"></param>
    ''' <param name="id_accorpamento"></param>
    ''' <param name="Tipo"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Analisi eventi</em>
    ''' </revision>
    ''' </revisionHistory>
    Function setAccorpamento(ByVal cod_contribuente As Integer, ByVal id_provvedimento As Integer, ByVal id_accorpamento As Integer, ByVal Tipo As String) As Integer
        Dim sSQL As String = ""
        Dim myDataView As New DataView
        Try
            Dim oDbManagerRepository As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
            Using ctx As DBModel = oDbManagerRepository
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_PGM_ACCORPAMENTO_IU", "IDACCORPAMENTO", "IDPROVVEDIMENTO", "IDCONTRIBUENTE", "TIPO", "OPERATORE")
                myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDACCORPAMENTO", id_accorpamento) _
                        , ctx.GetParam("IDPROVVEDIMENTO", id_provvedimento) _
                        , ctx.GetParam("IDCONTRIBUENTE", cod_contribuente) _
                        , ctx.GetParam("TIPO", Tipo) _
                        , ctx.GetParam("OPERATORE", ConstSession.UserName)
                    )
                For Each myRow As DataRowView In myDataView
                    id_accorpamento = StringOperation.FormatInt(myRow("id"))
                Next
                ctx.Dispose()
            End Using
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.setAccorpamento.errore: ", ex)
            Throw New Exception(ex.Message, ex)
        Finally
            myDataView.Dispose()
        End Try
        Return id_accorpamento
    End Function
    'Function setAccorpamento(ByVal cod_contribuente As Integer, ByVal id_provvedimento As Integer, ByVal id_accorpamento As Integer, ByVal Tipo As String) As Integer
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim myDataSet As New DataSet
    '    Try
    '        If id_accorpamento <= 0 Then
    '            'id_accorpamento non viene passato da parametro
    '            sSQL = "SELECT MAX(ID_ACCORPAMENTO)+1 AS NEW_ID_ACCORPAMENTO FROM PGM_ACCORPAMENTO"
    '            cmdMyCommand = New SqlClient.SqlCommand
    '            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '                cmdMyCommand.Connection.Open()
    '            End If
    '            cmdMyCommand.CommandTimeout = 0
    '            cmdMyCommand.CommandType = CommandType.Text
    '            cmdMyCommand.CommandText = sSQL
    '            myAdapter.SelectCommand = cmdMyCommand
    '            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '            myAdapter.Fill(myDataSet, "Create DataView")
    '            myAdapter.Dispose()

    '            If Not IsNothing(myDataSet) Then
    '                If myDataSet.Tables(0).Rows.Count > 0 Then
    '                    If Not IsDBNull(myDataSet.Tables(0).Rows(0)("new_id_accorpamento")) Then
    '                        id_accorpamento = myDataSet.Tables(0).Rows(0)("new_id_accorpamento")
    '                    Else
    '                        id_accorpamento = 1
    '                    End If
    '                End If
    '            End If
    '        End If

    '        sSQL = "INSERT INTO PGM_ACCORPAMENTO "
    '        sSQL += "(ID_ACCORPAMENTO,ID_PROVVEDIMENTO,COD_CONTRIBUENTE,TIPO) "
    '        sSQL += " VALUES(" & id_accorpamento & "," & id_provvedimento & "," & cod_contribuente & ",'" & Tipo & "')"
    '        cmdMyCommand = New SqlClient.SqlCommand
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        cmdMyCommand.CommandText = sSQL
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        If cmdMyCommand.ExecuteNonQuery <> 1 Then
    '            Throw New Exception("errore inserimento PGM_ACCORPAMENTO")
    '        End If

    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.setAccorpamento.errore: ", ex)
    '        Throw New Exception(ex.Message, ex)
    '    Finally
    '        cmdMyCommand.Connection.Close()
    '    End Try
    '    Return id_accorpamento
    'End Function
    '
    ''' <summary>
    ''' Inserimento del pagamento.
    ''' Elimino l'eventuale pagamento precedente; inserisco il pagamento con i dati attuali. 
    ''' Procedo alla copertura delle spese e allo scorporo del pagamento sugli atti che compongono l'accorpamento/rateizzo.
    ''' Ricalcolo la % di scorporo per gli interessi che deve essere pari alla % del pagato sul totale da pagare e scorporo gli interessi.
    ''' Verifico e, nel caso, metto l'arrotondamento.
    ''' </summary>
    ''' <param name="id_accorpamento">int id accorpamento</param>
    ''' <param name="listProvv">ArrayList atti che compongono l'accorpamento/rateizzo</param>
    ''' <param name="data_accredito">string data accredito</param>
    ''' <param name="data_pagamento">string data pagamento</param>
    ''' <param name="importo_pagato">string importo pagato</param>
    ''' <param name="provenienza">string provenienza</param>
    ''' <param name="n_rata">string numero rata</param>
    ''' <param name="id_rata_provv">int id rata provvedimento</param>
    ''' <param name="id_rata_acc">int id rata accorpamento</param>
    ''' <param name="id_pagato">int id pagamento</param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="13/03/2019">
    ''' <strong>scorporo pagamenti</strong>
    ''' <list type="bullet">
    ''' <item>
    ''' <em>Spese di notifica non coperte (1° pagamento) </em>
    ''' copro subito per intero le spese di notifica; ricalcolo la % di scorporo al netto delle spese con la formula <code>(((importo_pagato - impSpese) * 100) / (Totale - impSpese))</code>; scorporo il pagamento sugli atti.
    ''' </item>
    ''' <item>
    ''' <em>Spese di notifica coperte </em>
    ''' ricalcolo la % di scorporo con il totale al netto delle spese con la formula <code>((importo_pagato * 100) / (Totale - impSpese))</code>; scorporo il pagamento sugli atti.
    ''' </item>
    ''' </list>
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Analisi eventi</em>
    ''' </revision>
    ''' </revisionHistory>
    Function setPagamento(ByVal id_accorpamento As Integer, ByVal listProvv As ArrayList, ByVal data_accredito As String, ByVal data_pagamento As String, ByVal importo_pagato As String, ByVal provenienza As Integer, ByVal n_rata As String, ByVal id_rata_provv As Integer, ByVal id_rata_acc As Integer, id_pagato As Integer) As Integer
        Dim myDataView As New DataView()
        Dim htHashtable, htValoriProvvedimento As Hashtable
        Dim valore_interesse, valore As Double
        Dim clsGeneralFunction As New MyUtility
        Dim Totale, TotaleDettaglio, nPercentPag, impSpese As Double
        Dim bSpesePagate As Boolean = False
        Dim sSQL As String = ""

        Try
            Dim oDbManagerRepository As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
            Using ctx As DBModel = oDbManagerRepository
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_PagamentoDelete", "IDPAGATO")
                myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDPAGATO", id_pagato))
                For Each myRow As DataRowView In myDataView
                    id_pagato = StringOperation.FormatInt(myRow("id"))
                Next
                id_pagato = 0
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_PagamentoInsert", "IDACCORPAMENTO", "IDRATAPROVV", "IDRATAACC", "DATAACCREDITO", "DATAPAGAMENTO", "IMPORTO", "PROVENIENZA", "NRATA", "OPERATORE")
                myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDACCORPAMENTO", id_accorpamento) _
                        , ctx.GetParam("IDRATAPROVV", id_rata_provv) _
                        , ctx.GetParam("IDRATAACC", id_rata_acc) _
                        , ctx.GetParam("DATAACCREDITO", clsGeneralFunction.GiraData(data_accredito)) _
                        , ctx.GetParam("DATAPAGAMENTO", clsGeneralFunction.GiraData(data_pagamento)) _
                        , ctx.GetParam("IMPORTO", StringOperation.FormatDouble(importo_pagato)) _
                        , ctx.GetParam("PROVENIENZA", provenienza) _
                        , ctx.GetParam("NRATA", n_rata) _
                        , ctx.GetParam("OPERATORE", ConstSession.UserName)
                    )
                For Each myRow As DataRowView In myDataView
                    id_pagato = StringOperation.FormatInt(myRow("id"))
                Next
                If id_pagato > 0 Then
                    'prima di inserire in tabella dettaglio devo spalmare il valore di importo_pagato sui valori importo_spese,del provvedimento/accorpamento selezionato
                    Totale = getTotaleAccorpamento(id_accorpamento)
                    ''calcolo la percentuale di pagamento rispetto al totale accorpamento
                    'nPercentPag = ((importo_pagato * 100) / Totale)
                    'Inserimento dei valori spalmati per ogni provvedimento
                    'If nPercentPag > 0 Then
                    'copro le spese dell'accorpamento
                    Dim HasSpeseCoperte As Boolean = haSpesePagate(id_accorpamento, impSpese)
                    If HasSpeseCoperte = False Then
                        If impSpese < importo_pagato Then
                            'Le spese non sono state pagate
                            If impSpese > 0 Then
                                For Each myItem As Object In listProvv
                                    htValoriProvvedimento = getProvvedimento(CInt(myItem))
                                    htHashtable = setSpalmaImportoProvv(htValoriProvvedimento, 100)

                                    setDettaglioPagamento(id_pagato, htHashtable("IMPORTO_SPESE"), CInt(myItem), enumTIPO_CAPITOLO.PROVVEDIMENTO, enumTIPO_VOCE.SPESE)
                                    'setDettaglioPagamento(id_pagato, impSpese, CInt(myItem), enumTIPO_CAPITOLO.PROVVEDIMENTO, enumTIPO_VOCE.SPESE)
                                Next
                                'tolgo dal totale del provvedimento le spese altrimenti il calcolo non risulta corretto
                                nPercentPag = (((importo_pagato - impSpese) * 100) / (Totale - impSpese))
                            Else
                                nPercentPag = (importo_pagato * 100) / Totale
                            End If
                        Else
                            'Le spese sono maggiori dell'importo pagato
                            setDettaglioPagamento(id_pagato, impSpese, listProvv(0), enumTIPO_CAPITOLO.PROVVEDIMENTO, enumTIPO_VOCE.SPESE)
                            'tolgo dal totale del provvedimento le spese altrimenti il calcolo non risulta corretto
                            nPercentPag = 0
                        End If
                    Else
                        nPercentPag = (importo_pagato * 100) / Totale
                    End If
                    If HasSpeseCoperte Then
                        nPercentPag = (importo_pagato * 100) / (Totale - impSpese)
                    End If
                    'scorporo ed inserisco
                    For Each myItem As Object In listProvv
                        htValoriProvvedimento = getProvvedimento(CInt(myItem))
                        htHashtable = setSpalmaImportoProvv(htValoriProvvedimento, nPercentPag)

                        setDettaglioPagamento(id_pagato, htHashtable("IMPORTO_DIFFERENZA_IMPOSTA"), CInt(myItem), enumTIPO_CAPITOLO.PROVVEDIMENTO, enumTIPO_VOCE.DIFFERENZA_IMPOSTA)
                        setDettaglioPagamento(id_pagato, htHashtable("IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI"), CInt(myItem), enumTIPO_CAPITOLO.PROVVEDIMENTO, enumTIPO_VOCE.SANZIONI_NON_RIDUCIBILI)
                        setDettaglioPagamento(id_pagato, htHashtable("IMPORTO_SANZIONI_RIDOTTO"), CInt(myItem), enumTIPO_CAPITOLO.PROVVEDIMENTO, enumTIPO_VOCE.SANZIONI_RIDOTTE)
                        setDettaglioPagamento(id_pagato, htHashtable("IMPORTO_INTERESSI"), CInt(myItem), enumTIPO_CAPITOLO.PROVVEDIMENTO, enumTIPO_VOCE.INTERESSI)
                        setDettaglioPagamento(id_pagato, htHashtable("IMPORTO_ALTRO"), CInt(myItem), enumTIPO_CAPITOLO.PROVVEDIMENTO, enumTIPO_VOCE.ALTRO)
                        setDettaglioPagamento(id_pagato, htHashtable("IMPORTO_ARROTONDAMENTO_RIDOTTO"), CInt(myItem), enumTIPO_CAPITOLO.PROVVEDIMENTO, enumTIPO_VOCE.ARROTONDAMENTO_PROVVEDIMENTO)
                    Next
                    'End If

                    'Inserimento degli interessi 
                    valore_interesse = 0
                    nPercentPag = (importo_pagato * 100) / Totale
                    Dim myDataSet As New DataSet
                    myDataSet = getInteressiAccorpamento(id_accorpamento, id_rata_acc)
                    If Not IsNothing(myDataSet) Then
                        For Each myRow As DataRow In myDataSet.Tables(0).Rows 'For i = 0 To myDataSet.Tables(0).Rows.Count - 1
                            valore_interesse = myRow("valore_interesse")
                            valore = calcolaValore(valore_interesse, nPercentPag)
                            setDettaglioPagamento(id_pagato, valore, id_rata_acc, enumTIPO_CAPITOLO.INTERESSI, enumTIPO_VOCE.INTERESSI)
                        Next
                    End If

                    'Inserisco arrotondamento sul pagato
                    TotaleDettaglio = GetDettaglioPagamento(id_pagato, listProvv, enumTIPO_CAPITOLO.PROVVEDIMENTO)
                    If TotaleDettaglio > 0 Then
                        TotaleDettaglio = CDbl(importo_pagato) - TotaleDettaglio - valore
                    End If
                    setDettaglioPagamento(id_pagato, TotaleDettaglio, id_accorpamento, enumTIPO_CAPITOLO.ARROTONDAMENTO_PAGATO, enumTIPO_VOCE.ARROTONDAMENTO_PAGATO)
                End If
                ctx.Dispose()
            End Using
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.setPagamento.errore: ", ex)
            Throw New Exception(ex.Message, ex)
        Finally
            myDataView.Dispose()
        End Try
        Return id_accorpamento
    End Function
    'Function setPagamento(ByVal id_accorpamento As Integer, ByVal listProvv As ArrayList, ByVal data_accredito As String, ByVal data_pagamento As String, ByVal importo_pagato As String, ByVal provenienza As Integer, ByVal n_rata As String, ByVal id_rata_provv As Integer, ByVal id_rata_acc As Integer, id_pagato As Integer) As Integer
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim dtMyDati As New DataTable()
    '    Dim htHashtable, htValoriProvvedimento As Hashtable
    '    Dim valore_interesse, valore As Double
    '    Dim clsGeneralFunction As New MyUtility
    '    Dim Totale, TotaleDettaglio, nPercentPag, impSpese As Double
    '    Dim bSpesePagate As Boolean = False

    '    Try
    '        Try
    '            cmdMyCommand = New SqlClient.SqlCommand
    '            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '                cmdMyCommand.Connection.Open()
    '            End If
    '            cmdMyCommand.CommandTimeout = 0
    '            cmdMyCommand.CommandType = CommandType.StoredProcedure
    '            cmdMyCommand.CommandText = "prc_PagamentoDelete"
    '            cmdMyCommand.Parameters.Clear()
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPAGATO", SqlDbType.Int)).Value = id_pagato
    '            myAdapter.SelectCommand = cmdMyCommand
    '            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '            myAdapter.Fill(dtMyDati)
    '            For Each dtMyRow As DataRow In dtMyDati.Rows
    '                id_pagato = dtMyRow("ID")
    '            Next
    '        Catch ex As Exception
    '            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.setPagamento.DelPag.errore: ", ex)
    '            Throw New Exception(ex.Message, ex)
    '        Finally
    '            myAdapter.Dispose()
    '        End Try
    '        id_pagato = 0
    '        Try
    '            cmdMyCommand = New SqlClient.SqlCommand
    '            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '                cmdMyCommand.Connection.Open()
    '            End If
    '            cmdMyCommand.CommandTimeout = 0
    '            cmdMyCommand.CommandType = CommandType.StoredProcedure
    '            cmdMyCommand.CommandText = "prc_PagamentoInsert"
    '            cmdMyCommand.Parameters.Clear()
    '            cmdMyCommand.Parameters.AddWithValue("@IDACCORPAMENTO", id_accorpamento)
    '            cmdMyCommand.Parameters.AddWithValue("@IDRATAPROVV", id_rata_provv)
    '            cmdMyCommand.Parameters.AddWithValue("@IDRATAACC", id_rata_acc)
    '            cmdMyCommand.Parameters.AddWithValue("@DATAACCREDITO", clsGeneralFunction.GiraData(data_accredito))
    '            cmdMyCommand.Parameters.AddWithValue("@DATAPAGAMENTO", clsGeneralFunction.GiraData(data_pagamento))
    '            cmdMyCommand.Parameters.AddWithValue("@IMPORTO", importo_pagato.Replace(".", "").Replace(",", "."))
    '            cmdMyCommand.Parameters.AddWithValue("@PROVENIENZA", provenienza)
    '            cmdMyCommand.Parameters.AddWithValue("@NRATA", n_rata)
    '            myAdapter.SelectCommand = cmdMyCommand
    '            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '            myAdapter.Fill(dtMyDati)
    '            For Each dtMyRow As DataRow In dtMyDati.Rows
    '                id_pagato = dtMyRow("ID")
    '            Next
    '        Catch ex As Exception
    '            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.setPagamento.InsPag.errore: ", ex)
    '            Throw New Exception(ex.Message, ex)
    '        Finally
    '            myAdapter.Dispose()
    '        End Try
    '        If id_pagato > 0 Then
    '            'prima di inserire in tabella dettaglio devo spalmare il valore di importo_pagato sui valori importo_spese,del provvedimento/accorpamento selezionato
    '            Totale = getTotaleAccorpamento(id_accorpamento)
    '            ''calcolo la percentuale di pagamento rispetto al totale accorpamento
    '            'nPercentPag = ((importo_pagato * 100) / Totale)
    '            'Inserimento dei valori spalmati per ogni provvedimento
    '            'If nPercentPag > 0 Then
    '            'copro le spese dell'accorpamento
    '            Dim HasSpeseCoperte As Boolean = haSpesePagate(id_accorpamento, impSpese)
    '            If HasSpeseCoperte = False Then
    '                If impSpese < importo_pagato Then
    '                    'Le spese non sono state pagate
    '                    If impSpese > 0 Then
    '                        For Each myItem As Object In listProvv
    '                            htValoriProvvedimento = getProvvedimento(CInt(myItem))
    '                            htHashtable = setSpalmaImportoProvv(htValoriProvvedimento, 100)

    '                            setDettaglioPagamento(id_pagato, htHashtable("IMPORTO_SPESE"), CInt(myItem), enumTIPO_CAPITOLO.PROVVEDIMENTO, enumTIPO_VOCE.SPESE)
    '                            'setDettaglioPagamento(id_pagato, impSpese, CInt(myItem), enumTIPO_CAPITOLO.PROVVEDIMENTO, enumTIPO_VOCE.SPESE)
    '                        Next
    '                        'tolgo dal totale del provvedimento le spese altrimenti il calcolo non risulta corretto
    '                        nPercentPag = (((importo_pagato - impSpese) * 100) / (Totale - impSpese))
    '                    End If
    '                Else
    '                    'Le spese sono maggiori dell'importo pagato
    '                    setDettaglioPagamento(id_pagato, impSpese, listProvv(0), enumTIPO_CAPITOLO.PROVVEDIMENTO, enumTIPO_VOCE.SPESE)
    '                    'tolgo dal totale del provvedimento le spese altrimenti il calcolo non risulta corretto
    '                    nPercentPag = 0
    '                End If
    '            Else
    '                nPercentPag = (importo_pagato * 100) / Totale
    '            End If
    '            If HasSpeseCoperte Then
    '                nPercentPag = (importo_pagato * 100) / (Totale - impSpese)
    '            End If
    '            'scorporo ed inserisco
    '            For Each myItem As Object In listProvv
    '                htValoriProvvedimento = getProvvedimento(CInt(myItem))
    '                htHashtable = setSpalmaImportoProvv(htValoriProvvedimento, nPercentPag)

    '                setDettaglioPagamento(id_pagato, htHashtable("IMPORTO_DIFFERENZA_IMPOSTA"), CInt(myItem), enumTIPO_CAPITOLO.PROVVEDIMENTO, enumTIPO_VOCE.DIFFERENZA_IMPOSTA)
    '                setDettaglioPagamento(id_pagato, htHashtable("IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI"), CInt(myItem), enumTIPO_CAPITOLO.PROVVEDIMENTO, enumTIPO_VOCE.SANZIONI_NON_RIDUCIBILI)
    '                setDettaglioPagamento(id_pagato, htHashtable("IMPORTO_SANZIONI_RIDOTTO"), CInt(myItem), enumTIPO_CAPITOLO.PROVVEDIMENTO, enumTIPO_VOCE.SANZIONI_RIDOTTE)
    '                setDettaglioPagamento(id_pagato, htHashtable("IMPORTO_INTERESSI"), CInt(myItem), enumTIPO_CAPITOLO.PROVVEDIMENTO, enumTIPO_VOCE.INTERESSI)
    '                setDettaglioPagamento(id_pagato, htHashtable("IMPORTO_ALTRO"), CInt(myItem), enumTIPO_CAPITOLO.PROVVEDIMENTO, enumTIPO_VOCE.ALTRO)
    '                setDettaglioPagamento(id_pagato, htHashtable("IMPORTO_ARROTONDAMENTO_RIDOTTO"), CInt(myItem), enumTIPO_CAPITOLO.PROVVEDIMENTO, enumTIPO_VOCE.ARROTONDAMENTO_PROVVEDIMENTO)
    '            Next
    '            'End If

    '            'Inserimento degli interessi 
    '            valore_interesse = 0
    '            nPercentPag = (importo_pagato * 100) / Totale
    '            Dim myDataSet As New DataSet
    '            myDataSet = getInteressiAccorpamento(id_accorpamento, id_rata_acc)
    '            If Not IsNothing(myDataSet) Then
    '                For Each myRow As DataRow In myDataSet.Tables(0).Rows 'For i = 0 To myDataSet.Tables(0).Rows.Count - 1
    '                    valore_interesse = myRow("valore_interesse")
    '                    valore = calcolaValore(valore_interesse, nPercentPag)
    '                    setDettaglioPagamento(id_pagato, valore, id_rata_acc, enumTIPO_CAPITOLO.INTERESSI, enumTIPO_VOCE.INTERESSI)
    '                Next
    '            End If

    '            'Inserisco arrotondamento sul pagato
    '            TotaleDettaglio = GetDettaglioPagamento(id_pagato, listProvv, enumTIPO_CAPITOLO.PROVVEDIMENTO)
    '            If TotaleDettaglio > 0 Then
    '                TotaleDettaglio = CDbl(importo_pagato) - TotaleDettaglio
    '            End If
    '            setDettaglioPagamento(id_pagato, TotaleDettaglio, id_accorpamento, enumTIPO_CAPITOLO.ARROTONDAMENTO_PAGATO, enumTIPO_VOCE.ARROTONDAMENTO_PAGATO)
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.setPagamento.errore: ", ex)
    '        Throw New Exception(ex.Message, ex)
    '    Finally
    '        cmdMyCommand.Connection.Close()
    '    End Try
    '    Return id_accorpamento
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="IdPagamento"></param>
    ''' <param name="IdRataAcc"></param>
    ''' <param name="IdRataProv"></param>
    ''' <returns></returns>
    Function GetIdRata(ByVal IdPagamento As Integer, ByRef IdRataAcc As Integer, ByRef IdRataProv As Integer) As Boolean
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dtMyDati As New DataTable()

        Try
            cmdMyCommand = New SqlClient.SqlCommand
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_GetPagamentoIdRata"
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPAGATO", SqlDbType.Int)).Value = IdPagamento
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(dtMyDati)
            For Each dtMyRow As DataRow In dtMyDati.Rows
                IdRataAcc = dtMyRow("ID_RATA_ACC")
                IdRataProv = dtMyRow("ID_RATA_PROVV")
            Next
            Return True
        Catch ex As Exception
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.GetIdRata.errore: ", ex)
            Return False
        Finally
            myAdapter.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function

    '
    ''' <summary>
    ''' Spalmo il pagamento sul singolo provvedimento
    ''' </summary>
    ''' <param name="htValoriProvvedimento"></param>
    ''' <param name="nPercentuale"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/03/2019">
    ''' <strong>Spese su provvedimento singolo</strong>
    ''' devo analizzare le spese per ogni singolo provvedimento dell'accorpamento e non totali
    ''' </revision>
    ''' </revisionHistory>
    Function setSpalmaImportoProvv(ByVal htValoriProvvedimento As Hashtable, ByVal nPercentuale As Double) As Hashtable
        Dim htHashtable As Hashtable
        Dim valore As Double

        Try
            htHashtable = New Hashtable
            If nPercentuale > 0 Then
                valore = calcolaValore(htValoriProvvedimento("IMPORTO_DIFFERENZA_IMPOSTA"), nPercentuale)
                htHashtable.Add("IMPORTO_DIFFERENZA_IMPOSTA", valore)

                valore = calcolaValore(htValoriProvvedimento("IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI"), nPercentuale)
                htHashtable.Add("IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI", valore)

                valore = calcolaValore(htValoriProvvedimento("IMPORTO_SANZIONI_RIDOTTO"), nPercentuale)
                htHashtable.Add("IMPORTO_SANZIONI_RIDOTTO", valore)

                valore = calcolaValore(htValoriProvvedimento("IMPORTO_INTERESSI"), nPercentuale)
                htHashtable.Add("IMPORTO_INTERESSI", valore)

                valore = calcolaValore(htValoriProvvedimento("IMPORTO_ALTRO"), nPercentuale)
                htHashtable.Add("IMPORTO_ALTRO", valore)

                valore = calcolaValore(htValoriProvvedimento("IMPORTO_ARROTONDAMENTO_RIDOTTO"), nPercentuale)
                htHashtable.Add("IMPORTO_ARROTONDAMENTO_RIDOTTO", valore)

                valore = calcolaValore(htValoriProvvedimento("IMPORTO_SPESE"), nPercentuale)
                htHashtable.Add("IMPORTO_SPESE", valore)
            End If
            Return htHashtable
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.setSpalmaImportoProvv.errore: ", ex)
            Throw New Exception(ex.Message, ex)
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="dvalore"></param>
    ''' <param name="nPercentuale"></param>
    ''' <returns></returns>
    Function calcolaValore(ByVal dvalore As Object, ByVal nPercentuale As Double) As Decimal
        Dim valore As Decimal
        Try
            valore = 0
            If Not IsDBNull(dvalore) Then
                valore = ((dvalore * nPercentuale) / 100)
            End If
            Return valore
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.calcolaValore.errore: ", ex)
            Return -1
        End Try
    End Function
    '
    ''' <summary>
    ''' Inserimento del dettaglio del pagamento
    ''' </summary>
    ''' <param name="id_pagato"></param>
    ''' <param name="importo"></param>
    ''' <param name="id_capitolo"></param>
    ''' <param name="id_tipo_capitolo"></param>
    ''' <param name="id_tipo_voce"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Sub setDettaglioPagamento(ByVal id_pagato As Integer, ByVal importo As Decimal, ByVal id_capitolo As Integer, ByVal id_tipo_capitolo As enumTIPO_CAPITOLO, ByVal id_tipo_voce As enumTIPO_VOCE)
        Dim myDataView As New DataView
        Try
            If importo <> 0 Then
                Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_PagamentoInsDettaglio", "IDPAGATO", "IDCAPITOLO", "IDTIPOCAPITOLO", "IDTIPOVOCE", "IMPORTO")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDPAGATO", id_pagato) _
                            , ctx.GetParam("IDCAPITOLO", id_capitolo) _
                            , ctx.GetParam("IDTIPOCAPITOLO", id_tipo_capitolo) _
                            , ctx.GetParam("IDTIPOVOCE", id_tipo_voce) _
                            , ctx.GetParam("IMPORTO", importo)
                        )
                    For Each myRow As DataRowView In myDataView
                        id_pagato = StringOperation.FormatInt(myRow("id"))
                    Next
                    ctx.Dispose()
                End Using
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.setDettaglioPagamento.errore: ", ex)
            Throw New Exception(ex.Message, ex)
        Finally
            myDataView.Dispose()
        End Try
    End Sub
    'Function setDettaglioPagamento(ByVal id_pagato As Integer, ByVal importo As Decimal, ByVal id_capitolo As Integer, ByVal id_tipo_capitolo As enumTIPO_CAPITOLO, ByVal id_tipo_voce As enumTIPO_VOCE) As Integer
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim dtMyDati As New DataTable()
    '    Try
    '        If importo <> 0 Then
    '            Try
    '                'importo = FormatNumber(importo, 2)
    '                'sSQL = "INSERT INTO PGM_PAGATO_DETTAGLIO "
    '                'sSQL += "(ID_PAGATO, ID_CAPITOLO, ID_TIPO_CAPITOLO, ID_TIPO_VOCE, IMPORTO) "
    '                'sSQL += " VALUES ("
    '                'sSQL += "" & id_pagato & ","
    '                'sSQL += "" & id_capitolo & ","
    '                'sSQL += "" & id_tipo_capitolo & ","
    '                'sSQL += "" & id_tipo_voce & ","
    '                'sSQL += "" & importo.ToString.Replace(".", "").Replace(",", ".") & ")"
    '                'cmdMyCommand = New SqlClient.SqlCommand
    '                'cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '                'If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '                '    cmdMyCommand.Connection.Open()
    '                'End If
    '                'cmdMyCommand.CommandTimeout = 0
    '                'cmdMyCommand.CommandType = CommandType.Text
    '                'cmdMyCommand.CommandText = sSQL
    '                'If cmdMyCommand.ExecuteNonQuery <> 1 Then
    '                '    Throw New Exception("Errore inserimento PGM_PAGATO_DETTAGLIO")
    '                cmdMyCommand = New SqlClient.SqlCommand
    '                cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '                If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '                    cmdMyCommand.Connection.Open()
    '                End If
    '                cmdMyCommand.CommandTimeout = 0
    '                cmdMyCommand.CommandType = CommandType.StoredProcedure
    '                cmdMyCommand.CommandText = "prc_PagamentoInsDettaglio"
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.AddWithValue("@IDPAGATO", id_pagato)
    '                cmdMyCommand.Parameters.AddWithValue("@IDCAPITOLO", id_capitolo)
    '                cmdMyCommand.Parameters.AddWithValue("@IDTIPOCAPITOLO", id_tipo_capitolo)
    '                cmdMyCommand.Parameters.AddWithValue("@IDTIPOVOCE", id_tipo_voce)
    '                cmdMyCommand.Parameters.AddWithValue("@IMPORTO", importo.ToString.Replace(".", "").Replace(",", "."))
    '                myAdapter.SelectCommand = cmdMyCommand
    '                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '                myAdapter.Fill(dtMyDati)
    '                For Each dtMyRow As DataRow In dtMyDati.Rows
    '                    id_pagato = dtMyRow("ID")
    '                Next
    '            Catch ex As Exception
    '                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.setDettaglioPagamento.errore: ", ex)
    '                Throw New Exception(ex.Message, ex)
    '            Finally
    '                myAdapter.Dispose()
    '                cmdMyCommand.Connection.Close()
    '            End Try
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.setDettaglioPagamento.errore: ", ex)
    '        Throw New Exception(ex.Message, ex)
    '    End Try
    'End Function

    '
    ''' <summary>
    ''' Inserimento del dettaglio del pagamento
    ''' </summary>
    ''' <param name="id_pagato"></param>
    ''' <param name="listProvv"></param>
    ''' <param name="id_tipo_capitolo"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Function GetDettaglioPagamento(ByVal id_pagato As Integer, ByVal listProvv As ArrayList, ByVal id_tipo_capitolo As enumTIPO_CAPITOLO) As Double
        Dim myDataView As New DataView
        Dim Totale As Double = 0
        Dim sid_Capitolo As String = ""
        Try
            For Each myItem As String In listProvv
                If sid_Capitolo <> "" Then
                    sid_Capitolo += ","
                End If
                sid_Capitolo += myItem
            Next

            Dim oDbManagerRepository As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
            Using ctx As DBModel = oDbManagerRepository
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_GetDettaglioPagamento", "IDPAGATO", "TIPOCAPITOLO", "LISTPROVV")
                myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDPAGATO", id_pagato) _
                        , ctx.GetParam("TIPOCAPITOLO", id_tipo_capitolo) _
                        , ctx.GetParam("LISTPROVV", sid_Capitolo)
                    )
                For Each myRow As DataRowView In myDataView
                    Totale = StringOperation.FormatDouble(myRow("Totale"))
                Next
                ctx.Dispose()
            End Using

            Return Totale
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.GetDettaglioPagamento.errore:  ", ex)
            Throw New Exception(ex.Message, ex)
        Finally
            myDataView.Dispose()
        End Try
    End Function
    'Function GetDettaglioPagamento(ByVal id_pagato As Integer, ByVal listProvv As ArrayList, ByVal id_tipo_capitolo As enumTIPO_CAPITOLO) As Double
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim myDataSet As New DataSet
    '    Dim Totale As Double = 0
    '    Dim i As Integer
    '    Dim sid_Capitolo As String = ""
    '    Try

    '        sSQL = "SELECT SUM(IMPORTO) AS TOTALE FROM PGM_PAGATO_DETTAGLIO"
    '        sSQL += " WHERE ID_PAGATO=" & id_pagato
    '        sSQL += " AND ID_TIPO_CAPITOLO=" & id_tipo_capitolo
    '        For i = 0 To listProvv.Count - 1
    '            sid_Capitolo += listProvv(i) & ","
    '        Next
    '        sid_Capitolo = Mid(sid_Capitolo, 1, sid_Capitolo.Length - 1)
    '        sSQL += " AND ID_CAPITOLO IN (" & sid_Capitolo & ")"
    '        cmdMyCommand = New SqlClient.SqlCommand
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        cmdMyCommand.CommandText = sSQL
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(myDataSet, "Create DataView")
    '        myAdapter.Dispose()

    '        If Not IsNothing(myDataSet) Then
    '            If myDataSet.Tables(0).Rows.Count > 0 Then
    '                Totale = myDataSet.Tables(0).Rows(0)("Totale")
    '            End If
    '        End If
    '        Return Totale
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.GetDettaglioPagamento.errore: ", ex)
    '        Throw New Exception(ex.Message, ex)
    '    Finally
    '        cmdMyCommand.Connection.Close()
    '    End Try
    'End Function

    '
    ''' <summary>
    ''' Inserimento Rate Accorpamento
    ''' </summary>
    ''' <param name="id_accorpamento"></param>
    ''' <param name="n_rata"></param>
    ''' <param name="data_scadenza"></param>
    ''' <param name="valore_rata"></param>
    ''' <param name="valore_interesse"></param>
    ''' <param name="importo_totale_rata"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Analisi eventi</em>
    ''' </revision>
    ''' </revisionHistory>
    Function setRateAccorpamento(ByVal id_accorpamento As Integer, ByVal n_rata As Integer, ByVal data_scadenza As String, ByVal valore_rata As String, ByVal valore_interesse As String, ByVal importo_totale_rata As String) As Integer
        Dim myDataView As New DataView
        Dim id_rata_acc As Integer = 0
        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_PGM_RATA_ACCORPAMENTO_IU", "IDACCORPAMENTO", "NRATA", "DATASCADENZA", "VALORERATA", "VALOREINTERESSE", "IMPORTOTOTALERATA", "OPERATORE")
                myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDACCORPAMENTO", id_accorpamento) _
                        , ctx.GetParam("NRATA", n_rata) _
                        , ctx.GetParam("DATASCADENZA", data_scadenza) _
                        , ctx.GetParam("VALORERATA", StringOperation.FormatDouble(valore_rata)) _
                        , ctx.GetParam("VALOREINTERESSE", StringOperation.FormatDouble(valore_interesse)) _
                        , ctx.GetParam("IMPORTOTOTALERATA", StringOperation.FormatDouble(importo_totale_rata)) _
                        , ctx.GetParam("OPERATORE", ConstSession.UserName)
                    )
                For Each myRow As DataRowView In myDataView
                    id_rata_acc = StringOperation.FormatInt(myRow("id"))
                Next
                ctx.Dispose()
            End Using
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.setRateAccorpamento.errore: ", ex)
            Throw New Exception(ex.Message, ex)
        Finally
            myDataView.Dispose()
        End Try
        Return id_rata_acc
    End Function
    'Function setRateAccorpamento(ByVal id_accorpamento As Integer, ByVal n_rata As Integer, ByVal data_scadenza As String, ByVal valore_rata As String, ByVal valore_interesse As String, ByVal importo_totale_rata As String) As Integer
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim myDataSet As New DataSet
    '    Dim id_rata_acc As Integer = 0
    '    Try
    '        sSQL = "INSERT INTO PGM_RATA_ACCORPAMENTO "
    '        sSQL += "(ID_ACCORPAMENTO, N_RATA, DATA_SCADENZA, VALORE_RATA, VALORE_INTERESSE, IMPORTO_TOTALE_RATA) "
    '        sSQL += " VALUES ("
    '        sSQL += "" & id_accorpamento & ","
    '        sSQL += "" & n_rata & ","
    '        sSQL += "'" & data_scadenza & "',"
    '        sSQL += "" & valore_rata.Replace(".", "").Replace(",", ".") & ","
    '        sSQL += "" & valore_interesse.Replace(".", "").Replace(",", ".") & ","
    '        sSQL += "" & importo_totale_rata.Replace(".", "").Replace(",", ".") & ")"
    '        sSQL += " SELECT @@IDENTITY"
    '        cmdMyCommand = New SqlClient.SqlCommand
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        cmdMyCommand.CommandText = sSQL
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(myDataSet, "Create DataView")
    '        myAdapter.Dispose()

    '        If Not IsNothing(myDataSet) Then
    '            If myDataSet.Tables(0).Rows.Count > 0 Then
    '                id_rata_acc = myDataSet.Tables(0).Rows(0)(0)
    '            End If
    '        Else
    '            Throw New Exception("Errore inserimento PGN_RATA_ACCORPAMENTO")
    '        End If

    '        Return id_rata_acc
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.setRateAccorpamento.errore: ", ex)
    '        Throw New Exception(ex.Message, ex)
    '        Return id_rata_acc
    '    Finally
    '        cmdMyCommand.Connection.Close()
    '    End Try
    'End Function

    '
    ''' <summary>
    ''' Inserimento Rate Provvedimento
    ''' </summary>
    ''' <param name="id_provvedimento"></param>
    ''' <param name="n_rata"></param>
    ''' <param name="data_scadenza"></param>
    ''' <param name="valore_rata"></param>
    ''' <param name="valore_interesse"></param>
    ''' <param name="importo_totale_rata"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Function setRateProvvedimento(ByVal id_provvedimento As Integer, ByVal n_rata As Integer, ByVal data_scadenza As String, ByVal valore_rata As String, ByVal valore_interesse As String, ByVal importo_totale_rata As String) As Integer
        Dim myDataView As New DataView
        Dim id_rata_acc As Integer = 0
        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_PGM_RATA_PROVVEDIMENTO_IU", "IDPROVVEDIMENTO", "NRATA", "DATASCADENZA", "VALORERATA", "VALOREINTERESSE", "IMPORTOTOTALERATA")
                myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDPROVVEDIMENTO", id_provvedimento) _
                        , ctx.GetParam("NRATA", n_rata) _
                        , ctx.GetParam("DATASCADENZA", data_scadenza) _
                        , ctx.GetParam("VALORERATA", StringOperation.FormatDouble(valore_rata)) _
                        , ctx.GetParam("VALOREINTERESSE", StringOperation.FormatDouble(valore_interesse)) _
                        , ctx.GetParam("IMPORTOTOTALERATA", StringOperation.FormatDouble(importo_totale_rata))
                    )
                For Each myRow As DataRowView In myDataView
                    id_rata_acc = StringOperation.FormatInt(myRow("id"))
                Next
                ctx.Dispose()
            End Using
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.setRateProvvedimento.errore: ", ex)
            Throw New Exception(ex.Message, ex)
        Finally
            myDataView.Dispose()
        End Try
        Return id_rata_acc
    End Function
    'Function setRateProvvedimento(ByVal id_provvedimento As Integer, ByVal n_rata As Integer, ByVal data_scadenza As String, ByVal valore_rata As String, ByVal valore_interesse As String, ByVal importo_totale_rata As String) As Integer
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim myDataSet As New DataSet
    '    Dim id_rata_acc As Integer = 0
    '    Try
    '        sSQL = "INSERT INTO PGM_RATA_PROVVEDIMENTO "
    '        sSQL += "(ID_PROVVEDIMENTO, N_RATA, DATA_SCADENZA, VALORE_RATA, VALORE_INTERESSE,IMPORTO_TOTALE_RATA) "
    '        sSQL += " VALUES ("
    '        sSQL += "" & id_provvedimento & ","
    '        sSQL += "" & n_rata & ","
    '        sSQL += "'" & data_scadenza & "',"
    '        sSQL += "" & valore_rata.Replace(".", "").Replace(",", ".") & ","
    '        sSQL += "" & valore_interesse.Replace(".", "").Replace(",", ".") & ","
    '        sSQL += "" & importo_totale_rata.Replace(".", "").Replace(",", ".") & ")"
    '        sSQL += " SELECT @@IDENTITY"
    '        cmdMyCommand = New SqlClient.SqlCommand
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        cmdMyCommand.CommandText = sSQL
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(myDataSet, "Create DataView")
    '        myAdapter.Dispose()

    '        If Not IsNothing(myDataSet) Then
    '            If myDataSet.Tables(0).Rows.Count > 0 Then
    '                id_rata_acc = myDataSet.Tables(0).Rows(0)(0)
    '            End If
    '        Else
    '            Throw New Exception("Errore inserimento PGM_RATA_PROVVEDIMENTO")
    '        End If

    '        Return id_rata_acc

    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.setRateProvvedimento.errore: ", ex)
    '        Throw New Exception(ex.Message, ex)
    '        Return id_rata_acc
    '    Finally
    '        cmdMyCommand.Connection.Close()
    '    End Try
    'End Function

    '
    ''' <summary>
    ''' Cancellazione di una rata pagata e del relativo dettaglio
    ''' </summary>
    ''' <param name="id_pagato"></param>
    ''' <param name="id_rata_acc"></param>
    ''' <param name="id_rata_provv"></param>
    ''' <returns></returns>
    Function deleteRata(ByVal id_pagato As Integer, ByVal id_rata_acc As Integer, ByVal id_rata_provv As Integer) As Boolean
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim iDelete As Integer
        Try
            sSQL = "DELETE FROM PGM_PAGATO "
            sSQL += "WHERE ID_PAGATO= " & id_pagato
            cmdMyCommand = New SqlClient.SqlCommand
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.CommandText = sSQL
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            iDelete = cmdMyCommand.ExecuteNonQuery
            Log.Debug("PGM_PAGATO Cancellata rata " & id_pagato & " utente:" & ConstSession.UserName)

            sSQL = "DELETE FROM PGM_PAGATO_DETTAGLIO "
            sSQL += "WHERE ID_PAGATO= " & id_pagato
            cmdMyCommand = New SqlClient.SqlCommand
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.CommandText = sSQL
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            iDelete = cmdMyCommand.ExecuteNonQuery
            Log.Debug("PGM_PAGATO_DETTAGLIO Cancellata rata " & id_pagato & " utente:" & ConstSession.UserName)

            If id_rata_acc > 0 Then
                sSQL = "DELETE FROM PGM_RATA_ACCORPAMENTO "
                sSQL += "WHERE ID_RATA_ACC= " & id_rata_acc
                cmdMyCommand = New SqlClient.SqlCommand
                cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
                If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                    cmdMyCommand.Connection.Open()
                End If
                cmdMyCommand.CommandTimeout = 0
                cmdMyCommand.CommandType = CommandType.Text
                cmdMyCommand.CommandText = sSQL
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                iDelete = cmdMyCommand.ExecuteNonQuery
                Log.Debug("PGM_RATA_ACCORPAMENTO Cancellata rata " & id_pagato & " utente:" & ConstSession.UserName)
            End If

            If id_rata_provv > 0 Then
                sSQL = "DELETE FROM PGM_RATA_PROVVEDIMENTO "
                sSQL += "WHERE ID_RATA_PROVV= " & id_rata_provv
                cmdMyCommand = New SqlClient.SqlCommand
                cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
                If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                    cmdMyCommand.Connection.Open()
                End If
                cmdMyCommand.CommandTimeout = 0
                cmdMyCommand.CommandType = CommandType.Text
                cmdMyCommand.CommandText = sSQL
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                iDelete = cmdMyCommand.ExecuteNonQuery
                Log.Debug("PGM_RATA_PROVVEDIMENTO Cancellata rata " & id_pagato & " utente:" & ConstSession.UserName)
            End If

            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.deleteRata.errore: ", ex)
            Return False
        Finally
            cmdMyCommand.Connection.Close()
        End Try
    End Function

    '
    ''' <summary>
    ''' Cancellazione di tutti i dati relativi ad un accorpamento
    ''' </summary>
    ''' <param name="id_accorpamento"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Analisi eventi</em>
    ''' </revision>
    ''' </revisionHistory>
    Function deleteAccorpamento(ByVal id_accorpamento As Integer) As Boolean
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Try
            Log.Debug("Cancellazione accorpamento " & id_accorpamento & " utente:" & ConstSession.UserName)

            sSQL = "UPDATE PGM_RATA_ACCORPAMENTO SET DATA_VARIAZIONE=GETDATE()"
            sSQL += " WHERE ID_ACCORPAMENTO=@IDACCORPAMENTO"
            cmdMyCommand = New SqlClient.SqlCommand
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.CommandText = sSQL
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.AddWithValue("@IDACCORPAMENTO", id_accorpamento)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            cmdMyCommand.ExecuteNonQuery()
            Log.Debug("PGM_RATA_ACCORPAMENTO Cancellato Accorpamento " & id_accorpamento)

            sSQL = "UPDATE PGM_ACCORPAMENTO SET DATA_VARIAZIONE=GETDATE()"
            sSQL += " WHERE ID_ACCORPAMENTO=@IDACCORPAMENTO"
            cmdMyCommand = New SqlClient.SqlCommand
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.CommandText = sSQL
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.AddWithValue("@IDACCORPAMENTO", id_accorpamento)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            cmdMyCommand.ExecuteNonQuery()
            Log.Debug("PGM_ACCORPAMENTO Cancellato Accorpamento " & id_accorpamento)

            Log.Debug("Cancellazione accorpamento Terminata")
            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.deleteAccorpamento.errore: ", ex)
            Return False
        Finally
            cmdMyCommand.Connection.Close()
        End Try
    End Function
    'Function deleteAccorpamento(ByVal id_accorpamento As Integer) As Boolean
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim myDataSet As New DataSet
    '    Dim i, iDelete As Integer
    '    Dim id_provvedimento As Integer
    '    Try
    '        Log.Debug("Cancellazione accorpamento " & id_accorpamento & " utente:" & ConstSession.UserName)

    '        sSQL = "SELECT * FROM PGM_ACCORPAMENTO WHERE ID_ACCORPAMENTO=" & id_accorpamento
    '        cmdMyCommand = New SqlClient.SqlCommand
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        cmdMyCommand.CommandText = sSQL
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(myDataSet, "Create DataView")
    '        myAdapter.Dispose()

    '        If Not IsNothing(myDataSet) Then
    '            If myDataSet.Tables(0).Rows.Count > 0 Then
    '                For i = 0 To myDataSet.Tables(0).Rows.Count - 1
    '                    id_provvedimento = myDataSet.Tables(0).Rows(i)("id_provvedimento")
    '                    sSQL = "DELETE FROM PGM_RATA_PROVVEDIMENTO WHERE ID_PROVVEDIMENTO=" & id_provvedimento
    '                    cmdMyCommand = New SqlClient.SqlCommand
    '                    cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '                    If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '                        cmdMyCommand.Connection.Open()
    '                    End If
    '                    cmdMyCommand.CommandTimeout = 0
    '                    cmdMyCommand.CommandType = CommandType.Text
    '                    cmdMyCommand.CommandText = sSQL
    '                    Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '                    iDelete = cmdMyCommand.ExecuteNonQuery
    '                    Log.Debug("Cancellazione Rata Provvedimento " & id_provvedimento)
    '                Next
    '            End If
    '        End If

    '        sSQL = "DELETE FROM PGM_RATA_ACCORPAMENTO "
    '        sSQL += "WHERE ID_ACCORPAMENTO= " & id_accorpamento
    '        cmdMyCommand = New SqlClient.SqlCommand
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        cmdMyCommand.CommandText = sSQL
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        iDelete = cmdMyCommand.ExecuteNonQuery
    '        Log.Debug("PGM_RATA_ACCORPAMENTO Cancellato Accorpamento " & id_accorpamento)

    '        sSQL = "DELETE FROM PGM_ACCORPAMENTO "
    '        sSQL += "WHERE ID_ACCORPAMENTO= " & id_accorpamento
    '        cmdMyCommand = New SqlClient.SqlCommand
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '            cmdMyCommand.Connection.Open()
    '        End If
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.Text
    '        cmdMyCommand.CommandText = sSQL
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        iDelete = cmdMyCommand.ExecuteNonQuery
    '        Log.Debug("PGM_ACCORPAMENTO Cancellato Accorpamento " & id_accorpamento)

    '        Log.Debug("Cancellazione accorpamento Terminata")
    '        Return True
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.clsPagamenti.deleteAccorpamento.errore: ", ex)
    '        Return False
    '    Finally
    '        cmdMyCommand.Connection.Close()
    '    End Try
    'End Function
    Public Function GetExportRate(IdEnte As String) As DataView
        Dim dvMyDati As New DataView
        Dim sSQL As String = ""
        Try
            'prelevo i dati
            Dim oDbManagerRepository As New DBModel(ConstSession.DBType, strConnectionStringOPENgovProvvedimenti)
            Using ctx As DBModel = oDbManagerRepository
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetExportRate", "IDENTE")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", IdEnte))
                ctx.Dispose()
            End Using
            Return dvMyDati
        Catch Err As Exception
            Log.Debug(IdEnte + " - OPENgovPROVVEDIMENTI.ClsPagamenti.GetExportRate.errore: ", Err)
            Return Nothing
        End Try
    End Function
    Public Function GetExportPagamenti(IdEnte As String) As DataView
        Dim dvMyDati As New DataView
        Dim sSQL As String = ""
        Try
            'prelevo i dati
            Dim oDbManagerRepository As New DBModel(ConstSession.DBType, strConnectionStringOPENgovProvvedimenti)
            Using ctx As DBModel = oDbManagerRepository
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetExportPagamenti", "IDENTE")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", IdEnte))
                ctx.Dispose()
            End Using
            Return dvMyDati
        Catch Err As Exception
            Log.Debug(IdEnte + " - OPENgovPROVVEDIMENTI.ClsPagamenti.GetExportPagamenti.errore: ", Err)
            Return Nothing
        End Try
    End Function
End Class

