Imports Anagrafica.DLL
Imports AnagInterface
Imports ComPlusInterface
Imports log4net
''' <summary>
''' Pagina dei comandi per la gestione di accorpamenti o rateizzazioni per il pagamento.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
''' <revisionHistory>
''' <revision date="12/04/2019">
''' <strong>Qualificazione AgID-analisi_rel01</strong>
''' <em>Analisi eventi</em>
''' </revision>
''' </revisionHistory>
Partial Class Accorpamenti
    Inherits BasePage
    Enum grdProvvedimentiPosition
        'posizionamento delle colonne della griglia grdProvvedimenti
        anno = 0
        numero_atto = 1
        Desc_Tributo = 2
        Importo_Totale_Ridotto = 3
        Importo_Totale = 4
        data_elaborazione = 5
        data_notifica_avviso = 6
        data_stampa = 7
        data_consegna_avviso = 8
        data_annullamento_avviso = 9
        id_provvedimento = 10
        ID_ACCORPAMENTO = 11
        Gruppo = 12
        COD_TRIBUTO = 13
        chkSeleziona = 14
    End Enum
    Enum GrdRateizzazioniPosition
        'posizionamento delle colonne della griglia GrdRateizzazioni
        Rata = 0
        Importo = 1
        DataScadenza = 2
        Interessi = 3
        Totale = 4
        COD_TIPO_INTERESSE = 5
    End Enum
    'Private oAnagrafica As GestioneAnagrafica
    Private oDettaglioAnagrafica As DettaglioAnagrafica
    Private WFErrore As String

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblRare As System.Web.UI.WebControls.Label


    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    'Protected WithEvents ImpTotRateizzare As System.Web.UI.WebControls.Label
    'Protected WithEvents ImpTotRateizzarePieno As System.Web.UI.WebControls.Label
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(Accorpamenti))

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
        Dim objDS As DataSet
        Dim dw As DataView
        Dim sScript As String = ""
        Try
            If Not Page.IsPostBack Then
                Session("Provvedimenti") = Nothing
                PopolaComboIntervallo()
                PopolaComboInteresse()
                If Not Request.Item("codcontribuente") Is Nothing Then
                    hdIdContribuente.Value = Request.Item("codcontribuente")
                End If
            Else
                objDS = Session("Provvedimenti")
                If Not IsNothing(objDS) Then
                    ControllaCheckbox()
                    dw = objDS.Tables(0).DefaultView
                    If ViewState("SortKey") <> Nothing And ViewState("OrderBy") <> Nothing Then
                        dw.Sort = ViewState("SortKey") & " " & ViewState("OrderBy")
                    End If
                    GrdProvvedimenti.DataSource = dw
                End If
            End If
            '*** 201504 - Nuova Gestione anagrafica con form unico ***
            If ConstSession.HasPlainAnag Then
                ifrmAnag.Attributes.Add("src", "../../Generali/asp/VisualAnag.aspx?IdContribuente=" + hdIdContribuente.Value + "&Azione=" + Utility.Costanti.AZIONE_NEW.ToString())
            Else
                oDettaglioAnagrafica = New DettaglioAnagrafica
                Dim oAnagrafica As New GestioneAnagrafica
                oDettaglioAnagrafica = oAnagrafica.GetAnagrafica(hdIdContribuente.Value, -1, String.Empty, ConstSession.DBType, ConstSession.StringConnectionAnagrafica, False)
                txtNominativo.Text = oDettaglioAnagrafica.Cognome & " " & oDettaglioAnagrafica.Nome
                Session.Remove("DataTableImmobili")
                txtNominativo.Text = oDettaglioAnagrafica.Cognome & " " & oDettaglioAnagrafica.Nome

                ViewState.Add("COGNOME", oDettaglioAnagrafica.Cognome)
                ViewState.Add("NOME", oDettaglioAnagrafica.Nome)

                txtHiddenIdDataAnagrafica.Text = oDettaglioAnagrafica.ID_DATA_ANAGRAFICA
                ViewState("sessionName") = ""
            End If
            If hdIdContribuente.Value > 0 And Session("Provvedimenti") Is Nothing Then
                btnSearchProvvedimenti_Click(Nothing, Nothing)
            End If
            '*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI & DIVERSA GESTIONE QUOTA VARIABILE***
            If ConstSession.HasPlainAnag Then
                sScript += "document.getElementById('TRSpecAnag').style.display='none';"
            Else
                sScript += "document.getElementById('TRPlainAnag').style.display='none';"
            End If
            RegisterScript(sScript, Me.GetType)
            '*** ***
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Accorpamenti.Page_Load.errore: ", ex)
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
                Dim chkSeleziona As CheckBox
                Dim lblDataAnn, lblDataNot As Label

                chkSeleziona = e.Row.FindControl("chkSeleziona")
                lblDataAnn = e.Row.FindControl("lblDataAnn")
                lblDataNot = e.Row.FindControl("lblDataNot")
                If lblDataAnn.Text = "" Then 'And lblDataNot.Text <> "" Then
                    If CType(e.Row.FindControl("hfID_ACCORPAMENTO"), HiddenField).Value = "-1" Then
                        chkSeleziona.Attributes.Add("onclick", "AbilitafldPagamento('none');Abilita_btnCaricaRateizzazioni('none');Abilita_btnCalcolaTotale('none');Abilita_btnSalvaRate('none');")
                    Else
                        'Il provvedimento fa parte di un accorpamento non posso più selezionarlo
                        chkSeleziona.Enabled = False
                        chkSeleziona.Checked = False
                    End If
                Else
                    'E' un avviso annullato non posso selezionarlo
                    chkSeleziona.Enabled = False
                    chkSeleziona.Checked = False
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Accorpamenti.GrdRowDataBound.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub grdProvvedimenti_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdProvvedimenti.ItemDataBound
    'try
    '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '        Dim chkSeleziona As CheckBox
    '        Dim lblDataAnn, lblDataNot As Label

    '        chkSeleziona = e.Item.Cells(grdProvvedimentiPosition.chkSeleziona).FindControl("chkSeleziona")
    '        lblDataAnn = e.Item.Cells(grdProvvedimentiPosition.data_annullamento_avviso).FindControl("lblDataAnn")
    '        lblDataNot = e.Item.Cells(grdProvvedimentiPosition.data_notifica_avviso).FindControl("lblDataNot")
    '        If lblDataAnn.Text = "" Then 'And lblDataNot.Text <> "" Then
    '            If e.Item.Cells(grdProvvedimentiPosition.ID_ACCORPAMENTO).Text = "-1" Then
    '                chkSeleziona.Attributes.Add("onclick", "AbilitafldPagamento('none');Abilita_btnCaricaRateizzazioni('none');Abilita_btnCalcolaTotale('none');Abilita_btnSalvaRate('none');")
    '            Else
    '                'Il provvedimento fa parte di un accorpamento non posso più selezionarlo
    '                chkSeleziona.Enabled = False
    '                chkSeleziona.Checked = False
    '            End If
    '        Else
    '            'E' un avviso annullato non posso selezionarlo
    '            chkSeleziona.Enabled = False
    '            chkSeleziona.Checked = False
    '        End If
    '    End If
    '   Catch ex As Exception
    '   Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Accorpamenti.grdProvvedimenti_ItemDataBound.errore: ", ex)
    '  Response.Redirect("../../PaginaErrore.aspx")
    ' End Try
    'End Sub

    'Private Sub grdProvvedimenti_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles GrdProvvedimenti.SortCommand
    '    Dim objDS As DataSet
    '    Dim dw As DataView
    '    Dim strSortKey As String
    '    Try

    '        If e.SortExpression.ToString() = ViewState("SortKey").ToString() Then
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
    '        objDS = Session("Provvedimenti")
    '        dw = objDS.Tables(0).DefaultView
    '        dw.Sort = ViewState("SortKey") & " " & ViewState("OrderBy")

    '        GrdProvvedimenti.DataSource = dw
    '        GrdProvvedimenti.DataBind()


    '    Catch ex As Exception
    '   Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Accorpamenti.grdProvvedimenti_SortCommand.errore: ", ex)
    '  Response.Redirect("../../PaginaErrore.aspx")
    '        Throw New Exception(ex.Message, ex)
    '    End Try
    'End Sub
#End Region
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub ControllaCheckbox()
        Dim itemGrid As GridViewRow
        Dim x, nSel As Integer
        Dim objDS As DataSet
        Dim dw As DataView
        Dim ChkSelezionato As CheckBox

        Try
            nSel = 0
            objDS = Session("Provvedimenti")
            dw = objDS.Tables(0).DefaultView
            For Each itemGrid In GrdProvvedimenti.Rows
                For x = 0 To dw.Count - 1
                    If dw(x).Item("id_provvedimento").ToString = CType(itemGrid.FindControl("hfid_provvedimento"), HiddenField).Value Then
                        ChkSelezionato = itemGrid.FindControl("chkSeleziona")
                        If ChkSelezionato.Checked Then
                            dw(x).Item("Selezionato") = 1
                        Else
                            dw(x).Item("Selezionato") = 0
                        End If
                    End If
                Next
            Next
            Session("Provvedimenti") = objDS
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Accorpamenti.ControllaCheckbox.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw New Exception(ex.Message, ex)
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnSearchProvvedimenti_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchProvvedimenti.Click
        Dim objPagamenti As clsPagamenti
        Dim objDS As DataSet
        Dim sScript As String

        Try
            objPagamenti = New clsPagamenti(ConstSession.StringConnection) 'clsPagamenti(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))
            objDS = objPagamenti.getProvvedimenti(CInt(hdIdContribuente.Value), ConstSession.IdEnte)
            Session("Provvedimenti") = objDS

            If Not IsNothing(objDS) Then
                ViewState("SortKey") = "cod_tributo,anno"
                ViewState("OrderBy") = "ASC"

                GrdProvvedimenti.DataSource = objDS.Tables(0).DefaultView
                GrdProvvedimenti.DataBind()
                GrdProvvedimenti.Visible = True
                lblInfoProvv.Text = "Selezionare i provvedimenti da rateizzare (Se si selezionano più provvedimenti questi verranno accorpati)"
                lblInfoProvv.Visible = True
                sScript = "Abilita_btnRateizza('')"
            Else
                lblInfoProvv.Text = "Nessun provvedimento trovato per il contribuente selezionato"
                lblInfoProvv.Visible = True
                GrdProvvedimenti.Visible = False
                sScript = "Abilita_btnRateizza('none')"
            End If

            RegisterScript(sScript, Me.GetType)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Accorpamenti.btnSearchProvvedimenti_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw New Exception(ex.Message, ex)
        Finally
            objPagamenti.kill()
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnRateizzaSelezionati_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRateizzaSelezionati.Click
        Dim objDS As DataSet
        Dim i, count As Integer
        Dim ProvSel As objProvvSelezionato
        Dim oProvSel() As objProvvSelezionato
        '*** 20130125 - inserimento gestione pagato anche rispetto all'importo pieno ***
        Dim id_provvedimento, Importo_Totale_Ridotto, Importo_Totale_Pieno As String
        Dim totale_ridotto, sum_totale_ridotto As Double
        Dim totale_pieno, sum_totale_pieno As Double
        '*** ***
        Dim DataNotificaMax, DataNotifica As String
        Dim clsGeneralFunction As New MyUtility
        Try
            count = 0
            '*** 20130125 - inserimento gestione pagato anche rispetto all'importo pieno ***
            sum_totale_pieno = 0
            '*** ***
            sum_totale_ridotto = 0

            DataNotificaMax = "0"

            objDS = Session("Provvedimenti")
            If Not IsNothing(objDS) Then
                For i = 0 To objDS.Tables(0).Rows.Count - 1
                    If objDS.Tables(0).Rows(i).Item("Selezionato") = 1 Then
                        id_provvedimento = objDS.Tables(0).Rows(i).Item("id_provvedimento")
                        '*** 20130125 - inserimento gestione pagato anche rispetto all'importo pieno ***
                        Importo_Totale_Pieno = objDS.Tables(0).Rows(i).Item("Importo_Totale")
                        If Importo_Totale_Pieno <> "" Then
                            totale_pieno = Importo_Totale_Pieno
                        Else
                            totale_pieno = 0
                        End If
                        sum_totale_pieno += totale_pieno
                        '*** ***
                        Importo_Totale_Ridotto = objDS.Tables(0).Rows(i).Item("Importo_Totale_Ridotto")
                        If Importo_Totale_Ridotto <> "" Then
                            totale_ridotto = Importo_Totale_Ridotto
                        Else
                            totale_ridotto = 0
                        End If
                        sum_totale_ridotto += totale_ridotto
                        'Controllo data notifica
                        If Not IsDBNull(objDS.Tables(0).Rows(i).Item("data_notifica_avviso")) Then
                            If objDS.Tables(0).Rows(i).Item("data_notifica_avviso") <> "" Then
                                DataNotifica = objDS.Tables(0).Rows(i).Item("data_notifica_avviso")
                                If DataNotifica > DataNotificaMax Then
                                    DataNotificaMax = DataNotifica
                                End If
                            End If
                        End If

                        ProvSel = New objProvvSelezionato
                        ProvSel.id_Provvedimento = id_provvedimento
                        '*** 20130125 - inserimento gestione pagato anche rispetto all'importo pieno ***
                        ProvSel.Totale_Pieno = totale_pieno
                        '*** ***
                        ProvSel.Totale_Ridotto = totale_ridotto
                        ReDim Preserve oProvSel(count)
                        oProvSel(count) = ProvSel
                        count += 1
                        ProvSel = Nothing
                    End If
                Next

                If Not IsNothing(oProvSel) Then
                    If oProvSel.Length > 0 Then
                        Session("Provvedimenti_selezionati") = oProvSel
                        '*** 20130125 - inserimento gestione pagato anche rispetto all'importo pieno ***
                        OptRidotto.Text = "Totale ridotto da rateizzare € " & FormatNumber(sum_totale_ridotto, 2)
                        OptPieno.Text = "Totale pieno da rateizzare € " & FormatNumber(sum_totale_pieno, 2)
                        'ImpTotRateizzarePieno.Text = FormatNumber(sum_totale_pieno, 2)
                        'ImpTotRateizzare.Text = FormatNumber(sum_totale_ridotto, 2)
                        '*** ***
                        'aggiungo 60 giorni alla data di notifica
                        txtDataInizioInteressi.Text = DateAdd(DateInterval.Day, 60, CDate(clsGeneralFunction.GiraDataFromDB(DataNotificaMax)))
                        'id_provvedimento = Left(id_provvedimento, Len(id_provvedimento) - 1)
                        'txtID_PROVVEDIMENTO.Text = id_provvedimento
                        GrdRateizzazioni.Visible = False
                        RegisterScript("AbilitafldPagamento('');Abilita_btnCaricaRateizzazioni('');Abilita_btnCalcolaTotale('none');", Me.GetType)
                    Else
                        RegisterScript("AbilitafldPagamento('none');Abilita_btnCaricaRateizzazioni('none');Abilita_btnCalcolaTotale('none');GestAlert('a', 'warning', '', '', 'Selezionare almeno un provvedimento')", Me.GetType)
                    End If
                Else
                    RegisterScript("AbilitafldPagamento('none');Abilita_btnCaricaRateizzazioni('none');Abilita_btnCalcolaTotale('none');GestAlert('a', 'warning', '', '', 'Selezionare almeno un provvedimento')", Me.GetType)
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Accorpamenti.btnRateizzaSelezionati_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw New Exception(ex.Message, ex)
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnCreaRate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCreaRate.Click
        Dim oProvSel() As objProvvSelezionato

        Dim nRate, i, cod_tipo_interesse As Integer
        Dim importo, importoRate, valoreInteresse As Double
        Dim objUtility As New MyUtility
        Dim TabellaTotali As New DataTable
        Dim stringaInteresse As String
        Dim posizione As Object
        Dim dDataInizioRate As Date
        Dim bValorizzaDate As Boolean = False
        Try

            txtRateCalc.Text = False

            If txtDataInizioRate.Text <> "" Then
                dDataInizioRate = CDate(txtDataInizioRate.Text)
                bValorizzaDate = True
            Else
                dDataInizioRate = CDate(txtDataInizioInteressi.Text)
                bValorizzaDate = True
            End If

            If ddlInteressi.SelectedValue <> "-1" Then
                stringaInteresse = ddlInteressi.SelectedItem.ToString()
                posizione = stringaInteresse.Split("- ")
                valoreInteresse = posizione(1)
                cod_tipo_interesse = ddlInteressi.SelectedValue
            End If

            nRate = txtNumRate.Text

            oProvSel = Session("Provvedimenti_selezionati")
            '*** 20130125 - inserimento gestione pagato anche rispetto all'importo pieno ***
            If OptPieno.Checked = True Then
                For i = 0 To oProvSel.Length - 1
                    importo += oProvSel(i).Totale_Pieno
                Next
            Else
                For i = 0 To oProvSel.Length - 1
                    importo += oProvSel(i).Totale_Ridotto
                Next
            End If
            '*** ***

            If IsNumeric(nRate) Then
                importoRate = objUtility.cToDbl(importo / nRate)
            End If

            If ddlInteressi.SelectedValue <> "-1" Or nRate > 0 Then
                If TabellaTotali.Rows.Count.CompareTo(1) = -1 Then
                    TabellaTotali.Columns.Add("NUMERO_RATA")
                    TabellaTotali.Columns.Add("IMPORTO_RATA")
                    TabellaTotali.Columns.Add("SCADENZA")
                    TabellaTotali.Columns.Add("IMPORTO_INTERESSI")
                    TabellaTotali.Columns.Add("IMPORTO_TOTALE")
                    TabellaTotali.Columns.Add("COD_TIPO_INTERESSE")

                    Dim ArrCampi(5) As Object
                    For i = 1 To nRate

                        ArrCampi(0) = i.ToString()
                        ArrCampi(1) = importoRate.ToString()
                        If bValorizzaDate Then
                            If i = 1 Then
                                ArrCampi(2) = objUtility.GiraData(dDataInizioRate.ToString())
                            Else
                                If ddlTipoIntervallo.SelectedItem.Value = "d" Then
                                    'giorni
                                    dDataInizioRate = DateAdd(DateInterval.Day, CDbl(ddlIntervallo.SelectedItem.Value), dDataInizioRate)
                                Else
                                    'mesi
                                    dDataInizioRate = DateAdd(DateInterval.Month, CDbl(ddlIntervallo.SelectedItem.Value), dDataInizioRate)
                                End If
                                ArrCampi(2) = objUtility.GiraData(dDataInizioRate.ToString())
                            End If
                        Else
                            ArrCampi(2) = ""
                        End If
                        ArrCampi(3) = -1
                        ArrCampi(4) = -1
                        ArrCampi(5) = cod_tipo_interesse.ToString()

                        TabellaTotali.Rows.Add(ArrCampi)
                    Next
                End If

                GrdRateizzazioni.DataSource = TabellaTotali
                GrdRateizzazioni.DataBind()
                RegisterScript("AbilitafldPagamento('');Abilita_btnCaricaRateizzazioni('');Abilita_btnCalcolaTotale('');Abilita_fldRate('');", Me.GetType)
                GrdRateizzazioni.Visible = True
            Else
                GrdRateizzazioni.Visible = False
                RegisterScript("AbilitafldPagamento('');Abilita_btnCaricaRateizzazioni('');Abilita_btnCalcolaTotale('none')Abilita_fldRate('none');", Me.GetType)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Accorpamenti.btnCreaRate_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw New Exception(ex.Message, ex)
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnCalcolaRate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCalcolaRate.Click
        Dim scadenza, stringaInteresse As String
        Dim posizione As Object
        Dim differenza As Integer
        Dim i, y, nRecord, count As Integer
        Dim importo, totale, valoreInteresse, totInteresse, numero, importo_rata As Double
        Dim interesse As Double
        Dim objUtility As New MyUtility
        Dim strBuilder As String
        Dim oProvSel() As objProvvSelezionato
        Dim DataInizioInteressi As Date
        Dim Accorpamento As objRate
        Dim oAccorpamento(), oAccorpamentoProvv() As objRate

        Try
            DataInizioInteressi = CDate(txtDataInizioInteressi.Text())
            nRecord = GrdRateizzazioni.Rows.Count
            count = 0

            If nRecord <> 0 Then
                'Calcolo rate per accorpamento
                oProvSel = Session("Provvedimenti_selezionati")
                '*** 20130125 - inserimento gestione pagato anche rispetto all'importo pieno ***
                If OptPieno.Checked = True Then
                    For i = 0 To oProvSel.Length - 1
                        importo += oProvSel(i).Totale_Pieno
                    Next
                Else
                    For i = 0 To oProvSel.Length - 1
                        importo += oProvSel(i).Totale_Ridotto
                    Next
                End If
                '*** ***
                importo = objUtility.cToDbl(importo / nRecord)

                If ddlInteressi.SelectedValue <> "-1" Then
                    stringaInteresse = ddlInteressi.SelectedItem.ToString()
                    posizione = stringaInteresse.Split("- ")
                    numero = posizione(1)
                    valoreInteresse = numero
                End If

                Dim NRata As Integer = 0
                For Each myRow As GridViewRow In GrdRateizzazioni.Rows
                    Accorpamento = New objRate
                    NRata += 1

                    scadenza = CType(myRow.FindControl("txtDataScadenza"), TextBox).Text
                    importo_rata = CDbl(CType(myRow.FindControl("lblImportoRate"), Label).Text)
                    If scadenza <> "" Then
                        differenza = DateDiff("d", DataInizioInteressi, scadenza)
                        If differenza < 0 Then
                            'txtRateCalc.Text = False
                            '
                            'sscript+="alert('Le data Scadenza non può essere anteriore alla data odierna');")
                            '
                            'RegisterScript(sScript , Me.GetType())
                            'txtRateCalc.Text = False
                            'Exit For
                            totInteresse = 0
                            interesse = 0
                        Else
                            totInteresse = objUtility.cToDbl((importo_rata * (valoreInteresse / 100) * differenza) / 365)
                            interesse = EuroForGridView(totInteresse)
                        End If

                        'totalePagare = EuroForGridView(totale)
                        totale = objUtility.cToDbl(interesse + importo_rata)
                        CType(myRow.FindControl("txtInteressi"), TextBox).Text = EuroForGridView(totInteresse)
                        CType(myRow.FindControl("txtTotale"), TextBox).Text = EuroForGridView(totale)

                        txtRateCalc.Text = True

                        Accorpamento.NumRata = NRata
                        Accorpamento.DataScadenza = scadenza
                        Accorpamento.ImportoRata = importo_rata
                        Accorpamento.Interessi = interesse
                        Accorpamento.TotaleRata = totale

                        ReDim Preserve oAccorpamento(count)
                        oAccorpamento(count) = Accorpamento
                        count += 1
                        Accorpamento = Nothing
                    Else
                        txtRateCalc.Text = False

                        strBuilder = "AbilitafldPagamento('');Abilita_btnCaricaRateizzazioni('');Abilita_btnCalcolaTotale('');Abilita_btnSalvaRate('none');Abilita_fldRate('none');"
                        strBuilder += "GestAlert('a', 'warning', '', '', 'Inserire tutte le date Scadenza');"
                        RegisterScript(strBuilder, Me.GetType)
                        Exit For
                    End If
                Next

                'Calcolo rate per singolo provvedimento
                For y = 0 To oProvSel.Length - 1
                    '*** 20130125 - inserimento gestione pagato anche rispetto all'importo pieno ***
                    If OptPieno.Checked = True Then
                        importo = oProvSel(y).Totale_Pieno
                    Else
                        importo = oProvSel(y).Totale_Ridotto
                    End If
                    '*** ***
                    oAccorpamentoProvv = Nothing
                    count = 0
                    For Each myRow As GridViewRow In GrdRateizzazioni.Rows
                        Accorpamento = New objRate

                        scadenza = CType(myRow.FindControl("txtDataScadenza"), TextBox).Text
                        importo_rata = objUtility.cToDbl(importo / nRecord)
                        If scadenza <> "" Then
                            differenza = DateDiff("d", Date.Now().Date, scadenza)
                            totInteresse = objUtility.cToDbl((importo_rata * (valoreInteresse / 100) * differenza) / 365)
                            interesse = EuroForGridView(totInteresse)
                            'totalePagare = EuroForGridView(totale)
                            totale = objUtility.cToDbl(interesse + importo_rata)

                            Accorpamento.NumRata = i + 1
                            Accorpamento.DataScadenza = scadenza
                            Accorpamento.ImportoRata = importo_rata
                            Accorpamento.Interessi = interesse
                            Accorpamento.TotaleRata = totale

                            ReDim Preserve oAccorpamentoProvv(count)
                            oAccorpamentoProvv(count) = Accorpamento
                            count += 1
                            Accorpamento = Nothing
                        End If
                    Next

                    oProvSel(y).oAccorpamento = oAccorpamentoProvv
                Next
                Session("Provvedimenti_selezionati") = oProvSel

                If txtRateCalc.Text = True Then
                    Session("oAccorpamento") = oAccorpamento
                    RegisterScript("AbilitafldPagamento('');Abilita_btnCaricaRateizzazioni('');Abilita_btnCalcolaTotale('');Abilita_btnSalvaRate('');Abilita_fldRate('');", Me.GetType)
                End If

            Else
                Dim stringa As String
                txtRateCalc.Text = False

                stringa = "GestAlert('a', 'warning', '', '', 'Cliccare prima il Bottone di Ricerca Rate');"
                stringa += "Abilita_btnSalvaRate('none');"
                RegisterScript(stringa, Me.GetType)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Accorpamenti.btnCalcolaRate_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw New Exception(ex.Message, ex)
        Finally
            If Not IsNothing(Accorpamento) Then Accorpamento = Nothing
        End Try
    End Sub
    ''' <summary>
    ''' Pulsante per il salvataggio informazioni accorpamento. 
    ''' se ho + di un provvedimento TIPO=A{accorpamento} se ho 1 solo provvedimento TIPO=R{rateizzazione}
    ''' Inserimento rate in tbl pgm_rate_accorpamento e  in tbl pgm_rate_Provvedimento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Analisi eventi</em>
    ''' </revision>
    ''' </revisionHistory>
    Private Sub btnSalvaRateizzazioni_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalvaRateizzazioni.Click
        Dim oProvSel() As objProvvSelezionato
        Dim oAccorpamento() As objRate
        Dim objPagamenti As New clsPagamenti(ConstSession.StringConnection)
        Dim id_accorpamento, id_provvedimento, NumRata, i, y As Integer
        Dim ImportoRata, Interessi, TotaleRata, DataScadenza, sTipo As String
        Dim cod_contribuente As Integer
        Dim objUtility As New MyUtility
        Dim stringa As String
        Dim detthashtable As New Hashtable

        Try
            cod_contribuente = CInt(hdIdContribuente.Value)

            oProvSel = Session("Provvedimenti_selezionati")
            oAccorpamento = Session("oAccorpamento")
            'Salvataggio informazioni accorpamento
            id_accorpamento = objPagamenti.GetMaxAccorpamento()
            'Inserimento provvedimenti in tbl pgm_accorpamento
            For i = 0 To oProvSel.Length - 1
                id_provvedimento = oProvSel(i).id_Provvedimento
                '*** ***
                'se ho + di un provvedimento TIPO=A{accorpamento}
                'se ho 1 solo provvedimento TIPO=R{rateizzazione}
                If oProvSel.GetUpperBound(0) = 0 Then
                    sTipo = "R"
                Else
                    sTipo = "A"
                End If
                '*** ***
                objPagamenti.setAccorpamento(cod_contribuente, id_provvedimento, id_accorpamento, sTipo)
            Next

            'Inserimento rate in tbl pgm_rate_accorpamento
            For i = 0 To oAccorpamento.Length - 1
                NumRata = oAccorpamento(i).NumRata
                ImportoRata = oAccorpamento(i).ImportoRata
                DataScadenza = objUtility.GiraData(oAccorpamento(i).DataScadenza)
                Interessi = oAccorpamento(i).Interessi
                TotaleRata = oAccorpamento(i).TotaleRata
                objPagamenti.setRateAccorpamento(id_accorpamento, NumRata, DataScadenza, ImportoRata, Interessi, TotaleRata)
            Next

            'Inserimento rate in tbl pgm_rate_Provvedimento
            For y = 0 To oProvSel.Length - 1
                id_provvedimento = oProvSel(y).id_Provvedimento
                For i = 0 To oProvSel(y).oAccorpamento.Length - 1
                    NumRata = oProvSel(y).oAccorpamento(i).NumRata
                    ImportoRata = oProvSel(y).oAccorpamento(i).ImportoRata
                    DataScadenza = objUtility.GiraData(oProvSel(y).oAccorpamento(i).DataScadenza)
                    Interessi = oProvSel(y).oAccorpamento(i).Interessi
                    TotaleRata = oProvSel(y).oAccorpamento(i).TotaleRata
                    objPagamenti.setRateProvvedimento(id_provvedimento, NumRata, DataScadenza, ImportoRata, Interessi, TotaleRata)
                Next
            Next

            detthashtable.Add("id_accorpamento", id_accorpamento)
            detthashtable.Add("cod_contribuente", cod_contribuente)
            Session("detthashtable") = detthashtable
            Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
            fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Rateizzazione, "btnSalvaRateizzazioni", Utility.Costanti.AZIONE_NEW, Utility.Costanti.TRIBUTO_accertaMENTO, ConstSession.IdEnte, id_accorpamento)
            stringa = "NascondiTuttiBottoni();GestAlert('a', 'success', '', '', 'Salvataggio effettuato correttamente.');document.getElementById('btnSearchProvvedimenti').click();"
            RegisterScript(stringa, Me.GetType)
        Catch ex As Exception
            stringa = "NascondiTuttiBottoni();"
            RegisterScript(stringa, Me.GetType)
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Accorpamenti.btnSalvaRateizzazioni.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            objPagamenti.kill()
        End Try
    End Sub
    'Private Sub btnSalvaRateizzazioni_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalvaRateizzazioni.Click
    '    Dim oProvSel() As objProvvSelezionato
    '    Dim oAccorpamento() As objRate
    '    Dim objPagamenti As clsPagamenti
    '    Dim id_accorpamento, id_provvedimento, NumRata, i, y As Integer
    '    Dim ImportoRata, Interessi, TotaleRata, DataScadenza, sTipo As String
    '    Dim cod_contribuente As Integer
    '    Dim objUtility As New MyUtility
    '    Dim stringa As String
    '    Dim detthashtable As New Hashtable
    '    Try
    '        objPagamenti = New clsPagamenti(ConstSession.StringConnection) 'clsPagamenti(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))

    '        cod_contribuente = CInt(hdIdContribuente.Value)

    '        oProvSel = Session("Provvedimenti_selezionati")
    '        oAccorpamento = Session("oAccorpamento")
    '        'Salvataggio informazioni accorpamento

    '        id_accorpamento = objPagamenti.GetMaxAccorpamento()
    '        'Inserimento provvedimenti in tbl pgm_accorpamento
    '        For i = 0 To oProvSel.Length - 1
    '            id_provvedimento = oProvSel(i).id_Provvedimento
    '            '*** ***
    '            'se ho + di un provvedimento TIPO=A{accorpamento}
    '            'se ho 1 solo provvedimento TIPO=R{rateizzazione}
    '            If oProvSel.GetUpperBound(0) = 0 Then
    '                sTipo = "R"
    '            Else
    '                sTipo = "A"
    '            End If
    '            '*** ***
    '            objPagamenti.setAccorpamento(cod_contribuente, id_provvedimento, id_accorpamento, sTipo)
    '        Next

    '        'Inserimento rate in tbl pgm_rate_accorpamento
    '        For i = 0 To oAccorpamento.Length - 1
    '            NumRata = oAccorpamento(i).NumRata
    '            ImportoRata = oAccorpamento(i).ImportoRata
    '            DataScadenza = objUtility.GiraData(oAccorpamento(i).DataScadenza)
    '            Interessi = oAccorpamento(i).Interessi
    '            TotaleRata = oAccorpamento(i).TotaleRata
    '            objPagamenti.setRateAccorpamento(id_accorpamento, NumRata, DataScadenza, ImportoRata, Interessi, TotaleRata)
    '        Next

    '        'Inserimento rate in tbl pgm_rate_Provvedimento
    '        For y = 0 To oProvSel.Length - 1
    '            id_provvedimento = oProvSel(y).id_Provvedimento
    '            For i = 0 To oProvSel(y).oAccorpamento.Length - 1
    '                NumRata = oProvSel(y).oAccorpamento(i).NumRata
    '                ImportoRata = oProvSel(y).oAccorpamento(i).ImportoRata
    '                DataScadenza = objUtility.GiraData(oProvSel(y).oAccorpamento(i).DataScadenza)
    '                Interessi = oProvSel(y).oAccorpamento(i).Interessi
    '                TotaleRata = oProvSel(y).oAccorpamento(i).TotaleRata
    '                objPagamenti.setRateProvvedimento(id_provvedimento, NumRata, DataScadenza, ImportoRata, Interessi, TotaleRata)
    '            Next
    '        Next

    '        detthashtable.Add("id_accorpamento", id_accorpamento)
    '        detthashtable.Add("cod_contribuente", cod_contribuente)
    '        Session("detthashtable") = detthashtable

    '        'stringa = "NascondiTuttiBottoni();InserimentoPagamenti();"
    '        stringa = "NascondiTuttiBottoni();GestAlert('a', 'success', '', '', 'Salvataggio effettuato correttamente.');document.getElementById('btnSearchProvvedimenti').click();"
    '        'sscript+="alert('Salvataggio effettuato correttamente');")
    '        RegisterScript(stringa, Me.GetType)
    '        'btnSearchProvvedimenti_Click(Nothing, Nothing)
    '    Catch ex As Exception
    '        stringa = "NascondiTuttiBottoni();"
    '        RegisterScript(stringa, Me.GetType)
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Accorpamenti.btnSalvaRateizzazioni.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '        Throw New Exception(ex.Message, ex)
    '    Finally
    '        objPagamenti.kill()
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Imagebutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Imagebutton.Click
        Try
            'If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
            '	Response.Write(WFErrore)
            '	Response.End()
            'End If
            txtNominativo.Text = ""
            'Dim WFSessione As New OPENUtility.CreateSessione(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))
            'If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
            '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
            'End If

            Session.Remove(ViewState("sessionName"))

            'oAnagrafica = New GestioneAnagrafica(WFSessione.oSession, Session("Anagrafica"))
            oDettaglioAnagrafica = New DettaglioAnagrafica
            If hdIdContribuente.Value = "" Then
                oDettaglioAnagrafica.COD_CONTRIBUENTE = -1
            Else
                oDettaglioAnagrafica.COD_CONTRIBUENTE = hdIdContribuente.Value
            End If

            oDettaglioAnagrafica.ID_DATA_ANAGRAFICA = txtHiddenIdDataAnagrafica.Text
            ViewState("sessionName") = "codContribuente"
            Session(ViewState("sessionName")) = oDettaglioAnagrafica
            writeJavascriptAnagrafica(ViewState("sessionName"))
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Accorpamenti.Imagebutton_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")

        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nomeSessione"></param>
    Private Sub writeJavascriptAnagrafica(ByVal nomeSessione As String)
        Dim sScript As String = ""

        sScript += ""
        sScript += "ApriRicercaAnagrafe('" & nomeSessione & "');" & vbCrLf
        RegisterScript(sScript, Me.GetType())
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnRibalta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRibalta.Click
        'Azzero la varibile di sessione dell'accertato se ribalto i dati
        'di un nuovo contribuente da accertare
        Session.Remove("DataTableImmobili")
        oDettaglioAnagrafica = Session(ViewState("sessionName"))

        txtNominativo.Text = oDettaglioAnagrafica.Cognome & " " & oDettaglioAnagrafica.Nome

        ViewState.Add("COGNOME", oDettaglioAnagrafica.Cognome)
        ViewState.Add("NOME", oDettaglioAnagrafica.Nome)

        hdIdContribuente.Value = oDettaglioAnagrafica.COD_CONTRIBUENTE
        txtHiddenIdDataAnagrafica.Text = oDettaglioAnagrafica.ID_DATA_ANAGRAFICA
        ViewState("sessionName") = ""

        btnSearchProvvedimenti_Click(Nothing, Nothing)
        'txtCerca.Text = "1"
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="objtemp"></param>
    ''' <returns></returns>
    Protected Function annoBarra(ByVal objtemp As Object) As String
        Dim clsGeneralFunction As New MyUtility
        Dim strTemp As String = ""
        Try
            If Not IsDBNull(objtemp) Then
                If CStr(objtemp).CompareTo("") <> 0 Then
                    strTemp = clsGeneralFunction.GiraDataFromDB(objtemp)
                Else
                    strTemp = ""
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Accorpamenti.annoBara.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
        Return strTemp
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="NumeroDaFormattareParam"></param>
    ''' <param name="numDec"></param>
    ''' <returns></returns>
    Protected Function FormattaNumero(ByVal NumeroDaFormattareParam As String, ByVal numDec As Integer) As String
        FormattaNumero = ""
        Try
            If IsDBNull(NumeroDaFormattareParam) Or NumeroDaFormattareParam = "" Or NumeroDaFormattareParam = "-1" Or NumeroDaFormattareParam = "-1,00" Then
                NumeroDaFormattareParam = ""
            Else
                FormattaNumero = FormatNumber(NumeroDaFormattareParam, numDec)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Accorpamenti.FormattaNumero.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="cod_tributo"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="04/07/2012">
    ''' <strong>IMU</strong>
    ''' passaggio tributo da ICI a IMU
    ''' </revision>
    ''' </revisionHistory>
    Protected Function DescTributo(ByVal cod_tributo As String) As String
        DescTributo = ""
        '*** 20120704 - IMU ***
        Try
            Select Case cod_tributo
                Case Utility.Costanti.TRIBUTO_ICI : DescTributo = "ICI/IMU"
                Case Utility.Costanti.TRIBUTO_TARSU : DescTributo = "TARSU/TARES/TARI"
                Case Utility.Costanti.TRIBUTO_OSAP : DescTributo = "TOSAP/COSAP"
                Case "0465" : DescTributo = "TIA"
                Case Else : DescTributo = ""
            End Select
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Accorpamenti.DescTributo.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="iInput"></param>
    ''' <returns></returns>
    Protected Function EuroForGridView(ByVal iInput As Object) As String
        Dim ret As String = String.Empty

        Try
            If (iInput.ToString() = "-1") Or (iInput.ToString() = "-1,00") Then
                ret = "0"
            Else
                ret = Convert.ToDecimal(iInput).ToString("N")
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Accorpamenti.EuroForGridView.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            ret = "0"
        End Try
        Return ret
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="data"></param>
    ''' <returns></returns>
    Protected Function GiraDataFromDB(ByVal data As Object) As String
        'leggo la data nel formato aaaammgg  e la metto nel formato gg/mm/aaaa
        Dim Giorno As String
        Dim Mese As String
        Dim Anno As String
        Dim objUtility As New MyUtility

        GiraDataFromDB = ""
        data = objUtility.CToStr(data)
        Try
            If Not IsDBNull(data) And Not IsNothing(data) Then

                If data <> "" Then
                    Giorno = Mid(data, 7, 2)
                    Mese = Mid(data, 5, 2)
                    Anno = Mid(data, 1, 4)
                    GiraDataFromDB = Giorno & "/" & Mese & "/" & Anno
                Else
                    GiraDataFromDB = ""
                End If

                If IsDate(GiraDataFromDB) = False And GiraDataFromDB <> "" Then
                    Giorno = Mid(data, 7, 2)
                    Mese = Mid(data, 5, 2)
                    Anno = Mid(data, 1, 4)
                    GiraDataFromDB = Mese & "/" & Giorno & "/" & Anno
                End If
            End If
            Return GiraDataFromDB
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Accorpamenti.GiraDataFromDB.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub PopolaComboIntervallo()
        Dim i As Integer
        Try
            For i = 1 To 30
                ddlIntervallo.Items.Add(New ListItem(i, i))
            Next

            ddlTipoIntervallo.Items.Add(New ListItem("Giorni", "d"))
            ddlTipoIntervallo.Items.Add(New ListItem("Mesi", "m"))

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Accorpamenti.PopolaComboIntervallo.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub PopolaComboInteresse()

        Dim dw As DataView
        Dim objDSTipiInteressi As DataSet
        ''Dim objSessione As CreateSessione
        'Dim strConnectionStringOPENgovProvvedimenti As String
        Dim objHashTable As Hashtable = New Hashtable
        Dim i As Integer
        Dim stringa, valore, al As String

        Try
            'objSessione = New CreateSessione(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))

            'If Not objSessione.CreaSessione(ConstSession.UserName, strWFErrore) Then
            '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
            'End If

            'carico la hash table
            'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString
            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
            objHashTable.Add("USER", ConstSession.UserName)
            objHashTable.Add("CODENTE", ConstSession.IdEnte)

            objHashTable.Add("CODTIPOINTERESSE", "-1")
            objHashTable.Add("DAL", "")
            objHashTable.Add("AL", "")
            objHashTable.Add("TASSO", "")
            objHashTable.Add("CODTRIBUTO", "")

            Dim objCOMTipoVoci As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)
            objDSTipiInteressi = objCOMTipoVoci.GetTipoInteresse(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable)
            If Not objDSTipiInteressi Is Nothing Then
                dw = objDSTipiInteressi.Tables(0).DefaultView
            End If

            ddlInteressi.Items.Add(New ListItem("...", "-1"))
            For i = 0 To dw.Table.Rows.Count - 1
                al = dw.Table.Rows(i)("AL").ToString()
                If al = "" Then
                    stringa = dw.Table.Rows(i)("DESCRIZIONE").ToString() & " - " & dw.Table.Rows(i)("TASSO_ANNUALE").ToString()
                    valore = dw.Table.Rows(i)("COD_TIPO_INTERESSE").ToString()
                    ddlInteressi.Items.Add(New ListItem(stringa, valore))
                End If
            Next

        Catch ex As Exception
            'If Not IsNothing(objSessione) Then
            '    objSessione.Kill()
            '    objSessione = Nothing
            'End If
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Accorpamenti.PopolaComboInteresse.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw New Exception(ex.Message, ex)
            'Finally
            '    If Not IsNothing(objSessione) Then
            '        objSessione.Kill()
            '        objSessione = Nothing
            '    End If
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnNascondiGrdRateizzazioni_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNascondidgdRateizzazioni.Click
        GrdRateizzazioni.Visible = False
        RegisterScript("AbilitafldPagamento('');", Me.GetType)
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnPulisciGriglia_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPulisciGriglia.Click
        Session("Provvedimenti") = Nothing
        GrdProvvedimenti.Visible = False
        lblInfoProvv.Visible = False
        txtNominativo.Text = ""
        hdIdContribuente.Value = "-1"
        txtHiddenIdDataAnagrafica.Text = "-1"
        txtID_ACCORPAMENTO.Text = ""
        txtID_PROVVEDIMENTO.Text = ""
        txtSelezionato.Text = ""
    End Sub
End Class
''' <summary>
''' Definizione oggetto provvedimento selezionato
''' </summary>
Public Class objProvvSelezionato
    Public Sub New()
    End Sub

    Private _IdProvvedimento As Integer = -1
	'*** 20130125 - inserimento gestione pagato anche rispetto all'importo pieno ***
	Private _Totale_Pieno As Double = -1
	'*** ***
	Private _Totale_Ridotto As Double = -1
    Private _oAccorpamento() As objRate

    Public Property id_Provvedimento() As Integer
        Get
            Return _IdProvvedimento
        End Get
        Set(ByVal Value As Integer)
            _IdProvvedimento = Value
        End Set
    End Property
	'*** 20130125 - inserimento gestione pagato anche rispetto all'importo pieno ***
	Public Property Totale_Pieno() As Double
		Get
			Return _Totale_pieno
		End Get
		Set(ByVal Value As Double)
			_Totale_pieno = Value
		End Set
	End Property
	'*** ***
	Public Property Totale_Ridotto() As Double
		Get
			Return _Totale_Ridotto
		End Get
		Set(ByVal Value As Double)
			_Totale_Ridotto = Value
		End Set
	End Property

	Public Property oAccorpamento() As objRate()
		Get
			Return _oAccorpamento
		End Get
		Set(ByVal Value As objRate())
			_oAccorpamento = Value
		End Set
	End Property

End Class
''' <summary>
''' Definizione oggetto rate
''' </summary>
Public Class objRate
    Public Sub New()
    End Sub

    Private _NumRata As Integer = -1
    Private _ImportoRata As Double = -1
    Private _DataScadenza As String = ""
    Private _Interessi As Double = -1
    Private _TotaleRata As Double = -1

    Public Property NumRata() As Integer
        Get
            Return _NumRata
        End Get
        Set(ByVal Value As Integer)
            _NumRata = Value
        End Set
    End Property

    Public Property ImportoRata() As Double
        Get
            Return _ImportoRata
        End Get
        Set(ByVal Value As Double)
            _ImportoRata = Value
        End Set
    End Property

    Public Property DataScadenza() As String
        Get
            Return _DataScadenza
        End Get
        Set(ByVal Value As String)
            _DataScadenza = Value
        End Set
    End Property

    Public Property Interessi() As Double
        Get
            Return _Interessi
        End Get
        Set(ByVal Value As Double)
            _Interessi = Value
        End Set
    End Property

    Public Property TotaleRata() As Double
        Get
            Return _TotaleRata
        End Get
        Set(ByVal Value As Double)
            _TotaleRata = Value
        End Set
    End Property
End Class