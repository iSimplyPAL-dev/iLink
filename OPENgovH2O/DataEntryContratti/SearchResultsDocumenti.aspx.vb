Imports log4net

Partial Class SearchResultsDocumenti
    Inherits BasePage
    Private clsDBAccess As New DBAccess.getDBobject
    Private ModDate As New ClsGenerale.Generale

    Private Shared Log As ILog = LogManager.GetLogger(GetType(SearchResultsDocumenti))

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

    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    'Put user code to initialize the page here
    '    'Response.Write("<script language='javascript' type='text/javascript'>GestAlert('a', 'warning', '', '', 'Il codice del contratto è " & Request.Params("codcontratto") & "');")
    '    Dim lngRecordCount As Integer
    '    Dim DEContratti As GestContratti = New GestContratti
    '    Dim GetLista As objDBListSQL = DEContRATTI.getListaDocumenti(Request.Params("codcontratto"))

    '    lngRecordCount = GetLista.RecordCount
    'Try
    '    Select Case lngRecordCount
    '        Case 0
    '            lblMessage.Text = "Non sono stati trovati documenti relativi a contratti o preventivi per questo utente.<br><br><br>Chiusura finestra in corso..."
    '            'Response.Write("<script language='javascript' type='text/javascript'>window.opener.messaggio();")
    '            Response.Write("<script language='javascript' type='text/javascript'>window.setTimeout('chiudifinestra()', 4500);")
    '            'Response.Write("<script language='javascript' type='text/javascript'>window.setTimeout('chiudifinestra()',4000);")
    '        Case Is > 0

    '            GrdDocumenti.cnnConn = GetLista.oConn
    '            GrdDocumenti.sSQL= GetLista.Query
    '            GrdDocumenti.strSqlCountRecord = GetLista.QueryCount
    '            'GrdDocumenti.DataKeyField = "CODCONTRATTO"
    '            'Carico il Controllo Nella Tabella

    '            If Not Page.IsPostBack Then
    '                GrdDocumenti.Rows.Count = 0
    '                GrdDocumenti.BindData()
    '            End If
    '            GrdDocumenti.MouseSelectableDataGrid()
    'Catch ex As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchResultsDocumenti.Page_Load.errore: ", ex)
    '     Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    '    End Select

    'End Sub
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim DEContratti As New GestContratti
        Dim dv As DataView = DEContratti.getListaDocumenti(Request.Params("codcontratto"))
        Try
            If Not IsNothing(dv) Then
                If dv.Count <= 0 Then
                    lblMessage.Text = "Non sono stati trovati documenti relativi a contratti o preventivi per questo utente.<br><br><br>Chiusura finestra in corso..."
                    Response.Write("<script language='javascript' type='text/javascript'>window.setTimeout('chiudifinestra()', 4500);")
                    GrdDocumenti.Visible = False
                Else
                    GrdDocumenti.Visible = True
                    GrdDocumenti.DataSource = dv
                    GrdDocumenti.DataBind()
                End If
                Session("dvresultdocumenticontratto") = dv
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchResultsDocumenti.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

#Region "Griglie"
    Protected Sub GrdRowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                e.Row.Cells(6).Attributes.Add("Onclick", "ApriDocumento('" & CType(e.Row.FindControl("hfid"), HiddenField).Value & "');")
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchResultsDocumenti.GrdRowDataBound.errore: ", ex)
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
    'Private Sub GrdDocumenti_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdDocumenti.ItemDataBound
    '    'Response.Write("<script language='javascript' type='text/javascript'>GestAlert('a', 'warning', '', '', '" & e.Item.Cells(1).Text & "');")

    'Try
    '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

    '        e.Item.Cells(1).Attributes.Add("Onclick", "ApriDocumento('" & e.Item.Cells(6).Text & "');")
    '        e.Item.Cells(2).Attributes.Add("Onclick", "ApriDocumento('" & e.Item.Cells(6).Text & "');")
    '        e.Item.Cells(3).Attributes.Add("Onclick", "ApriDocumento('" & e.Item.Cells(6).Text & "');")
    '        e.Item.Cells(4).Attributes.Add("Onclick", "ApriDocumento('" & e.Item.Cells(6).Text & "');")
    '        e.Item.Cells(5).Attributes.Add("Onclick", "ApriDocumento('" & e.Item.Cells(6).Text & "');")
    '        e.Item.Cells(6).Attributes.Add("Onclick", "ApriDocumento('" & e.Item.Cells(6).Text & "');")
    '        e.Item.Cells(7).Attributes.Add("Onclick", "ApriDocumento('" & e.Item.Cells(6).Text & "');")

    '        '    e.Item.Cells(8).Attributes.Add("Onclick", "ApriDocumenti(" & e.Item.Cells(1).Text & ");")
    '        '    e.Item.Cells(8).ToolTip = "Ricerca Documenti"
    '        'e.Item.Cells(1).Style.Add("cursor", "hand")
    '        'e.Item.Cells(2).Style.Add("cursor", "hand")
    '        'e.Item.Cells(4).Style.Add("cursor", "hand")
    '    End If
    '  Catch ex As Exception
    '  Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchResultsDocumenti.GrdDocumenti_ItemDataBound.errore: ", ex)
    '  Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub


    'Private Sub GrdDocumenti_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles GrdDocumenti.SortCommand


    '    'Dim strSortKey As String
    'Try
    '    'If e.SortExpression.ToString() = viewstate("SortKey").ToString() Then
    '    '    Select Case viewstate("OrderBy").ToString()
    '    '        Case "ASC"
    '    '            viewstate("OrderBy") = "DESC"

    '    '        Case "DESC"
    '    '            viewstate("OrderBy") = "ASC"
    '    '    End Select
    '    'Else
    '    '    viewstate("SortKey") = e.SortExpression
    '    '    viewstate("OrderBy") = "ASC"
    '    'End If

    '    'Dim objAnagrafica As ANAGRAFICAWEB.AnagraficaDB = New ANAGRAFICAWEB.AnagraficaDB(e.SortExpression & " " & CType(ViewState("OrderBy"), String))
    '    'Dim ListAnagrafica As ANAGRAFICAWEB.ListAnagrafica = objAnagrafica.GetListAnagragrafica(strCognome, strNome, strCodiceFiscale, strPartitaIva, strCod_Ente, strCODContribuente, blnDaRicontrollare)


    '    'GrdDocumenti.start_index = 0
    '    'GrdDocumenti.setDataAdapter(ListAnagrafica.p_daItemsANAGRAFICA, Session("Anagrafica"))
    '    'GrdDocumenti.DataBind()
    '  Catch ex As Exception
    '  Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchResultsDocumenti.GrdDocumenti_SortCommand.errore: ", ex)
    '  Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub
#End Region

    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdDocumenti.DataSource = CType(Session("dvresultdocumenticontratto"), DataView)
            If page.HasValue Then
                GrdDocumenti.PageIndex = page.Value
            End If
            GrdDocumenti.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchResultsDocumenti.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
End Class
