Imports log4net

Partial Class ComandiRicercaContatori
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


    Private Shared Log As ILog = LogManager.GetLogger(GetType(ComandiRicercaContatori))
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim sScript As String = ""
        Try
            If ConstSession.DescrPeriodo = "" Then
                Dim FncGen As New ClsGenerale.Generale
                FncGen.GetPeriodoAttuale()
            End If
            info.Text = "Acquedotto - Contatori - Ricerca"
            Comune.Text = ConstSession.DescrizioneEnte
            If Not ConstSession.DescrPeriodo Is Nothing Then
                Comune.Text += "   -   Periodo: " & ConstSession.DescrPeriodo
            End If
            '*** 20140923 - GIS ***
            If ConstSession.VisualGIS = False Then
                sScript += "$('#GIS').addClass('DisableBtn');"
                RegisterScript(sScript, Me.GetType)
            End If
            '*** ***
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ComandiRicercaContatori.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
