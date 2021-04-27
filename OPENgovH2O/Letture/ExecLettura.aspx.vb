Imports System.Xml
Imports System.Data
Imports System.Text
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Reflection
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports log4net
Imports Utility

Partial Class ExecLettura
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ExecLettura))
    Dim GestLetture As GestLetture = New GestLetture
    Dim DBContatori As GestContatori = New GestContatori
    Dim clsLetture As New clsLetture
    Dim _Const As New Costanti
    Dim ContatoreID, LetturaID As Integer

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

    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    Dim ConsumoEffettivo As Long
    '    Dim Lettura As String
    '    Dim DataLettura As String
    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Dim WFErrore As String

    '    Try
    '        'inizializzo la connessione
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        ContatoreID = stringoperation.formatint(request("IDCONTATORE"))
    '        Lettura = Request("LETTURA")

    '        LetturaID = stringoperation.formatint(request("IDLETTURA"))

    '        Dim DetailContatore As New objContatore
    '        DetailContatore = DBContatori.GetDetailsContatori(ContatoreID, WFSessione)

    '        Dim DettaglioLetture As New ObjLettura
    '        DettaglioLetture = GestLetture.GetDettaglioLetture(LetturaID, ContatoreID, -1, Date.MinValue, -1, False)

    '        If LetturaID = 0 Then '''''''''''''''''''''''''''''''NUOVA LETTURA''''''''''''''''''''''''''''''''''''''''''
    '            If Not GestLetture.VerificaEsistenzaLettura(ContatoreID, DetailContatore.nIdUtente) Then
    '                dim sScript as string=""
    '                sScript +="")
    '                sScript +="parent.Visualizza.frmModifica.txtConsEffettivo.value='" & 0 & "';"
    '                sScript +="parent.Visualizza.frmModifica.txtConsEffettivo.readOnly=false;")
    '                sScript +="")
    '                RegisterScript(sscript,me.gettype())"Field", strBuilderField.ToString())
    '            Else
    '                clsLetture.CalcolaConsumoEffettivo(Lettura, ConsumoEffettivo, DettaglioLetture.tDataLetturaPrec.ToString, ContatoreID)

    '                dim sScript as string=""
    '                sScript +="")
    '                sScript +="parent.Visualizza.frmModifica.txtConsEffettivo.value='" & ConsumoEffettivo & "';"
    '                sScript +="")
    '                RegisterScript(sscript,me.gettype())"Field", strBuilderField.ToString())
    '            End If
    '        End If

    '        If LetturaID > 0 Then
    '            'Calcolo del Consumo Effettivo
    '            clsLetture.CalcolaConsumoEffettivo(DettaglioLetture.tDataLetturaAtt, ConsumoEffettivo, DettaglioLetture.tDataLetturaPrec.ToString, ContatoreID)

    '            dim sScript as string=""
    '            sScript +="")
    '            sScript +="parent.Visualizza.frmModifica.txtConsEffettivo.value='" & ConsumoEffettivo & "';"
    '            If ConsumoEffettivo = 0 Then
    '                sScript +="parent.Visualizza.frmModifica.txtConsEffettivo.readOnly=false;")
    '            End If
    '            sScript +="")
    '            RegisterScript(sscript,me.gettype())"Field", strBuilderField.ToString())
    '    End If
    '    Catch Err As Exception
    '       
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ExecLettura.Page_Load.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    Finally
    '        WFSessione.Kill()
    '    End Try
    'End Sub
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim ConsumoEffettivo As Long
        Dim Lettura As String

        Try
            ContatoreID = stringoperation.formatint(request("IDCONTATORE"))
            Lettura = Request("LETTURA")

            LetturaID = stringoperation.formatint(request("IDLETTURA"))

            Dim DetailContatore As New objContatore
            DetailContatore = DBContatori.GetDetailsContatori(ContatoreID, -1)

            Dim DettaglioLetture As New ObjLettura
            DettaglioLetture = GestLetture.GetDettaglioLetture(LetturaID, ContatoreID, -1, Date.MaxValue, -1, False)

            If LetturaID = 0 Then '''''''''''''''''''''''''''''''NUOVA LETTURA''''''''''''''''''''''''''''''''''''''''''
                If Not GestLetture.VerificaEsistenzaLettura(ContatoreID, DetailContatore.nIdUtente) Then
                    Dim sScript As String = ""
                    sScript += "parent.Visualizza.document.getElementById('txtConsEffettivo').value='" & 0 & "';"
                    sScript += "parent.Visualizza.document.getElementById('txtConsEffettivo').readOnly=false;"
                    RegisterScript(sScript, Me.GetType())
                Else
                    clsLetture.CalcolaConsumoEffettivo(Lettura, ConsumoEffettivo, DettaglioLetture.tDataLetturaPrec.ToString, ContatoreID)

                    Dim sScript As String = ""
                    sScript += "parent.Visualizza.document.getElementById('txtConsEffettivo').value='" & ConsumoEffettivo & "';"
                    RegisterScript(sScript, Me.GetType())
                End If
            End If

            If LetturaID > 0 Then
                'Calcolo del Consumo Effettivo
                clsLetture.CalcolaConsumoEffettivo(DettaglioLetture.tDataLetturaAtt, ConsumoEffettivo, DettaglioLetture.tDataLetturaPrec.ToString, ContatoreID)

                Dim sScript As String = ""
                sScript += "parent.Visualizza.document.getElementById('txtConsEffettivo').value='" & ConsumoEffettivo & "';"
                If ConsumoEffettivo = 0 Then
                    sScript += "parent.Visualizza.document.getElementById('txtConsEffettivo').readOnly=false;"
                End If
                RegisterScript(sScript, Me.GetType())
            End If
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ExecLettura.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
