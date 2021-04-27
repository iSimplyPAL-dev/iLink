Imports log4net

Partial Class ImportCMGC
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ImportCMGC))
    'Delegate Sub delImporterCMGC(ByVal WFSessione As CreateSessione, ByVal sEnteImport As String, ByVal sFileImport As String, ByVal nIdImport As Integer, ByVal sPeriodo As String, ByVal sOperatore As String, ByVal sISTAT As String, ByVal sEnteAppartenenza As String, ByVal nMyIDImpianto As Integer)
    Delegate Sub delImporterCMGC(ByVal sEnteImport As String, ByVal sFileImport As String, ByVal nIdImport As Integer, ByVal sPeriodo As String, ByVal sOperatore As String, ByVal sISTAT As String, ByVal sEnteAppartenenza As String, ByVal nMyIDImpianto As Integer)
    'Private WFSessione As CreateSessione
    'Private WFErrore As String = ""

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
            Dim sScript As String = ""
            Dim paginacomandi As String = "CmdImportCMGC.aspx"
            Dim parametri As String = "?title=Acquedotto - Acquisizione CMGC&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo
            Dim nRecord As Integer = 0
            LblLinkFileScarto.Attributes.Add("onclick", "CmdScarica.click()")

            If Page.IsPostBack = False Then
                nRecord = CheckImportazione()
                If nRecord <> 0 Then
                    sScript += "VisualizzaElaborazione();"
                    RegisterScript(sScript, Me.GetType())
                Else
                    PrelevaImportazione()
                    sScript += "VisualizzaForm();"
                    RegisterScript(sScript, Me.GetType())
                End If
                sScript = "document.getElementById('paginacomandi').value='" & paginacomandi & "';"
                sScript += "parent.Comandi.location.href='" & paginacomandi & parametri & "';"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ImportCMGC.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub CmdImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdImport.Click
        'Dim sNomeFile As String = ""
        'Dim sFileImport As String = ""
        'Dim oMyTotAcq As New ObjImportazione
        'Dim nIdImport As Integer = 0
        'Dim FncImport As New ObjImportazione
        'Dim sScript As String = ""

        'Try
        '    'prendo il file da importare
        '    sFileImport = FileUpload.PostedFile.FileName()
        '    If sFileImport <> "" Then
        '        Dim oMyFileInfo = New System.IO.FileInfo(sFileImport)
        '        'lo sposto nella cartella da_acquisire
        '        FileUpload.PostedFile.SaveAs(ConfigurationManager.AppSettings("PATH_ACQUISIZIONE_LETTURE").ToString() & oMyFileInfo.Name)
        '        sFileImport = ConfigurationManager.AppSettings("PATH_ACQUISIZIONE_LETTURE").ToString() & oMyFileInfo.Name

        '        '*** dati inizio acquisizione
        '        oMyTotAcq.IdEnte = ConstSession.IdEnte
        '        oMyTotAcq.sFileAcq = sFileImport
        '        oMyTotAcq.nStatoAcq = 1
        '        oMyTotAcq.sEsito = "Inizio Acquisizione"
        '        oMyTotAcq.tDataAcq = Now.Date
        '        oMyTotAcq.sProvenienza = "TXT CMGC"
        '        oMyTotAcq.sOperatore = Session("username")

        '        nIdImport = FncImport.SetAcquisizione(oMyTotAcq, 0)
        '        If nIdImport = 0 Then
        '            sScript = "VisualizzaForm();"
        '            RegisterScript(sScript, Me.GetType())
        '        Else
        '            sScript = "VisualizzaElaborazione();"
        '            RegisterScript(sScript, Me.GetType())
        '        End If

        '        '*** inizio acquisizione
        '        Dim FncImportCMGC As New ClsImport
        '        Dim delMyDelegate As New delImporterCMGC(AddressOf FncImportCMGC.StartImportCMGC)
        '        Dim AsyncRes As IAsyncResult = delMyDelegate.BeginInvoke(ConstSession.IdEnte, sFileImport, nIdImport, ConstSession.IdPeriodo, ConstSession.UserName, ConstSession.CodIstat, ConstSession.IdEnte, TxtIDImpianto.Text, Nothing, Nothing)

        '    Else
        '        sScript = "GestAlert('a', 'warning', '', '', 'Selezionare il file!');"
        '        RegisterScript(sScript, Me.GetType())
        '    End If
        'Catch Err As Exception

        '    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ImportCMGC.CmdImport_Click.errore: ", Err)
        '    Response.Redirect("../../PaginaErrore.aspx")
        'End Try
    End Sub

    'Private Sub CmdImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdImport.Click
    '    Dim sNomeFile As String = ""
    '    Dim sFileImport As String = ""
    '    Dim oMyTotAcq As New ObjImportazione
    '    Dim nIdImport As Integer = 0
    '    Dim FncImport As New ObjImportazione

    '    Try
    '        WFSessione = New CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        'apro la connessione al db
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("CmdImport_Click::" & "Errore durante l'apertura della sessione di WorkFlow")
    '            Exit Sub
    '        End If

    '        'prendo il file da importare
    '        sFileImport = FileUpload.PostedFile.FileName()
    '        If sFileImport <> "" Then
    '            Dim oMyFileInfo = New System.IO.FileInfo(sFileImport)
    '            'lo sposto nella cartella da_acquisire
    '            FileUpload.PostedFile.SaveAs(ConfigurationManager.AppSettings("PATH_ACQUISIZIONE_LETTURE").ToString() & oMyFileInfo.Name)
    '            sFileImport = ConfigurationManager.AppSettings("PATH_ACQUISIZIONE_LETTURE").ToString() & oMyFileInfo.Name

    '            '*** dati inizio acquisizione
    '            oMyTotAcq.IdEnte = ConstSession.IdEnte
    '            oMyTotAcq.sFileAcq = sFileImport
    '            oMyTotAcq.nStatoAcq = 1
    '            oMyTotAcq.sEsito = "Inizio Acquisizione"
    '            oMyTotAcq.tDataAcq = Now.Date
    '            oMyTotAcq.sProvenienza = "TXT CMGC"
    '            oMyTotAcq.sOperatore = Session("username")

    '            nIdImport = FncImport.SetAcquisizione(oMyTotAcq, 0)
    '            If nIdImport = 0 Then
    '                RegisterScript(sScript , Me.GetType())"", "VisualizzaForm();")
    '            Else
    '                RegisterScript(sScript , Me.GetType())"", "VisualizzaElaborazione();")
    '            End If

    '            '*** inizio acquisizione
    '            Dim FncImportCMGC As New ClsImport
    '            Dim delMyDelegate As New delImporterCMGC(AddressOf FncImportCMGC.StartImportCMGC)
    '            Dim AsyncRes As IAsyncResult = delMyDelegate.BeginInvoke(WFSessione, ConstSession.IdEnte, sFileImport, nIdImport, ConstSession.IdPeriodo, ConstSession.UserName, ConstSession.CodIstat, Session("CODCOMUNEENTE"), TxtIDImpianto.Text, Nothing, Nothing)

    '        Else
    '            RegisterScript(sScript , Me.GetType())"msg", "GestAlert('a', 'warning', '', '', 'Selezionare il file!');")
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ImportCMGC.CdmImport_Click.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub 

    'Public Function CheckImportazione() As Integer
    '    Dim oMyTotAcq As New ObjImportazione

    '    Try
    '        'apro la connessione al db
    '        WFSessione = New CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("CheckImportazione::" & "Errore durante l'apertura della sessione di WorkFlow")
    '            Exit Function
    '        End If

    '        Dim FncImport As New ObjImportazione
    '        'controllare se è in elaborazione 
    '        oMyTotAcq = FncImport.GetAcquisizione(WFSessione, 1, ConstSession.IdEnte, "TXT CMGC")
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
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ImportCMGC.CheckImportazione.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '        Return -1
    '    Finally
    '        WFSessione.Kill()
    '    End Try
    'End Function

    Public Function CheckImportazione() As Integer
        Dim oMyTotAcq As New ObjImportazione

        Try
            Dim FncImport As New ObjImportazione
            'controllare se è in elaborazione 
            oMyTotAcq = FncImport.GetAcquisizione(1, ConstSession.IdEnte, "TXT CMGC")
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
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ImportCMGC.CheckImportazione.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
            Return -1
        End Try
    End Function

    Public Sub PrelevaImportazione()
        Try
            Dim FncImport As New ObjImportazione
            Dim nIsFirstTime As Integer
            Dim nMaxId As Integer = -1
            Dim oMyTotAcq As New ObjImportazione

            If Not Session("IsFirstTime") Is Nothing Then
                nIsFirstTime = -1
            Else
                nIsFirstTime = 0
            End If
            Session("IsFirstTime") = 1

            'verifica se l'elaborazione è terminata
            nMaxId = FncImport.MaxIdImport(ConstSession.IdEnte)
            If nMaxId <> -1 Then
                'prelevo i dati dalla tabella dei flussi
                oMyTotAcq = FncImport.GetAcquisizione(nIsFirstTime, ConstSession.IdEnte, "TXT CMGC")
                If Not oMyTotAcq Is Nothing Then
                    If oMyTotAcq.Id = nMaxId And nMaxId <> -1 Then
                        'visualizzo il risultato a video
                        VisualEsito(oMyTotAcq)
                        Session("IsFirstTime") = Nothing
                    Else
                        'controlla se l'elaborazione è terminata con errori
                        nIsFirstTime = -1
                        oMyTotAcq = FncImport.GetAcquisizione(nIsFirstTime, ConstSession.IdEnte, "TXT CMGC")
                        If Not oMyTotAcq Is Nothing And oMyTotAcq.Id <> -1 Then
                            'visualizzo il risultato a video
                            VisualEsito(oMyTotAcq)
                        Else
                            LblNomeFile.Text = "" : LblEsito.Text = "" : LblLinkFileScarto.Text = ""
                            LblTotRecDaImport.Text = "" : LblImportoTotF.Text = "" : LblTotRecScartati.Text = ""
                        End If
                    End If
                Else
                    'controlla se l'elaborazione è terminata con errori
                    nIsFirstTime = -1
                    oMyTotAcq = FncImport.GetAcquisizione(nIsFirstTime, ConstSession.IdEnte, "TXT CMGC")
                    If Not oMyTotAcq Is Nothing And oMyTotAcq.Id <> -1 Then
                        'visualizzo il risultato a video
                        VisualEsito(oMyTotAcq)
                    Else
                        LblNomeFile.Text = "" : LblEsito.Text = "" : LblLinkFileScarto.Text = ""
                        LblTotRecDaImport.Text = "" : LblImportoTotF.Text = "" : LblTotRecScartati.Text = ""
                    End If
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ImportCMGC.PrelevaImportazione.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub VisualEsito(ByVal oEsito As ObjImportazione)
        Try
            Session("IsFirstTime") = Nothing
            Dim oMyFileInfo As New System.IO.FileInfo(oEsito.sFileAcq)
            'visualizzo il risultato a video
            LblNomeFile.Text = oMyFileInfo.Name
            LblEsito.Text = oEsito.sEsito
            LblLinkFileScarto.Text = oEsito.sFileScarti.Replace(ConfigurationManager.AppSettings("PATH_SPOSTA_ACQUISIZIONE").ToString(), "")
            LblTotRecDaImport.Text = oEsito.nRcFile
            LblImportoTotF.Text = oEsito.nRcImport
            LblTotRecScartati.Text = oEsito.nRcScarti
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ImportCMGC.VisualEsito.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub CmdForzaLetture_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdForzaLetture.Click
        Dim FncImport As New ClsImport
        Dim sScript As String = ""

        Try
            If FncImport.GeneraLetPrecFromAtt(ConstSession.IdPeriodo) < 1 Then
                sScript = "GestAlert('a', 'danger', '', '', 'Errore in Forzatura Letture');"
                RegisterScript(sScript, Me.GetType())
            Else
                sScript = "GestAlert('a', 'success', '', '', 'Elaborazione terminata correttamente!');"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ImportCMGC.CmdForzaLetture_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
