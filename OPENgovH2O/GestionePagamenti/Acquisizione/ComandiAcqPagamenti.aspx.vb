Public Class ComandiAcqPagamenti
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Comune As System.Web.UI.WebControls.Label
    Protected WithEvents info As System.Web.UI.WebControls.Label

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
        info.Text = "Acquedotto - " & CType(Session("DESC_TIPO_PROC_SERV"), String).Replace("Tabella", "")
        Comune.Text = "Ente: " & Session("COMUNEENTE") & " -- Periodo: " & Session("PERIODO")
    End Sub
End Class
