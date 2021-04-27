Imports System.Xml
Imports System.Data
Imports System.Text
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Reflection
Imports log4net

Partial Class ExecContatori
    Inherits BasePage
    Dim GestLetture As GestLetture = New GestLetture()
    Dim DBContatori As GestContatori = New GestContatori
    Dim clsLetture As New clsLetture()
    Dim _Const As New Costanti()
    Dim ContatoreID As Integer
    Dim LetturaID As Integer
    Private ModDate As New ClsGenerale.Generale
    Private iDB As New DBAccess.getDBobject()
    Dim RaiseError As New GestioneFile()

    Private Shared Log As ILog = LogManager.GetLogger(GetType(ExecContatori))

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

        dim sScript as string=""
        

        dim sSQL as string

        sSQL=""
        sSQL="UPDATE TP_CONTATORI SET" & vbCrLf
        sSQL+="IDGIRO=" & utility.stringoperation.formatint(Request("hdIDGiro")) & "," & vbCrLf
        sSQL+="CODPOSIZIONE=" & utility.stringoperation.formatint(Request("hdIDPosizione")) & "," & vbCrLf
        sSQL+="SEQUENZA=" & utility.stringoperation.formatstring(Request("hdSequenza")) & "," & vbCrLf
        sSQL+="LETTO=1" & "," & vbCrLf
        sSQL+="POSIZIONEPROGRESSIVA=" & utility.stringoperation.formatstring(Request("hdProgressivo")) & "," & vbCrLf
        sSQL+="COD_STRADA=" & utility.stringoperation.formatint(Request("UBICAZIONE")) & "," & vbCrLf
        sSQL+="CIVICO_UBICAZIONE=" & utility.stringoperation.formatstring(Request("hdNumeroCivico")) & "," & vbCrLf
        sSQL+="ESPONENTE_CIVICO=" & utility.stringoperation.formatstring(Request("ESPONENTE")) & "," & vbCrLf
        sSQL+="LATOSTRADA=" & utility.stringoperation.formatstring(Request("hdLatoStrada")) & "," & vbCrLf
        sSQL+="MATRICOLA=" & utility.stringoperation.formatstring(Request("MATRICOLA")) & "," & vbCrLf
        sSQL+="IDTIPOCONTATORE=" & utility.stringoperation.formatint(Request("TIPOCONTATORE")) & "," & vbCrLf
        sSQL+="CIFRECONTATORE=" & utility.stringoperation.formatstring(Request("hdCIFRECONTATORE")) & "," & vbCrLf
        sSQL+="NOTE=" & utility.stringoperation.formatstring(Request("hdNOTECONTATORE")) & "," & vbCrLf
        '*** Fabi
        sSQL+="VIA_UBICAZIONE=" & utility.stringoperation.formatstring(Request("hdSTRADA")) & vbCrLf
        '*** /Fabi

        sSQL+="WHERE" & vbCrLf
        sSQL+="CODCONTATORE=" & Request("hdIDContatore")

        Try
            If iDB.ExecuteNonQuery(sSQL) <> 1 Then
                Throw New Exception("errore in inserimento::" & sSQL)
            End If
        Catch er As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ExecContatori.Page_Load.errore: ", er)
            Response.Redirect("../../PaginaErrore.aspx")
            RaiseError.trace(er, sSql.Replace(vbCrLf, ""), ConfigurationManager.AppSettings("Filename"), ConfigurationManager.AppSettings("FilePath"), "")
            sScript += "parent.opener.parent.Visualizza.Error();"
        Finally
            sScript += "parent.window.close();"
            RegisterScript(sScript , Me.GetType())
        End Try
    End Sub
End Class
