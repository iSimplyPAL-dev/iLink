Imports RemotingInterfaceAnater
Imports Anater.Oggetti
Imports log4net
''' <summary>
''' Pagina per la visualizzazione del risultato della ricerca vani da ANATER.
''' Contiene la griglia dalla quale è possibile aggangiare le posizioni alla dichiarazione.
''' </summary>
Partial Class ResultRicVani
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ResultRicVani))

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
        Dim oVanoAnater() As OggettoVanoAnater = Nothing
        Dim MyFunction As New generalClass.generalFunction

        Try
            If Page.IsPostBack = False Then
                'carico i dati in griglia
                If Not Session("oListVaniAnater") Is Nothing Then
                    oVanoAnater = Session("oListVaniAnater")
                    GrdVani.DataSource = oVanoAnater
                    GrdVani.DataBind()
                    LblResult.Style.Add("display", "none")
                    chkSelezionaTutti.Style.Add("display", "")
                Else
                    LblResult.Style.Add("display", "")
                    chkSelezionaTutti.Style.Add("display", "none")
                End If
                Session("oVanoAnater") = oVanoAnater
                'carico le categorie
                MyFunction.LoadComboConfig(DdlCategoria, "TBLCATEGORIE", ConstSession.IdEnte, ConstSession.StringConnection)
                '*** 20130228 - gestione categoria Ateco per TARES ***
                Dim sSQL As String = "SELECT CODICECATEGORIA+' '+DEFINIZIONE, IDCATEGORIAATECO"
                sSQL += " FROM V_CATEGORIE_ATECO"
                sSQL += " WHERE ((fk_IdTypeAteco=0) OR (fk_IdTypeAteco=" & ConstSession.IdTypeAteco & "))"
                sSQL += " AND (ENTE='" & ConstSession.IdEnte & "')"
                sSQL += " ORDER BY RIGHT(REPLICATE('0',10)+CAST(IDCATEGORIAATECO AS VARCHAR),10)"
                MyFunction.LoadComboGenerale(DDlCatTARES, sSQL, ConstSession.StringConnection, True, Costanti.TipoDefaultCmb.STRINGA)
                '*** ***
            Else
                ControllaCheckbox()
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultRicVani.Page_Load.errore: ", Err)
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
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdVani.DataSource = CType(Session("oVanoAnater"), OggettoVanoAnater())
            If page.HasValue Then
                GrdVani.PageIndex = page.Value
            End If
            GrdVani.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultRicVani.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
    Private Sub DdlCategoria_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DdlCategoria.SelectedIndexChanged
        Try
            If DdlCategoria.SelectedItem.Value <> "" Then
                Session("IdCatTARSU") = DdlCategoria.SelectedItem.Value
                Session("DescrCatTARSU") = DdlCategoria.SelectedItem.Text
            Else
                Session("IdCatTARSU") = Nothing
                Session("DescrCatTARSU") = Nothing
            End If
            '*** 20140805 - Gestione Categorie Vani ***
            If DDlCatTARES.SelectedValue <> "" Then
                Session("IdCatTARES") = DDlCatTARES.SelectedValue
                Session("DescrCatTARES") = DDlCatTARES.SelectedItem.Text
            Else
                Session("IdCatTARES") = Nothing
                Session("DescrCatTARES") = Nothing
            End If
            If IsNumeric(TxtNComponenti.Text) Then
                Session("NCPF") = CInt(TxtNComponenti.Text)
            Else
                Session("NCPF") = Nothing
            End If
            If IsNumeric(TxtNComponentiPV.Text) Then
                Session("NCPV") = CInt(TxtNComponentiPV.Text)
            Else
                Session("NCPV") = Nothing
            End If
            '*** ***
            LoadSearch()
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultRicVani.SelectedIndexChanged.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub ControllaCheckbox()
        Dim myRow As GridViewRow
        Dim oVanoAnater() As OggettoVanoAnater
        Dim x As Integer

        Try
            oVanoAnater = CType(Session("oListVaniAnater"), OggettoVanoAnater())
            For Each myRow In GrdVani.Rows
                'prendo l'ID da aggiornare
                For x = 0 To oVanoAnater.GetUpperBound(0)
                    If oVanoAnater(x).IDProgressivo = CType(CType(myRow.FindControl("hfIDProgressivo"), HiddenField).Value, Long) Then
                        oVanoAnater(x).selezionato = CType(myRow.FindControl("ChkSelezionato"), CheckBox).Checked
                    End If
                Next
            Next
            Session("oVanoAnater") = oVanoAnater
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultRicVani.ControllaCheckbox.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub chkSelezionaTutti_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSelezionaTutti.CheckedChanged
        Dim oVanoAnater() As OggettoVanoAnater
        Dim x As Integer
        Dim bValue As Boolean

        oVanoAnater = CType(Session("oListVaniAnater"), OggettoVanoAnater())
        Try
            If (chkSelezionaTutti.Checked) Then
                bValue = True
                chkSelezionaTutti.Text = "Deseleziona tutti"
            Else
                bValue = False
                chkSelezionaTutti.Text = "Seleziona tutti"
            End If

            For x = 0 To oVanoAnater.Length - 1
                oVanoAnater(x).selezionato = bValue
            Next

            Session("oVanoAnater") = oVanoAnater
            GrdVani.DataSource = oVanoAnater
            GrdVani.DataBind()
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultRicVani.chkSelezionaTutti_CheckedChanged.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub TxtDataInizio_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtDataInizio.TextChanged
        Session("TxtDataInizio") = TxtDataInizio.Text
        LoadSearch()
    End Sub

    '*** 20140805 - Gestione Categorie Vani ***
    Private Sub DDlCatTARES_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDlCatTARES.SelectedIndexChanged
        Try
            If DdlCategoria.SelectedItem.Value <> "" Then
                Session("IdCatTARSU") = DdlCategoria.SelectedItem.Value
                Session("DescrCatTARSU") = DdlCategoria.SelectedItem.Text
            Else
                Session("IdCatTARSU") = Nothing
                Session("DescrCatTARSU") = Nothing
            End If
            If DDlCatTARES.SelectedValue <> "" Then
                Session("IdCatTARES") = DDlCatTARES.SelectedValue
                Session("DescrCatTARES") = DDlCatTARES.SelectedItem.Text
            Else
                Session("IdCatTARES") = Nothing
                Session("DescrCatTARES") = Nothing
            End If
            If IsNumeric(TxtNComponenti.Text) Then
                Session("NCPF") = CInt(TxtNComponenti.Text)
            Else
                Session("NCPF") = Nothing
            End If
            If IsNumeric(TxtNComponentiPV.Text) Then
                Session("NCPV") = CInt(TxtNComponentiPV.Text)
            Else
                Session("NCPV") = Nothing
            End If
            LoadSearch()
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultRicVani.DDICatTARES_SelectedIndexChanged.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub TxtNComponenti_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtNComponenti.TextChanged
        Try
            If DdlCategoria.SelectedItem.Value <> "" Then
                Session("IdCatTARSU") = DdlCategoria.SelectedItem.Value
                Session("DescrCatTARSU") = DdlCategoria.SelectedItem.Text
            Else
                Session("IdCatTARSU") = Nothing
                Session("DescrCatTARSU") = Nothing
            End If
            If DDlCatTARES.SelectedValue <> "" Then
                Session("IdCatTARES") = DDlCatTARES.SelectedValue
                Session("DescrCatTARES") = DDlCatTARES.SelectedItem.Text
            Else
                Session("IdCatTARES") = Nothing
                Session("DescrCatTARES") = Nothing
            End If
            If IsNumeric(TxtNComponenti.Text) Then
                Session("NCPF") = CInt(TxtNComponenti.Text)
            Else
                Session("NCPF") = Nothing
            End If
            If IsNumeric(TxtNComponentiPV.Text) Then
                Session("NCPV") = CInt(TxtNComponentiPV.Text)
            Else
                Session("NCPV") = Nothing
            End If
            LoadSearch()
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultRicVani.TxtNComponenti_TextChanged.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub TxtNComponentiPV_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtNComponentiPV.TextChanged
        Try
            If DdlCategoria.SelectedItem.Value <> "" Then
                Session("IdCatTARSU") = DdlCategoria.SelectedItem.Value
                Session("DescrCatTARSU") = DdlCategoria.SelectedItem.Text
            Else
                Session("IdCatTARSU") = Nothing
                Session("DescrCatTARSU") = Nothing
            End If
            If DDlCatTARES.SelectedValue <> "" Then
                Session("IdCatTARES") = DDlCatTARES.SelectedValue
                Session("DescrCatTARES") = DDlCatTARES.SelectedItem.Text
            Else
                Session("IdCatTARES") = Nothing
                Session("DescrCatTARES") = Nothing
            End If
            If IsNumeric(TxtNComponenti.Text) Then
                Session("NCPF") = CInt(TxtNComponenti.Text)
            Else
                Session("NCPF") = Nothing
            End If
            If IsNumeric(TxtNComponentiPV.Text) Then
                Session("NCPV") = CInt(TxtNComponentiPV.Text)
            Else
                Session("NCPV") = Nothing
            End If
            LoadSearch()
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultRicVani.TxtNComponentiPV_TextChanged.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** ***
End Class
