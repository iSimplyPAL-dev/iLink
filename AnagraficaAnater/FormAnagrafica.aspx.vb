Imports System
Imports System.Xml
Imports System.Collections
Imports System.Text
Imports System.ComponentModel
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports Anagrafica.DLL
Imports AnagInterface
Imports RemotingInterfaceAnater
Imports Anater.Oggetti
Imports OggettiComuniStrade
Imports log4net

'Namespace OPENgov
Partial Class FormAnagrafica
    Inherits BasePage
    Private FncAnater As New AnagrafeAnater.BasePageAnater
    Private strURI As System.Uri
    Private PAG_PREC As String
    Private popup As String
    Private Log As ILog = LogManager.GetLogger(GetType(FormAnagrafica))
    Public UrlPopComuni As String = COSTANTValue.ConstSession.UrlComuni
    Public UrlStradario As String = COSTANTValue.ConstSession.UrlStradario
    Protected FncGrd As New FunctionGrd
    Private GestError As New GestError
    Private _strXMLFileName As String

#Region " Web Form Designer Generated Code "
    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    '''''''''''''''''''''''''''''''''''''TextBox UTENTE'''''''''''''''''''''''''''''''''''''''''''''//
    Protected WithEvents txtCFPIUtente As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCognomeUtente As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtNomeUtente As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtComuneNascitaUtente As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtProvinciaNascitaUtente As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtDataNascitaUtente As System.Web.UI.WebControls.TextBox

    Protected WithEvents txtIndirizzoResidenzaUtente As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCivicoResidenzaUtente As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFrazioneResidenzaUtente As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtComuneResidenzaUtente As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCAPResidenzaUtente As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtProvinciaResidenzaUtente As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtTelefonoResidenzaUtente As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtNucleoFamiliareUtente As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCellulareResidenzaUtente As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCognomeSpedizioneUtente As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtNomeSpedizioneUtente As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtIndirizzoSpedizioneUtente As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCivicoSpedizioneUtente As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFrazioneSpedizioneUtente As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtComuneSpedizioneUtente As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCAPSpedizioneUtente As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtProvinciaSpedizioneUtente As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtBancaUtente As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtAgenziaUtente As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtABIUtente As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtCABUtente As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtNumeroCCUtente As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtEMailUtente As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtFaxUtente As System.Web.UI.WebControls.TextBox
    '''''''''''''''''''''''''''''''''''''Fine TextBox'''''''''''''''''''''''''''''''''''''''''''''//
    '''''''''''''''''''''''''''''''''''''CheckBox INTESTATARIO'''''''''''''''''''''''''''''''''''''''''''''//
    Protected WithEvents chkDomiciliazione As System.Web.UI.WebControls.CheckBox
    '''''''''''''''''''''''''''''''''''''Fine ChecktBox INTESTATARIO'''''''''''''''''''''''''''''''''''''''''''''//
    '''''''''''''''''''''''''''''''''''''CheckBox UTENTE'''''''''''''''''''''''''''''''''''''''''''''//
    Protected WithEvents chkDomiciliazioneUtente As System.Web.UI.WebControls.CheckBox
    '''''''''''''''''''''''''''''''''''''Fine ChecktBox UTENTE'''''''''''''''''''''''''''''''''''''''''''''//
    ''''''''''''''''''''''''''''''''''''' Radio Button INTESTATARIO'''''''''''''''''''''''''''''''''''''''''''''//
    Protected WithEvents Residenza As System.Web.UI.WebControls.RadioButtonList
    '''''''''''''''''''''''''''''''''''''Fine Radio Button INTESTATARIO'''''''''''''''''''''''''''''''''''''''''''''//
    ''''''''''''''''''''''''''''''''''''' Radio Button UTENTE'''''''''''''''''''''''''''''''''''''''''''''//
    Protected WithEvents ResidenzaUtente As System.Web.UI.WebControls.RadioButtonList
    '''''''''''''''''''''''''''''''''''''Fine Radio Button UTENTE'''''''''''''''''''''''''''''''''''''''''''''//

    ''''''''''''''''''''''''''''''''''''' DropDownList INTESTATARIO'''''''''''''''''''''''''''''''''''''''''''''//
    Protected WithEvents cboTitoloSoggetto As System.Web.UI.WebControls.DropDownList
    ''''''''''''''''''''''''''''''''''''' fine DropDownList INTESTATARIO'''''''''''''''''''''''''''''''''''''''''''''//
    ''''''''''''''''''''''''''''''''''''' DropDownList UTENTE'''''''''''''''''''''''''''''''''''''''''''''//
    Protected WithEvents cboSessoUtente As System.Web.UI.WebControls.DropDownList
    Protected WithEvents cboTitoloUtente As System.Web.UI.WebControls.DropDownList
    ''''''''''''''''''''''''''''''''''''' fine DropDownList  UTENTE'''''''''''''''''''''''''''''''''''''''''''''//
    ''''''''''''''''''''''''''''''''''/GESTIONE PROCESSO''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ''''''''''''''''''''''''''''''''''/GESTIONE PROCESSO''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    Protected WithEvents btnConferma As System.Web.UI.WebControls.Button


    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub
#End Region
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="27/10/2014">
    ''' <strong>visualizzazione tutti indirizzi spedizione</strong>
    ''' Nuova gestione indirizzi spedizione
    ''' </revision>
    ''' </revisionHistory>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
            CmdSaveSpedizione.Attributes.Add("OnClick", "return VerificaCampiObbligatoriSpedizione();")
            '*** ***
            btnSalva.Attributes.Add("OnClick", "return VerificaCampiObbligatori();")
            'btnControlloCINSalva.Attributes.Add("OnClick", "return VerificaCampiObbligatori();")
            btnDelete.Attributes.Add("OnClick", "return Conferma();")
            popup = Request.Item("popup")
            If Not Page.IsPostBack Then
                If CBool(ViewState("sessionName")) = False Then
                    ViewState("sessionName") = Request.Item("sessionName")
                End If
                Dim strscript As String

                If Request.Item("PAGEFROM") = "DOPP" Then 'Or Request.Item("PAGEFROM") = "MANC" Then
                    strscript = "parent.Comandi.location.href='./Comandi/ComandiAnagDoppieMancanti.aspx'" & vbCrLf
                Else
                    If popup = "1" Then
                        strscript = "parent.Comandi.location.href='./ComandiInsertSaveAnagraficaAnater.aspx'" & vbCrLf
                    Else
                        strscript = "parent.Comandi.location.href='./ComandiInsertSaveAnagraficaAnater.aspx'" & vbCrLf
                    End If
                End If
                RegisterScript(strscript, Me.GetType())

                '***************************************************************************************************
                '***************************************************************************************************
                LoadCombos()
                'Caricamento Dati Anagrafica
                LoadAnagrafica()
                GestResidenti.Style.Add("display", "none")
                'Gestione Storico Anagrafica
                If CInt(Request("STORICO")) <> Utility.Costanti.INIT_VALUE_NUMBER Then
                    SetEnabledAll(True, Nothing)
                End If

                ViewState.Add("PAGEFROM", Request.Item("PAGEFROM"))
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FormAnagrafica.PageLoad.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <revisionHistory>
    ''' <revision date="27/10/2014">
    ''' <strong>visualizzazione tutti indirizzi spedizione</strong>
    ''' Nuova gestione indirizzi spedizione
    ''' </revision>
    ''' </revisionHistory>
    Private Sub LoadAnagrafica()
        Dim COD_FISCALE As String
        Dim intIdRiga As Integer
        Dim remObject As IRemotingInterfaceICI = Activator.GetObject(GetType(IRemotingInterfaceICI), ConfigurationManager.AppSettings("URLanaterICI"))
        Dim sScript As String = ""
        lblConcurrencyMsg.Visible = False
        ''''GESTIONE ERRORE

        Try
            COD_FISCALE = Request.Item("COD_FISCALE")
            If Request.Item("idRiga") <> "" Then
                intIdRiga = Convert.ToInt32(Request.Item("idRiga"))
            Else
                intIdRiga = 0
            End If
        Catch ex As Exception
            ''''Verifica Del Passaggio dei Parametri
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FormAnagrafica.LoadAnagrafica.errore: ", ex)
            Throw New Exception("Errore Passaggio dei Parametri.I Parametri passati non sono corretti")
        End Try
        Dim oAnagrafica As New GestioneAnagrafica()
        Dim oDettaglioAnagraficaDLL As New DettaglioAnagrafica
        Dim objDatiComune As New clsAnagrafeAnater
        Dim DatiComune As clsAnagrafeAnater.DettaglioEnte
        Dim oDettaglioAnagrafica() As OggettoAnagraficaAnater

        oDettaglioAnagrafica = remObject.GetAnagraficaANATER(String.Empty, String.Empty, COD_FISCALE, String.Empty, CInt(COSTANTValue.ConstSession.IdEnte), intIdRiga)
        Try
            If oDettaglioAnagrafica.Length = 1 Then
                hdCODViaResidenza.Value = oDettaglioAnagrafica(0).AnagCodViaResid.ToString
                Select Case oDettaglioAnagrafica(0).AnagTipoSogg
                    Case "F"
                        txtCodiceFiscale.Text = oDettaglioAnagrafica(0).AnagCodFisc
                        txtPartitaIva.Text = ""
                    Case "D"
                        txtCodiceFiscale.Text = ""
                        txtPartitaIva.Text = oDettaglioAnagrafica(0).AnagPartIva
                    Case Else
                        txtCodiceFiscale.Text = oDettaglioAnagrafica(0).AnagCodFisc
                        txtPartitaIva.Text = ""
                End Select

                txtCognome.Text = oDettaglioAnagrafica(0).AnagCognome
                txtNome.Text = oDettaglioAnagrafica(0).AnagNome
                FncAnater.SelectIndexDropDownList(cboSesso, oDettaglioAnagrafica(0).AnagSesso)

                ' recupero i dati relativi al comune di nascita dela anagrafica
                If oDettaglioAnagrafica(0).AnagCodComNasc <> 0 Then
                    DatiComune = objDatiComune.GetEnteByCodComune(oDettaglioAnagrafica(0).AnagCodComNasc.ToString)
                    txtLuogoNascita.Text = DatiComune.COMUNE
                    txtProvinciaNascita.Text = DatiComune.PROV
                    hdCodComuneNascita.Value = DatiComune.CODBELFIORE
                Else
                    txtLuogoNascita.Text = ""
                    txtProvinciaNascita.Text = ""
                End If
                '***************************************************************************************************************
                If oDettaglioAnagrafica(0).AnagDataNascita <> DateTime.MinValue Then
                    txtDataNascita.Text = oDettaglioAnagrafica(0).AnagDataNascita.ToShortDateString()
                Else
                    txtDataNascita.Text = ""
                End If
                txtNazionalitaNascita.Text = ""
                txtProfessione.Text = ""
                txtNucleoFamiliare.Text = ""
                'Caricamento combo Tipi Riferimenti
                '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
                'oDettaglioAnagraficaDLL = oAnagrafica.GetAnagrafica(-1, COSTANTValue.ConstSession.CodTributo, COSTANTValue.Costanti.INIT_VALUE_NUMBER, COSTANTValue.ConstSession.StringConnectionAnagrafica)
                oDettaglioAnagraficaDLL = oAnagrafica.GetAnagrafica(-1, Utility.Costanti.INIT_VALUE_NUMBER, "", COSTANTValue.ConstSession.DBType, COSTANTValue.ConstSession.StringConnectionAnagrafica, False)
                LoadDropDownList(cboTipoContatto, oDettaglioAnagraficaDLL.dsTipiContatti, "IDTipoRiferimento", "DESCRIZIONE", -1)

                Session("DataSetContatti") = oDettaglioAnagraficaDLL.dsContatti
                dgContatti.DataSource = oDettaglioAnagraficaDLL.dsContatti
                dgContatti.DataBind()
                ' recupero i dati relativi al comune di residenza della anagrafica
                If oDettaglioAnagrafica(0).AnagCodComResid <> 0 Then
                    DatiComune = objDatiComune.GetEnteByCodComune(oDettaglioAnagrafica(0).AnagCodComResid.ToString)
                    txtComuneResidenza.Text = DatiComune.COMUNE
                    txtCAPResidenza.Text = DatiComune.CAP
                    txtProvinciaResidenza.Text = DatiComune.PROV
                    sScript += "document.frmAnagrafica.hdCodComuneResidenza.value='" & DatiComune.CODBELFIORE & "';"
                Else
                    txtComuneResidenza.Text = ""
                    txtCAPResidenza.Text = ""
                    txtProvinciaResidenza.Text = ""
                End If
                txtViaResidenza.Text = oDettaglioAnagrafica(0).AnagDescrViaResid

                ' se trovo il codice via faccio la ricerca nello stradario e mi prendo la descrizione e il codice. 
                If oDettaglioAnagrafica(0).AnagCodViaResid <> 0 Then
                    Dim oStradaRes As OggettoStrada = GetStradaAnater(oDettaglioAnagrafica(0).AnagCodViaResid, COSTANTValue.ConstSession.IdEnte)
                    If Not oStradaRes Is Nothing Then
                        ' popolo i campi coi dati della strada
                        txtViaResidenza.Text = oStradaRes.Strada
                    End If
                End If
                txtPosizioneResidenza.Text = "" 'oDettaglioAnagrafica.PosizioneCivicoResidenza
                If oDettaglioAnagrafica(0).AnagNumCivResid <> 0 Then
                    txtNumeroCivicoResidenza.Text = oDettaglioAnagrafica(0).AnagNumCivResid.ToString ' oDettaglioAnagrafica.CivicoResidenza
                Else
                    txtNumeroCivicoResidenza.Text = ""
                End If
                txtEsponenteCivicoResidenza.Text = oDettaglioAnagrafica(0).AnagEspoResid 'oDettaglioAnagrafica.EsponenteCivicoResidenza
                txtScalaResidenza.Text = oDettaglioAnagrafica(0).AnagScalaResid 'oDettaglioAnagrafica.ScalaCivicoResidenza
                txtInternoResidenza.Text = oDettaglioAnagrafica(0).AnagInternoResid 'oDettaglioAnagrafica.InternoCivicoResidenza
                txtFrazioneResidenza.Text = "" 'oDettaglioAnagrafica.FrazioneResidenza
                txtNazionalitaResidenza.Text = "" 'oDettaglioAnagrafica.NazionalitaResidenza

                txtCognomeSpedizione.Text = ""
                txtNomeSpedizione.Text = ""

                If oDettaglioAnagrafica(0).AnagCodComRec <> 0 Then
                    Dim ListSpedizione As New Generic.List(Of ObjIndirizziSpedizione)
                    Dim mySped As New ObjIndirizziSpedizione

                    DatiComune = objDatiComune.GetEnteByCodComune(oDettaglioAnagrafica(0).AnagCodComRec.ToString)
                    mySped.ViaRCP = oDettaglioAnagrafica(0).AnagDescrViaRec
                    mySped.CivicoRCP = oDettaglioAnagrafica(0).AnagNumCivRec.ToString
                    mySped.EsponenteCivicoRCP = oDettaglioAnagrafica(0).AnagEspoRec
                    mySped.ScalaCivicoRCP = oDettaglioAnagrafica(0).AnagScalaRec
                    mySped.InternoCivicoRCP = oDettaglioAnagrafica(0).AnagInternoRec
                    mySped.ComuneRCP = DatiComune.COMUNE
                    mySped.CapRCP = DatiComune.CAP
                    mySped.ProvinciaRCP = DatiComune.PROV
                    ListSpedizione.Add(mySped)
                    GrdInvio.DataSource = ListSpedizione.ToArray
                    GrdInvio.DataBind()
                End If

                txtNoteAnagrafica.Text = oDettaglioAnagrafica(0).AnagNote 'oDettaglioAnagrafica.Note
                chkDaRicontrollare.Checked = False 'oDettaglioAnagrafica.DaRicontrollare

                '// Registro lo script per valorizzare i campi nascosti.
                RegisterScript(sScript, Me.GetType())
            End If
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FormAnagrafica.LoadAnagrafica.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    Private Sub LoadDropDownList(ByVal cboTemp As DropDownList, ByVal dsTemp As DataSet, ByVal DataValueField As String, ByVal DataTextField As String, ByVal lngSelectedID As Long)
        cboTemp.DataSource = dsTemp
        cboTemp.DataValueField = DataValueField
        cboTemp.DataTextField = DataTextField
        cboTemp.DataBind()
    End Sub
    Protected Sub SetEnabledAll(ByVal bEnabled As Boolean, ByVal ctlPageForm As System.Web.UI.Control)
        Dim ctlForm As Control
        Dim strType As String

        Try
            If ctlPageForm Is Nothing Then
                ctlPageForm = Me.Page
            End If
            strType = ctlPageForm.ToString()

            If TypeOf ctlPageForm Is System.Web.UI.HtmlControls.HtmlForm Or TypeOf ctlPageForm Is System.Web.UI.WebControls.Panel Or strType.IndexOf("MultiPage") <> -1 Or strType.IndexOf("PageView") <> -1 Then
                ctlForm = ctlPageForm
            Else
                If TypeOf ctlPageForm Is System.Web.UI.Page Then
                    ctlForm = ctlPageForm
                    Dim ctlItem As Control
                    For Each ctlItem In ctlPageForm.Controls
                        If TypeOf ctlItem Is System.Web.UI.HtmlControls.HtmlForm Then
                            ctlForm = ctlItem
                            Exit For
                        End If
                    Next ctlItem
                Else
                    Return
                End If
            End If
            Dim ctl As Control
            For Each ctl In ctlForm.Controls
                If TypeOf ctl Is System.Web.UI.WebControls.DataGrid Or TypeOf ctl Is System.Web.UI.WebControls.TextBox Or TypeOf ctl Is System.Web.UI.WebControls.DropDownList Or TypeOf ctl Is System.Web.UI.WebControls.ListBox Or TypeOf ctl Is System.Web.UI.WebControls.CheckBox Or TypeOf ctl Is System.Web.UI.WebControls.CheckBoxList Or TypeOf ctl Is System.Web.UI.WebControls.RadioButton Or TypeOf ctl Is System.Web.UI.WebControls.RadioButtonList Then
                    If CType(ctl, WebControl).ClientID <> "CommandAction" Then
                        CType(ctl, WebControl).Enabled = bEnabled
                    End If
                Else
                    strType = ctl.ToString()
                    If TypeOf ctl Is System.Web.UI.WebControls.Panel Or strType.IndexOf("MultiPage") <> -1 Or strType.IndexOf("PageView") <> -1 Then
                        SetEnabledAll(bEnabled, ctl)
                    End If
                End If
            Next ctl
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FormAnagrafica.SetEnabledAll.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    Protected Function DescRiferimento(ByVal prdStatus As Object) As String
        Dim oAnagrafica As New GestioneAnagrafica()
        DescRiferimento = oAnagrafica.DescrizioneTipoContatto(prdStatus, COSTANTValue.ConstSession.StringConnectionAnagrafica)
        Return DescRiferimento
    End Function
    Private Sub GestReturnPage(ByVal oDettaglioAnagrafica As DettaglioAnagrafica, ByVal lngCOD_CONTRIBUENTE As Long)
        Try
            If ViewState("PAGEFROM").ToString = "DOPP" Then
                Dim strBuilder As New System.Text.StringBuilder
                strBuilder.Append("parent.Visualizza.location.href='RicercaAnagraficaDoppia.aspx';")
                RegisterScript(strBuilder.ToString(), Me.GetType())
            ElseIf ViewState("PAGEFROM").ToString = "MANC" Then
                Dim strBuilder As New System.Text.StringBuilder
                strBuilder.Append("parent.Visualizza.location.href='RicercaAnagraficaMancante.aspx';")
                RegisterScript(strBuilder.ToString(), Me.GetType())
            Else

                Dim buffScriptString As New System.Text.StringBuilder
                'Dim Costant As New ANAGRAFICAWEB.Costanti

                ''''Se non si sono verificati errori gestione della Pagina su cui tornare dopo il salvataggio

                ''''Gestione dell'Anagrafica da parte del data Entry massivo dei contatori 
                'PAG_PREC = Request("PAG_PREC")
                buffScriptString.Append("document.frmAnagrafica.hdIDDATAANAGRAFICA.value ='" & Replace(oDettaglioAnagrafica.ID_DATA_ANAGRAFICA.ToString, "'", "\'") & "';")
                buffScriptString.Append("document.frmAnagrafica.hdCOD_CONTRIBUENTE.value ='" & Replace(oDettaglioAnagrafica.COD_CONTRIBUENTE.ToString, "'", "\'") & "';")
                If (StrComp(Session("modificaDiretta").ToString, "True") = 0) Then
                    Session(ViewState("sessionName").ToString) = oDettaglioAnagrafica
                    buffScriptString.Append("parent.parent.opener.document.getElementById('btnRibalta').click();" & vbCrLf)
                End If
                buffScriptString.Append("parent.Comandi.location.href='cRicAnagrafeAnater.aspx?popup=" & popup & "';")
                buffScriptString.Append("parent.Visualizza.location.href='RicAnagrafeAnater.aspx?sessionName=" & ViewState("sessionName").ToString & "&popup=" & popup & "';")
                RegisterScript(buffScriptString.ToString(), Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FormAnagrafica.GestReturnPage.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    Private Function GetStradaAnater(ByVal CodStrada As Integer, ByVal CodEnte As String) As OggettoStrada
        Try
            Dim ArrStrade As OggettoStrada()
            Dim Strada As OggettoStrada = New OggettoStrada
            Strada.CodiceStrada = CodStrada
            Strada.CodiceEnte = CodEnte

            Dim objStradario As WsStradario.Stradario = New WsStradario.Stradario

            ArrStrade = objStradario.GetStrade(Strada)

            If ArrStrade.Length > 0 Then
                Return ArrStrade(0)
            Else
                Return Nothing
            End If

        Catch ex As Exception
            Dim strBuild As StringBuilder = New StringBuilder
            strBuild.Append("GestAlert('a', 'warning', '', '', 'Si sono verificati dei problemi nello stradario. Contattare il servizio di assistenza!');")
            RegisterScript(strBuild.ToString(), Me.GetType())
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FormAnagrafica.GetStradaAnater.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Return Nothing
        End Try
    End Function
    Private Sub btnSalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalva.Click
        Dim blnUpdate As Boolean = True
        Dim blnReturnSearch As Boolean

        Dim lngCOD_CONTRIBUENTE As Long = Utility.Costanti.INIT_VALUE_NUMBER
        Session.LCID = 1040

        Try
            Dim oAnagrafica As New GestioneAnagrafica()
            Dim oDettaglioAnagrafica As New DettaglioAnagrafica
            Log.Debug("inizio")
            oDettaglioAnagrafica.COD_CONTRIBUENTE = hdIdContribuente.Value
            oDettaglioAnagrafica.ID_DATA_ANAGRAFICA = hdIdAnagrafica.Value
            Log.Debug("prelevo anagrafica")
            oDettaglioAnagrafica.CodiceFiscale = txtCodiceFiscale.Text
            oDettaglioAnagrafica.PartitaIva = txtPartitaIva.Text
            oDettaglioAnagrafica.Cognome = txtCognome.Text
            oDettaglioAnagrafica.Nome = txtNome.Text
            If cboSesso.SelectedItem.Value = CStr(Utility.Costanti.INIT_VALUE_NUMBER) Then
                oDettaglioAnagrafica.Sesso = ""
            Else
                oDettaglioAnagrafica.Sesso = cboSesso.SelectedItem.Value
            End If
            Log.Debug("prelevo nascita")
            oDettaglioAnagrafica.ComuneNascita = txtLuogoNascita.Text
            oDettaglioAnagrafica.CodiceComuneNascita = hdCodComuneNascita.Value
            oDettaglioAnagrafica.ProvinciaNascita = txtProvinciaNascita.Text
            oDettaglioAnagrafica.DataNascita = txtDataNascita.Text
            oDettaglioAnagrafica.DataMorte = txtDataMorte.Text
            oDettaglioAnagrafica.NazionalitaNascita = txtNazionalitaNascita.Text
            Log.Debug("prelevo residenza")
            oDettaglioAnagrafica.ComuneResidenza = txtComuneResidenza.Text
            oDettaglioAnagrafica.CodiceComuneResidenza = hdCodComuneResidenza.Value
            oDettaglioAnagrafica.CapResidenza = txtCAPResidenza.Text
            oDettaglioAnagrafica.ProvinciaResidenza = txtProvinciaResidenza.Text
            oDettaglioAnagrafica.ViaResidenza = txtViaResidenza.Text
            oDettaglioAnagrafica.CodViaResidenza = hdCODViaResidenza.Value
            oDettaglioAnagrafica.PosizioneCivicoResidenza = txtPosizioneResidenza.Text
            oDettaglioAnagrafica.CivicoResidenza = txtNumeroCivicoResidenza.Text
            oDettaglioAnagrafica.EsponenteCivicoResidenza = txtEsponenteCivicoResidenza.Text
            oDettaglioAnagrafica.ScalaCivicoResidenza = txtScalaResidenza.Text
            oDettaglioAnagrafica.InternoCivicoResidenza = txtInternoResidenza.Text
            oDettaglioAnagrafica.FrazioneResidenza = txtFrazioneResidenza.Text
            oDettaglioAnagrafica.NazionalitaResidenza = txtNazionalitaResidenza.Text
            Log.Debug("prelevo altro")
            oDettaglioAnagrafica.Professione = txtProfessione.Text
            oDettaglioAnagrafica.NucleoFamiliare = txtNucleoFamiliare.Text
            oDettaglioAnagrafica.RappresentanteLegale = ""
            oDettaglioAnagrafica.CodContribuenteRappLegale = hdCODRappresentanteLegale.Value
            oDettaglioAnagrafica.Operatore = COSTANTValue.ConstSession.UserName
            oDettaglioAnagrafica.Note = txtNoteAnagrafica.Text
            oDettaglioAnagrafica.DaRicontrollare = chkDaRicontrollare.Checked
            oDettaglioAnagrafica.CodEnte = COSTANTValue.ConstSession.IdEnte
            'Log.Debug("prelevo Concurrency")
            'oDettaglioAnagrafica.Concurrency = GetConcurrencyObject().Concurrency
            If Not Session("IndirizziSpedizione") Is Nothing Then
                Log.Debug("prelevo spedizione")
                oDettaglioAnagrafica.ListSpedizioni = Session("IndirizziSpedizione")
            End If
            Log.Debug("prelevo contatti")
            oDettaglioAnagrafica.dsContatti = Session("DataSetContatti")
            If oDettaglioAnagrafica.COD_CONTRIBUENTE = Utility.Costanti.INIT_VALUE_NUMBER Then
                '*************************************************
                'NUOVO
                '*************************************************
                Log.Debug("inserisco nuovo")
                lngCOD_CONTRIBUENTE = oAnagrafica.SetAnagraficaCompleta(oDettaglioAnagrafica, COSTANTValue.ConstSession.DBType, COSTANTValue.ConstSession.StringConnectionAnagrafica)
            Else
                '*************************************************
                'MODIFICA
                '************************************************
                Log.Debug("inserisco modifica")
                Try
                    'Se il record non e' Loccato Continua
                    oAnagrafica.UpdateForLock(oDettaglioAnagrafica, COSTANTValue.ConstSession.StringConnectionAnagrafica)
                Catch ex As DBConcurrencyException
                    Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FormAnagrafica.btnSalva_Click.errore: ", ex)
                    Log.Debug("errore in DBConcurrencyException::", ex)
                    If ConcurrencyException(COSTANTValue.ConstSession.DBType, COSTANTValue.ConstSession.StringConnectionAnagrafica) Then
                        blnUpdate = False
                    Else
                        blnUpdate = False
                        'Se non sono state trovate modifiche sui campi
                        blnReturnSearch = True
                    End If
                    Dim strPostForm As New System.Text.StringBuilder
                    strPostForm.Append("document.frmAnagrafica.btnCodiceFiscale.disabled=true;")
                    strPostForm.Append("document.frmAnagrafica.btnDaCodiceFiscale.disabled=true;")
                    strPostForm.Append("document.frmAnagrafica.btnCaricaTabella.disabled=true;")
                    RegisterScript(strPostForm.ToString(), Me.GetType())
                Catch ex As DeletedRowInaccessibleException
                    Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FormAnagrafica.btnSalva_Click.errore: ", ex)
                    Log.Debug("errore in DeletedRowInaccessibleException::", ex)
                    Throw
                Catch ex As Exception
                    Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FormAnagrafica.btnSalva_Click.errore: ", ex)
                    Log.Debug("errore in updateforlock", ex)
                    Throw
                End Try

                'Se non c'e' stata concorrenza sul record si eseguono operazioni sul Data Base Anagrafica
                If blnUpdate Then
                    Log.Debug("devo fare gestiscianagrafica")
                    Dim oMyAnag As New DettaglioAnagraficaReturn
                    oMyAnag = oAnagrafica.GestisciAnagrafica(oDettaglioAnagrafica, COSTANTValue.ConstSession.DBType, COSTANTValue.ConstSession.StringConnectionAnagrafica, False, True)
                    'oMyAnag = oAnagrafica.GestisciAnagrafica(oDettaglioAnagrafica, COSTANTValue.ConstSession.StringConnectionAnagrafica, False)
                    lngCOD_CONTRIBUENTE = oMyAnag.COD_CONTRIBUENTE
                End If    'blnUpdate
            End If
            '************************************************
            ''''Se non si sono verificati errori gestione della Pagina su cui tornare dopo il salvataggio
            If blnUpdate Then
                GestReturnPage(oDettaglioAnagrafica, lngCOD_CONTRIBUENTE)
            End If
            If blnReturnSearch Then
                GestReturnPage(oDettaglioAnagrafica, lngCOD_CONTRIBUENTE)
            End If
            DivAttesa.Style.Add("display", "none")
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FormAnagrafica.btnSalva_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    Private Sub btnAnnulla_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAnnulla.Click
        Dim strBuilder As New System.Text.StringBuilder
        Try
            If StrComp(Session("modificaDiretta").ToString, "True") = 0 Then
                strBuilder.Append("parent.window.close();")
            Else
                If ViewState("PAGEFROM").ToString = "DOPP" Then
                    strBuilder.Append("parent.Visualizza.location.href='RicercaAnagraficaDoppia.aspx';")
                ElseIf ViewState("PAGEFROM").ToString = "MANC" Then
                    strBuilder.Append("parent.Visualizza.location.href='RicercaAnagraficaMancante.aspx';")
                Else
                    strBuilder.Append("parent.Visualizza.location.href='RicAnagrafeAnater.aspx?popup=" & popup & "&sessionName=" & ViewState("sessionName").ToString & "';")
                    strBuilder.Append("parent.Comandi.location.href='cRicAnagrafeAnater.aspx';")
                End If
            End If
            RegisterScript(strBuilder.ToString(), Me.GetType())
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FormAnagrafica.btnAnnulla_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim oAnagrafica As New GestioneAnagrafica()

        Try
            oAnagrafica.DeleteAnagrafica(hdIdContribuente.Value, hdIdAnagrafica.Value, COSTANTValue.ConstSession.DBType, COSTANTValue.ConstSession.StringConnectionAnagrafica)
            DivAttesa.Style.Add("display", "none")
            Dim strBuilder As New System.Text.StringBuilder
            strBuilder.Append("parent.Visualizza.location.href='RicercaAnagrafica.aspx?popup=" & popup & "';")
            RegisterScript(strBuilder.ToString(), Me.GetType())
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FormAnagrafica.btnDelete_Click.errore: ", ex)
            If ex.Message.IndexOf("00000") > 0 Then
                Dim strBuild As New System.Text.StringBuilder
                strBuild.Append("GestAlert('a', 'danger', '', '', 'Impossibile cancellare la posizione anagrafica selezionata, è utilizzata all\'interno del sistema!');")
                RegisterScript(strBuild.ToString(), Me.GetType())
            Else
                Response.Redirect("../../PaginaErrore.aspx")
            End If
        End Try
    End Sub
    Private Sub btnCancella_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancella.Click
        Try
            Dim hdCOD_CONTRIBUENTE As Integer = Request.Params("hdCOD_CONTRIBUENTE")
            Dim ID_DATA_ANAGRAFICA As String = Request.Params("ID_DATA_ANAGRAFICA")
            Dim oAnagrafica As New GestioneAnagrafica()

            oAnagrafica.DeleteAnagrafica(hdCOD_CONTRIBUENTE, ID_DATA_ANAGRAFICA, COSTANTValue.ConstSession.DBType, COSTANTValue.ConstSession.StringConnectionAnagrafica)
            Dim strBuilder As New System.Text.StringBuilder
            strBuilder.Append("parent.Visualizza.location.href='RicercaAnagrafica.aspx?popup=" & popup & "';")
            RegisterScript(strBuilder.ToString(), Me.GetType())
        Catch EX As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FormAnagrafica.btnCancella_Click.errore: ", EX)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    Private Function ConcurrencyException(DBType As String, ByVal StringConnection As String) As Boolean
        Dim operatore As String
        Dim message As String
        Dim controls As Hashtable = GetControlsMap()

        ConcurrencyException = False
        Try
            If FncAnater.ShowConcurrencyFields(controls, operatore, DBType, StringConnection) Then
                'Se sono state fatte delle Modifiche da un altro utente trova delle differenze e mostra le differenze dei campi
                ConcurrencyException = True
            End If
            lblConcurrencyMsg.Visible = True
            message = "l' Utente " & operatore & " ha modificato questa anagrafica da quando avete cominciato le Vostre modifiche." & vbCrLf
            message = message & "I cambiamenti fatti dall'altro utente " & operatore & " sono indicati dal colore grigio." & vbCrLf
            message = message & "Prego Ricontrollare i cambiamenti." & vbCrLf
            message = message & "Per modificare nuovamente questa anagrafica,riselezionarla dalla pagina di ricerca."

            lblConcurrencyMsg.Text = message

            SetEnabledAll(False, Nothing)
        Catch EX As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FormAnagrafica.ConcurrencyException.errore: ", EX)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Function
    Private Function GetControlsMap() As Hashtable
        Dim Controls As Hashtable = New Hashtable

        Controls.Add("CodiceFiscale", txtCodiceFiscale)
        Controls.Add("PartitaIva", txtPartitaIva)
        Controls.Add("Cognome", txtCognome)
        Controls.Add("Nome", txtNome)

        Controls.Add("ComuneNascita", txtLuogoNascita)
        Controls.Add("ProvinciaNascita", txtProvinciaNascita)
        Controls.Add("DataNascita", txtDataNascita)
        Controls.Add("DataMorte", txtDataMorte)
        Controls.Add("NazionalitaNascita", txtNazionalitaNascita)
        Controls.Add("Professione", txtProfessione)
        Controls.Add("NucleoFamiliare", txtNucleoFamiliare)
        Controls.Add("ComuneResidenza", txtComuneResidenza)
        Controls.Add("CapResidenza", txtCAPResidenza)
        Controls.Add("ProvinciaResidenza", txtProvinciaResidenza)
        Controls.Add("ViaResidenza", txtViaResidenza)
        Controls.Add("PosizioneCivicoResidenza", txtPosizioneResidenza)
        Controls.Add("CivicoResidenza", txtNumeroCivicoResidenza)
        Controls.Add("EsponenteCivicoResidenza", txtEsponenteCivicoResidenza)
        Controls.Add("ScalaCivicoResidenza", txtScalaResidenza)
        Controls.Add("InternoCivicoResidenza", txtInternoResidenza)
        Controls.Add("FrazioneResidenza", txtFrazioneResidenza)
        Controls.Add("NazionalitaResidenza", txtNazionalitaResidenza)
        'Controls.Add("CognomeInvio", txtCognomeSpedizione)
        'Controls.Add("NomeInvio", txtNomeSpedizione)
        'Controls.Add("ComuneRCP", txtComuneSpedizione)
        'Controls.Add("CapRCP", txtCAPSpedizione)
        'Controls.Add("ProvinciaRCP", txtProvinciaSpedizione)
        'Controls.Add("ViaRCP", txtIndirizzoSpedizione)
        'Controls.Add("PosizioneCivicoRCP", txtPosizioneSpedizione)
        'Controls.Add("CivicoRCP", txtNumeroCivicoSpedizione)
        'Controls.Add("EsponenteCivicoRCP", txtEsponenteSpedizione)
        'Controls.Add("ScalaCivicoRCP", txtScalaSpedizione)
        'Controls.Add("InternoCivicoRCP", txtInternoSpedizione)
        'Controls.Add("FrazioneRCP", txtFrazioneSpedizione)
        Controls.Add("Note", txtNoteAnagrafica)
        Controls.Add("DaRicontrollare", chkDaRicontrollare)
        Controls.Add("Sesso", cboSesso)
        Controls.Add("Contatti", dgContatti)

        Return Controls

    End Function
#Region "Spedizione"
    '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="27/10/2014">
    ''' <strong>visualizzazione tutti indirizzi spedizione</strong>
    ''' Nuova gestione indirizzi spedizione
    ''' </revision>
    ''' </revisionHistory>
    Protected Sub GrdRowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            Dim MyBtn As ImageButton
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                If CType(e.Row.DataItem, ObjIndirizziSpedizione).ID_DATA_SPEDIZIONE = -1 Then
                    MyBtn = CType(e.Row.FindControl("imgUpd"), ImageButton)
                    MyBtn.CssClass = "SubmitBtn Bottone BottoneNuovo"
                    MyBtn.ToolTip = "Nuovo"
                    e.Row.Cells(5).Enabled = False
                Else
                    MyBtn = CType(e.Row.FindControl("imgUpd"), ImageButton)
                    MyBtn.ImageUrl = "SubmitBtn Bottone BottoneApri"
                    MyBtn.ToolTip = "Modifica"
                    e.Row.Cells(5).Enabled = True
                End If
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FormAnagrafica.GrdRowDataBound.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Dim mySped As New ObjIndirizziSpedizione
        Dim ListSpedizione As New Generic.List(Of ObjIndirizziSpedizione)
        Try
            Dim IDRow As Integer = CInt(e.CommandArgument.ToString())
            If e.CommandName = "RowOpen" Then
                DivIndSped.Style.Add("display", "")
                For Each mySped In Session("IndirizziSpedizione")
                    If mySped.ID_DATA_SPEDIZIONE = IDRow Then
                        hdIdSpedizione.Value = mySped.ID_DATA_SPEDIZIONE.ToString
                        If mySped.CodTributo <> "" Then
                            ddlTributo.SelectedValue = mySped.CodTributo.PadLeft(4, CChar("0"))
                        End If
                        txtCognomeSpedizione.Text = mySped.CognomeInvio
                        txtNomeSpedizione.Text = mySped.NomeInvio
                        txtComuneSpedizione.Text = mySped.ComuneRCP
                        txtCAPSpedizione.Text = mySped.CapRCP
                        txtProvinciaSpedizione.Text = mySped.ProvinciaRCP
                        txtIndirizzoSpedizione.Text = mySped.ViaRCP
                        txtPosizioneSpedizione.Text = mySped.PosizioneCivicoRCP
                        txtNumeroCivicoSpedizione.Text = mySped.CivicoRCP
                        txtEsponenteSpedizione.Text = mySped.EsponenteCivicoRCP
                        txtScalaSpedizione.Text = mySped.ScalaCivicoRCP
                        txtInternoSpedizione.Text = mySped.InternoCivicoRCP
                        txtFrazioneSpedizione.Text = mySped.FrazioneRCP
                        hdCodComuneSpedizione.Value = mySped.CodComuneRCP
                        hdCODViaSpedizione.Value = mySped.CodViaRCP
                    End If
                Next
            ElseIf e.CommandName = "RowDelete" Then
                For Each mySped In Session("IndirizziSpedizione")
                    If mySped.ID_DATA_SPEDIZIONE <> IDRow Then
                        ListSpedizione.Add(mySped)
                    End If
                Next
                Session("IndirizziSpedizione") = ListSpedizione
                GrdInvio.DataSource = ListSpedizione.ToArray
                GrdInvio.DataBind()
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FormAnagrafica.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub GrdInvio_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdInvio.ItemDataBound
    '    Dim ddlMyDati As New DropDownList
    '    Dim MyBtn As ImageButton

    '    Try
    '        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '            If CType(e.Item.DataItem, ObjIndirizziSpedizione).ID_DATA_SPEDIZIONE <= 0 Then
    '                MyBtn = CType(e.Item.FindControl("imgUpd"), ImageButton)
    '                MyBtn.ImageUrl = "..\images\Bottoni\nuovoinserisci.png"
    '                MyBtn.ToolTip = "Nuovo"
    '                e.Item.Cells(5).Enabled = False
    '            Else
    '                MyBtn = CType(e.Item.FindControl("imgUpd"), ImageButton)
    '                MyBtn.ImageUrl = "..\images\Bottoni\apri1.png"
    '                MyBtn.ToolTip = "Modifica"
    '                e.Item.Cells(5).Enabled = True
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FormAnagrafica.GrdInvio_ItemDataBound.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '        strHTMLError = GestError.GetHTMLError(ex, Server.MapPath("/" & Application("nome_sito").tostring & "/ERRORIANAGRAFICA.css"), "parent.Visualizza.location.href='RicercaAnagrafica.aspx?popup=" & popup & "&sessionName=" & ViewState("sessionName").tostring & "';") & vbCrLf
    '        strHTMLError = strHTMLError & "<script>" & vbCrLf
    '        strHTMLError = strHTMLError & "parent.Comandi.location.href='" & "../aspError.aspx" & "';" & vbCrLf
    '        strHTMLError = strHTMLError & "</script>"
    '        Response.Write(strHTMLError)
    '        Response.End()
    '    End Try
    'End Sub

    'Private Sub GrdInvio_UpdateCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GrdInvio.UpdateCommand
    '    Dim mySped As New ObjIndirizziSpedizione
    '    Try
    '        DivIndSped.Style.Add("display", "")
    '        For Each mySped In Session("IndirizziSpedizione")
    '            If mySped.ID_DATA_SPEDIZIONE.ToString = e.Item.Cells(6).Text Then
    '                hdIdSpedizione.Value = mySped.ID_DATA_SPEDIZIONE
    '                If mySped.CodTributo <> "" Then
    '                    ddlTributo.SelectedValue = mySped.CodTributo.PadLeft(4, "0")
    '                End If
    '                txtCognomeSpedizione.Text = mySped.CognomeInvio
    '                txtNomeSpedizione.Text = mySped.NomeInvio
    '                txtComuneSpedizione.Text = mySped.ComuneRCP
    '                txtCAPSpedizione.Text = mySped.CapRCP
    '                txtProvinciaSpedizione.Text = mySped.ProvinciaRCP
    '                txtIndirizzoSpedizione.Text = mySped.ViaRCP
    '                txtPosizioneSpedizione.Text = mySped.PosizioneCivicoRCP
    '                txtNumeroCivicoSpedizione.Text = mySped.CivicoRCP
    '                txtEsponenteSpedizione.Text = mySped.EsponenteCivicoRCP
    '                txtScalaSpedizione.Text = mySped.ScalaCivicoRCP
    '                txtInternoSpedizione.Text = mySped.InternoCivicoRCP
    '                txtFrazioneSpedizione.Text = mySped.FrazioneRCP
    '                hdCodComuneSpedizione.Value = mySped.CodComuneRCP
    '                hdCODViaSpedizione.Value = mySped.CodViaRCP
    '            End If
    '        Next
    '    Catch ex As Exception
    '        Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FormAnagrafica.GrdInvio_UpdateCommand.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '        strHTMLError = GestError.GetHTMLError(ex, Server.MapPath("/" & Application("nome_sito").tostring & "/ERRORIANAGRAFICA.css"), "parent.Visualizza.location.href='RicercaAnagrafica.aspx?popup=" & popup & "&sessionName=" & ViewState("sessionName").tostring & "';") & vbCrLf
    '        strHTMLError = strHTMLError & "<script>" & vbCrLf
    '        strHTMLError = strHTMLError & "parent.Comandi.location.href='" & "../aspError.aspx" & "';" & vbCrLf
    '        strHTMLError = strHTMLError & "</script>"
    '        Response.Write(strHTMLError)
    '        Response.End()
    '    End Try
    'End Sub

    'Private Sub GrdInvio_EditCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GrdInvio.EditCommand
    '    Dim mySped As New ObjIndirizziSpedizione
    '    Dim ListSpedizione As New Generic.List(Of ObjIndirizziSpedizione)
    '    Try
    '        For Each mySped In Session("IndirizziSpedizione")
    '            If mySped.ID_DATA_SPEDIZIONE.ToString <> e.Item.Cells(6).Text Then
    '                ListSpedizione.Add(mySped)
    '            End If
    '        Next
    '        Session("IndirizziSpedizione") = ListSpedizione
    '        GrdInvio.DataSource = ListSpedizione.ToArray
    '        GrdInvio.DataBind()
    '    Catch ex As Exception
    '        Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FormAnagrafica.GrdInvio_EditCommand.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '        strHTMLError = GestError.GetHTMLError(ex, Server.MapPath("/" & Application("nome_sito").tostring & "/ERRORIANAGRAFICA.css"), "parent.Visualizza.location.href='RicercaAnagrafica.aspx?popup=" & popup & "&sessionName=" & ViewState("sessionName").tostring & "';") & vbCrLf
    '        strHTMLError = strHTMLError & "<script>" & vbCrLf
    '        strHTMLError = strHTMLError & "parent.Comandi.location.href='" & "../aspError.aspx" & "';" & vbCrLf
    '        strHTMLError = strHTMLError & "</script>"
    '        Response.Write(strHTMLError)
    '        Response.End()
    '    End Try
    'End Sub
    Private Sub LoadCombos()
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myDataReader As SqlClient.SqlDataReader
        Try

            cmdMyCommand.Connection = New SqlClient.SqlConnection(COSTANTValue.ConstSession.StringConnectionOPENgov)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_GetTributi"
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myDataReader = cmdMyCommand.ExecuteReader
            Try
                ddlTributo.Items.Clear()
                ddlTributo.Items.Add("...")
                ddlTributo.Items(0).Value = "-1"
                If Not myDataReader Is Nothing Then
                    Do While myDataReader.Read
                        If Not IsDBNull(myDataReader(0)) Then
                            ddlTributo.Items.Add(myDataReader(0).ToString)
                            ddlTributo.Items(ddlTributo.Items.Count - 1).Value = myDataReader(1).ToString
                        End If
                    Loop
                End If
            Catch ex As Exception
                Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FormAnagrafica.LoadCombos.errore: ", ex)
                Response.Redirect("../../PaginaErrore.aspx")
            End Try
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FormAnagrafica.LoadCombos.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw New Exception("Problemi nell'esecuzione di LoadComboGenerale " + ex.Message)
        Finally
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
    End Sub
    Private Sub CmdSaveSpedizione_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdSaveSpedizione.Click
        Dim mySped As New ObjIndirizziSpedizione
        Dim ListSpedizione As New Generic.List(Of ObjIndirizziSpedizione)
        Dim bTrovato As Boolean = False

        Try
            'carico i dati della videata
            mySped = LoadSpedFromForm()
            If mySped.ID_DATA_SPEDIZIONE <= 0 Then
                'carico un id fittizio per non sovrascrivermi se inserisco + di 1 indirizzo nuovo
                mySped.ID_DATA_SPEDIZIONE = (Session("IndirizziSpedizione").Count * -100)
            End If
            For Each oSped As ObjIndirizziSpedizione In Session("IndirizziSpedizione")
                If oSped.ID_DATA_SPEDIZIONE = mySped.ID_DATA_SPEDIZIONE Then
                    oSped = mySped
                    bTrovato = True
                End If
                If oSped.CodTributo <> "" Then
                    'il rigo vuoto si aggiunge in coda
                    ListSpedizione.Add(oSped)
                End If
            Next
            If bTrovato = False Then
                ListSpedizione.Add(mySped)
            End If
            ListSpedizione.Add(New ObjIndirizziSpedizione)
            'controllo che non ci sia un doppio tributo
            For Each oSped As ObjIndirizziSpedizione In ListSpedizione
                If oSped.CodTributo.PadLeft(4, CChar("0")) = mySped.CodTributo And oSped.ID_DATA_SPEDIZIONE <> mySped.ID_DATA_SPEDIZIONE Then
                    SvuotaIndSped()
                    Dim strBuild As New System.Text.StringBuilder
                    strBuild.Append("GestAlert('a', 'warning', '', '', 'Indirizzo per Tributo gia\' presente!\nImpossibile inserirlo come nuovo!');")
                    RegisterScript(strBuild.ToString(), Me.GetType())
                    Exit Sub
                End If
            Next
            'ricarico la griglia
            Session("IndirizziSpedizione") = ListSpedizione
            GrdInvio.DataSource = ListSpedizione.ToArray
            GrdInvio.DataBind()
            SvuotaIndSped()
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FormAnagrafica.CmdSaveSpedizione_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    Private Sub CmdRibaltaSpedizione_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdRibaltaSpedizione.Click
        Dim mySped As New ObjIndirizziSpedizione
        Dim ListSpedizione As New Generic.List(Of ObjIndirizziSpedizione)

        Try
            'carico i dati della videata
            mySped = LoadSpedFromForm()
            If mySped.ID_DATA_SPEDIZIONE <= 0 Then
                'carico un id fittizio per non sovrascrivermi se inserisco + di 1 indirizzo nuovo
                mySped.ID_DATA_SPEDIZIONE = (Session("IndirizziSpedizione").Count * -100)
            End If
            ListSpedizione.Add(mySped)
            'carico l'indirizzo per tutti gli altri tributi
            For Each myItem As ListItem In ddlTributo.Items
                If mySped.CodTributo <> myItem.Value And myItem.Value <> "-1" Then
                    Dim oSped As New ObjIndirizziSpedizione
                    oSped = LoadSpedFromForm()
                    oSped.ID_DATA_SPEDIZIONE = (ListSpedizione.Count * -100) + 1
                    oSped.CodTributo = myItem.Value
                    oSped.DescrTributo = myItem.Text
                    ListSpedizione.Add(oSped)
                End If
            Next
            ListSpedizione.Add(New ObjIndirizziSpedizione)
            'controllo che non ci sia un doppio tributo
            For Each oSped As ObjIndirizziSpedizione In ListSpedizione
                If oSped.CodTributo.PadLeft(4, CChar("0")) = mySped.CodTributo And oSped.ID_DATA_SPEDIZIONE <> mySped.ID_DATA_SPEDIZIONE Then
                    SvuotaIndSped()
                    Dim strBuild As New System.Text.StringBuilder
                    strBuild.Append("GestAlert('a', 'warning', '', '', 'Indirizzo per Tributo gia\' presente!\nImpossibile inserirlo come nuovo!');")
                    RegisterScript(strBuild.ToString(), Me.GetType())
                    Exit Sub
                End If
            Next
            'ricarico la griglia
            Session("IndirizziSpedizione") = ListSpedizione
            GrdInvio.DataSource = ListSpedizione.ToArray
            GrdInvio.DataBind()
            SvuotaIndSped()
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FormAnagrafica.CmdRibaltaSpedizione_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    Private Sub CmdUnloadSpedizione_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdUnloadSpedizione.Click
        Try
            SvuotaIndSped()
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FormAnagrafica.CmdUnloadSpedizione_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    Private Function LoadSpedFromForm() As ObjIndirizziSpedizione
        Dim mySped As New ObjIndirizziSpedizione

        Try
            mySped.ID_DATA_SPEDIZIONE = hdIdSpedizione.Value
            mySped.CodTributo = ddlTributo.SelectedValue
            mySped.DescrTributo = ddlTributo.SelectedItem.Text
            mySped.CognomeInvio = txtCognomeSpedizione.Text
            mySped.NomeInvio = txtNomeSpedizione.Text
            mySped.CodComuneRCP = hdCodComuneSpedizione.Value
            mySped.ComuneRCP = txtComuneSpedizione.Text
            mySped.CapRCP = txtCAPSpedizione.Text
            mySped.ProvinciaRCP = txtProvinciaSpedizione.Text
            mySped.CodViaRCP = hdCODViaSpedizione.Value
            mySped.ViaRCP = txtIndirizzoSpedizione.Text
            mySped.PosizioneCivicoRCP = txtPosizioneSpedizione.Text
            mySped.CivicoRCP = txtNumeroCivicoSpedizione.Text
            mySped.EsponenteCivicoRCP = txtEsponenteSpedizione.Text
            mySped.ScalaCivicoRCP = txtScalaSpedizione.Text
            mySped.InternoCivicoRCP = txtInternoSpedizione.Text
            mySped.FrazioneRCP = txtFrazioneSpedizione.Text
            mySped.OperatoreSpedizione = COSTANTValue.ConstSession.UserName
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FormAnagrafica.LoadSpedFromForm.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
        Return mySped
    End Function
    Private Sub SvuotaIndSped()
        Try
            'svuoto le text
            hdCODViaSpedizione.Value = "-1" : hdCodComuneSpedizione.Value = "-1"
            ddlTributo.SelectedValue = "-1"
            txtCognomeSpedizione.Text = "" : txtNomeSpedizione.Text = ""
            txtCAPSpedizione.Text = "" : txtComuneSpedizione.Text = "" : txtProvinciaSpedizione.Text = ""
            txtIndirizzoSpedizione.Text = "" : txtPosizioneSpedizione.Text = "" : txtNumeroCivicoSpedizione.Text = "" : txtEsponenteSpedizione.Text = "" : txtScalaSpedizione.Text = "" : txtInternoSpedizione.Text = ""
            txtFrazioneSpedizione.Text = ""
            DivIndSped.Style.Add("display", "none")
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FormAnagrafica.SvuotaIndSped.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** ***
#End Region
#Region "CF"
    Private Sub btnCodiceFiscaleServer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCodiceFiscaleServer.Click
        Dim ControlloCodiceFiscale As New ControlliCFPI()
        Dim Cognome As String
        Dim Nome As String
        Dim Comune As String
        Dim Sesso As String
        Dim DataNascita As String
        Dim Giorno As String
        Dim Mese As String
        Dim Anno As String

        Cognome = UCase(txtCognome.Text)
        Nome = UCase(txtNome.Text)
        Comune = UCase(txtLuogoNascita.Text & txtProvinciaNascita.Text)
        Sesso = UCase(cboSesso.SelectedItem.Value)

        DataNascita = UCase(txtDataNascita.Text)

        Giorno = Mid(DataNascita, 1, 2)
        Mese = Mid(DataNascita, 4, 2)
        Anno = Mid(DataNascita, 7, 4)

        Session.LCID = 1040

        Dim dtDataNascita As DateTime = New DateTime(Anno, Mese, Giorno)

        DataNascita = dtDataNascita.ToString

        Try
            txtCodiceFiscale.Text = ControlloCodiceFiscale.Calcolo_Codice_Fiscale(Cognome, Nome, DataNascita, Sesso, Comune, "", COSTANTValue.ConstSession.DBType, COSTANTValue.ConstSession.StringConnectionAnagrafica)
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FormAnagrafica.btnCodiceFiscaleServer_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    Private Sub btnDaCodiceFiscaleServer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDaCodiceFiscaleServer.Click
        Try
            Dim ControlloCodiceFiscale As New ControlliCFPI()

            Dim CodiceFiscale As String
            Dim Provincia As String
            Dim Identificativo As String

            CodiceFiscale = UCase(txtCodiceFiscale.Text)
            txtDataNascita.Text = ControlloCodiceFiscale.Data_Nascita_da_CodFiscale(CodiceFiscale, False, "")
            txtLuogoNascita.Text = ControlloCodiceFiscale.Luogo_Nascita_da_CodFiscale(CodiceFiscale, False, "", Identificativo, Provincia, COSTANTValue.ConstSession.DBType, COSTANTValue.ConstSession.StringConnectionAnagrafica)
            txtProvinciaNascita.Text = Provincia
            hdCodComuneNascita.Value = Identificativo
            FncAnater.SelectIndexDropDownList(cboSesso, ControlloCodiceFiscale.Sesso_da_CodFiscale(CodiceFiscale, False, ""))
        Catch EX As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FormAnagrafica.btnDaCodiceFiscaleServer_Click.errore: ", EX)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
#End Region
#Region "Gestione Contatti e griglia Contatti"
    '*** 20140515 - invio mail ***
    Public Sub dgContatti_OnItemDataBound(ByVal sender As Object, ByVal e As DataGridItemEventArgs)
        Dim lblSID As Label
        Dim lblDescrizione As Label
        Dim lblIDRIFERIMENTO As Label
        Dim lblDataInizioInvio As Label
        Dim strArgumentsID As String
        Dim strArgumentsDESC As String
        Dim strArgumentsIDRIFERIMENTO As String
        Dim strArgumentsDataInizioInvio As String
        Try
            Select Case e.Item.ItemType
                Case ListItemType.Item
                    lblSID = CType(e.Item.FindControl("lblTipoRiferimento"), Label)
                    lblIDRIFERIMENTO = CType(e.Item.FindControl("lblIDRIFERIMENTO"), Label)
                    lblDescrizione = CType(e.Item.FindControl("DatiRiferimento"), Label)
                    lblDataInizioInvio = CType(e.Item.FindControl("DataInizioInvio"), Label)

                    strArgumentsID = "'" & lblSID.Text & "'"
                    strArgumentsDESC = "'" & lblDescrizione.Text & "'"
                    strArgumentsIDRIFERIMENTO = "'" & lblIDRIFERIMENTO.Text & "'"
                    strArgumentsDataInizioInvio = "'" & lblDataInizioInvio.Text.Replace("Data validità invio: ", "") & "'"

                    e.Item.Attributes.Add("OnClick", "ModificaContatti(" & strArgumentsID & "," & strArgumentsDESC & "," & strArgumentsIDRIFERIMENTO & "," & strArgumentsDataInizioInvio & ");")
                Case ListItemType.AlternatingItem
                    lblSID = CType(e.Item.FindControl("lblTipoRiferimento"), Label)
                    lblIDRIFERIMENTO = CType(e.Item.FindControl("lblIDRIFERIMENTO"), Label)
                    lblDescrizione = CType(e.Item.FindControl("DatiRiferimento"), Label)
                    lblDataInizioInvio = CType(e.Item.FindControl("DataInizioInvio"), Label)

                    strArgumentsID = "'" & lblSID.Text & "'"
                    strArgumentsDESC = "'" & lblDescrizione.Text & "'"
                    strArgumentsIDRIFERIMENTO = "'" & lblIDRIFERIMENTO.Text & "'"
                    strArgumentsDataInizioInvio = "'" & lblDataInizioInvio.Text.Replace("Data validità invio: ", "") & "'"

                    e.Item.Attributes.Add("OnClick", "ModificaContatti(" & strArgumentsID & "," & strArgumentsDESC & "," & strArgumentsIDRIFERIMENTO & "," & strArgumentsDataInizioInvio & ");")
            End Select

            If e.Item.ItemType = ListItemType.Item Then
                e.Item.Attributes.Add("bgcolor", "White")
            End If

            If e.Item.ItemType = ListItemType.AlternatingItem Then
                e.Item.Attributes.Add("bgcolor", "WhiteSmoke")
            End If

            e.Item.Attributes.Add("onmouseover", "this.className='riga_tabella_mouse_over_Normal'")
            e.Item.Attributes.Add("onmouseout", "this.className='riga_tabella_Normal'")
        Catch EX As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FormAnagrafica.dgContatti_OnItemDataBound.errore: ", EX)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** ***
    Public Sub dgContatti_OnDeleteCommand(ByVal sender As Object, ByVal e As DataGridCommandEventArgs)
        Try
            Dim dataSet As DataSet = Session("DataSetContatti")
            Dim deletekey As String = dgContatti.DataKeys(CInt(e.Item.ItemIndex)).ToString
            Dim DBTable As DataTable = dataSet.Tables("CONTATTI")
            Dim DBRow As DataRow
            For Each DBRow In DBTable.Rows
                If DBRow("IDRIFERIMENTO").ToString = deletekey Then
                    DBRow.Delete()
                    Exit For
                End If
            Next

            'Accetta i cambiamenti
            dataSet.AcceptChanges()
            Session("DataSetContatti") = dataSet
            cboTipoContatto.SelectedIndex = -1
            txtDatiRiferimento.Text = ""
            dgContatti.EditItemIndex = -1

            dgContatti.DataKeyField = "IDRIFERIMENTO"

            dgContatti.DataSource = dataSet.Tables(0).DefaultView
            dgContatti.DataBind()

        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FormAnagrafica.dgContatti_OnDeleteCommand.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    Private Sub btnConfermaContatti_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfermaContatti.Click
        Try
            Dim hdIDRIFERIMENTO As Int32 = CInt(Request.Params("hdIDRIFERIMENTO"))
            Dim oAnagrafica As New GestioneAnagrafica()

            'Inserimento elementi nel Data Set 
            'Inserire il TipoRiferimento ed il Dato Riferimento

            Dim ds As DataSet = oAnagrafica.SetContatti(Session("DataSetContatti"), cboTipoContatto.SelectedIndex, txtDatiRiferimento.Text, txtDataInizioInvio.Text, hdIDRIFERIMENTO, "")
            'Dim Costant As New ANAGRAFICAWEB.Costanti
            lblInfo.Text = ""
            Session("DataSetContatti") = ds
            If hdIDRIFERIMENTO <> Utility.Costanti.INIT_VALUE_NUMBER Then
                Dim strBuilder As New System.Text.StringBuilder
                strBuilder.Append("document.frmAnagrafica.hdIDRIFERIMENTO.value='" & Utility.Costanti.INIT_VALUE_NUMBER & "';")
                RegisterScript(strBuilder.ToString(), Me.GetType())
            End If
            cboTipoContatto.SelectedIndex = -1
            txtDatiRiferimento.Text = ""
            chkInvioInformativeViaMail.Checked = False : txtDataInizioInvio.Text = ""

            dgContatti.DataKeyField = "IDRIFERIMENTO"
            dgContatti.DataSource = ds.Tables(0).DefaultView
            dgContatti.DataBind()
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FormAnagrafica.btnConfermaContatti_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    Private Sub btnEliminaContatti_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEliminaContatti.Click
        'Dim hdIDDATAANAGRAFICA As String = Request.Params("hdIDDATAANAGRAFICA")
        'Dim hdIDDATASPEDIZIONE As String = Request.Params("hdIDDATASPEDIZIONE")
        'Dim hdCOD_TRIBUTO As String = Request.Params("hdCOD_TRIBUTO")
        'Dim hdCOD_CONTRIBUENTE As String = Request.Params("hdCOD_CONTRIBUENTE")
        'Dim hdCodComuneResidenza As String = Request.Params("hdCodComuneResidenza")
        'Dim hdCodComuneSpedizione As String = Request.Params("hdCodComuneSpedizione")
        'Dim hdCodComuneNascita As String = Request.Params("hdCodComuneNascita")
        'Dim hdCODRappresentanteLegale As String = Request.Params("hdCODRappresentanteLegale")
        'Dim hdCODViaResidenza As String = Request.Params("hdCODViaResidenza")
        'Dim hdCODViaSpedizione As String = Request.Params("hdCODViaSpedizione")

        Try
            'Dim strPostForm = New System.Text.StringBuilder
            'strPostForm.Append("<script language='javascript'>")
            'strPostForm.Append("document.frmAnagrafica.hdCOD_TRIBUTO.value='" & hdCOD_TRIBUTO & "';")
            'strPostForm.Append("document.frmAnagrafica.hdCOD_CONTRIBUENTE.value='" & hdCOD_CONTRIBUENTE & "';")
            'strPostForm.Append("document.frmAnagrafica.hdIDDATAANAGRAFICA.value='" & hdIDDATAANAGRAFICA & "';")
            'strPostForm.Append("document.frmAnagrafica.hdIDDATASPEDIZIONE.value='" & hdIDDATASPEDIZIONE & "';")
            'strPostForm.Append("document.frmAnagrafica.hdCodComuneNascita.value='" & hdCodComuneNascita & "';")
            'strPostForm.Append("document.frmAnagrafica.hdCODRappresentanteLegale.value='" & hdCODRappresentanteLegale & "';")
            'strPostForm.Append("document.frmAnagrafica.hdCodComuneResidenza.value='" & hdCodComuneResidenza & "';")
            'strPostForm.Append("document.frmAnagrafica.hdCODViaResidenza.value='" & hdCODViaResidenza & "';")
            'strPostForm.Append("document.frmAnagrafica.hdCodComuneSpedizione.value='" & hdCodComuneSpedizione & "';")
            'strPostForm.Append("document.frmAnagrafica.hdCODViaSpedizione.value='" & hdCODViaSpedizione & "';")
            'strPostForm.Append("</script>")
            'RegisterScript("LoadHidden", strPostForm.ToString())

            Dim hdIDRIFERIMENTO As Int32 = CInt(Request.Params("hdIDRIFERIMENTO"))
            Dim oAnagrafica As New GestioneAnagrafica()
            'Inserimento elementi nel Data Set 
            'Inserire il TipoRiferimento ed il Dato Riferimento
            '*************************************************************************************************************
            '*************************************************************************************************************
            '================================================================================
            'WorkFlow
            '================================================================================
            Dim ds As DataSet = oAnagrafica.DeleteContatti(Session("DataSetContatti"), hdIDRIFERIMENTO)
            '*************************************************************************************************************
            '*************************************************************************************************************
            'Dim Costant As New ANAGRAFICAWEB.Costanti
            lblInfo.Text = ""
            Session("DataSetContatti") = ds
            If hdIDRIFERIMENTO <> Utility.Costanti.INIT_VALUE_NUMBER Then
                Dim strBuilder As New System.Text.StringBuilder
                strBuilder.Append("document.frmAnagrafica.hdIDRIFERIMENTO.value='" & Utility.Costanti.INIT_VALUE_NUMBER & "';")
                RegisterScript(strBuilder.ToString(), Me.GetType())
            End If
            cboTipoContatto.SelectedIndex = -1
            txtDatiRiferimento.Text = ""
            chkInvioInformativeViaMail.Checked = False : txtDataInizioInvio.Text = ""
            '***************************************************************************************************
            '***************************************************************************************************
            dgContatti.DataKeyField = "IDRIFERIMENTO"
            dgContatti.DataSource = ds.Tables(0).DefaultView
            dgContatti.DataBind()
            '***************************************************************************************************
            '***************************************************************************************************
        Catch EX As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FormAnagrafica.btnEliminaContatti_Click.errore: ", EX)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
#End Region




    '    Public Sub New()
    '    End Sub

    '    Private Sub btnComRes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnComRes.Click
    '        ' recupero i dati relativi al comune selezionato, e se c'è lo stradario faccio in modo che l'indirizzo venga scelto da quello
    '        Dim objEnte As New OggettiComuniStrade.OggettoEnte
    '        Dim ArrObjEnte As OggettiComuniStrade.OggettoEnte()

    '        objEnte.Cap = txtCAPResidenza.Text
    '        objEnte.CodBelfiore = ""
    '        objEnte.CodCNC = ""
    '        objEnte.CodIstat = ""
    '        objEnte.Denominazione = txtComuneResidenza.Text
    '        objEnte.Provincia = txtProvinciaResidenza.Text
    '        objEnte.Stradario = False

    '        ArrObjEnte = New WsStradario.Stradario().GetEnti(objEnte)
    ' Try
    '        If ArrObjEnte.Length = 1 Then
    '            txtCAPResidenza.Text = ArrObjEnte(0).Cap
    '            txtComuneResidenza.Text = ArrObjEnte(0).Denominazione
    '            txtProvinciaResidenza.Text = ArrObjEnte(0).Provincia
    '        Else
    '            ' se c'è più di un risultato apro il popup di ricerca dell'ente
    '            Dim strjs As String = String.Empty
    '            strjs += "<script language=""javascript"" type=""text/javascript"">"
    '            'ApriComuni(FunzioneJSRitorno,CodBelfiore,Cap,CodCNC,CodIstat,Denominazione,Provincia)
    '            strjs += "ApriComuni('PopolaComResidenza','" & objEnte.CodBelfiore & "','" & objEnte.Cap & "','" & objEnte.CodCNC & "','" & objEnte.CodIstat & "','" & objEnte.Denominazione & "','" & objEnte.Provincia & "');"
    '            strjs += "</script>"
    '            RegisterScript(Me.GetType(),"strjs", strjs)
    '        End If
    '  Catch EX As Exception
    '      Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FormAnagrafica.btnComRes_Click.errore: ", EX)
    '    Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    '    End Sub

End Class
'End Namespace
