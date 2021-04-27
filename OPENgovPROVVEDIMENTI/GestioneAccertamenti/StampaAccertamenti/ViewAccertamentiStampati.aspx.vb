Imports log4net
Imports RIBESElaborazioneDocumentiInterface.Stampa.oggetti
''' <summary>
''' Pagina per la consultazione dei documenti prodotti per il provvedimento.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class ViewAccertamentiStampati

    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ViewAccertamentiStampati))

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
        Dim objAccertamentiStampati As GruppoURL
        Dim myList As New ArrayList
        Try
            If Page.IsPostBack = False Then
                objAccertamentiStampati = CType(Session("ELENCO_ACCERTAMENTI_STAMPATI"), GruppoURL)
                If Not objAccertamentiStampati.URLComplessivo Is Nothing Then
                    Dim myItem As New oggettoURL
                    myItem.Url = objAccertamentiStampati.URLComplessivo.Url
                    myItem.Name = objAccertamentiStampati.URLComplessivo.Name
                    myList.Add(myItem)
                Else
                    For Each myItem As oggettoURL In objAccertamentiStampati.URLDocumenti
                        myList.Add(myItem)
                    Next
                End If
                ViewState.Add("AccertamentiStampati_URLDocumenti", objAccertamentiStampati)
                objAccertamentiStampati.URLDocumenti = CType(myList.ToArray(GetType(oggettoURL)), oggettoURL())
                GrdAnagrafica.DataSource = objAccertamentiStampati.URLDocumenti
                GrdAnagrafica.DataBind()
            Else
                GrdAnagrafica.DataSource = CType(ViewState("AccertamentiStampati_URLDocumenti"), GruppoURL).URLDocumenti 'objLettereStampate
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ViewAccertamentiStampati.Page_Load.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub

End Class
