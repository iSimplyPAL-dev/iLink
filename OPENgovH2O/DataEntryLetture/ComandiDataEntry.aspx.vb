Imports log4net

Partial Class ComandiDataEntry
    Inherits BasePage

    Private Shared Log As ILog = LogManager.GetLogger(GetType(ComandiDataEntry))
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sScript As String

        'info.Text = Request("title")
        'Comune.Text = Request("enteperiodo")
        Try
            If ConstSession.DescrPeriodo = "" Then
                Dim FncGen As New ClsGenerale.Generale
                FncGen.GetPeriodoAttuale
            End If
            info.Text = "Acquedotto - Letture - Gestione"
            Comune.Text = ConstSession.DescrizioneEnte & "   -   Periodo: " & ConstSession.DescrPeriodo

            If Request.Item("sprovenienza") = "AE" Then
                sScript = "document.getElementById('Conferma').style.display='none';"
                sScript += "document.getElementById('Salva').style.display='none';"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ComandiDataEntry.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

End Class
