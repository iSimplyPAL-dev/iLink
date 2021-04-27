Imports System.Web.UI.Control
Imports System.Web.UI
Imports log4net
''' <summary>
''' Pagina dei comandi per la ricerca delle posizioni anagrafiche.
''' Le possibili opzioni sono:
''' - Stampa
''' - Inserisci nuovo
''' - Ricerca
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class ComandiRicercaAnagrafica
    Inherits ANAGRAFICAWEB.BasePage

    Private Shared Log As ILog = LogManager.GetLogger(GetType(ComandiRicercaAnagrafica))
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
        'Put user code to initialize the page here
        Dim sScript As String = ""

        infoEnte.Text = ANAGRAFICAWEB.ConstSession.DescrizioneEnte
        info.Text = "Anagrafica - Ricerca"
        Try
            '*** 201511 - Funzioni Sovracomunali ***
            If ANAGRAFICAWEB.ConstSession.IdEnte = "" Then
                'sScript = "document.getElementById('NewInsert').style.display='none';"
                sScript += "$('#NewInsert').addClass('DisableBtn');"
                sScript += "$('#NewInsert').prop('disabled', true);"
                RegisterScript(sScript, Me.GetType)
            End If
            '*** ***
        Catch ex As Exception
            Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.ComandiRicercaAnagrafica.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
