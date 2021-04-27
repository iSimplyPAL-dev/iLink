Imports log4net
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
Imports AnagInterface
''' <summary>
''' Pagina per la visualizzazione/gestione delle dichiarazioni.
''' Contiene i dati di dettaglio e le funzioni della comandiera.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class GestDichiarazione
    Inherits BaseEnte
    Private Shared Log As ILog = LogManager.GetLogger("GestDichiarazione")
    Private sScript As String
    Protected FncGrd As New Formatta.FunctionGrd

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

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
    ''' <revision date="27/10/2014">
    ''' <strong>visualizzazione tutti indirizzi spedizione</strong>
    ''' Nuova gestione indirizzi spedizione
    ''' </revision>
    ''' </revisionHistory>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sScript As String = ""
        Dim sSQL As String
        Dim oLoadCombo As New generalClass.generalFunction
        Dim FncTestata As New ClsDichiarazione
        Dim oListTestata() As ObjTestata
        Dim oMyTestata As New ObjTestata

        Try
            '*** X UNIONE CON BANCADATI CMGC ***
            LnkNewUIAnater.Attributes.Add("onclick", "ShowRicUIAnater()")
            LnkNewUIAnater.ToolTip = "Nuovo Immobile da " & ConstSession.NameSistemaTerritorio
            '*** ***
            Session("oListUITessera") = Nothing
            Session("oListImmobili") = Nothing
            Session("oUITemp") = Nothing
            Session("oListRiepilogoPesature") = Nothing

            If Page.IsPostBack = False Then
                'se l'ultima dichiarazione inserita ha il numero solo con caratteri numerici propongo in automatico il successivo
                If TxtNDichiarazione.Text = "" Then
                    TxtNDichiarazione.Text = FncTestata.GetNDichAutomatico(ConstSession.StringConnection, ConstSession.IdEnte)
                End If
                'carico le combo
                sSQL = "SELECT *"
                sSQL += " FROM V_GETANNIRUOLO"
                sSQL += " WHERE IDENTE='" & ConstSession.IdEnte & "'"
                sSQL += " ORDER BY DESCRIZIONE DESC"
                oLoadCombo.LoadComboGenerale(ddlAnno, sSQL, ConstSession.StringConnection, True, Costanti.TipoDefaultCmb.NUMERO)
                'carico le tipologie di dichiazione
                sSQL = "SELECT DESCRIZIONE, IDPROVENIENZA"
                sSQL += " FROM TBLPROVENIENZADICHIARAZIONE"
                sSQL += " ORDER BY DESCRIZIONE"
                oLoadCombo.LoadComboGenerale(DdlTipoDich, sSQL, ConstSession.StringConnection, True, Costanti.TipoDefaultCmb.STRINGA)
                DdlTipoDich.Items(1).Selected = True
                DdlTipoDich.Enabled = False
                'controllo se ho già dei dati da caricare
                If Not Session("oTestata") Is Nothing Then
                    oMyTestata = Session("oTestata")
                    oMyTestata.oAnagrafe = Session("oAnagrafe")
                Else
                    'controllo se sono in visualizzazione di una dichiarazione
                    If Request.Item("IdUniqueTestata") <> "-1" Then
                        'prelevo i dati della dichiarazione
                        oListTestata = FncTestata.GetDichiarazione(ConstSession.StringConnection, Request.Item("IdUniqueTestata"), -1, -1, True, "")
                        If oListTestata Is Nothing Then
                            Response.Redirect("../../PaginaErrore.aspx")
                            Exit Sub
                        End If
                        oMyTestata = oListTestata(0)
                        'nIdContribuente = oMyTestata.IdContribuente
                        'prelevo i dati anagrafici
                        '*** 201504 - Nuova Gestione anagrafica con form unico ***
                        Dim oMyAnagrafica As New Anagrafica.DLL.GestioneAnagrafica()
                        '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
                        oMyTestata.oAnagrafe = oMyAnagrafica.GetAnagrafica(oMyTestata.IdContribuente, Utility.Costanti.INIT_VALUE_NUMBER, String.Empty, ConstSession.DBType, ConstSession.StringConnectionAnagrafica, False)
                        '*** ***
                        '*** ***
                        Session("oTestata") = oMyTestata
                    End If
                End If
                'carico i dati della dichiarazione in videata
                TxtIdDich.Text = oMyTestata.Id
                TxtIdTestata.Text = oMyTestata.IdTestata
                '*** 201504 - Nuova Gestione anagrafica con form unico ***
                LoadAnag(oMyTestata)
                '*** ***
                If oMyTestata.tDataDichiarazione <> Date.MinValue Then
                    TxtDataDichiarazione.Text = oMyTestata.tDataDichiarazione.ToShortDateString()
                End If
                If oMyTestata.sNDichiarazione <> "" Then
                    TxtNDichiarazione.Text = oMyTestata.sNDichiarazione
                End If
                If oMyTestata.tDataProtocollo <> Date.MinValue Then
                    TxtDataProtocollo.Text = oMyTestata.tDataProtocollo.ToShortDateString()
                End If
                TxtNProtocollo.Text = oMyTestata.sNProtocollo
                If oMyTestata.tDataCessazione <> Date.MinValue Then
                    TxtDataCessazione.Text = oMyTestata.tDataCessazione
                End If
                DdlTipoDich.SelectedValue = oMyTestata.sIdProvenienza
                TxtNoteDich.Text = oMyTestata.sNoteDichiarazione
                If oMyTestata.tDataCessazione <> Date.MinValue Then
                    TxtDataCessazione.Text = oMyTestata.tDataCessazione
                End If
                'carico gli oggetti della dichiarazione
                Session("oListTesVSUI") = oMyTestata.oTesUI
                Session("oListTessere") = oMyTestata.oTessere
                Session("oDatiFamiglia") = oMyTestata.oFamiglia
                '*** 201504 - Nuova Gestione anagrafica con form unico ***
                Session("oDatiFamigliaRes") = oMyTestata.dvFamigliaResidenti
                Session("oAnagrafe") = oMyTestata.oAnagrafe
                '*** ***
                '*** X UNIONE CON BANCADATI CMGC ***
                Session("oDettaglioTestata") = oMyTestata.oImmobili
                '*** ***
                'disabilito i comandi di modifica e cancellazione
                If Request.Item("AzioneProv") = Utility.Costanti.AZIONE_NEW Then
                    Abilita(False, 1)
                Else
                    Abilita(False, 0)
                End If
                'controllo se la dichiarazione è già in un ruolo e non ho mai fatto questo controllo
                If TxtIdDich.Text <> -1 And TxtIsInRuolo.Text = -1 Then
                    TxtIsInRuolo.Text = 0
                End If

                'controllo se devo caricare la griglia dei dati UI
                If Not Session("oListTesVSUI") Is Nothing Then
                    GrdTessere.DataSource = Session("oListTesVSUI")
                    GrdTessere.DataBind()
                    LblResultUITes.Style.Add("display", "none")
                Else
                    LblResultUITes.Style.Add("display", "")
                End If
                'controllo se devo caricare la griglia dei dati UI
                If Not Session("oDettaglioTestata") Is Nothing Then
                    GrdImmobili.DataSource = Session("oDettaglioTestata")
                    GrdImmobili.DataBind()
                    LblResultUI.Style.Add("display", "none")
                Else
                    LblResultUI.Style.Add("display", "")
                End If
                '*** X UNIONE CON BANCADATI CMGC ***
                If ConstSession.IsFromVariabile = "1" Then
                    DivTessere.Style.Add("display", "")
                    DivImmobili.Style.Add("display", "none")
                Else
                    DivTessere.Style.Add("display", "none")
                    DivImmobili.Style.Add("display", "")
                End If
                '*** ***
                Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Immobile, "Gestione", Utility.Costanti.AZIONE_LETTURA.ToString, ConstSession.CodTributo, ConstSession.IdEnte, oMyTestata.Id)
            End If
            If hfCalcoloPuntuale.Value = 1 Then
                sScript += "divCalcolo.style.display = '';divDichiarazione.style.display = 'none';"
                If ConstSession.IsFromVariabile <> "1" Then
                    sScript += "$('#DataConf').hide();$('#divCalcTessere').hide();"
                End If
            Else
                sScript += "divCalcolo.style.display = 'none';divDichiarazione.style.display = '';"
            End If
            '*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI & DIVERSA GESTIONE QUOTA VARIABILE***
            If ConstSession.HasPlainAnag Then
                sScript += "document.getElementById('TRSpecAnag').style.display='none';"
            Else
                sScript += "document.getElementById('TRPlainAnag').style.display='none';"
            End If
            If ConstSession.UserName.ToUpper <> Costanti.SuperUser Then
                sScript += "parent.Comandi.document.getElementById('Dovuto').style.display='none';"
            End If
            RegisterScript(sScript, Me.GetType)
            '*** ***
            If ConstSession.VisualGIS = False Then
                GrdImmobili.Columns(8).Visible = False
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDichiarazione.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            RegisterScript("parent.Basso.location.href='../../aspVuota.aspx';", Me.GetType)
        End Try
    End Sub
#Region "Griglie"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdRowCommand(ByVal sender As Object, e As GridViewCommandEventArgs)
        Dim nRow As Integer = -1
        Try
            Dim IDRow As Integer = CInt(e.CommandArgument.ToString())
            Select Case e.CommandName
                Case "RowOpen"
                    If CType(sender, Ribes.OPENgov.WebControls.RibesGridView).UniqueID = "GrdImmobili" Then
                        For Each myRow As GridViewRow In GrdImmobili.Rows
                            nRow += 1
                            If CType(myRow.FindControl("hfId"), HiddenField).Value = IDRow Then
                                ShowPopUp("I", -1, IDRow, nRow, Utility.Costanti.AZIONE_LETTURA)
                            End If
                        Next
                    Else
                        For Each myRow As GridViewRow In GrdTessere.Rows
                            nRow += 1
                            If CType(myRow.FindControl("hfIdUI"), HiddenField).Value = IDRow Then
                                ShowPopUp("I", CType(myRow.FindControl("hfIdTessera"), HiddenField).Value, IDRow, nRow, Utility.Costanti.AZIONE_LETTURA)
                            End If
                        Next
                    End If
                Case "RowTessera"
                    For Each myRow As GridViewRow In GrdTessere.Rows
                        nRow += 1
                        If CType(myRow.FindControl("hfIdTessera"), HiddenField).Value = IDRow Then
                            ShowPopUp("T", CType(myRow.FindControl("hfIdTessera"), HiddenField).Value, IDRow, nRow, Utility.Costanti.AZIONE_LETTURA)
                        End If
                    Next
            End Select
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDichiarazione.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Protected Sub GrdRowCommand(ByVal sender As Object, e As GridViewCommandEventArgs)
    '    Try
    '        Dim IDRow As Integer = CInt(e.CommandArgument.ToString())
    '        Select Case e.CommandName
    '            Case "RowOpen"
    '                If CType(sender, Ribes.OPENgov.WebControls.RibesGridView).UniqueID = "GrdImmobili" Then
    '                    For Each myRow As GridViewRow In GrdImmobili.Rows
    '                        If CType(myRow.FindControl("hfId"), HiddenField).Value = IDRow Then
    '                            ShowPopUp("I", -1, IDRow, Utility.Costanti.AZIONE_LETTURA)
    '                        End If
    '                    Next
    '                Else
    '                    For Each myRow As GridViewRow In GrdTessere.Rows
    '                        If CType(myRow.FindControl("hfIdUI"), HiddenField).Value = IDRow Then
    '                            ShowPopUp("I", CType(myRow.FindControl("hfIdTessera"), HiddenField).Value, IDRow, Utility.Costanti.AZIONE_LETTURA)
    '                        End If
    '                    Next
    '                End If
    '            Case "RowTessera"
    '                For Each myRow As GridViewRow In GrdTessere.Rows
    '                    If CType(myRow.FindControl("hfIdTessera"), HiddenField).Value = IDRow Then
    '                        ShowPopUp("T", CType(myRow.FindControl("hfIdTessera"), HiddenField).Value, IDRow, Utility.Costanti.AZIONE_LETTURA)
    '                    End If
    '                Next
    '        End Select
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDichiarazione.GrdRowCommand.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' Gestione del cambio pagina della griglia.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdPageIndexChanging(ByVal sender As Object, e As GridViewPageEventArgs)
        LoadSearch(e.NewPageIndex)
    End Sub
    ''*** ***
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            If Not Session("oListTesVSUI") Is Nothing Then
                GrdTessere.DataSource = Session("oListTesVSUI")
                If page.HasValue Then
                    GrdTessere.PageIndex = page.Value
                End If
                GrdTessere.DataBind()
            End If
            'controllo se devo caricare la griglia dei dati UI
            If Not Session("oDettaglioTestata") Is Nothing Then
                GrdImmobili.DataSource = Session("oDettaglioTestata")
                If page.HasValue Then
                    GrdImmobili.PageIndex = page.Value
                End If
                GrdImmobili.DataBind()
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDichiarazione.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdSaveDatiDich_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdSaveDatiDich.Click
        Dim x, y, z As Integer

        '***SE SONO IN NUOVO INSERIMENTO DICHIARAZIONE SALVO I DATI SOLO IN MEMORIA
        '***SE SONO IN MODIFICA DI UN VANO GIA' INSERITO SALVO I DATI IN MEMORIA E SUL DATABASE
        '***SE SONO IN AGGIUNTA DI UN VANO AD UNA DICHIARAZIONE GIA' INSERITA SALVO I DATI IN MEMORIA E SUL DATABASE

        Try
            '*** X UNIONE CON BANCADATI CMGC ***
            If ConstSession.IsFromVariabile = "1" Then
                'controllo che tutti gli immobili hanno vani, mq e che le date siano coerenti con quelle della dichiarazione
                If Not Session("oListTessere") Is Nothing Then
                    Dim oMyDettaglio() As ObjTessera
                    oMyDettaglio = Session("oListTessere")
                    For x = 0 To oMyDettaglio.GetUpperBound(0)
                        If Not IsNothing(oMyDettaglio(x).oImmobili) Then
                            'controllo che tutti i vani abbiamo la categoria
                            For y = 0 To oMyDettaglio(x).oImmobili.GetUpperBound(0)
                                'la data inizio è obbligatoria
                                If oMyDettaglio(x).oImmobili(y).tDataInizio = Date.MinValue Then
                                    sScript = "GestAlert('a', 'warning', '', '', 'Inserire la Data Inizio su tutti gli Immobili!');"
                                    RegisterScript(sScript, Me.GetType)
                                    Exit Sub
                                End If
                                If Not oMyDettaglio(x).oImmobili(y).oOggetti Is Nothing Then
                                    Dim oMyOggetti() As ObjOggetti
                                    oMyOggetti = oMyDettaglio(x).oImmobili(y).oOggetti
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
                        End If
                    Next

                    If sScript = "" Then
                        sScript = "document.getElementById('btnSalvaDichiarazione').click();"
                        RegisterScript(sScript, Me.GetType)
                    End If
                Else
                    sScript = "GestAlert('a', 'warning', '', '', 'Inserire almeno un\'immobile!');"
                    RegisterScript(sScript, Me.GetType)
                    Exit Sub
                End If
            ElseIf Not Session("oDettaglioTestata") Is Nothing Then
                'controllo che tutti gli immobili hanno vani, mq e che le date siano coerenti con quelle della dichiarazione
                Dim oListDettaglio() As ObjDettaglioTestata
                oListDettaglio = Session("oDettaglioTestata")
                'controllo che tutti i vani abbiamo la categoria
                For y = 0 To oListDettaglio.GetUpperBound(0)
                    'la data inizio è obbligatoria
                    If oListDettaglio(y).tDataInizio = Date.MinValue Then
                        sScript = "GestAlert('a', 'warning', '', '', 'Inserire la Data Inizio su tutti gli Immobili!');"
                        RegisterScript(sScript, Me.GetType)
                        Exit Sub
                    End If
                    If Not oListDettaglio(y).oOggetti Is Nothing Then
                        Dim oListOggetti() As ObjOggetti
                        oListOggetti = oListDettaglio(y).oOggetti
                        For z = 0 To oListOggetti.GetUpperBound(0)
                            If ConstSession.IsFromTARES = "1" Then
                                If oListOggetti(z).IdCatTARES = -1 Then
                                    sScript = "GestAlert('a', 'warning', '', '', 'Inserire la Categoria su tutti i vani!');"
                                    RegisterScript(sScript, Me.GetType)
                                    Exit Sub
                                End If
                            Else
                                If (oListOggetti(z).IdCategoria = "" Or oListOggetti(z).IdCategoria = "-1") Then
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
                    sScript = "document.getElementById('btnSalvaDichiarazione').click();"
                    RegisterScript(sScript, Me.GetType)
                End If
            Else
                sScript = "GestAlert('a', 'warning', '', '', 'Inserire almeno un\'immobile!');"
                RegisterScript(sScript, Me.GetType)
                Exit Sub
            End If

        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDichiarazione.CmdSaveDatiDich_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** 201504 - Nuova Gestione anagrafica con form unico ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub LnkAnagTributi_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles LnkAnagTributi.Click
        Try
            Session.Remove(ViewState("sessionName"))

            Dim oDettaglioAnagrafica As New DettaglioAnagrafica
            oDettaglioAnagrafica.COD_CONTRIBUENTE = hdIdContribuente.Value
            oDettaglioAnagrafica.ID_DATA_ANAGRAFICA = TxtIdDataAnagrafica.Text
            ViewState("sessionName") = "codContribuente"
            Session(ViewState("sessionName")) = oDettaglioAnagrafica
            writeJavascriptAnagrafica(ViewState("sessionName"))
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDichiarazione.LnkAnagTributi_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnRibalta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRibalta.Click
        Dim oDettaglioAnagrafica As DettaglioAnagrafica
        Try
            If Not Session(ViewState("sessionName")) Is Nothing Then
                oDettaglioAnagrafica = Session(ViewState("sessionName"))
                Session("oAnagrafe") = oDettaglioAnagrafica
                TxtCodFiscale.Text = oDettaglioAnagrafica.CodiceFiscale
                TxtPIva.Text = oDettaglioAnagrafica.PartitaIva
                TxtCognome.Text = oDettaglioAnagrafica.Cognome
                TxtNome.Text = oDettaglioAnagrafica.Nome
                Select Case oDettaglioAnagrafica.Sesso
                    Case "F"
                        F.Checked = True
                    Case "G"
                        G.Checked = True
                    Case "M"
                        M.Checked = True
                End Select
                TxtDataNascita.Text = oDettaglioAnagrafica.DataNascita
                TxtLuogoNascita.Text = oDettaglioAnagrafica.ComuneNascita
                TxtResVia.Text = oDettaglioAnagrafica.ViaResidenza
                TxtResCivico.Text = oDettaglioAnagrafica.CivicoResidenza
                TxtResEsponente.Text = oDettaglioAnagrafica.EsponenteCivicoResidenza
                TxtResInterno.Text = oDettaglioAnagrafica.InternoCivicoResidenza
                TxtResScala.Text = oDettaglioAnagrafica.ScalaCivicoResidenza
                TxtResCAP.Text = oDettaglioAnagrafica.CapResidenza
                TxtResComune.Text = oDettaglioAnagrafica.ComuneResidenza
                TxtResPv.Text = oDettaglioAnagrafica.ProvinciaResidenza

                hdIdContribuente.Value = oDettaglioAnagrafica.COD_CONTRIBUENTE
                TxtIdDataAnagrafica.Text = oDettaglioAnagrafica.ID_DATA_ANAGRAFICA
                ViewState("sessionName") = ""
                Session("SEARCHPARAMETRES") = Nothing
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDichiarazione.btnRibalta_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnRibaltaAnagAnater_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRibaltaAnagAnater.Click
        Dim oDettaglioAnagrafica As DettaglioAnagrafica
        Try
            If Not IsNothing(Session("AnagrafeAnaterRibaltata")) Then
                oDettaglioAnagrafica = CType(Session("AnagrafeAnaterRibaltata"), DettaglioAnagrafica)
                Session("oAnagrafe") = oDettaglioAnagrafica

                TxtCodFiscale.Text = oDettaglioAnagrafica.CodiceFiscale
                TxtPIva.Text = oDettaglioAnagrafica.PartitaIva
                TxtCognome.Text = oDettaglioAnagrafica.Cognome
                TxtNome.Text = oDettaglioAnagrafica.Nome
                Select Case oDettaglioAnagrafica.Sesso
                    Case "F"
                        F.Checked = True
                    Case "G"
                        G.Checked = True
                    Case "M"
                        M.Checked = True
                End Select
                TxtDataNascita.Text = oDettaglioAnagrafica.DataNascita
                TxtLuogoNascita.Text = oDettaglioAnagrafica.ComuneNascita
                TxtResVia.Text = oDettaglioAnagrafica.ViaResidenza
                TxtResCivico.Text = oDettaglioAnagrafica.CivicoResidenza
                TxtResEsponente.Text = oDettaglioAnagrafica.EsponenteCivicoResidenza
                TxtResInterno.Text = oDettaglioAnagrafica.InternoCivicoResidenza
                TxtResScala.Text = oDettaglioAnagrafica.ScalaCivicoResidenza
                TxtResCAP.Text = oDettaglioAnagrafica.CapResidenza
                TxtResComune.Text = oDettaglioAnagrafica.ComuneResidenza
                TxtResPv.Text = oDettaglioAnagrafica.ProvinciaResidenza
                hdIdContribuente.Value = oDettaglioAnagrafica.COD_CONTRIBUENTE
                TxtIdDataAnagrafica.Text = oDettaglioAnagrafica.ID_DATA_ANAGRAFICA

                ' Pulisco la variabile di sessione della anagrafica di anater.
                Session.Remove("AnagrafeAnaterRibaltata")
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDichiarazione.btnRibaltaAnagAnater_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub LnkNewTessera_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles LnkNewTessera.Click
        Session("oTessera") = Nothing
        ShowPopUp("T", -1, -1, -1, Utility.Costanti.AZIONE_NEW)
    End Sub

    '*** X UNIONE CON BANCADATI CMGC ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub LnkNewUI_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles LnkNewUI.Click
        Try
            'apro il popup passandogli l'indice dell'array da visualizzare
            ShowPopUp("I", -1, -1, -1, Utility.Costanti.AZIONE_NEW)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDichiarazione.LknNewUI_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdClearDatiDich_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdClearDatiDich.Click
        Try
            'ripulisco tutti i dati di sessioni dati dichiarazione
            ClearDatiDich()
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDichiarazione.CmdClearDatiDich_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdModDich_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdModDich.Click
        Dim oListOldDich As New ObjTestata
        Dim sTemp As String

        Try
            Abilita(True, 0)
            Abilita(False, 1)
            'memorizzo la dichiarazione originale
            oListOldDich = Session("oTestata")
            Session("oTestataOrg") = Session("oTestata")
            If IsNothing(Session("sOldMyDich")) Then
                'dipe 27/07/2010
                sTemp = hdIdContribuente.Value + "|" + TxtDataDichiarazione.Text + "|" + TxtNDichiarazione.Text + "|" + TxtDataProtocollo.Text + "|" + TxtNProtocollo.Text + "|" + TxtDataCessazione.Text
                sTemp += "|" + DdlTipoDich.SelectedItem.Value.ToString
                If Not oListOldDich.oTessere Is Nothing Then
                    sTemp += "|" + oListOldDich.oTessere.GetUpperBound(0).ToString
                End If
                If Not oListOldDich.oImmobili Is Nothing Then
                    sTemp += "|" + oListOldDich.oImmobili.GetUpperBound(0).ToString
                End If
                Session("sOldMyDich") = sTemp
            End If
            '*** 20130923 - gestione modifiche tributarie ***
            Session("OldContribuente") = hdIdContribuente.Value
            '*** ***
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDichiarazione.CmdModDich_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdDeleteDich_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdDeleteDich.Click
        Dim oMyTestata As New ObjTestata
        Dim FunctionTrovaTestata As New ClsDichiarazione

        Try
            If TxtIdTestata.Text <> "-1" Then
                'aggiorno il database
                oMyTestata = Session("oTestata")
                'Inserisce le date DATA_VARIAZIONE e DATA_CESSAZIONE in TBLTESTATA, TBLTesseraRSU, TBLOGGETTI
                If FunctionTrovaTestata.DeleteDichiarazione(ConstSession.DBType, ConstSession.StringConnection, ConstSession.StringConnectionOPENgov, oMyTestata) = 0 Then
                    sScript = "GestAlert('a', 'success', '', '', 'Cancellazione effettuata correttamente!');"
                    sScript += "parent.Visualizza.location.href='RicercaDichiarazione.aspx';"
                    sScript += "parent.Comandi.location.href='ComandiRicDichiarazione.aspx';"
                    RegisterScript(sScript, Me.GetType)
                Else
                    sScript = "GestAlert('a', 'success', '', '', 'Cancellazione effettuata correttamente!');"
                    sScript += "parent.Visualizza.location.href='RicercaDichiarazione.aspx';"
                    sScript += "parent.Comandi.location.href='ComandiRicDichiarazione.aspx';"
                    RegisterScript(sScript, Me.GetType)
                End If
            Else
                sScript = "GestAlert('a', 'warning', '', '', 'E\' possibile eliminare solo le dichiazioni gia\' salvate sulla base dati!');"
                RegisterScript(sScript, Me.GetType)
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDichiarazione.CmdDeleteDich_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdPopUpUI_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdPopUpUI.Click
        ShowPopUp("I", -1, -1, -1, Utility.Costanti.AZIONE_NEW)
    End Sub

    ''' <summary>
    ''' Pulsante per l'estrazione in formato XLS della ricerca effettuata
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdStampa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdStampa.Click
        Dim sPathProspetti, sNameXLS As String
        Dim FncStampa As New ClsStampaXLS
        Dim oListTestata() As ObjTestata
        Dim DtDatiStampa As New DataTable
        Dim aMyHeaders As String() = Nothing
        Dim aListColonne As ArrayList
        Dim x, nCol As Integer

        nCol = 23
        'valorizzo il nome del file
        sPathProspetti = ConstSession.PathProspetti
        sNameXLS = ConstSession.IdEnte & "_DETTAGLIO_DICHIARAZIONI_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
        Try
            'prelevo i dati da stampare
            If Not Session("oTestata") Is Nothing Then
                ReDim Preserve oListTestata(0)
                oListTestata(0) = Session("oTestata")
                DtDatiStampa = FncStampa.PrintDichiarazioni(oListTestata, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, ConstSession.IsFromTARES)
                If Not DtDatiStampa Is Nothing Then
                    'definisco le colonne
                    aListColonne = New ArrayList
                    For x = 0 To nCol
                        aListColonne.Add("")
                    Next
                    aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDichiarazione.CmdStampa_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
        If Not DtDatiStampa Is Nothing Then
            'definisco l'insieme delle colonne da esportare
            Dim MyCol() As Integer = New Integer(nCol) {}
            For x = 0 To nCol
                MyCol(x) = x
            Next
            'esporto i dati in excel
            Dim MyStampa As New RKLib.ExportData.Export("Web")
            MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
        End If
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnSalvaDichiarazione_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSalvaDichiarazione.Click
        Dim sNewMyDich As String
        Dim myFunctionDich As New ClsDichiarazione
        Dim FncTestata As New Utility.DichManagerTARSU(ConstSession.DBType, ConstSession.StringConnection, ConstSession.StringConnectionOPENgov, ConstSession.IdEnte) 'As New GestTestata
        Dim NewDichiarazione As Integer
        Dim oMyTestata As New ObjTestata
        Dim oMyTestataOrg As New ObjTestata
        Dim FncModificheTributarie As New Utility.ModificheTributarie

        Try
            NewDichiarazione = 1

            'controllo se ho già una dichiarazione caricata
            If Not Session("oTestataOrg") Is Nothing Then
                'carico l'array originario
                oMyTestataOrg = New ObjTestata
                oMyTestataOrg = Session("oTestataOrg")
            End If
            'carico i dati dal form
            oMyTestata = New ObjTestata
            oMyTestata.Id = TxtIdDich.Text
            oMyTestata.IdTestata = TxtIdTestata.Text
            oMyTestata.sEnte = ConstSession.IdEnte
            oMyTestata.IdContribuente = hdIdContribuente.Value
            oMyTestata.tDataDichiarazione = TxtDataDichiarazione.Text.Trim
            oMyTestata.sNDichiarazione = TxtNDichiarazione.Text.Trim
            If TxtDataProtocollo.Text.Trim <> "" Then
                oMyTestata.tDataProtocollo = TxtDataProtocollo.Text.Trim
            End If
            oMyTestata.sNProtocollo = TxtNProtocollo.Text.Trim
            oMyTestata.sIdProvenienza = DdlTipoDich.SelectedItem.Value
            oMyTestata.sNoteDichiarazione = TxtNoteDich.Text
            oMyTestata.tDataInserimento = Now
            oMyTestata.tDataVariazione = Nothing
            oMyTestata.sOperatore = Session("username")
            oMyTestata.oTessere = Session("oListTessere")
            oMyTestata.oFamiglia = Session("oDatiFamiglia")
            oMyTestata.oAnagrafe = Session("oAnagrafe")
            oMyTestata.oImmobili = Session("oDettaglioTestata")
            If TxtDataCessazione.Text <> "" Then
                oMyTestata.tDataCessazione = TxtDataCessazione.Text.Trim
                'devo valorizzare la data di chiusura sugli immobili
                If Not oMyTestata.oTessere Is Nothing Then
                    For Each item As ObjTessera In oMyTestata.oTessere
                        If item.tDataCessazione = Date.MinValue Or item.tDataCessazione = Date.MaxValue Then
                            item.tDataCessazione = oMyTestata.tDataCessazione
                        End If
                    Next
                End If
                If Not oMyTestata.oImmobili Is Nothing Then
                    For Each item As ObjDettaglioTestata In oMyTestata.oImmobili
                        If item.tDataFine = Date.MinValue Or item.tDataFine = Date.MaxValue Then
                            item.tDataFine = oMyTestata.tDataCessazione
                        End If
                    Next
                End If
            Else
                oMyTestata.tDataCessazione = Nothing
            End If
            'controllo se sono in modifica di una testata
            If Request.Item("IdUniqueTestata") <> "-1" Then
                'memorizzo il nuovo immobile
                sNewMyDich = hdIdContribuente.Value + "|" + TxtDataDichiarazione.Text + "|" + TxtNDichiarazione.Text + "|" + TxtDataProtocollo.Text + "|" + TxtNProtocollo.Text + "|" + TxtDataCessazione.Text
                sNewMyDich += "|" + DdlTipoDich.SelectedItem.Value.ToString
                If Not oMyTestata.oTessere Is Nothing Then
                    sNewMyDich += "|" + oMyTestata.oTessere.GetUpperBound(0).ToString
                End If
                If Not oMyTestata.oImmobili Is Nothing Then
                    sNewMyDich += "|" + oMyTestata.oImmobili.GetUpperBound(0).ToString
                End If
                'controllo se sono state apportate modifiche
                If Not Session("sOldMyDich") Is Nothing Then 'altrimenti non è mai stata abilitata la modifica
                    If sNewMyDich.CompareTo(Session("sOldMyDich")) <> 0 Then
                        oMyTestataOrg.tDataVariazione = Now
                        'storicizzo la dichiarazione originale
                        If FncTestata.SetTestata(Utility.Costanti.AZIONE_UPDATE, oMyTestataOrg) = 0 Then
                            Response.Redirect("../../PaginaErrore.aspx")
                            Exit Sub
                        End If
                        oMyTestata.Id = -1
                        oMyTestata.sOperatore = ConstSession.UserName
                        'devo reinserire tutto
                        NewDichiarazione = FncTestata.SetDichiarazione(oMyTestata, ConstSession.IsFromVariabile)
                        '*** 20130923 - gestione modifiche tributarie ***
                        If Session("OldContribuente") <> oMyTestata.IdContribuente Then
                            For Each item As ObjDettaglioTestata In oMyTestata.oImmobili
                                If FncModificheTributarie.SetModificheTributarie(ConstSession.StringConnectionOPENgov, Utility.ModificheTributarie.DBOperation.Insert, 0, ConstSession.IdEnte, Session("COD_TRIBUTO"), Utility.ModificheTributarie.ModificheTributarieCausali.VariazioneContribuente, item.sFoglio, item.sNumero, item.sSubalterno, Now, ConstSession.UserName, item.Id, Date.MaxValue) = False Then
                                    Log.Debug("Errore in SetModificheTributarie")
                                End If
                            Next
                        End If
                    Else
                        'se ho variato le note aggiorno la dichiarazione originale
                        If oMyTestataOrg.sNoteDichiarazione <> oMyTestata.sNoteDichiarazione Then
                            If FncTestata.SetTestata(Utility.Costanti.AZIONE_UPDATE, oMyTestata) = 0 Then
                                Response.Redirect("../../PaginaErrore.aspx")
                                Exit Sub
                            End If
                        End If
                    End If
                End If
                'svuoto la variabile di riferimento
                Session("sOldMyDich") = Nothing
            Else
                'controllo che numero e data dichiarazione siano univoci
                If myFunctionDich.IsUniqueDich(ConstSession.StringConnection, ConstSession.IdEnte, Convert.ToDateTime(TxtDataDichiarazione.Text.Trim), TxtNDichiarazione.Text.Trim) = 0 Then
                    sScript = "GestAlert('a', 'warning', '', '', 'Numero e Data dichiarazione devono essere univoci!');"
                    RegisterScript(sScript, Me.GetType)
                    Exit Sub
                End If
                'aggiorno il database
                NewDichiarazione = FncTestata.SetDichiarazione(oMyTestata, ConstSession.IsFromVariabile)
                Try
                    '*** 20130923 - gestione modifiche tributarie ***
                    For Each item As ObjDettaglioTestata In oMyTestata.oImmobili
                        If FncModificheTributarie.SetModificheTributarie(ConstSession.StringConnectionOPENgov, Utility.ModificheTributarie.DBOperation.Insert, 0, ConstSession.IdEnte, ConstSession.CodTributo, Utility.ModificheTributarie.ModificheTributarieCausali.NuovaDichiarazione, item.sFoglio, item.sNumero, item.sSubalterno, Now, ConstSession.UserName, item.Id, Date.MaxValue) = False Then
                            Log.Debug("Errore in SetModificheTributarie")
                        End If
                    Next
                    '*** ***
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDichiarazione.btnSalvaDichiarazione_Click.SetModificheTributarie.errore", ex)
                End Try
            End If
            'memorizzo l'oggetto nella sessione
            Session("oTestata") = oMyTestata

            If NewDichiarazione = 0 Then
                sScript += "GestAlert('a', 'warning', '', '', 'Inserimento non effettuato!');"
                RegisterScript(sScript, Me.GetType)
                'ClearDatiDich()
            Else
                sScript += "GestAlert('a', 'success', '', '', 'Inserimento effettuato correttamente!');"
                sScript += "parent.Visualizza.location.href='RicercaDichiarazione.aspx';"
                sScript += "parent.Comandi.location.href='ComandiRicDichiarazione.aspx';"
                RegisterScript(sScript, Me.GetType)
                ClearDatiDich()
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDichiarazione.btnSalvaDichiarazione_Click.errore: ", Err)
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
        Dim CodeGIS, sScript, sRifPrec As String
        Dim fncGIS As New RemotingInterfaceAnater.GIS
        Dim listRifCat As New Generic.List(Of Anater.Oggetti.RicercaUnitaImmobiliareAnater)
        Dim myRifCat As New Anater.Oggetti.RicercaUnitaImmobiliareAnater
        Try
            sRifPrec = ""
            For Each grdRow As GridViewRow In GrdImmobili.Rows
                If grdRow.Cells(19).Text.Replace("&nbsp;", "") <> "" Then
                    If CType(grdRow.Cells(9).FindControl("chkSel"), CheckBox).Checked And sRifPrec <> grdRow.Cells(17).Text.Replace("&nbsp;", "") + "|" + grdRow.Cells(18).Text.Replace("&nbsp;", "") + "|" + grdRow.Cells(19).Text.Replace("&nbsp;", "") Then
                        myRifCat = New Anater.Oggetti.RicercaUnitaImmobiliareAnater
                        myRifCat.Foglio = grdRow.Cells(19).Text.Replace("&nbsp;", "")
                        myRifCat.Mappale = grdRow.Cells(20).Text.Replace("&nbsp;", "")
                        myRifCat.Subalterno = grdRow.Cells(21).Text.Replace("&nbsp;", "")
                        myRifCat.CodiceRicerca = ConstSession.Belfiore
                        listRifCat.Add(myRifCat)
                    End If
                End If
                sRifPrec = grdRow.Cells(19).Text.Replace("&nbsp;", "") + "|" + grdRow.Cells(20).Text.Replace("&nbsp;", "") + "|" + grdRow.Cells(21).Text.Replace("&nbsp;", "")
            Next
            If listRifCat.ToArray.Length > 0 Then
                CodeGIS = fncGIS.getGIS(ConstSession.UrlWSGIS, listRifCat.ToArray())
                If Not CodeGIS Is Nothing Then
                    sScript = "window.open('" & ConstSession.UrlWebGIS & CodeGIS & "','wdwGIS');"
                    RegisterScript(sScript, Me.GetType)
                Else
                    sScript = "GestAlert('a', 'danger', '', '', 'Errore in interrogazione Cartografia!');"
                    RegisterScript(sScript, Me.GetType)
                End If
            Else
                sScript = "GestAlert('a', 'warning', '', '', 'Per accedere alla Cartografia avere almeno un foglio!');"
                RegisterScript(sScript, Me.GetType)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDichiarazione.CmdGIS_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdCalcola_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdCalcola.Click
        'calcolo gli articoli per i vari anni
        Dim oMyRuolo As New ObjRuolo
        Dim sTipoElab, sScript As String
        Dim FncRuolo As New ClsGestRuolo
        Dim FncCalcolo As New ClsElabRuolo
        Dim sErr As String = ""
        Dim nCheck As Integer
        Dim calcolo As New ClsCalcoloRuolo
        Dim ListAddizionali() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoAddizionale
        Dim ListFlussi As New ArrayList
        Dim dScadenza As Date

        Try
            sScript = ""
            sTipoElab = ObjRuolo.Generazione.DaDichiarazione
            CacheManager.RemoveRiepilogoCalcoloMassivo()
            CacheManager.RemoveAvanzamentoElaborazione()
            CacheManager.RemoveRiepilogoCalcoloMassivoArtVSCatPF()
            CacheManager.RemoveRiepilogoCalcoloMassivoArtVSCatPV()
            CacheManager.RemoveRiepilogoCalcoloMassivoArtVSCatPC()
            CacheManager.RemoveRiepilogoCalcoloMassivoArtVSCatPM()
            CacheManager.RemoveRiepilogoImportRuoli()
            'visualizzo il pannello di attesa
            DivAttesa.Style.Add("display", "")
            sTipoElab = ObjRuolo.Generazione.DaDichiarazione
            Date.TryParse(txtScadenza.Text, dScadenza)
            If dScadenza = Date.MinValue Or dScadenza = Date.MaxValue Then
                sScript = "GestAlert('a', 'warning', '', '', 'Inserire la data di scadenza!');"
                RegisterScript(sScript, Me.GetType)
                DivAttesa.Style.Add("display", "none")
                Exit Sub
            End If
            'controllo la presenza delle tariffe per tutte le categorie
            nCheck = FncCalcolo.CheckTariffe(ConstSession.StringConnection, "TARES", ConstSession.IdEnte, ddlAnno.SelectedValue, sErr)
            If nCheck = 0 Then
                sScript = "GestAlert('a', 'warning', '', '', 'Attenzione! le seguenti categorie non hanno una tariffa associata.\nImpossibile proseguire!\n" & sErr & "');"
                RegisterScript(sScript, Me.GetType)
                DivAttesa.Style.Add("display", "none")
                Exit Sub
            ElseIf nCheck = -1 Then
                sScript = "GestAlert('a', 'warning', '', '', 'Attenzione!\nNon sono presenti dichiarazioni o articoli per la generazione del ruolo!');"
                RegisterScript(sScript, Me.GetType)
                DivAttesa.Style.Add("display", "none")
                Exit Sub
            End If
            ListAddizionali = New GestAddizionali().GetAddizionale(ConstSession.StringConnection, ConstSession.IdEnte, ddlAnno.SelectedValue, "", ObjRuolo.Ruolo.APercentuale)
            If ListAddizionali Is Nothing Then
                sScript = "GestAlert('a', 'warning', '', '', 'Errore in lettura addizionali.\nImpossibile proseguire!');"
                RegisterScript(sScript, Me.GetType)
                Exit Sub
            End If
            Dim bIsFromVariabile As Boolean = False
            If ConstSession.IsFromVariabile = "1" Then
                bIsFromVariabile = True
            End If
            oMyRuolo = New ClsCalcoloRuolo().StartCalcoloPuntuale(ConstSession.DBType, ConstSession.StringConnection, ConstSession.IdEnte, sTipoElab, ObjRuolo.TipoCalcolo.TARES, ObjRuolo.Ruolo.Singolo, "PUNTUALE", 100, False, True, "D", ddlAnno.SelectedValue, 0, -1, hdIdContribuente.Value, chkSimulazione.Checked, bIsFromVariabile, CDate(New generalClass.generalFunction().FormattaData(ddlAnno.SelectedValue + "0101", "G")), CDate(New generalClass.generalFunction().FormattaData(ddlAnno.SelectedValue + "1231", "G")), ListAddizionali, ListFlussi, ConstSession.Belfiore, ConstSession.StringConnectionAnagrafica, ConstSession.StringConnectionOPENgov, ConstSession.StringConnectionICI, ConstSession.StringConnectionProvv, ConstSession.UrlServiziLiquidazione, ConstSession.UrlServiziAccertamenti, ConstSession.UserName, 60, dScadenza)
            'carico i dati dell'avviso
            If Not oMyRuolo Is Nothing Then
                If Not oMyRuolo.oAvvisi Is Nothing Then
                    For Each myAvviso As ObjAvviso In oMyRuolo.oAvvisi
                        Dim oListAvvisi() As ObjAvviso = New GestAvviso().GetAvviso(ConstSession.StringConnection, myAvviso.ID, ConstSession.IdEnte, myAvviso.IdFlussoRuolo, "", "", "", "", "", "", "", "", "", True, False, False, False, "", -1, Nothing)
                        For Each myItem As ObjAvviso In oListAvvisi
                            LoadAvviso(myItem)
                            Session("oMyAvviso") = myItem
                            For x As Integer = 1 To 4
                                '1 = imposta la data della stampa della minuta
                                '2 = imposta la data di approvazione della minuta
                                '3 = imposta la data di cartellazione
                                '4 = imposta la data di elaborazione documenti
                                If New ClsGestRuolo().SetDateRuolo(myAvviso.IdFlussoRuolo, x, "I") = 0 Then
                                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDichiarazione.CmdDocumenti_Click.errore in SetDateRuoliGenerati")
                                    Exit For
                                End If
                            Next
                        Next
                    Next
                Else
                    LblDatiAvviso.Text = oMyRuolo.sNote
                    LblEmesso.Text = "" : Label38.Style.Add("display", "none")
                    LblResultTessere.Style.Add("display", "")
                    LblResultArticoli.Style.Add("display", "")
                    LblResultDettVoci.Style.Add("display", "")
                    LblResultRate.Style.Add("display", "")
                End If
            Else
                LblDatiAvviso.Text = "Ruolo non calcolato."
                LblEmesso.Text = "" : Label38.Style.Add("display", "none")
                LblResultTessere.Style.Add("display", "")
                LblResultArticoli.Style.Add("display", "")
                LblResultDettVoci.Style.Add("display", "")
                LblResultRate.Style.Add("display", "")
            End If
            sScript += "divCalcolato.style.display = '';"
            RegisterScript(sScript, Me.GetType)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDichiarazione.CmdCalcola_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            DivAttesa.Style.Add("display", "none")
        End Try
    End Sub
    ''' <summary>
    ''' Pulsante per la ristampa del documento.
    ''' </summary>
    Private Sub CmdDocumenti_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdDocumenti.Click
        Dim sScript, sTypeOrd, sNameModello, sTipoRuolo As String
        Dim nTipoElab As Integer = 1
        Dim FncDoc As New ClsGestDocumenti
        Dim nReturn, nMaxDocPerFile, nDocDaElab, nDocElab, x As Integer
        Dim oListDocStampati() As RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL = Nothing
        Dim bCreaPDF As Boolean = False
        Dim oListAvvisi(0) As ObjAvviso
        Dim oListAvvisiConv() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoCartella
        Dim nDecimal As Integer = 2

        Try
            Session.Remove("ELENCO_DOCUMENTI_STAMPATI")
            sTypeOrd = "Nominativo"
            sNameModello = "TARSU_ORDINARIO"
            sTipoRuolo = "O"
            nMaxDocPerFile = ConstSession.nMaxDocPerFile
            nDocDaElab = 1
            nDocElab = 1

            oListAvvisi(0) = Session("oMyAvviso")
            oListAvvisiConv = FncDoc.ConvAvvisi(oListAvvisi)
            For x = 0 To oListAvvisiConv.GetUpperBound(0)
                oListAvvisiConv(x).Selezionato = True
            Next
            Try
                '*** 20140509 - TASI ***
                'in ristampa singolo viene sempre creato il documento per l'invio tramite mail
                Dim bSendByMail As Boolean = False
                nReturn = FncDoc.ElaboraDocumenti(ConstSession.IdEnte, oListAvvisiConv(0).IdFlussoRuolo, sTipoRuolo, oListAvvisiConv(0).AnnoRiferimento, nDocDaElab, nDocElab, nTipoElab, sTypeOrd, sNameModello, nMaxDocPerFile, Session("ElaboraBollettini"), oListAvvisiConv, oListDocStampati, bCreaPDF, nDecimal, "TARES", "BOLLETTINISTANDARD", "F24", bSendByMail)
                '*** ***
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDichiarazione.CmdDocumenti_Click.errore: ", Err)
                Response.Redirect("../../PaginaErrore.aspx")
            End Try

            If Not oListDocStampati Is Nothing Then
                Session.Add("ELENCO_DOCUMENTI_STAMPATI", oListDocStampati)
            Else
                nReturn = 0
            End If
            If nReturn = 0 Then
                RegisterScript("GestAlert('a', 'danger', '', '', 'Si e\' verificato un\'errore in stampa!');", Me.GetType)
            Else
                If New ClsGestRuolo().SetDateRuolo(oListAvvisiConv(0).IdFlussoRuolo, 5, "I") = 0 Then
                    Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDichiarazione.CmdDocumenti_Click.errore in SetDateRuoliGenerati")
                End If
                sScript = "document.getElementById('DivAttesa').style.display = 'none';"
                sScript += "document.getElementById('divStampa').style.display = '';document.getElementById('divAvviso').style.display = 'none'; "
                sScript += "document.getElementById('loadStampa').src = '../Avvisi/Documenti/ViewDocElaborati.aspx?Provenienza=A&IdFlussoRuolo=" & oListAvvisiConv(0).IdFlussoRuolo & "&IdContribuente=" & oListAvvisiConv(0).IdContribuente & "&Provenienza=1&ProvenienzaForm=SGRAVI';"
                RegisterScript(sScript, Me.GetType)
            End If
            Session("ListCartelle") = Nothing
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDichiarazione.CmdDocumenti_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Pulsante per la visualizzazione degli avvisi emessi
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdDovuto_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdDovuto.Click
        Dim sScript As String
        Dim mySearchForAvvisi As New ArrayList
        Dim myItem As New ObjTestata

        Try
            If Not Session("oTestata") Is Nothing Then
                myItem = Session("oTestata")
                mySearchForAvvisi.Add("")
                mySearchForAvvisi.Add("")
                mySearchForAvvisi.Add(myItem.oAnagrafe.Cognome)
                mySearchForAvvisi.Add(myItem.oAnagrafe.Nome)
                If myItem.oAnagrafe.PartitaIva <> "" Then
                    mySearchForAvvisi.Add(myItem.oAnagrafe.PartitaIva)
                Else
                    mySearchForAvvisi.Add(myItem.oAnagrafe.CodiceFiscale)
                End If
                mySearchForAvvisi.Add("")
                mySearchForAvvisi.Add("")
                Session("mySearchForAvvisi") = mySearchForAvvisi
                sScript = "parent.Comandi.location.href='../Avvisi/Gestione/ComandiRicAvvisi.aspx';parent.Visualizza.location.href='../Avvisi/Gestione/RicAvvisi.aspx?';"
            Else
                sScript = "GestAlert('a', 'warning', '', '', 'Funzione non abilitata su nuova dichiarazione!');"
            End If
            RegisterScript(sScript, Me.GetType)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDichiarazione.CmdDovuto_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="bTypeAbilita"></param>
    ''' <param name="IsSoloContrib"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Private Sub Abilita(ByVal bTypeAbilita As Boolean, ByVal IsSoloContrib As Integer)
        Dim sScript As String = ""
        Dim oListCmd() As Object
        Dim x As Integer

        Try
            If TxtIdDich.Text <= 0 Then
                sScript += "$('#Calcolo', parent.frames['Comandi'].document).addClass('DisableBtn');"
            Else
                sScript += "$('#Calcolo', parent.frames['Comandi'].document).removeClass('DisableBtn');"
            End If
            If IsSoloContrib <> 1 Then
                TxtDataDichiarazione.Enabled = bTypeAbilita : TxtNDichiarazione.Enabled = bTypeAbilita : TxtDataProtocollo.Enabled = bTypeAbilita : TxtNProtocollo.Enabled = bTypeAbilita : TxtDataCessazione.Enabled = bTypeAbilita
                LnkNewTessera.Enabled = bTypeAbilita : LnkNewUI.Enabled = bTypeAbilita
                TxtNoteDich.Enabled = bTypeAbilita
                DdlTipoDich.Enabled = bTypeAbilita
                'se passo da modifica a variazione devo disabilitare il pulsante di modifica/cancellazione e abilitare il pulsante di salvataggio
                If bTypeAbilita = True Then
                    ReDim Preserve oListCmd(1)
                    oListCmd(0) = "Modifica"
                    oListCmd(1) = "Delete"
                    For x = 0 To oListCmd.Length - 1
                        sScript += "$('#" + oListCmd(x).ToString() + "', parent.frames['Comandi'].document).addClass('DisableBtn');"
                    Next
                    ReDim Preserve oListCmd(2)
                    oListCmd(2) = "Salva"
                    For x = 2 To oListCmd.Length - 1
                        sScript += "$('#" + oListCmd(x).ToString() + "', parent.frames['Comandi'].document).removeClass('DisableBtn');"
                    Next
                    RegisterScript(sScript, Me.GetType)
                End If
            Else
                ReDim Preserve oListCmd(1)
                oListCmd(0) = "Modifica"
                oListCmd(1) = "Delete"
                For x = 0 To oListCmd.Length - 1
                    sScript += "$('#" + oListCmd(x).ToString() + "', parent.frames['Comandi'].document).addClass('DisableBtn');"
                Next
                ReDim Preserve oListCmd(2)
                oListCmd(2) = "Salva"
                For x = 2 To oListCmd.Length - 1
                    sScript += "$('#" + oListCmd(x).ToString() + "', parent.frames['Comandi'].document).removeClass('DisableBtn');"
                Next
                RegisterScript(sScript, Me.GetType)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDichiarazione.Abilita.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sTipoPopUP"></param>
    ''' <param name="nIdTessera"></param>
    ''' <param name="nIdUI"></param>
    ''' <param name="nRow"></param>
    ''' <param name="sAzione"></param>
    Private Sub ShowPopUp(ByVal sTipoPopUP As String, ByVal nIdTessera As Integer, ByVal nIdUI As Integer, nRow As Integer, ByVal sAzione As String)
        Try
            Dim oMyTestata As New ObjTestata

            'carico i dati dal form
            oMyTestata.Id = TxtIdDich.Text
            oMyTestata.IdTestata = TxtIdDich.Text
            oMyTestata.sEnte = ConstSession.IdEnte
            oMyTestata.IdContribuente = CInt(hdIdContribuente.Value)
            If TxtDataDichiarazione.Text.Trim <> "" Then
                oMyTestata.tDataDichiarazione = TxtDataDichiarazione.Text.Trim
            End If
            oMyTestata.sNDichiarazione = TxtNDichiarazione.Text.Trim
            If TxtDataProtocollo.Text.Trim <> "" Then
                oMyTestata.tDataProtocollo = TxtDataProtocollo.Text.Trim
            End If
            oMyTestata.sNProtocollo = TxtNProtocollo.Text.Trim
            oMyTestata.sIdProvenienza = DdlTipoDich.SelectedItem.Value
            oMyTestata.sNoteDichiarazione = TxtNoteDich.Text
            oMyTestata.tDataInserimento = Now
            oMyTestata.tDataCessazione = Nothing
            oMyTestata.sOperatore = ConstSession.UserName
            oMyTestata.oTessere = Session("oListTessere")
            oMyTestata.oTesUI = Session("oListTesUI")
            oMyTestata.oFamiglia = Session("oDatiFamiglia")
            oMyTestata.dvFamigliaResidenti = Session("oDatiFamigliaRes")
            oMyTestata.oAnagrafe = Session("oAnagrafe")
            '*** X UNIONE CON BANCADATI CMGC ***
            oMyTestata.oImmobili = Session("oDettaglioTestata")
            '*** ***
            'memorizzo l'oggetto nella sessione
            Session("oTestata") = oMyTestata

            Dim x As Integer
            If Not IsNothing(oMyTestata.oTessere) Then
                For x = 0 To oMyTestata.oTessere.GetUpperBound(0)
                    If nIdTessera = oMyTestata.oTessere(x).Id Then
                        Session("oTessera") = oMyTestata.oTessere(x)
                        Session("oListImmobili") = oMyTestata.oTessere(x).oImmobili
                    End If
                Next
            Else
                '*** X UNIONE CON BANCADATI CMGC ***
                If ConstSession.IsFromVariabile <> "1" Then
                    Session("oListImmobili") = oMyTestata.oImmobili
                End If
            End If
            'apro il popup passandogli l'indice dell'array da visualizzare
            Dim sScript, sProvenienza As String
            sProvenienza = Costanti.FormProvenienza.Dichiarazione
            If sTipoPopUP = "F" Then
                sScript = "ShowInsertFamiglia('" & TxtIdDich.Text & "','" & sAzione & "')"
                RegisterScript(sScript, Me.GetType)
            ElseIf sTipoPopUP = "T" Then
                sScript = "ShowInsertTessera('" & hdIdContribuente.Value & "','" & TxtIdDich.Text & "','" & nIdTessera & "','" & sAzione & "','" & sProvenienza & "')"
                RegisterScript(sScript, Me.GetType)
            ElseIf sTipoPopUP = "I" Then
                sScript = "ShowInsertUI('" & hdIdContribuente.Value & "','" & TxtIdDich.Text & "','" & nIdTessera & "','" & nIdUI & "','" & sAzione & "','" & sProvenienza & "','" & nRow & "','" & ConstSession.IsFromVariabile & "')"
                RegisterScript(sScript, Me.GetType)
            ElseIf sTipoPopUP = "A" Then
                sScript = "ShowRicUIAnater()"
                RegisterScript(sScript, Me.GetType)
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDichiarazione.ShowPopUp.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub ShowPopUp(ByVal sTipoPopUP As String, ByVal nIdTessera As Integer, ByVal nIdUI As Integer, ByVal sAzione As String)
    '    Try
    '        Dim oMyTestata As New ObjTestata

    '        'carico i dati dal form
    '        oMyTestata.Id = TxtIdDich.Text
    '        oMyTestata.IdTestata = TxtIdDich.Text
    '        oMyTestata.sEnte = ConstSession.IdEnte
    '        oMyTestata.IdContribuente = CInt(hdIdContribuente.Value) 'TxtCodContribuente.Text
    '        If TxtDataDichiarazione.Text.Trim <> "" Then
    '            oMyTestata.tDataDichiarazione = TxtDataDichiarazione.Text.Trim
    '        End If
    '        oMyTestata.sNDichiarazione = TxtNDichiarazione.Text.Trim
    '        If TxtDataProtocollo.Text.Trim <> "" Then
    '            oMyTestata.tDataProtocollo = TxtDataProtocollo.Text.Trim
    '        End If
    '        oMyTestata.sNProtocollo = TxtNProtocollo.Text.Trim
    '        oMyTestata.sIdProvenienza = DdlTipoDich.SelectedItem.Value
    '        oMyTestata.sNoteDichiarazione = TxtNoteDich.Text
    '        oMyTestata.tDataInserimento = Now
    '        oMyTestata.tDataCessazione = Nothing
    '        oMyTestata.sOperatore = ConstSession.UserName
    '        oMyTestata.oTessere = Session("oListTessere")
    '        oMyTestata.oTesUI = Session("oListTesUI")
    '        oMyTestata.oFamiglia = Session("oDatiFamiglia")
    '        oMyTestata.dvFamigliaResidenti = Session("oDatiFamigliaRes")
    '        oMyTestata.oAnagrafe = Session("oAnagrafe")
    '        '*** X UNIONE CON BANCADATI CMGC ***
    '        oMyTestata.oImmobili = Session("oDettaglioTestata")
    '        '*** ***
    '        'memorizzo l'oggetto nella sessione
    '        Session("oTestata") = oMyTestata

    '        Dim x As Integer
    '        If Not IsNothing(oMyTestata.oTessere) Then
    '            For x = 0 To oMyTestata.oTessere.GetUpperBound(0)
    '                If nIdTessera = oMyTestata.oTessere(x).Id Then
    '                    Session("oTessera") = oMyTestata.oTessere(x)
    '                    Session("oListImmobili") = oMyTestata.oTessere(x).oImmobili
    '                End If
    '            Next
    '        Else
    '            '*** X UNIONE CON BANCADATI CMGC ***
    '            If ConstSession.IsFromVariabile <> "1" Then
    '                Session("oListImmobili") = oMyTestata.oImmobili
    '            End If
    '        End If
    '        'apro il popup passandogli l'indice dell'array da visualizzare
    '        Dim sScript, sProvenienza As String
    '        sProvenienza = Costanti.FormProvenienza.Dichiarazione
    '        If sTipoPopUP = "F" Then
    '            sScript = "ShowInsertFamiglia('" & TxtIdDich.Text & "','" & sAzione & "')"
    '            RegisterScript(sScript, Me.GetType)
    '        ElseIf sTipoPopUP = "T" Then
    '            sScript = "ShowInsertTessera('" & hdIdContribuente.Value & "','" & TxtIdDich.Text & "','" & nIdTessera & "','" & sAzione & "','" & sProvenienza & "')"
    '            RegisterScript(sScript, Me.GetType)
    '        ElseIf sTipoPopUP = "I" Then
    '            'sScript = "ShowInsertUI('" & TxtCodContribuente.Text & "','" & nIdTessera & "','" & nIdUI & "','" & sAzione & "','" & sProvenienza & "')"
    '            sScript = "ShowInsertUI('" & hdIdContribuente.Value & "','" & TxtIdDich.Text & "','" & nIdTessera & "','" & nIdUI & "','" & sAzione & "','" & sProvenienza & "','-1','" & ConstSession.IsFromVariabile & "')"
    '            RegisterScript(sScript, Me.GetType)
    '        ElseIf sTipoPopUP = "A" Then
    '            sScript = "ShowRicUIAnater()"
    '            RegisterScript(sScript, Me.GetType)
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDichiarazione.ShowPopUp.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nomeSessione"></param>
    Private Sub writeJavascriptAnagrafica(ByVal nomeSessione As String)
        Dim sScript As String

        sScript = "ApriRicercaAnagrafe('" & nomeSessione & "');" & vbCrLf
        RegisterScript(sScript, Me.GetType)
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub ClearDatiDich()
        'ripulisco tutti i dati di sessioni dati dichiarazione
        Session.Remove("oTestata")
        Session.Remove("oTestataOrg")
        Session.Remove("oAnagrafe")
        Session.Remove("oTesUI")
        Session.Remove("oTessera")
        Session.Remove("oDatiFamiglia")
        Session.Remove("oDatiFamigliaRes")
        Session.Remove("oDatiVani")
        Session.Remove("oDatiRid")
        Session.Remove("oDatiDet")
        Session.Remove("oRiepilogo")
        Session.Remove("oArticoliSingoloContribuente")
        Session.Remove("oListImmobili")
        Session.Remove("oDatiRidTessera")
        Session.Remove("oDatiDetTessera")
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oMyTestata"></param>
    Private Sub LoadAnag(ByVal oMyTestata As ObjTestata)
        Try
            If ConstSession.HasPlainAnag Then
                ifrmAnag.Attributes.Add("src", "../../Generali/asp/VisualAnag.aspx?IdContribuente=" & oMyTestata.IdContribuente & "&Azione=" & Utility.Costanti.AZIONE_NEW)
                ifrmAnagPunt.Attributes.Add("src", "../../Generali/asp/VisualAnag.aspx?IdContribuente=" & oMyTestata.IdContribuente & "&Azione=" & Utility.Costanti.AZIONE_SELEZIONE)
                hdIdContribuente.Value = oMyTestata.IdContribuente
            Else
                hdIdContribuente.Value = oMyTestata.IdContribuente
                If Not oMyTestata.oAnagrafe Is Nothing Then
                    TxtIdDataAnagrafica.Text = oMyTestata.oAnagrafe.ID_DATA_ANAGRAFICA
                    TxtCodFiscale.Text = oMyTestata.oAnagrafe.CodiceFiscale
                    TxtPIva.Text = oMyTestata.oAnagrafe.PartitaIva
                    TxtCognome.Text = oMyTestata.oAnagrafe.Cognome
                    TxtNome.Text = oMyTestata.oAnagrafe.Nome
                    Select Case oMyTestata.oAnagrafe.Sesso
                        Case "F"
                            F.Checked = True
                        Case "G"
                            G.Checked = True
                        Case "M"
                            M.Checked = True
                    End Select
                    TxtDataNascita.Text = oMyTestata.oAnagrafe.DataNascita
                    TxtLuogoNascita.Text = oMyTestata.oAnagrafe.ComuneNascita
                    TxtResVia.Text = oMyTestata.oAnagrafe.ViaResidenza
                    TxtResCivico.Text = oMyTestata.oAnagrafe.CivicoResidenza
                    TxtResEsponente.Text = oMyTestata.oAnagrafe.EsponenteCivicoResidenza
                    TxtResInterno.Text = oMyTestata.oAnagrafe.InternoCivicoResidenza
                    TxtResScala.Text = oMyTestata.oAnagrafe.ScalaCivicoResidenza
                    TxtResCAP.Text = oMyTestata.oAnagrafe.CapResidenza
                    TxtResComune.Text = oMyTestata.oAnagrafe.ComuneResidenza
                    TxtResPv.Text = oMyTestata.oAnagrafe.ProvinciaResidenza
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestDichiarazione.LoadAnag.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    Private Sub LoadAvviso(myAvviso As ObjAvviso)
        Try
            LblDatiAvviso.Text = "Avviso per l'anno " & myAvviso.sAnnoRiferimento & " N. " & myAvviso.sCodiceCartella
            If myAvviso.tDataEmissione <> Date.MinValue Then
                LblDatiAvviso.Text += " del " & myAvviso.tDataEmissione.ToShortDateString
            End If
            LblEmesso.Text = FormatNumber(myAvviso.impCarico, 2).ToString & " €" : Label38.Style.Add("display", "")
            'controllo se devo caricare la griglia delle tessere
            Log.Debug("GestAvvisi::Page_Load::devo caricare le tessere")
            If Not myAvviso.oTessere Is Nothing Then
                GrdTessere.DataSource = myAvviso.oTessere
                GrdTessere.DataBind()
                GrdTessere.SelectedIndex = -1
                LblResultTessere.Style.Add("display", "none")
            Else
                LblResultTessere.Style.Add("display", "")
            End If
            'controllo se devo caricare la griglia degli articoli
            Log.Debug("GestAvvisi::Page_Load::devo caricare gli articoli")
            If Not myAvviso.oArticoli Is Nothing Then
                GrdArticoli.DataSource = myAvviso.oArticoli
                GrdArticoli.DataBind()
                GrdArticoli.SelectedIndex = -1
                LblResultArticoli.Style.Add("display", "none")
            Else
                LblResultArticoli.Style.Add("display", "")
            End If
            'controllo se devo caricare la griglia del dettaglio voci
            Log.Debug("GestAvvisi::Page_Load::devo caricare il dettaglio voci")
            If Not myAvviso.oDetVoci Is Nothing Then
                GrdDettaglioVoci.DataSource = myAvviso.oDetVoci
                GrdDettaglioVoci.DataBind()
                GrdDettaglioVoci.SelectedIndex = -1
                LblResultDettVoci.Style.Add("display", "none")
            Else
                LblResultDettVoci.Style.Add("display", "")
            End If
            'controllo se devo caricare la griglia delle rate
            Log.Debug("GestAvvisi::Page_Load::devo caricare le rate")
            If Not myAvviso.oRate Is Nothing Then
                GrdRate.DataSource = myAvviso.oRate
                GrdRate.DataBind()
                GrdRate.SelectedIndex = -1
                LblResultRate.Style.Add("display", "none")
            Else
                LblResultRate.Style.Add("display", "")
            End If
        Catch ex As Exception
            Throw New Exception("OPENgovTIA.GestDichiarazione.LoadAvviso.errore: " + ex.Message)
        End Try
    End Sub
End Class
