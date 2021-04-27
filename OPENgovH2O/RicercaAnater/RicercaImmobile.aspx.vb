Imports log4net

Partial Class RicercaImmobile
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(RicercaImmobile))
    Protected UrlStradario As String = ConstSession.UrlStradario

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
        Dim sScript As String
        Try
            If Not IsPostBack Then
                LnkApriStradario.Attributes.Add("onclick", "ApriStradario();")
                LnkPulisciStrada.Attributes.Add("onclick", "return ClearDatiVia();")
            End If
            If Not Request.Item("Via") Is Nothing Then
                TxtVia.Text = Request.Item("Via")
                sScript = "SearchImmobileAnater();"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RicercaImmobile.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try

    End Sub
End Class
