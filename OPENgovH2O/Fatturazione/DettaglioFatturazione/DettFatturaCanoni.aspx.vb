Imports log4net
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti

Partial Class DettFatturaCanoni
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(DettFatturaCanoni))

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
        Dim oListDettFatturaCanoni() As ObjTariffeCanone
        Try
            If Page.IsPostBack = False Then
                If Not Session("oDettFatturaCanoni") Is Nothing Then
                    oListDettFatturaCanoni = CType(Session("oDettFatturaCanoni"), ObjTariffeCanone())

                    GrdCanoni.DataSource = oListDettFatturaCanoni
                    GrdCanoni.DataBind()
                    GrdCanoni.SelectedIndex = -1
                    LblResultCanoni.Style.Add("display", "none")
                Else
                    LblResultCanoni.Style.Add("display", "")
                End If
            Else
                oListDettFatturaCanoni = CType(Session("oDettFatturaCanoni"), ObjTariffeCanone())
                GrdCanoni.DataSource = oListDettFatturaCanoni
                GrdCanoni.DataBind()
            End If
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DettFatturaCanoni.Page_Load.errore: ", Err)

            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub




#Region "Griglie"

    'aggiunta per test
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim IDRow As String = e.CommandArgument.ToString()
            If e.CommandName = "RowOpen" Then
                For Each myRow As GridViewRow In GrdCanoni.Rows

                    If IDRow = CType(myRow.FindControl("hfId"), HiddenField).Value Then

                    End If
                Next
            ElseIf e.CommandName = "RowDelete" Then

            End If
        Catch ex As Exception
            Log.Debug(".GrdRowCommand::errore::", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub




    Protected Sub GrdRowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            If (e.Row.RowType = DataControlRowType.DataRow) Then

            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicFatturazione.GrdRowDataBound.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub


    ''' <summary>
    ''' Gestione del cambio pagina della griglia.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdPageIndexChanging(ByVal sender As Object, e As GridViewPageEventArgs)
        'LoadSearch(e.NewPageIndex)
    End Sub

#End Region
End Class
