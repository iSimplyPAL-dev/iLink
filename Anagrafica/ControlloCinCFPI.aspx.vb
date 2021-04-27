Imports Anagrafica.DLL
Imports log4net
''' <summary>
''' Pagina nascosta per il controllo di congruenza di Codice Fiscale e Partita IVA.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class ControlloCinCFPI
    Inherits ANAGRAFICAWEB.BasePage

    Private Shared Log As ILog = LogManager.GetLogger(GetType(ControlloCinCFPI))
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
    '*** 201405 - NO RIBESFRAMEWORK ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim Cf As String
        Dim Pi As String
        Dim sScript As String = ""
        Dim controlloCFPI As New ControlliCFPI()

        Cf = Request.Item("CF")
        Pi = Request.Item("PI")

        Try

            If Len(Cf) > 0 Then
                If Not controlloCFPI.ControlloCinCF(Cf.ToUpper) Then
                    sScript += "ConfermaCINErrato('Controllo CIN Codice Fiscale non corretto.');"
                    RegisterScript(sScript, Me.GetType)
                End If
            End If

            If Len(Pi) > 0 Then
                If Not controlloCFPI.ControlloCinPI(Pi.ToUpper) Then
                    sScript += "ConfermaCINErrato('Controllo CIN Partita Iva non corretto.');"
                    RegisterScript(sScript, Me.GetType)
                End If
            End If

            ' se i controlli sono stati superati chiamo il Bottone del salvataggio.
            sScript += "SalvaAnagrafe();"
            RegisterScript(sScript, Me.GetType)
        Catch Ex As Exception
            Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.ControlloCinNCFPI.Page_Load.errore: ", Ex)
            Response.Redirect("../PaginaErrore.aspx")
        End Try
    End Sub
End Class
