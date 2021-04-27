Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports Utility
Imports log4net

Partial Class ResultRicercaQuotaFissa
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
    Private Shared Log As ILog = LogManager.GetLogger("ResultRicercaQuotaFissa")
    Protected FncGrd As New ClsGenerale.FunctionGrd
    Private myDvResult() As OggettoQuotaFissa

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim IDUtenza As String = ""
        Dim Anno As String = ""
        'Dim TipoCanone As String = ""

        IDUtenza = Server.UrlDecode(Request.Item("IDUtenza"))
        Anno = Request.Item("Anno")
        'TipoCanone = Request.Item("TipoCanone")
        Try
            If Page.IsPostBack = False Then
                Dim ClsQuotaFissa As New ClsQuotaFissa
                Dim objQuote As OggettoQuotaFissa()
                Dim objQuotaFissa As New OggettoQuotaFissa

                If IDUtenza <> "" Then
                    objQuotaFissa.idTipoUtenza = IDUtenza
                End If
                If Anno <> "" Then
                    objQuotaFissa.sAnno = Anno
                End If
                'If TipoCanone <> "" Then
                'objQuotaFissa.TipoCanone = TipoCanone
                'End If
                '*** 20121212 riclassifica contatori ***
                'objQuotaFissa.idtipocanone = Request.Item("TipoCanone")
                '*** ***
                objQuote = ClsQuotaFissa.GetQuotaFissaEnte(objQuotaFissa)
                If Not IsNothing(objQuote) Then
                    If objQuote.Length > 0 Then
                        Session.Add("objQuote", objQuote)
                        viewstate("SortKey") = "Anno"
                        viewstate("OrderBy") = TipoOrdinamento.Crescente

                        Dim objComparer As New Comparatore(New String() {"sAnno", "idTipoUtenza", "DA"}, New Boolean() {viewstate("OrderBy"), viewstate("OrderBy"), viewstate("OrderBy")})
                        Array.Sort(objQuote, objComparer)

                        GrdQuotaFissaEnte.DataSource = objQuote
                        GrdQuotaFissaEnte.DataBind()
                    End If
                Else
                    LblResult.Text = "La ricerca non ha prodotto risultati."
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicercaQuotaFissa.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

#Region "Griglie"
    Protected Sub GrdRowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                Dim IdQuotaFissa, IdTU, Da, A, TariffaH2O, TariffaDep, TariffaFog, Iva, Anno As String

                Dim obj As OggettoQuotaFissa

                obj = CType(e.Row.DataItem, OggettoQuotaFissa)

                IdQuotaFissa = obj.ID
                IdTU = obj.idTipoUtenza
                Iva = obj.dAliquota
                TariffaDep = obj.dImportoDep
                TariffaFog = obj.dImportoFog
                TariffaH2O = obj.dImportoH2O
                Anno = obj.sAnno
                Da = obj.DA
                A = obj.A

                e.Row.Attributes.Add("onClick", "DettaglioQuotaFissa(" & IdQuotaFissa & "," & IdTU & "," & Da & "," & A & ",'" & TariffaFog & "','" & TariffaDep & "','" & TariffaH2O & "','" & Iva & "','" & Anno & "')")
            End If
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicercaQuotaFissa.GrdRowDataBound.errore: ", ex)
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
    'Private Sub GrdQuotaFissaEnte_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdQuotaFissaEnte.ItemDataBound
    'Try
    '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '        'Gestione Riga Tabella

    '        Dim IdQuotaFissa, IdTU, Da, A, TariffaH2O, TariffaDep, TariffaFog, Iva, Anno As String

    '        Dim obj As OggettoQuotaFissa

    '        obj = CType(e.Item.DataItem, OggettoQuotaFissa)

    '        IdQuotaFissa = obj.ID
    '        IdTU = obj.idTipoUtenza
    '        Iva = obj.dAliquota
    '        TariffaDep = obj.dImportoDep
    '        TariffaFog = obj.dImportoFog
    '        TariffaH2O = obj.dImportoH2O
    '        Anno = obj.sAnno
    '        Da = obj.DA
    '        A = obj.A

    '        e.Item.Attributes.Add("onClick", "DettaglioQuotaFissa(" & IdQuotaFissa & "," & IdTU & "," & Da & "," & A & ",'" & TariffaFog & "','" & TariffaDep & "','" & TariffaH2O & "','" & Iva & "','" & Anno & "')")
    '    End If
    ' Catch ex As Exception
    '      Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicercaQuotaFissa.GrdQuotaFissaEnte_ItemDataBound.errore: ", ex)
    '      Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub

    'Private Sub GrdQuotaFissaEnte_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles GrdQuotaFissaEnte.SortCommand
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
    '        myDvResult = CType(Session("objQuote"), OggettoQuotaFissa())

    '        If Not IsNothing(myDvResult) Then
    '            ' ORDINO L'ARRAY DI OGGETTI
    '            Dim objComparer As New Comparatore(New String() {e.SortExpression}, New Boolean() {ViewState("OrderBy")})
    '            Array.Sort(myDvResult, objComparer)
    '        End If

    '        GrdQuotaFissaEnte.DataSource = myDvResult
    '        GrdQuotaFissaEnte.DataBind()
    '    Catch Err As Exception
    '         Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicercaQuotaFissa.GrdQuotaFissaEnte_SortCommand.errore: ", ex)
    '      Response.Redirect("../../PaginaErrore.aspx")
    '    End Try

    'End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdQuotaFissaEnte.DataSource = CType(Session("objQuote"), OggettoQuotaFissa())
            If page.HasValue Then
                GrdQuotaFissaEnte.PageIndex = page.Value
            End If
            GrdQuotaFissaEnte.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ResultRicercaQuotaFissa.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
End Class
