''' <summary>
''' Pagina dei comandi per la consultazione del provvedimento generato.
''' Le possibili opzioni sono:
''' - Torna alla videata precedente
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class ComandiRiepilogoAccertamento
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim sScript As String = ""

        lblTitolo.Text = ConstSession.DescrizioneEnte
        info.InnerText = "Accertamenti "
        If Utility.StringOperation.FormatString(Request.Item("Tributo")) = Utility.Costanti.TRIBUTO_TASI Then
            info.InnerText += "TASI"
        Else
            info.innertext += "IMU"
        End If
        info.InnerText += " -  Riepilogo Accertato"
    End Sub

End Class
