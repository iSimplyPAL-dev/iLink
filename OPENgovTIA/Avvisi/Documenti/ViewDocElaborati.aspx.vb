Imports log4net
Imports RIBESElaborazioneDocumentiInterface.Stampa.oggetti
Imports RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
Imports System.Net
Imports System.Security
'Imports System.IO.IOException
Imports System.IO
'Imports ICSharpCode.SharpZipLib.Core
''' <summary>
''' Pagina per il download dei documenti elaborati
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class ViewDocElaborati
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ViewDocElaborati))

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
        Dim oMyRuolo() As ObjRuolo
        Dim FncDoc As New ClsGestDocumenti
        'Dim oArrayOggettoDocumentiElaborati() As OggettoDocumentiElaborati
        'Dim oArrayDocDaElaborare() As OggettoDocumentiElaborati

        Dim objDocumentiStampate() As GruppoURL
        Dim oArrayOggettoUrl() As oggettoURL = Nothing
        Dim indiceoArrayOggettoUrl As Integer
        Dim indice, nDocElab, nDocDaElab As Integer

        Session("Provenienza") = 0

        Dim cmdMyCommand As SqlClient.SqlCommand = Nothing

        Try
            cmdMyCommand = New SqlClient.SqlCommand
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            
            TxtProvenienzaForm.Text = "DOC"
            TxtProvenienzaForm.Text = Request.Item("ProvenienzaForm")
            If Page.IsPostBack = False Then
                oMyRuolo = Session("oRuoloTIA")
                If Not IsNothing(oMyRuolo) Then
                    FncDoc.GetNDoc(ConstSession.IdEnte, oMyRuolo(0).IdFlusso, nDocElab, nDocDaElab, cmdMyCommand)
                    'lblDocElaborati.Text = "DOCUMENTI GIA' ELABORATI:   " + nDocElab.ToString
                    'lblDocDaElaborare.Text = "DOCUMENTI DA ELABORARE:  " + nDocDaElab.ToString

                    'lblTipoRuolo.Text = oMyRuolo(0).sDescrTipoRuolo
                    'lblAnnoRuolo.Text = oMyRuolo(0).sAnno
                    'lblDataCartellazione.Text = oMyRuolo(0).tDataCartellazione
                Else
                    'DivRuolo.Style.Add("display", "none")
                    lblDownloadAll.Style.Add("display", "none")
                    'ReDim Preserve objDocumentiStampate(0)
                    'objDocumentiStampate(0) = CType(Session("ELENCO_DOCUMENTI_STAMPATI"), GruppoURL)
                    'Session("ELENCO_DOCUMENTI_STAMPATI") = objDocumentiStampate
                End If
                objDocumentiStampate = CType(Session("ELENCO_DOCUMENTI_STAMPATI"), GruppoURL())
                indiceoArrayOggettoUrl = 0
                If Not objDocumentiStampate Is Nothing Then
                    Log.Debug("carico")
                    For indice = 0 To objDocumentiStampate.Length - 1
                        If Not IsNothing(objDocumentiStampate(indice)) Then
                            If Not objDocumentiStampate(indice).URLComplessivo Is Nothing Then
                                ReDim Preserve oArrayOggettoUrl(indiceoArrayOggettoUrl)
                                oArrayOggettoUrl(indiceoArrayOggettoUrl) = objDocumentiStampate(indice).URLComplessivo
                                indiceoArrayOggettoUrl += 1
                            Else
                                Log.Debug("finta di caricare")
                            End If
                        Else
                            Log.Debug("ho fatto finta di caricare")
                        End If
                    Next
                    If Not oArrayOggettoUrl Is Nothing Then
                        ViewState.Add("DocumentiStampati_URLComplessivo", objDocumentiStampate)
                        GrdDocumenti.DataSource = oArrayOggettoUrl
                        GrdDocumenti.DataBind()
                    Else
                        Log.Debug("non ho caricato nulla")
                    End If
                Else
                    Log.Debug("nullo")
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ViewDocElaborati.Page_Load.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        Finally
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Sub

    ''' <summary>
    ''' Pulsante per il download di tutti i documenti prodotti.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnDownloadAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDownloadAll.Click
        Dim ListDocumenti() As GruppoURL
        Dim oMyRuolo() As ObjRuolo
        Dim FileNameZip As String
        Dim strmZipOutputStream As ICSharpCode.SharpZipLib.Zip.ZipOutputStream = Nothing

        Try
            oMyRuolo = Session("oRuoloTIA")
            FileNameZip = ConstSession.IdEnte + "_DOCUMENTIELABORATI_" + oMyRuolo(0).IdFlusso.ToString.PadLeft(5, CChar("0")) + "_" + Format(Now, "yyyyMMdd_hhmmss") & ".zip"
            Dim di As New DirectoryInfo(ConstSession.PathZIP)
            If Not di.Exists Then di.Create()

            ListDocumenti = CType(Session("ELENCO_DOCUMENTI_STAMPATI"), GruppoURL())
            'creo zip
            Try
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ViewDocElaborati.btnDownloadAll_Click.devo creare percorso->" + ConstSession.PathZIP + FileNameZip)
                strmZipOutputStream = New ICSharpCode.SharpZipLib.Zip.ZipOutputStream(File.Create(ConstSession.PathZIP + FileNameZip))
                For Each myfile As GruppoURL In ListDocumenti
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ViewDocElaborati.btnDownloadAll_Click.devo zippare->" + myfile.URLComplessivo.Path)
                    Dim entry As New ICSharpCode.SharpZipLib.Zip.ZipEntry(Path.GetFileName(myfile.URLComplessivo.Path))
                    entry.DateTime = DateTime.Now
                    strmZipOutputStream.PutNextEntry(entry)
                    strmZipOutputStream.Write(File.ReadAllBytes(myfile.URLComplessivo.Path), 0, File.ReadAllBytes(myfile.URLComplessivo.Path).Length)
                Next
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ViewDocElaborati.btnDownloadAll_Click.CreaZip.errore: ", ex)
                Response.Redirect("../../../PaginaErrore.aspx")
            Finally
                strmZipOutputStream.Finish()
                strmZipOutputStream.Close()
            End Try

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ViewDocElaborati.btnDownloadAll_Click.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
        download(FileNameZip)
        Threading.Thread.Sleep(1000)
    End Sub
    ''' <summary>
    ''' Funzione per il download del documento.
    ''' </summary>
    ''' <param name="nomeFileZip"></param>
    Private Sub download(ByVal nomeFileZip As String)
        Response.ContentType = "*/*"
        Response.AppendHeader("content-disposition", "attachment; filename=" + nomeFileZip)
        Response.WriteFile(ConstSession.PathZIP + nomeFileZip)
        Response.End()
    End Sub
End Class
