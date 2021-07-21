Imports log4net
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
''' <summary>
''' Pagina per la visualizzazione/gestione degli immobili.
''' Contiene i dati di dettaglio e le funzioni della comandiera.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
''' <revisionHistory>
''' <revision date="12/04/2019">
''' <strong>Qualificazione AgID-analisi_rel01</strong>
''' <em>Analisi eventi</em>
''' </revision>
''' </revisionHistory>
Partial Class GestImmobili
    Inherits BaseEnte
    Private Shared Log As ILog = LogManager.GetLogger(GetType(GestImmobili))
    Public UrlStradario As String = ConstSession.UrlStradario
    Public UrlPopTerritorio As String = ConstSession.UrlTerritorio
    'Public ApplicazioneTerritorio As String = ConfigurationManager.AppSettings("OPENGOVT")
    Public SituazioneContribuente, AnnoSC As String
    Protected FncGrd As New Formatta.FunctionGrd
    'Private WFErrore As String
    'Private WFSessione As OPENUtility.CreateSessione

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Label42 As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
        '*** pagina comandi ***
        GestComandi()
        '*** ***
    End Sub

#End Region

    '*** 20140805 - Gestione Categorie Vani ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            Dim oMyDettaglioTestata As New ObjDettaglioTestata
            Dim oListDettaglioTestata() As ObjDettaglioTestata
            Dim x As Integer

            If ConstSession.IsFromVariabile(Request.Item("IsFromVariabile")) = "1" Then
            End If
            If Not Request.Item("Sportello") Is Nothing Then
                If Request.Item("Sportello") = "1" And Session("oTestata") Is Nothing Then
                    Dim oListTestata() As ObjTestata = New ClsDichiarazione().GetDichiarazione(ConstSession.StringConnection, -1, -1, Request.Item("IdUniqueUI"), True, "")
                    If oListTestata Is Nothing Then
                        Response.Redirect("../../PaginaErrore.aspx")
                        Exit Sub
                    End If
                    Session("oTestata") = oListTestata(0)
                    hdIdContribuente.Value = oListTestata(0).IdContribuente
                    Session("oDettaglioTestata") = oListTestata(0).oImmobili
                    Session("oListImmobili") = oListTestata(0).oImmobili
                ElseIf Not Session("oTestata") Is Nothing Then
                    Dim oListTestata As ObjTestata = Session("oTestata")
                    hdIdContribuente.Value = oListTestata.IdContribuente
                End If
            End If
            Log.Debug("GestImmobili::DB::" & ConstSession.StringConnection)
            If TxtViaRibaltata.Text <> "" Then
                TxtVia.Text = TxtViaRibaltata.Text
            End If
            'salvo la data dichiarazione testata
            Dim oMyTestata As New ObjTestata
            oMyTestata = Session("oTestata")
            If Not oMyTestata Is Nothing Then
                txtDataDichiarazione.Text = oMyTestata.tDataDichiarazione.ToShortDateString()
            End If

            LnkNewRid.Attributes.Add("OnClick", "ShowInsertRidEse('" & ObjCodDescr.TIPO_RIDUZIONI & "')")
            LnkNewDet.Attributes.Add("OnClick", "ShowInsertRidEse('" & ObjCodDescr.TIPO_ESENZIONI & "')")
            LnkDelDet.Attributes.Add("OnClick", "DeleteRidEse('" & ObjCodDescr.TIPO_ESENZIONI & "')")
            LnkOpenStradario.Attributes.Add("onclick", "return ApriStradario('RibaltaStrada', '" & ConstSession.IdEnte & "')")
            LnkNewVaniAnater.Attributes.Add("OnClick", "ShowInsertVaniAnater('" & Utility.Costanti.AZIONE_NEW & "')")
            LnkNewVaniAnater.ToolTip = "Nuovo Vano da " & ConstSession.NameSistemaTerritorio

            If Page.IsPostBack = False Then
                If Not Request.Item("IdTessera") Is Nothing Then
                    TxtIdTessera.Text = Request.Item("IdTessera")
                Else
                    TxtIdTessera.Text = "-1"
                End If
                'visualizzo i dati della tessera associata
                '*** 201504 - Nuova Gestione anagrafica con form unico ***
                Dim FncLoad As New ClsDichiarazione
                If Not Request.Item("IdContribuente") Is Nothing Then
                    hdIdContribuente.Value = CInt(Request.Item("IdContribuente"))
                End If
                If ConstSession.HasPlainAnag Then
                    ifrmAnag.Attributes.Add("src", "../../Generali/asp/VisualAnag.aspx?IdContribuente=" & hdIdContribuente.Value & "&Azione=" & Utility.Costanti.AZIONE_LETTURA)
                Else
                    FncLoad.LoadPannelAnagrafica(hdIdContribuente.Value, ConstSession.CodTributo, ConstSession.StringConnectionAnagrafica, lblNominativo, lblCFPIVA, lblDatiNascita, lblResidenza)
                End If
                '*** ***
                '*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI & DIVERSA GESTIONE QUOTA VARIABILE***
                If Request.Item("Provenienza") = Costanti.FormProvenienza.ICIDich Then
                    Try
                        Dim FncTessere As New GestTessera
                        Dim myTessera() As ObjTessera
                        myTessera = FncTessere.GetTessera(ConstSession.StringConnection, ConstSession.IdEnte, -1, -1, "", Request.Item("IdTessera"), "", -1, True, False)
                        Session("oTessera") = myTessera(0)
                    Catch ex As Exception
                        Log.Debug("GestImmobili::errore in caricamento tessera::", ex)
                    End Try
                End If
                '*** ***
                FncLoad.LoadPannelTessera(Session("oTessera"), LblCodUtente, LblCodInterno, LblNTessera, LblDataRilascio, LblDataCessazione)
                If Not IsNothing(Session("oTessera")) Then
                    Dim oMyTessera As New ObjTessera
                    oMyTessera = Session("oTessera")
                    Session("oListUITessera") = oMyTessera.oImmobili
                End If

                LoadPageCombo()

                hdProvenienza.Value = Request.Item("sProvenienza")

                If Not Session("oUITemp") Is Nothing Then
                    If LoadDatiIntoForm(CType(Session("oUITemp"), ObjDettaglioTestata)) = False Then
                        Throw New Exception("Errore in caricamento dati nel form")
                    End If
                    If Request.Item("AzioneProv") = Utility.Costanti.AZIONE_UPDATE Or Request.Item("AzioneProv") = Utility.Costanti.AZIONE_LETTURA Then
                        Abilita(False, Request.Item("sProvenienza"))
                    Else
                        Abilita(True, Request.Item("sProvenienza"))
                    End If
                Else
                    'controllo se sono in visualizzazione di un immobile
                    If Request.Item("AzioneProv") = Utility.Costanti.AZIONE_UPDATE Or Request.Item("AzioneProv") = Utility.Costanti.AZIONE_LETTURA Then
                        'controllo se arrivo da Agenzia Entrate Dati Mancanti 
                        If Request.Item("Provenienza") = Costanti.FormProvenienza.DatiAE Then
                            'Dim FncDettaglio As New ObjDettaglioTestata
                            'oMyDettaglioTestata = FncDettaglio.GetSingleDettaglioTestata(request.item("IdUniqueUI"))
                        ElseIf Request.Item("Provenienza") = Costanti.FormProvenienza.ICICheckRif Or Request.Item("Provenienza") = Costanti.FormProvenienza.ICIDich Or Request.Item("Provenienza") = Costanti.FormProvenienza.InterGen Then
                            If Not IsNothing(Request.Item("IdUniqueUI")) Then
                                If Request.Item("IdUniqueUI") > 0 Then
                                    Dim FncDettaglio As New GestDettaglioTestata
                                    oListDettaglioTestata = FncDettaglio.GetDettaglioTestata(ConstSession.StringConnection, Request.Item("IdUniqueUI"), -1, -1, ConstSession.IdEnte, False)
                                    oMyDettaglioTestata = oListDettaglioTestata(0)
                                    Session("oDettaglioTestataAnater") = oMyDettaglioTestata
                                Else
                                    Exit Sub
                                End If
                            Else
                                Exit Sub
                            End If
                        Else
                            'carico l'array originario
                            '*** X UNIONE CON BANCADATI CMGC ***
                            If ConstSession.IsFromVariabile = "1" Then
                                oListDettaglioTestata = Session("oListUITessera")
                            Else
                                oListDettaglioTestata = Session("oDettaglioTestata")
                                Session("oListUITessera") = Session("oDettaglioTestata")
                            End If
                            '*** ***
                            If IsNothing(oListDettaglioTestata) Then
                                Exit Sub
                            End If
                            'carico il singolo immobile selezionato
                            If Request.Item("IdUniqueUI") <> -1 Then
                                For x = 0 To oListDettaglioTestata.GetUpperBound(0)
                                    If oListDettaglioTestata(x).Id = Request.Item("IdUniqueUI") Then
                                        oMyDettaglioTestata = oListDettaglioTestata(x)
                                        Exit For
                                    End If
                                Next
                            Else
                                oMyDettaglioTestata = oListDettaglioTestata(Request.Item("IdList"))
                            End If
                            Session("oDettaglioTestataAnater") = oMyDettaglioTestata
                        End If
                        If LoadDatiIntoForm(oMyDettaglioTestata) = False Then
                            Throw New Exception("Errore in caricamento dati nel form")
                        End If
                        Abilita(False, Request.Item("Provenienza"))
                    Else
                        Abilita(True, Request.Item("Provenienza"))
                    End If
                End If
                'controllo se devo caricare la griglia dei dati vani
                If Not Session("oDatiVani") Is Nothing Then
                    GrdVani.Style.Add("display", "")
                    GrdVani.SelectedIndex = -1
                    GrdVani.DataSource = Session("oDatiVani")
                    GrdVani.DataBind()
                    LblResultVani.Style.Add("display", "none")
                    TxtCountVani.Text = 1
                Else
                    GrdVani.Style.Add("display", "none")
                    LblResultVani.Style.Add("display", "")
                    TxtCountVani.Text = 0
                End If
                'controllo se devo caricare la griglia delle riduzioni
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
                'controllo se devo caricare la griglia delle Detassazioni
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
                If ConstSession.IsFromTARES <> "1" Then
                    divUIVSCat.Style.Add("display", "none")
                    'Label13.Style.Add("display", "none")
                    'TxtNComponenti.Style.Add("display", "none")
                    'Label27.Style.Add("display", "none")
                    'TxtMQCatasto.Style.Add("display", "none")
                    'Label32.Style.Add("display", "none")
                    'TxtMQTassabili.Style.Add("display", "none")
                    'Label35.Style.Add("display", "none")
                    'DDlCatTARES.Style.Add("display", "none")
                    'Label34.Style.Add("display", "none")
                    'TxtNComponentiPV.Style.Add("display", "none")
                    'Label36.Style.Add("display", "none")
                    'ChkForzaPV.Style.Add("display", "none")
                End If
                '*** 20130325 - gestione mq tassabili per TARES ***
                TxtMQTassabili.Enabled = False '*** non è editabile
                '*** ***
                If TxtFoglio.Text <> "" Then
                    Dim FncDatiICI As New ClsDichiarazione
                    Dim tInizio, tFine As DateTime
                    tInizio = DateTime.MaxValue : tFine = DateTime.MaxValue
                    If TxtDataInizio.Text <> "" Then
                        tInizio = TxtDataInizio.Text
                    End If
                    If TxtDataFine.Text <> "" Then
                        tFine = TxtDataFine.Text
                    End If
                    FncDatiICI.LoadDatiICI(ConstSession.StringConnection, ConstSession.IdEnte, TxtFoglio.Text, TxtNumero.Text, TxtSubalterno.Text, tInizio, tFine, GrdICI, ibNewICI)
                End If
            Else
                '*** 20130325 - gestione mq tassabili per TARES ***
                If Not Session("oDettaglioTestataAnater") Is Nothing Then
                    oMyDettaglioTestata = Session("oDettaglioTestataAnater")
                    TxtMQTassabili.Text = oMyDettaglioTestata.nMQTassabili
                End If
                '*** ***
                ''controllo se devo caricare la griglia dei dati vani
                'If Not Session("oDatiVani") Is Nothing Then
                '    GrdVani.Style.Add("display", "")
                '    GrdVani.SelectedIndex = -1
                '    GrdVani.DataSource = Session("oDatiVani")
                '    GrdVani.DataBind()
                '    LblResultVani.Style.Add("display", "none")
                '    TxtCountVani.Text = 1
                'Else
                '    GrdVani.Style.Add("display", "none")
                '    LblResultVani.Style.Add("display", "")
                '    TxtCountVani.Text = 0
                'End If
                'controllo se devo caricare la griglia delle riduzioni
                'If Not Session("oDatiRid") Is Nothing Then
                '    GrdRiduzioni.Style.Add("display", "")
                '    GrdRiduzioni.DataSource = Session("oDatiRid")
                '    'GrdRiduzioni.SelectedIndex = -1
                '    GrdRiduzioni.DataBind()
                '    LblResultRid.Style.Add("display", "none")
                'Else
                '    GrdRiduzioni.Style.Add("display", "none")
                '    LblResultRid.Style.Add("display", "")
                'End If
                ''controllo se devo caricare la griglia delle Detassazioni
                'If Not Session("oDatiDet") Is Nothing Then
                '    GrdDetassazioni.Style.Add("display", "")
                '    GrdDetassazioni.DataSource = Session("oDatiDet")
                '    'GrdDetassazioni.SelectedIndex = -1
                '    GrdDetassazioni.DataBind()
                '    LblResultDet.Style.Add("display", "none")
                'Else
                '    GrdDetassazioni.Style.Add("display", "none")
                '    LblResultDet.Style.Add("display", "")
                'End If
            End If
            LoadUIVSCat()
            '*** X UNIONE CON BANCADATI CMGC ***
            If ConstSession.IsFromVariabile = "1" Then
                DivTessera.Style.Add("display", "")
            Else
                DivTessera.Style.Add("display", "none")
            End If
            '*** ***
            '*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI & DIVERSA GESTIONE QUOTA VARIABILE***
            Dim sScript As String = "" '"<script language='javascript'>"
            If ConstSession.HasDummyDich Then
                LblForzaPV.Style.Add("display", "none")
                ChkForzaPV.Style.Add("display", "none")
            Else
                sScript += "document.getElementById('divDatiICI').style.display='none';"
            End If
            If ConstSession.HasPlainAnag Then
                sScript += "document.getElementById('TRSpecAnag').style.display='none';"
            Else
                sScript += "document.getElementById('TRPlainAnag').style.display='none';"
            End If
            'Str += "</script>"
            'RegisterScript( sScript,Me.GetType)
            RegisterScript(sScript, Me.GetType)

            If Request.Item("Provenienza") = Costanti.FormProvenienza.ICIDich Then
                sScript = "parent.Comandi.document.getElementById('GIS').style.display='none';"
                sScript += "parent.Comandi.document.getElementById('Territorio').style.display='none';"
                sScript += "parent.Comandi.document.getElementById('Duplica').style.display='none';"
                sScript += "parent.Comandi.document.getElementById('Modifica').style.display='none';"
                sScript += "parent.Comandi.document.getElementById('Delete').style.display='none';"
                sScript += "parent.Comandi.document.getElementById('Salva').style.display='none';"
                'RegisterScript(Me.GetType(), "hidecmd", "<script language='javascript'>" + Str + "</script>")
                RegisterScript(sScript, Me.GetType)
            End If
            '*** ***
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestImmobili.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

#Region "Griglie"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Dim nRow As Integer = -1
        Try
            Dim IDRow As String = e.CommandArgument.ToString()
            If e.CommandName = "RowOpen" Then
                If CType(sender, Ribes.OPENgov.WebControls.RibesGridView).UniqueID = "GrdVani" Then
                    Dim sScript As String
                    Dim oMyUITemp As New ObjDettaglioTestata
                    For Each myRow As GridViewRow In GrdVani.Rows
                        nRow += 1
                        If CType(myRow.FindControl("hfIDOGGETTO"), HiddenField).Value = IDRow Then
                            Try
                                '*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI & DIVERSA GESTIONE QUOTA VARIABILE***
                                If Request.Item("Provenienza") = Costanti.FormProvenienza.ICIDich Then
                                    sScript = "GestAlert('a', 'warning', '', '', 'Impossibile accedere al dettaglio dalla gestione incrociata');"
                                    RegisterScript(sScript, Me.GetType())
                                Else
                                    'apro il popup passandogli l'indice dell'array da visualizzare
                                    If LoadDatiFromForm(0, 0, oMyUITemp) = False Then
                                        sScript = "GestAlert('a', 'danger', '', '', 'Errore in carico dati da form.')"
                                    End If
                                    Session("oUITemp") = oMyUITemp
                                    sScript = "ShowInsertVani('" & hdIdContribuente.Value & "','" & hdIdTestata.Value & "','" & Request.Item("IdTessera") & "','" & Request.Item("IdUniqueUI") & "','" & IDRow & "','" & TxtIdUI.Text & "','" & Utility.Costanti.AZIONE_UPDATE & "','" & Request.Item("Provenienza") & "','" & nRow & "','" & Request.Item("IdList") & "','" & Request.Item("ParamRitornoICI") & "')"
                                    RegisterScript(sScript, Me.GetType())
                                End If
                                '*** ***
                            Catch Err As Exception
                                Log.Debug("Si è verificato un errore in GestImmobili::GrdVani_SelectedIndexChanged::" & Err.Message)
                            End Try
                        End If
                    Next
                Else
                    Dim sScript As String
                    Try
                        For Each myRow As GridViewRow In GrdICI.Rows
                            If IDRow = CType(myRow.FindControl("hfIdOggetto"), HiddenField).Value Then
                                'se ho idoggetto chiedo se vogliono inserirlo altrimenti apro gestione ici
                                sScript = "parent.Comandi.location.href = '../.." & ConstSession.Path_ICI & "/CImmobileDettaglioMod.aspx?Operation=DETTAGLIO';"
                                sScript += "parent.Visualizza.location.href = '../.." & ConstSession.Path_ICI & "/ImmobileDettaglio.aspx"
                                sScript += "?IDTestata=" + CType(myRow.FindControl("hfIdTestata"), HiddenField).Value + "&IDImmobile=" + IDRow + "&IdAttoCompraVendita=0&IdDOCFA=0&TYPEOPERATION=DETTAGLIO&ProvenienzaTARSU=TARSU"
                                sScript += "&sProvenienza=" + Request.Item("sProvenienza")
                                sScript += "&IdContribuente=" + hdIdContribuente.Value + "&IdTessera=" + Request.Item("IdTessera") + "&IdUniqueUI=" + Request.Item("IdUniqueUI")
                                sScript += "&IsFromVariabile=" + Request.Item("IsFromVariabile")
                                sScript += "&AzioneProv=" + Request.Item("AzioneProv") + "&Provenienza=" + Request.Item("Provenienza") + "&IdList=" + Request.Item("IdList") + "'"
                                RegisterScript(sScript, Me.GetType())
                            End If
                        Next
                    Catch Err As Exception
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestImmobili.GrdRowCommand.errore: ", Err)
                        Response.Redirect("../../PaginaErrore.aspx")
                    End Try
                End If
            ElseIf e.CommandName = "RowDelete" Then
                If CType(sender, Ribes.OPENgov.WebControls.RibesGridView).UniqueID = "GrdRiduzioni" Then
                    Dim oListRid() As ObjRidEseApplicati
                    Dim oNewListRid() As ObjRidEseApplicati = Nothing
                    Dim oNewRid As ObjRidEseApplicati
                    Dim x, nList As Integer
                    Dim oListDettaglioTestata() As ObjDettaglioTestata

                    Try
                        'carico l'oggetto
                        oListRid = Session("oDatiRid")
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
                        'aggiorno il database
                        'carico l'array originario
                        oListDettaglioTestata = Session("oListUITessera")
                        'carico l'oggetto
                        Session("oDatiRid") = oNewListRid
                        If Not Session("oDatiRid") Is Nothing Then
                            GrdRiduzioni.Style.Add("display", "")
                            GrdRiduzioni.DataSource = Session("oDatiRid")
                            GrdRiduzioni.DataBind()
                            LblResultRid.Style.Add("display", "none")
                        Else
                            GrdRiduzioni.Style.Add("display", "none")
                            LblResultRid.Style.Add("display", "")
                        End If
                    Catch Err As Exception
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestImmobili.GrdRowCommand.errore: ", Err)
                        Response.Redirect("../../PaginaErrore.aspx")
                    End Try
                Else
                    Dim oListDet() As ObjRidEseApplicati
                    Dim oNewListDet() As ObjRidEseApplicati = Nothing
                    Dim oNewDet As ObjRidEseApplicati
                    Dim x, nList As Integer
                    Dim oListDettaglioTestata() As ObjDettaglioTestata

                    Try
                        'carico l'oggetto
                        oListDet = Session("oDatiDet")
                        'carico il nuovo oggetto senza la riga selezionata
                        nList = -1
                        For x = 0 To oListDet.GetUpperBound(0)
                            If oListDet(x).sCodice <> IDRow Then
                                nList += 1
                                ReDim Preserve oNewListDet(nList)
                                oNewDet = New ObjRidEseApplicati
                                oNewDet = oListDet(x)
                                oNewListDet(nList) = oNewDet
                            End If
                        Next
                        'aggiorno il database
                        'carico l'array originario
                        oListDettaglioTestata = Session("oListUITessera")
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
                    Catch Err As Exception
                        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestImmobili.GrdRowCommand.errore: ", Err)
                        Response.Redirect("../../PaginaErrore.aspx")
                    End Try
                End If
                CmdModUI_Click(Nothing, Nothing)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestImmobili.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
    '    Try
    '        Dim IDRow As String = e.CommandArgument.ToString()
    '        If e.CommandName = "RowOpen" Then
    '            If CType(sender, Ribes.OPENgov.WebControls.RibesGridView).UniqueID = "GrdVani" Then
    '                Dim sScript As String
    '                Dim oMyUITemp As New ObjDettaglioTestata

    '                Try
    '                    '*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI & DIVERSA GESTIONE QUOTA VARIABILE***
    '                    If Request.Item("Provenienza") = Costanti.FormProvenienza.ICIDich Then
    '                        sScript = "GestAlert('a', 'warning', '', '', 'Impossibile accedere al dettaglio dalla gestione incrociata');"
    '                        RegisterScript(sScript, Me.GetType())
    '                    Else
    '                        'apro il popup passandogli l'indice dell'array da visualizzare
    '                        If LoadDatiFromForm(0, 0, oMyUITemp) = False Then
    '                            sScript = "GestAlert('a', 'danger', '', '', 'Errore in carico dati da form.')"
    '                        End If
    '                        Session("oUITemp") = oMyUITemp
    '                        sScript = "ShowInsertVani('" & hdIdContribuente.Value & "','" & hdIdTestata.Value & "','" & Request.Item("IdTessera") & "','" & Request.Item("IdUniqueUI") & "','" & IDRow & "','" & TxtIdUI.Text & "','" & Utility.Costanti.AZIONE_UPDATE & "','" & Request.Item("Provenienza") & "','" & GrdVani.SelectedIndex & "','" & Request.Item("IdList") & "','" & Request.Item("ParamRitornoICI") & "')"
    '                        RegisterScript(sScript, Me.GetType())
    '                    End If
    '                    '*** ***
    '                Catch Err As Exception
    '                    Log.Debug("Si è verificato un errore in GestImmobili::GrdVani_SelectedIndexChanged::" & Err.Message)
    '                End Try
    '            Else
    '                Dim sScript As String
    '                Try
    '                    For Each myRow As GridViewRow In GrdICI.Rows
    '                        If IDRow = CType(myRow.FindControl("hfIdOggetto"), HiddenField).Value Then
    '                            'se ho idoggetto chiedo se vogliono inserirlo altrimenti apro gestione ici
    '                            sScript = "parent.Comandi.location.href = '../.." & ConstSession.Path_ICI & "/CImmobileDettaglioMod.aspx?Operation=DETTAGLIO';"
    '                            sScript += "parent.Visualizza.location.href = '../.." & ConstSession.Path_ICI & "/ImmobileDettaglio.aspx"
    '                            sScript += "?IDTestata=" + CType(myRow.FindControl("hfIdTestata"), HiddenField).Value + "&IDImmobile=" + IDRow + "&IdAttoCompraVendita=0&IdDOCFA=0&TYPEOPERATION=DETTAGLIO&ProvenienzaTARSU=TARSU"
    '                            sScript += "&sProvenienza=" + Request.Item("sProvenienza")
    '                            sScript += "&IdContribuente=" + hdIdContribuente.Value + "&IdTessera=" + Request.Item("IdTessera") + "&IdUniqueUI=" + Request.Item("IdUniqueUI")
    '                            sScript += "&IsFromVariabile=" + Request.Item("IsFromVariabile")
    '                            sScript += "&AzioneProv=" + Request.Item("AzioneProv") + "&Provenienza=" + Request.Item("Provenienza") + "&IdList=" + Request.Item("IdList") + "'"
    '                            RegisterScript(sScript, Me.GetType())
    '                        End If
    '                    Next
    '                Catch Err As Exception
    '                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestImmobili.GrdRowCommand.errore: ", Err)
    '                    Response.Redirect("../../PaginaErrore.aspx")
    '                End Try
    '            End If
    '        ElseIf e.CommandName = "RowDelete" Then
    '            If CType(sender, Ribes.OPENgov.WebControls.RibesGridView).UniqueID = "GrdRiduzioni" Then
    '                Dim oListRid() As ObjRidEseApplicati
    '                Dim oNewListRid() As ObjRidEseApplicati = Nothing
    '                Dim oNewRid As ObjRidEseApplicati
    '                Dim x, nList As Integer
    '                Dim oListDettaglioTestata() As ObjDettaglioTestata

    '                Try
    '                    'carico l'oggetto
    '                    oListRid = Session("oDatiRid")
    '                    'carico il nuovo oggetto senza la riga selezionata
    '                    nList = -1
    '                    For x = 0 To oListRid.GetUpperBound(0)
    '                        If oListRid(x).sCodice <> IDRow Then
    '                            nList += 1
    '                            ReDim Preserve oNewListRid(nList)
    '                            oNewRid = New ObjRidEseApplicati
    '                            oNewRid = oListRid(x)
    '                            oNewListRid(nList) = oNewRid
    '                        End If
    '                    Next
    '                    'aggiorno il database
    '                    'carico l'array originario
    '                    oListDettaglioTestata = Session("oListUITessera")
    '                    'carico l'oggetto
    '                    Session("oDatiRid") = oNewListRid
    '                    If Not Session("oDatiRid") Is Nothing Then
    '                        GrdRiduzioni.Style.Add("display", "")
    '                        GrdRiduzioni.DataSource = Session("oDatiRid")
    '                        GrdRiduzioni.DataBind()
    '                        LblResultRid.Style.Add("display", "none")
    '                    Else
    '                        GrdRiduzioni.Style.Add("display", "none")
    '                        LblResultRid.Style.Add("display", "")
    '                    End If
    '                Catch Err As Exception
    '                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestImmobili.GrdRowCommand.errore: ", Err)
    '                    Response.Redirect("../../PaginaErrore.aspx")
    '                End Try
    '            Else
    '                Dim oListDet() As ObjRidEseApplicati
    '                Dim oNewListDet() As ObjRidEseApplicati = Nothing
    '                Dim oNewDet As ObjRidEseApplicati
    '                Dim x, nList As Integer
    '                Dim oListDettaglioTestata() As ObjDettaglioTestata

    '                Try
    '                    'carico l'oggetto
    '                    oListDet = Session("oDatiDet")
    '                    'carico il nuovo oggetto senza la riga selezionata
    '                    nList = -1
    '                    For x = 0 To oListDet.GetUpperBound(0)
    '                        If oListDet(x).sCodice <> IDRow Then
    '                            nList += 1
    '                            ReDim Preserve oNewListDet(nList)
    '                            oNewDet = New ObjRidEseApplicati
    '                            oNewDet = oListDet(x)
    '                            oNewListDet(nList) = oNewDet
    '                        End If
    '                    Next
    '                    'aggiorno il database
    '                    'carico l'array originario
    '                    oListDettaglioTestata = Session("oListUITessera")
    '                    'carico l'oggetto
    '                    Session("oDatiDet") = oNewListDet
    '                    If Not Session("oDatiDet") Is Nothing Then
    '                        GrdDetassazioni.Style.Add("display", "")
    '                        GrdDetassazioni.DataSource = Session("oDatiDet")
    '                        GrdDetassazioni.SelectedIndex = -1
    '                        GrdDetassazioni.DataBind()
    '                        LblResultDet.Style.Add("display", "none")
    '                    Else
    '                        GrdDetassazioni.Style.Add("display", "none")
    '                        LblResultDet.Style.Add("display", "")
    '                    End If
    '                Catch Err As Exception
    '                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestImmobili.GrdRowCommand.errore: ", Err)
    '                    Response.Redirect("../../PaginaErrore.aspx")
    '                End Try
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestImmobili.GrdRowCommand.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
#End Region
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub LnkNewVani_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles LnkNewVani.Click
        Dim sScript As String
        Dim oMyUITemp As New ObjDettaglioTestata

        Try
            If LoadDatiFromForm(0, 0, oMyUITemp) = False Then
                sScript = "GestAlert('a', 'danger', '', '', 'Errore in carico dati da form.');"
            End If
            Session("oUITemp") = oMyUITemp
            sScript = "ShowInsertVani(" & hdIdContribuente.Value & "," & hdIdTestata.Value & "," & Request.Item("IdTessera") & "," & Request.Item("IdUniqueUI") & ",-1,-1,'" & Utility.Costanti.AZIONE_NEW & "','" & Request.Item("Provenienza") & "',-1,'" & Request.Item("IdList") & "')"
            'RegisterScript( sScript,Me.GetType)
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestImmobili.LinkNewVani_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ChkIsGiornaliera_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkIsGiornaliera.CheckedChanged
        If ChkIsGiornaliera.Checked = True Then
            TxtGGTarsu.Enabled = True
        Else
            TxtGGTarsu.Enabled = False
        End If
    End Sub

    'M.B. segnalazione 5/17 CMGC
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdClearDatiUI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClearDatiUI.Click
        Try

            'Dim sScript As String = ""
            'sScript += "document.getElementById('#Salva').disabled = False;"
            'RegisterScript(sScript, Me.GetType())

            'in origine era presente solo questa riga nel TRY
            UnloadPage()

        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestImmobili.CmdClearDatiUI_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    '*** 20140805 - Gestione Categorie Vani ***
    'Private Sub CmdSalvaDatiUI_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdSalvaDatiUI.Click
    '    Dim x, nVani As Integer
    '    Dim nMQ As Double
    '    Dim oMyTot As New GestOggetti
    '    Dim sScript, sProvenienza As String
    '    Dim oMyDettaglioTestata As New ObjDettaglioTestata

    '    '***SE SONO IN NUOVO INSERIMENTO DICHIARAZIONE SALVO I DATI SOLO IN MEMORIA
    '    '***SE SONO IN MODIFICA DI UN IMMOBILE GIA' INSERITO SALVO I DATI IN MEMORIA E SUL DATABASE
    '    '***SE SONO IN AGGIUNTA DI UN IMMOBILE AD UNA DICHIARAZIONE GIA' INSERITA SALVO I DATI IN MEMORIA E SUL DATABASE
    '    Try
    '        'inizializzo la connessione
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If
    '        'calcolo il totale di vani e mq per l'immobile
    '        x = oMyTot.GetTotOggetti(Session("oDatiVani"), nVani, nMQ)
    '        If x <> 1 Then
    '            Response.Redirect("../../PaginaErrore.aspx")
    '            Exit Sub
    '        End If
    '        'controllo che tutti i vani abbiamo la categoria
    '        If Not Session("oDatiVani") Is Nothing Then
    '            Dim oMyOggetti() As ObjOggetti
    '            oMyOggetti = Session("oDatiVani")
    '            For x = 0 To oMyOggetti.GetUpperBound(0)
    '                If oMyOggetti(x).IdCategoria = "" Then
    '                    sScript = "<script language='javascript'>alert('Inserire la Categoria su tutti i vani!')</script>"
    '                    RegisterScript(me.gettype(),"msg", sScript)
    '                    Exit Sub
    '                End If
    '            Next
    '        Else
    '            sScript = "<script language='javascript'>alert('Inserire almeno un vano!')</script>"
    '            RegisterScript(me.gettype(),"msg", sScript)
    '            Exit Sub
    '        End If
    '        'carico i dati dal form
    '        oMyDettaglioTestata.Id = TxtIdUI.Text
    '        oMyDettaglioTestata.IdDettaglioTestata = TxtIdDettaglioTestata.Text
    '        oMyDettaglioTestata.IdTessera = TxtIdTessera.Text
    '        oMyDettaglioTestata.IdPadre = TxtIdPadre.Text
    '        oMyDettaglioTestata.sCodVia = TxtCodVia.Text
    '        oMyDettaglioTestata.sVia = TxtVia.Text.Trim
    '        oMyDettaglioTestata.sCivico = TxtCivico.Text.Trim
    '        oMyDettaglioTestata.sEsponente = TxtEsponente.Text.Trim
    '        oMyDettaglioTestata.sInterno = TxtInterno.Text.Trim
    '        oMyDettaglioTestata.sScala = TxtScala.Text.Trim
    '        oMyDettaglioTestata.tDataInizio = TxtDataInizio.Text
    '        If TxtDataFine.Text <> "" Then
    '            oMyDettaglioTestata.tDataFine = TxtDataFine.Text
    '        End If
    '        oMyDettaglioTestata.nVani = nVani
    '        oMyDettaglioTestata.nMQ = nMQ
    '        If TxtGGTarsu.Text.Trim <> "" Then
    '            oMyDettaglioTestata.nGGTarsu = CInt(TxtGGTarsu.Text.Trim)
    '        End If
    '        If TxtNComponenti.Text <> "" Then
    '            oMyDettaglioTestata.nNComponenti = TxtNComponenti.Text
    '        End If
    '        oMyDettaglioTestata.sFoglio = TxtFoglio.Text
    '        oMyDettaglioTestata.sNumero = TxtNumero.Text
    '        oMyDettaglioTestata.sSubalterno = TxtSubalterno.Text
    '        '***Agenzia Entrate***
    '        oMyDettaglioTestata.sSezione = TxtSezione.Text
    '        oMyDettaglioTestata.sEstensioneParticella = TxtEstParticella.Text
    '        oMyDettaglioTestata.sIdTipoParticella = DdlTipoParticella.SelectedValue
    '        If DdlTitOccupaz.SelectedValue <> "" Then
    '            oMyDettaglioTestata.nIdTitoloOccupaz = DdlTitOccupaz.SelectedValue
    '        End If
    '        If DdlNatOccupaz.SelectedValue <> "" Then
    '            oMyDettaglioTestata.nIdNaturaOccupaz = DdlNatOccupaz.SelectedValue
    '        End If
    '        If DdlDestUso.SelectedValue <> "" Then
    '            oMyDettaglioTestata.nIdDestUso = DdlDestUso.SelectedValue
    '        End If
    '        oMyDettaglioTestata.sIdTipoUnita = DdlTipoUnita.SelectedValue
    '        If DdlAssenzaDatiCat.SelectedValue <> "" Then
    '            oMyDettaglioTestata.nIdAssenzaDatiCatastali = DdlAssenzaDatiCat.SelectedValue
    '        End If
    '        '*********************
    '        oMyDettaglioTestata.sIdStatoOccupazione = DdlStatoOccupazione.SelectedValue
    '        oMyDettaglioTestata.sNomeProprietario = TxtPropietario.Text
    '        oMyDettaglioTestata.sNomeOccupantePrec = TxtOccupantePrec.Text
    '        oMyDettaglioTestata.sNoteUI = TxtNoteUI.Text
    '        oMyDettaglioTestata.tDataInserimento = Now
    '        oMyDettaglioTestata.tDataCessazione = Nothing
    '        oMyDettaglioTestata.sOperatore = ConstSession.UserName
    '        oMyDettaglioTestata.oOggetti = Session("oDatiVani")
    '        ''*** 20140805 - Gestione Categorie Vani ***
    '        'se sono stati cambiati categoria e/o nc devono essere aggiornati anche i relativi oggetti
    '        If oMyDettaglioTestata.IdCatAteco.ToString() + "|" + oMyDettaglioTestata.nNComponenti.ToString() + "|" + oMyDettaglioTestata.nComponentiPV.ToString() <> Session("oldcattares") Then
    '            For Each myVano As ObjOggetti In oMyDettaglioTestata.oOggetti
    '                If myVano.IdCategoriatares.tostring() + "|" + myVano.nnc.tostring() + "|" + myVano.nncpv.tostring() = Session("oldcattares") Then
    '                    myVano.IdCategoriatares = oMyDettaglioTestata.IdCatAteco
    '                    myVano.nnc = oMyDettaglioTestata.nNComponenti
    '                    myVano.nncpv = oMyDettaglioTestata.nComponentiPV
    '                End If
    '            Next
    '        End If
    '        ''*** ***
    '        oMyDettaglioTestata.oRiduzioni = Session("oDatiRid")
    '        oMyDettaglioTestata.oDetassazioni = Session("oDatiDet")
    '        '*** 20130201 - gestione mq da catasto per TARES ***
    '        Log.Debug("GestImmobili::CmdSalvaDatiUI_Click::devo registrare mqcatasto")
    '        If TxtMQCatasto.Text.Trim <> "" Then
    '            oMyDettaglioTestata.nMQCatasto = CDbl(TxtMQCatasto.Text.Trim)
    '        End If
    '        '*** ***
    '        '*** 20130325 - gestione mq tassabili per TARES ***
    '        Log.Debug("GestImmobili::CmdSalvaDatiUI_Click::devo registrare mqtassabili")
    '        If TxtMQTassabili.Text.Trim <> "" Then
    '            oMyDettaglioTestata.nMQTassabili = CDbl(TxtMQTassabili.Text.Trim)
    '        End If
    '        '*** ***
    '        '*** 20130228 - gestione categoria Ateco per TARES ***
    '        'Log.Debug("GestImmobili::CmdSalvaDatiUI_Click::devo registrare ddlcattares")
    '        If ddlcattares.SelectedValue <> "" Then
    '            oMyDettaglioTestata.IdCatAteco = ddlcattares.SelectedValue
    '        End If
    '        'Log.Debug("GestImmobili::CmdSalvaDatiUI_Click::devo registrare ncomponentipv")
    '        If TxtNComponentiPV.Text.Trim <> "" Then
    '            oMyDettaglioTestata.nComponentiPV = CInt(TxtNComponentiPV.Text.Trim)
    '        Else
    '            oMyDettaglioTestata.nComponentiPV = oMyDettaglioTestata.nNComponenti
    '        End If
    '        'Log.Debug("GestImmobili::CmdSalvaDatiUI_Click::devo registrare forzapv")
    '        oMyDettaglioTestata.bForzaPV = ChkForzaPV.Checked
    '        '*** ***

    '        If Request.Item("Provenienza") = Costanti.FORMPROVENIENZA.DATIAE Then
    '            If SaveFromGestAE(oMyDettaglioTestata, WFSessione) = False Then
    '                Response.Redirect("../../PaginaErrore.aspx")
    '                Exit Sub
    '            Else
    '                sScript += "parent.Visualizza.location.href='../AgenziaEntrate/DatiMancanti/RicercaDatiMancanti.aspx';" & vbCrLf
    '                sScript += "parent.Comandi.location.href='../AgenziaEntrate/DatiMancanti/ComandiRicDatiMancanti.aspx';"
    '            End If
    '        Else
    '            If SaveFromGestDich(oMyDettaglioTestata, WFSessione, sScript) = False Then
    '                If sScript <> "" Then
    '                    RegisterScript(me.gettype(),"msg", sScript)
    '                    Exit Sub
    '                Else
    '                    Response.Redirect("../../PaginaErrore.aspx")
    '                    Exit Sub
    '                End If
    '            Else
    '                If Request.Item("Provenienza") = Costanti.FORMPROVENIENZA.TESSERA Then
    '                    sScript += BackToTessere()
    '                ElseIf Request.Item("Provenienza") = Costanti.FORMPROVENIENZA.DICHIARAZIONE Then
    '                    sScript += BackToDichiarazione()
    '                End If
    '            End If
    '        End If
    '        RegisterScript(me.gettype(),"", "<script language='javascript'>" & sScript & "</script>")
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestImmobili.CmdSalvaDatiUI_Click.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    Finally
    '        If Not IsNothing(WFSessione) Then
    '            WFSessione.Kill()
    '            WFSessione = Nothing
    '        End If
    '    End Try
    'End Sub
    ''' <summary>
    ''' Pulsante di salvataggio
    ''' ***SE SONO IN NUOVO INSERIMENTO DICHIARAZIONE SALVO I DATI SOLO IN MEMORIA
    ''' ***SE SONO IN MODIFICA DI UN IMMOBILE GIA' INSERITO SALVO I DATI IN MEMORIA E SUL DATABASE
    ''' ***SE SONO IN AGGIUNTA DI UN IMMOBILE AD UNA DICHIARAZIONE GIA' INSERITA SALVO I DATI IN MEMORIA E SUL DATABASE
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Analisi eventi</em>
    ''' </revision>
    ''' </revisionHistory>
    Private Sub CmdSalvaDatiUI_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdSalvaDatiUI.Click
        Dim x, nVani As Integer
        Dim nMQ As Double
        Dim oMyTot As New GestOggetti
        Dim sScript As String = ""
        Dim oMyDettaglioTestata As New ObjDettaglioTestata

        Try
            'calcolo il totale di vani e mq per l'immobile
            x = oMyTot.GetTotOggetti(Session("oDatiVani"), nVani, nMQ)
            If x <> 1 Then
                Response.Redirect("../../PaginaErrore.aspx")
                Exit Sub
            End If
            'controllo che tutti i vani abbiamo la categoria
            If Not Session("oDatiVani") Is Nothing Then
                Dim oMyOggetti() As ObjOggetti
                oMyOggetti = Session("oDatiVani")
                For x = 0 To oMyOggetti.GetUpperBound(0)
                    If ConstSession.IsFromTARES = "1" Then
                        If oMyOggetti(x).IdCatTARES = -1 Then
                            sScript = "GestAlert('a', 'warning', '', '', 'Inserire la Categoria su tutti i vani!');"
                            RegisterScript(sScript, Me.GetType())
                            Exit Sub
                        End If
                    Else
                        If (oMyOggetti(x).IdCategoria = "" Or oMyOggetti(x).IdCategoria = "-1") Then
                            sScript = "GestAlert('a', 'warning', '', '', 'Inserire la Categoria su tutti i vani!');"
                            RegisterScript(sScript, Me.GetType())
                            Exit Sub
                        End If
                    End If
                Next
            Else
                sScript = "GestAlert('a', 'warning', '', '', 'Inserire almeno un vano!');"
                RegisterScript(sScript, Me.GetType())
                Exit Sub
            End If
            If LoadDatiFromForm(nVani, nMQ, oMyDettaglioTestata) = False Then
                sScript = "GestAlert('a', 'danger', '', '', 'Errore in carico dati da form.');"
                RegisterScript(sScript, Me.GetType())
            Else
                If Request.Item("Provenienza") = Costanti.FormProvenienza.DatiAE Then
                    If SaveFromGestAE(oMyDettaglioTestata) = False Then
                        Response.Redirect("../../PaginaErrore.aspx")
                        Exit Sub
                    Else
                        sScript += "parent.Visualizza.location.href='../AgenziaEntrate/DatiMancanti/RicercaDatiMancanti.aspx';" & vbCrLf
                        sScript += "parent.Comandi.location.href='../AgenziaEntrate/DatiMancanti/ComandiRicDatiMancanti.aspx';"
                        RegisterScript(sScript, Me.GetType())
                    End If
                Else
                    If SaveFromGestDich(ConstSession.DBType, ConstSession.StringConnection, oMyDettaglioTestata, sScript) = False Then
                        If sScript <> "" Then
                            RegisterScript(sScript, Me.GetType())
                            Exit Sub
                        Else
                            Response.Redirect("../../PaginaErrore.aspx")
                            Exit Sub
                        End If
                    Else
                        '*** 20130923 - gestione modifiche tributarie ***
                        'sono in modifica di un immobile già inserito nel db
                        Dim FncModificheTributarie As New Utility.ModificheTributarie
                        If Request.Item("IdUniqueUI") <> -1 Then
                            Dim nTypeVar As Integer
                            If Session("OldFoglio") <> oMyDettaglioTestata.sFoglio Or Session("OldNumero") <> oMyDettaglioTestata.sNumero Or Session("OldSubalterno") <> oMyDettaglioTestata.sSubalterno Then
                                nTypeVar = Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneRiferimentiCatastali
                            ElseIf Session("OldDataInizio") <> oMyDettaglioTestata.tDataInizio Or Session("OldDataFine") <> oMyDettaglioTestata.tDataFine Then
                                nTypeVar = Utility.ModificheTributarie.ModificheTributarieCausali.VariazionePeriodo
                            ElseIf Session("OldStatoOccupazione") <> oMyDettaglioTestata.nIdNaturaOccupaz.ToString Then
                                nTypeVar = Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneStatoOccupazione
                            ElseIf Session("OldTitoloOccupazione") <> oMyDettaglioTestata.nIdTitoloOccupaz.ToString Then
                                nTypeVar = Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneTitoloOccupazione
                            ElseIf Session("OldDestinazioneUso") <> oMyDettaglioTestata.nIdDestUso.ToString Then
                                nTypeVar = Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneDestinazionedUso
                            End If

                            If FncModificheTributarie.SetModificheTributarie(ConstSession.StringConnectionOPENgov, Utility.ModificheTributarie.DBOperation.Insert, 0, ConstSession.IdEnte, ConstSession.CodTributo, nTypeVar, oMyDettaglioTestata.sFoglio, oMyDettaglioTestata.sNumero, oMyDettaglioTestata.sSubalterno, Now, ConstSession.UserName, oMyDettaglioTestata.Id, Date.MaxValue) = False Then
                                Log.Debug("Errore in SetModificheTributarie")
                            End If
                        Else
                            If FncModificheTributarie.SetModificheTributarie(ConstSession.StringConnectionOPENgov, Utility.ModificheTributarie.DBOperation.Insert, 0, ConstSession.IdEnte, ConstSession.CodTributo, Utility.ModificheTributarie.ModificheTributarieCausali.NuovaDichiarazione, oMyDettaglioTestata.sFoglio, oMyDettaglioTestata.sNumero, oMyDettaglioTestata.sSubalterno, Now, ConstSession.UserName, oMyDettaglioTestata.Id, Date.MaxValue) = False Then
                                Log.Debug("Errore in SetModificheTributarie")
                            End If
                        End If
                        '*** ***
                        UnloadPage()
                    End If
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestImmobili.CmdSalvaDatiUI_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub CmdSalvaDatiUI_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdSalvaDatiUI.Click
    '    Dim x, nVani As Integer
    '    Dim nMQ As Double
    '    Dim oMyTot As New GestOggetti
    '    Dim sScript As String = ""
    '    Dim oMyDettaglioTestata As New ObjDettaglioTestata

    '    '***SE SONO IN NUOVO INSERIMENTO DICHIARAZIONE SALVO I DATI SOLO IN MEMORIA
    '    '***SE SONO IN MODIFICA DI UN IMMOBILE GIA' INSERITO SALVO I DATI IN MEMORIA E SUL DATABASE
    '    '***SE SONO IN AGGIUNTA DI UN IMMOBILE AD UNA DICHIARAZIONE GIA' INSERITA SALVO I DATI IN MEMORIA E SUL DATABASE
    '    Try
    '        'inizializzo la connessione
    '        'WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        'If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '        '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        'End If
    '        'calcolo il totale di vani e mq per l'immobile
    '        x = oMyTot.GetTotOggetti(Session("oDatiVani"), nVani, nMQ)
    '        If x <> 1 Then
    '            Response.Redirect("../../PaginaErrore.aspx")
    '            Exit Sub
    '        End If
    '        'controllo che tutti i vani abbiamo la categoria
    '        If Not Session("oDatiVani") Is Nothing Then
    '            Dim oMyOggetti() As ObjOggetti
    '            oMyOggetti = Session("oDatiVani")
    '            For x = 0 To oMyOggetti.GetUpperBound(0)
    '                If ConstSession.IsFromTARES = "1" Then
    '                    If oMyOggetti(x).IdCatTARES = -1 Then
    '                        sScript = "GestAlert('a', 'warning', '', '', 'Inserire la Categoria su tutti i vani!');"
    '                        'RegisterScript( sScript,Me.GetType)
    '                        RegisterScript(sScript, Me.GetType())
    '                        Exit Sub
    '                    End If
    '                Else
    '                    If (oMyOggetti(x).IdCategoria = "" Or oMyOggetti(x).IdCategoria = "-1") Then
    '                        sScript = "GestAlert('a', 'warning', '', '', 'Inserire la Categoria su tutti i vani!');"
    '                        'RegisterScript( sScript,Me.GetType)
    '                        RegisterScript(sScript, Me.GetType())
    '                        Exit Sub
    '                    End If
    '                End If
    '            Next
    '        Else
    '            sScript = "GestAlert('a', 'warning', '', '', 'Inserire almeno un vano!');"
    '            'RegisterScript( sScript,Me.GetType)
    '            RegisterScript(sScript, Me.GetType())
    '            Exit Sub
    '        End If
    '        If LoadDatiFromForm(nVani, nMQ, oMyDettaglioTestata) = False Then
    '            sScript = "GestAlert('a', 'danger', '', '', 'Errore in carico dati da form.');"
    '            RegisterScript(sScript, Me.GetType())
    '        Else
    '            If Request.Item("Provenienza") = Costanti.FormProvenienza.DatiAE Then
    '                'If SaveFromGestAE(oMyDettaglioTestata, WFSessione) = False Then
    '                If SaveFromGestAE(oMyDettaglioTestata) = False Then
    '                    Response.Redirect("../../PaginaErrore.aspx")
    '                    Exit Sub
    '                Else
    '                    sScript += "parent.Visualizza.location.href='../AgenziaEntrate/DatiMancanti/RicercaDatiMancanti.aspx';" & vbCrLf
    '                    sScript += "parent.Comandi.location.href='../AgenziaEntrate/DatiMancanti/ComandiRicDatiMancanti.aspx';"
    '                    RegisterScript(sScript, Me.GetType())
    '                End If
    '            Else
    '                Log.Debug("sono in::" & ConstSession.StringConnection)
    '                If SaveFromGestDich(ConstSession.DBType, ConstSession.StringConnection, oMyDettaglioTestata, sScript) = False Then 'If SaveFromGestDich(oMyDettaglioTestata, WFSessione, sScript) = False Then
    '                    If sScript <> "" Then
    '                        'RegisterScript( sScript,Me.GetType)
    '                        RegisterScript(sScript, Me.GetType())
    '                        Exit Sub
    '                    Else
    '                        Response.Redirect("../../PaginaErrore.aspx")
    '                        Exit Sub
    '                    End If
    '                Else
    '                    '*** 20130923 - gestione modifiche tributarie ***
    '                    'sono in modifica di un immobile già inserito nel db
    '                    If Request.Item("IdUniqueUI") <> -1 Then 'If TxtIdTessera.Text <> "-1" Then
    '                        Dim FncModificheTributarie As New Utility.ModificheTributarie
    '                        If TxtIdUI.Text > 0 Then
    '                            If Session("OldFoglio") <> oMyDettaglioTestata.sFoglio Or Session("OldNumero") <> oMyDettaglioTestata.sNumero Or Session("OldSubalterno") <> oMyDettaglioTestata.sSubalterno Then
    '                                If FncModificheTributarie.SetModificheTributarie(ConstSession.StringConnectionOPENgov, Utility.ModificheTributarie.DBOperation.Insert, 0, ConstSession.IdEnte, ConstSession.CodTributo, Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneRiferimentiCatastali, oMyDettaglioTestata.sFoglio, oMyDettaglioTestata.sNumero, oMyDettaglioTestata.sSubalterno, Now, ConstSession.UserName, oMyDettaglioTestata.Id, Date.MaxValue) = False Then
    '                                    Log.Debug("Errore in SetModificheTributarie::@DBOperation=0::@IDVARIAZIONE=0::@IDENTE=" & ConstSession.IdEnte & "::@TRIBUTO=" & ConstSession.CodTributo & "::@IDCAUSALE=" & Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneRiferimentiCatastali & "::@FOGLIO=" & oMyDettaglioTestata.sFoglio & "::@NUMERO=" & oMyDettaglioTestata.sNumero & "::@SUBALTERNO=" & oMyDettaglioTestata.sSubalterno & "::@DATAVARIAZIONE=" & Now & "::@OPERATORE=" & ConstSession.UserName & "::@IDOGGETTOTRIBUTI=" & oMyDettaglioTestata.Id & "::@DATATRATTATO=" & Date.MaxValue)
    '                                End If
    '                            ElseIf Session("OldDataInizio") <> oMyDettaglioTestata.tDataInizio Or Session("OldDataFine") <> oMyDettaglioTestata.tDataFine Then
    '                                If FncModificheTributarie.SetModificheTributarie(ConstSession.StringConnectionOPENgov, Utility.ModificheTributarie.DBOperation.Insert, 0, ConstSession.IdEnte, ConstSession.CodTributo, Utility.ModificheTributarie.ModificheTributarieCausali.VariazionePeriodo, oMyDettaglioTestata.sFoglio, oMyDettaglioTestata.sNumero, oMyDettaglioTestata.sSubalterno, Now, ConstSession.UserName, oMyDettaglioTestata.Id, Date.MaxValue) = False Then
    '                                    Log.Debug("Errore in SetModificheTributarie::@DBOperation=0::@IDVARIAZIONE=0::@IDENTE=" & ConstSession.IdEnte & "::@TRIBUTO=" & ConstSession.CodTributo & "::@IDCAUSALE=" & Utility.ModificheTributarie.ModificheTributarieCausali.VariazionePeriodo & "::@FOGLIO=" & oMyDettaglioTestata.sFoglio & "::@NUMERO=" & oMyDettaglioTestata.sNumero & "::@SUBALTERNO=" & oMyDettaglioTestata.sSubalterno & "::@DATAVARIAZIONE=" & Now & "::@OPERATORE=" & ConstSession.UserName & "::@IDOGGETTOTRIBUTI=" & oMyDettaglioTestata.Id & "::@DATATRATTATO=" & Date.MaxValue)
    '                                End If
    '                            ElseIf Session("OldNComponenti") <> oMyDettaglioTestata.nNComponenti.ToString Then
    '                                If FncModificheTributarie.SetModificheTributarie(ConstSession.StringConnectionOPENgov, Utility.ModificheTributarie.DBOperation.Insert, 0, ConstSession.IdEnte, ConstSession.CodTributo, Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneComponenti, oMyDettaglioTestata.sFoglio, oMyDettaglioTestata.sNumero, oMyDettaglioTestata.sSubalterno, Now, ConstSession.UserName, oMyDettaglioTestata.Id, Date.MaxValue) = False Then
    '                                    Log.Debug("Errore in SetModificheTributarie::@DBOperation=0::@IDVARIAZIONE=0::@IDENTE=" & ConstSession.IdEnte & "::@TRIBUTO=" & ConstSession.CodTributo & "::@IDCAUSALE=" & Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneComponenti & "::@FOGLIO=" & oMyDettaglioTestata.sFoglio & "::@NUMERO=" & oMyDettaglioTestata.sNumero & "::@SUBALTERNO=" & oMyDettaglioTestata.sSubalterno & "::@DATAVARIAZIONE=" & Now & "::@OPERATORE=" & ConstSession.UserName & "::@IDOGGETTOTRIBUTI=" & oMyDettaglioTestata.Id & "::@DATATRATTATO=" & Date.MaxValue)
    '                                End If
    '                            ElseIf Session("OldStatoOccupazione") <> oMyDettaglioTestata.nIdNaturaOccupaz.ToString Then
    '                                If FncModificheTributarie.SetModificheTributarie(ConstSession.StringConnectionOPENgov, Utility.ModificheTributarie.DBOperation.Insert, 0, ConstSession.IdEnte, ConstSession.CodTributo, Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneStatoOccupazione, oMyDettaglioTestata.sFoglio, oMyDettaglioTestata.sNumero, oMyDettaglioTestata.sSubalterno, Now, ConstSession.UserName, oMyDettaglioTestata.Id, Date.MaxValue) = False Then
    '                                    Log.Debug("Errore in SetModificheTributarie::@DBOperation=0::@IDVARIAZIONE=0::@IDENTE=" & ConstSession.IdEnte & "::@TRIBUTO=" & ConstSession.CodTributo & "::@IDCAUSALE=" & Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneStatoOccupazione & "::@FOGLIO=" & oMyDettaglioTestata.sFoglio & "::@NUMERO=" & oMyDettaglioTestata.sNumero & "::@SUBALTERNO=" & oMyDettaglioTestata.sSubalterno & "::@DATAVARIAZIONE=" & Now & "::@OPERATORE=" & ConstSession.UserName & "::@IDOGGETTOTRIBUTI=" & oMyDettaglioTestata.Id & "::@DATATRATTATO=" & Date.MaxValue)
    '                                End If
    '                            ElseIf Session("OldTitoloOccupazione") <> oMyDettaglioTestata.nIdTitoloOccupaz.ToString Then
    '                                If FncModificheTributarie.SetModificheTributarie(ConstSession.StringConnectionOPENgov, Utility.ModificheTributarie.DBOperation.Insert, 0, ConstSession.IdEnte, ConstSession.CodTributo, Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneTitoloOccupazione, oMyDettaglioTestata.sFoglio, oMyDettaglioTestata.sNumero, oMyDettaglioTestata.sSubalterno, Now, ConstSession.UserName, oMyDettaglioTestata.Id, Date.MaxValue) = False Then
    '                                    Log.Debug("Errore in SetModificheTributarie::@DBOperation=0::@IDVARIAZIONE=0::@IDENTE=" & ConstSession.IdEnte & "::@TRIBUTO=" & ConstSession.CodTributo & "::@IDCAUSALE=" & Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneTitoloOccupazione & "::@FOGLIO=" & oMyDettaglioTestata.sFoglio & "::@NUMERO=" & oMyDettaglioTestata.sNumero & "::@SUBALTERNO=" & oMyDettaglioTestata.sSubalterno & "::@DATAVARIAZIONE=" & Now & "::@OPERATORE=" & ConstSession.UserName & "::@IDOGGETTOTRIBUTI=" & oMyDettaglioTestata.Id & "::@DATATRATTATO=" & Date.MaxValue)
    '                                End If
    '                            ElseIf Session("OldDestinazioneUso") <> oMyDettaglioTestata.nIdDestUso.ToString Then
    '                                If FncModificheTributarie.SetModificheTributarie(ConstSession.StringConnectionOPENgov, Utility.ModificheTributarie.DBOperation.Insert, 0, ConstSession.IdEnte, ConstSession.CodTributo, Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneDestinazionedUso, oMyDettaglioTestata.sFoglio, oMyDettaglioTestata.sNumero, oMyDettaglioTestata.sSubalterno, Now, ConstSession.UserName, oMyDettaglioTestata.Id, Date.MaxValue) = False Then
    '                                    Log.Debug("Errore in SetModificheTributarie::@DBOperation=0::@IDVARIAZIONE=0::@IDENTE=" & ConstSession.IdEnte & "::@TRIBUTO=" & ConstSession.CodTributo & "::@IDCAUSALE=" & Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneDestinazionedUso & "::@FOGLIO=" & oMyDettaglioTestata.sFoglio & "::@NUMERO=" & oMyDettaglioTestata.sNumero & "::@SUBALTERNO=" & oMyDettaglioTestata.sSubalterno & "::@DATAVARIAZIONE=" & Now & "::@OPERATORE=" & ConstSession.UserName & "::@IDOGGETTOTRIBUTI=" & oMyDettaglioTestata.Id & "::@DATATRATTATO=" & Date.MaxValue)
    '                                End If
    '                            End If
    '                        End If
    '                    End If
    '                    '*** ***
    '                    'If Request.Item("Provenienza") = Costanti.FORMPROVENIENZA.TESSERA Then
    '                    '    sScript += BackToTessere()
    '                    'ElseIf Request.Item("Provenienza") = Costanti.FORMPROVENIENZA.DICHIARAZIONE Then
    '                    '    sScript += BackToDichiarazione()
    '                    'End If
    '                    UnloadPage()
    '                End If
    '            End If
    '        End If
    '        'RegisterScript( sScript,Me.GetType)
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestImmobili.CmdSalvaDatiUI_Click.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    Finally
    '        'If Not IsNothing(WFSessione) Then
    '        '    WFSessione.Kill()
    '        '    WFSessione = Nothing
    '        'End If
    '    End Try
    'End Sub
    '*** ***
    ''' <summary>
    ''' pulsante per l'abilitazione della pagina alla modifica e memorizzazione dati originali
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdModUI_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdModUI.Click
        Dim sScript As String = ""
        Try
            Abilita(True, "")
            'memorizzo l'immobile originale
            Session("sOldMyUI") = TxtVia.Text + "|" + TxtCivico.Text + "|" + TxtEsponente.Text + "|" + TxtInterno.Text + "|" + TxtScala.Text + "|" +
            TxtDataInizio.Text + "|" + TxtDataFine.Text + "|" + TxtGGTarsu.Text + "|" + TxtNComponenti.Text + "|" + TxtMQCatasto.Text + "|" +
            TxtMQTassabili.Text + "|" + TxtRidImp.Text
            'memorizzo i dati non sostanziali originali
            Session("sOldMyVarieUI") = TxtFoglio.Text + "|" + TxtNumero.Text + "|" + TxtSubalterno.Text + "|" + TxtPropietario.Text + "|" + TxtOccupantePrec.Text + "|" + TxtNoteUI.Text
            '*** 20140805 - Gestione Categorie Vani ***
            'memorizzo i dati di categoria TARES originali
            Session("OldCatTARES") = DDlCatTARES.SelectedValue.ToString + "|" + TxtNComponenti.Text + "|" + TxtNComponentiPV.Text
            '*** ***
            '*** 20130923 - gestione modifiche tributarie ***
            Session("OldFoglio") = TxtFoglio.Text : Session("OldNumero") = TxtNumero.Text : Session("OldSubalterno") = TxtSubalterno.Text
            Session("OldNComponenti") = TxtNComponenti.Text
            Session("OldStatoOccupazione") = DdlNatOccupaz.SelectedItem.Value
            Session("OldTitoloOccupazione") = DdlTitOccupaz.SelectedItem.Value
            Session("OldDestinazioneUso") = DdlDestUso.SelectedItem.Value
            If TxtDataInizio.Text <> "" Then
                Session("OldDataInizio") = TxtDataInizio.Text
            Else
                Session("OldDataInizio") = DateTime.MinValue
            End If
            If TxtDataFine.Text <> "" Then
                Session("OldDataFine") = TxtDataFine.Text
            Else
                Session("OldDataFine") = DateTime.MinValue
            End If
            '*** ***
            'abilito il pulsante di salvataggio
            Dim oListCmd() As Object
            ReDim Preserve oListCmd(0)
            oListCmd(0) = "Salva"
            For Each myItem As String In oListCmd
                sScript += "$('#" & myItem & "').removeClass('DisableBtn');"
            Next
            ReDim Preserve oListCmd(2)
            oListCmd(0) = "Modifica"
            oListCmd(1) = "Delete"
            oListCmd(2) = "Duplica"
            For Each myItem As String In oListCmd
                sScript += "$('#" & myItem & "').addClass('DisableBtn');"
            Next
            sScript += "$('.BottoneCancellaGrd').removeClass('DisableBtn');"
            RegisterScript(sScript, Me.GetType())
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestImmobili.CmdModUI_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdDuplicaUI_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdDuplicaUI.Click
        Dim sScript As String = ""
        Dim sListCmd() As Object
        Dim x As Integer

        Try
            TxtIdUI.Text = "-1"
            TxtIdDettaglioTestata.Text = "-1"
            Session("IsDuplicaUI") = "1" 'per indicare che sto facendo una duplica UI
            TxtIsRibalta.Text = "0"

            Session("oDatiVani") = Nothing
            GrdVani.Style.Add("display", "none")
            LblResultVani.Style.Add("display", "")
            TxtCountVani.Text = 0

            Session("oDatiRid") = Nothing
            GrdRiduzioni.Style.Add("display", "none")
            LblResultRid.Style.Add("display", "")

            Session("oDatiDet") = Nothing
            GrdDetassazioni.Style.Add("display", "none")
            LblResultDet.Style.Add("display", "")

            LoadUIVSCat()
            Abilita(True, "")
            'abilito il pulsante di salvataggio
            ReDim Preserve sListCmd(0)
            sListCmd(0) = "Salva"
            For x = 0 To sListCmd.Length - 1
                sScript += "parent.Comandi.document.getElementById('" & sListCmd(x).ToString() & "').disabled=" & True.ToString.ToLower & ";"
            Next
            'abilito il pulsante di salvataggio
            ReDim Preserve sListCmd(2)
            sListCmd(0) = "Modifica"
            sListCmd(1) = "Delete"
            sListCmd(2) = "Duplica"
            For x = 0 To sListCmd.Length - 1
                sScript += "parent.Comandi.document.getElementById('" & sListCmd(x).ToString() & "').disabled=" & False.ToString.ToLower & ";"
            Next
            'Page.RegisterStartupScript("", "<script language='javascript'>" & sScript & "</script>")
            RegisterScript(sScript, Me.GetType())
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestImmobili.CmdDuplicaUI_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    ''' <summary>
    ''' Pulsante per la cancellazione di un immobile
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Analisi eventi</em>
    ''' </revision>
    ''' </revisionHistory>
    Private Sub CmdDeleteUI_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdDeleteUI.Click
        Dim oListDettaglioTestata() As ObjDettaglioTestata
        Dim oListNewDettaglioTestata() As ObjDettaglioTestata = Nothing
        Dim oNewDettaglioTestata As ObjDettaglioTestata
        Dim nList, x As Integer
        Dim sScript As String = ""
        Dim oDetTestataCanc As New ObjDettaglioTestata

        Try
            'carico l'array originario
            If ConstSession.IsFromVariabile = "1" Then
                oListDettaglioTestata = Session("oListUITessera")
            Else
                oListDettaglioTestata = Session("oListImmobili")
            End If
            'se ho solo un immobile non gli permetto la cancellazione
            If oListDettaglioTestata.GetUpperBound(0) = 0 And ConstSession.IsFromVariabile <> "1" Then
                sScript = "GestAlert('a', 'warning', '', '', 'Cancellando questo immobile la dichiarazione risulta senza immobili.\nProcedere all\'eliminazione della dichiarazione o alla configurazione di nuovi immobili!');"
                If Request.Item("Provenienza") = Costanti.FormProvenienza.Tessera Then
                    sScript += BackToTessere()
                ElseIf Request.Item("Provenienza") = Costanti.FormProvenienza.Dichiarazione Then
                    sScript += BackToDichiarazione()
                End If
                RegisterScript(sScript, Me.GetType())
            Else
                'carico il nuovo oggetto senza la riga selezionata
                nList = -1
                If Request.Item("IdUniqueUI") <> -1 Then
                    For x = 0 To oListDettaglioTestata.GetUpperBound(0)
                        If oListDettaglioTestata(x).Id <> Request.Item("IdUniqueUI") Then
                            nList += 1
                            ReDim Preserve oListNewDettaglioTestata(nList)
                            oNewDettaglioTestata = oListDettaglioTestata(x)
                            oListNewDettaglioTestata(nList) = oNewDettaglioTestata
                        Else
                            '*** Fabiana 13062008
                            oDetTestataCanc = oListDettaglioTestata(x)
                            '*** /Fabiana
                        End If
                    Next
                Else
                    For x = 0 To oListDettaglioTestata.GetUpperBound(0)
                        If x <> Request.Item("IdList") Then
                            nList += 1
                            ReDim Preserve oListNewDettaglioTestata(nList)
                            oNewDettaglioTestata = oListDettaglioTestata(x)
                            oListNewDettaglioTestata(nList) = oNewDettaglioTestata
                        End If
                    Next
                End If
                'memorizzo l'oggetto nella sessione
                If ConstSession.IsFromVariabile = "1" Then
                    Session("oListUITessera") = oListNewDettaglioTestata
                Else
                    Session("oListImmobili") = oListNewDettaglioTestata
                End If
                Dim FunctionDettaglioTestata As New GestDettaglioTestata
                'aggiorno il database solo se ho già inserito nel db
                If Request.Item("IdUniqueUI") <> -1 Then 'If TxtIdTessera.Text <> "-1" And Request.Item("IdUniqueUI") <> -1 Then
                    If FunctionDettaglioTestata.DeleteDettaglioTestataVani(ConstSession.DBType, ConstSession.StringConnection, oDetTestataCanc) = 0 Then
                        Response.Redirect("../../PaginaErrore.aspx")
                        Exit Sub
                    End If
                    'aggiorno la pagina chiamante
                    If Request.Item("Provenienza") = Costanti.FormProvenienza.Tessera Then
                        sScript = BackToTessere()
                    ElseIf Request.Item("Provenienza") = Costanti.FormProvenienza.Dichiarazione Then
                        sScript = BackToDichiarazione()
                    End If
                    RegisterScript(sScript, Me.GetType())
                Else
                    'aggiorno la pagina chiamante
                    If Request.Item("Provenienza") = Costanti.FormProvenienza.Tessera Then
                        sScript = BackToTessere()
                    ElseIf Request.Item("Provenienza") = Costanti.FormProvenienza.Dichiarazione Then
                        sScript = BackToDichiarazione()
                    End If
                    RegisterScript(sScript, Me.GetType())
                End If
            End If
            'ripulisco tutti i dati di sessioni dati immobile
            Session("oDatiVani") = Nothing
            Session("oDatiRid") = Nothing
            Session("oDatiDet") = Nothing
            Session("oUITemp") = Nothing
            Session.Remove("AbilitaGestioneImmobili")
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestImmobili.CmdDeleteUI_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub CmdDeleteUI_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdDeleteUI.Click
    '    Dim oListDettaglioTestata() As ObjDettaglioTestata
    '    Dim oListNewDettaglioTestata() As ObjDettaglioTestata = Nothing
    '    Dim oNewDettaglioTestata As ObjDettaglioTestata
    '    Dim nList, x As Integer
    '    Dim sScript As String = ""
    '    Dim oDetTestataCanc As New ObjDettaglioTestata
    '    Dim FunctionTrovaDettTestata As New ClsDichiarazione

    '    Try
    '        'carico l'array originario
    '        If ConstSession.IsFromVariabile = "1" Then
    '            oListDettaglioTestata = Session("oListUITessera")
    '        Else
    '            oListDettaglioTestata = Session("oListImmobili")
    '        End If
    '        'se ho solo un immobile non gli permetto la cancellazione
    '        If oListDettaglioTestata.GetUpperBound(0) = 0 And ConstSession.IsFromVariabile <> "1" Then
    '            sScript = "GestAlert('a', 'warning', '', '', 'Cancellando questo immobile la dichiarazione risulta senza immobili.\nProcedere all\'eliminazione della dichiarazione o alla configurazione di nuovi immobili!');"
    '            If Request.Item("Provenienza") = Costanti.FormProvenienza.Tessera Then
    '                sScript += BackToTessere()
    '            ElseIf Request.Item("Provenienza") = Costanti.FormProvenienza.Dichiarazione Then
    '                sScript += BackToDichiarazione()
    '            End If
    '            'RegisterScript(Me.GetType(), "Del", "<script language='javascript'>" & sScript & "</script>")
    '            RegisterScript(sScript, Me.GetType())
    '        Else
    '            'carico il nuovo oggetto senza la riga selezionata
    '            nList = -1
    '            If Request.Item("IdUniqueUI") <> -1 Then
    '                For x = 0 To oListDettaglioTestata.GetUpperBound(0)
    '                    If oListDettaglioTestata(x).Id <> Request.Item("IdUniqueUI") Then
    '                        nList += 1
    '                        ReDim Preserve oListNewDettaglioTestata(nList)
    '                        oNewDettaglioTestata = oListDettaglioTestata(x)
    '                        oListNewDettaglioTestata(nList) = oNewDettaglioTestata
    '                    Else
    '                        '*** Fabiana 13062008
    '                        oDetTestataCanc = oListDettaglioTestata(x)
    '                        '*** /Fabiana
    '                    End If
    '                Next
    '            Else
    '                For x = 0 To oListDettaglioTestata.GetUpperBound(0)
    '                    If x <> Request.Item("IdList") Then
    '                        nList += 1
    '                        ReDim Preserve oListNewDettaglioTestata(nList)
    '                        oNewDettaglioTestata = oListDettaglioTestata(x)
    '                        oListNewDettaglioTestata(nList) = oNewDettaglioTestata
    '                    End If
    '                Next
    '            End If
    '            'memorizzo l'oggetto nella sessione
    '            If ConstSession.IsFromVariabile = "1" Then
    '                Session("oListUITessera") = oListNewDettaglioTestata
    '            Else
    '                Session("oListImmobili") = oListNewDettaglioTestata
    '            End If
    '            Dim FunctionDettaglioTestata As New GestDettaglioTestata
    '            'aggiorno il database solo se ho già inserito nel db
    '            If Request.Item("IdUniqueUI") <> -1 Then 'If TxtIdTessera.Text <> "-1" And Request.Item("IdUniqueUI") <> -1 Then
    '                If FunctionDettaglioTestata.DeleteDettaglioTestataVani(ConstSession.DBType, ConstSession.StringConnection, oDetTestataCanc) = 0 Then
    '                    'If FunctionDettaglioTestata.DeleteDettaglioTestata(Request.Item("IdUniqueUI"), Now) = 0 Then
    '                    Response.Redirect("../../PaginaErrore.aspx")
    '                    Exit Sub
    '                End If
    '                'aggiorno la pagina chiamante
    '                If Request.Item("Provenienza") = Costanti.FormProvenienza.Tessera Then
    '                    sScript = BackToTessere()
    '                ElseIf Request.Item("Provenienza") = Costanti.FormProvenienza.Dichiarazione Then
    '                    sScript = BackToDichiarazione()
    '                End If
    '                'RegisterScript(Me.GetType(), "Del", "<script language='javascript'>" & sScript & "</script>")
    '                RegisterScript(sScript, Me.GetType())
    '            Else
    '                'aggiorno la pagina chiamante
    '                If Request.Item("Provenienza") = Costanti.FormProvenienza.Tessera Then
    '                    sScript = BackToTessere()
    '                ElseIf Request.Item("Provenienza") = Costanti.FormProvenienza.Dichiarazione Then
    '                    sScript = BackToDichiarazione()
    '                End If
    '                'RegisterScript(Me.GetType(), "Del", "<script language='javascript'>" & sScript & "</script>")
    '                RegisterScript(sScript, Me.GetType())
    '            End If
    '        End If
    '        'ripulisco tutti i dati di sessioni dati immobile
    '        Session("oDatiVani") = Nothing
    '        Session("oDatiRid") = Nothing
    '        Session("oDatiDet") = Nothing
    '        Session("oUITemp") = Nothing
    '        Session.Remove("AbilitaGestioneImmobili")
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestImmobili.CmdDeleteUI_Click.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdSalvaVani_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdSalvaVani.Click
        Try
            'controllo se devo caricare la griglia dei dati vani
            If Not Session("oDatiVani") Is Nothing Then
                GrdVani.Style.Add("display", "")
                GrdVani.SelectedIndex = -1
                GrdVani.DataSource = Session("oDatiVani")
                GrdVani.DataBind()
                LblResultVani.Style.Add("display", "none")
                TxtCountVani.Text = 1
            Else
                GrdVani.Style.Add("display", "none")
                LblResultVani.Style.Add("display", "")
                TxtCountVani.Text = 0
            End If
            'controllo se devo caricare la griglia delle riduzioni
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
            'controllo se devo caricare la griglia delle Detassazioni
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
            If Not Session("AbilitaGestioneImmobili") Is Nothing Then
                Abilita(Session("AbilitaGestioneImmobili"), "")
            End If
            CmdModUI_Click(Nothing, Nothing)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestImmobili.CmdSalvaVani_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdRibaltaUIAnater_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdRibaltaUIAnater.Click
        'controllo se devo caricare la griglia dei dati vani
        Try
            If Not Session("oDatiVani") Is Nothing Then
                GrdVani.Style.Add("display", "")
                GrdVani.SelectedIndex = -1
                GrdVani.DataSource = Session("oDatiVani")
                GrdVani.DataBind()
                LblResultVani.Style.Add("display", "none")
                TxtCountVani.Text = 1
            Else
                GrdVani.Style.Add("display", "none")
                LblResultVani.Style.Add("display", "")
                TxtCountVani.Text = 0
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestImmobili.CmdRibaltaUIAnater_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    '*** 20140923 - GIS ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdGIS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdGIS.Click
        Dim CodeGIS As String
        Dim sScript As String
        Dim fncGIS As New RemotingInterfaceAnater.GIS
        Dim listRifCat As New Generic.List(Of Anater.Oggetti.RicercaUnitaImmobiliareAnater)
        Dim myRifCat As New Anater.Oggetti.RicercaUnitaImmobiliareAnater
        Try
            If TxtFoglio.Text <> "" Then
                myRifCat.Foglio = TxtFoglio.Text
                myRifCat.Mappale = TxtNumero.Text
                myRifCat.Subalterno = TxtSubalterno.Text
                myRifCat.CodiceRicerca = ConstSession.Belfiore
                listRifCat.Add(myRifCat)
                CodeGIS = fncGIS.getGIS(ConstSession.UrlWSGIS, listRifCat.ToArray())
                If Not CodeGIS Is Nothing Then
                    sScript = "window.open('" & ConstSession.UrlWebGIS & CodeGIS & "','wdwGIS')"
                    'RegisterScript( sScript,Me.GetType)
                    RegisterScript(sScript, Me.GetType())
                Else
                    sScript = "GestAlert('a', 'danger', '', '', 'Errore in interrogazione Cartografia!');"
                    'RegisterScript( sScript,Me.GetType)
                    RegisterScript(sScript, Me.GetType())
                End If
            Else
                sScript = "GestAlert('a', 'warning', '', '', 'Per accedere alla Cartografia avere almeno un foglio!');"
                'RegisterScript( sScript,Me.GetType)
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestImmobili.CmdGis_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ibNewICI_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibNewICI.Click
        Dim retval As Boolean
        Dim idTestata As Integer = 0
        Dim idImmobile As Integer = 0
        Dim FncModificheTributarie As New Utility.ModificheTributarie
        Dim fncICI As New Utility.DichManagerICI(ConstSession.DBType, ConstSession.StringConnection)

        Try
            Dim RigaTestata As New Utility.DichManagerICI.TestataRow

            RigaTestata.ID = 0
            RigaTestata.Ente = ConstSession.IdEnte
            RigaTestata.NumeroDichiarazione = -1
            RigaTestata.Bonificato = False
            RigaTestata.Annullato = False
            RigaTestata.DataInizioValidità = DateTime.Now
            RigaTestata.DataFineValidità = DateTime.MinValue
            RigaTestata.Operatore = ConstSession.UserName
            RigaTestata.AnnoDichiarazione = Year(TxtDataInizio.Text)
            RigaTestata.IDContribuente = hdIdContribuente.Value
            RigaTestata.IDDenunciante = hdIdContribuente.Value
            RigaTestata.IDProvenienza = Utility.DichManagerICI.Dichiarazione_FITTIZIA
            fncICI.SetTestata(Utility.Costanti.AZIONE_NEW, RigaTestata, idTestata)
            If idTestata > 0 Then
                Dim rigaOggetti As New Utility.DichManagerICI.OggettiRow

                rigaOggetti.Annullato = False
                rigaOggetti.Bonificato = False
                rigaOggetti.CodUI = -1
                rigaOggetti.NumeroEcografico = -1
                rigaOggetti.PartitaCatastale = -1
                rigaOggetti.ValoreImmobile = -1
                rigaOggetti.IDImmobilePertinente = -1
                rigaOggetti.IDValuta = 1
                rigaOggetti.IdTestata = idTestata
                rigaOggetti.CodComune = CInt(ConstSession.IdEnte)
                rigaOggetti.Comune = ConstSession.DescrizioneEnte
                rigaOggetti.Ente = ConstSession.IdEnte
                rigaOggetti.CodVia = TxtCodVia.Text
                rigaOggetti.Via = TxtVia.Text
                If (TxtCivico.Text <> "") Then
                    rigaOggetti.NumeroCivico = CInt(TxtCivico.Text)
                Else
                    rigaOggetti.NumeroCivico = -1
                End If
                rigaOggetti.EspCivico = TxtEsponente.Text
                rigaOggetti.Scala = TxtScala.Text
                rigaOggetti.Interno = TxtInterno.Text
                rigaOggetti.Foglio = TxtFoglio.Text
                rigaOggetti.Numero = TxtNumero.Text
                If (TxtSubalterno.Text <> "") Then
                    rigaOggetti.Subalterno = CInt(TxtSubalterno.Text)
                Else
                    rigaOggetti.Subalterno = -1
                End If
                rigaOggetti.DataInizio = Convert.ToDateTime(TxtDataInizio.Text)
                If (TxtDataFine.Text.CompareTo("") <> 0) Then
                    rigaOggetti.DataFine = Convert.ToDateTime(TxtDataFine.Text)
                Else
                    rigaOggetti.DataFine = DateTime.MaxValue.Date
                End If
                rigaOggetti.DataFineValidità = DateTime.MinValue
                rigaOggetti.DataInizioValidità = DateTime.Now
                rigaOggetti.DataUltimaModifica = DateTime.Now
                rigaOggetti.Operatore = ConstSession.UserName
                rigaOggetti.Sezione = TxtSezione.Text

                retval = fncICI.SetOggetti(Utility.Costanti.AZIONE_NEW, rigaOggetti, idTestata, idImmobile)
                If idImmobile > 0 Then
                    '*** 20130923 - gestione modifiche tributarie ***
                    Dim idModTrib As Integer = 0
                    idModTrib = CType(Utility.ModificheTributarie.ModificheTributarieCausali.NuovaDichiarazione, Integer)
                    If (FncModificheTributarie.SetModificheTributarie(ConstSession.StringConnectionOPENgov, CType(Utility.ModificheTributarie.DBOperation.Insert, Integer), 0, ConstSession.IdEnte, "8852", idModTrib, rigaOggetti.Foglio, rigaOggetti.Numero, rigaOggetti.Subalterno.ToString, DateTime.Now, ConstSession.UserName, rigaOggetti.ID, DateTime.MaxValue) = False) Then
                        Log.Debug("Errore in SetModificheTributarie::@DBOperation=0::@IDVARIAZIONE=0::@IDENTE=" & ConstSession.IdEnte _
                                            & "::@TRIBUTO=" & ConstSession.CodTributo _
                                            & "::@IDCAUSALE=" & idModTrib.ToString _
                                            & "::@FOGLIO=" & rigaOggetti.Foglio _
                                            & "::@NUMERO=" & rigaOggetti.Numero _
                                            & "::@SUBALTERNO=" & rigaOggetti.Subalterno.ToString _
                                            & "::@DATAVARIAZIONE=" & DateTime.Now.ToString _
                                            & "::@OPERATORE=" & ConstSession.UserName _
                                            & "::@IDOGGETTOTRIBUTI=" & rigaOggetti.ID.ToString _
                                            & "::@DATATRATTATO=" & DateTime.MaxValue.ToString)
                    End If
                    '*** ***
                    Dim rigaDettaglioTestata As New Utility.DichManagerICI.DettaglioTestataRow
                    rigaDettaglioTestata.ID = -1
                    'rigaDettaglioTestata.AbitazionePrincipale = Integer.Parse(ddlAbitazionePrincipale.SelectedValue)
                    'rigaDettaglioTestata.NumeroModello = txtNumModello.Text
                    'rigaDettaglioTestata.NumeroOrdine = txtNumOrdine.Text
                    'rigaDettaglioTestata.Riduzione = Integer.Parse(ddlRiduzione.SelectedValue)
                    'rigaDettaglioTestata.Possesso = Integer.Parse(ddlPossesso.SelectedValue)
                    'rigaDettaglioTestata.EsclusioneEsenzione = Integer.Parse(ddlEsclusoEsente.SelectedValue)
                    'rigaDettaglioTestata.ColtivatoreDiretto = chkcoltivatori.Checked
                    'If (txtnumfigli.Text <> String.Empty) Then
                    '    rigaDettaglioTestata.NumeroFigli = Integer.Parse(txtnumfigli.Text)
                    '    If (rigaDettaglioTestata.NumeroFigli > 0) Then
                    '        Dim ListCaricoFigli() As CaricoFigliRow = New CaricoFigliRow((rigaDettaglioTestata.NumeroFigli) - 1) {}
                    '        Dim x As Integer = 0
                    '        For Each oItemGrid As GridViewRow In GrdCaricoFigli.Items
                    '            Dim oCaricoFiglio As CaricoFigliRow = New CaricoFigliRow
                    '            oCaricoFiglio.IdDettaglioTestata = riga.ID
                    '            oCaricoFiglio.nFiglio = (x + 1)
                    '            'oCaricoFiglio.Percentuale=int.Parse(((DropDownList)oItemGrid.FindControl("DdlCaricoFigli")).SelectedValue.ToString());
                    '            oCaricoFiglio.Percentuale = Integer.Parse(CType(oItemGrid.FindControl("TxtPercentCarico"), TextBox).Text.ToString)
                    '            ListCaricoFigli(x) = oCaricoFiglio
                    '            x = (x + 1)
                    '        Next
                    '        riga.ListCaricoFigli = ListCaricoFigli
                    '    End If
                    'End If
                    rigaDettaglioTestata.Annullato = False
                    rigaDettaglioTestata.Bonificato = False
                    rigaDettaglioTestata.Contitolare = False
                    rigaDettaglioTestata.DataFineValidità = DateTime.MinValue
                    rigaDettaglioTestata.DataInizioValidità = DateTime.Now
                    rigaDettaglioTestata.Ente = ConstSession.IdEnte
                    rigaDettaglioTestata.IdOggetto = idImmobile
                    rigaDettaglioTestata.IdSoggetto = hdIdContribuente.Value
                    rigaDettaglioTestata.IdTestata = idTestata
                    rigaDettaglioTestata.Operatore = ConstSession.UserName
                    rigaDettaglioTestata.PercPossesso = -1
                    rigaDettaglioTestata.TipoUtilizzo = 1
                    rigaDettaglioTestata.TipoPossesso = 1
                    rigaDettaglioTestata.ImpDetrazAbitazPrincipale = -1
                    rigaDettaglioTestata.MesiEsclusioneEsenzione = -1
                    rigaDettaglioTestata.MesiPossesso = -1
                    rigaDettaglioTestata.MesiRiduzione = -1
                    rigaDettaglioTestata.AbitazionePrincipaleAttuale = 0
                    rigaDettaglioTestata.NumeroUtilizzatori = 0

                    Dim idDettaglio As Integer
                    retval = fncICI.SetDettaglioTestataCompleta(rigaDettaglioTestata, idDettaglio)
                    If idDettaglio <= 0 Then
                        Dim UIIci As New Utility.DichManagerICI.OggettiRow
                        UIIci.ID = idImmobile
                        fncICI.SetOggetti(Utility.Costanti.AZIONE_DELETE, UIIci, idTestata, idImmobile)
                        Dim DichIci As New Utility.DichManagerICI.TestataRow
                        DichIci.ID = idTestata
                        fncICI.SetTestata(Utility.Costanti.AZIONE_DELETE, DichIci, idTestata)
                        Log.Debug("Errore in InsertDettaglioTestata::")
                    End If
                Else
                    Log.Debug("Errore in InsertOggetto::")
                    Dim DichIci As New Utility.DichManagerICI.TestataRow
                    DichIci.ID = idTestata
                    fncICI.SetTestata(Utility.Costanti.AZIONE_DELETE, DichIci, idTestata)
                End If
            Else
                Log.Debug("Errore InsertTestata::")
            End If
            If TxtFoglio.Text <> "" Then
                Dim FncDatiICI As New ClsDichiarazione
                Dim tInizio, tFine As DateTime
                tInizio = DateTime.MaxValue : tFine = DateTime.MaxValue
                If TxtDataInizio.Text <> "" Then
                    tInizio = TxtDataInizio.Text
                End If
                If TxtDataFine.Text <> "" Then
                    tFine = TxtDataFine.Text
                End If
                FncDatiICI.LoadDatiICI(ConstSession.StringConnection, ConstSession.IdEnte, TxtFoglio.Text, TxtNumero.Text, TxtSubalterno.Text, tInizio, tFine, GrdICI, ibNewICI)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestImmobili.ibNewICI_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    'Private Sub LoadPageCombo(ByVal WFSessione As OPENUtility.CreateSessione)
    '    dim sSQL as string
    '    Dim oLoadCombo As New generalClass.generalFunction
    '    Dim FncAE As New OPENgov_AgenziaEntrate.OPENgov_AgenziaEntrate.General
    '    Dim dvDati As New DataView

    '    Try
    '        'carico gli stati occupazione
    '        sSQL = "SELECT DESCRIZIONE, IDSTATOOCCUPAZIONE"
    '        sSQL += " FROM TBLSTATOOCCUPAZIONE"
    '        sSQL += " ORDER BY DESCRIZIONE"
    '        oLoadCombo.LoadComboGenerale(DdlStatoOccupazione, sSQL)
    '        '*** 20130228 - gestione categoria Ateco per TARES ***
    '        sSQL = "SELECT CODICECATEGORIA+' '+DEFINIZIONE, IDCATEGORIAATECO"
    '        sSQL += " FROM V_CATEGORIE_ATECO"
    '        sSQL += " WHERE ((fk_IdTypeAteco=0) OR (fk_IdTypeAteco=" & ConstSession.IdTypeAteco & "))"
    '        sSQL += " AND (ENTE='" & ConstSession.IdEnte & "')"
    '        sSQL += " ORDER BY RIGHT(REPLICATE('0',10)+CAST(IDCATEGORIAATECO AS VARCHAR),10)"
    '        oLoadCombo.LoadComboGenerale(ddlcattares, sSQL)
    '        '*** ***

    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConfigurationManager.AppSettings("OPENGOVG").ToString())
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If
    '        'popolo la combo richiamando la funzione LoadComboDati("TIT_OCCUPAZIONE", sIdEnte, DdlTitOccupazione);
    '        dvDati = FncAE.LoadComboDati("TIT_OCCUPAZIONE", "0434", WFSessione)
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
    '        dvDati = FncAE.LoadComboDati("ASSENZA_DATI_CAT", "0434", WFSessione)
    '        oLoadCombo.loadCombo(DdlAssenzaDatiCat, dvDati)
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestImmobili.LoadPageCombo.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub LoadPageCombo()
        Dim sSQL As String
        Dim oLoadCombo As New generalClass.generalFunction
        Dim FncAE As New OPENgov_AgenziaEntrate.OPENgov_AgenziaEntrate.General
        Dim dvDati As New DataView

        Try
            'carico gli stati occupazione
            sSQL = "SELECT DESCRIZIONE, IDSTATOOCCUPAZIONE"
            sSQL += " FROM TBLSTATOOCCUPAZIONE"
            sSQL += " ORDER BY DESCRIZIONE"
            oLoadCombo.LoadComboGenerale(DdlStatoOccupazione, sSQL, ConstSession.StringConnection, True, Costanti.TipoDefaultCmb.STRINGA)
            '*** 20130228 - gestione categoria Ateco per TARES ***
            sSQL = "SELECT CODICECATEGORIA+' '+DEFINIZIONE, IDCATEGORIAATECO"
            sSQL += " FROM V_CATEGORIE_ATECO"
            sSQL += " WHERE 1=1"
            'sSQL += " AND ((FK_IDTYPEATECO=0) OR (FK_IDTYPEATECO=" & ConstSession.IdTypeAteco & "))"
            sSQL += " And (ENTE='" & ConstSession.IdEnte & "')"
            sSQL += " ORDER BY RIGHT(REPLICATE('0',10)+CAST(IDCATEGORIAATECO AS VARCHAR),10)"
            oLoadCombo.LoadComboGenerale(DDlCatTARES, sSQL, ConstSession.StringConnection, True, Costanti.TipoDefaultCmb.STRINGA)
            '*** ***

            '*** 201511 - tolto RIBESFRAMEWORK ***
            'Dim WFSessione As New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConfigurationManager.AppSettings("OPENGOVG").ToString())
            'If Not WFSessione.CreaSessione(ConstSession.UserName, sSQL) Then
            '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
            'End If
            ''popolo la combo richiamando la funzione LoadComboDati("TIT_OCCUPAZIONE", sIdEnte, DdlTitOccupazione);
            'dvDati = FncAE.LoadComboDati("TIT_OCCUPAZIONE", utility.Costanti.TRIBUTOTARSU, WFSessione)
            'oLoadCombo.loadCombo(DdlTitOccupaz, dvDati)
            ''popolo la combo richiamando la funzione LoadComboDati("NAT_OCCUPAZIONE", sIdEnte, DdlNatOccupazione);
            'dvDati = FncAE.LoadComboDati("NAT_OCCUPAZIONE", "", WFSessione)
            'oLoadCombo.loadCombo(DdlNatOccupaz, dvDati)
            ''popolo la combo richiamando la funzione LoadComboDati("DEST_USO", sIdEnte, DdlDestUso);
            'dvDati = FncAE.LoadComboDati("DEST_USO", "", WFSessione)
            'oLoadCombo.loadCombo(DdlDestUso, dvDati)
            ''popolo la combo richiamando la funzione LoadComboDati("TIPO_UNITA", sIdEnte, DdlTipoUnita);
            'dvDati = FncAE.LoadComboDati("TIPO_UNITA", "", WFSessione)
            'oLoadCombo.loadCombo(DdlTipoUnita, dvDati)
            ''per DdlTipoUnita seleziono il valore F e la disabilito;
            'DdlTipoUnita.SelectedValue = "F" : DdlTipoUnita.Enabled = False
            ''popolo la combo richiamando la funzione LoadComboDati("TIPO_PARTICELLA", sIdEnte, DdlTipoParticella);
            'dvDati = FncAE.LoadComboDati("TIPO_PARTICELLA", "", WFSessione)
            'oLoadCombo.loadCombo(DdlTipoParticella, dvDati)
            ''popolo la combo richiamando la funzione LoadComboDati("TIPO_PARTICELLA", sIdEnte, DdlTipoParticella);
            'dvDati = FncAE.LoadComboDati("ASSENZA_DATI_CAT", utility.Costanti.TRIBUTOTARSU, WFSessione)
            'oLoadCombo.loadCombo(DdlAssenzaDatiCat, dvDati)
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
            'popolo la combo richiamando la funzione LoadComboDati("TIPO_PARTICELLA", sIdEnte, DdlTipoParticella);
            dvDati = FncAE.LoadComboDati("TIPO_PARTICELLA", "", ConstSession.StringConnectionOPENgov)
            oLoadCombo.loadCombo(DdlTipoParticella, dvDati, True, Costanti.TipoDefaultCmb.STRINGA)
            'popolo la combo richiamando la funzione LoadComboDati("TIPO_PARTICELLA", sIdEnte, DdlTipoParticella);
            dvDati = FncAE.LoadComboDati("ASSENZA_DATI_CAT", Utility.Costanti.TRIBUTO_TARSU, ConstSession.StringConnectionOPENgov)
            oLoadCombo.loadCombo(DdlAssenzaDatiCat, dvDati, True, Costanti.TipoDefaultCmb.STRINGA)
            '*** ***
            'per DdlTipoParticella seleziono il valore E e la disabilito;
            DdlTipoParticella.SelectedValue = "E" : DdlTipoParticella.Enabled = False
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestImmobili.LoadPageCombo.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub LoadPageCombo()
    '    dim sSQL as string
    '    Dim oLoadCombo As New generalClass.generalFunction
    '    Dim FncAE As New OPENgov_AgenziaEntrate.OPENgov_AgenziaEntrate.General
    '    Dim dvDati As New DataView

    '    Try
    '        'carico gli stati occupazione
    '        sSQL = "SELECT DESCRIZIONE, IDSTATOOCCUPAZIONE"
    '        sSQL += " FROM TBLSTATOOCCUPAZIONE"
    '        sSQL += " ORDER BY DESCRIZIONE"
    '        oLoadCombo.LoadComboGenerale(DdlStatoOccupazione, sSQL, ConstSession.StringConnection)
    '        '*** 20130228 - gestione categoria Ateco per TARES ***
    '        sSQL = "SELECT CODICECATEGORIA+' '+DEFINIZIONE, IDCATEGORIAATECO"
    '        sSQL += " FROM V_CATEGORIE_ATECO"
    '        sSQL += " WHERE ((fk_IdTypeAteco=0) OR (fk_IdTypeAteco=" & ConstSession.IdTypeAteco & "))"
    '        sSQL += " AND (ENTE='" & ConstSession.IdEnte & "')"
    '        sSQL += " ORDER BY RIGHT(REPLICATE('0',10)+CAST(IDCATEGORIAATECO AS VARCHAR),10)"
    '        oLoadCombo.LoadComboGenerale(DDlCatTARES, sSQL, ConstSession.StringConnection)
    '        '*** ***

    '        'popolo la combo richiamando la funzione LoadComboDati("TIT_OCCUPAZIONE", sIdEnte, DdlTitOccupazione);
    '        dvDati = FncAE.LoadComboDati("TIT_OCCUPAZIONE", utility.Costanti.TRIBUTOTARSU, ConstSession.StringConnectionOPENgov)
    '        oLoadCombo.loadCombo(DdlTitOccupaz, dvDati)
    '        'popolo la combo richiamando la funzione LoadComboDati("NAT_OCCUPAZIONE", sIdEnte, DdlNatOccupazione);
    '        dvDati = FncAE.LoadComboDati("NAT_OCCUPAZIONE", "", ConstSession.StringConnectionOPENgov)
    '        oLoadCombo.loadCombo(DdlNatOccupaz, dvDati)
    '        'popolo la combo richiamando la funzione LoadComboDati("DEST_USO", sIdEnte, DdlDestUso);
    '        dvDati = FncAE.LoadComboDati("DEST_USO", "", ConstSession.StringConnectionOPENgov)
    '        oLoadCombo.loadCombo(DdlDestUso, dvDati)
    '        'popolo la combo richiamando la funzione LoadComboDati("TIPO_UNITA", sIdEnte, DdlTipoUnita);
    '        dvDati = FncAE.LoadComboDati("TIPO_UNITA", "", ConstSession.StringConnectionOPENgov)
    '        oLoadCombo.loadCombo(DdlTipoUnita, dvDati)
    '        'per DdlTipoUnita seleziono il valore F e la disabilito;
    '        DdlTipoUnita.SelectedValue = "F" : DdlTipoUnita.Enabled = False
    '        'popolo la combo richiamando la funzione LoadComboDati("TIPO_PARTICELLA", sIdEnte, DdlTipoParticella);
    '        dvDati = FncAE.LoadComboDati("TIPO_PARTICELLA", "", ConstSession.StringConnectionOPENgov)
    '        oLoadCombo.loadCombo(DdlTipoParticella, dvDati)
    '        'per DdlTipoParticella seleziono il valore E e la disabilito;
    '        DdlTipoParticella.SelectedValue = "E" : DdlTipoParticella.Enabled = False
    '        'popolo la combo richiamando la funzione LoadComboDati("TIPO_PARTICELLA", sIdEnte, DdlTipoParticella);
    '        dvDati = FncAE.LoadComboDati("ASSENZA_DATI_CAT", utility.Costanti.TRIBUTOTARSU, ConstSession.StringConnectionOPENgov)
    '        oLoadCombo.loadCombo(DdlAssenzaDatiCat, dvDati)
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestImmobili.LoadPageCombo.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="bTypeAbilita"></param>
    ''' <param name="sProvenienza"></param>
    Private Sub Abilita(ByVal bTypeAbilita As Boolean, ByVal sProvenienza As String)
        Try
            'disabilito i dati
            TxtCivico.Enabled = bTypeAbilita : TxtEsponente.Enabled = bTypeAbilita : TxtInterno.Enabled = bTypeAbilita : TxtScala.Enabled = bTypeAbilita
            TxtDataInizio.Enabled = bTypeAbilita : TxtDataFine.Enabled = bTypeAbilita : DdlStatoOccupazione.Enabled = bTypeAbilita : TxtNComponenti.Enabled = bTypeAbilita
            ChkIsGiornaliera.Enabled = bTypeAbilita : TxtGGTarsu.Enabled = bTypeAbilita
            TxtFoglio.Enabled = bTypeAbilita : TxtNumero.Enabled = bTypeAbilita : TxtSubalterno.Enabled = bTypeAbilita
            TxtPropietario.Enabled = bTypeAbilita : TxtOccupantePrec.Enabled = bTypeAbilita
            LnkNewRid.Enabled = bTypeAbilita : LnkDelRid.Enabled = bTypeAbilita
            LnkNewDet.Enabled = bTypeAbilita : LnkDelDet.Enabled = bTypeAbilita
            LnkOpenStradario.Enabled = bTypeAbilita : LnkNewVani.Enabled = bTypeAbilita : LnkNewVaniAnater.Enabled = bTypeAbilita
            TxtNoteUI.Enabled = bTypeAbilita
            ibNewICI.Enabled = bTypeAbilita
            '***Agenzia Entrate***
            TxtSezione.Enabled = bTypeAbilita
            TxtEstParticella.Enabled = bTypeAbilita
            DdlTitOccupaz.Enabled = bTypeAbilita
            DdlNatOccupaz.Enabled = bTypeAbilita
            DdlDestUso.Enabled = bTypeAbilita
            DdlAssenzaDatiCat.Enabled = bTypeAbilita
            DdlTipoUnita.Enabled = bTypeAbilita
            '*********************
            '*** 20130201 - gestione mq da catasto per TARES ***
            TxtMQCatasto.Enabled = bTypeAbilita
            '*** ***
            '*** 20130325 - gestione mq tassabili per TARES ***
            TxtMQTassabili.Enabled = False '*** non è editabile
            '*** ***
            '*** 20130228 - gestione categoria Ateco per TARES ***
            DDlCatTARES.Enabled = bTypeAbilita
            ChkForzaPV.Enabled = bTypeAbilita
            TxtNComponentiPV.Enabled = bTypeAbilita
            '*** ***
            'BD 16/07/2021 abilita disabilita il txtimportofissorid
            TxtRidImp.Enabled = bTypeAbilita
            'BD 16/07/2021 abilita disabilita il txtimportofissorid

            'se passo da mofica a variazione devo disabilitare il pulsante di modifica/cancellazione e abilitare il pulsante di salvataggio
            Session("AbilitaGestioneImmobili") = bTypeAbilita
            'If bTypeAbilita = True Then
            '    If Request.Item("AzioneProv") = Utility.Costanti.AZIONE_NEW Then
            '        ReDim Preserve oListCmd(1)
            '        oListCmd(0) = "Modifica"
            '        oListCmd(1) = "Delete"
            '        For x = 0 To oListCmd.Length - 1
            '            sScript += "document.getElementById('" & oListCmd(x).ToString() & "').style.display='none';" '.disabled=" & False.ToString.ToLower & ";"
            '        Next
            '        ReDim Preserve oListCmd(2)
            '        oListCmd(2) = "Salva"
            '        For x = 2 To oListCmd.Length - 1
            '            sScript += "document.getElementById('" & oListCmd(x).ToString() & "').style.display='';" '.disabled=" & True.ToString.ToLower & ";"
            '        Next
            '    Else
            '        ReDim Preserve oListCmd(1)
            '        oListCmd(0) = "Modifica"
            '        oListCmd(1) = "Delete"
            '        For x = 0 To oListCmd.Length - 1
            '            sScript += "document.getElementById('" & oListCmd(x).ToString() & "').style.display='';" '.disabled=" & True.ToString.ToLower & ";"
            '        Next
            '        ReDim Preserve oListCmd(2)
            '        oListCmd(2) = "Salva"
            '        For x = 2 To oListCmd.Length - 1
            '            sScript += "document.getElementById('" & oListCmd(x).ToString() & "').style.display='none';" '.disabled=" & False.ToString.ToLower & ";"
            '        Next
            '    End If
            '    'RegisterScript( sScript,Me.GetType)
            '    RegisterScript(sScript, Me.GetType())
            'End If
            If bTypeAbilita = True Then
                bTypeAbilita = False
            Else
                bTypeAbilita = True
            End If
            '***se arrivo da gestione dati mancanti abilito solo i dati Agenzia Entrate***
            If sProvenienza = Costanti.FormProvenienza.DatiAE.ToString Then
                TxtFoglio.Enabled = True : TxtNumero.Enabled = True : TxtSubalterno.Enabled = True
                TxtSezione.Enabled = True : TxtEstParticella.Enabled = True
                DdlTitOccupaz.Enabled = True : DdlNatOccupaz.Enabled = True
                DdlDestUso.Enabled = True : DdlAssenzaDatiCat.Enabled = True
            End If
            '*********************



        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestImmobili.Abilita.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    'Private Function SaveFromGestDich(ByVal oMyDettaglioTestata As ObjDettaglioTestata, ByVal WFSessione As OPENUtility.CreateSessione, ByRef sScript As String) As Boolean
    '    Dim oMyDettaglioTestataOrg As New ObjDettaglioTestata
    '    Dim oListDettaglioTestata() As ObjDettaglioTestata
    '    Dim nList As Integer = -1
    '    Dim FunctionDettTestata As New GestDettaglioTestata
    '    Dim sNewMyDettaglioTestata, sNewMyVarieDettaglioTestata As String
    '    Dim FunctionDich As New ClsDichiarazione
    '    Dim oListUIGrd() As ObjDettaglioTestata
    '    Dim nListUIGrd As Integer = -1
    '    Dim FncRidEse As New GestRidEse

    '    Try
    '        'inizializzo la connessione
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If
    '        Log.Debug("GestImmobili::SaveFromGestDich::controllo se ho già degli immobili caricati")
    '        'controllo se ho già degli immobili caricati
    '        If Not Session("oListUITessera") Is Nothing Then
    '            'carico l'array originario
    '            oListDettaglioTestata = Session("oListUITessera")
    '            'aggiungo una riga
    '            nList = oListDettaglioTestata.GetUpperBound(0)
    '        End If
    '        'per la visualizzazione in griglia come sopra
    '        'controllo se ho già degli immobili caricati
    '        If Not Session("oListImmobili") Is Nothing Then
    '            'carico l'array originario
    '            oListUIGrd = Session("oListImmobili")
    '            'aggiungo una riga
    '            nListUIGrd = oListUIGrd.GetUpperBound(0)
    '        End If

    '        'ho la data di fine
    '        If oMyDettaglioTestata.tDataFine <> Date.MinValue Then
    '            'controllo se sono in modifica di un immobile
    '            If Request.Item("IdUniqueUI") <> "-1" Then
    '                'carico il singolo immobile selezionato
    '                For nList = 0 To oListDettaglioTestata.GetUpperBound(0)
    '                    If oListDettaglioTestata(nList).Id = Request.Item("IdUniqueUI") Then
    '                        oMyDettaglioTestataOrg = oListDettaglioTestata(nList)
    '                        oMyDettaglioTestataOrg.tDataFine = oMyDettaglioTestata.tDataFine
    '                        oMyDettaglioTestata.nMQAnater = oMyDettaglioTestataOrg.nMQAnater
    '                        oListDettaglioTestata(nList) = oMyDettaglioTestata
    '                        Exit For
    '                    End If
    '                Next
    '                'per la visualizzazione in griglia come sopra
    '                'aggiorno la riga
    '                For nListUIGrd = 0 To oListUIGrd.GetUpperBound(0)
    '                    If oListUIGrd(nListUIGrd).Id = Request.Item("IdUniqueUI") Then
    '                        'carico l'array con i dati della videata
    '                        oListUIGrd(nListUIGrd) = oMyDettaglioTestata
    '                        Exit For
    '                    End If
    '                Next

    '            End If
    '            'chiedo se si vuole ribaltare in nuovo immobile
    '            If TxtIsRibalta.Text = "1" Then
    '                'carico l'array originale
    '                oListDettaglioTestata(nList) = oMyDettaglioTestataOrg
    '                oMyDettaglioTestata.Id = -1
    '                oMyDettaglioTestata.IdDettaglioTestata = -1
    '                'la data inizio deve essere data fine + 1
    '                oMyDettaglioTestata.tDataInizio = DateAdd(DateInterval.Day, 1, oMyDettaglioTestataOrg.tDataFine)
    '                'data fine in nuovo oggetto deve essere vuota
    '                oMyDettaglioTestata.tDataFine = Nothing
    '                'id padre in nuovo oggetto deve essere id di oggetto originale
    '                If Not oMyDettaglioTestataOrg Is Nothing Then
    '                    oMyDettaglioTestata.IdPadre = oMyDettaglioTestataOrg.Id
    '                End If
    '                'controllo se ho lo stesso immobile con il periodo coerente
    '                If FunctionDettTestata.CheckPeriodi(oListDettaglioTestata, oMyDettaglioTestata) < 1 Then
    '                    'sScript = "<script language='javascript'>alert('Attenzione! Periodi non coerenti.\nImpossibile proseguire!')</script>"
    '                    sScript = "alert('Attenzione! Periodi non coerenti.');"
    '                    'Return False
    '                End If
    '                'aggiungo una riga
    '                nList = oListDettaglioTestata.GetUpperBound(0)
    '                nList += 1
    '                'dimensiono l'array
    '                ReDim Preserve oListDettaglioTestata(nList)
    '                'carico l'array con i dati della videata
    '                oListDettaglioTestata(nList) = oMyDettaglioTestata
    '                'sono in modifica di un immobile già inserito nel db
    '                If TxtIdTessera.Text <> "-1" Then
    '                    'storicizzo l'immobile originale
    '                    If FunctionDettTestata.UpdateDettaglioTestata(WFSessione, oMyDettaglioTestataOrg) = 0 Then
    '                        Return False
    '                    End If
    '                    'aggiungo il nuovo immobile con relativi vani
    '                    Dim oSingleDettTestata(0) As ObjDettaglioTestata
    '                    oSingleDettTestata(0) = oMyDettaglioTestata
    '                    If FunctionDettTestata.SetDettaglioTestataCompleta(oSingleDettTestata, TxtIdTessera.Text, WFSessione) = 0 Then
    '                        Return False
    '                    End If
    '                End If
    '                'aggiungo una riga
    '                nListUIGrd += 1
    '                'dimensiono l'array
    '                ReDim Preserve oListUIGrd(nListUIGrd)
    '                'carico l'array con i dati della videata
    '                oListUIGrd(nListUIGrd) = oMyDettaglioTestata
    '            Else
    '                'controllo se ho lo stesso immobile con il periodo coerente
    '                If FunctionDettTestata.CheckPeriodi(oListDettaglioTestata, oMyDettaglioTestata) < 1 Then
    '                    'sScript = "<script language='javascript'>alert('Attenzione! Periodi non coerenti.\nImpossibile proseguire!')</script>"
    '                    sScript = "alert('Attenzione! Periodi non coerenti.');"
    '                    'Return False
    '                End If
    '                ''aggiungo una riga
    '                'nList = oListDettaglioTestata.GetUpperBound(0)
    '                If Request.Item("AzioneProv") = Costanti.AZIONE_NEW Then
    '                    nList += 1
    '                    ''dimensiono l'array
    '                    'ReDim Preserve oListDettaglioTestata(nList)
    '                End If
    '                ''carico l'array con i dati della videata
    '                'oListDettaglioTestata(nList) = oMyDettaglioTestata
    '                'sono in modifica di un immobile già inserito nel db
    '                If TxtIdTessera.Text <> "-1" Then
    '                    'storicizzo il vano originale
    '                    If FunctionDettTestata.UpdateDettaglioTestata(WFSessione, oMyDettaglioTestata) = 0 Then
    '                        Return False
    '                    End If
    '                    If Not oMyDettaglioTestata.oRiduzioni Is Nothing Then
    '                        'inserisco i dati di riduzione svuotando prima tutto
    '                        If FncRidEse.SetRidEseApplicate(Costanti.AZIONE_DELETE, WFSessione, oMyDettaglioTestata.oRiduzioni, ObjRidEse.TIPO_RIDUZIONI, oMyDettaglioTestata.Id, -1) = 0 Then
    '                            Return 0
    '                        End If
    '                        If FncRidEse.SetRidEseApplicate(Costanti.AZIONE_NEW, WFSessione, oMyDettaglioTestata.oRiduzioni, ObjRidEse.TIPO_RIDUZIONI, oMyDettaglioTestata.Id, -1) = 0 Then
    '                            Return 0
    '                        End If
    '                    End If
    '                End If
    '            End If
    '        Else
    '            'controllo se ho lo stesso immobile con il periodo coerente
    '            If FunctionDettTestata.CheckPeriodi(oListDettaglioTestata, oMyDettaglioTestata) < 1 Then
    '                'sScript = "<script language='javascript'>alert('Attenzione! Periodi non coerenti.\nImpossibile proseguire!')</script>"
    '                sScript = "alert('Attenzione! Periodi non coerenti.');"
    '                'Return False
    '            End If
    '            'controllo se sono in modifica di un immobile
    '            Log.Debug("GestImmobili::SaveFromGestDich::controllo se sono in modifica di un immobile::azione::" & Request.Item("AzioneProv"))
    '            If Request.Item("AzioneProv") = Costanti.AZIONE_UPDATE Or Request.Item("AzioneProv") = Costanti.AZIONE_LETTURA Then
    '                'carico il singolo immobile selezionato
    '                For nList = 0 To oListDettaglioTestata.GetUpperBound(0)
    '                    If oListDettaglioTestata(nList).Id = Request.Item("IdUniqueUI") Then
    '                        'sono in modifica di un immobile già inserito nel db
    '                        If TxtIdTessera.Text <> "-1" Then
    '                            Log.Debug("GestImmobili::SaveFromGestDich::storicizzo l'immobile originale")
    '                            oMyDettaglioTestataOrg = oListDettaglioTestata(nList)
    '                            oMyDettaglioTestataOrg.tDataVariazione = Now
    '                            'storicizzo l'immobile originale
    '                            If FunctionDettTestata.UpdateDettaglioTestata(WFSessione, oMyDettaglioTestataOrg) = 0 Then
    '                                Return False
    '                            End If
    '                            'aggiungo il nuovo immobile con relativi vani
    '                            Dim oSingleDettTestata(0) As ObjDettaglioTestata
    '                            oSingleDettTestata(0) = oMyDettaglioTestata
    '                            Log.Debug("GestImmobili::SaveFromGestDich::aggiungo il nuovo immobile con relativi vani")
    '                            If FunctionDettTestata.SetDettaglioTestataCompleta(oSingleDettTestata, TxtIdTessera.Text, WFSessione) = 0 Then
    '                                Return False
    '                            End If
    '                        End If
    '                        oListDettaglioTestata(nList) = oMyDettaglioTestata
    '                        Exit For
    '                    End If
    '                Next
    '                'per la visualizzazione in griglia come sopra
    '                'aggiorno la riga
    '                For nListUIGrd = 0 To oListUIGrd.GetUpperBound(0)
    '                    If oListUIGrd(nListUIGrd).Id = Request.Item("IdUniqueUI") Then
    '                        'carico l'array con i dati della videata
    '                        oListUIGrd(nListUIGrd) = oMyDettaglioTestata
    '                        Exit For
    '                    End If
    '                Next
    '            Else
    '                'aggiungo una riga
    '                nList += 1
    '                'dimensiono l'array
    '                ReDim Preserve oListDettaglioTestata(nList)
    '                'carico l'array con i dati della videata
    '                oListDettaglioTestata(nList) = oMyDettaglioTestata

    '                'per la visualizzazione in griglia come sopra
    '                'aggiungo una riga
    '                nListUIGrd += 1
    '                'dimensiono l'array
    '                ReDim Preserve oListUIGrd(nListUIGrd)
    '                'carico l'array con i dati della videata
    '                oListUIGrd(nListUIGrd) = oMyDettaglioTestata
    '            End If
    '        End If
    '        'memorizzo l'oggetto nella sessione
    '        Session("oListUITessera") = oListDettaglioTestata
    '        'per la visualizzazione in griglia come sopra
    '        Session("oListImmobili") = oListUIGrd

    '        'ripulisco tutti i dati di sessioni dati immobile
    '        Session("oDatiVani") = Nothing
    '        Session("oDatiRid") = Nothing
    '        Session("oDatiDet") = Nothing
    '        Session.Remove("AbilitaGestioneImmobili")
    '        TxtIsRibalta.Text = "0"
    '        Return True
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestImmobili.SaveFromGestDich.errore: ", Err)
    'Response.Redirect("../../PaginaErrore.aspx")
    '        Return False
    '    Finally
    '        If Not IsNothing(WFSessione) Then
    '            WFSessione.Kill()
    '            WFSessione = Nothing
    '        End If
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="DBType"></param>
    ''' <param name="myConnectionString"></param>
    ''' <param name="oMyDettaglioTestata"></param>
    ''' <param name="sScript"></param>
    ''' <returns></returns>
    Private Function SaveFromGestDich(DBType As String, ByVal myConnectionString As String, ByVal oMyDettaglioTestata As ObjDettaglioTestata, ByRef sScript As String) As Boolean
        Dim oListDettaglioTestata() As ObjDettaglioTestata = Nothing
        Dim nList As Integer = -1
        Dim FncDettTestata As New Utility.DichManagerTARSU(DBType, myConnectionString, ConstSession.StringConnectionOPENgov, ConstSession.IdEnte) 'As New GestDettaglioTestata
        Dim FncGest As New GestDettaglioTestata
        Dim oListUIGrd() As ObjDettaglioTestata = Nothing
        Dim nListUIGrd As Integer = -1
        Dim oSingleDettTestata(0) As ObjDettaglioTestata

        Try
            Log.Debug("GestImmobili::SaveFromGestDich::controllo se ho già degli immobili caricati")
            'controllo se ho lo stesso immobile con il periodo coerente
            If FncGest.CheckPeriodi(oListDettaglioTestata, oMyDettaglioTestata) < 1 Then
                sScript = "GestAlert('a', 'warning', '', '', 'Attenzione! Periodi non coerenti.');"
            End If
            'controllo se ho già degli immobili caricati
            If Not Session("oListUITessera") Is Nothing Then
                'carico l'array originario
                oListDettaglioTestata = Session("oListUITessera")
                'aggiungo una riga
                nList = oListDettaglioTestata.GetUpperBound(0)
            End If
            'per la visualizzazione in griglia come sopra
            If Not Session("oListImmobili") Is Nothing Then
                'carico l'array originario
                oListUIGrd = Session("oListImmobili")
                'aggiungo una riga
                nListUIGrd = oListUIGrd.GetUpperBound(0)
            End If

            If Request.Item("IdUniqueUI") > -1 Then
                'storicizzo l'ui originale
                Dim ListUI() As ObjDettaglioTestata
                '*** X UNIONE CON BANCADATI CMGC ***
                If ConstSession.IsFromVariabile = "1" Then
                    Dim oMyTessera As New ObjTessera
                    oMyTessera = Session("oTessera")
                    ListUI = oMyTessera.oImmobili
                Else
                    ListUI = oListUIGrd
                End If
                If TxtIdUI.Text > 0 Then 'se minore di 0 arrivo da deduplica quindi no storicizzo
                    For Each oUI As ObjDettaglioTestata In ListUI
                        If oUI.Id = Request.Item("IdUniqueUI") Then
                            oUI.tDataVariazione = Now
                            'storicizzo l'immobile originale
                            'If FunctionDettTestata.UpdateDettaglioTestata(myConnectionString, oUI) = 0 Then
                            '    Return False
                            'End If
                            If FncDettTestata.SetDettaglioTestata(Utility.Costanti.AZIONE_UPDATE, oUI) = 0 Then
                                Return False
                            End If
                        End If
                    Next
                End If
                'salvo il nuovo immobile
                oMyDettaglioTestata.Id = -1 'ripulisco il campo per l'inserimento
                oSingleDettTestata(0) = oMyDettaglioTestata
                If FncDettTestata.SetDettaglioTestataCompleta(oSingleDettTestata, hdIdTestata.Value, TxtIdTessera.Text) = 0 Then
                    Return False
                End If
                If TxtIdUI.Text > 0 Then 'se minore di 0 arrivo da deduplica quindi no aggiorno ma aggiungo
                    'per la visualizzazione in griglia come sopra aggiorno la riga
                    For nListUIGrd = 0 To oListDettaglioTestata.GetUpperBound(0)
                        If oListDettaglioTestata(nListUIGrd).Id = Request.Item("IdUniqueUI") Then
                            'carico l'array con i dati della videata
                            oListDettaglioTestata(nListUIGrd) = oMyDettaglioTestata
                            Exit For
                        End If
                    Next
                    For nListUIGrd = 0 To oListUIGrd.GetUpperBound(0)
                        If oListUIGrd(nListUIGrd).Id = Request.Item("IdUniqueUI") Then
                            'carico l'array con i dati della videata
                            oListUIGrd(nListUIGrd) = oMyDettaglioTestata
                            Exit For
                        End If
                    Next
                Else
                    'aggiungo una riga
                    nList = oListDettaglioTestata.GetUpperBound(0) + 1
                    ReDim Preserve oListDettaglioTestata(nList)
                    oListDettaglioTestata(nList) = oMyDettaglioTestata
                    'aggiungo una riga
                    nListUIGrd = oListUIGrd.GetUpperBound(0) + 1
                    ReDim Preserve oListUIGrd(nListUIGrd)
                    oListUIGrd(nListUIGrd) = oMyDettaglioTestata
                End If
            ElseIf Request.Item("IdList") <> -1 Then
                'per la visualizzazione in griglia come sopra aggiorno la riga
                oListDettaglioTestata(Request.Item("IdList")) = oMyDettaglioTestata
                oListUIGrd(Request.Item("IdList")) = oMyDettaglioTestata
            Else
                'aggiungo una riga
                nList += 1
                'dimensiono l'array
                ReDim Preserve oListDettaglioTestata(nList)
                'carico l'array con i dati della videata
                oListDettaglioTestata(nList) = oMyDettaglioTestata

                'per la visualizzazione in griglia come sopra
                'aggiungo una riga
                nListUIGrd += 1
                'dimensiono l'array
                ReDim Preserve oListUIGrd(nListUIGrd)
                'carico l'array con i dati della videata
                oListUIGrd(nListUIGrd) = oMyDettaglioTestata
            End If
            'chiedo se si vuole ribaltare in nuovo immobile
            If TxtIsRibalta.Text = "1" Then
                Dim oUIRibaltato As New ObjDettaglioTestata
                oUIRibaltato.bForzaPV = oMyDettaglioTestata.bForzaPV
                oUIRibaltato.IdCatAteco = oMyDettaglioTestata.IdCatAteco
                oUIRibaltato.IdTessera = oMyDettaglioTestata.IdTessera
                oUIRibaltato.nComponentiPV = oMyDettaglioTestata.nComponentiPV
                oUIRibaltato.nGGTarsu = oMyDettaglioTestata.nGGTarsu
                oUIRibaltato.nIdAssenzaDatiCatastali = oMyDettaglioTestata.nIdAssenzaDatiCatastali
                oUIRibaltato.nIdDestUso = oMyDettaglioTestata.nIdDestUso
                oUIRibaltato.nIdNaturaOccupaz = oMyDettaglioTestata.nIdNaturaOccupaz
                oUIRibaltato.nIdTitoloOccupaz = oMyDettaglioTestata.nIdTitoloOccupaz
                oUIRibaltato.nMQ = oMyDettaglioTestata.nMQ
                oUIRibaltato.nMQAnater = oMyDettaglioTestata.nMQAnater
                oUIRibaltato.nMQCatasto = oMyDettaglioTestata.nMQCatasto
                oUIRibaltato.nMQTassabili = oMyDettaglioTestata.nMQTassabili
                oUIRibaltato.nNComponenti = oMyDettaglioTestata.nNComponenti
                oUIRibaltato.nVani = oMyDettaglioTestata.nVani
                oUIRibaltato.oDetassazioni = oMyDettaglioTestata.oDetassazioni
                oUIRibaltato.oOggetti = oMyDettaglioTestata.oOggetti
                oUIRibaltato.oRiduzioni = oMyDettaglioTestata.oRiduzioni
                oUIRibaltato.sCatAteco = oMyDettaglioTestata.sCatAteco
                oUIRibaltato.sCatCatastale = oMyDettaglioTestata.sCatCatastale
                oUIRibaltato.sCivico = oMyDettaglioTestata.sCivico
                oUIRibaltato.sCodVia = oMyDettaglioTestata.sCodVia
                oUIRibaltato.sDescrOccupazione = oMyDettaglioTestata.sDescrOccupazione
                oUIRibaltato.sEsponente = oMyDettaglioTestata.sEsponente
                oUIRibaltato.sEstensioneParticella = oMyDettaglioTestata.sEstensioneParticella
                oUIRibaltato.sFoglio = oMyDettaglioTestata.sFoglio
                oUIRibaltato.sIdStatoOccupazione = oMyDettaglioTestata.sIdStatoOccupazione
                oUIRibaltato.sIdTipoParticella = oMyDettaglioTestata.sIdTipoParticella
                oUIRibaltato.sIdTipoUnita = oMyDettaglioTestata.sIdTipoUnita
                oUIRibaltato.sInterno = oMyDettaglioTestata.sInterno
                oUIRibaltato.sNomeOccupantePrec = oMyDettaglioTestata.sNomeOccupantePrec
                oUIRibaltato.sNomeProprietario = oMyDettaglioTestata.sNomeProprietario
                oUIRibaltato.sNoteUI = oMyDettaglioTestata.sNoteUI
                oUIRibaltato.sNumero = oMyDettaglioTestata.sNumero
                oUIRibaltato.sOperatore = oMyDettaglioTestata.sOperatore
                oUIRibaltato.sScala = oMyDettaglioTestata.sScala
                oUIRibaltato.sSezione = oMyDettaglioTestata.sSezione
                oUIRibaltato.sSubalterno = oMyDettaglioTestata.sSubalterno
                oUIRibaltato.sVia = oMyDettaglioTestata.sVia
                oUIRibaltato.IdPadre = Request.Item("IdUniqueUI")
                oUIRibaltato.Id = -1
                oUIRibaltato.IdDettaglioTestata = -1
                'la data inizio deve essere data fine + 1
                oUIRibaltato.tDataInizio = DateAdd(DateInterval.Day, 1, oMyDettaglioTestata.tDataFine)
                'data fine in nuovo oggetto deve essere vuota
                oUIRibaltato.tDataFine = Nothing
                oUIRibaltato.tDataInserimento = Now
                If Request.Item("IdUniqueUI") <> -1 Then 'If TxtIdTessera.Text <> "-1" And Request.Item("IdUniqueUI") > -1 Then
                    'salvo il nuovo immobile
                    oSingleDettTestata(0) = oUIRibaltato
                    If FncDettTestata.SetDettaglioTestataCompleta(oSingleDettTestata, hdIdTestata.Value, TxtIdTessera.Text) = 0 Then
                        Return False
                    End If
                End If
                'aggiungo una riga
                nList = oListDettaglioTestata.GetUpperBound(0) + 1
                ReDim Preserve oListDettaglioTestata(nList)
                oListDettaglioTestata(nList) = oUIRibaltato
                'aggiungo una riga
                nListUIGrd = oListUIGrd.GetUpperBound(0) + 1
                ReDim Preserve oListUIGrd(nListUIGrd)
                oListUIGrd(nListUIGrd) = oUIRibaltato
            End If

            'memorizzo l'oggetto nella sessione
            Session("oListUITessera") = oListDettaglioTestata
            'per la visualizzazione in griglia come sopra
            Session("oListImmobili") = oListUIGrd
            CType(Session("oTestata"), ObjTestata).oImmobili = oListUIGrd
            'ripulisco tutti i dati di sessioni dati immobile
            Session("oDatiVani") = Nothing
            Session("oDatiRid") = Nothing
            Session("oDatiDet") = Nothing
            Session("oUITemp") = Nothing
            Session.Remove("AbilitaGestioneImmobili")
            TxtIsRibalta.Text = "0"
            Return True
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestImmobili.SaveFromGestDich.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
            Return False
        End Try
    End Function

    'Private Function SaveFromGestAE(ByVal oMyDettaglioTestata As ObjDettaglioTestata, ByVal WFSessione As OPENUtility.CreateSessione) As Boolean
    '    Dim FunctionDettTestata As New GestDettaglioTestata

    '    Try
    '        'inizializzo la connessione
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If
    '        'non storicizzo l'immobile originale ma aggiorno sempre i dati
    '        If FunctionDettTestata.UpdateDettaglioTestata(WFSessione, oMyDettaglioTestata) = 0 Then
    '            Return False
    '        End If
    '        'ripulisco tutti i dati di sessioni dati immobile
    '        Session("oDatiVani") = Nothing
    '        Session("oDatiRid") = Nothing
    '        Session("oDatiDet") = Nothing
    '        Session.Remove("AbilitaGestioneImmobili")
    '        TxtIsRibalta.Text = "0"
    '        Return True
    '    Catch Err As Exception
    '         Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestImmobili.SaveFromGestAE.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '        Return False
    '    Finally
    '        If Not IsNothing(WFSessione) Then
    '            WFSessione.Kill()
    '            WFSessione = Nothing
    '        End If
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oMyDettaglioTestata"></param>
    ''' <returns></returns>
    Private Function SaveFromGestAE(ByVal oMyDettaglioTestata As ObjDettaglioTestata) As Boolean
        Dim FunctionDettTestata As New Utility.DichManagerTARSU(ConstSession.DBType, ConstSession.StringConnection, ConstSession.StringConnectionOPENgov, ConstSession.IdEnte) 'As New GestDettaglioTestata

        Try
            'non storicizzo l'immobile originale ma aggiorno sempre i dati
            If FunctionDettTestata.SetDettaglioTestata(Utility.Costanti.AZIONE_UPDATE, oMyDettaglioTestata) = 0 Then 'If FunctionDettTestata.UpdateDettaglioTestata(ConstSession.StringConnection, oMyDettaglioTestata) = 0 Then
                Return False
            End If
            'ripulisco tutti i dati di sessioni dati immobile
            Session("oDatiVani") = Nothing
            Session("oDatiRid") = Nothing
            Session("oDatiDet") = Nothing
            Session("oUITemp") = Nothing
            Session.Remove("AbilitaGestioneImmobili")
            TxtIsRibalta.Text = "0"
            Return True
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestImmobili.SaveFromGestAE.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
            Return False
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Private Function BackToTessere() As String
        Dim sScript As String = ""
        Dim oMyTestata As New ObjTestata

        Try
            If AggiornaVarDatiVariabile(oMyTestata) = True Then
                sScript = "parent.Visualizza.location.href='GestTessere.aspx?IdContribuente=" & hdIdContribuente.Value & "&IdTestata=" & oMyTestata.Id & "&IdUniqueTessera=" & TxtIdTessera.Text & "&AzioneProv=" & Request.Item("AzioneProv") & "';" & vbCrLf
                sScript += "parent.Comandi.location.href='ComandiGestTessere.aspx?AzioneProv=" & Request.Item("AzioneProv") & "';"
            End If
            sScript += "parent.Basso.location.href='../../aspVuota.aspx';"
            sScript += "parent.Nascosto.location.href='../../aspVuota.aspx';"
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestImmobili.BackToTessere.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try

        Return sScript
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Private Function BackToDichiarazione() As String
        Dim sScript As String = ""
        Dim oMyTestata As New ObjTestata

        Try
            '*** X UNIONE CON BANCADATI CMGC ***
            If ConstSession.IsFromVariabile = "1" Then
                If AggiornaVarDatiVariabile(oMyTestata) = True Then
                    sScript = "parent.Visualizza.location.href='GestDichiarazione.aspx?IdUniqueTestata=" & oMyTestata.Id & "&sProvenienza=" & Request.Item("sProvenienza") & "&AzioneProv=" & Request.Item("AzioneProv") & "';" & vbCrLf
                    sScript += "parent.Comandi.location.href='ComandiGestDichiarazioni.aspx?AzioneProv=" & Request.Item("AzioneProv") & "';"
                End If
            Else
                If AggiornaVarDati(oMyTestata) = True Then
                    sScript = "parent.Visualizza.location.href='GestDichiarazione.aspx?IdUniqueTestata=" & oMyTestata.Id & "&sProvenienza=" & Request.Item("sProvenienza") & "&AzioneProv=" & Request.Item("AzioneProv") & "';" & vbCrLf
                    sScript += "parent.Comandi.location.href='ComandiGestDichiarazioni.aspx?AzioneProv=" & Request.Item("AzioneProv") & "';"
                End If
            End If
            sScript += "parent.Basso.location.href='../../aspVuota.aspx';"
            sScript += "parent.Nascosto.location.href='../../aspVuota.aspx';"
            '*** ***
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestImmobili.BackToDichiarazione.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
        Return sScript
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oMyTestata"></param>
    ''' <returns></returns>
    Private Function AggiornaVarDatiVariabile(ByRef oMyTestata As ObjTestata) As Boolean
        Dim nVani As Integer = 0
        Dim nMQ As Double = 0
        Dim oMyTot As New GestOggetti
        Dim FncTessere As New GestTessera

        Try
            'devo popolare l'oggetto TESSERAUI altrimenti non viene visualizzata correttamente la pagina della dichiarazione
            oMyTestata = Session("oTestata")
            oMyTestata.oTessere = Session("oListTessere")

            'aggiorno gli immobili per la tessera nella sessione
            Dim x As Integer
            Dim oListUI() As ObjDettaglioTestata
            oListUI = Session("oListUITessera")
            If Not oListUI Is Nothing Then
                If Not Session("oUITemp") Is Nothing Then 'se ho ancora questa variabile vuol dire che ho fatto solo modifiche ai vani quindi aggiorno quì l'array per la visualizzazione
                    'calcolo il totale di vani e mq per l'immobile
                    x = oMyTot.GetTotOggetti(Session("oDatiVani"), nVani, nMQ)
                    If x <> 1 Then
                        Return False
                    End If
                    CType(Session("oUITemp"), ObjDettaglioTestata).nVani = nVani
                    CType(Session("oUITemp"), ObjDettaglioTestata).nMQ = nMQ
                    'aggiorno l'array
                    If Request.Item("IdUniqueUI") > -1 Then
                        'per la visualizzazione in griglia come sopra aggiorno la riga
                        For Each myUI As ObjDettaglioTestata In oListUI
                            If myUI.Id = Request.Item("IdUniqueUI") Then
                                'carico l'array con i dati della videata
                                myUI = Session("oUITemp")
                                Exit For
                            End If
                        Next
                    ElseIf Request.Item("IdList") > -1 Then
                        'per la visualizzazione in griglia come sopra aggiorno la riga
                        oListUI(Request.Item("IdList")) = Session("oUITemp")
                    End If
                End If
                For x = 0 To oMyTestata.oTessere.GetUpperBound(0)
                    If oMyTestata.oTessere(x).Id = oListUI(0).IdTessera Then
                        oMyTestata.oTessere(x).oImmobili = oListUI
                        Session("oListImmobili") = oListUI
                        Exit For
                    End If
                Next
            End If
            'Log.Debug("GestImmobili::BackToTessere::oTessere.len::" & oMyTestata.oTessere.GetUpperBound(0).ToString())
            oMyTestata.oTesUI = FncTessere.GetTesVSUI(oMyTestata)
            Session("oTestata") = oMyTestata
            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestImmobili.AggiornaVarDatiVariabile.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Return False
        End Try
    End Function
    '*** X UNIONE CON BANCADATI CMGC ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oMyTestata"></param>
    ''' <returns></returns>
    Private Function AggiornaVarDati(ByRef oMyTestata As ObjTestata) As Boolean
        Dim nVani As Integer = 0
        Dim nMQ As Double = 0
        Dim oMyTot As New GestOggetti

        Try
            'devo popolare l'oggetto TESSERAUI altrimenti non viene visualizzata correttamente la pagina della dichiarazione
            oMyTestata = Session("oTestata")

            'aggiorno gli immobili per la tessera nella sessione
            Dim x As Integer
            Dim oListUI() As ObjDettaglioTestata
            oListUI = Session("oListImmobili") 'Session("oDettaglioTestata")
            If Not oListUI Is Nothing Then
                If Not Session("oUITemp") Is Nothing Then 'se ho ancora questa variabile vuol dire che ho fatto solo modifiche ai vani quindi aggiorno quì l'array per la visualizzazione
                    'calcolo il totale di vani e mq per l'immobile
                    x = oMyTot.GetTotOggetti(Session("oDatiVani"), nVani, nMQ)
                    If x <> 1 Then
                        Return False
                    End If
                    CType(Session("oUITemp"), ObjDettaglioTestata).nVani = nVani
                    CType(Session("oUITemp"), ObjDettaglioTestata).nMQ = nMQ
                    'aggiorno l'array
                    If Request.Item("IdUniqueUI") > -1 Then
                        'per la visualizzazione in griglia come sopra aggiorno la riga
                        For Each myUI As ObjDettaglioTestata In oListUI
                            If myUI.Id = Request.Item("IdUniqueUI") Then
                                'carico l'array con i dati della videata
                                myUI = Session("oUITemp")
                                Exit For
                            End If
                        Next
                    ElseIf Request.Item("IdList") > -1 Then
                        'per la visualizzazione in griglia come sopra aggiorno la riga
                        oListUI(Request.Item("IdList")) = Session("oUITemp")
                    End If
                End If
                oMyTestata.oImmobili = oListUI
            End If
            Session("oTestata") = oMyTestata
            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestImmobili.AggiornaVarDati.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Return False
        End Try
    End Function
    '*** ***
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub LoadUIVSCat()
        Dim myArray As New ArrayList
        Dim myUIVSCat As New ObjOggetti
        Dim oListVani() As ObjOggetti
        Dim bTrovato As Boolean

        Try
            If Not Session("oDatiVani") Is Nothing Then
                oListVani = Session("oDatiVani")
                For Each myVano As ObjOggetti In oListVani
                    bTrovato = False
                    For Each myItem As Object In myArray
                        If ConstSession.IsFromTARES = "1" Then
                            If myVano.IdCatTARES = CType(myItem, ObjOggetti).IdCatTARES And myVano.nNC = CType(myItem, ObjOggetti).nNC And myVano.nNCPV = CType(myItem, ObjOggetti).nNCPV Then
                                bTrovato = True
                                CType(myItem, ObjOggetti).nVani += myVano.nVani
                                CType(myItem, ObjOggetti).nMq += myVano.nMq
                                If myVano.bIsEsente = False Then 'appoggio per mq tassabili
                                    CType(myItem, ObjOggetti).sOperatore = CDbl(CType(myItem, ObjOggetti).sOperatore) + myVano.nMq
                                Else
                                    CType(myItem, ObjOggetti).sOperatore = CDbl(CType(myItem, ObjOggetti).sOperatore) + 0
                                End If
                            End If
                        Else
                            If myVano.IdCategoria = CType(myItem, ObjOggetti).IdCategoria Then
                                bTrovato = True
                                CType(myItem, ObjOggetti).nVani += myVano.nVani
                                CType(myItem, ObjOggetti).nMq += myVano.nMq
                                If myVano.bIsEsente = False Then 'appoggio per mq tassabili
                                    CType(myItem, ObjOggetti).sOperatore = CDbl(CType(myItem, ObjOggetti).sOperatore) + myVano.nMq
                                Else
                                    CType(myItem, ObjOggetti).sOperatore = CDbl(CType(myItem, ObjOggetti).sOperatore) + 0
                                End If
                            End If
                        End If
                    Next
                    If bTrovato = False Then
                        myUIVSCat = New ObjOggetti
                        myUIVSCat.nNC = myVano.nNC
                        myUIVSCat.nNCPV = myVano.nNCPV
                        myUIVSCat.nVani = myVano.nVani
                        myUIVSCat.nMq = myVano.nMq
                        If myVano.bIsEsente = False Then 'appoggio per mq tassabili
                            myUIVSCat.sOperatore = myVano.nMq
                        Else
                            myUIVSCat.sOperatore = 0
                        End If
                        If ConstSession.IsFromTARES = "1" Then
                            myUIVSCat.IdCatTARES = myVano.IdCatTARES
                            myUIVSCat.sCategoria = myVano.sDescrCatTARES
                            If myUIVSCat.sCategoria.ToUpper.StartsWith("DOM") Then
                                myUIVSCat.sCategoria += Space(3) & myVano.nNC.ToString & "  COMPONENTI PF - " & Space(3) & myVano.nNCPV.ToString & " COMPONENTI PV"
                            End If
                        Else
                            myUIVSCat.IdCategoria = myVano.IdCategoria
                            myUIVSCat.sCategoria = myVano.sCategoria
                        End If
                        myArray.Add(myUIVSCat)
                    End If
                Next
            End If

            If myArray.Count > 0 Then
                GrdUIVSCat.SelectedIndex = -1
                GrdUIVSCat.DataSource = CType(myArray.ToArray(GetType(ObjOggetti)), ObjOggetti())
                GrdUIVSCat.DataBind()
                GrdUIVSCat.Style.Add("display", "")
                LblResultUIVSCat.Style.Add("display", "none")
            Else
                GrdUIVSCat.Style.Add("display", "none")
                LblResultUIVSCat.Style.Add("display", "")
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestImmobili.LoadUIVSCat.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nVani"></param>
    ''' <param name="nMQ"></param>
    ''' <param name="oMyDettaglioTestata"></param>
    ''' <returns></returns>
    Private Function LoadDatiFromForm(ByVal nVani As Integer, ByVal nMQ As Double, ByRef oMyDettaglioTestata As ObjDettaglioTestata) As Boolean
        Try
            'carico i dati dal form
            oMyDettaglioTestata.Id = TxtIdUI.Text
            oMyDettaglioTestata.IdDettaglioTestata = TxtIdDettaglioTestata.Text
            If TxtIdTessera.Text = "" Then
                oMyDettaglioTestata.IdTessera = -1
            Else
                oMyDettaglioTestata.IdTessera = CInt(TxtIdTessera.Text)
            End If
            oMyDettaglioTestata.IdTestata = hdIdTestata.Value
            oMyDettaglioTestata.IdPadre = TxtIdPadre.Text
            oMyDettaglioTestata.sCodVia = TxtCodVia.Text
            oMyDettaglioTestata.sVia = TxtVia.Text.Trim
            oMyDettaglioTestata.sCivico = TxtCivico.Text.Trim
            oMyDettaglioTestata.sEsponente = TxtEsponente.Text.Trim
            oMyDettaglioTestata.sInterno = TxtInterno.Text.Trim
            oMyDettaglioTestata.sScala = TxtScala.Text.Trim
            If TxtDataInizio.Text <> "" Then
                oMyDettaglioTestata.tDataInizio = TxtDataInizio.Text
            End If
            If TxtDataFine.Text <> "" Then
                oMyDettaglioTestata.tDataFine = TxtDataFine.Text
            End If
            oMyDettaglioTestata.nVani = nVani
            oMyDettaglioTestata.nMQ = nMQ
            If TxtGGTarsu.Text.Trim <> "" Then
                oMyDettaglioTestata.nGGTarsu = CInt(TxtGGTarsu.Text.Trim)
            End If
            If TxtNComponenti.Text <> "" Then
                oMyDettaglioTestata.nNComponenti = TxtNComponenti.Text
            End If
            oMyDettaglioTestata.sFoglio = TxtFoglio.Text
            oMyDettaglioTestata.sNumero = TxtNumero.Text
            oMyDettaglioTestata.sSubalterno = TxtSubalterno.Text
            '***Agenzia Entrate***
            oMyDettaglioTestata.sSezione = TxtSezione.Text
            oMyDettaglioTestata.sEstensioneParticella = TxtEstParticella.Text
            oMyDettaglioTestata.sIdTipoParticella = DdlTipoParticella.SelectedValue
            If DdlTitOccupaz.SelectedValue <> "" Then
                oMyDettaglioTestata.nIdTitoloOccupaz = DdlTitOccupaz.SelectedValue
            End If
            If DdlNatOccupaz.SelectedValue <> "" Then
                oMyDettaglioTestata.nIdNaturaOccupaz = DdlNatOccupaz.SelectedValue
            End If
            If DdlDestUso.SelectedValue <> "" Then
                oMyDettaglioTestata.nIdDestUso = DdlDestUso.SelectedValue
            End If
            oMyDettaglioTestata.sIdTipoUnita = DdlTipoUnita.SelectedValue
            If DdlAssenzaDatiCat.SelectedValue <> "" Then
                oMyDettaglioTestata.nIdAssenzaDatiCatastali = DdlAssenzaDatiCat.SelectedValue
            End If
            '*********************
            oMyDettaglioTestata.sIdStatoOccupazione = DdlStatoOccupazione.SelectedValue
            oMyDettaglioTestata.sNomeProprietario = TxtPropietario.Text
            oMyDettaglioTestata.sNomeOccupantePrec = TxtOccupantePrec.Text
            oMyDettaglioTestata.sNoteUI = TxtNoteUI.Text
            oMyDettaglioTestata.tDataInserimento = Now
            oMyDettaglioTestata.tDataCessazione = Nothing
            oMyDettaglioTestata.sOperatore = ConstSession.UserName
            oMyDettaglioTestata.oOggetti = Session("oDatiVani")
            '*** 20130201 - gestione mq da catasto per TARES ***
            Log.Debug("GestImmobili::CmdSalvaDatiUI_Click::devo registrare mqcatasto")
            If TxtMQCatasto.Text.Trim <> "" Then
                oMyDettaglioTestata.nMQCatasto = CDbl(TxtMQCatasto.Text.Trim)
            End If
            '*** ***
            '*** 20130325 - gestione mq tassabili per TARES ***
            Log.Debug("GestImmobili::CmdSalvaDatiUI_Click::devo registrare mqtassabili")
            If TxtMQTassabili.Text.Trim <> "" Then
                oMyDettaglioTestata.nMQTassabili = CDbl(TxtMQTassabili.Text.Trim)
            End If
            '*** ***
            '*** 20130228 - gestione categoria Ateco per TARES ***
            'Log.Debug("GestImmobili::CmdSalvaDatiUI_Click::devo registrare ddlcattares")
            If DDlCatTARES.SelectedValue <> "" Then
                oMyDettaglioTestata.IdCatAteco = DDlCatTARES.SelectedValue
                oMyDettaglioTestata.sCatAteco = DDlCatTARES.SelectedItem.Text
            End If
            'Log.Debug("GestImmobili::CmdSalvaDatiUI_Click::devo registrare ncomponentipv")
            If TxtNComponentiPV.Text.Trim <> "" Then
                oMyDettaglioTestata.nComponentiPV = CInt(TxtNComponentiPV.Text.Trim)
            Else
                oMyDettaglioTestata.nComponentiPV = oMyDettaglioTestata.nNComponenti
            End If
            'Log.Debug("GestImmobili::CmdSalvaDatiUI_Click::devo registrare forzapv")
            oMyDettaglioTestata.bForzaPV = ChkForzaPV.Checked
            '*** ***
            '*** 20140805 - Gestione Categorie Vani ***
            'se sono stati cambiati categoria e/o nc devono essere aggiornati anche i relativi oggetti
            If oMyDettaglioTestata.IdCatAteco.ToString + "|" + oMyDettaglioTestata.nNComponenti.ToString + "|" + oMyDettaglioTestata.nComponentiPV.ToString <> Session("oldcattares") Then
                If Not oMyDettaglioTestata.oOggetti Is Nothing Then
                    For Each myVano As ObjOggetti In oMyDettaglioTestata.oOggetti
                        If myVano.IdCatTARES.ToString + "|" + myVano.nNC.ToString + "|" + myVano.nNCPV.ToString = Session("oldcattares") Then
                            myVano.IdCatTARES = oMyDettaglioTestata.IdCatAteco
                            myVano.sDescrCatTARES = oMyDettaglioTestata.sCatAteco
                            myVano.nNC = oMyDettaglioTestata.nNComponenti
                            myVano.nNCPV = oMyDettaglioTestata.nComponentiPV
                        End If
                    Next
                End If
            End If
            ''*** ***
            oMyDettaglioTestata.oRiduzioni = Session("oDatiRid")
            oMyDettaglioTestata.oDetassazioni = Session("oDatiDet")

            'BD 09/07/2021
            If TxtRidImp.Text.Trim <> "" Then
                oMyDettaglioTestata.ImportoFissoRid = CDbl(TxtRidImp.Text.Trim)
            End If
            'BD 09/07/2021

            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestImmobili.LoadDatiFromForm.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Return False
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oMyDettaglioTestata"></param>
    ''' <returns></returns>
    Private Function LoadDatiIntoForm(ByVal oMyDettaglioTestata As ObjDettaglioTestata) As Boolean
        Try
            'prelevo i vani
            If Not oMyDettaglioTestata.oOggetti Is Nothing Then
                Session("oDatiVani") = oMyDettaglioTestata.oOggetti
            End If
            'prelevo le riduzioni
            If Not oMyDettaglioTestata.oRiduzioni Is Nothing Then
                Session("oDatiRid") = oMyDettaglioTestata.oRiduzioni
            End If
            'prelevo le detassazioni
            If Not oMyDettaglioTestata.oDetassazioni Is Nothing Then
                Session("oDatiDet") = oMyDettaglioTestata.oDetassazioni
            End If
            'visualizzo i dati del singolo immobile selezionato
            TxtIdUI.Text = oMyDettaglioTestata.Id
            TxtIdDettaglioTestata.Text = oMyDettaglioTestata.IdDettaglioTestata
            TxtIdTessera.Text = oMyDettaglioTestata.IdTessera 'Request.Item("IdTessera")
            hdIdTestata.Value = oMyDettaglioTestata.IdTestata 'Request.Item("IdTestata")
            TxtIdPadre.Text = oMyDettaglioTestata.IdPadre
            TxtCodVia.Text = oMyDettaglioTestata.sCodVia
            TxtVia.Text = oMyDettaglioTestata.sVia
            If oMyDettaglioTestata.sCivico <> "0" And oMyDettaglioTestata.sCivico <> "-1" Then
                TxtCivico.Text = oMyDettaglioTestata.sCivico
            End If
            TxtEsponente.Text = oMyDettaglioTestata.sEsponente
            TxtInterno.Text = oMyDettaglioTestata.sInterno
            TxtScala.Text = oMyDettaglioTestata.sScala
            TxtDataInizio.Text = FncGrd.FormattaDataGrd(oMyDettaglioTestata.tDataInizio)
            TxtDataFine.Text = FncGrd.FormattaDataGrd(oMyDettaglioTestata.tDataFine)
            If oMyDettaglioTestata.nGGTarsu > 0 Then
                ChkIsGiornaliera.Checked = True
                If oMyDettaglioTestata.nGGTarsu <> -1 Then
                    TxtGGTarsu.Text = oMyDettaglioTestata.nGGTarsu
                End If
            End If
            If oMyDettaglioTestata.nNComponenti <> -1 Then
                TxtNComponenti.Text = oMyDettaglioTestata.nNComponenti
            Else
                TxtNComponenti.Text = "0"
            End If
            TxtFoglio.Text = oMyDettaglioTestata.sFoglio
            TxtNumero.Text = oMyDettaglioTestata.sNumero
            TxtSubalterno.Text = oMyDettaglioTestata.sSubalterno
            '***Agenzia Entrate***
            TxtSezione.Text = oMyDettaglioTestata.sSezione
            TxtEstParticella.Text = oMyDettaglioTestata.sEstensioneParticella
            DdlTipoParticella.SelectedValue = oMyDettaglioTestata.sIdTipoParticella
            If oMyDettaglioTestata.nIdTitoloOccupaz > 0 Then
                DdlTitOccupaz.SelectedValue = oMyDettaglioTestata.nIdTitoloOccupaz
            End If
            If oMyDettaglioTestata.nIdNaturaOccupaz > 0 Then
                DdlNatOccupaz.SelectedValue = oMyDettaglioTestata.nIdNaturaOccupaz
            End If
            If oMyDettaglioTestata.nIdDestUso > 0 Then
                DdlDestUso.SelectedValue = oMyDettaglioTestata.nIdDestUso
            End If
            If oMyDettaglioTestata.sIdTipoUnita <> "" Then
                DdlTipoUnita.SelectedValue = oMyDettaglioTestata.sIdTipoUnita
            End If
            If oMyDettaglioTestata.nIdAssenzaDatiCatastali <> 0 Then
                DdlAssenzaDatiCat.SelectedValue = oMyDettaglioTestata.nIdAssenzaDatiCatastali
            End If
            '*********************
            DdlStatoOccupazione.SelectedValue = oMyDettaglioTestata.sIdStatoOccupazione
            TxtPropietario.Text = oMyDettaglioTestata.sNomeProprietario
            TxtOccupantePrec.Text = oMyDettaglioTestata.sNomeOccupantePrec
            TxtNoteUI.Text = oMyDettaglioTestata.sNoteUI
            '*** 20130201 - gestione mq da catasto per TARES ***
            TxtMQCatasto.Text = oMyDettaglioTestata.nMQCatasto
            '*** ***
            '*** 20130325 - gestione mq tassabili per TARES ***
            TxtMQTassabili.Text = oMyDettaglioTestata.nMQTassabili
            '*** ***
            '*** 20130228 - gestione categoria Ateco per TARES ***
            If oMyDettaglioTestata.IdCatAteco > 0 Then
                DDlCatTARES.SelectedValue = oMyDettaglioTestata.IdCatAteco
            End If
            TxtNComponentiPV.Text = oMyDettaglioTestata.nComponentiPV
            ChkForzaPV.Checked = oMyDettaglioTestata.bForzaPV
            '*** ***

            'BD 09/07/2021
            TxtRidImp.Text = oMyDettaglioTestata.ImportoFissoRid
            'BD 09/07/2021

            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestImmobili.LoadDatiIntoForm.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Return False
        End Try
    End Function
    ''' <summary>
    ''' Funzione per abilitare/disabilitare i comandi
    ''' </summary>
    ''' <revisionHistory>
    ''' <revision date="23/09/2014">
    ''' GIS
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="07/2015">
    ''' GESTIONE INCROCIATA RIFIUTI/ICI e DIVERSA GESTIONE QUOTA VARIABILE
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Private Sub GestComandi()
        Try
            Dim sScript As String
            sScript = "parent.Comandi.location.href = '../../aspVuotaRemoveComandi.aspx';"
            sScript += "parent.Basso.location.href = '../../aspVuotaRemoveComandi.aspx';"
            sScript += "parent.Nascosto.location.href = '../../aspVuotaRemoveComandi.aspx';"

            lblTitolo.Text = ConstSession.DescrizioneEnte
            If ConstSession.IsFromTARES = "1" Then
                info.InnerText = "TARI "
            Else
                info.InnerText = "TARSU "
            End If
            If ConstSession.IsFromVariabile = "1" Then
                info.InnerText += " Variabile "
            End If
            info.InnerText += "- Dichiarazioni - Gestione Immobili"
            If Request.Item("sProvenienza") = Costanti.FormProvenienza.DatiAE Then
                info.InnerText += " per Agenzia Entrate"
            End If
            If ConstSession.VisualGIS = False Then
                sScript += "document.getElementById('GIS').style.display='none';"
            End If
            If IsNumeric(Request.Item("Provenienza")) Then
                If Request.Item("Provenienza") = Costanti.FormProvenienza.ICIDich Then
                    sScript += "document.getElementById('GIS').style.display='none';"
                    sScript += "document.getElementById('Territorio').style.display='none';"
                    sScript += "document.getElementById('Duplica').style.display='none';"
                    sScript += "document.getElementById('Modifica').style.display='none';"
                    sScript += "document.getElementById('Delete').style.display='none';"
                    sScript += "document.getElementById('Salva').style.display='none';"
                End If
            End If

            Dim oListCmd() As Object
            If Request.Item("sProvenienza") = Costanti.FormProvenienza.DatiAE Then
                ReDim Preserve oListCmd(2)
                oListCmd(0) = "Modifica"
                oListCmd(1) = "Delete"
                oListCmd(2) = "Duplica"
            ElseIf Request.Item("AzioneProv") = Utility.Costanti.AZIONE_NEW Then
                ReDim Preserve oListCmd(2)
                oListCmd(0) = "Modifica"
                oListCmd(1) = "Delete"
                oListCmd(2) = "Duplica"
            Else
                ReDim Preserve oListCmd(0)
                oListCmd(0) = "Salva"
            End If
            sScript += "$('.BottoneCancellaGrd').addClass('DisableBtn');"
            For Each myItem As String In oListCmd
                sScript += "$('#" & myItem & "').addClass('DisableBtn');"
            Next
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestImmobili.GestComandi.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Protected Sub TxtRidImp_TextChanged(sender As Object, e As EventArgs)

    End Sub
    'Private Sub GestComandi()
    '    Try
    '        Dim sScript As String
    '        sScript = "parent.Comandi.location.href = '../../aspVuotaRemoveComandi.aspx';"
    '        sScript += "parent.Basso.location.href = '../../aspVuotaRemoveComandi.aspx';"
    '        sScript += "parent.Nascosto.location.href = '../../aspVuotaRemoveComandi.aspx';"

    '        lblTitolo.Text = ConstSession.DescrizioneEnte
    '        If ConstSession.IsFromTARES = "1" Then
    '            info.InnerText = "TARI "
    '        Else
    '            info.InnerText = "TARSU "
    '        End If
    '        If ConstSession.IsFromVariabile = "1" Then
    '            info.InnerText += " Variabile "
    '        End If
    '        info.InnerText += "- Dichiarazioni - Gestione Immobili"
    '        If Request.Item("sProvenienza") = Costanti.FormProvenienza.DatiAE Then
    '            info.InnerText += " per Agenzia Entrate"
    '        End If
    '        '*** 20140923 - GIS ***
    '        If ConstSession.VisualGIS = False Then
    '            sScript += "document.getElementById('GIS').style.display='none';"
    '        End If
    '        '*** ***
    '        '*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI & DIVERSA GESTIONE QUOTA VARIABILE***
    '        If IsNumeric(Request.Item("Provenienza")) Then
    '            If Request.Item("Provenienza") = Costanti.FormProvenienza.ICIDich Then
    '                sScript += "document.getElementById('GIS').style.display='none';"
    '                sScript += "document.getElementById('Territorio').style.display='none';"
    '                sScript += "document.getElementById('Duplica').style.display='none';"
    '                sScript += "document.getElementById('Modifica').style.display='none';"
    '                sScript += "document.getElementById('Delete').style.display='none';"
    '                sScript += "document.getElementById('Salva').style.display='none';"
    '            End If
    '        End If
    '        '*** ***    

    '        Dim oListCmd() As Object
    '        If Request.Item("sProvenienza") = Costanti.FormProvenienza.DatiAE Then
    '            ReDim Preserve oListCmd(2)
    '            oListCmd(0) = "Modifica"
    '            oListCmd(1) = "Delete"
    '            oListCmd(2) = "Duplica"
    '            'sScript = MyFunction.PopolaJSdisabilita(oListCmd)
    '            'RegisterScript( sScript,Me.GetType)
    '        ElseIf Request.Item("AzioneProv") = Utility.Costanti.AZIONE_NEW Then
    '            ReDim Preserve oListCmd(2)
    '            oListCmd(0) = "Modifica"
    '            oListCmd(1) = "Delete"
    '            oListCmd(2) = "Duplica"
    '            'sScript = MyFunction.PopolaJSdisabilita(oListCmd)
    '            'RegisterScript( sScript,Me.GetType)
    '        Else
    '            ReDim Preserve oListCmd(0)
    '            oListCmd(0) = "Salva"
    '            'sScript = MyFunction.PopolaJSdisabilita(oListCmd)
    '            'RegisterScript( sScript,Me.GetType)
    '        End If
    '        For Each myItem As String In oListCmd
    '            sScript += "$('#" & myItem & "').addClass('hidden');"
    '        Next

    '        RegisterScript(sScript, Me.GetType())
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestImmobili.GestComandi.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub UnloadPage()
        Try
            'aggiorno la pagina chiamante
            Dim sScript As String = String.Empty

            'se provengo da situazione contribuente richiamo le relative pagine
            If Session("SituazioneContribuente") = "SC" Then
                Session("SituazioneContribuente") = Nothing
                sScript = "parent.Visualizza.location.href='../../SituazioneContribuente/GestioneSituazione.aspx?COD_CONTRIBUENTE=" & Request.Item("IdContribuenteDichT") & "&strAnno=" & CStr(Request.Item("AnnoSC")) & "';" & vbCrLf
                sScript += "parent.Comandi.location.href='../../SituazioneContribuente/CGestioneSituazione.aspx';"
            ElseIf Utility.StringOperation.FormatString(Request.Item("Sportello")) = "1" Then
                sScript = "parent.window.close();"
            ElseIf Utility.StringOperation.FormatString(request.item("Provenienza")) = Costanti.FormProvenienza.DatiAE Then
                sScript = "parent.Visualizza.location.href='../AgenziaEntrate/DatiMancanti/RicercaDatiMancanti.aspx';" & vbCrLf
                sScript += "parent.Comandi.location.href='../AgenziaEntrate/DatiMancanti/ComandiRicDatiMancanti.aspx';"
            ElseIf Utility.StringOperation.FormatString(request.item("Provenienza")) = Costanti.FormProvenienza.ICICheckRif Then
                sScript = "parent.Comandi.location.href = '../../aspVuotaRemoveComandi.aspx';"
                sScript += "parent.Visualizza.location.href = '../.." & ConstSession.Path_ICI & "/ConfrontaConCatasto/CheckRifCatastali.aspx'" '?TypeCheck=12
            ElseIf Utility.StringOperation.FormatString(request.item("Provenienza")) = Costanti.FormProvenienza.ICIDich Then
                sScript = "parent.Comandi.location.href = '../.." & ConstSession.Path_ICI & "/CImmobileDettaglioMod.aspx?Operation=DETTAGLIO';"
                sScript += "parent.Visualizza.location.href = '../.." & ConstSession.Path_ICI & "/ImmobileDettaglio.aspx?" & Request.Item("ParamRitornoICI").Replace("""", "").Replace("$", "&") & "';"
                sScript += "parent.Basso.location.href='../../aspVuota.aspx';"
                sScript += "parent.Nascosto.location.href='../../aspVuota.aspx';"
                '*** 20150703 - INTERROGAZIONE GENERALE ***
            ElseIf Utility.StringOperation.FormatString(request.item("Provenienza")) = Costanti.FormProvenienza.InterGen Then
                sScript = "parent.Visualizza.location.href='../../Interrogazioni/DichEmesso.aspx?Ente=" & ConstSession.IdEnte & "';"
                sScript += "parent.Comandi.location.href='../../aspVuotaRemoveComandi.aspx';"
                sScript += "parent.Basso.location.href='../../aspVuotaRemoveComandi.aspx';"
                sScript += "parent.Nascosto.location.href='../../aspVuotaRemoveComandi.aspx';"
                '*** ***
            ElseIf Utility.StringOperation.FormatString(request.item("Provenienza")) = Costanti.FormProvenienza.Tessera Then
                sScript = BackToTessere()
            ElseIf Utility.StringOperation.FormatString(request.item("Provenienza")) = Costanti.FormProvenienza.Dichiarazione Then
                sScript = BackToDichiarazione()
            End If
            Log.Debug("ritorno su::" & sScript)
            'RegisterScript( sScript,Me.GetType)
            RegisterScript(sScript, Me.GetType())
            'ripulisco tutti i dati di sessioni dati immobile
            Session("oDatiVani") = Nothing
            Session("oDatiRid") = Nothing
            Session("oDatiDet") = Nothing
            Session("oUITemp") = Nothing
            Session.Remove("AbilitaGestioneImmobili")
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestImmobili.UnloadPage.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
