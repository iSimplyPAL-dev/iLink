Imports log4net

Partial Class ResultDatiMancanti
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger("ResultDatiMancanti")

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
    '    Dim oListDatiMancanti() As ObjAEDatiMancanti
    '    Dim FncDatiMancanti As New ClsAEDatiMancanti
    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Dim WFErrore As String
    '    Dim oMyParam As RicercaDatiMancanti.ObjParamRicMancanti

    '    Try
    '        Session("datacatasto") = Nothing
    '        'Se non sto ricaricando la pagina da postback:
    '        If Page.IsPostBack = False Then
    '            'inizializzo la connessione
    '            WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '            If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '                Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '            End If
    '            'valorizzo le variabili locali con i parametri passati alla pagina;
    '            oMyParam.sAnno = Request.Item("DdlAnno")
    '            oMyParam.sCognome = Request.Item("TxtCognome")
    '            oMyParam.sNome = Request.Item("TxtNome")
    '            oMyParam.nTipoRicerca = Request.Item("TipoRicerca")
    '            Session("ParamSearchDatiMancanti") = oMyParam
    '            'popolo l'array di oggetti mancanti con il risultato della ricerca richiamando la funzione GetListDatiMancanti(sAnno, sCognome, sNome, nTypeRicerca);
    '            oListDatiMancanti = FncDatiMancanti.GetListDatiMancanti(ConstSession.IdEnte, oMyParam.sAnno, oMyParam.sCognome, oMyParam.sNome, oMyParam.nTipoRicerca, WFSessione)
    '            'se l'array non è vuoto:
    '            If Not oListDatiMancanti Is Nothing Then
    '                'assegno l'array alla griglia;
    '                If oListDatiMancanti.GetLength(0) > 0 Then
    '                    GrdResult.start_index = Convert.ToString(0)
    '                    GrdResult.Rows.Count = oListDatiMancanti.GetLength(0)
    '                    GrdResult.DataSource = oListDatiMancanti
    '                    GrdResult.DataBind()
    '                Else
    '                    'visualizzo la label di non sono presenti record per i parametri;
    '                    LblMessage.Text = "La ricerca non ha prodotto risultati."
    '                    LblMessage.Style.Add("display", "")
    '                End If
    '                'memorizzo in sessione il dataview;
    '                Session.Add("ResultRicercaDatiMancanti", oListDatiMancanti)
    '            Else
    '                'visualizzo la label di non sono presenti record per i parametri;
    '                LblMessage.Text = "La ricerca non ha prodotto risultati."
    '                LblMessage.Style.Add("display", "")
    '            End If
    '        Else
    '            'rassegno il dataview alla griglia;
    '            oListDatiMancanti = CType(Session("ResultRicercaDatiMancanti"), ObjAEDatiMancanti())
    '            'valorizzo correttamente la paginazione;
    '            GrdResult.AllowCustomPaging = False
    '            GrdResult.Rows.Count = oListDatiMancanti.GetLength(0)
    '            GrdResult.DataSource = CType(Session("ResultRicercaDatiMancanti"), ObjAEDatiMancanti())
    '            GrdResult.start_index = GrdResult.CurrentPageIndex.ToString()
    '        End If
    '    Catch Err As Exception
    '      Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultDatiMancanti.Page_Load.errore: ", Err)
    '      Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim oListDatiMancanti() As ObjAEDatiMancanti
        Dim FncDatiMancanti As New ClsAEDatiMancanti
        Dim oMyParam As RicercaDatiMancanti.ObjParamRicMancanti

        Try
            Session("datacatasto") = Nothing
            'Se non sto ricaricando la pagina da postback:
            If Page.IsPostBack = False Then
                'valorizzo le variabili locali con i parametri passati alla pagina;
                oMyParam.sAnno = Request.Item("DdlAnno")
                oMyParam.sCognome = Request.Item("TxtCognome")
                oMyParam.sNome = Request.Item("TxtNome")
                oMyParam.nTipoRicerca = Request.Item("TipoRicerca")
                Session("ParamSearchDatiMancanti") = oMyParam
                'popolo l'array di oggetti mancanti con il risultato della ricerca richiamando la funzione GetListDatiMancanti(sAnno, sCognome, sNome, nTypeRicerca);
                oListDatiMancanti = FncDatiMancanti.GetListDatiMancanti(ConstSession.IdEnte, oMyParam.sAnno, oMyParam.sCognome, oMyParam.sNome, oMyParam.nTipoRicerca)
                'se l'array non è vuoto:
                If Not oListDatiMancanti Is Nothing Then
                    'assegno l'array alla griglia;
                    If oListDatiMancanti.GetLength(0) > 0 Then
                        GrdResult.DataSource = oListDatiMancanti
                        GrdResult.DataBind()
                    Else
                        'visualizzo la label di non sono presenti record per i parametri;
                        LblMessage.Text = "La ricerca non ha prodotto risultati."
                        LblMessage.Style.Add("display", "")
                    End If
                    'memorizzo in sessione il dataview;
                    Session.Add("ResultRicercaDatiMancanti", oListDatiMancanti)
                Else
                    'visualizzo la label di non sono presenti record per i parametri;
                    LblMessage.Text = "La ricerca non ha prodotto risultati."
                    LblMessage.Style.Add("display", "")
                End If
            Else
                'rassegno il dataview alla griglia;
                oListDatiMancanti = CType(Session("ResultRicercaDatiMancanti"), ObjAEDatiMancanti())
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultDatiMancanti.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

#Region "Griglie"
    Protected Sub GrdRowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                e.Row.Attributes.Add("onClick", "parent.parent.Visualizza.LoadDettaglioArticolo('" & e.Row.Cells(4).Text & "','" & e.Row.Cells(5).Text & "')")
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultDatiMancanti.GrdRowDataBound.errore: ", ex)
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
    'Private Sub GrdResult_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdResult.ItemDataBound
    '    Try
    '        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '            'Se non sono sulla riga di intestazione:
    '            'assegno alla riga il richiamo sull’evento click alla funzione java LoadDettaglioArticolo(e.Item.Cells(5).Text);
    '            e.Item.Attributes.Add("onClick", "parent.parent.Visualizza.LoadDettaglioArticolo('" & e.Item.Cells(4).Text & "','" & e.Item.Cells(5).Text & "')")
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultDatiMancanti.GrdResult_ItemDataBound.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdResult.DataSource = CType(Session("ResultRicercaDatiMancanti"), ObjAEDatiMancanti())
            If page.HasValue Then
                GrdResult.PageIndex = page.Value
            End If
            GrdResult.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultDatiMancanti.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
End Class
