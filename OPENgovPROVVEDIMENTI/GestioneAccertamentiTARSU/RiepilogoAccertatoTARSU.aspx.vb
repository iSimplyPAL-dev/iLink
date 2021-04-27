Imports log4net
Imports RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
''' <summary>
''' Pagina per la visualizzazione del provvedimento generato.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class RiepilogoAccertatoTARSU
    Inherits BaseEnte
    Private Shared Log As ILog = LogManager.GetLogger(GetType(RiepilogoAccertatoTARSU))
    Protected FncGrd As New Formatta.FunctionGrd
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
    Public id_provvedimento As Integer

    '*** 20140701 - IMU/TARES ***
    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    ''Put user code to initialize the page here
    '    Dim sScript As String
    '    'Dim oDichiarato() As OggettoArticoloRuoloAccertamento
    '    'Dim oAccertato() As OggettoArticoloRuoloAccertamento
    '    Dim oDichiarato() As OggettoArticoloRuolo
    '    Dim oAccertato() As OggettoArticoloRuolo
    '    Dim oAttoInserito As OggettoAttoTARSU

    '    id_provvedimento = Request.Item("id_provvedimento")
    '    Dim intRETTIFICATO As Integer = Request.Item("RETTIFICATO")

    '    'carico i comandi
    '    sscript+= "parent.Comandi.location.href='ComandiRiepilogoAccertamentoTARSU.aspx?rettificato=" & intRETTIFICATO & "'"
    '    RegisterScript(sScript , Me.GetType())

    '    Try
    '        LblContribuente.Text = Request.Item("nominativo")
    '        LblAnnoAccertamento.Text = Request.Item("anno")
    '        LblTipoAvviso.Text = "Tipologia: " & CStr(Session("TipoAvviso")).ToUpper

    '        If Page.IsPostBack = False Then
    '            'carico il dichiarato
    '            If Not Session("oSituazioneDichiarato") Is Nothing Then
    '                'oDichiarato = CType(Session("oSituazioneDichiarato"), OggettoArticoloRuoloAccertamento())
    '                oDichiarato = CType(Session("oSituazioneDichiarato"), OggettoArticoloRuolo())
    '                If oDichiarato(0).IdLegame <> 0 Then
    '                    GrdDichiarato.start_index = 0
    '                    GrdDichiarato.DataSource = oDichiarato
    '                    GrdDichiarato.DataBind()
    '                End If
    '                'Session("DataSetDichiarazioni") = oDichiarato
    '            End If
    '            'carico l'accertato
    '            If Not Session("oSituazioneAccertato") Is Nothing Then
    '                'devo caricare l'oggetto ArticoloRuoloAccertamento per popolare la griglia
    '                'oAccertato = CType(Session("oSituazioneAccertato"), OggettoArticoloRuoloAccertamento())
    '                oAccertato = CType(Session("oSituazioneAccertato"), OggettoArticoloRuolo())
    '                If oAccertato(0).IdLegame <> 0 Then
    '                    GrdAccertato.start_index = 0
    '                    GrdAccertato.DataSource = oAccertato
    '                    GrdAccertato.DataBind()
    '                End If
    '                'Session("OggettoRiepilogoAccertamento") = oAccertato
    '            End If
    '            'carico l'atto
    '            If Not Session("oSituazioneAtto") Is Nothing Then
    '                oAttoInserito = CType(Session("oSituazioneAtto"), OggettoAttoTARSU)
    '                LblDiffImpostaAvviso.Text = FormatNumber(oAttoInserito.ImpImposta, 2)
    '                LblSanzAvviso.Text = FormatNumber(oAttoInserito.ImpSanzioni, 2)
    '                LblSanzRidotteAvviso.Text = FormatNumber(oAttoInserito.ImpSanzioniRid, 2)
    '                LblIntAvviso.Text = FormatNumber(oAttoInserito.ImpInteressi, 2)
    '                LblTotAvviso.Text = FormatNumber(oAttoInserito.ImpTotale, 2)
    '            End If

    '            Select Case CInt(GrdDichiarato.Rows.Count)
    '                Case 0
    '                    GrdDichiarato.Visible = False
    '                    LblDich.Text = "Nessun Immobile Dichiarato"
    '                Case Is > 0
    '                    GrdDichiarato.Visible = True
    '                    LblDich.Text = ""
    '            End Select
    '            Select Case CInt(GrdAccertato.Rows.Count)
    '                Case 0
    '                    GrdAccertato.Visible = False
    '                    LblAcc.Text = "Nessun Immobile Accertato"
    '                Case Is > 0
    '                    GrdAccertato.Visible = True
    '                    LblAcc.Text = ""
    '            End Select
    '            'è andato tutto a buon fine e quindi svuoto le variabili di sessione dell'accertamento appena effettuato
    '            Session("oAccertatiGriglia") = Nothing
    '            Session("oAccertato") = Nothing
    '            Session("DataSetDichiarazioni") = Nothing
    '            Session("HashTableDichAccertamentiTARSU") = Nothing
    '            'questa mi server per visualizzare le sanzioni
    '            'Session("oAcceratoXsanzioni") = Nothing
    '        End If

    '    Catch Err As Exception
    '    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RiepilogoAccertatoTARSU.Page_Load.errore: ", Err)
    ' Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ''Put user code to initialize the page here
        Dim sScript As String
        'Dim oDichiarato() As OggettoArticoloRuoloAccertamento
        'Dim oAccertato() As OggettoArticoloRuoloAccertamento
        Dim oDichiarato() As ObjArticoloAccertamento
        Dim oAccertato() As ObjArticoloAccertamento
        Dim oAttoInserito As ComPlusInterface.OggettoAttoTARSU
        Dim objHashTable As Hashtable

        Try
            objHashTable = Session("HashTableDichAccertamentiTARSU")
            id_provvedimento = Request.Item("id_provvedimento")
            Dim intRETTIFICATO As Integer = Request.Item("RETTIFICATO")

            'carico i comandi
            sScript += "parent.Comandi.location.href='ComandiRiepilogoAccertamentoTARSU.aspx?rettificato=" & intRETTIFICATO & "'"
            RegisterScript(sScript, Me.GetType())

            LblContribuente.Text = Request.Item("nominativo")
            LblAnnoAccertamento.Text = Request.Item("anno")
            LblTipoAvviso.Text = "Tipologia: " & CStr(Session("TipoAvviso")).ToUpper

            If Page.IsPostBack = False Then
                'carico il dichiarato
                If Not Session("oSituazioneDichiarato") Is Nothing Then
                    'oDichiarato = CType(Session("oSituazioneDichiarato"), OggettoArticoloRuoloAccertamento())
                    oDichiarato = CType(Session("oSituazioneDichiarato"), ObjArticoloAccertamento())
                    If oDichiarato.Length > 0 Then
                        GrdDichiarato.DataSource = oDichiarato
                        GrdDichiarato.DataBind()
                    End If
                    'Session("DataSetDichiarazioni") = oDichiarato
                End If
                'carico l'accertato
                If Not Session("oSituazioneAccertato") Is Nothing Then
                    'devo caricare l'oggetto ArticoloRuoloAccertamento per popolare la griglia
                    'oAccertato = CType(Session("oSituazioneAccertato"), OggettoArticoloRuoloAccertamento())
                    oAccertato = CType(Session("oSituazioneAccertato"), ObjArticoloAccertamento())
                    If oAccertato(0).IdLegame <> 0 Then
                        GrdAccertato.DataSource = oAccertato
                        GrdAccertato.DataBind()
                    End If
                    'Session("OggettoRiepilogoAccertamento") = oAccertato
                End If
                'carico l'atto
                If Not Session("oSituazioneAtto") Is Nothing Then
                    oAttoInserito = CType(Session("oSituazioneAtto"), ComPlusInterface.OggettoAttoTARSU)
                    LblDiffImpostaAvviso.Text = FormatNumber(oAttoInserito.IMPORTO_DIFFERENZA_IMPOSTA, 2)
                    LblSanzAvviso.Text = FormatNumber(oAttoInserito.IMPORTO_SANZIONI, 2)
                    LblSanzRidotteAvviso.Text = FormatNumber(oAttoInserito.IMPORTO_SANZIONI_RIDOTTE_ACC, 2)
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
                        If objHashTable("TipoTassazione") <> ObjRuolo.TipoCalcolo.TARES Then
                            GrdDichiarato.Columns(7).Visible = False
                            GrdDichiarato.Columns(8).Visible = False
                            GrdDichiarato.Columns(9).Visible = False
                        End If
                End Select
                Select Case CInt(GrdAccertato.Rows.Count)
                    Case 0
                        GrdAccertato.Visible = False
                        LblAcc.Text = "Nessun Immobile Accertato"
                    Case Is > 0
                        GrdAccertato.Visible = True
                        LblAcc.Text = ""
                        If objHashTable("TipoTassazione") <> ObjRuolo.TipoCalcolo.TARES Then
                            GrdAccertato.Columns(7).Visible = False
                            GrdAccertato.Columns(8).Visible = False
                            GrdAccertato.Columns(9).Visible = False
                        End If
                End Select
                'è andato tutto a buon fine e quindi svuoto le variabili di sessione dell'accertamento appena effettuato
                Session("oAccertatiGriglia") = Nothing
                Session("oAccertato") = Nothing
                Session("DataSetDichiarazioni") = Nothing
                Session("HashTableDichAccertamentiTARSU") = Nothing
                'questa mi server per visualizzare le sanzioni
                'Session("oAcceratoXsanzioni") = Nothing
            End If

        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RiepilogoAccertatoTARSU.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** ***

    'Public Function FormattaNumeriGrd(ByVal iNumGrd As Integer) As String
    'Try
    '    If iNumGrd <= 0 Then
    '        Return ""
    '    Else
    '        Return iNumGrd.ToString
    '    End If
    ' Catch Err As Exception
    '     Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RiepilogoAccertatoTARSU.FormattaNumeriGrd.errore: ", Err)
    '   Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Function

    'Public Function CalcolaTotaliGrd(ByVal dDifferenzaImposta As Double, ByVal dInteressi As Double, ByVal dSanzioni As Double) As String
    '    Dim dTotale As Double

    '    dTotale = dDifferenzaImposta + dInteressi + dSanzioni
    '    Return FormatNumber(dTotale, 2)
    'End Function

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
                    '*** 20140701 - IMU/TARES ***
                    e.Row.Cells(14).BackColor = Color.PaleGoldenrod
                    e.Row.Cells(14).Font.Bold = True
                    '*** ***
                Else
                    Dim chkSanzioni As CheckBox
                    Dim idSanzioni, idSanzioniPar, DescSanzione, Motivazione As String
                    Dim arraySanzioni() As String

                    '*** 20140701 - IMU/TARES ***
                    e.Row.Cells(19).BackColor = Color.PaleGoldenrod
                    e.Row.Cells(19).Font.Bold = True
                    chkSanzioni = e.Row.FindControl("chkSanzioni")
                    idSanzioni = CType(e.Row.FindControl("hfSanzioni"), HiddenField).Value
                    '*** ***
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
                        Motivazione = CType(e.Row.FindControl("hfsdescrSANZIONI"), HiddenField).Value
                        Motivazione = Motivazione.Replace("'", "\'").Replace("&nbsp;", "").Replace(vbCrLf, "<br>")
                        chkSanzioni.Attributes.Add("onClick", "return VisualizzaMotivazione('" & DescSanzione & "', '" & Motivazione & "')")
                        chkSanzioni.ToolTip = "Premere questo pulsante per visualizzare la sanzione e la motivazione associata all'immobile"

                        'e.row.Cells(15).Attributes.Add("onClick", "ApriDettaglioSanzioni('" & e.row.Cells(17).Text & "','" & chkSanzioni.ClientID.ToString & "','" & idSanzioniPar & "','1'," & e.row.Cells(18).Text & ")")
                        'e.row.Cells(idCelle.grdSanzioniSanzioni.cellCheckSanzioni).Attributes.Add("onClick", "ApriDettaglioSanzioni('" & e.row.Cells(0).Text & "','" & chkSanzioni.ClientID.ToString & "','" & idSanzioniPar & "','1')")
                    Else
                        chkSanzioni.Checked = False
                        chkSanzioni.Enabled = False
                    End If
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RiepilogoAccertatoTARSU.GrdRowDataBound.errore:" & CType(sender, Ribes.OPENgov.WebControls.RibesGridView).UniqueID, ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub GrdAccertato_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAccertato.ItemDataBound
    '    Dim objUtility As New MyUtility

    '    Try
    '        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '            Dim chkSanzioni As CheckBox
    '            Dim idSanzioni, idSanzioniPar, DescSanzione, Motivazione As String
    '            Dim check As CheckBox
    '            Dim arraySanzioni() As String

    '            '*** 20140701 - IMU/TARES ***
    '            'e.Item.Cells(17).BackColor = Color.PaleGoldenrod
    '            'e.Item.Cells(17).Font.Bold = True
    '            'chkSanzioni = e.Item.Cells(15).FindControl("chkSanzioni")
    '            'idSanzioni = e.Item.Cells(16).Text
    '            e.Item.Cells(20).BackColor = Color.PaleGoldenrod
    '            e.Item.Cells(20).Font.Bold = True
    '            chkSanzioni = e.Item.Cells(18).FindControl("chkSanzioni")
    '            idSanzioni = e.Item.Cells(19).Text
    '            '*** ***
    '            If idSanzioni <> "-1" And idSanzioni <> "&nbsp;" Then
    '                chkSanzioni.Checked = True
    '                arraySanzioni = Split(idSanzioni, "#")
    '                idSanzioniPar = arraySanzioni(0)

    '                Dim objGestOPENgovProvvedimenti As DBPROVVEDIMENTI.ProvvedimentiDB
    '                Dim objDS As DataSet
    '                'Creo oggetto ProvvedementiDB per l'esecuzione delle query
    '                objGestOPENgovProvvedimenti = New DBPROVVEDIMENTI.ProvvedimentiDB
    '                objDS = objGestOPENgovProvvedimenti.GetTipologieSanzioni(ConstSession.CodTributo, idSanzioniPar)
    '                DescSanzione = ""
    '                If Not IsNothing(objDS) Then
    '                    If objDS.Tables.Count > 0 Then
    '                        If objDS.Tables(0).Rows.Count > 0 Then
    '                            DescSanzione = UCase(objDS.Tables(0).Rows(0)("DESCRIZIONE"))
    '                        End If
    '                    End If
    '                End If
    '                DescSanzione = DescSanzione.Replace("'", "\'")
    '                objDS.Dispose()
    '                objGestOPENgovProvvedimenti = Nothing
    '                Motivazione = e.Item.Cells(21).Text()
    '                Motivazione = Motivazione.Replace("'", "\'").Replace("&nbsp;", "").Replace(vbCrLf, "<br>")
    '                chkSanzioni.Attributes.Add("onClick", "return VisualizzaMotivazione('" & DescSanzione & "', '" & Motivazione & "')")
    '                chkSanzioni.ToolTip = "Premere questo pulsante per visualizzare la sanzione e la motivazione associata all'immobile"

    '                'e.Item.Cells(15).Attributes.Add("onClick", "ApriDettaglioSanzioni('" & e.Item.Cells(17).Text & "','" & chkSanzioni.ClientID.ToString & "','" & idSanzioniPar & "','1'," & e.Item.Cells(18).Text & ")")
    '                'e.Item.Cells(idCelle.grdSanzioniSanzioni.cellCheckSanzioni).Attributes.Add("onClick", "ApriDettaglioSanzioni('" & e.Item.Cells(0).Text & "','" & chkSanzioni.ClientID.ToString & "','" & idSanzioniPar & "','1')")
    '            Else
    '                chkSanzioni.Checked = False
    '                chkSanzioni.Enabled = False
    '            End If
    '        End If
    '    Catch ex As Exception
    ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RiepilogoAccertatoTARSU.GrdAccertato_ItemDataBound.errore: ", ex)
    '   Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Private Sub GrdDichiarato_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdDichiarato.ItemDataBound
    '    Try
    '        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '            '*** 20140701 - IMU/TARES ***
    '            'e.Item.Cells(11).BackColor = Color.PaleGoldenrod
    '            'e.Item.Cells(11).Font.Bold = True
    '            e.Item.Cells(14).BackColor = Color.PaleGoldenrod
    '            e.Item.Cells(14).Font.Bold = True
    '            '*** ***
    '        End If
    '    Catch ex As Exception
    ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.RiepilogoAccertatoTARSU.GrdDichiarato_ItemDataBound.errore: ", ex)
    '   Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
#End Region
End Class
