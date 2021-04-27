Imports log4net
''' <summary>
''' Pagina per il controllo sui riferimenti catastali.
''' I dati possono essere filtrati per:
'''     Anno
'''     Periodo di validità (Dal-Al)
'''     Operatore che ha effettuato la modifica
'''     % di tolleranza
''' I controlli attivati sono:
'''     Riferimenti chiusi e non riaperti
'''     Rif.Catastali non in Dichiarazioni
'''     Sup.Dichiarata uguale a Catastale
'''     Riferimenti mancanti
'''     Rif.Dichiarati non a castato
'''     Sup.Dichiarata diversa da Catastale
'''     Riferimenti doppi per lo stesso periodo	
'''     Sup.Catastale maggiore di Dichiarata
'''     Sup.Catastale minore di Dichiarata
'''     Dichiarazioni modificate
'''     Dichiarazioni inserite	
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Public Class CheckRifCatastali
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(CheckRifCatastali))
    Protected FncGrd As New Formatta.FunctionGrd
    ''' <summary>
    ''' Al caricamento della pagina carica la combo con i possibili anni e i possibili operatori da filtrare.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim nOptSel As Integer = 0
        Try
            If Not Session("OptSelCheckRifCatastali") Is Nothing Then
                nOptSel = CInt(Session("OptSelCheckRifCatastali"))
            End If
            If Page.IsPostBack = False Then
                lblTitolo.Text = ConstSession.DescrizioneEnte
                If ConstSession.IsFromTARES = "1" Then
                    info.InnerText = "TARI "
                Else
                    info.InnerText = "TARSU "
                End If
                If ConstSession.IsFromVariabile(Request.Item("IsFromVariabile")) = "1" Then
                    info.InnerText += "Variabile "
                End If
                info.InnerText += " - Analisi - Controlli Riferimenti Catastali"

                'nascondo le opzioni non TARSU
                If Session("IsFromTARES") <> "1" Then
                    OptCatEqualDic.Style.Add("display", "none")
                    OptCatDifferentDic.Style.Add("display", "none")
                    OptCatGreaterDic.Style.Add("display", "none")
                    OptCatLowerDic.Style.Add("display", "none")
                    OptCatNoDic.Style.Add("display", "none")
                    OptDicNoCat.Style.Add("display", "none")
                Else
                    OptRifAccertati.Style.Add("display", "none")
                End If
                'popolo la combo per l'anno
                Dim myListItem1 As New ListItem
                Dim x As Integer
                DdlAnno.Items.Clear()
                For x = Now.Year To Now.Year - 6 Step -1
                    myListItem1 = New ListItem
                    myListItem1.Text = x
                    myListItem1.Value = x
                    DdlAnno.Items.Add(myListItem1)
                Next
                If Not IsNothing(Session("AnnoCheckRifCatastali")) Then
                    DdlAnno.SelectedValue = Session("AnnoCheckRifCatastali")
                End If
                If Not IsNothing(Session("TolleranzaCheckRifCatastali")) Then
                    TxtTolleranza.Text = Session("TolleranzaCheckRifCatastali")
                End If
                If Not IsNothing(Session("OperatoreCheckRifCatastali")) Then
                    DdlOperatore.SelectedValue = Session("OperatoreCheckRifCatastali")
                End If
                If Not IsNothing(Session("DalCheckRifCatastali")) Then
                    TxtDal.Text = Session("DalCheckRifCatastali")
                End If
                If Not IsNothing(Session("AlCheckRifCatastali")) Then
                    TxtAl.Text = Session("AlCheckRifCatastali")
                End If
                Select Case nOptSel
                    Case 1
                        OptRifChiusi.Checked = True
                    Case 2
                        OptRifDoppi.Checked = True
                    Case 3
                        OptRifMancanti.Checked = True
                    Case 4
                        OptRifAccertati.Checked = True
                    Case 5
                        OptCatEqualDic.Checked = True
                    Case 6
                        OptCatDifferentDic.Checked = True
                    Case 7
                        OptCatGreaterDic.Checked = True
                    Case 8
                        OptCatLowerDic.Checked = True
                    Case 9
                        OptCatNoDic.Checked = True
                    Case 10
                        OptDicNoCat.Checked = True
                        '*** 20130619 - estrazione posizioni modificate ***
                    Case 11
                        OptDichMod.Checked = True
                    Case 12
                        OptDichIns.Checked = True
                End Select
                '*** 20130619 - estrazione posizioni modificate ***
                dim sSQL as string
                Dim oLoadCombo As New generalClass.generalFunction
                sSQL = "exec prc_GetUtentiEnte '" + ConstSession.IdEnte + "'"
                oLoadCombo.LoadComboGenerale(DdlOperatore, sSQL, ConstSession.StringConnectionOPENgov, True, Costanti.TipoDefaultCmb.STRINGA)
                '*** ***
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.CheckRifCatastali.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Funzione che richiama l'attivazione dei campi della videata in base all'opzione selezionata
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub OptCatDifferentDic_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles OptCatDifferentDic.CheckedChanged
        SetHiddenParam("A")
    End Sub
    ''' <summary>
    ''' Funzione che richiama l'attivazione dei campi della videata in base all'opzione selezionata
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub OptCatEqualDic_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles OptCatEqualDic.CheckedChanged
        SetHiddenParam("A")
    End Sub
    ''' <summary>
    ''' Funzione che richiama l'attivazione dei campi della videata in base all'opzione selezionata
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub OptCatGreaterDic_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles OptCatGreaterDic.CheckedChanged
        SetHiddenParam("A")
    End Sub
    ''' <summary>
    ''' Funzione che richiama l'attivazione dei campi della videata in base all'opzione selezionata
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub OptCatLowerDic_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles OptCatLowerDic.CheckedChanged
        SetHiddenParam("A")
    End Sub
    ''' <summary>
    ''' Funzione che richiama l'attivazione dei campi della videata in base all'opzione selezionata
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub OptCatNoDic_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles OptCatNoDic.CheckedChanged
        SetHiddenParam("A")
    End Sub
    ''' <summary>
    ''' Funzione che richiama l'attivazione dei campi della videata in base all'opzione selezionata
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub OptDichMod_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles OptDichMod.CheckedChanged
        SetHiddenParam("")
    End Sub
    ''' <summary>
    ''' Funzione che richiama l'attivazione dei campi della videata in base all'opzione selezionata
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub OptDichIns_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles OptDichIns.CheckedChanged
        SetHiddenParam("")
    End Sub
    ''' <summary>
    ''' Funzione che richiama l'attivazione dei campi della videata in base all'opzione selezionata
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub OptDicNoCat_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles OptDicNoCat.CheckedChanged
        SetHiddenParam("A")
    End Sub
    ''' <summary>
    ''' Funzione che richiama l'attivazione dei campi della videata in base all'opzione selezionata
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub OptRifAccertati_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles OptRifAccertati.CheckedChanged
        SetHiddenParam("A")
    End Sub
    ''' <summary>
    ''' Funzione che richiama l'attivazione dei campi della videata in base all'opzione selezionata
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub OptRifChiusi_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles OptRifChiusi.CheckedChanged
        SetHiddenParam("A")
    End Sub
    ''' <summary>
    ''' Funzione che richiama l'attivazione dei campi della videata in base all'opzione selezionata
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub OptRifDoppi_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles OptRifDoppi.CheckedChanged
        SetHiddenParam("A")
    End Sub
    ''' <summary>
    ''' Funzione che richiama l'attivazione dei campi della videata in base all'opzione selezionata
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub OptRifMancanti_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles OptRifMancanti.CheckedChanged
        SetHiddenParam("A")
    End Sub
    ''' <summary>
    ''' Funzione che effettua la ricerca in base ai parametri impostati.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdRicerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdRicerca.Click
        Dim nOptSel As Integer = 0
        Dim oListRifCat() As oUIVSCat
        Dim FncAnalisi As New ClsAnalisi

        Try
            '*** 20130201 - gestione mq da catasto per TARES ***'*** 20130619 - estrazione posizioni modificate ***
            Dim nAnno As Integer = Now.Year
            Dim nTolleranzaPos As Double = 1
            Dim nTolleranzaNeg As Double = 1
            Dim sOperatore, sDal, sAl As String

            If IsNumeric(DdlAnno.SelectedValue) Then
                nAnno = DdlAnno.SelectedValue
            End If
            
            If IsNumeric(TxtTolleranza.Text) Then
                nTolleranzaPos += TxtTolleranza.Text / 100
                nTolleranzaNeg -= TxtTolleranza.Text / 100
            End If
            sOperatore = DdlOperatore.SelectedValue
            If TxtDal.Text = "" Then
                sDal = DateTime.MaxValue.ToShortDateString
            Else
                sDal = TxtDal.Text
            End If
            If TxtAl.Text = "" Then
                sAl = DateTime.MaxValue.ToShortDateString
            Else
                sAl = TxtAl.Text
            End If
            If OptRifChiusi.Checked = True Then
                nOptSel = 1
            ElseIf OptRifDoppi.Checked = True Then
                nOptSel = 2
            ElseIf OptRifMancanti.Checked = True Then
                nOptSel = 3
            ElseIf OptRifAccertati.Checked = True Then
                nOptSel = 4
            ElseIf OptCatEqualDic.Checked = True Then
                nOptSel = 5
            ElseIf OptCatDifferentDic.Checked = True Then
                nOptSel = 6
            ElseIf OptCatGreaterDic.Checked = True Then
                nOptSel = 7
            ElseIf OptCatLowerDic.Checked = True Then
                nOptSel = 8
            ElseIf OptCatNoDic.Checked = True Then
                nOptSel = 9
            ElseIf OptDicNoCat.Checked = True Then
                nOptSel = 10
            ElseIf OptDichMod.Checked = True Then
                nOptSel = 11 '*** 20130619 - estrazione posizioni modificate ***
                nAnno = 0
            ElseIf OptDichIns.Checked = True Then
                nOptSel = 12
                nAnno = 0
            End If

            Session("OptSelCheckRifCatastali") = nOptSel
            Session("AnnoCheckRifCatastali") = DdlAnno.SelectedValue
            Session("TolleranzaCheckRifCatastali") = TxtTolleranza.Text
            Session("OperatoreCheckRifCatastali") = DdlOperatore.SelectedValue
            Session("DalCheckRifCatastali") = TxtDal.Text
            Session("AlCheckRifCatastali") = TxtAl.Text
            Session("dvResultCheckRifCatastali") = Nothing
            'prelevo i dati dal db dell'emesso
            oListRifCat = FncAnalisi.GetResultCheckRifCatastali(ConstSession.IdEnte, nAnno, nOptSel, nTolleranzaNeg, nTolleranzaPos, ConstSession.nPercentMQCat, sOperatore, sDal, sAl)
            LoadAnalisi(oListRifCat, nOptSel)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.CheckRifCatastali.CmdRicerca_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            DivAttesa.Style.Add("display", "none")
        End Try
    End Sub
    ''' <summary>
    ''' Pulsante per l'estrazione in formato XLS della ricerca effettuata
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdStampa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdStampa.Click
        Dim nOptSel As Integer = 0
        Dim oListRifCat() As oUIVSCat = Nothing
        Dim sNameXLS, sIntestazioneReport As String
        Dim sScript As String
        Dim x, nCol As Integer
        Dim FncStampa As New ClsStampaXLS
        Dim DtDatiStampa As New DataTable
        Dim aListColonne As ArrayList
        Dim aMyHeaders As String()

        nCol = 15
        'valorizzo il nome del file
        sNameXLS = ConstSession.IdEnte & "_ANALISIERIFCATASTALI_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
        Try
            Log.Debug("ResultCheckRifCatastali::CmdStampa_Click::inizio::" & Now)
            If OptRifChiusi.Checked = True Then
                nOptSel = 1
            ElseIf OptRifDoppi.Checked = True Then
                nOptSel = 2
            ElseIf OptRifMancanti.Checked = True Then
                nOptSel = 3
            ElseIf OptRifAccertati.Checked = True Then
                nOptSel = 4
            ElseIf OptCatEqualDic.Checked = True Then
                nOptSel = 5
            ElseIf OptCatDifferentDic.Checked = True Then
                nOptSel = 6
            ElseIf OptCatGreaterDic.Checked = True Then
                nOptSel = 7
            ElseIf OptCatLowerDic.Checked = True Then
                nOptSel = 8
            ElseIf OptCatNoDic.Checked = True Then
                nOptSel = 9
            ElseIf OptDicNoCat.Checked = True Then
                nOptSel = 10
            ElseIf OptDichMod.Checked = True Then
                nOptSel = 11 '*** 20130619 - estrazione posizioni modificate ***
            ElseIf OptDichIns.Checked = True Then
                nOptSel = 12
            End If
            '*** 20130619 - estrazione posizioni modificate ***
            GrdDichMod.Style.Add("display", "none")
            '*** 20130201 - gestione mq da catasto per TARES ***
            'prelevo i dati dal db dell'emesso
            oListRifCat = Session("dvResultCheckRifCatastali")
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.CheckRifCatastali.CmdStampa_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            DivAttesa.Style.Add("display", "none")
        End Try
        If Not oListRifCat Is Nothing Then
            sIntestazioneReport = ""
            Select Case nOptSel
                Case 1
                    sIntestazioneReport = "Analisi riferimenti chiusi e non riaperti"
                Case 2
                    sIntestazioneReport = "Analisi riferimenti presenti in più periodi contemporaneamente"
                Case 3
                    sIntestazioneReport = "Analisi riferimenti mancanti"
                Case 4
                    sIntestazioneReport = "Analisi riferimenti accertati"
                Case 5
                    sIntestazioneReport = "Metri Dichiarati uguali a Metri a Catasto"
                Case 6
                    sIntestazioneReport = "Metri Dichiarati diversi da Metri a Catasto"
                Case 7
                    sIntestazioneReport = "Metri Dichiarati minori di Metri a Catasto"
                Case 8
                    sIntestazioneReport = "Metri Dichiarati maggiori di Metri a Catasto"
                Case 9
                    sIntestazioneReport = "Metri a Catasto non presenti in Dichiarazione"
                Case 10
                    sIntestazioneReport = "Metri Dichiarati non presenti a Catasto"
                        '*** 20130619 - estrazione posizioni modificate ***
                Case 11
                    sIntestazioneReport = "Dichiarazioni Modificate"
                Case 12
                    sIntestazioneReport = "Dichiarazioni Inserite"
            End Select
            Log.Debug("ResultCheckRifCatastali::CmdStampa_Click::devo prelevare i dati da stampare::" & Now)
            Select Case nOptSel
                Case 4
                    DtDatiStampa = FncStampa.PrintCheckRifCatastaliAccertati(ConstSession.IdEnte & "-" & Session("DESCRIZIONE_ENTE"), sIntestazioneReport, oListRifCat)
                Case 5, 6, 7, 8
                    DtDatiStampa = FncStampa.PrintCheckMQDicVsCat(ConstSession.IdEnte & "-" & Session("DESCRIZIONE_ENTE"), sIntestazioneReport, oListRifCat)
                Case 9
                    DtDatiStampa = FncStampa.PrintCheckMQCatNoDic(ConstSession.IdEnte & "-" & Session("DESCRIZIONE_ENTE"), sIntestazioneReport, oListRifCat)
                Case 11, 12
                    DtDatiStampa = FncStampa.PrintDichMod(ConstSession.IdEnte & "-" & Session("DESCRIZIONE_ENTE"), sIntestazioneReport, oListRifCat)
                Case Else
                    DtDatiStampa = FncStampa.PrintCheckRifCatastali(ConstSession.IdEnte & "-" & Session("DESCRIZIONE_ENTE"), sIntestazioneReport, oListRifCat)
            End Select
            Log.Debug("ResultCheckRifCatastali::CmdStampa_Click::dati prelevati::" & Now)
            If Not DtDatiStampa Is Nothing Then
                DivAttesa.Style.Add("display", "none")
                'definisco le colonne
                aListColonne = New ArrayList
                For x = 0 To nCol
                    aListColonne.Add("")
                Next
                aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

                'definisco l'insieme delle colonne da esportare
                'Dim MyCol() As Integer = New Integer() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15}
                Dim MyCol() As Integer = New Integer(nCol) {}
                For x = 0 To nCol
                    MyCol(x) = x
                Next
                'esporto i dati in excel
                Dim MyStampa As New RKLib.ExportData.Export("Win")
                Log.Debug("ResultCheckRifCatastali::CmdStampa_Click::richiamo RKLIB::" & Now)
                MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, ConstSession.PathProspetti & sNameXLS)
                Log.Debug("ResultCheckRifCatastali::CmdStampa_Click::finito RKLIB::" & Now)
                LblDownloadFile.Text = sNameXLS
            Else
                sScript = "GestAlert('a', 'warning', '', '', 'Non ci sono dati da stampare');"
                RegisterScript(sScript, Me.GetType())
            End If
        Else
            sScript = "GestAlert('a', 'warning', '', '', 'Non ci sono dati da stampare');"
            RegisterScript(sScript, Me.GetType())
        End If
        '*** ***
    End Sub
    ''' <summary>
    ''' Pulsante per il download dell'estrazione effettuata.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub LblDownloadFile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LblDownloadFile.Click
        Dim sFileExport As String
        sFileExport = ConstSession.PathProspetti + LblDownloadFile.Text
        Response.ContentType = "*/*"
        Response.AppendHeader("content-disposition", ("attachment; filename=" + LblDownloadFile.Text))
        Response.WriteFile(sFileExport)
        Response.End()
    End Sub
    ''' <summary>
    ''' Funzione che abilita/disabilita le opzioni in base alla selezione
    ''' </summary>
    ''' <param name="myType"></param>
    Private Sub SetHiddenParam(ByVal myType As String)
        Try
            If myType = "A" Then
                Label1.Style.Add("display", "") : DdlAnno.Style.Add("display", "") : Label2.Style.Add("display", "") : TxtTolleranza.Style.Add("display", "")
                Label3.Style.Add("display", "none") : DdlOperatore.Style.Add("display", "none") : Label4.Style.Add("display", "none") : TxtDal.Style.Add("display", "none") : Label5.Style.Add("display", "none") : TxtAl.Style.Add("display", "none")
            Else
                Label1.Style.Add("display", "none") : DdlAnno.Style.Add("display", "none") : Label2.Style.Add("display", "none") : TxtTolleranza.Style.Add("display", "none")
                Label3.Style.Add("display", "") : DdlOperatore.Style.Add("display", "") : Label4.Style.Add("display", "") : TxtDal.Style.Add("display", "") : Label5.Style.Add("display", "") : TxtAl.Style.Add("display", "")
            End If
            GrdCheckRifCatastaliAcc.Style.Add("display", "none")
            GrdCheckMQDicVsCat.Style.Add("display", "none")
            GrdCheckMQCatNoDic.Style.Add("display", "none")
            GrdDichMod.Style.Add("display", "none")
            GrdCheckRifCatastali.Style.Add("display", "none")
        Catch Err As Exception
Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.CheckRifCatastali.SetHiddenParam.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

#Region "Griglie"
    ''' <summary>
    ''' Gestione del cambio pagina della griglia.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdPageIndexChanging(ByVal sender As Object, e As GridViewPageEventArgs)
        Dim nOptSel As Integer = 0
        Try
            If Not Session("OptSelCheckRifCatastali") Is Nothing Then
                nOptSel = CInt(Session("OptSelCheckRifCatastali"))
            End If
            LoadAnalisi(Session("dvResultCheckRifCatastali"), nOptSel, e.NewPageIndex)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.CheckRifCatastali.GrdPageIndexChanging.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
#End Region
    ''' <summary>
    ''' Funzione che carica nella pagina il risultato della ricerca effettuata.
    ''' </summary>
    ''' <param name="oListRifCat"></param>
    ''' <param name="nOptSel"></param>
    ''' <param name="page"></param>
    Private Sub LoadAnalisi(ByVal oListRifCat() As oUIVSCat, ByVal nOptSel As Integer, Optional ByVal page As Integer? = 0)
        Dim myGrd As Ribes.OPENgov.WebControls.RibesGridView
        Try
            GrdCheckRifCatastaliAcc.Style.Add("display", "none")
            GrdCheckMQDicVsCat.Style.Add("display", "none")
            GrdCheckMQCatNoDic.Style.Add("display", "none")
            GrdDichMod.Style.Add("display", "none")
            GrdCheckRifCatastali.Style.Add("display", "none")
            If Not oListRifCat Is Nothing Then
                'popolo la griglia
                Select Case nOptSel
                    Case 4
                        myGrd = GrdCheckRifCatastaliAcc
                    Case 5, 6, 7, 8
                        myGrd = GrdCheckMQDicVsCat
                    Case 9
                        myGrd = GrdCheckMQCatNoDic
                        '*** 20130619 - estrazione posizioni modificate ***
                    Case 11, 12
                        myGrd = GrdDichMod
                    Case Else
                        myGrd = GrdCheckRifCatastali
                End Select
                myGrd.DataSource = oListRifCat
                If page.HasValue Then
                    myGrd.PageIndex = page.Value
                End If
                myGrd.DataBind()
                myGrd.Style.Add("display", "")
                Session("paginaSelezionata") = Nothing
                Session("dvResultCheckRifCatastali") = oListRifCat
                LblResult.Style.Add("display", "none")
            Else
                GrdCheckRifCatastaliAcc.Style.Add("display", "none")
                GrdCheckMQDicVsCat.Style.Add("display", "none")
                GrdCheckMQCatNoDic.Style.Add("display", "none")
                GrdDichMod.Style.Add("display", "none")
                GrdCheckRifCatastali.Style.Add("display", "none")
                LblResult.Text = "La ricerca non ha prodotto risultati."
                LblResult.Style.Add("display", "")
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.CheckRifCatastali.LoadAnalisi.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class