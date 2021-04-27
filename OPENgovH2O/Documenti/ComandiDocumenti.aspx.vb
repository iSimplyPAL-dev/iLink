Imports RIBESElaborazioneDocumentiInterface
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports OPENUtility
Imports log4net

Partial Class ComandiDocumenti
    Inherits BasePage

    Private Shared Log As ILog = LogManager.GetLogger(GetType(ComandiDocumenti))
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
        Dim sScript As String
        info.Text = "Acquedotto - Elaborazione Documenti"
        Comune.Text = "Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo

        Dim oMyTotRuolo As New ObjTotRuoloFatture
        oMyTotRuolo = CType(Session("oRuoloH2O"), ObjTotRuoloFatture)
        Try
            If Not oMyTotRuolo Is Nothing Then
                If oMyTotRuolo.tDataApprovazioneDOC <> Date.MinValue And oMyTotRuolo.tDataApprovazioneDOC.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                    sScript = ""
                    sScript += "document.getElementById('EliminaElabora').disabled=true;"
                    sScript += "document.getElementById('ApprovaMinuta').disabled=true;"
                    sScript += "document.getElementById('ElaborazioneDocumenti').disabled=true;"
                    sScript += "document.getElementById('Search').disabled=true;"
                    sScript += ""
                    RegisterScript(sScript, Me.GetType())
                Else
                    sScript = ""
                    sScript += "document.getElementById('EliminaElabora').disabled=false;"
                    sScript += "document.getElementById('ApprovaMinuta').disabled=false;"
                    sScript += "document.getElementById('ElaborazioneDocumenti').disabled=false;"
                    sScript += "document.getElementById('Search').disabled=false;"
                    sScript += ""
                    RegisterScript(sScript, Me.GetType())
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ComandiDocumenti.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
