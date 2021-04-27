Imports RIBESElaborazioneDocumentiInterface.Stampa.oggetti
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports log4net

Partial Class ViewDocumentiElaborati
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ViewDocumentiElaborati))

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
    ''' Pagina di visualizzazione e scarico documenti elaborati
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    ''' <revisionHistory><revision date="16/03/2021">Aggiunta la possibilità di ristampa fattura</revision></revisionHistory>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim oMyRuolo As New ObjTotRuoloFatture
        Dim FncDoc As New ClsElaborazioneDocumenti
        Dim objDocumentiStampate() As GruppoURL
        Dim oArrayOggettoUrl() As oggettoURL
        Dim indiceoArrayOggettoUrl As Integer
        Dim indice, nDocElab, nDocDaElab As Integer 'nDoc 

        Try
            oArrayOggettoUrl = Nothing
            'prelevo il ruolo
            oMyRuolo = Session("oRuoloH2O")

            If Page.IsPostBack = False Then
                If Not IsNothing(oMyRuolo) Then
                    If oMyRuolo.IdFlusso > 0 Then
                        lblAnnoRuolo.Text = Year(oMyRuolo.tDataEmissioneFattura)
                        lblDataCartellazione.Text = oMyRuolo.tDataEmissioneFattura

                        FncDoc.GetNDoc(ConstSession.IdEnte, oMyRuolo.IdFlusso, nDocElab, nDocDaElab)
                        lblDocElaborati.Text = "DOCUMENTI GIA' ELABORATI:   " + nDocElab.ToString
                        lblDocDaElaborare.Text = "DOCUMENTI DA ELABORARE:  " + nDocDaElab.ToString
                        Session("nDocDaElaborare") = nDocDaElab
                        RegisterScript("$('#divRuolo').show();$('#divComandiera').hide();", Me.GetType)
                    Else
                        RegisterScript("$('#divRuolo').hide();$('#divComandiera').show();", Me.GetType)
                    End If
                Else
                    RegisterScript("$('#divRuolo').hide();$('#divComandiera').show();", Me.GetType)
                End If
                'carico l'elenco dei docimenti elaborati
                objDocumentiStampate = CType(Session("ELENCO_DOCUMENTI_STAMPATI"), GruppoURL())
                indiceoArrayOggettoUrl = 0
                If Not IsNothing(objDocumentiStampate) Then
                    For indice = 0 To objDocumentiStampate.Length - 1
                        ReDim Preserve oArrayOggettoUrl(indiceoArrayOggettoUrl)
                        oArrayOggettoUrl(indiceoArrayOggettoUrl) = objDocumentiStampate(indice).URLComplessivo
                        indiceoArrayOggettoUrl += 1
                    Next
                    ViewState.Add("DocumentiStampati_URLComplessivo", objDocumentiStampate)
                    GrdDocumenti.DataSource = oArrayOggettoUrl
                    GrdDocumenti.DataBind()
                Else
                    lblMessage.Text = "Errore durante l'elaborazione dei documenti!"
                End If
            Else
                objDocumentiStampate = CType(Session("ELENCO_DOCUMENTI_STAMPATI"), GruppoURL())
                indiceoArrayOggettoUrl = 0
                For indice = 0 To objDocumentiStampate.Length - 1
                    ReDim Preserve oArrayOggettoUrl(indiceoArrayOggettoUrl)
                    oArrayOggettoUrl(indiceoArrayOggettoUrl) = objDocumentiStampate(indice).URLComplessivo
                    indiceoArrayOggettoUrl += 1
                Next
                GrdDocumenti.DataSource = oArrayOggettoUrl
                GrdDocumenti.DataBind()
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ViewDocumentiElaborati.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    'Put user code to initialize the page here
    '    Dim oMyRuolo As New ObjTotRuoloFatture
    '    Dim FncDoc As New ClsElaborazioneDocumenti
    '    Dim objDocumentiStampate() As GruppoURL
    '    Dim oArrayOggettoUrl() As oggettoURL
    '    Dim indiceoArrayOggettoUrl As Integer
    '    Dim indice, nDocElab, nDocDaElab As Integer 'nDoc 

    '    Try
    '        oArrayOggettoUrl = Nothing
    '        'prelevo il ruolo
    '        oMyRuolo = Session("oRuoloH2O")

    '        If Page.IsPostBack = False Then
    '            lblAnnoRuolo.Text = Year(CType(Session("oRuoloH2O"), ObjTotRuoloFatture).tDataEmissioneFattura)
    '            lblDataCartellazione.Text = oMyRuolo.tDataEmissioneFattura

    '            FncDoc.GetNDoc(ConstSession.IdEnte, oMyRuolo.IdFlusso, nDocElab, nDocDaElab)
    '            lblDocElaborati.Text = "DOCUMENTI GIA' ELABORATI:   " + nDocElab.ToString
    '            lblDocDaElaborare.Text = "DOCUMENTI DA ELABORARE:  " + nDocDaElab.ToString
    '            Session("nDocDaElaborare") = nDocDaElab

    '            'carico l'elenco dei docimenti elaborati
    '            objDocumentiStampate = CType(Session("ELENCO_DOCUMENTI_STAMPATI"), GruppoURL())
    '            indiceoArrayOggettoUrl = 0
    '            If Not IsNothing(objDocumentiStampate) Then
    '                For indice = 0 To objDocumentiStampate.Length - 1
    '                    ReDim Preserve oArrayOggettoUrl(indiceoArrayOggettoUrl)
    '                    oArrayOggettoUrl(indiceoArrayOggettoUrl) = objDocumentiStampate(indice).URLComplessivo
    '                    indiceoArrayOggettoUrl += 1
    '                Next
    '                ViewState.Add("DocumentiStampati_URLComplessivo", objDocumentiStampate)
    '                GrdDocumenti.DataSource = oArrayOggettoUrl
    '                GrdDocumenti.DataBind()
    '            Else
    '                lblMessage.Text = "Errore durante l'elaborazione dei documenti!"
    '            End If
    '        Else
    '            objDocumentiStampate = CType(Session("ELENCO_DOCUMENTI_STAMPATI"), GruppoURL())
    '            indiceoArrayOggettoUrl = 0
    '            For indice = 0 To objDocumentiStampate.Length - 1
    '                ReDim Preserve oArrayOggettoUrl(indiceoArrayOggettoUrl)
    '                oArrayOggettoUrl(indiceoArrayOggettoUrl) = objDocumentiStampate(indice).URLComplessivo
    '                indiceoArrayOggettoUrl += 1
    '            Next
    '            GrdDocumenti.DataSource = oArrayOggettoUrl
    '            GrdDocumenti.DataBind()
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ViewDocumentiElaborati.Page_Load.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try

    '    'If Page.IsPostBack = False Then
    '    '    oDatiRuolo = Session("objSearch")
    '    '    lblAnnoRuolo.Text = oDatiRuolo.sAnno
    '    '    lblDataCartellazione.Text = oDatiRuolo.DataCartellazione.ToString("dd/MM/yyyy")
    '    '    lblTipoRuolo.Text = oDatiRuolo.sTiporuolo

    '    '    'carico il pannello dei totalizzatori
    '    '    oMyRuolo = CType(Session("oRuoloH2O"), ObjTotRuoloFatture)
    '    '    If oMyRuolo.tDataApprovazioneDOC <> DateTime.MinValue Then
    '    '        lblDataApprovazioneDoc.Text = "DATA APPROVAZIONE:" & oMyRuolo.tDataApprovazioneDOC.ToString("dd/MM/yyyy")
    '    '        oListDocumentiElaborati = FncDoc.GetTabGuidaComunico("TBLGUIDA_COMUNICO_STORICO", oMyRuolo.IdFlusso)
    '    '        If oListDocumentiElaborati.Length > 0 Then
    '    '            lblDocElaborati.Text = "DOCUMENTI GIA' ELABORATI:   " & oListDocumentiElaborati.Length.ToString
    '    '        Else
    '    '            lblDocElaborati.Text = "DOCUMENTI GIA' ELABORATI:   0"
    '    '        End If
    '    '    Else
    '    '        oListDocumentiElaborati = FncDoc.GetTabGuidaComunico("TBLGUIDA_COMUNICO", oMyRuolo.IdFlusso)
    '    '        If oListDocumentiElaborati.Length > 0 Then
    '    '            lblDocElaborati.Text = "DOCUMENTI GIA' ELABORATI:   " & oListDocumentiElaborati.Length.ToString
    '    '            Session("DocElaborati") = oListDocumentiElaborati.Length.ToString
    '    '        Else
    '    '            lblDocElaborati.Text = "DOCUMENTI GIA' ELABORATI:   0"
    '    '            Session("DocElaborati") = 0
    '    '        End If

    '    '        nDoc = FncDoc.GetNumFileDocDaElaborare(oMyRuolo.IdFlusso, ConstSession.IdEnte)
    '    '        If nDoc > 0 Then
    '    '            lblDocDaElaborare.Text = "DOCUMENTI DA ELABORARE:  " & nDoc
    '    '            Session("nDocDaElaborare") = nDoc
    '    '        Else
    '    '            lblDocDaElaborare.Text = "DOCUMENTI DA ELABORARE:  0"
    '    '            Session("nDocDaElaborare") = 0
    '    '        End If
    '    '    End If

    '    '    'carico l'elenco dei docimenti elaborati
    '    '    objDocumentiStampate = CType(Session("ELENCO_DOCUMENTI_STAMPATI"), GruppoURL())
    '    '    indiceoArrayOggettoUrl = 0
    '    '    If Not IsNothing(objDocumentiStampate) Then
    '    '        For indice = 0 To objDocumentiStampate.Length - 1
    '    '            ReDim Preserve oArrayOggettoUrl(indiceoArrayOggettoUrl)
    '    '            oArrayOggettoUrl(indiceoArrayOggettoUrl) = objDocumentiStampate(indice).URLComplessivo
    '    '            indiceoArrayOggettoUrl += 1
    '    '        Next
    '    '        ViewState.Add("DocumentiStampati_URLComplessivo", objDocumentiStampate)
    '    '        GrdDocumenti.DataSource = oArrayOggettoUrl
    '    '        GrdDocumenti.DataBind()
    '    '    Else
    '    '        lblMessage.Text = "Errore durante l'elaborazione dei documenti!"
    '    '    End If
    '    'Else
    '    '    GrdDocumenti.AllowCustomPaging = False
    '    '    GrdDocumenti.start_index = GrdDocumenti.CurrentPageIndex.ToString()
    '    '    objDocumentiStampate = CType(Session("ELENCO_DOCUMENTI_STAMPATI"), GruppoURL())
    '    '    indiceoArrayOggettoUrl = 0
    '    '    For indice = 0 To objDocumentiStampate.Length - 1
    '    '        ReDim Preserve oArrayOggettoUrl(indiceoArrayOggettoUrl)
    '    '        oArrayOggettoUrl(indiceoArrayOggettoUrl) = objDocumentiStampate(indice).URLComplessivo
    '    '        indiceoArrayOggettoUrl += 1
    '    '    Next
    '    '    GrdDocumenti.DataSource = oArrayOggettoUrl
    '    '    GrdDocumenti.DataBind()
    '    'End If
    'End Sub
End Class
