Imports System.Web.UI.Control
Imports System.Web.UI
Imports System.DBNull
Imports log4net
''' <summary>
''' Pagina dei comandi per la gestione del provvedimento.
''' Le possibili opzioni sono:
''' - Visualizza documento Atto
''' - Forza Dati
''' - Stampa
''' - Stampa i bollettini
''' - Rettifica Avviso
''' - Salva 
''' - Torna alla videata precedente
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class ComandiGestioneAttiAccertamenti
    Inherits BasePage
    Public strPARAMETRI As String
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ComandiGestioneAttiAccertamenti))

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

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

        Dim strTipoProvvedimento As String
        Dim strUTENTE As String
        Dim sScript As String = ""
        Dim objUtility As New MyUtility

        'Dipe 25/10/2007 
        'Attivazione del tasto per ritornare alla videta dei dati attuali
        'Il valore della session è ti tipo booleano
        Try
            If (Session("DATI_ATTUALI") = Nothing) Then
                Session("DATI_ATTUALI") = False
            End If

            infoEnte.Text = Session("DESCRIZIONE_ENTE")
            info.Text = "Gestione Atti - Gestione Provvedimento"

            If (CType(Session("DATI_ATTUALI"), Boolean)) Then
                sscript += "document.getElementById (Of'backAttuali').style.display='inline';"
                sScript += "document.getElementById ('return').style.display='none';"
            Else
                sScript += "document.getElementById ('backAttuali').style.display='none';"
                sScript += "document.getElementById ('return').style.display='inline';"
            End If
            'If (Session("COD_TRIBUTO") <> "0434") Then
            '    sscript+= "document.getElementById ('VisualizzaDocumentoPDF').style.display='none';"
            'End If

            'If (Session("COD_TRIBUTO") = "0434") Then
            '    If Not CBool(ConfigurationManager.AppSettings("VIEW_PDF_TARSU")) Then
            '        sscript+= "document.getElementById ('VisualizzaDocumentoPDF').style.display='none';"
            '    End If
            '    'sscript+= "document.getElementById ('StampaBollettino').style.display='none';"
            'Else
            '    sscript+= "document.getElementById ('VisualizzaDocumentoPDF').style.display='none';"
            'End If
            RegisterScript(sScript, Me.GetType())

            If Not Request("TIPO_PROVVEDIMENTO") Is Nothing Or Not Request("UTENTE") Is Nothing Then
                strTipoProvvedimento = Session("PROVENIENZA")

                strTipoProvvedimento = objUtility.CToStr(Request("TIPO_PROVVEDIMENTO"))
                strUTENTE = objUtility.CToStr(Request("UTENTE"))

                strPARAMETRI = "?TIPO_PROVVEDIMENTO=" & strTipoProvvedimento & "&UTENTE=" & strUTENTE & "&NOMEPAGINA=" & "StatoElaborazioneQuestionari.aspx"
                txtPagina.Text = strPARAMETRI
            End If
            'Put user code to initialize the page here
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ComandiGestioneAttiAccertamenti.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
