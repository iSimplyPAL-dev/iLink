Imports log4net
''' <summary>
''' Pagina dei comandi per la gestione .
''' Le possibili opzioni sono:
''' - Stampa
''' - Ricerca
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class ComandiRicAvvisi
    Inherits BaseEnte
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ComandiRicAvvisi))
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
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sScript As String = ""
        Dim oListCmd() As Object
        Dim MyFunction As New generalClass.generalFunction

        'Put user code to initialize the page here
        lblTitolo.Text = ConstSession.DescrizioneEnte
        Try
            If ConstSession.IsFromTARES = "1" Then
                info.InnerText = "TARI "
            Else
                info.InnerText = "TARSU "
            End If
            If ConstSession.IsFromVariabile(Request.Item("IsFromVariabile")) = "1" Then
                info.InnerText += "Variabile"
            End If
            info.InnerText += " - Avvisi - Ricerca"
            If Request.Item("sProvenienza") = "C" Then
                ReDim Preserve oListCmd(0)
                oListCmd(0) = "NewInsert"
                sScript += "$('#" + oListCmd(0).ToString() + "').addClass('DisableBtn');"
                RegisterScript(sScript, Me.GetType)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ComandiRicAvvisi.Page_Load.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    Dim sScript As String
    '    Dim oListCmd() As Object
    '    Dim MyFunction As New generalClass.generalFunction

    '    'Put user code to initialize the page here
    '    lblTitolo.Text = ConstSession.DescrizioneEnte
    '    Try
    '        If ConstSession.IsFromTARES = "1" Then
    '            info.InnerText = "TARI "
    '        Else
    '            info.InnerText = "TARSU "
    '        End If
    '        If ConstSession.IsFromVariabile(Request.Item("IsFromVariabile")) = "1" Then
    '            info.InnerText += "Variabile"
    '        End If
    '        info.InnerText += " - Avvisi - Ricerca"
    '        If Request.Item("sProvenienza") = "C" Then
    '            ReDim Preserve oListCmd(0)
    '            oListCmd(0) = "NewInsert"
    '            sScript = MyFunction.PopolaJSdisabilita(oListCmd)
    '            RegisterScript(sScript, Me.GetType)
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ComandiRicAvvisi.Page_Load.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    End Try
    'End Sub
End Class
