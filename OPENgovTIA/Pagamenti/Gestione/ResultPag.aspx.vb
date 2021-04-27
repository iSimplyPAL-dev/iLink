'*** 20120921 - pagamenti ***
Imports Utility
Imports log4net
Imports RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti
''' <summary>
''' Pagina per la visualizzazione del risultato della ricerca pagamenti.
''' Contiene la griglia dalla quale è possibile accedere al dettaglio.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class ResultPag
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ResultPag))
    'Private WFSessione As OPENUtility.CreateSessione
    Private WFErrore As String
    Private oMySearchPag As New ObjSearchPagamenti
    Private FncGestPag As New ClsGestPag
    Protected FncGrd As New Formatta.FunctionGrd
    Private sScript As String = ""

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
        If ConstSession.IsFromVariabile(Request.Item("IsFromVariabile")) = "1" Then
        End If
        hfProvMenu.Value = ConstSession.Ambiente.ToUpper + "|" + Utility.StringOperation.FormatInt(ConstSession.IsFromVariabile).ToString
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim oListPagamenti() As OggettoPagamenti
        'Dim MyDBEngine As DAL.DBEngine = Nothing

        Try
            ''inizializzo la connessione
            'WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
            'If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
            '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
            'End If
            Dim myStringConnection As String = ConstSession.StringConnection
            Dim IdTributo As String = Request.Item("TRIBUTO")
            If IdTributo = Utility.Costanti.TRIBUTO_OSAP Or IdTributo = Utility.Costanti.TRIBUTO_SCUOLE Then
                myStringConnection = ConstSession.StringConnectionOSAP
            End If

            If Page.IsPostBack = False Then
                Session("oMySearchPag") = Nothing
                oMySearchPag.sEnte = ConstSession.IdEnte
                oMySearchPag.IdTributo = IdTributo
                oMySearchPag.bRicPag = True
                If Not Request.Item("CodContribuente") Is Nothing And Request.Item("CodContribuente") <> "" Then
                    oMySearchPag.IdContribuente = Request.Item("CodContribuente")
                Else
                    oMySearchPag.sCFPIVA = Request.Item("CFPIVA")
                    oMySearchPag.sCognome = Request.Item("Cognome")
                    oMySearchPag.sNome = Request.Item("Nome")
                    oMySearchPag.sNAvviso = Request.Item("NAvviso")
                End If
                oMySearchPag.sAnnoRif = Request.Item("AnnoRif")
                If Not IsNothing(Request.Item("DataPagDal")) And Request.Item("DataPagDal") <> "" Then
                    oMySearchPag.tDataPagamentoDal = Request.Item("DataPagDal")
                End If
                If Not IsNothing(Request.Item("DataPagAl")) And Request.Item("DataPagAl") <> "" Then
                    oMySearchPag.tDataPagamentoAl = Request.Item("DataPagAl")
                End If
                If Not IsNothing(Request.Item("DataDal")) And Request.Item("DataDal") <> "" Then
                    oMySearchPag.tDataAccreditoDal = Request.Item("DataDal")
                End If
                If Not IsNothing(Request.Item("DataAl")) And Request.Item("DataAl") <> "" Then
                    oMySearchPag.tDataAccreditoAl = Request.Item("DataAl")
                End If
                If Not IsNothing(Request.Item("NonAbb")) Then
                    oMySearchPag.IsNonAbbinati = Request.Item("NonAbb")
                End If
                oMySearchPag.Flusso = Request.Item("Flusso")
                oMySearchPag.Provenienza = Request.Item("Provenienza")
                Session("oMySearchPag") = oMySearchPag

                oListPagamenti = FncGestPag.GetListPagamenti(oMySearchPag, myStringConnection)
                If Not IsNothing(oListPagamenti) Then
                    If oListPagamenti.Length > 0 Then
                        Session.Add("oListPagamenti", oListPagamenti)
                        GrdPagamenti.DataSource = oListPagamenti
                        GrdPagamenti.DataBind()
                        LblResult.Style.Add("display", "")
                        Dim nPag As Integer = 0
                        Dim impPag As Double = 0
                        Dim myPag As OggettoPagamenti
                        For Each myPag In oListPagamenti
                            nPag += 1
                            impPag += myPag.dImportoPagamento
                        Next
                        LblNPag.Text = "N.Pagamenti:&emsp;" + nPag.ToString
                        LblImpPag.Text = "per un Totale di €&emsp;" + FormatNumber(impPag, 2)
                    Else
                        LblResult.Text = "La ricerca non ha prodotto risultati."
                        LblResult.Style.Add("display", "")
                    End If
                Else
                    LblResult.Text = "La ricerca non ha prodotto risultati."
                    LblResult.Style.Add("display", "")
                End If
            End If
            If Not IsNothing(Request.Item("NonAbb")) Then
                If Request.Item("NonAbb") = True Then
                    GrdPagamenti.Columns(1).HeaderText = "Id.Operazione"
                    GrdPagamenti.Columns(4).Visible = False
                Else
                    GrdPagamenti.Columns(1).HeaderText = "N.Avviso"
                    GrdPagamenti.Columns(4).Visible = True
                End If
            End If
            RegisterScript("parent.parent.Visualizza.DivAttesa.style.display='none';", Me.GetType)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultPag.Page_Load.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
            'Finally
            '    WFSessione.Kill()
        End Try
    End Sub

#Region "Griglie"
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim IDRow As String = e.CommandArgument.ToString()
            If e.CommandName = "RowOpen" Then
                For Each myRow As GridViewRow In GrdPagamenti.Rows
                    If IDRow = CType(myRow.FindControl("hfid"), HiddenField).Value Then
                        Dim sScript As String = ""
                        sScript += "LoadPagamento('" & Request.Item("TRIBUTO") & "','" & Request.Item("NonAbb") & "','" & CType(myRow.FindControl("hfid"), HiddenField).Value & "','" & myRow.Cells(0).Text & "','" & myRow.Cells(3).Text & "','" & myRow.Cells(1).Text & "','" & CType(myRow.FindControl("LblDataPag"), Label).Text & "');"
                        RegisterScript(sScript, Me.GetType)
                    End If
                Next
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultPag.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
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
    'Private Sub GrdPagamenti_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdPagamenti.ItemDataBound
    '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '        e.Item.Attributes.Add("onClick", "LoadPagamento('" & Request.Item("TRIBUTO") & "','" & Request.Item("NonAbb") & "','" & e.Item.Cells(9).Text & "','" & e.Item.Cells(0).Text & "','" & e.Item.Cells(3).Text & "','" & e.Item.Cells(1).Text & "','" & CType(e.Item.Cells(4).FindControl("LblDataPag"), Label).Text & "')")
    '    End If
    'End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdPagamenti.DataSource = CType(Session("oListPagamenti"), OggettoPagamenti())
            If page.HasValue Then
                GrdPagamenti.PageIndex = page.Value
            End If
            GrdPagamenti.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultPag.LoadSearch.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
'*** ***