Imports Anagrafica
Imports System.Data
Imports System.Data.SqlClient

Imports System.Runtime.Remoting.Channels
Imports System.Collections
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters
Imports Utility

Imports RemotingInterfaceAnater
Imports Anater.Oggetti
Imports log4net
'// -----------------------------------------------------------------------------
'// Project	 : Anagrafica
'// Class	 : SearchResultsAnagrafica
'// 
'// -----------------------------------------------------------------------------
'// <summary>
'// 
'// </summary>
'// <remarks>
'// </remarks>
'// <history>
'// 	[antonello]	25/01/2005	Created
'// </history>
'// -----------------------------------------------------------------------------
Partial Class SearchResultsAnagrafica
    Inherits BasePage
    'Private GestErrore As New VisualizzaErrore
    Private strCognome, strNome, strCodiceFiscale, strPartitaIva, strCod_Ente, strCODContribuente, strWFErrore As String
    Private Const SEARCH_PARAMETRES As String = "SEARCHPARAMETRES"
    Private Const FIRST_TIME As String = "1"
    Private blnDaRicontrollare As Boolean
    Private popup As String = "0"
    Private Shared Log As ILog = LogManager.GetLogger(GetType(SearchResultsAnagrafica))


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
    '// -----------------------------------------------------------------------------
    '// <summary>
    '// 
    '// </summary>
    '// <param name="sender"></param>
    '// <param name="e"></param>
    '// <remarks>
    '// </remarks>
    '// <history>
    '// 	[antonello]	24/01/2005	Created
    '// </history>
    '// -----------------------------------------------------------------------------
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim GestError As New DLL.GestError

        ' OGGETTO PER LA CONNESSIONE ALL?ANAGRAFICA DI ANATER
        Dim remObject As IRemotingInterfaceICI = Activator.GetObject(GetType(IRemotingInterfaceICI), ConfigurationManager.AppSettings("URLanaterICI"))

        Dim ListAnagraficaAnater() As OggettoAnagraficaAnater

        Try

            'Dim objAnagrafica As ANAGRAFICAWEB.AnagraficaDB
            'Dim ListAnagrafica As ANAGRAFICAWEB.ListAnagrafica

            strCognome = Trim(Request("cognome"))

            strNome = Trim(Request("nome"))

            strCodiceFiscale = Trim(Request("codicefiscale"))

            strPartitaIva = Trim(Request("partitaiva"))

            strCODContribuente = Trim(Request("codcontribuente"))
            blnDaRicontrollare = CBool(Request("DARICONTROLLARE"))

            If Not IsNothing(Request.Item("popup")) Then
                popup = Request.Item("popup")
            End If

            If Page.IsPostBack = False Then

                ViewState("SortKey") = "NOMINATIVO"
                ViewState("OrderBy") = TipoOrdinamento.Crescente

                Session.Add(SEARCH_PARAMETRES, GetSearchParametres)
                Session("PASSSEARCHANAGRAFICA") = FIRST_TIME

                'objAnagrafica = New ANAGRAFICAWEB.AnagraficaDB(ViewState("SortKey") & " " & ViewState("OrderBy"))


                ListAnagraficaAnater = remObject.GetAnagraficaANATER(strCognome, strNome, strCodiceFiscale, strPartitaIva, CInt(COSTANTValue.ConstSession.IdEnte), 0)

                'ListAnagraficaAnater(0).AnagCognome()
                'ListAnagraficaAnater(0).AnagNome()
                'ListAnagraficaAnater(0).AnagCodFisc()
                'ListAnagraficaAnater(0).AnagPartIva()
                'ListAnagraficaAnater(0).AnagDataNascita()
                'ListAnagraficaAnater(0).AnagSesso()


                'ListAnagrafica = objAnagrafica.GetListAnagragrafica(strCognome, strNome, strCodiceFiscale, strPartitaIva, strCod_Ente, strCODContribuente, blnDaRicontrollare)

                ' ORDINO PER COGNOME
                Dim objComparer As New Comparatore(New String() {"AnagCognome", "AnagNome"}, New Boolean() {ViewState("OrderBy"), ViewState("OrderBy")})

                Array.Sort(ListAnagraficaAnater, objComparer)

                GrdAnagrafica.DataSource = ListAnagraficaAnater
                'Session.Add("ListAnagraficaAnater", ListAnagraficaAnater)
                Session.Add("ListAnagraficaAnater", ListAnagraficaAnater)
                GrdAnagrafica.DataBind()


            Else

                'objAnagrafica = New ANAGRAFICAWEB.AnagraficaDB(ViewState("SortKey") & " " & ViewState("OrderBy"))
                'ListAnagrafica = objAnagrafica.GetListAnagragrafica(strCognome, strNome, strCodiceFiscale, strPartitaIva, strCod_Ente, strCODContribuente, blnDaRicontrollare)
                'GrdAnagrafica.setDataAdapter(ListAnagrafica.p_daItemsANAGRAFICA, Session("Anagrafica"))
                'GrdAnagrafica.DataSource = CType(Session("ListAnagraficaAnater"), OggettoAnagraficaAnater())

                'GrdAnagrafica.DataBind()

            End If

        Catch err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.SearchResultsAnagrafica.Page_Load.errore: ", err)
            Response.Redirect("../../PaginaErrore.aspx")
            Response.Write(GestError.GetHTMLError(err, Server.MapPath("/" & Application("nome_sito").tostring & "/ERRORIANAGRAFICA.css"), "history.back()"))
            Response.End()
        End Try

    End Sub
#Region "Griglie"
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Dim strCodFiscale As String
        Dim intIdRiga As Integer
        Try
            Dim IDRow As Integer = CInt(e.CommandArgument.ToString())
            If e.CommandName = "RowOpen" Then
                For Each myRow As GridViewRow In GrdAnagrafica.Rows
                    If IDRow = CType(myRow.FindControl("hfidriga"), HiddenField).Value Then
                        strCodFiscale = myRow.Cells(1).Text
                        intIdRiga = IDRow
                        CalPageaspx(strCodFiscale, intIdRiga)
                    End If
                Next
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.SearchResultsAnagrafica.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
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
    Protected Sub GrdSorting(sender As Object, e As GridViewSortEventArgs)
        Try
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.SearchResultsAnagrafica.GrdSorting.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Public Sub RibesDataGrid_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim intIDDataAnagrafica As Integer
    '    Dim intCod_Contribuente As Integer
    '    Dim strCodFiscale As String
    '    Dim intIdRiga As Integer

    '    'intIDDataAnagrafica = CInt(GrdAnagrafica.SelectedItem.Cells(7).Text())
    '    'intCod_Contribuente = CInt(GrdAnagrafica.SelectedItem.Cells(6).Text())
    '    strCodFiscale = GrdAnagrafica.SelectedItem.Cells(1).Text
    '    intIdRiga = Convert.ToInt32(GrdAnagrafica.SelectedItem.Cells(4).Text)


    '    CalPageaspx(strCodFiscale, intIdRiga)

    'End Sub

    'Private Sub GrdAnagrafica_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles GrdAnagrafica.SortCommand

    '    Dim strSortKey As String
    '    Dim OrderBy As Boolean
    'Try
    '    If e.SortExpression.ToString() = ViewState("SortKey").ToString() Then
    '        Select Case ViewState("OrderBy")
    '            Case TipoOrdinamento.Crescente
    '                ViewState("OrderBy") = TipoOrdinamento.Decrescente

    '            Case TipoOrdinamento.Decrescente
    '                ViewState("OrderBy") = TipoOrdinamento.Crescente
    '        End Select
    '    Else
    '        ViewState("SortKey") = e.SortExpression
    '        ViewState("OrderBy") = TipoOrdinamento.Crescente
    '    End If

    '    Dim bindObj As OggettoAnagraficaAnater() = CType(Session("ListAnagraficaAnater"), OggettoAnagraficaAnater())

    '    If Not IsNothing(bindObj) Then
    '        ' ORDINO L'ARRAY DI OGGETTI

    '        Dim objComparer As New Comparatore(New String() {"AnagCognome", "AnagNome"}, New Boolean() {ViewState("OrderBy"), ViewState("OrderBy")})

    '        Array.Sort(bindObj, objComparer)

    '    End If

    '    GrdAnagrafica.start_index = 0
    '    GrdAnagrafica.DataSource = bindObj

    '    Session("ListAnagraficaAnater") = bindObj

    '    GrdAnagrafica.DataBind()
    'Catch ex As Exception
    '      Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.SearchResultsAnagrafica.GrdAnagrafica_SortCommand.errore: ", ex)
    '    Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdAnagrafica.DataSource = CType(Session("ListAnagraficaAnater"), OggettoAnagraficaAnater())
            If page.HasValue Then
                GrdAnagrafica.PageIndex = page.Value
            End If
            GrdAnagrafica.DataBind()
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.SearchResultsAnagrafica.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub

    '// -----------------------------------------------------------------------------
    '// <summary>
    '// 
    '// </summary>
    '// <param name="intCod_Contribuente"></param>
    '// <param name="intIDDataAnagrafica"></param>
    '// <remarks>
    '// </remarks>
    '// <history>
    '// 	[antonello]	25/01/2005	Created
    '// </history>
    '// -----------------------------------------------------------------------------
    Protected Sub CalPageaspx(ByVal strCodFiscale As String, ByVal idRiga As Integer)
        'Dim _CONST As New ANAGRAFICAWEB.Costanti
        Try
            Dim strBuilder As New System.Text.StringBuilder
            strBuilder.Append("parent.location.href='FormAnagrafica.aspx?popup=" & popup & "&COD_FISCALE=" & strCodFiscale & "&idRiga=" & idRiga.ToString & "';")
            RegisterScript(strBuilder.ToString(), Me.GetType())
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.SearchResultsAnagrafica.CalPageaspx.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Function GetSearchParametres() As Hashtable
        Dim htSearchParametres As New Hashtable
        Try

            htSearchParametres.Add("Cognome", strCognome)
            htSearchParametres.Add("Nome", strNome)
            htSearchParametres.Add("CodiceFiscale", strCodiceFiscale)
            htSearchParametres.Add("PartitaIva", strPartitaIva)
            htSearchParametres.Add("CodContribuente", strCODContribuente)
            htSearchParametres.Add("CodEnte", strCod_Ente)
            htSearchParametres.Add("PASSING", Session("PASSSEARCHANAGRAFICA"))
            htSearchParametres.Add("DaRicontrollare", blnDaRicontrollare)


        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.SearchResultsAnagrafica.GetSearchParametres.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
        Return htSearchParametres
    End Function

    Public Function FormattaDataGrd(ByVal tDataGrd As DateTime) As String
        Try
            If tDataGrd = Date.MinValue Or tDataGrd = Date.MaxValue Then
                Return ""
            Else
                Return tDataGrd.ToShortDateString.ToString
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.SearchResultsAnagrafica.FormattaDataGrd.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Return ""
        End Try
    End Function
End Class
