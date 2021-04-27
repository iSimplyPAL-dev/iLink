Partial Class RicercaNoloC
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
    ''' Configurazione per anno e per singola tipologia di contatore del valore del nolo	
    ''' possibilità di applicare o meno l'iva	
    ''' possibilità di voler calcolare il nolo una tantum su un singolo utente e non a livello massivo (flaggherò unatantum ed agirò sul singolo utente in fase di fatturazione)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim ClsNoliContatore As New ClsNoliContatore
        Dim sScript As String
        ClsNoliContatore.LoadComboTipoContatore(ddlTipoContatore)
        'If Request.Item("EffettuaRicerca") = "si" Then
        sScript = "Search();"
        RegisterScript(sScript, Me.GetType())

        sScript = "parent.parent.Comandi.location.href='./CRicercaNoloC.aspx';"
        RegisterScript(sScript, Me.GetType())
    End Sub
End Class
