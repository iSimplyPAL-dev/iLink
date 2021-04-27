Imports log4net

Partial Class ComandiDataEntryContratti
    Inherits BasePage


    Private Shared Log As ILog = LogManager.GetLogger(GetType(ComandiDataEntryContratti))
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
        'Put user code to initialize the page here
        Dim sScript As String = ""
        Dim oListCmd() As Object
        Dim x As Integer

        Try
            If ConstSession.DescrPeriodo = "" Then
                Dim FncGen As New ClsGenerale.Generale
                FncGen.GetPeriodoAttuale()
            End If
            info.Text = "Acquedotto - Contratti - Gestione"
            Comune.Text = ConstSession.DescrizioneEnte & "   -   Periodo: " & ConstSession.DescrPeriodo

            btnvoltura.Attributes.Add("onclick", "CessaContratto()")
            If Request.Item("DataScaricoPDA") <> "" Then
                ReDim Preserve oListCmd(2)
                oListCmd(0) = "btnvoltura"
                oListCmd(1) = "button2"
                oListCmd(2) = "btnElimina"
                For x = 0 To oListCmd.Length - 1
                    sScript += "$('#" + oListCmd(x).ToString() + "').addClass('DisableBtn');"
                Next
                RegisterScript(sScript, Me.GetType())
            End If

        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ComandiDataEntryContratti.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    'Put user code to initialize the page here
    '    Dim sScript As String
    '    Dim oListCmd() As Object
    '    Dim x As Integer

    '    'info.Text = Request("title")
    '    'Comune.Text = Request("enteperiodo")
    '    Try
    '        If ConstSession.DescrPeriodo = "" Then
    '        Dim FncGen As New ClsGenerale.Generale
    '        FncGen.GetPeriodoAttuale
    '    End If
    '    info.Text = "Acquedotto - Contratti - Gestione"
    '    Comune.Text = ConstSession.DescrizioneEnte & "   -   Periodo: " & ConstSession.DescrPeriodo

    '    btnvoltura.Attributes.Add("onclick", "CessaContratto()")
    '        If Request.Item("DataScaricoPDA") <> "" Then
    '            ReDim Preserve oListCmd(2)
    '            oListCmd(0) = "btnvoltura"
    '            oListCmd(1) = "button2"
    '            oListCmd(2) = "btnElimina"
    '            For x = 0 To oListCmd.Length - 1
    '                'sScript += "document.getElementById('" & oListCmd(x).ToString() & "').disabled=" & True.ToString.ToLower & ";"
    '                sScript += "document.getElementById('" & oListCmd(x).ToString() & "').style.display='none';"
    '            Next
    '            RegisterScript(sScript, Me.GetType())
    '        End If

    '    Catch ex As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ComandiDataEntryContratti.Page_Load.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
End Class
