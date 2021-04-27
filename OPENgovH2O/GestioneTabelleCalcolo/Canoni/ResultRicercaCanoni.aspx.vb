Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports Utility
Imports log4net

Partial Class ResultRicercaCanoni
    Inherits BasePage
    Dim objDS As DataSet
    Protected FncGrd As New ClsGenerale.FunctionGrd

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
    Private Shared Log As ILog = LogManager.GetLogger("ResultRicercaCanoni")
    Private myDvResult() As OggettoCanone
    ''' <summary>
    ''' Pagina per la visualizzazione degli elementi presenti
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            'Put user code to initialize the page here
            Dim IDCanone As String = ""
            Dim ANNO As String = ""
            IDCanone = Server.UrlDecode(Request.Item("IDCanone"))
            ANNO = Request.Item("Anno")

            If Page.IsPostBack = False Then
                Dim ClsCanoni As New ClsCanoni
                'Dim dsCanoni As New DataSet
                Dim objCanoni As OggettoCanone()
                Dim objCanone As New OggettoCanone
                objCanone.sIdEnte = ConstSession.IdEnte
                If IDCanone <> "" Then
                    objCanone.idTipoCanone = IDCanone
                End If
                If ANNO <> "" Then
                    objCanone.sAnno = ANNO
                End If
                objCanoni = ClsCanoni.GetCanoniEnte(objCanone)
                If Not IsNothing(objCanoni) Then
                    If objCanoni.Length > 0 Then
                        Session.Add("objCanoni", objCanoni)
                        ViewState("SortKey") = "Anno"
                        ViewState("OrderBy") = TipoOrdinamento.Decrescente

                        Dim objComparer As New Comparatore(New String() {"sAnno", "idTipoUtenza", "idTipoCanone"}, New Boolean() {ViewState("OrderBy"), ViewState("OrderBy"), ViewState("OrderBy")})
                        Array.Sort(objCanoni, objComparer)

                        GrdCanoneEnte.DataSource = objCanoni
                        GrdCanoneEnte.DataBind()
                    End If
                Else
                    LblResult.Text = "La ricerca non ha prodotto risultati."
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicercaCanoni.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try

    End Sub
#Region "Griglie"
    Protected Sub GrdRowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            If (e.Row.RowType = DataControlRowType.DataRow) Then

                Dim IdCanoneEnte, IdTU, IdTC, Anno, PercentualeSuConsumo, Aliquota, Tariffa As String

                Dim obj As OggettoCanone

                obj = CType(e.Row.DataItem, OggettoCanone)

                IdCanoneEnte = obj.ID
                IdTC = obj.idTipoCanone
                IdTU = obj.idTipoUtenza
                Aliquota = obj.dAliquota
                PercentualeSuConsumo = obj.dPercentualeSuConsumo
                Tariffa = obj.dTariffa
                Anno = obj.sAnno

                e.Row.Attributes.Add("onClick", "DettaglioCanoni(" & IdCanoneEnte & "," & IdTC & "," & IdTU & ",'" & Aliquota & "','" & PercentualeSuConsumo & "','" & Tariffa & "','" & Anno & "')")
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicercaCanoni.GrdRowDataBound.errore: ", ex)
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
    'Private Sub GrdCanoneEnte_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdCanoneEnte.ItemDataBound
    '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '        'Gestione Riga Tabella

    '        Dim IdCanoneEnte, IdTU, IdTC, Anno, PercentualeSuConsumo, Aliquota, Tariffa As String

    '        Dim obj As OggettoCanone

    '        obj = CType(e.Item.DataItem, OggettoCanone)

    '        IdCanoneEnte = obj.ID
    '        IdTC = obj.idTipoCanone
    '        IdTU = obj.idTipoUtenza
    '        Aliquota = obj.dAliquota
    '        PercentualeSuConsumo = obj.dPercentualeSuConsumo
    '        Tariffa = obj.dTariffa
    '        Anno = obj.sAnno

    '        e.Item.Attributes.Add("onClick", "DettaglioCanoni(" & IdCanoneEnte & "," & IdTC & "," & IdTU & ",'" & Aliquota & "','" & PercentualeSuConsumo & "','" & Tariffa & "','" & Anno & "')")
    '    End If
    'End Sub
    'Private Sub GrdCanoneEnte_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles GrdCanoneEnte.SortCommand
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
    '        myDvResult = CType(Session("objCanoni"), OggettoCanone())

    '        If Not IsNothing(myDvResult) Then
    '            ' ORDINO L'ARRAY DI OGGETTI
    '            Dim objComparer As New Comparatore(New String() {e.SortExpression}, New Boolean() {ViewState("OrderBy")})
    '            Array.Sort(myDvResult, objComparer)
    '        End If

    '        GrdCanoneEnte.DataSource = myDvResult
    '        GrdCanoneEnte.DataBind()
    '    Catch Err As Exception
    '         Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicercaCanoni.GrdCanoneEnte_ItemDataBound.errore: ", Err)
    '        
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
            GrdCanoneEnte.DataSource = CType(Session("objCanoni"), OggettoCanone())
            If page.HasValue Then
                GrdCanoneEnte.PageIndex = page.Value
            End If
            GrdCanoneEnte.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ResultRicercaCanoni.GrdCanoneEnte_ItemDataBound.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
End Class
