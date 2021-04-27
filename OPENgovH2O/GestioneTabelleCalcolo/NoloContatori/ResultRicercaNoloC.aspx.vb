Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports Utility
Imports log4net

Partial Class ResultRicercaNoloC
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
    Private Shared Log As ILog = LogManager.GetLogger("ResultRicercaNoloC")
    Private myDvResult() As OggettoNoloContatore
    Protected FncGrd As New ClsGenerale.FunctionGrd
    ''' <summary>
    ''' Pagina per la visualizzazione degli elementi presenti
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim ID As String = ""
        Dim ANNO As String = ""
        ID = Server.UrlDecode(Request.Item("ID"))
        ANNO = Request.Item("Anno")

        Try
            If Page.IsPostBack = False Then
                Dim ClsNoliContatore As New ClsNoliContatore
                'Dim dsCanoni As New DataSet
                Dim objNoli As OggettoNoloContatore()
                Dim objNoloC As New OggettoNoloContatore
                If ID <> "" Then
                    objNoloC.ID = ID
                End If
                If ANNO <> "" Then
                    objNoloC.sAnno = ANNO
                End If
                objNoli = ClsNoliContatore.GetNoliContatoreEnte(objNoloC)
                If Not IsNothing(objNoli) Then
                    If objNoli.Length > 0 Then
                        Session.Add("objNoli", objNoli)
                        viewstate("SortKey") = "Anno"
                        viewstate("OrderBy") = TipoOrdinamento.Crescente

                        Dim objComparer As New Comparatore(New String() {"sAnno", "idTipoContatore"}, New Boolean() {viewstate("OrderBy"), viewstate("OrderBy")})
                        Array.Sort(objNoli, objComparer)

                        GrdNoliEnte.DataSource = objNoli
                        GrdNoliEnte.DataBind()
                    End If
                Else
                    LblResult.Text = "La ricerca non ha prodotto risultati."
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicercaNoloC.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
#Region "Griglie"
    Protected Sub GrdRowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                Dim IdNoloEnte, IdTContatore, Anno, Aliquota, Tariffa, IsUnaTantum As String

                Dim obj As OggettoNoloContatore

                obj = CType(e.Row.DataItem, OggettoNoloContatore)

                IdNoloEnte = obj.ID
                IdTContatore = obj.idTipoContatore
                Aliquota = obj.dAliquota
                Tariffa = obj.dImporto
                Anno = obj.sAnno
                IsUnaTantum = obj.bIsUnaTantum

                e.Row.Attributes.Add("onClick", "DettaglioNolo(" & IdNoloEnte & "," & IdTContatore & ",'" & Aliquota & "','" & Tariffa & "','" & Anno & "','" & IsUnaTantum & "')")
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicercaNoloC.GrdRowDataBound.errore: ", ex)
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
    'Private Sub GrdNoliEnte_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdNoliEnte.ItemDataBound
    '    Try
    '        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '            'Gestione Riga Tabella

    '            Dim IdNoloEnte, IdTContatore, Anno, Aliquota, Tariffa, IsUnaTantum As String

    '            Dim obj As OggettoNoloContatore

    '            obj = CType(e.Item.DataItem, OggettoNoloContatore)

    '            IdNoloEnte = obj.ID
    '            IdTContatore = obj.idTipoContatore
    '            Aliquota = obj.dAliquota
    '            Tariffa = obj.dImporto
    '            Anno = obj.sAnno
    '            IsUnaTantum = obj.bIsUnaTantum

    '            e.Item.Attributes.Add("onClick", "DettaglioNolo(" & IdNoloEnte & "," & IdTContatore & ",'" & Aliquota & "','" & Tariffa & "','" & Anno & "','" & IsUnaTantum & "')")
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicercaNoloC.GrdNoliEnte_ItemDataBound.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Private Sub GrdNoliEnte_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles GrdNoliEnte.SortCommand
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
    '        myDvResult = CType(Session("objNoli"), OggettoNoloContatore())

    '        If Not IsNothing(myDvResult) Then
    '            ' ORDINO L'ARRAY DI OGGETTI
    '            Dim objComparer As New Comparatore(New String() {e.SortExpression}, New Boolean() {ViewState("OrderBy")})
    '            Array.Sort(myDvResult, objComparer)
    '        End If

    '        GrdNoliEnte.DataSource = myDvResult
    '        GrdNoliEnte.DataBind()
    '    Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicercaNoloC.GrdNoliEnte_SortCommand.errore: ", Err)
    '       Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdNoliEnte.DataSource = CType(Session("objNoli"), OggettoNoloContatore())
            If page.HasValue Then
                GrdNoliEnte.PageIndex = page.Value
            End If
            GrdNoliEnte.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ResultRicercaNoloC.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
End Class
