Imports log4net
Imports Utility

#Region "Oggetti per Agenzia Entrate"
Public Class ObjAEDatiMancanti
    Private Shared Log As ILog = LogManager.GetLogger("ObjAEDatiMancanti")
    Private _nIdContribuente As Integer = -1
    Private _nIdArticolo As Integer = -1
    Private _sAnno As String = ""
    Private _sCognome As String = ""
    Private _sNome As String = ""
    Private _sCfPIva As String = ""
    Private _sViaRes As String = ""
    Private _sCivicoRes As String = ""
    Private _sCapRes As String = ""
    Private _sComuneRes As String = ""
    Private _sPvRes As String = ""
    Private _sNominativoCO As String = ""
    Private _sViaCO As String = ""
    Private _sCivicoCO As String = ""
    Private _sCapCO As String = ""
    Private _sComuneCO As String = ""
    Private _sPvCO As String = ""
    Private _sIndirizzoImmo As String = ""
    Private _sFoglio As String = ""
    Private _sParticella As String = ""
    Private _sEstParticella As String = ""
    Private _sDataInizio As String = ""
    Private _sDescrTitOccup As String = ""
    Private _sDescrNatOccup As String = ""
    Private _sDescrDestUso As String = ""
    Private _sDescrTipoUnita As String = ""
    Private _sDescrTipoParticella As String = ""
    Private _sDescrAnomalia As String = ""

    Public Property nIdContribuente() As Integer
        Get
            Return _nIdContribuente
        End Get
        Set(ByVal Value As Integer)
            _nIdContribuente = Value
        End Set
    End Property
    Public Property nIdArticolo() As Integer
        Get
            Return _nIdArticolo
        End Get
        Set(ByVal Value As Integer)
            _nIdArticolo = Value
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
    Public Property sCfPIva() As String
        Get
            Return _sCfPIva
        End Get
        Set(ByVal Value As String)
            _sCfPIva = Value
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
    Public Property sCapRes() As String
        Get
            Return _sCapRes
        End Get
        Set(ByVal Value As String)
            _sCapRes = Value
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
    Public Property sPvRes() As String
        Get
            Return _sPvRes
        End Get
        Set(ByVal Value As String)
            _sPvRes = Value
        End Set
    End Property
    Public Property sNominativoCO() As String
        Get
            Return _sNominativoCO
        End Get
        Set(ByVal Value As String)
            _sNominativoCO = Value
        End Set
    End Property
    Public Property sViaCO() As String
        Get
            Return _sViaCO
        End Get
        Set(ByVal Value As String)
            _sViaCO = Value
        End Set
    End Property
    Public Property sCivicoCO() As String
        Get
            Return _sCivicoCO
        End Get
        Set(ByVal Value As String)
            _sCivicoCO = Value
        End Set
    End Property
    Public Property sCapCO() As String
        Get
            Return _sCapCO
        End Get
        Set(ByVal Value As String)
            _sCapCO = Value
        End Set
    End Property
    Public Property sComuneCO() As String
        Get
            Return _sComuneCO
        End Get
        Set(ByVal Value As String)
            _sComuneCO = Value
        End Set
    End Property
    Public Property sPvCO() As String
        Get
            Return _sPvCO
        End Get
        Set(ByVal Value As String)
            _sPvCO = Value
        End Set
    End Property
    Public Property sIndirizzoImmo() As String
        Get
            Return _sIndirizzoImmo
        End Get
        Set(ByVal Value As String)
            _sIndirizzoImmo = Value
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
    Public Property sParticella() As String
        Get
            Return _sParticella
        End Get
        Set(ByVal Value As String)
            _sParticella = Value
        End Set
    End Property
    Public Property sEstParticella() As String
        Get
            Return _sEstParticella
        End Get
        Set(ByVal Value As String)
            _sEstParticella = Value
        End Set
    End Property
    Public Property sDataInizio() As String
        Get
            Return _sDataInizio
        End Get
        Set(ByVal Value As String)
            _sDataInizio = Value
        End Set
    End Property
    Public Property sDescrTitOccup() As String
        Get
            Return _sDescrTitOccup
        End Get
        Set(ByVal Value As String)
            _sDescrTitOccup = Value
        End Set
    End Property
    Public Property sDescrNatOccup() As String
        Get
            Return _sDescrNatOccup
        End Get
        Set(ByVal Value As String)
            _sDescrNatOccup = Value
        End Set
    End Property
    Public Property sDescrDestUso() As String
        Get
            Return _sDescrDestUso
        End Get
        Set(ByVal Value As String)
            _sDescrDestUso = Value
        End Set
    End Property
    Public Property sDescrTipoUnita() As String
        Get
            Return _sDescrTipoUnita
        End Get
        Set(ByVal Value As String)
            _sDescrTipoUnita = Value
        End Set
    End Property
    Public Property sDescrTipoParticella() As String
        Get
            Return _sDescrTipoParticella
        End Get
        Set(ByVal Value As String)
            _sDescrTipoParticella = Value
        End Set
    End Property
    Public Property sDescrAnomalia() As String
        Get
            Return _sDescrAnomalia
        End Get
        Set(ByVal Value As String)
            _sDescrAnomalia = Value
        End Set
    End Property
End Class
#End Region

Public Class ClsAEDatiMancanti
    Private Shared Log As ILog = LogManager.GetLogger("ClsAEDatiMancanti")
    Private FncDB As New ClsAELevelDB
    Private oReplace As New ClsGenerale.Generale
    Private FncGen As New ClsGenerale.Generale

    Public Sub LoadComboAnnoPeriodi(ByVal ddl As DropDownList, ByVal sEnte As String)
        Dim sSQL As String

        sSQL = "SELECT YEAR(TP_PERIODO.DADATA) AS ANNO, YEAR(TP_PERIODO.DADATA) AS IDANNO"
        sSQL += " FROM TP_PERIODO"
        sSQL += " WHERE (TP_PERIODO.COD_ENTE='" & sEnte & "')"
        sSQL += " UNION"
        sSQL += " SELECT YEAR(TP_PERIODO.DADATA) AS ANNO, YEAR(TP_PERIODO.DADATA) AS IDANNO"
        sSQL += " FROM TP_PERIODO"
        sSQL += " WHERE (TP_PERIODO.COD_ENTE='" & sEnte & "')"
        sSQL += " ORDER BY ANNO DESC"
        FncGen.LoadComboGenerale(ddl, sSQL)
    End Sub

    Public Function GetListDatiMancanti(ByVal sCodEnte As String, ByVal sAnno As String, ByVal sCognome As String, ByVal sNome As String, ByVal nTypeMancanti As Integer) As ObjAEDatiMancanti()
        Try
            Dim DvDati As DataView
            Dim x As Integer
            Dim nDaInserire As Integer = 0
            Dim nList As Integer = -1
            Dim oMyMancante As ObjAEDatiMancanti
            Dim oMyListMancanti() As ObjAEDatiMancanti
            oMyListMancanti = Nothing

            'popolo il dataview dei mancanti anagrafici richiamando la funzione GetDatiMancanti(sAnno, sCognome, sNome, nTypeMancanti);
            DvDati = FncDB.GetDatiMancanti(sCodEnte, sAnno, sCognome, sNome, nTypeMancanti)
            For x = 0 To DvDati.Count - 1
                'ciclo sul dataview totale;
                If CInt(DvDati.Item(x)("anomalia")) = 2 Then
                    'controllo i dati obbligatori per il 2009
                    If CStr(DvDati.Item(x)("anno")) >= "2009" Then
                        If Not IsDBNull(DvDati.Item(x)("foglio")) Or Not IsDBNull(DvDati.Item(x)("particella")) Then
                            If CStr(DvDati.Item(x)("foglio")) = "" Or CStr(DvDati.Item(x)("particella")) = "" Then
                                'se (FOGLIO o PARTICELLA sono vuoti)
                                nDaInserire = 1
                            Else
                                nDaInserire = 0
                            End If
                        Else
                            'se (FOGLIO o PARTICELLA sono vuoti)
                            nDaInserire = 1
                        End If
                    End If
                    'controllo gli altri dati
                    If nDaInserire = 0 Then
                        If IsDBNull(DvDati.Item(x)("titoccup")) Then
                            nDaInserire = 1
                        Else
                            If CStr(DvDati.Item(x)("titoccup")) = "-1" Then
                                nDaInserire = 1
                            End If
                        End If
                        If IsDBNull(DvDati.Item(x)("natoccup")) Then
                            nDaInserire = 1
                        Else
                            If CStr(DvDati.Item(x)("natoccup")) = "-1" Then
                                nDaInserire = 1
                            End If
                        End If
                        If IsDBNull(DvDati.Item(x)("tipounita")) Then
                            nDaInserire = 1
                        Else
                            If CStr(DvDati.Item(x)("tipounita")) = "" Then
                                nDaInserire = 1
                            End If
                        End If
                    End If
                Else
                    nDaInserire = 1
                End If
                If nDaInserire = 1 Then
                    'se(dainserireinarray = 1)
                    'incremento l'array di oggetti
                    nList += 1
                    'popolo il singolo oggetto con il record del dataview
                    oMyMancante = New ObjAEDatiMancanti
                    oMyMancante.nIdContribuente = CInt(DvDati.Item(x)("cod_contribuente"))
                    oMyMancante.nIdArticolo = CInt(DvDati.Item(x)("id"))
                    If Not IsDBNull(DvDati.Item(x)("anno")) Then oMyMancante.sAnno = CStr(DvDati.Item(x)("anno"))
                    If Not IsDBNull(DvDati.Item(x)("cognome")) Then oMyMancante.sCognome = CStr(DvDati.Item(x)("cognome"))
                    If Not IsDBNull(DvDati.Item(x)("nome")) Then oMyMancante.sNome = CStr(DvDati.Item(x)("nome"))
                    If Not IsDBNull(DvDati.Item(x)("cfpiva")) Then oMyMancante.sCfPIva = CStr(DvDati.Item(x)("cfpiva"))
                    If Not IsDBNull(DvDati.Item(x)("via_res")) Then oMyMancante.sViaRes = CStr(DvDati.Item(x)("via_res"))
                    If Not IsDBNull(DvDati.Item(x)("civico_res")) Then oMyMancante.sCivicoRes = CStr(DvDati.Item(x)("civico_res"))
                    If Not IsDBNull(DvDati.Item(x)("cap_res")) Then oMyMancante.sCapRes = CStr(DvDati.Item(x)("cap_res"))
                    If Not IsDBNull(DvDati.Item(x)("comune_res")) Then oMyMancante.sComuneRes = CStr(DvDati.Item(x)("comune_res"))
                    If Not IsDBNull(DvDati.Item(x)("provincia_res")) Then oMyMancante.sPvRes = CStr(DvDati.Item(x)("provincia_res"))
                    If Not IsDBNull(DvDati.Item(x)("nominativo")) Then oMyMancante.sNominativoCO = CStr(DvDati.Item(x)("nominativo"))
                    If Not IsDBNull(DvDati.Item(x)("via_rcp")) Then oMyMancante.sViaCO = CStr(DvDati.Item(x)("via_rcp"))
                    If Not IsDBNull(DvDati.Item(x)("civico_rcp")) Then oMyMancante.sCivicoCO = CStr(DvDati.Item(x)("civico_rcp"))
                    If Not IsDBNull(DvDati.Item(x)("cap_rcp")) Then oMyMancante.sCapCO = CStr(DvDati.Item(x)("cap_rcp"))
                    If Not IsDBNull(DvDati.Item(x)("comune_rcp")) Then oMyMancante.sComuneCO = CStr(DvDati.Item(x)("comune_rcp"))
                    If Not IsDBNull(DvDati.Item(x)("provincia_rcp")) Then oMyMancante.sPvCO = CStr(DvDati.Item(x)("provincia_rcp"))
                    If Not IsDBNull(DvDati.Item(x)("ind_immo")) Then oMyMancante.sIndirizzoImmo = CStr(DvDati.Item(x)("ind_immo"))
                    If Not IsDBNull(DvDati.Item(x)("foglio")) Then oMyMancante.sFoglio = CStr(DvDati.Item(x)("foglio"))
                    If Not IsDBNull(DvDati.Item(x)("particella")) Then oMyMancante.sParticella = CStr(DvDati.Item(x)("particella"))
                    If Not IsDBNull(DvDati.Item(x)("estensione_particella")) Then oMyMancante.sEstParticella = CStr(DvDati.Item(x)("estensione_particella"))
                    If Not IsDBNull(DvDati.Item(x)("data_inizio")) Then oMyMancante.sDataInizio = oReplace.GiraDataFromDB(CStr(DvDati.Item(x)("data_inizio")))
                    If Not IsDBNull(DvDati.Item(x)("titoccup")) Then oMyMancante.sDescrTitOccup = CStr(DvDati.Item(x)("titoccup"))
                    If Not IsDBNull(DvDati.Item(x)("natoccup")) Then oMyMancante.sDescrNatOccup = CStr(DvDati.Item(x)("natoccup"))
                    If Not IsDBNull(DvDati.Item(x)("tipounita")) Then oMyMancante.sDescrTipoUnita = CStr(DvDati.Item(x)("tipounita"))
                    If Not IsDBNull(DvDati.Item(x)("tipoparticella")) Then oMyMancante.sDescrTipoParticella = CStr(DvDati.Item(x)("tipoparticella"))
                    Select Case CInt(DvDati.Item(x)("anomalia"))
                        Case 1
                            oMyMancante.sDescrAnomalia = "Anagrafica Incompleta"
                        Case 2
                            oMyMancante.sDescrAnomalia = "Immobile Incompleto"
                        Case 3
                            oMyMancante.sDescrAnomalia = "Anagrafica e Immobile Incompleti"
                    End Select
                    'inserisco il singolo oggetto all’array di oggetti
                    ReDim Preserve oMyListMancanti(nList)
                    oMyListMancanti(nList) = oMyMancante
                End If
            Next
            Return oMyListMancanti
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsAEDatiMancanti.GetListDatiMancanti.errore: ", Err)
            Throw Err
        End Try
    End Function

    Public Function PrintDatiMancanti(ByVal oMyListMancanti() As ObjAEDatiMancanti) As DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim x As Integer

        Try
            'Carico il dataset con relative colonne;
            DsStampa.Tables.Add("STAMPA")
            For x = 0 To 18
                DsStampa.Tables("STAMPA").Columns.Add(Space(x))
            Next
            'inserisco l’intestazione del report tramite la funzione AddRowStampa(DtStampa, sDatiStampa);
            DtStampa = DsStampa.Tables("STAMPA")
            sDatiStampa = "Elenco Dati Mancanti"
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + 18, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota tramite la funzione AddRowStampa(DtStampa, “”);
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + 18, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'valorizzo le intestazioni di colonna ="Anno|Nominativo|Cod.Fiscale/P.Iva|Ubicazione Immobile|Tipo Anomalia"
            'inserisco le intestazioni di colonna tramite la funzione AddRowStampa(DtStampa, sDatiStampa);
            sDatiStampa = "Anno|Nominativo|Cod.Fiscale/P.Iva|Indirizzo Res.|Comune Res.|Nominativo Invio|Indirizzo Invio|Comune Invio|Ubicazione Immobile|Tipo Unità|Foglio|Particella|Est.Particella|Tipo Particella|Data Inizio Occup.|Titolo Occup.|Natura Occup.|Destinazione d'uso|Tipo Anomalia"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'ciclo sui dati da stampare
            For x = 0 To oMyListMancanti.GetUpperBound(0)
                'valorizzo il record da stampare e o inserisco tramite la funzione AddRowStampa(DtStampa, sDatiStampa);
                sDatiStampa = oMyListMancanti(x).sAnno
                sDatiStampa += "|" + oMyListMancanti(x).sCognome + " " + oMyListMancanti(x).sNome
                sDatiStampa += "|" + oMyListMancanti(x).sCfPIva
                sDatiStampa += "|" + oMyListMancanti(x).sViaRes + " " + oMyListMancanti(x).sCivicoRes
                sDatiStampa += "|" + oMyListMancanti(x).sCapRes + " " + oMyListMancanti(x).sComuneRes + " " + oMyListMancanti(x).sPvRes
                sDatiStampa += "|" + oMyListMancanti(x).sNominativoCO
                sDatiStampa += "|" + oMyListMancanti(x).sViaCO + " " + oMyListMancanti(x).sCivicoCO
                sDatiStampa += "|" + oMyListMancanti(x).sCapCO + " " + oMyListMancanti(x).sComuneCO + " " + oMyListMancanti(x).sPvCO
                sDatiStampa += "|" + oMyListMancanti(x).sIndirizzoImmo
                sDatiStampa += "|" + oMyListMancanti(x).sDescrTipoUnita
                sDatiStampa += "|" + oMyListMancanti(x).sFoglio
                sDatiStampa += "|" + oMyListMancanti(x).sParticella
                sDatiStampa += "|" + oMyListMancanti(x).sEstParticella
                sDatiStampa += "|" + oMyListMancanti(x).sDescrTipoParticella
                sDatiStampa += "|" + oMyListMancanti(x).sDataInizio
                sDatiStampa += "|" + oMyListMancanti(x).sDescrTitOccup
                sDatiStampa += "|" + oMyListMancanti(x).sDescrNatOccup
                sDatiStampa += "|" + oMyListMancanti(x).sDescrDestUso
                sDatiStampa += "|" + oMyListMancanti(x).sDescrAnomalia
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
            Next
            Return DtStampa
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsAEDatiMancanti.PrintDatiMancanti.errore: ", Err)

            Throw Err
        End Try
    End Function

    Private Function AddRowStampa(ByRef DtAddRow As DataTable, ByVal sValueRow As String) As Integer
        Dim sTextRow() As String
        Dim DrAddRow As DataRow
        Dim x As Integer = 0

        Try
            'aggiungo una nuova riga nel datarow
            DrAddRow = DtAddRow.NewRow
            'controllo se la riga e\' scritta
            If sValueRow <> "" Then
                sTextRow = sValueRow.Split(CChar("|"))
                For x = 0 To UBound(sTextRow)
                    'popolo la riga nel datarow
                    DrAddRow(x) = sTextRow(x)
                Next
            End If
            'aggiorno la riga al datatable
            DtAddRow.Rows.Add(DrAddRow)

            Return 1
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsAEDatiMancanti.AddRowStampa.errore: ", Err)
            Return 0
        End Try
    End Function

    'Public Function GetListDatiMancanti(ByVal sCodEnte As String, ByVal sAnno As String, ByVal sCognome As String, ByVal sNome As String, ByVal nTypeMancanti As Integer, ByVal WFSessione As OPENUtility.CreateSessione) As ObjAEDatiMancanti()
    '    Try
    '        Dim DvDati As DataView
    '        Dim x As Integer
    '        Dim nDaInserire As Integer = 0
    '        Dim nList As Integer = -1
    '        Dim oMyMancante As ObjAEDatiMancanti
    '        Dim oMyListMancanti() As ObjAEDatiMancanti

    '        'popolo il dataview dei mancanti anagrafici richiamando la funzione GetDatiMancanti(sAnno, sCognome, sNome, nTypeMancanti);
    '        DvDati = FncDB.GetDatiMancanti(sCodEnte, sAnno, sCognome, sNome, nTypeMancanti, WFSessione)
    '        For x = 0 To DvDati.Count - 1
    '            'ciclo sul dataview totale;
    '            If CInt(DvDati.Item(x)("anomalia")) = 2 Then
    '                'controllo i dati obbligatori per il 2009
    '                If CStr(DvDati.Item(x)("anno")) >= "2009" Then
    '                    If Not IsDBNull(DvDati.Item(x)("foglio")) Or Not IsDBNull(DvDati.Item(x)("particella")) Then
    '                        If CStr(DvDati.Item(x)("foglio")) = "" Or CStr(DvDati.Item(x)("particella")) = "" Then
    '                            'se (FOGLIO o PARTICELLA sono vuoti)
    '                            nDaInserire = 1
    '                        Else
    '                            nDaInserire = 0
    '                        End If
    '                    Else
    '                        'se (FOGLIO o PARTICELLA sono vuoti)
    '                        nDaInserire = 1
    '                    End If
    '                End If
    '                'controllo gli altri dati
    '                If nDaInserire = 0 Then
    '                    If IsDBNull(DvDati.Item(x)("titoccup")) Then
    '                        nDaInserire = 1
    '                    Else
    '                        If CStr(DvDati.Item(x)("titoccup")) = "-1" Then
    '                            nDaInserire = 1
    '                        End If
    '                    End If
    '                    If IsDBNull(DvDati.Item(x)("natoccup")) Then
    '                        nDaInserire = 1
    '                    Else
    '                        If CStr(DvDati.Item(x)("natoccup")) = "-1" Then
    '                            nDaInserire = 1
    '                        End If
    '                    End If
    '                    If IsDBNull(DvDati.Item(x)("tipounita")) Then
    '                        nDaInserire = 1
    '                    Else
    '                        If CStr(DvDati.Item(x)("tipounita")) = "" Then
    '                            nDaInserire = 1
    '                        End If
    '                    End If
    '                End If
    '            Else
    '                nDaInserire = 1
    '            End If
    '            If nDaInserire = 1 Then
    '                'se(dainserireinarray = 1)
    '                'incremento l'array di oggetti
    '                nList += 1
    '                'popolo il singolo oggetto con il record del dataview
    '                oMyMancante = New ObjAEDatiMancanti
    '                oMyMancante.nIdContribuente = CInt(DvDati.Item(x)("cod_contribuente"))
    '                oMyMancante.nIdArticolo = CInt(DvDati.Item(x)("id"))
    '                If Not IsDBNull(DvDati.Item(x)("anno")) Then oMyMancante.sAnno = CStr(DvDati.Item(x)("anno"))
    '                If Not IsDBNull(DvDati.Item(x)("cognome")) Then oMyMancante.sCognome = CStr(DvDati.Item(x)("cognome"))
    '                If Not IsDBNull(DvDati.Item(x)("nome")) Then oMyMancante.sNome = CStr(DvDati.Item(x)("nome"))
    '                If Not IsDBNull(DvDati.Item(x)("cfpiva")) Then oMyMancante.sCfPIva = CStr(DvDati.Item(x)("cfpiva"))
    '                If Not IsDBNull(DvDati.Item(x)("via_res")) Then oMyMancante.sViaRes = CStr(DvDati.Item(x)("via_res"))
    '                If Not IsDBNull(DvDati.Item(x)("civico_res")) Then oMyMancante.sCivicoRes = CStr(DvDati.Item(x)("civico_res"))
    '                If Not IsDBNull(DvDati.Item(x)("cap_res")) Then oMyMancante.sCapRes = CStr(DvDati.Item(x)("cap_res"))
    '                If Not IsDBNull(DvDati.Item(x)("comune_res")) Then oMyMancante.sComuneRes = CStr(DvDati.Item(x)("comune_res"))
    '                If Not IsDBNull(DvDati.Item(x)("provincia_res")) Then oMyMancante.sPvRes = CStr(DvDati.Item(x)("provincia_res"))
    '                If Not IsDBNull(DvDati.Item(x)("nominativo")) Then oMyMancante.sNominativoCO = CStr(DvDati.Item(x)("nominativo"))
    '                If Not IsDBNull(DvDati.Item(x)("via_rcp")) Then oMyMancante.sViaCO = CStr(DvDati.Item(x)("via_rcp"))
    '                If Not IsDBNull(DvDati.Item(x)("civico_rcp")) Then oMyMancante.sCivicoCO = CStr(DvDati.Item(x)("civico_rcp"))
    '                If Not IsDBNull(DvDati.Item(x)("cap_rcp")) Then oMyMancante.sCapCO = CStr(DvDati.Item(x)("cap_rcp"))
    '                If Not IsDBNull(DvDati.Item(x)("comune_rcp")) Then oMyMancante.sComuneCO = CStr(DvDati.Item(x)("comune_rcp"))
    '                If Not IsDBNull(DvDati.Item(x)("provincia_rcp")) Then oMyMancante.sPvCO = CStr(DvDati.Item(x)("provincia_rcp"))
    '                If Not IsDBNull(DvDati.Item(x)("ind_immo")) Then oMyMancante.sIndirizzoImmo = CStr(DvDati.Item(x)("ind_immo"))
    '                If Not IsDBNull(DvDati.Item(x)("foglio")) Then oMyMancante.sFoglio = CStr(DvDati.Item(x)("foglio"))
    '                If Not IsDBNull(DvDati.Item(x)("particella")) Then oMyMancante.sParticella = CStr(DvDati.Item(x)("particella"))
    '                If Not IsDBNull(DvDati.Item(x)("estensione_particella")) Then oMyMancante.sEstParticella = CStr(DvDati.Item(x)("estensione_particella"))
    '                If Not IsDBNull(DvDati.Item(x)("data_inizio")) Then oMyMancante.sDataInizio = oReplace.GiraDataFromDB(CStr(DvDati.Item(x)("data_inizio")))
    '                If Not IsDBNull(DvDati.Item(x)("titoccup")) Then oMyMancante.sDescrTitOccup = CStr(DvDati.Item(x)("titoccup"))
    '                If Not IsDBNull(DvDati.Item(x)("natoccup")) Then oMyMancante.sDescrNatOccup = CStr(DvDati.Item(x)("natoccup"))
    '                If Not IsDBNull(DvDati.Item(x)("tipounita")) Then oMyMancante.sDescrTipoUnita = CStr(DvDati.Item(x)("tipounita"))
    '                If Not IsDBNull(DvDati.Item(x)("tipoparticella")) Then oMyMancante.sDescrTipoParticella = CStr(DvDati.Item(x)("tipoparticella"))
    '                Select Case CInt(DvDati.Item(x)("anomalia"))
    '                    Case 1
    '                        oMyMancante.sDescrAnomalia = "Anagrafica Incompleta"
    '                    Case 2
    '                        oMyMancante.sDescrAnomalia = "Immobile Incompleto"
    '                    Case 3
    '                        oMyMancante.sDescrAnomalia = "Anagrafica e Immobile Incompleti"
    '                End Select
    '                'inserisco il singolo oggetto all’array di oggetti
    '                ReDim Preserve oMyListMancanti(nList)
    '                oMyListMancanti(nList) = oMyMancante
    '            End If
    '        Next
    '        Return oMyListMancanti
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsAEDatiMancanti.GetListDatiMancanti.errore: ", Err)
    '        Throw Err
    '    End Try
    'End Function
End Class

Public Class ClsAELevelDB
    Private Shared Log As ILog = LogManager.GetLogger("ClsAELevelDB")
    Private iDB As New DBAccess.getDBobject
    Private oReplace As New ClsGenerale.Generale
    Private cmdMyCommand As New SqlClient.SqlCommand
    Private DvDati As DataView
    Private DsDati As DataSet

#Region "Select"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sMyIdEnte"></param>
    ''' <param name="sMyAnno"></param>
    ''' <param name="sMyCognome"></param>
    ''' <param name="sMyNome"></param>
    ''' <param name="nMyTypeMancanti"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDatiMancanti(ByVal sMyIdEnte As String, ByVal sMyAnno As String, ByVal sMyCognome As String, ByVal sMyNome As String, ByVal nMyTypeMancanti As Integer) As DataView
        Dim dvMyDati As New DataView
        Try
                'Valorizzo la connessione
                Dim oDbManagerRepository As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                Dim sSQL As String = ""
                Using ctx As DBModel = oDbManagerRepository
                    sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_AEGetDatiMancanti", "CODISTAT", "ANNO")
                    dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("CODISTAT", sMyIdEnte) _
                        , ctx.GetParam("ANNO", sMyAnno)
                    )
                    ctx.Dispose()
                End Using

                Return dvMyDati
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsAELevelDB.GetDatiMancanti.errore: ", Err)
            Return Nothing
        End Try
    End Function
    'Public Function GetDatiMancanti(ByVal sMyIdEnte As String, ByVal sMyAnno As String, ByVal sMyCognome As String, ByVal sMyNome As String, ByVal nMyTypeMancanti As Integer) As DataView
    '    Try
    '        Dim NomeDBAnagrafica As String = ConfigurationManager.AppSettings("NOME_DATABASE_ANAGRAFICA").ToString()
    '        Dim NomeDBOPENgov As String = ConfigurationManager.AppSettings("NOME_DATABASE_OPENGOV").ToString()

    '        'Valorizzo la connessione
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        cmdMyCommand.CommandType = CommandType.Text
    '        'Valorizzo il commandtext:
    '        cmdMyCommand.CommandText = "SELECT AE.COD_CONTRIBUENTE, AE.ID, " & sMyAnno & " AS ANNO,"
    '        cmdMyCommand.CommandText += " AE.COGNOME, AE.NOME, AE.CFPIVA, "
    '        cmdMyCommand.CommandText += " AE.VIA_RES, AE.CIVICO_RES, AE.CAP_RES,  AE.COMUNE_RES,  AE.PROVINCIA_RES, "
    '        cmdMyCommand.CommandText += " SPED.COGNOME_INVIO+' '+SPED.NOME_INVIO AS NOMINATIVO, SPED.VIA_RCP, SPED.CIVICO_RCP, SPED.CAP_RCP, SPED.COMUNE_RCP, SPED.PROVINCIA_RCP,"
    '        cmdMyCommand.CommandText += " AE.IND_IMMO, AE.FOGLIO, AE.PARTICELLA, AE.ESTENSIONE_PARTICELLA,"
    '        cmdMyCommand.CommandText += " AE.DATA_INIZIO, TITOC.DESCRIZIONE AS TITOCCUP, TIPUT.DESCRIZIONE AS NATOCCUP,"
    '        cmdMyCommand.CommandText += " TIPUN.DESCRIZIONE AS TIPOUNITA, TIPPAR.DESCRIZIONE AS TIPOPARTICELLA,"
    '        cmdMyCommand.CommandText += " SUM(AE.IDANOMALIA) AS ANOMALIA"
    '        cmdMyCommand.CommandText += " FROM OPENae_GET_DATIMANCATI AE"
    '        cmdMyCommand.CommandText += " LEFT JOIN " & NomeDBAnagrafica & ".DBO.INDIRIZZI_SPEDIZIONE SPED ON AE.COD_CONTRIBUENTE=SPED.COD_CONTRIBUENTE"
    '        cmdMyCommand.CommandText += " LEFT JOIN " & NomeDBOPENgov & ".DBO.AE_TIPO_TITOLO_OCCUPAZIONE TITOC ON AE.ID_TITOLO_OCCUPAZIONE=TITOC.ID AND '9000'=TITOC.COD_TRIBUTO"
    '        cmdMyCommand.CommandText += " LEFT JOIN " & NomeDBOPENgov & ".DBO.AE_TIPO_TIPOLOGIA_UTENZA TIPUT ON AE.ID_TIPO_UTENZA=TIPUT.ID"
    '        cmdMyCommand.CommandText += " LEFT JOIN " & NomeDBOPENgov & ".DBO.AE_TIPO_TIPOLOGIA_UNITA TIPUN ON AE.ID_TIPO_UNITA=TIPUN.ID"
    '        cmdMyCommand.CommandText += " LEFT JOIN " & NomeDBOPENgov & ".DBO.AE_TIPO_TIPOLOGIA_PARTICELLA TIPPAR ON AE.ID_TIPO_PARTICELLA=TIPPAR.ID"
    '        cmdMyCommand.CommandText += " WHERE (SPED.DATA_FINE_VALIDITA IS NULL) AND (SPED.COD_TRIBUTO IS NULL OR SPED.COD_TRIBUTO='9000')"
    '        'Valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.CommandText += " AND (AE.IDENTE=@CODISTAT)"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODISTAT", SqlDbType.NVarChar)).Value = sMyIdEnte
    '        If nMyTypeMancanti <> 0 Then
    '            cmdMyCommand.CommandText += " AND (AE.IDANOMALIA=@TIPOANOMALIA)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPOANOMALIA", SqlDbType.Int)).Value = nMyTypeMancanti
    '        End If
    '        If sMyAnno <> "" Then
    '            cmdMyCommand.CommandText += " AND ((YEAR(AE.DATA_INIZIO)<=@ANNO"
    '            cmdMyCommand.CommandText += " AND (CASE WHEN NOT YEAR(AE.DATA_FINE) IS NULL AND YEAR(AE.DATA_FINE)<>'1900' THEN YEAR(AE.DATA_FINE) ELSE @ANNO END)>=@ANNO))"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = sMyAnno
    '        End If
    '        If sMyCognome <> "" Then
    '            cmdMyCommand.CommandText += " AND (AE.COGNOME LIKE @COGNOME)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COGNOME", SqlDbType.NVarChar)).Value = oReplace.ReplaceCharsForSearch(sMyCognome) & "%"
    '        End If
    '        If sMyNome <> "" Then
    '            cmdMyCommand.CommandText += " AND (AE.NOME LIKE @NOME)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOME", SqlDbType.NVarChar)).Value = oReplace.ReplaceCharsForSearch(sMyNome) & "%"
    '        End If
    '        cmdMyCommand.CommandText += " GROUP BY AE.COD_CONTRIBUENTE, AE.ID,"
    '        cmdMyCommand.CommandText += " AE.COGNOME, AE.NOME, AE.CFPIVA, "
    '        cmdMyCommand.CommandText += " AE.VIA_RES, AE.CIVICO_RES, AE.CAP_RES,  AE.COMUNE_RES,  AE.PROVINCIA_RES, "
    '        cmdMyCommand.CommandText += " SPED.COGNOME_INVIO+' '+SPED.NOME_INVIO, SPED.VIA_RCP, SPED.CIVICO_RCP, SPED.CAP_RCP, SPED.COMUNE_RCP, SPED.PROVINCIA_RCP,"
    '        cmdMyCommand.CommandText += " AE.IND_IMMO, AE.FOGLIO, AE.PARTICELLA, AE.ESTENSIONE_PARTICELLA,"
    '        cmdMyCommand.CommandText += " AE.DATA_INIZIO, TITOC.DESCRIZIONE, TIPUT.DESCRIZIONE,"
    '        cmdMyCommand.CommandText += " TIPUN.DESCRIZIONE, TIPPAR.DESCRIZIONE"
    '        cmdMyCommand.CommandText += " ORDER BY AE.COGNOME, AE.NOME, AE.CFPIVA"

    '        'eseguo la query
    '        DvDati = idb.GetDataview(cmdMyCommand)
    '        Return DvDati
    '    Catch Err As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsAELevelDB.GetDatiMancanti.errore: ", Err)
    '        Return Nothing
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sMyIdEnte"></param>
    ''' <param name="sMyAnno"></param>
    ''' <param name="sMyDataDal"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDatiEstrazione(ByVal sMyIdEnte As String, ByVal sMyAnno As String, ByVal sMyDataDal As String) As DataView
        Try
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            'Valorizzo il commandtext:
            'cmdMyCommand.CommandText = "SELECT OPENae_GET_DATITRACCIATO.*"
            'cmdMyCommand.CommandText += " FROM OPENae_GET_DATITRACCIATO"
            'cmdMyCommand.CommandText += " WHERE (OPENae_GET_DATITRACCIATO.IDENTE=@CODISTAT)"
            'cmdMyCommand.CommandText += " AND ((YEAR(OPENae_GET_DATITRACCIATO.DATA_INIZIO)<=@ANNO"
            'cmdMyCommand.CommandText += " AND (CASE WHEN NOT YEAR(OPENae_GET_DATITRACCIATO.DATA_FINE) IS NULL AND YEAR(OPENae_GET_DATITRACCIATO.DATA_FINE)<>1900 THEN YEAR(OPENae_GET_DATITRACCIATO.DATA_FINE) ELSE @ANNO END)>=@ANNO))"
            'cmdMyCommand.CommandText += " AND (OPENae_GET_DATITRACCIATO.DATA_INIZIO>=@DAL)"
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "OPENae_GET_DATITRACCIATO"
            'Valorizzo i parameters:
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODISTAT", SqlDbType.NVarChar)).Value = sMyIdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = sMyAnno
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DAL", SqlDbType.DateTime)).Value = sMyDataDal

            'eseguo la query
            DvDati = iDB.GetDataView(cmdMyCommand)
            Return DvDati
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsAELevelDB.GetDatiEstrazione.errore: ", Err)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sMyIdEnte"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDatiEnte(ByVal sMyIdEnte As String) As DataView
        Try
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnectionOPENgov)
            cmdMyCommand.CommandType = CommandType.Text
            'Valorizzo il commandtext:
            cmdMyCommand.CommandText = "SELECT *"
            cmdMyCommand.CommandText += " FROM ENTI"
            'Valorizzo i parameters:
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.CommandText += " WHERE (COD_ENTE=@CODISTAT)"
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODISTAT", SqlDbType.NVarChar)).Value = sMyIdEnte

            'eseguo la query
            DvDati = iDB.GetDataView(cmdMyCommand)
            Return DvDati
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsAELevelDB.GetDatiEnte.errore: ", Err)
            Return Nothing
        End Try
    End Function

    'Public Function GetDatiMancanti(ByVal sMyIdEnte As String, ByVal sMyAnno As String, ByVal sMyCognome As String, ByVal sMyNome As String, ByVal nMyTypeMancanti As Integer, ByVal WFSession As OPENUtility.CreateSessione) As DataView
    '    Try
    '        Dim NomeDBAnagrafica As String = ConfigurationManager.AppSettings("NOME_DATABASE_ANAGRAFICA").ToString()
    '        Dim NomeDBOPENgov As String = ConfigurationManager.AppSettings("NOME_DATABASE_OPENGOV").ToString()

    '        'Valorizzo la connessione
    '        cmdMyCommand.Connection = WFSession.oSession.oAppDB.GetConnection
    '        cmdMyCommand.CommandType = CommandType.Text
    '        'Valorizzo il commandtext:
    '        cmdMyCommand.CommandText = "SELECT AE.COD_CONTRIBUENTE, AE.ID, " & sMyAnno & " AS ANNO,"
    '        cmdMyCommand.CommandText += " AE.COGNOME, AE.NOME, AE.CFPIVA, "
    '        cmdMyCommand.CommandText += " AE.VIA_RES, AE.CIVICO_RES, AE.CAP_RES,  AE.COMUNE_RES,  AE.PROVINCIA_RES, "
    '        cmdMyCommand.CommandText += " SPED.COGNOME_INVIO+' '+SPED.NOME_INVIO AS NOMINATIVO, SPED.VIA_RCP, SPED.CIVICO_RCP, SPED.CAP_RCP, SPED.COMUNE_RCP, SPED.PROVINCIA_RCP,"
    '        cmdMyCommand.CommandText += " AE.IND_IMMO, AE.FOGLIO, AE.PARTICELLA, AE.ESTENSIONE_PARTICELLA,"
    '        cmdMyCommand.CommandText += " AE.DATA_INIZIO, TITOC.DESCRIZIONE AS TITOCCUP, TIPUT.DESCRIZIONE AS NATOCCUP,"
    '        cmdMyCommand.CommandText += " TIPUN.DESCRIZIONE AS TIPOUNITA, TIPPAR.DESCRIZIONE AS TIPOPARTICELLA,"
    '        cmdMyCommand.CommandText += " SUM(AE.IDANOMALIA) AS ANOMALIA"
    '        cmdMyCommand.CommandText += " FROM OPENae_GET_DATIMANCATI AE"
    '        cmdMyCommand.CommandText += " LEFT JOIN " & NomeDBAnagrafica & ".DBO.INDIRIZZI_SPEDIZIONE SPED ON AE.COD_CONTRIBUENTE=SPED.COD_CONTRIBUENTE"
    '        cmdMyCommand.CommandText += " LEFT JOIN " & NomeDBOPENgov & ".DBO.AE_TIPO_TITOLO_OCCUPAZIONE TITOC ON AE.ID_TITOLO_OCCUPAZIONE=TITOC.ID AND '9000'=TITOC.COD_TRIBUTO"
    '        cmdMyCommand.CommandText += " LEFT JOIN " & NomeDBOPENgov & ".DBO.AE_TIPO_TIPOLOGIA_UTENZA TIPUT ON AE.ID_TIPO_UTENZA=TIPUT.ID"
    '        cmdMyCommand.CommandText += " LEFT JOIN " & NomeDBOPENgov & ".DBO.AE_TIPO_TIPOLOGIA_UNITA TIPUN ON AE.ID_TIPO_UNITA=TIPUN.ID"
    '        cmdMyCommand.CommandText += " LEFT JOIN " & NomeDBOPENgov & ".DBO.AE_TIPO_TIPOLOGIA_PARTICELLA TIPPAR ON AE.ID_TIPO_PARTICELLA=TIPPAR.ID"
    '        cmdMyCommand.CommandText += " WHERE (SPED.DATA_FINE_VALIDITA IS NULL) AND (SPED.COD_TRIBUTO IS NULL OR SPED.COD_TRIBUTO='9000')"
    '        'Valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.CommandText += " AND (AE.IDENTE=@CODISTAT)"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODISTAT", SqlDbType.NVarChar)).Value = sMyIdEnte
    '        If nMyTypeMancanti <> 0 Then
    '            cmdMyCommand.CommandText += " AND (AE.IDANOMALIA=@TIPOANOMALIA)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TIPOANOMALIA", SqlDbType.Int)).Value = nMyTypeMancanti
    '        End If
    '        If sMyAnno <> "" Then
    '            cmdMyCommand.CommandText += " AND ((YEAR(AE.DATA_INIZIO)<=@ANNO"
    '            cmdMyCommand.CommandText += " AND (CASE WHEN NOT YEAR(AE.DATA_FINE) IS NULL AND YEAR(AE.DATA_FINE)<>'1900' THEN YEAR(AE.DATA_FINE) ELSE @ANNO END)>=@ANNO))"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = sMyAnno
    '        End If
    '        If sMyCognome <> "" Then
    '            cmdMyCommand.CommandText += " AND (AE.COGNOME LIKE @COGNOME)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@COGNOME", SqlDbType.NVarChar)).Value = oReplace.ReplaceCharsForSearch(sMyCognome) & "%"
    '        End If
    '        If sMyNome <> "" Then
    '            cmdMyCommand.CommandText += " AND (AE.NOME LIKE @NOME)"
    '            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@NOME", SqlDbType.NVarChar)).Value = oReplace.ReplaceCharsForSearch(sMyNome) & "%"
    '        End If
    '        cmdMyCommand.CommandText += " GROUP BY AE.COD_CONTRIBUENTE, AE.ID,"
    '        cmdMyCommand.CommandText += " AE.COGNOME, AE.NOME, AE.CFPIVA, "
    '        cmdMyCommand.CommandText += " AE.VIA_RES, AE.CIVICO_RES, AE.CAP_RES,  AE.COMUNE_RES,  AE.PROVINCIA_RES, "
    '        cmdMyCommand.CommandText += " SPED.COGNOME_INVIO+' '+SPED.NOME_INVIO, SPED.VIA_RCP, SPED.CIVICO_RCP, SPED.CAP_RCP, SPED.COMUNE_RCP, SPED.PROVINCIA_RCP,"
    '        cmdMyCommand.CommandText += " AE.IND_IMMO, AE.FOGLIO, AE.PARTICELLA, AE.ESTENSIONE_PARTICELLA,"
    '        cmdMyCommand.CommandText += " AE.DATA_INIZIO, TITOC.DESCRIZIONE, TIPUT.DESCRIZIONE,"
    '        cmdMyCommand.CommandText += " TIPUN.DESCRIZIONE, TIPPAR.DESCRIZIONE"
    '        cmdMyCommand.CommandText += " ORDER BY AE.COGNOME, AE.NOME, AE.CFPIVA"

    '        'eseguo la query
    '        DvDati = WFSession.oSession.oAppDB.GetPrivateDataview(cmdMyCommand)
    '        Return DvDati
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsAELevelDB.GetDatiMancanti.errore: ", Err)
    '        Return Nothing
    '    End Try
    'End Function

    'Public Function GetDatiEstrazione(ByVal sMyIdEnte As String, ByVal sMyAnno As String, ByVal sMyDataDal As String, ByVal WFSession As OPENUtility.CreateSessione) As DataView
    '    Try
    '        'Valorizzo la connessione
    '        cmdMyCommand.Connection = WFSession.oSession.oAppDB.GetConnection
    '        'Valorizzo il commandtext:
    '        'cmdMyCommand.CommandText = "SELECT OPENae_GET_DATITRACCIATO.*"
    '        'cmdMyCommand.CommandText += " FROM OPENae_GET_DATITRACCIATO"
    '        'cmdMyCommand.CommandText += " WHERE (OPENae_GET_DATITRACCIATO.IDENTE=@CODISTAT)"
    '        'cmdMyCommand.CommandText += " AND ((YEAR(OPENae_GET_DATITRACCIATO.DATA_INIZIO)<=@ANNO"
    '        'cmdMyCommand.CommandText += " AND (CASE WHEN NOT YEAR(OPENae_GET_DATITRACCIATO.DATA_FINE) IS NULL AND YEAR(OPENae_GET_DATITRACCIATO.DATA_FINE)<>1900 THEN YEAR(OPENae_GET_DATITRACCIATO.DATA_FINE) ELSE @ANNO END)>=@ANNO))"
    '        'cmdMyCommand.CommandText += " AND (OPENae_GET_DATITRACCIATO.DATA_INIZIO>=@DAL)"
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = "OPENae_GET_DATITRACCIATO"
    '        'Valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODISTAT", SqlDbType.NVarChar)).Value = sMyIdEnte
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@ANNO", SqlDbType.NVarChar)).Value = sMyAnno
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@DAL", SqlDbType.DateTime)).Value = sMyDataDal

    '        'eseguo la query
    '        DvDati = WFSession.oSession.oAppDB.GetPrivateDataview(cmdMyCommand)
    '        Return DvDati
    '    Catch Err As Exception
    '        Log.Debug("Si è verificato un errore in ClsAELevelDB::GetDatiEstrazione::" & Err.Message)
    '        Log.Warn("Si è verificato un errore in ClsAELevelDB::GetDatiEstrazione::" & Err.Message)
    '        Return Nothing
    '    End Try
    'End Function

    'Public Function GetDatiEnte(ByVal sMyIdEnte As String, ByVal WFSession As OPENUtility.CreateSessione) As DataView
    '    Try
    '        'Valorizzo la connessione
    '        cmdMyCommand.Connection = WFSession.oSession.oAppDB.GetConnection
    '        cmdMyCommand.CommandType = CommandType.Text
    '        'Valorizzo il commandtext:
    '        cmdMyCommand.CommandText = "SELECT ENTI.*"
    '        cmdMyCommand.CommandText += " FROM ENTI"
    '        'Valorizzo i parameters:
    '        cmdMyCommand.Parameters.Clear()
    '        cmdMyCommand.CommandText += " WHERE (ENTI.COD_ENTE=@CODISTAT)"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@CODISTAT", SqlDbType.NVarChar)).Value = sMyIdEnte

    '        'eseguo la query
    '        DvDati = WFSession.oSession.oAppDB.GetPrivateDataview(cmdMyCommand)
    '        Return DvDati
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsAELevelDB.GetDatiEnte.errore: ", Err)
    '        Return Nothing
    '    End Try
    'End Function
#End Region
End Class

Public Class ClsAETracciati
    Private Shared Log As ILog = LogManager.GetLogger("ClsAETracciati")
    Private FncDB As New ClsAELevelDB
    Private FncReplace As New ModificaDate

    Public Function GetDisposizioni(ByVal sMyEnte As String, ByVal sMyAnno As String, ByVal sMyDataDal As String) As WSOPENae.DisposizioneAE()
        Try
            Dim DvDati As DataView
            Dim x As Integer
            Dim oMyDisposizione As WSOPENae.DisposizioneAE
            Dim oListDisposizioni() As WSOPENae.DisposizioneAE
            Dim oDispDatiEnte As New WSOPENae.DisposizioneAE

            oListDisposizioni = Nothing
            'prelevo i dati dell'ente
            oDispDatiEnte = GetDatiEnte(sMyEnte)
            If oDispDatiEnte Is Nothing Then
                Return Nothing
            End If

            DvDati = FncDB.GetDatiEstrazione(sMyEnte, sMyAnno, sMyDataDal)
            For x = 0 To DvDati.Count - 1
                'inizializzo il nuovo oggetto
                oMyDisposizione = New WSOPENae.DisposizioneAE
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
                oMyDisposizione.sTributo = "9000"
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
                If Not IsDBNull(DvDati.Item(x)("tipocontratto")) Then oMyDisposizione.sTipoContratto = CStr(DvDati.Item(x)("tipocontratto"))
                oMyDisposizione.sDataInizio = CStr(DvDati.Item(x)("data_inizio"))
                If Not IsDBNull(DvDati.Item(x)("data_fine")) Then oMyDisposizione.sDataFine = CStr(DvDati.Item(x)("data_fine"))
                If Not IsDBNull(DvDati.Item(x)("id_assenza_dati_catastali")) Then
                    oMyDisposizione.nIDAssenzaDatiCatastali = CInt(DvDati.Item(x)("id_assenza_dati_catastali"))
                    If oMyDisposizione.nIDAssenzaDatiCatastali = -1 Then Stop
                Else
                    oMyDisposizione.nIDAssenzaDatiCatastali = 0
                End If
                If Not IsDBNull(DvDati.Item(x)("id_titolo_occupazione")) Then oMyDisposizione.nIDTitoloOccupazione = CInt(DvDati.Item(x)("id_titolo_occupazione"))
                If Not IsDBNull(DvDati.Item(x)("id_tipo_utenza")) Then oMyDisposizione.nIDTipoUtenza = CInt(DvDati.Item(x)("id_tipo_utenza")) + 1
                If Not IsDBNull(DvDati.Item(x)("id_tipo_unita")) Then oMyDisposizione.sIDTipoUnita = CStr(DvDati.Item(x)("id_tipo_unita"))
                If Not IsDBNull(DvDati.Item(x)("estremi_contratto")) Then oMyDisposizione.sEstremiContratto = CStr(DvDati.Item(x)("estremi_contratto"))
                If Not IsDBNull(DvDati.Item(x)("mesi_fatturazione")) Then oMyDisposizione.nMesiFatturazione = CInt(DvDati.Item(x)("mesi_fatturazione"))
                'If Not IsDBNull(DvDati.Item(x)("segno_fatturazione")) Then oMyDisposizione.sSegno = CStr(DvDati.Item(x)("segno_fatturazione"))
                If Not IsDBNull(DvDati.Item(x)("consumo")) Then oMyDisposizione.nConsumo = CInt(DvDati.Item(x)("consumo"))
                If Not IsDBNull(DvDati.Item(x)("importofatturato")) Then oMyDisposizione.nImportoFatturato = CDbl(DvDati.Item(x)("importofatturato"))

                'inserisco il singolo oggetto all’array di oggetti
                ReDim Preserve oListDisposizioni(x)
                oListDisposizioni(x) = oMyDisposizione
            Next
            Return oListDisposizioni
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsAETracciati.GetDisposizioni.errore: ", Err)
            Throw Err
        End Try
    End Function

    Private Function GetDatiEnte(ByVal sMyEnte As String) As WSOPENae.DisposizioneAE
        Try
            Dim oMyDisposizione As New WSOPENae.DisposizioneAE
            Dim DvDati As DataView
            Dim x As Integer

            DvDati = FncDB.GetDatiEnte(sMyEnte)
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

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsAETracciati.GetDatiEnte.errore: ", Err)
            Throw Err
        End Try
    End Function

    'Public Function GetDisposizioni(ByVal sMyEnte As String, ByVal sMyAnno As String, ByVal sMyDataDal As String, ByVal WFSessione As OPENUtility.CreateSessione, ByVal sParametroENV As String, ByVal sUsername As String) As WSOPENae.DisposizioneAE()
    '    Try
    '        Dim DvDati As DataView
    '        Dim x As Integer
    '        Dim oMyDisposizione As WSOPENae.DisposizioneAE
    '        Dim oListDisposizioni() As WSOPENae.DisposizioneAE
    '        Dim oDispDatiEnte As New WSOPENae.DisposizioneAE

    '        'prelevo i dati dell'ente
    '        oDispDatiEnte = GetDatiEnte(sMyEnte, sParametroENV, sUsername)
    '        If oDispDatiEnte Is Nothing Then
    '            Return Nothing
    '        End If

    '        DvDati = FncDB.GetDatiEstrazione(sMyEnte, sMyAnno, sMyDataDal, WFSessione)
    '        For x = 0 To DvDati.Count - 1
    '            'inizializzo il nuovo oggetto
    '            oMyDisposizione = New WSOPENae.DisposizioneAE
    '            'popolo i dati del comune
    '            oMyDisposizione.sCodFiscaleEnte = oDispDatiEnte.sCodFiscaleEnte
    '            oMyDisposizione.sCognomeEnte = oDispDatiEnte.sCognomeEnte
    '            oMyDisposizione.sNomeEnte = oDispDatiEnte.sNomeEnte
    '            oMyDisposizione.sSessoEnte = oDispDatiEnte.sSessoEnte
    '            oMyDisposizione.sDataNascitaEnte = oDispDatiEnte.sDataNascitaEnte
    '            oMyDisposizione.sComuneNascitaSedeEnte = oDispDatiEnte.sComuneNascitaSedeEnte
    '            oMyDisposizione.sPVNascitaSedeEnte = oDispDatiEnte.sPVNascitaSedeEnte
    '            oMyDisposizione.sCodComuneUbicazioneCatast = oDispDatiEnte.sCodComuneUbicazioneCatast
    '            oMyDisposizione.sComuneAmmUbicazione = oDispDatiEnte.sComuneAmmUbicazione
    '            oMyDisposizione.sComuneCatastUbicazione = oDispDatiEnte.sComuneCatastUbicazione
    '            oMyDisposizione.sPVAmmUbicazione = oDispDatiEnte.sPVAmmUbicazione
    '            'popolo i dati della posizione
    '            oMyDisposizione.sCodISTAT = sMyEnte
    '            oMyDisposizione.sTributo = MyUtility.TRIBUTO_H2O
    '            oMyDisposizione.nIDCollegamento = CInt(DvDati.Item(x)("id"))
    '            oMyDisposizione.nIDContribuente = CInt(DvDati.Item(x)("idcontribuente"))
    '            oMyDisposizione.sAnno = sMyAnno
    '            oMyDisposizione.sCodFiscale = CStr(DvDati.Item(x)("cf_piva"))
    '            oMyDisposizione.sCognome = CStr(DvDati.Item(x)("cognome"))
    '            If Not IsDBNull(DvDati.Item(x)("nome")) Then oMyDisposizione.sNome = CStr(DvDati.Item(x)("nome"))
    '            oMyDisposizione.sSesso = CStr(DvDati.Item(x)("sesso"))
    '            If Not IsDBNull(DvDati.Item(x)("data_nascita")) Then oMyDisposizione.sDataNascita = CStr(DvDati.Item(x)("data_nascita"))
    '            If Not IsDBNull(DvDati.Item(x)("comune_nascitasede")) Then oMyDisposizione.sComuneNascitaSede = CStr(DvDati.Item(x)("comune_nascitasede"))
    '            If Not IsDBNull(DvDati.Item(x)("pv_nascitasede")) Then oMyDisposizione.sPVNascitaSede = CStr(DvDati.Item(x)("pv_nascitasede"))
    '            If Not IsDBNull(DvDati.Item(x)("indirizzo")) Then oMyDisposizione.sIndirizzo = CStr(DvDati.Item(x)("indirizzo"))
    '            If Not IsDBNull(DvDati.Item(x)("civico")) Then oMyDisposizione.sCivico = CStr(DvDati.Item(x)("civico"))
    '            If Not IsDBNull(DvDati.Item(x)("interno")) Then oMyDisposizione.sInterno = CStr(DvDati.Item(x)("interno"))
    '            If Not IsDBNull(DvDati.Item(x)("scala")) Then oMyDisposizione.sScala = CStr(DvDati.Item(x)("scala"))
    '            If Not IsDBNull(DvDati.Item(x)("sezione")) Then oMyDisposizione.sSezione = CStr(DvDati.Item(x)("sezione"))
    '            If Not IsDBNull(DvDati.Item(x)("foglio")) Then oMyDisposizione.sFoglio = CStr(DvDati.Item(x)("foglio"))
    '            If Not IsDBNull(DvDati.Item(x)("particella")) Then oMyDisposizione.sParticella = CStr(DvDati.Item(x)("particella"))
    '            If Not IsDBNull(DvDati.Item(x)("subalterno")) Then oMyDisposizione.sSubalterno = CStr(DvDati.Item(x)("subalterno"))
    '            If Not IsDBNull(DvDati.Item(x)("estensione_particella")) Then oMyDisposizione.sEstensioneParticella = CStr(DvDati.Item(x)("estensione_particella"))
    '            If Not IsDBNull(DvDati.Item(x)("id_tipo_particella")) Then oMyDisposizione.sIDTipoParticella = CStr(DvDati.Item(x)("id_tipo_particella"))
    '            If Not IsDBNull(DvDati.Item(x)("comune_res")) Then oMyDisposizione.sComuneDomFisc = CStr(DvDati.Item(x)("comune_res"))
    '            If Not IsDBNull(DvDati.Item(x)("pv_res")) Then oMyDisposizione.sPVDomFisc = CStr(DvDati.Item(x)("pv_res"))
    '            If Not IsDBNull(DvDati.Item(x)("tipocontratto")) Then oMyDisposizione.sTipoContratto = CStr(DvDati.Item(x)("tipocontratto"))
    '            oMyDisposizione.sDataInizio = CStr(DvDati.Item(x)("data_inizio"))
    '            If Not IsDBNull(DvDati.Item(x)("data_fine")) Then oMyDisposizione.sDataFine = CStr(DvDati.Item(x)("data_fine"))
    '            If Not IsDBNull(DvDati.Item(x)("id_assenza_dati_catastali")) Then
    '                oMyDisposizione.nIDAssenzaDatiCatastali = CInt(DvDati.Item(x)("id_assenza_dati_catastali"))
    '                If oMyDisposizione.nIDAssenzaDatiCatastali = -1 Then Stop
    '            Else
    '                oMyDisposizione.nIDAssenzaDatiCatastali = 0
    '            End If
    '            If Not IsDBNull(DvDati.Item(x)("id_titolo_occupazione")) Then oMyDisposizione.nIDTitoloOccupazione = CInt(DvDati.Item(x)("id_titolo_occupazione"))
    '            If Not IsDBNull(DvDati.Item(x)("id_tipo_utenza")) Then oMyDisposizione.nIDTipoUtenza = CInt(DvDati.Item(x)("id_tipo_utenza")) + 1
    '            If Not IsDBNull(DvDati.Item(x)("id_tipo_unita")) Then oMyDisposizione.sIDTipoUnita = CStr(DvDati.Item(x)("id_tipo_unita"))
    '            If Not IsDBNull(DvDati.Item(x)("estremi_contratto")) Then oMyDisposizione.sEstremiContratto = CStr(DvDati.Item(x)("estremi_contratto"))
    '            If Not IsDBNull(DvDati.Item(x)("mesi_fatturazione")) Then oMyDisposizione.nMesiFatturazione = CInt(DvDati.Item(x)("mesi_fatturazione"))
    '            'If Not IsDBNull(DvDati.Item(x)("segno_fatturazione")) Then oMyDisposizione.sSegno = CStr(DvDati.Item(x)("segno_fatturazione"))
    '            If Not IsDBNull(DvDati.Item(x)("consumo")) Then oMyDisposizione.nConsumo = CInt(DvDati.Item(x)("consumo"))
    '            If Not IsDBNull(DvDati.Item(x)("importofatturato")) Then oMyDisposizione.nImportoFatturato = CDbl(DvDati.Item(x)("importofatturato"))

    '            'inserisco il singolo oggetto all’array di oggetti
    '            ReDim Preserve oListDisposizioni(x)
    '            oListDisposizioni(x) = oMyDisposizione
    '        Next
    '        Return oListDisposizioni
    '    Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsAETracciati.GetDisposizioni.errore: ", Err)
    '        Throw Err
    '    End Try
    'End Function

    'Private Function GetDatiEnte(ByVal sMyEnte As String, ByVal sParametroENV As String, ByVal sUsername As String) As WSOPENae.DisposizioneAE
    '    Try
    '        Dim oMyDisposizione As New WSOPENae.DisposizioneAE
    '        Dim DvDati As DataView
    '        Dim x As Integer
    '        Dim WFSessione As OPENUtility.CreateSessione
    '        Dim WFErrore As String

    '        WFSessione = New OPENUtility.CreateSessione(sParametroENV, sUsername, ConfigurationManager.AppSettings("OPENGOVG").ToString())
    '        If Not WFSessione.CreaSessione(sUsername, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        DvDati = FncDB.GetDatiEnte(sMyEnte, WFSessione)
    '        For x = 0 To DvDati.Count - 1
    '            oMyDisposizione.sCodISTAT = sMyEnte
    '            oMyDisposizione.sCodFiscaleEnte = CStr(DvDati.Item(x)("cf_piva"))
    '            oMyDisposizione.sCognomeEnte = CStr(DvDati.Item(x)("cognome"))
    '            oMyDisposizione.sNomeEnte = CStr(DvDati.Item(x)("nome"))
    '            oMyDisposizione.sSessoEnte = CStr(DvDati.Item(x)("sesso"))
    '            oMyDisposizione.sDataNascitaEnte = CStr(DvDati.Item(x)("data_nascita"))
    '            oMyDisposizione.sComuneNascitaSedeEnte = CStr(DvDati.Item(x)("comune_nascitasede"))
    '            oMyDisposizione.sPVNascitaSedeEnte = CStr(DvDati.Item(x)("pv_nascitasede"))
    '            oMyDisposizione.sCodComuneUbicazioneCatast = CStr(DvDati.Item(x)("cod_ubicazcat"))
    '            oMyDisposizione.sComuneAmmUbicazione = CStr(DvDati.Item(x)("descrizione_ente"))
    '            oMyDisposizione.sComuneCatastUbicazione = CStr(DvDati.Item(x)("comunecat"))
    '            oMyDisposizione.sPVAmmUbicazione = CStr(DvDati.Item(x)("provincia_sigla"))
    '        Next
    '        Return oMyDisposizione
    '    Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsAETracciati.GetDatiEnte.errore: ", Err)
    '        Throw Err
    '    End Try
    'End Function
End Class