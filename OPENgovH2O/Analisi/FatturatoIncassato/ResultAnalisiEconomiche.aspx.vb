Imports log4net

Partial Class ResultAnalisiEconomiche
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ResultAnalisiEconomiche))
    Private iDB As New DBAccess.getDBobject
    Private FncAnalisi As New ClsAnalisiLevelDB
	Private FncDettaglio As ClsDettaglioVoci

#Region " Web Form Designer Generated Code "

	'This call is required by the Web Form Designer.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
	Protected WithEvents LblTotFatturato As System.Web.UI.WebControls.Label
	Protected WithEvents LblFatturatoAcqua As System.Web.UI.WebControls.Label
	Protected WithEvents LblIncassatoAcqua As System.Web.UI.WebControls.Label

	'NOTE: The following placeholder declaration is required by the Web Form Designer.
	'Do not delete or move it.
	Private designerPlaceholderDeclaration As System.Object

	Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
		'CODEGEN: This method call is required by the Web Form Designer
		'Do not modify it using the code editor.
		InitializeComponent()
	End Sub

#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            If Not Page.IsPostBack Then
                Dim Anno As String = Request.Item("DdlAnno")
                Dim Periodo As String = Request.Item("DdlPeriodo")
                Dim DataAccreditoDal As String = Request.Item("AccreditoDal")
                Dim DataAccreditoAl As String = Request.Item("AccreditoAl")
                Dim sEnteRicerca As String = ConstSession.IdEnte

                '*** 20130204 - analisi economiche senza filtro per ente
                If CStr(Request.Item("TypeRicerca")).ToUpper = "NOENTE" Then
                    sEnteRicerca = ""
                    Label1.Text = "Dati Fatturato/Incassato per Comunità"
                    Label8.Text = "Riepilogo Generale per Comunità"
                    Label20.Text = "Dettaglio Fatturato per Comunità"
                    Label27.Text = "Dettaglio Incassato per Comunità"
                Else
                    Label1.Text = "Dati Fatturato/Incassato per Comune"
                    Label8.Text = "Riepilogo Generale per Comune"
                    Label20.Text = "Dettaglio Fatturato per Comune"
                    Label27.Text = "Dettaglio Incassato per Comune"
                End If
                '*** ***
                If DataAccreditoDal <> "" Then
                    DataAccreditoDal = CDate(DataAccreditoDal).ToString("yyyyMMdd")
                End If
                If DataAccreditoAl <> "" Then
                    DataAccreditoAl = CDate(DataAccreditoAl).ToString("yyyyMMdd")
                End If
                If LoadRiepilogoFatturato(sEnteRicerca, Anno, Periodo, DataAccreditoDal, DataAccreditoAl) = 0 Then
                    Response.End()
                End If
                If LoadDettaglioFatturato(sEnteRicerca, Anno, Periodo, DataAccreditoDal, DataAccreditoAl) = 0 Then
                    Response.End()
                End If
                If LoadDettaglioIncassato(sEnteRicerca, Anno, Periodo, DataAccreditoDal, DataAccreditoAl) = 0 Then
                    Response.End()
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultAnalisiEconomiche.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Function LoadRiepilogoFatturato(ByVal sMyEnte As String, ByVal Anno As String, ByVal Periodo As String, ByVal AccreditoDal As String, ByVal AccreditoAl As String) As Integer
        Dim dvMyDati As DataView
        Dim x, nUtenti, nFatture, nNoteCredito As Integer
        Dim impFatture, impNoteCredito As Double

        Try
            'azzero le text
            nUtenti = nFatture = nNoteCredito = 0
            impFatture = 0 : impNoteCredito = 0
            LblNUtenti.Text = "0"
            LblNFatture.Text = "0" : LblTotImpFatture.Text = FormatNumber("0", 2)
            LblNNoteCredito.Text = "0" : LblTotImpNoteCredito.Text = FormatNumber("0", 2)
            LblNNoteDaEmettere.Text = "0" : LblTotImpNoteDaEmettere.Text = FormatNumber("0", 2)
            LblNPagTot.Text = "0" : LblImpPagTot.Text = FormatNumber("0", 2)
            LblNPagParz.Text = "0" : LblImpPagParz.Text = FormatNumber("0", 2)
            LblInsoluto.Text = FormatNumber("0", 2) : LblPercentualeInsoluto.Text = FormatNumber("0", 2)
            'prelevo i dati dal db dell'Fatturato
            dvMyDati = FncAnalisi.GetRiepilogoEmesso(sMyEnte, Anno, Periodo, AccreditoDal, AccreditoAl)
            If Not dvMyDati Is Nothing Then
                For x = 0 To dvMyDati.Count - 1
                    Select Case CStr(dvMyDati.Item(x)("tipo_documento"))
                        Case "F"
                            If Not IsDBNull(dvMyDati.Item(x)("nutenti")) Then
                                nUtenti += FormatNumber(dvMyDati.Item(x)("nutenti"), 0)
                            End If
                            If Not IsDBNull(dvMyDati.Item(x)("nfatture")) Then
                                nFatture += FormatNumber(dvMyDati.Item(x)("nfatture"), 0)
                            End If
                            If Not IsDBNull(dvMyDati.Item(x)("impfatture")) Then
                                impFatture += FormatNumber(dvMyDati.Item(x)("impfatture"), 2)
                            End If
                        Case "N"
                            If Not IsDBNull(dvMyDati.Item(x)("nfatture")) Then
                                nNoteCredito += FormatNumber(dvMyDati.Item(x)("nfatture"), 0)
                            End If
                            If Not IsDBNull(dvMyDati.Item(x)("impfatture")) Then
                                impNoteCredito += FormatNumber(dvMyDati.Item(x)("impfatture"), 2)
                            End If
                    End Select
                Next
                LblNUtenti.Text = FormatNumber(nUtenti, 0)
                LblNFatture.Text = FormatNumber(nFatture, 0)
                LblTotImpFatture.Text = FormatNumber(impFatture, 2)
                LblNNoteCredito.Text = FormatNumber(nNoteCredito, 0)
                LblTotImpNoteCredito.Text = FormatNumber(impNoteCredito, 2)
            End If
            'prelevo i dati dal db delle note da emettere
            nNoteCredito = 0
            impNoteCredito = 0
            dvMyDati = FncAnalisi.GetRiepilogoDaEmettere(sMyEnte, Anno, Periodo, AccreditoDal, AccreditoAl)
            If Not dvMyDati Is Nothing Then
                For x = 0 To dvMyDati.Count - 1
                    If Not IsDBNull(dvMyDati.Item(x)("ndoc")) Then
                        nNoteCredito += FormatNumber(dvMyDati.Item(x)("ndoc"), 0)
                    End If
                    If Not IsDBNull(dvMyDati.Item(x)("impdoc")) Then
                        impNoteCredito += FormatNumber(dvMyDati.Item(x)("impdoc"), 2)
                    End If
                Next
                LblNNoteDaEmettere.Text = FormatNumber(nNoteCredito, 0)
                LblTotImpNoteDaEmettere.Text = FormatNumber(impNoteCredito, 2)
            End If
            'prelevo i dati dal db delle fatture evase totalmente
            dvMyDati = FncAnalisi.GetRiepilogoEmessoEvaso(sMyEnte, Anno, Periodo, AccreditoDal, AccreditoAl, 1)
            If Not dvMyDati Is Nothing Then
                For x = 0 To dvMyDati.Count - 1
                    LblNPagTot.Text = FormatNumber(dvMyDati.Item(x)("npag"), 0)
                    If Not IsDBNull(dvMyDati.Item(x)("imppag")) Then
                        If sMyEnte = "" Then
                            LblImpPagTot.Text = FormatNumber(dvMyDati.Item(x)("imppag"), 2)
                        Else
                            LblImpPagTot.Text = FormatNumber(dvMyDati.Item(x)("imppag"), 2)
                        End If
                    End If
                Next
            End If
            'prelevo i dati dal db delle fatture evase parzialmente
            dvMyDati = FncAnalisi.GetRiepilogoEmessoEvaso(sMyEnte, Anno, Periodo, AccreditoDal, AccreditoAl, 0)
            If Not dvMyDati Is Nothing Then
                For x = 0 To dvMyDati.Count - 1
                    LblNPagParz.Text = FormatNumber(dvMyDati.Item(x)("npag"), 0)
                    If Not IsDBNull(dvMyDati.Item(x)("imppag")) Then
                        LblImpPagParz.Text = FormatNumber(dvMyDati.Item(x)("imppag"), 2)
                    End If
                Next
            End If
            LblTotIncassato.Text = FormatNumber((CDbl(LblImpPagParz.Text) + CDbl(LblImpPagTot.Text)), 2)
            LblTotEmesso.Text = FormatNumber(CDbl(LblTotImpFatture.Text) + CDbl(LblTotImpNoteCredito.Text) + CDbl(LblTotImpNoteDaEmettere.Text), 2)
            LblEmesso.Text = LblTotEmesso.Text : LblIncassato.Text = LblTotIncassato.Text
            LblInsoluto.Text = FormatNumber(CDbl(LblEmesso.Text) - (CDbl(LblIncassato.Text)), 2)
            If CDbl(LblEmesso.Text) <> 0 Then
                LblPercentualeInsoluto.Text = FormatNumber(((CDbl(LblInsoluto.Text) * 100) / CDbl(LblEmesso.Text)), 2)
            Else
                LblPercentualeInsoluto.Text = "100,00"
            End If
            TxtIncassato.Text = LblTotIncassato.Text : TxtInsoluto.Text = LblInsoluto.Text
            Return 1
        Catch err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultAnalisiEconomiche.LoadRiepilogoFatturato.errore: ", err)
            Response.Redirect("../../PaginaErrore.aspx")
            Return 0
        End Try
    End Function

    Private Function LoadDettaglioFatturato(ByVal sMyEnte As String, ByVal Anno As String, ByVal Periodo As String, ByVal AccreditoDal As String, ByVal AccreditoAl As String) As Integer
        Dim dvMyDati As New DataView
        Dim x As Integer
        Dim Consumo, Fognatura, Depurazione, Nolo, QuotaFissa, Addizionali, Iva, Arrotondamento, FognaturaQF, DepurazioneQF As Double

        Try
            'azzero le text
            LblFatturatoConsumo.Text = FormatNumber(0, 2) : LblFatturatoFognatura.Text = FormatNumber(0, 2) : LblFatturatoDepurazione.Text = FormatNumber(0, 2) : LblFatturatoDepurazioneQF.Text = FormatNumber(0, 2) : LblFatturatoFognaturaQF.Text = FormatNumber(0, 2)
            LblFatturatoNolo.Text = FormatNumber(0, 2) : LblFatturatoQuotaFissa.Text = FormatNumber(0, 2)
            LblFatturatoAddizionali.Text = FormatNumber(0, 2) : LblFatturatoIVA.Text = FormatNumber(0, 2)
            LblFatturatoTot.Text = FormatNumber(0, 2)
            'prelevo i dati dal db dell'Fatturato
            dvMyDati = FncAnalisi.GetDettaglioEmesso(sMyEnte, Anno, Periodo, AccreditoDal, AccreditoAl)
            If Not dvMyDati Is Nothing Then
                For x = 0 To dvMyDati.Count - 1
                    Select Case CStr(dvMyDati.Item(x)("codice_capitolo"))
                        Case ClsDettaglioVoci.CAPITOLO_CONSUMO
                            Consumo += dvMyDati.Item(x)("importo")
                        Case ClsDettaglioVoci.CAPITOLO_CANONI
                            Select Case Utility.StringOperation.FormatString(dvMyDati.Item(x)("idvoce"))
                                Case ClsDettaglioVoci.VOCE_DEPURAZIONE
                                    Depurazione += dvMyDati.Item(x)("importo")
                                Case ClsDettaglioVoci.VOCE_FOGNATURA
                                    Fognatura += dvMyDati.Item(x)("importo")
                            End Select
                        Case ClsDettaglioVoci.CAPITOLO_ADDIZIONALI
                            Addizionali += dvMyDati.Item(x)("importo")
                        Case ClsDettaglioVoci.CAPITOLO_NOLO
                            Nolo += dvMyDati.Item(x)("importo")
                        Case ClsDettaglioVoci.CAPITOLO_QUOTAFISSA
                            QuotaFissa += dvMyDati.Item(x)("importo")
                        Case ClsDettaglioVoci.VOCE_DEPURAZIONEQF
                            DepurazioneQF += dvMyDati.Item(x)("importo")
                        Case ClsDettaglioVoci.VOCE_FOGNATURAQF
                            FognaturaQF += dvMyDati.Item(x)("importo")
                        Case ClsDettaglioVoci.CAPITOLO_IVA
                            Iva += dvMyDati.Item(x)("importo")
                        Case ClsDettaglioVoci.CAPITOLO_ARROTONDAMENTO
                            Arrotondamento += dvMyDati.Item(x)("importo")
                    End Select
                Next
            End If
            'prelevo i dati dal db delle note da emettere
            dvMyDati = FncAnalisi.GetDettaglioDaEmettere(sMyEnte, Anno, Periodo, AccreditoDal, AccreditoAl)
            If Not dvMyDati Is Nothing Then
                For x = 0 To dvMyDati.Count - 1
                    Select Case CStr(dvMyDati.Item(x)("codice_capitolo"))
                        Case ClsDettaglioVoci.CAPITOLO_CONSUMO
                            Consumo += dvMyDati.Item(x)("importo")
                        Case ClsDettaglioVoci.CAPITOLO_CANONI
                            Select Case Utility.StringOperation.FormatString(dvMyDati.Item(x)("idvoce"))
                                Case ClsDettaglioVoci.VOCE_DEPURAZIONE
                                    Depurazione += dvMyDati.Item(x)("importo")
                                Case ClsDettaglioVoci.VOCE_FOGNATURA
                                    Fognatura += dvMyDati.Item(x)("importo")
                            End Select
                        Case ClsDettaglioVoci.CAPITOLO_ADDIZIONALI
                            Addizionali += dvMyDati.Item(x)("importo")
                        Case ClsDettaglioVoci.CAPITOLO_NOLO
                            Nolo += dvMyDati.Item(x)("importo")
                        Case ClsDettaglioVoci.CAPITOLO_QUOTAFISSA
                            QuotaFissa += dvMyDati.Item(x)("importo")
                        Case ClsDettaglioVoci.VOCE_DEPURAZIONEQF
                            DepurazioneQF += dvMyDati.Item(x)("importo")
                        Case ClsDettaglioVoci.VOCE_FOGNATURAQF
                            FognaturaQF += dvMyDati.Item(x)("importo")
                        Case ClsDettaglioVoci.CAPITOLO_IVA
                            Iva += dvMyDati.Item(x)("importo")
                        Case ClsDettaglioVoci.CAPITOLO_ARROTONDAMENTO
                            Arrotondamento += dvMyDati.Item(x)("importo")
                    End Select
                Next
            End If

            LblFatturatoConsumo.Text = FormatNumber(Consumo + Arrotondamento, 2)
            LblFatturatoFognatura.Text = FormatNumber(Fognatura, 2)
            LblFatturatoDepurazione.Text = FormatNumber(Depurazione, 2)
            LblFatturatoFognaturaQF.Text = FormatNumber(FognaturaQF, 2)
            LblFatturatoDepurazioneQF.Text = FormatNumber(DepurazioneQF, 2)
            LblFatturatoNolo.Text = FormatNumber(Nolo, 2)
            LblFatturatoQuotaFissa.Text = FormatNumber(QuotaFissa, 2)
            LblFatturatoAddizionali.Text = FormatNumber(Addizionali, 2)
            LblFatturatoIVA.Text = FormatNumber(Iva, 2)
            LblFatturatoTot.Text = FormatNumber(Consumo + Fognatura + Depurazione + Nolo + QuotaFissa + Addizionali + Iva + Arrotondamento + FognaturaQF + DepurazioneQF, 2)

            Return 1
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultAnalisiEconomiche.LoadDettaglioFatturato.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
            Return 0
        Finally
            If Not dvMyDati Is Nothing Then
                dvMyDati.Dispose()
            End If
        End Try
    End Function

    Private Function LoadDettaglioIncassato(ByVal sMyEnte As String, ByVal Anno As String, ByVal Periodo As String, ByVal AccreditoDal As String, ByVal AccreditoAl As String) As Integer
        Dim dvMyDati As New DataView
        Dim x As Integer
        Dim Consumo, Fognatura, Depurazione, Nolo, QuotaFissa, Addizionali, Iva, Arrotondamento, FognaturaQF, DepurazioneQF As Double

        Try
            'azzero le text
            LblIncassatoConsumo.Text = FormatNumber(0, 2) : LblIncassatoFognatura.Text = FormatNumber(0, 2) : LblIncassatoDepurazione.Text = FormatNumber(0, 2) : LblIncassatoDepurazioneQF.Text = FormatNumber(0, 2) : LblIncassatoFognaturaQF.Text = FormatNumber(0, 2)
            LblIncassatoNolo.Text = FormatNumber(0, 2) : LblIncassatoQuotaFissa.Text = FormatNumber(0, 2)
            LblIncassatoAddizionali.Text = FormatNumber(0, 2) : LblIncassatoIVA.Text = FormatNumber(0, 2)
            LblIncassatoTot.Text = FormatNumber(0, 2)
            'prelevo i dati dal db dell'Incassato
            dvMyDati = FncAnalisi.GetDettaglioIncassato(sMyEnte, Anno, Periodo, AccreditoDal, AccreditoAl)
            If Not dvMyDati Is Nothing Then
                For x = 0 To dvMyDati.Count - 1
                    Select Case CStr(dvMyDati.Item(x)("codice_capitolo"))
                        Case ClsDettaglioVoci.CAPITOLO_CONSUMO
                            Consumo += dvMyDati.Item(x)("importo")
                        Case ClsDettaglioVoci.CAPITOLO_CANONI
                            Select Case Utility.StringOperation.FormatString(dvMyDati.Item(x)("idvoce"))
                                Case ClsDettaglioVoci.VOCE_DEPURAZIONE
                                    Depurazione += dvMyDati.Item(x)("importo")
                                Case ClsDettaglioVoci.VOCE_FOGNATURA
                                    Fognatura += dvMyDati.Item(x)("importo")
                            End Select
                        Case ClsDettaglioVoci.CAPITOLO_ADDIZIONALI
                            Addizionali += dvMyDati.Item(x)("importo")
                        Case ClsDettaglioVoci.CAPITOLO_NOLO
                            Nolo += dvMyDati.Item(x)("importo")
                        Case ClsDettaglioVoci.CAPITOLO_QUOTAFISSA
                            QuotaFissa += dvMyDati.Item(x)("importo")
                        Case ClsDettaglioVoci.VOCE_DEPURAZIONEQF
                            DepurazioneQF += dvMyDati.Item(x)("importo")
                        Case ClsDettaglioVoci.VOCE_FOGNATURAQF
                            FognaturaQF += dvMyDati.Item(x)("importo")
                        Case ClsDettaglioVoci.CAPITOLO_IVA
                            Iva += dvMyDati.Item(x)("importo")
                        Case ClsDettaglioVoci.CAPITOLO_ARROTONDAMENTO
                            Arrotondamento += dvMyDati.Item(x)("importo")
                    End Select
                Next
            End If

            LblIncassatoConsumo.Text = FormatNumber(Consumo + Arrotondamento, 2)
            LblIncassatoFognatura.Text = FormatNumber(Fognatura, 2)
            LblIncassatoDepurazione.Text = FormatNumber(Depurazione, 2)
            LblIncassatoFognaturaQF.Text = FormatNumber(FognaturaQF, 2)
            LblIncassatoDepurazioneQF.Text = FormatNumber(DepurazioneQF, 2)
            LblIncassatoNolo.Text = FormatNumber(Nolo, 2)
            LblIncassatoQuotaFissa.Text = FormatNumber(QuotaFissa, 2)
            LblIncassatoAddizionali.Text = FormatNumber(Addizionali, 2)
            LblIncassatoIVA.Text = FormatNumber(Iva, 2)
            LblIncassatoTot.Text = FormatNumber(Consumo + Fognatura + Depurazione + Nolo + QuotaFissa + Addizionali + Iva + Arrotondamento + FognaturaQF + DepurazioneQF, 2)

            Return 1
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultAnalisiEconomiche.LoadDettaglioIncassato.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
            Return 0
        Finally
            If Not dvMyDati Is Nothing Then
                dvMyDati.Dispose()
            End If
        End Try
    End Function

    Private Sub CmdStampa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdStampa.Click
        Dim sPathProspetti, sNameXLS As String
        Dim x, nCol As Integer
        Dim FunctionStampa As New ClsStampaXLS
        Dim DtDatiStampa As New DataTable
        Dim aListColonne As ArrayList
        Dim aMyHeaders As String()
        nCol = 6
        'valorizzo il nome del file
        sPathProspetti = ConstSession.PathProspetti
        sNameXLS = ConstSession.IdEnte & "_ANALISIFATTURATOINCASSATO_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
        DtDatiStampa = FunctionStampa.PrintAnalisiFatturatoIncassatoH2O(ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, LblNFatture.Text, LblTotImpFatture.Text, LblNNoteCredito.Text, LblTotImpNoteCredito.Text, LblTotEmesso.Text, LblNPagTot.Text, LblImpPagTot.Text, LblNPagParz.Text, LblImpPagParz.Text, LblIncassato.Text, LblNUtenti.Text, LblInsoluto.Text, LblPercentualeInsoluto.Text, LblFatturatoConsumo.Text, LblIncassatoConsumo.Text, LblFatturatoFognatura.Text, LblIncassatoFognatura.Text, LblFatturatoDepurazione.Text, LblIncassatoDepurazione.Text, LblFatturatoNolo.Text, LblIncassatoNolo.Text, LblFatturatoQuotaFissa.Text, LblIncassatoQuotaFissa.Text, LblFatturatoAddizionali.Text, LblIncassatoAddizionali.Text, LblFatturatoIVA.Text, LblIncassatoIVA.Text, LblFatturatoTot.Text, LblIncassatoTot.Text, LblFatturatoFognaturaQF.Text, LblIncassatoFognaturaQF.Text, LblFatturatoDepurazioneQF.Text, LblIncassatoDepurazioneQF.Text)

        If Not DtDatiStampa Is Nothing Then
            'definisco le colonne
            aListColonne = New ArrayList
            For x = 0 To ncol
                aListColonne.Add("")
            Next
            aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

            'definisco l'insieme delle colonne da esportare
            Dim MyCol() As Integer = New Integer(nCol) {}
            For x = 0 To nCol
                MyCol(x) = x
            Next
            'esporto i dati in excel
            Dim MyStampa As New RKLib.ExportData.Export("Web")
            MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
        End If
    End Sub


    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '	'Put user code to initialize the page here
    '	Dim WFSessione As OPENUtility.CreateSessione
    '	Dim WFErrore As String

    '	Try
    '		If Not Page.IsPostBack Then
    '			Dim Anno As String = Request.Item("DdlAnno")
    '			Dim Periodo As String = Request.Item("DdlPeriodo")
    '			Dim DataAccreditoDal As String = Request.Item("AccreditoDal")
    '			Dim DataAccreditoAl As String = Request.Item("AccreditoAl")
    '               Dim sEnteRicerca As String = ConstSession.IdEnte

    '               '*** 20130204 - analisi economiche senza filtro per ente
    '               If CStr(Request.Item("TypeRicerca")).ToUpper = "NOENTE" Then
    '                   sEnteRicerca = ""
    '                   Label1.Text = "Dati Fatturato/Incassato per Comunità"
    '                   Label8.Text = "Riepilogo Generale per Comunità"
    '                   Label20.Text = "Dettaglio Fatturato per Comunità"
    '                   Label27.Text = "Dettaglio Incassato per Comunità"
    '               Else
    '                   Label1.Text = "Dati Fatturato/Incassato per Comune"
    '                   Label8.Text = "Riepilogo Generale per Comune"
    '                   Label20.Text = "Dettaglio Fatturato per Comune"
    '                   Label27.Text = "Dettaglio Incassato per Comune"
    '               End If
    '               '*** ***
    '               If DataAccreditoDal <> "" Then
    '                   DataAccreditoDal = CDate(DataAccreditoDal).ToString("yyyyMMdd")
    '               End If
    '               If DataAccreditoAl <> "" Then
    '                   DataAccreditoAl = CDate(DataAccreditoAl).ToString("yyyyMMdd")
    '               End If
    '               'inizializzo la connessione
    '               WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '               If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '                   Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '               End If

    '               If LoadRiepilogoFatturato(WFSessione, sEnteRicerca, Anno, Periodo, DataAccreditoDal, DataAccreditoAl) = 0 Then
    '                   Response.End()
    '               End If
    '               If LoadDettaglioFatturato(WFSessione, sEnteRicerca, Anno, Periodo, DataAccreditoDal, DataAccreditoAl) = 0 Then
    '                   Response.End()
    '               End If
    '               If LoadDettaglioIncassato(WFSessione, sEnteRicerca, Anno, Periodo, DataAccreditoDal, DataAccreditoAl) = 0 Then
    '                   Response.End()
    '               End If

    '               'Dim sScript As String = ""
    '               'sScript = "parent.parent.Comandi.Grafico.disabled=false"
    '               'RegisterScript(sScript , Me.GetType())
    '           End If
    '       Catch Err As Exception
    '          Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultAnalisiEconomiche.Page_Load.errore: ", Err)
    '       Response.Redirect("../../PaginaErrore.aspx")
    '       Finally
    '           If Not IsNothing(WFSessione) Then
    '               WFSessione.Kill()
    '               WFSessione = Nothing
    '           End If
    '       End Try
    '   End Sub

    '   Private Function LoadRiepilogoFatturato(ByVal WFSessione As OPENUtility.CreateSessione, ByVal sMyEnte As String, ByVal Anno As String, ByVal Periodo As String, ByVal AccreditoDal As String, ByVal AccreditoAl As String) As Integer
    '       Dim dvMyDati As DataView
    '       Dim x, nUtenti, nFatture, nNoteCredito As Integer
    '       Dim impFatture, impNoteCredito As Double

    '       Try
    '           'azzero le text
    '           nUtenti = nFatture = nNoteCredito = 0
    '           impFatture = 0 : impNoteCredito = 0
    '           LblNUtenti.Text = "0"
    '           LblNFatture.Text = "0" : LblTotImpFatture.Text = FormatNumber("0", 2)
    '           LblNNoteCredito.Text = "0" : LblTotImpNoteCredito.Text = FormatNumber("0", 2)
    '           LblNNoteDaEmettere.Text = "0" : LblTotImpNoteDaEmettere.Text = FormatNumber("0", 2)
    '           LblNPagTot.Text = "0" : LblImpPagTot.Text = FormatNumber("0", 2)
    '           LblNPagParz.Text = "0" : LblImpPagParz.Text = FormatNumber("0", 2)
    '           LblInsoluto.Text = FormatNumber("0", 2) : LblPercentualeInsoluto.Text = FormatNumber("0", 2)
    '           'prelevo i dati dal db dell'Fatturato
    '           dvMyDati = FncAnalisi.GetRiepilogoEmesso(sMyEnte, Anno, Periodo, AccreditoDal, AccreditoAl, WFSessione)
    '           If Not dvMyDati Is Nothing Then
    '               For x = 0 To dvMyDati.Count - 1
    '                   Select Case CStr(dvMyDati.Item(x)("tipo_documento"))
    '                       Case "F"
    '                           If Not IsDBNull(dvMyDati.Item(x)("nutenti")) Then
    '                               nUtenti += FormatNumber(dvMyDati.Item(x)("nutenti"), 0)
    '                           End If
    '                           If Not IsDBNull(dvMyDati.Item(x)("nfatture")) Then
    '                               nFatture += FormatNumber(dvMyDati.Item(x)("nfatture"), 0)
    '                           End If
    '                           If Not IsDBNull(dvMyDati.Item(x)("impfatture")) Then
    '                               impFatture += FormatNumber(dvMyDati.Item(x)("impfatture"), 2)
    '                           End If
    '                       Case "N"
    '                           If Not IsDBNull(dvMyDati.Item(x)("nfatture")) Then
    '                               nNoteCredito += FormatNumber(dvMyDati.Item(x)("nfatture"), 0)
    '                           End If
    '                           If Not IsDBNull(dvMyDati.Item(x)("impfatture")) Then
    '                               impNoteCredito += FormatNumber(dvMyDati.Item(x)("impfatture"), 2)
    '                           End If
    '                   End Select
    '               Next
    '               LblNUtenti.Text = FormatNumber(nUtenti, 0)
    '               LblNFatture.Text = FormatNumber(nFatture, 0)
    '               LblTotImpFatture.Text = FormatNumber(impFatture, 2)
    '               LblNNoteCredito.Text = FormatNumber(nNoteCredito, 0)
    '               LblTotImpNoteCredito.Text = FormatNumber(impNoteCredito, 2)
    '           End If
    '           'prelevo i dati dal db delle note da emettere
    '           nNoteCredito = 0
    '           impNoteCredito = 0
    '           dvMyDati = FncAnalisi.GetRiepilogoDaEmettere(sMyEnte, Anno, Periodo, AccreditoDal, AccreditoAl, WFSessione)
    '           If Not dvMyDati Is Nothing Then
    '               For x = 0 To dvMyDati.Count - 1
    '                   If Not IsDBNull(dvMyDati.Item(x)("ndoc")) Then
    '                       nNoteCredito += FormatNumber(dvMyDati.Item(x)("ndoc"), 0)
    '                   End If
    '                   If Not IsDBNull(dvMyDati.Item(x)("impdoc")) Then
    '                       impNoteCredito += FormatNumber(dvMyDati.Item(x)("impdoc"), 2)
    '                   End If
    '               Next
    '               LblNNoteDaEmettere.Text = FormatNumber(nNoteCredito, 0)
    '               LblTotImpNoteDaEmettere.Text = FormatNumber(impNoteCredito, 2)
    '           End If
    '           'prelevo i dati dal db delle fatture evase totalmente
    '           dvMyDati = FncAnalisi.GetRiepilogoEmessoEvaso(sMyEnte, Anno, Periodo, AccreditoDal, AccreditoAl, 1, WFSessione)
    '           If Not dvMyDati Is Nothing Then
    '               For x = 0 To dvMyDati.Count - 1
    '                   LblNPagTot.Text = FormatNumber(dvMyDati.Item(x)("npag"), 0)
    '                   If Not IsDBNull(dvMyDati.Item(x)("imppag")) Then
    '                       If sMyEnte = "" Then
    '                           LblImpPagTot.Text = FormatNumber(dvMyDati.Item(x)("imppag"), 2)
    '                       Else
    '                           LblImpPagTot.Text = FormatNumber(dvMyDati.Item(x)("imppag"), 2)
    '                       End If
    '                   End If
    '               Next
    '           End If
    '           'prelevo i dati dal db delle fatture evase parzialmente
    '           dvMyDati = FncAnalisi.GetRiepilogoEmessoEvaso(sMyEnte, Anno, Periodo, AccreditoDal, AccreditoAl, 0, WFSessione)
    '           If Not dvMyDati Is Nothing Then
    '               For x = 0 To dvMyDati.Count - 1
    '                   LblNPagParz.Text = FormatNumber(dvMyDati.Item(x)("npag"), 0)
    '                   If Not IsDBNull(dvMyDati.Item(x)("imppag")) Then
    '                       LblImpPagParz.Text = FormatNumber(dvMyDati.Item(x)("imppag"), 2)
    '                   End If
    '               Next
    '           End If
    '           LblTotIncassato.Text = FormatNumber((CDbl(LblImpPagParz.Text) + CDbl(LblImpPagTot.Text)), 2)
    '           LblTotEmesso.Text = FormatNumber(CDbl(LblTotImpFatture.Text) + CDbl(LblTotImpNoteCredito.Text) + CDbl(LblTotImpNoteDaEmettere.Text), 2)
    '           LblEmesso.Text = LblTotEmesso.Text : LblIncassato.Text = LblTotIncassato.Text
    '           LblInsoluto.Text = FormatNumber(CDbl(LblEmesso.Text) - (CDbl(LblIncassato.Text)), 2)
    '           If CDbl(LblEmesso.Text) <> 0 Then
    '               LblPercentualeInsoluto.Text = FormatNumber(((CDbl(LblInsoluto.Text) * 100) / CDbl(LblEmesso.Text)), 2)
    '           Else
    '               LblPercentualeInsoluto.Text = "100,00"
    '           End If
    '           TxtIncassato.Text = LblTotIncassato.Text : TxtInsoluto.Text = LblInsoluto.Text
    '           Return 1
    '       Catch err As Exception
    '          Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultAnalisiEconomiche.LoadRiepilogoFatturato.errore: ", Err)
    '         Response.Redirect("../../PaginaErrore.aspx")
    '           Return 0
    '       Finally
    '           'dvMyDati.Dispose()
    '       End Try
    '   End Function

    '   Private Function LoadDettaglioFatturato(ByVal WFSessione As OPENUtility.CreateSessione, ByVal sMyEnte As String, ByVal Anno As String, ByVal Periodo As String, ByVal AccreditoDal As String, ByVal AccreditoAl As String) As Integer
    '       Dim dvMyDati As DataView
    '       Dim x As Integer
    '       Dim Consumo, Fognatura, Depurazione, Nolo, QuotaFissa, Addizionali, Iva, Arrotondamento, FognaturaQF, DepurazioneQF As Double

    '       Try
    '           'azzero le text
    '           LblFatturatoConsumo.Text = FormatNumber(0, 2) : LblFatturatoFognatura.Text = FormatNumber(0, 2) : LblFatturatoDepurazione.Text = FormatNumber(0, 2) : LblFatturatoDepurazioneQF.Text = FormatNumber(0, 2) : LblFatturatoFognaturaQF.Text = FormatNumber(0, 2)
    '           LblFatturatoNolo.Text = FormatNumber(0, 2) : LblFatturatoQuotaFissa.Text = FormatNumber(0, 2)
    '           LblFatturatoAddizionali.Text = FormatNumber(0, 2) : LblFatturatoIVA.Text = FormatNumber(0, 2)
    '           LblFatturatoTot.Text = FormatNumber(0, 2)
    '           'prelevo i dati dal db dell'Fatturato
    '           dvMyDati = FncAnalisi.GetDettaglioEmesso(sMyEnte, Anno, Periodo, AccreditoDal, AccreditoAl, WFSessione)
    '           If Not dvMyDati Is Nothing Then
    '               For x = 0 To dvMyDati.Count - 1
    '                   Select Case CStr(dvMyDati.Item(x)("codice_capitolo"))
    '                       Case FncDettaglio.CAPITOLO_CONSUMO
    '                           Consumo += dvMyDati.Item(x)("importo")
    '                       Case FncDettaglio.CAPITOLO_CANONI
    '                           Select Case dvMyDati.Item(x)("idvoce")
    '                               Case FncDettaglio.VOCE_DEPURAZIONE
    '                                   Depurazione += dvMyDati.Item(x)("importo")
    '                               Case FncDettaglio.VOCE_FOGNATURA
    '                                   Fognatura += dvMyDati.Item(x)("importo")
    '                           End Select
    '                       Case FncDettaglio.CAPITOLO_ADDIZIONALI
    '                           Addizionali += dvMyDati.Item(x)("importo")
    '                       Case FncDettaglio.CAPITOLO_NOLO
    '                           Nolo += dvMyDati.Item(x)("importo")
    '                       Case FncDettaglio.CAPITOLO_QUOTAFISSA
    '                           QuotaFissa += dvMyDati.Item(x)("importo")
    '                       Case FncDettaglio.VOCE_DEPURAZIONEQF
    '                           DepurazioneQF += dvMyDati.Item(x)("importo")
    '                       Case FncDettaglio.VOCE_FOGNATURAQF
    '                           FognaturaQF += dvMyDati.Item(x)("importo")
    '                       Case FncDettaglio.CAPITOLO_IVA
    '                           Iva += dvMyDati.Item(x)("importo")
    '                       Case FncDettaglio.CAPITOLO_ARROTONDAMENTO
    '                           Arrotondamento += dvMyDati.Item(x)("importo")
    '                   End Select
    '               Next
    '           End If
    '           'prelevo i dati dal db delle note da emettere
    '           dvMyDati = FncAnalisi.GetDettaglioDaEmettere(sMyEnte, Anno, Periodo, AccreditoDal, AccreditoAl, WFSessione)
    '           If Not dvMyDati Is Nothing Then
    '               For x = 0 To dvMyDati.Count - 1
    '                   Select Case CStr(dvMyDati.Item(x)("codice_capitolo"))
    '                       Case FncDettaglio.CAPITOLO_CONSUMO
    '                           Consumo += dvMyDati.Item(x)("importo")
    '                       Case FncDettaglio.CAPITOLO_CANONI
    '                           Select Case dvMyDati.Item(x)("idvoce")
    '                               Case FncDettaglio.VOCE_DEPURAZIONE
    '                                   Depurazione += dvMyDati.Item(x)("importo")
    '                               Case FncDettaglio.VOCE_FOGNATURA
    '                                   Fognatura += dvMyDati.Item(x)("importo")
    '                           End Select
    '                       Case FncDettaglio.CAPITOLO_ADDIZIONALI
    '                           Addizionali += dvMyDati.Item(x)("importo")
    '                       Case FncDettaglio.CAPITOLO_NOLO
    '                           Nolo += dvMyDati.Item(x)("importo")
    '                       Case FncDettaglio.CAPITOLO_QUOTAFISSA
    '                           QuotaFissa += dvMyDati.Item(x)("importo")
    '                       Case FncDettaglio.VOCE_DEPURAZIONEQF
    '                           DepurazioneQF += dvMyDati.Item(x)("importo")
    '                       Case FncDettaglio.VOCE_FOGNATURAQF
    '                           FognaturaQF += dvMyDati.Item(x)("importo")
    '                       Case FncDettaglio.CAPITOLO_IVA
    '                           Iva += dvMyDati.Item(x)("importo")
    '                       Case FncDettaglio.CAPITOLO_ARROTONDAMENTO
    '                           Arrotondamento += dvMyDati.Item(x)("importo")
    '                   End Select
    '               Next
    '           End If

    '           LblFatturatoConsumo.Text = FormatNumber(Consumo + Arrotondamento, 2)
    '           LblFatturatoFognatura.Text = FormatNumber(Fognatura, 2)
    '           LblFatturatoDepurazione.Text = FormatNumber(Depurazione, 2)
    '           LblFatturatoFognaturaQF.Text = FormatNumber(FognaturaQF, 2)
    '           LblFatturatoDepurazioneQF.Text = FormatNumber(DepurazioneQF, 2)
    '           LblFatturatoNolo.Text = FormatNumber(Nolo, 2)
    '           LblFatturatoQuotaFissa.Text = FormatNumber(QuotaFissa, 2)
    '           LblFatturatoAddizionali.Text = FormatNumber(Addizionali, 2)
    '           LblFatturatoIVA.Text = FormatNumber(Iva, 2)
    '           LblFatturatoTot.Text = FormatNumber(Consumo + Fognatura + Depurazione + Nolo + QuotaFissa + Addizionali + Iva + Arrotondamento + FognaturaQF + DepurazioneQF, 2)

    '           Return 1
    '       Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultAnalisiEconomiche.LoadDettaglioFatturato.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '           Return 0
    '       Finally
    '           If Not dvMyDati Is Nothing Then
    '               dvMyDati.Dispose()
    '           End If
    '       End Try
    '   End Function

    '   Private Function LoadDettaglioIncassato(ByVal WFSessione As OPENUtility.CreateSessione, ByVal sMyEnte As String, ByVal Anno As String, ByVal Periodo As String, ByVal AccreditoDal As String, ByVal AccreditoAl As String) As Integer
    '       Dim dvMyDati As DataView
    '       Dim x As Integer
    '       Dim Consumo, Fognatura, Depurazione, Nolo, QuotaFissa, Addizionali, Iva, Arrotondamento, FognaturaQF, DepurazioneQF As Double

    '       Try
    '           'azzero le text
    '           LblIncassatoConsumo.Text = FormatNumber(0, 2) : LblIncassatoFognatura.Text = FormatNumber(0, 2) : LblIncassatoDepurazione.Text = FormatNumber(0, 2) : LblIncassatoDepurazioneQF.Text = FormatNumber(0, 2) : LblIncassatoFognaturaQF.Text = FormatNumber(0, 2)
    '           LblIncassatoNolo.Text = FormatNumber(0, 2) : LblIncassatoQuotaFissa.Text = FormatNumber(0, 2)
    '           LblIncassatoAddizionali.Text = FormatNumber(0, 2) : LblIncassatoIVA.Text = FormatNumber(0, 2)
    '           LblIncassatoTot.Text = FormatNumber(0, 2)
    '           'prelevo i dati dal db dell'Incassato
    '           dvMyDati = FncAnalisi.GetDettaglioIncassato(sMyEnte, Anno, Periodo, AccreditoDal, AccreditoAl, WFSessione)
    '           If Not dvMyDati Is Nothing Then
    '               For x = 0 To dvMyDati.Count - 1
    '                   Select Case CStr(dvMyDati.Item(x)("codice_capitolo"))
    '                       Case FncDettaglio.CAPITOLO_CONSUMO
    '                           Consumo += dvMyDati.Item(x)("importo")
    '                       Case FncDettaglio.CAPITOLO_CANONI
    '                           Select Case dvMyDati.Item(x)("idvoce")
    '                               Case FncDettaglio.VOCE_DEPURAZIONE
    '                                   Depurazione += dvMyDati.Item(x)("importo")
    '                               Case FncDettaglio.VOCE_FOGNATURA
    '                                   Fognatura += dvMyDati.Item(x)("importo")
    '                           End Select
    '                       Case FncDettaglio.CAPITOLO_ADDIZIONALI
    '                           Addizionali += dvMyDati.Item(x)("importo")
    '                       Case FncDettaglio.CAPITOLO_NOLO
    '                           Nolo += dvMyDati.Item(x)("importo")
    '                       Case FncDettaglio.CAPITOLO_QUOTAFISSA
    '                           QuotaFissa += dvMyDati.Item(x)("importo")
    '                       Case FncDettaglio.VOCE_DEPURAZIONEQF
    '                           DepurazioneQF += dvMyDati.Item(x)("importo")
    '                       Case FncDettaglio.VOCE_FOGNATURAQF
    '                           FognaturaQF += dvMyDati.Item(x)("importo")
    '                       Case FncDettaglio.CAPITOLO_IVA
    '                           Iva += dvMyDati.Item(x)("importo")
    '                       Case FncDettaglio.CAPITOLO_ARROTONDAMENTO
    '                           Arrotondamento += dvMyDati.Item(x)("importo")
    '                   End Select
    '               Next
    '           End If

    '           LblIncassatoConsumo.Text = FormatNumber(Consumo + Arrotondamento, 2)
    '           LblIncassatoFognatura.Text = FormatNumber(Fognatura, 2)
    '           LblIncassatoDepurazione.Text = FormatNumber(Depurazione, 2)
    '           LblIncassatoFognaturaQF.Text = FormatNumber(FognaturaQF, 2)
    '           LblIncassatoDepurazioneQF.Text = FormatNumber(DepurazioneQF, 2)
    '           LblIncassatoNolo.Text = FormatNumber(Nolo, 2)
    '           LblIncassatoQuotaFissa.Text = FormatNumber(QuotaFissa, 2)
    '           LblIncassatoAddizionali.Text = FormatNumber(Addizionali, 2)
    '           LblIncassatoIVA.Text = FormatNumber(Iva, 2)
    '           LblIncassatoTot.Text = FormatNumber(Consumo + Fognatura + Depurazione + Nolo + QuotaFissa + Addizionali + Iva + Arrotondamento + FognaturaQF + DepurazioneQF, 2)

    '           Return 1
    '       Catch Err As Exception
    '      Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultAnalisiEconomiche.LoadDettaglioIncassato.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '           Return 0
    '       Finally
    '           If Not dvMyDati Is Nothing Then
    '               dvMyDati.Dispose()
    '           End If
    '       End Try
    '   End Function

    '   Private Sub CmdStampa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdStampa.Click
    '       Dim WFSessione As OPENUtility.CreateSessione
    '       Dim WFErrore As String
    '       Dim sPathProspetti, sNameFile, Str As String
    '       Dim sScript As String
    '       Dim x As Integer
    '       Dim FunctionStampa As New ClsStampaXLS
    '       Dim DtDatiStampa As New DataTable
    '       Dim aListColonne As ArrayList
    '       Dim aMyHeaders As String()

    '       'valorizzo il nome del file
    '       sPathProspetti = ConstSession.PathProspetti
    '       sNameFile = ConstSession.DescrizioneEnte & "_" & ConstSession.IdEnte & "_ANALISIFATTURATOINCASSATO_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
    '       DtDatiStampa = FunctionStampa.PrintAnalisiFatturatoIncassatoH2O(ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, LblNFatture.Text, LblTotImpFatture.Text, LblNNoteCredito.Text, LblTotImpNoteCredito.Text, LblTotEmesso.Text, LblNPagTot.Text, LblImpPagTot.Text, LblNPagParz.Text, LblImpPagParz.Text, LblIncassato.Text, LblNUtenti.Text, LblInsoluto.Text, LblPercentualeInsoluto.Text, LblFatturatoConsumo.Text, LblIncassatoConsumo.Text, LblFatturatoFognatura.Text, LblIncassatoFognatura.Text, LblFatturatoDepurazione.Text, LblIncassatoDepurazione.Text, LblFatturatoNolo.Text, LblIncassatoNolo.Text, LblFatturatoQuotaFissa.Text, LblIncassatoQuotaFissa.Text, LblFatturatoAddizionali.Text, LblIncassatoAddizionali.Text, LblFatturatoIVA.Text, LblIncassatoIVA.Text, LblFatturatoTot.Text, LblIncassatoTot.Text, LblFatturatoFognaturaQF.Text, LblIncassatoFognaturaQF.Text, LblFatturatoDepurazioneQF.Text, LblIncassatoDepurazioneQF.Text)
    'Try
    '       If Not DtDatiStampa Is Nothing Then
    '           'definisco le colonne
    '           aListColonne = New ArrayList
    '           For x = 0 To 6
    '               aListColonne.Add("")
    '           Next
    '           aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

    '           'definisco l'insieme delle colonne da esportare
    '           Dim MyCol() As Integer = New Integer() {0, 1, 2, 3, 4, 5, 6}
    '           'esporto i dati in excel
    '           Dim MyStampa As New RKLib.ExportData.Export("Web")
    '           MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameFile)
    '           Str = sPathProspetti & sNameFile
    '       End If
    '       Catch Err As Exception
    '      Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultAnalisiEconomiche.CmdStampa_Click.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '  End Try
    '   End Sub
End Class
