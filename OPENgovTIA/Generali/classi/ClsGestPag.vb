'*** 20120921 - pagamenti ***
Imports RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti
Imports log4net
Imports Utility
''' <summary>
''' Definizione oggetto ricerca pagamenti
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class ObjSearchPagamenti
    Private _sEnte As String = ""
    Private _IdTributo As String = ""
    Private _sCognome As String = ""
    Private _sNome As String = ""
    Private _sCFPIVA As String = ""
    Private _sAnnoRif As String = ""
    Private _sNAvviso As String = ""
    Private _IdContribuente As Integer = -1
    Private _nTipoStampa As Integer = 0 '{0=pagamenti;1=emesso non pagato;2=emesso pagato parzialmente;3=emesso pagato in eccesso}
    Private _IdPag As Integer = -1
    Private _tDataPagamentoDAl As Date = DateTime.MaxValue
    Private _tDataPagamentoAl As Date = DateTime.MaxValue
    Private _tDataAccreditoDAl As Date = DateTime.MaxValue
    Private _tDataAccreditoAl As Date = DateTime.MaxValue
    Private _bRicPag As Boolean = True
    Private _IsNonAbb As Boolean = False
    Private _codbollettino As String = ""
    Private _imppagato As Double = 0
    Private _flusso As String = ""
    Private _Provenienza As String = ""

    Public Property sEnte() As String
        Get
            Return _sEnte
        End Get
        Set(ByVal Value As String)
            _sEnte = Value
        End Set
    End Property
    Public Property IdTributo() As String
        Get
            Return _IdTributo
        End Get
        Set(ByVal Value As String)
            _IdTributo = Value
        End Set
    End Property
    Public Property sCognome() As String
        Get
            Return _sCognome
        End Get
        Set(ByVal Value As String)
            _sCognome = Value
        End Set
    End Property
    Public Property sNome() As String
        Get
            Return _sNome
        End Get
        Set(ByVal Value As String)
            _sNome = Value
        End Set
    End Property
    Public Property sCFPIVA() As String
        Get
            Return _sCFPIVA
        End Get
        Set(ByVal Value As String)
            _sCFPIVA = Value
        End Set
    End Property
    Public Property sAnnoRif() As String
        Get
            Return _sAnnoRif
        End Get
        Set(ByVal Value As String)
            _sAnnoRif = Value
        End Set
    End Property
    Public Property sNAvviso() As String
        Get
            Return _sNAvviso
        End Get
        Set(ByVal Value As String)
            _sNAvviso = Value
        End Set
    End Property
    Public Property IdContribuente() As String
        Get
            Return _IdContribuente
        End Get
        Set(ByVal Value As String)
            _IdContribuente = Value
        End Set
    End Property
    Public Property IdPagamento() As Integer
        Get
            Return _IdPag
        End Get
        Set(ByVal Value As Integer)
            _IdPag = Value
        End Set
    End Property
    Public Property tDataPagamentoDal() As Date
        Get
            Return _tDataPagamentoDAl
        End Get
        Set(ByVal Value As Date)
            _tDataPagamentoDAl = Value
        End Set
    End Property
    Public Property tDataPagamentoAl() As Date
        Get
            Return _tDataPagamentoAl
        End Get
        Set(ByVal Value As Date)
            _tDataPagamentoAl = Value
        End Set
    End Property
    Public Property tDataAccreditoDal() As Date
        Get
            Return _tDataAccreditoDAl
        End Get
        Set(ByVal Value As Date)
            _tDataAccreditoDAl = Value
        End Set
    End Property
    Public Property tDataAccreditoAl() As Date
        Get
            Return _tDataAccreditoAl
        End Get
        Set(ByVal Value As Date)
            _tDataAccreditoAl = Value
        End Set
    End Property
    Public Property nTipoStampa() As Integer
        Get
            Return _nTipoStampa
        End Get
        Set(ByVal value As Integer)
            _nTipoStampa = value
        End Set
    End Property
    Public Property bRicPag() As Boolean
        Get
            Return _bRicPag
        End Get
        Set(ByVal value As Boolean)
            _bRicPag = value
        End Set
    End Property
    Public Property IsNonAbbinati() As Boolean
        Get
            Return _IsNonAbb
        End Get
        Set(ByVal value As Boolean)
            _IsNonAbb = value
        End Set
    End Property
    Public Property sCodBollettino() As String
        Get
            Return _codbollettino
        End Get
        Set(ByVal Value As String)
            _codbollettino = Value
        End Set
    End Property
    Public Property impPagato() As Double
        Get
            Return _imppagato
        End Get
        Set(ByVal Value As Double)
            _imppagato = Value
        End Set
    End Property
    Public Property Flusso() As String
        Get
            Return _flusso
        End Get
        Set(ByVal Value As String)
            _flusso = Value
        End Set
    End Property
    Public Property Provenienza() As String
        Get
            Return _Provenienza
        End Get
        Set(value As String)
            _Provenienza = value
        End Set
    End Property
End Class
''' <summary>
''' Classe di gestione pagamenti
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class ClsGestPag
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ClsGestPag))
    Private cmdMyCommand As New SqlClient.SqlCommand
    Private FncGen As New generalClass.generalFunction
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Esportazione completa dati</em>
    ''' </revision>
    ''' </revisionHistory>
#Region "TipoStampa"
    Public Const TipoStampaPagamenti As Integer = 0
    Public Const TipoStampaNonPagato As Integer = 1
    Public Const TipoStampaPagatoParziale As Integer = 2
    Public Const TipoStampaPagatoInEccesso As Integer = 3
#End Region

#Region "DATA ENTRY"
    'Public Function GetCartellePagamenti(ByVal sIdEnte As String, ByVal nIdContribuente As Integer, ByVal sCodCartella As String, ByVal sAnno As String, ByVal WFSessione As OPENUtility.CreateSessione) As OggettoPagamenti()
    '    Dim DrDati As SqlClient.SqlDataReader
    '    Dim oMyPag As OggettoPagamenti
    '    Dim oListPag() As OggettoPagamenti
    '    Dim nList As Integer = -1

    '    Try
    '        cmdMyCommand.CommandText = "SELECT *"
    '        cmdMyCommand.CommandText += " FROM V_PAG_CARTELLEPAGAMENTI"
    '        cmdMyCommand.CommandText += " WHERE (IDENTE=@IDENTE)"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
    '        If sCodCartella <> "" Then
    '            cmdMyCommand.CommandText += " AND (CODICE_CARTELLA=@CODCARTELLA)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODCARTELLA", SqlDbType.NVarChar)).Value = sCodCartella
    '        End If
    '        If sAnno <> "" Then
    '            cmdMyCommand.CommandText += " AND (ANNO=@ANNO)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = sAnno
    '        End If
    '        If nIdContribuente <> -1 Then
    '            cmdMyCommand.CommandText += " AND (COD_CONTRIBUENTE =@IDCONTRIBUENTE)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = nIdContribuente
    '        End If
    '        cmdMyCommand.CommandText += " ORDER BY CODICE_CARTELLA, ID, NUMERO_RATA, DATA_SCADENZA"
    '        DrDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '        Do While DrDati.Read
    '            oMyPag = New OggettoPagamenti
    '            If Not IsDBNull(DrDati("ID_PAGAMENTO")) Then
    '                oMyPag.ID = CInt(DrDati("ID_PAGAMENTO"))
    '            End If
    '            If Not IsDBNull(DrDati("COD_CONTRIBUENTE")) Then
    '                oMyPag.IdContribuente = CInt(DrDati("COD_CONTRIBUENTE"))
    '            End If
    '            If Not IsDBNull(DrDati("CODICE_CARTELLA")) Then
    '                oMyPag.sNumeroAvviso = CStr(DrDati("CODICE_CARTELLA"))
    '            End If
    '            If Not IsDBNull(DrDati("ANNO")) Then
    '                oMyPag.sAnno = CStr(DrDati("ANNO"))
    '            End If
    '            If Not IsDBNull(DrDati("NUMERO_RATA")) Then
    '                oMyPag.sNumeroRata = CStr(DrDati("NUMERO_RATA"))
    '            End If
    '            If Not IsDBNull(DrDati("IMPORTO_RATA")) Then
    '                oMyPag.dImportoRata = CDbl(DrDati("IMPORTO_RATA"))
    '            End If
    '            If Not IsDBNull(DrDati("DATA_SCADENZA")) Then
    '                oMyPag.tDataScadenza = CDate(DrDati("DATA_SCADENZA"))
    '            End If
    '            If Not IsDBNull(DrDati("DATA_PAGAMENTO")) Then
    '                oMyPag.tDataPagamento = CDate(DrDati("DATA_PAGAMENTO"))
    '            End If
    '            If Not IsDBNull(DrDati("DATA_ACCREDITO")) Then
    '                oMyPag.tDataAccredito = CDate(DrDati("DATA_ACCREDITO"))
    '            End If
    '            If Not IsDBNull(DrDati("IMPORTO_PAGAMENTO")) Then
    '                oMyPag.dImportoPagamento = CDbl(DrDati("IMPORTO_PAGAMENTO"))
    '            Else
    '                oMyPag.dImportoPagamento = oMyPag.dImportoRata
    '            End If

    '            nList += 1
    '            ReDim Preserve oListPag(nList)
    '            oListPag(nList) = oMyPag
    '        Loop

    '        Return oListPag
    '    Catch Err As Exception
    '        Dim sValParametri As String = Utility.Costanti.GetValParamCmd(cmdMyCommand)
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsGestPag.GetCartellePagamenti.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in ClsPagamenti::GetCartellePagamenti::" & Err.Message & vbCrLf & "SQL::" & cmdMyCommand.CommandText & vbCrLf & " VALUES " & sValParametri)
    '    Finally
    '        DrDati.Close()
    '    End Try
    'End Function

    'Public Function CheckPagVSCartella(ByVal nTipo As Integer, ByVal oMyPagamento As OggettoPagamenti, ByVal WFSessione As OPENUtility.CreateSessione) As Boolean
    '    Dim DrDati As SqlClient.SqlDataReader

    '    Try
    '        Select Case nTipo
    '            Case 0 'controllo che l'importo pagato sia uguale alla rata emessa
    '                cmdMyCommand.CommandText = "SELECT *"
    '                cmdMyCommand.CommandText += " FROM V_PAG_CARTELLEPAGAMENTI"
    '                cmdMyCommand.CommandText += " WHERE (IDENTE=@IDENTE)"
    '                cmdMyCommand.CommandText += " AND (CODICE_CARTELLA=@CODCARTELLA)"
    '                cmdMyCommand.CommandText += " AND (NUMERO_RATA=@NRATA)"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oMyPagamento.IdEnte
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODCARTELLA", SqlDbType.NVarChar)).Value = oMyPagamento.sNumeroAvviso
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NRATA", SqlDbType.NVarChar)).Value = oMyPagamento.sNumeroRata
    '                DrDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '                Do While DrDati.Read
    '                    If Not IsDBNull(DrDati("importo_rata")) Then
    '                        If CDbl(DrDati("importo_rata")) < oMyPagamento.dImportoPagamento Then
    '                            Return False
    '                        Else
    '                            Return True
    '                        End If
    '                    Else
    '                        Return True
    '                    End If
    '                Loop
    '            Case 1 'controllo che l'importo totale pagato non superi l'importo dovuto
    '                cmdMyCommand.CommandText = "SELECT CODICE_CARTELLA, DOVUTO, SUM(IMPORTO_PAGAMENTO) AS PAGATO"
    '                cmdMyCommand.CommandText += " FROM V_PAG_CARTELLEPAGAMENTI"
    '                cmdMyCommand.CommandText += " WHERE (IDENTE=@IDENTE)"
    '                cmdMyCommand.CommandText += " AND (CODICE_CARTELLA=@CODCARTELLA)"
    '                cmdMyCommand.CommandText += " GROUP BY CODICE_CARTELLA, DOVUTO"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oMyPagamento.IdEnte
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODCARTELLA", SqlDbType.NVarChar)).Value = oMyPagamento.sNumeroAvviso
    '                DrDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '                Do While DrDati.Read
    '                    If Not IsDBNull(DrDati("pagato")) Then
    '                        If CDbl(DrDati("dovuto")) < (CDbl(DrDati("pagato")) + oMyPagamento.dImportoPagamento) Then
    '                            Return False
    '                        Else
    '                            Return True
    '                        End If
    '                    Else
    '                        If CDbl(DrDati("dovuto")) < oMyPagamento.dImportoPagamento Then
    '                            Return False
    '                        Else
    '                            Return True
    '                        End If
    '                    End If
    '                Loop
    '        End Select

    '        Return True
    '    Catch ex As Exception
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsGestPag.CheckPagVSCartella.errore: ", Err)
    '        Return False
    '    Finally
    '        DrDati.Close()
    '    End Try
    'End Function

    'Public Function AbbinaBollettino(ByRef oMyPagamento As OggettoPagamenti, ByVal WFSessione As OPENUtility.CreateSessione) As Boolean
    '    '{true= abbinamento trovato; false= abbinamento non trovato }
    '    Dim DrMyDati As SqlClient.SqlDataReader
    '    Dim bMyReturn As Boolean = False

    '    Try
    '        'valorizzo il commandtext:
    '        cmdMyCommand.CommandText = "SELECT *"
    '        cmdMyCommand.CommandText += " FROM V_PAG_CARTELLEPAGAMENTI"
    '        cmdMyCommand.CommandText += " WHERE (1=1)"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.CommandText += " AND (CODBOLLETTINO=@CODBOLLETTINO)"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODBOLLETTINO ", SqlDbType.NVarChar)).Value = oMyPagamento.sCodBollettino
    '        DrMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '        Do While DrMyDati.Read
    '            oMyPagamento.IdContribuente = CInt(DrMyDati("cod_contribuente"))
    '            oMyPagamento.sNumeroAvviso = CStr(DrMyDati("codice_cartella"))
    '            oMyPagamento.sAnno = CStr(DrMyDati("anno"))
    '            oMyPagamento.sNumeroRata = CStr(DrMyDati("numero_rata"))
    '            bMyReturn = True
    '        Loop
    '    Catch Err As Exception
    '        Dim sValParametri As String = Utility.Costanti.GetValParamCmd(cmdMyCommand)
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsGestPag.AbbinaBollettino.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in ClsPagamenti::AbbinaBollettino::" & Err.Message & vbCrLf & "SQL::" & cmdMyCommand.CommandText & vbCrLf & " VALUES " & sValParametri)
    '    Finally
    '        DrMyDati.Close()
    '    End Try
    '    Return bMyReturn
    'End Function

    'Public Function GetListPagamenti(ByVal oRicPagamenti As ObjSearchPagamenti, ByVal WFSessione As OPENUtility.CreateSessione) As OggettoPagamenti()
    '    Dim DrDati As SqlClient.SqlDataReader
    '    Dim oListPagamenti() As OggettoPagamenti
    '    Dim oMyPagamento As OggettoPagamenti
    '    Dim nList As Integer = -1

    '    Try
    '        'prelevo i dati della testata
    '        cmdMyCommand.CommandText = "SELECT *"
    '        cmdMyCommand.CommandText += " FROM V_GETPAGAMENTI"
    '        cmdMyCommand.CommandText += " WHERE (1=1) "
    '        cmdMyCommand.CommandText += " AND (IDENTE=@IDENTE)"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oRicPagamenti.sEnte
    '        If oRicPagamenti.bRicPag = True Then
    '            cmdMyCommand.CommandText += " AND (NOT ID IS NULL)"
    '        End If
    '        If oRicPagamenti.sAnnoRif <> "" Then
    '            cmdMyCommand.CommandText += " AND (ANNO=@ANNO) "
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = oRicPagamenti.sAnnoRif
    '        End If
    '        If oRicPagamenti.IdPagamento <> -1 Then
    '            cmdMyCommand.CommandText += " AND (ID =@IDPAG)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPAG", SqlDbType.Int)).Value = oRicPagamenti.IdPagamento
    '        End If
    '        If oRicPagamenti.IdContribuente <> -1 Then
    '            cmdMyCommand.CommandText += " AND (IDCONTRIBUENTE =@IDCONTRIBUENTE)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = oRicPagamenti.IdContribuente
    '        End If
    '        If oRicPagamenti.sCognome <> "" Then
    '            cmdMyCommand.CommandText += " AND (COGNOME LIKE @COGNOME)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COGNOME", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(oRicPagamenti.sCognome) & "%"
    '        End If
    '        If oRicPagamenti.sNome <> "" Then
    '            cmdMyCommand.CommandText += " AND (NOME LIKE @NOME)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOME", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(oRicPagamenti.sNome) & "%"
    '        End If
    '        If oRicPagamenti.sCFPIVA <> "" Then
    '            cmdMyCommand.CommandText += " AND (CFPIVA LIKE @CFPIVA)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CFPIVA", SqlDbType.NVarChar)).Value = oRicPagamenti.sCFPIVA & "%"
    '        End If
    '        If oRicPagamenti.sNAvviso <> "" Then
    '            cmdMyCommand.CommandText += " AND (CODICE_CARTELLA=@CODCARTELLA)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODCARTELLA", SqlDbType.NVarChar)).Value = oRicPagamenti.sNAvviso
    '        End If
    '        If oRicPagamenti.tDataAccreditoDal <> Date.MinValue And oRicPagamenti.tDataAccreditoAl <> Date.MinValue And oRicPagamenti.tDataAccreditoDal <> Date.MaxValue Then
    '            cmdMyCommand.CommandText += " AND (DATA_ACCREDITO >=@DATADAL)"
    '            cmdMyCommand.CommandText += " AND (DATA_ACCREDITO <=@DATAAL)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATADAL", SqlDbType.DateTime)).Value = oRicPagamenti.tDataAccreditoDal
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAAL", SqlDbType.DateTime)).Value = oRicPagamenti.tDataAccreditoAl
    '        End If
    '        If oRicPagamenti.tDataAccreditoDal <> Date.MinValue And oRicPagamenti.tDataAccreditoAl = Date.MinValue And oRicPagamenti.tDataAccreditoAl <> Date.MaxValue Then
    '            cmdMyCommand.CommandText += " AND (DATA_ACCREDITO=@DATADAL)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATADAL", SqlDbType.DateTime)).Value = oRicPagamenti.tDataAccreditoDal
    '        End If
    '        cmdMyCommand.CommandText += " ORDER BY COGNOME, NOME, CODICE_CARTELLA"
    '        'eseguo la query
    '        DrDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '        Do While DrDati.Read
    '            oMyPagamento = New OggettoPagamenti
    '            oMyPagamento.ID = CInt(DrDati("ID"))
    '            oMyPagamento.IdContribuente = CStr(DrDati("IDCONTRIBUENTE"))
    '            oMyPagamento.IdEnte = CStr(DrDati("IDENTE"))
    '            oMyPagamento.sAnno = CStr(DrDati("ANNO"))
    '            oMyPagamento.sCognome = CStr(DrDati("COGNOME"))
    '            oMyPagamento.sNome = CStr(DrDati("NOME"))
    '            oMyPagamento.sCFPIVA = CStr(DrDati("cfpiva"))
    '            If Not (DrDati("CODICE_CARTELLA") Is DBNull.Value) Then
    '                oMyPagamento.sNumeroAvviso = CStr(DrDati("CODICE_CARTELLA"))
    '            Else
    '                oMyPagamento.sNumeroAvviso = ""
    '            End If
    '            If Not (DrDati("NUMERO_RATA") Is DBNull.Value) Then
    '                oMyPagamento.sNumeroRata = CStr(DrDati("NUMERO_RATA"))
    '            Else
    '                oMyPagamento.sNumeroRata = ""
    '            End If
    '            If Not IsDBNull(DrDati("IMPORTO_RATA")) Then
    '                oMyPagamento.dImportoRata = DrDati("IMPORTO_RATA")
    '            Else
    '                oMyPagamento.dImportoRata = 0
    '            End If
    '            If Not IsDBNull(DrDati("IMPORTO_PAGATO")) Then
    '                oMyPagamento.dImportoPagamento = DrDati("IMPORTO_PAGATO")
    '            Else
    '                oMyPagamento.dImportoPagamento = 0
    '            End If
    '            If Not IsDBNull(DrDati("IMPORTO_PAGATOSTAT")) Then
    '                oMyPagamento.dImportoStat = DrDati("IMPORTO_PAGATOSTAT")
    '            Else
    '                oMyPagamento.dImportoStat = 0
    '            End If
    '            If Not (DrDati("DATA_PAGAMENTO") Is DBNull.Value) Then
    '                oMyPagamento.tDataPagamento = CDate(DrDati("DATA_PAGAMENTO"))
    '            Else
    '                oMyPagamento.tDataPagamento = Date.MinValue
    '            End If
    '            If Not (DrDati("DATA_ACCREDITO") Is DBNull.Value) Then
    '                oMyPagamento.tDataAccredito = CDate(DrDati("DATA_ACCREDITO"))
    '            Else
    '                oMyPagamento.tDataAccredito = Date.MinValue
    '            End If
    '            If Not (DrDati("DATA_SCADENZA") Is DBNull.Value) Then
    '                oMyPagamento.tDataScadenza = CDate(DrDati("DATA_SCADENZA"))
    '            Else
    '                oMyPagamento.tDataScadenza = Date.MinValue
    '            End If
    '            If Not (DrDati("provenienza") Is DBNull.Value) Then
    '                oMyPagamento.sProvenienza = CStr(DrDati("provenienza"))
    '            End If
    '            If Not IsDBNull(DrDati("TIPO_PAGAMENTO")) Then
    '                oMyPagamento.sTipoPagamento = CStr(DrDati("TIPO_PAGAMENTO"))
    '            Else
    '                oMyPagamento.sTipoPagamento = ""
    '            End If
    '            If Not (DrDati("NOTE") Is DBNull.Value) Then
    '                oMyPagamento.sNote = CStr(DrDati("NOTE"))
    '            Else
    '                oMyPagamento.sNote = ""
    '            End If
    '            If Not (DrDati("OPERATORE") Is DBNull.Value) Then
    '                oMyPagamento.sOperatore = CStr(DrDati("OPERATORE"))
    '            Else
    '                oMyPagamento.sOperatore = ""
    '            End If
    '            nList += 1
    '            ReDim Preserve oListPagamenti(nList)
    '            oListPagamenti(nList) = oMyPagamento
    '        Loop

    '        Return oListPagamenti
    '    Catch Err As Exception
    '        Dim sValParametri As String = Utility.Costanti.GetValParamCmd(cmdMyCommand)
    ''Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsGestPag.GetListPagamenti.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in objavviso::GetAvviso::" & Err.Message & "::cmdMyCommand.CommandText::" & cmdMyCommand.CommandText & vbCrLf & " VALUES " & sValParametri)
    '        Return Nothing
    '    Finally
    '        DrDati.Close()
    '    End Try
    'End Function


    'Public Function GetStampaPagamenti(ByVal oRicPagamenti As ObjSearchPagamenti, ByVal WFSessione As OPENUtility.CreateSessione) As DataView
    '    Dim dvPagamenti As New DataView

    '    Try
    '        cmdMyCommand.Connection = WFSessione.oSession.oAppDB.GetConnection
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandText = "SELECT COGNOME, NOME, CFPIVA, IDENTE, CODICE_CARTELLA, ANNO"
    '        cmdMyCommand.CommandText += ", DOVUTO, DOVUTOCOMUNE, DOVUTOSTAT"
    '        cmdMyCommand.CommandText += ", SUM(IMPORTO_PAGATO) AS IMPORTO_PAGATO, SUM(IMPORTO_PAGATOCOMUNE) AS IMPORTO_PAGATOCOMUNE, SUM(IMPORTO_PAGATOSTAT) AS IMPORTO_PAGATOSTAT"
    '        Select Case oRicPagamenti.nTipoStampa
    '            Case 0
    '                cmdMyCommand.CommandText += ", ID AS IDPAG, DATA_PAGAMENTO, DATA_ACCREDITO"
    '            Case Else
    '        End Select
    '        cmdMyCommand.CommandText += " FROM V_GETPAGAMENTI"
    '        cmdMyCommand.CommandText += " WHERE (IDENTE=@IDENTE)"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oRicPagamenti.sEnte
    '        If oRicPagamenti.sAnnoRif <> "" Then
    '            cmdMyCommand.CommandText += " AND (ANNO=@ANNO) "
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = oRicPagamenti.sAnnoRif
    '        End If
    '        If oRicPagamenti.IdContribuente <> -1 Then
    '            cmdMyCommand.CommandText += " AND (IDCONTRIBUENTE =@IDCONTRIBUENTE)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = oRicPagamenti.IdContribuente
    '        End If
    '        If oRicPagamenti.sCognome <> "" Then
    '            cmdMyCommand.CommandText += " AND (COGNOME LIKE @COGNOME)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COGNOME", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(oRicPagamenti.sCognome) & "%"
    '        End If
    '        If oRicPagamenti.sNome <> "" Then
    '            cmdMyCommand.CommandText += " AND (NOME LIKE @NOME)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOME", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(oRicPagamenti.sNome) & "%"
    '        End If
    '        If oRicPagamenti.sCFPIVA <> "" Then
    '            cmdMyCommand.CommandText += " AND (CFPIVA LIKE @CFPIVA)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CFPIVA", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(oRicPagamenti.sCFPIVA) & "%"
    '        End If
    '        If oRicPagamenti.sNAvviso <> "" Then
    '            cmdMyCommand.CommandText += " AND (CODICE_CARTELLA=@CODCARTELLA)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODCARTELLA", SqlDbType.NVarChar)).Value = oRicPagamenti.sNAvviso
    '        End If
    '        If oRicPagamenti.tDataAccreditoDal <> Date.MinValue And oRicPagamenti.tDataAccreditoAl <> Date.MinValue And oRicPagamenti.tDataAccreditoDal <> DateTime.MaxValue Then
    '            cmdMyCommand.CommandText += " AND (DATA_ACCREDITO >=@DATADAL)"
    '            cmdMyCommand.CommandText += " AND (DATA_ACCREDITO <=@DATAAL)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATADAL", SqlDbType.DateTime)).Value = oRicPagamenti.tDataAccreditoDal
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAAL", SqlDbType.DateTime)).Value = oRicPagamenti.tDataAccreditoAl
    '        End If
    '        If oRicPagamenti.tDataAccreditoDal <> Date.MinValue And oRicPagamenti.tDataAccreditoAl = Date.MinValue And oRicPagamenti.tDataAccreditoAl <> DateTime.MaxValue Then
    '            cmdMyCommand.CommandText += " AND (DATA_ACCREDITO=@DATADAL)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATADAL", SqlDbType.DateTime)).Value = oRicPagamenti.tDataAccreditoDal
    '        End If
    '        Select Case oRicPagamenti.nTipoStampa '{0=pagamenti;1=emesso non pagato;2=emesso pagato parzialmente;3=emesso pagato in eccesso}
    '            Case 0
    '                cmdMyCommand.CommandText += " AND (NOT DATA_PAGAMENTO IS NULL)"
    '            Case 1
    '                cmdMyCommand.CommandText += " AND (DATA_PAGAMENTO IS NULL)"
    '            Case Else
    '        End Select
    '        cmdMyCommand.CommandText += " GROUP BY COGNOME, NOME, CFPIVA, IDENTE, CODICE_CARTELLA, ANNO"
    '        cmdMyCommand.CommandText += ", DOVUTO, DOVUTOCOMUNE, DOVUTOSTAT"
    '        Select Case oRicPagamenti.nTipoStampa
    '            Case 0
    '                cmdMyCommand.CommandText += ", ID, DATA_PAGAMENTO, DATA_ACCREDITO"
    '            Case Else
    '        End Select
    '        Select Case oRicPagamenti.nTipoStampa '{0=pagamenti;1=emesso non pagato;2=emesso pagato parzialmente;3=emesso pagato in eccesso}
    '            Case 2
    '                cmdMyCommand.CommandText += " HAVING (CAST(DOVUTO-SUM(IMPORTO_PAGATO) AS NUMERIC(9,2))>0)"
    '            Case 3
    '                cmdMyCommand.CommandText += " HAVING (CAST(DOVUTO-SUM(IMPORTO_PAGATO) AS NUMERIC(9,2))<0)"
    '            Case Else
    '        End Select
    '        cmdMyCommand.CommandText += " ORDER BY COGNOME, NOME, CFPIVA, CODICE_CARTELLA"
    '        dvPagamenti = WFSessione.oSession.oAppDB.GetPrivateDataview(cmdMyCommand)
    '        Return dvPagamenti

    '    Catch ex As Exception
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsGestPag.GetStampaPagamenti.errore: ", ex)
    '        Return Nothing
    '    End Try
    'End Function

    'Public Function GetStampaPagamentiNonPres(ByVal oRicPagamenti As ObjSearchPagamenti, ByVal WFSessione As OPENUtility.CreateSessione) As DataView
    '    Dim dvPagamenti As New DataView

    '    Try
    '        cmdMyCommand.Connection = WFSessione.oSession.oAppDB.GetConnection
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = "prc_PAG_CARTELLENONPAGATE"
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oRicPagamenti.sEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = oRicPagamenti.sAnnoRif
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = oRicPagamenti.IdContribuente
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COGNOME", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(oRicPagamenti.sCognome)
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOME", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(oRicPagamenti.sNome)
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CFPIVA", SqlDbType.NVarChar)).Value = oRicPagamenti.sCFPIVA
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODCARTELLA", SqlDbType.NVarChar)).Value = oRicPagamenti.sNAvviso
    '        'cmdMyCommand.CommandText = "SELECT  DISTINCT COGNOME, NOME, CFPIVA, IDENTE, CODICE_CARTELLA, DOVUTO, ANNO, NULL AS IMPORTO_PAGATO"
    '        'cmdMyCommand.CommandText += " FROM V_PAG_CARTELLEPAGAMENTI "
    '        'cmdMyCommand.CommandText += " LEFT JOIN ("
    '        'cmdMyCommand.CommandText += " 	SELECT IDCONTRIBUENTE"
    '        'cmdMyCommand.CommandText += " 	FROM TBLPAGAMENTI"
    '        'cmdMyCommand.CommandText += " ) P ON V_PAG_CARTELLEPAGAMENTI.COD_CONTRIBUENTE=P.IDCONTRIBUENTE"
    '        'cmdMyCommand.CommandText += " WHERE (IDENTE=@IDENTE)"
    '        'cmdMyCommand.CommandText += " AND (P.IDCONTRIBUENTE IS NULL) "
    '        ''valorizzo i parameters:
    '        'cmdMyCommand.Parameters.Clear()
    '        'cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oRicPagamenti.sEnte
    '        'If oRicPagamenti.sAnnoRif <> "" Then
    '        '    cmdMyCommand.CommandText += " AND (ANNO=@ANNO) "
    '        '    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = oRicPagamenti.sAnnoRif
    '        'End If
    '        'If oRicPagamenti.IdContribuente <> -1 Then
    '        '    cmdMyCommand.CommandText += " AND (COD_CONTRIBUENTE =@IDCONTRIBUENTE)"
    '        '    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = oRicPagamenti.IdContribuente
    '        'End If
    '        'If oRicPagamenti.sCognome <> "" Then
    '        '    cmdMyCommand.CommandText += " AND (COGNOME LIKE @COGNOME)"
    '        '    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COGNOME", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(oRicPagamenti.sCognome)
    '        'End If
    '        'If oRicPagamenti.sNome <> "" Then
    '        '    cmdMyCommand.CommandText += " AND (NOME LIKE @NOME)"
    '        '    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOME", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(oRicPagamenti.sNome)
    '        'End If
    '        'If oRicPagamenti.sCFPIVA <> "" Then
    '        '    cmdMyCommand.CommandText += " AND (CFPIVA LIKE @CFPIVA)"
    '        '    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CFPIVA", SqlDbType.NVarChar)).Value = oRicPagamenti.sCFPIVA
    '        'End If
    '        'If oRicPagamenti.sNAvviso <> "" Then
    '        '    cmdMyCommand.CommandText += " AND (CODICE_CARTELLA=@CODCARTELLA)"
    '        '    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODCARTELLA", SqlDbType.NVarChar)).Value = oRicPagamenti.sNAvviso
    '        'End If
    '        'cmdMyCommand.CommandText += " AND (DATA_PAGAMENTO IS NULL)"
    '        'cmdMyCommand.CommandText += " ORDER BY COGNOME, NOME, CFPIVA, CODICE_CARTELLA"
    '        dvPagamenti = WFSessione.oSession.oAppDB.GetPrivateDataview(cmdMyCommand)
    '        Return dvPagamenti

    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsGestPag.GetStampaPagamentiNonPres.errore: ", Err)
    '        Return Nothing
    '    End Try
    'End Function

    'Public Function SetPagamento(ByVal oMyPagamento As OggettoPagamenti, ByVal nDBOperation As Integer, ByVal WFSessione As OPENUtility.CreateSessione) As Integer
    '    '{0= non a buon fine, >0= id tabella}
    '    Dim nMyReturn As Integer

    '    Try
    '        'valorizzo il CommandText:
    '        Select Case nDBOperation
    '            Case 0
    '                cmdMyCommand.CommandText = "INSERT INTO TBLPAGAMENTI (IDIMPORTAZIONE , IDFLUSSO, IDENTE, IDCONTRIBUENTE, PROVENIENZA,"
    '                cmdMyCommand.CommandText += " ANNO, CODICE_CARTELLA, CODICE_BOLLETTINO, NUMERO_RATA,"
    '                cmdMyCommand.CommandText += " DATA_PAGAMENTO, DATA_ACCREDITO, DIVISA, IMPORTO_PAGAMENTO,"
    '                cmdMyCommand.CommandText += " TIPO_BOLLETTINO, TIPO_PAGAMENTO,"
    '                cmdMyCommand.CommandText += " PROGRESSIVO_CARICAMENTO, PROGRESSIVO_SELEZIONE, CCBENEFICIARIO, UFFICIOSPORTELLO,"
    '                cmdMyCommand.CommandText += " NOTE, FLAG_DETTAGLIO, OPERATORE, DATA_INSERIMENTO)"
    '                cmdMyCommand.CommandText += " VALUES (@IDIMPORTAZIONE , @IDFLUSSO, @IDENTE, @IDCONTRIBUENTE, @PROVENIENZA,"
    '                cmdMyCommand.CommandText += " @ANNO, @CODICE_CARTELLA, @CODICE_BOLLETTINO, @NUMERO_RATA,"
    '                cmdMyCommand.CommandText += " @DATA_PAGAMENTO, @DATA_ACCREDITO, @DIVISA, @IMPORTO_PAGAMENTO,"
    '                cmdMyCommand.CommandText += " @TIPO_BOLLETTINO, @TIPO_PAGAMENTO,"
    '                cmdMyCommand.CommandText += " @PROGRESSIVO_CARICAMENTO, @PROGRESSIVO_SELEZIONE, @CCBENEFICIARIO, @UFFICIOSPORTELLO,"
    '                cmdMyCommand.CommandText += " @NOTE, @FLAGDETTAGLIO, @OPERATORE, @DATA_INSERIMENTO)"
    '                cmdMyCommand.CommandText += " SELECT @@IDENTITY"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDIMPORTAZIONE", SqlDbType.Int)).Value = oMyPagamento.IDImportazione
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSO", SqlDbType.Int)).Value = oMyPagamento.IDFlusso
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oMyPagamento.IdEnte
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = oMyPagamento.IdContribuente
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PROVENIENZA", SqlDbType.NVarChar)).Value = oMyPagamento.sProvenienza
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = oMyPagamento.sAnno
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE_CARTELLA", SqlDbType.NVarChar)).Value = oMyPagamento.sNumeroAvviso
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE_BOLLETTINO", SqlDbType.NVarChar)).Value = oMyPagamento.sCodBollettino
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO_RATA", SqlDbType.NVarChar)).Value = oMyPagamento.sNumeroRata
    '                If oMyPagamento.tDataPagamento = Date.MinValue Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_PAGAMENTO", SqlDbType.DateTime)).Value = DBNull.Value
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_PAGAMENTO", SqlDbType.DateTime)).Value = oMyPagamento.tDataPagamento
    '                End If
    '                If oMyPagamento.tDataAccredito = Date.MinValue Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_ACCREDITO", SqlDbType.DateTime)).Value = oMyPagamento.tDataPagamento
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_ACCREDITO", SqlDbType.DateTime)).Value = oMyPagamento.tDataAccredito
    '                End If
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DIVISA", SqlDbType.NVarChar)).Value = oMyPagamento.sDivisa
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO_PAGAMENTO", SqlDbType.Float)).Value = oMyPagamento.dImportoPagamento
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPO_BOLLETTINO", SqlDbType.NVarChar)).Value = oMyPagamento.sTipoBollettino
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPO_PAGAMENTO", SqlDbType.NVarChar)).Value = oMyPagamento.sTipoPagamento
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PROGRESSIVO_CARICAMENTO", SqlDbType.NVarChar)).Value = oMyPagamento.sProgCaricamento
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PROGRESSIVO_SELEZIONE", SqlDbType.NVarChar)).Value = oMyPagamento.sProgSelezione
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CCBENEFICIARIO", SqlDbType.NVarChar)).Value = oMyPagamento.sCCBeneficiario
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@UFFICIOSPORTELLO", SqlDbType.NVarChar)).Value = oMyPagamento.sUfficioSportello
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOTE", SqlDbType.NVarChar)).Value = oMyPagamento.sNote
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FLAGDETTAGLIO", SqlDbType.Bit)).Value = oMyPagamento.bFlagDettaglio
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@OPERATORE", SqlDbType.NVarChar)).Value = oMyPagamento.sOperatore
    '                If oMyPagamento.tDataInsert = Date.MinValue Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INSERIMENTO", SqlDbType.DateTime)).Value = Now
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INSERIMENTO", SqlDbType.DateTime)).Value = oMyPagamento.tDataInsert
    '                End If
    '                'eseguo la query()
    '                Dim DrReturn As SqlClient.SqlDataReader
    '                DrReturn = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '                Do While DrReturn.Read
    '                    nMyReturn = DrReturn(0)
    '                Loop
    '                DrReturn.Close()
    '            Case 1
    '                cmdMyCommand.CommandText = "UPDATE TBLPAGAMENTI"
    '                cmdMyCommand.CommandText += " SET DATA_PAGAMENTO=@DATA_PAGAMENTO,"
    '                cmdMyCommand.CommandText += " IMPORTO_PAGAMENTO=@IMPORTO_PAGAMENTO,"
    '                cmdMyCommand.CommandText += " ANNO=@ANNO,"
    '                cmdMyCommand.CommandText += " CODICE_CARTELLA=@CODICE_CARTELLA,"
    '                cmdMyCommand.CommandText += " NUMERO_RATA=@NUMERO_RATA,"
    '                cmdMyCommand.CommandText += " DATA_ACCREDITO =@DATA_ACCREDITO"
    '                cmdMyCommand.CommandText += " WHERE (IDENTE=@IDENTE) AND (IDCONTRIBUENTE=@IDCONTRIBUENTE) AND (ID=@IDPAGAMENTO)"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                If oMyPagamento.tDataPagamento = Date.MinValue Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_PAGAMENTO", SqlDbType.DateTime)).Value = DBNull.Value
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_PAGAMENTO", SqlDbType.DateTime)).Value = oMyPagamento.tDataPagamento
    '                End If
    '                If oMyPagamento.tDataAccredito = Date.MinValue Then
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_ACCREDITO", SqlDbType.DateTime)).Value = DBNull.Value
    '                Else
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_ACCREDITO", SqlDbType.DateTime)).Value = oMyPagamento.tDataAccredito
    '                End If
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO_PAGAMENTO", SqlDbType.Float)).Value = oMyPagamento.dImportoPagamento
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = oMyPagamento.sAnno
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE_CARTELLA", SqlDbType.NVarChar)).Value = oMyPagamento.sNumeroAvviso
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO_RATA", SqlDbType.NVarChar)).Value = oMyPagamento.sNumeroRata
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oMyPagamento.IdEnte
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = oMyPagamento.IdContribuente
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPAGAMENTO", SqlDbType.Int)).Value = oMyPagamento.ID
    '                nMyReturn = CInt(WFSessione.oSession.oAppDB.Execute(cmdMyCommand))
    '            Case 2
    '                cmdMyCommand.CommandText = "DELETE"
    '                cmdMyCommand.CommandText += " FROM TBLPAGAMENTI"
    '                cmdMyCommand.CommandText += " WHERE (IDIMPORTAZIONE=@IDIMPORTAZIONE)"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDIMPORTAZIONE", SqlDbType.Int)).Value = oMyPagamento.IDImportazione
    '                nMyReturn = CInt(WFSessione.oSession.oAppDB.Execute(cmdMyCommand))
    '            Case 3
    '                cmdMyCommand.CommandText = "UPDATE TBLPAGAMENTI SET FLAG_DETTAGLIO=1"
    '                cmdMyCommand.CommandText += " WHERE (ID=@IDPAGAMENTO)"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPAGAMENTO", SqlDbType.Int)).Value = oMyPagamento.ID
    '                nMyReturn = CInt(WFSessione.oSession.oAppDB.Execute(cmdMyCommand))
    '            Case 4
    '                cmdMyCommand.CommandText = "UPDATE TBLPAGAMENTI SET FLAG_DETTAGLIO=0"
    '                cmdMyCommand.CommandText += " WHERE (ID=@IDPAGAMENTO)"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPAGAMENTO", SqlDbType.Int)).Value = oMyPagamento.ID
    '                nMyReturn = CInt(WFSessione.oSession.oAppDB.Execute(cmdMyCommand))
    '            Case 5
    '                cmdMyCommand.CommandText = "DELETE"
    '                cmdMyCommand.CommandText += " FROM TBLPAGAMENTI"
    '                cmdMyCommand.CommandText += " WHERE (ID=@IDPAGAMENTO)"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPAGAMENTO", SqlDbType.Int)).Value = oMyPagamento.ID
    '                nMyReturn = CInt(WFSessione.oSession.oAppDB.Execute(cmdMyCommand))
    '        End Select
    '        Return nMyReturn
    '    Catch Err As Exception
    '        Dim sValParametri As String = Utility.Costanti.GetValParamCmd(cmdMyCommand)
    ''Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsGestPag.SetPagamenti.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in ClsPagamenti::SetPagamento::" & Err.Message & vbCrLf & "SQL::" & cmdMyCommand.CommandText & vbCrLf & " VALUES " & sValParametri)
    '        Return -1
    '    End Try
    'End Function

    '*** 20131104 - TARES ***
    'Public Function GetStampaPagamenti(ByVal oRicPagamenti As ObjSearchPagamenti, ByRef DBEngineOut As DAL.DBEngine) As DataView
    '    Dim MyDBEngine As DAL.DBEngine = Nothing
    '    Dim dtMyDati As New DataTable()
    '    Dim dvPagamenti As New DataView

    '    Try
    '        If (Not (DBEngineOut) Is Nothing) Then
    '            MyDBEngine = DBEngineOut
    '        Else
    '            MyDBEngine = Utility.DBEngineFactory.GetDBEngine(ConstSession.StringConnection)
    '            MyDBEngine.OpenConnection()
    '        End If
    '        MyDBEngine.ClearParameters()
    '        MyDBEngine.AddParameter("@IdEnte", oRicPagamenti.sEnte, ParameterDirection.Input)
    '        MyDBEngine.AddParameter("@RicPag", oRicPagamenti.bRicPag, ParameterDirection.Input)
    '        MyDBEngine.AddParameter("@Anno", oRicPagamenti.sAnnoRif, ParameterDirection.Input)
    '        MyDBEngine.AddParameter("@IdPag", oRicPagamenti.IdPagamento, ParameterDirection.Input)
    '        MyDBEngine.AddParameter("@IdContribuente", oRicPagamenti.IdContribuente, ParameterDirection.Input)
    '        MyDBEngine.AddParameter("@COGNOME", FncGen.ReplaceCharsForSearch(oRicPagamenti.sCognome) & "%", ParameterDirection.Input)
    '        MyDBEngine.AddParameter("@NOME", FncGen.ReplaceCharsForSearch(oRicPagamenti.sNome) & "%", ParameterDirection.Input)
    '        MyDBEngine.AddParameter("@CFPIVA", FncGen.ReplaceCharsForSearch(oRicPagamenti.sCFPIVA) & "%", ParameterDirection.Input)
    '        MyDBEngine.AddParameter("@CODCARTELLA", oRicPagamenti.sNAvviso, ParameterDirection.Input)
    '        MyDBEngine.AddParameter("@DataDal", oRicPagamenti.tDataAccreditoDal, ParameterDirection.Input)
    '        MyDBEngine.AddParameter("@DataAl", oRicPagamenti.tDataAccreditoAl, ParameterDirection.Input)
    '        MyDBEngine.AddParameter("@IsNonAbb", oRicPagamenti.IsNonAbbinati, ParameterDirection.Input)
    '        MyDBEngine.AddParameter("@TipoStampa", oRicPagamenti.nTipoStampa, ParameterDirection.Input)
    '        MyDBEngine.ExecuteQuery(dtMyDati, "prc_GetPagamenti", CommandType.StoredProcedure)
    '        dvPagamenti = dtMyDati.DefaultView
    '        Return dvPagamenti

    '    Catch ex As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsGestPag.GetStampaPagamenti.errore: ", Err)
    '        Return Nothing
    '    End Try
    'End Function

    Public Function AbbinaBollettino(ByRef oMyPagamento As OggettoPagamenti, ByVal myConnectionString As String) As Boolean
        '{true= abbinamento trovato; false= abbinamento non trovato }
        Dim DrMyDati As SqlClient.SqlDataReader = Nothing
        Dim bMyReturn As Boolean = False

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            'valorizzo il commandtext:
            cmdMyCommand.CommandText = "SELECT *"
            cmdMyCommand.CommandText += " FROM V_PAG_CARTELLEPAGAMENTI"
            cmdMyCommand.CommandText += " WHERE (1=1)"
            'valorizzo i parameters:
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.CommandText += " AND (CODBOLLETTINO=@CODBOLLETTINO)"
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODBOLLETTINO", SqlDbType.NVarChar)).Value = oMyPagamento.sCodBollettino
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            DrMyDati = cmdMyCommand.ExecuteReader
            Do While DrMyDati.Read
                oMyPagamento.IdEnte = CStr(DrMyDati("idente"))
                oMyPagamento.IdContribuente = CInt(DrMyDati("cod_contribuente"))
                oMyPagamento.sNumeroAvviso = CStr(DrMyDati("codice_cartella"))
                oMyPagamento.sAnno = CStr(DrMyDati("anno"))
                oMyPagamento.sNumeroRata = CStr(DrMyDati("numero_rata"))
                bMyReturn = True
            Loop
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.AbbinaBollettino.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
        Finally
            DrMyDati.Close()
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
        Return bMyReturn
    End Function

    Public Function GetStampaPagamenti(ByVal oRicPagamenti As ObjSearchPagamenti, ByVal myStringConnection As String) As DataView
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dtMyDati As New DataTable()
        Dim dvPagamenti As New DataView

        Try
            cmdMyCommand = New SqlClient.SqlCommand
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_GetStampaPagamenti"
            'valorizzo i parameters:
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oRicPagamenti.sEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTRIBUTO", SqlDbType.NVarChar)).Value = oRicPagamenti.IdTributo
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Anno", SqlDbType.NVarChar)).Value = oRicPagamenti.sAnnoRif
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COGNOME", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(oRicPagamenti.sCognome) & "%"
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOME", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(oRicPagamenti.sNome) & "%"
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CFPIVA", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(oRicPagamenti.sCFPIVA) & "%"
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODCARTELLA", SqlDbType.NVarChar)).Value = oRicPagamenti.sNAvviso
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DataPagDal", SqlDbType.DateTime)).Value = oRicPagamenti.tDataPagamentoDal
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DataPagAl", SqlDbType.DateTime)).Value = oRicPagamenti.tDataPagamentoAl
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DataDal", SqlDbType.DateTime)).Value = oRicPagamenti.tDataAccreditoDal
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DataAl", SqlDbType.DateTime)).Value = oRicPagamenti.tDataAccreditoAl
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IsNonAbb", SqlDbType.Bit)).Value = oRicPagamenti.IsNonAbbinati
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPOSTAMPA", SqlDbType.Int)).Value = oRicPagamenti.nTipoStampa
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Flusso", SqlDbType.NVarChar)).Value = oRicPagamenti.Flusso
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Provenienza", SqlDbType.NVarChar)).Value = oRicPagamenti.Provenienza
            'cmdMyCommand.CommandText = "SELECT COGNOME, NOME, CFPIVA, IDENTE, CODICE_CARTELLA, ANNO"
            'cmdMyCommand.CommandText += ", DOVUTO, DOVUTOCOMUNE, DOVUTOSTAT"
            'cmdMyCommand.CommandText += ", SUM(IMPORTO_PAGATO) AS IMPORTO_PAGATO, SUM(IMPORTO_PAGATOCOMUNE) AS IMPORTO_PAGATOCOMUNE, SUM(IMPORTO_PAGATOSTAT) AS IMPORTO_PAGATOSTAT"
            'Select Case oRicPagamenti.nTipoStampa
            '    Case 0
            '        cmdMyCommand.CommandText += ", ID AS IDPAG, DATA_PAGAMENTO, DATA_ACCREDITO"
            '    Case Else
            'End Select
            'cmdMyCommand.CommandText += " FROM V_GETPAGAMENTI"
            'cmdMyCommand.CommandText += " WHERE (IDENTE=@IDENTE)"
            'cmdMyCommand.CommandText += " AND (IDTRIBUTO=@IDTRIBUTO)"
            ''valorizzo i parameters:
            'cmdMyCommand.Parameters.Clear()
            'cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oRicPagamenti.sEnte
            'cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTRIBUTO", SqlDbType.NVarChar)).Value = oRicPagamenti.IdTributo
            'If oRicPagamenti.sAnnoRif <> "" Then
            '    cmdMyCommand.CommandText += " AND (ANNO=@ANNO) "
            '    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = oRicPagamenti.sAnnoRif
            'End If
            'If oRicPagamenti.IdContribuente <> -1 Then
            '    cmdMyCommand.CommandText += " AND (IDCONTRIBUENTE =@IDCONTRIBUENTE)"
            '    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = oRicPagamenti.IdContribuente
            'End If
            'If oRicPagamenti.sCognome <> "" Then
            '    cmdMyCommand.CommandText += " AND (COGNOME LIKE @COGNOME)"
            '    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COGNOME", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(oRicPagamenti.sCognome) & "%"
            'End If
            'If oRicPagamenti.sNome <> "" Then
            '    cmdMyCommand.CommandText += " AND (NOME LIKE @NOME)"
            '    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOME", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(oRicPagamenti.sNome) & "%"
            'End If
            'If oRicPagamenti.sCFPIVA <> "" Then
            '    cmdMyCommand.CommandText += " AND (CFPIVA LIKE @CFPIVA)"
            '    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CFPIVA", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(oRicPagamenti.sCFPIVA) & "%"
            'End If
            'If oRicPagamenti.sNAvviso <> "" Then
            '    cmdMyCommand.CommandText += " AND (CODICE_CARTELLA=@CODCARTELLA)"
            '    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODCARTELLA", SqlDbType.NVarChar)).Value = oRicPagamenti.sNAvviso
            'End If
            'If oRicPagamenti.tDataAccreditoDal <> Date.MinValue And oRicPagamenti.tDataAccreditoAl <> Date.MinValue And oRicPagamenti.tDataAccreditoDal <> DateTime.MaxValue Then
            '    cmdMyCommand.CommandText += " AND (DATA_ACCREDITO >=@DATADAL)"
            '    cmdMyCommand.CommandText += " AND (DATA_ACCREDITO <=@DATAAL)"
            '    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATADAL", SqlDbType.DateTime)).Value = oRicPagamenti.tDataAccreditoDal
            '    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAAL", SqlDbType.DateTime)).Value = oRicPagamenti.tDataAccreditoAl
            'End If
            'If oRicPagamenti.tDataAccreditoDal <> Date.MinValue And oRicPagamenti.tDataAccreditoAl = Date.MinValue And oRicPagamenti.tDataAccreditoAl <> DateTime.MaxValue Then
            '    cmdMyCommand.CommandText += " AND (DATA_ACCREDITO=@DATADAL)"
            '    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATADAL", SqlDbType.DateTime)).Value = oRicPagamenti.tDataAccreditoDal
            'End If
            'Select Case oRicPagamenti.nTipoStampa '{0=pagamenti;1=emesso non pagato;2=emesso pagato parzialmente;3=emesso pagato in eccesso}
            '    Case 0
            '        cmdMyCommand.CommandText += " AND (NOT DATA_PAGAMENTO IS NULL)"
            '    Case 1
            '        cmdMyCommand.CommandText += " AND (DATA_PAGAMENTO IS NULL)"
            '    Case Else
            'End Select
            'cmdMyCommand.CommandText += " GROUP BY COGNOME, NOME, CFPIVA, IDENTE, CODICE_CARTELLA, ANNO"
            'cmdMyCommand.CommandText += ", DOVUTO, DOVUTOCOMUNE, DOVUTOSTAT"
            'Select Case oRicPagamenti.nTipoStampa
            '    Case 0
            '        cmdMyCommand.CommandText += ", ID, DATA_PAGAMENTO, DATA_ACCREDITO"
            '    Case Else
            'End Select
            'Select Case oRicPagamenti.nTipoStampa '{0=pagamenti;1=emesso non pagato;2=emesso pagato parzialmente;3=emesso pagato in eccesso}
            '    Case 2
            '        cmdMyCommand.CommandText += " HAVING (CAST(DOVUTO-SUM(IMPORTO_PAGATO) AS NUMERIC(9,2))>0.01)"
            '    Case 3
            '        cmdMyCommand.CommandText += " HAVING (CAST(DOVUTO-SUM(IMPORTO_PAGATO) AS NUMERIC(9,2))<0)"
            '    Case Else
            'End Select
            'cmdMyCommand.CommandText += " ORDER BY COGNOME, NOME, CFPIVA, CODICE_CARTELLA"
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(dtMyDati)
            myAdapter.Dispose()
            dvPagamenti = dtMyDati.DefaultView
            Return dvPagamenti

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.GetStampaPagamenti.errore: ", ex)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        Finally
            'dtMyDati.Dispose()
            'cmdMyCommand.Dispose()
            'cmdMyCommand.Connection.Close()
        End Try
    End Function
    Public Function GetStampaRiversamento(ByVal oRicPagamenti As ObjSearchPagamenti, ByVal myStringConnection As String) As DataView
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dtMyDati As New DataTable()
        Dim dvPagamenti As New DataView

        Try
            cmdMyCommand = New SqlClient.SqlCommand
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_GetRiversamento"
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oRicPagamenti.sEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTRIBUTO", SqlDbType.NVarChar)).Value = oRicPagamenti.IdTributo
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Anno", SqlDbType.NVarChar)).Value = oRicPagamenti.sAnnoRif
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COGNOME", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(oRicPagamenti.sCognome) & "%"
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOME", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(oRicPagamenti.sNome) & "%"
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CFPIVA", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(oRicPagamenti.sCFPIVA) & "%"
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODCARTELLA", SqlDbType.NVarChar)).Value = oRicPagamenti.sNAvviso
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DataPagDal", SqlDbType.DateTime)).Value = oRicPagamenti.tDataPagamentoDal
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DataPagAl", SqlDbType.DateTime)).Value = oRicPagamenti.tDataPagamentoAl
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DataDal", SqlDbType.DateTime)).Value = oRicPagamenti.tDataAccreditoDal
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DataAl", SqlDbType.DateTime)).Value = oRicPagamenti.tDataAccreditoAl
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Flusso", SqlDbType.NVarChar)).Value = oRicPagamenti.Flusso
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Provenienza", SqlDbType.NVarChar)).Value = oRicPagamenti.Provenienza
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(dtMyDati)
            myAdapter.Dispose()
            dvPagamenti = dtMyDati.DefaultView
            Return dvPagamenti

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.GetStampaRiversamento.errore: ", ex)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        End Try
    End Function
    Public Function GetStampaQuadratura(ByVal oRicPagamenti As ObjSearchPagamenti, ByVal myStringConnection As String) As DataView
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dtMyDati As New DataTable()
        Dim dvPagamenti As New DataView

        Try
            cmdMyCommand = New SqlClient.SqlCommand
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_GetQuadratura"
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oRicPagamenti.sEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTRIBUTO", SqlDbType.NVarChar)).Value = oRicPagamenti.IdTributo
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Anno", SqlDbType.NVarChar)).Value = oRicPagamenti.sAnnoRif
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COGNOME", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(oRicPagamenti.sCognome) & "%"
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOME", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(oRicPagamenti.sNome) & "%"
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CFPIVA", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(oRicPagamenti.sCFPIVA) & "%"
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODCARTELLA", SqlDbType.NVarChar)).Value = oRicPagamenti.sNAvviso
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DataPagDal", SqlDbType.DateTime)).Value = oRicPagamenti.tDataPagamentoDal
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DataPagAl", SqlDbType.DateTime)).Value = oRicPagamenti.tDataPagamentoAl
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DataDal", SqlDbType.DateTime)).Value = oRicPagamenti.tDataAccreditoDal
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DataAl", SqlDbType.DateTime)).Value = oRicPagamenti.tDataAccreditoAl
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Flusso", SqlDbType.NVarChar)).Value = oRicPagamenti.Flusso
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Provenienza", SqlDbType.NVarChar)).Value = oRicPagamenti.Provenienza

            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(dtMyDati)
            myAdapter.Dispose()
            dvPagamenti = dtMyDati.DefaultView
            Return dvPagamenti

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.GetStampaQuadratura.errore: ", ex)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        End Try
    End Function

    Public Function GetStampaPagamentiNonPres(ByVal oRicPagamenti As ObjSearchPagamenti, ByVal myStringConnection As String) As DataView
        Dim dvPagamenti As New DataView
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dtMyDati As New DataTable()

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_PAG_CARTELLENONPAGATE"
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oRicPagamenti.sEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTRIBUTO", SqlDbType.NVarChar)).Value = oRicPagamenti.IdTributo
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = oRicPagamenti.sAnnoRif
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = oRicPagamenti.IdContribuente
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COGNOME", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(oRicPagamenti.sCognome)
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOME", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(oRicPagamenti.sNome)
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CFPIVA", SqlDbType.NVarChar)).Value = oRicPagamenti.sCFPIVA
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODCARTELLA", SqlDbType.NVarChar)).Value = oRicPagamenti.sNAvviso
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Flusso", SqlDbType.NVarChar)).Value = oRicPagamenti.Flusso
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Provenienza", SqlDbType.NVarChar)).Value = oRicPagamenti.Provenienza
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(dtMyDati)
            myAdapter.Dispose()
            dvPagamenti = dtMyDati.DefaultView
            Return dvPagamenti
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.GetStampaPagamentiNonPres.errore: ", ex)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        Finally
            dtMyDati.Dispose()
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function

    Public Function GetListPagamenti(ByVal oRicPagamenti As ObjSearchPagamenti, myStringConnection As String) As OggettoPagamenti()
        Dim myDataReader As SqlClient.SqlDataReader
        Dim dtMyDati As New DataTable()
        Dim dtMyRow As DataRow
        Dim oListPagamenti As New ArrayList
        Dim oMyPagamento As OggettoPagamenti
        Dim nList As Integer = -1

        Try
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandTimeout = 0
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If

            cmdMyCommand.CommandText = "prc_GetPagamenti"
            'valorizzo i parameters:
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oRicPagamenti.sEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTRIBUTO", SqlDbType.NVarChar)).Value = oRicPagamenti.IdTributo
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@RicPag", SqlDbType.Bit)).Value = oRicPagamenti.bRicPag
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Anno", SqlDbType.NVarChar)).Value = oRicPagamenti.sAnnoRif
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdPag", SqlDbType.Int)).Value = oRicPagamenti.IdPagamento
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdContribuente", SqlDbType.Int)).Value = oRicPagamenti.IdContribuente
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COGNOME", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(oRicPagamenti.sCognome) & "%"
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOME", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(oRicPagamenti.sNome) & "%"
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CFPIVA", SqlDbType.NVarChar)).Value = FncGen.ReplaceCharsForSearch(oRicPagamenti.sCFPIVA) & "%"
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODCARTELLA", SqlDbType.NVarChar)).Value = oRicPagamenti.sNAvviso
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DataPagDal", SqlDbType.DateTime)).Value = oRicPagamenti.tDataPagamentoDal
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DataPagAl", SqlDbType.DateTime)).Value = oRicPagamenti.tDataPagamentoAl
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DataDal", SqlDbType.DateTime)).Value = oRicPagamenti.tDataAccreditoDal
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DataAl", SqlDbType.DateTime)).Value = oRicPagamenti.tDataAccreditoAl
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IsNonAbb", SqlDbType.Bit)).Value = oRicPagamenti.IsNonAbbinati
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Flusso", SqlDbType.NVarChar)).Value = oRicPagamenti.Flusso
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Provenienza", SqlDbType.NVarChar)).Value = oRicPagamenti.Provenienza
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myDataReader = cmdMyCommand.ExecuteReader
            dtMyDati.Load(myDataReader)
            For Each dtMyRow In dtMyDati.Rows
                oMyPagamento = New OggettoPagamenti
                oMyPagamento.ID = CInt(dtMyRow("ID"))
                oMyPagamento.IdContribuente = CStr(dtMyRow("IDCONTRIBUENTE"))
                oMyPagamento.IdEnte = CStr(dtMyRow("IDENTE"))
                oMyPagamento.IDFlusso = CInt(dtMyRow("IDFLUSSO"))
                oMyPagamento.sAnno = CStr(dtMyRow("ANNO"))
                oMyPagamento.sCognome = CStr(dtMyRow("COGNOME"))
                oMyPagamento.sNome = CStr(dtMyRow("NOME"))
                oMyPagamento.sCFPIVA = CStr(dtMyRow("cfpiva"))
                If Not (dtMyRow("CODICE_CARTELLA") Is DBNull.Value) Then
                    oMyPagamento.sNumeroAvviso = CStr(dtMyRow("CODICE_CARTELLA"))
                Else
                    oMyPagamento.sNumeroAvviso = ""
                End If
                If Not (dtMyRow("NUMERO_RATA") Is DBNull.Value) Then
                    oMyPagamento.sNumeroRata = CStr(dtMyRow("NUMERO_RATA"))
                Else
                    oMyPagamento.sNumeroRata = ""
                End If
                If Not IsDBNull(dtMyRow("IMPORTO_RATA")) Then
                    oMyPagamento.dImportoRata = dtMyRow("IMPORTO_RATA")
                Else
                    oMyPagamento.dImportoRata = 0
                End If
                If Not IsDBNull(dtMyRow("IMPORTO_PAGATO")) Then
                    oMyPagamento.dImportoPagamento = dtMyRow("IMPORTO_PAGATO")
                Else
                    oMyPagamento.dImportoPagamento = 0
                End If
                If Not IsDBNull(dtMyRow("IMPORTO_PAGATOSTAT")) Then
                    oMyPagamento.dImportoStat = dtMyRow("IMPORTO_PAGATOSTAT")
                Else
                    oMyPagamento.dImportoStat = 0
                End If
                If Not (dtMyRow("DATA_PAGAMENTO") Is DBNull.Value) Then
                    oMyPagamento.tDataPagamento = CDate(dtMyRow("DATA_PAGAMENTO"))
                Else
                    oMyPagamento.tDataPagamento = Date.MinValue
                End If
                If Not (dtMyRow("DATA_ACCREDITO") Is DBNull.Value) Then
                    oMyPagamento.tDataAccredito = CDate(dtMyRow("DATA_ACCREDITO"))
                Else
                    oMyPagamento.tDataAccredito = Date.MinValue
                End If
                If Not (dtMyRow("DATA_SCADENZA") Is DBNull.Value) Then
                    oMyPagamento.tDataScadenza = CDate(dtMyRow("DATA_SCADENZA"))
                Else
                    oMyPagamento.tDataScadenza = Date.MinValue
                End If
                If Not (dtMyRow("provenienza") Is DBNull.Value) Then
                    oMyPagamento.sProvenienza = CStr(dtMyRow("provenienza"))
                End If
                If Not IsDBNull(dtMyRow("TIPO_PAGAMENTO")) Then
                    oMyPagamento.sTipoPagamento = CStr(dtMyRow("TIPO_PAGAMENTO"))
                Else
                    oMyPagamento.sTipoPagamento = ""
                End If
                If Not IsDBNull(dtMyRow("TIPO_BOLLETTINO")) Then
                    oMyPagamento.sTipoBollettino = CStr(dtMyRow("TIPO_BOLLETTINO"))
                Else
                    oMyPagamento.sTipoBollettino = ""
                End If
                If Not (dtMyRow("NOTE") Is DBNull.Value) Then
                    oMyPagamento.sNote = CStr(dtMyRow("NOTE"))
                Else
                    oMyPagamento.sNote = ""
                End If
                If Not (dtMyRow("OPERATORE") Is DBNull.Value) Then
                    oMyPagamento.sOperatore = CStr(dtMyRow("OPERATORE"))
                Else
                    oMyPagamento.sOperatore = ""
                End If
                'prelevo i dati della testata
                oListPagamenti.Add(oMyPagamento)
            Next

            Return CType(oListPagamenti.ToArray(GetType(OggettoPagamenti)), OggettoPagamenti())
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.GetListPagamenti.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        Finally
            dtMyDati.Dispose()
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function

    Public Function GetListEmessoPag(ByVal oRic As ObjSearchPagamenti, myStringConnection As String, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As OggettoPagamenti()
        Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dtMyDati As New DataTable()
        Dim dtMyRow As DataRow
        Dim oListPagamenti As New ArrayList
        Dim oMyPagamento As OggettoPagamenti
        Dim nList As Integer = -1

        Try
            If (Not (cmdMyCommandOut) Is Nothing) Then
                cmdMyCommand = cmdMyCommandOut
            Else
                cmdMyCommand = New SqlClient.SqlCommand
                cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
                cmdMyCommand.Connection.Open()
            End If
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.AddWithValue("@IdEnte", oRic.sEnte)
            cmdMyCommand.Parameters.AddWithValue("@IdTributo", oRic.IdTributo)
            cmdMyCommand.Parameters.AddWithValue("@Anno", oRic.sAnnoRif)
            cmdMyCommand.Parameters.AddWithValue("@COGNOME", FncGen.ReplaceCharsForSearch(oRic.sCognome) & "%")
            cmdMyCommand.Parameters.AddWithValue("@NOME", FncGen.ReplaceCharsForSearch(oRic.sNome) & "%")
            cmdMyCommand.Parameters.AddWithValue("@CFPIVA", FncGen.ReplaceCharsForSearch(oRic.sCFPIVA) & "%")
            cmdMyCommand.Parameters.AddWithValue("@CODCARTELLA", FncGen.ReplaceCharsForSearch(oRic.sNAvviso) & "%")
            cmdMyCommand.Parameters.AddWithValue("@CODBOLLETTINO", FncGen.ReplaceCharsForSearch(oRic.sCodBollettino) & "%")
            cmdMyCommand.Parameters.AddWithValue("@IMPORTO", oRic.impPagato)
            cmdMyCommand.CommandText = "prc_GetEmessoPagamenti"
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(dtMyDati)
            myAdapter.Dispose()
            For Each dtMyRow In dtMyDati.Rows
                oMyPagamento = New OggettoPagamenti
                oMyPagamento.IdContribuente = CStr(dtMyRow("IDCONTRIBUENTE"))
                oMyPagamento.IdEnte = CStr(dtMyRow("IDENTE"))
                oMyPagamento.sAnno = CStr(dtMyRow("ANNO"))
                oMyPagamento.sCognome = CStr(dtMyRow("COGNOME"))
                oMyPagamento.sNome = CStr(dtMyRow("NOME"))
                oMyPagamento.sCFPIVA = CStr(dtMyRow("cf_piva"))
                If Not (dtMyRow("CODICE_CARTELLA") Is DBNull.Value) Then
                    oMyPagamento.sNumeroAvviso = CStr(dtMyRow("CODICE_CARTELLA"))
                End If
                If Not (dtMyRow("NUMERO_RATA") Is DBNull.Value) Then
                    oMyPagamento.sNumeroRata = CStr(dtMyRow("NUMERO_RATA"))
                End If
                If Not IsDBNull(dtMyRow("IMPORTO_RATA")) Then
                    oMyPagamento.dImportoRata = dtMyRow("IMPORTO_RATA")
                End If
                If Not IsDBNull(dtMyRow("IMPORTO_EMESSO")) Then
                    oMyPagamento.sSegno = FormatNumber(dtMyRow("IMPORTO_EMESSO"), 2) 'uso il campo SEGNO che non serve come appoggio per non dover cambiare l'oggetto
                End If
                If Not IsDBNull(dtMyRow("IMPORTO_PAGATO")) Then
                    oMyPagamento.dImportoPagamento = dtMyRow("IMPORTO_PAGATO")
                End If
                If Not IsDBNull(dtMyRow("IMPORTO_PAGATOSTAT")) Then
                    oMyPagamento.dImportoStat = dtMyRow("IMPORTO_PAGATOSTAT")
                End If
                If Not (dtMyRow("codbollettino") Is DBNull.Value) Then
                    oMyPagamento.sCodBollettino = CStr(dtMyRow("codbollettino"))
                End If
                oMyPagamento.ID = CInt(dtMyRow("ID"))
                If Not (dtMyRow("IDFLUSSO") Is DBNull.Value) Then
                    oMyPagamento.IDFlusso = CInt(dtMyRow("IDFLUSSO")) 'uso il campo IDFLUSSO che non serve come appoggio per l'IDAVVISO per non dover cambiare l'oggetto
                End If
                If Not (dtMyRow("DATA_SCADENZA") Is DBNull.Value) Then
                    oMyPagamento.tDataScadenza = CDate(dtMyRow("DATA_SCADENZA"))
                Else
                    oMyPagamento.tDataScadenza = DateTime.MaxValue
                End If
                'prelevo i dati della testata
                oListPagamenti.Add(oMyPagamento)
            Next

            Return CType(oListPagamenti.ToArray(GetType(OggettoPagamenti)), OggettoPagamenti())
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.GetListEmessoPag.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        Finally
            dtMyDati.Dispose()
            If (cmdMyCommandOut Is Nothing) Then
                cmdMyCommand.Dispose()
                cmdMyCommand.Connection.Close()
            End If
        End Try
    End Function

    Public Function AbbinaPagamento(myStringConnection As String, ByVal sCFPIVA As String, ByVal sIdOperazione As String, ByVal sAnno As String, ByVal tDataPag As String, ByVal nIdContribuente As Integer, ByVal sCodCartella As String, ByVal sCodBollettino As String, ByVal sNRata As String, ByVal sAnnoEmesso As String, DataScadenza As DateTime, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As Boolean
        Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
        Dim MyRet As Integer = -1

        Try
            If (Not (cmdMyCommandOut) Is Nothing) Then
                cmdMyCommand = cmdMyCommandOut
            Else
                cmdMyCommand = New SqlClient.SqlCommand
                cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
                cmdMyCommand.Connection.Open()
            End If
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.AddWithValue("@ID", MyRet)
            cmdMyCommand.Parameters.AddWithValue("@IDENTE", ConstSession.IdEnte)
            cmdMyCommand.Parameters.AddWithValue("@ANNO", sAnno)
            cmdMyCommand.Parameters.AddWithValue("@CFPIVA", sCFPIVA)
            cmdMyCommand.Parameters.AddWithValue("@IDOPERAZIONE", sIdOperazione.Trim)
            cmdMyCommand.Parameters.AddWithValue("@DATAPAGAMENTO", FncGen.FormattaData(tDataPag, "A"))
            cmdMyCommand.Parameters.AddWithValue("@IDCONTRIBUENTE", nIdContribuente)
            cmdMyCommand.Parameters.AddWithValue("@CODCARTELLA", sCodCartella)
            cmdMyCommand.Parameters.AddWithValue("@CODBOLLETTINO", sCodBollettino)
            cmdMyCommand.Parameters.AddWithValue("@NRATA", sNRata)
            cmdMyCommand.Parameters.AddWithValue("@ANNOEMESSO", sAnnoEmesso)
            cmdMyCommand.Parameters.AddWithValue("@OPERATORE", ConstSession.UserName)
            cmdMyCommand.Parameters.AddWithValue("@DATASCADENZA", FncGen.FormattaData(DataScadenza, "A"))
            cmdMyCommand.Parameters("@ID").Direction = ParameterDirection.InputOutput
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_AbbinaPagamento"
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            cmdMyCommand.ExecuteNonQuery()
            MyRet = cmdMyCommand.Parameters("@ID").Value
            If MyRet > 0 Then
                Return True
            Else
                Return False
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.AbbinaPagamento.errore: ", Err)
            Return False
        Finally
            If (cmdMyCommandOut Is Nothing) Then
                cmdMyCommand.Dispose()
                cmdMyCommand.Connection.Close()
            End If
        End Try
    End Function
    ''' <summary>
    ''' Funzione per l'inserimento del pagamento.
    ''' La stored, se richiamata per inserimento, controlla se andare su ordinario piuttosto che su accertamento come segue:
    ''' controllo pagamento entro termine; se entro 10gg dalla scadenza inserisco in pagamenti e cancello eventuale data accertamento ed ingiunzione perchè ho pagato entro i termini; se oltre 10gg dalla scadenza inserisco su provvedimenti.
    ''' La stored, se richiamata per aggiornamento, prima storicizza il record e poi lo inserisce con i nuovi dati.
    ''' </summary>
    ''' <param name="MyStringConnection">string stringa di connessione</param>
    ''' <param name="oMyPagamento">OggettoPagamenti oggetto da gestire</param>
    ''' <param name="nDBOperation">int tipo di operazione {	1=aggiornamento, 2=cancellazione per id flusso, 3=setta come dettagliato, 4=setta come da dettagliare, 5=cancellazione per id pagamento, 6=setta non abbinato come non trattare, altro=inserimento pagamento}</param>
    ''' <returns>int 0= non a buon fine, >0= id tabella</returns>
    Public Function SetPagamento(ByVal MyStringConnection As String, ByVal oMyPagamento As OggettoPagamenti, ByVal nDBOperation As Integer) As Integer
        '{0= non a buon fine, >0= id tabella}
        Dim sSQL As String = ""
        Dim myDataView As New DataView
        Dim nMyReturn As Integer
        Try
            If oMyPagamento.tDataPagamento = Date.MinValue Or oMyPagamento.tDataPagamento = Date.MinValue.ToShortDateString Then
                oMyPagamento.tDataPagamento = Date.MaxValue
            End If
            If oMyPagamento.tDataAccredito = Date.MinValue Or oMyPagamento.tDataAccredito = Date.MinValue.ToShortDateString Then
                oMyPagamento.tDataAccredito = oMyPagamento.tDataPagamento
            End If
            If oMyPagamento.tDataScadenza = Date.MinValue Or oMyPagamento.tDataScadenza = Date.MinValue.ToShortDateString Then
                oMyPagamento.tDataScadenza = Date.MaxValue
            End If
            Using ctx As New DBModel(ConstSession.DBType, MyStringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_TBLPAGAMENTI_IU", "IDPAGAMENTO", "IDFLUSSO", "IDIMPORTAZIONE", "IDCONTRIBUENTE", "IDENTE", "PROVENIENZA", "ANNO", "CODICE_CARTELLA", "NUMERO_RATA", "IMPORTO_RATA", "CODICE_BOLLETTINO", "DATA_SCADENZA", "DATA_PAGAMENTO", "DATA_ACCREDITO", "DIVISA", "IMPORTO_PAGAMENTO", "TIPO_PAGAMENTO", "TIPO_BOLLETTINO", "PROGRESSIVO_CARICAMENTO", "PROGRESSIVO_SELEZIONE", "CCBENEFICIARIO", "UFFICIOSPORTELLO", "FLAGDETTAGLIO", "NOTE", "OPERATORE", "DATA_INSERIMENTO", "CHIAVE", "TYPEOPERATION", "CFPIVA")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDPAGAMENTO", oMyPagamento.ID) _
                            , ctx.GetParam("IDFLUSSO", oMyPagamento.IDFlusso) _
                            , ctx.GetParam("IDIMPORTAZIONE", oMyPagamento.IDImportazione) _
                            , ctx.GetParam("IDCONTRIBUENTE", oMyPagamento.IdContribuente) _
                            , ctx.GetParam("IDENTE", oMyPagamento.IdEnte) _
                            , ctx.GetParam("PROVENIENZA", oMyPagamento.sProvenienza) _
                            , ctx.GetParam("ANNO", oMyPagamento.sAnno) _
                            , ctx.GetParam("CODICE_CARTELLA", oMyPagamento.sNumeroAvviso) _
                            , ctx.GetParam("NUMERO_RATA", oMyPagamento.sNumeroRata) _
                            , ctx.GetParam("IMPORTO_RATA", oMyPagamento.dImportoRata) _
                            , ctx.GetParam("CODICE_BOLLETTINO", oMyPagamento.sCodBollettino) _
                            , ctx.GetParam("DATA_SCADENZA", oMyPagamento.tDataScadenza) _
                            , ctx.GetParam("DATA_PAGAMENTO", oMyPagamento.tDataPagamento) _
                            , ctx.GetParam("DATA_ACCREDITO", oMyPagamento.tDataAccredito) _
                            , ctx.GetParam("DIVISA", oMyPagamento.sDivisa) _
                            , ctx.GetParam("IMPORTO_PAGAMENTO", oMyPagamento.dImportoPagamento) _
                            , ctx.GetParam("TIPO_PAGAMENTO", oMyPagamento.sTipoPagamento) _
                            , ctx.GetParam("TIPO_BOLLETTINO", oMyPagamento.sTipoBollettino) _
                            , ctx.GetParam("PROGRESSIVO_CARICAMENTO", oMyPagamento.sProgCaricamento) _
                            , ctx.GetParam("PROGRESSIVO_SELEZIONE", oMyPagamento.sProgSelezione) _
                            , ctx.GetParam("CCBENEFICIARIO", oMyPagamento.sCCBeneficiario) _
                            , ctx.GetParam("UFFICIOSPORTELLO", oMyPagamento.sUfficioSportello) _
                            , ctx.GetParam("FLAGDETTAGLIO", oMyPagamento.bFlagDettaglio) _
                            , ctx.GetParam("NOTE", oMyPagamento.sNote) _
                            , ctx.GetParam("OPERATORE", oMyPagamento.sOperatore) _
                            , ctx.GetParam("DATA_INSERIMENTO", Now) _
                            , ctx.GetParam("CHIAVE", "") _
                            , ctx.GetParam("TYPEOPERATION", nDBOperation) _
                            , ctx.GetParam("CFPIVA", oMyPagamento.sCFPIVA)
                        )
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsElabRuolo.CalcoloRate.SetRateDettaglio.errore: ", ex)
                    nMyReturn = -1
                Finally
                    ctx.Dispose()
                End Try
                For Each myRow As DataRowView In myDataView
                    nMyReturn = Utility.StringOperation.FormatInt(myRow("id"))
                Next
            End Using
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.SetPagamento.errore: ", Err)
        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            nMyReturn = -1
        Finally
        myDataView.Dispose()
        End Try
        Return nMyReturn
    End Function
    Public Function SetPagamentoTransito(ByVal MyStringConnection As String, ByVal oMyPagamento As OggettoPagamenti, sTributo As String, ByVal nDBOperation As Integer) As Integer
        '{0= non a buon fine, >0= id tabella}
        Dim nMyReturn As Integer

        Try
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(MyStringConnection)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            'valorizzo il CommandText:
            cmdMyCommand.CommandText = "prc_TBLPAGAMENTITRANSITO_IU"
            'valorizzo i parameters:
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TYPEOPERATION", SqlDbType.Int)).Value = nDBOperation
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPAGAMENTO", SqlDbType.Int)).Value = oMyPagamento.ID
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDIMPORTAZIONE", SqlDbType.Int)).Value = oMyPagamento.IDImportazione
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSO", SqlDbType.Int)).Value = oMyPagamento.IDFlusso
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oMyPagamento.IdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTRIBUTO", SqlDbType.NVarChar)).Value = sTributo
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = oMyPagamento.IdContribuente
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PROVENIENZA", SqlDbType.NVarChar)).Value = oMyPagamento.sProvenienza
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = oMyPagamento.sAnno
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE_CARTELLA", SqlDbType.NVarChar)).Value = oMyPagamento.sNumeroAvviso
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CFPIVA", SqlDbType.NVarChar)).Value = oMyPagamento.sCFPIVA
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE_BOLLETTINO", SqlDbType.NVarChar)).Value = oMyPagamento.sCodBollettino
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO_RATA", SqlDbType.NVarChar)).Value = oMyPagamento.sNumeroRata
            If oMyPagamento.tDataPagamento = Date.MinValue Or oMyPagamento.tDataPagamento = Date.MinValue.ToShortDateString Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_PAGAMENTO", SqlDbType.DateTime)).Value = DBNull.Value
            Else
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_PAGAMENTO", SqlDbType.DateTime)).Value = oMyPagamento.tDataPagamento
            End If
            If oMyPagamento.tDataAccredito = Date.MinValue Or oMyPagamento.tDataAccredito = Date.MinValue.ToShortDateString Then
                If oMyPagamento.tDataPagamento = Date.MinValue Or oMyPagamento.tDataPagamento = Date.MinValue.ToShortDateString Then
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_ACCREDITO", SqlDbType.DateTime)).Value = DBNull.Value
                Else
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_ACCREDITO", SqlDbType.DateTime)).Value = oMyPagamento.tDataPagamento
                End If
            Else
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_ACCREDITO", SqlDbType.DateTime)).Value = oMyPagamento.tDataAccredito
            End If
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DIVISA", SqlDbType.NVarChar)).Value = oMyPagamento.sDivisa
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO_PAGAMENTO", SqlDbType.Float)).Value = oMyPagamento.dImportoPagamento
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPO_BOLLETTINO", SqlDbType.NVarChar)).Value = oMyPagamento.sTipoBollettino
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPO_PAGAMENTO", SqlDbType.NVarChar)).Value = oMyPagamento.sTipoPagamento
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PROGRESSIVO_CARICAMENTO", SqlDbType.NVarChar)).Value = oMyPagamento.sProgCaricamento
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PROGRESSIVO_SELEZIONE", SqlDbType.NVarChar)).Value = oMyPagamento.sProgSelezione
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CCBENEFICIARIO", SqlDbType.NVarChar)).Value = oMyPagamento.sCCBeneficiario
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@UFFICIOSPORTELLO", SqlDbType.NVarChar)).Value = oMyPagamento.sUfficioSportello
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOTE", SqlDbType.NVarChar)).Value = oMyPagamento.sNote
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FLAGDETTAGLIO", SqlDbType.Bit)).Value = oMyPagamento.bFlagDettaglio
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@OPERATORE", SqlDbType.NVarChar)).Value = oMyPagamento.sOperatore
            If oMyPagamento.tDataInsert = Date.MinValue Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INSERIMENTO", SqlDbType.DateTime)).Value = Now
            Else
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INSERIMENTO", SqlDbType.DateTime)).Value = oMyPagamento.tDataInsert
            End If
            cmdMyCommand.Parameters("@IDPAGAMENTO").Direction = ParameterDirection.InputOutput
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            cmdMyCommand.ExecuteNonQuery()
            nMyReturn = cmdMyCommand.Parameters("@IDPAGAMENTO").Value
            Return nMyReturn
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.SetPagamentoTransito.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return -1
        Finally
            cmdMyCommand.Connection.Close()
        End Try
    End Function

    Public Function CheckPagVSCartella(myStringConnection As String, ByVal nTipo As Integer, IdTributo As String, ByVal oMyPagamento As OggettoPagamenti) As Boolean
        Dim DrDati As SqlClient.SqlDataReader = Nothing

        Try
            cmdMyCommand.CommandType = CommandType.Text
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            Select Case nTipo
                Case 0 'controllo che l'importo pagato sia uguale alla rata emessa
                    cmdMyCommand.CommandText = "SELECT *"
                    cmdMyCommand.CommandText += " FROM V_PAG_CARTELLEPAGAMENTI"
                    cmdMyCommand.CommandText += " WHERE (IDENTE=@IDENTE)"
                    cmdMyCommand.CommandText += " AND (COD_CONTRIBUENTE=@IDCONTRIBUENTE)"
                    cmdMyCommand.CommandText += " AND (CODICE_CARTELLA=@CODCARTELLA)"
                    cmdMyCommand.CommandText += " AND (ANNO=@ANNO)"
                    cmdMyCommand.CommandText += " AND (@IDTRIBUTO='' OR IDTRIBUTO=@IDTRIBUTO)"
                    cmdMyCommand.CommandText += " AND (NUMERO_RATA=@NRATA)"
                    'valorizzo i parameters:
                    cmdMyCommand.Parameters.Clear()
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oMyPagamento.IdEnte
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = oMyPagamento.IdContribuente
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODCARTELLA", SqlDbType.NVarChar)).Value = oMyPagamento.sNumeroAvviso
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = oMyPagamento.sAnno
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTRIBUTO", SqlDbType.NVarChar)).Value = IdTributo
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NRATA", SqlDbType.NVarChar)).Value = oMyPagamento.sNumeroRata
                    Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                    DrDati = cmdMyCommand.ExecuteReader
                    Do While DrDati.Read
                        If Not IsDBNull(DrDati("importo_rata")) Then
                            If CDbl(DrDati("importo_rata")) < oMyPagamento.dImportoPagamento Then
                                Return False
                            Else
                                Return True
                            End If
                        Else
                            Return True
                        End If
                    Loop
                Case 1 'controllo che l'importo totale pagato non superi l'importo dovuto
                    cmdMyCommand.CommandText = "SELECT CODICE_CARTELLA, DOVUTO, SUM(IMPORTO_PAGAMENTO) AS PAGATO"
                    cmdMyCommand.CommandText += " FROM V_PAG_CARTELLEPAGAMENTI"
                    cmdMyCommand.CommandText += " WHERE (IDENTE=@IDENTE)"
                    cmdMyCommand.CommandText += " AND (COD_CONTRIBUENTE=@IDCONTRIBUENTE)"
                    cmdMyCommand.CommandText += " AND (CODICE_CARTELLA=@CODCARTELLA)"
                    cmdMyCommand.CommandText += " AND (ANNO=@ANNO)"
                    cmdMyCommand.CommandText += " AND (@IDTRIBUTO='' OR IDTRIBUTO=@IDTRIBUTO)"
                    cmdMyCommand.CommandText += " GROUP BY CODICE_CARTELLA, DOVUTO"
                    'valorizzo i parameters:
                    cmdMyCommand.Parameters.Clear()
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oMyPagamento.IdEnte
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = oMyPagamento.IdContribuente
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODCARTELLA", SqlDbType.NVarChar)).Value = oMyPagamento.sNumeroAvviso
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = oMyPagamento.sAnno
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTRIBUTO", SqlDbType.NVarChar)).Value = IdTributo
                    Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                    DrDati = cmdMyCommand.ExecuteReader
                    Do While DrDati.Read
                        If Not IsDBNull(DrDati("pagato")) Then
                            If CDbl(DrDati("dovuto")) < (CDbl(DrDati("pagato")) + oMyPagamento.dImportoPagamento) Then
                                Return False
                            Else
                                Return True
                            End If
                        Else
                            If CDbl(DrDati("dovuto")) < oMyPagamento.dImportoPagamento Then
                                Return False
                            Else
                                Return True
                            End If
                        End If
                    Loop
            End Select

            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.CheckPagVSCartella.errore: ", ex)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return False
        Finally
            DrDati.Close()
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function

    Public Function GetCartellePagamenti(myStringConnection As String, ByVal sIdEnte As String, IdTributo As String, ByVal nIdContribuente As Integer, ByVal sCodCartella As String, ByVal sAnno As String) As OggettoPagamenti()
        Dim DrDati As SqlClient.SqlDataReader = Nothing
        Dim oMyPag As OggettoPagamenti
        Dim oListPag() As OggettoPagamenti = Nothing
        Dim nList As Integer = -1

        Try
            cmdMyCommand.CommandType = CommandType.Text
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myStringConnection)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            cmdMyCommand.CommandText = "SELECT *"
            cmdMyCommand.CommandText += " FROM V_PAG_CARTELLEPAGAMENTI"
            cmdMyCommand.CommandText += " WHERE (IDENTE=@IDENTE)"
            cmdMyCommand.CommandText += " AND (@IDTRIBUTO='' OR IDTRIBUTO=@IDTRIBUTO)"
            'valorizzo i parameters:
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTRIBUTO", SqlDbType.NVarChar)).Value = IdTributo
            If sCodCartella <> "" Then
                cmdMyCommand.CommandText += " AND (CODICE_CARTELLA=@CODCARTELLA)"
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODCARTELLA", SqlDbType.NVarChar)).Value = sCodCartella
            End If
            If sAnno <> "" Then
                cmdMyCommand.CommandText += " AND (ANNO=@ANNO)"
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = sAnno
            End If
            If nIdContribuente <> -1 Then
                cmdMyCommand.CommandText += " AND (COD_CONTRIBUENTE=@IDCONTRIBUENTE)"
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = nIdContribuente
            End If
            cmdMyCommand.CommandText += " ORDER BY ANNO DESC, DATA_EMISSIONE DESC, CODICE_CARTELLA, ID, NUMERO_RATA, DATA_SCADENZA"
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            DrDati = cmdMyCommand.ExecuteReader
            Do While DrDati.Read
                oMyPag = New OggettoPagamenti
                If Not IsDBNull(DrDati("ID_PAGAMENTO")) Then
                    oMyPag.ID = CInt(DrDati("ID_PAGAMENTO"))
                End If
                If Not IsDBNull(DrDati("COD_CONTRIBUENTE")) Then
                    oMyPag.IdContribuente = CInt(DrDati("COD_CONTRIBUENTE"))
                End If
                If Not IsDBNull(DrDati("CODICE_CARTELLA")) Then
                    oMyPag.sNumeroAvviso = CStr(DrDati("CODICE_CARTELLA"))
                End If
                If Not IsDBNull(DrDati("ANNO")) Then
                    oMyPag.sAnno = CStr(DrDati("ANNO"))
                End If
                If Not IsDBNull(DrDati("NUMERO_RATA")) Then
                    oMyPag.sNumeroRata = CStr(DrDati("NUMERO_RATA"))
                End If
                If Not IsDBNull(DrDati("IMPORTO_RATA")) Then
                    oMyPag.dImportoRata = CDbl(DrDati("IMPORTO_RATA"))
                End If
                If Not IsDBNull(DrDati("DATA_SCADENZA")) Then
                    oMyPag.tDataScadenza = CDate(DrDati("DATA_SCADENZA"))
                End If
                If Not IsDBNull(DrDati("DATA_PAGAMENTO")) Then
                    oMyPag.tDataPagamento = CDate(DrDati("DATA_PAGAMENTO"))
                End If
                If Not IsDBNull(DrDati("DATA_ACCREDITO")) Then
                    oMyPag.tDataAccredito = CDate(DrDati("DATA_ACCREDITO"))
                End If
                If Not IsDBNull(DrDati("IMPORTO_PAGAMENTO")) Then
                    oMyPag.dImportoPagamento = CDbl(DrDati("IMPORTO_PAGAMENTO"))
                Else
                    oMyPag.dImportoPagamento = oMyPag.dImportoRata
                End If

                nList += 1
                ReDim Preserve oListPag(nList)
                oListPag(nList) = oMyPag
            Loop

            Return oListPag
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.GetCartellePagamenti.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        Finally
            DrDati.Close()
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function
    '*** ***
#End Region

#Region "Importazione Flussi"
    'Public Sub ImportPagamenti(ByVal sParametroENV As String, ByVal sUsername As String, ByVal sIdentificativoApplicazione As String, ByVal sEnteImport As String, ByVal sFileImport As String, ByVal sPathImportOK As String, ByVal sPathImportKO As String, ByVal sContoCorrente As String, ByVal sOperatore As String, ByVal nIDFlussoImport As Integer)
    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Dim WFErrore As String = ""
    '    Dim oMyTotAcq As New ObjImportPagamenti
    '    Dim nCheckFile As Integer
    '    Dim oMyCondizioni() As ObjCondizioniScarto

    '    Try
    '        Dim oMyFileInfo = New System.IO.FileInfo(sFileImport)
    '        Dim sNameImport As String = oMyFileInfo.Name
    '        Dim sPathNameScarti As String = sPathImportOK + "SCARTI_" & sNameImport
    '        'controllo se il file esiste già
    '        If System.IO.File.Exists(sPathImportOK + "SCARTI_" & sNameImport) Then
    '            'se si lo elimino
    '            System.IO.File.Delete(sPathImportOK + "SCARTI_" & sNameImport)
    '        End If

    '        'inizializzo la connessione
    '        WFSessione = New OPENUtility.CreateSessione(sParametroENV, sUsername, sIdentificativoApplicazione)
    '        If Not WFSessione.CreaSessione(sUsername, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        'controllo che il formato sia corretto
    '        nCheckFile = CheckFilePagamenti(sNameImport, sContoCorrente, WFSessione)
    '        Select Case nCheckFile
    '            Case -1 'errore
    '                'controllo se il file esiste già
    '                If System.IO.File.Exists(sPathImportKO + sNameImport) Then
    '                    'se si lo elimino
    '                    System.IO.File.Delete(sPathImportKO + sNameImport)
    '                End If
    '                'sposto il file nella cartella non acquisiti
    '                System.IO.File.Copy(sFileImport, sPathImportKO + sNameImport)
    '                System.IO.File.Delete(sFileImport)
    '                'registro l'errore acquisizione
    '                oMyTotAcq.Id = nIDFlussoImport
    '                oMyTotAcq.IdEnte = sEnteImport
    '                oMyTotAcq.sFileAcq = sFileImport
    '                oMyTotAcq.nStatoAcq = -1
    '                oMyTotAcq.sEsito = "Errore durante l'importazione: Errore nei controlli formali di Acquisizione."
    '                oMyTotAcq.tDataAcq = Now
    '                oMyTotAcq.sOperatore = sOperatore
    '                'registro l'esito d'acquisizione sulla base dati
    '                SetAcquisizione(oMyTotAcq, 1, WFSessione)
    '            Case 0 'formato non corretto
    '                'controllo se il file esiste già
    '                If System.IO.File.Exists(sPathImportKO + sNameImport) Then
    '                    'se si lo elimino
    '                    System.IO.File.Delete(sPathImportKO + sNameImport)
    '                End If
    '                'sposto il file nella cartella non acquisiti
    '                System.IO.File.Copy(sFileImport, sPathImportKO + sNameImport)
    '                System.IO.File.Delete(sFileImport)
    '                'registro il formato non corretto di acquisizione
    '                oMyTotAcq.Id = nIDFlussoImport
    '                oMyTotAcq.IdEnte = sEnteImport
    '                oMyTotAcq.sFileAcq = sFileImport
    '                oMyTotAcq.nStatoAcq = -1
    '                oMyTotAcq.sEsito = "Errore durante l'importazione: Formato di Acquisizione non valido e/o Conto corrente non coerente."
    '                oMyTotAcq.tDataAcq = Now
    '                oMyTotAcq.sOperatore = sOperatore
    '                'registro l'esito d'acquisizione sulla base dati
    '                SetAcquisizione(oMyTotAcq, 1, WFSessione)
    '            Case 1 'formato corretto
    '                'prelevo le condizioni di scarto
    '                oMyCondizioni = GetCondizioniScarto(sContoCorrente, WFSessione)
    '                If Not IsNothing(oMyCondizioni) Then
    '                    'richiamo l'importazione sulla base dati
    '                    If AcquisisciPagamenti(sEnteImport, sFileImport, sNameImport, sPathNameScarti, nIDFlussoImport, sOperatore, oMyCondizioni, WFSessione, oMyTotAcq) <= 0 Then
    '                        'elimino i pagamenti acquisiti
    '                        DeleteImportPagamenti(nIDFlussoImport, WFSessione)
    '                        'controllo se il file esiste già
    '                        If System.IO.File.Exists(sPathImportKO + sNameImport) Then
    '                            'se si lo elimino
    '                            System.IO.File.Delete(sPathImportKO + sNameImport)
    '                        End If
    '                        'sposto il file nella cartella non acquisiti
    '                        System.IO.File.Copy(sFileImport, sPathImportKO + sNameImport)
    '                        System.IO.File.Delete(sFileImport)
    '                        'registro l'errore acquisizione
    '                        oMyTotAcq.Id = nIDFlussoImport
    '                        oMyTotAcq.IdEnte = sEnteImport
    '                        oMyTotAcq.sFileAcq = sFileImport
    '                        oMyTotAcq.nStatoAcq = -1
    '                        oMyTotAcq.sEsito = "Errore durante l'importazione."
    '                        oMyTotAcq.tDataAcq = Now
    '                        oMyTotAcq.sOperatore = sOperatore
    '                        'registro l'esito d'acquisizione sulla base dati
    '                        SetAcquisizione(oMyTotAcq, 1, WFSessione)
    '                    Else
    '                        'controllo se il file esiste già
    '                        If System.IO.File.Exists(sPathImportOK + sNameImport) Then
    '                            'se si lo elimino
    '                            System.IO.File.Delete(sPathImportOK + sNameImport)
    '                        End If
    '                        'sposto il file nella cartella acquisiti
    '                        System.IO.File.Copy(sFileImport, sPathImportOK + sNameImport)
    '                        System.IO.File.Delete(sFileImport)
    '                        'registro l'avvenuta acquisizione
    '                        oMyTotAcq.Id = nIDFlussoImport
    '                        oMyTotAcq.IdEnte = sEnteImport
    '                        oMyTotAcq.sFileAcq = sFileImport
    '                        oMyTotAcq.nStatoAcq = 1
    '                        oMyTotAcq.sEsito = "Acquisizione terminata con successo! Dettaglio Pagamenti in corso!"
    '                        oMyTotAcq.tDataAcq = Now
    '                        oMyTotAcq.sOperatore = sOperatore
    '                        'registro l'esito d'acquisizione sulla base dati
    '                        SetAcquisizione(oMyTotAcq, 1, WFSessione)
    '                        '*** 20131104 - TARES ***
    '                        ''se il richiamo al dettaglio dei pagamenti attraverso la funzione DettagliaPagamenti(sEnteImport, sOperatore, WFSessione) è negativo:
    '                        'If DettagliaPagamenti(sEnteImport, sOperatore, WFSessione) <= 0 Then
    '                        '    'elimino i pagamenti acquisiti
    '                        '    DeleteImportPagamenti(nIDFlussoImport, WFSessione)
    '                        '    'registro nell’oggetto di riepilogo importazione l'errore di dettaglio;
    '                        '    oMyTotAcq.Id = nIDFlussoImport
    '                        '    oMyTotAcq.IdEnte = sEnteImport
    '                        '    oMyTotAcq.sFileAcq = sFileImport
    '                        '    oMyTotAcq.nStatoAcq = -1
    '                        '    oMyTotAcq.sEsito = "Errore durante il Dettaglio dei Pagamenti."
    '                        '    oMyTotAcq.tDataAcq = Now
    '                        '    oMyTotAcq.sOperatore = sOperatore
    '                        '    'registro l'esito d'acquisizione sulla base dati;
    '                        '    SetAcquisizione(oMyTotAcq, 1, WFSessione)
    '                        'Else
    '                        '    'registro nell’oggetto di riepilogo importazione l'avvenuto dettaglio;
    '                        '    oMyTotAcq.Id = nIDFlussoImport
    '                        '    oMyTotAcq.IdEnte = sEnteImport
    '                        '    oMyTotAcq.sFileAcq = sFileImport
    '                        '    oMyTotAcq.nStatoAcq = 0
    '                        '    oMyTotAcq.sEsito = "Importazione terminata con successo!"
    '                        '    oMyTotAcq.tDataAcq = Now
    '                        '    oMyTotAcq.sOperatore = sOperatore
    '                        '    'registro l'esito d'acquisizione sulla base dati;
    '                        '    SetAcquisizione(oMyTotAcq, 1, WFSessione)
    '                        'End If
    '                        'se il richiamo al dettaglio dei pagamenti attraverso la funzione DettagliaPagamenti(sEnteImport, sOperatore, WFSessione) è negativo:
    '                        If DettagliaPagamenti(sEnteImport, WFSessione.oSession.oAppDB.GetConnection.ConnectionString) <= 0 Then
    '                            'elimino i pagamenti acquisiti
    '                            'DeleteImportPagamenti(nIDFlussoImport, WFSessione)
    '                            'registro nell’oggetto di riepilogo importazione l'errore di dettaglio;
    '                            oMyTotAcq.Id = nIDFlussoImport
    '                            oMyTotAcq.IdEnte = sEnteImport
    '                            oMyTotAcq.sFileAcq = sFileImport
    '                            oMyTotAcq.nStatoAcq = -1
    '                            oMyTotAcq.sEsito = "Errore durante il Dettaglio dei Pagamenti."
    '                            oMyTotAcq.tDataAcq = Now
    '                            oMyTotAcq.sOperatore = sOperatore
    '                            'registro l'esito d'acquisizione sulla base dati;
    '                            SetAcquisizione(oMyTotAcq, 1, WFSessione)
    '                        Else
    '                            'registro nell’oggetto di riepilogo importazione l'avvenuto dettaglio;
    '                            oMyTotAcq.Id = nIDFlussoImport
    '                            oMyTotAcq.IdEnte = sEnteImport
    '                            oMyTotAcq.sFileAcq = sFileImport
    '                            oMyTotAcq.nStatoAcq = 0
    '                            oMyTotAcq.sEsito = "Importazione terminata con successo!"
    '                            oMyTotAcq.tDataAcq = Now
    '                            oMyTotAcq.sOperatore = sOperatore
    '                            'registro l'esito d'acquisizione sulla base dati;
    '                            SetAcquisizione(oMyTotAcq, 1, WFSessione)
    '                        End If
    '                        '*** ***
    '                    End If
    '                Else
    '                    'controllo se il file esiste già
    '                    If System.IO.File.Exists(sPathImportKO + sNameImport) Then
    '                        'se si lo elimino
    '                        System.IO.File.Delete(sPathImportKO + sNameImport)
    '                    End If
    '                    'sposto il file nella cartella non acquisiti
    '                    System.IO.File.Copy(sFileImport, sPathImportKO + sNameImport)
    '                    System.IO.File.Delete(sFileImport)
    '                    'registro l'errore acquisizione
    '                    oMyTotAcq.Id = nIDFlussoImport
    '                    oMyTotAcq.IdEnte = sEnteImport
    '                    oMyTotAcq.sFileAcq = sFileImport
    '                    oMyTotAcq.nStatoAcq = -1
    '                    oMyTotAcq.sEsito = "Errore durante l'importazione." + vbCrLf + "Le condizioni di scarto non sono correttamente configurate."
    '                    oMyTotAcq.tDataAcq = Now
    '                    oMyTotAcq.sOperatore = sOperatore
    '                    'registro l'esito d'acquisizione sulla base dati
    '                    SetAcquisizione(oMyTotAcq, 1, WFSessione)
    '                End If
    '            Case 2 'file già acquisito
    '                'controllo se il file esiste già
    '                If System.IO.File.Exists(sPathImportKO + sNameImport) Then
    '                    'se si lo elimino
    '                    System.IO.File.Delete(sPathImportKO + sNameImport)
    '                End If
    '                'sposto il file nella cartella non acquisiti
    '                System.IO.File.Copy(sFileImport, sPathImportKO + sNameImport)
    '                System.IO.File.Delete(sFileImport)
    '                'registro la mancanza di dati obbligatori
    '                oMyTotAcq.Id = nIDFlussoImport
    '                oMyTotAcq.IdEnte = sEnteImport
    '                oMyTotAcq.sFileAcq = sFileImport
    '                oMyTotAcq.nStatoAcq = -1
    '                oMyTotAcq.sEsito = "Errore durante l'importazione: File già acquisito."
    '                oMyTotAcq.tDataAcq = Now
    '                oMyTotAcq.sOperatore = sOperatore
    '                'registro l'esito d'acquisizione sulla base dati
    '                SetAcquisizione(oMyTotAcq, 1, WFSessione)
    '        End Select
    '    Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsGestPag.ImportaPagamenti.errore: ", Err)
    '        'registro l'errore acquisizione
    '        oMyTotAcq.Id = nIDFlussoImport
    '        oMyTotAcq.IdEnte = sEnteImport
    '        oMyTotAcq.sFileAcq = sFileImport
    '        oMyTotAcq.nStatoAcq = -1
    '        oMyTotAcq.sEsito = "Errore durante l'importazione."
    '        oMyTotAcq.tDataAcq = Now
    '        oMyTotAcq.sOperatore = sOperatore
    '        'registro l'esito d'acquisizione sulla base dati
    '        SetAcquisizione(oMyTotAcq, 1, WFSessione)
    '    Finally
    '        WFSessione.Kill()
    '    End Try
    'End Sub

    'Private Function CheckFilePagamenti(ByVal sNameFilePagamenti As String, ByVal sContoCorrente As String, ByVal WFSessione As OPENUtility.CreateSessione) As Integer
    '    '{1= formato corretto; 0= formato non corretto; 2= file già acquisito; -1= errore}
    '    Dim oMyFlussi() As ObjFlussoPagamenti

    '    Try
    '        'non è un file txt
    '        If Not sNameFilePagamenti.ToLower.EndsWith(".txt") Then
    '            Return 0
    '        End If
    '        'non è dell’ente in esame
    '        Log.Debug("ClsGestPag::CheckFilePagamenti:: conto corrente da file::" & sNameFilePagamenti.Substring(21, 8).Trim & "::conto corrente da db::" & sContoCorrente)
    '        If sNameFilePagamenti.Substring(21, 8).Trim <> sContoCorrente Then
    '            Return 0
    '        End If
    '        'il file è già stato acquisito
    '        oMyFlussi = GetFlussi(sNameFilePagamenti, WFSessione)
    '        If Not oMyFlussi Is Nothing Then
    '            Return 2
    '        End If
    '        Return 1
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsGestPag.CheckFilePagamenti.errore: ", Err)
    '        Return -1
    '    End Try
    'End Function

    'Private Function AcquisisciPagamenti(ByVal sIdEnte As String, ByVal sPathNameMyFile As String, ByVal sNameMyFile As String, ByVal sPathNameScarti As String, ByVal nIdImport As Integer, ByVal sOperatore As String, ByVal oMyCondScarto() As ObjCondizioniScarto, ByVal WFSessione As OPENUtility.CreateSessione, ByRef oRiepilogoAcq As ObjImportPagamenti) As Integer
    '    '{-1= non a buon fine, 1= buon fine}
    '    Dim oMyPagamento As OggettoPagamenti
    '    Dim oMyFlusso As ObjFlussoPagamenti
    '    Dim oMyFile As IO.StreamReader
    '    Dim nIdFlusso As Integer = -1
    '    Dim x, nPagAcq, nPagScarti As Integer
    '    Dim impPagAcq, impPagScarti As Double
    '    Dim sLineFile As String

    '    Try
    '        'apro il file
    '        oMyFile = New IO.StreamReader(sPathNameMyFile)
    '        Try
    '            Do
    '                'leggo la riga
    '                sLineFile = oMyFile.ReadLine
    '                If Not IsNothing(sLineFile) Then
    '                    'controllo su che tipo di record mi trovo
    '                    Select Case sLineFile.Substring(33, 3)
    '                        'sono su un record di coda
    '                        Case "999"
    '                            oMyFlusso = New ObjFlussoPagamenti
    '                            'prelevo i dati dal file
    '                            oMyFlusso.ID = nIdFlusso
    '                            oMyFlusso.IDImportazione = nIdImport
    '                            oMyFlusso.IDEnte = sIdEnte
    '                            oMyFlusso.sProvenienza = "POSTECC"
    '                            oMyFlusso.sOperatore = sOperatore
    '                            oMyFlusso.tDataAcq = Now
    '                            oMyFlusso.sFileAcq = sNameMyFile
    '                            oMyFlusso.sCodCUAS = sLineFile.Substring(0, 1)
    '                            oMyFlusso.sContoCorrente = sLineFile.Substring(1, 12)
    '                            oMyFlusso.nRcAcquisiti = sLineFile.Substring(36, 8)
    '                            oMyFlusso.impAcquisiti = Double.Parse(sLineFile.Substring(44, 12)) / 100
    '                            oMyFlusso.nPagamentiEsatti = sLineFile.Substring(56, 8)
    '                            oMyFlusso.impPagamentiEsatti = CDbl(sLineFile.Substring(64, 12)) / 100
    '                            oMyFlusso.nPagamentiErrati = sLineFile.Substring(76, 8)
    '                            oMyFlusso.impPagamentiErrati = CDbl(sLineFile.Substring(84, 12)) / 100
    '                            'se il numero di pagamenti totali è diverso da quelli scartati
    '                            If nPagScarti <> oMyFlusso.nRcAcquisiti Then
    '                                'registro il flusso come acquisito
    '                                If SetFlusso(oMyFlusso, 0, WFSessione) <= 0 Then
    '                                    Return -1
    '                                End If
    '                            End If
    '                            'incremento le variabili di totalizzazione importazione
    '                            oRiepilogoAcq.nRcAcquisiti += nPagAcq
    '                            oRiepilogoAcq.impAcquisiti += impPagAcq
    '                            oRiepilogoAcq.nRcScarti += nPagScarti
    '                            oRiepilogoAcq.impScarti += impPagScarti
    '                            'svuoto le variabiali riferite al singolo flusso
    '                            nPagAcq = 0 : nPagScarti = 0 : impPagAcq = 0 : impPagScarti = 0 : nIdFlusso = -1

    '                            'sono su un record di pagamento
    '                        Case Else
    '                            oMyPagamento = New OggettoPagamenti
    '                            'controllo se devo prelevare l'identificativo flusso
    '                            If nIdFlusso = -1 Then
    '                                'prelevo l'identificativo flusso
    '                                nIdFlusso = GetNewIdFlusso(sIdEnte, WFSessione)
    '                                If nIdFlusso <= 0 Then
    '                                    Return -1
    '                                End If
    '                            End If
    '                            'setto le variabili generali del pagamento
    '                            oMyPagamento.IDImportazione = nIdImport
    '                            oMyPagamento.IDFlusso = nIdFlusso
    '                            oMyPagamento.sProvenienza = "POSTECC"
    '                            oMyPagamento.sTipoPagamento = "RA"
    '                            oMyPagamento.sOperatore = sOperatore
    '                            oMyPagamento.tDataInsert = Now
    '                            'prelevo i dati dal file
    '                            oMyPagamento.sTipoBollettino = sLineFile.Substring(33, 3)
    '                            oMyPagamento.sCodBollettino = sLineFile.Substring(61, 16)
    '                            oMyPagamento.IdEnte = sIdEnte
    '                            oMyPagamento.sProgCaricamento = sLineFile.Substring(0, 8)
    '                            oMyPagamento.sProgSelezione = sLineFile.Substring(8, 7)
    '                            oMyPagamento.sCCBeneficiario = sLineFile.Substring(15, 12)
    '                            'oMyPagamento.tDataPagamento = FncGen.FormattaData("20" & sLineFile.Substring(27, 6), "G")
    '                            oMyPagamento.tDataPagamento = DateTime.ParseExact(FncGen.FormattaData("20" & sLineFile.Substring(27, 6), "G"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)

    '                            oMyPagamento.dImportoPagamento = CDbl(sLineFile.Substring(36, 10)) / 100
    '                            oMyPagamento.sUfficioSportello = sLineFile.Substring(46, 8)
    '                            oMyPagamento.sDivisa = "E" 'sLineFile.Substring(54, 1)
    '                            'oMyPagamento.tDataAccredito = FncGen.FormattaData("20" & sLineFile.Substring(55, 6), "G")
    '                            oMyPagamento.tDataPagamento = DateTime.ParseExact(FncGen.FormattaData("20" & sLineFile.Substring(55, 6), "G"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)

    '                            oMyPagamento.bFlagDettaglio = False
    '                            'incremento la variabile dei record presenti sul file
    '                            oRiepilogoAcq.nRcDaAcquisire += 1
    '                            oRiepilogoAcq.impDaAcquisire += oMyPagamento.dImportoPagamento
    '                            'controllo le condizioni di scarto in base al tipo bollettino che sto trattando
    '                            For x = 0 To oMyCondScarto.GetUpperBound(0)
    '                                If oMyPagamento.sTipoBollettino = oMyCondScarto(x).sTipoBollettino Then
    '                                    Select Case oMyCondScarto(x).sTipoScarto
    '                                        'il pagamento deve essere abbinato tramite il codice bollettino per essere acquisito
    '                                        Case 1
    '                                            'cerco se trovo l'abbinamento
    '                                            If AbbinaBollettino(oMyPagamento, WFSessione) = False Then
    '                                                'registro la presenza del file di scarti
    '                                                oRiepilogoAcq.sFileScarti = sPathNameScarti
    '                                                'non ho trovato quindi incremento le variabili degli scarti
    '                                                nPagScarti += 1
    '                                                impPagScarti += oMyPagamento.dImportoPagamento
    '                                                'scrivo il record sul file degli scarti
    '                                                If WriteFileScarti(oMyPagamento, sPathNameMyFile, sPathNameScarti) <= 0 Then
    '                                                    Return -1
    '                                                End If
    '                                            Else
    '                                                'ho trovato quindi registro il pagamento sulla base dati
    '                                                If SetPagamento(oMyPagamento, 0, WFSessione) <= 0 Then
    '                                                    Return -1
    '                                                End If
    '                                                'incremento le variabili dei pagamenti acquisiti
    '                                                nPagAcq += 1
    '                                                impPagAcq += oMyPagamento.dImportoPagamento
    '                                            End If
    '                                            'il pagamento deve essere messo su file di scarti
    '                                        Case 2
    '                                            'registro la presenza del file di scarti
    '                                            oRiepilogoAcq.sFileScarti = sPathNameScarti
    '                                            'incremento le variabili degli scarti
    '                                            nPagScarti += 1
    '                                            impPagScarti += oMyPagamento.dImportoPagamento
    '                                            'scrivo il record sul file degli scarti
    '                                            If WriteFileScarti(oMyPagamento, sPathNameMyFile, sPathNameScarti) <= 0 Then
    '                                                Return -1
    '                                            End If
    '                                            'il pagamento non deve essere acquisito
    '                                        Case 3
    '                                            'decremento le variabili di importazione
    '                                            oRiepilogoAcq.nRcDaAcquisire -= 1
    '                                            oRiepilogoAcq.impDaAcquisire -= oMyPagamento.dImportoPagamento
    '                                    End Select
    '                                End If
    '                            Next
    '                    End Select
    '                End If
    '            Loop Until sLineFile Is Nothing
    '        Catch Err As Exception
    '            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsGestPag.AcquisisciPagamenti.errore: ", Err)
    '            Return -1
    '        Finally
    '            oMyFile.Close()
    '        End Try
    '        Return 1
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsGestPag.AcquisisciPagamenti.errore: ", Err)
    '        Return -1
    '    End Try
    'End Function

    'Private Sub DeleteImportPagamenti(ByVal nIdImport As Integer, ByVal WFSessione As OPENUtility.CreateSessione)
    '    Dim oRiepImport As New ObjImportPagamenti
    '    Dim oRiepFlusso As New ObjFlussoPagamenti
    '    Dim oRiepPag As New OggettoPagamenti

    '    Try
    '        'valorizzo l'id importazione da eliminare
    '        oRiepImport.Id = nIdImport
    '        oRiepFlusso.IDImportazione = nIdImport
    '        oRiepPag.IDImportazione = nIdImport
    '        'svuoto la base dati
    '        SetAcquisizione(oRiepImport, 2, WFSessione)
    '        SetFlusso(oRiepFlusso, 2, WFSessione)
    '        SetPagamento(oRiepPag, 2, WFSessione)

    '    Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsGestPag.DeleteImportaPagamenti.errore: ", Err)
    '    End Try
    'End Sub

    'Public Function CheckImportazione(ByVal sIdEnte As String, ByVal WFSessione As OPENUtility.CreateSessione) As Integer
    '    Dim oMyTotAcq As New ObjImportPagamenti

    '    Try
    '        'controllare se è in elaborazione 
    '        oMyTotAcq = GetAcquisizione(sIdEnte, 1, WFSessione)
    '        If Not oMyTotAcq Is Nothing Then
    '            If oMyTotAcq.Id <> -1 Then
    '                Return 1
    '            Else
    '                Return 0
    '            End If
    '            Return 1
    '        Else
    '            Return 0
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsGestPag.CheckImportaPagamenti.errore: ", Err)
    '        Return -1
    '    End Try
    'End Function

    'Public Function PrelevaImportazione(ByVal sIdEnte As String, ByVal nIsFirstTime As Integer, ByVal WFSessione As OPENUtility.CreateSessione) As ObjImportPagamenti
    '    Dim oMyTotAcq As New ObjImportPagamenti
    '    Dim nMaxId As Integer

    '    Try
    '        'verifica se l'elaborazione è terminata
    '        nMaxId = MaxIdImport(sIdEnte, WFSessione)
    '        If Not IsDBNull(nMaxId) Then
    '            'prelevo i dati dalla tabella dei flussi
    '            oMyTotAcq = GetAcquisizione(sIdEnte, nIsFirstTime, WFSessione)
    '            If Not oMyTotAcq Is Nothing Then
    '                If oMyTotAcq.Id <> nMaxId Or nMaxId = -1 Then
    '                    'controlla se l'elaborazione è terminata con errori
    '                    oMyTotAcq = GetAcquisizione(sIdEnte, -1, WFSessione)
    '                End If
    '            Else
    '                'controlla se l'elaborazione è terminata con errori
    '                oMyTotAcq = GetAcquisizione(sIdEnte, -1, WFSessione)
    '            End If
    '        End If
    '        Return oMyTotAcq
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsGestPag.PrelevaImportazioni.errore: ", Err)
    '        Return Nothing
    '    End Try
    'End Function

    'Public Function MaxIdImport(ByVal sIdEnte As String, ByVal WFSessione As OPENUtility.CreateSessione) As Integer
    '    Dim DrReturn As SqlClient.SqlDataReader
    '    Dim myIdentity As Integer = -1

    '    Try
    '        cmdMyCommand.CommandText = "SELECT MAX(ID) AS MAXID"
    '        cmdMyCommand.CommandText += " FROM TBLIMPORTPAGAMENTI"
    '        cmdMyCommand.CommandText += " WHERE (TBLIMPORTPAGAMENTI.IDENTE = @IDENTE)"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE ", SqlDbType.NVarChar)).Value = sIdEnte
    '        DrReturn = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '        Do While DrReturn.Read
    '            If Not IsDBNull(DrReturn("maxid")) Then
    '                myIdentity = CInt(DrReturn("maxid"))
    '            End If
    '        Loop
    '        Return myIdentity
    '    Catch Err As Exception
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsGestPag.MaxIdImport.errore: ", Err)
    '        Dim sValParametri As String = Utility.Costanti.GetValParamCmd(cmdMyCommand)
    '        Log.Debug("Si è verificato un errore in ClsPagamenti::MaxIdImport::" & Err.Message & vbCrLf & "SQL::" & cmdMyCommand.CommandText & vbCrLf & " VALUES " & sValParametri)
    '        Return -1
    '    Finally
    '        DrReturn.Close()
    '    End Try
    'End Function

    'Public Function GetAcquisizione(ByVal sIDEnte As String, ByVal nStato As Integer, ByVal WFSessione As OPENUtility.CreateSessione) As ObjImportPagamenti
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
    '        DrReturn = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
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
    '        Dim sValParametri As String = Utility.Costanti.GetValParamCmd(cmdMyCommand)
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsGestPag.GetAcquisizione.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in ClsPagamenti::GetAcquisizione::" & Err.Message & vbCrLf & "SQL::" & cmdMyCommand.CommandText & vbCrLf & " VALUES " & sValParametri)
    '        Return Nothing
    '    Finally
    '        DrReturn.Close()
    '    End Try
    'End Function

    'Public Function SetAcquisizione(ByVal oMyImport As ObjImportPagamenti, ByVal nDBOperation As Integer, ByVal WFSessione As OPENUtility.CreateSessione) As Integer
    '    Dim nMyReturn As Integer

    '    Try
    '        '*** 20130415 - sbagliava in caso di annullo totale ***
    '        cmdMyCommand.CommandType = CommandType.Text
    '        '*** ***
    '        'Valorizzo la connessione
    '        cmdMyCommand.Connection = WFSessione.oSession.oAppDB.GetConnection
    '        'Valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDIMPORT", SqlDbType.NVarChar)).Value = oMyImport.Id
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oMyImport.IdEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FILE_IMPORT", SqlDbType.NVarChar)).Value = oMyImport.sFileAcq
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@STATO_IMPORTAZIONE", SqlDbType.Int)).Value = oMyImport.nStatoAcq
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ESITO", SqlDbType.NVarChar)).Value = FncGen.ReplaceChar(oMyImport.sEsito)
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
    '                DrReturn = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
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
    '                nMyReturn = WFSessione.oSession.oAppDB.Execute(cmdMyCommand)
    '            Case 2
    '                cmdMyCommand.CommandText = "DELETE"
    '                cmdMyCommand.CommandText += " FROM TBLIMPORTPAGAMENTI"
    '                cmdMyCommand.CommandText += " WHERE (TBLIMPORTPAGAMENTI.ID=@IDIMPORT)"
    '                nMyReturn = WFSessione.oSession.oAppDB.Execute(cmdMyCommand)
    '        End Select

    '        Return nMyReturn
    '    Catch Err As Exception
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsGestPag.SetAcquisizione.errore: ", Err)
    '        Dim sValParametri As String = Utility.Costanti.GetValParamCmd(cmdMyCommand)
    '        Log.Debug("Si è verificato un errore in ClsPagamenti::SetAcquisizione::" & Err.Message & vbCrLf & "SQL::" & cmdMyCommand.CommandText & vbCrLf & " VALUES " & sValParametri)
    '        Return -1
    '    End Try
    'End Function

    'Public Function GetCondizioniScarto(ByVal sContoCorrente As String, ByVal WFSessione As OPENUtility.CreateSessione) As ObjCondizioniScarto()
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
    '        DrMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
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
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsGestPag.GetCondizioniScarto.errore: ", Err)
    '        Dim sValParametri As String = Utility.Costanti.GetValParamCmd(cmdMyCommand)
    '        Log.Debug("Si è verificato un errore in ClsPagamenti::GetCondizioniScarto::" & Err.Message & vbCrLf & "SQL::" & cmdMyCommand.CommandText & vbCrLf & " VALUES " & sValParametri)
    '        Return Nothing
    '    Finally
    '        DrMyDati.Close()
    '    End Try
    'End Function

    'Public Function GetFlussi(ByVal sFileImport As String, ByVal WFSessione As OPENUtility.CreateSessione) As ObjFlussoPagamenti()
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
    '            cmdMyCommand.CommandText += " WHERE (NOME_FILE=@NOMEFILE)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOMEFILE ", SqlDbType.NVarChar)).Value = sFileImport
    '        End If
    '        DrMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
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
    '        Dim sValParametri As String = Utility.Costanti.GetValParamCmd(cmdMyCommand)
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsGestPag.GetFlussi.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in ClsPagamenti::GetFlussi::" & Err.Message & vbCrLf & "SQL::" & cmdMyCommand.CommandText & vbCrLf & " VALUES " & sValParametri)
    '        Return Nothing
    '    Finally
    '        DrMyDati.Close()
    '    End Try
    'End Function

    'Public Function GetNewIdFlusso(ByVal sMyIdEnte As String, ByVal WFSessione As OPENUtility.CreateSessione) As Integer
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
    '        DrMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '        Do While DrMyDati.Read
    '            nMyReturn = CInt(DrMyDati("id"))
    '        Loop
    '        nMyReturn += 1
    '        Return nMyReturn
    '    Catch Err As Exception
    '        Dim sValParametri As String = Utility.Costanti.GetValParamCmd(cmdMyCommand)
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsGestPag.GetNewIdFlusso.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in ClsPagamenti::GetNewIdFlusso::" & Err.Message & vbCrLf & "SQL::" & cmdMyCommand.CommandText & vbCrLf & " VALUES " & sValParametri)
    '        Return -1
    '    Finally
    '        DrMyDati.Close()
    '    End Try
    'End Function

    'Public Function SetFlusso(ByVal oMyFlusso As ObjFlussoPagamenti, ByVal nDBOperation As Integer, ByVal WFSessione As OPENUtility.CreateSessione) As Integer
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
    '                nMyReturn = WFSessione.oSession.oAppDB.Execute(cmdMyCommand)
    '            Case 2
    '                'valorizzo il commandtext:
    '                cmdMyCommand.CommandText = "DELETE"
    '                cmdMyCommand.CommandText += " FROM TBLDESCRIZIONEFLUSSIPAG"
    '                cmdMyCommand.CommandText += " WHERE (TBLDESCRIZIONEFLUSSIPAG.IDIMPORT=@IDIMPORT)"
    '                nMyReturn = WFSessione.oSession.oAppDB.Execute(cmdMyCommand)
    '        End Select
    '        Return nMyReturn
    '    Catch Err As Exception
    '        Dim sValParametri As String = Utility.Costanti.GetValParamCmd(cmdMyCommand)
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsGestPag.SetFlusso.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in ClsPagamenti::SetFlusso::" & Err.Message & vbCrLf & "SQL::" & cmdMyCommand.CommandText & vbCrLf & " VALUES " & sValParametri)
    '        Return -1
    '    End Try
    'End Function

    'Public Function GetPagamento(ByVal sIdEnte As String, ByVal WFSessione As OPENUtility.CreateSessione) As DataView
    '    Try
    '        Dim DvMyDati As New DataView
    '        cmdMyCommand.Connection = WFSessione.oSession.oAppDB.GetConnection

    '        '*** 20130415 - sbagliava in caso di annullo totale ***
    '        'valorizzo il CommandText;
    '        'cmdMyCommand.CommandText = "SELECT TBLPAGAMENTI.ID, TBLPAGAMENTI.IDENTE, TBLPAGAMENTI.CODICE_CARTELLA"
    '        'cmdMyCommand.CommandText += ", TBLPAGAMENTI.IMPORTO_PAGAMENTO AS IMPORTO_PAGAMENTO"
    '        'cmdMyCommand.CommandText += ", TBLCARTELLE.IMPORTO_CARICO"
    '        'cmdMyCommand.CommandText += " FROM TBLPAGAMENTI"
    '        'cmdMyCommand.CommandText += " INNER JOIN TBLCARTELLE ON TBLPAGAMENTI.IDENTE=TBLCARTELLE.IDENTE AND TBLPAGAMENTI.CODICE_CARTELLA=TBLCARTELLE.CODICE_CARTELLA"
    '        'cmdMyCommand.CommandText += " WHERE (FLAG_DETTAGLIO=0)"
    '        'cmdMyCommand.CommandText += " AND (TBLPAGAMENTI.IDENTE=@IDENTE)"
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = "prc_GetPagamentiDaDettagliare"
    '        '*** ***
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE ", SqlDbType.NVarChar)).Value = sIdEnte
    '        DvMyDati = WFSessione.oSession.oAppDB.GetPrivateDataview(cmdMyCommand)
    '        Return DvMyDati
    '    Catch Err As Exception
    '        Dim sValParametri As String = Utility.Costanti.GetValParamCmd(cmdMyCommand)
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsGestPag.GetPagamento.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in ClsPagamenti::GetPagamento::" & Err.Message & vbCrLf & "SQL::" & cmdMyCommand.CommandText & vbCrLf & " VALUES " & sValParametri)
    '        Return Nothing
    '    End Try
    'End Function

    'Public Function GetDettaglioEmesso(ByVal sCodCartella As String, ByVal WFSessione As OPENUtility.CreateSessione) As SqlClient.SqlDataReader
    '    Try
    '        Dim DrMyDati As SqlClient.SqlDataReader
    '        '*** 20130415 - sbagliava in caso di annullo totale ***
    '        'valorizzo il CommandText;
    '        'cmdMyCommand.CommandText = "SELECT *"
    '        'cmdMyCommand.CommandText += " FROM V_PAG_GETDETTAGLIOEMESSO"
    '        'cmdMyCommand.CommandText += " WHERE (CODICE_CARTELLA=@CODICECARTELLA)"
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = "prc_GetDettaglioEmesso"
    '        'valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICECARTELLA ", SqlDbType.NVarChar)).Value = sCodCartella
    '        DrMyDati = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)

    '        Return DrMyDati
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsGestPag.GetDettaglioEmesso.errore: ", Err)
    '        Return Nothing
    '    End Try
    'End Function

    'Public Function SetPagamentoDettagliato(ByVal oListMyDettaglio() As ObjDettaglioPagamento, ByVal WFSessione As OPENUtility.CreateSessione) As Integer
    '    '{0= non a buon fine, >0= id tabella}
    '    Try
    '        Dim x As Integer
    '        'Ciclo sull'array di oggetti di dettaglio:
    '        For x = 0 To oListMyDettaglio.GetUpperBound(0)
    '            'Se l'inserimento su base dati richiamando la funzione SetDettaglioPagamento(oListDettaglio(x)) non è andato a buon fine:
    '            If SetDettaglioPagamento(oListMyDettaglio(x), 0, WFSessione) <= 0 Then
    '                'Restituisco errore;
    '                Return 0
    '            End If
    '        Next
    '        Return 1
    '    Catch Err As Exception
    '        Dim sValParametri As String = Utility.Costanti.GetValParamCmd(cmdMyCommand)
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsGestPag.SetPagamentoDettagliato.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in ClsPagamenti::SetPagamentoDettagliato::" & Err.Message & vbCrLf & "SQL::" & cmdMyCommand.CommandText & vbCrLf & " VALUES " & sValParametri)
    '        Return -1
    '    End Try
    'End Function

    'Public Function SetDettaglioPagamento(ByVal oMyDettaglio As ObjDettaglioPagamento, ByVal nDBOperation As Integer, ByVal WFSessione As OPENUtility.CreateSessione) As Integer
    '    Dim nMyReturn As Integer
    '    Try
    '        '*** 20130415 - sbagliava in caso di annullo totale ***
    '        cmdMyCommand.CommandType = CommandType.Text
    '        '*** ***
    '        'valorizzo il CommandText;
    '        Select Case nDBOperation
    '            Case 0
    '                cmdMyCommand.CommandText = "INSERT INTO TBLDETTAGLIOPAGAMENTI"
    '                cmdMyCommand.CommandText += " (IDPAGAMENTO, COD_CAPITOLO, IDCATEGORIA, ANNO_RUOLO, DIVISA, IMPORTO, OPERATORE, DATA_INSERIMENTO)"
    '                cmdMyCommand.CommandText += " VALUES (@IDPAGAMENTO, @CODICECAPITOLO, @IDCATEGORIA, @ANNORUOLO, @DIVISA, @IMPORTO, @OPERATORE, @DATAINSERIMENTO)"
    '                cmdMyCommand.CommandText += " SELECT @@IDENTITY"
    '                'Valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPAGAMENTO", SqlDbType.Int)).Value = oMyDettaglio.IDPagamento
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICECAPITOLO", SqlDbType.NVarChar)).Value = oMyDettaglio.sCodCapitolo
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCATEGORIA", SqlDbType.NVarChar)).Value = oMyDettaglio.sIdCategoria
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNORUOLO", SqlDbType.NVarChar)).Value = oMyDettaglio.sAnno
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DIVISA", SqlDbType.NVarChar)).Value = oMyDettaglio.sDivisa
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO", SqlDbType.Float)).Value = oMyDettaglio.dImpDettaglio
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAINSERIMENTO", SqlDbType.DateTime)).Value = oMyDettaglio.tDataInsert
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@OPERATORE", SqlDbType.NVarChar)).Value = oMyDettaglio.sOperatore
    '                'eseguo la query
    '                Dim DrReturn As SqlClient.SqlDataReader
    '                DrReturn = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '                Do While DrReturn.Read
    '                    nMyReturn = DrReturn(0)
    '                Loop
    '                DrReturn.Close()
    '            Case 2
    '                cmdMyCommand.CommandText = "DELETE"
    '                cmdMyCommand.CommandText += " FROM TBLDETTAGLIOPAGAMENTI"
    '                cmdMyCommand.CommandText += " WHERE (IDPAGAMENTO=@IDPAGAMENTO)"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPAGAMENTO", SqlDbType.Int)).Value = oMyDettaglio.IDPagamento
    '                nMyReturn = CInt(WFSessione.oSession.oAppDB.Execute(cmdMyCommand))
    '        End Select
    '        Return nMyReturn
    '    Catch Err As Exception
    '        Dim sValParametri As String = Utility.Costanti.GetValParamCmd(cmdMyCommand)
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsGestPag.SetDettagliPagamento.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in ClsPagamenti::SetDettaglioPagamento::" & Err.Message & vbCrLf & "SQL::" & cmdMyCommand.CommandText & vbCrLf & " VALUES " & sValParametri)
    '        Return -1
    '    End Try
    'End Function

    Public Sub ImportF24(ByVal IdEnte As String, ByVal Belfiore As String, ByVal PercorsoF24 As String, ByVal PercorsoDestF24 As String, ByVal nFlussoImport As Integer, ByVal MyFileName As String, ByVal CodTributo As String, ByVal UserName As String, ByVal StringConnectionICI As String, ByVal StringConnectionTARSU As String)
        Dim sMyErr As String = ""
        Dim sAvanzamento As String
        Dim oMyTotAcq As New ObjImportPagamenti

        Try
            'Memorizzo nel CacheManager la Stringa da aggiungere al Div "DivAttesa".
            sAvanzamento = " Avanzamento Controlli"
            CacheManager.SetAvanzamentoElaborazione(sAvanzamento)
            'metodo che effettua i controlli sul file e salva i dati nella tabella TAB_ACQUISIZIONE_F24
            Log.Debug("PercorsoF24, MyFileName, StringConnectionICI, UserName::" & PercorsoF24 & "::" & MyFileName & "::" & StringConnectionICI & "::" & UserName)
            Dim messaggio As String = New ImportazioneF24.ImporterF24().AvviaImportazione(PercorsoF24, MyFileName, StringConnectionICI, UserName, sMyErr)
            Log.Debug("ho acquisito F24 ora devo abbinare su tributo")
            If messaggio = "ok" Then
                Dim RetVal As Boolean = New ImportazioneF24.ImporterF24().ImportSuTributo(IdEnte, CodTributo, StringConnectionICI, PercorsoF24, MyFileName, PercorsoDestF24, Belfiore, UserName, nFlussoImport, sMyErr)
                If RetVal = False Then
                    'registro l'errore acquisizione
                    oMyTotAcq.Id = nFlussoImport
                    oMyTotAcq.IdEnte = IdEnte
                    oMyTotAcq.sFileAcq = MyFileName
                    oMyTotAcq.nStatoAcq = -1
                    oMyTotAcq.sEsito = "Errore durante l'importazione - ImportSuTributo::" & sMyErr
                    oMyTotAcq.tDataAcq = Now
                    oMyTotAcq.sOperatore = UserName
                    'registro l'esito d'acquisizione sulla base dati
                    Log.Debug("Errore durante l'importazione - ImportSuTributo::" & sMyErr)
                    SetAcquisizione(oMyTotAcq, 3, StringConnectionTARSU)
                Else
                    If DettagliaPagamenti(IdEnte, StringConnectionTARSU) <= 0 Then
                        Log.Debug(IdEnte + " - OPENgovTIA.ClsGestPag.ImportF24.errore durante DettagliaPagamenti")
                        '    'registro l'errore acquisizione
                        '    oMyTotAcq.Id = nFlussoImport
                        '    oMyTotAcq.IdEnte = IdEnte
                        '    oMyTotAcq.sFileAcq = MyFileName
                        '    oMyTotAcq.nStatoAcq = -1
                        '    oMyTotAcq.sEsito = "Errore durante l'importazione - DettagliaPagamenti."
                        '    oMyTotAcq.tDataAcq = Now
                        '    oMyTotAcq.sOperatore = UserName
                        '    'registro l'esito d'acquisizione sulla base dati
                        '    Log.Debug("Errore durante l'importazione - DettagliaPagamenti.")
                        '    SetAcquisizione(oMyTotAcq, 3, StringConnectionTARSU)
                        'Else
                    End If
                    'registro nell’oggetto di riepilogo importazione l'avvenuto dettaglio;
                    oMyTotAcq.Id = nFlussoImport
                    oMyTotAcq.IdEnte = IdEnte
                    oMyTotAcq.sFileAcq = MyFileName
                    oMyTotAcq.nStatoAcq = 0
                    oMyTotAcq.sEsito = "Importazione terminata con successo!"
                    oMyTotAcq.tDataAcq = Now
                    oMyTotAcq.sOperatore = UserName
                    'registro l'esito d'acquisizione sulla base dati;
                    Log.Debug("Importazione terminata con successo")
                    SetAcquisizione(oMyTotAcq, 3, StringConnectionTARSU)
                End If
            Else
                'registro l'errore acquisizione
                oMyTotAcq.Id = nFlussoImport
                oMyTotAcq.IdEnte = IdEnte
                oMyTotAcq.sFileAcq = MyFileName
                oMyTotAcq.nStatoAcq = -1
                oMyTotAcq.sEsito = "Errore durante l'importazione::" & sMyErr
                oMyTotAcq.tDataAcq = Now
                oMyTotAcq.sOperatore = UserName
                'registro l'esito d'acquisizione sulla base dati
                Log.Debug("Errore durante l'importazione - AvviaImportazione::" & sMyErr)
                SetAcquisizione(oMyTotAcq, 3, StringConnectionTARSU)
            End If

        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.ImportF24.errore: ", Err)
            'registro l'errore acquisizione
            oMyTotAcq.Id = nFlussoImport
            oMyTotAcq.IdEnte = IdEnte
            oMyTotAcq.sFileAcq = MyFileName
            oMyTotAcq.nStatoAcq = -1
            oMyTotAcq.sEsito = "Errore durante l'importazione."
            oMyTotAcq.tDataAcq = Now
            oMyTotAcq.sOperatore = UserName
            'registro l'esito d'acquisizione sulla base dati
            SetAcquisizione(oMyTotAcq, 3, StringConnectionTARSU)
        Finally
            CacheManager.RemoveAvanzamentoElaborazione()
        End Try

    End Sub
    Public Sub ImportPagamenti(ByVal sEnteImport As String, sTributo As String, ByVal sFileImport As String, ByVal sPathImportOK As String, ByVal sPathImportKO As String, ByVal sContoCorrente As String, ByVal sOperatore As String, ByVal nIDFlussoImport As Integer, ByVal myConnectionString As String, bTransitoPag As Boolean)
        Dim oMyTotAcq As New ObjImportPagamenti
        Dim nCheckFile As Integer
        Dim sAvanzamento As String
        Dim oMyCondizioni() As ObjCondizioniScarto

        Try

            'Memorizzo nel CacheManager la Stringa da aggiungere al Div "DivAttesa".
            sAvanzamento = " Avanzamento Controlli"
            CacheManager.SetAvanzamentoElaborazione(sAvanzamento)

            Dim oMyFileInfo As New System.IO.FileInfo(sFileImport)
            Dim sNameImport As String = oMyFileInfo.Name
            Dim sPathNameScarti As String = sPathImportOK + "SCARTI_" & sNameImport
            'controllo se il file esiste già
            If System.IO.File.Exists(sPathImportOK + "SCARTI_" & sNameImport) Then
                'se si lo elimino
                System.IO.File.Delete(sPathImportOK + "SCARTI_" & sNameImport)
            End If

            'controllo che il formato sia corretto
            nCheckFile = CheckFilePagamenti(sNameImport, sContoCorrente, myConnectionString)
            Select Case nCheckFile
                Case -1 'errore
                    'controllo se il file esiste già
                    If System.IO.File.Exists(sPathImportKO + sNameImport) Then
                        'se si lo elimino
                        System.IO.File.Delete(sPathImportKO + sNameImport)
                    End If
                    'sposto il file nella cartella non acquisiti
                    System.IO.File.Copy(sFileImport, sPathImportKO + sNameImport)
                    System.IO.File.Delete(sFileImport)
                    'registro l'errore acquisizione
                    oMyTotAcq.Id = nIDFlussoImport
                    oMyTotAcq.IdEnte = sEnteImport
                    oMyTotAcq.sFileAcq = sFileImport
                    oMyTotAcq.nStatoAcq = -1
                    oMyTotAcq.sEsito = "Errore durante l'importazione: Errore nei controlli formali di Acquisizione."
                    oMyTotAcq.tDataAcq = Now
                    oMyTotAcq.sOperatore = sOperatore
                    'registro l'esito d'acquisizione sulla base dati
                    SetAcquisizione(oMyTotAcq, 1, myConnectionString)
                Case 0 'formato non corretto
                    'controllo se il file esiste già
                    If System.IO.File.Exists(sPathImportKO + sNameImport) Then
                        'se si lo elimino
                        System.IO.File.Delete(sPathImportKO + sNameImport)
                    End If
                    'sposto il file nella cartella non acquisiti
                    System.IO.File.Copy(sFileImport, sPathImportKO + sNameImport)
                    System.IO.File.Delete(sFileImport)
                    'registro il formato non corretto di acquisizione
                    oMyTotAcq.Id = nIDFlussoImport
                    oMyTotAcq.IdEnte = sEnteImport
                    oMyTotAcq.sFileAcq = sFileImport
                    oMyTotAcq.nStatoAcq = -1
                    oMyTotAcq.sEsito = "Errore durante l'importazione: Formato di Acquisizione non valido e/o Conto corrente non coerente."
                    oMyTotAcq.tDataAcq = Now
                    oMyTotAcq.sOperatore = sOperatore
                    'registro l'esito d'acquisizione sulla base dati
                    SetAcquisizione(oMyTotAcq, 1, myConnectionString)
                Case 1 'formato corretto
                    'prelevo le condizioni di scarto
                    oMyCondizioni = GetCondizioniScarto(sContoCorrente, myConnectionString)
                    If Not IsNothing(oMyCondizioni) Then
                        'richiamo l'importazione sulla base dati
                        If AcquisisciPagamenti(sEnteImport, sTributo, sFileImport, sNameImport, sPathNameScarti, nIDFlussoImport, sOperatore, oMyCondizioni, myConnectionString, bTransitoPag, oMyTotAcq) <= 0 Then
                            'elimino i pagamenti acquisiti
                            DeleteImportPagamenti(nIDFlussoImport, myConnectionString)
                            'controllo se il file esiste già
                            If System.IO.File.Exists(sPathImportKO + sNameImport) Then
                                'se si lo elimino
                                System.IO.File.Delete(sPathImportKO + sNameImport)
                            End If
                            'sposto il file nella cartella non acquisiti
                            System.IO.File.Copy(sFileImport, sPathImportKO + sNameImport)
                            System.IO.File.Delete(sFileImport)
                            'registro l'errore acquisizione
                            oMyTotAcq.Id = nIDFlussoImport
                            oMyTotAcq.IdEnte = sEnteImport
                            oMyTotAcq.sFileAcq = sFileImport
                            oMyTotAcq.nStatoAcq = -1
                            oMyTotAcq.sEsito = "Errore durante l'importazione."
                            oMyTotAcq.tDataAcq = Now
                            oMyTotAcq.sOperatore = sOperatore
                            'registro l'esito d'acquisizione sulla base dati
                            SetAcquisizione(oMyTotAcq, 1, myConnectionString)
                        Else
                            'controllo se il file esiste già
                            If System.IO.File.Exists(sPathImportOK + sNameImport) Then
                                'se si lo elimino
                                System.IO.File.Delete(sPathImportOK + sNameImport)
                            End If
                            'sposto il file nella cartella acquisiti
                            System.IO.File.Copy(sFileImport, sPathImportOK + sNameImport)
                            System.IO.File.Delete(sFileImport)
                            'registro l'avvenuta acquisizione
                            oMyTotAcq.Id = nIDFlussoImport
                            oMyTotAcq.IdEnte = sEnteImport
                            oMyTotAcq.sFileAcq = sFileImport
                            oMyTotAcq.nStatoAcq = 1
                            oMyTotAcq.sEsito = "Acquisizione terminata con successo! Dettaglio Pagamenti in corso!"
                            oMyTotAcq.tDataAcq = Now
                            oMyTotAcq.sOperatore = sOperatore
                            'registro l'esito d'acquisizione sulla base dati
                            SetAcquisizione(oMyTotAcq, 1, myConnectionString)
                            '*** 20131104 - TARES ***
                            'se il richiamo al dettaglio dei pagamenti attraverso la funzione DettagliaPagamenti(sEnteImport, sOperatore, WFSessione) è negativo:
                            If DettagliaPagamenti(sEnteImport, myConnectionString) <= 0 Then
                                'elimino i pagamenti acquisiti
                                'DeleteImportPagamenti(nIDFlussoImport, WFSessione)
                                'registro nell’oggetto di riepilogo importazione l'errore di dettaglio;
                                oMyTotAcq.Id = nIDFlussoImport
                                oMyTotAcq.IdEnte = sEnteImport
                                oMyTotAcq.sFileAcq = sFileImport
                                oMyTotAcq.nStatoAcq = 0
                                oMyTotAcq.sEsito = "Importazione terminata con successo! Errore durante il Dettaglio dei Pagamenti."
                                oMyTotAcq.tDataAcq = Now
                                oMyTotAcq.sOperatore = sOperatore
                                'registro l'esito d'acquisizione sulla base dati;
                                SetAcquisizione(oMyTotAcq, 1, myConnectionString)
                            Else
                                'registro nell’oggetto di riepilogo importazione l'avvenuto dettaglio;
                                oMyTotAcq.Id = nIDFlussoImport
                                oMyTotAcq.IdEnte = sEnteImport
                                oMyTotAcq.sFileAcq = sFileImport
                                oMyTotAcq.nStatoAcq = 0
                                oMyTotAcq.sEsito = "Importazione terminata con successo!"
                                oMyTotAcq.tDataAcq = Now
                                oMyTotAcq.sOperatore = sOperatore
                                'registro l'esito d'acquisizione sulla base dati;
                                SetAcquisizione(oMyTotAcq, 1, myConnectionString)
                            End If
                            '*** ***
                        End If
                    Else
                        'controllo se il file esiste già
                        If System.IO.File.Exists(sPathImportKO + sNameImport) Then
                            'se si lo elimino
                            System.IO.File.Delete(sPathImportKO + sNameImport)
                        End If
                        'sposto il file nella cartella non acquisiti
                        System.IO.File.Copy(sFileImport, sPathImportKO + sNameImport)
                        System.IO.File.Delete(sFileImport)
                        'registro l'errore acquisizione
                        oMyTotAcq.Id = nIDFlussoImport
                        oMyTotAcq.IdEnte = sEnteImport
                        oMyTotAcq.sFileAcq = sFileImport
                        oMyTotAcq.nStatoAcq = -1
                        oMyTotAcq.sEsito = "Errore durante l'importazione." + vbCrLf + "Le condizioni di scarto non sono correttamente configurate."
                        oMyTotAcq.tDataAcq = Now
                        oMyTotAcq.sOperatore = sOperatore
                        'registro l'esito d'acquisizione sulla base dati
                        SetAcquisizione(oMyTotAcq, 1, myConnectionString)
                    End If
                Case 2 'file già acquisito
                    'controllo se il file esiste già
                    If System.IO.File.Exists(sPathImportKO + sNameImport) Then
                        'se si lo elimino
                        System.IO.File.Delete(sPathImportKO + sNameImport)
                    End If
                    'sposto il file nella cartella non acquisiti
                    System.IO.File.Copy(sFileImport, sPathImportKO + sNameImport)
                    System.IO.File.Delete(sFileImport)
                    'registro la mancanza di dati obbligatori
                    oMyTotAcq.Id = nIDFlussoImport
                    oMyTotAcq.IdEnte = sEnteImport
                    oMyTotAcq.sFileAcq = sFileImport
                    oMyTotAcq.nStatoAcq = -1
                    oMyTotAcq.sEsito = "Errore durante l'importazione: File già acquisito."
                    oMyTotAcq.tDataAcq = Now
                    oMyTotAcq.sOperatore = sOperatore
                    'registro l'esito d'acquisizione sulla base dati
                    SetAcquisizione(oMyTotAcq, 1, myConnectionString)
            End Select
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.ImportPagamenti.errore: ", Err)
            'registro l'errore acquisizione
            oMyTotAcq.Id = nIDFlussoImport
            oMyTotAcq.IdEnte = sEnteImport
            oMyTotAcq.sFileAcq = sFileImport
            oMyTotAcq.nStatoAcq = -1
            oMyTotAcq.sEsito = "Errore durante l'importazione."
            oMyTotAcq.tDataAcq = Now
            oMyTotAcq.sOperatore = sOperatore
            'registro l'esito d'acquisizione sulla base dati
            SetAcquisizione(oMyTotAcq, 1, myConnectionString)
        End Try
    End Sub

    Private Function CheckFilePagamenti(ByVal sNameFilePagamenti As String, ByVal sContoCorrente As String, ByVal StringConnection As String) As Integer
        '{1= formato corretto; 0= formato non corretto; 2= file già acquisito; -1= errore}
        Dim oMyFlussi() As ObjFlussoPagamenti

        Try
            'non è un file txt
            If Not sNameFilePagamenti.ToLower.EndsWith(".txt") Then
                Return 0
            End If
            'non è dell’ente in esame
            Log.Debug("ClsGestPag::CheckFilePagamenti:: conto corrente da file::" & sNameFilePagamenti.Substring(21, 15).Trim & "::conto corrente da db::" & sContoCorrente)
            If sNameFilePagamenti.Substring(21, 15).Trim <> sContoCorrente.PadLeft(15, "0") Then
                Return 0
            End If
            'il file è già stato acquisito
            oMyFlussi = GetFlussi(sNameFilePagamenti, StringConnection)
            If Not oMyFlussi Is Nothing Then
                Return 2
            End If
            Return 1
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.CheckFilePagamenti.errore: ", Err)
            Return -1
        End Try
    End Function

    Private Function AcquisisciPagamenti(ByVal sIdEnte As String, sTributo As String, ByVal sPathNameMyFile As String, ByVal sNameMyFile As String, ByVal sPathNameScarti As String, ByVal nIdImport As Integer, ByVal sOperatore As String, ByVal oMyCondScarto() As ObjCondizioniScarto, ByVal myConnectionString As String, bTransitoPag As Boolean, ByRef oRiepilogoAcq As ObjImportPagamenti) As Integer
        '{-1= non a buon fine, 1= buon fine}
        Dim oMyPagamento As OggettoPagamenti
        Dim oMyFlusso As ObjFlussoPagamenti
        Dim oMyFile As IO.StreamReader
        Dim nIdFlusso As Integer = -1
        Dim x, nPagAcq, nPagScarti As Integer
        Dim impPagAcq, impPagScarti As Double
        Dim sLineFile As String

        Try
            'apro il file
            oMyFile = New IO.StreamReader(sPathNameMyFile)
            Try
                Do
                    'leggo la riga
                    sLineFile = oMyFile.ReadLine
                    If Not IsNothing(sLineFile) Then
                        'controllo su che tipo di record mi trovo
                        Select Case sLineFile.Substring(33, 3)
                            'sono su un record di coda
                            Case "999"
                                oMyFlusso = New ObjFlussoPagamenti
                                'prelevo i dati dal file
                                oMyFlusso.ID = nIdFlusso
                                oMyFlusso.IDImportazione = nIdImport
                                oMyFlusso.IDEnte = sIdEnte
                                oMyFlusso.sProvenienza = "POSTECC"
                                oMyFlusso.sOperatore = sOperatore
                                oMyFlusso.tDataAcq = Now
                                oMyFlusso.sFileAcq = sNameMyFile
                                oMyFlusso.sCodCUAS = sLineFile.Substring(0, 1)
                                oMyFlusso.sContoCorrente = sLineFile.Substring(1, 12)
                                oMyFlusso.nRcAcquisiti = sLineFile.Substring(36, 8)
                                oMyFlusso.impAcquisiti = Double.Parse(sLineFile.Substring(44, 12)) / 100
                                oMyFlusso.nPagamentiEsatti = sLineFile.Substring(56, 8)
                                oMyFlusso.impPagamentiEsatti = CDbl(sLineFile.Substring(64, 12)) / 100
                                oMyFlusso.nPagamentiErrati = sLineFile.Substring(76, 8)
                                oMyFlusso.impPagamentiErrati = CDbl(sLineFile.Substring(84, 12)) / 100
                                'se il numero di pagamenti totali è diverso da quelli scartati
                                If nPagScarti <> oMyFlusso.nRcAcquisiti Then
                                    'registro il flusso come acquisito
                                    If SetFlusso(oMyFlusso, 0, myConnectionString) <= 0 Then
                                        Return -1
                                    End If
                                End If
                                'incremento le variabili di totalizzazione importazione
                                oRiepilogoAcq.nRcAcquisiti += nPagAcq
                                oRiepilogoAcq.impAcquisiti += impPagAcq
                                oRiepilogoAcq.nRcScarti += nPagScarti
                                oRiepilogoAcq.impScarti += impPagScarti
                                'svuoto le variabiali riferite al singolo flusso
                                nPagAcq = 0 : nPagScarti = 0 : impPagAcq = 0 : impPagScarti = 0 : nIdFlusso = -1

                                'sono su un record di pagamento
                            Case Else
                                oMyPagamento = New OggettoPagamenti
                                'controllo se devo prelevare l'identificativo flusso
                                If nIdFlusso = -1 Then
                                    'prelevo l'identificativo flusso
                                    nIdFlusso = GetNewIdFlusso(sIdEnte, myConnectionString)
                                    If nIdFlusso <= 0 Then
                                        Return -1
                                    End If
                                End If
                                'setto le variabili generali del pagamento
                                oMyPagamento.IDImportazione = nIdImport
                                oMyPagamento.IDFlusso = nIdFlusso
                                oMyPagamento.sProvenienza = "POSTECC"
                                oMyPagamento.sTipoPagamento = "RA"
                                oMyPagamento.sOperatore = sOperatore
                                oMyPagamento.tDataInsert = Now
                                oMyPagamento.tDataScadenza = Date.MaxValue
                                oMyPagamento.dImportoRata = 0
                                'prelevo i dati dal file
                                oMyPagamento.sTipoBollettino = sLineFile.Substring(33, 3)
                                oMyPagamento.sCodBollettino = sLineFile.Substring(61, 16)
                                oMyPagamento.IdEnte = sIdEnte
                                oMyPagamento.sProgCaricamento = sLineFile.Substring(0, 8)
                                oMyPagamento.sProgSelezione = sLineFile.Substring(8, 7)
                                oMyPagamento.sCCBeneficiario = sLineFile.Substring(15, 12)
                                'oMyPagamento.tDataPagamento = FncGen.FormattaData("20" & sLineFile.Substring(27, 6), "G")
                                oMyPagamento.tDataPagamento = DateTime.ParseExact(FncGen.FormattaData("20" & sLineFile.Substring(27, 6), "G"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)

                                oMyPagamento.dImportoPagamento = CDbl(sLineFile.Substring(36, 10)) / 100
                                oMyPagamento.sUfficioSportello = sLineFile.Substring(46, 8)
                                oMyPagamento.sDivisa = "E" 'sLineFile.Substring(54, 1)
                                'oMyPagamento.tDataAccredito = FncGen.FormattaData("20" & sLineFile.Substring(55, 6), "G")
                                oMyPagamento.tDataAccredito = DateTime.ParseExact(FncGen.FormattaData("20" & sLineFile.Substring(55, 6), "G"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)

                                oMyPagamento.bFlagDettaglio = False
                                'incremento la variabile dei record presenti sul file
                                oRiepilogoAcq.nRcDaAcquisire += 1
                                oRiepilogoAcq.impDaAcquisire += oMyPagamento.dImportoPagamento
                                'controllo le condizioni di scarto in base al tipo bollettino che sto trattando
                                For x = 0 To oMyCondScarto.GetUpperBound(0)
                                    If oMyPagamento.sTipoBollettino = oMyCondScarto(x).sTipoBollettino Then
                                        Select Case oMyCondScarto(x).sTipoScarto
                                            Case 1 'il pagamento deve essere abbinato tramite il codice bollettino per essere acquisito
                                                'cerco se trovo l'abbinamento
                                                If AbbinaBollettino(oMyPagamento, myConnectionString) = False Then
                                                    If bTransitoPag Then
                                                        'ho trovato quindi registro il pagamento sulla base dati
                                                        If SetPagamentoTransito(myConnectionString, oMyPagamento, sTributo, 0) <= 0 Then
                                                            Log.Debug("AcquisisciPagamenti::errore insert in transito fallito")
                                                            Return -1
                                                        End If
                                                    Else
                                                        'registro la presenza del file di scarti
                                                        oRiepilogoAcq.sFileScarti = sPathNameScarti
                                                        'scrivo il record sul file degli scarti
                                                        If WriteFileScarti(oMyPagamento, sPathNameMyFile, sPathNameScarti) <= 0 Then
                                                            Return -1
                                                        End If
                                                    End If
                                                    'incremento le variabili degli scarti
                                                    nPagScarti += 1
                                                    impPagScarti += oMyPagamento.dImportoPagamento
                                                Else
                                                    'ho trovato quindi registro il pagamento sulla base dati
                                                    If SetPagamento(myConnectionString, oMyPagamento, OggettoPagamenti.TypeOperation.Insert) <= 0 Then
                                                        Return -1
                                                    End If
                                                    'incremento le variabili dei pagamenti acquisiti
                                                    nPagAcq += 1
                                                    impPagAcq += oMyPagamento.dImportoPagamento
                                                End If

                                            Case 2 'il pagamento deve essere messo su file di scarti
                                                If bTransitoPag Then
                                                    'ho trovato quindi registro il pagamento sulla base dati
                                                    If SetPagamentoTransito(myConnectionString, oMyPagamento, sTributo, 0) <= 0 Then
                                                        Log.Debug("AcquisisciPagamenti::errore insert in transito fallito")
                                                        Return -1
                                                    End If
                                                Else
                                                    'registro la presenza del file di scarti
                                                    oRiepilogoAcq.sFileScarti = sPathNameScarti
                                                    'scrivo il record sul file degli scarti
                                                    If WriteFileScarti(oMyPagamento, sPathNameMyFile, sPathNameScarti) <= 0 Then
                                                        Return -1
                                                    End If
                                                End If
                                                'incremento le variabili degli scarti
                                                nPagScarti += 1
                                                impPagScarti += oMyPagamento.dImportoPagamento

                                            Case 3 'il pagamento non deve essere acquisito
                                                If bTransitoPag Then
                                                    'registro la presenza del file di scarti
                                                    oRiepilogoAcq.sFileScarti = sPathNameScarti
                                                    'scrivo il record sul file degli scarti
                                                    If WriteFileScarti(oMyPagamento, sPathNameMyFile, sPathNameScarti) <= 0 Then
                                                        Return -1
                                                    End If
                                                    'incremento le variabili degli scarti
                                                    nPagScarti += 1
                                                    impPagScarti += oMyPagamento.dImportoPagamento
                                                Else
                                                    'decremento le variabili di importazione
                                                    oRiepilogoAcq.nRcDaAcquisire -= 1
                                                    oRiepilogoAcq.impDaAcquisire -= oMyPagamento.dImportoPagamento
                                                End If
                                        End Select
                                    End If
                                Next
                        End Select
                    End If
                Loop Until sLineFile Is Nothing
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.AcquisisciPagamenti.errore: ", Err)
                Return -1
            Finally
                oMyFile.Close()
            End Try
            Return 1
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.AcquisisciPagamenti.errore: ", Err)
            Return -1
        End Try
    End Function

    Private Sub DeleteImportPagamenti(ByVal nIdImport As Integer, ByVal myConnectionString As String)
        Dim oRiepImport As New ObjImportPagamenti
        Dim oRiepFlusso As New ObjFlussoPagamenti
        Dim oRiepPag As New OggettoPagamenti

        Try
            'valorizzo l'id importazione da eliminare
            oRiepImport.Id = nIdImport
            oRiepImport.tDataAcq = Now
            oRiepFlusso.IDImportazione = nIdImport
            oRiepFlusso.tDataAcq = Now
            oRiepPag.IDImportazione = nIdImport
            'svuoto la base dati
            SetAcquisizione(oRiepImport, 2, myConnectionString)
            SetFlusso(oRiepFlusso, 2, myConnectionString)
            SetPagamento(myConnectionString, oRiepPag, OggettoPagamenti.TypeOperation.DelByFlusso)

        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.DeleteImportPagamenti.errore: ", Err)
        End Try
    End Sub

    Private Function WriteFileScarti(ByVal oMyPagamento As OggettoPagamenti, ByVal sFileOrg As String, ByVal sFileScarti As String) As Integer
        '{-1= non a buon fine, 1= buon fine}
        Try
            Dim sDatiScarti As String = "Data Acquisizione: " + Now + ";Cod.Ente: " + oMyPagamento.IdEnte + ";Cod.Bollettino: " + oMyPagamento.sCodBollettino + ";Data Pagamento: " + oMyPagamento.tDataPagamento + ";Importo Pagato: " + FormatNumber(oMyPagamento.dImportoPagamento, 2)

            If FncGen.WriteFile(sFileScarti, sDatiScarti) = 0 Then
                Return 0
            Else
                Return 1
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.WriteFileScarti.errore: ", Err)
            Return -1
        End Try
    End Function

    Public Function CheckImportazione(ByVal sIdEnte As String, ByVal StringConnection As String) As Integer
        Dim oMyTotAcq As New ObjImportPagamenti

        Try
            'controllare se è in elaborazione 
            oMyTotAcq = GetAcquisizione(sIdEnte, 1, StringConnection)
            If Not oMyTotAcq Is Nothing Then
                If oMyTotAcq.Id <> -1 Then
                    Return 1
                Else
                    Return 0
                End If
                Return 1
            Else
                Return 0
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.CheckImportazione.errore: ", Err)
            Return -1
        End Try
    End Function

    Public Function PrelevaImportazione(ByVal sIdEnte As String, ByVal nIsFirstTime As Integer, ByVal StringConnection As String) As ObjImportPagamenti
        Dim myItem As New ObjImportPagamenti
        Dim nMaxId As Integer

        Try
            'verifica se l'elaborazione è terminata
            nMaxId = MaxIdImport(sIdEnte, StringConnection)
            If Not IsDBNull(nMaxId) Then
                'prelevo i dati dalla tabella dei flussi
                myItem = GetAcquisizione(sIdEnte, nIsFirstTime, StringConnection)
                If Not myItem Is Nothing Then
                    If myItem.Id <> nMaxId Or nMaxId = -1 Then
                        'controlla se l'elaborazione è terminata con errori
                        myItem = GetAcquisizione(sIdEnte, -1, StringConnection)
                    Else
                        '*** 20181024 - aggiunto controllo su corretta importazione ***
                        If ControlloImportF24(myItem.sFileAcq, StringConnection) = False Then
                            myItem.nStatoAcq = -1
                            myItem.sEsito = "Errore durante l'importazione: totali non corrispondenti! Contattare l'assistenza!"
                            SetAcquisizione(myItem, 3, StringConnection)
                            myItem = GetAcquisizione(sIdEnte, nIsFirstTime, StringConnection)
                        End If
                        '*** ****
                    End If
                Else
                    'controlla se l'elaborazione è terminata con errori
                    myItem = GetAcquisizione(sIdEnte, -1, StringConnection)
                End If
            End If
            Return myItem
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.PrelevaImportazione.errore: ", Err)
            Return Nothing
        End Try
    End Function

    Public Function MaxIdImport(ByVal sIdEnte As String, ByVal StringConnection As String) As Integer
        Dim DrReturn As SqlClient.SqlDataReader = Nothing
        Dim myIdentity As Integer = -1

        Try
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(StringConnection)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            'cmdMyCommand.CommandType = CommandType.Text
            'cmdMyCommand.CommandText = "SELECT MAX(ID) AS MAXID"
            'cmdMyCommand.CommandText += " FROM TBLIMPORTPAGAMENTI"
            'cmdMyCommand.CommandText += " WHERE (TBLIMPORTPAGAMENTI.IDENTE = @IDENTE)"
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_GetPagamentiAcquisizione"
            'valorizzo i parameters:
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ISMAX", SqlDbType.Int)).Value = 1
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            DrReturn = cmdMyCommand.ExecuteReader
            Do While DrReturn.Read
                If Not IsDBNull(DrReturn("maxid")) Then
                    myIdentity = CInt(DrReturn("maxid"))
                End If
            Loop
            Return myIdentity
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.MaxIdImport.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return -1
        Finally
            DrReturn.Close()
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sIDEnte"></param>
    ''' <param name="nStato">STATO_IMPORTAZIONE: {1=in corso, 0=finita correttamente, -1= finita con errori}</param>
    ''' <param name="StringConnection"></param>
    ''' <returns></returns>
    Public Function GetAcquisizione(ByVal sIDEnte As String, ByVal nStato As Integer, ByVal StringConnection As String) As ObjImportPagamenti
        Dim oMyImportPag As New ObjImportPagamenti
        Dim DrReturn As SqlClient.SqlDataReader = Nothing

        Try
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(StringConnection)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            ''*** 20130415 - sbagliava in caso di annullo totale ***
            'cmdMyCommand.CommandType = CommandType.Text
            ''*** ***
            'cmdMyCommand.CommandText = "SELECT TOP 1 *"
            'cmdMyCommand.CommandText += " FROM TBLIMPORTPAGAMENTI"
            'cmdMyCommand.CommandText += " WHERE (TBLIMPORTPAGAMENTI.IDENTE = @IDENTE)"
            'cmdMyCommand.CommandText += " ORDER BY ID DESC"
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_GetPagamentiAcquisizione"
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sIDEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ISMAX", SqlDbType.Int)).Value = 0
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            DrReturn = cmdMyCommand.ExecuteReader
            Do While DrReturn.Read
                If CInt(DrReturn("stato_importazione")) = nStato Or nStato <> 1 Then
                    oMyImportPag.Id = CInt(DrReturn("id"))
                    oMyImportPag.IdEnte = CStr(DrReturn("idente"))
                    oMyImportPag.sFileAcq = CStr(DrReturn("file_import"))
                    oMyImportPag.nStatoAcq = CInt(DrReturn("stato_importazione"))
                    oMyImportPag.sEsito = CStr(DrReturn("esito"))
                    oMyImportPag.sFileScarti = CStr(DrReturn("file_scarti"))
                    oMyImportPag.nRcDaAcquisire = CInt(DrReturn("rcdaacquisire"))
                    oMyImportPag.nRcAcquisiti = CInt(DrReturn("rcacquisiti"))
                    oMyImportPag.nRcScarti = CInt(DrReturn("rcscartati"))
                    oMyImportPag.impDaAcquisire = CDbl(DrReturn("impdaacquisire"))
                    oMyImportPag.impAcquisiti = CDbl(DrReturn("impacquisiti"))
                    oMyImportPag.impScarti = CDbl(DrReturn("impscartati"))
                    oMyImportPag.tDataAcq = CDate(DrReturn("data_import"))
                    oMyImportPag.sOperatore = CStr(DrReturn("operatore"))
                End If
            Loop
            Return oMyImportPag
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.GetAcquisizione.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        Finally
            DrReturn.Close()
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function

    Public Function SetAcquisizione(ByVal oMyImport As ObjImportPagamenti, ByVal nDBOperation As Integer, ByVal myConnectionString As String) As Integer
        Dim nMyReturn As Integer

        Try
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            'Valorizzo i parameters:
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = oMyImport.Id
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DBOPERATION", SqlDbType.Int)).Value = nDBOperation
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oMyImport.IdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FILE_IMPORT", SqlDbType.NVarChar)).Value = oMyImport.sFileAcq
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@STATO_IMPORTAZIONE", SqlDbType.Int)).Value = oMyImport.nStatoAcq
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ESITO", SqlDbType.NVarChar)).Value = FncGen.ReplaceChar(oMyImport.sEsito)
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@FILE_SCARTI", SqlDbType.NVarChar)).Value = oMyImport.sFileScarti
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@RCDAACQUISIRE", SqlDbType.Int)).Value = oMyImport.nRcDaAcquisire
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@RCACQUISITI", SqlDbType.Int)).Value = oMyImport.nRcAcquisiti
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@RCSCARTATI", SqlDbType.Int)).Value = oMyImport.nRcScarti
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPDAACQUISIRE", SqlDbType.Float)).Value = oMyImport.impDaAcquisire
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPACQUISITI", SqlDbType.Float)).Value = oMyImport.impAcquisiti
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPSCARTATI", SqlDbType.Float)).Value = oMyImport.impScarti
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_IMPORT", SqlDbType.DateTime)).Value = oMyImport.tDataAcq
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@OPERATORE", SqlDbType.NVarChar)).Value = oMyImport.sOperatore
            'Valorizzo il commandtext:
            Select Case nDBOperation
                Case 0, 1, 3
                    cmdMyCommand.CommandType = CommandType.StoredProcedure
                    cmdMyCommand.CommandText = "prc_TBLIMPORTPAGAMENTI_IU"
                    cmdMyCommand.Parameters("@ID").Direction = ParameterDirection.InputOutput
                    Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                    cmdMyCommand.ExecuteNonQuery()
                    nMyReturn = cmdMyCommand.Parameters("@ID").Value
                Case 2
                    cmdMyCommand.CommandType = CommandType.StoredProcedure
                    cmdMyCommand.CommandText = "prc_TBLIMPORTPAGAMENTI_D"
                    Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                    cmdMyCommand.ExecuteNonQuery()
            End Select
            ''*** 20130415 - sbagliava in caso di annullo totale ***
            'cmdMyCommand.CommandType = CommandType.Text
            ''*** ***
            'Select Case nDBOperation
            '    Case 0
            '        cmdMyCommand.CommandText = "INSERT INTO TBLIMPORTPAGAMENTI (IDENTE, FILE_IMPORT, STATO_IMPORTAZIONE, ESITO, FILE_SCARTI,"
            '        cmdMyCommand.CommandText += " RCDAACQUISIRE, RCACQUISITI, RCSCARTATI,"
            '        cmdMyCommand.CommandText += " IMPDAACQUISIRE, IMPACQUISITI, IMPSCARTATI,"
            '        cmdMyCommand.CommandText += " DATA_IMPORT, OPERATORE)"
            '        cmdMyCommand.CommandText += " VALUES (@IDENTE, @FILE_IMPORT, @STATO_IMPORTAZIONE, @ESITO, @FILE_SCARTI,"
            '        cmdMyCommand.CommandText += " @NPAGAMENTI_FILE, @NPAGAMENTI_IMPORT, @NPAGAMENTI_SCARTATI,"
            '        cmdMyCommand.CommandText += " @IMPPAGAMENTI_FILE, @IMPPAGAMENTI_IMPORT, @IMPPAGAMENTI_SCARTATI,"
            '        cmdMyCommand.CommandText += " @DATA_IMPORT, @OPERATORE)"
            '        cmdMyCommand.CommandText += " SELECT @@IDENTITY"
            '        'eseguo la query
            '        Dim DrReturn As SqlClient.SqlDataReader
            '        DrReturn = cmdMyCommand.ExecuteReader()
            '        Do While DrReturn.Read
            '            nMyReturn = DrReturn(0)
            '        Loop
            '        DrReturn.Close()
            '    Case 1
            '        cmdMyCommand.CommandText = "UPDATE TBLIMPORTPAGAMENTI SET IDENTE=@IDENTE, FILE_IMPORT=@FILE_IMPORT,"
            '        cmdMyCommand.CommandText += " STATO_IMPORTAZIONE=@STATO_IMPORTAZIONE, ESITO=@ESITO, FILE_SCARTI=@FILE_SCARTI, "
            '        cmdMyCommand.CommandText += " RCDAACQUISIRE=@NPAGAMENTI_FILE, RCACQUISITI=@NPAGAMENTI_IMPORT, RCSCARTATI=@NPAGAMENTI_SCARTATI,"
            '        cmdMyCommand.CommandText += " IMPDAACQUISIRE=@IMPPAGAMENTI_FILE, IMPACQUISITI=@IMPPAGAMENTI_IMPORT, IMPSCARTATI=@IMPPAGAMENTI_SCARTATI,"
            '        cmdMyCommand.CommandText += " DATA_IMPORT=@DATA_IMPORT, OPERATORE=@OPERATORE"
            '        cmdMyCommand.CommandText += " WHERE (TBLIMPORTPAGAMENTI.ID=@IDIMPORT)"
            '        nMyReturn = cmdMyCommand.ExecuteNonQuery()
            '    Case 2
            '        cmdMyCommand.CommandText = "DELETE"
            '        cmdMyCommand.CommandText += " FROM TBLIMPORTPAGAMENTI"
            '        cmdMyCommand.CommandText += " WHERE (TBLIMPORTPAGAMENTI.ID=@IDIMPORT)"
            '        nMyReturn = cmdMyCommand.ExecuteNonQuery
            '    Case 3
            '        cmdMyCommand.CommandText = "UPDATE TBLIMPORTPAGAMENTI SET IDENTE=@IDENTE, FILE_IMPORT=@FILE_IMPORT,"
            '        cmdMyCommand.CommandText += " STATO_IMPORTAZIONE=@STATO_IMPORTAZIONE, ESITO=@ESITO, FILE_SCARTI=@FILE_SCARTI, "
            '        cmdMyCommand.CommandText += " DATA_IMPORT=@DATA_IMPORT, OPERATORE=@OPERATORE"
            '        cmdMyCommand.CommandText += " WHERE (TBLIMPORTPAGAMENTI.ID=@IDIMPORT)"
            '        nMyReturn = cmdMyCommand.ExecuteNonQuery()
            'End Select

            Return nMyReturn
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.SetAcquisizione.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return -1
        Finally
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function
    '*** 20181024 ***
    Public Function ControlloImportF24(ByVal IdentificativoFile As String, ByVal StringConnection As String) As Boolean
        Dim DrReturn As SqlClient.SqlDataReader = Nothing
        Dim myRet As Boolean = True

        Try
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(StringConnection)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_GetControlloImportF24"
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTIFICATIVO", SqlDbType.NVarChar)).Value = IdentificativoFile
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            DrReturn = cmdMyCommand.ExecuteReader
            Do While DrReturn.Read
                myRet = False
            Loop
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.ControlloImportF24.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myRet = False
        Finally
            DrReturn.Close()
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
        Return myRet
    End Function
    '*** ***
    Public Function GetCondizioniScarto(ByVal sContoCorrente As String, ByVal myConnectionString As String) As ObjCondizioniScarto()
        Dim oListCondizioni() As ObjCondizioniScarto = Nothing
        Dim oMyCondizione As ObjCondizioniScarto
        Dim DrMyDati As SqlClient.SqlDataReader = Nothing
        Dim nList As Integer = -1

        Try
            'prelevo le condizioni di scarto dal database
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            'cmdMyCommand.CommandType = CommandType.Text
            'cmdMyCommand.CommandText = "SELECT *"
            'cmdMyCommand.CommandText += " FROM TBLCONFIGURAZIONEIMPORTPAGAMENTI"
            'cmdMyCommand.CommandText += " WHERE (TBLCONFIGURAZIONEIMPORTPAGAMENTI.NOME_FILE=@CONTOCORRENTE)"
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_GetPagamentiCondizioniScarto"
            'valorizzo i parameters:
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOME_FILE", SqlDbType.NVarChar)).Value = sContoCorrente
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            DrMyDati = cmdMyCommand.ExecuteReader
            Do While DrMyDati.Read
                oMyCondizione = New ObjCondizioniScarto
                oMyCondizione.sTipoBollettino = CStr(DrMyDati("tipo_bollettino"))
                oMyCondizione.sTipoScarto = CStr(DrMyDati("tipo_scarto"))
                'incremento l’array
                nList += 1
                ReDim Preserve oListCondizioni(nList)
                oListCondizioni(nList) = oMyCondizione
            Loop
            Return oListCondizioni
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.GetCondizioniScart.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        Finally
            DrMyDati.Close()
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
    End Function

    Public Function GetFlussi(ByVal sFileImport As String, ByVal myConnectionString As String) As ObjFlussoPagamenti()
        Dim oListFlussi() As ObjFlussoPagamenti = Nothing
        Dim oMyFlusso As ObjFlussoPagamenti
        Dim DrMyDati As SqlClient.SqlDataReader = Nothing
        Dim nList As Integer = -1

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.Text
            'valorizzo il commandtext:
            cmdMyCommand.CommandText = "SELECT *"
            cmdMyCommand.CommandText += " FROM TBLDESCRIZIONEFLUSSIPAG"
            'valorizzo i parameters:
            cmdMyCommand.Parameters.Clear()
            If sFileImport <> "" Then
                cmdMyCommand.CommandText += " WHERE (NOME_FILE=@NOMEFILE)"
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOMEFILE ", SqlDbType.NVarChar)).Value = sFileImport
            End If
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            DrMyDati = cmdMyCommand.ExecuteReader
            Do While DrMyDati.Read
                oMyFlusso = New ObjFlussoPagamenti
                oMyFlusso.ID = CInt(DrMyDati("id"))
                oMyFlusso.IDEnte = CStr(DrMyDati("idente"))
                oMyFlusso.IDImportazione = CInt(DrMyDati("idimport"))
                oMyFlusso.impAcquisiti = CDbl(DrMyDati("totale_importi"))
                oMyFlusso.impPagamentiErrati = CDbl(DrMyDati("totale_importi_errati"))
                oMyFlusso.impPagamentiEsatti = CDbl(DrMyDati("totale_importi_esatti"))
                oMyFlusso.nPagamentiErrati = CInt(DrMyDati("totale_documenti_errati"))
                oMyFlusso.nPagamentiEsatti = CInt(DrMyDati("totale_documenti_esatti"))
                oMyFlusso.nRcAcquisiti = CInt(DrMyDati("numero_pagamenti"))
                oMyFlusso.sCodCUAS = CStr(DrMyDati("codice_cuas"))
                oMyFlusso.sContoCorrente = CStr(DrMyDati("numero_cc"))
                oMyFlusso.sDivisa = CStr(DrMyDati("divisa"))
                oMyFlusso.sFileAcq = CStr(DrMyDati("nome_file"))
                oMyFlusso.sOperatore = CStr(DrMyDati("operatore"))
                oMyFlusso.sProvenienza = CStr(DrMyDati("provenienza"))
                oMyFlusso.tDataAcq = CDate(DrMyDati("data_import"))
                'incremento l’array
                nList += 1
                ReDim Preserve oListFlussi(nList)
                oListFlussi(nList) = oMyFlusso
            Loop
            Return oListFlussi
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.GetFlussi.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        Finally
            DrMyDati.Close()
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
    End Function

    Public Function GetNewIdFlusso(ByVal sMyIdEnte As String, ByVal myConnectionString As String) As Integer
        Dim DrMyDati As SqlClient.SqlDataReader = Nothing
        Dim nMyReturn As Integer = 0

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            'cmdMyCommand.CommandType = CommandType.Text
            'cmdMyCommand.CommandText = "SELECT TOP 1 ID"
            'cmdMyCommand.CommandText += " FROM TBLDESCRIZIONEFLUSSIPAG"
            'cmdMyCommand.CommandText += " WHERE (IDENTE =@IDENTE)"
            'cmdMyCommand.CommandText += " ORDER BY ID DESC"
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_TBLDESCRIZIONEFLUSSIPAG_S"
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = sMyIdEnte
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            DrMyDati = cmdMyCommand.ExecuteReader
            Do While DrMyDati.Read
                nMyReturn = CInt(DrMyDati("id"))
            Loop
            nMyReturn += 1
            Return nMyReturn
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.GetNewIdFlusso.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return -1
        Finally
            DrMyDati.Close()
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
    End Function

    Public Function SetFlusso(ByVal oMyFlusso As ObjFlussoPagamenti, ByVal nDBOperation As Integer, ByVal myConnectionString As String) As Integer
        Dim nMyReturn As Integer

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            'cmdMyCommand.CommandType = CommandType.Text
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            'Valorizzo i parameters:
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = oMyFlusso.ID
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDIMPORT", SqlDbType.Int)).Value = oMyFlusso.IDImportazione
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oMyFlusso.IDEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOME_FILE", SqlDbType.NVarChar)).Value = oMyFlusso.sFileAcq
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PROVENIENZA", SqlDbType.NVarChar)).Value = oMyFlusso.sProvenienza
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DIVISA", SqlDbType.NVarChar)).Value = oMyFlusso.sDivisa
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO_PAGAMENTI", SqlDbType.Int)).Value = oMyFlusso.nRcAcquisiti
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TOTALE_IMPORTI", SqlDbType.Float)).Value = oMyFlusso.impAcquisiti
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE_CUAS", SqlDbType.NVarChar)).Value = oMyFlusso.sCodCUAS
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO_CC", SqlDbType.NVarChar)).Value = oMyFlusso.sContoCorrente
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TOTALE_DOCUMENTI_ESATTI", SqlDbType.Int)).Value = oMyFlusso.nPagamentiEsatti
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TOTALE_IMPORTI_ESATTI", SqlDbType.Float)).Value = oMyFlusso.impPagamentiEsatti
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TOTALE_DOCUMENTI_ERRATI", SqlDbType.Int)).Value = oMyFlusso.nPagamentiErrati
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TOTALE_IMPORTI_ERRATI", SqlDbType.Float)).Value = oMyFlusso.impPagamentiErrati
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_IMPORT", SqlDbType.DateTime)).Value = oMyFlusso.tDataAcq
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@OPERATORE", SqlDbType.NVarChar)).Value = oMyFlusso.sOperatore
            Select Case nDBOperation
                Case 0
                    cmdMyCommand.CommandText = "prc_TBLDESCRIZIONEFLUSSIPAG_IU"
                    'cmdMyCommand.CommandText = "INSERT INTO TBLDESCRIZIONEFLUSSIPAG"
                    'cmdMyCommand.CommandText += "(ID, IDIMPORT, IDENTE, NOME_FILE, PROVENIENZA, DIVISA,"
                    'cmdMyCommand.CommandText += " NUMERO_PAGAMENTI, TOTALE_IMPORTI, CODICE_CUAS, NUMERO_CC,"
                    'cmdMyCommand.CommandText += " TOTALE_DOCUMENTI_ESATTI, TOTALE_IMPORTI_ESATTI, TOTALE_DOCUMENTI_ERRATI, TOTALE_IMPORTI_ERRATI,"
                    'cmdMyCommand.CommandText += " DATA_IMPORT, OPERATORE)"
                    'cmdMyCommand.CommandText += " VALUES (@IDFLUSSO, @IDIMPORT, @IDENTE, @NOME_FILE, @PROVENIENZA, @DIVISA,"
                    'cmdMyCommand.CommandText += " @NUMERO_PAGAMENTI, @TOTALE_IMPORTI, @CODICE_CUAS, @NUMERO_CC,"
                    'cmdMyCommand.CommandText += " @TOTALE_DOCUMENTI_ESATTI, @TOTALE_IMPORTI_ESATTI, @TOTALE_DOCUMENTI_ERRATI, @TOTALE_IMPORTI_ERRATI,"
                    'cmdMyCommand.CommandText += " @DATA_IMPORT, @OPERATORE)"
                    Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                    nMyReturn = cmdMyCommand.ExecuteNonQuery
                Case 2
                    cmdMyCommand.CommandText = "prc_TBLDESCRIZIONEFLUSSIPAG_D"
                    'cmdMyCommand.CommandText = "DELETE"
                    'cmdMyCommand.CommandText += " FROM TBLDESCRIZIONEFLUSSIPAG"
                    'cmdMyCommand.CommandText += " WHERE (TBLDESCRIZIONEFLUSSIPAG.IDIMPORT=@IDIMPORT)"
                    Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                    nMyReturn = cmdMyCommand.ExecuteNonQuery
            End Select
            Return nMyReturn
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.SetFlusso.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return -1
        Finally
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
    End Function

    '*** 20131104 - TARES ***
    Public Function GetPagamento(ByVal sIdEnte As String, ByVal StringConnection As String) As DataView
        Dim DvMyDati As New DataView
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim myDataSet As New DataSet

        Try
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(StringConnection)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            cmdMyCommand.CommandText = "prc_GetPagamentiDaDettagliare"
            '*** ***
            'valorizzo i parameters:
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE ", SqlDbType.NVarChar)).Value = sIdEnte
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(myDataSet, "Create DataView")
            myAdapter.Dispose()
            Return myDataSet.Tables(0).DefaultView()
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.GetPagamento.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        Finally
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function

    Public Function GetDettaglioEmesso(ByVal sCodCartella As String) As SqlClient.SqlDataReader
        Dim DrMyDati As SqlClient.SqlDataReader
        Try
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            cmdMyCommand.CommandText = "prc_GetDettaglioEmesso"
            'valorizzo i parameters:
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICECARTELLA ", SqlDbType.NVarChar)).Value = sCodCartella
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            DrMyDati = cmdMyCommand.ExecuteReader

            Return DrMyDati
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.GetDettaglioEmesso.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        Finally
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function

    Public Function SetPagamentoDettagliato(ByVal MyStringConnection As String, ByVal oListMyDettaglio() As ObjDettaglioPagamento) As Integer
        '{0= non a buon fine, >0= id tabella}
        Try
            Dim x As Integer
            'Ciclo sull'array di oggetti di dettaglio:
            For x = 0 To oListMyDettaglio.GetUpperBound(0)
                'Se l'inserimento su base dati richiamando la funzione SetDettaglioPagamento(oListDettaglio(x)) non è andato a buon fine:
                If SetDettaglioPagamento(MyStringConnection, oListMyDettaglio(x), 0) <= 0 Then
                    'Restituisco errore;
                    Return 0
                End If
            Next
            Return 1
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.SetPagamentoDettagliato.errore: ", Err)
            Return -1
        End Try
    End Function

    Public Function SetDettaglioPagamento(ByVal MyStringConnection As String, ByVal oMyDettaglio As ObjDettaglioPagamento, ByVal nDBOperation As Integer) As Integer
        Dim nMyReturn As Integer
        Try
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(MyStringConnection)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            'valorizzo i parameters:
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPAGAMENTO", SqlDbType.Int)).Value = oMyDettaglio.IDPagamento
            'Valorizzo il commandtext:
            Select Case nDBOperation
                Case 0
                    'cmdMyCommand.CommandText = "INSERT INTO TBLDETTAGLIOPAGAMENTI"
                    'cmdMyCommand.CommandText += " (IDPAGAMENTO, COD_CAPITOLO, IDCATEGORIA, ANNO_RUOLO, DIVISA, IMPORTO, OPERATORE, DATA_INSERIMENTO)"
                    'cmdMyCommand.CommandText += " VALUES (@IDPAGAMENTO, @CODICECAPITOLO, @IDCATEGORIA, @ANNORUOLO, @DIVISA, @IMPORTO, @OPERATORE, @DATAINSERIMENTO)"
                    'cmdMyCommand.CommandText += " SELECT @@IDENTITY"
                    cmdMyCommand.CommandText = "TBLDETTAGLIOPAGAMENTI_IU"
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = -1
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COD_CAPITOLO", SqlDbType.NVarChar)).Value = oMyDettaglio.sCodCapitolo
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODVOCE", SqlDbType.NVarChar)).Value = oMyDettaglio.sCodVoce
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO_RUOLO", SqlDbType.NVarChar)).Value = oMyDettaglio.sAnno
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPOPARTITA", SqlDbType.NVarChar)).Value = oMyDettaglio.TipoPartita
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCATEGORIA", SqlDbType.NVarChar)).Value = oMyDettaglio.sIdCategoria
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NCOMPONENTI", SqlDbType.Int)).Value = oMyDettaglio.NComponenti
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IMPORTO", SqlDbType.Decimal)).Value = oMyDettaglio.dImpDettaglio
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INSERIMENTO", SqlDbType.DateTime)).Value = oMyDettaglio.tDataInsert
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@OPERATORE", SqlDbType.NVarChar)).Value = oMyDettaglio.sOperatore
                    cmdMyCommand.Parameters("@ID").Direction = ParameterDirection.InputOutput
                    'eseguo la query
                    Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                    cmdMyCommand.ExecuteNonQuery()
                    nMyReturn = cmdMyCommand.Parameters("@ID").Value
                Case 2
                    cmdMyCommand.CommandText = "TBLDETTAGLIOPAGAMENTI_D"
                    Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                    nMyReturn = cmdMyCommand.ExecuteNonQuery
            End Select

            Return nMyReturn
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.SetDettaglioPagamento.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return -1
        Finally
            cmdMyCommand.Connection.Close()
        End Try
    End Function
    '*** ***
    Public Function GetImportazioni(ByVal IdEnte As String, myStringConnection As String) As ObjImportPagamenti()
        Dim sSQL As String
        Dim myItem As New ObjImportPagamenti
        Dim ListRuoli As New ArrayList
        Dim myDataView As New DataView

        Try
            Using ctx As New DBModel(ConstSession.DBType, myStringConnection)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetFlussiPagamenti", "IDENTE")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IDENTE", IdEnte))
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.GetImportazioni.erroreQuery: ", ex)
                    Return Nothing
                Finally
                    ctx.Dispose()
                End Try
                For Each myRow As DataRowView In myDataView
                    myItem = New ObjImportPagamenti
                    myItem.Id = StringOperation.FormatInt(myRow("ID"))
                    myItem.sFileAcq = StringOperation.FormatString(myRow("FILE_IMPORT"))
                    myItem.tDataAcq = StringOperation.FormatDateTime(myRow("DATA_IMPORT"))
                    myItem.sEsito = StringOperation.FormatString(myRow("ESITO"))
                    myItem.nRcDaAcquisire = StringOperation.FormatInt(myRow("RCDAACQUISIRE"))
                    myItem.nRcAcquisiti = StringOperation.FormatInt(myRow("RCACQUISITI"))
                    myItem.nRcScarti = StringOperation.FormatInt(myRow("RCSCARTATI"))
                    myItem.impDaAcquisire = StringOperation.FormatDouble(myRow("IMPDAACQUISIRE"))
                    myItem.impAcquisiti = StringOperation.FormatDouble(myRow("IMPACQUISITI"))
                    myItem.impScarti = StringOperation.FormatDouble(myRow("IMPSCARTATI"))
                    ListRuoli.Add(myItem)
                Next
            End Using
            Return CType(ListRuoli.ToArray(GetType(ObjImportPagamenti)), ObjImportPagamenti())
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.GetImportazioni.errore: ", Err)
            Return Nothing
        End Try
    End Function
#End Region

    'Public Function DettagliaPagamenti(ByVal sIdEnte As String, ByVal sOperatore As String, ByVal WFSessione As OPENUtility.CreateSessione) As Integer
    '    '{-1= non a buon fine, 1= buon fine}
    '    Try
    '        Dim DvMyDati As New DataView
    '        Dim DrMyEmesso As SqlClient.SqlDataReader
    '        Dim oMyDettaglioPag As ObjDettaglioPagamento
    '        Dim oListDettaglio() As ObjDettaglioPagamento
    '        Dim nPercentPag, nImpDettagliato As Double
    '        Dim nList As Integer = -1
    '        Dim x As Integer

    '        'Prelevo tutti i pagamenti che non hanno il flag dettaglio uguale a 0 dalla tabella TBLPAGAMENTI richiamando la funzione GetPagamento(sIdEnte);
    '        DvMyDati = GetPagamento(sIdEnte, WFSessione)
    '        'Ciclo sui record trovati:
    '        For x = 0 To DvMyDati.Count - 1
    '            nImpDettagliato = 0 : nList = -1
    '            If Not IsDBNull(DvMyDati.Item(x)("importo_carico")) Then
    '                If FormatNumber(CDbl(DvMyDati.Item(x)("importo_carico")), 2) <> 0 Then
    '                    'calcolo la percentuale di pagato rispetto all’emesso tramite la proporzione (emesso:100=pagato:x)
    '                    nPercentPag = ((DvMyDati.Item(x)("importo_pagamento") * 100) / DvMyDati.Item(x)("importo_carico"))
    '                    'Prelevo il dettaglio dell’emesso in base a IDENTE e CODICE_CARTELLA dalla tabella TBLCARTELLEDETTAGLIOVOCI richiamando la funzione GetDettaglioEmesso(sIdEnte, dv(“cod_cartella”));
    '                    DrMyEmesso = GetDettaglioEmesso(DvMyDati.Item(x)("codice_cartella"), WFSessione)
    '                    'Se il datareader è valorizzato:
    '                    If Not IsNothing(DrMyEmesso) Then
    '                        'ciclo sui record trovati:
    '                        Do While DrMyEmesso.Read
    '                            'dichiaro un nuovo oggetto di tipo ObjDettaglioPagamento;
    '                            oMyDettaglioPag = New ObjDettaglioPagamento
    '                            'valorizzo l’oggetto di dettaglio pagamento;
    '                            oMyDettaglioPag.IDPagamento = DvMyDati.Item(x)("id")
    '                            oMyDettaglioPag.sCodCapitolo = DrMyEmesso("codice_capitolo")
    '                            oMyDettaglioPag.sIdCategoria = DrMyEmesso("idcategoria")
    '                            oMyDettaglioPag.sAnno = DrMyEmesso("anno_ruolo")
    '                            oMyDettaglioPag.sDivisa = "E"
    '                            oMyDettaglioPag.tDataInsert = Now
    '                            oMyDettaglioPag.sOperatore = sOperatore
    '                            'calcolo l’importo di scorporo del pagamento rispetto alla voce tramite la proporzione (dettaglio:100=x:percentuale di pagato);
    '                            oMyDettaglioPag.dImpDettaglio = FormatNumber(((DrMyEmesso("importo") * nPercentPag) / 100), 2)
    '                            'incremento l’array di dettaglio pagamento;
    '                            nList += 1
    '                            ReDim Preserve oListDettaglio(nList)
    '                            'inserisco nell’array la voce di dettaglio pagamento appena valorizzata;
    '                            oListDettaglio(nList) = oMyDettaglioPag
    '                            nImpDettagliato += FormatNumber(oMyDettaglioPag.dImpDettaglio, 2)
    '                        Loop
    '                        DrMyEmesso.Close()
    '                        'se la somma delle voci dettagliate non corrisponde al pagato:
    '                        If FormatNumber((DvMyDati.Item(x)("importo_pagamento") - nImpDettagliato), 2) <> "0,00" Then
    '                            'dichiaro un nuovo oggetto di tipo ObjDettaglioPagamento;
    '                            oMyDettaglioPag = New ObjDettaglioPagamento
    '                            'creo una voce di arrotondamento;
    '                            oMyDettaglioPag.IDPagamento = DvMyDati.Item(x)("id")
    '                            oMyDettaglioPag.sCodCapitolo = "9999"
    '                            oMyDettaglioPag.sAnno = ""
    '                            oMyDettaglioPag.sDivisa = "E"
    '                            oMyDettaglioPag.tDataInsert = Now
    '                            oMyDettaglioPag.sOperatore = sOperatore
    '                            oMyDettaglioPag.dImpDettaglio = FormatNumber((DvMyDati.Item(x)("importo_pagamento") - nImpDettagliato), 2)
    '                            'incremento l’array di dettaglio pagamento;
    '                            nList += 1
    '                            ReDim Preserve oListDettaglio(nList)
    '                            'inserisco nell’array la voce di dettaglio pagamento appena valorizzata;
    '                            oListDettaglio(nList) = oMyDettaglioPag
    '                        End If
    '                        'se l’inserimento del dettaglio pagamento sulla base dati richiamando la funzione SetPagamentoDettagliato(oListDettaglio) è andato a buon fine:
    '                        If SetPagamentoDettagliato(oListDettaglio, WFSessione) > 0 Then
    '                            'registro l’avvenuto dettaglio del pagamento sulla base dati richiamando la funzione SetPagamento(2);
    '                            Dim oMyPag As New OggettoPagamenti
    '                            oMyPag.ID = DvMyDati.Item(x)("id")
    '                            If SetPagamento(oMyPag, 3, WFSessione) <= 0 Then
    '                                Return 0
    '                            End If
    '                        End If
    '                    End If
    '                End If
    '            End If
    '        Next
    '        DvMyDati.Dispose()
    '        Return 1
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsGestPag.DettagliaPagamenti.errore: ", Err)
    '        Return -1
    '    End Try
    'End Function
    '*** 20131104 - TARES ***
    Public Function DettagliaPagamenti(ByVal sIdEnte As String, ByVal MyStringConnection As String) As Integer
        '{-1= non a buon fine, 1= buon fine}
        Dim dvMyDati As New DataView
        Dim myPag As New OggettoPagamenti

        Try
            'Prelevo tutti i pagamenti che non hanno il flag dettaglio uguale a 0 dalla tabella TBLPAGAMENTI richiamando la funzione GetPagamento(sIdEnte);
            dvMyDati = GetPagamento(sIdEnte, MyStringConnection)
            'Ciclo sui record trovati:
            For Each myRow As DataRowView In dvMyDati
                myPag = New OggettoPagamenti
                myPag.ID = myRow("id")
                myPag.IdEnte = myRow("idente")
                myPag.IdContribuente = myRow("idcontribuente")
                myPag.sAnno = myRow("anno")
                myPag.sNumeroAvviso = myRow("codice_cartella")
                myPag.dImportoPagamento = myRow("importo_pagamento")
                myPag.dImportoStat = myRow("importo_pagamentostat")
                myPag.sOperatore = myRow("operatore")
                'elimino il dettaglio eventualmente già presente
                Dim myDettaglio As New ObjDettaglioPagamento
                myDettaglio.IDPagamento = myPag.ID
                SetDettaglioPagamento(MyStringConnection, myDettaglio, 2)
                'scorporo ed inserisco il dettaglio
                If DettagliaPagamenti(MyStringConnection, myPag) <= 0 Then
                    Return 0
                End If
            Next
            Return 1
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.DettagliaPagamenti.errore: ", Err)
            Return -1
        Finally
            dvMyDati.Dispose()
        End Try
    End Function

    Private Sub ValDettPag(ByVal IDPag As Integer, ByVal sAnno As String, ByVal impDett As Double, ByVal sCapitolo As String, sVoce As String, ByVal TipoPartita As String, ByVal sCategoria As String, ByVal nComponenti As Integer, ByVal sOperatore As String, ByRef listDettaglio() As ObjDettaglioPagamento)
        Dim myDettaglioPag As New ObjDettaglioPagamento
        Dim nList As Integer = -1

        Try
            If Not listDettaglio Is Nothing Then
                nList = listDettaglio.GetUpperBound(0)
            End If
            myDettaglioPag.IDPagamento = IDPag
            myDettaglioPag.sCodCapitolo = sCapitolo
            myDettaglioPag.sCodVoce = sVoce
            myDettaglioPag.TipoPartita = TipoPartita
            myDettaglioPag.sIdCategoria = sCategoria
            myDettaglioPag.NComponenti = nComponenti
            myDettaglioPag.sAnno = sAnno
            myDettaglioPag.sDivisa = "E"
            myDettaglioPag.tDataInsert = Now
            myDettaglioPag.sOperatore = sOperatore
            'calcolo l’importo di scorporo del pagamento rispetto alla voce tramite la proporzione (dettaglio:100=x:percentuale di pagato);
            myDettaglioPag.dImpDettaglio = impDett
            'incremento l’array di dettaglio pagamento;
            nList += 1
            ReDim Preserve listDettaglio(nList)
            'inserisco nell’array la voce di dettaglio pagamento appena valorizzata;
            listDettaglio(nList) = myDettaglioPag
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.ValDettPag.errore: ", ex)
            Throw ex
        End Try
    End Sub

    Private Sub ScorporoPagamento(ByVal MyStringConnection As String, ByVal myPag As OggettoPagamenti, ByVal bCopriStato As Boolean, ByRef listDettaglio() As ObjDettaglioPagamento, ByRef impDettagliato As Double)
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dtMyDati As New DataTable()
        Dim dtMyRow As DataRow

        Try
            'scorporo il restante pagamento
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandTimeout = 0
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(MyStringConnection)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            'valorizzo il commandText 
            cmdMyCommand.CommandText = "prc_GetDettaglioPagamenti"
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ENTE", SqlDbType.NVarChar)).Value = myPag.IdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDCONTRIBUENTE", SqlDbType.Int)).Value = myPag.IdContribuente
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.VarChar)).Value = myPag.sAnno
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NAVVISO", SqlDbType.VarChar)).Value = myPag.sNumeroAvviso
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TOTPAGATO", SqlDbType.Float)).Value = myPag.dImportoPagamento
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COPRISTATO", SqlDbType.Bit)).Value = bCopriStato
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(dtMyDati)
            myAdapter.Dispose()
            For Each dtMyRow In dtMyDati.Rows
                ValDettPag(myPag.ID, myPag.sAnno, dtMyRow("impdett"), dtMyRow("capitolo"), dtMyRow("voce"), dtMyRow("tipopartita"), dtMyRow("categoria"), dtMyRow("ncomponenti"), myPag.sOperatore, listDettaglio)
                impDettagliato += FormatNumber(dtMyRow("impdett"), 2)
            Next
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.ScorporoPagamento.errore: ", ex)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Throw ex
        Finally
            dtMyDati.Dispose()
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Sub

    Public Function DettagliaPagamenti(ByVal MyStringConnection As String, ByVal myPag As OggettoPagamenti) As Integer
        '{-1= non a buon fine, 1= buon fine}
        Try
            Dim oListDettaglio() As ObjDettaglioPagamento = Nothing
            Dim nImpDettagliato As Double
            Dim nList As Integer = -1
            Dim bCopriStato As Boolean = True

            nImpDettagliato = 0 : nList = -1
            If Not IsDBNull(myPag.dImportoPagamento) Then
                'inserisco in dettaglio la maggiorazione inputata da operatore
                If myPag.dImportoStat <> 0 Then
                    ValDettPag(myPag.ID, myPag.sAnno, myPag.dImportoStat, RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjDetVoci.Capitolo.Maggiorazione, "", RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjArticolo.PARTEMAGGIORAZIONE, "", 0, myPag.sOperatore, oListDettaglio)
                    nImpDettagliato += myPag.dImportoStat
                    bCopriStato = False
                    myPag.dImportoPagamento -= myPag.dImportoStat
                End If
                If FormatNumber(CDbl(myPag.dImportoPagamento), 2) <> 0 Then
                    ScorporoPagamento(MyStringConnection, myPag, bCopriStato, oListDettaglio, nImpDettagliato)
                End If
                'se la somma delle voci dettagliate non corrisponde al pagato:
                If FormatNumber((myPag.dImportoPagamento + myPag.dImportoStat) - nImpDettagliato, 2) <> "0,00" Then
                    ValDettPag(myPag.ID, myPag.sAnno, FormatNumber(((myPag.dImportoPagamento + myPag.dImportoStat) - nImpDettagliato), 2), RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjDetVoci.Capitolo.Arrotondamento, "", RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjArticolo.PARTEFISSA, "", 0, myPag.sOperatore, oListDettaglio)
                End If
                'se l’inserimento del dettaglio pagamento sulla base dati richiamando la funzione SetPagamentoDettagliato(oListDettaglio) è andato a buon fine:
                If SetPagamentoDettagliato(MyStringConnection, oListDettaglio) > 0 Then
                    'registro l’avvenuto dettaglio del pagamento sulla base dati richiamando la funzione SetPagamento(2);
                    Dim oMyPag As New OggettoPagamenti
                    oMyPag.ID = myPag.ID
                    If SetPagamento(MyStringConnection, oMyPag, OggettoPagamenti.TypeOperation.IsSplitted) <= 0 Then
                        Return 0
                    End If
                End If
            End If
            Return 1
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsGestPag.DettgliaPagamenti.errore: ", Err)
            Return -1
        End Try
    End Function
    ''' <summary>
    ''' Classe per la gestione cache dell'elaborazione asincrona
    ''' </summary>
    Public Class CacheManager
        Private Shared IISCache As System.Web.Caching.Cache = HttpRuntime.Cache
        Private Shared AvanzamentoElaborazioneKey As String = "AvanzamentoElaborazione"
        Private Shared Log As ILog = LogManager.GetLogger(GetType(CacheManager))
#Region "Avanzamento Elaborazione"
        Public Shared Function GetAvanzamentoElaborazione() As String
            Try
                If (Not (IISCache(AvanzamentoElaborazioneKey)) Is Nothing) Then
                    Return CType(IISCache(AvanzamentoElaborazioneKey), String)
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.CacheManager.GetAvanzamentoElaborazione.errore: ", ex)
                Return Nothing
            End Try
        End Function
        Public Shared Sub SetAvanzamentoElaborazione(ByVal sMyDati As String)
            IISCache.Insert(AvanzamentoElaborazioneKey, sMyDati, Nothing, System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.NotRemovable, Nothing)
        End Sub
        Public Shared Sub RemoveAvanzamentoElaborazione()
            IISCache.Remove(AvanzamentoElaborazioneKey)
        End Sub
    End Class
#End Region
    '*** ***
End Class
'*** ***