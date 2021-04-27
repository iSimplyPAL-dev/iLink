Imports System.Web.UI.Control
Imports System.Web.UI
Imports log4net

''' <summary>
''' Pagina dei comandi per la consultazione avanzata dei provvedimenti.
''' Le possibili opzioni sono:
''' - Stampa
''' - Gestione aggiornamento Massivo Date
''' - Attiva/Disattivare filtro per Data
''' - Ricerca
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class ComandiRicercaAvanzata
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ComandiRicercaAvanzata))

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
    ''' <revisionHistory>
    ''' <revision date="11/2015">
    ''' Funzioni Sovracomunali
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim sScript As String = ""

        lblTitolo.Text = ConstSession.DescrizioneEnte
        Try
            If ConstSession.IdEnte = "" Then
                Dim oListCmd() As Object
                ReDim Preserve oListCmd(0)
                oListCmd(0) = "Massiva"
                For x As Integer = 0 To oListCmd.Length - 1
                    sScript += "$('#" + oListCmd(x).ToString() + "').addClass('DisableBtn');"
                Next
                RegisterScript(sScript, Me.GetType())
            End If
            '***
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ComandiRicercaAvanzata.Page_Load.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    'Put user code to initialize the page here
    '    Dim sScript As String = ""

    '    lblTitolo.Text = ConstSession.DescrizioneEnte
    '    '*** 201511 - Funzioni Sovracomunali ***
    '    Try
    '        If ConstSession.IdEnte = "" Then
    '            Dim oListCmd() As Object
    '            ReDim Preserve oListCmd(0)
    '            oListCmd(0) = "Massiva"
    '            For i As Integer = 0 To oListCmd.Length - 1
    '                sScript += "document.getElementById(Of'" & oListCmd(i).ToString() & "').style.display='none';"
    '    Next
    '            RegisterScript(sScript, Me.GetType())
    '        End If
    '        '***
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ComandiRicercaAvanzata.Page_Load.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    End Try
    'End Sub
End Class
