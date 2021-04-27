''' <summary>
''' Pagina dei comandi per la generazione dei provvedimenti da accertamenti pregressi.
''' Le possibili opzioni sono:
''' - Associa
''' - Torna alla videata precedente
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class ComandiGestioneAccertato
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
        lblTitolo.Text = ConstSession.DescrizioneEnte
        Select Case ConstSession.CodTributo
            Case Utility.Costanti.TRIBUTO_ICI
                info.InnerText = "IMU "
            Case Utility.Costanti.TRIBUTO_TASI
                info.InnerText = "TASI "
            Case Utility.Costanti.TRIBUTO_TARSU
                info.InnerText = "TARES "
                If ConstSession.IsFromVariabile(Request.Item("IsFromVariabile")) = "1" Then
                    info.InnerText += "Variabile"
                End If
            Case Utility.Costanti.TRIBUTO_OSAP
                info.InnerText = "OSAP "
        End Select
        info.InnerText += " - Accertamenti - Ricerca Accertato"
    End Sub

End Class
