Imports log4net

Partial Class AcquisizioneLetture
    Inherits BasePage

    Private Shared Log As ILog = LogManager.GetLogger(GetType(AcquisizioneLetture))
    Delegate Sub delImporterLetture(ByVal myStringConnection As String, ByVal Operatore As String, ByVal sEnteImport As String, ByVal sFileImport As String, ByVal sIdImport As Integer, ByVal idPeriodo As String)

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
        Try
            Dim paginacomandi As String
            Dim parametri As String
            Dim sScript As String = ""
            Dim nRecord As Integer = 0
            lblLinkFileScarto.Attributes.Add("onclick", "CmdScarica.click()")

            If Page.IsPostBack = False Then
                nRecord = CheckImportazione()
                If nRecord <> 0 Then
                    sScript = "VisualizzaElaborazione();"
                    RegisterScript(sScript, Me.GetType())

                    If IsNothing(Session("paginacomandi")) Then
                        Session("paginacomandi") = Request("paginacomandi")
                        paginacomandi = Session("paginacomandi")
                    Else
                        paginacomandi = Session("paginacomandi")
                    End If
                    parametri = "?title=Acquedotto - Acquisizione Letture&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo
                Else
                    PrelevaImportazione()
                    sScript = "VisualizzaForm();"
                    RegisterScript(sScript, Me.GetType())

                    If IsNothing(Session("paginacomandi")) Then
                        Session("paginacomandi") = Request("paginacomandi")
                        paginacomandi = Session("paginacomandi")
                    Else
                        paginacomandi = Session("paginacomandi")
                    End If
                    parametri = "?title=Acquedotto - Acquisizione Letture&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENutenze.AcquisizioneLetture.Page_Load.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub btnImporta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImporta.Click
        Dim nomeFile As String = ""
        Dim sFileImport As String = ""
        Dim oMyTotAcq As New ObjImportazione
        Dim idImport As Integer = 0
        Dim clsObjImport As New ObjImportazione
        Dim clsImportazione As New ClsImport
        Dim sScript As String = ""

        Try
            'prendo il file da importare
            sFileImport = fileUpload.PostedFile.FileName()
            If sFileImport <> "" Then
                Dim oMyFileInfo As New System.IO.FileInfo(sFileImport)
                'lo sposto nella cartella da_acquisire
                fileUpload.PostedFile.SaveAs(ConfigurationManager.AppSettings("PATH_ACQUISIZIONE_LETTURE").ToString() & oMyFileInfo.Name)
                sFileImport = ConfigurationManager.AppSettings("PATH_ACQUISIZIONE_LETTURE").ToString() & oMyFileInfo.Name

                '*** dati inizio acquisizione
                oMyTotAcq.IdEnte = ConstSession.IdEnte
                oMyTotAcq.sFileAcq = sFileImport
                oMyTotAcq.nStatoAcq = 1  '*** inizio acquisizione
                oMyTotAcq.sEsito = "Inizio Acquisizione"
                oMyTotAcq.tDataAcq = Now.Date
                oMyTotAcq.sProvenienza = "Letture"
                oMyTotAcq.sOperatore = Session("username")

                idImport = clsObjImport.SetAcquisizione(oMyTotAcq, 0)
                If idImport = 0 Then
                    sScript = "VisualizzaForm();"
                    RegisterScript(sScript, Me.GetType())
                Else
                    sScript = "VisualizzaElaborazione();"
                    RegisterScript(sScript, Me.GetType())
                End If

                '*** inizio acquisizione
                Dim importaLetture As New ClsImport
                Dim remotedel As New delImporterLetture(AddressOf importaLetture.AvviaImportazione)
                Dim remar As IAsyncResult = remotedel.BeginInvoke(ConstSession.StringConnection, ConstSession.UserName, ConstSession.IdEnte, sFileImport, idImport, ConstSession.IdPeriodo, Nothing, Nothing)
            Else
                sScript = "GestAlert('a', 'warning', '', '', 'Selezionare il file!');"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENutenze.AcquisizioneLetture.btnImporta_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub

    Public Function CheckImportazione() As Integer
        Dim oMyTotAcq As New ObjImportazione

        Try
            Dim FunctionImport As New ObjImportazione
            '***controllare se è in elaborazione 
            oMyTotAcq = FunctionImport.GetAcquisizione(1, ConstSession.IdEnte)
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

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENutenze.AcquisizioneLetture.CheckImportazione.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
            Return -1
        End Try
    End Function

    Public Sub PrelevaImportazione()
        Try
            Dim FunctionImport As New ObjImportazione
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
            nMaxId = FunctionImport.MaxIdImport(ConstSession.IdEnte)
            If nMaxId <> -1 Then
                'prelevo i dati dalla tabella dei flussi
                oMyTotAcq = FunctionImport.GetAcquisizione(nIsFirstTime, ConstSession.IdEnte)
                If Not oMyTotAcq Is Nothing Then
                    If oMyTotAcq.Id = nMaxId And nMaxId <> -1 Then
                        'visualizzo il risultato a video
                        VisualEsito(oMyTotAcq)
                        Session("IsFirstTime") = Nothing
                    Else
                        'controlla se l'elaborazione è terminata con errori
                        nIsFirstTime = -1
                        oMyTotAcq = FunctionImport.GetAcquisizione(nIsFirstTime, ConstSession.IdEnte)
                        If Not oMyTotAcq Is Nothing And oMyTotAcq.Id <> -1 Then
                            'visualizzo il risultato a video
                            VisualEsito(oMyTotAcq)
                        Else
                            lblNomeFile.Text = "" : lblEsito.Text = "" : lblLinkFileScarto.Text = ""
                            lblTotRecDaImport.Text = "" : lblImportoTotF.Text = "" : lblTotRecScartati.Text = ""
                        End If
                    End If
                Else
                    'controlla se l'elaborazione è terminata con errori
                    nIsFirstTime = -1
                    oMyTotAcq = FunctionImport.GetAcquisizione(nIsFirstTime, ConstSession.IdEnte)
                    If Not oMyTotAcq Is Nothing And oMyTotAcq.Id <> -1 Then
                        'visualizzo il risultato a video
                        VisualEsito(oMyTotAcq)
                    Else
                        lblNomeFile.Text = "" : lblEsito.Text = "" : lblLinkFileScarto.Text = ""
                        lblTotRecDaImport.Text = "" : lblImportoTotF.Text = "" : lblTotRecScartati.Text = ""
                    End If
                End If
            End If
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENutenze.AcquisizioneLetture.PrelevaImportazione.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub VisualEsito(ByVal oEsito As ObjImportazione)
        Try
            Session("IsFirstTime") = Nothing
            Dim oMyFileInfo As New System.IO.FileInfo(oEsito.sFileAcq)
            'visualizzo il risultato a video
            lblNomeFile.Text = oMyFileInfo.Name
            lblEsito.Text = oEsito.sEsito
            lblLinkFileScarto.Text = oEsito.sFileScarti.Replace(ConfigurationManager.AppSettings("PATH_SPOSTA_ACQUISIZIONE").ToString(), "")
            lblTotRecDaImport.Text = oEsito.nRcFile
            lblImportoTotF.Text = oEsito.nRcImport
            lblTotRecScartati.Text = oEsito.nRcScarti
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENutenze.AcquisizioneLetture.VisualEsito.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub CmdScarica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdScarica.Click
        Try
            Response.ContentType = "*/*"
            Response.AppendHeader("content-disposition", "attachment; filename=" + lblLinkFileScarto.Text)
            Response.WriteFile(ConfigurationManager.AppSettings("PATH_SPOSTA_ACQUISIZIONE").ToString() + lblLinkFileScarto.Text)
            Response.End()
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENutenze.AcquisizioneLetture.CmdScarica_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
