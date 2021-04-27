Imports Anagrafica
Imports System.Data
Imports System.Data.SqlClient
Imports log4net
''' <summary>
''' Pagina per la visualizzazione del risultato della ricerca anagrafica.
''' Contiene la griglia dalla quale è possibile accedere al dettaglio.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
''' <history>
''' 	[antonello]	25/01/2005	Created
''' </history>
Partial Class SearchResultsAnagrafica
    Inherits ANAGRAFICAWEB.BasePage
    Private Log As ILog = LogManager.GetLogger(GetType(SearchResultsAnagrafica))
    'Private GestErrore As New ANAGRAFICAWEB.VisualizzaErrore
    Private GestError As New DLL.GestError
    Private sIdEnte, strCognome, strNome, strCodiceFiscale, strPartitaIva, strCODContribuente, strVia, strCodVia, strComuneResidenza, strProvinciaResidenza, strDataNascita, strDataMorte, strWFErrore As String
    Private Const SEARCH_PARAMETRES As String = "SEARCHPARAMETRES"
    Private Const FIRST_TIME As String = "1"
    Private blnDaRicontrollare As Boolean
    Private bNoStradario As Boolean
    Private nSearchContatti As Integer
    Private nSearchInvio As Integer
    Private nSearchModulo As Integer
    Private sScript As String = ""

    'Private bHasH2O As Boolean = False
    'Private bHasOSAP As Boolean = False

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

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
    ''' <history>
    ''' 	[antonello]	24/01/2005	Created
    ''' </history>
    ''' <revisionHistory>
    ''' <revision date="30/08/2007">
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="05/2014">
    ''' NO RIBESFRAMEWORK
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="23/07/2014">
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="11/2015">
    ''' Funzioni Sovracomunali
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Esportazione completa dati</em>
    ''' </revision>
    ''' </revisionHistory>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim objAnagrafica As ANAGRAFICAWEB.AnagraficaDB
            Dim ListAnagrafica As ANAGRAFICAWEB.ListAnagrafica

            nSearchContatti = -1
            nSearchInvio = -1
            nSearchModulo = -1
            If Request.QueryString.Count > 0 Then
                Log.Debug("ho parametri ricerca")
                sIdEnte = Utility.StringOperation.FormatString(Request("IdEnte"))
                strCognome = Utility.StringOperation.FormatString(Request("cognome"))
                strNome = Utility.StringOperation.FormatString(Request("nome"))
                strCodiceFiscale = Utility.StringOperation.FormatString(Request("codicefiscale"))
                strPartitaIva = Utility.StringOperation.FormatString(Request("partitaiva"))
                strVia = Utility.StringOperation.FormatString(Request.Item("Via"))
                strCodVia = Utility.StringOperation.FormatString(Request.Item("CodVia"))
                strComuneResidenza = Utility.StringOperation.FormatString(Request.Item("comuneresidenza"))
                strProvinciaResidenza = Utility.StringOperation.FormatString(Request.Item("provinciaresidenza"))
                strDataNascita = Utility.StringOperation.FormatString(Request.Item("datanascita"))
                strDataMorte = Utility.StringOperation.FormatString(Request.Item("datamorte"))
                strCODContribuente = Utility.StringOperation.FormatString(Request("codcontribuente"))
                blnDaRicontrollare = Utility.StringOperation.FormatString(Request("DARICONTROLLARE"))
                bNoStradario = Utility.StringOperation.FormatString(Request.Item("NONAGGANCIATE"))
                If Not Request.Item("ddlContatti") Is Nothing Then
                    nSearchContatti = Utility.StringOperation.FormatInt(Request.Item("ddlContatti"))
                End If
                If Not Request.Item("ddlTributoInvio") Is Nothing Then
                    nSearchInvio = Utility.StringOperation.FormatInt(Request.Item("ddlTributoInvio"))
                End If
                If Not Request.Item("ddlTributoPresente") Is Nothing Then
                    nSearchModulo = Utility.StringOperation.FormatInt(Request.Item("ddlTributoPresente"))
                End If
                If Page.IsPostBack = False Then
                    ViewState("SortKey") = "NOMINATIVO"
                    ViewState("OrderBy") = "ASC"

                    Session.Add(SEARCH_PARAMETRES, GetSearchParametres)
                    Session("PASSSEARCHANAGRAFICA") = FIRST_TIME
                    objAnagrafica = New ANAGRAFICAWEB.AnagraficaDB(ViewState("SortKey") & " " & ViewState("OrderBy"))
                    ListAnagrafica = objAnagrafica.GetListAnagragrafica(ANAGRAFICAWEB.ConstSession.StringConnectionAnagrafica, False, strCognome, strNome, strCodiceFiscale, strPartitaIva, ANAGRAFICAWEB.ConstSession.Ambiente, sIdEnte, strCODContribuente, blnDaRicontrollare, strComuneResidenza, strProvinciaResidenza, strDataNascita, strDataMorte, strVia, strCodVia, bNoStradario, nSearchContatti, nSearchInvio, nSearchModulo)
                    Session("ListAnagrafica") = ListAnagrafica
                    If Not (Session("ListAnagrafica")) Is Nothing Then
                        LoadSearch()
                    Else
                        Log.Debug("SearchResultsAnagrafica::errore!!!!!!!!!!!!!!")
                    End If
                End If
                Log.Debug("SearchResultsAnagrafica::GrdResult.Rows.Count:: " & GrdAnagrafiche.Rows.Count.ToString)
                Select Case CInt(GrdAnagrafiche.Rows.Count)
                    Case 0
                        GrdAnagrafiche.Visible = False
                        lblMessage.Text = "Non sono state trovate anagrafiche"
                    Case Is > 0
                        If ANAGRAFICAWEB.ConstSession.IdEnte = "" Then
                            GrdAnagrafiche.Columns(0).Visible = True
                        Else
                            GrdAnagrafiche.Columns(0).Visible = False
                        End If
                        If ANAGRAFICAWEB.ConstSession.ApplicationsEnabled.IndexOf(Utility.Costanti.TRIBUTO_ICI) > 0 Then
                            GrdAnagrafiche.Columns(6).Visible = True
                        Else
                            GrdAnagrafiche.Columns(6).Visible = False
                        End If
                        If ANAGRAFICAWEB.ConstSession.ApplicationsEnabled.IndexOf(Utility.Costanti.TRIBUTO_TARSU) > 0 Then
                            GrdAnagrafiche.Columns(7).Visible = True
                        Else
                            GrdAnagrafiche.Columns(7).Visible = False
                        End If
                        If ANAGRAFICAWEB.ConstSession.ApplicationsEnabled.IndexOf(Utility.Costanti.TRIBUTO_OSAP) > 0 Then
                            GrdAnagrafiche.Columns(8).Visible = True
                        Else
                            GrdAnagrafiche.Columns(8).Visible = False
                        End If
                        If ANAGRAFICAWEB.ConstSession.ApplicationsEnabled.IndexOf(Utility.Costanti.TRIBUTO_SCUOLE) > 0 Then
                            GrdAnagrafiche.Columns(9).Visible = True
                        Else
                            GrdAnagrafiche.Columns(9).Visible = False
                        End If
                        If ANAGRAFICAWEB.ConstSession.ApplicationsEnabled.IndexOf(Utility.Costanti.TRIBUTO_H2O) > 0 Then
                            GrdAnagrafiche.Columns(10).Visible = True
                        Else
                            GrdAnagrafiche.Columns(10).Visible = False
                        End If
                        If ANAGRAFICAWEB.ConstSession.ApplicationsEnabled.IndexOf(Utility.Costanti.TRIBUTO_Accertamento) > 0 Then
                            GrdAnagrafiche.Columns(11).Visible = True
                        Else
                            GrdAnagrafiche.Columns(11).Visible = False
                        End If
                        GrdAnagrafiche.Visible = True
                End Select
            Else
                Log.Debug("no ho parametri ricerca")
                GrdAnagrafiche.Visible = False
            End If
        Catch err As Exception
            Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.SearchResultsAnagrafica.Page_Load.errore: ", err)
            Response.Redirect("../PaginaErrore.aspx")
        Finally
            RegisterScript("parent.parent.Visualizza.DivAttesa.style.display='none';", Me.GetType)
        End Try
    End Sub
    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    'Dim strTRIBUTI As String = ""

    '    Try
    '        'Log.Debug("SearchResultsAnagrafica::entro")
    '        Dim objAnagrafica As ANAGRAFICAWEB.AnagraficaDB
    '        Dim ListAnagrafica As ANAGRAFICAWEB.ListAnagrafica

    '        'If Not IsNothing(ConfigurationManager.AppSettings("APPLICATIONS_ENABLED")) Then
    '        '    strTRIBUTI = ConfigurationManager.AppSettings("APPLICATIONS_ENABLED").ToString()
    '        'End If
    '        'If InStr(strTRIBUTI, "OPENGOVU") Then
    '        '    bHasH2O = True
    '        'Else
    '        '    bHasH2O = False
    '        'End If
    '        'If InStr(strTRIBUTI, "OPENGOVTOCO") Then
    '        '    bHasOSAP = True
    '        'Else
    '        '    bHasOSAP = False
    '        'End If
    '        '*** 201511 - Funzioni Sovracomunali ***
    '        If Not Request.Item("IdEnte") Is Nothing Then
    '            sIdEnte = Trim(Request("IdEnte"))
    '        Else
    '            sIdEnte = ""
    '        End If
    '        '*** ***
    '        strCognome = Trim(Request("cognome"))
    '        strNome = Trim(Request("nome"))
    '        strCodiceFiscale = Trim(Request("codicefiscale"))
    '        strPartitaIva = Trim(Request("partitaiva"))
    '        strVia = Trim(Request.Item("Via"))
    '        strCodVia = Trim(Request.Item("CodVia"))
    '        strComuneResidenza = Trim(Request.Item("comuneresidenza"))
    '        strProvinciaResidenza = Trim(Request.Item("provinciaresidenza"))
    '        strDataNascita = Trim(Request.Item("datanascita"))
    '        strDataMorte = Trim(Request.Item("datamorte"))
    '        If Trim(Request("codcontribuente")) <> "" Then
    '            strCODContribuente = Trim(Request("codcontribuente"))
    '        Else
    '            strCODContribuente = ""
    '        End If
    '        If Request("DARICONTROLLARE") <> "" Then
    '            blnDaRicontrollare = Request("DARICONTROLLARE")
    '        Else
    '            blnDaRicontrollare = "0"
    '        End If
    '        '***** Fabiana 30082007 *****
    '        blnNonAgganciate = Trim(Request.Item("NONAGGANCIATE"))
    '        '*** 20140723 ***'*** 201405 - NO RIBESFRAMEWORK ***'**** Fabiana 30082007
    '        Dim nSearchContatti As Integer = -1
    '        If Not Request.Item("ddlContatti") Is Nothing Then
    '            If IsNumeric(Request.Item("ddlContatti")) Then
    '                nSearchContatti = CInt(Request.Item("ddlContatti"))
    '            End If
    '        End If
    '        Dim nSearchInvio As Integer = -1
    '        If Not Request.Item("ddlTributoInvio") Is Nothing Then
    '            If IsNumeric(Request.Item("ddlTributoInvio")) Then
    '                nSearchInvio = CInt(Request.Item("ddlTributoInvio"))
    '            End If
    '        End If
    '        Dim nSearchModulo As Integer = -1
    '        If Not Request.Item("ddlTributoPresente") Is Nothing Then
    '            If IsNumeric(Request.Item("ddlTributoPresente")) Then
    '                nSearchModulo = CInt(Request.Item("ddlTributoPresente"))
    '            End If
    '        End If
    '        '*** ***
    '        If Page.IsPostBack = False Then
    '            ViewState("SortKey") = "NOMINATIVO"
    '            ViewState("OrderBy") = "ASC"

    '            Session.Add(SEARCH_PARAMETRES, GetSearchParametres)
    '            Session("PASSSEARCHANAGRAFICA") = FIRST_TIME
    '            'Log.Debug("SearchResultsAnagrafica:inizializzo dllanagrafica")
    '            objAnagrafica = New ANAGRAFICAWEB.AnagraficaDB(ViewState("SortKey") & " " & ViewState("OrderBy"))
    '            '*** 20140723 ***'*** 201405 - NO RIBESFRAMEWORK ***'**** Fabiana 30082007
    '            'ListAnagrafica = objAnagrafica.GetListAnagragrafica(strCognome, strNome, strCodiceFiscale, strPartitaIva, strCod_Ente, strCODContribuente, blnDaRicontrollare, strVia, strCodVia)
    '            'ListAnagrafica = objAnagrafica.GetListAnagragrafica(Nothing, strCognome, strNome, strCodiceFiscale, strPartitaIva, ANAGRAFICAWEB.ConstSession.IdEnte, strCODContribuente, blnDaRicontrollare, strComuneResidenza, strProvinciaResidenza, strDataNascita, strDataMorte, strVia, strCodVia, blnNonAgganciate)
    '            'Log.Debug("SearchResultsAnagrafica::richiamo GetListAnagragrafica")
    '            'ListAnagrafica = objAnagrafica.GetListAnagragrafica(strCognome, strNome, strCodiceFiscale, strPartitaIva, ANAGRAFICAWEB.ConstSession.IdEnte, strCODContribuente, blnDaRicontrollare, strComuneResidenza, strProvinciaResidenza, strDataNascita, strDataMorte, strVia, strCodVia, blnNonAgganciate)
    '            '*** 201511 - Funzioni Sovracomunali ***
    '            'ListAnagrafica = objAnagrafica.GetListAnagragrafica(False, strCognome, strNome, strCodiceFiscale, strPartitaIva, ANAGRAFICAWEB.ConstSession.IdEnte, strCODContribuente, blnDaRicontrollare, strComuneResidenza, strProvinciaResidenza, strDataNascita, strDataMorte, strVia, strCodVia, blnNonAgganciate, nSearchContatti, nSearchInvio, nSearchModulo)
    '            ListAnagrafica = objAnagrafica.GetListAnagragrafica(False, strCognome, strNome, strCodiceFiscale, strPartitaIva, ANAGRAFICAWEB.ConstSession.Ambiente, sIdEnte, strCODContribuente, blnDaRicontrollare, strComuneResidenza, strProvinciaResidenza, strDataNascita, strDataMorte, strVia, strCodVia, blnNonAgganciate, nSearchContatti, nSearchInvio, nSearchModulo)
    '            '*** ***
    '            Session("ListAnagrafica") = ListAnagrafica
    '            '*** ***
    '            'Log.Debug("GetListAnagragrafica::fatto")
    '            If Not (Session("ListAnagrafica")) Is Nothing Then
    '                'ListAnagrafica = Session("ListAnagrafica")
    '                'GrdAnagrafiche.DataSource = ListAnagrafica.p_dsItemsANAGRAFICA
    '                'Log.Debug("SearchResultsAnagrafica::devo bindare ListAnagrafica.p_dsItemsANAGRAFICA")
    '                'GrdAnagrafiche.DataBind()
    '                'Log.Debug("GetListAnagragrafica::fatto")
    '                LoadSearch()
    '            Else
    '                Log.Debug("SearchResultsAnagrafica::errore!!!!!!!!!!!!!!")
    '            End If
    '        Else
    '            'objAnagrafica = New ANAGRAFICAWEB.AnagraficaDB(ViewState("SortKey") & " " & ViewState("OrderBy"))
    '            ''*** 20140723 ***'*** 201405 - NO RIBESFRAMEWORK ***
    '            ''**** Fabiana 30082007
    '            ''ListAnagrafica = objAnagrafica.GetListAnagragrafica(strCognome, strNome, strCodiceFiscale, strPartitaIva, strCod_Ente, strCODContribuente, blnDaRicontrollare, strVia, strCodVia)
    '            ''ListAnagrafica = objAnagrafica.GetListAnagragrafica(nothing,strCognome, strNome, strCodiceFiscale, strPartitaIva, ANAGRAFICAWEB.ConstSession.IdEnte, strCODContribuente, blnDaRicontrollare, strComuneResidenza, strProvinciaResidenza, strDataNascita, strDataMorte, strVia, strCodVia, blnNonAgganciate)
    '            ''ListAnagrafica = objAnagrafica.GetListAnagragrafica(strCognome, strNome, strCodiceFiscale, strPartitaIva, ANAGRAFICAWEB.ConstSession.IdEnte, strCODContribuente, blnDaRicontrollare, strComuneResidenza, strProvinciaResidenza, strDataNascita, strDataMorte, strVia, strCodVia, blnNonAgganciate)
    '            'ListAnagrafica = Session("ListAnagrafica")
    '            ''*** ***
    '            'GrdAnagrafiche.start_index = GrdAnagrafiche.CurrentPageIndex
    '            'If Not ANAGRAFICAWEB.ConstSession.IdentificativoApplicazione Is Nothing Then
    '            '    GrdAnagrafiche.setDataAdapter(ListAnagrafica.p_daItemsANAGRAFICA, ANAGRAFICAWEB.ConstSession.IdentificativoApplicazione)
    '            'Else
    '            '    GrdAnagrafiche.setDataAdapter(ListAnagrafica.p_daItemsANAGRAFICA, "anag")
    '            'End If
    '        End If
    '        Log.Debug("SearchResultsAnagrafica::GrdResult.Rows.Count:: " & GrdAnagrafiche.Rows.Count.ToString)
    '        Select Case CInt(GrdAnagrafiche.Rows.Count)
    '            Case 0
    '                GrdAnagrafiche.Visible = False
    '                lblMessage.Text = "Non sono state trovate anagrafiche"
    '            Case Is > 0
    '                '*** 201511 - Funzioni Sovracomunali ***
    '                If ANAGRAFICAWEB.ConstSession.IdEnte = "" Then
    '                    GrdAnagrafiche.Columns(0).Visible = True
    '                Else
    '                    GrdAnagrafiche.Columns(0).Visible = False
    '                End If
    '                '*** ***
    '                If ANAGRAFICAWEB.ConstSession.ApplicationsEnabled.IndexOf("8852") > 0 Then
    '                    GrdAnagrafiche.Columns(6).Visible = True
    '                Else
    '                    GrdAnagrafiche.Columns(6).Visible = False
    '                End If
    '                If ANAGRAFICAWEB.ConstSession.ApplicationsEnabled.IndexOf("0434") > 0 Then
    '                    GrdAnagrafiche.Columns(7).Visible = True
    '                Else
    '                    GrdAnagrafiche.Columns(7).Visible = False
    '                End If
    '                If ANAGRAFICAWEB.ConstSession.ApplicationsEnabled.IndexOf("0453") > 0 Then
    '                    GrdAnagrafiche.Columns(8).Visible = True
    '                Else
    '                    GrdAnagrafiche.Columns(8).Visible = False
    '                End If
    '                If ANAGRAFICAWEB.ConstSession.ApplicationsEnabled.IndexOf("9253") > 0 Then
    '                    GrdAnagrafiche.Columns(9).Visible = True
    '                Else
    '                    GrdAnagrafiche.Columns(9).Visible = False
    '                End If
    '                If ANAGRAFICAWEB.ConstSession.ApplicationsEnabled.IndexOf("9000") > 0 Then
    '                    GrdAnagrafiche.Columns(10).Visible = True
    '                Else
    '                    GrdAnagrafiche.Columns(10).Visible = False
    '                End If
    '                If ANAGRAFICAWEB.ConstSession.ApplicationsEnabled.IndexOf("9999") > 0 Then
    '                    GrdAnagrafiche.Columns(11).Visible = True
    '                Else
    '                    GrdAnagrafiche.Columns(11).Visible = False
    '                End If

    '                GrdAnagrafiche.Visible = True
    '        End Select
    '    Catch err As Exception
    '        Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.SearchResultsAnagrafica.Page_Load.errore: ", err)
    '        Response.Redirect("../PaginaErrore.aspx")
    '        Response.Write(GestError.GetHTMLError(err, Server.MapPath("/" & Application("nome_sito") & "/ERRORIANAGRAFICA.css"), "history.back()"))
    '        Response.End()
    '    Finally
    '        RegisterScript("parent.parent.Visualizza.DivAttesa.style.display='none';", Me.GetType)
    '    End Try
    'End Sub
#Region "Griglie"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim IDRow As Integer = CInt(e.CommandArgument.ToString())
            If e.CommandName = "RowOpen" Then
                sScript = "parent.location.href='FormAnagrafica.aspx?popup=0&COD_CONTRIBUENTE=" & IDRow.ToString() & "&ID_DATA_ANAGRAFICA=-1';"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.SearchResultsAnagrafica.GrdRowCommand.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
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
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdSorting(sender As Object, e As GridViewSortEventArgs)
        Try
        Catch ex As Exception
            Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.SearchResultsAnagrafica.GrdSorting.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
        End Try
    End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdAnagrafiche.DataSource = CType(Session("ListAnagrafica"), ANAGRAFICAWEB.ListAnagrafica).p_dsItemsANAGRAFICA
            If page.HasValue Then
                GrdAnagrafiche.PageIndex = page.Value
            End If
            GrdAnagrafiche.DataBind()
        Catch ex As Exception
            Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.SearchResultsAnagrafica.LoadSearch.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub

    '*** 201511 - Funzioni Sovracomunali ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[antonello]	26/01/2005	Created
    ''' </history>
    ''' <revisionHistory>
    ''' <revision date="30/08/2007">
    ''' Fabiana
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="11/2015">
    ''' Funzioni Sovracomunali
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Private Function GetSearchParametres() As Hashtable
        Dim htSearchParametres As New Hashtable
        htSearchParametres.Add("Cognome", strCognome)
        htSearchParametres.Add("Nome", strNome)
        htSearchParametres.Add("CodiceFiscale", strCodiceFiscale)
        htSearchParametres.Add("PartitaIva", strPartitaIva)
        htSearchParametres.Add("CodContribuente", strCODContribuente)
        htSearchParametres.Add("CodEnte", sIdEnte)
        htSearchParametres.Add("PASSING", Session("PASSSEARCHANAGRAFICA"))
        htSearchParametres.Add("DaRicontrollare", blnDaRicontrollare)
        htSearchParametres.Add("NONAGGANCIATE", bNoStradario)
        htSearchParametres.Add("strComuneResidenza", strComuneResidenza)
        htSearchParametres.Add("strProvinciaResidenza", strProvinciaResidenza)
        htSearchParametres.Add("strDataNascita", strDataNascita)
        htSearchParametres.Add("strDataMorte", strDataMorte)
        htSearchParametres.Add("ddlContatti", nSearchContatti)
        htSearchParametres.Add("nSearchInvio", nSearchInvio)
        htSearchParametres.Add("nSearchModulo", nSearchModulo)
        Return htSearchParametres
    End Function
    'Private Function GetSearchParametres() As Hashtable
    '    Dim htSearchParametres As New Hashtable
    '    htSearchParametres.Add("Cognome", strCognome)
    '    htSearchParametres.Add("Nome", strNome)
    '    htSearchParametres.Add("CodiceFiscale", strCodiceFiscale)
    '    htSearchParametres.Add("PartitaIva", strPartitaIva)
    '    htSearchParametres.Add("CodContribuente", strCODContribuente)
    '    '*** 201511 - Funzioni Sovracomunali ***
    '    'htSearchParametres.Add("CodEnte", ANAGRAFICAWEB.ConstSession.IdEnte)
    '    htSearchParametres.Add("CodEnte", sIdEnte)
    '    '*** ***
    '    htSearchParametres.Add("PASSING", Session("PASSSEARCHANAGRAFICA"))
    '    htSearchParametres.Add("DaRicontrollare", blnDaRicontrollare)
    '    '***** Fabiana 30082007 *****
    '    htSearchParametres.Add("NonAgganciate", blnNonAgganciate)
    '    Return htSearchParametres
    'End Function
End Class
