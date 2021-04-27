Imports log4net
''' <summary>
''' Pagina dei comandi per la gestione degli immobili da ANATER.
''' Le possibili opzioni sono:
''' - Associa
''' - Ricerca Van
''' - Ricerca
''' - Torna alla videata precedente
''' </summary>
Partial Class ComandiRicImmobile
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ComandiRicImmobile))
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
        Dim sScript As String
        'LblTitolo.Text = ConstSession.DescrizioneEnte
        Try
            If Request.Item("Provenienza") = "A" Then
                sScript = "document.getElementById(Of'RicVani').style.display='none';"
                RegisterScript( sScript,Me.GetType)
            End If
            If ConstSession.IsFromTARES = "1" Then
                info.InnerText = "TARI "
            Else
                info.InnerText = "TARSU "
            End If
            info.InnerText += " Variabile - Dichiarazioni - Ricerca Immobile " & ConstSession.NameSistemaTerritorio
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ComandiRicImmobile.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try

    End Sub
End Class
