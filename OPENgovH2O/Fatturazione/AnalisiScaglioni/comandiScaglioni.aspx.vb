Imports log4net

Partial Class comandiScaglioni
    Inherits BasePage

    Private Shared Log As ILog = LogManager.GetLogger(GetType(comandiScaglioni))
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
        'info.Text = "Acquedotto - Analisi Scaglioni"
        'Comune.Text = " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo
        Try
            If ConstSession.DescrPeriodo = "" Then
                Dim FncGen As New ClsGenerale.Generale
                FncGen.GetPeriodoAttuale
            End If
            info.Text = "Acquedotto - Analisi Scaglioni"
            Comune.Text = ConstSession.DescrizioneEnte & "   -   Periodo: " & ConstSession.DescrPeriodo
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.comandiScaglioni.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

End Class
