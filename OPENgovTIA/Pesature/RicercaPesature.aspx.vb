Imports log4net
''' <summary>
''' Pagina per la ricerca pesature.
''' Contiene i parametri di ricerca e le funzioni della comandiera.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class RicercaPesature
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(RicercaPesature))

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

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
            If ConstSession.IsFromVariabile("1") = "1" Then 'sempre IsFromVariabile
            End If

            TxtCognome.Attributes.Add("onkeydown", "keyPress();")
            TxtNome.Attributes.Add("onkeydown", "keyPress();")
            TxtCodFiscale.Attributes.Add("onkeydown", "keyPress();")
            TxtPIva.Attributes.Add("onkeydown", "keyPress();")
            TxtCodUtente.Attributes.Add("onkeydown", "keyPress();")
            TxtNTessera.Attributes.Add("onkeydown", "keyPress();")
            TxtCodTessera.Attributes.Add("onkeydown", "keyPress();")
            TxtDal.Attributes.Add("onkeydown", "keyPress();")
            TxtAl.Attributes.Add("onkeydown", "keyPress();")
            If Page.IsPostBack = False Then
                'controllo se l'oggetto ricerca è valorizzato
                If Not Session("oRicercaUtentePesature") Is Nothing Then
                    'carico la ricerca già effettuata
                    Dim oMyRicerca As New ObjRicercaUtentePesature
                    oMyRicerca = Session("oRicercaUtentePesature")
                    If oMyRicerca.sCognome <> "" Then
                        TxtCognome.Text = oMyRicerca.sCognome
                    End If
                    If oMyRicerca.sNome <> "" Then
                        TxtNome.Text = oMyRicerca.sNome
                    End If
                    If oMyRicerca.sCodFiscale <> "" Then
                        TxtCodFiscale.Text = oMyRicerca.sCodFiscale
                    End If
                    If oMyRicerca.sPIva <> "" Then
                        TxtPIva.Text = oMyRicerca.sPIva
                    End If
                    If oMyRicerca.sCodUtente <> "" Then
                        TxtCodUtente.Text = oMyRicerca.sCodUtente
                    End If
                    If oMyRicerca.sNumTessera <> "" Then
                        TxtNTessera.Text = oMyRicerca.sNumTessera
                    End If
                    'If oMyRicerca.nNumeroTessera <> -1 Then
                    '    TxtCodTessera.Text = oMyRicerca.nNumeroTessera
                    'End If
                    If oMyRicerca.tPeriodoDal <> Date.MinValue And oMyRicerca.tPeriodoDal <> Date.MaxValue Then
                        TxtDal.Text = oMyRicerca.tPeriodoDal
                    End If
                    If oMyRicerca.tPeriodoAl <> Date.MinValue And oMyRicerca.tPeriodoAl <> Date.MaxValue Then
                        TxtAl.Text = oMyRicerca.tPeriodoAl
                    End If
                    'ricarico la ricerca
                    Dim sScript As String
                    sScript = "Search();"
                    RegisterScript(sScript, Me.GetType)
                End If
                Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, "Pesature", "Ricerca", Utility.Costanti.AZIONE_LETTURA.ToString, ConstSession.CodTributo, ConstSession.IdEnte, -1)
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.RicercaPesature.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    ''' <summary>
    ''' Pulsante per l'estrazione in formato XLS della ricerca effettuata
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdStampa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdStampa.Click
        Dim MyFunction As New ObjRicercaUtentePesature
        Dim oMyRicerca As New ObjRicercaUtentePesature
        Dim oListUtentiPesature() As ObjUtentePesature
        Dim x, nCol As Integer
        Dim sPathProspetti, sNameXLS As String
        Dim FunctionStampa As New ClsStampaXLS
        Dim DtDatiStampa As New DataTable
        Dim aListColonne As ArrayList
        Dim aMyHeaders As String()
        Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
        nCol = 8
        Try
            oMyRicerca.sEnte = ConstSession.IdEnte
            oMyRicerca.sCognome = TxtCognome.Text
            oMyRicerca.sNome = TxtNome.Text
            oMyRicerca.sCodFiscale = TxtCodFiscale.Text
            oMyRicerca.sPIva = TxtPIva.Text
            oMyRicerca.sCodUtente = TxtCodUtente.Text
            oMyRicerca.sNumTessera = TxtNTessera.Text
            If TxtCodTessera.Text <> "" Then
                oMyRicerca.sNumTessera = TxtCodTessera.Text
            End If
            If TxtDal.Text <> "" Then
                oMyRicerca.tPeriodoDal = TxtDal.Text
            End If
            If TxtAl.Text <> "" Then
                oMyRicerca.tPeriodoAl = TxtAl.Text
            End If
            If optYes.Checked Then
                oMyRicerca.IsFatturato = 1
            End If
            oMyRicerca.sFileImport = TxtFileImport.Text
            oListUtentiPesature = MyFunction.GetRicercaPesature(oMyRicerca, cmdMyCommand)
        Catch Err As Exception
            oListUtentiPesature = Nothing
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.RicercaPesature.CmdStampa_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
        'valorizzo il nome del file
        If Not oListUtentiPesature Is Nothing Then
            sPathProspetti = ConstSession.PathProspetti
            sNameXLS = ConstSession.IdEnte & "_PESATURE_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
            DtDatiStampa = FunctionStampa.PrintRicercaPesature(oListUtentiPesature, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte)
            If Not DtDatiStampa Is Nothing Then
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
                MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
            End If
        End If
    End Sub
End Class
