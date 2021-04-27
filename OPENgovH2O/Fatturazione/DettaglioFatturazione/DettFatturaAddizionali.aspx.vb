Imports log4net
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti

Partial Class DettFatturaAddizionali
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(DettFatturaAddizionali))

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
        Dim oListDettFatturaAddizionali() As ObjTariffeAddizionale
        Try
            If Page.IsPostBack = False Then
                If Not Session("oDettFatturaAddizionali") Is Nothing Then
                    oListDettFatturaAddizionali = CType(Session("oDettFatturaAddizionali"), ObjTariffeAddizionale())

                    GrdAddizionali.DataSource = oListDettFatturaAddizionali
                    GrdAddizionali.DataBind()
                    GrdAddizionali.SelectedIndex = -1
                    LblResultAddizionali.Style.Add("display", "none")
                Else
                    LblResultAddizionali.Style.Add("display", "")
                End If
            Else
                oListDettFatturaAddizionali = CType(Session("oDettFatturaAddizionali"), ObjTariffeAddizionale())
                GrdAddizionali.DataSource = oListDettFatturaAddizionali
                GrdAddizionali.DataBind()
            End If
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DettFatturaAddizionali.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")

        End Try
    End Sub
End Class
