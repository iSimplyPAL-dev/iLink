Imports log4net
''' <summary>
''' Pagina per la consultazione del dettaglio della posizione residente.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class DatiAnagrafeResidenti
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(DatiAnagrafeResidenti))

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
        Try
            If Not Request.Item("codfiscale") Is Nothing Then
                txtCodiceFiscale.Text = Request.Item("codfiscale")
            End If
            If Not Request.Item("cognome") Is Nothing Then
                txtCognome.Text = Request.Item("cognome")
            End If
            If Not Request.Item("nome") Is Nothing Then
                txtNome.Text = Request.Item("nome")
            End If
            If Not Request.Item("indirizzo") Is Nothing Then
                txtIndirizzo.Text = Request.Item("indirizzo")
            End If
            If Not Request.Item("datanascita") Is Nothing And Request.Item("datanascita") <> "" Then
                txtDataNascita.Text = Left(Request.Item("datanascita"), 2) + "/" + Mid(Request.Item("datanascita"), 3, 2) + "/" + Right(Request.Item("datanascita"), 4)
            End If
            If Not Request.Item("dataMorte") Is Nothing And Request.Item("dataMorte") <> "" And Request.Item("dataMorte") <> "31129999" Then
                txtDataMorte.Text = Left(Request.Item("dataMorte"), 2) + "/" + Mid(Request.Item("dataMorte"), 3, 2) + "/" + Right(Request.Item("dataMorte"), 4)
            End If

            If Not Request.Item("indirizzo") Is Nothing Then
                txtLuogoNascita.Text = Request.Item("luogonascita")
            End If
            If Not Request.Item("nfamiglia") Is Nothing Then
                txtNfamiglia.Text = Request.Item("nfamiglia")
            End If
            If Not Request.Item("posizione") Is Nothing Then
                txtPosizione.Text = Request.Item("posizione")
            End If
            If Not Request.Item("sesso") Is Nothing Then
                txtSesso.Text = Request.Item("sesso")
            End If
            If Not Request.Item("codvia") Is Nothing Then
                txtcodvia.Text = Request.Item("codvia")
            End If

            If Not Request.Item("azione") Is Nothing Then
                LblAzione.Text = Request.Item("azione")
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte + "." + COSTANTValue.ConstSession.UserName + " - OPENgov.DatiAnagrafeResidenti.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
