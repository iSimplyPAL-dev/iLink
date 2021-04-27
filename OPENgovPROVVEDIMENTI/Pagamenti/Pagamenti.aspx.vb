Imports Anagrafica.DLL
Imports AnagInterface
Imports log4net
''' <summary>
''' Pagina per la gestione dei pagamenti su accorpamenti/rateizzazioni.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
''' <revisionHistory>
''' <revision date="12/04/2019">
''' <strong>Qualificazione AgID-analisi_rel01</strong>
''' <em>Analisi eventi</em>
''' </revision>
''' </revisionHistory>
Partial Class Pagamenti
    Inherits BasePage

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents fldPagamento As System.Web.UI.HtmlControls.HtmlGenericControl
    Protected WithEvents ddlCapitolo As System.Web.UI.WebControls.DropDownList
    Protected WithEvents ddlVoce As System.Web.UI.WebControls.DropDownList

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    'Private oAnagrafica As GestioneAnagrafica
    Private oDettaglioAnagrafica As DettaglioAnagrafica
    Private WFErrore As String
    Private wucRate As Provvedimenti.usercontrol.WUCRate
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(Pagamenti))

    'Private id_accorpamento_ht, cod_contribuente_ht As Integer
    Private IsChecked As Boolean
    Enum GridPosition
        'posizionamento delle colonne della griglia
        Anno = 0
        numero_atto = 1
        Importo_totale_ridotto = 2
        Importo_totale = 3
        data_elaborazione = 4
        data_notifica_avviso = 5
        Provenienza = 6
        chkSeleziona = 7
        id_provvedimento = 8
        ID_ACCORPAMENTO = 9
        TIPO = 10
    End Enum
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim sFrom As String
        Dim sScript As String = ""
        Dim objDS As DataSet
        Try

            If Not Page.IsPostBack Then
                IsChecked = False
                sFrom = Request.Item("from")
                If Not Request.Item("IdPagamento") Is Nothing Then
                    hfIdPagamento.Value = Request.Item("IdPagamento").ToString
                Else
                    hfIdPagamento.Value = "-1"
                End If
                PopolaCombo()
                If sFrom = "Dettaglio" Then
                    If Not Request.Item("IdAccorpamento") Is Nothing Then
                        hfIdAccorpamento.Value = Request.Item("IdAccorpamento")
                    End If
                    If Not Request.Item("IdProvvedimento") Is Nothing Then
                        hfIdProvvedimento.Value = Request.Item("IdProvvedimento")
                    End If
                    If Not Request.Item("Tipo") Is Nothing Then
                        hfTipo.Value = Request.Item("Tipo")
                    End If
                    If Not Request.Item("IdContribuente") Is Nothing Then
                        hdIdContribuente.Value = Request.Item("IdContribuente")
                    End If
                    'sScript += "viewAnagrafica('none','');"
                    sScript += "parent.Comandi.document.getElementById('btnRicerca').style.display='none';"
                    sScript += "AbilitafldPagamento('');Abilita_btnSalvaPagamento('');"
                    btnSearchProvvedimenti_Click(Nothing, Nothing)
                    btnSelezionaProvvedimenti_Click(Nothing, Nothing)
                Else
                    'sScript += "viewAnagrafica('','none');"
                    sScript += "parent.Comandi.document.getElementById('btnRicerca').style.display='';"
                    sScript += "AbilitafldPagamento('displaynone');Abilita_btnSalvaPagamento('');"
                End If
            Else
                objDS = Session("Provvedimenti")
                If Not IsNothing(objDS) Then
                    ControllaCheckbox()

                    GrdProvvedimenti.DataSource = objDS
                    sScript += "Abilita_btnSalvaPagamento('');"
                    sScript += "AbilitafldPagamento('" & Session("AbilitafldPagamento") & "');"
                End If
            End If
            GetDatiAnagrafica(hdIdContribuente.Value)
            '*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI & DIVERSA GESTIONE QUOTA VARIABILE***
            If ConstSession.HasPlainAnag Then
                sScript += "document.getElementById('TRSpecAnag').style.display='none';"
            Else
                sScript += "document.getElementById('TRPlainAnag').style.display='none';"
            End If
            RegisterScript(sScript, Me.GetType())
            '*** ***
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Pagamenti.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub ControllaCheckbox()
        Dim itemGrid As GridViewRow
        Dim x As Integer
        Dim objDS As DataSet
        Dim dw As DataView
        Dim ChkSelezionato As CheckBox

        Try
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
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Pagamenti.ControllaCheckbox.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
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
        Try

            objPagamenti = New clsPagamenti(ConstSession.StringConnection) 'clsPagamenti(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))
            objDS = objPagamenti.getProvvedimenti(CInt(hdIdContribuente.Value), ConstSession.IdEnte)
            Session("Provvedimenti") = objDS

            If Not IsNothing(objDS) Then
                GrdProvvedimenti.DataSource = objDS.Tables(0).DefaultView
                GrdProvvedimenti.DataBind()
                GrdProvvedimenti.Visible = True
                lblInfoProvv.Text = "Seleziona il provvedimento o l'accorpamento per il quale si deve inserire il pagamento"
                lblInfoProvv.Visible = True
            Else
                lblInfoProvv.Text = "Nessun provvedimento trovato per il contribuente selezionato"
                lblInfoProvv.Visible = True
                GrdProvvedimenti.Visible = False
            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Pagamenti.btnSearchProvvedimenti_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw New Exception(ex.Message, ex)
        Finally
            objPagamenti.kill()
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
                Dim id_provvedimento, ID_ACCORPAMENTO As String
                id_provvedimento = CType(e.Row.FindControl("hfid_provvedimento"), HiddenField).Value
                ID_ACCORPAMENTO = CType(e.Row.FindControl("hfID_ACCORPAMENTO"), HiddenField).Value
                chkSeleziona = e.Row.FindControl("chkSeleziona")
                If IsNumeric(hfIdAccorpamento.Value) Then
                    If CInt(hfIdAccorpamento.Value) > 0 And CInt(hfIdAccorpamento.Value) = CInt(ID_ACCORPAMENTO) Then
                        chkSeleziona.Checked = True
                        IsChecked = True
                        hfIdAccorpamento.Value = ID_ACCORPAMENTO
                        hfIdProvvedimento.Value = id_provvedimento
                    End If
                End If
                'Bottone cancella 
                Dim objbtn As ImageButton
                objbtn = e.Row.FindControl("imgDelete")
                objbtn.ToolTip = "Premere questo Bottone per eliminare l'accorpamento"
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Pagamenti.GrdRowDataBound.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' funzione per la gestione degli eventi sulla griglia
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Analisi eventi</em>
    ''' </revision>
    ''' </revisionHistory>
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim IDRow As String = e.CommandArgument.ToString()
            If e.CommandName = "RowDelete" Then
                For Each myRow As GridViewRow In GrdProvvedimenti.Rows
                    If IDRow = CType(myRow.FindControl("hfID_ACCORPAMENTO"), HiddenField).Value Then
                        Dim objPagamenti As New clsPagamenti(ConstSession.StringConnection)
                        Try
                            If CDbl(myRow.Cells(7).Text) <> 0 Then
                                'esistono già dei pagamenti non è possibile eliminare l'accorpamento
                                Dim stringa As String
                                If CType(myRow.FindControl("hfid_provvedimento"), HiddenField).Value <> 0 Then
                                    stringa = "GestAlert('a', 'warning', '', '', 'Non è possibile eliminare il pagamento');"
                                Else
                                    stringa = "GestAlert('a', 'warning', '', '', 'Non è possibile eliminare l\'accorpamento');"
                                End If
                                RegisterScript(stringa, Me.GetType)
                            Else
                                If Not objPagamenti.deleteAccorpamento(IDRow) Then
                                    Throw New Exception("Errore cancellazione accorpamento")
                                End If
                                Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                                fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Rateizzazione, "GrdRowCommand", Utility.Costanti.AZIONE_DELETE, Utility.Costanti.TRIBUTO_accertaMENTO, ConstSession.IdEnte, IDRow)
                                btnSearchProvvedimenti_Click(Nothing, Nothing)
                            End If
                        Catch ex As Exception
                            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Pagamenti.GrdRowCommand.errore: ", ex)
                            Throw New Exception(ex.Message, ex)
                        Finally
                            objPagamenti.kill()
                            objPagamenti = Nothing
                        End Try
                    End If
                Next
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Pagamenti.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
    '    Try
    '        Dim IDRow As String = e.CommandArgument.ToString()
    '        If e.CommandName = "RowDelete" Then
    '            For Each myRow As GridViewRow In GrdProvvedimenti.Rows
    '                If IDRow = CType(myRow.FindControl("hfID_ACCORPAMENTO"), HiddenField).Value Then
    '                    Dim objPagamenti As New clsPagamenti(ConstSession.StringConnection)
    '                    Try
    '                        If CDbl(myRow.Cells(7).Text) <> 0 Then
    '                            'esistono già dei pagamenti non è possibile eliminare l'accorpamento
    '                            Dim stringa As String
    '                            If CType(myRow.FindControl("hfid_provvedimento"), HiddenField).Value <> 0 Then
    '                                stringa = "GestAlert('a', 'warning', '', '', 'Non è possibile eliminare il pagamento');"
    '                            Else
    '                                stringa = "GestAlert('a', 'warning', '', '', 'Non è possibile eliminare l\'accorpamento');"
    '                            End If
    '                            RegisterScript(stringa, Me.GetType)
    '                        Else
    '                            If Not objPagamenti.deleteAccorpamento(IDRow) Then
    '                                Throw New Exception("Errore cancellazione accorpamento")
    '                            End If
    '                            btnSearchProvvedimenti_Click(Nothing, Nothing)
    '                        End If
    '                    Catch ex As Exception
    '                        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Pagamenti.GrdRowCommand.errore: ", ex)
    '                        ' Response.Redirect("../../PaginaErrore.aspx")
    '                        Throw New Exception(ex.Message, ex)
    '                    Finally
    '                        objPagamenti.kill()
    '                        objPagamenti = Nothing
    '                    End Try
    '                End If
    '            Next
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Pagamenti.GrdRowCommand.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    'Private Sub grdProvvedimenti_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdProvvedimenti.ItemDataBound
    'Try
    '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '        Dim chkSeleziona As CheckBox
    '        Dim id_provvedimento, ID_ACCORPAMENTO As String
    '        id_provvedimento = e.Item.Cells(GridPosition.id_provvedimento).Text
    '        ID_ACCORPAMENTO = e.Item.Cells(GridPosition.ID_ACCORPAMENTO).Text
    '        chkSeleziona = e.Item.Cells(GridPosition.chkSeleziona).FindControl("chkSeleziona")
    '        If id_accorpamento_ht <> 0 And id_accorpamento_ht = CInt(ID_ACCORPAMENTO) Then
    '            chkSeleziona.Checked = True
    '            bChecked = True
    '            txtID_ACCORPAMENTO.Text = ID_ACCORPAMENTO
    '            txtID_PROVVEDIMENTO.Text = id_provvedimento
    '        End If
    '    End If
    'Catch ex As Exception
    '      Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Pagamenti.grdProvvedimenti_ItemDataBound.errore: ", ex)
    '    Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub
#End Region
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub chkSeleziona_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Try
            For Each myRow As GridViewRow In GrdProvvedimenti.Rows
                If CType(myRow.FindControl("chkSeleziona"), CheckBox).Checked Then
                    hfIdProvvedimento.Value = CType(myRow.FindControl("hfid_provvedimento"), HiddenField).Value
                    hfIdAccorpamento.Value = CType(myRow.FindControl("hfID_ACCORPAMENTO"), HiddenField).Value
                    hfTipo.Value = CType(myRow.FindControl("hfTIPO"), HiddenField).Value
                    'inserito qui il richiamo commentato sotto
                    btnSelezionaProvvedimenti_Click(Nothing, Nothing)
                Else
                    hfIdProvvedimento.Value = "0"
                    hfIdAccorpamento.Value = "0"
                    hfTipo.Value = ""
                End If
                'commentato perchè questo richiamo deve avvenire solo nel caso in cui sia la riga corretta ad essere selezionata
                'btnSelezionaProvvedimenti_Click(Nothing, Nothing)
            Next
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Pagamenti.chkSeleziona_CheckedChanged.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="08/05/2019">
    ''' segnalazione 17/VI/19
    ''' </revision>
    ''' </revisionHistory>
    Private Sub btnSelezionaProvvedimenti_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelezionaProvvedimenti.Click
        Dim i As Integer = 0
        Dim x As Integer = 0
        Dim id_provvedimento, id_accorpamento, tipo As String
        Dim sScript As String
        Dim chkSeleziona As CheckBox
        Dim sProvenienza As String
        Dim iNumRate, iRataSel As Integer
        Dim clsGeneralFunction As New MyUtility
        Dim objDS As DataSet
        Dim dw As DataView
        Try
            objDS = Session("Provvedimenti")
            dw = objDS.Tables(0).DefaultView

            iRataSel = -1
            wucRate = Page.FindControl("ElencoRate")
            wucRate.sRatePagate = ""
            wucRate.IdContribuente = hdIdContribuente.Value
            id_provvedimento = hfIdProvvedimento.Value
            id_accorpamento = hfIdAccorpamento.Value
            tipo = hfTipo.Value

            If txtSelezionato.Text = "1" And id_accorpamento = "" Then
                If Not IsNothing(objDS) Then
                    For i = 0 To objDS.Tables(0).Rows.Count - 1
                        objDS.Tables(0).Rows(i).Item("Selezionato") = 0
                    Next
                End If

                For Each myRow As GridViewRow In GrdProvvedimenti.Rows
                    chkSeleziona = myRow.FindControl("chkSeleziona")
                    chkSeleziona.Checked = False
                    chkSeleziona.Enabled = True
                Next
                wucRate.Visible = False
                txtSelezionato.Text = ""
                sScript = "AbilitafldPagamento('displaynone');Abilita_btnSalvaPagamento('none');"
            Else
                For x = 0 To dw.Count - 1
                    If id_accorpamento = "-1" Or tipo = "P" Then
                        'blocco la selezione di altri provvedimenti
                        If dw(x).Item("id_provvedimento").ToString = id_provvedimento Then
                            dw(x).Item("Selezionato") = 1
                            sProvenienza = dw(x).Item("ProvProc")
                        Else
                            dw(x).Item("Selezionato") = 0
                        End If
                    Else
                        'seleziono tutti i provvedimenti che hanno lo stesso accorpamento
                        If dw(x).Item("id_accorpamento").ToString = id_accorpamento Then
                            dw(x).Item("Selezionato") = 1
                            sProvenienza = dw(x).Item("ProvProc")
                        Else
                            dw(x).Item("Selezionato") = 0
                        End If
                    End If
                Next
                Session("Provvedimenti") = objDS
                GrdProvvedimenti.DataSource = dw
                GrdProvvedimenti.DataBind()
                If tipo = "P" Then
                    If IsNumeric(id_provvedimento) Then
                        wucRate.id_provvedimento = id_provvedimento
                        iNumRate = wucRate.getRateProvvedimento(ConstSession.IdEnte)
                        wucRate.txtlegend = "Elenco Rate relative al provvedimento '" & sProvenienza & "'"
                        wucRate.sTipo = tipo
                        If iNumRate > 0 Then
                            wucRate.Visible = True
                            wucRate.viewDelete = True
                            sScript = "AbilitafldPagamento('');Abilita_btnSalvaPagamento('');"
                            Session("AbilitafldPagamento") = ""
                        Else
                            wucRate.Visible = False
                            sScript = "AbilitafldPagamento('');Abilita_btnSalvaPagamento('');"
                            Session("AbilitafldPagamento") = "displaynone"
                        End If
                        txtDataAcc.Text = clsGeneralFunction.GiraDataFromDB(Date.Now.ToString("yyyyMMdd"))
                        txtDataPag.Text = clsGeneralFunction.GiraDataFromDB(Date.Now.ToString("yyyyMMdd"))
                        txtImporto.Text = ImportoTotaleProv(id_provvedimento)
                        If Utility.StringOperation.FormatInt(hfIdPagamento.Value) > 0 Then
                            If Utility.StringOperation.FormatInt(Request.Item("NRataSel")) > 0 Then
                                iRataSel = Utility.StringOperation.FormatInt(Request.Item("NRataSel"))
                                txtDataPag.Text = wucRate.DataPagamentoRata(iRataSel)
                                txtDataAcc.Text = wucRate.DataAccreditoRata(iRataSel)
                                txtImporto.Text = wucRate.ImportoTotaleRata(iRataSel)
                                ddlRata.SelectedValue = wucRate.IdRata(iRataSel)
                                txtRata.Text = wucRate.IdRata(iRataSel)
                                ddlProvenienza.SelectedValue = wucRate.ProvenienzaRata(iRataSel)
                            End If
                        End If
                    End If
                Else
                    If IsNumeric(id_accorpamento) Then
                        wucRate.id_accorpamento = id_accorpamento
                        wucRate.sTipo = tipo
                        iNumRate = wucRate.getRateAccorpamento(ConstSession.IdEnte)
                        wucRate.txtlegend = "Elenco Rate relative all'accorpamento/rateizzazione '" & sProvenienza & "'"
                        wucRate.Visible = True
                        wucRate.viewDelete = True
                        If iNumRate > 0 Then
                            txtRata.Visible = False : ddlRata.Visible = True
                            Dim IsModifica As Integer = 0
                            If Not Request.Item("IsModifica") Is Nothing Then
                                IsModifica = CInt(Request.Item("IsModifica"))
                            End If
                            iRataSel = PopolaComboRate(wucRate.sRatePagate, IsModifica)
                            If Not Request.Item("NRataSel") Is Nothing Then
                                iRataSel = Request.Item("NRataSel")
                                If iRataSel = 0 Then
                                    iRataSel = 1
                                End If
                            End If
                            sScript = "AbilitafldPagamento('');Abilita_btnSalvaPagamento('');"
                            Session("AbilitafldPagamento") = ""
                        Else
                            wucRate.Visible = False
                            txtRata.Visible = True
                            ddlRata.Visible = False
                            sScript = "AbilitafldPagamento('displaynone');Abilita_btnSalvaPagamento('none');"
                            Session("AbilitafldPagamento") = "displaynone"
                        End If

                        txtDataPag.Text = wucRate.DataPagamentoRata(iRataSel)
                        txtDataAcc.Text = wucRate.DataAccreditoRata(iRataSel)
                        txtImporto.Text = wucRate.ImportoTotaleRata(iRataSel)
                        ddlRata.SelectedValue = wucRate.IdRata(iRataSel)
                        ddlProvenienza.SelectedValue = wucRate.ProvenienzaRata(iRataSel)
                    End If
                End If
            End If
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Pagamenti.btnSelezionaProvvedimenti_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw New Exception(ex.Message, ex)
        End Try
    End Sub
    'Private Sub btnSelezionaProvvedimenti_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelezionaProvvedimenti.Click
    '    Dim i As Integer = 0
    '    Dim x As Integer = 0
    '    Dim id_provvedimento, id_accorpamento, tipo As String
    '    Dim sScript As String
    '    Dim chkSeleziona As CheckBox
    '    Dim sProvenienza As String
    '    Dim iNumRate, iRataSel As Integer
    '    Dim clsGeneralFunction As New MyUtility
    '    Dim objDS As DataSet
    '    Dim dw As DataView
    '    Try
    '        objDS = Session("Provvedimenti")
    '        dw = objDS.Tables(0).DefaultView

    '        iRataSel = -1
    '        wucRate = Page.FindControl("ElencoRate")
    '        wucRate.sRatePagate = ""
    '        wucRate.IdContribuente = hdIdContribuente.Value
    '        id_provvedimento = hfIdProvvedimento.Value
    '        id_accorpamento = hfIdAccorpamento.Value
    '        tipo = hfTipo.Value

    '        If txtSelezionato.Text = "1" And id_accorpamento = "" Then
    '            If Not IsNothing(objDS) Then
    '                For i = 0 To objDS.Tables(0).Rows.Count - 1
    '                    objDS.Tables(0).Rows(i).Item("Selezionato") = 0
    '                Next
    '            End If

    '            For Each myRow As GridViewRow In GrdProvvedimenti.Rows
    '                chkSeleziona = myRow.FindControl("chkSeleziona")
    '                chkSeleziona.Checked = False
    '                chkSeleziona.Enabled = True
    '            Next
    '            wucRate.Visible = False
    '            txtSelezionato.Text = ""
    '            'sScript = "document.getElementById ('fldPagamento').className ='displaynone';"
    '            'sScript += "parent.Comandi.document.getElementById ('btnSalvaPagamento').className='displaynone'"
    '            sScript = "AbilitafldPagamento('displaynone');Abilita_btnSalvaPagamento('none');"
    '        Else
    '            For x = 0 To dw.Count - 1
    '                If id_accorpamento = "-1" Or tipo = "P" Then
    '                    'blocco la selezione di altri provvedimenti
    '                    If dw(x).Item("id_provvedimento").ToString = id_provvedimento Then
    '                        dw(x).Item("Selezionato") = 1
    '                        sProvenienza = dw(x).Item("ProvProc")
    '                    Else
    '                        dw(x).Item("Selezionato") = 0
    '                    End If
    '                Else
    '                    'seleziono tutti i provvedimenti che hanno lo stesso accorpamento
    '                    If dw(x).Item("id_accorpamento").ToString = id_accorpamento Then
    '                        dw(x).Item("Selezionato") = 1
    '                        sProvenienza = dw(x).Item("ProvProc")
    '                    Else
    '                        dw(x).Item("Selezionato") = 0
    '                    End If
    '                End If
    '            Next
    '            Session("Provvedimenti") = objDS
    '            GrdProvvedimenti.DataSource = dw
    '            GrdProvvedimenti.DataBind()

    '            'For i = 0 To grdProvvedimenti.Items.Count - 1
    '            '    chkSeleziona = grdProvvedimenti.Items(i).Cells(GridPosition.chkSeleziona).FindControl("chkSeleziona")
    '            '    If id_accorpamento = "-1" Or tipo = "P" Then
    '            '        'blocco la selezione di altri provvedimenti
    '            '        If grdProvvedimenti.Items(i).Cells(GridPosition.id_provvedimento).Text = id_provvedimento Then
    '            '            chkSeleziona.Checked = True
    '            '            chkSeleziona.Enabled = True
    '            '            txtSelezionato.Text = "1"
    '            '            sProvenienza = grdProvvedimenti.Items(i).Cells(GridPosition.numero_atto).Text
    '            '            If InStr(sProvenienza, "nbsp") > 0 Then
    '            '                sProvenienza = ""
    '            '            End If

    '            '            lblImpTotRid = grdProvvedimenti.Items(i).Cells(GridPosition.Importo_totale_ridotto).FindControl("lblImporto_Totale_Ridotto")
    '            '            If lblImpTotRid.Text <> "" Then
    '            '                sImporto = lblImpTotRid.Text
    '            '            Else
    '            '                sImporto = ""
    '            '            End If
    '            '        Else
    '            '            chkSeleziona.Checked = False
    '            '            chkSeleziona.Enabled = False

    '            '        End If
    '            '    Else
    '            '        'seleziono tutti i provvedimenti che hanno lo stesso accorpamento
    '            '        If grdProvvedimenti.Items(i).Cells(GridPosition.ID_ACCORPAMENTO).Text = id_accorpamento Then
    '            '            chkSeleziona.Checked = True
    '            '            chkSeleziona.Enabled = True
    '            '            txtSelezionato.Text = "1"
    '            '            sProvenienza = grdProvvedimenti.Items(i).Cells(GridPosition.Provenienza).Text
    '            '        Else
    '            '            chkSeleziona.Checked = False
    '            '            chkSeleziona.Enabled = False

    '            '        End If
    '            '    End If
    '            'Next
    '            If tipo = "P" Then
    '                If IsNumeric(id_provvedimento) Then
    '                    wucRate.id_provvedimento = id_provvedimento
    '                    iNumRate = wucRate.getRateProvvedimento(ConstSession.IdEnte)
    '                    wucRate.txtlegend = "Elenco Rate relative al provvedimento '" & sProvenienza & "'"
    '                    wucRate.sTipo = tipo
    '                    If iNumRate > 0 Then
    '                        wucRate.Visible = True
    '                        wucRate.viewDelete = True
    '                        sScript = "AbilitafldPagamento('');Abilita_btnSalvaPagamento('');"
    '                        Session("AbilitafldPagamento") = ""
    '                    Else
    '                        wucRate.Visible = False
    '                        'sScript = "AbilitafldPagamento('displaynone');Abilita_btnSalvaPagamento('none');"
    '                        sScript = "AbilitafldPagamento('');Abilita_btnSalvaPagamento('');"
    '                        Session("AbilitafldPagamento") = "displaynone"
    '                    End If
    '                    txtDataAcc.Text = clsGeneralFunction.GiraDataFromDB(Date.Now.ToString("yyyyMMdd"))
    '                    txtDataPag.Text = clsGeneralFunction.GiraDataFromDB(Date.Now.ToString("yyyyMMdd"))
    '                    txtImporto.Text = ImportoTotaleProv(id_provvedimento)
    '                End If
    '            Else
    '                If IsNumeric(id_accorpamento) Then
    '                    wucRate.id_accorpamento = id_accorpamento
    '                    wucRate.sTipo = tipo
    '                    iNumRate = wucRate.getRateAccorpamento(ConstSession.IdEnte)
    '                    wucRate.txtlegend = "Elenco Rate relative all'accorpamento/rateizzazione '" & sProvenienza & "'"
    '                    wucRate.Visible = True
    '                    wucRate.viewDelete = True
    '                    If iNumRate > 0 Then
    '                        txtRata.Visible = False : ddlRata.Visible = True
    '                        Dim IsModifica As Integer = 0
    '                        If Not Request.Item("IsModifica") Is Nothing Then
    '                            IsModifica = CInt(Request.Item("IsModifica"))
    '                        End If
    '                        iRataSel = PopolaComboRate(wucRate.sRatePagate, IsModifica)
    '                        If Not Request.Item("NRataSel") Is Nothing Then
    '                            iRataSel = Request.Item("NRataSel")
    '                            If iRataSel = 0 Then
    '                                iRataSel = 1
    '                            End If
    '                        End If
    '                        sScript = "AbilitafldPagamento('');Abilita_btnSalvaPagamento('');"
    '                        Session("AbilitafldPagamento") = ""
    '                    Else
    '                        wucRate.Visible = False
    '                        txtRata.Visible = True
    '                        ddlRata.Visible = False
    '                        sScript = "AbilitafldPagamento('displaynone');Abilita_btnSalvaPagamento('none');"
    '                        Session("AbilitafldPagamento") = "displaynone"
    '                    End If

    '                    'txtDataAcc.Text = clsGeneralFunction.GiraDataFromDB(Date.Now.ToString("yyyyMMdd"))
    '                    'txtDataPag.Text = clsGeneralFunction.GiraDataFromDB(Date.Now.ToString("yyyyMMdd"))
    '                    txtDataPag.Text = wucRate.DataPagamentoRata(iRataSel)
    '                    txtDataAcc.Text = wucRate.DataAccreditoRata(iRataSel)
    '                    txtImporto.Text = wucRate.ImportoTotaleRata(iRataSel)
    '                    ddlRata.SelectedValue = wucRate.IdRata(iRataSel)
    '                    ddlProvenienza.SelectedValue = wucRate.ProvenienzaRata(iRataSel)
    '                End If
    '            End If
    '            'sScript = "document.getElementById ('fldPagamento').className ='';"
    '            'sScript += "parent.Comandi.document.getElementById ('btnSalvaPagamento').className='BottoneSalva'"
    '        End If
    '        RegisterScript(sScript, Me.GetType())
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Pagamenti.btnSelezionaProvvedimenti_Click.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '        Throw New Exception(ex.Message, ex)
    '    End Try
    'End Sub
    ''' <summary>
    ''' Pulsante per il salvataggio del pagamento.
    ''' Se è un pagamento su un singolo atto ed è il primo pagamento inserisco l'accorpamento fatto da 1 provvedimento e la rateizzazione fatta da 1 rata.
    ''' Altrimenti prelevo i riferimenti di accorpamento e rata e quindi inserisco il pagamento.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Analisi eventi</em>
    ''' </revision>
    ''' </revisionHistory>
    Private Sub btnSalvaPagamento_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalvaPagamento.Click
        Dim i As Integer
        Dim chkSeleziona As CheckBox
        Dim lblImpTotRid As Label
        Dim id_provvedimento, id_accorpamento As Integer
        Dim objPagamenti As New clsPagamenti(ConstSession.StringConnection)
        Dim sScript As String
        Dim objDS As DataSet
        Dim sImporto_tot_ridotto, tipo As String
        Dim sImporto, sProvenienza, sDataPagamento, sDataAccredito, sRata As String
        Dim cod_contribuente As Integer = 0
        Dim id_rata_acc, id_rata_provv, IDPagamento As Integer
        Try
            id_provvedimento = -1 : id_accorpamento = -1 : tipo = ""

            cod_contribuente = CInt(hdIdContribuente.Value)
            IDPagamento = CInt(hfIdPagamento.Value)
            sImporto = txtImporto.Text
            sProvenienza = ddlProvenienza.SelectedValue
            sDataPagamento = txtDataPag.Text
            sDataAccredito = txtDataAcc.Text
            If txtRata.Visible = True Then
                sRata = txtRata.Text
            Else
                sRata = ddlRata.SelectedItem.Text.ToUpper.Replace("RATA ", "")
            End If
            If sRata = "" Then sRata = "0"

            For Each myRow As GridViewRow In GrdProvvedimenti.Rows
                chkSeleziona = myRow.FindControl("chkSeleziona")
                If chkSeleziona.Checked Then
                    tipo = CType(myRow.FindControl("hfTIPO"), HiddenField).Value
                    id_accorpamento = CType(myRow.FindControl("hfID_ACCORPAMENTO"), HiddenField).Value
                    id_provvedimento = CType(myRow.FindControl("hfid_provvedimento"), HiddenField).Value

                    If tipo = "P" Or id_accorpamento = "-1" Then
                        'è un provvedimento singolo
                        lblImpTotRid = myRow.FindControl("lblImporto_Totale_Ridotto")
                        If lblImpTotRid.Text <> "" Then
                            sImporto_tot_ridotto = lblImpTotRid.Text
                        Else
                            'ERRORE
                            sImporto_tot_ridotto = 0
                        End If
                        Exit For
                    Else
                        'è un  accorpamento
                        Exit For
                    End If
                End If
            Next
            If id_provvedimento = -1 And id_accorpamento = -1 Then
                'errore
            Else
                Dim list_id_provvedimento As New ArrayList
                If IDPagamento > 0 Then
                    objPagamenti.GetIdRata(IDPagamento, id_rata_acc, id_rata_provv)
                End If
                If tipo = "P" Then
                    'salvo pagamento per singolo provvedimento
                    If id_accorpamento <= 0 Then
                        'Inserisco in pgm_accorpamento solo la prima volta
                        id_accorpamento = objPagamenti.setAccorpamento(cod_contribuente, id_provvedimento, id_accorpamento, tipo)
                    End If
                    list_id_provvedimento.Add(id_provvedimento)
                    If IDPagamento <= 0 Then
                        id_rata_acc = objPagamenti.setRateAccorpamento(id_accorpamento, sRata, "99991231", sImporto, 0, sImporto)
                        id_rata_provv = objPagamenti.setRateProvvedimento(id_provvedimento, sRata, "99991231", sImporto_tot_ridotto, 0, sImporto_tot_ridotto)
                    End If
                    Dim detthashtable As New Hashtable
                    detthashtable.Add("id_accorpamento", id_accorpamento)
                    detthashtable.Add("cod_contribuente", cod_contribuente)
                    detthashtable.Add("id_provvedimento", id_provvedimento)
                    Session("detthashtable") = detthashtable
                Else
                    'salvo pagamento per accorpamento
                    objDS = objPagamenti.getProvvedimentiAccorpamento(id_accorpamento, 0)
                    If Not IsNothing(objDS) Then
                        If objDS.Tables(0).Rows.Count > 0 Then
                            For i = 0 To objDS.Tables(0).Rows.Count - 1
                                list_id_provvedimento.Add(objDS.Tables(0).Rows(i)("id_provvedimento"))
                            Next
                            If IDPagamento <= 0 Then
                                id_rata_provv = 0
                                id_rata_acc = ddlRata.SelectedValue
                            End If
                        End If
                    Else
                        'errore
                        sScript = "GestAlert('a', 'danger', '', '', 'Errore in inserimento!');"
                        RegisterScript(sScript, Me.GetType())
                        Exit Sub
                    End If
                End If
                If id_rata_acc <= 0 And id_rata_provv <= 0 Then
                    sScript = "GestAlert('a', 'danger', '', '', 'Errore in inserimento!');"
                Else
                    objPagamenti.setPagamento(id_accorpamento, list_id_provvedimento, sDataAccredito, sDataPagamento, sImporto, sProvenienza, sRata, id_rata_provv, id_rata_acc, IDPagamento)
                    Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                    fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Pagamento, "btnSalvaPagamento", Utility.Costanti.AZIONE_NEW, Utility.Costanti.TRIBUTO_accertaMENTO, ConstSession.IdEnte, IDPagamento)
                    sScript = "GestAlert('a', 'success', '', '', 'Inserimento effettuato correttamente!');"
                    sScript += "$('#fldPagamento').addClass('hidden');"
                    sScript += "parent.Comandi.location.href='cmdPagamenti.aspx?from=Dettaglio';"
                    sScript += "parent.Visualizza.location.href='Pagamenti.aspx?from=Dettaglio&IdContribuente=" + cod_contribuente.ToString + "';"
                    'sScript += "parent.Visualizza.location.href='Dettaglio.aspx?IdAccorpamento=" & hfIdAccorpamento.Value & "&IdProvvedimento=" & hfIdProvvedimento.Value & "&IdContribuente=" & cod_contribuente & "';"
                    'sScript += "parent.Comandi.location.href='cmdDettaglio.aspx';"
                End If
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Pagamenti.btnSalvaPagamento_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            objPagamenti.kill()
        End Try
    End Sub
    'Private Sub btnSalvaPagamento_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalvaPagamento.Click
    '    Dim i As Integer
    '    Dim chkSeleziona As CheckBox
    '    Dim lblImpTotRid As Label
    '    Dim id_provvedimento, id_accorpamento As Integer
    '    Dim objPagamenti As clsPagamenti
    '    Dim sScript As String
    '    Dim objDS As DataSet
    '    Dim sImporto_tot_ridotto, tipo As String
    '    Dim sImporto, sProvenienza, sDataPagamento, sDataAccredito, sRata As String
    '    Dim cod_contribuente As Integer = 0
    '    Dim id_rata_acc, id_rata_provv, IDPagamento As Integer
    '    Try
    '        id_provvedimento = -1 : id_accorpamento = -1
    '        objPagamenti = New clsPagamenti(ConstSession.StringConnection) 'clsPagamenti(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))

    '        cod_contribuente = CInt(hdIdContribuente.Value)
    '        IDPagamento = CInt(hfIdPagamento.Value)
    '        sImporto = txtImporto.Text
    '        sProvenienza = ddlProvenienza.SelectedValue
    '        sDataPagamento = txtDataPag.Text
    '        sDataAccredito = txtDataAcc.Text
    '        If txtRata.Visible = True Then
    '            sRata = txtRata.Text
    '        Else
    '            sRata = ddlRata.SelectedItem.Text.ToUpper.Replace("RATA ", "")
    '        End If
    '        If sRata = "" Then sRata = "0"

    '        For Each myRow As GridViewRow In GrdProvvedimenti.Rows
    '            chkSeleziona = myRow.FindControl("chkSeleziona")
    '            If chkSeleziona.Checked Then
    '                tipo = CType(myRow.FindControl("hfTIPO"), HiddenField).Value
    '                id_accorpamento = CType(myRow.FindControl("hfID_ACCORPAMENTO"), HiddenField).Value
    '                id_provvedimento = CType(myRow.FindControl("hfid_provvedimento"), HiddenField).Value

    '                If tipo = "P" Or id_accorpamento = "-1" Then
    '                    'è un provvedimento singolo
    '                    lblImpTotRid = myRow.FindControl("lblImporto_Totale_Ridotto")
    '                    If lblImpTotRid.Text <> "" Then
    '                        sImporto_tot_ridotto = lblImpTotRid.Text
    '                    Else
    '                        'ERRORE
    '                        sImporto_tot_ridotto = 0
    '                    End If
    '                    Exit For
    '                Else
    '                    'è un  accorpamento
    '                    'id_accorpamento = grdProvvedimenti.Items(i).Cells(GridPosition.ID_ACCORPAMENTO).Text
    '                    Exit For
    '                End If
    '            End If
    '        Next
    '        If id_provvedimento = -1 And id_accorpamento = -1 Then
    '            'errore
    '        Else
    '            Dim list_id_provvedimento As New ArrayList
    '            If IDPagamento > 0 Then
    '                objPagamenti.GetIdRata(IDPagamento, id_rata_acc, id_rata_provv)
    '            End If
    '            If tipo = "P" Then
    '                'salvo pagamento per singolo provvedimento
    '                If id_accorpamento <= 0 Then
    '                    'Inserisco in pgm_accorpamento solo la prima volta
    '                    id_accorpamento = objPagamenti.setAccorpamento(cod_contribuente, id_provvedimento, id_accorpamento, tipo)
    '                End If
    '                list_id_provvedimento.Add(id_provvedimento)
    '                If IDPagamento <= 0 Then
    '                    id_rata_acc = objPagamenti.setRateAccorpamento(id_accorpamento, sRata, "99991231", sImporto, 0, sImporto)
    '                    id_rata_provv = objPagamenti.setRateProvvedimento(id_provvedimento, sRata, "99991231", sImporto_tot_ridotto, 0, sImporto_tot_ridotto)
    '                End If
    '                Dim detthashtable As New Hashtable
    '                detthashtable.Add("id_accorpamento", id_accorpamento)
    '                detthashtable.Add("cod_contribuente", cod_contribuente)
    '                detthashtable.Add("id_provvedimento", id_provvedimento)
    '                Session("detthashtable") = detthashtable
    '            Else
    '                'salvo pagamento per accorpamento
    '                objDS = objPagamenti.getProvvedimentiAccorpamento(id_accorpamento, 0)
    '                If Not IsNothing(objDS) Then
    '                    If objDS.Tables(0).Rows.Count > 0 Then
    '                        For i = 0 To objDS.Tables(0).Rows.Count - 1
    '                            list_id_provvedimento.Add(objDS.Tables(0).Rows(i)("id_provvedimento"))
    '                        Next
    '                        If IDPagamento <= 0 Then
    '                            id_rata_provv = 0
    '                            id_rata_acc = ddlRata.SelectedValue
    '                        End If
    '                    End If
    '                Else
    '                    'errore
    '                    sScript = "GestAlert('a', 'danger', '', '', 'Errore in inserimento!');"
    '                    RegisterScript(sScript, Me.GetType())
    '                    Exit Sub
    '                End If
    '                'btnSelezionaProvvedimenti_Click(Nothing, Nothing)
    '            End If
    '            If id_rata_acc <= 0 And id_rata_provv <= 0 Then
    '                sScript = "GestAlert('a', 'danger', '', '', 'Errore in inserimento!');"
    '            Else
    '                objPagamenti.setPagamento(id_accorpamento, list_id_provvedimento, sDataAccredito, sDataPagamento, sImporto, sProvenienza, sRata, id_rata_provv, id_rata_acc, IDPagamento)
    '                sScript = "GestAlert('a', 'success', '', '', 'Inserimento effettuato correttamente!');"
    '                sScript += "parent.Visualizza.location.href='Dettaglio.aspx?IdAccorpamento=" & hfIdAccorpamento.Value & "&IdProvvedimento=" & hfIdProvvedimento.Value & "&IdContribuente=" & cod_contribuente & "';"
    '                sScript += "parent.Comandi.location.href='cmdDettaglio.aspx';"
    '            End If
    '            RegisterScript(sScript, Me.GetType())
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Pagamenti.btnSalvaPagamento_Click.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '        Throw New Exception(ex.Message, ex)
    '    Finally
    '        objPagamenti.kill()
    '    End Try
    'End Sub
    ''' <summary>
    ''' Pulsante per la cancellazione di un pagamento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Analisi eventi</em>
    ''' </revision>
    ''' </revisionHistory>
    Private Sub CmdDeletePagamento_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdDeletePagamento.Click
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dtMyDati As New DataTable()

        Try
            If CInt(hfIdPagamento.Value) > 0 Then
                Try
                    cmdMyCommand = New SqlClient.SqlCommand
                    cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
                    If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                        cmdMyCommand.Connection.Open()
                    End If
                    cmdMyCommand.CommandTimeout = 0
                    cmdMyCommand.CommandType = CommandType.StoredProcedure
                    cmdMyCommand.CommandText = "prc_PagamentoDelete"
                    cmdMyCommand.Parameters.Clear()
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPAGATO", SqlDbType.Int)).Value = CInt(hfIdPagamento.Value)
                    myAdapter.SelectCommand = cmdMyCommand
                    Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                    myAdapter.Fill(dtMyDati)
                Catch ex As Exception
                    Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Pagamenti.CmdDeletePagamento.DelPag.errore: ", ex)
                Finally
                    myAdapter.Dispose()
                    cmdMyCommand.Connection.Close()
                End Try
                Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Pagamento, "CmdDeletePagamento", Utility.Costanti.AZIONE_DELETE, Utility.Costanti.TRIBUTO_accertaMENTO, ConstSession.IdEnte, CInt(hfIdPagamento.Value))
                Dim sScript As String
                sScript = "GestAlert('a', 'success', '', '', 'Pagamento cancellato correttamente!');"
                sScript += "parent.Visualizza.location.href='Dettaglio.aspx?IdAccorpamento=" & hfIdAccorpamento.Value & "&IdProvvedimento=" & hfIdProvvedimento.Value & "&IdContribuente=" & hdIdContribuente.Value & "';"
                sScript += "parent.Comandi.location.href='cmdDettaglio.aspx';"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Pagamenti.CmdDeletePagamento.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub CmdDeletePagamento_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdDeletePagamento.Click
    '    Dim cmdMyCommand As New SqlClient.SqlCommand
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim dtMyDati As New DataTable()
    '    Dim clsGeneralFunction As New MyUtility

    '    Try
    '        If CInt(hfIdPagamento.Value) > 0 Then
    '            Try
    '                cmdMyCommand = New SqlClient.SqlCommand
    '                cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '                If cmdMyCommand.Connection.State = ConnectionState.Closed Then
    '                    cmdMyCommand.Connection.Open()
    '                End If
    '                cmdMyCommand.CommandTimeout = 0
    '                cmdMyCommand.CommandType = CommandType.StoredProcedure
    '                cmdMyCommand.CommandText = "prc_PagamentoDelete"
    '                cmdMyCommand.Parameters.Clear()
    '                cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDPAGATO", SqlDbType.Int)).Value = CInt(hfIdPagamento.Value)
    '                myAdapter.SelectCommand = cmdMyCommand
    '                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '                myAdapter.Fill(dtMyDati)
    '                For Each dtMyRow As DataRow In dtMyDati.Rows
    '                    Dim myID As Integer = dtMyRow("ID")
    '                Next
    '            Catch ex As Exception
    '                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Pagamenti.CmdDeletePagamento.DelPag.errore: ", ex)
    '            Finally
    '                myAdapter.Dispose()
    '                cmdMyCommand.Connection.Close()
    '            End Try
    '            Dim sScript As String
    '            sScript = "GestAlert('a', 'success', '', '', 'Pagamento cancellato correttamente!');"
    '            sScript += "parent.Visualizza.location.href='Dettaglio.aspx?IdAccorpamento=" & hfIdAccorpamento.Value & "&IdProvvedimento=" & hfIdProvvedimento.Value & "&IdContribuente=" & hdIdContribuente.Value & "';"
    '            sScript += "parent.Comandi.location.href='cmdDettaglio.aspx';"
    '            RegisterScript(sScript, Me.GetType())
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Pagamenti.CmdDeletePagamento.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Imagebutton_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Imagebutton.Click
        Try
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
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Pagamenti.Imagebutton_Click.errore: ", ex)
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
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Pagamenti.annoBarra.errore: ", ex)
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
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Pagamenti.FormattaNumero.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnPulisciGriglia_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPulisciGriglia.Click
        Session("Provvedimenti") = Nothing
        GrdProvvedimenti.Visible = False
        lblInfoProvv.Visible = False
        wucRate = Page.FindControl("ElencoRate")
        wucRate.Visible = False
        wucRate.sRatePagate = ""

        txtNominativo.Text = ""
        hdIdContribuente.Value = "-1"
        txtHiddenIdDataAnagrafica.Text = "-1"

        txtDataAcc.Text = ""
        txtDataPag.Text = ""
        txtImporto.Text = ""
        txtRata.Text = ""

        hfIdAccorpamento.Value = "0"
        hfIdProvvedimento.Value = "0"
        hfTipo.Value = ""
        txtSelezionato.Text = ""
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub PopolaCombo()
        Dim Utility As New MyUtility
        Dim objPagamenti As clsPagamenti
        Try
            objPagamenti = New clsPagamenti(ConstSession.StringConnection) 'New clsPagamenti(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))
            'Combo provenienza
            Utility.FillDropDownSQLValueString(ddlProvenienza, objPagamenti.getProvenienza(ConstSession.IdEnte), "", "...")
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Pagamenti.PopolaCombo.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw New Exception(ex.Message, ex)
        Finally
            objPagamenti.kill()
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sElecoRate"></param>
    ''' <param name="IsModifica"></param>
    ''' <returns></returns>
    Private Function PopolaComboRate(ByVal sElecoRate As String, IsModifica As Integer) As Integer
        Dim iCount As Integer
        Dim myListItem As ListItem
        Dim arrElencoRate, arrValori As Array
        Dim bFound As Boolean = False
        Dim iFirstRate As Integer = -1
        Dim id_rata_acc, id_rata_provv, nRata As String
        Try
            ddlRata.Items.Clear()
            If sElecoRate <> "" Then
                sElecoRate = Left(sElecoRate, sElecoRate.Length - 1)
                arrElencoRate = sElecoRate.Split("#")

                For iCount = 0 To arrElencoRate.Length - 1
                    arrValori = arrElencoRate(iCount).split(",")
                    id_rata_acc = arrValori(0)
                    id_rata_provv = arrValori(1)
                    nRata = arrValori(2)
                    'rata non pagata la inserisco nella combo
                    myListItem = New ListItem
                    myListItem.Text = "Rata " & nRata
                    If id_rata_provv <> "0" Then
                        myListItem.Value = id_rata_provv
                    ElseIf id_rata_acc <> "0" Then
                        myListItem.Value = id_rata_acc
                    End If
                    ddlRata.Items.Add(myListItem)
                    If Not bFound And nRata <> "" Then
                        iFirstRate = nRata
                        bFound = True
                    End If
                Next
                If bFound Then
                    ddlRata.Visible = True
                Else
                    'nessuna rata da pagare
                    ddlRata.Visible = False
                    txtRata.Visible = True
                End If
            End If
            Return iFirstRate
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Pagamenti.PopolaComboRate.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw New Exception(ex.Message, ex)
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ddlRata_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlRata.SelectedIndexChanged
        Dim sScript As String
        Try
            wucRate = Page.FindControl("ElencoRate")
            '*** 20120809 - la funzione accetta in ingresso il numero di rata (1,2,3,...) e non l'id univoco nel db ***
            'txtImporto.Text = wucRate.ImportoTotaleRata(ddlRata.SelectedValue)
            txtImporto.Text = wucRate.ImportoTotaleRata(ddlRata.SelectedItem.Text.ToLower.Replace("rata", "").Trim)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Pagamenti.SelectedIndexChanged.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw New Exception(ex.Message, ex)
        Finally
            sScript = "document.getElementById ('fldPagamento').className ='';"
            RegisterScript(sScript, Me.GetType())
        End Try
    End Sub

    '
    'Function GetAnagrafica(ByVal cod_contribuente As Integer)
    '    Dim oAnagrafica As GestioneAnagrafica
    '    Dim oDettaglioAnagrafica As New DettaglioAnagrafica
    '    'Dim WFSessione As OPENUtility.CreateSessione
    '    Try
    '        'WFSessione = New OPENUtility.CreateSessione(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))
    '        'If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '        '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        'End If
    '        'oAnagrafica = New GestioneAnagrafica(WFSessione.oSession, Session("Anagrafica"))
    '        'oDettaglioAnagrafica = oAnagrafica.GetAnagrafica(cod_contribuente, "")
    '        oDettaglioAnagrafica = oAnagrafica.GetAnagrafica(cod_contribuente, -1, ConstSession.DBType, ConstSession.StringConnectionAnagrafica, False)

    '        txtNominativo.Text = oDettaglioAnagrafica.Cognome + " " + oDettaglioAnagrafica.Nome

    '        ViewState.Add("COGNOME", oDettaglioAnagrafica.Cognome)
    '        ViewState.Add("NOME", oDettaglioAnagrafica.Nome)

    '        hdIdContribuente.Value = oDettaglioAnagrafica.COD_CONTRIBUENTE
    '        txtHiddenIdDataAnagrafica.Text = oDettaglioAnagrafica.ID_DATA_ANAGRAFICA
    '        ViewState("sessionName") = ""

    '        btnSearchProvvedimenti_Click(Nothing, Nothing)
    '        If bChecked Then
    '            'se almeno un provvedimento è selezionato carico il relativo dettaglio sulle rate
    '            btnSelezionaProvvedimenti_Click(Nothing, Nothing)
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Pagamenti.GetAnagrafica.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '        Throw New Exception(ex.Message, ex)
    '    Finally
    '        'WFSessione.Kill()
    '        oAnagrafica = Nothing
    '        oDettaglioAnagrafica = Nothing
    '    End Try
    'End Function

    ''' <summary>
    ''' Reperimento dati anagrafici
    ''' </summary>
    ''' <param name="cod_contribuente"></param>
    Private Sub GetDatiAnagrafica(ByVal cod_contribuente As Integer)
        Dim oAnagrafica As New GestioneAnagrafica
        Dim oDettaglioAnagrafica As New DettaglioAnagrafica
        'Dim WFSessione As OPENUtility.CreateSessione
        Try
            'WFSessione = New OPENUtility.CreateSessione(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))
            'If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
            '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
            'End If
            'oAnagrafica = New GestioneAnagrafica(WFSessione.oSession, Session("Anagrafica"))
            'oDettaglioAnagrafica = oAnagrafica.GetAnagrafica(cod_contribuente, "")
            oDettaglioAnagrafica = oAnagrafica.GetAnagrafica(cod_contribuente, -1, String.Empty, ConstSession.DBType, ConstSession.StringConnectionAnagrafica, False)

            hdIdContribuente.Value = oDettaglioAnagrafica.COD_CONTRIBUENTE
            '*** 201504 - Nuova Gestione anagrafica con form unico ***
            If ConstSession.HasPlainAnag Then
                ifrmAnag.Attributes.Add("src", "../../Generali/asp/VisualAnag.aspx?IdContribuente=" + hdIdContribuente.Value + "&Azione=" + Utility.Costanti.AZIONE_NEW.ToString())
            Else
                lblCognomeNome.Text = oDettaglioAnagrafica.Cognome + " " + oDettaglioAnagrafica.Nome
                lblCfPiva.Text = oDettaglioAnagrafica.CodiceFiscale + " " + oDettaglioAnagrafica.PartitaIva
                lblSesso.Text = oDettaglioAnagrafica.Sesso
                lblDataNascita.Text = oDettaglioAnagrafica.DataNascita
                lblComuneNascita.Text = oDettaglioAnagrafica.ComuneNascita
                lblResidenza.Text = oDettaglioAnagrafica.ViaResidenza + " " + oDettaglioAnagrafica.CivicoResidenza + " " + oDettaglioAnagrafica.ComuneResidenza + " (" + oDettaglioAnagrafica.ProvinciaResidenza + ")"
            End If
            If Not Page.IsPostBack Then
                If cod_contribuente > 0 Then
                    btnSearchProvvedimenti_Click(Nothing, Nothing)
                    If IsChecked Then
                        'se almeno un provvedimento è selezionato carico il relativo dettaglio sulle rate
                        btnSelezionaProvvedimenti_Click(Nothing, Nothing)
                    End If
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Pagamenti.GetDatiAnagrafica.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw New Exception(ex.Message, ex)
        Finally
            'WFSessione.Kill()
            oAnagrafica = Nothing
            oDettaglioAnagrafica = Nothing
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="IDProv"></param>
    ''' <returns></returns>
    Private ReadOnly Property ImportoTotaleProv(ByVal IDProv As Integer) As String
        Get
            Dim myVal As Double = 0
            Try
                If IDProv > 0 Then
                    For Each myRow As GridViewRow In GrdProvvedimenti.Rows
                        If CType(myRow.FindControl("hfid_provvedimento"), HiddenField).Value = IDProv Then
                            Double.TryParse(CType(myRow.FindControl("lblImporto_Totale_Ridotto"), Label).Text, myVal)
                        End If
                    Next
                Else
                    Return ""
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Pagamenti.ImportoTotaleProv.errore: ", ex)
                Return ""
            End Try
            Return myVal.ToString
        End Get
    End Property
End Class
