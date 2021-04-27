Imports RIBESElaborazioneDocumentiInterface.Stampa.oggetti
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports log4net

Partial Class ConsultaDocElaborati
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ConsultaDocElaborati))
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
        'Put user code to initialize the page here
        Dim idFRuolo As Integer
        Dim oclsElabDoc As New ClsElaborazioneDocumenti

        Dim oArrayGruppoURL() As GruppoURL

        Dim oArrayOggettoUrl() As oggettoURL
        Dim indiceoArrayOggettoUrl As Integer
        Dim indice As Integer
        Try
            oArrayOggettoUrl = Nothing
            If Page.IsPostBack = False Then

                idFRuolo = CInt(Session("idFlussoRuolo"))
                oArrayGruppoURL = oclsElabDoc.GetDocElaboratiEffettivi(idFRuolo)

                indiceoArrayOggettoUrl = 0
                For indice = 0 To oArrayGruppoURL.Length - 1
                    ReDim Preserve oArrayOggettoUrl(indiceoArrayOggettoUrl)
                    oArrayOggettoUrl(indiceoArrayOggettoUrl) = oArrayGruppoURL(indice).URLComplessivo
                    indiceoArrayOggettoUrl += 1
                Next

                ViewState.Add("DocumentiStampati_URLComplessivo", oArrayGruppoURL)
                Session("oListConsultaDoc") = oArrayOggettoUrl
                GrdResult.DataSource = oArrayOggettoUrl

                GrdResult.DataBind()
            Else
                oArrayOggettoUrl = CType(Session("oListConsultaDoc"), oggettoURL())
                GrdResult.DataSource = oArrayOggettoUrl
            End If
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ConsultaDocElaborati.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
