Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports AnagInterface
Imports RIBESElaborazioneDocumentiInterface
Imports RIBESElaborazioneDocumentiInterface.Stampa
Imports RIBESElaborazioneDocumentiInterface.Stampa.oggetti
Imports log4net
Imports OggettiComuniStrade
'Imports WsStradario
Imports RemotingInterfaceAnater
Imports OPENgov_AgenziaEntrate
Imports Utility

Partial Class DatiGeneraliContr
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(DatiGeneraliContr))
    Protected FncGrd As New ClsGenerale.FunctionGrd
    Private FncGen As New Generali
    Dim _Const As New Costanti
    Dim DBAccess As New DBAccess.getDBobject
    Public ContrattoID As Integer
    Public FncContratti As New GestContratti
    Public oMyContratto As New objContratto
    Protected UrlStradario As String = ConstSession.UrlStradario
    Public nonmodificabile As Boolean

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtMatricola As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCodFisUte As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtContatorePrecedente As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtContatoreSuccessivo As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDataSostituzione As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDataRimTemp As System.Web.UI.WebControls.TextBox
    'Protected WithEvents grdContatori As RibesDataGrid.RibesDataGrid.RibesDataGrid
    Protected WithEvents hdcodanagrafeintestatario As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents txtQuoteAgevolate As System.Web.UI.WebControls.TextBox
    Protected WithEvents cboUbicazione As System.Web.UI.WebControls.DropDownList
    Protected WithEvents provaCAI As System.Web.UI.WebControls.TextBox
    Protected WithEvents provaCAU As System.Web.UI.WebControls.TextBox
    Protected WithEvents IDContatore As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents txtpiano As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtfoglio As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtnumero As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtsubalterno As System.Web.UI.WebControls.TextBox
    Protected WithEvents LnkAnagTributi As System.Web.UI.WebControls.ImageButton

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
        Dim sScript As String = ""
        Dim FncContatori As New GestContatori
        Dim dvCodiceImpianto, dvTipoContatore, dvGiro, dvPosizioneContatore, dvFognatura, dvDepurazione, dvDiametroContatore, dvDiametroPresa, dvTipoUtenza As DataView

        Try
            If TxtDataCessazioneRibaltata.Text <> "" Then
                txtDataCessazione.Text = TxtDataCessazioneRibaltata.Text
            End If

            '*** Fabi ****
            '*** Aggancio ricerca anagrafica anater ***
            LnkAnagAnater.Attributes.Add("onclick", "ApriRicAnater();")
            LnkAnagrAnatUtente.Attributes.Add("onclick", "ApriRicAnater();")

            '**** Aggancio per stradario ****
            LnkPulisciStrada.Attributes.Add("onclick", "return ClearDatiVia();")
            LnkOpenStradario.Attributes.Add("onclick", "return ApriStradario('RibaltaStrada', '" & ConstSession.IdEnte & "')")
            '**** /Fabi

            LnkAnagAnater.Attributes.Add("onclick", "ApriRicAnater();")
            btnAttivaContatore.Attributes.Add("onclick", "attivaContatore()")
            Session("precedentecatasto") = "Contratti"

            Dim codiceVia As Integer
            Dim ubicazione As String = ""

            If chkEsenteFognatura.Checked = True Then
                cboFognatura.Enabled = False
            End If

            If chkEsenteDepurazione.Checked = True Then
                cboDepurazione.Enabled = False
            End If

            If miointestatario.Text = "" Then
                miointestatario.Text = -1
            End If

            If mioutente.Text = "" Then
                mioutente.Text = -1
            End If

            If Not Page.IsPostBack Then
                HDtxtCodIntestatario.Text = -1
                HDTextCodUtente.Text = -1
                Session.Remove("datacatasto")

                ContrattoID = CInt(Request.Params("IDCONTRATTO"))
                If ContrattoID > 0 Then
                    txtidContatore.Text = ContrattoID
                Else
                    txtidContatore.Text = ""
                End If
                'prelevo i dati del contratto
                oMyContratto = FncContratti.GetDetailsContratti(ContrattoID, CInt(ConstSession.IdEnte))
                hfIDContatore.Value = oMyContratto.oContatore.nIdContatore
                '*** 20130328 - popolo la griglia dei riferimenti catastali***
                If Not IsNothing(oMyContratto.oContatore.oDatiCatastali) Then
                    GrdRifCat.DataSource = oMyContratto.oContatore.oDatiCatastali
                    GrdRifCat.DataBind()
                    GrdRifCat.Visible = True
                    LblResRifCat.Visible = False
                Else
                    GrdRifCat.Visible = False
                    LblResRifCat.Visible = True
                End If
                '*** ***

                '*** prelevo il numero utente ***
                Dim FncMy As New GestContratti

                If txtNumeroUtente.Text = "" Then
                    txtNumeroUtente.Text = FncMy.GetNumeroUtente(ConstSession.IdEnte, 0)
                End If
                '*** ***

                '*** controllo se ho una variazione in corso per il contatore ***
                Log.Debug("controllo se ho una variazione in corso per il contatore")
                Dim FncVar As New ClsRibaltaVar
                Dim bIsInVar As Boolean
                If Not IsNothing(FncVar.GetRicercaVariazioni(ConstSession.IdEnte, ClsRibaltaVar.sTypeRicerca.VARFATT_DAGESTIRE, oMyContratto.oContatore.nIdContatore, -1)) Then
                    bIsInVar = True
                Else
                    bIsInVar = False
                End If
                '*** ***

                '*** prelevo l'ultimo codice contratto inserito ***
                Log.Debug("prelevo l'ultimo codice contratto inserito")
                txtLastIdContratto.Text = FncContratti.GetLastCodContratto(ConstSession.IdEnte)
                '*** ***

                '***carico la pagina dei comandi***
                Dim paginacomandi As String = "ComandiDataEntryContratti.aspx" 'ConstSession.PathApplicazione & ConstSession.Path_H2O & "/DataEntryContratti/ComandiDataEntryContratti.aspx"
                Dim parametri As String = "?"
                '*** se ho una variazione in corso la DataScaricoPDA diventa valorizzata in questo modo la videata è bloccata alle variazioni ***
                If bIsInVar = True Then
                    parametri += "&DataScaricoPDA=" & Now
                Else
                    parametri += "&DataScaricoPDA=" & Request.Item("DataScaricoPDA")
                End If
                '*** ***
                sScript += "parent.Comandi.location.href='" & paginacomandi & parametri & "';"
                RegisterScript(sScript, Me.GetType())
                '**********************************

                txtEnteAppartenenza.Text = ConstSession.DescrizioneEnte
                '***gestione parametri di ricerca della videata precedente***
                sScript += "document.getElementById('IDContratto').value='" & ContrattoID & "';"
                sScript += "document.getElementById('hdCodVia').value='" & Request("hdCodiceVia") & "';"
                sScript += "document.getElementById('hdIntestatario').value='" & UCase(Request("hdIntestatario")) & "';"
                sScript += "document.getElementById('hdUtente').value='" & UCase(Request("hdUtente")) & "';"
                If Not Request("hdNumeroUtente") Is Nothing Then
                    sScript += "document.getElementById('hdNumeroUtente').value='" & Request("hdNumeroUtente") & "';"
                Else
                    sScript += "document.getElementById('hdNumeroUtente').value='';"
                End If
                If Not Request("hdMatricola") Is Nothing Then
                    sScript += "document.getElementById('hdMatricola').value='" & UCase(Request("hdMatricola")) & "';"
                Else
                    sScript += "document.getElementById('hdMatricola').value='';"
                End If
                sScript += "document.getElementById('hdAvviaRicerca').value='" & UCase(Request("hdAvviaRicerca")) & "';"

                RegisterScript(sScript, Me.GetType())
                '************************************************************

                'carico i dati del contratto
                sScript = "document.getElementById('hdEnteAppartenenza').value='" & ConstSession.IdEnte & "';"
                sScript += "document.getElementById('hdCodAnagrafeUtente').value='" & oMyContratto.nIdUtente & "';"
                sScript += "document.getElementById('hdCodContratto').value='" & oMyContratto.nIdContratto & "';"
                sScript += "document.getElementById('hdCodContatore').value='" & oMyContratto.oContatore.nIdContatore & "';"
                sScript += "document.getElementById('hdDataSottoScrizione').value='" & oMyContratto.sDataSottoscrizione & "';"
                sScript += "document.getElementById('hdConsumoMinimo').value='" & oMyContratto.oContatore.nConsumoMinimo & "';"
                sScript += "document.getElementById('hdTipoUtenzaContratto').value='" & oMyContratto.oContatore.nIdTipoUtenza & "';"
                sScript += "document.getElementById('hdIdDiametroContatoreContratto').value='" & oMyContratto.oContatore.nDiametroContatore & "';"
                sScript += "document.getElementById('hdIdDiametroPresaContratto').value='" & oMyContratto.oContatore.nDiametroPresa & "';"
                sScript += "document.getElementById('hdNumeroUtenzeContratto').value='" & oMyContratto.oContatore.nNumeroUtenze & "';"
                sScript += "document.getElementById('hdCodiceContratto').value='" & oMyContratto.sCodiceContratto & "';"
                sScript += "document.getElementById('hdCodiceVia').value='" & oMyContratto.oContatore.nIdVia & "';"
                sScript += "document.getElementById('hdVirtualIDContratto').value='0';"
                RegisterScript(sScript, Me.GetType())
                txtNoteSub.Text = oMyContratto.sNoteRichiestaSub
                If Not IsNothing(oMyContratto.oContatore.oListSubContatori) Then
                    chkRichiestaSub.Checked = True
                End If
                If StringOperation.FormatDateTime(oMyContratto.sDataSottoscrizione).ToShortDateString <> DateTime.MaxValue.ToShortDateString Then
                    txtDataSottoscr.Text = StringOperation.FormatDateTime(oMyContratto.sDataSottoscrizione)
                End If
                If StringOperation.FormatDateTime(oMyContratto.oContatore.sDataAttivazione).ToShortDateString <> DateTime.MaxValue.ToShortDateString And StringOperation.FormatDateTime(Request.Params("datavoltura")).ToShortDateString = DateTime.MaxValue.ToShortDateString Then
                    txtDataAttivazione.Text = StringOperation.FormatDateTime(oMyContratto.oContatore.sDataAttivazione)
                Else
                    txtDataAttivazione.Text = StringOperation.FormatDateTime(Request.Params("datavoltura"))
                End If
                If StringOperation.FormatDateTime(oMyContratto.sDataCessazione).ToShortDateString <> DateTime.MaxValue.ToShortDateString Then
                    txtDataCessazione.Text = StringOperation.FormatDateTime(oMyContratto.sDataCessazione)
                End If
                If StringOperation.FormatDateTime(oMyContratto.oContatore.sDataSospensioneUtenza).ToShortDateString <> DateTime.MaxValue.ToShortDateString Then
                    txtDataSospsensioneUtenza.Text = oMyContratto.oContatore.sDataSospensioneUtenza
                End If
                txtIdentificativoContratto.Text = oMyContratto.sCodiceContratto
                txtSequenza.Text = oMyContratto.oContatore.sSequenza
                txtProgressivo.Text = oMyContratto.oContatore.sProgressivo
                txtLatoStrada.Text = oMyContratto.oContatore.sLatoStrada
                txtNumeroUtenze.Text = oMyContratto.oContatore.nNumeroUtenze
                txtNote.Text = oMyContratto.sNote
                txtNumeroCifreContatore.Text = oMyContratto.oContatore.sCifreContatore
                '=============================
                'INIZIO MODIFICHE ALE CAO
                '=============================
                txtspesaprev.Text = FormatNumber(oMyContratto.oContatore.nSpesa)
                txtdirittisegr.Text = FormatNumber(oMyContratto.oContatore.nDiritti)
                '=============================
                'FINE MODIFICHE ALE CAO
                '=============================
                HDTextCodUtente.Text = oMyContratto.nIdUtente
                HDtxtCodIntestatario.Text = oMyContratto.nIdIntestatario

                '***carico le combo***
                dvTipoContatore = FncContatori.getListTipoContatore()
                FncGen.FillDropDownSQL(cboTipoContatore, dvTipoContatore, oMyContratto.oContatore.nTipoContatore)

                dvCodiceImpianto = FncContatori.getListCodiceImpianto()
                FncGen.FillDropDownSQL(cboImpianto, dvCodiceImpianto, oMyContratto.oContatore.nIdImpianto)

                dvGiro = FncContatori.getListGiro(ConstSession.IdEnte)
                FncGen.FillDropDownSQL(cboGiro, dvGiro, oMyContratto.oContatore.nGiro)

                dvPosizioneContatore = FncContatori.getListPosizioneContatore()
                FncGen.FillDropDownSQL(cboPosizione, dvPosizioneContatore, oMyContratto.oContatore.nPosizione)

                dvFognatura = FncContatori.getListCodFognatura()
                FncGen.FillDropDownSQL(cboFognatura, dvFognatura, oMyContratto.oContatore.nCodFognatura)

                dvDepurazione = FncContatori.getListCodDepurazione()
                FncGen.FillDropDownSQL(cboDepurazione, dvDepurazione, oMyContratto.oContatore.nCodDepurazione)

                dvDiametroContatore = FncContatori.getListDiametroContatore(ConstSession.CodIstat)
                FncGen.FillDropDownSQL(cboDiametroContatore, dvDiametroContatore, oMyContratto.oContatore.nDiametroContatore)

                dvDiametroPresa = FncContatori.getListDiametroPresa()
                FncGen.FillDropDownSQL(cboDiametroPresa, dvDiametroPresa, oMyContratto.oContatore.nDiametroPresa)

                dvTipoUtenza = FncContatori.getListTipoUtenza(ConstSession.IdEnte, oMyContratto.sDataSottoscrizione, oMyContratto.oContatore.nTipoUtenza)
                FncGen.FillDropDownSQL(cboTipoUtenze, dvTipoUtenza, oMyContratto.oContatore.nTipoUtenza)

                'drIVA = FncContatori.getListIVA()
                'FncGen.FillDropDownSQL(cboAssogettamentoIva, drIVA, oMyContratto.oContatore.nCodIva)
                'drTipoAttivita = FncContatori.getListTipoAttivita()
                'FncGen.FillDropDownSQL(cboAttivita, drTipoAttivita, oMyContratto.oContatore.nIdAttivita)

                FncGen.SelectIndexDropDownList(cboStatoContatore, oMyContratto.oContatore.sStatoContatore)
                'FncGen.SelectIndexDropDownList(cboPenalita, oMyContratto.oContatore.sPenalita)
                '*********************

                '***** GESTIONE DATI AGENZIA ENTRATE ******
                LoadDatiAE()
                If oMyContratto.oContatore.nIdAssenzaDatiCatastali <> -1 Then
                    ddlAssenzaDatiCat.SelectedValue = oMyContratto.oContatore.nIdAssenzaDatiCatastali
                End If

                If oMyContratto.oContatore.nIdTipoUtenza = -1 Then
                    ddlTipoUtenza.SelectedValue = ""
                Else
                    ddlTipoUtenza.SelectedValue = oMyContratto.oContatore.nIdTipoUtenza
                End If

                If oMyContratto.oContatore.sTipoUnita = "" Then
                    ddlTipoUnita.SelectedValue = "F"
                Else
                    ddlTipoUnita.SelectedValue = oMyContratto.oContatore.sTipoUnita
                End If

                If oMyContratto.oContatore.nIdTitoloOccupazione = -1 Then
                    ddlTitOccupazione.SelectedValue = ""
                Else
                    ddlTitOccupazione.SelectedValue = oMyContratto.oContatore.nIdTitoloOccupazione
                End If
                '**** /GESTIONE DATI AGENZIA ENTRATE

                '***carico i dati del contatore associato***
                LoadDatiContatore(ContrattoID, codiceVia, ubicazione)
                '*******************************************

                '*** Fabi
                If txtidContatore.Text <> "" Then
                    codiceVia = codiceVia
                    ubicazione = ubicazione
                Else
                    codiceVia = TxtCodVia.Text
                    ubicazione = TxtVia.Text
                End If
                TxtVia.Text = ubicazione
                TxtCodVia.Text = codiceVia
                '*** /Fabi

                If txtidContatore.Text <> "" And StringOperation.FormatString(Request.Params("volturato")) <> "1" Then 'carico solo se non volturato
                    '***carico i dati delle anagrafiche utente+intestatario***
                    Dim oAnagrafica As New Anagrafica.DLL.GestioneAnagrafica()
                    Dim oDettaglioAnagraficaInt As New DettaglioAnagrafica
                    Dim oDettaglioAnagraficaUtente As New DettaglioAnagrafica
                    '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
                    'oDettaglioAnagraficaInt = oAnagrafica.GetAnagrafica(oMyContratto.nIdIntestatario, ConstSession.CodTributo, -1, ConstSession.StringConnectionAnagrafica)
                    'oDettaglioAnagraficaUtente = oAnagrafica.GetAnagrafica(oMyContratto.nIdUtente, ConstSession.CodTributo, -1, ConstSession.StringConnectionAnagrafica)
                    oDettaglioAnagraficaInt = oAnagrafica.GetAnagrafica(oMyContratto.nIdIntestatario, -1, String.Empty, ConstSession.DBType, ConstSession.StringConnectionAnagrafica, False)
                    oDettaglioAnagraficaUtente = oAnagrafica.GetAnagrafica(oMyContratto.nIdUtente, -1, String.Empty, ConstSession.DBType, ConstSession.StringConnectionAnagrafica, False)
                    'ale
                    Session("oDettaglioAnagraficaInt") = oDettaglioAnagraficaInt
                    Session("oDettaglioAnagraficaUtente") = oDettaglioAnagraficaUtente
                    '/ale
                    txtCognomeIntestatario.Text = oDettaglioAnagraficaInt.Cognome & " " & oDettaglioAnagraficaInt.Nome & " " & oDettaglioAnagraficaInt.PartitaIva & " " & oDettaglioAnagraficaInt.CodiceFiscale
                    miointestatario.Text = oDettaglioAnagraficaInt.COD_CONTRIBUENTE
                    mioutente.Text = oDettaglioAnagraficaUtente.COD_CONTRIBUENTE
                    txtCodiceFiscaleIntestatario.Text = oDettaglioAnagraficaInt.DataNascita

                    txtViaIntestatario.Text = oDettaglioAnagraficaInt.ViaResidenza & " " & oDettaglioAnagraficaInt.CivicoResidenza
                    If Len(Trim(oDettaglioAnagraficaInt.EsponenteCivicoResidenza)) <> 0 Then
                        txtViaIntestatario.Text += "/" & Replace(oDettaglioAnagraficaInt.EsponenteCivicoResidenza, "/", "")
                    End If
                    txtViaIntestatario.Text += " - " & oDettaglioAnagraficaInt.CapResidenza & " " & oDettaglioAnagraficaInt.ComuneResidenza & " (" & oDettaglioAnagraficaInt.ProvinciaResidenza & ")"

                    txtCognomeUtente.Text = oDettaglioAnagraficaUtente.Cognome & " " & oDettaglioAnagraficaUtente.Nome & " " & oDettaglioAnagraficaUtente.PartitaIva & " " & oDettaglioAnagraficaUtente.CodiceFiscale
                    txtCodiceFiscaleUtente.Text = oDettaglioAnagraficaUtente.DataNascita

                    txtViaUtente.Text = oDettaglioAnagraficaUtente.ViaResidenza & " " & oDettaglioAnagraficaUtente.CivicoResidenza
                    If Len(Trim(oDettaglioAnagraficaUtente.EsponenteCivicoResidenza)) <> 0 Then
                        txtViaUtente.Text += "/" & Replace(oDettaglioAnagraficaUtente.EsponenteCivicoResidenza, "/", "")
                    End If
                    txtViaUtente.Text += " - " & oDettaglioAnagraficaUtente.CapResidenza & " " & oDettaglioAnagraficaUtente.ComuneResidenza & " (" & oDettaglioAnagraficaUtente.ProvinciaResidenza & ")"
                Else
                    HDtxtCodIntestatario.Text = "-1"
                    miointestatario.Text = "-1"
                    HDTextCodUtente.Text = "-1"
                    mioutente.Text = "-1"
                    txtidContatore.Text = "0"
                    hfIDContatore.Value = "-1"
                End If
            Else
                sScript += "doFocus=true;"
                RegisterScript(sScript, Me.GetType())

                codiceVia = TxtCodVia.Text
                ubicazione = TxtVia.Text
            End If

            If lblattcont.Text.IndexOf("Il contratto e") = -1 Then
                If Page.IsPostBack = False And StringOperation.FormatDateTime(txtDataAttivazione.Text) <> DateTime.MaxValue Then
                    lblattcont.Text = "Contatore gia' attivo per questo contratto"
                    btnAttivaContatore.Visible = False
                End If
                If StringOperation.FormatDateTime(txtDataAttivazione.Text) <> DateTime.MaxValue And lblattcont.Text <> "Contatore gia' attivo per questo contratto" Then
                    btnAttivaContatore.Visible = False
                    lblattcont.Text = "Al salvataggio, verrà attivato il<br>contatore per questo contratto"
                End If
                If StringOperation.FormatDateTime(txtDataCessazione.Text) <> DateTime.MaxValue Then
                    lblattcont.Text = "Il contratto e' stato cessato"
                End If
            End If

            'si verificano 2 controlli:
            'anagrafiche non presenti, e parametro via URL denominato "volturato" uguale a 1
            'nel caso entrambi siano veri, appare la conferma del contratto appena volturato.
            If miointestatario.Text.ToString() = "-1" Or mioutente.Text.ToString() = "-1" Then
                If Not Request.Params("volturato") Is Nothing Then
                    If Request.Params("volturato").ToString() = "1" Then
                        txtidContatore.Text = "0"
                        If Page.IsPostBack = False Then
                            sScript += "GestAlert('a', 'info', '', '', 'Il contratto e\' stato volturato.\nPer associare il vecchio contratto ad un nuovo utente,\nriempire i campi relativi alle anagrafiche ed ai dati generali, quindi effettuare il salvataggio.\nSe non verra\' inserita nessuna anagrafica, il contratto rimarra\' con le nuove anagrafiche pendenti.');"
                            RegisterScript(sScript, Me.GetType)
                        End If
                    End If
                End If
            End If

            If (txtDataSottoscr.Text) <> "" And (Not Page.IsPostBack) Then
                txtDataSottoscr.Enabled = False
            End If
            If (txtIdentificativoContratto.Text <> "") And (Not Page.IsPostBack) Then
                txtIdentificativoContratto.Enabled = False
            End If

            If (Not Page.IsPostBack) And (txtDataCessazione.Text) <> "" Then
                sScript += "GestAlert('a', 'warning', '', '', 'Il contratto e\' gia stato cessato e quindi non e\' modificabile');"
                RegisterScript(sScript, Me.GetType)
                ViewState("nonmodificabile") = True
            Else
                If ViewState("nonmodificabile") <> True Then
                    ViewState("nonmodificabile") = False
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DatiGeneraliContr.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    Private Sub LoadDatiContatore(ByVal IDContratto As Integer, ByRef codiceVia As Integer, ByRef ubicazione As String)
        Dim sSQL As String
        Dim dvMyDati As New DataView
        Try
            sSQL = "SELECT *"
            sSQL += " FROM TP_CONTATORI WITH (NOLOCK)"
            sSQL += " WHERE YEAR(CASE WHEN ISDATE(DATA_VARIAZIONE)=0 THEN '99991231' ELSE DATA_VARIAZIONE END)=9999"
            sSQL += " AND (TP_CONTATORI.CODCONTRATTO = " & IDContratto & ")"
            dvMyDati = DBAccess.GetDataView(sSQL)
            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    txtNumeroUtente.Text = StringOperation.FormatString(myRow("NUMEROUTENTE"))
                    txtCivico.Text = StringOperation.FormatString(myRow("CIVICO_UBICAZIONE"))
                    txtEsponente.Text = StringOperation.FormatString(myRow("ESPONENTE_CIVICO"))
                    chkEsenteFognatura.Checked = StringOperation.FormatBool(myRow("ESENTEFOGNATURA"))
                    chkEsenteDepurazione.Checked = StringOperation.FormatBool(myRow("ESENTEDEPURAZIONE"))
                    chkEsenteAcqua.Checked = StringOperation.FormatBool(myRow("ESENTEACQUA"))
                    '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
                    chkEsenteAcquaQF.Checked = StringOperation.FormatBool(myRow("ESENTEACQUAQF"))
                    chkEsenteDepQF.Checked = StringOperation.FormatBool(myRow("ESENTEDEPURAZIONEQF"))
                    chkEsenteFogQF.Checked = StringOperation.FormatBool(myRow("ESENTEFOGNATURAQF"))
                    If chkEsenteDepurazione.Checked = True Then
                        cboDepurazione.Enabled = False
                    Else
                        cboDepurazione.Enabled = True
                    End If
                    If chkEsenteFognatura.Checked = True Then
                        cboFognatura.Enabled = False
                    Else
                        cboFognatura.Enabled = True
                    End If
                    '*** Fabi
                    codiceVia = StringOperation.FormatInt(myRow("COD_STRADA"))
                    ubicazione = StringOperation.FormatString(myRow("VIA_UBICAZIONE"))
                    '*** /Fabi
                Next
            Else
                codiceVia = -1
                ubicazione = ""
            End If
            dvMyDati.Dispose()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DatiGeneraliContr.LoadDatiContatore.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Public Function TrovaStradario(ByVal codiceVia As Object) As String
        Dim nomeStrada As String = ""
        Try
            If Not IsDBNull(codiceVia) Then
                Dim ArrStrade() As OggettiComuniStrade.OggettoStrada
                ArrStrade = Nothing
                If Not ArrStrade Is Nothing Then
                    If ArrStrade.Length.CompareTo(1) = 0 Then
                        nomeStrada = ArrStrade(0).TipoStrada.ToString() + " " + ArrStrade(0).DenominazioneStrada.ToString()
                    Else
                        nomeStrada = ""
                    End If
                Else
                    nomeStrada = ""
                End If
            Else
                nomeStrada = ""
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DatiGeneraliContr.TrovaStradario.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
        Return nomeStrada
    End Function

    Private Sub btnStampa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStampa.Click

        Try
            'objToPrint = Nothing
            Dim sScript As String = "GestAlert('a', 'warning', '', '', 'Funzionalita\' al momento non disponibile!')"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DatiGeneraliContr.btnStampa_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub btnStampa2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStampa2.Click
        Try
            Dim sScript As String = "GestAlert('a', 'warning', '', '', 'Funzionalita\' al momento non disponibile!')"
            RegisterScript(sScript, Me.GetType())
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DatiGeneraliContr.btnStampa2_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub btnEvento2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEvento2.Click
        Dim sScript As String = ""
        Dim nIdContatore As Integer = -1

        Try
            Dim impNumericoSpesa, impNumericoDiritti As Boolean
            impNumericoSpesa = IsNumeric(txtspesaprev.Text)
            impNumericoDiritti = IsNumeric(txtdirittisegr.Text)

            If impNumericoSpesa = False Or impNumericoDiritti = False Then
                sScript += "GestAlert('a', 'warning', '', '', 'Gli importi relativi a spese e diritti di segreteria devono essere numerici.');"
                RegisterScript(sScript, Me.GetType)
                Exit Sub
            End If
            '=====================================================================
            'Se il contratto è già stato cessato, non posso salvare modifiche
            '=====================================================================
            If ViewState("nonmodificabile") = True Then
                sScript += "location.href='DatiGeneraliContr.aspx?IDCONTRATTO=" & Request.Params("IDCONTRATTO") & "';"
                RegisterScript(sScript, Me.GetType)
                Exit Sub
            End If

            '================================
            'Questo controllo permette di non inserire 2 numeri utente uguali
            '================================
            If txtNumeroUtente.Text <> "" Then
                If FncContratti.GetEsistente(ConstSession.IdEnte, txtNumeroUtente.Text, CInt(Request.Params("IDCONTRATTO"))) Then
                    sScript += "GestAlert('a', 'warning', '', '', 'Numero utente gia\' esistente.\nIl contratto non puo\' essere salvato!');"
                    RegisterScript(sScript, Me.GetType)
                    Exit Sub
                End If
            End If

            Try
                If txtidContatore.Text <> "" Then
                    ContrattoID = CInt(txtidContatore.Text)
                Else
                    ContrattoID = 0
                End If

                If CInt(ContrattoID) <= 0 And StringOperation.FormatString(Request.Params("volturato")) <> "1" Then
                    If FncContratti.ControllaCodice(ConstSession.IdEnte, txtIdentificativoContratto.Text, StringOperation.FormatInt(Request.Params("volturato"))) <> "-1" Then
                        'Non si puo inserire un codice contratto uguale ad uno precedentemente inserito
                        'se passa di qua, non verrà inserito un nuovo contratto in quanto il codice contratto
                        'di tipo stringa è già esistente
                        sScript += "GestAlert('a', 'warning', '', '', 'Codice Contratto gia\' esistente.\nIl contratto non puo\' essere salvato!');"
                        RegisterScript(sScript, Me.GetType)
                        Exit Sub
                    End If
                End If

                Dim DBContatori As GestContatori = New GestContatori
                '==========================================================================
                'Nuovo metodo verifica sel il codice utente e da considerare o meno
                'deve permettere l'inserimento di un codice utente anche se già esistente 
                'deve essere considerato come un nuovo contatore
                Dim oDatiContratto As New objContratto
                If ValorizzaDatiContratto(oDatiContratto) < 1 Then
                    lblError.Text = "Errore in inserimento dati!"
                    Exit Sub
                End If
                '==================================
                'mi calcolo l'id massimo presente nei contatori, mi servirà nel caso vengano inseriti per 
                'la prima volta i dati catastali
                Dim maxCont As Integer
                maxCont = New MyUtility().GetMaxID("TP_CONTATORI")

                'SALVATAGGIO
                If FncContratti.SaveContratto(ContrattoID, ConstSession.IdPeriodo, oDatiContratto, StringOperation.FormatInt(Request.Params("volturato")), False, nIdContatore) = False Then ', hfIDContatore.Value
                    sScript += "GestAlert('a', 'warning', '', '', 'Si è verificato un\'errore.\nIl contratto non può essere salvato!');"
                    RegisterScript(sScript, Me.GetType)
                    Exit Sub
                End If
                sScript += "GestAlert('a', 'success', '', '', 'Salvataggio avvenuto correttamente');"
                RegisterScript(sScript, Me.GetType)
                Session("datacatasto") = Nothing
                '==================================
                '====================================
                'CONTATORE PER ANATER
                '====================================
                Dim contPerAnater As Int32

                If txtidContatore.Text = "" Then
                    'NUOVO CONTATORE INSERITO
                    contPerAnater = maxCont
                Else
                    'CONTATORE ESISTENTEAGGIORNATO
                    Dim dvMyDati As New DataView
                    dvMyDati = DBAccess.GetDataView("SELECT CODCONTATORE FROM TP_CONTATORI WHERE CODCONTRATTO=" + txtidContatore.Text)
                    If Not dvMyDati Is Nothing Then
                        For Each myRow As DataRowView In dvMyDati
                            contPerAnater = StringOperation.FormatInt(myRow("CODCONTATORE"))
                        Next
                    End If
                End If

                Dim dsDatiContatore As DataSet
                dsDatiContatore = DBContatori.GetDataTableContatoreAnater(contPerAnater)

                'E RICHIAMO IL METODO NEL SERVIZIO
                If oDatiContratto.oContatore.nIdContatore > 0 Then
                    oDatiContratto.oContatore.nIdContatore = ContrattoID
                Else
                    txtidContatore.Text = oDatiContratto.oContatore.nIdContatore
                    ContrattoID = CInt(oDatiContratto.oContatore.nIdContatore)
                End If

                '=========================================================
                'PROVA X TENERE TRACCIA DEL CONTRATTO, SE E' STATO CESSATO
                '=========================================================
                Dim contrcessato As Int32 = 0
                If txtDataCessazione.Text <> "" Then
                    contrcessato = 1
                End If
                '=========================================================

                If txtDataCessazione.Text <> "" Then
                    sScript += "location.href='DatiGeneraliContr.aspx?IDCONTRATTO=" & oDatiContratto.nIdContratto & "&idcontatoreprec=" & nIdContatore & "&volturato=" & contrcessato & "&datavoltura=" & StringOperation.FormatDateTime(txtDataCessazione.Text).AddDays(1).ToShortDateString & "&hdMatricola=" & oDatiContratto.oContatore.sMatricola & "';"
                Else
                    sScript += "location.href='RicercaContratti.aspx';"
                End If
                RegisterScript(sScript, Me.GetType())

                '*** ribaltamento in Anater se abilitato
                If System.Configuration.ConfigurationManager.AppSettings("USO_ANATER").ToString() = True Then
                    Dim typeofRI As Type = GetType(IRemotingInterfaceH2O)
                    Dim remObject As IRemotingInterfaceH2O = Activator.GetObject(typeofRI, System.Configuration.ConfigurationManager.AppSettings("URLanaterH2O").ToString())
                    Dim clsTraduci As New ClsTraduciRibaltamento
                    Dim objContatoreAnater As New Anater.Oggetti.DatiContatore
                    Dim oAnagraficaAnater As New Anater.Oggetti.AnagraficaH2O
                    Dim codiceFiscale As String = ""
                    Dim percorso As String = ""
                    Dim nomeFile As String = ""
                    Dim dataFile As Date = Date.Now
                    Dim sdata As String = dataFile.ToString().Replace("/", "_")
                    sdata = sdata.Replace(" ", "_")
                    sdata = sdata.Replace(".", "_")

                    percorso = System.Configuration.ConfigurationManager.AppSettings("PATH_LETTURE_NON_ACQUI").ToString()
                    nomeFile = percorso + "Scarti_Ribaltamento_" + sdata + ".txt"

                    Dim oDettaglioAnagraficaUtente As New DettaglioAnagrafica
                    oDettaglioAnagraficaUtente = clsTraduci.TrovaAnagrafica(CInt(dsDatiContatore.Tables(0).Rows(0)("CODCONTATORE").ToString()))

                    oAnagraficaAnater = clsTraduci.TraduciAnagraficaAnater(oDettaglioAnagraficaUtente)
                    If oAnagraficaAnater.CodiceFiscale = "" Then
                        codiceFiscale = oAnagraficaAnater.PartitaIva
                    Else
                        codiceFiscale = oAnagraficaAnater.CodiceFiscale
                    End If
                    'RICAVO IL DT DEI DATI CATASTALI
                    Dim DSCatasto As DataSet
                    Dim FncRif As New GestContatori
                    DSCatasto = FncRif.getListaCatastali(-1, CInt(dsDatiContatore.Tables(0).Rows(0)("CODCONTATORE").ToString()))
                    objContatoreAnater = clsTraduci.TraduciContatoreAnater(dsDatiContatore, DSCatasto, codiceFiscale)
                    remObject.RibaltaInAnaterH2O(objContatoreAnater, oAnagraficaAnater, ConstSession.IdEnte, False, False, 0, nomeFile)
                End If

            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DatiGeneraliContr.btnEvento2_Click.errore: ", ex)
                Response.Redirect("../../PaginaErrore.aspx")
            End Try
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DatiGeneraliContr.btnEvento2_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    'Private Sub btnApriIntestatario_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnApriIntestatario.Click
    '    'devo creare la sessione al workflow
    '    Dim oSM As New RIBESFrameWork.SessionManager(Session("PARAMETROENV"))
    '    Dim oSession As RIBESFrameWork.Session
    '   Try
    '    If oSM.Initialize(ConstSession.UserName, Session("PARAMETROENV")) Then
    '        oSession = oSM.CreateSession(ConfigurationManager.AppSettings("IDENTIFICATIVOAPPLICAZIONE").ToString())
    '        If oSession Is Nothing Then
    '            'Errore creazione Session
    '        Else
    '            If oSession.oErr.Number <> 0 Then
    '                'Errore
    '            End If
    '        End If
    '    End If
    '    Dim COD_TRIBUTO As String = Session("COD_TRIBUTO")
    '    Dim oAnagrafica As New Anagrafica.DLL.GestioneAnagrafica()(oSession, ConfigurationManager.AppSettings("PARAMETRO_ANAGRAFICA"))
    '    Dim oDettaglioAnagraficaInt As New DettaglioAnagrafica
    '    Dim oDettaglioAnagraficaUtente As New DettaglioAnagrafica
    '    oDettaglioAnagraficaInt = oAnagrafica.GetAnagrafica(CLng(HDtxtCodIntestatario.Text), COD_TRIBUTO, -1)

    '    Session("oDettaglioAnagraficaInt") = oDettaglioAnagraficaInt

    '    Session("TipoAnagrafica") = "Intestatario"

    '    Dim strScript As String
    '    strScript = "<script language = ""javascript"">" & vbCrLf
    '    strScript += "ApriAnagrafica(" & CLng(HDtxtCodIntestatario.Text) & ", 'oDettaglioAnagraficaInt')" & vbCrLf
    '    strScript += ""
    '    RegisterScript(sScript , Me.GetType())"", strScript)
    ' Catch Err As Exception

    '   Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DatiGeneraliContr.btnApriIntestatario_Click.errore: ", Err)
    ' Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub

    Private Sub btnApriIntestatario_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnApriIntestatario.Click
        Dim oAnagrafica As New Anagrafica.DLL.GestioneAnagrafica()
        Dim oDettaglioAnagraficaInt As New DettaglioAnagrafica
        '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
        oDettaglioAnagraficaInt = oAnagrafica.GetAnagrafica(CLng(HDtxtCodIntestatario.Text), -1, String.Empty, ConstSession.DBType, ConstSession.StringConnectionAnagrafica, False)
        Session("oDettaglioAnagraficaInt") = oDettaglioAnagraficaInt

        Session("TipoAnagrafica") = "Intestatario"

        Dim sScript As String
        sScript = "ApriAnagrafica(" & CLng(HDtxtCodIntestatario.Text) & ", 'oDettaglioAnagraficaInt')"
        RegisterScript(sScript, Me.GetType())
    End Sub

    Private Sub LoadDropDownList(ByVal cboTemp As DropDownList, ByVal dsTemp As DataSet, ByVal DataValueField As String, ByVal DataTextField As String, ByVal lngSelectedID As Long)
        Try
            Dim dt As DataTable = dsTemp.Tables(0)
            Dim rowNull As DataRow = dt.NewRow()
            rowNull(DataTextField) = "..."
            rowNull(DataValueField) = "-1"
            dsTemp.Tables(0).Rows.InsertAt(rowNull, 0)

            cboTemp.DataSource = dsTemp
            cboTemp.DataValueField = DataValueField
            cboTemp.DataTextField = DataTextField
            cboTemp.DataBind()

            If lngSelectedID <> -1 Then
                FncGen.SelectIndexDropDownList(cboTemp, CStr(lngSelectedID))
            End If
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DatiGeneraliContr.LoadDropDownList.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    'Private Sub btnRibalta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRibalta.Click

    '    Dim buffScriptString As String

    '    Dim WFErrore As String
    '    Dim WFSessione As New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '    If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '        Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '    End If

    '    Dim oAnagrafica As New Anagrafica.DLL.GestioneAnagrafica()(WFSessione.oSession, Session("ANAGRAFICA"))
    '    Dim oDettaglioAnagrafica As New DettaglioAnagrafica
    '    Try
    '        Log.Debug("DatiGeneraliContr::btnRibalta_Click::Session(TipoAnagrafica)::" & Session("TipoAnagrafica"))
    '        Select Case Session("TipoAnagrafica")
    '            Case "Intestatario"
    '                oDettaglioAnagrafica = Session("oDettaglioAnagraficaInt")
    '                HDtxtCodIntestatario.Text = oDettaglioAnagrafica.COD_CONTRIBUENTE

    '                miointestatario.Text = oDettaglioAnagrafica.COD_CONTRIBUENTE
    '                mioutente.Text = HDTextCodUtente.Text

    '                'txtNomeIntestatario.text= Replace(oDettaglioAnagrafica.Nome, "'", "\'")
    '                'txtCivicoIntestatario.text= oDettaglioAnagrafica.CivicoResidenza
    '                'txtComuneIntestatario.text= Replace(oDettaglioAnagrafica.ComuneResidenza, "'", "\'") & " (" & Replace(oDettaglioAnagrafica.ProvinciaResidenza, "'", "\'") & ")"
    '                'txtProvinciaIntestatario.text= oDettaglioAnagrafica.ProvinciaResidenza
    '                txtCognomeIntestatario.Text = Replace(oDettaglioAnagrafica.Cognome & " " & oDettaglioAnagrafica.Nome, "'", "\'")
    '                txtCodiceFiscaleIntestatario.Text = oDettaglioAnagrafica.DataNascita
    '                txtViaIntestatario.Text = Replace(oDettaglioAnagrafica.ViaResidenza & " " & oDettaglioAnagrafica.CivicoResidenza & " " & oDettaglioAnagrafica.CapResidenza & " " & oDettaglioAnagrafica.ComuneResidenza & " " & oDettaglioAnagrafica.ProvinciaResidenza, "'", "\'")
    '                'hdcodanagrafeintestatario.Value = oDettaglioAnagrafica.COD_CONTRIBUENTE

    '                If mioutente.Text = "-1" Or mioutente.Text = "" Or mioutente.Text = miointestatario.Text Then
    '                    HDTextCodUtente.Text = oDettaglioAnagrafica.COD_CONTRIBUENTE
    '                    'txtNomeUtente.text= Replace(oDettaglioAnagrafica.Nome, "'", "\'")
    '                    'txtCivicoUtente.text= oDettaglioAnagrafica.CivicoResidenza
    '                    'txtComuneUtente.text= Replace(oDettaglioAnagrafica.ComuneResidenza, "'", "\'") & " (" & Replace(oDettaglioAnagrafica.ProvinciaResidenza, "'", "\'") & ")"
    '                    'txtProvinciaUtente.text= oDettaglioAnagrafica.ProvinciaResidenza
    '                    txtCognomeUtente.Text = Replace(oDettaglioAnagrafica.Cognome & " " & oDettaglioAnagrafica.Nome, "'", "\'")
    '                    txtCodiceFiscaleUtente.Text = oDettaglioAnagrafica.DataNascita
    '                    txtViaUtente.Text = Replace(oDettaglioAnagrafica.ViaResidenza & " " & oDettaglioAnagrafica.CivicoResidenza & " " & oDettaglioAnagrafica.CapResidenza & " " & oDettaglioAnagrafica.ComuneResidenza & " " & oDettaglioAnagrafica.ProvinciaResidenza, "'", "\'")
    '                    hdCodAnagrafeUtente.Value = oDettaglioAnagrafica.COD_CONTRIBUENTE
    '                    mioutente.Text = HDTextCodUtente.Text
    '                End If
    '            Case "Utente"
    '                oDettaglioAnagrafica = Session("oDettaglioAnagraficaUtente")

    '                HDTextCodUtente.Text = oDettaglioAnagrafica.COD_CONTRIBUENTE

    '                'txtCodFisUte.Text = oDettaglioAnagrafica.CodiceFiscale

    '                mioutente.Text = oDettaglioAnagrafica.COD_CONTRIBUENTE

    '                'txtNomeUtente.text= Replace(oDettaglioAnagrafica.Nome, "'", "\'")
    '                'txtCivicoUtente.text= oDettaglioAnagrafica.CivicoResidenza
    '                'txtComuneUtente.text= Replace(oDettaglioAnagrafica.ComuneResidenza, "'", "\'") & " (" & Replace(oDettaglioAnagrafica.ProvinciaResidenza, "'", "\'") & ")"
    '                'txtProvinciaUtente.text= oDettaglioAnagrafica.ProvinciaResidenza
    '                txtCognomeUtente.Text = Replace(oDettaglioAnagrafica.Cognome & " " & oDettaglioAnagrafica.Nome, "'", "\'")
    '                txtCodiceFiscaleUtente.Text = oDettaglioAnagrafica.DataNascita
    '                txtViaUtente.Text = Replace(oDettaglioAnagrafica.ViaResidenza & " " & oDettaglioAnagrafica.CivicoResidenza & " " & oDettaglioAnagrafica.CapResidenza & " " & oDettaglioAnagrafica.ComuneResidenza & " " & oDettaglioAnagrafica.ProvinciaResidenza, "'", "\'")
    '                hdCodAnagrafeUtente.Value = oDettaglioAnagrafica.COD_CONTRIBUENTE
    '                If HDTextCodUtente.Text = HDtxtCodIntestatario.Text Then
    '                    'txtNomeIntestatario.text= Replace(oDettaglioAnagrafica.Nome, "'", "\'")
    '                    'txtCivicoIntestatario.text= oDettaglioAnagrafica.CivicoResidenza
    '                    'txtComuneIntestatario.text= Replace(oDettaglioAnagrafica.ComuneResidenza, "'", "\'") & " (" & oDettaglioAnagrafica.ProvinciaResidenza & ")"
    '                    'txtProvinciaIntestatario.text= oDettaglioAnagrafica.ProvinciaResidenza
    '                    txtCognomeIntestatario.Text = Replace(oDettaglioAnagrafica.Cognome & " " & oDettaglioAnagrafica.Nome, "'", "\'")
    '                    txtCodiceFiscaleIntestatario.Text = oDettaglioAnagrafica.DataNascita
    '                    txtViaIntestatario.Text = Replace(oDettaglioAnagrafica.ViaResidenza & " " & oDettaglioAnagrafica.CivicoResidenza & " " & oDettaglioAnagrafica.CapResidenza & " " & oDettaglioAnagrafica.ComuneResidenza & " " & oDettaglioAnagrafica.ProvinciaResidenza, "'", "\'")
    '                    'hdcodanagrafeintestatario.Value = oDettaglioAnagrafica.COD_CONTRIBUENTE
    '                End If
    '        End Select
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DatiGeneraliContr.btnRibalta_Click.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    Private Sub btnRibalta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRibalta.Click
        Try
            Dim oDettaglioAnagrafica As New DettaglioAnagrafica
            Log.Debug("DatiGeneraliContr::btnRibalta_Click::Session(TipoAnagrafica)::" & Session("TipoAnagrafica"))
            Select Case Utility.StringOperation.FormatString(Session("TipoAnagrafica"))
                Case "Intestatario"
                    oDettaglioAnagrafica = Session("oDettaglioAnagraficaInt")
                    HDtxtCodIntestatario.Text = oDettaglioAnagrafica.COD_CONTRIBUENTE

                    miointestatario.Text = oDettaglioAnagrafica.COD_CONTRIBUENTE
                    mioutente.Text = HDTextCodUtente.Text

                    txtCognomeIntestatario.Text = Replace(oDettaglioAnagrafica.Cognome & " " & oDettaglioAnagrafica.Nome, "'", "\'")
                    txtCodiceFiscaleIntestatario.Text = oDettaglioAnagrafica.DataNascita
                    txtViaIntestatario.Text = Replace(oDettaglioAnagrafica.ViaResidenza & " " & oDettaglioAnagrafica.CivicoResidenza & " " & oDettaglioAnagrafica.CapResidenza & " " & oDettaglioAnagrafica.ComuneResidenza & " " & oDettaglioAnagrafica.ProvinciaResidenza, "'", "\'")

                    If mioutente.Text = "-1" Or mioutente.Text = "" Or mioutente.Text = miointestatario.Text Then
                        HDTextCodUtente.Text = oDettaglioAnagrafica.COD_CONTRIBUENTE
                        txtCognomeUtente.Text = Replace(oDettaglioAnagrafica.Cognome & " " & oDettaglioAnagrafica.Nome, "'", "\'")
                        txtCodiceFiscaleUtente.Text = oDettaglioAnagrafica.DataNascita
                        txtViaUtente.Text = Replace(oDettaglioAnagrafica.ViaResidenza & " " & oDettaglioAnagrafica.CivicoResidenza & " " & oDettaglioAnagrafica.CapResidenza & " " & oDettaglioAnagrafica.ComuneResidenza & " " & oDettaglioAnagrafica.ProvinciaResidenza, "'", "\'")
                        hdCodAnagrafeUtente.Value = oDettaglioAnagrafica.COD_CONTRIBUENTE
                        mioutente.Text = HDTextCodUtente.Text
                    End If
                Case "Utente"
                    oDettaglioAnagrafica = Session("oDettaglioAnagraficaUtente")

                    HDTextCodUtente.Text = oDettaglioAnagrafica.COD_CONTRIBUENTE

                    mioutente.Text = oDettaglioAnagrafica.COD_CONTRIBUENTE

                    txtCognomeUtente.Text = Replace(oDettaglioAnagrafica.Cognome & " " & oDettaglioAnagrafica.Nome, "'", "\'")
                    txtCodiceFiscaleUtente.Text = oDettaglioAnagrafica.DataNascita
                    txtViaUtente.Text = Replace(oDettaglioAnagrafica.ViaResidenza & " " & oDettaglioAnagrafica.CivicoResidenza & " " & oDettaglioAnagrafica.CapResidenza & " " & oDettaglioAnagrafica.ComuneResidenza & " " & oDettaglioAnagrafica.ProvinciaResidenza, "'", "\'")
                    hdCodAnagrafeUtente.Value = oDettaglioAnagrafica.COD_CONTRIBUENTE
                    If HDTextCodUtente.Text = HDtxtCodIntestatario.Text Then
                        txtCognomeIntestatario.Text = Replace(oDettaglioAnagrafica.Cognome & " " & oDettaglioAnagrafica.Nome, "'", "\'")
                        txtCodiceFiscaleIntestatario.Text = oDettaglioAnagrafica.DataNascita
                        txtViaIntestatario.Text = Replace(oDettaglioAnagrafica.ViaResidenza & " " & oDettaglioAnagrafica.CivicoResidenza & " " & oDettaglioAnagrafica.CapResidenza & " " & oDettaglioAnagrafica.ComuneResidenza & " " & oDettaglioAnagrafica.ProvinciaResidenza, "'", "\'")
                    End If
            End Select
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DatiGeneraliContr.btnRibalta_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    'Private Sub btnApriUtente_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnApriUtente.Click
    '    'devo creare la sessione al workflow
    '    Dim oSM As New RIBESFrameWork.SessionManager(Session("PARAMETROENV"))
    '    Dim oSession As RIBESFrameWork.Session
    '   Try
    '    If oSM.Initialize(ConstSession.UserName, Session("PARAMETROENV")) Then
    '        oSession = oSM.CreateSession(ConfigurationManager.AppSettings("IDENTIFICATIVOAPPLICAZIONE").ToString())
    '        If oSession Is Nothing Then
    '            'Errore creazione Session
    '        Else
    '            If oSession.oErr.Number <> 0 Then
    '                'Errore
    '            End If
    '        End If
    '    End If

    '    Dim COD_TRIBUTO As String = Session("COD_TRIBUTO")
    '    Dim oAnagrafica As New Anagrafica.DLL.GestioneAnagrafica()(oSession, ConfigurationManager.AppSettings("PARAMETRO_ANAGRAFICA"))
    '    Dim oDettaglioAnagraficaUtente As New DettaglioAnagrafica

    '    oDettaglioAnagraficaUtente = oAnagrafica.GetAnagrafica(CLng(HDTextCodUtente.Text), COD_TRIBUTO, -1)

    '    Session("oDettaglioAnagraficaUtente") = oDettaglioAnagraficaUtente

    '    Session("TipoAnagrafica") = "Utente"

    '    Dim strScript As String
    '    strScript = "<script language = ""javascript"">" & vbCrLf
    '    strScript += "ApriAnagrafica(" & CLng(HDTextCodUtente.Text) & ", 'oDettaglioAnagraficaUtente')" & vbCrLf
    '    strScript += ""

    '    RegisterScript(sScript , Me.GetType())"", strScript)
    '  Catch Err as Exception
    '    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DatiGeneraliContr.btnApriUtente_Click.errore: ", Err)
    '     Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub

    Private Sub btnApriUtente_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnApriUtente.Click
        Dim oAnagrafica As New Anagrafica.DLL.GestioneAnagrafica()
        Dim oDettaglioAnagraficaUtente As New DettaglioAnagrafica
        '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
        'oDettaglioAnagraficaUtente = oAnagrafica.GetAnagrafica(CLng(HDTextCodUtente.Text), ConstSession.CodTributo, -1, ConstSession.StringConnectionAnagrafica)
        oDettaglioAnagraficaUtente = oAnagrafica.GetAnagrafica(CLng(HDTextCodUtente.Text), -1, String.Empty, ConstSession.DBType, ConstSession.StringConnectionAnagrafica, False)
        Session("oDettaglioAnagraficaUtente") = oDettaglioAnagraficaUtente

        Session("TipoAnagrafica") = "Utente"

        Dim sScript As String
        sScript = "ApriAnagrafica(" & CLng(HDTextCodUtente.Text) & ", 'oDettaglioAnagraficaUtente');"
        RegisterScript(sScript, Me.GetType())
    End Sub

    Private Sub btnEliminaContratto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEliminaContratto.Click
        Dim sScript As String = ""
        Try
            Dim DECONTR As GestContratti = New GestContratti
            Dim Esistente As Integer = DECONTR.getEsistonoContatori(CInt(Request.Params("IDCONTRATTO")))

            If Esistente = True Then
                sScript += "GestAlert('a', 'warning', '', '', 'Il contratto NON può essere cancellato, ESISTE un contatore associato attivo o con alcune letture.');"
                RegisterScript(sScript, Me.GetType)
            Else
                sScript += "GestAlert('a', 'warning', '', '', 'Il contratto puo\' essere cancellato, non esiste nessun contatore associato attivo.');"
                '======================================
                'CANCELLAZIONE CONTRATTO
                '======================================
                DECONTR.EliminaContratto(CInt(Request.Params("IDCONTRATTO")))
                Response.Redirect("RicercaContratti.aspx?eliminato=1")
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DatiGeneraliContr.btnEliminaContratto_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oMyContratto"></param>
    ''' <returns></returns>
    ''' <revisionHistory><revision date="15/04/2021">inserito valorizzazione idcontatore e matricola per volture</revision></revisionHistory>
    Private Function ValorizzaDatiContratto(ByRef oMyContratto As objContratto) As Integer
        Dim oMyContatore As New objContatore
        'Dim oMyDatiCatastali As New objDatiCatastali

        Try
            oMyContratto.sCodiceContratto = txtIdentificativoContratto.Text
            oMyContratto.sDataSottoscrizione = txtDataSottoscr.Text
            oMyContatore.sIdEnte = ConstSession.IdEnte
            oMyContatore.sIdEnteAppartenenza = ConstSession.IdEnte
            oMyContatore.sCodiceISTAT = ConstSession.CodIstat
            oMyContatore.nIdContatore = hfIDContatore.Value
            oMyContatore.sMatricola = hdMatricola.Value
            If txtNumeroUtente.Text <> "" Then
                oMyContatore.sNumeroUtente = txtNumeroUtente.Text
            End If
            oMyContratto.sNoteRichiestaSub = txtNoteSub.Text
            If txtspesaprev.Text <> 0 Then
                oMyContatore.nSpesa = txtspesaprev.Text
            End If
            If txtdirittisegr.Text <> 0 Then
                oMyContatore.nDiritti = txtdirittisegr.Text
            End If
            If txtDataAttivazione.Text <> "" Then
                oMyContatore.bIsPendente = 0
            End If
            '===========================
            'FINE MODIFICHE ALE CAO
            '==========================
            If ConstSession.IdEnte <> 0 Or ConstSession.IdEnte <> -1 Then
                oMyContratto.sIdEnte = ConstSession.IdEnte
                oMyContatore.sIdEnteAppartenenza = ConstSession.IdEnte
            End If
            If cboImpianto.SelectedItem.Value <> 0 Or cboImpianto.SelectedItem.Value <> -1 Then
                oMyContatore.nIdImpianto = cboImpianto.SelectedItem.Value
            End If
            If cboGiro.SelectedItem.Value <> 0 Or cboGiro.SelectedItem.Value <> -1 Then
                oMyContatore.nGiro = cboGiro.SelectedItem.Value
            End If
            If txtSequenza.Text <> "" Then
                oMyContatore.sSequenza = txtSequenza.Text
            End If
            If cboPosizione.SelectedItem.Value <> 0 Or cboPosizione.SelectedItem.Value <> -1 Then
                oMyContatore.nPosizione = cboPosizione.SelectedItem.Value
            End If
            If txtProgressivo.Text <> "" Then
                oMyContatore.sProgressivo = txtProgressivo.Text
            End If
            If txtLatoStrada.Text <> "" Then
                oMyContatore.sLatoStrada = txtLatoStrada.Text
            End If
            If cboTipoContatore.SelectedItem.Value <> 0 Or cboTipoContatore.SelectedItem.Value <> -1 Then
                oMyContatore.nTipoContatore = cboTipoContatore.SelectedItem.Value
            End If
            If cboFognatura.SelectedItem.Value <> 0 Or cboFognatura.SelectedItem.Value <> -1 Then
                oMyContatore.nCodFognatura = cboFognatura.SelectedItem.Value
            End If
            If cboDepurazione.SelectedItem.Value <> 0 Or cboDepurazione.SelectedItem.Value <> -1 Then
                oMyContatore.nCodDepurazione = cboDepurazione.SelectedItem.Value
            End If
            If chkEsenteFognatura.Checked = True Then
                oMyContatore.bEsenteFognatura = 1
            End If
            If chkEsenteDepurazione.Checked = True Then
                oMyContatore.bEsenteDepurazione = 1
            End If
            If chkEsenteAcqua.Checked = True Then
                oMyContatore.bEsenteAcqua = 1
            End If
            '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
            If chkEsenteAcquaQF.Checked = True Then
                oMyContatore.bEsenteAcquaQF = 1
            End If
            If chkEsenteDepQF.Checked = True Then
                oMyContatore.bEsenteDepQF = 1
            End If
            If chkEsenteFogQF.Checked = True Then
                oMyContatore.bEsenteFogQF = 1
            End If
            If txtNote.Text <> "" Then
                oMyContratto.sNote = txtNote.Text
            End If
            If txtDataAttivazione.Text <> "" Then
                oMyContatore.sDataAttivazione = txtDataAttivazione.Text
            End If
            If txtDataCessazione.Text <> "" Then
                oMyContratto.sDataCessazione = txtDataCessazione.Text
                oMyContatore.sDataCessazione = txtDataCessazione.Text
            End If
            If txtNumeroUtenze.Text <> "" Then
                oMyContatore.nNumeroUtenze = txtNumeroUtenze.Text
            End If
            If cboTipoUtenze.SelectedItem.Value <> 0 And cboTipoUtenze.SelectedItem.Value <> -1 Then
                oMyContatore.nTipoUtenza = cboTipoUtenze.SelectedItem.Value
            End If
            If cboDiametroContatore.SelectedItem.Value <> 0 And cboDiametroContatore.SelectedItem.Value <> -1 Then
                oMyContatore.nDiametroContatore = cboDiametroContatore.SelectedItem.Value
            End If
            If cboDiametroPresa.SelectedItem.Value <> 0 And cboDiametroPresa.SelectedItem.Value <> -1 Then
                oMyContatore.nDiametroPresa = cboDiametroPresa.SelectedItem.Value
            End If
            oMyContratto.nIdIntestatario = miointestatario.Text
            oMyContratto.nIdUtente = mioutente.Text
            oMyContatore.nIdIntestatario = miointestatario.Text
            oMyContatore.nIdUtente = mioutente.Text
            If txtCivico.Text <> "" Then
                oMyContatore.sCivico = txtCivico.Text
            End If
            '*** Fabi 05032008
            If TxtVia.Text <> "" Then
                oMyContatore.sUbicazione = TxtVia.Text
            End If
            If TxtCodVia.Text <> -1 Then
                oMyContatore.nIdVia = TxtCodVia.Text
            End If
            '*** /Fabi
            If txtDataSottoscr.Text <> "" Then
                oMyContratto.sDataSottoscrizione = txtDataSottoscr.Text
            End If
            If txtNumeroUtenze.Text <> "" Then
                oMyContatore.nNumeroUtenze = txtNumeroUtenze.Text               'Request.Params("hdNumeroUtenzeContratto")
            End If
            If txtIdentificativoContratto.Text <> "" Then
                oMyContratto.sCodiceContratto = txtIdentificativoContratto.Text
            End If
            If txtDataSospsensioneUtenza.Text <> "" Then
                oMyContatore.sDataSospensioneUtenza = txtDataSospsensioneUtenza.Text
            End If
            If txtNumeroCifreContatore.Text <> "" Then
                oMyContatore.sCifreContatore = txtNumeroCifreContatore.Text
            End If
            If cboStatoContatore.SelectedItem.Value <> "" Then
                oMyContatore.sStatoContatore = cboStatoContatore.SelectedItem.Value
            End If
            If ConstSession.CodIstat <> "" Then
                oMyContatore.sCodiceISTAT = ConstSession.CodIstat
            End If
            If txtEsponente.Text <> "" Then
                oMyContatore.sEsponenteCivico = txtEsponente.Text
            End If

            '20022009 Dati agenzia entrate
            If ddlAssenzaDatiCat.SelectedValue <> "" Then
                oMyContatore.nIdAssenzaDatiCatastali = ddlAssenzaDatiCat.SelectedValue
            End If
            If ddlTipoUnita.SelectedValue <> "" Then
                oMyContatore.sTipoUnita = ddlTipoUnita.SelectedValue
            End If
            If ddlTipoUtenza.SelectedValue <> "" Then
                'es. utenza domestica/non domestica
                oMyContatore.nIdTipoUtenza = ddlTipoUtenza.SelectedValue
            End If
            If ddlTitOccupazione.SelectedValue <> "" Then
                'es. proprietario/affittuario
                oMyContatore.nIdTitoloOccupazione = ddlTitOccupazione.SelectedValue
            End If
            '/dati agenzia entrate
            oMyContatore.nIdContatorePrec = StringOperation.FormatInt(Request("idcontatoreprec"))
            If oMyContatore.nIdContatorePrec > 0 Then
                oMyContatore.oDatiCatastali = New GestContatori().GetDetailsCatasto(-1, oMyContatore.nIdContatorePrec)
            End If
            oMyContratto.oContatore = oMyContatore
            Return 1
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DatiGeneraliContr.ValorizzaDatiContratto.errore: ", Err)
            Return -1
        End Try
    End Function

    Private Sub CalcolaNU_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CalcolaNU.Click
        Dim FncMy As New GestContratti
        Dim mionumero As Int32
        Try
            If txtNumeroUtente.Text <> "" Then
                mionumero = txtNumeroUtente.Text
            Else
                mionumero = 0
            End If

            mionumero = FncMy.GetNumeroUtente(ConstSession.IdEnte, mionumero)
            If mionumero > 0 Then
                txtNumeroUtente.Text = mionumero
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DatiGeneraliContr.CalcolaNU_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")

        End Try
    End Sub

    Private Sub btnRibaltaAnagAnater_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRibaltaAnagAnater.Click
        Dim oDettaglioAnagrafica As DettaglioAnagrafica
        Dim sScript As String = ""

        Try
            If Not IsNothing(Session("AnagrafeAnaterRibaltata")) Then
                oDettaglioAnagrafica = CType(Session("AnagrafeAnaterRibaltata"), DettaglioAnagrafica)
                Session("oAnagrafe") = oDettaglioAnagrafica

                Select Case Utility.StringOperation.FormatString(Session("TipoAnagrafica"))
                    Case "Intestatario"
                        HDtxtCodIntestatario.Text = oDettaglioAnagrafica.COD_CONTRIBUENTE
                        miointestatario.Text = oDettaglioAnagrafica.COD_CONTRIBUENTE
                        sScript = "<script language=""javascript"">" + vbCrLf +
                                  "<!-- " + vbCrLf +
                                  "document.getElementById('txtCognomeIntestatario').value ='" & Replace(oDettaglioAnagrafica.Cognome & " " & oDettaglioAnagrafica.Nome, "'", "\'") & "';" + vbCrLf +
                                  "document.getElementById('txtCodiceFiscaleIntestatario').value ='" & oDettaglioAnagrafica.DataNascita & "';" + vbCrLf +
                                  "document.getElementById('txtViaIntestatario').value ='" & Replace(oDettaglioAnagrafica.ViaResidenza & " " & oDettaglioAnagrafica.CivicoResidenza & " " & oDettaglioAnagrafica.CapResidenza & " " & oDettaglioAnagrafica.ComuneResidenza & " " & oDettaglioAnagrafica.ProvinciaResidenza, "'", "\'") & "';" + vbCrLf                                     '+ _
                        If HDTextCodUtente.Text = "-1" Or HDTextCodUtente.Text = HDtxtCodIntestatario.Text Then
                            HDTextCodUtente.Text = oDettaglioAnagrafica.COD_CONTRIBUENTE
                            mioutente.Text = oDettaglioAnagrafica.COD_CONTRIBUENTE
                            sScript += "document.getElementById('txtCognomeUtente').value ='" & Replace(oDettaglioAnagrafica.Cognome & " " & oDettaglioAnagrafica.Nome, "'", "\'") & "';" + vbCrLf +
                                "document.getElementById('txtNomeUtente').value ='" & Replace(oDettaglioAnagrafica.Nome, "'", "\'") & "';" + vbCrLf +
                                "document.getElementById('txtViaUtente').value ='" & Replace(oDettaglioAnagrafica.ViaResidenza & " " & oDettaglioAnagrafica.CivicoResidenza & " " & oDettaglioAnagrafica.CapResidenza & " " & oDettaglioAnagrafica.ComuneResidenza & " " & oDettaglioAnagrafica.ProvinciaResidenza, "'", "\'") & "';" + vbCrLf +
                                "document.getElementById('hdCodAnagrafeUtente').value ='" & oDettaglioAnagrafica.COD_CONTRIBUENTE & "';" + vbCrLf
                        End If
                        sScript += "//parent.window.close();" + vbCrLf +
                             "--> " + vbCrLf +
                             ""

                    Case "Utente"
                        HDTextCodUtente.Text = oDettaglioAnagrafica.COD_CONTRIBUENTE
                        mioutente.Text = oDettaglioAnagrafica.COD_CONTRIBUENTE
                        sScript = "<script language=""javascript"">" + vbCrLf +
                             "<!-- " + vbCrLf +
                            "document.getElementById('txtCognomeUtente').value ='" & Replace(oDettaglioAnagrafica.Cognome & " " & oDettaglioAnagrafica.Nome, "'", "\'") & "';" + vbCrLf +
                            "document.getElementById('txtCodiceFiscaleUtente').value ='" & oDettaglioAnagrafica.DataNascita & "';" + vbCrLf +
                            "document.getElementById('txtViaUtente').value ='" & Replace(oDettaglioAnagrafica.ViaResidenza & " " & oDettaglioAnagrafica.CivicoResidenza & " " & oDettaglioAnagrafica.CapResidenza & " " & oDettaglioAnagrafica.ComuneResidenza & " " & oDettaglioAnagrafica.ProvinciaResidenza, "'", "\'") & "';" + vbCrLf +
                            "document.getElementById('hdCodAnagrafeUtente').value ='" & oDettaglioAnagrafica.COD_CONTRIBUENTE & "';" + vbCrLf
                        If HDTextCodUtente.Text = HDtxtCodIntestatario.Text Then
                            sScript += "document.getElementById('txtCognomeIntestatario').value ='" & Replace(oDettaglioAnagrafica.Cognome & " " & oDettaglioAnagrafica.Nome, "'", "\'") & "';" + vbCrLf +
                                  "document.getElementById('txtCodiceFiscaleIntestatario').value ='" & oDettaglioAnagrafica.DataNascita & "';" + vbCrLf +
                                  "document.getElementById('txtViaIntestatario').value ='" & Replace(oDettaglioAnagrafica.ViaResidenza & " " & oDettaglioAnagrafica.CivicoResidenza & " " & oDettaglioAnagrafica.CapResidenza & " " & oDettaglioAnagrafica.ComuneResidenza & " " & oDettaglioAnagrafica.ProvinciaResidenza, "'", "\'") & "';" + vbCrLf
                        End If
                        sScript += "//parent.window.close();" + vbCrLf +
                             "--> " + vbCrLf +
                             ""
                End Select
                RegisterScript(sScript, Me.GetType())
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DatiGeneraliContr.btnRibaltaAnagAnater_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub LnkAnagAnater_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles LnkAnagAnater.Click
        Session("TipoAnagrafica") = "Intestatario"
    End Sub

    Private Sub LnkAnagrAnatUtente_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles LnkAnagrAnatUtente.Click
        Session("TipoAnagrafica") = "Utente"
    End Sub

    Private Sub txtDataSottoscr_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDataSottoscr.TextChanged
        If txtDataSottoscr.Text <> "" Then
            'Try
            '    DateTime.Parse(txtDataSottoscr.Text)
            'Catch ex As FormatException
            '    MsgBox("Formato data non corretto", MsgBoxStyle.Critical)
            'End Try
            Dim FncContatori As New GestContatori
            Dim dvMyDati As New DataView
            dvMyDati = FncContatori.getListTipoUtenza(ConstSession.IdEnte, txtDataSottoscr.Text, -1)
            cboTipoUtenze.Items.Clear()
            FncGen.FillDropDownSQL(cboTipoUtenze, dvMyDati, -1)
        End If
    End Sub
    '*** 201511 - tolto RIBESFRAMEWORK ***
    'Private Sub LoadDatiAE()
    '    Try
    '        Dim FncAE As New OPENgov_AgenziaEntrate.OPENgov_AgenziaEntrate.General
    '        Dim dvDati As New DataView
    '        Dim sSQL, WFErrore As String
    '        Dim oLoadCombo As New ClsGenerale.Generale
    '        Dim WFSessione As OPENUtility.CreateSessione

    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConfigurationManager.AppSettings("OPENGOVG"))
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("LoadContatori::" & "Errore durante l'apertura della sessione di WorkFlow")
    '            Exit Sub
    '        End If

    '        dvDati = FncAE.LoadComboDati("TIT_OCCUPAZIONE", MyUtility.TRIBUTO_H2O, WFSessione)
    '        oLoadCombo.loadCombo(ddlTitOccupazione, dvDati)

    '        dvDati = FncAE.LoadComboDati("ASSENZA_DATI_CAT", MyUtility.TRIBUTO_H2O, WFSessione)
    '        oLoadCombo.loadCombo(ddlAssenzaDatiCat, dvDati)

    '        dvDati = FncAE.LoadComboDati("TIPO_UNITA", MyUtility.TRIBUTO_H2O, WFSessione)
    '        oLoadCombo.loadCombo(ddlTipoUnita, dvDati)
    '        ddlTipoUnita.SelectedValue = "F"
    '        ddlTipoUnita.Enabled = False

    '        dvDati = FncAE.LoadComboDati("TIPO_UTENZA", MyUtility.TRIBUTO_H2O, WFSessione)
    '        oLoadCombo.loadCombo(ddlTipoUtenza, dvDati)

    '        If oMyContratto.oContatore.nIdAssenzaDatiCatastali <> -1 Then
    '            ddlAssenzaDatiCat.SelectedValue = oMyContratto.oContatore.nIdAssenzaDatiCatastali
    '        End If

    '        If oMyContratto.oContatore.nIdTipoUtenza = -1 Then
    '            ddlTipoUtenza.SelectedValue = ""
    '        Else
    '            ddlTipoUtenza.SelectedValue = oMyContratto.oContatore.nIdTipoUtenza
    '        End If

    '        If oMyContratto.oContatore.sTipoUnita = "" Then
    '            ddlTipoUnita.SelectedValue = "F"
    '        Else
    '            ddlTipoUnita.SelectedValue = oMyContratto.oContatore.sTipoUnita
    '        End If

    '        If oMyContratto.oContatore.nIdTitoloOccupazione = -1 Then
    '            ddlTitOccupazione.SelectedValue = ""
    '        Else
    '            ddlTitOccupazione.SelectedValue = oMyContratto.oContatore.nIdTitoloOccupazione
    '        End If
    '    Catch ex As Exception
    '          Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DatiGeneraliContr.LoadDatiAE.errore: ", ex)
    '          Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    Private Sub LoadDatiAE()
        Try
            Dim FncAE As New OPENgov_AgenziaEntrate.OPENgov_AgenziaEntrate.General
            Dim dvDati As New DataView
            Dim oLoadCombo As New ClsGenerale.Generale

            dvDati = FncAE.LoadComboDati("TIT_OCCUPAZIONE", "9000", ConstSession.StringConnectionOPENgov)
            oLoadCombo.loadCombo(ddlTitOccupazione, dvDati)

            dvDati = FncAE.LoadComboDati("ASSENZA_DATI_CAT", "9000", ConstSession.StringConnectionOPENgov)
            oLoadCombo.loadCombo(ddlAssenzaDatiCat, dvDati)

            dvDati = FncAE.LoadComboDati("TIPO_UNITA", "9000", ConstSession.StringConnectionOPENgov)
            oLoadCombo.loadCombo(ddlTipoUnita, dvDati)
            ddlTipoUnita.SelectedValue = "F"
            ddlTipoUnita.Enabled = False

            dvDati = FncAE.LoadComboDati("TIPO_UTENZA", "9000", ConstSession.StringConnectionOPENgov)
            oLoadCombo.loadCombo(ddlTipoUtenza, dvDati)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DatiGeneraliContr.LoadDatiAE.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** ***


    'M.B.


#Region "Griglie"

    'aggiunta per test
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim IDRow As String = e.CommandArgument.ToString()
            If e.CommandName = "RowOpen" Then
                For Each myRow As GridViewRow In GrdRifCat.Rows

                    If IDRow = CType(myRow.FindControl("ID"), HiddenField).Value Then

                    End If
                Next
            ElseIf e.CommandName = "RowDelete" Then

            End If
        Catch ex As Exception
            Log.Debug(".GrdRowCommand::errore::", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub




    Protected Sub GrdRowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            If (e.Row.RowType = DataControlRowType.DataRow) Then

            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicFatturazione.GrdRowDataBound.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub


    ''' <summary>
    ''' Gestione del cambio pagina della griglia.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdPageIndexChanging(ByVal sender As Object, e As GridViewPageEventArgs)
        'LoadSearch(e.NewPageIndex)
    End Sub

#End Region






End Class
