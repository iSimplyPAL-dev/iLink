Imports log4net
''' <summary>
''' Pagina dei comandi per la gestione dell'anagrafica da ANATER.
''' Le possibili opzioni sono:
''' - Ricerca
''' - Torna alla videata precedente
''' </summary>
Partial Class cRicAnagrafeAnater
    Inherits BasePage

    Private Shared Log As ILog = LogManager.GetLogger(GetType(cRicAnagrafeAnater))

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
            If Request.Item("popup") = "1" Then
                btnChiudi.Visible = True
            Else
                btnChiudi.Visible = False
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.cRicAnagrafeAnater.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

End Class
