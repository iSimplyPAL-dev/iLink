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

Partial Class ExecDataLettura
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ExecDataLettura))

    Dim GestLetture As GestLetture = New GestLetture
    Dim DBContatori As GestContatori = New GestContatori
    Dim clsLetture As New clsLetture
    Dim _Const As New Costanti
    Dim ContatoreID As Integer
    Dim LetturaID As Integer
    Private ModDate As New ClsGenerale.Generale
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
    '    Dim GiorniDiConsumo, LetturaTeorica, ConsumoTeorico, DataLetturaSuccessiva, DataLetturaPrecedente As Long
    '    Dim DataValida As Boolean
    '    Dim DataLettura As String
    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Dim WFErrore As String

    '    Try
    '        'inizializzo la connessione
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        DataValida = False
    '        ContatoreID = stringoperation.formatint(request("IDCONTATORE"))
    '        LetturaID = stringoperation.formatint(request("IDLETTURA"))

    '        DataLettura = Request("DATALETTURA")

    '        Dim DetailContatore As New objContatore
    '        DetailContatore = DBContatori.GetDetailsContatori(ContatoreID, WFSessione)
    '        Dim DettaglioLetture As New ObjLettura
    '        DettaglioLetture = GestLetture.GetDettaglioLetture(LetturaID, ContatoreID, -1, Date.MinValue, -1, False)
    '        'Log.Debug("ExecDataLettura::prelevato contatori e letture")
    '        '********************************************************************************
    '        'NUOVA LETTURA
    '        '********************************************************************************
    '        If LetturaID = 0 Then
    '            '********************************************************************************
    '            'SE RITORNA FALSE NON CI SONO LETTURE PRECEDENTE
    '            '********************************************************************************
    '            If Not GestLetture.VerificaEsistenzaLettura(ContatoreID, DetailContatore.nIdUtente) Then
    '                If Len(DetailContatore.sDataAttivazione) > 0 Then
    '                    clsLetture.ControllaDataAttivazione(CStr(ContatoreID), DataLettura, DetailContatore.sDataAttivazione, ConsumoTeorico, _
    '                    GiorniDiConsumo, _
    '                    LetturaTeorica, _
    '                    DataValida, DetailContatore.nIdUtente, DetailContatore.nIdContratto)
    '                    '**Ale Cao
    '                    If Len(DetailContatore.sDataSostituzione) > 0 And DataValida = False Then
    '                        clsLetture.ControllaDataSostituzione(DataLettura, DataValida, DetailContatore.sDataSostituzione)
    '                    End If
    '                    '**
    '                    If Not DataValida Then
    '                        dim sScript as string=""
    '                        sscript+="")
    '                        sscript+="parent.Visualizza.frmModifica.txtConsumoTeorico.value='" & ConsumoTeorico.ToString & "';"
    '                        sscript+="parent.Visualizza.frmModifica.txtGGConsumo.value='" & GiorniDiConsumo & "';"
    '                        sscript+="parent.Visualizza.frmModifica.txtLetturaTeorica.value='" & LetturaTeorica & "';"
    '                        sscript+="")
    '                        RegisterScript(sScript , Me.GetType())
    '                    Else
    '                        dim sScript as string=""
    '                        sScript +="")
    '                        sScript +="parent.Visualizza.ErrorDataLettura();")
    '                        sScript +="")
    '                        RegisterScript(sScript , Me.GetType())
    '                    End If
    '                Else
    '                    dim sScript as string=""
    '                    sscript+="")
    '                    sscript+="parent.Visualizza.frmModifica.txtConsumoTeorico.value=0;")
    '                    sscript+="parent.Visualizza.frmModifica.txtGGConsumo.value=0;")
    '                    sscript+="parent.Visualizza.frmModifica.txtGGConsumo.disabled=false;")
    '                    sscript+="parent.Visualizza.frmModifica.txtLetturaTeorica.value=0;")
    '                    sscript+="")
    '                    RegisterScript(sScript , Me.GetType())
    '                End If
    '            Else
    '                clsLetture.ControllaDataLettura(CStr(ContatoreID), DataLettura, ConsumoTeorico, _
    '                GiorniDiConsumo, _
    '                LetturaTeorica, _
    '                DataValida, DetailContatore.nIdUtente, DetailContatore.nIdContratto)
    '                If Not DataValida Then
    '                    dim sScript as string=""
    '                    sscript+="")
    '                    sscript+="parent.Visualizza.frmModifica.txtConsumoTeorico.value='" & ConsumoTeorico.ToString & "';"
    '                    sscript+="parent.Visualizza.frmModifica.txtGGConsumo.value='" & GiorniDiConsumo & "';"
    '                    sscript+="parent.Visualizza.frmModifica.txtLetturaTeorica.value='" & LetturaTeorica & "';"
    '                    sscript+="")
    '                    RegisterScript(sScript , Me.GetType())
    '                Else
    '                    dim sScript as string=""
    '                    sScript +="")
    '                    sScript +="parent.Visualizza.ErrorDataLettura();")
    '                    sScript +="")
    '                    RegisterScript(sScript , Me.GetType())
    '                End If
    '            End If
    '        End If
    '        '*********************************************************************************************************************
    '        '''''''''''''''''''''''''''''''MODIFICA  LETTURA''''''''''''''''''''''''''''''''''''''''''
    '        '*********************************************************************************************************************
    '        If LetturaID > 0 Then
    '            If utility.stringoperation.formatint(ModDate.GiraData(DettaglioLetture.tDataLetturaAtt)) = 0 Then
    '                dim sScript as string=""
    '                sscript+="")
    '                sscript+="parent.Visualizza.frmModifica.txtConsumoTeorico.value=0;")
    '                sscript+="parent.Visualizza.frmModifica.txtGGConsumo.value=0;")
    '                sscript+="parent.Visualizza.frmModifica.txtLetturaTeorica.value=0;")
    '                sscript+="")
    '                RegisterScript(sScript , Me.GetType())
    '                Exit Sub
    '            End If
    '            'Log.Debug("ExecDataLettura::VerificaDataGriglia")
    '            If clsLetture.VerificaDataGriglia(DataLettura, utility.stringoperation.formatint(ModDate.GiraData(DettaglioLetture.tDataLetturaAtt)), CStr(ContatoreID), _
    '            DetailContatore.nIdUtente, _
    '            DataLetturaSuccessiva, _
    '            DataLetturaPrecedente) = False Then
    '                dim sScript as string=""
    '                sScript +="")
    '                sScript +="parent.Visualizza.ErrorDataLettura();")
    '                sScript +="")
    '                RegisterScript(sscript,me.gettype())
    '            Else
    '                'Aggiorno i giorni di consumo rispetto alla lettura precedente e LetturaTeorica e Consumo teorico
    '                GestLetture.ControllaDataLettura(CStr(ContatoreID), DettaglioLetture.tDataLetturaAtt, DataLettura, ConsumoTeorico, GiorniDiConsumo, LetturaTeorica, DetailContatore.nIdUtente, DetailContatore.nIdContratto)
    '                dim sScript as string=""
    '                sscript+="")
    '                sscript+="parent.Visualizza.frmModifica.txtConsumoTeorico.value='" & ConsumoTeorico.ToString & "';"
    '                sscript+="parent.Visualizza.frmModifica.txtGGConsumo.value='" & GiorniDiConsumo & "';"
    '                sscript+="parent.Visualizza.frmModifica.txtLetturaTeorica.value='" & LetturaTeorica & "';"
    '                sscript+="")
    '                RegisterScript(sScript , Me.GetType())
    '            End If
    '        End If
    '    Catch Err As Exception
    '       
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ExecDataLettura.Page_Load.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    Finally
    '        WFSessione.Kill()
    '    End Try
    'End Sub
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim GiorniDiConsumo, LetturaTeorica, ConsumoTeorico, DataLetturaSuccessiva, DataLetturaPrecedente As Long
        Dim DataValida As Boolean
        Dim DataLettura As String
        Dim sScript As String = ""

        Try
            DataValida = False
            ContatoreID = StringOperation.FormatInt(Request("IDCONTATORE"))
            LetturaID = StringOperation.FormatInt(Request("IDLETTURA"))

            DataLettura = Request("DATALETTURA")

            Dim DetailContatore As New objContatore
            DetailContatore = DBContatori.GetDetailsContatori(ContatoreID, -1)
            Dim DettaglioLetture As New ObjLettura
            DettaglioLetture = GestLetture.GetDettaglioLetture(LetturaID, ContatoreID, -1, Date.MaxValue, -1, False)
            'Log.Debug("ExecDataLettura::prelevato contatori e letture")
            '********************************************************************************
            'NUOVA LETTURA
            '********************************************************************************
            If LetturaID = 0 Then
                '********************************************************************************
                'SE RITORNA FALSE NON CI SONO LETTURE PRECEDENTE
                '********************************************************************************
                If Not GestLetture.VerificaEsistenzaLettura(ContatoreID, DetailContatore.nIdUtente) Then
                    If Len(DetailContatore.sDataAttivazione) > 0 Then
                        clsLetture.ControllaDataAttivazione(CStr(ContatoreID), DataLettura, DetailContatore.sDataAttivazione, ConsumoTeorico,
                        GiorniDiConsumo,
                        LetturaTeorica,
                        DataValida, DetailContatore.nIdUtente, DetailContatore.nIdContratto)
                        '**Ale Cao
                        If Len(DetailContatore.sDataSostituzione) > 0 And DataValida = False Then
                            clsLetture.ControllaDataSostituzione(DataLettura, DataValida, DetailContatore.sDataSostituzione)
                        End If
                        '**
                        If Not DataValida Then
                            sScript += "parent.Visualizza.document.getElementById('txtConsumoTeorico').value='" & ConsumoTeorico.ToString & "';"
                            sScript += "parent.Visualizza.document.getElementById('txtGGConsumo').value='" & GiorniDiConsumo & "';"
                            sScript += "parent.Visualizza.document.getElementById('txtLetturaTeorica').value='" & LetturaTeorica & "';"

                            RegisterScript(sScript, Me.GetType())
                        Else

                            sScript += "parent.Visualizza.ErrorDataLettura();"
                            RegisterScript(sScript, Me.GetType())
                        End If
                    Else
                        sScript += "parent.Visualizza.document.getElementById('txtConsumoTeorico').value=0;"
                        sScript += "parent.Visualizza.document.getElementById('txtGGConsumo').value=0;"
                        sScript += "parent.Visualizza.document.getElementById('txtGGConsumo').disabled=false;"
                        sScript += "parent.Visualizza.document.getElementById('txtLetturaTeorica').value=0;"

                        RegisterScript(sScript, Me.GetType())
                    End If
                Else
                    'clsLetture.ControllaDataLettura(CStr(ContatoreID), DataLettura, ConsumoTeorico, GiorniDiConsumo, LetturaTeorica, DataValida, DetailContatore.nIdUtente, DetailContatore.nIdContratto)
                    Call New GestLetture().ControllaDataLettura(ContatoreID, DataLettura, ConsumoTeorico, GiorniDiConsumo, LetturaTeorica, DetailContatore.nIdUtente, DetailContatore.nIdContratto)
                    If Not DataValida Then
                        sScript += "parent.Visualizza.document.getElementById('txtConsumoTeorico').value='" & ConsumoTeorico.ToString & "';"
                        sScript += "parent.Visualizza.document.getElementById('txtGGConsumo').value='" & GiorniDiConsumo & "';"
                        sScript += "parent.Visualizza.document.getElementById('txtLetturaTeorica').value='" & LetturaTeorica & "';"
                        RegisterScript(sScript, Me.GetType())
                    Else
                        sScript = "parent.Visualizza.ErrorDataLettura();"
                        RegisterScript(sScript, Me.GetType())
                    End If
                End If
            End If
            '*********************************************************************************************************************
            '''''''''''''''''''''''''''''''MODIFICA  LETTURA''''''''''''''''''''''''''''''''''''''''''
            '*********************************************************************************************************************
            If LetturaID > 0 Then
                If utility.stringoperation.formatint(ModDate.GiraData(DettaglioLetture.tDataLetturaAtt)) = 0 Then
                    sScript += "parent.Visualizza.document.getElementById('txtConsumoTeorico').value=0;"
                    sScript += "parent.Visualizza.document.getElementById('txtGGConsumo').value=0;"
                    sScript += "parent.Visualizza.document.getElementById('txtLetturaTeorica').value=0;"
                    RegisterScript(sScript, Me.GetType())
                    Exit Sub
                End If
                'Log.Debug("ExecDataLettura::VerificaDataGriglia")
                If clsLetture.VerificaDataGriglia(DataLettura, utility.stringoperation.formatint(ModDate.GiraData(DettaglioLetture.tDataLetturaAtt)), CStr(ContatoreID),
                DetailContatore.nIdUtente,
                DataLetturaSuccessiva,
                DataLetturaPrecedente) = False Then
                    sScript = "parent.Visualizza.ErrorDataLettura();"
                    RegisterScript(sScript, Me.GetType())
                Else
                    'Aggiorno i giorni di consumo rispetto alla lettura precedente e LetturaTeorica e Consumo teorico
                    GestLetture.ControllaDataLettura(CStr(ContatoreID), DataLettura, ConsumoTeorico, GiorniDiConsumo, LetturaTeorica, DetailContatore.nIdUtente, DetailContatore.nIdContratto)
                    sScript += "parent.Visualizza.document.getElementById('txtConsumoTeorico').value='" & ConsumoTeorico.ToString & "';"
                    sScript += "parent.Visualizza.document.getElementById('txtGGConsumo').value='" & GiorniDiConsumo & "';"
                    sScript += "parent.Visualizza.document.getElementById('txtLetturaTeorica').value='" & LetturaTeorica & "';"
                    RegisterScript(sScript, Me.GetType())
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ExecDataLettura.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
