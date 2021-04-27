Imports log4net
Imports Anagrafica
Imports System.Data
Imports System.Data.SqlClient
''' <summary>
''' Pagina per la visualizzazione del risultato della ricerca doppioni.
''' Contiene la griglia dalla quale è possibile accedere al dettaglio e sulla quale operare per eseguire la pulizia.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
''' <history>
''' 	[antonello]	25/01/2005	Created
''' </history>
Partial Class SearchResultsAnagraficaDoppia
    Inherits ANAGRAFICAWEB.BasePage 'System.Web.UI.Page
    Private Log As ILog = LogManager.GetLogger(GetType(SearchResultsAnagraficaDoppia))
    Protected FncGrd As New ANAGRAFICAWEB.FunctionGrd
    Private intTipoRicerca As Integer
    Private sNominativo As String
    Private dblPerc As Double
    Private HasVuoti As Boolean = False
    Private oAnagrafica As New DLL.GestioneAnagrafica()
    Private dsAnagDoppie As New DataSet

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
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[antonello]	24/01/2005	Created
    ''' </history>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

            intTipoRicerca = Request.Item("TIPO_RICERCA")
            dblPerc = CDbl(Request.Item("PERCENTUALE"))
            sNominativo = Request.Item("Nominativo")
            If Not Request.Item("HasVuoti") Is Nothing Then
                HasVuoti = Boolean.Parse(Request.Item("HasVuoti"))
            End If
            If Page.IsPostBack = False Then
                '***20090728 è stata cambiata la query in modo da aprire un'unica connessione al db***
                dsAnagDoppie = oAnagrafica.GetAnagraficaAnagraficheDoppie(intTipoRicerca, dblPerc, sNominativo, HasVuoti, ANAGRAFICAWEB.ConstSession.IdEnte, ANAGRAFICAWEB.ConstSession.DBType, ANAGRAFICAWEB.ConstSession.StringConnectionAnagrafica)
                '*************************************************************************************
                Session("dsAnagDoppie") = dsAnagDoppie
                GrdResult.DataSource = dsAnagDoppie
                GrdResult.DataBind()
            End If
            Select Case CInt(GrdResult.Rows.Count)
                Case 0
                    GrdResult.Visible = False
                    lblMessage.Text = "Non sono state trovate anagrafiche doppie"
                Case Is > 0
                    GrdResult.Visible = True
                    lblMessage.Text = ""
            End Select
            RegisterScript("parent.parent.Visualizza.DivAttesa.style.display='none';", Me.GetType)
        Catch err As Exception
            Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.SearchResultsAnagrafica.Page_Load.errore: ", err)
            Response.Redirect("../PaginaErrore.aspx")
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
            Dim sScript As String

            If IsNumeric(e.CommandArgument.ToString()) Then
                Dim IDRow As Integer = CInt(e.CommandArgument.ToString())
                If e.CommandName = "RowOpen" Then
                    For Each myRow As GridViewRow In GrdResult.Rows
                        If myRow.Cells(6).Text = IDRow.ToString() Then
                            sScript = "GoToDettaglio('" & CInt(myRow.Cells(6).Text) & "','" & CInt(CType(myRow.FindControl("hfIdDataAnagrafica"), HiddenField).Value) & "');"
                            RegisterScript(sScript, Me.GetType())
                            Exit For
                        End If
                    Next
                End If
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
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdResult.DataSource = CType(Session("dsAnagDoppie"), DataSet)
            If page.HasValue Then
                GrdResult.PageIndex = page.Value
            End If
            GrdResult.DataBind()
        Catch ex As Exception
            Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.SearchResultsAnagrafica.LoadSearch.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnSalva_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSalva.Click
        Try
            Dim iRetVal As Boolean
            Dim COD_CONTRIB_PRINCIPALE As Integer
            Dim COD_CONTRIB_SECONDARIO As Integer
            Dim ID_DATA_ANAGRAFICA As Long

            'Prelevo il codice contribuente dell'anagrafica principale
            For Each myRow As GridViewRow In GrdResult.Rows
                If CType(myRow.FindControl("ChkPrinc"), CheckBox).Checked = True Then
                    COD_CONTRIB_PRINCIPALE = myRow.Cells(6).Text
                End If
            Next
            'Prelevo il codice contribuente e l'id data anagrfaica dell'anagrafica secondaria
            For Each myRow As GridViewRow In GrdResult.Rows
                If CType(myRow.FindControl("ChkSelect"), CheckBox).Checked = True Then
                    COD_CONTRIB_SECONDARIO = myRow.Cells(6).Text
                    ID_DATA_ANAGRAFICA = CType(myRow.FindControl("hfIdDataAnagrafica"), HiddenField).Value
                    If COD_CONTRIB_SECONDARIO <> COD_CONTRIB_PRINCIPALE Then
                        iRetVal = oAnagrafica.UpdateCodContribNelSistema(COD_CONTRIB_PRINCIPALE, COD_CONTRIB_SECONDARIO, ID_DATA_ANAGRAFICA, ANAGRAFICAWEB.ConstSession.IdEnte, ANAGRAFICAWEB.ConstSession.DBType, ANAGRAFICAWEB.ConstSession.StringConnectionAnagrafica)
                    End If
                End If
            Next

            'ricarico le griglia
            '***20090728 è stata cambiata la query in modo da aprire un'unica connessione al db***
            dsAnagDoppie = oAnagrafica.GetAnagraficaAnagraficheDoppie(intTipoRicerca, dblPerc, sNominativo, HasVuoti, ANAGRAFICAWEB.ConstSession.IdEnte, ANAGRAFICAWEB.ConstSession.DBType, ANAGRAFICAWEB.ConstSession.StringConnectionAnagrafica)
            '*************************************************************************************

            ViewState.Add("dsAnagDoppie", dsAnagDoppie)
            GrdResult.DataSource = dsAnagDoppie
            GrdResult.DataBind()

            Dim sScript As String = ""
            If iRetVal = False Then
                sScript += "GestAlert('a', 'warnig', '', '', 'Aggiornamento non effettuato!');"
                sScript += "parent.parent.Visualizza.DivAttesa.style.display='none';"
            Else
                sScript += "GestAlert('a', 'success', '', '', 'Aggiornamento del soggetto anagrafico nell\'intero sistema effettuato con successo!');"
                sScript += "parent.parent.Visualizza.DivAttesa.style.display='none';"
            End If
            RegisterScript(sScript, Me.GetType)
        Catch err As Exception
            Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.SearchResultsAnagrafica.btnSalva_Click.errore: ", err)
            Response.Redirect("../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnStampaExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStampaExcel.Click
        Dim sNameXLS As String
        Dim aListColonne As ArrayList
        Dim aMyHeaders As String()
        Dim DtDatiStampa As New DataTable
        Dim x As Integer
        Dim nCol As Integer = 4

        DtDatiStampa = Stampa()
        If Not IsNothing(DtDatiStampa) Then
            'valorizzo il nome del file
            sNameXLS = ANAGRAFICAWEB.ConstSession.IdEnte & "_ANAGRAFICHE_DOPPIE_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
            'definisco le colonne
            aListColonne = New ArrayList
            For x = 0 To nCol
                aListColonne.Add("")
            Next
            aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

            'Dim MyCol As Integer() = {0, 1, 2, 3, 4}
            Dim MyCol() As Integer = New Integer(nCol) {}
            For x = 0 To nCol
                MyCol(x) = x
            Next
            Dim objExport As New RKLib.ExportData.Export("Web")
            objExport.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
        End If
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Private Function Stampa() As DataTable
        Dim DtDatiStampa As New DataTable
        Dim RowStampa As DataRow
        Dim nRow, nCol As Integer
        Dim dvDati As DataView
        Dim dsDatiStampa As New DataSet

        Try
            DtDatiStampa = Nothing
            nCol = 4
            If Not Session("dsAnagDoppie") Is Nothing Then
                DtDatiStampa = New DataTable
                dvDati = CType(Session("dsAnagDoppie"), DataSet).Tables(0).DefaultView
                dsDatiStampa = New DataSet
                dsDatiStampa.Tables.Add("STAMPA_ANAGRAFICHE")
                For nRow = 1 To nCol + 1
                    dsDatiStampa.Tables("STAMPA_ANAGRAFICHE").Columns.Add("Col" & nRow.ToString.PadLeft(3, "0"))
                Next

                DtDatiStampa = dsDatiStampa.Tables("STAMPA_ANAGRAFICHE")
                RowStampa = DtDatiStampa.NewRow
                RowStampa(0) = "Prospetto Anagrafiche"
                RowStampa(2) = "Data Stampa:" & DateTime.Now.Date
                DtDatiStampa.Rows.Add(RowStampa)
                RowStampa = DtDatiStampa.NewRow
                DtDatiStampa.Rows.Add(RowStampa)
                RowStampa = DtDatiStampa.NewRow
                DtDatiStampa.Rows.Add(RowStampa)
                RowStampa = DtDatiStampa.NewRow
                nRow = 0
                RowStampa(nRow) = "Nominativo"
                nRow += 1
                RowStampa(nRow) = "Codice Fiscale"
                nRow += 1
                RowStampa(nRow) = "Partita IVA"
                nRow += 1
                RowStampa(nRow) = "Indirizzo Residenza"
                nRow += 1
                RowStampa(nRow) = "Cod.Contribuente"
                DtDatiStampa.Rows.Add(RowStampa)
                For Each myItem As DataRowView In dvDati
                    RowStampa = DtDatiStampa.NewRow
                    nRow = 0
                    If Not IsDBNull(myItem("COGNOME_DENOMINAZIONE")) Then
                        RowStampa(nRow) = CStr(myItem("COGNOME_DENOMINAZIONE"))
                    End If
                    If Not IsDBNull(myItem("NOME")) Then
                        RowStampa(nRow) += " " & CStr(myItem("NOME"))
                    End If
                    nRow += 1
                    If Not IsDBNull(myItem("COD_FISCALE")) Then
                        RowStampa(nRow) = "'" & CStr(myItem("COD_FISCALE"))
                    End If
                    nRow += 1
                    If Not IsDBNull(myItem("PARTITA_IVA")) Then
                        If CStr(myItem("PARTITA_IVA")).Trim <> "" Then
                            RowStampa(nRow) = "'" & CStr(myItem("PARTITA_IVA"))
                        End If
                    End If
                    nRow += 1
                    If Not IsDBNull(myItem("RESIDENZA")) Then
                        RowStampa(nRow) = CStr(myItem("RESIDENZA"))
                    End If
                    nRow += 1
                    If Not IsDBNull(myItem("COD_CONTRIBUENTE")) Then
                        RowStampa(nRow) = CStr(myItem("COD_CONTRIBUENTE"))
                    End If
                    DtDatiStampa.Rows.Add(RowStampa)
                Next
                RowStampa = DtDatiStampa.NewRow
                DtDatiStampa.Rows.Add(RowStampa)
                RowStampa = DtDatiStampa.NewRow
                DtDatiStampa.Rows.Add(RowStampa)
                RowStampa = DtDatiStampa.NewRow
                RowStampa(0) = "Totale Contribuenti: " & (dvDati.Count)
                DtDatiStampa.Rows.Add(RowStampa)
            End If
            Return DtDatiStampa
        Catch Err As Exception
            Log.Debug(ANAGRAFICAWEB.ConstSession.IdEnte + "." + ANAGRAFICAWEB.ConstSession.UserName + " - Anagrafica.SearchResultsAnagrafica.Stampa.errore: ", Err)
            Response.Redirect("../PaginaErrore.aspx")
            Return Nothing
        Finally
            RegisterScript("parent.parent.Visualizza.DivAttesa.style.display='none';", Me.GetType)
        End Try
    End Function
End Class
