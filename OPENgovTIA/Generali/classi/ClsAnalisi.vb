Imports log4net
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
Imports Utility

'*** 20130201 - gestione mq da catasto per TARES ***
''' <summary>
''' Definizione oggetto per il controllo sui riferimenti catastali
''' </summary>
Public Class oUIVSCat
    Private _nId As Integer = -1
    Private _nIdArticoloRuolo As Integer = -1
    Private _nIdFlussoRuolo As Integer = -1
    Private _nIdContribuente As Integer = -1
    Private _sAnno As String = ""
    Private _sCognome As String = ""
    Private _sNome As String = ""
    Private _sCodFiscalePIva As String = ""
    Private _sViaRes As String = ""
    Private _sCivicoRes As String = ""
    Private _sEsponenteRes As String = ""
    Private _sInternoRes As String = ""
    Private _sScalaRes As String = ""
    Private _sCAPRes As String = ""
    Private _sComuneRes As String = ""
    Private _sPVRes As String = ""
    Private _sVia As String = ""
    Private _sCivico As String = ""
    Private _sEsponente As String = ""
    Private _sInterno As String = ""
    Private _sScala As String = ""
    Private _sFoglio As String = ""
    Private _sNumero As String = ""
    Private _sSubalterno As String = ""
    Private _sIdCategoria As String = ""
    Private _sDescrCategoria As String = ""
    Private _nMq As Double = 0
    Private _nBimestri As Integer = -1
    Private _impTariffa As Double = 0
    Private _impArticolo As Double = 0
    Private _impRiduzioni As Double = 0
    Private _impDetassazioni As Double = 0
    Private _impNetto As Double = 0
    Private _impSanzioni As Double = 0
    Private _impInteressi As Double = 0
    Private _impSpeseNot As Double = 0
    Private _IsBloccato As Integer = 0
    Private _tDataInizio As DateTime = Date.MinValue
    Private _tDataFine As DateTime = Date.MinValue
    Private _nIdTestata As Integer = 0
    Private _nIdDettaglioTestata As Integer = 0
    Private _nMQCat As Double = 0
    Private _nMQDif As Double = 0

    Public Property nId() As Integer
        Get
            Return _nId
        End Get
        Set(ByVal Value As Integer)
            _nId = Value
        End Set
    End Property
    Public Property nIdArticoloRuolo() As Integer
        Get
            Return _nIdArticoloRuolo
        End Get
        Set(ByVal Value As Integer)
            _nIdArticoloRuolo = Value
        End Set
    End Property
    Public Property nIdFlussoRuolo() As Integer
        Get
            Return _nIdFlussoRuolo
        End Get
        Set(ByVal Value As Integer)
            _nIdFlussoRuolo = Value
        End Set
    End Property
    Public Property nIdContribuente() As Integer
        Get
            Return _nIdContribuente
        End Get
        Set(ByVal Value As Integer)
            _nIdContribuente = Value
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
    Public Property sCodFiscalePIva() As String
        Get
            Return _sCodFiscalePIva
        End Get
        Set(ByVal Value As String)
            _sCodFiscalePIva = Value
        End Set
    End Property
    Public Property sViaRes() As String
        Get
            Return _sViaRes
        End Get
        Set(ByVal Value As String)
            _sViaRes = Value
        End Set
    End Property
    Public Property sCivicoRes() As String
        Get
            Return _sCivicoRes
        End Get
        Set(ByVal Value As String)
            _sCivicoRes = Value
        End Set
    End Property
    Public Property sInternoRes() As String
        Get
            Return _sInternoRes
        End Get
        Set(ByVal Value As String)
            _sInternoRes = Value
        End Set
    End Property
    Public Property sEsponenteRes() As String
        Get
            Return _sEsponenteRes
        End Get
        Set(ByVal Value As String)
            _sEsponenteRes = Value
        End Set
    End Property
    Public Property sScalaRes() As String
        Get
            Return _sScalaRes
        End Get
        Set(ByVal Value As String)
            _sScalaRes = Value
        End Set
    End Property
    Public Property sCAPRes() As String
        Get
            Return _sCAPRes
        End Get
        Set(ByVal Value As String)
            _sCAPRes = Value
        End Set
    End Property
    Public Property sComuneRes() As String
        Get
            Return _sComuneRes
        End Get
        Set(ByVal Value As String)
            _sComuneRes = Value
        End Set
    End Property
    Public Property sPVRes() As String
        Get
            Return _sPVRes
        End Get
        Set(ByVal Value As String)
            _sPVRes = Value
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
    Public Property sInterno() As String
        Get
            Return _sInterno
        End Get
        Set(ByVal Value As String)
            _sInterno = Value
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
    Public Property sScala() As String
        Get
            Return _sScala
        End Get
        Set(ByVal Value As String)
            _sScala = Value
        End Set
    End Property
    Public Property sFoglio() As String
        Get
            Return _sFoglio
        End Get
        Set(ByVal Value As String)
            _sFoglio = Value
        End Set
    End Property
    Public Property sNumero() As String
        Get
            Return _sNumero
        End Get
        Set(ByVal Value As String)
            _sNumero = Value
        End Set
    End Property
    Public Property sSubalterno() As String
        Get
            Return _sSubalterno
        End Get
        Set(ByVal Value As String)
            _sSubalterno = Value
        End Set
    End Property
    Public Property sIdCategoria() As String
        Get
            Return _sIdCategoria
        End Get
        Set(ByVal Value As String)
            _sIdCategoria = Value
        End Set
    End Property
    Public Property sDescrCategoria() As String
        Get
            Return _sDescrCategoria
        End Get
        Set(ByVal Value As String)
            _sDescrCategoria = Value
        End Set
    End Property
    Public Property nMQ() As Double
        Get
            Return _nMq
        End Get
        Set(ByVal Value As Double)
            _nMq = Value
        End Set
    End Property
    Public Property nBimestri() As Integer
        Get
            Return _nBimestri
        End Get
        Set(ByVal Value As Integer)
            _nBimestri = Value
        End Set
    End Property
    Public Property impTariffa() As Double
        Get
            Return _impTariffa
        End Get
        Set(ByVal Value As Double)
            _impTariffa = Value
        End Set
    End Property
    Public Property impArticolo() As Double
        Get
            Return _impArticolo
        End Get
        Set(ByVal Value As Double)
            _impArticolo = Value
        End Set
    End Property
    Public Property impRiduzioni() As Double
        Get
            Return _impRiduzioni
        End Get
        Set(ByVal Value As Double)
            _impRiduzioni = Value
        End Set
    End Property
    Public Property impDetassazioni() As Double
        Get
            Return _impDetassazioni
        End Get
        Set(ByVal Value As Double)
            _impDetassazioni = Value
        End Set
    End Property
    Public Property impNetto() As Double
        Get
            Return _impNetto
        End Get
        Set(ByVal Value As Double)
            _impNetto = Value
        End Set
    End Property
    Public Property impSanzioni() As Double
        Get
            Return _impSanzioni
        End Get
        Set(ByVal Value As Double)
            _impSanzioni = Value
        End Set
    End Property
    Public Property impInteressi() As Double
        Get
            Return _impInteressi
        End Get
        Set(ByVal Value As Double)
            _impInteressi = Value
        End Set
    End Property
    Public Property impSpeseNot() As Double
        Get
            Return _impSpeseNot
        End Get
        Set(ByVal Value As Double)
            _impSpeseNot = Value
        End Set
    End Property
    Public Property IsBloccato() As Integer
        Get
            Return _IsBloccato
        End Get
        Set(ByVal Value As Integer)
            _IsBloccato = Value
        End Set
    End Property
    Public Property tDataInizio() As DateTime
        Get
            Return _tDataInizio
        End Get
        Set(ByVal Value As DateTime)
            _tDataInizio = Value
        End Set
    End Property
    Public Property tDataFine() As DateTime
        Get
            Return _tDataFine
        End Get
        Set(ByVal Value As DateTime)
            _tDataFine = Value
        End Set
    End Property
    Public Property nIdTestata() As Integer
        Get
            Return _nIdTestata
        End Get
        Set(ByVal Value As Integer)
            _nIdTestata = Value
        End Set
    End Property
    Public Property nIdDettaglioTestata() As Integer
        Get
            Return _nIdDettaglioTestata
        End Get
        Set(ByVal Value As Integer)
            _nIdDettaglioTestata = Value
        End Set
    End Property
    Public Property nMQCat() As Double
        Get
            Return _nMQCat
        End Get
        Set(ByVal Value As Double)
            _nMQCat = Value
        End Set
    End Property
    Public Property nMQDif() As Double
        Get
            Return _nMQDif
        End Get
        Set(ByVal Value As Double)
            _nMQDif = Value
        End Set
    End Property
End Class
'*** ***
''' <summary>
''' Classe per il controllo sui riferimenti catastali
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class ClsAnalisi
    Private Shared Log As ILog = LogManager.GetLogger("ClsAnalisi")
    Private oReplace As New generalClass.generalFunction
    Private cmdMyCommand As New SqlClient.SqlCommand
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sIdEnte"></param>
    ''' <param name="nAnno"></param>
    ''' <param name="nTypeCheck"></param>
    ''' <param name="nTolleranzaNeg"></param>
    ''' <param name="nTolleranzaPos"></param>
    ''' <param name="nPercentMQCat"></param>
    ''' <param name="sOperatore"></param>
    ''' <param name="sDal"></param>
    ''' <param name="sAl"></param>
    ''' <returns></returns>
    Public Function GetResultCheckRifCatastali(ByVal sIdEnte As String, ByVal nAnno As Integer, ByVal nTypeCheck As Integer, ByVal nTolleranzaNeg As Double, ByVal nTolleranzaPos As Double, ByVal nPercentMQCat As Double, ByVal sOperatore As String, ByVal sDal As String, ByVal sAl As String) As oUIVSCat()
        'Dim oListRifCat() As ObjAnagArticolo
        Dim oListRifCat() As oUIVSCat = Nothing

        Try
            Select Case nTypeCheck
                Case 0              'nessuna selezione
                Case 1              'riferimenti chiusi e non riaperti
                    oListRifCat = CheckRifCatastaliChiusi(sIdEnte, nAnno, sOperatore, sDal, sAl)
                Case 2              'riferimenti presenti in più periodi contemporaneamente
                    oListRifCat = CheckRifCatastaliDoppi(sIdEnte, nAnno, sOperatore, sDal, sAl)
                Case 3              'riferimenti mancanti
                    oListRifCat = CheckRifCatastaliMancanti(sIdEnte, nAnno, sOperatore, sDal, sAl)
                Case 4              'riferimenti accertati
                    oListRifCat = CheckRifCatastaliAccertati(sIdEnte, nAnno, sOperatore, sDal, sAl)
                Case 9
                    oListRifCat = CheckRifCatNoDich(sIdEnte, nAnno, sOperatore, sDal, sAl)
                Case 10
                    oListRifCat = CheckRifDichNoCat(sIdEnte, nAnno, sOperatore, sDal, sAl)
                Case 11, 12            'dichiarazioni modificate
                    oListRifCat = CheckDichMod(sIdEnte, nAnno, sOperatore, sDal, sAl, nTypeCheck)
                Case Else                 '0=uguali; 1=diversi;2=Cat>Dich;3=Cat<Dich
                    oListRifCat = CheckMQDichVSCat(sIdEnte, nAnno, nTypeCheck - 5, nTolleranzaNeg, nTolleranzaPos, nPercentMQCat, sOperatore, sDal, sAl)
            End Select
            Return oListRifCat
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsAnalisi.GetResultCheckRifCatastali.errore: ", Err)
            Return Nothing
        End Try
    End Function

#Region "GET LIST - Controllo su Riferimenti Catastali"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sIdEnte"></param>
    ''' <param name="nAnno"></param>
    ''' <param name="sOperatore"></param>
    ''' <param name="sDal"></param>
    ''' <param name="sAl"></param>
    ''' <returns></returns>
    Public Function CheckRifCatastaliChiusi(ByVal sIdEnte As String, ByVal nAnno As Integer, ByVal sOperatore As String, ByVal sDal As String, ByVal sAl As String) As oUIVSCat()
        Dim oListRifCat() As oUIVSCat = Nothing
        Dim oMyRifCat As oUIVSCat
        Dim dvMyDati As New DataView
        Dim x As Integer

        Try
            dvMyDati = GetRifCatastaliChiusi(sIdEnte, nAnno, sOperatore, sDal, sAl)
            For x = 0 To dvMyDati.Count - 1
                oMyRifCat = New oUIVSCat                  'ObjAnagArticolo
                If Not IsDBNull(dvMyDati.Item(x)("idcontribuente")) Then
                    oMyRifCat.nIdContribuente = CStr(dvMyDati.Item(x)("idcontribuente"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("idtestata")) Then
                    oMyRifCat.nIdTestata = CStr(dvMyDati.Item(x)("idtestata"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("iddettagliotestata")) Then
                    oMyRifCat.nIdDettaglioTestata = CStr(dvMyDati.Item(x)("iddettagliotestata"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("cognome")) Then
                    oMyRifCat.sCognome = CStr(dvMyDati.Item(x)("cognome"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("nome")) Then
                    oMyRifCat.sNome = CStr(dvMyDati.Item(x)("nome"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("cfpiva")) Then
                    oMyRifCat.sCodFiscalePIva = CStr(dvMyDati.Item(x)("cfpiva"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("via")) Then
                    oMyRifCat.sVia = CStr(dvMyDati.Item(x)("via"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("civico")) Then
                    oMyRifCat.sCivico = CStr(dvMyDati.Item(x)("civico"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("esponente")) Then
                    oMyRifCat.sEsponente = CStr(dvMyDati.Item(x)("esponente"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("scala")) Then
                    oMyRifCat.sScala = CStr(dvMyDati.Item(x)("scala"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("interno")) Then
                    oMyRifCat.sInterno = CStr(dvMyDati.Item(x)("interno"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("foglio")) Then
                    oMyRifCat.sFoglio = CStr(dvMyDati.Item(x)("foglio"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("numero")) Then
                    oMyRifCat.sNumero = CStr(dvMyDati.Item(x)("numero"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("subalterno")) Then
                    oMyRifCat.sSubalterno = CStr(dvMyDati.Item(x)("subalterno"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("data_inizio")) Then
                    oMyRifCat.tDataInizio = CDate(dvMyDati.Item(x)("data_inizio"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("data_fine")) Then
                    oMyRifCat.tDataFine = CDate(dvMyDati.Item(x)("data_fine"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("mq")) Then
                    oMyRifCat.nMQ = CDbl(dvMyDati.Item(x)("mq"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("idcategoria")) Then
                    oMyRifCat.sIdCategoria = CStr(dvMyDati.Item(x)("idcategoria"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("descrcategoria")) Then
                    oMyRifCat.sDescrCategoria = CStr(dvMyDati.Item(x)("descrcategoria"))
                End If
                ReDim Preserve oListRifCat(x)
                oListRifCat(x) = oMyRifCat
            Next

            Return oListRifCat
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsAnalisi.CheckRifCatastaliChiusi.errore: ", Err)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sIdEnte"></param>
    ''' <param name="nAnno"></param>
    ''' <param name="sOperatore"></param>
    ''' <param name="sDal"></param>
    ''' <param name="sAl"></param>
    ''' <returns></returns>
    Public Function CheckRifCatastaliDoppi(ByVal sIdEnte As String, ByVal nAnno As Integer, ByVal sOperatore As String, ByVal sDal As String, ByVal sAl As String) As oUIVSCat()
        Dim oListRifCat() As oUIVSCat = Nothing
        Dim oMyRifCat As oUIVSCat
        Dim oMyDettaglioTestata As ObjDettaglioTestata
        Dim dvMyDati As New DataView
        Dim x, nList As Integer

        Try
            nList = -1
            dvMyDati = GetRifCatastali(sIdEnte, nAnno, sOperatore, sDal, sAl)
            For x = 0 To dvMyDati.Count - 1
                oMyRifCat = New oUIVSCat
                oMyDettaglioTestata = New ObjDettaglioTestata
                If Not IsDBNull(dvMyDati.Item(x)("idcontribuente")) Then
                    oMyRifCat.nIdContribuente = CStr(dvMyDati.Item(x)("idcontribuente"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("idtestata")) Then
                    oMyRifCat.nIdTestata = CStr(dvMyDati.Item(x)("idtestata"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("iddettagliotestata")) Then
                    oMyRifCat.nIdDettaglioTestata = CStr(dvMyDati.Item(x)("iddettagliotestata"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("foglio")) Then
                    oMyDettaglioTestata.sFoglio = CStr(dvMyDati.Item(x)("foglio"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("numero")) Then
                    oMyDettaglioTestata.sNumero = CStr(dvMyDati.Item(x)("numero"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("subalterno")) Then
                    oMyDettaglioTestata.sSubalterno = CStr(dvMyDati.Item(x)("subalterno"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("data_inizio")) Then
                    oMyDettaglioTestata.tDataInizio = CDate(dvMyDati.Item(x)("data_inizio"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("data_fine")) Then
                    oMyDettaglioTestata.tDataFine = CDate(dvMyDati.Item(x)("data_fine"))
                End If
                If oMyDettaglioTestata.sFoglio <> "" Then
                    'controllo che i rif.catastali non siano già presenti per lo stesso periodo 
                    If CheckUniqueRifCatastali(sIdEnte, oMyDettaglioTestata, CInt(dvMyDati.Item(x)("id"))) = False Then ', WFSessione
                        If Not IsDBNull(dvMyDati.Item(x)("cognome")) Then
                            oMyRifCat.sCognome = CStr(dvMyDati.Item(x)("cognome"))
                        End If
                        If Not IsDBNull(dvMyDati.Item(x)("nome")) Then
                            oMyRifCat.sNome = CStr(dvMyDati.Item(x)("nome"))
                        End If
                        If Not IsDBNull(dvMyDati.Item(x)("cfpiva")) Then
                            oMyRifCat.sCodFiscalePIva = CStr(dvMyDati.Item(x)("cfpiva"))
                        End If
                        If Not IsDBNull(dvMyDati.Item(x)("via")) Then
                            oMyRifCat.sVia = CStr(dvMyDati.Item(x)("via"))
                        End If
                        If Not IsDBNull(dvMyDati.Item(x)("civico")) Then
                            oMyRifCat.sCivico = CStr(dvMyDati.Item(x)("civico"))
                        End If
                        If Not IsDBNull(dvMyDati.Item(x)("esponente")) Then
                            oMyRifCat.sEsponente = CStr(dvMyDati.Item(x)("esponente"))
                        End If
                        If Not IsDBNull(dvMyDati.Item(x)("scala")) Then
                            oMyRifCat.sScala = CStr(dvMyDati.Item(x)("scala"))
                        End If
                        If Not IsDBNull(dvMyDati.Item(x)("interno")) Then
                            oMyRifCat.sInterno = CStr(dvMyDati.Item(x)("interno"))
                        End If
                        If Not IsDBNull(dvMyDati.Item(x)("foglio")) Then
                            oMyRifCat.sFoglio = CStr(dvMyDati.Item(x)("foglio"))
                        End If
                        If Not IsDBNull(dvMyDati.Item(x)("numero")) Then
                            oMyRifCat.sNumero = CStr(dvMyDati.Item(x)("numero"))
                        End If
                        If Not IsDBNull(dvMyDati.Item(x)("subalterno")) Then
                            oMyRifCat.sSubalterno = CStr(dvMyDati.Item(x)("subalterno"))
                        End If
                        If Not IsDBNull(dvMyDati.Item(x)("data_inizio")) Then
                            oMyRifCat.tDataInizio = CDate(dvMyDati.Item(x)("data_inizio"))
                        End If
                        If Not IsDBNull(dvMyDati.Item(x)("data_fine")) Then
                            oMyRifCat.tDataFine = CDate(dvMyDati.Item(x)("data_fine"))
                        End If
                        If Not IsDBNull(dvMyDati.Item(x)("mq")) Then
                            oMyRifCat.nMQ = CDbl(dvMyDati.Item(x)("mq"))
                        End If
                        If Not IsDBNull(dvMyDati.Item(x)("idcategoria")) Then
                            oMyRifCat.sIdCategoria = CStr(dvMyDati.Item(x)("idcategoria"))
                        End If
                        If Not IsDBNull(dvMyDati.Item(x)("descrcategoria")) Then
                            oMyRifCat.sDescrCategoria = CStr(dvMyDati.Item(x)("descrcategoria"))
                        End If
                        nList += 1
                        ReDim Preserve oListRifCat(nList)
                        oListRifCat(nList) = oMyRifCat
                    End If
                End If
            Next

            Return oListRifCat
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsAnalisi.CheckRifCatastaliDoppi.errore: ", Err)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sIdEnte"></param>
    ''' <param name="nAnno"></param>
    ''' <param name="sOperatore"></param>
    ''' <param name="sDal"></param>
    ''' <param name="sAl"></param>
    ''' <returns></returns>
    Public Function CheckRifCatastaliMancanti(ByVal sIdEnte As String, ByVal nAnno As Integer, ByVal sOperatore As String, ByVal sDal As String, ByVal sAl As String) As oUIVSCat()
        Dim oListRifCat() As oUIVSCat = Nothing
        Dim oMyRifCat As oUIVSCat
        Dim dvMyDati As New DataView
        Dim x As Integer

        Try
            dvMyDati = GetRifCatastaliMancanti(sIdEnte, nAnno, sOperatore, sDal, sAl)
            For x = 0 To dvMyDati.Count - 1
                oMyRifCat = New oUIVSCat                  'ObjAnagArticolo
                If Not IsDBNull(dvMyDati.Item(x)("idcontribuente")) Then
                    oMyRifCat.nIdContribuente = CStr(dvMyDati.Item(x)("idcontribuente"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("idtestata")) Then
                    oMyRifCat.nIdTestata = CStr(dvMyDati.Item(x)("idtestata"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("iddettagliotestata")) Then
                    oMyRifCat.nIdDettaglioTestata = CStr(dvMyDati.Item(x)("iddettagliotestata"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("cognome")) Then
                    oMyRifCat.sCognome = CStr(dvMyDati.Item(x)("cognome"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("nome")) Then
                    oMyRifCat.sNome = CStr(dvMyDati.Item(x)("nome"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("cfpiva")) Then
                    oMyRifCat.sCodFiscalePIva = CStr(dvMyDati.Item(x)("cfpiva"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("via")) Then
                    oMyRifCat.sVia = CStr(dvMyDati.Item(x)("via"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("civico")) Then
                    oMyRifCat.sCivico = CStr(dvMyDati.Item(x)("civico"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("esponente")) Then
                    oMyRifCat.sEsponente = CStr(dvMyDati.Item(x)("esponente"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("scala")) Then
                    oMyRifCat.sScala = CStr(dvMyDati.Item(x)("scala"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("interno")) Then
                    oMyRifCat.sInterno = CStr(dvMyDati.Item(x)("interno"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("foglio")) Then
                    oMyRifCat.sFoglio = CStr(dvMyDati.Item(x)("foglio"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("numero")) Then
                    oMyRifCat.sNumero = CStr(dvMyDati.Item(x)("numero"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("subalterno")) Then
                    oMyRifCat.sSubalterno = CStr(dvMyDati.Item(x)("subalterno"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("data_inizio")) Then
                    oMyRifCat.tDataInizio = CDate(dvMyDati.Item(x)("data_inizio"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("data_fine")) Then
                    oMyRifCat.tDataFine = CDate(dvMyDati.Item(x)("data_fine"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("mq")) Then
                    oMyRifCat.nMQ = CDbl(dvMyDati.Item(x)("mq"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("idcategoria")) Then
                    oMyRifCat.sIdCategoria = CStr(dvMyDati.Item(x)("idcategoria"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("descrcategoria")) Then
                    oMyRifCat.sDescrCategoria = CStr(dvMyDati.Item(x)("descrcategoria"))
                End If
                ReDim Preserve oListRifCat(x)
                oListRifCat(x) = oMyRifCat
            Next

            Return oListRifCat
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsAnalisi.CheckRifCatastaliMancanti.errore: ", Err)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sIdEnte"></param>
    ''' <param name="nAnno"></param>
    ''' <param name="sOperatore"></param>
    ''' <param name="sDal"></param>
    ''' <param name="sAl"></param>
    ''' <returns></returns>
    Public Function CheckRifCatastaliAccertati(ByVal sIdEnte As String, ByVal nAnno As Integer, ByVal sOperatore As String, ByVal sDal As String, ByVal sAl As String) As oUIVSCat()
        Dim oListRifCat() As oUIVSCat = Nothing
        Dim oMyRifCat As oUIVSCat
        Dim dvMyDati As New DataView
        Dim x As Integer

        Try
            dvMyDati = GetRifCatastaliAccertati(sIdEnte, nAnno, sOperatore, sDal, sAl)
            For x = 0 To dvMyDati.Count - 1
                oMyRifCat = New oUIVSCat                  'ObjAnagArticolo
                If Not IsDBNull(dvMyDati.Item(x)("cognome")) Then
                    oMyRifCat.sCognome = CStr(dvMyDati.Item(x)("cognome"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("nome")) Then
                    oMyRifCat.sNome = CStr(dvMyDati.Item(x)("nome"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("cfpiva")) Then
                    oMyRifCat.sCodFiscalePIva = CStr(dvMyDati.Item(x)("cfpiva"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("foglio")) Then
                    oMyRifCat.sFoglio = CStr(dvMyDati.Item(x)("foglio"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("numero")) Then
                    oMyRifCat.sNumero = CStr(dvMyDati.Item(x)("numero"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("subalterno")) Then
                    oMyRifCat.sSubalterno = CStr(dvMyDati.Item(x)("subalterno"))
                End If
                ReDim Preserve oListRifCat(x)
                oListRifCat(x) = oMyRifCat
            Next

            Return oListRifCat
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsAnalisi.CheckRifCatastaliAccertati.errore: ", Err)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sIdEnte"></param>
    ''' <param name="nAnno"></param>
    ''' <param name="sOperatore"></param>
    ''' <param name="sDal"></param>
    ''' <param name="sAl"></param>
    ''' <returns></returns>
    Public Function CheckRifCatNoDich(ByVal sIdEnte As String, ByVal nAnno As Integer, ByVal sOperatore As String, ByVal sDal As String, ByVal sAl As String) As oUIVSCat()
        Dim oListRifCat() As oUIVSCat = Nothing
        Dim oMyRifCat As oUIVSCat
        Dim dvMyDati As New DataView
        Dim x As Integer

        Try
            dvMyDati = GetCheckRifCatNoDich(sIdEnte, nAnno, sOperatore, sDal, sAl)
            For x = 0 To dvMyDati.Count - 1
                oMyRifCat = New oUIVSCat
                If Not IsDBNull(dvMyDati.Item(x)("via")) Then
                    oMyRifCat.sVia = CStr(dvMyDati.Item(x)("via"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("civico")) Then
                    oMyRifCat.sCivico = CStr(dvMyDati.Item(x)("civico"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("esponente")) Then
                    oMyRifCat.sEsponente = CStr(dvMyDati.Item(x)("esponente"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("scala")) Then
                    oMyRifCat.sScala = CStr(dvMyDati.Item(x)("scala"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("interno")) Then
                    oMyRifCat.sInterno = CStr(dvMyDati.Item(x)("interno"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("foglio")) Then
                    oMyRifCat.sFoglio = CStr(dvMyDati.Item(x)("foglio"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("numero")) Then
                    oMyRifCat.sNumero = CStr(dvMyDati.Item(x)("numero"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("subalterno")) Then
                    oMyRifCat.sSubalterno = CStr(dvMyDati.Item(x)("subalterno"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("mq")) Then
                    oMyRifCat.nMQ = CDbl(dvMyDati.Item(x)("mq"))
                End If
                ReDim Preserve oListRifCat(x)
                oListRifCat(x) = oMyRifCat
            Next

            Return oListRifCat
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsAnalisi.CheckRifCatNoDich.errore: ", Err)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sIdEnte"></param>
    ''' <param name="nAnno"></param>
    ''' <param name="sOperatore"></param>
    ''' <param name="sDal"></param>
    ''' <param name="sAl"></param>
    ''' <returns></returns>
    Public Function CheckRifDichNoCat(ByVal sIdEnte As String, ByVal nAnno As Integer, ByVal sOperatore As String, ByVal sDal As String, ByVal sAl As String) As oUIVSCat()
        Dim oListRifCat() As oUIVSCat = Nothing
        Dim oMyRifCat As oUIVSCat
        Dim dvMyDati As New DataView
        Dim x As Integer

        Try
            dvMyDati = GetCheckRifDichNoCat(sIdEnte, nAnno, sOperatore, sDal, sAl)
            For x = 0 To dvMyDati.Count - 1
                oMyRifCat = New oUIVSCat
                If Not IsDBNull(dvMyDati.Item(x)("idcontribuente")) Then
                    oMyRifCat.nIdContribuente = CStr(dvMyDati.Item(x)("idcontribuente"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("idtestata")) Then
                    oMyRifCat.nIdTestata = CStr(dvMyDati.Item(x)("idtestata"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("iddettagliotestata")) Then
                    oMyRifCat.nIdDettaglioTestata = CStr(dvMyDati.Item(x)("iddettagliotestata"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("cognome")) Then
                    oMyRifCat.sCognome = CStr(dvMyDati.Item(x)("cognome"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("nome")) Then
                    oMyRifCat.sNome = CStr(dvMyDati.Item(x)("nome"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("cfpiva")) Then
                    oMyRifCat.sCodFiscalePIva = CStr(dvMyDati.Item(x)("cfpiva"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("via")) Then
                    oMyRifCat.sVia = CStr(dvMyDati.Item(x)("via"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("civico")) Then
                    oMyRifCat.sCivico = CStr(dvMyDati.Item(x)("civico"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("esponente")) Then
                    oMyRifCat.sEsponente = CStr(dvMyDati.Item(x)("esponente"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("scala")) Then
                    oMyRifCat.sScala = CStr(dvMyDati.Item(x)("scala"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("interno")) Then
                    oMyRifCat.sInterno = CStr(dvMyDati.Item(x)("interno"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("foglio")) Then
                    oMyRifCat.sFoglio = CStr(dvMyDati.Item(x)("foglio"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("numero")) Then
                    oMyRifCat.sNumero = CStr(dvMyDati.Item(x)("numero"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("subalterno")) Then
                    oMyRifCat.sSubalterno = CStr(dvMyDati.Item(x)("subalterno"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("data_inizio")) Then
                    oMyRifCat.tDataInizio = CDate(dvMyDati.Item(x)("data_inizio"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("data_fine")) Then
                    oMyRifCat.tDataFine = CDate(dvMyDati.Item(x)("data_fine"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("idcategoria")) Then
                    oMyRifCat.sIdCategoria = CStr(dvMyDati.Item(x)("idcategoria"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("descrcategoria")) Then
                    oMyRifCat.sDescrCategoria = CStr(dvMyDati.Item(x)("descrcategoria"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("mq")) Then
                    oMyRifCat.nMQ = CDbl(dvMyDati.Item(x)("mq"))
                End If
                ReDim Preserve oListRifCat(x)
                oListRifCat(x) = oMyRifCat
            Next

            Return oListRifCat
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsAnalisi.CheckRifDichNoCat.errore: ", Err)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sIdEnte"></param>
    ''' <param name="nAnno"></param>
    ''' <param name="sOperatore"></param>
    ''' <param name="sDal"></param>
    ''' <param name="sAl"></param>
    ''' <param name="nTipoRic"></param>
    ''' <returns></returns>
    Public Function CheckDichMod(ByVal sIdEnte As String, ByVal nAnno As Integer, ByVal sOperatore As String, ByVal sDal As String, ByVal sAl As String, ByVal nTipoRic As Integer) As oUIVSCat()
        Dim oListRifCat() As oUIVSCat = Nothing
        Dim oMyRifCat As oUIVSCat
        Dim dvMyDati As New DataView
        Dim x As Integer

        Try
            'uso dei campi testo dell'oggetto già presenti per memorizzare le info estratte in questa funzione
            dvMyDati = GetCheckDichMod(sIdEnte, nAnno, sOperatore, sDal, sAl, nTipoRic)
            For x = 0 To dvMyDati.Count - 1
                oMyRifCat = New oUIVSCat
                If Not IsDBNull(dvMyDati.Item(x)("cognome")) Then
                    oMyRifCat.sCognome = CStr(dvMyDati.Item(x)("cognome"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("nome")) Then
                    oMyRifCat.sNome = CStr(dvMyDati.Item(x)("nome"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("cfpiva")) Then
                    oMyRifCat.sCodFiscalePIva = CStr(dvMyDati.Item(x)("cfpiva"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("ndichiarazione")) Then
                    oMyRifCat.sDescrCategoria = CStr(dvMyDati.Item(x)("ndichiarazione"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("TDATA_VARIAZIONE")) Then
                    oMyRifCat.sAnno = CDate(dvMyDati.Item(x)("TDATA_VARIAZIONE"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("TOPERATORE")) Then
                    oMyRifCat.sVia = CStr(dvMyDati.Item(x)("TOPERATORE"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("DDATA_VARIAZIONE")) Then
                    oMyRifCat.sCivico = CDate(dvMyDati.Item(x)("DDATA_VARIAZIONE"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("DOPERATORE")) Then
                    oMyRifCat.sEsponente = CStr(dvMyDati.Item(x)("DOPERATORE"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("ODATA_VARIAZIONE")) Then
                    oMyRifCat.sScala = CDate(dvMyDati.Item(x)("ODATA_VARIAZIONE"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("OOPERATORE")) Then
                    oMyRifCat.sInterno = CStr(dvMyDati.Item(x)("OOPERATORE"))
                End If
                ReDim Preserve oListRifCat(x)
                oListRifCat(x) = oMyRifCat
            Next

            Return oListRifCat
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsAnalisi.CheckDichMod.errore: ", Err)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sIdEnte"></param>
    ''' <param name="nAnno"></param>
    ''' <param name="nTypeCheck"></param>
    ''' <param name="nTolleranzaNeg"></param>
    ''' <param name="nTolleranzaPos"></param>
    ''' <param name="nPercentMQCat"></param>
    ''' <param name="sOperatore"></param>
    ''' <param name="sDal"></param>
    ''' <param name="sAl"></param>
    ''' <returns></returns>
    Public Function CheckMQDichVSCat(ByVal sIdEnte As String, ByVal nAnno As Integer, ByVal nTypeCheck As Integer, ByVal nTolleranzaNeg As Double, ByVal nTolleranzaPos As Double, ByVal nPercentMQCat As Double, ByVal sOperatore As String, ByVal sDal As String, ByVal sAl As String) As oUIVSCat()
        Dim oListRifCat() As oUIVSCat = Nothing
        Dim oMyRifCat As oUIVSCat
        Dim dvMyDati As New DataView
        Dim x As Integer

        Try
            dvMyDati = GetCheckMQDichVSCat(sIdEnte, nAnno, nTypeCheck, nTolleranzaNeg, nTolleranzaPos, nPercentMQCat, sOperatore, sDal, sAl)
            For x = 0 To dvMyDati.Count - 1
                oMyRifCat = New oUIVSCat
                If Not IsDBNull(dvMyDati.Item(x)("idcontribuente")) Then
                    oMyRifCat.nIdContribuente = CStr(dvMyDati.Item(x)("idcontribuente"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("idtestata")) Then
                    oMyRifCat.nIdTestata = CStr(dvMyDati.Item(x)("idtestata"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("iddettagliotestata")) Then
                    oMyRifCat.nIdDettaglioTestata = CStr(dvMyDati.Item(x)("iddettagliotestata"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("cognome")) Then
                    oMyRifCat.sCognome = CStr(dvMyDati.Item(x)("cognome"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("nome")) Then
                    oMyRifCat.sNome = CStr(dvMyDati.Item(x)("nome"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("cfpiva")) Then
                    oMyRifCat.sCodFiscalePIva = CStr(dvMyDati.Item(x)("cfpiva"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("via")) Then
                    oMyRifCat.sVia = CStr(dvMyDati.Item(x)("via"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("civico")) Then
                    oMyRifCat.sCivico = CStr(dvMyDati.Item(x)("civico"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("esponente")) Then
                    oMyRifCat.sEsponente = CStr(dvMyDati.Item(x)("esponente"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("scala")) Then
                    oMyRifCat.sScala = CStr(dvMyDati.Item(x)("scala"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("interno")) Then
                    oMyRifCat.sInterno = CStr(dvMyDati.Item(x)("interno"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("foglio")) Then
                    oMyRifCat.sFoglio = CStr(dvMyDati.Item(x)("foglio"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("numero")) Then
                    oMyRifCat.sNumero = CStr(dvMyDati.Item(x)("numero"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("subalterno")) Then
                    oMyRifCat.sSubalterno = CStr(dvMyDati.Item(x)("subalterno"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("data_inizio")) Then
                    oMyRifCat.tDataInizio = CDate(dvMyDati.Item(x)("data_inizio"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("data_fine")) Then
                    oMyRifCat.tDataFine = CDate(dvMyDati.Item(x)("data_fine"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("mq")) Then
                    oMyRifCat.nMQ = CDbl(dvMyDati.Item(x)("mq"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("mqcat")) Then
                    oMyRifCat.nMQCat = CDbl(dvMyDati.Item(x)("mqcat"))
                End If
                If Not IsDBNull(dvMyDati.Item(x)("dif")) Then
                    oMyRifCat.nMQDif = CDbl(dvMyDati.Item(x)("dif"))
                End If
                ReDim Preserve oListRifCat(x)
                oListRifCat(x) = oMyRifCat
            Next

            Return oListRifCat
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsAnalisi.CheckMQDichVSCat.errore: ", Err)
            Return Nothing
        End Try
    End Function
#End Region

#Region "QUERY DB - Controllo su Riferimenti Catastali"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sMyIdEnte"></param>
    ''' <param name="nAnno"></param>
    ''' <param name="sOperatore"></param>
    ''' <param name="sDal"></param>
    ''' <param name="sAl"></param>
    ''' <returns></returns>
    Public Function GetRifCatastaliChiusi(ByVal sMyIdEnte As String, ByVal nAnno As Integer, ByVal sOperatore As String, ByVal sDal As String, ByVal sAl As String) As DataView
        Dim myDataSet As New DataSet
        Dim myAdapter As New SqlClient.SqlDataAdapter

        Try
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            'valorizzo il commandText 
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_CHECKRIFCATASTALICHIUSI"
            'valorizzo i parameters
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdEnte", SqlDbType.NVarChar)).Value = sMyIdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.Int)).Value = nAnno
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Operatore", SqlDbType.NVarChar)).Value = sOperatore
            If sDal <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Dal", SqlDbType.NVarChar)).Value = oReplace.FormattaData(sDal, "A")
            End If
            If sAl <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Al", SqlDbType.NVarChar)).Value = oReplace.FormattaData(sAl, "A")
            End If
            'eseguo la query
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(myDataSet, "Create DataView")
            myAdapter.Dispose()
            Return myDataSet.Tables(0).DefaultView()
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsAnalisi.GetRifCatatastaliChiusi.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sMyIdEnte"></param>
    ''' <param name="nAnno"></param>
    ''' <param name="sOperatore"></param>
    ''' <param name="sDal"></param>
    ''' <param name="sAl"></param>
    ''' <returns></returns>
    Public Function GetRifCatastali(ByVal sMyIdEnte As String, ByVal nAnno As Integer, ByVal sOperatore As String, ByVal sDal As String, ByVal sAl As String) As DataView
        Dim myDataSet As New DataSet
        Dim myAdapter As New SqlClient.SqlDataAdapter

        Try
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            'valorizzo il commandText 
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_CHECKRIFCATASTALI"
            'valorizzo i parameters
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdEnte", SqlDbType.NVarChar)).Value = sMyIdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.Int)).Value = nAnno
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Operatore", SqlDbType.NVarChar)).Value = sOperatore
            If sDal <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Dal", SqlDbType.NVarChar)).Value = oReplace.FormattaData(sDal, "A")
            End If
            If sAl <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Al", SqlDbType.NVarChar)).Value = oReplace.FormattaData(sAl, "A")
            End If
            'eseguo la query
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(myDataSet, "Create DataView")
            myAdapter.Dispose()
            Return myDataSet.Tables(0).DefaultView()
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsAnalisi.GetRifCatastali.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sMyIdEnte"></param>
    ''' <param name="nAnno"></param>
    ''' <param name="sOperatore"></param>
    ''' <param name="sDal"></param>
    ''' <param name="sAl"></param>
    ''' <returns></returns>
    Public Function GetRifCatastaliMancanti(ByVal sMyIdEnte As String, ByVal nAnno As Integer, ByVal sOperatore As String, ByVal sDal As String, ByVal sAl As String) As DataView
        Dim myDataSet As New DataSet
        Dim myAdapter As New SqlClient.SqlDataAdapter

        Try
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            'valorizzo il commandText 
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_CHECKRIFCATASTALIMANCANTI"
            'valorizzo i parameters
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdEnte", SqlDbType.NVarChar)).Value = sMyIdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.Int)).Value = nAnno
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Operatore", SqlDbType.NVarChar)).Value = sOperatore
            If sDal <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Dal", SqlDbType.NVarChar)).Value = oReplace.FormattaData(sDal, "A")
            End If
            If sAl <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Al", SqlDbType.NVarChar)).Value = oReplace.FormattaData(sAl, "A")
            End If
            'eseguo la query
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(myDataSet, "Create DataView")
            myAdapter.Dispose()
            Return myDataSet.Tables(0).DefaultView()
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsAnalisi.GetRifCatastaliMancanti.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sMyIdEnte"></param>
    ''' <param name="nAnno"></param>
    ''' <param name="sOperatore"></param>
    ''' <param name="sDal"></param>
    ''' <param name="sAl"></param>
    ''' <returns></returns>
    Public Function GetRifCatastaliAccertati(ByVal sMyIdEnte As String, ByVal nAnno As Integer, ByVal sOperatore As String, ByVal sDal As String, ByVal sAl As String) As DataView
        Dim myDataSet As New DataSet
        Dim myAdapter As New SqlClient.SqlDataAdapter

        Try
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            'valorizzo il commandText 
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_CHECKRIFCATASTALIACCERTATI"
            'valorizzo i parameters
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdEnte", SqlDbType.NVarChar)).Value = sMyIdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.Int)).Value = nAnno
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Operatore", SqlDbType.NVarChar)).Value = sOperatore
            If sDal <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Dal", SqlDbType.NVarChar)).Value = oReplace.FormattaData(sDal, "A")
            End If
            If sAl <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Al", SqlDbType.NVarChar)).Value = oReplace.FormattaData(sAl, "A")
            End If
            'eseguo la query
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(myDataSet, "Create DataView")
            myAdapter.Dispose()
            Return myDataSet.Tables(0).DefaultView()
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsAnalisi.GetRifCatastaliAccertati.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sIdEnte"></param>
    ''' <param name="nAnno"></param>
    ''' <param name="sOperatore"></param>
    ''' <param name="sDal"></param>
    ''' <param name="sAl"></param>
    ''' <returns></returns>
    Public Function GetCheckRifCatNoDich(ByVal sIdEnte As String, ByVal nAnno As Integer, ByVal sOperatore As String, ByVal sDal As String, ByVal sAl As String) As DataView
        Dim myDataSet As New DataSet
        Dim myAdapter As New SqlClient.SqlDataAdapter

        Try
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            'valorizzo il commandText 
            cmdMyCommand.CommandText = "prc_MQCATnoMQDICH"
            'valorizzo i parameters
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdEnte", SqlDbType.NVarChar)).Value = sIdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.Int)).Value = nAnno
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Operatore", SqlDbType.NVarChar)).Value = sOperatore
            If sDal <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Dal", SqlDbType.NVarChar)).Value = oReplace.FormattaData(sDal, "A")
            End If
            If sAl <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Al", SqlDbType.NVarChar)).Value = oReplace.FormattaData(sAl, "A")
            End If
            'eseguo la query
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(myDataSet, "Create DataView")
            myAdapter.Dispose()
            Return myDataSet.Tables(0).DefaultView()
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsAnalisi.GetCheckRifCatNoDich.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sIdEnte"></param>
    ''' <param name="nAnno"></param>
    ''' <param name="sOperatore"></param>
    ''' <param name="sDal"></param>
    ''' <param name="sAl"></param>
    ''' <returns></returns>
    Public Function GetCheckRifDichNoCat(ByVal sIdEnte As String, ByVal nAnno As Integer, ByVal sOperatore As String, ByVal sDal As String, ByVal sAl As String) As DataView
        Dim myDataSet As New DataSet
        Dim myAdapter As New SqlClient.SqlDataAdapter

        Try
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            'valorizzo il commandText 
            cmdMyCommand.CommandText = "prc_MQDICHnoMQCAT"
            'valorizzo i parameters
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdEnte", SqlDbType.NVarChar)).Value = sIdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.Int)).Value = nAnno
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Operatore", SqlDbType.NVarChar)).Value = sOperatore
            If sDal <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Dal", SqlDbType.NVarChar)).Value = oReplace.FormattaData(sDal, "A")
            End If
            If sAl <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Al", SqlDbType.NVarChar)).Value = oReplace.FormattaData(sAl, "A")
            End If
            'eseguo la query
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(myDataSet, "Create DataView")
            myAdapter.Dispose()
            Return myDataSet.Tables(0).DefaultView()
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsAnalisi.GetCheckRifDichNoCat.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sIdEnte"></param>
    ''' <param name="nAnno"></param>
    ''' <param name="nTypeCheck"></param>
    ''' <param name="nTolleranzaNeg"></param>
    ''' <param name="nTolleranzaPos"></param>
    ''' <param name="nPercentMQCat"></param>
    ''' <param name="sOperatore"></param>
    ''' <param name="sDal"></param>
    ''' <param name="sAl"></param>
    ''' <returns></returns>
    Public Function GetCheckMQDichVSCat(ByVal sIdEnte As String, ByVal nAnno As Integer, ByVal nTypeCheck As Integer, ByVal nTolleranzaNeg As Double, ByVal nTolleranzaPos As Double, ByVal nPercentMQCat As Double, ByVal sOperatore As String, ByVal sDal As String, ByVal sAl As String) As DataView
        Dim myDataSet As New DataSet
        Dim myAdapter As New SqlClient.SqlDataAdapter

        Try
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            'valorizzo il commandText 
            cmdMyCommand.CommandText = "prc_MQDICHvsMQCAT"
            'valorizzo i parameters
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdEnte", SqlDbType.NVarChar)).Value = sIdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Anno", SqlDbType.Int)).Value = nAnno
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PercentMQCat", SqlDbType.Float)).Value = nPercentMQCat
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PercentTolleranzaNeg", SqlDbType.Float)).Value = nTolleranzaNeg
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@PercentTolleranzaPos", SqlDbType.Float)).Value = nTolleranzaPos
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TypeEstraz", SqlDbType.Int)).Value = nTypeCheck
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Operatore", SqlDbType.NVarChar)).Value = sOperatore
            If sDal <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Dal", SqlDbType.NVarChar)).Value = oReplace.FormattaData(sDal, "A")
            End If
            If sAl <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Al", SqlDbType.NVarChar)).Value = oReplace.FormattaData(sAl, "A")
            End If
            'eseguo la query
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(myDataSet, "Create DataView")
            myAdapter.Dispose()
            Return myDataSet.Tables(0).DefaultView()
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsAnalisi.GetCheckMQDichVSCat.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sMyIdEnte"></param>
    ''' <param name="nAnno"></param>
    ''' <param name="sOperatore"></param>
    ''' <param name="sDal"></param>
    ''' <param name="sAl"></param>
    ''' <param name="nTipoRic"></param>
    ''' <returns></returns>
    Public Function GetCheckDichMod(ByVal sMyIdEnte As String, ByVal nAnno As Integer, ByVal sOperatore As String, ByVal sDal As String, ByVal sAl As String, ByVal nTipoRic As Integer) As DataView
        'Dim myDataSet As New DataSet
        'Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dvMyDati As New DataView

        Try
            ''Valorizzo la connessione
            'cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            ''valorizzo il commandText 
            'cmdMyCommand.CommandType = CommandType.StoredProcedure
            'cmdMyCommand.CommandText = "prc_GetDichiarazioniModificate"
            ''valorizzo i parameters
            'cmdMyCommand.Parameters.Clear()
            'cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdEnte", SqlDbType.NVarChar)).Value = sMyIdEnte
            'cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Anno", SqlDbType.Int)).Value = nAnno
            'cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Operatore", SqlDbType.NVarChar)).Value = sOperatore
            'If sDal <> "" Then
            '    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Dal", SqlDbType.NVarChar)).Value = oReplace.FormattaData(sDal, "A")
            'End If
            'If sAl <> "" Then
            '    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@Al", SqlDbType.NVarChar)).Value = oReplace.FormattaData(sAl, "A")
            'End If
            'cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TipoRicerca", SqlDbType.Int)).Value = nTipoRic
            ''eseguo la query
            'myAdapter.SelectCommand = cmdMyCommand
            'Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            'myAdapter.Fill(myDataSet, "Create DataView")
            'myAdapter.Dispose()
            'Return myDataSet.Tables(0).DefaultView()
            Dim oDbManagerRepository As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
            Dim sSQL As String = ""
            Using ctx As DBModel = oDbManagerRepository
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetDichiarazioniModificate", "IdEnte", "ANNO", "Operatore", "Dal", "Al", "TipoRicerca")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IdEnte", sMyIdEnte) _
                        , ctx.GetParam("ANNO", nAnno) _
                        , ctx.GetParam("Operatore", sOperatore) _
                        , ctx.GetParam("DAL", oReplace.FormattaData(sDal, "A")) _
                        , ctx.GetParam("Al", oReplace.FormattaData(sAl, "A")) _
                        , ctx.GetParam("TipoRicerca", nTipoRic)
                    )
                ctx.Dispose()
            End Using
            Return dvMyDati
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsAnalisi.GetCheckDichMod.errore: ", Err)
            'Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sIdEnte"></param>
    ''' <param name="oMyDettaglio"></param>
    ''' <param name="nMyId"></param>
    ''' <returns></returns>
    Public Function CheckUniqueRifCatastali(ByVal sIdEnte As String, ByVal oMyDettaglio As ObjDettaglioTestata, ByVal nMyId As Integer) As Boolean
        'Dim sSQL As String
        'Dim DrDati As SqlClient.SqlDataReader = Nothing
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView

        Try
            'prelevo i dati di dettaglio testata
            Dim oDbManagerRepository As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
            sSQL = "SELECT *"
            sSQL += " FROM V_CHECKRIFCATASTALI"
            sSQL += " WHERE (IDENTE='" & sIdEnte & "')"
            sSQL += " AND (FOGLIO='" & oMyDettaglio.sFoglio & "')"
            sSQL += " AND (NUMERO='" & oMyDettaglio.sNumero & "')"
            sSQL += " AND (SUBALTERNO='" & oMyDettaglio.sSubalterno & "')"
            sSQL += " AND (DATA_FINE>='" & oReplace.ReplaceDataForDB(oMyDettaglio.tDataInizio) & "')"
            If oMyDettaglio.tDataFine <> Date.MinValue Then
                sSQL += " AND (DATA_INIZIO<='" & oReplace.ReplaceDataForDB(oMyDettaglio.tDataFine) & "')"
            End If
            If nMyId > 0 Then
                sSQL += " AND (ID<>" & nMyId & ")"
            End If
            Using ctx As DBModel = oDbManagerRepository
                sSQL = ctx.GetSQL(DBModel.TypeQuery.View, sSQL)
                dvMyDati = ctx.GetDataView(sSQL, "TBL")
                ctx.Dispose()
            End Using
            If dvMyDati.Count > 0 Then
                Return False
            End If
            'Dim cmdMyCommand As New SqlClient.SqlCommand
            'cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            'cmdMyCommand.Connection.Open()
            'cmdMyCommand.CommandTimeout = 0
            'cmdMyCommand.CommandType = CommandType.Text
            'cmdMyCommand.CommandText = sSQL
            ''eseguo la query
            'Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            'DrDati = cmdMyCommand.ExecuteReader
            'If DrDati.Read Then
            '    Return False
            'End If

            Return True
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsAnalisi.CheckUniqueRifCatastali.errore: ", Err)
            'Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return False
        Finally
            'DrDati.Close()
            dvMyDati.Dispose()
        End Try
    End Function
#End Region
#Region "Fatturato/Incassato"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sMyIdEnte"></param>
    ''' <param name="sAnno"></param>
    ''' <param name="sTipoRuolo"></param>
    ''' <param name="sAccreditoDal"></param>
    ''' <param name="sAccreditoAl"></param>
    ''' <param name="IsEvaseTotalmente"></param>
    ''' <returns></returns>
    Public Function GetRiepilogoEmessoEvaso(ByVal sMyIdEnte As String, ByVal sAnno As String, ByVal sTipoRuolo As String, ByVal sAccreditoDal As String, ByVal sAccreditoAl As String, ByVal IsEvaseTotalmente As Integer) As DataView
        Dim myDataSet As New DataSet
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Try
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            'valorizzo il commandText 
            cmdMyCommand.CommandText = "SELECT COUNT(TMPTBL.CODICE_CARTELLA) AS NPAG,SUM(TMPTBL.IMPPAG) AS IMPPAG"
            cmdMyCommand.CommandText += " FROM (SELECT TMPPAG.CODICE_CARTELLA, SUM(TMPPAG.IMPPAGATO) AS IMPPAG"
            cmdMyCommand.CommandText += " FROM TBLCARTELLE"
            cmdMyCommand.CommandText += " INNER JOIN (SELECT TBLCARTELLE.CODICE_CARTELLA, SUM(TBLPAGAMENTI.IMPORTO_PAGAMENTO*CAST(TBLPAGAMENTI.SEGNO+'1' AS NUMERIC)) AS IMPPAGATO"
            cmdMyCommand.CommandText += " FROM TBLCARTELLE"
            cmdMyCommand.CommandText += " INNER JOIN TBLPAGAMENTI ON TBLCARTELLE.CODICE_CARTELLA=TBLPAGAMENTI.CODICE_CARTELLA"
            cmdMyCommand.CommandText += " INNER JOIN TBLRUOLI_GENERATI ON TBLCARTELLE.IDFLUSSO_RUOLO=TBLRUOLI_GENERATI.IDFLUSSO"
            cmdMyCommand.CommandText += " WHERE (TBLRUOLI_GENERATI.IDENTE=@CODISTAT)"
            If sAnno <> "" Then
                cmdMyCommand.CommandText += " AND (TBLRUOLI_GENERATI.ANNO=@ANNORUOLO)"
            End If
            If sTipoRuolo <> "" Then
                cmdMyCommand.CommandText += " AND (TBLRUOLI_GENERATI.TIPO_RUOLO=@TIPORUOLO)"
            End If
            If sAccreditoDal <> "" Then
                cmdMyCommand.CommandText += " AND (TBLPAGAMENTI.DATA_ACCREDITO>=@VALUTADAL)"
            End If
            If sAccreditoAl <> "" Then
                cmdMyCommand.CommandText += " AND (TBLPAGAMENTI.DATA_ACCREDITO<=@VALUTAAL)"
            End If
            cmdMyCommand.CommandText += " GROUP BY TBLCARTELLE.CODICE_CARTELLA) TMPPAG ON TBLCARTELLE.CODICE_CARTELLA=TMPPAG.CODICE_CARTELLA"
            cmdMyCommand.CommandText += " GROUP BY TBLCARTELLE.IMPORTO_CARICO, TMPPAG.CODICE_CARTELLA"
            If IsEvaseTotalmente = 1 Then
                cmdMyCommand.CommandText += " HAVING (SUM(TMPPAG.IMPPAGATO)>=TBLCARTELLE.IMPORTO_CARICO)"
            Else
                cmdMyCommand.CommandText += " HAVING (SUM(TMPPAG.IMPPAGATO)<TBLCARTELLE.IMPORTO_CARICO)"
            End If
            cmdMyCommand.CommandText += " ) TMPTBL"

            'valorizzo i parameters
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODISTAT", SqlDbType.NVarChar)).Value = sMyIdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNORUOLO", SqlDbType.NVarChar)).Value = sAnno
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPORUOLO", SqlDbType.NVarChar)).Value = sTipoRuolo
            If sAccreditoDal <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTADAL", SqlDbType.DateTime)).Value = sAccreditoDal
            End If
            If sAccreditoAl <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTAAL", SqlDbType.DateTime)).Value = sAccreditoAl
            End If
            'eseguo la query
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(myDataSet, "Create DataView")
            myAdapter.Dispose()
            Return myDataSet.Tables(0).DefaultView()
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsAnalisi.GetRiepilogoEmessoEvaso.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        End Try
    End Function
    '*** 20131104 - TARES ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="IsFromVariabile"></param>
    ''' <param name="sMyIdEnte"></param>
    ''' <param name="sAnno"></param>
    ''' <param name="sTipoRuolo"></param>
    ''' <param name="sAccreditoDal"></param>
    ''' <param name="sAccreditoAl"></param>
    ''' <returns></returns>
    Public Function GetRiepilogoEmesso(ByVal IsFromVariabile As String, ByVal sMyIdEnte As String, ByVal sAnno As String, ByVal sTipoRuolo As String, ByVal sAccreditoDal As String, ByVal sAccreditoAl As String) As DataView
        Dim myDataSet As New DataSet
        Dim myAdapter As New SqlClient.SqlDataAdapter

        Try
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandTimeout = 0
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            'valorizzo il commandText 
            cmdMyCommand.CommandText = "prc_FatInc_RiepilogoEmesso"
            'valorizzo i parameters
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ISFROMVARIABILE", SqlDbType.NVarChar)).Value = IsFromVariabile
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODISTAT", SqlDbType.NVarChar)).Value = sMyIdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNORUOLO", SqlDbType.NVarChar)).Value = sAnno
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPORUOLO", SqlDbType.NVarChar)).Value = sTipoRuolo
            If sAccreditoDal <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTADAL", SqlDbType.DateTime)).Value = sAccreditoDal
            Else
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTADAL", SqlDbType.DateTime)).Value = DateTime.MaxValue
            End If
            If sAccreditoAl <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTAAL", SqlDbType.DateTime)).Value = sAccreditoAl
            Else
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTAAL", SqlDbType.DateTime)).Value = DateTime.MaxValue
            End If
            'eseguo la query
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(myDataSet, "Create DataView")
            myAdapter.Dispose()
            Return myDataSet.Tables(0).DefaultView()
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsAnalisi.GetRiepilogoEmesso.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        Finally
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="IsFromVariabile"></param>
    ''' <param name="sMyIdEnte"></param>
    ''' <param name="sTipoRuolo"></param>
    ''' <param name="sAnno"></param>
    ''' <param name="sAccreditoDal"></param>
    ''' <param name="sAccreditoAl"></param>
    ''' <returns></returns>
    Public Function GetDettaglioEmesso(ByVal IsFromVariabile As String, ByVal sMyIdEnte As String, ByVal sTipoRuolo As String, ByVal sAnno As String, ByVal sAccreditoDal As String, ByVal sAccreditoAl As String) As DataView
        Dim myDataSet As New DataSet
        Dim myAdapter As New SqlClient.SqlDataAdapter

        Try
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandTimeout = 0
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            'valorizzo il commandText 
            cmdMyCommand.CommandText = "prc_FatInc_DettaglioEmesso"
            'valorizzo i parameters
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ISFROMVARIABILE", SqlDbType.NVarChar)).Value = IsFromVariabile
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODISTAT", SqlDbType.NVarChar)).Value = sMyIdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNORUOLO", SqlDbType.NVarChar)).Value = sAnno
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPORUOLO", SqlDbType.NVarChar)).Value = sTipoRuolo
            If sAccreditoDal <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTADAL", SqlDbType.DateTime)).Value = sAccreditoDal
            Else
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTADAL", SqlDbType.DateTime)).Value = DateTime.MaxValue
            End If
            If sAccreditoAl <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTAAL", SqlDbType.DateTime)).Value = sAccreditoAl
            Else
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTAAL", SqlDbType.DateTime)).Value = DateTime.MaxValue
            End If
            'eseguo la query
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(myDataSet, "Create DataView")
            myAdapter.Dispose()
            Return myDataSet.Tables(0).DefaultView()
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsAnalisi.GetDettaglioEmesso.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        Finally
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="IsFromVariabile"></param>
    ''' <param name="sMyIdEnte"></param>
    ''' <param name="sTipoRuolo"></param>
    ''' <param name="sAnno"></param>
    ''' <param name="sAccreditoDal"></param>
    ''' <param name="sAccreditoAl"></param>
    ''' <returns></returns>
    Public Function GetDettaglioIncassato(ByVal IsFromVariabile As String, ByVal sMyIdEnte As String, ByVal sTipoRuolo As String, ByVal sAnno As String, ByVal sAccreditoDal As String, ByVal sAccreditoAl As String) As DataView
        Dim myDataSet As New DataSet
        Dim myAdapter As New SqlClient.SqlDataAdapter

        Try
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandTimeout = 0
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            'valorizzo il commandText 
            cmdMyCommand.CommandText = "prc_FatInc_DettaglioIncassato"
            'valorizzo i parameters
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ISFROMVARIABILE", SqlDbType.NVarChar)).Value = IsFromVariabile
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODISTAT", SqlDbType.NVarChar)).Value = sMyIdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNORUOLO", SqlDbType.NVarChar)).Value = sAnno
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPORUOLO", SqlDbType.NVarChar)).Value = sTipoRuolo
            If sAccreditoDal <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTADAL", SqlDbType.DateTime)).Value = sAccreditoDal
            Else
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTADAL", SqlDbType.DateTime)).Value = DateTime.MaxValue
            End If
            If sAccreditoAl <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTAAL", SqlDbType.DateTime)).Value = sAccreditoAl
            Else
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTAAL", SqlDbType.DateTime)).Value = DateTime.MaxValue
            End If
            'eseguo la query
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(myDataSet, "Create DataView")
            myAdapter.Dispose()
            Return myDataSet.Tables(0).DefaultView()
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsAnalisi.GetDettaglioIncassato.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        Finally
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="IsFromVariabile"></param>
    ''' <param name="sMyIdEnte"></param>
    ''' <param name="sTipoRuolo"></param>
    ''' <param name="sAnno"></param>
    ''' <param name="sAccreditoDal"></param>
    ''' <param name="sAccreditoAl"></param>
    ''' <returns></returns>
    Public Function GetEmessoImpostaVSCategoria(ByVal IsFromVariabile As String, ByVal sMyIdEnte As String, ByVal sTipoRuolo As String, ByVal sAnno As String, ByVal sAccreditoDal As String, ByVal sAccreditoAl As String) As DataView
        Dim myDataSet As New DataSet
        Dim myAdapter As New SqlClient.SqlDataAdapter

        Try
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandTimeout = 0
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            'valorizzo il commandText 
            cmdMyCommand.CommandText = "prc_FatInc_EmessoImpostaVSCategoria"
            'valorizzo i parameters
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ISFROMVARIABILE", SqlDbType.NVarChar)).Value = IsFromVariabile
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODISTAT", SqlDbType.NVarChar)).Value = sMyIdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNORUOLO", SqlDbType.NVarChar)).Value = sAnno
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPORUOLO", SqlDbType.NVarChar)).Value = sTipoRuolo
            If sAccreditoDal <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTADAL", SqlDbType.DateTime)).Value = sAccreditoDal
            Else
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTADAL", SqlDbType.DateTime)).Value = DateTime.MaxValue
            End If
            If sAccreditoAl <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTAAL", SqlDbType.DateTime)).Value = sAccreditoAl
            Else
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTAAL", SqlDbType.DateTime)).Value = DateTime.MaxValue
            End If
            'eseguo la query
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(myDataSet, "Create DataView")
            myAdapter.Dispose()
            Return myDataSet.Tables(0).DefaultView()
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsAnalisi.GetEmessoImpostaVSCategoria.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        Finally
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="IsFromVariabile"></param>
    ''' <param name="sMyIdEnte"></param>
    ''' <param name="sTipoRuolo"></param>
    ''' <param name="sAnno"></param>
    ''' <param name="sAccreditoDal"></param>
    ''' <param name="sAccreditoAl"></param>
    ''' <returns></returns>
    Public Function GetIncassatoImpostaVSCategoria(ByVal IsFromVariabile As String, ByVal sMyIdEnte As String, ByVal sTipoRuolo As String, ByVal sAnno As String, ByVal sAccreditoDal As String, ByVal sAccreditoAl As String) As DataView
        Dim myDataSet As New DataSet
        Dim myAdapter As New SqlClient.SqlDataAdapter

        Try
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandTimeout = 0
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            'valorizzo il commandText 
            cmdMyCommand.CommandText = "prc_FatInc_IncassatoImpostaVSCategoria"
            'valorizzo i parameters
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ISFROMVARIABILE", SqlDbType.NVarChar)).Value = IsFromVariabile
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODISTAT", SqlDbType.NVarChar)).Value = sMyIdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNORUOLO", SqlDbType.NVarChar)).Value = sAnno
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPORUOLO", SqlDbType.NVarChar)).Value = sTipoRuolo
            If sAccreditoDal <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTADAL", SqlDbType.DateTime)).Value = sAccreditoDal
            Else
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTADAL", SqlDbType.DateTime)).Value = DateTime.MaxValue
            End If
            If sAccreditoAl <> "" Then
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTAAL", SqlDbType.DateTime)).Value = sAccreditoAl
            Else
                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@VALUTAAL", SqlDbType.DateTime)).Value = DateTime.MaxValue
            End If
            'eseguo la query
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(myDataSet, "Create DataView")
            myAdapter.Dispose()
            Return myDataSet.Tables(0).DefaultView()
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsAnalisi.GetIncassatoImpostaVSCategoria.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Return Nothing
        Finally
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Function
    '*** ***
#End Region
End Class
''' <summary>
''' Classe per l'estrazione dei flussi per l'agenzia delle entrate
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class ClsAETracciati
    Private Shared Log As ILog = LogManager.GetLogger("ClsAETracciati")
    Private FncReplace As New generalClass.generalFunction
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sMyEnte"></param>
    ''' <param name="sMyAnno"></param>
    ''' <param name="sMyDataDal"></param>
    ''' <returns></returns>
    Public Function GetDisposizioni(ByVal sMyEnte As String, ByVal sMyAnno As String, ByVal sMyDataDal As String) As FncWSOpenAE.DisposizioneAE()
        Try
            Dim DvDati As New DataView
            Dim x As Integer
            Dim oMyDisposizione As FncWSOpenAE.DisposizioneAE
            Dim oListDisposizioni() As FncWSOpenAE.DisposizioneAE = Nothing
            Dim oDispDatiEnte As New FncWSOpenAE.DisposizioneAE

            'prelevo i dati dell'ente
            oDispDatiEnte = GetEnte(sMyEnte)
            If oDispDatiEnte Is Nothing Then
                Return Nothing
            End If

            DvDati = GetDatiEstrazione(sMyEnte, sMyAnno, sMyDataDal)
            For x = 0 To DvDati.Count - 1
                'inizializzo il nuovo oggetto
                oMyDisposizione = New FncWSOpenAE.DisposizioneAE
                'popolo i dati del comune
                oMyDisposizione.sCodFiscaleEnte = oDispDatiEnte.sCodFiscaleEnte
                oMyDisposizione.sCognomeEnte = oDispDatiEnte.sCognomeEnte
                oMyDisposizione.sNomeEnte = oDispDatiEnte.sNomeEnte
                oMyDisposizione.sSessoEnte = oDispDatiEnte.sSessoEnte
                oMyDisposizione.sDataNascitaEnte = oDispDatiEnte.sDataNascitaEnte
                oMyDisposizione.sComuneNascitaSedeEnte = oDispDatiEnte.sComuneNascitaSedeEnte
                oMyDisposizione.sPVNascitaSedeEnte = oDispDatiEnte.sPVNascitaSedeEnte
                oMyDisposizione.sCodComuneUbicazioneCatast = oDispDatiEnte.sCodComuneUbicazioneCatast
                oMyDisposizione.sComuneAmmUbicazione = oDispDatiEnte.sComuneAmmUbicazione
                oMyDisposizione.sComuneCatastUbicazione = oDispDatiEnte.sComuneCatastUbicazione
                oMyDisposizione.sPVAmmUbicazione = oDispDatiEnte.sPVAmmUbicazione
                'popolo i dati della posizione
                oMyDisposizione.sCodISTAT = sMyEnte
                oMyDisposizione.sTributo = Utility.Costanti.TRIBUTO_TARSU
                oMyDisposizione.nIDCollegamento = CInt(DvDati.Item(x)("id"))
                oMyDisposizione.nIDContribuente = CInt(DvDati.Item(x)("idcontribuente"))
                oMyDisposizione.sAnno = sMyAnno
                oMyDisposizione.sCodFiscale = CStr(DvDati.Item(x)("cf_piva"))
                oMyDisposizione.sCognome = CStr(DvDati.Item(x)("cognome"))
                If Not IsDBNull(DvDati.Item(x)("nome")) Then oMyDisposizione.sNome = CStr(DvDati.Item(x)("nome"))
                oMyDisposizione.sSesso = CStr(DvDati.Item(x)("sesso"))
                If Not IsDBNull(DvDati.Item(x)("data_nascita")) Then oMyDisposizione.sDataNascita = CStr(DvDati.Item(x)("data_nascita"))
                If Not IsDBNull(DvDati.Item(x)("comune_nascitasede")) Then oMyDisposizione.sComuneNascitaSede = CStr(DvDati.Item(x)("comune_nascitasede"))
                If Not IsDBNull(DvDati.Item(x)("pv_nascitasede")) Then oMyDisposizione.sPVNascitaSede = CStr(DvDati.Item(x)("pv_nascitasede"))
                If Not IsDBNull(DvDati.Item(x)("indirizzo")) Then oMyDisposizione.sIndirizzo = CStr(DvDati.Item(x)("indirizzo"))
                If Not IsDBNull(DvDati.Item(x)("civico")) Then oMyDisposizione.sCivico = CStr(DvDati.Item(x)("civico"))
                If Not IsDBNull(DvDati.Item(x)("interno")) Then oMyDisposizione.sInterno = CStr(DvDati.Item(x)("interno"))
                If Not IsDBNull(DvDati.Item(x)("scala")) Then oMyDisposizione.sScala = CStr(DvDati.Item(x)("scala"))
                If Not IsDBNull(DvDati.Item(x)("sezione")) Then oMyDisposizione.sSezione = CStr(DvDati.Item(x)("sezione"))
                If Not IsDBNull(DvDati.Item(x)("foglio")) Then oMyDisposizione.sFoglio = CStr(DvDati.Item(x)("foglio"))
                If Not IsDBNull(DvDati.Item(x)("particella")) Then oMyDisposizione.sParticella = CStr(DvDati.Item(x)("particella"))
                If Not IsDBNull(DvDati.Item(x)("subalterno")) Then oMyDisposizione.sSubalterno = CStr(DvDati.Item(x)("subalterno"))
                If Not IsDBNull(DvDati.Item(x)("estensione_particella")) Then oMyDisposizione.sEstensioneParticella = CStr(DvDati.Item(x)("estensione_particella"))
                If Not IsDBNull(DvDati.Item(x)("id_tipo_particella")) Then oMyDisposizione.sIDTipoParticella = CStr(DvDati.Item(x)("id_tipo_particella"))
                If Not IsDBNull(DvDati.Item(x)("comune_res")) Then oMyDisposizione.sComuneDomFisc = CStr(DvDati.Item(x)("comune_res"))
                If Not IsDBNull(DvDati.Item(x)("pv_res")) Then oMyDisposizione.sPVDomFisc = CStr(DvDati.Item(x)("pv_res"))
                If Not IsDBNull(DvDati.Item(x)("data_inizio")) Then oMyDisposizione.sDataInizio = FncReplace.FormattaData(CStr(DvDati.Item(x)("data_inizio")), "A")
                If Not IsDBNull(DvDati.Item(x)("data_fine")) Then oMyDisposizione.sDataFine = FncReplace.FormattaData(CStr(DvDati.Item(x)("data_fine")), "A")
                If Not IsDBNull(DvDati.Item(x)("id_assenza_dati_catastali")) Then
                    oMyDisposizione.nIDAssenzaDatiCatastali = CInt(DvDati.Item(x)("id_assenza_dati_catastali"))
                    If oMyDisposizione.nIDAssenzaDatiCatastali = -1 Then Stop
                Else
                    oMyDisposizione.nIDAssenzaDatiCatastali = 0
                End If
                If Not IsDBNull(DvDati.Item(x)("id_destinazione_uso")) Then oMyDisposizione.nIDDestinazioneUso = CInt(DvDati.Item(x)("id_destinazione_uso"))
                If Not IsDBNull(DvDati.Item(x)("id_titolo_occupazione")) Then oMyDisposizione.nIDTitoloOccupazione = CInt(DvDati.Item(x)("id_titolo_occupazione"))
                If Not IsDBNull(DvDati.Item(x)("id_natura_occupante")) Then oMyDisposizione.nIDTipoOccupante = CInt(DvDati.Item(x)("id_natura_occupante"))
                If Not IsDBNull(DvDati.Item(x)("id_tipo_unita")) Then oMyDisposizione.sIDTipoUnita = CStr(DvDati.Item(x)("id_tipo_unita"))
                Log.Debug("AETracciati.Dati->cf_piva=" & oMyDisposizione.sCodFiscale & ",foglio=" & oMyDisposizione.sFoglio & ",numero=" & oMyDisposizione.sParticella & ",subalterno=" & oMyDisposizione.sSubalterno & ",id_destinazione_uso=" & oMyDisposizione.nIDDestinazioneUso & ",id_titolo_occupazione=" & oMyDisposizione.nIDTitoloOccupazione & ",id_natura_occupante=" & oMyDisposizione.nIDTipoOccupante & ",id_assenza_dati_catastali=" & oMyDisposizione.nIDAssenzaDatiCatastali)
                'inserisco il singolo oggetto all’array di oggetti
                ReDim Preserve oListDisposizioni(x)
                oListDisposizioni(x) = oMyDisposizione
            Next
            Return oListDisposizioni
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsAETracciati.GetDisposizioni.errore: ", Err)
            Throw Err
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sMyEnte"></param>
    ''' <param name="sMyAnno"></param>
    ''' <param name="sMyDataDal"></param>
    ''' <returns></returns>
    Public Function GetDisposizioniDatiMancanti(ByVal sMyEnte As String, ByVal sMyAnno As String, ByVal sMyDataDal As String) As DataView
        Try
            Dim DvDati As New DataView

            DvDati = GetDatiMancanti(sMyEnte, sMyAnno, sMyDataDal)
            Return DvDati
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsAETtracciati.GetDisposizioniDatiMancanti.errore: ", Err)
            Throw Err
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sMyEnte"></param>
    ''' <returns></returns>
    Private Function GetEnte(ByVal sMyEnte As String) As FncWSOpenAE.DisposizioneAE
        Try
            Dim oMyDisposizione As New FncWSOpenAE.DisposizioneAE
            Dim DvDati As New DataView
            Dim x As Integer

            DvDati = GetDatiEnte(sMyEnte)
            For x = 0 To DvDati.Count - 1
                oMyDisposizione.sCodISTAT = sMyEnte
                oMyDisposizione.sCodFiscaleEnte = CStr(DvDati.Item(x)("cf_piva"))
                oMyDisposizione.sCognomeEnte = CStr(DvDati.Item(x)("cognome"))
                oMyDisposizione.sNomeEnte = CStr(DvDati.Item(x)("nome"))
                oMyDisposizione.sSessoEnte = CStr(DvDati.Item(x)("sesso"))
                oMyDisposizione.sDataNascitaEnte = CStr(DvDati.Item(x)("data_nascita"))
                oMyDisposizione.sComuneNascitaSedeEnte = CStr(DvDati.Item(x)("comune_nascitasede"))
                oMyDisposizione.sPVNascitaSedeEnte = CStr(DvDati.Item(x)("pv_nascitasede"))
                oMyDisposizione.sCodComuneUbicazioneCatast = CStr(DvDati.Item(x)("cod_ubicazcat"))
                oMyDisposizione.sComuneAmmUbicazione = CStr(DvDati.Item(x)("descrizione_ente"))
                oMyDisposizione.sComuneCatastUbicazione = CStr(DvDati.Item(x)("comunecat"))
                oMyDisposizione.sPVAmmUbicazione = CStr(DvDati.Item(x)("provincia_sigla"))
            Next
            Return oMyDisposizione
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsAETtracciati.GetEnte.errore: ", Err)
            Throw Err
        End Try
    End Function
#Region "Select"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sMyIdEnte"></param>
    ''' <param name="sMyAnno"></param>
    ''' <param name="sMyDataDal"></param>
    ''' <returns></returns>
    Public Function GetDatiEstrazione(ByVal sMyIdEnte As String, ByVal sMyAnno As String, ByVal sMyDataDal As String) As DataView
        Dim dvMyDati As New DataView
        Try
            'Valorizzo la connessione
            'cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnectionOPENgov)
            'Valorizzo il commandtext:
            'cmdMyCommand.CommandText = "SELECT *"
            'cmdMyCommand.CommandText += " FROM OPENae_GET_DATITRACCIATO"
            'cmdMyCommand.CommandText += " WHERE (IDENTE=@CODISTAT)"
            'cmdMyCommand.CommandText += " AND ((YEAR(DATA_INIZIO)<=@ANNO"
            'cmdMyCommand.CommandText += " AND (CASE WHEN NOT YEAR(DATA_FINE) IS NULL THEN YEAR(DATA_FINE) ELSE @ANNO END)>=@ANNO))"
            'cmdMyCommand.CommandText += " AND (DATA_INIZIO>=@DAL)"
            ''Valorizzo i parameters:
            'cmdMyCommand.Parameters.Clear()
            'cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODISTAT", SqlDbType.NVarChar)).Value = sMyIdEnte
            'cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = sMyAnno
            'cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DAL", SqlDbType.DateTime)).Value = FncReplace.FormattaData(sMyDataDal, "A")
            'myAdapter.SelectCommand = cmdMyCommand
            'myAdapter.Fill(dtMyDati)
            'dvMyDati = dtMyDati.DefaultView

            Dim oDbManagerRepository As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
            Dim sSQL As String = ""
            Using ctx As DBModel = oDbManagerRepository
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_AEGetDatiEstrazione", "CODISTAT", "ANNO", "DAL")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("CODISTAT", sMyIdEnte) _
                        , ctx.GetParam("ANNO", sMyAnno) _
                        , ctx.GetParam("DAL", sMyDataDal)
                    )
                ctx.Dispose()
            End Using

            Return dvMyDati
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsAETtracciati.GetDatiEstrazione.errore: ", Err)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sMyIdEnte"></param>
    ''' <param name="sMyAnno"></param>
    ''' <param name="sMyDataDal"></param>
    ''' <returns></returns>
    Public Function GetDatiMancanti(ByVal sMyIdEnte As String, ByVal sMyAnno As String, ByVal sMyDataDal As String) As DataView
        Dim dvMyDati As New DataView
        Try
            'Valorizzo la connessione
            Dim oDbManagerRepository As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
            Dim sSQL As String = ""
            Using ctx As DBModel = oDbManagerRepository
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure,"prc_AEGetDatiMancanti", "CODISTAT", "ANNO", "DAL")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("CODISTAT", sMyIdEnte) _
                        , ctx.GetParam("ANNO", sMyAnno) _
                        , ctx.GetParam("DAL", sMyDataDal)
                    )
                ctx.Dispose()
            End Using

            Return dvMyDati
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsAETtracciati.GetDatiMancanti.errore: ", Err)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sMyIdEnte"></param>
    ''' <returns></returns>
    Private Function GetDatiEnte(ByVal sMyIdEnte As String) As DataView
        Dim dvMyDati As New DataView
        Try
            Dim oDbManagerRepository As New DBModel(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
            Dim sSQL As String = ""
            Using ctx As DBModel = oDbManagerRepository
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "ENTI_S", "CODISTAT", "AMBIENTE", "BELFIORE")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("CODISTAT", sMyIdEnte) _
                                           , ctx.GetParam("AMBIENTE", "") _
                                           , ctx.GetParam("BELFIORE", "")
                                        )
                ctx.Dispose()
            End Using
            Return dvMyDati
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ClsAETtracciati.GetDatiEnte.errore: ", Err)
            Return Nothing
        End Try
    End Function
    'Private Function GetDatiEnte(ByVal sMyIdEnte As String) As DataView
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim dtMyDati As New DataTable()
    '    Dim dvMyDati As New DataView
    '    Try
    '        'Valorizzo la connessione
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnectionOPENgov)
    '        'Valorizzo il commandtext:
    '        cmdMyCommand.CommandText = "SELECT *"
    '        cmdMyCommand.CommandText += " FROM ENTI"
    '        'Valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.CommandText += " WHERE (COD_ENTE=@CODISTAT)"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODISTAT", SqlDbType.NVarChar)).Value = sMyIdEnte
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(dtMyDati)
    '        dvMyDati = dtMyDati.DefaultView
    '        Return dvMyDati
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsAETtracciati.GetDatiEnte.errore: ", Err)
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        Return Nothing
    '    End Try
    'End Function
#End Region
End Class