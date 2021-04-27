Imports log4net
''' <summary>
''' Pagina per la ricerca immobili da ANATER.
''' Contiene i parametri di ricerca e le funzioni della comandiera.
''' </summary>
Partial Class RicercaImmobile
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(RicercaImmobile))
    Protected UrlStradario As String = ConstSession.UrlStradario

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
        Dim bRicerca As Boolean = False
        Try
            If Not IsPostBack Then
                LnkApriStradario.Attributes.Add("onclick", "ApriStradario();")
                LnkPulisciStrada.Attributes.Add("onclick", "return ClearDatiVia();")
                If Request.Item("TxtCodVia") <> "" Then
                    txtCodVia.Text = Request.Item("TxtCodVia")
                    bRicerca = True
                End If
                If Request.Item("TxtVia") <> "" Then
                    TxtVia.Text = Request.Item("TxtVia")
                    bRicerca = True
                End If
                If Request.Item("TxtCivico") <> "" Then
                    TxtCivico.Text = Request.Item("TxtCivico")
                End If
                If Request.Item("TxtInterno") <> "" Then
                    TxtInterno.Text = Request.Item("TxtInterno")
                End If
                If Request.Item("TxtFoglio") <> "" Then
                    TxtFoglio.Text = Request.Item("TxtFoglio")
                End If
                If Request.Item("TxtNumero") <> "" Then
                    TxtNumero.Text = Request.Item("TxtNumero")
                End If
                If Request.Item("TxtSubalterno") <> "" Then
                    TxtSubalterno.Text = Request.Item("TxtSubalterno")
                End If
                If bRicerca = True Then
                    TxtVia.Enabled = False : TxtCivico.Enabled = False : TxtInterno.Enabled = False
                    TxtFoglio.Enabled = False : TxtNumero.Enabled = False : TxtSubalterno.Enabled = False
                    LnkApriStradario.Enabled = False : LnkPulisciStrada.Enabled = False
                    Dim sScript As String = "SearchImmobileAnater(1);"
                    RegisterScript( sScript,Me.GetType)
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.RicercaImmobile.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
