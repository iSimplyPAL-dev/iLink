Imports log4net

Partial Class Tracciati
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger("Tracciati")
    Protected FncGrd As New ClsGenerale.FunctionGrd
    Private sScript As String
    Private WFErrore As String = ""
    Private oListTracciati() As AgenziaEntrateDLL.AgenziaEntrate.objFlussoAE
	Private arrayobjFlussiAE() As WSOPENae.objFlussoAE
	Private FncWSOpenAE As New WSOPENae.ServiceOPENae

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
        Try
            Dim FncGenerale As New ClsAEDatiMancanti

            If Page.IsPostBack = False Then
                Dim paginacomandi As String = "ComandiTracciati.aspx"
                Dim parametri As String = "?"

                Dim sScript As String = ""
                sScript += "document.getElementById('paginacomandi').value='" & paginacomandi & "';"
                sScript += "parent.Comandi.location.href='" & paginacomandi & parametri & "';"
                RegisterScript(sScript, Me.GetType())

                'popolo combo Anno
                FncGenerale.LoadComboAnnoPeriodi(DdlAnno, ConstSession.IdEnte)
                'setto l'url del servizio
                FncWSOpenAE.Url = System.Configuration.ConfigurationManager.AppSettings("URLWSOPENae")
                Log.Debug("Tracciati::Page_Load::FncWSOpenAE.Url::" & FncWSOpenAE.Url)
                'popolo il dataview con il risultato della ricerca richiamando la funzione GetListFlussiTracciati(sAnno, sCognome, sNome, sCodFiscale, sPIva, sUbicazione);
                arrayobjFlussiAE = FncWSOpenAE.GetFlussiTracciatiAE("9000", ConstSession.IdEnte, WFErrore)
                Log.Debug("Tracciati::Page_Load::richiamato GetFlussiTracciatiAE per ::" & ConstSession.IdEnte)
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
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.AgenziaEntrateTracciati.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub btnPopolaTabAppoggio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPopolaTabAppoggio.Click
        Try
            Dim oListDatiAE() As WSOPENae.DisposizioneAE
            Dim FncTracciati As New ClsAETracciati

            If DdlAnno.SelectedValue <> "" Then
                'setto l'url del servizio
                FncWSOpenAE.Url = System.Configuration.ConfigurationManager.AppSettings("URLWSOPENae")
                Log.Debug("Richiamo il WEB SERVICE all'url:: " & FncWSOpenAE.Url)
                'prelevo i dati per popolare la tabella d'appoggio
                oListDatiAE = FncTracciati.GetDisposizioni(ConstSession.IdEnte, DdlAnno.SelectedValue, TxtDal.Text)
                Log.Debug("popolato oggetto da passare a ws")
                If Not oListDatiAE Is Nothing Then
                    'popolo la tabella d'appoggio
                    Log.Debug("richiamo PopolaTabAppoggioAE")
                    FncWSOpenAE.PopolaTabAppoggioAE(oListDatiAE)
                    Log.Debug("richiamo GetFlussiTracciatiAE")
                    'popolo il dataview con il risultato della ricerca richiamando la funzione GetListFlussiTracciati(sAnno, sCognome, sNome, sCodFiscale, sPIva, sUbicazione);
                    arrayobjFlussiAE = FncWSOpenAE.GetFlussiTracciatiAE("9000", ConstSession.IdEnte, WFErrore)
                    'se il dataview non è vuoto:
                    If Not arrayobjFlussiAE Is Nothing Then
                        oListTracciati = LoadListTracciati(arrayobjFlussiAE)
                        'assegno il dataview alla griglia;
                        GrdTracciatiAE.DataSource = oListTracciati
                        GrdTracciatiAE.DataBind()
                        sScript = "GestAlert('a', 'success', '', '', 'Popolamento effettuato con successo!'):"
                    Else
                        If WFErrore = "" Then
                            'visualizzo la label di non sono presenti record per i parametri;
                            LblMessage.Text = "La ricerca non ha prodotto risultati."
                        Else
                            Throw New Exception
                        End If
                        sScript = "GestAlert('a', 'danger', '', '', 'Errore durante il popolamento!');"
                    End If
                Else
                    sScript = "GestAlert('a', 'warning', '', '', 'Non sono presenti elementi da estrarre!');"
                End If
            Else
                sScript = "GestAlert('a', 'warning', '', '', 'Selezionare un\'anno!');"
            End If
            RegisterScript(sScript, Me.GetType())
            DivAttesa.Style.Add("display", "none")
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.AgenziaEntrateTracciati.btnPopolaTabAppoggio_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub btnEstraiTracciatoAE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEstraiTracciatoAE.Click
    '    Dim sFileEstratto, sNameFileEstratto As String
    '    Try
    '        If GrdTracciatiAE.SelectedIndex > -1 Then
    '            'setto l'url del servizio
    '            FncWSOpenAE.Url = System.Configuration.ConfigurationManager.AppSettings("URLWSOPENae")
    '            sFileEstratto = FncWSOpenAE.EstraiTracciatoAE("9000", GrdTracciatiAE.SelectedRow.Cells(0).Text, ConstSession.IdEnte, sNameFileEstratto)
    '            'popolo il dataview con il risultato della ricerca richiamando la funzione GetListFlussiTracciati(sAnno, sCognome, sNome, sCodFiscale, sPIva, sUbicazione);
    '            arrayobjFlussiAE = FncWSOpenAE.GetFlussiTracciatiAE("9000", ConstSession.IdEnte, WFErrore)
    '            'se il dataview non è vuoto:
    '            If Not arrayobjFlussiAE Is Nothing Then
    '                oListTracciati = LoadListTracciati(arrayobjFlussiAE)
    '                'assegno il dataview alla griglia;
    '                GrdTracciatiAE.DataSource = oListTracciati
    '                GrdTracciatiAE.DataBind()
    '            Else
    '                If WFErrore = "" Then
    '                    'visualizzo la label di non sono presenti record per i parametri;
    '                    LblMessage.Text = "La ricerca non ha prodotto risultati."
    '                Else
    '                    Throw New Exception
    '                End If
    '            End If
    '        Else
    '            sScript = "GestAlert('a', 'warning', '', '', 'Selezionare un record della griglia!')"
    '            RegisterScript(sScript, Me.GetType())
    '        End If
    '        DivAttesa.Style.Add("display", "none")
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.AgenziaEntrateTracciati.btnEstraiTracciatoAE_Click.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    '    If sFileEstratto <> "" Then
    '        Dim FncDownload As New Net.WebClient
    '        FncDownload.DownloadFile(sFileEstratto, ConfigurationManager.AppSettings("PATH_FILE_AE").ToString() + sNameFileEstratto)
    '        Response.ContentType = "*/*"
    '        Response.AppendHeader("content-disposition", "attachment; filename=" + sNameFileEstratto)
    '        Response.WriteFile(ConstSession.PathFileAE + sNameFileEstratto)
    '        Response.End()
    '        sScript = "GestAlert('a', 'warning', '', '', 'Estrazione effettuata con successo!')"
    '        RegisterScript(sScript, Me.GetType())
    '    Else
    '        sScript = "GestAlert('a', 'danger', '', '', 'Errore durante l\'estrazione!')"
    '        RegisterScript(sScript, Me.GetType())
    '    End If
    'End Sub
    Private Sub btnEstraiTracciatoAE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEstraiTracciatoAE.Click
        Dim sFileEstratto, sNameFileEstratto As String
        sFileEstratto = ""
        sNameFileEstratto = ""
        Try
            If DdlAnno.SelectedValue <> "" Then
                'setto l'url del servizio
                FncWSOpenAE.Url = System.Configuration.ConfigurationManager.AppSettings("URLWSOPENae")
                sFileEstratto = FncWSOpenAE.EstraiTracciatoAE("9000", DdlAnno.SelectedValue, ConstSession.IdEnte, sNameFileEstratto)
                'popolo il dataview con il risultato della ricerca richiamando la funzione GetListFlussiTracciati(sAnno, sCognome, sNome, sCodFiscale, sPIva, sUbicazione);
                arrayobjFlussiAE = FncWSOpenAE.GetFlussiTracciatiAE("9000", ConstSession.IdEnte, WFErrore)
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
                sScript = "DivAttesa.style.display='none';GestAlert('a', 'warning', '', '', 'Selezionare un\'anno!');"
                RegisterScript(sScript, Me.GetType())
            End If
            DivAttesa.Style.Add("display", "none")
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.AgenziaEntrateTracciati.btnEstraiTracciatoAE_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
        If sFileEstratto <> "" Then
            Dim FncDownload As New Net.WebClient
            FncDownload.DownloadFile(sFileEstratto, ConfigurationManager.AppSettings("PATH_FILE_AE").ToString() + sNameFileEstratto)
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
    Private Sub btnEstraiXMLAE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEstraiXMLAE.Click
        Dim sNameFileEstratto As String = ""
        Dim FncAE As New AESpesometro.ClsAESpesometro

        Try
            If DdlAnno.SelectedValue <> "" Then
                sNameFileEstratto = FncAE.ExctractSpesometro(ConstSession.IdEnte, DdlAnno.SelectedValue)
            Else
                sScript = "GestAlert('a', 'warning', '', '', 'Selezionare un\'anno!');"
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Tracciati.btnEstraiXMLAE_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
        If sNameFileEstratto <> "" Then
            Response.ContentType = "*/*"
            Response.AppendHeader("content-disposition", "attachment; filename=" + sNameFileEstratto)
            Response.WriteFile(ConstSession.PathFileAE + sNameFileEstratto)
            Response.End()
            sScript = "GestAlert('a', 'success', '', '', 'Estrazione effettuata con successo!');"
        Else
            If DdlAnno.SelectedValue <> "" Then
                sScript = "GestAlert('a', 'danger', '', '', 'Errore durante l\'estrazione!');"
            End If
        End If
        RegisterScript(sScript, Me.GetType)
        DivAttesa.Style.Add("display", "none")
    End Sub

    Private Function LoadListTracciati(ByVal oFlussi() As WSOPENae.objFlussoAE) As AgenziaEntrateDLL.AgenziaEntrate.objFlussoAE()
        Dim oListTracciati() As AgenziaEntrateDLL.AgenziaEntrate.objFlussoAE
        oListTracciati = Nothing
        Try
            Dim oMyTracciato As AgenziaEntrateDLL.AgenziaEntrate.objFlussoAE
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
            Log.Debug("Tracciati::LoadListTracciati::fine procedura")
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.AgenziaEntrateTracciati.LoadListTracciati.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
        Return oListTracciati
    End Function

#Region "Griglie"
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
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.AgenziaEntrateTracciati.GrdRowDataBound.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
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
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.AgenziaEntrateTracciati.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
End Class
