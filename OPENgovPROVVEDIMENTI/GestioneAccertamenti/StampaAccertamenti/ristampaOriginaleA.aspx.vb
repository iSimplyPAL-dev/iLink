Imports RIBESElaborazioneDocumentiInterface
Imports RIBESElaborazioneDocumentiInterface.Stampa.oggetti

Imports System.Globalization
Imports log4net
Imports ComPlusInterface
Imports AnagInterface

''' <summary>
''' Pagina per la produzione dei documenti per il provvedimento.
''' Contiene le funzioni della comandiera. 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class ristampaOriginaleA
    Inherits BaseEnte

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
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(ristampaOriginaleA))

    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    Dim strTipoDoc, sTributo, Anno As String
    '    Dim bIsBozza As Boolean
    '    Dim sScript As String = ""
    '    Dim IdDocToElab(,) As Integer = Nothing

    '    Try
    '        strTipoDoc = Request.Item("tipodoc")
    '        bIsBozza = Request.Item("IsBozza")
    '        sTributo = Request.Item("CODTRIBUTO")
    '        Anno = Request.Item("Anno")
    '        '###############################################################################################################################################################################
    '        'STAMPA DOCUMENTO DI ACCERTAMENTO
    '        '###############################################################################################################################################################################
    '        'If sTributo = Utility.Costanti.TRIBUTO_TARSU Then
    '        '    Select Case strTipoDoc
    '        '        Case CostantiProvv.DOCUMENTO_ANNULLAMENTO
    '        '            STAMPA_ANNULLAMENTO(strTipoDoc, bIsBozza, sTributo)
    '        '        Case Else
    '        '            STAMPA_ACCERTAMENTOTARSU(strTipoDoc, bIsBozza, sTributo)
    '        '    End Select
    '        '    '*** 20130801 - accertamento OSAP ***
    '        'ElseIf sTributo = Utility.Costanti.TRIBUTO_OSAP Then
    '        '    Select Case strTipoDoc
    '        '        Case CostantiProvv.DOCUMENTO_ANNULLAMENTO
    '        '            STAMPA_ANNULLAMENTO(strTipoDoc, bIsBozza, sTributo)
    '        '        Case Else
    '        '            STAMPA_ACCERTAMENTOOSAP(strTipoDoc, bIsBozza)
    '        '    End Select
    '        '    '*** ***
    '        'Else
    '        '    Select Case strTipoDoc
    '        '        Case CostantiProvv.DOCUMENTO_RIMBORSO_ICI
    '        '            STAMPA_RIMBORSOICI(strTipoDoc, bIsBozza, sTributo)
    '        '        Case CostantiProvv.DOCUMENTO_ANNULLAMENTO
    '        '            STAMPA_ANNULLAMENTO(strTipoDoc, bIsBozza, sTributo)
    '        '        Case Else
    '        '            STAMPA_ACCERTAMENTOICI(strTipoDoc, bIsBozza, sTributo)
    '        '    End Select
    '        'End If
    '        If strTipoDoc = CostantiProvv.DOCUMENTO_RIMBORSO_ICI Then
    '            STAMPA_RIMBORSOICI(strTipoDoc, bIsBozza, sTributo)
    '        ElseIf strTipoDoc = CostantiProvv.DOCUMENTO_ANNULLAMENTO Then
    '            If sTributo = Utility.Costanti.TRIBUTO_TARSU Then
    '                STAMPA_ANNULLAMENTO(strTipoDoc, bIsBozza, sTributo)
    '            ElseIf sTributo = Utility.Costanti.TRIBUTO_OSAP Then
    '                STAMPA_ANNULLAMENTO(strTipoDoc, bIsBozza, sTributo)
    '            Else
    '                STAMPA_ANNULLAMENTO(strTipoDoc, bIsBozza, sTributo)
    '            End If
    '        ElseIf strTipoDoc = CostantiProvv.BOLLETTINI_ACCERTAMENTO Then
    '            Dim oElaborazioneDati As ElaborazioneDatiStampeInterface.IElaborazioneStampeICI
    '            Dim oListDocStampati() As RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL
    '            Dim Esclusione() As String = Nothing
    '            oElaborazioneDati = Activator.GetObject(GetType(ElaborazioneDatiStampeInterface.IElaborazioneStampeICI), ConstSession.UrlServizioStampeICI)
    '            Log.Debug("UrlServizioStampeICI=" & ConstSession.UrlServizioStampeICI)
    '            Dim TipoElab As String = "EFFETTIVO"
    '            Dim dsStampa As DataSet = CType(Session("PROVVEDIMENTI_DA_STAMPARE"), DataSet)
    '            For Each myRow As DataRow In dsStampa.Tables(0).Rows
    '                ReDim Preserve IdDocToElab(1, 0)
    '                IdDocToElab(0, 0) = myRow("ID_PROVVEDIMENTO")
    '                IdDocToElab(1, 0) = myRow("COD_CONTRIBUENTE")
    '                oListDocStampati = oElaborazioneDati.ElaborazioneMassivaDocumenti(ConstSession.DBType, sTributo, IdDocToElab, myRow("Anno"), ConstSession.IdEnte, "-1", Esclusione, ConstSession.StringConnection, ConstSession.StringConnectionOPENgov, ConstSession.StringConnectionAnagrafica, ConstSession.PathStampe, ConstSession.PathVirtualStampe, 1, TipoElab, "BOLLETTINISTANDARD", "ACCERTAMENTO", "F24", True, True, False, 2, False, True, "")
    '                Session.Remove("ELENCO_ACCERTAMENTI_STAMPATI")
    '                If Not oListDocStampati Is Nothing Then
    '                    Session.Add("ELENCO_ACCERTAMENTI_STAMPATI", oListDocStampati(0))
    '                    sScript += "parent.corpo.location.href='ViewAccertamentiStampati.aspx';"
    '                    sScript += "parent.parent.document.getElementById('DivAttesa').style.display='none';"
    '                Else
    '                    sScript += "GestAlert('a', 'danger', '', '', 'Errore in fase di stampa');"
    '                End If
    '                RegisterScript(sScript, Me.GetType())
    '            Next
    '        Else
    '            If sTributo = Utility.Costanti.TRIBUTO_TARSU Then
    '                STAMPA_ACCERTAMENTOTARSU(strTipoDoc, bIsBozza, sTributo)
    '            ElseIf sTributo = Utility.Costanti.TRIBUTO_OSAP Then
    '                STAMPA_ACCERTAMENTOOSAP(strTipoDoc, bIsBozza)
    '            Else
    '                STAMPA_ACCERTAMENTOICI(strTipoDoc, bIsBozza, sTributo)
    '            End If
    '            'Dim oElaborazioneDati As ElaborazioneDatiStampeInterface.IElaborazioneStampeICI
    '            'Dim oListDocStampati() As RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL
    '            'Dim Esclusione() As String = Nothing
    '            'oElaborazioneDati = Activator.GetObject(GetType(ElaborazioneDatiStampeInterface.IElaborazioneStampeICI), ConstSession.UrlServizioStampeICI)
    '            'Log.Debug("UrlServizioStampeICI=" & ConstSession.UrlServizioStampeICI)
    '            'Dim TipoElab As String
    '            'If bIsBozza Then
    '            '    TipoElab = "PROVA"
    '            'Else
    '            '    TipoElab = "EFFETTIVO"
    '            'End If
    '            'Dim dsStampa As DataSet = CType(Session("PROVVEDIMENTI_DA_STAMPARE"), DataSet)
    '            'For Each myRow As DataRow In dsStampa.Tables(0).Rows
    '            '    ReDim Preserve IdDocToElab(1, 0)
    '            '    IdDocToElab(0, 0) = myRow("ID_PROVVEDIMENTO")
    '            '    IdDocToElab(1, 0) = myRow("COD_CONTRIBUENTE")
    '            '    oListDocStampati = oElaborazioneDati.ElaborazioneMassivaDocumenti(ConstSession.DBType, sTributo, IdDocToElab, myRow("Anno"), ConstSession.IdEnte, "-1", Esclusione, ConstSession.StringConnection, ConstSession.StringConnectionOPENgov, ConstSession.StringConnectionAnagrafica, ConstSession.PathStampe, ConstSession.PathVirtualStampe, 1, TipoElab, "NOBOLLETTINI", "ACCERTAMENTO", "", True, True, False, 2, False, False, "")
    '            '    Session.Remove("ELENCO_ACCERTAMENTI_STAMPATI")
    '            '    If Not oListDocStampati Is Nothing Then
    '            '        Session.Add("ELENCO_ACCERTAMENTI_STAMPATI", oListDocStampati(0))
    '            '        sScript += "parent.corpo.location.href='ViewAccertamentiStampati.aspx';"
    '            '        sScript += "parent.parent.document.getElementById('DivAttesa').style.display='none';"
    '            '    Else
    '            '        sScript += "GestAlert('a', 'danger', '', '', 'Errore in fase di stampa');"
    '            '    End If
    '            '    RegisterScript(sScript, Me.GetType())
    '            'Next
    '        End If
    '        '###############################################################################################################################################################################
    '        'FINE STAMPA DOCUMENTO DI ACCERTAMENTO
    '        '###############################################################################################################################################################################
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.Page_Load.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="05/11/2020">
    ''' devo aggiungere tributo F24 per poter gestire correttamente la stampa in caso di Ravvedimento IMU/TASI
    ''' </revision>
    ''' </revisionHistory>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim strTipoDoc, sTributo As String
        Dim bIsBozza As Boolean
        Dim sScript As String = ""
        Dim IdDocToElab(,) As Integer = Nothing

        Try
            strTipoDoc = Request.Item("tipodoc")
            bIsBozza = Request.Item("IsBozza")
            sTributo = Request.Item("CODTRIBUTO")
            If strTipoDoc = CostantiProvv.DOCUMENTO_ANNULLAMENTO Then
                If sTributo = Utility.Costanti.TRIBUTO_TARSU Then
                    STAMPA_ANNULLAMENTO(strTipoDoc, bIsBozza, sTributo)
                ElseIf sTributo = Utility.Costanti.TRIBUTO_OSAP Then
                    STAMPA_ANNULLAMENTO(strTipoDoc, bIsBozza, sTributo)
                Else
                    STAMPA_DocumentoICI(strTipoDoc, bIsBozza, sTributo)
                End If
            ElseIf strTipoDoc = CostantiProvv.BOLLETTINI_ACCERTAMENTO Then
                Dim oElaborazioneDati As ElaborazioneDatiStampeInterface.IElaborazioneStampeICI
                Dim oListDocStampati() As RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL
                Dim Esclusione() As String = Nothing
                oElaborazioneDati = Activator.GetObject(GetType(ElaborazioneDatiStampeInterface.IElaborazioneStampeICI), ConstSession.UrlServizioStampeICI)
                Log.Debug("UrlServizioStampeICI=" & ConstSession.UrlServizioStampeICI)
                Dim TipoElab As String = "EFFETTIVO"
                Dim dsStampa As DataSet = CType(Session("PROVVEDIMENTI_DA_STAMPARE"), DataSet)
                For Each myRow As DataRow In dsStampa.Tables(0).Rows
                    ReDim Preserve IdDocToElab(1, 0)
                    IdDocToElab(0, 0) = myRow("ID_PROVVEDIMENTO")
                    IdDocToElab(1, 0) = myRow("COD_CONTRIBUENTE")
                    oListDocStampati = oElaborazioneDati.ElaborazioneMassivaDocumenti(ConstSession.DBType, sTributo, IdDocToElab, myRow("Anno"), ConstSession.IdEnte, "-1", Esclusione, ConstSession.StringConnection, ConstSession.StringConnectionOPENgov, ConstSession.StringConnectionAnagrafica, ConstSession.PathStampe, ConstSession.PathVirtualStampe, 1, TipoElab, "BOLLETTINISTANDARD", "ACCERTAMENTO", "F24", True, True, False, 2, False, True, "", sTributo)
                    Session.Remove("ELENCO_ACCERTAMENTI_STAMPATI")
                    If Not oListDocStampati Is Nothing Then
                        Session.Add("ELENCO_ACCERTAMENTI_STAMPATI", oListDocStampati(0))
                        sScript += "parent.corpo.location.href='ViewAccertamentiStampati.aspx';"
                        sScript += "parent.parent.document.getElementById('DivAttesa').style.display='none';"
                    Else
                        sScript += "GestAlert('a', 'danger', '', '', 'Errore in fase di stampa');"
                    End If
                    RegisterScript(sScript, Me.GetType())
                Next
            Else
                If sTributo = Utility.Costanti.TRIBUTO_TARSU Then
                    STAMPA_ACCERTAMENTOTARSU(strTipoDoc, bIsBozza, sTributo)
                ElseIf sTributo = Utility.Costanti.TRIBUTO_OSAP Then
                    STAMPA_ACCERTAMENTOOSAP(strTipoDoc, bIsBozza)
                Else
                    STAMPA_DocumentoICI(strTipoDoc, bIsBozza, sTributo)
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.Page_Load.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub

#Region "Stampa"
    'Private Function STAMPA_ACCERTAMENTOICI(ByVal strTipoDoc As String, ByVal bIsBozza As Boolean)

    '    Dim sFilenameDOT, sFilenameDOC As String
    '    Dim objDSstampa As DataSet
    '    Dim objUtility As New MyUtility
    '    'contiene i dati anagrafici visualizzati nella grilgia della ricerca lettere
    '    objDSstampa = CType(Session("PROVVEDIMENTI_DA_STAMPARE"), DataSet)

    '    ' creo l'oggetto testata per l'oggetto da stampare
    '    'serve per indicare la posizione di salvataggio e il nome del file.
    '    Dim objTestataDOC As oggettoTestata
    '    Dim objTestataDOT As New oggettoTestata
    '    Dim IDPROVVEDIMENTO As String
    '    Dim IDPROCEDIMENTO As String
    '    Dim strTIPODOCUMENTO As String
    '    Dim strANNO As String

    '    ' COMPONGO IL PERCORSO DEL FILE DOT

    '    ' COMPONGO IL PERCORSO DEL FILE DOT

    '    sFilenameDOT = "AccertamentoICI_" & ConstSession.IdEnte & ".dot"
    '    sFilenameDOC = "AccertamentoICI"
    '    strTIPODOCUMENTO = Costanti.COD_TIPO_DOC_ACCERTAMENTO

    '    objTestataDOT.Atto = "Template"
    '    objTestataDOT.Dominio = "Provvedimenti"
    '    objTestataDOT.Ente = ConstSession.DescrizioneEnte
    '    objTestataDOT.Filename = sFilenameDOT

    '    Dim oArrBookmark As ArrayList
    '    Dim iCount As Integer
    '    Dim iImmobili As Integer
    '    Dim iErrori As Integer

    '    Dim objBookmark As oggettiStampa
    '    Dim oArrListOggettiDaStampare As New ArrayList
    '    Dim objToPrint As oggettoDaStampareCompleto
    '    Dim ArrayBookMark As oggettiStampa()
    '    Dim iCodContrib As Integer

    '    'Dim culture As IFormatProvider
    '    'culture = New CultureInfo("it-IT", True)
    '    'System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")
    '    Dim strRiga, strelencointeressi As String
    '    Dim strImmoTemp As String = String.Empty
    '    Dim strErroriTemp As String = String.Empty
    '    Dim strImmoTempTitolo As String = String.Empty
    '    Dim Anno As String = String.Empty
    '    'Dim IDdichiarazione As Long
    '    'Dim IDImmobile As Long

    '    Dim dsDatiProvv As New DataSet
    '    Dim dsImmobiliDichiarato As New DataSet
    '    Dim dsVersamenti As New DataSet
    '    Dim dsImmobiliAccertati As New DataSet
    '    Dim objDSTipiInteressi As New DataSet
    '    Dim objDSTipiInteressiL As New DataSet
    '    Dim objDSElencoSanzioni As New DataSet
    '    Dim objDSElencoSanzioniF2 As New DataSet
    '    Dim objDSElencoSanzioniF2Intr As New DataSet
    '    Dim objDSElencoSanzioniRiducibili As New DataSet
    '    Dim objDSElencoSanzioniF2Riducibili As New DataSet
    '    Dim objDSImportiInteressi As New DataSet
    '    Dim objDSImportiInteressiF2 As New DataSet
    '    '---------------------------------------
    '    'var per popolare i bookmark relativi 
    '    'alla sezione degli importi interessi
    '    Dim strImportoGiorni As String
    '    Dim strImportoSemestriACC As String
    '    Dim strImportoSemestriSAL As String
    '    Dim strNumSemestriACC As String
    '    Dim strNumSemestriSAL As String
    '    Dim iRetValImpInt As Boolean
    '    '---------------------------------------

    '    'Dim DSperInsertTabStorico As DataSet = CreateDataSetStampa()
    '    'Dim dr As DataRow

    '    'Dim strConnectionStringOPENgovProvvedimenti As String

    '    'Dim objSessione As CreateSessione
    '    Dim strWFErrore As String
    '    objSessione = New CreateSessione(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))
    'Try
    '    If Not objSessione.CreaSessione(ConstSession.UserName, strWFErrore) Then
    '        Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '    End If

    '    'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString

    '    Dim NomeDbIci As String = ConfigurationManager.AppSettings("NOME_DATABASE_ICI").ToString()
    '    Dim NomeDbOpenGov As String = ConfigurationManager.AppSettings("NOME_DATABASE_OPENGOV").ToString()

    '    Dim objHashTable As Hashtable = New Hashtable
    '    objHashTable.Add("CodENTE", ConstSession.IdEnte)
    '    objHashTable.Add("CODENTE", ConstSession.IdEnte)
    '    objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
    '    objHashTable.Add("NomeDbIci", NomeDbIci)
    '    objHashTable.Add("NomeDbOpenGov", NomeDbOpenGov)

    '    Dim objCOM As IElaborazioneLiquidazioni = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneLiquidazioni), ConfigurationManager.AppSettings("URLServiziLiquidazioni"))
    '    Dim objCOMTipoVoci As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)
    '    Dim objCOMACCERT As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
    '    Dim objICI As DichiarazioniICI.Database.TpSituazioneFinaleIci
    '    objICI = New DichiarazioniICI.Database.TpSituazioneFinaleIci

    '    Dim objVers As DichiarazioniICI.Database.VersamentiTable
    '    objVers = New DichiarazioniICI.Database.VersamentiTable(ConstSession.UserName)

    '    'Dim dsDettaglioAnagrafica As DataSet = Nothing
    '    Dim oDettaglioAnagrafica As DettaglioAnagrafica
    '    Dim objAnagrafica As Anagrafica.DLL.GestioneAnagrafica
    '    objAnagrafica = New Anagrafica.DLL.GestioneAnagrafica(objSessione.oSession, "OPENGOVA")


    '    For iCount = 0 To objDSstampa.Tables(0).Rows.Count - 1
    '        iCodContrib = objDSstampa.Tables(0).Rows(iCount)("COD_CONTRIBUENTE")
    '        IDPROVVEDIMENTO = objDSstampa.Tables(0).Rows(iCount)("ID_PROVVEDIMENTO")
    '        objHashTable.Add("COD_CONTRIBUENTE", iCodContrib)

    '        dsDatiProvv = objCOM.GetProvvedimentoPerStampaLiquidazione(objHashTable, IDPROVVEDIMENTO)

    '        IDPROCEDIMENTO = dsDatiProvv.Tables(0).Rows(iCount)("ID_PROCEDIMENTO")
    '        strANNO = dsDatiProvv.Tables(0).Rows(iCount)("ANNO")
    '        objHashTable.Add("ANNO", strANNO)
    '        '*** 20140701 - IMU/TARES ***
    '        If strANNO >= 2012 Then
    '            sFilenameDOT = sFilenameDOT.Replace("ICI", "IMU")
    '            sFilenameDOC = sFilenameDOC.Replace("ICI", "IMU")
    '            objTestataDOT.Filename = sFilenameDOT
    '            Log.Debug(sFilenameDOT & "::" & sFilenameDOC)
    '        End If
    '        '*** ***

    '        objTestataDOC = New oggettoTestata
    '        objTestataDOC.Atto = "Documenti"
    '        objTestataDOC.Dominio = "Provvedimenti"
    '        objTestataDOC.Ente = ConstSession.DescrizioneEnte
    '        objTestataDOC.Filename = iCodContrib & "_" & ConstSession.IdEnte & sFilenameDOC

    '        oArrBookmark = New ArrayList


    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "bozza"
    '        If bIsBozza = True Then
    '            objBookmark.Valore = "BOZZA"
    '        Else
    '            objBookmark.Valore = ""
    '        End If
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "nome_ente"
    '        objBookmark.Valore = CStr(Session("DESCRIZIONE_ENTE")).ToUpper
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "TipoProvvedimento"
    '        objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("TIPO_PROVVEDIMENTO")).ToUpper()
    '        oArrBookmark.Add(objBookmark)

    '        '************************************************************************************
    '        'DATI ANAGRAFICI
    '        '************************************************************************************
    '        oDettaglioAnagrafica = New DettaglioAnagrafica
    '        'oDettaglioAnagrafica = objAnagrafica.GetAnagrafica(iCodContrib, Session("COD_TRIBUTO"))
    '        oDettaglioAnagrafica = objAnagrafica.GetAnagrafica(iCodContrib, -1, ConstSession.DBType, ConstSession.StringConnectionAnagrafica, False)
    '        'dsDettaglioAnagrafica = addRowsObjAnagrafica(oDettaglioAnagrafica)

    '        Dim cognome, nome As String
    '        cognome = FormatStringToEmpty(oDettaglioAnagrafica.Cognome)
    '        nome = FormatStringToEmpty(oDettaglioAnagrafica.Nome)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "cognome"
    '        objBookmark.Valore = cognome
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "nome"
    '        objBookmark.Valore = nome
    '        oArrBookmark.Add(objBookmark)

    '        Dim strViaRes, strCivRes, strFrazRes, strCapRes, strCittaRes, strProvRes As String
    '        Dim strViaCO, strCivCO, strFrazCO, strCapCO, strCittaCO, strProvCO, strNominativo_CO As String
    '        Dim strVia, strCiv, strFraz, strCap, strCitta, strProv As String

    '        strViaRes = FormatStringToEmpty(oDettaglioAnagrafica.ViaResidenza)
    '        strCivRes = FormatStringToEmpty(oDettaglioAnagrafica.CivicoResidenza)
    '        strFrazRes = FormatStringToEmpty(oDettaglioAnagrafica.FrazioneResidenza)
    '        strCapRes = FormatStringToEmpty(oDettaglioAnagrafica.CapResidenza)
    '        strCittaRes = FormatStringToEmpty(oDettaglioAnagrafica.ComuneResidenza)
    '        strProvRes = FormatStringToEmpty(oDettaglioAnagrafica.ProvinciaResidenza)
    '        '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
    '        'strViaCO = FormatStringToEmpty(oDettaglioAnagrafica.ViaRCP)
    '        'strCivCO = FormatStringToEmpty(oDettaglioAnagrafica.CivicoRCP)
    '        'strFrazCO = FormatStringToEmpty(oDettaglioAnagrafica.FrazioneRCP)
    '        'strCapCO = FormatStringToEmpty(oDettaglioAnagrafica.CapRCP)
    '        'strCittaCO = FormatStringToEmpty(oDettaglioAnagrafica.ComuneRCP)
    '        'strProvCO = FormatStringToEmpty(oDettaglioAnagrafica.ProvinciaRCP)
    '        For Each mySped As ObjIndirizziSpedizione In oDettaglioAnagrafica.ListSpedizioni
    '            If mySped.CodTributo = Costanti.TRIBUTO_ICI Then
    '                cognome = FormatStringToEmpty(mySped.CognomeInvio)
    '                nome = FormatStringToEmpty(mySped.NomeInvio)
    '                strViaCO = FormatStringToEmpty(mySped.ViaRCP)
    '                strCivCO = FormatStringToEmpty(mySped.CivicoRCP)
    '                strFrazCO = FormatStringToEmpty(mySped.FrazioneRCP)
    '                strCapCO = FormatStringToEmpty(mySped.CapRCP)
    '                strCittaCO = FormatStringToEmpty(mySped.ComuneRCP)
    '                strProvCO = FormatStringToEmpty(mySped.ProvinciaRCP)
    '            End If
    '        Next
    '        '*** ***
    '        If (strViaCO = "") Then
    '            'visualizzo indirizzo residenza
    '            strVia = strViaRes.ToUpper()
    '            strCiv = strCivRes.ToUpper()
    '            strFraz = strFrazRes.ToUpper()
    '            strCap = strCapRes.ToUpper()
    '            strCitta = strCittaRes.ToUpper()
    '            strProv = strProvRes.ToUpper()
    '            strNominativo_CO = ""
    '        Else
    '            'visualizzo indirizzo spedizione
    '            strVia = strViaCO.ToUpper()
    '            strCiv = strCivCO.ToUpper()
    '            strFraz = strFrazCO.ToUpper()
    '            strCap = strCapCO.ToUpper()
    '            strCitta = strCittaCO.ToUpper()
    '            strProv = strProvCO.ToUpper()

    '            strNominativo_CO = "C/O " & cognome & " " & nome
    '            strNominativo_CO = strNominativo_CO.ToUpper()
    '        End If
    '        If strProv <> "" Then strProv = "(" & strProv & ")"
    '        'Nominativo_CO
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "Nominativo_CO"
    '        objBookmark.Valore = strNominativo_CO
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "via_residenza"
    '        objBookmark.Valore = strVia
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "civico_residenza"
    '        objBookmark.Valore = strCiv
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "frazione_residenza"
    '        objBookmark.Valore = strFraz
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "cap_residenza"
    '        objBookmark.Valore = strCap
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "citta_residenza"
    '        objBookmark.Valore = strCitta
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "prov_residenza"
    '        objBookmark.Valore = strProv

    '        oArrBookmark.Add(objBookmark)


    '        '******** vecchia versione ********
    '        'objBookmark = New oggettiStampa
    '        'objBookmark.Descrizione = "cognome"
    '        'objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("COGNOME"))
    '        'oArrBookmark.Add(objBookmark)

    '        'objBookmark = New oggettiStampa
    '        'objBookmark.Descrizione = "nome"
    '        'objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("NOME"))
    '        'oArrBookmark.Add(objBookmark)

    '        'objBookmark = New oggettiStampa
    '        'objBookmark.Descrizione = "via_residenza"
    '        'objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("VIA_RES"))
    '        'oArrBookmark.Add(objBookmark)

    '        'objBookmark = New oggettiStampa
    '        'objBookmark.Descrizione = "civico_residenza"
    '        'objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("CIVICO_RES"))
    '        'oArrBookmark.Add(objBookmark)

    '        'objBookmark = New oggettiStampa
    '        'objBookmark.Descrizione = "frazione_residenza"
    '        'objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("FRAZIONE_RES"))
    '        'oArrBookmark.Add(objBookmark)

    '        'objBookmark = New oggettiStampa
    '        'objBookmark.Descrizione = "cap_residenza"
    '        'objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("CAP_RES"))
    '        'oArrBookmark.Add(objBookmark)

    '        'objBookmark = New oggettiStampa
    '        'objBookmark.Descrizione = "citta_residenza"
    '        'objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("CITTA_RES"))
    '        'oArrBookmark.Add(objBookmark)

    '        'objBookmark = New oggettiStampa
    '        'objBookmark.Descrizione = "prov_residenza"
    '        'If CStr(dsDatiProvv.Tables(0).Rows(iCount)("PROVINCIA_RES")).CompareTo("") <> 0 Then
    '        '    objBookmark.Valore = "(" & dsDatiProvv.Tables(0).Rows(iCount)("PROVINCIA_RES") & ")"
    '        'Else
    '        '    objBookmark.Valore = ""
    '        'End If
    '        'oArrBookmark.Add(objBookmark)

    '        'Dim codice_fiscale, partita_iva As String
    '        'codice_fiscale = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("CODICE_FISCALE"))
    '        'partita_iva = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("PARTITA_IVA"))

    '        '******** vecchia versione ********

    '        Dim codice_fiscale, partita_iva As String
    '        codice_fiscale = FormatStringToEmpty(oDettaglioAnagrafica.CodiceFiscale)
    '        partita_iva = FormatStringToEmpty(oDettaglioAnagrafica.PartitaIva)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "codice_fiscale"
    '        objBookmark.Valore = codice_fiscale
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "partita_iva"
    '        objBookmark.Valore = partita_iva
    '        oArrBookmark.Add(objBookmark)

    '        'dipe 16/12/2009
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "codice_fiscale_1"
    '        objBookmark.Valore = codice_fiscale
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "partita_iva_1"
    '        objBookmark.Valore = partita_iva
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "codice_fiscale_2"
    '        objBookmark.Valore = codice_fiscale
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "partita_iva_2"
    '        objBookmark.Valore = partita_iva
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "codice_fiscale_3"
    '        objBookmark.Valore = codice_fiscale
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "partita_iva_3"
    '        objBookmark.Valore = partita_iva
    '        oArrBookmark.Add(objBookmark)
    '        '---------------------------------------------------------------------------------
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "anno_ici"
    '        objBookmark.Valore = strANNO
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "anno_ici_1"
    '        objBookmark.Valore = strANNO
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "n_provvedimento"
    '        objBookmark.Valore = objUtility.CToStr(dsDatiProvv.Tables(0).Rows(iCount)("NUMERO_ATTO"))

    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "data_provvedimento"
    '        If dsDatiProvv.Tables(0).Rows(iCount)("DATA_ELABORAZIONE") Is System.DBNull.Value Then
    '            objBookmark.Valore = ""
    '        Else
    '            objBookmark.Valore = objUtility.GiraDataFromDB(dsDatiProvv.Tables(0).Rows(iCount)("DATA_ELABORAZIONE"))
    '        End If
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "anno_imposta"
    '        objBookmark.Valore = strANNO 'dsDatiProvv.Tables(0).Rows(iCount)("ANNO")
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "anno_imposta1"
    '        objBookmark.Valore = strANNO 'dsDatiProvv.Tables(0).Rows(iCount)("ANNO")
    '        oArrBookmark.Add(objBookmark)

    '        '************************************************************************************
    '        'ELENCO IMMOBILI DICHIARATI
    '        '************************************************************************************
    '        dsImmobiliDichiarato = objCOMACCERT.getImmobiliDichiaratiPerStampaAccertamenti(objHashTable, IDPROCEDIMENTO)
    '        '*** 20130620 - richieste comune ***
    '        strRiga = FillBookMarkACCERTATO(dsImmobiliDichiarato, strANNO)
    '        'strRiga = FillBookMarkDICHIARATO(dsImmobiliDichiarato, strANNO)
    '        '*** ***

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "elenco_immobili"
    '        objBookmark.Valore = strRiga
    '        oArrBookmark.Add(objBookmark)

    '        '************************************************************************************
    '        'ELENCO VERSAMENTI
    '        '************************************************************************************
    '        dsVersamenti = objCOM.getVersamentiPerStampaLiquidazione(objHashTable, IDPROCEDIMENTO, Costanti.ID_FASE1)
    '        strRiga = FillBookMarkVersamenti(dsVersamenti)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "elenco_versamenti"
    '        objBookmark.Valore = "Dettaglio Versamenti" & vbCrLf & strRiga
    '        oArrBookmark.Add(objBookmark)

    '        '************************************************************************************
    '        'ELENCO IMMOBILI ACCERATI
    '        '************************************************************************************
    '        dsImmobiliAccertati = objCOMACCERT.getImmobiliAccertatiPerStampaAccertamenti(objHashTable, IDPROCEDIMENTO)
    '        strRiga = FillBookMarkACCERTATO(dsImmobiliAccertati, strANNO)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "elenco_immobili_acce"
    '        objBookmark.Valore = strRiga
    '        oArrBookmark.Add(objBookmark)

    '        Dim acconto, saldo As Double
    '        strRiga = FillBookMarkIMPORTODOVACC(dsImmobiliAccertati, acconto, saldo)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "imp_dov_acc"
    '        objBookmark.Valore = FormatImport(acconto) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "imp_dov_saldo"
    '        objBookmark.Valore = FormatImport(saldo) & " €"
    '        oArrBookmark.Add(objBookmark)



    '        '************************************************************************************
    '        'ELENCO INTERESSI CONFIGURATI
    '        '************************************************************************************
    '        'objHashTable.Add("CODTIPOINTERESSE", "-1")
    '        'objHashTable.Add("DAL", "")
    '        'objHashTable.Add("AL", "")
    '        'objHashTable.Add("TASSO", "")
    '        objHashTable.Add("CODTRIBUTO", Session("COD_TRIBUTO"))

    '        'objDSTipiInteressi = objCOMTipoVoci.GetTipoInteresse(objHashTable)
    '        objDSTipiInteressiL = objCOM.GetElencoInteressiPerStampaLiquidazione(objHashTable, IDPROVVEDIMENTO)
    '        'strRiga = "Differenza dichiarato/accertato" & Microsoft.VisualBasic.Constants.vbCrLf


    '        'strRiga += vbCrLf
    '        objDSTipiInteressi = objCOMACCERT.GetElencoInteressiPerStampaAccertamenti(objHashTable, IDPROVVEDIMENTO)
    '        'strelencointeressi = FillBookMarkELENCOINTERESSI(objDSTipiInteressi)
    '        'If Not strelencointeressi.StartsWith("Nessuna") Then
    '        '    strRiga += "Differenza accertato/dichiarato" & Microsoft.VisualBasic.Constants.vbCrLf
    '        '    strRiga += strelencointeressi
    '        'End If
    '        strRiga = FillBookMarkELENCOINTERESSI(objDSTipiInteressiL, objDSTipiInteressi, Session("COD_TRIBUTO"))

    '        strRiga += FillBookMarkTOTALEINTERESSI(objDSTipiInteressiL, objDSTipiInteressi)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "elenco_interessi"
    '        objBookmark.Valore = strRiga
    '        oArrBookmark.Add(objBookmark)

    '        '************************************************************************************
    '        'ELENCO SANZIONI APPLICATE CON IMPORTO 
    '        '************************************************************************************
    '        objHashTable.Add("riducibile", 0)
    '        'Sanzioni NON Riducibili
    '        objDSElencoSanzioni = objCOMACCERT.GetElencoSanzioniPerStampaAccertamenti(objHashTable, IDPROVVEDIMENTO)
    '        objDSElencoSanzioniF2 = objCOM.GetElencoSanzioniPerStampaLiquidazione(objHashTable, IDPROVVEDIMENTO)
    '        '*************************
    '        'Sanzioni Riducibili
    '        objHashTable.Remove("riducibile")
    '        objHashTable.Add("riducibile", 1)
    '        objDSElencoSanzioniRiducibili = objCOMACCERT.GetElencoSanzioniPerStampaAccertamenti(objHashTable, IDPROVVEDIMENTO)
    '        objDSElencoSanzioniF2Riducibili = objCOM.GetElencoSanzioniPerStampaLiquidazione(objHashTable, IDPROVVEDIMENTO)
    '        '*************************
    '        'Sanzioni Intrasmissibilità agli eredi 
    '        objHashTable.Remove("riducibile")
    '        objHashTable.Add("riducibile", "")
    '        objDSElencoSanzioniF2Intr = objCOM.GetElencoSanzioniPerStampaLiquidazione(objHashTable, IDPROVVEDIMENTO)
    '        '*************************

    '        strRiga = FillBookMarkELENCOSANZIONI(objDSElencoSanzioni, objDSElencoSanzioniF2, objDSElencoSanzioniRiducibili, objDSElencoSanzioniF2Riducibili, objDSElencoSanzioniF2Intr)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "elenco_sanzioni"
    '        objBookmark.Valore = strRiga
    '        oArrBookmark.Add(objBookmark)

    '        '************************************************************************************
    '        'IMPORTI (DIFF IMPOSTA - SANZIONI - INTERESSI - SPESE - ecc...
    '        '************************************************************************************
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "imposta_dovuta"
    '        'objBookmark.Valore = FormatNumberToZero(dsDatiProvv.Tables(0).Rows(iCount)("TOTALE_DICHIARATO")).Replace(",", ".") & " €"
    '        objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("TOTALE_DICHIARATO")) & " €"
    '        oArrBookmark.Add(objBookmark)
    '        '*** 20130121 - devo stampare il dovuto accertato altrimenti non tornano i conti nel prospetto ***
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "imposta_dovuta_accer"
    '        objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_ACCERTATO_ACC")) & " €"
    '        oArrBookmark.Add(objBookmark)
    '        '*** ***

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "imposta_versata"
    '        'objBookmark.Valore = FormatNumberToZero(dsDatiProvv.Tables(0).Rows(iCount)("TOTALE_VERSATO")).Replace(",", ".") & " €"
    '        objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("TOTALE_VERSATO")) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        Dim ImpDov, ImpVers As Double
    '        Dim tipo_versamento As String = ""

    '        ImpDov = dsDatiProvv.Tables(0).Rows(iCount)("TOTALE_DICHIARATO")
    '        ImpVers = dsDatiProvv.Tables(0).Rows(iCount)("TOTALE_VERSATO")

    '        If ImpDov > 0 And (ImpVers > 0 And ImpVers < ImpDov) Then
    '            tipo_versamento = "parziale"
    '        ElseIf ImpDov > 0 And ImpVers = 0 Then
    '            tipo_versamento = "omesso"
    '        End If
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "tipo_versamento"
    '        objBookmark.Valore = tipo_versamento
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "ImpostaAccertata"
    '        'objBookmark.Valore = FormatNumberToZero(dsDatiProvv.Tables(0).Rows(iCount)("TOTALE_VERSATO")).Replace(",", ".") & " €"
    '        objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("TOTALE_ACCERTATO")) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        'dipe
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "ImpostaAccertata_60g"
    '        objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("TOTALE_ACCERTATO")) & " €"
    '        oArrBookmark.Add(objBookmark)


    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "DiffImpostaDaVersare"
    '        'objBookmark.Valore = FormatNumberToZero(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_DIFFERENZA_IMPOSTA")).Replace(",", ".") & " €"
    '        Dim strImportoDiffImp As String = dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_DIFFERENZA_IMPOSTA")
    '        objBookmark.Valore = FormatImport(strImportoDiffImp) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        'dipe
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "DiffImpostaDaVer_60g"
    '        objBookmark.Valore = FormatImport(strImportoDiffImp) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "DiffImpDaVer_60g_1"
    '        objBookmark.Valore = FormatImport(strImportoDiffImp) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        '********************************************************************************
    '        '************ GESTIONE IMPORTI SANZIONI *****************************************
    '        '********************************************************************************
    '        Dim strImportoSanzioneRidotto As String
    '        objBookmark = New oggettiStampa
    '        strImportoSanzioneRidotto = FillBookMarkSanzioniRiducibili(objDSElencoSanzioniRiducibili)
    '        objBookmark.Descrizione = "ImportoSanzioneRid"
    '        objBookmark.Valore = FormatImport(strImportoSanzioneRidotto) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        Dim ImpSanzioni As String
    '        ImpSanzioni = CType((CType(FillBookMarkSanzioniRiducibili(objDSElencoSanzioni), Double) + CType(FillBookMarkSanzioniRiducibili(objDSElencoSanzioniF2), Double)), String)
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "ImportoSanzione"
    '        objBookmark.Valore = FormatImport(ImpSanzioni) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        Dim strImportoSanzioneRidotto_60g As String
    '        strImportoSanzioneRidotto_60g = dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SANZIONI_RIDOTTO") '(strImportoSanzioneRidotto / 4)
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "ImpSanzioneRid_60g"
    '        objBookmark.Valore = FormatImport(strImportoSanzioneRidotto_60g) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        Dim ImportoSanzione_60g As String
    '        ImportoSanzione_60g = ImpSanzioni
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "ImportoSanzione_60g"
    '        objBookmark.Valore = FormatImport(ImportoSanzione_60g) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        'OLD
    '        If 1 = 0 Then
    '            'objBookmark = New oggettiStampa
    '            'Dim ImpSanzioni As String
    '            'objBookmark.Descrizione = "ImportoSanzione"
    '            'ImpSanzioni = FillBookMarkSanzioniRiducibili(objDSElencoSanzioni)
    '            ''ImpSanzioni = objDSElencoSanzioni.Tables(0).Rows(0)("TOT_IMPORTO_SANZ")
    '            'objBookmark.Valore = FormatImport(ImpSanzioni) & " €"
    '            ''objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SANZIONI")) & " €"
    '            'Dim strImportoSanzione As String = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SANZIONI"))
    '            'oArrBookmark.Add(objBookmark)

    '            ''dipe
    '            'Dim ImportoSanzione_60g As String
    '            'objBookmark = New oggettiStampa
    '            'objBookmark.Descrizione = "ImportoSanzione_60g"
    '            ''ImportoSanzione_60g = objDSElencoSanzioni.Tables(0).Rows(0)("TOT_IMPORTO_SANZ") / 4
    '            'ImportoSanzione_60g = ImpSanzioni / 4
    '            'objBookmark.Valore = FormatImport(ImportoSanzione_60g) & " €"
    '            ''objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SANZIONI")) & " €"
    '            'Dim strImportoSanzione_60g As String = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SANZIONI"))
    '            'oArrBookmark.Add(objBookmark)


    '            'Dim strImportoSanzioneRidotto As String
    '            'objBookmark = New oggettiStampa
    '            'objBookmark.Descrizione = "ImportoSanzioneRid"
    '            ''strImportoSanzioneRidotto = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SANZIONI_RIDOTTO"))

    '            'If objDSElencoSanzioniF2.Tables(0).Rows.Count > 0 Then
    '            '    strImportoSanzioneRidotto = objDSElencoSanzioniF2.Tables(0).Rows(0)("TOT_IMPORTO_SANZ")
    '            'Else
    '            '    strImportoSanzioneRidotto = 0
    '            'End If

    '            'objBookmark.Valore = FormatImport(strImportoSanzioneRidotto) & " €"
    '            'oArrBookmark.Add(objBookmark)

    '            ''dipe
    '            'Dim strImportoSanzioneRidotto_60g As String
    '            'objBookmark = New oggettiStampa
    '            'objBookmark.Descrizione = "ImpSanzioneRid_60g"
    '            ''strImportoSanzioneRidotto_60g = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SANZIONI_RIDOTTO"))

    '            'If objDSElencoSanzioniF2.Tables(0).Rows.Count > 0 Then
    '            '    strImportoSanzioneRidotto_60g = FormatImport(objDSElencoSanzioniF2.Tables(0).Rows(0)("TOT_IMPORTO_SANZ"))
    '            'Else
    '            '    strImportoSanzioneRidotto_60g = 0
    '            'End If
    '            'objBookmark.Valore = FormatImport(strImportoSanzioneRidotto_60g) & " €"
    '            'oArrBookmark.Add(objBookmark)
    '        End If
    '        '********************************************************************************

    '        Dim spese_notifica As String
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "spese_notifica"
    '        If dsDatiProvv.Tables(0).Rows.Count > 0 Then
    '            spese_notifica = dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SPESE")
    '        Else
    '            spese_notifica = 0
    '        End If

    '        objBookmark.Valore = FormatImport(spese_notifica) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        'dipe
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "spese_notifica_60g"
    '        objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SPESE")) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        Dim Importo_arrotond As String
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "Importo_arrotond"
    '        'objBookmark.Valore = FormatNumberToZero(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_ARROTONDAMENTO")).Replace(",", ".") & " €"
    '        Importo_arrotond = dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_ARROTONDAMENTO")
    '        objBookmark.Valore = FormatImport(Importo_arrotond) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        '************************************************************************************
    '        'IMPORTI INTERESSI
    '        '************************************************************************************

    '        objDSImportiInteressi = objCOMACCERT.GetInteressiTotaliPerStampaAccertamenti(objHashTable, IDPROVVEDIMENTO)
    '        objDSImportiInteressiF2 = objCOM.GetInteressiTotaliPerStampaLiquidazione(objHashTable, IDPROVVEDIMENTO)
    '        iRetValImpInt = FillBookMarkIMPORTIINTERESSI(objDSImportiInteressi, objDSImportiInteressiF2, strImportoGiorni, strImportoSemestriACC, strImportoSemestriSAL, strNumSemestriACC, strNumSemestriSAL)
    '        Dim int_mor As String = 0
    '        If iRetValImpInt = True Then

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "imp_interessi_GIORNI"
    '            'objBookmark.Valore = strImportoGiorni.Replace(",", ".") & " €"
    '            objBookmark.Valore = strImportoGiorni & " €"
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "imp_semestri_ACCONTO"
    '            'objBookmark.Valore = strImportoSemestriACC.Replace(",", ".") & " €"
    '            objBookmark.Valore = strImportoSemestriACC & " €"
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "num_semestri_ACCONTO"
    '            objBookmark.Valore = strNumSemestriACC
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "imp_semestri_SALDO"
    '            'objBookmark.Valore = strImportoSemestriSAL.Replace(",", ".") & " €"
    '            objBookmark.Valore = strImportoSemestriSAL & " €"
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "num_semestri_SALDO"
    '            objBookmark.Valore = strNumSemestriSAL
    '            oArrBookmark.Add(objBookmark)


    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "int_mor"
    '            int_mor = CDbl(strImportoGiorni) + CDbl(strImportoSemestriACC) + CDbl(strImportoSemestriSAL)
    '            objBookmark.Valore = FormatImport(int_mor) & " €"
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "int_mor_60g"
    '            objBookmark.Valore = FormatImport(CDbl(strImportoGiorni) + CDbl(strImportoSemestriACC) + CDbl(strImportoSemestriSAL)) & " €"
    '            oArrBookmark.Add(objBookmark)

    '        End If
    '        '************************************************************************************
    '        'FINE IMPORTI INTERESSI
    '        '************************************************************************************

    '        '************************************************************************************
    '        'TOTALI
    '        '************************************************************************************
    '        Dim strImportoTotale As String
    '        'strImportoTotale = CDbl(strImportoDiffImp) + CDbl(int_mor) + CDbl(ImpSanzioni) + CDbl(strImportoSanzioneRidotto) + CDbl(spese_notifica) + CDbl(Importo_arrotond)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "Importo_totale"
    '        strImportoTotale = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_TOTALE"))
    '        'objBookmark.Valore = FormatImport(CDbl(strImportoTotale) - CDbl(strImportoSanzione) + CDbl(strImportoSanzioneRidotto)) & " €"
    '        objBookmark.Valore = FormatImport(strImportoTotale) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        Dim dblIMPORTOARROTONDAMENTO, dblIMPORTOARROTONDATO As Double
    '        Dim Importo_totale_60g As String
    '        'Importo_totale_60g = CDbl(strImportoDiffImp) + CDbl(int_mor) + CDbl(ImportoSanzione_60g) + CDbl(strImportoSanzioneRidotto) + CDbl(spese_notifica) + CDbl(Importo_arrotond)
    '        'Importo_totale_60g = CDbl(strImportoDiffImp) + CDbl(int_mor) + CDbl(ImportoSanzione_60g) + CDbl(strImportoSanzioneRidotto_60g) + CDbl(spese_notifica)
    '        'dblIMPORTOARROTONDATO = ImportoArrotondato(CDbl(Importo_totale_60g))
    '        'dblIMPORTOARROTONDAMENTO = dblIMPORTOARROTONDATO - CDbl(Importo_totale_60g)
    '        'Importo_totale_60g = dblIMPORTOARROTONDATO
    '        Importo_totale_60g = dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_TOTALE_RIDOTTO")
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "Importo_totale_60g"
    '        objBookmark.Valore = FormatImport(Importo_totale_60g) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        'dipe
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "Importo_arrotond_60g"
    '        dblIMPORTOARROTONDAMENTO = dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_ARROTONDAMENTO_RIDOTTO")
    '        objBookmark.Valore = FormatImport(dblIMPORTOARROTONDAMENTO) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "ImpTotNonRidotto"
    '        'objBookmark.Valore = FormatImport(CDbl(strImportoTotale) - CDbl(strImportoSanzione) + CDbl(strImportoSanzioneRidotto)) & " €"
    '        objBookmark.Valore = FormatImport(Importo_totale_60g) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "Importo_totale_1"
    '        'objBookmark.Valore = FormatImport(CDbl(strImportoTotale) - CDbl(strImportoSanzione_60g) + CDbl(strImportoSanzioneRidotto_60g)) & " €"
    '        objBookmark.Valore = FormatImport(Importo_totale_60g) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        '************************************************************************************
    '        'IMPORTI DA DICHIARATO
    '        '************************************************************************************
    '        'Dim dtICI As DataTable
    '        'dtICI = objICI.GetImportoDovuto(iCodContrib, strANNO, constsession.idente)
    '        Dim imp_dov_dich_acc, imp_dov_dich_saldo As Decimal
    '        'dsImmobiliDichiarato = objCOMACCERT.getImmobiliDichiaratiPerStampaAccertamenti(objHashTable, IDPROCEDIMENTO)
    '        FillBookMarkIMP_DOV_DICH(dsImmobiliDichiarato, imp_dov_dich_acc, imp_dov_dich_saldo)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "imp_dov_dich_acc"
    '        'objBookmark.Valore = FormatImport(dtICI.Rows(0)("ACCONTO")) & " €"
    '        objBookmark.Valore = FormatImport(imp_dov_dich_acc) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "imp_dov_dich_saldo"
    '        'objBookmark.Valore = FormatImport(dtICI.Rows(0)("SALDO")) & " €"
    '        objBookmark.Valore = FormatImport(imp_dov_dich_saldo) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        '************************************************************************************
    '        'FINE IMPORTI DA DICHIARATO
    '        '************************************************************************************

    '        '************************************************************************************
    '        'IMPORTI VERSATI
    '        '************************************************************************************
    '        Dim dvVers As DataView
    '        Dim i As Integer
    '        Dim dTot, dTotUS, dTotNoAS As Double

    '        'Senza Flag Acconto e saldo selezionati
    '        dvVers = objVers.GetVersamentiPerTipologia(ConstSession.IdEnte, strANNO, iCodContrib, False, False)
    '        dTotNoAS = 0
    '        For i = 0 To dvVers.Table.Rows.Count - 1
    '            dTotNoAS += dvVers.Table.Rows(i)("ImportoPagato")
    '        Next

    '        'Unica soluzione
    '        dvVers = objVers.GetVersamentiPerTipologia(ConstSession.IdEnte, strANNO, iCodContrib, True, True)
    '        dTotUS = 0
    '        For i = 0 To dvVers.Table.Rows.Count - 1
    '            dTotUS += dvVers.Table.Rows(i)("ImportoPagato")
    '        Next

    '        'Acconto
    '        dvVers = objVers.GetVersamentiPerTipologia(ConstSession.IdEnte, strANNO, iCodContrib, True, False)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "imp_vers_acc"
    '        dTot = dTotUS + dTotNoAS
    '        For i = 0 To dvVers.Table.Rows.Count - 1
    '            dTot += dvVers.Table.Rows(i)("ImportoPagato")
    '        Next
    '        objBookmark.Valore = FormatImport(dTot) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        'saldo
    '        dvVers = objVers.GetVersamentiPerTipologia(ConstSession.IdEnte, strANNO, iCodContrib, False, True)
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "imp_vers_saldo"
    '        dTot = 0
    '        For i = 0 To dvVers.Table.Rows.Count - 1
    '            dTot += dvVers.Table.Rows(i)("ImportoPagato")
    '        Next
    '        objBookmark.Valore = FormatImport(dTot) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        '************************************************************************************
    '        'FINE IMPORTI VERSATI
    '        '************************************************************************************

    '        ArrayBookMark = CType(oArrBookmark.ToArray(GetType(oggettiStampa)), oggettiStampa())

    '        objToPrint = New oggettoDaStampareCompleto
    '        objToPrint.TestataDOC = objTestataDOC
    '        objToPrint.TestataDOT = objTestataDOT
    '        objToPrint.Stampa = ArrayBookMark

    '        'oArrListOggettiDaStampare = New ArrayList
    '        oArrListOggettiDaStampare.Add(objToPrint)

    '        'dr = DSperInsertTabStorico.Tables("DS_TAB_STORICO_DOCUMENTI").NewRow

    '        'dr("COD_CONTRIBUENTE") = iCodContrib
    '        'dr("COD_ENTE") = constsession.idente
    '        'dr("ANNO") = strANNO
    '        'dr("TIPO_DOCUMENTO") = strTIPODOCUMENTO
    '        'dr("COD_TRIBUTO") = Session("COD_TRIBUTO")
    '        'dr("DATA_ELABORAZIONE") = DateTime.Now.ToString("yyyyMMdd")
    '        'dr("NOTE") = ""
    '        'dr("URL_DOCUMENTO") = "-1"

    '        'DSperInsertTabStorico.Tables("DS_TAB_STORICO_DOCUMENTI").Rows.Add(dr)

    '    Next

    '    Dim GruppoDOC As New GruppoDocumenti
    '    Dim GruppoDOCUMENTI As GruppoDocumenti()
    '    Dim ArrListGruppoDOC As New ArrayList

    '    Dim ArrOggCompleto As oggettoDaStampareCompleto()
    '    Dim objTestataGruppo As New oggettoTestata

    '    ArrOggCompleto = CType(oArrListOggettiDaStampare.ToArray(GetType(oggettoDaStampareCompleto)), oggettoDaStampareCompleto())

    '    GruppoDOC.OggettiDaStampare = ArrOggCompleto
    '    GruppoDOC.TestataGruppo = objTestataGruppo

    '    ArrListGruppoDOC.Add(GruppoDOC)

    '    GruppoDOCUMENTI = CType(ArrListGruppoDOC.ToArray(GetType(GruppoDocumenti)), GruppoDocumenti())

    '    'oggettoDaStampare[]  outArray = (oggettoDaStampare[])oArrListOggettiDaStampare.ToArray(typeof(oggettoDaStampare));

    '    ' oggetto per la stampa
    '    Dim oInterfaceStampaDocOggetti As IElaborazioneStampaDocOggetti
    '    oInterfaceStampaDocOggetti = Activator.GetObject(GetType(IElaborazioneStampaDocOggetti), ConfigurationManager.AppSettings("ServizioStampe").ToString())

    '    Dim retArray As GruppoURL

    '    Dim objTestataComplessiva As New RIBESElaborazioneDocumentiInterface.Stampa.oggetti.oggettoTestata
    '    objTestataComplessiva.Atto = ""
    '    objTestataComplessiva.Dominio = ""
    '    objTestataComplessiva.Ente = ""
    '    objTestataComplessiva.Filename = ""
    '    ' definisco anche il numero di documenti che voglio stampare.
    '    retArray = oInterfaceStampaDocOggetti.StampaDocumenti(objTestataComplessiva, GruppoDOCUMENTI)

    '    Session.Remove("ELENCO_ACCERTAMENTI_STAMPATI")

    '    If Not retArray Is Nothing Then

    '        Session.Add("ELENCO_ACCERTAMENTI_STAMPATI", retArray)

    '        'Dim sUrl As String
    '        'Dim strFileName As String
    '        'Dim CodContr As String
    '        'Dim DRperInsertTabStorico() As DataRow
    '        'Dim icontaUrl As Integer

    '        'For icontaUrl = 0 To retArray.URLDocumenti.Length - 1
    '        '    sUrl = retArray.URLDocumenti(icontaUrl).Url()
    '        '    strFileName = retArray.URLDocumenti(icontaUrl).Name()

    '        '    CodContr = strFileName.Substring(0, strFileName.IndexOf("_"))

    '        '    DRperInsertTabStorico = DSperInsertTabStorico.Tables(0).Select("COD_CONTRIBUENTE='" & CodContr & "'")
    '        '    If DRperInsertTabStorico.Length = 1 Then
    '        '        DRperInsertTabStorico(0)("URL_DOCUMENTO") = sUrl
    '        '    End If

    '        'Next

    '        'If objCOM.setTAB_STORICO_DOCUMENTI(objHashTable, DSperInsertTabStorico) = True Then

    '        dim sScript as string=""
    '        sscript+="parent.corpo.location.href='ViewAccertamentiStampati.aspx';" 
    '        RegisterScript(sScript , Me.GetType())

    '        'End If



    '    End If
    '    If Not IsNothing(objSessione) Then
    '        objSessione.Kill()
    '        objSessione = Nothing
    '    End If
    ' Catch ex As Exception
    '  Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.STAMPA_ACCERTAMENTOICI.errore: ", ex)
    '  Response.Redirect("../../../PaginaErrore.aspx")
    'End Try
    'End Function

    'Private Function STAMPA_RIMBORSOICI(ByVal strTipoDoc As String, ByVal bIsBozza As Boolean)
    '    Dim sFilenameDOT, sFilenameDOC As String
    '    Dim objDSstampa As DataSet
    '    Dim objUtility As New MyUtility
    '    'contiene i dati anagrafici visualizzati nella grilgia della ricerca lettere
    '    objDSstampa = CType(Session("PROVVEDIMENTI_DA_STAMPARE"), DataSet)

    '    ' creo l'oggetto testata per l'oggetto da stampare
    '    'serve per indicare la posizione di salvataggio e il nome del file.
    '    Dim objTestataDOC As oggettoTestata
    '    Dim objTestataDOT As New oggettoTestata
    '    Dim IDPROVVEDIMENTO As String
    '    Dim IDPROCEDIMENTO As String
    '    Dim strTIPODOCUMENTO As String
    '    Dim strANNO As String
    '    Dim oArrBookmark As ArrayList
    '    Dim iCount As Integer
    '    Dim iImmobili As Integer
    '    Dim iErrori As Integer

    '    Dim objBookmark As oggettiStampa
    '    Dim oArrListOggettiDaStampare As New ArrayList
    '    Dim objToPrint As oggettoDaStampareCompleto
    '    Dim ArrayBookMark As oggettiStampa()
    '    Dim iCodContrib As Integer

    '    Dim strRiga, strelencointeressi As String
    '    Dim strImmoTemp As String = String.Empty
    '    Dim strErroriTemp As String = String.Empty
    '    Dim strImmoTempTitolo As String = String.Empty
    '    Dim Anno As String = String.Empty

    '    Dim dsDatiProvv As New DataSet
    '    Dim dsImmobiliDichiarato As New DataSet
    '    Dim dsVersamenti As New DataSet
    '    Dim dsImmobiliAccertati As New DataSet
    '    Dim objDSTipiInteressi As New DataSet
    '    Dim objDSTipiInteressiL As New DataSet
    '    Dim objDSElencoSanzioni As New DataSet
    '    Dim objDSElencoSanzioniF2 As New DataSet
    '    Dim objDSElencoSanzioniF2Intr As New DataSet
    '    Dim objDSElencoSanzioniRiducibili As New DataSet
    '    Dim objDSElencoSanzioniF2Riducibili As New DataSet
    '    Dim objDSImportiInteressi As New DataSet
    '    Dim objDSImportiInteressiF2 As New DataSet
    '    '---------------------------------------
    '    'var per popolare i bookmark relativi 
    '    'alla sezione degli importi interessi
    '    Dim strImportoGiorni As String
    '    Dim strImportoSemestriACC As String
    '    Dim strImportoSemestriSAL As String
    '    Dim strNumSemestriACC As String
    '    Dim strNumSemestriSAL As String
    '    Dim iRetValImpInt As Boolean
    '    '---------------------------------------
    '    'Dim strConnectionStringOPENgovProvvedimenti As String

    '    'Dim objSessione As CreateSessione
    '    Dim strWFErrore As String
    '    Dim NomeDbIci As String = ConfigurationManager.AppSettings("NOME_DATABASE_ICI").ToString()
    '    Dim NomeDbOpenGov As String = ConfigurationManager.AppSettings("NOME_DATABASE_OPENGOV").ToString()

    '    Dim objHashTable As Hashtable = New Hashtable
    '    Dim objCOM As IElaborazioneLiquidazioni = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneLiquidazioni), ConfigurationManager.AppSettings("URLServiziLiquidazioni"))
    '    Dim objCOMTipoVoci As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)
    '    Dim objCOMACCERT As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
    '    Dim objICI As DichiarazioniICI.Database.TpSituazioneFinaleIci
    '    Dim objVers As DichiarazioniICI.Database.VersamentiTable
    '    'Dim dsDettaglioAnagrafica As DataSet = Nothing
    '    Dim oDettaglioAnagrafica As DettaglioAnagrafica
    '    Dim objAnagrafica As Anagrafica.DLL.GestioneAnagrafica
    '    Dim strViaRes, strCivRes, strFrazRes, strCapRes, strCittaRes, strProvRes As String
    '    Dim strViaCO, strCivCO, strFrazCO, strCapCO, strCittaCO, strProvCO, strNominativo_CO As String
    '    Dim strVia, strCiv, strFraz, strCap, strCitta, strProv As String
    '    Dim codice_fiscale, partita_iva As String

    '    ' COMPONGO IL PERCORSO DEL FILE DOT

    '    ' COMPONGO IL PERCORSO DEL FILE DOT
    '    sFilenameDOT = "Rimborso_ICI_" & ConstSession.IdEnte & ".dot"
    '    sFilenameDOC = "Rimborso_ICI_"
    '    strTIPODOCUMENTO = Costanti.COD_TIPO_DOC_RIMBORSO_ICI

    '    objTestataDOT.Atto = "Template"
    '    objTestataDOT.Dominio = "Provvedimenti"
    '    objTestataDOT.Ente = ConstSession.DescrizioneEnte
    '    objTestataDOT.Filename = sFilenameDOT

    '    'Dim DSperInsertTabStorico As DataSet = CreateDataSetStampa()
    '    'Dim dr As DataRow
    '    objSessione = New CreateSessione(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))
    'Try
    '    If Not objSessione.CreaSessione(ConstSession.UserName, strWFErrore) Then
    '        Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '    End If

    '    'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString

    '    objHashTable.Add("CodENTE", ConstSession.IdEnte)
    '    objHashTable.Add("CODENTE", ConstSession.IdEnte)
    '    objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
    '    objHashTable.Add("NomeDbIci", NomeDbIci)
    '    objHashTable.Add("NomeDbOpenGov", NomeDbOpenGov)
    '    objHashTable.Add("CODTRIBUTO", Session("COD_TRIBUTO"))

    '    objICI = New DichiarazioniICI.Database.TpSituazioneFinaleIci
    '    objVers = New DichiarazioniICI.Database.VersamentiTable(ConstSession.UserName)
    '    objAnagrafica = New Anagrafica.DLL.GestioneAnagrafica(objSessione.oSession, "OPENGOVA")

    '    For iCount = 0 To objDSstampa.Tables(0).Rows.Count - 1
    '        iCodContrib = objDSstampa.Tables(0).Rows(iCount)("COD_CONTRIBUENTE")
    '        IDPROVVEDIMENTO = objDSstampa.Tables(0).Rows(iCount)("ID_PROVVEDIMENTO")
    '        objHashTable.Add("COD_CONTRIBUENTE", iCodContrib)
    '        dsDatiProvv = objCOM.GetProvvedimentoPerStampaLiquidazione(objHashTable, IDPROVVEDIMENTO)

    '        IDPROCEDIMENTO = dsDatiProvv.Tables(0).Rows(iCount)("ID_PROCEDIMENTO")
    '        strANNO = dsDatiProvv.Tables(0).Rows(iCount)("ANNO")
    '        objHashTable.Add("ANNO", strANNO)

    '        objTestataDOC = New oggettoTestata
    '        objTestataDOC.Atto = "Documenti"
    '        objTestataDOC.Dominio = "Provvedimenti"
    '        objTestataDOC.Ente = ConstSession.DescrizioneEnte
    '        objTestataDOC.Filename = iCodContrib & "_" & ConstSession.IdEnte & sFilenameDOC

    '        oArrBookmark = New ArrayList


    '        '*** 20140701 - IMU/TARES ***
    '        If strANNO >= 2012 Then
    '            sFilenameDOT = sFilenameDOT.Replace("ICI", "IMU")
    '            sFilenameDOC = sFilenameDOC.Replace("ICI", "IMU")
    '            objTestataDOT.Filename = sFilenameDOT
    '        End If
    '        '*** ***
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "bozza"
    '        If bIsBozza = True Then
    '            objBookmark.Valore = "BOZZA"
    '        Else
    '            objBookmark.Valore = ""
    '        End If
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "nome_ente"
    '        objBookmark.Valore = CStr(Session("DESCRIZIONE_ENTE")).ToUpper
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "TipoProvvedimento"
    '        objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("TIPO_PROVVEDIMENTO")).ToUpper()
    '        oArrBookmark.Add(objBookmark)

    '        '************************************************************************************
    '        'DATI ANAGRAFICI
    '        '************************************************************************************
    '        oDettaglioAnagrafica = New DettaglioAnagrafica
    '        'oDettaglioAnagrafica = objAnagrafica.GetAnagrafica(iCodContrib, Session("COD_TRIBUTO"))
    '        oDettaglioAnagrafica = objAnagrafica.GetAnagrafica(iCodContrib, -1, ConstSession.DBType, ConstSession.StringConnectionAnagrafica, False) 'objAnagrafica.GetAnagrafica(iCodContrib, ConstSession.CodTributo, -1, ConstSession.DBType, ConstSession.StringConnectionAnagrafica)
    '        'dsDettaglioAnagrafica = addRowsObjAnagrafica(oDettaglioAnagrafica)

    '        Dim cognome, nome As String
    '        cognome = FormatStringToEmpty(oDettaglioAnagrafica.Cognome)
    '        nome = FormatStringToEmpty(oDettaglioAnagrafica.Nome)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "cognome"
    '        objBookmark.Valore = cognome
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "nome"
    '        objBookmark.Valore = nome
    '        oArrBookmark.Add(objBookmark)

    '        strViaRes = FormatStringToEmpty(oDettaglioAnagrafica.ViaResidenza)
    '        strCivRes = FormatStringToEmpty(oDettaglioAnagrafica.CivicoResidenza)
    '        strFrazRes = FormatStringToEmpty(oDettaglioAnagrafica.FrazioneResidenza)
    '        strCapRes = FormatStringToEmpty(oDettaglioAnagrafica.CapResidenza)
    '        strCittaRes = FormatStringToEmpty(oDettaglioAnagrafica.ComuneResidenza)
    '        strProvRes = FormatStringToEmpty(oDettaglioAnagrafica.ProvinciaResidenza)

    '        '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
    '        'strViaCO = FormatStringToEmpty(oDettaglioAnagrafica.ViaRCP)
    '        'strCivCO = FormatStringToEmpty(oDettaglioAnagrafica.CivicoRCP)
    '        'strFrazCO = FormatStringToEmpty(oDettaglioAnagrafica.FrazioneRCP)
    '        'strCapCO = FormatStringToEmpty(oDettaglioAnagrafica.CapRCP)
    '        'strCittaCO = FormatStringToEmpty(oDettaglioAnagrafica.ComuneRCP)
    '        'strProvCO = FormatStringToEmpty(oDettaglioAnagrafica.ProvinciaRCP)
    '        For Each mySped As ObjIndirizziSpedizione In oDettaglioAnagrafica.ListSpedizioni
    '            If mySped.CodTributo = Costanti.TRIBUTO_ICI Then
    '                cognome = FormatStringToEmpty(mySped.CognomeInvio)
    '                nome = FormatStringToEmpty(mySped.NomeInvio)
    '                strViaCO = FormatStringToEmpty(mySped.ViaRCP)
    '                strCivCO = FormatStringToEmpty(mySped.CivicoRCP)
    '                strFrazCO = FormatStringToEmpty(mySped.FrazioneRCP)
    '                strCapCO = FormatStringToEmpty(mySped.CapRCP)
    '                strCittaCO = FormatStringToEmpty(mySped.ComuneRCP)
    '                strProvCO = FormatStringToEmpty(mySped.ProvinciaRCP)
    '            End If
    '        Next
    '        '*** ***
    '        If (strViaCO = "") Then
    '            'visualizzo indirizzo residenza
    '            strVia = strViaRes.ToUpper()
    '            strCiv = strCivRes.ToUpper()
    '            strFraz = strFrazRes.ToUpper()
    '            strCap = strCapRes.ToUpper()
    '            strCitta = strCittaRes.ToUpper()
    '            strProv = strProvRes.ToUpper()
    '            strNominativo_CO = ""
    '        Else
    '            'visualizzo indirizzo spedizione
    '            strVia = strViaCO.ToUpper()
    '            strCiv = strCivCO.ToUpper()
    '            strFraz = strFrazCO.ToUpper()
    '            strCap = strCapCO.ToUpper()
    '            strCitta = strCittaCO.ToUpper()
    '            strProv = strProvCO.ToUpper()

    '            strNominativo_CO = "C/O " & cognome & " " & nome
    '            strNominativo_CO = strNominativo_CO.ToUpper()

    '        End If
    '        If strProv <> "" Then strProv = "(" & strProv & ")"
    '        'Nominativo_CO
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "Nominativo_CO"
    '        objBookmark.Valore = strNominativo_CO
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "via_residenza"
    '        objBookmark.Valore = strVia
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "civico_residenza"
    '        objBookmark.Valore = strCiv
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "frazione_residenza"
    '        objBookmark.Valore = strFraz
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "cap_residenza"
    '        objBookmark.Valore = strCap
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "citta_residenza"
    '        objBookmark.Valore = strCitta
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "prov_residenza"
    '        objBookmark.Valore = strProv
    '        oArrBookmark.Add(objBookmark)

    '        codice_fiscale = FormatStringToEmpty(oDettaglioAnagrafica.CodiceFiscale)
    '        partita_iva = FormatStringToEmpty(oDettaglioAnagrafica.PartitaIva)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "codice_fiscale"
    '        objBookmark.Valore = codice_fiscale
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "partita_iva"
    '        objBookmark.Valore = partita_iva
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "codice_fiscale_1"
    '        objBookmark.Valore = codice_fiscale
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "partita_iva_1"
    '        objBookmark.Valore = partita_iva
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "codice_fiscale_2"
    '        objBookmark.Valore = codice_fiscale
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "partita_iva_2"
    '        objBookmark.Valore = partita_iva
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "codice_fiscale_3"
    '        objBookmark.Valore = codice_fiscale
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "partita_iva_3"
    '        objBookmark.Valore = partita_iva
    '        oArrBookmark.Add(objBookmark)

    '        '---------------------------------------------------------------------------------

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "anno_ici"
    '        objBookmark.Valore = strANNO
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "anno_ici_1"
    '        objBookmark.Valore = strANNO
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "n_provvedimento"
    '        objBookmark.Valore = objUtility.CToStr(dsDatiProvv.Tables(0).Rows(iCount)("NUMERO_ATTO"))

    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "data_provvedimento"
    '        If dsDatiProvv.Tables(0).Rows(iCount)("DATA_ELABORAZIONE") Is System.DBNull.Value Then
    '            objBookmark.Valore = ""
    '        Else
    '            objBookmark.Valore = objUtility.GiraDataFromDB(dsDatiProvv.Tables(0).Rows(iCount)("DATA_ELABORAZIONE"))
    '        End If
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "anno_imposta"
    '        objBookmark.Valore = strANNO
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "anno_imposta1"
    '        objBookmark.Valore = strANNO
    '        oArrBookmark.Add(objBookmark)

    '        '************************************************************************************
    '        'ELENCO IMMOBILI DICHIARATI
    '        '************************************************************************************
    '        dsImmobiliDichiarato = objCOMACCERT.getImmobiliDichiaratiPerStampaAccertamenti(objHashTable, IDPROCEDIMENTO)
    '        strRiga = FillBookMarkDICHIARATO(dsImmobiliDichiarato, strANNO)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "elenco_immobili"
    '        objBookmark.Valore = strRiga
    '        oArrBookmark.Add(objBookmark)

    '        '************************************************************************************
    '        'ELENCO VERSAMENTI
    '        '************************************************************************************
    '        dsVersamenti = objCOM.getVersamentiPerStampaLiquidazione(objHashTable, IDPROCEDIMENTO, Costanti.ID_FASE1)
    '        strRiga = FillBookMarkVersamenti(dsVersamenti)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "elenco_versamenti"
    '        objBookmark.Valore = "Dettaglio Versamenti" & vbCrLf & strRiga
    '        oArrBookmark.Add(objBookmark)

    '        '************************************************************************************
    '        'ELENCO IMMOBILI ACCERATI
    '        '************************************************************************************
    '        dsImmobiliAccertati = objCOMACCERT.getImmobiliAccertatiPerStampaAccertamenti(objHashTable, IDPROCEDIMENTO)
    '        strRiga = FillBookMarkACCERTATO(dsImmobiliAccertati, strANNO)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "elenco_immobili_acce"
    '        objBookmark.Valore = strRiga
    '        oArrBookmark.Add(objBookmark)

    '        Dim acconto, saldo As Double
    '        strRiga = FillBookMarkIMPORTODOVACC(dsImmobiliAccertati, acconto, saldo)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "imp_dov_acc"
    '        objBookmark.Valore = FormatImport(acconto) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "imp_dov_saldo"
    '        objBookmark.Valore = FormatImport(saldo) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        '************************************************************************************
    '        'ELENCO SANZIONI APPLICATE CON IMPORTO 
    '        '************************************************************************************
    '        objHashTable.Add("riducibile", 0)
    '        'Sanzioni NON Riducibili
    '        objDSElencoSanzioni = objCOMACCERT.GetElencoSanzioniPerStampaAccertamenti(objHashTable, IDPROVVEDIMENTO)
    '        objDSElencoSanzioniF2 = objCOM.GetElencoSanzioniPerStampaLiquidazione(objHashTable, IDPROVVEDIMENTO)
    '        '*************************
    '        'Sanzioni Riducibili
    '        objHashTable.Remove("riducibile")
    '        objHashTable.Add("riducibile", 1)
    '        objDSElencoSanzioniRiducibili = objCOMACCERT.GetElencoSanzioniPerStampaAccertamenti(objHashTable, IDPROVVEDIMENTO)
    '        objDSElencoSanzioniF2Riducibili = objCOM.GetElencoSanzioniPerStampaLiquidazione(objHashTable, IDPROVVEDIMENTO)
    '        '*************************
    '        'Sanzioni Intrasmissibilità agli eredi 
    '        objHashTable.Remove("riducibile")
    '        objHashTable.Add("riducibile", "")
    '        objDSElencoSanzioniF2Intr = objCOM.GetElencoSanzioniPerStampaLiquidazione(objHashTable, IDPROVVEDIMENTO)
    '        '*************************
    '        strRiga = FillBookMarkELENCOSANZIONI(objDSElencoSanzioni, objDSElencoSanzioniF2, objDSElencoSanzioniRiducibili, objDSElencoSanzioniF2Riducibili, objDSElencoSanzioniF2Intr)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "elenco_sanzioni"
    '        objBookmark.Valore = strRiga
    '        oArrBookmark.Add(objBookmark)

    '        '************************************************************************************
    '        'IMPORTI (DIFF IMPOSTA - SANZIONI - INTERESSI - SPESE - ecc...
    '        '************************************************************************************
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "imposta_dovuta"
    '        'objBookmark.Valore = FormatNumberToZero(dsDatiProvv.Tables(0).Rows(iCount)("TOTALE_DICHIARATO")).Replace(",", ".") & " €"
    '        objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("TOTALE_DICHIARATO")) & " €"
    '        oArrBookmark.Add(objBookmark)
    '        '*** 20130121 - devo stampare il dovuto accertato altrimenti non tornano i conti nel prospetto ***
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "imposta_dovuta_accer"
    '        objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_ACCERTATO_ACC")) & " €"
    '        oArrBookmark.Add(objBookmark)
    '        '*** ***

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "imposta_versata"
    '        'objBookmark.Valore = FormatNumberToZero(dsDatiProvv.Tables(0).Rows(iCount)("TOTALE_VERSATO")).Replace(",", ".") & " €"
    '        objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("TOTALE_VERSATO")) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        Dim ImpDov, ImpVers As Double
    '        Dim tipo_versamento As String = ""

    '        ImpDov = dsDatiProvv.Tables(0).Rows(iCount)("TOTALE_DICHIARATO")
    '        ImpVers = dsDatiProvv.Tables(0).Rows(iCount)("TOTALE_VERSATO")

    '        If ImpDov > 0 And (ImpVers > 0 And ImpVers < ImpDov) Then
    '            tipo_versamento = "parziale"
    '        ElseIf ImpDov > 0 And ImpVers = 0 Then
    '            tipo_versamento = "omesso"
    '        End If
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "tipo_versamento"
    '        objBookmark.Valore = tipo_versamento
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "ImpostaAccertata"
    '        'objBookmark.Valore = FormatNumberToZero(dsDatiProvv.Tables(0).Rows(iCount)("TOTALE_VERSATO")).Replace(",", ".") & " €"
    '        objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("TOTALE_ACCERTATO")) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        'dipe
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "ImpostaAccertata_60g"
    '        objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("TOTALE_ACCERTATO")) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "DiffImpostaDaVersare"
    '        'objBookmark.Valore = FormatNumberToZero(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_DIFFERENZA_IMPOSTA")).Replace(",", ".") & " €"
    '        Dim strImportoDiffImp As String = dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_DIFFERENZA_IMPOSTA")
    '        objBookmark.Valore = FormatImport(strImportoDiffImp) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        'dipe
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "DiffImpostaDaVer_60g"
    '        strImportoDiffImp = strImportoDiffImp * (-1)
    '        objBookmark.Valore = FormatImport(strImportoDiffImp) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "DiffImpDaVer_60g_1"
    '        objBookmark.Valore = FormatImport(strImportoDiffImp) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        '********************************************************************************
    '        '************ GESTIONE IMPORTI SANZIONI *****************************************
    '        '********************************************************************************
    '        Dim strImportoSanzioneRidotto As String
    '        objBookmark = New oggettiStampa
    '        strImportoSanzioneRidotto = FillBookMarkSanzioniRiducibili(objDSElencoSanzioniRiducibili)
    '        objBookmark.Descrizione = "ImportoSanzioneRid"
    '        objBookmark.Valore = FormatImport(strImportoSanzioneRidotto) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        Dim ImpSanzioni As String
    '        ImpSanzioni = CType((CType(FillBookMarkSanzioniRiducibili(objDSElencoSanzioni), Double) + CType(FillBookMarkSanzioniRiducibili(objDSElencoSanzioniF2), Double)), String)
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "ImportoSanzione"
    '        objBookmark.Valore = FormatImport(ImpSanzioni) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        Dim strImportoSanzioneRidotto_60g As String
    '        strImportoSanzioneRidotto_60g = (strImportoSanzioneRidotto / 4)
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "ImpSanzioneRid_60g"
    '        objBookmark.Valore = FormatImport(strImportoSanzioneRidotto_60g) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        Dim ImportoSanzione_60g As String
    '        ImportoSanzione_60g = ImpSanzioni
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "ImportoSanzione_60g"
    '        objBookmark.Valore = FormatImport(ImportoSanzione_60g) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        Dim spese_notifica As String
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "spese_notifica"
    '        If dsDatiProvv.Tables(0).Rows.Count > 0 Then
    '            spese_notifica = dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SPESE")
    '        Else
    '            spese_notifica = 0
    '        End If

    '        objBookmark.Valore = FormatImport(spese_notifica) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        'dipe
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "spese_notifica_60g"
    '        objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SPESE")) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        Dim Importo_arrotond As String
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "Importo_arrotond"
    '        'objBookmark.Valore = FormatNumberToZero(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_ARROTONDAMENTO")).Replace(",", ".") & " €"
    '        Importo_arrotond = dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_ARROTONDAMENTO")
    '        objBookmark.Valore = FormatImport(Importo_arrotond) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        '************************************************************************************
    '        'IMPORTI INTERESSI
    '        '************************************************************************************
    '        objDSImportiInteressi = objCOMACCERT.GetInteressiTotaliPerStampaAccertamenti(objHashTable, IDPROVVEDIMENTO)
    '        objDSImportiInteressiF2 = objCOM.GetInteressiTotaliPerStampaLiquidazione(objHashTable, IDPROVVEDIMENTO)
    '        iRetValImpInt = FillBookMarkIMPORTIINTERESSI(objDSImportiInteressi, objDSImportiInteressiF2, strImportoGiorni, strImportoSemestriACC, strImportoSemestriSAL, strNumSemestriACC, strNumSemestriSAL)
    '        Dim int_mor As String = 0
    '        If iRetValImpInt = True Then
    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "imp_interessi_GIORNI"
    '            'objBookmark.Valore = strImportoGiorni.Replace(",", ".") & " €"
    '            objBookmark.Valore = strImportoGiorni & " €"
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "imp_semestri_ACCONTO"
    '            'objBookmark.Valore = strImportoSemestriACC.Replace(",", ".") & " €"
    '            objBookmark.Valore = strImportoSemestriACC & " €"
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "num_semestri_ACCONTO"
    '            objBookmark.Valore = strNumSemestriACC
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "imp_semestri_SALDO"
    '            'objBookmark.Valore = strImportoSemestriSAL.Replace(",", ".") & " €"
    '            objBookmark.Valore = strImportoSemestriSAL & " €"
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "num_semestri_SALDO"
    '            objBookmark.Valore = strNumSemestriSAL
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "int_mor"
    '            int_mor = CDbl(strImportoGiorni) + CDbl(strImportoSemestriACC) + CDbl(strImportoSemestriSAL)
    '            objBookmark.Valore = FormatImport(int_mor) & " €"
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "int_mor_60g"
    '            objBookmark.Valore = FormatImport(CDbl(strImportoGiorni) * (-1) + CDbl(strImportoSemestriACC) * (-1) + CDbl(strImportoSemestriSAL) * (-1)) & " €"
    '            oArrBookmark.Add(objBookmark)
    '        End If
    '        '************************************************************************************
    '        'FINE IMPORTI INTERESSI
    '        '************************************************************************************

    '        '************************************************************************************
    '        'TOTALI
    '        '************************************************************************************
    '        Dim strImportoTotale As String
    '        'strImportoTotale = CDbl(strImportoDiffImp) + CDbl(int_mor) + CDbl(ImpSanzioni) + CDbl(strImportoSanzioneRidotto) + CDbl(spese_notifica) + CDbl(Importo_arrotond)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "Importo_totale"
    '        strImportoTotale = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_TOTALE"))
    '        'objBookmark.Valore = FormatImport(CDbl(strImportoTotale) - CDbl(strImportoSanzione) + CDbl(strImportoSanzioneRidotto)) & " €"
    '        objBookmark.Valore = FormatImport(strImportoTotale) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        Dim dblIMPORTOARROTONDAMENTO, dblIMPORTOARROTONDATO As Double
    '        Dim Importo_totale_60g As String
    '        'Importo_totale_60g = CDbl(strImportoDiffImp) + CDbl(int_mor) + CDbl(ImportoSanzione_60g) + CDbl(strImportoSanzioneRidotto) + CDbl(spese_notifica) + CDbl(Importo_arrotond)
    '        'Importo_totale_60g = CDbl(strImportoDiffImp) + CDbl(int_mor) + CDbl(ImportoSanzione_60g) + CDbl(strImportoSanzioneRidotto_60g) + CDbl(spese_notifica)
    '        'dblIMPORTOARROTONDATO = ImportoArrotondato(CDbl(Importo_totale_60g))
    '        'dblIMPORTOARROTONDAMENTO = dblIMPORTOARROTONDATO - CDbl(Importo_totale_60g)
    '        'Importo_totale_60g = dblIMPORTOARROTONDATO
    '        Importo_totale_60g = dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_TOTALE_RIDOTTO")
    '        Importo_totale_60g = Importo_totale_60g * (-1)
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "Importo_totale_60g"
    '        objBookmark.Valore = FormatImport(Importo_totale_60g) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        'dipe
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "Importo_arrotond_60g"
    '        dblIMPORTOARROTONDAMENTO = dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_ARROTONDAMENTO_RIDOTTO")
    '        dblIMPORTOARROTONDAMENTO = dblIMPORTOARROTONDAMENTO * (-1)
    '        objBookmark.Valore = FormatImport(dblIMPORTOARROTONDAMENTO) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "ImpTotNonRidotto"
    '        'objBookmark.Valore = FormatImport(CDbl(strImportoTotale) - CDbl(strImportoSanzione) + CDbl(strImportoSanzioneRidotto)) & " €"
    '        objBookmark.Valore = FormatImport(Importo_totale_60g) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "Importo_totale_1"
    '        'objBookmark.Valore = FormatImport(CDbl(strImportoTotale) - CDbl(strImportoSanzione_60g) + CDbl(strImportoSanzioneRidotto_60g)) & " €"
    '        objBookmark.Valore = FormatImport(Importo_totale_60g) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        '************************************************************************************
    '        'IMPORTI DA DICHIARATO
    '        '************************************************************************************
    '        'Dim dtICI As DataTable
    '        'dtICI = objICI.GetImportoDovuto(iCodContrib, strANNO, constsession.idente)
    '        Dim imp_dov_dich_acc, imp_dov_dich_saldo As Decimal
    '        'dsImmobiliDichiarato = objCOMACCERT.getImmobiliDichiaratiPerStampaAccertamenti(objHashTable, IDPROCEDIMENTO)
    '        FillBookMarkIMP_DOV_DICH(dsImmobiliDichiarato, imp_dov_dich_acc, imp_dov_dich_saldo)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "imp_dov_dich_acc"
    '        'objBookmark.Valore = FormatImport(dtICI.Rows(0)("ACCONTO")) & " €"
    '        objBookmark.Valore = FormatImport(imp_dov_dich_acc) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "imp_dov_dich_saldo"
    '        'objBookmark.Valore = FormatImport(dtICI.Rows(0)("SALDO")) & " €"
    '        objBookmark.Valore = FormatImport(imp_dov_dich_saldo) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        '************************************************************************************
    '        'FINE IMPORTI DA DICHIARATO
    '        '************************************************************************************

    '        '************************************************************************************
    '        'IMPORTI VERSATI
    '        '************************************************************************************
    '        Dim dvVers As DataView
    '        Dim i As Integer
    '        Dim dTot, dTotUS, dTotNoAS As Double

    '        'Senza Flag Acconto e saldo selezionati
    '        dvVers = objVers.GetVersamentiPerTipologia(ConstSession.IdEnte, strANNO, iCodContrib, False, False)
    '        dTotNoAS = 0
    '        For i = 0 To dvVers.Table.Rows.Count - 1
    '            dTotNoAS += dvVers.Table.Rows(i)("ImportoPagato")
    '        Next

    '        'Unica soluzione
    '        dvVers = objVers.GetVersamentiPerTipologia(ConstSession.IdEnte, strANNO, iCodContrib, True, True)
    '        dTotUS = 0
    '        For i = 0 To dvVers.Table.Rows.Count - 1
    '            dTotUS += dvVers.Table.Rows(i)("ImportoPagato")
    '        Next

    '        'Acconto
    '        dvVers = objVers.GetVersamentiPerTipologia(ConstSession.IdEnte, strANNO, iCodContrib, True, False)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "imp_vers_acc"
    '        dTot = dTotUS + dTotNoAS
    '        For i = 0 To dvVers.Table.Rows.Count - 1
    '            dTot += dvVers.Table.Rows(i)("ImportoPagato")
    '        Next
    '        objBookmark.Valore = FormatImport(dTot) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        'saldo
    '        dvVers = objVers.GetVersamentiPerTipologia(ConstSession.IdEnte, strANNO, iCodContrib, False, True)
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "imp_vers_saldo"
    '        dTot = 0
    '        For i = 0 To dvVers.Table.Rows.Count - 1
    '            dTot += dvVers.Table.Rows(i)("ImportoPagato")
    '        Next
    '        objBookmark.Valore = FormatImport(dTot) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        '************************************************************************************
    '        'FINE IMPORTI VERSATI
    '        '************************************************************************************
    '        ArrayBookMark = CType(oArrBookmark.ToArray(GetType(oggettiStampa)), oggettiStampa())

    '        objToPrint = New oggettoDaStampareCompleto
    '        objToPrint.TestataDOC = objTestataDOC
    '        objToPrint.TestataDOT = objTestataDOT
    '        objToPrint.Stampa = ArrayBookMark

    '        oArrListOggettiDaStampare.Add(objToPrint)
    '    Next

    '    Dim GruppoDOC As New GruppoDocumenti
    '    Dim GruppoDOCUMENTI As GruppoDocumenti()
    '    Dim ArrListGruppoDOC As New ArrayList

    '    Dim ArrOggCompleto As oggettoDaStampareCompleto()
    '    Dim objTestataGruppo As New oggettoTestata

    '    ArrOggCompleto = CType(oArrListOggettiDaStampare.ToArray(GetType(oggettoDaStampareCompleto)), oggettoDaStampareCompleto())

    '    GruppoDOC.OggettiDaStampare = ArrOggCompleto
    '    GruppoDOC.TestataGruppo = objTestataGruppo

    '    ArrListGruppoDOC.Add(GruppoDOC)

    '    GruppoDOCUMENTI = CType(ArrListGruppoDOC.ToArray(GetType(GruppoDocumenti)), GruppoDocumenti())

    '    'oggettoDaStampare[]  outArray = (oggettoDaStampare[])oArrListOggettiDaStampare.ToArray(typeof(oggettoDaStampare));

    '    ' oggetto per la stampa
    '    Dim oInterfaceStampaDocOggetti As IElaborazioneStampaDocOggetti
    '    oInterfaceStampaDocOggetti = Activator.GetObject(GetType(IElaborazioneStampaDocOggetti), ConfigurationManager.AppSettings("ServizioStampe").ToString())

    '    Dim retArray As GruppoURL

    '    Dim objTestataComplessiva As New RIBESElaborazioneDocumentiInterface.Stampa.oggetti.oggettoTestata
    '    objTestataComplessiva.Atto = ""
    '    objTestataComplessiva.Dominio = ""
    '    objTestataComplessiva.Ente = ""
    '    objTestataComplessiva.Filename = ""
    '    ' definisco anche il numero di documenti che voglio stampare.
    '    retArray = oInterfaceStampaDocOggetti.StampaDocumenti(objTestataComplessiva, GruppoDOCUMENTI)

    '    Session.Remove("ELENCO_ACCERTAMENTI_STAMPATI")

    '    If Not retArray Is Nothing Then
    '        Session.Add("ELENCO_ACCERTAMENTI_STAMPATI", retArray)
    '        dim sScript as string=""
    '        sscript+="parent.corpo.location.href='ViewAccertamentiStampati.aspx';" 
    '        RegisterScript(sScript , Me.GetType())
    '    End If
    '    If Not IsNothing(objSessione) Then
    '        objSessione.Kill()
    '        objSessione = Nothing
    '    End If
    ' Catch ex As Exception
    '  Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.STAMPA_RIMBORSOICI.errore: ", ex)
    '  Response.Redirect("../../../PaginaErrore.aspx")
    'End Try
    'End Function

    'Private Function STAMPA_ANNULLAMENTO(ByVal strTipoDoc As String, ByVal bIsBozza As Boolean)
    '    Dim sFilenameDOT, sFilenameDOC As String
    '    Dim objDSstampa As DataSet
    '    Dim objUtility As New MyUtility
    '    ' creo l'oggetto testata per l'oggetto da stampare
    '    'serve per indicare la posizione di salvataggio e il nome del file.
    '    Dim objTestataDOC As oggettoTestata
    '    Dim objTestataDOT As New oggettoTestata
    '    Dim IDPROVVEDIMENTO As String
    '    Dim IDPROCEDIMENTO As String
    '    Dim strTIPODOCUMENTO As String
    '    Dim strANNO As String
    '    Dim oArrBookmark As ArrayList
    '    Dim iCount As Integer
    '    'Dim iImmobili As Integer
    '    'Dim iErrori As Integer

    '    Dim objBookmark As oggettiStampa
    '    Dim oArrListOggettiDaStampare As New ArrayList
    '    Dim objToPrint As oggettoDaStampareCompleto
    '    Dim ArrayBookMark As oggettiStampa()
    '    Dim iCodContrib As Integer

    '    'Dim strRiga, strelencointeressi As String
    '    'Dim strImmoTemp As String = String.Empty
    '    'Dim strErroriTemp As String = String.Empty
    '    'Dim strImmoTempTitolo As String = String.Empty
    '    'Dim Anno As String = String.Empty

    '    Dim dsDatiProvv As New DataSet
    '    'Dim dsImmobiliDichiarato As New DataSet
    '    'Dim dsVersamenti As New DataSet
    '    'Dim dsImmobiliAccertati As New DataSet
    '    'Dim objDSTipiInteressi As New DataSet
    '    'Dim objDSTipiInteressiL As New DataSet
    '    'Dim objDSElencoSanzioni As New DataSet
    '    'Dim objDSElencoSanzioniF2 As New DataSet
    '    'Dim objDSElencoSanzioniF2Intr As New DataSet
    '    'Dim objDSElencoSanzioniRiducibili As New DataSet
    '    'Dim objDSElencoSanzioniF2Riducibili As New DataSet
    '    'Dim objDSImportiInteressi As New DataSet
    '    'Dim objDSImportiInteressiF2 As New DataSet
    '    ''---------------------------------------
    '    ''var per popolare i bookmark relativi 
    '    ''alla sezione degli importi interessi
    '    'Dim strImportoGiorni As String
    '    'Dim strImportoSemestriACC As String
    '    'Dim strImportoSemestriSAL As String
    '    'Dim strNumSemestriACC As String
    '    'Dim strNumSemestriSAL As String
    '    'Dim iRetValImpInt As Boolean
    '    '---------------------------------------

    '    'Dim DSperInsertTabStorico As DataSet = CreateDataSetStampa()
    '    'Dim dr As DataRow

    '    'Dim strConnectionStringOPENgovProvvedimenti As String
    '    'Dim objSessione As CreateSessione
    '    Dim strWFErrore As String

    '    'contiene i dati anagrafici visualizzati nella grilgia della ricerca lettere
    '    objDSstampa = CType(Session("PROVVEDIMENTI_DA_STAMPARE"), DataSet)

    '    ' COMPONGO IL PERCORSO DEL FILE DOT

    '    sFilenameDOT = "Annullamento" & Session("COD_TRIBUTO") & "_" & ConstSession.IdEnte & ".dot"
    '    sFilenameDOC = "Annullamento" & Session("COD_TRIBUTO") & "_"
    '    strTIPODOCUMENTO = Costanti.COD_TIPO_DOC_ANNULLAMENTO

    '    objTestataDOT.Atto = "Template"
    '    objTestataDOT.Dominio = "Provvedimenti"
    '    objTestataDOT.Ente = ConstSession.DescrizioneEnte
    '    objTestataDOT.Filename = sFilenameDOT

    '    objSessione = New CreateSessione(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))
    '    If Not objSessione.CreaSessione(ConstSession.UserName, strWFErrore) Then
    '        Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '    End If

    '    'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString

    '    Dim NomeDbIci As String = ConfigurationManager.AppSettings("NOME_DATABASE_ICI").ToString()
    '    Dim NomeDbOpenGov As String = ConfigurationManager.AppSettings("NOME_DATABASE_OPENGOV").ToString()

    '    Dim objHashTable As Hashtable = New Hashtable
    '    objHashTable.Add("CodENTE", ConstSession.IdEnte)
    '    objHashTable.Add("CODENTE", ConstSession.IdEnte)
    '    objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
    '    objHashTable.Add("NomeDbIci", NomeDbIci)
    '    objHashTable.Add("NomeDbOpenGov", NomeDbOpenGov)
    '    objHashTable.Add("CODTRIBUTO", Session("COD_TRIBUTO"))

    '    Dim objCOM As IElaborazioneLiquidazioni = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneLiquidazioni), ConfigurationManager.AppSettings("URLServiziLiquidazioni"))
    '    Dim objCOMTipoVoci As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)
    '    Dim objCOMACCERT As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
    '    'Dim objICI As DichiarazioniICI.Database.TpSituazioneFinaleIci
    '    'objICI = New DichiarazioniICI.Database.TpSituazioneFinaleIci

    '    'Dim objVers As DichiarazioniICI.Database.VersamentiTable
    '    'objVers = New DichiarazioniICI.Database.VersamentiTable(ConstSession.UserName)

    '    'Dim dsDettaglioAnagrafica As DataSet = Nothing
    '    Dim oDettaglioAnagrafica As DettaglioAnagrafica
    '    Dim objAnagrafica As Anagrafica.DLL.GestioneAnagrafica
    '    objAnagrafica = New Anagrafica.DLL.GestioneAnagrafica(objSessione.oSession, "OPENGOVA")

    '    Dim strViaRes, strCivRes, strFrazRes, strCapRes, strCittaRes, strProvRes As String
    '    Dim strViaCO, strCivCO, strFrazCO, strCapCO, strCittaCO, strProvCO, strNominativo_CO As String
    '    Dim strVia, strCiv, strFraz, strCap, strCitta, strProv As String
    '    Dim codice_fiscale, partita_iva As String
    'Try
    '    For iCount = 0 To objDSstampa.Tables(0).Rows.Count - 1
    '        iCodContrib = objDSstampa.Tables(0).Rows(iCount)("COD_CONTRIBUENTE")

    '        IDPROVVEDIMENTO = objDSstampa.Tables(0).Rows(iCount)("ID_PROVVEDIMENTO")
    '        objHashTable.Add("COD_CONTRIBUENTE", iCodContrib)
    '        dsDatiProvv = objCOM.GetProvvedimentoPerStampaLiquidazione(objHashTable, IDPROVVEDIMENTO)
    '        strANNO = dsDatiProvv.Tables(0).Rows(iCount)("ANNO")
    '        objHashTable.Add("ANNO", strANNO)
    '        '*** 20140701 - IMU/TARES ***
    '        If strANNO >= 2012 And Session("COD_TRIBUTO") = "8852" Then
    '            sFilenameDOT = sFilenameDOT.Replace("8852", "IMU")
    '            sFilenameDOC = sFilenameDOC.Replace("8852", "IMU")
    '            objTestataDOT.Filename = sFilenameDOT
    '        End If
    '        '*** ***
    '        objTestataDOC = New oggettoTestata
    '        objTestataDOC.Atto = "Documenti"
    '        objTestataDOC.Dominio = "Provvedimenti"
    '        objTestataDOC.Ente = ConstSession.DescrizioneEnte
    '        objTestataDOC.Filename = iCodContrib & "_" & ConstSession.IdEnte & sFilenameDOC

    '        oArrBookmark = New ArrayList



    '        IDPROCEDIMENTO = dsDatiProvv.Tables(0).Rows(iCount)("ID_PROCEDIMENTO")

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "bozza"
    '        If bIsBozza = True Then
    '            objBookmark.Valore = "BOZZA"
    '        Else
    '            objBookmark.Valore = ""
    '        End If
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "nome_ente"
    '        objBookmark.Valore = CStr(Session("DESCRIZIONE_ENTE")).ToUpper
    '        oArrBookmark.Add(objBookmark)

    '        '************************************************************************************
    '        'DATI ANAGRAFICI
    '        '************************************************************************************
    '        oDettaglioAnagrafica = New DettaglioAnagrafica
    '        'oDettaglioAnagrafica = objAnagrafica.GetAnagrafica(iCodContrib, Session("COD_TRIBUTO"))
    '        oDettaglioAnagrafica = objAnagrafica.GetAnagrafica(iCodContrib, -1, ConstSession.DBType, ConstSession.StringConnectionAnagrafica, False) 'objAnagrafica.GetAnagrafica(iCodContrib, ConstSession.CodTributo, -1, ConstSession.DBType, ConstSession.StringConnectionAnagrafica)
    '        'dsDettaglioAnagrafica = addRowsObjAnagrafica(oDettaglioAnagrafica)

    '        Dim cognome, nome As String
    '        cognome = FormatStringToEmpty(oDettaglioAnagrafica.Cognome)
    '        nome = FormatStringToEmpty(oDettaglioAnagrafica.Nome)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "cognome"
    '        objBookmark.Valore = cognome
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "nome"
    '        objBookmark.Valore = nome
    '        oArrBookmark.Add(objBookmark)

    '        strViaRes = FormatStringToEmpty(oDettaglioAnagrafica.ViaResidenza)
    '        strCivRes = FormatStringToEmpty(oDettaglioAnagrafica.CivicoResidenza)
    '        strFrazRes = FormatStringToEmpty(oDettaglioAnagrafica.FrazioneResidenza)
    '        strCapRes = FormatStringToEmpty(oDettaglioAnagrafica.CapResidenza)
    '        strCittaRes = FormatStringToEmpty(oDettaglioAnagrafica.ComuneResidenza)
    '        strProvRes = FormatStringToEmpty(oDettaglioAnagrafica.ProvinciaResidenza)

    '        '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
    '        'strViaCO = FormatStringToEmpty(oDettaglioAnagrafica.ViaRCP)
    '        'strCivCO = FormatStringToEmpty(oDettaglioAnagrafica.CivicoRCP)
    '        'strFrazCO = FormatStringToEmpty(oDettaglioAnagrafica.FrazioneRCP)
    '        'strCapCO = FormatStringToEmpty(oDettaglioAnagrafica.CapRCP)
    '        'strCittaCO = FormatStringToEmpty(oDettaglioAnagrafica.ComuneRCP)
    '        'strProvCO = FormatStringToEmpty(oDettaglioAnagrafica.ProvinciaRCP)
    '        For Each mySped As ObjIndirizziSpedizione In oDettaglioAnagrafica.ListSpedizioni
    '            If mySped.CodTributo = ConstSession.CodTributo Then
    '                cognome = FormatStringToEmpty(mySped.CognomeInvio)
    '                nome = FormatStringToEmpty(mySped.NomeInvio)
    '                strViaCO = FormatStringToEmpty(mySped.ViaRCP)
    '                strCivCO = FormatStringToEmpty(mySped.CivicoRCP)
    '                strFrazCO = FormatStringToEmpty(mySped.FrazioneRCP)
    '                strCapCO = FormatStringToEmpty(mySped.CapRCP)
    '                strCittaCO = FormatStringToEmpty(mySped.ComuneRCP)
    '                strProvCO = FormatStringToEmpty(mySped.ProvinciaRCP)
    '            End If
    '        Next
    '        '*** ***
    '        If (strViaCO = "") Then
    '            'visualizzo indirizzo residenza
    '            strVia = strViaRes.ToUpper()
    '            strCiv = strCivRes.ToUpper()
    '            strFraz = strFrazRes.ToUpper()
    '            strCap = strCapRes.ToUpper()
    '            strCitta = strCittaRes.ToUpper()
    '            strProv = strProvRes.ToUpper()
    '            strNominativo_CO = ""
    '        Else
    '            'visualizzo indirizzo spedizione
    '            strVia = strViaCO.ToUpper()
    '            strCiv = strCivCO.ToUpper()
    '            strFraz = strFrazCO.ToUpper()
    '            strCap = strCapCO.ToUpper()
    '            strCitta = strCittaCO.ToUpper()
    '            strProv = strProvCO.ToUpper()

    '            strNominativo_CO = "C/O " & cognome & " " & nome
    '            strNominativo_CO = strNominativo_CO.ToUpper()
    '        End If
    '        If strProv <> "" Then strProv = "(" & strProv & ")"
    '        'Nominativo_CO
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "Nominativo_CO"
    '        objBookmark.Valore = strNominativo_CO
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "via_residenza"
    '        objBookmark.Valore = strVia
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "civico_residenza"
    '        objBookmark.Valore = strCiv
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "frazione_residenza"
    '        objBookmark.Valore = strFraz
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "cap_residenza"
    '        objBookmark.Valore = strCap
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "citta_residenza"
    '        objBookmark.Valore = strCitta
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "prov_residenza"
    '        objBookmark.Valore = strProv
    '        oArrBookmark.Add(objBookmark)

    '        codice_fiscale = FormatStringToEmpty(oDettaglioAnagrafica.CodiceFiscale)
    '        partita_iva = FormatStringToEmpty(oDettaglioAnagrafica.PartitaIva)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "codice_fiscale"
    '        objBookmark.Valore = codice_fiscale
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "partita_iva"
    '        objBookmark.Valore = partita_iva
    '        oArrBookmark.Add(objBookmark)
    '        '---------------------------------------------------------------------------------

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "anno_ici"
    '        objBookmark.Valore = strANNO
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "anno_provvedimento_ann"
    '        objBookmark.Valore = strANNO
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "n_provvedimento"
    '        objBookmark.Valore = objUtility.CToStr(dsDatiProvv.Tables(0).Rows(iCount)("NUMERO_ATTO"))
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "n_provvedimento_ann"
    '        objBookmark.Valore = objUtility.CToStr(dsDatiProvv.Tables(0).Rows(iCount)("NUMERO_ATTO"))
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "data_provvedimento"
    '        If dsDatiProvv.Tables(0).Rows(iCount)("DATA_ELABORAZIONE") Is System.DBNull.Value Then
    '            objBookmark.Valore = ""
    '        Else
    '            objBookmark.Valore = objUtility.GiraDataFromDB(dsDatiProvv.Tables(0).Rows(iCount)("DATA_ELABORAZIONE"))
    '        End If
    '        oArrBookmark.Add(objBookmark)

    '        ArrayBookMark = CType(oArrBookmark.ToArray(GetType(oggettiStampa)), oggettiStampa())

    '        objToPrint = New oggettoDaStampareCompleto
    '        objToPrint.TestataDOC = objTestataDOC
    '        objToPrint.TestataDOT = objTestataDOT
    '        objToPrint.Stampa = ArrayBookMark

    '        oArrListOggettiDaStampare.Add(objToPrint)
    '    Next

    '    Dim GruppoDOC As New GruppoDocumenti
    '    Dim GruppoDOCUMENTI As GruppoDocumenti()
    '    Dim ArrListGruppoDOC As New ArrayList

    '    Dim ArrOggCompleto As oggettoDaStampareCompleto()
    '    Dim objTestataGruppo As New oggettoTestata

    '    ArrOggCompleto = CType(oArrListOggettiDaStampare.ToArray(GetType(oggettoDaStampareCompleto)), oggettoDaStampareCompleto())

    '    GruppoDOC.OggettiDaStampare = ArrOggCompleto
    '    GruppoDOC.TestataGruppo = objTestataGruppo

    '    ArrListGruppoDOC.Add(GruppoDOC)

    '    GruppoDOCUMENTI = CType(ArrListGruppoDOC.ToArray(GetType(GruppoDocumenti)), GruppoDocumenti())

    '    'oggettoDaStampare[]  outArray = (oggettoDaStampare[])oArrListOggettiDaStampare.ToArray(typeof(oggettoDaStampare));

    '    ' oggetto per la stampa
    '    Dim oInterfaceStampaDocOggetti As IElaborazioneStampaDocOggetti
    '    oInterfaceStampaDocOggetti = Activator.GetObject(GetType(IElaborazioneStampaDocOggetti), ConfigurationManager.AppSettings("ServizioStampe").ToString())

    '    Dim retArray As GruppoURL

    '    Dim objTestataComplessiva As New RIBESElaborazioneDocumentiInterface.Stampa.oggetti.oggettoTestata
    '    objTestataComplessiva.Atto = ""
    '    objTestataComplessiva.Dominio = ""
    '    objTestataComplessiva.Ente = ""
    '    objTestataComplessiva.Filename = ""
    '    ' definisco anche il numero di documenti che voglio stampare.
    '    retArray = oInterfaceStampaDocOggetti.StampaDocumenti(objTestataComplessiva, GruppoDOCUMENTI)

    '    Session.Remove("ELENCO_ACCERTAMENTI_STAMPATI")

    '    If Not retArray Is Nothing Then
    '        Session.Add("ELENCO_ACCERTAMENTI_STAMPATI", retArray)
    '        dim sScript as string=""
    '        sscript+="parent.corpo.location.href='ViewAccertamentiStampati.aspx';" 
    '        RegisterScript(sScript , Me.GetType())
    '    End If
    '    If Not IsNothing(objSessione) Then
    '        objSessione.Kill()
    '        objSessione = Nothing
    '    End If
    ' Catch ex As Exception
    '  Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.STAMPA_ANNULLAMENTO.errore: ", ex)
    '  Response.Redirect("../../../PaginaErrore.aspx")
    'End Try
    'End Function
    'Private Sub STAMPA_ACCERTAMENTOICI(ByVal strTipoDoc As String, ByVal bIsBozza As Boolean, Tributo As String)
    '    Try
    '        Dim sFilenameDOT, sFilenameDOC As String
    '        Dim objDSstampa As DataSet
    '        Dim objUtility As New MyUtility
    '        'contiene i dati anagrafici visualizzati nella grilgia della ricerca lettere
    '        objDSstampa = CType(Session("PROVVEDIMENTI_DA_STAMPARE"), DataSet)

    '        ' creo l'oggetto testata per l'oggetto da stampare
    '        'serve per indicare la posizione di salvataggio e il nome del file.
    '        Dim objTestataDOC As oggettoTestata
    '        Dim objTestataDOT As New oggettoTestata
    '        Dim FileNameContrib As String
    '        Dim IDPROVVEDIMENTO As String
    '        Dim IDPROCEDIMENTO As String
    '        Dim strTIPODOCUMENTO As String
    '        Dim sAnno As String

    '        ' COMPONGO IL PERCORSO DEL FILE DOT

    '        ' COMPONGO IL PERCORSO DEL FILE DOT
    '        If Tributo = Utility.Costanti.TRIBUTO_ICI Then
    '            sFilenameDOT = "AccertamentoICI_" & ConstSession.IdEnte & ".doc"
    '            sFilenameDOC = "AccertamentoICI"
    '        Else
    '            sFilenameDOT = "AccertamentoTASI_" & ConstSession.IdEnte & ".doc"
    '            sFilenameDOC = "AccertamentoTASI"
    '        End If
    '        strTIPODOCUMENTO = CostantiProvv.COD_TIPO_DOC_ACCERTAMENTO

    '        objTestataDOT.Atto = "Template"
    '        objTestataDOT.Dominio = "Provvedimenti"
    '        objTestataDOT.Ente = ConstSession.DescrizioneEnte.Replace("Comune di ", "")
    '        objTestataDOT.Filename = sFilenameDOT

    '        Dim oArrBookmark As ArrayList
    '        Dim iCount As Integer

    '        Dim objBookmark As oggettiStampa
    '        Dim oArrListOggettiDaStampare As New ArrayList
    '        Dim objToPrint As oggettoDaStampareCompleto
    '        Dim ArrayBookMark As oggettiStampa()
    '        Dim iCodContrib As Integer

    '        'Dim culture As IFormatProvider
    '        'culture = New CultureInfo("it-IT", True)
    '        'System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")
    '        Dim strRiga As String=""
    '        Dim strImmoTemp As String = String.Empty
    '        Dim strErroriTemp As String = String.Empty
    '        Dim strImmoTempTitolo As String = String.Empty
    '        Dim Anno As String = String.Empty
    '        'Dim IDdichiarazione As Long
    '        'Dim IDImmobile As Long

    '        Dim dsDatiProvv As New DataSet
    '        Dim dsImmobiliDichiarato As New DataSet
    '        Dim dsVersamenti As New DataSet
    '        Dim dsImmobiliAccertati As New DataSet
    '        Dim objDSTipiInteressi As New DataSet
    '        Dim objDSTipiInteressiL As New DataSet
    '        Dim objDSElencoSanzioni As New DataSet
    '        Dim objDSElencoSanzioniF2 As New DataSet
    '        Dim objDSElencoSanzioniF2Intr As New DataSet
    '        Dim objDSElencoSanzioniRiducibili As New DataSet
    '        Dim objDSElencoSanzioniF2Riducibili As New DataSet
    '        Dim objDSImportiInteressi As New DataSet
    '        Dim objDSImportiInteressiF2 As New DataSet
    '        '---------------------------------------
    '        'var per popolare i bookmark relativi 
    '        'alla sezione degli importi interessi
    '        Dim strImportoGiorni As String
    '        Dim strImportoSemestriACC As String
    '        Dim strImportoSemestriSAL As String
    '        Dim strNumSemestriACC As String
    '        Dim strNumSemestriSAL As String
    '        Dim iRetValImpInt As Boolean
    '        '---------------------------------------
    '        Dim acconto, saldo As Double
    '        Dim ImpDov, ImpVers As Double
    '        Dim tipo_versamento As String = ""
    '        Dim impDiffImposta As Double = 0
    '        Dim impSanzioniNonRiducibili As Double = 0
    '        Dim impSanzioniRiducibili As Double = 0
    '        Dim impSanzioniRidotte As Double = 0
    '        Dim impInteressi As Double = 0
    '        Dim impSpeseNotifica As Double = 0
    '        Dim impArrotondamento As Double = 0
    '        Dim impTotale As Double = 0
    '        Dim impArrotondamentoRidotto As Double = 0
    '        Dim impTotaleRidotto As Double = 0
    '        Dim imp_dov_dich_acc, imp_dov_dich_saldo As Decimal
    '        Dim dvVers As DataView
    '        Dim i As Integer
    '        Dim dTot, dTotUS, dTotNoAS As Double

    '        Dim NomeDbIci As String = ConfigurationManager.AppSettings("NOME_DATABASE_ICI").ToString()
    '        Dim NomeDbOpenGov As String = ConfigurationManager.AppSettings("NOME_DATABASE_OPENGOV").ToString()

    '        Dim objHashTable As Hashtable = New Hashtable
    '        objHashTable.Add("CodENTE", ConstSession.IdEnte)
    '        objHashTable.Add("CODENTE", ConstSession.IdEnte)
    '        objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
    '        'objHashTable.Add("NomeDbIci", NomeDbIci)
    '        objHashTable.Add("NomeDbOpenGov", NomeDbOpenGov)

    '        Dim objCOM As IElaborazioneLiquidazioni = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneLiquidazioni), ConstSession.URLServiziLiquidazioni)
    '        Dim objCOMTipoVoci As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)
    '        Dim objCOMACCERT As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
    '        Dim objICI As DichiarazioniICI.Database.TpSituazioneFinaleIci
    '        objICI = New DichiarazioniICI.Database.TpSituazioneFinaleIci

    '        Dim objVers As DichiarazioniICI.Database.VersamentiTable
    '        objVers = New DichiarazioniICI.Database.VersamentiTable(ConstSession.UserName)

    '        'Dim dsDettaglioAnagrafica As DataSet = Nothing

    '        For iCount = 0 To objDSstampa.Tables(0).Rows.Count - 1
    '            iCodContrib = objDSstampa.Tables(0).Rows(iCount)("COD_CONTRIBUENTE")
    '            IDPROVVEDIMENTO = objDSstampa.Tables(0).Rows(iCount)("ID_PROVVEDIMENTO")
    '            objHashTable.Add("COD_CONTRIBUENTE", iCodContrib)

    '            dsDatiProvv = objCOM.GetProvvedimentoPerStampaLiquidazione(objHashTable, IDPROVVEDIMENTO)
    '            For Each myRow As DataRow In dsDatiProvv.Tables(0).Rows
    '                IDPROCEDIMENTO = myRow("ID_PROCEDIMENTO")
    '                sAnno = myRow("ANNO")
    '                impDiffImposta = myRow("IMPORTO_DIFFERENZA_IMPOSTA")
    '                impSanzioniNonRiducibili = myRow("IMPORTO_SANZIONI_NON_RIDUCIBILI")
    '                impSanzioniRiducibili = (myRow("IMPORTO_SANZIONI") - myRow("IMPORTO_SANZIONI_NON_RIDUCIBILI")).ToString
    '                impSanzioniRidotte = myRow("IMPORTO_SANZIONI_RIDOTTO") '(strImportoSanzioneRidotto / 4)
    '                impSpeseNotifica = myRow("IMPORTO_SPESE")
    '                impArrotondamento = myRow("IMPORTO_ARROTONDAMENTO")
    '                impInteressi = myRow("IMPORTO_INTERESSI")
    '                impTotale = FormatImport(myRow("IMPORTO_TOTALE"))
    '                impTotaleRidotto = myRow("IMPORTO_TOTALE_RIDOTTO")
    '                impArrotondamentoRidotto = myRow("IMPORTO_ARROTONDAMENTO_RIDOTTO")

    '                objHashTable.Add("ANNO", sAnno)
    '                '*** 20140701 - IMU/TARES ***
    '                If sAnno >= 2012 Then
    '                    sFilenameDOT = sFilenameDOT.Replace("ICI", "IMU")
    '                    sFilenameDOC = sFilenameDOC.Replace("ICI", "IMU")
    '                    objTestataDOT.Filename = sFilenameDOT
    '                    Log.Debug(sFilenameDOT & "::" & sFilenameDOC)
    '                End If
    '                '*** ***

    '                oArrBookmark = New ArrayList

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "bozza"
    '                If bIsBozza = True Then
    '                    objBookmark.Valore = "BOZZA"
    '                Else
    '                    objBookmark.Valore = ""
    '                End If
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "nome_ente"
    '                objBookmark.Valore = ConstSession.DescrizioneEnte.ToUpper
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "TipoProvvedimento"
    '                objBookmark.Valore = FormatStringToEmpty(myRow("TIPO_PROVVEDIMENTO")).ToUpper()
    '                oArrBookmark.Add(objBookmark)

    '                '************************************************************************************
    '                'DATI ANAGRAFICI
    '                '************************************************************************************
    '                oArrBookmark = FillBookMarkAnagrafica(oArrBookmark, iCodContrib, Utility.Costanti.TRIBUTO_ICI, FileNameContrib)
    '                If oArrBookmark Is Nothing Then
    '                    Throw New Exception("errore in popolamento anagrafica")
    '                End If
    '                '---------------------------------------------------------------------------------
    '                For x As Integer = 0 To 9
    '                    objBookmark = New oggettiStampa
    '                    objBookmark.Descrizione = Replace("anno_ici" & x.ToString(), "0", "")
    '                    objBookmark.Valore = sAnno
    '                    oArrBookmark.Add(objBookmark)
    '                Next
    '                For x As Integer = 0 To 9
    '                    objBookmark = New oggettiStampa
    '                    objBookmark.Descrizione = Replace("anno_imposta" & x.ToString(), "0", "")
    '                    objBookmark.Valore = sAnno
    '                    oArrBookmark.Add(objBookmark)
    '                Next
    '                For x As Integer = 0 To 9
    '                    objBookmark = New oggettiStampa
    '                    objBookmark.Descrizione = Replace("n_provvedimento" & x.ToString(), "0", "")
    '                    objBookmark.Valore = objUtility.CToStr(myRow("NUMERO_ATTO"))
    '                    oArrBookmark.Add(objBookmark)
    '                Next
    '                For x As Integer = 0 To 9
    '                    objBookmark = New oggettiStampa
    '                    objBookmark.Descrizione = Replace("data_provvedimento" & x.ToString(), "0", "")
    '                    If myRow("DATA_CONFERMA") Is System.DBNull.Value Then
    '                        objBookmark.Valore = ""
    '                    Else
    '                        objBookmark.Valore = objUtility.GiraDataFromDB(myRow("DATA_CONFERMA"))
    '                    End If
    '                    oArrBookmark.Add(objBookmark)
    '                Next

    '                '************************************************************************************
    '                'ELENCO IMMOBILI DICHIARATI
    '                '************************************************************************************
    '                dsImmobiliDichiarato = objCOMACCERT.getImmobiliDichiaratiPerStampaAccertamenti(objHashTable, IDPROCEDIMENTO)
    '                '*** 20130620 - richieste comune ***
    '                strRiga = FillBookMarkACCERTATO(dsImmobiliDichiarato, sAnno, Tributo)
    '                'strRiga = FillBookMarkDICHIARATO(dsImmobiliDichiarato, strANNO)
    '                '*** ***
    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "elenco_immobili"
    '                objBookmark.Valore = strRiga
    '                oArrBookmark.Add(objBookmark)

    '                '************************************************************************************
    '                'ELENCO IMMOBILI ACCERATI
    '                '************************************************************************************
    '                dsImmobiliAccertati = objCOMACCERT.getImmobiliAccertatiPerStampaAccertamenti(objHashTable, IDPROCEDIMENTO)
    '                strRiga = FillBookMarkACCERTATO(dsImmobiliAccertati, sAnno, Tributo)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "elenco_immobili_acce"
    '                objBookmark.Valore = strRiga
    '                oArrBookmark.Add(objBookmark)

    '                '************************************************************************************
    '                'ELENCO VERSAMENTI
    '                '************************************************************************************
    '                dsVersamenti = objCOM.getVersamentiPerStampaLiquidazione(objHashTable, IDPROCEDIMENTO, oggettoatto.fase.versamentitardivi)
    '                strRiga = FillBookMarkVersamenti(dsVersamenti)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "elenco_versamenti"
    '                objBookmark.Valore = "Dettaglio Versamenti" & vbCrLf & strRiga
    '                oArrBookmark.Add(objBookmark)

    '                strRiga = FillBookMarkIMPORTODOVACC(dsImmobiliAccertati, acconto, saldo)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "imp_dov_acc"
    '                objBookmark.Valore = FormatImport(acconto) & " €"
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "imp_dov_saldo"
    '                objBookmark.Valore = FormatImport(saldo) & " €"
    '                oArrBookmark.Add(objBookmark)

    '                '************************************************************************************
    '                'ELENCO INTERESSI CONFIGURATI
    '                '************************************************************************************
    '                objHashTable.Add("CODTRIBUTO", Utility.Costanti.TRIBUTO_ICI)
    '                objDSTipiInteressiL = objCOM.GetElencoInteressiPerStampaLiquidazione(objHashTable, IDPROVVEDIMENTO)
    '                objDSTipiInteressi = objCOMACCERT.GetElencoInteressiPerStampaAccertamenti(objHashTable, IDPROVVEDIMENTO)
    '                strRiga = FillBookMarkELENCOINTERESSI(objDSTipiInteressiL, objDSTipiInteressi, Utility.Costanti.TRIBUTO_ICI)
    '                strRiga += FillBookMarkTOTALEINTERESSI(objDSTipiInteressiL, objDSTipiInteressi)
    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "elenco_interessi"
    '                objBookmark.Valore = strRiga
    '                oArrBookmark.Add(objBookmark)

    '                '************************************************************************************
    '                'ELENCO SANZIONI APPLICATE CON IMPORTO 
    '                '************************************************************************************
    '                objHashTable.Add("riducibile", 0)
    '                'Sanzioni NON Riducibili
    '                objDSElencoSanzioni = objCOMACCERT.GetElencoSanzioniPerStampaAccertamenti(objHashTable, IDPROVVEDIMENTO)
    '                objDSElencoSanzioniF2 = objCOM.GetElencoSanzioniPerStampaLiquidazione(objHashTable, IDPROVVEDIMENTO)
    '                '*************************
    '                'Sanzioni Riducibili
    '                objHashTable.Remove("riducibile")
    '                objHashTable.Add("riducibile", 1)
    '                objDSElencoSanzioniRiducibili = objCOMACCERT.GetElencoSanzioniPerStampaAccertamenti(objHashTable, IDPROVVEDIMENTO)
    '                objDSElencoSanzioniF2Riducibili = objCOM.GetElencoSanzioniPerStampaLiquidazione(objHashTable, IDPROVVEDIMENTO)
    '                '*************************
    '                'Sanzioni Intrasmissibilità agli eredi 
    '                objHashTable.Remove("riducibile")
    '                objHashTable.Add("riducibile", "")
    '                objDSElencoSanzioniF2Intr = objCOM.GetElencoSanzioniPerStampaLiquidazione(objHashTable, IDPROVVEDIMENTO)
    '                '*************************

    '                strRiga = FillBookMarkELENCOSANZIONI(objDSElencoSanzioni, objDSElencoSanzioniF2, objDSElencoSanzioniRiducibili, objDSElencoSanzioniF2Riducibili, objDSElencoSanzioniF2Intr)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "elenco_sanzioni"
    '                objBookmark.Valore = strRiga
    '                oArrBookmark.Add(objBookmark)

    '                '************************************************************************************
    '                'IMPORTI (DIFF IMPOSTA - SANZIONI - INTERESSI - SPESE - ecc...
    '                '************************************************************************************
    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "imposta_dovuta"
    '                'objBookmark.Valore = FormatNumberToZero(myrow("TOTALE_DICHIARATO")).Replace(",", ".") & " €"
    '                objBookmark.Valore = FormatImport(myRow("TOTALE_DICHIARATO")) & " €"
    '                oArrBookmark.Add(objBookmark)
    '                '*** 20130121 - devo stampare il dovuto accertato altrimenti non tornano i conti nel prospetto ***
    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "imposta_dovuta_accer"
    '                objBookmark.Valore = FormatImport(myRow("IMPORTO_ACCERTATO_ACC")) & " €"
    '                oArrBookmark.Add(objBookmark)
    '                '*** ***

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "imposta_versata"
    '                'objBookmark.Valore = FormatNumberToZero(myrow("TOTALE_VERSATO")).Replace(",", ".") & " €"
    '                objBookmark.Valore = FormatImport(myRow("TOTALE_VERSATO")) & " €"
    '                oArrBookmark.Add(objBookmark)

    '                ImpDov = myRow("TOTALE_DICHIARATO")
    '                ImpVers = myRow("TOTALE_VERSATO")

    '                If ImpDov > 0 And (ImpVers > 0 And ImpVers < ImpDov) Then
    '                    tipo_versamento = "parziale"
    '                ElseIf ImpDov > 0 And ImpVers = 0 Then
    '                    tipo_versamento = "omesso"
    '                End If
    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "tipo_versamento"
    '                objBookmark.Valore = tipo_versamento
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "ImpostaAccertata"
    '                'objBookmark.Valore = FormatNumberToZero(myrow("TOTALE_VERSATO")).Replace(",", ".") & " €"
    '                objBookmark.Valore = FormatImport(myRow("TOTALE_ACCERTATO")) & " €"
    '                oArrBookmark.Add(objBookmark)

    '                'dipe
    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "ImpostaAccertata_60g"
    '                objBookmark.Valore = FormatImport(myRow("TOTALE_ACCERTATO")) & " €"
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "DiffImpostaDaVersare"
    '                'objBookmark.Valore = FormatNumberToZero(myrow("IMPORTO_DIFFERENZA_IMPOSTA")).Replace(",", ".") & " €"
    '                objBookmark.Valore = FormatImport(impDiffImposta) & " €"
    '                oArrBookmark.Add(objBookmark)

    '                'dipe
    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "DiffImpostaDaVer_60g"
    '                objBookmark.Valore = FormatImport(impDiffImposta) & " €"
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "DiffImpDaVer_60g_1"
    '                objBookmark.Valore = FormatImport(impDiffImposta) & " €"
    '                oArrBookmark.Add(objBookmark)

    '                '********************************************************************************
    '                '************ GESTIONE IMPORTI SANZIONI *****************************************
    '                '********************************************************************************
    '                objBookmark = New oggettiStampa
    '                'strImportoSanzioneRidotto = FillBookMarkSanzioniRiducibili(objDSElencoSanzioniRiducibili)
    '                objBookmark.Descrizione = "ImportoSanzioneRid"
    '                objBookmark.Valore = FormatImport(impSanzioniRiducibili) & " €"
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "ImportoSanzione"
    '                objBookmark.Valore = FormatImport(impSanzioniNonRiducibili) & " €"
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "ImportoSanzione_60g"
    '                objBookmark.Valore = FormatImport(impSanzioniNonRiducibili) & " €"
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "ImpSanzioneRid_60g"
    '                objBookmark.Valore = FormatImport(impSanzioniRidotte) & " €"
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "spese_notifica"
    '                objBookmark.Valore = FormatImport(impSpeseNotifica) & " €"
    '                oArrBookmark.Add(objBookmark)

    '                'dipe
    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "spese_notifica_60g"
    '                objBookmark.Valore = FormatImport(impSpeseNotifica) & " €"
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "Importo_arrotond"
    '                'objBookmark.Valore = FormatNumberToZero(myrow("IMPORTO_ARROTONDAMENTO")).Replace(",", ".") & " €"
    '                objBookmark.Valore = FormatImport(impArrotondamento) & " €"
    '                oArrBookmark.Add(objBookmark)

    '                '************************************************************************************
    '                'IMPORTI INTERESSI
    '                '************************************************************************************
    '                objDSImportiInteressi = objCOMACCERT.GetInteressiTotaliPerStampaAccertamenti(objHashTable, IDPROVVEDIMENTO)
    '                objDSImportiInteressiF2 = objCOM.GetInteressiTotaliPerStampaLiquidazione(objHashTable, IDPROVVEDIMENTO)
    '                iRetValImpInt = FillBookMarkIMPORTIINTERESSI(objDSImportiInteressi, objDSImportiInteressiF2, strImportoGiorni, strImportoSemestriACC, strImportoSemestriSAL, strNumSemestriACC, strNumSemestriSAL)
    '                If iRetValImpInt = True Then
    '                    objBookmark = New oggettiStampa
    '                    objBookmark.Descrizione = "imp_interessi_GIORNI"
    '                    'objBookmark.Valore = strImportoGiorni.Replace(",", ".") & " €"
    '                    objBookmark.Valore = strImportoGiorni & " €"
    '                    oArrBookmark.Add(objBookmark)

    '                    objBookmark = New oggettiStampa
    '                    objBookmark.Descrizione = "imp_semestri_ACCONTO"
    '                    'objBookmark.Valore = strImportoSemestriACC.Replace(",", ".") & " €"
    '                    objBookmark.Valore = strImportoSemestriACC & " €"
    '                    oArrBookmark.Add(objBookmark)

    '                    objBookmark = New oggettiStampa
    '                    objBookmark.Descrizione = "num_semestri_ACCONTO"
    '                    objBookmark.Valore = strNumSemestriACC
    '                    oArrBookmark.Add(objBookmark)

    '                    objBookmark = New oggettiStampa
    '                    objBookmark.Descrizione = "imp_semestri_SALDO"
    '                    'objBookmark.Valore = strImportoSemestriSAL.Replace(",", ".") & " €"
    '                    objBookmark.Valore = strImportoSemestriSAL & " €"
    '                    oArrBookmark.Add(objBookmark)

    '                    objBookmark = New oggettiStampa
    '                    objBookmark.Descrizione = "num_semestri_SALDO"
    '                    objBookmark.Valore = strNumSemestriSAL
    '                    oArrBookmark.Add(objBookmark)

    '                    objBookmark = New oggettiStampa
    '                    objBookmark.Descrizione = "int_mor"
    '                    'int_mor = CDbl(strImportoGiorni) + CDbl(strImportoSemestriACC) + CDbl(strImportoSemestriSAL)
    '                    objBookmark.Valore = FormatImport(impInteressi) & " €"
    '                    oArrBookmark.Add(objBookmark)

    '                    objBookmark = New oggettiStampa
    '                    objBookmark.Descrizione = "int_mor_60g"
    '                    'objBookmark.Valore = FormatImport(CDbl(strImportoGiorni) + CDbl(strImportoSemestriACC) + CDbl(strImportoSemestriSAL)) & " €"
    '                    objBookmark.Valore = FormatImport(impInteressi) & " €"
    '                    oArrBookmark.Add(objBookmark)
    '                End If
    '                '************************************************************************************
    '                'FINE IMPORTI INTERESSI
    '                '************************************************************************************

    '                '************************************************************************************
    '                'TOTALI
    '                '************************************************************************************
    '                'strImportoTotale = CDbl(strImportoDiffImp) + CDbl(int_mor) + CDbl(ImpSanzioni) + CDbl(strImportoSanzioneRidotto) + CDbl(spese_notifica) + CDbl(Importo_arrotond)
    '                'Importo_totale_60g = CDbl(strImportoDiffImp) + CDbl(int_mor) + CDbl(ImportoSanzione_60g) + CDbl(strImportoSanzioneRidotto) + CDbl(spese_notifica) + CDbl(Importo_arrotond)
    '                'Importo_totale_60g = CDbl(strImportoDiffImp) + CDbl(int_mor) + CDbl(ImportoSanzione_60g) + CDbl(strImportoSanzioneRidotto_60g) + CDbl(spese_notifica)
    '                'dblIMPORTOARROTONDATO = ImportoArrotondato(CDbl(Importo_totale_60g))
    '                'dblIMPORTOARROTONDAMENTO = dblIMPORTOARROTONDATO - CDbl(Importo_totale_60g)
    '                'Importo_totale_60g = dblIMPORTOARROTONDATO

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "Importo_totale"
    '                'objBookmark.Valore = FormatImport(CDbl(strImportoTotale) - CDbl(strImportoSanzione) + CDbl(strImportoSanzioneRidotto)) & " €"
    '                objBookmark.Valore = FormatImport(impTotale) & " €"
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "ImpTotNonRidotto"
    '                'objBookmark.Valore = FormatImport(CDbl(strImportoTotale) - CDbl(strImportoSanzione) + CDbl(strImportoSanzioneRidotto)) & " €"
    '                objBookmark.Valore = FormatImport(impTotale) & " €"
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "Importo_totale_60g"
    '                objBookmark.Valore = FormatImport(impTotaleRidotto) & " €"
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "Importo_totale_1"
    '                'objBookmark.Valore = FormatImport(CDbl(strImportoTotale) - CDbl(strImportoSanzione_60g) + CDbl(strImportoSanzioneRidotto_60g)) & " €"
    '                objBookmark.Valore = FormatImport(impTotaleRidotto) & " €"
    '                oArrBookmark.Add(objBookmark)

    '                'dipe
    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "Importo_arrotond_60g"
    '                objBookmark.Valore = FormatImport(impArrotondamentoRidotto) & " €"
    '                oArrBookmark.Add(objBookmark)

    '                '************************************************************************************
    '                'IMPORTI DA DICHIARATO
    '                '************************************************************************************
    '                'Dim dtICI As DataTable
    '                'dtICI = objICI.GetImportoDovuto(iCodContrib, strANNO, constsession.idente)
    '                'dsImmobiliDichiarato = objCOMACCERT.getImmobiliDichiaratiPerStampaAccertamenti(objHashTable, IDPROCEDIMENTO)
    '                FillBookMarkIMP_DOV_DICH(dsImmobiliDichiarato, imp_dov_dich_acc, imp_dov_dich_saldo)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "imp_dov_dich_acc"
    '                'objBookmark.Valore = FormatImport(dtICI.Rows(0)("ACCONTO")) & " €"
    '                objBookmark.Valore = FormatImport(imp_dov_dich_acc) & " €"
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "imp_dov_dich_saldo"
    '                'objBookmark.Valore = FormatImport(dtICI.Rows(0)("SALDO")) & " €"
    '                objBookmark.Valore = FormatImport(imp_dov_dich_saldo) & " €"
    '                oArrBookmark.Add(objBookmark)

    '                '************************************************************************************
    '                'FINE IMPORTI DA DICHIARATO
    '                '************************************************************************************

    '                '************************************************************************************
    '                'IMPORTI VERSATI
    '                '************************************************************************************

    '                'Senza Flag Acconto e saldo selezionati
    '                dvVers = objVers.GetVersamentiPerTipologia(ConstSession.IdEnte, sAnno, iCodContrib, False, False, False, Tributo)
    '                dTotNoAS = 0
    '                For i = 0 To dvVers.Table.Rows.Count - 1
    '                    dTotNoAS += dvVers.Table.Rows(i)("ImportoPagato")
    '                Next

    '                'Unica soluzione
    '                dvVers = objVers.GetVersamentiPerTipologia(ConstSession.IdEnte, sAnno, iCodContrib, True, True, False, Tributo)
    '                dTotUS = 0
    '                For i = 0 To dvVers.Table.Rows.Count - 1
    '                    dTotUS += dvVers.Table.Rows(i)("ImportoPagato")
    '                Next

    '                'Acconto
    '                dvVers = objVers.GetVersamentiPerTipologia(ConstSession.IdEnte, sAnno, iCodContrib, True, False, False, Tributo)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "imp_vers_acc"
    '                dTot = dTotUS + dTotNoAS
    '                For i = 0 To dvVers.Table.Rows.Count - 1
    '                    dTot += dvVers.Table.Rows(i)("ImportoPagato")
    '                Next
    '                objBookmark.Valore = FormatImport(dTot) & " €"
    '                oArrBookmark.Add(objBookmark)

    '                'saldo
    '                dvVers = objVers.GetVersamentiPerTipologia(ConstSession.IdEnte, sAnno, iCodContrib, False, True, False, Tributo)
    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "imp_vers_saldo"
    '                dTot = 0
    '                For i = 0 To dvVers.Table.Rows.Count - 1
    '                    dTot += dvVers.Table.Rows(i)("ImportoPagato")
    '                Next
    '                objBookmark.Valore = FormatImport(dTot) & " €"
    '                oArrBookmark.Add(objBookmark)

    '                '************************************************************************************
    '                'FINE IMPORTI VERSATI
    '                '************************************************************************************
    '                objTestataDOC = New oggettoTestata
    '                objTestataDOC.Atto = "Documenti"
    '                objTestataDOC.Dominio = "Provvedimenti"
    '                objTestataDOC.Ente = ConstSession.DescrizioneEnte.Replace("Comune di ", "")
    '                objTestataDOC.Filename = FileNameContrib & "_" & myRow("NUMERO_ATTO").ToString.Replace("/", "") & "_" & sFilenameDOC

    '                ArrayBookMark = CType(oArrBookmark.ToArray(GetType(oggettiStampa)), oggettiStampa())

    '                objToPrint = New oggettoDaStampareCompleto
    '                objToPrint.TestataDOC = objTestataDOC
    '                objToPrint.TestataDOT = objTestataDOT
    '                objToPrint.Stampa = ArrayBookMark

    '                'oArrListOggettiDaStampare = New ArrayList
    '                oArrListOggettiDaStampare.Add(objToPrint)
    '            Next
    '        Next

    '        Dim GruppoDOC As New GruppoDocumenti
    '        Dim GruppoDOCUMENTI As GruppoDocumenti()
    '        Dim ArrListGruppoDOC As New ArrayList

    '        Dim ArrOggCompleto As oggettoDaStampareCompleto()
    '        Dim objTestataGruppo As New oggettoTestata

    '        ArrOggCompleto = CType(oArrListOggettiDaStampare.ToArray(GetType(oggettoDaStampareCompleto)), oggettoDaStampareCompleto())

    '        GruppoDOC.OggettiDaStampare = ArrOggCompleto
    '        GruppoDOC.TestataGruppo = objTestataGruppo
    '        ArrListGruppoDOC.Add(GruppoDOC)

    '        GruppoDOCUMENTI = CType(ArrListGruppoDOC.ToArray(GetType(GruppoDocumenti)), GruppoDocumenti())
    '        PrintDoc(GruppoDOCUMENTI)
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.STAMPA_ACCERTAMENTOICI.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '        Dim sScript As String = ""
    '        sScript += "GestAlert('a', 'danger', '', '', 'Errore in fase di stampa');"
    '        RegisterScript(sScript, Me.GetType())
    '    End Try
    'End Sub
    'Private Sub STAMPA_RIMBORSOICI(ByVal strTipoDoc As String, ByVal bIsBozza As Boolean, Tributo As String)
    '    Dim sFilenameDOT, sFilenameDOC As String
    '    Dim objDSstampa As DataSet
    '    Dim objUtility As New MyUtility
    '    'contiene i dati anagrafici visualizzati nella grilgia della ricerca lettere
    '    objDSstampa = CType(Session("PROVVEDIMENTI_DA_STAMPARE"), DataSet)

    '    ' creo l'oggetto testata per l'oggetto da stampare
    '    'serve per indicare la posizione di salvataggio e il nome del file.
    '    Dim objTestataDOC As oggettoTestata
    '    Dim objTestataDOT As New oggettoTestata
    '    Dim FileNameContrib As String
    '    Dim IDPROVVEDIMENTO As String
    '    Dim IDPROCEDIMENTO As String
    '    Dim strTIPODOCUMENTO As String
    '    Dim strANNO As String
    '    Dim oArrBookmark As ArrayList
    '    Dim iCount As Integer

    '    Dim objBookmark As oggettiStampa
    '    Dim oArrListOggettiDaStampare As New ArrayList
    '    Dim objToPrint As oggettoDaStampareCompleto
    '    Dim ArrayBookMark As oggettiStampa()
    '    Dim iCodContrib As Integer

    '    Dim strRiga As String=""
    '    Dim strImmoTemp As String = String.Empty
    '    Dim strErroriTemp As String = String.Empty
    '    Dim strImmoTempTitolo As String = String.Empty
    '    Dim Anno As String = String.Empty

    '    Dim dsDatiProvv As New DataSet
    '    Dim dsImmobiliDichiarato As New DataSet
    '    Dim dsVersamenti As New DataSet
    '    Dim dsImmobiliAccertati As New DataSet
    '    Dim objDSTipiInteressi As New DataSet
    '    Dim objDSTipiInteressiL As New DataSet
    '    Dim objDSElencoSanzioni As New DataSet
    '    Dim objDSElencoSanzioniF2 As New DataSet
    '    Dim objDSElencoSanzioniF2Intr As New DataSet
    '    Dim objDSElencoSanzioniRiducibili As New DataSet
    '    Dim objDSElencoSanzioniF2Riducibili As New DataSet
    '    Dim objDSImportiInteressi As New DataSet
    '    Dim objDSImportiInteressiF2 As New DataSet
    '    '---------------------------------------
    '    'var per popolare i bookmark relativi 
    '    'alla sezione degli importi interessi
    '    Dim strImportoGiorni As String
    '    Dim strImportoSemestriACC As String
    '    Dim strImportoSemestriSAL As String
    '    Dim strNumSemestriACC As String
    '    Dim strNumSemestriSAL As String
    '    Dim iRetValImpInt As Boolean
    '    '---------------------------------------

    '    Dim NomeDbIci As String = ConfigurationManager.AppSettings("NOME_DATABASE_ICI").ToString()
    '    Dim NomeDbOpenGov As String = ConfigurationManager.AppSettings("NOME_DATABASE_OPENGOV").ToString()

    '    Dim objHashTable As Hashtable = New Hashtable
    '    Dim objCOM As IElaborazioneLiquidazioni = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneLiquidazioni), ConstSession.URLServiziLiquidazioni)
    '    Dim objCOMTipoVoci As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)
    '    Dim objCOMACCERT As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
    '    Dim objICI As DichiarazioniICI.Database.TpSituazioneFinaleIci
    '    Dim objVers As DichiarazioniICI.Database.VersamentiTable

    '    Try
    '        ' COMPONGO IL PERCORSO DEL FILE DOT
    '        If Tributo = Utility.Costanti.TRIBUTO_ICI Then
    '            sFilenameDOT = "Rimborso_ICI_" & ConstSession.IdEnte & ".doc"
    '            sFilenameDOC = "Rimborso_ICI"
    '        Else
    '            sFilenameDOT = "Rimborso_TASI_" & ConstSession.IdEnte & ".doc"
    '            sFilenameDOC = "Rimborso_TASI"
    '        End If
    '        strTIPODOCUMENTO = CostantiProvv.COD_TIPO_DOC_RIMBORSO_ICI

    '        objTestataDOT.Atto = "Template"
    '        objTestataDOT.Dominio = "Provvedimenti"
    '        objTestataDOT.Ente = ConstSession.DescrizioneEnte.Replace("Comune di ", "")
    '        objTestataDOT.Filename = sFilenameDOT


    '        objHashTable.Add("CodENTE", ConstSession.IdEnte)
    '        objHashTable.Add("CODENTE", ConstSession.IdEnte)
    '        objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
    '        'objHashTable.Add("NomeDbIci", NomeDbIci)
    '        objHashTable.Add("NomeDbOpenGov", NomeDbOpenGov)
    '        objHashTable.Add("CODTRIBUTO", Session("COD_TRIBUTO"))

    '        objICI = New DichiarazioniICI.Database.TpSituazioneFinaleIci
    '        objVers = New DichiarazioniICI.Database.VersamentiTable(ConstSession.UserName)

    '        For iCount = 0 To objDSstampa.Tables(0).Rows.Count - 1
    '            iCodContrib = objDSstampa.Tables(0).Rows(iCount)("COD_CONTRIBUENTE")
    '            IDPROVVEDIMENTO = objDSstampa.Tables(0).Rows(iCount)("ID_PROVVEDIMENTO")
    '            objHashTable.Add("COD_CONTRIBUENTE", iCodContrib)
    '            dsDatiProvv = objCOM.GetProvvedimentoPerStampaLiquidazione(objHashTable, IDPROVVEDIMENTO)

    '            IDPROCEDIMENTO = dsDatiProvv.Tables(0).Rows(iCount)("ID_PROCEDIMENTO")
    '            strANNO = dsDatiProvv.Tables(0).Rows(iCount)("ANNO")
    '            objHashTable.Add("ANNO", strANNO)

    '            oArrBookmark = New ArrayList

    '            '*** 20140701 - IMU/TARES ***
    '            If strANNO >= 2012 Then
    '                sFilenameDOT = sFilenameDOT.Replace("ICI", "IMU")
    '                sFilenameDOC = sFilenameDOC.Replace("ICI", "IMU")
    '                objTestataDOT.Filename = sFilenameDOT
    '            End If
    '            '*** ***
    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "bozza"
    '            If bIsBozza = True Then
    '                objBookmark.Valore = "BOZZA"
    '            Else
    '                objBookmark.Valore = ""
    '            End If
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "nome_ente"
    '            objBookmark.Valore = CStr(Session("DESCRIZIONE_ENTE")).ToUpper
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "TipoProvvedimento"
    '            objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("TIPO_PROVVEDIMENTO")).ToUpper()
    '            oArrBookmark.Add(objBookmark)

    '            '************************************************************************************
    '            'DATI ANAGRAFICI
    '            '************************************************************************************
    '            oArrBookmark = FillBookMarkAnagrafica(oArrBookmark, iCodContrib, Utility.Costanti.TRIBUTO_ICI, FileNameContrib)
    '            If oArrBookmark Is Nothing Then
    '                Throw New Exception("errore in popolamento anagrafica")
    '            End If
    '            '---------------------------------------------------------------------------------

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "anno_ici"
    '            objBookmark.Valore = strANNO
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "anno_ici_1"
    '            objBookmark.Valore = strANNO
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "n_provvedimento"
    '            objBookmark.Valore = objUtility.CToStr(dsDatiProvv.Tables(0).Rows(iCount)("NUMERO_ATTO"))

    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "data_provvedimento"
    '            If dsDatiProvv.Tables(0).Rows(iCount)("DATA_ELABORAZIONE") Is System.DBNull.Value Then
    '                objBookmark.Valore = ""
    '            Else
    '                objBookmark.Valore = objUtility.GiraDataFromDB(dsDatiProvv.Tables(0).Rows(iCount)("DATA_ELABORAZIONE"))
    '            End If
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "anno_imposta"
    '            objBookmark.Valore = strANNO
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "anno_imposta1"
    '            objBookmark.Valore = strANNO
    '            oArrBookmark.Add(objBookmark)

    '            '************************************************************************************
    '            'ELENCO IMMOBILI DICHIARATI
    '            '************************************************************************************
    '            dsImmobiliDichiarato = objCOMACCERT.getImmobiliDichiaratiPerStampaAccertamenti(objHashTable, IDPROCEDIMENTO)
    '            'strRiga = FillBookMarkDICHIARATO(dsImmobiliDichiarato, strANNO)
    '            strRiga = FillBookMarkACCERTATO(dsImmobiliDichiarato, strANNO, Tributo)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "elenco_immobili"
    '            objBookmark.Valore = strRiga
    '            oArrBookmark.Add(objBookmark)

    '            '************************************************************************************
    '            'ELENCO IMMOBILI ACCERATI
    '            '************************************************************************************
    '            dsImmobiliAccertati = objCOMACCERT.getImmobiliAccertatiPerStampaAccertamenti(objHashTable, IDPROCEDIMENTO)
    '            strRiga = FillBookMarkACCERTATO(dsImmobiliAccertati, strANNO, Tributo)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "elenco_immobili_acce"
    '            objBookmark.Valore = strRiga
    '            oArrBookmark.Add(objBookmark)

    '            '************************************************************************************
    '            'ELENCO VERSAMENTI
    '            '************************************************************************************
    '            dsVersamenti = objCOM.getVersamentiPerStampaLiquidazione(objHashTable, IDPROCEDIMENTO, oggettoatto.fase.versamentitardivi)
    '            strRiga = FillBookMarkVersamenti(dsVersamenti)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "elenco_versamenti"
    '            objBookmark.Valore = "Dettaglio Versamenti" & vbCrLf & strRiga
    '            oArrBookmark.Add(objBookmark)

    '            Dim acconto, saldo As Double
    '            strRiga = FillBookMarkIMPORTODOVACC(dsImmobiliAccertati, acconto, saldo)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "imp_dov_acc"
    '            objBookmark.Valore = FormatImport(acconto) & " €"
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "imp_dov_saldo"
    '            objBookmark.Valore = FormatImport(saldo) & " €"
    '            oArrBookmark.Add(objBookmark)

    '            '************************************************************************************
    '            'ELENCO SANZIONI APPLICATE CON IMPORTO 
    '            '************************************************************************************
    '            objHashTable.Add("riducibile", 0)
    '            'Sanzioni NON Riducibili
    '            objDSElencoSanzioni = objCOMACCERT.GetElencoSanzioniPerStampaAccertamenti(objHashTable, IDPROVVEDIMENTO)
    '            objDSElencoSanzioniF2 = objCOM.GetElencoSanzioniPerStampaLiquidazione(objHashTable, IDPROVVEDIMENTO)
    '            '*************************
    '            'Sanzioni Riducibili
    '            objHashTable.Remove("riducibile")
    '            objHashTable.Add("riducibile", 1)
    '            objDSElencoSanzioniRiducibili = objCOMACCERT.GetElencoSanzioniPerStampaAccertamenti(objHashTable, IDPROVVEDIMENTO)
    '            objDSElencoSanzioniF2Riducibili = objCOM.GetElencoSanzioniPerStampaLiquidazione(objHashTable, IDPROVVEDIMENTO)
    '            '*************************
    '            'Sanzioni Intrasmissibilità agli eredi 
    '            objHashTable.Remove("riducibile")
    '            objHashTable.Add("riducibile", "")
    '            objDSElencoSanzioniF2Intr = objCOM.GetElencoSanzioniPerStampaLiquidazione(objHashTable, IDPROVVEDIMENTO)
    '            '*************************
    '            strRiga = FillBookMarkELENCOSANZIONI(objDSElencoSanzioni, objDSElencoSanzioniF2, objDSElencoSanzioniRiducibili, objDSElencoSanzioniF2Riducibili, objDSElencoSanzioniF2Intr)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "elenco_sanzioni"
    '            objBookmark.Valore = strRiga
    '            oArrBookmark.Add(objBookmark)

    '            '************************************************************************************
    '            'IMPORTI (DIFF IMPOSTA - SANZIONI - INTERESSI - SPESE - ecc...
    '            '************************************************************************************
    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "imposta_dovuta"
    '            'objBookmark.Valore = FormatNumberToZero(dsDatiProvv.Tables(0).Rows(iCount)("TOTALE_DICHIARATO")).Replace(",", ".") & " €"
    '            objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("TOTALE_DICHIARATO")) & " €"
    '            oArrBookmark.Add(objBookmark)
    '            '*** 20130121 - devo stampare il dovuto accertato altrimenti non tornano i conti nel prospetto ***
    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "imposta_dovuta_accer"
    '            objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_ACCERTATO_ACC")) & " €"
    '            oArrBookmark.Add(objBookmark)
    '            '*** ***

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "imposta_versata"
    '            'objBookmark.Valore = FormatNumberToZero(dsDatiProvv.Tables(0).Rows(iCount)("TOTALE_VERSATO")).Replace(",", ".") & " €"
    '            objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("TOTALE_VERSATO")) & " €"
    '            oArrBookmark.Add(objBookmark)

    '            Dim ImpDov, ImpVers As Double
    '            Dim tipo_versamento As String = ""

    '            ImpDov = dsDatiProvv.Tables(0).Rows(iCount)("TOTALE_DICHIARATO")
    '            ImpVers = dsDatiProvv.Tables(0).Rows(iCount)("TOTALE_VERSATO")

    '            If ImpDov > 0 And (ImpVers > 0 And ImpVers < ImpDov) Then
    '                tipo_versamento = "parziale"
    '            ElseIf ImpDov > 0 And ImpVers = 0 Then
    '                tipo_versamento = "omesso"
    '            End If
    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "tipo_versamento"
    '            objBookmark.Valore = tipo_versamento
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "ImpostaAccertata"
    '            'objBookmark.Valore = FormatNumberToZero(dsDatiProvv.Tables(0).Rows(iCount)("TOTALE_VERSATO")).Replace(",", ".") & " €"
    '            objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("TOTALE_ACCERTATO")) & " €"
    '            oArrBookmark.Add(objBookmark)

    '            'dipe
    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "ImpostaAccertata_60g"
    '            objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("TOTALE_ACCERTATO")) & " €"
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "DiffImpostaDaVersare"
    '            'objBookmark.Valore = FormatNumberToZero(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_DIFFERENZA_IMPOSTA")).Replace(",", ".") & " €"
    '            Dim strImportoDiffImp As String = dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_DIFFERENZA_IMPOSTA")
    '            objBookmark.Valore = FormatImport(strImportoDiffImp) & " €"
    '            oArrBookmark.Add(objBookmark)

    '            'dipe
    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "DiffImpostaDaVer_60g"
    '            strImportoDiffImp = strImportoDiffImp * (-1)
    '            objBookmark.Valore = FormatImport(strImportoDiffImp) & " €"
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "DiffImpDaVer_60g_1"
    '            objBookmark.Valore = FormatImport(strImportoDiffImp) & " €"
    '            oArrBookmark.Add(objBookmark)

    '            '********************************************************************************
    '            '************ GESTIONE IMPORTI SANZIONI *****************************************
    '            '********************************************************************************
    '            Dim strImportoSanzioneRidotto As String
    '            objBookmark = New oggettiStampa
    '            strImportoSanzioneRidotto = FillBookMarkSanzioniRiducibili(objDSElencoSanzioniRiducibili)
    '            objBookmark.Descrizione = "ImportoSanzioneRid"
    '            objBookmark.Valore = FormatImport(strImportoSanzioneRidotto) & " €"
    '            oArrBookmark.Add(objBookmark)

    '            Dim ImpSanzioni As String
    '            ImpSanzioni = CType((CType(FillBookMarkSanzioniRiducibili(objDSElencoSanzioni), Double) + CType(FillBookMarkSanzioniRiducibili(objDSElencoSanzioniF2), Double)), String)
    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "ImportoSanzione"
    '            objBookmark.Valore = FormatImport(ImpSanzioni) & " €"
    '            oArrBookmark.Add(objBookmark)

    '            Dim strImportoSanzioneRidotto_60g As String
    '            strImportoSanzioneRidotto_60g = (strImportoSanzioneRidotto / 4)
    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "ImpSanzioneRid_60g"
    '            objBookmark.Valore = FormatImport(strImportoSanzioneRidotto_60g) & " €"
    '            oArrBookmark.Add(objBookmark)

    '            Dim ImportoSanzione_60g As String
    '            ImportoSanzione_60g = ImpSanzioni
    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "ImportoSanzione_60g"
    '            objBookmark.Valore = FormatImport(ImportoSanzione_60g) & " €"
    '            oArrBookmark.Add(objBookmark)

    '            Dim spese_notifica As String
    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "spese_notifica"
    '            If dsDatiProvv.Tables(0).Rows.Count > 0 Then
    '                spese_notifica = dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SPESE")
    '            Else
    '                spese_notifica = 0
    '            End If

    '            objBookmark.Valore = FormatImport(spese_notifica) & " €"
    '            oArrBookmark.Add(objBookmark)

    '            'dipe
    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "spese_notifica_60g"
    '            objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SPESE")) & " €"
    '            oArrBookmark.Add(objBookmark)

    '            Dim Importo_arrotond As String
    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "Importo_arrotond"
    '            'objBookmark.Valore = FormatNumberToZero(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_ARROTONDAMENTO")).Replace(",", ".") & " €"
    '            Importo_arrotond = dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_ARROTONDAMENTO")
    '            objBookmark.Valore = FormatImport(Importo_arrotond) & " €"
    '            oArrBookmark.Add(objBookmark)

    '            '************************************************************************************
    '            'IMPORTI INTERESSI
    '            '************************************************************************************
    '            objDSImportiInteressi = objCOMACCERT.GetInteressiTotaliPerStampaAccertamenti(objHashTable, IDPROVVEDIMENTO)
    '            objDSImportiInteressiF2 = objCOM.GetInteressiTotaliPerStampaLiquidazione(objHashTable, IDPROVVEDIMENTO)
    '            iRetValImpInt = FillBookMarkIMPORTIINTERESSI(objDSImportiInteressi, objDSImportiInteressiF2, strImportoGiorni, strImportoSemestriACC, strImportoSemestriSAL, strNumSemestriACC, strNumSemestriSAL)
    '            Dim int_mor As String = 0
    '            If iRetValImpInt = True Then
    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "imp_interessi_GIORNI"
    '                'objBookmark.Valore = strImportoGiorni.Replace(",", ".") & " €"
    '                objBookmark.Valore = strImportoGiorni & " €"
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "imp_semestri_ACCONTO"
    '                'objBookmark.Valore = strImportoSemestriACC.Replace(",", ".") & " €"
    '                objBookmark.Valore = strImportoSemestriACC & " €"
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "num_semestri_ACCONTO"
    '                objBookmark.Valore = strNumSemestriACC
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "imp_semestri_SALDO"
    '                'objBookmark.Valore = strImportoSemestriSAL.Replace(",", ".") & " €"
    '                objBookmark.Valore = strImportoSemestriSAL & " €"
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "num_semestri_SALDO"
    '                objBookmark.Valore = strNumSemestriSAL
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "int_mor"
    '                int_mor = CDbl(strImportoGiorni) + CDbl(strImportoSemestriACC) + CDbl(strImportoSemestriSAL)
    '                objBookmark.Valore = FormatImport(int_mor) & " €"
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "int_mor_60g"
    '                objBookmark.Valore = FormatImport(CDbl(strImportoGiorni) * (-1) + CDbl(strImportoSemestriACC) * (-1) + CDbl(strImportoSemestriSAL) * (-1)) & " €"
    '                oArrBookmark.Add(objBookmark)
    '            End If
    '            '************************************************************************************
    '            'FINE IMPORTI INTERESSI
    '            '************************************************************************************

    '            '************************************************************************************
    '            'TOTALI
    '            '************************************************************************************
    '            Dim strImportoTotale As String
    '            'strImportoTotale = CDbl(strImportoDiffImp) + CDbl(int_mor) + CDbl(ImpSanzioni) + CDbl(strImportoSanzioneRidotto) + CDbl(spese_notifica) + CDbl(Importo_arrotond)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "Importo_totale"
    '            strImportoTotale = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_TOTALE"))
    '            'objBookmark.Valore = FormatImport(CDbl(strImportoTotale) - CDbl(strImportoSanzione) + CDbl(strImportoSanzioneRidotto)) & " €"
    '            objBookmark.Valore = FormatImport(strImportoTotale) & " €"
    '            oArrBookmark.Add(objBookmark)

    '            Dim dblIMPORTOARROTONDAMENTO As Double
    '            Dim Importo_totale_60g As String
    '            'Importo_totale_60g = CDbl(strImportoDiffImp) + CDbl(int_mor) + CDbl(ImportoSanzione_60g) + CDbl(strImportoSanzioneRidotto) + CDbl(spese_notifica) + CDbl(Importo_arrotond)
    '            'Importo_totale_60g = CDbl(strImportoDiffImp) + CDbl(int_mor) + CDbl(ImportoSanzione_60g) + CDbl(strImportoSanzioneRidotto_60g) + CDbl(spese_notifica)
    '            'dblIMPORTOARROTONDATO = ImportoArrotondato(CDbl(Importo_totale_60g))
    '            'dblIMPORTOARROTONDAMENTO = dblIMPORTOARROTONDATO - CDbl(Importo_totale_60g)
    '            'Importo_totale_60g = dblIMPORTOARROTONDATO
    '            Importo_totale_60g = dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_TOTALE_RIDOTTO")
    '            Importo_totale_60g = Importo_totale_60g * (-1)
    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "Importo_totale_60g"
    '            objBookmark.Valore = FormatImport(Importo_totale_60g) & " €"
    '            oArrBookmark.Add(objBookmark)

    '            'dipe
    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "Importo_arrotond_60g"
    '            dblIMPORTOARROTONDAMENTO = dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_ARROTONDAMENTO_RIDOTTO")
    '            dblIMPORTOARROTONDAMENTO = dblIMPORTOARROTONDAMENTO * (-1)
    '            objBookmark.Valore = FormatImport(dblIMPORTOARROTONDAMENTO) & " €"
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "ImpTotNonRidotto"
    '            'objBookmark.Valore = FormatImport(CDbl(strImportoTotale) - CDbl(strImportoSanzione) + CDbl(strImportoSanzioneRidotto)) & " €"
    '            objBookmark.Valore = FormatImport(Importo_totale_60g) & " €"
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "Importo_totale_1"
    '            'objBookmark.Valore = FormatImport(CDbl(strImportoTotale) - CDbl(strImportoSanzione_60g) + CDbl(strImportoSanzioneRidotto_60g)) & " €"
    '            objBookmark.Valore = FormatImport(Importo_totale_60g) & " €"
    '            oArrBookmark.Add(objBookmark)

    '            '************************************************************************************
    '            'IMPORTI DA DICHIARATO
    '            '************************************************************************************
    '            'Dim dtICI As DataTable
    '            'dtICI = objICI.GetImportoDovuto(iCodContrib, strANNO, constsession.idente)
    '            Dim imp_dov_dich_acc, imp_dov_dich_saldo As Decimal
    '            'dsImmobiliDichiarato = objCOMACCERT.getImmobiliDichiaratiPerStampaAccertamenti(objHashTable, IDPROCEDIMENTO)
    '            FillBookMarkIMP_DOV_DICH(dsImmobiliDichiarato, imp_dov_dich_acc, imp_dov_dich_saldo)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "imp_dov_dich_acc"
    '            'objBookmark.Valore = FormatImport(dtICI.Rows(0)("ACCONTO")) & " €"
    '            objBookmark.Valore = FormatImport(imp_dov_dich_acc) & " €"
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "imp_dov_dich_saldo"
    '            'objBookmark.Valore = FormatImport(dtICI.Rows(0)("SALDO")) & " €"
    '            objBookmark.Valore = FormatImport(imp_dov_dich_saldo) & " €"
    '            oArrBookmark.Add(objBookmark)

    '            '************************************************************************************
    '            'FINE IMPORTI DA DICHIARATO
    '            '************************************************************************************

    '            '************************************************************************************
    '            'IMPORTI VERSATI
    '            '************************************************************************************
    '            Dim dvVers As DataView
    '            Dim i As Integer
    '            Dim dTot, dTotUS, dTotNoAS As Double

    '            'Senza Flag Acconto e saldo selezionati
    '            dvVers = objVers.GetVersamentiPerTipologia(ConstSession.IdEnte, strANNO, iCodContrib, False, False, False, Utility.Costanti.TRIBUTO_ICI)
    '            dTotNoAS = 0
    '            For i = 0 To dvVers.Table.Rows.Count - 1
    '                dTotNoAS += dvVers.Table.Rows(i)("ImportoPagato")
    '            Next

    '            'Unica soluzione
    '            dvVers = objVers.GetVersamentiPerTipologia(ConstSession.IdEnte, strANNO, iCodContrib, True, True, False, Utility.Costanti.TRIBUTO_ICI)
    '            dTotUS = 0
    '            For i = 0 To dvVers.Table.Rows.Count - 1
    '                dTotUS += dvVers.Table.Rows(i)("ImportoPagato")
    '            Next

    '            'Acconto
    '            dvVers = objVers.GetVersamentiPerTipologia(ConstSession.IdEnte, strANNO, iCodContrib, True, False, False, Utility.Costanti.TRIBUTO_ICI)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "imp_vers_acc"
    '            dTot = dTotUS + dTotNoAS
    '            For i = 0 To dvVers.Table.Rows.Count - 1
    '                dTot += dvVers.Table.Rows(i)("ImportoPagato")
    '            Next
    '            objBookmark.Valore = FormatImport(dTot) & " €"
    '            oArrBookmark.Add(objBookmark)

    '            'saldo
    '            dvVers = objVers.GetVersamentiPerTipologia(ConstSession.IdEnte, strANNO, iCodContrib, False, True, False, Utility.Costanti.TRIBUTO_ICI)
    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "imp_vers_saldo"
    '            dTot = 0
    '            For i = 0 To dvVers.Table.Rows.Count - 1
    '                dTot += dvVers.Table.Rows(i)("ImportoPagato")
    '            Next
    '            objBookmark.Valore = FormatImport(dTot) & " €"
    '            oArrBookmark.Add(objBookmark)

    '            '************************************************************************************
    '            'FINE IMPORTI VERSATI
    '            '************************************************************************************
    '            objTestataDOC = New oggettoTestata
    '            objTestataDOC.Atto = "Documenti"
    '            objTestataDOC.Dominio = "Provvedimenti"
    '            objTestataDOC.Ente = ConstSession.DescrizioneEnte.Replace("Comune di ", "")
    '            objTestataDOC.Filename = FileNameContrib & "_" & dsDatiProvv.Tables(0).Rows(iCount)("NUMERO_ATTO").ToString.Replace("/", "") & "_" & sFilenameDOC

    '            ArrayBookMark = CType(oArrBookmark.ToArray(GetType(oggettiStampa)), oggettiStampa())

    '            objToPrint = New oggettoDaStampareCompleto
    '            objToPrint.TestataDOC = objTestataDOC
    '            objToPrint.TestataDOT = objTestataDOT
    '            objToPrint.Stampa = ArrayBookMark

    '            oArrListOggettiDaStampare.Add(objToPrint)
    '        Next

    '        Dim GruppoDOC As New GruppoDocumenti
    '        Dim GruppoDOCUMENTI As GruppoDocumenti()
    '        Dim ArrListGruppoDOC As New ArrayList

    '        Dim ArrOggCompleto As oggettoDaStampareCompleto()
    '        Dim objTestataGruppo As New oggettoTestata

    '        ArrOggCompleto = CType(oArrListOggettiDaStampare.ToArray(GetType(oggettoDaStampareCompleto)), oggettoDaStampareCompleto())

    '        GruppoDOC.OggettiDaStampare = ArrOggCompleto
    '        GruppoDOC.TestataGruppo = objTestataGruppo

    '        ArrListGruppoDOC.Add(GruppoDOC)

    '        GruppoDOCUMENTI = CType(ArrListGruppoDOC.ToArray(GetType(GruppoDocumenti)), GruppoDocumenti())
    '        PrintDoc(GruppoDOCUMENTI)
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.STAMPA_RIMBORSOICI.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '        Dim sScript As String = ""
    '        sScript += "GestAlert('a', 'danger', '', '', 'Errore in fase di stampa');"
    '        RegisterScript(sScript, Me.GetType())
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strTipoDoc"></param>
    ''' <param name="bIsBozza"></param>
    ''' <param name="sTributo"></param>
    Private Sub STAMPA_ANNULLAMENTO(ByVal strTipoDoc As String, ByVal bIsBozza As Boolean, sTributo As String)
        Dim sFilenameDOT, sFilenameDOC As String
        Dim objDSstampa As DataSet
        Dim objUtility As New MyUtility
        ' creo l'oggetto testata per l'oggetto da stampare
        'serve per indicare la posizione di salvataggio e il nome del file.
        Dim objTestataDOC As oggettoTestata
        Dim objTestataDOT As New oggettoTestata
        Dim FileNameContrib As String
        Dim IDPROVVEDIMENTO As String
        Dim strANNO As String
        Dim oArrBookmark As ArrayList
        Dim iCount As Integer
        Dim objBookmark As oggettiStampa
        Dim oArrListOggettiDaStampare As New ArrayList
        Dim objToPrint As oggettoDaStampareCompleto
        Dim ArrayBookMark As oggettiStampa()
        Dim iCodContrib As Integer
        Dim dsDatiProvv As New DataSet
        Try
            'contiene i dati anagrafici visualizzati nella grilgia della ricerca lettere
            objDSstampa = CType(Session("PROVVEDIMENTI_DA_STAMPARE"), DataSet)

            ' COMPONGO IL PERCORSO DEL FILE DOT

            sFilenameDOT = "Annullamento" & sTributo & "_" & ConstSession.IdEnte & ".doc"
            sFilenameDOC = "Annullamento" & sTributo & "_"

            objTestataDOT.Atto = "Template"
            objTestataDOT.Dominio = "Provvedimenti"
            objTestataDOT.Ente = ConstSession.DescrizioneEnte.Replace("Comune di ", "")
            objTestataDOT.Filename = sFilenameDOT

            Dim NomeDbOpenGov As String = ConfigurationManager.AppSettings("NOME_DATABASE_OPENGOV").ToString()

            Dim objHashTable As Hashtable = New Hashtable
            objHashTable.Add("CodENTE", ConstSession.IdEnte)
            objHashTable.Add("CODENTE", ConstSession.IdEnte)
            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
            objHashTable.Add("NomeDbOpenGov", NomeDbOpenGov)
            objHashTable.Add("CODTRIBUTO", sTributo)

            Dim objCOM As IElaborazioneLiquidazioni = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneLiquidazioni), ConstSession.URLServiziLiquidazioni)

            For iCount = 0 To objDSstampa.Tables(0).Rows.Count - 1
                iCodContrib = objDSstampa.Tables(0).Rows(iCount)("COD_CONTRIBUENTE")

                IDPROVVEDIMENTO = objDSstampa.Tables(0).Rows(iCount)("ID_PROVVEDIMENTO")
                objHashTable.Add("COD_CONTRIBUENTE", iCodContrib)
                dsDatiProvv = objCOM.GetProvvedimentoPerStampaLiquidazione(ConstSession.StringConnection, objHashTable, IDPROVVEDIMENTO)
                strANNO = dsDatiProvv.Tables(0).Rows(iCount)("ANNO")
                objHashTable.Add("ANNO", strANNO)
                '*** 20140701 - IMU/TARES ***
                If strANNO >= 2012 And sTributo = Utility.Costanti.TRIBUTO_ICI Then
                    sFilenameDOT = sFilenameDOT.Replace(Utility.Costanti.TRIBUTO_ICI, "IMU")
                    sFilenameDOC = sFilenameDOC.Replace(Utility.Costanti.TRIBUTO_ICI, "IMU")
                    objTestataDOT.Filename = sFilenameDOT
                End If
                '*** ***

                oArrBookmark = New ArrayList

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "bozza"
                If bIsBozza = True Then
                    objBookmark.Valore = "BOZZA"
                Else
                    objBookmark.Valore = ""
                End If
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "nome_ente"
                objBookmark.Valore = CStr(Session("DESCRIZIONE_ENTE")).ToUpper
                oArrBookmark.Add(objBookmark)

                '************************************************************************************
                'DATI ANAGRAFICI
                '************************************************************************************
                oArrBookmark = FillBookMarkAnagrafica(oArrBookmark, iCodContrib, sTributo, FileNameContrib)
                If oArrBookmark Is Nothing Then
                    Throw New Exception("errore in popolamento anagrafica")
                End If
                '---------------------------------------------------------------------------------

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "anno_ici"
                objBookmark.Valore = strANNO
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "anno_provvedimento_ann"
                objBookmark.Valore = strANNO
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "n_provvedimento"
                objBookmark.Valore = objUtility.CToStr(dsDatiProvv.Tables(0).Rows(iCount)("NUMERO_ATTO"))
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "n_provvedimento_ann"
                objBookmark.Valore = objUtility.CToStr(dsDatiProvv.Tables(0).Rows(iCount)("NUMERO_ATTO"))
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "data_provvedimento"
                If dsDatiProvv.Tables(0).Rows(iCount)("DATA_ELABORAZIONE") Is System.DBNull.Value Then
                    objBookmark.Valore = ""
                Else
                    objBookmark.Valore = objUtility.GiraDataFromDB(dsDatiProvv.Tables(0).Rows(iCount)("DATA_ELABORAZIONE"))
                End If
                oArrBookmark.Add(objBookmark)

                objTestataDOC = New oggettoTestata
                objTestataDOC.Atto = "Documenti"
                objTestataDOC.Dominio = "Provvedimenti"
                objTestataDOC.Ente = ConstSession.DescrizioneEnte.Replace("Comune di ", "")
                objTestataDOC.Filename = FileNameContrib & "_" & dsDatiProvv.Tables(0).Rows(iCount)("NUMERO_ATTO").ToString.Replace("/", "") & "_" & sFilenameDOC
                ArrayBookMark = CType(oArrBookmark.ToArray(GetType(oggettiStampa)), oggettiStampa())

                objToPrint = New oggettoDaStampareCompleto
                objToPrint.TestataDOC = objTestataDOC
                objToPrint.TestataDOT = objTestataDOT
                objToPrint.Stampa = ArrayBookMark

                oArrListOggettiDaStampare.Add(objToPrint)
            Next

            Dim GruppoDOC As New GruppoDocumenti
            Dim GruppoDOCUMENTI As GruppoDocumenti()
            Dim ArrListGruppoDOC As New ArrayList

            Dim ArrOggCompleto As oggettoDaStampareCompleto()
            Dim objTestataGruppo As New oggettoTestata

            ArrOggCompleto = CType(oArrListOggettiDaStampare.ToArray(GetType(oggettoDaStampareCompleto)), oggettoDaStampareCompleto())

            GruppoDOC.OggettiDaStampare = ArrOggCompleto
            GruppoDOC.TestataGruppo = objTestataGruppo

            ArrListGruppoDOC.Add(GruppoDOC)

            GruppoDOCUMENTI = CType(ArrListGruppoDOC.ToArray(GetType(GruppoDocumenti)), GruppoDocumenti())
            PrintDoc(GruppoDOCUMENTI)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.STAMPA_ANNULLAMENTO.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
            Dim sScript As String = ""
            sScript += "GestAlert('a', 'danger', '', '', 'Errore in fase di stampa');"
            RegisterScript(sScript, Me.GetType())
        End Try
    End Sub
    '*** 20140701 - IMU/TARES ***
    'Private Function STAMPA_ACCERTAMENTOTARSU(ByVal strTipoDoc As String, ByVal bIsBozza As Boolean)

    '    Dim sFilenameDOT, sFilenameDOC As String
    '    Dim objDSstampa As DataSet
    '    Dim objUtility As New MyUtility
    '    'contiene i dati anagrafici visualizzati nella grilgia della ricerca lettere
    '    objDSstampa = CType(Session("PROVVEDIMENTI_DA_STAMPARE"), DataSet)

    '    ' creo l'oggetto testata per l'oggetto da stampare
    '    'serve per indicare la posizione di salvataggio e il nome del file.
    '    Dim objTestataDOC As oggettoTestata
    '    Dim objTestataDOT As New oggettoTestata
    '    Dim objDSTipiInteressiL As New DataSet
    '    Dim IDPROVVEDIMENTO As String
    '    Dim IDPROCEDIMENTO As String
    '    Dim strTIPODOCUMENTO As String
    '    Dim strANNO As String

    '    ' COMPONGO IL PERCORSO DEL FILE DOT
    '    'sFilenameDOT = "Accertamento_" & constsession.idente & ".dot"
    '    'sFilenameDOT = "AccertamentoTARSU.dot"
    '    'sFilenameDOC = "Accertamento"
    '    'strTIPODOCUMENTO = Costanti.COD_TIPO_DOC_ACCERTAMENTO

    '    sFilenameDOT = "AccertamentoTARSU_" & ConstSession.IdEnte & ".dot"
    '    sFilenameDOC = "AccertamentoTARSU"
    '    strTIPODOCUMENTO = Costanti.COD_TIPO_DOC_ACCERTAMENTO

    '    objTestataDOT.Atto = "Template"
    '    objTestataDOT.Dominio = "Provvedimenti"
    '    objTestataDOT.Ente = ConstSession.DescrizioneEnte
    '    objTestataDOT.Filename = sFilenameDOT

    '    Dim oArrBookmark As ArrayList
    '    Dim iCount As Integer
    '    Dim iImmobili As Integer
    '    Dim iErrori As Integer

    '    Dim objBookmark As oggettiStampa
    '    Dim oArrListOggettiDaStampare As New ArrayList
    '    Dim objToPrint As oggettoDaStampareCompleto
    '    Dim ArrayBookMark As oggettiStampa()
    '    Dim iCodContrib As Integer

    '    'Dim culture As IFormatProvider
    '    'culture = New CultureInfo("it-IT", True)
    '    'System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")
    '    Dim strRiga As String=""
    '    Dim strImmoTemp As String = String.Empty
    '    Dim strErroriTemp As String = String.Empty
    '    Dim strImmoTempTitolo As String = String.Empty
    '    Dim Anno As String = String.Empty
    '    'Dim IDdichiarazione As Long
    '    'Dim IDImmobile As Long

    '    Dim dsDatiProvv As New DataSet
    '    Dim dsImmobiliDichiarato As New DataSet
    '    Dim dsVersamenti As New DataSet
    '    Dim dsImmobiliAccertati As New DataSet
    '    Dim dsImmobiliDichAcc As New DataSet
    '    Dim objDSTipiInteressi As New DataSet
    '    Dim objDSElencoSanzioni As New DataSet
    '    Dim objDSElencoAddizionali As New DataSet
    '    Dim objDSElencoSanzioniF2 As New DataSet
    '    Dim objDSImportiInteressi As New DataSet
    '    Dim objDSImportiInteressiF2 As New DataSet
    '    '---------------------------------------
    '    'var per popolare i bookmark relativi 
    '    'alla sezione degli importi interessi
    '    ''Dim strImportoGiorni As String
    '    ''Dim strImportoSemestriACC As String
    '    ''Dim strImportoSemestriSAL As String
    '    ''Dim strNumSemestriACC As String
    '    ''Dim strNumSemestriSAL As String
    '    Dim iRetValImpInt As Boolean
    '    Dim ImportoTotaleRidotto As Double
    '    Dim ImportoTotAddizionali, ImportoTotSanzioni As Double
    '    '---------------------------------------

    '    'Dim DSperInsertTabStorico As DataSet = CreateDataSetStampa()
    '    'Dim dr As DataRow

    '    'Dim strConnectionStringOPENgovProvvedimenti As String

    '    'Dim objSessione As CreateSessione
    '    Dim strWFErrore As String
    '    objSessione = New CreateSessione(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))
    'Try
    '    If Not objSessione.CreaSessione(ConstSession.UserName, strWFErrore) Then
    '        Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '    End If

    '    'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString

    '    Dim objHashTable As Hashtable = New Hashtable
    '    objHashTable.Add("CodENTE", ConstSession.IdEnte)
    '    objHashTable.Add("CODENTE", ConstSession.IdEnte)
    '    objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)

    '    Dim objCOM As IElaborazioneLiquidazioni = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneLiquidazioni), ConfigurationManager.AppSettings("URLServiziLiquidazioni"))
    '    Dim objCOMTipoVoci As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)
    '    Dim objCOMACCERT As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
    '    Dim strImportoInteressi As String

    '    Dim UNISCI_IMMOBILI_ACCERTAMENTI As Boolean

    '    Dim NomeDbTarsu As String = ConfigurationManager.AppSettings("NOME_DATABASE_TARSU").ToString()
    '    objHashTable.Add("NomeDbTarsu", NomeDbTarsu)

    '    For iCount = 0 To objDSstampa.Tables(0).Rows.Count - 1

    '        iCodContrib = objDSstampa.Tables(0).Rows(iCount)("COD_CONTRIBUENTE")

    '        objTestataDOC = New oggettoTestata
    '        objTestataDOC.Atto = "Documenti"
    '        objTestataDOC.Dominio = "Provvedimenti"
    '        objTestataDOC.Ente = ConstSession.DescrizioneEnte
    '        objTestataDOC.Filename = iCodContrib & "_" & ConstSession.IdEnte & sFilenameDOC

    '        oArrBookmark = New ArrayList

    '        IDPROVVEDIMENTO = objDSstampa.Tables(0).Rows(iCount)("ID_PROVVEDIMENTO")
    '        objHashTable.Add("COD_CONTRIBUENTE", iCodContrib)

    '        dsDatiProvv = objCOM.GetProvvedimentoPerStampaLiquidazione(objHashTable, IDPROVVEDIMENTO)

    '        IDPROCEDIMENTO = dsDatiProvv.Tables(0).Rows(iCount)("ID_PROCEDIMENTO")
    '        strANNO = dsDatiProvv.Tables(0).Rows(iCount)("ANNO")

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "bozza"
    '        If bIsBozza = True Then
    '            objBookmark.Valore = "BOZZA"
    '        Else
    '            objBookmark.Valore = ""
    '        End If
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "nome_ente"
    '        objBookmark.Valore = CStr(Session("DESCRIZIONE_ENTE")).ToUpper
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "TipoProvvedimento"
    '        objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("TIPO_PROVVEDIMENTO")).ToUpper()
    '        oArrBookmark.Add(objBookmark)

    '        '************************************************************************************
    '        'DATI ANAGRAFICI
    '        '************************************************************************************
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "cognome"
    '        objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("COGNOME"))
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "nome"
    '        objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("NOME"))
    '        oArrBookmark.Add(objBookmark)

    '        'Nominativo_CO
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "Nominativo_CO"
    '        objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("CO"))
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "via_residenza"
    '        If Not IsDBNull(dsDatiProvv.Tables(0).Rows(iCount)("VIA_CO")) Then
    '            If CStr(dsDatiProvv.Tables(0).Rows(iCount)("VIA_CO")) <> "" Then
    '                objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("VIA_CO"))
    '            Else
    '                objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("VIA_RES"))
    '            End If
    '        Else
    '            objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("VIA_RES"))
    '        End If
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "civico_residenza"
    '        If Not IsDBNull(dsDatiProvv.Tables(0).Rows(iCount)("CIVICO_CO")) Then
    '            If CStr(dsDatiProvv.Tables(0).Rows(iCount)("CIVICO_CO")) <> "" Then
    '                objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("CIVICO_CO"))
    '            Else
    '                objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("CIVICO_RES"))
    '            End If
    '        Else
    '            objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("CIVICO_RES"))
    '        End If
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "frazione_residenza"
    '        If Not IsDBNull(dsDatiProvv.Tables(0).Rows(iCount)("FRAZIONE_CO")) Then
    '            If CStr(dsDatiProvv.Tables(0).Rows(iCount)("FRAZIONE_CO")) <> "" Then
    '                objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("FRAZIONE_CO"))
    '            Else
    '                objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("FRAZIONE_RES"))
    '            End If
    '        Else
    '            objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("FRAZIONE_RES"))
    '        End If
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "cap_residenza"
    '        If Not IsDBNull(dsDatiProvv.Tables(0).Rows(iCount)("CAP_CO")) Then
    '            If CStr(dsDatiProvv.Tables(0).Rows(iCount)("CAP_CO")) <> "" Then
    '                objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("CAP_CO"))
    '            Else
    '                objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("CAP_RES"))
    '            End If
    '        Else
    '            objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("CAP_RES"))
    '        End If
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "citta_residenza"
    '        If Not IsDBNull(dsDatiProvv.Tables(0).Rows(iCount)("CITTA_CO")) Then
    '            If CStr(dsDatiProvv.Tables(0).Rows(iCount)("CITTA_CO")) <> "" Then
    '                objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("CITTA_CO"))
    '            Else
    '                objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("CITTA_RES"))
    '            End If
    '        Else
    '            objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("CITTA_RES"))
    '        End If
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "prov_residenza"
    '        If Not IsDBNull(dsDatiProvv.Tables(0).Rows(iCount)("PROVINCIA_CO")) Then
    '            If CStr(dsDatiProvv.Tables(0).Rows(iCount)("PROVINCIA_CO")) <> "" Then
    '                objBookmark.Valore = "(" & FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("PROVINCIA_CO")) & ")"
    '            Else
    '                objBookmark.Valore = "(" & FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("PROVINCIA_RES")) & ")"
    '            End If
    '        Else
    '            objBookmark.Valore = "(" & FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("PROVINCIA_RES")) & ")"
    '        End If
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "codice_fiscale"
    '        objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("CODICE_FISCALE"))
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "partita_iva"
    '        objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("PARTITA_IVA"))
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "codice_fiscale_2"
    '        objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("CODICE_FISCALE"))
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "partita_iva_2"
    '        objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("PARTITA_IVA"))
    '        oArrBookmark.Add(objBookmark)


    '        '---------------------------------------------------------------------------------

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "anno_imposta"
    '        objBookmark.Valore = strANNO
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "anno_imposta_1"
    '        objBookmark.Valore = strANNO
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "n_provvedimento"
    '        objBookmark.Valore = objUtility.CToStr(dsDatiProvv.Tables(0).Rows(iCount)("NUMERO_ATTO"))

    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "data_provvedimento"
    '        objBookmark.Valore = objUtility.GiraDataFromDB(dsDatiProvv.Tables(0).Rows(iCount)("DATA_ELABORAZIONE"))
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "anno_imposta"
    '        objBookmark.Valore = strANNO 'dsDatiProvv.Tables(0).Rows(iCount)("ANNO")
    '        oArrBookmark.Add(objBookmark)

    '        'objBookmark = New oggettiStampa
    '        'objBookmark.Descrizione = "anno_imposta1"
    '        'objBookmark.Valore = strANNO 'dsDatiProvv.Tables(0).Rows(iCount)("ANNO")
    '        'oArrBookmark.Add(objBookmark)


    '        UNISCI_IMMOBILI_ACCERTAMENTI = CBool(ConfigurationManager.AppSettings("UNISCI_IMMOBILI_ACCERTAMENTI"))

    '        If UNISCI_IMMOBILI_ACCERTAMENTI Then

    '            dsImmobiliDichAcc = objCOMACCERT.getImmobiliDichAccPerStampaAccertamentiTARSU(objHashTable, IDPROVVEDIMENTO)

    '            strRiga = FillBookMarkDICHACCtarsu(dsImmobiliDichAcc, strANNO, Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "elenco_immobili"
    '            objBookmark.Valore = strRiga
    '            oArrBookmark.Add(objBookmark)


    '        Else


    '            '************************************************************************************
    '            'ELENCO IMMOBILI DICHIARATI
    '            '************************************************************************************
    '            dsImmobiliDichiarato = objCOMACCERT.getImmobiliDichiaratiPerStampaAccertamentiTARSU(objHashTable, IDPROVVEDIMENTO)
    '            strRiga = FillBookMarkDICHIARATOtarsu(dsImmobiliDichiarato, Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "elenco_immobili_dich"
    '            objBookmark.Valore = "Dettaglio Immobili Dichiarati" & vbCrLf & strRiga
    '            oArrBookmark.Add(objBookmark)

    '            '************************************************************************************
    '            'ELENCO IMMOBILI ACCERATI
    '            '************************************************************************************
    '            dsImmobiliAccertati = objCOMACCERT.getImmobiliAccertatiPerStampaAccertamentiTARSU(objHashTable, IDPROVVEDIMENTO)
    '            strRiga = FillBookMarkACCERTATOtarsu(dsImmobiliAccertati)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "elenco_immobili_acce"
    '            objBookmark.Valore = "Dettaglio Immobili Accertati" & vbCrLf & strRiga
    '            oArrBookmark.Add(objBookmark)
    '        End If

    '        ''************************************************************************************
    '        ''ELENCO VERSAMENTI
    '        ''************************************************************************************
    '        '[DETTAGLIO PAGAMENTI] 
    '        'ANNO	DATA PAGAMENTO	IMPORTO PAGATO
    '        objHashTable.Add("ANNO", strANNO)

    '        dsVersamenti = objCOMACCERT.getVersamentiPerStampaAccertamentiTARSU(objHashTable, IDPROVVEDIMENTO)
    '        strRiga = FillBookMarkVersamentiTARSU(dsVersamenti)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "elenco_versamenti"
    '        objBookmark.Valore = "Dettaglio Versamenti" & vbCrLf & strRiga
    '        oArrBookmark.Add(objBookmark)



    '        '************************************************************************************
    '        'ELENCO INTERESSI CONFIGURATI
    '        '************************************************************************************
    '        objHashTable.Add("CODTIPOINTERESSE", "5")
    '        objHashTable.Add("DAL", "")
    '        objHashTable.Add("AL", "")
    '        objHashTable.Add("TASSO", "")
    '        objHashTable.Add("CODTRIBUTO", Session("COD_TRIBUTO"))
    '        objHashTable.Add("riducibile", "0")

    '        objDSTipiInteressiL = objCOM.GetElencoInteressiPerStampaLiquidazione(objHashTable, IDPROVVEDIMENTO)
    '        objDSTipiInteressi = objCOMACCERT.GetElencoInteressiPerStampaAccertamenti(objHashTable, IDPROVVEDIMENTO)
    '        strRiga = FillBookMarkELENCOINTERESSI(objDSTipiInteressiL, objDSTipiInteressi, Session("COD_TRIBUTO"))
    '        'strRiga += FillBookMarkTOTALEINTERESSI(objDSTipiInteressiL, objDSTipiInteressi)

    '        'objDSTipiInteressi = objCOMTipoVoci.GetTipoInteresse(objHashTable)
    '        'strRiga = FillBookMarkELENCOINTERESSITARSU(objDSTipiInteressi)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "elenco_interessi"
    '        objBookmark.Valore = strRiga
    '        oArrBookmark.Add(objBookmark)

    '        ''************************************************************************************
    '        ''ELENCO SANZIONI APPLICATE CON IMPORTO 
    '        ''************************************************************************************
    '        objDSElencoSanzioni = objCOMACCERT.GetElencoSanzioniPerStampaAccertamenti(objHashTable, IDPROVVEDIMENTO)
    '        objDSElencoSanzioniF2 = objCOM.GetElencoSanzioniPerStampaLiquidazione(objHashTable, IDPROVVEDIMENTO)
    '        strRiga = FillBookMarkELENCOSANZIONITARSU(objDSElencoSanzioni, objDSElencoSanzioniF2, ImportoTotSanzioni)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "elenco_sanzioni"
    '        objBookmark.Valore = strRiga
    '        oArrBookmark.Add(objBookmark)

    '        strRiga = FillBookMarkELENCOVIOLAZIONI(objDSElencoSanzioni)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "Violazione"
    '        objBookmark.Valore = strRiga
    '        oArrBookmark.Add(objBookmark)


    '        'Tassa	                 €     [SOMMA ALGEBRICA DELLE DIFF. DI IMPOSTA]
    '        'Addizionale ex- Eca 5%	 €     
    '        'Maggiorazione ex-Eca 5% €     [IN BASE ALLE CONFIGURAZIONI]
    '        'Tributo Provinciale 1%  €     
    '        'Sanzione Amministrativa €     [SOMMA ALGEBRICA DELLE SANZIONI]
    '        'Interessi 				 €     [SOMMA ALGEBRICA DEGLI INTERESSI]
    '        'Spese di notifica       €     [SPESE DI NOTIFICA]
    '        'Arrotondamento          €     [ARROTONDAMENTO]
    '        'Totale                  €     [IMPORTO TOTALE AVVISO]


    '        '************************************************************************************
    '        'IMPORTI (DIFF IMPOSTA - SANZIONI - INTERESSI - SPESE - ecc...
    '        '************************************************************************************
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "imposta_dovuta"
    '        'objBookmark.Valore = FormatNumberToZero(dsDatiProvv.Tables(0).Rows(iCount)("TOTALE_DICHIARATO")).Replace(",", ".") & " €"
    '        objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_DIFFERENZA_IMPOSTA")) & " €"
    '        oArrBookmark.Add(objBookmark)



    '        objDSElencoAddizionali = objCOMACCERT.getAddizionaliPerStampaAccertamentiTARSU(objHashTable, IDPROVVEDIMENTO)
    '        strRiga = FillBookMarkELENCOADDIZIONALI(objDSElencoAddizionali, ImportoTotAddizionali)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "Elenco_Addizionali"
    '        objBookmark.Valore = strRiga
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "Importo_Sanzione"
    '        'objBookmark.Valore = FormatNumberToZero(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SANZIONI")).Replace(",", ".") & " €"
    '        objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SANZIONI")) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "spese_notifica"
    '        'objBookmark.Valore = FormatNumberToZero(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SPESE")).Replace(",", ".") & " €"
    '        objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SPESE")) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "Importo_arrotond"
    '        'objBookmark.Valore = FormatNumberToZero(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_ARROTONDAMENTO")).Replace(",", ".") & " €"
    '        objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_ARROTONDAMENTO")) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "Importo_totale"
    '        'objBookmark.Valore = FormatNumberToZero(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_TOTALE")).Replace(",", ".") & " €"
    '        objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_TOTALE")) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "Importo_totale_2"
    '        'objBookmark.Valore = FormatNumberToZero(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_TOTALE")).Replace(",", ".") & " €"
    '        objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_TOTALE")) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        '************************************************************************************
    '        'IMPORTI INTERESSI
    '        'objDSImportiInteressi = objCOMACCERT.GetInteressiPerStampaAccertamenti(objHashTable, IDPROVVEDIMENTO)
    '        'objDSImportiInteressiF2 = objCOM.GetInteressiPerStampaLiquidazione(objHashTable, IDPROVVEDIMENTO)
    '        'FillBookMarkIMPORTIINTERESSITARSU(objDSImportiInteressi, strImportoInteressi)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "importo_interessi"
    '        objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_INTERESSI")) & " €"
    '        oArrBookmark.Add(objBookmark)
    '        '************************************************************************************

    '        '************************************************'
    '        'devo stampare importo totale con importo_sanzione_ridotto se importo_sanzione > importo_sanzione_ridotto
    '        ''************************************************************************************
    '        ''ELENCO SANZIONI APPLICATE CON IMPORTO 
    '        ''************************************************************************************
    '        ''objDSElencoSanzioni = objCOMACCERT.GetElencoSanzioniPerStampaAccertamenti(objHashTable, IDPROVVEDIMENTO)
    '        ''objDSElencoSanzioniF2 = objCOM.GetElencoSanzioniPerStampaLiquidazione(objHashTable, IDPROVVEDIMENTO)
    '        ''strRiga = FillBookMarkELENCOSANZIONITARSU(objDSElencoSanzioni, objDSElencoSanzioniF2, ImportoTotSanzioni)

    '        ''objBookmark = New oggettiStampa
    '        ''objBookmark.Descrizione = "elenco_sanzioni_1"
    '        ''objBookmark.Valore = strRiga
    '        ''oArrBookmark.Add(objBookmark)

    '        'Tassa	                 €     [SOMMA ALGEBRICA DELLE DIFF. DI IMPOSTA]
    '        'Addizionale ex- Eca 5%	 €     
    '        'Maggiorazione ex-Eca 5% €     [IN BASE ALLE CONFIGURAZIONI]
    '        'Tributo Provinciale 1%  €     
    '        'Sanzione Amministrativa €     [SOMMA ALGEBRICA DELLE SANZIONI]
    '        'Interessi 				 €     [SOMMA ALGEBRICA DEGLI INTERESSI]
    '        'Spese di notifica       €     [SPESE DI NOTIFICA]
    '        'Arrotondamento          €     [ARROTONDAMENTO]
    '        'Totale                  €     [IMPORTO TOTALE AVVISO]


    '        '************************************************************************************
    '        'IMPORTI (DIFF IMPOSTA - SANZIONI - INTERESSI - SPESE - ecc...
    '        '************************************************************************************
    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "imposta_dovuta_1"
    '        'objBookmark.Valore = FormatNumberToZero(dsDatiProvv.Tables(0).Rows(iCount)("TOTALE_DICHIARATO")).Replace(",", ".") & " €"
    '        objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_DIFFERENZA_IMPOSTA")) & " €"
    '        oArrBookmark.Add(objBookmark)


    '        ''objHashTable.Add("ANNO", strANNO)
    '        ''objDSElencoAddizionali = objCOMACCERT.getAddizionaliPerStampaAccertamentiTARSU(objHashTable, IDPROVVEDIMENTO)
    '        strRiga = FillBookMarkELENCOADDIZIONALI(objDSElencoAddizionali, ImportoTotAddizionali)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "Elenco_Addizionali_1"
    '        objBookmark.Valore = strRiga
    '        oArrBookmark.Add(objBookmark)

    '        If Not IsDBNull(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SANZIONI_RIDOTTO")) Then
    '            'altrimenti stampo uguale'
    '            If dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SANZIONI") > dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SANZIONI_RIDOTTO") Then

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "Importo_Sanzione_1"
    '                'objBookmark.Valore = FormatNumberToZero(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SANZIONI")).Replace(",", ".") & " €"
    '                objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SANZIONI_RIDOTTO")) & " €"
    '                oArrBookmark.Add(objBookmark)

    '            Else
    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "Importo_Sanzione_1"
    '                'objBookmark.Valore = FormatNumberToZero(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SANZIONI")).Replace(",", ".") & " €"
    '                objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SANZIONI_RIDOTTO")) & " €"
    '                oArrBookmark.Add(objBookmark)

    '            End If
    '        Else
    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "Importo_Sanzione_1"
    '            'objBookmark.Valore = FormatNumberToZero(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SANZIONI")).Replace(",", ".") & " €"
    '            objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SANZIONI_RIDOTTO")) & " €"
    '            oArrBookmark.Add(objBookmark)

    '        End If

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "spese_notifica_1"
    '        'objBookmark.Valore = FormatNumberToZero(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SPESE")).Replace(",", ".") & " €"
    '        objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SPESE")) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "Importo_arrotond_1"
    '        'objBookmark.Valore = FormatNumberToZero(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_ARROTONDAMENTO")).Replace(",", ".") & " €"
    '        objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_ARROTONDAMENTO_RIDOTTO")) & " €"
    '        oArrBookmark.Add(objBookmark)

    '        objBookmark = New oggettiStampa
    '        objBookmark.Descrizione = "importo_interessi_1"
    '        objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_INTERESSI")) & " €"
    '        oArrBookmark.Add(objBookmark)


    '        'altrimenti stampo uguale'
    '        If Not IsDBNull(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SANZIONI_RIDOTTO")) Then
    '            If dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SANZIONI") > dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SANZIONI_RIDOTTO") Then
    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "Importo_totale_1"
    '                'objBookmark.Valore = FormatNumberToZero(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_TOTALE")).Replace(",", ".") & " €"
    '                'ImportoTotaleRidotto = CDbl(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_DIFFERENZA_IMPOSTA")) + ImportoTotAddizionali + CDbl(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SANZIONI_RIDOTTO")) + CDbl(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SPESE") + CDbl(strImportoInteressi)) + CDbl(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_ARROTONDAMENTO")) + CDbl(strImportoInteressi) & " €"
    '                ImportoTotaleRidotto = CDbl(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_DIFFERENZA_IMPOSTA")) + ImportoTotAddizionali + CDbl(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SANZIONI_RIDOTTO")) + CDbl(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SPESE")) + CDbl(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_INTERESSI")) & " €"
    '                objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_TOTALE_RIDOTTO")) & " €"
    '                oArrBookmark.Add(objBookmark)
    '            Else
    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "Importo_totale_1"
    '                objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_TOTALE_RIDOTTO")) & " €"
    '                oArrBookmark.Add(objBookmark)
    '            End If
    '        Else
    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "Importo_totale_1"
    '            objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_TOTALE_RIDOTTO")) & " €"
    '            oArrBookmark.Add(objBookmark)
    '        End If

    '        ArrayBookMark = CType(oArrBookmark.ToArray(GetType(oggettiStampa)), oggettiStampa())

    '        objToPrint = New oggettoDaStampareCompleto
    '        objToPrint.TestataDOC = objTestataDOC
    '        objToPrint.TestataDOT = objTestataDOT
    '        objToPrint.Stampa = ArrayBookMark

    '        'oArrListOggettiDaStampare = New ArrayList
    '        oArrListOggettiDaStampare.Add(objToPrint)

    '    Next

    '    Dim GruppoDOC As GruppoDocumenti
    '    Dim GruppoDOCUMENTI As GruppoDocumenti()
    '    Dim ArrListGruppoDOC As New ArrayList

    '    Dim ArrOggCompleto As oggettoDaStampareCompleto()
    '    Dim objTestataGruppo As oggettoTestata

    '    GruppoDOC = New Stampa.oggetti.GruppoDocumenti
    '    ArrOggCompleto = Nothing
    '    objTestataGruppo = New Stampa.oggetti.oggettoTestata

    '    ArrOggCompleto = CType(oArrListOggettiDaStampare.ToArray(GetType(oggettoDaStampareCompleto)), oggettoDaStampareCompleto())

    '    GruppoDOC.OggettiDaStampare = ArrOggCompleto
    '    GruppoDOC.TestataGruppo = objTestataGruppo

    '    ArrListGruppoDOC.Add(GruppoDOC)

    '    GruppoDOCUMENTI = CType(ArrListGruppoDOC.ToArray(GetType(GruppoDocumenti)), GruppoDocumenti())

    '    'oggettoDaStampare[]  outArray = (oggettoDaStampare[])oArrListOggettiDaStampare.ToArray(typeof(oggettoDaStampare));

    '    ' oggetto per la stampa
    '    Dim oInterfaceStampaDocOggetti As IElaborazioneStampaDocOggetti
    '    oInterfaceStampaDocOggetti = Activator.GetObject(GetType(IElaborazioneStampaDocOggetti), ConfigurationManager.AppSettings("ServizioStampe").ToString())

    '    Dim retArray As GruppoURL

    '    Dim objTestataComplessiva As New RIBESElaborazioneDocumentiInterface.Stampa.oggetti.oggettoTestata
    '    objTestataComplessiva.Atto = ""
    '    objTestataComplessiva.Dominio = ""
    '    objTestataComplessiva.Ente = ""
    '    objTestataComplessiva.Filename = ""
    '    ' definisco anche il numero di documenti che voglio stampare.
    '    retArray = oInterfaceStampaDocOggetti.StampaDocumenti(objTestataComplessiva, GruppoDOCUMENTI)

    '    Session.Remove("ELENCO_ACCERTAMENTI_STAMPATI")

    '    If Not retArray Is Nothing Then

    '        Session.Add("ELENCO_ACCERTAMENTI_STAMPATI", retArray)

    '        'Dim sUrl As String
    '        'Dim strFileName As String
    '        'Dim CodContr As String
    '        'Dim DRperInsertTabStorico() As DataRow
    '        'Dim icontaUrl As Integer

    '        'For icontaUrl = 0 To retArray.URLDocumenti.Length - 1
    '        '    sUrl = retArray.URLDocumenti(icontaUrl).Url()
    '        '    strFileName = retArray.URLDocumenti(icontaUrl).Name()

    '        '    CodContr = strFileName.Substring(0, strFileName.IndexOf("_"))

    '        '    DRperInsertTabStorico = DSperInsertTabStorico.Tables(0).Select("COD_CONTRIBUENTE='" & CodContr & "'")
    '        '    If DRperInsertTabStorico.Length = 1 Then
    '        '        DRperInsertTabStorico(0)("URL_DOCUMENTO") = sUrl
    '        '    End If

    '        'Next

    '        'If objCOM.setTAB_STORICO_DOCUMENTI(objHashTable, DSperInsertTabStorico) = True Then

    '        dim sScript as string=""
    '        sscript+="parent.corpo.location.href='ViewAccertamentiStampati.aspx';" 
    '        RegisterScript(sScript , Me.GetType())

    '        'End If



    '    End If
    '    If Not IsNothing(objSessione) Then
    '        objSessione.Kill()
    '        objSessione = Nothing
    '    End If
    'Catch ex As Exception
    '    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.STAMPA_ACCERTAMENTOTARSU.errore: ", ex)
    '  Response.Redirect("../../../PaginaErrore.aspx")
    'End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strTipoDoc"></param>
    ''' <param name="bIsBozza"></param>
    ''' <param name="sTributo"></param>
    Private Sub STAMPA_ACCERTAMENTOTARSU(ByVal strTipoDoc As String, ByVal bIsBozza As Boolean, ByVal sTributo As String)
        Dim sFilenameDOT, sFilenameDOC As String
        Dim objDSstampa As DataSet
        Dim objUtility As New MyUtility
        'contiene i dati anagrafici visualizzati nella grilgia della ricerca lettere

        ' creo l'oggetto testata per l'oggetto da stampare
        'serve per indicare la posizione di salvataggio e il nome del file.
        Dim objTestataDOC As oggettoTestata
        Dim objTestataDOT As New oggettoTestata
        Dim FileNameContrib As String
        Dim objDSTipiInteressiL As New DataSet
        Dim IDPROVVEDIMENTO As String
        Dim strANNO As String
        Dim oArrBookmark As ArrayList

        Dim objBookmark As oggettiStampa
        Dim oArrListOggettiDaStampare As New ArrayList
        Dim objToPrint As oggettoDaStampareCompleto
        Dim ArrayBookMark As oggettiStampa()
        Dim iCodContrib As Integer
        Dim strRiga As String = ""
        Dim dsDatiProvv As New DataSet
        Dim dsImmobiliDichiarato As New DataSet
        Dim dsVersamenti As New DataSet
        Dim dsImmobiliAccertati As New DataSet
        Dim dsImmobiliDichAcc As New DataSet
        Dim objDSTipiInteressi As New DataSet
        Dim objDSElencoSanzioni As New DataSet
        Dim objDSElencoSanzioniF2 As New DataSet
        Dim ImportoTotAddizionali, ImportoTotSanzioni As Double
        Dim objHashTable As Hashtable = New Hashtable
        Dim myRowStampa, myRowDett As DataRow

        Try
            objDSstampa = CType(Session("PROVVEDIMENTI_DA_STAMPARE"), DataSet)

            objHashTable.Add("CodENTE", ConstSession.IdEnte)
            objHashTable.Add("CODENTE", ConstSession.IdEnte)
            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)

            Dim objCOM As IElaborazioneLiquidazioni = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneLiquidazioni), ConstSession.URLServiziLiquidazioni)
            Dim objCOMACCERT As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)

            Dim UNISCI_IMMOBILI_ACCERTAMENTI As Boolean

            'Dim NomeDbTarsu As String = ConfigurationManager.AppSettings("NOME_DATABASE_TARSU").ToString()
            'objHashTable.Add("NomeDbTarsu", NomeDbTarsu)

            For Each myRowStampa In objDSstampa.Tables(0).Rows
                iCodContrib = myRowStampa("COD_CONTRIBUENTE")

                sFilenameDOT = "Accertamento" & myRowStampa("tipocalcolo") & "_" & ConstSession.IdEnte & ".doc"
                sFilenameDOC = "Accertamento" & myRowStampa("tipocalcolo")

                objTestataDOT.Atto = "Template"
                objTestataDOT.Dominio = "Provvedimenti"
                objTestataDOT.Ente = ConstSession.DescrizioneEnte.Replace("Comune di ", "")
                objTestataDOT.Filename = sFilenameDOT

                oArrBookmark = New ArrayList

                IDPROVVEDIMENTO = myRowStampa("ID_PROVVEDIMENTO")
                objHashTable.Add("COD_CONTRIBUENTE", iCodContrib)

                dsDatiProvv = objCOM.GetProvvedimentoPerStampaLiquidazione(ConstSession.StringConnection, objHashTable, IDPROVVEDIMENTO)
                For Each myRowDett In dsDatiProvv.Tables(0).Rows
                    strANNO = myRowDett("ANNO")

                    objBookmark = New oggettiStampa
                    objBookmark.Descrizione = "bozza"
                    If bIsBozza = True Then
                        objBookmark.Valore = "BOZZA"
                    Else
                        objBookmark.Valore = ""
                    End If
                    oArrBookmark.Add(objBookmark)

                    objBookmark = New oggettiStampa
                    objBookmark.Descrizione = "nome_ente"
                    objBookmark.Valore = CStr(Session("DESCRIZIONE_ENTE")).ToUpper
                    oArrBookmark.Add(objBookmark)

                    objBookmark = New oggettiStampa
                    objBookmark.Descrizione = "TipoProvvedimento"
                    If myRowStampa("tipocalcolo") = RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRuolo.TipoCalcolo.TARES Then
                        objBookmark.Valore = "AVVISO DI ACCERTAMENTO"
                    Else
                        objBookmark.Valore = FormatStringToEmpty(myRowDett("TIPO_PROVVEDIMENTO")).ToUpper()
                    End If
                    oArrBookmark.Add(objBookmark)

                    '************************************************************************************
                    'DATI ANAGRAFICI
                    '************************************************************************************
                    'oArrBookmark = FillBookMarkAnagrafica(oArrBookmark, myRowDett)
                    oArrBookmark = FillBookMarkAnagrafica(oArrBookmark, iCodContrib, Utility.Costanti.TRIBUTO_TARSU, FileNameContrib)
                    If oArrBookmark Is Nothing Then
                        Throw New Exception("Errore in popola anagrafica")
                    End If
                    '---------------------------------------------------------------------------------

                    objBookmark = New oggettiStampa
                    objBookmark.Descrizione = "anno_imposta"
                    objBookmark.Valore = strANNO
                    oArrBookmark.Add(objBookmark)

                    objBookmark = New oggettiStampa
                    objBookmark.Descrizione = "anno_imposta_1"
                    objBookmark.Valore = strANNO
                    oArrBookmark.Add(objBookmark)

                    objBookmark = New oggettiStampa
                    objBookmark.Descrizione = "n_provvedimento"
                    objBookmark.Valore = objUtility.CToStr(myRowDett("NUMERO_ATTO"))
                    oArrBookmark.Add(objBookmark)

                    objBookmark = New oggettiStampa
                    objBookmark.Descrizione = "data_provvedimento"
                    objBookmark.Valore = objUtility.GiraDataFromDB(myRowDett("DATA_ELABORAZIONE"))
                    oArrBookmark.Add(objBookmark)

                    objBookmark = New oggettiStampa
                    objBookmark.Descrizione = "anno_imposta"
                    objBookmark.Valore = strANNO 'myrowdett("ANNO")
                    oArrBookmark.Add(objBookmark)

                    UNISCI_IMMOBILI_ACCERTAMENTI = CBool(ConfigurationManager.AppSettings("UNISCI_IMMOBILI_ACCERTAMENTI"))
                    If UNISCI_IMMOBILI_ACCERTAMENTI Then
                        dsImmobiliDichAcc = objCOMACCERT.getImmobiliDichAccPerStampaAccertamentiTARSU(ConstSession.StringConnection, objHashTable, IDPROVVEDIMENTO)
                        If myRowStampa("tipocalcolo") = RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRuolo.TipoCalcolo.TARES Then
                            strRiga = FillBookMarkDICHACC_TARES(ConstSession.StringConnection, dsImmobiliDichAcc, strANNO)
                        Else
                            strRiga = FillBookMarkDICHACCtarsu(ConstSession.StringConnection, dsImmobiliDichAcc, strANNO, Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))
                        End If
                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "elenco_immobili"
                        objBookmark.Valore = strRiga
                        oArrBookmark.Add(objBookmark)
                    Else
                        '************************************************************************************
                        'ELENCO IMMOBILI DICHIARATI
                        '************************************************************************************
                        dsImmobiliDichiarato = objCOMACCERT.getImmobiliDichiaratiPerStampaAccertamentiTARSU(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable, IDPROVVEDIMENTO)
                        strRiga = FillBookMarkDICHIARATO_TARSU(ConstSession.StringConnection, dsImmobiliDichiarato, Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))

                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "elenco_immobili_dich"
                        objBookmark.Valore = "Dettaglio Immobili Dichiarati" & vbCrLf & strRiga
                        oArrBookmark.Add(objBookmark)

                        '************************************************************************************
                        'ELENCO IMMOBILI ACCERATI
                        '************************************************************************************
                        dsImmobiliAccertati = objCOMACCERT.getImmobiliAccertatiPerStampaAccertamentiTARSU(ConstSession.StringConnection, objHashTable, IDPROVVEDIMENTO)
                        strRiga = FillBookMarkACCERTATOtarsu(dsImmobiliAccertati)

                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "elenco_immobili_acce"
                        objBookmark.Valore = "Dettaglio Immobili Accertati" & vbCrLf & strRiga
                        oArrBookmark.Add(objBookmark)
                    End If

                    ''************************************************************************************
                    ''ELENCO VERSAMENTI
                    ''************************************************************************************
                    '[DETTAGLIO PAGAMENTI] 
                    'ANNO	DATA PAGAMENTO	IMPORTO PAGATO
                    objHashTable.Add("ANNO", strANNO)

                    dsVersamenti = objCOMACCERT.getVersamentiPerStampaAccertamentiTARSU(ConstSession.StringConnection, objHashTable, IDPROVVEDIMENTO)
                    strRiga = FillBookMarkVersamentiTARSU(dsVersamenti)

                    objBookmark = New oggettiStampa
                    objBookmark.Descrizione = "elenco_versamenti"
                    objBookmark.Valore = "Dettaglio Versamenti" & vbCrLf & strRiga
                    oArrBookmark.Add(objBookmark)

                    '************************************************************************************
                    'ELENCO INTERESSI CONFIGURATI
                    '************************************************************************************
                    objHashTable.Add("CODTIPOINTERESSE", "5")
                    objHashTable.Add("DAL", "")
                    objHashTable.Add("AL", "")
                    objHashTable.Add("TASSO", "")
                    objHashTable.Add("CODTRIBUTO", sTributo)
                    objHashTable.Add("riducibile", "0")

                    objDSTipiInteressiL = objCOM.GetElencoInteressiPerStampaLiquidazione(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable, IDPROVVEDIMENTO)
                    objDSTipiInteressi = objCOMACCERT.GetElencoInteressiPerStampaAccertamenti(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable, IDPROVVEDIMENTO)
                    objBookmark = New oggettiStampa
                    objBookmark.Descrizione = "elenco_interessi"
                    objBookmark.Valore = FillBookMarkELENCOINTERESSI(objDSTipiInteressiL, objDSTipiInteressi, sTributo)
                    oArrBookmark.Add(objBookmark)

                    ''************************************************************************************
                    ''ELENCO SANZIONI APPLICATE CON IMPORTO 
                    ''************************************************************************************
                    objDSElencoSanzioni = objCOMACCERT.GetElencoSanzioniPerStampaAccertamenti(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable, IDPROVVEDIMENTO)
                    objDSElencoSanzioniF2 = objCOM.GetElencoSanzioniPerStampaLiquidazione(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable, IDPROVVEDIMENTO)
                    objBookmark = New oggettiStampa
                    objBookmark.Descrizione = "elenco_sanzioni"
                    objBookmark.Valore = FillBookMarkELENCOSANZIONITARSU(objDSElencoSanzioni, objDSElencoSanzioniF2, ImportoTotSanzioni)
                    oArrBookmark.Add(objBookmark)

                    objBookmark = New oggettiStampa
                    objBookmark.Descrizione = "Violazione"
                    objBookmark.Valore = FillBookMarkELENCOVIOLAZIONI(objDSElencoSanzioni)
                    oArrBookmark.Add(objBookmark)

                    '*****************************************************************************************
                    'importi
                    oArrBookmark = FillBookMarkImporti(oArrBookmark, myRowDett, objHashTable, IDPROVVEDIMENTO, ImportoTotAddizionali)
                    If oArrBookmark Is Nothing Then
                        Throw New Exception("Errore in popola importi")
                    End If
                    '******************************************************************************************
                    objTestataDOC = New oggettoTestata
                    objTestataDOC.Atto = "Documenti"
                    objTestataDOC.Dominio = "Provvedimenti"
                    objTestataDOC.Ente = ConstSession.DescrizioneEnte.Replace("Comune di ", "")
                    objTestataDOC.Filename = FileNameContrib & "_" & myRowDett("NUMERO_ATTO").ToString.Replace("/", "") & "_" & sFilenameDOC
                    ArrayBookMark = CType(oArrBookmark.ToArray(GetType(oggettiStampa)), oggettiStampa())

                    objToPrint = New oggettoDaStampareCompleto
                    objToPrint.TestataDOC = objTestataDOC
                    objToPrint.TestataDOT = objTestataDOT
                    objToPrint.Stampa = ArrayBookMark

                    oArrListOggettiDaStampare.Add(objToPrint)
                Next
            Next

            Dim GruppoDOC As GruppoDocumenti
            Dim GruppoDOCUMENTI As GruppoDocumenti()
            Dim ArrListGruppoDOC As New ArrayList

            Dim ArrOggCompleto As oggettoDaStampareCompleto()
            Dim objTestataGruppo As oggettoTestata

            GruppoDOC = New Stampa.oggetti.GruppoDocumenti
            ArrOggCompleto = Nothing
            objTestataGruppo = New Stampa.oggetti.oggettoTestata

            ArrOggCompleto = CType(oArrListOggettiDaStampare.ToArray(GetType(oggettoDaStampareCompleto)), oggettoDaStampareCompleto())

            GruppoDOC.OggettiDaStampare = ArrOggCompleto
            GruppoDOC.TestataGruppo = objTestataGruppo

            ArrListGruppoDOC.Add(GruppoDOC)

            GruppoDOCUMENTI = CType(ArrListGruppoDOC.ToArray(GetType(GruppoDocumenti)), GruppoDocumenti())
            PrintDoc(GruppoDOCUMENTI)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.STAMPA_ACCERTAMENTOTARSU.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
            Dim sScript As String = ""
            sScript += "GestAlert('a', 'danger', '', '', 'Errore in fase di stampa');"
            RegisterScript(sScript, Me.GetType())
        End Try
    End Sub
    '*** ***
    '*** 20130801 - accertamento OSAP ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strTipoDoc"></param>
    ''' <param name="bIsBozza"></param>
    Private Sub STAMPA_ACCERTAMENTOOSAP(ByVal strTipoDoc As String, ByVal bIsBozza As Boolean)
        'Dim objSessione As CreateSessione
        Try
            Dim sFilenameDOT, sFilenameDOC As String
            Dim objDSstampa As DataSet
            Dim objUtility As New MyUtility
            'contiene i dati anagrafici visualizzati nella grilgia della ricerca lettere
            objDSstampa = CType(Session("PROVVEDIMENTI_DA_STAMPARE"), DataSet)

            ' creo l'oggetto testata per l'oggetto da stampare
            'serve per indicare la posizione di salvataggio e il nome del file.
            Dim objTestataDOC As oggettoTestata
            Dim objTestataDOT As New oggettoTestata
            Dim FileNameContrib As String
            Dim objDSTipiInteressiL As New DataSet
            Dim IDPROVVEDIMENTO As String
            Dim strANNO As String

            ' COMPONGO IL PERCORSO DEL FILE DOT
            sFilenameDOT = "AccertamentoOSAP_" & ConstSession.IdEnte & ".doc"
            sFilenameDOC = "AccertamentoOSAP"

            objTestataDOT.Atto = "Template"
            objTestataDOT.Dominio = "Provvedimenti"
            objTestataDOT.Ente = ConstSession.DescrizioneEnte.Replace("Comune di ", "")
            objTestataDOT.Filename = sFilenameDOT

            Dim oArrBookmark As ArrayList
            Dim iCount As Integer

            Dim objBookmark As oggettiStampa
            Dim oArrListOggettiDaStampare As New ArrayList
            Dim objToPrint As oggettoDaStampareCompleto
            Dim ArrayBookMark As oggettiStampa()
            Dim iCodContrib As Integer

            Dim strRiga As String = ""
            Dim dsDatiProvv As New DataSet
            Dim dsImmobiliDichiarato As New DataSet
            Dim dsVersamenti As New DataSet
            Dim dsImmobiliAccertati As New DataSet
            Dim objDSTipiInteressi As New DataSet
            Dim objDSElencoSanzioni As New DataSet
            Dim objDSElencoSanzioniF2 As New DataSet
            '---------------------------------------
            'var per popolare i bookmark relativi 
            'alla sezione degli importi interessi
            Dim ImportoTotSanzioni As Double
            Dim objHashTable As Hashtable = New Hashtable
            objHashTable.Add("CodENTE", ConstSession.IdEnte)
            objHashTable.Add("CODENTE", ConstSession.IdEnte)
            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)

            Dim objCOM As IElaborazioneLiquidazioni = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneLiquidazioni), ConstSession.URLServiziLiquidazioni)
            Dim objCOMACCERT As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)

            Dim NomeDbOSAP As String = ConfigurationManager.AppSettings("NOME_DATABASE_OSAP").ToString()
            objHashTable.Add("NomeDbOSAP", NomeDbOSAP)

            For iCount = 0 To objDSstampa.Tables(0).Rows.Count - 1
                iCodContrib = objDSstampa.Tables(0).Rows(iCount)("COD_CONTRIBUENTE")


                oArrBookmark = New ArrayList

                IDPROVVEDIMENTO = objDSstampa.Tables(0).Rows(iCount)("ID_PROVVEDIMENTO")
                objHashTable.Add("COD_CONTRIBUENTE", iCodContrib)

                dsDatiProvv = objCOM.GetProvvedimentoPerStampaLiquidazione(ConstSession.StringConnection, objHashTable, IDPROVVEDIMENTO)

                strANNO = dsDatiProvv.Tables(0).Rows(iCount)("ANNO")

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "bozza"
                If bIsBozza = True Then
                    objBookmark.Valore = "BOZZA"
                Else
                    objBookmark.Valore = ""
                End If
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "nome_ente"
                objBookmark.Valore = CStr(Session("DESCRIZIONE_ENTE")).ToUpper
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "TipoProvvedimento"
                objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("TIPO_PROVVEDIMENTO")).ToUpper()
                oArrBookmark.Add(objBookmark)

                '************************************************************************************
                'DATI ANAGRAFICI
                '************************************************************************************
                oArrBookmark = FillBookMarkAnagrafica(oArrBookmark, iCodContrib, Utility.Costanti.TRIBUTO_OSAP, FileNameContrib)
                If oArrBookmark Is Nothing Then
                    Throw New Exception("errore in popolamento anagrafica")
                End If
                '---------------------------------------------------------------------------------
                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "anno_imposta"
                objBookmark.Valore = strANNO
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "anno_imposta_1"
                objBookmark.Valore = strANNO
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "n_provvedimento"
                objBookmark.Valore = objUtility.CToStr(dsDatiProvv.Tables(0).Rows(iCount)("NUMERO_ATTO"))

                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "data_provvedimento"
                objBookmark.Valore = objUtility.GiraDataFromDB(dsDatiProvv.Tables(0).Rows(iCount)("DATA_ELABORAZIONE"))
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "anno_imposta"
                objBookmark.Valore = strANNO
                oArrBookmark.Add(objBookmark)

                '************************************************************************************
                'ELENCO IMMOBILI DICHIARATI
                '************************************************************************************
                dsImmobiliDichiarato = objCOMACCERT.getImmobiliDichAccPerStampaAccertamentiOSAP(ConstSession.StringConnection, "D", objHashTable, IDPROVVEDIMENTO)
                strRiga = FillBookMarkOSAPDichiaratoAccertato("D", dsImmobiliDichiarato)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "elenco_immobili_dich"
                objBookmark.Valore += strRiga
                oArrBookmark.Add(objBookmark)

                '************************************************************************************
                'ELENCO IMMOBILI ACCERATI
                '************************************************************************************
                dsImmobiliAccertati = objCOMACCERT.getImmobiliDichAccPerStampaAccertamentiOSAP(ConstSession.StringConnection, "A", objHashTable, IDPROVVEDIMENTO)
                strRiga = FillBookMarkOSAPDichiaratoAccertato("A", dsImmobiliAccertati)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "elenco_immobili_acce"
                objBookmark.Valore += strRiga
                oArrBookmark.Add(objBookmark)

                ''************************************************************************************
                ''ELENCO VERSAMENTI
                ''************************************************************************************
                '[DETTAGLIO PAGAMENTI] 
                'ANNO	DATA PAGAMENTO	IMPORTO PAGATO
                objHashTable.Add("ANNO", strANNO)

                dsVersamenti = objCOMACCERT.getVersamentiPerStampaAccertamentiOSAP(ConstSession.StringConnection, objHashTable, IDPROVVEDIMENTO)
                strRiga = FillBookMarkVersamentiTARSU(dsVersamenti)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "elenco_versamenti"
                objBookmark.Valore = "Dettaglio Versamenti" & vbCrLf & strRiga
                oArrBookmark.Add(objBookmark)

                '************************************************************************************
                'ELENCO INTERESSI CONFIGURATI
                '************************************************************************************
                objHashTable.Add("CODTIPOINTERESSE", "5")
                objHashTable.Add("DAL", "")
                objHashTable.Add("AL", "")
                objHashTable.Add("TASSO", "")
                objHashTable.Add("CODTRIBUTO", Session("COD_TRIBUTO"))
                objHashTable.Add("riducibile", "0")


                'M.B. 
                objDSTipiInteressiL = objCOM.GetElencoInteressiPerStampaLiquidazione(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable, IDPROVVEDIMENTO)
                objDSTipiInteressi = objCOMACCERT.GetElencoInteressiPerStampaAccertamenti(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable, IDPROVVEDIMENTO)
                strRiga = FillBookMarkELENCOINTERESSI(objDSTipiInteressiL, objDSTipiInteressi, Session("COD_TRIBUTO"))

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "elenco_interessi"
                objBookmark.Valore = strRiga
                oArrBookmark.Add(objBookmark)

                '************************************************************************************
                'ELENCO SANZIONI APPLICATE CON IMPORTO 
                '************************************************************************************
                objDSElencoSanzioni = objCOMACCERT.GetElencoSanzioniPerStampaAccertamenti(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable, IDPROVVEDIMENTO)
                objDSElencoSanzioniF2 = objCOM.GetElencoSanzioniPerStampaLiquidazione(ConstSession.StringConnection, ConstSession.IdEnte, objHashTable, IDPROVVEDIMENTO)
                strRiga = FillBookMarkELENCOSANZIONITARSU(objDSElencoSanzioni, objDSElencoSanzioniF2, ImportoTotSanzioni)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "elenco_sanzioni"
                objBookmark.Valore = strRiga
                oArrBookmark.Add(objBookmark)

                strRiga = FillBookMarkELENCOVIOLAZIONI(objDSElencoSanzioni)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "Violazione"
                objBookmark.Valore = strRiga
                oArrBookmark.Add(objBookmark)


                'Tassa	                 €     [SOMMA ALGEBRICA DELLE DIFF. DI IMPOSTA]
                'Sanzione Amministrativa €     [SOMMA ALGEBRICA DELLE SANZIONI]
                'Interessi 				 €     [SOMMA ALGEBRICA DEGLI INTERESSI]
                'Spese di notifica       €     [SPESE DI NOTIFICA]
                'Arrotondamento          €     [ARROTONDAMENTO]
                'Totale                  €     [IMPORTO TOTALE AVVISO]


                '************************************************************************************
                'IMPORTI (DIFF IMPOSTA - SANZIONI - INTERESSI - SPESE - ecc...
                '************************************************************************************
                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "imp_vers"
                objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("totale_versato")) & " €"
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "imp_dov"
                objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("totale_dichiarato")) & " €"
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "imp_acc"
                objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("totale_accertato")) & " €"
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "imposta_dovuta"
                'objBookmark.Valore = FormatNumberToZero(dsDatiProvv.Tables(0).Rows(iCount)("TOTALE_DICHIARATO")).Replace(",", ".") & " €"
                objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_DIFFERENZA_IMPOSTA")) & " €"
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "Importo_Sanzione"
                'objBookmark.Valore = FormatNumberToZero(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SANZIONI")).Replace(",", ".") & " €"
                objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SANZIONI")) & " €"
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "Importo_SanzNonRid"
                'objBookmark.Valore = FormatNumberToZero(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SANZIONI")).Replace(",", ".") & " €"
                objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_TOT_SANZIONI_NON_RIDUCIBILI")) & " €"
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "spese_notifica"
                'objBookmark.Valore = FormatNumberToZero(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SPESE")).Replace(",", ".") & " €"
                objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SPESE")) & " €"
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "Importo_arrotond"
                'objBookmark.Valore = FormatNumberToZero(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_ARROTONDAMENTO")).Replace(",", ".") & " €"
                objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_ARROTONDAMENTO")) & " €"
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "Importo_totale"
                'objBookmark.Valore = FormatNumberToZero(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_TOTALE")).Replace(",", ".") & " €"
                objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_TOTALE")) & " €"
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "Importo_totale_2"
                'objBookmark.Valore = FormatNumberToZero(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_TOTALE")).Replace(",", ".") & " €"
                objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_TOTALE")) & " €"
                oArrBookmark.Add(objBookmark)
                objBookmark.Descrizione = "ImpTotNonRidotto"
                objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_TOTALE"))
                oArrBookmark.Add(objBookmark)
                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "ImpDovuto"
                objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_TOTALE"))
                oArrBookmark.Add(objBookmark)
                '************************************************************************************
                'IMPORTI INTERESSI
                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "importo_interessi"
                objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_INTERESSI")) & " €"
                oArrBookmark.Add(objBookmark)
                '************************************************************************************

                '************************************************'
                'devo stampare importo totale con importo_sanzione_ridotto se importo_sanzione > importo_sanzione_ridotto

                'Tassa	                 €     [SOMMA ALGEBRICA DELLE DIFF. DI IMPOSTA]
                'Sanzione Amministrativa €     [SOMMA ALGEBRICA DELLE SANZIONI]
                'Interessi 				 €     [SOMMA ALGEBRICA DEGLI INTERESSI]
                'Spese di notifica       €     [SPESE DI NOTIFICA]
                'Arrotondamento          €     [ARROTONDAMENTO]
                'Totale                  €     [IMPORTO TOTALE AVVISO]


                '************************************************************************************
                'IMPORTI (DIFF IMPOSTA - SANZIONI - INTERESSI - SPESE - ecc...
                '************************************************************************************
                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "imposta_dovuta_1"
                objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_DIFFERENZA_IMPOSTA")) & " €"
                oArrBookmark.Add(objBookmark)

                If Not IsDBNull(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SANZIONI_RIDOTTO")) Then
                    'altrimenti stampo uguale'
                    If dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SANZIONI") > dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SANZIONI_RIDOTTO") Then
                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "Importo_Sanzione_1"
                        objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SANZIONI_RIDOTTO")) & " €"
                        oArrBookmark.Add(objBookmark)
                    Else
                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "Importo_Sanzione_1"
                        objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SANZIONI_RIDOTTO")) & " €"
                        oArrBookmark.Add(objBookmark)
                    End If
                Else
                    objBookmark = New oggettiStampa
                    objBookmark.Descrizione = "Importo_Sanzione_1"
                    objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SANZIONI_RIDOTTO")) & " €"
                    oArrBookmark.Add(objBookmark)
                End If

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "spese_notifica_1"
                objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SPESE")) & " €"
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "Importo_arrotond_1"
                objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_ARROTONDAMENTO_RIDOTTO")) & " €"
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "importo_interessi_1"
                objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_INTERESSI")) & " €"
                oArrBookmark.Add(objBookmark)

                'altrimenti stampo uguale'
                If Not IsDBNull(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SANZIONI_RIDOTTO")) Then
                    If dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SANZIONI") > dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SANZIONI_RIDOTTO") Then
                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "Importo_totale_1"
                        objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_TOTALE_RIDOTTO")) & " €"
                        oArrBookmark.Add(objBookmark)
                    Else
                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "Importo_totale_1"
                        objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_TOTALE_RIDOTTO")) & " €"
                        oArrBookmark.Add(objBookmark)
                    End If
                Else
                    objBookmark = New oggettiStampa
                    objBookmark.Descrizione = "Importo_totale_1"
                    objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_TOTALE_RIDOTTO")) & " €"
                    oArrBookmark.Add(objBookmark)
                End If

                objTestataDOC = New oggettoTestata
                objTestataDOC.Atto = "Documenti"
                objTestataDOC.Dominio = "Provvedimenti"
                objTestataDOC.Ente = ConstSession.DescrizioneEnte.Replace("Comune di ", "")
                objTestataDOC.Filename = FileNameContrib & "_" & dsDatiProvv.Tables(0).Rows(iCount)("NUMERO_ATTO").ToString.Replace("/", "") & "_" & sFilenameDOC
                ArrayBookMark = CType(oArrBookmark.ToArray(GetType(oggettiStampa)), oggettiStampa())

                objToPrint = New oggettoDaStampareCompleto
                objToPrint.TestataDOC = objTestataDOC
                objToPrint.TestataDOT = objTestataDOT
                objToPrint.Stampa = ArrayBookMark

                oArrListOggettiDaStampare.Add(objToPrint)
            Next

            Dim GruppoDOC As GruppoDocumenti
            Dim GruppoDOCUMENTI As GruppoDocumenti()
            Dim ArrListGruppoDOC As New ArrayList

            Dim ArrOggCompleto As oggettoDaStampareCompleto()
            Dim objTestataGruppo As oggettoTestata

            GruppoDOC = New Stampa.oggetti.GruppoDocumenti
            ArrOggCompleto = Nothing
            objTestataGruppo = New Stampa.oggetti.oggettoTestata

            ArrOggCompleto = CType(oArrListOggettiDaStampare.ToArray(GetType(oggettoDaStampareCompleto)), oggettoDaStampareCompleto())

            GruppoDOC.OggettiDaStampare = ArrOggCompleto
            GruppoDOC.TestataGruppo = objTestataGruppo

            ArrListGruppoDOC.Add(GruppoDOC)

            GruppoDOCUMENTI = CType(ArrListGruppoDOC.ToArray(GetType(GruppoDocumenti)), GruppoDocumenti())
            PrintDoc(GruppoDOCUMENTI)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.STAMPA_ACCERTAMENTOSAP.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
            Dim sScript As String = ""
            sScript += "GestAlert('a', 'danger', '', '', 'Errore in fase di stampa');"
            RegisterScript(sScript, Me.GetType())
        Finally
            'If Not IsNothing(objSessione) Then
            '    objSessione.Kill()
            '    objSessione = Nothing
            'End If
        End Try
    End Sub
    '*** ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strTipoDoc"></param>
    ''' <param name="bIsBozza"></param>
    ''' <param name="Tributo"></param>
    Private Sub STAMPA_DocumentoICI(ByVal strTipoDoc As String, ByVal bIsBozza As Boolean, Tributo As String)
        Try
            Dim sFilenameDOT, sFilenameDOC As String
            Dim dsMyStampa As DataSet
            Dim objUtility As New MyUtility
            ' creo l'oggetto testata per l'oggetto da stampare serve per indicare la posizione di salvataggio e il nome del file.
            Dim myTestataDOC As oggettoTestata
            Dim myTestataDOT As New oggettoTestata
            Dim FileNameContrib As String
            Dim IDPROVVEDIMENTO As String
            Dim IDPROCEDIMENTO As String
            Dim sAnno As String
            Dim ListBookmark As New ArrayList
            Dim iCount As Integer
            Dim objBookmark As oggettiStampa
            Dim ListOggettiDaStampare As New ArrayList
            Dim objToPrint As oggettoDaStampareCompleto
            Dim iCodContrib As Integer
            Dim strRiga As String = ""
            Dim dsDatiProvv As New DataSet
            Dim dsImmobiliDichiarato As New DataSet
            Dim dsVersamenti As New DataSet
            Dim dsImmobiliAccertati As New DataSet
            Dim dsTipiInteressi As New DataSet
            Dim dsTipiInteressiL As New DataSet
            Dim dsElencoSanzioni As New DataSet
            Dim dsElencoSanzioniF2 As New DataSet
            Dim dsElencoSanzioniF2Intr As New DataSet
            Dim dsElencoSanzioniRiducibili As New DataSet
            Dim dsElencoSanzioniF2Riducibili As New DataSet
            Dim dsImportiInteressi As New DataSet
            Dim dsImportiInteressiF2 As New DataSet
            Dim strImportoGiorni As String
            Dim strImportoSemestriACC As String
            Dim strImportoSemestriSAL As String
            Dim strNumSemestriACC As String
            Dim strNumSemestriSAL As String
            Dim acconto, saldo As Double
            Dim ImpDov, ImpVers As Double
            Dim tipo_versamento As String = ""
            Dim impDiffImposta As Double = 0
            Dim impSanzioniNonRiducibili As Double = 0
            Dim impSanzioniRiducibili As Double = 0
            Dim impSanzioniRidotte As Double = 0
            Dim impInteressi As Double = 0
            Dim impSpeseNotifica As Double = 0
            Dim impArrotondamento As Double = 0
            Dim impTotale As Double = 0
            Dim impArrotondamentoRidotto As Double = 0
            Dim impTotaleRidotto As Double = 0
            Dim imp_dov_dich_acc, imp_dov_dich_saldo As Double
            Dim dvVers As DataView
            Dim dTot, dTotUS, dTotNoAS As Double
            Dim myHashTable As New Hashtable
            Dim objCOM As IElaborazioneLiquidazioni = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneLiquidazioni), ConstSession.URLServiziLiquidazioni)
            Dim objCOMACCERT As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
            Dim objVers As New DichiarazioniICI.Database.VersamentiTable(ConstSession.UserName)

            'contiene i dati anagrafici visualizzati nella grilgia della ricerca lettere
            dsMyStampa = CType(Session("PROVVEDIMENTI_DA_STAMPARE"), DataSet)
            ' COMPONGO IL PERCORSO DEL FILE DOT
            If strTipoDoc = CostantiProvv.DOCUMENTO_RIMBORSO_ICI Then
                sFilenameDOT = "Rimborso"
            ElseIf strTipoDoc = CostantiProvv.DOCUMENTO_ANNULLAMENTO Then
                sFilenameDOT = "Annullamento"
            Else
                sFilenameDOT = "Accertamento"
            End If
            If Tributo = Utility.Costanti.TRIBUTO_ICI Then
                sFilenameDOT += "ICI"
            Else
                sFilenameDOT += "TASI"
            End If
            sFilenameDOT += ".doc"
            sFilenameDOC = sFilenameDOT.Replace(".doc", "")

            myTestataDOT.Atto = "Template"
            myTestataDOT.Dominio = "Provvedimenti"
            myTestataDOT.Ente = ConstSession.DescrizioneEnte.Replace("Comune di ", "")
            myTestataDOT.Filename = sFilenameDOT

            myHashTable.Add("CodENTE", ConstSession.IdEnte)
            myHashTable.Add("CODENTE", ConstSession.IdEnte)
            myHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)
            myHashTable.Add("NomeDbOpenGov", ConfigurationManager.AppSettings("NOME_DATABASE_OPENGOV").ToString())

            For iCount = 0 To dsMyStampa.Tables(0).Rows.Count - 1
                iCodContrib = dsMyStampa.Tables(0).Rows(iCount)("COD_CONTRIBUENTE")
                IDPROVVEDIMENTO = dsMyStampa.Tables(0).Rows(iCount)("ID_PROVVEDIMENTO")
                myHashTable.Add("COD_CONTRIBUENTE", iCodContrib)

                dsDatiProvv = objCOM.GetProvvedimentoPerStampaLiquidazione(ConstSession.StringConnection, myHashTable, IDPROVVEDIMENTO)
                For Each myRow As DataRow In dsDatiProvv.Tables(0).Rows
                    IDPROCEDIMENTO = myRow("ID_PROCEDIMENTO")
                    sAnno = myRow("ANNO")
                    impDiffImposta = myRow("IMPORTO_DIFFERENZA_IMPOSTA")
                    impSanzioniNonRiducibili = myRow("IMPORTO_SANZIONI_NON_RIDUCIBILI")
                    impSanzioniRiducibili = (myRow("IMPORTO_SANZIONI") - myRow("IMPORTO_SANZIONI_NON_RIDUCIBILI")).ToString
                    impSanzioniRidotte = myRow("IMPORTO_SANZIONI_RIDOTTO")
                    impSpeseNotifica = myRow("IMPORTO_SPESE")
                    impArrotondamento = myRow("IMPORTO_ARROTONDAMENTO")
                    impInteressi = myRow("IMPORTO_INTERESSI")
                    impTotale = FormatImport(myRow("IMPORTO_TOTALE"))
                    impTotaleRidotto = myRow("IMPORTO_TOTALE_RIDOTTO")
                    impArrotondamentoRidotto = myRow("IMPORTO_ARROTONDAMENTO_RIDOTTO")

                    myHashTable.Add("ANNO", sAnno)
                    '*** 20140701 - IMU/TARES ***
                    If sAnno >= 2012 Then
                        sFilenameDOT = sFilenameDOT.Replace("ICI", "IMU")
                        sFilenameDOC = sFilenameDOC.Replace("ICI", "IMU")
                        myTestataDOT.Filename = sFilenameDOT
                        Log.Debug(sFilenameDOT & "::" & sFilenameDOC)
                    End If
                    '*** ***

                    ListBookmark = New ArrayList

                    objBookmark = New oggettiStampa
                    objBookmark.Descrizione = "bozza"
                    If bIsBozza = True Then
                        objBookmark.Valore = "BOZZA"
                    Else
                        objBookmark.Valore = ""
                    End If
                    ListBookmark.Add(objBookmark)

                    For x As Integer = 0 To 9
                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "nome_ente" & Replace(x.ToString(), "0", "")
                        objBookmark.Valore = ConstSession.DescrizioneEnte.ToUpper
                        ListBookmark.Add(objBookmark)
                    Next
                    For x As Integer = 0 To 9
                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "TipoProvvedimento" & Replace(x.ToString(), "0", "")
                        objBookmark.Valore = FormatStringToEmpty(myRow("TIPO_PROVVEDIMENTO")).ToUpper()
                        ListBookmark.Add(objBookmark)
                    Next

                    '************************************************************************************
                    'DATI ANAGRAFICI
                    '************************************************************************************
                    ListBookmark = FillBookMarkAnagrafica(ListBookmark, iCodContrib, Utility.Costanti.TRIBUTO_ICI, FileNameContrib)
                    If ListBookmark Is Nothing Then
                        Throw New Exception("errore in popolamento anagrafica")
                    End If
                    '---------------------------------------------------------------------------------
                    For x As Integer = 0 To 9
                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "anno_ici" & Replace(x.ToString(), "0", "")
                        objBookmark.Valore = sAnno
                        ListBookmark.Add(objBookmark)
                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "anno_imposta" & Replace(x.ToString(), "0", "")
                        objBookmark.Valore = sAnno
                        ListBookmark.Add(objBookmark)
                    Next
                    For x As Integer = 0 To 9
                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "n_provvedimento" & Replace(x.ToString(), "0", "")
                        objBookmark.Valore = objUtility.CToStr(myRow("NUMERO_ATTO"))
                        ListBookmark.Add(objBookmark)
                    Next
                    For x As Integer = 0 To 9
                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "data_provvedimento" & Replace(x.ToString(), "0", "")
                        If myRow("DATA_CONFERMA") Is System.DBNull.Value Then
                            objBookmark.Valore = ""
                        Else
                            objBookmark.Valore = objUtility.GiraDataFromDB(myRow("DATA_CONFERMA"))
                        End If
                        ListBookmark.Add(objBookmark)
                    Next

                    '************************************************************************************
                    'ELENCO IMMOBILI DICHIARATI
                    '************************************************************************************
                    dsImmobiliDichiarato = objCOMACCERT.getImmobiliDichiaratiPerStampaAccertamenti(ConstSession.StringConnection, myHashTable, IDPROCEDIMENTO)
                    '*** 20130620 - richieste comune ***
                    strRiga = FillBookMarkACCERTATO(dsImmobiliDichiarato, sAnno, Tributo)
                    '*** ***
                    objBookmark = New oggettiStampa
                    objBookmark.Descrizione = "elenco_immobili"
                    objBookmark.Valore = strRiga
                    ListBookmark.Add(objBookmark)
                    '************************************************************************************
                    'ELENCO IMMOBILI ACCERATI
                    '************************************************************************************
                    dsImmobiliAccertati = objCOMACCERT.getImmobiliAccertatiPerStampaAccertamenti(ConstSession.StringConnection, myHashTable, IDPROCEDIMENTO)
                    strRiga = FillBookMarkACCERTATO(dsImmobiliAccertati, sAnno, Tributo)
                    objBookmark = New oggettiStampa
                    objBookmark.Descrizione = "elenco_immobili_acce"
                    objBookmark.Valore = strRiga
                    ListBookmark.Add(objBookmark)
                    '************************************************************************************
                    'ELENCO VERSAMENTI
                    '************************************************************************************
                    dsVersamenti = objCOM.GetVersamentiPerStampaLiquidazione(ConstSession.StringConnection, ConstSession.IdEnte, myHashTable, IDPROCEDIMENTO, OggettoAtto.Fase.VersamentiTardivi)
                    strRiga = FillBookMarkVersamenti(dsVersamenti)
                    objBookmark = New oggettiStampa
                    objBookmark.Descrizione = "elenco_versamenti"
                    objBookmark.Valore = "Dettaglio Versamenti" & vbCrLf & strRiga
                    ListBookmark.Add(objBookmark)

                    strRiga = FillBookMarkIMPORTODOVACC(dsImmobiliAccertati, acconto, saldo)

                    For x As Integer = 0 To 9
                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "imp_dov_acc" & Replace(x.ToString(), "0", "")
                        objBookmark.Valore = FormatImport(acconto) & " €"
                        ListBookmark.Add(objBookmark)
                    Next
                    For x As Integer = 0 To 9
                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "imp_dov_saldo" & Replace(x.ToString(), "0", "")
                        objBookmark.Valore = FormatImport(saldo) & " €"
                        ListBookmark.Add(objBookmark)
                    Next
                    '************************************************************************************
                    'ELENCO INTERESSI CONFIGURATI
                    '************************************************************************************
                    myHashTable.Add("CODTRIBUTO", Utility.Costanti.TRIBUTO_ICI)
                    dsTipiInteressiL = objCOM.GetElencoInteressiPerStampaLiquidazione(ConstSession.StringConnection, ConstSession.IdEnte, myHashTable, IDPROVVEDIMENTO)
                    dsTipiInteressi = objCOMACCERT.GetElencoInteressiPerStampaAccertamenti(ConstSession.StringConnection, ConstSession.IdEnte, myHashTable, IDPROVVEDIMENTO)
                    strRiga = FillBookMarkELENCOINTERESSI(dsTipiInteressiL, dsTipiInteressi, Utility.Costanti.TRIBUTO_ICI)
                    strRiga += FillBookMarkTOTALEINTERESSI(dsTipiInteressiL, dsTipiInteressi)
                    objBookmark = New oggettiStampa
                    objBookmark.Descrizione = "elenco_interessi"
                    objBookmark.Valore = strRiga
                    ListBookmark.Add(objBookmark)

                    '************************************************************************************
                    'ELENCO SANZIONI APPLICATE CON IMPORTO 
                    '************************************************************************************
                    myHashTable.Add("riducibile", 0)
                    'Sanzioni NON Riducibili
                    dsElencoSanzioni = objCOMACCERT.GetElencoSanzioniPerStampaAccertamenti(ConstSession.StringConnection, ConstSession.IdEnte, myHashTable, IDPROVVEDIMENTO)
                    dsElencoSanzioniF2 = objCOM.GetElencoSanzioniPerStampaLiquidazione(ConstSession.StringConnection, ConstSession.IdEnte, myHashTable, IDPROVVEDIMENTO)
                    '*************************
                    'Sanzioni Riducibili
                    myHashTable.Remove("riducibile")
                    myHashTable.Add("riducibile", 1)
                    dsElencoSanzioniRiducibili = objCOMACCERT.GetElencoSanzioniPerStampaAccertamenti(ConstSession.StringConnection, ConstSession.IdEnte, myHashTable, IDPROVVEDIMENTO)
                    dsElencoSanzioniF2Riducibili = objCOM.GetElencoSanzioniPerStampaLiquidazione(ConstSession.StringConnection, ConstSession.IdEnte, myHashTable, IDPROVVEDIMENTO)
                    '*************************
                    'Sanzioni Intrasmissibilità agli eredi 
                    myHashTable.Remove("riducibile")
                    myHashTable.Add("riducibile", "")
                    dsElencoSanzioniF2Intr = objCOM.GetElencoSanzioniPerStampaLiquidazione(ConstSession.StringConnection, ConstSession.IdEnte, myHashTable, IDPROVVEDIMENTO)
                    '*************************
                    strRiga = FillBookMarkELENCOSANZIONI(dsElencoSanzioni, dsElencoSanzioniF2, dsElencoSanzioniRiducibili, dsElencoSanzioniF2Riducibili, dsElencoSanzioniF2Intr)
                    objBookmark = New oggettiStampa
                    objBookmark.Descrizione = "elenco_sanzioni"
                    objBookmark.Valore = strRiga
                    ListBookmark.Add(objBookmark)
                    '************************************************************************************
                    'IMPORTI (DIFF IMPOSTA - SANZIONI - INTERESSI - SPESE - ecc...
                    '************************************************************************************
                    For x As Integer = 0 To 9
                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "imposta_dovuta" & Replace(x.ToString(), "0", "")
                        objBookmark.Valore = FormatImport(myRow("TOTALE_DICHIARATO")) & " €"
                        ListBookmark.Add(objBookmark)
                    Next
                    '*** 20130121 - devo stampare il dovuto accertato altrimenti non tornano i conti nel prospetto ***
                    For x As Integer = 0 To 9
                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "imposta_dovuta_accer" & Replace(x.ToString(), "0", "")
                        objBookmark.Valore = FormatImport(myRow("IMPORTO_ACCERTATO_ACC")) & " €"
                        ListBookmark.Add(objBookmark)
                    Next
                    '*** ***
                    For x As Integer = 0 To 9
                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "imposta_versata" & Replace(x.ToString(), "0", "")
                        objBookmark.Valore = FormatImport(myRow("TOTALE_VERSATO")) & " €"
                        ListBookmark.Add(objBookmark)
                    Next

                    ImpDov = myRow("TOTALE_DICHIARATO")
                    ImpVers = myRow("TOTALE_VERSATO")
                    If ImpDov > 0 And (ImpVers > 0 And ImpVers < ImpDov) Then
                        tipo_versamento = "parziale"
                    ElseIf ImpDov > 0 And ImpVers = 0 Then
                        tipo_versamento = "omesso"
                    End If
                    For x As Integer = 0 To 9
                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "tipo_versamento" & Replace(x.ToString(), "0", "")
                        objBookmark.Valore = tipo_versamento
                        ListBookmark.Add(objBookmark)
                    Next
                    For x As Integer = 0 To 9
                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "ImpostaAccertata" & Replace(x.ToString(), "0", "")
                        objBookmark.Valore = FormatImport(myRow("TOTALE_ACCERTATO")) & " €"
                        ListBookmark.Add(objBookmark)
                    Next
                    For x As Integer = 0 To 9
                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "ImpostaAccertata_60g" & Replace(x.ToString(), "0", "")
                        objBookmark.Valore = FormatImport(myRow("TOTALE_ACCERTATO")) & " €"
                        ListBookmark.Add(objBookmark)
                    Next
                    For x As Integer = 0 To 9
                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "DiffImpostaDaVersare" & Replace(x.ToString(), "0", "")
                        objBookmark.Valore = FormatImport(impDiffImposta) & " €"
                        ListBookmark.Add(objBookmark)
                    Next
                    For x As Integer = 0 To 9
                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "DiffImpostaDaVer_60g" & Replace(x.ToString(), "0", "")
                        objBookmark.Valore = FormatImport(impDiffImposta) & " €"
                        ListBookmark.Add(objBookmark)
                    Next
                    '********************************************************************************
                    'GESTIONE IMPORTI SANZIONI 
                    '********************************************************************************
                    For x As Integer = 0 To 9
                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "ImportoSanzioneRid" & Replace(x.ToString(), "0", "")
                        objBookmark.Valore = FormatImport(impSanzioniRiducibili) & " €"
                        ListBookmark.Add(objBookmark)
                    Next
                    For x As Integer = 0 To 9
                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "ImportoSanzione" & Replace(x.ToString(), "0", "")
                        objBookmark.Valore = FormatImport(impSanzioniNonRiducibili) & " €"
                        ListBookmark.Add(objBookmark)
                    Next
                    For x As Integer = 0 To 9
                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "ImportoSanzione_60g" & Replace(x.ToString(), "0", "")
                        objBookmark.Valore = FormatImport(impSanzioniNonRiducibili) & " €"
                        ListBookmark.Add(objBookmark)
                    Next
                    For x As Integer = 0 To 9
                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "ImpSanzioneRid_60g" & Replace(x.ToString(), "0", "")
                        objBookmark.Valore = FormatImport(impSanzioniRidotte) & " €"
                        ListBookmark.Add(objBookmark)
                    Next
                    For x As Integer = 0 To 9
                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "spese_notifica" & Replace(x.ToString(), "0", "")
                        objBookmark.Valore = FormatImport(impSpeseNotifica) & " €"
                        ListBookmark.Add(objBookmark)
                    Next
                    For x As Integer = 0 To 9
                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "spese_notifica_60g" & Replace(x.ToString(), "0", "")
                        objBookmark.Valore = FormatImport(impSpeseNotifica) & " €"
                        ListBookmark.Add(objBookmark)
                    Next
                    For x As Integer = 0 To 9
                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "Importo_arrotond" & Replace(x.ToString(), "0", "")
                        objBookmark.Valore = FormatImport(impArrotondamento) & " €"
                        ListBookmark.Add(objBookmark)
                    Next

                    '************************************************************************************
                    'IMPORTI INTERESSI
                    '************************************************************************************
                    dsImportiInteressi = objCOMACCERT.GetInteressiTotaliPerStampaAccertamenti(ConstSession.StringConnection, ConstSession.IdEnte, myHashTable, IDPROVVEDIMENTO)
                    dsImportiInteressiF2 = objCOM.GetInteressiTotaliPerStampaLiquidazione(ConstSession.StringConnection, ConstSession.IdEnte, myHashTable, IDPROVVEDIMENTO)
                    If FillBookMarkIMPORTIINTERESSI(dsImportiInteressi, dsImportiInteressiF2, strImportoGiorni, strImportoSemestriACC, strImportoSemestriSAL, strNumSemestriACC, strNumSemestriSAL) = True Then
                        For x As Integer = 0 To 9
                            objBookmark = New oggettiStampa
                            objBookmark.Descrizione = "imp_interessi_GIORNI" & Replace(x.ToString(), "0", "")
                            objBookmark.Valore = strImportoGiorni & " €"
                            ListBookmark.Add(objBookmark)
                        Next
                        For x As Integer = 0 To 9
                            objBookmark = New oggettiStampa
                            objBookmark.Descrizione = "imp_semestri_ACCONTO" & Replace(x.ToString(), "0", "")
                            objBookmark.Valore = strImportoSemestriACC & " €"
                            ListBookmark.Add(objBookmark)
                        Next
                        For x As Integer = 0 To 9
                            objBookmark = New oggettiStampa
                            objBookmark.Descrizione = "num_semestri_ACCONTO" & Replace(x.ToString(), "0", "")
                            objBookmark.Valore = strNumSemestriACC
                            ListBookmark.Add(objBookmark)
                        Next
                        For x As Integer = 0 To 9
                            objBookmark = New oggettiStampa
                            objBookmark.Descrizione = "imp_semestri_SALDO" & Replace(x.ToString(), "0", "")
                            objBookmark.Valore = strImportoSemestriSAL & " €"
                            ListBookmark.Add(objBookmark)
                        Next
                        For x As Integer = 0 To 9
                            objBookmark = New oggettiStampa
                            objBookmark.Descrizione = "num_semestri_SALDO" & Replace(x.ToString(), "0", "")
                            objBookmark.Valore = strNumSemestriSAL
                            ListBookmark.Add(objBookmark)
                        Next
                        For x As Integer = 0 To 9
                            objBookmark = New oggettiStampa
                            objBookmark.Descrizione = "int_mor" & Replace(x.ToString(), "0", "")
                            objBookmark.Valore = FormatImport(impInteressi) & " €"
                            ListBookmark.Add(objBookmark)
                        Next
                        For x As Integer = 0 To 9
                            objBookmark = New oggettiStampa
                            objBookmark.Descrizione = "int_mor_60g" & Replace(x.ToString(), "0", "")
                            objBookmark.Valore = FormatImport(impInteressi) & " €"
                            ListBookmark.Add(objBookmark)
                        Next
                    End If
                    '************************************************************************************
                    'TOTALI
                    '************************************************************************************
                    For x As Integer = 0 To 9
                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "Importo_totale" & Replace(x.ToString(), "0", "")
                        objBookmark.Valore = FormatImport(impTotale) & " €"
                        ListBookmark.Add(objBookmark)
                    Next
                    For x As Integer = 0 To 9
                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "ImpTotNonRidotto" & Replace(x.ToString(), "0", "")
                        objBookmark.Valore = FormatImport(impTotale) & " €"
                        ListBookmark.Add(objBookmark)
                    Next
                    For x As Integer = 0 To 9
                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "Importo_totale_60g" & Replace(x.ToString(), "0", "")
                        objBookmark.Valore = FormatImport(impTotaleRidotto) & " €"
                        ListBookmark.Add(objBookmark)
                    Next
                    For x As Integer = 0 To 9
                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "Importo_arrotond_60g" & Replace(x.ToString(), "0", "")
                        objBookmark.Valore = FormatImport(impArrotondamentoRidotto) & " €"
                        ListBookmark.Add(objBookmark)
                    Next

                    '************************************************************************************
                    'IMPORTI DA DICHIARATO
                    '************************************************************************************
                    FillBookMarkIMP_DOV_DICH(dsImmobiliDichiarato, imp_dov_dich_acc, imp_dov_dich_saldo)
                    For x As Integer = 0 To 9
                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "imp_dov_dich_acc" & Replace(x.ToString(), "0", "")
                        objBookmark.Valore = FormatImport(imp_dov_dich_acc) & " €"
                        ListBookmark.Add(objBookmark)
                    Next
                    For x As Integer = 0 To 9
                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "imp_dov_dich_saldo" & Replace(x.ToString(), "0", "")
                        objBookmark.Valore = FormatImport(imp_dov_dich_saldo) & " €"
                        ListBookmark.Add(objBookmark)
                    Next
                    '************************************************************************************
                    'IMPORTI VERSATI
                    '************************************************************************************
                    'Senza Flag Acconto e saldo selezionati
                    dvVers = objVers.GetVersamentiPerTipologia(ConstSession.IdEnte, sAnno, iCodContrib, False, False, False, Tributo)
                    dTotNoAS = 0
                    For i As Integer = 0 To dvVers.Table.Rows.Count - 1
                        dTotNoAS += dvVers.Table.Rows(i)("ImportoPagato")
                    Next
                    'Unica soluzione
                    dvVers = objVers.GetVersamentiPerTipologia(ConstSession.IdEnte, sAnno, iCodContrib, True, True, False, Tributo)
                    dTotUS = 0
                    For i As Integer = 0 To dvVers.Table.Rows.Count - 1
                        dTotUS += dvVers.Table.Rows(i)("ImportoPagato")
                    Next
                    'Acconto
                    dvVers = objVers.GetVersamentiPerTipologia(ConstSession.IdEnte, sAnno, iCodContrib, True, False, False, Tributo)
                    dTot = dTotUS + dTotNoAS
                    For i As Integer = 0 To dvVers.Table.Rows.Count - 1
                        dTot += dvVers.Table.Rows(i)("ImportoPagato")
                    Next
                    For x As Integer = 0 To 9
                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "imp_vers_acc" & Replace(x.ToString(), "0", "")
                        objBookmark.Valore = FormatImport(dTot) & " €"
                        ListBookmark.Add(objBookmark)
                    Next
                    'saldo
                    dvVers = objVers.GetVersamentiPerTipologia(ConstSession.IdEnte, sAnno, iCodContrib, False, True, False, Tributo)
                    dTot = 0
                    For i As Integer = 0 To dvVers.Table.Rows.Count - 1
                        dTot += dvVers.Table.Rows(i)("ImportoPagato")
                    Next
                    For x As Integer = 0 To 9
                        objBookmark = New oggettiStampa
                        objBookmark.Descrizione = "imp_vers_saldo" & Replace(x.ToString(), "0", "")
                        objBookmark.Valore = FormatImport(dTot) & " €"
                        ListBookmark.Add(objBookmark)
                    Next

                    myTestataDOC = New oggettoTestata
                    myTestataDOC.Atto = "Documenti"
                    myTestataDOC.Dominio = "Provvedimenti"
                    myTestataDOC.Ente = ConstSession.DescrizioneEnte.Replace("Comune di ", "")
                    myTestataDOC.Filename = FileNameContrib & "_" & myRow("NUMERO_ATTO").ToString.Replace("/", "") & "_" & sFilenameDOC

                    objToPrint = New oggettoDaStampareCompleto
                    objToPrint.TestataDOC = myTestataDOC
                    objToPrint.TestataDOT = myTestataDOT
                    objToPrint.Stampa = CType(ListBookmark.ToArray(GetType(oggettiStampa)), oggettiStampa())
                    ListOggettiDaStampare.Add(objToPrint)
                Next
            Next

            Dim GruppoDOC As New GruppoDocumenti
            Dim GruppoDOCUMENTI As GruppoDocumenti()
            Dim ArrListGruppoDOC As New ArrayList

            Dim ArrOggCompleto As oggettoDaStampareCompleto()
            Dim objTestataGruppo As New oggettoTestata

            ArrOggCompleto = CType(ListOggettiDaStampare.ToArray(GetType(oggettoDaStampareCompleto)), oggettoDaStampareCompleto())

            GruppoDOC.OggettiDaStampare = ArrOggCompleto
            GruppoDOC.TestataGruppo = objTestataGruppo
            ArrListGruppoDOC.Add(GruppoDOC)

            GruppoDOCUMENTI = CType(ArrListGruppoDOC.ToArray(GetType(GruppoDocumenti)), GruppoDocumenti())
            PrintDoc(GruppoDOCUMENTI)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.STAMPA_ACCERTAMENTOICI.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
            Dim sScript As String = ""
            sScript += "GestAlert('a', 'danger', '', '', 'Errore in fase di stampa');"
            RegisterScript(sScript, Me.GetType())
        End Try
    End Sub
#End Region
#Region "FillBookmark"
    'Private Function FillBookMarkDICHIARATO(ByVal ds As DataSet, ByVal annoAcc As Integer) As String

    '    Dim strRiga As String=""
    '    Dim strImmoTemp As String = String.Empty
    '    Dim iImmobili As Integer
    '    Dim objUtility As New MyUtility

    '    Dim culture As IFormatProvider
    '    culture = New CultureInfo("it-IT", True)
    '    System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")

    '    'Dal  se inferiore all’anno di accertamento mettere 1/1/ dell’anno di accertamento
    '    'Al  se = a 31/12/9999 lasciare in bianco, se successivo all’anno di accertamento 
    '    'mettere(31 / 12 / dell) 'anno di accertamento, se c’è già una data chiusa riferibile 
    '    'all'anno di accertamento es accertiamo il 2007 è c’è una data di chiusura del 
    '    '30/10/2007 lasciare quella.

    '    'Inserire se abitazione principale con relativa detrazione e se pertinenza 

    '    'Cambiare Tipologia Immobile in Tipo Rendita/Valore e a fianco indicare Valore 
    '    'presunto o effettivo e non rendita perché mi pare di capire che viene stampato 
    '    'il valore e non la rendita        

    '    Try
    '        If ds.Tables(0).Rows.Count > 0 Then
    '            Dim blnConfigDichiarazione As Boolean = Boolean.Parse(ConfigurationManager.AppSettings("CONFIGURAZIONE_DICHIARAZIONE").ToString())
    '            strRiga = ""
    '            strRiga = strRiga.PadLeft(144, "-") & Microsoft.VisualBasic.Constants.vbCrLf
    '            strImmoTemp = strRiga

    '            Dim IDdichiarazione As Long
    '            Dim IDImmobile As Long

    '            For iImmobili = 0 To ds.Tables(0).Rows.Count - 1

    '                IDdichiarazione = ds.Tables(0).Rows(iImmobili)("IDDichiarazione")
    '                IDImmobile = ds.Tables(0).Rows(iImmobili)("IdOggetto")

    '                If blnConfigDichiarazione = False Then
    '                    'strImmoTemp = strImmoTemp & "Dal: " & objUtility.CToStr(ds.Tables(0).Rows(iImmobili)("DATAINIZIO")) & Microsoft.VisualBasic.Constants.vbTab
    '                    'strImmoTemp = strImmoTemp & "Al: " & objUtility.CToStr(ds.Tables(0).Rows(iImmobili)("DATAFINE")) & Microsoft.VisualBasic.Constants.vbCrLf

    '                    If (Year(ds.Tables(0).Rows(iImmobili)("DATAINIZIO")) < annoAcc) Then
    '                        strImmoTemp = strImmoTemp & "Dal: 01/01/" & annoAcc & Microsoft.VisualBasic.Constants.vbTab
    '                    Else
    '                        strImmoTemp = strImmoTemp & "Dal: " & objUtility.CToStr(ds.Tables(0).Rows(iImmobili)("DATAINIZIO")) & Microsoft.VisualBasic.Constants.vbTab
    '                    End If
    '                    If Not IsDBNull(ds.Tables(0).Rows(iImmobili)("DATAFINE")) Then
    '                        If (Year(ds.Tables(0).Rows(iImmobili)("DATAFINE")) = "9999") Then
    '                            strImmoTemp = strImmoTemp & "Al:"
    '                        ElseIf (Year(ds.Tables(0).Rows(iImmobili)("DATAFINE")) > annoAcc) Then
    '                            strImmoTemp = strImmoTemp & "Al: 31/12/" & annoAcc & Microsoft.VisualBasic.Constants.vbTab
    '                        Else
    '                            strImmoTemp = strImmoTemp & "Al: " & objUtility.CToStr(ds.Tables(0).Rows(iImmobili)("DATAFINE")) & Microsoft.VisualBasic.Constants.vbTab
    '                        End If
    '                    Else
    '                        strImmoTemp = strImmoTemp & "Al:"
    '                    End If

    '                    strImmoTemp = strImmoTemp & Microsoft.VisualBasic.Constants.vbCrLf

    '                Else
    '                    strImmoTemp = strImmoTemp & "Anno Dich.: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("ANNODICHIARAZIONE")) & Microsoft.VisualBasic.Constants.vbCrLf
    '                End If

    '                'If ds.Tables(0).Rows(iImmobili)("TipoImmobile") <> "0" Then
    '                'strImmoTemp = strImmoTemp & "Tipologia Immobile: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("DescrTipoImmobile")) & Microsoft.VisualBasic.Constants.vbCrLf
    '                strImmoTemp = strImmoTemp & "Tipo Rendita/Valore: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("DescrTipoImmobile")) & Microsoft.VisualBasic.Constants.vbCrLf
    '                'Else
    '                'strImmoTemp = strImmoTemp & "Tipologia Immobile:" & Microsoft.VisualBasic.Constants.vbCrLf
    '                'End If
    '                strImmoTemp = strImmoTemp & "Ubicazione: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("Via")) & " " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("NumeroCivico")) & Microsoft.VisualBasic.Constants.vbCrLf
    '                strImmoTemp = strImmoTemp & "Foglio: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("FOGLIO")) & Microsoft.VisualBasic.Constants.vbTab
    '                strImmoTemp = strImmoTemp & "Numero: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("NUMERO")) & Microsoft.VisualBasic.Constants.vbTab
    '                strImmoTemp = strImmoTemp & "Subalterno: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("SUBALTERNO")) & Microsoft.VisualBasic.Constants.vbTab
    '                strImmoTemp = strImmoTemp & "Categoria: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("CODCATEGORIACATASTALE")) & Microsoft.VisualBasic.Constants.vbTab
    '                strImmoTemp = strImmoTemp & "Classe: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("CODCLASSE")) & Microsoft.VisualBasic.Constants.vbCrLf
    '                If Not IsDBNull(ds.Tables(0).Rows(iImmobili)("rendita")) Then
    '                    strImmoTemp = strImmoTemp & "Rendita: " & FormatImport(Replace(ds.Tables(0).Rows(iImmobili)("rendita"), ".", ",")) & " €" & Microsoft.VisualBasic.Constants.vbTab
    '                Else
    '                    strImmoTemp = strImmoTemp & "Rendita: " & FormatImport("0") & " €" & Microsoft.VisualBasic.Constants.vbTab
    '                End If
    '                strImmoTemp = strImmoTemp & "Valore: " & FormatImport(Replace(ds.Tables(0).Rows(iImmobili)("ValoreImmobile"), ".", ",")) & " €" & Microsoft.VisualBasic.Constants.vbCrLf
    '                strImmoTemp = strImmoTemp & "Tipo Possesso: " & ds.Tables(0).Rows(iImmobili)("DescTipoPossesso") & Microsoft.VisualBasic.Constants.vbTab
    '                strImmoTemp = strImmoTemp & "Perc. Possesso: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("PERCPOSSESSO")).Replace(",", ".") & "%" & Microsoft.VisualBasic.Constants.vbCrLf
    '                '*** 20130620 - richieste comune ***
    '                If Not IsDBNull(ds.Tables(0).Rows(iImmobili)("ICI_VALORE_ALIQUOTA")) Then
    '                    strImmoTemp = strImmoTemp & "Aliquota: " & FormatImport(ds.Tables(0).Rows(iImmobili)("ICI_VALORE_ALIQUOTA")) & Microsoft.VisualBasic.Constants.vbTab
    '                End If
    '                '*** ***
    '                strImmoTemp = strImmoTemp & "Importo Dovuto: " & FormatImport(ds.Tables(0).Rows(iImmobili)("ICI_TOTALE_DOVUTA")) & " €"             ' & Microsoft.VisualBasic.Constants.vbTab

    '                If blnConfigDichiarazione = True Then
    '                    strImmoTemp = strImmoTemp & Microsoft.VisualBasic.Constants.vbTab & "Possesso 31/12: " & BoolToStringForGridView(ds.Tables(0).Rows(iImmobili)("Possesso").ToString())
    '                End If

    '                If Not IsDBNull(ds.Tables(0).Rows(iImmobili)("FLAG_PRINCIPALE")) Then
    '                    If ds.Tables(0).Rows(iImmobili)("FLAG_PRINCIPALE") = 1 Then
    '                        strImmoTemp = strImmoTemp & Microsoft.VisualBasic.Constants.vbCrLf
    '                        strImmoTemp = strImmoTemp & "Abitazione Principale" & Microsoft.VisualBasic.Constants.vbTab
    '                        If Not IsDBNull(ds.Tables(0).Rows(iImmobili)("ici_totale_detrazione_applicata")) Then
    '                            If ds.Tables(0).Rows(iImmobili)("ici_totale_detrazione_applicata") <> 0 Then
    '                                strImmoTemp = strImmoTemp & "Detrazione applicata: " & FormatImport(ds.Tables(0).Rows(iImmobili)("ici_totale_detrazione_applicata")) & " €"
    '                            End If
    '                        End If
    '                    ElseIf ds.Tables(0).Rows(iImmobili)("FLAG_PRINCIPALE") = 2 Then
    '                        strImmoTemp = strImmoTemp & Microsoft.VisualBasic.Constants.vbCrLf
    '                        strImmoTemp = strImmoTemp & "Pertinenza" & Microsoft.VisualBasic.Constants.vbTab
    '                    End If
    '                End If
    '                strImmoTemp = strImmoTemp & Microsoft.VisualBasic.Constants.vbCrLf
    '                If Not IsDBNull(ds.Tables(0).Rows(iImmobili)("DESCRTIPOTASI")) Then
    '                    If ds.Tables(0).Rows(iImmobili)("DESCRTIPOTASI") <> "" Then
    '                        strImmoTemp = strImmoTemp & ds.Tables(0).Rows(iImmobili)("DESCRTIPOTASI")
    '                        strImmoTemp = strImmoTemp & Microsoft.VisualBasic.Constants.vbCrLf
    '                    End If
    '                End If
    '                strImmoTemp = strImmoTemp & strRiga
    '            Next
    '        Else
    '            strImmoTemp = strRiga
    '            strImmoTemp = "Nessun Immobile dichiarato." & Microsoft.VisualBasic.Constants.vbCrLf
    '            strImmoTemp = strImmoTemp & strRiga
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.FillBookMarkDICHIARATO.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    End Try
    '    Return strImmoTemp
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ds"></param>
    ''' <param name="ACCONTO"></param>
    ''' <param name="SALDO"></param>
    ''' <returns></returns>
    Private Function FillBookMarkIMP_DOV_DICH(ByVal ds As DataSet, ByRef ACCONTO As Decimal, ByRef SALDO As Decimal) As Boolean
        Dim iImmobili As Integer

        ACCONTO = 0
        SALDO = 0
        Try
            If ds.Tables(0).Rows.Count > 0 Then
                For iImmobili = 0 To ds.Tables(0).Rows.Count - 1
                    If (Not IsDBNull(ds.Tables(0).Rows(iImmobili)("ICI_DOVUTA_ACCONTO"))) Then
                        ACCONTO += ds.Tables(0).Rows(iImmobili)("ICI_DOVUTA_ACCONTO")
                    End If
                    If (Not IsDBNull(ds.Tables(0).Rows(iImmobili)("ICI_DOVUTA_SALDO"))) Then
                        SALDO += ds.Tables(0).Rows(iImmobili)("ICI_DOVUTA_SALDO")
                    End If
                Next
                FillBookMarkIMP_DOV_DICH = True
            Else
                FillBookMarkIMP_DOV_DICH = False
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.FillBookMarkIMP_DOV_DICH.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try


    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myConnectionString"></param>
    ''' <param name="ds"></param>
    ''' <param name="sParametroENV"></param>
    ''' <param name="sUserName"></param>
    ''' <param name="sApplicazione"></param>
    ''' <returns></returns>
    Private Function FillBookMarkDICHIARATO_TARSU(ByVal myConnectionString As String, ByVal ds As DataSet, ByVal sParametroENV As String, ByVal sUserName As String, ByVal sApplicazione As String) As String

        Dim strRiga As String = ""
        Dim strImmoTemp As String = String.Empty
        Dim iImmobili As Integer
        Dim objUtility As New MyUtility

        Try

            If ds.Tables(0).Rows.Count > 0 Then
                Dim objAcc As ClsGestioneAccertamenti
                Dim drDatiRid As SqlClient.SqlDataReader

                strRiga = ""
                strRiga = strRiga.PadLeft(120, "-") & Microsoft.VisualBasic.Constants.vbCrLf
                strImmoTemp = strRiga

                For iImmobili = 0 To ds.Tables(0).Rows.Count - 1
                    strImmoTemp = strImmoTemp & Microsoft.VisualBasic.Constants.vbCrLf

                    strImmoTemp = strImmoTemp & "<b>Periodo</b>"
                    strImmoTemp = strImmoTemp & " dal " & objUtility.GiraDataFromDB(ds.Tables(0).Rows(iImmobili)("DATA_INIZIO"))
                    strImmoTemp = strImmoTemp & " al " & objUtility.GiraDataFromDB(ds.Tables(0).Rows(iImmobili)("DATA_FINE")) & Microsoft.VisualBasic.Constants.vbCrLf

                    strImmoTemp = strImmoTemp & "Ubicazione: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("Via")) & " " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("Civico")) & " " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("Interno")) & Microsoft.VisualBasic.Constants.vbCrLf

                    strImmoTemp = strImmoTemp & "Categoria: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("IDCATEGORIA")) & " - " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("descrizione")) & Microsoft.VisualBasic.Constants.vbCrLf
                    strImmoTemp = strImmoTemp & "Foglio: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("FOGLIO")) & Microsoft.VisualBasic.Constants.vbTab
                    strImmoTemp = strImmoTemp & "Numero: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("NUMERO")) & Microsoft.VisualBasic.Constants.vbTab
                    strImmoTemp = strImmoTemp & "Subalterno: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("SUBALTERNO")) & Microsoft.VisualBasic.Constants.vbCrLf

                    strImmoTemp = strImmoTemp & "Tariffa: " & CStr(FormatNumber(ds.Tables(0).Rows(iImmobili)("IMPORTO_TARIFFA"))) & Microsoft.VisualBasic.Constants.vbTab

                    strImmoTemp = strImmoTemp & "Mq: " & FormatImport(ds.Tables(0).Rows(iImmobili)("MQ")) & Microsoft.VisualBasic.Constants.vbTab
                    strImmoTemp = strImmoTemp & "Bimestri: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("bimestri")) & Microsoft.VisualBasic.Constants.vbTab
                    strImmoTemp = strImmoTemp & "Importo: " & FormatImport(ds.Tables(0).Rows(iImmobili)("IMPORTO_NETTO")) & " €" & Microsoft.VisualBasic.Constants.vbCrLf


                    '********* Riduzioni ********
                    drDatiRid = objAcc.GetRiduzioni(myConnectionString, ds.Tables(0).Rows(iImmobili)("IDENTE"), ds.Tables(0).Rows(iImmobili)("IDDETTAGLIOTESTATA"), ds.Tables(0).Rows(iImmobili)("ANNO"))
                    If Not IsNothing(drDatiRid) Then
                        strImmoTemp += "Riduzione Applicata:" & Microsoft.VisualBasic.Constants.vbCrLf
                        Do While drDatiRid.Read
                            strImmoTemp += drDatiRid("CODICE") & " " & drDatiRid("descrizione") & " " & drDatiRid("valore") & Microsoft.VisualBasic.Constants.vbCrLf
                        Loop
                    End If
                    '****************************

                    strImmoTemp = strImmoTemp & Microsoft.VisualBasic.Constants.vbCrLf
                    strImmoTemp = strImmoTemp & strRiga

                Next

            Else
                strImmoTemp = strRiga
                strImmoTemp = "Nessun Immobile dichiarato." & Microsoft.VisualBasic.Constants.vbCrLf
                strImmoTemp = strImmoTemp & strRiga
            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.FillBookMarkDICHIARATO_TARSU.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
        Return strImmoTemp
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myConnectionString"></param>
    ''' <param name="ds"></param>
    ''' <param name="Anno"></param>
    ''' <param name="sParametroENV"></param>
    ''' <param name="sUserName"></param>
    ''' <param name="sApplicazione"></param>
    ''' <returns></returns>
    Private Function FillBookMarkDICHACCtarsu(ByVal myConnectionString As String, ByVal ds As DataSet, ByVal Anno As String, ByVal sParametroENV As String, ByVal sUserName As String, ByVal sApplicazione As String) As String

        Dim strRiga, strRiga1 As String
        Dim strImmoTemp As String = String.Empty
        Dim iImmobili As Integer
        Dim tipo_imm As String = ""
        Dim id_legame, id_legame_old As Integer
        Dim iCount As Integer = 0

        Dim temp_Superficie, temp_Importo, Diff_Superficie, Diff_Importo As Double

        Dim objAcc As New ClsGestioneAccertamenti
        Dim drDatiRid As SqlClient.SqlDataReader

        id_legame = 0
        id_legame_old = 0
        Try
            If ds.Tables.Count > 0 Then

                If ds.Tables(0).Rows.Count > 0 Then
                    strRiga = ""
                    strRiga1 = ""
                    strRiga = strRiga.PadLeft(164, "-") & Microsoft.VisualBasic.Constants.vbCrLf
                    strRiga1 = strRiga1.PadLeft(90, "-") & Microsoft.VisualBasic.Constants.vbCrLf
                    strImmoTemp = "" 'strRiga
                    Dim vbtab As Object = Microsoft.VisualBasic.Constants.vbTab

                    For iImmobili = 0 To ds.Tables(0).Rows.Count - 1
                        tipo_imm = ds.Tables(0).Rows(iImmobili)("TIPO_IMM")
                        If IsDBNull(ds.Tables(0).Rows(iImmobili)("ID_LEGAME")) Then
                            id_legame = -1
                        Else
                            id_legame = ds.Tables(0).Rows(iImmobili)("ID_LEGAME")
                        End If
                        If id_legame <> id_legame_old Then
                            iCount += 1
                            strImmoTemp += iCount & ")" & Microsoft.VisualBasic.Constants.vbCrLf
                            'legame diverso
                            If tipo_imm = "D" Then
                                'Dichiarato
                                strImmoTemp += Microsoft.VisualBasic.Constants.vbCrLf
                                strImmoTemp += "DATI RISULTANTI DALLA DICHIARAZIONE:" & vbtab & vbtab & vbtab & vbtab
                                'Periodo
                                strImmoTemp += "Periodo"
                                If Not ds.Tables(0).Rows(iImmobili)("DATA_INIZIO") Is DBNull.Value Then
                                    strImmoTemp += " dal " & ds.Tables(0).Rows(iImmobili)("DATA_INIZIO")
                                Else
                                    strImmoTemp += " dal 01/01/" & Anno
                                End If
                                If Not ds.Tables(0).Rows(iImmobili)("DATA_FINE") Is DBNull.Value Then
                                    strImmoTemp += " al " & ds.Tables(0).Rows(iImmobili)("DATA_FINE")
                                Else
                                    strImmoTemp += " al 31/12/" & Anno
                                End If
                                strImmoTemp += Microsoft.VisualBasic.Constants.vbCrLf & Microsoft.VisualBasic.Constants.vbCrLf
                                'Indirizzo
                                strImmoTemp += vbtab & "Indirizzo: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("Via")) & " " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("Civico")) & " " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("Interno")) & Microsoft.VisualBasic.Constants.vbCrLf
                                'Categoria
                                strImmoTemp += vbtab & "Categoria: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("IDCATEGORIA")) & " - " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("descrizione")) & Microsoft.VisualBasic.Constants.vbCrLf
                                'Altri dati
                                strImmoTemp += vbtab & "Tariffa: " & CStr(FormatNumber(ds.Tables(0).Rows(iImmobili)("IMPORTO_TARIFFA"))) & " € " & vbtab
                                strImmoTemp += "Superficie: " & FormatImport(ds.Tables(0).Rows(iImmobili)("MQ")) & " Mq" & vbtab
                                strImmoTemp += "Bimestri: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("bimestri")) & vbtab
                                strImmoTemp += "Importo dovuto: " & FormatImport(ds.Tables(0).Rows(iImmobili)("IMPORTO_NETTO")) & " €" & Microsoft.VisualBasic.Constants.vbCrLf

                                temp_Superficie = ds.Tables(0).Rows(iImmobili)("MQ")
                                temp_Importo = ds.Tables(0).Rows(iImmobili)("IMPORTO_NETTO")

                                '********* Riduzioni ********
                                Try
                                    drDatiRid = objAcc.GetRiduzioni(myConnectionString, ds.Tables(0).Rows(iImmobili)("IDENTE"), ds.Tables(0).Rows(iImmobili)("IDDETTAGLIOTESTATA"), ds.Tables(0).Rows(iImmobili)("ANNO"))
                                    If Not IsNothing(drDatiRid) Then
                                        If drDatiRid.HasRows Then
                                            strImmoTemp += vbtab & "Riduzione Applicata:" & Microsoft.VisualBasic.Constants.vbCrLf
                                            Do While drDatiRid.Read
                                                strImmoTemp += vbtab & drDatiRid("CODICE") & " - " & drDatiRid("descrizione") & Microsoft.VisualBasic.Constants.vbCrLf
                                            Loop
                                        End If

                                    End If
                                Catch ex As Exception
                                    strImmoTemp += ""
                                End Try
                                '****************************
                                'strImmoTemp += strRiga & Microsoft.VisualBasic.Constants.vbCrLf
                                strImmoTemp += Microsoft.VisualBasic.Constants.vbCrLf

                            Else
                                'Accertato
                                'Manca il dichiarato
                                strImmoTemp += Microsoft.VisualBasic.Constants.vbCrLf
                                strImmoTemp += "DATI RISULTANTI DALLA DICHIARAZIONE: Nessun Immobile Dichiarato" & Microsoft.VisualBasic.Constants.vbCrLf
                                strImmoTemp += Microsoft.VisualBasic.Constants.vbCrLf
                                strImmoTemp += "DATI ACCERTATI :" & Microsoft.VisualBasic.Constants.vbCrLf & Microsoft.VisualBasic.Constants.vbCrLf
                                'Periodo
                                strImmoTemp += vbtab & "Periodo"
                                If Not ds.Tables(0).Rows(iImmobili)("DATA_INIZIO") Is DBNull.Value Then
                                    strImmoTemp += " dal " & ds.Tables(0).Rows(iImmobili)("DATA_INIZIO")
                                Else
                                    strImmoTemp += " dal 01/01/" & Anno
                                End If
                                If Not ds.Tables(0).Rows(iImmobili)("DATA_FINE") Is DBNull.Value Then
                                    strImmoTemp += " al " & ds.Tables(0).Rows(iImmobili)("DATA_FINE")
                                Else
                                    strImmoTemp += " al 31/12/" & Anno
                                End If
                                strImmoTemp += Microsoft.VisualBasic.Constants.vbCrLf & Microsoft.VisualBasic.Constants.vbCrLf
                                'Indirizzo
                                strImmoTemp += vbtab & "Indirizzo: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("Via")) & " " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("Civico")) & " " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("Interno")) & Microsoft.VisualBasic.Constants.vbCrLf
                                'Categoria
                                strImmoTemp += vbtab & "Categoria: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("IDCATEGORIA")) & " - " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("descrizione")) & Microsoft.VisualBasic.Constants.vbCrLf
                                'Altri dati
                                strImmoTemp += vbtab & "Tariffa: " & CStr(FormatNumber(ds.Tables(0).Rows(iImmobili)("IMPORTO_TARIFFA"))) & " € " & vbtab
                                strImmoTemp += "Superficie: " & FormatImport(ds.Tables(0).Rows(iImmobili)("MQ")) & " Mq" & vbtab
                                strImmoTemp += "Bimestri: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("bimestri")) & vbtab
                                strImmoTemp += "Importo dovuto: " & FormatImport(ds.Tables(0).Rows(iImmobili)("IMPORTO_NETTO")) & " €" & Microsoft.VisualBasic.Constants.vbCrLf

                                strImmoTemp += vbtab & "Foglio: " & ds.Tables(0).Rows(iImmobili)("FOGLIO") & vbtab & vbtab
                                strImmoTemp += "Numero: " & ds.Tables(0).Rows(iImmobili)("NUMERO") & vbtab & vbtab
                                strImmoTemp += "Subalterno: " & ds.Tables(0).Rows(iImmobili)("SUBALTERNO") & Microsoft.VisualBasic.Constants.vbCrLf & Microsoft.VisualBasic.Constants.vbCrLf

                                Diff_Superficie = ds.Tables(0).Rows(iImmobili)("MQ") - temp_Superficie
                                Diff_Importo = ds.Tables(0).Rows(iImmobili)("IMPORTO_NETTO") - temp_Importo
                                strImmoTemp += "Differenza di superficie: " & FormatImport(Diff_Superficie) & " Mq" & vbtab & " Differenza di imposta: " & FormatImport(Diff_Importo) & " €" & Microsoft.VisualBasic.Constants.vbCrLf
                                'strImmoTemp += strRiga & Microsoft.VisualBasic.Constants.vbCrLf

                                '********* Riduzioni ********
                                Try
                                    'drDatiRid = objAcc.GetRiduzioniAccertato(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"), ds.Tables(0).Rows(iImmobili)("ID_PROVVEDIMENTO"), ds.Tables(0).Rows(iImmobili)("ID"))
                                    If Not IsNothing(drDatiRid) Then
                                        If drDatiRid.HasRows Then
                                            strImmoTemp += vbtab & "Riduzione Applicata:" & Microsoft.VisualBasic.Constants.vbCrLf
                                            Do While drDatiRid.Read
                                                strImmoTemp += vbtab & drDatiRid("CODICE") & " - " & drDatiRid("descrizione") & Microsoft.VisualBasic.Constants.vbCrLf
                                            Loop
                                        End If
                                    End If
                                Catch ex As Exception
                                    strImmoTemp += ""
                                End Try
                                '****************************
                                strImmoTemp += strRiga & Microsoft.VisualBasic.Constants.vbCrLf

                                temp_Superficie = 0
                                temp_Importo = 0
                                Diff_Superficie = 0
                                Diff_Importo = 0
                            End If
                        Else
                            'legame uguale
                            If tipo_imm = "D" Then
                                'Dichiarato
                            Else
                                'Accertato
                                strImmoTemp += Microsoft.VisualBasic.Constants.vbCrLf
                                strImmoTemp += "DATI ACCERTATI :" & Microsoft.VisualBasic.Constants.vbCrLf & Microsoft.VisualBasic.Constants.vbCrLf
                                'Periodo
                                strImmoTemp += vbtab & "Periodo"
                                If Not ds.Tables(0).Rows(iImmobili)("DATA_INIZIO") Is DBNull.Value Then
                                    strImmoTemp += " dal " & ds.Tables(0).Rows(iImmobili)("DATA_INIZIO")
                                Else
                                    strImmoTemp += " dal 01/01/" & Anno
                                End If
                                If Not ds.Tables(0).Rows(iImmobili)("DATA_FINE") Is DBNull.Value Then
                                    strImmoTemp += " al " & ds.Tables(0).Rows(iImmobili)("DATA_FINE")
                                Else
                                    strImmoTemp += " al 31/12/" & Anno
                                End If
                                strImmoTemp += Microsoft.VisualBasic.Constants.vbCrLf & Microsoft.VisualBasic.Constants.vbCrLf
                                'Indirizzo
                                strImmoTemp += vbtab & "Indirizzo: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("Via")) & " " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("Civico")) & " " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("Interno")) & Microsoft.VisualBasic.Constants.vbCrLf
                                'Categoria
                                strImmoTemp += vbtab & "Categoria: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("IDCATEGORIA")) & " - " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("descrizione")) & Microsoft.VisualBasic.Constants.vbCrLf
                                'Altri dati
                                strImmoTemp += vbtab & "Tariffa: " & CStr(FormatNumber(ds.Tables(0).Rows(iImmobili)("IMPORTO_TARIFFA"))) & " €" & vbtab
                                strImmoTemp += "Superficie: " & FormatImport(ds.Tables(0).Rows(iImmobili)("MQ")) & " Mq" & vbtab
                                strImmoTemp += "Bimestri: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("bimestri")) & vbtab
                                strImmoTemp += "Importo dovuto: " & FormatImport(ds.Tables(0).Rows(iImmobili)("IMPORTO_NETTO")) & " €" & Microsoft.VisualBasic.Constants.vbCrLf

                                strImmoTemp += vbtab & "Foglio: " & ds.Tables(0).Rows(iImmobili)("FOGLIO") & vbtab & vbtab
                                strImmoTemp += "Numero: " & ds.Tables(0).Rows(iImmobili)("NUMERO") & vbtab & vbtab
                                strImmoTemp += "Subalterno: " & ds.Tables(0).Rows(iImmobili)("SUBALTERNO") & Microsoft.VisualBasic.Constants.vbCrLf & Microsoft.VisualBasic.Constants.vbCrLf

                                Diff_Superficie = ds.Tables(0).Rows(iImmobili)("MQ") - temp_Superficie
                                Diff_Importo = ds.Tables(0).Rows(iImmobili)("IMPORTO_NETTO") - temp_Importo
                                strImmoTemp += "Differenza di superficie: " & FormatImport(Diff_Superficie) & " Mq" & vbtab & " Differenza di imposta: " & FormatImport(Diff_Importo) & " €" & Microsoft.VisualBasic.Constants.vbCrLf

                                '********* Riduzioni ********
                                Try
                                    'drDatiRid = objAcc.GetRiduzioniAccertato(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"), ds.Tables(0).Rows(iImmobili)("ID_PROVVEDIMENTO"), ds.Tables(0).Rows(iImmobili)("ID"))
                                    If Not IsNothing(drDatiRid) Then
                                        If drDatiRid.HasRows Then
                                            strImmoTemp += vbtab & "Riduzione Applicata:" & Microsoft.VisualBasic.Constants.vbCrLf
                                            Do While drDatiRid.Read
                                                strImmoTemp += vbtab & drDatiRid("CODICE") & " - " & drDatiRid("descrizione") & Microsoft.VisualBasic.Constants.vbCrLf
                                            Loop
                                        End If
                                    End If
                                Catch ex As Exception
                                    strImmoTemp += ""
                                End Try
                                '****************************
                                strImmoTemp += strRiga & Microsoft.VisualBasic.Constants.vbCrLf

                                temp_Superficie = 0
                                temp_Importo = 0
                                Diff_Superficie = 0
                                Diff_Importo = 0
                            End If
                        End If




                        id_legame_old = id_legame


                    Next

                Else
                    strImmoTemp = strRiga
                    strImmoTemp = "Nessun Immobile dichiarato." & Microsoft.VisualBasic.Constants.vbCrLf
                    strImmoTemp = strImmoTemp & strRiga
                End If
            Else
                strImmoTemp = "Nessun Immobile dichiarato." & Microsoft.VisualBasic.Constants.vbCrLf
            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.FillBookMarkDICHACCtarsu.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
        Return strImmoTemp
    End Function
    'Private Function FillBookMarkAnagrafica(ByVal ListBookmark As ArrayList, ByVal myRow As DataRow) As ArrayList
    '    Dim oBookmark As New oggettiStampa
    '    Dim oArrBookmark As New ArrayList

    '    Try
    '        oArrBookmark = ListBookmark

    '        oBookmark = New oggettiStampa
    '        oBookmark.Descrizione = "cognome"
    '        oBookmark.Valore = FormatStringToEmpty(myRow("COGNOME"))
    '        oArrBookmark.Add(oBookmark)

    '        oBookmark = New oggettiStampa
    '        oBookmark.Descrizione = "nome"
    '        oBookmark.Valore = FormatStringToEmpty(myRow("NOME"))
    '        oArrBookmark.Add(oBookmark)

    '        'Nominativo_CO
    '        oBookmark = New oggettiStampa
    '        oBookmark.Descrizione = "Nominativo_CO"
    '        oBookmark.Valore = FormatStringToEmpty(myRow("CO"))
    '        oArrBookmark.Add(oBookmark)

    '        oBookmark = New oggettiStampa
    '        oBookmark.Descrizione = "via_residenza"
    '        If Not IsDBNull(myRow("VIA_CO")) Then
    '            If CStr(myRow("VIA_CO")) <> "" Then
    '                oBookmark.Valore = FormatStringToEmpty(myRow("VIA_CO"))
    '            Else
    '                oBookmark.Valore = FormatStringToEmpty(myRow("VIA_RES"))
    '            End If
    '        Else
    '            oBookmark.Valore = FormatStringToEmpty(myRow("VIA_RES"))
    '        End If
    '        oArrBookmark.Add(oBookmark)

    '        oBookmark = New oggettiStampa
    '        oBookmark.Descrizione = "civico_residenza"
    '        If Not IsDBNull(myRow("CIVICO_CO")) Then
    '            If CStr(myRow("CIVICO_CO")) <> "" Then
    '                oBookmark.Valore = FormatStringToEmpty(myRow("CIVICO_CO"))
    '            Else
    '                oBookmark.Valore = FormatStringToEmpty(myRow("CIVICO_RES"))
    '            End If
    '        Else
    '            oBookmark.Valore = FormatStringToEmpty(myRow("CIVICO_RES"))
    '        End If
    '        oArrBookmark.Add(oBookmark)

    '        oBookmark = New oggettiStampa
    '        oBookmark.Descrizione = "frazione_residenza"
    '        If Not IsDBNull(myRow("FRAZIONE_CO")) Then
    '            If CStr(myRow("FRAZIONE_CO")) <> "" Then
    '                oBookmark.Valore = FormatStringToEmpty(myRow("FRAZIONE_CO"))
    '            Else
    '                oBookmark.Valore = FormatStringToEmpty(myRow("FRAZIONE_RES"))
    '            End If
    '        Else
    '            oBookmark.Valore = FormatStringToEmpty(myRow("FRAZIONE_RES"))
    '        End If
    '        oArrBookmark.Add(oBookmark)

    '        oBookmark = New oggettiStampa
    '        oBookmark.Descrizione = "cap_residenza"
    '        If Not IsDBNull(myRow("CAP_CO")) Then
    '            If CStr(myRow("CAP_CO")) <> "" Then
    '                oBookmark.Valore = FormatStringToEmpty(myRow("CAP_CO"))
    '            Else
    '                oBookmark.Valore = FormatStringToEmpty(myRow("CAP_RES"))
    '            End If
    '        Else
    '            oBookmark.Valore = FormatStringToEmpty(myRow("CAP_RES"))
    '        End If
    '        oArrBookmark.Add(oBookmark)

    '        oBookmark = New oggettiStampa
    '        oBookmark.Descrizione = "citta_residenza"
    '        If Not IsDBNull(myRow("CITTA_CO")) Then
    '            If CStr(myRow("CITTA_CO")) <> "" Then
    '                oBookmark.Valore = FormatStringToEmpty(myRow("CITTA_CO"))
    '            Else
    '                oBookmark.Valore = FormatStringToEmpty(myRow("CITTA_RES"))
    '            End If
    '        Else
    '            oBookmark.Valore = FormatStringToEmpty(myRow("CITTA_RES"))
    '        End If
    '        oArrBookmark.Add(oBookmark)

    '        oBookmark = New oggettiStampa
    '        oBookmark.Descrizione = "prov_residenza"
    '        If Not IsDBNull(myRow("PROVINCIA_CO")) Then
    '            If CStr(myRow("PROVINCIA_CO")) <> "" Then
    '                oBookmark.Valore = "(" & FormatStringToEmpty(myRow("PROVINCIA_CO")) & ")"
    '            Else
    '                oBookmark.Valore = "(" & FormatStringToEmpty(myRow("PROVINCIA_RES")) & ")"
    '            End If
    '        Else
    '            oBookmark.Valore = "(" & FormatStringToEmpty(myRow("PROVINCIA_RES")) & ")"
    '        End If
    '        oArrBookmark.Add(oBookmark)

    '        oBookmark = New oggettiStampa
    '        oBookmark.Descrizione = "codice_fiscale"
    '        oBookmark.Valore = FormatStringToEmpty(myRow("CODICE_FISCALE"))
    '        oArrBookmark.Add(oBookmark)

    '        oBookmark = New oggettiStampa
    '        oBookmark.Descrizione = "partita_iva"
    '        oBookmark.Valore = FormatStringToEmpty(myRow("PARTITA_IVA"))
    '        oArrBookmark.Add(oBookmark)

    '        oBookmark = New oggettiStampa
    '        oBookmark.Descrizione = "codice_fiscale_2"
    '        oBookmark.Valore = FormatStringToEmpty(myRow("CODICE_FISCALE"))
    '        oArrBookmark.Add(oBookmark)

    '        oBookmark = New oggettiStampa
    '        oBookmark.Descrizione = "partita_iva_2"
    '        oBookmark.Valore = FormatStringToEmpty(myRow("PARTITA_IVA"))
    '        oArrBookmark.Add(oBookmark)

    '        Return oArrBookmark
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.FillBookMarkAnagrafica.errore: ", ex)
    '  Response.Redirect("../../../PaginaErrore.aspx")
    '        Return Nothing
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ListBookmark"></param>
    ''' <param name="IdContrib"></param>
    ''' <param name="sTributo"></param>
    ''' <param name="FileNameContrib"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="27/10/2014">
    ''' <strong>visualizzazione tutti indirizzi spedizione</strong>
    ''' Nuova gestione indirizzi spedizione
    ''' </revision>
    ''' </revisionHistory>
    Private Function FillBookMarkAnagrafica(ByVal ListBookmark As ArrayList, ByVal IdContrib As Integer, ByVal sTributo As String, ByRef FileNameContrib As String) As ArrayList
        Dim oBookmark As New oggettiStampa
        Dim oArrBookmark As New ArrayList
        Dim oAnagrafica As New DettaglioAnagrafica
        Dim FncAnag As New Anagrafica.DLL.GestioneAnagrafica
        Dim sViaRes, sCivicoRes, sFrazioneRes, sCapRes, sComuneRes, sPVRes As String
        Dim sViaCO, sCivicoCO, sFrazioneCO, sCapCO, sComuneCO, sPVCO, sNominativoCO As String
        Dim sVia, sCivico, sFrazione, sCap, sComune, sPV As String

        Try
            oArrBookmark = ListBookmark

            oAnagrafica = FncAnag.GetAnagrafica(IdContrib, -1, String.Empty, ConstSession.DBType, ConstSession.StringConnectionAnagrafica, False)
            If Not oAnagrafica Is Nothing Then
                If oAnagrafica.PartitaIva = "" Then
                    FileNameContrib = oAnagrafica.CodiceFiscale
                Else
                    FileNameContrib = oAnagrafica.PartitaIva
                End If
                For x As Integer = 0 To 9
                    oBookmark = New oggettiStampa
                    oBookmark.Descrizione = Replace("cognome" & x.ToString(), "0", "")
                    oBookmark.Valore = FormatStringToEmpty(oAnagrafica.Cognome)
                    oArrBookmark.Add(oBookmark)
                Next
                For x As Integer = 0 To 9
                    oBookmark = New oggettiStampa
                    oBookmark.Descrizione = Replace("nome" & x.ToString(), "0", "")
                    oBookmark.Valore = FormatStringToEmpty(oAnagrafica.Nome)
                    oArrBookmark.Add(oBookmark)
                Next
                For x As Integer = 0 To 9
                    oBookmark = New oggettiStampa
                    oBookmark.Descrizione = Replace("codice_fiscale" & x.ToString(), "0", "")
                    oBookmark.Valore = FormatStringToEmpty(oAnagrafica.CodiceFiscale)
                    oArrBookmark.Add(oBookmark)
                Next
                For x As Integer = 0 To 9
                    oBookmark = New oggettiStampa
                    oBookmark.Descrizione = Replace("partita_iva" & x.ToString(), "0", "")
                    oBookmark.Valore = FormatStringToEmpty(oAnagrafica.PartitaIva)
                    oArrBookmark.Add(oBookmark)
                Next

                sViaRes = FormatStringToEmpty(oAnagrafica.ViaResidenza)
                sCivicoRes = FormatStringToEmpty(oAnagrafica.CivicoResidenza)
                sCivicoRes += " " + FormatStringToEmpty(oAnagrafica.EsponenteCivicoResidenza)
                sCivicoRes += " " + FormatStringToEmpty(oAnagrafica.ScalaCivicoResidenza)
                sCivicoRes += " " + FormatStringToEmpty(oAnagrafica.InternoCivicoResidenza)
                sFrazioneRes = FormatStringToEmpty(oAnagrafica.FrazioneResidenza)
                sCapRes = FormatStringToEmpty(oAnagrafica.CapResidenza)
                sComuneRes = FormatStringToEmpty(oAnagrafica.ComuneResidenza)
                sPVRes = FormatStringToEmpty(oAnagrafica.ProvinciaResidenza)
                '*** 20141027 - visualizzazione tutti indirizzi spedizione ***
                'strViaCO = FormatStringToEmpty(oDettaglioAnagrafica.ViaRCP)
                'strCivCO = FormatStringToEmpty(oDettaglioAnagrafica.CivicoRCP)
                'strFrazCO = FormatStringToEmpty(oDettaglioAnagrafica.FrazioneRCP)
                'strCapCO = FormatStringToEmpty(oDettaglioAnagrafica.CapRCP)
                'strCittaCO = FormatStringToEmpty(oDettaglioAnagrafica.ComuneRCP)
                'strProvCO = FormatStringToEmpty(oDettaglioAnagrafica.ProvinciaRCP)
                For Each mySped As ObjIndirizziSpedizione In oAnagrafica.ListSpedizioni
                    If mySped.CodTributo = CInt(sTributo).ToString Then
                        sNominativoCO = ("C/O " & FormatStringToEmpty(mySped.CognomeInvio) & " " & FormatStringToEmpty(mySped.NomeInvio)).ToUpper()
                        sViaCO = FormatStringToEmpty(mySped.ViaRCP)
                        sCivicoCO = FormatStringToEmpty(mySped.CivicoRCP)
                        sCivicoCO += " " + FormatStringToEmpty(mySped.EsponenteCivicoRCP)
                        sCivicoCO += " " + FormatStringToEmpty(mySped.ScalaCivicoRCP)
                        sCivicoCO += " " + FormatStringToEmpty(mySped.InternoCivicoRCP)
                        sFrazioneCO = FormatStringToEmpty(mySped.FrazioneRCP)
                        sCapCO = FormatStringToEmpty(mySped.CapRCP)
                        sComuneCO = FormatStringToEmpty(mySped.ComuneRCP)
                        sPVCO = FormatStringToEmpty(mySped.ProvinciaRCP)
                    End If
                Next
                '*** ***
                If (sViaCO = "") Then
                    'visualizzo indirizzo residenza
                    sVia = sViaRes.ToUpper()
                    sCivico = sCivicoRes.ToUpper()
                    sFrazione = sFrazioneRes.ToUpper()
                    sCap = sCapRes.ToUpper()
                    sComune = sComuneRes.ToUpper()
                    sPV = sPVRes.ToUpper()
                    sNominativoCO = ""
                Else
                    'visualizzo indirizzo spedizione
                    sVia = sViaCO.ToUpper()
                    sCivico = sCivicoCO.ToUpper()
                    sFrazione = sFrazioneCO.ToUpper()
                    sCap = sCapCO.ToUpper()
                    sComune = sComuneCO.ToUpper()
                    sPV = sPVCO.ToUpper()
                End If
                If sPV <> "" Then sPV = "(" & sPV & ")"
                'Nominativo_CO
                For x As Integer = 0 To 9
                    oBookmark = New oggettiStampa
                    oBookmark.Descrizione = Replace("Nominativo_CO" & x.ToString(), "0", "")
                    oBookmark.Valore = sNominativoCO
                    oArrBookmark.Add(oBookmark)
                Next
                For x As Integer = 0 To 9
                    oBookmark = New oggettiStampa
                    oBookmark.Descrizione = Replace("via_residenza" & x.ToString(), "0", "")
                    oBookmark.Valore = sVia
                    oArrBookmark.Add(oBookmark)
                Next
                For x As Integer = 0 To 9
                    oBookmark = New oggettiStampa
                    oBookmark.Descrizione = Replace("civico_residenza" & x.ToString(), "0", "")
                    oBookmark.Valore = sCivico
                    oArrBookmark.Add(oBookmark)
                Next
                For x As Integer = 0 To 9
                    oBookmark = New oggettiStampa
                    oBookmark.Descrizione = Replace("frazione_residenza" & x.ToString(), "0", "")
                    oBookmark.Valore = sFrazione
                    oArrBookmark.Add(oBookmark)
                Next
                For x As Integer = 0 To 9
                    oBookmark = New oggettiStampa
                    oBookmark.Descrizione = Replace("cap_residenza" & x.ToString(), "0", "")
                    oBookmark.Valore = sCap
                    oArrBookmark.Add(oBookmark)
                Next
                For x As Integer = 0 To 9
                    oBookmark = New oggettiStampa
                    oBookmark.Descrizione = Replace("citta_residenza" & x.ToString(), "0", "")
                    oBookmark.Valore = sComune
                    oArrBookmark.Add(oBookmark)
                Next
                For x As Integer = 0 To 9
                    oBookmark = New oggettiStampa
                    oBookmark.Descrizione = Replace("prov_residenza" & x.ToString(), "0", "")
                    oBookmark.Valore = sPV
                    oArrBookmark.Add(oBookmark)
                Next
                Return oArrBookmark
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.FillBookMarkAnagrafica.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ListBookmark"></param>
    ''' <param name="myRow"></param>
    ''' <param name="objHashTable"></param>
    ''' <param name="IDPROVVEDIMENTO"></param>
    ''' <param name="ImportoTotAddizionali"></param>
    ''' <returns></returns>
    Private Function FillBookMarkImporti(ByVal ListBookmark As ArrayList, ByVal myRow As DataRow, ByVal objHashTable As Hashtable, ByVal IDPROVVEDIMENTO As String, ByRef ImportoTotAddizionali As Double) As ArrayList
        Dim oBookmark As New oggettiStampa
        Dim oArrBookmark As New ArrayList
        Dim objDSElencoAddizionali As New DataSet
        Dim objCOMACCERT As IElaborazioneAccertamenti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAccertamenti), ConstSession.URLServiziAccertamenti)
        Dim sDatiStampa As String

        Try
            oArrBookmark = ListBookmark
            'Tassa	                 €     [SOMMA ALGEBRICA DELLE DIFF. DI IMPOSTA]
            'Addizionale ex- Eca 5%	 €     
            'Maggiorazione ex-Eca 5% €     [IN BASE ALLE CONFIGURAZIONI]
            'Tributo Provinciale 1%  €     
            'Sanzione Amministrativa €     [SOMMA ALGEBRICA DELLE SANZIONI]
            'Interessi 				 €     [SOMMA ALGEBRICA DEGLI INTERESSI]
            'Spese di notifica       €     [SPESE DI NOTIFICA]
            'Arrotondamento          €     [ARROTONDAMENTO]
            'Totale                  €     [IMPORTO TOTALE AVVISO]
            '************************************************************************************
            'IMPORTI (DIFF IMPOSTA - SANZIONI - INTERESSI - SPESE - ecc...
            '************************************************************************************
            oBookmark = New oggettiStampa
            oBookmark.Descrizione = "imposta_dovuta"
            oBookmark.Valore = FormatImport(myRow("IMPORTO_DIFFERENZA_IMPOSTA")) & " €"
            oArrBookmark.Add(oBookmark)

            objDSElencoAddizionali = objCOMACCERT.getAddizionaliPerStampaAccertamentiTARSU(ConstSession.StringConnection, objHashTable, IDPROVVEDIMENTO)
            sDatiStampa = FillBookMarkELENCOADDIZIONALI(objDSElencoAddizionali, ImportoTotAddizionali)
            oBookmark = New oggettiStampa
            oBookmark.Descrizione = "Elenco_Addizionali"
            oBookmark.Valore = sDatiStampa
            oArrBookmark.Add(oBookmark)

            oBookmark = New oggettiStampa
            oBookmark.Descrizione = "Elenco_Addizionali_1"
            oBookmark.Valore = sDatiStampa
            oArrBookmark.Add(oBookmark)

            oBookmark = New oggettiStampa
            oBookmark.Descrizione = "Importo_Sanzione"
            oBookmark.Valore = FormatImport(myRow("IMPORTO_SANZIONI")) & " €"
            oArrBookmark.Add(oBookmark)

            oBookmark = New oggettiStampa
            oBookmark.Descrizione = "spese_notifica"
            oBookmark.Valore = FormatImport(myRow("IMPORTO_SPESE")) & " €"
            oArrBookmark.Add(oBookmark)

            oBookmark = New oggettiStampa
            oBookmark.Descrizione = "Importo_arrotond"
            oBookmark.Valore = FormatImport(myRow("IMPORTO_ARROTONDAMENTO")) & " €"
            oArrBookmark.Add(oBookmark)

            oBookmark = New oggettiStampa
            oBookmark.Descrizione = "Importo_totale"
            oBookmark.Valore = FormatImport(myRow("IMPORTO_TOTALE")) & " €"
            oArrBookmark.Add(oBookmark)

            oBookmark = New oggettiStampa
            oBookmark.Descrizione = "Importo_totale_2"
            oBookmark.Valore = FormatImport(myRow("IMPORTO_TOTALE")) & " €"
            oArrBookmark.Add(oBookmark)

            oBookmark = New oggettiStampa
            oBookmark.Descrizione = "importo_interessi"
            oBookmark.Valore = FormatImport(myRow("IMPORTO_INTERESSI")) & " €"
            oArrBookmark.Add(oBookmark)

            oBookmark = New oggettiStampa
            oBookmark.Descrizione = "imposta_dovuta_1"
            oBookmark.Valore = FormatImport(myRow("IMPORTO_DIFFERENZA_IMPOSTA")) & " €"
            oArrBookmark.Add(oBookmark)

            If Not IsDBNull(myRow("IMPORTO_SANZIONI_RIDOTTO")) Then
                'altrimenti stampo uguale'
                If myRow("IMPORTO_SANZIONI") > myRow("IMPORTO_SANZIONI_RIDOTTO") Then
                    oBookmark = New oggettiStampa
                    oBookmark.Descrizione = "Importo_Sanzione_1"
                    oBookmark.Valore = FormatImport(myRow("IMPORTO_SANZIONI_RIDOTTO")) & " €"
                    oArrBookmark.Add(oBookmark)
                Else
                    oBookmark = New oggettiStampa
                    oBookmark.Descrizione = "Importo_Sanzione_1"
                    oBookmark.Valore = FormatImport(myRow("IMPORTO_SANZIONI_RIDOTTO")) & " €"
                    oArrBookmark.Add(oBookmark)
                End If
            Else
                oBookmark = New oggettiStampa
                oBookmark.Descrizione = "Importo_Sanzione_1"
                oBookmark.Valore = FormatImport(myRow("IMPORTO_SANZIONI_RIDOTTO")) & " €"
                oArrBookmark.Add(oBookmark)
            End If

            oBookmark = New oggettiStampa
            oBookmark.Descrizione = "spese_notifica_1"
            oBookmark.Valore = FormatImport(myRow("IMPORTO_SPESE")) & " €"
            oArrBookmark.Add(oBookmark)

            oBookmark = New oggettiStampa
            oBookmark.Descrizione = "Importo_arrotond_1"
            oBookmark.Valore = FormatImport(myRow("IMPORTO_ARROTONDAMENTO_RIDOTTO")) & " €"
            oArrBookmark.Add(oBookmark)

            oBookmark = New oggettiStampa
            oBookmark.Descrizione = "importo_interessi_1"
            oBookmark.Valore = FormatImport(myRow("IMPORTO_INTERESSI")) & " €"
            oArrBookmark.Add(oBookmark)

            'altrimenti stampo uguale'
            If Not IsDBNull(myRow("IMPORTO_SANZIONI_RIDOTTO")) Then
                If myRow("IMPORTO_SANZIONI") > myRow("IMPORTO_SANZIONI_RIDOTTO") Then
                    oBookmark = New oggettiStampa
                    oBookmark.Descrizione = "Importo_totale_1"
                    'ImportoTotaleRidotto = CDbl(myRow("IMPORTO_DIFFERENZA_IMPOSTA")) + ImportoTotAddizionali + CDbl(myRow("IMPORTO_SANZIONI_RIDOTTO")) + CDbl(myRow("IMPORTO_SPESE")) + CDbl(myRow("IMPORTO_INTERESSI")) & " €"
                    oBookmark.Valore = FormatImport(myRow("IMPORTO_TOTALE_RIDOTTO")) & " €"
                    oArrBookmark.Add(oBookmark)
                Else
                    oBookmark = New oggettiStampa
                    oBookmark.Descrizione = "Importo_totale_1"
                    oBookmark.Valore = FormatImport(myRow("IMPORTO_TOTALE_RIDOTTO")) & " €"
                    oArrBookmark.Add(oBookmark)
                End If
            Else
                oBookmark = New oggettiStampa
                oBookmark.Descrizione = "Importo_totale_1"
                oBookmark.Valore = FormatImport(myRow("IMPORTO_TOTALE_RIDOTTO")) & " €"
                oArrBookmark.Add(oBookmark)
            End If

            oBookmark = New oggettiStampa
            oBookmark.Descrizione = "TotQF"
            oBookmark.Valore = FormatImport(myRow("IMPQF")) & " €"
            oArrBookmark.Add(oBookmark)
            oBookmark = New oggettiStampa
            oBookmark.Descrizione = "TotQV"
            oBookmark.Valore = FormatImport(myRow("IMPQV")) & " €"
            oArrBookmark.Add(oBookmark)
            oBookmark = New oggettiStampa
            oBookmark.Descrizione = "TotMagg"
            oBookmark.Valore = FormatImport(myRow("IMPMAG")) & " €"
            oArrBookmark.Add(oBookmark)
            oBookmark = New oggettiStampa
            oBookmark.Descrizione = "TotQF_1"
            oBookmark.Valore = FormatImport(myRow("IMPQF")) & " €"
            oArrBookmark.Add(oBookmark)
            oBookmark = New oggettiStampa
            oBookmark.Descrizione = "TotQV_1"
            oBookmark.Valore = FormatImport(myRow("IMPQV")) & " €"
            oArrBookmark.Add(oBookmark)
            oBookmark = New oggettiStampa
            oBookmark.Descrizione = "TotMagg_1"
            oBookmark.Valore = FormatImport(myRow("IMPMAG")) & " €"
            oArrBookmark.Add(oBookmark)

            Return oArrBookmark
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.FillBookMarkImporti.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
            Return Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myConnectionString"></param>
    ''' <param name="ds"></param>
    ''' <param name="Anno"></param>
    ''' <returns></returns>
    Private Function FillBookMarkDICHACC_TARES(ByVal myConnectionString As String, ByVal ds As DataSet, ByVal Anno As String) As String
        Dim myRow As DataRow
        Dim sDescrDic, sDescrAcc As String
        Dim nUIDich, nUIAcc As Integer

        Try
            sDescrAcc = "" : sDescrDic = ""
            If ds.Tables.Count > 0 Then
                If ds.Tables(0).Rows.Count > 0 Then
                    sDescrAcc = "DATI RISULTANTI DALL'ACCERTAMENTO:" & Microsoft.VisualBasic.Constants.vbTab & Microsoft.VisualBasic.Constants.vbTab & Microsoft.VisualBasic.Constants.vbTab & Microsoft.VisualBasic.Constants.vbTab

                    For Each myRow In ds.Tables(0).Rows
                        Select Case Utility.StringOperation.FormatString(myRow("TIPO_IMM"))
                            Case "D"
                                sDescrDic += TARES_FormattaBookMarkUI(myConnectionString, myRow, Anno, nUIDich)
                            Case "A"
                                sDescrAcc += TARES_FormattaBookMarkUI(myConnectionString, myRow, Anno, nUIAcc)
                        End Select
                    Next
                    If sDescrDic <> "" Then
                        sDescrDic = "DATI RISULTANTI DALLA DICHIARAZIONE:" & Microsoft.VisualBasic.Constants.vbTab & Microsoft.VisualBasic.Constants.vbTab & Microsoft.VisualBasic.Constants.vbTab & Microsoft.VisualBasic.Constants.vbTab + sDescrDic
                    Else
                        sDescrDic = "Nessun Immobile dichiarato." & Microsoft.VisualBasic.Constants.vbCrLf
                    End If
                Else
                    sDescrDic = "Nessun Immobile dichiarato." & Microsoft.VisualBasic.Constants.vbCrLf
                End If
            Else
                sDescrDic = "Nessun Immobile dichiarato." & Microsoft.VisualBasic.Constants.vbCrLf
            End If

            Return sDescrDic & Microsoft.VisualBasic.Constants.vbCrLf & sDescrAcc
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.FillBookMarkDICHACC_TARES.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
            Return ""
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myConnectionString"></param>
    ''' <param name="myRow"></param>
    ''' <param name="Anno"></param>
    ''' <param name="nUI"></param>
    ''' <returns></returns>
    Private Function TARES_FormattaBookMarkUI(ByVal myConnectionString As String, ByVal myRow As DataRow, ByVal Anno As String, ByRef nUI As Integer) As String
        Dim sMyDati As String
        Dim FncAcc As New ClsGestioneAccertamenti
        Dim drDatiRid As SqlClient.SqlDataReader

        Try
            nUI += 1
            sMyDati += vbCrLf & nUI.ToString & ")" & vbTab
            'Periodo
            sMyDati += "Periodo"
            If Not myRow("DATA_INIZIO") Is DBNull.Value Then
                sMyDati += " dal " & myRow("DATA_INIZIO")
            Else
                sMyDati += " dal 01/01/" & Anno
            End If
            If Not myRow("DATA_FINE") Is DBNull.Value Then
                sMyDati += " al " & myRow("DATA_FINE")
            Else
                sMyDati += " al 31/12/" & Anno
            End If
            sMyDati += vbCrLf
            'Indirizzo
            sMyDati += vbTab & "Indirizzo: " & FormatStringToEmpty(myRow("Via")) & " " & FormatStringToEmpty(myRow("Civico")) & " " & FormatStringToEmpty(myRow("Interno")) & vbCrLf
            'Categoria
            sMyDati += vbTab & "Categoria: " & FormatStringToEmpty(myRow("IDCATEGORIA")) & " - " & FormatStringToEmpty(myRow("descrizione")) & vbCrLf
            'Altri dati
            sMyDati += vbTab & "Tariffa: " & CStr(FormatNumber(myRow("IMPORTO_TARIFFA"))) & " € "
            sMyDati += vbTab & "Superficie: " & FormatImport(myRow("MQ")) & " Mq"
            sMyDati += vbTab & "GG: " & FormatStringToEmpty(myRow("bimestri"))
            sMyDati += vbTab & "Importo dovuto: " & FormatImport(myRow("IMPORTO_NETTO")) & " €" & vbCrLf

            '********* Riduzioni ********
            Try
                drDatiRid = FncAcc.GetRiduzioni(myConnectionString, myRow("IDENTE"), myRow("IDDETTAGLIOTESTATA"), myRow("ANNO"))
                If Not IsNothing(drDatiRid) Then
                    If drDatiRid.HasRows Then
                        sMyDati += vbTab & "Riduzione Applicata:" & vbCrLf
                        Do While drDatiRid.Read
                            sMyDati += vbTab & drDatiRid("CODICE") & " - " & drDatiRid("descrizione") & vbCrLf
                        Loop
                    End If
                End If
            Catch ex As Exception
                sMyDati += ""
            End Try
            '****************************
            Return sMyDati
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.TARES_FormattaBookMarkUI.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
            Return ""
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ds"></param>
    ''' <returns></returns>
    Private Function FillBookMarkVersamenti(ByVal ds As DataSet) As String

        Dim strRiga As String = ""
        Dim strVersTemp As String = String.Empty
        Dim iVersamenti As Integer
        Dim objUtility As New MyUtility

        Dim culture As IFormatProvider
        culture = New CultureInfo("it-IT", True)
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")

        Try
            If ds.Tables(0).Rows.Count > 0 Then


                strRiga = ""
                strRiga = strRiga.PadLeft(148, "-")
                strVersTemp = strRiga


                For iVersamenti = 0 To ds.Tables(0).Rows.Count - 1

                    If CBool(ds.Tables(0).Rows(iVersamenti)("Acconto")) = True And CBool(ds.Tables(0).Rows(iVersamenti)("Saldo")) = False Then
                        strVersTemp = strVersTemp & "Tipo Vers: ACC"
                    ElseIf CBool(ds.Tables(0).Rows(iVersamenti)("Acconto")) = False And CBool(ds.Tables(0).Rows(iVersamenti)("Saldo")) = True Then
                        strVersTemp = strVersTemp & "Tipo Vers: SALDO"
                    ElseIf CBool(ds.Tables(0).Rows(iVersamenti)("Acconto")) = False And CBool(ds.Tables(0).Rows(iVersamenti)("Saldo")) = True Then
                        strVersTemp = strVersTemp & "Tipo Vers: US"
                    Else
                        strVersTemp = strVersTemp & "Tipo Vers: "
                    End If


                    strVersTemp = strVersTemp & Microsoft.VisualBasic.Constants.vbTab

                    strVersTemp = strVersTemp & "Data Vers: " & objUtility.GiraDataFromDB(ds.Tables(0).Rows(iVersamenti)("DataPagamento")) & Microsoft.VisualBasic.Constants.vbCrLf

                    strVersTemp = strVersTemp & "TOTALE €: " & FormatImport(ds.Tables(0).Rows(iVersamenti)("ImportoPagato")).Replace(",", ".") & Microsoft.VisualBasic.Constants.vbTab
                    strVersTemp = strVersTemp & "ABIT PRIN €: " & FormatImport(ds.Tables(0).Rows(iVersamenti)("ImportoAbitazPrincipale")).Replace(",", ".") & Microsoft.VisualBasic.Constants.vbTab
                    strVersTemp = strVersTemp & "ALTRI FABBR €: " & FormatImport(ds.Tables(0).Rows(iVersamenti)("ImportoAltriFabbric")).Replace(",", ".") & Microsoft.VisualBasic.Constants.vbCrLf

                    strVersTemp = strVersTemp & "AREE EDIF €: " & FormatImport(ds.Tables(0).Rows(iVersamenti)("ImportoAreeFabbric")).Replace(",", ".") & Microsoft.VisualBasic.Constants.vbTab
                    strVersTemp = strVersTemp & "TERR AGR €: " & FormatImport(ds.Tables(0).Rows(iVersamenti)("ImpoTerreni")).Replace(",", ".") & Microsoft.VisualBasic.Constants.vbTab
                    strVersTemp = strVersTemp & "DETRAZ €: " & FormatImport(ds.Tables(0).Rows(iVersamenti)("DetrazioneAbitazPrincipale")).Replace(",", ".") & Microsoft.VisualBasic.Constants.vbCrLf

                    'strVersTemp = strVersTemp & "Anno: " & FormatStringToEmpty(ds.Tables(0).Rows(iVersamenti)("AnnoRiferimento")) & Microsoft.VisualBasic.Constants.vbTab

                    strVersTemp = strVersTemp & "Ravv Oper: " & BoolToStringForGridView(ds.Tables(0).Rows(iVersamenti)("RavvedimentoOperoso")) & Microsoft.VisualBasic.Constants.vbTab
                    strVersTemp = strVersTemp & "Tardivo: " & BoolToStringForGridView(ds.Tables(0).Rows(iVersamenti)("FLAG_VERSAMENTO_TARDIVO")) & Microsoft.VisualBasic.Constants.vbTab
                    strVersTemp = strVersTemp & "Giorni Ritardo: " & FormatString(ds.Tables(0).Rows(iVersamenti)("GG"))

                    strVersTemp = strVersTemp & Microsoft.VisualBasic.Constants.vbCrLf
                    strVersTemp = strVersTemp & strRiga

                Next

            Else
                strVersTemp = strRiga
                strVersTemp = strVersTemp & "Nessun Versamento." & Microsoft.VisualBasic.Constants.vbCrLf
                strVersTemp = strVersTemp & strRiga

            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.FillBookMarkVersamenti.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
        Return strVersTemp
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ds"></param>
    ''' <returns></returns>
    Private Function FillBookMarkVersamentiTARSU(ByVal ds As DataSet) As String

        Dim strRiga As String = ""
        Dim strVersTemp As String = String.Empty
        Dim iVersamenti As Integer
        Try
            If ds.Tables(0).Rows.Count > 0 Then
                'Dim blnConfigDichiarazione As Boolean = Boolean.Parse(ConfigurationManager.AppSettings("CONFIGURAZIONE_DICHIARAZIONE").ToString())
                strRiga = ""
                strRiga = strRiga.PadLeft(144, "-")
                strVersTemp = strRiga

                For iVersamenti = 0 To ds.Tables(0).Rows.Count - 1
                    strVersTemp = strVersTemp & "Anno: " & FormatString(ds.Tables(0).Rows(iVersamenti)("ANNO")) & Microsoft.VisualBasic.Constants.vbTab
                    'strVersTemp = strVersTemp & "Data Pagamento: " & objUtility.GiraDataFromDB(ds.Tables(0).Rows(iVersamenti)("DATA_PAGAMENTO")) & Microsoft.VisualBasic.Constants.vbTab
                    strVersTemp = strVersTemp & "Data Pagamento: " & ds.Tables(0).Rows(iVersamenti)("DATA_PAGAMENTO") & Microsoft.VisualBasic.Constants.vbTab
                    strVersTemp = strVersTemp & "Importo: " & FormatImport(ds.Tables(0).Rows(iVersamenti)("IMPORTO_PAGATO")) & Microsoft.VisualBasic.Constants.vbTab

                    strVersTemp = strVersTemp & Microsoft.VisualBasic.Constants.vbCrLf
                    strVersTemp = strVersTemp & strRiga
                Next
            Else
                strVersTemp = strRiga
                strVersTemp = strVersTemp & "Nessun Versamento." & Microsoft.VisualBasic.Constants.vbCrLf
                strVersTemp = strVersTemp & strRiga
            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.FillBookMarkVersamentiTARSU.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
        Return strVersTemp
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ds"></param>
    ''' <param name="ds1"></param>
    ''' <returns></returns>
    Private Function FillBookMarkTOTALEINTERESSI(ByVal ds As DataSet, ByVal ds1 As DataSet) As String

        Dim strRiga As String = ""
        Dim strIntTemp As String = String.Empty
        Dim iInteressi As Integer
        Dim Totale As Double = 0
        strRiga = ""
        strRiga = strRiga.PadLeft(124, "-")
        Try
            If ds.Tables(0).Rows.Count > 0 Then
                For iInteressi = 0 To ds.Tables(0).Rows.Count - 1
                    If CInt(ds.Tables(0).Rows(iInteressi)("N_SEMESTRI_ACC")) <> 0 Or CInt(ds.Tables(0).Rows(iInteressi)("N_SEMESTRI_SALDO")) <> 0 Then
                        Totale += ds.Tables(0).Rows(iInteressi)("IMPORTO_TOTALE_SEMESTRI")
                    End If
                    If CInt(ds.Tables(0).Rows(iInteressi)("n_giorni_saldo")) <> 0 Or CInt(ds.Tables(0).Rows(iInteressi)("n_giorni_acconto")) <> 0 Then
                        Totale += ds.Tables(0).Rows(iInteressi)("importo_totale_giorni")
                    End If
                Next
            Else
                Totale = Totale + 0
            End If
            If ds1.Tables(0).Rows.Count > 0 Then
                For iInteressi = 0 To ds1.Tables(0).Rows.Count - 1
                    If CInt(ds1.Tables(0).Rows(iInteressi)("N_SEMESTRI_SALDO")) <> 0 Or CInt(ds1.Tables(0).Rows(iInteressi)("N_SEMESTRI_ACCONTO")) <> 0 Then
                        Totale += ds1.Tables(0).Rows(iInteressi)("IMPORTO_TOTALE_SEMESTRI")
                    End If
                    If CInt(ds1.Tables(0).Rows(iInteressi)("n_giorni_saldo")) <> 0 Or CInt(ds1.Tables(0).Rows(iInteressi)("n_giorni_acconto")) <> 0 Then
                        Totale += ds1.Tables(0).Rows(iInteressi)("importo_totale_giorni")
                    End If
                Next
            Else
                Totale = Totale + 0
            End If
            strIntTemp += strRiga & Microsoft.VisualBasic.Constants.vbCrLf
            strIntTemp += "TOTALE INTERESSI" & Microsoft.VisualBasic.Constants.vbTab & Microsoft.VisualBasic.Constants.vbTab & Microsoft.VisualBasic.Constants.vbTab
            strIntTemp += FormatImport(Totale) & " €" & Microsoft.VisualBasic.Constants.vbCrLf
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.FillBookMarkTOTALEINTERESSI.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
        Return strIntTemp
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ds"></param>
    ''' <param name="ds1"></param>
    ''' <param name="COD_TRIBUTO"></param>
    ''' <returns></returns>
    Private Function FillBookMarkELENCOINTERESSI(ByVal ds As DataSet, ByVal ds1 As DataSet, ByVal COD_TRIBUTO As String) As String
        Dim strIntTemp As String = String.Empty
        Dim iInteressi As Integer
        Dim objUtility As New MyUtility
        Try
            If COD_TRIBUTO = Utility.Costanti.TRIBUTO_ICI Then
                'ICI
                If ds.Tables(0).Rows.Count > 0 Then
                    For iInteressi = 0 To ds.Tables(0).Rows.Count - 1
                        If CInt(ds.Tables(0).Rows(iInteressi)("N_SEMESTRI_ACC")) <> 0 Or CInt(ds.Tables(0).Rows(iInteressi)("N_SEMESTRI_SALDO")) <> 0 Then
                            'strIntTemp += FormatStringToEmpty(ds.Tables(0).Rows(iInteressi)("DESCRIZIONE")) & Microsoft.VisualBasic.Constants.vbTab
                            strIntTemp += "INTERESSI " & Microsoft.VisualBasic.Constants.vbTab
                            strIntTemp += " dal " & objUtility.GiraDataFromDB(ds.Tables(0).Rows(iInteressi)("data_inizio"))
                            strIntTemp += " al " & objUtility.GiraDataFromDB(ds.Tables(0).Rows(iInteressi)("data_fine")) & Microsoft.VisualBasic.Constants.vbTab
                            strIntTemp += " Tasso al " & FormatStringToEmpty(ds.Tables(0).Rows(iInteressi)("tasso")) & "%" & Microsoft.VisualBasic.Constants.vbTab
                            'strIntTemp += FormatImport(ds.Tables(0).Rows(iInteressi)("importo_totale_giorni")) & " €" & Microsoft.VisualBasic.Constants.vbCrLf
                            strIntTemp += Microsoft.VisualBasic.Constants.vbCrLf
                        End If
                        If CInt(ds.Tables(0).Rows(iInteressi)("n_giorni_saldo")) <> 0 Or CInt(ds.Tables(0).Rows(iInteressi)("n_giorni_acconto")) <> 0 Then
                            'strIntTemp += FormatStringToEmpty(ds.Tables(0).Rows(iInteressi)("DESCRIZIONE")) & Microsoft.VisualBasic.Constants.vbTab
                            strIntTemp += "INTERESSI " & Microsoft.VisualBasic.Constants.vbTab
                            strIntTemp += " dal " & objUtility.GiraDataFromDB(ds.Tables(0).Rows(iInteressi)("data_inizio"))
                            strIntTemp += " al " & objUtility.GiraDataFromDB(ds.Tables(0).Rows(iInteressi)("data_fine")) & Microsoft.VisualBasic.Constants.vbTab
                            strIntTemp += " Tasso al " & FormatStringToEmpty(ds.Tables(0).Rows(iInteressi)("tasso")) & "%" & Microsoft.VisualBasic.Constants.vbTab
                            'strIntTemp += FormatImport(ds.Tables(0).Rows(iInteressi)("importo_totale_giorni")) & " €" & Microsoft.VisualBasic.Constants.vbCrLf
                            strIntTemp += Microsoft.VisualBasic.Constants.vbCrLf
                        End If
                    Next
                ElseIf ds1.Tables(0).Rows.Count > 0 Then
                    For iInteressi = 0 To ds1.Tables(0).Rows.Count - 1
                        If CInt(ds1.Tables(0).Rows(iInteressi)("N_SEMESTRI_SALDO")) <> 0 Or CInt(ds1.Tables(0).Rows(iInteressi)("N_SEMESTRI_ACCONTO")) <> 0 Then
                            'strIntTemp += FormatStringToEmpty(ds1.Tables(0).Rows(iInteressi)("DESCRIZIONE")) & Microsoft.VisualBasic.Constants.vbTab
                            strIntTemp += "INTERESSI " & Microsoft.VisualBasic.Constants.vbTab
                            strIntTemp += " dal " & objUtility.GiraDataFromDB(ds1.Tables(0).Rows(iInteressi)("data_inizio"))
                            strIntTemp += " al " & objUtility.GiraDataFromDB(ds1.Tables(0).Rows(iInteressi)("data_fine")) & Microsoft.VisualBasic.Constants.vbTab
                            strIntTemp += " Tasso al " & FormatStringToEmpty(ds1.Tables(0).Rows(iInteressi)("tasso")) & "%" & Microsoft.VisualBasic.Constants.vbTab
                            'strIntTemp += FormatImport(ds1.Tables(0).Rows(iInteressi)("importo_totale_giorni")) & " €" & Microsoft.VisualBasic.Constants.vbCrLf
                            strIntTemp += Microsoft.VisualBasic.Constants.vbCrLf
                        End If

                        If CInt(ds1.Tables(0).Rows(iInteressi)("n_giorni_saldo")) <> 0 Or CInt(ds1.Tables(0).Rows(iInteressi)("n_giorni_acconto")) <> 0 Then
                            'strIntTemp += FormatStringToEmpty(ds1.Tables(0).Rows(iInteressi)("DESCRIZIONE")) & Microsoft.VisualBasic.Constants.vbTab
                            strIntTemp += "INTERESSI " & Microsoft.VisualBasic.Constants.vbTab
                            strIntTemp += " dal " & objUtility.GiraDataFromDB(ds1.Tables(0).Rows(iInteressi)("data_inizio"))
                            strIntTemp += " al " & objUtility.GiraDataFromDB(ds1.Tables(0).Rows(iInteressi)("data_fine")) & Microsoft.VisualBasic.Constants.vbTab
                            strIntTemp += " Tasso al " & FormatStringToEmpty(ds1.Tables(0).Rows(iInteressi)("tasso")) & "%" & Microsoft.VisualBasic.Constants.vbTab
                            'strIntTemp += FormatImport(ds1.Tables(0).Rows(iInteressi)("importo_totale_giorni")) & " €" & Microsoft.VisualBasic.Constants.vbCrLf
                            strIntTemp += Microsoft.VisualBasic.Constants.vbCrLf
                        End If
                    Next
                Else
                    strIntTemp = "Nessuna Tipologia di Interessi Configurata." & Microsoft.VisualBasic.Constants.vbCrLf
                End If
                '*** 20130801 - accertamento OSAP ***
            Else      'If COD_TRIBUTO = "0434" Then
                'TARSU/OSAP
                If ds.Tables(0).Rows.Count > 0 Then
                    For iInteressi = 0 To ds.Tables(0).Rows.Count - 1
                        If CInt(ds.Tables(0).Rows(iInteressi)("IMPORTO_TOTALE_SEMESTRI")) <> 0 Or CInt(ds.Tables(0).Rows(iInteressi)("IMPORTO_TOTALE_GIORNI")) <> 0 Then
                            strIntTemp += "INTERESSI " & Microsoft.VisualBasic.Constants.vbTab
                            strIntTemp += " dal " & objUtility.GiraDataFromDB(ds.Tables(0).Rows(iInteressi)("data_inizio"))
                            strIntTemp += " al " & objUtility.GiraDataFromDB(ds.Tables(0).Rows(iInteressi)("data_fine")) & Microsoft.VisualBasic.Constants.vbTab
                            strIntTemp += " Tasso al " & FormatStringToEmpty(ds.Tables(0).Rows(iInteressi)("tasso")) & "%" & Microsoft.VisualBasic.Constants.vbTab
                            strIntTemp += Microsoft.VisualBasic.Constants.vbCrLf
                        End If
                    Next
                ElseIf ds1.Tables(0).Rows.Count > 0 Then
                    For iInteressi = 0 To ds1.Tables(0).Rows.Count - 1
                        If CDbl(ds1.Tables(0).Rows(iInteressi)("IMPORTO_TOTALE_SEMESTRI")) <> 0 Or CDbl(ds1.Tables(0).Rows(iInteressi)("IMPORTO_TOTALE_GIORNI")) <> 0 Then
                            strIntTemp += "INTERESSI " & Microsoft.VisualBasic.Constants.vbTab
                            strIntTemp += " dal " & objUtility.GiraDataFromDB(ds1.Tables(0).Rows(iInteressi)("data_inizio"))
                            strIntTemp += " al " & objUtility.GiraDataFromDB(ds1.Tables(0).Rows(iInteressi)("data_fine")) & Microsoft.VisualBasic.Constants.vbTab
                            strIntTemp += " Tasso al " & FormatStringToEmpty(ds1.Tables(0).Rows(iInteressi)("tasso")) & "%" & Microsoft.VisualBasic.Constants.vbTab
                            strIntTemp += Microsoft.VisualBasic.Constants.vbCrLf
                        End If
                    Next
                Else
                    strIntTemp = "Nessuna Tipologia di Interessi Configurata." & Microsoft.VisualBasic.Constants.vbCrLf
                End If
            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.FillBookMarkELENCOINTERESSI.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
        Return strIntTemp
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ds"></param>
    ''' <returns></returns>
    Private Function FillBookMarkELENCOINTERESSITARSU(ByVal ds As DataSet) As String
        Dim strIntTemp As String = String.Empty
        Dim iInteressi As Integer
        Dim objUtility As New MyUtility
        Try
            If ds.Tables(0).Rows.Count > 0 Then
                For iInteressi = 0 To ds.Tables(0).Rows.Count - 1

                    strIntTemp += FormatStringToEmpty(ds.Tables(0).Rows(iInteressi)("DESCRIZIONE")) & Microsoft.VisualBasic.Constants.vbTab
                    strIntTemp += "Dal: " & objUtility.GiraDataFromDB((ds.Tables(0).Rows(iInteressi)("DAL"))) & Microsoft.VisualBasic.Constants.vbTab
                    If Not IsDBNull(ds.Tables(0).Rows(iInteressi)("AL")) Then
                        strIntTemp += "Al: " & objUtility.GiraDataFromDB((ds.Tables(0).Rows(iInteressi)("AL"))) & Microsoft.VisualBasic.Constants.vbTab
                    Else
                        strIntTemp += "" & Microsoft.VisualBasic.Constants.vbTab
                    End If
                    strIntTemp += " Tasso al " & ds.Tables(0).Rows(iInteressi)("TASSO_ANNUALE") & "%" & Microsoft.VisualBasic.Constants.vbCrLf

                Next
            Else
                strIntTemp = "Nessuna Tipologia di Interessi Configurata." & Microsoft.VisualBasic.Constants.vbCrLf
            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.FillBookMarkELENCOINTERESSITARSU.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
        Return strIntTemp
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ds"></param>
    ''' <param name="ImportoTotaleAddizionali"></param>
    ''' <returns></returns>
    Private Function FillBookMarkELENCOADDIZIONALI(ByVal ds As DataSet, ByRef ImportoTotaleAddizionali As Double) As String
        Dim strIntTemp As String = String.Empty
        Dim iAddizionali As Integer

        'Tassa	                 €     [SOMMA ALGEBRICA DELLE DIFF. DI IMPOSTA]
        'Addizionale ex- Eca 5%	 €     
        'Maggiorazione ex-Eca 5% €     [IN BASE ALLE CONFIGURAZIONI]
        'Tributo Provinciale 1%  €     
        'Sanzione Amministrativa €     [SOMMA ALGEBRICA DELLE SANZIONI]
        'Interessi 				 €     [SOMMA ALGEBRICA DEGLI INTERESSI]
        'Spese di notifica       €     [SPESE DI NOTIFICA]
        'Arrotondamento          €     [ARROTONDAMENTO]
        'Totale                  €     [IMPORTO TOTALE AVVISO]
        ImportoTotaleAddizionali = 0
        Try
            If ds.Tables(0).Rows.Count > 0 Then
                For iAddizionali = 0 To ds.Tables(0).Rows.Count - 1
                    If iAddizionali > 0 Then
                        strIntTemp = strIntTemp & Microsoft.VisualBasic.Constants.vbCrLf
                    End If
                    'strIntTemp = strIntTemp & FormatStringToEmpty(LCase(ds.Tables(0).Rows(iAddizionali)("DESCRIZIONE")) & " " & ds.Tables(0).Rows(iAddizionali)("VALORE") & " %") & Microsoft.VisualBasic.Constants.vbTab
                    strIntTemp = strIntTemp & FormatStringToEmpty(ds.Tables(0).Rows(iAddizionali)("DESCRIZIONE")) & Microsoft.VisualBasic.Constants.vbTab
                    'strIntTemp = strIntTemp & "€     " & FormatImport(ds.Tables(0).Rows(iAddizionali)("IMPORTO"))
                    strIntTemp = strIntTemp & FormatImport(ds.Tables(0).Rows(iAddizionali)("IMPORTO")) & " €"
                    ImportoTotaleAddizionali += CDbl(ds.Tables(0).Rows(iAddizionali)("IMPORTO"))
                    ''If Not IsDBNull(ds.Tables(0).Rows(iAddizionali)("AL")) Then
                    ''    strIntTemp = strIntTemp & "Al: " & objUtility.GiraDataFromDB((ds.Tables(0).Rows(iAddizionali)("AL"))) & Microsoft.VisualBasic.Constants.vbTab
                    ''Else
                    ''    strIntTemp = strIntTemp & "" & Microsoft.VisualBasic.Constants.vbTab
                    ''End If
                    ''strIntTemp = strIntTemp & ds.Tables(0).Rows(iAddizionali)("TASSO_ANNUALE") & "%" & Microsoft.VisualBasic.Constants.vbCrLf
                Next
            Else
                'strIntTemp = "Nessun Addizionale Configurato." & Microsoft.VisualBasic.Constants.vbCrLf
                strIntTemp = "" '"Addizionale" & Microsoft.VisualBasic.Constants.vbTab & "0,00 €" & Microsoft.VisualBasic.Constants.vbCrLf
            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.FillBookMarkELENCOADDIZIONALI.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
        Return strIntTemp
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ds"></param>
    ''' <returns></returns>
    Private Function FillBookMarkSanzioniRiducibili(ByVal ds As DataSet) As String
        Dim strSanzTemp As String = String.Empty
        Dim iSanzioni As Integer
        Dim totale As Decimal = 0
        Try
            If ds.Tables(0).Rows.Count > 0 Then

                For iSanzioni = 0 To ds.Tables(0).Rows.Count - 1

                    totale = totale + CDbl(ds.Tables(0).Rows(iSanzioni)("TOT_IMPORTO_SANZ"))

                Next

                strSanzTemp = CStr(totale)


            Else
                strSanzTemp = "0"
            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.FillBookMarkSanzioniRiducibili.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
        Return strSanzTemp
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ds"></param>
    ''' <param name="dsF2"></param>
    ''' <param name="dsRid"></param>
    ''' <param name="dsF2Rid"></param>
    ''' <param name="dsF2Intr"></param>
    ''' <returns></returns>
    Private Function FillBookMarkELENCOSANZIONI(ByVal ds As DataSet, ByVal dsF2 As DataSet, ByVal dsRid As DataSet, ByVal dsF2Rid As DataSet, ByVal dsF2Intr As DataSet) As String

        Dim strRiga As String = ""
        Dim strSanzTemp As String = String.Empty
        Dim iSanzioni As Integer

        strRiga = ""
        strRiga = strRiga.PadLeft(124, "-")
        Try
            If ds.Tables(0).Rows.Count > 0 Or dsF2.Tables(0).Rows.Count > 0 Or dsRid.Tables(0).Rows.Count > 0 Or dsF2Rid.Tables(0).Rows.Count > 0 Or dsF2Intr.Tables(0).Rows.Count > 0 Then
                'Non riducibili
                For iSanzioni = 0 To ds.Tables(0).Rows.Count - 1
                    strSanzTemp += "Dati Catastali" & Microsoft.VisualBasic.Constants.vbCrLf
                    strSanzTemp += "Foglio: " & FormatStringToEmpty(ds.Tables(0).Rows(iSanzioni)("FOGLIO")) & Microsoft.VisualBasic.Constants.vbTab
                    strSanzTemp += "Numero: " & FormatStringToEmpty(ds.Tables(0).Rows(iSanzioni)("NUMERO")) & Microsoft.VisualBasic.Constants.vbTab
                    strSanzTemp += "Subalterno: " & FormatStringToEmpty(ds.Tables(0).Rows(iSanzioni)("SUBALTERNO")) & Microsoft.VisualBasic.Constants.vbCrLf
                    strSanzTemp += FormatStringToEmpty(ds.Tables(0).Rows(iSanzioni)("DESCRIZIONE_VOCE_ATTRIBUITA")) & Microsoft.VisualBasic.Constants.vbTab
                    strSanzTemp += FormatImport(ds.Tables(0).Rows(iSanzioni)("TOT_IMPORTO_SANZ")) & " €" & Microsoft.VisualBasic.Constants.vbCrLf
                    If FormatStringToEmpty(ds.Tables(0).Rows(iSanzioni)("MOTIVAZIONE")) <> "" Then
                        strSanzTemp += "Motivazione:" & Microsoft.VisualBasic.Constants.vbCrLf
                        strSanzTemp += Up(FormatStringToEmpty(ds.Tables(0).Rows(iSanzioni)("MOTIVAZIONE"))) & Microsoft.VisualBasic.Constants.vbCrLf
                    End If
                    strSanzTemp += strRiga & Microsoft.VisualBasic.Constants.vbCrLf
                Next

                For iSanzioni = 0 To dsF2.Tables(0).Rows.Count - 1
                    strSanzTemp += FormatStringToEmpty(dsF2.Tables(0).Rows(iSanzioni)("DESCRIZIONE_VOCE_ATTRIBUITA")) & Microsoft.VisualBasic.Constants.vbTab
                    strSanzTemp += FormatImport(dsF2.Tables(0).Rows(iSanzioni)("TOT_IMPORTO_SANZ")) & " €" & Microsoft.VisualBasic.Constants.vbCrLf
                    strSanzTemp += "Motivazione:" & Microsoft.VisualBasic.Constants.vbCrLf
                    strSanzTemp += Up(FormatStringToEmpty(dsF2.Tables(0).Rows(iSanzioni)("DESCRIZIONE_MOTIVAZIONE"))) & Microsoft.VisualBasic.Constants.vbCrLf
                    strSanzTemp += strRiga & Microsoft.VisualBasic.Constants.vbCrLf
                Next

                'Riducibili
                For iSanzioni = 0 To dsRid.Tables(0).Rows.Count - 1
                    strSanzTemp += "Dati Catastali" & Microsoft.VisualBasic.Constants.vbCrLf
                    strSanzTemp += "Foglio: " & FormatStringToEmpty(dsRid.Tables(0).Rows(iSanzioni)("FOGLIO")) & Microsoft.VisualBasic.Constants.vbTab
                    strSanzTemp += "Numero: " & FormatStringToEmpty(dsRid.Tables(0).Rows(iSanzioni)("NUMERO")) & Microsoft.VisualBasic.Constants.vbTab
                    strSanzTemp += "Subalterno: " & FormatStringToEmpty(dsRid.Tables(0).Rows(iSanzioni)("SUBALTERNO")) & Microsoft.VisualBasic.Constants.vbCrLf
                    strSanzTemp += FormatStringToEmpty(dsRid.Tables(0).Rows(iSanzioni)("DESCRIZIONE_VOCE_ATTRIBUITA")) & Microsoft.VisualBasic.Constants.vbTab
                    strSanzTemp += FormatImport(dsRid.Tables(0).Rows(iSanzioni)("TOT_IMPORTO_SANZ")) & " €" & Microsoft.VisualBasic.Constants.vbCrLf
                    If FormatStringToEmpty(dsRid.Tables(0).Rows(iSanzioni)("MOTIVAZIONE")) <> "" Then
                        strSanzTemp += "Motivazione:" & Microsoft.VisualBasic.Constants.vbCrLf
                        strSanzTemp += Up(FormatStringToEmpty(dsRid.Tables(0).Rows(iSanzioni)("MOTIVAZIONE"))) & Microsoft.VisualBasic.Constants.vbCrLf
                    End If
                    strSanzTemp += strRiga & Microsoft.VisualBasic.Constants.vbCrLf
                Next

                For iSanzioni = 0 To dsF2Rid.Tables(0).Rows.Count - 1
                    strSanzTemp += FormatStringToEmpty(dsF2Rid.Tables(0).Rows(iSanzioni)("DESCRIZIONE_VOCE_ATTRIBUITA")) & Microsoft.VisualBasic.Constants.vbTab
                    strSanzTemp += FormatImport(dsF2Rid.Tables(0).Rows(iSanzioni)("TOT_IMPORTO_SANZ")) & " €" & Microsoft.VisualBasic.Constants.vbCrLf
                    strSanzTemp += "Motivazione:" & Microsoft.VisualBasic.Constants.vbCrLf
                    strSanzTemp += Up(FormatStringToEmpty(dsF2Rid.Tables(0).Rows(iSanzioni)("DESCRIZIONE_MOTIVAZIONE"))) & Microsoft.VisualBasic.Constants.vbCrLf
                    strSanzTemp += strRiga & Microsoft.VisualBasic.Constants.vbCrLf
                Next

                For iSanzioni = 0 To dsF2Intr.Tables(0).Rows.Count - 1
                    strSanzTemp += FormatStringToEmpty(dsF2Intr.Tables(0).Rows(iSanzioni)("DESCRIZIONE_VOCE_ATTRIBUITA")) & Microsoft.VisualBasic.Constants.vbTab
                    strSanzTemp += FormatImport(dsF2Intr.Tables(0).Rows(iSanzioni)("TOT_IMPORTO_SANZ")) & " €" & Microsoft.VisualBasic.Constants.vbCrLf
                    strSanzTemp += "Motivazione:" & Microsoft.VisualBasic.Constants.vbCrLf
                    strSanzTemp += Up(FormatStringToEmpty(dsF2Intr.Tables(0).Rows(iSanzioni)("DESCRIZIONE_MOTIVAZIONE"))) & Microsoft.VisualBasic.Constants.vbCrLf
                    strSanzTemp += strRiga & Microsoft.VisualBasic.Constants.vbCrLf
                Next
            Else
                strSanzTemp = "Nessuna Tipologia di Sanzione Applicata." & Microsoft.VisualBasic.Constants.vbCrLf
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.FillBookMarkELENCOSANZIONI.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
        Return strSanzTemp
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ds"></param>
    ''' <param name="dsF2"></param>
    ''' <param name="ImportoTotaleSanzioni"></param>
    ''' <returns></returns>
    Private Function FillBookMarkELENCOSANZIONITARSU(ByVal ds As DataSet, ByVal dsF2 As DataSet, ByRef ImportoTotaleSanzioni As Double) As String
        Dim strSanzTemp As String = String.Empty
        Dim iSanzioni As Integer
        Try
            If ds.Tables(0).Rows.Count > 0 Or dsF2.Tables(0).Rows.Count > 0 Then

                For iSanzioni = 0 To ds.Tables(0).Rows.Count - 1

                    strSanzTemp = strSanzTemp & FormatStringToEmpty(ds.Tables(0).Rows(iSanzioni)("DESCRIZIONE_VOCE_ATTRIBUITA")) & Microsoft.VisualBasic.Constants.vbTab
                    strSanzTemp = strSanzTemp & FormatImport(ds.Tables(0).Rows(iSanzioni)("TOT_IMPORTO_SANZ")) & " €" & Microsoft.VisualBasic.Constants.vbCrLf
                    ImportoTotaleSanzioni += CDbl(ds.Tables(0).Rows(iSanzioni)("TOT_IMPORTO_SANZ"))
                    If FormatStringToEmpty(ds.Tables(0).Rows(iSanzioni)("MOTIVAZIONE")) <> "" Then
                        strSanzTemp += "Motivazione:" & Microsoft.VisualBasic.Constants.vbCrLf
                        strSanzTemp += Up(FormatStringToEmpty(ds.Tables(0).Rows(iSanzioni)("MOTIVAZIONE"))) & Microsoft.VisualBasic.Constants.vbCrLf
                    End If
                Next

                For iSanzioni = 0 To dsF2.Tables(0).Rows.Count - 1

                    strSanzTemp = strSanzTemp & FormatStringToEmpty(dsF2.Tables(0).Rows(iSanzioni)("DESCRIZIONE_VOCE_ATTRIBUITA")) & Microsoft.VisualBasic.Constants.vbTab
                    strSanzTemp = strSanzTemp & FormatImport(dsF2.Tables(0).Rows(iSanzioni)("TOT_IMPORTO_SANZ")) & " €" & Microsoft.VisualBasic.Constants.vbCrLf
                    ImportoTotaleSanzioni += CDbl(ds.Tables(0).Rows(iSanzioni)("TOT_IMPORTO_SANZ"))
                    If FormatStringToEmpty(dsF2.Tables(0).Rows(iSanzioni)("DESCRIZIONE_MOTIVAZIONE")) <> "" Then
                        strSanzTemp += "Motivazione:" & Microsoft.VisualBasic.Constants.vbCrLf
                        strSanzTemp += Up(FormatStringToEmpty(dsF2.Tables(0).Rows(iSanzioni)("DESCRIZIONE_MOTIVAZIONE"))) & Microsoft.VisualBasic.Constants.vbCrLf
                    End If
                Next
            Else
                strSanzTemp = "Nessuna Tipologia di Sanzione Applicata." & Microsoft.VisualBasic.Constants.vbCrLf
            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.FillBookMarkELENCOSANZIONITARSU.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
        Return strSanzTemp
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ds"></param>
    ''' <returns></returns>
    Private Function FillBookMarkELENCOVIOLAZIONI(ByVal ds As DataSet) As String
        Dim strSanzTemp As String = String.Empty
        Dim iSanzioni As Integer
        Try
            If ds.Tables(0).Rows.Count > 0 Then

                For iSanzioni = 0 To ds.Tables(0).Rows.Count - 1

                    If Not ds.Tables(0).Rows(iSanzioni)("VALORE") Is DBNull.Value And ds.Tables(0).Rows(iSanzioni)("VALORE") <> "" Then
                        strSanzTemp += FormatStringToEmpty(ds.Tables(0).Rows(iSanzioni)("DESCRIZIONE_VOCE_ATTRIBUITA"))
                        strSanzTemp += " irrogando la sanzione del "
                        strSanzTemp += ds.Tables(0).Rows(iSanzioni)("VALORE") & " %" & Microsoft.VisualBasic.Constants.vbCrLf
                        If FormatStringToEmpty(ds.Tables(0).Rows(iSanzioni)("MOTIVAZIONE")) <> "" Then
                            strSanzTemp += "Motivazione: "
                            strSanzTemp += Up(FormatStringToEmpty(ds.Tables(0).Rows(iSanzioni)("MOTIVAZIONE"))) & Microsoft.VisualBasic.Constants.vbCrLf
                        End If

                    End If
                Next
            Else
                strSanzTemp = "Nessuna Violazione Applicata." & Microsoft.VisualBasic.Constants.vbCrLf
            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.FillBookMarkACCERTATO.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
        Return strSanzTemp
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ds"></param>
    ''' <param name="annoAcc"></param>
    ''' <param name="Tributo"></param>
    ''' <returns></returns>
    Private Function FillBookMarkACCERTATO(ByVal ds As DataSet, ByVal annoAcc As Integer, Tributo As String) As String
        Dim strRiga As String = "" = ""
        Dim strImmoTemp As String = String.Empty
        Dim iImmobili As Integer
        Dim objUtility As New MyUtility

        Try
            If ds.Tables(0).Rows.Count > 0 Then
                strRiga = ""
                strRiga = strRiga.PadLeft(144, "-") & Microsoft.VisualBasic.Constants.vbCrLf
                strImmoTemp = strRiga

                For iImmobili = 0 To ds.Tables(0).Rows.Count - 1
                    If (Year(ds.Tables(0).Rows(iImmobili)("DATAINIZIO")) < annoAcc) Then
                        strImmoTemp = strImmoTemp & "Dal: 01/01/" & annoAcc & Microsoft.VisualBasic.Constants.vbTab
                    Else
                        strImmoTemp = strImmoTemp & "Dal: " & objUtility.CToStr(ds.Tables(0).Rows(iImmobili)("DATAINIZIO")) & Microsoft.VisualBasic.Constants.vbTab
                    End If

                    If Not IsDBNull(ds.Tables(0).Rows(iImmobili)("DATAFINE")) Then
                        If (Year(ds.Tables(0).Rows(iImmobili)("DATAFINE")) = "9999") Then
                            strImmoTemp = strImmoTemp & "Al:"
                        ElseIf (Year(ds.Tables(0).Rows(iImmobili)("DATAFINE")) > annoAcc) Then
                            strImmoTemp = strImmoTemp & "Al: 31/12/" & annoAcc & Microsoft.VisualBasic.Constants.vbTab
                        Else
                            strImmoTemp = strImmoTemp & "Al: " & objUtility.CToStr(ds.Tables(0).Rows(iImmobili)("DATAFINE")) & Microsoft.VisualBasic.Constants.vbTab
                        End If
                    Else
                        strImmoTemp = strImmoTemp & "Al:"
                    End If

                    strImmoTemp = strImmoTemp & Microsoft.VisualBasic.Constants.vbCrLf


                    strImmoTemp = strImmoTemp & "Tipo Rendita/Valore: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("DescrTipoImmobile")) & Microsoft.VisualBasic.Constants.vbCrLf
                    'strImmoTemp = strImmoTemp & "Tipologia Immobile: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("DescrTipoImmobile")) & Microsoft.VisualBasic.Constants.vbCrLf

                    strImmoTemp = strImmoTemp & "Ubicazione: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("Via")) & " " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("NumeroCivico")) & Microsoft.VisualBasic.Constants.vbCrLf
                    strImmoTemp = strImmoTemp & "Foglio: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("FOGLIO")) & Microsoft.VisualBasic.Constants.vbTab
                    strImmoTemp = strImmoTemp & "Numero: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("NUMERO")) & Microsoft.VisualBasic.Constants.vbTab
                    strImmoTemp = strImmoTemp & "Subalterno: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("SUBALTERNO")) & Microsoft.VisualBasic.Constants.vbTab
                    strImmoTemp = strImmoTemp & "Categoria: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("CODCATEGORIACATASTALE")) & Microsoft.VisualBasic.Constants.vbTab
                    strImmoTemp = strImmoTemp & "Classe: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("CODCLASSE")) & Microsoft.VisualBasic.Constants.vbCrLf

                    strImmoTemp = strImmoTemp & "Rendita: " & FormatImport(ds.Tables(0).Rows(iImmobili)("rendita")) & " €" & Microsoft.VisualBasic.Constants.vbTab
                    strImmoTemp = strImmoTemp & "Valore: " & FormatImport(ds.Tables(0).Rows(iImmobili)("ValoreImmobile")) & " €" & Microsoft.VisualBasic.Constants.vbCrLf
                    strImmoTemp = strImmoTemp & "Tipo Possesso: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("DescTipoPossesso")) & Microsoft.VisualBasic.Constants.vbTab
                    strImmoTemp = strImmoTemp & "Perc. Possesso: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("PERCPOSSESSO")).Replace(",", ".") & "%" & Microsoft.VisualBasic.Constants.vbCrLf
                    '*** 20130620 - richieste comune ***
                    If Not IsDBNull(ds.Tables(0).Rows(iImmobili)("ICI_VALORE_ALIQUOTA")) Then
                        strImmoTemp = strImmoTemp & "Aliquota: " & FormatImport(ds.Tables(0).Rows(iImmobili)("ICI_VALORE_ALIQUOTA")) & Microsoft.VisualBasic.Constants.vbTab
                    End If
                    '*** ***
                    strImmoTemp = strImmoTemp & "Importo Dovuto: " & FormatImport(ds.Tables(0).Rows(iImmobili)("IMPORTO_TOTALE_ICI_DOVUTO")) & " €"

                    If FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("TIPO_RENDITA")) = "AF" Then
                        strImmoTemp = strImmoTemp & Microsoft.VisualBasic.Constants.vbCrLf
                        strImmoTemp = strImmoTemp & "Zona: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("ZONA")) & Microsoft.VisualBasic.Constants.vbTab
                        strImmoTemp = strImmoTemp & "Valore al Mq: " & FormatImport(ds.Tables(0).Rows(iImmobili)("tariffa_euro")) & " €" & Microsoft.VisualBasic.Constants.vbTab
                        '*** 20130620 - richieste comune ***
                        strImmoTemp = strImmoTemp & "Mq: " & FormatImport(ds.Tables(0).Rows(iImmobili)("consistenza")) & " €" & Microsoft.VisualBasic.Constants.vbTab
                        '*** ***
                    End If

                    If Not IsDBNull(ds.Tables(0).Rows(iImmobili)("abitazioneprincipaleattuale")) Then
                        If ds.Tables(0).Rows(iImmobili)("abitazioneprincipaleattuale") <> 0 Then
                            strImmoTemp = strImmoTemp & Microsoft.VisualBasic.Constants.vbCrLf
                            strImmoTemp = strImmoTemp & "Abitazione Principale" & Microsoft.VisualBasic.Constants.vbTab
                            If Not IsDBNull(ds.Tables(0).Rows(iImmobili)("ici_totale_detrazione_applicata")) Then
                                If ds.Tables(0).Rows(iImmobili)("ici_totale_detrazione_applicata") <> 0 Then
                                    strImmoTemp = strImmoTemp & "Detrazione applicata: " & FormatImport(ds.Tables(0).Rows(iImmobili)("ici_totale_detrazione_applicata")) & " €"
                                End If
                            End If
                        End If
                    End If
                    If Not IsDBNull(ds.Tables(0).Rows(iImmobili)("idImmobilePertinente")) Then
                        If ds.Tables(0).Rows(iImmobili)("idImmobilePertinente") > 0 Then
                            strImmoTemp = strImmoTemp & Microsoft.VisualBasic.Constants.vbCrLf
                            strImmoTemp = strImmoTemp & "Pertinenza" & Microsoft.VisualBasic.Constants.vbTab
                        End If
                    End If
                    strImmoTemp = strImmoTemp & Microsoft.VisualBasic.Constants.vbCrLf
                    If Tributo = Utility.Costanti.TRIBUTO_TASI Then
                        If Not IsDBNull(ds.Tables(0).Rows(iImmobili)("DESCRTIPOTASI")) Then
                            If ds.Tables(0).Rows(iImmobili)("DESCRTIPOTASI") <> "" Then
                                strImmoTemp = strImmoTemp & ds.Tables(0).Rows(iImmobili)("DESCRTIPOTASI")
                                strImmoTemp = strImmoTemp & Microsoft.VisualBasic.Constants.vbCrLf
                            End If
                        End If
                    End If
                    strImmoTemp = strImmoTemp & strRiga
                Next
            Else
                'impossibile che succeda
                strImmoTemp = strRiga
                strImmoTemp = "Nessun immobile." & Microsoft.VisualBasic.Constants.vbCrLf
                strImmoTemp = strImmoTemp & strRiga
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.FillBookMarkACCERTATO.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
        Return strImmoTemp
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ds"></param>
    ''' <param name="sAcconto"></param>
    ''' <param name="sSaldo"></param>
    ''' <returns></returns>
    Private Function FillBookMarkIMPORTODOVACC(ByVal ds As DataSet, ByRef sAcconto As Double, ByRef sSaldo As Double) As String

        Dim strRiga As String = ""
        Dim strImmoTemp As String = String.Empty
        Dim iImmobili As Integer
        strRiga = ""


        sAcconto = 0
        sSaldo = 0
        Try
            If ds.Tables(0).Rows.Count > 0 Then
                strImmoTemp = strRiga

                For iImmobili = 0 To ds.Tables(0).Rows.Count - 1

                    sAcconto += CDbl(ds.Tables(0).Rows(iImmobili)("importo_totale_ici_acconto_dovuto"))
                    sSaldo += CDbl(ds.Tables(0).Rows(iImmobili)("importo_totale_ici_saldo_dovuto"))


                Next

            Else
                'impossibile che succeda
                strRiga = strRiga.PadLeft(148, "-") & Microsoft.VisualBasic.Constants.vbCrLf
                strImmoTemp = strRiga
                strImmoTemp = "Nessun immobile Accertato." & Microsoft.VisualBasic.Constants.vbCrLf
                strImmoTemp = strImmoTemp & strRiga
            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.FillBookMarkIMPORTODOVACC.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
        Return strImmoTemp
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ds"></param>
    ''' <returns></returns>
    Private Function FillBookMarkACCERTATOtarsu(ByVal ds As DataSet) As String

        Dim strRiga As String = ""
        Dim strImmoTemp As String = String.Empty
        Dim iImmobili As Integer
        Try

            If ds.Tables(0).Rows.Count > 0 Then
                strRiga = ""
                strRiga = strRiga.PadLeft(120, "-")
                strImmoTemp = strRiga

                For iImmobili = 0 To ds.Tables(0).Rows.Count - 1
                    strImmoTemp = strImmoTemp & Microsoft.VisualBasic.Constants.vbCrLf
                    strImmoTemp = strImmoTemp & "Ubicazione: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("Via")) & " " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("Civico")) & " " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("Interno")) & Microsoft.VisualBasic.Constants.vbTab
                    strImmoTemp = strImmoTemp & "Categoria: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("IDCATEGORIA")) & " - " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("descrizione")) & Microsoft.VisualBasic.Constants.vbTab


                    '*** 20112008 Fabi modifica visualizzazione delle tariffe -> solo 2 cifre decimali
                    strImmoTemp = strImmoTemp & "Tariffa: " & CStr(FormatNumber(ds.Tables(0).Rows(iImmobili)("IMPORTO_TARIFFA"), 2)) & Microsoft.VisualBasic.Constants.vbCrLf

                    strImmoTemp = strImmoTemp & "Mq: " & FormatImport(ds.Tables(0).Rows(iImmobili)("MQ")) & Microsoft.VisualBasic.Constants.vbTab & "Bimestri: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("bimestri")) & Microsoft.VisualBasic.Constants.vbTab & "Importo: " & FormatImport(ds.Tables(0).Rows(iImmobili)("IMPORTO_NETTO"))

                    strImmoTemp = strImmoTemp & Microsoft.VisualBasic.Constants.vbCrLf
                    strImmoTemp = strImmoTemp & strRiga

                Next

            Else
                strImmoTemp = strRiga
                strImmoTemp = "Nessun Immobile Accertato." & Microsoft.VisualBasic.Constants.vbCrLf
                strImmoTemp = strImmoTemp & strRiga
            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.FillBookMarkACCERTATOtarsu.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
        Return strImmoTemp
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ds"></param>
    ''' <param name="dsF2"></param>
    ''' <param name="ImportoGiorni"></param>
    ''' <param name="ImportoSemestriACC"></param>
    ''' <param name="ImportoSemestriSAL"></param>
    ''' <param name="NumSemestriACC"></param>
    ''' <param name="NumSemestriSAL"></param>
    ''' <returns></returns>
    Private Function FillBookMarkIMPORTIINTERESSI(ByVal ds As DataSet, ByVal dsF2 As DataSet, ByRef ImportoGiorni As String, ByRef ImportoSemestriACC As String, ByRef ImportoSemestriSAL As String, ByRef NumSemestriACC As String, ByRef NumSemestriSAL As String) As Boolean
        Dim iInteressi As Integer
        Dim ImportoGiorni_A, ImportoSemestriACC_A, ImportoSemestriSAL_A, NumSemestriACC_A, NumSemestriSAL_A As String
        Dim ImportoGiorni_F2, ImportoSemestriACC_F2, ImportoSemestriSAL_F2, NumSemestriACC_F2, NumSemestriSAL_F2 As String

        Try

            If ds.Tables(0).Rows.Count > 0 Or dsF2.Tables(0).Rows.Count > 0 Then
                'deve restituire sempre al max un record perchè la query
                'prevede un raggruppamento con somme
                For iInteressi = 0 To ds.Tables(0).Rows.Count - 1

                    ImportoGiorni_A = FormatImport(ds.Tables(0).Rows(iInteressi)("IMPORTO_TOTALE_GIORNI"))
                    ImportoSemestriACC_A = FormatImport(ds.Tables(0).Rows(iInteressi)("IMPORTO_ACC_SEMESTRI"))
                    ImportoSemestriSAL_A = FormatImport(ds.Tables(0).Rows(iInteressi)("IMPORTO_SALDO_SEMESTRI"))
                    NumSemestriACC_A = FormatNumberToZero(ds.Tables(0).Rows(iInteressi)("N_SEMESTRI_ACC"))
                    NumSemestriSAL_A = FormatNumberToZero(ds.Tables(0).Rows(iInteressi)("N_SEMESTRI_SALDO"))

                Next

                For iInteressi = 0 To dsF2.Tables(0).Rows.Count - 1

                    ImportoGiorni_F2 = FormatImport(dsF2.Tables(0).Rows(iInteressi)("IMPORTO_TOTALE_GIORNI"))
                    ImportoSemestriACC_F2 = FormatImport(dsF2.Tables(0).Rows(iInteressi)("IMPORTO_ACC_SEMESTRI"))
                    ImportoSemestriSAL_F2 = FormatImport(dsF2.Tables(0).Rows(iInteressi)("IMPORTO_SALDO_SEMESTRI"))
                    NumSemestriACC_F2 = FormatNumberToZero(dsF2.Tables(0).Rows(iInteressi)("N_SEMESTRI_ACC"))
                    NumSemestriSAL_F2 = FormatNumberToZero(dsF2.Tables(0).Rows(iInteressi)("N_SEMESTRI_SALDO"))

                Next


                ImportoGiorni = FormatImport(CDbl(ImportoGiorni_A) + CDbl(ImportoGiorni_F2))
                ImportoSemestriACC = FormatImport(CDbl(ImportoSemestriACC_A) + CDbl(ImportoSemestriACC_F2))
                ImportoSemestriSAL = FormatImport(CDbl(ImportoSemestriSAL_A) + CDbl(ImportoSemestriSAL_F2))
                NumSemestriACC = FormatImport(CDbl(NumSemestriACC_A) + CDbl(NumSemestriACC_F2))
                NumSemestriSAL = FormatImport(CDbl(NumSemestriSAL_A) + CDbl(NumSemestriSAL_F2))

            Else

                ImportoGiorni = "0"
                ImportoSemestriACC = "0"
                ImportoSemestriSAL = "0"
                NumSemestriACC = "0"
                NumSemestriSAL = "0"

            End If

            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.FillBookMarkIMPORTOINTERESSI.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ds"></param>
    ''' <param name="ImportoInteressi"></param>
    ''' <returns></returns>
    Private Function FillBookMarkIMPORTIINTERESSITARSU(ByVal ds As DataSet, ByRef ImportoInteressi As String) As Boolean
        Dim iInteressi As Integer
        Try
            If ds.Tables(0).Rows.Count > 0 Then
                'deve restituire sempre al max un record perchè la query
                'prevede un raggruppamento con somme
                For iInteressi = 0 To ds.Tables(0).Rows.Count - 1
                    ImportoInteressi = FormatImport(ds.Tables(0).Rows(iInteressi)("IMPORTO_TOTALE_SEMESTRI"))
                Next
            Else
                ImportoInteressi = FormatImport(0)
            End If

            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.FillBookMarkIMPORTOINTERESSITARSU.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Function
    '*** 20130801 - accertamento OSAP ***
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="TipoBookmark"></param>
    ''' <param name="ds"></param>
    ''' <returns></returns>
    Private Function FillBookMarkOSAPDichiaratoAccertato(ByVal TipoBookmark As String, ByVal ds As DataSet) As String
        Dim strImmoTemp As String = String.Empty
        Try
            Dim strRiga As String = ""
            Dim iImmobili As Integer

            'Dim culture As IFormatProvider
            'culture = New CultureInfo("it-IT", True)
            'System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("it-IT")

            If ds.Tables(0).Rows.Count > 0 Then
                Dim objAcc As New ClsGestioneAccertamenti
                Dim dt As DataTable

                'Dim blnConfigDichiarazione As Boolean = Boolean.Parse(ConfigurationManager.AppSettings("CONFIGURAZIONE_DICHIARAZIONE").ToString())
                strRiga = ""
                'strRiga = strRiga.PadLeft(144, "-")
                strRiga = strRiga.PadLeft(120, "-") & Microsoft.VisualBasic.Constants.vbCrLf
                strImmoTemp = strRiga

                For iImmobili = 0 To ds.Tables(0).Rows.Count - 1

                    strImmoTemp = strImmoTemp & Microsoft.VisualBasic.Constants.vbCrLf
                    'Periodo
                    strImmoTemp += " Durata: " & ds.Tables(0).Rows(iImmobili)("DURATAOCCUPAZIONE") & " " & ds.Tables(0).Rows(iImmobili)("TIPODURATA")
                    strImmoTemp += Microsoft.VisualBasic.Constants.vbTab & " Dal: " & ds.Tables(0).Rows(iImmobili)("DATAINIZIOOCCUPAZIONE") & " Al: " & ds.Tables(0).Rows(iImmobili)("DATAFINEOCCUPAZIONE")
                    strImmoTemp += Microsoft.VisualBasic.Constants.vbCrLf & Microsoft.VisualBasic.Constants.vbCrLf
                    'Indirizzo
                    strImmoTemp += vbTab & "Indirizzo: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("Via")) & " " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("Civico")) & Microsoft.VisualBasic.Constants.vbCrLf
                    'tipo occupazione
                    strImmoTemp += vbTab & "Occupazione: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("TIPOLOGIAOCCUPAZIONE")) & Microsoft.VisualBasic.Constants.vbCrLf
                    'Categoria
                    strImmoTemp += vbTab & "Categoria: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("CATEGORIA")) & Microsoft.VisualBasic.Constants.vbCrLf
                    'Altri dati
                    strImmoTemp += vbTab & "Tariffa: " & CStr(FormatNumber(ds.Tables(0).Rows(iImmobili)("TARIFFA_APPLICATA"))) & " € " & vbTab
                    strImmoTemp += "Consistenza: " & FormatImport(ds.Tables(0).Rows(iImmobili)("CONSISTENZA")) & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("TIPOCONSISTENZA")) & vbTab
                    strImmoTemp += "Importo dovuto: " & FormatImport(ds.Tables(0).Rows(iImmobili)("IMPORTO")) & " €" & Microsoft.VisualBasic.Constants.vbCrLf
                    '********* AGEVOLAZIONI ********
                    Try
                        dt = objAcc.OSAPGetAgevolazioniPerStampa(TipoBookmark, ds.Tables(0).Rows(iImmobili)("IDPOSIZIONE"))
                        strImmoTemp += vbTab & "Agevolazione Applicata:" & Microsoft.VisualBasic.Constants.vbCrLf
                        Dim myRow As DataRow
                        For Each myRow In dt.Rows
                            strImmoTemp += vbTab & myRow("descrizione") & Microsoft.VisualBasic.Constants.vbCrLf
                        Next
                        dt.Dispose()
                    Catch ex As Exception
                        strImmoTemp += ""
                    End Try
                    '****************************
                    strImmoTemp = strImmoTemp & Microsoft.VisualBasic.Constants.vbCrLf
                    strImmoTemp = strImmoTemp & strRiga
                Next
            Else
                strImmoTemp = strRiga
                If TipoBookmark = "D" Then
                    strImmoTemp = "Nessun Immobile dichiarato." & Microsoft.VisualBasic.Constants.vbCrLf
                Else
                    strImmoTemp = "Nessun Immobile accertato." & Microsoft.VisualBasic.Constants.vbCrLf
                End If
                strImmoTemp = strImmoTemp & strRiga
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.FillBookMarkOSAPDichiaratoAccertato.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
        Return strImmoTemp
    End Function
    '*** ***
#End Region
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="GruppoDocumenti"></param>
    Private Sub PrintDoc(GruppoDocumenti As GruppoDocumenti())
        Dim sScript As String = ""
        Try
            Session.Remove("ELENCO_ACCERTAMENTI_STAMPATI")
            Dim retArray As GruppoURL
            Dim objTestataComplessiva As New RIBESElaborazioneDocumentiInterface.Stampa.oggetti.oggettoTestata
            Dim oInterfaceStampaDocOggetti As IElaborazioneStampaDocOggetti
            oInterfaceStampaDocOggetti = Activator.GetObject(GetType(IElaborazioneStampaDocOggetti), ConstSession.UrlServizioStampe)

            objTestataComplessiva.Atto = ""
            objTestataComplessiva.Dominio = ""
            objTestataComplessiva.Ente = ""
            objTestataComplessiva.Filename = ""
            '*** 201511 - template documenti per ruolo ***
            retArray = oInterfaceStampaDocOggetti.StampaDocumenti(ConstSession.PathStampe, ConstSession.PathVirtualStampe, objTestataComplessiva, GruppoDocumenti, True, False)
            If Not retArray Is Nothing Then
                Session.Add("ELENCO_ACCERTAMENTI_STAMPATI", retArray)
                sScript += "parent.corpo.location.href='ViewAccertamentiStampati.aspx';"
                sScript += "parent.parent.document.getElementById('DivAttesa').style.display='none';"
            Else
                sScript += "GestAlert('a', 'danger', '', '', 'Errore in fase di stampa');"
            End If
        Catch ex As Exception
            sScript += "GestAlert('a', 'danger', '', '', 'Errore in fase di stampa');"
        End Try
        RegisterScript(sScript, Me.GetType())
    End Sub
    'Private Function ImportoArrotondato(ByVal ImportoEuro As Double) As Long

    '    'Funzione che in base alla nuova finanziaria prevede
    '    'gli importi arrotondati
    '    'x= importo da arrotondare + 0.5
    '    'importo arrotondato = parte intera di x

    '    Dim X As Double
    '    Dim ImportoOut As Long
    '    Try
    '        If ImportoEuro > 0 Then

    '            X = ImportoEuro + 0.5
    '            If InStr(X, ",") > 0 Then
    '                ImportoOut = Left(X, InStr(X, ",") - 1)
    '            Else
    '                ImportoOut = X
    '            End If

    '        ElseIf ImportoEuro < 0 Then

    '            X = ImportoEuro - 0.5
    '            If InStr(X, ",") > 0 Then
    '                ImportoOut = Left(X, InStr(X, ",") - 1)
    '            Else
    '                ImportoOut = X
    '            End If

    '        End If

    '        Return ImportoOut
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.ImportoArrotondato.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="iInput"></param>
    ''' <returns></returns>
    Private Function BoolToStringForGridView(ByVal iInput As Object) As String
        Dim ret As String = String.Empty
        Try
            If ((iInput.ToString() = "1") Or (iInput.ToString().ToUpper() = "TRUE")) Then
                ret = "SI"
            Else
                ret = "NO"
            End If

            Return ret
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.BoolToStringForGridView.errore: ", ex)
            Return ""
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="objInput"></param>
    ''' <returns></returns>
    Private Function FormatString(ByVal objInput As Object) As String
        Try
            Dim strOutput As String = String.Empty
            If (objInput Is Nothing) Then
                strOutput = ""
            Else
                strOutput = objInput.ToString()
            End If
            Return strOutput
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.FormatString.errore: ", ex)
            Return ""
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="objInput"></param>
    ''' <returns></returns>
    Private Function FormatStringToEmpty(ByVal objInput As Object) As String
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
            Return strOutput
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.FormatStringToEmpty.errore: ", ex)
            Return ""
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="objInput"></param>
    ''' <returns></returns>
    Private Function FormatImport(ByVal objInput As Object) As String
        Dim strOutput As String
        Try
            If Not IsDBNull(objInput) Then
                If CStr(objInput) = "" Or CStr(objInput) = "0" Or CStr(objInput) = "-1" Or CStr(objInput) = "0,00" Or CStr(objInput) = "-1,00" Then
                    If CStr(objInput) = "0" Or CStr(objInput) = "0,00" Then
                        Dim dblImporto As Double
                        dblImporto = 0
                        strOutput = Format(dblImporto, "#,##0.00")
                    Else
                        strOutput = 0
                    End If
                Else
                    '#,##0.00
                    Dim dblImporto As Double
                    dblImporto = CDbl(objInput)
                    strOutput = Format(dblImporto, "#,##0.00")
                End If
            Else
                strOutput = 0
            End If

            Return strOutput
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.FormatImport.errore: ", ex)
            Return ""
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="objInput"></param>
    ''' <returns></returns>
    Private Function FormatNumberToZero(ByVal objInput As Object) As String
        Dim strOutput As String
        Try
            If Not IsDBNull(objInput) Then
                If CStr(objInput) = "" Or CStr(objInput) = "0" Then
                    strOutput = 0
                Else
                    strOutput = objInput.ToString()
                End If
            Else
                strOutput = 0
            End If

            Return strOutput
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.FormatNumberToZero.errore: ", ex)
            Return ""
        End Try
    End Function
    'Private Function addRowsObjAnagrafica(ByVal Anagrafica As DettaglioAnagrafica) As DataSet
    '    Dim row As DataRow
    '    Dim newTable As DataTable
    '    Dim objAnagrafica As DataSet = Nothing
    '    Try
    '        objAnagrafica = CreateDsPerAnagrafica()
    '        newTable = objAnagrafica.Tables(0).Copy

    '        row = newTable.NewRow()
    '        'row.Item("cod_contribuente") = Anagrafica.COD_CONTRIBUENTE

    '        row.Item("COD_CONTRIBUENTE") = Anagrafica.COD_CONTRIBUENTE
    '        row.Item("ID_DATA_ANAGRAFICA") = Anagrafica.ID_DATA_ANAGRAFICA
    '        row.Item("Cognome") = Anagrafica.Cognome
    '        row.Item("RappresentanteLegale") = Anagrafica.RappresentanteLegale
    '        row.Item("Nome") = Anagrafica.Nome
    '        row.Item("CodiceFiscale") = Anagrafica.CodiceFiscale
    '        row.Item("PartitaIva") = Anagrafica.PartitaIva
    '        row.Item("CodiceComuneNascita") = Anagrafica.CodiceComuneNascita
    '        row.Item("ComuneNascita") = Anagrafica.ComuneNascita
    '        row.Item("ProvinciaNascita") = Anagrafica.ProvinciaNascita
    '        row.Item("DataNascita") = Anagrafica.DataNascita
    '        row.Item("NazionalitaNascita") = Anagrafica.NazionalitaNascita
    '        row.Item("Sesso") = Anagrafica.Sesso
    '        row.Item("CodiceComuneResidenza") = Anagrafica.CodiceComuneResidenza
    '        row.Item("ComuneResidenza") = Anagrafica.ComuneResidenza
    '        row.Item("ProvinciaResidenza") = Anagrafica.ProvinciaResidenza
    '        row.Item("CapResidenza") = Anagrafica.CapResidenza
    '        row.Item("CodViaResidenza") = Anagrafica.CodViaResidenza
    '        row.Item("ViaResidenza") = Anagrafica.ViaResidenza
    '        row.Item("PosizioneCivicoResidenza") = Anagrafica.PosizioneCivicoResidenza
    '        row.Item("CivicoResidenza") = Anagrafica.CivicoResidenza
    '        row.Item("EsponenteCivicoResidenza") = Anagrafica.EsponenteCivicoResidenza
    '        row.Item("ScalaCivicoResidenza") = Anagrafica.ScalaCivicoResidenza
    '        row.Item("InternoCivicoResidenza") = Anagrafica.InternoCivicoResidenza
    '        row.Item("FrazioneResidenza") = Anagrafica.FrazioneResidenza
    '        row.Item("NazionalitaResidenza") = Anagrafica.NazionalitaResidenza
    '        row.Item("NucleoFamiliare") = Anagrafica.NucleoFamiliare
    '        row.Item("DATA_MORTE") = Anagrafica.DataMorte
    '        row.Item("Professione") = Anagrafica.Professione
    '        row.Item("Note") = Anagrafica.Note
    '        row.Item("DaRicontrollare") = Anagrafica.DaRicontrollare
    '        row.Item("DataInizioValidita") = Anagrafica.DataInizioValidita
    '        row.Item("DataFineValidita") = Anagrafica.DataFineValidita
    '        row.Item("DataUltimaModifica") = Anagrafica.DataUltimaModifica
    '        row.Item("Operatore") = Anagrafica.Operatore
    '        row.Item("CodContribuenteRappLegale") = Anagrafica.CodContribuenteRappLegale
    '        row.Item("CodEnte") = Anagrafica.CodEnte
    '        row.Item("CodIndividuale") = Anagrafica.CodIndividuale
    '        row.Item("CodFamiglia") = Anagrafica.CodFamiglia
    '        row.Item("NCTributari") = Anagrafica.NCTributari
    '        row.Item("DataUltimoAggTributi") = Anagrafica.DataUltimoAggTributi
    '        row.Item("NCAnagraficaRes") = Anagrafica.NCAnagraficaRes
    '        row.Item("DataUltimoAggAnagrafe") = Anagrafica.DataUltimoAggAnagrafe
    '        row.Item("TipoRiferimento") = Anagrafica.TipoRiferimento
    '        row.Item("DatiRiferimento") = Anagrafica.DatiRiferimento
    '        row.Item("ID_DATA_SPEDIZIONE") = Anagrafica.ID_DATA_SPEDIZIONE
    '        row.Item("CodTributo") = Anagrafica.CodTributo
    '        row.Item("CognomeInvio") = Anagrafica.CognomeInvio
    '        row.Item("NomeInvio") = Anagrafica.NomeInvio
    '        row.Item("CodComuneRCP") = Anagrafica.CodComuneRCP
    '        row.Item("ComuneRCP") = Anagrafica.ComuneRCP
    '        row.Item("LocRCP") = Anagrafica.LocRCP
    '        row.Item("ProvinciaRCP") = Anagrafica.ProvinciaRCP
    '        row.Item("CapRCP") = Anagrafica.CapRCP
    '        row.Item("CodViaRCP") = Anagrafica.CodViaRCP
    '        row.Item("ViaRCP") = Anagrafica.ViaRCP
    '        row.Item("PosizioneCivicoRCP") = Anagrafica.PosizioneCivicoRCP
    '        row.Item("CivicoRCP") = Anagrafica.CivicoRCP
    '        row.Item("EsponenteCivicoRCP") = Anagrafica.EsponenteCivicoRCP
    '        row.Item("ScalaCivicoRCP") = Anagrafica.ScalaCivicoRCP
    '        row.Item("InternoCivicoRCP") = Anagrafica.InternoCivicoRCP
    '        row.Item("FrazioneRCP") = Anagrafica.FrazioneRCP
    '        row.Item("DataInizioValiditaSpedizione") = Anagrafica.DataInizioValiditaSpedizione
    '        row.Item("DataFineValiditaSpedizione") = Anagrafica.DataFineValiditaSpedizione
    '        row.Item("DataUltimaModificaSpedizione") = Anagrafica.DataUltimaModificaSpedizione
    '        row.Item("OperatoreSpedizione") = Anagrafica.OperatoreSpedizione

    '        newTable.Rows.Add(row)
    '        newTable.AcceptChanges()

    '        objAnagrafica.Tables(0).ImportRow(row)
    '        objAnagrafica.Tables(0).AcceptChanges()

    '        addRowsObjAnagrafica = objAnagrafica
    '    Catch ex As Exception
    ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.addRowsObjAnagrafica.errore: ", ex)
    '       Response.Redirect("../../../PaginaErrore.aspx")
    '    End Try
    'End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="myStr"></param>
    ''' <returns></returns>
    Function Up(ByVal myStr As String) As String
        Dim arr As String()
        Dim i As Integer
        Dim tempstr As String = String.Empty
        myStr = Trim(myStr)
        arr = Split(myStr, ".")
        Try
            If (arr.Length > 0) Then
                For i = 0 To arr.Length - 1
                    If arr(i) <> "" Then
                        If (Left(arr(i), 1) <> " ") Then
                            tempstr += UCase(Left(arr(i), 1)) & LCase(Mid(arr(i), 2)) & "."
                        Else
                            tempstr += UCase(Left(arr(i), 2)) & LCase(Mid(arr(i), 3)) & "."
                        End If
                    End If


                Next
            Else
                If (Left(myStr, 1) <> " ") Then
                    tempstr += UCase(Left(myStr, 1)) & LCase(Mid(myStr, 2)) & "."
                Else
                    tempstr += UCase(Left(myStr, 2)) & LCase(Mid(myStr, 3)) & "."
                End If
            End If

            Return tempstr
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.Up.errore: ", ex)
            Return ""
        End Try
    End Function
    'Private Function CreateDsPerAnagrafica() As DataSet
    '    Dim objDS As New DataSet
    '    Try
    '        Dim newTable As DataTable
    '        newTable = New DataTable("ANAGRAFICA")

    '        Dim NewColumn As New DataColumn
    '        NewColumn.ColumnName = "COD_CONTRIBUENTE"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ID_DATA_SPEDIZIONE"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ID_DATA_ANAGRAFICA"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "Cognome"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "RappresentanteLegale"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "Nome"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "CodiceFiscale"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "INDIRIZZO"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "PartitaIva"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "CodiceComuneNascita"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ComuneNascita"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ProvinciaNascita"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DataNascita"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "NazionalitaNascita"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "Sesso"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "CodiceComuneResidenza"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ComuneResidenza"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ProvinciaResidenza"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "CapResidenza"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "CodViaResidenza"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ViaResidenza"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "PosizioneCivicoResidenza"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "CivicoResidenza"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "EsponenteCivicoResidenza"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ScalaCivicoResidenza"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "InternoCivicoResidenza"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "FrazioneResidenza"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "NazionalitaResidenza"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "NucleoFamiliare"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DATA_MORTE"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "Professione"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "Note"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DaRicontrollare"
    '        NewColumn.DataType = System.Type.GetType("System.Boolean")
    '        NewColumn.DefaultValue = False
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DataInizioValidita"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DataFineValidita"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DataUltimaModifica"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "Operatore"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "CodContribuenteRappLegale"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "CodEnte"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "CodIndividuale"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "CodFamiglia"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "NCTributari"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DataUltimoAggTributi"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "NCAnagraficaRes"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DataUltimoAggAnagrafe"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "CodTributo"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "CognomeInvio"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "NomeInvio"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "CodComuneRCP"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ComuneRCP"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "LocRCP"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ProvinciaRCP"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "CapRCP"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "CodViaRCP"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ViaRCP"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "PosizioneCivicoRCP"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "CivicoRCP"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "EsponenteCivicoRCP"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "ScalaCivicoRCP"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "InternoCivicoRCP"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "FrazioneRCP"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DataInizioValiditaSpedizione"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DataFineValiditaSpedizione"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DataUltimaModificaSpedizione"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "OperatoreSpedizione"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "TipoRiferimento"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        NewColumn = New DataColumn
    '        NewColumn.ColumnName = "DatiRiferimento"
    '        NewColumn.DataType = System.Type.GetType("System.String")
    '        NewColumn.DefaultValue = ""
    '        newTable.Columns.Add(NewColumn)

    '        objDS.Tables.Add(newTable)
    '        Return objDS
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleA.CreateDsPerAnasgrafica.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    End Try
    'End Function
End Class
