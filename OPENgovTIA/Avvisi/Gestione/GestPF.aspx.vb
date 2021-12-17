Imports log4net
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
''' <summary>
''' Pagina per la visualizzazione/gestione degli articoli dell'avviso.
''' Contiene i dati di dettaglio e le funzioni della comandiera.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class GestPF
    Inherits BaseEnte
    Public UrlStradario As String = ConstSession.UrlStradario
    Private Shared Log As ILog = LogManager.GetLogger(GetType(GestPF))
    Private sScript As String
    Private FncArticolo As New GestArticolo

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Label11 As System.Web.UI.WebControls.Label
    Protected WithEvents Textbox1 As System.Web.UI.WebControls.TextBox

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
        Try
            lblTitolo.Text = ConstSession.DescrizioneEnte
            If ConstSession.IsFromTARES = "1" Then
                info.InnerText = "TARI " + info.InnerText
            Else
                info.InnerText = "TARSU " + info.InnerText
            End If
            '*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI & DIVERSA GESTIONE QUOTA VARIABILE***
            If ConstSession.HasDummyDich Then
                LblForzaPV.Style.Add("display", "none")
                ChkForzaPV.Style.Add("display", "none")
            End If

            If ConstSession.HasPlainAnag Then
                sScript += "document.getElementById('TRSpecAnag').style.display='none';"
            Else
                sScript += "document.getElementById('TRPlainAnag').style.display='none';"
            End If
            sScript += "parent.Comandi.location.href = '../../../aspVuotaRemoveComandi.aspx';"
            sScript += "parent.Basso.location.href = '../../../aspVuotaRemoveComandi.aspx';"
            sScript += "parent.Nascosto.location.href = '../../../aspVuotaRemoveComandi.aspx';"
            RegisterScript(sScript, Me.GetType)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPF.Page_Init.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub

#End Region
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            If TxtViaRibaltata.Text <> "" Then
                TxtVia.Text = TxtViaRibaltata.Text
            End If
            LnkOpenStradario.Attributes.Add("onclick", "return ApriStradario('RibaltaStrada', '" & ConstSession.IdEnte & "')")
            'LnkNewUIAnater.Attributes.Add("onclick", "ShowRicUIAnater()")
            'LnkNewUIAnater.ToolTip = "Nuovo Immobile da " & ConstSession.NameSistemaTerritorio
            LnkNewRid.Attributes.Add("OnClick", "ShowInsertRidDet('" & ObjCodDescr.TIPO_RIDUZIONI & "')")
            LnkNewDet.Attributes.Add("OnClick", "ShowInsertRidDet('" & ObjCodDescr.TIPO_ESENZIONI & "')")

            If Page.IsPostBack = False Then
                TxtIdFlussoRuolo.Text = Request.Item("idFlussoRuolo")
                LoadDatiAgenziaEntrate()
                'WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
                'If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
                '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
                'End If

                'controllo se sono in visualizzazione di una posizione
                If Request.Item("AzioneProv") = Utility.Costanti.AZIONE_UPDATE Then
                    LoadArticolo(Request.Item("IdUniqueIdArticolo"))
                ElseIf Request.Item("AzioneProv") = Utility.Costanti.AZIONE_NEW Then
                    LoadNewPartita(Request.Item("TipoPartita"))
                Else
                    Abilita(False, 1)
                End If
                'controllo se devo caricare la griglia delle riduzioni
                If Not Session("oDatiRid") Is Nothing Then
                    GrdRiduzioni.Style.Add("display", "")
                    GrdRiduzioni.DataSource = Session("oDatiRid")
                    GrdRiduzioni.DataBind()
                    LblResultRid.Style.Add("display", "none")
                Else
                    GrdRiduzioni.Style.Add("display", "none")
                    LblResultRid.Style.Add("display", "")
                End If
                'BD 4/10/2021
                Dim oListArticoli() As ObjArticolo
                Dim MyArticolo As New ObjArticolo
                oListArticoli = Session("oArticolo")
                MyArticolo = oListArticoli(0)
                TxtRidImp.Text = MyArticolo.ImportoFissoRid
                'BD 4/10/2021

                'controllo se devo caricare la griglia delle Detassazioni
                If Not Session("oDatiDet") Is Nothing Then
                    GrdDetassazioni.Style.Add("display", "")
                    GrdDetassazioni.DataSource = Session("oDatiDet")
                    GrdDetassazioni.DataBind()
                    LblResultDet.Style.Add("display", "none")
                Else
                    GrdDetassazioni.Style.Add("display", "none")
                    LblResultDet.Style.Add("display", "")
                End If
            End If
            'carico l'anagrafica
            '*** 201504 - Nuova Gestione anagrafica con form unico ***
            hdIdContribuente.Value = CInt(Request.Item("IdContribuente"))
            If ConstSession.HasPlainAnag Then
                ifrmAnag.Attributes.Add("src", "../../../Generali/asp/VisualAnag.aspx?IdContribuente=" & TxtCodContribuente.Text & "&Azione=" & Utility.Costanti.AZIONE_LETTURA)
            Else
                sScript += "LoadAnagrafica.location.href='../../Generali/DatiContribuente.aspx?IdContribuente=" & TxtCodContribuente.Text & "';"
                RegisterScript(sScript, Me.GetType)
            End If
            '*** ***
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPF.Page_Load.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdClearDatiArticolo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdClearDatiArticolo.Click
        Try
            'aggiorno la pagina chiamante
            'ripulisco tutti i dati di sessioni dati articolo
            ClearDatiRuolo()
            Dim sParam As String = ""
            sParam = "IdUniqueAvviso = " & TxtCodCartella.Text & " & AzioneProv = " & Utility.Costanti.AZIONE_UPDATE
            sParam += "&IsFromVariabile=" + ConstSession.IsFromVariabile
            sScript = "parent.Visualizza.location.href='GestAvvisi.aspx?" & sParam & "';"
            sScript += "parent.Comandi.location.href = '../../../aspVuotaRemoveComandi.aspx';"
            sScript += "parent.Basso.location.href = '../../../aspVuotaRemoveComandi.aspx';"
            sScript += "parent.Nascosto.location.href = '../../../aspVuotaRemoveComandi.aspx';"
            RegisterScript( sScript,Me.GetType)
        Catch Err As Exception
            Log.Debug("Si è verificato un errore in GestPF::CmdClearDatiArticolo_Click::" & Err.Message)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdModifica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdModifica.Click
        Try
            Abilita(True, 0)
            Abilita(False, 1)
            'memorizzo la dichiarazione originale
            Session("oArticoloOrg") = Session("oArticolo")
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPF.CmdModifica_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub

    '*** 20120917 - sgravi ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdSgravi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdSgravi.Click
        Dim oListArticoliOrg() As ObjArticolo
        Dim oArticoloSgravi As New ObjArticolo
        Dim oListArticoli() As ObjArticolo
        Dim x As Integer
        Dim sErrDati As String = ""

        Try
            If ControlloDatiArticolo(sErrDati) = False Then
                sScript = "GestAlert('a', 'warning', '', '', '" & sErrDati & "');"
                RegisterScript(sScript, Me.GetType)
            Else
                'carico l'articolo originale
                oListArticoliOrg = Session("oArticolo")
                'prelevo i dati dell'articolo
                oArticoloSgravi.Id = TxtId.Text
                oArticoloSgravi.IdArticolo = TxtIdArticolo.Text
                oArticoloSgravi.IdAvviso = TxtCodCartella.Text
                oArticoloSgravi.IdEnte = ConstSession.IdEnte
                oArticoloSgravi.IdContribuente = TxtCodContribuente.Text
                oArticoloSgravi.IdDettaglioTestata = TxtIdDettaglioTestata.Text
                oArticoloSgravi.TipoPartita = TxtTipoPartita.Text
                oArticoloSgravi.nCodVia = TxtCodVia.Text
                oArticoloSgravi.sVia = TxtVia.Text
                If TxtCivico.Text <> "" Then
                    oArticoloSgravi.sCivico = TxtCivico.Text
                End If
                oArticoloSgravi.sEsponente = TxtEsponente.Text
                oArticoloSgravi.sInterno = TxtInterno.Text
                oArticoloSgravi.sScala = TxtScala.Text
                oArticoloSgravi.sFoglio = TxtFoglio.Text
                oArticoloSgravi.sNumero = TxtNumero.Text
                oArticoloSgravi.sSubalterno = TxtSubalterno.Text
                If TxtGGTarsu.Text <> "" Then
                    oArticoloSgravi.nBimestri = TxtGGTarsu.Text
                End If
                oArticoloSgravi.sAnno = TxtAnno.Text
                If TxtDataInizio.Text <> "" Then
                    oArticoloSgravi.tDataInizio = TxtDataInizio.Text
                Else
                    oArticoloSgravi.tDataInizio = "01/01" & oArticoloSgravi.sAnno
                End If
                If TxtDataFine.Text <> "" Then
                    oArticoloSgravi.tDataFine = TxtDataFine.Text
                Else
                    oArticoloSgravi.tDataFine = "31/12/" & oArticoloSgravi.sAnno
                End If
                oArticoloSgravi.bIsTarsuGiornaliera = ChkIsGiornaliera.Checked
                If TxtNComponenti.Text <> "" Then
                    oArticoloSgravi.nComponenti = TxtNComponenti.Text
                End If
                oArticoloSgravi.sCategoria = DDlCatAteco.SelectedValue
                oArticoloSgravi.sDescrCategoria = DDlCatAteco.SelectedItem.Text
                oArticoloSgravi.nIdTariffa = TxtIdTariffa.Text
                If TxtTariffa.Text <> "" Then
                    oArticoloSgravi.impTariffa = FormatNumber(TxtTariffa.Text, 6)
                End If
                If TxtNComponenti.Text <> "" And oArticoloSgravi.sDescrCategoria.ToUpper.IndexOf("DOM") >= 0 Then
                    oArticoloSgravi.nComponenti = TxtNComponenti.Text
                End If
                If TxtNComponentiPV.Text <> "" And oArticoloSgravi.sDescrCategoria.ToUpper.IndexOf("DOM") >= 0 Then
                    oArticoloSgravi.nComponentiPV = TxtNComponentiPV.Text
                End If
                oArticoloSgravi.bForzaPV = ChkForzaPV.Checked
                If TxtMQTassabili.Text <> "" Then
                    oArticoloSgravi.nMQ = TxtMQTassabili.Text
                End If
                If TxtImpArticolo.Text <> "" Then
                    oArticoloSgravi.impRuolo = FormatNumber(TxtImpArticolo.Text, 2)
                End If
                oArticoloSgravi.impNetto = FormatNumber(TxtImpNetto.Text, 2)
                oArticoloSgravi.bIsImportoForzato = ChkImpForzato.Checked
                oArticoloSgravi.IdAvviso = TxtCodCartella.Text
                oArticoloSgravi.sNote = TxtNote.Text
                '***Agenzia Entrate***
                oArticoloSgravi.sSezione = TxtSezione.Text
                oArticoloSgravi.sEstensioneParticella = TxtEstParticella.Text
                oArticoloSgravi.sIdTipoParticella = DdlTipoParticella.SelectedValue
                If DdlTitOccupaz.SelectedValue <> "" Then
                    oArticoloSgravi.nIdTitoloOccupaz = DdlTitOccupaz.SelectedValue
                End If
                If DdlNatOccupaz.SelectedValue <> "" Then
                    oArticoloSgravi.nIdNaturaOccupaz = DdlNatOccupaz.SelectedValue
                End If
                If DdlDestUso.SelectedValue <> "" Then
                    oArticoloSgravi.nIdDestUso = DdlDestUso.SelectedValue
                End If
                If DdlTipoUnita.SelectedValue <> "" Then
                    oArticoloSgravi.sIdTipoUnita = DdlTipoUnita.SelectedValue
                End If
                If DdlAssenzaDatiCat.SelectedValue <> "" Then
                    oArticoloSgravi.nIdAssenzaDatiCatastali = DdlAssenzaDatiCat.SelectedValue
                End If

                ' BD 04/10/2021
                oArticoloSgravi.ImportoFissoRid = TxtRidImp.Text
                ' BD 04/10/2021

                '*********************
                'memorizzo l'oggetto in sessione
                oArticoloSgravi.oRiduzioni = Session("oDatiRid")
                If Not oArticoloSgravi.oRiduzioni Is Nothing Then
                    'mi appoggio ad operatore per utilizzarlo come ordinamento
                    Dim n As Integer
                    For n = 0 To oArticoloSgravi.oRiduzioni.GetUpperBound(0)
                        oArticoloSgravi.sOperatore += oArticoloSgravi.oRiduzioni(n).sCodice + "|"
                    Next
                End If
                'memorizzo l'oggetto in sessione
                oArticoloSgravi.oDetassazioni = Session("oDatiDet")
                If Not oArticoloSgravi.oDetassazioni Is Nothing Then
                    'mi appoggio ad operatore per utilizzarlo come ordinamento
                    Dim n As Integer
                    For n = 0 To oArticoloSgravi.oDetassazioni.GetUpperBound(0)
                        oArticoloSgravi.sOperatore += oArticoloSgravi.oDetassazioni(n).sCodice + "|"
                    Next
                End If
                '*** 20141211 - legami PF-PV ***
                oArticoloSgravi.IdOggetto = oListArticoliOrg(0).IdOggetto
                '*** ***
                'aggiorno gli articoli sull'avviso
                If Session("oListArticoliSgravi") Is Nothing Then
                    oListArticoli = Session("oListArticoli")
                Else
                    oListArticoli = Session("oListArticoliSgravi")
                End If
                'se sono un una nuova posizione aggiorno l'id con un numero fittizio perché non serve + identificarlo come nuovo
                If oArticoloSgravi.Id = -1 Then
                    'ORDINO PER partita+id
                    Array.Sort(oListArticoli, New Utility.Comparatore(New String() {"Id"}, New Boolean() {Utility.TipoOrdinamento.Decrescente}))
                    For x = 0 To oListArticoli.GetUpperBound(0)
                        oArticoloSgravi.Id = oListArticoli(x).Id + 1
                        Exit For
                    Next
                    'aggiungo nuova posizione ad array
                    Dim nList As Integer = oListArticoli.GetUpperBound(0) + 1
                    ReDim Preserve oListArticoli(nList)
                    oListArticoli(nList) = oArticoloSgravi
                End If

                For x = 0 To oListArticoli.GetUpperBound(0)
                    If oListArticoli(x).Id = oArticoloSgravi.Id Then
                        oArticoloSgravi.TipoPartita = oListArticoli(x).TipoPartita
                        oListArticoli(x) = oArticoloSgravi

                        'Exit For
                    Else
                        'forzo le date di inizio e fine se mancano
                        If oListArticoli(x).tDataInizio = DateTime.MinValue Then
                            oListArticoli(x).tDataInizio = "01/01/" & oArticoloSgravi.sAnno
                        End If
                        If oListArticoli(x).tDataFine = DateTime.MinValue Then
                            oListArticoli(x).tDataFine = "31/12/" & oArticoloSgravi.sAnno
                        End If
                        If Not oListArticoli(x).oRiduzioni Is Nothing Then
                            'mi appoggio ad operatore per utilizzarlo come ordinamento
                            Dim n As Integer
                            For n = 0 To oListArticoli(x).oRiduzioni.GetUpperBound(0)
                                oListArticoli(x).sOperatore += oListArticoli(x).oRiduzioni(n).sCodice + "|"
                            Next
                        End If
                        If Not oListArticoli(x).oDetassazioni Is Nothing Then
                            'mi appoggio ad operatore per utilizzarlo come ordinamento
                            Dim n As Integer
                            For n = 0 To oListArticoli(x).oDetassazioni.GetUpperBound(0)
                                oListArticoli(x).sOperatore += oListArticoli(x).oDetassazioni(n).sCodice + "|"
                            Next
                        End If
                    End If
                Next

                Dim FncElab As New ClsElabRuolo
                If FncElab.RicalcoloAvviso(ConstSession.StringConnection, oListArticoli, sScript, LblTipoCalcolo.Text, ChkMaggiorazione.Checked, ChkConferimenti.Checked, bp_TipoRuolo) = False Then
                    RegisterScript(sScript, Me.GetType)
                    Exit Sub
                End If
                'aggiorno la pagina chiamante
                'ripulisco tutti i dati di sessioni dati articolo
                ClearDatiRuolo()
                sScript += "GestAlert('a', 'info', 'CmdClearDatiArticolo', '', 'Procedura di Sgravio Inizializzata!\nPer rendere effettive le modifiche è necessario chiudere la procedura di sgravio\nattraverso l\'apposito pulsante nella videata di Gestione Avviso!');"
                RegisterScript(sScript, Me.GetType)
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPF.CmdSgravi_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub

    'Private Function GenPartiteTARES(ByVal myPrec As ObjArticolo, ByVal myArticolo As ObjArticolo, ByVal oArticoloSgravi As ObjArticolo, ByRef nMQ As Double, ByRef nMQPM As Double, ByRef newArticoli As ArrayList) As Boolean
    '    Dim CurrentItem As New ObjUnitaImmobiliare
    '    Try
    '        'CurrentItem = GeneraPV(myPrec, nMQ, LblTipoCalcolo.Text, oArticoloSgravi.oRiduzioni, oArticoloSgravi.oDetassazioni)
    '        CurrentItem = GeneraPV(myPrec, nMQ, LblTipoCalcolo.Text)
    '        If Not CurrentItem Is Nothing Then
    '            newArticoli.Add(CurrentItem)
    '        Else
    '            Return False
    '        End If
    '        'devo creare nuova pm x ultime pf passate
    '        CurrentItem = GeneraPM(myPrec, nMQPM, LblTipoCalcolo.Text)
    '        If Not CurrentItem Is Nothing Then
    '            newArticoli.Add(CurrentItem)
    '        Else
    '            Return False
    '        End If
    '        Return True
    '    Catch ex As Exception
    ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPF.GenPartiteTARES.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '        Return False
    '    End Try
    'End Function

    '*** 201511 - tolto RIBESFRAMEWORK ***
    'Private Sub LoadDatiAgenziaEntrate()
    '    Dim WFErrore As String
    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Dim oLoadCombo As New generalClass.generalFunction
    '    Dim FncAE As New OPENgov_AgenziaEntrate.OPENgov_AgenziaEntrate.General
    '    Dim dvDati As New DataView

    '    Try
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConfigurationManager.AppSettings("OPENGOVG").ToString())
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If
    '        'popolo la combo richiamando la funzione LoadComboDati("TIT_OCCUPAZIONE", sIdEnte, DdlTitOccupazione);
    '        dvDati = FncAE.LoadComboDati("TIT_OCCUPAZIONE", utility.Costanti.TRIBUTOTARSU, WFSessione)
    '        oLoadCombo.loadCombo(DdlTitOccupaz, dvDati)
    '        'popolo la combo richiamando la funzione LoadComboDati("NAT_OCCUPAZIONE", sIdEnte, DdlNatOccupazione);
    '        dvDati = FncAE.LoadComboDati("NAT_OCCUPAZIONE", "", WFSessione)
    '        oLoadCombo.loadCombo(DdlNatOccupaz, dvDati)
    '        'popolo la combo richiamando la funzione LoadComboDati("DEST_USO", sIdEnte, DdlDestUso);
    '        dvDati = FncAE.LoadComboDati("DEST_USO", "", WFSessione)
    '        oLoadCombo.loadCombo(DdlDestUso, dvDati)
    '        'popolo la combo richiamando la funzione LoadComboDati("TIPO_UNITA", sIdEnte, DdlTipoUnita);
    '        dvDati = FncAE.LoadComboDati("TIPO_UNITA", "", WFSessione)
    '        oLoadCombo.loadCombo(DdlTipoUnita, dvDati)
    '        'per DdlTipoUnita seleziono il valore F e la disabilito;
    '        DdlTipoUnita.SelectedValue = "F" : DdlTipoUnita.Enabled = False
    '        'popolo la combo richiamando la funzione LoadComboDati("TIPO_PARTICELLA", sIdEnte, DdlTipoParticella);
    '        dvDati = FncAE.LoadComboDati("TIPO_PARTICELLA", "", WFSessione)
    '        oLoadCombo.loadCombo(DdlTipoParticella, dvDati)
    '        'per DdlTipoParticella seleziono il valore E e la disabilito;
    '        DdlTipoParticella.SelectedValue = "E" : DdlTipoParticella.Enabled = False
    '        'popolo la combo richiamando la funzione LoadComboDati("TIPO_PARTICELLA", sIdEnte, DdlTipoParticella);
    '        dvDati = FncAE.LoadComboDati("ASSENZA_DATI_CAT", utility.Costanti.TRIBUTOTARSU, WFSessione)
    '        oLoadCombo.loadCombo(DdlAssenzaDatiCat, dvDati)

    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPF.LoadDatiAgenziaEntrate.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    Finally
    '        If Not IsNothing(WFSessione) Then
    '            WFSessione.Kill()
    '        End If
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub LoadDatiAgenziaEntrate()
        Dim oLoadCombo As New generalClass.generalFunction
        Dim FncAE As New OPENgov_AgenziaEntrate.OPENgov_AgenziaEntrate.General
        Dim dvDati As New DataView

        Try
            'popolo la combo richiamando la funzione LoadComboDati("TIT_OCCUPAZIONE", sIdEnte, DdlTitOccupazione);
            dvDati = FncAE.LoadComboDati("TIT_OCCUPAZIONE", Utility.Costanti.TRIBUTO_TARSU, ConstSession.StringConnectionOPENgov)
            oLoadCombo.loadCombo(DdlTitOccupaz, dvDati, True, Costanti.TipoDefaultCmb.STRINGA)
            'popolo la combo richiamando la funzione LoadComboDati("NAT_OCCUPAZIONE", sIdEnte, DdlNatOccupazione);
            dvDati = FncAE.LoadComboDati("NAT_OCCUPAZIONE", "", ConstSession.StringConnectionOPENgov)
            oLoadCombo.loadCombo(DdlNatOccupaz, dvDati, True, Costanti.TipoDefaultCmb.STRINGA)
            'popolo la combo richiamando la funzione LoadComboDati("DEST_USO", sIdEnte, DdlDestUso);
            dvDati = FncAE.LoadComboDati("DEST_USO", "", ConstSession.StringConnectionOPENgov)
            oLoadCombo.loadCombo(DdlDestUso, dvDati, True, Costanti.TipoDefaultCmb.STRINGA)
            'popolo la combo richiamando la funzione LoadComboDati("TIPO_UNITA", sIdEnte, DdlTipoUnita);
            dvDati = FncAE.LoadComboDati("TIPO_UNITA", "", ConstSession.StringConnectionOPENgov)
            oLoadCombo.loadCombo(DdlTipoUnita, dvDati, True, Costanti.TipoDefaultCmb.STRINGA)
            'per DdlTipoUnita seleziono il valore F e la disabilito;
            DdlTipoUnita.SelectedValue = "F" : DdlTipoUnita.Enabled = False
            'popolo la combo richiamando la funzione LoadComboDati("TIPO_PARTICELLA", sIdEnte, DdlTipoParticella);
            dvDati = FncAE.LoadComboDati("TIPO_PARTICELLA", "", ConstSession.StringConnectionOPENgov)
            oLoadCombo.loadCombo(DdlTipoParticella, dvDati, True, Costanti.TipoDefaultCmb.STRINGA)
            'per DdlTipoParticella seleziono il valore E e la disabilito;
            DdlTipoParticella.SelectedValue = "E" : DdlTipoParticella.Enabled = False
            'popolo la combo richiamando la funzione LoadComboDati("TIPO_PARTICELLA", sIdEnte, DdlTipoParticella);
            dvDati = FncAE.LoadComboDati("ASSENZA_DATI_CAT", Utility.Costanti.TRIBUTO_TARSU, ConstSession.StringConnectionOPENgov)
            oLoadCombo.loadCombo(DdlAssenzaDatiCat, dvDati, True, Costanti.TipoDefaultCmb.STRINGA)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPF.LoadDatiAgenziaEntrate.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="IdArticolo"></param>
    Private Sub LoadArticolo(ByVal IdArticolo As Integer)
        Dim oListArticoli() As ObjArticolo
        Dim MyArticolo As New ObjArticolo
        Dim MyAvviso As New ObjAvviso

        Try
            'carico l'articolo di ruolo
            MyAvviso = Session("oMyAvviso")
            oListArticoli = FncArticolo.GetArticoli(ConstSession.StringConnection, IdArticolo, MyAvviso.ID, -1, True)
            If Not oListArticoli Is Nothing Then
                For Each MyArticolo In oListArticoli
                    MyArticolo = MyArticolo
                Next
                If MyArticolo.Id <> IdArticolo Then
                    'aggiorno gli articoli sull'avviso
                    If Session("oListArticoliSgravi") Is Nothing Then
                        oListArticoli = Session("oListArticoli")
                    Else
                        oListArticoli = Session("oListArticoliSgravi")
                    End If
                    If Not oListArticoli Is Nothing Then
                        For Each MyArticolo In oListArticoli
                            If MyArticolo.Id = IdArticolo Then
                                Exit For
                            End If
                        Next
                    End If
                End If
            Else
                'aggiorno gli articoli sull'avviso
                If Session("oListArticoliSgravi") Is Nothing Then
                    oListArticoli = Session("oListArticoli")
                Else
                    oListArticoli = Session("oListArticoliSgravi")
                End If
                If Not oListArticoli Is Nothing Then
                    For Each MyArticolo In oListArticoli
                        If MyArticolo.Id = IdArticolo Then
                            Exit For
                        End If
                    Next
                    Abilita(False, 0)
                End If
            End If
            LoadDatiArticolo(MyArticolo)
            'memorizzo l'oggetto in sessione
            Session("oArticolo") = oListArticoli
            'memorizzo l'oggetto in sessione
            Session("oDatiRid") = MyArticolo.oRiduzioni
            'memorizzo l'oggetto in sessione
            Session("oDatiDet") = MyArticolo.oDetassazioni
            Abilita(False, 0)
            'per Situazione Contribuente
            If Not Request.Item("SituazioneContribuente") Is Nothing Then
                ClientScript.RegisterHiddenField("COD_CONTRIBUENTE", Request.Item("IdContribuente"))
                ClientScript.RegisterHiddenField("AnnoSC", Request.Item("AnnoSC"))
                ClientScript.RegisterHiddenField("SituazioneContribuente", Request.Item("SituazioneContribuente"))
            Else
                ClientScript.RegisterHiddenField("COD_CONTRIBUENTE", "")
                ClientScript.RegisterHiddenField("AnnoSC", "")
                ClientScript.RegisterHiddenField("SituazioneContribuente", "")
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestPF.LoadArticolo.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub LoadArticolo(ByVal IdArticolo As Integer)
    '    Dim oListArticoli() As ObjArticolo
    '    Dim MyArticolo As New ObjArticolo
    '    Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
    '    Dim MyAvviso As New ObjAvviso

    '    Try
    '        'carico l'articolo di ruolo
    '        MyAvviso = Session("oMyAvviso")
    '        oListArticoli = FncArticolo.GetArticoli(ConstSession.StringConnection, IdArticolo, MyAvviso.ID, -1, True, cmdMyCommand)
    '        If Not oListArticoli Is Nothing Then
    '            For Each MyArticolo In oListArticoli
    '                MyArticolo = MyArticolo
    '            Next
    '            If MyArticolo.Id <> IdArticolo Then
    '                'aggiorno gli articoli sull'avviso
    '                If Session("oListArticoliSgravi") Is Nothing Then
    '                    oListArticoli = Session("oListArticoli")
    '                Else
    '                    oListArticoli = Session("oListArticoliSgravi")
    '                End If
    '                If Not oListArticoli Is Nothing Then
    '                    For Each MyArticolo In oListArticoli
    '                        If MyArticolo.Id = IdArticolo Then
    '                            Exit For
    '                        End If
    '                    Next
    '                End If
    '            End If
    '        Else
    '            'aggiorno gli articoli sull'avviso
    '            If Session("oListArticoliSgravi") Is Nothing Then
    '                oListArticoli = Session("oListArticoli")
    '            Else
    '                oListArticoli = Session("oListArticoliSgravi")
    '            End If
    '            If Not oListArticoli Is Nothing Then
    '                For Each MyArticolo In oListArticoli
    '                    If MyArticolo.Id = IdArticolo Then
    '                        Exit For
    '                    End If
    '                Next
    '                Abilita(False, 0)
    '            End If
    '        End If
    '        LoadDatiArticolo(MyArticolo)
    '        'memorizzo l'oggetto in sessione
    '        Session("oArticolo") = oListArticoli
    '        'memorizzo l'oggetto in sessione
    '        Session("oDatiRid") = MyArticolo.oRiduzioni
    '        'memorizzo l'oggetto in sessione
    '        Session("oDatiDet") = MyArticolo.oDetassazioni
    '        Abilita(False, 0)
    '        'per Situazione Contribuente
    '        If Not Request.Item("SituazioneContribuente") Is Nothing Then
    '            ClientScript.RegisterHiddenField("COD_CONTRIBUENTE", Request.Item("IdContribuente"))
    '            ClientScript.RegisterHiddenField("AnnoSC", Request.Item("AnnoSC"))
    '            ClientScript.RegisterHiddenField("SituazioneContribuente", Request.Item("SituazioneContribuente"))
    '        Else
    '            ClientScript.RegisterHiddenField("COD_CONTRIBUENTE", "")
    '            ClientScript.RegisterHiddenField("AnnoSC", "")
    '            ClientScript.RegisterHiddenField("SituazioneContribuente", "")
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPF.LoadArticolo.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="TipoPartita"></param>
    Private Sub LoadNewPartita(ByVal TipoPartita As String)
        Dim oListArticoli() As ObjArticolo
        Dim MyArticolo As New ObjArticolo
        Dim MyAvviso As New ObjAvviso

        Try
            'carico l'articolo di ruolo
            MyAvviso = Session("oMyAvviso")
            MyArticolo.TipoPartita = TipoPartita
            MyArticolo.IdContribuente = MyAvviso.IdContribuente
            MyArticolo.IdAvviso = MyAvviso.ID
            MyArticolo.sAnno = MyAvviso.sAnnoRiferimento
            ReDim Preserve oListArticoli(0)
            oListArticoli(0) = MyArticolo

            If Not oListArticoli Is Nothing Then
                For Each MyArticolo In oListArticoli
                    LoadDatiArticolo(MyArticolo)
                    'memorizzo l'oggetto in sessione
                    Session("oArticolo") = oListArticoli
                    'memorizzo l'oggetto in sessione
                    Session("oDatiRid") = MyArticolo.oRiduzioni
                    'memorizzo l'oggetto in sessione
                    Session("oDatiDet") = MyArticolo.oDetassazioni
                Next
                Abilita(True, 0)
            End If
            'per Situazione Contribuente
            If Not Request.Item("SituazioneContribuente") Is Nothing Then
                ClientScript.RegisterHiddenField("COD_CONTRIBUENTE", Request.Item("IdContribuente"))
                ClientScript.RegisterHiddenField("AnnoSC", Request.Item("AnnoSC"))
                ClientScript.RegisterHiddenField("SituazioneContribuente", Request.Item("SituazioneContribuente"))
            Else
                ClientScript.RegisterHiddenField("COD_CONTRIBUENTE", "")
                ClientScript.RegisterHiddenField("AnnoSC", "")
                ClientScript.RegisterHiddenField("SituazioneContribuente", "")
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPF.LoadNowPartita.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="MyArticolo"></param>
    Private Sub LoadDatiArticolo(ByVal MyArticolo As ObjArticolo)
        Dim sSQL As String
        Dim oLoadCombo As New generalClass.generalFunction
        Dim FncRuolo As New ClsGestRuolo

        Try
            'carico l'articolo di ruolo
            TxtTipoPartita.Text = MyArticolo.TipoPartita
            TxtId.Text = MyArticolo.Id
            TxtIdArticolo.Text = MyArticolo.IdArticolo
            TxtCodContribuente.Text = MyArticolo.IdContribuente
            TxtIdDettaglioTestata.Text = MyArticolo.IdDettaglioTestata
            TxtAnno.Text = MyArticolo.sAnno
            TxtIdTariffa.Text = MyArticolo.nIdTariffa
            TxtTariffa.Text = FormatNumber(MyArticolo.impTariffa, 6)
            If MyArticolo.impRuolo <> 0 Then
                TxtImpArticolo.Text = FormatNumber(MyArticolo.impRuolo, 2)
            End If
            TxtImpNetto.Text = FormatNumber(MyArticolo.impNetto, 2)
            ChkImpForzato.Checked = MyArticolo.bIsImportoForzato
            TxtCodCartella.Text = MyArticolo.IdAvviso
            'carico le categorie
            FncRuolo.LoadTipoCalcolo(ConstSession.StringConnection, ConstSession.IdEnte, TxtAnno.Text, LblTipoCalcolo, LblTipoMQ, ChkConferimenti, ChkMaggiorazione)
            'se non selezionati nascondo
            If ChkMaggiorazione.Checked = False Then
                ChkMaggiorazione.Visible = False
            Else
                ChkMaggiorazione.Visible = True
            End If
            If ChkConferimenti.Checked = False Then
                ChkConferimenti.Visible = False
            Else
                ChkConferimenti.Visible = True
            End If
            If MyArticolo.TipoPartita = ObjArticolo.PARTEFISSA Then
                'If LblTipoCalcolo.Text = "TARES" Then
                '    MyArticolo.bIsTarsuGiornaliera = True
                '    Label1.Style.Add("display", "none") : TxtTariffa.Style.Add("display", "none")
                '    sSQL = "SELECT CODICECATEGORIA+' '+DEFINIZIONE, IDCATEGORIAATECO"
                '    sSQL += " FROM V_CATEGORIE_ATECO"
                '    sSQL += " WHERE ((fk_IdTypeAteco=0) OR (fk_IdTypeAteco=" & ConstSession.IdTypeAteco & "))"
                '    sSQL += " AND (ENTE='" & ConstSession.IdEnte & "')"
                '    sSQL += " ORDER BY RIGHT(REPLICATE('0',10)+CAST(IDCATEGORIAATECO AS VARCHAR),10)"
                'Else
                '    Label27.Style.Add("display", "none") : TxtNComponenti.Style.Add("display", "none")
                '    Label34.Style.Add("display", "none") : TxtNComponentiPV.Style.Add("display", "none")
                '    Label36.Style.Add("display", "none") : ChkForzaPV.Style.Add("display", "none")
                '    sSQL = "SELECT DESCRIZIONE, D.CODICE"
                '    sSQL += " FROM TBLCATEGORIE D"
                '    sSQL += " INNER JOIN TBLTARIFFE T ON D.CODICE=T.CODICE AND D.IDENTE=T.IDENTE"
                '    sSQL += " WHERE (D.IDENTE='" & ConstSession.IdEnte & "') AND (ANNO='" & TxtAnno.Text & "')"
                '    sSQL += " ORDER BY DESCRIZIONE"
                'End If
                Dim myAdapter As New SqlClient.SqlDataAdapter
                Dim dtMyDati As New DataTable()
                Dim dvMyDati As New DataView
                Dim cmdMyCommand As New SqlClient.SqlCommand

                Try
                    cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
                    cmdMyCommand.Connection.Open()
                    cmdMyCommand.CommandTimeout = 0
                    cmdMyCommand.CommandType = CommandType.StoredProcedure
                    cmdMyCommand.CommandText = "prc_GetCategorieEnte"
                    cmdMyCommand.Parameters.Clear()
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IdEnte", SqlDbType.VarChar)).Value = ConstSession.IdEnte
                    cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TipoTassazione", SqlDbType.VarChar)).Value = LblTipoCalcolo.Text
                    myAdapter.SelectCommand = cmdMyCommand
                    Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                    myAdapter.Fill(dtMyDati)
                    dvMyDati = dtMyDati.DefaultView
                    oLoadCombo.loadCombo(DDlCatAteco, dvMyDati, True, Costanti.TipoDefaultCmb.STRINGA)
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPF.LoadDatiArticolo.errore: ", ex)
                    Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                    Response.Redirect("../../../PaginaErrore.aspx")
                Finally
                    myAdapter.Dispose()
                    dtMyDati.Dispose()
                    cmdMyCommand.Dispose()
                    cmdMyCommand.Connection.Close()
                End Try
            ElseIf MyArticolo.TipoPartita = ObjArticolo.PARTECONFERIMENTI Then
                MyArticolo.bIsTarsuGiornaliera = True
                Label32.Text = "Conferimenti/Volume"
                Label27.Style.Add("display", "none") : TxtNComponenti.Style.Add("display", "none")
                Label34.Style.Add("display", "none") : TxtNComponentiPV.Style.Add("display", "none")
                LblForzaPV.Style.Add("display", "none") : ChkForzaPV.Style.Add("display", "none")
                sSQL = "SELECT *"
                sSQL += " FROM V_TIPOTESSERA"
                oLoadCombo.LoadComboGenerale(DDlCatAteco, sSQL, ConstSession.StringConnection, True, Costanti.TipoDefaultCmb.STRINGA)
            End If

            TxtCodVia.Text = MyArticolo.nCodVia
            TxtVia.Text = MyArticolo.sVia
            If MyArticolo.sCivico <> "0" And MyArticolo.sCivico <> "-1" Then
                TxtCivico.Text = MyArticolo.sCivico
            End If
            TxtEsponente.Text = MyArticolo.sEsponente
            TxtInterno.Text = MyArticolo.sInterno
            TxtScala.Text = MyArticolo.sScala
            If MyArticolo.tDataInizio <> Date.MinValue Then
                TxtDataInizio.Text = MyArticolo.tDataInizio
            Else
                TxtDataInizio.Text = String.Empty
            End If
            If MyArticolo.tDataFine <> Date.MinValue And MyArticolo.tDataFine.Date <> Date.MaxValue.Date Then
                TxtDataFine.Text = MyArticolo.tDataFine
            Else
                TxtDataFine.Text = String.Empty
            End If
            If MyArticolo.bIsTarsuGiornaliera Then
                ChkIsGiornaliera.Checked = True
                If MyArticolo.nBimestri <> -1 Then
                    TxtGGTarsu.Text = MyArticolo.nBimestri
                    TxtTempo.Text = MyArticolo.nBimestri
                End If
            End If
            If MyArticolo.nComponenti <> -1 Then
                TxtNComponenti.Text = MyArticolo.nComponenti
            End If
            TxtFoglio.Text = MyArticolo.sFoglio
            TxtNumero.Text = MyArticolo.sNumero
            TxtSubalterno.Text = MyArticolo.sSubalterno
            '***Agenzia Entrate***
            TxtSezione.Text = MyArticolo.sSezione
            TxtEstParticella.Text = MyArticolo.sEstensioneParticella
            DdlTipoParticella.SelectedValue = MyArticolo.sIdTipoParticella
            If MyArticolo.nIdTitoloOccupaz > 0 Then
                DdlTitOccupaz.SelectedValue = MyArticolo.nIdTitoloOccupaz
            End If
            If MyArticolo.nIdNaturaOccupaz > 0 Then
                DdlNatOccupaz.SelectedValue = MyArticolo.nIdNaturaOccupaz
            End If
            If MyArticolo.nIdDestUso > 0 Then
                DdlDestUso.SelectedValue = MyArticolo.nIdDestUso
            End If
            If MyArticolo.sIdTipoUnita <> "" Then
                DdlTipoUnita.SelectedValue = MyArticolo.sIdTipoUnita
            End If
            If MyArticolo.nIdAssenzaDatiCatastali <> 0 Then
                DdlAssenzaDatiCat.SelectedValue = MyArticolo.nIdAssenzaDatiCatastali
            End If
            '*********************
            TxtNote.Text = MyArticolo.sNote
            TxtMQTassabili.Text = MyArticolo.nMQ
            If MyArticolo.sCategoria <> "" Then
                DDlCatAteco.SelectedValue = MyArticolo.sCategoria
            End If
            TxtNComponentiPV.Text = MyArticolo.nComponentiPV
            ChkForzaPV.Checked = MyArticolo.bForzaPV
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPF.LoadDatiArticolo.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdDelete.Click
        Dim oArticoloOrg() As ObjArticolo
        Dim oListArticoli() As ObjArticolo
        Dim oListArticoliSgravi() As ObjArticolo = Nothing
        Dim x, nList As Integer

        Try
            'carico l'articolo originale
            oArticoloOrg = Session("oArticolo")
            'aggiorno gli articoli sull'avviso
            If Session("oListArticoliSgravi") Is Nothing Then
                oListArticoli = Session("oListArticoli")
            Else
                oListArticoli = Session("oListArticoliSgravi")
            End If
            nList = -1
            For x = 0 To oListArticoli.GetUpperBound(0)
                If oListArticoli(x).Id <> oArticoloOrg(0).Id Then
                    'forzo le date di inizio e fine se mancano
                    If oListArticoli(x).tDataInizio = DateTime.MinValue Then
                        oListArticoli(x).tDataInizio = "01/01" & oArticoloOrg(0).sAnno
                    End If
                    If oListArticoli(x).tDataFine = DateTime.MinValue Then
                        oListArticoli(x).tDataFine = "31/12/" & oArticoloOrg(0).sAnno
                    End If
                    If Not oListArticoli(x).oRiduzioni Is Nothing Then
                        'mi appoggio ad operatore per utilizzarlo come ordinamento
                        Dim n As Integer
                        For n = 0 To oListArticoli(x).oRiduzioni.GetUpperBound(0)
                            oListArticoli(x).sOperatore += oListArticoli(x).oRiduzioni(n).sCodice + "|"
                        Next
                    End If
                    If Not oListArticoli(x).oDetassazioni Is Nothing Then
                        'mi appoggio ad operatore per utilizzarlo come ordinamento
                        Dim n As Integer
                        For n = 0 To oListArticoli(x).oDetassazioni.GetUpperBound(0)
                            oListArticoli(x).sOperatore += oListArticoli(x).oDetassazioni(n).sCodice + "|"
                        Next
                    End If
                    nList += 1
                    ReDim Preserve oListArticoliSgravi(nList)
                    oListArticoliSgravi(nList) = oListArticoli(x)
                End If
            Next
            Dim FncElab As New ClsElabRuolo
            If FncElab.RicalcoloAvviso(ConstSession.StringConnection, oListArticoliSgravi, sScript, LblTipoCalcolo.Text, ChkMaggiorazione.Checked, ChkConferimenti.Checked, bp_TipoRuolo) = False Then
                RegisterScript(sScript, Me.GetType)
                Exit Sub
            End If
            Session("oListArticoli") = oListArticoliSgravi
            Session("oListArticoliSgravi") = oListArticoliSgravi
            'aggiorno la pagina chiamante
            'ripulisco tutti i dati di sessioni dati articolo
            ClearDatiRuolo()
            sScript = "GestAlert('a', 'info', 'CmdClearDatiArticolo', '', 'Procedura di Sgravio Inizializzata!\nPer rendere effettive le modifiche è necessario chiudere la procedura di sgravio\nattraverso l\'apposito pulsante nella videata di Gestione Avviso!');"
            RegisterScript(sScript, Me.GetType)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPF.CmdDelete_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub

    'Private Sub CmdSalvaRidEse_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdSalvaRidEse.Click
    '    Dim FncRidEse As New GestRidEse
    '    Dim oRicRidEse As New ObjRidEse
    '    Dim oListRidEse() As ObjRidEseApplicati
    '    Dim oListValRidEse() As ObjRidEseApplicati
    '    Dim x As Integer

    '    Try
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        oListRidEse = Session("oDatiRid")
    '        If Not IsNothing(oListRidEse) Then
    '            Log.Debug("GestPF::CmdSalvaRidEse_Click::n riduzioni trovate::" + oListRidEse.GetUpperBound(0).ToString())
    '            For x = 0 To oListRidEse.GetUpperBound(0)
    '                oRicRidEse.IdEnte = ConstSession.IdEnte
    '                oRicRidEse.sAnno = TxtAnno.Text
    '                oRicRidEse.sCodice = oListRidEse(x).sCodice
    '                oListValRidEse = FncRidEse.GetRidEseApplicate(WFSessione, oRicRidEse, ObjRidEse.TIPO_RIDUZIONI, "", -1)
    '                oListRidEse(x) = oListValRidEse(0)
    '            Next
    '            Session("oDatiRid") = oListRidEse
    '        Else
    '            Log.Debug("GestPF::CmdSalvaRidEse_Click::NO trovate riduzioni::")
    '        End If
    '        oListRidEse = Session("oDatiDet")
    '        If Not IsNothing(oListRidEse) Then
    '            For x = 0 To oListRidEse.GetUpperBound(0)
    '                oRicRidEse.IdEnte = ConstSession.IdEnte
    '                oRicRidEse.sAnno = TxtAnno.Text
    '                oRicRidEse.sCodice = oListRidEse(x).sCodice
    '                oListValRidEse = FncRidEse.GetRidEseApplicate(WFSessione, oRicRidEse, ObjRidEse.TIPO_ESENZIONI, "", -1)
    '                oListRidEse(x) = oListValRidEse(0)
    '            Next
    '            Session("oDatiDet") = oListRidEse
    '        End If
    '        'controllo se devo caricare la griglia delle riduzioni
    '        If Not Session("oDatiRid") Is Nothing Then
    '            GrdRiduzioni.Style.Add("display", "")
    '            GrdRiduzioni.DataSource = Session("oDatiRid")
    '            GrdRiduzioni.start_index = GrdRiduzioni.CurrentPageIndex
    '            GrdRiduzioni.DataBind()
    '            LblResultRid.Style.Add("display", "none")
    '        Else
    '            GrdRiduzioni.Style.Add("display", "none")
    '            LblResultRid.Style.Add("display", "")
    '        End If
    '        'controllo se devo caricare la griglia delle Detassazioni
    '        If Not Session("oDatiDet") Is Nothing Then
    '            GrdDetassazioni.Style.Add("display", "")
    '            GrdDetassazioni.DataSource = Session("oDatiDet")
    '            GrdDetassazioni.start_index = GrdDetassazioni.CurrentPageIndex
    '            GrdDetassazioni.DataBind()
    '            LblResultDet.Style.Add("display", "none")
    '        Else
    '            GrdDetassazioni.Style.Add("display", "none")
    '            LblResultDet.Style.Add("display", "")
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPF.CmdSalvaRidEse_Click.errore: ", Err)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    Finally
    '        'WFSessione.Kill()
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub DdlCategorie_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDlCatAteco.SelectedIndexChanged
        Dim FncTariffe As New GestTariffe
        Dim oMyTariffe As New ObjTariffa
        Dim oListTariffe() As ObjTariffa
        Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
        Dim TypeTassazione As Integer = 1

        Try
            If TxtAnno.Text <> "" Then
                'prelevo la tariffa
                oMyTariffe.IdEnte = ConstSession.IdEnte
                oMyTariffe.sAnno = TxtAnno.Text
                oMyTariffe.sCodice = DDlCatAteco.SelectedValue & "|" & TxtNComponenti.Text.ToString().PadLeft(2, "0")
                If TxtTipoPartita.Text = ObjArticolo.PARTECONFERIMENTI Then
                    TypeTassazione = 3
                End If
                oListTariffe = FncTariffe.GetTariffa(ConstSession.StringConnection, oMyTariffe, TypeTassazione)
                If Not IsNothing(oListTariffe) Then
                    TxtIdTariffa.Text = oListTariffe(0).ID
                    TxtTariffa.Text = oListTariffe(0).sValore
                End If
            Else
                sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario selezionare l\'anno prima di selezionare una categoria con relativa tariffa!');"
                RegisterScript(sScript, Me.GetType)
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestPF.SelectedIndexChanged.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub TxtAnno_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtAnno.TextChanged
        Dim oLoadCombo As New generalClass.generalFunction
        dim sSQL as string

        Try
            'carico le categorie
            If LblTipoCalcolo.Text = "TARES" Then
                sSQL = "SELECT CODICECATEGORIA+' '+DEFINIZIONE, IDCATEGORIAATECO"
                sSQL += " FROM V_CATEGORIE_ATECO"
                sSQL += " WHERE ((fk_IdTypeAteco=0) OR (fk_IdTypeAteco=" & ConstSession.IdTypeAteco & "))"
                sSQL += " AND (ENTE='" & ConstSession.IdEnte & "')"
                sSQL += " ORDER BY RIGHT(REPLICATE('0',10)+CAST(IDCATEGORIAATECO AS VARCHAR),10)"
            Else
                sSQL = "SELECT DESCRIZIONE, D.CODICE"
                sSQL += " FROM TBLCATEGORIE D"
                sSQL += " INNER JOIN TBLTARIFFE T ON D.CODICE=T.CODICE AND D.IDENTE=T.IDENTE"
                sSQL += " WHERE (D.IDENTE='" & ConstSession.IdEnte & "') AND (ANNO='" & TxtAnno.Text & "')"
                sSQL += " ORDER BY DESCRIZIONE"
            End If
            oLoadCombo.LoadComboGenerale(DDlCatAteco, sSQL, ConstSession.StringConnection, True, Costanti.TipoDefaultCmb.STRINGA)
            TxtIdTariffa.Text = "-1" : TxtTariffa.Text = ""
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPF.TextChanged.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="bTypeAbilita"></param>
    ''' <param name="IsSoloContrib"></param>
    ''' <revisionHistory>
    ''' <revision date="12/2017">
    ''' gestione tipo conferimento
    ''' </revision>
    ''' </revisionHistory>
    Private Sub Abilita(ByVal bTypeAbilita As Boolean, ByVal IsSoloContrib As Integer)
        Dim sScript As String = ""
        Dim oListCmd() As Object
        Dim x As Integer
        Try
            If IsSoloContrib <> 1 Then
                'non l'abilito mai:TxtIdTariffa, TxtImpNetto
                TxtImpArticolo.Enabled = bTypeAbilita : ChkImpForzato.Enabled = bTypeAbilita
                TxtDataInizio.Enabled = bTypeAbilita : TxtDataFine.Enabled = bTypeAbilita
                TxtMQTassabili.Enabled = bTypeAbilita
                LnkNewRid.Enabled = bTypeAbilita : LnkDelRid.Enabled = bTypeAbilita
                LnkNewDet.Enabled = bTypeAbilita : LnkDelDet.Enabled = bTypeAbilita
                TxtNote.Enabled = bTypeAbilita
                TxtRidImp.Enabled = bTypeAbilita

                If TxtTipoPartita.Text = ObjArticolo.PARTECONFERIMENTI Then
                    TxtVia.Enabled = False
                    TxtVia.ReadOnly = True
                    LnkOpenStradario.Enabled = False
                    TxtCivico.Enabled = False : TxtEsponente.Enabled = False : TxtInterno.Enabled = False : TxtScala.Enabled = False
                    TxtNComponenti.Enabled = False
                    ChkIsGiornaliera.Enabled = False : TxtGGTarsu.Enabled = False
                    TxtFoglio.Enabled = False : TxtNumero.Enabled = False : TxtSubalterno.Enabled = False
                    '***Agenzia Entrate***
                    TxtSezione.Enabled = False : TxtEstParticella.Enabled = False : DdlTitOccupaz.Enabled = False : DdlNatOccupaz.Enabled = False : DdlDestUso.Enabled = False : DdlAssenzaDatiCat.Enabled = False : DdlTipoUnita.Enabled = False
                    '*********************
                    DDlCatAteco.Enabled = False
                    ChkForzaPV.Enabled = False
                    TxtNComponentiPV.Enabled = False
                Else
                    TxtAnno.Enabled = bTypeAbilita
                    LnkOpenStradario.Enabled = bTypeAbilita

                    TxtCivico.Enabled = bTypeAbilita : TxtEsponente.Enabled = bTypeAbilita : TxtInterno.Enabled = bTypeAbilita : TxtScala.Enabled = bTypeAbilita
                    TxtNComponenti.Enabled = bTypeAbilita
                    ChkIsGiornaliera.Enabled = bTypeAbilita : TxtGGTarsu.Enabled = bTypeAbilita
                    TxtFoglio.Enabled = bTypeAbilita : TxtNumero.Enabled = bTypeAbilita : TxtSubalterno.Enabled = bTypeAbilita
                    '***Agenzia Entrate***
                    TxtSezione.Enabled = bTypeAbilita
                    TxtEstParticella.Enabled = bTypeAbilita
                    DdlTitOccupaz.Enabled = bTypeAbilita
                    DdlNatOccupaz.Enabled = bTypeAbilita
                    DdlDestUso.Enabled = bTypeAbilita
                    DdlAssenzaDatiCat.Enabled = bTypeAbilita
                    DdlTipoUnita.Enabled = bTypeAbilita
                    '*********************
                    DDlCatAteco.Enabled = bTypeAbilita
                    ChkForzaPV.Enabled = bTypeAbilita
                    TxtNComponentiPV.Enabled = bTypeAbilita
                End If
                'se passo da modifica a variazione devo disabilitare il pulsante di modifica/cancellazione e abilitare il pulsante di salvataggio
                If bTypeAbilita = True Then
                    ReDim Preserve oListCmd(1)
                    oListCmd(0) = "Modifica"
                    oListCmd(1) = "Delete"
                    For x = 0 To oListCmd.Length - 1
                        sScript += "$('#" + oListCmd(x).ToString + "').addClass('DisableBtn');"
                    Next
                    ReDim Preserve oListCmd(2)
                    oListCmd(2) = "sgravio"
                    For x = 2 To oListCmd.Length - 1
                        sScript += "$('#" + oListCmd(x).ToString + "').removeClass('DisableBtn');"
                    Next
                    RegisterScript(sScript, Me.GetType)
                Else
                    If Not Session("BloccoSgravio") Is Nothing Then
                        If CInt(Session("BloccoSgravio")) = Costanti.BloccoSgravi.NO Then 'non è stato attivato lo sgravio cmd bloccati
                            ReDim Preserve oListCmd(1)
                            oListCmd(0) = "Modifica"
                            oListCmd(1) = "Delete"
                            For x = 0 To oListCmd.Length - 1
                                sScript += "$('#" + oListCmd(x).ToString + "').addClass('DisableBtn');"
                            Next
                            ReDim Preserve oListCmd(2)
                            oListCmd(2) = "sgravio"
                            For x = 2 To oListCmd.Length - 1
                                sScript += "$('#" + oListCmd(x).ToString + "').addClass('DisableBtn');"
                            Next
                        Else
                            ReDim Preserve oListCmd(1)
                            oListCmd(0) = "Modifica"
                            oListCmd(1) = "Delete"
                            For x = 0 To oListCmd.Length - 1
                                sScript += "$('#" + oListCmd(x).ToString + "').removeClass('DisableBtn');"
                            Next
                            ReDim Preserve oListCmd(2)
                            oListCmd(2) = "sgravio"
                            For x = 2 To oListCmd.Length - 1
                                sScript += "$('#" + oListCmd(x).ToString + "').addClass('DisableBtn');"
                            Next
                        End If
                    Else
                        ReDim Preserve oListCmd(1)
                        oListCmd(0) = "Modifica"
                        oListCmd(1) = "Delete"
                        For x = 0 To oListCmd.Length - 1
                            sScript += "$('#" + oListCmd(x).ToString + "').addClass('DisableBtn');"
                        Next
                        ReDim Preserve oListCmd(2)
                        oListCmd(2) = "sgravio"
                        For x = 2 To oListCmd.Length - 1
                            sScript += "$('#" + oListCmd(x).ToString + "').addClass('DisableBtn');"
                        Next
                    End If
                    RegisterScript(sScript, Me.GetType)
                End If
            End If
            If LblTipoCalcolo.Text = "TARES" Then
                Label15.Style.Add("display", "none")
                TxtGGTarsu.Style.Add("display", "none")
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPF.Abilita.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub Abilita(ByVal bTypeAbilita As Boolean, ByVal IsSoloContrib As Integer)
    '    Dim sScript As String = ""
    '    Dim oListCmd() As Object
    '    Dim x As Integer
    '    Try
    '        If IsSoloContrib <> 1 Then
    '            '*** 201712 - gestione tipo conferimento *** 
    '            'non l'abilito mai:TxtIdTariffa, TxtImpNetto
    '            TxtImpArticolo.Enabled = bTypeAbilita : ChkImpForzato.Enabled = bTypeAbilita
    '            TxtDataInizio.Enabled = bTypeAbilita : TxtDataFine.Enabled = bTypeAbilita
    '            TxtMQTassabili.Enabled = bTypeAbilita
    '            LnkNewRid.Enabled = bTypeAbilita : LnkDelRid.Enabled = bTypeAbilita
    '            LnkNewDet.Enabled = bTypeAbilita : LnkDelDet.Enabled = bTypeAbilita
    '            TxtNote.Enabled = bTypeAbilita

    '            If TxtTipoPartita.Text = ObjArticolo.PARTECONFERIMENTI Then
    '                TxtVia.Enabled = False
    '                TxtVia.ReadOnly = True
    '                LnkOpenStradario.Enabled = False
    '                TxtCivico.Enabled = False : TxtEsponente.Enabled = False : TxtInterno.Enabled = False : TxtScala.Enabled = False
    '                TxtNComponenti.Enabled = False
    '                ChkIsGiornaliera.Enabled = False : TxtGGTarsu.Enabled = False
    '                TxtFoglio.Enabled = False : TxtNumero.Enabled = False : TxtSubalterno.Enabled = False
    '                '***Agenzia Entrate***
    '                TxtSezione.Enabled = False : TxtEstParticella.Enabled = False : DdlTitOccupaz.Enabled = False : DdlNatOccupaz.Enabled = False : DdlDestUso.Enabled = False : DdlAssenzaDatiCat.Enabled = False : DdlTipoUnita.Enabled = False
    '                '*********************
    '                DDlCatAteco.Enabled = False
    '                ChkForzaPV.Enabled = False
    '                TxtNComponentiPV.Enabled = False
    '            Else
    '                TxtAnno.Enabled = bTypeAbilita
    '                LnkOpenStradario.Enabled = bTypeAbilita

    '                TxtCivico.Enabled = bTypeAbilita : TxtEsponente.Enabled = bTypeAbilita : TxtInterno.Enabled = bTypeAbilita : TxtScala.Enabled = bTypeAbilita
    '                TxtNComponenti.Enabled = bTypeAbilita
    '                ChkIsGiornaliera.Enabled = bTypeAbilita : TxtGGTarsu.Enabled = bTypeAbilita
    '                TxtFoglio.Enabled = bTypeAbilita : TxtNumero.Enabled = bTypeAbilita : TxtSubalterno.Enabled = bTypeAbilita
    '                '***Agenzia Entrate***
    '                TxtSezione.Enabled = bTypeAbilita
    '                TxtEstParticella.Enabled = bTypeAbilita
    '                DdlTitOccupaz.Enabled = bTypeAbilita
    '                DdlNatOccupaz.Enabled = bTypeAbilita
    '                DdlDestUso.Enabled = bTypeAbilita
    '                DdlAssenzaDatiCat.Enabled = bTypeAbilita
    '                DdlTipoUnita.Enabled = bTypeAbilita
    '                '*********************
    '                DDlCatAteco.Enabled = bTypeAbilita
    '                ChkForzaPV.Enabled = bTypeAbilita
    '                TxtNComponentiPV.Enabled = bTypeAbilita
    '            End If
    '            'se passo da modifica a variazione devo disabilitare il pulsante di modifica/cancellazione e abilitare il pulsante di salvataggio
    '            If bTypeAbilita = True Then
    '                    ReDim Preserve oListCmd(1)
    '                    oListCmd(0) = "Modifica"
    '                    oListCmd(1) = "Delete"
    '                    For x = 0 To oListCmd.Length - 1
    '                    sScript += "parent.Comandi.document.getElementById('" & oListCmd(x).ToString() & "').disabled=" & True.ToString.ToLower & ";"
    '                Next
    '                ReDim Preserve oListCmd(2)
    '                oListCmd(2) = "sgravio"
    '                For x = 2 To oListCmd.Length - 1
    '                    sScript += "parent.Comandi.document.getElementById('" & oListCmd(x).ToString() & "').disabled=" & False.ToString.ToLower & ";"
    '                Next
    '                RegisterScript(sScript, Me.GetType)
    '            Else
    '                If Not Session("BloccoSgravio") Is Nothing Then
    '                    If CInt(Session("BloccoSgravio")) = Costanti.BloccoSgravi.NO Then 'non è stato attivato lo sgravio cmd bloccati
    '                        ReDim Preserve oListCmd(1)
    '                        oListCmd(0) = "Modifica"
    '                        oListCmd(1) = "Delete"
    '                        For x = 0 To oListCmd.Length - 1
    '                            sScript += "parent.Comandi.document.getElementById('" & oListCmd(x).ToString() & "').disabled=" & True.ToString.ToLower & ";"
    '                        Next
    '                        ReDim Preserve oListCmd(2)
    '                        oListCmd(2) = "sgravio"
    '                        For x = 2 To oListCmd.Length - 1
    '                            sScript += "parent.Comandi.document.getElementById('" & oListCmd(x).ToString() & "').disabled=" & True.ToString.ToLower & ";"
    '                        Next
    '                    Else
    '                        ReDim Preserve oListCmd(1)
    '                        oListCmd(0) = "Modifica"
    '                        oListCmd(1) = "Delete"
    '                        For x = 0 To oListCmd.Length - 1
    '                            sScript += "parent.Comandi.document.getElementById('" & oListCmd(x).ToString() & "').disabled=" & False.ToString.ToLower & ";"
    '                        Next
    '                        ReDim Preserve oListCmd(2)
    '                        oListCmd(2) = "sgravio"
    '                        For x = 2 To oListCmd.Length - 1
    '                            sScript += "parent.Comandi.document.getElementById('" & oListCmd(x).ToString() & "').disabled=" & True.ToString.ToLower & ";"
    '                        Next
    '                    End If
    '                Else
    '                    ReDim Preserve oListCmd(1)
    '                    oListCmd(0) = "Modifica"
    '                    oListCmd(1) = "Delete"
    '                    For x = 0 To oListCmd.Length - 1
    '                        sScript += "parent.Comandi.document.getElementById('" & oListCmd(x).ToString() & "').disabled=" & True.ToString.ToLower & ";"
    '                    Next
    '                    ReDim Preserve oListCmd(2)
    '                    oListCmd(2) = "sgravio"
    '                    For x = 2 To oListCmd.Length - 1
    '                        sScript += "parent.Comandi.document.getElementById('" & oListCmd(x).ToString() & "').disabled=" & True.ToString.ToLower & ";"
    '                    Next
    '                    End If
    '                    RegisterScript( sScript,Me.GetType)
    '                End If
    '            End If
    '            If LblTipoCalcolo.Text = "TARES" Then
    '            Label15.Style.Add("display", "none")
    '            TxtGGTarsu.Style.Add("display", "none")
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPF.Abilita.errore: ", Err)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub ClearDatiRuolo()
        'ripulisco tutti i dati di sessioni dati articolo
        Session.Remove("oArticolo")
        Session.Remove("oArticoloOrg")
        Session.Remove("oDatiRid")
        Session.Remove("oDatiDet")
        Session.Remove("IsRipulisci")
        'ripulisco le text
        TxtAnno.Text = "" : TxtTariffa.Text = ""
        TxtImpArticolo.Text = "" : TxtImpNetto.Text = "" : ChkImpForzato.Checked = False
        TxtId.Text = "-1" : TxtIdArticolo.Text = "-1" : TxtCodContribuente.Text = "-1" : TxtIdDettaglioTestata.Text = "" : TxtIdTariffa.Text = "-1"

        TxtCodVia.Text = "-1" : TxtVia.Text = "" : TxtCivico.Text = "" : TxtEsponente.Text = "" : TxtInterno.Text = "" : TxtScala.Text = ""
        TxtDataInizio.Text = "" : TxtDataFine.Text = "" : TxtNComponenti.Text = "0"
        ChkIsGiornaliera.Checked = False : TxtGGTarsu.Text = ""
        TxtFoglio.Text = "" : TxtNumero.Text = "" : TxtSubalterno.Text = ""
        TxtNote.Text = ""
        '***Agenzia Entrate***
        TxtSezione.Text = ""
        TxtEstParticella.Text = ""
        DdlTitOccupaz.SelectedIndex = -1 : DdlNatOccupaz.SelectedIndex = -1 : DdlDestUso.SelectedIndex = -1 : DdlAssenzaDatiCat.SelectedIndex = -1 : DdlTipoUnita.SelectedIndex = -1
        '*********************
        TxtMQTassabili.Text = "0"
        DDlCatAteco.SelectedIndex = -1
        ChkForzaPV.Checked = False
        TxtNComponentiPV.Text = "0"
    End Sub

#Region "Griglie"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim oListRid() As ObjRidEseApplicati
            Dim oNewListRid() As ObjRidEseApplicati = Nothing
            Dim oNewRid As New ObjRidEseApplicati
            Dim x, nList As Integer
            Dim myGrd As Ribes.OPENgov.WebControls.RibesGridView
            Dim IDRow As String = e.CommandArgument.ToString()
            If e.CommandName = "RowDelete" Then
                'carico l'oggetto
                If CType(sender, Ribes.OPENgov.WebControls.RibesGridView).UniqueID = "GrdRiduzioni" Then
                    oListRid = Session("oDatiRid")
                    myGrd = GrdRiduzioni
                Else
                    oListRid = Session("oDatiDet")
                    myGrd = GrdDetassazioni
                End If
                'carico il nuovo oggetto senza la riga selezionata
                nList = -1
                For x = 0 To oListRid.GetUpperBound(0)
                    If oListRid(x).sCodice <> IDRow Then
                        nList += 1
                        ReDim Preserve oNewListRid(nList)
                        oNewRid = New ObjRidEseApplicati
                        oNewRid = oListRid(x)
                        oNewListRid(nList) = oNewRid
                    End If
                Next
                'carico l'oggetto
                If CType(sender, Ribes.OPENgov.WebControls.RibesGridView).UniqueID = "GrdRiduzioni" Then
                    Session("oDatiRid") = oNewListRid
                Else
                    Session("oDatiDet") = oNewListRid
                End If
                If Not Session("oDatiRid") Is Nothing Then
                    myGrd.Style.Add("display", "")
                    myGrd.DataSource = Session("oDatiRid")
                    myGrd.SelectedIndex = -1
                    myGrd.DataBind()
                    LblResultRid.Style.Add("display", "none")
                Else
                    myGrd.Style.Add("display", "none")
                    LblResultRid.Style.Add("display", "")
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPF.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub LnkDelRid_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles LnkDelRid.Click
    '    Dim oListRid() As ObjRidEseApplicati
    '    Dim oNewListRid() As ObjRidEseApplicati
    '    Dim oNewRid As ObjRidEseApplicati
    '    Dim x, nList As Integer
    '    Dim sHTML As String

    '    Try
    '        'controllo che sia stato selezionato un elemento
    '        If GrdRiduzioni.SelectedIndex <> -1 Then
    '            'carico l'oggetto
    '            oListRid = Session("oDatiRid")
    '            'carico il nuovo oggetto senza la riga selezionata
    '            nList = -1
    '            For x = 0 To oListRid.GetUpperBound(0)
    '                If oListRid(x).sCodice <> GrdRiduzioni.Items(GrdRiduzioni.SelectedIndex).Cells(0).Text Then
    '                    nList += 1
    '                    ReDim Preserve oNewListRid(nList)
    '                    oNewRid = New ObjRidEseApplicati
    '                    oNewRid = oListRid(x)
    '                    oNewListRid(nList) = oNewRid
    '                End If
    '            Next
    '            'carico l'oggetto
    '            Session("oDatiRid") = oNewListRid
    '            If Not Session("oDatiRid") Is Nothing Then
    '                GrdRiduzioni.Style.Add("display", "")
    '                GrdRiduzioni.DataSource = Session("oDatiRid")
    '                GrdRiduzioni.SelectedIndex = -1
    '                GrdRiduzioni.DataBind()
    '                LblResultRid.Style.Add("display", "none")
    '            Else
    '                GrdRiduzioni.Style.Add("display", "none")
    '                LblResultRid.Style.Add("display", "")
    '            End If
    '        Else
    '            sHTML = "<script language='javascript'>alert('Selezionare una Riduzione!')</script>"
    '            RegisterScript(Me.GetType(), "msg", sHTML)
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPF.LknDelRid_Click.errore: ", Err)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Private Sub LnkDelDet_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles LnkDelDet.Click
    '    Dim oListDet() As ObjRidEseApplicati
    '    Dim oNewListDet() As ObjRidEseApplicati
    '    Dim oNewDet As ObjRidEseApplicati
    '    Dim x, nList As Integer
    '    Dim sHTML As String

    '    Try
    '        'controllo che sia stato selezionato un elemento
    '        If GrdDetassazioni.SelectedIndex <> -1 Then
    '            'carico l'oggetto
    '            oListDet = Session("oDatiDet")
    '            'carico il nuovo oggetto senza la riga selezionata
    '            nList = -1
    '            For x = 0 To oListDet.GetUpperBound(0)
    '                If oListDet(x).sCodice <> GrdDetassazioni.Items(GrdDetassazioni.SelectedIndex).Cells(0).Text Then
    '                    nList += 1
    '                    ReDim Preserve oNewListDet(nList)
    '                    oNewDet = New ObjRidEseApplicati
    '                    oNewDet = oListDet(x)
    '                    oNewListDet(nList) = oNewDet
    '                End If
    '            Next
    '            'carico l'oggetto
    '            Session("oDatiDet") = oNewListDet
    '            If Not Session("oDatiDet") Is Nothing Then
    '                GrdDetassazioni.Style.Add("display", "")
    '                GrdDetassazioni.DataSource = Session("oDatiDet")
    '                GrdDetassazioni.SelectedIndex = -1
    '                GrdDetassazioni.DataBind()
    '                LblResultDet.Style.Add("display", "none")
    '            Else
    '                GrdDetassazioni.Style.Add("display", "none")
    '                LblResultDet.Style.Add("display", "")
    '            End If
    '        Else
    '            sHTML = "<script language='javascript'>alert('Selezionare una Detassazione!')</script>"
    '            RegisterScript(Me.GetType(), "msg", sHTML)
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPF.LknDelDet_Click.errore: ", Err)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    End Try
    'End Sub
#End Region
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sMyErr"></param>
    ''' <returns></returns>
    Private Function ControlloDatiArticolo(ByRef sMyErr As String) As Boolean
        Try
            If TxtAnno.Text = "" Then
                sMyErr = "E\' necessario inserire l'Anno!"
                Return False
            End If
            'devo avere i dati dell'immobile
            If TxtVia.Text = "" Then
                sMyErr = "E\' necessario inserire la Via!"
                Return False
            Else
                If TxtTipoPartita.Text = ObjArticolo.PARTECONFERIMENTI Then
                    If TxtVia.Text.Length < 10 Then 'If TxtVia.Text.Length < 13 Then
                        'sMyErr = "La Via deve essere \'TESSERA \'+ numero Tessera +\' - COD.UT. \'+ codice utente +\' - COD.INT. \' + codice interno !"
                        sMyErr = "La Via deve essere \'TESSERA \'+ numero Tessera !"
                        Return False
                    End If
                End If
            End If
            'devo avere una data di inizio o i bimestri
            If TxtDataInizio.Text = "" And TxtGGTarsu.Text = "" Then
                sMyErr = "E\' necessario valorizzare il campo Data di Inizio o i Bimestri!"
                Return False
            End If
            'devo avere la tariffa
            If TxtTariffa.Text = "" Then
                sMyErr = "Tariffa mancante!"
                Return False
            End If
            'devo avere i mq
            If TxtMQTassabili.Text = "" Then
                sMyErr = "E\' necessario inserire i Metri!"
                Return False
            Else
                If Not IsNumeric(TxtMQTassabili.Text) Then
                    sMyErr = "Inserire solo NUMERI nel campo MQ!"
                    Return False
                End If
            End If
            'se forzo l'importo devo inserire l'importo articolo
            If ChkImpForzato.Checked = True Then
                If TxtImpArticolo.Text = "" Then
                    sMyErr = "E\' necessario inserire l'Importo Articolo!"
                    Return False
                Else
                    If Not IsNumeric(TxtImpArticolo.Text) Then
                        sMyErr = "Inserire solo NUMERI nel campo Importo Articolo!"
                        Return False
                    End If
                End If
            End If
            'se ho tarsu giornaliera devo avere i bimestri
            If ChkIsGiornaliera.Checked Then
                If TxtGGTarsu.Text = "" Then
                    sMyErr = "Inserire il numero di giorni per la TARSU giornaliera!"
                    Return False
                Else
                    If Not IsNumeric(TxtGGTarsu.Text) Then
                        sMyErr = "Inserire solo NUMERI INTERI nel campo Giorni TARSU!"
                        Return False
                    Else
                        Dim nMax As Integer
                        If LblTipoCalcolo.Text = "TARES" Then
                            nMax = 366
                        Else
                            nMax = 6
                        End If
                        'se ho i bimestri non devono essere maggiori di 6 e interi
                        If TxtGGTarsu.Text > nMax Then
                            sMyErr = "Inserire solo NUMERI inferiori a " & nMax.ToString & " nel campo Bimestri!"
                            Return False
                        End If
                    End If
                End If
            End If
            'se ho la data inizio controllo che sia valida e coerente con l'anno
            If TxtDataInizio.Text = "" Then
                sMyErr = "E\' necessario inserire la Data di Inizio!"
                Return False
            Else
                If TxtAnno.Text < Year(TxtDataInizio.Text) Then
                    sMyErr = "La Data di Inizio non e\' coerente con l\'Anno!"
                    Return False
                End If
            End If
            'se ho la data inizio e fine controllo che siano coerenti
            If TxtDataFine.Text <> "" Then
                If CDate(TxtDataFine.Text) <= CDate(TxtDataInizio.Text) Then
                    sMyErr = "La Data di Fine e\' minore/uguale alla Data di Inizio!"
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPF.ControlloDatiArticolo.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
            sMyErr = "ControlloDatiArticolo::" & ex.Message
            Return False
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdSalvaRidEse_Click(sender As Object, e As EventArgs) Handles CmdSalvaRidEse.Click
        Try
            'controllo se devo caricare la griglia delle riduzioni
            If Not Session("oDatiRid") Is Nothing Then
                    GrdRiduzioni.Style.Add("display", "")
                    GrdRiduzioni.DataSource = Session("oDatiRid")
                    GrdRiduzioni.DataBind()
                    LblResultRid.Style.Add("display", "none")
                Else
                    GrdRiduzioni.Style.Add("display", "none")
                    LblResultRid.Style.Add("display", "")
                End If
            'controllo se devo caricare la griglia delle Detassazioni
            If Not Session("oDatiDet") Is Nothing Then
                GrdDetassazioni.Style.Add("display", "")
                GrdDetassazioni.DataSource = Session("oDatiDet")
                GrdDetassazioni.DataBind()
                LblResultDet.Style.Add("display", "none")
            Else
                GrdDetassazioni.Style.Add("display", "none")
                LblResultDet.Style.Add("display", "")
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestPF.CmdSalvaRidEse_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
