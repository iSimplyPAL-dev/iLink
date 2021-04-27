Imports log4net
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
Imports Utility
''' <summary>
''' Pagina per la visualizzazione/gestione dei vani.
''' Contiene i dati di dettaglio e le funzioni della comandiera.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
''' <revisionHistory>
''' <revision date="12/04/2019">
''' <strong>Qualificazione AgID-analisi_rel01</strong>
''' <em>Analisi eventi</em>
''' </revision>
''' </revisionHistory>
Partial Class PopUpInsertVani
    Inherits BaseEnte
    Private Shared Log As ILog = LogManager.GetLogger(GetType(PopUpInsertVani))
    Private WFErrore As String

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object
    Protected WithEvents info As Global.System.Web.UI.HtmlControls.HtmlGenericControl

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    '*** 20140805 - Gestione Categorie Vani ***
    ''' <summary>
    ''' caricamento della pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="">
    ''' X UNIONE CON BANCADATI CMGC
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="05/08/2014">
    ''' Gestione Categorie Vani
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="04/2015">
    ''' Nuova Gestione anagrafica con form unico
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="07/2015">
    ''' GESTIONE INCROCIATA RIFIUTI/ICI e DIVERSA GESTIONE QUOTA VARIABILE*
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim sScript As String = ""
        Dim oListCmd() As Object
        Dim oMyVani As New ObjOggetti
        Dim oListVani As New ArrayList

        Try
            If Page.IsPostBack = False Then
                lblTitolo.Text = ConstSession.DescrizioneEnte
                If ConstSession.IsFromTARES = "1" Then
                    info.InnerText = "TARI "
                Else
                    info.InnerText = "TARSU "
                End If
                If ConstSession.IsFromVariabile = "1" Then
                    info.InnerText += " Variabile "
                End If
                info.InnerText += "- Dichiarazioni - Gestione Immobili - Gestione Dati Vani"
                'controllo se devo abilitare i pulsanti di modifica/eliminazione
                If Request.Item("AzioneProv") = Utility.Costanti.AZIONE_NEW Then
                    ReDim Preserve oListCmd(1)
                    oListCmd(0) = "Modifica"
                    oListCmd(1) = "Delete"
                    sScript += "$('#" + oListCmd(0).ToString() + "').addClass('DisableBtn');"
                    sScript += "$('#" + oListCmd(1).ToString() + "').addClass('DisableBtn');"
                    RegisterScript(sScript, Me.GetType)
                Else
                    ReDim Preserve oListCmd(0)
                    oListCmd(0) = "Salva"
                    sScript += "$('#" + oListCmd(0).ToString() + "').addClass('DisableBtn');"
                    RegisterScript(sScript, Me.GetType)
                End If

                'carico l'id dettaglio testata
                TxtIdDettaglioTestata.Text = Request.Item("IdDettaglioTestata")
                Dim FncLoad As New ClsDichiarazione
                hdIdContribuente.Value = CInt(Request.Item("IdContribuente"))
                If ConstSession.HasPlainAnag Then
                    ifrmAnag.Attributes.Add("src", "../../Generali/asp/VisualAnag.aspx?IdContribuente=" & CInt(Request.Item("IdContribuente")) & "&Azione=" & Utility.Costanti.AZIONE_LETTURA)
                Else
                    FncLoad.LoadPannelAnagrafica(CInt(Request.Item("IdContribuente")), ConstSession.CodTributo, ConstSession.StringConnectionAnagrafica, lblNominativo, lblCFPIVA, lblDatiNascita, lblResidenza)
                End If
                '*** ***
                FncLoad.LoadPannelTessera(Session("oTessera"), LblCodUtente, LblCodInterno, LblNTessera, LblDataRilascio, LblDataCessazione)
                FncLoad.LoadPannelImmobile(Session("oUITemp"), LblUbicazione, LblRifCat, LblDataInizio, LblDataFine, LblMQCatasto, LblMQTassabili, LblCat, LblComponentiPF, LblComponentiPV, LblForzaCalcoloPV)
                'controllo se sono in visualizzazione di un vano
                If Request.Item("IdUniqueVano") <> "-1" Then
                    'carico il singolo vano selezionato
                    For Each oMyVani In Session("oDatiVani")
                        If oMyVani.IdOggetto = Request.Item("IdUniqueVano") Then
                            oListVani.Add(oMyVani)
                        End If
                    Next
                    Abilita(False, 0)
                ElseIf Request.Item("IdList") <> -1 Then
                    'sono in visualizzazione di un vano appena aggiunto e non ancora salvato sul db
                    Dim ListVaniUI() As ObjOggetti
                    ListVaniUI = Session("oDatiVani")
                    oMyVani = ListVaniUI(Request.Item("IdList"))
                    oListVani.Add(oMyVani)
                    Abilita(False, 0)
                Else
                    oMyVani = New ObjOggetti
                    If IsNumeric(Request.Item("IdCategoriaTARES")) Then
                        oMyVani.IdCatTARES = Request.Item("IdCategoriaTARES")
                    End If
                    If IsNumeric(Request.Item("txtNC")) Then
                        oMyVani.nNC = Request.Item("txtNC")
                    End If
                    If IsNumeric(Request.Item("txtNCPV")) Then
                        oMyVani.nNCPV = Request.Item("txtNCPV")
                    End If
                    oListVani.Add(oMyVani)
                End If
                GrdVani.DataSource = CType(oListVani.ToArray(GetType(ObjOggetti)), ObjOggetti())
                GrdVani.DataBind()
                '*** ***
                Session("OldCategoria") = oMyVani.IdCatTARES.ToString
                Session("OldNC") = oMyVani.nNC.ToString
                Session("OldNCPV") = oMyVani.nNCPV.ToString
                Session("OldCategoria") = oMyVani.IdCatTARES.ToString
                Session("OldMQ") = oMyVani.nMq.ToString
            End If
            If ConstSession.IsFromVariabile = "1" Then
                DivTessera.Style.Add("display", "")
            Else
                DivTessera.Style.Add("display", "none")
            End If
            If ConstSession.HasDummyDich Then
                GrdVani.Columns(8).Visible = False
            End If

            If ConstSession.HasPlainAnag Then
                sScript = "document.getElementById('TRSpecAnag').style.display='none';"
            Else
                sScript = "document.getElementById('TRPlainAnag').style.display='none';"
            End If
            RegisterScript(sScript, Me.GetType)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.PopUpInsertVani.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    'Put user code to initialize the page here
    '    Dim sScript As String = ""
    '    Dim oListCmd() As Object
    '    Dim MyFunction As New generalClass.generalFunction
    '    Dim oLoadCombo As New generalClass.generalFunction
    '    Dim oMyVani As New ObjOggetti
    '    Dim oListVani As New ArrayList 'Dim oListVani() As Object
    '    Dim nList As Integer = 0

    '    Try
    '        If Page.IsPostBack = False Then
    '            lblTitolo.Text = ConstSession.DescrizioneEnte
    '            If ConstSession.IsFromTARES = "1" Then
    '                info.InnerText = "TARI "
    '            Else
    '                info.InnerText = "TARSU "
    '            End If
    '            If ConstSession.IsFromVariabile = "1" Then
    '                info.InnerText += " Variabile "
    '            End If
    '            info.InnerText += "- Dichiarazioni - Gestione Immobili - Gestione Dati Vani"
    '            'controllo se devo abilitare i pulsanti di modifica/eliminazione
    '            If Request.Item("AzioneProv") = Costanti.AZIONE_NEW Then
    '                ReDim Preserve oListCmd(1)
    '                oListCmd(0) = "Modifica"
    '                oListCmd(1) = "Delete"
    '                sScript = MyFunction.PopolaJSdisabilita(oListCmd)
    '                RegisterScript(sScript, Me.GetType)
    '            Else
    '                ReDim Preserve oListCmd(0)
    '                oListCmd(0) = "Salva"
    '                sScript = MyFunction.PopolaJSdisabilita(oListCmd)
    '                RegisterScript(sScript, Me.GetType)
    '            End If

    '            'carico l'id dettaglio testata
    '            TxtIdDettaglioTestata.Text = Request.Item("IdDettaglioTestata")
    '            '*** 201504 - Nuova Gestione anagrafica con form unico ***
    '            '*** 20140805 - Gestione Categorie Vani ***
    '            Dim FncLoad As New ClsDichiarazione
    '            hdIdContribuente.Value = CInt(Request.Item("IdContribuente"))
    '            If ConstSession.HasPlainAnag Then
    '                ifrmAnag.Attributes.Add("src", "../../Generali/asp/VisualAnag.aspx?IdContribuente=" & CInt(Request.Item("IdContribuente")) & "&Azione=" & Costanti.AZIONE_LETTURA)
    '            Else
    '                FncLoad.LoadPannelAnagrafica(CInt(Request.Item("IdContribuente")), ConstSession.CodTributo, ConstSession.StringConnectionAnagrafica, lblNominativo, lblCFPIVA, lblDatiNascita, lblResidenza)
    '            End If
    '            '*** ***
    '            FncLoad.LoadPannelTessera(Session("oTessera"), LblCodUtente, LblCodInterno, LblNTessera, LblDataRilascio, LblDataCessazione)
    '            FncLoad.LoadPannelImmobile(Session("oUITemp"), LblUbicazione, LblRifCat, LblDataInizio, LblDataFine, LblMQCatasto, LblMQTassabili, LblCat, LblComponentiPF, LblComponentiPV, LblForzaCalcoloPV)
    '            'controllo se sono in visualizzazione di un vano
    '            If Request.Item("IdUniqueVano") <> "-1" Then
    '                'carico il singolo vano selezionato
    '                For Each oMyVani In Session("oDatiVani")
    '                    If oMyVani.IdOggetto = Request.Item("IdUniqueVano") Then
    '                        oListVani.Add(oMyVani)
    '                    End If
    '                Next
    '                Abilita(False, 0)
    '            ElseIf Request.Item("IdList") <> -1 Then
    '                'sono in visualizzazione di un vano appena aggiunto e non ancora salvato sul db
    '                Dim ListVaniUI() As ObjOggetti
    '                ListVaniUI = Session("oDatiVani")
    '                oMyVani = ListVaniUI(Request.Item("IdList"))
    '                oListVani.Add(oMyVani)
    '                Abilita(False, 0)
    '            Else
    '                oMyVani = New ObjOggetti
    '                If IsNumeric(Request.Item("IdCategoriaTARES")) Then
    '                    oMyVani.IdCatTARES = Request.Item("IdCategoriaTARES")
    '                End If
    '                If IsNumeric(Request.Item("txtNC")) Then
    '                    oMyVani.nNC = Request.Item("txtNC")
    '                End If
    '                If IsNumeric(Request.Item("txtNCPV")) Then
    '                    oMyVani.nNCPV = Request.Item("txtNCPV")
    '                End If
    '                oListVani.Add(oMyVani)
    '            End If
    '            GrdVani.DataSource = CType(oListVani.ToArray(GetType(ObjOggetti)), ObjOggetti())
    '            GrdVani.DataBind()
    '            '*** ***
    '        End If
    '        '*** X UNIONE CON BANCADATI CMGC ***
    '        If ConstSession.IsFromVariabile = "1" Then
    '            DivTessera.Style.Add("display", "")
    '        Else
    '            DivTessera.Style.Add("display", "none")
    '        End If
    '        '*** ***
    '        '*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI & DIVERSA GESTIONE QUOTA VARIABILE***
    '        If ConstSession.HasDummyDich Then
    '            GrdVani.Columns(8).Visible = False
    '        End If

    '        If ConstSession.HasPlainAnag Then
    '            sScript = "document.getElementById('TRSpecAnag').style.display='none';"
    '        Else
    '            sScript = "document.getElementById('TRPlainAnag').style.display='none';"
    '        End If
    '        RegisterScript(sScript, Me.GetType)
    '        '*** ***
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.PopUpInsertVani.Page_Load.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="bTypeAbilita"></param>
    ''' <param name="IsModifica"></param>
    Private Sub Abilita(ByVal bTypeAbilita As Boolean, ByVal IsModifica As Integer)
        'disabilito i dati del contribuente
        DdlCategorie1.Enabled = bTypeAbilita : DdlTipoVano1.Enabled = bTypeAbilita : TxtNVani1.Enabled = bTypeAbilita : TxtNMQ1.Enabled = bTypeAbilita
        '*** 20130325 - gestione mq tassabili per TARES ***
        ChkEsente1.Enabled = bTypeAbilita
        '*** ***
        '*** 20140805 - Gestione Categorie Vani ***
        GrdVani.Enabled = bTypeAbilita
        '*** ***
        Try
            If IsModifica <> 1 Then
                DdlCategorie2.Enabled = bTypeAbilita : DdlTipoVano2.Enabled = bTypeAbilita : TxtNVani2.Enabled = bTypeAbilita : TxtNMQ2.Enabled = bTypeAbilita
                DdlCategorie3.Enabled = bTypeAbilita : DdlTipoVano3.Enabled = bTypeAbilita : TxtNVani3.Enabled = bTypeAbilita : TxtNMQ3.Enabled = bTypeAbilita
                DdlCategorie4.Enabled = bTypeAbilita : DdlTipoVano4.Enabled = bTypeAbilita : TxtNVani4.Enabled = bTypeAbilita : TxtNMQ4.Enabled = bTypeAbilita
                DdlCategorie5.Enabled = bTypeAbilita : DdlTipoVano5.Enabled = bTypeAbilita : TxtNVani5.Enabled = bTypeAbilita : TxtNMQ5.Enabled = bTypeAbilita
                DdlCategorie6.Enabled = bTypeAbilita : DdlTipoVano6.Enabled = bTypeAbilita : TxtNVani6.Enabled = bTypeAbilita : TxtNMQ6.Enabled = bTypeAbilita
                DdlCategorie7.Enabled = bTypeAbilita : DdlTipoVano7.Enabled = bTypeAbilita : TxtNVani7.Enabled = bTypeAbilita : TxtNMQ7.Enabled = bTypeAbilita
                DdlCategorie8.Enabled = bTypeAbilita : DdlTipoVano8.Enabled = bTypeAbilita : TxtNVani8.Enabled = bTypeAbilita : TxtNMQ8.Enabled = bTypeAbilita
                '*** 20130325 - gestione mq tassabili per TARES ***
                ChkEsente2.Enabled = bTypeAbilita : ChkEsente3.Enabled = bTypeAbilita : ChkEsente4.Enabled = bTypeAbilita : ChkEsente5.Enabled = bTypeAbilita : ChkEsente6.Enabled = bTypeAbilita : ChkEsente7.Enabled = bTypeAbilita : ChkEsente8.Enabled = bTypeAbilita
                '*** ***
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.PopUpInsertVani.Abilita.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="25/03/2013">
    ''' gestione mq tassabili per TARES
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Private Sub CmdModVani_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdModVani.Click
        Dim sScript As String = ""
        Dim oListCmd() As Object
        Dim oMyVani As New ObjOggetti

        Try
            For Each oMyVani In Session("oDatiVani")
                If oMyVani.IdOggetto = Request.Item("IdUniqueVano") Then
                    'memorizzo il vano originale
                    Session("sOldMyVani") = oMyVani.IdOggetto.ToString + "|" + oMyVani.IdDettaglioTestata.ToString + "|" + oMyVani.IdCategoria + "|" + oMyVani.IdCatTARES.ToString + "|" + oMyVani.nNC.ToString + "|" + oMyVani.nNCPV.ToString + "|" + oMyVani.IdTipoVano.ToString + "|" + oMyVani.nVani.ToString + "|" + oMyVani.nMq.ToString + "|" + oMyVani.bIsEsente.ToString
                    Session("OldCategoria") = oMyVani.IdCatTARES.ToString
                    Session("OldNC") = oMyVani.nNC.ToString
                    Session("OldNCPV") = oMyVani.nNCPV.ToString
                    Session("OldCategoria") = oMyVani.IdCatTARES.ToString
                    Session("OldMQ") = oMyVani.nMq.ToString
                    Session("OldEsente") = oMyVani.bIsEsente.ToString
                End If
            Next
            Abilita(True, 1)
            ReDim Preserve oListCmd(1)
            oListCmd(0) = "Modifica"
            oListCmd(1) = "Delete"
            sScript += "$('#" + oListCmd(0).ToString() + "').addClass('DisableBtn');"
            sScript += "$('#" + oListCmd(1).ToString() + "').addClass('DisableBtn');"
            RegisterScript(sScript, Me.GetType)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.PopUpInsertVani.CmdModVani_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub CmdModVani_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdModVani.Click
    '    Dim sScript As String
    '    Dim oListCmd() As Object
    '    Dim MyFunction As New generalClass.generalFunction
    '    Dim oMyVani As New ObjOggetti

    '    Try
    '        For Each oMyVani In Session("oDatiVani")
    '            If oMyVani.IdOggetto = Request.Item("IdUniqueVano") Then
    '                '*** 20130325 - gestione mq tassabili per TARES ***
    '                'memorizzo il vano originale
    '                Session("sOldMyVani") = oMyVani.IdOggetto.ToString + "|" + oMyVani.IdDettaglioTestata.ToString + "|" + oMyVani.IdCategoria + "|" + oMyVani.IdCatTARES.ToString + "|" + oMyVani.nNC.ToString + "|" + oMyVani.nNCPV.ToString + "|" + oMyVani.IdTipoVano.ToString + "|" + oMyVani.nVani.ToString + "|" + oMyVani.nMq.ToString + "|" + oMyVani.bIsEsente.ToString
    '                '*** ***
    '            End If
    '        Next
    '        Abilita(True, 1)
    '        ReDim Preserve oListCmd(1)
    '        oListCmd(0) = "Modifica"
    '        oListCmd(1) = "Delete"
    '        sScript = MyFunction.PopolaJSdisabilita(oListCmd)
    '        RegisterScript(sScript, Me.GetType)
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.PopUpInsertVani.CmdModVani_Click.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' Pulsante per la cancellazione di un vano
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Analisi eventi</em>
    ''' </revision>
    ''' </revisionHistory>
    Private Sub CmdDeleteVani_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdDeleteVani.Click
        Dim oListVani() As ObjOggetti
        Dim oListNewVani() As ObjOggetti = Nothing
        Dim oNewVano As New ObjOggetti
        Dim nList, x As Integer
        Dim sScript As String
        Dim nMQTassabili As Double = 0
        Dim oMyDettaglioTestataAnater As New ObjDettaglioTestata

        Try
            If Not IsNothing(Session("oDettaglioTestataAnater")) Then
                oMyDettaglioTestataAnater = Session("oDettaglioTestataAnater")
            End If
            'carico l'array originario
            oListVani = Session("oDatiVani")
            'se ho solo un immobile non gli permetto la cancellazione
            If oListVani.GetUpperBound(0) = 0 Then
                sScript = "GestAlert('a', 'warning', '', '', 'Cancellando questo vano l\'immobile risulta senza vani.\nProcedere all\'eliminazione dell\'immobile o alla configurazione di nuovi vani!');"
                RegisterScript(sScript, Me.GetType)
            Else
                'carico il nuovo oggetto senza la riga selezionata
                nList = -1
                If Request.Item("IdUniqueVano") <> -1 Then
                    For x = 0 To oListVani.GetUpperBound(0)
                        If oListVani(x).IdOggetto <> Request.Item("IdUniqueVano") Then
                            nList += 1
                            ReDim Preserve oListNewVani(nList)
                            oNewVano = New ObjOggetti
                            oNewVano = oListVani(x)
                            oListNewVani(nList) = oNewVano
                        End If
                    Next
                Else
                    For x = 0 To oListVani.GetUpperBound(0)
                        If x <> Request.Item("IdList") Then
                            nList += 1
                            ReDim Preserve oListNewVani(nList)
                            oNewVano = New ObjOggetti
                            oNewVano = oListVani(x)
                            oListNewVani(nList) = oNewVano
                        End If
                    Next
                End If
                Dim FunctionVani As New GestOggetti
                'aggiorno il database solo se ho già inserito nel db
                If TxtIdDettaglioTestata.Text <> "-1" And Request.Item("IdList") <> -1 Then
                    If FunctionVani.DeleteOggetti(ConstSession.StringConnection, Request.Item("IdUniqueVano"), Now, CInt(TxtIdDettaglioTestata.Text), nMQTassabili) = 0 Then
                        Response.Redirect("../../PaginaErrore.aspx")
                        Exit Sub
                    End If
                End If
                'memorizzo l'oggetto nella sessione
                Session("oDatiVani") = oListNewVani
                If Not Session("oUITemp") Is Nothing Then
                    CType(Session("oUITemp"), ObjDettaglioTestata).oOggetti = oListNewVani
                End If
                '*** 20130325 - gestione mq tassabili per TARES ***
                oMyDettaglioTestataAnater.nMQTassabili = nMQTassabili
                oMyDettaglioTestataAnater.oOggetti = oListNewVani
                Session("oDettaglioTestataAnater") = oMyDettaglioTestataAnater
                '*** ***
                Dim fncActionEvent As New DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Immobile, "CmdDeleteVani", Utility.Costanti.AZIONE_UPDATE, ConstSession.CodTributo, ConstSession.IdEnte, CInt(TxtIdDettaglioTestata.Text))
            End If

            '***Fabiana
            'aggiorno la pagina chiamante
            sScript = BackToUI()
            RegisterScript(sScript, Me.GetType)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.PopUpInsertVani.CmdDeleteVani_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub CmdDeleteVani_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdDeleteVani.Click
    '    Dim oListVani() As ObjOggetti
    '    Dim oListNewVani() As ObjOggetti = Nothing
    '    Dim oNewVano As New ObjOggetti
    '    Dim nList, x As Integer
    '    Dim sScript As String
    '    Dim sProvenienza As String = ""
    '    Dim nMQTassabili As Double = 0
    '    Dim oMyDettaglioTestataAnater As New ObjDettaglioTestata

    '    Try
    '        If Not IsNothing(Session("oDettaglioTestataAnater")) Then
    '            oMyDettaglioTestataAnater = Session("oDettaglioTestataAnater")
    '        End If
    '        'carico l'array originario
    '        oListVani = Session("oDatiVani")
    '        'se ho solo un immobile non gli permetto la cancellazione
    '        If oListVani.GetUpperBound(0) = 0 Then
    '            sScript = "GestAlert('a', 'warning', '', '', 'Cancellando questo vano l\'immobile risulta senza vani.\nProcedere all\'eliminazione dell\'immobile o alla configurazione di nuovi vani!');"
    '            RegisterScript(sScript, Me.GetType)
    '        Else
    '            'carico il nuovo oggetto senza la riga selezionata
    '            nList = -1
    '            If Request.Item("IdUniqueVano") <> -1 Then
    '                For x = 0 To oListVani.GetUpperBound(0)
    '                    If oListVani(x).IdOggetto <> Request.Item("IdUniqueVano") Then
    '                        nList += 1
    '                        ReDim Preserve oListNewVani(nList)
    '                        oNewVano = New ObjOggetti
    '                        oNewVano = oListVani(x)
    '                        oListNewVani(nList) = oNewVano
    '                    End If
    '                Next
    '            Else
    '                For x = 0 To oListVani.GetUpperBound(0)
    '                    If x <> Request.Item("IdList") Then
    '                        nList += 1
    '                        ReDim Preserve oListNewVani(nList)
    '                        oNewVano = New ObjOggetti
    '                        oNewVano = oListVani(x)
    '                        oListNewVani(nList) = oNewVano
    '                    End If
    '                Next
    '            End If
    '            Dim FunctionVani As New GestOggetti
    '            'aggiorno il database solo se ho già inserito nel db
    '            If TxtIdDettaglioTestata.Text <> "-1" And Request.Item("IdList") <> -1 Then
    '                If FunctionVani.DeleteOggetti(ConstSession.StringConnection, Request.Item("IdUniqueVano"), Now, CInt(TxtIdDettaglioTestata.Text), nMQTassabili) = 0 Then
    '                    Response.Redirect("../../PaginaErrore.aspx")
    '                    Exit Sub
    '                End If
    '            End If
    '            'memorizzo l'oggetto nella sessione
    '            Session("oDatiVani") = oListNewVani
    '            If Not Session("oUITemp") Is Nothing Then
    '                CType(Session("oUITemp"), ObjDettaglioTestata).oOggetti = oListNewVani
    '            End If
    '            '*** 20130325 - gestione mq tassabili per TARES ***
    '            oMyDettaglioTestataAnater.nMQTassabili = nMQTassabili
    '            oMyDettaglioTestataAnater.oOggetti = oListNewVani
    '            Session("oDettaglioTestataAnater") = oMyDettaglioTestataAnater
    '            '*** ***
    '        End If

    '        '***Fabiana
    '        If TxtIdDettaglioTestata.Text <> "-1" Then
    '            sProvenienza = Costanti.AZIONE_UPDATE
    '        Else
    '            sProvenienza = Costanti.AZIONE_NEW
    '        End If
    '        'aggiorno la pagina chiamante
    '        sScript = BackToUI()
    '        RegisterScript(sScript, Me.GetType)
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.PopUpInsertVani.CmdDeleteVani_Click.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' Pulsante per il salvataggio delle modifiche apportate
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Analisi eventi</em>
    ''' </revision>
    ''' </revisionHistory>
    Private Sub CmdSalvaVani_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdSalvaVani.Click
        Dim oMyVano As New ObjOggetti
        Dim myArray As New ArrayList
        Dim dgiMyRow As GridViewRow
        Dim FncVani As New DichManagerTARSU(ConstSession.DBType, ConstSession.StringConnection, ConstSession.StringConnectionOPENgov, ConstSession.IdEnte) 'As New GestOggetti
        Dim sNewMyVani As String
        Dim nMQTassabili As Double = 0
        Dim oMyDettaglioTestataAnater As New ObjDettaglioTestata
        Dim sScript As String = ""

        Try
            'controllo che ci siano tutti i dati
            If CheckDatiVani(sScript) = True Then
                If Not IsNothing(Session("oDettaglioTestataAnater")) Then
                    oMyDettaglioTestataAnater = Session("oDettaglioTestataAnater")
                End If
                'controllo se sono in modifica di un vano già salvato sul db
                If Request.Item("IdUniqueVano") <> "-1" Then
                    'carico i dati dal form
                    For Each dgiMyRow In GrdVani.Rows
                        oMyVano = ValDatiVano(dgiMyRow)
                        If oMyVano Is Nothing Then
                            Throw New Exception("Errore in recupero dati vano vano")
                        End If
                    Next
                    '*** 20130325 - gestione mq tassabili per TARES ***
                    'memorizzo il nuovo vano
                    sNewMyVani = oMyVano.IdOggetto.ToString + "|" + oMyVano.IdDettaglioTestata.ToString + "|" + oMyVano.IdCategoria + "|" + oMyVano.IdCatTARES.ToString + "|" + oMyVano.nNC.ToString + "|" + oMyVano.nNCPV.ToString + "|" + oMyVano.IdTipoVano.ToString + "|" + oMyVano.nVani.ToString + "|" + oMyVano.nMq.ToString + "|" + oMyVano.bIsEsente.ToString + "|" + oMyVano.bForzaCalcolaPV.ToString
                    '*** ***
                    'controllo se sono state apportate modifiche
                    If sNewMyVani.CompareTo(Session("sOldMyVani")) <> 0 Then
                        If Not Session("oDatiVani") Is Nothing Then
                            'carico il singolo vano selezionato
                            For Each myItem As ObjOggetti In Session("oDatiVani")
                                If myItem.IdOggetto = Request.Item("IdUniqueVano") Then
                                    'storicizzo ed inserisco il vano originale solo se ho già inserito nel db
                                    If TxtIdDettaglioTestata.Text <> "-1" Then
                                        'storicizzo il vano originale
                                        myItem.tDataVariazione = Now
                                        'aggiorno il db
                                        If FncVani.SetOggetto(Utility.Costanti.AZIONE_NEW, myItem) = 0 Then
                                            Response.Redirect("../../PaginaErrore.aspx")
                                            Exit Sub
                                        End If
                                        If FncVani.SetOggetto(Utility.Costanti.AZIONE_NEW, oMyVano) = 0 Then
                                            Response.Redirect("../../PaginaErrore.aspx")
                                            Exit Sub
                                        End If

                                        Dim myUI As New ObjDettaglioTestata
                                        Dim nTypeVar As Integer
                                        myUI = Session("oUITemp")
                                        If StringOperation.FormatString(Session("OldCategoria")) <> oMyVano.IdCatTARES.ToString Then
                                            nTypeVar = ModificheTributarie.ModificheTributarieCausali.VariazioneCategoria
                                        ElseIf StringOperation.FormatString(Session("OldNC")) <> oMyVano.nNC.ToString Or StringOperation.FormatString(Session("OldNCPV")) <> oMyVano.nNCPV.ToString Then
                                            nTypeVar = ModificheTributarie.ModificheTributarieCausali.VariazioneComponenti
                                        ElseIf StringOperation.FormatString(Session("OldCategoria")) <> oMyVano.IdCatTARES.ToString Then
                                            nTypeVar = ModificheTributarie.ModificheTributarieCausali.VariazioneTipoVano
                                        ElseIf StringOperation.FormatString(Session("OldMQ")) <> oMyVano.nMq.ToString Then
                                            nTypeVar = ModificheTributarie.ModificheTributarieCausali.VariazioneMq
                                        ElseIf StringOperation.FormatString(Session("OldEsente")) <> oMyVano.bIsEsente.ToString Then
                                            nTypeVar = ModificheTributarie.ModificheTributarieCausali.VariazioneEsenzione
                                        End If
                                        If New ModificheTributarie().SetModificheTributarie(ConstSession.StringConnectionOPENgov, ModificheTributarie.DBOperation.Insert, 0, ConstSession.IdEnte, ConstSession.CodTributo, nTypeVar, myUI.sFoglio, myUI.sNumero, myUI.sSubalterno, Now, ConstSession.UserName, oMyVano.IdOggetto, Date.MaxValue) = False Then
                                            Log.Debug("Errore in SetModificheTributarie")
                                        End If
                                    End If
                                    myItem = oMyVano
                                End If
                                myArray.Add(myItem)
                            Next
                        End If
                    End If
                    'svuoto la variabile di riferimento
                    Session("sOldMyVani") = Nothing
                Else
                    Dim IdDettaglioTestata As Integer = -1
                    'controllo se ho già dei vani caricati
                    If Not Session("oDatiVani") Is Nothing Then
                        'carico l'array originario
                        For Each oMyVano In Session("oDatiVani")
                            myArray.Add(oMyVano)
                            IdDettaglioTestata = oMyVano.IdDettaglioTestata
                        Next
                    End If
                    'leggo i valori già inseriti in griglia
                    For Each dgiMyRow In GrdVani.Rows
                        oMyVano = ValDatiVano(dgiMyRow)
                        'deve essere quello degli altri vani altrimenti sbaglia salvataggio
                        oMyVano.IdDettaglioTestata = IdDettaglioTestata
                        'controllo se sono in modifica di un vano non ancora salvato sul db
                        If Request.Item("IdList") <> -1 Then
                            'aggiorno la posizione dell'oggetto
                            myArray(Request.Item("IdList")) = oMyVano
                        Else
                            If Not oMyVano Is Nothing Then
                                myArray.Add(oMyVano)
                            Else
                                Throw New Exception("Errore in recupero dati vano vano")
                            End If
                        End If
                    Next
                    'aggiorno il database solo se sono in inserimento nuovo vano su testata già inserita
                    If Not Session("oDatiVani") Is Nothing And TxtIdDettaglioTestata.Text <> "-1" Then
                        '*** 20130325 - gestione mq tassabili per TARES ***
                        If FncVani.SetOggetti(CType(myArray.ToArray(GetType(ObjOggetti)), ObjOggetti()), TxtIdDettaglioTestata.Text, nMQTassabili) = 0 Then
                            Response.Redirect("../../PaginaErrore.aspx")
                            Exit Sub
                        End If
                        '*** ***
                    End If
                End If
                'memorizzo l'oggetto nella sessione
                Session("oDatiVani") = CType(myArray.ToArray(GetType(ObjOggetti)), ObjOggetti())
                If Not Session("oUITemp") Is Nothing Then
                    CType(Session("oUITemp"), ObjDettaglioTestata).oOggetti = CType(myArray.ToArray(GetType(ObjOggetti)), ObjOggetti())
                End If
                '*** 20130325 - gestione mq tassabili per TARES ***
                oMyDettaglioTestataAnater.nMQTassabili = nMQTassabili
                oMyDettaglioTestataAnater.oOggetti = CType(myArray.ToArray(GetType(ObjOggetti)), ObjOggetti())
                Session("oDettaglioTestataAnater") = oMyDettaglioTestataAnater
                '*** ***
                Dim fncActionEvent As New DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Immobile, "CmdSalvaVani", Utility.Costanti.AZIONE_UPDATE, ConstSession.CodTributo, ConstSession.IdEnte, CInt(TxtIdDettaglioTestata.Text))
                'aggiorno la pagina chiamante
                sScript = BackToUI()
            End If
            RegisterScript(sScript, Me.GetType)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.PopUpInsertVani.CmdSalvaVani_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub CmdSalvaVani_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdSalvaVani.Click
    '    Dim oMyVano As New ObjOggetti
    '    Dim myArray As New ArrayList
    '    Dim dgiMyRow As GridViewRow
    '    Dim FncVani As New DichManagerTARSU(ConstSession.DBType, ConstSession.StringConnection) 'As New GestOggetti
    '    Dim sNewMyVani As String ', sDatiInsert
    '    Dim nMQTassabili As Double = 0
    '    Dim oMyDettaglioTestataAnater As New ObjDettaglioTestata
    '    Dim sScript As String = ""

    '    Try
    '        'controllo che ci siano tutti i dati
    '        If CheckDatiVani(sScript) = True Then
    '            If Not IsNothing(Session("oDettaglioTestataAnater")) Then
    '                oMyDettaglioTestataAnater = Session("oDettaglioTestataAnater")
    '            End If
    '            'controllo se sono in modifica di un vano già salvato sul db
    '            If Request.Item("IdUniqueVano") <> "-1" Then
    '                'carico i dati dal form
    '                For Each dgiMyRow In GrdVani.Rows
    '                    oMyVano = ValDatiVano(dgiMyRow)
    '                    If oMyVano Is Nothing Then
    '                        Throw New Exception("Errore in recupero dati vano vano")
    '                    End If
    '                Next
    '                '*** 20130325 - gestione mq tassabili per TARES ***
    '                'memorizzo il nuovo vano
    '                sNewMyVani = oMyVano.IdOggetto.ToString + "|" + oMyVano.IdDettaglioTestata.ToString + "|" + oMyVano.IdCategoria + "|" + oMyVano.IdCatTARES.ToString + "|" + oMyVano.nNC.ToString + "|" + oMyVano.nNCPV.ToString + "|" + oMyVano.IdTipoVano.ToString + "|" + oMyVano.nVani.ToString + "|" + oMyVano.nMq.ToString + "|" + oMyVano.bIsEsente.ToString + "|" + oMyVano.bForzaCalcolaPV.ToString
    '                '*** ***
    '                'controllo se sono state apportate modifiche
    '                If sNewMyVani.CompareTo(Session("sOldMyVani")) <> 0 Then
    '                    If Not Session("oDatiVani") Is Nothing Then
    '                        'carico il singolo vano selezionato
    '                        For Each oMyVani As ObjOggetti In Session("oDatiVani")
    '                            If oMyVani.IdOggetto = Request.Item("IdUniqueVano") Then
    '                                'storicizzo ed inserisco il vano originale solo se ho già inserito nel db
    '                                If TxtIdDettaglioTestata.Text <> "-1" Then
    '                                    'storicizzo il vano originale
    '                                    oMyVani.tDataVariazione = Now
    '                                    'aggiorno il db
    '                                    If FncVani.SetOggetto(Costanti.AZIONE_NEW, oMyVani) = 0 Then
    '                                        Response.Redirect("../../PaginaErrore.aspx")
    '                                        Exit Sub
    '                                    End If
    '                                    If FncVani.SetOggetto(Costanti.AZIONE_NEW, oMyVano) = 0 Then
    '                                        Response.Redirect("../../PaginaErrore.aspx")
    '                                        Exit Sub
    '                                    End If
    '                                End If
    '                                oMyVani = oMyVano
    '                            End If
    '                            myArray.Add(oMyVani)
    '                        Next
    '                    End If
    '                End If
    '                'svuoto la variabile di riferimento
    '                Session("sOldMyVani") = Nothing
    '            Else
    '                Dim IdDettaglioTestata As Integer = -1
    '                'controllo se ho già dei vani caricati
    '                If Not Session("oDatiVani") Is Nothing Then
    '                    'carico l'array originario
    '                    For Each oMyVano In Session("oDatiVani")
    '                        myArray.Add(oMyVano)
    '                        IdDettaglioTestata = oMyVano.IdDettaglioTestata
    '                    Next
    '                End If
    '                'leggo i valori già inseriti in griglia
    '                For Each dgiMyRow In GrdVani.Rows
    '                    oMyVano = ValDatiVano(dgiMyRow)
    '                    'deve essere quello degli altri vani altrimenti sbaglia salvataggio
    '                    oMyVano.IdDettaglioTestata = IdDettaglioTestata
    '                    'controllo se sono in modifica di un vano non ancora salvato sul db
    '                    If Request.Item("IdList") <> -1 Then
    '                        'aggiorno la posizione dell'oggetto
    '                        myArray(Request.Item("IdList")) = oMyVano
    '                    Else
    '                        If Not oMyVano Is Nothing Then
    '                            myArray.Add(oMyVano)
    '                        Else
    '                            Throw New Exception("Errore in recupero dati vano vano")
    '                        End If
    '                    End If
    '                Next
    '                'aggiorno il database solo se sono in inserimento nuovo vano su testata già inserita
    '                If Not Session("oDatiVani") Is Nothing And TxtIdDettaglioTestata.Text <> "-1" Then
    '                    '*** 20130325 - gestione mq tassabili per TARES ***
    '                    If FncVani.SetOggetti(CType(myArray.ToArray(GetType(ObjOggetti)), ObjOggetti()), TxtIdDettaglioTestata.Text, nMQTassabili) = 0 Then
    '                        Response.Redirect("../../PaginaErrore.aspx")
    '                        Exit Sub
    '                    End If
    '                    '*** ***
    '                End If
    '            End If
    '            'memorizzo l'oggetto nella sessione
    '            Session("oDatiVani") = CType(myArray.ToArray(GetType(ObjOggetti)), ObjOggetti())
    '            If Not Session("oUITemp") Is Nothing Then
    '                CType(Session("oUITemp"), ObjDettaglioTestata).oOggetti = CType(myArray.ToArray(GetType(ObjOggetti)), ObjOggetti())
    '            End If
    '            '*** 20130325 - gestione mq tassabili per TARES ***
    '            oMyDettaglioTestataAnater.nMQTassabili = nMQTassabili
    '            oMyDettaglioTestataAnater.oOggetti = CType(myArray.ToArray(GetType(ObjOggetti)), ObjOggetti())
    '            Session("oDettaglioTestataAnater") = oMyDettaglioTestataAnater
    '            '*** ***
    '            'aggiorno la pagina chiamante
    '            sScript = BackToUI()
    '        End If
    '        RegisterScript(sScript, Me.GetType)
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.PopUpInsertVani.CmdSalvaVani_Click.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

#Region "Griglie"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdRowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            Dim ddlMyDati As New DropDownList
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                ddlMyDati = CType(e.Row.FindControl("ddlCatTARSU"), DropDownList)
                ddlMyDati.SelectedValue = CType(e.Row.DataItem, ObjOggetti).IdCategoria

                ddlMyDati = CType(e.Row.FindControl("ddlCatTARES"), DropDownList)
                ddlMyDati.SelectedValue = CType(e.Row.DataItem, ObjOggetti).IdCatTARES

                ddlMyDati = CType(e.Row.FindControl("ddlTipoVano"), DropDownList)
                ddlMyDati.SelectedValue = CType(e.Row.DataItem, ObjOggetti).IdTipoVano
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.PopUpInsertVani.GrdRowDataBound.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Dim oMyVani As New ObjOggetti
        Dim myArray As New ArrayList
        Dim dgiMyRow As GridViewRow

        Try
            If e.CommandName = "RowNew" Then
                'leggo i valori già inseriti in griglia
                For Each dgiMyRow In GrdVani.Rows
                    oMyVani = ValDatiVano(dgiMyRow)
                    If Not oMyVani Is Nothing Then
                        myArray.Add(oMyVani)
                    Else
                        Throw New Exception("Errore in recupero dati vano vano")
                    End If
                Next
                'aggiungo la riga nuova solo se arrivo da nuovo altrimenti alert
                If Request.Item("AzioneProv") = Utility.Costanti.AZIONE_NEW Then
                    oMyVani = New ObjOggetti
                    If IsNumeric(Request.Item("IdCategoriaTARES")) Then
                        oMyVani.IdCatTARES = Request.Item("IdCategoriaTARES")
                    End If
                    If IsNumeric(Request.Item("txtNC")) Then
                        oMyVani.nNC = Request.Item("txtNC")
                    End If
                    If IsNumeric(Request.Item("txtNCPV")) Then
                        oMyVani.nNCPV = Request.Item("txtNCPV")
                    End If
                    myArray.Add(oMyVani)
                    'carico la griglia
                    GrdVani.DataSource = CType(myArray.ToArray(GetType(ObjOggetti)), ObjOggetti())
                    GrdVani.DataBind()
                Else
                    RegisterScript("GestAlert('a', 'warning', '', '', 'Impossibile aggiungere nuovi vani se si e\' su di un singolo vano!');", Me.GetType)
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.PopUpInsertVani.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub GrdVani_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdVani.ItemDataBound
    '    Dim ddlMyDati As New DropDownList

    '    Try
    '        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '            ddlMyDati = CType(e.Item.FindControl("ddlCatTARSU"), DropDownList)
    '            ddlMyDati.SelectedValue = CType(e.Item.DataItem, ObjOggetti).IdCategoria

    '            ddlMyDati = CType(e.Item.FindControl("ddlCatTARES"), DropDownList)
    '            ddlMyDati.SelectedValue = CType(e.Item.DataItem, ObjOggetti).IdCatTARES

    '            ddlMyDati = CType(e.Item.FindControl("ddlTipoVano"), DropDownList)
    '            ddlMyDati.SelectedValue = CType(e.Item.DataItem, ObjOggetti).IdTipoVano
    '        End If
    '    Catch ex As Exception
    '             Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.PopUpInsertVani.GrdVani_ItemDataBound.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Private Sub GrdVani_UpdateCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GrdVani.UpdateCommand
    '    Dim oMyVani As New ObjOggetti
    '    Dim myArray As New ArrayList
    '    Dim dgiMyRow As GridViewRow

    '    Try
    '        'leggo i valori già inseriti in griglia
    '        For Each dgiMyRow In GrdVani.Items
    '            oMyVani = ValDatiVano(dgiMyRow)
    '            If Not oMyVani Is Nothing Then
    '                myArray.Add(oMyVani)
    '            Else
    '                Throw New Exception("Errore in recupero dati vano vano")
    '            End If
    '        Next
    '        'aggiungo la riga nuova solo se arrivo da nuovo altrimenti alert
    '        If Request.Item("AzioneProv") = Costanti.AZIONE_NEW Then
    '            oMyVani = New ObjOggetti
    '            If IsNumeric(Request.Item("IdCategoriaTARES")) Then
    '                oMyVani.IdCatTARES = Request.Item("IdCategoriaTARES")
    '            End If
    '            If IsNumeric(Request.Item("txtNC")) Then
    '                oMyVani.nNC = Request.Item("txtNC")
    '            End If
    '            If IsNumeric(Request.Item("txtNCPV")) Then
    '                oMyVani.nNCPV = Request.Item("txtNCPV")
    '            End If
    '            myArray.Add(oMyVani)
    '            'carico la griglia
    '            GrdVani.DataSource = CType(myArray.ToArray(GetType(ObjOggetti)), ObjOggetti())
    '            GrdVani.DataBind()
    '            GrdVani.start_index = Convert.ToString(GrdVani.CurrentPageIndex)
    '            GrdVani.AllowCustomPaging = False
    '        Else
    '            RegisterScript(Me.GetType(), "grdup", "<script language='javascript'>alert('Impossibile aggiungere nuovi vani se si e\' su di un singolo vano!')</script>")
    '        End If
    '    Catch ex As Exception
    '               Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.PopUpInsertVani.GrdVani_UpdateCommand.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
#End Region
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Protected Function GetCatTARSU() As DataView
        Dim dvMyDati As New DataView
        Dim sSQL As String = ""
        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetCategorieEnte", "IdEnte", "TipoTassazione")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IdEnte", ConstSession.IdEnte) _
                            , ctx.GetParam("TipoTassazione", ObjRuolo.TipoCalcolo.TARSU)
                        )
                ctx.Dispose()
            End Using
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.PopUpInsertVani.GetCatTARSU.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
        Return dvMyDati
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Protected Function GetCatTARES() As DataView
        Dim dvMyDati As New DataView
        Dim sSQL As String = ""
        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GetCategorieEnte", "IdEnte", "TipoTassazione")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IdEnte", ConstSession.IdEnte) _
                            , ctx.GetParam("TipoTassazione", ObjRuolo.TipoCalcolo.TARES)
                        )
                ctx.Dispose()
            End Using
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.PopUpInsertVani.GetCatTARES.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
        Return dvMyDati
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Protected Function GetTipoVano() As DataView
        Dim dvMyDati As New DataView
        Dim sSQL As String = ""
        Try
            Using ctx As New DBModel(ConstSession.DBType, ConstSession.StringConnection)
                sSQL = "SELECT '...' AS DESCRIZIONE,'-1' AS IDTIPOVANO"
                sSQL += " UNION"
                sSQL += " SELECT DESCRIZIONE, IDTIPOVANO"
                sSQL += " FROM TBLTIPOVANI"
                sSQL += " WHERE (IDENTE=@IdEnte)"
                sSQL += " ORDER BY DESCRIZIONE"
                sSQL = ctx.GetSQL(DBModel.TypeQuery.View, sSQL)
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("IdEnte", ConstSession.IdEnte))
                ctx.Dispose()
            End Using
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.PopUpInsertVani.GetTipoVano.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
        Return dvMyDati
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="dgiRow"></param>
    ''' <returns></returns>
    Private Function ValDatiVano(ByVal dgiRow As GridViewRow) As ObjOggetti
        Dim oMyVano As New ObjOggetti

        Try
            oMyVano.IdCategoria = CType(dgiRow.FindControl("ddlCatTARSU"), DropDownList).SelectedValue
            oMyVano.sCategoria = CType(dgiRow.FindControl("ddlCatTARSU"), DropDownList).SelectedItem.Text
            oMyVano.IdCatTARES = CType(dgiRow.FindControl("ddlCatTARES"), DropDownList).SelectedValue
            oMyVano.sDescrCatTARES = CType(dgiRow.FindControl("ddlCatTARES"), DropDownList).SelectedItem.Text
            oMyVano.nNC = CType(dgiRow.FindControl("txtNC"), TextBox).Text
            oMyVano.nNCPV = CType(dgiRow.FindControl("txtNCPV"), TextBox).Text
            oMyVano.bForzaCalcolaPV = CType(dgiRow.FindControl("chkForzaCalcoloPV"), CheckBox).Checked
            oMyVano.IdTipoVano = CType(dgiRow.FindControl("ddlTipoVano"), DropDownList).SelectedValue
            oMyVano.sTipoVano = CType(dgiRow.FindControl("ddlTipoVano"), DropDownList).SelectedItem.Text
            oMyVano.nMq = CType(dgiRow.FindControl("txtMQ"), TextBox).Text
            oMyVano.nVani = CType(dgiRow.FindControl("txtNVani"), TextBox).Text
            oMyVano.bIsEsente = CType(dgiRow.FindControl("chkEsente"), CheckBox).Checked
            oMyVano.IdOggetto = CType(dgiRow.FindControl("hfIdOggetto"), HiddenField).Value
            oMyVano.IdDettaglioTestata = TxtIdDettaglioTestata.Text
            oMyVano.sProvenienza = "MANUALE"
            oMyVano.tDataInserimento = Now
            oMyVano.tDataVariazione = Nothing
            oMyVano.tDataCessazione = Nothing
            oMyVano.sOperatore = ConstSession.UserName
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.PopUpInsertVani.ValDatiVano.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            oMyVano = Nothing
        End Try
        Return oMyVano
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sErr"></param>
    ''' <returns></returns>
    Private Function CheckDatiVani(ByRef sErr As String) As Boolean
        Try
            For Each dgiRow As GridViewRow In GrdVani.Rows
                'controllo che ci sia la categoria
                If (CType(dgiRow.FindControl("ddlCatTARSU"), DropDownList).SelectedValue = "-1" Or CType(dgiRow.FindControl("ddlCatTARSU"), DropDownList).SelectedValue = "") And ConstSession.IsFromTARES <> "1" Then
                    sErr = "GestAlert('a', 'warning', '', '', 'E\' necessario selezionare una Categoria TARSU!');"
                    Return False
                End If
                If CType(dgiRow.FindControl("ddlCatTARES"), DropDownList).SelectedValue <= 0 And ConstSession.IsFromTARES = "1" Then
                    sErr = "GestAlert('a', 'warning', '', '', 'E\' necessario selezionare una Categoria!');"
                    Return False
                End If
                'controllo che ci sia il tipo vano
                If CType(dgiRow.FindControl("ddlTipoVano"), DropDownList).SelectedValue = "-1" Or CType(dgiRow.FindControl("ddlTipoVano"), DropDownList).SelectedValue = "" Then
                    sErr = "GestAlert('a', 'warning', '', '', 'E\' necessario selezionare un Tipo Vano!');"
                    Return False
                End If
                'controllo che ci sia il numero di vani
                If IsNumeric(CType(dgiRow.FindControl("txtNVani"), TextBox).Text) = False Then
                    sErr = "GestAlert('a', 'warning', '', '', 'E\' necessario valorizzare il campo Numero Vani!');"
                    Return False
                Else
                    If CInt(CType(dgiRow.FindControl("txtNVani"), TextBox).Text) <= 0 Then
                        sErr = "GestAlert('a', 'warning', '', '', 'E\' necessario valorizzare il campo Numero Vani!');"
                        Return False
                    End If
                End If
                'controllo che ci siano i mq
                If IsNumeric(CType(dgiRow.FindControl("txtMQ"), TextBox).Text) = False Then
                    sErr = "GestAlert('a', 'warning', '', '', 'E\' necessario valorizzare il campo Mq!');"
                    Return False
                Else
                    If CInt(CType(dgiRow.FindControl("txtMQ"), TextBox).Text) <= 0 Then
                        sErr = "GestAlert('a', 'warning', '', '', 'E\' necessario valorizzare il campo Mq!');"
                        Return False
                    End If
                End If
            Next
            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.PopUpInsertVani.CheckDatiVani.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            sErr = "Errore in controllo dati::" & ex.Message
            Return False
        End Try
    End Function

    'Private Function BackToUI() As String
    '    Dim sScript As String

    '    sScript = "opener.parent.Comandi.location.href='ComandiGestImmobili.aspx?Provenienza=" & Request.Item("Provenienza") & "&AzioneProv=" & Request.Item("AzioneProv") & "';"
    '    sScript += "opener.parent.Visualizza.GestImmobili.CmdSalvaVani.click();"
    '    sScript += "window.close()"
    '    Return sScript
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Private Function BackToUI() As String
        Dim sScript As String = ""
        Dim sParametri As String = "IdContribuente=" + Request.Item("IdContribuente") + "&IdTestata=" + Request.Item("IdTestata") + "&IdTessera=" + Request.Item("IdTessera") + "&IdUniqueUI=" + Request.Item("IdUniqueUI") + "&AzioneProv=" + Request.Item("AzioneProv") + "&Provenienza=" + Request.Item("Provenienza") + "&IdList=" & Request.Item("IdListUI")
        sParametri += "&IsFromVariabile=" + ConstSession.IsFromVariabile
        'sScript = "parent.Comandi.location.href='ComandiGestImmobili.aspx?Provenienza=" & Request.Item("Provenienza") & "&AzioneProv=" & Request.Item("AzioneProv") & "';"
        sScript += "parent.Visualizza.location.href='GestImmobili.aspx?" + sParametri + "';"
        sScript += "parent.Comandi.location.href = '../../aspVuotaRemoveComandi';"
        sScript += "parent.Basso.location.href = '../../aspVuotaRemoveComandi';"
        sScript += "parent.Nascosto.location.href = '../../aspVuotaRemoveComandi';"
        Return sScript
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdBack.Click
        Try
            Dim sScript As String
            sScript = BackToUI()
            RegisterScript(sScript, Me.GetType)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.PopUpInsertVani.CmdBack_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** ***
End Class
