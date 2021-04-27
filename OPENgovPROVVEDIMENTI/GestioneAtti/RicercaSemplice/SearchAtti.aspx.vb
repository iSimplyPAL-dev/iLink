Imports log4net
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
''' <summary>
''' Pagina per la visualizzazione dei provvedimenti.
''' Contiene la griglia per la visualizzazione del risultato e iframe per la consultazione puntuale del soggetto. 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class SearchAtti
    Inherits BasePage
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(SearchAtti))
   
    Dim objDS As DataSet
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
    Protected intbookmarkIndex As Integer = 0
    Protected intCurrentPageIndex As Integer = 0
    Protected intitemCount As Integer = 0
    Protected blnbookMark As Boolean = True
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
        Dim dt As DataView
        Dim intTOTALEAVVISI As Integer = 0
        Dim sScript As String = ""
        Dim objUtility As New MyUtility

        Try
            Log.Debug("SearchAtti_PageLoad::inizio")
            '*****************************GESTIONE DEL PARAMETRO PROGRESSIVO OPERAZIONE*************
            If Len(objUtility.CToStr(Request("PROGRESSIVO_OPERAZIONE"))) > 0 Then
                blnSearchProvvedimento = True
                objHashTable.Add("PROGRESSIVO_ELABORAZIONE", objUtility.CToStr(Request("PROGRESSIVO_OPERAZIONE")))
            End If

            '*****************************GESTIONE DEL PARAMETRO PROGRESSIVO OPERAZIONE*************
            If Not blnSearchProvvedimento Then
                objHashTable.Add("COGNOME", CType(Request("COGNOME"), String))
                objHashTable.Add("NOME", CType(Request("NOME"), String))
                objHashTable.Add("CODICEFISCALE", CType(Request("CODICEFISCALE"), String))
                objHashTable.Add("PARTITAIVA", CType(Request("PARTITAIVA"), String))
                strNUMEROPROVVEDIMENTO = CType(Request("NUMEROPROVVEDIMENTO"), String)
                objHashTable.Add("NUMEROPROVVEDIMENTO", strNUMEROPROVVEDIMENTO)

                objHashTable.Add("CODTRIBUTO", Request("CODTRIBUTO"))
                objHashTable.Add("PARAMETROORDINAMENTOGRIGLIA", "")
            End If
            objHashTable.Add("CodENTE", ConstSession.IdEnte)
            objHashTable.Add("CODENTE", ConstSession.IdEnte)

            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
            objHashTable.Add("CONNECTIONSTRINGANAGRAFICA", ConstSession.StringConnectionAnagrafica)
            If Page.IsPostBack = False Then
                Session.Add(SEARCH_PARAMETRES_RICERCA_SEMPLICE, GetSearchParametres)
                If Len(objUtility.CToStr(Session("CODICE_CONTRIBUENTE_RICERCA_SEMPLICE"))) > 0 Then
                    Log.Debug("SearchAtti_PageLoad::bind a griglia")
                    objDS = Session("SELEZIONE_PROVVEDIMENTI_SEMPLICE")
                    If Not objDS Is Nothing Then
                        ViewState("SortKey") = "NOMINATIVO"
                        ViewState("OrderBy") = "ASC"
                        dt = objDS.Tables(0).DefaultView
                        dt.Sort = ViewState("SortKey") & " " & ViewState("OrderBy")
                    End If
                    GrdAtti.DataSource = dt
                    GrdAtti.DataBind()

                    CalPageaspx(objUtility.CToStr(Session("CODICE_CONTRIBUENTE_RICERCA_SEMPLICE")))
                    GrdAtti.SelectedIndex = CType(Session("intbookmarkIndex"), Integer)

                    Session.Remove("CODICE_CONTRIBUENTE_RICERCA_SEMPLICE")
                Else
                    Log.Debug("SearchAtti_PageLoad::url servizio" & ConfigurationManager.AppSettings("URLServiziElaborazioneAtti"))
                    Dim objCOMRicerca As IElaborazioneAtti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAtti), ConstSession.URLServiziElaborazioneAtti)
                    If Not blnSearchProvvedimento Then
                        Dim FncRic As New ClsGestioneAccertamenti
                        objDS = FncRic.getATTIRicercaSemplice(objHashTable) 'objCOMRicerca.GetDatiAttiRicercaSemplice(objHashTable)
                    Else
                        objDS = objCOMRicerca.getDatiProvvedimento_PerTipo(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable)
                    End If

                    Session.Remove("SELEZIONE_PROVVEDIMENTI_SEMPLICE")
                    Session("SELEZIONE_PROVVEDIMENTI_SEMPLICE") = objDS
                    Session("HashTableAttiRicercaSemplice") = objHashTable

                    If Not objDS Is Nothing Then
                        ViewState("SortKey") = "NOMINATIVO"
                        ViewState("OrderBy") = "ASC"
                        dt = objDS.Tables(0).DefaultView
                        dt.Sort = ViewState("SortKey") & " " & ViewState("OrderBy")
                    End If

                    GrdAtti.DataSource = dt
                    Log.Debug("SearchAtti_PageLoad::bind a griglia")
                    GrdAtti.DataBind()
                End If
                Select Case CInt(GrdAtti.Rows.Count)
                    Case 0
                        intTOTALEAVVISI = 0
                        GrdAtti.Visible = False

                        lblMessage.Text = "Non sono state trovate Anagrafiche per la ricerca effettuata"
                    Case Is > 0
                        If Not blnSearchProvvedimento Then
                            sScript += "parent.document.getElementById('txtRicercaAttiva').value='1';"
                            RegisterScript(sScript, Me.GetType())
                        End If
                        intTOTALEAVVISI = CInt(GrdAtti.Rows.Count)
                        txtTotaleAvvisi.Text = "&nbsp;&nbsp;&nbsp;&nbsp;" & FormatNumber(intTOTALEAVVISI, 0)

                        sScript += "Riepilogativi.style.display='';"
                        RegisterScript(sScript, Me.GetType())
                End Select
                If Not blnSearchProvvedimento Then
                    sScript += "parent.attesaGestioneAtti.style.display='none';"
                    RegisterScript(sScript, Me.GetType())
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAtti.Page_Load.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    Dim dt As DataView
    '    Dim strCodContribuente As String = ""
    '    Dim intTOTALEAVVISI As Integer = 0
    '    Dim intTOTALEIMPORTO As Integer = 0
    '    Dim strTOTALEIMPORTORETTIFICHE As String = "0"
    '    Dim strTOTALE_GENERALE_PROVVEDIMENTI As String = "0"
    '    dim sScript as string=""
    '    Dim objUtility As New MyUtility

    '    Try
    '        log.debug("SearchAtti_PageLoad::inizio")
    '        '*****************************GESTIONE DEL PARAMETRO PROGRESSIVO OPERAZIONE*************
    '        If Len(objUtility.CToStr(Request("PROGRESSIVO_OPERAZIONE"))) > 0 Then
    '            blnSearchProvvedimento = True
    '            objHashTable.Add("PROGRESSIVO_ELABORAZIONE", objUtility.CToStr(Request("PROGRESSIVO_OPERAZIONE")))
    '        End If

    '        '*****************************GESTIONE DEL PARAMETRO PROGRESSIVO OPERAZIONE*************
    '        If Not blnSearchProvvedimento Then
    '            'Session("ComplitDataLettere") = Nothing
    '            'Session("HashTableAttiRicercaSemplice") = Nothing
    '            objHashTable.Add("COGNOME", CType(Request("COGNOME"), String))
    '            objHashTable.Add("NOME", CType(Request("NOME"), String))
    '            objHashTable.Add("CODICEFISCALE", CType(Request("CODICEFISCALE"), String))
    '            objHashTable.Add("PARTITAIVA", CType(Request("PARTITAIVA"), String))
    '            strNUMEROPROVVEDIMENTO = CType(Request("NUMEROPROVVEDIMENTO"), String)
    '            objHashTable.Add("NUMEROPROVVEDIMENTO", strNUMEROPROVVEDIMENTO)

    '            'objHashTable.Add("CODTRIBUTO", Session("COD_TRIBUTO"))
    '            objHashTable.Add("CODTRIBUTO", Request("CODTRIBUTO"))
    '            objHashTable.Add("PARAMETROORDINAMENTOGRIGLIA", "")
    '        End If
    '        objHashTable.Add("CodENTE", ConstSession.IdEnte)
    '        objHashTable.Add("CODENTE", ConstSession.IdEnte)
    '        objHashTable.Add("IDSOTTOAPPLICAZIONEANAGRAFICA", ConfigurationManager.AppSettings("OPENGOVA"))
    '        objHashTable.Add("IDSOTTOAPPLICAZIONETERRITORIO", ConfigurationManager.AppSettings("OPENGOVT"))
    '        objHashTable.Add("IDSOTTOAPPLICAZIONEUTILITA", ConfigurationManager.AppSettings("OPENGOVU"))
    '        objHashTable.Add("IDSOTTOAPPLICAZIONEICI", ConfigurationManager.AppSettings("OPENGOVI"))

    '        'Dim strWFErrore As String
    '        'objSessione = New CreateSessione(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))
    '        'If Not objSessione.CreaSessione(ConstSession.UserName, strWFErrore) Then
    '        '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        'End If
    '        'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString 'objSessione.oSession.oAppDB.GetConnection.ConnectionString
    '        objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
    '        objHashTable.Add("CONNECTIONSTRINGANAGRAFICA", ConstSession.StringConnectionAnagrafica)
    '        If Page.IsPostBack = False Then
    '            Session.Add(SEARCH_PARAMETRES_RICERCA_SEMPLICE, GetSearchParametres)
    '            If Len(objUtility.CToStr(Session("CODICE_CONTRIBUENTE_RICERCA_SEMPLICE"))) > 0 Then
    '                Log.Debug("SearchAtti_PageLoad::bind a griglia")
    '                objDS = Session("SELEZIONE_PROVVEDIMENTI_SEMPLICE")
    '                If Not objDS Is Nothing Then
    '                    ViewState("SortKey") = "NOMINATIVO"
    '                ViewState("OrderBy") = "ASC"
    '                dt = objDS.Tables(0).DefaultView
    '                dt.Sort = ViewState("SortKey") & " " & ViewState("OrderBy")
    '            End If
    '            GrdAtti.DataSource = dt
    '                GrdAtti.DataBind()

    '                CalPageaspx(objUtility.CToStr(Session("CODICE_CONTRIBUENTE_RICERCA_SEMPLICE")))
    '                Log.Debug("SearchAtti_PageLoad::devo fare il GrdAtti_PageIndexChanged")
    '                'GrdAtti_PageIndexChanged(Nothing, Nothing)
    '                Log.Debug("SearchAtti_PageLoad::GrdAtti_PageIndexChanged fatto")
    '                GrdAtti.SelectedIndex = CType(Session("intbookmarkIndex"), Integer)

    '                Session.Remove("CODICE_CONTRIBUENTE_RICERCA_SEMPLICE")
    '                'Session.Remove("intCurrentPageIndex")
    '                'Session.Remove("intbookmarkIndex")
    '            Else
    '                Log.Debug("SearchAtti_PageLoad::url servizio" & ConfigurationManager.AppSettings("URLServiziElaborazioneAtti"))
    '                Dim objCOMRicerca As IElaborazioneAtti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAtti), ConstSession.URLServiziElaborazioneAtti)
    '                If Not blnSearchProvvedimento Then
    '                    Dim FncRic As New ClsGestioneAccertamenti
    '                    objDS = FncRic.getATTIRicercaSemplice(objHashTable) 'objCOMRicerca.GetDatiAttiRicercaSemplice(objHashTable)
    '                Else
    '                    objDS = objCOMRicerca.getDatiProvvedimento_PerTipo(objHashTable)
    '                End If

    '                Session.Remove("SELEZIONE_PROVVEDIMENTI_SEMPLICE")
    '                Session("SELEZIONE_PROVVEDIMENTI_SEMPLICE") = objDS
    '                Session("HashTableAttiRicercaSemplice") = objHashTable

    '                If Not objDS Is Nothing Then
    '                    ViewState("SortKey") = "NOMINATIVO"
    '                    ViewState("OrderBy") = "ASC"
    '                    dt = objDS.Tables(0).DefaultView
    '                    dt.Sort = ViewState("SortKey") & " " & ViewState("OrderBy")
    '                End If

    '                GrdAtti.DataSource = dt
    '                Log.Debug("SearchAtti_PageLoad::bind a griglia")
    '                GrdAtti.DataBind()
    '            End If
    '            Select Case CInt(GrdAtti.Rows.Count)
    '                Case 0
    '                    intTOTALEAVVISI = 0
    '                    GrdAtti.Visible = False

    '                    lblMessage.Text = "Non sono state trovate Anagrafiche per la ricerca effettuata"
    '                Case Is > 0
    '                    If Not blnSearchProvvedimento Then
    '                        sScript += "parent.formRicercaAnagrafica.txtRicercaAttiva.value='1';"
    '                        RegisterScript(sScript, Me.GetType())
    '                    End If
    '                    intTOTALEAVVISI = CInt(GrdAtti.Rows.Count)
    '                    txtTotaleAvvisi.Text = "&nbsp;&nbsp;&nbsp;&nbsp;" & FormatNumber(intTOTALEAVVISI, 0)

    '                    sScript += "Riepilogativi.style.display='';"
    '                    RegisterScript(sScript, Me.GetType())

    '                    'strTOTALEIMPORTORETTIFICHE = objUtility.CToStr(objDS.Tables("TP_ATTI_RICERCA_SEMPLICE_TOTALE_RETTIFICATO").Rows(0).Item("IMPORTO_TOTALE"))
    '                    'If Len(strTOTALEIMPORTORETTIFICHE) > 0 Then
    '                    '  txtTotaleRettifiche.Text = FormatNumber(strTOTALEIMPORTORETTIFICHE, 2)
    '                    'Else
    '                    '  txtTotaleRettifiche.Text = "0"
    '                    'End If

    '                    'strTOTALE_GENERALE_PROVVEDIMENTI = objUtility.CToStr(objDS.Tables("TP_ATTI_RICERCA_SEMPLICE_IMPORTO_TOTALE").Rows(0).Item("IMPORTO_TOTALE"))
    '                    'If Len(strTOTALE_GENERALE_PROVVEDIMENTI) > 0 Then
    '                    '  txtImportoTotaleAvvisi.Text = FormatNumber(strTOTALE_GENERALE_PROVVEDIMENTI, 2)
    '                    'Else
    '                    '  txtImportoTotaleAvvisi.Text = "0"
    '                    'End If
    '            End Select
    '            If Not blnSearchProvvedimento Then
    '                sScript += "parent.attesaGestioneAtti.style.display='none';"
    '                RegisterScript(sScript , Me.GetType())
    '            End If
    '        End If
    '        'If Page.IsPostBack = False Then
    '        '  If Len(objUtility.CToStr(Session("CODICE_CONTRIBUENTE_RICERCA_SEMPLICE"))) > 0 Then
    '        '    CalPageaspx(objUtility.CToStr(Session("CODICE_CONTRIBUENTE_RICERCA_SEMPLICE")))

    '        '    GrdAtti.start_index = CType(Session("intCurrentPageIndex"), Integer)
    '        '    GrdAtti.CurrentPageIndex = CType(Session("intCurrentPageIndex"), Integer)
    '        '    GrdAtti.SelectedIndex = CType(Session("intbookmarkIndex"), Integer)
    '        '    GrdAtti.DataSource = dt
    '        '    GrdAtti.DataBind()

    '        '    Session.Remove("CODICE_CONTRIBUENTE_RICERCA_SEMPLICE")
    '        '    Session.Remove("intCurrentPageIndex")
    '        '    Session.Remove("intbookmarkIndex")
    '        '  End If
    '        'End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAtti.Page_Load.errore: ", ex)
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
                CalPageaspx(IDRow)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAtti.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Gestione del cambio pagina della griglia.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdPageIndexChanging(ByVal sender As Object, e As GridViewPageEventArgs)
        Dim objUtility As New MyUtility
        Try
            LoadSearch(e.NewPageIndex)

            txtNominativo.Text = ""
            txtCFPI.Text = ""
            txtSesso.Text = ""
            txtDataNascita.Text = ""
            txtIndirizzo.Text = ""
            txtComune.Text = ""
            txtProvincia.Text = ""
            txtCap.Text = ""
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAtti.GrdPageIndexChanging.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
            Log.Debug("SearchAtti::GrdAtti_PageIndexChanged:: si è verificato il seguente errore::" & ex.Message & "::CODICE_CONTRIBUENTE_RICERCA_SEMPLICE::" & objUtility.CToStr(Session("CODICE_CONTRIBUENTE_RICERCA_SEMPLICE")) & "::intCurrentPageIndex::" & CType(Session("intCurrentPageIndex"), Integer))
        End Try
    End Sub
    'Public Sub RibesDataGrid_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GrdAtti.SelectedIndexChanged

    '    Dim strCOD_CONTRIBUENTE As String
    '    strCOD_CONTRIBUENTE = GrdAtti.SelectedItem.Cells(3).Text()

    '    CalPageaspx(strCOD_CONTRIBUENTE)

    'End Sub
    'Private Sub GrdAtti_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GrdAtti.ItemCommand
    'Try
    '    If e.CommandName = "Select" Then
    '        If blnbookMark Then
    '            intbookmarkIndex = e.Item.ItemIndex
    '            intCurrentPageIndex = GrdAtti.CurrentPageIndex
    '            Session("intbookmarkIndex") = intbookmarkIndex
    '            Session("intCurrentPageIndex") = intCurrentPageIndex
    '        End If
    '    End If
    ' Catch ex As Exception
    '     Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAtti.GrdAtti_ItemCommand.errore: ", ex)
    '   Response.Redirect("../../../PaginaErrore.aspx")
    'End Try
    'End Sub

    'Private Sub GrdAtti_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles GrdAtti.PageIndexChanged
    '    Dim objUtility As New MyUtility
    '    Dim dt As DataView
    '    Try
    '        If Len(objUtility.CToStr(Session("CODICE_CONTRIBUENTE_RICERCA_SEMPLICE"))) > 0 Then
    '            Log.Debug("SearchAtti::GrdAtti_PageIndexChanged::ho già una ricerca in memoria")
    '            GrdAtti.CurrentPageIndex = CType(Session("intCurrentPageIndex"), Integer)
    '            Log.Debug("SearchAtti::GrdAtti_PageIndexChanged::cambiato current page")
    '            objDS = CType(Session("SELEZIONE_PROVVEDIMENTI_SEMPLICE"), DataSet)
    '            GrdAtti.start_index = GrdAtti.CurrentPageIndex
    '            Log.Debug("SearchAtti::GrdAtti_PageIndexChanged::cambiato start index")
    '            GrdAtti.AllowCustomPaging = False
    '            ViewState("SortKey") = "NOMINATIVO"
    '            ViewState("OrderBy") = "ASC"
    '            dt = objDS.Tables(0).DefaultView
    '            dt.Sort = ViewState("SortKey") & " " & ViewState("OrderBy")
    '            GrdAtti.DataSource = dt
    '            GrdAtti.DataBind()
    '            Log.Debug("SearchAtti::GrdAtti_PageIndexChanged::fatto il bind alla griglia")
    '        Else
    '            txtNominativo.Text = ""
    '            txtCFPI.Text = ""
    '            txtSesso.Text = ""
    '            txtDataNascita.Text = ""
    '            txtIndirizzo.Text = ""
    '            txtComune.Text = ""
    '            txtProvincia.Text = ""
    '            txtCap.Text = ""
    '        End If
    '    Catch ex As Exception
    '     Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAtti.GrdAtti_PageIndexChanged.errore: ", ex)
    '   Response.Redirect("../../../PaginaErrore.aspx")
    '        Log.Debug("SearchAtti::GrdAtti_PageIndexChanged:: si è verificato il seguente errore::" & ex.Message & "::CODICE_CONTRIBUENTE_RICERCA_SEMPLICE::" & objUtility.CToStr(Session("CODICE_CONTRIBUENTE_RICERCA_SEMPLICE")) & "::intCurrentPageIndex::" & CType(Session("intCurrentPageIndex"), Integer) & "::GrdAtti.CurrentPageIndex::" & GrdAtti.CurrentPageIndex)
    '    End Try
    'End Sub

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

    '    GrdAtti.start_index = 0
    '    GrdAtti.DataSource = dt
    '    GrdAtti.DataBind()
    '    Catch ex As Exception
    '     Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAtti.GrdAtti_SortCommand.errore: ", ex)
    '   Response.Redirect("../../../PaginaErrore.aspx")
    '        Log.Debug("SearchAtti::GrdAtti_PageIndexChanged:: si è verificato il seguente errore::" & ex.Message & "::CODICE_CONTRIBUENTE_RICERCA_SEMPLICE::" & objUtility.CToStr(Session("CODICE_CONTRIBUENTE_RICERCA_SEMPLICE")) & "::intCurrentPageIndex::" & CType(Session("intCurrentPageIndex"), Integer) & "::GrdAtti.CurrentPageIndex::" & GrdAtti.CurrentPageIndex)
    '    End Try
    'End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdAtti.DataSource = CType(Session("SELEZIONE_PROVVEDIMENTI_SEMPLICE"), DataSet)
            If page.HasValue Then
                GrdAtti.PageIndex = page.Value
            End If
            GrdAtti.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAtti.LoadSearch.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strCOD_CONTRIBUENTE"></param>
    Protected Sub CalPageaspx(ByVal strCOD_CONTRIBUENTE As String)
        Try
            Log.Debug("SearchAtti_PageLoad::url servizio::" & ConfigurationManager.AppSettings("URLServiziElaborazioneAtti"))

            objHashTable.Add("CodContribuente", strCOD_CONTRIBUENTE)
            objHashTable.Add("Manuale", True)
            objHashTable.Add("DA", "")
            objHashTable.Add("A", "")

            Dim myAnag As New AnagInterface.DettaglioAnagrafica
            Dim FncAnag As New Anagrafica.DLL.GestioneAnagrafica
            myAnag = FncAnag.GetAnagrafica(CLng(strCOD_CONTRIBUENTE), -1, "", ConstSession.DBType, ConstSession.StringConnectionAnagrafica, False)
            txtNominativo.Text = myAnag.Cognome & " " & myAnag.Nome
            If myAnag.PartitaIva = "" Then
                txtCFPI.Text = myAnag.CodiceFiscale
            Else
                txtCFPI.Text = myAnag.PartitaIva
            End If
            txtSesso.Text = myAnag.Sesso
            txtDataNascita.Text = myAnag.DataNascita
            txtIndirizzo.Text = myAnag.ViaResidenza & " " & myAnag.CivicoResidenza
            txtComune.Text = myAnag.ComuneResidenza
            txtProvincia.Text = myAnag.ProvinciaResidenza
            txtCap.Text = myAnag.CapResidenza

            Dim sScript as string=""
            sScript += "document.getElementById('loadGrid').src='LoadAtti.aspx?COD_CONTRIBUENTE=" & strCOD_CONTRIBUENTE & "';"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAtti.CallPageaspx.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Private Function GetSearchParametres() As Hashtable

        Dim htSearchParametres As Hashtable = New Hashtable
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

End Class
