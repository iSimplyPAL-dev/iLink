Imports Anagrafica
Imports System.Web.SessionState.HttpSessionState
Imports System.Data
Imports System.Data.SqlClient
Imports AnagInterface
Imports Utility
Imports log4net
''' <summary>
''' Pagina per la visualizzazione del risultato della ricerca anagrafica da popup.
''' Contiene la griglia dalla quale è possibile accedere al dettaglio.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class SearchResultsAnagraficaGenerale
    Inherits ANAGRAFICAWEB.BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(SearchResultsAnagraficaGenerale))
    'Private GestErrore As New ANAGRAFICAWEB.VisualizzaErrore
    Private FncAnag As DLL.GestioneAnagrafica
    Private oMyAnagrafica As DettaglioAnagrafica
    Private strCognome, strNome, strCodiceFiscale, strPartitaIva, strCODContribuente, strComuneResidenza, strProvinciaResidenza, strDataNascita, strDataMorte, strWFErrore As String
    Private Const SEARCH_PARAMETRES As String = "SEARCHPARAMETRES"
    Private Const FIRST_TIME As String = "1"
    Private blnDaRicontrollare As Boolean
    Private bNoStradario As Boolean
    Private nSearchContatti As Integer
    Private nSearchInvio As Integer
    Private nSearchModulo As Integer
    'Dim GestError As New DLL.GestError
    Private ListAnagrafica As ANAGRAFICAWEB.ListAnagrafica
    Private sScript As String = ""

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents btnNuovo As System.Web.UI.WebControls.Button
    Protected WithEvents btnAnnulla As System.Web.UI.WebControls.Button
    Protected WithEvents lblAnagrafeAziende As System.Web.UI.WebControls.Label
    Protected WithEvents lblAnagrafeResidenti As System.Web.UI.WebControls.Label

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
    ''' <revisionHistory>
    ''' <revision date="05/2014">
    ''' NO RIBESFRAMEWORK
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="23/07/2014">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Esportazione completa dati</em>
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

            If Page.IsPostBack = False Then
                nSearchContatti = -1
                nSearchInvio = -1
                nSearchModulo = -1
                strCognome = Trim(Request("cognome"))
                strNome = Trim(Request("nome"))
            strCodiceFiscale = Trim(Request("codicefiscale"))
            strPartitaIva = Trim(Request("partitaiva"))
            strComuneResidenza = Trim(Request.Item("comuneresidenza"))
            strProvinciaResidenza = Trim(Request.Item("provinciaresidenza"))
            strDataNascita = Trim(Request.Item("datanascita"))
            strDataMorte = Trim(Request.Item("datamorte"))
            strCODContribuente = Trim(Request("codcontribuente"))
                blnDaRicontrollare = Request("DARICONTROLLARE")
                bNoStradario = Utility.StringOperation.FormatBool(Request("NONAGGANCIATE"))
                If Not Request.Item("ddlContatti") Is Nothing Then
                If IsNumeric(Request.Item("ddlContatti")) Then
                    nSearchContatti = CInt(Request.Item("ddlContatti"))
                End If
            End If
            If Not Request.Item("ddlTributoInvio") Is Nothing Then
                If IsNumeric(Request.Item("ddlTributoInvio")) Then
                    nSearchInvio = CInt(Request.Item("ddlTributoInvio"))
                End If
            End If
            If Not Request.Item("ddlTributoPresente") Is Nothing Then
                If IsNumeric(Request.Item("ddlTributoPresente")) Then
                    nSearchModulo = CInt(Request.Item("ddlTributoPresente"))
                End If
            End If

            ViewState("SortKey") = "NOMINATIVO"
            ViewState("OrderBy") = "ASC"
                ViewState("sessionName") = Request.Item("sessionName")
                Session.Add(SEARCH_PARAMETRES, GetSearchParametres())
                Session("PASSSEARCHANAGRAFICA") = FIRST_TIME
                objAnagrafica = New ANAGRAFICAWEB.AnagraficaDB(ViewState("SortKey") & " " & ViewState("OrderBy"))
                ListAnagrafica = objAnagrafica.GetListAnagragrafica(ANAGRAFICAWEB.ConstSession.StringConnectionAnagrafica, False, strCognome, strNome, strCodiceFiscale, strPartitaIva, ANAGRAFICAWEB.ConstSession.Ambiente, ANAGRAFICAWEB.ConstSession.IdEnte, strCODContribuente, blnDaRicontrollare, strComuneResidenza, strProvinciaResidenza, strDataNascita, strDataMorte, "", "", bNoStradario, nSearchContatti, nSearchInvio, nSearchModulo)
                Session("ListAnagrafica") = ListAnagrafica
                GrdAnagrafiche.DataSource = ListAnagrafica.p_dsItemsANAGRAFICA
                GrdAnagrafiche.DataBind()

                Select Case CInt(GrdAnagrafiche.Rows.Count)
                    Case 0
                        GrdAnagrafiche.Visible = False
                        lblMessage.Text = "Non sono state trovate anagrafiche"
                    Case Is > 0
                        GrdAnagrafiche.Visible = True
                        If ANAGRAFICAWEB.ConstSession.IdEnte <> "" Then
                            GrdAnagrafiche.Columns(1).Visible = False
                        End If
                End Select
            End If
        Catch err As Exception
            Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.SearchResultsAnagraficaGenerale.Page_Load.errore: ", err)
            Response.Redirect("../PaginaErrore.aspx")
        Finally
            RegisterScript("$('.divGrdBtn').hide();", Me.GetType())
        End Try
    End Sub
    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    Try

    '        Dim objAnagrafica As ANAGRAFICAWEB.AnagraficaDB

    '        strCognome = Trim(Request("cognome"))

    '        strNome = Trim(Request("nome"))

    '        strCodiceFiscale = Trim(Request("codicefiscale"))

    '        strPartitaIva = Trim(Request("partitaiva"))

    '        strComuneResidenza = Trim(Request.Item("comuneresidenza"))

    '        strProvinciaResidenza = Trim(Request.Item("provinciaresidenza"))

    '        strDataNascita = Trim(Request.Item("datanascita"))

    '        strDataMorte = Trim(Request.Item("datamorte"))

    '        strCODContribuente = Trim(Request("codcontribuente"))
    '        blnDaRicontrollare = Request("DARICONTROLLARE")
    '        '*** 20140723 ***
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
    '            ViewState("sessionName") = Request.Item("sessionName")
    '            Session.Add(SEARCH_PARAMETRES, GetSearchParametres)
    '            Session("PASSSEARCHANAGRAFICA") = FIRST_TIME
    '            objAnagrafica = New ANAGRAFICAWEB.AnagraficaDB(ViewState("SortKey") & " " & ViewState("OrderBy"))
    '            '*** 20140723 ***'*** 201405 - NO RIBESFRAMEWORK ***
    '            'ListAnagrafica = objAnagrafica.GetListAnagragrafica(strCognome, strNome, strCodiceFiscale, strPartitaIva, strCod_Ente, strCODContribuente, blnDaRicontrollare, strComuneResidenza, strProvinciaResidenza, strDataNascita, strDataMorte)
    '            'ListAnagrafica = objAnagrafica.GetListAnagragrafica(strCognome, strNome, strCodiceFiscale, strPartitaIva, ANAGRAFICAWEB.ConstSession.IdEnte, strCODContribuente, blnDaRicontrollare, strComuneResidenza, strProvinciaResidenza, strDataNascita, strDataMorte, "", "", False)
    '            ListAnagrafica = objAnagrafica.GetListAnagragrafica(False, strCognome, strNome, strCodiceFiscale, strPartitaIva, ANAGRAFICAWEB.ConstSession.Ambiente, ANAGRAFICAWEB.ConstSession.IdEnte, strCODContribuente, blnDaRicontrollare, strComuneResidenza, strProvinciaResidenza, strDataNascita, strDataMorte, "", "", False, nSearchContatti, nSearchInvio, nSearchModulo)
    '            Session("ListAnagrafica") = ListAnagrafica
    '            '*** ***
    '            GrdAnagrafiche.DataSource = ListAnagrafica.p_dsItemsANAGRAFICA
    '            GrdAnagrafiche.DataBind()

    '            Select Case CInt(GrdAnagrafiche.Rows.Count)
    '                Case 0
    '                    GrdAnagrafiche.Visible = False
    '                    lblMessage.Text = "Non sono state trovate anagrafiche"
    '                Case Is > 0
    '                    GrdAnagrafiche.Visible = True
    '                    If ANAGRAFICAWEB.ConstSession.IdEnte <> "" Then
    '                        GrdAnagrafiche.Columns(1).Visible = False
    '                    End If
    '            End Select
    '        End If
    '    Catch err As Exception
    '        Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.SearchResultsAnagraficaGenerale.Page_Load.errore: ", err)
    '        Response.Redirect("../PaginaErrore.aspx")
    '    Finally
    '        RegisterScript("$('.divGrdBtn').hide();", Me.GetType())
    '    End Try
    'End Sub
#Region "Griglie"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdRowCommand(ByVal sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim IDRow As Integer = CInt(e.CommandArgument.ToString())
            Select Case e.CommandName
                Case "RowOpen"
                    sScript = "parent.location.href='FormAnagrafica.aspx?sessionName=" & ViewState("sessionName") & "&popup=1&COD_CONTRIBUENTE=" & IDRow.ToString()
                    RegisterScript(sScript, Me.GetType())
                Case "RowBind"
                    Dim sCognome, sNome, sCodFiscale, sPartitaIva As String
                    For Each myRow As GridViewRow In GrdAnagrafiche.Rows
                        If IDRow = CType(myRow.FindControl("hfCODICE_CONTRIBUENTE"), HiddenField).Value Then
                            sCognome = CType(myRow.FindControl("hfCOGNOME_DENOMINAZIONE"), HiddenField).Value
                            sNome = Replace(UCase(CType(myRow.FindControl("hfNOME"), HiddenField).Value), "&NBSP;", "")
                            If sNome <> "" Then
                                sNome = CType(myRow.FindControl("hfNOME"), HiddenField).Value
                            End If
                            sCodFiscale = Replace(UCase(CType(myRow.FindControl("hfCOD_FISCALE"), HiddenField).Value), "&NBSP;", "")
                            If sCodFiscale <> "" Then
                                sCodFiscale = CType(myRow.FindControl("hfCOD_FISCALE"), HiddenField).Value
                            End If
                            sPartitaIva = Replace(UCase(CType(myRow.FindControl("hfPARTITA_IVA"), HiddenField).Value), "&NBSP;", "")
                            If sPartitaIva <> "" Then
                                sPartitaIva = CType(myRow.FindControl("hfPARTITA_IVA"), HiddenField).Value
                            End If
                            CalPageaspx(IDRow, sCognome, sNome, sCodFiscale, sPartitaIva)
                            Return
                        End If
                    Next
            End Select
        Catch ex As Exception
            Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.SearchResultsAnagraficaGenerale.GrdRowCommand.errore: ", ex)
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
            Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.SearchResultsAnagraficaGenerale.GrdAnagrafiche_SortCommand.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="cod_contribuente"></param>
    ''' <param name="Cognome"></param>
    ''' <param name="Nome"></param>
    ''' <param name="CodiceFiscale"></param>
    ''' <param name="PartitaIva"></param>
    Protected Sub CalPageaspx(ByVal cod_contribuente As Integer, ByVal Cognome As String, ByVal Nome As String, ByVal CodiceFiscale As String, ByVal PartitaIva As String)
        Try
            FncAnag = New DLL.GestioneAnagrafica()
            oMyAnagrafica = New DettaglioAnagrafica

            oMyAnagrafica = FncAnag.GetAnagrafica(cod_contribuente, Utility.Costanti.INIT_VALUE_NUMBER, String.Empty, ANAGRAFICAWEB.ConstSession.DBType, ANAGRAFICAWEB.ConstSession.StringConnectionAnagrafica, False)
            Session(ViewState("sessionName")) = oMyAnagrafica
            Dim sScript As String = "parent.parent.opener.document.getElementById('btnRibalta').click();"
            sScript += "parent.parent.window.close();"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.SearchResultsAnagraficaGenerale.CalPageaspx.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnAnnulla_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAnnulla.Click

        Try
            RegisterScript("parent.window.close();", Me.GetType())

        Catch ex As Exception
            Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.SearchResultsAnagraficaGenerale.btnAnnulla_Click.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
        End Try

    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnAssocia_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAssocia.Click
        Try
            Dim COD_CONTRIB As Long = CLng(txtCodContrib.Text)
            Dim sCognome, sNome, sCodFiscale, sPartitaIva As String

            If (GrdAnagrafiche.SelectedIndex >= 0) Then
                For Each myRow As GridViewRow In GrdAnagrafiche.Rows
                    If myRow.RowIndex = GrdAnagrafiche.SelectedIndex Then
                        sCognome = myRow.Cells(9).Text()
                        sNome = Replace(UCase(myRow.Cells(10).Text()), "&NBSP;", "")
                        If sNome <> "" Then
                            sNome = myRow.Cells(10).Text()
                        End If

                        sCodFiscale = Replace(UCase(myRow.Cells(11).Text()), "&NBSP;", "")
                        If sCodFiscale <> "" Then
                            sCodFiscale = myRow.Cells(11).Text()
                        End If
                        sPartitaIva = Replace(UCase(myRow.Cells(12).Text()), "&NBSP;", "")
                        If sPartitaIva <> "" Then
                            sPartitaIva = myRow.Cells(12).Text()
                        End If
                        CalPageaspx(COD_CONTRIB, sCognome, sNome, sCodFiscale, sPartitaIva)
                        Return
                    End If
                Next
            End If
        Catch ex As Exception
            Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.SearchResultsAnagraficaGenerale.btnAssocia_Click.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
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
        htSearchParametres.Add("CodEnte", ANAGRAFICAWEB.ConstSession.IdEnte)
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
    '    Dim htSearchParametres As Hashtable = New Hashtable

    '    htSearchParametres.Add("Cognome", strCognome)
    '    htSearchParametres.Add("Nome", strNome)
    '    htSearchParametres.Add("CodiceFiscale", strCodiceFiscale)
    '    htSearchParametres.Add("PartitaIva", strPartitaIva)
    '    htSearchParametres.Add("CodContribuente", strCODContribuente)
    '    htSearchParametres.Add("CodEnte", ANAGRAFICAWEB.ConstSession.IdEnte)
    '    htSearchParametres.Add("PASSING", Session("PASSSEARCHANAGRAFICA"))
    '    htSearchParametres.Add("DaRicontrollare", blnDaRicontrollare)
    '    Return htSearchParametres
    'End Function
End Class


