Imports RemotingInterfaceAnater
Imports Anater.Oggetti
Imports Utility
Imports log4net

Partial Class ResultRicImmobile
    Inherits BasePage

    Private Shared Log As ILog = LogManager.GetLogger(GetType(ResultRicImmobile))
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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim oRicercaAnater As New RicercaTarsuAnater
        Dim TypeOfRI As Type = GetType(IRemotingInterfaceTARSU)
        Dim RemH2O As IRemotingInterfaceTARSU
        Dim oImmobileAnater() As OggettoUnitaImmobiliareAnater
        Try
            'carico i parametri di ricerca
            If Page.IsPostBack = False Then
                oRicercaAnater.CodiceComune = ConstSession.IdEnte
                If Request.Item("TxtVia") <> "" Then
                    oRicercaAnater.Via = Request.Item("TxtVia")
                End If
                If Request.Item("TxtCivico") <> "" Then
                    oRicercaAnater.Civico = Request.Item("TxtCivico")
                End If
                If Request.Item("TxtInterno") <> "" Then
                    oRicercaAnater.Interno = Request.Item("TxtInterno")
                End If
                If Request.Item("TxtFoglio") <> "" Then
                    oRicercaAnater.Foglio = Request.Item("TxtFoglio")
                End If
                If Request.Item("TxtNumero") <> "" Then
                    oRicercaAnater.Numero = Request.Item("TxtNumero")
                End If
                If Request.Item("TxtSubalterno") <> "" Then
                    oRicercaAnater.Subalterno = Request.Item("TxtSubalterno")
                End If
                'attivo il servizio
                RemH2O = Activator.GetObject(TypeOfRI, ConfigurationManager.AppSettings("URLanaterH2O"))
                'eseguo la ricerca su ANATER
                oImmobileAnater = RemH2O.GetUnitaImmobiliare(oRicercaAnater)
                'carico i dati in griglia
                If oImmobileAnater.Length > 0 Then
                    GrdRicerca.DataSource = oImmobileAnater
                    GrdRicerca.DataBind()
                    LblResult.Style.Add("display", "none")
                End If
                Session("oImmobileAnater") = oImmobileAnater
            Else
                ControllaCheckbox()
                ' eseguo il cambio di pagina se la pagina ha fatto il postback
                GrdRicerca.DataSource = Session("oImmobileAnater")
                GrdRicerca.DataBind()
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicImmobile.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try

    End Sub

    Private Sub CmdRibaltaAnater_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdRibaltaAnater.Click
        Dim sScript As String = ""
        Dim x As Integer
        Dim oListImmobile() As OggettoUnitaImmobiliareAnater
        Dim DtDati As New DataTable
        Dim DrDati As DataRow
        Dim DBContatori As GestContatori = New GestContatori

        Try
            oListImmobile = Session("oImmobileAnater")
            'ribalto
            'controllo se ho già degli immobili inseriti
            If Not Session("datacatasto") Is Nothing Then
                DtDati = CType(Session("datacatasto"), System.Data.DataTable)
            End If
            For x = 0 To oListImmobile.GetUpperBound(0)
                If oListImmobile(x).Selezionato = True Then
                    If CInt(Request.Params("IDContatore")) <> 0 Then
                        DBContatori.SetDatiCatastali(oListImmobile(x).UniInternoUbi, oListImmobile(x).UniPianoUbi, oListImmobile(x).UniFoglio, oListImmobile(x).UniNumProgrFabbricato, oListImmobile(x).UniSubalterno, -1, CInt(Request.Params("IDContatore")), "", "", "")
                        sScript = "parent.parent.opener.document.getElementById('CmdRibaltaUIAnater').click();"
                        sScript += "parent.parent.window.close();"
                    Else
                        DrDati = DtDati.NewRow()
                        If DtDati.Rows.Count = 0 Then
                            DrDati.Item(0) = 1
                        Else
                            DrDati.Item(0) = CInt(CType(DtDati.Rows(DtDati.Rows.Count - 1), System.Data.DataRow).ItemArray(0)) + 1
                        End If

                        DrDati.Item(1) = 1
                        DrDati.Item(2) = oListImmobile(x).UniInternoUbi
                        DrDati.Item(3) = oListImmobile(x).UniPianoUbi
                        DrDati.Item(4) = oListImmobile(x).UniFoglio
                        DrDati.Item(5) = oListImmobile(x).UniNumProgrFabbricato
                        DrDati.Item(6) = oListImmobile(x).UniSubalterno

                        DtDati.Rows.Add(DrDati)
                        Session("ViaUIAnater") = oListImmobile(x).UniDescrizioneVia
                        Session("CodViaUIAnater") = oListImmobile(x).UniCodVia
                        Session("CivicoUIAnater") = oListImmobile(x).UniNumeroCiv
                        Session("EsponenteUIAnater") = oListImmobile(x).UniEspoUbi
                        Session("datacatasto") = DtDati
                        sScript = "parent.parent.opener.document.getElementById('CmdRibaltaUIAnater').click();"
                        sScript += "parent.parent.window.close();"
                    End If
                End If
            Next
            RegisterScript(sScript, Me.GetType())
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicImmobile.CmdRibaltaAnater_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

#Region "Griglie"
    ''' <summary>
    ''' Gestione del cambio pagina della griglia.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdPageIndexChanging(ByVal sender As Object, e As GridViewPageEventArgs)
        LoadSearch(e.NewPageIndex)
    End Sub
    'Private Sub GrdRicerca_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles GrdRicerca.SortCommand

    '    Dim oImmobileAnater() As OggettoUnitaImmobiliareAnater

    '    oImmobileAnater = CType(Session("oImmobileAnater"), OggettoUnitaImmobiliareAnater())
    'Try
    '    If ViewState("OrderBy") = TipoOrdinamento.Crescente Then
    '        ViewState("OrderBy") = TipoOrdinamento.Decrescente
    '    Else
    '        ViewState("OrderBy") = TipoOrdinamento.Crescente
    '    End If

    '    If e.SortExpression = "UBICAZIONE" Then
    '        Dim objComparer As Comparatore = New Comparatore(New String() {"UniTipoVia", "UniDescrizioneVia"}, New Boolean() {ViewState("OrderBy"), ViewState("OrderBy")})
    '        Array.Sort(oImmobileAnater, objComparer)
    '    End If

    '    GrdRicerca.DataSource = oImmobileAnater
    ' Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicImmobile.GrdRicerca_SortCommand.errore: ", Err)
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
            GrdRicerca.DataSource = CType(Session("oImmobileAnater"), DataView)
            If page.HasValue Then
                GrdRicerca.PageIndex = page.Value
            End If
            GrdRicerca.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicImmobile.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub

    Private Sub ControllaCheckbox()
        Dim itemGrid As GridViewRow
        Dim oImmobileAnater() As OggettoUnitaImmobiliareAnater
        Dim x, nSel As Integer

        Try
            nSel = 0
            oImmobileAnater = CType(Session("oImmobileAnater"), OggettoUnitaImmobiliareAnater())
            For Each itemGrid In GrdRicerca.Rows
                'prendo l'ID da aggiornare
                For x = 0 To oImmobileAnater.GetUpperBound(0)
                    If oImmobileAnater(x).IDProgressivo = CType(itemGrid.Cells(15).Text, Long) Then
                        oImmobileAnater(x).Selezionato = CType(itemGrid.FindControl("ChkSelezionato"), CheckBox).Checked
                        If oImmobileAnater(x).Selezionato = True Then
                            nSel += 1
                        End If
                    End If
                Next
            Next
            Session("oImmobileAnater") = oImmobileAnater
            'se arrivo da ruolo posso selezionare un'immobile solo
            If Request.Item("Provenienza") = "A" And nSel > 1 Then
                Dim sScript As String
                sScript = "GestAlert('a', 'warning', '', '', 'E\' possibile associare un solo immobile al ruolo.\n Sarà associato solo il primo selezionato!');"
                RegisterScript(sScript , Me.GetType())
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicImmobile.ControllaCheckbox.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Public Function FormattaDataGrd(ByVal tDataGrd As DateTime) As String
        If tDataGrd = Date.MinValue Or tDataGrd = Date.MaxValue Then
            Return ""
        Else
            Return tDataGrd.ToShortDateString
        End If
    End Function
End Class
