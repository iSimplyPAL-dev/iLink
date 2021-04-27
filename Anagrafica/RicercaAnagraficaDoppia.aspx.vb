Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports Anagrafica
Imports System.Web.SessionState.HttpSessionState
Imports AnagInterface
Imports log4net
''' <summary>
''' Pagina per la ricerca delle anagrafiche doppie.
''' Contiene i parametri di ricerca.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class RicercaAnagraficaDoppia
    Inherits ANAGRAFICAWEB.BasePage

    Private Shared Log As ILog = LogManager.GetLogger(GetType(RicercaAnagraficaDoppia))
    Public oAnagrafica As DLL.GestioneAnagrafica
    Public oDettaglioAnagrafica As DettaglioAnagrafica
    Public sessionName As Object
    Private Const SEARCH_PARAMETRES As String = "SEARCHPARAMETRES"
    Private Const FIRST_TIME As String = "1"

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
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                txtPercentuale.Text = "100"
                Dim sScript As String = ""
                sScript = "parent.Comandi.location.href='./Comandi/ComandiRicercaAnagraficaDop.aspx';"
                RegisterScript( sScript,Me.GetType)
            End If
        Catch ex As Exception
            Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.RicercaAnagraficaDoppia.Page_Load.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
        End Try
    End Sub
End Class


