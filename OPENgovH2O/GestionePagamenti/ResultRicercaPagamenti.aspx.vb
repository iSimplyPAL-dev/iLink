Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports Utility
Imports log4net

Partial Class ResultRicercaPagamenti
    Inherits BasePage
    Private Cognome, Nome, CodFiscale, PIva, NFattura, DataFattura, Anno, Provenienza, DataAccreditoDal, DataAccreditoAl, Periodo As String

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
    Private myPagamentiForSearch As New OggettoPagamento
    Private myDvResult() As OggettoPagamento
    Private myRicerca As New ClsPagamenti
    Protected oReplace As New ClsGenerale.Generale
    Private Shared Log As ILog = LogManager.GetLogger("ResultRicercaPagamenti")

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            Cognome = Request.Item("Cognome")
            Nome = Request.Item("Nome")
            CodFiscale = Request.Item("CodFiscale")
            PIva = Request.Item("PIva")
            NFattura = Request.Item("NFattura")
            DataFattura = Request.Item("DataFattura")
            Anno = Request.Item("Anno")
            Periodo = Request.Item("DdlPeriodo")
            Provenienza = Request.Item("Provenienza")

            DataAccreditoDal = Request.Item("DataDal")
            DataAccreditoAl = Request.Item("DataAl")

            If Page.IsPostBack = False Then
                myPagamentiForSearch.IDEnte = ConstSession.IdEnte
                myPagamentiForSearch.sCognome = Cognome
                myPagamentiForSearch.sNome = Nome
                If PIva <> "" Then
                    myPagamentiForSearch.sCodFiscalePIva = PIva
                Else
                    myPagamentiForSearch.sCodFiscalePIva = CodFiscale
                End If
                myPagamentiForSearch.sNumeroFattura = NFattura
                If DataFattura <> "" Then
                    myPagamentiForSearch.sDataFattura = oReplace.FormattaData("A", "/", DataFattura, False)
                Else
                    myPagamentiForSearch.sDataFattura = ""
                End If
                myPagamentiForSearch.sAnnoEmissioneFattura = Anno

                If Periodo <> "" Then
                    myPagamentiForSearch.nperiodo = Periodo
                End If

                myPagamentiForSearch.sProvenienza = Provenienza

                If DataAccreditoDal <> "" Then
                    myPagamentiForSearch.sDataAccredito = oReplace.FormattaData("A", "/", DataAccreditoDal, False)
                Else
                    myPagamentiForSearch.sDataAccredito = ""
                End If

                If DataAccreditoAl <> "" Then
                    myPagamentiForSearch.sDataAccreditoAl = oReplace.FormattaData("A", "/", DataAccreditoAl, False)
                Else
                    myPagamentiForSearch.sDataAccreditoAl = ""
                End If

                Session("myPagamentiForSearch") = myPagamentiForSearch

                myDvResult = myRicerca.GetPagamenti(myPagamentiForSearch)
                If Not IsNothing(myDvResult) Then
                    If myDvResult.Length > 0 Then
                        Session.Add("myDvResult", myDvResult)
                        viewstate("SortKey") = "sCognome"
                        viewstate("OrderBy") = TipoOrdinamento.Crescente
                        ' ORDINO PER COGNOME e NOME
                        Dim objComparer As New Comparatore(New String() {"sCognome", "sNome"}, New Boolean() {viewstate("OrderBy"), viewstate("OrderBy")})
                        Array.Sort(myDvResult, objComparer)

                        GrdPagamenti.DataSource = myDvResult
                        GrdPagamenti.DataBind()

                    Else
                        LblResult.Text = "La ricerca non ha prodotto risultati."
                        LblResult.Style.Add("display", "")
                    End If
                Else
                    LblResult.Text = "La ricerca non ha prodotto risultati."
                    LblResult.Style.Add("display", "")
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicercaPagamenti.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub


    'Protected Function GiraDataFromDB(ByVal data As String) As String
    '    'leggo la data nel formato aaaammgg  e la metto nel formato gg/mm/aaaa
    '    Dim Giorno As String
    '    Dim Mese As String
    '    Dim Anno As String
    '    Try
    '        If data <> "" Then
    '            Giorno = Mid(data, 7, 2)
    '            Mese = Mid(data, 5, 2)
    '            Anno = Mid(data, 1, 4)
    '            GiraDataFromDB = Giorno & "/" & Mese & "/" & Anno
    '        Else
    '            GiraDataFromDB = ""
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicercaPagamenti.GiraDataFromDB.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Function

#Region "Griglie"
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim IDRow As String = e.CommandArgument.ToString()
            Dim sScript As String
            If e.CommandName = "RowOpen" Then
                For Each myRow As GridViewRow In GrdPagamenti.Rows
                    If IDRow = CType(myRow.FindControl("hfId"), HiddenField).Value Then
                        Dim obj As New OggettoPagamento

                        obj = CType(myRow.DataItem, OggettoPagamento)

                        myPagamentiForSearch = Session("myPagamentiForSearch")

                        myPagamentiForSearch.ID = IDRow
                        myPagamentiForSearch.IDEnte = CType(myRow.FindControl("hfIdEnte"), HiddenField).Value
                        myPagamentiForSearch.sProvenienza = CType(myRow.FindControl("hfsProvenienza"), HiddenField).Value
                        myPagamentiForSearch.ImportoEmesso = CType(myRow.FindControl("hfImportoEmesso"), HiddenField).Value
                        Session("myPagamentiForSearch") = myPagamentiForSearch
                        myDvResult = myRicerca.GetFatturaPagamenti(Session("myPagamentiForSearch"))

                        obj = myDvResult(0)

                        sScript = "DettaglioPagamento('" & obj.ID & "','" & obj.sNumeroFattura & "','" & obj.sDataFattura & "','','" & obj.ImportoPagamento & "','" & obj.ImportoEmesso & "','" & obj.nCodUtente & "','" & obj.sDataAccredito & "')"
                        RegisterScript(sScript, Me.GetType())
                    End If
                Next
            ElseIf e.CommandName = "RowDelete" Then
                If CType(sender, Ribes.OPENgov.WebControls.RibesGridView).UniqueID = "Grd" Then
                    For Each myRow As GridViewRow In GrdPagamenti.Rows
                        If IDRow = CType(myRow.FindControl("hfId"), HiddenField).Value Then

                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            Log.Debug(".GrdRowCommand::errore::", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub







    'Protected Sub GrdRowDataBound(sender As Object, e As GridViewRowEventArgs)
    '    Try
    '        If (e.Row.RowType = DataControlRowType.DataRow) Then
    '            Dim Id, NumeroFattura, DataFattura, Anno, ImportoPagamento, ImportoEmesso, CodUtente, Provenienza, DataAccredito As String

    '            Dim obj As New OggettoPagamento

    '            obj = CType(e.Row.DataItem, OggettoPagamento)

    '            Id = obj.ID
    '            NumeroFattura = obj.sNumeroFattura
    '            DataFattura = obj.sDataFattura
    '            ImportoPagamento = obj.ImportoPagamento
    '            ImportoEmesso = obj.ImportoEmesso
    '            'Anno = obj.sAnnoEmissioneFattura
    '            CodUtente = obj.nCodUtente

    '            Provenienza = obj.sProvenienza

    '            DataAccredito = obj.sDataAccredito

    '            If Provenienza = "Parini" Then
    '                e.Row.Attributes.Add("onClick", "GestAlert('a', 'warning', '', '', 'Impossibile visualizzare il dettaglio di un pagamento pregresso!');")
    '            Else
    '                e.Row.Attributes.Add("onClick", "DettaglioPagamento('" & Id & "','" & NumeroFattura & "','" & DataFattura & "','" & Anno & "','" & ImportoPagamento & "','" & ImportoEmesso & "','" & CodUtente & "','" & DataAccredito & "')")
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicercaPagamenti.GrdRowDataBound.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' Gestione del cambio pagina della griglia.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdPageIndexChanging(ByVal sender As Object, e As GridViewPageEventArgs)
        LoadSearch(e.NewPageIndex)
    End Sub
    'Private Sub GrdPagamenti_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdPagamenti.ItemDataBound
    'Try
    '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '        'Gestione Riga Tabella

    '        Dim Id, NumeroFattura, DataFattura, Anno, ImportoPagamento, ImportoEmesso, CodUtente, Provenienza, DataAccredito As String

    '        Dim obj As New OggettoPagamento

    '        obj = CType(e.Item.DataItem, OggettoPagamento)

    '        Id = obj.ID
    '        NumeroFattura = obj.sNumeroFattura
    '        DataFattura = obj.sDataFattura
    '        ImportoPagamento = obj.ImportoPagamento
    '        ImportoEmesso = obj.ImportoEmesso
    '        'Anno = obj.sAnnoEmissioneFattura
    '        CodUtente = obj.nCodUtente

    '        Provenienza = obj.sProvenienza

    '        DataAccredito = obj.sDataAccredito

    '        If Provenienza = "Parini" Then
    '            e.Item.Attributes.Add("onClick", "GestAlert('a', 'warning', '', '', 'Impossibile visualizzare il dettaglio di un pagamento pregresso!');")
    '        Else
    '            e.Item.Attributes.Add("onClick", "DettaglioPagamento('" & Id & "','" & NumeroFattura & "','" & DataFattura & "','" & Anno & "','" & ImportoPagamento & "','" & ImportoEmesso & "','" & CodUtente & "','" & DataAccredito & "')")
    '        End If
    '    End If
    '  Catch ex As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicercaPagamenti.GrdPagamenti_ItemDataBound.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    ' End Try
    'End Sub

    'Private Sub GrdPagamenti_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles GrdPagamenti.SortCommand
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
    '        myDvResult = CType(Session("myDvResult"), OggettoPagamento())

    '        If Not IsNothing(myDvResult) Then
    '            ' ORDINO L'ARRAY DI OGGETTI
    '            Dim objComparer As New Comparatore(New String() {e.SortExpression}, New Boolean() {ViewState("OrderBy")})
    '            Array.Sort(myDvResult, objComparer)
    '        End If

    '        GrdPagamenti.DataSource = myDvResult
    '        GrdPagamenti.DataBind()
    '    Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicercaPagamenti.GrdPagamenti_SortCommand.errore: ", Err)
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
            GrdPagamenti.DataSource = CType(Session("myDvResult"), OggettoPagamento())
            If page.HasValue Then
                GrdPagamenti.PageIndex = page.Value
            End If
            GrdPagamenti.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicercaPagamenti.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
End Class
