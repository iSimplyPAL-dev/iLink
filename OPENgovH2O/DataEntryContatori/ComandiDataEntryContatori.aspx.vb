Imports log4net

Partial Class ComandiDataEntryContatori
    Inherits BasePage

    Private Shared Log As ILog = LogManager.GetLogger(GetType(ComandiDataEntryContatori))

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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sScript As String = ""
        Try
            If ConstSession.DescrPeriodo = "" Then
                Dim FncGen As New ClsGenerale.Generale
                FncGen.GetPeriodoAttuale()
            End If
            info.Text = "Acquedotto - Contatori - Gestione"
            Comune.Text = ConstSession.DescrizioneEnte & "   -   Periodo: " & ConstSession.DescrPeriodo

            btnSost.Attributes.Add("onclick", "sostituisciContatore()")

            If Request.Item("sProvenienza") = "AE" Then
                btnApriContratti.Disabled = True
                btnApriLetture.Disabled = True
                btnSub.Disabled = True
                btnSost.Disabled = True
                btnAttivaContatore.Disabled = True
                btnAnnullaSitContrib.Visible = False
            ElseIf Request.Item("sProvenienza") = "R" Then
                btnApriContratti.Disabled = True
                btnApriLetture.Disabled = True
                btnSub.Disabled = True
                btnSost.Disabled = True
                btnAttivaContatore.Disabled = True
                button2.Disabled = True
                btnAnnullaSitContrib.Visible = False
            ElseIf Request.Item("sProvenienza") = "SC" Then
                btnApriContratti.Disabled = True
                btnApriLetture.Disabled = True
                btnSub.Disabled = True
                btnSost.Disabled = True
                btnAttivaContatore.Disabled = True
                button2.Disabled = True
                btnAnnulla.Visible = False
                btnAnnullaSitContrib.Visible = True
            Else
                btnAnnullaSitContrib.Visible = False
            End If
            '*** 20140923 - GIS ***
            If ConstSession.VisualGIS = False Then
                sScript += "document.getElementById('GIS').style.display='none';"
            End If
            '*** ***
            RegisterScript(sScript, Me.GetType())

        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ComandiDataEntryContatori.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    Private Sub btnSub_ServerClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSub.ServerClick

    End Sub
End Class
