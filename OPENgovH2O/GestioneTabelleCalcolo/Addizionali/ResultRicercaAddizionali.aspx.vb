Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports Utility
Imports log4net

Partial Class ResultRicercaAddizionali
    Inherits BasePage
    Private myDvResult() As OggettoAddizionaleEnte
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
    Private Shared Log As ILog = LogManager.GetLogger("ResultRicercaAddizionali")
    ''' <summary>
    ''' Pagina per la visualizzazione degli elementi presenti
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim IDADDIZIONALE As String = ""
        Dim ANNO As String = ""
        IDADDIZIONALE = Server.UrlDecode(Request.Item("IdAddizionale"))
        ANNO = Request.Item("Anno")
        Try
            If Page.IsPostBack = False Then
                Dim ClsAddizionali As New ClsAddizionali
                Dim ObjAddizionali As OggettoAddizionaleEnte()
                Dim objAdd As New OggettoAddizionaleEnte

                If IDADDIZIONALE <> "" Then
                    objAdd.IDaddizionale = IDADDIZIONALE
                End If
                If ANNO <> "" Then
                    objAdd.sAnno = ANNO
                End If

                ObjAddizionali = ClsAddizionali.GetAddizionaliEnte(objAdd)
                If Not IsNothing(ObjAddizionali) Then
                    If ObjAddizionali.Length > 0 Then
                        Session.Add("ObjAddizionali", ObjAddizionali)
                        viewstate("SortKey") = "Anno"
                        viewstate("OrderBy") = TipoOrdinamento.Crescente

                        Dim objComparer As New Comparatore(New String() {"sAnno", "IDaddizionale"}, New Boolean() {viewstate("OrderBy"), viewstate("OrderBy")})
                        Array.Sort(ObjAddizionali, objComparer)

                        GrdAddizionaliEnte.DataSource = ObjAddizionali
                        GrdAddizionaliEnte.DataBind()
                    End If
                Else
                    LblResult.Text = "La ricerca non ha prodotto risultati."

                End If
            End If

            '    If dsAddizionali.Tables.Item(0).Rows.Count > 0 Then
            '        ViewState.Add("dsAddizionaliEnte", dsAddizionali)
            '        ViewState.Add("SortKey", "ANNO")
            '        ViewState.Add("OrderBy", "ASC")

            '        Dim TabellaSort As DataSet

            '        TabellaSort = CType(HttpContext.Current.Session("dsAddizionaliEnte"), DataSet)
            '        TabellaSort.Tables(0).DefaultView.Sort = ViewState("SortKey") + " " + ViewState("OrderBy")

            '        Session("dsAddizionaliEnte") = TabellaSort

            '        GrdAddizionaliEnte.start_index = Convert.ToString(GrdAddizionaliEnte.CurrentPageIndex)
            '        GrdAddizionaliEnte.AllowCustomPaging = False

            '        GrdAddizionaliEnte.DataSource = TabellaSort
            '        GrdAddizionaliEnte.DataBind()

            '        LblResult.Text = "Risultati della Ricerca"
            '    Else
            '        LblResult.Text = "La ricerca non ha prodotto risultati."
            '    End If
            'Else

            '    'popolo il dataset per l'ordinamento
            '    Dim TabellaSort As DataSet

            '    TabellaSort = CType(HttpContext.Current.Session("dsAddizionaliEnte"), DataSet)
            '    TabellaSort.Tables(0).DefaultView.Sort = ViewState("SortKey") + " " + ViewState("OrderBy")

            '    Session("dsAddizionaliEnte") = TabellaSort

            '    GrdAddizionaliEnte.start_index = Convert.ToString(GrdAddizionaliEnte.CurrentPageIndex)
            '    GrdAddizionaliEnte.AllowCustomPaging = False

            '    GrdAddizionaliEnte.DataSource = TabellaSort

            '    GrdAddizionaliEnte.DataBind()
            'End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicercaAddizionali.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
#Region "Griglie"
    Protected Sub GrdRowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                Dim IdAddizionaleEnte, IdAddizionale, Tariffa, Anno, Iva As String

                Dim obj As OggettoAddizionaleEnte

                obj = CType(e.Row.DataItem, OggettoAddizionaleEnte)

                IdAddizionaleEnte = obj.ID
                IdAddizionale = obj.IDaddizionale
                Iva = obj.Aliquota
                Tariffa = obj.dImporto
                Anno = obj.sAnno
                e.Row.Attributes.Add("onClick", "DettaglioAddizionali('" & IdAddizionaleEnte.Replace("'", "\'") & "','" & IdAddizionale.Replace("'", "\'") & "','" & Tariffa & "','" & Anno & "','" & Iva.Replace("'", "\'") & "')")
            End If
        Catch ex As Exception
            Log.Debug(".GrdRowDataBound::errore::", ex)
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
    'Private Sub GrdAddizionaliEnte_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAddizionaliEnte.ItemDataBound
    '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '        'Gestione Riga Tabella

    '        Dim IdAddizionaleEnte, IdAddizionale, Tariffa, Anno, Iva As String

    '        Dim obj As OggettoAddizionaleEnte

    '        obj = CType(e.Item.DataItem, OggettoAddizionaleEnte)

    '        IdAddizionaleEnte = obj.ID
    '        IdAddizionale = obj.IDaddizionale
    '        Iva = obj.Aliquota
    '        Tariffa = obj.dImporto
    '        Anno = obj.sAnno
    '        e.Item.Attributes.Add("onClick", "DettaglioAddizionali('" & IdAddizionaleEnte.Replace("'", "\'") & "','" & IdAddizionale.Replace("'", "\'") & "','" & Tariffa & "','" & Anno & "','" & Iva.Replace("'", "\'") & "')")
    '    End If
    'End Sub
    'Private Sub GrdAddizionaliEnte_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles GrdAddizionaliEnte.SortCommand
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
    '        myDvResult = CType(Session("ObjAddizionali"), OggettoAddizionaleEnte())

    '        If Not IsNothing(myDvResult) Then
    '            ' ORDINO L'ARRAY DI OGGETTI
    '            Dim objComparer As New Comparatore(New String() {e.SortExpression}, New Boolean() {ViewState("OrderBy")})
    '            Array.Sort(myDvResult, objComparer)
    '        End If

    '        GrdAddizionaliEnte.DataSource = myDvResult
    '        GrdAddizionaliEnte.DataBind()
    '    Catch Err As Exception
    '        Log.Debug("Si è verificato un errore in ResultRicercaAddizionaliEnte::GrdAddizionaliEnte_SortCommand::" & Err.Message)
    '        Log.Warn("Si è verificato un errore in ResultRicercaAddizionaliEnte::GrdAddizionaliEnte_SortCommand::" & Err.Message)
    '        'Response.Redirect("../../PaginaErrore.aspx")
    '    End Try

    'End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdAddizionaliEnte.DataSource = CType(Session("ObjAddizionali"), OggettoAddizionaleEnte())
            If page.HasValue Then
                GrdAddizionaliEnte.PageIndex = page.Value
            End If
            GrdAddizionaliEnte.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicercaAddizionali.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub

End Class
