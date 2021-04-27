
Imports log4net

Partial Class RicercaFatture
    Inherits BasePage
    Private Cognome, Nome, ImportoPagamento, ImportoEmesso, NFattura, DataFattura, Anno, Operazione As String
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents StampaExcel As System.Web.UI.WebControls.Button
    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Private oReplace As New ClsGenerale.Generale
    Private clsGenerale As New ClsGenerale.Generale
    Private Shared Log As ILog = LogManager.GetLogger(GetType(RicercaFatture))
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            'Put user code to initialize the page here
            Operazione = Request.Item("Operazione")
            If Not Operazione Is Nothing Then
                txtOperazione.Text = Operazione
            End If
            If Operazione = "modifica" Then
                Cognome = Request.Item("Cognome")
                Nome = Request.Item("Nome")
                ImportoPagamento = Request.Item("ImportoPagamento")
                ImportoEmesso = Request.Item("ImportoEmesso")
                NFattura = Request.Item("NumeroFattura")
                DataFattura = Request.Item("DataFattura")
                Anno = Request.Item("Anno")

                txtCognome.Text = Cognome
                txtNome.Text = Nome
                txtAnno.Text = Anno
                txtDataFattura.Text = DataFattura
                txtNFattura.Text = NFattura
                txtImporto.Text = ImportoEmesso

                Dim sScript As String
                sScript = "SearchModifica();"
                RegisterScript(sScript, Me.GetType())

                txtDataFattura.Text = oReplace.GiraDataFromDB(DataFattura)
                txtImporto.Text = New ClsGenerale.FunctionGrd().euroforgridview(ImportoEmesso)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RicercaFatture.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
