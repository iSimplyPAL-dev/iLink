Imports System.Web.UI.Control
Imports System.Web.UI

Partial Class ComandiGestioneAttiRuoloCoattivo
    Inherits BasePage
    Public strPARAMETRI As String


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
        'Put user code to initialize the page here
        Dim strTipoProvvedimento As String
        Dim strUTENTE As String
        Dim objUtility As New MyUtility

        strTipoProvvedimento = objUtility.CToStr(Request("TIPO_PROVVEDIMENTO"))
        strUTENTE = objUtility.CToStr(Request("UTENTE"))

        strPARAMETRI = "?TIPO_PROVVEDIMENTO=" & strTipoProvvedimento & "&UTENTE=" & strUTENTE & "&NOMEPAGINA=" & "StatoElaborazioneQuestionari.aspx"
        txtPagina.Text = strPARAMETRI
    End Sub

End Class
