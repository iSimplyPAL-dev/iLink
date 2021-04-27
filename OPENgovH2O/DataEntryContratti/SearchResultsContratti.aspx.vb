Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports AnagInterface
Imports System.Web.UI.WebControls
Imports OggettiComuniStrade
Imports Utility
Imports log4net

Partial Class SearchResultContratti
    Inherits BasePage
    Protected FncGrd As New ClsGenerale.FunctionGrd
    Private clsDBAccess As New DBAccess.getDBobject
    Private ModDate As New ClsGenerale.Generale
    'Private oAnagrafica As New Anagrafica.DLL.GestioneAnagrafica()
    Private oDettaglioAnagraficaUtente As New DettaglioAnagrafica
    Dim strUtente, strIntestatario, Ubicazione, nomeIntestatario, nomeUtente, codContratto As String
    Dim lngRecordCount, intIDVia As Integer
    Dim stato As Integer
    Private Shared Log As ILog = LogManager.GetLogger(GetType(SearchResultContratti))

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Label12 As System.Web.UI.WebControls.Label
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label

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
        Dim sScript As String = ""
        Try
            strIntestatario = RTrim(LTrim(Request("intestatario")))
            strUtente = RTrim(LTrim(Request("utente")))
            nomeIntestatario = RTrim(LTrim(Request("NomeIntestatario")))
            nomeUtente = RTrim(LTrim(Request("NomeUtente")))
            If Len(strIntestatario) > 0 Then
                strIntestatario = Replace(strIntestatario, "'", "''")
            End If
            If Len(strUtente) > 0 Then
                strUtente = Replace(strUtente, "'", "''")
            End If
            If Len(nomeIntestatario) > 0 Then
                nomeIntestatario = Replace(nomeIntestatario, "'", "''")
            End If
            If Len(nomeUtente) > 0 Then
                nomeUtente = Replace(nomeUtente, "'", "''")
            End If

            intIDVia = stringoperation.formatint(request("ubicazione"))

            stato = Request.Params("stato")
            codContratto = Request("codcontratto")

            Dim DEContRATTI As GestContratti = New GestContratti
            Dim GetLista As DataSet = DEContRATTI.GetListaContratti(ConstSession.IdEnte, strUtente, strIntestatario, codContratto, intIDVia, stato, nomeIntestatario, nomeUtente)
            Session.Remove("DS_ANA")
            Session("DS_ANA") = GetLista
            Dim dv As New DataView

            If Not Page.IsPostBack Then
                If Not IsNothing(GetLista) Then
                    ViewState("SortKey") = "cognomeint, cognomeut"
                    ViewState("OrderBy") = "ASC"
                    dv = GetLista.Tables(0).DefaultView
                    dv.Sort = ViewState("SortKey") & " " & ViewState("OrderBy")
                End If

                If dv.Count <= 0 Then
                    GrdContratti.Visible = False
                    lblMessage.Text = "La ricerca non ha prodotto risultati"
                    lblMessage.Visible = True
                Else
                    GrdContratti.Visible = True
                    Session("vistaAna") = dv
                    GrdContratti.DataSource = dv
                    GrdContratti.DataBind()
                    Session("gridsource2") = dv
                End If
                sScript = "parent.parent.Visualizza.DivAttesa.style.display='none';"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            lblMessage.Text = ex.Message
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchResultContratti.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

#Region "Griglie"

    Protected Sub GrdRowCommand(ByVal sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim sScript As String
            Dim IDRow As Integer = CInt(e.CommandArgument.ToString())
            If e.CommandName = "RowOpen" Then
                '*** 201511 - Funzioni Sovracomunali ***'*** 20140923 - GIS ***
                sScript = "ApriContratto(" & IDRow & ",'');"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultRicDichiarazione.GrdRowCommand.errore: ", ex)
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
    'Private Sub GrdContratti_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdContratti.ItemDataBound
    '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '        e.Item.Cells(0).Attributes.Add("Onclick", "ApriContratto(" & e.Item.Cells(9).Text & ",'" & e.Item.Cells(10).Text & "');")
    '        e.Item.Cells(1).Attributes.Add("Onclick", "ApriContratto(" & e.Item.Cells(9).Text & ",'" & e.Item.Cells(10).Text & "');")
    '        e.Item.Cells(2).Attributes.Add("Onclick", "ApriContratto(" & e.Item.Cells(9).Text & ",'" & e.Item.Cells(10).Text & "');")
    '        e.Item.Cells(3).Attributes.Add("Onclick", "ApriContratto(" & e.Item.Cells(9).Text & ",'" & e.Item.Cells(10).Text & "');")
    '        e.Item.Cells(4).Attributes.Add("Onclick", "ApriContratto(" & e.Item.Cells(9).Text & ",'" & e.Item.Cells(10).Text & "');")
    '        e.Item.Cells(5).Attributes.Add("Onclick", "ApriContratto(" & e.Item.Cells(9).Text & ",'" & e.Item.Cells(10).Text & "');")
    '        e.Item.Cells(6).Attributes.Add("Onclick", "ApriDocumenti(" & e.Item.Cells(9).Text & ",'" & e.Item.Cells(7).Text & "','" & e.Item.Cells(8).Text & "');")
    '        e.Item.Cells(6).ToolTip = "Ricerca Documenti"
    '    End If
    'End Sub
    'Private Sub GrdContratti_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles GrdContratti.SortCommand
    '    Dim dt As DataView

    '    Try
    '        If e.SortExpression.ToString().ToUpper() = ViewState("SortKey").ToString().ToUpper() Then
    '            Select Case ViewState("OrderBy").ToString()
    '                Case "ASC"
    '                    ViewState("OrderBy") = "DESC"

    '                Case "DESC"
    '                    ViewState("OrderBy") = "ASC"
    '            End Select
    '        Else
    '            ViewState("SortKey") = e.SortExpression
    '            ViewState("OrderBy") = "ASC"
    '        End If

    '        dt = Session("GridSource2")
    '        dt.Sort = ViewState("SortKey") & " " & ViewState("OrderBy")

    '        Session("GridSource2") = dt
    '        'GrigliaStorico.start_index = 0
    '        GrdContratti.DataSource = dt
    '        GrdContratti.DataBind()

    '    Catch ex As Exception
    ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchResultContratti.GrdContatti_ItemDataBound.errore: ", ex)
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
            GrdContratti.DataSource = CType(Session("vistaAna"), DataView)
            If page.HasValue Then
                GrdContratti.PageIndex = page.Value
            End If
            GrdContratti.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.SearchResultContratti.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
End Class
