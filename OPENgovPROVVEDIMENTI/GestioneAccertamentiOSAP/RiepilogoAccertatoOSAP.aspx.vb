Imports log4net
Imports ComPlusInterface
''' <summary>
''' Pagina per la visualizzazione del provvedimento generato.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class RiepilogoAccertatoOSAP
    Inherits BaseEnte
    Private Shared Log As ILog = LogManager.GetLogger(GetType(RiepilogoAccertatoOSAP))
    Protected FncForGrd As New Formatta.SharedGrd

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
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim sScript As String
        Dim oDichiarato() As OSAPAccertamentoArticolo
        Dim oAccertato() As OSAPAccertamentoArticolo
        Dim oAttoInserito As OggettoAttoOSAP

        Dim intRETTIFICATO As Integer = Request.Item("RETTIFICATO")

        'carico i comandi
        sScript = "parent.Comandi.location.href='ComandiRiepilogoAccertamentoOSAP.aspx?rettificato=" & intRETTIFICATO & "';"
        RegisterScript(sScript, Me.GetType())

        Try
            LblContribuente.Text = Request.Item("nominativo")
            LblAnnoAccertamento.Text = Request.Item("anno")
            LblTipoAvviso.Text = "Tipologia: " & CStr(Session("TipoAvviso")).ToUpper

            If Page.IsPostBack = False Then
                'carico il dichiarato
                If Not Session("oSituazioneDichiarato") Is Nothing Then
                    oDichiarato = CType(Session("oSituazioneDichiarato"), OSAPAccertamentoArticolo())
                    If oDichiarato(0).IdLegame <> 0 Then
                        GrdDichiarato.DataSource = oDichiarato
                        GrdDichiarato.DataBind()
                    End If
                End If
                'carico l'accertato
                If Not Session("oSituazioneAccertato") Is Nothing Then
                    'devo caricare l'oggetto ArticoloRuoloAccertamento per popolare la griglia
                    oAccertato = CType(Session("oSituazioneAccertato"), OSAPAccertamentoArticolo())
                    If oAccertato(0).IdLegame <> 0 Then
                        GrdAccertato.DataSource = oAccertato
                        GrdAccertato.DataBind()
                    End If
                End If
                'carico l'atto
                If Not Session("oSituazioneAtto") Is Nothing Then
                    oAttoInserito = CType(Session("oSituazioneAtto"), OggettoAttoOSAP)
                    'Riepilogo PreAccertamento FASE 2 {confronto pagato con dovuto}+FASE 1 {confronto data pagato con scadenza dovuto}
                    LblImpDichPreAcc.Text = FormatNumber(oAttoInserito.IMPORTO_DICHIARATO_F2, 2)
                    LblImpVersPreAcc.Text = FormatNumber(oAttoInserito.IMPORTO_PAGATO, 2)
                    LblImpDifImpPreAcc.Text = FormatNumber(oAttoInserito.IMPORTO_DIFFERENZA_IMPOSTA_F2, 2)
                    LblImpSanzPreAcc.Text = FormatNumber(oAttoInserito.IMPORTO_SANZIONI_F2, 2)
                    LblImpIntPreAcc.Text = FormatNumber(oAttoInserito.IMPORTO_INTERESSI_F2, 2)
                    LblImpTotPreAcc.Text = FormatNumber(oAttoInserito.IMPORTO_TOTALE_F2, 2)

                    'Riepilogo Accertamento {confronto dichiarato con accertato}
                    LblImpDich.Text = FormatNumber(oAttoInserito.IMPORTO_DICHIARATO_F2, 2)
                    LblImpAcc.Text = FormatNumber(oAttoInserito.IMPORTO_ACCERTATO_ACC, 2)
                    LblImpDifImp.Text = FormatNumber(oAttoInserito.IMPORTO_DIFFERENZA_IMPOSTA_ACC, 2)
                    LblImpSanz.Text = FormatNumber(oAttoInserito.IMPORTO_SANZIONI_ACC, 2)
                    LblImpSanzRid.Text = FormatNumber(oAttoInserito.IMPORTO_SANZIONI_RIDOTTE_ACC, 2)
                    LblImpInt.Text = FormatNumber(oAttoInserito.IMPORTO_INTERESSI_ACC, 2)
                    LblImpTot.Text = FormatNumber(oAttoInserito.IMPORTO_TOTALE_ACC, 2)

                    LblDiffImpostaAvviso.Text = FormatNumber(oAttoInserito.IMPORTO_DIFFERENZA_IMPOSTA, 2)
                    LblSanzAvviso.Text = FormatNumber(oAttoInserito.IMPORTO_SANZIONI, 2)
                    LblSanzRidotteAvviso.Text = FormatNumber(oAttoInserito.IMPORTO_SANZIONI_RIDOTTO, 2)
                    LblIntAvviso.Text = FormatNumber(oAttoInserito.IMPORTO_INTERESSI, 2)
                    LblTotAvviso.Text = FormatNumber(oAttoInserito.IMPORTO_TOTALE, 2)
                End If

                Select Case CInt(GrdDichiarato.Rows.Count)
                    Case 0
                        GrdDichiarato.Visible = False
                        LblDich.Text = "Nessun Immobile Dichiarato"
                    Case Is > 0
                        GrdDichiarato.Visible = True
                        LblDich.Text = ""
                End Select
                Select Case CInt(GrdAccertato.Rows.Count)
                    Case 0
                        GrdAccertato.Visible = False
                        LblAcc.Text = "Nessun Immobile Accertato"
                    Case Is > 0
                        GrdAccertato.Visible = True
                        LblAcc.Text = ""
                End Select
                'è andato tutto a buon fine e quindi svuoto le variabili di sessione dell'accertamento appena effettuato
                Session("oAccertatiGriglia") = Nothing
                Session("oAccertato") = Nothing
                Session("DataSetDichiarazioni") = Nothing
                Session("HashTableRettificaAccertamenti") = Nothing
            End If

        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RiepilogoAccertatoOSAP.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

#Region "Griglie"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdRowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                If CType(sender, Ribes.OPENgov.WebControls.RibesGridView).UniqueID = "GrdDichiarato" Then
                    e.Row.Cells(10).BackColor = Color.PaleGoldenrod
                    e.Row.Cells(10).Font.Bold = True
                Else
                    Dim chkSanzioni As CheckBox
                    Dim idSanzioni, idSanzioniPar, DescSanzione As String
                    Dim arraySanzioni() As String

                    e.Row.Cells(15).BackColor = Color.PaleGoldenrod
                    e.Row.Cells(15).Font.Bold = True


                    chkSanzioni = e.Row.FindControl("chkSanzioni")
                    idSanzioni = CType(e.Row.FindControl("hfIDSANZIONI"), HiddenField).Value
                    If idSanzioni <> "-1" And idSanzioni <> "&nbsp;" Then
                        chkSanzioni.Checked = True
                        arraySanzioni = Split(idSanzioni, "#")
                        idSanzioniPar = arraySanzioni(0)

                        Dim objGestOPENgovProvvedimenti As DBPROVVEDIMENTI.ProvvedimentiDB
                        Dim objDS As DataSet
                        'Creo oggetto ProvvedementiDB per l'esecuzione delle query
                        objGestOPENgovProvvedimenti = New DBPROVVEDIMENTI.ProvvedimentiDB
                        objDS = objGestOPENgovProvvedimenti.GetTipologieSanzioni(ConstSession.CodTributo, idSanzioniPar)
                        DescSanzione = ""
                        If Not IsNothing(objDS) Then
                            If objDS.Tables.Count > 0 Then
                                If objDS.Tables(0).Rows.Count > 0 Then
                                    DescSanzione = UCase(objDS.Tables(0).Rows(0)("DESCRIZIONE"))
                                End If
                            End If
                        End If
                        DescSanzione = DescSanzione.Replace("'", "\'")
                        objDS.Dispose()
                        objGestOPENgovProvvedimenti = Nothing
                        'Motivazione = e.Row.Cells(18).Text()
                        'Motivazione = Motivazione.Replace("'", "\'").Replace("&nbsp;", "").Replace(vbCrLf, "<br>")
                        'chkSanzioni.Attributes.Add("onClick", "return VisualizzaMotivazione('" & DescSanzione & "', '" & Motivazione & "')")
                        'chkSanzioni.ToolTip = "Premere questo pulsante per visualizzare la sanzione e la motivazione associata all'immobile"
                    Else
                        chkSanzioni.Checked = False
                        chkSanzioni.Enabled = False
                    End If
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RiepilogoAccertatoOSAP.GrdRowDataBound.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub GrdAccertato_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAccertato.ItemDataBound
    '    Dim objUtility As New MyUtility
    'Try
    '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '        Dim chkSanzioni As CheckBox
    '        Dim idSanzioni, idSanzioniPar, DescSanzione, Motivazione As String
    '        Dim check As CheckBox
    '        Dim arraySanzioni() As String

    '        e.Item.Cells(16).BackColor = Color.PaleGoldenrod
    '        e.Item.Cells(16).Font.Bold = True


    '        chkSanzioni = e.Item.Cells(14).FindControl("chkSanzioni")
    '        idSanzioni = e.Item.Cells(15).Text
    '        If idSanzioni <> "-1" And idSanzioni <> "&nbsp;" Then
    '            chkSanzioni.Checked = True
    '            arraySanzioni = Split(idSanzioni, "#")
    '            idSanzioniPar = arraySanzioni(0)

    '            Dim objGestOPENgovProvvedimenti As DBPROVVEDIMENTI.ProvvedimentiDB
    '            Dim objDS As DataSet
    '            'Creo oggetto ProvvedementiDB per l'esecuzione delle query
    '            objGestOPENgovProvvedimenti = New DBPROVVEDIMENTI.ProvvedimentiDB
    '            objDS = objGestOPENgovProvvedimenti.GetTipologieSanzioni(ConstSession.CodTributo, idSanzioniPar)
    '            DescSanzione = ""
    '            If Not IsNothing(objDS) Then
    '                If objDS.Tables.Count > 0 Then
    '                    If objDS.Tables(0).Rows.Count > 0 Then
    '                        DescSanzione = UCase(objDS.Tables(0).Rows(0)("DESCRIZIONE"))
    '                    End If
    '                End If
    '            End If
    '            DescSanzione = DescSanzione.Replace("'", "\'")
    '            objDS.Dispose()
    '            objGestOPENgovProvvedimenti = Nothing
    '            Motivazione = e.Item.Cells(18).Text()
    '            Motivazione = Motivazione.Replace("'", "\'").Replace("&nbsp;", "").Replace(vbCrLf, "<br>")
    '            chkSanzioni.Attributes.Add("onClick", "return VisualizzaMotivazione('" & DescSanzione & "', '" & Motivazione & "')")
    '            chkSanzioni.ToolTip = "Premere questo pulsante per visualizzare la sanzione e la motivazione associata all'immobile"
    '        Else
    '            chkSanzioni.Checked = False
    '            chkSanzioni.Enabled = False
    '        End If
    '    End If
    ' Catch ex As Exception
    '    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RiepilogoAccertatoOSAP.GrdAccertato_ItemDataBound.errore: ", ex)
    ' Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub

    'Private Sub GrdDichiarato_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdDichiarato.ItemDataBound
    '    Dim objUtility As New MyUtility
    'Try
    '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '        e.Item.Cells(10).BackColor = Color.PaleGoldenrod
    '        e.Item.Cells(10).Font.Bold = True
    '    End If
    ' Catch ex As Exception
    '    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RiepilogoAccertatoOSAP.GrdDichiarato_ItemDataBound.errore: ", ex)
    ' Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub
#End Region
End Class
