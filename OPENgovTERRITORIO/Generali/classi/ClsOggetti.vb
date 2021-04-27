Imports log4net

Public Class ObjFabbricato
    Private _ente As String
    Private _nomeFabbricato As String
    Private _CatCatastale As String
    Private _note As String
    Private _NumeroCivico As Integer
    Private _PosizioneCivico As String
    Private _esponentecivico As String
    Private _codFabbricato As Integer
    Private _CodVia As Integer
    Private _Piani As Integer
    Private _numCampanelli As Integer
    Private _numAlloggi As Integer
    Private _numAutorimesse As Integer
    Private _numDepositi As Integer
    Private _numTettoie As Integer
    Private _numNegozi As Integer
    Private _numUffici As Integer
    Private _numLaboratori As Integer
    Private _StatoConservazione As Integer
    Private _areeAmministrative As Integer
    Private _areeUrbanistiche As Integer
    Private _CodSottotetto As String
    Private _CodSeminterrato As String
    Private _codTipoFab As String
    Private _zona As Integer
    Private _Microzona As Integer

    Private _Targa As Boolean
    Private _bSenzaNumero As Boolean
    Private _bCondominio As Boolean
    Private _bCortile As Boolean

    Private _iCodFabbricatoReale As Integer

    Private _unitaImmobiliari As ObjUnitaImmobiliare()
    Private _Passi As ObjPasso()
    Private _condominio As ObjCondominio
    'Private _codareaa As Integer
    'Private _codareau As Integer
    'Private _codzona As Integer
    'Private _codmicrozona As Integer
    'Private _codecografico As Integer
    'Private _codstrada1 As Integer
    'Private _numcivico1 As String
    'Private _numerocivico As Integer
    'Private _flagsn As Boolean
    'Private _codtipofabbricato As String
    'Private _flagcondominio As Boolean
    'Private _codcondominio As Integer
    'Private _flagpresenzacortile As Boolean
    'Private _infoseminterrato As String
    'Private _infosottotetto As String
    'Private _idutenza As Integer
    'Private _idreferente As Integer
    'Private _catcatastaleteorica As String
    'Private _statofabbricato As String
    'Private _notedatigenerali As String
    'Private _statoascensore As Integer
    'Private _statobarriere As Integer
    'Private _statoamianto As Integer
    'Private _codtipocopertura As Integer
    'Private _statointeressestorico As Integer
    'Private _flagsoggettocalamita As Integer
    'Private _notecaratteristiche As String
    'Private _datacostruzione As String
    'Private _dataultimaristrutturazione As String
    'Private _provenienzacatasto As String
    'Private _fabbricatodefinito As String

    Public Property sIdEnte() As String
        Get
            Return _ente
        End Get
        Set(ByVal Value As String)
            _ente = Value
        End Set
    End Property
    Public Property sNomeFabbricato() As String
        Get
            Return _nomeFabbricato
        End Get
        Set(ByVal Value As String)
            _nomeFabbricato = Value
        End Set
    End Property
    Public Property sCatCatastale() As String
        Get
            Return _CatCatastale
        End Get
        Set(ByVal Value As String)
            _CatCatastale = Value
        End Set
    End Property
    Public Property sNote() As String
        Get
            Return _note
        End Get
        Set(ByVal Value As String)
            _note = Value
        End Set
    End Property
    Public Property nNumeroCivico() As Integer
        Get
            Return _NumeroCivico
        End Get
        Set(ByVal Value As Integer)
            _NumeroCivico = Value
        End Set
    End Property
    Public Property sPosizioneCivico() As String
        Get
            Return _PosizioneCivico
        End Get
        Set(ByVal Value As String)
            _PosizioneCivico = Value
        End Set
    End Property
    Public Property sEsponenteCivico() As String
        Get
            Return _esponentecivico
        End Get
        Set(ByVal Value As String)
            _esponentecivico = Value
        End Set
    End Property
    Public Property nIdFabbricato() As Integer
        Get
            Return _codFabbricato
        End Get
        Set(ByVal Value As Integer)
            _codFabbricato = Value
        End Set
    End Property
    Public Property nCodVia() As Integer
        Get
            Return _CodVia
        End Get
        Set(ByVal Value As Integer)
            _CodVia = Value
        End Set
    End Property
    Public Property nPiani() As Integer
        Get
            Return _Piani
        End Get
        Set(ByVal Value As Integer)
            _Piani = Value
        End Set
    End Property
    Public Property nCampanelli() As Integer
        Get
            Return _numCampanelli
        End Get
        Set(ByVal Value As Integer)
            _numCampanelli = Value
        End Set
    End Property
    Public Property nAlloggi() As Integer
        Get
            Return _numAlloggi
        End Get
        Set(ByVal Value As Integer)
            _numAlloggi = Value
        End Set
    End Property
    Public Property nAutorimesse() As Integer
        Get
            Return _numAutorimesse
        End Get
        Set(ByVal Value As Integer)
            _numAutorimesse = Value
        End Set
    End Property
    Public Property nDepositi() As Integer
        Get
            Return _numDepositi
        End Get
        Set(ByVal Value As Integer)
            _numDepositi = Value
        End Set
    End Property
    Public Property nTettoie() As Integer
        Get
            Return _numTettoie
        End Get
        Set(ByVal Value As Integer)
            _numTettoie = Value
        End Set
    End Property
    Public Property nNegozi() As Integer
        Get
            Return _numNegozi
        End Get
        Set(ByVal Value As Integer)
            _numNegozi = Value
        End Set
    End Property
    Public Property nUffici() As Integer
        Get
            Return _numUffici
        End Get
        Set(ByVal Value As Integer)
            _numUffici = Value
        End Set
    End Property
    Public Property nLaboratori() As Integer
        Get
            Return _numLaboratori
        End Get
        Set(ByVal Value As Integer)
            _numLaboratori = Value
        End Set
    End Property
    Public Property nStatoConservazione() As Integer
        Get
            Return _StatoConservazione
        End Get
        Set(ByVal Value As Integer)
            _StatoConservazione = Value
        End Set
    End Property
    Public Property nAreeAmministrative() As Integer
        Get
            Return _areeAmministrative
        End Get
        Set(ByVal Value As Integer)
            _areeAmministrative = Value
        End Set
    End Property
    Public Property nAreeUrbanistiche() As Integer
        Get
            Return _areeUrbanistiche
        End Get
        Set(ByVal Value As Integer)
            _areeUrbanistiche = Value
        End Set
    End Property
    Public Property sCodSottotetto() As String
        Get
            Return _CodSottotetto
        End Get
        Set(ByVal Value As String)
            _CodSottotetto = Value
        End Set
    End Property
    Public Property sCodSeminterrato() As String
        Get
            Return _CodSeminterrato
        End Get
        Set(ByVal Value As String)
            _CodSeminterrato = Value
        End Set
    End Property
    Public Property sCodTipoFab() As String
        Get
            Return _codTipoFab
        End Get
        Set(ByVal Value As String)
            _codTipoFab = Value
        End Set
    End Property
    Public Property nZona() As Integer
        Get
            Return _zona
        End Get
        Set(ByVal Value As Integer)
            _zona = Value
        End Set
    End Property
    Public Property nMicrozona() As Integer
        Get
            Return _Microzona
        End Get
        Set(ByVal Value As Integer)
            _Microzona = Value
        End Set
    End Property
    Public Property bTarga() As Boolean
        Get
            Return _Targa
        End Get
        Set(ByVal Value As Boolean)
            _Targa = Value
        End Set
    End Property
    Public Property bSenzaNumero() As Boolean
        Get
            Return _bSenzaNumero
        End Get
        Set(ByVal Value As Boolean)
            _bSenzaNumero = Value
        End Set
    End Property
    Public Property bCondominio() As Boolean
        Get
            Return _bCondominio
        End Get
        Set(ByVal Value As Boolean)
            _bCondominio = Value
        End Set
    End Property
    Public Property bCortile() As Boolean
        Get
            Return _bCortile
        End Get
        Set(ByVal Value As Boolean)
            _bCortile = Value
        End Set
    End Property
    Public Property iCodFabbricatoReale() As Integer
        Get
            Return _iCodFabbricatoReale
        End Get
        Set(ByVal Value As Integer)
            _iCodFabbricatoReale = Value
        End Set
    End Property
    Public Property oListUnitaImmobiliari() As ObjUnitaImmobiliare()
        Get
            Return _unitaImmobiliari
        End Get
        Set(ByVal Value As ObjUnitaImmobiliare())
            _unitaImmobiliari = Value
        End Set
    End Property
    Public Property oListPassi() As ObjPasso()
        Get
            Return _Passi
        End Get
        Set(ByVal Value As ObjPasso())
            _Passi = Value
        End Set
    End Property
    Public Property oCondominio() As ObjCondominio
        Get
            Return _condominio
        End Get
        Set(ByVal Value As ObjCondominio)
            _condominio = Value
        End Set
    End Property

    Sub New()
        _ente = ""
        _CodVia = -1
        _nomeFabbricato = ""
        _CatCatastale = "-1"
        _note = ""
        _NumeroCivico = -1
        _PosizioneCivico = ""
        _esponentecivico = ""
        _codFabbricato = -1
        _Piani = -1
        _numCampanelli = -1
        _numAlloggi = -1
        _numAutorimesse = -1
        _numDepositi = -1
        _numTettoie = -1
        _numNegozi = -1
        _numUffici = -1
        _numLaboratori = -1
        _StatoConservazione = -1
        _areeAmministrative = -1
        _areeUrbanistiche = -1
        _CodSottotetto = ""
        _CodSeminterrato = ""
        _codTipoFab = ""
        _zona = -1
        _Microzona = -1
        _Targa = False
        _bSenzaNumero = False
        _bCondominio = False
        _bCortile = False
        _unitaImmobiliari = Nothing
        _Passi = Nothing
        _condominio = New ObjCondominio
    End Sub
End Class

Public Class ObjUnitaImmobiliare
    Private _codUI As Integer
    Private _codFabbricato As Integer
    Private _codZonaCensuaria As Integer
    Private _foglio As Integer
    Private _numero As String
    Private _subalterno As Integer
    Private _codCessazione As String
    Private _ente As String
    Private _nomeFabbricato As String
    Private _indirizzo As String
    Private _interno As String
    Private _sezione As String
    Private _Scala As String
    Private _InternoCortile As String
    Private _InternoGarage As String
    Private _codEcografico As String
    Private _graffatura As String
    Private _provCatastale As String
    Private _note As String
    Private _dal As DateTime
    Private _al As DateTime
    Private _classificazione As ObjClassificazione()
    Private _Piani As ObjPiano()

    Public Property nIdUnitaImmobiliare() As Integer
        Get
            Return _codUI
        End Get
        Set(ByVal Value As Integer)
            _codUI = Value
        End Set
    End Property
    Public Property nIdFabbricato() As Integer
        Get
            Return _codFabbricato
        End Get
        Set(ByVal Value As Integer)
            _codFabbricato = Value
        End Set
    End Property
    Public Property nCodZonaCensuaria() As Integer
        Get
            Return _codZonaCensuaria
        End Get
        Set(ByVal Value As Integer)
            _codZonaCensuaria = Value
        End Set
    End Property
    Public Property nFoglio() As Integer
        Get
            Return _foglio
        End Get
        Set(ByVal Value As Integer)
            _foglio = Value
        End Set
    End Property
    Public Property sNumero() As String
        Get
            Return _numero
        End Get
        Set(ByVal Value As String)
            _numero = Value
        End Set
    End Property
    Public Property nSubalterno() As Integer
        Get
            Return _subalterno
        End Get
        Set(ByVal Value As Integer)
            _subalterno = Value
        End Set
    End Property
    Public Property sCodCessazione() As String
        Get
            Return _codCessazione
        End Get
        Set(ByVal Value As String)
            _codCessazione = Value
        End Set
    End Property
    Public Property sIdEnte() As String
        Get
            Return _ente
        End Get
        Set(ByVal Value As String)
            _ente = Value
        End Set
    End Property
    Public Property sNomeFabbricato() As String
        Get
            Return _nomeFabbricato
        End Get
        Set(ByVal Value As String)
            _nomeFabbricato = Value
        End Set
    End Property
    Public Property sIndirizzo() As String
        Get
            Return _indirizzo
        End Get
        Set(ByVal Value As String)
            _indirizzo = Value
        End Set
    End Property
    Public Property sInterno() As String
        Get
            Return _interno
        End Get
        Set(ByVal Value As String)
            _interno = Value
        End Set
    End Property
    Public Property sSezione() As String
        Get
            Return _sezione
        End Get
        Set(ByVal Value As String)
            _sezione = Value
        End Set
    End Property
    Public Property sScala() As String
        Get
            Return _Scala
        End Get
        Set(ByVal Value As String)
            _Scala = Value
        End Set
    End Property
    Public Property sInternoCortile() As String
        Get
            Return _InternoCortile
        End Get
        Set(ByVal Value As String)
            _InternoCortile = Value
        End Set
    End Property
    Public Property sInternoGarage() As String
        Get
            Return _InternoGarage
        End Get
        Set(ByVal Value As String)
            _InternoGarage = Value
        End Set
    End Property
    Public Property sCodEcografico() As String
        Get
            Return _codEcografico
        End Get
        Set(ByVal Value As String)
            _codEcografico = Value
        End Set
    End Property
    Public Property sGraffatura() As String
        Get
            Return _graffatura
        End Get
        Set(ByVal Value As String)
            _graffatura = Value
        End Set
    End Property
    Public Property sProvCatastale() As String
        Get
            Return _provCatastale
        End Get
        Set(ByVal Value As String)
            _provCatastale = Value
        End Set
    End Property
    Public Property sNote() As String
        Get
            Return _note
        End Get
        Set(ByVal Value As String)
            _note = Value
        End Set
    End Property
    Public Property dDal() As DateTime
        Get
            Return _dal
        End Get
        Set(ByVal Value As DateTime)
            _dal = Value
        End Set
    End Property
    Public Property dAl() As DateTime
        Get
            Return _al
        End Get
        Set(ByVal Value As DateTime)
            _al = Value
        End Set
    End Property
    Public Property oListClassificazioni() As ObjClassificazione()
        Get
            Return _classificazione
        End Get
        Set(ByVal Value As ObjClassificazione())
            _classificazione = Value
        End Set
    End Property
    Public Property oListPiani() As ObjPiano()
        Get
            Return _Piani
        End Get
        Set(ByVal Value As ObjPiano())
            _Piani = Value
        End Set
    End Property

    '*** per visualizzazione in videata ***
    Private _zonacensuaria As String
    Public Property sZonaCensuaria() As String
        Get
            Return _zonacensuaria
        End Get
        Set(ByVal Value As String)
            _zonacensuaria = Value
        End Set
    End Property
    '*** ***

    Sub New()
        _codUI = -1
        _codFabbricato = -1
        _codZonaCensuaria = -1
        _foglio = -1
        _numero = ""
        _subalterno = -1
        _ente = ""
        _nomeFabbricato = ""
        _indirizzo = ""
        _interno = ""
        _sezione = ""
        _Scala = ""
        _InternoCortile = ""
        _InternoGarage = ""
        _codEcografico = ""
        _graffatura = ""
        _provCatastale = ""
        _codCessazione = ""
        _note = ""
        _dal = Date.MaxValue.ToShortDateString
        _al = Date.MaxValue.ToShortDateString
        _classificazione = Nothing
        _Piani = Nothing
        _zonacensuaria = ""
    End Sub
End Class

Public Class ObjClassificazione
    Private _codclassificazione As Integer
    Private _codUI As Integer
    Private _DifformitaCat As Integer
    Private _codTipoRendita As Integer
    Private _coddestuso As Integer
    Private _codinagibilita As Integer
    Private _consistenza As Integer
    Private _codCategoriaCatastale As String
    Private _codClasse As String
    Private _valoreRendita As String
    Private _nprotocollo As String
    Private _Note As String
    Private _superficieCatastale As Double
    Private _superficieNetta As Double
    Private _superficieLorda As Double
    Private _flagPertinenza As Boolean
    Private _inagibilita As Boolean
    Private _dataInizio As DateTime
    Private _dataFine As DateTime
    Private _dataattribuzione As DateTime
    Private _dataefficacia As DateTime
    Private _dataprotocollo As DateTime
    Private _dataeffettivoutilizzo As DateTime
    Private _datafinelavori As DateTime
    Private _vani As ObjVano()
    Private _proprieta As ObjProprieta()
    Private _conduzioni As ObjConduzione()

    Public Property nIdClassificazione() As Integer
        Get
            Return _codclassificazione
        End Get
        Set(ByVal Value As Integer)
            _codclassificazione = Value
        End Set
    End Property
    Public Property nIdUI() As Integer
        Get
            Return _codUI
        End Get
        Set(ByVal Value As Integer)
            _codUI = Value
        End Set
    End Property
    Public Property nDifformitaCat() As Integer
        Get
            Return _DifformitaCat
        End Get
        Set(ByVal Value As Integer)
            _DifformitaCat = Value
        End Set
    End Property
    Public Property nCodTipoRendita() As Integer
        Get
            Return _codTipoRendita
        End Get
        Set(ByVal Value As Integer)
            _codTipoRendita = Value
        End Set
    End Property
    Public Property nCodDestUso() As Integer
        Get
            Return _coddestuso
        End Get
        Set(ByVal Value As Integer)
            _coddestuso = Value
        End Set
    End Property
    Public Property nCodInagibilita() As Integer
        Get
            Return _codinagibilita
        End Get
        Set(ByVal Value As Integer)
            _codinagibilita = Value
        End Set
    End Property
    Public Property nConsistenza() As Integer
        Get
            Return _consistenza
        End Get
        Set(ByVal Value As Integer)
            _consistenza = Value
        End Set
    End Property
    Public Property sCodCategoriaCatastale() As String
        Get
            Return _codCategoriaCatastale
        End Get
        Set(ByVal Value As String)
            _codCategoriaCatastale = Value
        End Set
    End Property
    Public Property sCodClasse() As String
        Get
            Return _codClasse
        End Get
        Set(ByVal Value As String)
            _codClasse = Value
        End Set
    End Property
    Public Property sValoreRendita() As String
        Get
            Return _valoreRendita
        End Get
        Set(ByVal Value As String)
            _valoreRendita = Value
        End Set
    End Property
    Public Property sNProtocollo() As String
        Get
            Return _nprotocollo
        End Get
        Set(ByVal Value As String)
            _nprotocollo = Value
        End Set
    End Property
    Public Property sNote() As String
        Get
            Return _Note
        End Get
        Set(ByVal Value As String)
            _Note = Value
        End Set
    End Property
    Public Property nSuperficieCatastale() As Double
        Get
            Return _superficieCatastale
        End Get
        Set(ByVal Value As Double)
            _superficieCatastale = Value
        End Set
    End Property
    Public Property nSuperficieNetta() As Double
        Get
            Return _superficieNetta
        End Get
        Set(ByVal Value As Double)
            _superficieNetta = Value
        End Set
    End Property
    Public Property nSuperficieLorda() As Double
        Get
            Return _superficieLorda
        End Get
        Set(ByVal Value As Double)
            _superficieLorda = Value
        End Set
    End Property
    Public Property bFlagPertinenza() As Boolean
        Get
            Return _flagPertinenza
        End Get
        Set(ByVal Value As Boolean)
            _flagPertinenza = Value
        End Set
    End Property
    Public Property bInagibilita() As Boolean
        Get
            Return _inagibilita
        End Get
        Set(ByVal Value As Boolean)
            _inagibilita = Value
        End Set
    End Property
    Public Property dDal() As DateTime
        Get
            Return _dataInizio
        End Get
        Set(ByVal Value As DateTime)
            _dataInizio = Value
        End Set
    End Property
    Public Property dAl() As DateTime
        Get
            Return _dataFine
        End Get
        Set(ByVal Value As DateTime)
            _dataFine = Value
        End Set
    End Property
    Public Property dDataAttribuzione() As DateTime
        Get
            Return _dataattribuzione
        End Get
        Set(ByVal Value As DateTime)
            _dataattribuzione = Value
        End Set
    End Property
    Public Property dDataEfficacia() As DateTime
        Get
            Return _dataefficacia
        End Get
        Set(ByVal Value As DateTime)
            _dataefficacia = Value
        End Set
    End Property
    Public Property dDataProtocollo() As DateTime
        Get
            Return _dataprotocollo
        End Get
        Set(ByVal Value As DateTime)
            _dataprotocollo = Value
        End Set
    End Property
    Public Property dDataEffettivoUtilizzo() As DateTime
        Get
            Return _dataeffettivoutilizzo
        End Get
        Set(ByVal Value As DateTime)
            _dataeffettivoutilizzo = Value
        End Set
    End Property
    Public Property dDataFineLavori() As DateTime
        Get
            Return _datafinelavori
        End Get
        Set(ByVal Value As DateTime)
            _datafinelavori = Value
        End Set
    End Property
    Public Property oListVani() As ObjVano()
        Get
            Return _vani
        End Get
        Set(ByVal Value As ObjVano())
            _vani = Value
        End Set
    End Property
    Public Property oListProprieta() As ObjProprieta()
        Get
            Return _proprieta
        End Get
        Set(ByVal Value As ObjProprieta())
            _proprieta = Value
        End Set
    End Property
    Public Property oListConduzioni() As ObjConduzione()
        Get
            Return _conduzioni
        End Get
        Set(ByVal Value As ObjConduzione())
            _conduzioni = Value
        End Set
    End Property
    '*** per visualizzazione in videata ***
    Private _tipoRendita As String
    Public Property sTipoRendita() As String
        Get
            Return _tipoRendita
        End Get
        Set(ByVal Value As String)
            _tipoRendita = Value
        End Set
    End Property
    '*** ***

    Sub New()
        _codclassificazione = -1
        _codUI = -1
        _DifformitaCat = -1
        _codTipoRendita = -1
        _consistenza = -1
        _codCategoriaCatastale = ""
        _codClasse = ""
        _valoreRendita = ""
        _superficieCatastale = 0
        _superficieNetta = 0
        _superficieLorda = 0
        _flagPertinenza = False
        _dataInizio = Date.MaxValue.ToShortDateString
        _dataFine = Date.MaxValue.ToShortDateString
        _vani = Nothing
        _proprieta = Nothing
        _tipoRendita = ""
        _dataattribuzione = Date.MaxValue.ToShortDateString
        _dataefficacia = Date.MaxValue.ToShortDateString
        _dataprotocollo = Date.MaxValue.ToShortDateString
        _nprotocollo = ""
        _dataeffettivoutilizzo = Date.MaxValue.ToShortDateString
        _datafinelavori = Date.MaxValue.ToShortDateString
        _coddestuso = -1
        _inagibilita = False
        _codinagibilita = -1
        _Note = ""
        _conduzioni = Nothing
    End Sub
End Class

Public Class ObjVano
    Private _codVano As Integer
    Private _codClassificazione As Integer
    Private _piano As Integer
    Private _percentUso As Integer
    Private _TipoVano As String
    Private _mq As Double
    Private _pesoCat As Double
    Private _NomeRaster As String
    Private _note As String

    Public Property nIdVano() As Integer
        Get
            Return _codVano
        End Get
        Set(ByVal Value As Integer)
            _codVano = Value
        End Set
    End Property
    Public Property nIdClassificazione() As Integer
        Get
            Return _codClassificazione
        End Get
        Set(ByVal Value As Integer)
            _codClassificazione = Value
        End Set
    End Property
    Public Property nPiano() As Integer
        Get
            Return _piano
        End Get
        Set(ByVal Value As Integer)
            _piano = Value
        End Set
    End Property
    Public Property nPercentUso() As Integer
        Get
            Return _percentUso
        End Get
        Set(ByVal Value As Integer)
            _percentUso = Value
        End Set
    End Property
    Public Property nTipoVano() As Integer
        Get
            Return _TipoVano
        End Get
        Set(ByVal Value As Integer)
            _TipoVano = Value
        End Set
    End Property
    Public Property nMQ() As Double
        Get
            Return _mq
        End Get
        Set(ByVal Value As Double)
            _mq = Value
        End Set
    End Property
    Public Property nPesoCat() As Double
        Get
            Return _pesoCat
        End Get
        Set(ByVal Value As Double)
            _pesoCat = Value
        End Set
    End Property
    Public Property sNomeRaster() As String
        Get
            Return _NomeRaster
        End Get
        Set(ByVal Value As String)
            _NomeRaster = Value
        End Set
    End Property
    Public Property sNote() As String
        Get
            Return _note
        End Get
        Set(ByVal Value As String)
            _note = Value
        End Set
    End Property
    '*** per visualizzazione in videata ***
    Private _Vano As String
    Private _tipoPiano As String
    Public Property sTipoPiano() As String
        Get
            Return _tipoPiano
        End Get
        Set(ByVal Value As String)
            _tipoPiano = Value
        End Set
    End Property
    Public Property sTipoVano() As String
        Get
            Return _Vano
        End Get
        Set(ByVal Value As String)
            _Vano = Value
        End Set
    End Property
    '*** ***

    Sub New()
        _codVano = -1
        _codClassificazione = -1
        _percentUso = 0
        _piano = -1
        _TipoVano = -1
        _mq = 0
        _pesoCat = 0
        _NomeRaster = ""
        _tipoPiano = ""
        _Vano = ""
        _note = ""
    End Sub
End Class

Public Class ObjProprieta
    Private _codProprieta As Integer
    Private _codProprietario As Integer
    Private _codClassificazione As Integer
    Private _dataInizio As DateTime
    Private _dataFine As DateTime
    Private _codAnagrafica As Integer
    Private _tipoProprieta As Integer
    Private _tipoPossesso As Integer
    Private _tipoUtilizzo As Integer
    Private _tipoParentela As Integer
    Private _cognome As String
    Private _nome As String
    Private _proprieta As String
    Private _possesso As String
    Private _utilizzo As String
    Private _parentela As String
    Private _note As String
    Private _percentProprieta As Double
    Private _percentPossesso As Double

    Public Property nIdProprieta() As Integer
        Get
            Return _codProprieta
        End Get
        Set(ByVal Value As Integer)
            _codProprieta = Value
        End Set
    End Property
    Public Property nIdClassificazione() As Integer
        Get
            Return _codClassificazione
        End Get
        Set(ByVal Value As Integer)
            _codClassificazione = Value
        End Set
    End Property
    Public Property nIdProprietario() As Integer
        Get
            Return _codProprietario
        End Get
        Set(ByVal Value As Integer)
            _codProprietario = Value
        End Set
    End Property
    Public Property nIdAnagrafica() As Integer
        Get
            Return _codAnagrafica
        End Get
        Set(ByVal Value As Integer)
            _codAnagrafica = Value
        End Set
    End Property
    Public Property nTipoProprieta() As Integer
        Get
            Return _tipoProprieta
        End Get
        Set(ByVal Value As Integer)
            _tipoProprieta = Value
        End Set
    End Property
    Public Property nTipoPossesso() As Integer
        Get
            Return _tipoPossesso
        End Get
        Set(ByVal Value As Integer)
            _tipoPossesso = Value
        End Set
    End Property
    Public Property nTipoUtilizzo() As Integer
        Get
            Return _tipoUtilizzo
        End Get
        Set(ByVal Value As Integer)
            _tipoUtilizzo = Value
        End Set
    End Property
    Public Property nTipoParentela() As Integer
        Get
            Return _tipoParentela
        End Get
        Set(ByVal Value As Integer)
            _tipoParentela = Value
        End Set
    End Property
    Public Property dDataInizio() As DateTime
        Get
            Return _dataInizio
        End Get
        Set(ByVal Value As DateTime)
            _dataInizio = Value
        End Set
    End Property
    Public Property dDataFine() As DateTime
        Get
            Return _dataFine
        End Get
        Set(ByVal Value As DateTime)
            _dataFine = Value
        End Set
    End Property
    Public Property sCognome() As String
        Get
            Return _cognome
        End Get
        Set(ByVal Value As String)
            _cognome = Value
        End Set
    End Property
    Public Property sNome() As String
        Get
            Return _nome
        End Get
        Set(ByVal Value As String)
            _nome = Value
        End Set
    End Property
    Public Property sProprieta() As String
        Get
            Return _proprieta
        End Get
        Set(ByVal Value As String)
            _proprieta = Value
        End Set
    End Property
    Public Property sPossesso() As String
        Get
            Return _possesso
        End Get
        Set(ByVal Value As String)
            _possesso = Value
        End Set
    End Property
    Public Property sUtilizzo() As String
        Get
            Return _utilizzo
        End Get
        Set(ByVal Value As String)
            _utilizzo = Value
        End Set
    End Property
    Public Property sParentela() As String
        Get
            Return _parentela
        End Get
        Set(ByVal Value As String)
            _parentela = Value
        End Set
    End Property
    Public Property sNote() As String
        Get
            Return _note
        End Get
        Set(ByVal Value As String)
            _note = Value
        End Set
    End Property
    Public Property nPercentProprieta() As Double
        Get
            Return _percentProprieta
        End Get
        Set(ByVal Value As Double)
            _percentProprieta = Value
        End Set
    End Property
    Public Property nPercentPossesso() As Double
        Get
            Return _percentPossesso
        End Get
        Set(ByVal Value As Double)
            _percentPossesso = Value
        End Set
    End Property

    Sub New()
        _codProprieta = -1
        _codClassificazione = -1
        _codProprietario = -1
        _codAnagrafica = -1
        _tipoProprieta = -1
        _tipoPossesso = -1
        _tipoUtilizzo = -1
        _tipoParentela = -1
        _dataInizio = Date.MaxValue.ToShortDateString
        _dataFine = Date.MaxValue.ToShortDateString
        _cognome = ""
        _nome = ""
        _proprieta = ""
        _possesso = ""
        _utilizzo = ""
        _parentela = ""
        _note = ""
        _percentProprieta = 0
        _percentPossesso = 0
    End Sub
End Class

Public Class ObjConduzione
    Private _codConduzione As Integer
    Private _codClassificazione As Integer
    Private _codStatoOccupazione As Integer
    Private _codTipoOccupazione As Integer
    Private _codTipoUtilizzo As Integer
    Private _codConduttore As Integer
    Private _codAnagrafica As Integer
    Private _numOccupanti As Integer
    Private _cognome As String
    Private _nome As String
    Private _note As String
    Private _statoOccupazione As String
    Private _MotivoUtilizzo As String
    Private _percentUtilizzo As Double
    Private _dataInizio As DateTime
    Private _dataFine As DateTime

    Public Property nIdConduzione() As Integer
        Get
            Return _codConduzione
        End Get
        Set(ByVal Value As Integer)
            _codConduzione = Value
        End Set
    End Property
    Public Property nIdClassificazione() As Integer
        Get
            Return _codClassificazione
        End Get
        Set(ByVal Value As Integer)
            _codClassificazione = Value
        End Set
    End Property
    Public Property dDataInizio() As DateTime
        Get
            Return _dataInizio
        End Get
        Set(ByVal Value As DateTime)
            _dataInizio = Value
        End Set
    End Property
    Public Property dDataFine() As DateTime
        Get
            Return _dataFine
        End Get
        Set(ByVal Value As DateTime)
            _dataFine = Value
        End Set
    End Property
    Public Property nCodStatoOccupazione() As Integer
        Get
            Return _codStatoOccupazione
        End Get
        Set(ByVal Value As Integer)
            _codStatoOccupazione = Value
        End Set
    End Property
    Public Property nCodTipoOccupazione() As Integer
        Get
            Return _codTipoOccupazione
        End Get
        Set(ByVal Value As Integer)
            _codTipoOccupazione = Value
        End Set
    End Property
    Public Property nTipoUtilizzo() As Integer
        Get
            Return _codTipoUtilizzo
        End Get
        Set(ByVal Value As Integer)
            _codTipoUtilizzo = Value
        End Set
    End Property
    Public Property nIdConduttore() As Integer
        Get
            Return _codConduttore
        End Get
        Set(ByVal Value As Integer)
            _codConduttore = Value
        End Set
    End Property
    Public Property nIdAnagrafica() As Integer
        Get
            Return _codAnagrafica
        End Get
        Set(ByVal Value As Integer)
            _codAnagrafica = Value
        End Set
    End Property
    Public Property nOccupanti() As Integer
        Get
            Return _numOccupanti
        End Get
        Set(ByVal Value As Integer)
            _numOccupanti = Value
        End Set
    End Property
    Public Property sCognome() As String
        Get
            Return _cognome
        End Get
        Set(ByVal Value As String)
            _cognome = Value
        End Set
    End Property
    Public Property sNome() As String
        Get
            Return _nome
        End Get
        Set(ByVal Value As String)
            _nome = Value
        End Set
    End Property
    Public Property sNote() As String
        Get
            Return _note
        End Get
        Set(ByVal Value As String)
            _note = Value
        End Set
    End Property
    Public Property sStatoOccupazione() As String
        Get
            Return _statoOccupazione
        End Get
        Set(ByVal Value As String)
            _statoOccupazione = Value
        End Set
    End Property
    Public Property sMotivoUtilizzo() As String
        Get
            Return _MotivoUtilizzo
        End Get
        Set(ByVal Value As String)
            _MotivoUtilizzo = Value
        End Set
    End Property
    Public Property nPercentUtilizzo() As Double
        Get
            Return _percentUtilizzo
        End Get
        Set(ByVal Value As Double)
            _percentUtilizzo = Value
        End Set
    End Property

    Sub New()
        _codConduzione = -1
        _codClassificazione = -1
        _codStatoOccupazione = -1
        _codTipoOccupazione = -1
        _codTipoUtilizzo = -1
        _codConduttore = -1
        _codAnagrafica = -1
        _cognome = ""
        _nome = ""
        _statoOccupazione = ""
        _MotivoUtilizzo = ""
        _note = ""
        _percentUtilizzo = 0
        _numOccupanti = 0
        _dataInizio = Date.MaxValue.ToShortDateString
        _dataFine = Date.MaxValue.ToShortDateString
    End Sub
End Class

Public Class ObjPasso
    Private _IdPasso As Integer
    Private _codFabbricato As Integer
    Private _CodTipoPasso As Integer

    Public Property nIdPasso() As Integer
        Get
            Return _IdPasso
        End Get
        Set(ByVal Value As Integer)
            _IdPasso = Value
        End Set
    End Property
    Public Property nIdFabbricato() As Integer
        Get
            Return _codFabbricato
        End Get
        Set(ByVal Value As Integer)
            _codFabbricato = Value
        End Set
    End Property
    Public Property nIdTipoPasso() As Integer
        Get
            Return _CodTipoPasso
        End Get
        Set(ByVal Value As Integer)
            _CodTipoPasso = Value
        End Set
    End Property

    Sub New()
        _IdPasso = -1
        _codFabbricato = -1
        _CodTipoPasso = -1
    End Sub
End Class

Public Class ObjPiano
    Private _IdPiano As Integer
    Private _codUI As Integer
    Private _CodTipoPiano As Integer

    Public Property nIdPiano() As Integer
        Get
            Return _IdPiano
        End Get
        Set(ByVal Value As Integer)
            _IdPiano = Value
        End Set
    End Property
    Public Property nIdUI() As Integer
        Get
            Return _codUI
        End Get
        Set(ByVal Value As Integer)
            _codUI = Value
        End Set
    End Property
    Public Property nIdTipoPiano() As Integer
        Get
            Return _CodTipoPiano
        End Get
        Set(ByVal Value As Integer)
            _CodTipoPiano = Value
        End Set
    End Property

    Sub New()
        _IdPiano = -1
        _codUI = -1
        _CodTipoPiano = -1
    End Sub
End Class

Public Class ObjCondominio
    Private _IdCondominio As Integer
    Private _DatiAmministratore As String
    Private _VieAmministrative As String

    Public Property nIdCondominio() As Integer
        Get
            Return _IdCondominio
        End Get
        Set(ByVal Value As Integer)
            _IdCondominio = Value
        End Set
    End Property
    Public Property sDatiAmministratore() As String
        Get
            Return _DatiAmministratore
        End Get
        Set(ByVal Value As String)
            _DatiAmministratore = Value
        End Set
    End Property
    Public Property sVieAmministrative() As String
        Get
            Return _VieAmministrative
        End Get
        Set(ByVal Value As String)
            _VieAmministrative = Value
        End Set
    End Property

    Sub New()
        _IdCondominio = -1
        _DatiAmministratore = ""
        _VieAmministrative = ""
    End Sub
End Class

Public Class ObjStradario
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(ObjStradario))
    Private _sEnte As String
    Private _lCodStrada As Long
    Private _bSenzaNumero As Integer = False
    Private _sPosizioneCivico As String = ""
    Private _iNCivico As Integer = 0
    Private _sEsponenteCivico As String = ""
    Private _iProgressivoCivico As Integer = 0
    Private _sTipoFabbr As String = ""
    Private _iCondominio As Integer = 0
    Private _sNomeFabbr As String = ""
    Private _iCortile As Integer = 0
    Private _sSeminterrato As String = ""
    Private _sSottotetto As String = ""
    Private _iCampanelli As Integer = 0
    Private _iAlloggi As Integer = 0
    Private _iPiani As Integer = 0
    Private _sCategoria As String = ""
    Private _iNegozi As Integer = 0
    Private _iNuffici As Integer = 0
    Private _iDepositi As Integer = 0
    Private _iLaboratori As Integer = 0
    Private _iAutorimesse As Integer = 0
    Private _iTettoie As Integer = 0
    Private _sNote As String = ""
    Private _nIdStradario As Integer
    Private _nIdFabbricato As Integer = 0
    Private _nIdCondominio As Integer = 0
    Private _nResidenti As Integer = 0
    Private _nUtenza As Integer = 0
    Private _nReferente As Integer = 0
    Private _sStato As String = ""
    Private _nLink As Integer = 0
    Private _nIdConservazione As Integer = 0
    Private _sDatiAmministratore As String = ""
    Private _sVieCiviciCondominio As String = ""

    Public Property sEnte() As String
        Get
            Return _sEnte
        End Get
        Set(ByVal Value As String)
            _sEnte = Value
        End Set
    End Property

    Public Property lCodStrada() As Long
        Get
            Return _lCodStrada
        End Get
        Set(ByVal Value As Long)
            _lCodStrada = Value
        End Set
    End Property

    Public Property bSenzaNumero() As Boolean
        Get
            Return _bSenzaNumero
        End Get
        Set(ByVal Value As Boolean)
            _bSenzaNumero = Value
        End Set
    End Property

    Public Property sPosizioneCivico() As String
        Get
            Return _sPosizioneCivico
        End Get
        Set(ByVal Value As String)
            _sPosizioneCivico = Value
        End Set
    End Property

    Public Property iNCivico() As Integer
        Get
            Return _iNCivico
        End Get
        Set(ByVal Value As Integer)
            _iNCivico = Value
        End Set
    End Property

    Public Property sEsponenteCivico() As String
        Get
            Return _sEsponenteCivico
        End Get
        Set(ByVal Value As String)
            _sEsponenteCivico = Value
        End Set
    End Property

    Public Property iProgressivoCivico() As Integer
        Get
            Return _iProgressivoCivico
        End Get
        Set(ByVal Value As Integer)
            _iProgressivoCivico = Value
        End Set
    End Property

    Public Property sTipoFabbr() As String
        Get
            Return _sTipoFabbr
        End Get
        Set(ByVal Value As String)
            _sTipoFabbr = Value
        End Set
    End Property

    Public Property iCondominio() As Integer
        Get
            Return _iCondominio
        End Get
        Set(ByVal Value As Integer)
            _iCondominio = Value
        End Set
    End Property
    Public Property sNomeFabbr() As String
        Get
            Return _sNomeFabbr
        End Get
        Set(ByVal Value As String)
            _sNomeFabbr = Value
        End Set
    End Property

    Public Property iCortile() As Integer
        Get
            Return _iCortile
        End Get
        Set(ByVal Value As Integer)
            _iCortile = Value
        End Set
    End Property

    Public Property sSeminterrato() As String
        Get
            Return _sSeminterrato
        End Get
        Set(ByVal Value As String)
            _sSeminterrato = Value
        End Set
    End Property

    Public Property sSottotetto() As String
        Get
            Return _sSottotetto
        End Get
        Set(ByVal Value As String)
            _sSottotetto = Value
        End Set
    End Property

    Public Property iCampanelli() As Integer
        Get
            Return _iCampanelli
        End Get
        Set(ByVal Value As Integer)
            _iCampanelli = Value
        End Set
    End Property

    Public Property iAlloggi() As Integer
        Get
            Return _iAlloggi
        End Get
        Set(ByVal Value As Integer)
            _iAlloggi = Value
        End Set
    End Property

    Public Property iPiani() As Integer
        Get
            Return _iPiani
        End Get
        Set(ByVal Value As Integer)
            _iPiani = Value
        End Set
    End Property

    Public Property sCategoria() As String
        Get
            Return _sCategoria
        End Get
        Set(ByVal Value As String)
            _sCategoria = Value
        End Set
    End Property

    Public Property iNegozi() As Integer
        Get
            Return _iNegozi
        End Get
        Set(ByVal Value As Integer)
            _iNegozi = Value
        End Set
    End Property

    Public Property iNuffici() As Integer
        Get
            Return _iNuffici
        End Get
        Set(ByVal Value As Integer)
            _iNuffici = Value
        End Set
    End Property

    Public Property iDepositi() As Integer
        Get
            Return _iDepositi
        End Get
        Set(ByVal Value As Integer)
            _iDepositi = Value
        End Set
    End Property

    Public Property iLaboratori() As Integer
        Get
            Return _iLaboratori
        End Get
        Set(ByVal Value As Integer)
            _iLaboratori = Value
        End Set
    End Property

    Public Property iAutorimesse() As Integer
        Get
            Return _iAutorimesse
        End Get
        Set(ByVal Value As Integer)
            _iAutorimesse = Value
        End Set
    End Property

    Public Property iTettoie() As Integer
        Get
            Return _iTettoie
        End Get
        Set(ByVal Value As Integer)
            _iTettoie = Value
        End Set
    End Property

    Public Property sNote() As String
        Get
            Return _sNote
        End Get
        Set(ByVal Value As String)
            _sNote = Value
        End Set
    End Property

    Public Property nIdStradario() As Integer
        Get
            Return _nIdStradario
        End Get
        Set(ByVal Value As Integer)
            _nIdStradario = Value
        End Set
    End Property
    Public Property nIdFabbricato() As Integer
        Get
            Return _nIdFabbricato
        End Get
        Set(ByVal Value As Integer)
            _nIdFabbricato = Value
        End Set
    End Property
    Public Property nIdCondominio() As Integer
        Get
            Return _nIdCondominio
        End Get
        Set(ByVal Value As Integer)
            _nIdCondominio = Value
        End Set
    End Property
    Public Property nResidenti() As Integer
        Get
            Return _nResidenti
        End Get
        Set(ByVal Value As Integer)
            _nResidenti = Value
        End Set
    End Property
    Public Property nUtenza() As Integer
        Get
            Return _nUtenza
        End Get
        Set(ByVal Value As Integer)
            _nUtenza = Value
        End Set
    End Property
    Public Property nReferente() As Integer
        Get
            Return _nReferente
        End Get
        Set(ByVal Value As Integer)
            _nReferente = Value
        End Set
    End Property
    Public Property sStato() As String
        Get
            Return _sStato
        End Get
        Set(ByVal Value As String)
            _sStato = Value
        End Set
    End Property
    Public Property nLink() As Integer
        Get
            Return _nLink
        End Get
        Set(ByVal Value As Integer)
            _nLink = Value
        End Set
    End Property
    Public Property nIdConservazione() As Integer
        Get
            Return _nIdConservazione
        End Get
        Set(ByVal Value As Integer)
            _nIdConservazione = Value
        End Set
    End Property
    Public Property sDatiAmministratore() As String
        Get
            Return _sDatiAmministratore
        End Get
        Set(ByVal Value As String)
            _sDatiAmministratore = Value
        End Set
    End Property
    Public Property sVieCiviciCondominio() As String
        Get
            Return _sVieCiviciCondominio
        End Get
        Set(ByVal Value As String)
            _sVieCiviciCondominio = Value
        End Set
    End Property

    Public Sub New()
    End Sub

    Public Sub New(ByVal drDati As SqlClient.SqlDataReader)
        Try
            If Not drDati Is Nothing Then
                Do While drDati.Read
                    If Not IsDBNull(drDati("COD_STRADARIO")) Then
                        Me.nIdStradario = CInt(drDati("COD_STRADARIO"))
                    End If
                    If Not IsDBNull(drDati("NOME_FABBRICATO")) Then
                        Me.sNomeFabbr = CStr(drDati("NOME_FABBRICATO"))
                    End If
                    If Not IsDBNull(drDati("POSIZIONE_CIVICO")) Then
                        Me.sPosizioneCivico = CStr(drDati("POSIZIONE_CIVICO"))
                    End If
                    If Not IsDBNull(drDati("COD_STRADA")) Then
                        Me.lCodStrada = CInt(drDati("COD_STRADA"))
                    End If
                    If Not IsDBNull(drDati("NUMERO_CIVICO")) Then
                        If CInt(drDati("NUMERO_CIVICO")) <> 0 Then
                            Me.iNCivico = CInt(drDati("NUMERO_CIVICO"))
                        End If
                    End If
                    If Not IsDBNull(drDati("ESPONENTE_CIVICO")) Then
                        Me.sEsponenteCivico = CStr(drDati("ESPONENTE_CIVICO"))
                    End If
                    If Not IsDBNull(drDati("PROGRESSIVO_CIVICO")) Then
                        If CInt(drDati("PROGRESSIVO_CIVICO")) <> 0 Then
                            Me.iProgressivoCivico = CInt(drDati("PROGRESSIVO_CIVICO"))
                        End If
                    End If
                    If Not IsDBNull(drDati("FLAG_SN")) Then
                        Me.bSenzaNumero = CBool(drDati("FLAG_SN"))
                    End If
                    If Not IsDBNull(drDati("COD_TIPO_FABBRICATO")) Then
                        Me.sTipoFabbr = CStr(drDati("COD_TIPO_FABBRICATO"))
                    End If
                    If Not IsDBNull(drDati("INFO_SEMINTERRATO")) Then
                        Me.sSeminterrato = CStr(drDati("INFO_SEMINTERRATO"))
                    End If
                    If Not IsDBNull(drDati("INFO_SOTTOTETTO")) Then
                        Me.sSottotetto = CStr(drDati("INFO_SOTTOTETTO"))
                    End If
                    If Not IsDBNull(drDati("CAT_CATASTALE_TEORICA")) Then
                        Me.sCategoria = CStr(drDati("CAT_CATASTALE_TEORICA"))
                    End If
                    If Not IsDBNull(drDati("FLAG_CONDOMINIO")) Then
                        If CInt(drDati("FLAG_CONDOMINIO")) = 0 Then
                            Me.iCondominio = 0
                        Else
                            Me.iCondominio = 1
                        End If
                    End If
                    If Not IsDBNull(drDati("FLAG_PRESENZA_CORTILE")) Then
                        If CInt(drDati("FLAG_PRESENZA_CORTILE")) = 0 Then
                            Me.iCortile = 0
                        Else
                            Me.iCortile = 1
                        End If
                    End If
                    If Not IsDBNull(drDati("NUM_CAMPANELLI")) Then
                        Me.iCampanelli = CInt(drDati("NUM_CAMPANELLI"))
                    End If
                    If Not IsDBNull(drDati("NUM_ALLOGGI")) Then
                        Me.iAlloggi = CInt(drDati("NUM_ALLOGGI"))
                    End If
                    If Not IsDBNull(drDati("NUM_PIANI")) Then
                        Me.iPiani = CInt(drDati("NUM_PIANI"))
                    End If
                    If Not IsDBNull(drDati("NUM_NEGOZI")) Then
                        Me.iNegozi = CInt(drDati("NUM_NEGOZI"))
                    End If
                    If Not IsDBNull(drDati("NUM_UFFICI")) Then
                        Me.iNuffici = CInt(drDati("NUM_UFFICI"))
                    End If
                    If Not IsDBNull(drDati("NUM_DEPOSITI")) Then
                        Me.iDepositi = CInt(drDati("NUM_DEPOSITI"))
                    End If
                    If Not IsDBNull(drDati("NUM_LABORATORI")) Then
                        Me.iLaboratori = CInt(drDati("NUM_LABORATORI"))
                    End If
                    If Not IsDBNull(drDati("NUM_AUTORIMESSE")) Then
                        Me.iAutorimesse = CInt(drDati("NUM_AUTORIMESSE"))
                    End If
                    If Not IsDBNull(drDati("NUM_TETTOIE")) Then
                        Me.iTettoie = CInt(drDati("NUM_TETTOIE"))
                    End If
                    If Not IsDBNull(drDati("NOTE")) Then
                        Me.sNote = CStr(drDati("NOTE"))
                    End If
                    If Not IsDBNull(drDati("COD_FABBRICATO")) Then
                        Me.nIdFabbricato = CInt(drDati("COD_FABBRICATO"))
                    End If
                    If Not IsDBNull(drDati("COD_CONDOMINIO")) Then
                        Me.nIdCondominio = CInt(drDati("COD_CONDOMINIO"))
                    End If
                    If Not IsDBNull(drDati("NUM_RESIDENTI")) Then
                        Me.nResidenti = CInt(drDati("NUM_RESIDENTI"))
                    End If
                    If Not IsDBNull(drDati("ID_UTENZA")) Then
                        Me.nUtenza = CInt(drDati("ID_UTENZA"))
                    End If
                    If Not IsDBNull(drDati("ID_REFERENTE")) Then
                        Me.nReferente = CInt(drDati("ID_REFERENTE"))
                    End If
                    If Not IsDBNull(drDati("STATO_FABBRICATO")) Then
                        Me.sStato = CStr(drDati("STATO_FABBRICATO"))
                    End If
                    If Not IsDBNull(drDati("COD_LINK")) Then
                        Me.nLink = CInt(drDati("COD_LINK"))
                    End If
                    If Not IsDBNull(drDati("COD_STATO_CONSERVAZIONE")) Then
                        Me.nIdConservazione = CInt(drDati("COD_STATO_CONSERVAZIONE"))
                    End If
                    If Not IsDBNull(drDati("DATI_AMMINISTRATORE")) Then
                        Me.sDatiAmministratore = CStr(drDati("DATI_AMMINISTRATORE"))
                    End If
                    If Not IsDBNull(drDati("VIE_NUMERI_CIVICI")) Then
                        Me.sVieCiviciCondominio = CStr(drDati("VIE_NUMERI_CIVICI"))
                    End If
                Loop
            End If
        Catch ex As Exception
            Log.Debug("ObjStradario::New::Si è verificato il seguente errore::" & ex.Message)
        End Try
    End Sub
End Class
