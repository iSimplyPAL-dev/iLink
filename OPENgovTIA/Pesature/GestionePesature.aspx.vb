Imports log4net
Imports AnagInterface
Imports OPENUtility
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti

''' <summary>
''' Pagina per la visualizzazione/gestione della pesatura.
''' Contiene i parametri di dettaglio e le funzioni della comandiera.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class GestionePesature
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(GestionePesature))

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
        If ConstSession.IsFromVariabile("1") = "1" Then 'sempre IsFromVariabile
        End If
        lblTitolo.Text = ConstSession.DescrizioneEnte
        If ConstSession.IsFromTARES = "1" Then
            info.InnerText = "TARI "
        Else
            info.InnerText = "TARSU "
        End If
        info.InnerText += "Variabile "
        info.InnerText += " - Conferimenti - Gestione"
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
        'Put user code to initialize the page here
        Try
            Dim DatiContribuente As New DettaglioAnagrafica
            Dim oMyTesseraPesature As New ObjTesseraPesature
            Dim MyFunction As New ObjTesseraPesature

            If Page.IsPostBack = False Then
                '*** 201511 - gestione tipo tessera ***
                LoadPageCombos()
                '*** ***
                'controllo se sono in visualizzazione di una posizione
                If Not Request.Item("IdTessera") Is Nothing Then
                    oMyTesseraPesature = MyFunction.GetTesseraPesature(Request.Item("IdTessera"), ConstSession.IdEnte, ConstSession.StringConnection)
                    If Not oMyTesseraPesature Is Nothing Then
                        'carico l'anagrafica
                        If ConstSession.HasPlainAnag Then
                            ifrmAnag.Attributes.Add("src", "../../Generali/asp/VisualAnag.aspx?IdContribuente=" & oMyTesseraPesature.oTessera.IdContribuente & "&Azione=" & Utility.Costanti.AZIONE_NEW)
                            hdIdContribuente.Value = oMyTesseraPesature.oTessera.IdContribuente
                        Else
                            hdIdContribuente.Value = oMyTesseraPesature.oTessera.IdContribuente
                            Dim oMyAnagrafica As New Anagrafica.DLL.GestioneAnagrafica()
                            '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
                            DatiContribuente = oMyAnagrafica.GetAnagrafica(oMyTesseraPesature.oTessera.IdContribuente, Utility.Costanti.INIT_VALUE_NUMBER, String.Empty, ConstSession.DBType, ConstSession.StringConnectionAnagrafica, False)
                            TxtIdDataAnagrafica.Text = DatiContribuente.ID_DATA_ANAGRAFICA
                            TxtCodFiscale.Text = DatiContribuente.CodiceFiscale
                            TxtPIva.Text = DatiContribuente.PartitaIva
                            TxtCognome.Text = DatiContribuente.Cognome
                            TxtNome.Text = DatiContribuente.Nome
                            Select Case DatiContribuente.Sesso
                                Case "F"
                                    F.Checked = True
                                Case "G"
                                    G.Checked = True
                                Case "M"
                                    M.Checked = True
                            End Select
                            If DatiContribuente.DataNascita <> "" Then
                                TxtDataNascita.Text = DatiContribuente.DataNascita
                            End If
                            TxtLuogoNascita.Text = DatiContribuente.ComuneNascita
                            TxtResVia.Text = DatiContribuente.ViaResidenza
                            TxtResCivico.Text = DatiContribuente.CivicoResidenza
                            TxtResEsponente.Text = DatiContribuente.EsponenteCivicoResidenza
                            TxtResInterno.Text = DatiContribuente.InternoCivicoResidenza
                            TxtResScala.Text = DatiContribuente.ScalaCivicoResidenza
                            TxtResCAP.Text = DatiContribuente.CapResidenza
                            TxtResComune.Text = DatiContribuente.ComuneResidenza
                            TxtResPv.Text = DatiContribuente.ProvinciaResidenza
                        End If
                    End If
                    'carico la videata
                    TxtCodUtente.Text = oMyTesseraPesature.oTessera.sCodUtente
                    TxtNTessera.Text = oMyTesseraPesature.oTessera.sNumeroTessera
                    TxtCodice.Text = oMyTesseraPesature.oTessera.sCodInterno
                    TxtDataRilascio.Text = oMyTesseraPesature.oTessera.tDataRilascio
                    If oMyTesseraPesature.oTessera.tDataCessazione <> Date.MinValue Then
                        TxtDataCessazione.Text = oMyTesseraPesature.oTessera.tDataCessazione
                    End If
                    TxtNote.Text = oMyTesseraPesature.oTessera.sNote
                    hdIdTessera.Value = oMyTesseraPesature.oTessera.Id
                    '*** 201511 - gestione tipo tessera ***
                    DdlTipoTessera.SelectedValue = oMyTesseraPesature.oTessera.IdTipoTessera
                    '*** ***
                    'carico la griglia
                    LoadConferimenti(oMyTesseraPesature)
                    'If Not oMyTesseraPesature.oPesature Is Nothing Then
                    '    Array.Sort(oMyTesseraPesature.oPesature, New Utility.Comparatore(New String() {"tDataOraConferimento"}, New Boolean() {Utility.TipoOrdinamento.Decrescente}))
                    '    GrdPesature.Style.Add("display", "")
                    '    GrdPesature.DataSource = oMyTesseraPesature.oPesature
                    '    GrdPesature.start_index = GrdPesature.CurrentPageIndex
                    '    GrdPesature.DataBind()
                    '    LblResult.Style.Add("display", "none")
                    '    'carico i totalizzatori
                    '    GetTotalizzatori(oMyTesseraPesature.oPesature, nKG, nVolume, nConferimenti)
                    '    LblTotKG.Text = "Totale KG: " & FormatNumber(nKG, 2)
                    '    LblTotVolume.Text = "Totale Volume: " & FormatNumber(nVolume, 2)
                    '    LblTotConferimenti.Text = "Totale Conferimenti: " & FormatNumber(nConferimenti, 0)
                    '    'carico i mq di dichiarazioni
                    '    'nMq = MyFunction.GetMqTARSU(oMyTesseraPesature.oTessera.IdEnte, oMyTesseraPesature.oTessera.IdContribuente)
                    '    nMq = MyFunction.GetMqTARSU(oMyTesseraPesature.oTessera)
                    '    If nMq <> -1 Then
                    '        LblTotMq.Text = "MQ Tassabili: " & FormatNumber(nMq, 2)
                    '    End If
                    '    Session("oTesseraPesature") = oMyTesseraPesature
                    'Else
                    '    GrdPesature.Style.Add("display", "none")
                    '    LblResult.Style.Add("display", "")
                    '    LblTotKG.Style.Add("display", "")
                    '    LblTotConferimenti.Style.Add("display", "")
                    'End If
                End If
                Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, "Pesature", "Gestione", Utility.Costanti.AZIONE_LETTURA.ToString, ConstSession.CodTributo, ConstSession.IdEnte, Request.Item("IdTessera").ToString)
            End If
            '*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI & DIVERSA GESTIONE QUOTA VARIABILE***
            Dim sScript As String = ""
            If ConstSession.HasPlainAnag Then
                sScript += "document.getElementById('TRSpecAnag').style.display='none';"
            Else
                sScript += "document.getElementById('TRPlainAnag').style.display='none';"
            End If
            RegisterScript(sScript, Me.GetType)
            '*** ***
            sScript = "document.getElementById('GestConferimento').style.display='none';"
            RegisterScript(sScript, Me.GetType)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.GestionePesature.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            'If Not WFSessione Is Nothing Then
            '    WFSessione.Kill()
            'End If
        End Try
    End Sub

    Private Sub LnkNewConferimento_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles LnkNewConferimento.Click
        Try
            txtData.Text = Now.ToShortDateString
            txtOra.Text = Now.ToShortTimeString.Replace(".", ":") '.Hour.ToString & ":" & Now.Minute.ToString & ":" & Now.Second.ToString & ":"
            txtKG.Text = 0
            txtVolume.Text = 0
            hdIdConferimento.Value = -1
            hdIdPesatura.Value = -1
            hdIdFlusso.Value = -1

            Dim sScript As String = "document.getElementById('GestConferimento').style.display='';"
            RegisterScript(sScript, Me.GetType)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestionePesature.LnkNewConferimento_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

#Region "Griglie"
    '*** 201712 - gestione tipo conferimento ***
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim IDRow As String = e.CommandArgument.ToString()
            If e.CommandName = "RowOpen" Then
                For Each myRow As GridViewRow In GrdPesature.Rows
                    If IDRow = CType(myRow.FindControl("hfid"), HiddenField).Value Then
                        txtData.Text = CDate(myRow.Cells(0).Text).ToShortDateString
                        txtOra.Text = CDate(myRow.Cells(0).Text).ToShortTimeString.Replace(".", ":")
                        For Each myItem As ListItem In ddlTipoConferimento.Items
                            If myRow.Cells(1).Text = myItem.Text Then
                                ddlTipoConferimento.SelectedValue = myItem.Value
                                Exit For
                            End If
                        Next
                        txtKG.Text = CDbl(myRow.Cells(2).Text)
                        txtVolume.Text = CDbl(myRow.Cells(3).Text)
                        hdIdConferimento.Value = CType(myRow.FindControl("hfid"), HiddenField).Value
                        hdIdPesatura.Value = CType(myRow.FindControl("hfidpesatura"), HiddenField).Value
                        hdIdFlusso.Value = CType(myRow.FindControl("hfidflusso"), HiddenField).Value

                        Dim sScript As String = "document.getElementById('GestConferimento').style.display='';"
                        RegisterScript(sScript, Me.GetType)
                    End If
                Next
            ElseIf e.CommandName = "RowDelete" Then
                Dim FncPesature As New GestPesatura
                Dim oMyPesatura As New ObjPesatura
                Dim myItem As New ObjTesseraPesature

                Try
                    hdIdConferimento.Value = IDRow
                    If CInt(hdIdConferimento.Value) > 0 Then
                        myItem = CType(Session("oTesseraPesature"), ObjTesseraPesature)
                        For Each oMyPesatura In myItem.oPesature
                            If oMyPesatura.Id = CInt(hdIdConferimento.Value) Then
                                oMyPesatura.tDataVariazione = Now
                                oMyPesatura.tDataCessazione = Now
                                If FncPesature.SetPesatura(ConstSession.StringConnection, oMyPesatura, Utility.Costanti.AZIONE_NEW) <= 0 Then
                                    Response.Redirect("../../PaginaErrore.aspx")
                                    Exit Sub
                                End If
                            End If
                        Next
                    End If
                    'ricarico i dati a video
                    Dim FncTesVSPes As New ObjTesseraPesature
                    LoadConferimenti(FncTesVSPes.GetTesseraPesature(Request.Item("IdTessera"), ConstSession.IdEnte, ConstSession.StringConnection))
                    RegisterScript("GestAlert('a', 'success', '', '', 'Eliminazione effettuata con successo!');", Me.GetType)
                Catch Err As Exception
                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestionePesature.GrdRowCommand.errore: ", Err)
                    Response.Redirect("../../PaginaErrore.aspx")
                End Try
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestionePesature.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** ***
    ''' <summary>
    ''' Gestione del cambio pagina della griglia.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdPageIndexChanging(ByVal sender As Object, e As GridViewPageEventArgs)
        LoadSearch(e.NewPageIndex)
    End Sub
    'Private Sub GrdPesature_UpdateCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GrdPesature.UpdateCommand
    '    Try
    '        txtData.Text = CDate(e.Item.Cells(0).Text).ToShortDateString
    '        txtOra.Text = CDate(e.Item.Cells(0).Text).ToShortTimeString.Replace(".", ":")
    '        txtKG.Text = CDbl(e.Item.Cells(1).Text)
    '        txtVolume.Text = CDbl(e.Item.Cells(2).Text)
    '        hdIdConferimento.Value = e.Item.Cells(5).Text
    '        hdIdPesatura.Value = e.Item.Cells(6).Text
    '        hdIdFlusso.Value = e.Item.Cells(7).Text
    '        
    '        Str += "document.getElementById('GestConferimento').style.display='';"
    '        Str += "</script>"
    '        RegisterScript( sScript,Me.GetType)
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestionePesature.GrdPesature_UpdateCommand.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Private Sub GrdPesature_EditCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GrdPesature.EditCommand
    '    Dim FncPesature As New GestPesatura
    '    Dim oMyPesatura As New ObjPesatura
    '    Dim myItem As New ObjTesseraPesature

    '    Try
    '        hdIdConferimento.Value = e.Item.Cells(5).Text
    '        If CInt(hdIdConferimento.Value) > 0 Then
    '            myItem = CType(Session("oTesseraPesature"), ObjTesseraPesature)
    '            For Each oMyPesatura In myItem.oPesature
    '                If oMyPesatura.Id = CInt(hdIdConferimento.Value) Then
    '                    oMyPesatura.tDataVariazione = Now
    '                    oMyPesatura.tDataCessazione = Now
    '                    If FncPesature.SetPesatura(ConstSession.StringConnection, oMyPesatura, Utility.Costanti.AZIONE_NEW) <= 0 Then
    '                        Response.Redirect("../../PaginaErrore.aspx")
    '                        Exit Sub
    '                    End If
    '                End If
    '            Next
    '        End If
    '        'ricarico i dati a video
    '        Dim FncTesVSPes As New ObjTesseraPesature
    '        LoadConferimenti(FncTesVSPes.GetTesseraPesature(Request.Item("IdTessera"), ConstSession.IdEnte, ConstSession.StringConnection))
    '        RegisterScript(Me.GetType(), "newup", "<script language='javascript'>alert('Eliminazione effettuata con successo!');</script>")
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestionePesature.GrdPesature_EditCommand.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdPesature.DataSource = CType(Session("oTesseraPesature"), ObjTesseraPesature).oPesature
            If page.HasValue Then
                GrdPesature.PageIndex = page.Value
            End If
            GrdPesature.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestionePesature.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub

    '*** 201712 - gestione tipo conferimento ***
    Private Sub CmdSaveConferimento_Click(sender As Object, e As System.EventArgs) Handles CmdSaveConferimento.Click
        Dim FncPesature As New GestPesatura
        Dim oMyPesatura As New ObjPesatura
        Dim myItem As New ObjTesseraPesature
        Dim myDate As DateTime
        Dim sScript As String = ""

        Try
            'controllo la coerenza della data
            If TxtDataCessazione.Text <> "" Then
                If CDate(txtData.Text) > CDate(TxtDataCessazione.Text) Then
                    sScript = "GestAlert('a', 'warning', '', '', 'La Data del conferimento deve essere compresa tra Data Rilascio e Data Cessazione!');"
                    RegisterScript(sScript, Me.GetType)
                    Exit Sub
                End If
            End If
            If CDate(txtData.Text) < CDate(TxtDataRilascio.Text) Then
                sScript = "GestAlert('a', 'warning', '', '', 'La Data del conferimento deve essere compresa tra Data Rilascio e Data Cessazione!');"
                RegisterScript(sScript, Me.GetType)
                Exit Sub
            End If
            Try
                myDate = CDate(txtData.Text & " " & txtOra.Text)
            Catch ex As Exception
                sScript = "GestAlert('a', 'warning', '', '', 'La Data e Ora in formato non valido!');"
                RegisterScript(sScript, Me.GetType)
                Exit Sub
            End Try
            If ddlTipoConferimento.SelectedValue = "-1" Then
                sScript = "GestAlert('a', 'warning', '', '', 'Selezionare il Tipo di Conferimento!');"
                RegisterScript(sScript, Me.GetType)
                Exit Sub
            End If
            'se sono in modifica devo prima storicizzare
            If CInt(hdIdConferimento.Value) > 0 Then
                myItem = CType(Session("oTesseraPesature"), ObjTesseraPesature)
                For Each oMyPesatura In myItem.oPesature
                    If oMyPesatura.Id = CInt(hdIdConferimento.Value) Then
                        For Each myDDL As ListItem In ddlTipoConferimento.Items
                            If oMyPesatura.sTipoConferimento = myDDL.Text Then
                                oMyPesatura.sTipoConferimento = myDDL.Value
                            End If
                        Next
                        oMyPesatura.tDataVariazione = Now
                        If FncPesature.SetPesatura(ConstSession.StringConnection, oMyPesatura, Utility.Costanti.AZIONE_NEW) <= 0 Then
                            Response.Redirect("../../PaginaErrore.aspx")
                            Exit Sub
                        End If
                    End If
                Next
            End If
            'salvo i dati a video
            oMyPesatura = New ObjPesatura
            oMyPesatura.Id = -1
            oMyPesatura.IdEnte = ConstSession.IdEnte
            oMyPesatura.IdFlusso = CInt(hdIdFlusso.Value)
            oMyPesatura.IdPesatura = CInt(hdIdPesatura.Value)
            oMyPesatura.IdTessera = CInt(hdIdTessera.Value)
            oMyPesatura.nLitri = CDbl(txtKG.Text)
            oMyPesatura.nVolume = CDbl(txtVolume.Text)
            oMyPesatura.sCodInterno = TxtCodice.Text
            oMyPesatura.sCodUtente = TxtCodUtente.Text
            oMyPesatura.sComune = ConstSession.DescrizioneEnte
            oMyPesatura.sNumeroTessera = TxtNTessera.Text
            oMyPesatura.sOperatore = ConstSession.UserName
            oMyPesatura.sPuntoConferimento = ""
            oMyPesatura.sTipoConferimento = ddlTipoConferimento.SelectedValue
            oMyPesatura.tDataCessazione = DateTime.MaxValue
            oMyPesatura.tDataInserimento = Now
            oMyPesatura.tDataOraConferimento = myDate
            oMyPesatura.tDataVariazione = DateTime.MaxValue

            If FncPesature.SetPesatura(ConstSession.StringConnection, oMyPesatura, Utility.Costanti.AZIONE_NEW) <= 0 Then
                Response.Redirect("../../PaginaErrore.aspx")
            Else
                Dim FncTesVSPes As New ObjTesseraPesature
                LoadConferimenti(FncTesVSPes.GetTesseraPesature(Request.Item("IdTessera"), ConstSession.IdEnte, ConstSession.StringConnection))
                sScript = "GestAlert('a', 'success', '', '', 'Inserimento effettuato con successo!');"
                RegisterScript(sScript, Me.GetType)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestionePesature.CmdSaveSpedizione_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** ***
    Private Sub CmdClearDati_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClearDati.Click
        Try
            'aggiorno la pagina chiamante
            Dim sScript As String = ""

            'se provengo da situazione contribuente richiamo le relative pagine
            If Request.Item("Provenienza") = Costanti.FormProvenienza.Tessera Then
                sScript += "parent.Visualizza.location.href='../Dichiarazioni/GestTessere.aspx?IdContribuente=" & hdIdContribuente.Value & "&IdTestata=" & Request.Item("IdTestata") & "&IdUniqueTessera=" & Request.Item("IdTessera") & "&AzioneProv=" & Request.Item("AzioneProv") & "';"
                sScript += "parent.Comandi.location.href='../Dichiarazioni/ComandiGestTessere.aspx?AzioneProv=" & Request.Item("AzioneProv") & "';"
            Else
                sScript += "parent.Visualizza.location.href = 'RicercaPesature.aspx';"
                sScript += "parent.Comandi.location.href = 'ComandiRicPesature.aspx';"
            End If
            Log.Debug("ritorno su::" & sScript)
            RegisterScript(sScript, Me.GetType)
            'ripulisco tutti i dati di sessioni dati immobile
            Session("oTesseraPesature") = Nothing
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestionePesature.CmdClearDati_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub GetTotalizzatori(ByVal oListPesature() As ObjPesatura, ByRef nTotKG As Double, ByRef nTotVolume As Double, ByRef nTotConferimenti As Integer)
        Dim x As Integer
        Try
            For x = 0 To oListPesature.GetUpperBound(0)
                nTotKG += oListPesature(x).nLitri
                nTotVolume += oListPesature(x).nVolume
            Next
            nTotConferimenti = oListPesature.Length
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestionePesature.GetTotalizzatori.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    Private Sub LoadConferimenti(oMyTesseraPesature As ObjTesseraPesature)
        Dim FncTesVSPes As New ObjTesseraPesature
        Dim nKG As Double = 0
        Dim nVolume As Double = 0
        Dim nConferimenti As Integer = 0
        Dim nMq As Double = -1

        Try
            If Not oMyTesseraPesature Is Nothing Then
                If Not oMyTesseraPesature.oPesature Is Nothing Then
                    Array.Sort(oMyTesseraPesature.oPesature, New Utility.Comparatore(New String() {"tDataOraConferimento"}, New Boolean() {Utility.TipoOrdinamento.Decrescente}))
                    GrdPesature.Style.Add("display", "")
                    GrdPesature.DataSource = oMyTesseraPesature.oPesature
                    GrdPesature.DataBind()
                    LblResult.Style.Add("display", "none")
                    'carico i totalizzatori
                    GetTotalizzatori(oMyTesseraPesature.oPesature, nKG, nVolume, nConferimenti)
                    LblTotKG.Text = "Totale KG: " & FormatNumber(nKG, 2)
                    LblTotVolume.Text = "Totale Volume/KG: " & FormatNumber(nVolume, 2)
                    LblTotConferimenti.Text = "Totale Conferimenti: " & FormatNumber(nConferimenti, 0)
                    'carico i mq di dichiarazioni
                    nMq = FncTesVSPes.GetMqTARSU(oMyTesseraPesature.oTessera)
                    If nMq <> -1 Then
                        LblTotMq.Text = "MQ Tassabili: " & FormatNumber(nMq, 2)
                    End If
                    Session("oTesseraPesature") = oMyTesseraPesature
                Else
                    GrdPesature.Style.Add("display", "none")
                    LblResult.Style.Add("display", "")
                    LblTotKG.Style.Add("display", "")
                    LblTotConferimenti.Style.Add("display", "")
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestionePesature.LoadConferimenti.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** 201511 - gestione tipo tessera ***
    Private Sub LoadPageCombos()
        Dim sSQL As String
        Dim oLoadCombo As New generalClass.generalFunction

        Try
            sSQL = "SELECT *"
            sSQL += " FROM V_TIPOTESSERA"
            sSQL += " ORDER BY DESCRIZIONE"
            oLoadCombo.LoadComboGenerale(DdlTipoTessera, sSQL, ConstSession.StringConnection, False, Costanti.TipoDefaultCmb.NUMERO)
            sSQL = "SELECT *"
            sSQL += " FROM V_TIPOCONFERIMENTO"
            sSQL += " ORDER BY DESCRIZIONE"
            oLoadCombo.LoadComboGenerale(ddlTipoConferimento, sSQL, ConstSession.StringConnection, False, Costanti.TipoDefaultCmb.STRINGA)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestionePesature.LoadSearch.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** ***
    'Public Function FormattaDataGrd(ByVal tDataGrd As DateTime) As String
    '    If tDataGrd = Date.MinValue Then
    '        Return ""
    '    Else
    '        Return tDataGrd.ToShortDateString
    '    End If
    'End Function
End Class
