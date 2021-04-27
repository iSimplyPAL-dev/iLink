Namespace TabelleDiDecodifica
    Public Class DecEnum
        Public Enum DBOperation
            DB_INSERT = 1
            DB_UPDATE = 0
        End Enum
    End Class

    Public Class DetailTipiUtenza
        ''' <summary>
        ''' Definizione della tipologia delle utenze da associare al contatore
        ''' </summary>
        ''' <remarks></remarks>
        Private m_lngIDTipiUtenza As Long = 0
        Private m_Descrizione As String = ""
        Private m_ConsumoMinimoAnnuo As String = ""
        Private m_SogliaPositiva As String = ""
        Private m_SogliaNegativa As String = ""
        Private m_CodiceEsterno As String = ""
        Private m_Note As String = ""
        Private m_Dal As String = ""
        Private m_Al As String = ""

        Public Property IDTipiUtenza() As Long

            Get
                Return m_lngIDTipiUtenza
            End Get
            Set(ByVal Value As Long)
                m_lngIDTipiUtenza = Value
            End Set

        End Property

        Public Property Dal() As String
            Get
                Return m_Dal
            End Get
            Set(ByVal Value As String)
                m_Dal = Value
            End Set
        End Property
        Public Property Al() As String
            Get
                Return m_Al
            End Get
            Set(ByVal Value As String)
                m_Al = Value
            End Set
        End Property

        Public Property Descrizione() As String
            Get
                Return m_Descrizione
            End Get
            Set(ByVal Value As String)
                m_Descrizione = Value
            End Set
        End Property

        Public Property CodiceEsterno() As String
            Get
                Return m_CodiceEsterno
            End Get
            Set(ByVal Value As String)
                m_CodiceEsterno = Value
            End Set
        End Property

        Public Property ConsumoMinimoAnnuo() As String
            Get
                Return m_ConsumoMinimoAnnuo
            End Get
            Set(ByVal Value As String)
                m_ConsumoMinimoAnnuo = Value
            End Set
        End Property


        Public Property SogliaPositiva() As String
            Get
                Return m_SogliaPositiva
            End Get
            Set(ByVal Value As String)
                m_SogliaPositiva = Value
            End Set
        End Property

        Public Property SogliaNegativa() As String
            Get
                Return m_SogliaNegativa
            End Get
            Set(ByVal Value As String)
                m_SogliaNegativa = Value
            End Set
        End Property

        Public Property Note() As String
            Get
                Return m_Note
            End Get
            Set(ByVal Value As String)
                m_Note = Value
            End Set
        End Property

    End Class

    Public Class DiametroContatore
        ''' <summary>
        ''' Definizione del diametro contatore da associare al contatore
        ''' </summary>
        ''' <remarks></remarks>
        Public m_lngCodDiametroContatore As Long
        Public m_lngCodTariffaContatore As Long
        Public m_Descrizione As String
        Public m_DiametroContatore As String
        Public m_Note As String
        Public m_Prevalente As Boolean
        Public m_CodiceISTAT As String
        Public m_dsImporto As DataSet

        Public Property CodiceISTAT() As String
            Get
                Return m_CodiceISTAT
            End Get
            Set(ByVal Value As String)
                m_CodiceISTAT = Value
            End Set
        End Property
        Public Property CodDiametroContatore() As Long
            Get
                Return m_lngCodDiametroContatore
            End Get
            Set(ByVal Value As Long)
                m_lngCodDiametroContatore = Value
            End Set
        End Property

        Public Property CodTariffaContatore() As Long
            Get
                Return m_lngCodTariffaContatore
            End Get
            Set(ByVal Value As Long)
                m_lngCodTariffaContatore = Value
            End Set
        End Property
        Public Property Descrizione() As String
            Get
                Return m_Descrizione
            End Get
            Set(ByVal Value As String)
                m_Descrizione = Value
            End Set
        End Property

        Public Property DiametroContatore() As String
            Get
                Return m_DiametroContatore
            End Get
            Set(ByVal Value As String)
                m_DiametroContatore = Value
            End Set
        End Property

        Public Property Note() As String
            Get
                Return m_Note
            End Get
            Set(ByVal Value As String)
                m_Note = Value
            End Set
        End Property

        Public Property Importo() As DataSet
            Get
                Return m_dsImporto
            End Get
            Set(ByVal Value As DataSet)
                m_dsImporto = Value
            End Set
        End Property

        Public Property Prevalente() As Boolean
            Get
                Return m_Prevalente
            End Get
            Set(ByVal Value As Boolean)
                m_Prevalente = Value
            End Set
        End Property

    End Class

    Public Class DiametroPresa
        ''' <summary>
        ''' Definizione del diametro della presa da associare al contatore
        ''' </summary>
        ''' <remarks></remarks>
        Public m_lngCodDiametroPresa As Long
        Public m_lngCodTariffaContatore As Long
        Public m_Descrizione As String
        Public m_DiametroPresa As String
        Public m_Note As String
        Public m_dsImporto As DataSet

        Public Property CodDiametroPresa() As Long
            Get
                Return m_lngCodDiametroPresa
            End Get
            Set(ByVal Value As Long)
                m_lngCodDiametroPresa = Value
            End Set
        End Property

        Public Property CodTariffaContatore() As Long
            Get
                Return m_lngCodTariffaContatore
            End Get
            Set(ByVal Value As Long)
                m_lngCodTariffaContatore = Value
            End Set
        End Property

        Public Property Descrizione() As String
            Get
                Return m_Descrizione
            End Get
            Set(ByVal Value As String)
                m_Descrizione = Value
            End Set
        End Property

        Public Property DiametroPresa() As String
            Get
                Return m_DiametroPresa
            End Get
            Set(ByVal Value As String)
                m_DiametroPresa = Value
            End Set
        End Property

        Public Property Note() As String
            Get
                Return m_Note
            End Get
            Set(ByVal Value As String)
                m_Note = Value
            End Set
        End Property

        Public Property Importo() As DataSet
            Get
                Return m_dsImporto
            End Get
            Set(ByVal Value As DataSet)
                m_dsImporto = Value
            End Set
        End Property

    End Class

    Public Class Impianti
        ''' <summary>
        ''' Definizione dell'impianto da associare al contatore
        ''' </summary>
        ''' <remarks></remarks>
        Public m_lngIDImpianto As Long
        Public m_CodiceImpianto As String
        Public m_Descrizione As String
        Public m_Note As String

        Public Property IDImpianto() As Long
            Get
                Return m_lngIDImpianto
            End Get
            Set(ByVal Value As Long)
                m_lngIDImpianto = Value
            End Set
        End Property

        Public Property CodiceImpianto() As String
            Get
                Return m_CodiceImpianto
            End Get
            Set(ByVal Value As String)
                m_CodiceImpianto = Value
            End Set
        End Property

        Public Property Descrizione() As String
            Get
                Return m_Descrizione
            End Get
            Set(ByVal Value As String)
                m_Descrizione = Value
            End Set
        End Property

        Public Property Note() As String
            Get
                Return m_Note
            End Get
            Set(ByVal Value As String)
                m_Note = Value
            End Set
        End Property
    End Class

    Public Class Giri
        ''' <summary>
        ''' Definizione del giro di lettura da associare al contatore
        ''' </summary>
        ''' <remarks></remarks>
        Public m_lngIDGIRO As Long
        Public m_lngCODENTE As Long
        Public m_Descrizione As String
        Public m_CodiceGiro As String
        Public m_Note As String

        Public Property IDGIRO() As Long
            Get
                Return m_lngIDGIRO
            End Get
            Set(ByVal Value As Long)
                m_lngIDGIRO = Value
            End Set
        End Property

        Public Property CODENTE() As Long
            Get
                Return m_lngCODENTE
            End Get
            Set(ByVal Value As Long)
                m_lngCODENTE = Value
            End Set
        End Property

        Public Property Descrizione() As String
            Get
                Return m_Descrizione
            End Get
            Set(ByVal Value As String)
                m_Descrizione = Value
            End Set
        End Property
        Public Property CodiceGiro() As String
            Get
                Return m_CodiceGiro
            End Get
            Set(ByVal Value As String)
                m_CodiceGiro = Value
            End Set
        End Property
        Public Property Note() As String
            Get
                Return m_Note
            End Get
            Set(ByVal Value As String)
                m_Note = Value
            End Set
        End Property

    End Class

    Public Class PosizioneContatore
        ''' <summary>
        ''' Definizione della posizione fisica da associare al contatore
        ''' </summary>
        ''' <remarks></remarks>
        Public m_lngCODPOSIZIONE As Long
        Public m_lngPosizione As Long
        Public m_Descrizione As String
        Public m_Note As String

        Public Property CODPOSIZIONE() As Long
            Get
                Return m_lngCODPOSIZIONE
            End Get
            Set(ByVal Value As Long)
                m_lngCODPOSIZIONE = Value
            End Set
        End Property

        Public Property Posizione() As Long
            Get
                Return m_lngPosizione
            End Get
            Set(ByVal Value As Long)
                m_lngPosizione = Value
            End Set
        End Property

        Public Property Descrizione() As String
            Get
                Return m_Descrizione
            End Get
            Set(ByVal Value As String)
                m_Descrizione = Value
            End Set
        End Property

        Public Property Note() As String
            Get
                Return m_Note
            End Get
            Set(ByVal Value As String)
                m_Note = Value
            End Set
        End Property
    End Class

    Public Class TipoContatore
        ''' <summary>
        ''' Definizione della tipologia del contatore
        ''' </summary>
        ''' <remarks></remarks>
        Public m_lngIDTIPOCONTATORE As Long
        Public m_lngFondoScala As Long
        Public m_Descrizione As String
        Public m_Note As String

        Public Property IDTIPOCONTATORE() As Long
            Get
                Return m_lngIDTIPOCONTATORE
            End Get
            Set(ByVal Value As Long)
                m_lngIDTIPOCONTATORE = Value
            End Set
        End Property

        Public Property FondoScala() As Long
            Get
                Return m_lngFondoScala
            End Get
            Set(ByVal Value As Long)
                m_lngFondoScala = Value
            End Set
        End Property

        Public Property Descrizione() As String
            Get
                Return m_Descrizione
            End Get
            Set(ByVal Value As String)
                m_Descrizione = Value
            End Set
        End Property

        Public Property Note() As String
            Get
                Return m_Note
            End Get
            Set(ByVal Value As String)
                m_Note = Value
            End Set
        End Property
    End Class

    Public Class DetailAnomalie
        ''' <summary>
        ''' Definizione dell'anomalia da associare alla lettura
        ''' </summary>
        ''' <remarks></remarks>
        Public m_lngCodAnomalia As Long
        Public m_Descrizione As String
        Public m_CodiceAnomalia As String
        Public m_Note As String

        Public Property CodAnomalia() As Long
            Get
                Return m_lngCodAnomalia
            End Get
            Set(ByVal Value As Long)
                m_lngCodAnomalia = Value
            End Set
        End Property


        Public Property Descrizione() As String
            Get
                Return m_Descrizione
            End Get
            Set(ByVal Value As String)
                m_Descrizione = Value
            End Set
        End Property

        Public Property CodiceAnomalia() As String
            Get
                Return m_CodiceAnomalia
            End Get
            Set(ByVal Value As String)
                m_CodiceAnomalia = Value
            End Set
        End Property


        Public Property Note() As String
            Get
                Return m_Note
            End Get
            Set(ByVal Value As String)
                m_Note = Value
            End Set
        End Property
    End Class

    Public Class DetailIVA
        ''' <summary>
        ''' Definizione del tipo di IVA da associare al contatore
        ''' </summary>
        ''' <remarks></remarks>
        Public m_lngCodIVA As Long
        Public m_Descrizione As String
        Public m_CodiceIVA As String
        Public m_Note As String

        Public Property CodIVA() As Long
            Get
                Return m_lngCodIVA
            End Get
            Set(ByVal Value As Long)
                m_lngCodIVA = Value
            End Set
        End Property


        Public Property Descrizione() As String
            Get
                Return m_Descrizione
            End Get
            Set(ByVal Value As String)
                m_Descrizione = Value
            End Set
        End Property

        Public Property CodiceIVA() As String
            Get
                Return m_CodiceIVA
            End Get
            Set(ByVal Value As String)
                m_CodiceIVA = Value
            End Set
        End Property


        Public Property Note() As String
            Get
                Return m_Note
            End Get
            Set(ByVal Value As String)
                m_Note = Value
            End Set
        End Property

    End Class

    Public Class DetailPeriodo
        ''' <summary>
        ''' Definizione del periodo di lavorazione
        ''' </summary>
        ''' <remarks></remarks>
        Private m_CodPeriodo As Long
        Private m_Periodo As String
        Private m_DaData As String
        Private m_AData As String
        Private m_Note As String
        Private m_Storico As Boolean
        Private m_Attuale As Boolean
        Private _nTipoArrotondamentoConsumo As Integer

        Public Property CodPeriodo() As Long
            Get
                Return m_CodPeriodo
            End Get
            Set(ByVal Value As Long)
                m_CodPeriodo = Value
            End Set
        End Property
        Public Property Periodo() As String
            Get
                Return m_Periodo
            End Get
            Set(ByVal Value As String)
                m_Periodo = Value
            End Set
        End Property
        Public Property Storico() As Boolean
            Get
                Return m_Storico
            End Get
            Set(ByVal Value As Boolean)
                m_Storico = Value
            End Set
        End Property
        Public Property Attuale() As Boolean
            Get
                Return m_Attuale
            End Get
            Set(ByVal Value As Boolean)
                m_Attuale = Value
            End Set
        End Property
        Public Property DaData() As String
            Get
                Return m_DaData
            End Get
            Set(ByVal Value As String)
                m_DaData = Value
            End Set
        End Property
        Public Property AData() As String
            Get
                Return m_AData
            End Get
            Set(ByVal Value As String)
                m_AData = Value
            End Set
        End Property
        Public Property Note() As String
            Get
                Return m_Note
            End Get
            Set(ByVal Value As String)
                m_Note = Value
            End Set
        End Property
        Public Property nTipoArrotondamentoConsumo() As Integer
            Get
                Return _nTipoArrotondamentoConsumo
            End Get
            Set(ByVal Value As Integer)
                _nTipoArrotondamentoConsumo = Value
            End Set
        End Property
    End Class

    Public Class MinimiFatturabili
        ''' <summary>
        ''' Definizione minimo fatturabile per contatore
        ''' </summary>
        ''' <remarks></remarks>
        Public m_lngIDMINIMO As Long
        Public m_lngIDTipoUtenza As Long
        Public m_Descrizione As String
        Public m_Minimo As String
        Public m_dsTipoUtenza As DataSet
        Public m_Note As String

        Public Property IDMinimo() As Long
            Get
                Return m_lngIDMINIMO
            End Get
            Set(ByVal Value As Long)
                m_lngIDMINIMO = Value
            End Set
        End Property

        Public Property TipoUtenza() As Long
            Get
                Return m_lngIDTipoUtenza
            End Get
            Set(ByVal Value As Long)
                m_lngIDTipoUtenza = Value
            End Set
        End Property

        Public Property Descrizione() As String
            Get
                Return m_Descrizione
            End Get
            Set(ByVal Value As String)
                m_Descrizione = Value
            End Set
        End Property

        Public Property Minimo() As String
            Get
                Return m_Minimo
            End Get
            Set(ByVal Value As String)
                m_Minimo = Value
            End Set
        End Property

        Public Property dsTipoUtenza() As DataSet
            Get
                Return m_dsTipoUtenza
            End Get
            Set(ByVal Value As DataSet)
                m_dsTipoUtenza = Value
            End Set
        End Property

        Public Property Note() As String
            Get
                Return m_Note
            End Get
            Set(ByVal Value As String)
                m_Note = Value
            End Set
        End Property

    End Class

    Public Class DetailAttivita
        ''' <summary>
        ''' Definizione delle tipologie di attività da associare ad un contatore
        ''' </summary>
        ''' <remarks></remarks>
        Public m_lngIDAttivita As Long
        Public m_Descrizione As String
        Public m_CodiceAttivita As String
        Public m_Note As String

        Public Property IDAttivita() As Long
            Get
                Return m_lngIDAttivita
            End Get
            Set(ByVal Value As Long)
                m_lngIDAttivita = Value
            End Set
        End Property


        Public Property Descrizione() As String
            Get
                Return m_Descrizione
            End Get
            Set(ByVal Value As String)
                m_Descrizione = Value
            End Set
        End Property

        Public Property CodiceAttivita() As String
            Get
                Return m_CodiceAttivita
            End Get
            Set(ByVal Value As String)
                m_CodiceAttivita = Value
            End Set
        End Property


        Public Property Note() As String
            Get
                Return m_Note
            End Get
            Set(ByVal Value As String)
                m_Note = Value
            End Set
        End Property

    End Class

    'Public Class DetailTitoloSoggetti
    '    Public m_lngIDTitoloSoggetto As Long
    '    Public m_Descrizione As String
    '    Public m_Note As String

    '    Public Property IDTitoloSoggetto() As Long
    '        Get
    '            Return m_lngIDTitoloSoggetto
    '        End Get
    '        Set(ByVal Value As Long)
    '            m_lngIDTitoloSoggetto = Value
    '        End Set
    '    End Property

    '    Public Property Descrizione() As String
    '        Get
    '            Return m_Descrizione
    '        End Get
    '        Set(ByVal Value As String)
    '            m_Descrizione = Value
    '        End Set
    '    End Property

    '    Public Property Note() As String
    '        Get
    '            Return m_Note
    '        End Get
    '        Set(ByVal Value As String)
    '            m_Note = Value
    '        End Set
    '    End Property

    'End Class
    'Public Class DetailFognatura
    '    Public m_lngCodFognatura As Long
    '    Public m_Descrizione As String
    '    Public m_CodiceFognatura As String

    '    Public m_Note As String

    '    Public Property CodFognatura() As Long
    '        Get
    '            Return m_lngCodFognatura
    '        End Get
    '        Set(ByVal Value As Long)
    '            m_lngCodFognatura = Value
    '        End Set
    '    End Property


    '    Public Property Descrizione() As String
    '        Get
    '            Return m_Descrizione
    '        End Get
    '        Set(ByVal Value As String)
    '            m_Descrizione = Value
    '        End Set
    '    End Property

    '    Public Property CodiceFognatura() As String
    '        Get
    '            Return m_CodiceFognatura
    '        End Get
    '        Set(ByVal Value As String)
    '            m_CodiceFognatura = Value
    '        End Set
    '    End Property


    '    Public Property Note() As String
    '        Get
    '            Return m_Note
    '        End Get
    '        Set(ByVal Value As String)
    '            m_Note = Value
    '        End Set
    '    End Property
    'End Class
    'Public Class DetailDepurazione
    '    Public m_lngCodDepurazione As Long
    '    Public m_Descrizione As String
    '    Public m_CodiceDepurazione As String

    '    Public m_Note As String

    '    Public Property CodDepurazione() As Long
    '        Get
    '            Return m_lngCodDepurazione
    '        End Get
    '        Set(ByVal Value As Long)
    '            m_lngCodDepurazione = Value
    '        End Set
    '    End Property


    '    Public Property Descrizione() As String
    '        Get
    '            Return m_Descrizione
    '        End Get
    '        Set(ByVal Value As String)
    '            m_Descrizione = Value
    '        End Set
    '    End Property

    '    Public Property CodiceDepurazione() As String
    '        Get
    '            Return m_CodiceDepurazione
    '        End Get
    '        Set(ByVal Value As String)
    '            m_CodiceDepurazione = Value
    '        End Set
    '    End Property


    '    Public Property Note() As String
    '        Get
    '            Return m_Note
    '        End Get
    '        Set(ByVal Value As String)
    '            m_Note = Value
    '        End Set
    '    End Property
    'End Class
    'Public Class TariffeContatore

    '    Public m_lngCODTARIFFACONTATORE As Long
    '    Public m_TariffaContatore As Double
    '    Public m_Descrizione As String
    '    Public m_Note As String

    '    Public Property CODTARIFFACONTATORE() As Long
    '        Get
    '            Return m_lngCODTARIFFACONTATORE
    '        End Get
    '        Set(ByVal Value As Long)
    '            m_lngCODTARIFFACONTATORE = Value
    '        End Set
    '    End Property

    '    Public Property TariffaContatore() As Double
    '        Get
    '            Return m_TariffaContatore
    '        End Get
    '        Set(ByVal Value As Double)
    '            m_TariffaContatore = Value
    '        End Set
    '    End Property

    '    Public Property Descrizione() As String
    '        Get
    '            Return m_Descrizione
    '        End Get
    '        Set(ByVal Value As String)
    '            m_Descrizione = Value
    '        End Set
    '    End Property

    '    Public Property Note() As String
    '        Get
    '            Return m_Note
    '        End Get
    '        Set(ByVal Value As String)
    '            m_Note = Value
    '        End Set
    '    End Property

    'End Class
    'Public Class Stradario

    '    Public m_lngCODstrada As Long
    '    Public m_CodiceStrada As String
    '    Public m_TipoStrada As String
    '    Public m_Strada As String
    '    Public m_CodiceCitta As String
    '    Public m_CodComune As String
    '    Public m_CodiceISTAT As String
    '    Public m_dsTipoStrada As DataSet



    '    Public Property CODstrada() As Long
    '        Get
    '            Return m_lngCODstrada
    '        End Get
    '        Set(ByVal Value As Long)
    '            m_lngCODstrada = Value
    '        End Set
    '    End Property

    '    Public Property CodiceStrada() As String
    '        Get
    '            Return m_CodiceStrada
    '        End Get
    '        Set(ByVal Value As String)
    '            m_CodiceStrada = Value
    '        End Set
    '    End Property

    '    Public Property CodiceCitta() As String
    '        Get
    '            Return m_CodiceCitta
    '        End Get
    '        Set(ByVal Value As String)
    '            m_CodiceCitta = Value
    '        End Set
    '    End Property

    '    Public Property TipoStrada() As String
    '        Get
    '            Return m_TipoStrada
    '        End Get
    '        Set(ByVal Value As String)
    '            m_TipoStrada = Value
    '        End Set
    '    End Property

    '    Public Property Strada() As String
    '        Get
    '            Return m_Strada
    '        End Get
    '        Set(ByVal Value As String)
    '            m_Strada = Value
    '        End Set
    '    End Property

    '    Public Property CodComune() As String
    '        Get
    '            Return m_CodComune
    '        End Get
    '        Set(ByVal Value As String)
    '            m_CodComune = Value
    '        End Set
    '    End Property

    '    Public Property CodiceISTAT() As String
    '        Get
    '            Return m_CodiceISTAT
    '        End Get
    '        Set(ByVal Value As String)
    '            m_CodiceISTAT = Value
    '        End Set
    '    End Property

    '    Public Property dsTipoStrada() As DataSet
    '        Get
    '            Return m_dsTipoStrada
    '        End Get
    '        Set(ByVal Value As DataSet)
    '            m_dsTipoStrada = Value
    '        End Set
    '    End Property

    'End Class
    'Public Class TIPO_STRADA
    '    Public m_Tipo_Strada As String
    '    Public m_CODTipo_Strada As String
    '    Public Property Tipo_Strada() As String
    '        Get
    '            Return m_Tipo_Strada
    '        End Get
    '        Set(ByVal Value As String)
    '            m_Tipo_Strada = Value
    '        End Set
    '    End Property

    '    Public Property CODTipo_Strada() As String
    '        Get
    '            Return m_CODTipo_Strada
    '        End Get
    '        Set(ByVal Value As String)
    '            m_CODTipo_Strada = Value
    '        End Set
    '    End Property
    'End Class
    'Public Class DetailCodiciCitta


    '    Public m_Comune As String
    '    Public m_Cap As String
    '    Public m_Provincia As String
    '    Public m_CodiceCitta As String
    '    Public m_Note As String

    '    Public Property Comune() As String
    '        Get
    '            Return m_Comune
    '        End Get
    '        Set(ByVal Value As String)
    '            m_Comune = Value
    '        End Set
    '    End Property
    '    Public Property Cap() As String
    '        Get
    '            Return m_Cap
    '        End Get
    '        Set(ByVal Value As String)
    '            m_Cap = Value
    '        End Set
    '    End Property
    '    Public Property Provincia() As String
    '        Get
    '            Return m_Provincia
    '        End Get
    '        Set(ByVal Value As String)
    '            m_Provincia = Value
    '        End Set
    '    End Property

    '    Public Property Codice_Citta() As String
    '        Get
    '            Return m_CodiceCitta
    '        End Get
    '        Set(ByVal Value As String)
    '            m_CodiceCitta = Value
    '        End Set
    '    End Property

    '    Public Property Note() As String
    '        Get
    '            Return m_Note
    '        End Get
    '        Set(ByVal Value As String)
    '            m_Note = Value
    '        End Set
    '    End Property

    'End Class
End Namespace