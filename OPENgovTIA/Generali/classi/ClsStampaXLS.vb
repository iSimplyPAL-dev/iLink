Imports log4net
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
''' <summary>
''' Classe per la gestione delle stampe in formato CSV/XLS
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class ClsStampaXLS
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ClsStampaXLS))

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
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsStampaXLS.AddRowStampa.errore: ", Err)
            Return 0
        End Try
    End Function
    Public Function PrintMinutaRuolo(DvDati As DataView, ByVal HasConferimenti As Boolean, ByVal HasMaggiorazione As Boolean, ByVal sIntestazioneEnte As String, ByVal hasAnagAllRow As Integer, nCampi As Integer, IsMinutaXStampatore As Integer) As DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim x, IdContribPrec As Integer
        Dim nTotContribuenti As Integer = 0
        Dim nTotArticoli As Integer = 0
        Dim nTotMq As Double = 0
        Dim impTotArticoli As Double = 0
        Dim impTotRid As Double = 0
        Dim impTotDet As Double = 0
        Dim impTotNetto As Double = 0
        Dim impPF, impPV, impPC, impPM, impPrec, impECA, impMECA, impProvincialePerEnte, impAggioPerEnte, impProvinciale, impSpese, impSanzioni, impTotale As Double
        Dim sAvanzamento As String
        Dim nAvanzamento As Integer = 0

        Try
            impPF = 0 : impPV = 0 : impPC = 0 : impPM = 0 : impPrec = 0 : impECA = 0 : impMECA = 0 : impProvincialePerEnte = 0 : impAggioPerEnte = 0 : impProvinciale = 0 : impSpese = 0 : impSanzioni = 0 : impTotale = 0
            '*** 201511 - PF e PV su unica riga ***'*** 20130123 - aggiunti i riferimenti catastali ***
            'nCampi = 61 '54 '45 '39 '27 '24 '21
            'carico il dataset
            DsStampa.Tables.Add("STAMPA")
            'carico le colonne nel dataset
            For x = 0 To nCampi + 1
                DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
            Next
            'carico il datatable
            DtStampa = DsStampa.Tables("STAMPA")
            'inserisco l'intestazione dell'ente
            sDatiStampa = sIntestazioneEnte
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco l'intestazione del report
            sDatiStampa = "Minuta Ruolo Anno " & DvDati.Item(0)("annoruolo")
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco le intestazioni di colonna
            sDatiStampa = ""
            sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.Iva"
            If IsMinutaXStampatore = 1 Then
                sDatiStampa += "|Data Nascita|Comune Nascita|PV Nascita|Sesso"
            End If
            sDatiStampa += "|Indirizzo Residenza|Civico Residenza|CAP Residenza|Comune Residenza|Provincia Residenza"
            If IsMinutaXStampatore = 1 Then
                sDatiStampa += "|Nominativo Invio|Indirizzo Invio|Civico Invio|CAP Invio|Comune Invio|Provincia Invio"
                sDatiStampa += "|N.Avviso"
                sDatiStampa += "|Imp.Netto"
                sDatiStampa += "|Imp.Addizionali EX ECA|Imp.Tributo EX ECA|Imp.Aggio per Ente|Imp.Addizionale Provinciale per Ente|Imp.Addizionale Provinciale"
                If HasMaggiorazione Then
                    sDatiStampa += "|Imp.Maggiorazione"
                End If
                If HasConferimenti Then
                    sDatiStampa += "|Imp.Conferimenti"
                End If
                sDatiStampa += "|Imp.Arrotondamento|Imp.Totale|Imp.Originale"
                If DvDati(0)("tipo_ruolo") = ObjRuolo.Ruolo.CartelleInsoluti Then
                    sDatiStampa += "|Imp.Sanzioni|Imp.Spese Notifica"
                End If
                sDatiStampa += "|Tipo Utenza"
            End If
            sDatiStampa += "|Anno|Ubicazione|Civico|Esponente|Interno"
            '*** 20130123 - aggiunti i riferimenti catastali ***
            sDatiStampa += "|Foglio|Numero|Subalterno"
            '*** ***
            sDatiStampa += "|Tipo Partita|Categoria|N.Componenti"
            If HasConferimenti Then
                sDatiStampa += "|MQ/LT"
            Else
                sDatiStampa += "|MQ"
            End If
            sDatiStampa += "|Tempo"
            sDatiStampa += "|Tariffa PF|Imp.Articoli PF|Imp.Riduzioni PF|Imp.Detassazioni PF|Imp.Netto PF"
            sDatiStampa += "|Tariffa PV|Imp.Articoli PV|Imp.Riduzioni PV|Imp.Detassazioni PV|Imp.Netto PV"
            sDatiStampa += "|Totale Netto"
            sDatiStampa += "|Riduzioni"
            If IsMinutaXStampatore = 1 Then
                sDatiStampa += "|Imp.Rata 1|Imp.Rata 2|Imp.Rata 3|Imp.Rata U"
            End If
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'ciclo sui dati da stampare
            For Each myRow As DataRowView In DvDati
                nAvanzamento += 1
                sAvanzamento = "Scrittura posizione " & nAvanzamento & " su " & DvDati.Count
                CacheManager.SetAvanzamentoElaborazione(sAvanzamento)
                If CInt(myRow("idcontribuente")) <> IdContribPrec Then
                    nTotContribuenti += 1
                End If
                sDatiStampa = ""
                nTotArticoli += 1
                If CInt(myRow("idcontribuente")) <> IdContribPrec Or IsMinutaXStampatore = 1 Or hasAnagAllRow = 1 Then
                    If Not IsDBNull(myRow("Cognome")) Then
                        sDatiStampa += myRow("Cognome").ToString
                    Else
                        sDatiStampa += "|"
                    End If
                    If Not IsDBNull(myRow("Nome")) Then
                        sDatiStampa += "|" + myRow("Nome").ToString
                    Else
                        sDatiStampa += "|"
                    End If
                    If Not IsDBNull(myRow("cfpiva")) Then
                        sDatiStampa += "|'" + myRow("cfpiva").ToString.ToUpper()
                    Else
                        sDatiStampa += "|"
                    End If
                    If IsMinutaXStampatore = 1 Then
                        If Not IsDBNull(myRow("Data_Nascita")) Then
                            If CDate(myRow("Data_Nascita").ToString) <> DateTime.MaxValue Then
                                sDatiStampa += "|" + CDate(myRow("Data_Nascita")).ToShortDateString
                            Else
                                sDatiStampa += "|"
                            End If
                        Else
                            sDatiStampa += "|"
                        End If
                        If Not IsDBNull(myRow("Comune_Nascita")) Then
                            sDatiStampa += "|" + myRow("Comune_Nascita").ToString
                        Else
                            sDatiStampa += "|"
                        End If
                        If Not IsDBNull(myRow("PV_Nascita")) Then
                            sDatiStampa += "|" + myRow("PV_Nascita").ToString
                        Else
                            sDatiStampa += "|"
                        End If
                        If Not IsDBNull(myRow("Sesso")) Then
                            sDatiStampa += "|" + myRow("Sesso").ToString
                        Else
                            sDatiStampa += "|"
                        End If
                    End If
                    If Not IsDBNull(myRow("via_res")) Then
                        sDatiStampa += "|" + myRow("via_res").ToString
                    Else
                        sDatiStampa += "|"
                    End If
                    If Not IsDBNull(myRow("Civico_Res")) Then
                        If myRow("Civico_Res").ToString <> "-1" Then
                            sDatiStampa += "|'" + myRow("Civico_Res").ToString
                        Else
                            sDatiStampa += "|"
                        End If
                    Else
                        sDatiStampa += "|"
                    End If
                    If Not IsDBNull(myRow("CAP_Res")) Then
                        'sDatiStampa += "|" &  myRow("sEsponenteRes
                        'sDatiStampa += "|" &  myRow("sInternoRes & " " &  myRow("sScalaRes
                        sDatiStampa += "|'" + myRow("CAP_Res").ToString
                    Else
                        sDatiStampa += "|"
                    End If
                    If Not IsDBNull(myRow("Comune_Res")) Then
                        sDatiStampa += "|" + myRow("Comune_Res").ToString
                    Else
                        sDatiStampa += "|"
                    End If
                    If Not IsDBNull(myRow("Provincia_Res")) Then
                        sDatiStampa += "|" + myRow("Provincia_Res").ToString
                    Else
                        sDatiStampa += "|"
                    End If
                    If IsMinutaXStampatore = 1 Then
                        'Dim y As Integer
                        Dim impNetto As Double = 0
                        If Not IsDBNull(myRow("NominativoCO")) Then
                            sDatiStampa += "|" + myRow("NominativoCO").ToString
                        Else
                            sDatiStampa += "|"
                        End If
                        If Not IsDBNull(myRow("IndirizzoCO")) Then
                            sDatiStampa += "|" + myRow("IndirizzoCO").ToString
                        Else
                            sDatiStampa += "|"
                        End If
                        If Not IsDBNull(myRow("CivicoCO")) Then
                            If myRow("CivicoCO").ToString <> "-1" Then
                                sDatiStampa += "|'" + myRow("CivicoCO").ToString
                            Else
                                sDatiStampa += "|"
                            End If
                        Else
                            sDatiStampa += "|"
                        End If
                        If Not IsDBNull(myRow("CAPCO")) Then
                            sDatiStampa += "|'" + myRow("CAPCO").ToString
                        Else
                            sDatiStampa += "|"
                        End If
                        If Not IsDBNull(myRow("ComuneCO")) Then
                            sDatiStampa += "|" + myRow("ComuneCO").ToString
                        Else
                            sDatiStampa += "|"
                        End If
                        If Not IsDBNull(myRow("PvCO")) Then
                            sDatiStampa += "|" + myRow("PvCO").ToString
                        Else
                            sDatiStampa += "|"
                        End If
                        If Not IsDBNull(myRow("Codice_Cartella")) Then
                            sDatiStampa += "|'" + myRow("Codice_Cartella").ToString
                        Else
                            sDatiStampa += "|"
                        End If
                        If Not IsDBNull(myRow("importo_PF")) Then
                            impNetto += CDbl(myRow("importo_PF"))
                        End If
                        If Not IsDBNull(myRow("importo_PV")) Then
                            impNetto += CDbl(myRow("importo_PV"))
                        End If
                        sDatiStampa += "|" + FormatNumber(impNetto.ToString, 2)
                        If Not IsDBNull(myRow("impprovincialeperente")) Then
                            impProvincialePerEnte += myRow("impprovincialeperente").ToString
                        End If
                        If Not IsDBNull(myRow("impaggioperente")) Then
                            impAggioPerEnte += myRow("impaggioperente").ToString
                        End If
                        If Not IsDBNull(myRow("impeca")) Then
                            impECA += myRow("impeca").ToString
                        End If
                        If Not IsDBNull(myRow("impmeca")) Then
                            impMECA += myRow("impmeca").ToString
                        End If
                        If Not IsDBNull(myRow("addprov")) Then
                            impProvinciale += myRow("addprov").ToString
                        End If
                        sDatiStampa += "|" + FormatNumber(impECA.ToString, 2)
                        sDatiStampa += "|" + FormatNumber(impMECA.ToString, 2)
                        sDatiStampa += "|" + FormatNumber(impAggioPerEnte.ToString, 2)
                        sDatiStampa += "|" + FormatNumber(impProvincialePerEnte.ToString, 2)
                        sDatiStampa += "|" + FormatNumber(impProvinciale.ToString, 2)
                        If HasMaggiorazione Then
                            If Not IsDBNull(myRow("importo_PM")) Then
                                sDatiStampa += "|" + FormatNumber(myRow("importo_PM").ToString, 2)
                            Else
                                sDatiStampa += "|"
                            End If
                        End If
                        If HasConferimenti Then
                            If Not IsDBNull(myRow("importo_PC")) Then
                                sDatiStampa += "|" + FormatNumber(myRow("importo_PC").ToString, 2)
                            Else
                                sDatiStampa += "|"
                            End If
                        End If
                        If Not IsDBNull(myRow("importo_Arrotondamento")) Then
                            sDatiStampa += "|" + FormatNumber(myRow("importo_Arrotondamento").ToString, 2)
                        Else
                            sDatiStampa += "|"
                        End If
                        If Not IsDBNull(myRow("importo_Carico")) Then
                            impTotale += FormatNumber(myRow("importo_Carico").ToString, 2)
                            sDatiStampa += "|" + FormatNumber(myRow("importo_Carico").ToString, 2)
                        Else
                            sDatiStampa += "|"
                        End If
                        If myRow("tipo_ruolo") = ObjRuolo.Ruolo.CartelleInsoluti Then
                            If Not IsDBNull(myRow("impsanzioni")) Then
                                impSanzioni += FormatNumber(myRow("impsanzioni").ToString, 2)
                                sDatiStampa += "|" + FormatNumber(myRow("impsanzioni").ToString, 2)
                            Else
                                sDatiStampa += "|"
                            End If
                            If Not IsDBNull(myRow("impspesenotifica")) Then
                                impSpese += FormatNumber(myRow("impspesenotifica").ToString, 2)
                                sDatiStampa += "|" + FormatNumber(myRow("impspesenotifica").ToString, 2)
                            Else
                                sDatiStampa += "|"
                            End If
                        End If
                        If Not IsDBNull(myRow("importo_PRE_Sgravio")) Then
                            If CDbl(myRow("importo_PRE_Sgravio")) >= 0 Then
                                sDatiStampa += "|" + FormatNumber(myRow("importo_PRE_Sgravio").ToString, 2)
                            Else
                                sDatiStampa += "|"
                            End If
                        Else
                            sDatiStampa += "|"
                        End If
                        If Not IsDBNull(myRow("tipoutenza")) Then
                            sDatiStampa += "|" + myRow("tipoutenza").ToString
                        Else
                            sDatiStampa += "|"
                        End If
                    End If
                Else
                    sDatiStampa += "|||||||"
                End If
                If Not IsDBNull(myRow("annoruolo")) Then
                    sDatiStampa += "|" + myRow("annoruolo").ToString
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("ubicazione")) Then
                    sDatiStampa += "|" + myRow("ubicazione").ToString
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("Civico")) Then
                    sDatiStampa += "|'" + myRow("Civico").ToString
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("Esponente")) Then
                    sDatiStampa += "|" & " " & myRow("Esponente").ToString
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("Interno")) Then
                    sDatiStampa += "|" & " " & myRow("Interno").ToString
                Else
                    sDatiStampa += "|"
                End If
                '*** 20130123 - aggiunti i riferimenti catastali ***
                If Not IsDBNull(myRow("Foglio")) Then
                    sDatiStampa += "|" + myRow("Foglio").ToString
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("Numero")) Then
                    sDatiStampa += "|" + myRow("Numero").ToString
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("Subalterno")) Then
                    sDatiStampa += "|" + myRow("Subalterno").ToString
                Else
                    sDatiStampa += "|"
                End If
                '*** ***
                '*** 20141211 - legami PF-PV ***
                sDatiStampa += "|" + myRow("descrTipoPartita").ToString
                '*** ***
                If Not IsDBNull(myRow("DescrCategoria")) Then
                    sDatiStampa += "|" + myRow("DescrCategoria").ToString
                    If myRow("DescrCategoria").ToString.ToUpper.StartsWith("DOM") Then
                        sDatiStampa += "|" + myRow("noccupanti").ToString
                    Else
                        sDatiStampa += "|"
                    End If
                Else
                    sDatiStampa += "||"
                End If
                If Not IsDBNull(myRow("MQ")) Then
                    nTotMq += CDbl(myRow("MQ"))
                    If CDbl(myRow("MQ")) > 0 Then
                        sDatiStampa += "|" + myRow("MQ").ToString
                    Else
                        sDatiStampa += "|"
                    End If
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("tempo")) Then
                    sDatiStampa += "|" + myRow("tempo").ToString
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("Tariffa")) Then
                    sDatiStampa += "|" + myRow("Tariffa").ToString
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("importo")) Then
                    sDatiStampa += "|" + FormatNumber(myRow("importo").ToString, 2)
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("importo_Riduzioni")) Then
                    impTotRid += CDbl(myRow("importo_Riduzioni"))
                    If CDbl(myRow("importo_Riduzioni")) <> 0 Then
                        sDatiStampa += "|" + FormatNumber(myRow("importo_Riduzioni").ToString, 2)
                    Else
                        sDatiStampa += "|"
                    End If
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("importo_Detassazioni")) Then
                    impTotDet += CDbl(myRow("importo_Detassazioni"))
                    If CDbl(myRow("importo_Detassazioni")) <> 0 Then
                        sDatiStampa += "|" + FormatNumber(myRow("importo_Detassazioni").ToString, 2)
                    Else
                        sDatiStampa += "|"
                    End If
                Else
                    sDatiStampa += "|"
                End If
                Select Case myRow("TipoPartita").ToString
                    Case ObjArticolo.PARTEFISSA
                        'sDatiStampa += "|Imposta"
                        If myRow("ubicazione").ToString.ToUpper.IndexOf(" EMESSO") < 0 Then
                            If Not IsDBNull(myRow("importo")) Then
                                impPF += CDbl(myRow("importo"))
                                impTotArticoli += CDbl(myRow("importo"))
                            End If
                            If Not IsDBNull(myRow("importo_Netto")) Then
                                impTotNetto += FormatNumber(myRow("importo_Netto").ToString, 2)
                            End If
                            If Not IsDBNull(myRow("importo_lordo_pv")) Then
                                impPV += CDbl(myRow("importo_lordo_pv"))
                                impTotArticoli += CDbl(myRow("importo_lordo_pv"))
                            End If
                            If Not IsDBNull(myRow("importo_Netto_pv")) Then
                                impTotNetto += FormatNumber(myRow("importo_Netto_pv").ToString, 2)
                            End If
                        Else
                            If Not IsDBNull(myRow("totnetto")) Then
                                impPrec += CDbl(myRow("totnetto"))
                            End If
                        End If
                    Case ObjArticolo.PARTECONFERIMENTI
                        'sDatiStampa += "|Variabile Tessere"
                        If Not IsDBNull(myRow("totnetto")) Then
                            If myRow("ubicazione").ToString.ToUpper.IndexOf(" EMESSO") < 0 Then
                                impPC += CDbl(myRow("totnetto"))
                            Else
                                impPrec += CDbl(myRow("totnetto"))
                            End If
                        End If
                    Case ObjArticolo.PARTEMAGGIORAZIONE
                        'sDatiStampa += "|Maggiorazione"
                        If Not IsDBNull(myRow("totnetto")) Then
                            impPM += CDbl(myRow("totnetto"))
                        End If
                End Select
                If Not IsDBNull(myRow("importo_Netto")) Then
                    sDatiStampa += "|" + FormatNumber(myRow("importo_Netto").ToString, 2)
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("Tariffa_pv")) Then
                    sDatiStampa += "|" + myRow("Tariffa_pv").ToString
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("importo_lordo_pv")) Then
                    sDatiStampa += "|" + FormatNumber(myRow("importo_lordo_pv").ToString, 2)
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("importo_Riduzioni_pv")) Then
                    impTotRid += CDbl(myRow("importo_Riduzioni_pv"))
                    If CDbl(myRow("importo_Riduzioni_pv")) > 0 Then
                        sDatiStampa += "|" + FormatNumber(myRow("importo_Riduzioni_pv").ToString, 2)
                    Else
                        sDatiStampa += "|"
                    End If
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("importo_Detassazioni_pv")) Then
                    impTotDet += CDbl(myRow("importo_Detassazioni_pv"))
                    If CDbl(myRow("importo_Detassazioni_pv")) > 0 Then
                        sDatiStampa += "|" + FormatNumber(myRow("importo_Detassazioni_pv").ToString, 2)
                    Else
                        sDatiStampa += "|"
                    End If
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("importo_Netto_pv")) Then
                    sDatiStampa += "|" + FormatNumber(myRow("importo_Netto_pv").ToString, 2)
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("totnetto")) Then
                    sDatiStampa += "|" + FormatNumber(CDbl(myRow("totnetto")).ToString, 2)
                Else
                    sDatiStampa += "|"
                End If
                sDatiStampa += "|'"
                If Not IsDBNull(myRow("Riduzioni")) Then
                    sDatiStampa += myRow("Riduzioni").ToString
                End If
                If IsMinutaXStampatore = 1 Then
                    sDatiStampa += "|"
                    If Not IsDBNull(myRow("rata1")) Then
                        sDatiStampa += myRow("rata1").ToString
                    End If
                    sDatiStampa += "|"
                    If Not IsDBNull(myRow("rata2")) Then
                        sDatiStampa += myRow("rata2").ToString
                    End If
                    sDatiStampa += "|"
                    If Not IsDBNull(myRow("rata3")) Then
                        sDatiStampa += myRow("rata3").ToString
                    End If
                    sDatiStampa += "|"
                    If Not IsDBNull(myRow("ratau")) Then
                        sDatiStampa += myRow("ratau").ToString
                    End If
                End If
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                IdContribPrec = myRow("IdContribuente")
            Next
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco i totalizzatori
            If PrintMinutaTotali(nTotContribuenti, nTotArticoli, nTotMq, impPF, impPV, impTotArticoli, impTotRid, impTotDet, impTotNetto, impPC, impPM, impPrec, impECA, impMECA, impProvincialePerEnte, impAggioPerEnte, impProvinciale, impSpese, impSanzioni, impTotale, nCampi, DtStampa) = False Then
                Return Nothing
            End If
            Return DtStampa
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsStampaXLS.PrintMinutaRuolo.errore: ", Err)
            Return Nothing
        End Try
    End Function
    '*** ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oListAvvisi"></param>
    ''' <param name="HasConferimenti"></param>
    ''' <param name="HasMaggiorazione"></param>
    ''' <param name="sIntestazioneEnte"></param>
    ''' <param name="hasAnagAllRow"></param>
    ''' <param name="nCampi"></param>
    ''' <param name="IsMinutaXStampatore"></param>
    ''' <param name="TipoRuolo"></param>
    ''' <returns>DataTable</returns>
    ''' <revisionHistory>
    ''' <revision date="09/01/2013">
    ''' devo testare se ho le addizionali perchè in caso di avviso a zero non ci sono
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="23/01/2013">
    ''' aggiunti i riferimenti catastali
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="04/11/2013">
    ''' <strong>TARES</strong>
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="11/12/2014">
    ''' legami PF-PV
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="11/2015">
    ''' <strong>PF e PV su unica riga</strong>
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="09/2018">
    ''' <strong>Bollettazione Vigliano in OPENgov</strong>
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Esportazione completa dati</em>
    ''' </revision>
    ''' </revisionHistory>
    Public Function PrintMinutaRuolo(ByVal oListAvvisi() As ObjAvviso, ByVal HasConferimenti As Boolean, ByVal HasMaggiorazione As Boolean, ByVal sIntestazioneEnte As String, ByVal hasAnagAllRow As Integer, nCampi As Integer, IsMinutaXStampatore As Integer, TipoRuolo As String) As DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim myAvviso As New ObjAvviso
        Dim myArticolo As New ObjArticolo
        Dim x, IdContribPrec As Integer
        Dim CartellaPrec As String
        Dim nTotContribuenti As Integer = 0
        Dim nTotArticoli As Integer = 0
        Dim nTotMq As Double = 0
        Dim impTotPF, impTotPV, impTotPC, impTotPM, impTotArticoli, impTotRid, impTotDet, impTotNetto, impTotPrec, impTotECA, impTotMECA, impTotProvincialePerEnte, impTotAggioPerEnte, impTotProvinciale, impTotSpese, impTotSanzioni, impTotale As Double
        Dim sAvanzamento As String
        Dim nAvanzamento As Integer = 0

        Try
            impTotPF = 0 : impTotPV = 0 : impTotPC = 0 : impTotPM = 0 : impTotArticoli = 0 : impTotRid = 0 : impTotDet = 0 : impTotNetto = 0 : impTotPrec = 0 : impTotECA = 0 : impTotMECA = 0 : impTotProvincialePerEnte = 0 : impTotAggioPerEnte = 0 : impTotProvinciale = 0 : impTotSpese = 0 : impTotSanzioni = 0 : impTotale = 0
            CartellaPrec = ""
            'carico il dataset
            DsStampa.Tables.Add("STAMPA")
            'carico le colonne nel dataset
            For x = 0 To nCampi + 1
                DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
            Next
            'carico il datatable
            DtStampa = DsStampa.Tables("STAMPA")
            'inserisco l'intestazione dell'ente
            sDatiStampa = sIntestazioneEnte
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco l'intestazione del report
            sDatiStampa = "Minuta Ruolo Anno " & oListAvvisi(0).sAnnoRiferimento
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco le intestazioni di colonna
            sDatiStampa = ""
            sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.Iva"
            If IsMinutaXStampatore = 1 Then
                sDatiStampa += "|Data Nascita|Comune Nascita|PV Nascita|Sesso"
            End If
            sDatiStampa += "|Indirizzo Residenza|Civico Residenza|CAP Residenza|Comune Residenza|Provincia Residenza"
            If IsMinutaXStampatore = 1 Then
                sDatiStampa += "|Nominativo Invio|Indirizzo Invio|Civico Invio|CAP Invio|Comune Invio|Provincia Invio"
                sDatiStampa += "|N.Avviso"
                sDatiStampa += "|Imp.Netto"
                sDatiStampa += "|Imp.Addizionali EX ECA|Imp.Tributo EX ECA|Imp.Aggio per Ente|Imp.Addizionale Provinciale per Ente|Imp.Addizionale Provinciale"
                If HasMaggiorazione Then
                    sDatiStampa += "|Imp.Maggiorazione"
                End If
                If HasConferimenti Then
                    sDatiStampa += "|Imp.Conferimenti"
                End If
                sDatiStampa += "|Imp.Arrotondamento|Imp.Totale|Imp.Originale"
                If TipoRuolo = ObjRuolo.Ruolo.CartelleInsoluti Then
                    sDatiStampa += "|Imp.Sanzioni|Imp.Spese Notifica"
                End If
                sDatiStampa += "|Tipo Utenza"
            End If
            sDatiStampa += "|Anno|Ubicazione|Civico|Esponente|Interno"
            sDatiStampa += "|Foglio|Numero|Subalterno"
            sDatiStampa += "|Tipo Partita|Categoria|N.Componenti|MQ|Tempo"
            sDatiStampa += "|Tariffa|Imp.Articoli (tassa base)|Imp.Riduzioni|Imp.Detassazioni|Imp.Netto"
            sDatiStampa += "|Riduzioni"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'ciclo sui dati da stampare
            For Each myAvviso In oListAvvisi
                nAvanzamento += 1
                sAvanzamento = "Scrittura posizione " & nAvanzamento & " su " & oListAvvisi.GetUpperBound(0) + 1
                CacheManager.SetAvanzamentoElaborazione(sAvanzamento)
                nTotContribuenti += 1
                For Each myArticolo In myAvviso.oArticoli
                    sDatiStampa = ""
                    nTotArticoli += 1
                    If myAvviso.IdContribuente <> IdContribPrec Or IsMinutaXStampatore = 1 Or hasAnagAllRow = 1 Then
                        sDatiStampa += myAvviso.sCognome
                        sDatiStampa += "|" + myAvviso.sNome
                        sDatiStampa += "|'" + myAvviso.sCodFiscale.ToUpper()
                        If IsMinutaXStampatore = 1 Then
                            If myAvviso.tDataNascita <> DateTime.MaxValue Then
                                sDatiStampa += "|" + myAvviso.tDataNascita
                            Else
                                sDatiStampa += "|"
                            End If
                            sDatiStampa += "|" + myAvviso.sComuneNascita
                            sDatiStampa += "|" + myAvviso.sPVNascita
                            sDatiStampa += "|" + myAvviso.sSesso
                        End If
                        sDatiStampa += "|" + myAvviso.sIndirizzoRes
                        If myAvviso.sCivicoRes <> "-1" Then
                            sDatiStampa += "|'" + myAvviso.sCivicoRes
                        Else
                            sDatiStampa += "|"
                        End If
                        sDatiStampa += "|" + myAvviso.sCAPRes
                        sDatiStampa += "|" + myAvviso.sComuneRes
                        sDatiStampa += "|" + myAvviso.sProvRes
                        If IsMinutaXStampatore = 1 Then
                            Dim y As Integer
                            Dim impNetto As Double = 0
                            Dim impECA As Double = 0
                            Dim impMECA As Double = 0
                            Dim impAggioPerEnte As Double = 0
                            Dim impProvincialePerEnte As Double = 0
                            Dim impProvinciale As Double = 0
                            Dim impSpese As Double = 0
                            Dim impSanzioni As Double = 0

                            sDatiStampa += "|'" + myAvviso.sNominativoCO
                            sDatiStampa += "|" + myAvviso.sIndirizzoCO
                            If myAvviso.sCivicoCO <> "-1" Then
                                sDatiStampa += "|'" + myAvviso.sCivicoCO
                            Else
                                sDatiStampa += "|"
                            End If
                            sDatiStampa += "|" + myAvviso.sCAPCO
                            sDatiStampa += "|" + myAvviso.sComuneCO
                            sDatiStampa += "|" + myAvviso.sProvCO

                            sDatiStampa += "|'" + myAvviso.sCodiceCartella
                            For Each myArt As ObjArticolo In myAvviso.oArticoli
                                If myArt.TipoPartita.ToUpper.IndexOf("MAGG") < 0 Then
                                    If myArt.TipoPartita.ToUpper.IndexOf("CREDIT") < 0 Then
                                        impNetto += myArt.impNetto
                                    Else
                                        impNetto -= myArt.impNetto
                                    End If
                                End If
                            Next
                            If Not IsNothing(myAvviso.oDetVoci) Then
                                For y = 0 To myAvviso.oDetVoci.GetUpperBound(0)
                                    Select Case myAvviso.oDetVoci(y).sCapitolo
                                        Case ObjDetVoci.Capitolo.ProvincialeEnte
                                            impProvincialePerEnte += myAvviso.oDetVoci(y).impDettaglio
                                        Case ObjDetVoci.Capitolo.AggioEnte
                                            impAggioPerEnte += myAvviso.oDetVoci(y).impDettaglio
                                        Case ObjDetVoci.Capitolo.ECA
                                            impECA += myAvviso.oDetVoci(y).impDettaglio
                                        Case ObjDetVoci.Capitolo.MECA
                                            impMECA += myAvviso.oDetVoci(y).impDettaglio
                                        Case ObjDetVoci.Capitolo.Provinciale
                                            impProvinciale += myAvviso.oDetVoci(y).impDettaglio
                                        Case ObjDetVoci.Capitolo.SpeseNotifica
                                            impSpese += myAvviso.oDetVoci(y).impDettaglio
                                        Case ObjDetVoci.Capitolo.Sanzione
                                            impSanzioni += myAvviso.oDetVoci(y).impDettaglio
                                        Case Else
                                    End Select
                                Next
                            Else
                                impProvincialePerEnte += 0
                                impAggioPerEnte += 0
                                impECA += 0
                                impMECA += 0
                                impProvinciale += 0
                                impSpese += 0
                                impSanzioni += 0
                            End If
                            sDatiStampa += "|" + FormatNumber(impNetto.ToString, 2)
                            sDatiStampa += "|" + FormatNumber(impECA.ToString, 2)
                            sDatiStampa += "|" + FormatNumber(impMECA.ToString, 2)
                            sDatiStampa += "|" + FormatNumber(impAggioPerEnte.ToString, 2)
                            sDatiStampa += "|" + FormatNumber(impProvincialePerEnte.ToString, 2)
                            sDatiStampa += "|" + FormatNumber(impProvinciale.ToString, 2)

                            If myAvviso.sCodiceCartella <> CartellaPrec Then
                                impTotNetto += impNetto
                                impTotECA += impECA
                                impTotMECA += impMECA
                                impTotAggioPerEnte += impAggioPerEnte
                                impTotProvincialePerEnte += impProvincialePerEnte
                                impTotProvinciale += impProvinciale
                                impTotSpese += impSpese
                                impTotSanzioni += impSanzioni
                                impTotale += FormatNumber(myAvviso.impCarico.ToString, 2)
                            End If

                            If HasMaggiorazione Then
                                impTotPM += FormatNumber(myAvviso.impPM.ToString, 2)
                                sDatiStampa += "|" + FormatNumber(myAvviso.impPM.ToString, 2)
                            End If
                            If HasConferimenti Then
                                impTotPC += FormatNumber(myAvviso.impPC.ToString, 2)
                                sDatiStampa += "|" + FormatNumber(myAvviso.impPC.ToString, 2)
                            End If
                            sDatiStampa += "|" + FormatNumber(myAvviso.impArrotondamento.ToString, 2)
                            sDatiStampa += "|" + FormatNumber(myAvviso.impCarico.ToString, 2)
                            If myAvviso.impPRESgravio >= 0 Then
                                sDatiStampa += "|" + FormatNumber(myAvviso.impPRESgravio.ToString, 2)
                            Else
                                sDatiStampa += "|"
                            End If
                            If TipoRuolo = ObjRuolo.Ruolo.CartelleInsoluti Then
                                sDatiStampa += "|" + FormatNumber(impSanzioni.ToString, 2)
                                sDatiStampa += "|" + FormatNumber(impSpese.ToString, 2)
                            End If
                            sDatiStampa += "|" + myArticolo.sIdTipoUnita
                        End If
                    Else
                        sDatiStampa += "|||||||"
                    End If
                    sDatiStampa += "|" + myAvviso.sAnnoRiferimento
                    sDatiStampa += "|" + myArticolo.sVia
                    sDatiStampa += "|'" + myArticolo.sCivico
                    sDatiStampa += "|" & " " & myArticolo.sEsponente
                    sDatiStampa += "|" & " " & myArticolo.sInterno & " " & myArticolo.sScala
                    sDatiStampa += "|" + myArticolo.sFoglio
                    sDatiStampa += "|" + myArticolo.sNumero
                    sDatiStampa += "|" + myArticolo.sSubalterno
                    sDatiStampa += "|" + myArticolo.TipoPartita
                    If myArticolo.TipoPartita.ToUpper.StartsWith("IMPOSTA") Then
                        If myArticolo.sVia.ToUpper.IndexOf(" EMESSO") < 0 Then
                            impTotPF += myArticolo.impRuolo
                        Else
                            impTotPrec += myArticolo.impNetto
                        End If
                    Else
                        impTotPV += myArticolo.impRuolo
                    End If
                    '*** ***
                    sDatiStampa += "|" + myArticolo.sDescrCategoria
                    If myArticolo.sDescrCategoria.ToUpper.StartsWith("DOM") Then
                        sDatiStampa += "|" + myArticolo.nComponenti.ToString
                    Else
                        sDatiStampa += "|"
                    End If
                    nTotMq += myArticolo.nMQ
                    If myArticolo.nMQ > 0 Then
                        sDatiStampa += "|" + myArticolo.nMQ.ToString
                    Else
                        sDatiStampa += "|"
                    End If
                    sDatiStampa += "|" + myArticolo.nBimestri.ToString
                    sDatiStampa += "|" + myArticolo.impTariffa.ToString
                    impTotArticoli += myArticolo.impRuolo
                    sDatiStampa += "|" + FormatNumber(myArticolo.impRuolo, 2)
                    impTotRid += myArticolo.impRiduzione
                    If myArticolo.impRiduzione > 0 Then
                        sDatiStampa += "|" + FormatNumber(myArticolo.impRiduzione, 2)
                    Else
                        sDatiStampa += "|"
                    End If
                    impTotDet += myArticolo.impDetassazione
                    If myArticolo.impDetassazione > 0 Then
                        sDatiStampa += "|" + FormatNumber(myArticolo.impDetassazione, 2)
                    Else
                        sDatiStampa += "|"
                    End If
                    sDatiStampa += "|" + FormatNumber(myArticolo.impNetto, 2)
                    sDatiStampa += "|'"
                    Dim oRid As New ObjRidEseApplicati
                    If Not myArticolo.oRiduzioni Is Nothing Then
                        For Each oRid In myArticolo.oRiduzioni
                            sDatiStampa += "-" + oRid.sDescrizione + " " + oRid.sDescrTipo + " " + oRid.sValore
                        Next
                    End If
                    If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                        Return Nothing
                    End If
                    IdContribPrec = myAvviso.IdContribuente
                    CartellaPrec = myAvviso.sCodiceCartella
                Next
            Next
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco i totalizzatori
            If PrintMinutaTotali(nTotContribuenti, nTotArticoli, nTotMq, impTotPF, impTotPV, impTotArticoli, impTotRid, impTotDet, impTotNetto, impTotPC, impTotPM, impTotPrec, impTotECA, impTotMECA, impTotProvincialePerEnte, impTotAggioPerEnte, impTotProvinciale, impTotSpese, impTotSanzioni, impTotale, nCampi, DtStampa) = False Then
                Return Nothing
            End If
            Return DtStampa
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsStampaXLS.PrintMinutaRuolo.errore: ", Err)
            Return Nothing
        End Try
    End Function
    'Public Function PrintMinutaRuolo(ByVal oListAvvisi() As ObjAvviso, ByVal HasConferimenti As Boolean, ByVal HasMaggiorazione As Boolean, ByVal sIntestazioneEnte As String, ByVal hasAnagAllRow As Integer, nCampi As Integer, IsMinutaXStampatore As Integer, TipoRuolo As String) As DataTable
    '    Dim sDatiStampa As String
    '    Dim DsStampa As New DataSet
    '    Dim DtStampa As New DataTable
    '    Dim myAvviso As New ObjAvviso
    '    Dim myArticolo As New ObjArticolo
    '    Dim x, IdContribPrec As Integer
    '    Dim CartellaPrec As String
    '    Dim nTotContribuenti As Integer = 0
    '    Dim nTotArticoli As Integer = 0
    '    Dim nTotMq As Double = 0
    '    Dim impTotPF, impTotPV, impTotPC, impTotPM, impTotArticoli, impTotRid, impTotDet, impTotNetto, impTotPrec, impTotECA, impTotMECA, impTotProvincialePerEnte, impTotAggioPerEnte, impTotProvinciale, impTotSpese, impTotSanzioni, impTotale As Double
    '    Dim sAvanzamento As String
    '    Dim nAvanzamento As Integer = 0

    '    Try
    '        impTotPF = 0 : impTotPV = 0 : impTotPC = 0 : impTotPM = 0 : impTotArticoli = 0 : impTotRid = 0 : impTotDet = 0 : impTotNetto = 0 : impTotPrec = 0 : impTotECA = 0 : impTotMECA = 0 : impTotProvincialePerEnte = 0 : impTotAggioPerEnte = 0 : impTotProvinciale = 0 : impTotSpese = 0 : impTotSanzioni = 0 : impTotale = 0
    '        CartellaPrec = ""
    '        '*** 201511 - PF e PV su unica riga ***'*** 20130123 - aggiunti i riferimenti catastali ***
    '        'nCampi = 61 '54 '45 '39 '27 '24 '21
    '        'carico il dataset
    '        DsStampa.Tables.Add("STAMPA")
    '        'carico le colonne nel dataset
    '        For x = 0 To nCampi + 1
    '            DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
    '        Next
    '        'carico il datatable
    '        DtStampa = DsStampa.Tables("STAMPA")
    '        'inserisco l'intestazione dell'ente
    '        sDatiStampa = sIntestazioneEnte
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco una riga vuota
    '        sDatiStampa = ""
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco l'intestazione del report
    '        sDatiStampa = "Minuta Ruolo Anno " & oListAvvisi(0).sAnnoRiferimento
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco una riga vuota
    '        sDatiStampa = ""
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco le intestazioni di colonna
    '        sDatiStampa = ""
    '        sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.Iva"
    '        If IsMinutaXStampatore = 1 Then
    '            sDatiStampa += "|Data Nascita|Comune Nascita|PV Nascita|Sesso"
    '        End If
    '        sDatiStampa += "|Indirizzo Residenza|Civico Residenza|CAP Residenza|Comune Residenza|Provincia Residenza"
    '        If IsMinutaXStampatore = 1 Then
    '            sDatiStampa += "|Nominativo Invio|Indirizzo Invio|Civico Invio|CAP Invio|Comune Invio|Provincia Invio"
    '            sDatiStampa += "|N.Avviso"
    '            sDatiStampa += "|Imp.Netto"
    '            sDatiStampa += "|Imp.Addizionali EX ECA|Imp.Tributo EX ECA|Imp.Aggio per Ente|Imp.Addizionale Provinciale per Ente|Imp.Addizionale Provinciale"
    '            If HasMaggiorazione Then
    '                sDatiStampa += "|Imp.Maggiorazione"
    '            End If
    '            If HasConferimenti Then
    '                sDatiStampa += "|Imp.Conferimenti"
    '            End If
    '            sDatiStampa += "|Imp.Arrotondamento|Imp.Totale|Imp.Originale"
    '            If TipoRuolo = ObjRuolo.Ruolo.CartelleInsoluti Then
    '                sDatiStampa += "|Imp.Sanzioni|Imp.Spese Notifica"
    '            End If
    '            sDatiStampa += "|Tipo Utenza"
    '        End If
    '        sDatiStampa += "|Anno|Ubicazione|Civico|Esponente|Interno"
    '        '*** 20130123 - aggiunti i riferimenti catastali ***
    '        sDatiStampa += "|Foglio|Numero|Subalterno"
    '        '*** ***
    '        sDatiStampa += "|Tipo Partita|Categoria|N.Componenti|MQ|Tempo"
    '        sDatiStampa += "|Tariffa|Imp.Articoli (tassa base)|Imp.Riduzioni|Imp.Detassazioni|Imp.Netto"
    '        sDatiStampa += "|Riduzioni"
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'ciclo sui dati da stampare
    '        For Each myAvviso In oListAvvisi
    '            nAvanzamento += 1
    '            sAvanzamento = "Scrittura posizione " & nAvanzamento & " su " & oListAvvisi.GetUpperBound(0) + 1
    '            CacheManager.SetAvanzamentoElaborazione(sAvanzamento)
    '            nTotContribuenti += 1
    '            For Each myArticolo In myAvviso.oArticoli
    '                sDatiStampa = ""
    '                nTotArticoli += 1
    '                If myAvviso.IdContribuente <> IdContribPrec Or IsMinutaXStampatore = 1 Or hasAnagAllRow = 1 Then
    '                    sDatiStampa += myAvviso.sCognome
    '                    sDatiStampa += "|" + myAvviso.sNome
    '                    sDatiStampa += "|'" + myAvviso.sCodFiscale.ToUpper()
    '                    If IsMinutaXStampatore = 1 Then
    '                        If myAvviso.tDataNascita <> DateTime.MaxValue Then
    '                            sDatiStampa += "|" + myAvviso.tDataNascita
    '                        Else
    '                            sDatiStampa += "|"
    '                        End If
    '                        sDatiStampa += "|" + myAvviso.sComuneNascita
    '                        sDatiStampa += "|" + myAvviso.sPVNascita
    '                        sDatiStampa += "|" + myAvviso.sSesso
    '                    End If
    '                    sDatiStampa += "|" + myAvviso.sIndirizzoRes
    '                    If myAvviso.sCivicoRes <> "-1" Then
    '                        sDatiStampa += "|'" + myAvviso.sCivicoRes
    '                    Else
    '                        sDatiStampa += "|"
    '                    End If
    '                    'sDatiStampa += "|" & MyAvviso.sEsponenteRes
    '                    'sDatiStampa += "|" & MyAvviso.sInternoRes & " " & MyAvviso.sScalaRes
    '                    sDatiStampa += "|" + myAvviso.sCAPRes
    '                    sDatiStampa += "|" + myAvviso.sComuneRes
    '                    sDatiStampa += "|" + myAvviso.sProvRes
    '                    If IsMinutaXStampatore = 1 Then
    '                        Dim y As Integer
    '                        Dim impNetto As Double = 0
    '                        Dim impECA As Double = 0
    '                        Dim impMECA As Double = 0
    '                        Dim impAggioPerEnte As Double = 0
    '                        Dim impProvincialePerEnte As Double = 0
    '                        Dim impProvinciale As Double = 0
    '                        Dim impSpese As Double = 0
    '                        Dim impSanzioni As Double = 0

    '                        sDatiStampa += "|'" + myAvviso.sNominativoCO
    '                        sDatiStampa += "|" + myAvviso.sIndirizzoCO
    '                        If myAvviso.sCivicoCO <> "-1" Then
    '                            sDatiStampa += "|'" + myAvviso.sCivicoCO
    '                        Else
    '                            sDatiStampa += "|"
    '                        End If
    '                        sDatiStampa += "|" + myAvviso.sCAPCO
    '                        sDatiStampa += "|" + myAvviso.sComuneCO
    '                        sDatiStampa += "|" + myAvviso.sProvCO

    '                        sDatiStampa += "|'" + myAvviso.sCodiceCartella
    '                        For Each myArt As ObjArticolo In myAvviso.oArticoli
    '                            If myArt.TipoPartita.ToUpper.IndexOf("MAGG") < 0 Then
    '                                If myArt.TipoPartita.ToUpper.IndexOf("CREDIT") < 0 Then
    '                                    impNetto += myArt.impNetto
    '                                Else
    '                                    impNetto -= myArt.impNetto
    '                                End If
    '                            End If
    '                        Next
    '                        If Not IsNothing(myAvviso.oDetVoci) Then           '*** 20130109 - devo testare se ho le addizionali perchè in caso di avviso a zero non ci sono ***
    '                            For y = 0 To myAvviso.oDetVoci.GetUpperBound(0)
    '                                Select Case myAvviso.oDetVoci(y).sCapitolo
    '                                    Case ObjDetVoci.Capitolo.ProvincialeEnte
    '                                        impProvincialePerEnte += myAvviso.oDetVoci(y).impDettaglio
    '                                    Case ObjDetVoci.Capitolo.AggioEnte
    '                                        impAggioPerEnte += myAvviso.oDetVoci(y).impDettaglio
    '                                    Case ObjDetVoci.Capitolo.ECA
    '                                        impECA += myAvviso.oDetVoci(y).impDettaglio
    '                                    Case ObjDetVoci.Capitolo.MECA
    '                                        impMECA += myAvviso.oDetVoci(y).impDettaglio
    '                                    Case ObjDetVoci.Capitolo.Provinciale
    '                                        impProvinciale += myAvviso.oDetVoci(y).impDettaglio
    '                                    Case ObjDetVoci.Capitolo.SpeseNotifica
    '                                        impSpese += myAvviso.oDetVoci(y).impDettaglio
    '                                    Case ObjDetVoci.Capitolo.Sanzione
    '                                        impSanzioni += myAvviso.oDetVoci(y).impDettaglio
    '                                    Case Else
    '                                End Select
    '                            Next
    '                        Else
    '                            impProvincialePerEnte += 0
    '                            impAggioPerEnte += 0
    '                            impECA += 0
    '                            impMECA += 0
    '                            impProvinciale += 0
    '                            impSpese += 0
    '                            impSanzioni += 0
    '                        End If
    '                        sDatiStampa += "|" + FormatNumber(impNetto.ToString, 2)
    '                        sDatiStampa += "|" + FormatNumber(impECA.ToString, 2)
    '                        sDatiStampa += "|" + FormatNumber(impMECA.ToString, 2)
    '                        sDatiStampa += "|" + FormatNumber(impAggioPerEnte.ToString, 2)
    '                        sDatiStampa += "|" + FormatNumber(impProvincialePerEnte.ToString, 2)
    '                        sDatiStampa += "|" + FormatNumber(impProvinciale.ToString, 2)

    '                        If myAvviso.sCodiceCartella <> CartellaPrec Then
    '                            impTotNetto += impNetto
    '                            impTotECA += impECA
    '                            impTotMECA += impMECA
    '                            impTotAggioPerEnte += impAggioPerEnte
    '                            impTotProvincialePerEnte += impProvincialePerEnte
    '                            impTotProvinciale += impProvinciale
    '                            impTotSpese += impSpese
    '                            imptotsanzioni += impSanzioni
    '                            impTotale += FormatNumber(myAvviso.impCarico.ToString, 2)
    '                        End If

    '                        If HasMaggiorazione Then
    '                                impTotPM += FormatNumber(myAvviso.impPM.ToString, 2)
    '                                sDatiStampa += "|" + FormatNumber(myAvviso.impPM.ToString, 2)
    '                            End If
    '                            If HasConferimenti Then
    '                                impTotPC += FormatNumber(myAvviso.impPC.ToString, 2)
    '                                sDatiStampa += "|" + FormatNumber(myAvviso.impPC.ToString, 2)
    '                            End If
    '                            sDatiStampa += "|" + FormatNumber(myAvviso.impArrotondamento.ToString, 2)
    '                            sDatiStampa += "|" + FormatNumber(myAvviso.impCarico.ToString, 2)
    '                        If myAvviso.impPRESgravio >= 0 Then
    '                            sDatiStampa += "|" + FormatNumber(myAvviso.impPRESgravio.ToString, 2)
    '                        Else
    '                            sDatiStampa += "|"
    '                            End If
    '                            If TipoRuolo = ObjRuolo.Ruolo.CartelleInsoluti Then
    '                                sDatiStampa += "|" + FormatNumber(impSanzioni.ToString, 2)
    '                                sDatiStampa += "|" + FormatNumber(impSpese.ToString, 2)
    '                            End If
    '                            sDatiStampa += "|" + myArticolo.sIdTipoUnita
    '                        End If
    '                    Else
    '                    sDatiStampa += "|||||||"
    '                End If
    '                sDatiStampa += "|" + myAvviso.sAnnoRiferimento
    '                sDatiStampa += "|" + myArticolo.sVia
    '                sDatiStampa += "|'" + myArticolo.sCivico
    '                sDatiStampa += "|" & " " & myArticolo.sEsponente
    '                sDatiStampa += "|" & " " & myArticolo.sInterno & " " & myArticolo.sScala
    '                '*** 20130123 - aggiunti i riferimenti catastali ***
    '                sDatiStampa += "|" + myArticolo.sFoglio
    '                sDatiStampa += "|" + myArticolo.sNumero
    '                sDatiStampa += "|" + myArticolo.sSubalterno
    '                '*** ***
    '                '*** 20141211 - legami PF-PV ***
    '                sDatiStampa += "|" + myArticolo.TipoPartita
    '                If myArticolo.TipoPartita.ToUpper.StartsWith("IMPOSTA") Then
    '                    If myArticolo.sVia.ToUpper.IndexOf(" EMESSO") < 0 Then
    '                        impTotPF += myArticolo.impRuolo
    '                    Else
    '                        impTotPrec += myArticolo.impNetto
    '                    End If
    '                Else
    '                    impTotPV += myArticolo.impRuolo
    '                End If
    '                '*** ***
    '                sDatiStampa += "|" + myArticolo.sDescrCategoria
    '                If myArticolo.sDescrCategoria.ToUpper.StartsWith("DOM") Then
    '                    sDatiStampa += "|" + myArticolo.nComponenti.ToString
    '                Else
    '                    sDatiStampa += "|"
    '                End If
    '                nTotMq += myArticolo.nMQ
    '                If myArticolo.nMQ > 0 Then
    '                    sDatiStampa += "|" + myArticolo.nMQ.ToString
    '                Else
    '                    sDatiStampa += "|"
    '                End If
    '                sDatiStampa += "|" + myArticolo.nBimestri.ToString
    '                sDatiStampa += "|" + myArticolo.impTariffa.ToString
    '                impTotArticoli += myArticolo.impRuolo
    '                sDatiStampa += "|" + FormatNumber(myArticolo.impRuolo, 2)
    '                impTotRid += myArticolo.impRiduzione
    '                If myArticolo.impRiduzione > 0 Then
    '                    sDatiStampa += "|" + FormatNumber(myArticolo.impRiduzione, 2)
    '                Else
    '                    sDatiStampa += "|"
    '                End If
    '                impTotDet += myArticolo.impDetassazione
    '                If myArticolo.impDetassazione > 0 Then
    '                    sDatiStampa += "|" + FormatNumber(myArticolo.impDetassazione, 2)
    '                Else
    '                    sDatiStampa += "|"
    '                End If
    '                sDatiStampa += "|" + FormatNumber(myArticolo.impNetto, 2)
    '                sDatiStampa += "|'"
    '                Dim oRid As New ObjRidEseApplicati
    '                If Not myArticolo.oRiduzioni Is Nothing Then
    '                    For Each oRid In myArticolo.oRiduzioni
    '                        sDatiStampa += "-" + oRid.sDescrizione + " " + oRid.sDescrTipo + " " + oRid.sValore
    '                    Next
    '                End If
    '                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '                    Return Nothing
    '                End If
    '                IdContribPrec = myAvviso.IdContribuente
    '                cartellaprec = myAvviso.sCodiceCartella
    '            Next
    '        Next
    '        'inserisco una riga vuota
    '        sDatiStampa = ""
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco i totalizzatori
    '        If PrintMinutaTotali(nTotContribuenti, nTotArticoli, nTotMq, impTotPF, impTotPV, impTotArticoli, impTotRid, impTotDet, impTotNetto, impTotPC, impTotPM, impTotPrec, impTotECA, impTotMECA, impTotProvincialePerEnte, impTotAggioPerEnte, impTotProvinciale, impTotSpese, imptotsanzioni, impTotale, nCampi, DtStampa) = False Then
    '            Return Nothing
    '        End If
    '        Return DtStampa
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsStampaXLS.PrintMinutaRuolo.errore: ", Err)
    '        Return Nothing
    '    End Try
    'End Function
    Public Function PrintMinutaTotali(nTotContribuenti As Integer, nArticoli As Integer, nMq As Double, impPF As Double, impPV As Double, impArticoli As Double, impRid As Double, impDet As Double, impNetto As Double, impPC As Double, impPM As Double, impPrec As Double, impECA As Double, impMECA As Double, impProvincialePerEnte As Double, impAggioPerEnte As Double, impProvinciale As Double, impSpese As Double, impSanzioni As Double, impTotale As Double, nCampi As Integer, ByRef DtStampa As DataTable) As Boolean
        Dim sDatiStampa As String
        Try
            sDatiStampa = "Tot.Contribuenti |" & nTotContribuenti.ToString
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return False
            End If
            sDatiStampa = "Tot.Articoli |" & nArticoli.ToString
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return False
            End If
            sDatiStampa = "Tot.Mq |" & nMq.ToString
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return False
            End If
            sDatiStampa = "Tot.Fissa TARES |" & FormatNumber(impPF, 2)
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return False
            End If
            sDatiStampa = "Tot.Variabile TARES |" & FormatNumber(impPV, 2)
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return False
            End If
            sDatiStampa = "Tot.Imp.Articoli |" & FormatNumber(impArticoli, 2)
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return False
            End If
            sDatiStampa = "Tot.Imp.Riduzioni |" & FormatNumber(impRid, 2)
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return False
            End If
            sDatiStampa = "Tot.Imp.Detassazioni |" & FormatNumber(impDet, 2)
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return False
            End If
            sDatiStampa = "Tot.Imp.Netto |" & FormatNumber(impNetto, 2)
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return False
            End If
            If impPC <> 0 Then
                sDatiStampa = "Tot.Variabile Tessere |" & FormatNumber(impPC, 2)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return False
                End If
            End If
            If impPM <> 0 Then
                sDatiStampa = "Tot.Maggiorazione |" & FormatNumber(impPM, 2)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return False
                End If
            End If
            If impPrec <> 0 Then
                sDatiStampa = "Tot.Imp.Già Emesso |" & FormatNumber(impPrec, 2)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return False
                End If
            End If
            If impECA <> 0 Then
                sDatiStampa = "Tot.Imp.Addizionali EX ECA |" & FormatNumber(impECA, 2)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return False
                End If
            End If
            If impMECA <> 0 Then
                sDatiStampa = "Tot.Imp.Tributo EX ECA |" & FormatNumber(impMECA, 2)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return False
                End If
            End If
            If impProvincialePerEnte <> 0 Then
                sDatiStampa = "Tot.Imp.Provinciale per Ente |" & FormatNumber(impProvincialePerEnte, 2)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return False
                End If
            End If
            If impProvinciale <> 0 Then
                sDatiStampa = "Tot.Imp.Provinciale |" & FormatNumber(impProvinciale, 2)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return False
                End If
            End If
            If impAggioPerEnte <> 0 Then
                sDatiStampa = "Tot.Imp.Aggio |" & FormatNumber(impAggioPerEnte, 2)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return False
                End If
            End If
            If impTotale <> 0 Then
                sDatiStampa = "Tot.Imp.Totale |" & FormatNumber(impTotale, 2)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return False
                End If
            End If
            If impSpese <> 0 Then
                sDatiStampa = "Tot.Imp.Spese |" & FormatNumber(impSpese, 2)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return False
                End If
            End If
            If impSanzioni <> 0 Then
                sDatiStampa = "Tot.Imp.Sanzioni |" & FormatNumber(impSanzioni, 2)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsStampaXLS.PrintMinutaTotali.errore: ", ex)
            Return False
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="DvDati"></param>
    ''' <param name="sIntestazioneEnte"></param>
    ''' <param name="sAnno"></param>
    ''' <param name="nCampi"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="09/2018">
    ''' <strong>Bollettazione Vigliano in OPENgov</strong>
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Esportazione completa dati</em>
    ''' </revision>
    ''' </revisionHistory>
    Public Function PrintMinutaRate(DvDati As DataView, ByVal sIntestazioneEnte As String, sAnno As String, nCampi As Integer) As DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim x As Integer
        Dim sAvanzamento As String
        Dim nAvanzamento As Integer = 0

        Try
            'carico il dataset
            DsStampa.Tables.Add("STAMPA")
            'carico le colonne nel dataset
            For x = 0 To nCampi + 1
                DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
            Next
            'carico il datatable
            DtStampa = DsStampa.Tables("STAMPA")
            'inserisco l'intestazione dell'ente
            sDatiStampa = sIntestazioneEnte
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco l'intestazione del report
            sDatiStampa = "Minuta Rate Anno " & sAnno
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            '*** 201809 ***
            'inserisco le intestazioni di colonna
            sDatiStampa = "N.Avviso"
            sDatiStampa += "|Cod.Tributo|Anno|Cod.Ente|Sezione|Acconto|Saldo|Num.Fab."
            sDatiStampa += "|Numero Rata|Descrizione Rata|Data Scadenza|Importo Rata"
            sDatiStampa += "|Codice Bollettino|Codeline|Barcode"
            sDatiStampa += "|Conto corrente|Descrizione Riga 1|Descrizione Riga 2"
            sDatiStampa += "|Tipo Utenza"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'ciclo sui dati da stampare
            For Each myRow As DataRowView In DvDati
                nAvanzamento += 1
                sAvanzamento = "Scrittura posizione " & nAvanzamento & " su " & DvDati.Count
                CacheManager.SetAvanzamentoElaborazione(sAvanzamento)
                sDatiStampa = printminutaraterowdati(myRow)
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
            Next
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            Return DtStampa
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsStampaXLS.PrintMinutaRate.errore: ", Err)
            Return Nothing
        End Try
    End Function
    'Public Function PrintMinutaRate(DvDati As DataView, ByVal sIntestazioneEnte As String, sAnno As String, nCampi As Integer) As DataTable
    '    Dim sDatiStampa As String
    '    Dim DsStampa As New DataSet
    '    Dim DtStampa As New DataTable
    '    Dim x As Integer
    '    Dim sAvanzamento As String
    '    Dim nAvanzamento As Integer = 0

    '    Try
    '        'carico il dataset
    '        DsStampa.Tables.Add("STAMPA")
    '        'carico le colonne nel dataset
    '        For x = 0 To nCampi + 1
    '            DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
    '        Next
    '        'carico il datatable
    '        DtStampa = DsStampa.Tables("STAMPA")
    '        'inserisco l'intestazione dell'ente
    '        sDatiStampa = sIntestazioneEnte
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco una riga vuota
    '        sDatiStampa = ""
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco l'intestazione del report
    '        sDatiStampa = "Minuta Rate Anno " & sAnno
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco una riga vuota
    '        sDatiStampa = ""
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        '*** 201809 Bollettazione Vigliano in OPENgov***
    '        'inserisco le intestazioni di colonna
    '        sDatiStampa = "N.Avviso"
    '        sDatiStampa += "|Cod.Tributo|Anno|Cod.Ente|Sezione|Acconto|Saldo|Num.Fab."
    '        sDatiStampa += "|Numero Rata|Descrizione Rata|Data Scadenza|Importo Rata"
    '        sDatiStampa += "|Codice Bollettino|Codeline|Barcode"
    '        sDatiStampa += "|Conto corrente|Descrizione Riga 1|Descrizione Riga 2"
    '        sDatiStampa += "|Tipo Utenza"
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'ciclo sui dati da stampare
    '        For Each myRow As DataRowView In DvDati
    '            nAvanzamento += 1
    '            sAvanzamento = "Scrittura posizione " & nAvanzamento & " su " & DvDati.Count
    '            CacheManager.SetAvanzamentoElaborazione(sAvanzamento)
    '            sDatiStampa = ""
    '            If Not IsDBNull(myRow("CODICE_CARTELLA")) Then
    '                sDatiStampa += "'" + myRow("CODICE_CARTELLA").ToString
    '            End If
    '            If Not IsDBNull(myRow("CODTRIBUTO")) Then
    '                sDatiStampa += "|'" + myRow("CODTRIBUTO").ToString
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("ANNO")) Then
    '                sDatiStampa += "|" + myRow("ANNO").ToString
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("COD_ENTE")) Then
    '                sDatiStampa += "|" + myRow("COD_ENTE").ToString
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("SEZIONE")) Then
    '                sDatiStampa += "|" + myRow("SEZIONE").ToString
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("ISACCONTO")) Then
    '                sDatiStampa += "|" + myRow("ISACCONTO").ToString
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("ISSALDO")) Then
    '                sDatiStampa += "|" + myRow("ISSALDO").ToString
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("NUMFABB")) Then
    '                sDatiStampa += "|" + myRow("NUMFABB").ToString
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("NUMERO_RATA")) Then
    '                sDatiStampa += "|'" + myRow("NUMERO_RATA").ToString
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("DESCRIZIONE_RATA")) Then
    '                sDatiStampa += "|" & " " & myRow("DESCRIZIONE_RATA").ToString
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("DATA_SCADENZA")) Then
    '                sDatiStampa += "|" & " " & myRow("DATA_SCADENZA").ToString
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("IMPORTO_RATA")) Then
    '                sDatiStampa += "|" + FormatNumber(myRow("IMPORTO_RATA").ToString, 2)
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("CODICE_BOLLETTINO")) Then
    '                sDatiStampa += "|'" & " " & myRow("CODICE_BOLLETTINO").ToString
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("CODELINE")) Then
    '                sDatiStampa += "|'" & " " & myRow("CODELINE").ToString
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("BARCODE")) Then
    '                sDatiStampa += "|'" & " " & myRow("BARCODE").ToString
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("CONTO_CORRENTE")) Then
    '                sDatiStampa += "|'" & " " & myRow("CONTO_CORRENTE").ToString
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("DESCRIZIONE_1_RIGA")) Then
    '                sDatiStampa += "|" & " " & myRow("DESCRIZIONE_1_RIGA").ToString
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("DESCRIZIONE_2_RIGA")) Then
    '                sDatiStampa += "|" & " " & myRow("DESCRIZIONE_2_RIGA").ToString
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            '*** 201809 Bollettazione Vigliano in OPENgov***
    '            If Not IsDBNull(myRow("tipoutenza")) Then
    '                sDatiStampa += "|" & " " & myRow("tipoutenza").ToString
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '                Return Nothing
    '            End If
    '        Next
    '        'inserisco una riga vuota
    '        sDatiStampa = ""
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        Return DtStampa
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsStampaXLS.PrintMinutaRate.errore: ", Err)
    '        Return Nothing
    '    End Try
    'End Function
    'Public Function PrintMinutaAvvisi(ByVal oListAvvisi() As ObjAvviso, ByVal sIntestazioneEnte As String, ByVal HasMaggiorazione As Boolean, ByVal HasConferimenti As Boolean, nCampi As Integer) As DataTable
    '    Dim sDatiStampa, sDatiStampaGen As String
    '    Dim DsStampa As New DataSet
    '    Dim DtStampa As New DataTable
    '    Dim x, IdContribPrec As Integer
    '    Dim nTotContribuenti As Integer = 0
    '    Dim nTotAvvisi As Integer = 0
    '    Dim nTotMq As Double = 0
    '    Dim impTotArticoli As Double = 0
    '    Dim impRid As Double = 0
    '    Dim impDet As Double = 0
    '    Dim impTotSanzioni As Double = 0
    '    Dim impTotInteressi As Double = 0
    '    Dim impTotSpeseNot As Double = 0
    '    Dim impTotNetto As Double = 0
    '    Dim impNetto As Double = 0
    '    Dim impTotECA As Double = 0
    '    Dim impECA As Double = 0
    '    Dim impTotMECA As Double = 0
    '    Dim impMECA As Double = 0
    '    Dim impTotAggio As Double = 0
    '    Dim impAggioPerEnte As Double = 0
    '    Dim impTotProvPerEnte As Double = 0
    '    Dim impProvincialePerEnte As Double = 0
    '    Dim impTotProvinciale As Double = 0
    '    Dim impProvinciale As Double = 0
    '    Dim impTotArrotondamento As Double = 0
    '    Dim impTotCarico As Double = 0
    '    Dim impTotPM As Double = 0
    '    Dim impTotPC As Double = 0
    '    Dim nDom As Integer = 0
    '    Dim nNonDom As Integer = 0
    '    Dim impDom As Double = 0
    '    Dim impNonDom As Double = 0
    '    Dim impTotDom As Double = 0
    '    Dim impTotNonDom As Double = 0

    '    Try
    '        'carico il  dataset
    '        DsStampa.Tables.Add("STAMPA")
    '        'carico le colonne nel dataset
    '        For x = 1 To nCampi + 1
    '            DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
    '        Next
    '        'carico il datatable
    '        DtStampa = DsStampa.Tables("STAMPA")
    '        'inserisco l'intestazione dell'ente
    '        sDatiStampa = sIntestazioneEnte
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco una riga vuota
    '        sDatiStampa = ""
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco l'intestazione del report
    '        sDatiStampa = "Minuta Avvisi Anno " & oListAvvisi(0).sAnnoRiferimento
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco una riga vuota
    '        sDatiStampa = ""
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco le intestazioni di colonna
    '        sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.Iva|"
    '        sDatiStampa += "N.Avviso|"
    '        If ConstSession.IsMinutaXStampatore = True Then
    '            sDatiStampa += "Imp.Domestiche Euro|Imp.Non Domestiche Euro|"
    '        End If
    '        sDatiStampa += "Imp.Fissa Euro|Imp.Variabile Euro|Imp.Riduzioni Euro|Imp.Detassazioni Euro|"
    '        sDatiStampa += "Imp.Netto Euro|"
    '        sDatiStampa += "Imp.Addizionali EX ECA Euro|Imp.Tributo EX ECA Euro|Imp.Aggio per Ente Euro|Imp.Addizionale Provinciale per Ente Euro|Imp.Addizionale Provinciale Euro|"
    '        If HasMaggiorazione Then
    '            sDatiStampa += "Imp.Maggiorazione|"
    '        End If
    '        If HasConferimenti Then
    '            sDatiStampa += "Imp.Conferimenti|"
    '        End If
    '        sDatiStampa += "Imp.Arrotondamento Euro|Imp.Totale Euro|Imp.Originale"
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'ciclo sui dati da stampare
    '        For Each myAvv As ObjAvviso In oListAvvisi
    '            sDatiStampaGen = "" : impDom = 0 : impNonDom = 0 : impRid = 0 : impDet = 0 : impECA = 0 : impMECA = 0 : impAggioPerEnte = 0 : impProvincialePerEnte = 0 : impProvinciale = 0 : impNetto = 0
    '            nTotAvvisi += 1
    '            nTotContribuenti += 1
    '            If Not IsNothing(myAvv.oDetVoci) Then           '*** 20130109 - devo testare se ho le addizionali perchè in caso di avviso a zero non ci sono ***
    '                For Each myDet As ObjDetVoci In myAvv.oDetVoci
    '                    Select Case myDet.sCapitolo
    '                        Case "0094"
    '                            impProvincialePerEnte += myDet.impDettaglio
    '                        Case "0095"
    '                            impAggioPerEnte += myDet.impDettaglio
    '                        Case "9986"
    '                            impECA += myDet.impDettaglio
    '                        Case "9987"
    '                            impMECA += myDet.impDettaglio
    '                        Case "9994"
    '                            impProvinciale += myDet.impDettaglio
    '                        Case "0000"
    '                            Select Case myDet.CodVoce
    '                                Case 1 'DOMESTICA
    '                                    nDom += 1
    '                                    impDom += myDet.impDettaglio
    '                                Case Else 'NON DOMESTICA
    '                                    nNonDom += 1
    '                                    impNonDom += myDet.impDettaglio
    '                            End Select
    '                    End Select
    '                Next
    '            Else
    '                impProvincialePerEnte += 0
    '                impAggioPerEnte += 0
    '                impECA += 0
    '                impMECA += 0
    '                impProvinciale += 0
    '            End If
    '            impNetto += myAvv.impPF
    '            impNetto += myAvv.impPV

    '            sDatiStampaGen += myAvv.sCognome
    '            sDatiStampaGen += "|" + myAvv.sNome
    '            If myAvv.sPIVA <> "" Then
    '                sDatiStampaGen += "|'" + myAvv.sPIVA
    '            Else
    '                sDatiStampaGen += "|'" + myAvv.sCodFiscale.ToUpper()
    '            End If
    '            sDatiStampaGen += "|'" + myAvv.sCodiceCartella
    '            'For y = 0 To myAvv.oArticoli.GetUpperBound(0)
    '            '    impNetto += myAvv.oArticoli(y).impNetto
    '            'Next
    '            If ConstSession.IsMinutaXStampatore = True Then
    '                sDatiStampaGen += "|" + FormatNumber(impDom, 2)
    '                sDatiStampaGen += "|" + FormatNumber(impNonDom, 2)
    '            End If
    '            sDatiStampaGen += "|" + FormatNumber(myAvv.impPF.ToString, 2)
    '            sDatiStampaGen += "|" + FormatNumber(myAvv.impPV.ToString, 2)
    '            sDatiStampaGen += "|" + FormatNumber(imprid.ToString, 2)
    '            sDatiStampaGen += "|" + FormatNumber(impdet.ToString, 2)
    '            sDatiStampaGen += "|" + FormatNumber(impNetto.ToString, 2)
    '            sDatiStampaGen += "|" + FormatNumber(impECA.ToString, 2)
    '            sDatiStampaGen += "|" + FormatNumber(impMECA.ToString, 2)
    '            sDatiStampaGen += "|" + FormatNumber(impAggioPerEnte.ToString, 2)
    '            sDatiStampaGen += "|" + FormatNumber(impProvincialePerEnte.ToString, 2)
    '            sDatiStampaGen += "|" + FormatNumber(impProvinciale.ToString, 2)
    '            If HasMaggiorazione Then
    '                sDatiStampaGen += "|" + FormatNumber(myAvv.impPM.ToString, 2)
    '            End If
    '            If HasConferimenti Then
    '                sDatiStampaGen += "|" + FormatNumber(myAvv.impPC.ToString, 2)
    '            End If
    '            sDatiStampaGen += "|" + FormatNumber(myAvv.impArrotondamento.ToString, 2)
    '            sDatiStampaGen += "|" + FormatNumber(myAvv.impCarico.ToString, 2)
    '            If myAvv.impPRESgravio >= 0 Then
    '                sDatiStampaGen += "|" + FormatNumber(myAvv.impPRESgravio.ToString, 2)
    '            Else
    '                sDatiStampaGen += "|"
    '            End If
    '            If AddRowStampa(DtStampa, sDatiStampaGen) = 0 Then
    '                Return Nothing
    '            End If

    '            impTotDom += impDom : impTotNonDom += impNonDom
    '            impTotNetto += impNetto
    '            impTotECA += impECA
    '            impTotMECA += impMECA
    '            impTotAggio += impAggioPerEnte
    '            impTotProvPerEnte += impProvincialePerEnte
    '            impTotProvinciale += impProvinciale
    '            impTotPM += myAvv.impPM
    '            impTotPC += myAvv.impPC
    '            impTotArrotondamento += myAvv.impArrotondamento
    '            impTotCarico += myAvv.impCarico
    '        Next
    '        'inserisco una riga vuota
    '        sDatiStampa = ""
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco i totalizzatori
    '        sDatiStampa = "Tot.Contribuenti " & nTotContribuenti.ToString
    '        sDatiStampa += "|Tot.Avvisi " & nTotAvvisi.ToString
    '        If ConstSession.IsMinutaXStampatore = True Then
    '            sDatiStampa += "|di cui Domestiche " & nDom.ToString
    '            sDatiStampa += "|Non Domestiche " & nNonDom.ToString
    '        End If
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        sDatiStampa = "Tot.Imp.Netto Euro " & FormatNumber(impTotNetto.ToString, 2)
    '        If ConstSession.IsMinutaXStampatore = True Then
    '            sDatiStampa += "|di cui Domestiche Euro " & impTotDom.ToString
    '            sDatiStampa += "|Non Domestiche Euro " & impTotNonDom.ToString
    '        End If
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        sDatiStampa = "Tot.Imp.Addizionali EX ECA Euro " & FormatNumber(impTotECA.ToString, 2)
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        sDatiStampa = "Tot.Imp.Tributo EX ECA Euro " & FormatNumber(impTotMECA.ToString, 2)
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        sDatiStampa = "Tot.Imp.Aggio per Ente Euro " & FormatNumber(impTotAggio.ToString, 2)
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        sDatiStampa = "Tot.Imp.Addizionale Provinciale per Ente Euro " & FormatNumber(impTotProvPerEnte.ToString, 2)
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        sDatiStampa = "Tot.Imp.Addizionale Provinciale Euro " & FormatNumber(impTotProvinciale.ToString, 2)
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        If HasMaggiorazione Then
    '            sDatiStampa = "Tot.Imp.Maggiorazione Euro " & FormatNumber(impTotPM.ToString, 2)
    '            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
    '            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '                Return Nothing
    '            End If
    '        End If
    '        If HasConferimenti Then
    '            sDatiStampa = "Tot.Imp.Conferimenti Euro " & FormatNumber(impTotPC.ToString, 2)
    '            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
    '            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '                Return Nothing
    '            End If
    '        End If
    '        sDatiStampa = "Tot.Imp.Arrotondamento Euro " & FormatNumber(impTotArrotondamento.ToString, 2)
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        sDatiStampa = "Tot.Imp.Totale Euro " & FormatNumber(impTotCarico.ToString, 2)
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        Return DtStampa
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsStampaXLS.PrintMinutaAvvisi.errore: ", Err)
    '        Return Nothing
    '    End Try
    'End Function
    Public Function PrintMinutaRateRowDati(myRow As DataRowView) As String
        Dim sDatiStampa As String = ""

        Try
            sDatiStampa += "'" + Utility.StringOperation.FormatString(myRow("CODICE_CARTELLA"))
            sDatiStampa += "|'" + Utility.StringOperation.FormatString(myRow("CODTRIBUTO"))
            sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("ANNO"))
            sDatiStampa += "|'" + Utility.StringOperation.FormatString(myRow("COD_ENTE"))
            sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("SEZIONE"))
            sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("ISACCONTO"))
            sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("ISSALDO"))
            sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("NUMFABB"))
            sDatiStampa += "|'" + Utility.StringOperation.FormatString(myRow("NUMERO_RATA"))
            sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("DESCRIZIONE_RATA"))
            sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("DATA_SCADENZA"))
            sDatiStampa += "|" + FormatNumber(Utility.StringOperation.FormatString(myRow("IMPORTO_RATA")), 2)
            sDatiStampa += "|'" + Utility.StringOperation.FormatString(myRow("CODICE_BOLLETTINO"))
            sDatiStampa += "|'" + Utility.StringOperation.FormatString(myRow("CODELINE"))
            sDatiStampa += "|'" + Utility.StringOperation.FormatString(myRow("BARCODE"))
            sDatiStampa += "|'" + Utility.StringOperation.FormatString(myRow("CONTO_CORRENTE"))
            sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("DESCRIZIONE_1_RIGA"))
            sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("DESCRIZIONE_2_RIGA"))
            sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("tipoutenza"))
        Catch Err As Exception
            Log.Debug(myRow("IdEnte") + " - OPENgovTIA.ClsStampaXLS.PrintMinutaRateRowDati.errore: ", Err)
            sDatiStampa = ""
        End Try
        Return sDatiStampa
    End Function
    Public Function PrintMinutaAvvisi(DvDati As DataView, ByVal sIntestazioneEnte As String, ByVal HasMaggiorazione As Boolean, ByVal HasConferimenti As Boolean, nCampi As Integer) As DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim x As Integer
        Dim nTotContribuenti As Integer = 0
        Dim nTotAvvisi As Integer = 0
        Dim impTotPF As Double = 0
        Dim impTotPV As Double = 0
        Dim impTotRid As Double = 0
        Dim impTotDet As Double = 0
        Dim impTotNetto As Double = 0
        Dim impTotECA As Double = 0
        Dim impTotMECA As Double = 0
        Dim impTotAggio As Double = 0
        Dim impTotProvPerEnte As Double = 0
        Dim impTotProvinciale As Double = 0
        Dim impTotArrotondamento As Double = 0
        Dim impTotCarico As Double = 0
        Dim impTotPM As Double = 0
        Dim impTotPC As Double = 0
        Dim nDom As Integer = 0
        Dim nNonDom As Integer = 0
        Dim impTotDom As Double = 0
        Dim impTotNonDom As Double = 0

        Try
            'carico il  dataset
            DsStampa.Tables.Add("STAMPA")
            'carico le colonne nel dataset
            For x = 1 To nCampi + 1
                DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
            Next
            'carico il datatable
            DtStampa = DsStampa.Tables("STAMPA")
            'inserisco l'intestazione dell'ente
            sDatiStampa = sIntestazioneEnte
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco l'intestazione del report
            sDatiStampa = "Minuta Avvisi Anno " & DvDati.Item(0)("anno")
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco le intestazioni di colonna
            sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.Iva|"
            sDatiStampa += "N.Avviso|"
            'sDatiStampa += "Imp.Domestiche Euro|Imp.Non Domestiche Euro|"
            sDatiStampa += "Imp.Fissa Euro|Imp.Variabile Lordo Euro|Imp.Riduzioni Euro|Imp.Detassazioni Euro|"
            sDatiStampa += "Imp.Variabile Netto Euro|"
            sDatiStampa += "Imp.Addizionali EX ECA Euro|Imp.Tributo EX ECA Euro|Imp.Aggio per Ente Euro|Imp.Addizionale Provinciale per Ente Euro|Imp.Addizionale Provinciale Euro|"
            If HasMaggiorazione Then
                sDatiStampa += "Imp.Maggiorazione|"
            End If
            If HasConferimenti Then
                sDatiStampa += "Imp.Conferimenti|"
            End If
            sDatiStampa += "Imp.Arrotondamento Euro|Imp.Totale Euro|Imp.Originale"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'ciclo sui dati da stampare
            For Each myRow As DataRowView In DvDati
                nTotAvvisi += 1
                nTotContribuenti += 1
                sDatiStampa = ""
                If Not IsDBNull(myRow("COGNOME")) Then
                    sDatiStampa += myRow("COGNOME").ToString
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("NOME")) Then
                    sDatiStampa += "|" + myRow("NOME").ToString
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("CFPIVA")) Then
                    sDatiStampa += "|'" + myRow("CFPIVA").ToString.ToUpper()
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("CODICE_CARTELLA")) Then
                    sDatiStampa += "|'" + myRow("CODICE_CARTELLA").ToString.ToUpper()
                Else
                    sDatiStampa += "|"
                End If
                'If Not IsDBNull(myRow("IMPORTO_DOM")) Then
                '    sDatiStampa += "|" + FormatNumber(myRow("IMPORTO_DOM").ToString, 2)
                '    impTotDom += CDbl(myRow("IMPORTO_DOM"))
                '    If CDbl(myRow("IMPORTO_DOM")) > 0 Then
                '        nDom += 1
                '    End If
                'Else
                '    sDatiStampa += "|"
                'End If
                'If Not IsDBNull(myRow("IMPORTO_NONDOM")) Then
                '    sDatiStampa += "|" + FormatNumber(myRow("IMPORTO_NONDOM").ToString, 2)
                '    impTotNonDom += CDbl(myRow("IMPORTO_NONDOM"))
                '    If CDbl(myRow("IMPORTO_NONDOM")) > 0 Then
                '        nNonDom += 1
                '    End If
                'Else
                '    sDatiStampa += "|"
                'End If
                If Not IsDBNull(myRow("IMPORTO_PF")) Then
                    sDatiStampa += "|" + FormatNumber(myRow("IMPORTO_PF").ToString, 2)
                    impTotPF += CDbl(myRow("IMPORTO_PF"))
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("IMPORTO_PV_LORDO")) Then
                    sDatiStampa += "|" + FormatNumber(myRow("IMPORTO_PV_LORDO").ToString, 2)
                    impTotPV += CDbl(myRow("IMPORTO_PV_LORDO"))
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("IMPORTO_RIDUZIONI")) Then
                    sDatiStampa += "|" + FormatNumber(myRow("IMPORTO_RIDUZIONI").ToString, 2)
                    impTotRid += CDbl(myRow("IMPORTO_RIDUZIONI"))
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("IMPORTO_DETASSAZIONI")) Then
                    sDatiStampa += "|" + FormatNumber(myRow("IMPORTO_DETASSAZIONI").ToString, 2)
                    impTotDet += CDbl(myRow("IMPORTO_DETASSAZIONI"))
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("IMPORTO_PV")) Then
                    sDatiStampa += "|" + FormatNumber(myRow("IMPORTO_PV").ToString, 2)
                    impTotNetto += CDbl(myRow("IMPORTO_PV"))
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("IMPORTO_ECA")) Then
                    sDatiStampa += "|" + FormatNumber(myRow("IMPORTO_ECA").ToString, 2)
                    impTotECA += CDbl(myRow("IMPORTO_ECA"))
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("IMPORTO_MECA")) Then
                    sDatiStampa += "|" + FormatNumber(myRow("IMPORTO_MECA").ToString, 2)
                    impTotMECA += CDbl(myRow("IMPORTO_MECA"))
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("IMPORTO_AGGIOPERENTE")) Then
                    sDatiStampa += "|" + FormatNumber(myRow("IMPORTO_AGGIOPERENTE").ToString, 2)
                    impTotAggio += CDbl(myRow("IMPORTO_AGGIOPERENTE"))
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("IMPORTO_PROVPERENTE")) Then
                    sDatiStampa += "|" + FormatNumber(myRow("IMPORTO_PROVPERENTE").ToString, 2)
                    impTotProvPerEnte += CDbl(myRow("IMPORTO_PROVPERENTE"))
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("IMPORTO_PROV")) Then
                    sDatiStampa += "|" + FormatNumber(myRow("IMPORTO_PROV").ToString, 2)
                    impTotProvinciale += CDbl(myRow("IMPORTO_PROV"))
                Else
                    sDatiStampa += "|"
                End If
                If HasMaggiorazione Then
                    If Not IsDBNull(myRow("IMPORTO_PM")) Then
                        sDatiStampa += "|" + FormatNumber(myRow("IMPORTO_PM").ToString, 2)
                        impTotPM += CDbl(myRow("IMPORTO_PM"))
                    Else
                        sDatiStampa += "|"
                    End If
                End If
                If HasConferimenti Then
                    If Not IsDBNull(myRow("IMPORTO_PC")) Then
                        sDatiStampa += "|" + FormatNumber(myRow("IMPORTO_PC").ToString, 2)
                        impTotPC += CDbl(myRow("IMPORTO_PC"))
                    Else
                        sDatiStampa += "|"
                    End If
                End If
                If Not IsDBNull(myRow("IMPORTO_ARROTONDAMENTO")) Then
                    sDatiStampa += "|" + FormatNumber(myRow("IMPORTO_ARROTONDAMENTO").ToString, 2)
                    impTotArrotondamento += CDbl(myRow("IMPORTO_ARROTONDAMENTO"))
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("IMPORTO_CARICO")) Then
                    sDatiStampa += "|" + FormatNumber(myRow("IMPORTO_CARICO").ToString, 2)
                    impTotCarico += CDbl(myRow("IMPORTO_CARICO"))
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("IMPORTO_PRE_SGRAVIO")) Then
                    If CDbl(myRow("IMPORTO_PRE_SGRAVIO")) >= 0 Then
                        sDatiStampa += "|" + FormatNumber(myRow("IMPORTO_PRE_SGRAVIO").ToString, 2)
                    Else
                        sDatiStampa += "|"
                    End If
                Else
                    sDatiStampa += "|"
                End If
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
            Next
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco i totalizzatori
            sDatiStampa = "Tot.Contribuenti|" & nTotContribuenti.ToString
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = "Tot.Avvisi|" & nTotAvvisi.ToString
            'sDatiStampa += "|di cui Domestiche " & nDom.ToString
            'sDatiStampa += "|Non Domestiche " & nNonDom.ToString
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = "Tot.Fissa Euro|" & FormatNumber(impTotPF.ToString, 2)
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = "Tot.Variabile Euro|" & FormatNumber(impTotPV.ToString, 2)
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = "Tot.Riduzione Euro|" & FormatNumber(impTotRid.ToString, 2)
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = "Tot.Detassazione Euro|" & FormatNumber(impTotDet.ToString, 2)
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = "Tot.Netto Euro|" & FormatNumber(impTotNetto.ToString, 2)
            'sDatiStampa += "|di cui Domestiche Euro|" & impTotDom.ToString
            'sDatiStampa += "|Non Domestiche Euro|" & impTotNonDom.ToString
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = "Tot.Addizionali EX ECA Euro|" & FormatNumber(impTotECA.ToString, 2)
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = "Tot.Tributo EX ECA Euro|" & FormatNumber(impTotMECA.ToString, 2)
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = "Tot.Aggio per Ente Euro|" & FormatNumber(impTotAggio.ToString, 2)
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = "Tot.Addizionale Provinciale per Ente Euro|" & FormatNumber(impTotProvPerEnte.ToString, 2)
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = "Tot.Addizionale Provinciale Euro|" & FormatNumber(impTotProvinciale.ToString, 2)
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            If HasMaggiorazione Then
                sDatiStampa = "Tot.Maggiorazione Euro|" & FormatNumber(impTotPM.ToString, 2)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
            End If
            If HasConferimenti Then
                sDatiStampa = "Tot.Conferimenti Euro|" & FormatNumber(impTotPC.ToString, 2)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
            End If
            sDatiStampa = "Tot.Arrotondamento Euro|" & FormatNumber(impTotArrotondamento.ToString, 2)
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = "Totale Euro|" & FormatNumber(impTotCarico.ToString, 2)
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            Return DtStampa
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsStampaXLS.PrintMinutaAvvisi.errore: ", Err)
            Return Nothing
        End Try
    End Function
    '*** ***

    'Public Function PrintTessere(ByVal oListUtentiTessere() As ObjUtenteTessere, ByVal sIntestazioneEnte As String) As DataTable
    '    Dim sDatiStampa, sDatiUtenteStampa As String
    '    Dim DsStampa As New DataSet
    '    Dim DtStampa As New DataTable
    '    Dim x, y As Integer

    '    Try
    '        'carico il dataset
    '        DsStampa.Tables.Add("STAMPA")
    '        'carico le colonne nel dataset
    '        DsStampa.Tables("STAMPA").Columns.Add("Cognome")
    '        DsStampa.Tables("STAMPA").Columns.Add("Nome")
    '        DsStampa.Tables("STAMPA").Columns.Add("CodFiscalePIva")
    '        DsStampa.Tables("STAMPA").Columns.Add("Via")
    '        DsStampa.Tables("STAMPA").Columns.Add("Civico")
    '        DsStampa.Tables("STAMPA").Columns.Add("CAP")
    '        DsStampa.Tables("STAMPA").Columns.Add("Comune")
    '        DsStampa.Tables("STAMPA").Columns.Add("PV")
    '        DsStampa.Tables("STAMPA").Columns.Add("CodUtente")
    '        DsStampa.Tables("STAMPA").Columns.Add("NTessera")
    '        DsStampa.Tables("STAMPA").Columns.Add("CodTessera")
    '        DsStampa.Tables("STAMPA").Columns.Add("DataAttivazione")
    '        DsStampa.Tables("STAMPA").Columns.Add("DataCessazione")
    '        DsStampa.Tables("STAMPA").Columns.Add("NoteTessera")
    '        'carico il datatable
    '        DtStampa = DsStampa.Tables("STAMPA")
    '        'inserisco l'intestazione dell'ente
    '        sDatiStampa = sIntestazioneEnte
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + 13, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco una riga vuota
    '        sDatiStampa = ""
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + 13, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco l'intestazione del report
    '        sDatiStampa = "Elenco Tessere"
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + 13, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco una riga vuota
    '        sDatiStampa = ""
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + 13, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco le intestazioni di colonna
    '        sDatiStampa = ""
    '        sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.Iva|Via|Civico|CAP|Citta'|Provincia|Cod.Utente|N.Tessera|Cod.Tessera|Data Rilascio|Data Cessazione|Note"
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'ciclo sui dati da stampare
    '        For x = 0 To oListUtentiTessere.GetUpperBound(0)
    '            sDatiUtenteStampa = oListUtentiTessere(x).sCognome
    '            sDatiUtenteStampa += "|" + oListUtentiTessere(x).sNome()
    '            sDatiUtenteStampa += "|'" + oListUtentiTessere(x).sCFPIva
    '            sDatiUtenteStampa += "|" + oListUtentiTessere(x).sVia
    '            sDatiUtenteStampa += "|" + oListUtentiTessere(x).sCivico & " " & oListUtentiTessere(x).sInterno & " " & oListUtentiTessere(x).sEsponente & " " & oListUtentiTessere(x).sScala
    '            sDatiUtenteStampa += "|" + oListUtentiTessere(x).sCAP
    '            sDatiUtenteStampa += "|" + oListUtentiTessere(x).sComune
    '            sDatiUtenteStampa += "|" + oListUtentiTessere(x).sPV()
    '            For y = 0 To oListUtentiTessere(x).oTessere.GetUpperBound(0)
    '                sDatiStampa = "|" + oListUtentiTessere(x).oTessere(y).sCodUtente
    '                sDatiStampa += "|" + oListUtentiTessere(x).oTessere(y).sCodInterno
    '                sDatiStampa += "|" + oListUtentiTessere(x).oTessere(y).sNumeroTessera.ToString
    '                sDatiStampa += "|" + oListUtentiTessere(x).oTessere(y).tDataRilascio.ToShortDateString
    '                If oListUtentiTessere(x).oTessere(y).tDataCessazione <> Date.MinValue Then
    '                    sDatiStampa += "|" + oListUtentiTessere(x).oTessere(y).tDataCessazione.ToShortDateString
    '                Else
    '                    sDatiStampa += "|"
    '                End If
    '                sDatiStampa += "|" + oListUtentiTessere(x).oTessere(y).sNote
    '                If AddRowStampa(DtStampa, sDatiUtenteStampa + sDatiStampa) = 0 Then
    '                    Return Nothing
    '                End If
    '                sDatiUtenteStampa = "|||||||"
    '            Next y
    '        Next
    '        Return DtStampa
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsStampaXLS.PrintTessere.errore: ", Err)
    '        Return Nothing
    '    End Try
    'End Function

    Public Function PrintRicercaPesature(ByVal oListUtentiPesature() As ObjUtentePesature, ByVal sIntestazioneEnte As String) As DataTable
        Dim nTotContribuenti, nTotConferimenti As Integer
        Dim nTotKG, nTotVolume As Double
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim x As Integer

        Try
            'carico il dataset
            DsStampa.Tables.Add("STAMPA")
            'carico le colonne nel dataset
            DsStampa.Tables("STAMPA").Columns.Add("Nominativo")
            DsStampa.Tables("STAMPA").Columns.Add("CodFiscalePIva")
            DsStampa.Tables("STAMPA").Columns.Add("Ubicazione")
            DsStampa.Tables("STAMPA").Columns.Add("NTessera")
            DsStampa.Tables("STAMPA").Columns.Add("CodUtente")
            DsStampa.Tables("STAMPA").Columns.Add("Codice")
            DsStampa.Tables("STAMPA").Columns.Add("TotKG")
            DsStampa.Tables("STAMPA").Columns.Add("TotVolume")
            DsStampa.Tables("STAMPA").Columns.Add("NConferimenti")
            'carico il datatable
            DtStampa = DsStampa.Tables("STAMPA")
            'inserisco l'intestazione dell'ente
            sDatiStampa = sIntestazioneEnte
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + 8, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + 8, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco l'intestazione del report
            sDatiStampa = "Elenco Pesature"
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + 8, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + 8, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco le intestazioni di colonna
            sDatiStampa = ""
            sDatiStampa = "Nominativo|Cod.Fiscale/P.Iva|Ubicazione|N.Tessera|Cod.Utente|Codice|Tot.KG|Tot.Volume|N.Conferimenti"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'ciclo sui dati da stampare
            For x = 0 To oListUtentiPesature.GetUpperBound(0)
                sDatiStampa = ""
                sDatiStampa += oListUtentiPesature(x).sCognome & " " & oListUtentiPesature(x).sNome
                sDatiStampa += "|" + oListUtentiPesature(x).sCFPIva
                sDatiStampa += "|" + oListUtentiPesature(x).sVia & " " & oListUtentiPesature(x).sCivico & " " & oListUtentiPesature(x).sInterno & " " & oListUtentiPesature(x).sEsponente & " " & oListUtentiPesature(x).sScala
                sDatiStampa += "|" + oListUtentiPesature(x).sNumTessera
                sDatiStampa += "|" + oListUtentiPesature(x).sCodUtente
                sDatiStampa += "|" + oListUtentiPesature(x).nCodTessera.ToString
                sDatiStampa += "|" + oListUtentiPesature(x).nTotKg.ToString
                sDatiStampa += "|" + oListUtentiPesature(x).nTotVolume.ToString
                sDatiStampa += "|" + oListUtentiPesature(x).nTotConferimenti.ToString
                nTotKG += oListUtentiPesature(x).nTotKg
                nTotVolume += oListUtentiPesature(x).nTotVolume
                nTotConferimenti += oListUtentiPesature(x).nTotConferimenti
                nTotContribuenti += 1
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
            Next
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + 8, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco i totalizzatori
            sDatiStampa = "Tot.Contribuenti " & nTotContribuenti.ToString
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + 8, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = "Tot.Conferimenti " & nTotConferimenti.ToString
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + 8, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = "Tot.KG " & nTotKG.ToString
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + 8, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = "Tot.Volume " & nTotVolume.ToString
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + 8, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            Return DtStampa
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsStampaXLS.PrintRicercaPesature.errore: ", Err)
            Return Nothing
        End Try
    End Function

    Public Function PrintDichiarazioniSintetico(ByVal DvDati As DataView, ByVal sIntestazioneEnte As String, ByVal IsFromTARES As String, nCampi As Integer) As DataTable
        Dim sDatiStampa As String = ""
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim x As Integer

        Try
            Log.Debug("PrintDichiarazioniSintetico::carico il dataset")
            '*** 201511 - Funzioni Sovracomunali ***
            'nCampi = 37 '33
            'carico il dataset
            DsStampa.Tables.Add("STAMPA")
            'carico le colonne nel dataset
            For x = 0 To nCampi + 1
                DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
            Next

            'carico il datatable
            DtStampa = DsStampa.Tables("STAMPA")
            'inserisco l'intestazione dell'ente
            sDatiStampa = sIntestazioneEnte
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco l'intestazione del report
            sDatiStampa = "Elenco Dichiarazioni TARSU Sintetico"
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco le intestazioni di colonna
            sDatiStampa = ""
            '*** 201511 - Funzioni Sovracomunali ***
            If ConstSession.IdEnte = "" Then
                sDatiStampa = "Ente|"
            End If
            '*** 20130325 - gestione mq tassabili per TARES ***
            sDatiStampa += "Cognome|Nome|Cod.Fiscale/P.Iva|"
            sDatiStampa += "Residente|Provenienza Dichiarazione|"
            If ConstSession.IsFromVariabile = "1" Then
                sDatiStampa += "Tipo Tessera|N.Tessera|Cod.Interno|Cod.Utente|Data Rilascio|Data Cessazione|"
            End If
            sDatiStampa += "Via|Civico|Esponente|Interno|Data Inizio Occupazione|Data Fine Occupazione|Foglio|Numero|Subalterno|"
            sDatiStampa += "Stato Occupazione|"
            sDatiStampa += "Categoria Catastale|MQ Totali|"
            If ConstSession.IsFromTARES = "1" Then
                sDatiStampa += "MQ Tassabili|Categoria Tariffaria|N.Componenti|N.Componenti PV|"
                '*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI & DIVERSA GESTIONE QUOTA VARIABILE***
                If Not ConstSession.HasDummyDich Then
                    sDatiStampa += "Forza Calcolo PV|"
                End If
                '*** ***
            End If
            sDatiStampa += "Tarsu Giornaliera|Giorni|Riduzioni|Agevolazioni|Occupazione/Detenzione|Singolo Nucleo|Destinazione d'uso|Tipo Unità"
            '*** ***
            sDatiStampa += "|Note"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            Log.Debug("PrintDichiarazioniSintetico::inserito intestazioni")
            If Not IsNothing(DvDati) Then
                'ciclo sui dati da stampare
                For Each myRow As DataRowView In DvDati
                    sDatiStampa = ""
                    If ConstSession.IdEnte = "" Then
                        If Not IsDBNull(myRow("DESCRIZIONE_ENTE")) Then
                            sDatiStampa += myRow("DESCRIZIONE_ENTE").ToString() + "|"
                        Else
                            sDatiStampa += "|"
                        End If
                    End If
                    If Not IsDBNull(myRow("COGNOME_DENOMINAZIONE")) Then
                        sDatiStampa += myRow("COGNOME_DENOMINAZIONE").ToString()
                    End If
                    If IsDBNull(myRow("NOME")) Then
                        sDatiStampa += "|"
                    Else
                        sDatiStampa += "|" + myRow("NOME").ToString().ToUpper()
                    End If
                    If IsDBNull(myRow("cfpiva")) Or myRow("cfpiva").ToString() = "" Then
                        sDatiStampa += "|"
                    Else
                        sDatiStampa += "|'" + myRow("cfpiva").ToString()
                    End If
                    If IsDBNull(myRow("residente")) Then
                        sDatiStampa += "|"
                    Else
                        sDatiStampa += "|'" + myRow("residente").ToString()
                    End If
                    If IsDBNull(myRow("provdich")) Then
                        sDatiStampa += "|"
                    Else
                        sDatiStampa += "|'" + myRow("provdich").ToString()
                    End If
                    If ConstSession.IsFromVariabile = "1" Then
                        If IsDBNull(myRow("TIPO_TESSERA")) Then
                            sDatiStampa += "|"
                        Else
                            sDatiStampa += "|' " + myRow("TIPO_TESSERA").ToString()
                        End If
                        If IsDBNull(myRow("NUMERO_TESSERA")) Then
                            sDatiStampa += "|"
                        Else
                            sDatiStampa += "|' " + myRow("NUMERO_TESSERA").ToString()
                        End If
                        If IsDBNull(myRow("CODICE_INTERNO")) Then
                            sDatiStampa += "|"
                        Else
                            sDatiStampa += "|' " + myRow("CODICE_INTERNO").ToString()
                        End If
                        If IsDBNull(myRow("CODICE_UTENTE")) Then
                            sDatiStampa += "|"
                        Else
                            sDatiStampa += "|' " + myRow("CODICE_UTENTE").ToString()
                        End If
                        If IsDBNull(myRow("DATA_RILASCIO")) Then
                            sDatiStampa += "| "
                        Else
                            sDatiStampa += "|" + CDate(myRow("DATA_RILASCIO").ToString())
                        End If
                        If IsDBNull(myRow("DATA_CESSAZIONE")) Then
                            sDatiStampa += "| "
                        Else
                            sDatiStampa += "|" + CDate(myRow("DATA_CESSAZIONE").ToString())
                        End If
                    End If
                    If IsDBNull(myRow("VIA")) Then
                        sDatiStampa += "| "
                    Else
                        sDatiStampa += "|" + myRow("VIA").ToString() + " "
                    End If
                    If IsDBNull(myRow("CIVICO")) Then
                        sDatiStampa += "| "
                    Else
                        If myRow("CIVICO").ToString() <> "-1" And myRow("CIVICO").ToString() <> "0" Then
                            sDatiStampa += "|'" + myRow("CIVICO").ToString() + " "
                        Else
                            sDatiStampa += "|"
                        End If
                    End If
                    If IsDBNull(myRow("ESPONENTE")) Then
                        sDatiStampa += "| "
                    Else
                        If myRow("ESPONENTE").ToString() <> "-1" And myRow("ESPONENTE").ToString() <> "0" Then
                            sDatiStampa += "|" + myRow("ESPONENTE").ToString()
                        Else
                            sDatiStampa += "|"
                        End If
                    End If
                    If IsDBNull(myRow("INTERNO")) Then
                        sDatiStampa += "| "
                    Else
                        If myRow("INTERNO").ToString() <> "-1" And myRow("INTERNO").ToString() <> "0" Then
                            sDatiStampa += "|" + myRow("INTERNO").ToString()
                        Else
                            sDatiStampa += "|"
                        End If
                    End If
                    If IsDBNull(myRow("DATA_INIZIO")) Then
                        sDatiStampa += "| "
                    Else
                        sDatiStampa += "|" + CDate(myRow("DATA_INIZIO").ToString())
                    End If
                    If IsDBNull(myRow("DATA_FINE")) Then
                        sDatiStampa += "| "
                    Else
                        sDatiStampa += "|" + CDate(myRow("DATA_FINE").ToString())
                    End If
                    If IsDBNull(myRow("FOGLIO")) Then
                        sDatiStampa += "| "
                    Else
                        sDatiStampa += "|" + myRow("FOGLIO").ToString()
                    End If
                    If IsDBNull(myRow("NUMERO")) Then
                        sDatiStampa += "| "
                    Else
                        sDatiStampa += "|" + myRow("NUMERO").ToString()
                    End If
                    If IsDBNull(myRow("SUBALTERNO")) Then
                        sDatiStampa += "| "
                    Else
                        sDatiStampa += "|" + myRow("SUBALTERNO").ToString()
                    End If
                    If IsDBNull(myRow("statooccupaz")) Then
                        sDatiStampa += "|"
                    Else
                        sDatiStampa += "|'" + myRow("statooccupaz").ToString()
                    End If
                    If IsDBNull(myRow("CODCATEGORIACATASTALE")) Then
                        sDatiStampa += "|"
                    Else
                        sDatiStampa += "|' " + myRow("CODCATEGORIACATASTALE").ToString()
                    End If
                    If IsDBNull(myRow("mqImmobile")) Then
                        sDatiStampa += "| "
                    Else
                        sDatiStampa += "|" + myRow("mqImmobile").ToString()
                    End If
                    '*** 20130325 - gestione mq tassabili per TARES ***
                    If ConstSession.IsFromTARES = "1" Then
                        If IsDBNull(myRow("MQTASSABILI")) Then
                            sDatiStampa += "|"
                        Else
                            sDatiStampa += "|" + myRow("MQTassabili").ToString()
                        End If
                        If IsDBNull(myRow("cattares")) Then
                            sDatiStampa += "| "
                        Else
                            sDatiStampa += "|" + myRow("cattares").ToString()
                        End If
                        If IsDBNull(myRow("ncomponenti")) Then
                            sDatiStampa += "| "
                        Else
                            sDatiStampa += "|" + myRow("ncomponenti").ToString()
                        End If
                        If IsDBNull(myRow("ncomponenti_pv")) Then
                            sDatiStampa += "| "
                        Else
                            sDatiStampa += "|" + myRow("ncomponenti_pv").ToString()
                        End If
                        '*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI & DIVERSA GESTIONE QUOTA VARIABILE***
                        If Not ConstSession.HasDummyDich Then
                            If IsDBNull(myRow("FORZA_CALCOLAPV")) Then
                                sDatiStampa += "|"
                            Else
                                sDatiStampa += "|" + myRow("FORZA_CALCOLAPV").ToString()
                            End If
                        End If
                        '*** ***
                    End If
                    '*** ***
                    If IsDBNull(myRow("GGTARSU")) Then
                        sDatiStampa += "|No|"
                    Else
                        sDatiStampa += "|Si|" + myRow("GGTARSU").ToString()
                    End If
                    If IsDBNull(myRow("idriduzione")) Then
                        sDatiStampa += "|"
                    Else
                        sDatiStampa += "|' " + myRow("idriduzione").ToString()
                    End If
                    If IsDBNull(myRow("iddetassazione")) Then
                        sDatiStampa += "|"
                    Else
                        sDatiStampa += "|' " + myRow("iddetassazione").ToString()
                    End If
                    If IsDBNull(myRow("DescTitoloOccupazione")) Then
                        sDatiStampa += "| "
                    Else
                        sDatiStampa += "|" + myRow("DescTitoloOccupazione").ToString()
                    End If
                    If IsDBNull(myRow("DescNaturaOccupante")) Then
                        sDatiStampa += "| "
                    Else
                        sDatiStampa += "|" + myRow("DescNaturaOccupante").ToString()
                    End If
                    If IsDBNull(myRow("DescTipoUnita")) Then
                        sDatiStampa += "| "
                    Else
                        sDatiStampa += "|" + myRow("DescTipoUnita").ToString()
                    End If
                    If IsDBNull(myRow("DescDestUso")) Then
                        sDatiStampa += "| "
                    Else
                        sDatiStampa += "|" + myRow("DescDestUso").ToString()
                    End If
                    If IsDBNull(myRow("notedich")) Then
                        sDatiStampa += "|"
                    Else
                        sDatiStampa += "|' " + myRow("notedich").ToString()
                    End If
                    If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                        Log.Debug("PrintDichiarazioniSintetico::ha dato errore in inserimento riga")
                        Return Nothing
                    End If
                Next
            Else
                Log.Debug("PrintDichiarazioniSintetico::dataset vuoto causa errore non posso stampare")
            End If

            Return DtStampa
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsStampaXLS.PrintDichiarazioniSintetico.errore: ", Err)
            Log.Debug("Si è verificato un errore in PrintDichiarazioniSintetico::" & Err.Message & "::DatiStampa::" & sDatiStampa)
            Return Nothing
        End Try
    End Function

    Public Function PrintDichiarazioniAnalitico(ByVal DvDati As DataView, ByVal sIntestazioneEnte As String, ByVal IsFromTARES As String, nCampi As Integer) As DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim x As Integer

        Try
            '*** 201511 - Funzioni Sovracomunali ***
            'nCampi = 35 '31
            'carico il dataset
            DsStampa.Tables.Add("STAMPA")
            'carico le colonne nel dataset
            For x = 0 To nCampi + 1
                DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
            Next

            'carico il datatable
            DtStampa = DsStampa.Tables("STAMPA")
            'inserisco l'intestazione dell'ente
            sDatiStampa = sIntestazioneEnte
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco l'intestazione del report
            sDatiStampa = "Elenco Dichiarazioni TARSU Analitico"
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco le intestazioni di colonna
            '*** 201511 - Funzioni Sovracomunali ***'*** 20130325 - gestione mq tassabili per TARES ***
            sDatiStampa = ""
            If ConstSession.IdEnte = "" Then
                sDatiStampa = "Ente|"
            End If
            sDatiStampa += "Cognome|Nome|Cod.Fiscale/P.Iva|"
            sDatiStampa += "Residente|Provenienza Dichiarazione|"
            If ConstSession.IsFromVariabile = "1" Then
                sDatiStampa += "Tipo Tessera|N.Tessera|Cod.Interno|Cod.Utente|Data Rilascio|Data Cessazione|"
            End If
            sDatiStampa += "Via|Civico|Esponente|Interno|Data Inizio Occupazione|Data Fine Occupazione|Foglio|Numero|Subalterno|"
            sDatiStampa += "Stato Occupazione|"
            If ConstSession.IsFromTARES = "1" Then
                sDatiStampa += "MQ Tassabili|"
            End If
            sDatiStampa += "Tipo Vano|MQ Vano|"
            If ConstSession.IsFromTARES = "1" Then
                sDatiStampa += "Vani Esenti|"
            End If
            sDatiStampa += "Categoria Tariffaria|"
            If ConstSession.IsFromTARES = "1" Then
                sDatiStampa += "N.Componenti|N.Componenti PV|"
                '*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI & DIVERSA GESTIONE QUOTA VARIABILE***
                If Not ConstSession.HasDummyDich Then
                    sDatiStampa += "Forza Calcolo PV|"
                End If
                '*** ***
            End If
            sDatiStampa += "Tarsu Giornaliera|Giorni|Riduzioni|Agevolazioni"
            '*** ***
            sDatiStampa += "|Note"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'ciclo sui dati da stampare
            For Each myRow As DataRowView In DvDati
                sDatiStampa = ""
                If ConstSession.IdEnte = "" Then
                    If Not IsDBNull(myRow("DESCRIZIONE_ENTE")) Then
                        sDatiStampa += myRow("DESCRIZIONE_ENTE").ToString() + "|"
                    Else
                        sDatiStampa += "|"
                    End If
                End If
                If Not IsDBNull(myRow("COGNOME_DENOMINAZIONE")) Then
                    sDatiStampa += myRow("COGNOME_DENOMINAZIONE").ToString()
                End If
                If IsDBNull(myRow("NOME")) Then
                    sDatiStampa += "|"
                Else
                    sDatiStampa += "|" + myRow("NOME").ToString().ToUpper()
                End If
                If IsDBNull(myRow("cfpiva")) Then
                    sDatiStampa += "|"
                Else
                    sDatiStampa += "|'" + myRow("cfpiva").ToString()
                End If
                If IsDBNull(myRow("residente")) Then
                    sDatiStampa += "|"
                Else
                    sDatiStampa += "|'" + myRow("residente").ToString()
                End If
                If IsDBNull(myRow("provdich")) Then
                    sDatiStampa += "|"
                Else
                    sDatiStampa += "|'" + myRow("provdich").ToString()
                End If
                If ConstSession.IsFromVariabile = "1" Then
                    If IsDBNull(myRow("TIPO_TESSERA")) Then
                        sDatiStampa += "|"
                    Else
                        sDatiStampa += "|' " + myRow("TIPO_TESSERA").ToString()
                    End If
                    If IsDBNull(myRow("NUMERO_TESSERA")) Then
                        sDatiStampa += "|"
                    Else
                        sDatiStampa += "|' " + myRow("NUMERO_TESSERA").ToString()
                    End If
                    If IsDBNull(myRow("CODICE_INTERNO")) Then
                        sDatiStampa += "|"
                    Else
                        sDatiStampa += "|' " + myRow("CODICE_INTERNO").ToString()
                    End If
                    If IsDBNull(myRow("CODICE_UTENTE")) Then
                        sDatiStampa += "|"
                    Else
                        sDatiStampa += "|' " + myRow("CODICE_UTENTE").ToString()
                    End If
                    If IsDBNull(myRow("DATA_RILASCIO")) Then
                        sDatiStampa += "| "
                    Else
                        sDatiStampa += "|" + CDate(myRow("DATA_RILASCIO").ToString())
                    End If
                    If IsDBNull(myRow("DATA_CESSAZIONE")) Then
                        sDatiStampa += "| "
                    Else
                        sDatiStampa += "|" + CDate(myRow("DATA_CESSAZIONE").ToString())
                    End If
                End If
                If IsDBNull(myRow("VIA")) Then
                    sDatiStampa += "|"
                Else
                    sDatiStampa += "|" + myRow("VIA").ToString() + " "
                End If
                If IsDBNull(myRow("CIVICO")) Then
                    sDatiStampa += "|"
                Else
                    If myRow("CIVICO").ToString() <> "-1" And myRow("CIVICO").ToString() <> "0" Then
                        sDatiStampa += "|'" + myRow("CIVICO").ToString() + " "
                    Else
                        sDatiStampa += "| "
                    End If
                End If
                If IsDBNull(myRow("ESPONENTE")) Then
                    sDatiStampa += "|"
                Else
                    If myRow("ESPONENTE").ToString() <> "-1" And myRow("ESPONENTE").ToString() <> "0" Then
                        sDatiStampa += "|" + myRow("ESPONENTE").ToString()
                    Else
                        sDatiStampa += "| "
                    End If
                End If
                If IsDBNull(myRow("INTERNO")) Then
                    sDatiStampa += "|"
                Else
                    If myRow("INTERNO").ToString() <> "-1" And myRow("INTERNO").ToString() <> "0" Then
                        sDatiStampa += "|" + myRow("INTERNO").ToString()
                    Else
                        sDatiStampa += "| "
                    End If
                End If
                If IsDBNull(myRow("DATA_INIZIO")) Then
                    sDatiStampa += "| "
                Else
                    sDatiStampa += "|" + CDate(myRow("DATA_INIZIO").ToString())
                End If
                If IsDBNull(myRow("DATA_FINE")) Then
                    sDatiStampa += "| "
                Else
                    sDatiStampa += "|" + CDate(myRow("DATA_FINE").ToString())
                End If
                If IsDBNull(myRow("FOGLIO")) Then
                    sDatiStampa += "|"
                Else
                    sDatiStampa += "|" + myRow("FOGLIO").ToString()
                End If
                If IsDBNull(myRow("NUMERO")) Then
                    sDatiStampa += "|"
                Else
                    sDatiStampa += "|" + myRow("NUMERO").ToString()
                End If
                If IsDBNull(myRow("SUBALTERNO")) Then
                    sDatiStampa += "|"
                Else
                    sDatiStampa += "|" + myRow("SUBALTERNO").ToString()
                End If
                If IsDBNull(myRow("statooccupaz")) Then
                    sDatiStampa += "|"
                Else
                    sDatiStampa += "|'" + myRow("statooccupaz").ToString()
                End If
                If ConstSession.IsFromTARES = "1" Then
                    If IsDBNull(myRow("MQTASSABILI")) Then
                        sDatiStampa += "|"
                    Else
                        sDatiStampa += "|" + myRow("mqtassabili").ToString()
                    End If
                End If
                If IsDBNull(myRow("DESCTIPOVANO")) Then
                    sDatiStampa += "|"
                Else
                    sDatiStampa += "|" + myRow("DescTipoVano").ToString()
                End If
                If IsDBNull(myRow("MQ")) Then
                    sDatiStampa += "|"
                Else
                    sDatiStampa += "|" + myRow("mq").ToString()
                End If
                If ConstSession.IsFromTARES = "1" Then
                    If IsDBNull(myRow("ESENTE")) Then
                        sDatiStampa += "|"
                    Else
                        If CBool(myRow("esente")) = True Then
                            sDatiStampa += "|X"
                        Else
                            sDatiStampa += "|"
                        End If
                    End If
                    If IsDBNull(myRow("cattares")) Then
                        sDatiStampa += "| "
                    Else
                        sDatiStampa += "|" + myRow("cattares").ToString()
                    End If
                    If IsDBNull(myRow("ncomponenti")) Then
                        sDatiStampa += "| "
                    Else
                        sDatiStampa += "|" + myRow("ncomponenti").ToString()
                    End If
                    If IsDBNull(myRow("ncomponenti_pv")) Then
                        sDatiStampa += "| "
                    Else
                        sDatiStampa += "|" + myRow("ncomponenti_pv").ToString()
                    End If
                    '*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI & DIVERSA GESTIONE QUOTA VARIABILE***
                    If Not ConstSession.HasDummyDich Then
                        sDatiStampa += "|" + myRow("FORZA_CALCOLAPV").ToString()
                    End If
                    '*** ***
                Else
                    If IsDBNull(myRow("DescCategorie")) Then
                        sDatiStampa += "| "
                    Else
                        sDatiStampa += "|" + myRow("DescCategorie").ToString()
                    End If
                End If
                If IsDBNull(myRow("GGTARSU")) Then
                    sDatiStampa += "|No|"
                Else
                    sDatiStampa += "|Si|" + myRow("GGTARSU").ToString()
                End If
                If IsDBNull(myRow("idriduzione")) Or myRow("idriduzione").ToString() = "" Then
                    sDatiStampa += "|"
                Else
                    sDatiStampa += "|' " + myRow("idriduzione").ToString()
                End If
                If IsDBNull(myRow("iddetassazione")) Or myRow("iddetassazione").ToString() = "" Then
                    sDatiStampa += "|"
                Else
                    sDatiStampa += "|' " + myRow("iddetassazione").ToString()
                End If
                If IsDBNull(myRow("notedich")) Then
                    sDatiStampa += "|"
                Else
                    sDatiStampa += "|' " + myRow("notedich").ToString()
                End If
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
            Next

            Return DtStampa
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsStampaXLS.PrintDichiarazioniAnalitico.errore: ", Err)
            Return Nothing
        End Try
    End Function
    Public Function PrintDichiarazioni(ByVal oListTestata() As ObjTestata, ByVal sIntestazioneEnte As String, ByVal IsFromTARES As String) As DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim x, y, z, w As Integer

        Try
            'carico il dataset
            DsStampa.Tables.Add("STAMPA")
            'carico gli colonne nel dataset
            For x = 1 To 24
                DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
            Next
            'carico il datatable
            DtStampa = DsStampa.Tables("STAMPA")
            'inserisco l'intestazione dell'ente
            sDatiStampa = sIntestazioneEnte
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + 23, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + 23, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco l'intestazione del report
            sDatiStampa = "Elenco Dichiarazioni"
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + 23, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + 23, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco le intestazioni di colonna
            sDatiStampa = ""
            sDatiStampa = "Nominativo|Cod.Fiscale/P.Iva|Indirizzo Residenza|Data Dichiarazione|N.Dichiarazione|Data Protocollo|N.Protocollo|Data Cessazione|Provenienza"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'ciclo sui dati da stampare
            For x = 0 To oListTestata.GetUpperBound(0)
                sDatiStampa = ""
                'inserisco i dati della testata
                sDatiStampa += oListTestata(x).oAnagrafe.Cognome & " " & oListTestata(x).oAnagrafe.Nome
                sDatiStampa += "|" + oListTestata(x).oAnagrafe.CodiceFiscale + oListTestata(x).oAnagrafe.PartitaIva
                sDatiStampa += "|" + oListTestata(x).oAnagrafe.ViaResidenza & " " & oListTestata(x).oAnagrafe.CivicoResidenza & " " & oListTestata(x).oAnagrafe.InternoCivicoResidenza & " " & oListTestata(x).oAnagrafe.EsponenteCivicoResidenza & " " & oListTestata(x).oAnagrafe.ScalaCivicoResidenza
                sDatiStampa += "|" + oListTestata(x).tDataDichiarazione.ToString
                sDatiStampa += "|" + oListTestata(x).sNDichiarazione
                If oListTestata(x).tDataProtocollo <> Date.MinValue Then
                    sDatiStampa += "|" + oListTestata(x).tDataProtocollo.ToString
                Else
                    sDatiStampa += "|"
                End If
                sDatiStampa += "|" + oListTestata(x).sNProtocollo
                If oListTestata(x).tDataCessazione <> Date.MinValue Then
                    sDatiStampa += "|" + oListTestata(x).tDataCessazione.ToString
                Else
                    sDatiStampa += "|"
                End If
                sDatiStampa += "|" + oListTestata(x).sIdProvenienza
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                'inserisco i dati di dettaglio
                If Not oListTestata(x).oTessere Is Nothing Then
                    For w = 0 To oListTestata(x).oTessere.GetUpperBound(0)
                        'inserisco le intestazioni di colonna
                        sDatiStampa = "|N.Tessera|Cod.Interno|Cod.Utente|Data Rilascio|Data Cessazione"
                        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                            Return Nothing
                        End If
                        sDatiStampa = "|" + oListTestata(x).oTessere(w).sNumeroTessera
                        sDatiStampa += "|" + oListTestata(x).oTessere(w).sCodInterno
                        sDatiStampa += "|" + oListTestata(x).oTessere(w).sCodUtente
                        If oListTestata(x).oTessere(w).tDataRilascio <> Date.MinValue Then
                            sDatiStampa += "|" + oListTestata(x).oTessere(w).tDataRilascio.ToString
                        Else
                            sDatiStampa += "|"
                        End If
                        If oListTestata(x).oTessere(w).tDataCessazione <> Date.MinValue Then
                            sDatiStampa += "|" + oListTestata(x).oTessere(w).tDataCessazione.ToString
                        Else
                            sDatiStampa += "|"
                        End If
                        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                            Return Nothing
                        End If
                        If Not oListTestata(x).oTessere(w).oImmobili Is Nothing Then
                            For y = 0 To oListTestata(x).oTessere(w).oImmobili.GetUpperBound(0)
                                'inserisco le intestazioni di colonna
                                sDatiStampa = "||Ubicazione|Foglio-Numero-Subalterno|Data Inizio|Data Fine|GG Tarsu|Stato Occupazione|Nome Proprietario|Nome Occupante Prec|Note"
                                '*** 20130325 - gestione mq tassabili per TARES ***
                                If IsFromTARES = "1" Then
                                    sDatiStampa = "||Ubicazione|Foglio-Numero-Subalterno|Data Inizio|Data Fine|MQ Catasto|MQ Tassabili|Cat.Ateco|N.Occupanti|GG Tarsu|Stato Occupazione|Nome Proprietario|Nome Occupante Prec|Note"
                                Else
                                    sDatiStampa = "||Ubicazione|Foglio-Numero-Subalterno|Data Inizio|Data Fine|GG Tarsu|Stato Occupazione|Nome Proprietario|Nome Occupante Prec|Note||||"
                                End If
                                '*** ***
                                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                                    Return Nothing
                                End If
                                sDatiStampa = "||" + oListTestata(x).oTessere(w).oImmobili(y).sVia & " " & oListTestata(x).oTessere(w).oImmobili(y).sCivico & " " & oListTestata(x).oTessere(w).oImmobili(y).sInterno & " " & oListTestata(x).oTessere(w).oImmobili(y).sEsponente & " " & oListTestata(x).oTessere(w).oImmobili(y).sScala
                                sDatiStampa += "|" + oListTestata(x).oTessere(w).oImmobili(y).sFoglio + "-" + oListTestata(x).oTessere(w).oImmobili(y).sNumero + "-" + oListTestata(x).oTessere(w).oImmobili(y).sSubalterno
                                sDatiStampa += "|" + oListTestata(x).oTessere(w).oImmobili(y).tDataInizio.ToString
                                If oListTestata(x).oTessere(w).oImmobili(y).tDataFine <> Date.MinValue Then
                                    sDatiStampa += "|" + oListTestata(x).oTessere(w).oImmobili(y).tDataFine.ToString
                                Else
                                    sDatiStampa += "|"
                                End If
                                '*** 20130325 - gestione mq tassabili per TARES ***
                                If IsFromTARES = "1" Then
                                    sDatiStampa += "|" + oListTestata(x).oTessere(w).oImmobili(y).nMQCatasto.ToString()
                                    sDatiStampa += "|" + oListTestata(x).oTessere(w).oImmobili(y).nMQTassabili.ToString()
                                    sDatiStampa += "|" + oListTestata(x).oTessere(w).oImmobili(y).sCatAteco
                                    sDatiStampa += "|" + oListTestata(x).oTessere(w).oImmobili(y).nNComponenti.ToString()
                                End If
                                '*** ***
                                If oListTestata(x).oTessere(w).oImmobili(y).nGGTarsu > 0 Then
                                    sDatiStampa += "|" + oListTestata(x).oTessere(w).oImmobili(y).nGGTarsu.ToString
                                Else
                                    sDatiStampa += "|"
                                End If
                                sDatiStampa += "|" + oListTestata(x).oTessere(w).oImmobili(y).sIdStatoOccupazione
                                sDatiStampa += "|" + oListTestata(x).oTessere(w).oImmobili(y).sNomeProprietario
                                sDatiStampa += "|" + oListTestata(x).oTessere(w).oImmobili(y).sNomeOccupantePrec
                                sDatiStampa += "|" + oListTestata(x).oTessere(w).oImmobili(y).sNoteUI
                                '*** 20130325 - gestione mq tassabili per TARES ***
                                If IsFromTARES <> "1" Then
                                    sDatiStampa += "||||"
                                End If
                                '*** ***
                                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                                    Return Nothing
                                End If
                                'inserisco i dati dei vani
                                'inserisco le intestazioni di colonna
                                '*** 20130325 - gestione mq tassabili per TARES ***
                                If IsFromTARES = "1" Then
                                    sDatiStampa = "|||Categoria|Tipo Vano|N.Vano|MQ|Esente"
                                Else
                                    sDatiStampa = "|||Categoria|Tipo Vano|N.Vano|MQ|"
                                End If
                                '*** ***
                                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                                    Return Nothing
                                End If
                                For z = 0 To oListTestata(x).oTessere(w).oImmobili(y).oOggetti.GetUpperBound(0)
                                    sDatiStampa = ""
                                    sDatiStampa += "|||" + oListTestata(x).oTessere(w).oImmobili(y).oOggetti(z).sCategoria
                                    sDatiStampa += "|" + oListTestata(x).oTessere(w).oImmobili(y).oOggetti(z).sTipoVano
                                    sDatiStampa += "|" + oListTestata(x).oTessere(w).oImmobili(y).oOggetti(z).nVani.ToString
                                    sDatiStampa += "|" + oListTestata(x).oTessere(w).oImmobili(y).oOggetti(z).nMq.ToString
                                    '*** 20130325 - gestione mq tassabili per TARES ***
                                    If IsFromTARES = "1" And oListTestata(x).oTessere(w).oImmobili(y).oOggetti(z).bIsEsente = True Then
                                        sDatiStampa += "|X"
                                    Else
                                        sDatiStampa += "|"
                                    End If
                                    '*** ***
                                    If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                                        Return Nothing
                                    End If
                                Next
                                'inserisco i dati delle riduzioni
                                If Not oListTestata(x).oTessere(w).oImmobili(y).oRiduzioni Is Nothing Then
                                    For z = 0 To oListTestata(x).oTessere(w).oImmobili(y).oRiduzioni.GetUpperBound(0)
                                        'inserisco le intestazioni di colonna
                                        sDatiStampa = "|||Riduzione:"
                                        sDatiStampa += "|" + oListTestata(x).oTessere(w).oImmobili(y).oRiduzioni(z).sDescrizione
                                        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                                            Return Nothing
                                        End If
                                    Next
                                End If
                                'inserisco i dati delle detrazioni
                                If Not oListTestata(x).oTessere(w).oImmobili(y).oDetassazioni Is Nothing Then
                                    For z = 0 To oListTestata(x).oTessere(w).oImmobili(y).oDetassazioni.GetUpperBound(0)
                                        'inserisco le intestazioni di colonna
                                        sDatiStampa = "|||Detrazione:"
                                        sDatiStampa += "|" + oListTestata(x).oTessere(w).oImmobili(y).oDetassazioni(z).sDescrizione
                                        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                                            Return Nothing
                                        End If
                                    Next
                                End If
                            Next
                        End If
                    Next
                End If
            Next
            Return DtStampa
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsStampaXLS.PrintDichiarazioni.errore: ", Err)
            Return Nothing
        End Try
    End Function
    '**** 201809 - Cartelle Insoluti ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="DvDati"></param>
    ''' <param name="sIntestazioneEnte"></param>
    ''' <param name="IsFromTARES"></param>
    ''' <param name="nCampi"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Esportazione completa dati</em>
    ''' </revision>
    ''' </revisionHistory>
    Public Function PrintAvvisi(ByVal DvDati As DataView, ByVal sIntestazioneEnte As String, ByVal IsFromTARES As String, nCampi As Integer) As DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim x As Integer

        Try
            'carico il dataset
            DsStampa.Tables.Add("STAMPA")
            'carico le colonne nel dataset
            For x = 0 To nCampi + 1
                DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
            Next

            'carico il datatable
            DtStampa = DsStampa.Tables("STAMPA")
            'inserisco l'intestazione dell'ente
            sDatiStampa = sIntestazioneEnte
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco l'intestazione del report
            sDatiStampa = "Elenco Avvisi"
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco le intestazioni di colonna
            sDatiStampa = ""
            If ConstSession.IdEnte = "" Then
                sDatiStampa = "Ente|"
            End If

            sDatiStampa += "Cognome|Nome|Cod.Fiscale/P.Iva"
            sDatiStampa += "|Anno|N.Avviso|Imp.Fissa|Imp.Variabile"
            If ConstSession.IsFromVariabile = "1" Then
                sDatiStampa += "|Imp.Conferimenti"
            End If
            sDatiStampa += "|Imp.Maggiorazione|Carico|Pagato"
            If ConstSession.HasNotifiche Then
                sDatiStampa += "|Data Notifica"
            End If
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'ciclo sui dati da stampare
            For Each myRow As DataRowView In DvDati
                sDatiStampa = PrintAvvisiRowDati(ConstSession.IdEnte, ConstSession.IsFromVariabile, ConstSession.HasNotifiche, myRow)
                If sDatiStampa <> "" Then
                    If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                        Return Nothing
                    End If
                Else
                    Return Nothing
                End If
            Next

            Return DtStampa
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsStampaXLS.PrintDichiarazioniAnalitico.errore: ", Err)
            Return Nothing
        End Try
    End Function
    'Public Function PrintAvvisi(ByVal DvDati As DataView, ByVal sIntestazioneEnte As String, ByVal IsFromTARES As String, nCampi As Integer) As DataTable
    '    Dim sDatiStampa As String
    '    Dim DsStampa As New DataSet
    '    Dim DtStampa As New DataTable
    '    Dim x As Integer

    '    Try
    '        'carico il dataset
    '        DsStampa.Tables.Add("STAMPA")
    '        'carico le colonne nel dataset
    '        For x = 0 To nCampi + 1
    '            DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
    '        Next

    '        'carico il datatable
    '        DtStampa = DsStampa.Tables("STAMPA")
    '        'inserisco l'intestazione dell'ente
    '        sDatiStampa = sIntestazioneEnte
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco una riga vuota
    '        sDatiStampa = ""
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco l'intestazione del report
    '        sDatiStampa = "Elenco Avvisi"
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco una riga vuota
    '        sDatiStampa = ""
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco le intestazioni di colonna
    '        sDatiStampa = ""
    '        If ConstSession.IdEnte = "" Then
    '            sDatiStampa = "Ente|"
    '        End If

    '        sDatiStampa += "Cognome|Nome|Cod.Fiscale/P.Iva"
    '        sDatiStampa += "|Anno|N.Avviso|Imp.Fissa|Imp.Variabile"
    '        If ConstSession.IsFromVariabile = "1" Then
    '            sDatiStampa += "|Imp.Conferimenti"
    '        End If
    '        sDatiStampa += "|Imp.Maggiorazione|Carico|Pagato"
    '        If ConstSession.HasNotifiche Then
    '            sDatiStampa += "|Data Notifica"
    '        End If
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'ciclo sui dati da stampare
    '        For Each myRow As DataRowView In DvDati
    '            sDatiStampa = ""
    '            If ConstSession.IdEnte = "" Then
    '                If Not IsDBNull(myRow("DESCRIZIONE_ENTE")) Then
    '                    sDatiStampa += myRow("DESCRIZIONE_ENTE").ToString() + "|"
    '                Else
    '                    sDatiStampa += "|"
    '                End If
    '            End If
    '            If Not IsDBNull(myRow("COGNOME")) Then
    '                sDatiStampa += myRow("COGNOME").ToString()
    '            End If
    '            If IsDBNull(myRow("NOME")) Then
    '                sDatiStampa += "|"
    '            Else
    '                sDatiStampa += "|" + myRow("NOME").ToString().ToUpper()
    '            End If
    '            If IsDBNull(myRow("cfpiva")) Then
    '                sDatiStampa += "|"
    '            Else
    '                sDatiStampa += "|'" + myRow("cfpiva").ToString()
    '            End If
    '            If IsDBNull(myRow("anno")) Then
    '                sDatiStampa += "|"
    '            Else
    '                sDatiStampa += "|" + myRow("anno").ToString()
    '            End If
    '            If IsDBNull(myRow("codice_cartella")) Then
    '                sDatiStampa += "|"
    '            Else
    '                sDatiStampa += "|'" + myRow("codice_cartella").ToString()
    '            End If
    '            If IsDBNull(myRow("importo_pf")) Then
    '                sDatiStampa += "|"
    '            Else
    '                sDatiStampa += "|" + FormatNumber(CStr(myRow("importo_pf")), 2)
    '            End If
    '            If IsDBNull(myRow("importo_pv")) Then
    '                sDatiStampa += "|"
    '            Else
    '                sDatiStampa += "|" + FormatNumber(CStr(myRow("importo_pv")), 2)
    '            End If
    '            If ConstSession.IsFromVariabile = "1" Then
    '                If IsDBNull(myRow("importo_pc")) Then
    '                    sDatiStampa += "|"
    '                Else
    '                    sDatiStampa += "|" + FormatNumber(CStr(myRow("importo_pc")), 2)
    '                End If
    '            End If
    '            If IsDBNull(myRow("importo_pm")) Then
    '                sDatiStampa += "|"
    '            Else
    '                sDatiStampa += "|" + FormatNumber(CStr(myRow("importo_pm")), 2)
    '            End If
    '            If IsDBNull(myRow("importo_carico")) Then
    '                sDatiStampa += "|"
    '            Else
    '                sDatiStampa += "|" + FormatNumber(CStr(myRow("importo_carico")), 2)
    '            End If
    '            If IsDBNull(myRow("pagato")) Then
    '                sDatiStampa += "|"
    '            Else
    '                sDatiStampa += "|" + FormatNumber(CStr(myRow("pagato")), 2)
    '            End If
    '            If ConstSession.HasNotifiche Then
    '                If IsDBNull(myRow("data_notifica")) Then
    '                    sDatiStampa += "|"
    '                Else
    '                    sDatiStampa += "|" + New Formatta.FunctionGrd().FormattaDataGrd(myRow("data_notifica"))
    '                End If
    '            End If
    '            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '                Return Nothing
    '            End If
    '        Next

    '        Return DtStampa
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsStampaXLS.PrintDichiarazioniAnalitico.errore: ", Err)
    '        Return Nothing
    '    End Try
    'End Function
    '*** 20120921 - pagamenti ***
    Public Function PrintAvvisiRowDati(IdEnte As String, IsFromVariabile As String, HasNotifiche As Boolean, myRow As DataRowView) As String
        Dim sDatiStampa As String = ""

        Try
            If IdEnte = "" Then
                If Not IsDBNull(myRow("DESCRIZIONE_ENTE")) Then
                    sDatiStampa += myRow("DESCRIZIONE_ENTE").ToString() + "|"
                Else
                    sDatiStampa += "|"
                End If
            End If
            If Not IsDBNull(myRow("COGNOME")) Then
                sDatiStampa += myRow("COGNOME").ToString()
            End If
            If IsDBNull(myRow("NOME")) Then
                sDatiStampa += "|"
            Else
                sDatiStampa += "|" + myRow("NOME").ToString().ToUpper()
            End If
            If IsDBNull(myRow("cfpiva")) Then
                sDatiStampa += "|"
            Else
                sDatiStampa += "|'" + myRow("cfpiva").ToString()
            End If
            If IsDBNull(myRow("anno")) Then
                sDatiStampa += "|"
            Else
                sDatiStampa += "|" + myRow("anno").ToString()
            End If
            If IsDBNull(myRow("codice_cartella")) Then
                sDatiStampa += "|"
            Else
                sDatiStampa += "|'" + myRow("codice_cartella").ToString()
            End If
            If IsDBNull(myRow("importo_pf")) Then
                sDatiStampa += "|"
            Else
                sDatiStampa += "|" + FormatNumber(CStr(myRow("importo_pf")), 2)
            End If
            If IsDBNull(myRow("importo_pv")) Then
                sDatiStampa += "|"
            Else
                sDatiStampa += "|" + FormatNumber(CStr(myRow("importo_pv")), 2)
            End If
            If IsFromVariabile = "1" Then
                If IsDBNull(myRow("importo_pc")) Then
                    sDatiStampa += "|"
                Else
                    sDatiStampa += "|" + FormatNumber(CStr(myRow("importo_pc")), 2)
                End If
            End If
            If IsDBNull(myRow("importo_pm")) Then
                sDatiStampa += "|"
            Else
                sDatiStampa += "|" + FormatNumber(CStr(myRow("importo_pm")), 2)
            End If
            If IsDBNull(myRow("importo_carico")) Then
                sDatiStampa += "|"
            Else
                sDatiStampa += "|" + FormatNumber(CStr(myRow("importo_carico")), 2)
            End If
            If IsDBNull(myRow("pagato")) Then
                sDatiStampa += "|"
            Else
                sDatiStampa += "|" + FormatNumber(CStr(myRow("pagato")), 2)
            End If
            If HasNotifiche Then
                If IsDBNull(myRow("data_notifica")) Then
                    sDatiStampa += "|"
                Else
                    sDatiStampa += "|" + New Formatta.FunctionGrd().FormattaDataGrd(myRow("data_notifica"))
                End If
            End If
        Catch ex As Exception
            Log.Debug(IdEnte + " - OPENgovTIA.ClsStampaXLS.PrintAvvisiRowDati.errore: ", ex)
            sDatiStampa = ""
        End Try
        Return sDatiStampa
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nTipoStampa">int definito da costanti di ClsGestPag</param>
    ''' <param name="IdTributo"></param>
    ''' <param name="dvDati"></param>
    ''' <param name="sIntestazioneEnte"></param>
    ''' <param name="sTitoloReport"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Esportazione completa dati</em>
    ''' </revision>
    ''' </revisionHistory>
    Public Function PrintPagamenti(ByVal nTipoStampa As Integer, IdTributo As String, ByVal dvDati As DataView, ByVal sIntestazioneEnte As String, ByVal sTitoloReport As String) As DataTable
        Dim sDatiStampa, sDatiStampaGen As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim x As Integer
        Dim nTotPag As Integer = 0
        Dim nCampi As Integer
        Dim impTotPagatoComune As Double = 0
        Dim impTotPagatoStat As Double = 0
        Dim impTotPagato As Double = 0
        Dim impTotEmessoComune As Double = 0
        Dim impTotEmessoStat As Double = 0
        Dim impTotEmesso As Double = 0

        Try
            nCampi = 13
            'carico il dataset
            DsStampa.Tables.Add("STAMPA")
            'carico le colonne nel dataset
            For x = 1 To nCampi + 1
                DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
            Next
            'carico il datatable
            DtStampa = DsStampa.Tables("STAMPA")
            'inserisco l'intestazione dell'ente
            sDatiStampa = sIntestazioneEnte
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco l'intestazione del report
            sDatiStampa = sTitoloReport
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco le intestazioni di colonna
            sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.Iva|"
            sDatiStampa += "Anno|N.Avviso"
            If IdTributo <> Utility.Costanti.TRIBUTO_OSAP And IdTributo <> Utility.Costanti.TRIBUTO_SCUOLE Then
                sDatiStampa += "|Imp.Emesso Comune|Imp.Emesso Maggiorazione"
            End If
            sDatiStampa += "|Imp.Emesso"
            Select Case nTipoStampa
                Case ClsGestPag.TipoStampaPagamenti
                    sDatiStampa += "|Data Accredito|Data Pagamento"
                    If IdTributo <> Utility.Costanti.TRIBUTO_OSAP And IdTributo <> Utility.Costanti.TRIBUTO_SCUOLE Then
                        sDatiStampa += "|Imp.Pagato Comume|Imp.Pagato Maggiorazione|Imp.Pagato"
                    Else
                        sDatiStampa += "|Imp.Pagato"
                    End If
                    sDatiStampa += "|Provenienza"
                Case ClsGestPag.TipoStampaNonPagato
                    sDatiStampa += ""
                Case Else
                    If IdTributo <> Utility.Costanti.TRIBUTO_OSAP And IdTributo <> Utility.Costanti.TRIBUTO_SCUOLE Then
                        sDatiStampa += "|Imp.Pagato Comume|Imp.Pagato Maggiorazione|Imp.Pagato"
                    Else
                        sDatiStampa += "|Imp.Pagato"
                    End If
            End Select
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'ciclo sui dati da stampare
            For x = 0 To dvDati.Count - 1
                sDatiStampaGen = PrintPagamentiRowDati(ConstSession.IdEnte, dvDati.Item(x), nTipoStampa, IdTributo)
                If AddRowStampa(DtStampa, sDatiStampaGen) = 0 Then
                    Return Nothing
                End If

                nTotPag += 1
                impTotEmessoComune += CDbl(dvDati.Item(x)("dovutocomune"))
                impTotEmessoStat += CDbl(dvDati.Item(x)("dovutostat"))
                impTotEmesso += CDbl(dvDati.Item(x)("dovuto"))
                impTotPagatoComune += CDbl(dvDati.Item(x)("importo_pagatocomune"))
                impTotPagatoStat += CDbl(dvDati.Item(x)("importo_pagatostat"))
                impTotPagato += CDbl(dvDati.Item(x)("importo_pagato"))
            Next
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco i totalizzatori
            If nTipoStampa = 0 Then
                sDatiStampa = "Tot.Pagamenti "
            Else
                sDatiStampa = "Tot.Avvisi "
            End If
            sDatiStampa += nTotPag.ToString
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            If IdTributo <> Utility.Costanti.TRIBUTO_OSAP And IdTributo <> Utility.Costanti.TRIBUTO_SCUOLE Then
                sDatiStampa = "Tot.Imp.Emesso Comune " & FormatNumber(impTotEmessoComune.ToString, 2)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                sDatiStampa = "Tot.Imp.Emesso Maggiorazione " & FormatNumber(impTotEmessoStat.ToString, 2)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
            End If
            sDatiStampa = "Tot.Imp.Emesso " & FormatNumber(impTotEmesso.ToString, 2)
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            If IdTributo <> Utility.Costanti.TRIBUTO_OSAP And IdTributo <> Utility.Costanti.TRIBUTO_SCUOLE Then
                sDatiStampa = "Tot.Imp.Pagato Comune " & FormatNumber(impTotPagatoComune.ToString, 2)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                sDatiStampa = "Tot.Imp.Pagato Maggiorazione " & FormatNumber(impTotPagatoStat.ToString, 2)
                sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
            End If
            sDatiStampa = "Tot.Imp.Pagato " & FormatNumber(impTotPagato.ToString, 2)
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            Return DtStampa
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsStampaXLS.PrintPagamenti.errore: ", Err)
            Return Nothing
        End Try
    End Function
    'Public Function PrintPagamenti(ByVal nTipoStampa As Integer, IdTributo As String, ByVal dvDati As DataView, ByVal sIntestazioneEnte As String, ByVal sTitoloReport As String) As DataTable
    '    '{0=pagamenti;1=emesso non pagato;2=emesso pagato parzialmente;3=emesso pagato in eccesso}
    '    Dim sDatiStampa, sDatiStampaGen As String
    '    Dim DsStampa As New DataSet
    '    Dim DtStampa As New DataTable
    '    Dim x As Integer
    '    Dim nTotPag As Integer = 0
    '    Dim nCampi As Integer
    '    Dim impTotPagatoComune As Double = 0
    '    Dim impTotPagatoStat As Double = 0
    '    Dim impTotPagato As Double = 0
    '    Dim impTotEmessoComune As Double = 0
    '    Dim impTotEmessoStat As Double = 0
    '    Dim impTotEmesso As Double = 0

    '    Try
    '        nCampi = 13
    '        'carico il dataset
    '        DsStampa.Tables.Add("STAMPA")
    '        'carico le colonne nel dataset
    '        For x = 1 To nCampi + 1
    '            DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
    '        Next
    '        'carico il datatable
    '        DtStampa = DsStampa.Tables("STAMPA")
    '        'inserisco l'intestazione dell'ente
    '        sDatiStampa = sIntestazioneEnte
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco una riga vuota
    '        sDatiStampa = ""
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco l'intestazione del report
    '        sDatiStampa = sTitoloReport
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco una riga vuota
    '        sDatiStampa = ""
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco le intestazioni di colonna
    '        sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.Iva|"
    '        sDatiStampa += "Anno|N.Avviso"
    '        If IdTributo <> Utility.Costanti.TRIBUTO_OSAP And IdTributo <> Utility.Costanti.TRIBUTO_SCUOLE Then
    '            sDatiStampa += "|Imp.Emesso Comune|Imp.Emesso Maggiorazione"
    '        End If
    '        sDatiStampa += "|Imp.Emesso"
    '        Select Case nTipoStampa
    '            Case 0
    '                sDatiStampa += "|Data Accredito|Data Pagamento"
    '                If IdTributo <> Utility.Costanti.TRIBUTO_OSAP And IdTributo <> Utility.Costanti.TRIBUTO_SCUOLE Then
    '                    sDatiStampa += "|Imp.Pagato Comume|Imp.Pagato Maggiorazione|Imp.Pagato"
    '                Else
    '                    sDatiStampa += "|Imp.Pagato"
    '                End If
    '                sDatiStampa += "|Provenienza"
    '            Case 1
    '                sDatiStampa += ""
    '            Case Else
    '                If IdTributo <> Utility.Costanti.TRIBUTO_OSAP And IdTributo <> Utility.Costanti.TRIBUTO_SCUOLE Then
    '                    sDatiStampa += "|Imp.Pagato Comume|Imp.Pagato Maggiorazione|Imp.Pagato"
    '                Else
    '                    sDatiStampa += "|Imp.Pagato"
    '                End If
    '        End Select
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'ciclo sui dati da stampare
    '        For x = 0 To dvDati.Count - 1
    '            sDatiStampaGen = ""
    '            nTotPag += 1
    '            sDatiStampaGen += CStr(dvDati.Item(x)("Cognome"))
    '            sDatiStampaGen += "|" + CStr(dvDati.Item(x)("Nome"))
    '            sDatiStampaGen += "|'" + CStr(dvDati.Item(x)("cfpiva")).ToUpper()
    '            sDatiStampaGen += "|" + CStr(dvDati.Item(x)("Anno"))
    '            sDatiStampaGen += "|'" + CStr(dvDati.Item(x)("codice_cartella"))
    '            If IdTributo <> Utility.Costanti.TRIBUTO_OSAP And IdTributo <> Utility.Costanti.TRIBUTO_SCUOLE Then
    '                sDatiStampaGen += "|" + FormatNumber(CStr(dvDati.Item(x)("dovutocomune")), 2)
    '                sDatiStampaGen += "|" + FormatNumber(CStr(dvDati.Item(x)("dovutostat")), 2)
    '            End If
    '            sDatiStampaGen += "|" + FormatNumber(CStr(dvDati.Item(x)("dovuto")), 2)
    '            Select Case nTipoStampa
    '                Case 0
    '                    If Not IsDBNull(dvDati.Item(x)("data_accredito")) Then
    '                        sDatiStampaGen += "|'" + Format(CDate(dvDati.Item(x)("data_accredito")), "dd/MM/yyyy")
    '                    Else
    '                        sDatiStampaGen += "|'"
    '                    End If
    '                    sDatiStampaGen += "|'" + Format(CDate(dvDati.Item(x)("data_pagamento")), "dd/MM/yyyy")
    '                    If IdTributo <> Utility.Costanti.TRIBUTO_OSAP And IdTributo <> Utility.Costanti.TRIBUTO_SCUOLE Then
    '                        sDatiStampaGen += "|" + FormatNumber(CStr(dvDati.Item(x)("importo_pagatocomune")), 2)
    '                        sDatiStampaGen += "|" + FormatNumber(CStr(dvDati.Item(x)("importo_pagatostat")), 2)
    '                    End If
    '                    sDatiStampaGen += "|" + FormatNumber(CStr(dvDati.Item(x)("importo_pagato")), 2)
    '                    sDatiStampaGen += "|" + CStr(dvDati.Item(x)("provenienza"))
    '                Case 1
    '                Case Else
    '                    If IdTributo <> Utility.Costanti.TRIBUTO_OSAP And IdTributo <> Utility.Costanti.TRIBUTO_SCUOLE Then
    '                        sDatiStampaGen += "|" + FormatNumber(CStr(dvDati.Item(x)("importo_pagatocomune")), 2)
    '                        sDatiStampaGen += "|" + FormatNumber(CStr(dvDati.Item(x)("importo_pagatostat")), 2)
    '                    End If
    '                    sDatiStampaGen += "|" + FormatNumber(CStr(dvDati.Item(x)("importo_pagato")), 2)
    '            End Select
    '            If AddRowStampa(DtStampa, sDatiStampaGen) = 0 Then
    '                Return Nothing
    '            End If

    '            impTotEmessoComune += CDbl(dvDati.Item(x)("dovutocomune"))
    '            impTotEmessoStat += CDbl(dvDati.Item(x)("dovutostat"))
    '            impTotEmesso += CDbl(dvDati.Item(x)("dovuto"))
    '            impTotPagatoComune += CDbl(dvDati.Item(x)("importo_pagatocomune"))
    '            impTotPagatoStat += CDbl(dvDati.Item(x)("importo_pagatostat"))
    '            impTotPagato += CDbl(dvDati.Item(x)("importo_pagato"))
    '        Next
    '        'inserisco una riga vuota
    '        sDatiStampa = ""
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco i totalizzatori
    '        If nTipoStampa = 0 Then
    '            sDatiStampa = "Tot.Pagamenti "
    '        Else
    '            sDatiStampa = "Tot.Avvisi "
    '        End If
    '        sDatiStampa += nTotPag.ToString
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        If IdTributo <> Utility.Costanti.TRIBUTO_OSAP And IdTributo <> Utility.Costanti.TRIBUTO_SCUOLE Then
    '            sDatiStampa = "Tot.Imp.Emesso Comune " & FormatNumber(impTotEmessoComune.ToString, 2)
    '            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
    '            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '                Return Nothing
    '            End If
    '            sDatiStampa = "Tot.Imp.Emesso Maggiorazione " & FormatNumber(impTotEmessoStat.ToString, 2)
    '            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
    '            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '                Return Nothing
    '            End If
    '        End If
    '        sDatiStampa = "Tot.Imp.Emesso " & FormatNumber(impTotEmesso.ToString, 2)
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        If IdTributo <> Utility.Costanti.TRIBUTO_OSAP And IdTributo <> Utility.Costanti.TRIBUTO_SCUOLE Then
    '            sDatiStampa = "Tot.Imp.Pagato Comune " & FormatNumber(impTotPagatoComune.ToString, 2)
    '            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
    '            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '                Return Nothing
    '            End If
    '            sDatiStampa = "Tot.Imp.Pagato Maggiorazione " & FormatNumber(impTotPagatoStat.ToString, 2)
    '            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
    '            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '                Return Nothing
    '            End If
    '        End If
    '        sDatiStampa = "Tot.Imp.Pagato " & FormatNumber(impTotPagato.ToString, 2)
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        Return DtStampa
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsStampaXLS.PrintPagamenti.errore: ", Err)
    '        Return Nothing
    '    End Try
    'End Function
    '*** ***
    Public Function PrintPagamentiRowDati(IdEnte As String, myRow As DataRowView, ByVal nTipoStampa As Integer, IdTributo As String) As String
        Dim sDatiStampa As String = ""

        Try
            If IdEnte = "" Then
                If Not IsDBNull(myRow("DESCRIZIONE_ENTE")) Then
                    sDatiStampa += myRow("DESCRIZIONE_ENTE").ToString() + "|"
                Else
                    sDatiStampa += "|"
                End If
            End If
            sDatiStampa += Utility.StringOperation.FormatString(myRow("Cognome"))
            sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("Nome"))
            sDatiStampa += "|'" + Utility.StringOperation.FormatString(myRow("cfpiva")).ToUpper()
            sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("Anno"))
            sDatiStampa += "|'" + Utility.StringOperation.FormatString(myRow("codice_cartella"))
            If IdTributo <> Utility.Costanti.TRIBUTO_OSAP And IdTributo <> Utility.Costanti.TRIBUTO_SCUOLE Then
                sDatiStampa += "|" + FormatNumber(Utility.StringOperation.FormatString(myRow("dovutocomune")), 2)
                sDatiStampa += "|" + FormatNumber(Utility.StringOperation.FormatString(myRow("dovutostat")), 2)
            End If
            sDatiStampa += "|" + FormatNumber(Utility.StringOperation.FormatString(myRow("dovuto")), 2)
            Select Case nTipoStampa
                Case ClsGestPag.TipoStampaPagamenti
                    sDatiStampa += "|'" + New Formatta.FunctionGrd().FormattaDataGrd(Utility.StringOperation.FormatDateTime(myRow("data_accredito")))
                    sDatiStampa += "|'" + New Formatta.FunctionGrd().FormattaDataGrd(Utility.StringOperation.FormatDateTime(myRow("data_pagamento")))
                    If IdTributo <> Utility.Costanti.TRIBUTO_OSAP And IdTributo <> Utility.Costanti.TRIBUTO_SCUOLE Then
                        sDatiStampa += "|" + FormatNumber(Utility.StringOperation.FormatString(myRow("importo_pagatocomune")), 2)
                        sDatiStampa += "|" + FormatNumber(Utility.StringOperation.FormatString(myRow("importo_pagatostat")), 2)
                    End If
                    sDatiStampa += "|" + FormatNumber(Utility.StringOperation.FormatString(myRow("importo_pagato")), 2)
                    sDatiStampa += "|" + Utility.StringOperation.FormatString(myRow("provenienza"))
                Case ClsGestPag.TipoStampaNonPagato
                Case Else
                    If IdTributo <> Utility.Costanti.TRIBUTO_OSAP And IdTributo <> Utility.Costanti.TRIBUTO_SCUOLE Then
                        sDatiStampa += "|" + FormatNumber(Utility.StringOperation.FormatString(myRow("importo_pagatocomune")), 2)
                        sDatiStampa += "|" + FormatNumber(Utility.StringOperation.FormatString(myRow("importo_pagatostat")), 2)
                    End If
                    sDatiStampa += "|" + FormatNumber(Utility.StringOperation.FormatString(myRow("importo_pagato")), 2)
            End Select
        Catch ex As Exception
            Log.Debug(IdEnte + " - OPENgovTIA.ClsStampaXLS.PrintPagamentiRowDati.errore: ", ex)
            sDatiStampa = ""
        End Try
        Return sDatiStampa
    End Function
    Public Function PrintRiversamento(ByVal dvDati As DataView, ByVal sIntestazioneEnte As String, ByVal sTitoloReport As String) As DataTable
        Dim sDatiStampa, sDatiStampaGen As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim x As Integer
        Dim nTotPag As Integer = 0
        Dim nCampi As Integer
        Dim impTotPagato As Double = 0

        Try
            nCampi = 5
            'carico il dataset
            DsStampa.Tables.Add("STAMPA")
            'carico le colonne nel dataset
            For x = 1 To nCampi + 1
                DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
            Next
            'carico il datatable
            DtStampa = DsStampa.Tables("STAMPA")
            'inserisco l'intestazione dell'ente
            sDatiStampa = sIntestazioneEnte
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco l'intestazione del report
            sDatiStampa = sTitoloReport
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco le intestazioni di colonna
            sDatiStampa = "Anno|Capitolo|Voce|Categoria|Importo"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'ciclo sui dati da stampare
            For x = 0 To dvDati.Count - 1
                sDatiStampaGen = ""
                nTotPag += 1
                If Not IsDBNull(dvDati.Item(x)("anno")) Then
                    sDatiStampaGen += CStr(dvDati.Item(x)("anno"))
                Else
                    sDatiStampaGen += "|"
                End If
                If Not IsDBNull(dvDati.Item(x)("capitolo")) Then
                    sDatiStampaGen += "|" + CStr(dvDati.Item(x)("capitolo"))
                Else
                    sDatiStampaGen += "|"
                End If
                If Not IsDBNull(dvDati.Item(x)("voce")) Then
                    sDatiStampaGen += "|" + CStr(dvDati.Item(x)("voce"))
                Else
                    sDatiStampaGen += "|"
                End If
                If Not IsDBNull(dvDati.Item(x)("categoria")) Then
                    sDatiStampaGen += "|" + CStr(dvDati.Item(x)("categoria"))
                Else
                    sDatiStampaGen += "|"
                End If
                If Not IsDBNull(dvDati.Item(x)("pagato")) Then
                    sDatiStampaGen += "|" + FormatNumber(CStr(dvDati.Item(x)("pagato")), 2)
                    impTotPagato += CDbl(dvDati.Item(x)("pagato"))
                Else
                    sDatiStampaGen += "|"
                End If
                If AddRowStampa(DtStampa, sDatiStampaGen) = 0 Then
                    Return Nothing
                End If
            Next
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco i totalizzatori
            sDatiStampa = "Tot.Imp.Pagato " & FormatNumber(impTotPagato.ToString, 2)
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            Return DtStampa
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsStampaXLS.PrintRiversamento.errore: ", Err)
            Return Nothing
        End Try
    End Function
    Public Function PrintQuadratura(ByVal dvDati As DataView, ByVal sIntestazioneEnte As String, ByVal sTitoloReport As String) As DataTable
        Dim sDatiStampa, sDatiStampaGen As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim x As Integer
        Dim nTotPag As Integer = 0
        Dim nCampi As Integer
        Dim impTotPagato As Double = 0

        Try
            nCampi = 3
            'carico il dataset
            DsStampa.Tables.Add("STAMPA")
            'carico le colonne nel dataset
            For x = 1 To nCampi + 1
                DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
            Next
            'carico il datatable
            DtStampa = DsStampa.Tables("STAMPA")
            'inserisco l'intestazione dell'ente
            sDatiStampa = sIntestazioneEnte
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco l'intestazione del report
            sDatiStampa = sTitoloReport
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco le intestazioni di colonna
            sDatiStampa = "Provenienza|Data Accredito|Importo"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'ciclo sui dati da stampare
            For x = 0 To dvDati.Count - 1
                sDatiStampaGen = ""
                nTotPag += 1
                If Not IsDBNull(dvDati.Item(x)("provenienza")) Then
                    sDatiStampaGen += CStr(dvDati.Item(x)("provenienza"))
                Else
                    sDatiStampaGen += "|"
                End If
                If Not IsDBNull(dvDati.Item(x)("data_accredito")) Then
                    sDatiStampaGen += "|'" + Format(CDate(dvDati.Item(x)("data_accredito")), "dd/MM/yyyy")
                Else
                    sDatiStampaGen += "|'"
                End If
                If Not IsDBNull(dvDati.Item(x)("pagato")) Then
                    sDatiStampaGen += "|" + FormatNumber(CStr(dvDati.Item(x)("pagato")), 2)
                    impTotPagato += CDbl(dvDati.Item(x)("pagato"))
                Else
                    sDatiStampaGen += "|"
                End If
                If AddRowStampa(DtStampa, sDatiStampaGen) = 0 Then
                    Return Nothing
                End If
            Next
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco i totalizzatori
            sDatiStampa = "Tot.Imp.Pagato " & FormatNumber(impTotPagato.ToString, 2)
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            Return DtStampa
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsStampaXLS.PrintQuadratura.errore: ", Err)
            Return Nothing
        End Try
    End Function
#Region "Check Rif.Catastali"
    Public Function PrintCheckRifCatastali(ByVal sIntestazioneEnte As String, ByVal sIntestazioneReport As String, ByVal oListRifCat() As oUIVSCat) As DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim x As Integer
        Dim iLenght As Integer = 15

        Try
            'carico il dataset
            DsStampa.Tables.Add("STAMPA")
            'carico gli colonne nel dataset
            For x = 1 To iLenght + 1
                DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
            Next
            'carico il datatable
            DtStampa = DsStampa.Tables("STAMPA")
            'inserisco l'intestazione dell'ente
            sDatiStampa = sIntestazioneEnte
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco l'intestazione del report
            sDatiStampa = sIntestazioneReport
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco gli intestazioni di colonna
            sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.IVA|Via|Civico|Esponente|Scala|Interno|Foglio|Numero|Subalterno|Data Inizio|Data Fine|Categoria|MQ"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco i valori
            For x = 0 To oListRifCat.GetUpperBound(0)
                'inserisco i valori
                sDatiStampa = ""
                sDatiStampa += oListRifCat(x).sCognome
                sDatiStampa += "|" + oListRifCat(x).sNome
                sDatiStampa += "|'" + oListRifCat(x).sCodFiscalePIva.ToUpper()
                sDatiStampa += "|" + oListRifCat(x).sVia
                sDatiStampa += "|'" + oListRifCat(x).sCivico
                sDatiStampa += "|" + oListRifCat(x).sEsponente
                sDatiStampa += "|" + oListRifCat(x).sScala
                sDatiStampa += "|" + oListRifCat(x).sInterno
                sDatiStampa += "|'" + oListRifCat(x).sFoglio
                sDatiStampa += "|'" + oListRifCat(x).sNumero
                sDatiStampa += "|'" + oListRifCat(x).sSubalterno
                If oListRifCat(x).tDataInizio <> Date.MinValue Then
                    sDatiStampa += "|" + oListRifCat(x).tDataInizio.ToString
                Else
                    sDatiStampa += "|"
                End If
                If oListRifCat(x).tDataFine <> Date.MinValue Then
                    sDatiStampa += "|" + oListRifCat(x).tDataFine.ToString
                Else
                    sDatiStampa += "|"
                End If
                sDatiStampa += "|" + oListRifCat(x).sIdCategoria & "-" & oListRifCat(x).sDescrCategoria
                If oListRifCat(x).nMQ > 0 Then
                    sDatiStampa += "|" + oListRifCat(x).nMQ.ToString
                Else
                    sDatiStampa += "|"
                End If
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
            Next

            Return DtStampa
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsStampaXLS.PrintCheckRifCatastali.errore: ", Err)
            Return Nothing
        End Try
    End Function
    Public Function PrintCheckRifCatastaliAccertati(ByVal sIntestazioneEnte As String, ByVal sIntestazioneReport As String, ByVal oListRifCat() As oUIVSCat) As DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim x As Integer
        Dim iLenght As Integer = 15

        Try
            'carico il dataset
            DsStampa.Tables.Add("STAMPA")
            'carico gli colonne nel dataset
            For x = 1 To iLenght + 1
                DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
            Next
            'carico il datatable
            DtStampa = DsStampa.Tables("STAMPA")
            'inserisco l'intestazione dell'ente
            sDatiStampa = sIntestazioneEnte
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco l'intestazione del report
            sDatiStampa = sIntestazioneReport
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco gli intestazioni di colonna
            sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.IVA|Foglio|Numero|Subalterno|||||||||"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco i valori
            For x = 0 To oListRifCat.GetUpperBound(0)
                'inserisco i valori
                sDatiStampa = ""
                sDatiStampa += oListRifCat(x).sCognome
                sDatiStampa += "|" + oListRifCat(x).sNome
                sDatiStampa += "|'" + oListRifCat(x).sCodFiscalePIva.ToUpper()
                sDatiStampa += "|'" + oListRifCat(x).sFoglio
                sDatiStampa += "|'" + oListRifCat(x).sNumero
                sDatiStampa += "|'" + oListRifCat(x).sSubalterno
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
            Next

            Return DtStampa
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsStampaXLS.PrintCheckRifCatastaliAccertati.errore: ", Err)
            Return Nothing
        End Try
    End Function
    '*** 20130201 - gestione mq da catasto per TARES ***
    Public Function PrintCheckMQDicVsCat(ByVal sIntestazioneEnte As String, ByVal sIntestazioneReport As String, ByVal oListRifCat() As oUIVSCat) As DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim x As Integer
        Dim iLenght As Integer = 15

        Try
            'carico il dataset
            DsStampa.Tables.Add("STAMPA")
            'carico gli colonne nel dataset
            For x = 1 To iLenght + 1
                DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
            Next
            'carico il datatable
            DtStampa = DsStampa.Tables("STAMPA")
            'inserisco l'intestazione dell'ente
            sDatiStampa = sIntestazioneEnte
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco l'intestazione del report
            sDatiStampa = sIntestazioneReport
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco gli intestazioni di colonna
            sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.IVA|Via|Civico|Esponente|Scala|Interno|Foglio|Numero|Subalterno|Data Inizio|Data Fine|MQ Dichiarazione|MQ Catasto|Differenza MQ"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco i valori
            For x = 0 To oListRifCat.GetUpperBound(0)
                'inserisco i valori
                sDatiStampa = ""
                sDatiStampa += oListRifCat(x).sCognome
                sDatiStampa += "|" + oListRifCat(x).sNome
                sDatiStampa += "|'" + oListRifCat(x).sCodFiscalePIva.ToUpper()
                sDatiStampa += "|" + oListRifCat(x).sVia
                sDatiStampa += "|'" + oListRifCat(x).sCivico
                sDatiStampa += "|" + oListRifCat(x).sEsponente
                sDatiStampa += "|" + oListRifCat(x).sScala
                sDatiStampa += "|" + oListRifCat(x).sInterno
                sDatiStampa += "|'" + oListRifCat(x).sFoglio
                sDatiStampa += "|'" + oListRifCat(x).sNumero
                sDatiStampa += "|'" + oListRifCat(x).sSubalterno
                If oListRifCat(x).tDataInizio <> Date.MinValue Then
                    sDatiStampa += "|" + oListRifCat(x).tDataInizio.ToString
                Else
                    sDatiStampa += "|"
                End If
                If oListRifCat(x).tDataFine <> Date.MinValue Then
                    sDatiStampa += "|" + oListRifCat(x).tDataFine.ToString
                Else
                    sDatiStampa += "|"
                End If
                sDatiStampa += "|" + oListRifCat(x).nMQ.ToString
                sDatiStampa += "|" + oListRifCat(x).nMQCat.ToString
                sDatiStampa += "|" + oListRifCat(x).nMQDif.ToString
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
            Next

            Return DtStampa
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsStampaXLS.PrintCheckMQDicVsCat.errore: ", Err)
            Return Nothing
        End Try
    End Function

    Public Function PrintCheckMQCatNoDic(ByVal sIntestazioneEnte As String, ByVal sIntestazioneReport As String, ByVal oListRifCat() As oUIVSCat) As DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim x As Integer
        Dim iLenght As Integer = 15

        Try
            'carico il dataset
            DsStampa.Tables.Add("STAMPA")
            'carico gli colonne nel dataset
            For x = 1 To iLenght + 1
                DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
            Next
            'carico il datatable
            DtStampa = DsStampa.Tables("STAMPA")
            'inserisco l'intestazione dell'ente
            sDatiStampa = sIntestazioneEnte
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco l'intestazione del report
            sDatiStampa = sIntestazioneReport
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco gli intestazioni di colonna
            sDatiStampa = "Via|Civico|Esponente|Scala|Interno|Foglio|Numero|Subalterno|MQ Catasto|||||||"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco i valori
            For x = 0 To oListRifCat.GetUpperBound(0)
                'inserisco i valori
                sDatiStampa = ""
                sDatiStampa += oListRifCat(x).sVia
                sDatiStampa += "|'" + oListRifCat(x).sCivico
                sDatiStampa += "|" + oListRifCat(x).sEsponente
                sDatiStampa += "|" + oListRifCat(x).sScala
                sDatiStampa += "|" + oListRifCat(x).sInterno
                sDatiStampa += "|'" + oListRifCat(x).sFoglio
                sDatiStampa += "|'" + oListRifCat(x).sNumero
                sDatiStampa += "|'" + oListRifCat(x).sSubalterno
                sDatiStampa += "|" + oListRifCat(x).nMQ.ToString
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
            Next

            Return DtStampa
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsStampaXLS.PrintCheckMQCatNoDic.errore: ", Err)
            Return Nothing
        End Try
    End Function
    '*** ***
    '*** 20130619 - estrazione posizioni modificate ***
    Public Function PrintDichMod(ByVal sIntestazioneEnte As String, ByVal sIntestazioneReport As String, ByVal oListRifCat() As oUIVSCat) As DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim x As Integer
        Dim iLenght As Integer = 15

        Try
            'carico il dataset
            DsStampa.Tables.Add("STAMPA")
            'carico gli colonne nel dataset
            For x = 1 To iLenght + 1
                DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
            Next
            'carico il datatable
            DtStampa = DsStampa.Tables("STAMPA")
            'inserisco l'intestazione dell'ente
            sDatiStampa = sIntestazioneEnte
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco l'intestazione del report
            sDatiStampa = sIntestazioneReport
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco gli intestazioni di colonna
            sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.IVA|N.Dichiarazione|Data Variazione Testata|Operatore Variazione Testata|Data Variazione Immobile|Operatore Variazione Immobile|Data Variazione Vano|Operatore Variazione Vano"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco i valori
            For x = 0 To oListRifCat.GetUpperBound(0)
                'inserisco i valori
                sDatiStampa = ""
                sDatiStampa += oListRifCat(x).sCognome
                sDatiStampa += "|" + oListRifCat(x).sNome
                sDatiStampa += "|'" + oListRifCat(x).sCodFiscalePIva.ToUpper()
                sDatiStampa += "|'" + oListRifCat(x).sDescrCategoria
                sDatiStampa += "|'" + oListRifCat(x).sAnno
                sDatiStampa += "|" + oListRifCat(x).sVia
                sDatiStampa += "|'" + oListRifCat(x).sCivico
                sDatiStampa += "|" + oListRifCat(x).sEsponente
                sDatiStampa += "|" + oListRifCat(x).sScala
                sDatiStampa += "|" + oListRifCat(x).sInterno
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
            Next

            Return DtStampa
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsStampaXLS.PrintDichMod.errore: ", Err)
            Return Nothing
        End Try
    End Function
    '*** ***
#End Region
    Public Function PrintAnalisiEconomiche(ByVal sIntestazioneEnte As String, ByVal dvDatiEmesso As DataView, ByVal dvDatiIncassato As DataView, ByVal nArticoliEmessi As Integer, ByVal impArticoliEmessi As Double, ByVal nAvvisiEmessi As Integer, ByVal impAvvisiEmessi As Double, ByVal nPagTotalmente As Integer, ByVal impPagTotalmente As Double, ByVal nPagParzialmente As Integer, ByVal impPagParzialmente As Double, ByVal impIncassato As Double, ByVal nUtenti As Integer, ByVal impInsoluto As Double, ByVal nPercentInsoluto As Double, ByVal impEmessoSanzioni As Double, ByVal impPagSanzioni As Double, ByVal impEmessoInteressi As Double, ByVal impPagInteressi As Double, ByVal impEmessoECA As Double, ByVal impPagECA As Double, ByVal impEmessoMECA As Double, ByVal impPagMECA As Double, ByVal impEmessoAggio As Double, ByVal impPagAggio As Double, ByVal impEmessoProvEnte As Double, ByVal impPagProvEnte As Double, ByVal impEmessoProv As Double, ByVal impPagProv As Double, ByVal impEmessoTotale As Double, ByVal impPagTotale As Double, ByVal impEmessoStat As Double, ByVal impIncassatoStat As Double, ByVal impInsolutoStat As Double, ByVal nPercentInsolutoStat As Double, ByVal impEmessoImposta As Double, ByVal impPagImposta As Double) As DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim x As Integer
        Dim iLenght As Integer = 6

        Try
            'carico il dataset
            DsStampa.Tables.Add("STAMPA")
            'carico gli colonne nel dataset
            For x = 1 To 7
                DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
            Next
            'carico il datatable
            DtStampa = DsStampa.Tables("STAMPA")
            'inserisco l'intestazione dell'ente
            sDatiStampa = sIntestazioneEnte
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco l'intestazione del report
            sDatiStampa = "Analisi Economiche"
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco l'intestazione del blocco
            sDatiStampa = "Dati Ruolo/Avvisi"
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco gli articoli emessi
            sDatiStampa = "N. Articoli|" & nArticoliEmessi & "||Totale Importo a Ruolo|" & impArticoliEmessi & "||"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco gli Avvisi Emessi
            sDatiStampa = "N. Avvisi Emessi|" & nAvvisiEmessi & "||Totale Avvisi|" & impAvvisiEmessi & "||"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco gli Avvisi Pagati totalmente
            sDatiStampa = "N. Avvisi Pagati Totalmente|" & nPagTotalmente & "||Totale Pagato|" & impPagTotalmente & "||"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco gli Avvisi Pagati parzialmente
            sDatiStampa = "N. Avvisi Pagati Parzialmente|" & nPagParzialmente & "||Totale Acconti|" & impPagParzialmente & "||"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco il Totale incassato
            sDatiStampa = "|||Totale Incassato|" & impIncassato & "||"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco l'intestazione del blocco
            sDatiStampa = "Riepilogo Generale"
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco gli intestazioni di colonna
            sDatiStampa = "N.Utenti|Emesso Avvisi||Incassato||Insoluto|% Insoluto"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco i valori
            sDatiStampa = nUtenti & "|" & impAvvisiEmessi & "|-|" & impIncassato & "|=|" & impInsoluto & "|" & nPercentInsoluto
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            '*** 20131104 - TARES ***
            'inserisco gli intestazioni di colonna
            sDatiStampa = "Maggiorazione|Emesso||Incassato||Insoluto|% Insoluto"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco i valori
            sDatiStampa = "|" & impEmessoStat & "|-|" & impIncassatoStat & "|=|" & impInsolutoStat & "|" & nPercentInsolutoStat
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            '*** ***
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco l'intestazione del blocco
            sDatiStampa = "Dettaglio Emesso||||Dettaglio Incassato||"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            '*** 20131104 - TARES ***
            sDatiStampa = "Imposta|" & impEmessoImposta & "|+||Imposta|" & impPagImposta & "|+"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            '*** ***
            'inserisco i valori
            sDatiStampa = "Sanzioni|" & impEmessoSanzioni & "|+||Sanzioni|" & impPagSanzioni & "|+"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco i valori
            sDatiStampa = "Interessi|" & impEmessoInteressi & "|+||Interessi|" & impPagInteressi & "|+"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco i valori
            sDatiStampa = "Addizionale EX ECA|" & impEmessoECA & "|+||Addizionale EX ECA|" & impPagECA & "|+"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco i valori
            sDatiStampa = "Tributo EX ECA|" & impEmessoMECA & "|+||Tributo EX ECA|" & impPagMECA & "|+"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco i valori
            sDatiStampa = "Aggio Per Ente|" & impEmessoAggio & "|+||Aggio Per Ente|" & impPagAggio & "|+"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco i valori
            sDatiStampa = "Addizionale Provinciale Per Ente|" & impEmessoProvEnte & "|+||Addizionale Provinciale Per Ente|" & impPagProvEnte & "|+"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco i valori
            sDatiStampa = "Addizionale Provinciale|" & impEmessoProv & "|=||Addizionale Provinciale|" & impPagProv & "|="
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco i valori
            sDatiStampa = "Importo Emesso|" & impEmessoTotale & "|||Importo Incassato|" & impPagTotale & "|"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            '*** 20131104 - TARES ***
            sDatiStampa = "Maggiorazione Emesso|" & impEmessoStat & "|||Maggiorazione Incassato|" & impIncassatoStat & "|"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            '*** ***
            Return DtStampa
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsStampaXLS.PrintAnalisiEconomiche.errore: ", Err)
            Return Nothing
        End Try
    End Function
    Public Function PrintAEDatiMancanti(ByVal DvDati As DataView, ByVal sIntestazioneEnte As String, nCampi As Integer) As DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim x As Integer

        Try
            'carico il dataset
            DsStampa.Tables.Add("STAMPA")
            'carico le colonne nel dataset
            For x = 0 To nCampi + 1
                DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
            Next

            'carico il datatable
            DtStampa = DsStampa.Tables("STAMPA")
            'inserisco l'intestazione dell'ente
            sDatiStampa = sIntestazioneEnte
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco l'intestazione del report
            sDatiStampa = "Elenco Dati Mancanti Agenzia Entrate"
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco le intestazioni di colonna
            sDatiStampa = "Anno|Cognome|Nome|Cod.Fiscale/P.Iva|Ubicazione Immobile|Foglio|Particella|Subalterno|Data Inizio|Data Fine|Tipo Anomalia"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'ciclo sui dati da stampare
            For Each myRow As DataRowView In DvDati
                sDatiStampa = ""
                If Not IsDBNull(myRow("ANNO")) Then
                    sDatiStampa += myRow("ANNO").ToString()
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("COGNOME")) Then
                    sDatiStampa += "|" + myRow("COGNOME").ToString()
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("NOME")) Then
                    sDatiStampa += "|" + myRow("NOME").ToString()
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("CFPIVA")) Then
                    sDatiStampa += "|" + myRow("CFPIVA").ToString()
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("IND_IMMO")) Then
                    sDatiStampa += "|" + myRow("IND_IMMO").ToString()
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("FOGLIO")) Then
                    sDatiStampa += "|" + myRow("FOGLIO").ToString()
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("PARTICELLA")) Then
                    sDatiStampa += "|" + myRow("PARTICELLA").ToString()
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("SUBALTERNO")) Then
                    sDatiStampa += "|" + myRow("SUBALTERNO").ToString()
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("INIZIO")) Then
                    sDatiStampa += "|" + myRow("INIZIO").ToString()
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("FINE")) Then
                    sDatiStampa += "|" + myRow("FINE").ToString()
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(myRow("DESCR_ANOMALIA")) Then
                    sDatiStampa += "|" + myRow("DESCR_ANOMALIA").ToString()
                Else
                    sDatiStampa += "|"
                End If
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
            Next
            Return DtStampa
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ClsStampaXLS.PrintAEDatiMancanti.errore: ", Err)
            Return Nothing
        End Try
    End Function
End Class
