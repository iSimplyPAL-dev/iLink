Imports System.Xml
Imports System.Data
Imports System.Text
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Reflection
Imports AnagInterface
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports RemotingInterfaceMotoreH2O.RemotingInterfaceMotoreH2O
Imports log4net
Imports Utility

Partial Class ModLettureHome
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ModLettureHome))
    Protected FncGrd As New ClsGenerale.FunctionGrd
    Private ModDate As New ClsGenerale.Generale
    Dim LetturaID As Integer
    Dim ContatoreID As Integer
    Dim CodUtente As Integer
    Dim _Const As New Costanti
    Dim GestLetture As GestLetture = New GestLetture
    Dim DBContatori As GestContatori = New GestContatori
    'Dim GestLettureStoricheDEContatori As DEContatori.GestLettureStoricheDEContatori = New DEContatori.GestLettureStoricheDEContatori
    Dim clsLetture As New clsLetture
    Dim clsDBAcces As New DBAccess.getDBobject
    Dim Generali As New Generali
    'Dim _Err As New Errori
    Private Shared m_lngIDPAdvMyDatiE As Long
    Private Shared m_lngDataGriglia, m_lngLettura As Long
    Private Shared m_strCodLettura, m_strDataLetturaGriglia, m_strLetturaGriglia, m_strFatturazioneSospesaGriglia, m_strFatturazioneGriglia, m_strIncongruenteForzatoGriglia, m_strCodModalitaLetturaGriglia, m_strIdStatoLetturaGriglia, m_strCodLetturaGriglia As String
    Dim TypeOfRI As Type = GetType(IH2O)
    Dim RemoRuoloH2O As IH2O

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtPeriodo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtEnte As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkIncongruenteForzatoGrid As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkDARicontrollare As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkAttivaAnomalie As System.Web.UI.WebControls.CheckBox

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    Dim dvMyDatiAnomalie, dvMyDati, dvMyDatiModalitaLettura, dvMyDatiStatoLettura As new dataview
    '    Dim AnomalieApplicate As New ListBox
    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Dim WFErrore As String

    '    Try
    '        'inizializzo la connessione
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        Dim lngRecordConut As Long

    '        LetturaID = stringoperation.formatint(request("IDLettura"))
    '        ContatoreID = stringoperation.formatint(request("IDCONTATORE"))
    '        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '        Dim DetailContatore As New objContatore
    '        DetailContatore = DBContatori.GetDetailsContatori(ContatoreID, WFSessione, CInt(ConstSession.IdEnte), ConstSession.CodIstat)

    '        Dim DettaglioLetture As New ObjLettura
    '        Dim bLastLettura As Boolean
    '        If LetturaID > 0 Then
    '            bLastLettura = False
    '        Else
    '            bLastLettura = True
    '        End If
    '        DettaglioLetture = GestLetture.GetDettaglioLetture(LetturaID, ContatoreID, -1, Date.MinValue, -1, bLastLettura)
    '        Dim COD_TRIBUTO As String = Session("COD_TRIBUTO")
    '        Dim oAnagrafica As DLL.GestioneAnagrafica

    '        'devo creare la sessione al workflow per la dllanagrafica
    '        Dim oSM As New RIBESFrameWork.SessionManager(Session("PARAMETROENV"))
    '        Dim oSession As RIBESFrameWork.Session

    '        If oSM.Initialize(ConstSession.UserName, Session("PARAMETROENV")) Then

    '            oSession = oSM.CreateSession(ConfigurationManager.AppSettings("IDENTIFICATIVOAPPLICAZIONE").ToString())

    '            If oSession Is Nothing Then
    '                'Errore creazione Session
    '            Else
    '                If oSession.oErr.Number <> 0 Then
    '                    'Errore
    '                Else
    '                    oAnagrafica = New Anagrafica.DLL.GestioneAnagrafica()(oSession, ConfigurationManager.AppSettings("PARAMETRO_ANAGRAFICA").ToString())
    '                End If
    '            End If
    '        End If

    '        Dim oDettaglioAnagraficaUtente As New DettaglioAnagrafica
    '        'oDettaglioAnagraficaUtente = oAnagrafica.GetAnagrafica(DetailContatore.nidutente, COD_TRIBUTO, -1)
    '        oDettaglioAnagraficaUtente = oAnagrafica.GetAnagrafica(DetailContatore.nIdUtente, COD_TRIBUTO, -1)

    '        dim sScript as string=""

    '        dim sScript as string=""
    '        sScript +="")
    '        sScript +="document.frmModifica.IDContatore.value='" & ContatoreID & "';")
    '        sScript +="document.frmModifica.IDLettura.value='" & LetturaID & "';")
    '        If LetturaID > 0 Then
    '            sScript +="document.frmModifica.hdDataLettura.value='" & DettaglioLetture.tDataLetturaAtt & "';")
    '            sScript +="document.frmModifica.hdLettura.value='" & DettaglioLetture.nLetturaAtt & "';")
    '        End If
    '        sScript +="")
    '        RegisterScript(sscript,me.gettype())
    '        

    '        '========================================Gestione Anomalie==============================================
    '        sscript+="LettureLabel.style.display='';")
    '        sscript+="LettureField.style.display='';")

    '        sscript+="LettureFieldAnomalie.style.display='';")


    '        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
    '        If Not Page.IsPostBack Then
    '            Dim nConsumoSub As Integer
    '            nConsumoSub = GestLetture.GetConsumoSubContatore(ContatoreID)
    '            If nConsumoSub >= 0 Then
    '                txtSubConsumo.Text = nConsumoSub
    '            End If

    '            dvMyDatiAnomalie = GestLetture.getListAnomalie()

    '            lstAnomalie.DataSource = dvMyDatiAnomalie
    '            lstAnomalie.DataTextField = "DESCRIZIONE"
    '            lstAnomalie.DataValueField = "CODANOMALIA"
    '            lstAnomalie.DataBind()
    '            lstAnomalie.SelectedIndex = 0
    '            If LetturaID > 0 Then
    '                Dim li As ListItem
    '                For Each li In AnomalieApplicate.Items
    '                    lstAnomalieScelte.Items.Add(New ListItem(li.Text, li.Value.ToString))
    '                Next
    '                If AnomalieApplicate.Items.Count > 0 Then
    '                    '	txtDatadiLettura.Enabled = False
    '                    '	txtLetturaAttuale.Enabled = False
    '                    '	cboModalitaLettura.Enabled = False
    '                    '	cboStatoLetturaPage.Enabled = False
    '                    '	chkLasciatoAvviso.Enabled = False
    '                    '	chkDARicontrollare.Enabled = False
    '                    '	chkFatturazioneSosp.Enabled = False
    '                    '	chkFatturazioneSosp.Checked = False
    '                    '	chkLasciatoAvviso.Checked = False
    '                    '	chkDARicontrollare.Checked = False
    '                    '	txtIDAnomalia.Text = "3"
    '                End If
    '            End If
    '            If DettaglioLetture.tDataPassaggio <> Date.MinValue Then
    '                txtDataPassaggio.Text = DettaglioLetture.tDataPassaggio
    '            End If
    '            'chkLasciatoAvviso.Checked = DettaglioLetture.LasciatoAvviso
    '            'chkFatturazioneSosp.Checked = DettaglioLetture.FatturazioneSospesa
    '        End If

    '        '========================================Fine Gestione Anomalie===============================================
    '        Select Case stringoperation.formatint(request("PAG_PREC"))

    '            Case costanti.enmcontesto.DELETTURE

    '            Case costanti.enmcontesto.DECONTATORI
    '                sscript+="ContatoriLabel.style.display='';")
    '                sscript+="ContatoriField.style.display='';")
    '                sscript+="SelezionePeriodo.style.display='';")
    '                sscript+="cboSelezionePeriodo.style.display='';")
    '                If Not Page.IsPostBack Then
    '                    ''''chkFatturazione.Checked = DettaglioLetture.Fatturazione
    '                    chkFatturazione.Checked = DettaglioLetture.bIsFatturata
    '                    chkFatturazioneSospesa.Checked = DettaglioLetture.bFattSospesa
    '                    chkIncongruenteForzato.Checked = DettaglioLetture.bIsIncongruenteForzato
    '                    chkGiroContatore.Checked = DettaglioLetture.bIsGiroContatore

    '                    dvMyDati = GestLetture.getListPeriodo(ConstSession.IdEnte)
    '                    FilldvMyDatiopDownSQL(cboSelezionePeriodo, dvMyDati, DettaglioLetture.nIdPeriodo)

    '                    cboSelezionePeriodo.SelectedItem.Value = DettaglioLetture.nIdPeriodo
    '                End If
    '        End Select

    '        
    '        RegisterScript(sScript , Me.GetType())

    '        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '        lblContatore.Text = "  " & "Lettura associata al contatore Matricola :  " & DetailContatore.sMatricola & " - Utente : " & oDettaglioAnagraficaUtente.Cognome & " " & oDettaglioAnagraficaUtente.Nome

    '        If Not Page.IsPostBack Then
    '            dvMyDatiModalitaLettura = GestLetture.getListModalitaLettura()
    '            FilldvMyDatiopDownSQL(cboModalitaLettura, dvMyDatiModalitaLettura, DettaglioLetture.nCodModoLett)
    '            dvMyDatiStatoLettura = GestLetture.getListStatoLetture()
    '            FilldvMyDatiopDownSQL(cboStatoLetturaPage, dvMyDatiStatoLettura, DettaglioLetture.nIdStatoLettura)
    '            If bLastLettura = True Then
    '                If DettaglioLetture.tDataLetturaAtt <> Date.MinValue Then
    '                    txtDatadiLetturaPrec.Text = DettaglioLetture.tDataLetturaAtt
    '                End If
    '                txtLetturaAttualePrec.Text = DettaglioLetture.nLetturaAtt
    '            Else
    '                If DettaglioLetture.tDataLetturaPrec <> Date.MinValue Then
    '                    txtDatadiLetturaPrec.Text = DettaglioLetture.tDataLetturaPrec
    '                End If
    '                txtLetturaAttualePrec.Text = DettaglioLetture.nLetturaPrec
    '                If DettaglioLetture.tDataLetturaAtt <> Date.MinValue Then
    '                    txtDatadiLettura.Text = DettaglioLetture.tDataLetturaAtt
    '                End If
    '                txtLetturaAttuale.Text = DettaglioLetture.nLetturaAtt
    '                If DettaglioLetture.nGiorni <> -1 Then
    '                    txtGGConsumo.Text = DettaglioLetture.nGiorni
    '                End If
    '                txtLetturaTeorica.Text = DettaglioLetture.sLetturaTeorica
    '                If DettaglioLetture.nConsumo <> -1 Then
    '                    txtConsEffettivo.Text = DettaglioLetture.nConsumo
    '                End If
    '                If DettaglioLetture.nConsumoTeorico <> -1 Then
    '                    txtConsumoTeorico.Text = DettaglioLetture.nConsumoTeorico
    '                End If
    '            End If
    '        End If
    '        txtNoteLettura.Text = DettaglioLetture.sNote
    '        'dipe 04/11/2009 aggiunto controllo su lettura fatturata
    '        If txtDatadiLettura.Enabled = True And Request.Item("IsFatturata") <> "1" Then
    '            dim sScript as string=""
    '            sScript +="")

    '            Select Case stringoperation.formatint(request("PAG_PREC"))
    '                Case costanti.enmcontesto.DELETTURE
    '                    sScript +="document.frmModifica.txtDatadiLettura.focus();")
    '                    sScript +="document.frmModifica.txtDatadiLettura.select();")
    '                    sScript +="")

    '                Case costanti.enmcontesto.DECONTATORI
    '                    sScript +="document.frmModifica.cboSelezionePeriodo.focus();")
    '                    sScript +="document.frmModifica.cboSelezionePeriodo.select();")

    '                    sScript +="")
    '            End Select
    '            RegisterScript(sscript,me.gettype())
    '        End If

    '        If Len(Trim(DettaglioLetture.tDataPassaggio)) > 0 Then

    '        End If
    '        btnConferma.Attributes.Add("OnClick", "return VerificaConsumoNegativo()")

    '        'se la lettura è stata fatturata blocco la modifica/cancellazione
    '        If Request.Item("IsFatturata") = "1" Then
    '            txtDatadiLettura.Enabled = False
    '            txtLetturaAttuale.Enabled = False
    '            txtGGConsumo.Enabled = False
    '            txtLetturaTeorica.Enabled = False
    '            txtConsEffettivo.Enabled = False
    '            cboModalitaLettura.Enabled = False
    '            cboStatoLetturaPage.Enabled = False
    '            txtConsumoTeorico.Enabled = False
    '            txtNoteLettura.Enabled = False
    '            chkFatturazione.Enabled = False
    '            chkFatturazioneSospesa.Enabled = False
    '            txtDataPassaggio.Enabled = False
    '            chkLasciatoAvviso.Enabled = False
    '            lstAnomalie.Enabled = False
    '            lstAnomalieScelte.Enabled = False
    '            GrdLettureHome.Enabled = False
    '            chkIncongruenteForzato.Enabled = False
    '            Button1.Enabled = False
    '            Button2.Enabled = False
    '            txtIDAnomalia.Enabled = False
    '            cboSelezionePeriodo.Enabled = False
    '            chkGiroContatore.Enabled = False
    '            chkConsumoNegativoForzato.Enabled = False
    '        End If

    '    Catch Err As Exception
    '        lblError.Text = Err.Message
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ModLettureHome.Page_Load.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    Finally
    '        WFSessione.Kill()
    '    End Try
    'End Sub
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim myContatore As New objContatore
        Dim myLettura As New ObjLettura
        Dim bLastLettura As Boolean = False
        Try
            LetturaID = Utility.StringOperation.FormatInt(Request("IDLettura"))
            ContatoreID = Utility.StringOperation.FormatInt(Request("IDCONTATORE"))
            If Not Page.IsPostBack Then
                myContatore = DBContatori.GetDetailsContatori(ContatoreID, -1) ', CInt(ConstSession.IdEnte))
                If Not myContatore Is Nothing Then
                    If LetturaID > 0 Then
                        bLastLettura = False
                    Else
                        bLastLettura = True
                    End If
                    myLettura = GestLetture.GetDettaglioLetture(LetturaID, ContatoreID, -1, Date.MaxValue, -1, bLastLettura)
                    If Not myLettura Is Nothing Then
                        ''prelevo i dati della lettura precedente
                        'If GestLetture.getDatiLetturaPrecedente(myLettura.tDataLetturaAtt, myLettura.nIdContatore, myLettura.nLetturaPrec, myLettura.tDataLetturaPrec) < 1 Then
                        '    Throw New Exception("errore in prelievo dati della lettura precedente")
                        'End If
                        LoadHidden(myLettura.tDataLetturaAtt, myLettura.nLetturaAtt)
                        Dim nConsumoSub As Integer
                        nConsumoSub = GestLetture.GetConsumoSubContatore(ContatoreID)
                        If nConsumoSub >= 0 Then
                            txtSubConsumo.Text = nConsumoSub
                        End If
                        LoadAnomalie()
                        LoadDati(myContatore, myLettura)
                        If myLettura.tDataPassaggio.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                            txtDataPassaggio.Text = myLettura.tDataPassaggio
                        End If
                        If StringOperation.FormatInt(Request("PAG_PREC")) = Costanti.enmContesto.DECONTATORI Then
                            ''''chkFatturazione.Checked = DettaglioLetture.Fatturazione
                            chkFatturazione.Checked = myLettura.bIsFatturata
                            chkFatturazioneSospesa.Checked = myLettura.bFattSospesa
                            chkIncongruenteForzato.Checked = myLettura.bIsIncongruenteForzato
                            chkGiroContatore.Checked = myLettura.bIsGiroContatore

                            Dim dvMyDati As New DataView
                            dvMyDati = GestLetture.getListPeriodo(ConstSession.IdEnte)
                            FilldvMyDatiopDownSQL(cboSelezionePeriodo, dvMyDati, myLettura.nIdPeriodo)

                            cboSelezionePeriodo.SelectedItem.Value = myLettura.nIdPeriodo
                        End If

                        'dipe 04/11/2009 aggiunto controllo su lettura fatturata
                        If txtDatadiLettura.Enabled = True And Request.Item("IsFatturata") <> "1" Then
                            Dim sScript As String = ""

                            Select Case StringOperation.FormatInt(Request("PAG_PREC"))
                                Case Costanti.enmContesto.DELETTURE
                                    sScript += "document.getElementById('txtDatadiLettura').focus();"
                                    sScript += "document.getElementById('txtDatadiLettura').select();"

                                Case Costanti.enmContesto.DECONTATORI
                                    sScript += "document.getElementById('cboSelezionePeriodo').focus();"
                                    sScript += "document.getElementById('cboSelezionePeriodo').select();"
                            End Select
                            RegisterScript(sScript, Me.GetType())
                        End If
                        'btnConferma.Attributes.Add("OnClick", "return VerificaConsumoNegativo()")
                        'se la lettura è stata fatturata blocco la modifica/cancellazione
                        If Request.Item("IsFatturata") = "1" Then
                            LockVideata()
                        End If
                    End If
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ModLettureHone.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    'Private Sub btnAppoggio_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAppoggio.Click
    '    Dim Inconguente, LetturaErrata, ConsumoNegativo As Boolean
    '    Dim blnStorico As Boolean = True
    '    Dim blnGestioneAnomalieConDataPassaggio As Boolean = False
    '    Dim blnGestioneAnomalieSenzaLettura As Boolean = False
    '    Dim blnGestioneLettureNormale As Boolean = False
    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Dim WFErrore As String

    '    Try
    '        'inizializzo la connessione
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        ContatoreID = stringoperation.formatint(request("IDCONTATORE"))

    '        LetturaID = stringoperation.formatint(request("IDLettura"))

    '        Dim DetailContatore As New objContatore
    '        DetailContatore = DBContatori.GetDetailsContatori(ContatoreID, WFSessione)

    '        '******************************************************************
    '        'VERIFICA SE IL PERIODO IN USO E' UN PERIODO STORICO
    '        ''******************************************************************
    '        Select Case stringoperation.formatint(request("PAG_PREC"))

    '            Case costanti.enmcontesto.DELETTURE

    '                blnStorico = False

    '                If Len(Trim(txtDataPassaggio.Text)) > 0 Or chkLasciatoAvviso.Checked Then
    '                    blnGestioneAnomalieConDataPassaggio = True
    '                End If

    '                If Len(Trim(txtDatadiLettura.Text)) > 0 And Len(Trim(txtLetturaAttuale.Text)) = 0 And lstAnomalieScelte.Items.Count > 0 Then
    '                    blnGestioneAnomalieSenzaLettura = True
    '                End If
    '                If Not blnGestioneAnomalieConDataPassaggio And Not blnGestioneAnomalieSenzaLettura Then
    '                    blnGestioneLettureNormale = True
    '                End If

    '                If Not blnGestioneAnomalieConDataPassaggio Then

    '                    'NUOVA LETTURA
    '                    If LetturaID = 0 Then
    '                        '***********************************************************************************************************
    '                        'VERIFICA SE LA LETTURA INSERITA E' MINORE DELLA LETTURA PRECEDENTE IN QUESTO CASO
    '                        'SEGNALA UN CONSUMO NEGATIVO
    '                        '***********************************************************************************************************
    '                        If Not blnGestioneAnomalieSenzaLettura Then
    '                            clsLetture.VerificaLettura(utility.stringoperation.formatstring(ModDate.GiraData(txtDatadiLettura.Text)), txtLetturaAttuale.Text, _
    '                             ContatoreID, DetailContatore.nIdUtente, LetturaErrata, ConsumoNegativo)
    '                        End If

    '                    End If

    '                End If

    '                If Not blnGestioneAnomalieConDataPassaggio Then
    '                    'MODIFICA LETTURA
    '                    '**************************************************************************************************************
    '                    'LA FUNZIONE VIENE UTILIZZATA SE MODIFICO UNA LETTURA DALLA GRIGLIA DI VISUALIZZAZIONE LETTURE
    '                    '**************************************************************************************************************
    '                    If LetturaID > 0 Then
    '                        'If Not blnGestioneAnomalieSenzaLettura Then
    '                        '  clsLetture.VerificaLetturaGriglia(utility.stringoperation.formatint(ModDate.GiraData(Request("hdDataLettura"))), utility.stringoperation.formatint(Request("hdLettura")), txtLetturaAttuale.Text, ContatoreID, DetailContatore.nidutente, ConsumoNegativo, LetturaErrata)
    '                        'End If
    '                    End If
    '                    '***************************************************************************************************************
    '                End If

    '                If Not blnGestioneAnomalieConDataPassaggio Then
    '                    'SE IL CONTROLLO RESTITUISCE UN CONSUMO NEGATIVO....
    '                    If Not blnGestioneAnomalieSenzaLettura Then
    '                        'If ConsumoNegativo Then
    '                        '  dim sScript as string=""
    '                        '  
    '                        '  sscript+="ConfermaConsumoNegativo();")
    '                        '  
    '                        '  RegisterScript(sScript , Me.GetType())
    '                        '  Exit Sub
    '                        'End If
    '                    End If

    '                    '***************************************************************************************************************
    '                End If

    '                'If ConsumoNegativo Then
    '                '  dim sScript as string=""
    '                '  
    '                '  sscript+="ConfermaConsumoNegativo();")
    '                '  
    '                '  RegisterScript(sScript , Me.GetType())
    '                '  Exit Sub
    '                'End If

    '                If LetturaErrata Then
    '                    'dim sScript as string=""
    '                    '
    '                    'sscript+="GestAlert('a', 'warning', '', '', 'Attenzione:lettura Inserita Errata!!');")
    '                    '
    '                    'RegisterScript(sScript , Me.GetType())
    '                    'Exit Sub
    '                End If

    '                If Not blnGestioneAnomalieConDataPassaggio Then
    '                    'SE UNA LETTURA ERRATA....
    '                    'UNA LETTURA ERRATA....
    '                    If Not blnGestioneAnomalieSenzaLettura Then
    '                        If LetturaErrata Then
    '                            'dim sScript as string=""
    '                            '
    '                            'sscript+="GestAlert('a', 'warning', '', '', 'Attenzione:lettura Inserita Errata!!');")
    '                            '
    '                            'RegisterScript(sScript , Me.GetType())
    '                            'Exit Sub
    '                        End If
    '                    End If
    '                End If

    '                'If blnGestioneAnomalieConDataPassaggio Then
    '                'MemoLettureNormaliATTUALI(txtDataPassaggio.Text, False)
    '                'End If

    '                'If blnGestioneAnomalieSenzaLettura Then
    '                'MemoLettureNormaliATTUALI(False, False)
    '                'End If
    '                'If blnGestioneLettureNormale Then
    '                If MemoLetture() = False Then

    '                End If
    '                'End If

    '                '*************************************************************************************************************
    '                'PARTE RIGUARDANTE IL DATAENTRY DELLE LETTURE DA DATAENTRY CONTATORI
    '                'AGGIUNTA LA GESTIONE DELLE ANOMALIE
    '                '*************************************************************************************************************
    '            Case costanti.enmcontesto.DECONTATORI

    '                '*************************************************************************************************************
    '                'QUESTA FUNZIONE CONSENTE DI VERIFICARE SE IL PERIODO DI FATTURAZIONE UTILIZZATO PER L'INSERIMENTO DELLE LETTURE
    '                'E' STORICO
    '                'SE IL PERIODO E' STORICO NON VENGONO ESEGUITI I CONTROLLI 
    '                '*************************************************************************************************************
    '                ' If Not GestLetture.VerificaPeriodo(CInt(cboSelezionePeriodo.SelectedItem.Value)) Then
    '                '//Parametro da passare alla funzione MemoLetture
    '                blnStorico = False
    '                '====================================================================================
    '                '====================================================================================
    '                If Len(Trim(txtDataPassaggio.Text)) > 0 Or chkLasciatoAvviso.Checked Then
    '                    blnGestioneAnomalieConDataPassaggio = True
    '                End If

    '                If Len(Trim(txtDatadiLettura.Text)) > 0 And Len(Trim(txtLetturaAttuale.Text)) = 0 And lstAnomalieScelte.Items.Count > 0 Then
    '                    blnGestioneAnomalieSenzaLettura = True
    '                End If
    '                If Not blnGestioneAnomalieConDataPassaggio And Not blnGestioneAnomalieSenzaLettura Then
    '                    blnGestioneLettureNormale = True
    '                End If

    '                '====================================================================================

    '                If Not blnGestioneAnomalieConDataPassaggio Then

    '                    'NUOVA LETTURA
    '                    If LetturaID = 0 Then
    '                        '***********************************************************************************************************
    '                        'VERIFICA SE LA LETTURA INSERITA E' MINORE DELLA LETTURA PRECEDENTE IN QUESTO CASO
    '                        'SEGNALA UN CONSUMO NEGATIVO
    '                        '***********************************************************************************************************
    '                        If Not blnGestioneAnomalieSenzaLettura Then
    '                            clsLetture.VerificaLettura(utility.stringoperation.formatstring(ModDate.GiraData(txtDatadiLettura.Text)), txtLetturaAttuale.Text, _
    '                             ContatoreID, DetailContatore.nIdUtente, LetturaErrata, ConsumoNegativo)
    '                        End If

    '                    End If


    '                End If

    '                If Not blnGestioneAnomalieConDataPassaggio Then
    '                    'MODIFICA LETTURA
    '                    '**************************************************************************************************************
    '                    'LA FUNZIONE VIENE UTILIZZATA SE MODIFICO UNA LETTURA DALLA GRIGLIA DI VISUALIZZAZIONE LETTURE
    '                    '**************************************************************************************************************
    '                    If LetturaID > 0 Then
    '                        If Not blnGestioneAnomalieSenzaLettura Then
    '                            clsLetture.VerificaLetturaGriglia(utility.stringoperation.formatint(ModDate.GiraData(Request("hdDataLettura"))), utility.stringoperation.formatint(Request("hdLettura")), txtLetturaAttuale.Text, ContatoreID, DetailContatore.nIdUtente, ConsumoNegativo, LetturaErrata)
    '                        End If
    '                    End If
    '                    '***************************************************************************************************************
    '                End If

    '                If Not blnGestioneAnomalieConDataPassaggio Then
    '                    'SE IL CONTROLLO RESTITUISCE UN CONSUMO NEGATIVO....
    '                    If Not blnGestioneAnomalieSenzaLettura Then
    '                        If ConsumoNegativo Then
    '                            dim sScript as string=""
    '                            
    '                            sscript+="ConfermaConsumoNegativo();")
    '                            
    '                            RegisterScript(sScript , Me.GetType())
    '                            Exit Sub
    '                        End If
    '                    End If

    '                    '***************************************************************************************************************
    '                End If

    '                If Not blnGestioneAnomalieConDataPassaggio Then
    '                    'SE UNA LETTURA ERRATA....
    '                    'UNA LETTURA ERRATA....
    '                    If Not blnGestioneAnomalieSenzaLettura Then
    '                        If LetturaErrata Then
    '                            dim sScript as string=""
    '                            
    '                            sscript+="GestAlert('a', 'warning', '', '', 'Attenzione:lettura Inserita Errata!!');")
    '                            
    '                            RegisterScript(sScript , Me.GetType())
    '                            Exit Sub
    '                        End If
    '                    End If
    '                End If

    '                If blnGestioneAnomalieConDataPassaggio Then
    '                    If MemoLetture() = False Then

    '                    End If
    '                End If

    '                If blnGestioneAnomalieSenzaLettura Then
    '                    If MemoLetture() = False Then

    '                    End If
    '                End If
    '                If blnGestioneLettureNormale Then
    '                    If MemoLetture() = False Then

    '                    End If
    '                End If

    '        End Select
    '        '*********************************************************************************************************************
    '        '******************************************************
    '        'FINE VERIFICA SE IL PERIODO IN USO E' UN PERIODO STORICO
    '        '******************************************************


    '        '*********************************************************************************************************************
    '        'CHIAMATA ALLA FUNZIONE DI AGGIORNAMETO DELLA TABELLA DI OPENutenze TP_LETTURE
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ModLettureHone.btnAppoggio_Click.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    Finally
    '        WFSessione.Kill()
    '    End Try
    'End Sub
    Private Sub btnAppoggio_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAppoggio.Click
        Dim LetturaErrata, ConsumoNegativo As Boolean
        Dim blnStorico As Boolean = True
        Dim blnGestioneAnomalieConDataPassaggio As Boolean = False
        Dim blnGestioneAnomalieSenzaLettura As Boolean = False
        Dim blnGestioneLettureNormale As Boolean = False

        Try
            ContatoreID = StringOperation.FormatInt(Request("IDCONTATORE"))

            LetturaID = StringOperation.FormatInt(Request("IDLettura"))

            Dim DetailContatore As New objContatore
            DetailContatore = DBContatori.GetDetailsContatori(ContatoreID, -1)

            '******************************************************************
            'VERIFICA SE IL PERIODO IN USO E' UN PERIODO STORICO
            ''******************************************************************
            Select Case StringOperation.FormatInt(Request("PAG_PREC"))
                Case Costanti.enmContesto.DELETTURE
                    blnStorico = False

                    If Len(Trim(txtDataPassaggio.Text)) > 0 Or chkLasciatoAvviso.Checked Then
                        blnGestioneAnomalieConDataPassaggio = True
                    End If

                    If Len(Trim(txtDatadiLettura.Text)) > 0 And Len(Trim(txtLetturaAttuale.Text)) = 0 And lstAnomalieScelte.Items.Count > 0 Then
                        blnGestioneAnomalieSenzaLettura = True
                    End If
                    If Not blnGestioneAnomalieConDataPassaggio And Not blnGestioneAnomalieSenzaLettura Then
                        blnGestioneLettureNormale = True
                    End If

                    If Not blnGestioneAnomalieConDataPassaggio Then

                        'NUOVA LETTURA
                        If LetturaID = 0 Then
                            '***********************************************************************************************************
                            'VERIFICA SE LA LETTURA INSERITA E' MINORE DELLA LETTURA PRECEDENTE IN QUESTO CASO
                            'SEGNALA UN CONSUMO NEGATIVO
                            '***********************************************************************************************************
                            If Not blnGestioneAnomalieSenzaLettura Then
                                clsLetture.VerificaLettura(Utility.StringOperation.FormatString(ModDate.GiraData(txtDatadiLettura.Text)), txtLetturaAttuale.Text,
                                 ContatoreID, DetailContatore.nIdUtente, LetturaErrata, ConsumoNegativo)
                            End If
                        End If
                    End If

                    If Not blnGestioneAnomalieConDataPassaggio Then
                        'MODIFICA LETTURA
                        '**************************************************************************************************************
                        'LA FUNZIONE VIENE UTILIZZATA SE MODIFICO UNA LETTURA DALLA GRIGLIA DI VISUALIZZAZIONE LETTURE
                        '**************************************************************************************************************
                        If LetturaID > 0 Then
                            'If Not blnGestioneAnomalieSenzaLettura Then
                            '  clsLetture.VerificaLetturaGriglia(utility.stringoperation.formatint(ModDate.GiraData(Request("hdDataLettura"))), utility.stringoperation.formatint(Request("hdLettura")), txtLetturaAttuale.Text, ContatoreID, DetailContatore.nidutente, ConsumoNegativo, LetturaErrata)
                            'End If
                        End If
                        '***************************************************************************************************************
                    End If

                    If Not blnGestioneAnomalieConDataPassaggio Then
                        'SE IL CONTROLLO RESTITUISCE UN CONSUMO NEGATIVO....
                        If Not blnGestioneAnomalieSenzaLettura Then
                            'If ConsumoNegativo Then
                            '  dim sScript as string=""
                            '  
                            '  sscript+="ConfermaConsumoNegativo();")
                            '  
                            '  RegisterScript(sScript , Me.GetType())
                            '  Exit Sub
                            'End If
                        End If
                        '***************************************************************************************************************
                    End If

                    'If ConsumoNegativo Then
                    '  dim sScript as string=""
                    '  
                    '  sscript+="ConfermaConsumoNegativo();")
                    '  
                    '  RegisterScript(sScript , Me.GetType())
                    '  Exit Sub
                    'End If

                    If LetturaErrata Then
                        'dim sScript as string=""
                        '
                        'sscript+="GestAlert('a', 'warning', '', '', 'Attenzione:lettura Inserita Errata!!');")
                        '
                        'RegisterScript(sScript , Me.GetType())
                        'Exit Sub
                    End If

                    If Not blnGestioneAnomalieConDataPassaggio Then
                        'SE UNA LETTURA ERRATA....
                        'UNA LETTURA ERRATA....
                        If Not blnGestioneAnomalieSenzaLettura Then
                            If LetturaErrata Then
                                'dim sScript as string=""
                                '
                                'sscript+="GestAlert('a', 'warning', '', '', 'Attenzione:lettura Inserita Errata!!');")
                                '
                                'RegisterScript(sScript , Me.GetType())
                                'Exit Sub
                            End If
                        End If
                    End If

                    'If blnGestioneAnomalieConDataPassaggio Then
                    'MemoLettureNormaliATTUALI(txtDataPassaggio.Text, False)
                    'End If

                    'If blnGestioneAnomalieSenzaLettura Then
                    'MemoLettureNormaliATTUALI(False, False)
                    'End If
                    'If blnGestioneLettureNormale Then
                    If MemoLetture(DetailContatore) = False Then

                    End If
                    'End If
                    '*************************************************************************************************************
                    'PARTE RIGUARDANTE IL DATAENTRY DELLE LETTURE DA DATAENTRY CONTATORI
                    'AGGIUNTA LA GESTIONE DELLE ANOMALIE
                    '*************************************************************************************************************
                Case Costanti.enmContesto.DECONTATORI
                    '*************************************************************************************************************
                    'QUESTA FUNZIONE CONSENTE DI VERIFICARE SE IL PERIODO DI FATTURAZIONE UTILIZZATO PER L'INSERIMENTO DELLE LETTURE
                    'E' STORICO
                    'SE IL PERIODO E' STORICO NON VENGONO ESEGUITI I CONTROLLI 
                    '*************************************************************************************************************
                    ' If Not GestLetture.VerificaPeriodo(CInt(cboSelezionePeriodo.SelectedItem.Value)) Then
                    '//Parametro da passare alla funzione MemoLetture
                    blnStorico = False
                    '====================================================================================
                    '====================================================================================
                    If Len(Trim(txtDataPassaggio.Text)) > 0 Or chkLasciatoAvviso.Checked Then
                        blnGestioneAnomalieConDataPassaggio = True
                    End If

                    If Len(Trim(txtDatadiLettura.Text)) > 0 And Len(Trim(txtLetturaAttuale.Text)) = 0 And lstAnomalieScelte.Items.Count > 0 Then
                        blnGestioneAnomalieSenzaLettura = True
                    End If
                    If Not blnGestioneAnomalieConDataPassaggio And Not blnGestioneAnomalieSenzaLettura Then
                        blnGestioneLettureNormale = True
                    End If
                    '====================================================================================
                    If Not blnGestioneAnomalieConDataPassaggio Then
                        'NUOVA LETTURA
                        If LetturaID = 0 Then
                            '***********************************************************************************************************
                            'VERIFICA SE LA LETTURA INSERITA E' MINORE DELLA LETTURA PRECEDENTE IN QUESTO CASO
                            'SEGNALA UN CONSUMO NEGATIVO
                            '***********************************************************************************************************
                            If Not blnGestioneAnomalieSenzaLettura Then
                                clsLetture.VerificaLettura(Utility.StringOperation.FormatString(ModDate.GiraData(txtDatadiLettura.Text)), txtLetturaAttuale.Text,
                                 ContatoreID, DetailContatore.nIdUtente, LetturaErrata, ConsumoNegativo)
                            End If
                        End If
                    End If

                    If Not blnGestioneAnomalieConDataPassaggio Then
                        'MODIFICA LETTURA
                        '**************************************************************************************************************
                        'LA FUNZIONE VIENE UTILIZZATA SE MODIFICO UNA LETTURA DALLA GRIGLIA DI VISUALIZZAZIONE LETTURE
                        '**************************************************************************************************************
                        If LetturaID > 0 Then
                            If Not blnGestioneAnomalieSenzaLettura Then
                                clsLetture.VerificaLetturaGriglia(Utility.StringOperation.FormatInt(ModDate.GiraData(Request("hdDataLettura"))), Utility.StringOperation.FormatInt(Request("hdLettura")), txtLetturaAttuale.Text, ContatoreID, DetailContatore.nIdUtente, ConsumoNegativo, LetturaErrata)
                            End If
                        End If
                        '***************************************************************************************************************
                    End If

                    If Not blnGestioneAnomalieConDataPassaggio Then
                        'SE IL CONTROLLO RESTITUISCE UN CONSUMO NEGATIVO....
                        If Not blnGestioneAnomalieSenzaLettura Then
                            If ConsumoNegativo Then
                                Dim sScript As String = ""

                                sScript += "ConfermaConsumoNegativo();"

                                RegisterScript(sScript, Me.GetType())
                                Exit Sub
                            End If
                        End If
                        '***************************************************************************************************************
                    End If

                    If Not blnGestioneAnomalieConDataPassaggio Then
                        'SE UNA LETTURA ERRATA....
                        'UNA LETTURA ERRATA....
                        If Not blnGestioneAnomalieSenzaLettura Then
                            If LetturaErrata Then
                                Dim sScript As String = ""

                                sScript += "GestAlert('a', 'warning', '', '', 'Attenzione:lettura Inserita Errata!!');"
                                RegisterScript(sScript, Me.GetType())
                                Exit Sub
                            End If
                        End If
                    End If

                    If blnGestioneAnomalieConDataPassaggio Then
                        If MemoLetture(DetailContatore) = False Then

                        End If
                    End If

                    If blnGestioneAnomalieSenzaLettura Then
                        If MemoLetture(DetailContatore) = False Then

                        End If
                    End If
                    If blnGestioneLettureNormale Then
                        If MemoLetture(DetailContatore) = False Then

                        End If
                    End If
            End Select
            '*********************************************************************************************************************
            '******************************************************
            'FINE VERIFICA SE IL PERIODO IN USO E' UN PERIODO STORICO
            '******************************************************
            '*********************************************************************************************************************
            'CHIAMATA ALLA FUNZIONE DI AGGIORNAMETO DELLA TABELLA DI OPENutenze TP_LETTURE
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ModLettureHone.btnAppoggio_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    'Private Sub btnElimina_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnElimina.Click
    '    Dim DetailContatore As New objContatore
    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Dim WFErrore As String

    '    Try
    '        'inizializzo la connessione
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If
    '        ContatoreID = stringoperation.formatint(request("IDCONTATORE"))

    '        LetturaID = stringoperation.formatint(request("IDLettura"))

    '        DetailContatore = DBContatori.GetDetailsContatori(ContatoreID, WFSessione)

    '        GestLetture.DelLetture(LetturaID, ContatoreID, DetailContatore.nIdUtente, txtDatadiLettura.Text, txtLetturaAttuale.Text, txtDataPassaggio.Text)
    '        Select Case stringoperation.formatint(request("PAG_PREC"))

    '            Case costanti.enmcontesto.DELETTURE

    '                dim sScript as string=""

    '                
    '                sscript+="parent.opener.parent.Visualizza.frmHidden.action='Letture.aspx';")
    '                sscript+="parent.opener.parent.Visualizza.frmHidden.submit();")
    '                sscript+="parent.window.close();")
    '                

    '                RegisterScript(sScript , Me.GetType())

    '            Case costanti.enmcontesto.DECONTATORI


    '                dim sScript as string=""

    '                
    '                sscript+="parent.opener.parent.Visualizza.location.href='Letture.aspx?hdIDContatore=" & ContatoreID & "&PAG_PREC=" & stringoperation.formatint(request("PAG_PREC")) & "';")
    '                sscript+="parent.window.close();")
    '                

    '                RegisterScript(sScript , Me.GetType())


    '        End Select
    '    Catch Err As Exception
    '        lblError.Text = Err.Message
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ModLettureHone.btnElimina_Click.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    Finally
    '        WFSessione.Kill()
    '    End Try
    'End Sub

    Private Sub btnElimina_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnElimina.Click
        'Dim DetailContatore As New objContatore
        Dim sScript As String = ""

        Try
            ContatoreID = StringOperation.FormatInt(Request("IDCONTATORE"))
            LetturaID = StringOperation.FormatInt(Request("IDLettura"))
            'DetailContatore = DBContatori.GetDetailsContatori(ContatoreID, -1)
            GestLetture.DelLetture(LetturaID, sScript)
            'GestLetture.DelLetture(LetturaID, ContatoreID, DetailContatore.nIdUtente, txtDatadiLettura.Text, txtLetturaAttuale.Text, txtDataPassaggio.Text)
            If sScript = "" Then
                sScript += "parent.opener.parent.Visualizza.ReloadGrdLetture();"
                sScript += "parent.window.close();"
                RegisterScript(sScript, Me.GetType())
            Else
                lblError.Text = sScript
            End If
        Catch Err As Exception
            lblError.Text = Err.Message
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ModLettureHone.btnElimina_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub SelectIndexdvMyDatiopDownList(ByVal cboTemp As DropDownList, ByVal strValue As String)
        Try
            Dim blnFindElement As Boolean = False
            Dim intCount As Integer = 1
            Dim intNumberElements As Integer = cboTemp.Items.Count
            Do While intCount < intNumberElements
                cboTemp.SelectedIndex = intCount
                If cboTemp.SelectedItem.Value = strValue Then
                    cboTemp.SelectedItem.Text = cboTemp.Items(intCount).Text
                    blnFindElement = True
                    Exit Do
                End If
                intCount = intCount + 1
            Loop
            If Not blnFindElement Then cboTemp.SelectedIndex = "-1"
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ModLettureHone.SelectIndexdvMyDatiopDownList.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Protected Function DescrizioneAnomalia(ByVal prdStatus As Object) As String
        DescrizioneAnomalia = GestLetture.DescrizioneAnomalie(prdStatus)
        Return DescrizioneAnomalia
    End Function

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim strSelection As String
        Dim li As ListItem
        Try
            maxanomalie.Text = ""
            For Each li In lstAnomalie.Items
                If li.Selected Then
                    strSelection = li.Text
                    If lstAnomalieScelte.Items.Count = 3 Then
                        Exit For
                    End If
                    If lstAnomalieScelte.Items.FindByText(strSelection) Is Nothing Then
                        lstAnomalieScelte.Items.Add(New ListItem(li.Text, li.Value.ToString))
                        lstAnomalieScelte.SelectedIndex = 0
                    End If
                End If
            Next

            If lstAnomalieScelte.Items.Count > 0 Then

                'txtDatadiLettura.Text = ""
                'txtLetturaAttuale.Text = ""
                'cboStatoLetturaPage.SelectedIndex = "-1"
                'cboModalitaLettura.SelectedIndex = "-1"
                'txtConsumoTeorico.Text = ""
                'txtLetturaTeorica.Text = ""
                'txtConsEffettivo.Text = ""
                'txtGGConsumo.Text = ""

                'txtDatadiLettura.Enabled = False
                'txtLetturaAttuale.Enabled = False
                'cboModalitaLettura.Enabled = False
                'cboStatoLetturaPage.Enabled = False
                'chkLasciatoAvviso.Enabled = False
                'chkDARicontrollare.Enabled = False
                'chkLasciatoAvviso.Checked = False
                'chkDARicontrollare.Checked = False
                'chkFatturazioneSosp.Enabled = False
                'chkFatturazioneSosp.Checked = False

                txtIDAnomalia.Text = "3"
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ModLettureHone.Button1_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim i As Integer
        Try
            If lstAnomalieScelte.SelectedIndex > -1 Then
                For i = lstAnomalieScelte.Items.Count - 1 To 0 Step -1
                    If lstAnomalieScelte.Items(i).Selected Then
                        lstAnomalieScelte.Items.Remove(lstAnomalieScelte.SelectedItem)
                    End If
                Next
            End If

            If lstAnomalieScelte.Items.Count = 0 And chkLasciatoAvviso.Checked = False Then
                'txtDatadiLettura.Enabled = True
                'txtLetturaAttuale.Enabled = True
                'cboModalitaLettura.Enabled = True
                'cboStatoLetturaPage.Enabled = True
                'chkLasciatoAvviso.Enabled = True
                'chkDARicontrollare.Enabled = True
                ''chkFatturazioneSosp.Enabled = True
                txtIDAnomalia.Text = ""
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ModLettureHone.Button2_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    '***********************************************************************************************
    'CONFERMA I DATI DELLA LETTURA INSERITA
    '***********************************************************************************************
    'Private Sub btnConferma_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConferma.Click
    '    Dim Inconguente, LetturaErrata, ConsumoNegativo As Boolean
    '    Dim blnStorico As Boolean = True
    '    Dim blnGestioneAnomalieConDataPassaggio As Boolean = False
    '    Dim blnGestioneAnomalieSenzaLettura As Boolean = False
    '    Dim blnGestioneLettureNormale As Boolean = False
    '    dim sSQL as string = ""
    '    Dim dvMyDati As SqlClient.new dataview
    '    Dim DBAccess As New DBAccess.getDBobject
    '    Dim sDataPeriodoDal As String = ""
    '    Dim sDataPeriodoAl As String = ""
    '    Dim ModDate As New ClsGenerale.Generale
    '    Dim stringa As String = ""
    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Dim WFErrore As String

    '    Try
    '        'inizializzo la connessione
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        ContatoreID = stringoperation.formatint(request("IDCONTATORE"))

    '        LetturaID = stringoperation.formatint(request("IDLettura"))

    '        'modifica del 14/02/2007
    '        'è necessario verificare che la data di lettura inserito sia interna al periodo di fatturazione
    '        If ConstSession.IdPeriodo <> "" Then
    '            sSQL = "SELECT *"
    '            sSQL += " FROM TP_PERIODO"
    '            sSQL += " WHERE CODPERIODO = " & ConstSession.IdPeriodo
    '            dvMyDati = DBAccess.getdataview(sSQL)
    '            If dvMyDati.Read Then
    '                sDataPeriodoDal = myrow("DADATA")
    '                sDataPeriodoAl = myrow("ADATA")
    '                'controllo se la data è compresa
    '                If ModDate.GiraData(txtDatadiLettura.Text) < sDataPeriodoDal Or ModDate.GiraData(txtDatadiLettura.Text) > sDataPeriodoAl Then
    '                    stringa = ""
    '                    stringa = stringa & "GestAlert('a', 'warning', '', '', 'Controllare la data lettura.Deve essere compresa tra il " & ModDate.GiraDataFromDB(sDataPeriodoDal) & " e il " & ModDate.GiraDataFromDB(sDataPeriodoAl) & "!');"
    '                    stringa = stringa & ""
    '                    RegisterScript(sScript , Me.GetType())
    '                    Exit Sub
    '                End If
    '            End If
    '            dvmydati.dispose()
    '        End If

    '        Dim DetailContatore As New objContatore
    '        DetailContatore = DBContatori.GetDetailsContatori(ContatoreID, WFSessione)
    '        If Len(DetailContatore.sDataAttivazione) = 0 Then
    '            Response.Write("<script language='javascript' type='text/javascript'>GestAlert('a', 'warning', '', '', 'Non è possibile inserire letture su un contatore non attivo');")
    '            Exit Sub
    '        End If

    '        '******************************************************************
    '        'verifico che la data di lettura non sia successiva alla data di cessazione
    '        '******************************************************************
    '        If Len(DetailContatore.sDataCessazione) > 0 Then
    '            If ModDate.GiraData(txtDatadiLettura.Text) > ModDate.GiraData(DetailContatore.sDataCessazione) Then
    '                Response.Write("<script language='javascript' type='text/javascript'>GestAlert('a', 'warning', '', '', 'Non è possibile inserire letture su un contatore cessato');")
    '                Exit Sub
    '            End If
    '        End If
    '        '******************************************************************
    '        'verifico che la data di lettura non sia successiva alla data di rimozione
    '        '******************************************************************
    '        If Len(DetailContatore.sDataRimTemp) > 0 Then
    '            If ModDate.GiraData(txtDatadiLettura.Text) > ModDate.GiraData(DetailContatore.sDataRimTemp) Then
    '                Response.Write("<script language='javascript' type='text/javascript'>GestAlert('a', 'warning', '', '', 'Non è possibile inserire letture su un contatore rimosso');")
    '                Exit Sub
    '            End If
    '        End If
    '        '******************************************************************
    '        'verifico che la data di lettura non sia successiva alla data di sostituzione
    '        '******************************************************************
    '        If Len(DetailContatore.sDataSostituzione) > 0 Then
    '            If ModDate.GiraData(txtDatadiLettura.Text) > ModDate.GiraData(DetailContatore.sDataSostituzione) Then
    '                Response.Write("<script language='javascript' type='text/javascript'>GestAlert('a', 'warning', '', '', 'Non è possibile inserire letture su un contatore sostituito');")
    '                Exit Sub
    '            End If
    '        End If
    '        '******************************************************************
    '        'verifico che la data di lettura non sia successiva alla data di sospensione
    '        '******************************************************************
    '        If Len(DetailContatore.sDataSospensioneUtenza) > 0 Then
    '            If ModDate.GiraData(txtDatadiLettura.Text) > ModDate.GiraData(DetailContatore.sDataSospensioneUtenza) Then
    '                Response.Write("<script language='javascript' type='text/javascript'>GestAlert('a', 'warning', '', '', 'Non è possibile inserire letture su un contatore sospeso');")
    '                Exit Sub
    '            End If
    '        End If

    '        '******************************************************************
    '        'VERIFICA SE IL PERIODO IN USO E' UN PERIODO STORICO
    '        ''******************************************************************
    '        Select Case stringoperation.formatint(request("PAG_PREC"))

    '            Case costanti.enmcontesto.DELETTURE

    '                blnStorico = False

    '                If Len(Trim(txtDataPassaggio.Text)) > 0 Or chkLasciatoAvviso.Checked Then
    '                    blnGestioneAnomalieConDataPassaggio = True
    '                End If

    '                If Len(Trim(txtDatadiLettura.Text)) > 0 And Len(Trim(txtLetturaAttuale.Text)) = 0 And lstAnomalieScelte.Items.Count > 0 Then
    '                    blnGestioneAnomalieSenzaLettura = True
    '                End If
    '                If Not blnGestioneAnomalieConDataPassaggio And Not blnGestioneAnomalieSenzaLettura Then
    '                    blnGestioneLettureNormale = True
    '                End If

    '                If Not blnGestioneAnomalieConDataPassaggio Then

    '                    'NUOVA LETTURA
    '                    If LetturaID = 0 Then
    '                        '***********************************************************************************************************
    '                        'VERIFICA SE LA LETTURA INSERITA E' MINORE DELLA LETTURA PRECEDENTE IN QUESTO CASO
    '                        'SEGNALA UN CONSUMO NEGATIVO
    '                        '***********************************************************************************************************
    '                        If Not blnGestioneAnomalieSenzaLettura Then
    '                            clsLetture.VerificaLettura(utility.stringoperation.formatstring(ModDate.GiraData(txtDatadiLettura.Text)), txtLetturaAttuale.Text, _
    '                             ContatoreID, DetailContatore.nIdUtente, LetturaErrata, ConsumoNegativo)
    '                        End If

    '                    End If

    '                End If

    '                'If LetturaID = 0 Then
    '                '  clsLetture.VerificaLettura(utility.stringoperation.formatstring(ModDate.GiraData(txtDatadiLettura.Text)), txtLetturaAttuale.Text, _
    '                '   ContatoreID, DetailContatore.nidutente, LetturaErrata, ConsumoNegativo)
    '                'End If


    '                'If LetturaID > 0 Then
    '                '  clsLetture.VerificaLetturaGriglia(utility.stringoperation.formatint(ModDate.GiraData(Request("hdDataLettura"))), utility.stringoperation.formatint(Request("hdLettura")), txtLetturaAttuale.Text, ContatoreID, DetailContatore.nidutente, ConsumoNegativo, LetturaErrata)
    '                'End If
    '                If Not blnGestioneAnomalieConDataPassaggio Then
    '                    'MODIFICA LETTURA
    '                    '**************************************************************************************************************
    '                    'LA FUNZIONE VIENE UTILIZZATA SE MODIFICO UNA LETTURA DALLA GRIGLIA DI VISUALIZZAZIONE LETTURE
    '                    '**************************************************************************************************************
    '                    If LetturaID > 0 Then
    '                        If Not blnGestioneAnomalieSenzaLettura Then
    '                            clsLetture.VerificaLetturaGriglia(utility.stringoperation.formatint(ModDate.GiraData(Request("hdDataLettura"))), utility.stringoperation.formatint(Request("hdLettura")), txtLetturaAttuale.Text, ContatoreID, DetailContatore.nIdUtente, ConsumoNegativo, LetturaErrata)
    '                        End If
    '                    End If
    '                    '***************************************************************************************************************
    '                End If

    '                If Not blnGestioneAnomalieConDataPassaggio Then
    '                    'SE IL CONTROLLO RESTITUISCE UN CONSUMO NEGATIVO....
    '                    If Not blnGestioneAnomalieSenzaLettura Then
    '                        If ConsumoNegativo Then
    '                            'dim sScript as string=""
    '                            '
    '                            'sscript+="ConfermaConsumoNegativo();")
    '                            '
    '                            'RegisterScript(sScript , Me.GetType())
    '                            'Exit Sub
    '                        End If
    '                    End If

    '                    '***************************************************************************************************************
    '                End If

    '                'If ConsumoNegativo Then
    '                '  dim sScript as string=""
    '                '  
    '                '  sscript+="ConfermaConsumoNegativo();")
    '                '  
    '                '  RegisterScript(sScript , Me.GetType())
    '                '  Exit Sub
    '                'End If

    '                'If LetturaErrata Then
    '                '  dim sScript as string=""
    '                '  
    '                '  sscript+="GestAlert('a', 'warning', '', '', 'Attenzione:lettura Inserita Errata!!');")
    '                '  
    '                '  RegisterScript(sScript , Me.GetType())
    '                '  Exit Sub
    '                'End If

    '                If Not blnGestioneAnomalieConDataPassaggio Then
    '                    'SE UNA LETTURA ERRATA....
    '                    'UNA LETTURA ERRATA....
    '                    If Not blnGestioneAnomalieSenzaLettura Then
    '                        If LetturaErrata Then
    '                            'dim sScript as string=""
    '                            '
    '                            'sscript+="GestAlert('a', 'warning', '', '', 'Attenzione:lettura Inserita Errata!!');")
    '                            '
    '                            'RegisterScript(sScript , Me.GetType())
    '                            'Exit Sub
    '                        End If
    '                    End If
    '                End If

    '                'If blnGestioneAnomalieConDataPassaggio Then
    '                'MemoLettureNormaliATTUALI(txtDataPassaggio.Text, chkConsumoNegativoForzato.Checked)
    '                'End If

    '                'If blnGestioneAnomalieSenzaLettura Then
    '                'etLettureNormaliATTUALI(chkConsumoNegativoForzato.Checked)
    '                '	End If
    '                'If blnGestioneLettureNormale Then
    '                If MemoLetture() = False Then

    '                End If
    '                '	End If

    '                'PARTE RIGUARDANTE IL DATAENTRY DELLE LETTURE DA DATAENTRY CONTATORI
    '                'AGGIUNTA LA GESTIONE DELLE ANOMALIE
    '                '*************************************************************************************************************
    '            Case costanti.enmcontesto.DECONTATORI

    '                '*************************************************************************************************************
    '                'QUESTA FUNZIONE CONSENTE DI VERIFICARE SE IL PERIODO DI FATTURAZIONE UTILIZZATO PER L'INSERIMENTO DELLE LETTURE
    '                'E' STORICO
    '                'SE IL PERIODO E' STORICO NON VENGONO ESEGUITI I CONTROLLI 
    '                '*************************************************************************************************************
    '                ' If Not GestLetture.VerificaPeriodo(CInt(cboSelezionePeriodo.SelectedItem.Value)) Then
    '                '//Parametro da passare alla funzione MemoLetture
    '                blnStorico = False
    '                '====================================================================================
    '                '====================================================================================
    '                If Len(Trim(txtDataPassaggio.Text)) > 0 Or chkLasciatoAvviso.Checked Then
    '                    blnGestioneAnomalieConDataPassaggio = True
    '                End If

    '                If Len(Trim(txtDatadiLettura.Text)) > 0 And Len(Trim(txtLetturaAttuale.Text)) = 0 And lstAnomalieScelte.Items.Count > 0 Then
    '                    blnGestioneAnomalieSenzaLettura = True
    '                End If
    '                If Not blnGestioneAnomalieConDataPassaggio And Not blnGestioneAnomalieSenzaLettura Then
    '                    blnGestioneLettureNormale = True
    '                End If

    '                '====================================================================================

    '                ' If Not blnGestioneAnomalieConDataPassaggio Then

    '                ''NUOVA LETTURA
    '                'If LetturaID = 0 Then
    '                '  '***********************************************************************************************************
    '                '  'VERIFICA SE LA LETTURA INSERITA E' MINORE DELLA LETTURA PRECEDENTE IN QUESTO CASO
    '                '  'SEGNALA UN CONSUMO NEGATIVO
    '                '  '***********************************************************************************************************
    '                '  If Not blnGestioneAnomalieSenzaLettura Then
    '                '	clsLetture.VerificaLettura(utility.stringoperation.formatstring(ModDate.GiraData(txtDatadiLettura.Text)), txtLetturaAttuale.Text, _
    '                '	 ContatoreID, DetailContatore.nidutente, LetturaErrata, ConsumoNegativo)
    '                '  End If

    '                'End If


    '                ' End If

    '                ' If Not blnGestioneAnomalieConDataPassaggio Then
    '                ''MODIFICA LETTURA
    '                ''**************************************************************************************************************
    '                ''LA FUNZIONE VIENE UTILIZZATA SE MODIFICO UNA LETTURA DALLA GRIGLIA DI VISUALIZZAZIONE LETTURE
    '                ''**************************************************************************************************************
    '                'If LetturaID > 0 Then
    '                '  If Not blnGestioneAnomalieSenzaLettura Then
    '                '	clsLetture.VerificaLetturaGriglia(utility.stringoperation.formatint(ModDate.GiraData(Request("hdDataLettura"))), utility.stringoperation.formatint(Request("hdLettura")), txtLetturaAttuale.Text, ContatoreID, DetailContatore.nidutente, ConsumoNegativo, LetturaErrata)
    '                '  End If
    '                'End If
    '                ''***************************************************************************************************************
    '                ' End If

    '                ' If Not blnGestioneAnomalieConDataPassaggio Then
    '                ''SE IL CONTROLLO RESTITUISCE UN CONSUMO NEGATIVO....
    '                'If Not blnGestioneAnomalieSenzaLettura Then
    '                '' If ConsumoNegativo Then
    '                ''dim sScript as string=""
    '                ''
    '                ''sscript+="ConfermaConsumoNegativo();")
    '                ''
    '                ''RegisterScript(sScript , Me.GetType())
    '                ''Exit Sub
    '                '' End If
    '                'End If

    '                '***************************************************************************************************************
    '                '  End If

    '                ' If Not blnGestioneAnomalieConDataPassaggio Then
    '                ''SE UNA LETTURA ERRATA....
    '                ''UNA LETTURA ERRATA....
    '                'If Not blnGestioneAnomalieSenzaLettura Then
    '                '' If LetturaErrata Then
    '                ''dim sScript as string=""
    '                ''
    '                ''sscript+="GestAlert('a', 'warning', '', '', 'Attenzione:lettura Inserita Errata!!');")
    '                ''
    '                ''RegisterScript(sScript , Me.GetType())
    '                ''Exit Sub
    '                '' End If
    '                'End If
    '                ' End If

    '                ' If blnGestioneAnomalieConDataPassaggio Then
    '                'MemoLetture(txtDataPassaggio.Text)
    '                ' End If

    '                ' If blnGestioneAnomalieSenzaLettura Then
    '                'MemoLetture()
    '                ' End If
    '                'If blnGestioneLettureNormale Then
    '                If MemoLetture() = False Then

    '                End If
    '                ' End If

    '        End Select
    '        '*********************************************************************************************************************
    '        '******************************************************
    '        'FINE VERIFICA SE IL PERIODO IN USO E' UN PERIODO STORICO
    '        '******************************************************


    '        '*********************************************************************************************************************
    '        'CHIAMATA ALLA FUNZIONE DI AGGIORNAMETO DELLA TABELLA DI OPENutenze TP_LETTURE

    '        '*********************************************************************************************************************
    '    Catch Err As Exception
    '          Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ModLettureHone.btnConferma_Click.errore: ", Err)
    '       Response.Redirect("../../PaginaErrore.aspx")

    '    Finally
    '        WFSessione.Kill()
    '    End Try
    'End Sub

    Private Sub btnConferma_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConferma.Click
        Dim LetturaErrata, ConsumoNegativo As Boolean
        Dim blnStorico As Boolean = True
        Dim blnGestioneAnomalieConDataPassaggio As Boolean = False
        Dim blnGestioneAnomalieSenzaLettura As Boolean = False
        Dim blnGestioneLettureNormale As Boolean = False
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Dim DBAccess As New DBAccess.getDBobject
        Dim sDataPeriodoDal As String = ""
        Dim sDataPeriodoAl As String = ""
        Dim ModDate As New ClsGenerale.Generale
        Dim sScript As String = ""

        Try
            ContatoreID = StringOperation.FormatInt(Request("IDCONTATORE"))
            LetturaID = StringOperation.FormatInt(Request("IDLettura"))

            'modifica del 14/02/2007
            'è necessario verificare che la data di lettura inserito sia interna al periodo di fatturazione
            If ConstSession.IdPeriodo > 0 Then
                sSQL = "SELECT *"
                sSQL += " FROM TP_PERIODO"
                sSQL += " WHERE CODPERIODO = " & ConstSession.IdPeriodo
                dvMyDati = DBAccess.GetDataView(sSQL)
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        sDataPeriodoDal = myRow("DADATA")
                        sDataPeriodoAl = myRow("ADATA")
                        'controllo se la data è compresa
                        If ModDate.GiraData(txtDatadiLettura.Text) < sDataPeriodoDal Or ModDate.GiraData(txtDatadiLettura.Text) > sDataPeriodoAl Then
                            sScript = ""
                            sScript = sScript & "GestAlert('a', 'warning', '', '', 'Controllare la data lettura.Deve essere compresa tra il " & ModDate.GiraDataFromDB(sDataPeriodoDal) & " e il " & ModDate.GiraDataFromDB(sDataPeriodoAl) & "!');"
                            sScript = sScript & ""
                            RegisterScript(sScript, Me.GetType())
                            Exit Sub
                        End If
                    Next
                End If
                dvMyDati.Dispose()
            End If

            Dim DetailContatore As New objContatore
            DetailContatore = DBContatori.GetDetailsContatori(ContatoreID, -1)
            If Len(DetailContatore.sDataAttivazione) = 0 Then
                sScript += "GestAlert('a', 'warning', '', '', 'Non è possibile inserire letture su un contatore non attivo');"
                RegisterScript(sScript, Me.GetType)
                Exit Sub
            End If
            '******************************************************************
            'verifico che la data di lettura non sia successiva alla data di cessazione
            '******************************************************************
            If Len(DetailContatore.sDataCessazione) > 0 Then
                If ModDate.GiraData(txtDatadiLettura.Text) > ModDate.GiraData(DetailContatore.sDataCessazione) Then
                    sScript += "GestAlert('a', 'warning', '', '', 'Non è possibile inserire letture su un contatore cessato');"
                    RegisterScript(sScript, Me.GetType)
                    Exit Sub
                End If
            End If
            '******************************************************************
            'verifico che la data di lettura non sia successiva alla data di rimozione
            '******************************************************************
            If Len(DetailContatore.sDataRimTemp) > 0 Then
                If ModDate.GiraData(txtDatadiLettura.Text) > ModDate.GiraData(DetailContatore.sDataRimTemp) Then
                    sScript += "GestAlert('a', 'warning', '', '', 'Non è possibile inserire letture su un contatore rimosso');"
                    RegisterScript(sScript, Me.GetType)
                    Exit Sub
                End If
            End If
            '******************************************************************
            'verifico che la data di lettura non sia successiva alla data di sostituzione
            '******************************************************************
            If Len(DetailContatore.sDataSostituzione) > 0 Then
                If ModDate.GiraData(txtDatadiLettura.Text) > ModDate.GiraData(DetailContatore.sDataSostituzione) Then
                    sScript += "GestAlert('a', 'warning', '', '', 'Non è possibile inserire letture su un contatore sostituito');"
                    RegisterScript(sScript, Me.GetType)
                    Exit Sub
                End If
            End If
            '******************************************************************
            'verifico che la data di lettura non sia successiva alla data di sospensione
            '******************************************************************
            If Len(DetailContatore.sDataSospensioneUtenza) > 0 Then
                If ModDate.GiraData(txtDatadiLettura.Text) > ModDate.GiraData(DetailContatore.sDataSospensioneUtenza) Then
                    sScript += "GestAlert('a', 'warning', '', '', 'Non è possibile inserire letture su un contatore sospeso');"
                    RegisterScript(sScript, Me.GetType)
                    Exit Sub
                End If
            End If

            '******************************************************************
            'VERIFICA SE IL PERIODO IN USO E' UN PERIODO STORICO
            ''******************************************************************
            Select Case StringOperation.FormatInt(Request("PAG_PREC"))
                Case Costanti.enmContesto.DELETTURE
                    blnStorico = False

                    If Len(Trim(txtDataPassaggio.Text)) > 0 Or chkLasciatoAvviso.Checked Then
                        blnGestioneAnomalieConDataPassaggio = True
                    End If

                    If Len(Trim(txtDatadiLettura.Text)) > 0 And Len(Trim(txtLetturaAttuale.Text)) = 0 And lstAnomalieScelte.Items.Count > 0 Then
                        blnGestioneAnomalieSenzaLettura = True
                    End If
                    If Not blnGestioneAnomalieConDataPassaggio And Not blnGestioneAnomalieSenzaLettura Then
                        blnGestioneLettureNormale = True
                    End If

                    If Not blnGestioneAnomalieConDataPassaggio Then
                        'NUOVA LETTURA
                        If LetturaID = 0 Then
                            '***********************************************************************************************************
                            'VERIFICA SE LA LETTURA INSERITA E' MINORE DELLA LETTURA PRECEDENTE IN QUESTO CASO
                            'SEGNALA UN CONSUMO NEGATIVO
                            '***********************************************************************************************************
                            If Not blnGestioneAnomalieSenzaLettura Then
                                clsLetture.VerificaLettura(Utility.StringOperation.FormatString(ModDate.GiraData(txtDatadiLettura.Text)), txtLetturaAttuale.Text,
                                 ContatoreID, DetailContatore.nIdUtente, LetturaErrata, ConsumoNegativo)
                            End If
                        End If
                    End If

                    If Not blnGestioneAnomalieConDataPassaggio Then
                        'MODIFICA LETTURA
                        '**************************************************************************************************************
                        'LA FUNZIONE VIENE UTILIZZATA SE MODIFICO UNA LETTURA DALLA GRIGLIA DI VISUALIZZAZIONE LETTURE
                        '**************************************************************************************************************
                        If LetturaID > 0 Then
                            If Not blnGestioneAnomalieSenzaLettura Then
                                clsLetture.VerificaLetturaGriglia(Utility.StringOperation.FormatInt(ModDate.GiraData(Request("hdDataLettura"))), Utility.StringOperation.FormatInt(Request("hdLettura")), txtLetturaAttuale.Text, ContatoreID, DetailContatore.nIdUtente, ConsumoNegativo, LetturaErrata)
                            End If
                        End If
                        '***************************************************************************************************************
                    End If
                    'PARTE RIGUARDANTE IL DATAENTRY DELLE LETTURE DA DATAENTRY CONTATORI
                    'AGGIUNTA LA GESTIONE DELLE ANOMALIE
                    '*************************************************************************************************************
                Case Costanti.enmContesto.DECONTATORI

                    '*************************************************************************************************************
                    'QUESTA FUNZIONE CONSENTE DI VERIFICARE SE IL PERIODO DI FATTURAZIONE UTILIZZATO PER L'INSERIMENTO DELLE LETTURE
                    'E' STORICO
                    'SE IL PERIODO E' STORICO NON VENGONO ESEGUITI I CONTROLLI 
                    '*************************************************************************************************************
                    ' If Not GestLetture.VerificaPeriodo(CInt(cboSelezionePeriodo.SelectedItem.Value)) Then
                    '//Parametro da passare alla funzione MemoLetture
                    blnStorico = False
                    '====================================================================================
                    '====================================================================================
                    If Len(Trim(txtDataPassaggio.Text)) > 0 Or chkLasciatoAvviso.Checked Then
                        blnGestioneAnomalieConDataPassaggio = True
                    End If

                    If Len(Trim(txtDatadiLettura.Text)) > 0 And Len(Trim(txtLetturaAttuale.Text)) = 0 And lstAnomalieScelte.Items.Count > 0 Then
                        blnGestioneAnomalieSenzaLettura = True
                    End If
                    If Not blnGestioneAnomalieConDataPassaggio And Not blnGestioneAnomalieSenzaLettura Then
                        blnGestioneLettureNormale = True
                    End If
                    '====================================================================================
            End Select
            If MemoLetture(DetailContatore) = False Then
                Throw New Exception("Errore in salvataggio lettura")
                'Else
                '    Dim str As String
                '    str = ""
                '    str += "parent.opener.parent.Visualizza.ReloadGrdLetture();"
                '    str += "parent.window.close();"
                '    RegisterScript(str, Me.GetType())
            End If
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ModLettureHome.btnConferma_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub txtLetturaAttuale_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtLetturaAttuale.TextChanged
        Dim nConsumoEffettivo As Integer = 0

        Try
            'clsLetture.CalcolaConsumoEffettivo(txtLetturaAttuale.Text, nConsumoEffettivo, txtDatadiLetturaPrec.Text, CStr(Request("IDCONTATORE")))
            If IsNumeric(txtLetturaAttuale.Text) Then
                If IsNumeric(txtLetturaAttualePrec.Text) Then
                    nConsumoEffettivo = CInt(txtLetturaAttuale.Text) - CInt(txtLetturaAttualePrec.Text)
                    If IsNumeric(txtSubConsumo.Text) Then
                        nConsumoEffettivo -= CInt(txtSubConsumo.Text)
                    End If
                End If
            End If
            txtConsEffettivo.Text = nConsumoEffettivo
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ModLettureHone.txtLetturaAttuale.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub txtDatadiLettura_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDatadiLettura.TextChanged
        Dim sGGConsumo As String = ""
        Try
            If IsDate(txtDatadiLettura.Text) Then
                If IsDate(txtDatadiLetturaPrec.Text) Then
                    sGGConsumo = DateDiff(DateInterval.Day, CDate(txtDatadiLetturaPrec.Text), CDate(txtDatadiLettura.Text))
                End If
            End If
            txtGGConsumo.Text = sGGConsumo
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ModLettureHone.txtDatadiLettura.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    '******************************************************************
    'FUNZIONE CHE SI OCCUPA DELL'AGGIORNAMENTO DELLA TABELLA TP_LETTURE
    'DI OPENutenze
    '******************************************************************
    'Private Function MemoLetture() As Boolean
    '    Dim nBaseTempo As Integer = System.Configuration.ConfigurationManager.AppSettings("BaseTempo")
    '    Dim oMyLettura As New ObjLettura
    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Dim WFErrore As String

    '    Try
    '        'inizializzo la connessione
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If
    '        ContatoreID = stringoperation.formatint(request("IDCONTATORE"))
    '        LetturaID = stringoperation.formatint(request("IDLettura"))
    '        Log.Debug("modletturahome::btnconferma::prelavato ContatoreID::" & ContatoreID & "::LetturaID::" & LetturaID)

    '        Dim DetailContatore As New objContatore
    '        DetailContatore = DBContatori.GetDetailsContatori(ContatoreID, WFSessione)

    '        'valorizzo la lettura con i dati della videata
    '        oMyLettura.nIdContatore = ContatoreID
    '        If oMyLettura.IdLettura > 0 Then
    '            oMyLettura.tDataVariazione = Now
    '            oMyLettura.sAzione = "DE-INS"
    '        Else
    '            oMyLettura.tDataInserimento = Now
    '            oMyLettura.sAzione = "DE-INS"
    '        End If
    '        If Not IsNothing(cboSelezionePeriodo.SelectedItem) Then
    '            oMyLettura.nIdPeriodo = CInt(cboSelezionePeriodo.SelectedItem.Value)
    '        Else
    '            oMyLettura.nIdPeriodo = ConstSession.IdPeriodo
    '        End If
    '        oMyLettura.IdLettura = stringoperation.formatint(request("IDLETTURA"))
    '        oMyLettura.nIdUtente = DetailContatore.nIdUtente
    '        oMyLettura.tDataLetturaAtt = txtDatadiLettura.Text
    '        oMyLettura.nLetturaAtt = txtLetturaAttuale.Text
    '        If txtGGConsumo.Text <> "" Then
    '            oMyLettura.nGiorni = txtGGConsumo.Text
    '        Else
    '            Log.Debug("modletturahome::btnconferma::non ho giorni")
    '            oMyLettura.nGiorni = -1
    '        End If
    '        oMyLettura.sLetturaTeorica = txtLetturaTeorica.Text
    '        If IsNumeric(txtConsEffettivo.Text) Then
    '            oMyLettura.nConsumo = txtConsEffettivo.Text
    '        Else
    '            Log.Debug("modletturahome::btnconferma::non ho txtConsEffettivo.Text")
    '        End If
    '        If IsNumeric(cboModalitaLettura.SelectedItem.Value) Then
    '            oMyLettura.nCodModoLett = cboModalitaLettura.SelectedItem.Value
    '        Else
    '            Log.Debug("modletturahome::btnconferma::non ho cboModalitaLettura.SelectedItem.Value")
    '        End If
    '        If IsNumeric(cboStatoLetturaPage.SelectedItem.Value) Then
    '            oMyLettura.nIdStatoLettura = cboStatoLetturaPage.SelectedItem.Value
    '        Else
    '            Log.Debug("modletturahome::btnconferma::non ho cboStatoLetturaPage.SelectedItem.Value")
    '        End If
    '        If IsNumeric(txtConsumoTeorico.Text) Then
    '            oMyLettura.nConsumoTeorico = txtConsumoTeorico.Text
    '        Else
    '            Log.Debug("modletturahome::btnconferma::non ho txtConsumoTeorico.Text")
    '        End If
    '        oMyLettura.bIsFatturata = chkFatturazione.Checked
    '        oMyLettura.bFattSospesa = chkFatturazioneSospesa.Checked
    '        oMyLettura.bIsIncongruenteForzato = chkIncongruenteForzato.Checked
    '        oMyLettura.bIsGiroContatore = chkGiroContatore.Checked
    '        oMyLettura.sNote = txtNoteLettura.Text
    '        Select Case lstAnomalieScelte.Items.Count
    '            Case 1
    '                oMyLettura.nIdAnomalia1 = lstAnomalieScelte.Items(0).Value
    '            Case 2
    '                oMyLettura.nIdAnomalia1 = lstAnomalieScelte.Items(0).Value
    '                oMyLettura.nIdAnomalia2 = lstAnomalieScelte.Items(1).Value
    '            Case 3
    '                oMyLettura.nIdAnomalia1 = lstAnomalieScelte.Items(0).Value
    '                oMyLettura.nIdAnomalia2 = lstAnomalieScelte.Items(1).Value
    '                oMyLettura.nIdAnomalia3 = lstAnomalieScelte.Items(2).Value
    '        End Select
    '        If txtDataPassaggio.Text <> "" Then
    '            oMyLettura.tDataPassaggio = txtDataPassaggio.Text
    '        End If
    '        Log.Debug("modletturahome::btnconferma::prelevo lettura precedente")
    '        'prelevo i dati della lettura precedente
    '        If GestLetture.getDatiLetturaPrecedente(oMyLettura.tDataLetturaAtt, oMyLettura.nIdContatore, oMyLettura.nLetturaPrec, oMyLettura.tDataLetturaPrec) < 1 Then
    '            Return -1
    '        End If
    '        Log.Debug("modletturahome::btnconferma::prelevata")
    '        ''''''''''''''''''''''SALVA DATI''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '        Select Case stringoperation.formatint(request("PAG_PREC"))

    '            Case costanti.enmcontesto.DELETTURE
    '                'attivo il servizio
    '                RemoRuoloH2O = Activator.GetObject(TypeOfRI, ConfigurationManager.AppSettings("URLServizioRuoloH2O"))
    '                'ricalcolo il consumo
    '                If oMyLettura.nConsumo = -1 Then
    '                    oMyLettura.nConsumo = RemoRuoloH2O.CalcolaConsumo(oMyLettura.nLetturaPrec, oMyLettura.nLetturaAtt, oMyLettura.nConsumoSubContatore, DetailContatore.nFondoScala)
    '                End If
    '                Log.Debug("modletturahome::btnconferma::ho consumo")
    '                If oMyLettura.nConsumo >= 0 Then
    '                    'ricalcolo i giorni
    '                    If oMyLettura.nGiorni = -1 Then
    '                        oMyLettura.nGiorni = RemoRuoloH2O.CalcolaGiorni(oMyLettura.tDataLetturaPrec, oMyLettura.tDataLetturaAtt, nBaseTempo)
    '                    End If
    '                    If oMyLettura.nGiorni = -1 Then
    '                        Return -1
    '                    End If
    '                    Log.Debug("modletturahome::btnconferma::ho giorni")
    '                End If
    '                '=====================================================================================================================
    '                'SALVATAGGIO DELLE LETTURE NUOVE
    '                '=====================================================================================================================
    '                If oMyLettura.IdLettura > 0 Then
    '                    oMyLettura.tDataVariazione = Now
    '                End If
    '                oMyLettura.sAzione = "DE-INS"
    '                Log.Debug("modletturahome::btnconferma::inserisco")
    '                If GestLetture.SetLetture(oMyLettura.IdLettura, oMyLettura) < 1 Then
    '                    Return False
    '                End If

    '                dim sScript as string=""

    '                
    '                sscript+="parent.opener.parent.Visualizza.frmHidden.action='Letture.aspx';")
    '                sscript+="parent.opener.parent.Visualizza.frmHidden.submit();")
    '                sscript+="parent.window.close();")
    '                

    '                RegisterScript(sScript , Me.GetType())

    '                '**************************************************************************************************************
    '                '**************************************************************************************************************
    '                'GESTIONE DI INSERIMENTO MODIFICA LETTURA DA DECONTATORI
    '                '**************************************************************************************************************
    '                '**************************************************************************************************************
    '            Case costanti.enmcontesto.DECONTATORI

    '                '***************************************************************************************************************************
    '                'SALVA I DATI DI LETTURA SE IL PERIODO E' STORICO NELLA TABELLA TP_LETTURE
    '                '***************************************************************************************************************************
    '                'attivo il servizio
    '                RemoRuoloH2O = Activator.GetObject(TypeOfRI, ConfigurationManager.AppSettings("URLServizioRuoloH2O"))
    '                'ricalcolo il consumo
    '                If oMyLettura.nConsumo = -1 Then
    '                    oMyLettura.nConsumo = RemoRuoloH2O.CalcolaConsumo(oMyLettura.nLetturaPrec, oMyLettura.nLetturaAtt, oMyLettura.nConsumoSubContatore, DetailContatore.nFondoScala)
    '                End If
    '                If oMyLettura.nConsumo >= 0 Then
    '                    'ricalcolo i giorni
    '                    If oMyLettura.nGiorni = -1 Then
    '                        oMyLettura.nGiorni = RemoRuoloH2O.CalcolaGiorni(oMyLettura.tDataLetturaPrec, oMyLettura.tDataLetturaAtt, nBaseTempo)
    '                    End If
    '                    If oMyLettura.nGiorni = -1 Then
    '                        Return -1
    '                    End If
    '                End If
    '                If GestLetture.SetLetture(oMyLettura.IdLettura, oMyLettura) < 1 Then
    '                    Return False
    '                End If
    '                dim sScript as string=""
    '                
    '                sscript+="parent.opener.parent.Visualizza.location.href='Letture.aspx?hdIDContatore=" & ContatoreID & "&PAG_PREC=" & stringoperation.formatint(request("PAG_PREC")) & "';")
    '                sscript+="parent.window.close();")
    '                
    '                RegisterScript(sScript , Me.GetType())
    '        End Select
    '        Return True
    '    Catch Err As Exception
    '        lblError.Text = Err.Message
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ModLettureHone.MemoLetture.errore: ", Err)
    '      Response.Redirect("../../PaginaErrore.aspx")
    '        Return False
    '    Finally
    '        WFSessione.Kill()
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="DetailContatore"></param>
    ''' <returns></returns>
    Private Function MemoLetture(ByVal DetailContatore As objContatore) As Boolean
        Dim nBaseTempo As Integer = ConstSession.BaseTempo
        Dim oMyLettura As New ObjLettura
        Dim sScript As String = ""

        Try
            ContatoreID = StringOperation.FormatInt(Request("IDCONTATORE"))
            LetturaID = StringOperation.FormatInt(Request("IDLettura"))
            Log.Debug("modletturahome::btnconferma::prelavato ContatoreID::" & ContatoreID & "::LetturaID::" & LetturaID)

            'valorizzo la lettura con i dati della videata
            oMyLettura.nIdContatore = ContatoreID
            If oMyLettura.IdLettura > 0 Then
                oMyLettura.tDataVariazione = Now
                oMyLettura.sAzione = "DE-INS"
            Else
                oMyLettura.tDataInserimento = Now
                oMyLettura.sAzione = "DE-INS"
            End If
            If Not IsNothing(cboSelezionePeriodo.SelectedItem) Then
                oMyLettura.nIdPeriodo = CInt(cboSelezionePeriodo.SelectedItem.Value)
            Else
                oMyLettura.nIdPeriodo = ConstSession.IdPeriodo
            End If
            oMyLettura.IdLettura = StringOperation.FormatInt(Request("IDLETTURA"))
            oMyLettura.nIdUtente = DetailContatore.nIdUtente
            oMyLettura.tDataLetturaAtt = txtDatadiLettura.Text
            oMyLettura.nLetturaAtt = txtLetturaAttuale.Text
            If txtGGConsumo.Text <> "" Then
                oMyLettura.nGiorni = txtGGConsumo.Text
            Else
                Log.Debug("modletturahome::btnconferma::non ho giorni")
                oMyLettura.nGiorni = -1
            End If
            oMyLettura.sLetturaTeorica = txtLetturaTeorica.Text
            If IsNumeric(txtConsEffettivo.Text) Then
                oMyLettura.nConsumo = txtConsEffettivo.Text
            Else
                Log.Debug("modletturahome::btnconferma::non ho txtConsEffettivo.Text")
            End If
            If IsNumeric(cboModalitaLettura.SelectedItem.Value) Then
                oMyLettura.nCodModoLett = cboModalitaLettura.SelectedItem.Value
            Else
                Log.Debug("modletturahome::btnconferma::non ho cboModalitaLettura.SelectedItem.Value")
            End If
            If IsNumeric(cboStatoLetturaPage.SelectedItem.Value) Then
                oMyLettura.nIdStatoLettura = cboStatoLetturaPage.SelectedItem.Value
            Else
                Log.Debug("modletturahome::btnconferma::non ho cboStatoLetturaPage.SelectedItem.Value")
            End If
            If IsNumeric(txtConsumoTeorico.Text) Then
                oMyLettura.nConsumoTeorico = txtConsumoTeorico.Text
            Else
                Log.Debug("modletturahome::btnconferma::non ho txtConsumoTeorico.Text")
            End If
            If IsNumeric(txtSubConsumo.Text) Then
                oMyLettura.nConsumoSubContatore = txtSubConsumo.Text
            Else
                Log.Debug("modletturahome::btnconferma::non ho txtSubConsumo.Text")
            End If
            oMyLettura.bIsFatturata = chkFatturazione.Checked
            oMyLettura.bFattSospesa = chkFatturazioneSospesa.Checked
            oMyLettura.bIsIncongruenteForzato = chkIncongruenteForzato.Checked
            oMyLettura.bIsGiroContatore = chkGiroContatore.Checked
            oMyLettura.sNote = txtNoteLettura.Text
            Select Case lstAnomalieScelte.Items.Count
                Case 1
                    oMyLettura.nIdAnomalia1 = lstAnomalieScelte.Items(0).Value
                Case 2
                    oMyLettura.nIdAnomalia1 = lstAnomalieScelte.Items(0).Value
                    oMyLettura.nIdAnomalia2 = lstAnomalieScelte.Items(1).Value
                Case 3
                    oMyLettura.nIdAnomalia1 = lstAnomalieScelte.Items(0).Value
                    oMyLettura.nIdAnomalia2 = lstAnomalieScelte.Items(1).Value
                    oMyLettura.nIdAnomalia3 = lstAnomalieScelte.Items(2).Value
            End Select
            If txtDataPassaggio.Text <> "" Then
                oMyLettura.tDataPassaggio = txtDataPassaggio.Text
            End If
            Log.Debug("modletturahome::btnconferma::prelevo lettura precedente")
            'prelevo i dati della lettura precedente
            If GestLetture.getDatiLetturaPrecedente(oMyLettura.tDataLetturaAtt, oMyLettura.nIdContatore, oMyLettura.nLetturaPrec, oMyLettura.tDataLetturaPrec.ToString) < 1 Then
                Return -1
            End If
            Log.Debug("modletturahome::btnconferma::prelevata")
            ''''''''''''''''''''''SALVA DATI''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'attivo il servizio
            RemoRuoloH2O = Activator.GetObject(TypeOfRI, ConfigurationManager.AppSettings("URLServizioRuoloH2O"))
            If Not oMyLettura.bIsPrimaLettura Then
                'ricalcolo il consumo
                oMyLettura.nConsumo = RemoRuoloH2O.CalcolaConsumo(oMyLettura.nLetturaPrec, oMyLettura.nLetturaAtt, oMyLettura.nConsumoSubContatore, DetailContatore.nFondoScala)
                Log.Debug("modletturahome::btnconferma::ho " & oMyLettura.nConsumo.ToString & " consumo per " & oMyLettura.nLetturaPrec.ToString & " :: " & oMyLettura.nLetturaAtt.ToString & " :: " & oMyLettura.nConsumoSubContatore.ToString & " :: " & DetailContatore.nFondoScala.ToString)
                oMyLettura.nGiorni = RemoRuoloH2O.CalcolaGiorni(oMyLettura.tDataLetturaPrec, oMyLettura.tDataLetturaAtt, nBaseTempo)
                Log.Debug("modletturahome::btnconferma::ho " & oMyLettura.nGiorni.ToString & " giorni per " & oMyLettura.tDataLetturaPrec.ToShortDateString & " :: " & oMyLettura.tDataLetturaAtt.ToShortDateString & " :: " & nBaseTempo.ToString)
            Else
                oMyLettura.nConsumo = 0 : oMyLettura.nGiorni = 0
            End If
            If oMyLettura.IdLettura > 0 Then
                oMyLettura.tDataVariazione = Now
            End If
            oMyLettura.sAzione = "DE-INS"
            Log.Debug("modletturahome::btnconferma::inserisco")
            If GestLetture.SetLetture(oMyLettura.IdLettura, oMyLettura) < 1 Then
                Return False
            End If
            Select Case StringOperation.FormatInt(Request("PAG_PREC"))
                Case Costanti.enmContesto.DELETTURE
                    sScript += "console.log('devo fare reload');"
                    sScript += "parent.opener.parent.Visualizza.ReloadGrdLetture();"
                    sScript += "console.log('ho fatto reload');"
                    sScript += "parent.window.close();"
                    RegisterScript(sScript, Me.GetType())
                Case Costanti.enmContesto.DECONTATORI

                    sScript += "console.log('devo fare reload');"
                    sScript += "parent.opener.parent.Visualizza.ReloadGrdLetture();"
                    sScript += "console.log('ho fatto reload');"
                    sScript += "parent.window.close();"
                    RegisterScript(sScript, Me.GetType())
            End Select
            Return True
        Catch Err As Exception
            lblError.Text = Err.Message
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ModLettureHone.MemoLetture.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
            Return False
        End Try
    End Function
    'Private Function MemoLetture(ByVal DetailContatore As objContatore) As Boolean
    '    Dim nBaseTempo As Integer = System.Configuration.ConfigurationManager.AppSettings("BaseTempo")
    '    Dim oMyLettura As New ObjLettura
    '    Dim sScript As String = ""

    '    Try
    '        ContatoreID = stringoperation.formatint(request("IDCONTATORE"))
    '        LetturaID = stringoperation.formatint(request("IDLettura"))
    '        Log.Debug("modletturahome::btnconferma::prelavato ContatoreID::" & ContatoreID & "::LetturaID::" & LetturaID)

    '        'valorizzo la lettura con i dati della videata
    '        oMyLettura.nIdContatore = ContatoreID
    '        If oMyLettura.IdLettura > 0 Then
    '            oMyLettura.tDataVariazione = Now
    '            oMyLettura.sAzione = "DE-INS"
    '        Else
    '            oMyLettura.tDataInserimento = Now
    '            oMyLettura.sAzione = "DE-INS"
    '        End If
    '        If Not IsNothing(cboSelezionePeriodo.SelectedItem) Then
    '            oMyLettura.nIdPeriodo = CInt(cboSelezionePeriodo.SelectedItem.Value)
    '        Else
    '            oMyLettura.nIdPeriodo = ConstSession.IdPeriodo
    '        End If
    '        oMyLettura.IdLettura = stringoperation.formatint(request("IDLETTURA"))
    '        oMyLettura.nIdUtente = DetailContatore.nIdUtente
    '        oMyLettura.tDataLetturaAtt = txtDatadiLettura.Text
    '        oMyLettura.nLetturaAtt = txtLetturaAttuale.Text
    '        If txtGGConsumo.Text <> "" Then
    '            oMyLettura.nGiorni = txtGGConsumo.Text
    '        Else
    '            Log.Debug("modletturahome::btnconferma::non ho giorni")
    '            oMyLettura.nGiorni = -1
    '        End If
    '        oMyLettura.sLetturaTeorica = txtLetturaTeorica.Text
    '        If IsNumeric(txtConsEffettivo.Text) Then
    '            oMyLettura.nConsumo = txtConsEffettivo.Text
    '        Else
    '            Log.Debug("modletturahome::btnconferma::non ho txtConsEffettivo.Text")
    '        End If
    '        If IsNumeric(cboModalitaLettura.SelectedItem.Value) Then
    '            oMyLettura.nCodModoLett = cboModalitaLettura.SelectedItem.Value
    '        Else
    '            Log.Debug("modletturahome::btnconferma::non ho cboModalitaLettura.SelectedItem.Value")
    '        End If
    '        If IsNumeric(cboStatoLetturaPage.SelectedItem.Value) Then
    '            oMyLettura.nIdStatoLettura = cboStatoLetturaPage.SelectedItem.Value
    '        Else
    '            Log.Debug("modletturahome::btnconferma::non ho cboStatoLetturaPage.SelectedItem.Value")
    '        End If
    '        If IsNumeric(txtConsumoTeorico.Text) Then
    '            oMyLettura.nConsumoTeorico = txtConsumoTeorico.Text
    '        Else
    '            Log.Debug("modletturahome::btnconferma::non ho txtConsumoTeorico.Text")
    '        End If
    '        If IsNumeric(txtSubConsumo.Text) Then
    '            oMyLettura.nConsumoSubContatore = txtSubConsumo.Text
    '        Else
    '            Log.Debug("modletturahome::btnconferma::non ho txtSubConsumo.Text")
    '        End If
    '        oMyLettura.bIsFatturata = chkFatturazione.Checked
    '        oMyLettura.bFattSospesa = chkFatturazioneSospesa.Checked
    '        oMyLettura.bIsIncongruenteForzato = chkIncongruenteForzato.Checked
    '        oMyLettura.bIsGiroContatore = chkGiroContatore.Checked
    '        oMyLettura.sNote = txtNoteLettura.Text
    '        Select Case lstAnomalieScelte.Items.Count
    '            Case 1
    '                oMyLettura.nIdAnomalia1 = lstAnomalieScelte.Items(0).Value
    '            Case 2
    '                oMyLettura.nIdAnomalia1 = lstAnomalieScelte.Items(0).Value
    '                oMyLettura.nIdAnomalia2 = lstAnomalieScelte.Items(1).Value
    '            Case 3
    '                oMyLettura.nIdAnomalia1 = lstAnomalieScelte.Items(0).Value
    '                oMyLettura.nIdAnomalia2 = lstAnomalieScelte.Items(1).Value
    '                oMyLettura.nIdAnomalia3 = lstAnomalieScelte.Items(2).Value
    '        End Select
    '        If txtDataPassaggio.Text <> "" Then
    '            oMyLettura.tDataPassaggio = txtDataPassaggio.Text
    '        End If
    '        Log.Debug("modletturahome::btnconferma::prelevo lettura precedente")
    '        'prelevo i dati della lettura precedente
    '        If GestLetture.getDatiLetturaPrecedente(oMyLettura.tDataLetturaAtt, oMyLettura.nIdContatore, oMyLettura.nLetturaPrec, oMyLettura.tDataLetturaPrec) < 1 Then
    '            Return -1
    '        End If
    '        Log.Debug("modletturahome::btnconferma::prelevata")
    '        ''''''''''''''''''''''SALVA DATI''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '        'attivo il servizio
    '        RemoRuoloH2O = Activator.GetObject(TypeOfRI, ConfigurationManager.AppSettings("URLServizioRuoloH2O"))
    '        If Not oMyLettura.bIsPrimaLettura Then
    '            'ricalcolo il consumo
    '            'If oMyLettura.nConsumo = -1 Then
    '            'oMyLettura.nConsumo = RemoRuoloH2O.CalcolaConsumo(oMyLettura.nLetturaPrec, oMyLettura.nLetturaAtt, oMyLettura.nConsumoSubContatore, DetailContatore.nFondoScala)
    '            oMyLettura.nConsumo = RemoRuoloH2O.CalcolaConsumo(oMyLettura.nLetturaPrec, oMyLettura.nLetturaAtt, oMyLettura.nConsumoSubContatore, New clsLetture().GetFondoScala(DetailContatore.nIdContatore))
    '            'End If
    '            Log.Debug("modletturahome::btnconferma::ho " & oMyLettura.nConsumo.ToString & " consumo per " & oMyLettura.nLetturaPrec.ToString & " :: " & oMyLettura.nLetturaAtt.ToString & " :: " & oMyLettura.nConsumoSubContatore.ToString & " :: " & New clsLetture().GetFondoScala(DetailContatore.nIdContatore).ToString)
    '            'If oMyLettura.nConsumo >= 0 Then
    '            '    'ricalcolo i giorni
    '            '    If oMyLettura.nGiorni = -1 Then
    '            oMyLettura.nGiorni = RemoRuoloH2O.CalcolaGiorni(oMyLettura.tDataLetturaPrec, oMyLettura.tDataLetturaAtt, nBaseTempo)
    '            '    End If
    '            '    If oMyLettura.nGiorni = -1 Then
    '            '        Return -1
    '            '    End If
    '            Log.Debug("modletturahome::btnconferma::ho " & oMyLettura.nGiorni.ToString & " giorni per " & oMyLettura.tDataLetturaPrec.ToShortDateString & " :: " & oMyLettura.tDataLetturaAtt.ToShortDateString & " :: " & nBaseTempo.ToString)
    '            'End If
    '        Else
    '            oMyLettura.nConsumo = 0 : oMyLettura.nGiorni = 0
    '        End If
    '        If oMyLettura.IdLettura > 0 Then
    '            oMyLettura.tDataVariazione = Now
    '        End If
    '        oMyLettura.sAzione = "DE-INS"
    '        Log.Debug("modletturahome::btnconferma::inserisco")
    '        If GestLetture.SetLetture(oMyLettura.IdLettura, oMyLettura) < 1 Then
    '            Return False
    '        End If
    '        Select Case stringoperation.formatint(request("PAG_PREC"))
    '            Case costanti.enmcontesto.DELETTURE

    '                'sscript+="parent.opener.parent.Visualizza.frmHidden.action='Letture.aspx';")
    '                'sscript+="parent.opener.parent.Visualizza.frmHidden.submit();")
    '                'sscript+="parent.opener.parent.Comandi.location.href='../DataEntryLetture/ComandiDataEntry.aspx?title=Acquedotto - " & Session("DESC_TIPO_PROC_SERV") & " - " & "Inserimento-Modifica Letture" & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo & "';")
    '                'sscript+="parent.opener.parent.Visualizza.location.href='Letture.aspx?hdIDContatore=" & ContatoreID & "&PAG_PREC=" & stringoperation.formatint(request("PAG_PREC")) & "';")


    '                'Test Chris
    '                sScript += "console.log('devo fare reload');"
    '                sScript += "parent.opener.parent.Visualizza.ReloadGrdLetture();"
    '                sScript += "console.log('ho fatto reload');"
    '                sScript += "parent.window.close();"

    '                RegisterScript(sScript, Me.GetType())
    '            Case costanti.enmcontesto.DECONTATORI

    '                'sscript+="parent.opener.parent.Visualizza.location.href='Letture.aspx?hdIDContatore=" & ContatoreID & "&PAG_PREC=" & stringoperation.formatint(request("PAG_PREC")) & "';")


    '                'Test Chris
    '                sScript += "console.log('devo fare reload');"
    '                sScript += "parent.opener.parent.Visualizza.ReloadGrdLetture();"
    '                sScript += "console.log('ho fatto reload');"
    '                sScript += "parent.window.close();"

    '                RegisterScript(sScript, Me.GetType())
    '        End Select
    '        Return True
    '    Catch Err As Exception
    '        lblError.Text = Err.Message
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ModLettureHone.MemoLetture.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '        Return False
    '    End Try
    'End Function

    'Private Function MemoLetture(ByVal strDataPassaggio As String) As Boolean

    '    ContatoreID = stringoperation.formatint(request("IDCONTATORE"))
    '    LetturaID = stringoperation.formatint(request("IDLettura"))

    '    Try

    '        Dim DetailContatore As New objContatore
    '        DetailContatore = DBContatori.GetDetailsContatori(ContatoreID)

    '        ''''''''''''''''''''''SALVA DATI''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '        '**************************************************************************************************************
    '        '**************************************************************************************************************
    '        'GESTIONE DI INSERIMENTO MODIFICA LETTURA DA DECONTATORI
    '        '**************************************************************************************************************
    '        '**************************************************************************************************************

    '        '***************************************************************************************************************************
    '        'SALVA I DATI DI LETTURA SE IL PERIODO E' STORICO NELLA TABELLA TP_LETTURE
    '        '***************************************************************************************************************************
    '        GestLettureStoricheDEContatori.setLettureConPeriodoStorico(ContatoreID, CInt(cboSelezionePeriodo.SelectedItem.Value), stringoperation.formatint(request("IDLETTURA")), _
    '       DetailContatore.nIdUtente, txtNoteLettura.Text, DetailContatore, strDataPassaggio, lstAnomalie, lstAnomalieScelte)

    '        dim sScript as string=""

    '        
    '        sscript+="parent.opener.parent.Visualizza.location.href='Letture.aspx?hdIDContatore=" & ContatoreID & "&PAG_PREC=" & stringoperation.formatint(request("PAG_PREC")) & "';")
    '        sscript+="parent.window.close();")
    '        

    '        RegisterScript(sScript , Me.GetType())
    '        Return True
    '    Catch ex As Exception

    '        lblError.Text = ex.Message
    '        '**************************************************************************************************************
    '        'Gestione Pagina Errore ed Invio E-Mail
    '        '**************************************************************************************************************
    '        ' _Err.SendEmailError(ex)
    '  Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ModLettureHone.MemoLetture.errore: ", Err)
    '       Response.Redirect("../../PaginaErrore.aspx")
    '        'Response.Write(_Err.GetHTMLError(ex, "../Styles.css", "../images/testata_Utenze.jpg"))
    '        'Response.End()
    '        Return False
    '    End Try
    'End Function

    'Private Function SetLetture() As Boolean

    '    ContatoreID = stringoperation.formatint(request("IDCONTATORE"))
    '    LetturaID = stringoperation.formatint(request("IDLettura"))

    '    Try

    '        Dim DetailContatore As New objContatore
    '        DetailContatore = DBContatori.GetDetailsContatori(ContatoreID)

    '        ''''''''''''''''''''''SALVA DATI''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '        '**************************************************************************************************************
    '        '**************************************************************************************************************
    '        'GESTIONE DI INSERIMENTO MODIFICA LETTURA DA DECONTATORI
    '        '**************************************************************************************************************
    '        '**************************************************************************************************************

    '        '***************************************************************************************************************************
    '        'SALVA I DATI DI LETTURA SE IL PERIODO E' STORICO NELLA TABELLA TP_LETTURE
    '        '***************************************************************************************************************************
    '        GestLettureStoricheDEContatori.setLettureConPeriodoStorico(ContatoreID, CInt(cboSelezionePeriodo.SelectedItem.Value), stringoperation.formatint(request("IDLETTURA")), _
    '       DetailContatore.nIdUtente, txtNoteLettura.Text, txtDatadiLettura.Text, txtGGConsumo.Text, txtLetturaTeorica.Text, _
    '       txtConsEffettivo.Text, cboModalitaLettura.SelectedItem.Value, cboStatoLetturaPage.SelectedItem.Value, txtConsumoTeorico.Text, chkFatturazione.Checked, _
    '       chkFatturazioneSospesa.Checked, chkIncongruenteForzato.Checked, chkGiroContatore.Checked, DetailContatore, lstAnomalie, lstAnomalieScelte)

    '        dim sScript as string=""

    '        
    '        sscript+="parent.opener.parent.Visualizza.location.href='Letture.aspx?hdIDContatore=" & ContatoreID & "&PAG_PREC=" & stringoperation.formatint(request("PAG_PREC")) & "';")
    '        sscript+="parent.window.close();")
    '        

    '        RegisterScript(sScript , Me.GetType())
    '        Return True
    '    Catch ex As Exception

    '        lblError.Text = ex.Message
    '        '**************************************************************************************************************
    '        'Gestione Pagina Errore ed Invio E-Mail
    '        '**************************************************************************************************************
    '        ' _Err.SendEmailError(ex)
    ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ModLettureHone.SetLetture.errore: ", Err)
    '       Response.Redirect("../../PaginaErrore.aspx")
    '        'Response.Write(_Err.GetHTMLError(ex, "../Styles.css", "../images/testata_Utenze.jpg"))
    '        'Response.End()
    '        Return False
    '    End Try
    'End Function

    Sub FilldvMyDatiopDownSQL(ByVal cboTemp As DropDownList, ByVal dvMyDati As DataView, ByVal lngSelectedID As Long)

        Try
            Dim myListItem As ListItem
            myListItem = New ListItem
            myListItem.Text = "..."
            myListItem.Value = "-1"
            cboTemp.Items.Add(myListItem)
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    myListItem = New ListItem

                    myListItem.Text = StringOperation.FormatString(myRow(1))
                    myListItem.Value = StringOperation.FormatInt(myRow(0))
                    cboTemp.Items.Add(myListItem)
                Next
            End If

            If lngSelectedID <> -1 Then
                SelectIndexdvMyDatiopDownList(cboTemp, CStr(lngSelectedID))
            End If

        Catch myException As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ModLettureHone.FilldvMyDatiopDownSQL.errore: ", myException)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            dvMyDati.Dispose()
        End Try
    End Sub

    ''============================================================================================
    ''LETTURE NORMALI ATTUALI
    ''============================================================================================
    'Protected Overloads Sub MemoLettureNormaliATTUALI(ByVal storico As Boolean, ByVal blnConsumoNegativo As Boolean)

    '    ContatoreID = stringoperation.formatint(request("IDCONTATORE"))
    '    LetturaID = stringoperation.formatint(request("IDLettura"))

    '    Try

    '        Dim DetailContatore As New objContatore
    '        DetailContatore = DBContatori.GetDetailsContatori(ContatoreID)

    '        ''''''''''''''''''''''SALVA DATI''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '        Select Case stringoperation.formatint(request("PAG_PREC"))

    '            Case costanti.enmcontesto.DELETTURE

    '                dim sSQL as string
    '                Dim rd As new dataview
    '                Dim idPeriodo As Integer
    '                sSQL="SELECT * FROM TP_PERIODO WHERE (COD_ENTE='" & ConstSession.IdEnte & "') AND (ATTUALE=1)"
    '                rd = clsDBAcces.getdataview(sSQL)

    '                If rd.Read Then
    '                    idPeriodo = MyUtility.CIdFromDB(rd("CODPERIODO"))
    '                Else
    '                    Throw New Exception("Attenzione:configurare il periodo di fatturazione!")
    '                End If

    '                rd.Close()
    '                '=====================================================================================================================
    '                'SALVATAGGIO DELLE LETTURE NUOVE
    '                '=====================================================================================================================
    '                ' GestLetture.MemoLetture(storico, ContatoreID, idPeriodo, stringoperation.formatint(request("IDLETTURA")), DetailContatore.nidutente, stringoperation.formatint(request("PAG_PREC")), txtDatadiLettura.Text, _
    '                'txtLetturaAttuale.Text, txtGGConsumo.Text, txtLetturaTeorica.Text, txtConsEffettivo.Text, cboModalitaLettura.SelectedIndex, cboStatoLetturaPage.SelectedIndex, _
    '                'txtConsumoTeorico.Text, txtNoteLettura.Text, txtDataPassaggio.Text, chkLasciatoAvviso.Checked, chkDARicontrollare.Checked, False, False, False, _
    '                'False, False, lstAnomalieScelte, DetailContatore)
    '                GestLettureStoricheDEContatori.MemoLettureAttualiNORMALI(ContatoreID, idPeriodo, stringoperation.formatint(request("IDLETTURA")), _
    '                DetailContatore.nIdUtente, txtDatadiLettura.Text, txtLetturaAttuale.Text, txtGGConsumo.Text, txtLetturaTeorica.Text, _
    '                txtConsEffettivo.Text, cboModalitaLettura.SelectedItem.Value, cboStatoLetturaPage.SelectedItem.Value, txtConsumoTeorico.Text, chkFatturazione.Checked, _
    '                chkFatturazioneSospesa.Checked, chkIncongruenteForzato.Checked, chkGiroContatore.Checked, txtNoteLettura.Text, DetailContatore, lstAnomalie, lstAnomalieScelte, txtDataPassaggio.Text, chkLasciatoAvviso.Checked, blnConsumoNegativo)

    '                dim sScript as string=""

    '                
    '                sscript+="parent.opener.parent.Visualizza.frmHidden.action='Letture.aspx';")
    '                ' If lstAnomalieScelte.Items.Count > 0 Then
    '                'sscript+="parent.opener.parent.Visualizza.frmHidden.hdConAnomalie.value='True';")
    '                ' End If
    '                sscript+="parent.opener.parent.Visualizza.frmHidden.submit();")
    '                sscript+="parent.window.close();")
    '                

    '                RegisterScript(sScript , Me.GetType())
    '        End Select

    '    Catch ex As Exception
    '            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ModLettureHone.MemoLettureNormaliATTUALI.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '        lblError.Text = ex.Message
    '        '**************************************************************************************************************
    '        'Gestione Pagina Errore ed Invio E-Mail
    '        '**************************************************************************************************************
    '        ' _Err.SendEmailError(ex)
    '        'Response.Write(_Err.GetHTMLError(ex, "../Styles.css", "../images/testata_Utenze.jpg"))
    '        'Response.End()
    '    End Try
    'End Sub

    'Protected Overloads Sub MemoLettureNormaliATTUALI(ByVal strDataPassaggio As String, ByVal blnConsumoNegativo As Boolean)
    '    Try

    '        ContatoreID = stringoperation.formatint(request("IDCONTATORE"))
    '        LetturaID = stringoperation.formatint(request("IDLettura"))

    '        Dim DetailContatore As New objContatore
    '        DetailContatore = DBContatori.GetDetailsContatori(ContatoreID)
    '        ''''''''''''''''''''''SALVA DATI''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '        '**************************************************************************************************************
    '        '**************************************************************************************************************
    '        'GESTIONE DI INSERIMENTO MODIFICA LETTURA DA DECONTATORI
    '        '**************************************************************************************************************
    '        '**************************************************************************************************************
    '        dim sSQL as string
    '        Dim rd As new dataview
    '        Dim idPeriodo As Integer
    '        sSQL="SELECT * FROM TP_PERIODO WHERE (ATTUALE=1) AND (COD_ENTE='" & ConstSession.IdEnte & "')"
    '        rd = clsDBAcces.getdataview(sSQL)

    '        If rd.Read Then
    '            idPeriodo = MyUtility.CIdFromDB(rd("CODPERIODO"))
    '        Else
    '            Throw New Exception("Attenzione:configurare il periodo di fatturazione!")
    '        End If

    '        rd.Close()
    '        '***************************************************************************************************************************
    '        'SALVA I DATI DI LETTURA SE IL PERIODO E' STORICO NELLA TABELLA TP_LETTURE
    '        '***************************************************************************************************************************
    '        GestLettureStoricheDEContatori.MemoLettureAttualiNORMALI(ContatoreID, idPeriodo, stringoperation.formatint(request("IDLETTURA")), _
    '       DetailContatore.nIdUtente, txtNoteLettura.Text, DetailContatore, strDataPassaggio, lstAnomalie, lstAnomalieScelte, blnConsumoNegativo)

    '        dim sScript as string=""

    '        
    '        sscript+="parent.opener.parent.Visualizza.location.href='Letture.aspx?hdIDContatore=" & ContatoreID & "&PAG_PREC=" & stringoperation.formatint(request("PAG_PREC")) & "';")
    '        sscript+="parent.window.close();")
    '        

    '        RegisterScript(sScript , Me.GetType())
    '    Catch ex As Exception
    '            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ModLettureHone.MemoLettureNormaliATTUALI.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '        lblError.Text = ex.Message
    '        '**************************************************************************************************************
    '        'Gestione Pagina Errore ed Invio E-Mail
    '        '**************************************************************************************************************
    '        ' _Err.SendEmailError(ex)
    '        'Response.Write(_Err.GetHTMLError(ex, "../Styles.css", "../images/testata_Utenze.jpg"))
    '        'Response.End()
    '    End Try
    'End Sub

    'Protected Overloads Sub MemoLettureNormaliATTUALI(ByVal blnConsumoNegativo As Boolean)
    '    Try

    '        ContatoreID = stringoperation.formatint(request("IDCONTATORE"))
    '        LetturaID = stringoperation.formatint(request("IDLettura"))
    '        Dim DetailContatore As New objContatore
    '        DetailContatore = DBContatori.GetDetailsContatori(ContatoreID)

    '        ''''''''''''''''''''''SALVA DATI''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '        '**************************************************************************************************************
    '        '**************************************************************************************************************
    '        'GESTIONE DI INSERIMENTO MODIFICA LETTURA DA DECONTATORI
    '        '**************************************************************************************************************
    '        '**************************************************************************************************************
    '        dim sSQL as string
    '        Dim rd As new dataview
    '        Dim idPeriodo As Integer
    '        sSQL="SELECT * FROM TP_PERIODO WHERE (ATTUALE=1) AND (COD_ENTE='" & ConstSession.IdEnte & "')"
    '        rd = clsDBAcces.getdataview(sSQL)

    '        If rd.Read Then
    '            idPeriodo = MyUtility.CIdFromDB(rd("CODPERIODO"))
    '        Else
    '            Throw New Exception("Attenzione:configurare il periodo di fatturazione!")
    '        End If
    '        '***************************************************************************************************************************
    '        'SALVA I DATI DI LETTURA SE IL PERIODO E' STORICO NELLA TABELLA TP_LETTURE
    '        '***************************************************************************************************************************
    '        GestLettureStoricheDEContatori.MemoLettureAttualiNORMALI(ContatoreID, idPeriodo, stringoperation.formatint(request("IDLETTURA")), _
    '       DetailContatore.nIdUtente, txtNoteLettura.Text, txtDatadiLettura.Text, txtGGConsumo.Text, txtLetturaTeorica.Text, _
    '       txtConsEffettivo.Text, cboModalitaLettura.SelectedItem.Value, cboStatoLetturaPage.SelectedItem.Value, txtConsumoTeorico.Text, chkFatturazione.Checked, _
    '       chkFatturazioneSospesa.Checked, chkIncongruenteForzato.Checked, chkGiroContatore.Checked, DetailContatore, lstAnomalie, lstAnomalieScelte, blnConsumoNegativo)

    '        dim sScript as string=""

    '        
    '        sscript+="parent.opener.parent.Visualizza.location.href='Letture.aspx?hdIDContatore=" & ContatoreID & "&PAG_PREC=" & stringoperation.formatint(request("PAG_PREC")) & "';")
    '        sscript+="parent.window.close();")
    '        

    '        RegisterScript(sScript , Me.GetType())
    '    Catch ex As Exception
    '            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ModLettureHone.MemoLettureNormaliATTUALI.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '        lblError.Text = ex.Message
    '        '**************************************************************************************************************
    '        'Gestione Pagina Errore ed Invio E-Mail
    '        '**************************************************************************************************************
    '        ' _Err.SendEmailError(ex)
    '        'Response.Write(_Err.GetHTMLError(ex, "../Styles.css", "../images/testata_Utenze.jpg"))
    '        'Response.End()
    '    End Try
    'End Sub
    Private Sub LoadHidden(ByVal tDataLetturaAtt As DateTime, ByVal nLetturaAtt As Integer)
        Dim sScript As String = ""
        Try
            sScript += "document.getElementById('IDContatore').value='" & ContatoreID & "';"
            sScript += "document.getElementById('IDLettura').value='" & LetturaID & "';"
            If LetturaID > 0 Then
                sScript += "document.getElementById('hdDataLettura').value='" & tDataLetturaAtt & "';"
                sScript += "document.getElementById('hdLettura').value='" & nLetturaAtt & "';"
            End If
            RegisterScript(sScript, Me.GetType())


            sScript = "LettureLabel.style.display='';"
            sScript += "LettureField.style.display='';"
            sScript += "LettureFieldAnomalie.style.display='';"
            If StringOperation.FormatInt(Request("PAG_PREC")) = Costanti.enmContesto.DECONTATORI Then
                sScript += "ContatoriLabel.style.display='';"
                sScript += "ContatoriField.style.display='';"
                sScript += "SelezionePeriodo.style.display='';"
                sScript += "cboSelezionePeriodo.style.display='';"
            End If
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ModLettureHone.LoadHidden.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw New Exception("LoadHidden::", ex)
        End Try
    End Sub
    Private Sub LoadAnomalie()
        Dim dvMyDatiAnomalie As New DataView
        Dim AnomalieApplicate As New ListBox

        Try
            dvMyDatiAnomalie = GestLetture.getListAnomalie()
            lstAnomalie.DataSource = dvMyDatiAnomalie
            lstAnomalie.DataTextField = "DESCRIZIONE"
            lstAnomalie.DataValueField = "CODANOMALIA"
            lstAnomalie.DataBind()
            lstAnomalie.SelectedIndex = 0
            If LetturaID > 0 Then
                Dim li As ListItem
                For Each li In AnomalieApplicate.Items
                    lstAnomalieScelte.Items.Add(New ListItem(li.Text, li.Value.ToString))
                Next
                If AnomalieApplicate.Items.Count > 0 Then
                    '	txtDatadiLettura.Enabled = False
                    '	txtLetturaAttuale.Enabled = False
                    '	cboModalitaLettura.Enabled = False
                    '	cboStatoLetturaPage.Enabled = False
                    '	chkLasciatoAvviso.Enabled = False
                    '	chkDARicontrollare.Enabled = False
                    '	chkFatturazioneSosp.Enabled = False
                    '	chkFatturazioneSosp.Checked = False
                    '	chkLasciatoAvviso.Checked = False
                    '	chkDARicontrollare.Checked = False
                    '	txtIDAnomalia.Text = "3"
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ModLettureHone.ModLettureHome.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    Private Sub LoadDati(ByVal DetailContatore As objContatore, ByVal DettaglioLetture As ObjLettura)
        Dim dvMyDatiModalitaLettura, dvMyDatiStatoLettura As New DataView
        Dim bLastLettura As Boolean
        Dim oAnagrafica As New Anagrafica.DLL.GestioneAnagrafica()
        Dim oDettaglioAnagraficaUtente As New DettaglioAnagrafica
        Try
            If LetturaID > 0 Then
                bLastLettura = False
            Else
                bLastLettura = True
            End If
            '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
            'oDettaglioAnagraficaUtente = oAnagrafica.GetAnagrafica(DetailContatore.nIdUtente, ConstSession.CodTributo, -1, ConstSession.StringConnectionAnagrafica)
            oDettaglioAnagraficaUtente = oAnagrafica.GetAnagrafica(DetailContatore.nIdUtente, -1, String.Empty, ConstSession.DBType, ConstSession.StringConnectionAnagrafica, False)
            lblContatore.Text = "  " & "Lettura associata al contatore Matricola :  " & DetailContatore.sMatricola & " - Utente : " & oDettaglioAnagraficaUtente.Cognome & " " & oDettaglioAnagraficaUtente.Nome
            If LetturaID > 0 Then
                chkPrimaLett.Checked = DettaglioLetture.bIsPrimaLettura
            Else
                chkPrimaLett.Checked = False
            End If
            dvMyDatiModalitaLettura = GestLetture.getListModalitaLettura()
            FilldvMyDatiopDownSQL(cboModalitaLettura, dvMyDatiModalitaLettura, DettaglioLetture.nCodModoLett)
            dvMyDatiStatoLettura = GestLetture.getListStatoLetture()
            FilldvMyDatiopDownSQL(cboStatoLetturaPage, dvMyDatiStatoLettura, DettaglioLetture.nIdStatoLettura)
            If bLastLettura = True Then
                If DettaglioLetture.tDataLetturaAtt.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                    txtDatadiLetturaPrec.Text = DettaglioLetture.tDataLetturaAtt
                End If
                txtLetturaAttualePrec.Text = DettaglioLetture.nLetturaAtt
            Else
                If DettaglioLetture.tDataLetturaPrec.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                    txtDatadiLetturaPrec.Text = DettaglioLetture.tDataLetturaPrec
                End If
                txtLetturaAttualePrec.Text = DettaglioLetture.nLetturaPrec
                If DettaglioLetture.tDataLetturaAtt.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                    txtDatadiLettura.Text = DettaglioLetture.tDataLetturaAtt
                End If
                txtLetturaAttuale.Text = DettaglioLetture.nLetturaAtt
                If DettaglioLetture.nGiorni <> -1 Then
                    txtGGConsumo.Text = DettaglioLetture.nGiorni
                End If
                txtLetturaTeorica.Text = DettaglioLetture.sLetturaTeorica
                If DettaglioLetture.nConsumo <> -1 Then
                    txtConsEffettivo.Text = DettaglioLetture.nConsumo
                End If
                If DettaglioLetture.nConsumoTeorico <> -1 Then
                    txtConsumoTeorico.Text = DettaglioLetture.nConsumoTeorico
                End If
            End If
            txtNoteLettura.Text = DettaglioLetture.sNote
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ModLettureHone.LoadDati.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw New Exception("LoadDati::", ex)
        End Try
    End Sub
    Private Sub LockVideata()
        Try
            txtDatadiLettura.Enabled = False
            txtLetturaAttuale.Enabled = False
            txtGGConsumo.Enabled = False
            txtLetturaTeorica.Enabled = False
            txtConsEffettivo.Enabled = False
            cboModalitaLettura.Enabled = False
            cboStatoLetturaPage.Enabled = False
            txtConsumoTeorico.Enabled = False
            txtNoteLettura.Enabled = False
            chkFatturazione.Enabled = False
            chkFatturazioneSospesa.Enabled = False
            txtDataPassaggio.Enabled = False
            chkLasciatoAvviso.Enabled = False
            lstAnomalie.Enabled = False
            lstAnomalieScelte.Enabled = False
            GrdLettureHome.Enabled = False
            chkIncongruenteForzato.Enabled = False
            Button1.Enabled = False
            Button2.Enabled = False
            txtIDAnomalia.Enabled = False
            cboSelezionePeriodo.Enabled = False
            chkGiroContatore.Enabled = False
            chkConsumoNegativoForzato.Enabled = False
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ModLettureHone.LockVideata.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw New Exception("LockVideata::", ex)
        End Try
    End Sub
End Class
