Imports ComPlusInterface
Imports log4net
Imports DichiarazioniICI.CalcoloICI
''' <summary>
''' Pagina per la generazione dei provvedimenti IMU.
''' Contiene le funzioni della comandiera e la griglia per la gestione del dichiarato. 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class SearchDatiAccertamenti
    Inherits BasePage
    Protected FncGrd As New Formatta.FunctionGrd

   
    'Private idCelle As New DataGridIndex
    Private Progressivo As Integer = 0
    Private FncGen As New ClsGestioneAccertamenti

    Private workTable As New DataTable("IMMOBILI")
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(SearchDatiAccertamenti))
    Private TipoCalcolo As Integer = DichiarazioniICI.CalcoloICI.CalcoloICI.TIPOCalcolo_STANDARD

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
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim ListSituazioneFinale() As objSituazioneFinale
        Dim ListDich() As objUIICIAccert
        Dim objHashTable As Hashtable
        Dim sScript As String

        Try
            'Setto la variabile che mi dice che ho effettuato ricerca in dichiarato
            'Session("cercaDichiarato") = True
            Log.Debug("SearchDatiDichiarato::entro")
            ListSituazioneFinale = Nothing
            'Viene Salvata da GestioneAccertamenti.aspx.vb
            'pere avere il codice contribuente e anno di accertamento
            objHashTable = Session("HashTableDichiarazioniAccertamenti")

            If objHashTable.Contains("TIPO_OPERAZIONE") = False Then
                objHashTable.Add("TIPO_OPERAZIONE", oggettoatto.procedimento.ACCERTAMENTO)
            Else
                objHashTable("TIPO_OPERAZIONE") = oggettoatto.procedimento.ACCERTAMENTO
            End If

            If Not Page.IsPostBack Then
                setHashTable(objHashTable)
            End If
            '*** 20140509 - TASI ***
            If objHashTable.Contains("URLServiziFreezer") = False Then
                objHashTable.Add("URLServiziFreezer", ConstSession.URLServiziFreezer)
            Else
                objHashTable("URLServiziFreezer") = ConstSession.URLServiziFreezer
            End If
            If objHashTable.Contains("CONFIGURAZIONE_DICHIARAZIONE") = False Then
                objHashTable.Add("CONFIGURAZIONE_DICHIARAZIONE", ConstSession.CONFIGURAZIONE_DICHIARAZIONE)
            Else
                objHashTable("CONFIGURAZIONE_DICHIARAZIONE") = ConstSession.CONFIGURAZIONE_DICHIARAZIONE
            End If
            If objHashTable.Contains("TRIBUTOCALCOLO") = False Then
                objHashTable.Add("TRIBUTOCALCOLO", Utility.Costanti.TRIBUTO_ICI)
            Else
                objHashTable("TRIBUTOCALCOLO") = Utility.Costanti.TRIBUTO_ICI
            End If
            If objHashTable.Contains("ANNODA") = False Then
                objHashTable.Add("ANNODA", objHashTable("ANNOACCERTAMENTO"))
            Else
                objHashTable("ANNODA") = objHashTable("ANNOACCERTAMENTO")
            End If
            If objHashTable.Contains("ANNOA") = False Then
                objHashTable.Add("ANNOA", -1)
            Else
                objHashTable("ANNOA") = -1
            End If
            If objHashTable.Contains("CONNECTIONSTRINGOPENGOV") = False Then
                objHashTable.Add("CONNECTIONSTRINGOPENGOV", ConstSession.StringConnectionOPENgov)
            Else
                objHashTable("CONNECTIONSTRINGOPENGOV") = ConstSession.StringConnectionOPENgov
            End If
            '*** ***
            '******************************************************************
            'Cerco tutte le dichiarazioni per l'anno selezionato
            '******************************************************************
            Log.Debug("SearchDatiAccertamenti::valorizzato objhastable")
            'Salvo il data set delle dichiarazioni
            'Rifacciamo la query anche se è da rivedere (trovare soluzione
            'ottimale per evitare di rifare ogni volta la query)

            If Page.IsPostBack = False Then
                'Dipe Spostata ricerca internamente
                Log.Debug("SearchDatiAccertamenti::devo richiamare getdatiaccertamenti")
                Dim objCOMDichiarazioniAccertamenti As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
                ListDich = objCOMDichiarazioniAccertamenti.GetDatiAccertamenti(ConstSession.StringConnectionICI, ConstSession.StringConnectionOPENgov, ConstSession.IdEnte, Utility.StringOperation.FormatInt(objHashTable("CODCONTRIBUENTE")), objHashTable, ListSituazioneFinale)
                Log.Debug("SearchDatiAccertamenti::richiamato getdatiaccertamenti")
                If Not ListDich Is Nothing Then
                    If ListDich.GetLength(0) > 0 Then
                        'Svuota la Session con la dichiarazione precedente
                        Session.Remove("DataSetDichiarazioni")
                        Session.Remove("DATASET_CALCOLO_ICI_IMMOBILI_DICHIARATI")
                        Session.Add("DATASET_CALCOLO_ICI_IMMOBILI_DICHIARATI", ListSituazioneFinale)
                        GrdDichiarato.DataSource = ListDich
                        GrdDichiarato.DataBind()
                    End If
                    chkSelTutti.Visible = True
                    lblTesto.Visible = True
                End If
                Session("DataSetDichiarazioni") = ListDich
            End If

            Select Case CInt(GrdDichiarato.Rows.Count)
                Case 0
                    GrdDichiarato.Visible = False
                    lblMessage.Text = "Non sono state trovate Dichiarazioni"
                Case Is > 0
                    If Session("ESCLUDI_PREACCERTAMENTO") Then
                        lblMessage.Text = "Gli immobili dichiarati non verranno presi in considerazione per il calcolo di questo accertamento in quanto già calcolati in altro accertamento definitivo"
                        lblTesto.Visible = False
                        chkSelTutti.Visible = False
                        sScript = "parent.parent.Comandi.document.getElementById('btnRibaltaImmobile').style.display='none';"
                        RegisterScript(sScript, Me.GetType)
                    End If
                    GrdDichiarato.Visible = True
            End Select
            sScript = "parent.document.getElementById('loadGridDichiarato').style.display='' ;"
            sScript += "parent.document.getElementById('attesaCarica').style.display='none';"
            RegisterScript(sScript, Me.GetType)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamenti.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Response.Write("<script language='javascript'>")
            Response.Write("</script>")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnRibaltaImmobiliAcc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRibaltaImmobiliAcc.Click
        Try
            Dim ListUI() As objUIICIAccert
            Dim ListAcc As New ArrayList()
            Dim nProg As Integer = 0

            ListUI = Session("DataSetDichiarazioni")
            For Each myRow As GridViewRow In GrdDichiarato.Rows
                If CType(myRow.FindControl("chkRibaltaIm"), CheckBox).Checked = True Then
                    For Each myUI As objUIICIAccert In ListUI
                        If myUI.Progressivo = myRow.Cells(0).Text Then
                            nProg = PopolaRibaltamento(myUI, nProg, ListAcc)
                            Exit For
                        End If
                    Next
                End If
            Next
            Session("DataTableImmobiliDaAccertare") = CType(ListAcc.ToArray(GetType(objUIICIAccert)), objUIICIAccert())
            Dim sScript As String = ""
            If nProg > 0 Then
                sScript += "parent.document.getElementById('loadGridAccertato').src='grdAccertato.aspx';"
            Else
                sScript += "GestAlert('a', 'warning', '', '', 'Selezionare almeno un immobile per il ribaltamento!')"
            End If
            RegisterScript(sScript, Me.GetType())
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamenti.btnRibalta_Click.errore: ", Err)
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
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                e.Row.Cells(0).BackColor = Color.PaleGoldenrod
                e.Row.Cells(0).Font.Bold = True

                e.Row.Cells(22).ToolTip = "Premere questo pulsante per modificare il legame dell'immobile"
                e.Row.Cells(23).ToolTip = "Premere questo pulsante per eliminare l'immobile"
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamenti.GrdRowDataBound.errore: ", ex)
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
            Select Case e.CommandName
                Case "RowEdit"
                    For Each myRow As GridViewRow In GrdDichiarato.Rows
                        If IDRow = CType(myRow.FindControl("hfprogressivo"), HiddenField).Value Then
                            GrdDichiarato.EditIndex = myRow.RowIndex
                            GrdDichiarato.DataSource = Session("DataSetDichiarazioni")
                            GrdDichiarato.DataBind()
                        End If
                    Next
                Case "RowUpdate"
                    For Each myRow As GridViewRow In GrdDichiarato.Rows
                        If IDRow = CType(myRow.FindControl("hfprogressivo"), HiddenField).Value Then
                            Dim idLegame As TextBox = myRow.FindControl("txtLegame")

                            If idLegame.Text = "" Then
                                Dim sScript As String = ""
                                sScript += "msgLegameVuoto();" & vbCrLf
                                RegisterScript(sScript, Me.GetType())
                            Else
                                Dim Progressivo As String
                                Dim objDS As DataSet
                                objDS = Session("DataSetDichiarazioni")
                                Progressivo = IDRow
                                'salvo Dati
                                objDS.Tables(0).DefaultView.RowFilter = "PROGRESSIVO='" & Progressivo & "'"
                                If objDS.Tables(0).DefaultView.Count > 0 Then
                                    objDS.Tables(0).DefaultView.Item(0).Item("IDLegame") = idLegame.Text
                                    objDS.Tables(0).AcceptChanges()
                                End If
                                objDS.Tables(0).DefaultView.RowFilter = ""

                                GrdDichiarato.EditIndex = -1
                                objDS.Tables(0).DefaultView.Sort = "FOGLIO, NUMERO, SUBALTERNO, IDLEGAME ASC"
                                GrdDichiarato.DataSource = objDS
                                GrdDichiarato.DataBind()
                                Session("DataSetDichiarazioni") = objDS
                            End If
                        End If
                    Next
                Case "RowCancel"
                    GrdDichiarato.EditIndex = -1
                    Dim objDS As DataSet
                    objDS = Session("DataSetDichiarazioni")
                    GrdDichiarato.DataSource = objDS
                    GrdDichiarato.DataBind()
                Case "RowDelete"
                    For Each myRow As GridViewRow In GrdDichiarato.Rows
                        If IDRow = CType(myRow.FindControl("hfprogressivo"), HiddenField).Value Then
                            Dim dt As DataTable
                            Dim row() As DataRow

                            dt = ViewState("DataTableImmobiliDaAccertare")
                            row = dt.Select("PROGRESSIVO='" & IDRow & "'")
                            dt.Rows.Remove(row(0))
                            dt.AcceptChanges()
                            dt.DefaultView.Sort = "FOGLIO, NUMERO, SUBALTERNO, IDLEGAME ASC"
                            ViewState("DataTableImmobiliDaAccertare") = dt

                            'Aggiorno la session con l'immobile in meno prendendolo dal DataTable--->DataSet
                            Session("DataSetDichiarazioni") = dt.DataSet
                            If dt.Rows.Count <= 0 Then
                                GrdDichiarato.Visible = False
                            Else
                                GrdDichiarato.Visible = True
                                GrdDichiarato.DataSource = dt
                                GrdDichiarato.DataBind()
                            End If
                        End If
                    Next
            End Select
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamenti.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
#End Region
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub chkSelTutti_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSelTutti.CheckedChanged

        Try
            If chkSelTutti.Checked = True Then
                '** ciclare la griglia e selezionare tutti gli immobili
                For Each myRow As GridViewRow In GrdDichiarato.Rows
                    CType(myRow.FindControl("chkRibaltaIm"), CheckBox).Checked = True
                Next
            Else
                '** nessuna selezione -> selezione manuale dell'operatore
                For Each myRow As GridViewRow In GrdDichiarato.Rows
                    CType(myRow.FindControl("chkRibaltaIm"), CheckBox).Checked = False
                Next
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamenti.CheckedChanged.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="SelRow"></param>
    ''' <param name="prog"></param>
    ''' <param name="ListUI"></param>
    ''' <returns></returns>
    Private Function PopolaRibaltamento(ByVal SelRow As objUIICIAccert, ByVal prog As Integer, ByRef ListUI As ArrayList) As Integer
        Try
            prog = prog + 1

            Dim myUI As New objUIICIAccert

            myUI.IdLegame = SelRow.IdLegame
            myUI.Progressivo = prog
            myUI.Tributo = SelRow.Tributo
            myUI.IdEnte = SelRow.IdEnte
            myUI.Anno = SelRow.Anno
            myUI.TipoRendita = SelRow.TipoRendita
            myUI.Categoria = SelRow.Categoria
            myUI.Classe = SelRow.Classe
            myUI.Zona = SelRow.Zona
            myUI.TipoTasi = SelRow.TipoTasi
            myUI.DescrTipoTasi = SelRow.DescrTipoTasi
            myUI.Foglio = SelRow.Foglio
            myUI.Numero = SelRow.Numero
            myUI.Subalterno = SelRow.Subalterno
            myUI.Provenienza = SelRow.Provenienza
            myUI.Caratteristica = SelRow.Caratteristica
            myUI.Via = SelRow.Via
            myUI.NCivico = SelRow.NCivico
            myUI.Esponente = SelRow.Esponente
            myUI.Scala = SelRow.Scala
            myUI.Interno = SelRow.Interno
            myUI.Piano = SelRow.Piano
            myUI.Barrato = SelRow.Barrato
            myUI.Sezione = SelRow.Sezione
            myUI.Protocollo = SelRow.Protocollo
            myUI.DataScadenza = SelRow.DataScadenza
            myUI.DataInizio = SelRow.DataInizio
            myUI.TipoOperazione = SelRow.TipoOperazione
            myUI.TitPossesso = SelRow.TitPossesso
            myUI.Id = SelRow.Id
            myUI.IdContribuente = SelRow.IdContribuente
            myUI.IdContribuenteCalcolo = SelRow.IdContribuenteCalcolo
            myUI.IdContribuenteDich = SelRow.IdContribuenteDich
            myUI.IdProcedimento = SelRow.IdProcedimento
            myUI.IdRiferimento = SelRow.IdRiferimento
            myUI.IdVia = SelRow.IdVia
            myUI.NumeroFigli = SelRow.NumeroFigli
            myUI.MesiPossesso = SelRow.MesiPossesso
            myUI.Mesi = SelRow.Mesi
            myUI.IdTipoUtilizzo = SelRow.IdTipoUtilizzo
            myUI.IdTipoPossesso = SelRow.IdTipoPossesso
            myUI.NUtilizzatori = SelRow.NUtilizzatori
            myUI.FlagPrincipale = SelRow.FlagPrincipale
            myUI.FlagRiduzione = SelRow.FlagRiduzione
            myUI.FlagEsente = SelRow.FlagEsente
            myUI.FlagStorico = SelRow.FlagStorico
            myUI.FlagPosseduto = SelRow.FlagPosseduto
            myUI.FlagProvvisorio = SelRow.FlagProvvisorio
            myUI.MesiRiduzione = SelRow.MesiRiduzione
            myUI.MesiEsenzione = SelRow.MesiEsenzione
            myUI.AccMesi = SelRow.AccMesi
            myUI.IdImmobile = SelRow.IdImmobile
            myUI.IdImmobilePertinenza = SelRow.IdImmobilePertinenza
            myUI.IdImmobileDichiarato = SelRow.IdImmobileDichiarato
            myUI.MeseInizio = SelRow.MeseInizio
            myUI.AbitazionePrincipaleAttuale = SelRow.AbitazionePrincipaleAttuale
            myUI.AccSenzaDetrazione = SelRow.AccSenzaDetrazione
            myUI.AccDetrazioneApplicata = SelRow.AccDetrazioneApplicata
            myUI.AccDovuto = SelRow.AccDovuto
            myUI.AccDetrazioneResidua = SelRow.AccDetrazioneResidua
            myUI.SalSenzaDetrazione = SelRow.SalSenzaDetrazione
            myUI.SalDetrazioneApplicata = SelRow.SalDetrazioneApplicata
            myUI.SalDovuto = SelRow.SalDovuto
            myUI.SalDetrazioneResidua = SelRow.SalDetrazioneResidua
            myUI.TotSenzaDetrazione = SelRow.TotSenzaDetrazione
            myUI.TotDetrazioneApplicata = SelRow.TotDetrazioneApplicata
            myUI.TotDovuto = SelRow.TotDovuto
            myUI.TotDetrazioneResidua = SelRow.TotDetrazioneResidua
            myUI.IdAliquota = SelRow.IdAliquota
            myUI.Aliquota = SelRow.Aliquota
            myUI.AliquotaStatale = SelRow.AliquotaStatale
            myUI.PercentCaricoFigli = SelRow.PercentCaricoFigli
            myUI.AccDovutoStatale = SelRow.AccDovutoStatale
            myUI.AccDetrazioneApplicataStatale = SelRow.AccDetrazioneApplicataStatale
            myUI.AccDetrazioneResiduaStatale = SelRow.AccDetrazioneResiduaStatale
            myUI.SalDovutoStatale = SelRow.SalDovutoStatale
            myUI.SalDetrazioneApplicataStatale = SelRow.SalDetrazioneApplicataStatale
            myUI.SalDetrazioneResiduaStatale = SelRow.SalDetrazioneResiduaStatale
            myUI.TotDovutoStatale = SelRow.TotDovutoStatale
            myUI.TotDetrazioneApplicataStatale = SelRow.TotDetrazioneApplicataStatale
            myUI.TotDetrazioneResiduaStatale = SelRow.TotDetrazioneResiduaStatale
            myUI.PercPossesso = SelRow.PercPossesso
            myUI.Rendita = SelRow.Rendita
            myUI.Valore = SelRow.Valore
            myUI.ValoreReale = SelRow.ValoreReale
            myUI.Consistenza = SelRow.Consistenza
            myUI.ImpDetrazione = SelRow.ImpDetrazione
            myUI.DiffImposta = SelRow.DiffImposta
            myUI.Totale = SelRow.Totale
            myUI.IsColtivatoreDiretto = SelRow.IsColtivatoreDiretto
            myUI.Dal = SelRow.Dal
            myUI.Al = SelRow.Al
            myUI.IdSanzioni = SelRow.IdSanzioni
            myUI.DescrSanzioni = SelRow.DescrSanzioni
            myUI.CalcolaInteressi = SelRow.CalcolaInteressi
            myUI.ImpInteressi = SelRow.ImpInteressi
            myUI.ImpSanzioni = SelRow.ImpSanzioni
            myUI.ImpSanzioniRidotto = SelRow.ImpSanzioniRidotto

            Dim myArray As New ArrayList(ListUI)
            myArray.Add(myUI)

            ListUI = myArray
            Return prog
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamenti.PopolaRibaltamento.errore:  ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="objHashTable"></param>
    Private Sub setHashTable(ByRef objHashTable As Hashtable)
        Try
            'Recupero la hash table
            objHashTable.Add("IDSOTTOAPPLICAZIONEICI", ConfigurationManager.AppSettings("OPENGOVI"))

            'Aggiungo gli altri campi nella hash table
            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
            objHashTable.Add("CONNECTIONSTRINGOPENGOVICI", ConstSession.StringConnectionICI)
            objHashTable.Add("USER", ConstSession.UserName)
            objHashTable.Add("CODENTE", ConstSession.IdEnte)
            objHashTable.Add("CodENTE", ConstSession.IdEnte)
            objHashTable.Add("CODTIPOPROCEDIMENTO", "L")
            objHashTable.Add("TASIAPROPRIETARIO", 0)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamenti.setHashTable.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class


'Partial Class SearchDatiAccertamenti
'    Inherits BasePage
'    Protected FncGrd As New Formatta.FunctionGrd

'   
'	Private idCelle As New DataGridIndex
'	Private Progressivo As Integer = 0
'	Private FncGen As New ClsGestioneAccertamenti

'	Private workTable As New DataTable("IMMOBILI")
'	Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(SearchDatiAccertamenti))
'	Private TipoCalcolo As Integer = DichiarazioniICI.CalcoloICI.CalcoloICI.TIPOCalcolo_STANDARD

'#Region " Web Form Designer Generated Code "

'	'This call is required by the Web Form Designer.
'	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

'	End Sub

'	'NOTE: The following placeholder declaration is required by the Web Form Designer.
'	'Do not delete or move it.
'	Private designerPlaceholderDeclaration As System.Object

'	Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
'		'CODEGEN: This method call is required by the Web Form Designer
'		'Do not modify it using the code editor.
'		InitializeComponent()
'	End Sub

'#End Region

'    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
'    '	'Put user code to initialize the page here
'    '	Dim dw As DataView
'    '	Dim objDSDichiarazioni As DataSet
'    '	Dim blnResult As Boolean = False
'    '	Dim objHashTable As Hashtable
'    '	'Dim objSessione As CreateSessione
'    '	Dim strWFErrore As String

'    '	Try
'    '		'Setto la variabile che mi dice che ho effettuato ricerca in dichiarato
'    '		'Session("cercaDichiarato") = True

'    '		objSessione = New CreateSessione(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))

'    '		If Not objSessione.CreaSessione(ConstSession.UserName, strWFErrore) Then
'    '			Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
'    '		End If

'    '		'Viene Salvata da GestioneAccertamenti.aspx.vb
'    '		'pere avere il codice contribuente e anno di accertamento
'    '		objHashTable = Session("HashTableDichiarazioniAccertamenti")

'    '		If objHashTable.Contains("TIPO_OPERAZIONE") = False Then
'    '			objHashTable.Add("TIPO_OPERAZIONE", Costanti.PROCEDIMENTO_ACCERTAMENTO)
'    '		Else
'    '			objHashTable("TIPO_OPERAZIONE") = Costanti.PROCEDIMENTO_ACCERTAMENTO
'    '		End If

'    '		If Not Page.IsPostBack Then
'    '			setHashTable(objHashTable, objSessione)
'    '		End If
'    '           '*** 20140509 - TASI ***
'    '           If objHashTable.Contains("URLServiziFreezer") = False Then
'    '               objHashTable.Add("URLServiziFreezer", ConstSession.URLServiziFreezer)
'    '           Else
'    '               objHashTable("URLServiziFreezer") = ConstSession.URLServiziFreezer
'    '           End If
'    '           If objHashTable.Contains("CONFIGURAZIONE_DICHIARAZIONE") = False Then
'    '               objHashTable.Add("CONFIGURAZIONE_DICHIARAZIONE", ConstSession.CONFIGURAZIONE_DICHIARAZIONE)
'    '           Else
'    '               objHashTable("CONFIGURAZIONE_DICHIARAZIONE") = ConstSession.CONFIGURAZIONE_DICHIARAZIONE
'    '           End If
'    '           If objHashTable.Contains("TRIBUTOCALCOLO") = False Then
'    '               objHashTable.Add("TRIBUTOCALCOLO", Costanti.TRIBUTO_ICI)
'    '           Else
'    '               objHashTable("TRIBUTOCALCOLO") = Costanti.TRIBUTO_ICI
'    '           End If
'    '           If objHashTable.Contains("ANNODA") = False Then
'    '               objHashTable.Add("ANNODA", objHashTable("ANNOACCERTAMENTO"))
'    '           Else
'    '               objHashTable("ANNODA") = objHashTable("ANNOACCERTAMENTO")
'    '           End If
'    '           If objHashTable.Contains("ANNOA") = False Then
'    '               objHashTable.Add("ANNOA", -1)
'    '           Else
'    '               objHashTable("ANNOA") = -1
'    '           End If
'    '           If objHashTable.Contains("CONNECTIONSTRINGOPENGOV") = False Then
'    '               objHashTable.Add("CONNECTIONSTRINGOPENGOV", ConstSession.StringConnectionOPENgov)
'    '           Else
'    '               objHashTable("CONNECTIONSTRINGOPENGOV") = ConstSession.StringConnectionOPENgov
'    '           End If
'    '           '*** ***
'    '		'******************************************************************
'    '		'Cerco tutte le dichiarazioni per l'anno selezionato
'    '		'******************************************************************

'    '		'Salvo il data set delle dichiarazioni
'    '		'Rifacciamo la query anche se è da rivedere (trovare soluzione
'    '		'ottimale per evitare di rifare ogni volta la query)
'    '		'Session("DSDichiarazioniAccertamenti") = objDSDichiarazioni

'    '		If Page.IsPostBack = False Then
'    '			'Dipe Spostata ricerca internamente
'    '			Dim objCOMDichiarazioniAccertamenti As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
'    '			objDSDichiarazioni = objCOMDichiarazioniAccertamenti.GetDatiAccertamenti(objHashTable)

'    '			If Not objDSDichiarazioni Is Nothing Then
'    '				If objDSDichiarazioni.Tables(0).DefaultView.Count > 0 Then
'    '					'Svuota la Session con la dichiarazione precedente
'    '					Session.Remove("DataSetDichiarazioni")
'    '					'Calcolo L'ICI su tutti gli Immobili
'    '					'Aggiorno campi IDLegame e ICICalcolato
'    '					Dim row As DataRow
'    '					Dim nElementi, i As Integer
'    '					nElementi = objDSDichiarazioni.Tables(0).DefaultView.Count

'    '					Dim objICI As DataSet
'    '					objICI = FncGen.CreateDSperCalcoloICI()

'    '					For i = 0 To nElementi - 1
'    '						row = objDSDichiarazioni.Tables(0).Rows(i)
'    '                           If objHashTable.Contains("ID_IMMOBILE") = False Then
'    '                               objHashTable.Add("ID_IMMOBILE", row.Item("ID").ToString)
'    '                           Else
'    '                               objHashTable("ID_IMMOBILE") = row.Item("ID").ToString
'    '                           End If
'    '                           CalcolaICI(objHashTable, row.Item("ID"), objICI)
'    '						row.Item("IDLegame") = i + 1
'    '						row.Item("Progressivo") = i + 1
'    '						row.AcceptChanges()
'    '						objDSDichiarazioni.AcceptChanges()
'    '					Next
'    '                       Log.Debug("devo fare calcolo ici totale")
'    '					'Lancio procedura calcolo ICI
'    '					CalcolaICITotale(objICI, objHashTable, TipoCalcolo)

'    '					Session.Remove("DATASET_CALCOLO_ICI_IMMOBILI_DICHIARATI")
'    '					Session.Add("DATASET_CALCOLO_ICI_IMMOBILI_DICHIARATI", objICI)
'    '                       Log.Debug("CalcolaICITotale fatto")

'    '					'Aggiorno Colonna ICI Calcolato nella Griglia dell'accertato
'    '					For i = 0 To objDSDichiarazioni.Tables(0).Rows.Count - 1
'    '						Dim rows() As DataRow
'    '						rows = objICI.Tables("TP_SITUAZIONE_FINALE_ICI").Select("ID_SITUAZIONE_FINALE='" & i & "'")
'    '						objDSDichiarazioni.Tables(0).Rows(i).Item("ICICalcolato") = rows(0).Item("ICI_TOTALE_DOVUTA")
'    '						objDSDichiarazioni.Tables(0).Rows(i).Item("ICICalcolatoACCONTO") = rows(0).Item("ICI_DOVUTA_ACCONTO")
'    '						objDSDichiarazioni.Tables(0).Rows(i).Item("ICICalcolatoSALDO") = rows(0).Item("ICI_DOVUTA_SALDO")
'    '						objDSDichiarazioni.Tables(0).Rows(i).Item("ICI_VALORE_ALIQUOTA") = rows(0).Item("ICI_VALORE_ALIQUOTA")
'    '						objDSDichiarazioni.Tables(0).AcceptChanges()
'    '					Next
'    '                       Log.Debug("Aggiornato Colonna ICI Calcolato nella Griglia dell'accertato")
'    '                       Dim dt As DataTable
'    '					dw = objDSDichiarazioni.Tables(0).DefaultView
'    '					dw.Sort = "FOGLIO, NUMERO, SUBALTERNO, IDLEGAME ASC"
'    '					dt = dw.Table
'    '					viewstate.Add("DataTableImmobiliDaAccertare", dt)
'    '				End If

'    '				chkSelTutti.Visible = True
'    '				lblTesto.Visible = True
'    '			End If

'    '			GrdDichiarato.start_index = 0
'    '			GrdDichiarato.DataSource = dw
'    '			GrdDichiarato.DataBind()
'    '			If Session("ESCLUDI_PREACCERTAMENTO") Then
'    '				'cancello da dichiarazioni i relativi immobili
'    '				Session.Remove("DATASET_CALCOLO_ICI_IMMOBILI_DICHIARATI")
'    '				viewstate.Remove("DataTableImmobiliDaAccertare")
'    '				Dim i As Integer
'    '				For i = objDSDichiarazioni.Tables(0).Rows.Count - 1 To 0 Step -1
'    '					objDSDichiarazioni.Tables(0).Rows(i).Delete()
'    '					objDSDichiarazioni.AcceptChanges()
'    '				Next
'    '			End If
'    '               Session("DataSetDichiarazioni") = objDSDichiarazioni
'    '           Else
'    '               'GrdDichiarato.AllowCustomPaging = False
'    '               'GrdDichiarato.start_index = GrdDichiarato.CurrentPageIndex
'    '               'dw = objDSDichiarazioni.Tables(0).DefaultView
'    '               'dw.Sort = "FOGLIO, NUMERO, SUBALTERNO, IDLEGAME ASC"
'    '               'GrdDichiarato.DataSource = dw
'    '		End If

'    '		Select Case CInt(GrdDichiarato.Rows.Count)
'    '			Case 0
'    '				GrdDichiarato.Visible = False
'    '				lblMessage.Text = "Non sono state trovate Dichiarazioni"
'    '			Case Is > 0
'    '				If Session("ESCLUDI_PREACCERTAMENTO") Then
'    '					lblMessage.Text = "Gli immobili dichiarati non verranno presi in considerazione per il calcolo di questo accertamento in quanto già calcolati in altro accertamento definitivo"
'    '					lblTesto.Visible = False
'    '					chkSelTutti.Visible = False
'    '					Dim strHidden As String
'    '					strHidden = "<script language='javascript'>"
'    '					strHidden += "parent.parent.Comandi.document.getElementById('btnRibaltaImmobile').style.display='none';"
'    '					strHidden += "</script>"
'    '					RegisterScript(sScript , Me.GetType())
'    '				End If
'    '				GrdDichiarato.Visible = True
'    '		End Select

'    '		dim sScript as string=""
'    '		
'    '		sscript+="parent.document.getElementById('attesaCarica').style.display='none';")
'    '		sscript+="parent.document.getElementById('loadGridDichiarato').style.display='' ;")
'    '		
'    '           RegisterScript(sScript , Me.GetType())

'    '       Catch ex As Exception
'    '         Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamenti.Page_Load.errore: ", ex)
'    '         Response.Redirect("../../PaginaErrore.aspx")
'    '           If Not IsNothing(objSessione) Then
'    '               objSessione.Kill()
'    '               objSessione = Nothing
'    '           End If
'    '           Response.Write("<script language='javascript'>")
'    '           Response.Write("</script>")
'    '           Response.Write(GestErrore.GetHTMLError(ex, "../../Styles.css"))
'    '           Response.End()
'    '	Finally
'    '		If Not IsNothing(objSessione) Then
'    '			objSessione.Kill()
'    '			objSessione = Nothing
'    '		End If

'    '	End Try
'    'End Sub
'    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
'        'Put user code to initialize the page here
'        Dim dw As DataView
'        Dim ListUIAcc() As objUIICIAccert
'        Dim blnResult As Boolean = False
'        Dim objHashTable As Hashtable
'        Dim sScript As String = ""
'        Dim ListSituazioneFinale() As objSituazioneFinale

'        Try
'            'Setto la variabile che mi dice che ho effettuato ricerca in dichiarato
'            'Session("cercaDichiarato") = True

'            Log.Debug("SearchDatiAccertamenti::entro")
'            'Viene Salvata da GestioneAccertamenti.aspx.vb
'            'pere avere il codice contribuente e anno di accertamento
'            objHashTable = Session("HashTableDichiarazioniAccertamenti")

'            If objHashTable.Contains("TIPO_OPERAZIONE") = False Then
'                objHashTable.Add("TIPO_OPERAZIONE", oggettoatto.procedimento.ACCERTAMENTO)
'            Else
'                objHashTable("TIPO_OPERAZIONE") = oggettoatto.procedimento.ACCERTAMENTO
'            End If

'            If Not Page.IsPostBack Then
'                setHashTable(objHashTable)
'            End If
'            '*** 20140509 - TASI ***
'            If objHashTable.Contains("URLServiziFreezer") = False Then
'                objHashTable.Add("URLServiziFreezer", ConstSession.URLServiziFreezer)
'            Else
'                objHashTable("URLServiziFreezer") = ConstSession.URLServiziFreezer
'            End If
'            If objHashTable.Contains("CONFIGURAZIONE_DICHIARAZIONE") = False Then
'                objHashTable.Add("CONFIGURAZIONE_DICHIARAZIONE", ConstSession.CONFIGURAZIONE_DICHIARAZIONE)
'            Else
'                objHashTable("CONFIGURAZIONE_DICHIARAZIONE") = ConstSession.CONFIGURAZIONE_DICHIARAZIONE
'            End If
'            If objHashTable.Contains("TRIBUTOCALCOLO") = False Then
'                objHashTable.Add("TRIBUTOCALCOLO", Utility.Costanti.TRIBUTO_ICI)
'            Else
'                objHashTable("TRIBUTOCALCOLO") = Utility.Costanti.TRIBUTO_ICI
'            End If
'            If objHashTable.Contains("ANNODA") = False Then
'                objHashTable.Add("ANNODA", objHashTable("ANNOACCERTAMENTO"))
'            Else
'                objHashTable("ANNODA") = objHashTable("ANNOACCERTAMENTO")
'            End If
'            If objHashTable.Contains("ANNOA") = False Then
'                objHashTable.Add("ANNOA", -1)
'            Else
'                objHashTable("ANNOA") = -1
'            End If
'            If objHashTable.Contains("CONNECTIONSTRINGOPENGOV") = False Then
'                objHashTable.Add("CONNECTIONSTRINGOPENGOV", ConstSession.StringConnectionOPENgov)
'            Else
'                objHashTable("CONNECTIONSTRINGOPENGOV") = ConstSession.StringConnectionOPENgov
'            End If
'            '*** ***
'            '******************************************************************
'            'Cerco tutte le dichiarazioni per l'anno selezionato
'            '******************************************************************
'            Log.Debug("SearchDatiAccertamenti::valorizzato objhastable")
'            'Salvo il data set delle dichiarazioni
'            'Rifacciamo la query anche se è da rivedere (trovare soluzione
'            'ottimale per evitare di rifare ogni volta la query)
'            'Session("DSDichiarazioniAccertamenti") = objDSDichiarazioni

'            If Page.IsPostBack = False Then
'                'Dipe Spostata ricerca internamente
'                Log.Debug("SearchDatiAccertamenti::devo richiamare getdatiaccertamenti")
'                Dim objCOMDichiarazioniAccertamenti As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
'                Log.Debug("SearchDatiAccertamenti::ho valorizzato objCOMDichiarazioniAccertamenti")
'                ListUIAcc = objCOMDichiarazioniAccertamenti.GetDatiAccertamenti(objHashTable, ListSituazioneFinale)
'                Log.Debug("SearchDatiAccertamenti::richiamato getdatiaccertamenti")
'                If Not ListUIAcc Is Nothing Then
'                    If ListUIAcc.GetLength(0) > 0 Then
'                        'Svuota la Session con la dichiarazione precedente
'                        Session.Remove("DataSetDichiarazioni")
'                        Session.Remove("DATASET_CALCOLO_ICI_IMMOBILI_DICHIARATI")
'                        Session.Add("DATASET_CALCOLO_ICI_IMMOBILI_DICHIARATI", ListSituazioneFinale)
'                        ViewState.Add("DataTableImmobiliDaAccertare", ListUIAcc)
'                        GrdDichiarato.DataSource = ListUIAcc
'                        GrdDichiarato.DataBind()
'                    End If
'                    chkSelTutti.Visible = True
'                    lblTesto.Visible = True
'                End If
'                Session("DataSetDichiarazioni") = ListUIAcc
'            Else
'                'GrdDichiarato.AllowCustomPaging = False
'                'GrdDichiarato.start_index = GrdDichiarato.CurrentPageIndex
'                'dw = objDSDichiarazioni.Tables(0).DefaultView
'                'dw.Sort = "FOGLIO, NUMERO, SUBALTERNO, IDLEGAME ASC"
'                'GrdDichiarato.DataSource = dw
'            End If

'            Select Case CInt(GrdDichiarato.Rows.Count)
'                Case 0
'                    GrdDichiarato.Visible = False
'                    lblMessage.Text = "Non sono state trovate Dichiarazioni"
'                Case Is > 0
'                    If Session("ESCLUDI_PREACCERTAMENTO") Then
'                        lblMessage.Text = "Gli immobili dichiarati non verranno presi in considerazione per il calcolo di questo accertamento in quanto già calcolati in altro accertamento definitivo"
'                        lblTesto.Visible = False
'                        chkSelTutti.Visible = False
'                        sScript += "parent.parent.Comandi.document.getElementById('btnRibaltaImmobile').style.display='none';"
'                        RegisterScript(sScript , Me.GetType())
'                    End If
'                    GrdDichiarato.Visible = True
'            End Select
'            sScript += "parent.document.getElementById('loadGridDichiarato').style.display='' ;"
'            sScript += "parent.document.getElementById('attesaCarica').style.display='none';"
'            RegisterScript(sScript , Me.GetType())
'        Catch ex As Exception
'            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamenti.Page_Load.errore: ", ex)
'            Response.Redirect("../../PaginaErrore.aspx")
'            Response.Write("<script language='javascript'>")
'            Response.Write("</script>")
'            Response.Write(GestErrore.GetHTMLError(ex, "../../Styles.css"))
'            Response.End()
'        Finally
'            'dim sScript as string=""
'            '
'            'sscript+="parent.getElementById('attesaCarica').style.display='none';")
'            '
'            'RegisterScript(sScript , Me.GetType())
'        End Try
'    End Sub

'#Region "Griglie"
'    Protected Sub GrdRowDataBound(sender As Object, e As GridViewRowEventArgs)
'        Try
'            If (e.Row.RowType = DataControlRowType.DataRow) Then
'                e.Row.Cells(0).BackColor = Color.PaleGoldenrod
'                e.Row.Cells(0).Font.Bold = True

'                e.Row.Cells(22).ToolTip = "Premere questo pulsante per modificare il legame dell'immobile"
'                e.Row.Cells(23).ToolTip = "Premere questo pulsante per eliminare l'immobile"

'                e.Row.Cells(6).Text = FncGrd.IntForGridView(e.Row.Cells(6).Text)
'                e.Row.Cells(16).Text = FncGrd.FormatStringToZero(e.Row.Cells(16).Text)
'            End If
'        Catch ex As Exception
'            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamenti.GrdRowDataBound.errore: ", ex)
'            Response.Redirect("../../PaginaErrore.aspx")
'        End Try
'    End Sub
'    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
'        Try
'            Dim IDRow As String = e.CommandArgument.ToString()
'            Select Case e.CommandName
'                Case "RowEdit"
'                    For Each myRow As GridViewRow In GrdDichiarato.Rows
'                        If IDRow = CType(myRow.FindControl("hfprogressivo"), HiddenField).Value Then
'                            GrdDichiarato.EditIndex = myRow.RowIndex
'                            GrdDichiarato.DataSource = Session("DataSetDichiarazioni")
'                            GrdDichiarato.DataBind()
'                        End If
'                    Next
'                Case "RowUpdate"
'                    For Each myRow As GridViewRow In GrdDichiarato.Rows
'                        If IDRow = CType(myRow.FindControl("hfprogressivo"), HiddenField).Value Then
'                            Dim idLegame As TextBox = myRow.FindControl("txtLegame")

'                            If idLegame.Text = "" Then
'                                dim sScript as string=""
'                                sscript+= "msgLegameVuoto();" & vbCrLf
'                                RegisterScript(sScript , Me.GetType())
'                            Else
'                                Dim Progressivo As String
'                                Dim objDS As DataSet
'                                objDS = Session("DataSetDichiarazioni")
'                                Progressivo = IDRow
'                                'salvo Dati
'                                objDS.Tables(0).DefaultView.RowFilter = "PROGRESSIVO='" & Progressivo & "'"
'                                If objDS.Tables(0).DefaultView.Count > 0 Then
'                                    objDS.Tables(0).DefaultView.Item(0).Item("IDLegame") = idLegame.Text
'                                    objDS.Tables(0).AcceptChanges()
'                                End If
'                                objDS.Tables(0).DefaultView.RowFilter = ""

'                                GrdDichiarato.EditIndex = -1
'                                objDS.Tables(0).DefaultView.Sort = "FOGLIO, NUMERO, SUBALTERNO, IDLEGAME ASC"
'                                GrdDichiarato.DataSource = objDS
'                                GrdDichiarato.DataBind()
'                                Session("DataSetDichiarazioni") = objDS
'                            End If
'                        End If
'                    Next
'                Case "RowCancel"
'                    GrdDichiarato.EditIndex = -1
'                    Dim objDS As DataSet
'                    objDS = Session("DataSetDichiarazioni")
'                    GrdDichiarato.DataSource = objDS
'                    GrdDichiarato.DataBind()
'                Case "RowDelete"
'                    For Each myRow As GridViewRow In GrdDichiarato.Rows
'                        If IDRow = CType(myRow.FindControl("hfprogressivo"), HiddenField).Value Then
'                            Dim dt As DataTable
'                            Dim row() As DataRow

'                            dt = ViewState("DataTableImmobiliDaAccertare")
'                            row = dt.Select("PROGRESSIVO='" & IDRow & "'")
'                            dt.Rows.Remove(row(0))
'                            dt.AcceptChanges()
'                            dt.DefaultView.Sort = "FOGLIO, NUMERO, SUBALTERNO, IDLEGAME ASC"
'                            ViewState("DataTableImmobiliDaAccertare") = dt

'                            'Aggiorno la session con l'immobile in meno prendendolo dal DataTable--->DataSet
'                            Session("DataSetDichiarazioni") = dt.DataSet
'                            If dt.Rows.Count <= 0 Then
'                                GrdDichiarato.Visible = False
'                            Else
'                                GrdDichiarato.Visible = True
'                                GrdDichiarato.DataSource = dt
'                                GrdDichiarato.DataBind()
'                            End If
'                        End If
'                    Next
'            End Select
'        Catch ex As Exception
'            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamenti.GrdRowCommand.errore: ", ex)
'            Response.Redirect("../../PaginaErrore.aspx")
'        End Try
'    End Sub
'    'Protected Sub GrdDichiarato_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdDichiarato.ItemDataBound
'    'Try
'    '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
'    '        Dim lblLeg As Label
'    '        Dim idLegame As Integer
'    '        Dim objDS As DataSet

'    '        lblLeg = e.Item.Cells(idCelle.grdSanzioniSanzioni.cellLegame).FindControl("lblLegame")
'    '        idLegame = e.Item.Cells(idCelle.grdSanzioniSanzioni.cellIDLegame).Text
'    '        lblLeg.Text = idLegame

'    '        e.Item.Cells(0).BackColor = Color.PaleGoldenrod
'    '        e.Item.Cells(0).Font.Bold = True

'    '        e.Item.Cells(idCelle.grdSanzioniSanzioni.cellBottoneModifica).ToolTip = "Premere questo pulsante per modificare il legame dell'immobile"
'    '        e.Item.Cells(idCelle.grdSanzioniSanzioni.cellBottoneDelete).ToolTip = "Premere questo pulsante per eliminare l'immobile"

'    '        e.Item.Cells(idCelle.grdSanzioniSanzioni.cellSub).Text = IntForGridView(e.Item.Cells(idCelle.grdSanzioniSanzioni.cellSub).Text)
'    '        e.Item.Cells(idCelle.grdSanzioniSanzioni.cellNumeroUtilizzatori).Text = FormatStringToZero(e.Item.Cells(idCelle.grdSanzioniSanzioni.cellNumeroUtilizzatori).Text)

'    '    End If
'    'Catch ex As Exception
'    '      Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamenti.GrdDichiarato_ItemDataBound.errore: ", ex)
'    '    Response.Redirect("../../PaginaErrore.aspx")
'    'End Try
'    'End Sub

'    'Protected Sub GrdDichiarato_Edit(ByVal sender As Object, ByVal e As DataGridCommandEventArgs) Handles GrdDichiarato.EditCommand

'    '    GrdDichiarato.EditItemIndex = e.Item.ItemIndex
'    '    GrdDichiarato.DataSource = Session("DataSetDichiarazioni")
'    '    GrdDichiarato.DataBind()

'    'End Sub
'    'Protected Sub GrdDichiarato_Update(ByVal sender As Object, ByVal e As DataGridCommandEventArgs) Handles GrdDichiarato.UpdateCommand

'    '    'Prendo l'idLegame
'    '    Dim idLegame As TextBox = e.Item.Cells(idCelle.grdSanzioniSanzioni.cellLegame).FindControl("txtLegame")
'    'Try
'    '    If idLegame.Text = "" Then
'    '        dim sScript as string=""
'    '        sscript+= "msgLegameVuoto();" & vbCrLf
'    '        RegisterScript(sScript , Me.GetType())

'    '    Else


'    '        Dim Progressivo As String
'    '        Dim objDS As DataSet
'    '        objDS = Session("DataSetDichiarazioni")
'    '        Progressivo = e.Item.Cells(0).Text

'    '        'salvo Dati

'    '        objDS.Tables(0).DefaultView.RowFilter = "PROGRESSIVO='" & Progressivo & "'"
'    '        If objDS.Tables(0).DefaultView.Count > 0 Then
'    '            objDS.Tables(0).DefaultView.Item(0).Item("IDLegame") = idLegame.Text
'    '            objDS.Tables(0).AcceptChanges()
'    '        End If
'    '        objDS.Tables(0).DefaultView.RowFilter = ""

'    '        GrdDichiarato.EditItemIndex = -1
'    '        objDS.Tables(0).DefaultView.Sort = "FOGLIO, NUMERO, SUBALTERNO, IDLEGAME ASC"
'    '        GrdDichiarato.DataSource = objDS
'    '        GrdDichiarato.DataBind()
'    '        Session("DataSetDichiarazioni") = objDS
'    '    End If
'    'Catch ex As Exception
'    '      Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamenti.GrdDichiarato_Update.errore: ", ex)
'    '    Response.Redirect("../../PaginaErrore.aspx")
'    'End Try
'    'End Sub

'    'Protected Sub GrdDichiarato_Cancel(ByVal sender As Object, ByVal e As DataGridCommandEventArgs) Handles GrdDichiarato.CancelCommand
'    '    GrdDichiarato.EditItemIndex = -1
'    '    Dim objDS As DataSet
'    '    objDS = Session("DataSetDichiarazioni")
'    '    GrdDichiarato.DataSource = objDS
'    '    GrdDichiarato.DataBind()
'    'End Sub

'    'Protected Sub GrdDichiarato_DeleteCommand(ByVal s As Object, ByVal e As DataGridCommandEventArgs) Handles GrdDichiarato.DeleteCommand
'    '    Dim dt As DataTable
'    '    Dim row() As DataRow

'    '    dt = ViewState("DataTableImmobiliDaAccertare")
'    '    row = dt.Select("PROGRESSIVO='" & e.Item.Cells(0).Text & "'")

'    '    dt.Rows.Remove(row(0))
'    '    dt.AcceptChanges()

'    '    dt.DefaultView.Sort = "FOGLIO, NUMERO, SUBALTERNO, IDLEGAME ASC"
'    '    ViewState("DataTableImmobiliDaAccertare") = dt

'    '    'Aggiorno la session con l'immobile in meno prendendolo dal DataTable--->DataSet
'    '    Session("DataSetDichiarazioni") = dt.DataSet
'    'Try
'    '    If dt.Rows.Count <= 0 Then

'    '        GrdDichiarato.Visible = False

'    '    Else
'    '        GrdDichiarato.Visible = True
'    '        GrdDichiarato.DataSource = dt
'    '        GrdDichiarato.DataBind()

'    '    End If
'    'Catch ex As Exception
'    '      Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamenti.GrdDichiarato_DeleteCommand.errore: ", ex)
'    '    Response.Redirect("../../PaginaErrore.aspx")
'    'End Try
'    'End Sub
'#End Region

'    'Private Sub btnRibaltaImmobiliAcc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRibaltaImmobiliAcc.Click
'    '    Try
'    '        Dim i As Integer = 0
'    '        Dim idImmobile As Integer = -1
'    '        Dim sScript As String = ""

'    '        Dim dt As DataTable
'    '        Dim dt1 As New DataTable
'    '        Dim row() As DataRow

'    '        Dim objDBProvv As New DBPROVVEDIMENTI.ProvvedimentiDB

'    '        Dim dsImmobili As New DataSet
'    '        Dim objHashTable As New Hashtable

'    '        Dim dsRibalta As New DataSet("RIBALTA")
'    '        Dim nProg As Integer = 0

'    '        Dim dtPerRibaltamento As New DataTable

'    '        dt = ViewState("DataTableImmobiliDaAccertare")
'    '        dsImmobili = Session("DataSetDichiarazioni")
'    '        objHashTable = Session("HashTableDichiarazioniAccertamenti")

'    '        'nRecord = GrdDichiarato.Items.Count
'    '        For Each myRow As GridViewRow In GrdDichiarato.Rows
'    '            If CType(myRow.FindControl("chkRibaltaIm"), CheckBox).Checked = True Then
'    '                row = dt.Select("PROGRESSIVO='" & myRow.Cells(0).Text & "'")
'    '                nProg = PopolaRibaltamento(row, nProg)
'    '            End If
'    '        Next
'    '        'Associa()

'    '        If nProg > 0 Then
'    '            sScript += "parent.document.getElementById('loadGridAccertato').src='grdAccertato.aspx';" '"parent.parent.Visualizza.loadGridAccertato.location.href='grdAccertato.aspx';"
'    '        Else
'    '            sScript += "alert('Selezionare almeno un immobile per il ribaltamento!');"
'    '        End If
'    '        RegisterScript(sScript, Me.GetType())
'    '        'Session.Remove("RibaltaImmobili")
'    '    Catch Err As Exception
'    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamenti.btnRibalta_Click.errore: ", Err)
'    '        Response.Redirect("../../PaginaErrore.aspx")
'    '        Dim sErr As String = Err.Message
'    '    End Try
'    'End Sub
'    Private Sub btnRibaltaImmobiliAcc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRibaltaImmobiliAcc.Click
'        Try
'            Dim ListUI() As objUIICIAccert
'            Dim nProg As Integer = 0

'            ListUI = ViewState("DataTableImmobiliDaAccertare")

'            For Each myRow As GridViewRow In GrdDichiarato.Rows
'                If CType(myRow.FindControl("chkRibaltaIm"), CheckBox).Checked = True Then
'                    For Each myUI As objUIICIAccert In ListUI
'                        If myUI.Progressivo = myRow.Cells(0).Text Then
'                            nProg = PopolaRibaltamento(myUI, nProg)
'                            Exit For
'                        End If
'                    Next
'                End If
'            Next

'            Dim sScript As String = ""
'            If nProg > 0 Then
'                sScript += "parent.document.getElementById('loadGridAccertato').src='grdAccertato.aspx';" '"parent.parent.Visualizza.loadGridAccertato.location.href='grdAccertato.aspx';"
'            Else
'                sScript += "alert('Selezionare almeno un immobile per il ribaltamento!');"
'            End If
'            RegisterScript(sScript, Me.GetType())
'            'Session.Remove("RibaltaImmobili")
'        Catch Err As Exception
'            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamenti.btnRibalta_Click.errore: ", Err)
'            Response.Redirect("../../PaginaErrore.aspx")
'            Dim sErr As String = Err.Message
'        End Try
'    End Sub

'    Private Sub chkSelTutti_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSelTutti.CheckedChanged
'        Dim i As Integer = 0
'        Dim count As Integer = 0

'        Try
'            If chkSelTutti.Checked = True Then
'                '** ciclare la griglia e selezionare tutti gli immobili
'                For Each myRow As GridViewRow In GrdDichiarato.Rows
'                    CType(myRow.FindControl("chkRibaltaIm"), CheckBox).Checked = True
'                Next
'                'Session("RibaltaImmobili") = Session("DataSetDichiarazioni")
'            Else
'                '** nessuna selezione -> selezione manuale dell'operatore
'                For Each myRow As GridViewRow In GrdDichiarato.Rows
'                    CType(myRow.FindControl("chkRibaltaIm"), CheckBox).Checked = False
'                Next
'                'Session.Remove("RibaltaImmobili")
'            End If
'        Catch ex As Exception
'            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamenti.CheckedChanged.errore: ", ex)
'            Response.Redirect("../../PaginaErrore.aspx")
'        End Try
'    End Sub

'    Private Function PopolaRibaltamento(ByVal SelRow As objUIICIAccert, ByRef prog As Integer) As Integer
'        'Dim workTable As New DataTable("IMMOBILI")
'        Dim ListUIAccert() As objUIICIAccert

'        Try
'            If Not Session("DataTableImmobiliDaAccertare") Is Nothing Then
'                ListUIAccert = Session("DataTableImmobiliDaAccertare")
'            End If

'            prog = prog + 1

'            Dim myUI As New objUIICIAccert
'            myUI = SelRow
'            myUI.Progressivo = prog
'            myUI.AccDovuto = 0
'            myUI.SalDovuto = 0

'            Dim myArray As New ArrayList(ListUIAccert)
'            myArray.Add(myUI)
'            ListUIAccert = CType(myArray.ToArray(GetType(objUIICIAccert)), objUIICIAccert())

'            Session("DataTableImmobiliDaAccertare") = ListUIAccert
'            Return prog
'        Catch ex As Exception
'            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamenti.PopolaRibaltamento.errore:  ", ex)
'            Response.Redirect("../../PaginaErrore.aspx")
'        End Try
'    End Function

'    'Private Function CalcolaICI(ByVal objHashTable As Hashtable, ByVal Id_Immobile As String, ByVal objICI() As objSituazioneFinale) As Double
'    '    'CalcolaICI = 0
'    '    'Dim rnd As Random
'    '    'rnd = New Random
'    '    'CalcolaICI = CDbl(rnd.Next(10000)) / 100
'    '    'System.Threading.Thread.Sleep(500)
'    '    Return addRowsCalcoloICI(objICI, objHashTable, Id_Immobile)
'    'End Function
'    'Private Function addRowsCalcoloICI(ByRef objICI() As objSituazioneFinale, ByVal objHashTable As Hashtable, ByVal Id_Immobile As String) As Double
'    '    Dim row As New objSituazioneFinale
'    '    Dim objDSImmobili As DataSet
'    '    Dim clsGeneralFunction As New MyUtility

'    '    '******************************************************************
'    '    'Cerco tutte le dichiarazioni per l'anno selezionato
'    '    '******************************************************************
'    '    Log.Debug("addRowsCalcoloICI::richiamo GetImmobiliDichiaratoVirtualeICI")
'    '    Dim objCOMDichiarazioniAccertamenti As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
'    '    objDSImmobili = objCOMDichiarazioniAccertamenti.GetImmobiliDichiaratoVirtualeICI(objHashTable)
'    '    Log.Debug("addRowsCalcoloICI::richiamato GetImmobiliDichiaratoVirtualeICI")
'    '    Dim i As Integer = 0
'    '    Dim nUtilizzatori As Integer

'    '    Dim rowImmobile() As DataRow

'    '    rowImmobile = objDSImmobili.Tables(0).Select("ID_IMMOBILE='" & Id_Immobile & "'")
'    '    Try
'    '        If IsDBNull(rowImmobile(0).Item("NumeroUtilizzatori")) Then
'    '            nUtilizzatori = 0
'    '        ElseIf CStr(rowImmobile(0).Item("NumeroUtilizzatori")).CompareTo("") = 0 Then
'    '            nUtilizzatori = 0
'    '        Else
'    '            nUtilizzatori = rowImmobile(0).Item("NumeroUtilizzatori")
'    '        End If
'    '        row.NUtilizzatori = nUtilizzatori

'    '        If Not IsDBNull(rowImmobile(0).Item("COD_CONTRIBUENTE")) Then
'    '            row.IdContribuente = rowImmobile(0).Item("COD_CONTRIBUENTE")
'    '        End If
'    '        If Not IsDBNull(rowImmobile(0).Item("ANNO")) Then
'    '            row.Anno = rowImmobile(0).Item("ANNO")
'    '        End If
'    '        If Not IsDBNull(rowImmobile(0).Item("NUMERO_MESI_ACCONTO")) Then
'    '            row.AccMesi = rowImmobile(0).Item("NUMERO_MESI_ACCONTO")
'    '        End If
'    '        If Not IsDBNull(rowImmobile(0).Item("NUMERO_MESI_TOTALI")) Then
'    '            row.Mesi = rowImmobile(0).Item("NUMERO_MESI_TOTALI")
'    '        End If
'    '        If Not IsDBNull(rowImmobile(0).Item("PERC_POSSESSO")) Then
'    '            row.PercPossesso = rowImmobile(0).Item("PERC_POSSESSO")
'    '        End If
'    '        If Not IsDBNull(rowImmobile(0).Item("RIDUZIONE")) Then
'    '            row.FlagRiduzione = rowImmobile(0).Item("RIDUZIONE")
'    '        End If

'    '        If Not IsDBNull(rowImmobile(0).Item("TIPO_RENDITA")) Then
'    '            row.TipoRendita = rowImmobile(0).Item("TIPO_RENDITA")
'    '        End If
'    '        '*** 20140509 - TASI ***
'    '        'row("TIPO_POSSESSO") = rowImmobile(0).Item("TIPOPOSSESSO")
'    '        If Not IsDBNull(rowImmobile(0).Item("IDTIPOUTILIZZO")) Then
'    '            row.IdTipoUtilizzo = rowImmobile(0).Item("IDTIPOUTILIZZO")
'    '        End If
'    '        If Not IsDBNull(rowImmobile(0).Item("IDTIPOPOSSESSO")) Then
'    '            row.IdTipoPossesso = rowImmobile(0).Item("IDTIPOPOSSESSO")
'    '        End If
'    '        row.Tributo = Utility.Costanti.TRIBUTO_ICI
'    '        If Not IsDBNull(rowImmobile(0).Item("ZONA")) Then
'    '            row.Zona = rowImmobile(0).Item("ZONA")
'    '        End If
'    '        If Not IsDBNull(rowImmobile(0).Item("datainizio")) Then
'    '            row.DataInizio = clsGeneralFunction.ReplaceDataForDB(rowImmobile(0).Item("datainizio"))
'    '        End If
'    '        '*** ***
'    '        If Not IsDBNull(rowImmobile(0).Item("ENTE")) Then
'    '            row.IdEnte = rowImmobile(0).Item("ENTE")
'    '        End If
'    '        row.IdProcedimento = 0
'    '        row.IdRiferimento = 0
'    '        row.Provenienza = "D"
'    '        If Not IsDBNull(rowImmobile(0).Item("CARATTERISTICA")) Then
'    '            row.Caratteristica = rowImmobile(0).Item("CARATTERISTICA")
'    '        End If
'    '        If Not IsDBNull(rowImmobile(0).Item("VIA")) Then
'    '            row.Indirizzo = rowImmobile(0).Item("VIA")
'    '        End If
'    '        If Not IsDBNull(rowImmobile(0).Item("NUMEROCIVICO")) Then
'    '            row.Indirizzo += " " & rowImmobile(0).Item("NUMEROCIVICO")
'    '        End If
'    '        If Not IsDBNull(rowImmobile(0).Item("SEZIONE")) Then
'    '            row.Sezione = rowImmobile(0).Item("SEZIONE")
'    '        End If
'    '        If Not IsDBNull(rowImmobile(0).Item("FOGLIO")) Then
'    '            row.FOGLIO = rowImmobile(0).Item("FOGLIO")
'    '        End If
'    '        If Not IsDBNull(rowImmobile(0).Item("NUMERO")) Then
'    '            row.Numero = rowImmobile(0).Item("NUMERO")
'    '        End If
'    '        If Not IsDBNull(rowImmobile(0).Item("SUBALTERNO")) Then
'    '            row.SUBALTERNO = rowImmobile(0).Item("SUBALTERNO")
'    '        End If
'    '        If Not IsDBNull(rowImmobile(0).Item("CODCATEGORIACATASTALE")) Then
'    '            row.Categoria = rowImmobile(0).Item("CODCATEGORIACATASTALE")
'    '        End If
'    '        If Not IsDBNull(rowImmobile(0).Item("CODCLASSE")) Then
'    '            row.CLASSE = rowImmobile(0).Item("CODCLASSE")
'    '        End If
'    '        row.Protocollo = 0
'    '        If Not IsDBNull(rowImmobile(0).Item("STORICO")) Then
'    '            row.FlagStorico = rowImmobile(0).Item("STORICO")
'    '        End If
'    '        If Not IsDBNull(rowImmobile(0).Item("FLAGVALOREPROVV")) Then
'    '            row.FlagProvvisorio = rowImmobile(0).Item("FLAGVALOREPROVV")
'    '        End If
'    '        If Not IsDBNull(rowImmobile(0).Item("MESIPOSSESSO")) Then
'    '            row.MesiPossesso = rowImmobile(0).Item("MESIPOSSESSO")
'    '        End If
'    '        If Not IsDBNull(rowImmobile(0).Item("MESIESCLUSIONEESENZIONE")) Then
'    '            row.MesiEsenzione = rowImmobile(0).Item("MESIESCLUSIONEESENZIONE")
'    '        End If
'    '        If Not IsDBNull(rowImmobile(0).Item("MESIRIDUZIONE")) Then
'    '            row.MesiRiduzione = rowImmobile(0).Item("MESIRIDUZIONE")
'    '        End If
'    '        If Not IsDBNull(rowImmobile(0).Item("IMPDETRAZABITAZPRINCIPALE")) Then
'    '            row.ImpDetrazione = rowImmobile(0).Item("IMPDETRAZABITAZPRINCIPALE")
'    '        End If
'    '        If Not IsDBNull(rowImmobile(0).Item("POSSESSO")) Then
'    '            row.FlagPosseduto = rowImmobile(0).Item("POSSESSO")
'    '        End If
'    '        If Not IsDBNull(rowImmobile(0).Item("ESENTE_ESCLUSO")) Then
'    '            row.FlagEsente = rowImmobile(0).Item("ESENTE_ESCLUSO")
'    '        End If
'    '        If Not IsDBNull(rowImmobile(0).Item("esclusioneesenzione")) And Not IsDBNull(row.MesiEsenzione) Then
'    '            If rowImmobile(0).Item("esclusioneesenzione") = 2 And row.MesiEsenzione > 0 Then
'    '                row.FlagEsente = 0
'    '            End If
'    '        End If
'    '        If Not IsDBNull(rowImmobile(0).Item("RIDUZIONE")) Then
'    '            row.FlagRiduzione = rowImmobile(0).Item("RIDUZIONE")
'    '        End If
'    '        If Not IsDBNull(rowImmobile(0).Item("ID")) Then
'    '            row.IdImmobile = rowImmobile(0).Item("ID")
'    '        End If
'    '        If Not IsDBNull(rowImmobile(0).Item("IDIMMOBILEPERTINENTE")) Then
'    '            row.IdImmobilePertinenza = rowImmobile(0).Item("IDIMMOBILEPERTINENTE")
'    '        End If

'    '        If Not IsDBNull(rowImmobile(0).Item("datainizio")) Then
'    '            row.Dal = clsGeneralFunction.GiraData(rowImmobile(0).Item("datainizio"))
'    '        End If
'    '            If Not IsDBNull(rowImmobile(0).Item("datafine")) Then
'    '            row.Al = clsGeneralFunction.GiraData(rowImmobile(0).Item("datafine"))
'    '        End If

'    '        row.MeseInizio = 0
'    '        row.DataScadenza = ""
'    '        row.TipoOperazione = objHashTable("TIPO_OPERAZIONE")

'    '        If Not IsDBNull(rowImmobile(0).Item("rendita")) Then
'    '                    row.Rendita = rowImmobile(0).Item("rendita")
'    '                End If

'    '        '*************************************************************
'    '        'PERTINENZA
'    '        '*************************************************************
'    '        'Se è una pertinenza metto 2 a il flag principale
'    '        'If Not IsDBNull(rowImmobile(0).Item("COD_IMMOBILE_PERTINENZA")) Then
'    '        'ale 24052007
'    '        'If rowImmobile(0).Item("COD_IMMOBILE_PERTINENZA") <> "" Then
'    '        '    row.FLAG_PRINCIPALE") = 2
'    '        '    row.COD_IMMOBILE") = rowImmobile(0).Item("COD_IMMOBILE_DA_ACCERTAMENTO")
'    '        '    row.COD_IMMOBILE_PERTINENZA") = rowImmobile(0).Item("COD_IMMOBILE_PERTINENZA")
'    '        'Else
'    '        '    If rowImmobile(0).Item("FLAG_PRINCIPALE") = 0 Then
'    '        '        row.FLAG_PRINCIPALE") = 0
'    '        '    End If

'    '        '    If rowImmobile(0).Item("FLAG_PRINCIPALE") = 1 Then
'    '        '        row.FLAG_PRINCIPALE") = 1
'    '        '    End If
'    '        'End If


'    '        If Boolean.Parse(rowImmobile(0).Item("FLAG_PRINCIPALE").ToString) = True Then
'    '            row.FlagPrincipale = 1
'    '            'End If
'    '            'If Boolean.Parse(objDSDichiarazioniTotalePerAnno(iCount)("FLAG_PRINCIPALE").ToString) = False Then
'    '        Else
'    '                    If rowImmobile(0).Item("IDIMMOBILEPERTINENTE").ToString.Length > 0 Then
'    '                        If rowImmobile(0).Item("IDIMMOBILEPERTINENTE").ToString = "-1" Then
'    '                    row.FlagPrincipale = 0
'    '                Else
'    '                    row.FlagPrincipale = 2
'    '                End If
'    '                    ElseIf rowImmobile(0).Item("IDIMMOBILEPERTINENTE").ToString.Length = 0 Then
'    '                row.FlagPrincipale = 0
'    '            End If
'    '                End If

'    '        '*************************************************************
'    '        'FINE PERTINENZA
'    '        '*************************************************************
'    '        row.Id = Progressivo

'    '        If Not IsDBNull(rowImmobile(0).Item("ICI_VALORE_ALIQUOTA")) Then
'    '            row.Aliquota = rowImmobile(0).Item("ICI_VALORE_ALIQUOTA")
'    '        End If
'    '            If Not IsDBNull(rowImmobile(0).Item("diffimposta")) Then
'    '                row.diffimposta = rowImmobile(0).Item("diffimposta")
'    '        End If
'    '        If Not IsDBNull(rowImmobile(0).Item("totale")) Then
'    '            row.totale = rowImmobile(0).Item("totale")
'    '        End If

'    '            'row.VALORE") = rowImmobile(0).Item("VALORE")
'    '            '*** 20120530 - IMU ***
'    '            'devo ricalcolare il valore aggiornato
'    '            Dim FncValore As New ComPlusInterface.FncICI
'    '            Dim sClasse As String = ""
'    '            Dim sZona As String = ""
'    '            Dim sCategoria As String = ""
'    '            Dim nConsistenza As Double = 0
'    '            Dim nRendita As Double = 0
'    '            If Not IsDBNull(rowImmobile(0).Item("CODCLASSE")) Then
'    '                sClasse = rowImmobile(0).Item("CODCLASSE")
'    '            End If
'    '            If Not IsDBNull(rowImmobile(0).Item("ZONA")) Then
'    '                sZona = rowImmobile(0).Item("ZONA")
'    '            End If
'    '            If Not IsDBNull(rowImmobile(0).Item("CONSISTENZA")) Then
'    '                nConsistenza = rowImmobile(0).Item("CONSISTENZA")
'    '            End If
'    '            If Not IsDBNull(rowImmobile(0).Item("CODCATEGORIACATASTALE")) Then
'    '                sCategoria = rowImmobile(0).Item("CODCATEGORIACATASTALE")
'    '            End If
'    '            If Not IsDBNull(rowImmobile(0).Item("RENDITA")) Then
'    '                If CStr(rowImmobile(0).Item("RENDITA")) <> "" Then
'    '                    nRendita = rowImmobile(0).Item("RENDITA")
'    '                Else
'    '                    nRendita = 0
'    '                End If
'    '            Else
'    '                nRendita = 0
'    '            End If
'    '            '*** ***
'    '            If Not IsDBNull(rowImmobile(0).Item("COLTIVATOREDIRETTO")) Then
'    '            row.IsColtivatoreDiretto = rowImmobile(0).Item("COLTIVATOREDIRETTO")
'    '        End If
'    '        If Not IsDBNull(rowImmobile(0).Item("NUMEROFIGLI")) Then
'    '                row.NumeroFigli = rowImmobile(0).Item("NUMEROFIGLI")
'    '            End If
'    '            If Not IsDBNull(rowImmobile(0).Item("PERCENTCARICOFIGLI")) Then
'    '                row.PercentCaricoFigli = rowImmobile(0).Item("PERCENTCARICOFIGLI")
'    '            End If
'    '        '*** 20120709 - IMU per AF e LC devo usare il campo valore ***
'    '        Dim ValoreDich As Double = 0
'    '        If Not IsDBNull(rowImmobile(0).Item("VALORE")) Then
'    '            ValoreDich = rowImmobile(0).Item("VALORE")
'    '        End If
'    '        row.Valore = FncValore.CalcoloValore(ConstSession.StringConnectionOPENgov, ConstSession.StringConnectionICI, ConstSession.IdEnte, Year(rowImmobile(0).Item("datainizio")), rowImmobile(0).Item("TIPO_RENDITA"), sCategoria, sClasse, sZona, nRendita, ValoreDich, nConsistenza, CDate(rowImmobile(0).Item("datainizio")), row.IsColtivatoreDiretto)
'    '        If row.VALORE <= 0 Then
'    '            row.Valore = rowImmobile(0).Item("VALORE")
'    '        End If
'    '        '*** ***
'    '        '*** 20140509 - TASI ***
'    '        If Not IsDBNull(rowImmobile(0).Item("VALORE")) Then
'    '            row.ValoreReale = row.Valore
'    '        End If
'    '        '*** ***

'    '        Progressivo = Progressivo + 1

'    '        'Mi serve per calcolare l'ICI solo su 1 Immobile. Viene passato alla procedura di calcolo
'    '        'dell'ICI. Viene poi distrutto all'uscita del metodo
'    '        'Dim objDSImmobiliAppoggio As DataSet
'    '        ''Creo la struttura 
'    '        'objDSImmobiliAppoggio = FncGen.CreateDSperCalcoloICI()
'    '        'objDSImmobiliAppoggio.Tables("TP_SITUAZIONE_FINALE_ICI").ImportRow(row)
'    '        'objDSImmobiliAppoggio.AcceptChanges()

'    '        'Aggiungo Riga a mio DS x Situazione Finali ICI
'    '        Dim myArray As New ArrayList(objICI)
'    '        myArray.Add(row)
'    '        objICI = CType(myArray.ToArray(GetType(objSituazioneFinale)), objSituazioneFinale())
'    '    Catch ex As Exception
'    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamenti.addRowsCalcoloICI.errore:   ", ex)
'    '        Response.Redirect("../../PaginaErrore.aspx")
'    '    End Try
'    'End Function
'    'Private Sub CalcolaICITotale(ByRef objICI As objSituazioneFinale(), ByVal objHashTable As Hashtable, ByVal TipoCalcolo As Integer)
'    '    Try
'    '        '*** 20140509 - TASI ***
'    '        'Calcolo ICI
'    '        'Dim objICICalcolato As DataSet
'    '        'Dim objCOMCalcoloICI As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
'    '        'objICI = objCOMCalcoloICI.GetICI(objICI, objHashTable, TipoCalcolo)
'    '        Dim objCOMCalcoloICI As IFreezer = Activator.GetObject(GetType(ComPlusInterface.IFreezer), ConstSession.URLServiziFreezer)
'    '        objICI = objCOMCalcoloICI.CalcoloICI(objICI, objHashTable, TipoCalcolo)
'    '        '*** ***
'    '    Catch ex As Exception
'    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamenti.CalcolaICITotale.errore: ", ex)
'    '        Response.Redirect("../../PaginaErrore.aspx")
'    '    End Try
'    'End Sub

'    'Private Function CreateDsPerCalcoloICI_RESAUNICA_IN_CLSGESTIONEACCERTAMENTI() As DataSet
'    '    Dim objDS As New DataSet

'    '    Dim newTable As DataTable

'    '    Try

'    '        newTable = New DataTable("TP_SITUAZIONE_FINALE_ICI")

'    '        Dim NewColumn As New DataColumn
'    '        NewColumn.ColumnName = "ID_SITUAZIONE_FINALE"
'    '        NewColumn.DataType = System.Type.GetType("System.String")
'    '        NewColumn.DefaultValue = ""
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "ANNO"
'    '        NewColumn.DataType = System.Type.GetType("System.String")
'    '        NewColumn.DefaultValue = ""
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "COD_ENTE"
'    '        NewColumn.DataType = System.Type.GetType("System.String")
'    '        NewColumn.DefaultValue = ""
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "ID_PROCEDIMENTO"
'    '        NewColumn.DataType = System.Type.GetType("System.Int64")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "ID_RIFERIMENTO"
'    '        NewColumn.DataType = System.Type.GetType("System.Int64")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "PROVENIENZA"
'    '        NewColumn.DataType = System.Type.GetType("System.String")
'    '        NewColumn.DefaultValue = ""
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "CARATTERISTICA"
'    '        NewColumn.DataType = System.Type.GetType("System.String")
'    '        NewColumn.DefaultValue = ""
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "INDIRIZZO"
'    '        NewColumn.DataType = System.Type.GetType("System.String")
'    '        NewColumn.DefaultValue = ""
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "SEZIONE"
'    '        NewColumn.DataType = System.Type.GetType("System.String")
'    '        NewColumn.DefaultValue = ""
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "FOGLIO"
'    '        NewColumn.DataType = System.Type.GetType("System.String")
'    '        NewColumn.DefaultValue = ""
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "NUMERO"
'    '        NewColumn.DataType = System.Type.GetType("System.String")
'    '        NewColumn.DefaultValue = ""
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "SUBALTERNO"
'    '        NewColumn.DataType = System.Type.GetType("System.String")
'    '        NewColumn.DefaultValue = ""
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "CATEGORIA"
'    '        NewColumn.DataType = System.Type.GetType("System.String")
'    '        NewColumn.DefaultValue = ""
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "CLASSE"
'    '        NewColumn.DataType = System.Type.GetType("System.String")
'    '        NewColumn.DefaultValue = ""
'    '        newTable.Columns.Add(NewColumn)


'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "PROTOCOLLO"
'    '        NewColumn.DataType = System.Type.GetType("System.String")
'    '        NewColumn.DefaultValue = ""
'    '        newTable.Columns.Add(NewColumn)


'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "FLAG_STORICO"
'    '        NewColumn.DataType = System.Type.GetType("System.Int64")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "VALORE"
'    '        NewColumn.DataType = System.Type.GetType("System.Double")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)


'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "FLAG_PROVVISORIO"
'    '        NewColumn.DataType = System.Type.GetType("System.Int64")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "PERC_POSSESSO"
'    '        NewColumn.DataType = System.Type.GetType("System.Double")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "MESI_POSSESSO"
'    '        NewColumn.DataType = System.Type.GetType("System.Int64")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)


'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "MESI_ESCL_ESENZIONE"
'    '        NewColumn.DataType = System.Type.GetType("System.Int64")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "MESI_RIDUZIONE"
'    '        NewColumn.DataType = System.Type.GetType("System.Int64")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)


'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "IMPORTO_DETRAZIONE"
'    '        NewColumn.DataType = System.Type.GetType("System.Double")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "FLAG_POSSEDUTO"
'    '        NewColumn.DataType = System.Type.GetType("System.Int64")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "FLAG_ESENTE"
'    '        NewColumn.DataType = System.Type.GetType("System.Int64")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "FLAG_RIDUZIONE"
'    '        NewColumn.DataType = System.Type.GetType("System.Int64")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "FLAG_PRINCIPALE"
'    '        NewColumn.DataType = System.Type.GetType("System.Int64")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "COD_CONTRIBUENTE"
'    '        NewColumn.DataType = System.Type.GetType("System.Int64")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "COD_IMMOBILE_PERTINENZA"
'    '        NewColumn.DataType = System.Type.GetType("System.Int64")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "COD_IMMOBILE"
'    '        NewColumn.DataType = System.Type.GetType("System.Int64")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "DAL"
'    '        NewColumn.DataType = System.Type.GetType("System.String")
'    '        NewColumn.DefaultValue = ""
'    '        newTable.Columns.Add(NewColumn)


'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "AL"
'    '        NewColumn.DataType = System.Type.GetType("System.String")
'    '        NewColumn.DefaultValue = ""
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "NUMERO_MESI_ACCONTO"
'    '        NewColumn.DataType = System.Type.GetType("System.Int64")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)


'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "NUMERO_MESI_TOTALI"
'    '        NewColumn.DataType = System.Type.GetType("System.Int64")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "NUMERO_UTILIZZATORI"
'    '        NewColumn.DataType = System.Type.GetType("System.Int64")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "TIPO_RENDITA"
'    '        NewColumn.DataType = System.Type.GetType("System.String")
'    '        NewColumn.DefaultValue = ""
'    '        newTable.Columns.Add(NewColumn)


'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "ICI_ACCONTO_SENZA_DETRAZIONE"
'    '        NewColumn.DataType = System.Type.GetType("System.Double")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)


'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "ICI_ACCONTO_DETRAZIONE_APPLICATA"
'    '        NewColumn.DataType = System.Type.GetType("System.Double")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "ICI_DOVUTA_ACCONTO"
'    '        NewColumn.DataType = System.Type.GetType("System.Double")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)


'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "ICI_ACCONTO_DETRAZIONE_RESIDUA"
'    '        NewColumn.DataType = System.Type.GetType("System.Double")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)


'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "ICI_TOTALE_SENZA_DETRAZIONE"
'    '        NewColumn.DataType = System.Type.GetType("System.Double")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "ICI_TOTALE_DETRAZIONE_APPLICATA"
'    '        NewColumn.DataType = System.Type.GetType("System.Double")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "ICI_TOTALE_DOVUTA"
'    '        NewColumn.DataType = System.Type.GetType("System.Double")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "ICI_TOTALE_DETRAZIONE_RESIDUA"
'    '        NewColumn.DataType = System.Type.GetType("System.Double")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "ICI_DOVUTA_SALDO"
'    '        NewColumn.DataType = System.Type.GetType("System.Double")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "ICI_DOVUTA_DETRAZIONE_SALDO"
'    '        NewColumn.DataType = System.Type.GetType("System.Double")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)


'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "ICI_DOVUTA_SENZA_DETRAZIONE"
'    '        NewColumn.DataType = System.Type.GetType("System.Double")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "ICI_DOVUTA_DETRAZIONE_RESIDUA"
'    '        NewColumn.DataType = System.Type.GetType("System.Double")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)



'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "RIDUZIONE"
'    '        NewColumn.DataType = System.Type.GetType("System.Int64")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)


'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "MESE_INIZIO"
'    '        NewColumn.DataType = System.Type.GetType("System.Int64")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "DATA_SCADENZA"
'    '        NewColumn.DataType = System.Type.GetType("System.String")
'    '        NewColumn.DefaultValue = ""
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "TIPO_POSSESSO"
'    '        NewColumn.DataType = System.Type.GetType("System.String")
'    '        NewColumn.DefaultValue = ""
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "TIPO_OPERAZIONE"
'    '        NewColumn.DataType = System.Type.GetType("System.String")
'    '        NewColumn.DefaultValue = ""
'    '        newTable.Columns.Add(NewColumn)


'    '        '--------------------------------------------------------------------------------------

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "ICI_ACCONTO_DETRAZIONE_STATALE_CALCOLATA"
'    '        NewColumn.DataType = System.Type.GetType("System.Double")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "ICI_ACCONTO_DETRAZIONE_STATALE_APPLICATA"
'    '        NewColumn.DataType = System.Type.GetType("System.Double")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "ICI_ACCONTO_DETRAZIONE_STATALE_RESIDUA"
'    '        NewColumn.DataType = System.Type.GetType("System.Double")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "ICI_SALDO_DETRAZIONE_STATALE_CALCOLATA"
'    '        NewColumn.DataType = System.Type.GetType("System.Double")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "ICI_SALDO_DETRAZIONE_STATALE_APPLICATA"
'    '        NewColumn.DataType = System.Type.GetType("System.Double")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "ICI_SALDO_DETRAZIONE_STATALE_RESIDUA"
'    '        NewColumn.DataType = System.Type.GetType("System.Double")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "ICI_TOTALE_DETRAZIONE_STATALE_CALCOLATA"
'    '        NewColumn.DataType = System.Type.GetType("System.Double")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "ICI_TOTALE_DETRAZIONE_STATALE_APPLICATA"
'    '        NewColumn.DataType = System.Type.GetType("System.Double")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "ICI_TOTALE_DETRAZIONE_STATALE_RESIDUA"
'    '        NewColumn.DataType = System.Type.GetType("System.Double")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "RENDITA"
'    '        NewColumn.DataType = System.Type.GetType("System.Double")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)
'    '        '--------------------------------------------------------------------------------------

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "ICI_VALORE_ALIQUOTA"
'    '        NewColumn.DataType = System.Type.GetType("System.Double")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "diffimposta"
'    '        NewColumn.DataType = System.Type.GetType("System.Double")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)

'    '        NewColumn = New DataColumn
'    '        NewColumn.ColumnName = "totale"
'    '        NewColumn.DataType = System.Type.GetType("System.Double")
'    '        NewColumn.DefaultValue = "0"
'    '        newTable.Columns.Add(NewColumn)



'    '        '--------------------------------------------------------------------------------------

'    '        objDS.Tables.Add(newTable)

'    '        Return objDS

'    '    Catch ex As Exception
'    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamenti.CreateDsPerCalcoloICI_RESAUNICA_IN_CLSGESTIONEACCERTAMENTI.errore: ", ex)
'    '       Response.Redirect("../../PaginaErrore.aspx")
'    '    End Try

'    'End Function



'    'Private Sub setHashTable(ByRef objHashTable, ByVal objSessione)
'    '    'Dim strConnectionStringOPENgovProvvedimenti As String
'    '    Dim strConnectionStringOPENgovICI As String
'    'Try
'    '    'Recupero la hash table
'    '    objHashTable.Add("IDSOTTOAPPLICAZIONEICI", ConfigurationManager.AppSettings("OPENGOVI"))
'    '    'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString
'    '    strConnectionStringOPENgovICI = objSessione.oSession.GetPrivateDBManager(objHashTable("IDSOTTOAPPLICAZIONEICI")).GetConnection.ConnectionString


'    '    'Aggiungo gli altri campi nella hash table
'    '    objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
'    '    objHashTable.Add("CONNECTIONSTRINGOPENGOVICI", strConnectionStringOPENgovICI)
'    '    objHashTable.Add("USER", ConstSession.UserName)
'    '    objHashTable.Add("CODENTE", ConstSession.IdEnte)
'    '    objHashTable.Add("CodENTE", ConstSession.IdEnte)
'    '    objHashTable.Add("CODTIPOPROCEDIMENTO", "L")

'    '    If Not IsNothing(objSessione) Then
'    '        objSessione.Kill()
'    '        objSessione = Nothing
'    '    End If
'    '    Catch ex As Exception
'    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamenti.setHashTable.errore: ", ex)
'    '       Response.Redirect("../../PaginaErrore.aspx")
'    '    End Try
'    'End Sub
'    Private Sub setHashTable(ByRef objHashTable)
'        Try
'            'Recupero la hash table
'            objHashTable.Add("IDSOTTOAPPLICAZIONEICI", ConfigurationManager.AppSettings("OPENGOVI"))

'            'Aggiungo gli altri campi nella hash table
'            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
'            objHashTable.Add("CONNECTIONSTRINGOPENGOVICI", ConstSession.StringConnectionICI)
'            objHashTable.Add("USER", ConstSession.UserName)
'            objHashTable.Add("CODENTE", ConstSession.IdEnte)
'            objHashTable.Add("CodENTE", ConstSession.IdEnte)
'            objHashTable.Add("CODTIPOPROCEDIMENTO", "L")
'            objHashTable.add("TASIAPROPRIETARIO", 0)
'        Catch ex As Exception
'            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamenti.setHashTable.errore: ", ex)
'            Response.Redirect("../../PaginaErrore.aspx")
'        End Try
'    End Sub

'    'Protected Function annoBarra(ByVal objtemp As Object) As String
'    '    Dim clsGeneralFunction As New MyUtility
'    '    Dim strTemp As String = ""
'    '    Try
'    '        If Not IsDBNull(objtemp) Then

'    '            If CDate(objtemp).Date = Date.MinValue.Date Or CDate(objtemp).Date = Date.MaxValue.Date Then
'    '                strTemp = ""
'    '            Else
'    '                Dim MiaData As String = CType(objtemp, DateTime).ToString("yyyy/MM/dd")
'    '                strTemp = clsGeneralFunction.GiraDataCompletaFromDB(MiaData)
'    '            End If
'    '        End If
'    '        Return strTemp
'    '    Catch ex As Exception
'    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamenti.annoBara.errore: ", ex)
'    '        Response.Redirect("../../PaginaErrore.aspx")
'    '    End Try
'    'End Function
'    'Public Function IntForGridView(ByVal iInput As Object) As String

'    '    Dim ret As String = String.Empty
'    '    Try
'    '        If iInput.ToString() = "-1" Or iInput.ToString() = "-1,00" Then
'    '            ret = String.Empty
'    '        Else
'    '            ret = Convert.ToString(iInput)
'    '        End If

'    '        Return ret
'    '    Catch ex As Exception
'    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamenti.IntForGridView.errore: ", ex)
'    '        Response.Redirect("../../PaginaErrore.aspx")
'    '    End Try
'    'End Function
'    'Protected Function checkFlag(ByVal objtemp As Object) As String
'    '    Dim strTemp As String = ""
'    '    Try
'    '        If Not IsDBNull(objtemp) Then
'    '            If objtemp = "1" Then
'    '                Return "True"
'    '            Else
'    '                Return "False"
'    '            End If
'    '        Else
'    '            Return "False"
'    '        End If
'    '    Catch ex As Exception
'    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamenti.checkFlag.errore: ", ex)
'    '        Response.Redirect("../../PaginaErrore.aspx")
'    '    End Try
'    'End Function
'    'Protected Function checkFlagRidotto(ByVal objtemp As Object) As String
'    '    Dim strTemp As String = ""
'    '    Try
'    '        If Not IsDBNull(objtemp) Then
'    '            If objtemp = "0" Then
'    '                Return "True"
'    '            Else
'    '                Return "False"
'    '            End If
'    '        Else
'    '            Return "False"
'    '        End If
'    '    Catch ex As Exception
'    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamenti.checkFlagRidotto.errore: ", ex)
'    '        Response.Redirect("../../PaginaErrore.aspx")
'    '    End Try
'    'End Function
'    'Protected Function checkMesiRiduzione(ByVal objtemp As Object) As String
'    '    Try
'    '        If Not IsDBNull(objtemp) Then
'    '            If Int(objtemp) > 0 Then
'    '                Return "True"
'    '            Else
'    '                Return "False"
'    '            End If
'    '        Else
'    '            Return "False"
'    '        End If
'    '    Catch ex As Exception
'    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamenti.checkMesiRiduzione.errore: ", ex)
'    '        Response.Redirect("../../PaginaErrore.aspx")
'    '    End Try
'    'End Function
'    'Protected Function checkPertinenza(ByVal objtempPERT As Object, ByVal objtempIMM As Object) As String

'    '    Dim strTemp As String = ""
'    '    Try
'    '        If Not IsDBNull(objtempPERT) Or Not IsDBNull(objtempIMM) Then
'    '            If objtempPERT <> "-1" And objtempPERT <> objtempIMM Then
'    '                Return "True"
'    '            Else
'    '                Return "False"
'    '            End If
'    '        Else
'    '            Return "False"
'    '        End If
'    '    Catch ex As Exception
'    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamenti.checkPertinenza.errore: ", ex)
'    '        Response.Redirect("../../PaginaErrore.aspx")
'    '    End Try
'    'End Function
'    'Private Function FormatStringToEmpty(ByVal objInput As Object) As String

'    '    Dim strOutput As String
'    '    Try
'    '        If (objInput Is Nothing) Then
'    '            strOutput = ""
'    '        ElseIf IsDBNull(objInput) Then
'    '            strOutput = ""
'    '        Else
'    '            If CStr(objInput) = "" Or CStr(objInput) = "0" Or CStr(objInput) = "-1" Then
'    '                strOutput = ""
'    '            Else
'    '                strOutput = objInput.ToString()
'    '            End If

'    '        End If
'    '        Return strOutput
'    '    Catch ex As Exception
'    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamenti.FormatStringToEmpty.errore: ", ex)
'    '        Response.Redirect("../../PaginaErrore.aspx")
'    '    End Try
'    'End Function
'    'Private Function FormatStringToZero(ByVal objInput As Object) As String

'    '    Dim strOutput As String
'    '    Try
'    '        If (objInput Is Nothing) Then
'    '            strOutput = ""
'    '        ElseIf IsDBNull(objInput) Then
'    '            strOutput = ""
'    '        Else
'    '            If CStr(objInput) = "" Or CStr(objInput) = "0" Or CStr(objInput) = "-1" Then
'    '                strOutput = ""
'    '            Else
'    '                strOutput = objInput.ToString()
'    '            End If

'    '        End If
'    '        Return strOutput
'    '    Catch ex As Exception
'    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamenti.FormatStringToZero.errore: ", ex)
'    '        Response.Redirect("../../PaginaErrore.aspx")
'    '    End Try
'    'End Function

'    ''Private Function Associa()
'    ''Dim dtDic, dtImmobili As DataTable
'    ''Dim i, iProg, iLeg, iProgMax, iLegMAx As Integer
'    ''Dim sScript As String
'    ''Try
'    ''    iProgMax = 0
'    ''    iLegMAx = 0
'    ''    dtDic = Session("DataTableImmobiliDic")
'    ''    dtImmobili = Session("DataTableImmobili")

'    ''    If Not dtDic Is Nothing Then

'    ''        If dtImmobili Is Nothing Then
'    ''            creaDataSet()
'    ''            dtImmobili = Session("DataTableImmobili")
'    ''        Else
'    ''            'Recupero maxlegame e macproggressivo immobili dichiarati
'    ''            If dtImmobili.Rows.Count > 0 Then
'    ''                For i = 0 To dtImmobili.Rows.Count - 1

'    ''                    iProg = dtImmobili.Rows(i).Item("progressivo")
'    ''                    iLeg = dtImmobili.Rows(i).Item("IDLEGAME")

'    ''                    If iProg > iProgMax Then
'    ''                        iProgMax = iProg
'    ''                    End If
'    ''                    If iLeg > iLegMAx Then
'    ''                        iLegMAx = iLeg
'    ''                    End If

'    ''                Next
'    ''            End If
'    ''        End If
'    ''        If dtDic.Rows.Count > 0 Then
'    ''            For i = 0 To dtDic.Rows.Count - 1
'    ''                iLegMAx += 1
'    ''                iProgMax += 1
'    ''                dtDic.Rows(i).Item("progressivo") = iProgMax
'    ''                dtDic.Rows(i).Item("IDLEGAME") = iLegMAx
'    ''                dtDic.AcceptChanges()

'    ''                dtImmobili.Rows.Add(dtDic.Rows(i).ItemArray)
'    ''            Next
'    ''        End If

'    ''        Session("DataTableImmobili") = dtImmobili
'    ''    End If



'    ''Catch ex As Exception
'    '' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamenti.Associa.errore: ", ex)
'    ''  Response.Redirect("../../PaginaErrore.aspx")
'    ''End Try
'    ''End Function
'    'Private Function PopolaRibaltamento(ByVal tempRow() As DataRow, ByRef prog As Integer) As Integer
'    '    Dim workTableImmobiliManuali As New DataTable("IMMOBILIMANUALI")

'    '    Dim i As Integer = 0
'    '    Dim idImmobile As Integer = -1
'    '    Dim dt1 As New DataTable
'    '    Dim objDBProvv As New DBPROVVEDIMENTI.ProvvedimentiDB
'    '    Dim objTipo_Rendita As New DichiarazioniICI.Database.Tipo_RenditaTable(ConstSession.UserName)
'    '    Dim Valore As Double

'    '    Dim dsImmobili As New DataSet
'    '    Dim objHashTable As New Hashtable

'    '    Dim LocalObjDSDichiarazioni As DataSet
'    '    Dim miodatarow As DataRow()
'    '    LocalObjDSDichiarazioni = CType(Session("DataSetDichiarazioni"), DataSet)

'    '    Dim pertineza As Integer
'    '    Dim riduzione As Integer

'    '    Try

'    '        objHashTable = Session("HashTableDichiarazioniAccertamenti")

'    '        If workTable.Rows.Count = 0 Then
'    '            workTable.Columns.Add("DAL")                       '0
'    '            workTable.Columns.Add("AL")                     '1
'    '            workTable.Columns.Add("FOGLIO")                    '2
'    '            workTable.Columns.Add("NUMERO")                    '3
'    '            workTable.Columns.Add("SUBALTERNO")                   '4
'    '            workTable.Columns.Add("CATEGORIA")                     '5
'    '            workTable.Columns.Add("CLASSE")                    '6
'    '            workTable.Columns.Add("CONSISTENZA")                     '7
'    '            workTable.Columns.Add("TR")                     '8
'    '            workTable.Columns.Add("RENDITA")                      '9
'    '            workTable.Columns.Add("RENDITA_VALORE")                  '10
'    '            workTable.Columns.Add("ZONA")                        '11
'    '            workTable.Columns.Add("IDSANZIONI")                   '12
'    '            workTable.Columns.Add("IDLEGAME", System.Type.GetType("System.Int32"))                      '13
'    '            workTable.Columns.Add("ICICALCOLATO")                      '14
'    '            workTable.Columns.Add("PROGRESSIVO")                     '15
'    '            workTable.Columns.Add("SANZIONI")                       '16
'    '            workTable.Columns.Add("INTERESSI")                     '17
'    '            workTable.Columns.Add("PERCPOSSESSO")                      '18
'    '            workTable.Columns.Add("TITPOSSESSO")                     '19
'    '            workTable.Columns.Add("FLAG_PRINCIPALE")                    '20
'    '            workTable.Columns.Add("FLAG_PERTINENZA")                    '21
'    '            workTable.Columns.Add("FLAG_RIDOTTO")                      '22
'    '            workTable.Columns.Add("IDIMMOBILEPERTINENZA")                    '23
'    '            workTable.Columns.Add("SEZIONE")                      '24
'    '            workTable.Columns.Add("INDIRIZZO")                     '25
'    '            workTable.Columns.Add("CODTITPOSSESSO")                  '26
'    '            workTable.Columns.Add("CODTIPORENDITA")                  '27
'    '            workTable.Columns.Add("ICICALCOLATOACCONTO")                   '28
'    '            workTable.Columns.Add("ICICALCOLATOSALDO")                   '29
'    '            workTable.Columns.Add("NUMEROUTILIZZATORI")                 '30
'    '            workTable.Columns.Add("ID_IMMOBILE_ORIGINALE_DICHIARATO")                 '31
'    '            workTable.Columns.Add("CODICE_VIA")                   '32
'    '            workTable.Columns.Add("CALCOLA_INTERESSI")                   '33
'    '            workTable.Columns.Add("DESC_TIPO_RENDITA")                   '34
'    '            workTable.Columns.Add("DESC_SANZIONE")                    '35
'    '            workTable.Columns.Add("ID_IMMOBILE_ACCERTATO")                  '36
'    '            workTable.Columns.Add("CODCOMUNE")                     '37
'    '            workTable.Columns.Add("COMUNE")                    '38
'    '            workTable.Columns.Add("ESPCIVICO")                     '39
'    '            workTable.Columns.Add("INTERNO")                      '40
'    '            workTable.Columns.Add("NUMEROCIVICO")                      '41
'    '            workTable.Columns.Add("PIANO")                      '42
'    '            workTable.Columns.Add("SCALA")                      '43
'    '            workTable.Columns.Add("INTERNO1")                       '44
'    '            workTable.Columns.Add("BARRATO")                      '45
'    '            workTable.Columns.Add("MESIESCLUSIONEESENZIONE")                  '46
'    '            workTable.Columns.Add("FLAG_ESENTE")                     '47
'    '            workTable.Columns.Add("MESIRIDUZIONE")                    '48

'    '            workTable.Columns.Add("ICI_VALORE_ALIQUOTA")                   '49
'    '            workTable.Columns.Add("diffimposta")                     '50
'    '            workTable.Columns.Add("totale")                    '51

'    '            '*** 20120530 - IMU ***
'    '            workTable.Columns.Add("COLTIVATOREDIRETTO")             '52
'    '            workTable.Columns.Add("NUMEROFIGLI")                '53
'    '            workTable.Columns.Add("PERCENTCARICOFIGLI")             '54
'    '            '*** ***
'    '            '*** 20140509 - TASI ***
'    '            workTable.Columns.Add("CODTRIBUTO")
'    '            '*** ***
'    '            'Session("DataTableImmobiliDic") = workTable
'    '            Session("DataTableImmobili") = workTable
'    '        End If

'    '        prog = prog + 1

'    '        Dim newtempRow As DataRow
'    '        newtempRow = workTable.NewRow()

'    '        If tempRow(0).ItemArray(0).ToString() <> "" Then
'    '            newtempRow(0) = CDate(tempRow(0).ItemArray(0).ToString())               'dal
'    '        Else
'    '            newtempRow(0) = ""
'    '        End If

'    '        If tempRow(0).ItemArray(1).ToString() <> "" Then
'    '            newtempRow(1) = CDate(tempRow(0).ItemArray(1).ToString())               'al
'    '        Else
'    '            newtempRow(1) = ""
'    '        End If
'    '        'forzo la data di fine prendendo quella della textbox
'    '        If TxtDataFineRibalta.Text <> "" Then
'    '            newtempRow(1) = CDate(TxtDataFineRibalta.Text)
'    '        End If

'    '        newtempRow(2) = tempRow(0).ItemArray(2).ToString()           'foglio
'    '        newtempRow(3) = tempRow(0).ItemArray(3).ToString()            'numero
'    '        newtempRow(4) = tempRow(0).ItemArray(4).ToString()           'sub
'    '        newtempRow(5) = tempRow(0).ItemArray(5).ToString()           'Categoria
'    '        newtempRow(6) = tempRow(0).ItemArray(6).ToString()           ' Classe
'    '        newtempRow(7) = tempRow(0).ItemArray(7).ToString()           'Consistenza
'    '        newtempRow(8) = tempRow(0).ItemArray(8).ToString()           'TR

'    '        newtempRow(9) = tempRow(0).ItemArray(9).ToString()           'rendita
'    '        newtempRow(11) = tempRow(0).ItemArray(11).ToString()             'zona
'    '        If tempRow(0).ItemArray(9).ToString() <> "" Then
'    '            '*** 20120709 - IMU per AF e LC devo usare il campo valore ***
'    '            Dim ValoreDich As Double = 0
'    '            If tempRow(0).ItemArray(10).ToString() > 0 Then
'    '                ValoreDich = tempRow(0).ItemArray(10)
'    '            End If
'    '            'Valore = objDBProvv.CalcoloValoredaRendita(tempRow(0).ItemArray(9).ToString(), tempRow(0).ItemArray(8).ToString(), tempRow(0).ItemArray(5).ToString(), objHashTable("ANNOACCERTAMENTO").ToString())
'    '            '*** 20120530 - IMU ***
'    '            Dim FncValore As New ComPlusInterface.FncICI
'    '            Try
'    '                Valore = FncValore.CalcoloValore(ConstSession.StringConnectionOPENgov, ConstSession.StringConnectionICI, ConstSession.IdEnte, Year(tempRow(0).ItemArray(0).ToString()), tempRow(0).ItemArray(8).ToString(), tempRow(0).ItemArray(5).ToString(), tempRow(0).ItemArray(6).ToString(), tempRow(0).ItemArray(11).ToString(), tempRow(0).ItemArray(9).ToString(), ValoreDich, tempRow(0).ItemArray(7).ToString(), CDate(tempRow(0).ItemArray(0).ToString()), tempRow(0).ItemArray(54).ToString())
'    '            Catch ex As Exception
'    '                Log.Debug("Popolaribaltamento::calcolo valore::" & ex.Message)
'    '            End Try
'    '            '*** ***
'    '            If Valore.ToString() <> tempRow(0).ItemArray(10).ToString() And Valore <> 0 Then
'    '                newtempRow(10) = Valore.ToString()                 'valore
'    '            Else
'    '                newtempRow(10) = tempRow(0).ItemArray(10).ToString()                   'valore
'    '            End If
'    '            If CDbl(tempRow(0).ItemArray(9)) = 0 Then
'    '                'calcolo rendita da valore
'    '                Valore = objDBProvv.CalcoloRenditadaValore(tempRow(0).ItemArray(10).ToString(), tempRow(0).ItemArray(8).ToString(), tempRow(0).ItemArray(5).ToString(), CDate(tempRow(0).ItemArray(0).ToString()).Year.ToString)
'    '                newtempRow(9) = Valore.ToString()                  'rendita
'    '            End If
'    '        Else
'    '            newtempRow(10) = tempRow(0).ItemArray(10).ToString()
'    '            'calcolo rendita da valore
'    '            Valore = objDBProvv.CalcoloRenditadaValore(tempRow(0).ItemArray(10).ToString(), tempRow(0).ItemArray(8).ToString(), tempRow(0).ItemArray(5).ToString(), CDate(tempRow(0).ItemArray(0).ToString()).Year.ToString)
'    '            newtempRow(9) = Valore.ToString()               'rendita
'    '        End If

'    '        newtempRow(12) = -1          'IDsanzioni
'    '        newtempRow(13) = tempRow(0).ItemArray(13).ToString()             'legame
'    '        newtempRow(14) = "-1"           'ICICalcolato
'    '        newtempRow(15) = prog           'progressivo
'    '        newtempRow(16) = -1           'sanzioni 
'    '        newtempRow(17) = 1            'interessi
'    '        newtempRow(18) = tempRow(0).ItemArray(18).ToString()             '% possesso
'    '        newtempRow(19) = tempRow(0).ItemArray(19).ToString()             'titPossesso
'    '        newtempRow(20) = tempRow(0).ItemArray(20).ToString()             'flag_principale abit princ

'    '        If tempRow(0).ItemArray(22).ToString() = "0" Then
'    '            riduzione = 1
'    '        Else
'    '            riduzione = 0
'    '        End If

'    '        newtempRow(48) = tempRow(0).ItemArray(50).ToString()             ' mesi riduzione
'    '        If tempRow(0).ItemArray(50) > 0 Then
'    '            riduzione = 1
'    '        Else
'    '            riduzione = 0
'    '        End If


'    '        If tempRow(0).ItemArray(20).ToString() = "1" Then
'    '            pertineza = 0
'    '            newtempRow(23) = ""
'    '        Else
'    '            'se pertinenza ha valore 0 o -1 -> l'immobile non è una pertinenza
'    '            'se invece è valorizzata, contiene l'id dell'immobile
'    '            If tempRow(0).ItemArray(23).ToString() = "-1" Or tempRow(0).ItemArray(23).ToString() = "0" Then              'pertinenza
'    '                pertineza = 0
'    '                newtempRow(23) = ""
'    '            Else
'    '                pertineza = 1
'    '                miodatarow = LocalObjDSDichiarazioni.Tables(0).Select("ID='" & tempRow(0).ItemArray(23).ToString() & "'")
'    '                If miodatarow.Length = 0 Then
'    '                    newtempRow(23) = tempRow(0).ItemArray(13).ToString()                         'miodatarow(i).Item("idlegame")

'    '                Else
'    '                    newtempRow(23) = tempRow(0).ItemArray(23).ToString()                         'id_immobile_pertinenza
'    '                End If

'    '            End If
'    '        End If

'    '        newtempRow(21) = pertineza           'chkPert
'    '        newtempRow(22) = riduzione           'chkRidotto


'    '        newtempRow(24) = tempRow(0).ItemArray(24).ToString()             'Sezione
'    '        newtempRow(25) = tempRow(0).ItemArray(25).ToString()              'Ubicazione     
'    '        newtempRow(26) = tempRow(0).ItemArray(26).ToString()             '-1 'TitPossesso


'    '        Dim TpRendita As DataTable
'    '        TpRendita = objTipo_Rendita.List()
'    '        Dim iTR As Integer = -1
'    '        If TpRendita.Rows.Count > 0 Then
'    '            For i = 0 To TpRendita.Rows.Count - 1
'    '                If (TpRendita.Rows(i)("SIGLA").ToString() = tempRow(0).ItemArray(8).ToString()) Then
'    '                    iTR = TpRendita.Rows(i)("COD_RENDITA")
'    '                    Exit For
'    '                End If
'    '            Next
'    '        End If

'    '        newtempRow(27) = iTR             'Codice TR
'    '        newtempRow(28) = ""          'iciCalcolatoACCONTO
'    '        newtempRow(29) = ""          'iciCalcolatoSALDO
'    '        newtempRow(30) = tempRow(0).ItemArray(30).ToString()             'NumUtiliz -> numero utilizzatori
'    '        newtempRow(31) = tempRow(0).ItemArray(31).ToString()             'ID IMMOBILE ORIGINALE DICHIARATO
'    '        newtempRow(32) = tempRow(0).ItemArray(32).ToString()             'CodVia
'    '        newtempRow(33) = True            'calcola interessi
'    '        newtempRow(34) = tempRow(0).ItemArray(34).ToString()             'desc tipo rendita
'    '        newtempRow(35) = ""          'desc sanzione
'    '        newtempRow(36) = -1          'id immobile accertato

'    '        newtempRow(37) = tempRow(0).ItemArray(37).ToString()             'codcomune
'    '        newtempRow(38) = tempRow(0).ItemArray(38).ToString()             'comune
'    '        newtempRow(39) = tempRow(0).ItemArray(39).ToString()             'EspCivico
'    '        newtempRow(40) = tempRow(0).ItemArray(40).ToString()             'interno
'    '        newtempRow(41) = tempRow(0).ItemArray(41).ToString()             'numero civico
'    '        newtempRow(42) = tempRow(0).ItemArray(42).ToString()             'piano
'    '        newtempRow(43) = tempRow(0).ItemArray(43).ToString()             'scala
'    '        newtempRow(44) = tempRow(0).ItemArray(44).ToString()             'interno1
'    '        newtempRow(45) = tempRow(0).ItemArray(45).ToString()             'barrato
'    '        newtempRow(46) = tempRow(0).ItemArray(46).ToString()             'mesiEsclusioneEsenzione
'    '        newtempRow(47) = tempRow(0).ItemArray(47).ToString()             'EsclusioneEsenzione
'    '        Log.Debug("sono su:" + newtempRow(2) + "|" + newtempRow(3) + "|" + newtempRow(4) + " esenzione->" + newtempRow(47))
'    '        'dipe 03/03/2011
'    '        newtempRow(49) = tempRow(0).ItemArray(51).ToString()             'ICI_VALORE_ALIQUOTA
'    '        newtempRow(50) = tempRow(0).ItemArray(52).ToString()             'diffimposta
'    '        newtempRow(51) = tempRow(0).ItemArray(53).ToString()             'totale


'    '        '*** 20120530 - IMU ***
'    '        newtempRow(52) = tempRow(0).ItemArray(54).ToString()             'COLTIVATOREDIRETTO
'    '        newtempRow(53) = tempRow(0).ItemArray(55).ToString()             'NUMEROFIGLI
'    '        newtempRow(54) = tempRow(0).ItemArray(56).ToString()             'PERCENTCARICOFIGLI
'    '        '*** ***
'    '        '*** 20140509 - TASI ***
'    '        newtempRow("CODTRIBUTO") = tempRow(0).ItemArray(57).ToString
'    '        '*** ***
'    '        workTable.Rows.Add(newtempRow)

'    '        Session("DataTableImmobili") = workTable
'    '        'Session("DataTableImmobiliDic") = workTable

'    '        Return prog
'    '    Catch ex As Exception
'    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamenti.PopolaRibaltamento.errore: ", ex)
'    '        Response.Redirect("../../PaginaErrore.aspx")
'    '    End Try
'    'End Function
'    'Function creaDataSet()
'    '    Dim workTable As New DataTable("IMMOBILI")


'    '    workTable.Columns.Add("DAL")                 '0
'    '    workTable.Columns.Add("AL")               '1
'    '    workTable.Columns.Add("FOGLIO")              '2
'    '    workTable.Columns.Add("NUMERO")              '3
'    '    workTable.Columns.Add("SUBALTERNO")             '4
'    '    workTable.Columns.Add("CATEGORIA")            '5
'    '    workTable.Columns.Add("CLASSE")              '6
'    '    workTable.Columns.Add("CONSISTENZA")               '7
'    '    workTable.Columns.Add("TR")               '8
'    '    workTable.Columns.Add("RENDITA")                '9
'    '    workTable.Columns.Add("RENDITA_VALORE")            '10
'    '    workTable.Columns.Add("ZONA")               '11
'    '    workTable.Columns.Add("IDSANZIONI")             '12
'    '    workTable.Columns.Add("IDLEGAME", System.Type.GetType("System.Int32"))                '13
'    '    workTable.Columns.Add("ICICALCOLATO")             '14
'    '    workTable.Columns.Add("PROGRESSIVO")               '15
'    '    workTable.Columns.Add("SANZIONI")              '16
'    '    workTable.Columns.Add("INTERESSI")            '17
'    '    workTable.Columns.Add("PERCPOSSESSO")             '18
'    '    workTable.Columns.Add("TITPOSSESSO")               '19
'    '    workTable.Columns.Add("FLAG_PRINCIPALE")              '20
'    '    workTable.Columns.Add("FLAG_PERTINENZA")              '21
'    '    workTable.Columns.Add("FLAG_RIDOTTO")             '22
'    '    workTable.Columns.Add("IDIMMOBILEPERTINENZA")           '23
'    '    workTable.Columns.Add("SEZIONE")                '24
'    '    workTable.Columns.Add("INDIRIZZO")            '25
'    '    workTable.Columns.Add("CODTITPOSSESSO")            '26
'    '    workTable.Columns.Add("CODTIPORENDITA")            '27
'    '    workTable.Columns.Add("ICICALCOLATOACCONTO")             '28
'    '    workTable.Columns.Add("ICICALCOLATOSALDO")          '29
'    '    workTable.Columns.Add("NUMEROUTILIZZATORI")           '30
'    '    workTable.Columns.Add("ID_IMMOBILE_ORIGINALE_DICHIARATO")        '31
'    '    workTable.Columns.Add("CODICE_VIA")             '32
'    '    workTable.Columns.Add("CALCOLA_INTERESSI")          '33
'    '    workTable.Columns.Add("DESC_TIPO_RENDITA")          '34
'    '    workTable.Columns.Add("DESC_SANZIONE")           '35
'    '    workTable.Columns.Add("ID_IMMOBILE_ACCERTATO")         '36
'    '    workTable.Columns.Add("CODCOMUNE")            '37
'    '    workTable.Columns.Add("COMUNE")              '38
'    '    workTable.Columns.Add("ESPCIVICO")            '39
'    '    workTable.Columns.Add("INTERNO")                '40
'    '    workTable.Columns.Add("NUMEROCIVICO")             '41
'    '    workTable.Columns.Add("PIANO")             '42
'    '    workTable.Columns.Add("SCALA")             '43
'    '    workTable.Columns.Add("INTERNO1")              '44
'    '    workTable.Columns.Add("BARRATO")                '45
'    '    workTable.Columns.Add("MESIESCLUSIONEESENZIONE")            '46
'    '    workTable.Columns.Add("FLAG_ESENTE")               '47
'    '    workTable.Columns.Add("MESIRIDUZIONE")           '48
'    '    '*** 20120530 - IMU ***
'    '    workTable.Columns.Add("COLTIVATOREDIRETTO")       '52
'    '    workTable.Columns.Add("NUMEROFIGLI")          '53
'    '    workTable.Columns.Add("PERCENTCARICOFIGLI")       '54
'    '    '*** ***
'    '    '*** 20140509 - TASI ***
'    '    workTable.Columns.Add("CODTRIBUTO")
'    '    '*** ***
'    '    Session("DataTableImmobili") = workTable


'    'End Function

'    'Private Function PopolaRibaltamentoOld(ByVal dtRibalta As DataTable) As Integer

'    '    Dim workTable As New DataTable("IMMOBILI")
'    '    Dim workTableImmobiliManuali As New DataTable("IMMOBILIMANUALI")

'    '    Dim i As Integer = 0
'    '    Dim nRecord As Integer
'    '    Dim idImmobile As Integer = -1

'    '    Dim dt As DataTable
'    '    Dim dt1 As New DataTable
'    '    Dim row() As DataRow

'    '    Dim objDBProvv As New DBPROVVEDIMENTI.ProvvedimentiDB
'    '    Dim sRequestIdProgr As String
'    '    Dim Valore As Double

'    '    Dim dsImmobili As New DataSet
'    '    Dim objHashTable As New Hashtable

'    '    Dim prog As Integer = 0
'    '    Dim pertineza As Integer


'    '    objHashTable = Session("HashTableDichiarazioniAccertamenti")

'    '    workTable.Columns.Add("DAL")
'    '    workTable.Columns.Add("AL")
'    '    workTable.Columns.Add("FOGLIO")
'    '    workTable.Columns.Add("NUMERO")
'    '    workTable.Columns.Add("SUBALTERNO")
'    '    workTable.Columns.Add("CATEGORIA")
'    '    workTable.Columns.Add("CLASSE")
'    '    workTable.Columns.Add("CONSISTENZA")
'    '    workTable.Columns.Add("TR")
'    '    workTable.Columns.Add("RENDITA_VALORE")
'    '    workTable.Columns.Add("IDSANZIONI")
'    '    workTable.Columns.Add("IDLEGAME")
'    '    workTable.Columns.Add("ICICALCOLATO")
'    '    workTable.Columns.Add("PROGRESSIVO")
'    '    workTable.Columns.Add("SANZIONI")
'    '    workTable.Columns.Add("INTERESSI")
'    '    workTable.Columns.Add("PERCPOSSESSO")
'    '    workTable.Columns.Add("TITPOSSESSO")
'    '    workTable.Columns.Add("FLAG_PRINCIPALE")
'    '    workTable.Columns.Add("FLAG_PERTINENZA")
'    '    workTable.Columns.Add("FLAG_RIDOTTO")
'    '    workTable.Columns.Add("IDIMMOBILEPERTINENZA")
'    '    workTable.Columns.Add("SEZIONE")
'    '    workTable.Columns.Add("INDIRIZZO")
'    '    workTable.Columns.Add("CODTITPOSSESSO")
'    '    workTable.Columns.Add("CODTIPORENDITA")
'    '    workTable.Columns.Add("ICICALCOLATOACCONTO")
'    '    workTable.Columns.Add("ICICALCOLATOSALDO")
'    '    workTable.Columns.Add("NUMEROUTILIZZATORI")
'    '    workTable.Columns.Add("ID_IMMOBILE_ORIGINALE_DICHIARATO")
'    '    workTable.Columns.Add("CODICE_VIA")
'    '    workTable.Columns.Add("CALCOLA_INTERESSI")

'    '    Session("DataTableImmobili") = workTable
'    'Try

'    '    For i = 0 To dtRibalta.Rows.Count - 1

'    '        prog = prog + 1

'    '        Dim tempRow As DataRow
'    '        tempRow = workTable.NewRow()

'    '        If dtRibalta.Rows(i)("DAL").ToString() <> "" Then
'    '            tempRow(0) = CDate(dtRibalta.Rows(i)("DAL").ToString())
'    '        Else
'    '            tempRow(0) = ""
'    '        End If

'    '        If dtRibalta.Rows(i)("AL").ToString() <> "" Then
'    '            tempRow(1) = CDate(dtRibalta.Rows(i)("AL").ToString())
'    '        Else
'    '            tempRow(1) = ""
'    '        End If

'    '        tempRow(2) = dtRibalta.Rows(i)("Foglio").ToString()
'    '        tempRow(3) = dtRibalta.Rows(i)("Numero").ToString()
'    '        tempRow(4) = dtRibalta.Rows(i)("Subalterno").ToString()
'    '        tempRow(5) = dtRibalta.Rows(i)("Categoria").ToString() 'Categoria
'    '        tempRow(6) = dtRibalta.Rows(i)("Classe").ToString() ' Classe
'    '        tempRow(7) = dtRibalta.Rows(i)("Consistenza").ToString() 'txtConsistenza
'    '        tempRow(8) = dtRibalta.Rows(i)("tr").ToString() 'TR

'    '        '*** 20120530 - IMU ***
'    '        'Valore = objDBProvv.CalcoloValoredaRendita(dtRibalta.Rows(i)("rendita_valore").ToString(), dtRibalta.Rows(i)("tr").ToString(), dtRibalta.Rows(i)("Categoria").ToString(), objHashTable("ANNOACCERTAMENTO").ToString())
'    '        Dim FncValore As New ComPlusInterface.FncICI
'    '        Valore = FncValore.CalcoloValore(ConstSession.StringConnectionOPENgov, ConstSession.StringConnectionICI, constsession.idente, Year(dtRibalta.Rows(i)("dal").ToString()), dtRibalta.Rows(i)("tr").ToString(), dtRibalta.Rows(i)("Categoria").ToString(), dtRibalta.Rows(i)("Classe").ToString(), "", dtRibalta.Rows(i)("rendita_valore").ToString(), dtRibalta.Rows(i)("consistenza").ToString(), CDate(dtRibalta.Rows(i)("dal").ToString()))
'    '        '*** ***
'    '        tempRow(9) = Valore

'    '        'Se ho -1 non ho applicato nessuna sanzione
'    '        tempRow(10) = "-1"  'IDsanzioni
'    '        tempRow(11) = dtRibalta.Rows(i)("IDLEGAME").ToString()   'GrdDichiarato.Items(i).Cells(12).Text 'legame
'    '        tempRow(12) = "-1"
'    '        tempRow(13) = prog  'progressivo
'    '        tempRow(14) = -1
'    '        tempRow(15) = -1
'    '        tempRow(16) = dtRibalta.Rows(i)("percPossesso").ToString() '% possesso

'    '        tempRow(17) = dtRibalta.Rows(i)("TITPOSSESSO").ToString()

'    '        tempRow(18) = dtRibalta.Rows(i)("flag_principale").ToString() 'abit princ

'    '        If dtRibalta.Rows(i)("pertinenza").ToString() = "-1" Then
'    '            pertineza = 1
'    '        Else
'    '            pertineza = dtRibalta.Rows(i)("pertinenza").ToString()
'    '        End If

'    '        tempRow(19) = pertineza 'chkPert
'    '        tempRow(20) = dtRibalta.Rows(i)("flag_ridotto").ToString()  'chkRidotto
'    '        tempRow(21) = ""
'    '        tempRow(22) = dtRibalta.Rows(i)("Sezione").ToString() 'Sezione
'    '        tempRow(23) = dtRibalta.Rows(i)("Via").ToString()  'Ubicazione     
'    '        tempRow(24) = -1 'TitPossesso
'    '        tempRow(25) = -1 'TR
'    '        tempRow(26) = ""
'    '        tempRow(27) = ""
'    '        tempRow(28) = dtRibalta.Rows(i)("NumeroUtilizzatori").ToString()  'NumUtiliz -> numero utilizzatori
'    '        tempRow(29) = "-1" 'ID IMMOBILE ORIGINALE DICHIARATO
'    '        tempRow(30) = dtRibalta.Rows(i)("CodVia").ToString() 'CodVia
'    '        tempRow(31) = False


'    '        workTable.Rows.Add(tempRow)

'    '        Session("DataTableImmobili") = workTable

'    '    Next

'    '    Return prog
'    ' Catch ex As Exception
'    '     Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamenti.PopolaRibaltamentoOld.errore: ", ex)
'    '  Response.Redirect("../../PaginaErrore.aspx")sage)
'    'End Try
'    'End Function


'End Class


