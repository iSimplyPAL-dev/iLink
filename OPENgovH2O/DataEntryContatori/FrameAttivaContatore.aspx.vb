Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports log4net
Imports Utility

Partial Class FrameAttivaContatore
    Inherits BasePage
    Dim DBAccess As New DBAccess.getDBobject
    Private FncDate As New ClsGenerale.Generale

    Private Shared Log As ILog = LogManager.GetLogger(GetType(FrameAttivaContatore))

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
            If CStr(Request.Params("IDCont")) <> 0 Then
                Dim sSQL As String = ""
                Dim dvMyDati As New DataView
                Dim conteggio As Int16 = 0

                'prelevo la data di sottoscrizione per il controllo sulla congruenza delle date
                sSQL = "SELECT DATASOTTOSCRIZIONE"
                sSQL += " FROM TP_CONTRATTI"
                sSQL += " WHERE (CODCONTRATTO=" & Request.Params("IdContratto") & ")"
                dvMyDati = DBAccess.GetDataView(sSQL)
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        TxtDataSottoscrizione.Text = FncDate.GiraDataFromDB(StringOperation.FormatString(myRow("datasottoscrizione")))
                    Next
                End If
                dvMyDati.Dispose()

                'prelevo le eventuali letture presenti
                Try
                    dvMyDati = New clsLetture().GetTopLetture(ConstSession.DBType, ConstSession.StringConnection, 100, Utility.StringOperation.FormatInt(Request.Params("IDCont")), "", "")
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            conteggio += 1
                        Next
                    End If
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.FrameAttivaContatore.GetTop100Letture.Update.errore: ", ex)
                Finally
                    dvMyDati.Dispose()
                End Try
                Dim sScript As String = ""
                If conteggio > 0 Then
                    If conteggio = 1 Then
                        lblTesto.Text = "Modificando la data di attivazione, verrà modificata anche la data relativa alla prima lettura."
                        sScript += "parent.opener.parent.Visualizza.document.getElementById('UpdPrimaLettura').value='UPDATE';"
                        RegisterScript(sScript, Me.GetType())
                    Else
                        lblTesto.Text = "Sono presenti più letture, non è possibile modificare la data di attivazione."
                        txtDataAttiv.Style.Add("display", "none")
                        sScript += "document.getElementById('BottoneSalva').style.display='none';"
                        RegisterScript(sScript, Me.GetType())
                    End If
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.FrameAttivaContatore.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
