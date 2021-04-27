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
''' Pagina per la visualizzazione avanzata dei provvedimenti.
''' Contiene la griglia per la visualizzazione del risultato. 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class SearchAttiRicercaAvanzata
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(SearchAttiRicercaAvanzata))
    Dim ModDate As New ModificaDate
    Protected strNOMINATIVO As String = ""
    Protected strNUMEROPROVVEDIMENTO As String = ""
    Protected strIDNOMINATIVO As String = ""
    Protected strIDDATAANAGRAFICA As String = ""
    Private Const SESSION_DATE As String = "SESSION_DATE"

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label
    Protected WithEvents GrdAtti As Ribes.OPENgov.WebControls.RibesGridView
    Protected WithEvents txtDateRicorso As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label37 As System.Web.UI.WebControls.Label
    Protected WithEvents lblNumeroTotaleAvvisi As System.Web.UI.WebControls.Label
    Protected WithEvents Label38 As System.Web.UI.WebControls.Label
    Protected WithEvents lblImportoTotaleAvvisi As System.Web.UI.WebControls.Label
    Protected WithEvents Label39 As System.Web.UI.WebControls.Label
    Protected WithEvents lblRettifiche As System.Web.UI.WebControls.Label
    Protected WithEvents lblContribuenti As System.Web.UI.WebControls.Label
    Protected WithEvents lblTotContribuenti As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents lblImportoTotaleAvvisiRidotto As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents lblRettificheRidotto As System.Web.UI.WebControls.Label
    Protected WithEvents btnElaboraProvvedimenti As System.Web.UI.WebControls.Button
    Protected WithEvents btnElaboraBollettini As System.Web.UI.WebControls.Button

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
        Dim myDataSet As DataSet
        Dim dt As New DataView
        Dim objHashTable As New Hashtable
        Dim mySearch As New ObjSearchAtti
        Dim sScript As String = ""

        Try
            If Not Page.IsPostBack Then
                If Not IsNothing(Session("oSearchAttiAvanzata")) Then
                    mySearch = Session("oSearchAttiAvanzata")
                    objHashTable.Add("AMBIENTE", ConstSession.Ambiente)
                    objHashTable.Add("CODENTE", mySearch.IdEnte)
                    objHashTable.Add("CODTRIBUTO", mySearch.Tributo)
                    objHashTable.Add("ANNO", mySearch.Anno)
                    objHashTable.Add("TIPOPROVVEDIMENTO", mySearch.TipoProv)
                    objHashTable.Add("PARAMDATE", mySearch)
                    objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)

                    Session(SESSION_DATE) = objHashTable
                    If Not IsNothing(Session("paginaSelezionata")) Then
                        Session.Remove("paginaSelezionata")

                        myDataSet = CType(Session("SELEZIONE_PROVVEDIMENTI_MASSIVA"), DataSet)
                        dt = myDataSet.Tables("TP_ATTI_RICERCA_AVANZATA").DefaultView
                        ViewState("SortKey") = "ANNO"
                        ViewState("OrderBy") = "DESC"
                        dt.Sort = ViewState("SortKey") & " " & ViewState("OrderBy")
                        GrdAtti.DataSource = dt
                        GrdAtti.DataBind()
                    Else
                        Dim objCOMRicerca As IElaborazioneAtti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAtti), ConstSession.URLServiziElaborazioneAtti)
                        myDataSet = objCOMRicerca.GetDatiAttiRicercaAvanzata(ConstSession.StringConnection, mySearch.IdEnte, objHashTable, mySearch)

                        Session.Remove("SELEZIONE_PROVVEDIMENTI_MASSIVA")
                        Session("SELEZIONE_PROVVEDIMENTI_MASSIVA") = myDataSet
                        Session("HashTableAttiRicercaMassiva") = objHashTable
                        Session("TP_RICERCA_AVANZATA_PER_STAMPA") = myDataSet.Tables("TP_RICERCA_AVANZATA_PER_STAMPA")
                        If Not myDataSet Is Nothing Then
                            ViewState("SortKey") = "ANNO"
                            ViewState("OrderBy") = "DESC"
                            dt = myDataSet.Tables("TP_ATTI_RICERCA_AVANZATA").DefaultView
                            dt.Sort = ViewState("SortKey") & " " & ViewState("OrderBy")
                        End If
                        GrdAtti.DataSource = dt
                        GrdAtti.DataBind()
                    End If
                    If CInt(GrdAtti.Rows.Count) = 0 Then
                        GrdAtti.Visible = False
                        lblMessage.Text = "Non sono stati trovati Atti per la ricerca effettuata"
                    ElseIf CInt(GrdAtti.Rows.Count) > 0 Then
                        '*** 201511 - Funzioni Sovracomunali ***
                        If ConstSession.IdEnte = "" Then
                            GrdAtti.Columns(0).Visible = True
                        Else
                            GrdAtti.Columns(0).Visible = False
                            sScript = "parent.parent.Comandi.Excel.style.display='';"
                            sScript += "parent.parent.Comandi.btnElaboraProvvedimenti.style.display='';"
                            'SSCRIPT+="parent.parent.Comandi.btnElaboraBollettini.style.display='';")
                            sScript += "parent.document.getElementById('txtRicercaAttiva').value='1';"
                            RegisterScript(sScript, Me.GetType())
                        End If
                        '*** ***

                        sScript = "Riepilogativi.style.display='';"
                        RegisterScript(sScript, Me.GetType())
                        GrdAtti.Visible = True

                        lblNumeroTotaleAvvisi.Text = FormatNumber(dt.Count, 0) 'FormatNumber(GrdAtti.Rows.Count, 0)
                        If myDataSet.Tables("TP_TOTALE_RETTIFICATO").Rows(0).Item("IMPORTO_TOTALE").ToString <> "" Then
                            lblRettifiche.Text = "€ " & FormatNumber(myDataSet.Tables("TP_TOTALE_RETTIFICATO").Rows(0).Item("IMPORTO_TOTALE").ToString, 2)
                        End If
                        If myDataSet.Tables("TP_TOTALE_RETTIFICATO").Rows(0).Item("IMPORTO_TOTALE_RIDOTTO").ToString <> "" Then
                            lblRettificheRidotto.Text = "€ " & FormatNumber(myDataSet.Tables("TP_TOTALE_RETTIFICATO").Rows(0).Item("IMPORTO_TOTALE_RIDOTTO").ToString, 2)
                        End If
                        If myDataSet.Tables("TP_TOTALE_GENERALE").Rows(0).Item("IMPORTO_TOTALE").ToString <> "" Then
                            lblImportoTotaleAvvisi.Text = "€ " & FormatNumber(myDataSet.Tables("TP_TOTALE_GENERALE").Rows(0).Item("IMPORTO_TOTALE").ToString, 2)
                        End If
                        If myDataSet.Tables("TP_TOTALE_GENERALE").Rows(0).Item("IMPORTO_TOTALE_RIDOTTO").ToString <> "" Then
                            lblImportoTotaleAvvisiRidotto.Text = "€ " & FormatNumber(myDataSet.Tables("TP_TOTALE_GENERALE").Rows(0).Item("IMPORTO_TOTALE_RIDOTTO").ToString, 2)
                        End If

                        Dim dwContribuenti As DataView
                        If Not myDataSet Is Nothing Then
                            dwContribuenti = myDataSet.Tables("TP_TOTALE_CONTRIBUENTI").DefaultView
                            lblTotContribuenti.Text = dwContribuenti.Table.Rows.Count
                        Else
                            lblTotContribuenti.Text = "0"
                        End If
                    End If

                    sScript = "parent.attesaGestioneAtti.style.display='none';"
                        RegisterScript(sScript, Me.GetType())
                    End If
                Else
                If Not IsNothing(Session(SESSION_DATE)) Then
                    objHashTable = Session(SESSION_DATE)
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAttiRicercaAvanzata.Page_Load.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    Dim myDataSet As DataSet
    '    Dim dt As DataView
    '    Dim objHashTable As New Hashtable
    '    Dim mySearch As New ObjSearchAtti
    '    Dim sScript As String = ""

    '    Try
    '        If Not Page.IsPostBack Then
    '            mySearch = Session("oSearchAttiAvanzata")
    '            objHashTable.Add("AMBIENTE", ConstSession.Ambiente)
    '            objHashTable.Add("CODENTE", mySearch.IdEnte)
    '            objHashTable.Add("CODTRIBUTO", mySearch.Tributo)
    '            objHashTable.Add("ANNO", mySearch.Anno)
    '            objHashTable.Add("TIPOPROVVEDIMENTO", mySearch.TipoProv)
    '            objHashTable.Add("PARAMDATE", mySearch)
    '            objHashTable.Add("IDSOTTOAPPLICAZIONEANAGRAFICA", ConfigurationManager.AppSettings("OPENGOVA"))
    '            objHashTable.Add("IDSOTTOAPPLICAZIONETERRITORIO", ConfigurationManager.AppSettings("OPENGOVT"))
    '            objHashTable.Add("IDSOTTOAPPLICAZIONEUTILITA", ConfigurationManager.AppSettings("OPENGOVU"))
    '            objHashTable.Add("IDSOTTOAPPLICAZIONEICI", ConfigurationManager.AppSettings("OPENGOVI"))
    '            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)

    '            Session(SESSION_DATE) = objHashTable
    '        Else
    '            objHashTable = Session(SESSION_DATE)
    '        End If

    '        If Page.IsPostBack = False Then
    '            If Not IsNothing(Session("paginaSelezionata")) Then
    '                Session.Remove("paginaSelezionata")

    '                myDataSet = CType(Session("SELEZIONE_PROVVEDIMENTI_MASSIVA"), DataSet)
    '                dt = myDataSet.Tables("TP_ATTI_RICERCA_AVANZATA").DefaultView
    '                ViewState("SortKey") = "ANNO"
    '                ViewState("OrderBy") = "DESC"
    '                dt.Sort = ViewState("SortKey") & " " & ViewState("OrderBy")
    '                GrdAtti.DataSource = dt
    '                GrdAtti.DataBind()
    '            Else
    '                Dim objCOMRicerca As IElaborazioneAtti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAtti), ConstSession.URLServiziElaborazioneAtti)
    '                myDataSet = objCOMRicerca.GetDatiAttiRicercaAvanzata(objHashTable, mySearch)

    '                Session.Remove("SELEZIONE_PROVVEDIMENTI_MASSIVA")
    '                Session("SELEZIONE_PROVVEDIMENTI_MASSIVA") = myDataSet
    '                Session("HashTableAttiRicercaMassiva") = objHashTable
    '                Session("TP_RICERCA_AVANZATA_PER_STAMPA") = myDataSet.Tables("TP_RICERCA_AVANZATA_PER_STAMPA")
    '                If Not myDataSet Is Nothing Then
    '                    ViewState("SortKey") = "ANNO"
    '                    ViewState("OrderBy") = "DESC"
    '                    dt = myDataSet.Tables("TP_ATTI_RICERCA_AVANZATA").DefaultView
    '                    dt.Sort = ViewState("SortKey") & " " & ViewState("OrderBy")
    '                End If
    '                GrdAtti.DataSource = dt
    '                GrdAtti.DataBind()
    '            End If
    '            Select Case CInt(GrdAtti.Rows.Count)
    '                Case 0
    '                    GrdAtti.Visible = False
    '                    lblMessage.Text = "Non sono stati trovati Atti per la ricerca effettuata"
    '                Case Is > 0
    '                    '*** 201511 - Funzioni Sovracomunali ***
    '                    If ConstSession.IdEnte = "" Then
    '                        GrdAtti.Columns(0).Visible = True
    '                    Else
    '                        GrdAtti.Columns(0).Visible = False
    '                        sScript = "parent.parent.Comandi.Excel.style.display='';"
    '                        sScript += "parent.parent.Comandi.btnElaboraProvvedimenti.style.display='';"
    '                        'SSCRIPT+="parent.parent.Comandi.btnElaboraBollettini.style.display='';")
    '                        sScript += "parent.formRicercaAnagrafica.txtRicercaAttiva.value='1';"
    '                        RegisterScript(sScript, Me.GetType())
    '                    End If
    '                    '*** ***

    '                    sScript = "Riepilogativi.style.display='';"
    '                    RegisterScript(sScript, Me.GetType())
    '                    GrdAtti.Visible = True

    '                    lblNumeroTotaleAvvisi.Text = FormatNumber(dt.Count, 0) 'FormatNumber(GrdAtti.Rows.Count, 0)
    '                    If myDataSet.Tables("TP_TOTALE_RETTIFICATO").Rows(0).Item("IMPORTO_TOTALE").ToString <> "" Then
    '                        lblRettifiche.Text = "€ " & FormatNumber(myDataSet.Tables("TP_TOTALE_RETTIFICATO").Rows(0).Item("IMPORTO_TOTALE").ToString, 2)
    '                    End If
    '                    If myDataSet.Tables("TP_TOTALE_RETTIFICATO").Rows(0).Item("IMPORTO_TOTALE_RIDOTTO").ToString <> "" Then
    '                        lblRettificheRidotto.Text = "€ " & FormatNumber(myDataSet.Tables("TP_TOTALE_RETTIFICATO").Rows(0).Item("IMPORTO_TOTALE_RIDOTTO").ToString, 2)
    '                    End If
    '                    If myDataSet.Tables("TP_TOTALE_GENERALE").Rows(0).Item("IMPORTO_TOTALE").ToString <> "" Then
    '                        lblImportoTotaleAvvisi.Text = "€ " & FormatNumber(myDataSet.Tables("TP_TOTALE_GENERALE").Rows(0).Item("IMPORTO_TOTALE").ToString, 2)
    '                    End If
    '                    If myDataSet.Tables("TP_TOTALE_GENERALE").Rows(0).Item("IMPORTO_TOTALE_RIDOTTO").ToString <> "" Then
    '                        lblImportoTotaleAvvisiRidotto.Text = "€ " & FormatNumber(myDataSet.Tables("TP_TOTALE_GENERALE").Rows(0).Item("IMPORTO_TOTALE_RIDOTTO").ToString, 2)
    '                    End If

    '                    Dim dwContribuenti As DataView
    '                    If Not myDataSet Is Nothing Then
    '                        dwContribuenti = myDataSet.Tables("TP_TOTALE_CONTRIBUENTI").DefaultView
    '                        lblTotContribuenti.Text = dwContribuenti.Table.Rows.Count
    '                    Else
    '                        lblTotContribuenti.Text = "0"
    '                    End If
    '            End Select

    '            sScript = "parent.attesaGestioneAtti.style.display='none';"
    '            RegisterScript(sScript, Me.GetType())
    '        End If

    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAttiRicercaAvanzata.Page_Load.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
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

                        If ConstSession.IdEnte = "" Then
                            RegisterScript("GestAlert('a', 'warning', '', '', 'Impossibile accedere al dettaglio dalla funzione sovracomunale');", Me.GetType())
                        Else
                            Session("paginaSelezionata") = GrdAtti.PageIndex

                            strAnno = myRow.Cells(1).Text()

                            strTipoTributo = myRow.Cells(4).Text()
                            strNumeroAtto = myRow.Cells(5).Text()

                            intIDProvvedimento = CInt(IDRow)
                            intCodTipoProvvedimento = CInt(CType(myRow.FindControl("hfCOD_TIPO_PROVVEDIMENTO"), HiddenField).Value)
                            strTipoProcedimento = CType(myRow.FindControl("hfCOD_TIPO_PROCEDIMENTO"), HiddenField).Value
                            Session("COD_TRIBUTO") = CType(myRow.FindControl("hfCOD_TRIBUTO"), HiddenField).Value

                            CalPageaspx(intIDProvvedimento, strTipoTributo, strAnno, strNumeroAtto, intCodTipoProvvedimento, strTipoTributo, strTipoProcedimento)
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAttiRicercaAvanzata.GrdRowCommand.errore: ", ex)
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
    ''*** 201511 - Funzioni Sovracomunali ***
    'Public Sub RibesDataGrid_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GrdAtti.SelectedIndexChanged
    '    Dim intIDProvvedimento As Integer
    '    Dim intCodTipoProvvedimento As Integer

    '    Dim strTipoTributo As String
    '    Dim strTipoProvvedimento As String
    '    Dim strAnno As String
    '    Dim strNumeroAtto As String
    '    Dim strTipoProcedimento As String
    'Try
    '    If ConstSession.IdEnte = "" Then
    '        ClientScript.RegisterScript(Me.[GetType](), "fsra", "<script language='javascript'>alert('Impossibile accedere al dettaglio dalla funzione sovracomunale');</script>")
    '    Else
    '        Session("paginaSelezionata") = GrdAtti.CurrentPageIndex

    '        strAnno = GrdAtti.SelectedItem.Cells(0).Text()

    '        strTipoTributo = GrdAtti.SelectedItem.Cells(3).Text()
    '        strNumeroAtto = GrdAtti.SelectedItem.Cells(4).Text()

    '        intIDProvvedimento = CInt(GrdAtti.SelectedItem.Cells(9).Text())
    '        intCodTipoProvvedimento = CInt(GrdAtti.SelectedItem.Cells(10).Text())
    '        strTipoProcedimento = GrdAtti.SelectedItem.Cells(11).Text()

    '        Session("COD_TRIBUTO") = GrdAtti.SelectedItem.Cells(12).Text()

    '        CalPageaspx(intIDProvvedimento,
    '           strTipoTributo,
    '           strTipoProvvedimento,
    '           strAnno,
    '           strNumeroAtto,
    '           intCodTipoProvvedimento, strTipoTributo, strTipoProcedimento)
    '    End If
    ' Catch ex As Exception
    '      Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAttiRicercaAvanzata.RibesDataGrid_SelectedIndexChanged.errore: ", ex)
    '     Response.Redirect("../../../PaginaErrore.aspx")
    'End Try
    'End Sub
    ''*** ***
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
    '        If e.SortExpression.ToString = "CFPIVA" Then
    '            If ViewState("OrderBy").ToString() = "ASC" Then
    '                ViewState("SortKey") = "PARTITA_IVA DESC,CODICE_FISCALE "
    '                ViewState("OrderBy") = "DESC"
    '            Else
    '                ViewState("SortKey") = "PARTITA_IVA ASC,CODICE_FISCALE "
    '                ViewState("OrderBy") = "ASC"
    '            End If
    '        Else
    '            ViewState("SortKey") = e.SortExpression
    '            ViewState("OrderBy") = "ASC"
    '        End If
    '    End If

    '    dt = objDS.Tables(0).DefaultView
    '    dt.Sort = ViewState("SortKey") & " " & ViewState("OrderBy")

    '    GrdAtti.start_index = 0
    '    GrdAtti.DataSource = dt
    '    GrdAtti.DataBind()
    ' Catch ex As Exception
    '      Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAttiRicercaAvanzata.GrdAtti_SortCommand.errore: ", ex)
    '     Response.Redirect("../../../PaginaErrore.aspx")
    'End Try
    'End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdAtti.DataSource = CType(Session("SELEZIONE_PROVVEDIMENTI_MASSIVA"), DataSet)
            If page.HasValue Then
                GrdAtti.PageIndex = page.Value
            End If
            GrdAtti.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAttiRicercaAvanzata.LoadSearch.errore: ", ex)
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
        Dim strPARAMETRI As String
        Try
            strPARAMETRI = "?IDPROVVEDIMENTO=" & intIDProvvedimento
            strPARAMETRI = strPARAMETRI & "&TIPOTRIBUTO=" & Replace(strTipoTributo, "'", "&quot;")
            'strPARAMETRI = strPARAMETRI & "&TIPOPROVVEDIMENTO=" & strTipoProvvedimento
            strPARAMETRI = strPARAMETRI & "&ANNO=" & strAnno
            strPARAMETRI = strPARAMETRI & "&NUMEROATTO=" & strNumeroAtto
            strPARAMETRI = strPARAMETRI & "&IDTIPOPROVVEDIMENTO=" & intCodTipoProvvedimento
            strPARAMETRI = strPARAMETRI & "&DESCTRIBUTO=" & Replace(strDescTipoTributo, "'", "&quot;")
            strPARAMETRI = strPARAMETRI & "&TIPOPROCEDIMENTO=" & strTipoProcedimento
            strPARAMETRI = strPARAMETRI & "&PAGINAPRECEDENTE=AVANZATA"

            RegisterScript("parent.location.href='../GestioneAtti.aspx" & strPARAMETRI & "';", Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAttiRicercaAvanzata.CallPageaspx.errore: ", ex)
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
            strNOMINATIVO = CType(Request("NOMINATIVO"), String)
            strNUMEROPROVVEDIMENTO = CType(Request("NUMEROPROVVEDIMENTO"), String)
            strIDNOMINATIVO = CType(Request("IDNOMINATIVO"), String)
            strIDDATAANAGRAFICA = CType(Request("IDDATAANAGRAFICA"), String)

            htSearchParametres.Add("NOMINATIVO", strNOMINATIVO)
            htSearchParametres.Add("NUMEROPROVVEDIMENTO", strNUMEROPROVVEDIMENTO)
            htSearchParametres.Add("IDNOMINATIVO", strIDNOMINATIVO)
            htSearchParametres.Add("IDDATAANAGRAFICA", strIDDATAANAGRAFICA)

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAttiRicercaAvanzata.GetSearchParametres.errore: ", ex)
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
    '    Try
    '        Dim objUtility As New MyUtility
    '        Dim strIDProvvedimento As String
    '        Dim strTIPO_PROVVEDIMENTO As String
    '        Dim objProvvedimenti As New DBPROVVEDIMENTI.ProvvedimentiDB
    '        Dim objDS As DataSet
    '        objDS = CType(Session("SELEZIONE_PROVVEDIMENTI_MASSIVA"), DataSet)

    '        strIDProvvedimento = objUtility.CToStr(prdIDPROVVEDIMENTO)
    '        strTIPO_PROVVEDIMENTO = objUtility.CToStr(prdTIPO_PROVVEDIMENTO)

    '        Stato = objProvvedimenti.getStato(strIDProvvedimento, strTIPO_PROVVEDIMENTO, objDS)
    '        Return Stato
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAttiRicercaAvanzata.Stato.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '        Return ""
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="CODICE_FISCALE"></param>
    ''' <param name="PARTITA_IVA"></param>
    ''' <returns></returns>
    Protected Function CFPIVA(ByVal CODICE_FISCALE As Object, ByVal PARTITA_IVA As Object) As String
        Dim objUtility As New MyUtility
        Dim strRet As String = ""
        Try
            If Not IsDBNull(PARTITA_IVA) Then
                If objUtility.CToStr(PARTITA_IVA) <> "" Then
                    strRet = objUtility.CToStr(PARTITA_IVA)
                End If
            End If
            If strRet = "" Then
                If Not IsDBNull(CODICE_FISCALE) Then
                    If objUtility.CToStr(CODICE_FISCALE) <> "" Then
                        strRet = objUtility.CToStr(CODICE_FISCALE)
                    End If
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAttiRicercaAvanzata.CFPIVA.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
        Return strRet
    End Function
    'Private Function ValDate(HasTableOrg As Hashtable) As Hashtable
    '    Dim myHashTable As New Hashtable
    '    Dim myUtility As New MyUtility
    '    Dim blnCHEK_DATAELABORAZIONE As Boolean
    '    Dim blnCHEK_DATACONFERMAAVVISO As Boolean

    '    Try
    '        myHashTable = HasTableOrg
    '        '************************************DATA ELABORAZIONE*******************************************
    '        If myUtility.CToStr(Request("chkDataGenerazione")) = "on" Then
    '            blnCHEK_DATAELABORAZIONE = True
    '        Else
    '            blnCHEK_DATAELABORAZIONE = False
    '        End If

    '        myHashTable.Add("CHEK_DATAELABORAZIONE", blnCHEK_DATAELABORAZIONE)

    '        If myUtility.CToStr(Request("DataGenerazione")).CompareTo("DATA") = 0 Then
    '            myHashTable.Add("OPT_DATAELABORAZIONE", "DATA")
    '        Else
    '            myHashTable.Add("OPT_DATAELABORAZIONE", "NESSUNA")
    '        End If

    '        myHashTable.Add("DATAELABORAZIONE_DAL", myUtility.CToStr(Request("txtDataGenerazioneDal")))
    '        myHashTable.Add("DATAELABORAZIONE_AL", myUtility.CToStr(Request("txtDataGenerazioneAL")))
    '        '************************************FINE DATA ELABORAZIONE*******************************************

    '        '****************************************DATA CONFERMA AVVISO*****************************************
    '        If myUtility.CToStr(Request("chkDataConfermaAvviso")) = "on" Then
    '            blnCHEK_DATACONFERMAAVVISO = True
    '        Else
    '            blnCHEK_DATACONFERMAAVVISO = False
    '        End If

    '        myHashTable.Add("CHEK_DATACONFERMAAVVISO", blnCHEK_DATACONFERMAAVVISO)

    '        If myUtility.CToStr(Request("DataConfermaAvviso")).CompareTo("DATA") = 0 Then
    '            myHashTable.Add("OPT_DATACONFERMAAVVISO", "DATA")
    '        Else
    '            myHashTable.Add("OPT_DATACONFERMAAVVISO", "NESSUNA")
    '        End If

    '        myHashTable.Add("DATACONFERMAAVVISO_DAL", myUtility.CToStr(Request("txtDataConfermaAvvisoDal")))
    '        myHashTable.Add("DATACONFERMAAVVISO_AL", myUtility.CToStr(Request("txtDataConfermaAvvisoAl")))

    '        '****************************************DATA CONFERMA AVVISO*****************************************


    '        '****************************************DATA STAMPA  AVVISO*****************************************
    '        Dim blnCHEK_DATASTAMPAAVVISO As Boolean
    '        If myUtility.CToStr(Request("chkDataStampaAvviso")) = "on" Then
    '            blnCHEK_DATASTAMPAAVVISO = True
    '        Else
    '            blnCHEK_DATASTAMPAAVVISO = False
    '        End If

    '        myHashTable.Add("CHEK_DATASTAMPAAVVISO", blnCHEK_DATASTAMPAAVVISO)

    '        If myUtility.CToStr(Request("DataStampaAvviso")).CompareTo("DATA") = 0 Then
    '            myHashTable.Add("OPT_DATASTAMPAAVVISO", "DATA")
    '        Else
    '            myHashTable.Add("OPT_DATASTAMPAAVVISO", "NESSUNA")
    '        End If

    '        myHashTable.Add("DATASTAMPAAVVISO_DAL", myUtility.CToStr(Request("txtDataStampaAvvisoDal")))
    '        myHashTable.Add("DATASTAMPAAVVISO_AL", myUtility.CToStr(Request("txtDataStampaAvvisoAl")))

    '        '****************************************DATA STAMPA AVVISO*****************************************

    '        '****************************************DATA CONSEGNA  AVVISO*****************************************
    '        Dim blnCHEK_DATACONSEGNAAVVISO As Boolean
    '        If myUtility.CToStr(Request("chkDataConsegnaAvviso")) = "on" Then
    '            blnCHEK_DATACONSEGNAAVVISO = True
    '        Else
    '            blnCHEK_DATACONSEGNAAVVISO = False
    '        End If

    '        myHashTable.Add("CHEK_DATACONSEGNAAVVISO", blnCHEK_DATACONSEGNAAVVISO)

    '        If myUtility.CToStr(Request("DataConsegnaAvviso")).CompareTo("DATA") = 0 Then
    '            myHashTable.Add("OPT_DATACONSEGNAAVVISO", "DATA")
    '        Else
    '            myHashTable.Add("OPT_DATACONSEGNAAVVISO", "NESSUNA")
    '        End If

    '        myHashTable.Add("DATACONSEGNAAVVISO_DAL", myUtility.CToStr(Request("txtDataConsegnaAvvisoDal")))
    '        myHashTable.Add("DATACONSEGNAAVVISO_AL", myUtility.CToStr(Request("txtDataConsegnaAvvisoAl")))

    '        '****************************************DATA CONSEGNA AVVISO*****************************************
    '        '****************************************DATA NOTIFICA  AVVISO*****************************************
    '        Dim blnCHEK_DATANOTIFICAAVVISO As Boolean

    '        If myUtility.CToStr(Request("chkDataNotificaAvviso")) = "on" Then
    '            blnCHEK_DATANOTIFICAAVVISO = True
    '        Else
    '            blnCHEK_DATANOTIFICAAVVISO = False
    '        End If

    '        myHashTable.Add("CHEK_DATANOTIFICAAVVISO", blnCHEK_DATANOTIFICAAVVISO)

    '        If myUtility.CToStr(Request("DataNotificaAvviso")).CompareTo("DATA") = 0 Then
    '            myHashTable.Add("OPT_DATANOTIFICAAVVISO", "DATA")
    '        Else
    '            myHashTable.Add("OPT_DATANOTIFICAAVVISO", "NESSUNA")
    '        End If

    '        myHashTable.Add("DATANOTIFICAAVVISO_DAL", myUtility.CToStr(Request("txtDataNotificaAvvisoDal")))
    '        myHashTable.Add("DATANOTIFICAAVVISO_AL", myUtility.CToStr(Request("txtDataNotificaAvvisoAl")))

    '        '****************************************DATA NOTIFICA AVVISO*****************************************

    '        '****************************************DATA RETTIFICA  AVVISO*****************************************
    '        Dim blnCHEK_DATARETTIFICAAVVISO As Boolean
    '        If myUtility.CToStr(Request("chkDataRettificaAvviso")) = "on" Then
    '            blnCHEK_DATARETTIFICAAVVISO = True
    '        Else
    '            blnCHEK_DATARETTIFICAAVVISO = False
    '        End If

    '        myHashTable.Add("CHEK_DATARETTIFICAAVVISO", blnCHEK_DATARETTIFICAAVVISO)

    '        If myUtility.CToStr(Request("DataRettificaAvviso")).CompareTo("DATA") = 0 Then
    '            myHashTable.Add("OPT_DATARETTIFICAAVVISO", "DATA")
    '        Else
    '            myHashTable.Add("OPT_DATARETTIFICAAVVISO", "NESSUNA")
    '        End If

    '        myHashTable.Add("DATARETTIFICAAVVISO_DAL", myUtility.CToStr(Request("txtDataRettificaAvvisoDal")))
    '        myHashTable.Add("DATARETTIFICAAVVISO_AL", myUtility.CToStr(Request("txtDataRettificaAvvisoAl")))

    '        '****************************************DATA RETTIFICA AVVISO*****************************************

    '        '****************************************DATA annullamento  AVVISO*****************************************
    '        Dim blnCHEK_DATAANNULLAMENTOAVVISO As Boolean
    '        If myUtility.CToStr(Request("chkDataAnnulamentoAvviso")) = "on" Then
    '            blnCHEK_DATAANNULLAMENTOAVVISO = True
    '        Else
    '            blnCHEK_DATAANNULLAMENTOAVVISO = False
    '        End If

    '        myHashTable.Add("CHEK_DATAANNULLAMENTOAVVISO", blnCHEK_DATAANNULLAMENTOAVVISO)

    '        If myUtility.CToStr(Request("DataAnnulamentoAvviso")).CompareTo("DATA") = 0 Then
    '            myHashTable.Add("OPT_DATAANNULLAMENTOAVVISO", "DATA")
    '        Else
    '            myHashTable.Add("OPT_DATAANNULLAMENTOAVVISO", "NESSUNA")
    '        End If

    '        myHashTable.Add("DATAANNULLAMENTOAVVISO_DAL", myUtility.CToStr(Request("txtDataAnnulamentoAvvisoDal")))
    '        myHashTable.Add("DATAANNULLAMENTOAVVISO_AL", myUtility.CToStr(Request("txtDataAnnulamentoAvvisoAl")))

    '        '****************************************DATA annullamento AVVISO*****************************************

    '        '****************************************DATA AUTOTUTELA  AVVISO*****************************************
    '        Dim blnCHEK_DATAAUTOTUTELAAVVISO As Boolean
    '        If myUtility.CToStr(Request("chkDataSopensioneAutotutela")) = "on" Then
    '            blnCHEK_DATAAUTOTUTELAAVVISO = True
    '        Else
    '            blnCHEK_DATAAUTOTUTELAAVVISO = False
    '        End If

    '        myHashTable.Add("CHEK_DATAAUTOTUTELAAVVISO", blnCHEK_DATAAUTOTUTELAAVVISO)

    '        If myUtility.CToStr(Request("DataSopensioneAutotutela")).CompareTo("DATA") = 0 Then
    '            myHashTable.Add("OPT_DATAAUTOTUTELAAVVISO", "DATA")
    '        Else
    '            myHashTable.Add("OPT_DATAAUTOTUTELAAVVISO", "NESSUNA")
    '        End If

    '        myHashTable.Add("DATAAUTOTUTELAAVVISO_DAL", myUtility.CToStr(Request("txtDataSopensioneAutotutelaDal")))
    '        myHashTable.Add("DATAAUTOTUTELAAVVISO_AL", myUtility.CToStr(Request("txtDataSopensioneAutotutelaAl")))

    '        '****************************************DATA AUTOTUTELA AVVISO*****************************************
    '        '****************************************DATA Irreperibilità*****************************************
    '        Dim blnCHEK_DATAIrreperibile As Boolean

    '        If myUtility.CToStr(Request("chkDataIrreperibile")) = "on" Then
    '            blnCHEK_DATAIrreperibile = True
    '        Else
    '            blnCHEK_DATAIrreperibile = False
    '        End If

    '        myHashTable.Add("CHEK_DATAIrreperibile", blnCHEK_DATAIrreperibile)

    '        If myUtility.CToStr(Request("DataIrreperibile")).CompareTo("DATA") = 0 Then
    '            myHashTable.Add("OPT_DATAIrreperibile", "DATA")
    '        Else
    '            myHashTable.Add("OPT_DATAIrreperibile", "NESSUNA")
    '        End If

    '        myHashTable.Add("DATAIrreperibile_DAL", myUtility.CToStr(Request("txtDataIrreperibileDal")))
    '        myHashTable.Add("DATAIrreperibile_AL", myUtility.CToStr(Request("txtDataIrreperibileAl")))
    '        '****************************************DATA Irreperibilità*****************************************
    '        '****************************************GESTIONE RICORSI***********************************************
    '        Dim blnCHEK_RICORSOPROVINCIALE As Boolean
    '        Dim blnCHEK_SOSPENSIONEPROVINCIALE As Boolean
    '        Dim blnCHEK_SENTENZAPROVINCIALE As Boolean

    '        If myUtility.CToStr(Request("chkDataRicorsoProvinciale")) = "on" Then
    '            blnCHEK_RICORSOPROVINCIALE = True
    '        Else
    '            blnCHEK_RICORSOPROVINCIALE = False
    '        End If

    '        myHashTable.Add("CHEK_RICORSOPROVINCIALE", blnCHEK_RICORSOPROVINCIALE)

    '        If myUtility.CToStr(Request("DataRicorsoProvinciale")).CompareTo("DATA") = 0 Then
    '            myHashTable.Add("OPT_RICORSOPROVINCIALE", "DATA")
    '        Else
    '            myHashTable.Add("OPT_RICORSOPROVINCIALE", "NESSUNA")
    '        End If

    '        myHashTable.Add("RICORSOPROVINCIALE_DAL", myUtility.CToStr(Request("txtDataRicorsoProvincialeDal")))
    '        myHashTable.Add("RICORSOPROVINCIALE_AL", myUtility.CToStr(Request("txtDataRicorsoProvincialeAl")))
    '        '*******************************************************************************************************

    '        If myUtility.CToStr(Request("chkDataSopensioneProvinciale")) = "on" Then
    '            blnCHEK_SOSPENSIONEPROVINCIALE = True
    '        Else
    '            blnCHEK_SOSPENSIONEPROVINCIALE = False
    '        End If

    '        myHashTable.Add("CHEK_SOSPENSIONEPROVINCIALE", blnCHEK_SOSPENSIONEPROVINCIALE)

    '        If myUtility.CToStr(Request("DataSopensioneProvinciale")).CompareTo("DATA") = 0 Then
    '            myHashTable.Add("OPT_SOSPENSIONEPROVINCIALE", "DATA")
    '        Else
    '            myHashTable.Add("OPT_SOSPENSIONEPROVINCIALE", "NESSUNA")
    '        End If

    '        myHashTable.Add("SOSPENSIONEPROVINCIALE_DAL", myUtility.CToStr(Request("txtDataSopensioneProvincialeDal")))
    '        myHashTable.Add("SOSPENSIONEPROVINCIALE_AL", myUtility.CToStr(Request("txtDataSopensioneProvincialeAl")))
    '        '*******************************************************************************************************
    '        If myUtility.CToStr(Request("chkDataSentenzaProvinciale")) = "on" Then
    '            blnCHEK_SENTENZAPROVINCIALE = True
    '        Else
    '            blnCHEK_SENTENZAPROVINCIALE = False
    '        End If

    '        myHashTable.Add("CHEK_SENTENZAPROVINCIALE", blnCHEK_SENTENZAPROVINCIALE)

    '        If myUtility.CToStr(Request("DataSentenzaProvinciale")).CompareTo("DATA") = 0 Then
    '            myHashTable.Add("OPT_SENTENZAPROVINCIALE", "DATA")
    '        Else
    '            myHashTable.Add("OPT_SENTENZAPROVINCIALE", "NESSUNA")
    '        End If

    '        myHashTable.Add("SENTENZAPROVINCIALE_DAL", myUtility.CToStr(Request("txtDataSentenzaProvincialeDal")))
    '        myHashTable.Add("SENTENZAPROVINCIALE_AL", myUtility.CToStr(Request("txtDataSentenzaProvincialeAl")))
    '        '*******************************************************************************************************

    '        Dim blnCHEK_RICORSOREGIONALE As Boolean
    '        Dim blnCHEK_SOSPENSIONEREGIONALE As Boolean
    '        Dim blnCHEK_SENTENZAREGIONALE As Boolean
    '        '*******************************************************************************************************
    '        If myUtility.CToStr(Request("chkDataRicorsoRegionale")) = "on" Then
    '            blnCHEK_RICORSOREGIONALE = True
    '        Else
    '            blnCHEK_RICORSOREGIONALE = False
    '        End If

    '        myHashTable.Add("CHEK_RICORSOREGIONALE", blnCHEK_RICORSOREGIONALE)

    '        If myUtility.CToStr(Request("DataRicorsoRegionale")).CompareTo("DATA") = 0 Then
    '            myHashTable.Add("OPT_RICORSOREGIONALE", "DATA")
    '        Else
    '            myHashTable.Add("OPT_RICORSOREGIONALE", "NESSUNA")
    '        End If

    '        myHashTable.Add("RICORSOREGIONALE_DAL", myUtility.CToStr(Request("txtDataRicorsoRegionaleDal")))
    '        myHashTable.Add("RICORSOREGIONALE_AL", myUtility.CToStr(Request("txtDataRicorsoRegionaleAl")))
    '        '*******************************************************************************************************
    '        If myUtility.CToStr(Request("chkDataSopensioneRegionale")) = "on" Then
    '            blnCHEK_SOSPENSIONEREGIONALE = True
    '        Else
    '            blnCHEK_SOSPENSIONEREGIONALE = False
    '        End If

    '        myHashTable.Add("CHEK_SOSPENSIONEREGIONALE", blnCHEK_SOSPENSIONEREGIONALE)

    '        If myUtility.CToStr(Request("DataSopensioneRegionale")).CompareTo("DATA") = 0 Then
    '            myHashTable.Add("OPT_SOSPENSIONEREGIONALE", "DATA")
    '        Else
    '            myHashTable.Add("OPT_SOSPENSIONEREGIONALE", "NESSUNA")
    '        End If

    '        myHashTable.Add("SOSPENSIONEREGIONALE_DAL", myUtility.CToStr(Request("txtDataSopensioneRegionaleDal")))
    '        myHashTable.Add("SOSPENSIONEREGIONALE_AL", myUtility.CToStr(Request("txtDataSopensioneRegionaleAl")))
    '        '*******************************************************************************************************
    '        If myUtility.CToStr(Request("chkDataSentenzaRegionale")) = "on" Then
    '            blnCHEK_SENTENZAREGIONALE = True
    '        Else
    '            blnCHEK_SENTENZAREGIONALE = False
    '        End If

    '        myHashTable.Add("CHEK_SENTENZAREGIONALE", blnCHEK_SENTENZAREGIONALE)

    '        If myUtility.CToStr(Request("DataSentenzaRegionale")).CompareTo("DATA") = 0 Then
    '            myHashTable.Add("OPT_SENTENZAREGIONALE", "DATA")
    '        Else
    '            myHashTable.Add("OPT_SENTENZAREGIONALE", "NESSUNA")
    '        End If

    '        myHashTable.Add("SENTENZAREGIONALE_DAL", myUtility.CToStr(Request("txtDataSentenzaRegionaleDal")))
    '        myHashTable.Add("SENTENZAREGIONALE_AL", myUtility.CToStr(Request("txtDataSentenzaRegionaleAl")))
    '        '*******************************************************************************************************
    '        Dim blnCHEK_RICORSOCASSAZIONE As Boolean
    '        Dim blnCHEK_SOSPENSIONECASSAZIONE As Boolean
    '        Dim blnCHEK_SENTENZACASSAZIONE As Boolean
    '        '*******************************************************************************************************
    '        If myUtility.CToStr(Request("chkDataRicorsoCassazione")) = "on" Then
    '            blnCHEK_RICORSOCASSAZIONE = True
    '        Else
    '            blnCHEK_RICORSOCASSAZIONE = False
    '        End If

    '        myHashTable.Add("CHEK_RICORSOCASSAZIONE", blnCHEK_RICORSOCASSAZIONE)

    '        If myUtility.CToStr(Request("DataRicorsoCassazione")).CompareTo("DATA") = 0 Then
    '            myHashTable.Add("OPT_RICORSOCASSAZIONE", "DATA")
    '        Else
    '            myHashTable.Add("OPT_RICORSOCASSAZIONE", "NESSUNA")
    '        End If

    '        myHashTable.Add("RICORSOCASSAZIONE_DAL", myUtility.CToStr(Request("txtDataRicorsoCassazioneDal")))
    '        myHashTable.Add("RICORSOCASSAZIONE_AL", myUtility.CToStr(Request("txtDataRicorsoCassazioneAl")))
    '        '*******************************************************************************************************
    '        If myUtility.CToStr(Request("chkDataSopensioneCassazione")) = "on" Then
    '            blnCHEK_SOSPENSIONECASSAZIONE = True
    '        Else
    '            blnCHEK_SOSPENSIONECASSAZIONE = False
    '        End If

    '        myHashTable.Add("CHEK_SOSPENSIONECASSAZIONE", blnCHEK_SOSPENSIONECASSAZIONE)

    '        If myUtility.CToStr(Request("DataSopensioneCassazione")).CompareTo("DATA") = 0 Then
    '            myHashTable.Add("OPT_SOSPENSIONECASSAZIONE", "DATA")
    '        Else
    '            myHashTable.Add("OPT_SOSPENSIONECASSAZIONE", "NESSUNA")
    '        End If

    '        myHashTable.Add("SOSPENSIONECASSAZIONE_DAL", myUtility.CToStr(Request("txtDataSopensioneCassazioneDal")))
    '        myHashTable.Add("SOSPENSIONECASSAZIONE_AL", myUtility.CToStr(Request("txtDataSopensioneCassazioneAl")))
    '        '*******************************************************************************************************
    '        If myUtility.CToStr(Request("chkDataSentenzaCassazione")) = "on" Then
    '            blnCHEK_SENTENZACASSAZIONE = True
    '        Else
    '            blnCHEK_SENTENZACASSAZIONE = False
    '        End If

    '        myHashTable.Add("CHEK_SENTENZACASSAZIONE", blnCHEK_SENTENZACASSAZIONE)

    '        If myUtility.CToStr(Request("DataSentenzaCassazione")).CompareTo("DATA") = 0 Then
    '            myHashTable.Add("OPT_SENTENZACASSAZIONE", "DATA")
    '        Else
    '            myHashTable.Add("OPT_SENTENZACASSAZIONE", "NESSUNA")
    '        End If

    '        myHashTable.Add("SENTENZACASSAZIONE_DAL", myUtility.CToStr(Request("txtDataSentenzaCassazioneDal")))
    '        myHashTable.Add("SENTENZACASSAZIONE_AL", myUtility.CToStr(Request("txtDataSentenzaCassazioneAl")))
    '        '*******************************************************************************************************
    '        '****************************************FINE GESTIONE RICORSI***********************************************

    '        Dim blnCHEK_ATTODEFINITIVO As Boolean
    '        '*******************************************************************************************************
    '        If myUtility.CToStr(Request("chkDataAttoDefinitivo")) = "on" Then
    '            blnCHEK_ATTODEFINITIVO = True
    '        Else
    '            blnCHEK_ATTODEFINITIVO = False
    '        End If

    '        myHashTable.Add("CHEK_ATTODEFINITIVO", blnCHEK_ATTODEFINITIVO)

    '        If myUtility.CToStr(Request("DataAttoDefinitivo")).CompareTo("DATA") = 0 Then
    '            myHashTable.Add("OPT_ATTODEFINITIVO", "DATA")
    '        Else
    '            myHashTable.Add("OPT_ATTODEFINITIVO", "NESSUNA")
    '        End If

    '        myHashTable.Add("ATTODEFINITIVO_DAL", myUtility.CToStr(Request("txtDataAttoDefinitivoDal")))
    '        myHashTable.Add("ATTODEFINITIVO_AL", myUtility.CToStr(Request("txtDataAttoDefinitivoAl")))
    '        '*******************************************************************************************************
    '        Dim blnCHEK_PAGAMENTO As Boolean
    '        '*******************************************************************************************************
    '        If myUtility.CToStr(Request("chkDataPagamento")) = "on" Then
    '            blnCHEK_PAGAMENTO = True
    '        Else
    '            blnCHEK_PAGAMENTO = False
    '        End If

    '        myHashTable.Add("CHEK_PAGAMENTO", blnCHEK_PAGAMENTO)

    '        If myUtility.CToStr(Request("DataPagamento")).CompareTo("DATA") = 0 Then
    '            myHashTable.Add("OPT_PAGAMENTO", "DATA")
    '        Else
    '            myHashTable.Add("OPT_PAGAMENTO", "NESSUNA")
    '        End If

    '        myHashTable.Add("PAGAMENTO_DAL", myUtility.CToStr(Request("txtDataPagamentoDal")))
    '        myHashTable.Add("PAGAMENTO_AL", myUtility.CToStr(Request("txtDataPagamentoAl")))

    '        '*******************************************************************************************************
    '        Dim blnCHEK_SOLLECITOBONARIO As Boolean
    '        '*******************************************************************************************************
    '        If myUtility.CToStr(Request("chkDataSollecitoBonario")) = "on" Then
    '            blnCHEK_SOLLECITOBONARIO = True
    '        Else
    '            blnCHEK_SOLLECITOBONARIO = False
    '        End If

    '        myHashTable.Add("CHEK_SOLLECITOBONARIO", blnCHEK_SOLLECITOBONARIO)

    '        If myUtility.CToStr(Request("DataSollecitoBonario")).CompareTo("DATA") = 0 Then
    '            myHashTable.Add("OPT_SOLLECITOBONARIO", "DATA")
    '        Else
    '            myHashTable.Add("OPT_SOLLECITOBONARIO", "NESSUNA")
    '        End If

    '        myHashTable.Add("SOLLECITOBONARIO_DAL", myUtility.CToStr(Request("txtDataSollecitoBonarioDal")))
    '        myHashTable.Add("SOLLECITOBONARIO_AL", myUtility.CToStr(Request("txtDataSollecitoBonarioAl")))
    '        '*******************************************************************************************************

    '        Dim blnCHEK_RUOLOORDINARIO As Boolean
    '        '*******************************************************************************************************
    '        If myUtility.CToStr(Request("chkDataRuoloOrdinario")) = "on" Then
    '            blnCHEK_RUOLOORDINARIO = True
    '        Else
    '            blnCHEK_RUOLOORDINARIO = False
    '        End If

    '        myHashTable.Add("CHEK_RUOLOORDINARIO", blnCHEK_RUOLOORDINARIO)

    '        If myUtility.CToStr(Request("DataRuoloOrdinario")).CompareTo("DATA") = 0 Then
    '            myHashTable.Add("OPT_RUOLOORDINARIO", "DATA")
    '        Else
    '            myHashTable.Add("OPT_RUOLOORDINARIO", "NESSUNA")
    '        End If

    '        myHashTable.Add("RUOLOORDINARIO_DAL", myUtility.CToStr(Request("txtDataRuoloOrdinarioDal")))
    '        myHashTable.Add("RUOLOORDINARIO_AL", myUtility.CToStr(Request("txtDataRuoloOrdinarioAl")))

    '        '*******************************************************************************************************

    '        Dim blnCHEK_COATTIVO As Boolean
    '        '*******************************************************************************************************

    '        If myUtility.CToStr(Request("chkDataCoattivo")) = "on" Then
    '            blnCHEK_COATTIVO = True
    '        Else
    '            blnCHEK_COATTIVO = False
    '        End If

    '        myHashTable.Add("CHEK_COATTIVO", blnCHEK_COATTIVO)

    '        If myUtility.CToStr(Request("DataCoattivo")).CompareTo("DATA") = 0 Then
    '            myHashTable.Add("OPT_COATTIVO", "DATA")
    '        Else
    '            myHashTable.Add("OPT_COATTIVO", "NESSUNA")
    '        End If

    '        myHashTable.Add("COATTIVO_DAL", myUtility.CToStr(Request("txtDataCoattivoDal")))
    '        myHashTable.Add("COATTIVO_AL", myUtility.CToStr(Request("txtDataCoattivoAl")))

    '        '*******************************************************************************************************
    '        Return myHashTable
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAttiRicercaAvanzata.ValDate.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '        Return HasTableOrg
    '    End Try
    'End Function
    'Private Function ValDate(HasTableOrg As Hashtable, mySearch As ObjSearchAtti) As Hashtable
    '    Dim myHashTable As New Hashtable
    '    Dim oReplace As New OPENgovTIa.generalClass.generalFunction
    '    Try
    '        myHashTable = HasTableOrg
    '        '************************************DATA ELABORAZIONE*******************************************
    '        myHashTable.Add("DATAELABORAZIONE_DAL", oReplace.FormattaData(mySearch.DataGenerazioneDal, "A"))
    '        myHashTable.Add("DATAELABORAZIONE_AL", oReplace.FormattaData(mySearch.DataGenerazioneAL, "A"))
    '        '************************************FINE DATA ELABORAZIONE*******************************************
    '        '****************************************DATA CONFERMA AVVISO*****************************************
    '        If Not mySearch.DataConfermaAvviso.Dal Is Nothing Then
    '            myHashTable.Add("DATACONFERMAAVVISO_DAL", oReplace.FormattaData(mySearch.DataConfermaAvvisoDal, "A"))
    '            myHashTable.Add("DATACONFERMAAVVISO_AL", oReplace.FormattaData(mySearch.DataConfermaAvvisoAl, "A"))
    '        Else
    '            myHashTable.Add("DATACONFERMAAVVISO_DAL", Nothing)
    '            myHashTable.Add("DATACONFERMAAVVISO_AL", Nothing)
    '        End If
    '        '****************************************DATA CONFERMA AVVISO*****************************************
    '        '****************************************DATA STAMPA  AVVISO*****************************************
    '        myHashTable.Add("DATASTAMPAAVVISO_DAL", oReplace.FormattaData(mySearch.DataStampaAvvisoDal, "A"))
    '        myHashTable.Add("DATASTAMPAAVVISO_AL", oReplace.FormattaData(mySearch.DataStampaAvvisoAl, "A"))
    '        '****************************************DATA STAMPA AVVISO*****************************************
    '        '****************************************DATA CONSEGNA  AVVISO*****************************************
    '        myHashTable.Add("DATACONSEGNAAVVISO_DAL", oReplace.FormattaData(mySearch.DataConsegnaAvvisoDal, "A"))
    '        myHashTable.Add("DATACONSEGNAAVVISO_AL", oReplace.FormattaData(mySearch.DataConsegnaAvvisoAl, "A"))
    '        '****************************************DATA CONSEGNA AVVISO*****************************************
    '        '****************************************DATA NOTIFICA  AVVISO*****************************************
    '        myHashTable.Add("DATANOTIFICAAVVISO_DAL", oReplace.FormattaData(mySearch.DataNotificaAvvisoDal, "A"))
    '        myHashTable.Add("DATANOTIFICAAVVISO_AL", oReplace.FormattaData(mySearch.DataNotificaAvvisoAl, "A"))
    '        '****************************************DATA NOTIFICA AVVISO*****************************************
    '        '****************************************DATA RETTIFICA  AVVISO*****************************************
    '        myHashTable.Add("DATARETTIFICAAVVISO_DAL", oReplace.FormattaData(mySearch.DataRettificaAvvisoDal, "A"))
    '        myHashTable.Add("DATARETTIFICAAVVISO_AL", oReplace.FormattaData(mySearch.DataRettificaAvvisoAl, "A"))
    '        '****************************************DATA RETTIFICA AVVISO*****************************************
    '        '****************************************DATA annullamento  AVVISO*****************************************
    '        myHashTable.Add("DATAANNULLAMENTOAVVISO_DAL", oReplace.FormattaData(mySearch.DataAnnulamentoAvvisoDal, "A"))
    '        myHashTable.Add("DATAANNULLAMENTOAVVISO_AL", oReplace.FormattaData(mySearch.DataAnnulamentoAvvisoAl, "A"))
    '        '****************************************DATA annullamento AVVISO*****************************************
    '        '****************************************DATA AUTOTUTELA  AVVISO*****************************************
    '        myHashTable.Add("DATAAUTOTUTELAAVVISO_DAL", oReplace.FormattaData(mySearch.DataSopensioneAutotutelaDal, "A"))
    '        myHashTable.Add("DATAAUTOTUTELAAVVISO_AL", oReplace.FormattaData(mySearch.DataSopensioneAutotutelaAl, "A"))
    '        '****************************************DATA AUTOTUTELA AVVISO*****************************************
    '        '****************************************DATA Irreperibilità*****************************************
    '        myHashTable.Add("DATAIrreperibile_DAL", oReplace.FormattaData(mySearch.DataIrreperibileDal, "A"))
    '        myHashTable.Add("DATAIrreperibile_AL", oReplace.FormattaData(mySearch.DataIrreperibileAl, "A"))
    '        '****************************************DATA Irreperibilità*****************************************
    '        '****************************************GESTIONE RICORSI***********************************************
    '        myHashTable.Add("RICORSOPROVINCIALE_DAL", oReplace.FormattaData(mySearch.DataRicorsoProvincialeDal, "A"))
    '        myHashTable.Add("RICORSOPROVINCIALE_AL", oReplace.FormattaData(mySearch.DataRicorsoProvincialeAl, "A"))
    '        myHashTable.Add("SOSPENSIONEPROVINCIALE_DAL", oReplace.FormattaData(mySearch.DataSopensioneProvincialeDal, "A"))
    '        myHashTable.Add("SOSPENSIONEPROVINCIALE_AL", oReplace.FormattaData(mySearch.DataSopensioneProvincialeAl, "A"))
    '        myHashTable.Add("SENTENZAPROVINCIALE_DAL", oReplace.FormattaData(mySearch.DataSentenzaProvincialeDal, "A"))
    '        myHashTable.Add("SENTENZAPROVINCIALE_AL", oReplace.FormattaData(mySearch.DataSentenzaProvincialeAl, "A"))
    '        '***
    '        myHashTable.Add("RICORSOREGIONALE_DAL", oReplace.FormattaData(mySearch.DataRicorsoRegionaleDal, "A"))
    '        myHashTable.Add("RICORSOREGIONALE_AL", oReplace.FormattaData(mySearch.DataRicorsoRegionaleAl, "A"))
    '        myHashTable.Add("SOSPENSIONEREGIONALE_DAL", oReplace.FormattaData(mySearch.DataSopensioneRegionaleDal, "A"))
    '        myHashTable.Add("SOSPENSIONEREGIONALE_AL", oReplace.FormattaData(mySearch.DataSopensioneRegionaleAl, "A"))
    '        myHashTable.Add("SENTENZAREGIONALE_DAL", oReplace.FormattaData(mySearch.DataSentenzaRegionaleDal, "A"))
    '        myHashTable.Add("SENTENZAREGIONALE_AL", oReplace.FormattaData(mySearch.DataSentenzaRegionaleAl, "A"))
    '        '***
    '        myHashTable.Add("RICORSOCASSAZIONE_DAL", oReplace.FormattaData(mySearch.DataRicorsoCassazioneDal, "A"))
    '        myHashTable.Add("RICORSOCASSAZIONE_AL", oReplace.FormattaData(mySearch.DataRicorsoCassazioneAl, "A"))
    '        myHashTable.Add("SOSPENSIONECASSAZIONE_DAL", oReplace.FormattaData(mySearch.DataSopensioneCassazioneDal, "A"))
    '        myHashTable.Add("SOSPENSIONECASSAZIONE_AL", oReplace.FormattaData(mySearch.DataSopensioneCassazioneAl, "A"))
    '        myHashTable.Add("SENTENZACASSAZIONE_DAL", oReplace.FormattaData(mySearch.DataSentenzaCassazioneDal, "A"))
    '        myHashTable.Add("SENTENZACASSAZIONE_AL", oReplace.FormattaData(mySearch.DataSentenzaCassazioneAl, "A"))
    '        '****************************************FINE GESTIONE RICORSI***********************************************
    '        '****************************************DATA ATTO DEFINITIVO*******************************************
    '        myHashTable.Add("ATTODEFINITIVO_DAL", oReplace.FormattaData(mySearch.DataAttoDefinitivoDal, "A"))
    '        myHashTable.Add("ATTODEFINITIVO_AL", oReplace.FormattaData(mySearch.DataAttoDefinitivoAl, "A"))
    '        '****************************************DATA ATTO DEFINITIVO*******************************************
    '        '****************************************DATA PAGAMENTO*************************************************
    '        myHashTable.Add("PAGAMENTO_DAL", oReplace.FormattaData(mySearch.DataPagamentoDal, "A"))
    '        myHashTable.Add("PAGAMENTO_AL", oReplace.FormattaData(mySearch.DataPagamentoAl, "A"))
    '        '****************************************DATA PAGAMENTO*************************************************
    '        '****************************************DATA SOLLECITO BONARIO*****************************************
    '        myHashTable.Add("SOLLECITOBONARIO_DAL", oReplace.FormattaData(mySearch.DataSollecitoBonarioDal, "A"))
    '        myHashTable.Add("SOLLECITOBONARIO_AL", oReplace.FormattaData(mySearch.DataSollecitoBonarioAl, "A"))
    '        '****************************************DATA SOLLECITO BONARIO*****************************************
    '        '****************************************DATA RUOLO ORDINARIO*******************************************
    '        myHashTable.Add("RUOLOORDINARIO_DAL", oReplace.FormattaData(mySearch.DataRuoloOrdinarioDal, "A"))
    '        myHashTable.Add("RUOLOORDINARIO_AL", oReplace.FormattaData(mySearch.DataRuoloOrdinarioAl, "A"))
    '        '****************************************DATA RUOLO ORDINARIO*******************************************
    '        '****************************************DATA RUOLO COATTIVO********************************************
    '        myHashTable.Add("COATTIVO_DAL", oReplace.FormattaData(mySearch.DataCoattivoDal, "A"))
    '        myHashTable.Add("COATTIVO_AL", oReplace.FormattaData(mySearch.DataCoattivoAl, "A"))
    '        '****************************************DATA RUOLO COATTIVO********************************************
    '        Return myHashTable
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAttiRicercaAvanzata.ValDate.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '        Return HasTableOrg
    '    End Try
    'End Function
End Class
