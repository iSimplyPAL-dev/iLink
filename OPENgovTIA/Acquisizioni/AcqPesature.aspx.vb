Imports log4net
Imports OPENUtility
''' <summary>
''' Pagina per l'importazione dei flussi delle pesature
'''  Per una corretta importazione, si ricorda che il file deve:
''' - avere il nome che inizia con [codice_ente]_[mese di riferimento]_[anno di riferimento]
''' - essere In formato CSV
''' - avere dimensione massima 4MB (4095KB)
''' - avere come separatore il carattere ; (punto e virgola)
''' - avere come campi, nell'ordine:
'''     CODICE TESSERA
'''     TIPO CONFERIMENTO
'''     DATA ORA
'''     LITRI
'''     NOTE
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class AcqPesature
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(AcqPesature))
    Delegate Sub delImporter(DBType As String, ByVal myConnectionString As String, ByVal sUserEnv As String, ByVal sEnteImport As String, ByVal sFileImport As String, ByVal nIdFlussoImport As Integer)

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

    'Delegate Sub delImporter(ByVal sParamEnv As String, ByVal sUserEnv As String, ByVal sIdApplicativoEnv As String, ByVal sEnteImport As String, ByVal sFileImport As String, ByVal nIdFlussoImport As Integer)
    'Public Function CheckImportazione() As Integer
    '    Dim WFSessione As CreateSessione
    '    Dim WFErrore As String = ""
    '    Dim oMyTotAcq As New ObjTotAcquisizione

    '    Try
    '        'apro la connessione al db
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("CheckImportazione::" & "Errore durante l'apertura della sessione di WorkFlow")
    '            Exit Function
    '        End If

    '        Dim FunctionImport As New ObjTotAcquisizione
    '        '***controllare se è in elaborazione 
    '        oMyTotAcq = FunctionImport.GetAcquisizione(WFSessione, 1, ConstSession.IdEnte)
    '        If Not oMyTotAcq Is Nothing Then
    '            If oMyTotAcq.Id <> -1 Then
    '                Return 1
    '            Else
    '                Return 0
    '            End If
    '            Return 1
    '        Else
    '            Return 0
    '        End If
    '    Catch Err As Exception
    '           Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.AcqPesature.Page_Load.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '
    '        Return -1
    '    Finally
    '        If Not WFSessione Is Nothing Then
    '            WFSessione.Kill()
    '        End If
    '    End Try
    'End Function

    'Public Function PrelevaImportazione()
    '    Dim WFSessione As CreateSessione

    '    Try
    '        Dim WFErrore As String = ""
    '        Dim FunctionImport As New ObjTotAcquisizione
    '        Dim nIsFirstTime As Integer
    '        Dim nMaxId As Integer = -1
    '        Dim oMyTotAcq As New ObjTotAcquisizione

    '        If Not Session("IsFirstTime") Is Nothing Then
    '            nIsFirstTime = -1
    '        Else
    '            nIsFirstTime = 0
    '        End If
    '        Session("IsFirstTime") = 1
    '        'apro la connessione al db
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("GetContribInDich::" & "Errore durante l'apertura della sessione di WorkFlow")
    '            Exit Function
    '        End If

    '        'verifica se l'elaborazione è terminata
    '        nMaxId = FunctionImport.MaxIdImport(ConstSession.IdEnte, WFSessione)
    '        If Not IsDBNull(nMaxId) Then
    '            'prelevo i dati dalla tabella dei flussi
    '            oMyTotAcq = FunctionImport.GetAcquisizione(WFSessione, nIsFirstTime, ConstSession.IdEnte)
    '            If Not oMyTotAcq Is Nothing Then
    '                If oMyTotAcq.Id = nMaxId And nMaxId <> -1 Then
    '                    'visualizzo il risultato a video
    '                    VisualEsito(oMyTotAcq)
    '                    Session("IsFirstTime") = Nothing
    '                Else
    '                    'controlla se l'elaborazione è terminata con errori
    '                    nIsFirstTime = -1
    '                    oMyTotAcq = FunctionImport.GetAcquisizione(WFSessione, nIsFirstTime, ConstSession.IdEnte)
    '                    If Not oMyTotAcq Is Nothing And oMyTotAcq.Id <> -1 Then
    '                        'visualizzo il risultato a video
    '                        VisualEsito(oMyTotAcq)
    '                    Else
    '                        LblNomeFile.Text = "" : LblEsito.Text = "" : LblFileScarti.Text = ""
    '                        LblTessereDaImp.Text = "" : LblTessereImport.Text = "" : LblKGImport.Text = "" : LblRcImport.Text = ""
    '                    End If
    '                End If
    '            Else
    '                'controlla se l'elaborazione è terminata con errori
    '                nIsFirstTime = -1
    '                oMyTotAcq = FunctionImport.GetAcquisizione(WFSessione, nIsFirstTime, ConstSession.IdEnte)
    '                If Not oMyTotAcq Is Nothing And oMyTotAcq.Id <> -1 Then
    '                    'visualizzo il risultato a video
    '                    VisualEsito(oMyTotAcq)
    '                Else
    '                    LblNomeFile.Text = "" : LblEsito.Text = "" : LblFileScarti.Text = ""
    '                    LblTessereDaImp.Text = "" : LblTessereImport.Text = "" : LblKGImport.Text = "" : LblRcImport.Text = ""
    '                End If
    '            End If
    '        End If
    '    Catch Err As Exception
    '           Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.AcqPesature.PrelevaImportazione.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    Finally
    '        If Not WFSessione Is Nothing Then
    '            WFSessione.Kill()
    '        End If
    '    End Try
    'End Function

    'Private Sub CmdImporta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdImporta.Click
    '    Dim WFSessione As CreateSessione
    '    Dim WFErrore As String =""

    '    Try
    '        Dim sFileImport As String = ""
    '        Dim FunctionImport As New ObjTotAcquisizione
    '        Dim oMyTotAcq As New ObjTotAcquisizione
    '        Dim nFlussoImport As Integer

    '        'apro la connessione al db
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("CmdImporta_Click::" & "Errore durante l'apertura della sessione di WorkFlow")
    '            Exit Sub
    '        End If

    '        'prendo il file da importare
    '        sFileImport = fileUpload.PostedFile.FileName()
    '        If sFileImport <> "" Then
    '            Dim oMyFileInfo = New System.IO.FileInfo(sFileImport)
    '            'lo sposto nella cartella da_acquisire
    '            fileUpload.PostedFile.SaveAs(ConfigurationManager.AppSettings("PATH_ACQUISIZIONE").ToString() & CStr(oMyFileInfo.Name).Replace(oMyFileInfo.extension, "") & "_" & Format(Now, "yyyyMMdd_hhmmss") & oMyFileInfo.extension)
    '            sFileImport = ConfigurationManager.AppSettings("PATH_ACQUISIZIONE").ToString() & CStr(oMyFileInfo.Name).Replace(oMyFileInfo.extension, "") & "_" & Format(Now, "yyyyMMdd_hhmmss") & oMyFileInfo.extension
    '            'registro l'inizio acquisizione
    '            oMyTotAcq.IdEnte = ConstSession.IdEnte
    '            oMyTotAcq.sFileAcq = sFileImport
    '            oMyTotAcq.nStatoAcq = 1
    '            oMyTotAcq.sEsito = "Inizio Acquisizione"
    '            oMyTotAcq.tDataAcq = Now
    '            nFlussoImport = FunctionImport.SetAcquisizione(oMyTotAcq, 0, WFSessione)
    '            If nFlussoImport = 0 Then
    '                RegisterScript(me.gettype(),"", "<script language='javascript'>VisualizzaForm();</script>")
    '            Else
    '                RegisterScript(me.gettype(),"", "<script language='javascript'>VisualizzaElaborazione();</script>")
    '            End If
    '            'richiamo l'acquisizione
    '            Dim oMyImport As New ClsImport
    '            Dim oRemImport As New delImporter(AddressOf oMyImport.StartImport)
    '            Dim oMyAsyncResult As IAsyncResult = oRemImport.BeginInvoke(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione, ConstSession.IdEnte, sFileImport, nFlussoImport, Nothing, Nothing)

    '        Else
    '            RegisterScript(me.gettype(),"msg", "<script language='javascript'>alert('Selezionare il file!');</script>")
    '        End If
    '    Catch Err As Exception
    '           Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.AcqPesature.CmdImporta_Click.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    Finally
    '        If Not WFSessione Is Nothing Then
    '            WFSessione.Kill()
    '        End If
    '    End Try
    'End Sub
    ''' <summary>
    ''' Al caricamento della pagina controlla se ci sono elaborazione in corso; se non presenti visualizza il riepilogo dell'ultima importazione fatta
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            Dim nRecord As Integer = 0

            If ConstSession.IsFromVariabile(Request.Item("IsFromVariabile")) = "1" Then
            End If

            LblFileScarti.Attributes.Add("onclick", "CmdScarica.click()")
            lblTitolo.Text = ConstSession.DescrizioneEnte
            If ConstSession.IsFromTARES = "1" Then
                info.InnerText = "TARI "
            Else
                info.InnerText = "TARSU "
            End If
            If ConstSession.IsFromVariabile(Request.Item("IsFromVariabile")) = "1" Then
                info.InnerText += "Variabile"
            End If
            info.InnerText += " - Conferimenti - Acquisizione"
            If Page.IsPostBack = False Then
                nRecord = CheckImportazione()
                If nRecord <> 0 Then
                    RegisterScript("VisualizzaElaborazione();", Me.GetType())
                Else
                    PrelevaImportazione()
                    RegisterScript("VisualizzaForm();", Me.GetType())
                End If
                Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, "Pesature", "Importazione", Utility.Costanti.AZIONE_LETTURA.ToString, ConstSession.CodTributo, ConstSession.IdEnte, -1)
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.AcqPesature.Page_Load.errore:  ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Carica nella pagina il riepilogo dell'importazione.
    ''' </summary>
    ''' <param name="oEsito"></param>
    Private Sub VisualEsito(ByVal oEsito As ObjTotAcquisizione)
        Try
            Session("IsFirstTime") = Nothing
            Dim oMyFileInfo As New System.IO.FileInfo(oEsito.sFileAcq)
            'visualizzo il risultato a video
            LblNomeFile.Text = oMyFileInfo.Name
            LblEsito.Text = oEsito.sEsito
            LblFileScarti.Text = oEsito.sFileScarti.Replace(ConstSession.PathSpostaImport, "")
            LblRcDaImp.Text = oEsito.nRcFile
            LblTessereDaImp.Text = oEsito.nTessereFile
            LblTessereImport.Text = oEsito.nTessereImport
            LblConfImport.Text = oEsito.nConferimentiImport
            LblLitriImport.Text = FormatNumber(oEsito.nLitriImport, 2)
            LblRcImport.Text = oEsito.nRcImport
            LblRcScarti.Text = oEsito.nRcScarti
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.AcqPesature.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Pulsante per lo scarico del file degli scarti.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdScarica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdScarica.Click
        Response.ContentType = "*/*"
        Response.AppendHeader("content-disposition", "attachment; filename=" + LblFileScarti.Text)
        Response.WriteFile(ConstSession.PathSpostaImport + LblFileScarti.Text)
        Response.End()
    End Sub
    ''' <summary>
    ''' Funzione che controlla se ci sono importazioni in corso
    ''' </summary>
    ''' <returns></returns>
    Public Function CheckImportazione() As Integer
        Dim oMyTotAcq As New ObjTotAcquisizione

        Try
            Dim FunctionImport As New ObjTotAcquisizione
            '***controllare se è in elaborazione 
            oMyTotAcq = FunctionImport.GetAcquisizione(ConstSession.StringConnection, 1, ConstSession.IdEnte)
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
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.AcqPesature.CheckImportazione.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
            Return -1
        End Try
    End Function
    ''' <summary>
    ''' Preleva i dati dell'ultima importazione eseguita
    ''' </summary>
    Public Sub PrelevaImportazione()
        Try
            Dim FunctionImport As New ObjTotAcquisizione
            Dim nIsFirstTime As Integer
            Dim nMaxId As Integer = -1
            Dim oMyTotAcq As New ObjTotAcquisizione

            If Not Session("IsFirstTime") Is Nothing Then
                nIsFirstTime = -1
            Else
                nIsFirstTime = 0
            End If
            Session("IsFirstTime") = 1
            'verifica se l'elaborazione è terminata
            nMaxId = FunctionImport.MaxIdImport(ConstSession.StringConnection, ConstSession.IdEnte)
            If Not IsDBNull(nMaxId) Then
                'prelevo i dati dalla tabella dei flussi
                oMyTotAcq = FunctionImport.GetAcquisizione(ConstSession.StringConnection, nIsFirstTime, ConstSession.IdEnte)
                If Not oMyTotAcq Is Nothing Then
                    If oMyTotAcq.Id = nMaxId And nMaxId <> -1 Then
                        'visualizzo il risultato a video
                        VisualEsito(oMyTotAcq)
                        Session("IsFirstTime") = Nothing
                    Else
                        'controlla se l'elaborazione è terminata con errori
                        nIsFirstTime = -1
                        oMyTotAcq = FunctionImport.GetAcquisizione(ConstSession.StringConnection, nIsFirstTime, ConstSession.IdEnte)
                        If Not oMyTotAcq Is Nothing And oMyTotAcq.Id <> -1 Then
                            'visualizzo il risultato a video
                            VisualEsito(oMyTotAcq)
                        Else
                            LblNomeFile.Text = "" : LblEsito.Text = "" : LblFileScarti.Text = ""
                            LblTessereDaImp.Text = "" : LblTessereImport.Text = "" : LblConfImport.Text = "" : LblLitriImport.Text = "" : LblRcImport.Text = ""
                        End If
                    End If
                Else
                    'controlla se l'elaborazione è terminata con errori
                    nIsFirstTime = -1
                    oMyTotAcq = FunctionImport.GetAcquisizione(ConstSession.StringConnection, nIsFirstTime, ConstSession.IdEnte)
                    If Not oMyTotAcq Is Nothing And oMyTotAcq.Id <> -1 Then
                        'visualizzo il risultato a video
                        VisualEsito(oMyTotAcq)
                    Else
                        LblNomeFile.Text = "" : LblEsito.Text = "" : LblFileScarti.Text = ""
                        LblTessereDaImp.Text = "" : LblTessereImport.Text = "" : LblConfImport.Text = "" : LblLitriImport.Text = "" : LblRcImport.Text = ""
                    End If
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.AcqPesature.PrelevaImportazione.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Pulsante per avviare l'importazione di un flusso.
    ''' Richiama in modo asincrono ClsImport.StartImport 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdImporta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdImporta.Click
        Try
            Dim sFileImport As String = ""
            Dim FunctionImport As New ObjTotAcquisizione
            Dim oMyTotAcq As New ObjTotAcquisizione
            Dim nFlussoImport As Integer

            'prendo il file da importare
            sFileImport = fileUpload.PostedFile.FileName()
            If sFileImport <> "" Then
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.AcqPesature.CmdImporta_Click.devo importare->" + sFileImport)
                Dim oMyFileInfo As New System.IO.FileInfo(sFileImport)
                'lo sposto nella cartella da_acquisire
                fileUpload.PostedFile.SaveAs(ConstSession.PathImport & CStr(oMyFileInfo.Name).Replace(oMyFileInfo.Extension, "") & "_" & Format(Now, "yyyyMMdd_hhmmss") & oMyFileInfo.Extension)
                sFileImport = ConstSession.PathImport & CStr(oMyFileInfo.Name).Replace(oMyFileInfo.Extension, "") & "_" & Format(Now, "yyyyMMdd_hhmmss") & oMyFileInfo.Extension
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.AcqPesature.CmdImporta_Click.file da importare->" + sFileImport)
                'registro l'inizio acquisizione
                oMyTotAcq.IdEnte = ConstSession.IdEnte
                oMyTotAcq.sFileAcq = sFileImport
                oMyTotAcq.nStatoAcq = 1
                oMyTotAcq.sEsito = "Inizio Acquisizione"
                oMyTotAcq.tDataAcq = Now
                nFlussoImport = FunctionImport.SetAcquisizione(ConstSession.StringConnection, oMyTotAcq, 0)
                If nFlussoImport = 0 Then
                    RegisterScript("VisualizzaForm();", Me.GetType())
                Else
                    RegisterScript("VisualizzaElaborazione();", Me.GetType())
                End If
                'richiamo l'acquisizione
                Dim oMyImport As New ClsImport
                Dim oRemImport As New delImporter(AddressOf oMyImport.StartImport)
                oRemImport.BeginInvoke(ConstSession.DBType, ConstSession.StringConnection, ConstSession.UserName, ConstSession.IdEnte, sFileImport, nFlussoImport, Nothing, Nothing)
            Else
                RegisterScript("GestAlert('a', 'warning', '', '', 'Selezionare il file!');", Me.GetType())
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.AcqPesature.CmdImporta_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
