Imports Anagrafica.DLL
Imports log4net
Imports RemotingInterfaceMotoreTarsu.RemotingInterfaceMotoreTarsu
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
''' <summary>
''' Pagina per la generazione dei provvedimenti TARI.
''' Contiene le funzioni della comandiera e la griglia per la gestione del dichiarato. 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class SearchDatiAccertamentiTARSU
    Inherits BasePage
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(SearchDatiAccertamentiTARSU))
   
    'Private idCelle As New DataGridIndex
    Private Progressivo As Integer = 0
    'Private objTARSU As New OPENgovTARSU.SalvataggioRuolo.SalvataggioRuolo
    Private workTable As New DataTable("IMMOBILI")
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

    '*** 20140701 - IMU/TARES ***
    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    'Put user code to initialize the page here
    '    Dim dw As DataView
    '    Dim objDSDichiarazioni As DataSet
    '    Dim blnResult As Boolean = False
    '    Dim objHashTable As Hashtable
    '    'Dim objSessione As CreateSessione
    '    Dim strWFErrore As String
    '    Dim objSessioneTARSU As OPENUtility.CreateSessione
    '    Dim objRiepilogoContribuente() As OPENgovTARSU.ObjRiepilogoAnnoRuoloSingleContrib
    '    Dim intArticoli As Integer
    '    'Dim oAccertamento() As OggettoArticoloRuoloAccertamento
    '    'Dim oAccertamentoSingolo As OggettoArticoloRuoloAccertamento
    '    Dim oAccertamento() As OggettoArticoloRuolo
    '    Dim oAccertamentoSingolo As OggettoArticoloRuolo
    '    Dim intProgressivo As Integer
    '    Dim intDet As Integer
    '    Dim intRid As Integer
    '    Dim ArrayDetassazioni() As OggettoDetassazione
    '    Dim objDet As OggettoDetassazione
    '    Dim ArrayRiduzioni() As OggettoRiduzione
    '    Dim objRid As OggettoRiduzione

    '    Try
    '        ''Setto la variabile che mi dice che ho effettuato ricerca in dichiarato
    '        objSessione = New CreateSessione(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))
    '        If Not objSessione.CreaSessione(ConstSession.UserName, strWFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        'creo sessione per il db della TARSU
    '        objSessioneTARSU = New OPENUtility.CreateSessione(Session("PARAMETROENV"), ConstSession.UserName, ConfigurationManager.AppSettings("OPENGOVTA"))
    '        If Not objSessioneTARSU.CreaSessione(ConstSession.UserName, strWFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        'Viene Salvata da GestioneAccertamenti.aspx.vb
    '        'per avere il codice contribuente e anno di accertamento
    '        objHashTable = Session("HashTableDichAccertamentiTARSU")

    '        If Not Page.IsPostBack Then
    '            If Not Session("DataSetDichiarazioni") Is Nothing Then
    '                oAccertamento = CType(Session("DataSetDichiarazioni"), OggettoArticoloRuolo())
    '                'oAccertamento = CType(Session("DataSetDichiarazioni"), OggettoArticoloRuoloAccertamento())
    '                GrdDichiarato.start_index = 0
    '                GrdDichiarato.DataSource = oAccertamento
    '                GrdDichiarato.DataBind()
    '            Else
    '                SetHashTable(objHashTable, objSessione)
    '                '******************************************************************
    '                'Cerco tutte le dichiarazioni per l'anno selezionato
    '                '******************************************************************
    '                objRiepilogoContribuente = objTARSU.CalcolaRuoloFromDich(Session("COD_ENTE"), -1, objHashTable("ANNOACCERTAMENTO"), objHashTable("ANNOACCERTAMENTO"), "O", "", "A", objSessioneTARSU, objHashTable("CODCONTRIBUENTE"), True)
    '                'Salvo il data set delle dichiarazioni
    '                'Rifacciamo la query anche se è da rivedere (trovare soluzione
    '                'ottimale per evitare di rifare ogni volta la query)
    '                'Session("DSDichiarazioniAccertamenti") = objDSDichiarazioni
    '                intProgressivo = 0
    '                If Not objRiepilogoContribuente Is Nothing Then
    '                    If objRiepilogoContribuente.Length > 0 Then
    '                        If Not objRiepilogoContribuente(0).oArticoli Is Nothing Then
    '                            If objRiepilogoContribuente(0).oArticoli.Length > 0 Then
    '                                Dim arrayListImmobili As New ArrayList

    '                                For intArticoli = 0 To objRiepilogoContribuente(0).oArticoli.Length - 1
    '                                    ReDim Preserve oAccertamento(intArticoli)
    '                                    'oAccertamentoSingolo = New OggettoArticoloRuoloAccertamento
    '                                    oAccertamentoSingolo = New OggettoArticoloRuolo
    '                                    oAccertamentoSingolo.Anno = objRiepilogoContribuente(0).oArticoli(intArticoli).Anno
    '                                    oAccertamentoSingolo.Categoria = objRiepilogoContribuente(0).oArticoli(intArticoli).Categoria
    '                                    oAccertamentoSingolo.Civico = objRiepilogoContribuente(0).oArticoli(intArticoli).Civico
    '                                    oAccertamentoSingolo.CodCartella = objRiepilogoContribuente(0).oArticoli(intArticoli).CodCartella
    '                                    oAccertamentoSingolo.CodVia = objRiepilogoContribuente(0).oArticoli(intArticoli).CodVia
    '                                    oAccertamentoSingolo.DaAccertamento = objRiepilogoContribuente(0).oArticoli(intArticoli).DaAccertamento
    '                                    oAccertamentoSingolo.DataFine = objRiepilogoContribuente(0).oArticoli(intArticoli).DataFine
    '                                    oAccertamentoSingolo.DataInizio = objRiepilogoContribuente(0).oArticoli(intArticoli).DataInizio
    '                                    oAccertamentoSingolo.DataVariazione = objRiepilogoContribuente(0).oArticoli(intArticoli).DataVariazione
    '                                    oAccertamentoSingolo.DescrDiffImposta = objRiepilogoContribuente(0).oArticoli(intArticoli).DescrDiffImposta
    '                                    oAccertamentoSingolo.DescrInteressi = objRiepilogoContribuente(0).oArticoli(intArticoli).DescrInteressi
    '                                    oAccertamentoSingolo.DescrSanzioni = objRiepilogoContribuente(0).oArticoli(intArticoli).DescrSanzioni
    '                                    oAccertamentoSingolo.DescrSpeseNotifica = objRiepilogoContribuente(0).oArticoli(intArticoli).DescrSpeseNotifica
    '                                    oAccertamentoSingolo.Ente = objRiepilogoContribuente(0).oArticoli(intArticoli).Ente
    '                                    oAccertamentoSingolo.Esponente = objRiepilogoContribuente(0).oArticoli(intArticoli).Esponente
    '                                    oAccertamentoSingolo.Foglio = objRiepilogoContribuente(0).oArticoli(intArticoli).Foglio
    '                                    oAccertamentoSingolo.Id = objRiepilogoContribuente(0).oArticoli(intArticoli).Id
    '                                    oAccertamentoSingolo.IdArticoloRuolo = objRiepilogoContribuente(0).oArticoli(intArticoli).IdArticoloRuolo
    '                                    oAccertamentoSingolo.IdContribuente = objRiepilogoContribuente(0).oArticoli(intArticoli).IdContribuente
    '                                    oAccertamentoSingolo.IdDettaglioTestata = objRiepilogoContribuente(0).oArticoli(intArticoli).IdDettaglioTestata
    '                                    oAccertamentoSingolo.IdTestata = objRiepilogoContribuente(0).oArticoli(intArticoli).IdTestata
    '                                    Session("IdTestata") = objRiepilogoContribuente(0).oArticoli(intArticoli).IdTestata
    '                                    oAccertamentoSingolo.IdFlussoRuolo = objRiepilogoContribuente(0).oArticoli(intArticoli).IdFlussoRuolo
    '                                    oAccertamentoSingolo.IDTariffa = objRiepilogoContribuente(0).oArticoli(intArticoli).IDTariffa
    '                                    oAccertamentoSingolo.ImpInteressi = objRiepilogoContribuente(0).oArticoli(intArticoli).ImpInteressi
    '                                    oAccertamentoSingolo.ImportoDetassazione = objRiepilogoContribuente(0).oArticoli(intArticoli).ImportoDetassazione
    '                                    oAccertamentoSingolo.ImportoForzato = objRiepilogoContribuente(0).oArticoli(intArticoli).ImportoForzato
    '                                    oAccertamentoSingolo.ImportoNetto = objRiepilogoContribuente(0).oArticoli(intArticoli).ImportoNetto
    '                                    oAccertamentoSingolo.ImportoRiduzione = objRiepilogoContribuente(0).oArticoli(intArticoli).ImportoRiduzione
    '                                    oAccertamentoSingolo.ImportoRuolo = objRiepilogoContribuente(0).oArticoli(intArticoli).ImportoRuolo
    '                                    oAccertamentoSingolo.ImpSanzioni = objRiepilogoContribuente(0).oArticoli(intArticoli).ImpSanzioni
    '                                    oAccertamentoSingolo.ImpSpeseNotifica = objRiepilogoContribuente(0).oArticoli(intArticoli).ImpSpeseNotifica
    '                                    oAccertamentoSingolo.ImpTariffa = objRiepilogoContribuente(0).oArticoli(intArticoli).ImpTariffa
    '                                    oAccertamentoSingolo.InformazioniCartella = objRiepilogoContribuente(0).oArticoli(intArticoli).InformazioniCartella
    '                                    oAccertamentoSingolo.Interno = objRiepilogoContribuente(0).oArticoli(intArticoli).Interno
    '                                    oAccertamentoSingolo.IsTarsuGiornaliera = objRiepilogoContribuente(0).oArticoli(intArticoli).IsTarsuGiornaliera
    '                                    oAccertamentoSingolo.MQ = objRiepilogoContribuente(0).oArticoli(intArticoli).MQ
    '                                    oAccertamentoSingolo.nComponenti = objRiepilogoContribuente(0).oArticoli(intArticoli).nComponenti
    '                                    oAccertamentoSingolo.Numero = objRiepilogoContribuente(0).oArticoli(intArticoli).Numero
    '                                    oAccertamentoSingolo.NumeroBimestri = objRiepilogoContribuente(0).oArticoli(intArticoli).NumeroBimestri

    '                                    If Not objRiepilogoContribuente(0).oArticoli(intArticoli).oDetassazioni Is Nothing Then
    '                                        For intDet = 0 To objRiepilogoContribuente(0).oArticoli(intArticoli).oDetassazioni.Length - 1
    '                                            objDet = New OggettoDetassazione
    '                                            objDet.Descrizione = objRiepilogoContribuente(0).oArticoli(intArticoli).oDetassazioni(intDet).Descrizione
    '                                            objDet.IdDetassazione = objRiepilogoContribuente(0).oArticoli(intArticoli).oDetassazioni(intDet).IdDetassazione
    '                                            objDet.IdDettaglioTestata = objRiepilogoContribuente(0).oArticoli(intArticoli).oDetassazioni(intDet).IdDettaglioTestata
    '                                            objDet.sIdEnte = objRiepilogoContribuente(0).oArticoli(intArticoli).oDetassazioni(intDet).sIdEnte
    '                                            objDet.sTipo = objRiepilogoContribuente(0).oArticoli(intArticoli).oDetassazioni(intDet).sTipo
    '                                            objDet.sTipoValoreDet = objRiepilogoContribuente(0).oArticoli(intArticoli).oDetassazioni(intDet).sTipoValoreDet
    '                                            objDet.sValore = objRiepilogoContribuente(0).oArticoli(intArticoli).oDetassazioni(intDet).sValore
    '                                            ReDim Preserve ArrayDetassazioni(intDet)
    '                                            ArrayDetassazioni(intDet) = objDet
    '                                        Next
    '                                        If ArrayDetassazioni.Length > 0 Then
    '                                            oAccertamentoSingolo.oDetassazioni = ArrayDetassazioni
    '                                        End If
    '                                    End If

    '                                    If Not objRiepilogoContribuente(0).oArticoli(intArticoli).oRiduzioni Is Nothing Then
    '                                        For intRid = 0 To objRiepilogoContribuente(0).oArticoli(intArticoli).oRiduzioni.Length - 1
    '                                            objRid = New OggettoRiduzione
    '                                            objRid.Descrizione = objRiepilogoContribuente(0).oArticoli(intArticoli).oRiduzioni(intRid).Descrizione
    '                                            objRid.IdRiduzione = objRiepilogoContribuente(0).oArticoli(intArticoli).oRiduzioni(intRid).IdRiduzione
    '                                            objRid.IdDettaglioTestata = objRiepilogoContribuente(0).oArticoli(intArticoli).oRiduzioni(intRid).IdDettaglioTestata
    '                                            objRid.sIdEnte = objRiepilogoContribuente(0).oArticoli(intArticoli).oRiduzioni(intRid).sIdEnte
    '                                            objRid.sTipo = objRiepilogoContribuente(0).oArticoli(intArticoli).oRiduzioni(intRid).sTipo
    '                                            objRid.sTipoValoreRid = objRiepilogoContribuente(0).oArticoli(intArticoli).oRiduzioni(intRid).sTipoValoreRid
    '                                            objRid.sValore = objRiepilogoContribuente(0).oArticoli(intArticoli).oRiduzioni(intRid).sValore
    '                                            ReDim Preserve ArrayRiduzioni(intRid)
    '                                            ArrayRiduzioni(intRid) = objRid
    '                                        Next
    '                                        If ArrayRiduzioni.Length > 0 Then
    '                                            oAccertamentoSingolo.oRiduzioni = ArrayRiduzioni
    '                                        End If
    '                                    End If

    '                                    oAccertamentoSingolo.Scala = objRiepilogoContribuente(0).oArticoli(intArticoli).Scala
    '                                    oAccertamentoSingolo.Subalterno = objRiepilogoContribuente(0).oArticoli(intArticoli).Subalterno
    '                                    oAccertamentoSingolo.TipoRuolo = objRiepilogoContribuente(0).oArticoli(intArticoli).TipoRuolo
    '                                    oAccertamentoSingolo.Via = objRiepilogoContribuente(0).oArticoli(intArticoli).Via
    '                                    oAccertamentoSingolo.Progressivo = intProgressivo + 1
    '                                    intProgressivo = intProgressivo + 1
    '                                    oAccertamentoSingolo.IdLegame = oAccertamentoSingolo.Progressivo

    '                                    oAccertamento(intArticoli) = oAccertamentoSingolo
    '                                Next
    '                            End If
    '                        End If
    '                        If Page.IsPostBack = False Then
    '                            'Svuota la Session con la dichirazione precedente
    '                            Session.Remove("DataSetDichiarazioni")
    '                            GrdDichiarato.start_index = 0
    '                            GrdDichiarato.DataSource = oAccertamento
    '                            GrdDichiarato.DataBind()
    '                            Session("DataSetDichiarazioni") = oAccertamento
    '                        End If
    '                    End If
    '                End If
    '                If Session("ESCLUDI_PREACCERTAMENTO") Then
    '                    Session("DataSetDichiarazioni") = Nothing
    '                End If

    '                Select Case CInt(GrdDichiarato.Rows.Count)
    '                    Case 0
    '                        GrdDichiarato.Visible = False
    '                        lblMessage.Text = "Non sono presenti Immobili Dichiarati"
    '                    Case Is > 0
    '                        GrdDichiarato.Visible = True
    '                        If Session("ESCLUDI_PREACCERTAMENTO") Then
    '                            lblMessage.Text = "Gli immobili dichiarati non verranno presi in considerazione per il calcolo di questo accertamento in quanto già calcolati in altro accertamento definitivo"
    '                            lblTesto.Visible = False
    '                            chkSelTutti.Visible = False
    '                            Dim strHidden As String
    '                            strHidden = "<script language='javascript'>"
    '                            strHidden += "parent.parent.Comandi.document.getElementById('btnRibaltaImmobile').style.display='none';"
    '                            strHidden += "</script>"
    '                            RegisterScript(sScript , Me.GetType())
    '                        Else
    '                            lblMessage.Visible = False
    '                            chkSelTutti.Visible = True
    '                            lblTesto.Visible = True
    '                        End If

    '                End Select
    '            End If
    '        End If
    '        dim sScript as string=""
    '        
    '        sscript+="parent.document.getElementById('attesaCarica').style.display='none';")
    '        sscript+="parent.document.getElementById('loadGridDichiarato').style.display='' ;")
    '        

    '        RegisterScript(sScript , Me.GetType())
    '    Catch Err As Exception
    '  Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamentiTARSU.Page_Load.errore: ", Err)
    '       Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim intProgressivo As Integer = 0
        Dim objHashTable As Hashtable
        Dim FncCalcoloRuolo As New OPENgovTIA.ClsElabRuolo
        Dim MyArray As New ArrayList
        Dim CurrentItem As New ObjArticoloAccertamento
        Dim ListAccertamento() As ObjArticoloAccertamento
        Dim oMyRuolo() As ObjRuolo
        Dim myArticolo As New ObjArticolo
        Dim sScript As String = ""

        Try
            Log.Debug("si inizia")
            'Viene Salvata da GestioneAccertamenti.aspx.vb
            'per avere il codice contribuente e anno di accertamento
            objHashTable = Session("HashTableDichAccertamentiTARSU")

            If Not Page.IsPostBack Then
                If Not Session("DataSetDichiarazioni") Is Nothing Then
                    ListAccertamento = CType(Session("DataSetDichiarazioni"), ObjArticoloAccertamento())
                    GrdDichiarato.DataSource = ListAccertamento
                    GrdDichiarato.DataBind()
                Else
                    SetHashTable(objHashTable) 'SetHashTable(objHashTable, objSessione)
                    '******************************************************************
                    'Cerco tutte le dichiarazioni per l'anno selezionato
                    '******************************************************************
                    '*** 20181011 Dal/Al Conferimenti ***
                    objHashTable("tDataInizioConf") = New OPENgovTIA.generalClass.generalFunction().FormattaData(objHashTable("ANNOACCERTAMENTO") + "0101", "G")
                    objHashTable("tDataFineConf") = New OPENgovTIA.generalClass.generalFunction().FormattaData(objHashTable("ANNOACCERTAMENTO") + "1231", "G")
                    Log.Debug("richiamo CalcolaRuoloFromDich")
                    oMyRuolo = FncCalcoloRuolo.CalcolaRuoloFromDich(ConstSession.StringConnectionTARSU, ConstSession.IdEnte, objHashTable("ANNOACCERTAMENTO"), objHashTable("TipoTassazione"), objHashTable("TipoCalcolo"), objHashTable("DescrTipoCalcolo"), objHashTable("PercentTariffe"), objHashTable("HasMaggiorazione"), objHashTable("HasConferimenti"), objHashTable("TipoMQ"), objHashTable("impSogliaAvvisi"), objHashTable("IdTestata"), objHashTable("CODCONTRIBUENTE"), objHashTable("tDataInizioConf"), objHashTable("tDataFineConf"))
                    'controllo se ho un riepilogo articoli
                    If Not oMyRuolo Is Nothing Then
                        If Not oMyRuolo(0).oAvvisi Is Nothing Then
                            If Not oMyRuolo(0).oAvvisi(0) Is Nothing Then
                                If Not oMyRuolo(0).oAvvisi(0).oArticoli Is Nothing Then
                                    For Each myArticolo In oMyRuolo(0).oAvvisi(0).oArticoli
                                        Dim FncAcc As New ClsGestioneAccertamenti
                                        CurrentItem = FncAcc.ArticoloTOArticoloAccertamento(myArticolo, False)
                                        If Not CurrentItem Is Nothing Then
                                            CurrentItem.IdEnte = ConstSession.IdEnte
                                            CurrentItem.sNote = objHashTable("TipoTassazione")
                                            CurrentItem.Progressivo = intProgressivo + 1
                                            intProgressivo = intProgressivo + 1
                                            CurrentItem.IdLegame = CurrentItem.Progressivo
                                            MyArray.Add(CurrentItem)
                                        Else
                                            Throw New Exception("errore in caricamento articolo")
                                        End If
                                    Next
                                Else
                                    Log.Debug("ruolo.avvisi.articoli=nothing")
                                End If
                                ListAccertamento = CType(MyArray.ToArray(GetType(ObjArticoloAccertamento)), ObjArticoloAccertamento())
                            End If
                        Else
                            Log.Debug("ruolo.avvisi=nothing")
                        End If
                        If Page.IsPostBack = False Then
                            'Svuota la Session con la dichirazione precedente
                            Session.Remove("DataSetDichiarazioni")
                            GrdDichiarato.DataSource = ListAccertamento
                            GrdDichiarato.DataBind()
                            Session("DataSetDichiarazioni") = ListAccertamento
                            If objHashTable("TipoTassazione") = ObjRuolo.TipoCalcolo.TARES Then
                                LoadPartiteNegative(ListAccertamento)
                            End If
                        End If
                    Else
                        Log.Debug("ruolo=nothing")
                    End If
                    If Session("ESCLUDI_PREACCERTAMENTO") Then
                        Session("DataSetDichiarazioni") = Nothing
                    End If

                    Select Case CInt(GrdDichiarato.Rows.Count)
                        Case 0
                            GrdDichiarato.Visible = False
                            lblMessage.Text = "Non sono presenti Immobili Dichiarati"
                        Case Is > 0
                            GrdDichiarato.Visible = True
                            If objHashTable("TipoTassazione") <> ObjRuolo.TipoCalcolo.TARES Then
                                GrdDichiarato.Columns(10).Visible = False
                                GrdDichiarato.Columns(11).Visible = False
                                GrdDichiarato.Columns(12).Visible = False
                            End If
                            If Session("ESCLUDI_PREACCERTAMENTO") Then
                                lblMessage.Text = "Gli immobili dichiarati non verranno presi in considerazione per il calcolo di questo accertamento in quanto già calcolati in altro accertamento definitivo"
                                lblTesto.Visible = False
                                chkSelTutti.Visible = False
                                sScript += "parent.parent.Comandi.document.getElementById('btnRibaltaImmobile').style.display='none';"
                                RegisterScript(sScript, Me.GetType())
                            Else
                                lblMessage.Visible = False
                                chkSelTutti.Visible = True
                                lblTesto.Visible = True
                            End If
                    End Select
                End If
            End If
            sScript += "parent.document.getElementById('attesaCarica').style.display='none';"
            sScript += "parent.document.getElementById('loadGridDichiarato').style.display='' ;"
            RegisterScript(sScript, Me.GetType())
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamentiTARSU.Page_Load.errore: ", Err)
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
        Try
            Dim IDRow As String = e.CommandArgument.ToString()
            Select Case e.CommandName
                Case "RowUpdate"
                    For Each myRow As GridViewRow In GrdDichiarato.Rows
                        If IDRow = CType(myRow.FindControl("hfprogressivo"), HiddenField).Value Then
                            Dim oAccertamentoUpdate() As ObjArticoloAccertamento
                            Dim intArticoliUpdate As Integer

                            'Prendo l'idLegame
                            Dim IdLegameGrid As TextBox = myRow.Cells(0).FindControl("txtLegame")
                            Dim IdGridUpdate As String = myRow.Cells(0).Text

                            If IdLegameGrid.Text = "" Then
                                Dim sScript As String = ""
                                sScript += "msgLegameVuoto();" & vbCrLf
                                RegisterScript(sScript, Me.GetType())
                            Else
                                oAccertamentoUpdate = CType(Session("DataSetDichiarazioni"), ObjArticoloAccertamento())

                                For intArticoliUpdate = 0 To oAccertamentoUpdate.Length - 1
                                    If oAccertamentoUpdate(intArticoliUpdate).Progressivo = IdGridUpdate Then
                                        oAccertamentoUpdate(intArticoliUpdate).IdLegame = IdLegameGrid.Text
                                    End If
                                Next
                                GrdDichiarato.EditIndex = -1
                                GrdDichiarato.DataSource = oAccertamentoUpdate
                                GrdDichiarato.DataBind()
                                Session("DataSetDichiarazioni") = oAccertamentoUpdate
                            End If
                        End If
                    Next
                Case "RowDelete"
                    For Each myRow As GridViewRow In GrdDichiarato.Rows
                        If IDRow = CType(myRow.FindControl("hfprogressivo"), HiddenField).Value Then
                            Dim oAccertamentoDelete() As ObjArticoloAccertamento
                            Dim oAccertamentoReturn() As ObjArticoloAccertamento
                            Dim intArticoliDelete As Integer
                            Dim intArticoliReturn As Integer

                            Dim IdGridDelete As String = myRow.Cells(0).Text

                            oAccertamentoDelete = CType(Session("DataSetDichiarazioni"), ObjArticoloAccertamento())

                            intArticoliReturn = -1
                            If Not oAccertamentoDelete Is Nothing Then
                                For intArticoliDelete = 0 To oAccertamentoDelete.Length - 1
                                    If oAccertamentoDelete(intArticoliDelete).Progressivo <> IdGridDelete Then
                                        intArticoliReturn += 1
                                        ReDim Preserve oAccertamentoReturn(intArticoliReturn)
                                        oAccertamentoReturn(intArticoliReturn) = oAccertamentoDelete(intArticoliDelete)
                                    End If
                                Next

                                If Not oAccertamentoReturn Is Nothing Then
                                    GrdDichiarato.EditIndex = -1
                                    GrdDichiarato.DataSource = oAccertamentoReturn
                                    GrdDichiarato.DataBind()
                                    Session("DataSetDichiarazioni") = oAccertamentoReturn
                                Else
                                    GrdDichiarato.EditIndex = -1
                                    GrdDichiarato.DataSource = Nothing
                                    GrdDichiarato.DataBind()
                                    GrdDichiarato.Style.Add("display", "none")
                                    Session("DataSetDichiarazioni") = Nothing
                                End If
                            Else
                                GrdDichiarato.EditIndex = -1
                                GrdDichiarato.DataSource = Nothing
                                GrdDichiarato.DataBind()
                                GrdDichiarato.Style.Add("display", "none")
                                Session("DataSetDichiarazioni") = Nothing
                            End If
                        End If
                    Next
            End Select
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamentiTARSU.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
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

                e.Row.Cells(17).ToolTip = "Premere questo pulsante per modificare il legame dell'immobile"
                e.Row.Cells(18).ToolTip = "Premere questo pulsante per eliminare l'immobile"

                e.Row.Cells(6).Text = IntForGridView(e.Row.Cells(6).Text)
                e.Row.Cells(16).Text = FormatStringToZero(e.Row.Cells(16).Text)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamentiTARSU.GrdRowDataBound.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="iInput"></param>
    ''' <returns></returns>
    Public Function IntForGridView(ByVal iInput As Object) As String

        Dim ret As String = String.Empty
        Try
            If iInput.ToString() = "-1" Or iInput.ToString() = "-1,00" Then
                ret = String.Empty
            Else
                ret = Convert.ToString(iInput)
            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamentiTARSU.IntForGridView.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
        Return ret
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="objInput"></param>
    ''' <returns></returns>
    Private Function FormatStringToZero(ByVal objInput As Object) As String

        Dim strOutput As String
        Try
            If (objInput Is Nothing) Then
                strOutput = ""
            ElseIf IsDBNull(objInput) Then
                strOutput = ""
            Else
                If CStr(objInput) = "" Or CStr(objInput) = "0" Or CStr(objInput) = "-1" Then
                    strOutput = ""
                Else
                    strOutput = objInput.ToString()
                End If

            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamentiTARSU.FormatStringToZero.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
        Return strOutput
    End Function
    'Protected Sub GrdDichiarato_DeleteCommand(ByVal s As Object, ByVal e As DataGridCommandEventArgs) Handles GrdDichiarato.DeleteCommand
    '    Dim oAccertamentoDelete() As ObjArticoloAccertamento
    '    Dim oAccertamentoReturn() As ObjArticoloAccertamento

    '    Dim intArticoliDelete As Integer
    '    Dim intArticoliReturn As Integer
    '    Try
    '        Dim IdGridDelete As String = e.Item.Cells(0).Text

    '        oAccertamentoDelete = CType(Session("DataSetDichiarazioni"), ObjArticoloAccertamento())

    '        intArticoliReturn = -1
    '        If Not oAccertamentoDelete Is Nothing Then
    '            For intArticoliDelete = 0 To oAccertamentoDelete.Length - 1
    '                If oAccertamentoDelete(intArticoliDelete).Progressivo <> IdGridDelete Then
    '                    intArticoliReturn += 1
    '                    ReDim Preserve oAccertamentoReturn(intArticoliReturn)
    '                    oAccertamentoReturn(intArticoliReturn) = oAccertamentoDelete(intArticoliDelete)
    '                End If
    '            Next

    '            If Not oAccertamentoReturn Is Nothing Then
    '                GrdDichiarato.EditItemIndex = -1
    '                GrdDichiarato.DataSource = oAccertamentoReturn
    '                GrdDichiarato.DataBind()
    '                Session("DataSetDichiarazioni") = oAccertamentoReturn
    '            Else
    '                GrdDichiarato.EditItemIndex = -1
    '                GrdDichiarato.DataSource = Nothing
    '                GrdDichiarato.DataBind()
    '                GrdDichiarato.Style.Add("display", "none")
    '                Session("DataSetDichiarazioni") = Nothing
    '            End If
    '        Else
    '            GrdDichiarato.EditItemIndex = -1
    '            GrdDichiarato.DataSource = Nothing
    '            GrdDichiarato.DataBind()
    '            GrdDichiarato.Style.Add("display", "none")
    '            Session("DataSetDichiarazioni") = Nothing
    '        End If
    '    Catch Err As Exception
    '      Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamentiTARSU.GrdDichiarato_DeleteCommand.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Protected Sub GrdDichiarato_Update(ByVal sender As Object, ByVal e As DataGridCommandEventArgs) Handles GrdDichiarato.UpdateCommand
    '    Dim oAccertamentoUpdate() As ObjArticoloAccertamento
    '    Dim oAccertamentoReturn() As ObjArticoloAccertamento
    '    Dim intArticoliUpdate As Integer

    '    Try
    '        'Prendo l'idLegame
    '        Dim IdLegameGrid As TextBox = e.Item.Cells(0).FindControl("txtLegame")
    '        Dim IdGridUpdate As String = e.Item.Cells(0).Text

    '        If IdLegameGrid.Text = "" Then
    '            dim sScript as string=""
    '            sscript+= "msgLegameVuoto();" & vbCrLf
    '            RegisterScript(sScript , Me.GetType())
    '        Else
    '            oAccertamentoUpdate = CType(Session("DataSetDichiarazioni"), ObjArticoloAccertamento())

    '            Dim arrayListImmobili As New ArrayList
    '            Dim oAccertamentoSingolo As New ObjArticoloAccertamento

    '            For intArticoliUpdate = 0 To oAccertamentoUpdate.Length - 1
    '                If oAccertamentoUpdate(intArticoliUpdate).Progressivo = IdGridUpdate Then
    '                    oAccertamentoUpdate(intArticoliUpdate).IdLegame = IdLegameGrid.Text
    '                End If
    '            Next

    '            GrdDichiarato.EditItemIndex = -1
    '            GrdDichiarato.DataSource = oAccertamentoUpdate
    '            GrdDichiarato.DataBind()
    '            Session("DataSetDichiarazioni") = oAccertamentoUpdate
    '        End If
    '    Catch Err As Exception
    '          Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamentiTARSU.GrdDichiarato_Update.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
#End Region
    'Protected Sub GrdDichiarato_DeleteCommand(ByVal s As Object, ByVal e As DataGridCommandEventArgs) Handles GrdDichiarato.DeleteCommand
    '    'Dim oAccertamentoDelete() As OggettoArticoloRuoloAccertamento
    '    'Dim oAccertamentoReturn() As OggettoArticoloRuoloAccertamento
    '    Dim oAccertamentoDelete() As OggettoArticoloRuolo
    '    Dim oAccertamentoReturn() As OggettoArticoloRuolo

    '    Dim intArticoliDelete As Integer
    '    Dim intArticoliReturn As Integer
    '    Try
    '        Dim IdGridDelete As String = e.Item.Cells(0).Text

    '        'oAccertamentoDelete = CType(Session("DataSetDichiarazioni"), OggettoArticoloRuoloAccertamento())
    '        oAccertamentoDelete = CType(Session("DataSetDichiarazioni"), OggettoArticoloRuolo())

    '        intArticoliReturn = -1
    '        If Not oAccertamentoDelete Is Nothing Then
    '            For intArticoliDelete = 0 To oAccertamentoDelete.Length - 1
    '                If oAccertamentoDelete(intArticoliDelete).Progressivo <> IdGridDelete Then
    '                    intArticoliReturn += 1
    '                    ReDim Preserve oAccertamentoReturn(intArticoliReturn)
    '                    oAccertamentoReturn(intArticoliReturn) = oAccertamentoDelete(intArticoliDelete)
    '                End If
    '            Next

    '            If Not oAccertamentoReturn Is Nothing Then
    '                GrdDichiarato.EditItemIndex = -1
    '                GrdDichiarato.DataSource = oAccertamentoReturn
    '                GrdDichiarato.DataBind()
    '                Session("DataSetDichiarazioni") = oAccertamentoReturn
    '            Else
    '                GrdDichiarato.EditItemIndex = -1
    '                GrdDichiarato.DataSource = Nothing
    '                GrdDichiarato.DataBind()
    '                GrdDichiarato.Style.Add("display", "none")
    '                Session("DataSetDichiarazioni") = Nothing
    '            End If
    '        Else
    '            GrdDichiarato.EditItemIndex = -1
    '            GrdDichiarato.DataSource = Nothing
    '            GrdDichiarato.DataBind()
    '            GrdDichiarato.Style.Add("display", "none")
    '            Session("DataSetDichiarazioni") = Nothing
    '        End If
    '    Catch Err As Exception
    '      Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamentiTARSU.GrdDichiarato_DeleteCommand.errore: ", Err)
    '     Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Protected Sub GrdDichiarato_Update(ByVal sender As Object, ByVal e As DataGridCommandEventArgs) Handles GrdDichiarato.UpdateCommand
    '    'Dim oAccertamentoUpdate() As OggettoArticoloRuoloAccertamento
    '    'Dim oAccertamentoReturn() As OggettoArticoloRuoloAccertamento
    '    Dim oAccertamentoUpdate() As OggettoArticoloRuolo
    '    Dim oAccertamentoReturn() As OggettoArticoloRuolo
    '    Dim intArticoliUpdate As Integer

    '    Try
    '        'Prendo l'idLegame
    '        Dim IdLegameGrid As TextBox = e.Item.Cells(0).FindControl("txtLegame")
    '        Dim IdGridUpdate As String = e.Item.Cells(0).Text

    '        If IdLegameGrid.Text = "" Then
    '            dim sScript as string=""
    '            sscript+= "msgLegameVuoto();" & vbCrLf
    '            RegisterScript(sScript , Me.GetType())
    '        Else
    '            'oAccertamentoUpdate = CType(Session("DataSetDichiarazioni"), OggettoArticoloRuoloAccertamento())
    '            oAccertamentoUpdate = CType(Session("DataSetDichiarazioni"), OggettoArticoloRuolo())

    '            Dim arrayListImmobili As New ArrayList
    '            'Dim oAccertamentoSingolo As OggettoArticoloRuoloAccertamento
    '            'oAccertamentoSingolo = New OggettoArticoloRuoloAccertamento
    '            Dim oAccertamentoSingolo As OggettoArticoloRuolo
    '            oAccertamentoSingolo = New OggettoArticoloRuolo

    '            For intArticoliUpdate = 0 To oAccertamentoUpdate.Length - 1
    '                If oAccertamentoUpdate(intArticoliUpdate).Progressivo = IdGridUpdate Then
    '                    oAccertamentoUpdate(intArticoliUpdate).IdLegame = IdLegameGrid.Text
    '                End If
    '            Next

    '            GrdDichiarato.EditItemIndex = -1
    '            GrdDichiarato.DataSource = oAccertamentoUpdate
    '            GrdDichiarato.DataBind()
    '            Session("DataSetDichiarazioni") = oAccertamentoUpdate
    '        End If
    '    Catch Err As Exception
    '           Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamentiTARSU.GrdDichiarato_Update.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Private Sub btnRibaltaImmobiliAcc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRibaltaImmobiliAcc.Click
    '    Try
    '        Dim x, y As Integer
    '        Dim sScript As String
    '        Dim sIDSel As String
    '        Dim oListDaAccertare() As OggettoArticoloRuolo
    '        Dim oListAccertato() As OggettoArticoloRuolo
    '        'Dim oListDaAccertare() As OggettoArticoloRuoloAccertamento
    '        'Dim oListAccertato() As OggettoArticoloRuoloAccertamento

    '        oListDaAccertare = Session("DataSetDichiarazioni")

    '        For x = 0 To GrdDichiarato.Items.Count - 1
    '            If CType(GrdDichiarato.Items(x).Cells(26).FindControl("chkRibaltaIm"), CheckBox).Checked = True Then '*** 20140701 - IMU/TARES *** .Cells(24)
    '                sIDSel = GrdDichiarato.Items(x).Cells(0).Text
    '                For y = 0 To oListDaAccertare.Length - 1
    '                    If oListDaAccertare(y).Progressivo = sIDSel Then
    '                        ReDim Preserve oListAccertato(y)
    '                        oListDaAccertare(y).Calcola_Interessi = True
    '                        oListAccertato(y) = oListDaAccertare(y)
    '                        Exit For
    '                    End If
    '                Next
    '            End If
    '        Next

    '        Session("oAccertatiGriglia") = oListAccertato
    '        If Not IsNothing(oListAccertato) Then
    '            sScript = "parent.parent.Visualizza.loadGridAccertato.location.href='SearchAccertatiTARSU.aspx';"
    '        Else
    '            sScript = "alert('Selezionare almeno un immobile per il ribaltamento!')"
    '        End If
    '        RegisterScript(sScript , Me.GetType())

    '    Catch Err As Exception
    '          Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamentiTARSU.btnRibaltaImmobiliAcc_Click.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnRibaltaImmobiliAcc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRibaltaImmobiliAcc.Click
        Try
            Dim y As Integer
            Dim sScript As String
            Dim sIDSel As String
            Dim oListDaAccertare() As ObjArticoloAccertamento
            Dim oListAccertato() As ObjArticoloAccertamento
            oListDaAccertare = Session("DataSetDichiarazioni")

            For Each myRow As GridViewRow In GrdDichiarato.Rows
                If CType(myRow.FindControl("chkRibaltaIm"), CheckBox).Checked = True Then '*** 20140701 - IMU/TARES *** .Cells(24)
                    sIDSel = myRow.Cells(0).Text
                    For y = 0 To oListDaAccertare.Length - 1
                        If oListDaAccertare(y).Progressivo = sIDSel Then
                            ReDim Preserve oListAccertato(y)
                            oListDaAccertare(y).Calcola_Interessi = True
                            oListAccertato(y) = oListDaAccertare(y)
                            Exit For
                        End If
                    Next
                End If
            Next
            Session("oAccertatiGriglia") = oListAccertato
            If Not IsNothing(oListAccertato) Then
                sScript = "parent.document.getElementById('loadGridAccertato').src='SearchAccertatiTARSU.aspx';" 'sScript = "parent.parent.Visualizza.loadGridAccertato.location.href='SearchAccertatiTARSU.aspx';"
            Else
                sScript = "GestAlert('a', 'warning', '', '', 'Selezionare almeno un immobile per il ribaltamento!');"
            End If
            RegisterScript(sScript, Me.GetType())
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamentiTARSU.btnRibaltaImmobiliAcc_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="oListAccertato"></param>
    Private Sub LoadPartiteNegative(ByVal oListAccertato() As ObjArticoloAccertamento)
        Try
            'devo generare delle partite generali di segno contrario per far tornare i conti
            Dim myArtAcc As New ObjArticoloAccertamento
            Dim myDichPF As New ObjArticoloAccertamento
            Dim myDichPV As New ObjArticoloAccertamento
            Dim myDichPM As New ObjArticoloAccertamento
            Dim myDichPC As New ObjArticoloAccertamento
            Dim myArray As New ArrayList

            myDichPF.IdContribuente = oListAccertato(0).IdContribuente
            myDichPF.IdEnte = oListAccertato(0).IdEnte
            myDichPF.sAnno = oListAccertato(0).sAnno
            myDichPF.tDataInizio = "01/01/" & myDichPF.sAnno
            myDichPF.tDataFine = "31/12/" & myDichPF.sAnno
            myDichPF.TipoPartita = ObjArticolo.PARTEFISSA
            myDichPF.sVia = myDichPF.TipoPartita & " - DA DICHIARAZIONE"

            myDichPV.IdContribuente = oListAccertato(0).IdContribuente
            myDichPV.IdEnte = oListAccertato(0).IdEnte
            myDichPV.sAnno = oListAccertato(0).sAnno
            myDichPV.tDataInizio = "01/01/" & myDichPV.sAnno
            myDichPV.tDataFine = "31/12/" & myDichPV.sAnno
            myDichPV.TipoPartita = ObjArticolo.PARTEVARIABILE
            myDichPV.sVia = myDichPV.TipoPartita & " - DA DICHIARAZIONE"

            myDichPM.IdContribuente = oListAccertato(0).IdContribuente
            myDichPM.IdEnte = oListAccertato(0).IdEnte
            myDichPM.sAnno = oListAccertato(0).sAnno
            myDichPM.tDataInizio = "01/01/" & myDichPM.sAnno
            myDichPM.tDataFine = "31/12/" & myDichPM.sAnno
            myDichPM.TipoPartita = ObjArticolo.PARTEMAGGIORAZIONE
            myDichPM.sVia = myDichPM.TipoPartita & " - DA DICHIARAZIONE"

            myDichPC.IdContribuente = oListAccertato(0).IdContribuente
            myDichPC.IdEnte = oListAccertato(0).IdEnte
            myDichPC.sAnno = oListAccertato(0).sAnno
            myDichPC.tDataInizio = "01/01/" & myDichPC.sAnno
            myDichPC.tDataFine = "31/12/" & myDichPC.sAnno
            myDichPC.TipoPartita = ObjArticolo.PARTECONFERIMENTI
            myDichPC.sVia = myDichPC.TipoPartita & " - DA DICHIARAZIONE"

            For Each myArtAcc In oListAccertato
                Select Case myArtAcc.TipoPartita
                    Case ObjArticolo.PARTEFISSA
                        myDichPF.impNetto -= myArtAcc.impNetto
                    Case ObjArticolo.PARTEVARIABILE
                        myDichPV.impNetto -= myArtAcc.impNetto
                    Case ObjArticolo.PARTEMAGGIORAZIONE
                        myDichPM.impNetto -= myArtAcc.impNetto
                    Case ObjArticolo.PARTECONFERIMENTI
                        myDichPC.impNetto -= myArtAcc.impNetto
                End Select
            Next
            If myDichPF.impNetto <> 0 Then
                myArray.Add(myDichPF)
            End If
            If myDichPV.impNetto <> 0 Then
                myArray.Add(myDichPV)
            End If
            If myDichPM.impNetto <> 0 Then
                myArray.Add(myDichPM)
            End If
            If myDichPC.impNetto <> 0 Then
                myArray.Add(myDichPC)
            End If
            Session("oAccertatiDaDichiarazione") = CType(myArray.ToArray(GetType(ObjArticoloAccertamento)), ObjArticoloAccertamento())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamentiTARSU.LoadPartiteNegative.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="objHashTable"></param>
    Private Sub SetHashTable(ByRef objHashTable As Hashtable)
        'Dim strConnectionStringOPENgovProvvedimenti As String
        'Dim strConnectionStringOPENgovTARSU As String

        Try
            'Recupero la hash table
            If objHashTable.ContainsKey("IDSOTTOAPPLICAZIONEICI") = False Then
                objHashTable.Add("IDSOTTOAPPLICAZIONEICI", ConfigurationManager.AppSettings("OPENGOVTA"))
            Else
                objHashTable("IDSOTTOAPPLICAZIONEICI") = ConfigurationManager.AppSettings("OPENGOVTA")
            End If
            'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString
            'strConnectionStringOPENgovTARSU = ConstSession.StringConnectionICI

            'Aggiungo gli altri campi nella hash table
            If objHashTable.ContainsKey("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI") = False Then
                objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
            Else
                objHashTable("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI") = ConstSession.StringConnection
            End If
            If objHashTable.ContainsKey("CONNECTIONSTRINGOPENGOVTARSU") = False Then
                objHashTable.Add("CONNECTIONSTRINGOPENGOVTARSU", ConstSession.StringConnectionICI)
            Else
                objHashTable("CONNECTIONSTRINGOPENGOVTARSU") = ConstSession.StringConnectionICI
            End If
            If objHashTable.ContainsKey("USER") = False Then
                objHashTable.Add("USER", ConstSession.UserName)
            Else
                objHashTable("USER") = ConstSession.UserName
            End If
            If objHashTable.ContainsKey("CODENTE") = False Then
                objHashTable.Add("CODENTE", ConstSession.IdEnte)
            Else
                objHashTable("CODENTE") = ConstSession.IdEnte
            End If
            If objHashTable.ContainsKey("CodENTE") = False Then
                objHashTable.Add("CodENTE", ConstSession.IdEnte)
            Else
                objHashTable("CodENTE") = ConstSession.IdEnte
            End If
            If objHashTable.ContainsKey("CODTIPOPROCEDIMENTO") = False Then
                objHashTable.Add("CODTIPOPROCEDIMENTO", "L")
            Else
                objHashTable("CODTIPOPROCEDIMENTO") = "L"
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamentiTARSU.SetHashTable.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Response.Write("<script language='javascript'>")
            Response.Write("</script>")
        End Try
    End Sub
    'Private Sub SetHashTable(ByRef objHashTable As Hashtable, ByVal objSessione)
    '    'Dim strConnectionStringOPENgovProvvedimenti As String
    '    'Dim strConnectionStringOPENgovTARSU As String

    '    Try
    '        'Recupero la hash table
    '        If objHashTable.ContainsKey("IDSOTTOAPPLICAZIONEICI") = False Then
    '            objHashTable.Add("IDSOTTOAPPLICAZIONEICI", ConfigurationManager.AppSettings("OPENGOVTA"))
    '        Else
    '            objHashTable("IDSOTTOAPPLICAZIONEICI") = ConfigurationManager.AppSettings("OPENGOVTA")
    '        End If
    '        'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString
    '        'strConnectionStringOPENgovTARSU = objSessione.oSession.GetPrivateDBManager(objHashTable("IDSOTTOAPPLICAZIONEICI")).GetConnection.ConnectionString

    '        'Aggiungo gli altri campi nella hash table
    '        If objHashTable.ContainsKey("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI") = False Then
    '            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
    '        Else
    '            objHashTable("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI") = ConstSession.StringConnection
    '        End If
    '        If objHashTable.ContainsKey("CONNECTIONSTRINGOPENGOVTARSU") = False Then
    '            objHashTable.Add("CONNECTIONSTRINGOPENGOVTARSU", ConstSession.StringConnectionICI)
    '        Else
    '            objHashTable("CONNECTIONSTRINGOPENGOVTARSU") = ConstSession.StringConnectionICI
    '        End If
    '        If objHashTable.ContainsKey("USER") = False Then
    '            objHashTable.Add("USER", ConstSession.UserName)
    '        Else
    '            objHashTable("USER") = ConstSession.UserName
    '        End If
    '        If objHashTable.ContainsKey("CODENTE") = False Then
    '            objHashTable.Add("CODENTE", ConstSession.IdEnte)
    '        Else
    '            objHashTable("CODENTE") = ConstSession.IdEnte
    '        End If
    '        If objHashTable.ContainsKey("CodENTE") = False Then
    '            objHashTable.Add("CodENTE", ConstSession.IdEnte)
    '        Else
    '            objHashTable("CodENTE") = ConstSession.IdEnte
    '        End If
    '        If objHashTable.ContainsKey("CODTIPOPROCEDIMENTO") = False Then
    '            objHashTable.Add("CODTIPOPROCEDIMENTO", "L")
    '        Else
    '            objHashTable("CODTIPOPROCEDIMENTO") = "L"
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamentiTARSU.SetHashTable.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '        Response.Write("<script language='javascript'>")
    '        Response.Write("</script>")
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub chkSelTutti_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSelTutti.CheckedChanged
        Try
            If chkSelTutti.Checked = True Then
                '** ciclare la griglia e selezionare tutti gli immobili
                For Each myRow As GridViewRow In GrdDichiarato.Rows
                    CType(myRow.FindControl("chkRibaltaIm"), CheckBox).Checked = True ''*** 20140701 - IMU/TARES *** .Cells(23)
                Next
            Else
                '** nessuna selezione -> selezione manuale dell'operatore
                For Each myRow As GridViewRow In GrdDichiarato.Rows
                    CType(myRow.FindControl("chkRibaltaIm"), CheckBox).Checked = False '*** 20140701 - IMU/TARES *** .Cells(23)
                Next
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.SearchDatiAccertamentiTARSU.chkSelTutti_CheckedChanged.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
''' <summary>
''' Definizione oggetto
''' </summary>
Public Class ObjInsertAccertamento
    Private _codcontribuente As Integer
    Private _provenienza As String

    Public Property codcontribuente() As Integer
        Get
            Return _codcontribuente
        End Get
        Set(ByVal Value As Integer)
            _codcontribuente = Value
        End Set
    End Property
    Public Property provenienza() As String
        Get
            Return _provenienza
        End Get
        Set(ByVal Value As String)
            _provenienza = Value
        End Set
    End Property
End Class

