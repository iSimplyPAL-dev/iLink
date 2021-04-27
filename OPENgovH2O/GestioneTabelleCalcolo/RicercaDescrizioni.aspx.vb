Imports log4net

Partial Class RicercaDescrizioni
    Inherits BasePage
    Public Tabella As String = ""
    Dim ClsAddizionali As New ClsAddizionali
    Dim ClsCanoni As New ClsCanoni
    Private Shared Log As ILog = LogManager.GetLogger(GetType(CConfiguraAddizionali))
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
    ''' Configurazione descrizione spese fisse e canoni
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sScript As String
        Try
            If Request.Item("Tabella") <> "" Then
                Tabella = Request.Item("Tabella")
                'Put user code to initialize the page here
                If InStr(Tabella, ";") > 0 Then
                    Tabella = Tabella.Substring(0, InStr(Tabella, ";") - 1)
                End If
                txtTabella.Style.Add("display", "none")
                txtTabella.Text = Tabella
            End If

            If txtTabella.Text = "TP_ADDIZIONALI" Then
                ClsAddizionali.LoadComboAddizionali(ddlCodice)
            Else
                ClsCanoni.LoadComboCanoni(ddlCodice)
            End If

            'If Request.Item("EffettuaRicerca") = "si" Then
            sScript = "Search('" & Tabella & "');"
            RegisterScript(sScript, Me.GetType())
            'End If
            sScript = "parent.parent.Comandi.location.href='./ComandiRicercaDescrizioni.aspx';"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RicercaDescrizioni.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

End Class
