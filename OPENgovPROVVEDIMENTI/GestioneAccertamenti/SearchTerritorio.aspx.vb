Imports log4net

Partial Class SearchTerritorio
    Inherits BasePage

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents btnNuovo As System.Web.UI.WebControls.Button

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Public codContribuente As String
    Public annoAccertamento As String
    Public txtNominativo As String
   


    Private Shared Log As ILog = LogManager.GetLogger(GetType(SearchTerritorio))
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here

        Try
            If Not Page.IsPostBack Then
                ViewState.Add("codContribuente", Request.Item("codContribuente"))
                annoAccertamento = Request.Item("anno")


                lblContribuente.Text = Server.UrlDecode(Request.Item("nominativo"))
                lblAnnoAccertamento.Text = annoAccertamento
                dim sScript as string=""
                sscript+= "parent.Comandi.location.href='ComandiGestioneTerritorio.aspx'" & vbCrLf
                RegisterScript(sScript , Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchTerritorio.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub btnSearchTerritorio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearchTerritorio.Click
        Try
            dim sScript as string=""
            Dim objHashTable As Hashtable = New Hashtable

            objHashTable.Add("CODCONTRIBUENTE", ViewState("codContribuente"))
            objHashTable.Add("ANNOACCERTAMENTO", lblAnnoAccertamento.Text)
            objHashTable.Add("FOGLIO", txtFoglio.Text)
            objHashTable.Add("NUMERO", txtNumero.Text)
            objHashTable.Add("SUBALTERNO", txtSubalterno.Text)
            objHashTable.Add("CLASSE", txtClasse.Text)
            objHashTable.Add("CATEGORIA", txtCategoria.Text)

            Session("HashTableDichiarazioniAccertamentiTerritorio") = objHashTable

            sScript += "frames.item('loadGrid').location.href ='SearchDatiTerritorio.aspx';"

            RegisterScript(sScript , Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchTerritorio.btnSearchTerritorio_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
