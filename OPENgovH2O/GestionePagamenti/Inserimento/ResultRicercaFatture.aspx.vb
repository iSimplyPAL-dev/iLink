Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports Utility
Imports log4net
Partial Class ResultRicercaFatture
    Inherits BasePage
    Private Cognome, Nome, Importo, NFattura, DataFattura, Anno, Operazione As String

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
    Private myFattureForSearch As New OggettoPagamento
    Private myDvResultFatture() As OggettoPagamento
    Private myRicerca As New ClsPagamenti
    Protected oReplace As New ClsGenerale.Generale
    Protected FncGrd As New ClsGenerale.FunctionGrd
    Private Shared Log As ILog = LogManager.GetLogger("ResultRicercaFatture")

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            Cognome = Request.Item("Cognome")
            Nome = Request.Item("Nome")
            Importo = Request.Item("Importo")
            If InStr(Request.Item("DataFattura"), "/") > 0 Then
                DataFattura = oReplace.FormattaData("A", "", Request.Item("DataFattura"), False)
            Else
                DataFattura = Request.Item("DataFattura")
            End If
            NFattura = Request.Item("NFattura")
            Anno = Request.Item("Anno")

            If Not Request.Item("Operazione") Is Nothing Then
                Operazione = Request.Item("Operazione")
            End If
            If Page.IsPostBack = False Then
                myFattureForSearch.sCognome = Cognome
                myFattureForSearch.sNome = Nome
                If Importo <> "" Then
                    myFattureForSearch.ImportoEmesso = Importo
                End If
                myFattureForSearch.sNumeroFattura = NFattura
                myFattureForSearch.sDataFattura = DataFattura
                myFattureForSearch.sAnnoEmissioneFattura = Anno
                Session("myFattureForSearch") = myFattureForSearch
                myDvResultFatture = myRicerca.GetFatturaPagamenti(myFattureForSearch)
                If Not IsNothing(myDvResultFatture) Then
                    If myDvResultFatture.Length > 0 Then
                        Session.Add("myDvResultFatture", myDvResultFatture)
                        ViewState("SortKey") = "sCognome"
                        ViewState("OrderBy") = TipoOrdinamento.Crescente
                        ' ORDINO PER COGNOME e NOME
                        Dim objComparer As New Comparatore(New String() {"sCognome", "sNome", "sDataFattura", "sNumeroFattura"}, New Boolean() {ViewState("OrderBy"), ViewState("OrderBy"), TipoOrdinamento.Decrescente, TipoOrdinamento.Decrescente})
                        Array.Sort(myDvResultFatture, objComparer)

                        GrdFatture.DataSource = myDvResultFatture
                        GrdFatture.DataBind()
                    Else
                        LblResult.Text = "La ricerca non ha prodotto risultati."
                        LblResult.Style.Add("display", "")
                    End If
                Else
                    LblResult.Text = "La ricerca non ha prodotto risultati."
                    LblResult.Style.Add("display", "")
                End If
            Else
                If Operazione = "modifica" Then
                    myDvResultFatture = myRicerca.GetFatturaPagamenti(Session("myFattureForSearch"))
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicercaFatture.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

#Region "Griglie"
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim IDRow As String = e.CommandArgument.ToString()
            Dim sScript As String
            If e.CommandName = "RowOpen" Then
                For Each myRow As GridViewRow In GrdFatture.Rows
                    If IDRow = CType(myRow.FindControl("hfIdFatturaNota"), HiddenField).Value Then

                        Dim Id, NumeroFattura, DataFattura, Anno, Importo, ImportoEmesso, DataPagamento, CodUtente, DataAccredito As String
                        Dim obj As OggettoPagamento

                        obj = CType(myRow.DataItem, OggettoPagamento)
                        myFattureForSearch = Session("myFattureForSearch")

                        myFattureForSearch.IdFatturaNota = IDRow
                        myFattureForSearch.sNumeroFattura = CType(myRow.FindControl("hfNumeroFattura"), HiddenField).Value
                        myFattureForSearch.sDataFattura = CType(myRow.FindControl("hfDataFattura"), HiddenField).Value

                        Session("myFattureForSearch") = myFattureForSearch

                        myDvResultFatture = myRicerca.GetFatturaPagamenti(Session("myFattureForSearch"))
                        obj = myDvResultFatture(0)

                        Id = obj.ID
                        NumeroFattura = obj.sNumeroFattura
                        DataFattura = obj.sDataFattura
                        Importo = obj.ImportoPagamento
                        ImportoEmesso = obj.ImportoEmesso
                        DataPagamento = obj.sDataPagamento
                        Anno = obj.sAnnoEmissioneFattura
                        CodUtente = obj.nCodUtente
                        DataAccredito = obj.sDataAccredito

                        'serve? fare controllo
                        ''myRow.Attributes.Add("onClick", "DettaglioPagamento('" & Id & "','" & NumeroFattura & "','" & DataFattura & "','" & Importo & "','" & ImportoEmesso & "','" & DataPagamento & "','" & CodUtente & "','" & Operazione & "','" & DataAccredito & "')")
                        sScript = "DettaglioPagamento('" & myDvResultFatture(0).ID & "','" & myDvResultFatture(0).sNumeroFattura & "','" & myDvResultFatture(0).sDataFattura & "','" & myDvResultFatture(0).ImportoPagamento & "','" & myDvResultFatture(0).ImportoEmesso & "','" & myDvResultFatture(0).sDataPagamento & "','" & myDvResultFatture(0).nCodUtente & "','" & Operazione & "','" & myDvResultFatture(0).sDataAccredito & "');"
                        RegisterScript(sScript, Me.GetType())
                    End If
                Next
            End If
        Catch ex As Exception
            Log.Debug(".GrdRowCommand::errore::", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'fine sostituizione


    ''' <summary>
    ''' Gestione del cambio pagina della griglia.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdPageIndexChanging(ByVal sender As Object, e As GridViewPageEventArgs)
        LoadSearch(e.NewPageIndex)
    End Sub
    'Private Sub GrdFatture_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdFatture.ItemDataBound
    'Try
    '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '        'Gestione Riga Tabella

    '        Dim Id, NumeroFattura, DataFattura, Anno, Importo, ImportoEmesso, DataPagamento, CodUtente, DataAccredito As String
    '        Dim obj As OggettoPagamento

    '        obj = CType(e.Item.DataItem, OggettoPagamento)

    '        Id = obj.ID
    '        NumeroFattura = obj.sNumeroFattura
    '        DataFattura = obj.sDataFattura
    '        Importo = obj.ImportoPagamento
    '        ImportoEmesso = obj.ImportoEmesso
    '        DataPagamento = obj.sDataPagamento
    '        Anno = obj.sAnnoEmissioneFattura
    '        CodUtente = obj.nCodUtente
    '        DataAccredito = obj.sDataAccredito

    '        e.Item.Attributes.Add("onClick", "DettaglioPagamento('" & Id & "','" & NumeroFattura & "','" & DataFattura & "','" & Importo & "','" & ImportoEmesso & "','" & DataPagamento & "','" & CodUtente & "','" & Operazione & "','" & DataAccredito & "')")
    '    End If
    ' Catch ex As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicercaFatture.GrdFatture_ItemDataBound.errore: ", ex)
    '       Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub

    'Private Sub GrdFatture_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles GrdFatture.SortCommand
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
    '        myDvResultFatture = CType(Session("myDvResultFatture"), OggettoPagamento())

    '        If Not IsNothing(myDvResultFatture) Then
    '            ' ORDINO L'ARRAY DI OGGETTI
    '            Dim objComparer As New Comparatore(New String() {e.SortExpression}, New Boolean() {ViewState("OrderBy")})
    '            Array.Sort(myDvResultFatture, objComparer)
    '        End If
    '        GrdFatture.DataSource = myDvResultFatture
    '        GrdFatture.DataBind()
    '    Catch Err As Exception
    '      Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicercaFatture.GrdFatture_SortCommand.errore: ",Err)
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
            GrdFatture.DataSource = CType(Session("myDvResultFatture"), OggettoPagamento())
            If page.HasValue Then
                GrdFatture.PageIndex = page.Value
            End If
            GrdFatture.DataBind()
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicercaFatture.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
End Class
