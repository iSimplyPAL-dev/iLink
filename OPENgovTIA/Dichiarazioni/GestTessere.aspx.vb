Imports log4net
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
Imports AnagInterface
''' <summary>
''' Pagina per la visualizzazione/gestione delle tessere.
''' Contiene i dati di dettaglio e le funzioni della comandiera.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class GestTessere
    Inherits BaseEnte
    Private Shared Log As ILog = LogManager.GetLogger(GetType(GestTessere))
    'Private WFSessione As OPENUtility.CreateSessione
    'Private WFErrore As String
    Protected FncGrd As New Formatta.FunctionGrd

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Label14 As System.Web.UI.WebControls.Label
    Protected WithEvents Label8 As System.Web.UI.WebControls.Label
    Protected WithEvents Label11 As System.Web.UI.WebControls.Label
    Protected WithEvents Label15 As System.Web.UI.WebControls.Label
    Protected WithEvents Label18 As System.Web.UI.WebControls.Label
    Protected WithEvents LnkNewImmobili As System.Web.UI.WebControls.ImageButton
    Protected WithEvents LnkNewImmobiliAnater As System.Web.UI.WebControls.ImageButton
    Protected WithEvents Label47 As System.Web.UI.WebControls.Label
    Protected WithEvents TxtNoteTessera As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label21 As System.Web.UI.WebControls.Label
    Protected WithEvents Label13 As System.Web.UI.WebControls.Label
    Protected WithEvents TxtIdDettaglioTestata As System.Web.UI.WebControls.TextBox

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
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim FncTessera As New GestTessera
        Dim oListTessera() As ObjTessera
        Dim oMyTessera As New ObjTessera
        Dim FncImmo As New GestDettaglioTestata
        Dim x As Integer
        Dim oListRiepilogoPesature() As ObjRiepilogoPesature = Nothing

        Try
            LnkNewUIAnater.Attributes.Add("onclick", "ShowRicUIAnater()")
            LnkNewUIAnater.ToolTip = "Nuovo Immobile da " & ConstSession.NameSistemaTerritorio
            LnkNewRid.Attributes.Add("OnClick", "ShowInsertRidEse('" & ObjCodDescr.TIPO_RIDUZIONI & "')")
            LnkDelRid.Attributes.Add("OnClick", "DeleteRidEse('" & ObjCodDescr.TIPO_RIDUZIONI & "')")
            LnkNewDet.Attributes.Add("OnClick", "ShowInsertRidEse('" & ObjCodDescr.TIPO_ESENZIONI & "')")
            LnkDelDet.Attributes.Add("OnClick", "DeleteRidEse('" & ObjCodDescr.TIPO_ESENZIONI & "')")

            TxtIdTestata.Text = Request.Item("IdTestata")
            If Page.IsPostBack = False Then
                'propongo in automatico il successivo codice utente
                If TxtCodUtente.Text = "" Then
                    TxtCodUtente.Text = FncTessera.GetCodUtenteAutomatico(ConstSession.StringConnection, ConstSession.IdEnte, ConstSession.DescrizioneEnte)
                End If
                '*** 201511 - gestione tipo tessera ***
                loadpagecombos()
                '*** ***
                '*** 20140805 - Gestione Categorie Vani ***
                '*** 201504 - Nuova Gestione anagrafica con form unico ***
                Dim FncLoad As New ClsDichiarazione
                hdIdContribuente.Value = CInt(Request.Item("IdContribuente"))
                If ConstSession.HasPlainAnag Then
                    ifrmAnag.Attributes.Add("src", "../../Generali/asp/VisualAnag.aspx?IdContribuente=" & CInt(Request.Item("IdContribuente")) & "&Azione=" & Utility.Costanti.AZIONE_LETTURA)
                Else
                    FncLoad.LoadPannelAnagrafica(CInt(Request.Item("IdContribuente")), ConstSession.CodTributo, ConstSession.StringConnectionAnagrafica, lblNominativo, lblCFPIVA, lblDatiNascita, lblResidenza)
                End If
                '*** ***
                'controllo se sono in visualizzazione di un immobile
                If Request.Item("AzioneProv") = Utility.Costanti.AZIONE_LETTURA Then
                    'carico l'array originario
                    oListTessera = Session("oListTessere")
                    'carico il singolo immobile selezionato
                    For x = 0 To oListTessera.GetUpperBound(0)
                        If oListTessera(x).Id = Request.Item("IdUniqueTessera") Then
                            oMyTessera = oListTessera(x)
                            Exit For
                        End If
                    Next
                    'prelevo i Immobili
                    If Session("oListUITessera") Is Nothing Then
                        Session("oListUITessera") = oMyTessera.oImmobili
                    End If
                    'immobili per la visualizzione della griglia
                    If Session("oListImmobili") Is Nothing Then
                        Session("oListImmobili") = oMyTessera.oImmobili
                    End If
                    'prelevo le riduzioni
                    If Session("oDatiRidTessera") Is Nothing Then
                        Session("oDatiRidTessera") = oMyTessera.oRiduzioni
                    End If
                    'prelevo le detassazioni
                    If Session("oDatiDetTessera") Is Nothing Then
                        Session("oDatiDetTessera") = oMyTessera.oDetassazioni
                    End If
                    If oMyTessera.Id <> -1 Then
                        'devo prelevare i conferimenti per la tessera
                        oListRiepilogoPesature = FncTessera.GetRiepilogoConferimenti(ConstSession.StringConnection, oMyTessera.Id)
                        Session("oListRiepilogoPesature") = oListRiepilogoPesature
                    End If

                    'visualizzo i dati del singolo immobile selezionato
                    TxtId.Text = oMyTessera.Id
                    TxtIdTessera.Text = oMyTessera.IdTessera
                    TxtIdTestata.Text = oMyTessera.IdTestata
                    If oMyTessera.sCodUtente <> "" Then
                        TxtCodUtente.Text = oMyTessera.sCodUtente
                    End If
                    TxtCodInterno.Text = oMyTessera.sCodInterno
                    TxtNumeroTessera.Text = oMyTessera.sNumeroTessera
                    If oMyTessera.tDataRilascio <> Date.MinValue And oMyTessera.tDataRilascio <> Date.MaxValue Then
                        TxtDataRilascio.Text = oMyTessera.tDataRilascio
                    End If
                    If oMyTessera.tDataCessazione <> Date.MinValue And oMyTessera.tDataCessazione <> Date.MaxValue Then
                        TxtDataCessazione.Text = oMyTessera.tDataCessazione
                    End If
                    TxtNote.Text = oMyTessera.sNote
                    '*** 201511 - gestione tipo tessera ***
                    DdlTipoTessera.SelectedValue = oMyTessera.IdTipoTessera
                    '*** ***
                    Abilita(False)
                Else
                    If Not IsNothing(Session("oTessera")) Then
                        oMyTessera = Session("oTessera")
                        'visualizzo i dati del singolo immobile selezionato
                        TxtId.Text = oMyTessera.Id
                        TxtIdTessera.Text = oMyTessera.IdTessera
                        TxtIdTestata.Text = oMyTessera.IdTestata
                        If oMyTessera.sCodUtente <> "" Then
                            TxtCodUtente.Text = oMyTessera.sCodUtente
                        End If
                        TxtCodInterno.Text = oMyTessera.sCodInterno
                        TxtNumeroTessera.Text = oMyTessera.sNumeroTessera
                        If oMyTessera.tDataRilascio <> Date.MinValue Then
                            TxtDataRilascio.Text = oMyTessera.tDataRilascio
                        End If
                        If oMyTessera.tDataCessazione <> Date.MinValue Then
                            TxtDataCessazione.Text = oMyTessera.tDataCessazione
                        End If
                        TxtNote.Text = oMyTessera.sNote
                        '*** 201511 - gestione tipo tessera ***
                        DdlTipoTessera.SelectedValue = oMyTessera.IdTipoTessera
                        '*** ***
                    Else
                        TxtNumeroTessera.Text = ObjTessera.TESSERA_BIDONE
                    End If
                End If
                'se sono in visualizzazione di una tessera che non è BIDONE devo visualizzare anche tutte le UI associate all'eventuale tessera BIDONE
                If TxtIdTestata.Text <> "-1" Then
                    If oMyTessera.sNumeroTessera <> ObjTessera.TESSERA_BIDONE Then
                        Dim oListImmobili() As ObjDettaglioTestata
                        Dim oListUI() As ObjDettaglioTestata
                        Dim bTrovato As Boolean = False
                        'oListUI = FncImmo.GetDettaglioTestata(WFSessione, -1, TxtIdTestata.Text, ConstSession.IdEnte, True)
                        oListUI = FncImmo.GetDettaglioTestata(ConstSession.StringConnection, -1, oMyTessera.Id, TxtIdTestata.Text, ConstSession.IdEnte, True)
                        If Not oListUI Is Nothing Then
                            oListImmobili = oMyTessera.oImmobili
                            For x = 0 To oListUI.GetUpperBound(0)
                                bTrovato = False
                                For Each myItem As ObjDettaglioTestata In oListImmobili
                                    If myItem.Id = oListUI(x).Id Then
                                        bTrovato = True
                                    End If
                                Next
                                If bTrovato = False Then
                                    ReDim Preserve oListImmobili(oMyTessera.oImmobili.GetUpperBound(0) + x + 1)
                                    oListImmobili(oMyTessera.oImmobili.GetUpperBound(0) + x + 1) = oListUI(x)
                                End If
                            Next
                            Session("oListImmobili") = oListImmobili
                        End If
                    End If
                End If
                'controllo se devo caricare la griglia dei dati Immobili
                If Not Session("oListImmobili") Is Nothing Then
                    GrdImmobili.Style.Add("display", "")
                    GrdImmobili.SelectedIndex = -1
                    GrdImmobili.DataSource = Session("oListImmobili")
                    GrdImmobili.DataBind()
                    LblResultImmobili.Style.Add("display", "none")
                    SettaCheckbox()
                Else
                    GrdImmobili.Style.Add("display", "none")
                    LblResultImmobili.Style.Add("display", "")
                End If
            End If
            'controllo se devo caricare la griglia delle riduzioni
            If Not Session("oDatiRidTessera") Is Nothing Then
                GrdRiduzioni.Style.Add("display", "")
                GrdRiduzioni.DataSource = Session("oDatiRidTessera")
                GrdRiduzioni.SelectedIndex = -1
                GrdRiduzioni.DataBind()
                LblResultRid.Style.Add("display", "none")
            Else
                GrdRiduzioni.Style.Add("display", "none")
                LblResultRid.Style.Add("display", "")
            End If
            'controllo se devo caricare la griglia delle Detassazioni
            If Not Session("oDatiDetTessera") Is Nothing Then
                GrdDetassazioni.Style.Add("display", "")
                GrdDetassazioni.DataSource = Session("oDatiDetTessera")
                GrdDetassazioni.SelectedIndex = -1
                GrdDetassazioni.DataBind()
                LblResultDet.Style.Add("display", "none")
            Else
                GrdDetassazioni.Style.Add("display", "none")
                LblResultDet.Style.Add("display", "")
            End If
            'controllo se devo caricare la griglia delle Conferimenti
            If Not Session("oListRiepilogoPesature") Is Nothing Then
                GrdConferimenti.Style.Add("display", "")
                GrdConferimenti.DataSource = Session("oListRiepilogoPesature")
                GrdConferimenti.SelectedIndex = -1
                GrdConferimenti.DataBind()
                LblResultConferimenti.Style.Add("display", "none")
            Else
                GrdConferimenti.Style.Add("display", "none")
                LblResultConferimenti.Style.Add("display", "")
            End If
            '*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI & DIVERSA GESTIONE QUOTA VARIABILE***
            Dim sScript As String = ""
            If ConstSession.HasPlainAnag Then
                sScript += "document.getElementById('TRSpecAnag').style.display='none';"
            Else
                sScript += "document.getElementById('TRPlainAnag').style.display='none';"
            End If
            RegisterScript( sScript,Me.GetType)
            '*** ***
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTessere.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            ''chiudo la connessione
            'If Not WFSessione Is Nothing Then
            '    WFSessione.Kill()
            'End If
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub LnkNewUI_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles LnkNewUI.Click
        ShowPopUp("I", -1, -1, Utility.Costanti.AZIONE_NEW)
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Private Sub LnkNewConferimento_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles LnkNewConferimento.Click
        Dim sScript As String = ""
        Try
            sScript += "parent.Comandi.location.href = '../../aspVuotaRemoveComandi.aspx';"
            sScript += "parent.Basso.location.href = '../../aspVuotaRemoveComandi.aspx';"
            sScript += "parent.Visualizza.location.href = '../Pesature/GestionePesature.aspx?IdTessera=" & TxtId.Text & "&Provenienza=" & Costanti.FormProvenienza.Tessera
            sScript += "&IdTestata=" & Request.Item("IdTestata") & "&AzioneProv=" & Request.Item("AzioneProv") & "';"
            RegisterScript(sScript, Me.GetType)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTessere.LnkNewConferimento_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub LnkNewConferimento_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles LnkNewConferimento.Click
    '    Dim sScript As String = ""
    '    Try
    '        sScript += "parent.Comandi.location.href = '../Pesature/ComandiGestPesature.aspx';"
    '        sScript += "parent.Visualizza.location.href = '../Pesature/GestionePesature.aspx?IdTessera=" & TxtId.Text & "&Provenienza=" & Costanti.FormProvenienza.Tessera
    '        sScript += "&IdTestata=" & Request.Item("IdTestata") & "&AzioneProv=" & Request.Item("AzioneProv") & "';"
    '        RegisterScript(sScript, Me.GetType)
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTessere.LnkNewConferimento_Click.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdRibaltaUIAnater_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdRibaltaUIAnater.Click
        Try
            If Not Session("oListImmobili") Is Nothing Then
                GrdImmobili.DataSource = Session("oListImmobili")
                GrdImmobili.DataBind()
                GrdImmobili.SelectedIndex = -1
                GrdImmobili.Style.Add("display", "")
                LblResultImmobili.Style.Add("display", "none")
                SettaCheckbox()
            Else
                GrdImmobili.Style.Add("display", "none")
                LblResultImmobili.Style.Add("display", "")
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTessere.CmdRibaltaUIAnater_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdSalvaDatiTessera_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdSalvaDatiTessera.Click
        Dim y, z As Integer
        Dim sScript As String = ""

        '***SE SONO IN NUOVO INSERIMENTO TESSERA SALVO I DATI SOLO IN MEMORIA
        '***SE SONO IN MODIFICA DI UNA TESSERA GIA' INSERITA SALVO I DATI IN MEMORIA E SUL DATABASE
        '***SE SONO IN AGGIUNTA DI UNA TESSERA AD UNA DICHIARAZIONE GIA' INSERITA SALVO I DATI IN MEMORIA E SUL DATABASE
        Try
            '*** 20130403 - tolto vincolo di almeno un immobile per tessera, posso anche avere tessere senza immobili ***
            'controllo che tutti gli immobili hanno vani, mq e che le date siano coerenti con quelle della dichiarazione
            If Not Session("oListImmobili") Is Nothing Then
                Dim oMyImmobili() As ObjDettaglioTestata
                oMyImmobili = Session("oListImmobili")
                'controllo che tutti i vani abbiamo la categoria
                For y = 0 To oMyImmobili.GetUpperBound(0)
                    'la data inizio è obbligatoria
                    If oMyImmobili(y).tDataInizio = Date.MinValue Then
                        sScript = "GestAlert('a', 'warning', '', '', 'Inserire la Data Inizio su tutti gli Immobili!');"
                        RegisterScript(sScript, Me.GetType)
                        Exit Sub
                    End If
                    If Not oMyImmobili(y).oOggetti Is Nothing Then
                        Dim oMyOggetti() As ObjOggetti
                        oMyOggetti = oMyImmobili(y).oOggetti
                        For z = 0 To oMyOggetti.GetUpperBound(0)
                            If ConstSession.IsFromTARES = "1" Then
                                If oMyOggetti(z).IdCatTARES = -1 Then
                                    sScript = "GestAlert('a', 'warning', '', '', 'Inserire la Categoria su tutti i vani!');"
                                    RegisterScript(sScript, Me.GetType)
                                    Exit Sub
                                End If
                            Else
                                If (oMyOggetti(z).IdCategoria = "" Or oMyOggetti(z).IdCategoria = "-1") Then
                                    sScript = "GestAlert('a', 'warning', '', '', 'Inserire la Categoria su tutti i vani!');"
                                    RegisterScript(sScript, Me.GetType)
                                    Exit Sub
                                End If
                            End If
                        Next
                    Else
                        sScript = "GestAlert('a', 'warning', '', '', 'Inserire almeno un vano!');"
                        RegisterScript(sScript, Me.GetType)
                        Exit Sub
                    End If
                Next

                If sScript = "" Then
                    sScript = "document.getElementById('CmdSalvaTessera').click();"
                    RegisterScript(sScript, Me.GetType)
                End If
            Else
                'sHTML = "alert('Inserire almeno un\'immobile!');"
                sScript = "document.getElementById('CmdSalvaTessera').click();"
                RegisterScript(sScript, Me.GetType)
            End If
            '*** ***
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTessere.CmdSalvaDatiTessera_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    'Private Sub CmdSalvaTessera_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdSalvaTessera.Click
    '    '***SE SONO IN NUOVO INSERIMENTO SALVO I DATI SOLO IN MEMORIA
    '    '***SE SONO IN MODIFICA DI QUALCOSA GIA' INSERITO SALVO I DATI IN MEMORIA E SUL DATABASE
    '    '***SE SONO IN AGGIUNTA DI QUALCOSA SALVO I DATI IN MEMORIA E SUL DATABASE
    '    Dim oListTessera(), oListInsert() As ObjTessera
    '    Dim oMyTessera As New ObjTessera
    '    Dim oMyTesseraOrg As New ObjTessera
    '    Dim nList As Integer = -1
    '    Dim FncTessera As New GestTessera
    '    Dim sScript As String
    '    Dim x As Integer

    '    Try
    '        'inizializzo la connessione
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If
    '        'controllo che il numero tessera sia univoco
    '        If TxtIdTessera.Text = "-1" And TxtNumeroTessera.Text <> ObjTessera.TESSERA_BIDONE Then
    '            If FncTessera.GetNTessera(WFSessione, TxtNumeroTessera.Text) > 0 Then
    '                sScript = "alert('Il Numero Tessera deve essere univoco!\nImpossibile proseguire.')"
    '                RegisterScript(me.gettype(),"", "" & sScript & ";")
    '                Exit Sub
    '            End If
    '        End If
    '        'controllo se ho già delle tessere caricate
    '        If Not Session("oListTessere") Is Nothing Then
    '            'carico l'array originario
    '            oListTessera = Session("oListTessere")
    '            'aggiungo una riga
    '            nList = oListTessera.GetUpperBound(0)
    '        End If
    '        'carico i dati dal form
    '        oMyTessera.Id = TxtId.Text
    '        oMyTessera.IdTessera = TxtIdTessera.Text
    '        oMyTessera.IdTestata = TxtIdTestata.Text
    '        oMyTessera.IdEnte = ConstSession.IdEnte
    '        oMyTessera.sCodUtente = TxtCodUtente.Text.Trim
    '        oMyTessera.sCodInterno = TxtCodInterno.Text.Trim
    '        If TxtNumeroTessera.Text <> "" Then
    '            oMyTessera.sNumeroTessera = TxtNumeroTessera.Text
    '        End If
    '        If TxtDataRilascio.Text <> "" Then
    '            oMyTessera.tDataRilascio = TxtDataRilascio.Text
    '        End If
    '        If TxtDataCessazione.Text <> "" Then
    '            oMyTessera.tDataCessazione = TxtDataCessazione.Text
    '        End If
    '        oMyTessera.sNote = TxtNote.Text.Trim
    '        oMyTessera.oImmobili = Session("oListUITessera")
    '        oMyTessera.oRiduzioni = Session("oDatiRidTessera")
    '        oMyTessera.oDetassazioni = Session("oDatiDetTessera")
    '        oMyTessera.tDataInserimento = Now
    '        oMyTessera.sOperatore = ConstSession.UserName
    '        'controllo se sono in modifica di una tessera
    '        If Request.Item("IdUniqueTessera") <> "-1" Then
    '            'sono in modifica di una tessera già inserita nel db
    '            'carico la singola tessera selezionata
    '            For x = 0 To oListTessera.GetUpperBound(0)
    '                If Request.Item("IdUniqueTessera") = oListTessera(x).Id Then
    '                    oMyTesseraOrg = oListTessera(x)
    '                    oMyTesseraOrg.tDataVariazione = Now
    '                    'storicizzo la tessera originale
    '                    If FncTessera.SetTessera(oMyTesseraOrg, Costanti.AZIONE_UPDATE, WFSessione) = 0 Then
    '                        Response.Redirect("../../PaginaErrore.aspx")
    '                        Exit Sub
    '                    End If
    '                    'aggiungo la nuova tessera
    '                    ReDim Preserve oListInsert(0)
    '                    oListInsert(0) = oMyTessera
    '                    If FncTessera.SetTesseraCompleta(oListInsert, oMyTessera.IdTestata, WFSessione) <= 0 Then
    '                        Response.Redirect("../../PaginaErrore.aspx")
    '                        Exit Sub
    '                    End If
    '                    'aggiorno la tessera selezionata
    '                    oListTessera(x) = oMyTessera
    '                    Exit For
    '                End If
    '            Next
    '        Else
    '            ''aggiungo una riga
    '            'nList += 1
    '            ''dimensiono l'array
    '            'ReDim Preserve oListTessera(nList)
    '            ''carico l'array con i dati della videata
    '            'oListTessera(nList) = oMyTessera
    '            'aggiungo una riga
    '            nList = TxtIdTessera.Text
    '            If nList = -1 Then
    '                nList = oListTessera.GetUpperBound(0) + 1
    '                'dimensiono l'array
    '                ReDim Preserve oListTessera(nList)
    '            End If
    '            'carico l'array con i dati della videata
    '            oListTessera(nList) = oMyTessera
    '            'memorizzo l'oggetto nella sessione
    '            Session("oListTessere") = oListTessera
    '        End If
    '        'aggiorno la pagina chiamante
    '        sScript += BackToDichiarazione()
    '        RegisterScript(me.gettype(),"", "" & sScript & ";")
    '    Catch Err As Exception
    '      Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTessere.CmdSalvaTessera_Click.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    Finally
    '        'chiudo la connessione
    '        If Not WFSessione Is Nothing Then
    '            WFSessione.Kill()
    '        End If
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdSalvaTessera_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdSalvaTessera.Click
        '***SE SONO IN NUOVO INSERIMENTO SALVO I DATI SOLO IN MEMORIA
        '***SE SONO IN MODIFICA DI QUALCOSA GIA' INSERITO SALVO I DATI IN MEMORIA E SUL DATABASE
        '***SE SONO IN AGGIUNTA DI QUALCOSA SALVO I DATI IN MEMORIA E SUL DATABASE
        Dim oListInsert() As ObjTessera
        Dim oListTessera As New ArrayList
        Dim oMyTessera As New ObjTessera
        Dim oMyTesseraOrg As New ObjTessera
        'Dim nList As Integer = -1
        Dim FncTessera As New Utility.DichManagerTARSU(ConstSession.DBType, ConstSession.StringConnection, ConstSession.StringConnectionOPENgov, ConstSession.IdEnte) 'As New GestTessera
        Dim FncGest As New GestTessera
        Dim sScript As String = ""
        Dim x As Integer

        Try
            'controllo che il numero tessera sia univoco
            If TxtIdTessera.Text = "-1" And TxtNumeroTessera.Text <> ObjTessera.TESSERA_BIDONE Then
                If FncGest.GetNTessera(ConstSession.StringConnection, TxtNumeroTessera.Text) > 0 Then
                    sScript = "GestAlert('a', 'warning', '', '', 'Il Numero Tessera deve essere univoco!\nImpossibile proseguire.');"
                    RegisterScript(sScript, Me.GetType)
                    Exit Sub
                End If
            End If
            'controllo se ho già delle tessere caricate
            If Not Session("oListTessere") Is Nothing Then
                'carico l'array originario
                For Each oMyTessera In Session("oListTessere")
                    oListTessera.Add(oMyTessera)
                Next
            End If
            'carico i dati dal form
            oMyTessera = New ObjTessera
            oMyTessera.Id = TxtId.Text
            oMyTessera.IdTessera = TxtIdTessera.Text
            oMyTessera.IdTestata = TxtIdTestata.Text
            oMyTessera.IdEnte = ConstSession.IdEnte
            oMyTessera.sCodUtente = TxtCodUtente.Text.Trim
            oMyTessera.sCodInterno = TxtCodInterno.Text.Trim
            If TxtNumeroTessera.Text <> "" Then
                oMyTessera.sNumeroTessera = TxtNumeroTessera.Text
            End If
            If TxtDataRilascio.Text <> "" Then
                oMyTessera.tDataRilascio = TxtDataRilascio.Text
            End If
            If TxtDataCessazione.Text <> "" Then
                oMyTessera.tDataCessazione = TxtDataCessazione.Text
            End If
            oMyTessera.sNote = TxtNote.Text.Trim
            oMyTessera.oImmobili = Session("oListUITessera")
            oMyTessera.oRiduzioni = Session("oDatiRidTessera")
            oMyTessera.oDetassazioni = Session("oDatiDetTessera")
            oMyTessera.tDataInserimento = Now
            oMyTessera.sOperatore = ConstSession.UserName
            '*** 201511 - gestione tipo tessera ***
            oMyTessera.IdTipoTessera = DdlTipoTessera.SelectedValue
            '*** ***
            'controllo se sono in modifica di una tessera
            If Request.Item("IdUniqueTessera") <> "-1" Then
                'sono in modifica di una tessera già inserita nel db
                'carico la singola tessera selezionata
                For x = 0 To oListTessera.Count
                    If Request.Item("IdUniqueTessera") = oListTessera(x).Id Then
                        oMyTesseraOrg = oListTessera(x)
                        oMyTesseraOrg.tDataVariazione = Now
                        'storicizzo la tessera originale
                        If FncTessera.SetTessera(Utility.Costanti.AZIONE_UPDATE, oMyTesseraOrg, oMyTesseraOrg.IdContribuente) = 0 Then
                            Response.Redirect("../../PaginaErrore.aspx")
                            Exit Sub
                        End If
                        'aggiungo la nuova tessera
                        ReDim Preserve oListInsert(0)
                        oListInsert(0) = oMyTessera
                        If FncTessera.SetTesseraCompleta(oListInsert, oMyTessera.IdTestata) <= 0 Then
                            Response.Redirect("../../PaginaErrore.aspx")
                            Exit Sub
                        End If
                        'aggiorno il legame fra tessere e pesature
                        Dim fncPes As New GestPesatura
                        fncPes.SetTesseraVSPesatura(ConstSession.StringConnection, oMyTesseraOrg.Id, oMyTessera.Id)

                        'aggiorno la tessera selezionata
                        oListTessera(x) = oMyTessera
                        Exit For
                    End If
                Next
            Else
                Dim bTrovato As Boolean = False
                If Not oListTessera Is Nothing Then
                    For Each oTessera As ObjTessera In oListTessera
                        If oMyTessera.Id = oTessera.Id Then
                            oTessera = oMyTessera
                            bTrovato = True
                        End If
                    Next
                End If
                If bTrovato = False Then
                    'carico l'array con i dati della videata
                    oListTessera.Add(oMyTessera)
                End If
                'memorizzo l'oggetto nella sessione
                Session("oListTessere") = CType(oListTessera.ToArray(GetType(ObjTessera)), ObjTessera())
            End If
            'aggiorno la pagina chiamante
            sScript += BackToDichiarazione("N")
            RegisterScript(sScript, Me.GetType)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTessere.CmdSalvaTessera_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="11/2015">
    ''' gestione tipo tessera
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Private Sub CmdModTessera_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdModTessera.Click
        Dim sScript, sOldListTessera As String
        Dim oListCmd() As Object
        Dim oMyImmobili() As ObjDettaglioTestata
        Dim oMyAgevolazioni() As ObjRidEseApplicati
        Dim x As Integer

        sScript = ""
        Abilita(True)
        oMyImmobili = Session("oListUITessera")
        'memorizzo il vano originale
        Try
            If Session("sOldListTessera") Is Nothing Then
                sOldListTessera = TxtCodUtente.Text + "|" + TxtCodInterno.Text + "|" + TxtNumeroTessera.Text + "|" + TxtDataRilascio.Text + "|" + TxtDataCessazione.Text + "|" + TxtNote.Text
                sOldListTessera += "|" + DdlTipoTessera.SelectedValue
                If oMyImmobili Is Nothing Then
                    sOldListTessera += "|0"
                Else
                    sOldListTessera += "|" + oMyImmobili.GetUpperBound(0).ToString
                End If
                oMyAgevolazioni = Session("oDatiRidTessera")
                If Not IsNothing(oMyAgevolazioni) Then
                    sOldListTessera += "|" + oMyAgevolazioni.GetUpperBound(0).ToString
                Else
                    sOldListTessera += "|-1"
                End If
                oMyAgevolazioni = Session("oDatiDetTessera")
                If Not IsNothing(oMyAgevolazioni) Then
                    sOldListTessera += "|" + oMyAgevolazioni.GetUpperBound(0).ToString
                Else
                    sOldListTessera += "|-1"
                End If
                Session("sOldListTessera") = sOldListTessera
            End If
            'abilito il pulsante di salvataggio
            ReDim Preserve oListCmd(0)
            oListCmd(0) = "Salva"
            For x = 0 To oListCmd.Length - 1
                sScript += "$('#" + oListCmd(x).ToString() + "', parent.frames['Comandi'].document).removeClass('DisableBtn');"
            Next
            'disabilito il pulsante di salvataggio
            ReDim Preserve oListCmd(1)
            oListCmd(0) = "Modifica"
            oListCmd(1) = "Delete"
            For x = 0 To oListCmd.Length - 1
                sScript += "$('#" + oListCmd(x).ToString() + "', parent.frames['Comandi'].document).addClass('DisableBtn');"
            Next
            RegisterScript(sScript, Me.GetType)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTessere.CmdModTessera_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub CmdModTessera_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdModTessera.Click
    '    Dim sScript, sOldListTessera As String
    '    Dim oListCmd() As Object
    '    Dim MyFunction As New generalClass.generalFunction
    '    Dim oMyImmobili() As ObjDettaglioTestata
    '    Dim oMyAgevolazioni() As ObjRidEseApplicati
    '    Dim x As Integer

    '    sScript = ""
    '    Abilita(True)
    '    oMyImmobili = Session("oListUITessera")
    '    'memorizzo il vano originale
    '    Try
    '        If Session("sOldListTessera") Is Nothing Then
    '            sOldListTessera = TxtCodUtente.Text + "|" + TxtCodInterno.Text + "|" + TxtNumeroTessera.Text + "|" + TxtDataRilascio.Text + "|" + TxtDataCessazione.Text + "|" + TxtNote.Text
    '            '*** 201511 - gestione tipo tessera ***
    '            sOldListTessera += "|" + DdlTipoTessera.SelectedValue
    '            '*** ***
    '            If oMyImmobili Is Nothing Then
    '                sOldListTessera += "|0"
    '            Else
    '                sOldListTessera += "|" + oMyImmobili.GetUpperBound(0).ToString
    '            End If
    '            oMyAgevolazioni = Session("oDatiRidTessera")
    '            If Not IsNothing(oMyAgevolazioni) Then
    '                sOldListTessera += "|" + oMyAgevolazioni.GetUpperBound(0).ToString
    '            Else
    '                sOldListTessera += "|-1"
    '            End If
    '            oMyAgevolazioni = Session("oDatiDetTessera")
    '            If Not IsNothing(oMyAgevolazioni) Then
    '                sOldListTessera += "|" + oMyAgevolazioni.GetUpperBound(0).ToString
    '            Else
    '                sOldListTessera += "|-1"
    '            End If
    '            Session("sOldListTessera") = sOldListTessera
    '        End If
    '        'abilito il pulsante di salvataggio
    '        ReDim Preserve oListCmd(0)
    '        oListCmd(0) = "Salva"
    '        For x = 0 To oListCmd.Length - 1
    '            sScript += "parent.Comandi.document.getElementById('" & oListCmd(x).ToString() & "').disabled=" & False.ToString.ToLower & ";"
    '        Next
    '        'abilito il pulsante di salvataggio
    '        ReDim Preserve oListCmd(1)
    '        oListCmd(0) = "Modifica"
    '        oListCmd(1) = "Delete"
    '        For x = 0 To oListCmd.Length - 1
    '            sScript += "parent.Comandi.document.getElementById('" & oListCmd(x).ToString() & "').disabled=" & True.ToString.ToLower & ";"
    '        Next
    '        RegisterScript(sScript, Me.GetType)
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTessere.CmdModTessera_Click.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Private Sub CmdDeleteTessera_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdDeleteTessera.Click
    '    Dim oListTessera() As ObjTessera
    '    Dim oListNewTessera() As ObjTessera
    '    Dim oMyTessera As New ObjTessera
    '    Dim nList As Integer = -1
    '    Dim FncTessera As New GestTessera
    '    Dim sScript, sNewListTessera As String
    '    Dim x As Integer

    '    Try
    '        'inizializzo la connessione
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If
    '        'carico l'array originario
    '        oListTessera = Session("oListTessere")
    '        'carico il nuovo oggetto senza la riga selezionata
    '        nList = -1
    '        For x = 0 To oListTessera.GetUpperBound(0)
    '            If oListTessera(x).Id <> Request.Item("IdUniqueTessera") Then
    '                nList += 1
    '                ReDim Preserve oListNewTessera(nList)
    '                oMyTessera = New ObjTessera
    '                oMyTessera = oListTessera(x)
    '                oListNewTessera(nList) = oMyTessera
    '            End If
    '        Next
    '        'carico la singola tessera selezionata
    '        oMyTessera = Session("oTessera")
    '        oMyTessera.tDataVariazione = Now
    '        oMyTessera.tDataCessazione = Now
    '        'aggiungo la nuova tessera
    '        If FncTessera.SetTessera(oMyTessera, 1, WFSessione) = 0 Then
    '            Response.Redirect("../../PaginaErrore.aspx")
    '            Exit Sub
    '        End If
    '        'memorizzo l'oggetto nella sessione
    '        Session("oListTessere") = oListNewTessera
    '        'aggiorno la pagina chiamante
    '        sScript = "alert('Eliminazione effettuata con successo!');" & vbCrLf
    '        sScript += BackToDichiarazione()
    '        RegisterScript(me.gettype(),"", "" & sScript & ";")
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTessere.CmdDeleteTessera_Click.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    Finally
    '        'chiudo la connessione
    '        If Not WFSessione Is Nothing Then
    '            WFSessione.Kill()
    '        End If
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdDeleteTessera_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdDeleteTessera.Click
        Dim oListTessera() As ObjTessera
        Dim oListNewTessera() As ObjTessera = Nothing
        Dim oMyTessera As New ObjTessera
        Dim nList As Integer = -1
        Dim FncTessera As New Utility.DichManagerTARSU(ConstSession.DBType, ConstSession.StringConnection, ConstSession.StringConnectionOPENgov, ConstSession.IdEnte) 'As New GestTessera
        Dim sScript As String
        Dim x As Integer

        Try
            'carico l'array originario
            oListTessera = Session("oListTessere")
            'carico il nuovo oggetto senza la riga selezionata
            nList = -1
            For x = 0 To oListTessera.GetUpperBound(0)
                If oListTessera(x).Id <> Request.Item("IdUniqueTessera") Then
                    nList += 1
                    ReDim Preserve oListNewTessera(nList)
                    oMyTessera = New ObjTessera
                    oMyTessera = oListTessera(x)
                    oListNewTessera(nList) = oMyTessera
                End If
            Next
            'carico la singola tessera selezionata
            oMyTessera = Session("oTessera")
            oMyTessera.tDataVariazione = Now
            oMyTessera.tDataCessazione = Now
            'aggiungo la nuova tessera
            If FncTessera.SetTessera(Utility.Costanti.AZIONE_UPDATE, oMyTessera, oMyTessera.IdContribuente) = 0 Then
                Response.Redirect("../../PaginaErrore.aspx")
                Exit Sub
            End If
            'memorizzo l'oggetto nella sessione
            Session("oListTessere") = oListNewTessera
            'aggiorno la pagina chiamante
            sScript = "GestAlert('a', 'success', '', '', 'Eliminazione effettuata con successo!');"
            sScript += BackToDichiarazione("N")
            RegisterScript(sScript, Me.GetType)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTessere.CmdDeleteTessera_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdClearDatiTessera_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdClearDatiTessera.Click
        Dim sScript As String
        Try
            'aggiorno la pagina chiamante
            sScript = BackToDichiarazione("N")
            RegisterScript(sScript, Me.GetType)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTessere.CmdClearDatiTessera_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    'Private Sub CmdLegaImmobili_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdLegaImmobili.Click
    '    '*** ***
    '    'ciclo sulla griglia 
    '    'se la riga ha il flag di associazione settato allora aggiorno l'immobile settandogli il legame con la tessera corrente 
    '    'altrimenti aggiorno l'immobile settandogli il legame con la tessera bidone
    '    '*** ***
    '    Dim oImmobile As ObjDettaglioTestata
    '    Dim sScript As String
    '    Dim itemGrid As GridViewRow
    '    Dim x, nIdTesseraBidone, nTesVis, nTesBid As Integer
    '    Dim oListImmobili() As ObjDettaglioTestata
    '    Dim oUITesVis() As ObjDettaglioTestata
    '    Dim oUITesBid() As ObjDettaglioTestata
    '    Dim FncTessera As New GestTessera
    '    Dim FncDettaglio As New GestDettaglioTestata
    '    Dim oListTessere() As ObjTessera

    '    Try
    '        'inizializzo la connessione
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        'prelevo gli immobili caricati in griglia
    '        oListImmobili = CType(Session("oListImmobili"), ObjDettaglioTestata())

    '        'prelevo la tessera bidone
    '        nIdTesseraBidone = FncTessera.GetTesseraBidone(WFSessione, ConstSession.IdEnte, GrdImmobili, TxtId.Text, TxtIdTestata.Text, ConstSession.UserName)

    '        nTesVis = 0 : nTesBid = 0
    '        'ciclo per vedere le associazioni degli immobili
    '        For Each itemGrid In GrdImmobili.Items
    '            If CType(itemGrid.FindControl("ChkAssociata"), CheckBox).Checked = True Then
    '                For x = 0 To oListImmobili.GetUpperBound(0)
    '                    If oListImmobili(x).Id = CType(itemGrid.Cells(10).Text, Long) Then
    '                        oListImmobili(x).IdTessera = TxtId.Text
    '                        'carico l'ui tra le ui tessera
    '                        ReDim Preserve oUITesVis(nTesVis)
    '                        oUITesVis(nTesVis) = oListImmobili(x)
    '                        nTesVis += 1
    '                        Exit For
    '                    End If
    '                Next
    '            Else
    '                For x = 0 To oListImmobili.GetUpperBound(0)
    '                    If oListImmobili(x).Id = CType(itemGrid.Cells(10).Text, Long) Then
    '                        oListImmobili(x).IdTessera = nIdTesseraBidone
    '                        'carico l'ui tra le ui bidone
    '                        ReDim Preserve oUITesBid(nTesBid)
    '                        oUITesBid(nTesBid) = oListImmobili(x)
    '                        nTesBid += 1
    '                        Exit For
    '                    End If
    '                Next
    '            End If
    '        Next
    '        'aggiorno il database
    '        For x = 0 To oListImmobili.GetUpperBound(0)
    '            If FncDettaglio.UpdateDettaglioTestata(WFSessione, oListImmobili(x)) < 1 Then
    '                Response.Redirect("../../PaginaErrore.aspx")
    '                Exit Sub
    '            End If
    '        Next

    '        'aggiorno l'array originario di tessere
    '        oListTessere = Session("oListTessere")
    '        'carico la singola tessera selezionata
    '        For x = 0 To oListTessere.GetUpperBound(0)
    '            If oListTessere(x).Id = Request.Item("IdUniqueTessera") Then
    '                'aggiorno gli immobili
    '                oListTessere(x).oImmobili = oUITesVis
    '            ElseIf oListTessere(x).sNumeroTessera = ObjTessera.TESSERA_BIDONE Then
    '                'aggiorno gli immobili
    '                oListTessere(x).oImmobili = oUITesBid
    '            End If
    '        Next
    '        Session("oListTessere") = oListTessere

    '        'aggiorno la pagina chiamante
    '        sScript = "alert('Associazione effettuata con successo!');" & vbCrLf
    '        sScript += BackToDichiarazione()
    '        RegisterScript(me.gettype(),"", "" & sScript & ";")
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTessere.CmdLegaImmobili_Click.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    Finally
    '        'chiudo la connessione
    '        If Not WFSessione Is Nothing Then
    '            WFSessione.Kill()
    '        End If
    '    End Try
    'End Sub
    ''' <summary>
    ''' ciclo sulla griglia se la riga ha il flag di associazione settato allora aggiorno l'immobile settandogli il legame con la tessera corrente altrimenti aggiorno l'immobile settandogli il legame con la tessera bidone
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdLegaImmobili_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdLegaImmobili.Click
        'Dim oImmobile As ObjDettaglioTestata
        Dim sScript As String
        Dim itemGrid As GridViewRow
        Dim x, nIdTesseraBidone, nTesVis, nTesBid As Integer
        Dim oListImmobili() As ObjDettaglioTestata
        Dim oUITesVis() As ObjDettaglioTestata = Nothing
        Dim oUITesBid() As ObjDettaglioTestata = Nothing
        Dim FncTessera As New GestTessera
        Dim FncDettaglio As New Utility.DichManagerTARSU(ConstSession.DBType, ConstSession.StringConnection, ConstSession.StringConnectionOPENgov, ConstSession.IdEnte) 'As New GestDettaglioTestata
        Dim oListTessere() As ObjTessera

        Try
            'prelevo gli immobili caricati in griglia
            oListImmobili = CType(Session("oListImmobili"), ObjDettaglioTestata())

            'prelevo la tessera bidone
            nIdTesseraBidone = FncTessera.GetTesseraBidone(ConstSession.DBType, ConstSession.StringConnection, ConstSession.IdEnte, GrdImmobili, TxtId.Text, TxtIdTestata.Text, ConstSession.UserName)

            nTesVis = 0 : nTesBid = 0
            'ciclo per vedere le associazioni degli immobili
            For Each itemGrid In GrdImmobili.Rows
                If CType(itemGrid.FindControl("ChkAssociata"), CheckBox).Checked = True Then
                    For x = 0 To oListImmobili.GetUpperBound(0)
                        If oListImmobili(x).Id = CType(CType(itemGrid.FindControl("hfID"), HiddenField).Value, Long) Then
                            oListImmobili(x).IdTessera = TxtId.Text
                            'carico l'ui tra le ui tessera
                            ReDim Preserve oUITesVis(nTesVis)
                            oUITesVis(nTesVis) = oListImmobili(x)
                            nTesVis += 1
                            Exit For
                        End If
                    Next
                Else
                    For x = 0 To oListImmobili.GetUpperBound(0)
                        If oListImmobili(x).Id = CType(CType(itemGrid.FindControl("hfID"), HiddenField).Value, Long) Then
                            oListImmobili(x).IdTessera = nIdTesseraBidone
                            'carico l'ui tra le ui bidone
                            ReDim Preserve oUITesBid(nTesBid)
                            oUITesBid(nTesBid) = oListImmobili(x)
                            nTesBid += 1
                            Exit For
                        End If
                    Next
                End If
            Next
            'aggiorno il database
            For x = 0 To oListImmobili.GetUpperBound(0)
                If FncDettaglio.SetDettaglioTestata(Utility.Costanti.AZIONE_UPDATE, oListImmobili(x)) < 1 Then 'If FncDettaglio.UpdateDettaglioTestata(ConstSession.StringConnection, oListImmobili(x)) < 1 Then
                    Response.Redirect("../../PaginaErrore.aspx")
                    Exit Sub
                End If
            Next

            'aggiorno l'array originario di tessere
            oListTessere = Session("oListTessere")
            'carico la singola tessera selezionata
            For x = 0 To oListTessere.GetUpperBound(0)
                If oListTessere(x).Id = Request.Item("IdUniqueTessera") Then
                    'aggiorno gli immobili
                    oListTessere(x).oImmobili = oUITesVis
                ElseIf oListTessere(x).sNumeroTessera = ObjTessera.TESSERA_BIDONE Then
                    'aggiorno gli immobili
                    oListTessere(x).oImmobili = oUITesBid
                End If
            Next
            Session("oListTessere") = oListTessere

            'aggiorno la pagina chiamante
            sScript = "GestAlert('a', 'success', '', '', 'Associazione effettuata con successo!');"
            sScript += BackToDichiarazione("N")
            RegisterScript(sScript, Me.GetType)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTessere.CmdLegaImmobili_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

#Region "Griglie"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdRowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            If (e.Row.RowType = DataControlRowType.Header) Then
                e.Row.Cells(6).Text = "Tot.MQ " & ConstSession.NameSistemaTerritorio
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTessere.GrdRowDataBound.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim IDRow As String = e.CommandArgument.ToString()
            If e.CommandName = "RowOpen" Then
                For Each myRow As GridViewRow In GrdImmobili.Rows
                    If IDRow = CType(myRow.FindControl("hfid"), HiddenField).Value Then
                        ShowPopUp("I", CInt(IDRow), myRow.RowIndex, Utility.Costanti.AZIONE_LETTURA)
                    End If
                Next
            ElseIf e.CommandName = "RowDelete" Then
                If CType(sender, Ribes.OPENgov.WebControls.RibesGridView).UniqueID = "GrdRiduzioni" Then
                Else
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTessere.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub GrdImmobili_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdImmobili.ItemDataBound
    '    If e.Item.ItemType = ListItemType.Header Then
    '        e.Item.Cells(6).Text = "Tot.MQ " & ConstSession.NameSistemaTerritorio
    '    End If
    'End Sub
    'Private Sub GrdImmobili_Update(ByVal sender As Object, ByVal e As DataGridCommandEventArgs) Handles GrdImmobili.UpdateCommand
    '    Try
    '        'apro il popup passandogli l'indice dell'array da visualizzare
    '        ShowPopUp("I", CInt(e.Item.Cells(10).Text), e.Item.ItemIndex, Utility.Costanti.AZIONE_LETTURA)
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTessere.GrdImmobili_ItemDataBound.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
#End Region

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="bTypeAbilita"></param>
    ''' <revisionHistory>
    ''' <revision date="11/2015">
    ''' gestione tipo tessera
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Private Sub Abilita(ByVal bTypeAbilita As Boolean)
        Dim sScript As String = ""
        Dim oListCmd() As Object
        Dim x As Integer

        'disabilito i dati
        TxtCodUtente.Enabled = bTypeAbilita : TxtCodInterno.Enabled = bTypeAbilita : TxtNumeroTessera.Enabled = bTypeAbilita
        TxtDataRilascio.Enabled = bTypeAbilita : TxtDataCessazione.Enabled = bTypeAbilita : TxtNote.Enabled = bTypeAbilita
        DdlTipoTessera.Enabled = bTypeAbilita
        'se sono sulla tessera bidone non posso usare il pulsante per associare/disassociare le ui
        Try
            If TxtNumeroTessera.Text = ObjTessera.TESSERA_BIDONE Then
                ReDim Preserve oListCmd(0)
                oListCmd(0) = "Switch"
                For x = 0 To oListCmd.Length - 1
                    sScript += "$('#" + oListCmd(x).ToString() + "', parent.frames['Comandi'].document).addClass('DisableBtn');"
                Next
                RegisterScript(sScript, Me.GetType)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTessere.Abilita.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub Abilita(ByVal bTypeAbilita As Boolean)
    '    Dim sScript As String = ""
    '    Dim oListCmd() As Object
    '    Dim x As Integer

    '    'disabilito i dati
    '    TxtCodUtente.Enabled = bTypeAbilita : TxtCodInterno.Enabled = bTypeAbilita : TxtNumeroTessera.Enabled = bTypeAbilita
    '    TxtDataRilascio.Enabled = bTypeAbilita : TxtDataCessazione.Enabled = bTypeAbilita : TxtNote.Enabled = bTypeAbilita
    '    '*** 201511 - gestione tipo tessera ***
    '    DdlTipoTessera.Enabled = bTypeAbilita
    '    '*** ***
    '    'se sono sulla tessera bidone non posso usare il pulsante per associare/disassociare le ui
    '    Try
    '        If TxtNumeroTessera.Text = ObjTessera.TESSERA_BIDONE Then
    '            ReDim Preserve oListCmd(0)
    '            oListCmd(0) = "Switch"
    '            For x = 0 To oListCmd.Length - 1
    '                sScript += "parent.Comandi.document.getElementById('" & oListCmd(x).ToString() & "').disabled=" & True.ToString.ToLower & ";"
    '            Next
    '            RegisterScript(sScript, Me.GetType)
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTessere.Abilita.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    '*** 20140805 - Gestione Categorie Vani ***
    'Private Sub ShowPopUp(ByVal sTipoPopUP As String, ByVal nIdUI As Integer, ByVal sAzione As String)
    '    Dim oMyTessera As New ObjTessera
    '    Dim oListTessera() As ObjTessera = Nothing
    '    Dim nList As Integer = -1
    '    Dim bTrovato As Boolean = False

    '    Try
    '        'carico i dati dal form
    '        oMyTessera.Id = TxtId.Text
    '        oMyTessera.IdTessera = TxtIdTessera.Text
    '        oMyTessera.IdTestata = TxtIdTestata.Text
    '        oMyTessera.sCodUtente = TxtCodUtente.Text
    '        oMyTessera.sCodInterno = TxtCodInterno.Text
    '        oMyTessera.sNumeroTessera = TxtNumeroTessera.Text
    '        If TxtDataRilascio.Text <> "" Then
    '            oMyTessera.tDataRilascio = TxtDataRilascio.Text
    '        End If
    '        oMyTessera.sNote = TxtNote.Text
    '        oMyTessera.tDataInserimento = Now
    '        If TxtDataCessazione.Text <> "" Then
    '            oMyTessera.tDataCessazione = TxtDataCessazione.Text
    '        End If
    '        oMyTessera.sOperatore = ConstSession.UserName
    '        oMyTessera.oImmobili = Session("oListUITessera")
    '        'memorizzo l'oggetto nella sessione
    '        Session("oTessera") = oMyTessera

    '        'controllo se ho già delle tessere caricate
    '        If Not Session("oListTessere") Is Nothing Then
    '            'carico l'array originario
    '            oListTessera = Session("oListTessere")
    '            nList = oListTessera.GetUpperBound(0)
    '        End If
    '        If Not oListTessera Is Nothing Then
    '            For Each oTessera As ObjTessera In oListTessera
    '                If oTessera.Id = oMyTessera.Id Then
    '                    oTessera = oMyTessera
    '                    bTrovato = True
    '                End If
    '            Next
    '        End If
    '        If bTrovato = False Then
    '            'aggiungo una riga
    '            nList += 1
    '            'dimensiono l'array
    '            ReDim Preserve oListTessera(nList)
    '            'carico l'array con i dati della videata
    '            oListTessera(nList) = oMyTessera
    '        End If
    '        'memorizzo l'oggetto nella sessione
    '        Session("oListTessere") = oListTessera

    '        'apro il popup passandogli l'indice dell'array da visualizzare
    '        Dim sScript, sProvenienza As String
    '        sProvenienza = Costanti.FORMPROVENIENZA.TESSERA
    '        If sTipoPopUP = "I" Then
    '            sScript = "ShowInsertUI('" & Request.Item("IdContribuente") & "','" & oMyTessera.Id & "','" & nIdUI & "','" & sAzione & "','" & sProvenienza & "')"
    '            RegisterScript(me.gettype(),"", "" & sScript & ";")
    '        ElseIf sTipoPopUP = "A" Then
    '            sScript = "ShowRicUIAnater()"
    '            RegisterScript(me.gettype(),"", "" & sScript & ";")
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTessere.ShowPopUp.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sTipoPopUP"></param>
    ''' <param name="nIdUI"></param>
    ''' <param name="nIdList"></param>
    ''' <param name="sAzione"></param>
    Private Sub ShowPopUp(ByVal sTipoPopUP As String, ByVal nIdUI As Integer, ByVal nIdList As Integer, ByVal sAzione As String)
        Dim oMyTessera As New ObjTessera
        Dim oListTessera() As ObjTessera = Nothing
        Dim nList As Integer = -1
        Dim bTrovato As Boolean = False

        Try
            'carico i dati dal form
            oMyTessera.Id = TxtId.Text
            oMyTessera.IdTessera = TxtIdTessera.Text
            oMyTessera.IdTestata = TxtIdTestata.Text
            oMyTessera.sCodUtente = TxtCodUtente.Text
            oMyTessera.sCodInterno = TxtCodInterno.Text
            oMyTessera.sNumeroTessera = TxtNumeroTessera.Text
            If TxtDataRilascio.Text <> "" Then
                oMyTessera.tDataRilascio = TxtDataRilascio.Text
            End If
            oMyTessera.sNote = TxtNote.Text
            oMyTessera.tDataInserimento = Now
            If TxtDataCessazione.Text <> "" Then
                oMyTessera.tDataCessazione = TxtDataCessazione.Text
            End If
            oMyTessera.sOperatore = ConstSession.UserName
            oMyTessera.oImmobili = Session("oListUITessera")
            '*** 201511 - gestione tipo tessera ***
            oMyTessera.IdTipoTessera = DdlTipoTessera.SelectedValue
            '*** ***
            'memorizzo l'oggetto nella sessione
            Session("oTessera") = oMyTessera

            'controllo se ho già delle tessere caricate
            If Not Session("oListTessere") Is Nothing Then
                'carico l'array originario
                oListTessera = Session("oListTessere")
                nList = oListTessera.GetUpperBound(0)
            End If
            If Not oListTessera Is Nothing Then
                For Each oTessera As ObjTessera In oListTessera
                    If oTessera.Id = oMyTessera.Id Then
                        oTessera = oMyTessera
                        bTrovato = True
                    End If
                Next
            End If
            If bTrovato = False Then
                'aggiungo una riga
                nList += 1
                'dimensiono l'array
                ReDim Preserve oListTessera(nList)
                'carico l'array con i dati della videata
                oListTessera(nList) = oMyTessera
            End If
            'memorizzo l'oggetto nella sessione
            Session("oListTessere") = oListTessera

            'apro il popup passandogli l'indice dell'array da visualizzare
            Dim sScript, sProvenienza As String
            sProvenienza = Costanti.FormProvenienza.Tessera
            If sTipoPopUP = "I" Then
                sScript = "ShowInsertUI('" & Request.Item("IdContribuente") & "','" & oMyTessera.IdTestata & "','" & oMyTessera.Id & "','" & nIdUI & "','" & sAzione & "','" & sProvenienza & "','" & nIdList & "','" & ConstSession.IsFromVariabile & "')"
                RegisterScript(sScript, Me.GetType)
            ElseIf sTipoPopUP = "A" Then
                sScript = "ShowRicUIAnater()"
                RegisterScript(sScript, Me.GetType)
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTessere.ShowPopUp.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** ***
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub SettaCheckbox()
        Try
            For Each myRow As GridViewRow In GrdImmobili.Rows
                If TxtId.Text = CType(myRow.FindControl("hfIDTESSERA"), HiddenField).Value Or CType(myRow.FindControl("hfIDTESSERA"), HiddenField).Value = "-1" Then
                    CType(myRow.FindControl("ChkAssociata"), CheckBox).Checked = True
                Else
                    CType(myRow.FindControl("ChkAssociata"), CheckBox).Checked = False
                End If
            Next
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTessere.SettaCheckBox.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sProvenienza"></param>
    ''' <returns></returns>
    Private Function BackToDichiarazione(ByVal sProvenienza As String) As String
        Dim sScript As String = ""
        Dim FncTessere As New GestTessera
        Dim oMyTestata As New ObjTestata

        Try
            'devo popolare l'oggetto TESSERAUI altrimenti non viene visualizzata correttamente la pagina della dichiarazione
            If Not Session("oTestata") Is Nothing Then
                oMyTestata = Session("oTestata")
                If Not Session("oListTessere") Is Nothing Then
                    oMyTestata.oTessere = Session("oListTessere")
                End If
            End If
            Log.Debug("GestTessere::BackToDichiarazione::oTessere.len::" & oMyTestata.oTessere.GetUpperBound(0).ToString())
            oMyTestata.oTesUI = FncTessere.GetTesVSUI(oMyTestata)
            Session("oTestata") = oMyTestata

            Session("oListImmobili") = Nothing ': Session("oListUITessera") = Nothing
            Session("oTessera") = Nothing
            sScript = "parent.Visualizza.location.href='GestDichiarazione.aspx?IdUniqueTestata=" & TxtIdTestata.Text & "&AzioneProv=" & Request.Item("AzioneProv") & "';" & vbCrLf
            sScript += "parent.Comandi.location.href='ComandiGestDichiarazioni.aspx?sProvenienza=" & sProvenienza & "';"
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTessere.BackToDichiarazione.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
        Return sScript
    End Function
    '*** 201511 - gestione tipo tessera ***
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub LoadPageCombos()
        Dim sSQL As String
        Dim oLoadCombo As New generalClass.generalFunction

        Try
            sSQL = "SELECT *"
            sSQL += " FROM V_TIPOTESSERA"
            sSQL += " ORDER BY DESCRIZIONE"
            oLoadCombo.LoadComboGenerale(DdlTipoTessera, sSQL, ConstSession.StringConnection, False, Costanti.TipoDefaultCmb.NUMERO)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestTessere.LoadPageCombos.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** ***
End Class
