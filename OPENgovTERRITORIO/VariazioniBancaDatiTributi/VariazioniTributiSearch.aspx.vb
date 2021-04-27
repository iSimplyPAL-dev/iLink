Imports log4net
''' <summary>
''' Pagina per la consultazione/gestione delle modifiche tributarie.
''' Contiene i parametri di ricerca, le funzioni della comandiera e la griglia per la visualizzazione del risultato. 
''' Le possibili opzioni sono:
''' - Stampa
''' - Setta come trattato
''' - Ricerca
''' </summary>
Public Class VariazioniTributiSearch
    Inherits BaseEnte
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(VariazioniTributiSearch))

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    'Protected WithEvents FrmVariazionitributiSearch As System.Web.UI.HtmlControls.HtmlForm
    'Protected WithEvents lblTitolo As System.Web.UI.WebControls.Label
    'Protected WithEvents Stampa As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents SetTrattato As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents Search As System.Web.UI.WebControls.ImageButton
    'Protected WithEvents TblParametri As System.Web.UI.WebControls.Panel
    'Protected WithEvents RbNonTrattato As System.Web.UI.WebControls.RadioButton
    'Protected WithEvents RbTrattato As System.Web.UI.WebControls.RadioButton
    'Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    'Protected WithEvents DdlTributo As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    'Protected WithEvents TxtFoglio As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    'Protected WithEvents TxtNumero As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    'Protected WithEvents TxtSub As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    'Protected WithEvents TxtDal As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    'Protected WithEvents TxtAl As System.Web.UI.WebControls.TextBox
    'Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    'Protected WithEvents DdlOperatore As System.Web.UI.WebControls.DropDownList
    'Protected WithEvents LblResultdichiarazioni As System.Web.UI.WebControls.Label
    'Protected WithEvents GrdResult As RIDataGrid.RibesGrid

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            lblTitolo.Text = Session("DESCRIZIONE_ENTE")
            If Not Page.IsPostBack Then
                Dim oLoadCombo As New ClsUtilities
                Dim oListParam() As SearchParameter
                Dim oSingleParam As SearchParameter
                ReDim Preserve oListParam(0)
                oSingleParam = New SearchParameter
                If Not Request.Item("ente") Is Nothing Then
                    Session("cod_ente") = Request.Item("ente")
                End If
                oSingleParam.Value = ConstSession.idente
                oSingleParam.Name = "@IdEnte"
                oSingleParam.Direction = ParameterDirection.Input
                oListParam(0) = oSingleParam
                oLoadCombo.LoadComboGenerale(DdlOperatore, ConstSession.StringConnectionOPENgov, "prc_GetUtentiEnte", oListParam)
                oLoadCombo.LoadComboGenerale(DdlTributo, ConstSession.StringConnectionOPENgov, "prc_GetTributi", Nothing)
                oLoadCombo.LoadComboGenerale(Ddlcausali, ConstSession.StringConnectionOPENgov, "prc_GetCausaliVarTrib", Nothing)
                RestoreSearch()
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTERRITORIO.VariazioniTributiSearch.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

#Region "Griglie"
    Protected Sub GrdRowCommand(ByVal sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim sScript As String
            Dim IDRow As Integer = CInt(e.CommandArgument.ToString())
            If e.CommandName = "RowOpen" Then
                For Each myRow As GridViewRow In GrdResult.Rows
                    If IDRow = CType(myRow.FindControl("hfId"), HiddenField).Value Then
                        sScript = "parent.parent.Nascosto.location.href='../../aspVuota.aspx';"
                        If CInt(CType(myRow.FindControl("hfIdUI"), HiddenField).Value) <= -1 And CInt(CType(myRow.FindControl("hfIdClass"), HiddenField).Value) <= -1 Then
                            sScript += "parent.parent.Comandi.location.href='../Fabbricati/Gestione Fabbricati/ComandiInserimento.aspx';"
                            sScript += "location.href ='../Fabbricati/Gestione Fabbricati/GestFabbricato.aspx?IdVariazioneTributaria=" + CType(myRow.FindControl("hfIdVar"), HiddenField).Value + "&Provenienza=I';"
                        Else
                            If CType(myRow.FindControl("hfIdVar"), HiddenField).Value.ToUpper = MetodiVariazioniTributi.VariazioneSuClass Then
                                sScript += "parent.parent.Comandi.location.href='../Fabbricati/Gestione Fabbricati/Classificazioni/ComandiClassificazioni.aspx';"
                                sScript += "location.href ='../Fabbricati/Gestione Fabbricati/Classificazioni/GestClassificazioni.aspx?IdVariazioneTributaria=" + CType(myRow.FindControl("hfIdVar"), HiddenField).Value + "&CodUI=" + CType(myRow.FindControl("hfIdUI"), HiddenField).Value + "&CodClas=" + CType(myRow.FindControl("hfIdClass"), HiddenField).Value + "';"
                            Else
                                sScript += "parent.parent.Comandi.location.href='../Fabbricati/Gestione Fabbricati/ComandiSituazioneUI.aspx';"
                                sScript += "location.href ='../Fabbricati/Gestione Fabbricati/GestUI.aspx?IdVariazioneTributaria=" + CType(myRow.FindControl("hfIdVar"), HiddenField).Value + "&CodUI=" + CType(myRow.FindControl("hfIdUI"), HiddenField).Value + "';"
                            End If
                        End If
                        RegisterScript(sScript, Me.GetType())
                    End If
                Next
            End If
        Catch ex As Exception
            Log.Debug("OPENgovTERRITORIO.VariazioniTributiSearch.GrdRowCommand::errore::", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Gestione del cambio pagina della griglia.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdPageIndexChanging(ByVal sender As Object, e As GridViewPageEventArgs)
        DataBindGridResult(e.NewPageIndex)
    End Sub
    'Private Sub GrdResult_EditCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GrdResult.EditCommand
    '    Try
    '        Dim CellIdVar As TableCell = ClsUtilities.CellByName(e.Item, "Id")
    '        Dim CellIdUI As TableCell = ClsUtilities.CellByName(e.Item, "IdRifUI")
    '        Dim CellIdClass As TableCell = ClsUtilities.CellByName(e.Item, "IdRifClass")
    '        Dim CellTipoVar As TableCell = ClsUtilities.CellByName(e.Item, "TipoVariazione")

    '        'apro il popup passandogli l'indice dell'array da visualizzare
    '        'ShowPopUp("I", CInt(e.Item.Cells(9).Text), CInt(e.Item.Cells(11).Text), ConstSession.AZIONE_LETTURA)
    '        'sScript = "LoadClass('" & CellIdVar.Text & "','" & CellIdUI.Text & "','" & CellIdClass.Text & "')"
    '        Dim sScript As String
    '        Dim sBuilder As New System.Text.StringBuilder
    '        sScript = "<script language='javascript'>"
    '        sScript += "parent.parent.Nascosto.location.href='../Generali/asp/aspVuota.aspx';"
    '        If CInt(CellIdUI.Text) <= -1 And CInt(CellIdClass.Text) <= -1 Then
    '            sScript += "parent.parent.Comandi.location.href='../Fabbricati/Gestione Fabbricati/ComandiInserimento.aspx';"
    '            sScript += "location.href ='../Fabbricati/Gestione Fabbricati/GestFabbricato.aspx?IdVariazioneTributaria=" + CellIdVar.Text + "&Provenienza=I';"
    '        Else
    '            If CellTipoVar.Text.ToUpper = MetodiVariazioniTributi.VariazioneSuClass Then
    '                sScript += "parent.parent.Comandi.location.href='../Fabbricati/Gestione Fabbricati/Classificazioni/ComandiClassificazioni.aspx';"
    '                sScript += "location.href ='../Fabbricati/Gestione Fabbricati/Classificazioni/GestClassificazioni.aspx?IdVariazioneTributaria=" + CellIdVar.Text + "&CodUI=" + CellIdUI.Text + "&CodClas=" + CellIdClass.Text + "';"
    '            Else
    '                sScript += "parent.parent.Comandi.location.href='../Fabbricati/Gestione Fabbricati/ComandiSituazioneUI.aspx';"
    '                sScript += "location.href ='../Fabbricati/Gestione Fabbricati/GestUI.aspx?IdVariazioneTributaria=" + CellIdVar.Text + "&CodUI=" + CellIdUI.Text + "';"
    '            End If
    '        End If
    '        sScript += "</script>"
    '        sBuilder.Append(sScript)
    '        RegisterScript("loadnewpage", sBuilder.ToString())
    '    Catch Err As Exception
    '        Log.Debug("Si è verificato un errore in GestDichiarazione::GrdTessere_SelectedIndexChanged::" & Err.Message)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    'Private Sub GrdResult_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles GrdResult.SortCommand
    '    Try
    '        If Not ViewState("SortKey") Is Nothing Then
    '            If (e.SortExpression.ToString.CompareTo(ViewState("SortKey").ToString) = 0) Then
    '                Select Case (ViewState("OrderBy").ToString)
    '                    Case "ASC"
    '                        ViewState("OrderBy") = "DESC"
    '                    Case "DESC"
    '                        ViewState("OrderBy") = "ASC"
    '                End Select
    '            Else
    '                ViewState("SortKey") = e.SortExpression.ToString
    '                ViewState("OrderBy") = "ASC"
    '            End If
    '        Else
    '            ViewState("SortKey") = e.SortExpression.ToString
    '            ViewState("OrderBy") = "ASC"
    '        End If
    '        Dim TabellaSort As DataTable
    '        TabellaSort = CType(Session("oVariazioniTributiSearch"),dataview)
    '        TabellaSort.DefaultView.Sort = (ViewState("SortKey") + (" " + ViewState("OrderBy")))
    '        Session("oVariazioniTributiSearch") = TabellaSort
    '        GrdResult.start_index = Convert.ToString(GrdResult.CurrentPageIndex)
    '        GrdResult.AllowCustomPaging = False
    '        GrdResult.DataSource = TabellaSort.DefaultView
    '        GrdResult.DataBind()
    '    Catch Err As Exception
    '        Log.Debug("Si è verificato un errore in VariazioniTributiSearch::GrdResult_SortCommand::" & Err.Message)
    '        Response.Redirect("../PaginaErrore.aspx")
    '    End Try
    'End Sub
#End Region

    Private Sub RestoreSearch()
        Dim nsParam As String
        Dim newSearch As Boolean = False
        Dim SearchParams As New objVariazioniTributiSearch
        Try
            Log.Debug("VariazioniTributiSearch::RestoreSearch::prelevo request.item")
            If Not Request("NewSearch") Is Nothing Then
                nsParam = Request("NewSearch")
            End If
            Log.Debug("VariazioniTributiSearch::RestoreSearch::prelevo parametri precedenti")
            If nsParam <> "" Then
                newSearch = Boolean.Parse(nsParam)
            End If
            ' Sto tornando alla pagina da quella di visualizzazione dettaglio
            ' per cui ripopolo la ricerca
            If Not newSearch Then
                Log.Debug("VariazioniTributiSearch::RestoreSearch::ho parametri prec quindi li ricarico")
                If Not Session("oVariazioniTributiParamSearch") Is Nothing Then
                    SearchParams = CType(Session("oVariazioniTributiParamSearch"), objVariazioniTributiSearch)
                    If (Not (SearchParams) Is Nothing) Then
                        DdlTributo.SelectedValue = SearchParams.Tributo
                        TxtFoglio.Text = SearchParams.Foglio
                        TxtNumero.Text = SearchParams.Numero
                        TxtSub.Text = SearchParams.Subalterno
                        If SearchParams.Dal <> DateTime.MaxValue Then
                            TxtDal.Text = SearchParams.Dal
                        End If
                        If SearchParams.Al <> DateTime.MaxValue Then
                            TxtAl.Text = SearchParams.Al
                        End If
                        DdlOperatore.SelectedValue = SearchParams.Operatore
                        DdlCausali.SelectedValue = SearchParams.IdCausale
                        RbTrattato.Checked = SearchParams.IsTrattato

                        Session("oVariazioniTributiSearch") = MetodiVariazioniTributi.SearchVariazioniTributi(SearchParams, ConstSession.StringConnectionOPENgov)
                        DataBindGridResult()
                    End If
                End If
            Else
                Session("oVariazioniTributiParamSearch") = Nothing
                Session("oVariazioniTributiSearch") = Nothing
            End If
        Catch Err As Exception
            Log.Debug("Si è verificato un errore in VariazioniTributiSearch::RestoreSearch::" & Err.Message)
            Response.Redirect("../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub DataBindGridResult(Optional ByVal page As Integer? = 0)
        Try
            GrdResult.DataSource = CType(Session("oVariazioniTributiSearch"), DataView)
            If page.HasValue Then
                GrdResult.PageIndex = page.Value
            End If
            GrdResult.DataBind()

            If GrdResult.Rows.Count > 0 Then
                GrdResult.Visible = True
                LblResultdichiarazioni.Visible = False
                GrdResult.Columns(7).Visible = False
                GrdResult.Columns(8).Visible = False
            Else
                LblResultdichiarazioni.Visible = True
                GrdResult.Visible = False
            End If
        Catch Err As Exception
            Log.Debug("OPENgovTERRITORIO.VariazioniTributiSearch.DataBindGridResult.errore::", Err)
            Response.Redirect("../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub SetTrattato_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles SetTrattato.Click
        Dim FncVariazioniTributi As New Utility.ModificheTributarie
        Dim bRet As Boolean
        Dim dToUpdate As DateTime = DateTime.MaxValue

        Try
            If RbNonTrattato.Checked = True Then
                dToUpdate = Now
            End If
            For Each myRow As GridViewRow In GrdResult.Rows
                If CType(myRow.FindControl("ckbSelezione"), CheckBox).Checked = True Then
                    bRet = FncVariazioniTributi.SetModificheTributarie(ConstSession.StringConnectionOPENgov, Utility.ModificheTributarie.DBOperation.Update, CInt(CType(myRow.FindControl("hfId"), HiddenField).Value), "", "", -1, "", "", "", DateTime.MaxValue, "", -1, Now)
                    If bRet = False Then
                        RegisterScript("GestAlert('a', 'danger', '', '', 'Errore in aggiornamento variazione trattata!');", Me.GetType())
                        Exit For
                    End If
                End If
            Next
            If bRet = True Then
                RegisterScript("alert('Aggiornamento variazione trattata effettuato con successo!');", Me.GetType())
            End If
            RestoreSearch()
        Catch ex As Exception
            Log.Debug("Si è verificato un errore in VariazioniTributiSearch::SetTrattato_Click::" & ex.Message)
        End Try
    End Sub

    Private Sub RbTrattato_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RbTrattato.CheckedChanged
        Try
            If RbTrattato.Checked = True Then
                RbNonTrattato.Checked = False
            End If
        Catch ex As Exception
            Log.Debug("Si è verificato un errore in VariazioniTributiSearch::RbTrattato_CheckedChanged::" & ex.Message)
        End Try
    End Sub

    Private Sub RbNonTrattato_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RbNonTrattato.CheckedChanged
        Try
            If RbNonTrattato.Checked = True Then
                RbTrattato.Checked = False
            End If
        Catch ex As Exception
            Log.Debug("Si è verificato un errore in VariazioniTributiSearch::RbNonTrattato_CheckedChanged::" & ex.Message)
        End Try
    End Sub

    Protected Sub CmdSearch_Click(sender As Object, e As System.EventArgs) Handles CmdSearch.Click
        Try
            Session("oVariazioniTributiParamSearch") = Nothing
            Session("oVariazioniTributiSearch") = Nothing
            Dim SearchParams As New objVariazioniTributiSearch
            SearchParams.IdEnte = ConstSession.IdEnte
            SearchParams.Tributo = DdlTributo.SelectedValue
            SearchParams.Foglio = TxtFoglio.Text.Trim
            SearchParams.Numero = TxtNumero.Text.Trim
            SearchParams.Subalterno = TxtSub.Text.Trim
            If TxtDal.Text.Trim <> "" Then
                SearchParams.Dal = TxtDal.Text.Trim
            End If
            If TxtAl.Text.Trim <> "" Then
                SearchParams.Al = TxtAl.Text.Trim
            End If
            SearchParams.Operatore = DdlOperatore.SelectedValue
            SearchParams.IdCausale = DdlCausali.SelectedValue
            If RbTrattato.Checked = True Then
                SearchParams.IsTrattato = 1
            Else
                SearchParams.IsTrattato = 0
            End If

            Session("oVariazioniTributiSearch") = MetodiVariazioniTributi.SearchVariazioniTributi(SearchParams, ConstSession.StringConnectionOPENgov)
            DataBindGridResult()
            Session("oVariazioniTributiParamSearch") = SearchParams
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTERRITORIO.VariazioniTributiSearch.CmdSearch_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            DivAttesa.Style.Add("display", "none")
        End Try
    End Sub
    ''' <summary>
    ''' Pulsante per l'estrazione in formato XLS della ricerca effettuata
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdStampa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdStampa.Click
        Dim sNameXLS As String
        Dim DvDati As New DataView
        Dim FncStampa As New ClsStampaXLS
        Dim DtDatiStampa As New DataTable
        Dim aListColonne As ArrayList
        Dim x, nCol As Integer
        Dim aMyHeaders As String()

        Try
            nCol = 7
            Dim SearchParams As New objVariazioniTributiSearch
            SearchParams.IdEnte = ConstSession.IdEnte
            SearchParams.Tributo = DdlTributo.SelectedValue
            SearchParams.Foglio = TxtFoglio.Text.Trim
            SearchParams.Numero = TxtNumero.Text.Trim
            SearchParams.Subalterno = TxtSub.Text.Trim
            If TxtDal.Text.Trim <> "" Then
                SearchParams.Dal = TxtDal.Text.Trim
            End If
            If TxtAl.Text.Trim <> "" Then
                SearchParams.Al = TxtAl.Text.Trim
            End If
            SearchParams.Operatore = DdlOperatore.SelectedValue
            SearchParams.IdCausale = DdlCausali.SelectedValue
            If RbTrattato.Checked = True Then
                SearchParams.IsTrattato = 1
            Else
                SearchParams.IsTrattato = 0
            End If

            DtDatiStampa = FncStampa.PrintVariazioniTributi(MetodiVariazioniTributi.SearchVariazioniTributi(SearchParams, ConstSession.StringConnectionOPENgov), ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, nCol)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTERRITORIO.VariazioniTributiSearch.CmdStampa_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            DvDati.Dispose()
        End Try

        If Not DtDatiStampa Is Nothing Then
            sNameXLS = ConstSession.IdEnte & "_ELENCO_VARIAZIONI_TRIBUTI_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"

            'definisco le colonne
            aListColonne = New ArrayList
            For x = 0 To nCol
                aListColonne.Add("")
            Next
            aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

            'definisco l'insieme delle colonne da esportare
            Dim MyCol() As Integer = New Integer(nCol) {}
            For x = 0 To nCol
                MyCol(x) = x
            Next
            'esporto i dati in excel
            Dim MyStampa As New RKLib.ExportData.Export("Web")
            MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
        Else
            Dim sScript As String = "GestAlert('a', 'danger', '', '', 'Errore in estrazione!');"
            sScript += "DivAttesa.style.display='none';"
            RegisterScript(sScript, Me.GetType)
        End If
    End Sub
End Class