Imports ComPlusInterface
Imports log4net
''' <summary>
''' Pagina per la generazione dei provvedimenti TASI.
''' Contiene le funzioni della comandiera e la griglia per la gestione del dichiarato. 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class SearchDatiDichiaratoTASI
    Inherits BaseEnte
    Protected FncGrd As New Formatta.FunctionGrd
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(SearchDatiDichiaratoTASI))
   
    Private TipoCalcolo As Integer = DichiarazioniICI.CalcoloICI.CalcoloICI.TIPOCalcolo_STANDARD
    Private Progressivo As Integer = 0
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim ListSituazioneFinale() As objSituazioneFinale
        Dim ListDich() As objUIICIAccert
        Dim objHashTable As Hashtable
        Dim sScript As String

        Try
            Progressivo += 1
            'Setto la variabile che mi dice che ho effettuato ricerca in dichiarato
            'Session("cercaDichiarato") = True
            Log.Debug("SearchDatiDichiaratoTASI.entro Progressivo" + Progressivo.ToString)
            ListSituazioneFinale = Nothing
            'Viene Salvata da GestioneAccertamenti.aspx.vb
            'pere avere il codice contribuente e anno di accertamento
            objHashTable = Session("HashTableDichiarazioniAccertamenti")

            If Not Page.IsPostBack Then
                setHashTable(objHashTable)
                '******************************************************************
                'Cerco tutte le dichiarazioni per l'anno selezionato
                '******************************************************************
                Log.Debug("SearchDatiDichiaratoTASI.valorizzato objhastable")
                'Salvo il data set delle dichiarazioni
                'Rifacciamo la query anche se è da rivedere (trovare soluzione
                'ottimale per evitare di rifare ogni volta la query)

                'Dipe Spostata ricerca internamente
                Log.Debug("SearchDatiDichiaratoTASI.devo richiamare getdatiaccertamenti")
                Dim objCOMDichiarazioniAccertamenti As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
                ListDich = objCOMDichiarazioniAccertamenti.GetDatiAccertamenti(ConstSession.StringConnectionICI, ConstSession.StringConnectionOPENgov, ConstSession.IdEnte, Utility.StringOperation.FormatInt(objHashTable("CODCONTRIBUENTE")), objHashTable, ListSituazioneFinale)
                Log.Debug("SearchDatiDichiaratoTASI.richiamato getdatiaccertamenti")
                If Not ListDich Is Nothing Then
                    If ListDich.GetLength(0) > 0 Then
                        'Svuota la Session con la dichiarazione precedente
                        Session.Remove("DataSetDichiarazioni")
                        Session.Remove("DATASET_CALCOLO_ICI_IMMOBILI_DICHIARATI")
                        Session.Add("DATASET_CALCOLO_ICI_IMMOBILI_DICHIARATI", ListSituazioneFinale)
                        Session.Add("DataTableImmobiliDaAccertare", ListDich)
                        Log.Debug("SearchDatiDichiaratoTASI.devo dare GrdDataSource")
                        GrdDichiarato.DataSource = ListDich
                        Log.Debug("SearchDatiDichiaratoTASI.dato GrdDataSource")
                        GrdDichiarato.DataBind()
                        Log.Debug("SearchDatiDichiaratoTASI.fatto GrdDataBind")
                    End If
                    chkSelTutti.Visible = True
                    lblTesto.Visible = True
                End If
                Session("DataSetDichiarazioni") = ListDich

                Log.Debug("SearchDatiDichiaratoTASI.test se visualizzare grd")
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
                sScript = "parent.document.getElementById('loadGridDichiarato').style.display='';"
                sScript += "parent.document.getElementById('attesaCarica').style.display='none';"
                RegisterScript(sScript, Me.GetType)
            End If
            '*** ***
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiDichiarato.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
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

            ListUI = Session("DataTableImmobiliDaAccertare")
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
                sScript += "parent.document.getElementById('loadGridAccertato').src='SearchDatiAccertato.aspx';"
            Else
                sScript += "GestAlert('a', 'warning', '', '', 'Selezionare almeno un immobile per il ribaltamento!');"
            End If
            RegisterScript(sScript, Me.GetType())
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiDichiarato.btnRibaltaImmobiliAcc.errore: ", Err)
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
        Log.Debug("SearchDatiDichiaratoTASI.GrdRowDataBound.IN.Progressivo=" + Progressivo.ToString)
        Try
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                e.Row.Cells(0).BackColor = Color.PaleGoldenrod
                e.Row.Cells(0).Font.Bold = True

                e.Row.Cells(22).ToolTip = "Premere questo pulsante per modificare il legame dell'immobile"
                e.Row.Cells(23).ToolTip = "Premere questo pulsante per eliminare l'immobile"
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiDichiarato.GrdRowDataBound.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
        Log.Debug("SearchDatiDichiaratoTASI.GrdRowDataBound.OUT")
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
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiDichiarato.GrdRowCommand.errore: ", ex)
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
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiDichiarato.chkSelTutti_CheckedChanged.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="objHashTable"></param>
    Private Sub setHashTable(ByRef objHashTable As Hashtable)
        If objHashTable.Contains("TIPO_OPERAZIONE") = False Then
            objHashTable.Add("TIPO_OPERAZIONE", OggettoAtto.Procedimento.Accertamento)
        Else
            objHashTable("TIPO_OPERAZIONE") = OggettoAtto.Procedimento.Accertamento
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
        If objHashTable.Contains("COD_TRIBUTO") = False Then
            objHashTable.Add("COD_TRIBUTO", Utility.Costanti.TRIBUTO_TASI)
        Else
            objHashTable("COD_TRIBUTO") = Utility.Costanti.TRIBUTO_TASI
        End If
        If objHashTable.Contains("TRIBUTOCALCOLO") = False Then
            objHashTable.Add("TRIBUTOCALCOLO", Utility.Costanti.TRIBUTO_TASI)
        Else
            objHashTable("TRIBUTOCALCOLO") = Utility.Costanti.TRIBUTO_TASI
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
        If objHashTable.ContainsKey("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI") Then
            objHashTable("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI") = ConstSession.StringConnection
        Else
            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
        End If
        If objHashTable.ContainsKey("CONNECTIONSTRINGOPENGOVICI") Then
            objHashTable("CONNECTIONSTRINGOPENGOVICI") = ConstSession.StringConnectionICI
        Else
            objHashTable.Add("CONNECTIONSTRINGOPENGOVICI", ConstSession.StringConnectionICI)
        End If
        If objHashTable.ContainsKey("USER") Then
            objHashTable("USER") = ConstSession.UserName
        Else
            objHashTable.Add("USER", ConstSession.UserName)
        End If
        If objHashTable.ContainsKey("CODENTE") Then
            objHashTable("CODENTE") = ConstSession.IdEnte
        Else
            objHashTable.Add("CODENTE", ConstSession.IdEnte)
        End If
        If objHashTable.ContainsKey("CodENTE") Then
            objHashTable("CodENTE") = ConstSession.IdEnte
        Else
            objHashTable.Add("CodENTE", ConstSession.IdEnte)
        End If
        If objHashTable.ContainsKey("CODTIPOPROCEDIMENTO") Then
            objHashTable("CODTIPOPROCEDIMENTO") = "L"
        Else
            objHashTable.Add("CODTIPOPROCEDIMENTO", "L")
        End If
        If objHashTable.ContainsKey("TASIAPROPRIETARIO") Then
            objHashTable("TASIAPROPRIETARIO") = 1
        Else
            objHashTable.Add("TASIAPROPRIETARIO", 1)
        End If
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
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiDichiarato.PopolaRibaltamento.errore:  ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Function
    'Private Function PopolaRibaltamento(ByVal tempRow() As DataRow, ByRef prog As Integer) As Integer
    '    Dim workTable As New DataTable("IMMOBILI")
    '    Dim workTableImmobiliManuali As New DataTable("IMMOBILIMANUALI")

    '    Dim i As Integer = 0
    '    Dim idImmobile As Integer = -1
    '    Dim dt1 As New DataTable
    '    Dim objDBProvv As New DBPROVVEDIMENTI.ProvvedimentiDB
    '    Dim objTipo_Rendita As New DichiarazioniICI.Database.Tipo_RenditaTable(ConstSession.UserName)
    '    Dim Valore As Double

    '    Dim dsImmobili As New DataSet
    '    Dim objHashTable As New Hashtable

    '    Dim LocalObjDSDichiarazioni As DataSet
    '    Dim miodatarow As DataRow()
    '    LocalObjDSDichiarazioni = CType(Session("DataSetDichiarazioni"), DataSet)

    '    Dim pertineza As Integer
    '    Dim riduzione As Integer

    '    Try
    '        If Not Session("DataTableImmobili") Is Nothing Then
    '            workTable = Session("DataTableImmobili")
    '        End If
    '        objHashTable = Session("HashTableDichiarazioniAccertamenti")

    '        If workTable.Rows.Count = 0 Then
    '            workTable.Columns.Add("DAL")                       '0
    '            workTable.Columns.Add("AL")                     '1
    '            workTable.Columns.Add("FOGLIO")                    '2
    '            workTable.Columns.Add("NUMERO")                    '3
    '            workTable.Columns.Add("SUBALTERNO")                   '4
    '            workTable.Columns.Add("CATEGORIA")                     '5
    '            workTable.Columns.Add("CLASSE")                    '6
    '            workTable.Columns.Add("CONSISTENZA")                     '7
    '            workTable.Columns.Add("TR")                     '8
    '            workTable.Columns.Add("RENDITA")                      '9
    '            workTable.Columns.Add("RENDITA_VALORE")                  '10
    '            workTable.Columns.Add("ZONA")                        '11
    '            workTable.Columns.Add("IDSANZIONI")                   '12
    '            workTable.Columns.Add("IDLEGAME", System.Type.GetType("System.Int32"))                      '13
    '            workTable.Columns.Add("ICICALCOLATO")                      '14
    '            workTable.Columns.Add("PROGRESSIVO")                     '15
    '            workTable.Columns.Add("SANZIONI")                       '16
    '            workTable.Columns.Add("INTERESSI")                     '17
    '            workTable.Columns.Add("PERCPOSSESSO")                      '18
    '            workTable.Columns.Add("TITPOSSESSO")                     '19
    '            workTable.Columns.Add("FLAG_PRINCIPALE")                    '20
    '            workTable.Columns.Add("FLAG_PERTINENZA")                    '21
    '            workTable.Columns.Add("FLAG_RIDOTTO")                      '22
    '            workTable.Columns.Add("IDIMMOBILEPERTINENZA")                    '23
    '            workTable.Columns.Add("SEZIONE")                      '24
    '            workTable.Columns.Add("INDIRIZZO")                     '25
    '            workTable.Columns.Add("CODTITPOSSESSO")                  '26
    '            workTable.Columns.Add("CODTIPORENDITA")                  '27
    '            workTable.Columns.Add("ICICALCOLATOACCONTO")                   '28
    '            workTable.Columns.Add("ICICALCOLATOSALDO")                   '29
    '            workTable.Columns.Add("NUMEROUTILIZZATORI")                 '30
    '            workTable.Columns.Add("ID_IMMOBILE_ORIGINALE_DICHIARATO")                 '31
    '            workTable.Columns.Add("CODICE_VIA")                   '32
    '            workTable.Columns.Add("CALCOLA_INTERESSI")                   '33
    '            workTable.Columns.Add("DESC_TIPO_RENDITA")                   '34
    '            workTable.Columns.Add("DESC_SANZIONE")                    '35
    '            workTable.Columns.Add("ID_IMMOBILE_ACCERTATO")                  '36
    '            workTable.Columns.Add("CODCOMUNE")                     '37
    '            workTable.Columns.Add("COMUNE")                    '38
    '            workTable.Columns.Add("ESPCIVICO")                     '39
    '            workTable.Columns.Add("INTERNO")                      '40
    '            workTable.Columns.Add("NUMEROCIVICO")                      '41
    '            workTable.Columns.Add("PIANO")                      '42
    '            workTable.Columns.Add("SCALA")                      '43
    '            workTable.Columns.Add("INTERNO1")                       '44
    '            workTable.Columns.Add("BARRATO")                      '45
    '            workTable.Columns.Add("MESIESCLUSIONEESENZIONE")                  '46
    '            workTable.Columns.Add("FLAG_ESENTE")                     '47
    '            workTable.Columns.Add("MESIRIDUZIONE")                    '48

    '            workTable.Columns.Add("ICI_VALORE_ALIQUOTA")                   '49
    '            workTable.Columns.Add("diffimposta")                     '50
    '            workTable.Columns.Add("totale")                    '51

    '            '*** 20120530 - IMU ***
    '            workTable.Columns.Add("COLTIVATOREDIRETTO")             '52
    '            workTable.Columns.Add("NUMEROFIGLI")                '53
    '            workTable.Columns.Add("PERCENTCARICOFIGLI")             '54
    '            '*** ***
    '            '*** 20140509 - TASI ***
    '            workTable.Columns.Add("CODTRIBUTO")
    '            '*** ***
    '            'Session("DataTableImmobiliDic") = workTable
    '            Session("DataTableImmobili") = workTable
    '        End If

    '        prog = prog + 1

    '        Dim newtempRow As DataRow
    '        newtempRow = workTable.NewRow()

    '        If tempRow(0).ItemArray(0).ToString() <> "" Then
    '            newtempRow(0) = CDate(tempRow(0).ItemArray(0).ToString())               'dal
    '        Else
    '            newtempRow(0) = ""
    '        End If

    '        If tempRow(0).ItemArray(1).ToString() <> "" Then
    '            newtempRow(1) = CDate(tempRow(0).ItemArray(1).ToString())               'al
    '        Else
    '            newtempRow(1) = ""
    '        End If

    '        newtempRow(2) = tempRow(0).ItemArray(2).ToString()           'foglio
    '        newtempRow(3) = tempRow(0).ItemArray(3).ToString()            'numero
    '        newtempRow(4) = tempRow(0).ItemArray(4).ToString()           'sub
    '        newtempRow(5) = tempRow(0).ItemArray(5).ToString()           'Categoria
    '        newtempRow(6) = tempRow(0).ItemArray(6).ToString()           ' Classe
    '        newtempRow(7) = tempRow(0).ItemArray(7).ToString()           'Consistenza
    '        newtempRow(8) = tempRow(0).ItemArray(8).ToString()           'TR

    '        newtempRow(9) = tempRow(0).ItemArray(9).ToString()           'rendita
    '        newtempRow(11) = tempRow(0).ItemArray(11).ToString()             'zona
    '        If tempRow(0).ItemArray(9).ToString() <> "" Then
    '            '*** 20120709 - IMU per AF e LC devo usare il campo valore ***
    '            Dim ValoreDich As Double = 0
    '            If tempRow(0).ItemArray(10).ToString() > 0 Then
    '                ValoreDich = tempRow(0).ItemArray(10)
    '            End If
    '            'Valore = objDBProvv.CalcoloValoredaRendita(tempRow(0).ItemArray(9).ToString(), tempRow(0).ItemArray(8).ToString(), tempRow(0).ItemArray(5).ToString(), objHashTable("ANNOACCERTAMENTO").ToString())
    '            '*** 20120530 - IMU ***
    '            Dim FncValore As New ComPlusInterface.FncICI
    '            Try
    '                Valore = FncValore.CalcoloValore(ConstSession.StringConnectionOPENgov, ConstSession.StringConnectionICI, ConstSession.IdEnte, Year(tempRow(0).ItemArray(0).ToString()), tempRow(0).ItemArray(8).ToString(), tempRow(0).ItemArray(5).ToString(), tempRow(0).ItemArray(6).ToString(), tempRow(0).ItemArray(11).ToString(), tempRow(0).ItemArray(9).ToString(), ValoreDich, tempRow(0).ItemArray(7).ToString(), CDate(tempRow(0).ItemArray(0).ToString()), tempRow(0).ItemArray(54).ToString())
    '            Catch ex As Exception
    '                Log.Debug("Popolaribaltamentocalcolo valore: " & ex.Message)
    '            End Try
    '            '*** ***
    '            If Valore.ToString() <> tempRow(0).ItemArray(10).ToString() And Valore <> 0 Then
    '                newtempRow(10) = Valore.ToString()                 'valore
    '            Else
    '                newtempRow(10) = tempRow(0).ItemArray(10).ToString()                   'valore
    '            End If
    '            If CDbl(tempRow(0).ItemArray(9)) = 0 Then
    '                'calcolo rendita da valore
    '                Valore = objDBProvv.CalcoloRenditadaValore(tempRow(0).ItemArray(10).ToString(), tempRow(0).ItemArray(8).ToString(), tempRow(0).ItemArray(5).ToString(), CDate(tempRow(0).ItemArray(0).ToString()).Year.ToString)
    '                newtempRow(9) = Valore.ToString()                  'rendita
    '            End If
    '        Else
    '            newtempRow(10) = tempRow(0).ItemArray(10).ToString()
    '            'calcolo rendita da valore
    '            Valore = objDBProvv.CalcoloRenditadaValore(tempRow(0).ItemArray(10).ToString(), tempRow(0).ItemArray(8).ToString(), tempRow(0).ItemArray(5).ToString(), CDate(tempRow(0).ItemArray(0).ToString()).Year.ToString)
    '            newtempRow(9) = Valore.ToString()               'rendita
    '        End If

    '        newtempRow(12) = -1          'IDsanzioni
    '        newtempRow(13) = tempRow(0).ItemArray(13).ToString()             'legame
    '        newtempRow(14) = "-1"           'ICICalcolato
    '        newtempRow(15) = prog           'progressivo
    '        newtempRow(16) = -1           'sanzioni 
    '        newtempRow(17) = 1            'interessi
    '        newtempRow(18) = tempRow(0).ItemArray(18).ToString()             '% possesso
    '        newtempRow(19) = tempRow(0).ItemArray(19).ToString()             'titPossesso
    '        newtempRow(20) = tempRow(0).ItemArray(20).ToString()             'flag_principale abit princ

    '        If tempRow(0).ItemArray(22).ToString() = "0" Then
    '            riduzione = 1
    '        Else
    '            riduzione = 0
    '        End If

    '        newtempRow(48) = tempRow(0).ItemArray(50).ToString()             ' mesi riduzione
    '        If tempRow(0).ItemArray(50) > 0 Then
    '            riduzione = 1
    '        Else
    '            riduzione = 0
    '        End If

    '        If tempRow(0).ItemArray(20).ToString() = "1" Then
    '            pertineza = 0
    '            newtempRow(23) = ""
    '        Else
    '            'se pertinenza ha valore 0 o -1 -> l'immobile non è una pertinenza
    '            'se invece è valorizzata, contiene l'id dell'immobile
    '            If tempRow(0).ItemArray(23).ToString() = "-1" Or tempRow(0).ItemArray(23).ToString() = "0" Then              'pertinenza
    '                pertineza = 0
    '                newtempRow(23) = ""
    '            Else
    '                pertineza = 1
    '                miodatarow = LocalObjDSDichiarazioni.Tables(0).Select("ID='" & tempRow(0).ItemArray(23).ToString() & "'")
    '                If miodatarow.Length = 0 Then
    '                    newtempRow(23) = tempRow(0).ItemArray(13).ToString()                         'miodatarow(i).Item("idlegame")

    '                Else
    '                    newtempRow(23) = tempRow(0).ItemArray(23).ToString()                         'id_immobile_pertinenza
    '                End If

    '            End If
    '        End If

    '        newtempRow(21) = pertineza           'chkPert
    '        newtempRow(22) = riduzione           'chkRidotto

    '        newtempRow(24) = tempRow(0).ItemArray(24).ToString()             'Sezione
    '        newtempRow(25) = tempRow(0).ItemArray(25).ToString()              'Ubicazione     
    '        newtempRow(26) = tempRow(0).ItemArray(26).ToString()             '-1 'TitPossesso

    '        Dim TpRendita As DataTable
    '        TpRendita = objTipo_Rendita.List()
    '        Dim iTR As Integer = -1
    '        If TpRendita.Rows.Count > 0 Then
    '            For i = 0 To TpRendita.Rows.Count - 1
    '                If (TpRendita.Rows(i)("SIGLA").ToString() = tempRow(0).ItemArray(8).ToString()) Then
    '                    iTR = TpRendita.Rows(i)("COD_RENDITA")
    '                    Exit For
    '                End If
    '            Next
    '        End If

    '        newtempRow(27) = iTR             'Codice TR
    '        newtempRow(28) = ""          'iciCalcolatoACCONTO
    '        newtempRow(29) = ""          'iciCalcolatoSALDO
    '        newtempRow(30) = tempRow(0).ItemArray(30).ToString()             'NumUtiliz -> numero utilizzatori
    '        newtempRow(31) = tempRow(0).ItemArray(31).ToString()             'ID IMMOBILE ORIGINALE DICHIARATO
    '        newtempRow(32) = tempRow(0).ItemArray(32).ToString()             'CodVia
    '        newtempRow(33) = True            'calcola interessi
    '        newtempRow(34) = tempRow(0).ItemArray(34).ToString()             'desc tipo rendita
    '        newtempRow(35) = ""          'desc sanzione
    '        newtempRow(36) = -1          'id immobile accertato

    '        newtempRow(37) = tempRow(0).ItemArray(37).ToString()             'codcomune
    '        newtempRow(38) = tempRow(0).ItemArray(38).ToString()             'comune
    '        newtempRow(39) = tempRow(0).ItemArray(39).ToString()             'EspCivico
    '        newtempRow(40) = tempRow(0).ItemArray(40).ToString()             'interno
    '        newtempRow(41) = tempRow(0).ItemArray(41).ToString()             'numero civico
    '        newtempRow(42) = tempRow(0).ItemArray(42).ToString()             'piano
    '        newtempRow(43) = tempRow(0).ItemArray(43).ToString()             'scala
    '        newtempRow(44) = tempRow(0).ItemArray(44).ToString()             'interno1
    '        newtempRow(45) = tempRow(0).ItemArray(45).ToString()             'barrato
    '        newtempRow(46) = tempRow(0).ItemArray(46).ToString()             'mesiEsclusioneEsenzione
    '        newtempRow(47) = tempRow(0).ItemArray(47).ToString()             'EsclusioneEsenzione
    '        'dipe 03/03/2011
    '        newtempRow(49) = tempRow(0).ItemArray(51).ToString()             'ICI_VALORE_ALIQUOTA
    '        newtempRow(50) = tempRow(0).ItemArray(52).ToString()             'diffimposta
    '        newtempRow(51) = tempRow(0).ItemArray(53).ToString()             'totale

    '        '*** 20120530 - IMU ***
    '        newtempRow(52) = tempRow(0).ItemArray(54).ToString()             'COLTIVATOREDIRETTO
    '        newtempRow(53) = tempRow(0).ItemArray(55).ToString()             'NUMEROFIGLI
    '        newtempRow(54) = tempRow(0).ItemArray(56).ToString()             'PERCENTCARICOFIGLI
    '        '*** ***
    '        '*** 20140509 - TASI ***
    '        newtempRow("CODTRIBUTO") = tempRow(0).ItemArray(57).ToString
    '        '*** ***
    '        workTable.Rows.Add(newtempRow)

    '        Session("DataTableImmobili") = workTable
    '        Return prog
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiDichiarato.PopolaRibaltamento.errore:  ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Function
    'Private Function CalcolaICI(ByVal objHashTable As Hashtable, ByVal Id_Immobile As String, ByVal objICI() As objSituazioneFinale) As Double
    '    Return addRowsCalcoloICI(objICI, objHashTable, Id_Immobile)
    'End Function
    'Private Sub CalcolaICITotale(ByRef objICI() As objSituazioneFinale, ByVal objHashTable As Hashtable, ByVal TipoCalcolo As Integer)
    '    Try
    '        '*** 20140509 - TASI ***
    '        'Calcolo 
    '        Dim objCOMCalcoloICI As IFreezer = Activator.GetObject(GetType(ComPlusInterface.IFreezer), ConstSession.URLServiziFreezer)
    '        objICI = objCOMCalcoloICI.CalcoloICI(objICI, objHashTable, TipoCalcolo)
    '        '*** ***
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiDichiarato.CalcolaICITotale.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    'Private Function addRowsCalcoloICI(ByRef objICI() As objSituazioneFinale, ByVal objHashTable As Hashtable, ByVal Id_Immobile As String) As Double
    '    Dim row As New objSituazioneFinale
    '    'Dim newTable As DataTable
    '    'newTable = objICI.Tables(0).Copy
    '    Dim objDSImmobili As DataSet
    '    Dim clsGeneralFunction As New MyUtility
    '    Dim FncGen As New ClsGestioneAccertamenti
    '    Try
    '        '******************************************************************
    '        'Cerco tutte le dichiarazioni per l'anno selezionato
    '        '******************************************************************
    '        Log.Debug("addRowsCalcoloICI::richiamo GetImmobiliDichiaratoVirtualeICI")
    '        Dim objCOMDichiarazioniAccertamenti As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
    '        objDSImmobili = objCOMDichiarazioniAccertamenti.GetImmobiliDichiaratoVirtualeICI(objHashTable)
    '        Log.Debug("addRowsCalcoloICI::richiamato GetImmobiliDichiaratoVirtualeICI")
    '        Dim i As Integer = 0
    '        Dim nUtilizzatori As Integer

    '        Dim rowImmobile() As DataRow

    '        rowImmobile = objDSImmobili.Tables(0).Select("ID_IMMOBILE='" & Id_Immobile & "'")
    '        If IsDBNull(rowImmobile(0).Item("NumeroUtilizzatori")) Then
    '            nUtilizzatori = 0
    '        ElseIf CStr(rowImmobile(0).Item("NumeroUtilizzatori")).CompareTo("") = 0 Then
    '            nUtilizzatori = 0
    '        Else
    '            nUtilizzatori = rowImmobile(0).Item("NumeroUtilizzatori")
    '        End If
    '        row.NUtilizzatori = nUtilizzatori

    '        If Not IsDBNull(rowImmobile(0).Item("COD_CONTRIBUENTE")) Then
    '            row.IdContribuente = rowImmobile(0).Item("COD_CONTRIBUENTE")
    '        End If
    '        If Not IsDBNull(rowImmobile(0).Item("ANNO")) Then
    '            row.Anno = rowImmobile(0).Item("ANNO")
    '        End If
    '        If Not IsDBNull(rowImmobile(0).Item("NUMERO_MESI_ACCONTO")) Then
    '            row.AccMesi = rowImmobile(0).Item("NUMERO_MESI_ACCONTO")
    '        End If
    '        If Not IsDBNull(rowImmobile(0).Item("NUMERO_MESI_TOTALI")) Then
    '            row.Mesi = rowImmobile(0).Item("NUMERO_MESI_TOTALI")
    '        End If
    '        If Not IsDBNull(rowImmobile(0).Item("PERC_POSSESSO")) Then
    '            row.PercPossesso = rowImmobile(0).Item("PERC_POSSESSO")
    '        End If
    '        If Not IsDBNull(rowImmobile(0).Item("RIDUZIONE")) Then
    '            row.FlagRiduzione = rowImmobile(0).Item("RIDUZIONE")
    '        End If

    '        If Not IsDBNull(rowImmobile(0).Item("TIPO_RENDITA")) Then
    '            row.TipoRendita = rowImmobile(0).Item("TIPO_RENDITA")
    '        End If
    '        '*** 20140509 - TASI ***
    '        'row.TIPO_POSSESSO") = rowImmobile(0).Item("TIPOPOSSESSO")
    '        If Not IsDBNull(rowImmobile(0).Item("IDTIPOUTILIZZO")) Then
    '            row.IdTipoUtilizzo = rowImmobile(0).Item("IDTIPOUTILIZZO")
    '        End If
    '        If Not IsDBNull(rowImmobile(0).Item("IDTIPOPOSSESSO")) Then
    '            row.IdTipoPossesso = rowImmobile(0).Item("IDTIPOPOSSESSO")
    '        End If
    '        row.Tributo = Utility.Costanti.TRIBUTO_ICI
    '        If Not IsDBNull(rowImmobile(0).Item("ZONA")) Then
    '            row.Zona = rowImmobile(0).Item("ZONA")
    '        End If
    '        If Not IsDBNull(rowImmobile(0).Item("datainizio")) Then
    '            row.DataInizio = clsGeneralFunction.ReplaceDataForDB(rowImmobile(0).Item("datainizio"))
    '        End If
    '        '*** ***
    '        If Not IsDBNull(rowImmobile(0).Item("ENTE")) Then
    '            row.IdEnte = rowImmobile(0).Item("ENTE")
    '        End If
    '        row.IdProcedimento = 0
    '        row.IdRiferimento = 0
    '        row.Provenienza = "D"
    '        If Not IsDBNull(rowImmobile(0).Item("CARATTERISTICA")) Then
    '            row.Caratteristica = rowImmobile(0).Item("CARATTERISTICA")
    '        End If
    '        If Not IsDBNull(rowImmobile(0).Item("VIA")) Then
    '            row.Indirizzo = rowImmobile(0).Item("VIA")
    '        End If
    '        If Not IsDBNull(rowImmobile(0).Item("NUMEROCIVICO")) Then
    '            row.Indirizzo += " " & rowImmobile(0).Item("NUMEROCIVICO")
    '        End If
    '        If Not IsDBNull(rowImmobile(0).Item("SEZIONE")) Then
    '            row.Sezione = rowImmobile(0).Item("SEZIONE")
    '        End If
    '        If Not IsDBNull(rowImmobile(0).Item("FOGLIO")) Then
    '            row.Foglio = rowImmobile(0).Item("FOGLIO")
    '        End If
    '        If Not IsDBNull(rowImmobile(0).Item("NUMERO")) Then
    '            row.Numero = rowImmobile(0).Item("NUMERO")
    '        End If
    '        If Not IsDBNull(rowImmobile(0).Item("SUBALTERNO")) Then
    '            row.Subalterno = rowImmobile(0).Item("SUBALTERNO")
    '        End If
    '        If Not IsDBNull(rowImmobile(0).Item("CODCATEGORIACATASTALE")) Then
    '            row.Categoria = rowImmobile(0).Item("CODCATEGORIACATASTALE")
    '        End If
    '        If Not IsDBNull(rowImmobile(0).Item("CODCLASSE")) Then
    '            row.Classe = rowImmobile(0).Item("CODCLASSE")
    '        End If
    '        row.Protocollo = 0
    '        If Not IsDBNull(rowImmobile(0).Item("STORICO")) Then
    '            row.FlagStorico = rowImmobile(0).Item("STORICO")
    '        End If
    '        If Not IsDBNull(rowImmobile(0).Item("FLAGVALOREPROVV")) Then
    '            row.FlagProvvisorio = rowImmobile(0).Item("FLAGVALOREPROVV")
    '        End If
    '        If Not IsDBNull(rowImmobile(0).Item("MESIPOSSESSO")) Then
    '            row.MesiPossesso = rowImmobile(0).Item("MESIPOSSESSO")
    '        End If
    '        If Not IsDBNull(rowImmobile(0).Item("MESIESCLUSIONEESENZIONE")) Then
    '            row.MesiEsenzione = rowImmobile(0).Item("MESIESCLUSIONEESENZIONE")
    '        End If
    '        If Not IsDBNull(rowImmobile(0).Item("MESIRIDUZIONE")) Then
    '            row.MesiRiduzione = rowImmobile(0).Item("MESIRIDUZIONE")
    '        End If
    '        If Not IsDBNull(rowImmobile(0).Item("IMPDETRAZABITAZPRINCIPALE")) Then
    '            row.ImpDetrazione = rowImmobile(0).Item("IMPDETRAZABITAZPRINCIPALE")
    '        End If
    '        If Not IsDBNull(rowImmobile(0).Item("POSSESSO")) Then
    '            row.FlagPosseduto = rowImmobile(0).Item("POSSESSO")
    '        End If
    '        If Not IsDBNull(rowImmobile(0).Item("ESENTE_ESCLUSO")) Then
    '            row.FlagEsente = rowImmobile(0).Item("ESENTE_ESCLUSO")
    '        End If
    '        If Not IsDBNull(rowImmobile(0).Item("esclusioneesenzione")) And Not IsDBNull(row.MesiEsenzione) Then
    '            If rowImmobile(0).Item("esclusioneesenzione") = 2 And row.MesiEsenzione > 0 Then
    '                row.FlagEsente = 0
    '            End If
    '        End If
    '        If Not IsDBNull(rowImmobile(0).Item("RIDUZIONE")) Then
    '            row.FlagRiduzione = rowImmobile(0).Item("RIDUZIONE")
    '        End If
    '        If Not IsDBNull(rowImmobile(0).Item("ID")) Then
    '            row.IdImmobile = rowImmobile(0).Item("ID")
    '        End If
    '        If Not IsDBNull(rowImmobile(0).Item("IDIMMOBILEPERTINENTE")) Then
    '            row.IdImmobilePertinenza = rowImmobile(0).Item("IDIMMOBILEPERTINENTE")
    '        End If

    '        If Not IsDBNull(rowImmobile(0).Item("datainizio")) Then
    '            row.Dal = clsGeneralFunction.GiraData(rowImmobile(0).Item("datainizio"))
    '        End If
    '        If Not IsDBNull(rowImmobile(0).Item("datafine")) Then
    '            row.Al = clsGeneralFunction.GiraData(rowImmobile(0).Item("datafine"))
    '        End If

    '        row.MeseInizio = 0
    '        row.DataScadenza = ""
    '        row.TipoOperazione = objHashTable("TIPO_OPERAZIONE")

    '        If Not IsDBNull(rowImmobile(0).Item("rendita")) Then
    '            row.Rendita = rowImmobile(0).Item("rendita")
    '        End If

    '        If Boolean.Parse(rowImmobile(0).Item("FLAG_PRINCIPALE").ToString) = True Then
    '            row.FlagPrincipale = 1
    '        Else
    '            If rowImmobile(0).Item("IDIMMOBILEPERTINENTE").ToString.Length > 0 Then
    '                If rowImmobile(0).Item("IDIMMOBILEPERTINENTE").ToString = "-1" Then
    '                    row.FlagPrincipale = 0
    '                Else
    '                    row.FlagPrincipale = 2
    '                End If
    '            ElseIf rowImmobile(0).Item("IDIMMOBILEPERTINENTE").ToString.Length = 0 Then
    '                row.FlagPrincipale = 0
    '            End If
    '        End If

    '        row.Id = Progressivo

    '        If Not IsDBNull(rowImmobile(0).Item("ICI_VALORE_ALIQUOTA")) Then
    '            row.Aliquota = rowImmobile(0).Item("ICI_VALORE_ALIQUOTA")
    '        End If
    '        If Not IsDBNull(rowImmobile(0).Item("diffimposta")) Then
    '            row.diffimposta = rowImmobile(0).Item("diffimposta")
    '        End If
    '        If Not IsDBNull(rowImmobile(0).Item("totale")) Then
    '            row.totale = rowImmobile(0).Item("totale")
    '        End If

    '        'row.VALORE") = rowImmobile(0).Item("VALORE")
    '        '*** 20120530 - IMU ***
    '        'devo ricalcolare il valore aggiornato
    '        Dim FncValore As New ComPlusInterface.FncICI
    '        Dim sClasse As String = ""
    '        Dim sZona As String = ""
    '        Dim sCategoria As String = ""
    '        Dim nConsistenza As Double = 0
    '        Dim nRendita As Double = 0
    '        If Not IsDBNull(rowImmobile(0).Item("CODCLASSE")) Then
    '            sClasse = rowImmobile(0).Item("CODCLASSE")
    '        End If
    '        If Not IsDBNull(rowImmobile(0).Item("ZONA")) Then
    '            sZona = rowImmobile(0).Item("ZONA")
    '        End If
    '        If Not IsDBNull(rowImmobile(0).Item("CONSISTENZA")) Then
    '            nConsistenza = rowImmobile(0).Item("CONSISTENZA")
    '        End If
    '        If Not IsDBNull(rowImmobile(0).Item("CODCATEGORIACATASTALE")) Then
    '            sCategoria = rowImmobile(0).Item("CODCATEGORIACATASTALE")
    '        End If
    '        If Not IsDBNull(rowImmobile(0).Item("RENDITA")) Then
    '            If CStr(rowImmobile(0).Item("RENDITA")) <> "" Then
    '                nRendita = rowImmobile(0).Item("RENDITA")
    '            Else
    '                nRendita = 0
    '            End If
    '        Else
    '            nRendita = 0
    '        End If
    '        '*** ***
    '        If Not IsDBNull(rowImmobile(0).Item("COLTIVATOREDIRETTO")) Then
    '            row.IsColtivatoreDiretto = rowImmobile(0).Item("COLTIVATOREDIRETTO")
    '        End If
    '        If Not IsDBNull(rowImmobile(0).Item("NUMEROFIGLI")) Then
    '            row.NumeroFigli = rowImmobile(0).Item("NUMEROFIGLI")
    '        End If
    '        If Not IsDBNull(rowImmobile(0).Item("PERCENTCARICOFIGLI")) Then
    '            row.PercentCaricoFigli = rowImmobile(0).Item("PERCENTCARICOFIGLI")
    '        End If
    '        '*** 20120709 - IMU per AF e LC devo usare il campo valore ***
    '        Dim ValoreDich As Double = 0
    '        If Not IsDBNull(rowImmobile(0).Item("VALORE")) Then
    '            ValoreDich = rowImmobile(0).Item("VALORE")
    '        End If
    '        row.Valore = FncValore.CalcoloValore(ConstSession.StringConnectionOPENgov, ConstSession.StringConnectionICI, ConstSession.IdEnte, Year(rowImmobile(0).Item("datainizio")), rowImmobile(0).Item("TIPO_RENDITA"), sCategoria, sClasse, sZona, nRendita, ValoreDich, nConsistenza, CDate(rowImmobile(0).Item("datainizio")), row.IsColtivatoreDiretto)
    '        If row.VALORE <= 0 Then
    '            row.Valore = rowImmobile(0).Item("VALORE")
    '        End If
    '        '*** ***
    '        '*** 20140509 - TASI ***
    '        If Not IsDBNull(rowImmobile(0).Item("VALORE")) Then
    '            row.ValoreReale = row.Valore
    '        End If
    '        '*** ***
    '        Progressivo += 1
    '        'newTable.Rows.Add(row)
    '        'newTable.AcceptChanges()

    '        'Aggiungo Riga a mio DS x Situazione Finali ICI
    '        Dim myArray As New ArrayList(objICI)
    '        myArray.Add(row)
    '        objICI = CType(myArray.ToArray(GetType(objSituazioneFinale)), objSituazioneFinale())
    '        'objICI.Tables(0).ImportRow(row)
    '        'objICI.Tables(0).AcceptChanges()

    '        'Mi serve per calcolare l'ICI solo su 1 Immobile. Viene passato alla procedura di calcolo
    '        'dell'ICI. Viene poi distrutto all'uscita del metodo
    '        'Dim objDSImmobiliAppoggio As DataSet
    '        'Creo la struttura 
    '        'objDSImmobiliAppoggio = FncGen.CreateDSperCalcoloICI()
    '        'objDSImmobiliAppoggio.Tables("TP_SITUAZIONE_FINALE_ICI").ImportRow(row)
    '        'objDSImmobiliAppoggio.AcceptChanges()
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiDichiarato.addRowsCalcoloICI.errore  ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Function
End Class