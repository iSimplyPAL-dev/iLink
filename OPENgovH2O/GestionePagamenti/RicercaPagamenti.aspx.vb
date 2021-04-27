Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.HttpContext
Imports System.Globalization
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports log4net

Partial Class RicercaPagamenti
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(RicercaPagamenti))
    Private myPagamentiForSearch As New OggettoPagamento
    Private oReplace As New ClsGenerale.Generale

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents StampaPagamenti As System.Web.UI.WebControls.Button
    Protected WithEvents StampaRiversamento As System.Web.UI.WebControls.Button
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
        Dim sSQL As String
        Dim oLoadCombo As New ClsGenerale.Generale
        Dim sScript As String

        Try
            If Page.IsPostBack = False Then
                Dim FncGen As New ClsGenerale.Generale
                FncGen.GetPeriodoAttuale
                'carico i periodi
                sSQL = "SELECT PERIODO, CODPERIODO"
                sSQL += " FROM TP_PERIODO "
                sSQL += " WHERE (COD_ENTE='" & ConstSession.IdEnte & "')"
                sSQL += " ORDER BY PERIODO"
                oLoadCombo.LoadComboGenerale(DdlPeriodo, sSQL)
            End If
            'str = "Search();"
            'RegisterScript(sScript , Me.GetType())

            sScript = "parent.parent.Comandi.location.href='./CRicercaPagamenti.aspx';"
            RegisterScript(sScript, Me.GetType())
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RicercaPagamenti.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Function CreaDatasetStampaPagMin() As DataSet
        Dim dsTmp As New DataSet

        dsTmp.Tables.Add("STAMPA_RIMBORSI")
        dsTmp.Tables("STAMPA_RIMBORSI").Columns.Add("").DataType = String.Empty.GetType()
        dsTmp.Tables("STAMPA_RIMBORSI").Columns.Add("").DataType = String.Empty.GetType()
        dsTmp.Tables("STAMPA_RIMBORSI").Columns.Add("").DataType = String.Empty.GetType()
        dsTmp.Tables("STAMPA_RIMBORSI").Columns.Add("").DataType = String.Empty.GetType()
        dsTmp.Tables("STAMPA_RIMBORSI").Columns.Add("").DataType = String.Empty.GetType()
        dsTmp.Tables("STAMPA_RIMBORSI").Columns.Add("").DataType = String.Empty.GetType()
        dsTmp.Tables("STAMPA_RIMBORSI").Columns.Add("").DataType = String.Empty.GetType()
        dsTmp.Tables("STAMPA_RIMBORSI").Columns.Add("").DataType = String.Empty.GetType()

        Return dsTmp
    End Function

#Region "Rimborsi"
    Private Sub StampaExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdStampaExcel.Click
        Dim NameXLS As String
        Dim x As Integer
        Dim FncStampa As New ClsStampaXLS
        Dim FncPagamenti As New ClsPagamenti
        Dim DtDatiStampa As New DataTable
        Dim dvStampa As DataView
        Dim aListColonne As ArrayList
        Dim aMyHeaders As String()
        'valorizzo il nome del file
        NameXLS = ConstSession.IdEnte & "_RIMBORSI_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
        'prelevo i dati da stampare
        MyPagamentiForSearch.IDEnte = ConstSession.IdEnte
        MyPagamentiForSearch.sCognome = txtCognome.Text.Trim
        MyPagamentiForSearch.sNome = txtNome.Text.Trim
        If txtPIVA.Text.Trim <> "" Then
            MyPagamentiForSearch.sCodFiscalePIva = txtPIVA.Text.Trim
        Else
            MyPagamentiForSearch.sCodFiscalePIva = txtCF.Text.Trim
        End If
        MyPagamentiForSearch.sNumeroFattura = txtNFattura.Text.Trim
        If txtDataFattura.Text.Trim <> "" Then
            myPagamentiForSearch.sDataFattura = oReplace.FormattaData("A", "/", txtDataFattura.Text.Trim, False)
        Else
            myPagamentiForSearch.sDataFattura = ""
        End If
        If txtDataAccreditoDal.Text.Trim <> "" Then
            myPagamentiForSearch.sDataAccredito = oReplace.FormattaData("A", "/", txtDataAccreditoDal.Text.Trim, False)
        Else
            myPagamentiForSearch.sDataAccredito = ""
        End If
        If txtDataAccreditoAl.Text.Trim <> "" Then
            myPagamentiForSearch.sDataAccreditoAl = oReplace.FormattaData("A", "/", txtDataAccreditoAl.Text.Trim, False)
        Else
            myPagamentiForSearch.sDataAccreditoAl = ""
        End If
        MyPagamentiForSearch.sAnnoEmissioneFattura = txtAnno.Text
        If DdlPeriodo.SelectedValue <> "" Then
            MyPagamentiForSearch.nPeriodo = DdlPeriodo.SelectedValue
        End If
        'dvStampa = FncPagamenti.GetRimborsi(ConstSession.IdEnte, MyPagamentiForSearch)
        dvStampa = FncPagamenti.GetStampaPagamenti(ClsPagamenti.TypeStampa.Rimborsi, myPagamentiForSearch)
        DtDatiStampa = FncStampa.PrintRimborsiH2O(dvStampa, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte)
        If Not DtDatiStampa Is Nothing Then
            Dim nCol As Integer = 8
            'definisco le colonne
            aListColonne = New ArrayList
            For x = 0 To nCol
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
            MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, NameXLS)
        End If
        'Dim datatablevuoto As DataTable

        'Dim WFErrore As String = ""
        'Dim sql As String = ""
        'Dim strscript As String

        'Dim culture As IFormatProvider
        'culture = New CultureInfo("it-IT", True)
        'System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")

        'Dim WFSessioneEnte As New RIBESFrameWork.DBManager
        'Dim WFSessione As CreateSessione

        'Dim ente As String = ConstSession.IdEnte
        'Dim operatore = Session("username")
        'Dim descEnte = ConstSession.DescrizioneEnte
        'Dim DEContRATTIPR As New GestContratti

        'descEnte = DEContRATTIPR.GetEnte(CInt(ConstSession.CodIstat))

        'Dim nome As String = ""
        'Dim cognome As String = ""
        'Dim numeroUtente As Long = 0
        'Dim codFiscPIVA As String = ""
        'Dim importoPagato As Double = 0
        'Dim importoPagatoStampa As String = ""
        'Dim importoEmesso As Double = 0
        'Dim importoEmessoStampa As String = ""
        'Dim importoDifferenza As Double = 0
        'Dim importoDifferenzaStampa As String = ""
        'Dim Recapito As String = ""
        'Dim ClsPagamenti As New ClsPagamenti
        'Dim ClsGenerale As New ClsGenerale.Generale

        'Dim dr As DataRow
        'Dim ds As DataSet
        'Dim dsContribNonP As New DataSet
        'Dim dtNonPag As DataTable
        'Dim arratlistNomiColonne As ArrayList
        'Dim arraystr As String()
        'Dim sPathProspetti, NameXLS, Str As String
        'arratlistNomiColonne = New ArrayList

        'arratlistNomiColonne.Add("")
        'arratlistNomiColonne.Add("")
        'arratlistNomiColonne.Add("")
        'arratlistNomiColonne.Add("")
        'arratlistNomiColonne.Add("")
        'arratlistNomiColonne.Add("")
        'arratlistNomiColonne.Add("")
        'arratlistNomiColonne.Add("")

        'arraystr = CType(arratlistNomiColonne.ToArray(GetType(String)), String())

        'sPathProspetti = ConstSession.PathProspetti
        'NameXLS = descEnte & "_STAMPA_RIMBORSI_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"

        'ds = CreaDatasetStampaPagMin()
        'dtNonPag = ds.Tables("STAMPA_RIMBORSI")
        'dr = dtNonPag.NewRow()
        'dr(0) = "Comune di " & descEnte
        'dr(3) = "Data Stampa " & DateTime.Now.Date
        'dtNonPag.Rows.Add(dr)

        'dr = dtNonPag.NewRow()
        'dtNonPag.Rows.Add(dr)

        'dr = dtNonPag.NewRow()
        'dr(0) = "Periodo " & ConstSession.IdPeriodo
        'dtNonPag.Rows.Add(dr)

        'dr = dtNonPag.NewRow()
        'dtNonPag.Rows.Add(dr)

        'dr = dtNonPag.NewRow()
        'dr(0) = "Prospetto Rimborsi"
        'dtNonPag.Rows.Add(dr)

        'dr = dtNonPag.NewRow()
        'dtNonPag.Rows.Add(dr)

        'dr = dtNonPag.NewRow()
        'dr(0) = "Numero Utente"
        'dr(1) = "Cognome Utente"
        'dr(2) = "Nome Utente"
        'dr(3) = "Codice Fiscale/Partita IVA"
        'dr(4) = "Recapito Utente"
        'dr(5) = "Importo Emesso"
        'dr(6) = "Importo Pagato"
        'dr(7) = "Differenza"
        'dtNonPag.Rows.Add(dr)

        'oPagamentiForSearch.IDEnte = ConstSession.IdEnte
        'oPagamentiForSearch.sCognome = txtCognome.Text.Trim
        'oPagamentiForSearch.sNome = txtNome.Text.Trim
        'If txtPIVA.Text.Trim <> "" Then
        '    oPagamentiForSearch.sCodFiscalePIva = txtPIVA.Text.Trim
        'Else
        '    oPagamentiForSearch.sCodFiscalePIva = txtCF.Text.Trim
        'End If
        'oPagamentiForSearch.sNumeroFattura = txtNFattura.Text.Trim
        'If txtDataFattura.Text.Trim <> "" Then
        '    oPagamentiForSearch.sDataFattura = oReplace.FormattaData("A", "/", txtDataFattura.Text.Trim, False)
        'End If
        'If txtDataAccreditoDal.Text.Trim <> "" Then
        '    oPagamentiForSearch.sDataAccredito = oReplace.FormattaData("A", "/", txtDataAccreditoDal.Text.Trim, False)
        'End If
        'If txtDataAccreditoAl.Text.Trim <> "" Then
        '    oPagamentiForSearch.sDataAccreditoAl = oReplace.FormattaData("A", "/", txtDataAccreditoAl.Text.Trim, False)
        'End If
        'dsContribNonP = ClsPagamenti.GetRimborsi(oPagamentiForSearch)
        'If dsContribNonP.Tables(0).Rows.Count > 0 Then
        '    Dim i As Integer = 0
        '    For i = 0 To dsContribNonP.Tables(0).Rows.Count - 1
        '        numeroUtente = dsContribNonP.Tables(0).Rows(i)("NUMEROUTENTE")
        '        If Not IsDBNull(dsContribNonP.Tables(0).Rows(i)("COGNOME_DENOMINAZIONE")) Then
        '            If dsContribNonP.Tables(0).Rows(i)("COGNOME_DENOMINAZIONE").ToString() <> "" Then
        '                cognome = dsContribNonP.Tables(0).Rows(i)("COGNOME_DENOMINAZIONE").ToString()
        '                cognome = cognome.Replace("""", "")
        '            Else
        '                cognome = ""
        '            End If
        '        Else
        '            cognome = ""
        '        End If

        '        If dsContribNonP.Tables(0).Rows(i)("NOME").ToString() <> "" Then
        '            nome = dsContribNonP.Tables(0).Rows(i)("NOME").ToString()
        '            nome = nome.Replace("""", "")
        '        Else
        '            nome = ""
        '        End If

        '        If (dsContribNonP.Tables(0).Rows(i)("COD_FISCALE").ToString() <> "") Then
        '            codFiscPIVA = dsContribNonP.Tables(0).Rows(i)("COD_FISCALE").ToString()
        '        Else
        '            If (dsContribNonP.Tables(0).Rows(i)("PARTITA_IVA").ToString() <> "") Then
        '                codFiscPIVA = "  " & dsContribNonP.Tables(0).Rows(i)("PARTITA_IVA").ToString()
        '            Else
        '                codFiscPIVA = ""
        '            End If
        '        End If

        '        If Not IsDBNull(dsContribNonP.Tables(0).Rows(i)("IMPORTO_FATTURANOTA")) Then
        '            importoEmesso = New ClsGenerale.FunctionGrd().euroforgridview(dsContribNonP.Tables(0).Rows(i)("IMPORTO_FATTURANOTA").ToString())
        '        Else
        '            importoEmesso = New ClsGenerale.FunctionGrd().euroforgridview(importoEmesso)
        '        End If
        '        importoEmessoStampa = New ClsGenerale.FunctionGrd().euroforgridview(importoEmesso)
        '        If Not IsDBNull(dsContribNonP.Tables(0).Rows(i)("IMPORTO_PAGATO")) Then
        '            importoPagato = New ClsGenerale.FunctionGrd().euroforgridview(dsContribNonP.Tables(0).Rows(i)("IMPORTO_PAGATO").ToString())
        '        Else
        '            importoPagato = New ClsGenerale.FunctionGrd().euroforgridview(importoPagato)
        '        End If
        '        importoPagatoStampa = New ClsGenerale.FunctionGrd().euroforgridview(importoPagato)
        '        importoDifferenza = importoPagato - importoEmesso
        '        importoDifferenza = New ClsGenerale.FunctionGrd().euroforgridview(importoDifferenza)
        '        importoDifferenzaStampa = New ClsGenerale.FunctionGrd().euroforgridview(importoDifferenza)
        '        Recapito = ""
        '        If Not IsDBNull(dsContribNonP.Tables(0).Rows(i)("VIA_RCP")) Then
        '            Recapito += " " & dsContribNonP.Tables(0).Rows(i)("VIA_RCP")
        '            If Not IsDBNull(dsContribNonP.Tables(0).Rows(i)("CIVICO_RCP")) Then
        '                Recapito += " " & dsContribNonP.Tables(0).Rows(i)("CIVICO_RCP")
        '            End If
        '            If Not IsDBNull(dsContribNonP.Tables(0).Rows(i)("INTERNO_CIVICO_RCP")) Then
        '                Recapito += " " & dsContribNonP.Tables(0).Rows(i)("INTERNO_CIVICO_RCP")
        '            End If
        '            If Not IsDBNull(dsContribNonP.Tables(0).Rows(i)("COMUNE_RCP")) Then
        '                Recapito += " " & dsContribNonP.Tables(0).Rows(i)("COMUNE_RCP")
        '            End If
        '            If Not IsDBNull(dsContribNonP.Tables(0).Rows(i)("PROVINCIA_RCP")) Then
        '                Recapito += " " & dsContribNonP.Tables(0).Rows(i)("PROVINCIA_RCP")
        '            End If
        '            If Not IsDBNull(dsContribNonP.Tables(0).Rows(i)("CAP_RCP")) Then
        '                Recapito += " (" & dsContribNonP.Tables(0).Rows(i)("CAP_RCP") & ")"
        '            End If
        '        Else
        '            Recapito = ""
        '            If Not IsDBNull(dsContribNonP.Tables(0).Rows(i)("VIA_RES")) Then
        '                Recapito += dsContribNonP.Tables(0).Rows(i)("VIA_RES")
        '            End If
        '            If Not IsDBNull(dsContribNonP.Tables(0).Rows(i)("CIVICO_RES")) Then
        '                Recapito += " " & dsContribNonP.Tables(0).Rows(i)("CIVICO_RES")
        '            End If
        '            If Not IsDBNull(dsContribNonP.Tables(0).Rows(i)("INTERNO_CIVICO_RES")) Then
        '                Recapito += " " & dsContribNonP.Tables(0).Rows(i)("INTERNO_CIVICO_RES")
        '            End If
        '            If Not IsDBNull(dsContribNonP.Tables(0).Rows(i)("COMUNE_RES")) Then
        '                Recapito += " " & dsContribNonP.Tables(0).Rows(i)("COMUNE_RES")
        '            End If
        '            If Not IsDBNull(dsContribNonP.Tables(0).Rows(i)("PROVINCIA_RES")) Then
        '                Recapito += " " & dsContribNonP.Tables(0).Rows(i)("PROVINCIA_RES")
        '            End If
        '            If Not IsDBNull(dsContribNonP.Tables(0).Rows(i)("CAP_RES")) Then
        '                Recapito += " (" & dsContribNonP.Tables(0).Rows(i)("CAP_RES") & ")"
        '            End If
        '        End If

        '        dr = dtNonPag.NewRow()
        '        dr(0) = numeroUtente
        '        dr(1) = cognome
        '        dr(2) = nome
        '        dr(3) = codFiscPIVA
        '        dr(4) = Recapito
        '        dr(5) = importoEmesso
        '        dr(6) = importoPagato
        '        dr(7) = importoDifferenza

        '        dtNonPag.Rows.Add(dr)

        '    Next
        'End If

        'dr = dtNonPag.NewRow()
        'dtNonPag.Rows.Add(dr)

        ''dr = dtNonPag.NewRow()
        ''dr(0) = "Totale soggetti: " & dsContribNonP.Tables(0).Rows.Count
        ''dtNonPag.Rows.Add(dr)

        'If dsContribNonP.Tables(0).Rows.Count > 0 Then
        '    Dim iColumns As Integer() = {0, 1, 2, 3, 4, 5, 6, 7}
        '    Dim objExport As New RKLib.ExportData.Export("Web")
        '    objExport.ExportDetails(dtNonPag, iColumns, arraystr, RKLib.ExportData.Export.ExportFormat.Excel, NameXLS)
        '    Str = sPathProspetti & NameXLS
        'Else
        '    Response.Write("<script language='javascript' type='text/javascript'>GestAlert('a', 'warning', '', '', 'Non esistono elementi da estrarre per i criteri selezionati');")
        'End If
    End Sub
#End Region

#Region "Riversamento"
    Private Sub CmdStampaRiversamento_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdStampaRiversamento.Click
        Dim NameXLS As String = ""
        Dim x As Integer
        Dim FncStampa As New ClsStampaXLS
        Dim FncPagamenti As New ClsPagamenti
        Dim DtDatiStampa As New DataTable
        Dim dvStampa As DataView
        Dim aListColonne As ArrayList
        Dim aMyHeaders As String()
        Try
            'valorizzo il nome del file
            NameXLS = ConstSession.IdEnte & "_RIVERSAMENTO_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
            'prelevo i dati da stampare
            myPagamentiForSearch.IDEnte = ConstSession.IdEnte
            myPagamentiForSearch.sCognome = txtCognome.Text.Trim
            MyPagamentiForSearch.sNome = txtNome.Text.Trim
            If txtPIVA.Text.Trim <> "" Then
                MyPagamentiForSearch.sCodFiscalePIva = txtPIVA.Text.Trim
            Else
                MyPagamentiForSearch.sCodFiscalePIva = txtCF.Text.Trim
            End If
            MyPagamentiForSearch.sNumeroFattura = txtNFattura.Text.Trim
            If txtDataFattura.Text.Trim <> "" Then
                myPagamentiForSearch.sDataFattura = oReplace.FormattaData("A", "/", txtDataFattura.Text.Trim, False)
            Else
                myPagamentiForSearch.sDataFattura = ""
            End If
            If txtDataAccreditoDal.Text.Trim <> "" Then
                myPagamentiForSearch.sDataAccredito = oReplace.FormattaData("A", "/", txtDataAccreditoDal.Text.Trim, False)
            Else
                myPagamentiForSearch.sDataAccredito = ""
            End If
            If txtDataAccreditoAl.Text.Trim <> "" Then
                myPagamentiForSearch.sDataAccreditoAl = oReplace.FormattaData("A", "/", txtDataAccreditoAl.Text.Trim, False)
            Else
                myPagamentiForSearch.sDataAccreditoAl = ""
            End If
            MyPagamentiForSearch.sAnnoEmissioneFattura = txtAnno.Text
            If DdlPeriodo.SelectedValue <> "" Then
                MyPagamentiForSearch.nPeriodo = DdlPeriodo.SelectedValue
            End If
            'dvStampa = FncPagamenti.GetRiversamento(ConstSession.IdEnte, MyPagamentiForSearch)
            dvStampa = FncPagamenti.GetStampaPagamenti(ClsPagamenti.TypeStampa.Riversamento, myPagamentiForSearch)
            DtDatiStampa = FncStampa.PrintRiversamentoH2O(dvStampa, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RicercaPagamenti.CmdStampaRiversamento_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
        If Not DtDatiStampa Is Nothing Then
            '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
            Dim nCol As Integer = 12
            'definisco le colonne
            aListColonne = New ArrayList
            For x = 0 To nCol
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
            MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, NameXLS)
        End If
    End Sub
#End Region

#Region "Elenco Pagamenti"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
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
    Private Sub CmdStampaPagamenti_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdStampaPagamenti.Click
        Dim NameXLS As String = ""
        Dim x As Integer
        Dim FncStampa As New ClsStampaXLS
        Dim FncPagamenti As New ClsPagamenti
        Dim DtDatiStampa As New DataTable
        Dim dvStampa As DataView
        Dim aListColonne As ArrayList
        Dim aMyHeaders As String()
        Try
            'valorizzo il nome del file
            NameXLS = ConstSession.IdEnte & "_ELENCOPAGAMENTI_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
            'prelevo i dati da stampare
            myPagamentiForSearch.IDEnte = ConstSession.IdEnte
            myPagamentiForSearch.sCognome = txtCognome.Text.Trim
            MyPagamentiForSearch.sNome = txtNome.Text.Trim
            If txtPIVA.Text.Trim <> "" Then
                MyPagamentiForSearch.sCodFiscalePIva = txtPIVA.Text.Trim
            Else
                MyPagamentiForSearch.sCodFiscalePIva = txtCF.Text.Trim
            End If
            MyPagamentiForSearch.sNumeroFattura = txtNFattura.Text.Trim
            If txtDataFattura.Text.Trim <> "" Then
                myPagamentiForSearch.sDataFattura = oReplace.FormattaData("A", "/", txtDataFattura.Text.Trim, False)
            Else
                myPagamentiForSearch.sDataFattura = ""
            End If
            If txtDataAccreditoDal.Text.Trim <> "" Then
                myPagamentiForSearch.sDataAccredito = oReplace.FormattaData("A", "/", txtDataAccreditoDal.Text.Trim, False)
            Else
                myPagamentiForSearch.sDataAccredito = ""
            End If
            If txtDataAccreditoAl.Text.Trim <> "" Then
                myPagamentiForSearch.sDataAccreditoAl = oReplace.FormattaData("A", "/", txtDataAccreditoAl.Text.Trim, False)
            Else
                myPagamentiForSearch.sDataAccreditoAl = ""
            End If
            MyPagamentiForSearch.sAnnoEmissioneFattura = txtAnno.Text
            If DdlPeriodo.SelectedValue <> "" Then
                MyPagamentiForSearch.nPeriodo = DdlPeriodo.SelectedValue
            End If
            'dvStampa = FncPagamenti.GetStampaPagamenti(ConstSession.StringConnection, ConstSession.IdEnte, MyPagamentiForSearch)
            dvStampa = FncPagamenti.GetStampaPagamenti(ClsPagamenti.TypeStampa.Pagamenti, myPagamentiForSearch)
            DtDatiStampa = FncStampa.PrintPagamentiH2O(dvStampa, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RicercaPagamenti.CmdStampaPagamenti_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
        If Not DtDatiStampa Is Nothing Then
            Dim nCol As Integer = 20
            'definisco le colonne
            aListColonne = New ArrayList
            For x = 0 To nCol
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
            MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, NameXLS)
        End If
    End Sub
    'Private Sub CmdStampaPagamenti_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdStampaPagamenti.Click
    '    Dim sPathProspetti, NameXLS, Str As String
    '    Dim x As Integer
    '    Dim FncStampa As New ClsStampaXLS
    '    Dim FncPagamenti As New ClsPagamenti
    '    Dim DtDatiStampa As New DataTable
    '    Dim dvStampa As DataView
    '    Dim aListColonne As ArrayList
    '    Dim aMyHeaders As String()
    '    Try
    '        'valorizzo il nome del file
    '        sPathProspetti = ConstSession.PathProspetti
    '        NameXLS = ConstSession.IdEnte & "_ELENCOPAGAMENTI_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
    '        'prelevo i dati da stampare
    '        oPagamentiForSearch.sCognome = txtCognome.Text.Trim
    '        oPagamentiForSearch.sNome = txtNome.Text.Trim
    '        If txtPIVA.Text.Trim <> "" Then
    '            oPagamentiForSearch.sCodFiscalePIva = txtPIVA.Text.Trim
    '        Else
    '            oPagamentiForSearch.sCodFiscalePIva = txtCF.Text.Trim
    '        End If
    '        oPagamentiForSearch.sNumeroFattura = txtNFattura.Text.Trim
    '        If txtDataFattura.Text.Trim <> "" Then
    '            oPagamentiForSearch.sDataFattura = oReplace.FormattaData("A", "/", txtDataFattura.Text.Trim, False)
    '        End If
    '        If txtDataAccreditoDal.Text.Trim <> "" Then
    '            oPagamentiForSearch.sDataAccredito = oReplace.FormattaData("A", "/", txtDataAccreditoDal.Text.Trim, False)
    '        End If
    '        If txtDataAccreditoAl.Text.Trim <> "" Then
    '            oPagamentiForSearch.sDataAccreditoAl = oReplace.FormattaData("A", "/", txtDataAccreditoAl.Text.Trim, False)
    '        End If
    '        oPagamentiForSearch.sAnnoEmissioneFattura = txtAnno.Text
    '        If DdlPeriodo.SelectedValue <> "" Then
    '            oPagamentiForSearch.nPeriodo = DdlPeriodo.SelectedValue
    '        End If
    '        dvStampa = FncPagamenti.GetStampaPagamenti(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione, ConstSession.IdEnte, oPagamentiForSearch)
    '        DtDatiStampa = FncStampa.PrintPagamentiH2O(dvStampa, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte)
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RicercaPagamenti.CmdStampaPagamenti_Click.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    '    If Not DtDatiStampa Is Nothing Then
    '        'definisco le colonne
    '        aListColonne = New ArrayList
    '        '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
    '        For x = 0 To 19      '17
    '            aListColonne.Add("")
    '        Next
    '        aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

    '        'definisco l'insieme delle colonne da esportare
    '        Dim MyCol() As Integer = New Integer() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19}
    '        'esporto i dati in excel
    '        Dim MyStampa As New RKLib.ExportData.Export("Web")
    '        MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, NameXLS)
    '        Str = sPathProspetti & NameXLS
    '    End If
    'End Sub
#End Region

#Region "Insoluti"
    Private Sub CmdStampaInsoluto_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdStampaInsoluto.Click
        Dim NameXLS As String = ""
        Dim x As Integer
        Dim FncStampa As New ClsStampaXLS
        Dim FncPagamenti As New ClsPagamenti
        Dim DtDatiStampa As New DataTable
        Dim dvStampa As DataView
        Dim aListColonne As ArrayList
        Dim aMyHeaders As String()
        Try
            'valorizzo il nome del file
            NameXLS = ConstSession.IdEnte & "_ELENCOINSOLUTI_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
            'prelevo i dati da stampare
            myPagamentiForSearch.IDEnte = ConstSession.IdEnte
            MyPagamentiForSearch.sCognome = txtCognome.Text.Trim
            MyPagamentiForSearch.sNome = txtNome.Text.Trim
            If txtPIVA.Text.Trim <> "" Then
                MyPagamentiForSearch.sCodFiscalePIva = txtPIVA.Text.Trim
            Else
                MyPagamentiForSearch.sCodFiscalePIva = txtCF.Text.Trim
            End If
            MyPagamentiForSearch.sNumeroFattura = txtNFattura.Text.Trim
            If txtDataFattura.Text.Trim <> "" Then
                myPagamentiForSearch.sDataFattura = oReplace.FormattaData("A", "/", txtDataFattura.Text.Trim, False)
            Else
                myPagamentiForSearch.sDataFattura = ""
            End If
            If txtDataAccreditoDal.Text.Trim <> "" Then
                myPagamentiForSearch.sDataAccredito = oReplace.FormattaData("A", "/", txtDataAccreditoDal.Text.Trim, False)
            Else
                myPagamentiForSearch.sDataAccredito = ""
            End If
            If txtDataAccreditoAl.Text.Trim <> "" Then
                myPagamentiForSearch.sDataAccreditoAl = oReplace.FormattaData("A", "/", txtDataAccreditoAl.Text.Trim, False)
            Else
                myPagamentiForSearch.sDataAccreditoAl = ""
            End If
            MyPagamentiForSearch.sAnnoEmissioneFattura = txtAnno.Text
            If DdlPeriodo.SelectedValue <> "" Then
                MyPagamentiForSearch.nPeriodo = DdlPeriodo.SelectedValue
            End If
            'dvStampa = FncPagamenti.GetInsoluti(ConstSession.IdEnte, oPagamentiForSearch)
            dvStampa = FncPagamenti.GetStampaPagamenti(ClsPagamenti.TypeStampa.Insoluti, myPagamentiForSearch)
            DtDatiStampa = FncStampa.PrintInsolutiH2O(dvStampa, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RicercaPagamenti.CmdStampaInsoluto_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
        If Not DtDatiStampa Is Nothing Then
            Dim nCol As Integer = 8
            'definisco le colonne
            aListColonne = New ArrayList
            For x = 0 To nCol
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
            MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, NameXLS)
        End If
    End Sub
#End Region

    Protected Function FormattaNumero(ByVal NumeroDaFormattareParam As String) As String
		FormattaNumero = Replace(NumeroDaFormattareParam, ",", ".")
	End Function
End Class
