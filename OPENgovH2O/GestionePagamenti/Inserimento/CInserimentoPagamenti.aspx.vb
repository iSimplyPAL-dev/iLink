Imports log4net

Partial Class CInserimentoPagamenti
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



    Private Shared Log As ILog = LogManager.GetLogger(GetType(CInserimentoPagamenti))
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sScript As String = ""
        'info.Text = "Acquedotto - " & CType(Session("DESC_TIPO_PROC_SERV"), String).Replace("Tabella", "")
        'Comune.Text = "Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo
        Try
            If ConstSession.DescrPeriodo = "" Then
                Dim FncGen As New ClsGenerale.Generale
                FncGen.GetPeriodoAttuale
            End If
            info.Text = "Acquedotto - Pagamenti - Gestione"
            Comune.Text = ConstSession.DescrizioneEnte & "   -   Periodo: " & ConstSession.DescrPeriodo

            'Put user code to initialize the page here
            If Request.Item("Selezionato") Is Nothing Or Request.Item("Selezionato") <> "Si" Then
                sScript = ""
                sScript += "document.getElementById('Delete').disabled=true;"
                sScript += "document.getElementById('Insert').disabled=true;"
                sScript += ""
                RegisterScript(sScript, Me.GetType())
            ElseIf Request.Item("Selezionato") = "Si" Then
                sScript = ""
                sScript += "document.getElementById('Delete').disabled=false;"
                sScript += "document.getElementById('Insert').disabled=false;"
                sScript += ""
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.CInserimentoPagamenti.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

End Class
