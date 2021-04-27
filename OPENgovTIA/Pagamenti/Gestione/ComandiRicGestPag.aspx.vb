Imports log4net
''' <summary>
''' Pagina dei comandi per la gestione del pagamento.
''' Le possibili opzioni sono:
''' - Riversamento
''' - Quadratura
''' - Elenco soggetti che non hanno pagato
''' - Elenco soggetti con importo pagato maggiore di importo emesso
''' - Elenco soggetti con importo pagato minore di importo emesso
''' - Elenco pagamenti
''' - Nuovo Inserimento
''' - Ricerca
''' </summary>
Partial Class ComandiRicGestPag
    Inherits BasePage

    Private Shared Log As ILog = LogManager.GetLogger(GetType(ComandiRicGestPag))
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
        Dim IdTributo As String = Request.Item("TRIBUTO")
        Try
            If IdTributo = Utility.Costanti.TRIBUTO_OSAP Then
                info.InnerText = "TOSAP/COSAP "
            ElseIf IdTributo = Utility.Costanti.TRIBUTO_SCUOLE Then
                info.InnerText = "SCUOLA "
            Else
                If ConstSession.IsFromTARES = "1" Then
                    info.InnerText = "TARI "
                Else
                    info.InnerText = "TARSU "
                End If
            End If
            If ConstSession.IsFromVariabile(Request.Item("IsFromVariabile")) = "1" Then
                info.InnerText += "Variabile"
            End If
            info.InnerText += " - Pagamenti - Ricerca"
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ComandiRicGestPag.Page_Load.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub

End Class
