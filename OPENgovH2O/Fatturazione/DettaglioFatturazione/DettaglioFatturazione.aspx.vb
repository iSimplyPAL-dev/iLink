Imports log4net
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports RemotingInterfaceMotoreH2O.RemotingInterfaceMotoreH2O

Partial Class DettaglioFatturazione
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(DettaglioFatturazione))

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

	End Sub
    Protected WithEvents LblResult As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

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
    ''' <revision date="17/11/2014">
    ''' voce di costo specifica per utente
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Esportazione completa dati</em>
    ''' </revision>
    ''' </revisionHistory>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            Dim oListFattura() As ObjFattura
            Dim FunctionFattura As New ClsFatture
            Dim sScript As String = ""

            TxtPaginaComandiChiamante.Text = Request.Item("PaginaChiamante") & "&Provenienza=" & Request.Item("ProvenienzaChiamante")
            If Page.IsPostBack = False Then
                Dim paginacomandi As String = Request("paginacomandi")
                Dim parametri As String
                parametri = "?title=Acquedotto - Fatturazione - Dettaglio Fatturazione&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo & "&Provenienza=" & Request.Item("Provenienza")

                sScript += "document.getElementById('paginacomandi').value='" & paginacomandi & "';"
                sScript += "parent.Comandi.location.href='" & paginacomandi & parametri & "';"
                RegisterScript(sScript, Me.GetType())
                LoadCombos()
                'controllo se sono in visualizzazione di una posizione
                If Not Request.Item("IdDocumento") Is Nothing Then
                    'carico il documento
                    oListFattura = FunctionFattura.GetFattura(ConstSession.StringConnection, ConstSession.IdEnte, -1, Request.Item("IdDocumento"), False)
                    If Not oListFattura Is Nothing Then
                        LoadFattura(oListFattura(0))
                    End If
                End If
            End If
            sScript = ""
            sScript += "LoadScaglioni.location.href='DettFatturaScaglioni.aspx';"
            sScript += "LoadCanoni.location.href='DettFatturaCanoni.aspx';"
            sScript += "LoadNolo.location.href='DettFatturaNolo.aspx';"
            sScript += "LoadQuotaFissa.location.href='DettFatturaQuotaFissa.aspx';"
            sScript += "LoadDettaglioIva.location.href='DettFatturaDettaglioIva.aspx';"
            RegisterScript(sScript, Me.GetType())
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DettaglioFatturazione.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    'Put user code to initialize the page here
    '    Try
    '        Dim oListFattura() As ObjFattura
    '        Dim FunctionFattura As New ClsFatture
    '        Dim sScript As String = ""

    '        TxtPaginaComandiChiamante.Text = Request.Item("PaginaChiamante") & "&Provenienza=" & Request.Item("ProvenienzaChiamante")
    '        If Page.IsPostBack = False Then
    '            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
    '            Dim paginacomandi As String = Request("paginacomandi")
    '            Dim parametri As String
    '            parametri = "?title=Acquedotto - Fatturazione - Dettaglio Fatturazione&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo & "&Provenienza=" & Request.Item("Provenienza")

    '            sScript += "document.getElementById('paginacomandi').value='" & paginacomandi & "';"
    '            sScript += "parent.Comandi.location.href='" & paginacomandi & parametri & "';"
    '            RegisterScript(sScript, Me.GetType())
    '            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
    '            LoadCombos()
    '            'controllo se sono in visualizzazione di una posizione
    '            If Not Request.Item("IdDocumento") Is Nothing Then
    '                'carico il documento
    '                oListFattura = FunctionFattura.GetFattura(ConstSession.IdEnte, -1, Request.Item("IdDocumento"), False)
    '                If Not oListFattura Is Nothing Then
    '                    LoadFattura(oListFattura(0))
    '                End If
    '            End If
    '        End If
    '        sScript = ""
    '        sScript += "LoadScaglioni.location.href='DettFatturaScaglioni.aspx';"
    '        sScript += "LoadCanoni.location.href='DettFatturaCanoni.aspx';"
    '        sScript += "LoadNolo.location.href='DettFatturaNolo.aspx';"
    '        sScript += "LoadQuotaFissa.location.href='DettFatturaQuotaFissa.aspx';"
    '        '*** 20141117 - voce di costo specifica per utente ***
    '        'sScript += "LoadAddizionali.location.href='DettFatturaAddizionali.aspx';"
    '        '*** ***
    '        sScript += "LoadDettaglioIva.location.href='DettFatturaDettaglioIva.aspx';"
    '        RegisterScript(sScript, Me.GetType())
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DettaglioFatturazione.Page_Load.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' '
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="22/11/2017">
    ''' calcolo quota fissa acqua+depurazione+fognatura
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="17/11/2014">
    ''' voce di costo specifica per utente
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Private Sub CmdModifica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdModifica.Click
        Dim sScript As String = ""
        Dim oListCmd() As Object
        Dim x As Integer
        Try
            Session("sOldFattura") = TxtNUtenze.Text + "|" + TxtDataLettPrec.Text + "|" + TxtLettPrec.Text + "|" + TxtDataLettAtt.Text + "|" + TxtLettAtt.Text + "|" + TxtConsumo.Text + "|" + TxtGiorni.Text + "|" + DdlTipoUtenza.SelectedValue + "|" + DdlTipoContatore.SelectedValue + "|" + DdlFognatura.SelectedValue + "|" + DdlDepurazione.SelectedValue + "|" + ChkEsenAcqua.Checked.ToString + "|" + ChkEsenFognatura.Checked.ToString + "|" + ChkEsenDepurazione.Checked.ToString + "|" + ChkNoloUnaTantum.Checked.ToString
            Session("sOldFattura") += "|" + ChkEsenAcquaQF.Checked.ToString + "|" + ChkEsenDepQF.Checked.ToString + "|" + ChkEsenFogQF.Checked.ToString
            If Not CType(Session("oDettFattura"), ObjFattura).oAddizionali Is Nothing Then
                Session("sOldFattura") += "|" + CType(Session("oDettFattura"), ObjFattura).oAddizionali.GetUpperBound(0).ToString
            End If
            Abilita(False, 1)

            'se passo da modifica a variazione devo disabilitare il pulsante di modifica/cancellazione e abilitare il pulsante di salvataggio
            ReDim Preserve oListCmd(0)
            oListCmd(0) = "Modifica"
            For x = 0 To oListCmd.Length - 1
                sScript += "$('#" + oListCmd(x).ToString() + "', parent.frames['Comandi'].document).addClass('DisableBtn');"
            Next
            ReDim Preserve oListCmd(1)
            oListCmd(1) = "Ricalcolo"
            For x = 1 To oListCmd.Length - 1
                sScript += "$('#" + oListCmd(x).ToString() + "', parent.frames['Comandi'].document).removeClass('DisableBtn');"
            Next
            RegisterScript(sScript, Me.GetType())
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DettaglioFatturazione.cmdModifica_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub CmdModifica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdModifica.Click
    '    Dim sScript As String
    '    Dim oListCmd() As Object
    '    Dim x As Integer
    '    Try
    '        '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
    '        Session("sOldFattura") = TxtNUtenze.Text + "|" + TxtDataLettPrec.Text + "|" + TxtLettPrec.Text + "|" + TxtDataLettAtt.Text + "|" + TxtLettAtt.Text + "|" + TxtConsumo.Text + "|" + TxtGiorni.Text + "|" + DdlTipoUtenza.SelectedValue + "|" + DdlTipoContatore.SelectedValue + "|" + DdlFognatura.SelectedValue + "|" + DdlDepurazione.SelectedValue + "|" + ChkEsenAcqua.Checked.ToString + "|" + ChkEsenFognatura.Checked.ToString + "|" + ChkEsenDepurazione.Checked.ToString + "|" + ChkNoloUnaTantum.Checked.ToString
    '        Session("sOldFattura") += "|" + ChkEsenAcquaQF.Checked.ToString + "|" + ChkEsenDepQF.Checked.ToString + "|" + ChkEsenFogQF.Checked.ToString
    '        '*** ***
    '        '*** 20141117 - voce di costo specifica per utente ***
    '        If Not CType(Session("oDettFattura"), ObjFattura).oAddizionali Is Nothing Then
    '            Session("sOldFattura") += "|" + CType(Session("oDettFattura"), ObjFattura).oAddizionali.GetUpperBound(0).ToString
    '        End If
    '        '*** ***
    '        Abilita(False, 1)

    '        'se passo da modifica a variazione devo disabilitare il pulsante di modifica/cancellazione e abilitare il pulsante di salvataggio
    '        ReDim Preserve oListCmd(0)
    '        oListCmd(0) = "Modifica"
    '        For x = 0 To oListCmd.Length - 1
    '            sScript += "parent.Comandi.document.getElementById('" & oListCmd(x).ToString() & "').disabled=" & True.ToString.ToLower & ";"
    '        Next
    '        ReDim Preserve oListCmd(1)
    '        oListCmd(1) = "Ricalcolo"
    '        For x = 1 To oListCmd.Length - 1
    '            sScript += "parent.Comandi.document.getElementById('" & oListCmd(x).ToString() & "').disabled=" & False.ToString.ToLower & ";"
    '        Next
    '        RegisterScript(sScript, Me.GetType())
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DettaglioFatturazione.cmdModifica_Click.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Private Sub CmdRicalcolo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdRicalcolo.Click
    '    Dim culture As IFormatProvider
    '    culture = New System.Globalization.CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")
    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Try
    '        Dim oMyRuoloH2O As New ObjTotRuoloFatture
    '        Dim sNewFattura As String
    '        Dim oMyFatturaOrg As New ObjFattura
    '        Dim oMyFattura As New ObjFattura
    '        Dim FunctionFatture As New ClsFatture
    '        Dim oMyContatore As New objContatore
    '        Dim oListLetture() As ObjLettura
    '        Dim oMyLettura As New ObjLettura
    '        Dim nList As Integer = -1
    '        Dim TypeOfRI As Type = GetType(IH2O)
    '        Dim RemoRuoloH2O As IH2O
    '        Dim FunctionTariffe As New ClsTariffe
    '        Dim oMyTariffe As New ObjTariffe
    '        Dim sScript, WFErrore As String
    '        Dim nBaseTempo As Integer = System.Configuration.ConfigurationManager.AppSettings("BaseTempo")
    '        Dim FncLetture As New clsLetture
    '        Dim FncAnagrafica As Anagrafica.DLL.GestioneAnagrafica
    '        Dim FncPeriodo As New TabelleDiDecodifica.DBPeriodo
    '        Dim nArrotondConsumo As Integer

    '        'inizializzo la connessione
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If
    '        FncAnagrafica = New Anagrafica.DLL.GestioneAnagrafica(WFSessione.oSession, ConfigurationManager.AppSettings("PARAMETRO_ANAGRAFICA"))
    '        'controllo se sono state fatte delle modifiche
    '        '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
    '        sNewFattura = TxtNUtenze.Text + "|" + TxtDataLettPrec.Text + "|" + TxtLettPrec.Text + "|" + TxtDataLettAtt.Text + "|" + TxtLettAtt.Text + "|" + TxtConsumo.Text + "|" + TxtGiorni.Text + "|" + DdlTipoUtenza.SelectedValue + "|" + DdlTipoContatore.SelectedValue + "|" + DdlFognatura.SelectedValue + "|" + DdlDepurazione.SelectedValue + "|" + ChkEsenAcqua.Checked.ToString + "|" + ChkEsenFognatura.Checked.ToString + "|" + ChkEsenDepurazione.Checked.ToString + "|" + ChkNoloUnaTantum.Checked.ToString
    '        sNewFattura += "|" + ChkEsenAcquaQF.Checked.ToString + "|" + ChkEsenDepQF.Checked.ToString + "|" + ChkEsenFogQF.Checked.ToString
    '        '*** ***
    '        If sNewFattura.CompareTo(Session("sOldFattura")) <> 0 Then
    '            'carico i dati originali
    '            oMyFatturaOrg = Session("oDettFattura")
    '            'attivo il servizio
    '            RemoRuoloH2O = Activator.GetObject(TypeOfRI, ConfigurationManager.AppSettings("URLServizioRuoloH2O"))
    '            'carico i dati della pagina
    '            nList += 1
    '            oMyLettura.nIdPeriodo = ConstSession.IdPeriodo
    '            If TxtCodUtente.Text <> "" Then
    '                oMyLettura.nIdUtente = CInt(TxtCodUtente.Text)
    '            End If
    '            oMyLettura.sNUtente = LblNUtente.Text.Replace("Numero Utente ", "")
    '            oMyLettura.tDataLetturaAtt = TxtDataLettAtt.Text
    '            oMyLettura.nLetturaAtt = CInt(TxtLettAtt.Text)
    '            oMyLettura.tDataLetturaPrec = TxtDataLettPrec.Text
    '            oMyLettura.nLetturaPrec = CInt(TxtLettPrec.Text)
    '            If TxtSubConsumo.Text <> "" Then
    '                oMyLettura.nConsumoSubContatore = CInt(TxtSubConsumo.Text)
    '            End If
    '            'prelevo il tipo di arrotondamento da applicare al consumo
    '            nArrotondConsumo = FncPeriodo.GetArrotondamentoConsumo(oMyLettura.nIdPeriodo, WFSessione)
    '            If nArrotondConsumo = -1 Then
    '                sScript = "GestAlert('a', 'danger', '', '', 'Errore nel prelavare il tipo di arrotondamento!');"
    '                RegisterScript(sScript , Me.GetType())"CmdRicalcolo", "" & sScript & "")
    '                Exit Sub
    '            End If
    '            oMyLettura.nTipoArrotondConsumo = nArrotondConsumo
    '            oMyContatore.nFondoScala = FncLetture.GetFondoScala(DdlTipoContatore.SelectedValue)
    '            'controllo se calcolare il consumo
    '            If oMyLettura.nConsumo = -1 Then
    '                oMyLettura.nConsumo = RemoRuoloH2O.CalcolaConsumo(oMyLettura.nLetturaPrec, oMyLettura.nLetturaAtt, oMyLettura.nConsumoSubContatore, oMyContatore.nFondoScala)
    '                If oMyLettura.nConsumo = -1 Then
    '                    sScript = "GestAlert('a', 'danger', '', '', 'Errore nel calcolo del consumo!');"
    '                    RegisterScript(sScript , Me.GetType())"CmdRicalcolo", "" & sScript & "")
    '                    Exit Sub
    '                End If
    '            End If
    '            'controllo se calcolare i giorni
    '            If oMyLettura.nGiorni = -1 Then
    '                oMyLettura.nGiorni = RemoRuoloH2O.CalcolaGiorni(oMyLettura.tDataLetturaPrec, oMyLettura.tDataLetturaAtt, nBaseTempo)
    '                If oMyLettura.nGiorni = -1 Then
    '                    sScript = "GestAlert('a', 'danger', '', '', 'Errore nel calcolo del periodo!');"
    '                    RegisterScript(sScript , Me.GetType())"CmdRicalcolo", "" & sScript & "")
    '                    Exit Sub
    '                End If
    '            End If
    '            ReDim Preserve oMyContatore.oListLetture(nList)
    '            oMyContatore.oListLetture(nList) = oMyLettura

    '            oMyContatore.sIdEnte = ConstSession.IdEnte
    '            If TxtCodIntestatario.Text <> "" Then
    '                oMyContatore.nIdIntestatario = CInt(TxtCodIntestatario.Text)
    '            End If
    '            oMyContatore.oAnagUtente = FncAnagrafica.GetAnagrafica(oMyLettura.nIdUtente, -1, ConstSession.StringConnectionAnagrafica, False) 'FncAnagrafica.GetAnagrafica(oMyLettura.nIdUtente, Session("COD_TRIBUTO"), -1)
    '            oMyContatore.nTipoContatore = CInt(DdlTipoContatore.SelectedValue)
    '            oMyContatore.nTipoUtenza = CInt(DdlTipoUtenza.SelectedValue)
    '            oMyContatore.nNumeroUtenze = CInt(TxtNUtenze.Text)
    '            oMyContatore.sMatricola = TxtMatricola.Text
    '            oMyContatore.sUbicazione = TxtVia.Text
    '            oMyContatore.sCivico = TxtCivico.Text
    '            oMyContatore.sFrazione = TxtFrazione.Text
    '            oMyContatore.bEsenteAcqua = ChkEsenAcqua.Checked
    '            If DdlDepurazione.SelectedValue <> "" Then
    '                oMyContatore.nCodDepurazione = CInt(DdlDepurazione.SelectedValue)
    '            End If
    '            oMyContatore.bEsenteDepurazione = ChkEsenDepurazione.Checked
    '            If DdlFognatura.SelectedValue <> "" Then
    '                oMyContatore.nCodFognatura = CInt(DdlFognatura.SelectedValue)
    '            End If
    '            oMyContatore.bEsenteFognatura = ChkEsenFognatura.Checked

    '            oMyFattura.sIdEnte = oMyContatore.sIdEnte
    '            oMyFattura.nIdPeriodo = oMyLettura.nIdPeriodo
    '            oMyFattura.sAnno = CStr(ConstSession.DescrPeriodo).Substring(0, 4)
    '            oMyFattura.oAnagrafeUtente = oMyContatore.oAnagUtente
    '            oMyFattura.nIdIntestatario = oMyContatore.nIdIntestatario
    '            oMyFattura.nIdUtente = oMyLettura.nIdUtente
    '            oMyFattura.sNUtente = oMyLettura.sNUtente
    '            oMyFattura.sMatricola = oMyContatore.sMatricola
    '            oMyFattura.sViaContatore = oMyContatore.sUbicazione
    '            oMyFattura.sCivicoContatore = oMyContatore.sCivico
    '            oMyFattura.sFrazioneContatore = oMyContatore.sFrazione
    '            oMyFattura.nTipoContatore = oMyContatore.nTipoContatore
    '            oMyFattura.nUtenze = oMyContatore.nNumeroUtenze
    '            oMyFattura.nTipoUtenza = oMyContatore.nTipoUtenza
    '            oMyFattura.tDataLetturaPrec = oMyLettura.tDataLetturaPrec
    '            oMyFattura.nLetturaPrec = oMyLettura.nLetturaPrec
    '            oMyFattura.tDataLetturaAtt = oMyLettura.tDataLetturaAtt
    '            oMyFattura.nLetturaAtt = oMyLettura.nLetturaAtt
    '            oMyFattura.nConsumo = oMyLettura.nConsumo
    '            oMyFattura.nGiorni = oMyLettura.nGiorni
    '            oMyFattura.nCodDepurazione = oMyContatore.nCodDepurazione
    '            oMyFattura.nCodFognatura = oMyContatore.nCodFognatura
    '            oMyFattura.bEsenteAcqua = ChkEsenAcqua.Checked
    '            oMyFattura.bEsenteDepurazione = oMyContatore.bEsenteDepurazione
    '            oMyFattura.bEsenteFognatura = oMyContatore.bEsenteFognatura
    '            '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
    '            oMyFattura.bEsenteAcquaQF = ChkEsenAcquaQF.Checked
    '            oMyFattura.bEsenteDepQF = ChkEsenDepQF.Checked
    '            oMyFattura.bEsenteFogQF = ChkEsenFogQF.Checked
    '            oMyContatore.bEsenteAcquaQF = ChkEsenAcquaQF.Checked
    '            oMyContatore.bEsenteDepQF = ChkEsenDepQF.Checked
    '            oMyContatore.bEsenteFogQF = ChkEsenFogQF.Checked
    '            '*** ***

    '            'prelevo le tariffe
    '            oMyTariffe = FunctionTariffe.GetTariffe(oMyContatore.sIdEnte, oMyFattura.sAnno, Nothing)
    '            If oMyTariffe Is Nothing Then
    '                sScript = "GestAlert('a', 'warning', '', '', 'Mancano le tariffe!');"
    '                RegisterScript(sScript , Me.GetType())"CmdRicalcolo", "" & sScript & "")
    '                Exit Sub
    '            End If
    '            'calcolo gli importi
    '            If RemoRuoloH2O.CalcolaDovutoH2O(ConfigurationManager.AppSettings("forzaquotafissa"), oMyContatore, oMyTariffe, ConstSession.UserName, ChkNoloUnaTantum.Checked, oMyFattura) = 0 Then
    '                sScript = "GestAlert('a', 'danger', '', '', 'Errore nel calcolo degli importi!');"
    '                RegisterScript(sScript , Me.GetType())"CmdRicalcolo", "" & sScript & "")
    '                Exit Sub
    '            End If
    '            'determino i nuovi importi/consumi per differenza rispetto a quelli
    '            If Request.Item("Provenienza") = "E" Then
    '                oMyFattura.nIdFlusso = oMyFatturaOrg.nIdFlusso
    '            End If
    '            'determino il tipo documento
    '            If oMyFattura.impFattura < 0 Then
    '                oMyFattura.sTipoDocumento = "N"
    '            Else
    '                oMyFattura.sTipoDocumento = "F"
    '            End If
    '            If LblDataDoc.Text.ToString <> "" Then
    '                oMyFattura.tDataDocumentoRif = LblDataDoc.Text.ToString.Replace(" DEL ", "")
    '            End If
    '            oMyFattura.sNDocumentoRif = LblNDoc.Text.ToString.Replace("N. ", "")
    '            oMyFattura.oLetture = oMyFatturaOrg.oLetture
    '            oMyFattura.sOperatore = Session("username")
    '            oMyFattura.tDataInserimento = Now
    '            'inserisco il documento
    '            If FunctionFatture.SetFatturaCompleta(oMyFattura, WFSessione) = 0 Then
    '                sScript = "GestAlert('a', 'danger', '', '', 'Errore in inserimento variazione!');"
    '            Else
    '                WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE"))
    '                If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '                    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '                End If
    '                oMyRuoloH2O = Session("oRuoloH2O")
    '                If DeleteFattura(oMyFatturaOrg, oMyRuoloH2O, WFSessione) = False Then
    '                    sScript = "GestAlert('a', 'danger', '', '', 'Errore in inserimento annullo!');"
    '                Else
    '                    sScript = "GestAlert('a', 'warning', '', '', 'Variazione registrata con successo!');"
    '                    sScript += "parent.Visualizza.location.href='" & TxtPaginaComandiChiamante.Text & "';"
    '                End If
    '            End If
    '            RegisterScript(sScript , Me.GetType())"CmdRicalcolo", "" & sScript & "")
    '        Else
    '            sScript = "GestAlert('a', 'warning', '', '', 'Non sono state fatte Variazioni!');"
    '            RegisterScript(sScript , Me.GetType())"CmdRicalcolo", "" & sScript & "")
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DettaglioFatturazione.CmdRicalcolo_Click.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    Finally
    '        WFSessione.Kill()
    '    End Try
    'End Sub
    Private Sub CmdRicalcolo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdRicalcolo.Click
        Try
            Dim oMyRuoloH2O As New ObjTotRuoloFatture
            Dim sNewFattura As String
            Dim oMyFatturaOrg As New ObjFattura
            Dim oMyFattura As New ObjFattura
            Dim FunctionFatture As New ClsFatture
            Dim oMyContatore As New objContatore
            Dim oMyLettura As New ObjLettura
            Dim nList As Integer = -1
            Dim TypeOfRI As Type = GetType(IH2O)
            Dim RemoRuoloH2O As IH2O
            Dim FunctionTariffe As New ClsTariffe
            Dim oMyTariffe As New ObjTariffe
            Dim sScript As String
            Dim FncAnagrafica As New Anagrafica.DLL.GestioneAnagrafica
            Dim FncPeriodo As New TabelleDiDecodifica.DBPeriodo
            Dim nArrotondConsumo As Integer

            'controllo se sono state fatte delle modifiche
            '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
            sNewFattura = TxtNUtenze.Text + "|" + TxtDataLettPrec.Text + "|" + TxtLettPrec.Text + "|" + TxtDataLettAtt.Text + "|" + TxtLettAtt.Text + "|" + TxtConsumo.Text + "|" + TxtGiorni.Text + "|" + DdlTipoUtenza.SelectedValue + "|" + DdlTipoContatore.SelectedValue + "|" + DdlFognatura.SelectedValue + "|" + DdlDepurazione.SelectedValue + "|" + ChkEsenAcqua.Checked.ToString + "|" + ChkEsenFognatura.Checked.ToString + "|" + ChkEsenDepurazione.Checked.ToString + "|" + ChkNoloUnaTantum.Checked.ToString
            sNewFattura += "|" + ChkEsenAcquaQF.Checked.ToString + "|" + ChkEsenDepQF.Checked.ToString + "|" + ChkEsenFogQF.Checked.ToString
            '*** ***
            '*** 20141117 - voce di costo specifica per utente ***
            If Not CType(Session("oDettFattura"), ObjFattura).oAddizionali Is Nothing Then
                sNewFattura += "|" + CType(Session("oDettFattura"), ObjFattura).oAddizionali.GetUpperBound(0).ToString
            End If
            '*** ***
            If sNewFattura.CompareTo(Session("sOldFattura")) <> 0 Then
                'carico i dati originali
                oMyFatturaOrg = Session("oDettFatturaOrg")
                'attivo il servizio
                RemoRuoloH2O = Activator.GetObject(TypeOfRI, ConstSession.UrlMotoreH2O)
                'carico i dati della pagina
                nList += 1
                oMyLettura.nIdPeriodo = ConstSession.IdPeriodo
                If TxtCodUtente.Text <> "" Then
                    oMyLettura.nIdUtente = CInt(TxtCodUtente.Text)
                End If
                oMyLettura.sNUtente = LblNUtente.Text.Replace("Numero Utente ", "")
                oMyLettura.tDataLetturaAtt = TxtDataLettAtt.Text
                oMyLettura.nLetturaAtt = CInt(TxtLettAtt.Text)
                oMyLettura.tDataLetturaPrec = TxtDataLettPrec.Text
                oMyLettura.nLetturaPrec = CInt(TxtLettPrec.Text)
                If TxtSubConsumo.Text <> "" Then
                    oMyLettura.nConsumoSubContatore = CInt(TxtSubConsumo.Text)
                End If
                'prelevo il tipo di arrotondamento da applicare al consumo
                nArrotondConsumo = FncPeriodo.GetArrotondamentoConsumo(oMyLettura.nIdPeriodo)
                If nArrotondConsumo = -1 Then
                    sScript = "GestAlert('a', 'danger', '', '', 'Errore nel prelavare il tipo di arrotondamento!');"
                    RegisterScript(sScript, Me.GetType())
                    Exit Sub
                End If
                oMyLettura.nTipoArrotondConsumo = nArrotondConsumo
                oMyContatore.nFondoScala = New GestLetture().GetFondoScala(DdlTipoContatore.SelectedValue, -1)
                'controllo se calcolare il consumo
                If oMyLettura.nConsumo = -1 Then
                    oMyLettura.nConsumo = RemoRuoloH2O.CalcolaConsumo(oMyLettura.nLetturaPrec, oMyLettura.nLetturaAtt, oMyLettura.nConsumoSubContatore, oMyContatore.nFondoScala)
                    If oMyLettura.nConsumo = -1 Then
                        sScript = "GestAlert('a', 'danger', '', '', 'Errore nel calcolo del consumo!');"
                        RegisterScript(sScript, Me.GetType())
                        Exit Sub
                    End If
                End If
                'controllo se calcolare i giorni
                If oMyLettura.nGiorni = -1 Then
                    oMyLettura.nGiorni = RemoRuoloH2O.CalcolaGiorni(oMyLettura.tDataLetturaPrec, oMyLettura.tDataLetturaAtt, ConstSession.BaseTempo)
                    If oMyLettura.nGiorni = -1 Then
                        sScript = "GestAlert('a', 'danger', '', '', 'Errore nel calcolo del periodo!');"
                        RegisterScript(sScript, Me.GetType())
                        Exit Sub
                    End If
                End If
                ReDim Preserve oMyContatore.oListLetture(nList)
                oMyContatore.oListLetture(nList) = oMyLettura

                oMyContatore.sIdEnte = ConstSession.IdEnte
                If TxtCodIntestatario.Text <> "" Then
                    oMyContatore.nIdIntestatario = CInt(TxtCodIntestatario.Text)
                End If
                '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
                oMyContatore.oAnagUtente = FncAnagrafica.GetAnagrafica(oMyLettura.nIdUtente, -1, String.Empty, ConstSession.DBType, ConstSession.StringConnectionAnagrafica, False) 'FncAnagrafica.GetAnagrafica(oMyLettura.nIdUtente, Session("COD_TRIBUTO"), -1)
                oMyContatore.nTipoContatore = CInt(DdlTipoContatore.SelectedValue)
                oMyContatore.nTipoUtenza = CInt(DdlTipoUtenza.SelectedValue)
                oMyContatore.nNumeroUtenze = CInt(TxtNUtenze.Text)
                oMyContatore.sMatricola = TxtMatricola.Text
                oMyContatore.sUbicazione = TxtVia.Text
                oMyContatore.sCivico = TxtCivico.Text
                oMyContatore.sFrazione = TxtFrazione.Text
                oMyContatore.bEsenteAcqua = ChkEsenAcqua.Checked
                If DdlDepurazione.SelectedValue <> "" Then
                    oMyContatore.nCodDepurazione = CInt(DdlDepurazione.SelectedValue)
                End If
                oMyContatore.bEsenteDepurazione = ChkEsenDepurazione.Checked
                If DdlFognatura.SelectedValue <> "" Then
                    oMyContatore.nCodFognatura = CInt(DdlFognatura.SelectedValue)
                End If
                oMyContatore.bEsenteFognatura = ChkEsenFognatura.Checked

                oMyFattura.sIdEnte = oMyContatore.sIdEnte
                oMyFattura.nIdPeriodo = oMyLettura.nIdPeriodo
                oMyFattura.sAnno = CStr(ConstSession.DescrPeriodo).Substring(0, 4)
                oMyFattura.oAnagrafeUtente = oMyContatore.oAnagUtente
                oMyFattura.nIdIntestatario = oMyContatore.nIdIntestatario
                oMyFattura.nIdUtente = oMyLettura.nIdUtente
                oMyFattura.sNUtente = oMyLettura.sNUtente
                oMyFattura.sMatricola = oMyContatore.sMatricola
                oMyFattura.sViaContatore = oMyContatore.sUbicazione
                oMyFattura.sCivicoContatore = oMyContatore.sCivico
                oMyFattura.sFrazioneContatore = oMyContatore.sFrazione
                oMyFattura.nTipoContatore = oMyContatore.nTipoContatore
                oMyFattura.nUtenze = oMyContatore.nNumeroUtenze
                oMyFattura.nTipoUtenza = oMyContatore.nTipoUtenza
                oMyFattura.tDataLetturaPrec = oMyLettura.tDataLetturaPrec
                oMyFattura.nLetturaPrec = oMyLettura.nLetturaPrec
                oMyFattura.tDataLetturaAtt = oMyLettura.tDataLetturaAtt
                oMyFattura.nLetturaAtt = oMyLettura.nLetturaAtt
                oMyFattura.nConsumo = oMyLettura.nConsumo
                oMyFattura.nGiorni = oMyLettura.nGiorni
                oMyFattura.nCodDepurazione = oMyContatore.nCodDepurazione
                oMyFattura.nCodFognatura = oMyContatore.nCodFognatura
                oMyFattura.bEsenteAcqua = ChkEsenAcqua.Checked
                oMyFattura.bEsenteDepurazione = oMyContatore.bEsenteDepurazione
                oMyFattura.bEsenteFognatura = oMyContatore.bEsenteFognatura
                '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
                oMyFattura.bEsenteAcquaQF = ChkEsenAcquaQF.Checked
                oMyFattura.bEsenteDepQF = ChkEsenDepQF.Checked
                oMyFattura.bEsenteFogQF = ChkEsenFogQF.Checked
                oMyContatore.bEsenteAcquaQF = ChkEsenAcquaQF.Checked
                oMyContatore.bEsenteDepQF = ChkEsenDepQF.Checked
                oMyContatore.bEsenteFogQF = ChkEsenFogQF.Checked
                '*** ***
                'prelevo le tariffe
                oMyTariffe = FunctionTariffe.GetTariffe(oMyContatore.sIdEnte, oMyFattura.sAnno, Nothing)
                If oMyTariffe Is Nothing Then
                    sScript = "GestAlert('a', 'warning', '', '', 'Mancano le tariffe!');"
                    RegisterScript(sScript, Me.GetType())
                    Exit Sub
                End If

                'M.B. innesto per verificare se addizionali di o
                'Dim fatturatemp As ObjFattura
                'fatturatemp = CType(Session("oDettFattura"), ObjFattura)

                'sostituisco le addizionali con quanto presente in griglia
                'LoadTariffeAddizionali(CType(Session("oDettFattura"), ObjFattura), oMyTariffe)
                'calcolo gli importi
                If RemoRuoloH2O.CalcolaDovutoH2O(ConfigurationManager.AppSettings("forzaquotafissa"), oMyContatore, oMyTariffe, ConstSession.UserName, ChkNoloUnaTantum.Checked, oMyFattura) = 0 Then
                    sScript = "GestAlert('a', 'danger', '', '', 'Errore nel calcolo degli importi!');"
                    RegisterScript(sScript, Me.GetType())
                    Exit Sub
                End If
                'determino i nuovi importi/consumi per differenza rispetto a quelli
                If Request.Item("Provenienza") = "E" Then
                    oMyFattura.nIdFlusso = oMyFatturaOrg.nIdFlusso
                End If
                'determino il tipo documento
                If oMyFattura.impFattura < 0 Then
                    oMyFattura.sTipoDocumento = "N"
                Else
                    oMyFattura.sTipoDocumento = "F"
                End If
                If LblDataDoc.Text.ToString <> "" Then
                    oMyFattura.tDataDocumentoRif = LblDataDoc.Text.ToString.Replace(" DEL ", "")
                End If
                oMyFattura.sNDocumentoRif = LblNDoc.Text.ToString.Replace("N. ", "")
                oMyFattura.oLetture = oMyFatturaOrg.oLetture
                oMyFattura.sOperatore = Session("username")
                oMyFattura.tDataInserimento = Now
                'inserisco il documento
                If FunctionFatture.SetFatturaCompleta(oMyFattura) = 0 Then
                    sScript = "GestAlert('a', 'danger', '', '', 'Errore in inserimento variazione!');"
                Else
                    oMyRuoloH2O = Session("oRuoloH2O")
                    If DeleteFattura(oMyFatturaOrg, oMyRuoloH2O) = False Then
                        sScript = "GestAlert('a', 'danger', '', '', 'Errore in inserimento annullo!');"
                    Else
                        sScript = "GestAlert('a', 'success', '', '', 'Variazione registrata con successo!');"
                        sScript += "parent.Visualizza.location.href='" & TxtPaginaComandiChiamante.Text & "';"
                    End If
                End If
                RegisterScript(sScript, Me.GetType())
            Else
                sScript = "GestAlert('a', 'warning', '', '', 'Non sono state fatte Variazioni!');"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DettaglioFatturazione.CmdRicalcolo_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    'Private Sub CmdAnnullo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdAnnullo.Click
    '    Dim culture As IFormatProvider
    '    culture = New System.Globalization.CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")
    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Try
    '        Dim oMyFattura As New ObjFattura
    '        Dim sScript, WFErrore As String
    '        Dim oMyRuoloH2O As New ObjTotRuoloFatture

    '        'inizializzo la connessione
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If
    '        'carico i dati originali
    '        oMyRuoloH2O = Session("oRuoloH2O")
    '        oMyFattura = Session("oDettFattura")
    '        'controllo se generare la nota di annullo o eliminare il documento creato
    '        If DeleteFattura(oMyFattura, oMyRuoloH2O, WFSessione) = False Then
    '            sScript = "GestAlert('a', 'danger', '', '', 'Errore in inserimento annullo!');"
    '        Else
    '            sScript = "GestAlert('a', 'warning', '', '', 'Annullo registrato con successo!');"
    '            sScript += "parent.Visualizza.location.href='" & TxtPaginaComandiChiamante.Text & "';"
    '        End If
    '        RegisterScript(sScript , Me.GetType())"CmdAnnullo", "" & sScript & "")
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DettaglioFatturazione.CmdRAnnullo_Click.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    Finally
    '        WFSessione.Kill()
    '    End Try
    'End Sub
    Private Sub CmdAnnullo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdAnnullo.Click
        Try
            Dim oMyFattura As New ObjFattura
            Dim sScript As String
            Dim oMyRuoloH2O As New ObjTotRuoloFatture

            'carico i dati originali
            oMyRuoloH2O = Session("oRuoloH2O")
            oMyFattura = Session("oDettFattura")
            'controllo se generare la nota di annullo o eliminare il documento creato
            If DeleteFattura(oMyFattura, oMyRuoloH2O) = False Then
                sScript = "GestAlert('a', 'danger', '', '', 'Errore in inserimento annullo!');"
            Else
                sScript = "GestAlert('a', 'success', '', '', 'Annullo registrato con successo!');"
                sScript += "parent.Visualizza.location.href='" & TxtPaginaComandiChiamante.Text & "';"
            End If
            RegisterScript(sScript, Me.GetType())
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DettaglioFatturazione.CmdAnnullo_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Pulsante per la ristampa del documento.
    ''' </summary>
    ''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    ''' <revisionHistory><revision date="16/03/2021">Aggiunta la possibilità di ristampa fattura</revision></revisionHistory>
    Private Sub CmdStampaDoc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdStampaDoc.Click
        Dim sScript, sTypeOrd, sNameModello As String
        Dim nTipoElab As Integer = 1
        Dim nReturn, nMaxDocPerFile As Integer
        Dim oMyRuolo As New ObjTotRuoloFatture
        Dim oListDocStampati() As RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL
        Dim bCreaPDF As Boolean = False
        Dim nDecimal As Integer = 2
        Dim TipoStampaBollettini As String = ""
        Dim bSendByMail As Boolean = False
        Dim oListAvvisi() As ObjAnagDocumenti

        Try
            oListDocStampati = Nothing

            Session.Remove("ELENCO_DOCUMENTI_STAMPATI")
            sTypeOrd = "Nominativo"
            sNameModello = "Modello_Fattura_Acquedotto"
            nMaxDocPerFile = 1

            Try
                oListAvvisi = New ClsElaborazioneDocumenti().ConvAvvisi(New ClsFatture().GetFattura(ConstSession.StringConnection, ConstSession.IdEnte, -1, Request.Item("IdDocumento"), False))
                nReturn = New ClsElaborazioneDocumenti().ElaboraDocumenti(ConstSession.CodTributo, ConstSession.IdEnte, oMyRuolo.IdFlusso, ConstSession.DescrPeriodo.Substring(0, 4), ConstSession.StringConnection, ConstSession.StringConnectionOPENgov, ConstSession.StringConnectionAnagrafica, ConstSession.PathStampe, ConstSession.PathVirtualStampe, -1, -1, nTipoElab, sTypeOrd, sNameModello, nMaxDocPerFile, False, oListAvvisi, oListDocStampati, bCreaPDF, nDecimal, TipoStampaBollettini, "", bSendByMail)
                '*** ***
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DettaglioFatturazione.CmdElaborazione_Click.errore: ", Err)
                Response.Redirect("../../PaginaErrore.aspx")
            End Try

            If Not oListDocStampati Is Nothing Then
                Session.Add("ELENCO_DOCUMENTI_STAMPATI", oListDocStampati)
            End If

            If nReturn = 0 Then
                'si è verificato uin errore
                sScript = "GestAlert('a', 'warning', '', '', 'Errore in estrazione fatture elettroniche!');"
                RegisterScript(sScript, Me.GetType())
                Exit Sub
            Else
                sScript = "document.getElementById('DivAttesa').style.display = 'none';"
                sScript += "document.getElementById('divStampa').style.display = '';"
                sScript += "document.getElementById('divDettaglio').style.display = 'none';"
                sScript += "document.getElementById('loadStampa').src = '../../Documenti/ViewDocumentiElaborati.aspx';"
                RegisterScript(sScript, Me.GetType)
            End If
            Session("ListFatture") = Nothing
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.DettaglioFatturazione.CmdElaborazione_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    Public Function FormattaDataGrd(ByVal tDataGrd As DateTime) As String
        If tDataGrd = Date.MinValue Or tDataGrd = Date.MaxValue Then
            Return ""
        Else
            Return tDataGrd.ToShortDateString
        End If
    End Function

    'Private Function DeleteFattura(ByVal oMyFattura As ObjFattura, ByVal oMyRuoloH2O As ObjTotRuoloFatture, ByVal WFSessione As OPENUtility.CreateSessione) As Boolean
    '    Dim FunctionFatture As New ClsFatture
    '    Dim FunctionRuolo As New ClsRuoloH2O
    '    Dim oMyTot As New ObjTotalizzatoriDocumenti
    '    Dim oMyRic As New ObjRicercaDoc
    '    Dim myIdentity, x As Integer

    '    Try
    '        If Request.Item("Provenienza") = "E" Then
    '            oMyFattura.tDataVariazione = Now
    '            oMyFattura.tDataCessazione = Now
    '            If FunctionFatture.SetFattura(oMyFattura, 1, -1, WFSessione) = 0 Then
    '                Return False
    '            Else
    '                'sblocco la lettura
    '                myIdentity = FunctionFatture.BloccoLettura(oMyFattura.Id, oMyFattura.oLetture, 0, WFSessione)
    '                If myIdentity < 1 Then
    '                    Return False
    '                Else
    '                    Return True
    '                End If
    '                'prelevo i totalizzatori per il flusso
    '                oMyRic.sEnte = oMyFattura.sIdEnte
    '                oMyRic.nFlusso = oMyFattura.nIdFlusso
    '                oMyTot = FunctionFatture.GetTotaliRicDoc(oMyRic, WFSessione)
    '                'aggiorno il ruolo
    '                oMyRuoloH2O.nNContribuenti = oMyTot.nContribuenti
    '                oMyRuoloH2O.nNDocumenti = oMyTot.nFatture + oMyTot.nNote
    '                oMyRuoloH2O.impPositivi = oMyTot.impFatture
    '                oMyRuoloH2O.impNegativi = oMyTot.impNote
    '                myIdentity = FunctionRuolo.SetRuoloH2O(oMyRuoloH2O, 1, WFSessione)
    '                If myIdentity = 0 Then
    '                    Return False
    '                Else
    '                    Return True
    '                End If
    '            End If
    '        Else
    '            oMyFattura.nIdFlusso = -1
    '            oMyFattura.tDataDocumento = Date.MinValue
    '            oMyFattura.sNDocumento = ""
    '            'setto a negativi gli importi/consumi
    '            oMyFattura.nConsumo = (oMyFattura.nConsumo * -1)
    '            oMyFattura.impScaglioni = (oMyFattura.impScaglioni * -1)
    '            oMyFattura.impCanoni = (oMyFattura.impCanoni * -1)
    '            oMyFattura.impNolo = (oMyFattura.impNolo * -1)
    '            '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
    '            oMyFattura.impQuoteFisse = (oMyFattura.impQuoteFisse * -1)
    '            oMyFattura.impQuoteFisseDep = (oMyFattura.impQuoteFisseDep * -1)
    '            oMyFattura.impQuoteFisseFog = (oMyFattura.impQuoteFisseFog * -1)
    '            '*** ***
    '            oMyFattura.impAddizionali = (oMyFattura.impAddizionali * -1)
    '            oMyFattura.impImponibile = (oMyFattura.impImponibile * -1)
    '            oMyFattura.impEsente = (oMyFattura.impEsente * -1)
    '            oMyFattura.impIva = (oMyFattura.impIva * -1)
    '            oMyFattura.impTotale = (oMyFattura.impTotale * -1)
    '            oMyFattura.impArrotondamento = (oMyFattura.impArrotondamento * -1)
    '            oMyFattura.impFattura = (oMyFattura.impFattura * -1)
    '            oMyFattura.sTipoDocumento = "N"
    '            oMyFattura.tDataDocumentoRif = LblDataDoc.Text.ToString.Replace(" DEL ", "")
    '            oMyFattura.sNDocumentoRif = LblNDoc.Text.ToString.Replace("N. ", "")
    '            oMyFattura.sOperatore = Session("username")
    '            oMyFattura.tDataInserimento = Now
    '            For x = 0 To oMyFattura.oDettaglioIva.GetUpperBound(0)
    '                oMyFattura.oDettaglioIva(x).impDettaglio = (oMyFattura.oDettaglioIva(x).impDettaglio * -1)
    '            Next
    '            'inserisco il documento
    '            If FunctionFatture.SetFatturaCompleta(oMyFattura, WFSessione) = 0 Then
    '                Return False
    '            Else
    '                '*** non devo sbloccare la lettura perchè devo gestire il ribaltamento variazioni e inoltre la fattura può essere legata a più letture quindi non saprei quale sbloccare ***
    '                ''sblocco la lettura
    '                'myIdentity = FunctionFatture.BloccoLettura(oMyFattura.nIdLettura, 0, WFSessione)
    '                'If myIdentity < 1 Then
    '                '    Return False
    '                'Else
    '                '    Return True
    '                'End If
    '                Return True
    '            End If
    '        End If

    '        Return True
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DettaglioFatturazione.DeleteFattura.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '        Return False
    '    End Try
    'End Function
    Private Function DeleteFattura(ByVal oMyFattura As ObjFattura, ByVal oMyRuoloH2O As ObjTotRuoloFatture) As Boolean
        Dim FunctionFatture As New ClsFatture
        Dim FunctionRuolo As New ClsRuoloH2O
        Dim oMyTot As New ObjTotalizzatoriDocumenti
        Dim oMyRic As New ObjRicercaDoc
        Dim myIdentity As Integer

        Try
            If Request.Item("Provenienza") = "E" Then
                oMyFattura.tDataVariazione = Now
                oMyFattura.tDataCessazione = Now
                If FunctionFatture.SetFattura(oMyFattura, 1, -1, -1) = 0 Then
                    Return False
                Else
                    'sblocco la lettura
                    myIdentity = FunctionFatture.BloccoLettura(oMyFattura.Id, oMyFattura.oLetture, 0)
                    If myIdentity < 1 Then
                        Return False
                    Else
                        Return True
                    End If
                    'prelevo i totalizzatori per il flusso
                    oMyRic.sEnte = oMyFattura.sIdEnte
                    oMyRic.nFlusso = oMyFattura.nIdFlusso
                    oMyTot = FunctionFatture.GetTotaliRicDoc(oMyRic)
                    'aggiorno il ruolo
                    oMyRuoloH2O.nNContribuenti = oMyTot.nContribuenti
                    oMyRuoloH2O.nNDocumenti = oMyTot.nFatture + oMyTot.nNote
                    oMyRuoloH2O.impPositivi = oMyTot.impFatture
                    oMyRuoloH2O.impNegativi = oMyTot.impNote
                    myIdentity = FunctionRuolo.SetRuoloH2O(oMyRuoloH2O, 1)
                    If myIdentity = 0 Then
                        Return False
                    Else
                        Return True
                    End If
                End If
            Else
                oMyFattura.sTipoDocumento = "N"
                oMyFattura.sOperatore = ConstSession.UserName
                'inserisco il documento
                If FunctionFatture.SetFatturaCompleta(oMyFattura) = 0 Then
                    Return False
                Else
                    '*** non devo sbloccare la lettura perchè devo gestire il ribaltamento variazioni e inoltre la fattura può essere legata a più letture quindi non saprei quale sbloccare ***
                    ''sblocco la lettura
                    'myIdentity = FunctionFatture.BloccoLettura(oMyFattura.nIdLettura, 0, WFSessione)
                    'If myIdentity < 1 Then
                    '    Return False
                    'Else
                    '    Return True
                    'End If
                    Return True
                End If
            End If

            Return True
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DettaglioFatturazione.DeleteFattura.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
            Return False
        End Try
    End Function

    Private Sub LoadCombos()
        Dim oLoadCombo As New ClsGenerale.Generale
        Dim sSQL As String

        Try
            'carico i tipi utenza
            sSQL = "SELECT DESCRIZIONE, IDTIPOUTENZA"
            sSQL += " FROM TP_TIPIUTENZA"
            sSQL += " WHERE (COD_ENTE='" & ConstSession.IdEnte & "')"
            sSQL += " ORDER BY DESCRIZIONE"
            oLoadCombo.LoadComboGenerale(DdlTipoUtenza, sSQL)
            'carico i tipi contatori
            sSQL = "SELECT DESCRIZIONE, IDTIPOCONTATORE"
            sSQL += " FROM TP_TIPOCONTATORE"
            sSQL += " ORDER BY DESCRIZIONE"
            oLoadCombo.LoadComboGenerale(DdlTipoContatore, sSQL)
            'carico le fognature
            sSQL = "SELECT CODICEFOGNATURA +' - ' + DESCRIZIONE, CODFOGNATURA"
            sSQL += " FROM TP_FOGNATURA"
            sSQL += " ORDER BY CODICEFOGNATURA +' - ' + DESCRIZIONE"
            oLoadCombo.LoadComboGenerale(DdlFognatura, sSQL)
            'carico le depurazioni
            sSQL = "SELECT CODICEDEPURAZIONE +' - ' + DESCRIZIONE, CODDEPURAZIONE"
            sSQL += " FROM TP_DEPURAZIONE"
            sSQL += " ORDER BY CODICEDEPURAZIONE +' - ' + DESCRIZIONE"
            oLoadCombo.LoadComboGenerale(DdlDepurazione, sSQL)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DettaglioFatturazione.LoadCombos.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw New Exception("LoadCombos::", ex)
        End Try
    End Sub
    Private Sub LoadFattura(ByVal oMyFattura As ObjFattura)
        Dim FunctionFattura As New ClsFatture
        Dim oListRate() As ObjRata
        Dim FunctionRate As New ClsRate
        Dim oMyNotaCredito As New ObjFattura
        Try
            Session("oDettFattura") = oMyFattura
            Session("oDettFatturaOrg") = oMyFattura
            'carico l'intestatario
            TxtCodIntestatario.Text = oMyFattura.nIdIntestatario
            LblCognomeIntest.Text = oMyFattura.oAnagrafeIntestatario.Cognome
            LblNomeIntest.Text = oMyFattura.oAnagrafeIntestatario.Nome
            LblDataNascitaIntest.Text = oMyFattura.oAnagrafeIntestatario.DataNascita
            LblIndirizzoIntest.Text = oMyFattura.oAnagrafeIntestatario.ViaResidenza & " " & oMyFattura.oAnagrafeIntestatario.CivicoResidenza & " " & oMyFattura.oAnagrafeIntestatario.EsponenteCivicoResidenza
            LblComuneIntest.Text = oMyFattura.oAnagrafeIntestatario.CapResidenza & " - " & oMyFattura.oAnagrafeIntestatario.ComuneResidenza & " (" & oMyFattura.oAnagrafeIntestatario.ProvinciaResidenza & ")"
            'carico l'anagrafica
            TxtCodUtente.Text = oMyFattura.nIdUtente
            LblCognome.Text = oMyFattura.oAnagrafeUtente.Cognome
            LblNome.Text = oMyFattura.oAnagrafeUtente.Nome
            LblDataNascita.Text = oMyFattura.oAnagrafeUtente.DataNascita
            LblIndirizzo.Text = oMyFattura.oAnagrafeUtente.ViaResidenza & " " & oMyFattura.oAnagrafeUtente.CivicoResidenza & " " & oMyFattura.oAnagrafeUtente.EsponenteCivicoResidenza
            LblComune.Text = oMyFattura.oAnagrafeUtente.CapResidenza & " - " & oMyFattura.oAnagrafeUtente.ComuneResidenza & " (" & oMyFattura.oAnagrafeUtente.ProvinciaResidenza & ")"
            LblNUtente.Text = "Numero Utente " & oMyFattura.sNUtente
            'carico il documento
            LblTipoDoc.Text = oMyFattura.sDescrTipoDocumento
            LblNDoc.Text = "N. " & oMyFattura.sNDocumento
            If oMyFattura.tDataDocumento.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                LblDataDoc.Text = " DEL " & oMyFattura.tDataDocumento.ToShortDateString
            End If
            LblTotFattura.Text = " PER UN TOTALE DI EURO " & FormatNumber(oMyFattura.impFattura, 2)
            'carico il contatore
            TxtVia.Text = oMyFattura.sViaContatore
            If oMyFattura.sCivicoContatore <> "-1" Then
                TxtCivico.Text = oMyFattura.sCivicoContatore
            End If
            TxtFrazione.Text = oMyFattura.sFrazioneContatore
            TxtMatricola.Text = oMyFattura.sMatricola
            DdlTipoUtenza.SelectedValue = oMyFattura.nTipoUtenza
            TxtNUtenze.Text = oMyFattura.nUtenze
            If oMyFattura.nTipoContatore > 0 Then
                DdlTipoContatore.SelectedValue = oMyFattura.nTipoContatore
            End If
            If oMyFattura.bEsenteAcqua = True Then
                ChkEsenAcqua.Checked = True
            End If
            If oMyFattura.bEsenteDepurazione = False Then
                DdlDepurazione.SelectedIndex = 1
            Else
                ChkEsenDepurazione.Checked = True
            End If
            If oMyFattura.bEsenteFognatura = False Then
                DdlFognatura.SelectedIndex = 1
            Else
                ChkEsenFognatura.Checked = True
            End If
            '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
            ChkEsenAcquaQF.Checked = oMyFattura.bEsenteAcquaQF
            ChkEsenDepQF.Checked = oMyFattura.bEsenteDepQF
            ChkEsenFogQF.Checked = oMyFattura.bEsenteFogQF
            '*** ***
            'carico le letture
            TxtDataLettPrec.Text = oMyFattura.tDataLetturaPrec.ToShortDateString
            TxtLettPrec.Text = oMyFattura.nLetturaPrec
            TxtDataLettAtt.Text = oMyFattura.tDataLetturaAtt.ToShortDateString
            TxtLettAtt.Text = oMyFattura.nLetturaAtt
            TxtConsumo.Text = oMyFattura.nConsumo
            TxtGiorni.Text = oMyFattura.nGiorni

            Dim FncLetture As New GestLetture
            Dim nConsumoSub As Integer
            nConsumoSub = FncLetture.GetConsumoSubContatore(oMyFattura.nIdContatore)
            If nConsumoSub >= 0 Then
                TxtSubConsumo.Text = nConsumoSub
            End If
            'controllo se è stata fatta una nota di credito
            If oMyFattura.tDataDocumento.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                oMyNotaCredito = FunctionFattura.GetNotaCreditoNewFattura(ConstSession.IdEnte, oMyFattura.tDataDocumento, oMyFattura.sNDocumento)
                If Not oMyNotaCredito Is Nothing Then
                    If oMyNotaCredito.Id > 0 Then
                        LblRifNotaCredito.Text = "Annullata da Nota di Credito "
                        If oMyNotaCredito.tDataDocumento.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                            LblRifNotaCredito.Text += "N. " & oMyNotaCredito.sNDocumento & "del " & oMyNotaCredito.tDataDocumento.ToShortDateString
                        Else
                            LblRifNotaCredito.Text += " ancora da emettere"
                        End If
                        LnkNotaCredito.Attributes.Add("OnClick", "ShowNotaCreditoNewFattura('" & oMyNotaCredito.Id & "','N')")
                        LblRifNotaCredito.Style.Add("display", "")
                        LnkNotaCredito.Style.Add("display", "")
                    Else
                        LblRifNotaCredito.Style.Add("display", "none")
                        LnkNotaCredito.Style.Add("display", "none")
                    End If
                Else
                    LblRifNotaCredito.Style.Add("display", "none")
                    LnkNotaCredito.Style.Add("display", "none")
                End If
            Else
                LblRifNotaCredito.Style.Add("display", "none")
                LnkNotaCredito.Style.Add("display", "none")
            End If
            'carico il dettaglio tariffe
            Session("oDettFatturaScaglioni") = oMyFattura.oScaglioni
            Session("oDettFatturaCanoni") = oMyFattura.oCanoni
            Session("oDettFatturaNolo") = oMyFattura.oNolo
            Session("oDettFatturaQuotaFissa") = oMyFattura.oQuoteFisse
            Session("oDettFatturaAddizionali") = oMyFattura.oAddizionali
            Session("oDettFatturaDettaglioIva") = oMyFattura.oDettaglioIva
            If Not IsNothing(oMyFattura.oNolo) Then
                For Each myNolo As ObjTariffeNolo In oMyFattura.oNolo
                    If myNolo.bIsUnaTantum = True Then
                        ChkNoloUnaTantum.Checked = True
                        Exit For
                    End If
                Next
            End If
            '*** 20141117 - voce di costo specifica per utente ***
            'carico le addizionali
            LoadAddizionali(oMyFattura)
            'carico una riga vuota nelle voci aggiuntive
            LoadVoceAggiuntiva(oMyFattura, -1, True)
            '*** ***
            'carico le rate
            oListRate = FunctionRate.GetRata(oMyFattura.Id)
            If Not oListRate Is Nothing Then
                GrdRate.Style.Add("display", "")
                GrdRate.DataSource = oListRate
                GrdRate.DataBind()
            Else
                GrdRate.Style.Add("display", "none")
            End If
            Abilita(True, 0)
        Catch ex As Exception
            Throw New Exception("LoadFattura::", ex)
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DettaglioFatturazione.LoadFattura.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub Abilita(ByVal bTypeAbilita As Boolean, ByVal IsVariazione As Integer)
        Try
            If IsVariazione <> 1 Then
                TxtVia.ReadOnly = bTypeAbilita : TxtCivico.ReadOnly = bTypeAbilita : TxtFrazione.ReadOnly = bTypeAbilita
                TxtMatricola.ReadOnly = bTypeAbilita
            End If
            TxtDataLettPrec.ReadOnly = bTypeAbilita : TxtLettPrec.ReadOnly = bTypeAbilita
            TxtNUtenze.ReadOnly = bTypeAbilita
            TxtDataLettAtt.ReadOnly = bTypeAbilita : TxtLettAtt.ReadOnly = bTypeAbilita
            '*** 20130115 - non devo abilitare in scrittura questi due campi perchè sono ricalcolati dal motore in base alla lettura prec e alla lettura att ***
            'TxtConsumo.ReadOnly = bTypeAbilita : TxtGiorni.ReadOnly = bTypeAbilita
            TxtConsumo.Enabled = False : TxtGiorni.Enabled = False
            '*** ***
            'inverto per gestire le combo
            If bTypeAbilita = False Then
                bTypeAbilita = True
                '*** 20141117 - voce di costo specifica per utente ***
                divVoceAggiuntiva.Style.Add("display", "")
                '*** **
            Else
                bTypeAbilita = False
                '*** 20141117 - voce di costo specifica per utente ***
                divVoceAggiuntiva.Style.Add("display", "none")
                '*** ***
            End If
            DdlTipoUtenza.Enabled = bTypeAbilita : DdlTipoContatore.Enabled = bTypeAbilita
            DdlFognatura.Enabled = bTypeAbilita : DdlDepurazione.Enabled = bTypeAbilita
            ChkEsenAcqua.Enabled = bTypeAbilita : ChkEsenFognatura.Enabled = bTypeAbilita : ChkEsenDepurazione.Enabled = bTypeAbilita
            ChkNoloUnaTantum.Enabled = bTypeAbilita
            '*** 20121217 - calcolo quota fissa acqua+depurazione+fognatura ***
            ChkEsenAcquaQF.Enabled = bTypeAbilita
            ChkEsenDepQF.Enabled = bTypeAbilita
            ChkEsenFogQF.Enabled = bTypeAbilita
            '*** ***
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DettaglioFatturazione.Abilita.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** 20141117 - voce di costo specifica per utente ***

#Region "Griglie"
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim isMod As Boolean
            Dim IDRow As String = e.CommandArgument.ToString()
            If e.CommandName = "RowOpen" Then
                For Each myRow As GridViewRow In GrdRate.Rows
                    If IDRow = CType(myRow.FindControl("hfid"), HiddenField).Value Then
                        LoadVoceAggiuntiva(Session("oDettFattura"), myRow.Cells(4).Text, isMod)
                        If isMod = True Then
                            divVoceAggiuntiva.Style.Add("display", "")
                        Else
                            Dim sScript As String = "GestAlert('a', 'warning', '', '', 'Non e\' possibile modificare un\'addizionale che non e\' aggiuntiva!');"
                            RegisterScript(sScript, Me.GetType())
                        End If
                    End If
                Next
            ElseIf e.CommandName = "RowUpdate" Then
                Dim ListVociAggiuntive As New Generic.List(Of ObjTariffeAddizionale)
                Dim myVoceAggiuntiva As New ObjTariffeAddizionale
                Dim oMyFattura As New ObjFattura
                Dim bTrovato As Boolean = False
                Dim nList As Integer = 0
                For Each myRow As GridViewRow In GrdRate.Rows
                    If IDRow = CType(myRow.FindControl("hfid"), HiddenField).Value Then

                        'popolo lista da con quanto già presente
                        oMyFattura = Session("oDettFattura")
                        If Not oMyFattura.oAddizionali Is Nothing Then
                            nList = oMyFattura.oAddizionali.Length
                        End If
                        'leggo i valori inseriti in griglia
                        For Each dgiMyRow As GridViewRow In GrdRate.Rows
                            myVoceAggiuntiva = ValDatiVoceAggiuntiva(dgiMyRow, nList)
                            If Not myVoceAggiuntiva Is Nothing Then
                                myVoceAggiuntiva.sIdEnte = oMyFattura.sIdEnte
                                myVoceAggiuntiva.nIdFattura = oMyFattura.Id
                                myVoceAggiuntiva.sAnno = oMyFattura.sAnno
                                myVoceAggiuntiva.sOperatore = ConstSession.UserName
                                myVoceAggiuntiva.tDataInserimento = Now
                                myVoceAggiuntiva.tDataVariazione = Nothing
                                myVoceAggiuntiva.tDataCessazione = Nothing
                                If Not oMyFattura.oAddizionali Is Nothing Then
                                    For Each myAdd As ObjTariffeAddizionale In oMyFattura.oAddizionali
                                        If myAdd.Id <> myVoceAggiuntiva.Id Then
                                            ListVociAggiuntive.Add(myAdd)
                                        Else
                                            ListVociAggiuntive.Add(myVoceAggiuntiva)
                                            bTrovato = True
                                        End If
                                    Next
                                End If
                                If bTrovato = False Then
                                    ListVociAggiuntive.Add(myVoceAggiuntiva)
                                End If
                            Else
                                Throw New Exception("Errore in recupero dati voce aggiuntiva")
                            End If
                        Next

                        'aggiorno la variabile di sessione
                        oMyFattura.oAddizionali = ListVociAggiuntive.ToArray
                        Session("oDettFattura") = oMyFattura
                        LoadAddizionali(oMyFattura)
                        'carico una riga vuota nelle voci aggiuntive e nascondo
                        LoadVoceAggiuntiva(oMyFattura, -1, True)
                        divVoceAggiuntiva.Style.Add("display", "none")
                    End If
                Next
            ElseIf e.CommandName = "RowDelete" Then
                For Each myRow As GridViewRow In GrdRate.Rows
                    Dim ListVociAggiuntive As New Generic.List(Of ObjTariffeAddizionale)
                    Dim myVoceAggiuntiva As New ObjTariffeAddizionale
                    Dim oMyFattura As New ObjFattura
                    If IDRow = CType(myRow.FindControl("hfid"), HiddenField).Value Then
                        'popolo lista da con quanto già presente
                        oMyFattura = Session("oDettFattura")
                        'leggo i valori inseriti in griglia
                        For Each dgiMyRow As GridViewRow In GrdVoceAggiuntiva.Rows
                            If dgiMyRow.Cells(6).Text = Costanti.IDVOCEAGGIUNTIVAUTENTE Then
                                For Each myVoceAggiuntiva In oMyFattura.oAddizionali
                                    If dgiMyRow.Cells(5).Text <> myVoceAggiuntiva.Id Then
                                        ListVociAggiuntive.Add(myVoceAggiuntiva)
                                    End If
                                Next
                                'aggiorno la variabile di sessione
                                oMyFattura.oAddizionali = ListVociAggiuntive.ToArray
                            Else
                                Dim sScript As String = "GestAlert('a', 'warning', '', '', 'Non e\' possibile eliminare un\'addizionale che non e\' aggiuntiva!');"
                                RegisterScript(sScript, Me.GetType())
                            End If
                        Next
                        Session("oDettFattura") = oMyFattura
                        LoadAddizionali(oMyFattura)
                        'carico una riga vuota nelle voci aggiuntive e nascondo
                        LoadVoceAggiuntiva(oMyFattura, -1, True)
                        divVoceAggiuntiva.Style.Add("display", "none")
                    End If
                Next
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DettaglioFatturazione.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

#End Region

    'Private Sub GrdAddizionali_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GrdAddizionali.ItemCommand
    '    Try
    '        Dim isMod As Boolean
    '        If e.CommandName = "Select" Then
    '            LoadVoceAggiuntiva(Session("oDettFattura"), e.Item.Cells(4).Text, ismod)
    '            If ismod = True Then
    '                divVoceAggiuntiva.Style.Add("display", "")
    '            Else
    '                Dim sScript As String = "GestAlert('a', 'warning', '', '', 'Non e\' possibile modificare un\'addizionale che non e\' aggiuntiva!');"
    '                RegisterScript(sScript , Me.GetType())"VoceDel", "" & sScript & "")
    '            End If
    '        End If
    '    Catch ex As Exception
    '         Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DettaglioFatturazione.GrdAddizionali_ItemCommand.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Private Sub GrdVoceAggiuntiva_UpdateCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GrdVoceAggiuntiva.UpdateCommand
    '    Dim ListVociAggiuntive As New Generic.List(Of ObjTariffeAddizionale)
    '    Dim myVoceAggiuntiva As New ObjTariffeAddizionale
    '    Dim oMyFattura As New ObjFattura
    '    Dim bTrovato As Boolean = False
    '    Dim nList As Integer = 0
    '    Try
    '        'popolo lista da con quanto già presente
    '        oMyFattura = Session("oDettFattura")
    '        If Not oMyFattura.oAddizionali Is Nothing Then
    '            nList = oMyFattura.oAddizionali.Length
    '        End If
    '        'leggo i valori inseriti in griglia
    '        For Each dgiMyRow As GridViewRow In GrdVoceAggiuntiva.Items
    '            myVoceAggiuntiva = ValDatiVoceAggiuntiva(dgiMyRow, nList)
    '            If Not myVoceAggiuntiva Is Nothing Then
    '                myVoceAggiuntiva.sIdEnte = oMyFattura.sIdEnte
    '                myVoceAggiuntiva.nIdFattura = oMyFattura.Id
    '                myVoceAggiuntiva.sAnno = oMyFattura.sAnno
    '                myVoceAggiuntiva.sOperatore = ConstSession.UserName
    '                myVoceAggiuntiva.tDataInserimento = Now
    '                myVoceAggiuntiva.tDataVariazione = Nothing
    '                myVoceAggiuntiva.tDataCessazione = Nothing
    '                If Not oMyFattura.oAddizionali Is Nothing Then
    '                    For Each myAdd As ObjTariffeAddizionale In oMyFattura.oAddizionali
    '                        If myAdd.Id <> myVoceAggiuntiva.Id Then
    '                            ListVociAggiuntive.Add(myAdd)
    '                        Else
    '                            ListVociAggiuntive.Add(myVoceAggiuntiva)
    '                            bTrovato = True
    '                        End If
    '                    Next
    '                End If
    '                If bTrovato = False Then
    '                    ListVociAggiuntive.Add(myVoceAggiuntiva)
    '                End If
    '            Else
    '                Throw New Exception("Errore in recupero dati voce aggiuntiva")
    '            End If
    '        Next

    '        'aggiorno la variabile di sessione
    '        oMyFattura.oAddizionali = ListVociAggiuntive.ToArray
    '        Session("oDettFattura") = oMyFattura
    '        LoadAddizionali(oMyFattura)
    '        'carico una riga vuota nelle voci aggiuntive e nascondo
    '        LoadVoceAggiuntiva(oMyFattura, -1, True)
    '        divVoceAggiuntiva.Style.Add("display", "none")
    '    Catch ex As Exception
    '         Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DettaglioFatturazione.GrdVoceAggiuntiva_UpdateCommand.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Private Sub GrdVoceAggiuntiva_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GrdVoceAggiuntiva.DeleteCommand
    '    Dim ListVociAggiuntive As New Generic.List(Of ObjTariffeAddizionale)
    '    Dim myVoceAggiuntiva As New ObjTariffeAddizionale
    '    Dim oMyFattura As New ObjFattura
    '    Try
    '        'popolo lista da con quanto già presente
    '        oMyFattura = Session("oDettFattura")
    '        'leggo i valori inseriti in griglia
    '        For Each dgiMyRow As GridViewRow In GrdVoceAggiuntiva.Items
    '            If dgiMyRow.Cells(6).Text = Costanti.IDVOCEAGGIUNTIVAUTENTE Then
    '                For Each myVoceAggiuntiva In oMyFattura.oAddizionali
    '                    If dgiMyRow.Cells(5).Text <> myVoceAggiuntiva.Id Then
    '                        ListVociAggiuntive.Add(myVoceAggiuntiva)
    '                    End If
    '                Next
    '                'aggiorno la variabile di sessione
    '                oMyFattura.oAddizionali = ListVociAggiuntive.ToArray
    '            Else
    '                Dim sScript As String = "GestAlert('a', 'warning', '', '', 'Non e\' possibile eliminare un\'addizionale che non e\' aggiuntiva!');"
    '                RegisterScript(sScript , Me.GetType())"VoceDel", "" & sScript & "")
    '            End If
    '        Next
    '        Session("oDettFattura") = oMyFattura
    '        LoadAddizionali(oMyFattura)
    '        'carico una riga vuota nelle voci aggiuntive e nascondo
    '        LoadVoceAggiuntiva(oMyFattura, -1, True)
    '        divVoceAggiuntiva.Style.Add("display", "none")
    '    Catch ex As Exception
    '         Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DettaglioFatturazione.GrdVoceAggiuntiva_DeleteCommand.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    Private Sub LoadAddizionali(ByVal oMyFattura As ObjFattura)
        Try
            If Not oMyFattura.oAddizionali Is Nothing Then
                GrdAddizionali.DataSource = oMyFattura.oAddizionali
                GrdAddizionali.DataBind()
                GrdAddizionali.SelectedIndex = -1
                LblResultAddizionali.Style.Add("display", "none")
            Else
                LblResultAddizionali.Style.Add("display", "")
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DettaglioFatturazione.LoadAddizionali.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw New Exception("Si è verificato un errore in LoadAddizionali::", ex)
        End Try
    End Sub
    ''' <summary>
    ''' Gestione del cambio pagina della griglia.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdPageIndexChanging(ByVal sender As Object, e As GridViewPageEventArgs)
        LoadSearch(e.NewPageIndex)
    End Sub
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdRate.DataSource = CType(Session("oAnagDocumenti"), ObjAnagDocumenti())
            If page.HasValue Then
                GrdRate.PageIndex = page.Value
            End If
            GrdRate.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicFatturazione.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
    Private Sub LoadVoceAggiuntiva(ByVal oMyFattura As ObjFattura, ByVal IdVoceAggiuntiva As Integer, ByRef IsMod As Boolean)
        Try
            Dim ListVociAggiuntive As New Generic.List(Of ObjTariffeAddizionale)
            Dim myVoceAggiuntiva As New ObjTariffeAddizionale

            If IdVoceAggiuntiva <= 0 Then
                IsMod = True
                myVoceAggiuntiva.sIdEnte = oMyFattura.sIdEnte
                myVoceAggiuntiva.nIdFattura = oMyFattura.Id
                myVoceAggiuntiva.nIdAddizionale = Costanti.IDVOCEAGGIUNTIVAUTENTE
                myVoceAggiuntiva.sAnno = oMyFattura.sAnno
                myVoceAggiuntiva.sOperatore = ConstSession.UserName
                myVoceAggiuntiva.tDataInserimento = Now
                ListVociAggiuntive.Add(myVoceAggiuntiva)
            Else
                For Each myVoce As ObjTariffeAddizionale In oMyFattura.oAddizionali
                    If myVoce.Id = IdVoceAggiuntiva Then
                        myVoceAggiuntiva.Id = myVoce.Id
                        myVoceAggiuntiva.sIdEnte = oMyFattura.sIdEnte
                        myVoceAggiuntiva.nIdFattura = oMyFattura.Id
                        myVoceAggiuntiva.nIdAddizionale = myVoce.nIdAddizionale
                        myVoceAggiuntiva.impAddizionale = myVoce.impAddizionale
                        myVoceAggiuntiva.impTariffa = myVoce.impTariffa
                        myVoceAggiuntiva.nAliquota = myVoce.nAliquota
                        myVoceAggiuntiva.sDescrizione = myVoce.sDescrizione
                        myVoceAggiuntiva.sAnno = oMyFattura.sAnno
                        myVoceAggiuntiva.sOperatore = ConstSession.UserName
                        myVoceAggiuntiva.tDataInserimento = Now
                        If myVoce.nIdAddizionale = Costanti.IDVOCEAGGIUNTIVAUTENTE Then
                            IsMod = True
                        Else
                            IsMod = False
                        End If
                        ListVociAggiuntive.Add(myVoceAggiuntiva)
                    End If
                Next
            End If

            GrdVoceAggiuntiva.DataSource = ListVociAggiuntive.ToArray()
            GrdVoceAggiuntiva.DataBind()
            GrdVoceAggiuntiva.SelectedIndex = -1
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DettaglioFatturazione.LoadVoceAggiuntiva.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    Private Sub LoadTariffeAddizionali(ByVal myFattura As ObjFattura, ByVal myTariffe As ObjTariffe)
        Try
            Dim listAddiz As New Generic.List(Of OggettoAddizionaleEnte)
            For Each myRow As ObjTariffeAddizionale In myFattura.oAddizionali
                Dim myAddiz As New OggettoAddizionaleEnte
                myAddiz.Aliquota = myRow.nAliquota
                myAddiz.dImporto = myRow.impTariffa
                myAddiz.ID = myRow.nIdAddizionale
                myAddiz.sAnno = myRow.sAnno
                myAddiz.sDescrizione = myRow.sDescrizione
                myAddiz.sIdEnte = myRow.sIdEnte
                listAddiz.Add(myAddiz)
            Next
            myTariffe.oAddizionali = listAddiz.ToArray
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DettaglioFatturazione.LoadTariffeAddizionali.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    Private Function ValDatiVoceAggiuntiva(ByVal dgiRow As GridViewRow, ByVal nList As Integer) As ObjTariffeAddizionale
        Dim oMyVoce As New ObjTariffeAddizionale
        Try
            If CInt(dgiRow.Cells(5).Text) <= 0 Then
                oMyVoce.Id = nList + 1
            Else
                oMyVoce.Id = CInt(dgiRow.Cells(5).Text)
            End If
            oMyVoce.impTariffa = CType(dgiRow.FindControl("TxtTariffaVoceAgg"), TextBox).Text
            oMyVoce.impAddizionale = oMyVoce.impTariffa
            oMyVoce.nAliquota = CType(dgiRow.FindControl("TxtAliquotaVoceAgg"), TextBox).Text
            oMyVoce.nIdAddizionale = dgiRow.Cells(6).Text
            oMyVoce.sDescrizione = CType(dgiRow.FindControl("TxtDescrVoceAgg"), TextBox).Text
            oMyVoce.sOperatore = ConstSession.UserName
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DettaglioFatturazione.ValDatiVoceAggiuntiva.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            oMyVoce = Nothing
        End Try
        Return oMyVoce
    End Function
    '*** ***
End Class
