Imports log4net

Partial Class ComandiDettFatturazione
    Inherits BasePage

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
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ComandiDettFatturazione))
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
        Try
            If ConstSession.DescrPeriodo = "" Then
                Dim FncGen As New ClsGenerale.Generale
                FncGen.GetPeriodoAttuale
            End If
            info.Text = "Acquedotto - Fatturazione - Dettaglio"
            Comune.Text = ConstSession.DescrizioneEnte & "   -   Periodo: " & ConstSession.DescrPeriodo

            'Provenienza=E: elaborazione fatture, non blocco
            'Provenienza=V: gestione note/variazioni, blocco la cancellazione
            'Provenienza=C: solo consultazione, blocco la modifica e la cancellazione
            'Provenienza=N: visualizzazione nota di credito su fattura, blocco tutto
            Dim sScript As String = ""
            Dim oListCmd() As Object
            Dim x As Integer

            If Request.Item("Provenienza") = "C" Or Request.Item("Provenienza") = "N" Then
                ReDim Preserve oListCmd(2)
                oListCmd(0) = "Modifica"
                oListCmd(1) = "Ricalcolo"
                oListCmd(2) = "Delete"
                For x = 0 To oListCmd.Length - 1
                    sScript += "$('#" + oListCmd(x).ToString() + "').addClass('DisableBtn');"
                Next
            ElseIf Request.Item("Provenienza") = "V" Then
                ReDim Preserve oListCmd(0)
                oListCmd(0) = "Ricalcolo"
                For x = 0 To oListCmd.Length - 1
                    sScript += "$('#" + oListCmd(x).ToString() + "').addClass('DisableBtn');"
                Next
            End If
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ComandiDettFatturazione.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    'Put user code to initialize the page here
    '    'info.Text = Request("title")
    '    'Comune.Text = Request("enteperiodo")
    '    Try
    '        If ConstSession.DescrPeriodo = "" Then
    '            Dim FncGen As New ClsGenerale.Generale
    '            FncGen.GetPeriodoAttuale
    '        End If
    '        info.Text = "Acquedotto - Fatturazione - Dettaglio"
    '        Comune.Text = ConstSession.DescrizioneEnte & "   -   Periodo: " & ConstSession.DescrPeriodo

    '        'Provenienza=E: elaborazione fatture, bloccavo la modifica
    '        'Provenienza=V: gestione note/variazioni, blocco la cancellazione
    '        'Provenienza=C: solo consultazione, blocco la modifica e la cancellazione
    '        'Provenienza=N: visualizzazione nota di credito su fattura, blocco tutto
    '        Dim sScript As String
    '        Dim oListCmd() As Object
    '        Dim x As Integer

    '        If Request.Item("Provenienza") = "E" Then
    '            'ReDim Preserve oListCmd(1)
    '            'oListCmd(0) = "Modifica"
    '            'oListCmd(1) = "Ricalcolo"
    '            'For x = 0 To oListCmd.Length - 1
    '            '    'sScript += "document.getElementById('" & oListCmd(x).ToString() & "').disabled=" & True.ToString.ToLower & ";"
    '            '    sScript += "document.getElementById('" & oListCmd(x).ToString() & "').style.display='none';"
    '            'Next
    '        ElseIf Request.Item("Provenienza") = "C" Or Request.Item("Provenienza") = "N" Then
    '            ReDim Preserve oListCmd(2)
    '            oListCmd(0) = "Modifica"
    '            oListCmd(1) = "Ricalcolo"
    '            oListCmd(2) = "Delete"
    '            For x = 0 To oListCmd.Length - 1
    '                'sScript += "document.getElementById('" & oListCmd(x).ToString() & "').disabled=" & True.ToString.ToLower & ";"
    '                sScript += "document.getElementById('" & oListCmd(x).ToString() & "').style.display='none';"
    '            Next
    '        ElseIf Request.Item("Provenienza") = "V" Then
    '            ReDim Preserve oListCmd(0)
    '            oListCmd(0) = "Ricalcolo"
    '            For x = 0 To oListCmd.Length - 1
    '                sScript += "document.getElementById('" & oListCmd(x).ToString() & "').disabled=" & True.ToString.ToLower & ";"
    '            Next
    '        End If
    '        RegisterScript(sScript, Me.GetType())
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ComandiDettFatturazione.Page_Load.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
End Class
