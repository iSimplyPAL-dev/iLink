Imports System.Xml
Imports System.Data
Imports System.Text
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Reflection
Imports Anagrafica
Imports OggettiComuniStrade
'Imports WsStradario
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports log4net
Imports AnagInterface
Imports Utility

Partial Class Letture
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(Letture))
    Protected FncGrd As New ClsGenerale.FunctionGrd
    Private ModDate As New ClsGenerale.Generale
    Private FncGen As New Generali
    Dim ContatoreID As Integer
    Dim CodUtente As Integer
    Dim _Const As New Costanti
    Dim GestLetture As GestLetture = New GestLetture
    Dim DBContatori As GestContatori = New GestContatori
    Dim clsLetture As New clsLetture
    Dim clsDBAcces As New DBAccess.getDBobject
    Dim Generali As New Generali
    Private Shared blnGridActivate As Boolean
    Private Shared blnUpdateGrid As Boolean
    Private Shared m_lngIDPADRE As Long
    Private Shared m_lngDataGriglia, m_lngLettura As Long
    Private Shared m_strCodLettura, m_strDataLetturaGriglia, m_strLetturaGriglia, m_strFatturazioneSospesaGriglia, m_strFatturazioneGriglia, m_strIncongruenteForzatoGriglia, m_strCodModalitaLetturaGriglia, m_strIdStatoLetturaGriglia, m_strCodLetturaGriglia As String
    Protected UrlStradario As String = ConstSession.UrlStradario

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtPeriodo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtEnte As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtNomeUtente As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDatadiLettura As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtLetturaAttuale As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtGGConsumo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtLetturaTeorica As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtConsEffettivo As System.Web.UI.WebControls.TextBox
    Protected WithEvents cboModalitaLettura As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboStatoLetturaPage As System.Web.UI.WebControls.DropDownList
    Protected WithEvents txtConsumoTeorico As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDataPassaggio As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkLasciatoAvviso As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkDARicontrollare As System.Web.UI.WebControls.CheckBox
    Protected WithEvents txtNoteLettura As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtConsumoNegativo As System.Web.UI.WebControls.TextBox
    Protected WithEvents chkFatturazione As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkIncongruenteForzatoGrid As System.Web.UI.WebControls.CheckBox
    Protected WithEvents chkIncongruenteForzato As System.Web.UI.WebControls.CheckBox
    Protected WithEvents txtConfirm As System.Web.UI.WebControls.TextBox

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    Dim FncContatori As New GestContatori
    '    Dim drTipoContatore, drGiro, drPosizioneContatore As SqlDataReader
    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Dim WFErrore As String

    '    Try
    '        'inizializzo la connessione
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        '*** Fabi
    '        '**** Aggancio per stradario ****
    '        LnkPulisciStrada.Attributes.Add("onclick", "return ClearDatiVia();")
    '        LnkOpenStradario.Attributes.Add("onclick", "return ApriStradario('RibaltaStrada', '" & ConstSession.IdEnte & "')")
    '        '**** /Fabi

    '        Dim lngRecordCount As Long
    '        Dim getListaLetture As objDBListSQL
    '        dim sScript as string=""

    '        If stringoperation.formatint(request("hdIDContatore")) > 0 Then
    '            ContatoreID = stringoperation.formatint(request("hdIDContatore"))
    '            txtCodContatore.Text = ContatoreID
    '        Else
    '            ContatoreID = txtCodContatore.Text
    '            
    '            sscript+="document.getElementById('hdIDContatore.value='" & ContatoreID & "';"
    '            
    '            RegisterScript("CallPage", strBuilder.ToString())
    '        End If

    '        Dim DetailContatore As New objContatore
    '        DetailContatore = DBContatori.GetDetailsContatori(ContatoreID, WFSessione, CInt(ConstSession.IdEnte), ConstSession.CodIstat)
    '        'DetailContatore.drCodiceImpianto.Close()
    '        'DetailContatore.drFognatura.Close()
    '        'DetailContatore.drDepurazione.Close()
    '        'DetailContatore.drStrade.Close()
    '        'DetailContatore.drDiametroContatore.Close()
    '        'DetailContatore.drDiametroPresa.Close()
    '        'DetailContatore.drIVA.Close()


    '        '*** Fabi 05032008
    '        Dim codiceVia As Integer
    '        Dim nomeStrada As String
    '        Dim ubicazioneVia As String = ""

    '        If Not Page.IsPostBack Then
    '            If IsNumeric(ContatoreID) And ContatoreID <> -1 Then
    '                codiceVia = DetailContatore.nIdVia
    '                ubicazioneVia = DetailContatore.sUbicazione
    '            Else
    '                codiceVia = TxtCodVia.Text
    '                ubicazioneVia = TxtVia.Text
    '            End If
    '        Else
    '            codiceVia = TxtCodVia.Text
    '            ubicazioneVia = TxtVia.Text
    '        End If

    '        'Session("codVia") = codiceVia

    '        'If Session("codVia") = -1 And DetailContatore.Ubicazione <> "" Then
    '        '    nomeStrada = DetailContatore.Ubicazione
    '        'Else
    '        '    Dim objStradario As Stradario = New Stradario
    '        '    Dim ArrStrade() As OggettiComuniStrade.OggettoStrada
    '        '    Dim strada As New OggettiComuniStrade.OggettoStrada
    '        '    strada.CodiceStrada = Session("codVia")
    '        '    strada.CodiceEnte = CInt(ConstSession.IdEnte)
    '        '    ArrStrade = objStradario.GetStrade(strada)
    '        '    If Not ArrStrade Is Nothing Then
    '        '        If ArrStrade.Length.CompareTo(1) = 0 Then
    '        '            nomeStrada = ArrStrade(0).TipoStrada.ToString() + " " + ArrStrade(0).DenominazioneStrada.ToString()
    '        '        Else
    '        '            nomeStrada = ""
    '        '            codiceVia = -1
    '        '        End If
    '        '    Else
    '        '        codiceVia = -1
    '        '        nomeStrada = ""
    '        '    End If
    '        'End If

    '        TxtVia.Text = ubicazioneVia
    '        TxtCodVia.Text = codiceVia
    '        '*** /Fabi

    '        Dim DetailLetture As DetailsLetture = GestLetture.GetDetailsLetture(ContatoreID)

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
    '        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '        'oDettaglioAnagraficaUtente = oAnagrafica.GetAnagrafica(DetailContatore.nidutente, COD_TRIBUTO, -1)
    '        oDettaglioAnagraficaUtente = oAnagrafica.GetAnagrafica(DetailContatore.nIdUtente, COD_TRIBUTO, -1)

    '        strBuilder = New System.Text.StringBuilder
    '        
    '        Dim nidPeriodo As Integer

    '        If ConstSession.IdPeriodo <> 0 Then
    '            nidPeriodo = CInt(ConstSession.IdPeriodo)
    '        Else
    '            nidPeriodo = -1
    '        End If

    '        Select Case stringoperation.formatint(request("PAG_PREC"))

    '            Case costanti.enmcontesto.DELETTURE

    '                If GestLetture.VerificaVecchioContatore(ContatoreID, DetailContatore.nIdUtente, nidPeriodo, m_lngIDPADRE) Then
    '                    btnVecchioContatore.Visible = True
    '                    ''''Ale Cao 01/04/08
    '                    txtCodVecchioContatore.Text = CStr(DetailContatore.nIdContatorePrec)
    '                    ''''
    '                    btnVecchioContatore.Attributes.Add("OnClick", "return VisualizzaVecchioContatore();")
    '                    'txtCodVecchioContatore.Text = CStr(m_lngIDPADRE)
    '                End If

    '                sscript+="document.getElementById('hdIDContatore.value='" & ContatoreID & "';"
    '                sscript+="document.getElementById('hdCodiceVia.value='" & Request("hdCodiceVia") & "';"
    '                sscript+="document.getElementById('hdIntestatario.value='" & Request("hdIntestatario") & "';"
    '                sscript+="document.getElementById('hdUtente.value='" & Request("hdUtente") & "';"
    '                sscript+="document.getElementById('hdGiro.value='" & Request("hdGiro") & "';"
    '                sscript+="document.getElementById('hdNumeroUtente.value='" & Request("hdNumeroUtente") & "';"
    '                sscript+="document.getElementById('hdUbicazioneText.value='" & Request("hdUbicazioneText") & "';"
    '                sscript+="document.getElementById('hdCessati.value='" & Request("hdCessati") & "';"
    '                sscript+="document.getElementById('hdMATRICOLA.value='" & Request("hdMatricola") & "';"
    '                sscript+="document.getElementById('paginacomandi.value='" & Request("paginacomandi") & "';"
    '                sscript+="document.getElementById('PAG_PREC.value='" & costanti.enmcontesto.DELETTURE & "';"

    '                'Gestione de campi editabili e della pulsanteria se arrivo da DELetture
    '                dim sScript as string=""
    '                sScript +="")
    '                sScript +="document.getElementById('hdEnteAppartenenzaLetture.value='" & Session("CODCOMUNEENTE") & "';"
    '                sScript +="document.getElementById('hdCodiceViaLetture.value='" & DetailContatore.nIdVia & "';"
    '                sScript +="document.getElementById('hdSceltaViaLetture.value='1';")

    '                sScript +="")


    '                RegisterScript("Hidden", strBuilderHidden.ToString())

    '                '*** Fabi
    '                'cboUbicazione.Enabled = True
    '                '*** /Fabi
    '                txtNCivico.Enabled = True
    '                cboGiro.Enabled = True
    '                cboPosizione.Enabled = True
    '                txtSequenza.Enabled = True
    '                txtLatoStrada.Enabled = True
    '                txtProgressivo.Enabled = True
    '                txtNoteContatore.Enabled = True
    '                txtCifreContatore.Enabled = True

    '                txtNCivico.ReadOnly = False
    '                txtEsponente.ReadOnly = False
    '                txtSequenza.ReadOnly = False
    '                txtLatoStrada.ReadOnly = False
    '                txtProgressivo.ReadOnly = False
    '                txtNoteContatore.ReadOnly = False

    '                txtMatricola.ReadOnly = False
    '                cboTipoContatore.Enabled = True

    '                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/

    '            Case costanti.enmcontesto.DECONTATORI

    '                cboGiro.Enabled = False
    '                cboPosizione.Enabled = False

    '                '*** Fabi
    '                'cboUbicazione.Enabled = False
    '                '*** /Fabi

    '                txtEsponente.Enabled = False
    '                txtNCivico.ReadOnly = True
    '                txtSequenza.ReadOnly = True
    '                txtLatoStrada.ReadOnly = True
    '                txtProgressivo.ReadOnly = True
    '                txtNoteContatore.ReadOnly = True
    '                txtMatricola.Enabled = False
    '                cboTipoContatore.Enabled = False
    '                txtCifreContatore.Enabled = False
    '                dim sScript as string=""
    '                sScript +="")
    '                sScript +="document.getElementById('hdSceltaViaLetture.value='0';")
    '                sScript +="")
    '                RegisterScript("Hidden", strBuilderHidden.ToString())

    '                sscript+="document.getElementById('hdIDContatore.value='" & ContatoreID & "';"
    '                sscript+="document.getElementById('PAG_PREC.value='" & costanti.enmcontesto.DECONTATORI & "';"

    '        End Select
    '        
    '        RegisterScript("SetHidden", strBuilder.ToString())

    '        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '        If stringoperation.formatint(request("PAG_PREC")) = costanti.enmcontesto.DELETTURE Then
    '            getListaLetture = GestLetture.BindData(ContatoreID, DetailContatore.nIdUtente)
    '        Else
    '            getListaLetture = GestLetture.BindData(ContatoreID, DetailContatore.nIdUtente)
    '        End If

    '        lblContatore.Text = "  " & " Letture associate al Contatore Matricola:  " & DetailContatore.sMatricola & " - Utente : " & oDettaglioAnagraficaUtente.Cognome & " " & oDettaglioAnagraficaUtente.Nome
    '        CodUtente = DetailContatore.nIdUtente

    '        grdLetture.DataKeyField = "CODLETTURA"
    '        lngRecordCount = getListaLetture.RecordCount

    '        Select Case lngRecordCount
    '            Case 0
    '                info.Text = "Non sono presenti Letture"

    '            Case Is > 0
    '                grdLetture.cnnConn = getListaLetture.oConn
    '                grdLetture.sSQL= getListaLetture.Query
    '                grdLetture.strSqlCountRecord = ""
    '                grdLetture._NumberRecord = lngRecordCount
    '        End Select

    '        If Not Page.IsPostBack Then

    '            drTipoContatore = FncContatori.getListTipoContatore()
    '            FncGen.FillDropDownSQL(cboTipoContatore, drTipoContatore, DetailContatore.nTipoContatore)

    '            drGiro = FncContatori.getListGiro(ConstSession.IdEnte)
    '            FncGen.FillDropDownSQL(cboGiro, drGiro, DetailContatore.nGiro)

    '            drPosizioneContatore = FncContatori.getListPosizioneContatore()
    '            FncGen.FillDropDownSQL(cboPosizione, drPosizioneContatore, DetailContatore.nPosizione)

    '            '*** Fabi
    '            'FillDropDownSQLStrade(cboUbicazione, DetailContatore.drStrade, Detailcontatore.nidvia)
    '            '*** /Fabi

    '            '''''''''''''''''''/BindDataGrid''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''//
    '            If lngRecordCount > 0 Then
    '                grdLetture.Rows.Count = 0
    '                grdLetture.BindData()
    '            End If

    '            '''''''''''''''''''/Fine BindDataGrid''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''//

    '            txtEnteAppartenenza.Text = ConstSession.DescrizioneEnte
    '            If DetailContatore.nIdImpianto <> -1 Then
    '                txtImpianto.Text = DetailContatore.nIdImpianto
    '            End If
    '            'txtUbicazione.Text = DetailContatore.Ubicazione

    '            txtNCivico.Text = DetailContatore.sCivico
    '            txtEsponente.Text = DetailContatore.sEsponenteCivico
    '            txtSequenza.Text = DetailContatore.sSequenza
    '            txtLatoStrada.Text = DetailContatore.sLatoStrada
    '            txtProgressivo.Text = DetailContatore.sProgressivo
    '            txtMatricola.Text = DetailContatore.sMatricola
    '            txtNoteContatore.Text = DetailContatore.sNote
    '            txtDataAttivazione.Text = DetailContatore.sDataAttivazione
    '            txtDataSostituzione.Text = DetailContatore.sDataSostituzione
    '            txtDataRimTemp.Text = DetailContatore.sDataRimTemp
    '            txtDataCessazione.Text = DetailContatore.sDataCessazione
    '            txtNumeroUtenze.Text = DetailContatore.nNumeroUtenze
    '            txtNUtente.Text = DetailContatore.sNumeroUtente
    '            txtTipoUtenza.Text = DetailLetture.TipoUtenza
    '            txtMinFatt.Text = DetailLetture.MinimoFatturabile
    '            txtMinFattRim.Text = DetailLetture.MinFattRim

    '            If Len(Trim(DetailContatore.sCifreContatore)) = 0 Then
    '                lblInformation.Text = "Attenzione:non sono state assegnate le cifre contatore.Impossibile verificare Giro Contatore,durante inserimento letture."
    '            Else
    '                txtCifreContatore.Text = DetailContatore.sCifreContatore
    '            End If

    '            dim sScript as string=""
    '            sScript +="")
    '            sScript +=" document.getElementById('txtSequenza.focus();")
    '            sScript +="document.getElementById('txtSequenza.select();")
    '            sScript +="")

    '            RegisterScript("focus", strFocus.ToString())

    '        End If
    '        grdLetture.MouseSelectableDataGrid()

    '        'btnConferma.Attributes.Add("OnClick", "return VerificaCampi();")
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Letture.Page_Load.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    Finally
    '        WFSessione.Kill()
    '    End Try
    'End Sub
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim FncContatori As New GestContatori
        Dim sScript As String = ""
        Try

            ''*** Fabi
            ''**** Aggancio per stradario ****
            'LnkPulisciStrada.Attributes.Add("onclick", "return ClearDatiVia();")
            'LnkOpenStradario.Attributes.Add("onclick", "return ApriStradario('RibaltaStrada', '" & ConstSession.IdEnte & "')")
            ''**** /Fabi

            ContatoreID = StringOperation.FormatInt(Request("hdIDContatore"))
            If ContatoreID > 0 Then
                txtCodContatore.Text = ContatoreID
            Else
                ContatoreID = txtCodContatore.Text
                RegisterScript("document.getElementById('hdIDContatore.value='" & ContatoreID & "';", Me.GetType())
            End If

            Dim myContatore As New objContatore
            Dim codiceVia As Integer
            Dim ubicazioneVia As String = ""
            'Dim DetailLetture As DetailsLetture
            Dim oAnagrafica As New Anagrafica.DLL.GestioneAnagrafica()
            Dim oDettaglioAnagraficaUtente As New DettaglioAnagrafica

            If Not Page.IsPostBack Then
                If IsNumeric(ContatoreID) And ContatoreID <> -1 Then
                    myContatore = DBContatori.GetDetailsContatori(ContatoreID, -1) ', CInt(ConstSession.IdEnte))
                    'DetailLetture = GestLetture.GetDetailsLetture(ContatoreID)
                    '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
                    'oDettaglioAnagraficaUtente = oAnagrafica.GetAnagrafica(myContatore.nIdUtente, ConstSession.CodTributo, -1, ConstSession.StringConnectionAnagrafica)
                    oDettaglioAnagraficaUtente = oAnagrafica.GetAnagrafica(myContatore.nIdUtente, -1, String.Empty, ConstSession.DBType, ConstSession.StringConnectionAnagrafica, False)
                    Dim dv As DataView = GestLetture.BindData(ContatoreID)
                    If Not IsNothing(dv) Then
                        If dv.Count <= 0 Then
                            GrdLetture.Visible = False
                        Else
                            GrdLetture.Visible = True
                            GrdLetture.DataSource = dv
                            GrdLetture.DataBind()
                        End If
                    End If

                    lblContatore.Text = "  " & " Letture associate al Contatore Matricola:  " & myContatore.sMatricola & " - Utente : " & oDettaglioAnagraficaUtente.Cognome & " " & oDettaglioAnagraficaUtente.Nome
                    codiceVia = myContatore.nIdVia
                    ubicazioneVia = myContatore.sUbicazione

                    LoadCombos(myContatore)
                    'txtEnteAppartenenza.Text = ConstSession.DescrizioneEnte
                    'If myContatore.nIdImpianto <> -1 Then
                    '    txtImpianto.Text = myContatore.nIdImpianto
                    'End If
                    'txtUbicazione.Text = DetailContatore.Ubicazione
                    txtNCivico.Text = myContatore.sCivico
                    txtEsponente.Text = myContatore.sEsponenteCivico
                    txtSequenza.Text = myContatore.sSequenza
                    txtLatoStrada.Text = myContatore.sLatoStrada
                    txtProgressivo.Text = myContatore.sProgressivo
                    'txtMatricola.Text = DetailContatore.sMatricola
                    txtNoteContatore.Text = myContatore.sNote
                    txtDataAttivazione.Text = myContatore.sDataAttivazione
                    txtDataSostituzione.Text = myContatore.sDataSostituzione
                    txtDataRimTemp.Text = myContatore.sDataRimTemp
                    txtDataCessazione.Text = myContatore.sDataCessazione
                    txtNumeroUtenze.Text = myContatore.nNumeroUtenze
                    chkEsenteFognatura.Checked = myContatore.bEsenteFognatura
                    chkEsenteDepurazione.Checked = myContatore.bEsenteDepurazione
                    chkEsenteAcqua.Checked = myContatore.bEsenteAcqua
                    '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
                    chkEsenteAcquaQF.Checked = myContatore.bEsenteAcquaQF
                    chkEsenteDepQF.Checked = myContatore.bEsenteDepQF
                    chkEsenteFogQF.Checked = myContatore.bEsenteFogQF
                    'txtNUtente.Text = DetailContatore.sNumeroUtente
                    'txtTipoUtenza.Text = DetailLetture.TipoUtenza
                    'txtMinFatt.Text = DetailLetture.MinimoFatturabile
                    'txtMinFattRim.Text = DetailLetture.MinFattRim
                    If Len(Trim(myContatore.sCifreContatore)) = 0 Then
                        lblInformation.Text = "Attenzione:non sono state assegnate le cifre contatore.Impossibile verificare Giro Contatore,durante inserimento letture."
                    Else
                        txtCifreContatore.Text = myContatore.sCifreContatore
                    End If

                    'dim sScript as string=""
                    'sScript +="")
                    'sScript +=" document.getElementById('txtSequenza.focus();")
                    'sScript +="document.getElementById('txtSequenza.select();")
                    'sScript +="")
                    'RegisterScript("focus", strFocus.ToString())

                    Select Case StringOperation.FormatInt(Request("PAG_PREC"))
                        Case CStr(Costanti.enmContesto.DELETTURE)
                            sScript += "document.getElementById('hdIDContatore').value='" & ContatoreID & "';"
                            sScript += "document.getElementById('hdCodiceVia').value='" & Request("hdCodiceVia") & "';"
                            sScript += "document.getElementById('hdIntestatario').value='" & Request("hdIntestatario") & "';"
                            sScript += "document.getElementById('hdUtente').value='" & Request("hdUtente") & "';"
                            sScript += "document.getElementById('hdGiro').value='" & Request("hdGiro") & "';"
                            sScript += "document.getElementById('hdNumeroUtente').value='" & Request("hdNumeroUtente") & "';"
                            sScript += "document.getElementById('hdUbicazioneText').value='" & Request("hdUbicazioneText") & "';"
                            sScript += "document.getElementById('hdCessati').value='" & Request("hdCessati") & "';"
                            sScript += "document.getElementById('hdMATRICOLA').value='" & Request("hdMatricola") & "';"
                            sScript += "document.getElementById('paginacomandi').value='" & Request("paginacomandi") & "';"
                            sScript += "document.getElementById('PAG_PREC').value='" & Costanti.enmContesto.DELETTURE & "';"

                            'Gestione de campi editabili e della pulsanteria se arrivo da DELetture
                            sScript += "document.getElementById('hdEnteAppartenenzaLetture').value='" & ConstSession.IdEnte & "';"
                            sScript += "document.getElementById('hdCodiceViaLetture').value='" & myContatore.nIdVia & "';"
                            sScript += "document.getElementById('hdSceltaViaLetture').value='1';"
                            RegisterScript(sScript, Me.GetType())

                            ''*** Fabi
                            ''cboUbicazione.Enabled = True
                            ''*** /Fabi
                            'txtNCivico.Enabled = True
                            'cboGiro.Enabled = True
                            'cboPosizione.Enabled = True
                            'txtSequenza.Enabled = True
                            'txtLatoStrada.Enabled = True
                            'txtProgressivo.Enabled = True
                            'txtNoteContatore.Enabled = True
                            'txtCifreContatore.Enabled = True

                            'txtNCivico.ReadOnly = False
                            'txtEsponente.ReadOnly = False
                            'txtSequenza.ReadOnly = False
                            'txtLatoStrada.ReadOnly = False
                            'txtProgressivo.ReadOnly = False
                            'txtNoteContatore.ReadOnly = False

                            ''txtMatricola.ReadOnly = False
                            'cboTipoContatore.Enabled = True

                        Case CStr(Costanti.enmContesto.DECONTATORI)
                            'cboGiro.Enabled = False
                            'cboPosizione.Enabled = False
                            ''*** Fabi
                            ''cboUbicazione.Enabled = False
                            ''*** /Fabi
                            'txtEsponente.Enabled = False
                            'txtNCivico.ReadOnly = True
                            'txtSequenza.ReadOnly = True
                            'txtLatoStrada.ReadOnly = True
                            'txtProgressivo.ReadOnly = True
                            'txtNoteContatore.ReadOnly = True
                            ''txtMatricola.Enabled = False
                            'cboTipoContatore.Enabled = False
                            'txtCifreContatore.Enabled = False
                            sScript = "document.getElementById('hdSceltaViaLetture').value='0';"
                            sScript += "document.getElementById('hdIDContatore').value='" & ContatoreID & "';"
                            sScript += "document.getElementById('PAG_PREC').value='" & Costanti.enmContesto.DECONTATORI & "';"
                            RegisterScript(sScript, Me.GetType())
                    End Select
                    RegisterScript(sScript, Me.GetType())
                Else
                    'codiceVia = TxtCodVia.Text
                    ubicazioneVia = TxtVia.Text


                End If
                'Else
                '    'test 29052017
                '    myContatore = DBContatori.GetDetailsContatori(ContatoreID, -1) ', CInt(ConstSession.IdEnte))
                '    DetailLetture = GestLetture.GetDetailsLetture(ContatoreID)
                '    '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
                '    'oDettaglioAnagraficaUtente = oAnagrafica.GetAnagrafica(myContatore.nIdUtente, ConstSession.CodTributo, -1, ConstSession.StringConnectionAnagrafica)
                '    oDettaglioAnagraficaUtente = oAnagrafica.GetAnagrafica(myContatore.nIdUtente, -1, String.Empty, ConstSession.DBType, ConstSession.StringConnectionAnagrafica, False)
                '    Dim dv As DataView = GestLetture.BindData(ContatoreID)
                '    If Not IsNothing(dv) Then
                '        If dv.Count <= 0 Then
                '            GrdLetture.Visible = False
                '        Else
                '            GrdLetture.Visible = True
                '            GrdLetture.DataSource = dv
                '            GrdLetture.DataBind()
                '        End If
                '    End If
                '    'test 29052017

                '    'codiceVia = TxtCodVia.Text
                '    ubicazioneVia = TxtVia.Text
            End If
            TxtVia.Text = ubicazioneVia
            'TxtCodVia.Text = codiceVia
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Letture.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''''''''''''''''''''''''''''''''''''''''FUNZIONI PER LA FORMATTAZIONE DEI CAMPI DELLA GRIGLIA'''''''''''''''''''''''''''''''''''''''
    Protected Function CheckIndex(ByVal prdStatus As Object) As Integer
        CheckIndex = MyUtility.CIdFromDB(prdStatus)
        Return CheckIndex
    End Function


    Protected Function PopulateDropDownList(ByVal sSQL As String, ByVal strNameTable As String, ByVal DataValueField As String, ByVal DataTextField As String) As DataSet

        Dim dataAdapter As SqlDataAdapter  ' sqldatasetcommand
        Dim DS As New DataSet
        Dim sqlConn As New SqlConnection
        sqlConn.ConnectionString = ConstSession.StringConnection
        sqlConn.Open()
        dataAdapter = New SqlDataAdapter(sSQL, sqlConn)
        dataAdapter.SelectCommand.CommandType =
           CommandType.Text

        dataAdapter.Fill(DS, strNameTable)  'filldataset
        Dim dt As DataTable = DS.Tables(0)
        Dim rowNull As DataRow = dt.NewRow()
        rowNull(DataTextField) = "..."
        rowNull(DataValueField) = "-1"
        DS.Tables(0).Rows.InsertAt(rowNull, 0)

        Return DS

    End Function

    ''''''''''''''''''''''''''''''''''''''''FINE FUNZIONI PER LA FORMATTAZIONE DEI CAMPI DELLA GRIGLIA'''''''''''''''''''''''''''''''''''''''

    Private Sub LoadCombos(ByVal myContatore As objContatore)
        Dim dvMyDati As New DataView
        Dim FncContatori As New GestContatori
        Try
            dvMyDati = FncContatori.getListCodiceImpianto()
            FncGen.FillDropDownSQL(cboImpianto, dvMyDati, myContatore.nIdImpianto)

            dvMyDati = FncContatori.getListTipoUtenza(ConstSession.IdEnte, myContatore.sDataAttivazione, myContatore.nTipoUtenza)
            FncGen.FillDropDownSQL(cboTipoUtenze, dvMyDati, myContatore.nTipoUtenza)

            dvMyDati = FncContatori.getListTipoContatore()
            FncGen.FillDropDownSQL(cboTipoContatore, dvMyDati, myContatore.nTipoContatore)

            dvMyDati = FncContatori.getListGiro(ConstSession.IdEnte)
            FncGen.FillDropDownSQL(cboGiro, dvMyDati, myContatore.nGiro)

            dvMyDati = FncContatori.getListPosizioneContatore()
            FncGen.FillDropDownSQL(cboPosizione, dvMyDati, myContatore.nPosizione)

        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Letture.LoadCombos.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

#Region "Griglie"
    Protected Sub GrdRowCommand(ByVal sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim sScript As String = ""
            Dim sIsFatt As String = ""
            Dim IDRow As Integer = CInt(e.CommandArgument.ToString())
            '*** se ho una variazione in corso la videata è bloccata alle variazioni ***
            If StringOperation.FormatString(Request("bIsInVar")) <> "True" Then
                If e.CommandName = "RowOpen" Then
                    For Each myRow As GridViewRow In GrdLetture.Rows
                        If CType(myRow.FindControl("hfIdLettura"), HiddenField).Value = IDRow Then
                            Select Case StringOperation.FormatString(CType(myRow.FindControl("FATTURATA"), Label).Text)
                                Case "Si"
                                    sIsFatt = "1"
                                Case Else
                                    sIsFatt = "0"
                            End Select
                            sScript += "ModificaLetture(" & IDRow.ToString & "," + sIsFatt + ");"
                            RegisterScript(sScript, Me.GetType())
                            Exit For
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Letture.GrdRowDataBound.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Public Sub grdLetture_OnItemDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles GrdLetture.ItemDataBound
    '    Dim lblSID As Label
    '    Dim LblIsFatturata As Label
    '    Dim strArguments, sIsFatt As String
    'Try
    '    '*** se ho una variazione in corso la videata è bloccata alle variazioni ***
    '    If Request("bIsInVar") <> "True" Then
    '        Select Case e.Item.ItemType
    '            Case ListItemType.Item, ListItemType.AlternatingItem
    '                lblSID = CType(e.Item.FindControl("CODLETTURA"), Label)
    '                LblIsFatturata = CType(e.Item.FindControl("FATTURATA"), Label)
    '                strArguments = "'" & lblSID.Text & "' "
    '                Select Case LblIsFatturata.Text
    '                    Case "Si"
    '                        sIsFatt = "1"
    '                    Case "No"
    '                        sIsFatt = "0"
    '                End Select
    '                e.Item.Attributes.Add("OnClick", "ModificaLetture(" & strArguments & "," + sIsFatt + ");")
    '        End Select
    '    End If
    ' Catch ex As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Letture.grdLetture_OnItemDataBound.errore: ", ex)
    '      Response.Redirect("../../PaginaErrore.aspx")
    'End Try

    'End Sub
#End Region
    Private Sub btnReloadGrdLetture_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReloadGrdLetture.Click
        Try
            'Page_Load(sender, e)
            ContatoreID = StringOperation.FormatInt(Request("hdIDContatore"))
            Dim dv As DataView = GestLetture.BindData(ContatoreID)
            If Not IsNothing(dv) Then
                If dv.Count <= 0 Then
                    GrdLetture.Visible = False
                Else
                    GrdLetture.Visible = True
                    GrdLetture.DataSource = dv
                    GrdLetture.DataBind()
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Letture.btnReloadGrdLetture_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class




