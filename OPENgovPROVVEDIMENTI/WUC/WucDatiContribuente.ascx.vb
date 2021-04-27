Imports AnagInterface
Imports log4net
''' <summary>
''' UserControl per la visualizzazione anagrafica in gestione accertamenti OSAP.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class WucDatiContribuente
    Inherits System.Web.UI.UserControl
    Public oAnagrafica As DettaglioAnagrafica

    Private Shared Log As ILog = LogManager.GetLogger(GetType(WucDatiContribuente))
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
            If Not Page.IsPostBack Then
                BindData()
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.WucDatiContribuente.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub BindData()
        Try
            If (Not (oAnagrafica) Is Nothing) Then
                lblCognome.Text = oAnagrafica.Cognome
                lblNome.Text = oAnagrafica.Nome
                If ((Not (oAnagrafica.DataNascita) Is Nothing) _
                            AndAlso (oAnagrafica.DataNascita.CompareTo("00/00/1900") <> 0)) Then
                    lblDataNascita.Text = oAnagrafica.DataNascita
                Else
                    lblDataNascita.Text = String.Empty
                End If
                lblIndirizzo.Text = (oAnagrafica.ViaResidenza + (", " + oAnagrafica.CivicoResidenza))
                lblComune.Text = (oAnagrafica.ComuneResidenza + (" (" _
                            + (oAnagrafica.ProvinciaResidenza + ")")))
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.WucDatiContribuente.BindData.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="CodContribuente"></param>
    Public Sub LoadAnagrafica(ByVal CodContribuente As Integer)
        Dim objAnagrafica As DAO.AnagraficheDAO = New DAO.AnagraficheDAO
        Me.oAnagrafica = CType(objAnagrafica.GetAnagraficaContribuente(CodContribuente), DettaglioAnagrafica)
    End Sub
End Class
