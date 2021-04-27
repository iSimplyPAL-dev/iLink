Imports log4net

Partial Class CRicercaAddizionali
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
    Private Shared Log As ILog = LogManager.GetLogger(GetType(CRicercaAddizionali))

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'info.Text = "Acquedotto - Configurazioni -" & CType(Session("DESC_TIPO_PROC_SERV"), String).Replace("Tabella", "")
        'Comune.Text = "Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo
        Try
            If ConstSession.DescrPeriodo = "" Then
                Dim FncGen As New ClsGenerale.Generale
                FncGen.GetPeriodoAttuale
            End If
            info.Text = "Acquedotto - Configurazioni - Addizionali"
            Comune.Text = ConstSession.DescrizioneEnte & "   -   Periodo: " & ConstSession.DescrPeriodo

            Dim sArray() As Object

            ReDim Preserve sArray(0)

            sArray(0) = "NewInsert"

            'Dim ClsGenerale As New ClsGenerale.Generale

            'If Session("dirittioperatore") = "LETTURA" Then
            '    'str = "document.getElementById('ddlAddizionali').disabled=true;"
            '    str = ClsGenerale.PopolaJSdisabilita(sArray)
            '    RegisterScript(sScript , Me.GetType())"load", str)
            'End If
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.CRicercaAddizionali.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

End Class
