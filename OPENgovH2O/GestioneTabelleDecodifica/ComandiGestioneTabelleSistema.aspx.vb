Imports log4net

Partial Class ComandiGestioneTabelleSistema
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
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ComandiGestioneTabelleSistema))
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'Dim prova As String

        'prova = "Acquedotto - Configurazioni -" & CType(Session("DESC_TIPO_PROC_SERV"), String).Replace("Tabella", "")
        'info.Text = Request("title")
        'info.Text = "Acquedotto - Configurazioni -" & CType(Session("DESC_TIPO_PROC_SERV"), String).Replace("Tabella", "")
        'Comune.Text = Request("enteperiodo")
        Try
            If ConstSession.DescrPeriodo = "" Then
                Dim FncGen As New ClsGenerale.Generale
                FncGen.GetPeriodoAttuale
            End If
            info.Text = "Acquedotto - Configurazioni"
            Comune.Text = ConstSession.DescrizioneEnte
            If ConstSession.DescrPeriodo <> "" Then
                Comune.Text += "   -   Periodo: " & ConstSession.DescrPeriodo
            End If
            Select Case Request("conf")
                Case 1
                    info.Text += " Impianto"
                Case 2
                    info.Text += " Periodo"
                Case 3
                    info.Text += " Attività"
                Case 4
                    info.Text += " Giri"
                Case 5
                    info.Text += " Posizione Contatore"
                Case 6
                    info.Text += " Tipo Contatore"
                Case 7
                    info.Text += " Tipo Utenza"
                Case 8
                    info.Text += " Diametro Contatore"
                Case 9
                    info.Text += " Diametro Presa"
                Case 10
                    info.Text += " Anomalie"
                Case Else
            End Select
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ComandiGestioneTabelleSistema.Page_Load.errore :   ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

End Class
