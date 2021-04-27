Imports log4net

Partial Class RicercaRuoliPerConsultaDocElaborati
    Inherits BasePage

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
    Private oLoadCombo As New ClsGenerale.Generale

    Private Shared Log As ILog = LogManager.GetLogger(GetType(RicercaRuoliPerConsultaDocElaborati))
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        '********************************************************
        'devo caricare le combo con i dati
        '********************************************************
        dim sSQL as string
        Try
            If IsPostBack = False Then
                '********************************************************
                'carico le combo
                'carico gli anni dei ruoli approvati
                '********************************************************
                sSQL = "SELECT (SUBSTRING(DATA_EMISSIONE_FATTURA,7, 2) + '/' + SUBSTRING(DATA_EMISSIONE_FATTURA, 5, 2) + '/' + SUBSTRING(DATA_EMISSIONE_FATTURA,1, 4)) AS DATA_EMISSIONE_FATTURA, DATA_EMISSIONE_FATTURA AS ID"
                sSQL += " FROM TP_FATTURAZIONI_GENERATE"
                sSQL += " WHERE (IDENTE='" & ConstSession.IdEnte & "') AND (IDPERIODO=" & ConstSession.IdPeriodo & ") AND (NOT DATA_NUMERAZIONE IS NULL)"
                sSQL += " GROUP BY DATA_EMISSIONE_FATTURA"
                sSQL += " ORDER BY DATA_EMISSIONE_FATTURA DESC"
                oLoadCombo.LoadComboGenerale(cmbDataFattura, sSQL)
            End If
            Session("nDocDaElaborare") = 0

            Dim sScript As String
            'str = "Search();"
            'RegisterScript(sScript , Me.GetType())

            sScript = "parent.parent.Comandi.location.href='./ComandiConsultaDocElaborati.aspx';"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RicercaRuoliPerConsultaDocElaborati.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
