Imports log4net
''' <summary>
''' Pagina dei comandi per la consultazione del provvedimento generato.
''' Le possibili opzioni sono:
''' - Torna alla videata precedente
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class ComandiRiepilogoAccertamentoTARSU
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ComandiRiepilogoAccertamentoTARSU))
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents infoEnte As System.Web.UI.WebControls.Label
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
        Dim sRETTIFICATO As String = Request.Item("rettificato")
        Try
            'If sRETTIFICATO = 1 Then
            '    sScript = "document.getElementById('Search').style.display='none'"
            '    sScript += "document.getElementById('close').style.display=''"
            'Else
            '    sScript = "document.getElementById('Search').style.display=''"
            '    sScript += "document.getElementById('close').style.display='none'"
            'End If
            'sScript = "AbilitaPulsanti('" & sRETTIFICATO & "')"
            'RegisterScript("abilbtn", "<script language='javascript'>" & sScript & "</script>")

            If sRETTIFICATO = 1 Then
                btnSearch.Visible = False
                btnClose.Visible = True
            Else
                btnSearch.Visible = True
                btnClose.Visible = False
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ComandiRiepilogoAccertamentoTARSU.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
