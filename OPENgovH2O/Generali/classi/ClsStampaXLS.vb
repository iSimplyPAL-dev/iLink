Imports log4net
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
''' <summary>
''' Classe per la gestione delle stampe in formato CSV/XLS
''' </summary>
Public Class ClsStampaXLS
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ClsStampaXLS))
    Private oReplace As New ClsGenerale.Generale

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

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsStampaXLS.AddRowStampa.errore: ", Err)
            Return 0
        End Try
    End Function

    ''' <summary>
    ''' Restituisce un DataTable contenente i campi utilizzati per la stampa della minuta di validazione.
    ''' </summary>
    ''' <param name="oListFatture">array di oggetti di tipo ObjFattura</param>
    ''' <param name="sIntestazioneEnte"></param>
    ''' <param name="sPeriodo"></param>
    ''' <param name="hasAnagAllRow"></param>
    ''' <param name="nCampi"></param>
    ''' <returns>DataTable</returns>
    ''' <revisionHistory>
    ''' <revision date="17/12/2012">
    ''' calcolo quota fissa acqua+depurazione+fognatura
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="25/11/2014">
    ''' Componente aggiuntiva sui consumi
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Esportazione completa dati</em>
    ''' </revision>
    ''' </revisionHistory>
    '''<revisionHistory><revision date="18/02/2020">bisogna esporre separatamento tutte le voci dei canoni</revision></revisionHistory>
    Public Function PrintMinutaRuoloH2O(ByVal oListFatture() As ObjFattura, ByVal sIntestazioneEnte As String, ByVal sPeriodo As String, ByVal hasAnagAllRow As Integer, nCampi As Integer) As DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim x, nIdIntestPrec, IdContribPrec As Integer
        Dim nTotContribuenti As Integer = 0
        Dim nTotFatture As Integer = 0
        Dim nTotConsumo As Integer = 0
        Dim impTotImponibile As Double = 0
        Dim impTotIVA As Double = 0
        Dim impTotEsente As Double = 0
        Dim impTotArrotondamento As Double = 0
        Dim impTotDocumenti As Double = 0

        Try
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
            sDatiStampa = "Minuta Periodo " & sPeriodo
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
            sDatiStampa = "Nominativo Intestatario|N.Utente|Nominativo Utente|Cod.Fiscale/P.Iva"
            sDatiStampa += "|Indirizzo Residenza|Civico Residenza|CAP Residenza|Comune Residenza|Provincia Residenza|Belfiore Residenza"
            sDatiStampa += "|Ubicazione Via|Ubicazione Civico|Matricola|Tipo Utenza|N.Utenze|Esente H2O|Esente QF Acqua|Esente Depurazione|Esente QF Depurazione|Esente Fognatura|Esente QF Fognatura"
            sDatiStampa += "|Tipo Documento|Data Lettura Prec.|Lettura Prec.|Data Lettura Att.|Lettura Att.|Consumo|Giorni"
            sDatiStampa += "|Importo Scaglioni Euro|Importo Canone Depurazione Euro|Importo Canone Fognatura Euro"
            sDatiStampa += "|Importo UI1 Acqua Euro|Importo UI1 Depurazione Euro|Importo UI1 Fognatura Euro"
            sDatiStampa += "|Importo UI2 Acqua Euro|Importo UI2 Depurazione Euro|Importo UI2 Fognatura Euro"
            sDatiStampa += "|Importo UI3 Acqua Euro|Importo UI3 Depurazione Euro|Importo UI3 Fognatura Euro"
            sDatiStampa += "|Importo Nolo Euro|Importo Quote Fisse Acqua Euro|Importo Quote Fisse Depurazione Euro|Importo Quote Fisse Fognatura Euro|Importo Addizionali Euro"
            sDatiStampa += "|Importo Imponibile Euro|Importo Iva Euro|Importo Esente Euro|Importo Arrotondamento Euro|Importo Documento Euro|Data Documento|N.Documento"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'ciclo sui dati da stampare
            For x = 0 To oListFatture.GetUpperBound(0)
                nTotFatture += 1
                sDatiStampa = PrintMinutaRuoloH2ORowDati(oListFatture(x), IdContribPrec, nIdIntestPrec, hasAnagAllRow)
                If oListFatture(x).nIdUtente <> IdContribPrec Or hasAnagAllRow = 1 Then
                    nTotContribuenti += 1
                End If
                nTotConsumo += oListFatture(x).nConsumo
                impTotImponibile += oListFatture(x).impImponibile
                impTotIVA += oListFatture(x).impIva
                impTotEsente += oListFatture(x).impEsente
                impTotArrotondamento += oListFatture(x).impArrotondamento
                impTotDocumenti += oListFatture(x).impFattura
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                nIdIntestPrec = oListFatture(x).nIdIntestatario
                IdContribPrec = oListFatture(x).nIdUtente
            Next
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco i totalizzatori
            sDatiStampa = "Tot.Contribuenti|" & nTotContribuenti.ToString
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = "Tot.Fatture|" & nTotFatture.ToString
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = "Tot.Consumi|" & nTotConsumo.ToString
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = "Tot.Imponibile Euro|" & FormatNumber(impTotImponibile, 2)
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = "Tot.IVA Euro|" & FormatNumber(impTotIVA, 2)
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = "Tot.Esente Euro|" & FormatNumber(impTotEsente, 2)
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = "Tot.Arrotondamento Euro|" & FormatNumber(impTotArrotondamento, 2)
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = "Tot.Fatture Euro|" & FormatNumber(impTotDocumenti, 2)
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'aggiorno la data di creazione minuta sul db
            Dim FunctionRuolo As New ClsRuoloH2O
            If FunctionRuolo.SetDateRuoliH2OGenerati(oListFatture(0).nIdFlusso, 0) = 0 Then
                Return Nothing
            End If
            Return DtStampa
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsStampaXLS.PrintMinutaRuoloH2O.errore: ", Err)
            Return Nothing
        End Try
    End Function
    'Public Function PrintMinutaRuoloH2O(ByVal oListFatture() As ObjFattura, ByVal sIntestazioneEnte As String, ByVal sPeriodo As String, ByVal hasAnagAllRow As Integer, nCampi As Integer) As DataTable
    '    Dim sDatiStampa As String
    '    Dim DsStampa As New DataSet
    '    Dim DtStampa As New DataTable
    '    Dim x, nIdIntestPrec, IdContribPrec As Integer
    '    Dim nTotContribuenti As Integer = 0
    '    Dim nTotFatture As Integer = 0
    '    Dim nTotConsumo As Integer = 0
    '    Dim impTotImponibile As Double = 0
    '    Dim impTotIVA As Double = 0
    '    Dim impTotEsente As Double = 0
    '    Dim impTotArrotondamento As Double = 0
    '    Dim impTotDocumenti As Double = 0

    '    Try
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
    '        sDatiStampa = "Minuta Periodo " & sPeriodo
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
    '        sDatiStampa = "Nominativo Intestatario|N.Utente|Nominativo Utente|Cod.Fiscale/P.Iva"
    '        sDatiStampa += "|Indirizzo Residenza|Civico Residenza|CAP Residenza|Comune Residenza|Provincia Residenza|Belfiore Residenza"
    '        sDatiStampa += "|Ubicazione Via|Ubicazione Civico|Matricola|Tipo Utenza|N.Utenze|Esente H2O|Esente QF Acqua|Esente Depurazione|Esente QF Depurazione|Esente Fognatura|Esente QF Fognatura"
    '        sDatiStampa += "|Tipo Documento|Data Lettura Prec.|Lettura Prec.|Data Lettura Att.|Lettura Att.|Consumo|Giorni"
    '        sDatiStampa += "|Importo Scaglioni Euro|Importo Canone Depurazione Euro|Importo Canone Fognatura Euro"
    '        sDatiStampa += "|Importo UI1 Acqua Euro|Importo UI1 Depurazione Euro|Importo UI1 Fognatura Euro"
    '        sDatiStampa += "|Importo Nolo Euro|Importo Quote Fisse Acqua Euro|Importo Quote Fisse Depurazione Euro|Importo Quote Fisse Fognatura Euro|Importo Addizionali Euro"
    '        sDatiStampa += "|Importo Imponibile Euro|Importo Iva Euro|Importo Esente Euro|Importo Arrotondamento Euro|Importo Documento Euro|Data Documento|N.Documento"
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'ciclo sui dati da stampare
    '        For x = 0 To oListFatture.GetUpperBound(0)
    '            nTotFatture += 1
    '            sDatiStampa = PrintMinutaRuoloH2ORowDati(oListFatture(x), IdContribPrec, nIdIntestPrec, hasAnagAllRow)
    '            If oListFatture(x).nIdUtente <> IdContribPrec Or hasAnagAllRow = 1 Then
    '                nTotContribuenti += 1
    '            End If
    '            nTotConsumo += oListFatture(x).nConsumo
    '            impTotImponibile += oListFatture(x).impImponibile
    '            impTotIVA += oListFatture(x).impIva
    '            impTotEsente += oListFatture(x).impEsente
    '            impTotArrotondamento += oListFatture(x).impArrotondamento
    '            impTotDocumenti += oListFatture(x).impFattura
    '            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '                Return Nothing
    '            End If
    '            nIdIntestPrec = oListFatture(x).nIdIntestatario
    '            IdContribPrec = oListFatture(x).nIdUtente
    '        Next
    '        'inserisco una riga vuota
    '        sDatiStampa = ""
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco i totalizzatori
    '        sDatiStampa = "Tot.Contribuenti|" & nTotContribuenti.ToString
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        sDatiStampa = "Tot.Fatture|" & nTotFatture.ToString
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        sDatiStampa = "Tot.Consumi|" & nTotConsumo.ToString
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        sDatiStampa = "Tot.Imponibile Euro|" & FormatNumber(impTotImponibile, 2)
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        sDatiStampa = "Tot.IVA Euro|" & FormatNumber(impTotIVA, 2)
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        sDatiStampa = "Tot.Esente Euro|" & FormatNumber(impTotEsente, 2)
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        sDatiStampa = "Tot.Arrotondamento Euro|" & FormatNumber(impTotArrotondamento, 2)
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        sDatiStampa = "Tot.Fatture Euro|" & FormatNumber(impTotDocumenti, 2)
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'aggiorno la data di creazione minuta sul db
    '        Dim FunctionRuolo As New ClsRuoloH2O
    '        If FunctionRuolo.SetDateRuoliH2OGenerati(oListFatture(0).nIdFlusso, 0) = 0 Then
    '            Return Nothing
    '        End If
    '        Return DtStampa
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsStampaXLS.PrintMinutaRuoloH2O.errore: ", Err)
    '        Return Nothing
    '    End Try
    'End Function
    'Public Function PrintMinutaRuoloH2O(ByVal oListFatture() As ObjFattura, ByVal sIntestazioneEnte As String, ByVal sPeriodo As String, ByVal hasAnagAllRow As Integer, nCampi As Integer) As DataTable
    '    Dim sDatiStampa As String
    '    Dim DsStampa As New DataSet
    '    Dim DtStampa As New DataTable
    '    Dim x, nIdIntestPrec, IdContribPrec As Integer
    '    Dim nTotContribuenti As Integer = 0
    '    Dim nTotFatture As Integer = 0
    '    Dim nTotConsumo As Integer = 0
    '    Dim impTotImponibile As Double = 0
    '    Dim impTotIVA As Double = 0
    '    Dim impTotEsente As Double = 0
    '    Dim impTotArrotondamento As Double = 0
    '    Dim impTotDocumenti As Double = 0
    '    Dim impDepurazione, impFognatura As Double
    '    Dim impUI1H2O, impUI1Dep, impUI1Fog As Double

    '    Try
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
    '        sDatiStampa = "Minuta Periodo " & sPeriodo
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
    '        '*** 20141125 - Componente aggiuntiva sui consumi ***'*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
    '        sDatiStampa = "Nominativo Intestatario|N.Utente|Nominativo Utente|Cod.Fiscale/P.Iva"
    '        sDatiStampa += "|Indirizzo Residenza|Civico Residenza|CAP Residenza|Comune Residenza|Provincia Residenza|Belfiore Residenza"
    '        sDatiStampa += "|Ubicazione Via|Ubicazione Civico|Matricola|Tipo Utenza|N.Utenze|Esente H2O|Esente QF Acqua|Esente Depurazione|Esente QF Depurazione|Esente Fognatura|Esente QF Fognatura"
    '        sDatiStampa += "|Tipo Documento|Data Lettura Prec.|Lettura Prec.|Data Lettura Att.|Lettura Att.|Consumo|Giorni"
    '        sDatiStampa += "|Importo Scaglioni Euro|Importo Canone Depurazione Euro|Importo Canone Fognatura Euro"
    '        sDatiStampa += "|Importo UI1 Acqua Euro|Importo UI1 Depurazione Euro|Importo UI1 Fognatura Euro"
    '        sDatiStampa += "|Importo Nolo Euro|Importo Quote Fisse Acqua Euro|Importo Quote Fisse Depurazione Euro|Importo Quote Fisse Fognatura Euro|Importo Addizionali Euro"
    '        sDatiStampa += "|Importo Imponibile Euro|Importo Iva Euro|Importo Esente Euro|Importo Arrotondamento Euro|Importo Documento Euro|Data Documento|N.Documento"
    '        '*** ***
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'ciclo sui dati da stampare
    '        For x = 0 To oListFatture.GetUpperBound(0)
    '            sDatiStampa = "" : impDepurazione = 0 : impFognatura = 0 : impUI1H2O = 0 : impUI1dep = 0 : impUI1fog = 0
    '            nTotFatture += 1
    '            If oListFatture(x).nIdIntestatario <> nIdIntestPrec Or hasAnagAllRow = 1 Then
    '                sDatiStampa += oListFatture(x).oAnagrafeIntestatario.Cognome & " " & oListFatture(x).oAnagrafeIntestatario.Nome
    '            End If
    '            If oListFatture(x).nIdUtente <> IdContribPrec Or hasAnagAllRow = 1 Then
    '                nTotContribuenti += 1
    '                sDatiStampa += "|" + oListFatture(x).sNUtente
    '                sDatiStampa += "|" + oListFatture(x).oAnagrafeUtente.Cognome & " " & oListFatture(x).oAnagrafeUtente.Nome
    '                If oListFatture(x).oAnagrafeUtente.PartitaIva <> "" Then
    '                    sDatiStampa += "|'" + oListFatture(x).oAnagrafeUtente.PartitaIva
    '                Else
    '                    sDatiStampa += "|'" + oListFatture(x).oAnagrafeUtente.CodiceFiscale
    '                End If
    '                sDatiStampa += "|" + oListFatture(x).oAnagrafeUtente.ViaResidenza
    '                sDatiStampa += "|'" + oListFatture(x).oAnagrafeUtente.CivicoResidenza & " " & oListFatture(x).oAnagrafeUtente.InternoCivicoResidenza & " " & oListFatture(x).oAnagrafeUtente.EsponenteCivicoResidenza
    '                sDatiStampa += "|'" + oListFatture(x).oAnagrafeUtente.CapResidenza
    '                sDatiStampa += "|" + oListFatture(x).oAnagrafeUtente.ComuneResidenza
    '                sDatiStampa += "|" + oListFatture(x).oAnagrafeUtente.ProvinciaResidenza
    '                sDatiStampa += "|" + oListFatture(x).oAnagrafeUtente.CodiceComuneResidenza
    '            Else
    '                sDatiStampa += "|||||"
    '                sDatiStampa += "||||"
    '            End If
    '            sDatiStampa += "|" + oListFatture(x).sViaContatore
    '            sDatiStampa += "|'" + oListFatture(x).sCivicoContatore & " " & oListFatture(x).sFrazioneContatore
    '            sDatiStampa += "|'" + oListFatture(x).sMatricola
    '            sDatiStampa += "|" + oListFatture(x).sDescrTipoUtenza
    '            If oListFatture(x).impFattura < 0 Then
    '                sDatiStampa += "|" + (oListFatture(x).nUtenze * -1).ToString
    '            Else
    '                sDatiStampa += "|" + oListFatture(x).nUtenze.ToString
    '            End If
    '            '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
    '            If oListFatture(x).bEsenteAcqua = True Then
    '                sDatiStampa += "|X"
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If oListFatture(x).bEsenteAcquaQF = True Then
    '                sDatiStampa += "|X"
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If oListFatture(x).bEsenteDepurazione = True Then
    '                sDatiStampa += "|X"
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If oListFatture(x).bEsenteDepQF = True Then
    '                sDatiStampa += "|X"
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If oListFatture(x).bEsenteFognatura = True Then
    '                sDatiStampa += "|X"
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If oListFatture(x).bEsenteFogQF = True Then
    '                sDatiStampa += "|X"
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            '*** ***
    '            sDatiStampa += "|" + oListFatture(x).sDescrTipoDocumento
    '            If oListFatture(x).tDataLetturaPrec <> Date.MinValue Then
    '                sDatiStampa += "|" + oListFatture(x).tDataLetturaPrec.ToShortDateString.ToString
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            sDatiStampa += "|" + oListFatture(x).nLetturaPrec.ToString
    '            If oListFatture(x).tDataLetturaAtt <> Date.MinValue Then
    '                sDatiStampa += "|" + oListFatture(x).tDataLetturaAtt.ToShortDateString.ToString
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            sDatiStampa += "|" + oListFatture(x).nLetturaAtt.ToString
    '            nTotConsumo += oListFatture(x).nConsumo
    '            sDatiStampa += "|" + oListFatture(x).nConsumo.ToString
    '            sDatiStampa += "|" + oListFatture(x).nGiorni.ToString
    '            sDatiStampa += "|" + FormatNumber(oListFatture(x).impScaglioni.ToString, 2)
    '            'sDatiStampa += "|" + FormatNumber(oListFatture(x).impCanoni.ToString, 2)
    '            If Not oListFatture(x).oCanoni Is Nothing Then
    '                For Each myCanone As ObjTariffeCanone In oListFatture(x).oCanoni
    '                    If myCanone.idtipocanone = OggettoCanone.Canone_Depurazione Then
    '                        impDepurazione += myCanone.impCanone
    '                    ElseIf myCanone.idtipocanone = OggettoCanone.Canone_Fognatura Then
    '                        impFognatura += myCanone.impCanone
    '                    Else
    '                        If myCanone.idServizio = OggettoCanone.Canone_Depurazione Then
    '                            impUI1Dep += myCanone.impCanone
    '                        ElseIf myCanone.idServizio = OggettoCanone.Canone_Fognatura Then
    '                            impUI1Fog += myCanone.impCanone
    '                        Else
    '                            impUI1H2O += myCanone.impCanone
    '                        End If
    '                    End If
    '                Next
    '            End If
    '            If oListFatture(x).impFattura < 0 Then
    '                impDepurazione = impDepurazione * -1
    '                impFognatura = impFognatura * -1
    '                impUI1dep = impUI1dep * -1
    '                impUI1fog = impUI1fog * -1
    '                impUI1H2O = impUI1H2O * -1
    '            End If
    '            sDatiStampa += "|" + FormatNumber(impDepurazione.ToString, 2)
    '            sDatiStampa += "|" + FormatNumber(impFognatura.ToString, 2)
    '            '*** 20141125 - Componente aggiuntiva sui consumi ***
    '            sDatiStampa += "|" + FormatNumber(impUI1H2O.ToString, 2)
    '            sDatiStampa += "|" + FormatNumber(impUI1dep.ToString, 2)
    '            sDatiStampa += "|" + FormatNumber(impUI1fog.ToString, 2)
    '            '*** ***
    '            sDatiStampa += "|" + FormatNumber(oListFatture(x).impNolo.ToString, 2)
    '            sDatiStampa += "|" + FormatNumber(oListFatture(x).impQuoteFisse.ToString, 2)
    '            '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
    '            sDatiStampa += "|" + FormatNumber(oListFatture(x).impQuoteFisseDep.ToString, 2)
    '            sDatiStampa += "|" + FormatNumber(oListFatture(x).impQuoteFisseFog.ToString, 2)
    '            '*** ***
    '            sDatiStampa += "|" + FormatNumber(oListFatture(x).impAddizionali.ToString, 2)
    '            impTotImponibile += oListFatture(x).impImponibile
    '            sDatiStampa += "|" + FormatNumber(oListFatture(x).impImponibile.ToString, 2)
    '            impTotIVA += oListFatture(x).impIva
    '            sDatiStampa += "|" + FormatNumber(oListFatture(x).impIva.ToString, 2)
    '            impTotEsente += oListFatture(x).impEsente
    '            sDatiStampa += "|" + FormatNumber(oListFatture(x).impEsente.ToString, 2)
    '            impTotArrotondamento += oListFatture(x).impArrotondamento
    '            sDatiStampa += "|" + FormatNumber(oListFatture(x).impArrotondamento.ToString, 2)
    '            impTotDocumenti += oListFatture(x).impFattura
    '            sDatiStampa += "|" + FormatNumber(oListFatture(x).impFattura.ToString, 2)
    '            If oListFatture(x).tDataDocumento <> Date.MinValue Then
    '                sDatiStampa += "|" + oListFatture(x).tDataDocumento.ToShortDateString.ToString
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            sDatiStampa += "|" + oListFatture(x).sNDocumento
    '            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '                Return Nothing
    '            End If
    '            nIdIntestPrec = oListFatture(x).nIdIntestatario
    '            IdContribPrec = oListFatture(x).nIdUtente
    '        Next
    '        'inserisco una riga vuota
    '        sDatiStampa = ""
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + nCampi, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco i totalizzatori
    '        sDatiStampa = "Tot.Contribuenti|" & nTotContribuenti.ToString
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        sDatiStampa = "Tot.Fatture|" & nTotFatture.ToString
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        sDatiStampa = "Tot.Consumi|" & nTotConsumo.ToString
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        sDatiStampa = "Tot.Imponibile Euro|" & FormatNumber(impTotImponibile, 2)
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        sDatiStampa = "Tot.IVA Euro|" & FormatNumber(impTotIVA, 2)
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        sDatiStampa = "Tot.Esente Euro|" & FormatNumber(impTotEsente, 2)
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        sDatiStampa = "Tot.Arrotondamento Euro|" & FormatNumber(impTotArrotondamento, 2)
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        sDatiStampa = "Tot.Fatture Euro|" & FormatNumber(impTotDocumenti, 2)
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + (nCampi - sDatiStampa.Split(CChar("|")).Length), "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'aggiorno la data di creazione minuta sul db
    '        Dim FunctionRuolo As New ClsRuoloH2O
    '        If FunctionRuolo.SetDateRuoliH2OGenerati(oListFatture(0).nIdFlusso, 0) = 0 Then
    '            Return Nothing
    '        End If
    '        Return DtStampa
    '    Catch Err As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsStampaXLS.PrintMinutaRuoloH2O.errore: ", Err)
    '        Return Nothing
    '    End Try
    'End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myRow"></param>
    ''' <param name="IdContribPrec"></param>
    ''' <param name="nIdIntestPrec"></param>
    ''' <param name="hasAnagAllRow"></param>
    ''' <returns></returns>
    '''<revisionHistory><revision date="18/02/2020">bisogna esporre separatamento tutte le voci dei canoni</revision></revisionHistory>
    Public Function PrintMinutaRuoloH2ORowDati(myRow As ObjFattura, IdContribPrec As Integer, nIdIntestPrec As Integer, hasAnagAllRow As Integer) As String
        Dim sDatiStampa As String = ""
        Dim impDepurazione, impFognatura As Double
        Dim impUI1H2O, impUI1Dep, impUI1Fog, impUI2H2O, impUI2Dep, impUI2Fog, impUI3H2O, impUI3Dep, impUI3Fog As Double
        Try
            If myRow.nIdIntestatario <> nIdIntestPrec Or hasAnagAllRow = 1 Then
                sDatiStampa += myRow.oAnagrafeIntestatario.Cognome & " " & myRow.oAnagrafeIntestatario.Nome
            End If
            If myRow.nIdUtente <> IdContribPrec Or hasAnagAllRow = 1 Then
                sDatiStampa += "|" + myRow.sNUtente
                sDatiStampa += "|" + myRow.oAnagrafeUtente.Cognome & " " & myRow.oAnagrafeUtente.Nome
                If myRow.oAnagrafeUtente.PartitaIva <> "" Then
                    sDatiStampa += "|'" + myRow.oAnagrafeUtente.PartitaIva
                Else
                    sDatiStampa += "|'" + myRow.oAnagrafeUtente.CodiceFiscale
                End If
                sDatiStampa += "|" + myRow.oAnagrafeUtente.ViaResidenza
                sDatiStampa += "|'" + myRow.oAnagrafeUtente.CivicoResidenza & " " & myRow.oAnagrafeUtente.InternoCivicoResidenza & " " & myRow.oAnagrafeUtente.EsponenteCivicoResidenza
                sDatiStampa += "|'" + myRow.oAnagrafeUtente.CapResidenza
                sDatiStampa += "|" + myRow.oAnagrafeUtente.ComuneResidenza
                sDatiStampa += "|" + myRow.oAnagrafeUtente.ProvinciaResidenza
                sDatiStampa += "|" + myRow.oAnagrafeUtente.CodiceComuneResidenza
            Else
                sDatiStampa += "|||||"
                sDatiStampa += "||||"
            End If
            sDatiStampa += "|" + myRow.sViaContatore
            sDatiStampa += "|'" + myRow.sCivicoContatore & " " & myRow.sFrazioneContatore
            sDatiStampa += "|'" + myRow.sMatricola
            sDatiStampa += "|" + myRow.sDescrTipoUtenza
            If myRow.impFattura < 0 Then
                sDatiStampa += "|" + (myRow.nUtenze * -1).ToString
            Else
                sDatiStampa += "|" + myRow.nUtenze.ToString
            End If
            If myRow.bEsenteAcqua = True Then
                sDatiStampa += "|X"
            Else
                sDatiStampa += "|"
            End If
            If myRow.bEsenteAcquaQF = True Then
                sDatiStampa += "|X"
            Else
                sDatiStampa += "|"
            End If
            If myRow.bEsenteDepurazione = True Then
                sDatiStampa += "|X"
            Else
                sDatiStampa += "|"
            End If
            If myRow.bEsenteDepQF = True Then
                sDatiStampa += "|X"
            Else
                sDatiStampa += "|"
            End If
            If myRow.bEsenteFognatura = True Then
                sDatiStampa += "|X"
            Else
                sDatiStampa += "|"
            End If
            If myRow.bEsenteFogQF = True Then
                sDatiStampa += "|X"
            Else
                sDatiStampa += "|"
            End If
            sDatiStampa += "|" + myRow.sDescrTipoDocumento
            If myRow.tDataLetturaPrec <> DateTime.MaxValue Then
                sDatiStampa += "|" + myRow.tDataLetturaPrec.ToShortDateString.ToString
            Else
                sDatiStampa += "|"
            End If
            sDatiStampa += "|" + myRow.nLetturaPrec.ToString
            If myRow.tDataLetturaAtt <> DateTime.MaxValue Then
                sDatiStampa += "|" + myRow.tDataLetturaAtt.ToShortDateString.ToString
            Else
                sDatiStampa += "|"
            End If
            sDatiStampa += "|" + myRow.nLetturaAtt.ToString
            sDatiStampa += "|" + myRow.nConsumo.ToString
            sDatiStampa += "|" + myRow.nGiorni.ToString
            sDatiStampa += "|" + FormatNumber(myRow.impScaglioni.ToString, 2)
            If Not myRow.oCanoni Is Nothing Then
                For Each myCanone As ObjTariffeCanone In myRow.oCanoni
                    If myCanone.idTipoCanone = OggettoCanone.Canone_Depurazione Then
                        impDepurazione += myCanone.impCanone
                    ElseIf myCanone.idTipoCanone = OggettoCanone.Canone_Fognatura Then
                        impFognatura += myCanone.impCanone
                    Else
                        Select Case myCanone.sDescrizione.Substring(0, 3).ToUpper
                            Case "UI1"
                                If myCanone.idServizio = OggettoCanone.Canone_Depurazione Then
                                    impUI1Dep += myCanone.impCanone
                                ElseIf myCanone.idServizio = OggettoCanone.Canone_Fognatura Then
                                    impUI1Fog += myCanone.impCanone
                                Else
                                    impUI1H2O += myCanone.impCanone
                                End If
                            Case "UI2"
                                If myCanone.idServizio = OggettoCanone.Canone_Depurazione Then
                                    impUI2Dep += myCanone.impCanone
                                ElseIf myCanone.idServizio = OggettoCanone.Canone_Fognatura Then
                                    impUI2Fog += myCanone.impCanone
                                Else
                                    impUI2H2O += myCanone.impCanone
                                End If
                            Case "UI3"
                                If myCanone.idServizio = OggettoCanone.Canone_Depurazione Then
                                    impUI3Dep += myCanone.impCanone
                                ElseIf myCanone.idServizio = OggettoCanone.Canone_Fognatura Then
                                    impUI3Fog += myCanone.impCanone
                                Else
                                    impUI3H2O += myCanone.impCanone
                                End If
                        End Select
                    End If
                Next
            End If
            If myRow.impFattura < 0 Then
                impDepurazione = impDepurazione * -1
                impFognatura = impFognatura * -1
                impUI1Dep = impUI1Dep * -1
                impUI1Fog = impUI1Fog * -1
                impUI1H2O = impUI1H2O * -1
                impUI2Dep = impUI2Dep * -1
                impUI2Fog = impUI2Fog * -1
                impUI2H2O = impUI2H2O * -1
                impUI3Dep = impUI3Dep * -1
                impUI3Fog = impUI3Fog * -1
                impUI3H2O = impUI3H2O * -1
            End If
            sDatiStampa += "|" + FormatNumber(impDepurazione.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impFognatura.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impUI1H2O.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impUI1Dep.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impUI1Fog.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impUI2H2O.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impUI2Dep.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impUI2Fog.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impUI3H2O.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impUI3Dep.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impUI3Fog.ToString, 2)
            sDatiStampa += "|" + FormatNumber(myRow.impNolo.ToString, 2)
            sDatiStampa += "|" + FormatNumber(myRow.impQuoteFisse.ToString, 2)
            sDatiStampa += "|" + FormatNumber(myRow.impQuoteFisseDep.ToString, 2)
            sDatiStampa += "|" + FormatNumber(myRow.impQuoteFisseFog.ToString, 2)
            sDatiStampa += "|" + FormatNumber(myRow.impAddizionali.ToString, 2)
            sDatiStampa += "|" + FormatNumber(myRow.impImponibile.ToString, 2)
            sDatiStampa += "|" + FormatNumber(myRow.impIva.ToString, 2)
            sDatiStampa += "|" + FormatNumber(myRow.impEsente.ToString, 2)
            sDatiStampa += "|" + FormatNumber(myRow.impArrotondamento.ToString, 2)
            sDatiStampa += "|" + FormatNumber(myRow.impFattura.ToString, 2)
            If myRow.tDataDocumento <> DateTime.MaxValue Then
                sDatiStampa += "|" + myRow.tDataDocumento.ToShortDateString.ToString
            Else
                sDatiStampa += "|"
            End If
            sDatiStampa += "|" + myRow.sNDocumento
        Catch ex As Exception
            Throw ex
        End Try
        Return sDatiStampa
    End Function
    'Public Function PrintMinutaRuoloH2ORowDati(myRow As ObjFattura, IdContribPrec As Integer, nIdIntestPrec As Integer, hasAnagAllRow As Integer) As String
    '    Dim sDatiStampa As String = ""
    '    Dim impDepurazione, impFognatura As Double
    '    Dim impUI1H2O, impUI1Dep, impUI1Fog As Double
    '    Try
    '        If myRow.nIdIntestatario <> nIdIntestPrec Or hasAnagAllRow = 1 Then
    '            sDatiStampa += myRow.oAnagrafeIntestatario.Cognome & " " & myRow.oAnagrafeIntestatario.Nome
    '        End If
    '        If myRow.nIdUtente <> IdContribPrec Or hasAnagAllRow = 1 Then
    '            sDatiStampa += "|" + myRow.sNUtente
    '            sDatiStampa += "|" + myRow.oAnagrafeUtente.Cognome & " " & myRow.oAnagrafeUtente.Nome
    '            If myRow.oAnagrafeUtente.PartitaIva <> "" Then
    '                sDatiStampa += "|'" + myRow.oAnagrafeUtente.PartitaIva
    '            Else
    '                sDatiStampa += "|'" + myRow.oAnagrafeUtente.CodiceFiscale
    '            End If
    '            sDatiStampa += "|" + myRow.oAnagrafeUtente.ViaResidenza
    '            sDatiStampa += "|'" + myRow.oAnagrafeUtente.CivicoResidenza & " " & myRow.oAnagrafeUtente.InternoCivicoResidenza & " " & myRow.oAnagrafeUtente.EsponenteCivicoResidenza
    '            sDatiStampa += "|'" + myRow.oAnagrafeUtente.CapResidenza
    '            sDatiStampa += "|" + myRow.oAnagrafeUtente.ComuneResidenza
    '            sDatiStampa += "|" + myRow.oAnagrafeUtente.ProvinciaResidenza
    '            sDatiStampa += "|" + myRow.oAnagrafeUtente.CodiceComuneResidenza
    '        Else
    '            sDatiStampa += "|||||"
    '            sDatiStampa += "||||"
    '        End If
    '        sDatiStampa += "|" + myRow.sViaContatore
    '        sDatiStampa += "|'" + myRow.sCivicoContatore & " " & myRow.sFrazioneContatore
    '        sDatiStampa += "|'" + myRow.sMatricola
    '        sDatiStampa += "|" + myRow.sDescrTipoUtenza
    '        If myRow.impFattura < 0 Then
    '            sDatiStampa += "|" + (myRow.nUtenze * -1).ToString
    '        Else
    '            sDatiStampa += "|" + myRow.nUtenze.ToString
    '        End If
    '        If myRow.bEsenteAcqua = True Then
    '            sDatiStampa += "|X"
    '        Else
    '            sDatiStampa += "|"
    '        End If
    '        If myRow.bEsenteAcquaQF = True Then
    '            sDatiStampa += "|X"
    '        Else
    '            sDatiStampa += "|"
    '        End If
    '        If myRow.bEsenteDepurazione = True Then
    '            sDatiStampa += "|X"
    '        Else
    '            sDatiStampa += "|"
    '        End If
    '        If myRow.bEsenteDepQF = True Then
    '            sDatiStampa += "|X"
    '        Else
    '            sDatiStampa += "|"
    '        End If
    '        If myRow.bEsenteFognatura = True Then
    '            sDatiStampa += "|X"
    '        Else
    '            sDatiStampa += "|"
    '        End If
    '        If myRow.bEsenteFogQF = True Then
    '            sDatiStampa += "|X"
    '        Else
    '            sDatiStampa += "|"
    '        End If
    '        sDatiStampa += "|" + myRow.sDescrTipoDocumento
    '        If myRow.tDataLetturaPrec <> DateTime.MaxValue Then
    '            sDatiStampa += "|" + myRow.tDataLetturaPrec.ToShortDateString.ToString
    '        Else
    '            sDatiStampa += "|"
    '        End If
    '        sDatiStampa += "|" + myRow.nLetturaPrec.ToString
    '        If myRow.tDataLetturaAtt <> DateTime.MaxValue Then
    '            sDatiStampa += "|" + myRow.tDataLetturaAtt.ToShortDateString.ToString
    '        Else
    '            sDatiStampa += "|"
    '        End If
    '        sDatiStampa += "|" + myRow.nLetturaAtt.ToString
    '        sDatiStampa += "|" + myRow.nConsumo.ToString
    '        sDatiStampa += "|" + myRow.nGiorni.ToString
    '        sDatiStampa += "|" + FormatNumber(myRow.impScaglioni.ToString, 2)
    '        If Not myRow.oCanoni Is Nothing Then
    '            For Each myCanone As ObjTariffeCanone In myRow.oCanoni
    '                If myCanone.idTipoCanone = OggettoCanone.Canone_Depurazione Then
    '                    impDepurazione += myCanone.impCanone
    '                ElseIf myCanone.idTipoCanone = OggettoCanone.Canone_Fognatura Then
    '                    impFognatura += myCanone.impCanone
    '                Else
    '                    If myCanone.idServizio = OggettoCanone.Canone_Depurazione Then
    '                        impUI1Dep += myCanone.impCanone
    '                    ElseIf myCanone.idServizio = OggettoCanone.Canone_Fognatura Then
    '                        impUI1Fog += myCanone.impCanone
    '                    Else
    '                        impUI1H2O += myCanone.impCanone
    '                    End If
    '                End If
    '            Next
    '        End If
    '        If myRow.impFattura < 0 Then
    '            impDepurazione = impDepurazione * -1
    '            impFognatura = impFognatura * -1
    '            impUI1Dep = impUI1Dep * -1
    '            impUI1Fog = impUI1Fog * -1
    '            impUI1H2O = impUI1H2O * -1
    '        End If
    '        sDatiStampa += "|" + FormatNumber(impDepurazione.ToString, 2)
    '        sDatiStampa += "|" + FormatNumber(impFognatura.ToString, 2)
    '        sDatiStampa += "|" + FormatNumber(impUI1H2O.ToString, 2)
    '        sDatiStampa += "|" + FormatNumber(impUI1Dep.ToString, 2)
    '        sDatiStampa += "|" + FormatNumber(impUI1Fog.ToString, 2)
    '        sDatiStampa += "|" + FormatNumber(myRow.impNolo.ToString, 2)
    '        sDatiStampa += "|" + FormatNumber(myRow.impQuoteFisse.ToString, 2)
    '        sDatiStampa += "|" + FormatNumber(myRow.impQuoteFisseDep.ToString, 2)
    '        sDatiStampa += "|" + FormatNumber(myRow.impQuoteFisseFog.ToString, 2)
    '        sDatiStampa += "|" + FormatNumber(myRow.impAddizionali.ToString, 2)
    '        sDatiStampa += "|" + FormatNumber(myRow.impImponibile.ToString, 2)
    '        sDatiStampa += "|" + FormatNumber(myRow.impIva.ToString, 2)
    '        sDatiStampa += "|" + FormatNumber(myRow.impEsente.ToString, 2)
    '        sDatiStampa += "|" + FormatNumber(myRow.impArrotondamento.ToString, 2)
    '        sDatiStampa += "|" + FormatNumber(myRow.impFattura.ToString, 2)
    '        If myRow.tDataDocumento <> DateTime.MaxValue Then
    '            sDatiStampa += "|" + myRow.tDataDocumento.ToShortDateString.ToString
    '        Else
    '            sDatiStampa += "|"
    '        End If
    '        sDatiStampa += "|" + myRow.sNDocumento
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    '    Return sDatiStampa
    'End Function
    ''' <summary>
    ''' Restituisce un DataTable contenente i campi utilizzati per la stampa degli insoluti.
    ''' </summary>
    ''' <param name="dsMyDatiStampa">Dataset</param>
    ''' <param name="sIntestazioneEnte"></param>
    ''' <param name="sTipoProspetto">Indica se sono insoluti parziali o totali</param>
    ''' <returns>DataTable</returns>
    ''' <remarks>
    ''' </remarks>
    Public Function PrintInsoluti(ByVal dsMyDatiStampa As DataSet, ByVal sIntestazioneEnte As String, ByVal sTipoProspetto As String) As DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim x, IdContribPrec As Integer
        Dim nTotContribuenti As Integer = 0
        Dim nTotInsoluti As Integer = 0
        Dim impTotEmesso As Double = 0
        Dim impTotPagato As Double = 0
        Dim impTotInsoluto As Double = 0

        Try
            'carico il dataset
            DsStampa.Tables.Add("STAMPA")
            'carico le colonne nel dataset
            DsStampa.Tables("STAMPA").Columns.Add("NominativoIntestatario")
            DsStampa.Tables("STAMPA").Columns.Add("NumeroUtente")
            DsStampa.Tables("STAMPA").Columns.Add("NominativoUtente")
            DsStampa.Tables("STAMPA").Columns.Add("CodFiscalePIva")
            DsStampa.Tables("STAMPA").Columns.Add("Indirizzo_res")
            DsStampa.Tables("STAMPA").Columns.Add("IndirizzoInvio")
            DsStampa.Tables("STAMPA").Columns.Add("Ubicazione")
            DsStampa.Tables("STAMPA").Columns.Add("Matricola")
            DsStampa.Tables("STAMPA").Columns.Add("TipoUtenza")
            DsStampa.Tables("STAMPA").Columns.Add("NUtenze")
            DsStampa.Tables("STAMPA").Columns.Add("DataFattura")
            DsStampa.Tables("STAMPA").Columns.Add("NFattura")
            DsStampa.Tables("STAMPA").Columns.Add("NoteCredito")
            DsStampa.Tables("STAMPA").Columns.Add("impEmesso")
            DsStampa.Tables("STAMPA").Columns.Add("impPagato")
            DsStampa.Tables("STAMPA").Columns.Add("impInsoluto")
            'carico il datatable
            DtStampa = DsStampa.Tables("STAMPA")
            'inserisco l'intestazione dell'ente
            sDatiStampa = sIntestazioneEnte
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + 15, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + 15, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco l'intestazione del report
            sDatiStampa = "Insoluti " & sTipoProspetto
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + 15, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + 15, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco le intestazioni di colonna
            sDatiStampa = ""
            sDatiStampa = "Nominativo Intestatario|N.Utente|Nominativo Utente|Cod.Fiscale/P.Iva|Indirizzo Residenza|Indirizzo Invio|Ubicazione|Matricola|Tipo Utenza|N.Utenze|Data Fattura|N.Fattura|Riferimenti Note di Credito|Importo Emesso Euro|Importo Pagato Euro|Importo Insoluto Euro"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'ciclo sui dati da stampare
            For x = 0 To dsMyDatiStampa.Tables(0).Rows.Count - 1
                sDatiStampa = ""
                nTotInsoluti += 1
                If CStr(dsMyDatiStampa.Tables(0).Rows(x)("cod_utente")) <> IdContribPrec Then
                    nTotContribuenti += 1
                End If
                sDatiStampa += CStr(dsMyDatiStampa.Tables(0).Rows(x)("cognome_intest")) & " " & CStr(dsMyDatiStampa.Tables(0).Rows(x)("nome_intest"))
                sDatiStampa += "|" + CStr(dsMyDatiStampa.Tables(0).Rows(x)("numeroutente"))
                sDatiStampa += "|" + CStr(dsMyDatiStampa.Tables(0).Rows(x)("cognome_denominazione")) & " " & CStr(dsMyDatiStampa.Tables(0).Rows(x)("nome"))
                sDatiStampa += "|" + CStr(dsMyDatiStampa.Tables(0).Rows(x)("cfpiva"))
                sDatiStampa += "|" + CStr(dsMyDatiStampa.Tables(0).Rows(x)("Via_res")) & " " & CStr(dsMyDatiStampa.Tables(0).Rows(x)("Civico_res")) & " " & CStr(dsMyDatiStampa.Tables(0).Rows(x)("Interno_res")) & " " & CStr(dsMyDatiStampa.Tables(0).Rows(x)("Esponente_res")) & " " & CStr(dsMyDatiStampa.Tables(0).Rows(x)("Scala_res")) & " - " & CStr(dsMyDatiStampa.Tables(0).Rows(x)("cap_res")) & " " & CStr(dsMyDatiStampa.Tables(0).Rows(x)("comune_res")) & " " & CStr(dsMyDatiStampa.Tables(0).Rows(x)("provincia_res"))
                sDatiStampa += "|" + CStr(dsMyDatiStampa.Tables(0).Rows(x)("Via_rcp")) & " " & CStr(dsMyDatiStampa.Tables(0).Rows(x)("Civico_rcp")) & " " & CStr(dsMyDatiStampa.Tables(0).Rows(x)("Interno_rcp")) & " " & CStr(dsMyDatiStampa.Tables(0).Rows(x)("Esponente_rcp")) & " " & CStr(dsMyDatiStampa.Tables(0).Rows(x)("Scala_rcp")) & " - " & CStr(dsMyDatiStampa.Tables(0).Rows(x)("cap_rcp")) & " " & CStr(dsMyDatiStampa.Tables(0).Rows(x)("comune_rcp")) & " " & CStr(dsMyDatiStampa.Tables(0).Rows(x)("provincia_rcp"))
                sDatiStampa += "|" + CStr(dsMyDatiStampa.Tables(0).Rows(x)("via_contatore")) & " " & CStr(dsMyDatiStampa.Tables(0).Rows(x)("civico_contatore")) & " " & CStr(dsMyDatiStampa.Tables(0).Rows(x)("frazione_contatore"))
                sDatiStampa += "|" + CStr(dsMyDatiStampa.Tables(0).Rows(x)("matricola"))
                sDatiStampa += "|" + CStr(dsMyDatiStampa.Tables(0).Rows(x)("descrizione"))
                sDatiStampa += "|" + CStr(dsMyDatiStampa.Tables(0).Rows(x)("nutenze"))
                sDatiStampa += "|" + oReplace.GiraDataFromDB(CStr(dsMyDatiStampa.Tables(0).Rows(x)("data_fattura")))
                sDatiStampa += "|" + CStr(dsMyDatiStampa.Tables(0).Rows(x)("numero_fattura"))
                sDatiStampa += "|" + CStr(dsMyDatiStampa.Tables(0).Rows(x)("rif_note_credito"))
                sDatiStampa += "|" + CStr(dsMyDatiStampa.Tables(0).Rows(x)("impemesso"))
                sDatiStampa += "|" + CStr(dsMyDatiStampa.Tables(0).Rows(x)("imppagato"))
                sDatiStampa += "|" + CStr(dsMyDatiStampa.Tables(0).Rows(x)("impdifferenza"))
                impTotEmesso += CStr(dsMyDatiStampa.Tables(0).Rows(x)("impemesso"))
                impTotPagato += CStr(dsMyDatiStampa.Tables(0).Rows(x)("imppagato"))
                impTotInsoluto += CStr(dsMyDatiStampa.Tables(0).Rows(x)("impdifferenza"))
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
                IdContribPrec = CStr(dsMyDatiStampa.Tables(0).Rows(x)("cod_utente"))
            Next
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + 15, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco i totalizzatori
            sDatiStampa = "Tot.Contribuenti " & nTotContribuenti.ToString
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + 15, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = "Tot.Insoluti " & nTotInsoluti.ToString
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + 15, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = "Tot.Imp.Emesso Euro " & impTotEmesso.ToString
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + 15, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = "Tot.Imp.Pagato Euro " & impTotPagato.ToString
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + 15, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = "Tot.Imp.Insoluto Euro " & impTotInsoluto.ToString
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + 15, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            Return DtStampa
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ClsStampaXLS.PrintInsoluti.errore: ", Err)
            Return Nothing
        End Try
    End Function

    ' -----------------------------------------------------------------------------
    ' <summary>
    ' Restituisce un DataTable contenente i campi utilizzati per la stampa dell'analisi fatturato/incassato.
    ' </summary>
    ' <param name="sIntestazioneEnte"></param>
    ' <returns>DataTable</returns>
    ' <remarks>
    ' </remarks>
    ' <history>

    ' </history>
    ' -----------------------------------------------------------------------------
    Public Function PrintAnalisiFatturatoIncassatoH2O(ByVal sIntestazioneEnte As String, ByVal nFattureEmesse As Integer, ByVal impFattureEmesse As Double, ByVal nNoteEmesse As Integer, ByVal impNoteEmesse As Double, ByVal impNettoNote As Double, ByVal nPagTotalmente As Integer, ByVal impPagTotalmente As Double, ByVal nPagParzialmente As Integer, ByVal impPagParzialmente As Double, ByVal impIncassato As Double, ByVal nUtenti As Integer, ByVal impInsoluto As Double, ByVal nPercentInsoluto As Double, ByVal impFattConsumo As Double, ByVal impPagConsumo As Double, ByVal impFattFognatura As Double, ByVal impPagFognatura As Double, ByVal impFattDepurazione As Double, ByVal impPagDepurazione As Double, ByVal impFattNolo As Double, ByVal impPagNolo As Double, ByVal impFattQuotaFissa As Double, ByVal impPagQuotaFissa As Double, ByVal impFattAddizionali As Double, ByVal impPagAddizionali As Double, ByVal impFattIVA As Double, ByVal impPagIVA As Double, ByVal impFattTotale As Double, ByVal impPagTotale As Double, ByVal impFattFognaturaQF As Double, ByVal impPagFognaturaQF As Double, ByVal impFattDepurazioneQF As Double, ByVal impPagDepurazioneQF As Double) As DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim x As Integer
        Dim iLenght As Integer = 6

        Try
            'carico il dataset
            DsStampa.Tables.Add("STAMPA")
            'carico le colonne nel dataset
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
            'inserisco l'intestazione del blocco
            sDatiStampa = "Dati Fatturato/Incassato"
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco le fatture emesse
            sDatiStampa = "N. Fatture Emesse|" & nFattureEmesse & "||Totale Fatture|" & impFattureEmesse & "||"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco le note di credito emesse
            sDatiStampa = "N. Note di Credito Emesse|" & nNoteEmesse & "||Totale Note di Credito|" & impNoteEmesse & "||"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco il totale al netto delle note di credito
            sDatiStampa = "|||Totale al Netto di Note di Credito|" & impNettoNote & "||"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco le fatture pagate totalmente
            sDatiStampa = "N. Fatture Pagate Totalmente|" & nPagTotalmente & "||Totale Pagato|" & impPagTotalmente & "||"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco le fatture pagate parzialmente
            sDatiStampa = "N. Fatture Pagate Parzialmente|" & nPagParzialmente & "||Totale Acconti|" & impPagParzialmente & "||"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco il totale incassato
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
            'inserisco le intestazioni di colonna
            sDatiStampa = "N.Utenti|Emesso al Netto di Note di Credito||Incassato||Insoluto|% Insoluto"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco i valori
            sDatiStampa = nUtenti & "|" & impNettoNote & "|-|" & impIncassato & "|=|" & impInsoluto & "|" & nPercentInsoluto
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
            sDatiStampa = "Dettaglio Fatturato||||Dettaglio Incassato||"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco i valori
            sDatiStampa = "Consumo Acqua|" & impFattConsumo & "|+||Consumo Acqua|" & impPagConsumo & "|+"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco i valori
            sDatiStampa = "Fognatura|" & impFattFognatura & "|+||Fognatura|" & impPagFognatura & "|+"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco i valori
            sDatiStampa = "Depurazione|" & impFattDepurazione & "|+||Depurazione|" & impPagDepurazione & "|+"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco i valori
            sDatiStampa = "Nolo|" & impFattNolo & "|+||Nolo|" & impPagNolo & "|+"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco i valori
            sDatiStampa = "Quota Fissa|" & impFattQuotaFissa & "|+||Quota Fissa|" & impPagQuotaFissa & "|+"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco i valori
            sDatiStampa = "Quota Fissa Depurazione |" & impFattDepurazioneQF & "|+||Quota Fissa Depurazione |" & impPagDepurazioneQF & "|+"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco i valori
            sDatiStampa = "Quota Fissa Fognatura|" & impFattFognaturaQF & "|+||Quota Fissa Fognatura|" & impPagFognaturaQF & "|+"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco i valori
            sDatiStampa = "Addizionali|" & impFattAddizionali & "|+||Addizionali|" & impPagAddizionali & "|+"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco i valori
            sDatiStampa = "IVA|" & impFattIVA & "|=||IVA|" & impPagIVA & "|="
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco i valori
            sDatiStampa = "Importo Fatturato|" & impFattTotale & "|||Importo Incassato|" & impPagTotale & "|"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            Return DtStampa
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsStampaXLS.PrintAnalisiFatturatoIncassatoH2O.errore: ", Err)
            Return Nothing
        End Try
    End Function

    Public Function PrintElencoContratti(ByVal dvMyDatiStampa As DataView, ByVal sIntestazioneEnte As String, ByVal sPeriodo As String) As DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim x As Integer
        Dim iLenght As Integer = 32

        Try
            'carico il dataset
            DsStampa.Tables.Add("STAMPA")
            'carico le colonne nel dataset
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
            'inserisco il periodo
            sDatiStampa = "Periodo " & sPeriodo
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
            'inserisco il titolo report
            sDatiStampa = "Elenco Contratti"
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco le intestazioni dei blocchi di colonna
            sDatiStampa = "||Dati Intestatario|||||||||Dati Utente||||||||||Ubicazione|||||||||||"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco le intestazioni di colonna
            sDatiStampa = "Codice Contratto|Data Sottoscrizione|Cognome|Nome|Cod.Fiscale/P.IVA|Via|Civico|Esponente|Cap|Comune|Provincia|N.Utente|Cognome|Nome|Cod.Fiscale/P.IVA|Via|Civico|Esponente|Cap|Comune|Provincia|Matricola|Via|Civico|Esponente|Foglio|Numero|Subalterno|Tipo Utenza|N.Utenze|Stato Contratto|Data Cessazione"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = ""
            'ciclo sui dati da stampare
            For x = 0 To dvMyDatiStampa.Count - 1
                sDatiStampa = ""
                If Not IsDBNull(dvMyDatiStampa.Item(x)("codicecontratto")) Then
                    sDatiStampa = CStr(dvMyDatiStampa.Item(x)("codicecontratto"))
                End If
                If Not IsDBNull(dvMyDatiStampa.Item(x)("datasottoscrizione")) Then
                    sDatiStampa += "|" + oReplace.GiraDataFromDB(CStr(dvMyDatiStampa.Item(x)("datasottoscrizione")))
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(dvMyDatiStampa.Item(x)("COGNOME_INT")) Then
                    sDatiStampa += "|" + CStr(dvMyDatiStampa.Item(x)("COGNOME_INT"))
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(dvMyDatiStampa.Item(x)("NOME_INT")) Then
                    sDatiStampa += "|" + CStr(dvMyDatiStampa.Item(x)("NOME_INT"))
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(dvMyDatiStampa.Item(x)("CFPIVA_INT")) Then
                    sDatiStampa += "|" + "'" & CStr(dvMyDatiStampa.Item(x)("CFPIVA_INT"))
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(dvMyDatiStampa.Item(x)("VIA_INT")) Then
                    sDatiStampa += "|" + CStr(dvMyDatiStampa.Item(x)("VIA_INT"))
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(dvMyDatiStampa.Item(x)("CIVICO_INT")) Then
                    If CStr(dvMyDatiStampa.Item(x)("CIVICO_INT")) <> "-1" And CStr(dvMyDatiStampa.Item(x)("CIVICO_INT")) <> "0" Then
                        sDatiStampa += "|" + "'" & dvMyDatiStampa.Item(x)("CIVICO_INT")
                    Else
                        sDatiStampa += "|"
                    End If
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(dvMyDatiStampa.Item(x)("esponente_civico_int")) Then
                    If CStr(dvMyDatiStampa.Item(x)("esponente_civico_int")) <> "-1" And CStr(dvMyDatiStampa.Item(x)("esponente_civico_int")) <> "0" Then
                        sDatiStampa += "|" + "'" & dvMyDatiStampa.Item(x)("esponente_civico_int")
                    Else
                        sDatiStampa += "|"
                    End If
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(dvMyDatiStampa.Item(x)("CAP_INT")) Then
                    sDatiStampa += "|" + CStr(dvMyDatiStampa.Item(x)("CAP_INT"))
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(dvMyDatiStampa.Item(x)("COMUNE_INT")) Then
                    sDatiStampa += "|" + CStr(dvMyDatiStampa.Item(x)("COMUNE_INT"))
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(dvMyDatiStampa.Item(x)("PROVINCIA_INT")) Then
                    sDatiStampa += "|" + CStr(dvMyDatiStampa.Item(x)("PROVINCIA_INT"))
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(dvMyDatiStampa.Item(x)("NUMEROUTENTE")) Then
                    sDatiStampa += "|" + CStr(dvMyDatiStampa.Item(x)("NUMEROUTENTE"))
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(dvMyDatiStampa.Item(x)("COGNOME_UT")) Then
                    sDatiStampa += "|" + CStr(dvMyDatiStampa.Item(x)("COGNOME_UT"))
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(dvMyDatiStampa.Item(x)("NOME_UT")) Then
                    sDatiStampa += "|" + CStr(dvMyDatiStampa.Item(x)("NOME_UT"))
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(dvMyDatiStampa.Item(x)("CFPIVA_UT")) Then
                    sDatiStampa += "|" + "'" & CStr(dvMyDatiStampa.Item(x)("CFPIVA_UT"))
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(dvMyDatiStampa.Item(x)("VIA_UT")) Then
                    sDatiStampa += "|" + CStr(dvMyDatiStampa.Item(x)("VIA_UT"))
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(dvMyDatiStampa.Item(x)("CIVICO_UT")) Then
                    If CStr(dvMyDatiStampa.Item(x)("CIVICO_UT")) <> "-1" And CStr(dvMyDatiStampa.Item(x)("CIVICO_UT")) <> "0" Then
                        sDatiStampa += "|" + CStr(dvMyDatiStampa.Item(x)("CIVICO_UT"))
                    Else
                        sDatiStampa += "|"
                    End If
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(dvMyDatiStampa.Item(x)("esponente_civico_ut")) Then
                    If CStr(dvMyDatiStampa.Item(x)("esponente_civico_ut")) <> "-1" And CStr(dvMyDatiStampa.Item(x)("esponente_civico_ut")) <> "0" Then
                        sDatiStampa += "|" + "'" & dvMyDatiStampa.Item(x)("esponente_civico_ut")
                    Else
                        sDatiStampa += "|"
                    End If
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(dvMyDatiStampa.Item(x)("CAP_UT")) Then
                    sDatiStampa += "|" + CStr(dvMyDatiStampa.Item(x)("CAP_UT"))
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(dvMyDatiStampa.Item(x)("COMUNE_UT")) Then
                    sDatiStampa += "|" + CStr(dvMyDatiStampa.Item(x)("COMUNE_UT"))
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(dvMyDatiStampa.Item(x)("PROVINCIA_UT")) Then
                    sDatiStampa += "|" + CStr(dvMyDatiStampa.Item(x)("PROVINCIA_UT"))
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(dvMyDatiStampa.Item(x)("MATRICOLA")) Then
                    sDatiStampa += "|" + "'" & CStr(dvMyDatiStampa.Item(x)("MATRICOLA"))
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(dvMyDatiStampa.Item(x)("VIA_UBICAZIONE")) Then
                    sDatiStampa += "|" + CStr(dvMyDatiStampa.Item(x)("VIA_UBICAZIONE"))
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(dvMyDatiStampa.Item(x)("CIVICO_UBICAZIONE")) Then
                    If CStr(dvMyDatiStampa.Item(x)("CIVICO_UBICAZIONE")) <> "-1" And CStr(dvMyDatiStampa.Item(x)("CIVICO_UBICAZIONE")) <> "0" Then
                        sDatiStampa += "|" + CStr(dvMyDatiStampa.Item(x)("CIVICO_UBICAZIONE"))
                    Else
                        sDatiStampa += "|"
                    End If
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(dvMyDatiStampa.Item(x)("esponente_civico")) Then
                    If CStr(dvMyDatiStampa.Item(x)("esponente_civico")) <> "-1" And CStr(dvMyDatiStampa.Item(x)("esponente_civico")) <> "0" Then
                        sDatiStampa += "|" + "'" & dvMyDatiStampa.Item(x)("esponente_civico")
                    Else
                        sDatiStampa += "|"
                    End If
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(dvMyDatiStampa.Item(x)("FOGLIO")) Then
                    sDatiStampa += "|" + CStr(dvMyDatiStampa.Item(x)("FOGLIO"))
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(dvMyDatiStampa.Item(x)("NUMERO")) Then
                    sDatiStampa += "|" + CStr(dvMyDatiStampa.Item(x)("NUMERO"))
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(dvMyDatiStampa.Item(x)("SUBALTERNO")) Then
                    If CStr(dvMyDatiStampa.Item(x)("SUBALTERNO")) <> "-1" Then
                        sDatiStampa += "|" + CStr(dvMyDatiStampa.Item(x)("SUBALTERNO"))
                    Else
                        sDatiStampa += "|"
                    End If
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(dvMyDatiStampa.Item(x)("DESCRIZIONE")) Then
                    sDatiStampa += "|" + CStr(dvMyDatiStampa.Item(x)("DESCRIZIONE"))
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(dvMyDatiStampa.Item(x)("NUMEROUTENZE")) Then
                    sDatiStampa += "|" + CStr(dvMyDatiStampa.Item(x)("NUMEROUTENZE"))
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(dvMyDatiStampa.Item(x)("statocontratto")) Then
                    Select Case CStr(dvMyDatiStampa.Item(x)("statocontratto"))
                        Case 1
                            sDatiStampa += "|ATTIVO"
                        Case 2
                            sDatiStampa += "|PENDENTE"
                        Case 3
                            sDatiStampa += "|CESSATO"
                    End Select
                Else
                    sDatiStampa += "|"
                End If
                If Not IsDBNull(dvMyDatiStampa.Item(x)("datacessazione")) Then
                    sDatiStampa += "|" + oReplace.GiraDataFromDB(CStr(dvMyDatiStampa.Item(x)("datacessazione")))
                Else
                    sDatiStampa += "|"
                End If
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
            Next
            Return DtStampa
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsStampaXLS.PrintElencoContatti.errore: ", Err)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="dvMyDatiStampa"></param>
    ''' <param name="sIntestazioneEnte"></param>
    ''' <param name="sPeriodo"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="11/2015">
    ''' <strong>Funzioni Sovracomunali</strong>
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Esportazione completa dati</em>
    ''' </revision>
    ''' </revisionHistory>
    Public Function PrintElencoContatori(ByVal dvMyDatiStampa As DataView, ByVal sIntestazioneEnte As String, ByVal sPeriodo As String) As DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim x As Integer
        Dim iLenght As Integer = 32 '31

        Try
            'carico il dataset
            DsStampa.Tables.Add("STAMPA")
            'carico le colonne nel dataset
            For x = 0 To 32 '31
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
            'inserisco il periodo
            sDatiStampa = "Periodo " & sPeriodo
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
            'inserisco il titolo report
            sDatiStampa = "Elenco Contatori"
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco le intestazioni dei blocchi di colonna
            sDatiStampa = ""
            If ConstSession.IdEnte = "" Then
                sDatiStampa = "Ente|"
            End If
            sDatiStampa += "Dati Intestatario|||||||||Dati Utente||||||||||Ubicazione||||||||||||"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco le intestazioni di colonna
            sDatiStampa = ""
            If ConstSession.IdEnte = "" Then
                sDatiStampa = "Ente|"
            End If
            sDatiStampa += "Cognome|Nome|Cod.Fiscale/P.IVA|Via|Civico|Esponente|Cap|Comune|Provincia|N.Utente|Cognome|Nome|Cod.Fiscale/P.IVA|Via|Civico|Esponente|Cap|Comune|Provincia|Matricola|Via|Civico|Esponente|Foglio|Numero|Subalterno|Tipo Utenza|N.Utenze|Tipo Contatore|Data Attivazione|Data Sostituzione|Data Cessazione"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = ""
            'ciclo sui dati da stampare
            For Each myRow As DataRowView In dvMyDatiStampa
                sDatiStampa = PrintElencoContatoriRowDati(ConstSession.IdEnte, myRow)
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
            Next
            Return DtStampa
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsStampaXLS.PrintElencoContatori.errore: ", Err)
            Return Nothing
        End Try
    End Function
    'Public Function PrintElencoContatori(ByVal dvMyDatiStampa As DataView, ByVal sIntestazioneEnte As String, ByVal sPeriodo As String) As DataTable
    '    Dim sDatiStampa As String
    '    Dim DsStampa As New DataSet
    '    Dim DtStampa As New DataTable
    '    Dim x As Integer
    '    Dim iLenght As Integer = 32 '31

    '    Try
    '        'carico il dataset
    '        DsStampa.Tables.Add("STAMPA")
    '        'carico le colonne nel dataset
    '        For x = 0 To 32 '31
    '            DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
    '        Next
    '        'carico il datatable
    '        DtStampa = DsStampa.Tables("STAMPA")
    '        'inserisco l'intestazione dell'ente
    '        sDatiStampa = sIntestazioneEnte
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco una riga vuota
    '        sDatiStampa = ""
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco il periodo
    '        sDatiStampa = "Periodo " & sPeriodo
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco una riga vuota
    '        sDatiStampa = ""
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco il titolo report
    '        sDatiStampa = "Elenco Contatori"
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco le intestazioni dei blocchi di colonna
    '        '*** 201511 - Funzioni Sovracomunali ***
    '        sDatiStampa = ""
    '        If ConstSession.IdEnte = "" Then
    '            sDatiStampa = "Ente|"
    '        End If
    '        sDatiStampa += "Dati Intestatario|||||||||Dati Utente||||||||||Ubicazione||||||||||||"
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco le intestazioni di colonna
    '        sDatiStampa = ""
    '        If ConstSession.IdEnte = "" Then
    '            sDatiStampa = "Ente|"
    '        End If
    '        sDatiStampa += "Cognome|Nome|Cod.Fiscale/P.IVA|Via|Civico|Esponente|Cap|Comune|Provincia|N.Utente|Cognome|Nome|Cod.Fiscale/P.IVA|Via|Civico|Esponente|Cap|Comune|Provincia|Matricola|Via|Civico|Esponente|Foglio|Numero|Subalterno|Tipo Utenza|N.Utenze|Tipo Contatore|Data Attivazione|Data Sostituzione|Data Cessazione"
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        sDatiStampa = ""
    '        'ciclo sui dati da stampare
    '        For Each myRow As DataRowView In dvMyDatiStampa
    '            sDatiStampa = ""
    '            If ConstSession.IdEnte = "" Then
    '                If Not IsDBNull(myRow("DESCRIZIONE_ENTE")) Then
    '                    sDatiStampa += myRow("DESCRIZIONE_ENTE").ToString() + "|"
    '                Else
    '                    sDatiStampa += "|"
    '                End If
    '            End If
    '            If Not IsDBNull(myRow("COGNOME_INT")) Then
    '                sDatiStampa += CStr(myRow("COGNOME_INT"))
    '            End If
    '            If Not IsDBNull(myRow("NOME_INT")) Then
    '                sDatiStampa += "|" + CStr(myRow("NOME_INT"))
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("CFPIVA_INT")) Then
    '                sDatiStampa += "|" + "'" & CStr(myRow("CFPIVA_INT"))
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("VIA_INT")) Then
    '                sDatiStampa += "|" + CStr(myRow("VIA_INT"))
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("CIVICO_INT")) Then
    '                If CStr(myRow("CIVICO_INT")) <> "-1" And CStr(myRow("CIVICO_INT")) <> "0" Then
    '                    sDatiStampa += "|" + "'" & myRow("CIVICO_INT")
    '                Else
    '                    sDatiStampa += "|"
    '                End If
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("esponente_civico_int")) Then
    '                If CStr(myRow("esponente_civico_int")) <> "-1" And CStr(myRow("esponente_civico_int")) <> "0" Then
    '                    sDatiStampa += "|" + "'" & myRow("esponente_civico_int")
    '                Else
    '                    sDatiStampa += "|"
    '                End If
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("CAP_INT")) Then
    '                sDatiStampa += "|" + CStr(myRow("CAP_INT"))
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("COMUNE_INT")) Then
    '                sDatiStampa += "|" + CStr(myRow("COMUNE_INT"))
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("PROVINCIA_INT")) Then
    '                sDatiStampa += "|" + CStr(myRow("PROVINCIA_INT"))
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("NUMEROUTENTE")) Then
    '                sDatiStampa += "|" + CStr(myRow("NUMEROUTENTE"))
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("COGNOME_UT")) Then
    '                sDatiStampa += "|" + CStr(myRow("COGNOME_UT"))
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("NOME_UT")) Then
    '                sDatiStampa += "|" + CStr(myRow("NOME_UT"))
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("CFPIVA_UT")) Then
    '                sDatiStampa += "|" + "'" & CStr(myRow("CFPIVA_UT"))
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("VIA_UT")) Then
    '                sDatiStampa += "|" + CStr(myRow("VIA_UT"))
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("CIVICO_UT")) Then
    '                If CStr(myRow("CIVICO_UT")) <> "-1" And CStr(myRow("CIVICO_UT")) <> "0" Then
    '                    sDatiStampa += "|" + CStr(myRow("CIVICO_UT"))
    '                Else
    '                    sDatiStampa += "|"
    '                End If
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("esponente_civico_ut")) Then
    '                If CStr(myRow("esponente_civico_ut")) <> "-1" And CStr(myRow("esponente_civico_ut")) <> "0" Then
    '                    sDatiStampa += "|" + "'" & myRow("esponente_civico_ut")
    '                Else
    '                    sDatiStampa += "|"
    '                End If
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("CAP_UT")) Then
    '                sDatiStampa += "|" + CStr(myRow("CAP_UT"))
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("COMUNE_UT")) Then
    '                sDatiStampa += "|" + CStr(myRow("COMUNE_UT"))
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("PROVINCIA_UT")) Then
    '                sDatiStampa += "|" + CStr(myRow("PROVINCIA_UT"))
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("MATRICOLA")) Then
    '                sDatiStampa += "|" + "'" & CStr(myRow("MATRICOLA"))
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("VIA_UBICAZIONE")) Then
    '                sDatiStampa += "|" + CStr(myRow("VIA_UBICAZIONE"))
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("CIVICO_UBICAZIONE")) Then
    '                If CStr(myRow("CIVICO_UBICAZIONE")) <> "-1" And CStr(myRow("CIVICO_UBICAZIONE")) <> "0" Then
    '                    sDatiStampa += "|" + CStr(myRow("CIVICO_UBICAZIONE"))
    '                Else
    '                    sDatiStampa += "|"
    '                End If
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("esponente_civico")) Then
    '                If CStr(myRow("esponente_civico")) <> "-1" And CStr(myRow("esponente_civico")) <> "0" Then
    '                    sDatiStampa += "|" + "'" & myRow("esponente_civico")
    '                Else
    '                    sDatiStampa += "|"
    '                End If
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("FOGLIO")) Then
    '                sDatiStampa += "|" + CStr(myRow("FOGLIO"))
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("NUMERO")) Then
    '                sDatiStampa += "|" + CStr(myRow("NUMERO"))
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("SUBALTERNO")) Then
    '                If CStr(myRow("SUBALTERNO")) <> "-1" Then
    '                    sDatiStampa += "|" + CStr(myRow("SUBALTERNO"))
    '                Else
    '                    sDatiStampa += "|"
    '                End If
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("DESCRIZIONE")) Then
    '                sDatiStampa += "|" + CStr(myRow("DESCRIZIONE"))
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("NUMEROUTENZE")) Then
    '                sDatiStampa += "|" + CStr(myRow("NUMEROUTENZE"))
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("DESC_TIPOCONTATORE")) Then
    '                sDatiStampa += "|" + CStr(myRow("DESC_TIPOCONTATORE"))
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("DATAATTIVAZIONE")) Then
    '                sDatiStampa += "|" + oReplace.GiraDataFromDB(myRow("DATAATTIVAZIONE"))
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("DATASOSTITUZIONE")) Then
    '                sDatiStampa += "|" + oReplace.GiraDataFromDB(myRow("DATASOSTITUZIONE"))
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If Not IsDBNull(myRow("DATACESSAZIONE")) Then
    '                sDatiStampa += "|" + oReplace.GiraDataFromDB(myRow("DATACESSAZIONE"))
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '                Return Nothing
    '            End If
    '        Next
    '        Return DtStampa
    '    Catch Err As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsStampaXLS.PrintElencoContatori.errore: ", Err)
    '        Return Nothing
    '    End Try
    'End Function
    Public Function PrintElencoContatoriRowDati(IdEnte As String, myRow As DataRowView) As String
        Dim sDatiStampa As String = ""
        Try
            If IdEnte = "" Then
                If Not IsDBNull(myRow("DESCRIZIONE_ENTE")) Then
                    sDatiStampa += myRow("DESCRIZIONE_ENTE").ToString() + "|"
                Else
                    sDatiStampa += "|"
                End If
            End If
            If Not IsDBNull(myRow("COGNOME_INT")) Then
                sDatiStampa += CStr(myRow("COGNOME_INT"))
            End If
            If Not IsDBNull(myRow("NOME_INT")) Then
                sDatiStampa += "|" + CStr(myRow("NOME_INT"))
            Else
                sDatiStampa += "|"
            End If
            If Not IsDBNull(myRow("CFPIVA_INT")) Then
                sDatiStampa += "|" + "'" & CStr(myRow("CFPIVA_INT"))
            Else
                sDatiStampa += "|"
            End If
            If Not IsDBNull(myRow("VIA_INT")) Then
                sDatiStampa += "|" + CStr(myRow("VIA_INT"))
            Else
                sDatiStampa += "|"
            End If
            If Not IsDBNull(myRow("CIVICO_INT")) Then
                If CStr(myRow("CIVICO_INT")) <> "-1" And CStr(myRow("CIVICO_INT")) <> "0" Then
                    sDatiStampa += "|" + "'" & myRow("CIVICO_INT")
                Else
                    sDatiStampa += "|"
                End If
            Else
                sDatiStampa += "|"
            End If
            If Not IsDBNull(myRow("esponente_civico_int")) Then
                If CStr(myRow("esponente_civico_int")) <> "-1" And CStr(myRow("esponente_civico_int")) <> "0" Then
                    sDatiStampa += "|" + "'" & myRow("esponente_civico_int")
                Else
                    sDatiStampa += "|"
                End If
            Else
                sDatiStampa += "|"
            End If
            If Not IsDBNull(myRow("CAP_INT")) Then
                sDatiStampa += "|" + CStr(myRow("CAP_INT"))
            Else
                sDatiStampa += "|"
            End If
            If Not IsDBNull(myRow("COMUNE_INT")) Then
                sDatiStampa += "|" + CStr(myRow("COMUNE_INT"))
            Else
                sDatiStampa += "|"
            End If
            If Not IsDBNull(myRow("PROVINCIA_INT")) Then
                sDatiStampa += "|" + CStr(myRow("PROVINCIA_INT"))
            Else
                sDatiStampa += "|"
            End If
            If Not IsDBNull(myRow("NUMEROUTENTE")) Then
                sDatiStampa += "|" + CStr(myRow("NUMEROUTENTE"))
            Else
                sDatiStampa += "|"
            End If
            If Not IsDBNull(myRow("COGNOME_UT")) Then
                sDatiStampa += "|" + CStr(myRow("COGNOME_UT"))
            Else
                sDatiStampa += "|"
            End If
            If Not IsDBNull(myRow("NOME_UT")) Then
                sDatiStampa += "|" + CStr(myRow("NOME_UT"))
            Else
                sDatiStampa += "|"
            End If
            If Not IsDBNull(myRow("CFPIVA_UT")) Then
                sDatiStampa += "|" + "'" & CStr(myRow("CFPIVA_UT"))
            Else
                sDatiStampa += "|"
            End If
            If Not IsDBNull(myRow("VIA_UT")) Then
                sDatiStampa += "|" + CStr(myRow("VIA_UT"))
            Else
                sDatiStampa += "|"
            End If
            If Not IsDBNull(myRow("CIVICO_UT")) Then
                If CStr(myRow("CIVICO_UT")) <> "-1" And CStr(myRow("CIVICO_UT")) <> "0" Then
                    sDatiStampa += "|" + CStr(myRow("CIVICO_UT"))
                Else
                    sDatiStampa += "|"
                End If
            Else
                sDatiStampa += "|"
            End If
            If Not IsDBNull(myRow("esponente_civico_ut")) Then
                If CStr(myRow("esponente_civico_ut")) <> "-1" And CStr(myRow("esponente_civico_ut")) <> "0" Then
                    sDatiStampa += "|" + "'" & myRow("esponente_civico_ut")
                Else
                    sDatiStampa += "|"
                End If
            Else
                sDatiStampa += "|"
            End If
            If Not IsDBNull(myRow("CAP_UT")) Then
                sDatiStampa += "|" + CStr(myRow("CAP_UT"))
            Else
                sDatiStampa += "|"
            End If
            If Not IsDBNull(myRow("COMUNE_UT")) Then
                sDatiStampa += "|" + CStr(myRow("COMUNE_UT"))
            Else
                sDatiStampa += "|"
            End If
            If Not IsDBNull(myRow("PROVINCIA_UT")) Then
                sDatiStampa += "|" + CStr(myRow("PROVINCIA_UT"))
            Else
                sDatiStampa += "|"
            End If
            If Not IsDBNull(myRow("MATRICOLA")) Then
                sDatiStampa += "|" + "'" & CStr(myRow("MATRICOLA"))
            Else
                sDatiStampa += "|"
            End If
            If Not IsDBNull(myRow("VIA_UBICAZIONE")) Then
                sDatiStampa += "|" + CStr(myRow("VIA_UBICAZIONE"))
            Else
                sDatiStampa += "|"
            End If
            If Not IsDBNull(myRow("CIVICO_UBICAZIONE")) Then
                If CStr(myRow("CIVICO_UBICAZIONE")) <> "-1" And CStr(myRow("CIVICO_UBICAZIONE")) <> "0" Then
                    sDatiStampa += "|" + CStr(myRow("CIVICO_UBICAZIONE"))
                Else
                    sDatiStampa += "|"
                End If
            Else
                sDatiStampa += "|"
            End If
            If Not IsDBNull(myRow("esponente_civico")) Then
                If CStr(myRow("esponente_civico")) <> "-1" And CStr(myRow("esponente_civico")) <> "0" Then
                    sDatiStampa += "|" + "'" & myRow("esponente_civico")
                Else
                    sDatiStampa += "|"
                End If
            Else
                sDatiStampa += "|"
            End If
            If Not IsDBNull(myRow("FOGLIO")) Then
                sDatiStampa += "|" + CStr(myRow("FOGLIO"))
            Else
                sDatiStampa += "|"
            End If
            If Not IsDBNull(myRow("NUMERO")) Then
                sDatiStampa += "|" + CStr(myRow("NUMERO"))
            Else
                sDatiStampa += "|"
            End If
            If Not IsDBNull(myRow("SUBALTERNO")) Then
                If CStr(myRow("SUBALTERNO")) <> "-1" Then
                    sDatiStampa += "|" + CStr(myRow("SUBALTERNO"))
                Else
                    sDatiStampa += "|"
                End If
            Else
                sDatiStampa += "|"
            End If
            If Not IsDBNull(myRow("DESCRIZIONE")) Then
                sDatiStampa += "|" + CStr(myRow("DESCRIZIONE"))
            Else
                sDatiStampa += "|"
            End If
            If Not IsDBNull(myRow("NUMEROUTENZE")) Then
                sDatiStampa += "|" + CStr(myRow("NUMEROUTENZE"))
            Else
                sDatiStampa += "|"
            End If
            If Not IsDBNull(myRow("DESC_TIPOCONTATORE")) Then
                sDatiStampa += "|" + CStr(myRow("DESC_TIPOCONTATORE"))
            Else
                sDatiStampa += "|"
            End If
            If Not IsDBNull(myRow("DATAATTIVAZIONE")) Then
                sDatiStampa += "|" + oReplace.GiraDataFromDB(myRow("DATAATTIVAZIONE"))
            Else
                sDatiStampa += "|"
            End If
            If Not IsDBNull(myRow("DATASOSTITUZIONE")) Then
                sDatiStampa += "|" + oReplace.GiraDataFromDB(myRow("DATASOSTITUZIONE"))
            Else
                sDatiStampa += "|"
            End If
            If Not IsDBNull(myRow("DATACESSAZIONE")) Then
                sDatiStampa += "|" + oReplace.GiraDataFromDB(myRow("DATACESSAZIONE"))
            Else
                sDatiStampa += "|"
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return sDatiStampa
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="dvMyDatiStampa"></param>
    ''' <param name="sIntestazioneEnte"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="17/12/2012">
    ''' calcolo quota fissa acqua+depurazione+fognatura
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Esportazione completa dati</em>
    ''' </revision>
    ''' </revisionHistory>
    Public Function PrintPagamentiH2O(ByVal dvMyDatiStampa As DataView, ByVal sIntestazioneEnte As String) As DataTable
        Dim sDatiStampa, sRigaPrec As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim x As Integer
        Dim iLenght As Integer = 20
        Dim FncDettaglio As New ClsDettaglioVoci
        Dim impEmesso, impConsumo, impDepurazione, impFognatura, impAddizionali, impNolo, impQuotaFissa, impDepurazioneQF, impFognaturaQF, impIVA, impArrotondamento, impTotale As Double
        Dim impTotEmesso, impTotConsumo, impTotDepurazione, impTotFognatura, impTotAddizionali, impTotNolo, impTotQuotaFissa, impTotDepurazioneQF, impTotFognaturaQF, impTotIVA, impTotArrotondamento, impTotTotale As Double

        Try
            sRigaPrec = ""
            'carico il dataset
            DsStampa.Tables.Add("STAMPA")
            'carico le colonne nel dataset
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
            'inserisco l'intestazione del blocco
            sDatiStampa = "Elenco Pagamenti"
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco le intestazioni di colonna
            sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.IVA|Data Fattura|N.Fattura|Periodo|Importo Emesso|Data Accredito|Data Pagamento|Provenienza|Importo Acqua|Importo Depurazione|Importo Fognatura|Importo Addizionali|Importo Nolo|Importo Quota Fissa Acqua|Importo Quota Fissa Depurazione|Importo Quota Fissa Fognatura|Importo IVA|Importo Arrotondamento|Importo Pagato"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = ""
            'ciclo sui dati da stampare
            For x = 0 To dvMyDatiStampa.Count - 1
                If sRigaPrec <> CStr(dvMyDatiStampa.Item(x)("id")) & CStr(dvMyDatiStampa.Item(x)("cod_utente")) & CStr(dvMyDatiStampa.Item(x)("data_fattura")) & CStr(dvMyDatiStampa.Item(x)("numero_fattura")) & CStr(dvMyDatiStampa.Item(x)("provenienza")) & CStr(dvMyDatiStampa.Item(x)("data_accredito")) & CStr(dvMyDatiStampa.Item(x)("data_pagamento")) Then
                    'inserisco i valori della riga precedente
                    If sDatiStampa <> "" Then
                        sDatiStampa += "|" + FormatNumber(impConsumo.ToString, 2)
                        sDatiStampa += "|" + FormatNumber(impDepurazione.ToString, 2)
                        sDatiStampa += "|" + FormatNumber(impFognatura.ToString, 2)
                        sDatiStampa += "|" + FormatNumber(impAddizionali.ToString, 2)
                        sDatiStampa += "|" + FormatNumber(impNolo.ToString, 2)
                        sDatiStampa += "|" + FormatNumber(impQuotaFissa.ToString, 2)
                        sDatiStampa += "|" + FormatNumber(impDepurazioneQF.ToString, 2)
                        sDatiStampa += "|" + FormatNumber(impFognaturaQF.ToString, 2)
                        sDatiStampa += "|" + FormatNumber(impIVA.ToString, 2)
                        sDatiStampa += "|" + FormatNumber(impArrotondamento.ToString, 2)
                        sDatiStampa += "|" + FormatNumber(impTotale.ToString, 2)
                        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                            Return Nothing
                        End If
                        'incremento i totalizzatori
                        impTotEmesso += impEmesso
                        impTotConsumo += impConsumo
                        impTotDepurazione += impDepurazione
                        impTotFognatura += impFognatura
                        impTotAddizionali += impAddizionali
                        impTotNolo += impNolo
                        impTotQuotaFissa += impQuotaFissa
                        impTotDepurazioneQF += impDepurazioneQF
                        impTotFognaturaQF += impFognaturaQF
                        impTotIVA += impIVA
                        impTotArrotondamento += impArrotondamento
                        impTotTotale += impTotale
                    End If
                    'inizializzo la nuova riga
                    sDatiStampa = "" : impEmesso = 0 : impConsumo = 0 : impDepurazione = 0 : impFognatura = 0 : impAddizionali = 0 : impNolo = 0 : impQuotaFissa = 0 : impDepurazioneQF = 0 : impFognaturaQF = 0 : impIVA = 0 : impArrotondamento = 0 : impTotale = 0
                    sDatiStampa += CStr(dvMyDatiStampa.Item(x)("cognome")).ToUpper
                    sDatiStampa += "|" + CStr(dvMyDatiStampa.Item(x)("nome")).ToUpper
                    sDatiStampa += "|'" + CStr(dvMyDatiStampa.Item(x)("cfpiva")).ToUpper
                    sDatiStampa += "|'" + oReplace.GiraDataFromDB(CStr(dvMyDatiStampa.Item(x)("data_fattura")))
                    sDatiStampa += "|" + CStr(dvMyDatiStampa.Item(x)("numero_fattura"))
                    sDatiStampa += "|" + CStr(dvMyDatiStampa.Item(x)("periodo"))
                    sDatiStampa += "|" + FormatNumber(CStr(dvMyDatiStampa.Item(x)("impemesso")), 2)
                    impEmesso = CDbl(dvMyDatiStampa.Item(x)("impemesso"))
                    sDatiStampa += "|'" + oReplace.GiraDataFromDB(CStr(dvMyDatiStampa.Item(x)("data_accredito")))
                    sDatiStampa += "|'" + oReplace.GiraDataFromDB(CStr(dvMyDatiStampa.Item(x)("data_pagamento")))
                    sDatiStampa += "|" + CStr(dvMyDatiStampa.Item(x)("provenienza"))
                End If
                Select Case CStr(dvMyDatiStampa.Item(x)("cod_capitolo"))
                    Case ClsDettaglioVoci.CAPITOLO_CONSUMO
                        impConsumo += CDbl(dvMyDatiStampa.Item(x)("importo"))
                    Case ClsDettaglioVoci.CAPITOLO_CANONI
                        Select Case dvMyDatiStampa.Item(x)("idvoce").ToString()
                            Case ClsDettaglioVoci.VOCE_DEPURAZIONE
                                impDepurazione += CDbl(dvMyDatiStampa.Item(x)("importo"))
                            Case ClsDettaglioVoci.VOCE_FOGNATURA
                                impFognatura += CDbl(dvMyDatiStampa.Item(x)("importo"))
                        End Select
                    Case ClsDettaglioVoci.CAPITOLO_ADDIZIONALI
                        impAddizionali += CDbl(dvMyDatiStampa.Item(x)("importo"))
                    Case ClsDettaglioVoci.CAPITOLO_NOLO
                        impNolo += CDbl(dvMyDatiStampa.Item(x)("importo"))
                    Case ClsDettaglioVoci.CAPITOLO_QUOTAFISSA
                        impQuotaFissa += CDbl(dvMyDatiStampa.Item(x)("importo"))
                    Case ClsDettaglioVoci.VOCE_DEPURAZIONEQF
                        impDepurazioneQF += CDbl(dvMyDatiStampa.Item(x)("importo"))
                    Case ClsDettaglioVoci.VOCE_FOGNATURAQF
                        impFognaturaQF += CDbl(dvMyDatiStampa.Item(x)("importo"))
                    Case ClsDettaglioVoci.CAPITOLO_IVA
                        impIVA += CDbl(dvMyDatiStampa.Item(x)("importo"))
                    Case ClsDettaglioVoci.CAPITOLO_ARROTONDAMENTO
                        impArrotondamento += (dvMyDatiStampa.Item(x)("importo"))
                End Select
                impTotale += CDbl(dvMyDatiStampa.Item(x)("importo"))
                sRigaPrec = CStr(dvMyDatiStampa.Item(x)("id")) & CStr(dvMyDatiStampa.Item(x)("cod_utente")) & CStr(dvMyDatiStampa.Item(x)("data_fattura")) & CStr(dvMyDatiStampa.Item(x)("numero_fattura")) & CStr(dvMyDatiStampa.Item(x)("provenienza")) & CStr(dvMyDatiStampa.Item(x)("data_accredito")) & CStr(dvMyDatiStampa.Item(x)("data_pagamento"))
            Next
            'inserisco i valori della riga precedente
            sDatiStampa += "|" + FormatNumber(impConsumo.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impDepurazione.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impFognatura.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impAddizionali.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impNolo.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impQuotaFissa.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impDepurazioneQF.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impFognaturaQF.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impIVA.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impArrotondamento.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impTotale.ToString, 2)
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'incremento i totalizzatori
            impTotEmesso += impEmesso
            impTotConsumo += impConsumo
            impTotDepurazione += impDepurazione
            impTotFognatura += impFognatura
            impTotAddizionali += impAddizionali
            impTotNolo += impNolo
            impTotQuotaFissa += impQuotaFissa
            impTotDepurazioneQF += impDepurazioneQF
            impTotFognaturaQF += impFognaturaQF
            impTotIVA += impIVA
            impTotArrotondamento += impArrotondamento
            impTotTotale += impTotale
            'inserisco i totalizzatori
            sDatiStampa = "||||Totale"
            sDatiStampa += "|" + FormatNumber(impTotEmesso.ToString, 2)
            sDatiStampa += "||||" + FormatNumber(impTotConsumo.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impTotDepurazione.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impTotFognatura.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impTotAddizionali.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impTotNolo.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impTotQuotaFissa.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impTotDepurazioneQF.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impTotFognaturaQF.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impTotIVA.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impTotArrotondamento.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impTotTotale.ToString, 2)
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            Return DtStampa
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsStampaXLS.PrintPagamentiH2O.errore: ", Err)
            Return Nothing
        End Try
    End Function
    'Public Function PrintPagamentiH2O(ByVal dvMyDatiStampa As DataView, ByVal sIntestazioneEnte As String) As DataTable
    '    Dim sDatiStampa, sRigaPrec As String
    '    Dim DsStampa As New DataSet
    '    Dim DtStampa As New DataTable
    '    Dim x As Integer
    '    Dim iLenght As Integer = 19      '16
    '    Dim FncDettaglio As New ClsDettaglioVoci
    '    Dim impEmesso, impConsumo, impDepurazione, impFognatura, impAddizionali, impNolo, impQuotaFissa, impDepurazioneQF, impFognaturaQF, impIVA, impArrotondamento, impTotale As Double
    '    Dim impTotEmesso, impTotConsumo, impTotDepurazione, impTotFognatura, impTotAddizionali, impTotNolo, impTotQuotaFissa, impTotDepurazioneQF, impTotFognaturaQF, impTotIVA, impTotArrotondamento, impTotTotale As Double

    '    Try
    '        'carico il dataset
    '        DsStampa.Tables.Add("STAMPA")
    '        '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
    '        'carico le colonne nel dataset
    '        For x = 1 To 20      '18
    '            DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
    '        Next
    '        'carico il datatable
    '        DtStampa = DsStampa.Tables("STAMPA")
    '        'inserisco l'intestazione dell'ente
    '        sDatiStampa = sIntestazioneEnte
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco una riga vuota
    '        sDatiStampa = ""
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco l'intestazione del blocco
    '        sDatiStampa = "Elenco Pagamenti"
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco le intestazioni di colonna
    '        '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
    '        'sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.IVA|Data Fattura|N.Fattura|Importo Emesso|Data Accredito|Data Pagamento|Provenienza|Importo Acqua|Importo Depurazione|Importo Fognatura|Importo Addizionali|Importo Nolo|Importo Quota Fissa|Importo IVA|Importo Arrotondamento|Importo Pagato"
    '        sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.IVA|Data Fattura|N.Fattura|Importo Emesso|Data Accredito|Data Pagamento|Provenienza|Importo Acqua|Importo Depurazione|Importo Fognatura|Importo Addizionali|Importo Nolo|Importo Quota Fissa Acqua|Importo Quota Fissa Depurazione|Importo Quota Fissa Fognatura|Importo IVA|Importo Arrotondamento|Importo Pagato"
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        sDatiStampa = ""
    '        'ciclo sui dati da stampare
    '        For x = 0 To dvMyDatiStampa.Count - 1
    '            If sRigaPrec <> CStr(dvMyDatiStampa.Item(x)("id")) & CStr(dvMyDatiStampa.Item(x)("cod_utente")) & CStr(dvMyDatiStampa.Item(x)("data_fattura")) & CStr(dvMyDatiStampa.Item(x)("numero_fattura")) & CStr(dvMyDatiStampa.Item(x)("provenienza")) & CStr(dvMyDatiStampa.Item(x)("data_accredito")) & CStr(dvMyDatiStampa.Item(x)("data_pagamento")) Then
    '                'inserisco i valori della riga precedente
    '                If sDatiStampa <> "" Then
    '                    sDatiStampa += "|" + FormatNumber(impConsumo.ToString, 2)
    '                    sDatiStampa += "|" + FormatNumber(impDepurazione.ToString, 2)
    '                    sDatiStampa += "|" + FormatNumber(impFognatura.ToString, 2)
    '                    sDatiStampa += "|" + FormatNumber(impAddizionali.ToString, 2)
    '                    sDatiStampa += "|" + FormatNumber(impNolo.ToString, 2)
    '                    sDatiStampa += "|" + FormatNumber(impQuotaFissa.ToString, 2)
    '                    sDatiStampa += "|" + FormatNumber(impDepurazioneQF.ToString, 2)
    '                    sDatiStampa += "|" + FormatNumber(impFognaturaQF.ToString, 2)
    '                    sDatiStampa += "|" + FormatNumber(impIVA.ToString, 2)
    '                    sDatiStampa += "|" + FormatNumber(impArrotondamento.ToString, 2)
    '                    sDatiStampa += "|" + FormatNumber(impTotale.ToString, 2)
    '                    If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '                        Return Nothing
    '                    End If
    '                    'incremento i totalizzatori
    '                    impTotEmesso += impEmesso
    '                    impTotConsumo += impConsumo
    '                    impTotDepurazione += impDepurazione
    '                    impTotFognatura += impFognatura
    '                    impTotAddizionali += impAddizionali
    '                    impTotNolo += impNolo
    '                    impTotQuotaFissa += impQuotaFissa
    '                    impTotDepurazioneQF += impDepurazioneQF
    '                    impTotFognaturaQF += impFognaturaQF
    '                    impTotIVA += impIVA
    '                    impTotArrotondamento += impArrotondamento
    '                    impTotTotale += impTotale
    '                End If
    '                'inizializzo la nuova riga
    '                sDatiStampa = "" : impEmesso = 0 : impConsumo = 0 : impDepurazione = 0 : impFognatura = 0 : impAddizionali = 0 : impNolo = 0 : impQuotaFissa = 0 : impDepurazioneQF = 0 : impFognaturaQF = 0 : impIVA = 0 : impArrotondamento = 0 : impTotale = 0
    '                sDatiStampa += CStr(dvMyDatiStampa.Item(x)("cognome")).ToUpper
    '                sDatiStampa += "|" + CStr(dvMyDatiStampa.Item(x)("nome")).ToUpper
    '                sDatiStampa += "|'" + CStr(dvMyDatiStampa.Item(x)("cfpiva")).ToUpper
    '                sDatiStampa += "|'" + oReplace.GiraDataFromDB(CStr(dvMyDatiStampa.Item(x)("data_fattura")))
    '                sDatiStampa += "|" + CStr(dvMyDatiStampa.Item(x)("numero_fattura"))
    '                sDatiStampa += "|" + FormatNumber(CStr(dvMyDatiStampa.Item(x)("impemesso")), 2)
    '                impEmesso = CDbl(dvMyDatiStampa.Item(x)("impemesso"))
    '                sDatiStampa += "|'" + oReplace.GiraDataFromDB(CStr(dvMyDatiStampa.Item(x)("data_accredito")))
    '                sDatiStampa += "|'" + oReplace.GiraDataFromDB(CStr(dvMyDatiStampa.Item(x)("data_pagamento")))
    '                sDatiStampa += "|" + CStr(dvMyDatiStampa.Item(x)("provenienza"))
    '            End If
    '            Select Case CStr(dvMyDatiStampa.Item(x)("cod_capitolo"))
    '                Case ClsDettaglioVoci.CAPITOLO_CONSUMO
    '                    impConsumo += CDbl(dvMyDatiStampa.Item(x)("importo"))
    '                Case ClsDettaglioVoci.CAPITOLO_CANONI
    '                    Select Case dvMyDatiStampa.Item(x)("idvoce").ToString()
    '                        Case ClsDettaglioVoci.VOCE_DEPURAZIONE
    '                            impDepurazione += CDbl(dvMyDatiStampa.Item(x)("importo"))
    '                        Case ClsDettaglioVoci.VOCE_FOGNATURA
    '                            impFognatura += CDbl(dvMyDatiStampa.Item(x)("importo"))
    '                    End Select
    '                Case ClsDettaglioVoci.CAPITOLO_ADDIZIONALI
    '                    impAddizionali += CDbl(dvMyDatiStampa.Item(x)("importo"))
    '                Case ClsDettaglioVoci.CAPITOLO_NOLO
    '                    impNolo += CDbl(dvMyDatiStampa.Item(x)("importo"))
    '                Case ClsDettaglioVoci.CAPITOLO_QUOTAFISSA
    '                    impQuotaFissa += CDbl(dvMyDatiStampa.Item(x)("importo"))
    '                Case ClsDettaglioVoci.VOCE_DEPURAZIONEQF
    '                    impDepurazioneQF += CDbl(dvMyDatiStampa.Item(x)("importo"))
    '                Case ClsDettaglioVoci.VOCE_FOGNATURAQF
    '                    impFognaturaQF += CDbl(dvMyDatiStampa.Item(x)("importo"))
    '                Case ClsDettaglioVoci.CAPITOLO_IVA
    '                    impIVA += CDbl(dvMyDatiStampa.Item(x)("importo"))
    '                Case ClsDettaglioVoci.CAPITOLO_ARROTONDAMENTO
    '                    impArrotondamento += (dvMyDatiStampa.Item(x)("importo"))
    '            End Select
    '            impTotale += CDbl(dvMyDatiStampa.Item(x)("importo"))
    '            sRigaPrec = CStr(dvMyDatiStampa.Item(x)("id")) & CStr(dvMyDatiStampa.Item(x)("cod_utente")) & CStr(dvMyDatiStampa.Item(x)("data_fattura")) & CStr(dvMyDatiStampa.Item(x)("numero_fattura")) & CStr(dvMyDatiStampa.Item(x)("provenienza")) & CStr(dvMyDatiStampa.Item(x)("data_accredito")) & CStr(dvMyDatiStampa.Item(x)("data_pagamento"))
    '        Next
    '        'inserisco i valori della riga precedente
    '        sDatiStampa += "|" + FormatNumber(impConsumo.ToString, 2)
    '        sDatiStampa += "|" + FormatNumber(impDepurazione.ToString, 2)
    '        sDatiStampa += "|" + FormatNumber(impFognatura.ToString, 2)
    '        sDatiStampa += "|" + FormatNumber(impAddizionali.ToString, 2)
    '        sDatiStampa += "|" + FormatNumber(impNolo.ToString, 2)
    '        sDatiStampa += "|" + FormatNumber(impQuotaFissa.ToString, 2)
    '        sDatiStampa += "|" + FormatNumber(impDepurazioneQF.ToString, 2)
    '        sDatiStampa += "|" + FormatNumber(impFognaturaQF.ToString, 2)
    '        sDatiStampa += "|" + FormatNumber(impIVA.ToString, 2)
    '        sDatiStampa += "|" + FormatNumber(impArrotondamento.ToString, 2)
    '        sDatiStampa += "|" + FormatNumber(impTotale.ToString, 2)
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'incremento i totalizzatori
    '        impTotEmesso += impEmesso
    '        impTotConsumo += impConsumo
    '        impTotDepurazione += impDepurazione
    '        impTotFognatura += impFognatura
    '        impTotAddizionali += impAddizionali
    '        impTotNolo += impNolo
    '        impTotQuotaFissa += impQuotaFissa
    '        impTotDepurazioneQF += impDepurazioneQF
    '        impTotFognaturaQF += impFognaturaQF
    '        impTotIVA += impIVA
    '        impTotArrotondamento += impArrotondamento
    '        impTotTotale += impTotale
    '        'inserisco i totalizzatori
    '        sDatiStampa = "||||Totale"
    '        sDatiStampa += "|" + FormatNumber(impTotEmesso.ToString, 2)
    '        sDatiStampa += "||||" + FormatNumber(impTotConsumo.ToString, 2)
    '        sDatiStampa += "|" + FormatNumber(impTotDepurazione.ToString, 2)
    '        sDatiStampa += "|" + FormatNumber(impTotFognatura.ToString, 2)
    '        sDatiStampa += "|" + FormatNumber(impTotAddizionali.ToString, 2)
    '        sDatiStampa += "|" + FormatNumber(impTotNolo.ToString, 2)
    '        sDatiStampa += "|" + FormatNumber(impTotQuotaFissa.ToString, 2)
    '        sDatiStampa += "|" + FormatNumber(impTotDepurazioneQF.ToString, 2)
    '        sDatiStampa += "|" + FormatNumber(impTotFognaturaQF.ToString, 2)
    '        sDatiStampa += "|" + FormatNumber(impTotIVA.ToString, 2)
    '        sDatiStampa += "|" + FormatNumber(impTotArrotondamento.ToString, 2)
    '        sDatiStampa += "|" + FormatNumber(impTotTotale.ToString, 2)
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        Return DtStampa
    '    Catch Err As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsStampaXLS.PrintPagamentiH2O.errore: ", Err)
    '        Return Nothing
    '    End Try
    'End Function

    Public Function PrintRimborsiH2O(ByVal dvMyDatiStampa As DataView, ByVal sIntestazioneEnte As String) As DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim x As Integer
        Dim iLenght As Integer = 8
        Dim TotEmesso, TotPagato, TotDif As Double

        Try
            'carico il dataset
            DsStampa.Tables.Add("STAMPA")
            'carico le colonne nel dataset
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
            'inserisco l'intestazione del blocco
            sDatiStampa = "Elenco Rimborsi"
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco le intestazioni di colonna
            sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.IVA|Data Fattura|N.Fattura|Periodo|Importo Emesso|Importo Pagato|Importo Rimborso"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = ""
            'ciclo sui dati da stampare
            For x = 0 To dvMyDatiStampa.Count - 1
                'inizializzo la nuova riga
                sDatiStampa = ""
                sDatiStampa += CStr(dvMyDatiStampa.Item(x)("cognome")).ToUpper
                sDatiStampa += "|" + CStr(dvMyDatiStampa.Item(x)("nome")).ToUpper
                sDatiStampa += "|'" + CStr(dvMyDatiStampa.Item(x)("cfpiva")).ToUpper
                sDatiStampa += "|'" + oReplace.GiraDataFromDB(CStr(dvMyDatiStampa.Item(x)("data_fattura")))
                sDatiStampa += "|" + CStr(dvMyDatiStampa.Item(x)("numero_fattura"))
                sDatiStampa += "|" + CStr(dvMyDatiStampa.Item(x)("periodo"))
                sDatiStampa += "|" + FormatNumber(CStr(dvMyDatiStampa.Item(x)("impemesso")), 2)
                TotEmesso += CDbl(dvMyDatiStampa.Item(x)("impemesso"))
                sDatiStampa += "|" + FormatNumber(CStr(dvMyDatiStampa.Item(x)("pagato")), 2)
                TotPagato += CDbl(dvMyDatiStampa.Item(x)("pagato"))
                sDatiStampa += "|" + FormatNumber(CStr(CDbl(dvMyDatiStampa.Item(x)("dif")) * -1), 2)
                TotDif += CDbl(dvMyDatiStampa.Item(x)("dif")) * -1
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
            Next
            'inserisco i totali
            sDatiStampa = "Totale Dovuto:" & FormatNumber(CStr(TotEmesso), 2)
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = "Totale Pagato:" & FormatNumber(CStr(TotPagato), 2)
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = "Totale Insoluto:" & FormatNumber(CStr(TotDif), 2)
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            Return DtStampa
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsStampaXLS.PrintRimborsiH2O.errore: ", Err)
            Return Nothing
        End Try
    End Function

    Public Function PrintInsolutiH2O(ByVal dvMyDatiStampa As DataView, ByVal sIntestazioneEnte As String) As DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim x As Integer
        Dim iLenght As Integer = 8
        Dim TotEmesso, TotPagato, TotDif As Double

        Try
            'carico il dataset
            DsStampa.Tables.Add("STAMPA")
            'carico le colonne nel dataset
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
            'inserisco l'intestazione del blocco
            sDatiStampa = "Elenco Insoluti"
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco le intestazioni di colonna
            sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.IVA|Data Fattura|N.Fattura|Periodo|Importo Emesso|Importo Pagato|Importo Insoluto"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = ""
            'ciclo sui dati da stampare
            For x = 0 To dvMyDatiStampa.Count - 1
                'inizializzo la nuova riga
                sDatiStampa = ""
                sDatiStampa += CStr(dvMyDatiStampa.Item(x)("cognome")).ToUpper
                sDatiStampa += "|" + CStr(dvMyDatiStampa.Item(x)("nome")).ToUpper
                sDatiStampa += "|'" + CStr(dvMyDatiStampa.Item(x)("cfpiva")).ToUpper
                sDatiStampa += "|'" + oReplace.GiraDataFromDB(CStr(dvMyDatiStampa.Item(x)("data_fattura")))
                sDatiStampa += "|" + CStr(dvMyDatiStampa.Item(x)("numero_fattura"))
                sDatiStampa += "|" + CStr(dvMyDatiStampa.Item(x)("periodo"))
                sDatiStampa += "|" + FormatNumber(CStr(dvMyDatiStampa.Item(x)("impemesso")), 2)
                TotEmesso += CDbl(dvMyDatiStampa.Item(x)("impemesso"))
                sDatiStampa += "|" + FormatNumber(CStr(dvMyDatiStampa.Item(x)("pagato")), 2)
                TotPagato += CDbl(dvMyDatiStampa.Item(x)("pagato"))
                sDatiStampa += "|" + FormatNumber(CStr(dvMyDatiStampa.Item(x)("dif")), 2)
                TotDif += CDbl(dvMyDatiStampa.Item(x)("dif"))
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
            Next
            'inserisco una riga vuota
            sDatiStampa = ""
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco i totali
            sDatiStampa = "Totale Dovuto:" & FormatNumber(CStr(TotEmesso), 2)
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = "Totale Pagato:" & FormatNumber(CStr(TotPagato), 2)
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = "Totale Insoluto:" & FormatNumber(CStr(TotDif), 2)
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            Return DtStampa
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsStampaXLS.PrintInsolutiH2O.errore: ", Err)
            Return Nothing
        End Try
    End Function

    Public Function PrintRiversamentoH2O(ByVal dvMyDatiStampa As DataView, ByVal sIntestazioneEnte As String) As DataTable
        Dim sDatiStampa, sRigaPrec As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim x As Integer
        Dim iLenght As Integer = 12   '10
        Dim FncDettaglio As New ClsDettaglioVoci
        Dim impConsumo, impDepurazione, impFognatura, impAddizionali, impNolo, impQuotaFissa, impIVA, impArrotondamento, impTotale, impDepurazioneQF, impFognaturaQF As Double
        Dim impTotConsumo, impTotDepurazione, impTotFognatura, impTotAddizionali, impTotNolo, impTotQuotaFissa, impTotIVA, impTotArrotondamento, impTotTotale, impTotDepurazioneQF, impTotFognaturaQF As Double

        sDatiStampa = "" : sRigaPrec = ""
        Try
            'carico il dataset
            DsStampa.Tables.Add("STAMPA")
            'carico le colonne nel dataset
            For x = 1 To iLenght + 1      '11
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
            'inserisco l'intestazione del blocco
            sDatiStampa = "Dati Riversamento"
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco le intestazioni di colonna
            '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
            'sDatiStampa = "Anno Riferimento|Data Accredito|Importo Acqua|Importo Depurazione|Importo Fognatura|Importo Addizionali|Importo Nolo|Importo Quota Fissa|Importo IVA|Importo Arrotondamento|Importo Totale"
            sDatiStampa = "Anno Riferimento|Data Accredito|Importo Acqua|Importo Depurazione|Importo Fognatura|Importo Addizionali|Importo Nolo|Importo Quota Fissa Acqua|Importo Quota Fissa Depurazione|Importo Quota Fissa Fognatura|Importo IVA|Importo Arrotondamento|Importo Totale"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = ""
            'ciclo sui dati da stampare
            For x = 0 To dvMyDatiStampa.Count - 1
                If sRigaPrec <> CStr(dvMyDatiStampa.Item(x)("anno_fattura")) & CStr(dvMyDatiStampa.Item(x)("data_accredito")) Then
                    'inserisco i valori della riga precedente
                    If sDatiStampa <> "" Then
                        sDatiStampa += "|" + FormatNumber(impConsumo.ToString, 2)
                        sDatiStampa += "|" + FormatNumber(impDepurazione.ToString, 2)
                        sDatiStampa += "|" + FormatNumber(impFognatura.ToString, 2)
                        sDatiStampa += "|" + FormatNumber(impAddizionali.ToString, 2)
                        sDatiStampa += "|" + FormatNumber(impNolo.ToString, 2)
                        sDatiStampa += "|" + FormatNumber(impQuotaFissa.ToString, 2)
                        sDatiStampa += "|" + FormatNumber(impDepurazioneQF.ToString, 2)
                        sDatiStampa += "|" + FormatNumber(impFognaturaQF.ToString, 2)
                        sDatiStampa += "|" + FormatNumber(impIVA.ToString, 2)
                        sDatiStampa += "|" + FormatNumber(impArrotondamento.ToString, 2)
                        sDatiStampa += "|" + FormatNumber(impTotale.ToString, 2)
                        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                            Return Nothing
                        End If
                        'incremento i totalizzatori
                        impTotConsumo += impConsumo
                        impTotDepurazione += impDepurazione
                        impTotFognatura += impFognatura
                        impTotAddizionali += impAddizionali
                        impTotNolo += impNolo
                        impTotQuotaFissa += impQuotaFissa
                        impTotDepurazioneQF += impDepurazioneQF
                        impTotFognaturaQF += impFognaturaQF
                        impTotIVA += impIVA
                        impTotArrotondamento += impArrotondamento
                        impTotTotale += impTotale
                    End If
                    'inizializzo la nuova riga
                    sDatiStampa = "" : impConsumo = 0 : impDepurazione = 0 : impFognatura = 0 : impAddizionali = 0 : impNolo = 0 : impQuotaFissa = 0 : impDepurazioneQF = 0 : impFognaturaQF = 0 : impIVA = 0 : impArrotondamento = 0 : impTotale = 0
                    sDatiStampa += CStr(dvMyDatiStampa.Item(x)("anno_fattura"))
                    sDatiStampa += "|" + oReplace.GiraDataFromDB(CStr(dvMyDatiStampa.Item(x)("data_accredito")))
                End If
                Select Case CStr(dvMyDatiStampa.Item(x)("cod_capitolo"))
                    Case ClsDettaglioVoci.CAPITOLO_CONSUMO
                        impConsumo += CDbl(dvMyDatiStampa.Item(x)("importo"))
                    Case ClsDettaglioVoci.CAPITOLO_CANONI
                        Select Case dvMyDatiStampa.Item(x)("idvoce").ToString
                            Case ClsDettaglioVoci.VOCE_DEPURAZIONE
                                impDepurazione += CDbl(dvMyDatiStampa.Item(x)("importo"))
                            Case ClsDettaglioVoci.VOCE_FOGNATURA
                                impFognatura += CDbl(dvMyDatiStampa.Item(x)("importo"))
                        End Select
                    Case ClsDettaglioVoci.CAPITOLO_ADDIZIONALI
                        impAddizionali += CDbl(dvMyDatiStampa.Item(x)("importo"))
                    Case ClsDettaglioVoci.CAPITOLO_NOLO
                        impNolo += CDbl(dvMyDatiStampa.Item(x)("importo"))
                    Case ClsDettaglioVoci.CAPITOLO_QUOTAFISSA
                        impQuotaFissa += CDbl(dvMyDatiStampa.Item(x)("importo"))
                    Case ClsDettaglioVoci.VOCE_DEPURAZIONEQF
                        impDepurazioneQF += CDbl(dvMyDatiStampa.Item(x)("importo"))
                    Case ClsDettaglioVoci.VOCE_FOGNATURAQF
                        impFognaturaQF += CDbl(dvMyDatiStampa.Item(x)("importo"))
                    Case ClsDettaglioVoci.CAPITOLO_IVA
                        impIVA += CDbl(dvMyDatiStampa.Item(x)("importo"))
                    Case ClsDettaglioVoci.CAPITOLO_ARROTONDAMENTO
                        impArrotondamento += (dvMyDatiStampa.Item(x)("importo"))
                End Select
                impTotale += CDbl(dvMyDatiStampa.Item(x)("importo"))
                sRigaPrec = CStr(dvMyDatiStampa.Item(x)("anno_fattura")) & CStr(dvMyDatiStampa.Item(x)("data_accredito"))
            Next
            'inserisco i valori della riga precedente
            sDatiStampa += "|" + FormatNumber(impConsumo.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impDepurazione.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impFognatura.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impAddizionali.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impNolo.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impQuotaFissa.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impDepurazioneQF.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impFognaturaQF.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impIVA.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impArrotondamento.ToString, 2)
            sDatiStampa += "|" + FormatNumber(impTotale.ToString, 2)
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'incremento i totalizzatori
            impTotConsumo += impConsumo
            impTotDepurazione += impDepurazione
            impTotFognatura += impFognatura
            impTotAddizionali += impAddizionali
            impTotNolo += impNolo
            impTotQuotaFissa += impQuotaFissa
            impTotDepurazioneQF += impDepurazioneQF
            impTotFognaturaQF += impFognaturaQF
            impTotIVA += impIVA
            impTotArrotondamento += impArrotondamento
            impTotTotale += impTotale
            'inserisco i totalizzatori
            sDatiStampa = "|Totale"
            sDatiStampa += "|" + impTotConsumo.ToString
            sDatiStampa += "|" + impTotDepurazione.ToString
            sDatiStampa += "|" + impTotFognatura.ToString
            sDatiStampa += "|" + impTotAddizionali.ToString
            sDatiStampa += "|" + impTotNolo.ToString
            sDatiStampa += "|" + impTotQuotaFissa.ToString
            sDatiStampa += "|" + impTotDepurazioneQF.ToString
            sDatiStampa += "|" + impTotFognaturaQF.ToString
            sDatiStampa += "|" + impTotIVA.ToString
            sDatiStampa += "|" + impTotArrotondamento.ToString
            sDatiStampa += "|" + impTotTotale.ToString
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            Return DtStampa
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsStampaXLS.PrintRiversamentoH2O.errore: ", Err)
            Return Nothing
        End Try
    End Function

    Public Function PrintStampaLetturista(ByVal dvMyDatiStampa As DataView, ByVal sIntestazioneEnte As String, ByVal sPeriodo As String) As DataTable
        Dim sDatiStampa, sUbicazione As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim x As Integer
        Dim iLenght As Integer = 35

        Try
            'carico il dataset
            DsStampa.Tables.Add("STAMPA")
            'carico le colonne nel dataset
            For x = 1 To 36
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
            'inserisco il periodo
            sDatiStampa = "Periodo " & sPeriodo
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
            'inserisco il titolo report
            sDatiStampa = "Stampa Letturista"
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco le intestazioni dei blocchi di colonna
            sDatiStampa = "|Dati Intestatario|||||||||Dati Utente||||||||||Ubicazione|||||||||||||"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco le intestazioni di colonna
            sDatiStampa = "Codice Contatore|Cognome|Nome|Cod.Fiscale/P.IVA|Via|Civico|Esponente|Cap|Comune|Provincia|"
            sDatiStampa += "N.Utente|Cognome|Nome|Cod.Fiscale/P.IVA|Via|Civico|Esponente|Cap|Comune|Provincia|"
            sDatiStampa += "Via|Civico|Esponente|Matricola|Tipo Utenza|N.Utenze|Giro|Sequenza|Posizione|Matricola Contatore Principale|Data Lettura Precedente|Lettura Precedente|Consumo Precedente|Data Lettura|Lettura"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = ""
            'ciclo sui dati da stampare
            For x = 0 To dvMyDatiStampa.Count - 1
                sUbicazione = ""
                sDatiStampa = dvMyDatiStampa.Item(x)("codcontatore").ToString()
                'dati intestatario
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("cognome_int")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("COGNOME_INT")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("nome_int")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("NOME_INT")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("COD_FISCALE_INT")) Then
                    sDatiStampa += "'" & dvMyDatiStampa.Item(x)("COD_FISCALE_INT")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("VIA_INT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("VIA_INT")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("CIVICO_INT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("CIVICO_INT")
                End If
                If Not IsDBNull(dvMyDatiStampa.Item(x)("esponente_civico_int")) Then
                    If CStr(dvMyDatiStampa.Item(x)("esponente_civico_int")) <> "-1" And CStr(dvMyDatiStampa.Item(x)("esponente_civico_int")) <> "0" Then
                        sDatiStampa += "|" + "'" & dvMyDatiStampa.Item(x)("esponente_civico_int")
                    Else
                        sDatiStampa += "|"
                    End If
                Else
                    sDatiStampa += "|"
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("CAP_INT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("CAP_INT")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("COMUNE_INT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("COMUNE_INT")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("PROVINCIA_INT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("PROVINCIA_INT")
                End If

                'dati utente
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("NUMEROUTENTE")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("NUMEROUTENTE")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("COGNOME_UT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("COGNOME_UT")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("NOME_UT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("NOME_UT")
                End If

                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("COD_FISCALE_UT")) Then
                    sDatiStampa += "'" & dvMyDatiStampa.Item(x)("COD_FISCALE_UT")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("VIA_UT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("VIA_UT")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("CIVICO_UT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("CIVICO_UT")
                End If
                If Not IsDBNull(dvMyDatiStampa.Item(x)("esponente_civico_ut")) Then
                    If CStr(dvMyDatiStampa.Item(x)("esponente_civico_ut")) <> "-1" And CStr(dvMyDatiStampa.Item(x)("esponente_civico_ut")) <> "0" Then
                        sDatiStampa += "|" + "'" & dvMyDatiStampa.Item(x)("esponente_civico_ut")
                    Else
                        sDatiStampa += "|"
                    End If
                Else
                    sDatiStampa += "|"
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("CAP_UT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("CAP_UT")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("COMUNE_UT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("COMUNE_UT")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("PROVINCIA_UT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("PROVINCIA_UT")
                End If
                'Dati Ubicazione
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("VIA_UBICAZIONE")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("VIA_UBICAZIONE")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("civico_ubicazione")) Then
                    sUbicazione += dvMyDatiStampa.Item(x)("civico_ubicazione")
                End If
                sDatiStampa += sUbicazione.Trim
                If Not IsDBNull(dvMyDatiStampa.Item(x)("esponente_civico")) Then
                    If CStr(dvMyDatiStampa.Item(x)("esponente_civico")) <> "-1" And CStr(dvMyDatiStampa.Item(x)("esponente_civico")) <> "0" Then
                        sDatiStampa += "|" + "'" & dvMyDatiStampa.Item(x)("esponente_civico")
                    Else
                        sDatiStampa += "|"
                    End If
                Else
                    sDatiStampa += "|"
                End If
                'dati contatore
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("MATRICOLA")) Then
                    sDatiStampa += "'" & dvMyDatiStampa.Item(x)("MATRICOLA")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("TipoUtenza")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("TipoUtenza")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("numeroutenze")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("numeroutenze").ToString()
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("descrgiro")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("descrgiro")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("sequenza")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("sequenza")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("posizione")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("posizione")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("issubcont")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("issubcont")
                End If

                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("MAXDATALETTURA")) Then
                    sDatiStampa += oReplace.GiraDataFromDB(dvMyDatiStampa.Item(x)("MAXDATALETTURA"))
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("LETTURA")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("LETTURA").ToString()
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("CONSUMOPREC")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("CONSUMOPREC").ToString()
                End If

                sDatiStampa += "|"
                sDatiStampa += "|"
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
            Next
            Return DtStampa
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsStampaXLS.PrintStampaLetturista.errore: ", Err)
            Return Nothing
        End Try
    End Function

    Public Function PrintContatoriCessati(ByVal dvMyDatiStampa As DataView, ByVal sIntestazioneEnte As String, ByVal sPeriodo As String) As DataTable
        Dim sDatiStampa, sCivico As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim x As Integer
        Dim iLenght As Integer = 29

        Try
            'carico il dataset
            DsStampa.Tables.Add("STAMPA")
            'carico le colonne nel dataset
            For x = 1 To 30
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
            'inserisco il periodo
            sDatiStampa = "Periodo " & sPeriodo
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
            'inserisco il titolo report
            sDatiStampa = "Elenco Contatori Cessati, Sostituiti, Sospesi"
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco le intestazioni dei blocchi di colonna
            sDatiStampa = "Dati Intestatario|||||||||Dati Utente||||||||||Ubicazione|||||||||"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco le intestazioni di colonna
            sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.IVA|Via|Civico|Esponente|Cap|Comune|Provincia|"
            sDatiStampa += "N.Utente|Cognome|Nome|Cod.Fiscale/P.IVA|Via|Civico|Esponente|Cap|Comune|Provincia|"
            sDatiStampa += "Via|Civico|Esponente|Matricola|Tipo Utenza|N.Utenze|Data Sostituzione|Data Cessazione|Data Rimozione Temporanea|Data Sospensione"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = ""
            'ciclo sui dati da stampare
            For x = 0 To dvMyDatiStampa.Count - 1
                sDatiStampa = ""
                'dati Intestatario
                If Not IsDBNull(dvMyDatiStampa.Item(x)("COGNOME_INT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("COGNOME_INT")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("NOME_INT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("NOME_INT")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("COD_FISCALE_INT")) Then
                    sDatiStampa += "'" & dvMyDatiStampa.Item(x)("COD_FISCALE_INT")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("VIA_INT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("VIA_INT")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("CIVICO_INT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("CIVICO_INT")
                End If
                If Not IsDBNull(dvMyDatiStampa.Item(x)("esponente_civico_int")) Then
                    If CStr(dvMyDatiStampa.Item(x)("esponente_civico_int")) <> "-1" And CStr(dvMyDatiStampa.Item(x)("esponente_civico_int")) <> "0" Then
                        sDatiStampa += "|" + "'" & dvMyDatiStampa.Item(x)("esponente_civico_int")
                    Else
                        sDatiStampa += "|"
                    End If
                Else
                    sDatiStampa += "|"
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("CAP_INT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("CAP_INT")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("COMUNE_INT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("COMUNE_INT")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("PROVINCIA_INT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("PROVINCIA_INT")
                End If
                'dati utente
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("NUMEROUTENTE")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("NUMEROUTENTE")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("COGNOME_UT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("COGNOME_UT")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("NOME_UT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("NOME_UT")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("COD_FISCALE_UT")) Then
                    sDatiStampa += "'" & dvMyDatiStampa.Item(x)("COD_FISCALE_UT")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("VIA_UT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("VIA_UT")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("CIVICO_UT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("CIVICO_UT")
                End If
                If Not IsDBNull(dvMyDatiStampa.Item(x)("esponente_civico_UT")) Then
                    If CStr(dvMyDatiStampa.Item(x)("esponente_civico_UT")) <> "-1" And CStr(dvMyDatiStampa.Item(x)("esponente_civico_UT")) <> "0" Then
                        sDatiStampa += "|" + "'" & dvMyDatiStampa.Item(x)("esponente_civico_UT")
                    Else
                        sDatiStampa += "|"
                    End If
                Else
                    sDatiStampa += "|"
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("CAP_UT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("CAP_UT")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("COMUNE_UT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("COMUNE_UT")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("PROVINCIA_UT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("PROVINCIA_UT")
                End If
                'Ubicazione contatore
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("VIA_UBICAZIONE")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("VIA_UBICAZIONE")
                End If
                sDatiStampa += "|"
                sCivico = ""
                If Not IsDBNull(dvMyDatiStampa.Item(x)("CIVICO_UBICAZIONE")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("CIVICO_UBICAZIONE")
                End If
                sDatiStampa += sCivico
                If Not IsDBNull(dvMyDatiStampa.Item(x)("esponente_civico")) Then
                    If CStr(dvMyDatiStampa.Item(x)("esponente_civico")) <> "-1" And CStr(dvMyDatiStampa.Item(x)("esponente_civico")) <> "0" Then
                        sDatiStampa += "|" + "'" & dvMyDatiStampa.Item(x)("esponente_civico")
                    Else
                        sDatiStampa += "|"
                    End If
                Else
                    sDatiStampa += "|"
                End If
                'descrizione utenza
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("MATRICOLA")) Then
                    sDatiStampa += "'" & dvMyDatiStampa.Item(x)("MATRICOLA")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("tipoutenza")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("tipoutenza")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("NUMEROUTENZE")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("NUMEROUTENZE").ToString()
                End If
                'DATE
                'DATASOSTITUZIONE|DATACESSAZIONE|DATASOSPENSIONEUTENZA|DATARIMOZIONETEMPORANEA
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("DATASOSTITUZIONE")) Then
                    sDatiStampa += oReplace.GiraDataFromDB(dvMyDatiStampa.Item(x)("DATASOSTITUZIONE"))
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("DATACESSAZIONE")) Then
                    sDatiStampa += oReplace.GiraDataFromDB(dvMyDatiStampa.Item(x)("DATACESSAZIONE"))
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("DATASOSPENSIONEUTENZA")) Then
                    sDatiStampa += oReplace.GiraDataFromDB(dvMyDatiStampa.Item(x)("DATASOSPENSIONEUTENZA"))
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("DATARIMOZIONETEMPORANEA")) Then
                    sDatiStampa += oReplace.GiraDataFromDB(dvMyDatiStampa.Item(x)("DATARIMOZIONETEMPORANEA"))
                End If
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
            Next
            Return DtStampa
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsStampaXLS.PrimiContatoriCessati.errore: ", Err)
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="dvMyDatiStampa"></param>
    ''' <param name="sIntestazioneEnte"></param>
    ''' <param name="sPeriodo"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Esportazione completa dati</em>
    ''' </revision>
    ''' </revisionHistory>
    Public Function PrintLetture(ByVal dvMyDatiStampa As DataView, ByVal sIntestazioneEnte As String, ByVal sPeriodo As String) As DataTable
        Dim sDatiStampa As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim x As Integer
        Dim iLenght As Integer = 30

        sDatiStampa = ""
        Try
            'carico il dataset
            DsStampa.Tables.Add("STAMPA")
            'carico le colonne nel dataset
            For x = 1 To 31
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
            'inserisco il periodo
            sDatiStampa = "Periodo " & sPeriodo
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
            'inserisco il titolo report
            sDatiStampa = "Elenco Letture"
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco le intestazioni dei blocchi di colonna
            sDatiStampa = "Dati Intestatario|||||||||Dati Utente||||||||||Ubicazione||||||||||"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco le intestazioni di colonna
            sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.IVA|Via|Civico|Esponente|Cap|Comune|Provincia|"
            sDatiStampa += "N.Utente|Cognome|Nome|Cod.Fiscale/P.IVA|Via|Civico|Esponente|Cap|Comune|Provincia|"
            sDatiStampa += "Via|Civico|Esponente|Matricola|Tipo Utenza|N.Utenze|Data Lettura Precedente|Lettura Precedente|Data Lettura Attuale|Lettura Attuale|Prima Lettura"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = ""
            'ciclo sui dati da stampare
            For x = 0 To dvMyDatiStampa.Count - 1
                sDatiStampa = PrintLettureRowDati(dvMyDatiStampa.Item(x))
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
            Next
            Return DtStampa
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsStampaXLS.PrintLetture.errore: ", Err)
            Return Nothing
        End Try
    End Function
    'Public Function PrintLetture(ByVal dvMyDatiStampa As DataView, ByVal sIntestazioneEnte As String, ByVal sPeriodo As String) As DataTable
    '    Dim sDatiStampa, sCivico As String
    '    Dim DsStampa As New DataSet
    '    Dim DtStampa As New DataTable
    '    Dim x As Integer
    '    Dim iLenght As Integer = 30

    '    Try
    '        'carico il dataset
    '        DsStampa.Tables.Add("STAMPA")
    '        'carico le colonne nel dataset
    '        For x = 1 To 31
    '            DsStampa.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, "0"))
    '        Next
    '        'carico il datatable
    '        DtStampa = DsStampa.Tables("STAMPA")
    '        'inserisco l'intestazione dell'ente
    '        sDatiStampa = sIntestazioneEnte
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco una riga vuota
    '        sDatiStampa = ""
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco il periodo
    '        sDatiStampa = "Periodo " & sPeriodo
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco una riga vuota
    '        sDatiStampa = ""
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco il titolo report
    '        sDatiStampa = "Elenco Letture"
    '        sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco le intestazioni dei blocchi di colonna
    '        sDatiStampa = "Dati Intestatario|||||||||Dati Utente||||||||||Ubicazione||||||||||"
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        'inserisco le intestazioni di colonna
    '        sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.IVA|Via|Civico|Esponente|Cap|Comune|Provincia|"
    '        sDatiStampa += "N.Utente|Cognome|Nome|Cod.Fiscale/P.IVA|Via|Civico|Esponente|Cap|Comune|Provincia|"
    '        sDatiStampa += "Via|Civico|Esponente|Matricola|Tipo Utenza|N.Utenze|Data Lettura Precedente|Lettura Precedente|Data Lettura Attuale|Lettura Attuale|Prima Lettura"
    '        If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '            Return Nothing
    '        End If
    '        sDatiStampa = ""
    '        'ciclo sui dati da stampare
    '        For x = 0 To dvMyDatiStampa.Count - 1
    '            sDatiStampa = ""
    '            'dati Intestatario
    '            If Not IsDBNull(dvMyDatiStampa.Item(x)("COGNOME_INT")) Then
    '                sDatiStampa += dvMyDatiStampa.Item(x)("COGNOME_INT")
    '            End If
    '            sDatiStampa += "|"
    '            If Not IsDBNull(dvMyDatiStampa.Item(x)("NOME_INT")) Then
    '                sDatiStampa += dvMyDatiStampa.Item(x)("NOME_INT")
    '            End If
    '            sDatiStampa += "|"
    '            If Not IsDBNull(dvMyDatiStampa.Item(x)("COD_FISCALE_INT")) Then
    '                sDatiStampa += "'" & dvMyDatiStampa.Item(x)("COD_FISCALE_INT")
    '            End If
    '            sDatiStampa += "|"
    '            If Not IsDBNull(dvMyDatiStampa.Item(x)("VIA_INT")) Then
    '                sDatiStampa += dvMyDatiStampa.Item(x)("VIA_INT")
    '            End If
    '            sDatiStampa += "|"
    '            If Not IsDBNull(dvMyDatiStampa.Item(x)("CIVICO_INT")) Then
    '                sDatiStampa += dvMyDatiStampa.Item(x)("CIVICO_INT")
    '            End If
    '            If Not IsDBNull(dvMyDatiStampa.Item(x)("esponente_civico_int")) Then
    '                If CStr(dvMyDatiStampa.Item(x)("esponente_civico_int")) <> "-1" And CStr(dvMyDatiStampa.Item(x)("esponente_civico_int")) <> "0" Then
    '                    sDatiStampa += "|" + "'" & dvMyDatiStampa.Item(x)("esponente_civico_int")
    '                Else
    '                    sDatiStampa += "|"
    '                End If
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            sDatiStampa += "|"
    '            If Not IsDBNull(dvMyDatiStampa.Item(x)("CAP_INT")) Then
    '                sDatiStampa += dvMyDatiStampa.Item(x)("CAP_INT")
    '            End If
    '            sDatiStampa += "|"
    '            If Not IsDBNull(dvMyDatiStampa.Item(x)("COMUNE_INT")) Then
    '                sDatiStampa += dvMyDatiStampa.Item(x)("COMUNE_INT")
    '            End If
    '            sDatiStampa += "|"
    '            If Not IsDBNull(dvMyDatiStampa.Item(x)("PROVINCIA_INT")) Then
    '                sDatiStampa += dvMyDatiStampa.Item(x)("PROVINCIA_INT")
    '            End If
    '            'dati utente
    '            sDatiStampa += "|"
    '            If Not IsDBNull(dvMyDatiStampa.Item(x)("NUMEROUTENTE")) Then
    '                sDatiStampa += dvMyDatiStampa.Item(x)("NUMEROUTENTE")
    '            End If
    '            sDatiStampa += "|"
    '            If Not IsDBNull(dvMyDatiStampa.Item(x)("COGNOME_UT")) Then
    '                sDatiStampa += dvMyDatiStampa.Item(x)("COGNOME_UT")
    '            End If
    '            sDatiStampa += "|"
    '            If Not IsDBNull(dvMyDatiStampa.Item(x)("NOME_UT")) Then
    '                sDatiStampa += dvMyDatiStampa.Item(x)("NOME_UT")
    '            End If
    '            sDatiStampa += "|"
    '            If Not IsDBNull(dvMyDatiStampa.Item(x)("COD_FISCALE_UT")) Then
    '                sDatiStampa += "'" & dvMyDatiStampa.Item(x)("COD_FISCALE_UT")
    '            End If
    '            sDatiStampa += "|"
    '            If Not IsDBNull(dvMyDatiStampa.Item(x)("VIA_UT")) Then
    '                sDatiStampa += dvMyDatiStampa.Item(x)("VIA_UT")
    '            End If
    '            sDatiStampa += "|"
    '            If Not IsDBNull(dvMyDatiStampa.Item(x)("CIVICO_UT")) Then
    '                sDatiStampa += dvMyDatiStampa.Item(x)("CIVICO_UT")
    '            End If
    '            If Not IsDBNull(dvMyDatiStampa.Item(x)("esponente_civico_UT")) Then
    '                If CStr(dvMyDatiStampa.Item(x)("esponente_civico_UT")) <> "-1" And CStr(dvMyDatiStampa.Item(x)("esponente_civico_UT")) <> "0" Then
    '                    sDatiStampa += "|" + "'" & dvMyDatiStampa.Item(x)("esponente_civico_UT")
    '                Else
    '                    sDatiStampa += "|"
    '                End If
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            sDatiStampa += "|"
    '            If Not IsDBNull(dvMyDatiStampa.Item(x)("CAP_UT")) Then
    '                sDatiStampa += dvMyDatiStampa.Item(x)("CAP_UT")
    '            End If
    '            sDatiStampa += "|"
    '            If Not IsDBNull(dvMyDatiStampa.Item(x)("COMUNE_UT")) Then
    '                sDatiStampa += dvMyDatiStampa.Item(x)("COMUNE_UT")
    '            End If
    '            sDatiStampa += "|"
    '            If Not IsDBNull(dvMyDatiStampa.Item(x)("PROVINCIA_UT")) Then
    '                sDatiStampa += dvMyDatiStampa.Item(x)("PROVINCIA_UT")
    '            End If
    '            'Ubicazione contatore
    '            sDatiStampa += "|"
    '            If Not IsDBNull(dvMyDatiStampa.Item(x)("VIA_UBICAZIONE")) Then
    '                sDatiStampa += dvMyDatiStampa.Item(x)("VIA_UBICAZIONE")
    '            End If
    '            sDatiStampa += "|"
    '            sCivico = ""
    '            If Not IsDBNull(dvMyDatiStampa.Item(x)("CIVICO_UBICAZIONE")) Then
    '                sCivico += dvMyDatiStampa.Item(x)("CIVICO_UBICAZIONE")
    '            End If
    '            sDatiStampa += sCivico
    '            If Not IsDBNull(dvMyDatiStampa.Item(x)("esponente_civico")) Then
    '                If CStr(dvMyDatiStampa.Item(x)("esponente_civico")) <> "-1" And CStr(dvMyDatiStampa.Item(x)("esponente_civico")) <> "0" Then
    '                    sDatiStampa += "|" + "'" & dvMyDatiStampa.Item(x)("esponente_civico")
    '                Else
    '                    sDatiStampa += "|"
    '                End If
    '            Else
    '                sDatiStampa += "|"
    '            End If
    '            'descrizione utenza
    '            sDatiStampa += "|"
    '            If Not IsDBNull(dvMyDatiStampa.Item(x)("MATRICOLA")) Then
    '                sDatiStampa += "'" & dvMyDatiStampa.Item(x)("MATRICOLA")
    '            End If
    '            sDatiStampa += "|"
    '            If Not IsDBNull(dvMyDatiStampa.Item(x)("tipoutenza")) Then
    '                sDatiStampa += dvMyDatiStampa.Item(x)("tipoutenza")
    '            End If
    '            sDatiStampa += "|"
    '            If Not IsDBNull(dvMyDatiStampa.Item(x)("NUMEROUTENZE")) Then
    '                sDatiStampa += dvMyDatiStampa.Item(x)("NUMEROUTENZE").ToString()
    '            End If
    '            'letture
    '            sDatiStampa += "|"
    '            If Not IsDBNull(dvMyDatiStampa.Item(x)("DATALETTURAPRECEDENTE")) Then
    '                sDatiStampa += oReplace.GiraDataFromDB(dvMyDatiStampa.Item(x)("DATALETTURAPRECEDENTE"))
    '            End If
    '            sDatiStampa += "|"
    '            If Not IsDBNull(dvMyDatiStampa.Item(x)("LETTURAPRECEDENTE")) Then
    '                sDatiStampa += dvMyDatiStampa.Item(x)("LETTURAPRECEDENTE").ToString()
    '            End If
    '            sDatiStampa += "|"
    '            If Not IsDBNull(dvMyDatiStampa.Item(x)("DATALETTURAATTUALE")) Then
    '                sDatiStampa += oReplace.GiraDataFromDB(dvMyDatiStampa.Item(x)("DATALETTURAATTUALE"))
    '            End If
    '            sDatiStampa += "|"
    '            If Not IsDBNull(dvMyDatiStampa.Item(x)("LETTURAATTUALE")) Then
    '                sDatiStampa += dvMyDatiStampa.Item(x)("LETTURAATTUALE").ToString()
    '            End If
    '            sDatiStampa += "|"
    '            If Not IsDBNull(dvMyDatiStampa.Item(x)("PRIMALETTURA")) Then
    '                sDatiStampa += "x"
    '            End If
    '            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
    '                Return Nothing
    '            End If
    '        Next
    '        Return DtStampa
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsStampaXLS.PrintLetture.errore: ", Err)
    '        Return Nothing
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myRow"></param>
    ''' <returns></returns>
    Public Function PrintLettureRowDati(myRow As DataRowView) As String
        Dim sDatiStampa As String = ""
        Try
            'dati Intestatario
            If Not IsDBNull(myRow("COGNOME_INT")) Then
                sDatiStampa += myRow("COGNOME_INT")
            End If
            sDatiStampa += "|"
            If Not IsDBNull(myRow("NOME_INT")) Then
                sDatiStampa += myRow("NOME_INT")
            End If
            sDatiStampa += "|"
            If Not IsDBNull(myRow("COD_FISCALE_INT")) Then
                sDatiStampa += "'" & myRow("COD_FISCALE_INT")
            End If
            sDatiStampa += "|"
            If Not IsDBNull(myRow("VIA_INT")) Then
                sDatiStampa += myRow("VIA_INT")
            End If
            sDatiStampa += "|"
            If Not IsDBNull(myRow("CIVICO_INT")) Then
                sDatiStampa += myRow("CIVICO_INT")
            End If
            If Not IsDBNull(myRow("esponente_civico_int")) Then
                If CStr(myRow("esponente_civico_int")) <> "-1" And CStr(myRow("esponente_civico_int")) <> "0" Then
                    sDatiStampa += "|" + "'" & myRow("esponente_civico_int")
                Else
                    sDatiStampa += "|"
                End If
            Else
                sDatiStampa += "|"
            End If
            sDatiStampa += "|"
            If Not IsDBNull(myRow("CAP_INT")) Then
                sDatiStampa += myRow("CAP_INT")
            End If
            sDatiStampa += "|"
            If Not IsDBNull(myRow("COMUNE_INT")) Then
                sDatiStampa += myRow("COMUNE_INT")
            End If
            sDatiStampa += "|"
            If Not IsDBNull(myRow("PROVINCIA_INT")) Then
                sDatiStampa += myRow("PROVINCIA_INT")
            End If
            'dati utente
            sDatiStampa += "|"
            If Not IsDBNull(myRow("NUMEROUTENTE")) Then
                sDatiStampa += myRow("NUMEROUTENTE")
            End If
            sDatiStampa += "|"
            If Not IsDBNull(myRow("COGNOME_UT")) Then
                sDatiStampa += myRow("COGNOME_UT")
            End If
            sDatiStampa += "|"
            If Not IsDBNull(myRow("NOME_UT")) Then
                sDatiStampa += myRow("NOME_UT")
            End If
            sDatiStampa += "|"
            If Not IsDBNull(myRow("COD_FISCALE_UT")) Then
                sDatiStampa += "'" & myRow("COD_FISCALE_UT")
            End If
            sDatiStampa += "|"
            If Not IsDBNull(myRow("VIA_UT")) Then
                sDatiStampa += myRow("VIA_UT")
            End If
            sDatiStampa += "|"
            If Not IsDBNull(myRow("CIVICO_UT")) Then
                sDatiStampa += myRow("CIVICO_UT")
            End If
            If Not IsDBNull(myRow("esponente_civico_UT")) Then
                If CStr(myRow("esponente_civico_UT")) <> "-1" And CStr(myRow("esponente_civico_UT")) <> "0" Then
                    sDatiStampa += "|" + "'" & myRow("esponente_civico_UT")
                Else
                    sDatiStampa += "|"
                End If
            Else
                sDatiStampa += "|"
            End If
            sDatiStampa += "|"
            If Not IsDBNull(myRow("CAP_UT")) Then
                sDatiStampa += myRow("CAP_UT")
            End If
            sDatiStampa += "|"
            If Not IsDBNull(myRow("COMUNE_UT")) Then
                sDatiStampa += myRow("COMUNE_UT")
            End If
            sDatiStampa += "|"
            If Not IsDBNull(myRow("PROVINCIA_UT")) Then
                sDatiStampa += myRow("PROVINCIA_UT")
            End If
            'Ubicazione contatore
            sDatiStampa += "|"
            If Not IsDBNull(myRow("VIA_UBICAZIONE")) Then
                sDatiStampa += myRow("VIA_UBICAZIONE")
            End If
            sDatiStampa += "|"
            If Not IsDBNull(myRow("CIVICO_UBICAZIONE")) Then
                sDatiStampa += myRow("CIVICO_UBICAZIONE")
            End If
            If Not IsDBNull(myRow("esponente_civico")) Then
                If CStr(myRow("esponente_civico")) <> "-1" And CStr(myRow("esponente_civico")) <> "0" Then
                    sDatiStampa += "|" + "'" & myRow("esponente_civico")
                Else
                    sDatiStampa += "|"
                End If
            Else
                sDatiStampa += "|"
            End If
            'descrizione utenza
            sDatiStampa += "|"
            If Not IsDBNull(myRow("MATRICOLA")) Then
                sDatiStampa += "'" & myRow("MATRICOLA")
            End If
            sDatiStampa += "|"
            If Not IsDBNull(myRow("tipoutenza")) Then
                sDatiStampa += myRow("tipoutenza")
            End If
            sDatiStampa += "|"
            If Not IsDBNull(myRow("NUMEROUTENZE")) Then
                sDatiStampa += myRow("NUMEROUTENZE").ToString()
            End If
            'letture
            sDatiStampa += "|"
            If Not IsDBNull(myRow("DATALETTURAPRECEDENTE")) Then
                sDatiStampa += oReplace.GiraDataFromDB(myRow("DATALETTURAPRECEDENTE"))
            End If
            sDatiStampa += "|"
            If Not IsDBNull(myRow("LETTURAPRECEDENTE")) Then
                sDatiStampa += myRow("LETTURAPRECEDENTE").ToString()
            End If
            sDatiStampa += "|"
            If Not IsDBNull(myRow("DATALETTURAATTUALE")) Then
                sDatiStampa += oReplace.GiraDataFromDB(myRow("DATALETTURAATTUALE"))
            End If
            sDatiStampa += "|"
            If Not IsDBNull(myRow("LETTURAATTUALE")) Then
                sDatiStampa += myRow("LETTURAATTUALE").ToString()
            End If
            sDatiStampa += "|"
            If Not IsDBNull(myRow("PRIMALETTURA")) Then
                sDatiStampa += "x"
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return sDatiStampa
    End Function
    Public Function PrintLettureMancanti(ByVal dvMyDatiStampa As DataView, ByVal sIntestazioneEnte As String, ByVal sPeriodo As String) As DataTable
        Dim sDatiStampa, sCivico As String
        Dim DsStampa As New DataSet
        Dim DtStampa As New DataTable
        Dim x As Integer
        Dim iLenght As Integer = 25

        Try
            'carico il dataset
            DsStampa.Tables.Add("STAMPA")
            'carico le colonne nel dataset
            For x = 1 To 26
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
            'inserisco il periodo
            sDatiStampa = "Periodo " & sPeriodo
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
            'inserisco il titolo report
            sDatiStampa = "Elenco Contatori con Letture Mancanti"
            sDatiStampa = sDatiStampa.PadRight(sDatiStampa.Length + iLenght, "|")
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco le intestazioni dei blocchi di colonna
            sDatiStampa = "Dati Intestatario|||||||||Dati Utente||||||||||Ubicazione|||||"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            'inserisco le intestazioni di colonna
            sDatiStampa = "Cognome|Nome|Cod.Fiscale/P.IVA|Via|Civico|Esponente|Cap|Comune|Provincia|"
            sDatiStampa += "N.Utente|Cognome|Nome|Cod.Fiscale/P.IVA|Via|Civico|Esponente|Cap|Comune|Provincia|"
            sDatiStampa += "Via|Civico|Esponente|Matricola|Tipo Utenza|N.Utenze"
            If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                Return Nothing
            End If
            sDatiStampa = ""
            'ciclo sui dati da stampare
            For x = 0 To dvMyDatiStampa.Count - 1
                sDatiStampa = ""
                'dati Intestatario
                If Not IsDBNull(dvMyDatiStampa.Item(x)("COGNOME_INT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("COGNOME_INT")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("NOME_INT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("NOME_INT")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("COD_FISCALE_INT")) Then
                    sDatiStampa += "'" & dvMyDatiStampa.Item(x)("COD_FISCALE_INT")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("VIA_INT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("VIA_INT")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("CIVICO_INT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("CIVICO_INT")
                End If
                If Not IsDBNull(dvMyDatiStampa.Item(x)("esponente_civico_int")) Then
                    If CStr(dvMyDatiStampa.Item(x)("esponente_civico_int")) <> "-1" And CStr(dvMyDatiStampa.Item(x)("esponente_civico_int")) <> "0" Then
                        sDatiStampa += "|" + "'" & dvMyDatiStampa.Item(x)("esponente_civico_int")
                    Else
                        sDatiStampa += "|"
                    End If
                Else
                    sDatiStampa += "|"
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("CAP_INT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("CAP_INT")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("COMUNE_INT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("COMUNE_INT")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("PROVINCIA_INT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("PROVINCIA_INT")
                End If
                'dati utente
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("NUMEROUTENTE")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("NUMEROUTENTE")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("COGNOME_UT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("COGNOME_UT")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("NOME_UT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("NOME_UT")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("COD_FISCALE_UT")) Then
                    sDatiStampa += "'" & dvMyDatiStampa.Item(x)("COD_FISCALE_UT")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("VIA_UT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("VIA_UT")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("CIVICO_UT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("CIVICO_UT")
                End If
                If Not IsDBNull(dvMyDatiStampa.Item(x)("esponente_civico_UT")) Then
                    If CStr(dvMyDatiStampa.Item(x)("esponente_civico_UT")) <> "-1" And CStr(dvMyDatiStampa.Item(x)("esponente_civico_UT")) <> "0" Then
                        sDatiStampa += "|" + "'" & dvMyDatiStampa.Item(x)("esponente_civico_UT")
                    Else
                        sDatiStampa += "|"
                    End If
                Else
                    sDatiStampa += "|"
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("CAP_UT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("CAP_UT")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("COMUNE_UT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("COMUNE_UT")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("PROVINCIA_UT")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("PROVINCIA_UT")
                End If
                'Ubicazione contatore
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("VIA_UBICAZIONE")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("VIA_UBICAZIONE")
                End If
                sDatiStampa += "|"
                sCivico = ""
                If Not IsDBNull(dvMyDatiStampa.Item(x)("CIVICO_UBICAZIONE")) Then
                    sCivico += dvMyDatiStampa.Item(x)("CIVICO_UBICAZIONE")
                End If
                sDatiStampa += sCivico
                If Not IsDBNull(dvMyDatiStampa.Item(x)("esponente_civico")) Then
                    If CStr(dvMyDatiStampa.Item(x)("esponente_civico")) <> "-1" And CStr(dvMyDatiStampa.Item(x)("esponente_civico")) <> "0" Then
                        sDatiStampa += "|" + "'" & dvMyDatiStampa.Item(x)("esponente_civico")
                    Else
                        sDatiStampa += "|"
                    End If
                Else
                    sDatiStampa += "|"
                End If
                'descrizione utenza
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("MATRICOLA")) Then
                    sDatiStampa += "'" & dvMyDatiStampa.Item(x)("MATRICOLA")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("tipoutenza")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("tipoutenza")
                End If
                sDatiStampa += "|"
                If Not IsDBNull(dvMyDatiStampa.Item(x)("NUMEROUTENZE")) Then
                    sDatiStampa += dvMyDatiStampa.Item(x)("NUMEROUTENZE").ToString()
                End If
                If AddRowStampa(DtStampa, sDatiStampa) = 0 Then
                    Return Nothing
                End If
            Next
            Return DtStampa
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ClsStampaXLS.PrintLettureMancanti.errore: ", Err)
            Return Nothing
        End Try
    End Function
End Class
