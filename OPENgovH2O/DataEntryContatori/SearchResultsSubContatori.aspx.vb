Imports System
Imports log4net

Partial Class SearchResultsSubContatori
    Inherits BasePage
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(SearchResultsSubContatori))

#Region " Web Form Designer Generated Code "
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents btnAssocia As System.Web.UI.WebControls.Button

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
    End Sub

    Private Sub btnAssocia_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAssocia.Click
    End Sub
End Class
