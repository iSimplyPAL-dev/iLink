Imports DichiarazioniICI.Database
Imports Business
Imports log4net
''' <summary>
''' Pagina dei comandi per la gestione delle tariffe estimo.
''' Contiene i parametri di ricerca ed inserimento, le funzioni della comandiera e la griglia per la visualizzazione del risultato.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class ConfigTariffe
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ConfigTariffe))
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblNote As System.Web.UI.WebControls.Label

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
        Try
            If Not Page.IsPostBack Then
                'richiamo la pagina dei comandi
                Dim strscript As String
                strscript = "parent.Comandi.location.href='./ComandiConfigTariffe.aspx';"
                RegisterScript(strscript, Me.GetType())

                btnElimina.Attributes.Add("onclick", "return ConfermaElimina();")
                Dim fncActionEvent As New Utility.DBUtility(COSTANTValue.ConstSession.DBType, COSTANTValue.ConstSession.StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, COSTANTValue.ConstSession.UserName, "TariffeEstimo", "ConfigurazioneTariffeEstimo", Utility.Costanti.AZIONE_LETTURA.ToString, "", COSTANTValue.ConstSession.IdEnte, -1)
            Else
                'If radioTabella.SelectedValue = "TabEstimoFab" Then
                '    If Not Session("TabRicerca") Is Nothing Then
                '        GrdTariffeFab.DataSource = Session("TabRicerca")
                '    End If
                'Else
                '    If Not Session("TabRicerca") Is Nothing Then
                '        GrdTariffeUrbane.DataSource = CType(Session("TabRicerca"), DataTable)
                '    End If
                'End If
                'DataTable TabSource = (DataTable)Session["TABELLA_RICERCA"];
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.ConfigTariffe.Page_Load.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub CaricaComboCategorie()
        Dim DtCategorie As New DataTable
        DtCategorie = New CategoriaCatastaleTable(ConstWrapper.sUsername).List()

        ddlCategoria.Items.Clear()
        Try
            If DtCategorie.Rows.Count > 0 Then
                Dim li As New ListItem("...", "...")
                ddlCategoria.Items.Add(li)

                For Each dr As DataRow In DtCategorie.Rows
                    Dim li1 As New ListItem(dr("CategoriaCatastale").ToString, dr("CategoriaCatastale").ToString)
                    ddlCategoria.Items.Add(li1)
                Next
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.ConfigTariffe.CaricaComboCategorie.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub CaricaComboClassi()
        Dim DtClasse As New DataTable
        DtClasse = New ClasseTable(ConstWrapper.sUsername).List()

        ddlClasse.Items.Clear()
        Try
            If DtClasse.Rows.Count > 0 Then
                Dim li As New ListItem("...", "...")
                ddlClasse.Items.Add(li)

                For Each dr As DataRow In DtClasse.Rows
                    Dim li1 As New ListItem(dr("Classe").ToString, dr("Classe").ToString)
                    ddlClasse.Items.Add(li1)
                Next
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.ConfigTariffe.CaricaComboClassi.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub radioTabella_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioTabella.SelectedIndexChanged
        Select Case radioTabella.SelectedItem.Value
            Case "TabEstimoFab"
                ' blocco le combo per la selezione della categoria e della classe
                ddlCategoria.Enabled = False
                ddlClasse.Enabled = False
                lblClasse.Enabled = False
                lblCategoria.Enabled = False
                GrdTariffeUrbane.Visible = False
                txtDataFine.Text = ""
                txtDataInizio.Text = ""
                txtNote.Text = ""
                txtTariffa.Text = ""
                txtZona.Text = ""
                txtID.Text = ""
            Case "TabEstimo"
                CaricaComboCategorie()
                CaricaComboClassi()
                ddlCategoria.Enabled = True
                ddlClasse.Enabled = True
                lblClasse.Enabled = True
                lblCategoria.Enabled = True
                GrdTariffeFab.Visible = False
                txtDataFine.Text = ""
                txtDataInizio.Text = ""
                txtNote.Text = ""
                txtTariffa.Text = ""
                txtZona.Text = ""
                txtID.Text = ""
        End Select
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnRicerca_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRicerca.Click
        ' a seconda della tabella selezionata eseguo una ricerca diversa
        Try
            Select Case radioTabella.SelectedItem.Value
                Case "TabEstimoFab"
                    ' recupero i valori dalle caselle di ricerca
                    Dim DataInizioValidita As DateTime = DateTime.MinValue
                    Dim DataFineValidita As DateTime = DateTime.MinValue
                    Dim Tariffa As Decimal = 0
                    Dim Zona As String = ""
                    If txtDataInizio.Text <> "" Then
                        DataInizioValidita = DateTime.Parse(txtDataInizio.Text)
                    End If
                    If txtDataFine.Text <> "" Then
                        DataFineValidita = DateTime.Parse(txtDataFine.Text)
                    End If

                    If txtTariffa.Text <> "" Then
                        Tariffa = Decimal.Parse(txtTariffa.Text)
                    End If

                    If txtZona.Text <> "" Then
                        Zona = txtZona.Text
                    End If

                    Dim objRicerca As New TariffeEstimoFabRow
                    objRicerca.DataFineValidita = DataFineValidita
                    objRicerca.DataInizioValidita = DataInizioValidita
                    objRicerca.TariffaEuro = Tariffa
                    objRicerca.Ente = ConstWrapper.CodiceEnte
                    objRicerca.Zona = Zona

                    Dim TabRicerca As DataTable
                    TabRicerca = New TariffeEstimoAFTable(ConstWrapper.sUsername).Ricerca(objRicerca)
                    If TabRicerca.Rows.Count > 0 Then
                        Session("TabRicerca") = TabRicerca
                        GrdTariffeFab.DataSource = TabRicerca
                        GrdTariffeFab.DataBind()
                        lblRisultati.Visible = False
                        GrdTariffeFab.Visible = True
                    Else
                        lblRisultati.Visible = True
                        GrdTariffeFab.Visible = False
                    End If

                Case "TabEstimo"
                    ' recupero i valori dalle caselle di ricerca
                    Dim DataInizioValidita As DateTime = DateTime.MinValue
                    Dim DataFineValidita As DateTime = DateTime.MinValue
                    Dim Tariffa As Decimal = 0

                    Dim Zona As String = ""
                    Dim Categoria As String = ""
                    Dim Classe As String = ""

                    If txtDataInizio.Text <> "" Then
                        DataInizioValidita = DateTime.Parse(txtDataInizio.Text)
                    End If
                    If txtDataFine.Text <> "" Then
                        DataFineValidita = DateTime.Parse(txtDataFine.Text)
                    End If
                    If txtTariffa.Text <> "" Then
                        Tariffa = Decimal.Parse(txtTariffa.Text.ToString())
                    End If
                    If txtZona.Text <> "" Then
                        Zona = txtZona.Text
                    End If
                    If ddlCategoria.SelectedItem.Text <> "..." Then
                        Categoria = ddlCategoria.SelectedItem.Text
                    End If
                    If ddlClasse.SelectedItem.Text <> "..." Then
                        Classe = ddlClasse.SelectedItem.Text
                    End If

                    Dim objRicerca As New TariffeEstimoRow
                    objRicerca.Ente = ConstWrapper.CodiceEnte
                    objRicerca.Categoria = Categoria
                    objRicerca.Classe = Classe
                    objRicerca.Zona = Zona
                    objRicerca.DataFineValidita = DataFineValidita
                    objRicerca.DataInizioValidita = DataInizioValidita
                    objRicerca.TariffaEuro = Tariffa

                    Dim TabRicerca As DataTable
                    TabRicerca = New TariffeEstimoTable(ConstWrapper.sUsername).Ricerca(objRicerca)

                    If TabRicerca.Rows.Count > 0 Then
                        Session("TabRicerca") = TabRicerca
                        GrdTariffeUrbane.DataSource = Session("TabRicerca")
                        GrdTariffeUrbane.DataSource = TabRicerca
                        GrdTariffeUrbane.DataBind()
                        lblRisultati.Visible = False
                        GrdTariffeUrbane.Visible = True
                    Else
                        lblRisultati.Visible = True
                        GrdTariffeUrbane.Visible = False
                    End If
            End Select
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.ConfigTariffe.btnRicerca_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub

#Region "Griglie"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim IDRow As String = e.CommandArgument.ToString()
            If e.CommandName = "RowOpen" Then
                If CType(sender, Ribes.OPENgov.WebControls.RibesGridView).UniqueID = "GrdTariffeFab" Then
                    For Each myRow As GridViewRow In GrdTariffeFab.Rows

                        'If IDRow = myRow.Cells(1).Text Then
                        If IDRow = CType(myRow.FindControl("hfID"), HiddenField).Value Then
                            'txtID.Text = myRow.RowIndex
                            txtID.Text = CType(myRow.FindControl("hfID"), HiddenField).Value

                            txtTariffa.Text = CType(myRow.FindControl("lblTariffaGrd"), Label).Text
                            txtDataInizio.Text = CType(myRow.FindControl("lblDataInizioGrid"), Label).Text
                            txtDataFine.Text = CType(myRow.FindControl("lblDataFineGrid"), Label).Text
                            txtZona.Text = myRow.Cells(1).Text
                            If CType(myRow.FindControl("hfNOTEfab"), HiddenField).Value.CompareTo("&nbsp;") <> 0 Then
                                txtNote.Text = CType(myRow.FindControl("hfNOTEfab"), HiddenField).Value
                            Else
                                txtNote.Text = ""
                            End If
                        End If
                    Next
                Else
                    For Each myRow As GridViewRow In GrdTariffeUrbane.Rows
                        If IDRow = myRow.Cells(1).Text Then
                            txtID.Text = myRow.RowIndex.ToString

                            txtTariffa.Text = CType(myRow.FindControl("lblTariffaGrdUrb"), Label).Text
                            txtDataInizio.Text = CType(myRow.FindControl("lblDataInizioGridUrb"), Label).Text
                            txtDataFine.Text = CType(myRow.FindControl("lblDataFineGridUrb"), Label).Text
                            txtZona.Text = myRow.Cells(1).Text
                            If CType(myRow.FindControl("hfNOTEUrb"), HiddenField).Value.CompareTo("&nbsp;") <> 0 Then
                                txtNote.Text = CType(myRow.FindControl("hfNOTEUrb"), HiddenField).Value
                            Else
                                txtNote.Text = ""
                            End If
                            Dim Categoria As String = myRow.Cells(5).Text
                            Dim Classe As String = myRow.Cells(6).Text

                            ddlCategoria.SelectedIndex = ddlCategoria.Items.IndexOf(ddlCategoria.Items.FindByText(Categoria))
                            ddlClasse.SelectedIndex = ddlClasse.Items.IndexOf(ddlClasse.Items.FindByText(Classe))
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.ConfigTariffe.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Gestione del cambio pagina della griglia.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdPageIndexChanging(ByVal sender As Object, e As GridViewPageEventArgs)
        LoadSearch(CType(sender, Ribes.OPENgov.WebControls.RibesGridView), e.NewPageIndex)
    End Sub
    'Private Sub GrdTariffeFab_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GrdTariffeFab.SelectedIndexChanged
    '    ' prendo i valori che leggo dalla tabella e li assegno ai controlli
    'Try
    '    txtID.Text = GrdTariffeFab.DataKeys(GrdTariffeFab.SelectedIndex)

    '    txtTariffa.Text = CType(GrdTariffeFab.SelectedItem.Cells(0).FindControl("lblTariffaGrd"), Label).Text
    '    'txtTariffa.Text = GrdTariffeFab.SelectedItem.Cells(0).Text
    '    txtDataInizio.Text = CType(GrdTariffeFab.SelectedItem.Cells(2).FindControl("lblDataInizioGrid"), Label).Text
    '    'txtDataInizio.Text = DateTime.Parse(GrdTariffeFab.SelectedItem.Cells(2).Text).ToShortDateString
    '    txtDataFine.Text = CType(GrdTariffeFab.SelectedItem.Cells(3).FindControl("lblDataFineGrid"), Label).Text
    '    'txtDataFine.Text = DateTime.Parse(GrdTariffeFab.SelectedItem.Cells(3).Text).ToShortDateString        
    '    txtZona.Text = GrdTariffeFab.SelectedItem.Cells(1).Text
    '    If GrdTariffeFab.SelectedItem.Cells(4).Text.CompareTo("&nbsp;") <> 0 Then
    '        txtNote.Text = GrdTariffeFab.SelectedItem.Cells(4).Text
    '    Else
    '        txtNote.Text = ""
    '    End If
    ' Catch ex As Exception
    '      Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.ConfigTariffe.LoadSearch.errore: ", ex)
    '      Response.Redirect("../../../PaginaErrore.aspx")
    'End Try
    'End Sub

    'Private Sub GrdTariffeUrbane_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GrdTariffeUrbane.SelectedIndexChanged
    '    ' prendo i valori che leggo dalla tabella e li assegno ai controlli
    'Try
    '    txtID.Text = GrdTariffeUrbane.DataKeys(GrdTariffeUrbane.SelectedIndex)

    '    txtTariffa.Text = CType(GrdTariffeUrbane.SelectedItem.Cells(0).FindControl("lblTariffaGrdUrb"), Label).Text
    '    'txtTariffa.Text = GrdTariffeFab.SelectedItem.Cells(0).Text
    '    txtDataInizio.Text = CType(GrdTariffeUrbane.SelectedItem.Cells(2).FindControl("lblDataInizioGridUrb"), Label).Text
    '    'txtDataInizio.Text = DateTime.Parse(GrdTariffeFab.SelectedItem.Cells(2).Text).ToShortDateString
    '    txtDataFine.Text = CType(GrdTariffeUrbane.SelectedItem.Cells(3).FindControl("lblDataFineGridUrb"), Label).Text
    '    'txtDataFine.Text = DateTime.Parse(GrdTariffeFab.SelectedItem.Cells(3).Text).ToShortDateString        
    '    txtZona.Text = GrdTariffeUrbane.SelectedItem.Cells(1).Text
    '    If GrdTariffeUrbane.SelectedItem.Cells(7).Text.CompareTo("&nbsp;") <> 0 Then
    '        txtNote.Text = GrdTariffeUrbane.SelectedItem.Cells(7).Text
    '    Else
    '        txtNote.Text = ""
    '    End If

    '    Dim Categoria As String = GrdTariffeUrbane.SelectedItem.Cells(5).Text
    '    Dim Classe As String = GrdTariffeUrbane.SelectedItem.Cells(6).Text

    '    ddlCategoria.SelectedIndex = ddlCategoria.Items.IndexOf(ddlCategoria.Items.FindByText(Categoria))
    '    ddlClasse.SelectedIndex = ddlClasse.Items.IndexOf(ddlClasse.Items.FindByText(Classe))
    ' Catch ex As Exception
    '   Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.ConfigTariffe.LoadSearch.errore: ", ex)
    '   Response.Redirect("../../../PaginaErrore.aspx")
    'End Try
    'End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(myGrd As Ribes.OPENgov.WebControls.RibesGridView, Optional ByVal page As Integer? = 0)
        Try
            myGrd.DataSource = CType(Session("TabRicerca"), DataTable)
            If page.HasValue Then
                myGrd.PageIndex = page.Value
            End If
            myGrd.DataBind()
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.ConfigTariffe.LoadSearch.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sData"></param>
    ''' <returns></returns>
    Public Function FormattaData(ByVal sData As DateTime) As String
        Return sData.ToShortDateString
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sDec"></param>
    ''' <returns></returns>
    Public Function FormattaDecimale(ByVal sDec As Decimal) As String
        Return sDec.ToString("N")
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnNuovo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNuovo.Click
        Select Case radioTabella.SelectedValue
            Case "TabEstimoFab"
                txtID.Text = ""
                txtDataFine.Text = ""
                txtDataInizio.Text = ""
                txtNote.Text = ""
                txtTariffa.Text = ""
                txtZona.Text = ""
                ddlCategoria.Enabled = False
                ddlClasse.Enabled = False
            Case "TabEstimo"
                txtID.Text = ""
                txtDataFine.Text = ""
                txtDataInizio.Text = ""
                txtNote.Text = ""
                txtTariffa.Text = ""
                txtZona.Text = ""
        End Select
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnSalva_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalva.Click
        ' se sono in presenza di txtID valorizzato vuol dire che sto modificando altrimenti sono in presenza di nuovo inserimento
        Try
            If txtID.Text <> String.Empty Then ' sto modificando
                Select Case radioTabella.SelectedValue
                    Case "TabEstimoFab"
                        Dim objModifica As New TariffeEstimoFabRow
                        objModifica.ID = Integer.Parse(txtID.Text)
                        objModifica.Zona = txtZona.Text
                        objModifica.Ente = ConstWrapper.CodiceEnte
                        If txtDataFine.Text <> "" Then
                            objModifica.DataFineValidita = DateTime.Parse(txtDataFine.Text)
                        End If
                        objModifica.DataInizioValidita = DateTime.Parse(txtDataInizio.Text)
                        If txtTariffa.Text <> "" Then
                            objModifica.TariffaEuro = Decimal.Parse(txtTariffa.Text)
                        Else
                            objModifica.TariffaEuro = 0
                        End If
                        objModifica.Note = txtNote.Text

                        Dim tModifica As New TariffeEstimoAFTable(ConstWrapper.sUsername)
                        If tModifica.Modify(objModifica) Then
                            ' modifica andata a buon fine
                            Dim strscript As String = "GestAlert('a', 'success', '', '', 'Modifica della Tariffa effettuata con successo!');"
                            RegisterScript(strscript, Me.GetType())

                            AggiornaGrigliaTariffeFab()
                        Else
                            ' modifica fallita
                            Dim strscript As String = "GestAlert('a', 'danger', '', '', 'Modifica della Tariffa non effettuata.');"
                            RegisterScript(strscript, Me.GetType())
                        End If

                    Case "TabEstimo"
                        Dim objModifica As New TariffeEstimoRow

                        objModifica.Categoria = ddlCategoria.SelectedValue
                        objModifica.Classe = ddlClasse.SelectedValue
                        If txtDataFine.Text <> "" Then
                            objModifica.DataFineValidita = DateTime.Parse(txtDataFine.Text)
                        Else
                            objModifica.DataFineValidita = DateTime.MaxValue
                        End If
                        objModifica.DataInizioValidita = DateTime.Parse(txtDataInizio.Text)
                        objModifica.Ente = ConstWrapper.CodiceEnte
                        objModifica.ID = txtID.Text
                        objModifica.Zona = txtZona.Text
                        objModifica.Note = txtNote.Text
                        objModifica.TariffaEuro = Decimal.Parse(txtTariffa.Text)

                        Dim tModifica As New TariffeEstimoTable(ConstWrapper.sUsername)

                        If tModifica.Modify(objModifica) Then
                            Dim strscript As String = "GestAlert('a', 'success', '', '', 'Modifica della Tariffa effettuata con successo!');"
                            RegisterScript(strscript, Me.GetType())
                            ' aggiorno la griglia con le tariffe
                            AggiornaGrigliaTariffeUrbane()
                        Else
                            Dim strscript As String = "GestAlert('a', 'danger', '', '', 'Modifica della Tariffa non effettuata !');"
                            RegisterScript(strscript, Me.GetType())
                        End If
                End Select
            Else 'sto inserendo nuova tariffa
                Select Case radioTabella.SelectedValue
                    Case "TabEstimoFab"
                        Dim objInsert As New TariffeEstimoFabRow
                        objInsert.Ente = ConstWrapper.CodiceEnte
                        objInsert.Note = txtNote.Text
                        objInsert.DataInizioValidita = DateTime.Parse(txtDataInizio.Text)
                        If txtDataFine.Text <> "" Then
                            objInsert.DataFineValidita = DateTime.Parse(txtDataFine.Text)
                        Else
                            objInsert.DataFineValidita = DateTime.MaxValue
                        End If
                        objInsert.Zona = txtZona.Text
                        objInsert.TariffaEuro = Decimal.Parse(txtTariffa.Text)
                        objInsert.Note = txtNote.Text

                        Dim tInsert As New TariffeEstimoAFTable(ConstWrapper.sUsername)

                        If tInsert.Insert(objInsert) Then ' operazione di inserimento effettuata con successo
                            ' modifica andata a buon fine
                            Dim strscript As String = "GestAlert('a', 'success', '', '', 'Inserimento della Tariffa effettuata con successo!');"
                            RegisterScript(strscript, Me.GetType())

                            AggiornaGrigliaTariffeFab()
                        Else
                            ' modifica andata a buon fine
                            Dim strscript As String = "GestAlert('a', 'danger', '', '', 'Inserimento della Tariffa non effettuata!');"
                            RegisterScript(strscript, Me.GetType())
                        End If
                    Case "TabEstimo"
                        Dim objInsert As New TariffeEstimoRow

                        objInsert.Categoria = ddlCategoria.SelectedValue
                        objInsert.Classe = ddlClasse.SelectedValue
                        If txtDataFine.Text <> "" Then
                            objInsert.DataFineValidita = DateTime.Parse(txtDataFine.Text)
                        Else
                            objInsert.DataFineValidita = DateTime.MaxValue
                        End If
                        objInsert.DataInizioValidita = DateTime.Parse(txtDataInizio.Text)
                        objInsert.Ente = ConstWrapper.CodiceEnte
                        ' objInsert.ID = txtID.Text
                        objInsert.Zona = txtZona.Text
                        objInsert.Note = txtNote.Text
                        objInsert.TariffaEuro = Decimal.Parse(txtTariffa.Text)

                        Dim tInsert As New TariffeEstimoTable(ConstWrapper.sUsername)

                        If tInsert.Insert(objInsert) Then
                            Dim strscript As String = "GestAlert('a', 'success', '', '', 'Inserimento della Tariffa effettuato con successo!');"
                            RegisterScript(strscript, Me.GetType())

                            AggiornaGrigliaTariffeUrbane()
                        Else
                            Dim strscript As String = "GestAlert('a', 'danger', '', '', 'Inserimento della Tariffa non effettuato !');"
                            RegisterScript(strscript, Me.GetType())
                        End If
                End Select
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.ConfigTariffe.btnSalva_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnElimina_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnElimina.Click
        Dim strscriptOK As String = ""
        strscriptOK = "GestAlert('a', 'success', '', '', 'Eliminazione Tariffa effettuata con successo')"

        Dim strscriptKO As String = ""
        strscriptKO = "GestAlert('a', 'danger', '', '', 'Eliminazione Tariffa non effettuata')"
        Try
            Select Case radioTabella.SelectedValue
                Case "TabEstimoFab"
                    Dim ID As Integer
                    ID = Integer.Parse(txtID.Text)
                    Dim objElimina As New TariffeEstimoAFTable(ConstWrapper.sUsername)
                    If objElimina.Delete(ID) Then
                        RegisterScript(strscriptOK, Me.GetType())
                    Else
                        RegisterScript(strscriptKO, Me.GetType())
                    End If

                    AggiornaGrigliaTariffeFab()

                    PulisciControlli()
                Case "TabEstimo"
                    Dim ID As Integer

                    ID = Integer.Parse(txtID.Text)
                    Dim objElimina As New TariffeEstimoTable(ConstWrapper.sUsername)
                    If objElimina.Delete(ID) Then
                        RegisterScript(strscriptOK, Me.GetType())
                    Else
                        RegisterScript(strscriptKO, Me.GetType())
                    End If

                    AggiornaGrigliaTariffeUrbane()

                    PulisciControlli()

            End Select
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.ConfigTariffe.btnElimina_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub AggiornaGrigliaTariffeUrbane()
        ' aggiorno la griglia con le tariffe
        Dim TabRicerca As DataTable = New TariffeEstimoTable(ConstWrapper.sUsername).Ricerca(ConstWrapper.CodiceEnte)
        Try
            If TabRicerca.Rows.Count > 0 Then
                GrdTariffeUrbane.DataSource = TabRicerca
                Session("TabRicerca") = TabRicerca
                GrdTariffeUrbane.DataBind()
                lblRisultati.Visible = False
            Else
                lblRisultati.Visible = True
                GrdTariffeUrbane.Visible = False
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.ConfigTariffe.AggiornaGrigliaTariffeUrbane.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub AggiornaGrigliaTariffeFab()
        ' aggiorno la griglia con le tariffe
        Dim TabRicerca As DataTable = New TariffeEstimoAFTable(ConstWrapper.sUsername).Ricerca(ConstWrapper.CodiceEnte)
        Try
            If TabRicerca.Rows.Count > 0 Then
                GrdTariffeFab.DataSource = TabRicerca
                Session("TabRicerca") = TabRicerca
                GrdTariffeFab.DataBind()
                lblRisultati.Visible = False
            Else
                lblRisultati.Visible = True
                GrdTariffeFab.Visible = False
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.ConfigTariffe.AggiornaGrigliaTariffeFab.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub PulisciControlli()
        txtDataFine.Text = ""
        txtDataInizio.Text = ""
        txtID.Text = ""
        txtNote.Text = ""
        txtTariffa.Text = ""
        txtZona.Text = ""
    End Sub
End Class
