Imports log4net

Partial Class ComandiGestioneTabelle
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ComandiGestioneTabelle))

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

        'Dim prova As String

        'prova = "Acquedotto - Configurazioni -" & CType(Session("DESC_TIPO_PROC_SERV"), String).Replace("Tabella", "")
        'info.Text = Request("title")
        'info.Text = "Acquedotto - Configurazioni -" & CType(Session("DESC_TIPO_PROC_SERV"), String).Replace("Tabella", "").Replace("periodo", "Periodo").Replace("Attivita", "Attività")
        'Comune.Text = Request("enteperiodo")
        Try
            If ConstSession.DescrPeriodo = "" Then
            Dim FncGen As New ClsGenerale.Generale
            FncGen.GetPeriodoAttuale
        End If
            info.Text = "Acquedotto - Configurazioni - " & Utility.StringOperation.FormatString(Request("title")) '& CType(Session("DESC_TIPO_PROC_SERV"), String).Replace("Tabella", "")
            Comune.Text = ConstSession.DescrizioneEnte & "   -   Periodo: " & ConstSession.DescrPeriodo
            'Select Case Request("PAG_PREC")
            '    Case "STRADE"


            'End Select

        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ComandiGestioneTabelle.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub



End Class
