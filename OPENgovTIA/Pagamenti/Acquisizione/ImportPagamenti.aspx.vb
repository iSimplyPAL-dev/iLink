'*** 20120921 - pagamenti ***
Imports log4net
Imports OPENUtility
Imports RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti

''' <summary>
''' Pagina per l'importazione dei pagamenti.
''' Contiene i parametri di dettaglio, le funzioni della comandiera e i dati per la visualizzazione del risultato.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
''' <revisionHistory>
''' <revision date="26/11/2018">
''' <strong>Insoluti e Coattivo</strong>
''' In base alla data di pagamento il sistema abbinerà:
''' <list type="bullet">
'''   <item>se entro i termini sull'avviso, cancellandone la data di accertamento e cancellandone l’ingiunzione</item>
'''   <item>se oltre i termini sull'ingiunzione</item>
''' </list>
''' In pratica sarà tentato un primo abbinamento cercando in ordinario oltre che per IDOPERAZIONE o CODICE FISCALE/PARTITA IVA+ANNO ed IMPORTO anche per DATA filtrando tutti gli avvisi aventi termine di pagamento entro la data pagamento stessa, se non si riesce l'abbinamento sarà tentato un secondo abbinamento in provvedimenti filtrando tutte le ingiunzioni non “IN ATTESA”. Nel caso non si riuscisse l’abbinamento il pagamento sarà messo nei non abbinati. Nel caso di abbinamento al primo tentativo oltre la query di inserimento saranno fatte anche le query di aggiornamento e cancellazione sopra citate.
''' Nel caso di abbinamento al secondo tentativo sarà registrato L'IDFLUSSO nella tabella dei pagamenti degli accertamenti.
''' I termini di pagamento corrispondono a 60GG dalla data di notifica; nel caso mancasse si assume come data di notifica la data di pagamento.
''' </revision>
''' </revisionHistory>
Partial Class ImportPagamenti
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ImportPagamenti))
    Private WFErrore As String
    Private FncPag As New ClsGestPag
    Private oMyTotAcq As New ObjImportPagamenti
    Delegate Sub delImporter(ByVal sEnteImport As String, sTributo As String, ByVal sFileImport As String, ByVal sPathImportOK As String, ByVal sPathImportKO As String, ByVal sContoCorrente As String, ByVal sOperatore As String, ByVal nIDFlussoImport As Integer, ByVal myConnectionString As String, bTransitoPag As Boolean)
    Delegate Sub delImporterF24(ByVal IdEnte As String, ByVal Belfiore As String, ByVal PercorsoF24 As String, ByVal PercorsoDestF24 As String, ByVal nFlussoImport As Integer, ByVal MyFileName As String, ByVal CodTributo As String, ByVal UserName As String, ByVal StringConnectionICI As String, ByVal StringConnectionTARSU As String)
    Protected FncGrd As New Formatta.FunctionGrd
    Dim sScript As String

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
        Dim nRecord, nIsFirstTime As Integer
        Dim myStringConnection As String = ConstSession.StringConnection

        Try
            sScript = "$('#DivRiepilogoDaElab').show();"
            RegisterScript(sScript, Me.GetType)

            If ConstSession.IsFromVariabile(Request.Item("IsFromVariabile")) = "1" Then
            End If
            Dim IdTributo As String = Request.Item("TRIBUTO")
            If IdTributo = Utility.Costanti.TRIBUTO_OSAP Or IdTributo = Utility.Costanti.TRIBUTO_SCUOLE Then
                myStringConnection = ConstSession.StringConnectionOSAP
            End If
            Log.Debug("Page_Load::ConstSession.IsFromVariabile::" & ConstSession.IsFromVariabile)
            LblFileScarti.Attributes.Add("onclick", "CmdScarica.click()")

            If Not IsNothing(CacheManager.GetAvanzamentoElaborazione()) Then
                If (CacheManager.GetAvanzamentoElaborazione() <> "") Then
                    ShowCalcoloInCorso()
                End If
            End If
            'Se non sto ricaricando la pagina da postback:
            If Page.IsPostBack = False Then
                'prelevo l'id dell'ultima importazione fatta richiamando la funzione CheckImportazione();
                Log.Debug("CheckImportazione")
                nRecord = FncPag.CheckImportazione(ConstSession.IdEnte, myStringConnection)
                'se l'id è diverso da 0:
                If nRecord <> 0 Then
                    'visualizzo il pannello di attesa richiamando la funzione java VisualizzaElaborazione();
                    RegisterScript("VisualizzaElaborazione();", Me.GetType())
                Else
                    If Not Session("IsFirstTime") Is Nothing Then
                        nIsFirstTime = -1
                    Else
                        nIsFirstTime = 0
                    End If
                    Session("IsFirstTime") = 1
                    'prelevo l'ultima importazione fatta richiamando la funzione PrelevaImportazione;
                    oMyTotAcq = FncPag.PrelevaImportazione(ConstSession.IdEnte, nIsFirstTime, myStringConnection)
                    If Not oMyTotAcq Is Nothing Then
                        If oMyTotAcq.Id <> -1 Then
                            'visualizzo il risultato a video
                            VisualEsito(oMyTotAcq)
                        Else
                            LblNomeFile.Text = "" : LblEsito.Text = "" : LblFileScarti.Text = ""
                            LblRcDaImp.Text = "" : LblRcAcq.Text = "" : LblRcScarti.Text = ""
                            LblImportiAcq.Text = "" : LblImportiDaImp.Text = "" : LblImportiScarti.Text = ""
                        End If
                    Else
                        LblNomeFile.Text = "" : LblEsito.Text = "" : LblFileScarti.Text = ""
                        LblRcDaImp.Text = "" : LblRcAcq.Text = "" : LblRcScarti.Text = ""
                        LblImportiAcq.Text = "" : LblImportiDaImp.Text = "" : LblImportiScarti.Text = ""
                    End If
                    'nascondo il pannello di attesa richiamando la funzione java VisualizzaForm();
                    RegisterScript("VisualizzaForm();", Me.GetType())
                End If
                Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Pagamento, "Importazione", Utility.Costanti.AZIONE_LETTURA.ToString(), ConstSession.CodTributo, ConstSession.IdEnte, -1)
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ImportPagamenti.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub CmdScarica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdScarica.Click
        Response.ContentType = "*/*"
        Response.AppendHeader("content-disposition", "attachment; filename=" + LblFileScarti.Text)
        Response.WriteFile(ConstSession.PathPagamentiScarti + LblFileScarti.Text)
        Response.End()
    End Sub
    Private Sub CmdImporta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdImporta.Click
        Dim sFileImport As String = ""
        Dim oMyTotAcq As New ObjImportPagamenti
        Dim nFlussoImport As Integer
        Dim oClsContiCorrente As New OPENUtility.ClsContiCorrenti
        Dim oContoCorrente As OPENUtility.objContoCorrente
        Dim oMyImport As New ClsGestPag
        Dim myStringConnection As String = ConstSession.StringConnection
        Dim sTributoToImport As String = ConstSession.CodTributo

        Try
            Dim IdTributo As String = Request.Item("TRIBUTO")
            If IdTributo = Utility.Costanti.TRIBUTO_OSAP Or IdTributo = Utility.Costanti.TRIBUTO_SCUOLE Then
                myStringConnection = ConstSession.StringConnectionOSAP
                sTributoToImport = IdTributo
            End If

            If ConstSession.PathPagamentiInLavorazione = "" Or ConstSession.PathPagamentiOK = "" Or ConstSession.PathPagamentiScarti = "" Or ConstSession.PathF24 = "" Or ConstSession.PathF24Acquisiti = "" Then
                RegisterScript("GestAlert('a', 'warning', '', '', 'Percorsi di import non configurati!');", Me.GetType())
            Else
                'prendo il file da importare
                sFileImport = fileUpload.PostedFile.FileName()
                If sFileImport <> "" Then
                    Log.Debug("ho file da importare::" & sFileImport)
                    Dim oMyFileInfo As New System.IO.FileInfo(sFileImport)
                    'registro l'inizio acquisizione
                    oMyTotAcq.IdEnte = ConstSession.IdEnte
                    oMyTotAcq.sFileAcq = sFileImport
                    oMyTotAcq.nStatoAcq = 1
                    oMyTotAcq.sEsito = "Inizio Acquisizione"
                    oMyTotAcq.tDataAcq = Now
                    'se la registrazione sul db non  ha avuto successo
                    'Log.Debug("devo settare rc TBLIMPORTPAGAMENTI")
                    nFlussoImport = FncPag.SetAcquisizione(oMyTotAcq, 0, myStringConnection)
                    If nFlussoImport <= 0 Then
                        'nascondo il pannello di attesa richiamando la funzione java VisualizzaForm()
                        RegisterScript("VisualizzaForm();", Me.GetType())
                    Else
                        'visualizzo il pannello di attesa richiamando la funzione java VisualizzaElaborazione()
                        RegisterScript("VisualizzaElaborazione();", Me.GetType())
                        Select Case oMyFileInfo.Name.Substring(0, 3).ToUpper 'sFileImport.Substring(0, 3).ToUpper
                            Case "F24"
                                'lo sposto nella cartella da_acquisire
                                'Log.Debug("sposto il file al percorso::" & PercorsoF24 + oMyFileInfo.Name)
                                sFileImport = ConstSession.PathF24 + oMyFileInfo.Name
                                fileUpload.PostedFile.SaveAs(sFileImport)
                                ShowCalcoloInCorso()
                                Dim oRemImport As New delImporterF24(AddressOf oMyImport.ImportF24)
                                oRemImport.BeginInvoke(ConstSession.IdEnte, ConstSession.Belfiore, ConstSession.PathF24, ConstSession.PathF24Acquisiti, nFlussoImport, oMyFileInfo.Name, sTributoToImport, ConstSession.UserName, ConstSession.StringConnectionICI, myStringConnection, Nothing, Nothing)
                            Case Else
                                'lo sposto nella cartella da_acquisire
                                sFileImport = ConstSession.PathPagamentiInLavorazione + oMyFileInfo.Name
                                fileUpload.PostedFile.SaveAs(sFileImport)
                                'prelevo il conto corrente di riferimento
                                oContoCorrente = oClsContiCorrente.GetContoCorrente(ConstSession.IdEnte, sTributoToImport, ConstSession.UserName, ConstSession.StringConnectionOPENgov)
                                'controllo se è presente il conto corrente
                                If oContoCorrente Is Nothing Then
                                    'se non presente do messaggio d'errore
                                    RegisterScript("GestAlert('a', 'warning', '', '', 'Non è presente il Conto Corrente.\nNon si può procedere con l'importazione dei pagamenti!');", Me.GetType())
                                Else
                                    'se presente richiamo l'acquisizione asincrona
                                    ShowCalcoloInCorso()
                                    Dim oRemImport As New delImporter(AddressOf oMyImport.ImportPagamenti)
                                    oRemImport.BeginInvoke(ConstSession.IdEnte, sTributoToImport, sFileImport, ConstSession.PathPagamentiOK, ConstSession.PathPagamentiScarti, oContoCorrente.ContoCorrente, ConstSession.UserName, nFlussoImport, myStringConnection, ConstSession.bTransitoPag, Nothing, Nothing)
                                End If
                        End Select
                    End If
                Else
                    RegisterScript("GestAlert('a', 'warning', '', '', 'Selezionare il file!');", Me.GetType())
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ImportPagamenti.CmdImporta_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            'WFSessione.Kill()
        End Try
    End Sub
    Private Sub CmdOld_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdOld.Click
        Try
            GrdHistory.Visible = True
            GrdHistory.DataSource = FncPag.Getimportazioni(ConstSession.IdEnte, ConstSession.StringConnection)
            GrdHistory.DataBind()
            GrdHistory.SelectedIndex = -1
            sScript = "$('#DivRiepilogoDaElab').show();"
            RegisterScript(sScript, Me.GetType)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ImportPagamenti.CmdOld_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    Private Sub ShowCalcoloInCorso()
        DivAttesa.Style.Add("display", "")
        Response.AppendHeader("refresh", "60")
        LblAvanzamento.Text = CacheManager.GetAvanzamentoElaborazione()
    End Sub


    Private Sub VisualEsito(ByVal oEsito As ObjImportPagamenti)
        Try
            Session("IsFirstTime") = Nothing
            Dim oMyFileInfo As New System.IO.FileInfo(oEsito.sFileAcq)
            'visualizzo il risultato a video
            LblNomeFile.Text = oMyFileInfo.Name
            LblEsito.Text = oEsito.sEsito
            LblFileScarti.Text = oEsito.sFileScarti.Replace(ConstSession.PathPagamentiOK, "")
            LblRcDaImp.Text = oEsito.nRcDaAcquisire
            LblRcAcq.Text = oEsito.nRcAcquisiti
            LblRcScarti.Text = oEsito.nRcScarti
            LblImportiAcq.Text = oEsito.impAcquisiti
            LblImportiDaImp.Text = oEsito.impDaAcquisire
            LblImportiScarti.Text = oEsito.impScarti
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ImportPagamenti.VisualEsito.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class

'*** ***