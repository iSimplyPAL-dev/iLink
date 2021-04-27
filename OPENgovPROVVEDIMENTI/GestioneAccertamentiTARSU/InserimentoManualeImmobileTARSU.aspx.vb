Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
Imports RemotingInterfaceMotoreTarsu.RemotingInterfaceMotoreTarsu
Imports ComPlusInterface
Imports log4net
''' <summary>
''' Pagina per la gestione manuale di un immobile in provvedimento.
''' Contiene le funzioni della comandiera e la griglia per la visualizzazione del risultato. 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class InserimentoManualeImmobileTARSU
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(InserimentoManualeImmobileTARSU))
    Private PROGRESSIVO As String
    Private IDCONTRIBUENTE As String
    Private ANNO As String
    Private PROVENIENZA As String
    Protected UrlStradario As String = ConstSession.UrlStradario
    Protected StileStradario As String = ConstSession.StileStradario

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents LnkNewUIAnater As System.Web.UI.WebControls.ImageButton


    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    '*** 20140701 - IMU/TARES ***
    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    'Put user code to initialize the page here
    '    dim sScript as string=""
    '    dim sSQL as string
    '    Dim x As Integer

    '    Try
    '        If TxtViaRibaltata.Text <> "" Then
    '            TxtVia.Text = TxtViaRibaltata.Text
    '        End If

    '        sscript+= "parent.Comandi.location.href='CInserimentoImmobileTARSU.aspx'" & vbCrLf
    '        RegisterScript(sScript , Me.GetType())

    '        LnkNewRid.Attributes.Add("OnClick", "ShowInsertRidDet('R')")
    '        LnkNewDet.Attributes.Add("OnClick", "ShowInsertRidDet('D')")
    '        btnRibalta.Attributes.Add("OnClick", "return Controlli()")

    '        'Dim oAccertato() As OggettoArticoloRuoloAccertamento
    '        Dim oAccertato() As OggettoArticoloRuolo

    '        Dim oMyArticolo() As ObjDettaglioTestata
    '        Dim oDatiAccertamento As New ObjInsertAccertamento

    '        If Request.Item("provenienza") <> "" Then
    '            PROVENIENZA = Request.Item("provenienza")
    '            oDatiAccertamento.provenienza = PROVENIENZA
    '        End If

    '        If Request.Item("codcontribuente") <> "" Then
    '            txtCodContribuente.Text = Request.Item("codcontribuente")
    '            oDatiAccertamento.codcontribuente = txtCodContribuente.Text
    '        End If

    '        If Not IsPostBack Then
    '            LnkOpenStradario.Attributes.Add("onclick", "ApriStradario();")
    '            LnkPulisciStrada.Attributes.Add("onclick", "return ClearDatiVia();")
    '        End If

    '        If Page.IsPostBack = False Then
    '            Dim oLoadCombo As New ClsGenerale.Generale

    '            If Request.Item("anno") <> "" Then
    '                TxtAnno.Text = Request.Item("anno")
    '            End If

    '            'se l'anno è valorizzato
    '            If TxtAnno.Text <> "" Then
    '                Session("IDENTIFICATIVOAPPLICAZIONE") = "OPENGOVTA"
    '                'carico combo categorie
    '                sSQL = "SELECT DESCRIZIONE, CODICE"
    '                sSQL += " FROM TBLCATEGORIE INNER JOIN TBLTARIFFE ON TBLCATEGORIE.CODICE=TBLTARIFFE.IDCATEGORIA"
    '                sSQL += " AND TBLCATEGORIE.IDENTE=TBLTARIFFE.IDENTE"
    '                sSQL += " WHERE (TBLCATEGORIE.IDENTE='" & ConstSession.IdEnte & "') AND (ANNO='" & TxtAnno.Text & "')"
    '                sSQL += " ORDER BY DESCRIZIONE"
    '                oLoadCombo.LoadComboGenerale(DdlCategorie, sSQL)
    '                Session("IDENTIFICATIVOAPPLICAZIONE") = "OPENGOVP"
    '            End If

    '            If Request.Item("idprogressivo") <> "" Then
    '                If Not Session("oAccertatiGriglia") Is Nothing Then
    '                    oAccertato = Session("oAccertatiGriglia")
    '                    For x = 0 To oAccertato.Length - 1
    '                        If oAccertato(x).Progressivo = Request.Item("idprogressivo") Then
    '                            txtIdDettaglioTestata.Text = oAccertato(x).IdDettaglioTestata
    '                            TxtCodVia.Text = oAccertato(x).CodVia
    '                            TxtVia.Text = oAccertato(x).Via
    '                            TxtCivico.Text = oAccertato(x).Civico
    '                            TxtEsponente.Text = oAccertato(x).Esponente
    '                            TxtInterno.Text = oAccertato(x).Interno
    '                            TxtScala.Text = oAccertato(x).Scala
    '                            TxtFoglio.Text = oAccertato(x).Foglio
    '                            TxtNumero.Text = oAccertato(x).Numero
    '                            TxtSubalterno.Text = oAccertato(x).Subalterno
    '                            If oAccertato(x).DataInizio <> Date.MinValue Then
    '                                TxtDataInizio.Text = oAccertato(x).DataInizio
    '                            End If
    '                            If oAccertato(x).DataFine <> Date.MinValue Then
    '                                TxtDataFine.Text = oAccertato(x).DataFine
    '                            End If
    '                            If oAccertato(x).MQ <> 0 Then
    '                                TxtMQTassabili.Text = oAccertato(x).MQ
    '                            End If
    '                            If CStr(oAccertato(x).NumeroBimestri) <> 0 Then
    '                                TxtTempo.Text = oAccertato(x).NumeroBimestri
    '                            End If
    '                            If CStr(oAccertato(x).nComponenti) <> 0 Then
    '                                TxtNComponenti.Text = oAccertato(x).nComponenti
    '                            End If

    '                            If oAccertato(x).Categoria <> "-1" Then
    '                                DdlCategorie.SelectedValue = oAccertato(x).Categoria
    '                            End If
    '                            ChkImpForzato.Checked = oAccertato(x).ImportoForzato
    '                            If oAccertato(x).ImportoNetto <> 0.0 Then
    '                                TxtImpNetto.Text = FormatNumber(oAccertato(x).ImportoNetto, 2)
    '                            End If

    '                            If oAccertato(x).nComponenti <> 0 Then
    '                                TxtNComponenti.Text = oAccertato(x).nComponenti
    '                            End If
    '                            If oAccertato(x).ImportoRuolo <> 0.0 Then
    '                                TxtImpArticolo.Text = FormatNumber(oAccertato(x).ImportoRuolo, 2)
    '                            End If
    '                            If oAccertato(x).ImpTariffa <> 0.0 Then
    '                                TxtTariffa.Text = oAccertato(x).ImpTariffa
    '                            End If
    '                            TxtProgGriglia.Text = oAccertato(x).Progressivo
    '                            If oAccertato(x).IsTarsuGiornaliera = True Then
    '                                ChkIsGiornaliera.Checked = True
    '                            ElseIf oAccertato(x).IsTarsuGiornaliera = False Then
    '                                ChkIsGiornaliera.Checked = False
    '                            End If
    '                            TxtIdLegame.Text = oAccertato(x).IdLegame
    '                            Session("oDatiRid") = oAccertato(x).oRiduzioni
    '                            Session("oDatiDet") = oAccertato(x).oDetassazioni
    '                        End If
    '                    Next
    '                ElseIf Not Session("oDettaglioTestata") Is Nothing Then
    '                    oMyArticolo = Session("oDettaglioTestata")

    '                    TxtCodVia.Text = oMyArticolo(0).sCodVia
    '                    TxtVia.Text = oMyArticolo(0).sVia
    '                    TxtCivico.Text = oMyArticolo(0).sCivico
    '                    TxtEsponente.Text = oMyArticolo(0).sEsponente
    '                    TxtInterno.Text = oMyArticolo(0).sInterno
    '                    TxtScala.Text = oMyArticolo(0).sScala
    '                    TxtFoglio.Text = oMyArticolo(0).sFoglio
    '                    TxtNumero.Text = oMyArticolo(0).sNumero
    '                    TxtSubalterno.Text = oMyArticolo(0).sSubalterno
    '                    If oMyArticolo(0).tDataInizio <> Date.MinValue Then
    '                        TxtDataInizio.Text = oMyArticolo(0).tDataInizio
    '                    End If
    '                    If oMyArticolo(0).tDataFine <> Date.MinValue Then
    '                        TxtDataFine.Text = oMyArticolo(0).tDataFine
    '                    End If
    '                    If oMyArticolo(0).nMQ <> 0 Then
    '                        TxtMQTassabili.Text = oMyArticolo(0).nMQ
    '                    End If
    '                    Session("oDettaglioTestata") = Nothing
    '                End If
    '            Else
    '                'idprogressivo non è valorizzato
    '                If Not Session("oDettaglioTestata") Is Nothing Then
    '                    oMyArticolo = Session("oDettaglioTestata")

    '                    TxtCodVia.Text = oMyArticolo(0).sCodVia
    '                    TxtVia.Text = oMyArticolo(0).sVia
    '                    TxtCivico.Text = oMyArticolo(0).sCivico
    '                    TxtEsponente.Text = oMyArticolo(0).sEsponente
    '                    TxtInterno.Text = oMyArticolo(0).sInterno
    '                    TxtScala.Text = oMyArticolo(0).sScala
    '                    TxtFoglio.Text = oMyArticolo(0).sFoglio
    '                    TxtNumero.Text = oMyArticolo(0).sNumero
    '                    TxtSubalterno.Text = oMyArticolo(0).sSubalterno

    '                    If oMyArticolo(0).tDataInizio <> Date.MinValue Then
    '                        TxtDataInizio.Text = oMyArticolo(0).tDataInizio
    '                    End If
    '                    If oMyArticolo(0).tDataFine <> Date.MinValue Then
    '                        TxtDataFine.Text = oMyArticolo(0).tDataFine
    '                    End If
    '                    If oMyArticolo(0).nMQ <> 0 Then
    '                        TxtMQTassabili.Text = oMyArticolo(0).nMQ
    '                    End If
    '                    Session("oDettaglioTestata") = Nothing
    '                End If
    '            End If

    '            Session("oDatiAccertamento") = oDatiAccertamento
    '            ''''controllo se devo caricare la griglia delle riduzioni
    '            '''If Not Session("oDatiRid") Is Nothing Then
    '            '''    GrdRiduzioni.Style.Add("display", "")
    '            '''    GrdRiduzioni.DataSource = Session("oDatiRid")
    '            '''    GrdRiduzioni.start_index = GrdRiduzioni.CurrentPageIndex
    '            '''    GrdRiduzioni.DataBind()
    '            '''    LblResultRid.Style.Add("display", "none")
    '            '''Else
    '            '''    GrdRiduzioni.Style.Add("display", "none")
    '            '''    LblResultRid.Style.Add("display", "")
    '            '''End If

    '            ''''controllo se devo caricare la griglia delle Detassazioni
    '            '''If Not Session("oDatiDet") Is Nothing Then
    '            '''    GrdDetassazioni.Style.Add("display", "")
    '            '''    GrdDetassazioni.DataSource = Session("oDatiDet")
    '            '''    GrdDetassazioni.start_index = GrdDetassazioni.CurrentPageIndex
    '            '''    GrdDetassazioni.DataBind()
    '            '''    LblResultDet.Style.Add("display", "none")
    '            '''Else
    '            '''    GrdDetassazioni.Style.Add("display", "none")
    '            '''    LblResultDet.Style.Add("display", "")
    '            '''End If
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
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeImmobileTARSU.Page_Load.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim x As Integer
        Dim oMyArticolo() As ObjArticoloAccertamento

        Try
            If TxtViaRibaltata.Text <> "" Then
                TxtVia.Text = TxtViaRibaltata.Text
            End If
            LnkNewRid.Attributes.Add("OnClick", "ShowInsertRidDet('" & ObjCodDescr.TIPO_RIDUZIONI & "')")
            LnkNewDet.Attributes.Add("OnClick", "ShowInsertRidDet('" & ObjCodDescr.TIPO_ESENZIONI & "')")
            btnRibalta.Attributes.Add("OnClick", "return Controlli()")

            Dim oAccertato() As ObjArticoloAccertamento

            Dim oDatiAccertamento As New ObjInsertAccertamento

            If Request.Item("provenienza") <> "" Then
                PROVENIENZA = Request.Item("provenienza")
                oDatiAccertamento.provenienza = PROVENIENZA
            End If

            If Request.Item("codcontribuente") <> "" Then
                txtCodContribuente.Text = Request.Item("codcontribuente")
                oDatiAccertamento.codcontribuente = txtCodContribuente.Text
            End If

            If Not IsPostBack Then
                LnkOpenStradario.Attributes.Add("onclick", "ApriStradario();")
                LnkPulisciStrada.Attributes.Add("onclick", "return ClearDatiVia();")
            End If

            If Page.IsPostBack = False Then
                If Request.Item("anno") <> "" Then
                    TxtAnno.Text = Request.Item("anno")
                End If

                If Request.Item("idprogressivo") <> "" Then
                    If Not Session("oAccertatiGriglia") Is Nothing Then
                        oAccertato = Session("oAccertatiGriglia")
                        For x = 0 To oAccertato.Length - 1
                            If oAccertato(x).Progressivo = Request.Item("idprogressivo") Then
                                LoadDatiArticolo(oAccertato(x))
                            End If
                        Next
                    ElseIf Not Session("oDettaglioTestata") Is Nothing Then
                        oMyArticolo = Session("oDettaglioTestata")

                        LoadDatiArticolo(oMyArticolo(0))
                        Session("oDettaglioTestata") = Nothing
                    Else
                        ReDim Preserve oMyArticolo(0)
                        oMyArticolo(0).TipoPartita = ObjArticolo.PARTEFISSA
                        oMyArticolo(0).IdEnte = ConstSession.IdEnte
                        oMyArticolo(0).sAnno = TxtAnno.Text
                        oMyArticolo(0).IdContribuente = txtCodContribuente.Text
                        LoadDatiArticolo(oMyArticolo(0))
                    End If
                Else
                    'idprogressivo non è valorizzato
                    If Not Session("oDettaglioTestata") Is Nothing Then
                        oMyArticolo = Session("oDettaglioTestata")

                        LoadDatiArticolo(oMyArticolo(0))
                        Session("oDettaglioTestata") = Nothing
                    Else
                        ReDim Preserve oMyArticolo(0)
                        oMyArticolo(0) = New ObjArticoloAccertamento
                        oMyArticolo(0).TipoPartita = ObjArticolo.PARTEFISSA
                        oMyArticolo(0).IdEnte = ConstSession.IdEnte
                        oMyArticolo(0).sAnno = TxtAnno.Text
                        oMyArticolo(0).IdContribuente = txtCodContribuente.Text
                        LoadDatiArticolo(oMyArticolo(0))
                    End If
                End If
                Session("oDatiAccertamento") = oDatiAccertamento
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
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeImmobileTARSU.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="MyArticolo"></param>
    Private Sub LoadDatiArticolo(ByVal MyArticolo As ObjArticoloAccertamento)
        Dim sSQL As String
        Dim oLoadCombo As New OPENgovTIA.generalClass.generalFunction

        Try
            'carico l'articolo di ruolo
            TxtId.Text = MyArticolo.Progressivo
            TxtIdArticolo.Text = MyArticolo.IdLegame
            TxtProgGriglia.Text = MyArticolo.Progressivo
            TxtIdLegame.Text = MyArticolo.IdLegame
            TxtTipoPartita.Text = MyArticolo.TipoPartita
            txtCodContribuente.Text = MyArticolo.IdContribuente
            txtIdDettaglioTestata.Text = MyArticolo.IdDettaglioTestata
            TxtAnno.Text = MyArticolo.sAnno
            TxtIdTariffa.Text = MyArticolo.nIdTariffa
            TxtTariffa.Text = FormatNumber(MyArticolo.impTariffa, 6)
            If MyArticolo.impRuolo <> 0 Then
                TxtImpArticolo.Text = FormatNumber(MyArticolo.impRuolo, 2)
            End If
            TxtImpNetto.Text = FormatNumber(MyArticolo.impNetto, 2)
            ChkImpForzato.Checked = MyArticolo.bIsImportoForzato
            Dim FncAcc As New ClsGestioneAccertamenti
            FncAcc.LoadTipoCalcolo(ConstSession.IdEnte, TxtAnno.Text, LblTipoCalcolo, LblTipoMQ, ChkConferimenti, ChkMaggiorazione, txtInizioConf)
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
                If LblTipoCalcolo.Text = "TARES" Then
                    MyArticolo.bIsTarsuGiornaliera = True
                    Label1.Style.Add("display", "none") : TxtTariffa.Style.Add("display", "none")
                    sSQL = "SELECT CODICECATEGORIA+' '+DEFINIZIONE, IDCATEGORIAATECO"
                    sSQL += " FROM V_CATEGORIE_ATECO"
                    sSQL += " WHERE 1=1"
                    'sSQL += " AND ((fk_IdTypeAteco=0) Or (fk_IdTypeAteco=" & ConstSession.IdTypeAteco & "))"
                    sSQL += " And (ENTE='" & ConstSession.IdEnte & "')"
                    sSQL += " ORDER BY RIGHT(REPLICATE('0',10)+CAST(IDCATEGORIAATECO AS VARCHAR),10)"
                Else
                    Label27.Style.Add("display", "none") : TxtNComponenti.Style.Add("display", "none")
                    Label34.Style.Add("display", "none") : TxtNComponentiPV.Style.Add("display", "none")
                    Label36.Style.Add("display", "none") : ChkForzaPV.Style.Add("display", "none")
                    sSQL = "SELECT DESCRIZIONE, D.CODICE"
                    sSQL += " FROM TBLCATEGORIE D"
                    sSQL += " INNER JOIN TBLTARIFFE T ON D.CODICE=T.IDCATEGORIA AND D.IDENTE=T.IDENTE"
                    sSQL += " WHERE (D.IDENTE='" & ConstSession.IdEnte & "') AND (ANNO='" & TxtAnno.Text & "')"
                    sSQL += " ORDER BY DESCRIZIONE"
                End If
            ElseIf MyArticolo.TipoPartita = ObjArticolo.PARTECONFERIMENTI Then
                MyArticolo.bIsTarsuGiornaliera = True
                Label32.Text = "Conferimenti/Volume"
                Label27.Style.Add("display", "none") : TxtNComponenti.Style.Add("display", "none")
                Label34.Style.Add("display", "none") : TxtNComponentiPV.Style.Add("display", "none")
                Label36.Style.Add("display", "none") : ChkForzaPV.Style.Add("display", "none")
                sSQL = "SELECT 'DOMESTICA' AS DESCRIZIONE, 'DOM' AS CODICE"
                sSQL += " UNION "
                sSQL += "SELECT 'NON DOMESTICA' AS DESCRIZIONE, 'NONDOM' AS CODICE"
            End If
            Log.Debug("carico categorie->" & sSQL)
            oLoadCombo.LoadComboGenerale(DDlCatAteco, sSQL, ConstSession.StringConnectionTARSU, True, OPENgovTIA.Costanti.TipoDefaultCmb.STRINGA)

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
            If MyArticolo.tDataFine <> Date.MinValue Then
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
            TxtSezione.Text = MyArticolo.sSezione
            TxtEstParticella.Text = MyArticolo.sEstensioneParticella
            DdlTipoParticella.SelectedValue = MyArticolo.sIdTipoParticella
            TxtMQTassabili.Text = MyArticolo.nMQ
            If MyArticolo.sCategoria <> "" Then
                DDlCatAteco.SelectedValue = MyArticolo.sCategoria
            End If
            TxtNComponentiPV.Text = MyArticolo.nComponentiPV
            ChkForzaPV.Checked = MyArticolo.bForzaPV
            Session("oDatiRid") = MyArticolo.oRiduzioni
            Session("oDatiDet") = MyArticolo.oDetassazioni
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeImmobileTARSU.LoadDatiArticolo.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    'Private Sub DdlCategorie_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DdlCategorie.SelectedIndexChanged
    '    GestioneTariffa()
    'End Sub

    'Private Sub btnGestioneTariffa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGestioneTariffa.Click
    '    GestioneTariffa()
    'End Sub

    'Function GestioneTariffa()
    '    Dim FunctionTariffe As New ClsTariffe
    '    Dim DsDati As New DataSet
    '    Dim oMyTariffe As New ObjTariffe
    '    Dim sScript As String
    '    Dim WFSessioneTariffe As OPENUtility.CreateSessione
    '    Dim ErrWFtariffe As String

    '    Try
    '        If TxtAnno.Text <> "" Then
    '            'inizializzo la connessione
    '            WFSessioneTariffe = New OPENUtility.CreateSessione(HttpContext.Current.Session("PARAMETROENV"), ConstSession.UserName, ConfigurationManager.AppSettings("OPENGOVTA"))
    '            If Not WFSessioneTariffe.CreaSessione(ConstSession.UserName, ErrWFtariffe) Then
    '                Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '            End If

    '            'prelevo la tariffa
    '            oMyTariffe.sAnno = TxtAnno.Text
    '            oMyTariffe.sIDcategoria = DdlCategorie.SelectedValue
    '            DsDati = FunctionTariffe.GetTariffe(oMyTariffe, WFSessioneTariffe)
    '            If DsDati.Tables(0).Rows.Count <> 0 Then
    '                TxtIdTariffa.Text = CStr(DsDati.Tables(0).Rows(0)("idtariffa"))
    '                TxtTariffa.Text = CStr(DsDati.Tables(0).Rows(0)("importo"))
    '            End If
    '        Else
    '            sScript = "alert('E\' necessario selezionare l\'anno prima di selezionare una categoria con relativa tariffa!')"
    '            RegisterScript(sScript , Me.GetType())
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeImmobileTARSU.GestioneTariffa.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    Finally
    '        WFSessioneTariffe.Kill()
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub DdlCategorie_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DDlCatAteco.SelectedIndexChanged
        Dim FncTariffe As New OPENgovTIA.GestTariffe
        Dim oMyTariffe As New ObjTariffa
        Dim oListTariffe() As ObjTariffa
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
                oListTariffe = FncTariffe.GetTariffa(ConstSession.StringConnectionTARSU, oMyTariffe, TypeTassazione)
                If Not IsNothing(oListTariffe) Then
                    TxtIdTariffa.Text = oListTariffe(0).ID
                    TxtTariffa.Text = oListTariffe(0).sValore
                End If
            Else
                Dim sScript As String = "GestAlert('a', 'warning', '', '', 'E\' necessario selezionare l\'anno prima di selezionare una categoria con relativa tariffa!');"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeImmobileTARSU.DDICatAteco_SelectedIndexChanged.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    'Private Sub btnRibalta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRibalta.Click
    '    'Dim oAccertato As New MotoreTarsu.Oggetti.OggettoArticoloRuoloAccertamento
    '    'Dim oAccertato As New OggettoArticoloRuoloAccertamento
    '    'Dim oAccertatoGriglia() As OggettoArticoloRuoloAccertamento
    '    Dim oAccertato As New OggettoArticoloRuolo
    '    Dim oAccertatoGriglia() As OggettoArticoloRuolo
    '    Dim oMyArticolo() As ObjDettaglioTestata
    '    Dim objArticolo As OggettoArticoloRuolo
    '    Dim RemoRuoloTARSU As IRuoloTARSU
    '    Dim objRiduzioni() As OggettoRiduzione
    '    Dim objDetassazioni() As OggettoDetassazione
    '    Dim TypeOfRI As Type = GetType(IRuoloTARSU)
    '    Dim sScript As String
    '    Dim nBimestri As Integer

    '    Try
    '        'attivo il servizio
    '        RemoRuoloTARSU = Activator.GetObject(TypeOfRI, ConfigurationManager.AppSettings("URLServizioRuoloTARSU"))

    '        objArticolo = New OggettoArticoloRuolo

    '        objArticolo.Via = TxtVia.Text
    '        If TxtCodVia.Text <> "" Then
    '            objArticolo.CodVia = TxtCodVia.Text
    '        End If
    '        If TxtCivico.Text <> "" Then objArticolo.Civico = TxtCivico.Text
    '        objArticolo.Esponente = TxtEsponente.Text
    '        objArticolo.Interno = TxtInterno.Text
    '        objArticolo.Scala = TxtScala.Text
    '        objArticolo.Foglio = TxtFoglio.Text.Trim
    '        objArticolo.Numero = TxtNumero.Text.Trim
    '        objArticolo.Subalterno = TxtSubalterno.Text.Trim
    '        If TxtDataInizio.Text <> Date.MinValue Then
    '            objArticolo.DataInizio = TxtDataInizio.Text
    '        End If
    '        If TxtDataFine.Text <> "" Then
    '            If TxtDataFine.Text <> Date.MinValue Then
    '                objArticolo.DataFine = TxtDataFine.Text
    '            End If
    '        End If
    '        If TxtMQTassabili.Text <> 0 Then
    '            objArticolo.MQ = TxtMQTassabili.Text
    '        End If
    '        If ChkIsGiornaliera.Checked = True Then
    '            objArticolo.IsTarsuGiornaliera = True
    '        ElseIf ChkIsGiornaliera.Checked = False Then
    '            objArticolo.IsTarsuGiornaliera = False
    '        End If
    '        objArticolo.Anno = TxtAnno.Text
    '        objArticolo.Categoria = DdlCategorie.SelectedValue
    '        objArticolo.ImpTariffa = TxtTariffa.Text
    '        objArticolo.IDTariffa = TxtIdTariffa.Text
    '        If TxtNComponenti.Text.CompareTo("") <> 0 Then
    '            objArticolo.nComponenti = TxtNComponenti.Text
    '        Else
    '            objArticolo.nComponenti = 0
    '        End If
    '        objArticolo.IdContribuente = Session("oDatiAccertamento").codcontribuente
    '        If TxtImpArticolo.Text <> "" Then
    '            objArticolo.ImportoRuolo = TxtImpArticolo.Text
    '        End If
    '        objArticolo.ImportoForzato = ChkImpForzato.Checked
    '        If Not Session("oDatiDet") Is Nothing Then
    '            'objArticolo.oDetassazioni = DetProvvedimentiToDetTarsu(Session("oDatiDet"))
    '            'objDetassazioni = DetProvvedimentiToDetTarsu(Session("oDatiDet"))

    '            objArticolo.oDetassazioni = Session("oDatiDet")
    '            objDetassazioni = CType(Session("oDatiDet"), RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDetassazione())
    '            Session("oDatiDet") = Nothing
    '        Else
    '            objDetassazioni = Nothing
    '        End If
    '        If Not Session("oDatiRid") Is Nothing Then
    '            'objArticolo.oRiduzioni = RidProvvedimentiToRidTarsu(Session("oDatiRid"))
    '            'objRiduzioni = RidProvvedimentiToRidTarsu(Session("oDatiRid"))
    '            objArticolo.oRiduzioni = Session("oDatiRid")
    '            objRiduzioni = CType(Session("oDatiRid"), RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoRiduzione())
    '            Session("oDatiRid") = Nothing
    '        Else
    '            objRiduzioni = Nothing
    '        End If

    '        If TxtDataInizio.Text <> Date.MinValue Then
    '            If TxtTempo.Text = "" Then
    '                Dim tDataInizioPerCalcolo As DateTime
    '                Dim tDataFinePerCalcolo As DateTime
    '                If objArticolo.DataInizio < "01/01/" + objArticolo.Anno Then
    '                    tDataInizioPerCalcolo = "31/12/" + (CInt(objArticolo.Anno) - 1).ToString
    '                Else
    '                    tDataInizioPerCalcolo = objArticolo.DataInizio
    '                End If
    '                If objArticolo.DataFine > "31/12/" + objArticolo.Anno Then
    '                    tDataFinePerCalcolo = "31/12/" + objArticolo.Anno
    '                Else
    '                    tDataFinePerCalcolo = objArticolo.DataFine
    '                End If
    '                nBimestri = RemoRuoloTARSU.GetBimestri(tDataInizioPerCalcolo, tDataFinePerCalcolo, objArticolo.Anno)
    '                If nBimestri = -1 Then
    '                    sScript = "Bimestri"
    '                    RegisterScript(sScript, "<script language='javascript'>alert('Parametri Inseriti Errati')</script>")
    '                    Exit Sub
    '                Else
    '                    objArticolo.NumeroBimestri = nBimestri
    '                End If
    '            Else
    '                objArticolo.NumeroBimestri = TxtTempo.Text
    '            End If
    '        End If
    '        'controllo se devo calcolare l'importo
    '        If objArticolo.DaAccertamento = False Then
    '            If objArticolo.ImportoForzato = 0 Then
    '                If RemoRuoloTARSU.CalcolaImportiRuolo(objArticolo, objRiduzioni, objDetassazioni, 1) = False Then
    '                    Response.Redirect("../../PaginaErrore.aspx")
    '                    Exit Sub
    '                End If
    '            End If
    '            'calcolo gli importi netti
    '            If RemoRuoloTARSU.CalcolaImportiRuolo(objArticolo, objRiduzioni, objDetassazioni, 2) = False Then
    '                Response.Redirect("../../PaginaErrore.aspx")
    '                Exit Sub
    '            End If
    '        Else
    '            objArticolo.ImportoNetto = objArticolo.ImportoRuolo
    '        End If

    '        'popolo oggetto OggettoArticoloRuoloAccertamento
    '        oAccertato.CodVia = objArticolo.CodVia
    '        oAccertato.Via = objArticolo.Via
    '        oAccertato.Civico = objArticolo.Civico
    '        oAccertato.Esponente = objArticolo.Esponente
    '        oAccertato.Interno = objArticolo.Interno
    '        oAccertato.Scala = objArticolo.Scala
    '        oAccertato.Foglio = objArticolo.Foglio
    '        oAccertato.Numero = objArticolo.Numero
    '        oAccertato.Subalterno = objArticolo.Subalterno
    '        If objArticolo.DataInizio <> Date.MinValue Then
    '            oAccertato.DataInizio = objArticolo.DataInizio
    '        End If
    '        If objArticolo.DataFine <> Date.MinValue Then
    '            oAccertato.DataFine = objArticolo.DataFine
    '        End If
    '        oAccertato.MQ = objArticolo.MQ
    '        oAccertato.IsTarsuGiornaliera = objArticolo.IsTarsuGiornaliera
    '        oAccertato.Anno = objArticolo.Anno
    '        oAccertato.NumeroBimestri = objArticolo.NumeroBimestri
    '        oAccertato.Categoria = objArticolo.Categoria
    '        oAccertato.ImpTariffa = objArticolo.ImpTariffa
    '        oAccertato.IDTariffa = objArticolo.IDTariffa
    '        oAccertato.nComponenti = objArticolo.nComponenti
    '        oAccertato.ImportoForzato = objArticolo.ImportoForzato
    '        If Not objArticolo.oRiduzioni Is Nothing Then
    '            'oAccertato.oRiduzioni = RidTarsuToRidProvvedimenti(objArticolo.oRiduzioni)
    '            oAccertato.oRiduzioni = objArticolo.oRiduzioni
    '        End If
    '        If Not objArticolo.oDetassazioni Is Nothing Then
    '            'oAccertato.oDetassazioni = DetTarsuToDetProvvedimenti(objArticolo.oDetassazioni)
    '            oAccertato.oDetassazioni = objArticolo.oDetassazioni
    '        End If
    '        'aggiorno gli importi dell'articolo calcolati
    '        oAccertato.ImportoRuolo = objArticolo.ImportoRuolo
    '        oAccertato.ImportoRiduzione = objArticolo.ImportoRiduzione
    '        oAccertato.ImportoDetassazione = objArticolo.ImportoDetassazione
    '        oAccertato.ImportoNetto = objArticolo.ImportoNetto
    '        oAccertato.IdContribuente = objArticolo.IdContribuente
    '        'carico il progressivo della griglia
    '        If TxtProgGriglia.Text <> "-1" Then
    '            oAccertato.Progressivo = TxtProgGriglia.Text
    '        End If
    '        If TxtIdLegame.Text <> "" Then
    '            oAccertato.IdLegame = TxtIdLegame.Text
    '        End If
    '        oAccertato.Calcola_Interessi = True

    '        'assegno sanzione,descrsanzione e calcolointeressi all'immobile se è in modifica
    '        If Not Session("oAccertatiGriglia") Is Nothing Then
    '            oAccertatoGriglia = Session("oAccertatiGriglia")
    '            Dim i As Integer
    '            For i = 0 To oAccertatoGriglia.GetUpperBound(0)
    '                If oAccertato.Progressivo = oAccertatoGriglia(i).Progressivo Then
    '                    oAccertato.Sanzioni = oAccertatoGriglia(i).Sanzioni
    '                    oAccertato.DescrSanzioni = oAccertatoGriglia(i).DescrSanzioni
    '                    oAccertato.Calcola_Interessi = oAccertatoGriglia(i).Calcola_Interessi
    '                    Exit For
    '                End If
    '            Next
    '        End If

    '        If txtIdDettaglioTestata.Text = "" Then
    '            oAccertato.IdDettaglioTestata = -1
    '        Else
    '            oAccertato.IdDettaglioTestata = txtIdDettaglioTestata.Text
    '        End If
    '        If IsNothing(Session("IdTestata")) Then
    '            oAccertato.IdTestata = -1
    '        Else
    '            If Session("IdTestata").ToString() = "" Then
    '                oAccertato.IdTestata = -1
    '            Else
    '                oAccertato.IdTestata = Session("IdTestata")
    '            End If
    '        End If

    '        Session("oAccertato") = oAccertato
    '        Session("oDetassazioni") = Nothing
    '        Session("oRiduzioni") = Nothing
    '        Dim s As String
    '        If Session("oDatiAccertamento").provenienza = "Pulsante" Then
    '            s = "Pulsante"
    '            sScript = "parent.parent.opener.document.getElementById('loadGridAccertato').src='SearchAccertatiTARSU.aspx';" & vbCrLf
    '        ElseIf Session("oDatiAccertamento").provenienza = "Griglia" Then
    '            s = "Griglia"
    '            sScript = "parent.parent.opener.parent.document.getElementById('loadGridAccertato').src='SearchAccertatiTARSU.aspx';" & vbCrLf
    '        End If
    '        sscript+= "parent.window.close()"
    '        'sScript += "window.close()"
    '        RegisterScript(sScript , Me.GetType())
    '    Catch ex As Exception
    ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeImmobileTARSU.btnRibalta_Click.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnRibalta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRibalta.Click
        Dim oAccertatoInserito() As ObjArticoloAccertamento
        Dim myArticolo As New ObjArticolo
        Dim oListArticoli() As ObjArticolo
        Dim myArray As New ArrayList
        Dim myArtAcc As New ObjArticoloAccertamento
        Dim FncAcc As New ClsGestioneAccertamenti
        Dim FncRicalcolo As New OPENgovTIA.ClsElabRuolo
        Dim sScript As String = ""
        Dim bTrovato As Boolean = False

        Try
            myArticolo = LoadDatiVideata()
            If Not myArticolo Is Nothing Then
                'carico l'articolo originale
                oAccertatoInserito = CType(Session("oAccertatiGriglia"), ObjArticoloAccertamento())
                If Not Session("oAccertatiGriglia") Is Nothing Then
                    oListArticoli = FncAcc.PrepareArticoliForCalcolo(oAccertatoInserito, myArticolo, LblTipoCalcolo.Text, bTrovato)
                    If bTrovato = False Then
                        Dim nList As Integer = oListArticoli.GetUpperBound(0) + 1
                        ReDim Preserve oListArticoli(nList)
                        oListArticoli(nList) = myArticolo
                    End If
                Else
                    myArticolo.Id = 1
                    myArticolo.IdArticolo = 1
                    myArray.Add(myArticolo)
                    oListArticoli = CType(myArray.ToArray(GetType(ObjArticolo)), ObjArticolo())
                End If

                If FncRicalcolo.RicalcoloAvviso(ConstSession.StringConnectionTARSU, oListArticoli, sScript, LblTipoCalcolo.Text, ChkMaggiorazione.Checked, ChkConferimenti.Checked, "P") = False Then
                    RegisterScript(sScript, Me.GetType())
                    Exit Sub
                End If

                Dim CurrentItem As New ObjArticoloAccertamento
                myArray.Clear()
                'prelevo gli articoli determinati dal ricalcolo
                oListArticoli = Session("oListArticoli")
                For Each myArticolo In oListArticoli
                    CurrentItem = FncAcc.ArticoloTOArticoloAccertamento(myArticolo, True)
                    If Not CurrentItem Is Nothing Then
                        CurrentItem.IdEnte = ConstSession.IdEnte
                        If Not oAccertatoInserito Is Nothing Then
                            For Each myArtAcc In oAccertatoInserito
                                If CurrentItem.Id = myArtAcc.Progressivo Then
                                    CurrentItem.Sanzioni = myArtAcc.Sanzioni
                                    CurrentItem.sDescrSanzioni = myArtAcc.sDescrSanzioni
                                    CurrentItem.Calcola_Interessi = myArtAcc.Calcola_Interessi
                                End If
                            Next
                        End If
                    Else
                        Throw New Exception("errore in caricamento articolo")
                    End If
                    myArray.Add(CurrentItem)
                Next
                oAccertatoInserito = CType(myArray.ToArray(GetType(ObjArticoloAccertamento)), ObjArticoloAccertamento())

                Session("oAccertatiGriglia") = oAccertatoInserito
                Session("oDatiRid") = Nothing
                Session("oDatiDet") = Nothing

                sScript = "parent.parent.opener.location.href='SearchAccertatiTARSU.aspx';"
                sScript += "parent.window.close();"
                RegisterScript(sScript, Me.GetType())
            Else
                Response.Redirect("../../PaginaErrore.aspx")
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeImmobileTARSU.btnRibalta_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Private Function LoadDatiVideata() As ObjArticolo
        Dim myArticolo As New ObjArticolo
        Try
            'carico il progressivo della griglia
            If IsNumeric(TxtProgGriglia.Text) Then
                If CInt(TxtProgGriglia.Text) > 0 Then
                    myArticolo.Id = TxtProgGriglia.Text
                End If
            End If
            If IsNumeric(TxtIdLegame.Text) Then
                If CInt(TxtIdLegame.Text) > 0 Then
                    myArticolo.IdArticolo = TxtIdLegame.Text
                End If
            End If
            'prelevo i dati dell'articolo
            myArticolo.IdEnte = ConstSession.IdEnte
            myArticolo.IdContribuente = txtCodContribuente.Text
            myArticolo.IdDettaglioTestata = txtIdDettaglioTestata.Text
            myArticolo.nCodVia = TxtCodVia.Text
            myArticolo.sVia = TxtVia.Text
            If TxtCivico.Text <> "" Then
                myArticolo.sCivico = TxtCivico.Text
            End If
            myArticolo.sEsponente = TxtEsponente.Text
            myArticolo.sInterno = TxtInterno.Text
            myArticolo.sScala = TxtScala.Text
            myArticolo.sFoglio = TxtFoglio.Text
            myArticolo.sNumero = TxtNumero.Text
            myArticolo.sSubalterno = TxtSubalterno.Text
            If TxtGGTarsu.Text <> "" Then
                myArticolo.nBimestri = TxtGGTarsu.Text
            End If
            myArticolo.sAnno = TxtAnno.Text
            If TxtDataInizio.Text <> "" Then
                myArticolo.tDataInizio = TxtDataInizio.Text
            Else
                myArticolo.tDataInizio = "01/01" & myArticolo.sAnno
            End If
            If TxtDataFine.Text <> "" Then
                myArticolo.tDataFine = TxtDataFine.Text
            Else
                myArticolo.tDataFine = "31/12/" & myArticolo.sAnno
            End If
            myArticolo.bIsTarsuGiornaliera = ChkIsGiornaliera.Checked
            If TxtNComponenti.Text <> "" Then
                myArticolo.nComponenti = TxtNComponenti.Text
            End If
            myArticolo.sCategoria = DDlCatAteco.SelectedValue
            myArticolo.sDescrCategoria = DDlCatAteco.SelectedItem.Text
            myArticolo.nIdTariffa = TxtIdTariffa.Text
            If TxtTariffa.Text <> "" Then
                myArticolo.impTariffa = FormatNumber(TxtTariffa.Text, 6)
            End If
            If TxtNComponenti.Text <> "" And myArticolo.sDescrCategoria.IndexOf("DOM") >= 0 Then
                myArticolo.nComponenti = TxtNComponenti.Text
            End If
            If TxtNComponentiPV.Text <> "" And myArticolo.sDescrCategoria.IndexOf("DOM") >= 0 Then
                myArticolo.nComponentiPV = TxtNComponentiPV.Text
            End If
            myArticolo.bForzaPV = ChkForzaPV.Checked
            If TxtMQTassabili.Text <> "" Then
                myArticolo.nMQ = TxtMQTassabili.Text
            End If
            If TxtImpArticolo.Text <> "" Then
                myArticolo.impRuolo = FormatNumber(TxtImpArticolo.Text, 2)
            End If
            myArticolo.impNetto = FormatNumber(TxtImpNetto.Text, 2)
            myArticolo.bIsImportoForzato = ChkImpForzato.Checked
            '***Agenzia Entrate***
            myArticolo.sSezione = TxtSezione.Text
            myArticolo.sEstensioneParticella = TxtEstParticella.Text
            myArticolo.sIdTipoParticella = DdlTipoParticella.SelectedValue
            '*********************
            'memorizzo l'oggetto in sessione
            myArticolo.oRiduzioni = Session("oDatiRid")
            '/* S.T. DEBUG */
            'Dim MYRID As New ObjRidEseApplicati
            'MYRID.ID = 186
            'MYRID.sCodice = "5"
            'MYRID.sDescrizione = "60% PER CASS. DIST."
            'MYRID.Riferimento = "ARTICOLO"
            'Dim MYLIST As New Generic.List(Of ObjRidEseApplicati)
            'MYLIST.Add(MYRID)
            'myArticolo.oRiduzioni = MYLIST.ToArray
            '/*  */
            If Not myArticolo.oRiduzioni Is Nothing Then
                'mi appoggio ad operatore per utilizzarlo come ordinamento
                Dim n As Integer
                For n = 0 To myArticolo.oRiduzioni.GetUpperBound(0)
                    Log.Debug("RID::" + myArticolo.oRiduzioni(n).sCodice + "::val::" + myArticolo.oRiduzioni(n).sValore + "::tipoval::" + myArticolo.oRiduzioni(n).sTipoValore)
                    myArticolo.sOperatore += myArticolo.oRiduzioni(n).sCodice + "|"
                Next
            Else
                Log.Debug("non ho rid")
            End If
            'memorizzo l'oggetto in sessione
            myArticolo.oDetassazioni = Session("oDatiDet")
            If Not myArticolo.oDetassazioni Is Nothing Then
                'mi appoggio ad operatore per utilizzarlo come ordinamento
                Dim n As Integer
                For n = 0 To myArticolo.oDetassazioni.GetUpperBound(0)
                    myArticolo.sOperatore += myArticolo.oDetassazioni(n).sCodice + "|"
                Next
            End If
            Return myArticolo
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeImmobileTARSU.LoadDatiVideata.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Return Nothing
        End Try
    End Function
    'Private Sub TxtAnno_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtAnno.TextChanged
    '    Dim oLoadCombo As New ClsGenerale.Generale
    '    dim sSQL as string
    '    'se l'anno è valorizzato
    'Try
    '    If TxtAnno.Text <> "" Then
    '        Session("IDENTIFICATIVOAPPLICAZIONE") = "OPENGOVTA"
    '        'carico combo categorie
    '        sSQL = "SELECT DESCRIZIONE, CODICE"
    '        sSQL += " FROM TBLCATEGORIE INNER JOIN TBLTARIFFE ON TBLCATEGORIE.CODICE=TBLTARIFFE.IDCATEGORIA"
    '        sSQL += " AND TBLCATEGORIE.IDENTE=TBLTARIFFE.IDENTE"
    '        sSQL += " WHERE (TBLCATEGORIE.IDENTE='" & constsession.idente & "') AND (ANNO='" & TxtAnno.Text & "')"
    '        sSQL += " ORDER BY DESCRIZIONE"
    '        oLoadCombo.LoadComboGenerale(DdlCategorie, sSQL)
    '        Session("IDENTIFICATIVOAPPLICAZIONE") = "OPENGOVP"
    '    End If
    '  Catch ex As Exception
    '   Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeImmobileTARSU.GrdRowCommand.errore: ", ex)
    '  Response.Redirect("../../PaginaErrore.aspx")

    ' End Try
    'End Sub

    'Private Sub LnkDelDet_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles LnkDelDet.Click
    '    Try
    '        'oggetto della remoting interface tarsu
    '        Dim oListDet() As OggettoDetassazione
    '        'oggetto della remoting interface tarsu
    '        Dim oNewListDet() As OggettoDetassazione
    '        'oggetto della remoting interface tarsu
    '        Dim oNewDetraz As OggettoDetassazione
    '        Dim x, nList As Integer
    '        Dim sHTML As String

    '        'controllo che sia stato selezionato un elemento
    '        If GrdDetassazioni.SelectedIndex <> -1 Then
    '            'carico l'oggetto
    '            oListDet = Session("oDatiDet")
    '            'carico il nuovo oggetto senza la riga selezionata
    '            nList = -1
    '            For x = 0 To oListDet.GetUpperBound(0)
    '                If oListDet(x).IdDetassazione <> GrdDetassazioni.Items(GrdDetassazioni.SelectedIndex).Cells(3).Text Then
    '                    nList += 1
    '                    ReDim Preserve oNewListDet(x)
    '                    oNewDetraz = New OggettoDetassazione
    '                    oNewDetraz = oListDet(x)
    '                    oNewListDet(x) = oNewDetraz
    '                End If
    '            Next
    '            'carico l'oggetto
    '            If Not oNewListDet Is Nothing Then
    '                'Session("oDatiDet") = DetProvvedimentiToDetTarsu(oNewListDet)
    '                Session("oDatiDet") = oNewListDet
    '            Else
    '                Session("oDatiDet") = Nothing
    '            End If

    '            If Not Session("oDatiDet") Is Nothing Then
    '                GrdDetassazioni.Style.Add("display", "")
    '                GrdDetassazioni.DataSource = Session("oDatiDet")
    '                GrdDetassazioni.start_index = GrdDetassazioni.CurrentPageIndex
    '                GrdDetassazioni.SelectedIndex = -1
    '                GrdDetassazioni.DataBind()
    '                LblResultDet.Style.Add("display", "none")
    '            Else
    '                GrdDetassazioni.Style.Add("display", "none")
    '                LblResultDet.Style.Add("display", "")
    '            End If
    '        Else
    '            sHTML = "<script language='javascript'>alert('Selezionare un'Agevolazione!')</script>"
    '            RegisterScript(sScript , Me.GetType())
    '        End If
    '    Catch Err As Exception

    '   Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeImmobileTARSU.LnkDelDet_Click.errore: ", ex)
    '  Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Private Sub LnkDelRid_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles LnkDelRid.Click
    '    Try
    '        'Dim oListRid() As ComPlusInterface.ProvvedimentiTarsu.Oggetti.OggettoRiduzione
    '        'Dim oNewListRid() As ComPlusInterface.ProvvedimentiTarsu.Oggetti.OggettoRiduzione
    '        'Dim oNewRid As ComPlusInterface.ProvvedimentiTarsu.Oggetti.OggettoRiduzione
    '        Dim oListRid() As OggettoRiduzione
    '        Dim oNewListRid() As OggettoRiduzione
    '        Dim oNewRid As OggettoRiduzione
    '        Dim x, nList As Integer
    '        Dim sHTML As String


    '        'controllo che sia stato selezionato un elemento
    '        If GrdRiduzioni.SelectedIndex <> -1 Then
    '            'carico l'oggetto
    '            'oListRid = RidProvvedimentiToRidTarsu(Session("oDatiRid"))
    '            oListRid = Session("oDatiRid")
    '            'carico il nuovo oggetto senza la riga selezionata
    '            nList = -1
    '            For x = 0 To oListRid.GetUpperBound(0)
    '                If oListRid(x).IdRiduzione <> GrdRiduzioni.Items(GrdRiduzioni.SelectedIndex).Cells(3).Text Then
    '                    nList += 1
    '                    ReDim Preserve oNewListRid(nList)
    '                    'oNewRid = New ComPlusInterface.ProvvedimentiTarsu.Oggetti.OggettoRiduzione
    '                    oNewRid = New OggettoRiduzione
    '                    oNewRid = oListRid(x)
    '                    oNewListRid(nList) = oNewRid
    '                End If
    '            Next
    '            'carico l'oggetto
    '            If Not oNewListRid Is Nothing Then
    '                Session("oDatiRid") = oNewListRid
    '                'Session("oDatiRid") = RidTarsuToRidProvvedimenti(oNewListRid)
    '                'Session("oDatiRid") = oNewListRid
    '            Else
    '                Session("oDatiRid") = Nothing
    '            End If

    '            If Not Session("oDatiRid") Is Nothing Then
    '                GrdRiduzioni.Style.Add("display", "")
    '                GrdRiduzioni.DataSource = oNewListRid
    '                GrdRiduzioni.start_index = GrdRiduzioni.CurrentPageIndex
    '                GrdRiduzioni.SelectedIndex = -1
    '                GrdRiduzioni.DataBind()
    '                LblResultRid.Style.Add("display", "none")
    '            Else
    '                GrdRiduzioni.Style.Add("display", "none")
    '                LblResultRid.Style.Add("display", "")
    '            End If
    '        Else
    '            sHTML = "<script language='javascript'>alert('Selezionare una Riduzione!')</script>"
    '            RegisterScript(sScript , Me.GetType())
    '        End If
    '    Catch Err As Exception
    '    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeImmobileTARSU.LnkDelRid_Click.errore: ", Err)
    '  Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

#Region "Griglie"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Dim sScript As String = ""
        Try
            If e.CommandName = "RowDelete" Then
                If CType(sender, Ribes.OPENgov.WebControls.RibesGridView).UniqueID = "GrdRiduzioni" Then
                    Dim oListRid() As ObjRidEseApplicati
                    Dim oNewListRid() As ObjRidEseApplicati
                    Dim oNewRid As ObjRidEseApplicati
                    Dim x, nList As Integer

                    Try
                        'controllo che sia stato selezionato un elemento
                        If GrdRiduzioni.SelectedIndex <> -1 Then
                            'carico l'oggetto
                            oListRid = Session("oDatiRid")
                            'carico il nuovo oggetto senza la riga selezionata
                            nList = -1
                            For x = 0 To oListRid.GetUpperBound(0)
                                For Each myRow As GridViewRow In GrdRiduzioni.Rows
                                    If myRow.RowIndex = GrdRiduzioni.SelectedRow.RowIndex Then
                                        If oListRid(x).sCodice <> myRow.Cells(0).Text Then
                                            nList += 1
                                            ReDim Preserve oNewListRid(nList)
                                            oNewRid = New ObjRidEseApplicati
                                            oNewRid = oListRid(x)
                                            oNewListRid(nList) = oNewRid
                                        End If
                                    End If
                                Next
                            Next
                            'carico l'oggetto
                            Session("oDatiRid") = oNewListRid
                            If Not Session("oDatiRid") Is Nothing Then
                                GrdRiduzioni.Style.Add("display", "")
                                GrdRiduzioni.DataSource = Session("oDatiRid")
                                GrdRiduzioni.SelectedIndex = -1
                                GrdRiduzioni.DataBind()
                                LblResultRid.Style.Add("display", "none")
                            Else
                                GrdRiduzioni.Style.Add("display", "none")
                                LblResultRid.Style.Add("display", "")
                            End If
                        Else
                            sScript += "GestAlert('a', 'warning', '', '', 'Selezionare una Riduzione!');"
                            RegisterScript(sScript, Me.GetType())
                        End If
                    Catch Err As Exception
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeImmobileTARSU.GrdRowCommand.errore: ", Err)
                        Response.Redirect("../../PaginaErrore.aspx")
                    End Try
                Else
                    Dim oListDet() As ObjRidEseApplicati
                    Dim oNewListDet() As ObjRidEseApplicati
                    Dim oNewDet As ObjRidEseApplicati
                    Dim x, nList As Integer

                    Try
                        'controllo che sia stato selezionato un elemento
                        If GrdDetassazioni.SelectedIndex <> -1 Then
                            'carico l'oggetto
                            oListDet = Session("oDatiDet")
                            'carico il nuovo oggetto senza la riga selezionata
                            nList = -1
                            For x = 0 To oListDet.GetUpperBound(0)
                                For Each myRow As GridViewRow In GrdDetassazioni.Rows
                                    If myRow.RowIndex = GrdRiduzioni.SelectedRow.RowIndex Then
                                        If oListDet(x).sCodice <> myRow.Cells(0).Text Then
                                            nList += 1
                                            ReDim Preserve oNewListDet(nList)
                                            oNewDet = New ObjRidEseApplicati
                                            oNewDet = oListDet(x)
                                            oNewListDet(nList) = oNewDet
                                        End If
                                    End If
                                Next
                            Next
                            'carico l'oggetto
                            Session("oDatiDet") = oNewListDet
                            If Not Session("oDatiDet") Is Nothing Then
                                GrdDetassazioni.Style.Add("display", "")
                                GrdDetassazioni.DataSource = Session("oDatiDet")
                                GrdDetassazioni.SelectedIndex = -1
                                GrdDetassazioni.DataBind()
                                LblResultDet.Style.Add("display", "none")
                            Else
                                GrdDetassazioni.Style.Add("display", "none")
                                LblResultDet.Style.Add("display", "")
                            End If
                        Else
                            sScript += "GestAlert('a', 'warning', '', '', 'Selezionare una Detassazione!');"
                            RegisterScript(sScript, Me.GetType())
                        End If
                    Catch Err As Exception
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeImmobileTARSU.GrdRowCommand.errore: ", Err)
                        Response.Redirect("../../PaginaErrore.aspx")
                    End Try
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeImmobileTARSU.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
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
    '                For Each myRow As GridViewRow In GrdRiduzioni.Rows
    '                    If myRow.RowIndex = GrdRiduzioni.SelectedRow.RowIndex Then
    '                        If oListRid(x).sCodice <> myRow.Cells(0).Text Then
    '                            nList += 1
    '                            ReDim Preserve oNewListRid(nList)
    '                            oNewRid = New ObjRidEseApplicati
    '                            oNewRid = oListRid(x)
    '                            oNewListRid(nList) = oNewRid
    '                        End If
    '                    End If
    '                Next
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
    '            RegisterScript(sScript , Me.GetType())
    '        End If
    '    Catch Err As Exception
    '         Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeImmobileTARSU.LnkDelRid_Click.errore: ", Err)
    '       Response.Redirect("../../PaginaErrore.aspx")
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
    '                For Each myRow As GridViewRow In GrdDetassazioni.Rows
    '                    If myRow.RowIndex = GrdRiduzioni.SelectedRow.RowIndex Then
    '                        If oListDet(x).sCodice <> myRow.Cells(0).Text Then
    '                            nList += 1
    '                            ReDim Preserve oNewListDet(nList)
    '                            oNewDet = New ObjRidEseApplicati
    '                            oNewDet = oListDet(x)
    '                            oNewListDet(nList) = oNewDet
    '                        End If
    '                    End If
    '                Next
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
    '            RegisterScript(sScript , Me.GetType())
    '        End If
    '    Catch Err As Exception
    '         Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeImmobileTARSU.LnkDelDet_Click.errore: ", Err)
    '       Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
#End Region
    '*** ***



    'Public Function RidTarsuToRidProvvedimenti(ByVal oRidTARSU() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoRiduzione) As ComPlusInterface.ProvvedimentiTarsu.Oggetti.OggettoRiduzione() ', ByVal oRidProvvedimenti() As ComPlusInterface.ProvvedimentiTarsu.Oggetti.OggettoRiduzione) As ComPlusInterface.ProvvedimentiTarsu.Oggetti.OggettoRiduzione()

    '    Dim intRid As Integer
    '    Dim objRid As ComPlusInterface.ProvvedimentiTarsu.Oggetti.OggettoRiduzione
    '    Dim ArrayRiduzioni() As ComPlusInterface.ProvvedimentiTarsu.Oggetti.OggettoRiduzione
    'Try
    '    For intRid = 0 To oRidTARSU.Length - 1
    '        objRid = New ComPlusInterface.ProvvedimentiTarsu.Oggetti.OggettoRiduzione
    '        objRid.Descrizione = oRidTARSU(intRid).Descrizione
    '        objRid.IdRiduzione = oRidTARSU(intRid).IdRiduzione
    '        objRid.IdDettaglioTestata = oRidTARSU(intRid).IdDettaglioTestata
    '        objRid.sIdEnte = oRidTARSU(intRid).sIdEnte
    '        objRid.sTipo = oRidTARSU(intRid).sTipo
    '        objRid.sTipoValoreRid = oRidTARSU(intRid).sTipoValoreRid
    '        objRid.sValore = oRidTARSU(intRid).sValore
    '        ReDim Preserve ArrayRiduzioni(intRid)
    '        ArrayRiduzioni(intRid) = objRid
    '    Next
    '    If ArrayRiduzioni.Length > 0 Then
    '        Return ArrayRiduzioni
    '    Else
    '        Return Nothing
    '    End If
    '    Catch Err As Exception
    '         Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeImmobileTARSU.RidTarsuToRidProvvedimenti.errore: ", Err)
    '       Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Function

    'Public Function RidProvvedimentiToRidTarsu(ByVal oRidProvvedimenti() As ComPlusInterface.ProvvedimentiTarsu.Oggetti.OggettoRiduzione) As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoRiduzione() ', ByVal oRidTARSU() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoRiduzione) As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoRiduzione()
    '    Dim intRid As Integer
    '    Dim objRid As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoRiduzione
    '    Dim ArrayRiduzioni() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoRiduzione
    'Try
    '    For intRid = 0 To oRidProvvedimenti.Length - 1
    '        objRid = New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoRiduzione
    '        objRid.Descrizione = oRidProvvedimenti(intRid).Descrizione
    '        objRid.IdRiduzione = oRidProvvedimenti(intRid).IdRiduzione
    '        objRid.IdDettaglioTestata = oRidProvvedimenti(intRid).IdDettaglioTestata
    '        objRid.sIdEnte = oRidProvvedimenti(intRid).sIdEnte
    '        objRid.sTipo = oRidProvvedimenti(intRid).sTipo
    '        objRid.sTipoValoreRid = oRidProvvedimenti(intRid).sTipoValoreRid
    '        objRid.sValore = oRidProvvedimenti(intRid).sValore
    '        ReDim Preserve ArrayRiduzioni(intRid)
    '        ArrayRiduzioni(intRid) = objRid
    '    Next
    '    If ArrayRiduzioni.Length > 0 Then
    '        Return ArrayRiduzioni
    '    Else
    '        Return Nothing
    '    End If
    '    Catch Err As Exception
    '         Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeImmobileTARSU.RidProvvedimentiToRidTarsu.errore: ", Err)
    '       Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Function

    'Public Function DetTarsuToDetProvvedimenti(ByVal oDetTARSU() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDetassazione) As ComPlusInterface.ProvvedimentiTarsu.Oggetti.OggettoDetassazione()
    '    Dim intDet As Integer
    '    Dim objDet As ComPlusInterface.ProvvedimentiTarsu.Oggetti.OggettoDetassazione
    '    Dim ArrayDetassazioni() As ComPlusInterface.ProvvedimentiTarsu.Oggetti.OggettoDetassazione
    'Try
    '    For intDet = 0 To oDetTARSU.Length - 1
    '        objDet = New ComPlusInterface.ProvvedimentiTarsu.Oggetti.OggettoDetassazione
    '        objDet.Descrizione = oDetTARSU(intDet).Descrizione
    '        objDet.IdDetassazione = oDetTARSU(intDet).IdDetassazione
    '        objDet.IdDettaglioTestata = oDetTARSU(intDet).IdDettaglioTestata
    '        objDet.sIdEnte = oDetTARSU(intDet).sIdEnte
    '        objDet.sTipo = oDetTARSU(intDet).sTipo
    '        objDet.sTipoValoreDet = oDetTARSU(intDet).sTipoValoreDet
    '        objDet.sValore = oDetTARSU(intDet).sValore
    '        ReDim Preserve ArrayDetassazioni(intDet)
    '        ArrayDetassazioni(intDet) = objDet
    '    Next
    '    If ArrayDetassazioni.Length > 0 Then
    '        Return ArrayDetassazioni
    '    Else
    '        Return Nothing
    '    End If
    '    Catch Err As Exception
    '         Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeImmobileTARSU.DetTarsuToDetProvvedimenti.errore: ", Err)
    '       Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Function

    'Public Function DetProvvedimentiToDetTarsu(ByVal oDetProvvedimenti() As ComPlusInterface.ProvvedimentiTarsu.Oggetti.OggettoDetassazione) As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDetassazione() ', ByVal oRidTARSU() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoRiduzione) As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoRiduzione()
    '    Dim intDet As Integer
    '    Dim objDet As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDetassazione
    '    Dim ArrayDetassazioni() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDetassazione
    'Try
    '    For intDet = 0 To oDetProvvedimenti.Length - 1
    '        objDet = New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDetassazione
    '        objDet.Descrizione = oDetProvvedimenti(intDet).Descrizione
    '        objDet.IdDetassazione = oDetProvvedimenti(intDet).IdDetassazione
    '        objDet.IdDettaglioTestata = oDetProvvedimenti(intDet).IdDettaglioTestata
    '        objDet.sIdEnte = oDetProvvedimenti(intDet).sIdEnte
    '        objDet.sTipo = oDetProvvedimenti(intDet).sTipo
    '        objDet.sTipoValoreDet = oDetProvvedimenti(intDet).sTipoValoreDet
    '        objDet.sValore = oDetProvvedimenti(intDet).sValore
    '        ReDim Preserve ArrayDetassazioni(intDet)
    '        ArrayDetassazioni(intDet) = objDet
    '    Next
    '    If ArrayDetassazioni.Length > 0 Then
    '        Return ArrayDetassazioni
    '    Else
    '        Return Nothing
    '    End If
    '    Catch Err As Exception
    '         Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeImmobileTARSU.DetProvvedimentiToDetTarsu.errore: ", Err)
    '       Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Function
End Class
