Imports System.Configuration
Imports System.Runtime.Remoting
Imports System.Runtime.Remoting.Channels
Imports System.Runtime.Remoting.Channels.Http
Imports System.IO
Imports System.Xml
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters
Imports System.Collections
Imports ComPlusInterface
Imports log4net
''' <summary>
''' Pagina per la consultazione dei provvedimenti di un soggetto.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class LoadAtti
    Inherits BasePage
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(LoadAtti))

    Dim ModDate As New ModificaDate
    Protected strCOGNOME As String = ""
    Protected strNOME As String = ""
    Protected strCODICEFISCALE As String = ""
    Protected strPARTITAIVA As String = ""
    Protected strNUMEROPROVVEDIMENTO As String = ""
    Protected strIDNOMINATIVO As String = ""
    Protected strIDDATAANAGRAFICA As String = ""
    Private Const SEARCH_PARAMETRES_RICERCA_SEMPLICE As String = "SEARCH_PARAMETRES_RICERCA_SEMPLICE"
    Dim blnSearchProvvedimento As Boolean = False
    Dim objHashTable As Hashtable = New Hashtable
    Protected strCodContribuente As String = ""

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
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim dt As New DataView

        objHashTable.Add("COD_CONTRIBUENTE", Utility.StringOperation.FormatInt(Request("COD_CONTRIBUENTE")))
        objHashTable.Add("CODTRIBUTO", "")
        objHashTable.Add("PARAMETROORDINAMENTOGRIGLIA", "")
        objHashTable.Add("CODENTE", ConstSession.IdEnte)

        Try
            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
            objHashTable.Add("CONNECTIONSTRINGANAGRAFICA", ConstSession.StringConnectionAnagrafica)

            If Page.IsPostBack = False Then
                Dim objCOMRicerca As IElaborazioneAtti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAtti), ConstSession.URLServiziElaborazioneAtti)
                Dim objDS As DataSet

                objDS = objCOMRicerca.getProvvedimentiContribuente(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable)

                Session.Remove("PROVVEDIMENTI_CONTRIBUENTE")
                Session("PROVVEDIMENTI_CONTRIBUENTE") = objDS
                Session("HASH_PROVVEDIMENTI_CONTRIBUENTE") = objHashTable

                If Not objDS Is Nothing Then
                    ViewState("SortKey") = "ANNO"
                    ViewState("OrderBy") = "DESC"
                    dt = objDS.Tables(0).DefaultView
                    dt.Sort = ViewState("SortKey") & " " & ViewState("OrderBy")
                End If

                GrdAtti.DataSource = dt
                GrdAtti.DataBind()
                If CInt(GrdAtti.Rows.Count) = 0 Then
                    GrdAtti.Visible = False
                    If Utility.StringOperation.FormatInt(Request("COD_CONTRIBUENTE")) > 0 Then
                        lblMessage.Text = "Non sono stati trovati Atti per la ricerca effettuata"
                    End If
                End If
                End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.LoadAtti.Page_Load.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    'Dim str As String
    '    Dim dt As DataView
    '    'Dim remCanale As HttpChannel
    '    'Dim strConnectionStringOPENgovICI As String
    '    'Dim strConnectionStringOPENgovProvvedimenti As String
    '    'Dim strConnectionStringAnagrafica As String
    '    'Dim strConnectionStringOPENgovTerritorio As String
    '    'Dim strConnectionStringOPENgovUTILITA As String
    '    'dim sScript as string=""
    '    'dim sScript as string=""
    '    Dim objUtility As New MyUtility

    '    objHashTable.Add("COD_CONTRIBUENTE", objUtility.CToStr(Request("COD_CONTRIBUENTE")))
    '    'objHashTable.Add("CODTRIBUTO", Session("COD_TRIBUTO"))
    '    objHashTable.Add("CODTRIBUTO", "")
    '    objHashTable.Add("PARAMETROORDINAMENTOGRIGLIA", "")
    '    objHashTable.Add("CODENTE", ConstSession.IdEnte)
    '    objHashTable.Add("IDSOTTOAPPLICAZIONEANAGRAFICA", ConfigurationManager.AppSettings("OPENGOVA"))
    '    objHashTable.Add("IDSOTTOAPPLICAZIONETERRITORIO", ConfigurationManager.AppSettings("OPENGOVT"))
    '    objHashTable.Add("IDSOTTOAPPLICAZIONEUTILITA", ConfigurationManager.AppSettings("OPENGOVU"))
    '    objHashTable.Add("IDSOTTOAPPLICAZIONEICI", ConfigurationManager.AppSettings("OPENGOVI"))

    '    Try
    '        objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
    '        objHashTable.Add("CONNECTIONSTRINGANAGRAFICA", ConstSession.StringConnectionAnagrafica)

    '        If Page.IsPostBack = False Then
    '            Dim objCOMRicerca As IElaborazioneAtti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAtti), ConstSession.URLServiziElaborazioneAtti)
    '            Dim objDS As DataSet

    '            objDS = objCOMRicerca.getProvvedimentiContribuente(objHashTable)

    '            Session.Remove("PROVVEDIMENTI_CONTRIBUENTE")
    '            Session("PROVVEDIMENTI_CONTRIBUENTE") = objDS
    '            Session("HASH_PROVVEDIMENTI_CONTRIBUENTE") = objHashTable

    '            If Not objDS Is Nothing Then
    '                ViewState("SortKey") = "ANNO"
    '                ViewState("OrderBy") = "DESC"
    '                dt = objDS.Tables(0).DefaultView
    '                dt.Sort = ViewState("SortKey") & " " & ViewState("OrderBy")
    '            End If

    '            GrdAtti.DataSource = dt
    '            GrdAtti.DataBind()
    '            Select Case CInt(GrdAtti.Rows.Count)
    '                Case 0
    '                    GrdAtti.Visible = False
    '                    lblMessage.Text = "Non sono stati trovati Atti per la ricerca effettuata"
    '            End Select
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.LoadAtti.Page_Load.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    End Try
    'End Sub

#Region "Griglie"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdRowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            If (e.Row.RowType = DataControlRowType.DataRow) Then
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.LoadAtti.GrdRowDataBound.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim IDRow As String = e.CommandArgument.ToString()
            If e.CommandName = "RowOpen" Then
                For Each myRow As GridViewRow In GrdAtti.Rows
                    If IDRow = CType(myRow.FindControl("hfID_PROVVEDIMENTO"), HiddenField).Value Then
                        Dim intIDProvvedimento As Integer
                        Dim intCodTipoProvvedimento As Integer
                        Dim strTipoTributo As String
                        Dim strAnno As String
                        Dim strNumeroAtto As String
                        Dim strTipoProcedimento As String

                        strAnno = myRow.Cells(0).Text()

                        strTipoTributo = myRow.Cells(1).Text().Replace("'", " ")
                        strNumeroAtto = myRow.Cells(2).Text()
                        Session("COD_TRIBUTO") = CType(myRow.FindControl("hfcod_tributo"), HiddenField).Value

                        intIDProvvedimento = CInt(IDRow)
                        intCodTipoProvvedimento = CInt(CType(myRow.FindControl("hfcod_tipo_PROVVEDIMENTO"), HiddenField).Value)
                        strTipoProcedimento = CType(myRow.FindControl("hfcod_tipo_procedimento"), HiddenField).Value
                        CalPageaspx(intIDProvvedimento, strTipoTributo, strAnno, strNumeroAtto, intCodTipoProvvedimento, strTipoTributo, strTipoProcedimento)
                    End If
                Next
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.LoadAtti.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
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
    'Private Sub GrdAtti_SortCommand(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles GrdAtti.SortCommand
    '    Dim strSortKey As String
    '    Dim dt As DataView
    'Try
    '    If e.SortExpression.ToString() = ViewState("SortKey").ToString() Then
    '        Select Case ViewState("OrderBy").ToString()
    '            Case "ASC"
    '                ViewState("OrderBy") = "DESC"

    '            Case "DESC"
    '                ViewState("OrderBy") = "ASC"
    '        End Select
    '    Else
    '        ViewState("SortKey") = e.SortExpression
    '        ViewState("OrderBy") = "ASC"
    '    End If

    '    dt = objDS.Tables(0).DefaultView
    '    dt.Sort = ViewState("SortKey") & " " & ViewState("OrderBy")

    '    GrdAtti.DataSource = dt
    ' Catch ex As Exception
    '   Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.LoadAtti.GrdAtti_SortCommand.errore: ", ex)
    '  Response.Redirect("../../../PaginaErrore.aspx")
    'End Try
    'End Sub
    'Public Sub RibesDataGrid_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GrdAtti.SelectedIndexChanged

    '    Dim intIDProvvedimento As Integer
    '    Dim intCodTipoProvvedimento As Integer

    '    Dim strTipoTributo As String
    '    Dim strTipoProvvedimento As String
    '    Dim strAnno As String
    '    Dim strNumeroAtto As String
    '    Dim strTipoProcedimento As String


    '    strAnno = GrdAtti.SelectedItem.Cells(0).Text()

    '    strTipoTributo = GrdAtti.SelectedItem.Cells(1).Text()
    '    strNumeroAtto = GrdAtti.SelectedItem.Cells(2).Text()
    '    Session("COD_TRIBUTO") = GrdAtti.SelectedItem.Cells(10).Text()

    '    intIDProvvedimento = CInt(GrdAtti.SelectedItem.Cells(7).Text())
    '    intCodTipoProvvedimento = CInt(GrdAtti.SelectedItem.Cells(8).Text())
    '    strTipoProcedimento = GrdAtti.SelectedItem.Cells(9).Text()
    '    CalPageaspx(intIDProvvedimento,
    '                strTipoTributo,
    '                strTipoProvvedimento,
    '                strAnno,
    '                strNumeroAtto,
    '                intCodTipoProvvedimento, strTipoTributo, strTipoProcedimento)
    'End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdAtti.DataSource = CType(Session("PROVVEDIMENTI_CONTRIBUENTE"), DataSet)
            If page.HasValue Then
                GrdAtti.PageIndex = page.Value
            End If
            GrdAtti.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.LoadAtti.LoadSearch.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="intIDProvvedimento"></param>
    ''' <param name="strTipoTributo"></param>
    ''' <param name="strAnno"></param>
    ''' <param name="strNumeroAtto"></param>
    ''' <param name="intCodTipoProvvedimento"></param>
    ''' <param name="strDescTipoTributo"></param>
    ''' <param name="strTipoProcedimento"></param>
    Protected Sub CalPageaspx(ByVal intIDProvvedimento As Integer, ByVal strTipoTributo As String, ByVal strAnno As String, ByVal strNumeroAtto As String, ByVal intCodTipoProvvedimento As Integer, ByVal strDescTipoTributo As String, ByVal strTipoProcedimento As String)
        Dim objUtility As New MyUtility
        Dim myParam As String
        Dim sScript As String
        Try
            myParam = "?IDPROVVEDIMENTO=" & intIDProvvedimento
            myParam = myParam & "&TIPOTRIBUTO=" & Replace(strTipoTributo, "'", "&quot;")
            'strPARAMETRI = strPARAMETRI & "&TIPOPROVVEDIMENTO=" & strTipoProvvedimento
            myParam = myParam & "&ANNO=" & strAnno
            myParam = myParam & "&NUMEROATTO=" & strNumeroAtto
            myParam = myParam & "&IDTIPOPROVVEDIMENTO=" & intCodTipoProvvedimento
            myParam = myParam & "&DESCTRIBUTO=" & Replace(strDescTipoTributo, "'", "&quot;")
            myParam = myParam & "&TIPOPROCEDIMENTO=" & strTipoProcedimento
            myParam = myParam & "&PAGINAPRECEDENTE=SEMPLICE"

            Session("ParamGestioneAtti") = myParam
            Session("CODICE_CONTRIBUENTE_RICERCA_SEMPLICE") = objUtility.CToStr(Request("COD_CONTRIBUENTE"))

            sScript = "parent.parent.location.href='../GestioneAtti.aspx" & myParam & "';"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.LoadAtti.CallPageaspx.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Private Function GetSearchParametres() As Hashtable

        Dim htSearchParametres As Hashtable = New Hashtable
        Try
            strCOGNOME = CType(Request("COGNOME"), String)
            strNOME = CType(Request("NOME"), String)
            strCODICEFISCALE = CType(Request("CODICEFISCALE"), String)
            strPARTITAIVA = CType(Request("PARTITAIVA"), String)
            strNUMEROPROVVEDIMENTO = CType(Request("NUMEROPROVVEDIMENTO"), String)

            htSearchParametres.Add("COGNOME", strCOGNOME)
            htSearchParametres.Add("NOME", strNOME)
            htSearchParametres.Add("CODICEFISCALE", strCODICEFISCALE)
            htSearchParametres.Add("PARTITAIVA", strPARTITAIVA)
            htSearchParametres.Add("NUMEROPROVVEDIMENTO", strNUMEROPROVVEDIMENTO)

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.LoadAtti.GetSearchParametres.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
        Return htSearchParametres
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="prdStatus"></param>
    ''' <returns></returns>
    Protected Function GiraData(ByVal prdStatus As Object) As String
        Dim objUtility As New MyUtility

        GiraData = ModDate.GiraDataFromDB(objUtility.CToStr(prdStatus))
        Return GiraData
    End Function

    'Protected Function Stato(ByVal prdIDPROVVEDIMENTO As Object, ByVal prdTIPO_PROVVEDIMENTO As Object) As String
    '    Dim myStato As String
    '    Try
    '        Dim objUtility As New MyUtility
    '        Dim objProvvedimenti As New DBPROVVEDIMENTI.ProvvedimentiDB

    '        myStato = objProvvedimenti.getStato(objUtility.CToStr(prdIDPROVVEDIMENTO), objUtility.CToStr(prdTIPO_PROVVEDIMENTO), CType(Session("PROVVEDIMENTI_CONTRIBUENTE"), DataSet))
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.LoadAtti.Stato.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '        myStato = ""
    '    End Try
    '    Return myStato
    'End Function
End Class
