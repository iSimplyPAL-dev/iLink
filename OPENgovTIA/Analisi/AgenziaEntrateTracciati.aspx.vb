Imports log4net
''' <summary>
''' Pagina per l'estrazione dei flussi per l'agenzia delle entrate.
''' I dati dei flussi sono gestiti dal Web Service FncWSOpenAE.
''' Per una corretta estrazione del tracciato procedere come segue:
''' - effettuare il controllo sui dati mancanti
''' - popolare la tabella d'appoggio per l'estrazione del tracciato
''' - selezionare l'anno nella tabella e generare il file
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class AgenziaEntrateTracciati
    Inherits BaseEnte
    Private Shared Log As ILog = LogManager.GetLogger(GetType(AgenziaEntrateTracciati))
    Protected FncGrd As New Formatta.FunctionGrd
    Private WFErrore As String = ""
    Private sScript As String = ""
    Private oListTracciati() As AgenziaEntrateDLL.AgenziaEntrate.objFlussoAE
    Private arrayobjFlussiAE() As FncWSOpenAE.objFlussoAE
    Private FncWSOpenAE As New FncWSOpenAE.ServiceOPENae
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub AgenziaEntrateTracciati_Init(sender As Object, e As EventArgs) Handles Me.Init
        lblTitolo.Text = Session("DESCRIZIONE_ENTE")
        If ConstSession.IsFromTARES = "1" Then
            info.InnerText = "TARES " + info.InnerText
        Else
            info.InnerText = "TARSU " + info.InnerText
        End If
    End Sub
    ''' <summary>
    ''' Al caricamento della pagina visualizza il riepilogo dei flussi già elaborati e carica la combo con i possibili anni da trattare.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Page.IsPostBack = False Then
                'popolo combo Anno
                LoadComboAnnoDichiarazioni(DdlAnno)
                'setto l'url del servizio
                FncWSOpenAE.Url = ConstSession.UrlWSOPENae
                Log.Debug("Richiamo il WEB SERVICE all'url:: " & FncWSOpenAE.Url)
                'popolo il dataview con il risultato della ricerca richiamando la funzione GetListFlussiTracciati(sAnno, sCognome, sNome, sCodFiscale, sPIva, sUbicazione);
                Try
                    arrayobjFlussiAE = FncWSOpenAE.GetFlussiTracciatiAE(Utility.Costanti.TRIBUTO_TARSU, ConstSession.IdEnte, WFErrore)
                Catch Err As Exception
                    Log.Debug("Tracciati.Page_Load.richiamoWS::errore::" & Err.Message & "::Richiamato il WEB SERVICE all'url:: " & FncWSOpenAE.Url)
                End Try
                'se il dataview non è vuoto:
                If Not arrayobjFlussiAE Is Nothing Then
                    oListTracciati = LoadListTracciati(arrayobjFlussiAE)
                    'assegno il dataview alla griglia;
                    GrdTracciatiAE.DataSource = oListTracciati
                    GrdTracciatiAE.DataBind()
                Else
                    If WFErrore = "" Then
                        'visualizzo la label di non sono presenti record per i parametri;
                        LblMessage.Text = "La ricerca non ha prodotto risultati."
                    Else
                        Throw New Exception
                    End If
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.AgenziaEntrateTracciati.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Pulsante per il popolamento della tabella di appoggio
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdPopolaTabAppoggio_Click(sender As Object, e As EventArgs) Handles CmdPopolaTabAppoggio.Click
        Try
            Dim oListDatiAE() As FncWSOpenAE.DisposizioneAE
            Dim FncTracciati As New ClsAETracciati

            If DdlAnno.SelectedValue <> "" Then
                'setto l'url del servizio
                FncWSOpenAE.Url = ConstSession.UrlWSOPENae
                Log.Debug("Richiamo il WEB SERVICE all'url:: " & FncWSOpenAE.Url)
                'prelevo i dati per popolare la tabella d'appoggio
                oListDatiAE = FncTracciati.GetDisposizioni(ConstSession.IdEnte, DdlAnno.SelectedValue, TxtDal.Text)
                'popolo la tabella d'appoggio
                If Not oListDatiAE Is Nothing Then
                    FncWSOpenAE.PopolaTabAppoggioAE(oListDatiAE)
                    'popolo il dataview con il risultato della ricerca richiamando la funzione GetListFlussiTracciati(sAnno, sCognome, sNome, sCodFiscale, sPIva, sUbicazione);
                    arrayobjFlussiAE = FncWSOpenAE.GetFlussiTracciatiAE(Utility.Costanti.TRIBUTO_TARSU, ConstSession.IdEnte, WFErrore)
                    'se il dataview non è vuoto:
                    If Not arrayobjFlussiAE Is Nothing Then
                        oListTracciati = LoadListTracciati(arrayobjFlussiAE)
                        'assegno il dataview alla griglia;
                        GrdTracciatiAE.DataSource = oListTracciati
                        GrdTracciatiAE.DataBind()
                        sScript = "GestAlert('a', 'success', '', '', 'Popolamento effettuato con successo!')"
                    Else
                        If WFErrore = "" Then
                            'visualizzo la label di non sono presenti record per i parametri;
                            LblMessage.Text = "La ricerca non ha prodotto risultati."
                        Else
                            Throw New Exception
                        End If
                        sScript = "GestAlert('a', 'danger', '', '', 'Errore durante il popolamento!')"
                    End If
                Else
                    sScript = "GestAlert('a', 'warning', '', '', 'Non sono presenti elementi da estrarre!')"
                End If
            Else
                sScript = "GestAlert('a', 'warning', '', '', 'Selezionare un\'anno!')"
            End If
            RegisterScript(sScript, Me.GetType)
            DivAttesa.Style.Add("display", "none")
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.AgenziaEntrateTracciati.CmdPopolaTabAppoggio.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Pulsante per la creazione del flusso da inviare ad Agenzia Entrate
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdEstraiTracciatoAE_Click(sender As Object, e As EventArgs) Handles CmdEstraiTracciatoAE.Click
        Dim sFileEstratto, sNameFileEstratto As String
        sFileEstratto = ""
        sNameFileEstratto = ""
        Try
            If DdlAnno.SelectedValue <> "" Then
                'setto l'url del servizio
                FncWSOpenAE.Url = ConstSession.UrlWSOPENae
                Log.Debug("Richiamo il WEB SERVICE all'url:: " & FncWSOpenAE.Url)
                sFileEstratto = FncWSOpenAE.EstraiTracciatoAE(Utility.Costanti.TRIBUTO_TARSU, DdlAnno.SelectedValue, ConstSession.IdEnte, sNameFileEstratto)
                'popolo il dataview con il risultato della ricerca richiamando la funzione GetListFlussiTracciati(sAnno, sCognome, sNome, sCodFiscale, sPIva, sUbicazione);
                arrayobjFlussiAE = FncWSOpenAE.GetFlussiTracciatiAE(Utility.Costanti.TRIBUTO_TARSU, ConstSession.IdEnte, WFErrore)
                'se il dataview non è vuoto:
                If Not arrayobjFlussiAE Is Nothing Then
                    oListTracciati = LoadListTracciati(arrayobjFlussiAE)
                    'assegno il dataview alla griglia;
                    GrdTracciatiAE.DataSource = oListTracciati
                    GrdTracciatiAE.DataBind()
                Else
                    If WFErrore = "" Then
                        'visualizzo la label di non sono presenti record per i parametri;
                        LblMessage.Text = "La ricerca non ha prodotto risultati."
                    Else
                        Throw New Exception
                    End If
                End If
            Else
                sScript = "DivAttesa.style.display='none';GestAlert('a', 'warning', '', '', 'Selezionare un\'anno!')"
                RegisterScript(sScript, Me.GetType)
            End If
            DivAttesa.Style.Add("display", "none")
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.AgenziaEntrateTracciati.CmdEstraiTracciatoAE.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
        If sFileEstratto <> "" Then
            Dim FncDownload As New Net.WebClient
            FncDownload.DownloadFile(sFileEstratto, ConstSession.PathFileAE + sNameFileEstratto)
            Response.ContentType = "*/*"
            Response.AppendHeader("content-disposition", "attachment; filename=" + sNameFileEstratto)
            Response.WriteFile(ConstSession.PathFileAE + sNameFileEstratto)
            Response.End()
            sScript = "GestAlert('a', 'success', '', '', 'Estrazione effettuata con successo!');"
            RegisterScript(sScript, Me.GetType())
        Else
            sScript = "GestAlert('a', 'danger', '', '', 'Errore durante l\'estrazione!');"
            RegisterScript(sScript, Me.GetType())
        End If
    End Sub
    ''' <summary>
    ''' Pulsante per la stampa del controllo dei dati mancanti
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdStampa_Click(sender As Object, e As EventArgs) Handles CmdStampa.Click
        Dim sPathProspetti, sNameXLS As String
        Dim DvDati As New DataView
        Dim FncTracciati As New ClsAETracciati
        Dim FncStampa As New ClsStampaXLS
        Dim DtDatiStampa As New DataTable
        Dim aListColonne As ArrayList
        Dim x, nCol As Integer
        Dim aMyHeaders As String()

        Try
            nCol = 10
            DvDati = FncTracciati.GetDisposizioniDatiMancanti(ConstSession.IdEnte, DdlAnno.SelectedValue, TxtDal.Text)
            DtDatiStampa = FncStampa.PrintAEDatiMancanti(DvDati, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, nCol)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.AgenziaEntrateTracciati.CmdStampa_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            DvDati.Dispose()
        End Try

        If Not DtDatiStampa Is Nothing Then
            sPathProspetti = ConstSession.PathProspetti
            sNameXLS = ConstSession.IdEnte & "_ELENCO_DATIMANCANTI_AE_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"

            'definisco le colonne
            aListColonne = New ArrayList
            For x = 0 To nCol
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
            RegisterScript(sScript, Me.GetType())
        End If
    End Sub
    ''' <summary>
    ''' Funzione per la conversione dal tipo FncWSOpenAE.objFlussoAE al tipo AgenziaEntrateDLL.AgenziaEntrate.objFlussoAE
    ''' </summary>
    ''' <param name="oFlussi"></param>
    ''' <returns>Array di AgenziaEntrateDLL.AgenziaEntrate.objFlussoAE</returns>
    Private Function LoadListTracciati(ByVal oFlussi() As FncWSOpenAE.objFlussoAE) As AgenziaEntrateDLL.AgenziaEntrate.objFlussoAE()
        Try
            Dim oMyTracciato As AgenziaEntrateDLL.AgenziaEntrate.objFlussoAE
            Dim oListTracciati() As AgenziaEntrateDLL.AgenziaEntrate.objFlussoAE = Nothing
            Dim x As Integer

            Log.Debug("Tracciati::LoadListTracciati::inizio procedura")
            For x = 0 To oFlussi.GetUpperBound(0)
                oMyTracciato = New AgenziaEntrateDLL.AgenziaEntrate.objFlussoAE
                oMyTracciato.Anno = oFlussi(x).Anno
                oMyTracciato.CodiceISTAT = oFlussi(x).CodiceISTAT
                oMyTracciato.DataEstrazione = oFlussi(x).DataEstrazione
                oMyTracciato.IdFlusso = oFlussi(x).IdFlusso
                oMyTracciato.NomeFile = oFlussi(x).NomeFile
                oMyTracciato.NumeroArticoli = oFlussi(x).NumeroArticoli
                oMyTracciato.NumeroRecords = oFlussi(x).NumeroRecords
                oMyTracciato.NumeroUtenti = oFlussi(x).NumeroUtenti
                ReDim Preserve oListTracciati(x)
                oListTracciati(x) = oMyTracciato
            Next
            Return oListTracciati
            Log.Debug("Tracciati::LoadListTracciati::fine procedura")
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.AgenziaEntrateTracciati.LoadListTracciati.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' Funzione per il popolamento della combo con gli anni gestiti
    ''' </summary>
    ''' <param name="ddl"></param>
    Public Sub LoadComboAnnoDichiarazioni(ByVal ddl As DropDownList)
        dim sSQL as string
        Dim x As Integer
        Dim FncGen As New generalClass.generalFunction
        Try
            sSQL = "SELECT "
            For x = 2007 To Now.Year - 1
                sSQL += x & " AS ANNO," & x & " AS IDANNO UNION SELECT "
            Next
            sSQL = sSQL.Substring(0, sSQL.Length - 14)
            sSQL += " ORDER BY IDANNO DESC"
            FncGen.LoadComboGenerale(ddl, sSQL, ConstSession.StringConnection, False, Costanti.TipoDefaultCmb.NUMERO)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.AgenziaEntrateTracciati.LoadComboAnnoDichiarazioni.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
#Region "Griglie"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdRowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                If e.Row.Cells(1).Text = "" Or e.Row.Cells(1).Text = "&nbsp;" Then
                    e.Row.Cells(6).Visible = False
                Else
                    Dim MyButton As Button
                    MyButton = e.Row.Cells(6).Controls(0)
                    MyButton.CssClass = "BottoneDownload"
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.AgenziaEntrateTracciati.GrdRowDataBound.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Gestione degli eventi sulla griglia. Con il comando RowDownload viene scaricato il flusso prodotto.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        'Try
        Dim IDRow As String = e.CommandArgument.ToString()
        If e.CommandName = "RowDownload" Then
            For Each myRow As GridViewRow In GrdTracciatiAE.Rows
                If IDRow = CType(myRow.FindControl("hfid"), HiddenField).Value Then
                    Response.ContentType = "*/*"
                    Response.AppendHeader("content-disposition", "attachment; filename=" + myRow.Cells(1).Text)
                    Response.WriteFile(ConstSession.PathFileAE + myRow.Cells(1).Text)
                    Response.End()
                End If
            Next
        End If
        'Catch ex As Exception
        '   Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.AgenziaEntrateTracciati.GrdRowCommand.errore: ", ex)
        '     Response.Redirect("../../PaginaErrore.aspx")
        'End Try
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
            GrdTracciatiAE.DataSource = CType(Session("ListAnagrafica"), DataView)
            If page.HasValue Then
                GrdTracciatiAE.PageIndex = page.Value
            End If
            GrdTracciatiAE.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.AgenziaEntrateTracciati.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
End Class