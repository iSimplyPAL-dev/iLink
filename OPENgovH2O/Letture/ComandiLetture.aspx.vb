Imports System.Text
Imports log4net
Imports Utility

Partial Class FrameComandiLetture
    Inherits BasePage
    Dim _Const As New Costanti
    Public OriginePagina As String
    Private Shared Log As ILog = LogManager.GetLogger(GetType(FrameComandiLetture))
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
            'info.Text = "Acquedotto - Letture"
            'Comune.Text = Request("enteperiodo")
            If ConstSession.DescrPeriodo = "" Then
                Dim FncGen As New ClsGenerale.Generale
                FncGen.GetPeriodoAttuale
            End If
            info.Text = "Acquedotto - Letture - Dettaglio"
            Comune.Text = ConstSession.DescrizioneEnte & "   -   Periodo: " & ConstSession.DescrPeriodo
            OriginePagina = Request("PAG_PREC")
            If IsNumeric(Request("PAG_PREC")) = False Then
                OriginePagina = "1"
            End If

            Select Case OriginePagina
                Case CStr(Costanti.enmContesto.DECONTATORI)
                    If StringOperation.FormatInt(Request("VIEWLETTURE")) = 1 Then
                        sScript += "document.getElementById('Nuovo').style.display='none';"
                        sScript += "document.getElementById('Conferma').style.display='none';"
                        sScript += "document.getElementById('CANCELLA').style.display='none';"
                    Else
                        sScript += "document.getElementById('Nuovo').style.display='none';"
                        If StringOperation.FormatInt(Request("IDLettura")) = 0 Then
                            sScript += "document.getElementById('CANCELLA').style.display='none';"
                        End If
                    End If
                Case Else
                    sScript += "document.getElementById('Nuovo').style.display='none';"
                    If StringOperation.FormatInt(Request("IDLettura")) = 0 Then
                        sScript += "document.getElementById('CANCELLA').style.display='none';"
                    End If
            End Select
            RegisterScript(sScript, Me.GetType())

            'se la lettura è già fatturata non posso modificarla/cancellarla
            If Request.Item("IsFatturata") = "1" Then

                sScript += "document.getElementById('Conferma').style.display='none';"
                sScript += "document.getElementById('Nuovo').style.display='none';"
                sScript += "document.getElementById('CANCELLA').style.display='none';"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ComandiLetture.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
