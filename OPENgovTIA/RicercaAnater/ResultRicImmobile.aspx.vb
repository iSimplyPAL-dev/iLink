Imports RemotingInterfaceAnater
Imports Anater.Oggetti
Imports log4net
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
''' <summary>
''' Pagina per la visualizzazione del risultato della ricerca immobili da ANATER.
''' Contiene la griglia dalla quale è possibile accedere al dettaglio e le funzioni della comandiera.
''' </summary>
Partial Class ResultRicImmobile
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ResultRicImmobile))
    Protected FncGrd As New Formatta.FunctionGrd

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
        'Put user code to initialize the page here
        Dim oRicercaAnater As New RicercaTarsuAnater
        Dim TypeOfRI As Type = GetType(IRemotingInterfaceTARSU)
        Dim RemobjTARSU As IRemotingInterfaceTARSU
        Dim oImmobileAnater() As OggettoUnitaImmobiliareAnater

        Try
            'carico i parametri di ricerca
            If Page.IsPostBack = False Then
                oRicercaAnater.CodiceComune = ConstSession.IdEnte
                If Request.Item("TxtCodVia") <> "" Then
                    oRicercaAnater.CodVia = Request.Item("TxtCodVia")
                End If
                If Request.Item("TxtVia") <> "" Then
                    oRicercaAnater.Via = Request.Item("TxtVia")
                End If
                If Request.Item("TxtCivico") <> "" Then
                    oRicercaAnater.Civico = Request.Item("TxtCivico")
                End If
                If Request.Item("TxtInterno") <> "" Then
                    oRicercaAnater.Interno = Request.Item("TxtInterno")
                End If
                If Request.Item("TxtFoglio") <> "" Then
                    oRicercaAnater.Foglio = Request.Item("TxtFoglio")
                End If
                If Request.Item("TxtNumero") <> "" Then
                    oRicercaAnater.Numero = Request.Item("TxtNumero")
                End If
                If Request.Item("TxtSubalterno") <> "" Then
                    oRicercaAnater.Subalterno = Request.Item("TxtSubalterno")
                End If
                'attivo il servizio
                RemobjTARSU = Activator.GetObject(TypeOfRI, ConstSession.UrlServizioAnater)
                'eseguo la ricerca su ANATER
                oImmobileAnater = RemobjTARSU.GetUnitaImmobiliare(oRicercaAnater)
                Log.Debug("ResultRicImmobile::Page_Load::trovati " & oImmobileAnater.Length & " immobili in territorio")
                'carico i dati in griglia
                If oImmobileAnater.Length > 0 Then
                    If (Request.Item("NewVani") <> 1) Or (Request.Item("NewVani") = 1 And oImmobileAnater.Length = 1) Then
                        GrdRicerca.DataSource = oImmobileAnater
                        GrdRicerca.DataBind()
                        LblResult.Style.Add("display", "none")
                    ElseIf (Request.Item("NewVani") = 1 And oImmobileAnater.Length > 1) Then
                        LblResult.Text = "Sono presenti più Immobili per l'indirizzo. Impossibile visualizzarne i vani."
                    End If
                End If
                Session("oImmobileAnater") = oImmobileAnater
                If Request.Item("NewVani") = 1 Then
                    SettaCheckbox()
                    Dim sScript As String = "document.getElementById('CmdSearchVani').click();"
                    RegisterScript(sScript, Me.GetType)
                End If
            Else
                If Request.Item("NewVani") <> 1 Then
                    ControllaCheckbox()
                Else
                    GrdRicerca.Enabled = False
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultRicImmobile.Page_Load.errore: ", Err)
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
    'Private Sub GrdRicerca_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles GrdRicerca.SortCommand
    '    Dim oImmobileAnater() As OggettoUnitaImmobiliareAnater

    '    oImmobileAnater = CType(Session("oImmobileAnater"), OggettoUnitaImmobiliareAnater())
    'Try
    '    If ViewState("OrderBy") = Utility.TipoOrdinamento.Crescente Then
    '        ViewState("OrderBy") = Utility.TipoOrdinamento.Decrescente
    '    Else
    '        ViewState("OrderBy") = Utility.TipoOrdinamento.Crescente
    '    End If

    '    If e.SortExpression = "UBICAZIONE" Then
    '        Dim objComparer As New Utility.Comparatore(New String() {"UniTipoVia", "UniDescrizioneVia"}, New Boolean() {ViewState("OrderBy"), ViewState("OrderBy")})
    '        Array.Sort(oImmobileAnater, objComparer)
    '    End If

    '    GrdRicerca.DataSource = oImmobileAnater
    '   Catch Err As Exception
    '     Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultRicImmobile.GrdRicerca_SortCommand.errore: ", Err)
    '     Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdRicerca.DataSource = CType(Session("oImmobileAnater"), OggettoUnitaImmobiliareAnater())
            If page.HasValue Then
                GrdRicerca.PageIndex = page.Value
            End If
            GrdRicerca.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultRicImmobile.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub

    Private Sub CmdSearchVani_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdSearchVani.Click
        Try
            Dim x, y, nList As Integer
            Dim oRicercaAnater As New RicercaTarsuAnater
            Dim TypeOfRI As Type = GetType(IRemotingInterfaceTARSU)
            Dim RemobjTARSU As IRemotingInterfaceTARSU
            Dim oImmobileAnater() As OggettoUnitaImmobiliareAnater
            Dim oVanoAnater() As OggettoVanoAnater
            Dim oListVaniAnater() As OggettoVanoAnater = Nothing

            nList = -1 : Session.Remove("oListVaniAnater")
            oImmobileAnater = CType(Session("oImmobileAnater"), OggettoUnitaImmobiliareAnater())
            For x = 0 To oImmobileAnater.GetUpperBound(0)
                'se è selezionata la riga devo fare la ricerca per i vani
                If oImmobileAnater(x).Selezionato = True Then
                    'carico i parametri di ricerca
                    oRicercaAnater.CodiceComune = ConstSession.IdEnte
                    oRicercaAnater.Foglio = oImmobileAnater(x).UniFoglio
                    oRicercaAnater.Numero = oImmobileAnater(x).UniNumMapp
                    oRicercaAnater.Subalterno = oImmobileAnater(x).UniSubalterno
                    oRicercaAnater.ProgressivoFabbricato = oImmobileAnater(x).UniNumProgrFabbricato
                    Log.Debug("cerco vani per CodiceComune->" + ConstSession.IdEnte + ", oRicercaAnater.Foglio-> " + oImmobileAnater(x).UniFoglio + ", oRicercaAnater.Numero->" + oImmobileAnater(x).UniNumMapp + ", oRicercaAnater.Subalterno-> " + oImmobileAnater(x).UniSubalterno + ", oRicercaAnater.ProgressivoFabbricato->" + oImmobileAnater(x).UniNumProgrFabbricato)
                    'attivo il servizio
                    RemobjTARSU = Activator.GetObject(TypeOfRI, ConstSession.UrlServizioAnater)
                    'eseguo la ricerca su ANATER
                    oVanoAnater = RemobjTARSU.GetVaniAnater(oRicercaAnater)
                    'carico i vani già prelevati
                    If Not Session("oListVaniAnater") Is Nothing Then
                        oListVaniAnater = Session("oListVaniAnater")
                        'calcolo il numero di vani già prelevati
                        nList = oListVaniAnater.GetUpperBound(0)
                    End If
                    For y = 0 To oVanoAnater.GetUpperBound(0)
                        nList += 1
                        Dim oMyVano As New OggettoVanoAnater
                        oMyVano = oVanoAnater.GetValue(y)
                        oMyVano.IDProgressivo = CInt(oImmobileAnater(x).IDProgressivo & oMyVano.IDProgressivo)
                        oMyVano.IDProgressivoUI = oImmobileAnater(x).IDProgressivo
                        ReDim Preserve oListVaniAnater(nList)
                        oListVaniAnater(nList) = oMyVano
                    Next
                End If
            Next
            'carico i dati in griglia
            Session("oListVaniAnater") = oListVaniAnater
            'carico la pagina del risultato
            Dim sScript As String
            sScript = "LoadGridVani.src='ResultRicVani.aspx'"
                    RegisterScript( sScript,Me.GetType)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultRicImmobile.CmdSearchVani_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub CmdRibaltaAnater_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdRibaltaAnater.Click
        Dim sScript As String
        Dim x, nListDett, nDett, nListUIGrd As Integer
        Dim oListImmobile() As OggettoUnitaImmobiliareAnater
        Dim oListDettaglioTestata() As ObjDettaglioTestata = Nothing
        Dim oListUIGrd() As ObjDettaglioTestata = Nothing

        Try
            oListImmobile = Session("oImmobileAnater")
            'ribalto
            'controllo se ho già degli immobili inseriti
            If Not Session("oListUITessera") Is Nothing Then
                oListDettaglioTestata = Session("oListUITessera")
                nListDett = oListDettaglioTestata.GetUpperBound(0)
            Else
                nListDett = -1
            End If
            'per la visualizzazione in griglia come sopra
            If Not Session("oListImmobili") Is Nothing Then
                oListUIGrd = Session("oListImmobili")
                nListUIGrd = oListUIGrd.GetUpperBound(0)
            Else
                nListUIGrd = -1
            End If

            For x = 0 To oListImmobile.GetUpperBound(0)
                If oListImmobile(x).Selezionato = True Then
                    'se non arrivo da NEWVANI devo aggiungere il dettaglio testata con relativi vani associati
                    If Request.Item("NewVani") <> 1 Then
                        nListDett += 1
                        If AddImmobile(oListImmobile(x), oListDettaglioTestata, nListDett) < 0 Then
                            Response.Redirect("../../PaginaErrore.aspx")
                            Exit Sub
                        End If
                        'per la visualizzazione in griglia come sopra
                        nListUIGrd += 1
                        If AddImmobile(oListImmobile(x), oListUIGrd, nListUIGrd) < 0 Then
                            Response.Redirect("../../PaginaErrore.aspx")
                            Exit Sub
                        End If
                    Else
                        'devo aggiungere solo i vani selezionati all'immobile in modifica
                        If AddVani(oListImmobile(x), oListDettaglioTestata, nDett) < 0 Then
                            Response.Redirect("../../PaginaErrore.aspx")
                            Exit Sub
                        End If
                        'per la visualizzazione in griglia come sopra
                        If AddVani(oListImmobile(x), oListUIGrd, nDett) < 0 Then
                            Response.Redirect("../../PaginaErrore.aspx")
                            Exit Sub
                        End If
                    End If
                End If
            Next

            'per la visualizzazione in griglia come sotto
            If Not oListUIGrd Is Nothing Then
                Session("oListImmobili") = oListUIGrd
            End If

            If Not oListDettaglioTestata Is Nothing Then
                Session("oListUITessera") = oListDettaglioTestata
                If Request.Item("NewVani") = 1 Then
                    Session("oDatiVani") = oListDettaglioTestata(nDett).oOggetti
                End If
                sScript = "parent.parent.opener.document.getElementById('CmdRibaltaUIAnater').click();"
                sScript += "parent.parent.window.close();"
            Else
                sScript = "GestAlert('a', 'warning', '', '', 'Selezionare un Soggetto Anagrafico dalla griglia e premere il pulsante Associa.');"
            End If
            RegisterScript( sScript,Me.GetType)
            Session("TxtDataInizio") = Nothing : Session("IdCatTARSU") = Nothing : Session("DescrCatTARSU") = Nothing
            '*** 20140805 - Gestione Categorie Vani ***
            Session("IdCatTARES") = Nothing : Session("DescrCatTARES") = Nothing : Session("NCPF") = Nothing : Session("NCPV") = Nothing
            '*** ***
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultRicImmobile.CmdRibaltaAnater_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub ControllaCheckbox()
        Dim oImmobileAnater() As OggettoUnitaImmobiliareAnater
        Dim x, nSel As Integer

        Try
            nSel = 0
            oImmobileAnater = CType(Session("oImmobileAnater"), OggettoUnitaImmobiliareAnater())
            If Not oImmobileAnater Is Nothing Then
                For Each myRow As GridViewRow In GrdRicerca.Rows
                    'prendo l'ID da aggiornare
                    For x = 0 To oImmobileAnater.GetUpperBound(0)
                        If oImmobileAnater(x).IDProgressivo = CType(CType(myRow.FindControl("hfIDProgressivo"), HiddenField).Value, Long) Then
                            oImmobileAnater(x).Selezionato = CType(myRow.FindControl("ChkSelezionato"), CheckBox).Checked
                            If oImmobileAnater(x).Selezionato = True Then
                                nSel += 1
                            End If
                        End If
                    Next
                Next
            End If
            Session("oImmobileAnater") = oImmobileAnater
            'se arrivo da ruolo posso selezionare un'immobile solo
            If Request.Item("Provenienza") = "A" And nSel > 1 Then
                Dim sScript As String
                sScript = "GestAlert('a', 'warning', '', '', 'E\' possibile associare un solo immobile al ruolo.\n Sarà associato solo il primo selezionato!');"
                RegisterScript( sScript,Me.GetType)
            End If
            Log.Debug("fine controllacheckbox")
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultRicImmobile.ControllaCheckbox.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub SettaCheckbox()
        Dim itemGrid As GridViewRow
        Dim oImmobileAnater() As OggettoUnitaImmobiliareAnater
        Dim x, nSel As Integer

        Try
            nSel = 0
            oImmobileAnater = CType(Session("oImmobileAnater"), OggettoUnitaImmobiliareAnater())
            For Each itemGrid In GrdRicerca.Rows
                'prendo l'ID da aggiornare
                For x = 0 To oImmobileAnater.GetUpperBound(0)
                    If oImmobileAnater(x).IDProgressivo = CType(CType(itemGrid.FindControl("hfidprogressivo"), HiddenField).Value, Long) Then
                        CType(itemGrid.FindControl("ChkSelezionato"), CheckBox).Checked = True
                        oImmobileAnater(x).Selezionato = True
                    End If
                Next
            Next
            Session("oImmobileAnater") = oImmobileAnater
            'se arrivo da ruolo posso selezionare un'immobile solo
            If Request.Item("Provenienza") = "A" And nSel > 1 Then
                Dim sScript As String
                sScript = "GestAlert('a', 'warning', '', '', 'E\' possibile associare un solo immobile al ruolo.\n Sarà associato solo il primo selezionato!');"
                RegisterScript( sScript,Me.GetType)
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultRicImmobile.SettaCheckbox.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    'Public Function FormattaDataGrd(ByVal tDataGrd As DateTime) As String
    '    If tDataGrd = Date.MinValue Then
    '        Return ""
    '    Else
    '        Return tDataGrd.ToShortDateString
    '    End If
    'End Function

    Private Function AddImmobile(ByVal oMyImmobile As OggettoUnitaImmobiliareAnater, ByRef oListDettaglioTestataAdd() As ObjDettaglioTestata, ByRef nListDettAdd As Integer) As Integer
        Dim oMyDettTestata As ObjDettaglioTestata
        Dim x, nListVaniAdd As Integer
        Dim oListVani() As OggettoVanoAnater
        Dim oMyVani As ObjOggetti
        Dim oListDettVani() As ObjOggetti = Nothing
        Dim oMyTot As New GestOggetti
        Dim retTotOggetti, nVani As Integer
        Dim nMQ As Double

        Try
            ReDim Preserve oListDettaglioTestataAdd(nListDettAdd)
            oMyDettTestata = New ObjDettaglioTestata
            oMyDettTestata.sCodVia = oMyImmobile.UniCodVia
            oMyDettTestata.sVia = oMyImmobile.UniTipoVia & " " & oMyImmobile.UniDescrizioneVia
            oMyDettTestata.sCivico = oMyImmobile.UniNumeroCiv
            oMyDettTestata.sEsponente = oMyImmobile.UniEspoUbi
            oMyDettTestata.sInterno = oMyImmobile.UniInternoUbi
            oMyDettTestata.sScala = oMyImmobile.UniScalaUbi
            oMyDettTestata.sFoglio = oMyImmobile.UniFoglio
            oMyDettTestata.sNumero = oMyImmobile.UniNumMapp
            oMyDettTestata.sSubalterno = oMyImmobile.UniSubalterno
            If Not Session("TxtDataInizio") Is Nothing Then
                If Session("TxtDataInizio") <> "" Then
                    oMyDettTestata.tDataInizio = Session("TxtDataInizio")
                End If
            End If
            '*** 20140805 - Gestione Categorie Vani ***
            If Not Session("IdCatTARES") Is Nothing Then
                oMyDettTestata.IdCatAteco = Session("IdCatTARES")
                oMyDettTestata.sCatAteco = Session("DescrCatTARES")
            End If
            If Not Session("NCPF") Is Nothing Then
                oMyDettTestata.nNComponenti = Session("NCPF")
            End If
            If Not Session("NCPV") Is Nothing Then
                oMyDettTestata.nComponentiPV = Session("NCPV")
            End If
            '*** ***
            'oMyDettTestata.tDataInizio = oMyImmobile.UniDataInizio
            'oMyDettTestata.tDataFine = oMyImmobile.UniDataFine
            oMyDettTestata.sNoteUI = oMyImmobile.UniNote
            oMyDettTestata.tDataInserimento = Now
            nListVaniAdd = -1
            oListVani = Session("oListVaniAnater")
            If Not oListVani Is Nothing Then
                For x = 0 To oListVani.GetUpperBound(0)
                    If oMyImmobile.IDProgressivo = oListVani(x).IDProgressivoUI Then
                        If oListVani(x).selezionato = True Then
                            nListVaniAdd += 1
                            ReDim Preserve oListDettVani(nListVaniAdd)
                            oMyVani = New ObjOggetti
                            oMyVani.IdOggetto = -1
                            '***18/10/2007 - tolto ribaltamento perchè per Anater questa non è la categoria***
                            'oMyVani.IdCategoria = oMyImmobile.UniCodTabDestUso
                            'oMyVani.sCategoria = oMyImmobile.UniDescrDestUso
                            '***associo la categoria***
                            If Not Session("IdCatTARSU") Is Nothing Then
                                oMyVani.IdCategoria = Session("IdCatTARSU")
                                oMyVani.sCategoria = Session("DescrCatTARSU")
                            End If
                            '*** 20140805 - Gestione Categorie Vani ***
                            If Not Session("IdCatTARES") Is Nothing Then
                                oMyVani.IdCatTARES = Session("IdCatTARES")
                                oMyVani.sDescrCatTARES = Session("DescrCatTARES")
                            End If
                            If Not Session("NCPF") Is Nothing Then
                                oMyVani.nNC = Session("NCPF")
                            End If
                            If Not Session("NCPV") Is Nothing Then
                                oMyVani.nNCPV = Session("NCPV")
                            End If
                            '*** ***
                            oMyVani.IdTipoVano = oListVani(x).CUMCodDestinazione
                            oMyVani.sTipoVano = oListVani(x).DescrizioneDestinazione
                            oMyVani.nMq = oListVani(x).CUMSuperficie
                            oMyVani.nVani = 1
                            oMyVani.sProvenienza = "ANATER"
                            oMyVani.tDataInserimento = Now
                            oListDettVani(nListVaniAdd) = oMyVani
                        End If
                    End If
                Next
                'se ho dei vani li associo all'immobile
                If Not oListDettVani Is Nothing Then
                    'calcolo il totale di vani e mq per l'immobile
                    retTotOggetti = oMyTot.GetTotOggetti(oListDettVani, nVani, nMQ)
                    If retTotOggetti <> 1 Then
                        Response.Redirect("../../PaginaErrore.aspx")
                        Exit Function
                    End If
                    oMyDettTestata.oOggetti = oListDettVani
                    oListDettVani = Nothing
                    oMyDettTestata.nVani = nVani
                    oMyDettTestata.nMQ = nMQ
                End If
            End If
            oListDettaglioTestataAdd(nListDettAdd) = oMyDettTestata

            Return 1
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultRicImmobile.AddImmobile.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
            Return -1
        End Try
    End Function

    Private Function AddVani(ByVal oMyImmobile As OggettoUnitaImmobiliareAnater, ByRef oListDettaglioTestataAdd() As ObjDettaglioTestata, ByRef nDettaglioTestata As Integer) As Integer
        Dim x, y, nListVaniAdd As Integer
        Dim oListVani() As OggettoVanoAnater
        Dim oMyVani As ObjOggetti
        Dim oListDettVani() As ObjOggetti
        Dim oMyTot As New GestOggetti
        Dim retTotOggetti, nVani As Integer
        Dim nMQ As Double

        Try
            'ciclo per posizionarmi sullo stesso immobile
            For x = 0 To oListDettaglioTestataAdd.GetUpperBound(0)
                If oListDettaglioTestataAdd(x).sCodVia.ToUpper = oMyImmobile.UniCodVia.ToString.ToUpper And oListDettaglioTestataAdd(x).sVia.ToUpper = CStr(oMyImmobile.UniTipoVia & " " & oMyImmobile.UniDescrizioneVia).ToUpper And oListDettaglioTestataAdd(x).sCivico.ToUpper = oMyImmobile.UniNumeroCiv.ToString.ToUpper And oListDettaglioTestataAdd(x).sEsponente.ToUpper = oMyImmobile.UniEspoUbi.ToUpper And oListDettaglioTestataAdd(x).sInterno.ToUpper = oMyImmobile.UniInternoUbi.ToUpper And oListDettaglioTestataAdd(x).sScala.ToUpper = oMyImmobile.UniScalaUbi.ToUpper And oListDettaglioTestataAdd(x).sFoglio.ToUpper = oMyImmobile.UniFoglio.ToUpper And oListDettaglioTestataAdd(x).sNumero.ToUpper = oMyImmobile.UniNumMapp.ToUpper And oListDettaglioTestataAdd(x).sSubalterno.ToUpper = oMyImmobile.UniSubalterno.ToUpper Then
                    'memorizzo l'id dettaglio testata corretto
                    nDettaglioTestata = x
                    'carico i vani precedenti
                    nListVaniAdd = oListDettaglioTestataAdd(x).oOggetti.GetUpperBound(0)
                    oListDettVani = oListDettaglioTestataAdd(x).oOggetti
                    oListVani = Session("oListVaniAnater")
                    If Not oListVani Is Nothing Then
                        For y = 0 To oListVani.GetUpperBound(0)
                            If oMyImmobile.IDProgressivo = oListVani(y).IDProgressivoUI Then
                                If oListVani(y).selezionato = True Then
                                    nListVaniAdd += 1
                                    ReDim Preserve oListDettVani(nListVaniAdd)
                                    oMyVani = New ObjOggetti
                                    oMyVani.IdOggetto = y
                                    '***associo la categoria***
                                    If Not Session("IdCatTARSU") Is Nothing Then
                                        oMyVani.IdCategoria = Session("IdCatTARSU")
                                        oMyVani.sCategoria = Session("DescrCatTARSU")
                                    End If
                                    '*** 20140805 - Gestione Categorie Vani ***
                                    If Not Session("IdCatTARES") Is Nothing Then
                                        oMyVani.IdCatTARES = Session("IdCatTARES")
                                        oMyVani.sDescrCatTARES = Session("DescrCatTARES")
                                    End If
                                    If Not Session("NCPF") Is Nothing Then
                                        oMyVani.nNC = Session("NCPF")
                                    End If
                                    If Not Session("NCPV") Is Nothing Then
                                        oMyVani.nNCPV = Session("NCPV")
                                    End If
                                    '*** ***
                                    oMyVani.IdTipoVano = oListVani(y).CUMCodDestinazione
                                    oMyVani.sTipoVano = oListVani(y).DescrizioneDestinazione
                                    oMyVani.nMq = oListVani(y).CUMSuperficie
                                    oMyVani.nVani = 1
                                    oMyVani.sProvenienza = "ANATER"
                                    oMyVani.tDataInserimento = Now
                                    oListDettVani(nListVaniAdd) = oMyVani
                                End If
                            End If
                        Next
                    End If
                    'se ho dei vani li associo all'immobile
                    If Not oListDettVani Is Nothing Then
                        'calcolo il totale di vani e mq per l'immobile
                        retTotOggetti = oMyTot.GetTotOggetti(oListDettVani, nVani, nMQ)
                        If retTotOggetti <> 1 Then
                            Response.Redirect("../../PaginaErrore.aspx")
                            Exit Function
                        End If
                        oListDettaglioTestataAdd(x).oOggetti = oListDettVani
                        oListDettVani = Nothing
                        oListDettaglioTestataAdd(x).nVani = nVani
                        oListDettaglioTestataAdd(x).nMQ = nMQ
                    End If
                End If
            Next

            Return 1
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultRicImmobile.AddVani.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
            Return -1
        End Try
    End Function
End Class
