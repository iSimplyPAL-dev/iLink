Imports log4net
Imports Utility
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti

''' <summary>
''' Pagina per la visualizzazione del risultato della ricerca dichiarazioni.
''' Contiene la griglia dalla quale è possibile accedere al dettaglio e le funzioni della comandiera.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class ResultRicDichiarazione
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ResultRicDichiarazione))
    Protected FncGrd As New Formatta.FunctionGrd
    Private sErrResult As String
    Private myRicerca As New ClsDichiarazione
    Private DvResult As New DataView
    Private myDvResult() As ObjTestataSearch
    Private mySearch As New ObjSearchTestata
    Private NewRicerca As New ClsDichiarazione
    'Private WFSessione As OPENUtility.CreateSessione

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
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'Dim TipoDichiarazione As Integer
        Dim myGrd As Ribes.OPENgov.WebControls.RibesGridView

        Try
            If Not (Session("myObjectForTestataSearch")) Is Nothing Then
                mySearch = Session("myObjectForTestataSearch")
                If Page.IsPostBack = False Then
                    Log.Debug("ResultRicDichiarazione::DB::" & ConstSession.StringConnection)
                    Session("myDvResult") = Nothing
                    Session("OrderByOrder") = Nothing
                    Session("OrderByFields") = Nothing
                    If mySearch.rbSoggetto Then
                        '*** 201511 - Funzioni Sovracomunali ***
                        'myDvResult = NewRicerca.GetSoggettiFromDich(ConstSession.StringConnection, ConstSession.IdEnte, mySearch, sErrResult)
                        myDvResult = NewRicerca.GetSoggettiFromDich(ConstSession.StringConnection, mySearch, sErrResult)
                        '*** ***
                        If sErrResult <> "" Then
                            Response.Redirect("../../PaginaErrore.aspx")
                        Else
                            If Not IsNothing(myDvResult) Then
                                If myDvResult.Length > 0 Then
                                    Session.Add("myDvResult", myDvResult)
                                    ViewState("SortKey") = "sCognome"
                                    ViewState("OrderBy") = TipoOrdinamento.Crescente
                                    ' ORDINO PER COGNOME e NOME
                                    Dim objComparer As New Comparatore(New String() {"DescrizioneEnte", "sCognome", "sNome"}, New Boolean() {ViewState("OrderBy"), ViewState("OrderBy"), ViewState("OrderBy")})
                                    Array.Sort(myDvResult, objComparer)
                                    GrdUtenti.DataSource = myDvResult
                                    GrdUtenti.DataBind()
                                End If
                            End If
                        End If
                    Else
                        '*** 201511 - Funzioni Sovracomunali ***
                        'myDvResult = NewRicerca.GetSoggettiFromImmobili(ConstSession.StringConnection, ConstSession.IdEnte, mySearch, sErrResult)
                        myDvResult = NewRicerca.GetSoggettiFromImmobili(ConstSession.StringConnection, mySearch, sErrResult)
                        '*** ***
                        If sErrResult <> "" Then
                            Response.Redirect("../../PaginaErrore.aspx")
                        Else
                            If Not IsNothing(myDvResult) Then
                                If myDvResult.Length > 0 Then
                                    Session.Add("myDvResult", myDvResult)
                                    ViewState("SortKey") = "sCognome"
                                    ViewState("OrderBy") = TipoOrdinamento.Crescente
                                    ' ORDINO PER COGNOME e NOME
                                    Dim objComparer As New Comparatore(New String() {"DescrizioneEnte", "sCognome", "sNome"}, New Boolean() {ViewState("OrderBy"), ViewState("OrderBy"), ViewState("OrderBy")})
                                    Array.Sort(myDvResult, objComparer)
                                    GrdImmobili.DataSource = myDvResult
                                    GrdImmobili.DataBind()
                                End If
                            End If
                        End If
                    End If
                Else
                    '*** 20140923 - GIS ***
                    'SetGrdCheck(sTipoRicerca)
                    SetGrdCheck(mySearch.rbSoggetto)
                    '***  ***
                    myDvResult = CType(Session("myDvResult"), ObjTestataSearch())
                    If mySearch.rbSoggetto Then
                        GrdUtenti.DataSource = CType(Session("myDvResult"), ObjTestataSearch())
                    Else
                        GrdImmobili.DataSource = CType(Session("myDvResult"), ObjTestataSearch())
                    End If
                End If
            End If
            '*** 201511 - Funzioni Sovracomunali ***'*** X UNIONE CON BANCADATI CMGC ***
            Dim nColGIS As Integer = 0
            If mySearch.rbSoggetto Then
                myGrd = GrdUtenti
                nColGIS = 7
            Else
                myGrd = GrdImmobili
                nColGIS = 9
            End If
            If ConstSession.IdEnte = "" Then
                myGrd.Columns(0).Visible = True
            Else
                myGrd.Columns(0).Visible = False
            End If
            If ConstSession.VisualGIS = False Then
                myGrd.Columns(nColGIS).Visible = False
            End If
            '*** ***
            Dim sScript As String = ""
            If Not myDvResult Is Nothing Then
                If myDvResult.Length <= 0 Then
                    sScript = "$('#LblResult').show();"
                Else
                    sScript = "$('#LblResult').hide();"
                End If
            End If
            sScript += "parent.parent.Visualizza.DivAttesa.style.display='none';"
            RegisterScript(sScript, Me.GetType())
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultRicDichiarazione.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            RegisterScript("$('.divGrdBtn').hide();", Me.GetType())
        End Try
    End Sub
#Region "Griglie"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdRowCommand(ByVal sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim sScript As String
            Dim IDRow As Integer = CInt(e.CommandArgument.ToString())
            If e.CommandName = "RowOpen" Then
                '*** 201511 - Funzioni Sovracomunali ***'*** 20140923 - GIS ***
                If ConstSession.IdEnte <> "" Then
                    sScript = "LoadDichiarazione(" + IDRow.ToString + ");"
                Else
                    sScript = "GestAlert('a', 'warning', '', '', 'Impossibile accedere al dettaglio dalla funzione sovracomunale');"
                End If
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultRicDichiarazione.GrdRowCommand.errore: ", ex)
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
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdSorting(sender As Object, e As GridViewSortEventArgs)
        Try
            If e.SortExpression.ToString() = ViewState("SortKey").ToString() Then
                Select Case CBool(ViewState("OrderBy"))
                    Case TipoOrdinamento.Crescente
                        ViewState("OrderBy") = TipoOrdinamento.Decrescente

                    Case TipoOrdinamento.Decrescente
                        ViewState("OrderBy") = TipoOrdinamento.Crescente
                End Select
            Else
                ViewState("SortKey") = e.SortExpression
                ViewState("OrderBy") = TipoOrdinamento.Crescente
            End If
            myDvResult = CType(Session("myDvResult"), ObjTestataSearch())

            If Not IsNothing(myDvResult) Then
                ' ORDINO L'ARRAY DI OGGETTI
                If e.SortExpression = "sIndirizzo" Then
                    Dim objComparer As New Comparatore(New String() {"sVia", "sCivico", "sInterno", "sCognome", "sNome"}, New Boolean() {ViewState("OrderBy"), ViewState("OrderBy"), ViewState("OrderBy"), ViewState("OrderBy"), ViewState("OrderBy")})
                    Array.Sort(myDvResult, objComparer)
                Else
                    Dim objComparer As New Comparatore(New String() {e.SortExpression, "sCognome", "sNome"}, New Boolean() {ViewState("OrderBy"), ViewState("OrderBy"), ViewState("OrderBy")})
                    Array.Sort(myDvResult, objComparer)
                End If
            End If

            Dim myGrd As Ribes.OPENgov.WebControls.RibesGridView
            If mySearch.rbSoggetto Then
                myGrd = GrdUtenti
            Else
                myGrd = GrdImmobili
            End If
            myGrd.DataSource = myDvResult
            myGrd.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultRicDichiarazione.GrdSorting.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

#End Region
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="11/2015">
    ''' <strong>Funzioni Sovracomunali</strong>
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Esportazione completa dati</em>
    ''' </revision>
    ''' </revisionHistory>
    Private Sub CmdStampaAnalitica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdStampaAnalitica.Click
        'Stampa Dichiarazioni TARSU Analitico
        Dim sPathProspetti, sNameXLS As String
        Dim DvDati As New DataView
        Dim FncStampa As New ClsStampaXLS
        Dim DtDatiStampa As New DataTable
        Dim aListColonne As ArrayList
        Dim x, nCol As Integer
        Dim aMyHeaders As String()

        Try
            nCol = 36
            If Not (Session("myObjectForTestataSearch")) Is Nothing Then
                mySearch = Session("myObjectForTestataSearch")
                '*** 201511 - Funzioni Sovracomunali ***
                DvDati = NewRicerca.GetStampaDichiarazioni(ConstSession.StringConnection, ClsDichiarazione.TipoStampaAnalitica, mySearch)
                '*** ***
                DtDatiStampa = FncStampa.PrintDichiarazioniAnalitico(DvDati, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, ConstSession.IsFromTARES, nCol)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultRicDichiarazione.CmdStampaAnalitica_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            DvDati.Dispose()
        End Try

        If Not DtDatiStampa Is Nothing Then
            sPathProspetti = ConstSession.PathProspetti
            sNameXLS = ConstSession.IdEnte & "_ELENCO_DICHIARAZIONI_ANALITICO_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"

            'definisco le colonne
            aListColonne = New ArrayList
            For x = 0 To nCol '31
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
            sScript += "parent.parent.Visualizza.DivAttesa.style.display='none';"
            RegisterScript(sScript, Me.GetType)
        End If
    End Sub
    'Private Sub CmdStampaAnalitica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdStampaAnalitica.Click
    '    'Stampa Dichiarazioni TARSU Analitico
    '    Dim sPathProspetti, sNameXLS, Str As String
    '    Dim DvDati As New DataView
    '    Dim FncStampa As New ClsStampaXLS
    '    Dim DtDatiStampa As New DataTable
    '    Dim aListColonne As ArrayList
    '    Dim x, nCol As Integer
    '    Dim aMyHeaders As String()

    '    Try
    '        nCol = 36
    '        If Not (Session("myObjectForTestataSearch")) Is Nothing Then
    '            mySearch = Session("myObjectForTestataSearch")
    '            '*** 201511 - Funzioni Sovracomunali ***
    '            'DvDati = NewRicerca.GetStampaDichiarazioni(ConstSession.StringConnection, ConstSession.IdEnte, "A", mySearch)
    '            DvDati = NewRicerca.GetStampaDichiarazioni(ConstSession.StringConnection, "A", mySearch)
    '            '*** ***
    '            DtDatiStampa = FncStampa.PrintDichiarazioniAnalitico(DvDati, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, ConstSession.IsFromTARES, nCol)
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultRicDichiarazione.CmdStampaAnalitica_Click.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    Finally
    '        DvDati.Dispose()
    '    End Try

    '    If Not DtDatiStampa Is Nothing Then
    '        sPathProspetti = ConstSession.PathProspetti
    '        sNameXLS = ConstSession.IdEnte & "_ELENCO_DICHIARAZIONI_ANALITICO_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"

    '        'definisco le colonne
    '        aListColonne = New ArrayList
    '        For x = 0 To nCol '31
    '            aListColonne.Add("")
    '        Next
    '        aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

    '        'definisco l'insieme delle colonne da esportare
    '        'Dim MyCol() As Integer = New Integer() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31}
    '        Dim MyCol() As Integer = New Integer(nCol) {}
    '        For x = 0 To nCol
    '            MyCol(x) = x
    '        Next
    '        'esporto i dati in excel
    '        Dim MyStampa As New RKLib.ExportData.Export("Web")
    '        MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
    '        Str = sPathProspetti & sNameXLS
    '    Else
    '        Dim sScript As String = "GestAlert('a', 'danger', '', '', 'Errore in estrazione!');"
    '        sScript += "parent.parent.Visualizza.DivAttesa.style.display='none';"
    '        RegisterScript(sScript, Me.GetType)
    '    End If
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="11/2015">
    ''' <strong>Funzioni Sovracomunali</strong>
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Esportazione completa dati</em>
    ''' </revision>
    ''' </revisionHistory>
    Private Sub CmdStampaSintetica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdStampaSintetica.Click
        'Stampa Dichiarazioni TARSU Sintetico
        Dim sPathProspetti, sNameXLS As String
        Dim DvDati As New DataView
        Dim FncStampa As New ClsStampaXLS
        Dim DtDatiStampa As New DataTable
        Dim aListColonne As ArrayList
        Dim x, nCol As Integer
        Dim aMyHeaders As String()

        Try
            nCol = 38
            If Not (Session("myObjectForTestataSearch")) Is Nothing Then
                mySearch = Session("myObjectForTestataSearch")
                '*** 201511 - Funzioni Sovracomunali ***
                DvDati = NewRicerca.GetStampaDichiarazioni(ConstSession.StringConnection, ClsDichiarazione.TipoStampaSintetica, mySearch)
                DtDatiStampa = FncStampa.PrintDichiarazioniSintetico(DvDati, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, ConstSession.IsFromTARES, nCol)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultRicDichiarazione.CmdStampaSintetica_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            DvDati.Dispose()
        End Try

        If Not DtDatiStampa Is Nothing Then
            sPathProspetti = ConstSession.PathProspetti
            sNameXLS = ConstSession.IdEnte & "_ELENCO_DICHIARAZIONI_SINTETICO_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"

            'definisco le colonne
            aListColonne = New ArrayList
            For x = 0 To nCol '32
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
        End If
    End Sub
    'Private Sub CmdStampaSintetica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdStampaSintetica.Click
    '    'Stampa Dichiarazioni TARSU Sintetico
    '    Dim sPathProspetti, sNameXLS, Str As String
    '    Dim DvDati As New DataView
    '    Dim FncStampa As New ClsStampaXLS
    '    Dim DtDatiStampa As New DataTable
    '    Dim aListColonne As ArrayList
    '    Dim x, nCol As Integer
    '    Dim aMyHeaders As String()

    '    Try
    '        nCol = 38
    '        If Not (Session("myObjectForTestataSearch")) Is Nothing Then
    '            mySearch = Session("myObjectForTestataSearch")
    '            '*** 201511 - Funzioni Sovracomunali ***
    '            'DvDati = NewRicerca.GetStampaDichiarazioni(ConstSession.StringConnection, ConstSession.IdEnte, "S", mySearch)
    '            DvDati = NewRicerca.GetStampaDichiarazioni(ConstSession.StringConnection, "S", mySearch)
    '            DtDatiStampa = FncStampa.PrintDichiarazioniSintetico(DvDati, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, ConstSession.IsFromTARES, nCol)
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultRicDichiarazione.CmdStampaSintetica_Click.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    Finally
    '        DvDati.Dispose()
    '    End Try

    '    If Not DtDatiStampa Is Nothing Then
    '        sPathProspetti = ConstSession.PathProspetti
    '        sNameXLS = ConstSession.IdEnte & "_ELENCO_DICHIARAZIONI_SINTETICO_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"

    '        'definisco le colonne
    '        aListColonne = New ArrayList
    '        For x = 0 To nCol '32
    '            aListColonne.Add("")
    '        Next
    '        aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

    '        'definisco l'insieme delle colonne da esportare
    '        'Dim MyCol() As Integer = New Integer() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33}
    '        Dim MyCol() As Integer = New Integer(nCol) {}
    '        For x = 0 To nCol
    '            MyCol(x) = x
    '        Next
    '        'esporto i dati in excel
    '        Dim MyStampa As New RKLib.ExportData.Export("Web")
    '        MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
    '        Str = sPathProspetti & sNameXLS
    '    End If
    'End Sub
    '*** 20140923 - GIS ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="IsRicForSoggetto"></param>
    Private Sub SetGrdCheck(ByVal IsRicForSoggetto As Boolean)
        Dim myGrd As Ribes.OPENgov.WebControls.RibesGridView

        Try
            Dim myDvResult() As ObjTestataSearch = CType(Session("myDvResult"), ObjTestataSearch())
            If IsRicForSoggetto Then
                myGrd = GrdUtenti
            Else
                myGrd = GrdImmobili
            End If
            For Each itemGrid As GridViewRow In myGrd.Rows
                'prendo l'ID da aggiornare
                For Each myItem As ObjTestataSearch In myDvResult
                    If myItem.Id.ToString() = CType(itemGrid.FindControl("hfIdUI"), HiddenField).Value Then
                        myItem.bSel = CType(itemGrid.FindControl("chkSel"), CheckBox).Checked
                    End If
                Next
            Next
            Session("myDvResult") = myDvResult
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ResultRicDichiarazione.SetGrdCheck.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '***  ***
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            Dim myGrd As Ribes.OPENgov.WebControls.RibesGridView
            If mySearch.rbSoggetto Then
                myGrd = GrdUtenti
            Else
                myGrd = GrdImmobili
            End If
            myGrd.DataSource = CType(Session("myDvResult"), ObjTestataSearch())
            If page.HasValue Then
                myGrd.PageIndex = page.Value
            End If
            myGrd.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultRicDichiarazione.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
End Class
