Imports ComPlusInterface
Imports log4net
'Imports ComPlusInterface.ProvvedimentiTarsu.Oggetti
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
''' <summary>
''' Pagina per la generazione dei provvedimenti da accertamenti pregressi.
''' Contiene le funzioni della comandiera e la griglia per la visualizzazione del risultato. 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
''' <revisionHistory>
''' <revision date="12/04/2019">
''' <strong>Qualificazione AgID-analisi_rel01</strong>
''' <em>Analisi eventi</em>
''' </revision>
''' </revisionHistory>
Partial Class SearchAccertato
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(SearchAccertato))
    Private objHashTable As Hashtable = New Hashtable
    Private FncGestAccert As New ClsGestioneAccertamenti
    Private codContribuente As String
    Private annoAccertamento As String
    Private Tributo As String
    'Private idCelle As New DataGridIndex
    Protected FncForGrd As New Formatta.SharedGrd
    Protected FncGrd As New Formatta.FunctionGrd
    Private sScript As String = ""

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblInfo As System.Web.UI.WebControls.Label

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
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Analisi eventi</em>
    ''' </revision>
    ''' </revisionHistory>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            '---------------------------GESTIONE ACCERTAMENTI DA RETTIFICA-------------------------------------------
            Dim myHasTbl As New Hashtable
            Try
                If Not Session("HashTableRettificaAccertamenti") Is Nothing Then
                    myHasTbl = Session("HashTableRettificaAccertamenti")
                End If

                codContribuente = myHasTbl("CODCONTRIBUENTE")
                Tributo = myHasTbl("COD_TRIBUTO")
                annoAccertamento = myHasTbl("ANNOACCERTAMENTO")
            Catch err As Exception
            End Try
            If Not Page.IsPostBack Then
                TxtTributo.Text = Tributo
                If Not Session("oAnagrafe") Is Nothing Then
                    Dim oDettaglioAnagrafica As AnagInterface.DettaglioAnagrafica = Session("oAnagrafe")

                    sScript += "$('#divHeader').append('"
                    sScript += "<label class=\'Input_Label\'>Nominativo:</label>"
                    sScript += "<label class=\'Legend\'>" + oDettaglioAnagrafica.Cognome + " " + oDettaglioAnagrafica.Nome + "</label>"
                    sScript += "&nbsp;&nbsp;&nbsp;<label class=\'Input_Label\'>Cod.Fiscale/P.IVA:</label>"
                    sScript += "<label class=\'Legend\'>"
                    If oDettaglioAnagrafica.PartitaIva <> "" Then
                        sScript += oDettaglioAnagrafica.PartitaIva
                    Else
                        sScript += oDettaglioAnagrafica.CodiceFiscale
                    End If
                    sScript += "</label><br>"
                    sScript += "<label class=\'Input_Label\'>Anno da Accertare:</label>"
                    sScript += "<label class=\'Legend\'>" + annoAccertamento + "</label>"
                    sScript += "&nbsp;&nbsp;&nbsp;<label class=\'Input_Label\'>N.B. saranno visualizzati solo gli immobili aperti nell'anno da accertare</label>"
                    sScript += "');"
                    RegisterScript(sScript, Me.GetType)
                End If
                CaricaGriglia(codContribuente, annoAccertamento, TxtTributo.Text)
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertato.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    '    Try
    '        '---------------------------GESTIONE ACCERTAMENTI DA RETTIFICA-------------------------------------------
    '        Dim myHasTbl As New Hashtable
    '        Try
    '            If Not Session("HashTableRettificaAccertamenti") Is Nothing Then
    '                myHasTbl = Session("HashTableRettificaAccertamenti")
    '            End If

    '            codContribuente = myHasTbl("CODCONTRIBUENTE")
    '            Tributo = myHasTbl("COD_TRIBUTO")
    '            annoAccertamento = myHasTbl("ANNOACCERTAMENTO")
    '        Catch err As Exception
    '        End Try
    '        'codContribuente = Request.Item("codContribuente")
    '        'annoAccertamento = Request.Item("anno")
    '        'If Not IsNothing(Request.Item("tributo")) Then
    '        '    Tributo = Request.Item("tributo")
    '        'Else
    '        '    Tributo = Utility.Costanti.TRIBUTO_ICI
    '        'End If
    '        '--------------------------------------------------------------------------------------------------------
    '        If Not Page.IsPostBack Then
    '            TxtTributo.Text = Tributo
    '            If Not Session("oAnagrafe") Is Nothing Then
    '                Dim oDettaglioAnagrafica As AnagInterface.DettaglioAnagrafica = Session("oAnagrafe")
    '                lblNominativo.Text = "Nominativo:" + oDettaglioAnagrafica.Cognome + " " + oDettaglioAnagrafica.Nome
    '                lblNominativo.Text += "   Cod.Fiscale/P.IVA: "
    '                If oDettaglioAnagrafica.PartitaIva <> "" Then
    '                    lblNominativo.Text += oDettaglioAnagrafica.PartitaIva
    '                Else
    '                    lblNominativo.Text += oDettaglioAnagrafica.CodiceFiscale
    '                End If
    '            End If
    '            lblAnno.Text = "Anno da Accertare: " + annoAccertamento + Space(10) + " N.B. saranno visualizzati solo gli immobili aperti nell'anno da accertare"
    '            CaricaGriglia(codContribuente, annoAccertamento, TxtTributo.Text)
    '        End If
    '        'sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='none';"
    '        'RegisterScript(sScript, Me.GetType())
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertato.Page_Load.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
#Region "Griglie"
    ''' <summary>
    ''' Funzione di gestione eventi griglia
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Analisi eventi</em>
    ''' </revision>
    ''' </revisionHistory>
    Protected Sub GrdRowCommand(ByVal sender As Object, e As GridViewCommandEventArgs)
        Try
            If (Session("SOLA_LETTURA") = "1") Then
                sScript = "GestAlert('a', 'warning', '', '', 'Non si hanno i privilegi per eliminare l\'accertamento selezionato.');"
                RegisterScript(sScript, Me.GetType())
            Else
                Dim sScript As String = ""

                If e.CommandName = "RowBind" Then
                    sScript = GetRowBind(CType(sender, Ribes.OPENgov.WebControls.RibesGridView).UniqueID, CInt(e.CommandArgument.ToString()))
                    RegisterScript(sScript, Me.GetType())
                ElseIf e.CommandName = "RowDelete" Then
                    sScript = GetRowDelete(CInt(e.CommandArgument.ToString()))
                    RegisterScript(sScript, Me.GetType())
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertato.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Protected Sub GrdRowCommand(ByVal sender As Object, e As GridViewCommandEventArgs)
    '    Try
    '        If (Session("SOLA_LETTURA") = "1") Then
    '            sScript = "GestAlert('a', 'warning', '', '', 'Non si hanno i privilegi per eliminare l\'accertamento selezionato.');"
    '            RegisterScript(sScript, Me.GetType())
    '        Else
    '            Dim dw As New DataView
    '            Dim dt() As objUIICIAccert
    '            Dim sScript As String = ""
    '            Dim sSrc As String = ""
    '            Dim intIDProvvedimento, annoAccSelezionato As Integer
    '            Dim oListAccertamento() As ObjArticoloAccertamento
    '            Dim bTrovato As Boolean
    '            Dim iRettifica As Integer
    '            Dim ID_PROVVEDIMENTO_RETTIFICA As String
    '            Dim bAnnoMod As Boolean = False
    '            Dim oListAccertamentoOSAP() As ComPlusInterface.OSAPAccertamentoArticolo
    '            Dim IDRow As Integer = CInt(e.CommandArgument.ToString())

    '            If e.CommandName = "RowBind" Then
    '                Dim myRow As GridViewRow
    '                If CType(sender, Ribes.OPENgov.WebControls.RibesGridView).UniqueID = "GrdAtti" Then
    '                    For Each myRow In GrdAtti.Rows
    '                        If CInt(CType(myRow.FindControl("hfIdProv"), HiddenField).Value) = IDRow Then
    '                            objHashTable = Session("HashTableDichAccertamentiTARSU")
    '                            ID_PROVVEDIMENTO_RETTIFICA = Session("ID_PROVVEDIMENTO_RETTIFICA")
    '                            annoAccSelezionato = myRow.Cells(0).Text()
    '                            intIDProvvedimento = CType(myRow.FindControl("hfIdProv"), HiddenField).Value
    '                            codContribuente = CType(myRow.FindControl("hfIdContrib"), HiddenField).Value
    '                            iRettifica = CInt(CType(myRow.FindControl("hfRettifica"), HiddenField).Value)
    '                            If CType(myRow.FindControl("hfIdTributo"), HiddenField).Value = Utility.Costanti.TRIBUTO_TARSU Then
    '                                oListAccertamento = FncGestAccert.TARSU_RicercaAccertato(ConstSession.StringConnection, intIDProvvedimento, bAnnoMod, annoAccertamento, False, objHashTable("TipoTassazione"), objHashTable("HasMaggiorazione"), objHashTable("HasConferimenti"))
    '                                If Not IsNothing(oListAccertamento) Then
    '                                    GrdImmobiliTARSU.DataSource = oListAccertamento
    '                                    GrdImmobiliTARSU.DataBind()
    '                                    GrdImmobiliTARSU.Visible = True
    '                                    Session("oAccertatiGrigliaLocal") = oListAccertamento
    '                                    bTrovato = True
    '                                    If bAnnoMod Then
    '                                        lblNotFound.Text = "La data di inizio e fine, dei singoli immobili, è stata modificata assegnado come anno l'anno di accertamento selezionato"
    '                                        lblNotFound.Visible = True
    '                                    End If
    '                                    '??Session.Remove("oAcceratoXsanzioni")
    '                                Else
    '                                    Session.Remove("oAccertatiGrigliaLocal")
    '                                    GrdImmobiliTARSU.Visible = False
    '                                    bTrovato = False
    '                                End If
    '                                '*** 20130801 - accertamento OSAP ***
    '                            ElseIf CType(myRow.FindControl("hfIdTributo"), HiddenField).Value = Utility.Costanti.TRIBUTO_OSAP Then
    '                                oListAccertamentoOSAP = FncGestAccert.OSAPRicercaArticoliDichAcc("A", intIDProvvedimento)
    '                                If Not IsNothing(oListAccertamentoOSAP) Then
    '                                    GrdImmobiliOSAP.DataSource = oListAccertamentoOSAP
    '                                    GrdImmobiliOSAP.DataBind()
    '                                    GrdImmobiliOSAP.Visible = True
    '                                    'devo svuotare sanzioni ed interessi
    '                                    For Each myPos As ComPlusInterface.OSAPAccertamentoArticolo In oListAccertamentoOSAP
    '                                        myPos.ImpDiffImposta = 0
    '                                        myPos.ImpSanzioni = 0
    '                                        myPos.ImpSanzioniRidotto = 0
    '                                        myPos.ImpInteressi = 0
    '                                    Next
    '                                    Session("oAccertatiGrigliaLocal") = oListAccertamentoOSAP
    '                                    bTrovato = True
    '                                    If bAnnoMod Then
    '                                        lblNotFound.Text = "La data di inizio e fine, dei singoli immobili, è stata modificata assegnado come anno l'anno di accertamento selezionato"
    '                                        lblNotFound.Visible = True
    '                                    End If
    '                                Else
    '                                    Session.Remove("oAccertatiGrigliaLocal")
    '                                    GrdImmobiliOSAP.Visible = False
    '                                    bTrovato = False
    '                                End If
    '                                '*** ***
    '                            Else
    '                                'dt = FncGestAccert.RicercaAccertatoICI(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"), constsession.idente, codContribuente, annoAccSelezionato, intIDProvvedimento, annoAccertamento)
    '                                dt = FncGestAccert.RicercaDicAccICI("A", ConstSession.IdEnte, codContribuente, annoAccSelezionato, intIDProvvedimento, annoAccertamento, ConstSession.StringConnection)
    '                                If dt.GetLength(0) > 0 Then
    '                                    Array.Sort(dt, New Utility.Comparatore(New String() {"IdLegame"}, New Boolean() {Utility.TipoOrdinamento.Crescente}))
    '                                    GrdImmobiliICI.DataSource = dt
    '                                    GrdImmobiliICI.DataBind()
    '                                    GrdImmobiliICI.Visible = True
    '                                    Session("DataTableImmobiliLocal") = dt
    '                                    bTrovato = True
    '                                Else
    '                                    GrdImmobiliICI.Visible = False
    '                                    Session.Remove("DataTableImmobiliLocal")
    '                                    bTrovato = False
    '                                End If
    '                            End If

    '                            If iRettifica = 1 Then
    '                                If ID_PROVVEDIMENTO_RETTIFICA <> intIDProvvedimento.ToString() Then
    '                                    lblNotFound.Text = "Avviso sottoposto a Rettifica. Per poterlo modificare accedere da Gestione Atti"
    '                                    lblNotFound.Visible = True
    '                                    sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='none';"
    '                                    RegisterScript(sScript, Me.GetType())
    '                                Else
    '                                    lblNotFound.Visible = False
    '                                    sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='';"
    '                                    RegisterScript(sScript, Me.GetType())
    '                                End If
    '                            ElseIf iRettifica = 2 Then
    '                                sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='';"
    '                                RegisterScript(sScript, Me.GetType())
    '                            ElseIf iRettifica = 3 Then
    '                                If ID_PROVVEDIMENTO_RETTIFICA <> intIDProvvedimento.ToString() Then
    '                                    If annoAccSelezionato = annoAccertamento Then
    '                                        lblNotFound.Text = "Avviso Notificato."
    '                                        lblNotFound.Visible = True
    '                                        sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='none';"
    '                                        RegisterScript(sScript, Me.GetType())
    '                                    Else
    '                                        If bTrovato = True Then
    '                                            lblNotFound.Visible = False
    '                                            sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='';"
    '                                            RegisterScript(sScript, Me.GetType())
    '                                        Else
    '                                            lblNotFound.Text = "Non è stato trovato nessun immobile accertato o aperto per l'anno " & annoAccertamento
    '                                            lblNotFound.Visible = True
    '                                            sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='none';"
    '                                            RegisterScript(sScript, Me.GetType())
    '                                        End If
    '                                    End If
    '                                Else
    '                                    If bTrovato = True Then
    '                                        lblNotFound.Visible = False
    '                                        sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='';"
    '                                        RegisterScript(sScript, Me.GetType())
    '                                    Else
    '                                        lblNotFound.Text = "Non è stato trovato nessun immobile accertato o aperto per l'anno " & annoAccertamento
    '                                        lblNotFound.Visible = True
    '                                        sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='none';"
    '                                        RegisterScript(sScript, Me.GetType())
    '                                    End If
    '                                End If
    '                            ElseIf bTrovato = True Then
    '                                lblNotFound.Visible = False
    '                                sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='';"
    '                                RegisterScript(sScript, Me.GetType())
    '                            Else
    '                                lblNotFound.Text = "Non è stato trovato nessun immobile accertato o aperto per l'anno " & annoAccertamento
    '                                lblNotFound.Visible = True
    '                                sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='none';"
    '                                RegisterScript(sScript, Me.GetType())
    '                            End If
    '                            Return
    '                        End If
    '                    Next
    '                Else
    '                    If TxtTributo.Text = Utility.Costanti.TRIBUTO_TARSU Then
    '                        'TARSU
    '                        Session("oAccertatiGriglia") = Session("oAccertatiGrigliaLocal")
    '                        sSrc = "../GestioneAccertamentiTARSU/SearchAccertatiTARSU.aspx"
    '                        '*** 20130801 - accertamento OSAP ***
    '                    ElseIf TxtTributo.Text = Utility.Costanti.TRIBUTO_OSAP Or TxtTributo.Text = "OSAP" Then
    '                        'TARSU
    '                        Session("oAccertatiGriglia") = Session("oAccertatiGrigliaLocal")
    '                        sSrc = "../GestioneAccertamentiOSAP/SearchDatiAccertatoOSAP.aspx"
    '                        '*** ***
    '                    Else
    '                        'ICI
    '                        Session("DataTableImmobiliDaAccertare") = Session("DataTableImmobiliLocal")
    '                        If (ConstSession.CodTributo <> Utility.Costanti.TRIBUTO_TASI) Then
    '                            sSrc = "grdAccertato.aspx"
    '                        Else
    '                            sSrc = "../GestioneAccertamentiTASI/SearchDatiAccertato.aspx"
    '                        End If
    '                    End If
    '                    sScript += "parent.parent.opener.location.href='" & sSrc & "';"
    '                    sScript += "parent.window.close();"
    '                    RegisterScript(sScript, Me.GetType())
    '                End If
    '            ElseIf e.CommandName = "RowDelete" Then
    '                Dim myRow As GridViewRow
    '                For Each myRow In GrdAtti.Rows
    '                    If CInt(CType(myRow.FindControl("hfIdProv"), HiddenField).Value) = IDRow Then
    '                        If CType(myRow.FindControl("hfIdTributo"), HiddenField).Value = Utility.Costanti.TRIBUTO_TARSU Then
    '                            If FncGestAccert.DeleteAttoTARSU(CInt(CType(myRow.FindControl("hfIdProv"), HiddenField).Value()), myRow.Cells(0).Text(), CType(myRow.FindControl("hfIdContrib"), HiddenField).Value) = False Then
    '                                sScript = "GestAlert('a', 'danger', '', '', 'Si e\' verificato un\'errore durante la cancellazione!');"
    '                            Else
    '                                CaricaGriglia(CType(myRow.FindControl("hfIdContrib"), HiddenField).Value, myRow.Cells(0).Text(), CType(myRow.FindControl("hfIdTributo"), HiddenField).Value)
    '                                sScript = "GestAlert('a', 'success', '', '', 'Eliminazione effettuata con successo!');"
    '                            End If
    '                            '*** 20130801 - accertamento OSAP ***
    '                        ElseIf CType(myRow.FindControl("hfIdTributo"), HiddenField).Value = Utility.Costanti.TRIBUTO_OSAP Then
    '                            If FncGestAccert.DeleteAttoOSAP(ConfigurationManager.AppSettings("connectionStringOPENgovPROVVEDIMENTI"), CInt(CType(myRow.FindControl("hfIdProv"), HiddenField).Value())) = False Then
    '                                sScript = "GestAlert('a', 'danger', '', '', 'Si e\' verificato un\'errore durante la cancellazione!');"
    '                            Else
    '                                CaricaGriglia(CType(myRow.FindControl("hfIdContrib"), HiddenField).Value, myRow.Cells(0).Text(), CType(myRow.FindControl("hfIdTributo"), HiddenField).Value)
    '                                sScript = "GestAlert('a', 'success', '', '', 'Eliminazione effettuata con successo!');"
    '                            End If
    '                            '*** ***
    '                        Else
    '                            If FncGestAccert.DeleteAttoICI(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"), ConstSession.IdEnte, CInt(CType(myRow.FindControl("hfIdProv"), HiddenField).Value()), CInt(CType(myRow.FindControl("hfIdProc"), HiddenField).Value), myRow.Cells(0).Text(), CType(myRow.FindControl("hfIdContrib"), HiddenField).Value) = False Then
    '                                sScript = "GestAlert('a', 'danger', '', '', 'Si e\' verificato un\'errore durante la cancellazione!');"
    '                            Else
    '                                CaricaGriglia(CType(myRow.FindControl("hfIdContrib"), HiddenField).Value, myRow.Cells(0).Text(), CType(myRow.FindControl("hfIdTributo"), HiddenField).Value)
    '                                sScript = "GestAlert('a', 'success', '', '', 'Eliminazione effettuata con successo!');"
    '                            End If
    '                        End If
    '                        RegisterScript(sScript, Me.GetType())
    '                        Return
    '                    End If
    '                Next
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertato.GrdRowCommand.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    'Private Sub GrdAtti_ItemDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdAtti.ItemDataBound
    '    Dim sStato As String
    '    Dim objImgButt As System.Web.UI.WebControls.ImageButton
    '    Dim iRettificato As Integer
    '    Try
    '        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '            sStato = FncGrd.Stato(e.Item.Cells(idCelle.grdAtti.cellID_PROVVEDIMENTO).Text, e.Item.Cells(idCelle.grdAtti.cellCOD_TIPO_PROVVEDIMENTO).Text)
    '            iRettificato = CInt(e.Item.Cells(13).Text)
    '            If sStato = "NOTIFICATO" Or iRettificato > 0 Or Session("SOLA_LETTURA") = "1" Then
    '                objImgButt = CType(e.Item.Cells(idCelle.grdAtti.cellBottoneDelete).Controls.Item(1), System.Web.UI.WebControls.ImageButton)
    '                objImgButt.Visible = False
    '            Else
    '                objImgButt = CType(e.Item.Cells(idCelle.grdAtti.cellBottoneDelete).Controls.Item(1), System.Web.UI.WebControls.ImageButton)
    '                objImgButt.Attributes.Add("title", "Premere questo pulsante per eliminare l'Atto")
    '                objImgButt.Attributes.Add("onclick", "return DeleteAtto();")
    '                e.Item.Cells(idCelle.grdAtti.cellBottoneDelete).Attributes.Clear()
    '            End If
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertato.GrdAtti_ItemDataBound.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Private Sub GrdAtti_DeleteCommand(ByVal s As Object, ByVal e As DataGridCommandEventArgs) Handles GrdAtti.DeleteCommand
    '    Dim sScript As String

    '    Try
    '        If (Session("SOLA_LETTURA") = "1") Then
    '            sScript = "<script>"
    '            sScript += "alert('Non si hanno i privilegi per eliminare l\'accertamento selezionato.');"
    '            sScript += "</script>"
    '            RegisterScript("del", sScript)
    '        Else
    '            If Not GrdAtti.SelectedItem Is Nothing Then
    '                If GrdAtti.SelectedItem.Cells(9).Text() = Utility.Costanti.TRIBUTO_TARSU Then
    '                    If FncGestAccert.DeleteAttoTARSU(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"), CInt(GrdAtti.SelectedItem.Cells(6).Text()), GrdAtti.SelectedItem.Cells(0).Text(), GrdAtti.SelectedItem.Cells(10).Text()) = False Then
    '                        sScript = "alert('Si e\' verificato un\'errore durante la cancellazione!');"
    '                    Else
    '                        CaricaGriglia(GrdAtti.SelectedItem.Cells(10).Text(), GrdAtti.SelectedItem.Cells(0).Text(), GrdAtti.SelectedItem.Cells(9).Text())
    '                        sScript = "alert('Eliminazione effettuata con successo!');"
    '                    End If
    '                    '*** 20130801 - accertamento OSAP ***
    '                ElseIf GrdAtti.SelectedItem.Cells(9).Text() = Utility.Costanti.TRIBUTO_OSAP Then
    '                    If FncGestAccert.DeleteAttoOSAP(ConfigurationManager.AppSettings("connectionStringOPENgovPROVVEDIMENTI"), CInt(GrdAtti.SelectedItem.Cells(6).Text())) = False Then
    '                        sScript = "alert('Si e\' verificato un\'errore durante la cancellazione!');"
    '                    Else
    '                        CaricaGriglia(GrdAtti.SelectedItem.Cells(10).Text(), GrdAtti.SelectedItem.Cells(0).Text(), GrdAtti.SelectedItem.Cells(9).Text())
    '                        sScript = "alert('Eliminazione effettuata con successo!');"
    '                    End If
    '                    '*** ***
    '                Else
    '                    If FncGestAccert.DeleteAttoICI(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"), ConstSession.IdEnte, CInt(GrdAtti.SelectedItem.Cells(6).Text()), CInt(GrdAtti.SelectedItem.Cells(11).Text()), GrdAtti.SelectedItem.Cells(0).Text(), GrdAtti.SelectedItem.Cells(10).Text()) = False Then
    '                        sScript = "alert('Si e\' verificato un\'errore durante la cancellazione!');"
    '                    Else
    '                        CaricaGriglia(GrdAtti.SelectedItem.Cells(10).Text(), GrdAtti.SelectedItem.Cells(0).Text(), GrdAtti.SelectedItem.Cells(9).Text())
    '                        sScript = "alert('Eliminazione effettuata con successo!');"
    '                    End If
    '                End If
    '                RegisterScript("del", "<script>" + sScript + "</script>")
    '            Else
    '                sScript = "<script>"
    '                sScript += "alert('Selezionare l\'accertamento da eliminare.');"
    '                sScript += "</script>"
    '                RegisterScript("del", sScript)
    '            End If
    '        End If

    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertato.GrdAtti_DeleteCommand.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''*** 20140701 - IMU/TARES ***
    ''Private Sub GrdAtti_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GrdAtti.SelectedIndexChanged
    ''    Dim dw As New DataView
    ''    Dim dt As New DataTable
    ''    Dim sScript, sStato As String
    ''    Dim intIDProvvedimento, annoAccSelezionato As Integer
    ''    Dim oListAccertamento() As OggettoArticoloRuolo
    ''    'Dim oListAccertamento() As OggettoArticoloRuoloAccertamento
    ''    Dim bTrovato As Boolean
    ''    Dim iRettifica As Integer
    ''    Dim ID_PROVVEDIMENTO_RETTIFICA As String
    ''    Dim bAnnoMod As Boolean = False
    ''    Dim oListAccertamentoOSAP() As ComPlusInterface.OSAPAccertamentoArticolo

    ''    Try
    ''        ID_PROVVEDIMENTO_RETTIFICA = Session("ID_PROVVEDIMENTO_RETTIFICA")
    ''        annoAccSelezionato = GrdAtti.SelectedItem.Cells(0).Text()
    ''        intIDProvvedimento = CInt(GrdAtti.SelectedItem.Cells(6).Text())
    ''        codContribuente = GrdAtti.SelectedItem.Cells(10).Text()
    ''        iRettifica = CInt(GrdAtti.SelectedItem.Cells(13).Text())
    ''        If GrdAtti.SelectedItem.Cells(9).Text() = "0434" Then
    ''            oListAccertamento = FncGestAccert.RicercaAccertatoTARSU(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"), intIDProvvedimento, bAnnoMod, annoAccertamento)
    ''            If Not IsNothing(oListAccertamento) Then
    ''                GrdImmobiliTARSU.start_index = 0
    ''                GrdImmobiliTARSU.DataSource = oListAccertamento
    ''                GrdImmobiliTARSU.DataBind()
    ''                GrdImmobiliTARSU.Visible = True
    ''                Session("oAccertatiGrigliaLocal") = oListAccertamento
    ''                bTrovato = True
    ''                If bAnnoMod Then
    ''                    lblNotFound.Text = "La data di inizio e fine, dei singoli immobili, è stata modificata assegnado come anno l'anno di accertamento selezionato"
    ''                    lblNotFound.Visible = True
    ''                End If
    ''                '??Session.Remove("oAcceratoXsanzioni")
    ''            Else
    ''                Session.Remove("oAccertatiGrigliaLocal")
    ''                GrdImmobiliTARSU.Visible = False
    ''                bTrovato = False
    ''            End If
    ''            '*** 20130801 - accertamento OSAP ***
    ''        ElseIf GrdAtti.SelectedItem.Cells(9).Text() = Costanti.TRIBUTO_OSAP Then
    ''            oListAccertamentoOSAP = FncGestAccert.OSAPRicercaArticoliDichAcc("A", intIDProvvedimento)
    ''            If Not IsNothing(oListAccertamentoOSAP) Then
    ''                GrdImmobiliOSAP.start_index = 0
    ''                GrdImmobiliOSAP.DataSource = oListAccertamentoOSAP
    ''                GrdImmobiliOSAP.DataBind()
    ''                GrdImmobiliOSAP.Visible = True
    ''                Session("oAccertatiGrigliaLocal") = oListAccertamentoOSAP
    ''                bTrovato = True
    ''                If bAnnoMod Then
    ''                    lblNotFound.Text = "La data di inizio e fine, dei singoli immobili, è stata modificata assegnado come anno l'anno di accertamento selezionato"
    ''                    lblNotFound.Visible = True
    ''                End If
    ''            Else
    ''                Session.Remove("oAccertatiGrigliaLocal")
    ''                GrdImmobiliOSAP.Visible = False
    ''                bTrovato = False
    ''            End If
    ''            '*** ***
    ''        Else
    ''            'dt = FncGestAccert.RicercaAccertatoICI(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"), constsession.idente, codContribuente, annoAccSelezionato, intIDProvvedimento, annoAccertamento)
    ''            dt = FncGestAccert.RicercaAccertatoICI(ConstSession.IdEnte, codContribuente, annoAccSelezionato, intIDProvvedimento, annoAccertamento, ConstSession.StringConnection)
    ''            If dt.Rows.Count > 0 Then
    ''                dw = dt.DefaultView
    ''                dw.Sort = "IDLEGAME, FOGLIO, NUMERO, SUBALTERNO"
    ''                GrdImmobiliICI.start_index = 0
    ''                GrdImmobiliICI.DataSource = dw
    ''                GrdImmobiliICI.DataBind()
    ''                GrdImmobiliICI.Visible = True
    ''                Session("DataTableImmobiliLocal") = dt
    ''                bTrovato = True
    ''            Else
    ''                GrdImmobiliICI.Visible = False
    ''                Session.Remove("DataTableImmobiliLocal")
    ''                bTrovato = False
    ''            End If
    ''        End If

    ''        If iRettifica = 1 Then
    ''            If ID_PROVVEDIMENTO_RETTIFICA <> intIDProvvedimento.ToString() Then
    ''                lblNotFound.Text = "Avviso sottoposto a Rettifica. Per poterlo modificare accedere da Gestione Atti"
    ''                lblNotFound.Visible = True
    ''                sScript = "<script>"
    ''                sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='none';"
    ''                sScript += "</script>"
    ''                RegisterScript("nascondi", sScript)
    ''            Else
    ''                lblNotFound.Visible = False
    ''                sScript = "<script>"
    ''                sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='';"
    ''                sScript += "</script>"
    ''                RegisterScript("visualizza", sScript)
    ''            End If

    ''        ElseIf iRettifica = 2 Then
    ''            'If ID_PROVVEDIMENTO_RETTIFICA <> intIDProvvedimento.ToString() Then
    ''            '    lblNotFound.Text = "Avviso di Autotutela. Per poterlo modificare accedere all'avviso Originale da Gestione Atti"
    ''            '    lblNotFound.Visible = True
    ''            '    sScript = "<script>"
    ''            '    sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='none';"
    ''            '    sScript += "</script>"
    ''            '    RegisterScript("nascondi", sScript)
    ''            'Else
    ''            '    lblNotFound.Visible = False
    ''            '    sScript = "<script>"
    ''            '    sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='';"
    ''            '    sScript += "</script>"
    ''            '    RegisterScript("visualizza", sScript)
    ''            'End If
    ''            'lblNotFound.Visible = False
    ''            sScript = "<script>"
    ''            sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='';"
    ''            sScript += "</script>"
    ''            RegisterScript("visualizza", sScript)
    ''        ElseIf iRettifica = 3 Then
    ''            If ID_PROVVEDIMENTO_RETTIFICA <> intIDProvvedimento.ToString() Then
    ''                If annoAccSelezionato = annoAccertamento Then
    ''                    lblNotFound.Text = "Avviso Notificato."
    ''                    lblNotFound.Visible = True
    ''                    sScript = "<script>"
    ''                    sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='none';"
    ''                    sScript += "</script>"
    ''                    RegisterScript("nascondi", sScript)
    ''                Else
    ''                    If bTrovato = True Then
    ''                        lblNotFound.Visible = False
    ''                        sScript = "<script>"
    ''                        sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='';"
    ''                        sScript += "</script>"
    ''                        RegisterScript("visualizza", sScript)
    ''                    Else
    ''                        lblNotFound.Text = "Non è stato trovato nessun immobile accertato o aperto per l'anno " & annoAccertamento
    ''                        lblNotFound.Visible = True
    ''                        sScript = "<script>"
    ''                        sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='none';"
    ''                        sScript += "</script>"
    ''                        RegisterScript("nascondi", sScript)
    ''                    End If
    ''                End If
    ''            Else
    ''                If bTrovato = True Then
    ''                    lblNotFound.Visible = False
    ''                    sScript = "<script>"
    ''                    sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='';"
    ''                    sScript += "</script>"
    ''                    RegisterScript("visualizza", sScript)
    ''                Else
    ''                    lblNotFound.Text = "Non è stato trovato nessun immobile accertato o aperto per l'anno " & annoAccertamento
    ''                    lblNotFound.Visible = True
    ''                    sScript = "<script>"
    ''                    sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='none';"
    ''                    sScript += "</script>"
    ''                    RegisterScript("nascondi", sScript)
    ''                End If
    ''            End If
    ''        ElseIf bTrovato = True Then
    ''            lblNotFound.Visible = False
    ''            sScript = "<script>"
    ''            sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='';"
    ''            sScript += "</script>"
    ''            RegisterScript("visualizza", sScript)
    ''        Else
    ''            lblNotFound.Text = "Non è stato trovato nessun immobile accertato o aperto per l'anno " & annoAccertamento
    ''            lblNotFound.Visible = True
    ''            sScript = "<script>"
    ''            sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='none';"
    ''            sScript += "</script>"
    ''            RegisterScript("nascondi", sScript)
    ''        End If
    ''    Catch Err As Exception
    ''        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertato.GrdAtti_SelectedIndexChanged.errore: ", Err)
    ''        Response.Redirect("../../PaginaErrore.aspx")
    ''    End Try
    ''End Sub

    'Private Sub GrdAtti_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GrdAtti.SelectedIndexChanged
    '    Dim dw As New DataView
    '    Dim dt As New DataTable
    '    Dim sScript, sStato As String
    '    Dim intIDProvvedimento, annoAccSelezionato As Integer
    '    Dim oListAccertamento() As ObjArticoloAccertamento
    '    Dim bTrovato As Boolean
    '    Dim iRettifica As Integer
    '    Dim ID_PROVVEDIMENTO_RETTIFICA As String
    '    Dim bAnnoMod As Boolean = False
    '    Dim oListAccertamentoOSAP() As ComPlusInterface.OSAPAccertamentoArticolo

    '    Try
    '        objHashTable = Session("HashTableDichAccertamentiTARSU")
    '        ID_PROVVEDIMENTO_RETTIFICA = Session("ID_PROVVEDIMENTO_RETTIFICA")
    '        annoAccSelezionato = GrdAtti.SelectedItem.Cells(0).Text()
    '        intIDProvvedimento = CInt(GrdAtti.SelectedItem.Cells(6).Text())
    '        codContribuente = GrdAtti.SelectedItem.Cells(10).Text()
    '        iRettifica = CInt(GrdAtti.SelectedItem.Cells(13).Text())
    '        If GrdAtti.SelectedItem.Cells(9).Text() = Utility.Costanti.TRIBUTO_TARSU Then
    '            oListAccertamento = FncGestAccert.TARSU_RicercaAccertato(ConstSession.StringConnection, intIDProvvedimento, bAnnoMod, annoAccertamento, False, objHashTable("TipoTassazione"), objHashTable("HasMaggiorazione"), objHashTable("HasConferimenti"))
    '            If Not IsNothing(oListAccertamento) Then
    '                GrdImmobiliTARSU.start_index = 0
    '                GrdImmobiliTARSU.DataSource = oListAccertamento
    '                GrdImmobiliTARSU.DataBind()
    '                GrdImmobiliTARSU.Visible = True
    '                Session("oAccertatiGrigliaLocal") = oListAccertamento
    '                bTrovato = True
    '                If bAnnoMod Then
    '                    lblNotFound.Text = "La data di inizio e fine, dei singoli immobili, è stata modificata assegnado come anno l'anno di accertamento selezionato"
    '                    lblNotFound.Visible = True
    '                End If
    '                '??Session.Remove("oAcceratoXsanzioni")
    '            Else
    '                Session.Remove("oAccertatiGrigliaLocal")
    '                GrdImmobiliTARSU.Visible = False
    '                bTrovato = False
    '            End If
    '            '*** 20130801 - accertamento OSAP ***
    '        ElseIf GrdAtti.SelectedItem.Cells(9).Text() = Utility.Costanti.TRIBUTO_OSAP Then
    '            oListAccertamentoOSAP = FncGestAccert.OSAPRicercaArticoliDichAcc("A", intIDProvvedimento)
    '            If Not IsNothing(oListAccertamentoOSAP) Then
    '                GrdImmobiliOSAP.start_index = 0
    '                GrdImmobiliOSAP.DataSource = oListAccertamentoOSAP
    '                GrdImmobiliOSAP.DataBind()
    '                GrdImmobiliOSAP.Visible = True
    '                Session("oAccertatiGrigliaLocal") = oListAccertamentoOSAP
    '                bTrovato = True
    '                If bAnnoMod Then
    '                    lblNotFound.Text = "La data di inizio e fine, dei singoli immobili, è stata modificata assegnado come anno l'anno di accertamento selezionato"
    '                    lblNotFound.Visible = True
    '                End If
    '            Else
    '                Session.Remove("oAccertatiGrigliaLocal")
    '                GrdImmobiliOSAP.Visible = False
    '                bTrovato = False
    '            End If
    '            '*** ***
    '        Else
    '            'dt = FncGestAccert.RicercaAccertatoICI(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"), constsession.idente, codContribuente, annoAccSelezionato, intIDProvvedimento, annoAccertamento)
    '            dt = FncGestAccert.RicercaAccertatoICI(ConstSession.IdEnte, codContribuente, annoAccSelezionato, intIDProvvedimento, annoAccertamento, ConstSession.StringConnection)
    '            If dt.Rows.Count > 0 Then
    '                dw = dt.DefaultView
    '                dw.Sort = "IDLEGAME, FOGLIO, NUMERO, SUBALTERNO"
    '                GrdImmobiliICI.start_index = 0
    '                GrdImmobiliICI.DataSource = dw
    '                GrdImmobiliICI.DataBind()
    '                GrdImmobiliICI.Visible = True
    '                Session("DataTableImmobiliLocal") = dt
    '                bTrovato = True
    '            Else
    '                GrdImmobiliICI.Visible = False
    '                Session.Remove("DataTableImmobiliLocal")
    '                bTrovato = False
    '            End If
    '        End If

    '        If iRettifica = 1 Then
    '            If ID_PROVVEDIMENTO_RETTIFICA <> intIDProvvedimento.ToString() Then
    '                lblNotFound.Text = "Avviso sottoposto a Rettifica. Per poterlo modificare accedere da Gestione Atti"
    '                lblNotFound.Visible = True
    '                sScript = "<script>"
    '                sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='none';"
    '                sScript += "</script>"
    '                RegisterScript("nascondi", sScript)
    '            Else
    '                lblNotFound.Visible = False
    '                sScript = "<script>"
    '                sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='';"
    '                sScript += "</script>"
    '                RegisterScript("visualizza", sScript)
    '            End If

    '        ElseIf iRettifica = 2 Then
    '            sScript = "<script>"
    '            sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='';"
    '            sScript += "</script>"
    '            RegisterScript("visualizza", sScript)
    '        ElseIf iRettifica = 3 Then
    '            If ID_PROVVEDIMENTO_RETTIFICA <> intIDProvvedimento.ToString() Then
    '                If annoAccSelezionato = annoAccertamento Then
    '                    lblNotFound.Text = "Avviso Notificato."
    '                    lblNotFound.Visible = True
    '                    sScript = "<script>"
    '                    sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='none';"
    '                    sScript += "</script>"
    '                    RegisterScript("nascondi", sScript)
    '                Else
    '                    If bTrovato = True Then
    '                        lblNotFound.Visible = False
    '                        sScript = "<script>"
    '                        sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='';"
    '                        sScript += "</script>"
    '                        RegisterScript("visualizza", sScript)
    '                    Else
    '                        lblNotFound.Text = "Non è stato trovato nessun immobile accertato o aperto per l'anno " & annoAccertamento
    '                        lblNotFound.Visible = True
    '                        sScript = "<script>"
    '                        sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='none';"
    '                        sScript += "</script>"
    '                        RegisterScript("nascondi", sScript)
    '                    End If
    '                End If
    '            Else
    '                If bTrovato = True Then
    '                    lblNotFound.Visible = False
    '                    sScript = "<script>"
    '                    sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='';"
    '                    sScript += "</script>"
    '                    RegisterScript("visualizza", sScript)
    '                Else
    '                    lblNotFound.Text = "Non è stato trovato nessun immobile accertato o aperto per l'anno " & annoAccertamento
    '                    lblNotFound.Visible = True
    '                    sScript = "<script>"
    '                    sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='none';"
    '                    sScript += "</script>"
    '                    RegisterScript("nascondi", sScript)
    '                End If
    '            End If
    '        ElseIf bTrovato = True Then
    '            lblNotFound.Visible = False
    '            sScript = "<script>"
    '            sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='';"
    '            sScript += "</script>"
    '            RegisterScript("visualizza", sScript)
    '        Else
    '            lblNotFound.Text = "Non è stato trovato nessun immobile accertato o aperto per l'anno " & annoAccertamento
    '            lblNotFound.Visible = True
    '            sScript = "<script>"
    '            sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='none';"
    '            sScript += "</script>"
    '            RegisterScript("nascondi", sScript)
    '        End If
    '    Catch Err As Exception
    '         Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertato.GrdAtti_SelectedIndexChanged.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''*** ***
    'Private Sub btnAssocia_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAssocia.Click
    '	Dim sSrc, sScript As String
    '	Try

    '           If TxtTributo.Text = Utility.Costanti.TRIBUTO_TARSU Then
    '               'TARSU
    '               Session("oAccertatiGriglia") = Session("oAccertatiGrigliaLocal")
    '               sSrc = "../GestioneAccertamentiTARSU/SearchAccertatiTARSU.aspx"
    '               '*** 20130801 - accertamento OSAP ***
    '           ElseIf TxtTributo.Text = Utility.Costanti.TRIBUTO_OSAP Or TxtTributo.Text = "OSAP" Then
    '               'TARSU
    '               Session("oAccertatiGriglia") = Session("oAccertatiGrigliaLocal")
    '               sSrc = "../GestioneAccertamentiOSAP/SearchDatiAccertatoOSAP.aspx"
    '               '*** ***
    '           Else
    '               'ICI
    '               Session("DataTableImmobili") = Session("DataTableImmobiliLocal")
    '               sSrc = "grdAccertato.aspx"
    '           End If
    '		sScript = "<script>"
    '		sScript += "parent.opener.document.getElementById('loadGridAccertato').src='" & sSrc & "';"
    '		sScript += "parent.window.close();"
    '		sScript += "</script>"
    '		RegisterScript("", sScript)
    '	Catch Err As Exception
    '		 Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertato.btnAssocia_Click.errore: ", Err)
    '		Response.Redirect("../../PaginaErrore.aspx")
    '	End Try
    'End Sub
#End Region
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="codContribuente"></param>
    ''' <param name="anno"></param>
    ''' <param name="sTributo"></param>
    Private Sub CaricaGriglia(ByVal codContribuente As Integer, ByVal anno As Integer, ByVal sTributo As String)
        Dim dw As DataView
        Dim dt As DataTable
        Dim ID_PROVVEDIMENTO_RETTIFICA As String
        Dim objDS As DataSet

        Try
            ID_PROVVEDIMENTO_RETTIFICA = Session("ID_PROVVEDIMENTO_RETTIFICA")
            '*** 20140701 - IMU/TARES ***
            'dt = FncGestAccert.RicercaAtti(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"), ConstSession.IdEnte, codContribuente, annoAccertamento, sTributo, ID_PROVVEDIMENTO_RETTIFICA, objDS, objHashTable)
            dt = FncGestAccert.RicercaAtti(ConstSession.IdEnte, codContribuente, annoAccertamento, sTributo, ID_PROVVEDIMENTO_RETTIFICA, objDS, objHashTable)
            '*** ***
            If dt.Rows.Count > 0 Then
                Session.Remove("PROVVEDIMENTI_CONTRIBUENTE")
                Session("PROVVEDIMENTI_CONTRIBUENTE") = objDS
                Session("HASH_PROVVEDIMENTI_CONTRIBUENTE") = objHashTable
                dw = dt.DefaultView
                GrdAtti.DataSource = dw
                GrdAtti.DataBind()
                GrdAtti.Visible = True
            Else
                lblNotFound.Text = "Non è stato trovato nessun Atto non definitivo"
                lblNotFound.Visible = True
                GrdAtti.Visible = False
            End If
            GrdImmobiliTARSU.Visible = False
            GrdImmobiliICI.Visible = False
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchAccertato.CaricaGriglia.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub creaDataSet()
    '    Dim workTable As New DataTable("IMMOBILI")

    '    workTable.Columns.Add("DAL")                                      '0
    '    workTable.Columns.Add("AL")                                    '1
    '    workTable.Columns.Add("FOGLIO")                                '2
    '    workTable.Columns.Add("NUMERO")                                '3
    '    workTable.Columns.Add("SUBALTERNO")                            '4
    '    workTable.Columns.Add("CATEGORIA")                              '5
    '    workTable.Columns.Add("CLASSE")                                '6
    '    workTable.Columns.Add("CONSISTENZA")                              '7
    '    workTable.Columns.Add("TR")                                    '8
    '    workTable.Columns.Add("RENDITA")                                  '9
    '    workTable.Columns.Add("RENDITA_VALORE")                        '10
    '    workTable.Columns.Add("ZONA")                                    '11
    '    workTable.Columns.Add("IDSANZIONI")                            '12
    '    workTable.Columns.Add("IDLEGAME", System.Type.GetType("System.Int32"))                                     '13
    '    workTable.Columns.Add("ICICALCOLATO")                            '14
    '    workTable.Columns.Add("PROGRESSIVO")                              '15
    '    workTable.Columns.Add("SANZIONI")                                '16
    '    workTable.Columns.Add("INTERESSI")                              '17
    '    workTable.Columns.Add("PERCPOSSESSO")                            '18
    '    workTable.Columns.Add("TITPOSSESSO")                              '19
    '    workTable.Columns.Add("FLAG_PRINCIPALE")                          '20
    '    workTable.Columns.Add("FLAG_PERTINENZA")                          '21
    '    workTable.Columns.Add("FLAG_RIDOTTO")                            '22
    '    workTable.Columns.Add("IDIMMOBILEPERTINENZA")                    '23
    '    workTable.Columns.Add("SEZIONE")                                  '24
    '    workTable.Columns.Add("INDIRIZZO")                              '25
    '    workTable.Columns.Add("CODTITPOSSESSO")                        '26
    '    workTable.Columns.Add("CODTIPORENDITA")                        '27
    '    workTable.Columns.Add("ICICALCOLATOACCONTO")                      '28
    '    workTable.Columns.Add("ICICALCOLATOSALDO")                      '29
    '    workTable.Columns.Add("NUMEROUTILIZZATORI")                    '30
    '    workTable.Columns.Add("ID_IMMOBILE_ORIGINALE_DICHIARATO")        '31
    '    workTable.Columns.Add("CODICE_VIA")                            '32
    '    workTable.Columns.Add("CALCOLA_INTERESSI")                      '33
    '    workTable.Columns.Add("DESC_TIPO_RENDITA")                      '34
    '    workTable.Columns.Add("DESC_SANZIONE")                          '35
    '    workTable.Columns.Add("ID_IMMOBILE_ACCERTATO")                  '36
    '    workTable.Columns.Add("CODCOMUNE")                              '37
    '    workTable.Columns.Add("COMUNE")                                '38
    '    workTable.Columns.Add("ESPCIVICO")                              '39
    '    workTable.Columns.Add("INTERNO")                                  '40
    '    workTable.Columns.Add("NUMEROCIVICO")                            '41
    '    workTable.Columns.Add("PIANO")                                  '42
    '    workTable.Columns.Add("SCALA")                                  '43
    '    workTable.Columns.Add("INTERNO1")                                '44
    '    workTable.Columns.Add("BARRATO")                                  '45
    '    workTable.Columns.Add("MESIESCLUSIONEESENZIONE")                  '46
    '    workTable.Columns.Add("FLAG_ESENTE")                              '47
    '    workTable.Columns.Add("MESIRIDUZIONE")                          '48
    '    workTable.Columns.Add("DIFFIMPOSTA")                              '49
    '    workTable.Columns.Add("TOTALE")                                '50
    '    workTable.Columns.Add("ICI_VALORE_ALIQUOTA")       '51
    '    '*** 20120530 - IMU ***
    '    workTable.Columns.Add("COLTIVATOREDIRETTO")    '52
    '    workTable.Columns.Add("NUMEROFIGLI")       '53
    '    workTable.Columns.Add("PERCENTCARICOFIGLI")    '54
    '    '*** ***

    '    Session("DataTableImmobiliDaAccertare") = workTable
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnEsci_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEsci.Click
        Session("DataTableImmobiliLocal") = Nothing
        Session("oAccertatiGrigliaLocal") = Nothing
        'Session.Remove("DataTableImmobiliAccertati")
    End Sub
    ''' <summary>
    ''' Funzione per la ricerca e visualizzazione degli immobili legati all'atto selezionato
    ''' </summary>
    ''' <param name="mySender">String nome della griglia di origine</param>
    ''' <param name="IDRow">Int identificativo della riga</param>
    ''' <returns></returns>
    Private Function GetRowBind(mySender As String, IDRow As Integer) As String
        Dim dt() As objUIICIAccert
        Dim intIDProvvedimento, annoAccSelezionato As Integer
        Dim oListAccertamento() As ObjArticoloAccertamento
        Dim bTrovato As Boolean
        Dim iRettifica As Integer
        Dim ID_PROVVEDIMENTO_RETTIFICA As String
        Dim oListAccertamentoOSAP() As ComPlusInterface.OSAPAccertamentoArticolo
        Dim sSrc As String = ""
        Dim bAnnoMod As Boolean = False
        Dim myRow As GridViewRow
        Try
            If mySender = "GrdAtti" Then
                For Each myRow In GrdAtti.Rows
                    If CInt(CType(myRow.FindControl("hfIdProv"), HiddenField).Value) = IDRow Then
                        objHashTable = Session("HashTableDichAccertamentiTARSU")
                        ID_PROVVEDIMENTO_RETTIFICA = Session("ID_PROVVEDIMENTO_RETTIFICA")
                        annoAccSelezionato = myRow.Cells(0).Text()
                        intIDProvvedimento = CType(myRow.FindControl("hfIdProv"), HiddenField).Value
                        codContribuente = CType(myRow.FindControl("hfIdContrib"), HiddenField).Value
                        iRettifica = CInt(CType(myRow.FindControl("hfRettifica"), HiddenField).Value)
                        If CType(myRow.FindControl("hfIdTributo"), HiddenField).Value = Utility.Costanti.TRIBUTO_TARSU Then
                            oListAccertamento = FncGestAccert.TARSU_RicercaAccertato(ConstSession.StringConnection, intIDProvvedimento, bAnnoMod, annoAccertamento, False, objHashTable("TipoTassazione"), objHashTable("HasMaggiorazione"), objHashTable("HasConferimenti"))
                            If Not IsNothing(oListAccertamento) Then
                                GrdImmobiliTARSU.DataSource = oListAccertamento
                                GrdImmobiliTARSU.DataBind()
                                GrdImmobiliTARSU.Visible = True
                                Session("oAccertatiGrigliaLocal") = oListAccertamento
                                bTrovato = True
                                If bAnnoMod Then
                                    lblNotFound.Text = "La data di inizio e fine, dei singoli immobili, è stata modificata assegnado come anno l'anno di accertamento selezionato"
                                    lblNotFound.Visible = True
                                End If
                            Else
                                Session.Remove("oAccertatiGrigliaLocal")
                                GrdImmobiliTARSU.Visible = False
                                bTrovato = False
                            End If
                            '*** 20130801 - accertamento OSAP ***
                        ElseIf CType(myRow.FindControl("hfIdTributo"), HiddenField).Value = Utility.Costanti.TRIBUTO_OSAP Then
                            oListAccertamentoOSAP = FncGestAccert.OSAPRicercaArticoliDichAcc("A", intIDProvvedimento)
                            If Not IsNothing(oListAccertamentoOSAP) Then
                                GrdImmobiliOSAP.DataSource = oListAccertamentoOSAP
                                GrdImmobiliOSAP.DataBind()
                                GrdImmobiliOSAP.Visible = True
                                'devo svuotare sanzioni ed interessi
                                For Each myPos As ComPlusInterface.OSAPAccertamentoArticolo In oListAccertamentoOSAP
                                    myPos.ImpDiffImposta = 0
                                    myPos.ImpSanzioni = 0
                                    myPos.ImpSanzioniRidotto = 0
                                    myPos.ImpInteressi = 0
                                Next
                                Session("oAccertatiGrigliaLocal") = oListAccertamentoOSAP
                                bTrovato = True
                                If bAnnoMod Then
                                    lblNotFound.Text = "La data di inizio e fine, dei singoli immobili, è stata modificata assegnado come anno l'anno di accertamento selezionato"
                                    lblNotFound.Visible = True
                                End If
                            Else
                                Session.Remove("oAccertatiGrigliaLocal")
                                GrdImmobiliOSAP.Visible = False
                                bTrovato = False
                            End If
                            '*** ***
                        Else
                            dt = FncGestAccert.RicercaDicAccICI("A", ConstSession.IdEnte, codContribuente, annoAccSelezionato, intIDProvvedimento, annoAccertamento, ConstSession.StringConnection)
                            If dt.GetLength(0) > 0 Then
                                Array.Sort(dt, New Utility.Comparatore(New String() {"IdLegame"}, New Boolean() {Utility.TipoOrdinamento.Crescente}))
                                GrdImmobiliICI.DataSource = dt
                                GrdImmobiliICI.DataBind()
                                GrdImmobiliICI.Visible = True
                                Session("DataTableImmobiliLocal") = dt
                                bTrovato = True
                            Else
                                GrdImmobiliICI.Visible = False
                                Session.Remove("DataTableImmobiliLocal")
                                bTrovato = False
                            End If
                        End If

                        If iRettifica = 1 Then
                            If ID_PROVVEDIMENTO_RETTIFICA <> intIDProvvedimento.ToString() Then
                                lblNotFound.Text = "Avviso sottoposto a Rettifica. Per poterlo modificare accedere da Gestione Atti"
                                lblNotFound.Visible = True
                                'sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='none';"

                            Else
                                lblNotFound.Visible = False
                                'sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='';"

                            End If
                        ElseIf iRettifica = 2 Then
                            'sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='';"

                        ElseIf iRettifica = 3 Then
                            If ID_PROVVEDIMENTO_RETTIFICA <> intIDProvvedimento.ToString() Then
                                If annoAccSelezionato = annoAccertamento Then
                                    lblNotFound.Text = "Avviso Notificato."
                                    lblNotFound.Visible = True
                                    'sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='none';"

                                Else
                                    If bTrovato = True Then
                                        lblNotFound.Visible = False
                                        'sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='';"

                                    Else
                                        lblNotFound.Text = "Non è stato trovato nessun immobile accertato o aperto per l'anno " & annoAccertamento
                                        lblNotFound.Visible = True
                                        'sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='none';"

                                    End If
                                End If
                            Else
                                If bTrovato = True Then
                                    lblNotFound.Visible = False
                                    'sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='';"

                                Else
                                    lblNotFound.Text = "Non è stato trovato nessun immobile accertato o aperto per l'anno " & annoAccertamento
                                    lblNotFound.Visible = True
                                    'sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='none';"

                                End If
                            End If
                        ElseIf bTrovato = True Then
                            lblNotFound.Visible = False
                            'sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='';"

                        Else
                            lblNotFound.Text = "Non è stato trovato nessun immobile accertato o aperto per l'anno " & annoAccertamento
                            lblNotFound.Visible = True
                            'sScript += "parent.Comandi.document.getElementById ('btnAssocia').style.display='none';"

                        End If
                        Exit For
                    End If
                Next
            Else
                If TxtTributo.Text = Utility.Costanti.TRIBUTO_TARSU Then
                    'TARSU
                    Session("oAccertatiGriglia") = Session("oAccertatiGrigliaLocal")
                    sSrc = "../GestioneAccertamentiTARSU/SearchAccertatiTARSU.aspx"
                    '*** 20130801 - accertamento OSAP ***
                ElseIf TxtTributo.Text = Utility.Costanti.TRIBUTO_OSAP Or TxtTributo.Text = "OSAP" Then
                    'TARSU
                    Session("oAccertatiGriglia") = Session("oAccertatiGrigliaLocal")
                    sSrc = "../GestioneAccertamentiOSAP/SearchDatiAccertatoOSAP.aspx"
                    '*** ***
                Else
                    'ICI
                    Session("DataTableImmobiliDaAccertare") = Session("DataTableImmobiliLocal")
                    If (ConstSession.CodTributo <> Utility.Costanti.TRIBUTO_TASI) Then
                        sSrc = "grdAccertato.aspx"
                    Else
                        sSrc = "../GestioneAccertamentiTASI/SearchDatiAccertato.aspx"
                    End If
                End If
                sScript += "parent.parent.opener.location.href='" & sSrc & "';"
                sScript += "parent.window.close();"
            End If
        Catch ex As Exception
            Throw New Exception("GetRowBind.errore: " + ex.StackTrace)
        End Try
        Return sScript
    End Function
    ''' <summary>
    ''' Funzione per la cancellazione dell'atto selezionato
    ''' </summary>
    ''' <param name="IDRow">Int identificativo della riga</param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Analisi eventi</em>
    ''' </revision>
    ''' </revisionHistory>
    Private Function GetRowDelete(IDRow As Integer) As String
        Dim hasError As Boolean = False
        Try
            Dim myRow As GridViewRow
            For Each myRow In GrdAtti.Rows
                If CInt(CType(myRow.FindControl("hfIdProv"), HiddenField).Value) = IDRow Then
                    If CType(myRow.FindControl("hfIdTributo"), HiddenField).Value = Utility.Costanti.TRIBUTO_TARSU Then
                        If FncGestAccert.DeleteAttoTARSU(CInt(CType(myRow.FindControl("hfIdProv"), HiddenField).Value()), myRow.Cells(0).Text(), CType(myRow.FindControl("hfIdContrib"), HiddenField).Value) = False Then
                            hasError = True
                        End If
                        '*** 20130801 - accertamento OSAP ***
                    ElseIf CType(myRow.FindControl("hfIdTributo"), HiddenField).Value = Utility.Costanti.TRIBUTO_OSAP Then
                        If FncGestAccert.DeleteAtto(CInt(CType(myRow.FindControl("hfIdProv"), HiddenField).Value())) = False Then
                            hasError = True
                        End If
                        '*** ***
                    Else
                        If FncGestAccert.DeleteAttoICI(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"), ConstSession.IdEnte, CInt(CType(myRow.FindControl("hfIdProv"), HiddenField).Value()), CInt(CType(myRow.FindControl("hfIdProc"), HiddenField).Value), myRow.Cells(0).Text(), CType(myRow.FindControl("hfIdContrib"), HiddenField).Value) = False Then
                            hasError = True
                        End If
                    End If
                    If hasError Then
                        sScript = "GestAlert('a', 'danger', '', '', 'Si e\' verificato un\'errore durante la cancellazione!');"
                    Else
                        CaricaGriglia(CType(myRow.FindControl("hfIdContrib"), HiddenField).Value, myRow.Cells(0).Text(), CType(myRow.FindControl("hfIdTributo"), HiddenField).Value)
                        sScript = "GestAlert('a', 'success', '', '', 'Eliminazione effettuata con successo!');"
                        Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                        fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Provvedimenti, "GetRowDelete", Utility.Costanti.AZIONE_DELETE, ConstSession.CodTributo, ConstSession.IdEnte, IDRow)
                    End If
                    Exit For
                End If
            Next
        Catch ex As Exception
            Throw New Exception("GetRowDelete.errore: " + ex.StackTrace)
        End Try
        Return sScript
    End Function
    'Private Function GetRowDelete(IDRow As Integer) As String
    '    Dim hasError As Boolean = False
    '    Try
    '        Dim myRow As GridViewRow
    '        For Each myRow In GrdAtti.Rows
    '            If CInt(CType(myRow.FindControl("hfIdProv"), HiddenField).Value) = IDRow Then
    '                If CType(myRow.FindControl("hfIdTributo"), HiddenField).Value = Utility.Costanti.TRIBUTO_TARSU Then
    '                    If FncGestAccert.DeleteAttoTARSU(CInt(CType(myRow.FindControl("hfIdProv"), HiddenField).Value()), myRow.Cells(0).Text(), CType(myRow.FindControl("hfIdContrib"), HiddenField).Value) = False Then
    '                        haserror = True
    '                    End If
    '                    '*** 20130801 - accertamento OSAP ***
    '                ElseIf CType(myRow.FindControl("hfIdTributo"), HiddenField).Value = Utility.Costanti.TRIBUTO_OSAP Then
    '                    If FncGestAccert.DeleteAttoOSAP(ConfigurationManager.AppSettings("connectionStringOPENgovPROVVEDIMENTI"), CInt(CType(myRow.FindControl("hfIdProv"), HiddenField).Value())) = False Then
    '                        haserror = True
    '                    End If
    '                    '*** ***
    '                Else
    '                    If FncGestAccert.DeleteAttoICI(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"), ConstSession.IdEnte, CInt(CType(myRow.FindControl("hfIdProv"), HiddenField).Value()), CInt(CType(myRow.FindControl("hfIdProc"), HiddenField).Value), myRow.Cells(0).Text(), CType(myRow.FindControl("hfIdContrib"), HiddenField).Value) = False Then
    '                        haserror = True
    '                    End If
    '                End If
    '                If HasError Then
    '                    sScript = "GestAlert('a', 'danger', '', '', 'Si e\' verificato un\'errore durante la cancellazione!');"
    '                Else
    '                    CaricaGriglia(CType(myRow.FindControl("hfIdContrib"), HiddenField).Value, myRow.Cells(0).Text(), CType(myRow.FindControl("hfIdTributo"), HiddenField).Value)
    '                    sScript = "GestAlert('a', 'success', '', '', 'Eliminazione effettuata con successo!');"
    '                    Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
    '                    fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Provvedimenti, "GetRowDelete", Utility.Costanti.AZIONE_DELETE, ConstSession.CodTributo, ConstSession.IdEnte, IDRow)
    '                End If
    '                Exit For
    '            End If
    '        Next
    '    Catch ex As Exception
    '        Throw New Exception("GetRowDelete.errore: " + ex.StackTrace)
    '    End Try
    '    Return sScript
    'End Function
End Class
