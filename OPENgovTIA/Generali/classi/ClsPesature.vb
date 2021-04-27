Imports log4net
Imports OPENUtility
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
Imports Utility
''' <summary>
''' Definizione oggetto riepilogo pesature
''' </summary>
Public Class ObjRiepilogoPesature
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ObjRiepilogoPesature))
    Private _IdTessera As Integer = -1
    Private _sAnno As String = ""
    Private _nKG As Double = 0
    Private _nVolume As Double = 0
    Private _nConferimenti As Integer = 0
    Private _TipoConferimento As String = ""

    Public Property IdTessera() As Integer
        Get
            Return _IdTessera
        End Get
        Set(ByVal Value As Integer)
            _IdTessera = Value
        End Set
    End Property
    Public Property sAnno() As String
        Get
            Return _sAnno
        End Get
        Set(ByVal Value As String)
            _sAnno = Value
        End Set
    End Property
    Public Property nKG() As Double
        Get
            Return _nKG
        End Get
        Set(ByVal Value As Double)
            _nKG = Value
        End Set
    End Property
    Public Property nVolume() As Double
        Get
            Return _nVolume
        End Get
        Set(ByVal Value As Double)
            _nVolume = Value
        End Set
    End Property
    Public Property nConferimenti() As Integer
        Get
            Return _nConferimenti
        End Get
        Set(ByVal Value As Integer)
            _nConferimenti = Value
        End Set
    End Property
    Public Property TipoConferimento() As String
        Get
            Return _TipoConferimento
        End Get
        Set(ByVal Value As String)
            _TipoConferimento = Value
        End Set
    End Property
End Class
''' <summary>
''' Definizione oggetto ricerca pesature
''' </summary>
Public Class ObjRicercaUtentePesature
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ObjRicercaUtentePesature))
    Private oReplace As New generalClass.generalFunction
    'Private sNameDBAnag As String = ConfigurationManager.AppSettings("NOME_DATABASE_ANAGRAFICA")
    Private _sEnte As String = ""
    Private _sCognome As String = ""
    Private _sNome As String = ""
    Private _sCodFiscale As String = ""
    Private _sPIva As String = ""
    Private _sCodUtente As String = ""
    Private _sNumTessera As String = ""
    Private _nCodTessera As Integer = -1
    Private _tPeriodoDal As Date = Date.MaxValue
    Private _tPeriodoAl As Date = Date.MaxValue
    Private _IsFatturato As Integer = 2
    Private _sFileImport As String = ""

    Public Property sEnte() As String
        Get
            Return _sEnte
        End Get
        Set(ByVal Value As String)
            _sEnte = Value
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
    Public Property sCodFiscale() As String
        Get
            Return _sCodFiscale
        End Get
        Set(ByVal Value As String)
            _sCodFiscale = Value
        End Set
    End Property
    Public Property sPIva() As String
        Get
            Return _sPIva
        End Get
        Set(ByVal Value As String)
            _sPIva = Value
        End Set
    End Property
    Public Property sCodUtente() As String
        Get
            Return _sCodUtente
        End Get
        Set(ByVal Value As String)
            _sCodUtente = Value
        End Set
    End Property
    Public Property sNumTessera() As String
        Get
            Return _sNumTessera
        End Get
        Set(ByVal Value As String)
            _sNumTessera = Value
        End Set
    End Property
    Public Property nCodTessera() As Integer
        Get
            Return _nCodTessera
        End Get
        Set(ByVal Value As Integer)
            _nCodTessera = Value
        End Set
    End Property
    Public Property tPeriodoDal() As Date
        Get
            Return _tPeriodoDal
        End Get
        Set(ByVal Value As Date)
            _tPeriodoDal = Value
        End Set
    End Property
    Public Property tPeriodoAl() As Date
        Get
            Return _tPeriodoAl
        End Get
        Set(ByVal Value As Date)
            _tPeriodoAl = Value
        End Set
    End Property
    Public Property IsFatturato() As Integer
        Get
            Return _IsFatturato
        End Get
        Set(ByVal Value As Integer)
            _IsFatturato = Value
        End Set
    End Property
    Public Property sFileImport() As String
        Get
            Return _sFileImport
        End Get
        Set(ByVal Value As String)
            _sFileImport = Value
        End Set
    End Property

    'Public Function GetRicercaPesature(ByVal oMyRicerca As ObjRicercaUtentePesature) As ObjUtentePesature()
    '    dim sSQL as string
    '    Dim DrReturn As SqlClient.SqlDataReader
    '    Dim WFErrore As String = ""
    '    Dim WFSessione As CreateSessione
    '    Dim oListUtenti() As ObjUtentePesature
    '    Dim oMyUtentePesature As ObjUtentePesature
    '    Dim nList As Integer = -1

    '    Try
    '        'inizializzo la connessione
    '        WFSessione = New CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        'sSQL = "SELECT IDCONTRIBUENTE, COGNOME_DENOMINAZIONE AS COGNOME, NOME,"
    '        'sSQL += " CF_PIVA= CASE WHEN NOT PARTITA_IVA IS NULL AND PARTITA_IVA <>'' THEN PARTITA_IVA ELSE COD_FISCALE END,"
    '        'sSQL += " VIA_RES, CIVICO_RES, ESPONENTE_CIVICO_RES, INTERNO_CIVICO_RES, SCALA_CIVICO_RES,"
    '        'sSQL += " TBLTESSERE.ID, CODICE_UTENTE, NUMERO_TESSERA, TBLTESSERE.CODICE_TESSERA, SUM(KG) AS TOTKG, COUNT(NConferimenti) AS TOTConferimenti"
    '        'sSQL += " FROM TBLTESSERE"
    '        'sSQL += " INNER JOIN " & sNameDBAnag & ".DBO.ANAGRAFICA ON TBLTESSERE.IDCONTRIBUENTE=" & sNameDBAnag & ".DBO.ANAGRAFICA.COD_CONTRIBUENTE"
    '        'sSQL += " INNER JOIN TBLPESATURE ON TBLTESSERE.ID=TBLPESATURE.IDTESSERA"
    '        'sSQL += " WHERE (DATA_FINE_VALIDITA IS NULL) AND (TBLTESSERE.DATA_VARIAZIONE IS NULL) AND (TBLPESATURE.DATA_VARIAZIONE IS NULL)"
    '        'sSQL += " AND (TBLTESSERE.IDENTE='" & oMyRicerca.sEnte & "')"
    '        'If oMyRicerca.sCognome <> "" Then
    '        '    sSQL += " AND (COGNOME_DENOMINAZIONE LIKE '" & oReplace.ReplaceCharsForSearch(oMyRicerca.sCognome) & "%')"
    '        'End If
    '        'If oMyRicerca.sNome <> "" Then
    '        '    sSQL += " AND (NOME LIKE '" & oReplace.ReplaceCharsForSearch(oMyRicerca.sNome) & "%')"
    '        'End If
    '        'If oMyRicerca.sCodFiscale <> "" Then
    '        '    sSQL += " AND (COD_FISCALE LIKE '" & oReplace.ReplaceCharsForSearch(oMyRicerca.sCodFiscale) & "%')"
    '        'End If
    '        'If oMyRicerca.sPIva <> "" Then
    '        '    sSQL += " AND (PARTITA_IVA LIKE '" & oReplace.ReplaceCharsForSearch(oMyRicerca.sPIva) & "%')"
    '        'End If
    '        'If oMyRicerca.sCodUtente <> "" Then
    '        '    sSQL += " AND (CODICE_UTENTE='" & oMyRicerca.sCodUtente & "')"
    '        'End If
    '        'If oMyRicerca.sNumTessera <> "" Then
    '        '    sSQL += " AND (NUMERO_TESSERA='" & oMyRicerca.sNumTessera & "')"
    '        'End If
    '        'If oMyRicerca.nCodTessera <> -1 Then
    '        '    sSQL += " AND (TBLTESSERE.CODICE_TESSERA ='" & oMyRicerca.nCodTessera & "')"
    '        'End If
    '        'If oMyRicerca.tPeriodoDal <> Date.MinValue Then
    '        '    sSQL += " AND (DATA_ORA>='" & oReplace.ReplaceDataForDB(oMyRicerca.tPeriodoDal) & "' AND DATA_ORA<='" & oReplace.ReplaceDataForDB(oMyRicerca.tPeriodoAl) & "')"
    '        'End If
    '        'sSQL += " GROUP BY IDCONTRIBUENTE, COGNOME_DENOMINAZIONE, NOME, CASE WHEN NOT PARTITA_IVA IS NULL AND PARTITA_IVA <>'' THEN PARTITA_IVA ELSE COD_FISCALE END, VIA_RES, CIVICO_RES, ESPONENTE_CIVICO_RES, INTERNO_CIVICO_RES, SCALA_CIVICO_RES,"
    '        'sSQL += " TBLTESSERE.ID, CODICE_UTENTE, NUMERO_TESSERA, TBLTESSERE.CODICE_TESSERA"
    '        'sSQL += " ORDER BY COGNOME_DENOMINAZIONE, NOME, VIA_RES, CIVICO_RES, ESPONENTE_CIVICO_RES, INTERNO_CIVICO_RES, SCALA_CIVICO_RES,"
    '        'sSQL += " CODICE_UTENTE, NUMERO_TESSERA, TBLTESSERE.CODICE_TESSERA"
    '        sSQL = "SELECT *"
    '        sSQL += " FROM V_RICERCAPESATURE"
    '        sSQL += " WHERE (IDENTE='" & oMyRicerca.sEnte & "')"
    '        If oMyRicerca.sCognome <> "" Then
    '            sSQL += " AND (COGNOME LIKE '" & oReplace.ReplaceCharsForSearch(oMyRicerca.sCognome) & "%')"
    '        End If
    '        If oMyRicerca.sNome <> "" Then
    '            sSQL += " AND (NOME LIKE '" & oReplace.ReplaceCharsForSearch(oMyRicerca.sNome) & "%')"
    '        End If
    '        If oMyRicerca.sCodFiscale <> "" Then
    '            sSQL += " AND (CF_PIVA LIKE '" & oReplace.ReplaceCharsForSearch(oMyRicerca.sCodFiscale) & "%')"
    '        End If
    '        If oMyRicerca.sPIva <> "" Then
    '            sSQL += " AND (CF_PIVA LIKE '" & oReplace.ReplaceCharsForSearch(oMyRicerca.sPIva) & "%')"
    '        End If
    '        If oMyRicerca.sCodUtente <> "" Then
    '            sSQL += " AND (CODICE_UTENTE='" & oMyRicerca.sCodUtente & "')"
    '        End If
    '        If oMyRicerca.sNumTessera <> "" Then
    '            sSQL += " AND (NUMERO_TESSERA='" & oMyRicerca.sNumTessera & "')"
    '        End If
    '        If oMyRicerca.nCodTessera <> -1 Then
    '            sSQL += " AND (CODICE_TESSERA ='" & oMyRicerca.nCodTessera & "')"
    '        End If
    '        If oMyRicerca.tPeriodoDal <> Date.MinValue Then
    '            sSQL += " AND (DATA_ORA>='" & oReplace.ReplaceDataForDB(oMyRicerca.tPeriodoDal) & "' AND DATA_ORA<='" & oReplace.ReplaceDataForDB(oMyRicerca.tPeriodoAl) & "')"
    '        End If
    '        sSQL += " ORDER BY COGNOME, NOME"
    '        sSQL += ", CODICE_UTENTE, NUMERO_TESSERA, CODICE_TESSERA"
    '        'eseguo la query
    '        DrReturn = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '        Do While DrReturn.Read
    '            'incremento l'indice
    '            nList += 1
    '            oMyUtentePesature = New ObjUtentePesature

    '            oMyUtentePesature.nIdContribuente = stringoperation.formatint(DrReturn("idcontribuente"))
    '            oMyUtentePesature.sCognome = stringoperation.formatstring(DrReturn("cognome"))
    '            oMyUtentePesature.sNome = stringoperation.formatstring(DrReturn("nome"))
    '            oMyUtentePesature.sCFPIva = stringoperation.formatstring(DrReturn("cf_piva"))
    '            oMyUtentePesature.sVia = stringoperation.formatstring(DrReturn("via_res"))
    '            If Not IsDBNull(DrReturn("civico_res")) Then
    '                oMyUtentePesature.sCivico = stringoperation.formatstring(DrReturn("civico_res"))
    '            End If
    '            oMyUtentePesature.sEsponente = stringoperation.formatstring(DrReturn("esponente_civico_res"))
    '            oMyUtentePesature.sInterno = stringoperation.formatstring(DrReturn("interno_civico_res"))
    '            oMyUtentePesature.sScala = stringoperation.formatstring(DrReturn("scala_civico_res"))
    '            oMyUtentePesature.nIdTessera = stringoperation.formatint(DrReturn("id"))
    '            oMyUtentePesature.sCodUtente = stringoperation.formatstring(DrReturn("codice_utente"))
    '            oMyUtentePesature.sNumTessera = stringoperation.formatstring(DrReturn("numero_tessera"))
    '            'oMyUtentePesature.nCodTessera = stringoperation.formatint(DrReturn("codice_tessera"))
    '            oMyUtentePesature.nTotKg = stringoperation.formatdouble(DrReturn("totkg"))
    '            oMyUtentePesature.nTotvolume = stringoperation.formatdouble(DrReturn("totvolume"))
    '            oMyUtentePesature.nTotConferimenti = stringoperation.formatint(DrReturn("totConferimenti"))
    '            'dimensiono l'array
    '            ReDim Preserve oListUtenti(nList)
    '            'memorizzo i dati nell'array
    '            oListUtenti(nList) = oMyUtentePesature
    '        Loop

    '        Return oListUtenti
    '    Catch Err As Exception
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ObjRicercaUtentePesature.GetRicercaPesature.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in ObjRicercaUtentePesature::GetRicercaPesature::" & Err.Message & " SQL::" & sSQL)
    '        Return Nothing
    '    Finally
    '        DrReturn.Close()
    '        'chiudo la connessione
    '        If Not WFSessione Is Nothing Then
    '            WFSessione.Kill()
    '        End If
    '    End Try
    'End Function

    Public Sub GetTotalizzatori(ByVal oListUtentiPesature() As ObjUtentePesature, ByRef nTotUtenti As Integer, ByRef nTotKG As Double, ByRef nTotVolume As Double, ByRef nTotConferimenti As Integer)
        Dim x, nContribPrec As Integer
        Try
            For x = 0 To oListUtentiPesature.GetUpperBound(0)
                If nContribPrec <> oListUtentiPesature(x).nIdContribuente Then
                    nTotUtenti += 1
                End If
                nTotKG += oListUtentiPesature(x).nTotKg
                nTotVolume += oListUtentiPesature(x).nTotVolume
                nTotConferimenti += oListUtentiPesature(x).nTotConferimenti
            Next
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ObjRicercaUtentePesature.GetTotalizzatori.errore: ", Err)
        End Try
    End Sub
    '*** 20131104 - TARES ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oMyRicerca"></param>
    ''' <param name="cmdMyCommandOut"></param>
    ''' <returns></returns>
    Public Function GetRicercaPesature(ByVal oMyRicerca As ObjRicercaUtentePesature, ByRef cmdMyCommandOut As SqlClient.SqlCommand) As ObjUtentePesature()
        Dim oListUtenti() As ObjUtentePesature = Nothing
        Dim oMyUtentePesature As ObjUtentePesature
        Dim nList As Integer = -1
        Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dtMyDati As New DataTable()
        Dim dtMyRow As DataRow

        Try
            If ConstSession.IsFromVariabile("1") = "1" Then 'sempre IsFromVariabile
            End If
            If (Not (cmdMyCommandOut) Is Nothing) Then
                cmdMyCommand = cmdMyCommandOut
            Else
                cmdMyCommand = New SqlClient.SqlCommand
                cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
                cmdMyCommand.Connection.Open()
            End If
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure

            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.AddWithValue("@IdEnte", oMyRicerca.sEnte)
            cmdMyCommand.Parameters.AddWithValue("@COGNOME", oMyRicerca.sCognome)
            cmdMyCommand.Parameters.AddWithValue("@NOME", oMyRicerca.sNome)
            cmdMyCommand.Parameters.AddWithValue("@CF", oMyRicerca.sCodFiscale)
            cmdMyCommand.Parameters.AddWithValue("@PIVA", oMyRicerca.sPIva)
            cmdMyCommand.Parameters.AddWithValue("@CODUTENTE", oMyRicerca.sCodUtente)
            cmdMyCommand.Parameters.AddWithValue("@NUMEROTESSERA", oMyRicerca.sNumTessera)
            cmdMyCommand.Parameters.AddWithValue("@DAL", oMyRicerca.tPeriodoDal)
            cmdMyCommand.Parameters.AddWithValue("@AL", oMyRicerca.tPeriodoAl)
            cmdMyCommand.Parameters.AddWithValue("@ISFATTURATO", oMyRicerca.IsFatturato)
            cmdMyCommand.Parameters.AddWithValue("@FILEIMPORT", oMyRicerca.sFileImport)
            cmdMyCommand.CommandText = "prc_GetRicercaPesature"
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(dtMyDati)
            myAdapter.Dispose()
            For Each dtMyRow In dtMyDati.Rows
                'incremento l'indice
                nList += 1
                oMyUtentePesature = New ObjUtentePesature

                oMyUtentePesature.nIdContribuente = StringOperation.FormatInt(dtMyRow("idcontribuente"))
                oMyUtentePesature.sCognome = StringOperation.FormatString(dtMyRow("cognome"))
                oMyUtentePesature.sNome = StringOperation.FormatString(dtMyRow("nome"))
                oMyUtentePesature.sCFPIva = StringOperation.FormatString(dtMyRow("cf_piva"))
                oMyUtentePesature.sVia = StringOperation.FormatString(dtMyRow("via_res"))
                If Not IsDBNull(dtMyRow("civico_res")) Then
                    oMyUtentePesature.sCivico = StringOperation.FormatString(dtMyRow("civico_res"))
                End If
                oMyUtentePesature.sEsponente = StringOperation.FormatString(dtMyRow("esponente_civico_res"))
                oMyUtentePesature.sInterno = StringOperation.FormatString(dtMyRow("interno_civico_res"))
                oMyUtentePesature.sScala = StringOperation.FormatString(dtMyRow("scala_civico_res"))
                oMyUtentePesature.nIdTessera = StringOperation.FormatInt(dtMyRow("id"))
                oMyUtentePesature.sCodUtente = StringOperation.FormatString(dtMyRow("codice_utente"))
                oMyUtentePesature.sNumTessera = StringOperation.FormatString(dtMyRow("numero_tessera"))
                'oMyUtentePesature.nCodTessera = stringoperation.formatint(dtmyrow("codice_tessera"))
                oMyUtentePesature.nTotKg = StringOperation.FormatDouble(dtMyRow("totkg"))
                oMyUtentePesature.nTotVolume = StringOperation.FormatDouble(dtMyRow("totvolume"))
                oMyUtentePesature.nTotConferimenti = StringOperation.FormatInt(dtMyRow("totConferimenti"))
                oMyUtentePesature.IsFatturato = StringOperation.FormatInt(dtMyRow("isfatturato"))
                'dimensiono l'array
                ReDim Preserve oListUtenti(nList)
                'memorizzo i dati nell'array
                oListUtenti(nList) = oMyUtentePesature
            Next
            Return oListUtenti
        Catch Err As Exception
            Log.Debug(oMyRicerca.sEnte + " - OPENgovTIA.ObjRicercaUtentePesature.GetRicercaPesature.errore: connessione-> " + cmdMyCommand.Connection.ConnectionString, Err)
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
    '*** ***
End Class
''' <summary>
''' Definizione oggetto pesature per utente
''' </summary>
Public Class ObjUtentePesature
    Private _nIdContribuente As Integer = -1
    Private _sCognome As String = ""
    Private _sNome As String = ""
    Private _sCFPIva As String = ""
    Private _sVia As String = ""
    Private _sCivico As String = ""
    Private _sEsponente As String = ""
    Private _sInterno As String = ""
    Private _sScala As String = ""
    Private _nIdTessera As Integer = -1
    Private _sCodUtente As String = ""
    Private _sNumTessera As String = ""
    Private _nCodTessera As Integer = -1
    Private _nTotKg As Double = -1
    Private _nTotVolume As Double = -1
    Private _nTotConferimenti As Integer = -1
    Private _IsFatturato As Integer = 0

    Public Property nIdContribuente() As Integer
        Get
            Return _nIdContribuente
        End Get
        Set(ByVal Value As Integer)
            _nIdContribuente = Value
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
    Public Property sCFPIva() As String
        Get
            Return _sCFPIva
        End Get
        Set(ByVal Value As String)
            _sCFPIva = Value
        End Set
    End Property
    Public Property sVia() As String
        Get
            Return _sVia
        End Get
        Set(ByVal Value As String)
            _sVia = Value
        End Set
    End Property
    Public Property sCivico() As String
        Get
            Return _sCivico
        End Get
        Set(ByVal Value As String)
            _sCivico = Value
        End Set
    End Property
    Public Property sEsponente() As String
        Get
            Return _sEsponente
        End Get
        Set(ByVal Value As String)
            _sEsponente = Value
        End Set
    End Property
    Public Property sInterno() As String
        Get
            Return _sInterno
        End Get
        Set(ByVal Value As String)
            _sInterno = Value
        End Set
    End Property
    Public Property sScala() As String
        Get
            Return _sScala
        End Get
        Set(ByVal Value As String)
            _sScala = Value
        End Set
    End Property
    Public Property nIdTessera() As Integer
        Get
            Return _nIdTessera
        End Get
        Set(ByVal Value As Integer)
            _nIdTessera = Value
        End Set
    End Property
    Public Property sCodUtente() As String
        Get
            Return _sCodUtente
        End Get
        Set(ByVal Value As String)
            _sCodUtente = Value
        End Set
    End Property
    Public Property sNumTessera() As String
        Get
            Return _sNumTessera
        End Get
        Set(ByVal Value As String)
            _sNumTessera = Value
        End Set
    End Property
    Public Property nCodTessera() As Integer
        Get
            Return _nCodTessera
        End Get
        Set(ByVal Value As Integer)
            _nCodTessera = Value
        End Set
    End Property
    Public Property nTotKg() As Double
        Get
            Return _nTotKg
        End Get
        Set(ByVal Value As Double)
            _nTotKg = Value
        End Set
    End Property
    Public Property nTotVolume() As Double
        Get
            Return _nTotVolume
        End Get
        Set(ByVal Value As Double)
            _nTotVolume = Value
        End Set
    End Property
    Public Property nTotConferimenti() As Integer
        Get
            Return _nTotConferimenti
        End Get
        Set(ByVal Value As Integer)
            _nTotConferimenti = Value
        End Set
    End Property
    Public Property IsFatturato() As Integer
        Get
            Return _IsFatturato
        End Get
        Set(ByVal Value As Integer)
            _IsFatturato = Value
        End Set
    End Property
End Class
''' <summary>
''' Definizione oggetto pesature per tessera
''' </summary>
Public Class ObjTesseraPesature
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ObjRicercaUtentePesature))
    Private oReplace As New generalClass.generalFunction
    Private _oTessera As ObjTessera = Nothing
    Private _oPesature() As ObjPesatura = Nothing

    Public Property oTessera() As ObjTessera
        Get
            Return _oTessera
        End Get
        Set(ByVal Value As ObjTessera)
            _oTessera = Value
        End Set
    End Property
    Public Property oPesature() As ObjPesatura()
        Get
            Return _oPesature
        End Get
        Set(ByVal Value As ObjPesatura())
            _oPesature = Value
        End Set
    End Property

    'Public Function GetTesseraPesature(ByVal WFSessione As CreateSessione, ByVal nIdTessera As Integer) As ObjTesseraPesature
    '    Dim oMyTessera() As ObjTessera
    '    Dim FunctionTessera As New GestTessera
    '    Dim oListPesature() As ObjPesatura
    '    Dim FunctionPesatura As New GestPesatura
    '    Dim oMyTesseraPesature As New ObjTesseraPesature

    '    Try
    '        'prelevo i dati della tessera
    '        oMyTessera = FunctionTessera.GetTessera(WFSessione, "", -1, -1, "", nIdTessera, "", -1, True, False)
    '        If oMyTessera Is Nothing Then
    '            Return Nothing
    '        End If
    '        'prelevo le pesature
    '        oListPesature = FunctionPesatura.GetPesatura(Nothing, -1, nIdTessera)
    '        If oListPesature Is Nothing Then
    '            Return Nothing
    '        End If
    '        oMyTesseraPesature.oTessera = oMyTessera(0)
    '        oMyTesseraPesature.oPesature = oListPesature

    '        Return oMyTesseraPesature
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ObjTesseraPesature.GetTesseraPesature.errore: ", Err)
    '        Return Nothing
    '    End Try
    'End Function
    Public Function GetTesseraPesature(ByVal nIdTessera As Integer, ByVal sIdEnte As String, ByVal myConnectionString As String) As ObjTesseraPesature
        Dim oMyTessera() As ObjTessera
        Dim FunctionTessera As New GestTessera
        Dim oListPesature() As ObjPesatura
        Dim FunctionPesatura As New GestPesatura
        Dim oMyTesseraPesature As New ObjTesseraPesature

        Try
            'prelevo i dati della tessera
            oMyTessera = FunctionTessera.GetTessera(myConnectionString, sIdEnte, -1, -1, "", nIdTessera, "", -1, True, False)
            If oMyTessera Is Nothing Then
                Return Nothing
            End If
            'prelevo le pesature
            'oListPesature = FunctionPesatura.GetPesatura(myConnectionString, -1, nIdTessera)
            oListPesature = FunctionPesatura.GetPesatura(myConnectionString, -1, oMyTessera(0).Id, oMyTessera(0).sNumeroTessera)
            'If oListPesature Is Nothing Then
            '    Return Nothing
            'End If
            oMyTesseraPesature.oTessera = oMyTessera(0)
            oMyTesseraPesature.oPesature = oListPesature

            Return oMyTesseraPesature
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ObjTesseraPesature.GetTesseraPesature.errore: ", Err)
            Return Nothing
        End Try
    End Function

    'Public Function GetMqTARSU(ByVal sEnte As String, ByVal nContrib As Integer) As Integer
    '    Dim sSQL, WFErrore As String
    '    Dim WFSessione As CreateSessione
    '    Dim DrReturn As SqlClient.SqlDataReader
    '    Try
    '        Dim nMQTARSU As Double = -1
    '        'apro la connessione sul db TARSU
    '        WFSessione = New CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConfigurationManager.AppSettings("OPENGOVTA").ToString())
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If
    '        sSQL = "SELECT SUM(MQ) AS TOTMQ"
    '        sSQL += " FROM TBLOGGETTITARSU"
    '        sSQL += " INNER JOIN TBLDETTAGLIOTESTATATARSU ON TBLOGGETTITARSU.IDDETTAGLIOTESTATA=TBLDETTAGLIOTESTATATARSU.ID"
    '        sSQL += " INNER JOIN TBLTESTATATARSU ON TBLDETTAGLIOTESTATATARSU.IDTESTATA=TBLTESTATATARSU.ID"
    '        sSQL += " WHERE ((CASE WHEN DATA_FINE IS NULL THEN '" & oReplace.ReplaceDataForDB(Now.ToString) & "' ELSE DATA_FINE END)<='" & oReplace.ReplaceDataForDB(Now.ToString) & "')"
    '        sSQL += " AND (IDENTE='" & sEnte & "')"
    '        sSQL += " AND (IDCONTRIBUENTE=" & nContrib & ")"
    '        'eseguo la query
    '        DrReturn = WFSessione.oSession.oAppDB.GetPrivateDataReader(sSQL)
    '        Do While DrReturn.Read
    '            If Not IsDBNull(DrReturn("totmq")) Then
    '                nMQTARSU = stringoperation.formatdouble(DrReturn("totmq"))
    '            End If
    '        Loop
    '        Return nMQTARSU
    '    Catch Err As Exception
    '        Log.Debug("Si è verificato un errore in ObjTesseraPesature::GetMqTARSU::" & Err.Message & " SQL::" & sSQL)
    '   Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ObjTesseraPesature.GetMqTARSU.errore: ", Err)
    '        Return -1
    '    Finally
    '        DrReturn.Close()
    '        If Not WFSessione Is Nothing Then
    '            WFSessione.Kill()
    '        End If
    '    End Try
    'End Function
    Public Function GetMqTARSU(ByVal oMyTessera As ObjTessera) As Integer
        Try
            Dim nMQTARSU As Double = 0
            For Each myUI As ObjDettaglioTestata In oMyTessera.oImmobili
                nMQTARSU += myUI.nMQTassabili
            Next
            Return nMQTARSU
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ObjTesseraPesature.GetMqTARSU.errore: ", Err)
            Return -1
        End Try
    End Function
End Class
''' <summary>
''' Classe gestione pesature
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class GestPesatura
    Private Shared Log As ILog = LogManager.GetLogger(GetType(GestPesatura))
    Private oReplace As New generalClass.generalFunction
    Private DrReturn As SqlClient.SqlDataReader

    'Public Function GetPesatura(ByRef DBEngineOut As DBEngine, Optional ByVal nIdPesatura As Integer = -1, Optional ByVal nIdTessera As Integer = -1) As ObjPesatura()
    '    Dim oMyPesatura As ObjPesatura
    '    Dim oListPesature() As ObjPesatura
    '    Dim nPesature As Integer = -1
    '    Dim MyDBEngine As DBEngine = Nothing
    '    Dim dtMyDati As New DataTable()
    '    Dim dtMyRow As DataRow

    '    Try
    '        If (Not (DBEngineOut) Is Nothing) Then
    '            MyDBEngine = DBEngineOut
    '        Else
    '            MyDBEngine = Utility.DBEngineFactory.GetDBEngine(ConstSession.StringConnection)
    '            MyDBEngine.OpenConnection()
    '            MyDBEngine.BeginTransaction()
    '        End If
    '        MyDBEngine.ClearParameters()
    '        MyDBEngine.AddParameter("@ID", nIdPesatura, ParameterDirection.Input)
    '        MyDBEngine.AddParameter("@IDTESSERA", nIdTessera, ParameterDirection.Input)
    '        MyDBEngine.ExecuteQuery(dtMyDati, "prc_GetPesature", CommandType.StoredProcedure)
    '        For Each dtMyRow In dtMyDati.Rows
    '            oMyPesatura = New ObjPesatura

    '            oMyPesatura.Id = stringoperation.formatint(dtMyRow("id"))
    '            oMyPesatura.IdPesatura = stringoperation.formatint(dtMyRow("idpesatura"))
    '            oMyPesatura.IdEnte = stringoperation.formatstring(dtMyRow("idente"))
    '            oMyPesatura.IdFlusso = stringoperation.formatint(dtMyRow("idflusso"))
    '            oMyPesatura.IdTessera = stringoperation.formatint(dtMyRow("idtessera"))
    '            oMyPesatura.tDataOraConferimento = stringoperation.formatdatetime(dtMyRow("data_ora"))
    '            oMyPesatura.nKG = stringoperation.formatdouble(dtMyRow("kg"))
    '            If Not IsDBNull(dtMyRow("volume")) Then
    '                oMyPesatura.nVolume = stringoperation.formatdouble(dtMyRow("volume"))
    '            End If
    '            If Not IsDBNull(dtMyRow("codice_interno")) Then
    '                oMyPesatura.sCodInterno = stringoperation.formatstring(dtMyRow("codice_interno"))
    '            End If
    '            If Not IsDBNull(dtMyRow("cod_utente")) Then
    '                oMyPesatura.sCodUtente = stringoperation.formatstring(dtMyRow("cod_utente"))
    '            End If
    '            If Not IsDBNull(dtMyRow("comune")) Then
    '                oMyPesatura.sComune = stringoperation.formatstring(dtMyRow("comune"))
    '            End If
    '            If Not IsDBNull(dtMyRow("numero_tessera")) Then
    '                oMyPesatura.sNumeroTessera = stringoperation.formatstring(dtMyRow("numero_tessera"))
    '            End If
    '            If Not IsDBNull(dtMyRow("punto_conferimento")) Then
    '                oMyPesatura.sPuntoConferimento = stringoperation.formatstring(dtMyRow("punto_conferimento"))
    '            End If
    '            oMyPesatura.tDataInserimento = stringoperation.formatdatetime(dtMyRow("data_inserimento"))
    '            If Not IsDBNull(dtMyRow("data_variazione")) Then
    '                oMyPesatura.tDataVariazione = stringoperation.formatdatetime(dtMyRow("data_variazione"))
    '            End If
    '            If Not IsDBNull(dtMyRow("data_cessazione")) Then
    '                oMyPesatura.tDataCessazione = stringoperation.formatdatetime(dtMyRow("data_cessazione"))
    '            End If
    '            oMyPesatura.sOperatore = stringoperation.formatstring(dtMyRow("operatore"))
    '            nPesature += 1
    '            ReDim Preserve oListPesature(nPesature)
    '            oListPesature(nPesature) = oMyPesatura
    '        Next

    '        Return oListPesature
    '    Catch Err As Exception
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPesatura.GetPesatura.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in ObjPesatura::GetPesatura::" & Err.Message & " SQL::" & cmdMyCommand.CommandText)
    '        Return Nothing
    '    Finally
    '        dtMyDati.Dispose()
    '        If (DBEngineOut Is Nothing) Then
    '            MyDBEngine.CloseConnection()
    '        End If
    '    End Try
    'End Function

    'Public Function SetPesatura(ByVal oPesatura As ObjPesatura, ByVal DbOperation As Integer, ByVal WFSessione As CreateSessione, Optional ByVal nIdTesseraDel As Integer = -1, Optional ByVal tDalDel As String = "", Optional ByVal tAlDel As String = "", Optional ByVal sEnteDel As String = "") As Integer
    '    Try
    '        Dim myIdentity As Integer

    '        Select Case DbOperation
    '            Case Costanti.AZIONE_NEW
    '                'DATA E ORA|SISTEMA|CODICE INTERNO|PESO|VOLUME|N° TESSERA|CODICE UTENTE|COMUNE 
    '                cmdMyCommand.CommandText = "INSERT INTO TBLPESATURE (IDPESATURA, IDENTE, IDFLUSSO, IDTESSERA"
    '                cmdMyCommand.CommandText += ", DATA_ORA, KG, VOLUME"
    '                cmdMyCommand.CommandText += ", NUMERO_TESSERA, CODICE_INTERNO, COD_UTENTE"
    '                cmdMyCommand.CommandText += ", COMUNE, PUNTO_CONFERIMENTO"
    '                cmdMyCommand.CommandText += ", DATA_INSERIMENTO, OPERATORE)"
    '                cmdMyCommand.CommandText += " VALUES (@IDPESATURA,@IDENTE,@IDFLUSSO,@IDTESSERA"
    '                cmdMyCommand.CommandText += ",@DATA_ORA,@KG,@VOLUME"
    '                cmdMyCommand.CommandText += ",@NUMERO_TESSERA,@CODICE_INTERNO,@COD_UTENTE"
    '                cmdMyCommand.CommandText += ",@COMUNE,@PUNTO_CONFERIMENTO"
    '                cmdMyCommand.CommandText += ",@DATA_INSERIMENTO,@OPERATORE)"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPESATURA", SqlDbType.Int)).Value = oPesatura.IdPesatura
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oPesatura.IdEnte
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSO", SqlDbType.Int)).Value = oPesatura.IdFlusso
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTESSERA", SqlDbType.Int)).Value = oPesatura.IdTessera
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_ORA", SqlDbType.DateTime)).Value = oPesatura.tDataOraConferimento
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@KG", SqlDbType.Float)).Value = oPesatura.nKG
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VOLUME", SqlDbType.Float)).Value = oPesatura.nVolume
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO_TESSERA", SqlDbType.NVarChar)).Value = oPesatura.sNumeroTessera
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE_INTERNO", SqlDbType.NVarChar)).Value = oPesatura.sCodInterno
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COD_UTENTE", SqlDbType.NVarChar)).Value = oPesatura.sCodUtente
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COMUNE", SqlDbType.NVarChar)).Value = oPesatura.sComune
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PUNTO_CONFERIMENTO", SqlDbType.NVarChar)).Value = oPesatura.sPuntoConferimento
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INSERIMENTO", SqlDbType.DateTime)).Value = oPesatura.tDataInserimento
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@OPERATORE", SqlDbType.NVarChar)).Value = oPesatura.sOperatore
    '                cmdMyCommand.CommandText += " SELECT @@IDENTITY"
    '                'eseguo la query
    '                Dim DrReturn As SqlClient.SqlDataReader
    '                DrReturn = WFSessione.oSession.oAppDB.GetPrivateDataReader(cmdMyCommand)
    '                Do While DrReturn.Read
    '                    myIdentity = DrReturn(0)
    '                Loop
    '                DrReturn.Close()
    '                'controllo se devo aggiornare l'IDPESATURA
    '                If oPesatura.IdPesatura = -1 Then
    '                    cmdMyCommand.CommandText = "UPDATE TBLPESATURE SET IDPESATURA=ID"
    '                    cmdMyCommand.CommandText += " WHERE (ID=@ID)"
    '                    'valorizzo i parameters:
    '                    cmdMyCommand.Parameters.Clear()
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = myIdentity
    '                    If WFSessione.oSession.oAppDB.Execute(cmdMyCommand) < 0 Then
    '                        Return 0
    '                    End If
    '                End If
    '            Case Costanti.AZIONE_DELETE
    '                cmdMyCommand.CommandText = "DELETE"
    '                cmdMyCommand.CommandText += " FROM TBLPESATURE"
    '                cmdMyCommand.CommandText += " WHERE 1=1"
    '                'valorizzo i parameters:
    '                cmdMyCommand.Parameters.Clear()
    '                If nIdTesseraDel <> -1 Then
    '                    cmdMyCommand.CommandText += " AND (IDTESSERA=@IDTESSERA)"
    '                    cmdMyCommand.CommandText += " AND (DATA_ORA>=@DATADAL"
    '                    cmdMyCommand.CommandText += " AND DATA_ORA<=@DATAAL)"
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTESSERA", SqlDbType.Int)).Value = nIdTesseraDel
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATADAL", SqlDbType.DateTime)).Value = tDalDel
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAAL", SqlDbType.DateTime)).Value = tAlDel
    '                Else
    '                    cmdMyCommand.CommandText += " AND (ID=@ID)"
    '                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.NVarChar)).Value = oPesatura.Id
    '                End If
    '                'eseguo la query
    '                If WFSessione.oSession.oAppDB.Execute(cmdMyCommand) < 0 Then
    '                    Return 0
    '                End If
    '                myIdentity = 1
    '        End Select
    '        Return myIdentity
    '    Catch Err As Exception
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPesatura.SetPesatura.errore: ", Err)
    '        Log.Debug("Si è verificato un errore in ObjPesatura::SetPesatura::" & Err.Message & " SQL::" & cmdMyCommand.CommandText)
    '        Return 0
    '    End Try
    'End Function
    Public Function GetPesatura(ByVal myConnectionString As String, ByVal nIdPesatura As Integer, ByVal nIdTessera As Integer, sNTessera As String) As ObjPesatura()
        Dim oMyPesatura As ObjPesatura
        Dim oListPesature() As ObjPesatura = Nothing
        Dim nPesature As Integer = -1
        Dim sSQL As String = ""
        Dim myDataView As New DataView
        Dim nMyReturn As Integer = -1

        Try
            Using ctx As New DBModel(ConstSession.DBType, myConnectionString)
                Try
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetPesature", "ID", "IDTESSERA", "NTESSERA")
                    myDataView = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("ID", nIdPesatura) _
                            , ctx.GetParam("IDTESSERA", nIdTessera) _
                            , ctx.GetParam("NTESSERA", sNTessera)
                        )
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestPesatura.GetPesatura.erroreQuery: ", ex)
                    Return Nothing
                Finally
                    ctx.Dispose()
                End Try
                For Each myRow As DataRowView In myDataView
                    oMyPesatura = New ObjPesatura

                    oMyPesatura.Id = StringOperation.FormatInt(myRow("id"))
                    oMyPesatura.IdPesatura = StringOperation.FormatInt(myRow("idpesatura"))
                    oMyPesatura.IdEnte = StringOperation.FormatString(myRow("idente"))
                    oMyPesatura.IdFlusso = StringOperation.FormatInt(myRow("idflusso"))
                    oMyPesatura.IdTessera = StringOperation.FormatInt(myRow("idtessera"))
                    oMyPesatura.tDataOraConferimento = StringOperation.FormatDateTime(myRow("data_ora"))
                    oMyPesatura.nLitri = StringOperation.FormatDouble(myRow("kg"))
                    oMyPesatura.nVolume = StringOperation.FormatDouble(myRow("volume"))
                    oMyPesatura.sCodInterno = StringOperation.FormatString(myRow("codice_interno"))
                    oMyPesatura.sCodUtente = StringOperation.FormatString(myRow("cod_utente"))
                    oMyPesatura.sComune = StringOperation.FormatString(myRow("comune"))
                    oMyPesatura.sNumeroTessera = StringOperation.FormatString(myRow("numero_tessera"))
                    '*** 201712 - gestione tipo conferimento ***
                    oMyPesatura.sTipoConferimento = StringOperation.FormatString(myRow("tipoconferimento"))
                    '*** ***
                    oMyPesatura.sPuntoConferimento = StringOperation.FormatString(myRow("punto_conferimento"))
                    oMyPesatura.tDataInserimento = StringOperation.FormatDateTime(myRow("data_inserimento"))
                    oMyPesatura.tDataVariazione = StringOperation.FormatDateTime(myRow("data_variazione"))
                    oMyPesatura.tDataCessazione = StringOperation.FormatDateTime(myRow("data_cessazione"))
                    oMyPesatura.sOperatore = StringOperation.FormatString(myRow("operatore"))
                    nPesature += 1
                    ReDim Preserve oListPesature(nPesature)
                    oListPesature(nPesature) = oMyPesatura
                Next
            End Using
            Return oListPesature
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestPesatura.GetPesatura.errore: ", Err)
            Return Nothing
        Finally
            myDataView.Dispose()
        End Try
    End Function
    'Public Function GetPesatura(ByVal myConnectionString As String, ByVal nIdPesatura As Integer, ByVal nIdTessera As Integer, sNTessera As String) As ObjPesatura()
    '    Dim oMyPesatura As ObjPesatura
    '    Dim oListPesature() As ObjPesatura = Nothing
    '    Dim nPesature As Integer = -1
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim dtMyDati As New DataTable()
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim dtMyRow As DataRow

    '    Try
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.AddWithValue("@ID", nIdPesatura)
    '        cmdMyCommand.Parameters.AddWithValue("@IDTESSERA", nIdTessera)
    '        cmdMyCommand.Parameters.AddWithValue("@NTESSERA", sNTessera)
    '        cmdMyCommand.CommandText = "prc_GetPesature"
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(dtMyDati)
    '        For Each dtMyRow In dtMyDati.Rows
    '            oMyPesatura = New ObjPesatura

    '            oMyPesatura.Id = stringoperation.formatint(dtMyRow("id"))
    '            oMyPesatura.IdPesatura = stringoperation.formatint(dtMyRow("idpesatura"))
    '            oMyPesatura.IdEnte = stringoperation.formatstring(dtMyRow("idente"))
    '            oMyPesatura.IdFlusso = stringoperation.formatint(dtMyRow("idflusso"))
    '            oMyPesatura.IdTessera = stringoperation.formatint(dtMyRow("idtessera"))
    '            oMyPesatura.tDataOraConferimento = stringoperation.formatdatetime(dtMyRow("data_ora"))
    '            oMyPesatura.nLitri = stringoperation.formatdouble(dtMyRow("kg"))
    '            If Not IsDBNull(dtMyRow("volume")) Then
    '                oMyPesatura.nVolume = stringoperation.formatdouble(dtMyRow("volume"))
    '            End If
    '            If Not IsDBNull(dtMyRow("codice_interno")) Then
    '                oMyPesatura.sCodInterno = stringoperation.formatstring(dtMyRow("codice_interno"))
    '            End If
    '            If Not IsDBNull(dtMyRow("cod_utente")) Then
    '                oMyPesatura.sCodUtente = stringoperation.formatstring(dtMyRow("cod_utente"))
    '            End If
    '            If Not IsDBNull(dtMyRow("comune")) Then
    '                oMyPesatura.sComune = stringoperation.formatstring(dtMyRow("comune"))
    '            End If
    '            If Not IsDBNull(dtMyRow("numero_tessera")) Then
    '                oMyPesatura.sNumeroTessera = stringoperation.formatstring(dtMyRow("numero_tessera"))
    '            End If
    '            '*** 201712 - gestione tipo conferimento ***
    '            If Not IsDBNull(dtMyRow("tipoconferimento")) Then
    '                oMyPesatura.sTipoConferimento = stringoperation.formatstring(dtMyRow("tipoconferimento"))
    '            End If
    '            '*** ***
    '            If Not IsDBNull(dtMyRow("punto_conferimento")) Then
    '                oMyPesatura.sPuntoConferimento = stringoperation.formatstring(dtMyRow("punto_conferimento"))
    '            End If
    '            oMyPesatura.tDataInserimento = stringoperation.formatdatetime(dtMyRow("data_inserimento"))
    '            If Not IsDBNull(dtMyRow("data_variazione")) Then
    '                oMyPesatura.tDataVariazione = stringoperation.formatdatetime(dtMyRow("data_variazione"))
    '            End If
    '            If Not IsDBNull(dtMyRow("data_cessazione")) Then
    '                oMyPesatura.tDataCessazione = stringoperation.formatdatetime(dtMyRow("data_cessazione"))
    '            End If
    '            oMyPesatura.sOperatore = stringoperation.formatstring(dtMyRow("operatore"))
    '            nPesature += 1
    '            ReDim Preserve oListPesature(nPesature)
    '            oListPesature(nPesature) = oMyPesatura
    '        Next

    '        Return oListPesature
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestPesatura.GetPesatura.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return Nothing
    '    Finally
    '        myAdapter.Dispose()
    '        dtMyDati.Dispose()
    '        cmdMyCommand.Dispose()
    '        cmdMyCommand.Connection.Close()
    '    End Try
    'End Function

    Public Function SetPesatura(ByVal myConnectionString As String, ByVal oPesatura As ObjPesatura, ByVal DbOperation As Integer) As Integer
        'Public Function SetPesatura(ByVal myConnectionString As String, ByVal oPesatura As ObjPesatura, ByVal DbOperation As Integer, Optional ByVal nIdTesseraDel As Integer = -1, Optional ByVal tDalDel As String = "", Optional ByVal tAlDel As String = "", Optional ByVal sEnteDel As String = "") As Integer
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            If DbOperation = Utility.Costanti.AZIONE_DELETE Then
                'cmdMyCommand.CommandText = "DELETE"
                'cmdMyCommand.CommandText += " FROM TBLPESATURE"
                'cmdMyCommand.CommandText += " WHERE 1=1"
                ''valorizzo i parameters:
                'cmdMyCommand.Parameters.Clear()
                'If nIdTesseraDel <> -1 Then
                '    cmdMyCommand.CommandText += " AND (IDTESSERA=@IDTESSERA)"
                '    cmdMyCommand.CommandText += " AND (DATA_ORA>=@DATADAL"
                '    cmdMyCommand.CommandText += " AND DATA_ORA<=@DATAAL)"
                '    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTESSERA", SqlDbType.Int)).Value = nIdTesseraDel
                '    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATADAL", SqlDbType.DateTime)).Value = tDalDel
                '    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATAAL", SqlDbType.DateTime)).Value = tAlDel
                'Else
                '    cmdMyCommand.CommandText += " AND (ID=@ID)"
                '    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.NVarChar)).Value = oPesatura.Id
                'End If
                cmdMyCommand.CommandText = "prc_TBLPESATURE_D"
                cmdMyCommand.Parameters.Clear()
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = oPesatura.Id
                'eseguo la query
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                If cmdMyCommand.ExecuteNonQuery < 0 Then
                    Return 0
                End If
            Else
                cmdMyCommand.CommandText = "prc_TBLPESATURE_IU"
                'valorizzo i parameters:
                cmdMyCommand.Parameters.Clear()
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ID", SqlDbType.Int)).Value = oPesatura.Id
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPESATURA", SqlDbType.Int)).Value = oPesatura.IdPesatura
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTE", SqlDbType.NVarChar)).Value = oPesatura.IdEnte
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSO", SqlDbType.Int)).Value = oPesatura.IdFlusso
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTESSERA", SqlDbType.Int)).Value = oPesatura.IdTessera
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_ORA", SqlDbType.DateTime)).Value = oPesatura.tDataOraConferimento
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@KG", SqlDbType.Float)).Value = oPesatura.nLitri
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VOLUME", SqlDbType.Float)).Value = oPesatura.nVolume
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NUMERO_TESSERA", SqlDbType.NVarChar)).Value = oPesatura.sNumeroTessera
                '*** 201712 - gestione tipo conferimento ***
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPOCONFERIMENTO", SqlDbType.NVarChar)).Value = oPesatura.sTipoConferimento
                '*** ***
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODICE_INTERNO", SqlDbType.NVarChar)).Value = oPesatura.sCodInterno
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COD_UTENTE", SqlDbType.NVarChar)).Value = oPesatura.sCodUtente
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COMUNE", SqlDbType.NVarChar)).Value = oPesatura.sComune
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PUNTO_CONFERIMENTO", SqlDbType.NVarChar)).Value = oPesatura.sPuntoConferimento
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_INSERIMENTO", SqlDbType.DateTime)).Value = oPesatura.tDataInserimento
                If oPesatura.tDataVariazione = DateTime.MinValue Or oPesatura.tDataVariazione = DateTime.MaxValue Then
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = DBNull.Value
                Else
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_VARIAZIONE", SqlDbType.DateTime)).Value = oPesatura.tDataVariazione
                End If
                If oPesatura.tDataCessazione = DateTime.MinValue Or oPesatura.tDataCessazione = DateTime.MaxValue Then
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = DBNull.Value
                Else
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DATA_CESSAZIONE", SqlDbType.DateTime)).Value = oPesatura.tDataCessazione
                End If
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@OPERATORE", SqlDbType.NVarChar)).Value = oPesatura.sOperatore
                cmdMyCommand.Parameters("@ID").Direction = ParameterDirection.InputOutput
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                cmdMyCommand.ExecuteNonQuery()
                oPesatura.Id = cmdMyCommand.Parameters("@ID").Value
            End If
            Return oPesatura.Id
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestPesatura.SetPesatura.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return 0
        Finally
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function

    Public Sub SetTesseraVSPesatura(myConnectionString As String, IdOld As Integer, IdNew As Integer)
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Try
            cmdMyCommand = New SqlClient.SqlCommand
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
            cmdMyCommand.CommandTimeout = 0
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            'valorizzo i parameters:
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTESSERAOLD", SqlDbType.Int)).Value = IdOld
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDTESSERANEW", SqlDbType.Int)).Value = IdNew
            'Valorizzo il commandtext:
            cmdMyCommand.CommandText = "prc_SetTesserePesature"
            'eseguo la query
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            cmdMyCommand.ExecuteNonQuery()
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestPesatura.SetTesseraVSPesatura.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
        Finally
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Sub
    ''' <summary>
    ''' Funzione per Esportazione completa dati
    ''' </summary>
    ''' <param name="myConnectionString">string</param>
    ''' <param name="IdEnte">string</param>
    ''' <returns>dataview</returns>
    Public Function GetExportPesature(ByVal myConnectionString As String, IdEnte As String) As DataView
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dtMyDati As New DataTable()
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(myConnectionString)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.AddWithValue("@IDENTE", IdEnte)
            cmdMyCommand.CommandText = "prc_GetExportPesature"
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(dtMyDati)
            Return dtMyDati.DefaultView
        Catch Err As Exception
            Log.Debug(IdEnte + " - OPENgovTIA.GestPesatura.GetExportPesature.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        Finally
            myAdapter.Dispose()
            dtMyDati.Dispose()
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function
End Class