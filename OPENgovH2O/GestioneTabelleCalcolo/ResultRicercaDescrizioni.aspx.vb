Imports log4net

Partial Class ResultRicercaDescrizioni
    Inherits BasePage
    Private CODICE As String = ""
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Private TABELLA As String = ""

    Private Shared Log As ILog = LogManager.GetLogger(GetType(ResultRicercaDescrizioni))

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
    ''' Pagina per la visualizzazione degli elementi presenti
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            If Not Request.Item("Codice") Is Nothing Then
            CODICE = Request.Item("Codice")
        Else
            CODICE = ""
        End If
        TABELLA = Request.Item("Tabella")
            If Page.IsPostBack = False Then
                Dim ClsGenerale As New ClsGenerale.Generale
                Dim dsDescrizioni As New DataSet
                'dsCategorie = clSelectCategorie.GetCategorie(CODICE_CATEGORIA, "", "", "")

                dsDescrizioni = ClsGenerale.GetDescrizioni(CODICE, "", TABELLA)
                If Not dsDescrizioni Is Nothing Then
                    If dsDescrizioni.Tables.Item(0).Rows.Count > 0 Then
                        'ViewState.Add("dsCATEGORIE", dsCategorie)
                        'DGCategorie.DataSource = dsCategorie
                        'DGCategorie.DataBind()
                        '*********************************'
                        ViewState.Add("SortKey", "DESCRIZIONE")
                        ViewState.Add("OrderBy", "ASC")
                        '*********************************'

                        ViewState.Add("dsDescrizioni", dsDescrizioni)
                        'DGRidDet.DataSource = dsRidDet
                        'DGRidDet.DataBind()
                        Dim TabellaSort As DataSet
                        TabellaSort = CType(dsDescrizioni, DataSet)
                        TabellaSort.Tables(0).DefaultView.Sort = ViewState("SortKey") + " " + ViewState("OrderBy")

                        Session("dsDescrizioni") = TabellaSort
                        GrdAddizionali.DataSource = TabellaSort
                        GrdAddizionali.DataBind()

                        LblResult.Text = "Risultati della Ricerca"
                    Else
                        LblResult.Text = "La ricerca non ha prodotto risultati."
                    End If
                Else
                    LblResult.Text = "La ricerca non ha prodotto risultati."
                End If
            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicercaDescrizioni.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
#Region "Griglie"
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim IDRow As String = e.CommandArgument.ToString()
            If e.CommandName = "RowOpen" Then
                For Each myRow As GridViewRow In GrdAddizionali.Rows
                    If IDRow = CType(myRow.FindControl("hfid"), HiddenField).Value Then
                        RegisterScript("DettaglioDescrizione('" & IDRow & "','" & myRow.Cells(2).Text & "','" & CType(myRow.FindControl("hfIDSERVIZIO"), HiddenField).Value & "','" & TABELLA & "')", Me.GetType())
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
    'Private Sub GrdAddizionali_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAddizionali.ItemDataBound
    ' Try
    'If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '        'Gestione Riga Tabella
    '        Dim Codice, Descrizione As String
    '        Codice = e.Item.DataItem.row.itemarray(0)
    '        Descrizione = e.Item.DataItem.row.itemarray(1)
    '        '*** 20141125 - Componente aggiuntiva sui consumi ***
    '        'e.Item.Attributes.Add("onClick", "DettaglioDescrizione('" & Codice.Replace("'", "\'") & "','" & Descrizione.Replace("'", "\'") & "','" & TABELLA & "')")
    '        e.Item.Attributes.Add("onClick", "DettaglioDescrizione('" & Codice.Replace("'", "\'") & "','" & Descrizione.Replace("'", "\'") & "','" & e.Item.DataItem.row.itemarray(2) & "','" & TABELLA & "')")
    '        '*** ***
    '    End If
    'End Sub
    'Private Sub GrdAddizionali_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles GrdAddizionali.SortCommand
    '    If (e.SortExpression.ToString().CompareTo(ViewState("SortKey").ToString()) = 0) Then
    '        Select Case (ViewState("OrderBy").ToString())
    '            Case "ASC"
    '                ViewState("OrderBy") = "DESC"
    '            Case "DESC"
    '                ViewState("OrderBy") = "ASC"
    '        End Select
    '    Else
    '        ViewState("SortKey") = e.SortExpression.ToString()
    '        ViewState("OrderBy") = "ASC"
    '    End If

    '    ''Dim TabellaSort As DataTable
    '    Dim TabellaSort As DataSet
    '    ''TabellaSort = (DataTable) Session("TABELLA_RICERCA");
    '    TabellaSort = CType(HttpContext.Current.Session("dsDescrizioni"), DataSet)
    '    ''((DataTable)(Session["TABELLA_RICERCA"])).DefaultView.Sort = ViewState["SortKey"] + " " + ViewState["OrderBy"]
    '    TabellaSort.Tables(0).DefaultView.Sort = ViewState("SortKey") + " " + ViewState("OrderBy")

    '    Session("dsDescrizioni") = TabellaSort

    '    GrdAddizionali.DataSource = TabellaSort
    '    GrdAddizionali.DataBind()
    ' Catch ex As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicercaDescrizioni.GrdAddizionali_ItemDataBound.errore: ", ex)
    '       Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdAddizionali.DataSource = CType(Session("dsDescrizioni"), DataView)
            If page.HasValue Then
                GrdAddizionali.PageIndex = page.Value
            End If
            GrdAddizionali.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicercaDescrizioni.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
End Class
