Imports System
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports AnagInterface
Imports RIBESElaborazioneDocumentiInterface
Imports log4net
'Imports OggettiComuniStrade
'Imports WsStradario
Imports RemotingInterfaceAnater
Imports System.Web
Imports System.Web.SessionState
Imports OPENgov_AgenziaEntrate
Imports OPENUtility
Imports log4net.Config
Imports System.IO
Imports Utility

Partial Class DatiGenerali
    Inherits BasePage
    'Public ContatoreID As Integer
    Protected UrlStradario As String = ConstSession.UrlStradario
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(DatiGenerali))
    Protected FncGrd As New ClsGenerale.FunctionGrd
    Private iDB As New DBAccess.getDBobject
    Private _Const As New Costanti
    'Private _Err As New Errori
    Private FncGen As New Generali

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub


    'Protected WithEvents grdContatori As RibesDataGrid.RibesDataGrid.RibesDataGrid
    'Protected WithEvents RibesDataGrid As RibesDataGrid.RibesDataGrid.RibesDataGrid




    'Protected WithEvents hdCodAnagrafeUtente As System.Web.UI.HtmlControls.HtmlInputHidden

    Protected WithEvents lblMessage As System.Web.UI.WebControls.Label

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    ''' <summary>
    ''' l'evento  Page_Load di questa pagina
    ''' ricava il dettaglio del Contatore selezionato
    ''' L'estrazione dati avviene nella classe  DEContatori.vb
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim codiceVia As Integer
        Dim ubicazioneVia As String
        Dim myContatoreID As Integer

        Try
            Log.Debug("Entrato in Page_Load dati generali contatore")
            If TxtDataAttivazioneRibaltata.Text <> "" Then
                txtDataAttivazione.Text = TxtDataAttivazioneRibaltata.Text
            End If
            If TxtDataSostituzioneRibaltata.Text <> "" Then
                txtDataSostituzione.Text = TxtDataSostituzioneRibaltata.Text
            End If

            Session("precedentecatasto") = "Contatori"

            '*** Fabi ****
            '*** Aggancio ricerca anagrafica anater ***
            LnkAnagAnater.Attributes.Add("onclick", "ApriRicAnater();")
            LnkAnagrAnatUtente.Attributes.Add("onclick", "ApriRicAnater();")
            '**** Aggancio per stradario ****
            LnkPulisciStrada.Attributes.Add("onclick", "return ClearDatiVia();")
            LnkOpenStradario.Attributes.Add("onclick", "return ApriStradario('RibaltaStrada', '" & ConstSession.IdEnte & "')")
            '**** /Fabi
            'assegno i metodi per inserire i dati catastali
            LnkNewUIAnater.Attributes.Add("onclick", "ShowRicUIAnater()")
            LnkNewUIManuale.Attributes.Add("onclick", "ApriNuovo(" & CInt(Request.Params("IDCONTATORE")) & ")")

            Log.Debug("Chiamata a stradario dati generali contatore")

            If Page.IsPostBack = False Then
                myContatoreID = CInt(Request.Params("IDCONTATORE"))
                Log.Debug("Controllo.Load->ContatoreID x txtidContatore da Request=" + myContatoreID.ToString)
                If myContatoreID > 0 Then
                    txtidContatore.Text = myContatoreID
                Else
                    txtidContatore.Text = "0"
            End If
                'carico i dati catastali
                Dim sScript As String = ""
                sScript += "document.getElementById('loadGrid').src='searchResultsCatasto.aspx?ContatoreID=" & txtidContatore.Text & "';"
                RegisterScript(sScript, Me.GetType())

            HDtxtCodIntestatario.Text = -1
            HDTextCodUtente.Text = -1

                '*** controllo se ho una variazione in corso per il contatore ***
                Log.Debug("controllo se ho una variazione in corso per il contatore")
                Dim FncVar As New ClsRibaltaVar
                Dim bIsInVar As Boolean
                If Not IsNothing(FncVar.GetRicercaVariazioni(ConstSession.IdEnte, ClsRibaltaVar.sTypeRicerca.VARFATT_DAGESTIRE, CInt(txtidContatore.Text), -1)) Then
                    bIsInVar = True
                Else
                    bIsInVar = False
                End If
                '*** ***

                Log.Debug("carica comandi dati generali contatore")
                LoadComandi(bIsInVar)
                Log.Debug("Prima di recuparo dati dal db dati generali contatore")
                'prelevo i dati del contatore
                Dim DBContatori As GestContatori = New GestContatori
                Dim myContatore As objContatore = DBContatori.GetDetailsContatori(CInt(txtidContatore.Text), -1) ', ConstSession.IdEnte, ConstSession.CodIstat)
                Session("myContatore") = myContatore
                '*** Fabi 05032008
                If Not Page.IsPostBack Then
                    If txtidContatore.Text <> "" Then
                        codiceVia = myContatore.nIdVia
                        ubicazioneVia = myContatore.sUbicazione
                    Else
                        codiceVia = TxtCodVia.Text
                        ubicazioneVia = TxtVia.Text
                    End If
                Else
                    codiceVia = TxtCodVia.Text
                    ubicazioneVia = TxtVia.Text
                End If
                TxtVia.Text = ubicazioneVia
                TxtCodVia.Text = codiceVia
                '*** /Fabi

                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''//
                LoadHidden(myContatore)
                '========================================================================================
                'Gestione parametri di ricerca
                If txtidContatore.Text <> "" Then
                    lblError.Text = "Si può procedere con l'inserimento o la modifica delle letture"
                End If

                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''//

                '***** GESTIONE DATI AGENZIA ENTRATE ******
                LoadDatiAE()
                '**** FINE CARICAMENTO DATI PER AGENZIA ENTRATE ****    
                LoadCombos(myContatore)
                If txtidContatore.Text <> "" Then
                    LoadAnagrafica(myContatore)
                End If
                LoadDati(myContatore)
                '=======================================
                'controllo se questo contatore è già legato ad un contratto: se si,
                'non è possibile modificare le anagrafiche di intestatario e utente, 
                'nonchè la tipologia dell'intestatario
                '=======================================
                Dim dvMyDati As New DataView
                Dim sSQL As String
                sSQL = "SELECT CODCONTRATTO FROM TP_CONTATORI"
                sSQL += " WHERE CODCONTATORE=" & txtidContatore.Text
                dvMyDati = iDB.GetDataView(sSQL)
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        If Not IsDBNull(myRow("CODCONTRATTO")) Then
                            btnApriUtente.Visible = False
                            btnApriIntestatario.Visible = False
                            'pul1.Visible = False
                            'pul2.Visible = False

                            '**** Nascondo i link per la ricerca anagrafica in Anater ***
                            '*** Fabi
                            LnkAnagAnater.Visible = False
                            LnkAnagrAnatUtente.Visible = False
                            '*** /Fabi
                        End If
                    Next
                End If
                dvMyDati.Dispose()

                If Not Page.IsPostBack And txtContatoreSuccessivo.Text <> "" And txtContatoreSuccessivo.Text <> "-1" And txtContatoreSuccessivo.Text <> "0" Then
                    txtSostituito.Text = "SOSTITUITO"
                End If

                If Not IsNothing(Session("oListSubContatori")) Then
                    Dim oListSubContatori() As ObjSubContatore
                    oListSubContatori = CType(Session("oListSubContatori"), ObjSubContatore())
                    GrdSubContatori.DataSource = oListSubContatori
                    GrdSubContatori.DataBind()
                End If

                If Request.Item("sProvenienza") = "AE" Or Request.Item("sProvenienza") = "SC" Then
                    LockPage()
                End If

                '*** prelevo il numero utente ***
                Dim FncMy As New GestContratti
                Dim mionumero As Int32

                If txtNumeroUtente.Text <> "" Then
                    mionumero = txtNumeroUtente.Text
                Else
                    mionumero = 0
                    mionumero = FncMy.GetNumeroUtente(ConstSession.IdEnte, mionumero)
                    If mionumero > 0 Then
                        txtNumeroUtente.Text = mionumero
                    End If
                End If
                '*** ***
                Log.Debug("Fine Page_Load dati generali contatore")
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DatiGenerali.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
    Private Sub btnEvento_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEvento.Click
        Dim sScript As String = ""
        Dim ModDate As New ClsGenerale.Generale
        Dim FncContatori As New GestContatori

        Try
            If txtSostituito.Text = "SOSTITUITO" Then
                sScript += "GestAlert('a', 'warning', '', '', 'Non e\' possibile modificare un contratto gia\' sostituito o cessato');"
                RegisterScript(sScript, Me.GetType)
                Exit Sub
            End If

            Log.Debug("Controllo.btnEvento->txtidcontatore=" + txtidContatore.Text)
            'non serve più questo controllo
            'If FncContatori.VerificaMatricola(txtMatricola.Text, ContatoreID, ConstSession.CodIstat) Then
            '    sScript = "if (confirm('Attenzione, Matricola Esistente!\nSi desidera continuare?')){ document.getElementById('btnSalvaContatore').click() }"
            '    RegisterScript(sScript, Me.GetType())
            '    lblError.Text = "Attenzione: " & sScript
            '    Exit Sub
            'End If
            If CInt(txtidContatore.Text) <= 0 Then
                If FncContatori.VerificaNumeroUtente(txtNumeroUtente.Text, CInt(txtidContatore.Text), ConstSession.CodIstat) Then
                    sScript = sScript & "  " & "Numero Utente Esistente"
                End If
            End If
            If FncContatori.VerificaSequenza(cboGiro.SelectedIndex.ToString, txtSequenza.Text, CInt(txtidContatore.Text), ConstSession.CodIstat) Then
                sScript = sScript & "  " & "Sequenza Esistente"
            End If
            Select Case Len(txtDataAttivazione.Text)
                Case Is > 0
                    If Len(txtDataCessazione.Text) > 0 Then
                        If ModDate.GiraData(txtDataCessazione.Text) < ModDate.GiraData(txtDataAttivazione.Text) Then
                            sScript = sScript & " " & "Data Cessazione Minore di Data Attivazione"
                        End If
                    End If
                    If Len(txtDataRimTemp.Text) > 0 Then
                        If ModDate.GiraData(txtDataRimTemp.Text) < ModDate.GiraData(txtDataAttivazione.Text) Then
                            sScript = sScript & " " & "Data Rimozione Temporanea Minore di Data Attivazione"
                        End If
                    End If
                    If Len(txtDataSostituzione.Text) > 0 Then
                        If ModDate.GiraData(txtDataSostituzione.Text) < ModDate.GiraData(txtDataAttivazione.Text) Then
                            sScript = sScript & " " & "Data Sostituzione Minore di Data Attivazione"
                        End If
                    End If
            End Select
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/

            If Len(sScript) > 0 Then
                RegisterScript("GestAlert('a', 'warning', '', '', 'Attenzione: " & sScript & "');", Me.GetType())
                lblError.Text = "Attenzione: " & sScript
                Exit Sub
            Else
                sScript = "document.getElementById('btnSalvaContatore').click()"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DatiGenerali.btnEvento_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub btnSalvaContatore_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalvaContatore.Click
        Try
            Dim FncContatori As New GestContatori
            Dim oDatiContatore As New objContatore
            Dim sScript As String = ""

            oDatiContatore = SetDatiContatore()
            If oDatiContatore Is Nothing Then
                sScript += "GestAlert('a', 'danger', '', '', 'Errore durante il Salvataggio!');"
                RegisterScript(sScript, Me.GetType())
                Exit Sub
            End If
            Log.Debug("Controllo.btnSalvaContatore->txtidContatore.Text=" + txtidContatore.Text)
            If FncContatori.SetDatiContatore(txtidContatore.Text, oDatiContatore, False) = False Then
                sScript += "GestAlert('a', 'danger', '', '', 'Errore durante il Salvataggio!');"
                RegisterScript(sScript, Me.GetType())
                Exit Sub
            End If           'FncContatori.SetContatore(utility.stringoperation.formatstring(ContatoreID), oDatiContatore)
            Try
                If Not IsNothing(Session("datacatasto")) Then
                    'Dim DtCatasto As System.Data.DataTable
                    'DtCatasto = CType(Session("datacatasto"), System.Data.DataTable)
                    'FncContatori.SetCatastaliDatatable(DtCatasto, oDatiContatore.nIdContatore)
                    Dim IDTipoParticella As String
                    For Each myCatRow As DataRow In CType(Session("datacatasto"), System.Data.DataTable).Rows
                        If IsDBNull(myCatRow("ID_TIPO_PARTICELLA")) Then
                            IDTipoParticella = ""
                        Else
                            IDTipoParticella = New GestContatori().GetIdTipoParticella(myCatRow("ID_TIPO_PARTICELLA"))
                        End If
                        Log.Debug("Controllo.btnSalvaContatore.Catasto->oDatiContatore.nIdContatore=" + oDatiContatore.nIdContatore.ToString)
                        If New GestContatori().SetDatiCatastali(myCatRow("INTERNO"), myCatRow("PIANO"), myCatRow("FOGLIO"), myCatRow("NUMERO"), myCatRow("SUBALTERNO"), -1, oDatiContatore.nIdContatore, myCatRow("SEZIONE"), myCatRow("ESTENSIONE_PARTICELLA"), IDTipoParticella) <= 0 Then
                            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DatiGenerali.btnSalvaContatore_Click.errore: errore in inserimento dati catastali")
                        End If
                    Next
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DatiGenerali.btnSalvaContatore_Click.DatiCatasto.errore: ", ex)
            End Try
            Session("datacatasto") = Nothing

            Dim dsDatiContatore As DataSet
            Log.Debug("Controllo.btnSalvaContatore->oDatiContatore.nIdContatore=" + oDatiContatore.nIdContatore.ToString)
            dsDatiContatore = FncContatori.GetDataTableContatoreAnater(oDatiContatore.nIdContatore)

            '==========================================
            'INSERIMENTO PRIMA LETTURA
            '==========================================
            Try
                If Not IsDBNull(oDatiContatore.sDataAttivazione) Then
                    'se la data di attivazione non è Null
                    If oDatiContatore.sDataAttivazione <> "" Then
                        'ed è diversa da una stringa vuota
                        Dim dvMyDati As New DataView
                        dvMyDati = iDB.GetDataView("SELECT COUNT(*) AS CONTEGGIO FROM TP_LETTURE WHERE CODCONTATORE=" & oDatiContatore.nIdContatore)
                        Dim totale As Int16 = -1
                        If Not dvMyDati Is Nothing Then
                            For Each myRow As DataRowView In dvMyDati
                                totale = myRow("conteggio")
                            Next
                        End If
                        dvMyDati.Dispose()
                        If totale <= 0 Then
                            'inserisco la prima lettura uguale a 0 per questo contatore
                            'Dim sqlConn As New SqlConnection
                            'sqlConn.ConnectionString = ConstSession.StringConnection
                            'sqlConn.Open()
                            FncContatori.SetPrimaLettura(oDatiContatore.nIdContatore, ConstSession.IdPeriodo, oDatiContatore.sDataAttivazione, oDatiContatore.nIdContatorePrec, oDatiContatore.nIdUtente, oDatiContatore.sNumeroUtente, 0)
                        End If
                    End If
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DatiGenerali.btnSalvaContatore_Click.InserimentoPrimaLettura.errore: ", ex)
            End Try
            '=======================
            '/PRIMA LETTURA
            '=======================

            '=========================================================
            'SE MODIFICO DATA ATTIVAZIONE, MODIFICO ANCHE PRIMA LETTURA
            '=========================================================
            If CStr(UpdPrimaLettura.Text.ToUpper()) = "UPDATE" Then
                Log.Debug("Controllo.btnSalvaContatore.PrimaLettura->txtidContatore.Text=" + txtidContatore.Text)
                If txtidContatore.Text <> "" Then
                    FncContatori.UpdatePrimaLettura(CInt(txtidContatore.Text), CStr(txtDataAttivazione.Text))
                End If
            End If
            '=========================================================
            'FINE
            '=========================================================

            If txtDataSostituzione.Text <> "" Then
                Log.Debug("Controllo.btnSalvaContatore.sostituzione->ContatoreID per goContatoreSostituito=" + oDatiContatore.nIdContatore.ToString)
                goContatoreSostituito(oDatiContatore.nIdContatore)
            End If
            If oDatiContatore.nIdContatore > 0 Then
                Log.Debug("Controllo.btnSalvaContatore->txtidcontatore prima=" + txtidContatore.Text)
                txtidContatore.Text = oDatiContatore.nIdContatore
                Log.Debug("Controllo.btnSalvaContatore->txtidcontatoreda oDatiContatore.nIdContatore=" + txtidContatore.Text)
            End If

            '*** ribaltamento in Anater se abilitato ***
            Try
                If System.Configuration.ConfigurationManager.AppSettings("USO_ANATER").ToString() = True Then
                    '***AGGANCIO SERVIZIO RIBALTAMENTO IN ANATER ***
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

                    ''RICAVO IL DT DEI DATI CATASTALI
                    'Dim WFSessione As OPENUtility.CreateSessione
                    'Dim WFErrore As String

                    ''inizializzo la connessione
                    'WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
                    'If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
                    '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
                    'End If
                    Dim DSCatasto As DataSet

                    DSCatasto = iDB.RunSPReturnDataSet("prc_GetDatiCatastali", "", New SqlParameter("@Id", -1), New SqlParameter("@IdContatore", CInt(dsDatiContatore.Tables(0).Rows(0)("CODCONTATORE").ToString()))) 'FncRif.getListaCatastali(-1, CInt(dsDatiContatore.Tables(0).Rows(0)("CODCONTATORE").ToString())) , WFSessione)
                    objContatoreAnater = clsTraduci.TraduciContatoreAnater(dsDatiContatore, DSCatasto, codiceFiscale)
                    remObject.RibaltaInAnaterH2O(objContatoreAnater, oAnagraficaAnater, ConstSession.IdEnte, False, False, 0, nomeFile)
                    'WFSessione.Kill()
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DatiGenerali.btnSalvaContatore_Click.Antater.errore: ", ex)
            End Try

            sScript += "GestAlert('a', 'success', '', '', 'Salvataggio avvenuto correttamente.');"
            If Request.Item("sProvenienza") <> "AE" Then
                sScript += "parent.Comandi.location.href='ComandiRicercaContatori.aspx';"
                sScript += "location.href='RicercaContatori.aspx';"
            Else
                sScript += "location.href='../AgenziaEntrate/DatiMancanti/RicercaDatiMancanti.aspx';"
            End If
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DatiGenerali.btnSalvaContatore_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    Private Function SetDatiContatore() As objContatore
        Dim myItem As New objContatore
        Dim NewCodContatore As String = ""
        Try
            If Not IsNothing(Session("oListSubContatori")) Then
                myItem.oListSubContatori = Session("oListSubContatori")
            End If
            If txtspesaprev.Text <> 0 Then
                myItem.nSpesa = txtspesaprev.Text
            End If
            If txtdirittisegr.Text <> 0 Then
                myItem.nDiritti = txtdirittisegr.Text
            End If
            If txtDataAttivazione.Text <> "" Then
                myItem.bIsPendente = 0
            End If
            If txtMatricola.Text <> "" Then
                myItem.sMatricola = txtMatricola.Text
            End If
            If ConstSession.IdEnte <> 0 Or ConstSession.IdEnte <> -1 Then
                myItem.sIdEnte = CInt(ConstSession.IdEnte)
                myItem.sIdEnteAppartenenza = myItem.sIdEnte
            End If
            If cboImpianto.SelectedItem.Value <> 0 Or cboImpianto.SelectedItem.Value <> -1 Then
                myItem.nIdImpianto = cboImpianto.SelectedItem.Value
            End If
            If cboGiro.SelectedItem.Value <> 0 Or cboGiro.SelectedItem.Value <> -1 Then
                myItem.nGiro = cboGiro.SelectedItem.Value
            End If
            If txtSequenza.Text <> "" Then
                myItem.sSequenza = txtSequenza.Text
            End If
            If cboPosizione.SelectedItem.Value <> 0 Or cboPosizione.SelectedItem.Value <> -1 Then
                myItem.nPosizione = cboPosizione.SelectedItem.Value
            End If
            If txtProgressivo.Text <> "" Then
                myItem.sProgressivo = txtProgressivo.Text
            End If
            If txtLatoStrada.Text <> "" Then
                myItem.sLatoStrada = txtLatoStrada.Text
            End If
            If txtNumeroUtente.Text <> "" Then
                myItem.sNumeroUtente = txtNumeroUtente.Text
            End If
            If cboTipoContatore.SelectedItem.Value <> 0 Or cboTipoContatore.SelectedItem.Value <> -1 Then
                myItem.nTipoContatore = cboTipoContatore.SelectedItem.Value
            End If
            If txtContatorePrecedente.Text <> "" Then
                myItem.nIdContatorePrec = txtContatorePrecedente.Text
            End If
            If txtContatoreSuccessivo.Text <> "" Then
                myItem.nIdContatoreSucc = txtContatoreSuccessivo.Text
            End If
            If cboFognatura.SelectedItem.Value <> 0 Or cboFognatura.SelectedItem.Value <> -1 Then
                myItem.nCodFognatura = cboFognatura.SelectedItem.Value
            End If
            If cboDepurazione.SelectedItem.Value <> 0 Or cboDepurazione.SelectedItem.Value <> -1 Then
                myItem.nCodDepurazione = cboDepurazione.SelectedItem.Value
            End If
            If chkEsenteFognatura.Checked = True Then
                myItem.bEsenteFognatura = 1
            End If
            If chkEsenteDepurazione.Checked = True Then
                myItem.bEsenteDepurazione = 1
            End If
            If chkEsenteAcqua.Checked = True Then
                myItem.bEsenteAcqua = 1
            End If
            '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
            If chkEsenteAcquaQF.Checked = True Then
                myItem.bEsenteAcquaQF = 1
            End If
            If chkEsenteDepQF.Checked = True Then
                myItem.bEsenteDepQF = 1
            End If
            If chkEsenteFogQF.Checked = True Then
                myItem.bEsenteFogQF = 1
            End If
            If chkIgnoraMora.Checked = True Then
                myItem.bIgnoraMora = 1
            End If
            If txtNote.Text <> "" Then
                myItem.sNote = txtNote.Text
            End If
            If txtDataAttivazione.Text <> "" Then
                myItem.sDataAttivazione = txtDataAttivazione.Text
            End If
            If txtDataRimTemp.Text <> "" Then
                myItem.sDataRimTemp = txtDataRimTemp.Text
            End If
            If txtDataCessazione.Text <> "" Then
                myItem.sDataCessazione = txtDataCessazione.Text
            End If
            If txtDataSostituzione.Text <> "" Then
                myItem.sDataSostituzione = txtDataSostituzione.Text
                myItem.sDataCessazione = myItem.sDataSostituzione
            End If
            If txtNumeroUtenze.Text <> "" Then
                myItem.nNumeroUtenze = txtNumeroUtenze.Text
            End If
            If cboTipoUtenze.SelectedItem.Value <> 0 And cboTipoUtenze.SelectedItem.Value <> -1 Then
                myItem.nTipoUtenza = cboTipoUtenze.SelectedItem.Value
            End If
            If cboDiametroContatore.SelectedItem.Value <> 0 And cboDiametroContatore.SelectedItem.Value <> -1 Then
                myItem.nDiametroContatore = cboDiametroContatore.SelectedItem.Value
            End If
            If cboDiametroPresa.SelectedItem.Value <> 0 And cboDiametroPresa.SelectedItem.Value <> -1 Then
                myItem.nDiametroPresa = cboDiametroPresa.SelectedItem.Value
            End If
            If CInt(Request.Params("hdCodAnagrafeIntestatario")) > 0 Then
                myItem.nIdIntestatario = CInt(Request.Params("hdCodAnagrafeIntestatario"))
            End If
            If CInt(Request.Params("HDTextCodUtente")) > 0 Then
                myItem.nIdUtente = CInt(Request.Params("HDTextCodUtente"))
            End If
            If txtCivico.Text <> "" And txtCivico.Text <> "-1" And txtCivico.Text <> "0" Then
                myItem.sCivico = txtCivico.Text
            End If
            If TxtCodVia.Text <> -1 Then
                myItem.nIdVia = TxtCodVia.Text
            Else
                myItem.nIdVia = -1
            End If
            If TxtVia.Text <> "" Then
                myItem.sUbicazione = TxtVia.Text
            Else
                myItem.sUbicazione = ""
            End If
            If CInt(Request.Params("hdCodContratto")) > 0 Then
                myItem.nIdContratto = CInt(Request.Params("hdCodContratto"))
            End If
            If Request.Params("hdConsumoMinimo") <> "" Then
                myItem.nConsumoMinimo = Request.Params("hdConsumoMinimo")
            End If
            If NewCodContatore <> "" Then
                myItem.nIdContatore = NewCodContatore
                Log.Debug("Controllo.SetDatiContatore->myItem.nIdContatore da NewCodContatore=" + myItem.nIdContatore.ToString)
            End If
            If txtDataSospsensioneUtenza.Text <> "" Then
                myItem.sDataSospensioneUtenza = txtDataSospsensioneUtenza.Text
            End If
            If chkUtenteSospeso.Checked = True Then
                myItem.bUtenteSospeso = 1
            End If
            If txtQuoteAgevolate.Text <> "" Then
                myItem.sQuoteAgevolate = txtQuoteAgevolate.Text
            End If
            If txtCodiceFabbricatore.Text <> "" Then
                myItem.sCodiceFabbricante = txtCodiceFabbricatore.Text
            End If
            If txtNumeroCifreContatore.Text <> "" Then
                myItem.sCifreContatore = txtNumeroCifreContatore.Text
            End If
            'If cboAssogettamentoIva.SelectedItem.Value <> 0 And cboAssogettamentoIva.SelectedItem.Value <> -1 Then
            '    myItem.nCodIva = cboAssogettamentoIva.SelectedItem.Value
            'End If
            If cboStatoContatore.SelectedItem.Value <> "" Then
                myItem.sStatoContatore = cboStatoContatore.SelectedItem.Value
            End If
            'If cboPenalita.SelectedItem.Value <> "" Then
            '    myItem.sPenalita = cboPenalita.SelectedItem.Value
            'End If
            If ConstSession.CodIstat <> "" Then
                myItem.sCodiceISTAT = ConstSession.CodIstat
            End If
            If txtEsponente.Text <> "" Then
                myItem.sEsponenteCivico = txtEsponente.Text
            End If
            If cboMinimi.SelectedItem.Value <> 0 And cboMinimi.SelectedItem.Value <> -1 Then
                myItem.nIdMinimo = cboMinimi.SelectedItem.Value
            End If
            'If cboAttivita.SelectedItem.Value <> 0 And cboAttivita.SelectedItem.Value <> -1 Then
            '    myItem.nIdAttivita = cboAttivita.SelectedItem.Value
            'End If
            'agenzia entrate
            If ddlAssenzaDatiCat.SelectedValue <> "" Then
                myItem.nIdAssenzaDatiCatastali = ddlAssenzaDatiCat.SelectedValue
            End If
            If ddlTipoUnita.SelectedValue <> "" Then
                myItem.sTipoUnita = ddlTipoUnita.SelectedValue
            End If
            If ddlTipoUtenza.SelectedValue <> "" Then
                'es. utenza domestica/non domestica
                myItem.nIdTipoUtenza = ddlTipoUtenza.SelectedValue
            End If
            If ddlTitOccupazione.SelectedValue <> "" Then
                'es. proprietario/affittuario
                myItem.nIdTitoloOccupazione = ddlTitOccupazione.SelectedValue
            End If
            '/agenzia entrate
            myItem.tDataInserimento = Now
            myItem.tDataVariazione = DateTime.MaxValue
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DatiGenerali.SetDatiContatore.errore: ", ex)
            myItem = Nothing
        End Try
        Return myItem
    End Function
    Private Sub btnStampa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStampa.Click

        Try
            Dim sScript As String = "GestAlert('a', 'warning', '', '', 'Funzionalita\' al momento non disponibile!');"
            RegisterScript(sScript, Me.GetType())
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DatiGenerali.btnStampa_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Public Sub FillDropDownMinimo(ByVal cboTemp As DropDownList, ByVal dvMyDati As DataView, ByVal lngSelectedID As Long)
        Try
            Dim myListItem As ListItem
            Dim lngRecordCount As Long

            myListItem = New ListItem
            myListItem.Text = "..."
            myListItem.Value = "-1"
            cboTemp.Items.Add(myListItem)

            If Not dvMyDati Is Nothing Then
                For Each myRow As DataRowView In dvMyDati
                    myListItem = New ListItem
                    myListItem.Text = StringOperation.FormatString(myRow(1)) & "--" & "[" & StringOperation.FormatString(myRow(2)) & "]"
                    myListItem.Value = StringOperation.FormatInt(dvMyDati(0))
                    cboTemp.Items.Add(myListItem)
                    lngRecordCount = lngRecordCount + 1
                Next
            End If
            If lngSelectedID <> -1 Then
                FncGen.SelectIndexDropDownList(cboTemp, CStr(lngSelectedID))
            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DatiGenerali.FillDropDownMinimo.errore: ", ex)
        Finally
            dvMyDati.Dispose()
        End Try
    End Sub
    'Public Sub FillDropDownMinimo(ByVal myDropDown As DropDownList, ByVal myDV As DataView, ByVal SelectedID As Integer)
    '    Try
    '        Dim myListItem As ListItem

    '        myListItem = New ListItem
    '        myListItem.Text = "..."
    '        myListItem.Value = "-1"
    '        myDropDown.Items.Add(myListItem)

    '        For Each myRow As DataRowView In myDV
    '            myListItem = New ListItem
    '            myListItem.Text = StringOperation.FormatString(myRow(1)) & "--" & "[" & StringOperation.FormatString(myRow(2)) & "]"
    '            myListItem.Value = StringOperation.FormatInt(myRow(0))
    '            myDropDown.Items.Add(myListItem)
    '        Next

    '        If SelectedID <> -1 Then
    '            FncGen.SelectIndexDropDownList(myDropDown, CStr(SelectedID))
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DatiGenerali.FillDropDownMinimo.errore: ", ex)
    '    End Try
    'End Sub

    Private Sub bntCalcolaNumeroUtente_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles bntCalcolaNumeroUtente.Click
        Dim FncMy As New GestContratti
        Dim mionumero As Int32

        If txtNumeroUtente.Text <> "" Then
            mionumero = txtNumeroUtente.Text
        Else
            mionumero = 0
        End If

        mionumero = FncMy.GetNumeroUtente(ConstSession.IdEnte, mionumero)
        If mionumero > 0 Then
            txtNumeroUtente.Text = mionumero
        End If
    End Sub

    Private Sub cboTipoUtenze_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboTipoUtenze.SelectedIndexChanged
        '*******************************************************
        'Caricamento del Combo minimi in base alla tipologia di utenza
        '*******************************************************
        Dim hdCodiceVia As Integer = CInt(Request.Params("hdCodiceVia"))
        Dim hdCodAnagrafeIntestatario As Integer = CInt(Request.Params("hdCodAnagrafeIntestatario"))
        Dim HDTextCodUtente As Integer = CInt(Request.Params("HDTextCodUtente"))

        Dim hdCodContratto As Integer = CInt(Request.Params("hdCodContratto"))
        Try
            If hdCodContratto = -1 Then
                hdCodContratto = 0
            End If

            Dim hdDataSottoScrizione As String = Request.Params("hdDataSottoScrizione")
            Dim hdConsumoMinimo As String = Request.Params("hdConsumoMinimo")
            Dim hdTipoUtenzaContratto As Integer = CInt(Request.Params("hdTipoUtenzaContratto"))
            Dim hdIdDiametroContatoreContratto As Integer = CInt(Request.Params("hdIdDiametroContatoreContratto"))
            Dim hdIdDiametroPresaContratto As Integer = CInt(Request.Params("hdIdDiametroPresaContratto"))
            Dim hdNumeroUtenzeContratto As Integer = Request.Params("hdNumeroUtenzeContratto")
            Dim hdCodiceContratto As String = Request.Params("hdCodiceContratto")
            Dim hdVirtualIDContratto As Integer = CInt(Request.Params("hdVirtualIDContratto"))

            Dim sScript As String = ""

            sScript += "document.getElementById('hdEnteAppartenenza').value='" & ConstSession.IdEnte & "';"
            sScript += "document.getElementById('hdCodAnagrafeIntestatario').value='" & hdCodAnagrafeIntestatario & "';"
            sScript += "document.getElementById('HDTextCodUtente').value='" & HDTextCodUtente & "';"
            sScript += "document.getElementById('hdCodContratto').value='" & hdCodContratto & "';"
            sScript += "document.getElementById('hdCodContatore').value='" & txtidContatore.Text & "';"
            sScript += "document.getElementById('hdDataSottoScrizione').value='" & hdDataSottoScrizione & "';"
            sScript += "document.getElementById('hdConsumoMinimo').value='" & hdConsumoMinimo & "';"
            sScript += "document.getElementById('hdTipoUtenzaContratto').value='" & hdTipoUtenzaContratto & "';"
            sScript += "document.getElementById('hdIdDiametroContatoreContratto').value='" & hdIdDiametroContatoreContratto & "';"
            sScript += "document.getElementById('hdIdDiametroPresaContratto').value='" & hdIdDiametroPresaContratto & "';"
            sScript += "document.getElementById('hdNumeroUtenzeContratto').value='" & hdNumeroUtenzeContratto & "';"
            sScript += "document.getElementById('hdCodiceContratto').value='" & hdCodiceContratto & "';"
            sScript += "document.getElementById('hdCodiceVia').value='" & hdCodiceVia & "';"
            sScript += "document.getElementById('hdVirtualIDContratto').value='" & hdVirtualIDContratto & "';"


            cboMinimi.Items.Clear()

            Dim dvMyDati As New DataView
            Try
                dvMyDati = New TabelleDiDecodifica.DBTipiUtenza().GetListaTipiUtenza(ConstSession.IdEnte, cboTipoUtenze.SelectedItem.Value, -1)
                If Not dvMyDati Is Nothing Then
                    For Each myRow As DataRowView In dvMyDati
                        If Utility.StringOperation.FormatBool(myRow("PRINCIPALE")) Then
                            sScript += "document.getElementById('hdRiportaNumeroUtenze').value='1';"
                            If Len(Trim(txtNumeroUtenze.Text)) > 0 Then
                                txtQuoteAgevolate.Text = txtNumeroUtenze.Text
                            End If
                        Else
                            txtQuoteAgevolate.Text = "0"
                        End If
                    Next
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBPeriodo.GetTipiUte.Update.errore: ", ex)
            Finally
                dvMyDati.Dispose()
            End Try
            Try
                dvMyDati = New TabelleDiDecodifica.DBMinimiFatturabili().GetListaMinimiFatturabili(cboTipoUtenze.SelectedItem.Value)
                If Not dvMyDati Is Nothing Then
                    FillDropDownMinimo(cboMinimi, dvMyDati, cboTipoUtenze.SelectedItem.Value)
                End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DBPeriodo.GetMinimiFatt.Update.errore: ", ex)
            Finally
                dvMyDati.Dispose()
            End Try
            RegisterScript(sScript, Me.GetType())
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DatiGenerali.SelectedIndexChanged.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    'Private Sub btnApriIntestatario_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnApriIntestatario.Click
    '    'devo creare la sessione al workflow
    '    Dim oSM As New RIBESFrameWork.SessionManager(Session("PARAMETROENV"))
    '    Dim oSession As RIBESFrameWork.Session
    'Try
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

    '    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DatiGenerali.btnApriIntestatario_Click.errore: ", Err)
    '    Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub

    Private Sub btnApriIntestatario_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnApriIntestatario.Click
        Dim oAnagrafica As New Anagrafica.DLL.GestioneAnagrafica()
        Dim oDettaglioAnagraficaInt As New DettaglioAnagrafica
        '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
        oDettaglioAnagraficaInt = oAnagrafica.GetAnagrafica(CLng(HDtxtCodIntestatario.Text), -1, String.Empty, ConstSession.DBType, ConstSession.StringConnectionAnagrafica, False)
        Session("oDettaglioAnagraficaInt") = oDettaglioAnagraficaInt

        Session("TipoAnagrafica") = "Intestatario"

        Dim sScript As String = ""
        sScript += "ApriAnagrafica(" & CLng(HDtxtCodIntestatario.Text) & ", 'oDettaglioAnagraficaInt');"
        RegisterScript(sScript, Me.GetType())
    End Sub

    'Private Sub btnRibalta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRibalta.Click
    '    Try
    '        Dim buffScriptString As String
    '        Dim WFErrore As String
    '        Dim WFSessione As New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        Dim oAnagrafica As New Anagrafica.DLL.GestioneAnagrafica()(WFSessione.oSession, Session("ANAGRAFICA"))
    '        Dim oDettaglioAnagrafica As New DettaglioAnagrafica

    '        Select Case Session("TipoAnagrafica")
    '            Case "Intestatario"
    '                oDettaglioAnagrafica = Session("oDettaglioAnagraficaInt")
    '                HDtxtCodIntestatario.Text = oDettaglioAnagrafica.COD_CONTRIBUENTE
    '                'txtNomeIntestatario.text= Replace(oDettaglioAnagrafica.Nome, "'", "\'")
    '                'txtCivicoIntestatario.text= oDettaglioAnagrafica.CivicoResidenza
    '                'txtComuneIntestatario.text= Replace(oDettaglioAnagrafica.ComuneResidenza, "'", "\'") & " (" & Replace(oDettaglioAnagrafica.ProvinciaResidenza, "'", "\'") & ")"
    '                'txtProvinciaIntestatario.text= oDettaglioAnagrafica.ProvinciaResidenza
    '                txtCognomeIntestatario.Text = Replace(oDettaglioAnagrafica.Cognome & " " & oDettaglioAnagrafica.Nome, "'", "\'")
    '                txtCodiceFiscaleIntestatario.Text = oDettaglioAnagrafica.DataNascita
    '                txtViaIntestatario.Text = Replace(oDettaglioAnagrafica.ViaResidenza & " " & oDettaglioAnagrafica.CivicoResidenza & " " & oDettaglioAnagrafica.CapResidenza & " " & oDettaglioAnagrafica.ComuneResidenza & " " & oDettaglioAnagrafica.ProvinciaResidenza, "'", "\'")
    '                hdCodAnagrafeIntestatario.Text = oDettaglioAnagrafica.COD_CONTRIBUENTE
    '                If HDTextCodUtente.Text = "-1" Or HDTextCodUtente.Text = "" Or HDTextCodUtente.Text = HDtxtCodIntestatario.Text Then
    '                    HDTextCodUtente.Text = oDettaglioAnagrafica.COD_CONTRIBUENTE
    '                    'txtCodiceFiscaleUtente.text= oDettaglioAnagrafica.DataNascita
    '                    'txtCivicoUtente.text= oDettaglioAnagrafica.CivicoResidenza
    '                    'txtComuneUtente.text= Replace(oDettaglioAnagrafica.ComuneResidenza, "'", "\'") & " (" & Replace(oDettaglioAnagrafica.ProvinciaResidenza, "'", "\'") & ")"
    '                    'txtProvinciaUtente.text= oDettaglioAnagrafica.ProvinciaResidenza
    '                    txtCognomeUtente.Text = Replace(oDettaglioAnagrafica.Cognome & " " & oDettaglioAnagrafica.Nome, "'", "\'")
    '                    'txtNomeUtente.text = Replace(oDettaglioAnagrafica.Nome, "'", "\'")
    '                    txtViaUtente.Text = Replace(oDettaglioAnagrafica.ViaResidenza & " " & oDettaglioAnagrafica.CivicoResidenza & " " & oDettaglioAnagrafica.CapResidenza & " " & oDettaglioAnagrafica.ComuneResidenza & " " & oDettaglioAnagrafica.ProvinciaResidenza, "'", "\'")
    '                    HDTextCodUtente.Text = oDettaglioAnagrafica.COD_CONTRIBUENTE
    '                End If

    '            Case "Utente"
    '                oDettaglioAnagrafica = Session("oDettaglioAnagraficaUtente")

    '                HDTextCodUtente.Text = oDettaglioAnagrafica.COD_CONTRIBUENTE
    '                txtCodFisUte.Text = oDettaglioAnagrafica.CodiceFiscale
    '                '   txtNomeUtente.text= Replace(oDettaglioAnagrafica.Nome, "'", "\'")
    '                'txtCivicoUtente.text= oDettaglioAnagrafica.CivicoResidenza
    '                ' txtComuneUtente.text= Replace(oDettaglioAnagrafica.ComuneResidenza, "'", "\'") & " (" & Replace(oDettaglioAnagrafica.ComuneResidenza, "'", "\'") & ")"
    '                ' txtProvinciaUtente.text= oDettaglioAnagrafica.ProvinciaResidenza
    '                txtCognomeUtente.Text = Replace(oDettaglioAnagrafica.Cognome & " " & oDettaglioAnagrafica.Nome, "'", "\'")
    '                txtCodiceFiscaleUtente.Text = oDettaglioAnagrafica.DataNascita
    '                txtViaUtente.Text = Replace(oDettaglioAnagrafica.ViaResidenza & " " & oDettaglioAnagrafica.CivicoResidenza & " " & oDettaglioAnagrafica.CapResidenza & " " & oDettaglioAnagrafica.ComuneResidenza & " " & oDettaglioAnagrafica.ProvinciaResidenza, "'", "\'")
    '                HDTextCodUtente.Text = oDettaglioAnagrafica.COD_CONTRIBUENTE
    '                If HDTextCodUtente.Text = HDtxtCodIntestatario.Text Then
    '                    'txtNomeIntestatario.text= Replace(oDettaglioAnagrafica.Nome, "'", "\'")
    '                    'txtCivicoIntestatario.text= oDettaglioAnagrafica.CivicoResidenza
    '                    'txtComuneIntestatario.text= Replace(oDettaglioAnagrafica.ComuneResidenza, "'", "\'") & " (" & Replace(oDettaglioAnagrafica.ComuneResidenza, "'", "\'") & ")" & " ';" + vbCrLf + _
    '                    'txtProvinciaIntestatario.text= oDettaglioAnagrafica.ProvinciaResidenza
    '                    txtCognomeIntestatario.Text = Replace(oDettaglioAnagrafica.Cognome & " " & oDettaglioAnagrafica.Nome, "'", "\'")
    '                    txtCodiceFiscaleIntestatario.Text = oDettaglioAnagrafica.DataNascita
    '                    txtViaIntestatario.Text = Replace(oDettaglioAnagrafica.ViaResidenza & " " & oDettaglioAnagrafica.CivicoResidenza & " " & oDettaglioAnagrafica.CapResidenza & " " & oDettaglioAnagrafica.ComuneResidenza & " " & oDettaglioAnagrafica.ProvinciaResidenza, "'", "\'")
    '                    hdCodAnagrafeIntestatario.Text = oDettaglioAnagrafica.COD_CONTRIBUENTE
    '                End If
    '        End Select

    '    Catch ex As Exception
    '     Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DatiGenerali.btnRibalta_Click.errore: ", ex)
    '    Response.Redirect("../../PaginaErrore.aspx")  
    '    End Try
    'End Sub

    Private Sub btnRibalta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRibalta.Click
        Try
            Dim oDettaglioAnagrafica As New DettaglioAnagrafica

            Select Case Utility.StringOperation.FormatString(Session("TipoAnagrafica"))
                Case "Intestatario"
                    oDettaglioAnagrafica = Session("oDettaglioAnagraficaInt")
                    HDtxtCodIntestatario.Text = oDettaglioAnagrafica.COD_CONTRIBUENTE
                    txtCognomeIntestatario.Text = Replace(oDettaglioAnagrafica.Cognome & " " & oDettaglioAnagrafica.Nome, "'", "\'")
                    txtCodiceFiscaleIntestatario.Text = oDettaglioAnagrafica.DataNascita
                    txtViaIntestatario.Text = Replace(oDettaglioAnagrafica.ViaResidenza & " " & oDettaglioAnagrafica.CivicoResidenza & " " & oDettaglioAnagrafica.CapResidenza & " " & oDettaglioAnagrafica.ComuneResidenza & " " & oDettaglioAnagrafica.ProvinciaResidenza, "'", "\'")
                    hdCodAnagrafeIntestatario.Text = oDettaglioAnagrafica.COD_CONTRIBUENTE
                    If HDTextCodUtente.Text = "-1" Or HDTextCodUtente.Text = "" Or HDTextCodUtente.Text = HDtxtCodIntestatario.Text Then
                        HDTextCodUtente.Text = oDettaglioAnagrafica.COD_CONTRIBUENTE
                        txtCognomeUtente.Text = Replace(oDettaglioAnagrafica.Cognome & " " & oDettaglioAnagrafica.Nome, "'", "\'")
                        txtViaUtente.Text = Replace(oDettaglioAnagrafica.ViaResidenza & " " & oDettaglioAnagrafica.CivicoResidenza & " " & oDettaglioAnagrafica.CapResidenza & " " & oDettaglioAnagrafica.ComuneResidenza & " " & oDettaglioAnagrafica.ProvinciaResidenza, "'", "\'")
                        HDTextCodUtente.Text = oDettaglioAnagrafica.COD_CONTRIBUENTE
                    End If

                Case "Utente"
                    oDettaglioAnagrafica = Session("oDettaglioAnagraficaUtente")

                    HDTextCodUtente.Text = oDettaglioAnagrafica.COD_CONTRIBUENTE
                    txtCodFisUte.Text = oDettaglioAnagrafica.CodiceFiscale
                    '   txtNomeUtente.text= Replace(oDettaglioAnagrafica.Nome, "'", "\'")
                    'txtCivicoUtente.text= oDettaglioAnagrafica.CivicoResidenza
                    ' txtComuneUtente.text= Replace(oDettaglioAnagrafica.ComuneResidenza, "'", "\'") & " (" & Replace(oDettaglioAnagrafica.ComuneResidenza, "'", "\'") & ")"
                    ' txtProvinciaUtente.text= oDettaglioAnagrafica.ProvinciaResidenza
                    txtCognomeUtente.Text = Replace(oDettaglioAnagrafica.Cognome & " " & oDettaglioAnagrafica.Nome, "'", "\'")
                    txtCodiceFiscaleUtente.Text = oDettaglioAnagrafica.DataNascita
                    txtViaUtente.Text = Replace(oDettaglioAnagrafica.ViaResidenza & " " & oDettaglioAnagrafica.CivicoResidenza & " " & oDettaglioAnagrafica.CapResidenza & " " & oDettaglioAnagrafica.ComuneResidenza & " " & oDettaglioAnagrafica.ProvinciaResidenza, "'", "\'")
                    HDTextCodUtente.Text = oDettaglioAnagrafica.COD_CONTRIBUENTE
                    If HDTextCodUtente.Text = HDtxtCodIntestatario.Text Then
                        'txtNomeIntestatario.text= Replace(oDettaglioAnagrafica.Nome, "'", "\'")
                        'txtCivicoIntestatario.text= oDettaglioAnagrafica.CivicoResidenza
                        'txtComuneIntestatario.text= Replace(oDettaglioAnagrafica.ComuneResidenza, "'", "\'") & " (" & Replace(oDettaglioAnagrafica.ComuneResidenza, "'", "\'") & ")" & " ';" + vbCrLf + _
                        'txtProvinciaIntestatario.text= oDettaglioAnagrafica.ProvinciaResidenza
                        txtCognomeIntestatario.Text = Replace(oDettaglioAnagrafica.Cognome & " " & oDettaglioAnagrafica.Nome, "'", "\'")
                        txtCodiceFiscaleIntestatario.Text = oDettaglioAnagrafica.DataNascita
                        txtViaIntestatario.Text = Replace(oDettaglioAnagrafica.ViaResidenza & " " & oDettaglioAnagrafica.CivicoResidenza & " " & oDettaglioAnagrafica.CapResidenza & " " & oDettaglioAnagrafica.ComuneResidenza & " " & oDettaglioAnagrafica.ProvinciaResidenza, "'", "\'")
                        hdCodAnagrafeIntestatario.Text = oDettaglioAnagrafica.COD_CONTRIBUENTE
                    End If
            End Select
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DatiGenerali.btnRibalta_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    'Private Sub btnApriUtente_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnApriUtente.Click
    '    'devo creare la sessione al workflow
    '    Dim oSM As New RIBESFrameWork.SessionManager(Session("PARAMETROENV"))
    '    Dim oSession As RIBESFrameWork.Session
    '    Try
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
    'Catch Err As Exception
    ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DatiGenerali.btnApriUtente_Click.errore: ", Err)
    '      Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub

    Private Sub btnApriUtente_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnApriUtente.Click
        Dim oAnagrafica As New Anagrafica.DLL.GestioneAnagrafica()
        Dim oDettaglioAnagraficaUtente As New DettaglioAnagrafica
        '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
        oDettaglioAnagraficaUtente = oAnagrafica.GetAnagrafica(CLng(HDTextCodUtente.Text), -1, String.Empty, ConstSession.DBType, ConstSession.StringConnectionAnagrafica, False)
        Session("oDettaglioAnagraficaUtente") = oDettaglioAnagraficaUtente

        Session("TipoAnagrafica") = "Utente"

        Dim sScript As String = ""
        sScript += "ApriAnagrafica(" & CLng(HDTextCodUtente.Text) & ", 'oDettaglioAnagraficaUtente');"
        RegisterScript(sScript, Me.GetType())
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
                        sScript = "<script language=""javascript"">" + vbCrLf +
                        "<!-- " + vbCrLf +
                        "document.getElementById('txtCognomeIntestatario').value ='" & Replace(oDettaglioAnagrafica.Cognome & " " & oDettaglioAnagrafica.Nome, "'", "\'") & "';" + vbCrLf +
                        "document.getElementById('txtCodiceFiscaleIntestatario').value ='" & oDettaglioAnagrafica.DataNascita & "';" + vbCrLf +
                        "document.getElementById('txtViaIntestatario').value ='" & Replace(oDettaglioAnagrafica.ViaResidenza & " " & oDettaglioAnagrafica.CivicoResidenza & " " & oDettaglioAnagrafica.CapResidenza & " " & oDettaglioAnagrafica.ComuneResidenza & " " & oDettaglioAnagrafica.ProvinciaResidenza, "'", "\'") & "';" + vbCrLf +
                        "document.getElementById('hdCodAnagrafeIntestatario').value ='" & oDettaglioAnagrafica.COD_CONTRIBUENTE & "';" + vbCrLf
                        If HDTextCodUtente.Text = "-1" Or HDTextCodUtente.Text = HDtxtCodIntestatario.Text Then
                            HDTextCodUtente.Text = oDettaglioAnagrafica.COD_CONTRIBUENTE
                            sScript += "document.getElementById('txtCognomeUtente').value ='" & Replace(oDettaglioAnagrafica.Cognome & " " & oDettaglioAnagrafica.Nome, "'", "\'") & "';" + vbCrLf +
                            "document.getElementById('txtCodiceFiscaleUtente').value ='" & oDettaglioAnagrafica.DataNascita & "';" + vbCrLf +
                            "document.getElementById('txtViaUtente').value ='" & Replace(oDettaglioAnagrafica.ViaResidenza & " " & oDettaglioAnagrafica.CivicoResidenza & " " & oDettaglioAnagrafica.CapResidenza & " " & oDettaglioAnagrafica.ComuneResidenza & " " & oDettaglioAnagrafica.ProvinciaResidenza, "'", "\'") & "';" + vbCrLf +
                            "document.getElementById('HDTextCodUtente').value ='" & oDettaglioAnagrafica.COD_CONTRIBUENTE & "';" + vbCrLf
                        End If
                        sScript += "//parent.window.close();" + vbCrLf +
                         "--> " + vbCrLf +
                         ""

                    Case "Utente"
                        HDTextCodUtente.Text = oDettaglioAnagrafica.COD_CONTRIBUENTE
                        sScript = "<script language=""javascript"">" + vbCrLf +
                         "<!-- " + vbCrLf +
                        "document.getElementById('txtCognomeUtente').value ='" & Replace(oDettaglioAnagrafica.Cognome & " " & oDettaglioAnagrafica.Nome, "'", "\'") & "';" + vbCrLf +
                        "document.getElementById('txtCodiceFiscaleUtente').value ='" & oDettaglioAnagrafica.DataNascita & "';" + vbCrLf +
                        "document.getElementById('txtViaUtente').value ='" & Replace(oDettaglioAnagrafica.ViaResidenza & " " & oDettaglioAnagrafica.CivicoResidenza & " " & oDettaglioAnagrafica.CapResidenza & " " & oDettaglioAnagrafica.ComuneResidenza & " " & oDettaglioAnagrafica.ProvinciaResidenza, "'", "\'") & "';" + vbCrLf +
                        "document.getElementById('HDTextCodUtente').value ='" & oDettaglioAnagrafica.COD_CONTRIBUENTE & "';" + vbCrLf
                        If HDTextCodUtente.Text = HDtxtCodIntestatario.Text Then
                            sScript += "document.getElementById('txtCognomeIntestatario').value ='" & Replace(oDettaglioAnagrafica.Cognome, "'", "\'") & "';" + vbCrLf +
                            "document.getElementById('txtCodiceFiscaleIntestatario').value ='" & oDettaglioAnagrafica.DataNascita & "';" + vbCrLf +
                            "document.getElementById('txtViaIntestatario').value ='" & Replace(oDettaglioAnagrafica.ViaResidenza & " " & oDettaglioAnagrafica.CivicoResidenza & " " & oDettaglioAnagrafica.CapResidenza & " " & oDettaglioAnagrafica.ComuneResidenza & " " & oDettaglioAnagrafica.ProvinciaResidenza, "'", "\'") & "';" + vbCrLf +
                            "document.getElementById('hdCodAnagrafeIntestatario').value ='" & oDettaglioAnagrafica.COD_CONTRIBUENTE & "';" + vbCrLf
                        End If

                        sScript += "//parent.window.close();" + vbCrLf +
                         "--> " + vbCrLf +
                         ""
                End Select
                RegisterScript(sScript, Me.GetType())
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DatiGenerali.btnRibaltaAnagAnater_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    'Private Sub btnStampa2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStampa2.Click
    '    Dim objTestataDOC As New Stampa.oggetti.oggettoTestata
    '    Dim objTestataDOT As New Stampa.oggetti.oggettoTestata
    '    Dim oDettaglioAnagraficaint As DettaglioAnagrafica
    '    Dim oDettaglioAnagraficaUtente As DettaglioAnagrafica
    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Dim WFErrore As String

    '    Try
    '        'inizializzo la connessione
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If
    '        oDettaglioAnagraficaint = CType(Session("oDettaglioAnagraficaint"), DettaglioAnagrafica)
    '        oDettaglioAnagraficaUtente = CType(Session("oDettaglioAnagraficaUtente"), DettaglioAnagrafica)

    '        ' PERCORSO DEL FILE .DOC SOTTO LA CARTELLA SERVIZIO STAMPE

    '        objTestataDOC.Dominio = "Documenti"
    '        objTestataDOC.Ente = "OpenUtenze"
    '        objTestataDOC.Filename = "Preventivo"
    '        objTestataDOC.Atto = ""

    '        Dim oArrBookmark As ArrayList

    '        Dim objBookmark As Stampa.oggetti.oggettiStampa
    '        Dim oArrListOggettiDaStampare As New ArrayList
    '        Dim objToPrint As Stampa.oggetti.oggettoDaStampare
    '        Dim ArrayBookMark As Stampa.oggetti.oggettiStampa()

    '        oArrBookmark = New ArrayList

    '        Dim data As String = DateTime.Now.ToString("dd/MM/yyyy")
    '        objBookmark = New Stampa.oggetti.oggettiStampa
    '        objBookmark.Descrizione = "data"
    '        objBookmark.Valore = data
    '        oArrBookmark.Add(objBookmark)
    '        'copiare queste 4 righe nel caso ci fossero + segnalibri

    '        objBookmark = New Stampa.oggetti.oggettiStampa
    '        objBookmark.Descrizione = "comune"
    '        objBookmark.Valore = ConstSession.DescrizioneEnte
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New Stampa.oggetti.oggettiStampa
    '        objBookmark.Descrizione = "datainoltro"
    '        objBookmark.Valore = data
    '        oArrBookmark.Add(objBookmark)

    '        Dim sesso As String = oDettaglioAnagraficaint.Sesso
    '        If sesso.ToUpper = "F" Then
    '            sesso = "Sig.ra "
    '        End If
    '        If sesso.ToUpper = "M" Then
    '            sesso = "Sig. "
    '        End If
    '        objBookmark = New Stampa.oggetti.oggettiStampa
    '        objBookmark.Descrizione = "sigsigra"
    '        objBookmark.Valore = sesso
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New Stampa.oggetti.oggettiStampa
    '        objBookmark.Descrizione = "nome"
    '        objBookmark.Valore = oDettaglioAnagraficaint.Nome
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New Stampa.oggetti.oggettiStampa
    '        objBookmark.Descrizione = "cognome"
    '        objBookmark.Valore = oDettaglioAnagraficaint.Cognome
    '        oArrBookmark.Add(objBookmark)

    '        Dim DBContatori As GestContatori = New GestContatori
    '        Dim DetailContatore As New objContatore
    '        DetailContatore = DBContatori.GetDetailsContatori(ContatoreID, WFSessione, CInt(ConstSession.IdEnte), ConstSession.CodIstat)
    '        'ALE CAO HERE

    '        objBookmark = New Stampa.oggetti.oggettiStampa
    '        objBookmark.Descrizione = "preventivo"
    '        objBookmark.Valore = DetailContatore.nSpesa
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New Stampa.oggetti.oggettiStampa
    '        objBookmark.Descrizione = "dirittisegr"
    '        objBookmark.Valore = DetailContatore.nDiritti
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New Stampa.oggetti.oggettiStampa
    '        objBookmark.Descrizione = "via"
    '        objBookmark.Valore = oDettaglioAnagraficaint.ViaResidenza & ","
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New Stampa.oggetti.oggettiStampa
    '        objBookmark.Descrizione = "civico"
    '        objBookmark.Valore = oDettaglioAnagraficaint.CivicoResidenza
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New Stampa.oggetti.oggettiStampa
    '        objBookmark.Descrizione = "frazione"
    '        objBookmark.Valore = oDettaglioAnagraficaint.FrazioneResidenza
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New Stampa.oggetti.oggettiStampa
    '        objBookmark.Descrizione = "cap"
    '        objBookmark.Valore = oDettaglioAnagraficaint.CapResidenza
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New Stampa.oggetti.oggettiStampa
    '        objBookmark.Descrizione = "com"
    '        objBookmark.Valore = oDettaglioAnagraficaint.ComuneResidenza
    '        oArrBookmark.Add(objBookmark)

    '        ArrayBookMark = CType(oArrBookmark.ToArray(GetType(Stampa.oggetti.oggettiStampa)), Stampa.oggetti.oggettiStampa())

    '        objToPrint = New Stampa.oggetti.oggettoDaStampare
    '        objToPrint.Testata = objTestataDOC
    '        objToPrint.Stampa = ArrayBookMark

    '        oArrListOggettiDaStampare.Add(objToPrint)

    '        Dim GruppoDOCUMENTIDaStampare As Stampa.oggetti.oggettoDaStampare()
    '        Dim ArrListGruppoDOC As New ArrayList

    '        Dim ArrOggCompleto As Stampa.oggetti.oggettoDaStampareCompleto()
    '        Dim objTestataGruppo As New Stampa.oggetti.oggettoTestata

    '        GruppoDOCUMENTIDaStampare = CType(oArrListOggettiDaStampare.ToArray(GetType(Stampa.oggetti.oggettoDaStampare)), Stampa.oggetti.oggettoDaStampare())

    '        ' oggetto per la stampa
    '        Dim oInterfaceStampaDocOggetti As IElaborazioneStampaDocOggetti
    '        oInterfaceStampaDocOggetti = Activator.GetObject(GetType(IElaborazioneStampaDocOggetti), ConfigurationManager.AppSettings("ServizioStampe").ToString())

    '        Dim retArray As Stampa.oggetti.oggettoURL()

    '        retArray = oInterfaceStampaDocOggetti.StampaDocumenti("OpenUtenze\serviziopreventivo", GruppoDOCUMENTIDaStampare)
    '        Dim link As String = retArray(0).Url

    '        Response.Write("<script language='javascript' type='text/javascript'>window.open('" & link & "');")
    '    Catch Err As Exception
    '      Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DatiGenerali.btnStampa2_Click.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    Finally
    '        WFSessione.Kill()
    '    End Try
    'End Sub

    Private Sub btnStampa2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStampa2.Click

        Try
            Dim sScript As String = "GestAlert('a', 'warning', '', '', 'Funzionalita\' al momento non disponibile!');"
            RegisterScript(sScript, Me.GetType())
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DatiGenerali.btnStampa2_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Public Sub goContatoreSostituito(ByVal codContatore As String)
        RegisterScript("GestAlert('a', 'warning', '', '', 'Il contatore e\' stato sostituito.\n Verrai reindirizzato al nuovo contatore.');", Me.GetType)
        Log.Debug("Controllo->codcontatore per request goContatoreSostituito=" + codContatore)
        RegisterScript("location.href='DatiGenerali.aspx?IDCONTATORE=" & codContatore & "';", Me.GetType)
    End Sub

    Private Sub LnkAnagAnater_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles LnkAnagAnater.Click
        Session("TipoAnagrafica") = "Intestatario"
    End Sub

    Private Sub LnkAnagrUtente_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Session("TipoAnagrafica") = "Utente"
    End Sub

    Private Sub CmdRibaltaUIAnater_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdRibaltaUIAnater.Click
        'carico i dati catastali
        TxtVia.Text = Session("ViaUIAnater")
        TxtCodVia.Text = Session("CodViaUIAnater")
        txtCivico.Text = Session("CivicoUIAnater")
        txtEsponente.Text = Session("EsponenteUIAnater")
        Dim sScript As String = ""
        sScript += "document.getElementById('loadGrid').src='searchResultsCatasto.aspx?ContatoreID=" & txtidContatore.Text & "';"
        RegisterScript(sScript, Me.GetType())
    End Sub

    Public Function controllaDBNullInteri(ByVal input As Object) As Integer
        If Not IsDBNull(input) Then
            Return input
        Else
            Return -1
        End If
    End Function

    Public Function controllaDBNullStringhe(ByVal input As Object) As String
        If Not IsDBNull(input) Then
            Return input
        Else
            Return "Null"
        End If
    End Function

    Public Function controllaDBnullBool(ByVal input As Object) As Boolean
        If Not IsDBNull(input) Then
            Return input
        Else
            Return False
        End If
    End Function

    Private Sub LockPage()
        Dim sScript As String = ""

        cboImpianto.Enabled = False : cboGiro.Enabled = False : cboPosizione.Enabled = False : cboTipoContatore.Enabled = False
        cboFognatura.Enabled = False : cboDepurazione.Enabled = False
        cboTipoUtenze.Enabled = False : cboDiametroContatore.Enabled = False : cboDiametroPresa.Enabled = False
        cboStatoContatore.Enabled = False ' : cboAssogettamentoIva.Enabled = False
        cboMinimi.Enabled = False ': cboPenalita.Enabled = False : cboAttivita.Enabled = False

        chkEsenteFognatura.Enabled = False : chkEsenteDepurazione.Enabled = False : chkEsenteAcqua.Enabled = False
        '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
        chkEsenteAcquaQF.Enabled = False : chkEsenteDepQF.Enabled = False : chkEsenteFogQF.Enabled = False
        chkIgnoraMora.Enabled = False : chkUtenteSospeso.Enabled = False

        txtCivico.Enabled = False : TxtCodVia.Enabled = False : TxtVia.Enabled = False
        txtEsponente.Enabled = False : txtpiano.Enabled = False
        txtspesaprev.Enabled = False : txtdirittisegr.Enabled = False
        txtMatricola.Enabled = False : txtSequenza.Enabled = False
        txtNumeroUtente.Enabled = False : txtNumeroUtenze.Enabled = False
        txtProgressivo.Enabled = False : txtLatoStrada.Enabled = False
        txtContatorePrecedente.Enabled = False : txtContatoreSuccessivo.Enabled = False
        txtDataAttivazione.Enabled = False : txtDataSostituzione.Enabled = False
        txtDataRimTemp.Enabled = False : txtDataCessazione.Enabled = False : txtDataSospsensioneUtenza.Enabled = False
        txtQuoteAgevolate.Enabled = False : txtCodiceFabbricatore.Enabled = False : txtNumeroCifreContatore.Enabled = False
        txtNote.Enabled = False

        LnkAnagAnater.Enabled = False : LnkAnagrAnatUtente.Enabled = False
        LnkPulisciStrada.Enabled = False : LnkOpenStradario.Enabled = False
        LnkNewUIAnater.Enabled = False : LnkNewUIManuale.Enabled = False

        sScript += "document.getElementById('btnCodiceFiscale').disabled=true;"
        RegisterScript(sScript, Me.GetType())
    End Sub

    Private Sub AzzeraContatore(ByRef oMyContatore As objContatore)
        'azzeramento variabili
        oMyContatore.sMatricola = ""
        oMyContatore.sIdEnte = "-1"
        oMyContatore.sIdEnteAppartenenza = ""
        oMyContatore.nIdImpianto = -1
        oMyContatore.nGiro = -1
        oMyContatore.sSequenza = ""
        oMyContatore.nPosizione = -1
        oMyContatore.sProgressivo = ""
        oMyContatore.sLatoStrada = ""
        oMyContatore.sNumeroUtente = ""
        oMyContatore.nTipoContatore = -1
        oMyContatore.nIdContatorePrec = -1
        oMyContatore.sMatricolaContatorePrec = ""
        oMyContatore.nIdContatoreSucc = -1
        oMyContatore.sMatricolaContatoreSucc = ""
        oMyContatore.nCodDepurazione = -1
        oMyContatore.nCodFognatura = -1
        oMyContatore.bEsenteDepurazione = 0
        oMyContatore.bEsenteFognatura = 0
        oMyContatore.bEsenteAcqua = 0
        '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
        oMyContatore.bEsenteAcquaQF = 0
        oMyContatore.bEsenteDepQF = 0
        oMyContatore.bEsenteFogQF = 0
        oMyContatore.bIgnoraMora = 0
        oMyContatore.sNote = ""
        oMyContatore.sDataAttivazione = ""
        oMyContatore.sDataSostituzione = ""
        oMyContatore.sDataRimTemp = ""
        oMyContatore.sDataCessazione = ""
        oMyContatore.nNumeroUtenze = 0
        oMyContatore.nTipoUtenza = -1
        oMyContatore.nDiametroContatore = -1
        oMyContatore.nDiametroPresa = -1
        oMyContatore.nIdIntestatario = -1
        oMyContatore.nIdUtente = -1
        oMyContatore.sUbicazione = ""
        oMyContatore.sCivico = ""
        oMyContatore.nIdVia = -1
        oMyContatore.nIdContratto = -1
        oMyContatore.nConsumoMinimo = ""
        oMyContatore.sDataSospensioneUtenza = ""
        oMyContatore.bUtenteSospeso = 0
        oMyContatore.sQuoteAgevolate = ""
        oMyContatore.sCodiceFabbricante = ""
        oMyContatore.sCifreContatore = ""
        oMyContatore.nCodIva = -1
        oMyContatore.sStatoContatore = ""
        oMyContatore.sPenalita = ""
        oMyContatore.sCodiceISTAT = ""
        oMyContatore.nIdMinimo = -1
        oMyContatore.nIdAttivita = -1
        '===========================
        'INIZIO MODIFICHE ALE CAO
        '===========================
        oMyContatore.nSpesa = 0
        oMyContatore.nDiritti = 0
        oMyContatore.bIsPendente = 1
        oMyContatore.oListSubContatori = Nothing
        oMyContatore.nProprietario = 0

        '**** Fabi ***
        oMyContatore.sTipoUnita = ""
        oMyContatore.nIdTipoUtenza = -1
        oMyContatore.nIdAssenzaDatiCatastali = -1
        oMyContatore.nIdTitoloOccupazione = -1
    End Sub

#Region "Griglie"
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim IDRow As String = e.CommandArgument.ToString()
            Dim x, nList As Integer
            Dim oListSubContatori() As ObjSubContatore
            Dim oListSubContatoriNew() As ObjSubContatore
            oListSubContatoriNew = Nothing

            If Not IsNothing(Session("oListSubContatori")) Then
                oListSubContatori = CType(Session("oListSubContatori"), ObjSubContatore())
                nList = -1
                For x = 0 To oListSubContatori.GetUpperBound(0)
                    If oListSubContatori(x).sMatricola <> IDRow Then
                        nList += 1
                        ReDim Preserve oListSubContatoriNew(nList)
                        oListSubContatoriNew(nList) = oListSubContatori(x)
                    End If
                Next
                Session("oListSubContatori") = oListSubContatoriNew
                If Not IsNothing(oListSubContatoriNew) Then
                    GrdSubContatori.DataSource = oListSubContatoriNew
                    GrdSubContatori.DataBind()
                Else
                    GrdSubContatori.Visible = False
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DatiGenerali.GrdRowCommand.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub GrdSubContatori_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GrdSubContatori.DeleteCommand
    '    Dim x, nList As Integer
    '    Dim oListSubContatori() As ObjSubContatore
    '    Dim oListSubContatoriNew() As ObjSubContatore

    '    Try
    '        If Not IsNothing(Session("oListSubContatori")) Then
    '            oListSubContatori = CType(Session("oListSubContatori"), ObjSubContatore())
    '            nList = -1
    '            For x = 0 To oListSubContatori.GetUpperBound(0)
    '                If oListSubContatori(x).sMatricola <> e.Item().Cells(0).Text Then
    '                    nList += 1
    '                    ReDim Preserve oListSubContatoriNew(nList)
    '                    oListSubContatoriNew(nList) = oListSubContatori(x)
    '                End If
    '            Next
    '            Session("oListSubContatori") = oListSubContatoriNew
    '            If Not IsNothing(oListSubContatoriNew) Then
    '                GrdSubContatori.DataSource = oListSubContatoriNew
    '                GrdSubContatori.DataBind()
    '            Else
    '                GrdSubContatori.Visible = False
    '            End If
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DatiGenerali.GrdSubContatori_DeleteCommand.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
#End Region

    Private Sub CmdRibaltaSubContatori_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdRibaltaSubContatori.Click
        Try
            If Not IsNothing(Session("oListSubContatori")) Then
                Dim oListSubContatori() As ObjSubContatore
                oListSubContatori = CType(Session("oListSubContatori"), ObjSubContatore())
                GrdSubContatori.DataSource = oListSubContatori
                GrdSubContatori.DataBind()
            End If
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DatiGenerali.CmdRibaltaSubContatori_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub LoadComandi(ByVal bIsInVar As Boolean)
        'carico i comandi
        Dim paginacomandi As String = ConstSession.PathApplicazione & ConstSession.Path_H2O & "/DataEntryContatori/ComandiDataEntryContatori.aspx"
        Dim parametri As String = "?"
        Try
            'If Not Request.Item("DescTitolo") Is Nothing Then
            '    parametri = "?title=Acquedotto - " & CType(Request.Item("DescTitolo"), String)
            'Else
            '    parametri = "?title=Acquedotto - " & CType(Session("DESC_TIPO_PROC_SERV"), String).Replace("DE ", "")
            'End If
            'parametri += " - " & "Gestione" & "&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo
            If bIsInVar = True Then
                parametri += "&sProvenienza=R"
            Else
                parametri += "&sProvenienza=" & Request.Item("sProvenienza")
            End If
            '*** se ho una variazione in corso la provenienza diventa R{reading} in questo modo la videata è bloccata alle variazioni ***
            If Not Request.Item("sProvenienza") Is Nothing Then
                If Request.Item("sProvenienza") = "SC" Then
                    bIsInVar = False
                    parametri += "&OPENgovPATH=" & Request.Item("OPENgovPATH") & "&CodContribuente=" & Request.Item("CodContribuente")
                End If
            End If
            '*** ***

            Dim sScript As String = ""
            sScript += "parent.Comandi.location.href='" & paginacomandi & parametri & "';"
            RegisterScript(sScript, Me.GetType())
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DatiGenerali.LoadComandi.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    Private Sub LoadHidden(ByVal myContatore As objContatore)
        Try
            Log.Debug("Controllo->LoadHidden.txtidContatore=" + txtidContatore.Text)
            Dim sScript As String = ""
            sScript += "document.getElementById('hdEnteAppartenenza').value='" & ConstSession.DescrizioneEnte & "';"
            sScript += "document.getElementById('hdCodAnagrafeIntestatario').value='" & myContatore.nIdIntestatario & "';"
            sScript += "document.getElementById('HDTextCodUtente').value='" & myContatore.nIdUtente & "';"
            sScript += "document.getElementById('hdCodContratto').value='" & myContatore.nIdContratto & "';"
            sScript += "document.getElementById('hdCodContatore').value='" & txtidContatore.Text & "';"
            sScript += "document.getElementById('hdConsumoMinimo').value='" & myContatore.nConsumoMinimo & "';"
            sScript += "document.getElementById('hdTipoUtenzaContratto').value='" & myContatore.nIdTipoUtenza & "';"
            sScript += "document.getElementById('hdIdDiametroContatoreContratto').value='" & myContatore.nDiametroContatore & "';"
            sScript += "document.getElementById('hdIdDiametroPresaContratto').value='" & myContatore.nDiametroPresa & "';"
            sScript += "document.getElementById('hdNumeroUtenzeContratto').value='" & myContatore.nNumeroUtenze & "';"
            sScript += "document.getElementById('hdCodiceContratto').value='" & myContatore.nIdContratto & "';"
            sScript += "document.getElementById('hdCodiceVia').value='" & myContatore.nIdVia & "';"
            sScript += "document.getElementById('hdVirtualIDContratto').value='" & 0 & "';"
            RegisterScript(sScript, Me.GetType())

            'Gestione parametri di ricerca
            '========================================================================================

            sScript = "document.getElementById('IDContatore').value='" & txtidContatore.Text & "';"
            sScript += "document.getElementById('hdCodVia').value='" & Request("hdCodiceVia") & "';"
            sScript += "document.getElementById('hdIntestatario').value='" & UCase(Request("hdIntestatario")) & "';"
            sScript += "document.getElementById('hdUtente').value='" & UCase(Request("hdUtente")) & "';"
            sScript += "document.getElementById('hdNumeroUtente').value='" & Request("hdNumeroUtente") & "';"
            sScript += "document.getElementById('hdMatricola').value='" & UCase(Request("hdMatricola")) & "';"
            sScript += "document.getElementById('hdAvviaRicerca').value='" & UCase(Request("hdAvviaRicerca")) & "';"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DatiGenerali.LoadHidden.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** 201511 - tolto RIBESFRAMEWORK ***
    'Private Sub LoadDatiAE()
    '    Try
    '        Dim FncAE As New OPENgov_AgenziaEntrate.OPENgov_AgenziaEntrate.General
    '        Dim dvDati As New DataView
    '        Dim oLoadCombo As New ClsGenerale.Generale
    '        Dim WFSessione As OPENUtility.CreateSessione
    '        Dim WFErrore As String

    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConfigurationManager.AppSettings("OPENGOVG"))
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("LoadContatori::" & "Errore durante l'apertura della sessione di WorkFlow")
    '            Exit Sub
    '        End If

    '        Log.Debug("chiamata ad agenzia entrate dati generali contatore")

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
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DatiGenerali.LoadDatiAE.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    Private Sub LoadDatiAE()
        Try
            Dim FncAE As New OPENgov_AgenziaEntrate.OPENgov_AgenziaEntrate.General
            Dim dvDati As New DataView
            Dim oLoadCombo As New ClsGenerale.Generale

            Log.Debug("chiamata ad agenzia entrate dati generali contatore")

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
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DatiGenerali.LoadDatiAE.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** ***
    Private Sub LoadCombos(ByVal myContatore As objContatore)
        Dim FncContatori As New GestContatori
        Dim myReader As New DataView
        Dim dvMyDati As New DataView

        Try
            Log.Debug("carica combo dati generali contatore")
            dvMyDati = FncContatori.getListTipoContatore()
            FncGen.FillDropDownSQL(cboTipoContatore, dvMyDati, myContatore.nTipoContatore)

            dvMyDati = FncContatori.getListCodiceImpianto()
            FncGen.FillDropDownSQL(cboImpianto, dvMyDati, myContatore.nIdImpianto)

            dvMyDati = FncContatori.getListGiro(ConstSession.IdEnte)
            FncGen.FillDropDownSQL(cboGiro, dvMyDati, myContatore.nGiro)

            dvMyDati = FncContatori.getListPosizioneContatore()
            FncGen.FillDropDownSQL(cboPosizione, dvMyDati, myContatore.nPosizione)

            dvMyDati = FncContatori.getListCodFognatura()
            FncGen.FillDropDownSQL(cboFognatura, dvMyDati, myContatore.nCodFognatura)

            dvMyDati = FncContatori.getListCodDepurazione()
            FncGen.FillDropDownSQL(cboDepurazione, dvMyDati, myContatore.nCodDepurazione)

            dvMyDati = FncContatori.getListDiametroContatore(ConstSession.CodIstat)
            FncGen.FillDropDownSQL(cboDiametroContatore, dvMyDati, myContatore.nDiametroContatore)

            dvMyDati = FncContatori.getListDiametroPresa()
            FncGen.FillDropDownSQL(cboDiametroPresa, dvMyDati, myContatore.nDiametroPresa)


            dvMyDati = FncContatori.getListTipoUtenza(ConstSession.IdEnte, myContatore.sDataAttivazione, myContatore.nTipoUtenza)
            FncGen.FillDropDownSQL(cboTipoUtenze, dvMyDati, myContatore.nTipoUtenza)

            'myReader = FncContatori.getListIVA()
            'FncGen.FillDropDownSQL(cboAssogettamentoIva, myReader, myContatore.nCodIva)
            'myReader = FncContatori.getListTipoAttivita()
            'FncGen.FillDropDownSQL(cboAttivita, myReader, myContatore.nIdAttivita)

            myReader = iDB.GetDataView("SELECT IDMINIMO,DESCRIZIONE,MINIMO  FROM TP_MININIMIFATTURABILI WHERE IDTIPOUTENZA = " & myContatore.nIdTipoUtenza)
            FncGen.FillDropDownSQL(cboMinimi, myReader, myContatore.nIdMinimo)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DatiGenerali.LoadCombos.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    Private Sub LoadAnagrafica(ByVal myContatore As objContatore)
        Try
            Dim oAnagrafica As New Anagrafica.DLL.GestioneAnagrafica()
            Dim oDettaglioAnagraficaInt As New DettaglioAnagrafica
            Dim oDettaglioAnagraficaUtente As New DettaglioAnagrafica
            '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
            oDettaglioAnagraficaInt = oAnagrafica.GetAnagrafica(myContatore.nIdIntestatario, -1, String.Empty, ConstSession.DBType, ConstSession.StringConnectionAnagrafica, False)
            oDettaglioAnagraficaUtente = oAnagrafica.GetAnagrafica(myContatore.nIdUtente, -1, String.Empty, ConstSession.DBType, ConstSession.StringConnectionAnagrafica, False)
            'ale
            Session("oDettaglioAnagraficaInt") = oDettaglioAnagraficaInt
            Session("oDettaglioAnagraficaUtente") = oDettaglioAnagraficaUtente
            '/ale

            txtCognomeIntestatario.Text = oDettaglioAnagraficaInt.Cognome & " " & oDettaglioAnagraficaInt.Nome & " " & oDettaglioAnagraficaInt.PartitaIva & " " & oDettaglioAnagraficaInt.CodiceFiscale
            txtCodiceFiscaleIntestatario.Text = oDettaglioAnagraficaInt.DataNascita

            txtViaIntestatario.Text = oDettaglioAnagraficaInt.ViaResidenza & " " & oDettaglioAnagraficaInt.CivicoResidenza
            If Len(Trim(oDettaglioAnagraficaInt.EsponenteCivicoResidenza)) <> 0 Then
                txtViaIntestatario.Text += "/" & Replace(oDettaglioAnagraficaInt.EsponenteCivicoResidenza, "/", "")
            End If
            txtViaIntestatario.Text += " - " & oDettaglioAnagraficaInt.CapResidenza & " " & oDettaglioAnagraficaInt.ComuneResidenza & " (" & oDettaglioAnagraficaInt.ProvinciaResidenza & ")"

            txtCognomeUtente.Text = oDettaglioAnagraficaUtente.Cognome & " " & oDettaglioAnagraficaUtente.Nome & " " & oDettaglioAnagraficaUtente.PartitaIva & " " & oDettaglioAnagraficaUtente.CodiceFiscale
            txtCodiceFiscaleUtente.Text = oDettaglioAnagraficaUtente.DataNascita
            txtCodFisUte.Text = oDettaglioAnagraficaUtente.CodiceFiscale
            'Response.Write("<script language='javascript' type='text/javascript'>GestAlert('a', 'warning', '', '', '" & txtCodFisUte.Text & "');")

            txtViaUtente.Text = oDettaglioAnagraficaUtente.ViaResidenza & " " & oDettaglioAnagraficaUtente.CivicoResidenza
            If Len(Trim(oDettaglioAnagraficaUtente.EsponenteCivicoResidenza)) <> 0 Then
                txtViaUtente.Text += "/" & Replace(oDettaglioAnagraficaUtente.EsponenteCivicoResidenza, "/", "")
            End If
            txtViaUtente.Text += " - " & oDettaglioAnagraficaUtente.CapResidenza & " " & oDettaglioAnagraficaUtente.ComuneResidenza & " (" & oDettaglioAnagraficaUtente.ProvinciaResidenza & ")"
            'End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DatiGenerali.LoadAnagrafica.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    Private Sub LoadDati(ByVal myContatore As objContatore)
        Try
            If myContatore.nIdAssenzaDatiCatastali <> -1 Then
                ddlAssenzaDatiCat.SelectedValue = myContatore.nIdAssenzaDatiCatastali
            End If

            If myContatore.nIdTipoUtenza = -1 Then
                ddlTipoUtenza.SelectedValue = ""
            Else
                ddlTipoUtenza.SelectedValue = myContatore.nIdTipoUtenza
            End If

            If myContatore.sTipoUnita = "" Then
                ddlTipoUnita.SelectedValue = "F"
            Else
                ddlTipoUnita.SelectedValue = myContatore.sTipoUnita
            End If

            If myContatore.nIdTitoloOccupazione = -1 Or myContatore.nIdTitoloOccupazione = 0 Then
                ddlTitOccupazione.SelectedValue = ""
            Else
                ddlTitOccupazione.SelectedValue = myContatore.nIdTitoloOccupazione
            End If
            myContatore.sStatoContatore = "-1"
            If Not IsDBNull(myContatore.sDataAttivazione) Then
                If myContatore.sDataAttivazione <> "" Then
                    myContatore.sStatoContatore = "ATT"
                End If
            End If
            If Not IsDBNull(myContatore.sDataCessazione) Then
                If myContatore.sDataCessazione <> "" Then
                    myContatore.sStatoContatore = "RIM"
                End If
            End If
            FncGen.SelectIndexDropDownList(cboStatoContatore, myContatore.sStatoContatore)
            'FncGen.SelectIndexDropDownList(cboPenalita, myContatore.sPenalita)

            txtMatricola.Text = myContatore.sMatricola
            txtSequenza.Text = myContatore.sSequenza
            txtProgressivo.Text = myContatore.sProgressivo
            txtLatoStrada.Text = myContatore.sLatoStrada
            txtNumeroUtenze.Text = myContatore.nNumeroUtenze

            Session("oListSubContatori") = myContatore.oListSubContatori

            txtNumeroUtente.Text = myContatore.sNumeroUtente
            txtNote.Text = myContatore.sNote

            If myContatore.sCivico <> "" And myContatore.sCivico <> "-1" And myContatore.sCivico <> "0" Then
                txtCivico.Text = myContatore.sCivico
            Else
                txtCivico.Text = ""
            End If

            txtEsponente.Text = myContatore.sEsponenteCivico
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''//
            chkEsenteFognatura.Checked = myContatore.bEsenteFognatura
            chkEsenteDepurazione.Checked = myContatore.bEsenteDepurazione
            chkEsenteAcqua.Checked = myContatore.bEsenteAcqua
            '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
            chkEsenteAcquaQF.Checked = myContatore.bEsenteAcquaQF
            chkEsenteDepQF.Checked = myContatore.bEsenteDepQF
            chkEsenteFogQF.Checked = myContatore.bEsenteFogQF

            chkEsenteFognatura.Checked = myContatore.bEsenteFognatura

            If chkEsenteFognatura.Checked = True Then
                cboFognatura.Enabled = False
            End If

            chkEsenteDepurazione.Checked = myContatore.bEsenteDepurazione

            If chkEsenteDepurazione.Checked = True Then
                cboDepurazione.Enabled = False
            End If

            Log.Debug("carica i dati in dati generali contatore")
            chkIgnoraMora.Checked = myContatore.bIgnoraMora
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''//
            txtDataAttivazione.Text = myContatore.sDataAttivazione
            txtDataSostituzione.Text = myContatore.sDataSostituzione
            txtDataRimTemp.Text = myContatore.sDataRimTemp
            txtDataCessazione.Text = myContatore.sDataCessazione

            chkUtenteSospeso.Checked = myContatore.bUtenteSospeso
            txtDataSospsensioneUtenza.Text = myContatore.sDataSospensioneUtenza
            txtNumeroCifreContatore.Text = myContatore.sCifreContatore
            txtQuoteAgevolate.Text = myContatore.sQuoteAgevolate
            txtCodiceFabbricatore.Text = myContatore.sCodiceFabbricante
            txtContatorePrecedente.Text = FncGrd.IntForGridView(myContatore.nIdContatorePrec)
            TxtMatricolaContatorePrecedente.Text = myContatore.sMatricolaContatorePrec
            txtContatoreSuccessivo.Text = FncGrd.IntForGridView(myContatore.nIdContatoreSucc)
            TxtMatricolaContatoreSuccessivo.Text = myContatore.sMatricolaContatoreSucc

            '=============================
            'INIZIO MODIFICHE ALE CAO
            '=============================
            'txtpiano.Text = DetailoContatore.oDatiCatastali.spiano
            'txtfoglio.Text = DetailoContatore.oDatiCatastali.sfoglio
            'txtnumero.Text = DetailContatore.oDatiCatastali.snumero
            'txtsubalterno.Text = DetailContatore.oDatiCatastali.nsubalterno

            txtspesaprev.Text = myContatore.nSpesa
            txtdirittisegr.Text = myContatore.nDiritti
            '=============================
            'FINE MODIFICHE ALE CAO
            '=============================

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''//
            txtEnteAppartenenza.Text = ConstSession.DescrizioneEnte

            HDTextCodUtente.Text = myContatore.nIdUtente
            HDtxtCodIntestatario.Text = myContatore.nIdIntestatario
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DatiGenerali.LoadDati.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** 20140923 - GIS ***
    Private Sub CmdGIS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdGIS.Click
        Dim CodeGIS, sScript, sRifPrec As String
        Dim fncGIS As New RemotingInterfaceAnater.GIS
        Dim listRifCat As New Generic.List(Of Anater.Oggetti.RicercaUnitaImmobiliareAnater)
        Dim myRifCat As New Anater.Oggetti.RicercaUnitaImmobiliareAnater
        Try
            sRifPrec = ""
            If Not Session("myContatore") Is Nothing Then
                For Each myRif As objDatiCatastali In CType(Session("myContatore"), objContatore).oDatiCatastali
                    If myRif.sFoglio <> "" Then
                        If sRifPrec <> myRif.sFoglio + "|" + myRif.sNumero + "|" + myRif.nSubalterno.ToString Then
                            myRifCat = New Anater.Oggetti.RicercaUnitaImmobiliareAnater
                            myRifCat.Foglio = myRif.sFoglio
                            myRifCat.Mappale = myRif.sNumero
                            myRifCat.Subalterno = myRif.nSubalterno.ToString
                            myRifCat.CodiceRicerca = ConstSession.Belfiore
                            listRifCat.Add(myRifCat)
                        End If
                    End If
                    sRifPrec = myRif.sFoglio + "|" + myRif.sNumero + "|" + myRif.nSubalterno.ToString
                Next
                If listRifCat.ToArray.Length > 0 Then
                    CodeGIS = fncGIS.getGIS(ConstSession.UrlWSGIS, listRifCat.ToArray())
                    If Not CodeGIS Is Nothing Then
                        sScript = "window.open('" & ConstSession.UrlWebGIS & CodeGIS & "','wdwGIS')"
                        RegisterScript(sScript, Me.GetType())
                    Else
                        sScript = "GestAlert('a', 'danger', '', '', 'Errore in interrogazione Cartografia!');"
                        RegisterScript(sScript, Me.GetType())
                    End If
                Else
                    sScript = "GestAlert('a', 'warning', '', '', 'Per accedere alla Cartografia avere almeno un foglio!')"
                    RegisterScript(sScript, Me.GetType())
                End If
            Else
                sScript = "GestAlert('a', 'warning', '', '', 'Per accedere alla Cartografia avere almeno un foglio!')"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DatiGenerali.CmdGIS_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** ***
End Class
