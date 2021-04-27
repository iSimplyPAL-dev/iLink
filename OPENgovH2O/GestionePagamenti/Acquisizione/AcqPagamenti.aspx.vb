Imports log4net
Imports OPENUtility
Imports RemotingInterfaceMotoreH2O.Motoreh2o.Oggetti

Public Class AcqPagamenti
    Inherits System.Web.UI.Page
    Private Shared Log As ILog = LogManager.GetLogger(GetType(AcqPagamenti))
    Delegate Sub delImporter(ByVal sEnteImport As String, ByVal sFileImport As String, ByVal sContoCorrente As String, ByVal sOperatore As String, ByVal nIDFlussoImport As Integer, ByVal WFSessione As CreateSessione)

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents LblPercorso As System.Web.UI.WebControls.Label
    Protected WithEvents LblTipoFile As System.Web.UI.WebControls.Label
    Protected WithEvents LblNomeFile As System.Web.UI.WebControls.Label
    Protected WithEvents LblTitoloEsito As System.Web.UI.WebControls.Label
    Protected WithEvents LblEsito As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents LblFileScarti As System.Web.UI.WebControls.Label
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents LblRcDaImp As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents LblImportiDaImp As System.Web.UI.WebControls.Label
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents LblRcAcq As System.Web.UI.WebControls.Label
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents LblImportiAcq As System.Web.UI.WebControls.Label
    Protected WithEvents Label7 As System.Web.UI.WebControls.Label
    Protected WithEvents LblRcScarti As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents LblImportiScarti As System.Web.UI.WebControls.Label
    Protected WithEvents CmdImporta As System.Web.UI.WebControls.Button
    Protected WithEvents CmdScarica As System.Web.UI.WebControls.Button
    Protected WithEvents fileUpload As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents DivAttesa As System.Web.UI.HtmlControls.HtmlGenericControl

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' La videata permette di importare i flussi di pagamenti scaricati dal sito di POSTE e di abbinare in automatico le disposizioni provenienti da bollettini TD896.<br/>
    ''' Le disposizioni provenienti da altri tipi di bollettino saranno scaricate su file di scarti per permetterne l'inserimento manuale.<br/><br/>
    ''' In ingresso alla il sistema controlla lo stato dell'ultima importazione.<br/>
    ''' In base allo stato visualizza i dati dell'importazione piuttosto che il pannello di attesa perchè l'acquisizione è ancora in corso.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[monicatarello]	07/04/2009	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            Dim nRecord As Integer = 0
            LblFileScarti.Attributes.Add("onclick", "CmdScarica.click()")

            'Se non sto ricaricando la pagina da postback:
            If Page.IsPostBack = False Then
                'prelevo l'id dell'ultima importazione fatta richiamando la funzione CheckImportazione();
                nRecord = CheckImportazione()
                'se l'id è diverso da 0:
                If nRecord <> 0 Then
                    'visualizzo il pannello di attesa richiamando la funzione java VisualizzaElaborazione();
                    Page.RegisterStartupScript("", "<script language='javascript'>VisualizzaElaborazione();</script>")
                Else
                    'prelevo l'ultima importazione fatta richiamando la funzione PrelevaImportazione;
                    PrelevaImportazione()
                    'nascondo il pannello di attesa richiamando la funzione java VisualizzaForm();
                    Page.RegisterStartupScript("", "<script language='javascript'>VisualizzaForm();</script>")
                End If
            End If
        Catch Err As Exception
            Log.Debug("Si è verificato un errore in AcqPagamenti::Page_Load::" & Err.Message)
            Log.Warn("Si è verificato un errore in AcqPagamenti::Page_Load::" & Err.Message)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Il pulsante CmdImporta posta il file alla cartella di sistema per l'acquisizione e lancia l'importazione asincrona.<br/>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[monicatarello]	07/04/2009	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub CmdImporta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdImporta.Click
        Dim WFSessione As CreateSessione
        Dim WFErrore As String = ""

        Try
            Dim sFileImport As String = ""
            Dim FunctionImport As New ClsPagamenti
            Dim oMyTotAcq As New objimportpagamenti
            Dim nFlussoImport As Integer
            Dim oClsContiCorrente As New OPENUtility.ClsContiCorrenti
            Dim oContoCorrente As OPENUtility.objContoCorrente

            'apro la connessione al db
            WFSessione = New CreateSessione(Session("PARAMETROENV"), Session("username"), Session("IDENTIFICATIVOAPPLICAZIONE"))
            If Not WFSessione.CreaSessione(Session("username"), WFErrore) Then
                Throw New Exception("CmdImporta_Click::" & "Errore durante l'apertura della sessione di WorkFlow")
                Exit Sub
            End If

            'prendo il file da importare
            sFileImport = fileUpload.PostedFile.FileName()
            If sFileImport <> "" Then
                Dim oMyFileInfo = New System.IO.FileInfo(sFileImport)
                'lo sposto nella cartella da_acquisire
                sFileImport = ConfigurationSettings.AppSettings("PATH_INACQUISIZIONE_PAG").ToString() + oMyFileInfo.Name
                fileUpload.PostedFile.SaveAs(sFileImport)
                'registro l'inizio acquisizione
                oMyTotAcq.IdEnte = Session("COD_ENTE")
                oMyTotAcq.sFileAcq = sFileImport
                oMyTotAcq.nStatoAcq = 1
                oMyTotAcq.sEsito = "Inizio Acquisizione"
                oMyTotAcq.tDataAcq = Now
                'se la registrazione sul db non  ha avuto successo
                nFlussoImport = FunctionImport.SetAcquisizione(oMyTotAcq, 0, WFSessione)
                If nFlussoImport <= 0 Then
                    'nascondo il pannello di attesa richiamando la funzione java VisualizzaForm()
                    Page.RegisterStartupScript("", "<script language='javascript'>VisualizzaForm();</script>")
                Else
                    'visualizzo il pannello di attesa richiamando la funzione java VisualizzaElaborazione()
                    Page.RegisterStartupScript("", "<script language='javascript'>VisualizzaElaborazione();</script>")
                    'prelevo il conto corrente di riferimento
                    oContoCorrente = oClsContiCorrente.GetContoCorrente(Session("COD_ENTE"), "0434", Session("username"))
                    'controllo se è presente il conto corrente
                    If oContoCorrente Is Nothing Then
                        'se non presente do messaggio d'errore
                        Page.RegisterStartupScript("msg", "<script language='javascript'>alert('Non è presente il Conto Corrente.\nNon si può procedere con l'importazione dei pagamenti!');</script>")
                    Else
                        'se presente richiamo l'acquisizione asincrona
                        Dim oMyImport As New ClsImportPagamenti
                        Dim oRemImport As New delImporter(AddressOf oMyImport.ImportPagamenti)
                        Dim oMyAsyncResult As IAsyncResult = oRemImport.BeginInvoke(Session("COD_ENTE"), sFileImport, oContoCorrente.ContoCorrente, Session("username"), nFlussoImport, WFSessione, Nothing, Nothing)
                    End If
                End If
            Else
                Page.RegisterStartupScript("msg", "<script language='javascript'>alert('Selezionare il file!');</script>")
            End If
        Catch Err As Exception
            Log.Debug("Si è verificato un errore in AcqPagamenti::CmdImporta_Click::" & Err.Message)
            Log.Warn("Si è verificato un errore in AcqPagamenti::CmdImporta_Click::" & Err.Message)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            WFSessione.Kill()
        End Try
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Il pulsante CmdScarica permette di scaricare il file di scarti prodotto al percorso desiderato.<br/>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[monicatarello]	07/04/2009	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub CmdScarica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdScarica.Click
        Response.ContentType = "*/*"
        Response.AppendHeader("content-disposition", "attachment; filename=" + LblFileScarti.Text)
        Response.WriteFile(ConfigurationSettings.AppSettings("PATH_OKACQUISIZIONE_PAG").ToString() + LblFileScarti.Text)
        Response.End()
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' La funzione preleva l'ultima elaborazione presenta a sistema e controlla se è terminata o se invece è ancora in corso.
    ''' </summary>
    ''' <returns>Integer</br>
    ''' Il metodo restituisce <c>1</c> se l'elaborazione è terminata; mentre restituisce <c>0</c> se l'elaborazione è ancora in corso.
    ''' </returns>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[monicatarello]	07/04/2009	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Function CheckImportazione() As Integer
        Dim WFSessione As CreateSessione
        Dim WFErrore As String
        Dim oMyTotAcq As New objimportpagamenti

        Try
            'apro la connessione al db
            WFSessione = New CreateSessione(Session("PARAMETROENV"), Session("username"), Session("IDENTIFICATIVOAPPLICAZIONE"))
            If Not WFSessione.CreaSessione(Session("username"), WFErrore) Then
                Throw New Exception("CheckImportazione::" & "Errore durante l'apertura della sessione di WorkFlow")
                Exit Function
            End If

            Dim FncImport As New ClsPagamenti
            'controllare se è in elaborazione 
            oMyTotAcq = FncImport.GetAcquisizione(Session("COD_ENTE"), 1, WFSessione)
            If Not oMyTotAcq Is Nothing Then
                If oMyTotAcq.Id <> -1 Then
                    Return 1
                Else
                    Return 0
                End If
                Return 1
            Else
                Return 0
            End If
        Catch Err As Exception
            Log.Debug("Si è verificato un errore in AcqPagamenti::CheckImportazione::" & Err.Message)
            Log.Warn("Si è verificato un errore in AcqPagamenti::CheckImportazione::" & Err.Message)
            Return -1
        Finally
            WFSessione.Kill()
        End Try
    End Function

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' La routine preleva il dettaglio dell'ultima elaborazione presente a sistema popolando l'oggetto <c>objimportpagamenti</c> che è utilizzato per visualizzare i dati a video.
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[monicatarello]	07/04/2009	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub PrelevaImportazione()
        Dim WFSessione As CreateSessione

        Try
            Dim WFErrore As String
            Dim FncImport As New ClsPagamenti
            Dim nIsFirstTime As Integer
            Dim nMaxId As Integer = -1
            Dim oMyTotAcq As New ObjImportPagamenti

            If Not Session("IsFirstTime") Is Nothing Then
                nIsFirstTime = -1
            Else
                nIsFirstTime = 0
            End If
            Session("IsFirstTime") = 1
            'apro la connessione al db
            WFSessione = New CreateSessione(Session("PARAMETROENV"), Session("username"), Session("IDENTIFICATIVOAPPLICAZIONE"))
            If Not WFSessione.CreaSessione(Session("username"), WFErrore) Then
                Throw New Exception("GetContribInDich::" & "Errore durante l'apertura della sessione di WorkFlow")
                Exit Sub
            End If

            'verifica se l'elaborazione è terminata
            nMaxId = FncImport.MaxIdImport(Session("COD_ENTE"), WFSessione)
            If Not IsDBNull(nMaxId) Then
                'prelevo i dati dalla tabella dei flussi
                oMyTotAcq = FncImport.GetAcquisizione(Session("COD_ENTE"), nIsFirstTime, WFSessione)
                If Not oMyTotAcq Is Nothing Then
                    If oMyTotAcq.Id = nMaxId And nMaxId <> -1 Then
                        'visualizzo il risultato a video
                        VisualEsito(oMyTotAcq)
                        Session("IsFirstTime") = Nothing
                    Else
                        'controlla se l'elaborazione è terminata con errori
                        nIsFirstTime = -1
                        oMyTotAcq = FncImport.GetAcquisizione(Session("COD_ENTE"), nIsFirstTime, WFSessione)
                        If Not oMyTotAcq Is Nothing And oMyTotAcq.Id <> -1 Then
                            'visualizzo il risultato a video
                            VisualEsito(oMyTotAcq)
                        Else
                            LblNomeFile.Text = "" : LblEsito.Text = "" : LblFileScarti.Text = ""
                            LblRcDaImp.Text = "" : LblRcAcq.Text = "" : LblRcScarti.Text = ""
                            LblImportiAcq.Text = "" : LblImportiDaImp.Text = "" : LblImportiScarti.Text = ""
                        End If
                    End If
                Else
                    'controlla se l'elaborazione è terminata con errori
                    nIsFirstTime = -1
                    oMyTotAcq = FncImport.GetAcquisizione(Session("COD_ENTE"), nIsFirstTime, WFSessione)
                    If Not oMyTotAcq Is Nothing And oMyTotAcq.Id <> -1 Then
                        'visualizzo il risultato a video
                        VisualEsito(oMyTotAcq)
                    Else
                        LblNomeFile.Text = "" : LblEsito.Text = "" : LblFileScarti.Text = ""
                        LblRcDaImp.Text = "" : LblRcAcq.Text = "" : LblRcScarti.Text = ""
                        LblImportiAcq.Text = "" : LblImportiDaImp.Text = "" : LblImportiScarti.Text = ""
                    End If
                End If
            End If
        Catch Err As Exception
            Log.Debug("Si è verificato un errore in AcqPagamenti::PrelevaImportazione::" & Err.Message)
            Log.Warn("Si è verificato un errore in AcqPagamenti::PrelevaImportazione::" & Err.Message)
        Finally
            WFSessione.Kill()
        End Try
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' La routine dispone i dati dell'oggetto in ingresso a video.
    ''' </summary>
    ''' <param name="oEsito">oggetto di tipo objimportpagamenti contiene i dati da presentare a video</param>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[monicatarello]	07/04/2009	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Private Sub VisualEsito(ByVal oEsito As objimportpagamenti)
        Try
            Session("IsFirstTime") = Nothing
            Dim oMyFileInfo = New System.IO.FileInfo(oEsito.sFileAcq)
            'visualizzo il risultato a video
            LblNomeFile.Text = oMyFileInfo.Name
            LblEsito.Text = oEsito.sEsito
            LblFileScarti.Text = oEsito.sFileScarti.Replace(ConfigurationSettings.AppSettings("PATH_OKACQUISIZIONE_PAG").ToString(), "")
            LblRcDaImp.Text = oEsito.nRcDaacquisire
            LblRcAcq.Text = oEsito.nRcAcquisiti
            LblRcScarti.Text = oEsito.nRcScarti
            LblImportiAcq.Text = oEsito.ImpAcquisiti
            LblImportiDaImp.Text = oEsito.ImpDaacquisire
            LblImportiScarti.Text = oEsito.ImpScarti
        Catch Err As Exception
            Log.Debug("Si è verificato un errore in AcqPagamenti::VisualEsito::" & Err.Message)
            Log.Warn("Si è verificato un errore in AcqPagamenti::VisualEsito::" & Err.Message)
        End Try
    End Sub
End Class
