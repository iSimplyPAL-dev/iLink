Partial Class RicercaAddizionali
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
    ''' Configurazione per anno dei valori delle spese fisse 
    ''' possibilità di applicare o meno l'iva
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim clsAddizionali As New ClsAddizionali
        Dim sScript As String
        clsAddizionali.LoadComboAddizionali(ddlAddizionali)
        'If Request.Item("EffettuaRicerca") = "si" Then
        sScript = "Search();"
        RegisterScript(sScript , Me.GetType())

        sScript = "parent.parent.Comandi.location.href='./CRicercaAddizionali.aspx';"
        RegisterScript(sScript , Me.GetType())

    End Sub

End Class
