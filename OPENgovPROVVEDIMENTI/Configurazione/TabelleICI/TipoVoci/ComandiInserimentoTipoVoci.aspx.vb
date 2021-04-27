''' <summary>
''' Pagina dei comandi per la gestione dell'inserimento tipo voci.
''' Le possibili opzioni sono:
''' - Abilita
''' - Elimina
''' - Configura
''' - Salva
''' - Torna alla videata precedente
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class ComandiInserimentoTipoVoci
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
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        infoEnte.InnerText = ConstSession.DescrizioneEnte
        info.InnerText = "Configurazione - Tabelle - Voci"
    End Sub
End Class
