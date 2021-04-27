Imports log4net
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
''' <summary>
''' Pagina per la visualizzazione del risultato della ricerca avvisi.
''' Contiene la griglia dalla quale è possibile accedere al dettaglio.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class ResultAvvisi
    Inherits BaseEnte
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ResultAvvisi))
    Protected FncGrd As New Formatta.FunctionGrd

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
        If ConstSession.IsFromVariabile(Request.Item("IsFromVariabile")) = "1" Then
        End If
    End Sub

#End Region
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim oListAvvisi() As ObjAvviso
        Dim FncAvvisi As New GestAvviso
        Dim nIdRuolo As Integer = -1

        Try
            If ConstSession.IsFromVariabile(Request.Item("IsFromVariabile")) = "1" Then
            End If
            Session.Remove("BloccoSgravio") : Session.Remove("MyAvvisoRicalcolato")
            If Page.IsPostBack = False Then
                If Request.Item("IdRuolo") <> "" Then
                    nIdRuolo = Request.Item("IdRuolo")
                End If
                oListAvvisi = FncAvvisi.GetAvviso(ConstSession.StringConnection, -1, ConstSession.IdEnte, nIdRuolo, Request.Item("DdlAnno"), Request.Item("DdlTipoRuolo"), Request.Item("DdlNProgRuolo"), Request.Item("TxtCognome"), Request.Item("TxtNome"), Request.Item("TxtCFPIVA"), Request.Item("TxtCodCartella"), "", "", False, False, Request.Item("ChkSgravate"), False, "", -1, Nothing)
                If Not IsNothing(oListAvvisi) Then
                    If oListAvvisi.Length > 0 Then
                        Session.Add("oListAvvisi", oListAvvisi)
                        GrdAvvisi.DataSource = oListAvvisi
                        GrdAvvisi.DataBind()
                    Else
                        LblResult.Text = "La ricerca non ha prodotto risultati."
                        LblResult.Style.Add("display", "")
                    End If
                End If
                Dim mySearchForAvvisi As New ArrayList
                mySearchForAvvisi.Add(Request.Item("DdlAnno"))
                mySearchForAvvisi.Add(Request.Item("DdlTipoRuolo"))
                mySearchForAvvisi.Add(Request.Item("TxtCognome"))
                mySearchForAvvisi.Add(Request.Item("TxtNome"))
                mySearchForAvvisi.Add(Request.Item("TxtCFPIVA"))
                mySearchForAvvisi.Add(Request.Item("TxtCodCartella"))
                mySearchForAvvisi.Add(Request.Item("ChkSgravate"))
                Session("mySearchForAvvisi") = mySearchForAvvisi
            End If
            If ConstSession.IsFromVariabile() = "1" Then
                GrdAvvisi.Columns(6).Visible = True
            Else
                GrdAvvisi.Columns(6).Visible = False
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultAvvisi.Page_Load.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        Finally
            RegisterScript("parent.parent.Visualizza.DivAttesa.style.display='none';", Me.GetType)
            'chiudo la connessione
            'If Not WFSessione Is Nothing Then
            '    WFSessione.Kill()
            'End If
        End Try
    End Sub

#Region "Griglie"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim IDRow As String = e.CommandArgument.ToString()
            If e.CommandName = "RowOpen" Then
                Dim sScript As String = "LoadAvvisi('" & ConstSession.IsFromVariabile & "','" & IDRow & "','" & Request.Item("IdRuolo") & "','" & Utility.Costanti.AZIONE_UPDATE & "')"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultAvvisi.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Gestione del cambio pagina della griglia.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdPageIndexChanging(ByVal sender As Object, e As GridViewPageEventArgs)
        LoadSearch(e.NewPageIndex)
    End Sub
    'Private Sub GrdAvvisi_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAvvisi.ItemDataBound
    'Try
    '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '        e.Item.Attributes.Add("onClick", "LoadAvvisi('" & ConstSession.IsFromVariabile & "','" & e.Item.Cells(12).Text & "','" & Request.Item("IdRuolo") & "','" & Utility.Costanti.AZIONE_UPDATE & "')")
    '    End If
    ' Catch ex As Exception
    '    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultAvvisi.GrdAvvisi_ItemDataBound.errore: ", ex)
    '   Response.Redirect("../../../PaginaErrore.aspx")
    'End Try
    'End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdAvvisi.DataSource = CType(Session("oListAvvisi"), ObjAvviso())
            If page.HasValue Then
                GrdAvvisi.PageIndex = page.Value
            End If
            GrdAvvisi.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultAvvisi.LoadSearch.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
