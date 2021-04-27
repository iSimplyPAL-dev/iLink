Imports ComPlusInterface
Imports log4net
''' <summary>
''' Pagina per la gestione manuale di un immobile in provvedimento.
''' Contiene le funzioni della comandiera e la griglia per la visualizzazione del risultato. 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class InserimentoManualeImmobile
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(InserimentoManualeImmobile))
    Protected UrlStradario As String = ConstSession.UrlStradario
    Protected StileStradario As String = ConstSession.stilestradario

    Private idLegame As Integer

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtZona As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label

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
        Try
            If TxtViaRibaltata.Text <> "" Then
                txtUbicazione.Text = TxtViaRibaltata.Text
            End If
            hfAnno.Value = Request.Item("anno")
            txtCodComune.Text = ConstSession.IdEnte
            txtComune.Text = ConstSession.DescrizioneEnte

            'chkPert.Attributes.Add("onClick", "AssociaPertinenze")
            btnRibalta.Attributes.Add("onClick", "return checkDate('" & hfAnno.Value & "');")
            chkPrinc.Attributes.Add("onClick", "checkPrincipale();")

            lblUbicazione.Attributes.Add("onclick", "ApriStradario();")
            lblUbicazione.Attributes.Add("style", "text-decoration:underline;cursor:hand;")

            txtUbicazione.Attributes.Add("onchange", "PulisciCodVia();")

            If Not Page.IsPostBack Then
                lnkRendita.Attributes.Add("onclick", "return CalcolaTariffa();")
                lnkValore.Attributes.Add("onclick", "return CalcolaValoreImmobile();")
                loadCombo()
                'Log.Debug("InserimentoManualeImmobile::Page_Load::loadcombo fatto")
                'loadComboTitPossesso()
                If Not Request.Item("idProgressivo") Is Nothing Then
                    If Request.Item("idProgressivo") <> "" Then
                        LoadImmobile(CLng(Request.Item("idProgressivo")))
                    End If
                End If
                'Log.Debug("InserimentoManualeImmobile::Page_Load::loadimmobile fatto")
                '*** 20130304 - gestione dati da territorio ***
                Try
                    If Not Request.Item("txtIdTerUI") Is Nothing Then
                        If IsNumeric(Request.Item("txtIdTerUI")) Then
                            ControlsBindFromTer(Request.Item("codContribuente"), Request.Item("txtIdTerUI"), Request.Item("txtIdTerProprieta"), Request.Item("txtIdTerProprietario"))
                        End If
                    End If
                Catch ex As Exception
                    Log.Debug("InserimentoManualeImmobile::Page_Load::errore in gestione dati da territorio", ex)
                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeImmobile.Page_Load.errore: ", ex)
                    ' Response.Redirect("../../PaginaErrore.aspx")
                End Try
                '*** ***
                dim sScript as string=""
                sScript += "parent.Comandi.location.href='ComandiInserimentoImmobili.aspx';"
                If ConstSession.CodTributo = Utility.Costanti.TRIBUTO_TASI Then
                    sScript += "$('#divTipoTASI').show();"
                Else
                    sScript += "$('#divTipoTASI').hide();"
                End If
                RegisterScript(sScript , Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeImmobile.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            'Response.Write(GestErrore.GetHTMLError(ex, "../../../Styles.css"))
            'Response.End()
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnRibalta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRibalta.Click
        Try
            Dim myArray As New ArrayList()
            Dim ListToCalc() As objSituazioneFinale
            Dim worktable() As objUIICIAccert
            Dim tempRow As New objSituazioneFinale
            Dim myProp As New objSituazioneFinale
            Dim myUIAcc As New objUIICIAccert
            Dim sRequestIdProgr As String
            Dim mesiEsclusioneEsenzione As Integer
            Dim remObjectFreezer As IFreezer = Activator.GetObject(GetType(IFreezer), ConstSession.URLServiziFreezer)
            Dim objHashTable As New Hashtable

            If Not Session("DataTableImmobiliDaAccertare") Is Nothing Then
                myArray = New ArrayList(CType(Session("DataTableImmobiliDaAccertare"), objUIICIAccert()))
            End If
            If Request.Item("idProgressivo") Is Nothing Then
                sRequestIdProgr = ""
            Else
                sRequestIdProgr = Request.Item("idProgressivo")
            End If
            tempRow.IdEnte = ConstSession.IdEnte
            tempRow.Anno = hfAnno.Value
            tempRow.Tributo = ConstSession.CodTributo
            If txtDal.Text <> "" Then
                tempRow.Dal = CDate(txtDal.Text) '
            End If
            If txtAl.Text <> "" Then
                tempRow.Al = CDate(txtAl.Text)
            End If
            If tempRow.Al < CDate("30/06/" & hfAnno.Value) Then
                tempRow.AccMesi = DateDiff(DateInterval.Month, tempRow.Dal, tempRow.Al) + 1
            Else
                tempRow.AccMesi = DateDiff(DateInterval.Month, tempRow.Dal, CDate("30/06/" & hfAnno.Value)) + 1
            End If
            If tempRow.AccMesi > 6 Then
                tempRow.AccMesi = 6
            End If
            If tempRow.Al <> DateTime.MaxValue Then
                If Year(tempRow.Al) > hfAnno.Value Then
                    tempRow.Mesi = DateDiff(DateInterval.Month, tempRow.Dal, CDate("31/12/" & hfAnno.Value)) + 1
                Else
                    tempRow.Mesi = DateDiff(DateInterval.Month, tempRow.Dal, tempRow.Al) + 1
                End If
            Else
                tempRow.Mesi = DateDiff(DateInterval.Month, tempRow.Dal, CDate("31/12/" & hfAnno.Value)) + 1
            End If
            If tempRow.Mesi > 12 Then
                tempRow.Mesi = 12
            End If
            tempRow.Foglio = txtFoglio.Text
            tempRow.Numero = txtNumero.Text
            If txtSubalterno.Text <> "" Then
                tempRow.Subalterno = txtSubalterno.Text
            End If
            tempRow.Categoria = ddlCategoria.SelectedValue 'txtCategoria.Text
            tempRow.Classe = ddlClasse.SelectedValue  ' txtClasse.Text
            If IsNumeric(txtConsistenza.Text) Then
                tempRow.Consistenza = txtConsistenza.Text
            End If
            tempRow.TipoRendita = Left(ddlTR.SelectedItem.Text, 2) 'ddlTR.SelectedValue
            'Dipe 26/02/2008 aggiunto codice per calcolare valore/rendita
            'Valore = objDBProvv.CalcoloValoredaRendita(txtRendita.Text, Left(ddlTR.SelectedItem.Text, 2), ddlCategoria.SelectedValue, txtAnnoAccertamento.Text)
            If IsNumeric(txtRendita.Text) Then
                tempRow.Rendita = txtRendita.Text
            End If
            If IsNumeric(txtValore.Text) Then
                tempRow.Valore = txtValore.Text
            End If
            tempRow.Zona = ddlZona.SelectedValue
            If IsNumeric(txtPercICI.Text) Then
                tempRow.PercPossesso = txtPercICI.Text
            End If
            If ddlTitPossesso.SelectedIndex > 0 Then
                tempRow.IdTipoUtilizzo = ddlTitPossesso.SelectedValue
                tempRow.TitPossesso = ddlTitPossesso.SelectedItem.Text
            End If
            If chkPrinc.Checked Then
                tempRow.FlagPrincipale = 1
            End If
            If chkPert.Checked Then
                tempRow.FlagPrincipale = 2
                tempRow.IdImmobilePertinenza = Session("IdImmobileDiPertinenza")
            End If
            tempRow.FlagRiduzione = chkRidotto.Checked
            If chkRidotto.Checked Then
                tempRow.MesiRiduzione = 12
            End If
            tempRow.Sezione = txtSezione.Text
            If IsNumeric(txtNumUtiliz.Text) Then
                tempRow.NUtilizzatori = txtNumUtiliz.Text
            End If
            tempRow.Via = txtUbicazione.Text
            If IsNumeric(txtCodVia.Text) Then
                tempRow.IdVia = txtCodVia.Text
            End If
            tempRow.NCIVICO = txtCivico.Text     'numeroCivico
            tempRow.ESPonente = txtEspCivico.Text     'EspCivico
            tempRow.SCALA = txtScala.Text     'Scala
            tempRow.INTERNO = txtInterno.Text     'Interno
            tempRow.PIANO = txtPiano.Text     'Piano
            tempRow.BARRATO = txtBarrato.Text     'barrato
            'Se sono presenti entrambe le date dell'immobile e mesiEsclusioneEsenzione=0 (selezionato SI)
            'allora calcolo l'effettivo numero di mesi in base alle date
            mesiEsclusioneEsenzione = 0
            If ddlEsente.SelectedValue = 0 Then
                mesiEsclusioneEsenzione = DateDiff(DateInterval.Month, tempRow.Dal, tempRow.Al) + 1
                If mesiEsclusioneEsenzione > 12 Then
                    mesiEsclusioneEsenzione = 12
                End If
            End If
            tempRow.MesiEsenzione = mesiEsclusioneEsenzione    'mesiEsclusioneEsenzione
            tempRow.FlagEsente = ddlEsente.SelectedValue 'FLAG_ESENTE
            '*** 20120530 - IMU ***
            tempRow.IsColtivatoreDiretto = chkcoltivatore.Checked
            If IsNumeric(txtnumfigli.Text) = True Then
                tempRow.NumeroFigli = CInt(txtnumfigli.Text) '
                For Each oItemGrid As GridViewRow In GrdCaricoFigli.Rows
                    tempRow.PercentCaricoFigli = Integer.Parse(CType(oItemGrid.FindControl("TxtPercentCarico"), TextBox).Text.ToString)
                Next
            End If
            '*** ***
            myArray = New ArrayList()
            If chkInqu.Checked Then
                tempRow.TipoTasi = Utility.Costanti.TIPOTASI_INQUILINO
                tempRow.DescrTipoTasi = "INQUILINO"
                If chkProp.Checked Then
                    myProp = New objSituazioneFinale
                    myProp.Tributo = tempRow.Tributo
                    myProp.IdEnte = tempRow.IdEnte
                    myProp.Anno = tempRow.Anno
                    myProp.TipoRendita = tempRow.TipoRendita
                    myProp.Categoria = tempRow.Categoria
                    myProp.Classe = tempRow.Classe
                    myProp.Zona = tempRow.Zona
                    myProp.Foglio = tempRow.Foglio
                    myProp.Numero = tempRow.Numero
                    myProp.Subalterno = tempRow.Subalterno
                    myProp.Provenienza = tempRow.Provenienza
                    myProp.Caratteristica = tempRow.Caratteristica
                    myProp.Via = tempRow.Via
                    myProp.NCivico = tempRow.NCivico
                    myProp.Esponente = tempRow.Esponente
                    myProp.Scala = tempRow.Scala
                    myProp.Interno = tempRow.Interno
                    myProp.Piano = tempRow.Piano
                    myProp.Barrato = tempRow.Barrato
                    myProp.Sezione = tempRow.Sezione
                    myProp.Protocollo = tempRow.Protocollo
                    myProp.DataScadenza = tempRow.DataScadenza
                    myProp.DataInizio = tempRow.DataInizio
                    myProp.TipoOperazione = tempRow.TipoOperazione
                    myProp.TitPossesso = tempRow.TitPossesso
                    myProp.Id = tempRow.Id
                    myProp.IdContribuente = tempRow.IdContribuente
                    myProp.IdContribuenteCalcolo = tempRow.IdContribuenteCalcolo
                    myProp.IdProcedimento = tempRow.IdProcedimento
                    myProp.IdRiferimento = tempRow.IdRiferimento
                    myProp.IdLegame = tempRow.IdLegame
                    myProp.Progressivo = tempRow.Progressivo
                    myProp.IdVia = tempRow.IdVia
                    myProp.NumeroFigli = tempRow.NumeroFigli
                    myProp.MesiPossesso = tempRow.MesiPossesso
                    myProp.Mesi = tempRow.Mesi
                    myProp.IdTipoUtilizzo = tempRow.IdTipoUtilizzo
                    myProp.IdTipoPossesso = tempRow.IdTipoPossesso
                    myProp.NUtilizzatori = tempRow.NUtilizzatori
                    myProp.FlagPrincipale = tempRow.FlagPrincipale
                    myProp.FlagRiduzione = tempRow.FlagRiduzione
                    myProp.FlagEsente = tempRow.FlagEsente
                    myProp.FlagStorico = tempRow.FlagStorico
                    myProp.FlagPosseduto = tempRow.FlagPosseduto
                    myProp.FlagProvvisorio = tempRow.FlagProvvisorio
                    myProp.MesiRiduzione = tempRow.MesiRiduzione
                    myProp.MesiEsenzione = tempRow.MesiEsenzione
                    myProp.AccMesi = tempRow.AccMesi
                    myProp.IdImmobile = tempRow.IdImmobile
                    myProp.IdImmobilePertinenza = tempRow.IdImmobilePertinenza
                    myProp.IdImmobileDichiarato = tempRow.IdImmobileDichiarato
                    myProp.MeseInizio = tempRow.MeseInizio
                    myProp.AbitazionePrincipaleAttuale = tempRow.AbitazionePrincipaleAttuale
                    myProp.AccSenzaDetrazione = tempRow.AccSenzaDetrazione
                    myProp.AccDetrazioneApplicata = tempRow.AccDetrazioneApplicata
                    myProp.AccDovuto = tempRow.AccDovuto
                    myProp.AccDetrazioneResidua = tempRow.AccDetrazioneResidua
                    myProp.SalSenzaDetrazione = tempRow.SalSenzaDetrazione
                    myProp.SalDetrazioneApplicata = tempRow.SalDetrazioneApplicata
                    myProp.SalDovuto = tempRow.SalDovuto
                    myProp.SalDetrazioneResidua = tempRow.SalDetrazioneResidua
                    myProp.TotSenzaDetrazione = tempRow.TotSenzaDetrazione
                    myProp.TotDetrazioneApplicata = tempRow.TotDetrazioneApplicata
                    myProp.TotDovuto = tempRow.TotDovuto
                    myProp.TotDetrazioneResidua = tempRow.TotDetrazioneResidua
                    myProp.IdAliquota = tempRow.IdAliquota
                    myProp.Aliquota = tempRow.Aliquota
                    myProp.AliquotaStatale = tempRow.AliquotaStatale
                    myProp.PercentCaricoFigli = tempRow.PercentCaricoFigli
                    myProp.AccDovutoStatale = tempRow.AccDovutoStatale
                    myProp.AccDetrazioneApplicataStatale = tempRow.AccDetrazioneApplicataStatale
                    myProp.AccDetrazioneResiduaStatale = tempRow.AccDetrazioneResiduaStatale
                    myProp.SalDovutoStatale = tempRow.SalDovutoStatale
                    myProp.SalDetrazioneApplicataStatale = tempRow.SalDetrazioneApplicataStatale
                    myProp.SalDetrazioneResiduaStatale = tempRow.SalDetrazioneResiduaStatale
                    myProp.TotDovutoStatale = tempRow.TotDovutoStatale
                    myProp.TotDetrazioneApplicataStatale = tempRow.TotDetrazioneApplicataStatale
                    myProp.TotDetrazioneResiduaStatale = tempRow.TotDetrazioneResiduaStatale
                    myProp.PercPossesso = tempRow.PercPossesso
                    myProp.Rendita = tempRow.Rendita
                    myProp.Valore = tempRow.Valore
                    myProp.ValoreReale = tempRow.ValoreReale
                    myProp.Consistenza = tempRow.Consistenza
                    myProp.ImpDetrazione = tempRow.ImpDetrazione
                    myProp.DiffImposta = tempRow.DiffImposta
                    myProp.Totale = tempRow.Totale
                    myProp.IsColtivatoreDiretto = tempRow.IsColtivatoreDiretto
                    myProp.Dal = tempRow.Dal
                    myProp.Al = tempRow.Al
                    myProp.TipoTasi = Utility.Costanti.TIPOTASI_PROPRIETARIO
                    myProp.DescrTipoTasi = "PROPRIETARIO"
                    'ricalcolo i mesi
                    myProp.Mesi = remObjectFreezer.CalcolaMesi(myProp.Dal, myProp.Al, myProp.Anno)
                    myArray.Add(myProp)
                End If
            End If
            'ricalcolo i mesi
            tempRow.Mesi = remObjectFreezer.CalcolaMesi(tempRow.Dal, tempRow.Al, tempRow.Anno)
            myArray.Add(tempRow)
            objHashTable.Add("CONNECTIONSTRINGOPENGOV", ConstSession.StringConnectionOPENgov)
            objHashTable.Add("CONNECTIONSTRINGOPENGOVICI", ConstSession.StringConnectionICI)
            objHashTable.Add("CodENTE", ConstSession.IdEnte)
            ListToCalc = CType(myArray.ToArray(GetType(objSituazioneFinale)), objSituazioneFinale())
            '*** devo fare questo volo pindarico passando un arraylist di stringhe perché non produzione non accetta in ingresso l'oggetto valorizzato su cui lavorare ***
            Dim ArrayStringUI As New ArrayList
            Dim ListSituazioneFinale() As objSituazioneFinale = Nothing
            ArrayStringUI = ObjToArrayList(ListToCalc)
            If remObjectFreezer.CalcoloFromUI(ConstSession.StringConnectionOPENgov, ConstSession.StringConnectionICI, ConstSession.IdEnte, ArrayStringUI, 0, ListSituazioneFinale) = False Then
                Throw New Exception("Errore in calcolo ICI")
            End If
            '*** ***
            myArray = New ArrayList()
            Dim MaxProg As Integer = 1
            If Not Session("DataTableImmobiliDaAccertare") Is Nothing Then
                worktable = CType(Session("DataTableImmobiliDaAccertare"), objUIICIAccert())
                For Each myItem As objUIICIAccert In worktable
                    If myItem.Progressivo.ToString <> sRequestIdProgr Then
                        myArray.Add(myItem)
                    Else
                        myuiacc = myItem
                    End If
                    MaxProg = myItem.Progressivo + 1
                Next
            End If
            For Each myUI As objSituazioneFinale In ListSituazioneFinale 'myCalc
                myUIAcc = CastFormToAcc(myUI, myUIAcc, maxprog)
                myArray.Add(myUIAcc)
            Next
            worktable = CType(myArray.ToArray(GetType(objUIICIAccert)), objUIICIAccert())
            Array.Sort(worktable, New Utility.Comparatore(New String() {"IdLegame"}, New Boolean() {Utility.TipoOrdinamento.Crescente}))
            Session.Remove("IdImmobileDiPertinenza")
            Session("DataTableImmobiliDaAccertare") = workTable

            Dim sScript As String = ""
            If (ConstSession.CodTributo <> Utility.Costanti.TRIBUTO_TASI) Then
                sScript += "parent.parent.opener.location.href='grdAccertato.aspx';"
            Else
                sScript += "parent.parent.opener.location.href='../GestioneAccertamentiTASI/SearchDatiAccertato.aspx';"
            End If
            sScript += "parent.window.close();"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeImmobile.btnRibalta_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ListUI"></param>
    ''' <returns></returns>
    Private Function ObjToArrayList(ListUI() As objSituazioneFinale) As ArrayList
        Dim myArray As New ArrayList
        Try
            For Each myUI As objSituazioneFinale In ListUI
                myArray.Add(myUI.Tributo)
                myArray.Add(myUI.IdEnte)
                myArray.Add(myUI.Anno)
                myArray.Add(myUI.TipoRendita)
                myArray.Add(myUI.Categoria)
                myArray.Add(myUI.Classe)
                myArray.Add(myUI.Zona)
                myArray.Add(myUI.Foglio)
                myArray.Add(myUI.Numero)
                myArray.Add(myUI.Subalterno)
                myArray.Add(myUI.Provenienza)
                myArray.Add(myUI.Caratteristica)
                myArray.Add(myUI.Via)
                myArray.Add(myUI.NCivico)
                myArray.Add(myUI.Esponente)
                myArray.Add(myUI.Scala)
                myArray.Add(myUI.Interno)
                myArray.Add(myUI.Piano)
                myArray.Add(myUI.Barrato)
                myArray.Add(myUI.Sezione)
                myArray.Add(myUI.Protocollo)
                myArray.Add(myUI.DataScadenza)
                myArray.Add(myUI.DataInizio)
                myArray.Add(myUI.TipoOperazione)
                myArray.Add(myUI.TitPossesso)
                myArray.Add(myUI.Id)
                myArray.Add(myUI.IdContribuente)
                myArray.Add(myUI.IdContribuenteCalcolo)
                myArray.Add(myUI.IdProcedimento)
                myArray.Add(myUI.IdRiferimento)
                myArray.Add(myUI.IdLegame)
                myArray.Add(myUI.Progressivo)
                myArray.Add(myUI.IdVia)
                myArray.Add(myUI.NumeroFigli)
                myArray.Add(myUI.MesiPossesso)
                myArray.Add(myUI.Mesi)
                myArray.Add(myUI.IdTipoUtilizzo)
                myArray.Add(myUI.IdTipoPossesso)
                myArray.Add(myUI.NUtilizzatori)
                myArray.Add(myUI.FlagPrincipale)
                myArray.Add(myUI.FlagRiduzione)
                myArray.Add(myUI.FlagEsente)
                myArray.Add(myUI.FlagStorico)
                myArray.Add(myUI.FlagPosseduto)
                myArray.Add(myUI.FlagProvvisorio)
                myArray.Add(myUI.MesiRiduzione)
                myArray.Add(myUI.MesiEsenzione)
                myArray.Add(myUI.AccMesi)
                myArray.Add(myUI.IdImmobile)
                myArray.Add(myUI.IdImmobilePertinenza)
                myArray.Add(myUI.IdImmobileDichiarato)
                myArray.Add(myUI.MeseInizio)
                myArray.Add(myUI.AbitazionePrincipaleAttuale)
                myArray.Add(myUI.AccSenzaDetrazione)
                myArray.Add(myUI.AccDetrazioneApplicata)
                myArray.Add(myUI.AccDovuto)
                myArray.Add(myUI.AccDetrazioneResidua)
                myArray.Add(myUI.SalSenzaDetrazione)
                myArray.Add(myUI.SalDetrazioneApplicata)
                myArray.Add(myUI.SalDovuto)
                myArray.Add(myUI.SalDetrazioneResidua)
                myArray.Add(myUI.TotSenzaDetrazione)
                myArray.Add(myUI.TotDetrazioneApplicata)
                myArray.Add(myUI.TotDovuto)
                myArray.Add(myUI.TotDetrazioneResidua)
                myArray.Add(myUI.IdAliquota)
                myArray.Add(myUI.Aliquota)
                myArray.Add(myUI.AliquotaStatale)
                myArray.Add(myUI.PercentCaricoFigli)
                myArray.Add(myUI.AccDovutoStatale)
                myArray.Add(myUI.AccDetrazioneApplicataStatale)
                myArray.Add(myUI.AccDetrazioneResiduaStatale)
                myArray.Add(myUI.SalDovutoStatale)
                myArray.Add(myUI.SalDetrazioneApplicataStatale)
                myArray.Add(myUI.SalDetrazioneResiduaStatale)
                myArray.Add(myUI.TotDovutoStatale)
                myArray.Add(myUI.TotDetrazioneApplicataStatale)
                myArray.Add(myUI.TotDetrazioneResiduaStatale)
                myArray.Add(myUI.PercPossesso)
                myArray.Add(myUI.Rendita)
                myArray.Add(myUI.Valore)
                myArray.Add(myUI.ValoreReale)
                myArray.Add(myUI.Consistenza)
                myArray.Add(myUI.ImpDetrazione)
                myArray.Add(myUI.DiffImposta)
                myArray.Add(myUI.Totale)
                myArray.Add(myUI.IsColtivatoreDiretto)
                myArray.Add(myUI.Dal)
                myArray.Add(myUI.Al)
                myArray.Add(myUI.TipoTasi)
                myArray.Add(myUI.DescrTipoTasi)
            Next
        Catch ex As Exception
            Log.Debug("ObjToArrayList.errore->", ex)
            myArray = Nothing
        End Try
        Return myArray
    End Function
    '*** 20130304 - gestione dati da territorio ***
    'Private Sub ControlsBindFromTer(ByVal IdContribuente As String, ByVal IdUI As Integer, ByVal IdProprieta As Integer, ByVal IdProprietario As Integer)
    '    Dim WFSessione As OPENUtility.CreateSessione = New OPENUtility.CreateSessione(HttpContext.Current.Session("PARAMETROENV").ToString, ConstSession.UserName.ToString, System.Configuration.ConfigurationManager.AppSettings("OPENGOVT"))
    '    Dim x As Integer = 0
    '    Try
    '        Dim WFErrore As String = ""
    '        'inizializzo la connessione
    '        If Not WFSessione.CreaSessione(ConstSession.UserName.ToString, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If
    '        Dim myDsResult As DataSet = New DataSet
    '        myDsResult = New DichiarazioniICI.Database.UITerritorio().GetRow(WFSessione, ConstSession.IdEnte.ToString, IdContribuente, -1, "", "", "", IdUI, IdProprieta, IdProprietario)
    '        If (Not (myDsResult) Is Nothing) Then
    '            If (myDsResult.Tables(0).Rows.Count > 0) Then
    '                For x = 0 To myDsResult.Tables(0).Rows.Count - 1
    '                    If (myDsResult.Tables(0).Rows(x)("ValoreCatastale").ToString <> "-1,00") Then
    '                        txtValore.Text = myDsResult.Tables(0).Rows(x)("ValoreCatastale").ToString
    '                    Else
    '                        txtValore.Text = String.Empty
    '                    End If
    '                    txtUbicazione.Text = myDsResult.Tables(0).Rows(x)("indirizzo").ToString
    '                    txtCodVia.Text = myDsResult.Tables(0).Rows(x)("id_Via").ToString
    '                    If (Integer.Parse(myDsResult.Tables(0).Rows(x)("Civico").ToString) = -1) Then
    '                        txtCivico.Text = String.Empty
    '                    Else
    '                        txtCivico.Text = myDsResult.Tables(0).Rows(x)("Civico").ToString
    '                    End If
    '                    txtEspCivico.Text = myDsResult.Tables(0).Rows(x)("Esponente").ToString
    '                    txtInterno.Text = myDsResult.Tables(0).Rows(x)("Interno").ToString
    '                    txtScala.Text = myDsResult.Tables(0).Rows(x)("Scala").ToString
    '                    txtPiano.Text = myDsResult.Tables(0).Rows(x)("Piano").ToString
    '                    txtFoglio.Text = myDsResult.Tables(0).Rows(x)("Foglio").ToString
    '                    txtNumero.Text = myDsResult.Tables(0).Rows(x)("Numero").ToString
    '                    If (Integer.Parse(myDsResult.Tables(0).Rows(x)("Subalterno").ToString) = -1) Then
    '                        txtSubalterno.Text = String.Empty
    '                    Else
    '                        txtSubalterno.Text = myDsResult.Tables(0).Rows(x)("Subalterno").ToString
    '                    End If
    '                    ddlCategoria.SelectedItem.Selected = False
    '                    ddlCategoria.Items.FindByValue(myDsResult.Tables(0).Rows(x)("CatCatastale").ToString).Selected = True
    '                    ddlClasse.SelectedItem.Selected = False
    '                    ddlClasse.Items.FindByValue(myDsResult.Tables(0).Rows(x)("ClasseCatastale").ToString).Selected = True
    '                    'ddlCodiceRendita.SelectedItem.Selected = False
    '                    'ddlCodiceRendita.Items.FindByValue(myDsResult.Tables(0).Rows(x)("Cod_tipo_Rendita").ToString).Selected = True
    '                    '' popolo la combo estimo
    '                    'If (ddlCodiceRendita.SelectedItem.Text.CompareTo("AF") = 0) Then
    '                    '    PopolaDDlEstimo("AF")
    '                    'Else
    '                    '    PopolaDDlEstimo("")
    '                    'End If
    '                    'ddlCaratteristica.SelectedItem.Selected = False
    '                    'Select Case (ddlCodiceRendita.SelectedValue)
    '                    '    Case "TA"
    '                    '        ddlCaratteristica.Items.FindByValue("1").Selected = True
    '                    '    Case "AF"
    '                    '        ddlCaratteristica.Items.FindByValue("2").Selected = True
    '                    '        PopolaDDlEstimo("AF")
    '                    '    Case "RE"
    '                    '        ddlCaratteristica.Items.FindByValue("3").Selected = True
    '                    '    Case "LC"
    '                    '        ddlCaratteristica.Items.FindByValue("4").Selected = True
    '                    'End Select
    '                    txtDal.Text = Business.Utility.FormattaDataGrd(myDsResult.Tables(0).Rows(x)("Data_Inizio"))
    '                    txtAl.Text = Business.Utility.FormattaDataGrd(myDsResult.Tables(0).Rows(x)("Data_Fine"))
    '                    If ((myDsResult.Tables(0).Rows(x)("Consistenza").ToString.CompareTo("-1,00") = 0) _
    '                                OrElse (myDsResult.Tables(0).Rows(x)("Consistenza").ToString.CompareTo("-1") = 0)) Then
    '                        txtConsistenza.Text = String.Empty
    '                    Else
    '                        txtConsistenza.Text = myDsResult.Tables(0).Rows(x)("Consistenza").ToString
    '                    End If
    '                    If (myDsResult.Tables(0).Rows(x)("Percentuale_proprieta").ToString = "-1") Then
    '                        txtPercICI.Text = String.Empty
    '                    Else
    '                        txtPercICI.Text = myDsResult.Tables(0).Rows(x)("Percentuale_proprieta").ToString
    '                    End If
    '                    'If (myDsResult.Tables(0).Rows(x)("MesiPossesso").ToString = "-1") Then
    '                    '    txtMesiPossesso.Text = String.Empty
    '                    'Else
    '                    '    txtMesiPossesso.Text = myDsResult.Tables(0).Rows(x)("MesiPossesso").ToString
    '                    'End If
    '                    ddlTitPossesso.SelectedItem.Selected = False
    '                    ddlTitPossesso.Items.FindByValue(myDsResult.Tables(0).Rows(x)("cod_tipo_proprieta").ToString).Selected = True
    '                Next
    '            End If
    '        End If
    '    Catch Err As Exception
    '         Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeImmobile.ControlsBindFromTer.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    Finally
    '        WFSessione.Kill()
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="IdContribuente"></param>
    ''' <param name="IdUI"></param>
    ''' <param name="IdProprieta"></param>
    ''' <param name="IdProprietario"></param>
    Private Sub ControlsBindFromTer(ByVal IdContribuente As String, ByVal IdUI As Integer, ByVal IdProprieta As Integer, ByVal IdProprietario As Integer)
        'Dim x As Integer = 0
        Try
            Dim sScript As String = "GestAlert('a', 'info', '', '', 'Funzionalita\' non attiva.');"
            RegisterScript(sScript, Me.GetType())
            'Dim myDsResult As DataSet = New DataSet
            'myDsResult = New DichiarazioniICI.Database.UITerritorio().GetRow(ConstSession.StringConnectionTERRITORIO, ConstSession.IdEnte.ToString, IdContribuente, -1, "", "", "", IdUI, IdProprieta, IdProprietario)
            'If (Not (myDsResult) Is Nothing) Then
            '    If (myDsResult.Tables(0).Rows.Count > 0) Then
            '        For x = 0 To myDsResult.Tables(0).Rows.Count - 1
            '            If (myDsResult.Tables(0).Rows(x)("ValoreCatastale").ToString <> "-1,00") Then
            '                txtValore.Text = myDsResult.Tables(0).Rows(x)("ValoreCatastale").ToString
            '            Else
            '                txtValore.Text = String.Empty
            '            End If
            '            txtUbicazione.Text = myDsResult.Tables(0).Rows(x)("indirizzo").ToString
            '            txtCodVia.Text = myDsResult.Tables(0).Rows(x)("id_Via").ToString
            '            If (Integer.Parse(myDsResult.Tables(0).Rows(x)("Civico").ToString) = -1) Then
            '                txtCivico.Text = String.Empty
            '            Else
            '                txtCivico.Text = myDsResult.Tables(0).Rows(x)("Civico").ToString
            '            End If
            '            txtEspCivico.Text = myDsResult.Tables(0).Rows(x)("Esponente").ToString
            '            txtInterno.Text = myDsResult.Tables(0).Rows(x)("Interno").ToString
            '            txtScala.Text = myDsResult.Tables(0).Rows(x)("Scala").ToString
            '            txtPiano.Text = myDsResult.Tables(0).Rows(x)("Piano").ToString
            '            txtFoglio.Text = myDsResult.Tables(0).Rows(x)("Foglio").ToString
            '            txtNumero.Text = myDsResult.Tables(0).Rows(x)("Numero").ToString
            '            If (Integer.Parse(myDsResult.Tables(0).Rows(x)("Subalterno").ToString) = -1) Then
            '                txtSubalterno.Text = String.Empty
            '            Else
            '                txtSubalterno.Text = myDsResult.Tables(0).Rows(x)("Subalterno").ToString
            '            End If
            '            ddlCategoria.SelectedItem.Selected = False
            '            ddlCategoria.Items.FindByValue(myDsResult.Tables(0).Rows(x)("CatCatastale").ToString).Selected = True
            '            ddlClasse.SelectedItem.Selected = False
            '            ddlClasse.Items.FindByValue(myDsResult.Tables(0).Rows(x)("ClasseCatastale").ToString).Selected = True
            '            txtDal.Text = Business.CoreUtility.FormattaDataGrd(myDsResult.Tables(0).Rows(x)("Data_Inizio"))
            '            txtAl.Text = Business.CoreUtility.FormattaDataGrd(myDsResult.Tables(0).Rows(x)("Data_Fine"))
            '            If ((myDsResult.Tables(0).Rows(x)("Consistenza").ToString.CompareTo("-1,00") = 0) _
            '                        OrElse (myDsResult.Tables(0).Rows(x)("Consistenza").ToString.CompareTo("-1") = 0)) Then
            '                txtConsistenza.Text = String.Empty
            '            Else
            '                txtConsistenza.Text = myDsResult.Tables(0).Rows(x)("Consistenza").ToString
            '            End If
            '            If (myDsResult.Tables(0).Rows(x)("Percentuale_proprieta").ToString = "-1") Then
            '                txtPercICI.Text = String.Empty
            '            Else
            '                txtPercICI.Text = myDsResult.Tables(0).Rows(x)("Percentuale_proprieta").ToString
            '            End If
            '            ddlTitPossesso.SelectedItem.Selected = False
            '            ddlTitPossesso.Items.FindByValue(myDsResult.Tables(0).Rows(x)("cod_tipo_proprieta").ToString).Selected = True
            '        Next
            '    End If
            'End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeImmobile.ControlsBindFromTer.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="idProgressivoImmobile"></param>
    Private Sub LoadImmobile(ByVal idProgressivoImmobile As Long)
        Dim workTable() As objUIICIAccert
        If Not Session("DataTableImmobiliDaAccertare") Is Nothing Then
            workTable = CType(Session("DataTableImmobiliDaAccertare"), objUIICIAccert())
        End If

        Try
            For Each myUI As objUIICIAccert In workTable
                If myUI.Progressivo = idProgressivoImmobile Then
                    Dim iCount As Integer
                    txtDal.Text = myUI.Dal
                    If Year(myUI.Al) <> 9999 Then
                        txtAl.Text = myUI.Al
                    Else
                        txtAl.Text = ""
                    End If
                    If myUI.Foglio = "ND" Then
                        txtFoglio.Text = ""
                    Else
                        txtFoglio.Text = myUI.Foglio
                    End If
                    If myUI.Numero = "ND" Then
                        txtNumero.Text = ""
                    Else
                        txtNumero.Text = myUI.Numero
                    End If
                    If IsDBNull(myUI.Subalterno) Then
                        txtSubalterno.Text = ""
                    ElseIf myUI.Subalterno = "-1" Or myUI.Subalterno = "" Then
                        txtSubalterno.Text = ""
                    Else
                        txtSubalterno.Text = myUI.Subalterno
                    End If
                    If Not IsDBNull(myUI.Categoria) Then
                        For iCount = 0 To ddlCategoria.Items.Count - 1
                            If ddlCategoria.Items(iCount).Value = myUI.Categoria Then
                                ddlCategoria.SelectedIndex = iCount
                                Exit For
                            End If
                        Next
                    End If
                    If Not IsDBNull(myUI.Classe) Then
                        For iCount = 0 To ddlClasse.Items.Count - 1
                            If ddlClasse.Items(iCount).Value = myUI.Classe Then
                                ddlClasse.SelectedIndex = iCount
                                Exit For
                            End If
                        Next
                    End If
                    txtConsistenza.Text = myUI.Consistenza
                    Dim iCodRendita As String = getIDrendita(myUI.TipoRendita)
                    If Not IsDBNull(iCodRendita) Then
                        For iCount = 0 To ddlTR.Items.Count - 1
                            If ddlTR.Items(iCount).Value = iCodRendita Then
                                ddlTR.SelectedIndex = iCount
                                Exit For
                            End If
                        Next
                    End If
                    txtRendita.Text = myUI.Rendita
                    txtValore.Text = myUI.Valore
                    txtPercICI.Text = myUI.PercPossesso
                    For iCount = 0 To ddlTitPossesso.Items.Count - 1
                        If ddlTitPossesso.Items(iCount).Value = myUI.IdTipoUtilizzo Then
                            ddlTitPossesso.SelectedIndex = iCount
                            Exit For
                        End If
                    Next
                    PopolaDDlEstimo(ddlTR.SelectedValue)
                    If Not IsDBNull(myUI.Zona) Then
                        For iCount = 0 To ddlZona.Items.Count - 1
                            If ddlZona.Items(iCount).Value = myUI.Zona Then
                                ddlZona.SelectedIndex = iCount
                                Exit For
                            End If
                        Next
                    End If
                    If myUI.FlagPrincipale = 1 Then
                        chkPrinc.Checked = myUI.FlagPrincipale
                    End If
                    If myUI.FlagPrincipale = 2 Then
                        chkPert.Checked = True
                    End If
                    If myUI.FlagRiduzione = False Then
                        chkRidotto.Checked = True
                    End If
                    txtMesiEsclusioneEsezione.Text = myUI.MesiEsenzione
                    PopolaDDlEsente(myUI.FlagEsente)
                    Session("IdImmobileDiPertinenza") = myUI.IdImmobilePertinenza
                    txtSezione.Text = myUI.Sezione
                    txtNumUtiliz.Text = myUI.NUtilizzatori

                    txtUbicazione.Text = myUI.Via
                    txtCodVia.Text = myUI.IdVia
                    txtCivico.Text = myUI.NCivico
                    txtEspCivico.Text = myUI.Esponente
                    txtScala.Text = myUI.Scala
                    txtInterno.Text = myUI.Interno
                    txtPiano.Text = myUI.Piano
                    txtBarrato.Text = myUI.Barrato

                    If myUI.TipoTasi = Utility.Costanti.TIPOTASI_INQUILINO Then
                        chkInqu.Checked = True
                    Else
                        chkProp.Checked = True
                    End If
                End If
            Next
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeImmobile.LoadImmobile.errore:   ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myCalc"></param>
    ''' <param name="myAcc"></param>
    ''' <param name="MaxProg"></param>
    ''' <returns></returns>
    Private Function CastFormToAcc(myCalc As objSituazioneFinale, myAcc As objUIICIAccert, MaxProg As Integer) As objUIICIAccert
        Try
            If myCalc.IdLegame <= 0 And myAcc.Progressivo <= 0 Then
                myAcc.Progressivo = MaxProg
            End If
            If myCalc.IdLegame <= 0 And myAcc.IdLegame <= 0 Then
                myAcc.IdLegame = MaxProg
            End If
            myAcc.Dal = myCalc.Dal
            myAcc.Al = myCalc.Al
            myAcc.Foglio = myCalc.Foglio
            myAcc.Numero = myCalc.Numero
            myAcc.Subalterno = myCalc.Subalterno
            myAcc.Categoria = myCalc.Categoria
            myAcc.Classe = myCalc.Classe
            myAcc.Consistenza = myCalc.Consistenza
            myAcc.TipoRendita = myCalc.TipoRendita
            myAcc.Rendita = myCalc.Rendita
            myAcc.Valore = myCalc.Valore
            myAcc.Zona = myCalc.Zona
            myAcc.PercPossesso = myCalc.PercPossesso
            myAcc.IdTipoUtilizzo = myCalc.IdTipoUtilizzo
            myAcc.TitPossesso = myCalc.TitPossesso
            myAcc.FlagPrincipale = myCalc.FlagPrincipale
            myAcc.IdImmobilePertinenza = myCalc.IdImmobilePertinenza
            myAcc.FlagRiduzione = myCalc.FlagRiduzione
            myAcc.MesiRiduzione = myCalc.MesiRiduzione
            myAcc.Sezione = myCalc.Sezione
            myAcc.NUtilizzatori = myCalc.NUtilizzatori
            myAcc.Via = myCalc.Via
            myAcc.IdVia = myCalc.IdVia
            myAcc.NCivico = myCalc.NCivico
            myAcc.Esponente = myCalc.Esponente
            myAcc.Scala = myCalc.Scala
            myAcc.Interno = myCalc.Interno
            myAcc.Piano = myCalc.Piano
            myAcc.Barrato = myCalc.Barrato
            myAcc.MesiEsenzione = myCalc.MesiEsenzione
            myAcc.FlagEsente = myCalc.FlagEsente
            myAcc.IsColtivatoreDiretto = myCalc.IsColtivatoreDiretto
            myAcc.NumeroFigli = myCalc.NumeroFigli
            myAcc.PercentCaricoFigli = myCalc.PercentCaricoFigli
            myAcc.TipoTasi = myCalc.TipoTasi
            myAcc.DescrTipoTasi = myCalc.DescrTipoTasi
            myAcc.AccSenzaDetrazione = myCalc.AccSenzaDetrazione
            myAcc.AccDetrazioneApplicata = myCalc.AccDetrazioneApplicata
            myAcc.AccDovuto = myCalc.AccDovuto
            myAcc.AccDetrazioneResidua = myCalc.AccDetrazioneResidua
            myAcc.SalSenzaDetrazione = myCalc.SalSenzaDetrazione
            myAcc.SalDetrazioneApplicata = myCalc.SalDetrazioneApplicata
            myAcc.SalDovuto = myCalc.SalDovuto
            myAcc.SalDetrazioneResidua = myCalc.SalDetrazioneResidua
            myAcc.TotSenzaDetrazione = myCalc.TotSenzaDetrazione
            myAcc.TotDetrazioneApplicata = myCalc.TotDetrazioneApplicata
            myAcc.TotDovuto = myCalc.TotDovuto
            myAcc.TotDetrazioneResidua = myCalc.TotDetrazioneResidua
            myAcc.IdAliquota = myCalc.IdAliquota
            myAcc.Aliquota = myCalc.Aliquota
            myAcc.AliquotaStatale = myCalc.AliquotaStatale
            myAcc.AccDovutoStatale = myCalc.AccDovutoStatale
            myAcc.AccDetrazioneApplicataStatale = myCalc.AccDetrazioneApplicataStatale
            myAcc.AccDetrazioneResiduaStatale = myCalc.AccDetrazioneResiduaStatale
            myAcc.SalDovutoStatale = myCalc.SalDovutoStatale
            myAcc.SalDetrazioneApplicataStatale = myCalc.SalDetrazioneApplicataStatale
            myAcc.SalDetrazioneResiduaStatale = myCalc.SalDetrazioneResiduaStatale
            myAcc.TotDovutoStatale = myCalc.TotDovutoStatale
            myAcc.TotDetrazioneApplicataStatale = myCalc.TotDetrazioneApplicataStatale
            myAcc.TotDetrazioneResiduaStatale = myCalc.TotDetrazioneResiduaStatale
            myAcc.DiffImposta = myCalc.DiffImposta
            myAcc.Totale = myCalc.Totale
            Return myAcc
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strRendita"></param>
    ''' <returns></returns>
    Private Function getIDrendita(ByVal strRendita As String) As String
        getIDrendita = ""
        Try
            Select Case strRendita.ToUpper
                Case "RE"
                    getIDrendita = "1"
                Case "RP"
                    getIDrendita = "2"
                Case "RPM"
                    getIDrendita = "3"
                Case "LC"
                    getIDrendita = "4"
                Case "AF"
                    getIDrendita = "5"
                Case "TA"
                    getIDrendita = "6"
                Case Else
                    getIDrendita = "0"
            End Select
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeImmobile.getIDrendita.errore ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Function

    'Private Function maxLegame(ByVal campo As String) As Integer
    '    Dim workTable As DataTable
    '    Try
    '        If Not Session("DataTableImmobiliDaAccertare") Is Nothing Then
    '            workTable = CType(Session("DataTableImmobiliDaAccertare"), DataTable)
    '        End If
    '        Dim i As Integer = 0
    '        If workTable.DefaultView.Count = 0 Then
    '            maxLegame = 0
    '        Else
    '            Dim dw As DataView
    '            dw = workTable.DefaultView
    '            dw.Sort = "idLegame desc"
    '            maxLegame = dw.Item(0).Item("idLegame")
    '        End If

    '        Return maxLegame
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeImmobile.maxLegame.errore ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Function

    'Private Function maxProgressivo(ByVal campo As String) As Integer
    '    Dim maxProgressivo1, maxProgressivo2 As Integer
    '    Dim workTable As DataTable
    '    Try
    '        If Not Session("DataTableImmobiliDaAccertare") Is Nothing Then
    '            workTable = CType(Session("DataTableImmobiliDaAccertare"), DataTable)
    '        End If
    '        Dim i As Integer = 0
    '        If workTable.DefaultView.Count = 0 Then
    '            maxProgressivo = 0
    '        Else
    '            Dim dw As DataView
    '            dw = workTable.DefaultView
    '            dw.Sort = "idLegame desc"
    '            maxProgressivo1 = dw.Item(0).Item("PROGRESSIVO")

    '            dw.Sort = "PROGRESSIVO desc"
    '            maxProgressivo2 = dw.Item(0).Item("PROGRESSIVO")

    '            If maxProgressivo1 > maxProgressivo2 Then
    '                maxProgressivo = maxProgressivo1
    '            Else
    '                maxProgressivo = maxProgressivo2
    '            End If

    '        End If

    '        Return maxProgressivo
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeImmobile.maxProgressivo.errore ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="TipoRendita"></param>
    Private Sub PopolaDDlEstimo(ByVal TipoRendita As String)
        Dim TabEstimo As DataTable
        Dim i As Integer
        Dim myListItem As ListItem
        Try
            myListItem = New ListItem

            'If (TipoRendita.CompareTo("AF") = 0) Then
            If (TipoRendita.CompareTo("5") = 0) Then
                TabEstimo = New DichiarazioniICI.Database.TariffeEstimoAFTable(ConstSession.UserName).ListDistinct(ConstSession.IdEnte)
                'TabEstimo = CType(Session("TABELLA_ESTIMO_CATASTALE_FAB"), DataTable)
            Else
                'TabEstimo = CType(Session("TABELLA_ESTIMO_CATASTALE"), DataTable)
                TabEstimo = New DichiarazioniICI.Database.TariffeEstimoTable(ConstSession.UserName).ListDistinct(ConstSession.IdEnte)
            End If

            ddlZona.Items.Clear()
            myListItem.Text = "..."
            myListItem.Value = "0"
            ddlZona.Items.Add(myListItem)

            For i = 0 To TabEstimo.Rows.Count - 1
                myListItem = New ListItem

                If TabEstimo.Rows(i).Item(1) <> "" Then
                    myListItem.Text = TabEstimo.Rows(i).Item(0) & " - " & TabEstimo.Rows(i).Item(1)
                Else
                    myListItem.Text = TabEstimo.Rows(i).Item(0)
                End If
                myListItem.Value = TabEstimo.Rows(i).Item(0)
                ddlZona.Items.Add(myListItem)
            Next
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeImmobile.PopolaDDIEstimo.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Flag_esente"></param>
    Private Sub PopolaDDlEsente(ByVal Flag_esente As Integer)
        Dim myListItem As ListItem
        Try
            ddlEsente.Items.Clear()
            If Flag_esente < 0 Then
                Flag_esente = 1
            End If
            'valori invertiti perchè in dichiarato dati possesso
            '0=SI
            '1=NO
            '2=non compilato

            myListItem = New ListItem
            myListItem.Text = "SI"
            myListItem.Value = "0"
            ddlEsente.Items.Add(myListItem)

            myListItem = New ListItem
            myListItem.Text = "NO"
            myListItem.Value = "1"
            ddlEsente.Items.Add(myListItem)

            myListItem = New ListItem
            myListItem.Text = "NON COMPILATO"
            myListItem.Value = "2"
            ddlEsente.Items.Add(myListItem)

            ddlEsente.SelectedValue = Flag_esente
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeImmobile.PopolaDDIEsente.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub loadCombo()
        Dim objHashTable As New Hashtable
        Dim Utility As New MyUtility
        'Dim strConnectionStringOPENgovICI As String
        'Dim objSessione As CreateSessione

        Try
            objHashTable.Add("IDSOTTOAPPLICAZIONEICI", ConfigurationManager.AppSettings("OPENGOVI"))
            'strConnectionStringOPENgovICI = objSessione.oSession.GetPrivateDBManager(objHashTable("IDSOTTOAPPLICAZIONEICI")).GetConnection.ConnectionString
            objHashTable.Add("CONNECTIONSTRINGOPENGOVICI", ConstSession.StringConnectionICI)

            Dim objGestioneConfigurazione As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)

            ddlTR.Items.Clear()
            Utility.FillDropDownSQLValueString(ddlTR, objGestioneConfigurazione.GetListTipoRendita(ConstSession.StringConnectionICI, objHashTable), -1, "...")

            ddlTitPossesso.Items.Clear()
            Utility.FillDropDownSQLValueString(ddlTitPossesso, objGestioneConfigurazione.GetListTipoPossesso(ConstSession.StringConnectionICI, objHashTable), -1, "...")

            ddlCategoria.Items.Clear()
            Utility.FillDropDownSQLSingleString(ddlCategoria, objGestioneConfigurazione.GetListCategorie(ConstSession.StringConnectionICI, objHashTable), -1, "...")

            ddlClasse.Items.Clear()
            Utility.FillDropDownSQLSingleString(ddlClasse, objGestioneConfigurazione.GetListClasse(ConstSession.StringConnectionICI, objHashTable), -1, "...")

            PopolaDDlEsente(1)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeImmobile.laodCombo.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub loadComboTitPossesso()
        Dim myListItem As ListItem
        Try
            myListItem = New ListItem
            myListItem.Text = "..."
            myListItem.Value = ""
            ddlTitPossesso.Items.Add(myListItem)

            myListItem = New ListItem
            myListItem.Text = "Proprietario"
            myListItem.Value = "0"
            ddlTitPossesso.Items.Add(myListItem)

            myListItem = New ListItem
            myListItem.Text = "Uso Gratuito ai familiari"
            myListItem.Value = "1"
            ddlTitPossesso.Items.Add(myListItem)

            myListItem = New ListItem
            myListItem.Text = "Diritto di Abitazione"
            myListItem.Value = "2"
            ddlTitPossesso.Items.Add(myListItem)

            myListItem = New ListItem
            myListItem.Text = "Usufruttuario"
            myListItem.Value = "3"
            ddlTitPossesso.Items.Add(myListItem)

            myListItem = New ListItem
            myListItem.Text = "Esente"
            myListItem.Value = "4"
            ddlTitPossesso.Items.Add(myListItem)

            myListItem = New ListItem
            myListItem.Text = "A disposizione"
            myListItem.Value = "5"
            ddlTitPossesso.Items.Add(myListItem)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeImmobile.laodComboTitPossesso.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub chkPert_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPert.CheckedChanged
        dim sScript as string=""
        Try
            If chkPert.Checked = True Then

                If chkPrinc.Checked = True Then
                    sscript+= "alertPertinenza();" & vbCrLf
                    chkPert.Checked = False
                Else
                    sscript+= "cercaImmobilePertinenza();" & vbCrLf
                End If

                'sscript+= "cercaImmobilePertinenza();" & vbCrLf
                RegisterScript(sScript , Me.GetType())
            Else
                Session.Remove("IdImmobileDiPertinenza")
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeImmobile.chkPert_CheckedChanged.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnPulisci_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPulisci.Click
        txtDal.Text = ""
        txtAl.Text = ""
        txtSezione.Text = ""
        txtFoglio.Text = ""
        txtNumero.Text = ""
        txtSubalterno.Text = ""
        'txtCategoria.Text = ""
        'txtClasse.Text = ""
        ddlCategoria.SelectedIndex = 0
        ddlClasse.SelectedIndex = 0
        txtConsistenza.Text = ""
        ddlTR.SelectedValue = 0
        txtRendita.Text = ""
        txtValore.Text = ""
        txtPercICI.Text = ""
        ddlTitPossesso.SelectedIndex = 0
        txtCodVia.Text = ""
        txtUbicazione.Text = ""

        chkPrinc.Checked = False
        chkPert.Checked = False
        chkRidotto.Checked = False
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnResetPertinenza_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResetPertinenza.Click
        chkPert.Checked = False
    End Sub

    ' 
    ''' <summary>
    ''' Serve per il calcolo della rendita immobile.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub lnkRendita_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkRendita.Click
        Dim sScript As String = ""
        Dim TabTariffa As DataTable
        Dim AnnoAccertamento As String
        AnnoAccertamento = hfAnno.Value
        ' controllo il tipo di immobile
        Try
            If (ddlTR.SelectedItem.Text.CompareTo("AF - Aree edificabili") = 0) Then
                sScript += "GestAlert('a', 'warning', '', '', 'Le aree edificabili prevedono solo il calcolo del valore.');"
                RegisterScript(sScript, Me.GetType())
                txtRendita.Text = "0"

                ''devo vedere se c'è qualche tariffa configurata per la zona selezionata

                'TabTariffa = New DichiarazioniICI.DatabaseOpengov.TariffeEstimoAFTable(ConstSession.UserName).SelectTariffa(constsession.idente, ddlZona.SelectedValue, DateTime.Parse(txtDal.Text))

                'If (TabTariffa.Rows.Count < 1) Then

                '    'se nn ci sono tariffe configurate


                '    strBuild = "<script language=""javascript"" type=""text/javascript"">"
                '    strBuild += "alert('Attenzione, non ci sono tariffe configurate per la zona selezionata!')"
                '    strBuild += "</script>"

                '    RegisterScript(sScript , Me.GetType())

                'Else
                '    Dim Tariffa As Decimal = Decimal.Parse(TabTariffa.Rows(0)("TARIFFA_EURO").ToString())
                '    'calcolo la rendita con i dati che possiedo
                '    Dim Rendita As Decimal = 0
                '    Dim Consistenza As Decimal = Decimal.Parse(txtConsistenza.Text)

                '    Rendita = Consistenza * Tariffa

                '    txtRendita.Text = Rendita.ToString("N")

                'End If
            ElseIf (ddlTR.SelectedItem.Text.CompareTo("LC - Libri contabili") = 0) Then
                sScript += "GestAlert('a', 'warning', '', '', 'Gli immobili con codice rendita LC prevedono solo il calcolo del valore.');"
                RegisterScript(sScript, Me.GetType())
                txtRendita.Text = "0"
            ElseIf (ddlTR.SelectedItem.Text.CompareTo("TA - Terreni agricoli") = 0) Then
                sScript += "GestAlert('a', 'warning', '', '', 'Inserire manualmente il Reddito Domenicale nel campo rendita.');"
                RegisterScript(sScript, Me.GetType())
            Else
                'devo vedere se c'è qualche tariffa configurata per la zona, la classe e la categoria selezionata
                'TabTariffa = New DichiarazioniICI.DatabaseOpengov.TariffeEstimoTable(ConstSession.UserName).SelectTariffa(constsession.idente, ddlZona.SelectedValue, ddlCategoria.SelectedValue, ddlClasse.SelectedValue, DateTime.Parse(txtDal.Text))
                TabTariffa = New DichiarazioniICI.Database.TariffeEstimoTable(ConstSession.UserName).SelectTariffa(ConstSession.IdEnte, ddlZona.SelectedValue, ddlCategoria.SelectedValue, ddlClasse.SelectedValue, DateTime.Parse("01/01/" & AnnoAccertamento))

                If (TabTariffa.Rows.Count < 1) Then
                    'se nn ci sono tariffe configurate
                    sScript += "GestAlert('a', 'warning', '', '', 'Attenzione, non ci sono tariffe configurate per la zona selezionata!');"
                    RegisterScript(sScript, Me.GetType())
                Else
                    Dim Tariffa As Decimal = Decimal.Parse(TabTariffa.Rows(0)("TARIFFA_EURO").ToString())
                    'calcolo la rendita con i dati che possiedo
                    Dim Rendita As Decimal = 0
                    Dim Consistenza As Decimal = Decimal.Parse(txtConsistenza.Text)
                    Rendita = Consistenza * Tariffa
                    txtRendita.Text = Rendita.ToString("N")
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeImmobile.InkRendita_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub lnkValore_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkValore.Click
        Dim sScript As String = ""
        'Dim Rendita As Decimal
        'Dim Categoria As String
        'Dim ValoreImmobile As Decimal = 0
        'Dim Rivalutazione As Decimal
        'Dim TabTariffa As DataTable
        'Dim strBuild As String
        'Dim AnnoAccertamento As String
        'AnnoAccertamento = txtAnnoAccertamento.Text
        Try
            'If (ddlTR.SelectedItem.Text.CompareTo("AF - Aree edificabili") = 0 Or ddlTR.SelectedItem.Text.CompareTo("LC - Libri contabili") = 0) Then
            '    'devo vedere se c'è qualche tariffa configurata per la zona selezionata
            '    TabTariffa = New DichiarazioniICI.DatabaseOpengov.TariffeEstimoAFTable(ConstSession.UserName).SelectTariffa(constsession.idente, ddlZona.SelectedValue, DateTime.Parse("01/01/" & AnnoAccertamento))
            '    'TabTariffa = New DichiarazioniICI.DatabaseOpengov.TariffeEstimoAFTable(ConstSession.UserName).SelectTariffa(constsession.idente, ddlZona.SelectedValue, DateTime.Parse(txtDal.Text))

            '    If (TabTariffa.Rows.Count < 1) Then
            '        'se nn ci sono tariffe configurate
            '        strBuild = "<script language=""javascript"" type=""text/javascript"">"
            '        strBuild += "alert('Attenzione, non ci sono tariffe configurate per la zona selezionata!')"
            '        strBuild += "</script>"
            '        RegisterScript(sScript , Me.GetType())
            '        Exit Sub
            '    Else
            '        Dim Tariffa As Decimal = Decimal.Parse(TabTariffa.Rows(0)("TARIFFA_EURO").ToString())
            '        'calcolo la rendita con i dati che possiedo
            '        Dim valore As Decimal = 0
            '        Dim Consistenza As Decimal = Decimal.Parse(txtConsistenza.Text)
            '        valore = Consistenza * Tariffa
            '        txtValore.Text = valore.ToString("N")
            '        Exit Sub
            '    End If
            'End If

            'Rendita = Decimal.Parse(txtRendita.Text)
            'Categoria = ddlCategoria.SelectedItem.Text.ToString
            '' verifico se mi trovo a calcolare TA 
            'If (ddlTR.SelectedItem.Text = "TA") Then
            '    ValoreImmobile = Decimal.Parse(txtRendita.Text) * 75
            '    If (DateTime.Parse(txtDal.Text) >= DateTime.Parse("01/01/1997")) Then
            '        'If (AnnoAccertamento >= 1997) Then
            '        Rivalutazione = ValoreImmobile * 25 / 100
            '        ValoreImmobile = ValoreImmobile + Rivalutazione
            '    End If
            '    txtValore.Text = ValoreImmobile.ToString("N")
            '    Exit Sub
            'End If
            ''se sono in presenza di categoria a/10 o c/1
            'If (Categoria.CompareTo("A/10") = 0) Then
            '    ValoreImmobile = Rendita * 50
            '    'calcolo la rivalutazione se immobile > 1997
            '    If (DateTime.Parse(txtDal.Text) >= DateTime.Parse("01/01/1997")) Then
            '        'If (AnnoAccertamento >= 1997) Then
            '        Rivalutazione = ValoreImmobile * 5 / 100
            '        ValoreImmobile = ValoreImmobile + Rivalutazione
            '    End If
            '    txtValore.Text = ValoreImmobile.ToString("N")
            '    Exit Sub
            'ElseIf (Categoria.CompareTo("C/1") = 0) Then
            '    ValoreImmobile = Rendita * 34
            '    If (DateTime.Parse(txtDal.Text) >= DateTime.Parse("01/01/1997")) Then
            '        'If (AnnoAccertamento >= 1997) Then
            '        Rivalutazione = ValoreImmobile * 5 / 100
            '        ValoreImmobile = ValoreImmobile + Rivalutazione
            '    End If
            '    txtValore.Text = ValoreImmobile.ToString("N")
            '    Exit Sub
            'End If

            'Dim GruppoCategoria As String = Categoria.Substring(0, 1)
            'If (GruppoCategoria.CompareTo("A") = 0 Or GruppoCategoria.CompareTo("C") = 0) Then
            '    ValoreImmobile = Rendita * 100
            'End If
            'If (GruppoCategoria.CompareTo("D") = 0) Then
            '    ValoreImmobile = Rendita * 50
            'ElseIf (GruppoCategoria.CompareTo("B") = 0) Then
            '    If (CInt(AnnoAccertamento) = 2006) Then
            '        Dim RenditaMensile, valorePreOtt, valorePostOtt As Double
            '        RenditaMensile = Rendita / 12
            '        valorePreOtt = RenditaMensile * 9 * 100
            '        valorePostOtt = RenditaMensile * 3 * 140
            '        ValoreImmobile = valorePreOtt + valorePostOtt
            '    ElseIf (CInt(AnnoAccertamento) < 2006) Then
            '        ValoreImmobile = Rendita * 100
            '    Else
            '        ValoreImmobile = Rendita * 140
            '    End If
            'End If
            'If (DateTime.Parse(txtDal.Text) >= DateTime.Parse("01/01/1997")) Then
            '    'If (AnnoAccertamento >= 1997) Then
            '    Rivalutazione = ValoreImmobile * 5 / 100
            '    ValoreImmobile = ValoreImmobile + Rivalutazione
            'End If
            'txtValore.Text = ValoreImmobile.ToString("N")
            '*** 20120709 - IMU per AF e LC devo usare il campo valore ***
            Dim ValoreDich As Double = 0
            If txtValore.Text <> "" Then
                ValoreDich = txtValore.Text
            End If
            '*** 20120530 - IMU ***
            Dim FncValore As New ComPlusInterface.FncICI
            txtValore.Text = FncValore.CalcoloValore(ConstSession.DBType, ConstSession.StringConnectionOPENgov, ConstSession.StringConnectionICI, ConstSession.IdEnte, Year(txtDal.Text), ddlTR.SelectedItem.Text.Substring(0, ddlTR.SelectedItem.Text.IndexOf("-")).Trim, ddlCategoria.SelectedItem.Text, ddlClasse.SelectedValue, ddlZona.SelectedValue, txtRendita.Text, ValoreDich, txtConsistenza.Text, CDate(txtDal.Text), chkcoltivatore.Checked)
            If txtValore.Text <= 0 Then
                sScript += "GestAlert('a', 'warning', '', '', 'Attenzione! Non ci sono tariffe configurate per la zona selezionata!');"
                RegisterScript(sScript, Me.GetType())
            End If
            '*** ***
            '*** ***
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeImmobile.InkValore_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ddlTR_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTR.SelectedIndexChanged

        PopolaDDlEstimo(ddlTR.SelectedValue)

    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub txtnumfigli_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtnumfigli.TextChanged
        Try
            Dim nFigli As Integer = 0
            nFigli = Business.CoreUtility.ConvertiNumero(txtnumfigli.Text)
            If (nFigli > 6) Then
                nFigli = 6
            End If
            If (nFigli > 0) Then
                Dim dtCaricoFigli As DataTable = New DataTable("CaricoFigli")
                dtCaricoFigli.Columns.Add("nFiglio")
                dtCaricoFigli.Columns.Add("percentuale")
                Dim ListPercCarico() As Object = New Object((2) - 1) {}
                ListPercCarico.Initialize()
                Dim x As Integer = 1
                Do While (x <= nFigli)
                    ListPercCarico(0) = ("Figlio n." + x.ToString)
                    ListPercCarico(1) = ""
                    dtCaricoFigli.Rows.Add(ListPercCarico)
                    x = (x + 1)
                Loop
                GrdCaricoFigli.DataSource = dtCaricoFigli
                GrdCaricoFigli.DataBind()
            Else
                GrdCaricoFigli.Visible = False
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.InserimentoManualeImmobile.txtnumfigli_TextChanged.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
