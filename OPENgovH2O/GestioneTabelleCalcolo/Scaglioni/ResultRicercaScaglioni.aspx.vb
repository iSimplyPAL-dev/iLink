Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports Utility
Imports log4net
Partial Class ResultRicercaScaglioni
    Inherits BasePage
    Dim objDS As DataSet
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
    Private Shared Log As ILog = LogManager.GetLogger("ResultRicercaScaglioni")
    Private myDvResult() As OggettoScaglione
    Protected FncGrd As New ClsGenerale.FunctionGrd
    ''' <summary>
    ''' Pagina per la visualizzazione degli elementi presenti
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim IDUtenza As String = ""
        Dim Anno As String = ""

        IDUtenza = Server.UrlDecode(Request.Item("IDUtenza"))
        Anno = Request.Item("Anno")
        Try
            If Page.IsPostBack = False Then
                Dim ClsScaglioni As New ClsScaglioni

                Dim objScaglioni As OggettoScaglione()
                Dim objScaglione As New OggettoScaglione
                If IDUtenza <> "" Then
                    objScaglione.idTipoUtenza = IDUtenza
                End If
                If Anno <> "" Then
                    objScaglione.sAnno = Anno
                End If
                objScaglioni = ClsScaglioni.GetScaglioniEnte(objScaglione)
                If Not IsNothing(objScaglioni) Then
                    If objScaglioni.Length > 0 Then
                        Session.Add("objScaglioni", objScaglioni)
                        viewstate("SortKey") = "Anno"
                        viewstate("OrderBy") = TipoOrdinamento.Crescente

                        Dim objComparer As New Comparatore(New String() {"sAnno", "idTipoUtenza", "DA"}, New Boolean() {viewstate("OrderBy"), viewstate("OrderBy"), viewstate("OrderBy")})
                        Array.Sort(objScaglioni, objComparer)

                        GrdScaglioneEnte.DataSource = objScaglioni
                        GrdScaglioneEnte.DataBind()
                    End If
                Else
                    LblResult.Text = "La ricerca non ha prodotto risultati."

                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicercaScaglioni.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
#Region "Griglie"
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim IDRow As String = e.CommandArgument.ToString()
            If e.CommandName = "RowOpen" Then
                For Each myRow As GridViewRow In GrdScaglioneEnte.Rows
                    If IDRow = CType(myRow.FindControl("hfid"), HiddenField).Value Then
                        RegisterScript("DettaglioScaglioni(" & IDRow & "," & CType(myRow.FindControl("hfidTipoUtenza"), HiddenField).Value & "," & myRow.Cells(2).Text & "," & myRow.Cells(3).Text & ",'" & myRow.Cells(4).Text & "','" & myRow.Cells(5).Text & "','" & myRow.Cells(6).Text & "','" & myRow.Cells(0).Text & "')", Me.GetType())
                    End If
                Next
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchGiri.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
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
    'Private Sub GrdScaglioneEnte_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdScaglioneEnte.ItemDataBound
    'Try
    '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '        'Gestione Riga Tabella

    '        Dim IdScaglione, IdTU, Da, A, Tariffa, Minimo, Iva, Anno As String

    '        Dim obj As OggettoScaglione

    '        obj = CType(e.Item.DataItem, OggettoScaglione)

    '        IdScaglione = obj.ID
    '        IdTU = obj.idTipoUtenza
    '        Iva = obj.dAliquota
    '        Minimo = obj.dMinimo
    '        Tariffa = obj.dTariffa
    '        Anno = obj.sAnno
    '        Da = obj.DA
    '        A = obj.A

    '        e.Item.Attributes.Add("onClick", "DettaglioScaglioni(" & IdScaglione & "," & IdTU & "," & Da & "," & A & ",'" & Tariffa & "','" & Minimo & "','" & Iva & "','" & Anno & "')")
    '    End If
    ' Catch ex As Exception
    '     Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicercaScaglioni.GrdScaglioneEnte_ItemDataBound.errore: ", ex)
    '    Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub

    'Private Sub GrdScaglioneEnte_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles GrdScaglioneEnte.SortCommand
    '    Dim strSortKey As String
    '    Dim dt As DataView

    '    Try
    '        If e.SortExpression.ToString() = ViewState("SortKey").ToString() Then
    '            Select Case ViewState("OrderBy")
    '                Case TipoOrdinamento.Crescente
    '                    ViewState("OrderBy") = TipoOrdinamento.Decrescente

    '                Case TipoOrdinamento.Decrescente
    '                    ViewState("OrderBy") = TipoOrdinamento.Crescente
    '            End Select
    '        Else
    '            ViewState("SortKey") = e.SortExpression
    '            ViewState("OrderBy") = TipoOrdinamento.Crescente
    '        End If
    '        myDvResult = CType(Session("objScaglioni"), OggettoScaglione())

    '        If Not IsNothing(myDvResult) Then
    '            ' ORDINO L'ARRAY DI OGGETTI
    '            Dim objComparer As New Comparatore(New String() {e.SortExpression}, New Boolean() {ViewState("OrderBy")})
    '            Array.Sort(myDvResult, objComparer)
    '        End If

    '        GrdScaglioneEnte.DataSource = myDvResult
    '        GrdScaglioneEnte.DataBind()
    '    Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicercaScaglioni.GrdScaglioneEnte_SortCommand.errore: ", Err)
    '    Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdScaglioneEnte.DataSource = CType(Session("objScaglioni"), OggettoScaglione())
            If page.HasValue Then
                GrdScaglioneEnte.PageIndex = page.Value
            End If
            GrdScaglioneEnte.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ResultRicercaScaglioni.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
End Class
