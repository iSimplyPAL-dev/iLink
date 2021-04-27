Imports log4net
Imports RIBESElaborazioneDocumentiInterface
Imports RIBESElaborazioneDocumentiInterface.Stampa.oggetti
Imports System.Globalization
Imports ComPlusInterface

''' <summary>
''' Pagina per la produzione del bollettino per il provvedimento.
''' Contiene le funzioni della comandiera. 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class ristampaOriginaleL
    Inherits BaseEnte
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(ristampaOriginaleL))

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
        Dim bCreaPDF As Boolean = False
        Dim strTipoDoc As String
        strTipoDoc = Request.Item("tipodoc")
        Try
            If strTipoDoc.CompareTo(CostantiProvv.DOCUMENTO_PREACCERTAMENTO) = 0 Or strTipoDoc.CompareTo(CostantiProvv.DOCUMENTO_PREACCERTAMENTO_BOZZA) = 0 Then
                ' STAMPA_PREACCERTAMENTO(strTipoDoc)
            ElseIf strTipoDoc.CompareTo(CostantiProvv.BOLLETTINI_ACCERTAMENTO) = 0 Then
                STAMPA_BOLLETTINI_PRE_e_ACCERTAMENTO(Request.Item("CODTRIBUTO"), strTipoDoc, bCreaPDF)
                'ElseIf strTipoDoc.CompareTo(CostantiProvv.BOLLETTINI_ACCERTAMENTO_RATE) = 0 Then
                '    STAMPA_BOLLETTINI_PRE_e_ACCERTAMENTO_RATEIZZATI(Request.Item("CODTRIBUTO"), strTipoDoc, bCreaPDF)
                'Else
                'If strTipoDoc.CompareTo(CostantiProvv.DOCUMENTO_LETTERA_RI) = 0 Then
                '    STAMPA_LETTERA_CONTATTO(Request.Item("CODTRIBUTO"))
                'ElseIf strTipoDoc.CompareTo(CostantiProvv.DOCUMENTO_RI) = 0 Then
                '    STAMPA_LETTERA_RICHIESTA_INFO(Request.Item("CODTRIBUTO"))
                'End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleL.Page_Load.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub STAMPA_PREACCERTAMENTO(ByVal strTipoDoc As String)
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

    '    Dim objICI As New DichiarazioniICI.Database.TpSituazioneFinaleIci
    '    Dim objVers As New DichiarazioniICI.Database.VersamentiTable(ConstSession.UserName)
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
    '    Dim dsImmobiliCatasto As New DataSet
    '    Dim objDSTipiInteressi As New DataSet
    '    Dim objDSElencoSanzioni As New DataSet
    '    Dim objDSImportiInteressi As New DataSet
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
    '    Dim objHashTable As Hashtable = New Hashtable
    '    Dim objCOM As IElaborazioneLiquidazioni = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneLiquidazioni), ConstSession.URLServiziLiquidazioni)
    '    Dim objCOMTipoVoci As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)

    '    ' COMPONGO IL PERCORSO DEL FILE DOT
    '    Try
    '        sFilenameDOT = "PreAccertamento_" & ConstSession.IdEnte & ".doc"
    '        sFilenameDOC = "PreAccertamento"
    '        strTIPODOCUMENTO = CostantiProvv.COD_TIPO_DOC_PREACCERTAMENTO

    '        objTestataDOT.Atto = "Template"
    '        objTestataDOT.Dominio = "Provvedimenti"
    '        objTestataDOT.Ente = ConstSession.DescrizioneEnte.ToUpper.Replace("COMUNE DI ", "")
    '        objTestataDOT.Filename = sFilenameDOT

    '        objHashTable.Add("CodENTE", ConstSession.IdEnte)
    '        objHashTable.Add("CODENTE", ConstSession.IdEnte)
    '        objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)

    '        For iCount = 0 To objDSstampa.Tables(0).Rows.Count - 1
    '            iCodContrib = objDSstampa.Tables(0).Rows(iCount)("COD_CONTRIBUENTE")
    '            IDPROVVEDIMENTO = objDSstampa.Tables(0).Rows(iCount)("ID_PROVVEDIMENTO")
    '            objHashTable.Add("COD_CONTRIBUENTE", iCodContrib)

    '            dsDatiProvv = objCOM.GetProvvedimentoPerStampaLiquidazione(objHashTable, IDPROVVEDIMENTO)

    '            IDPROCEDIMENTO = dsDatiProvv.Tables(0).Rows(iCount)("ID_PROCEDIMENTO")
    '            strANNO = dsDatiProvv.Tables(0).Rows(iCount)("ANNO")

    '            '*** 20140701 - IMU/TARES ***
    '            If strANNO >= 2012 Then
    '                sFilenameDOT = sFilenameDOT.Replace("ICI", "IMU")
    '                sFilenameDOC = sFilenameDOC.Replace("ICI", "IMU")
    '                objTestataDOT.Filename = sFilenameDOT
    '                Log.Debug(sFilenameDOT & "::" & sFilenameDOC)
    '            End If
    '            '*** ***

    '            objTestataDOC = New oggettoTestata
    '            objTestataDOC.Atto = "Documenti"
    '            objTestataDOC.Dominio = "Provvedimenti"
    '            objTestataDOC.Ente = ConstSession.DescrizioneEnte.ToUpper.Replace("COMUNE DI ", "")
    '            objTestataDOC.Filename = iCodContrib & "_" & ConstSession.IdEnte & sFilenameDOC

    '            oArrBookmark = New ArrayList
    '            If strTipoDoc.CompareTo(CostantiProvv.DOCUMENTO_PREACCERTAMENTO_BOZZA) = 0 Then 'BOZZA
    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "bozza"
    '                objBookmark.Valore = "BOZZA"
    '                oArrBookmark.Add(objBookmark)
    '            End If

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "nome_ente"
    '            objBookmark.Valore = ConstSession.DescrizioneEnte.ToUpper
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "TipoProvvedimento"
    '            objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("TIPO_PROVVEDIMENTO")).ToUpper()
    '            oArrBookmark.Add(objBookmark)

    '            '************************************************************************************
    '            'DATI ANAGRAFICI
    '            '************************************************************************************
    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "cognome"
    '            objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("COGNOME"))
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "nome"
    '            objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("NOME"))
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "nomeinvio"
    '            objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("co"))
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "via_residenza"
    '            objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("VIA_RES"))
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "civico_residenza"
    '            objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("CIVICO_RES"))
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "frazione_residenza"
    '            objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("FRAZIONE_RES"))
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "cap_residenza"
    '            objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("CAP_RES"))
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "citta_residenza"
    '            objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("CITTA_RES"))
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "prov_residenza"
    '            If CStr(dsDatiProvv.Tables(0).Rows(iCount)("PROVINCIA_RES")).CompareTo("") <> 0 Then
    '                objBookmark.Valore = "(" & dsDatiProvv.Tables(0).Rows(iCount)("PROVINCIA_RES") & ")"
    '            Else
    '                objBookmark.Valore = ""
    '            End If
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "codice_fiscale"
    '            objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("CODICE_FISCALE"))
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "partita_iva"
    '            objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("PARTITA_IVA"))
    '            oArrBookmark.Add(objBookmark)

    '            '---------------------------------------------------------------------------------

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "anno_ici"
    '            objBookmark.Valore = strANNO 'objDSstampa.Tables(0).Rows(iCount)("ANNO")
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "n_provvedimento"
    '            If strTipoDoc.CompareTo(CostantiProvv.DOCUMENTO_PREACCERTAMENTO_BOZZA) = 0 Then
    '                objBookmark.Valore = dsDatiProvv.Tables(0).Rows(iCount)("NUMERO_AVVISO")
    '            Else
    '                objBookmark.Valore = objUtility.CToStr(dsDatiProvv.Tables(0).Rows(iCount)("NUMERO_ATTO"))
    '            End If
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
    '            objBookmark.Valore = strANNO 'dsDatiProvv.Tables(0).Rows(iCount)("ANNO")
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "anno_imposta1"
    '            objBookmark.Valore = strANNO 'dsDatiProvv.Tables(0).Rows(iCount)("ANNO")
    '            oArrBookmark.Add(objBookmark)

    '            ''''''************************************************************************************
    '            ''''''ELENCO IMMOBILI DICHIARATI
    '            ''''''************************************************************************************
    '            dsImmobiliDichiarato = objCOM.getImmobiliDichiaratiPerStampaLiquidazione(objHashTable, IDPROCEDIMENTO)
    '            strRiga = FillBookMarkDICHIARATO(dsImmobiliDichiarato, CBool(ConfigurationManager.AppSettings("CONFIGURAZIONE_DICHIARAZIONE").ToString()))

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "elenco_immobili"
    '            objBookmark.Valore = "Dettaglio Immobili Dichiarati" & vbCrLf & strRiga
    '            oArrBookmark.Add(objBookmark)

    '            ''''''************************************************************************************
    '            ''''''ELENCO VERSAMENTI
    '            ''''''************************************************************************************
    '            dsVersamenti = objCOM.getVersamentiPerStampaLiquidazione(objHashTable, IDPROCEDIMENTO, 0)
    '            strRiga = FillBookMarkVersamenti(dsVersamenti, CBool(ConfigurationManager.AppSettings("CONFIGURAZIONE_DICHIARAZIONE").ToString()))

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "elenco_versamenti"
    '            objBookmark.Valore = "Dettaglio Versamenti" & vbCrLf & strRiga
    '            oArrBookmark.Add(objBookmark)

    '            ''''''************************************************************************************
    '            ''''''ELENCO IMMOBILI DA CATASTO
    '            ''''''************************************************************************************
    '            dsImmobiliCatasto = objCOM.getImmobiliCatastoPerStampaLiquidazione(objHashTable, IDPROCEDIMENTO)
    '            strRiga = FillBookMarkCATASTO(dsImmobiliCatasto, CBool(ConfigurationManager.AppSettings("CONFIGURAZIONE_DICHIARAZIONE").ToString()))

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "elenco_immobili_cat"
    '            objBookmark.Valore = "Dettaglio Immobili abbinati a Catasto" & vbCrLf & strRiga
    '            oArrBookmark.Add(objBookmark)

    '            ''''''************************************************************************************
    '            ''''''ELENCO INTERESSI CONFIGURATI
    '            ''''''************************************************************************************
    '            objHashTable.Add("CODTIPOINTERESSE", "-1")
    '            objHashTable.Add("DAL", "")
    '            objHashTable.Add("AL", "")
    '            objHashTable.Add("TASSO", "")
    '            objHashTable.Add("CODTRIBUTO", ConstSession.CodTributo)

    '            objDSTipiInteressi = objCOM.GetElencoInteressiPerStampaLiquidazione(objHashTable, IDPROVVEDIMENTO)
    '            strRiga = FillBookMarkELENCOINTERESSI(objDSTipiInteressi)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "elenco_interessi"
    '            objBookmark.Valore = strRiga
    '            oArrBookmark.Add(objBookmark)

    '            ''''''************************************************************************************
    '            ''''''ELENCO SANZIONI APPLICATE CON IMPORTO 
    '            ''''''************************************************************************************

    '            objDSElencoSanzioni = objCOM.GetElencoSanzioniPerStampaLiquidazione(objHashTable, IDPROVVEDIMENTO)
    '            strRiga = FillBookMarkELENCOSANZIONI(objDSElencoSanzioni)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "elenco_sanzioni"
    '            objBookmark.Valore = strRiga
    '            oArrBookmark.Add(objBookmark)

    '            ''''''************************************************************************************
    '            ''''''IMPORTI (DIFF IMPOSTA - SANZIONI - INTERESSI - SPESE - ecc...
    '            ''''''************************************************************************************
    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "imposta_dovuta"
    '            objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("TOTALE_DICHIARATO")) & " €"
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "imposta_versata"
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
    '            objBookmark.Descrizione = "DiffImpostaDaVersare"
    '            objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_DIFFERENZA_IMPOSTA")) & " €"
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "ImportoSanzione"
    '            objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SANZIONI")) & " €"
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "spese_notifica"
    '            objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_SPESE")) & " €"
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "Importo_arrotond"
    '            objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_ARROTONDAMENTO")) & " €"
    '            oArrBookmark.Add(objBookmark)

    '            objBookmark = New oggettiStampa
    '            objBookmark.Descrizione = "Importo_totale"
    '            objBookmark.Valore = FormatImport(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_TOTALE")) & " €"
    '            oArrBookmark.Add(objBookmark)

    '            ''''''************************************************************************************
    '            ''''''IMPORTI INTERESSI
    '            ''''''************************************************************************************

    '            objDSImportiInteressi = objCOM.GetInteressiTotaliPerStampaLiquidazione(objHashTable, IDPROVVEDIMENTO)
    '            iRetValImpInt = FillBookMarkIMPORTIINTERESSI(objDSImportiInteressi, strImportoGiorni, strImportoSemestriACC, strImportoSemestriSAL, strNumSemestriACC, strNumSemestriSAL)

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

    '            End If
    '            ''''''************************************************************************************
    '            ''''''FINE IMPORTI INTERESSI
    '            ''''''************************************************************************************

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

    '        ' oggetto per la stampa
    '        Dim oInterfaceStampaDocOggetti As IElaborazioneStampaDocOggetti
    '        oInterfaceStampaDocOggetti = Activator.GetObject(GetType(IElaborazioneStampaDocOggetti), ConstSession.UrlServizioStampe)

    '        Dim retArray As GruppoURL

    '        Dim objTestataComplessiva As New RIBESElaborazioneDocumentiInterface.Stampa.oggetti.oggettoTestata
    '        objTestataComplessiva.Atto = ""
    '        objTestataComplessiva.Dominio = ""
    '        objTestataComplessiva.Ente = ""
    '        objTestataComplessiva.Filename = ""
    '        ' definisco anche il numero di documenti che voglio stampare.
    '        '*** 201511 - template documenti per ruolo ***
    '        'retArray = oInterfaceStampaDocOggetti.StampaDocumenti(objTestataComplessiva, GruppoDOCUMENTI)
    '        retArray = oInterfaceStampaDocOggetti.StampaDocumenti(ConstSession.PathStampe, ConstSession.PathVirtualStampe, objTestataComplessiva, GruppoDOCUMENTI, True, False)
    '        'retArray = oInterfaceStampaDocOggetti.StampaDocumenti(objTestataComplessiva, GruppoDOCUMENTI, True, False)

    '        Session.Remove("ELENCO_LIQUIDAZIONI_STAMPATE")

    '        If Not retArray Is Nothing Then
    '            Session.Add("ELENCO_LIQUIDAZIONI_STAMPATE", retArray)
    '            Dim sScript As String = ""
    '            sScript += "parent.frames.item('corpo').location.href='ViewLiquidazioniStampate.aspx'" & vbCrLf
    '            RegisterScript(sScript, Me.GetType())
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleL.STAMPA_PREACCERTAMENTO.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    'Private Sub STAMPA_LETTERA_RICHIESTA_INFO(Tributo As String)

    '    Dim sFilenameDOT, sFilenameDOC As String
    '    Dim objDSstampa As DataSet
    '    'contiene i dati anagrafici visualizzati nella grilgia della videata di gestione elaborazione
    '    objDSstampa = CType(Session("PROVVEDIMENTI_DA_STAMPARE"), DataSet)

    '    'AGGIUNGO LA COLONNA SCARTATO AL DATASET DELLA STAMPA-------------------------------------------------------------------------------------
    '    Dim NewColumn As New DataColumn
    '    NewColumn.ColumnName = "SCARTATO"
    '    NewColumn.DataType = System.Type.GetType("System.Boolean")
    '    NewColumn.DefaultValue = 0
    '    objDSstampa.Tables(0).Columns.Add(NewColumn)

    '    ' creo l'oggetto testata per l'oggetto da stampare
    '    'serve per indicare la posizione di salvataggio e il nome del file.
    '    Dim objTestataDOC As oggettoTestata
    '    Dim objTestataDOT As New oggettoTestata
    '    Dim IDPROVVEDIMENTO As String
    '    Dim IDPROCEDIMENTO As String
    '    Dim sANNO As String

    '    ' COMPONGO IL PERCORSO DEL FILE DOT

    '    sFilenameDOT = "RichiestaInfo.doc"
    '    sFilenameDOC = "RichiestaInfo"

    '    objTestataDOT.Atto = "Template"
    '    objTestataDOT.Dominio = "Provvedimenti"
    '    objTestataDOT.Ente = ConstSession.DescrizioneEnte
    '    objTestataDOT.Filename = sFilenameDOT

    '    'objTestataDOC.Atto = "Documenti"
    '    'objTestataDOC.Dominio = "Provvedimenti"
    '    'objTestataDOC.Ente = ConstSession.DescrizioneEnte
    '    'objTestataDOC.Filename = sFilenameDOC

    '    Dim oArrBookmark As ArrayList
    '    Dim iCount As Integer
    '    Dim objBookmark As oggettiStampa
    '    Dim oArrListOggettiDaStampare As New ArrayList
    '    Dim objToPrint As oggettoDaStampareCompleto
    '    Dim ArrayBookMark As oggettiStampa()
    '    Dim iCodContrib As Integer
    '    Dim sNoteContrib As String

    '    Dim strRigaImmobili As String
    '    Dim strRigaVersamenti As String
    '    Dim strImmobili As String = String.Empty
    '    Dim strVersamenti As String = String.Empty

    '    Dim Anno As String = String.Empty
    '    Dim sElencoAnni As String = String.Empty

    '    Dim dsImmobiliDichiarato As New DataSet
    '    Dim dsVersamenti As New DataSet
    '    Dim dsImmobiliCatasto As New DataSet
    '    'Dim strConnectionStringOPENgovProvvedimenti As String

    '    Dim objHashTable As Hashtable = New Hashtable

    '    'Dim objSessione As CreateSessione
    '    Dim objCOM As IElaborazioneLiquidazioni = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneLiquidazioni), ConstSession.URLServiziLiquidazioni)
    '    'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString

    '    objHashTable.Add("CodENTE", ConstSession.IdEnte)
    '    objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)

    '    Dim DSperInsertTabStorico As DataSet = CreateDataSetStampa()

    '    Dim drStampa() As DataRow
    '    Dim dr As DataRow
    '    Try
    '        For iCount = 0 To objDSstampa.Tables(0).Rows.Count - 1

    '            If objDSstampa.Tables(0).Rows(iCount)("SCARTATO") = False Then

    '                iCodContrib = objDSstampa.Tables(0).Rows(iCount)("COD_CONTRIBUENTE")

    '                objTestataDOC = New oggettoTestata
    '                objTestataDOC.Atto = "Documenti"
    '                objTestataDOC.Dominio = "Provvedimenti"
    '                objTestataDOC.Ente = ConstSession.DescrizioneEnte
    '                objTestataDOC.Filename = iCodContrib & "_" & ConstSession.IdEnte & sFilenameDOC
    '                sNoteContrib = objDSstampa.Tables(0).Rows(iCount)("NOTEPREACC")

    '                IDPROVVEDIMENTO = objDSstampa.Tables(0).Rows(iCount)("ID_PROVVEDIMENTO")
    '                sANNO = objDSstampa.Tables(0).Rows(iCount)("ANNO")

    '                oArrBookmark = New ArrayList

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "nome_ente"
    '                objBookmark.Valore = CStr(Session("DESCRIZIONE_ENTE")).ToUpper
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "note_contrib"
    '                objBookmark.Valore = sNoteContrib
    '                oArrBookmark.Add(objBookmark)

    '                '************************************************************************************
    '                'DATI ANAGRAFICI
    '                '************************************************************************************
    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "cognome"
    '                objBookmark.Valore = FormatStringToEmpty(objDSstampa.Tables(0).Rows(iCount)("COGNOME"))
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "nome"
    '                objBookmark.Valore = FormatStringToEmpty(objDSstampa.Tables(0).Rows(iCount)("NOME"))
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "via_residenza"
    '                objBookmark.Valore = FormatStringToEmpty(objDSstampa.Tables(0).Rows(iCount)("VIA_RES"))
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "civico_residenza"
    '                objBookmark.Valore = FormatStringToEmpty(objDSstampa.Tables(0).Rows(iCount)("CIVICO_RES"))
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "frazione_residenza"
    '                objBookmark.Valore = FormatStringToEmpty(objDSstampa.Tables(0).Rows(iCount)("FRAZIONE_RES"))
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "cap_residenza"
    '                objBookmark.Valore = FormatStringToEmpty(objDSstampa.Tables(0).Rows(iCount)("CAP_RES"))
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "citta_residenza"
    '                objBookmark.Valore = FormatStringToEmpty(objDSstampa.Tables(0).Rows(iCount)("CITTA_RES"))
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "prov_residenza"
    '                If CStr(objDSstampa.Tables(0).Rows(iCount)("PROVINCIA_RES")).CompareTo("") <> 0 Then
    '                    objBookmark.Valore = "(" & objDSstampa.Tables(0).Rows(iCount)("PROVINCIA_RES") & ")"
    '                Else
    '                    objBookmark.Valore = ""
    '                End If
    '                '                objBookmark.Valore = "(" & objDSstampa.Tables(0).Rows(iCount)("PROVINCIA_RES") & ")"
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "codice_fiscale"
    '                objBookmark.Valore = FormatStringToEmpty(objDSstampa.Tables(0).Rows(iCount)("CODICE_FISCALE"))
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "partita_iva"
    '                objBookmark.Valore = FormatStringToEmpty(objDSstampa.Tables(0).Rows(iCount)("PARTITA_IVA"))
    '                oArrBookmark.Add(objBookmark)

    '                '---------------------------------------------------------------------------------

    '                drStampa = objDSstampa.Tables(0).Select("COD_CONTRIBUENTE='" & iCodContrib & "'")

    '                sElencoAnni = "" : strImmobili = "" : strVersamenti = "" : strRigaImmobili = "" : strRigaVersamenti = ""

    '                'For x = 0 To drstampa.Length - 1
    '                For Each row As DataRow In objDSstampa.Tables(0).Select("COD_CONTRIBUENTE='" & iCodContrib & "'")

    '                    IDPROCEDIMENTO = row("ID_PROCEDIMENTO")
    '                    sElencoAnni = sElencoAnni & row("ANNO") & ","

    '                    ''''''************************************************************************************
    '                    ''''''ELENCO IMMOBILI DICHIARATI
    '                    ''''''************************************************************************************
    '                    dsImmobiliDichiarato = objCOM.getImmobiliDichiaratiPerStampaLiquidazione(objHashTable, IDPROCEDIMENTO)
    '                    strRigaImmobili = FillBookMarkDICHIARATO(dsImmobiliDichiarato, CBool(ConfigurationManager.AppSettings("CONFIGURAZIONE_DICHIARAZIONE").ToString()))
    '                    strImmobili = strImmobili & "Dettaglio Immobili Anno " & row("ANNO") & vbCrLf & strRigaImmobili & vbCrLf & vbCrLf

    '                    dsVersamenti = objCOM.getVersamentiPerStampaLiquidazione(objHashTable, IDPROCEDIMENTO, oggettoatto.fase.versatodichiarato)
    '                    strRigaVersamenti = FillBookMarkVersamenti(dsVersamenti, CBool(ConfigurationManager.AppSettings("CONFIGURAZIONE_DICHIARAZIONE").ToString()))
    '                    strVersamenti = strVersamenti & "Dettaglio Versamenti (Ordinari e Ravvedimento Operoso) Anno " & row("ANNO") & vbCrLf & strRigaVersamenti & vbCrLf & vbCrLf

    '                    row("SCARTATO") = True

    '                Next

    '                sElencoAnni = Left(sElencoAnni, Len(sElencoAnni) - 1)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "elenco_immobili_dich"
    '                objBookmark.Valore = strImmobili
    '                oArrBookmark.Add(objBookmark)

    '                ''''''************************************************************************************
    '                ''''''FINE IMMOBILI DICHIARATI
    '                ''''''************************************************************************************

    '                ''''''************************************************************************************
    '                ''''''ELENCO VERSAMENTI
    '                ''''''************************************************************************************

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "elenco_versamenti"
    '                objBookmark.Valore = strVersamenti
    '                oArrBookmark.Add(objBookmark)

    '                ''''''************************************************************************************
    '                ''''''FINE ELENCO VERSAMENTI
    '                ''''''************************************************************************************


    '                ArrayBookMark = CType(oArrBookmark.ToArray(GetType(oggettiStampa)), oggettiStampa())

    '                objToPrint = New oggettoDaStampareCompleto
    '                objToPrint.TestataDOC = objTestataDOC
    '                objToPrint.TestataDOT = objTestataDOT
    '                objToPrint.Stampa = ArrayBookMark

    '                'oArrListOggettiDaStampare = New ArrayList
    '                oArrListOggettiDaStampare.Add(objToPrint)


    '                dr = DSperInsertTabStorico.Tables("DS_TAB_STORICO_DOCUMENTI").NewRow

    '                dr("COD_CONTRIBUENTE") = iCodContrib
    '                dr("COD_ENTE") = ConstSession.IdEnte
    '                dr("ANNO") = sElencoAnni
    '                dr("TIPO_DOCUMENTO") = CostantiProvv.COD_TIPO_DOC_RICHIESTAINFO
    '                dr("COD_TRIBUTO") = Tributo
    '                dr("DATA_ELABORAZIONE") = DateTime.Now.ToString("yyyyMMdd")
    '                dr("NOTE") = sNoteContrib
    '                dr("URL_DOCUMENTO") = "-1"

    '                DSperInsertTabStorico.Tables("DS_TAB_STORICO_DOCUMENTI").Rows.Add(dr)

    '            End If


    '        Next

    '        Dim GruppoDOC As New GruppoDocumenti
    '        Dim GruppoDOCUMENTI As GruppoDocumenti()
    '        Dim ArrListGruppoDOC As New ArrayList

    '        Dim ArrOggCompleto As oggettoDaStampareCompleto()
    '        Dim objTestataGruppo As New oggettoTestata
    '        Dim icontaUrl As Integer

    '        ArrOggCompleto = CType(oArrListOggettiDaStampare.ToArray(GetType(oggettoDaStampareCompleto)), oggettoDaStampareCompleto())

    '        GruppoDOC.OggettiDaStampare = ArrOggCompleto
    '        GruppoDOC.TestataGruppo = objTestataGruppo

    '        ArrListGruppoDOC.Add(GruppoDOC)

    '        GruppoDOCUMENTI = CType(ArrListGruppoDOC.ToArray(GetType(GruppoDocumenti)), GruppoDocumenti())

    '        'oggettoDaStampare[]  outArray = (oggettoDaStampare[])oArrListOggettiDaStampare.ToArray(typeof(oggettoDaStampare));

    '        ' oggetto per la stampa
    '        Dim oInterfaceStampaDocOggetti As IElaborazioneStampaDocOggetti
    '        oInterfaceStampaDocOggetti = Activator.GetObject(GetType(IElaborazioneStampaDocOggetti), ConfigurationManager.AppSettings("ServizioStampe").ToString())

    '        Dim retArray As GruppoURL

    '        Dim objTestataComplessiva As New RIBESElaborazioneDocumentiInterface.Stampa.oggetti.oggettoTestata
    '        objTestataComplessiva.Atto = ""
    '        objTestataComplessiva.Dominio = ""
    '        objTestataComplessiva.Ente = ""
    '        objTestataComplessiva.Filename = ""
    '        ' definisco anche il numero di documenti che voglio stampare.
    '        '*** 201511 - template documenti per ruolo ***
    '        'retArray = oInterfaceStampaDocOggetti.StampaDocumenti(objTestataComplessiva, GruppoDOCUMENTI)
    '        retArray = oInterfaceStampaDocOggetti.StampaDocumenti(ConstSession.PathStampe, ConstSession.PathVirtualStampe, objTestataComplessiva, GruppoDOCUMENTI, True, False)
    '        'retArray = oInterfaceStampaDocOggetti.StampaDocumenti(objTestataComplessiva, GruppoDOCUMENTI, True, False)

    '        Session.Remove("ELENCO_LIQUIDAZIONI_STAMPATE")
    '        If Not retArray Is Nothing Then

    '            Session.Add("ELENCO_LIQUIDAZIONI_STAMPATE", retArray)

    '            Dim sUrl As String
    '            Dim strFileName As String
    '            Dim CodContr As String
    '            Dim DRperInsertTabStorico() As DataRow

    '            For icontaUrl = 0 To retArray.URLDocumenti.Length - 1
    '                sUrl = retArray.URLDocumenti(icontaUrl).Url()
    '                strFileName = retArray.URLDocumenti(icontaUrl).Name()

    '                CodContr = strFileName.Substring(0, strFileName.IndexOf("_"))

    '                DRperInsertTabStorico = DSperInsertTabStorico.Tables(0).Select("COD_CONTRIBUENTE='" & CodContr & "'")
    '                If DRperInsertTabStorico.Length = 1 Then
    '                    DRperInsertTabStorico(0)("URL_DOCUMENTO") = sUrl
    '                End If

    '            Next

    '            If objCOM.setTAB_STORICO_DOCUMENTI(objHashTable, DSperInsertTabStorico) = True Then

    '                Dim sScript As String = ""
    '                sScript += "parent.frames.item('corpo').location.href='ViewLiquidazioniStampate.aspx'" & vbCrLf
    '                RegisterScript(sScript, Me.GetType())

    '            End If

    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleL.STAMPA_LETTERA_RICHIESTA_INFO.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    'Private Sub STAMPA_LETTERA_CONTATTO(Tributo As String)
    '    Dim sFilenameDOT, sFilenameDOC As String
    '    Dim objDSstampa As DataSet
    '    'contiene i dati anagrafici visualizzati nella grilgia della videata di gestione elaborazione
    '    objDSstampa = CType(Session("PROVVEDIMENTI_DA_STAMPARE"), DataSet)

    '    'AGGIUNGO LA COLONNA SCARTATO AL DATASET DELLA STAMPA-------------------------------------------------------------------------------------
    '    Dim NewColumn As New DataColumn
    '    NewColumn.ColumnName = "SCARTATO"
    '    NewColumn.DataType = System.Type.GetType("System.Boolean")
    '    NewColumn.DefaultValue = 0
    '    objDSstampa.Tables(0).Columns.Add(NewColumn)

    '    ' creo l'oggetto testata per l'oggetto da stampare
    '    'serve per indicare la posizione di salvataggio e il nome del file.
    '    Dim objTestataDOC As oggettoTestata
    '    Dim objTestataDOT As New oggettoTestata
    '    Dim IDPROVVEDIMENTO As String
    '    Dim IDPROCEDIMENTO As String
    '    Dim sANNO As String

    '    ' COMPONGO IL PERCORSO DEL FILE DOT

    '    sFilenameDOT = "LetteraRichiestaInfo.doc"
    '    sFilenameDOC = "LetteraRichiestaInfo"

    '    objTestataDOT.Atto = "Template"
    '    objTestataDOT.Dominio = "Provvedimenti"
    '    objTestataDOT.Ente = ConstSession.DescrizioneEnte
    '    objTestataDOT.Filename = sFilenameDOT

    '    'objTestataDOC.Atto = "Documenti"
    '    'objTestataDOC.Dominio = "Provvedimenti"
    '    'objTestataDOC.Ente = ConstSession.DescrizioneEnte
    '    'objTestataDOC.Filename = sFilenameDOC

    '    Dim oArrBookmark As ArrayList
    '    Dim iCount As Integer

    '    Dim objBookmark As oggettiStampa
    '    Dim oArrListOggettiDaStampare As New ArrayList
    '    Dim objToPrint As oggettoDaStampareCompleto
    '    Dim ArrayBookMark As oggettiStampa()
    '    Dim iCodContrib As Integer
    '    Dim sNoteContrib As String

    '    Dim strRigaImmobili As String
    '    Dim strRigaVersamenti As String
    '    Dim strImmobili As String = String.Empty
    '    Dim strVersamenti As String = String.Empty

    '    Dim Anno As String = String.Empty
    '    Dim sElencoAnni As String = String.Empty

    '    Dim dsImmobiliDichiarato As New DataSet
    '    Dim dsVersamenti As New DataSet
    '    Dim dsImmobiliCatasto As New DataSet
    '    'Dim strConnectionStringOPENgovProvvedimenti As String

    '    Dim objHashTable As Hashtable = New Hashtable

    '    'Dim objSessione As CreateSessione
    '    Dim objCOM As IElaborazioneLiquidazioni = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneLiquidazioni), ConstSession.URLServiziLiquidazioni)
    '    'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString

    '    objHashTable.Add("CodENTE", ConstSession.IdEnte)
    '    objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)

    '    Dim DSperInsertTabStorico As DataSet = CreateDataSetStampa()

    '    Dim drStampa() As DataRow
    '    Dim dr As DataRow
    '    Try
    '        For iCount = 0 To objDSstampa.Tables(0).Rows.Count - 1

    '            If objDSstampa.Tables(0).Rows(iCount)("SCARTATO") = False Then

    '                iCodContrib = objDSstampa.Tables(0).Rows(iCount)("COD_CONTRIBUENTE")

    '                objTestataDOC = New oggettoTestata
    '                objTestataDOC.Atto = "Documenti"
    '                objTestataDOC.Dominio = "Provvedimenti"
    '                objTestataDOC.Ente = ConstSession.DescrizioneEnte
    '                objTestataDOC.Filename = iCodContrib & "_" & ConstSession.IdEnte & sFilenameDOC
    '                sNoteContrib = objDSstampa.Tables(0).Rows(iCount)("NOTEPREACC")

    '                IDPROVVEDIMENTO = objDSstampa.Tables(0).Rows(iCount)("ID_PROVVEDIMENTO")
    '                sANNO = objDSstampa.Tables(0).Rows(iCount)("ANNO")

    '                oArrBookmark = New ArrayList

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "nome_ente"
    '                objBookmark.Valore = CStr(Session("DESCRIZIONE_ENTE")).ToUpper
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "note_contrib"
    '                objBookmark.Valore = sNoteContrib
    '                oArrBookmark.Add(objBookmark)

    '                '************************************************************************************
    '                'DATI ANAGRAFICI
    '                '************************************************************************************
    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "cognome"
    '                objBookmark.Valore = FormatStringToEmpty(objDSstampa.Tables(0).Rows(iCount)("COGNOME"))
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "nome"
    '                objBookmark.Valore = FormatStringToEmpty(objDSstampa.Tables(0).Rows(iCount)("NOME"))
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "via_residenza"
    '                objBookmark.Valore = FormatStringToEmpty(objDSstampa.Tables(0).Rows(iCount)("VIA_RES"))
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "civico_residenza"
    '                objBookmark.Valore = FormatStringToEmpty(objDSstampa.Tables(0).Rows(iCount)("CIVICO_RES"))
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "frazione_residenza"
    '                objBookmark.Valore = FormatStringToEmpty(objDSstampa.Tables(0).Rows(iCount)("FRAZIONE_RES"))
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "cap_residenza"
    '                objBookmark.Valore = FormatStringToEmpty(objDSstampa.Tables(0).Rows(iCount)("CAP_RES"))
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "citta_residenza"
    '                objBookmark.Valore = FormatStringToEmpty(objDSstampa.Tables(0).Rows(iCount)("CITTA_RES"))
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "prov_residenza"
    '                If CStr(objDSstampa.Tables(0).Rows(iCount)("PROVINCIA_RES")).CompareTo("") <> 0 Then
    '                    objBookmark.Valore = "(" & objDSstampa.Tables(0).Rows(iCount)("PROVINCIA_RES") & ")"
    '                Else
    '                    objBookmark.Valore = ""
    '                End If
    '                'objBookmark.Valore = "(" & objDSstampa.Tables(0).Rows(iCount)("PROVINCIA_RES") & ")"
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "codice_fiscale"
    '                objBookmark.Valore = FormatStringToEmpty(objDSstampa.Tables(0).Rows(iCount)("CODICE_FISCALE"))
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "partita_iva"
    '                objBookmark.Valore = FormatStringToEmpty(objDSstampa.Tables(0).Rows(iCount)("PARTITA_IVA"))
    '                oArrBookmark.Add(objBookmark)

    '                '---------------------------------------------------------------------------------

    '                drStampa = objDSstampa.Tables(0).Select("COD_CONTRIBUENTE='" & iCodContrib & "'")

    '                sElencoAnni = "" : strImmobili = "" : strVersamenti = "" : strRigaImmobili = "" : strRigaVersamenti = ""

    '                'For x = 0 To drstampa.Length - 1
    '                For Each row As DataRow In objDSstampa.Tables(0).Select("COD_CONTRIBUENTE='" & iCodContrib & "'")

    '                    IDPROCEDIMENTO = row("ID_PROCEDIMENTO")
    '                    sElencoAnni = sElencoAnni & row("ANNO") & ","

    '                    ''''''************************************************************************************
    '                    ''''''ELENCO IMMOBILI DICHIARATI
    '                    ''''''************************************************************************************
    '                    dsImmobiliDichiarato = objCOM.getImmobiliDichiaratiPerStampaLiquidazione(objHashTable, IDPROCEDIMENTO)
    '                    strRigaImmobili = FillBookMarkDICHIARATO(dsImmobiliDichiarato, CBool(ConfigurationManager.AppSettings("CONFIGURAZIONE_DICHIARAZIONE").ToString()))
    '                    strImmobili = strImmobili & "Dettaglio Immobili Anno " & row("ANNO") & vbCrLf & strRigaImmobili & vbCrLf & vbCrLf

    '                    dsVersamenti = objCOM.getVersamentiPerStampaLiquidazione(objHashTable, IDPROCEDIMENTO, oggettoatto.fase.versatodichiarato)
    '                    strRigaVersamenti = FillBookMarkVersamenti(dsVersamenti, CBool(ConfigurationManager.AppSettings("CONFIGURAZIONE_DICHIARAZIONE").ToString()))
    '                    strVersamenti = strVersamenti & "Dettaglio Versamenti (Ordinari e Ravvedimento Operoso) Anno " & row("ANNO") & vbCrLf & strRigaVersamenti & vbCrLf & vbCrLf

    '                    row("SCARTATO") = True

    '                Next

    '                sElencoAnni = Left(sElencoAnni, Len(sElencoAnni) - 1)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "elenco_immobili_dich"
    '                objBookmark.Valore = strImmobili
    '                oArrBookmark.Add(objBookmark)

    '                ''''''************************************************************************************
    '                ''''''FINE IMMOBILI DICHIARATI
    '                ''''''************************************************************************************

    '                ''''''************************************************************************************
    '                ''''''ELENCO VERSAMENTI
    '                ''''''************************************************************************************

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "elenco_versamenti"
    '                objBookmark.Valore = strVersamenti
    '                oArrBookmark.Add(objBookmark)

    '                ''''''************************************************************************************
    '                ''''''FINE ELENCO VERSAMENTI
    '                ''''''************************************************************************************


    '                ArrayBookMark = CType(oArrBookmark.ToArray(GetType(oggettiStampa)), oggettiStampa())

    '                objToPrint = New oggettoDaStampareCompleto
    '                objToPrint.TestataDOC = objTestataDOC
    '                objToPrint.TestataDOT = objTestataDOT
    '                objToPrint.Stampa = ArrayBookMark

    '                'oArrListOggettiDaStampare = New ArrayList
    '                oArrListOggettiDaStampare.Add(objToPrint)


    '                dr = DSperInsertTabStorico.Tables("DS_TAB_STORICO_DOCUMENTI").NewRow

    '                dr("COD_CONTRIBUENTE") = iCodContrib
    '                dr("COD_ENTE") = ConstSession.IdEnte
    '                dr("ANNO") = sElencoAnni
    '                dr("TIPO_DOCUMENTO") = CostantiProvv.COD_TIPO_DOC_LETTERA_CONTATTO
    '                dr("COD_TRIBUTO") = Tributo
    '                dr("DATA_ELABORAZIONE") = DateTime.Now.ToString("yyyyMMdd")
    '                dr("NOTE") = sNoteContrib
    '                dr("URL_DOCUMENTO") = "-1"

    '                DSperInsertTabStorico.Tables("DS_TAB_STORICO_DOCUMENTI").Rows.Add(dr)

    '            End If


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

    '        'oggettoDaStampare[]  outArray = (oggettoDaStampare[])oArrListOggettiDaStampare.ToArray(typeof(oggettoDaStampare));

    '        ' oggetto per la stampa
    '        Dim oInterfaceStampaDocOggetti As IElaborazioneStampaDocOggetti
    '        oInterfaceStampaDocOggetti = Activator.GetObject(GetType(IElaborazioneStampaDocOggetti), ConfigurationManager.AppSettings("ServizioStampe").ToString())

    '        Dim retArray As GruppoURL

    '        Dim objTestataComplessiva As New RIBESElaborazioneDocumentiInterface.Stampa.oggetti.oggettoTestata
    '        objTestataComplessiva.Atto = ""
    '        objTestataComplessiva.Dominio = ""
    '        objTestataComplessiva.Ente = ""
    '        objTestataComplessiva.Filename = ""
    '        ' definisco anche il numero di documenti che voglio stampare.
    '        '*** 201511 - template documenti per ruolo ***
    '        'retArray = oInterfaceStampaDocOggetti.StampaDocumenti(objTestataComplessiva, GruppoDOCUMENTI)
    '        retArray = oInterfaceStampaDocOggetti.StampaDocumenti(ConstSession.PathStampe, ConstSession.PathVirtualStampe, objTestataComplessiva, GruppoDOCUMENTI, True, False)
    '        'retArray = oInterfaceStampaDocOggetti.StampaDocumenti(objTestataComplessiva, GruppoDOCUMENTI, True, False)

    '        Session.Remove("ELENCO_LIQUIDAZIONI_STAMPATE")
    '        If Not retArray Is Nothing Then

    '            Session.Add("ELENCO_LIQUIDAZIONI_STAMPATE", retArray)

    '            Dim sUrl As String
    '            Dim strFileName As String
    '            Dim CodContr As String
    '            Dim DRperInsertTabStorico() As DataRow
    '            Dim icontaUrl As Integer

    '            For icontaUrl = 0 To retArray.URLDocumenti.Length - 1
    '                sUrl = retArray.URLDocumenti(icontaUrl).Url()
    '                strFileName = retArray.URLDocumenti(icontaUrl).Name()

    '                CodContr = strFileName.Substring(0, strFileName.IndexOf("_"))

    '                DRperInsertTabStorico = DSperInsertTabStorico.Tables(0).Select("COD_CONTRIBUENTE='" & CodContr & "'")
    '                If DRperInsertTabStorico.Length = 1 Then
    '                    DRperInsertTabStorico(0)("URL_DOCUMENTO") = sUrl
    '                End If

    '            Next

    '            If objCOM.setTAB_STORICO_DOCUMENTI(objHashTable, DSperInsertTabStorico) = True Then

    '                Dim sScript As String = ""
    '                sScript += "parent.frames.item('corpo').location.href='ViewLiquidazioniStampate.aspx'" & vbCrLf
    '                RegisterScript(sScript, Me.GetType())

    '            End If

    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleL.STAMPA_LETTERA_CONTATTO.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Tributo"></param>
    ''' <param name="strTipoDoc"></param>
    ''' <param name="bCreaPDF"></param>
    Private Sub STAMPA_BOLLETTINI_PRE_e_ACCERTAMENTO(Tributo As String, ByVal strTipoDoc As String, ByVal bCreaPDF As Boolean)
        Dim sFilenameDOT, sFilenameDOC, FileNameContrib As String
        Dim objDSstampa As DataSet
        Dim objUtility As New MyUtility
        'contiene i dati anagrafici visualizzati nella grilgia della ricerca lettere
        objDSstampa = CType(Session("PROVVEDIMENTI_DA_STAMPARE"), DataSet)

        ' creo l'oggetto testata per l'oggetto da stampare
        'serve per indicare la posizione di salvataggio e il nome del file.
        Dim objTestataDOC As oggettoTestata
        Dim objTestataDOT As New oggettoTestata
        Dim IDPROVVEDIMENTO As String
        Dim oArrBookmark As ArrayList
        Dim iCount As Integer

        Dim objBookmark As oggettiStampa
        Dim oArrListOggettiDaStampare As New ArrayList
        Dim objToPrint As oggettoDaStampareCompleto
        Dim ArrayBookMark As oggettiStampa()
        Dim iCodContrib As Integer
        Dim dsDatiProvv As New DataSet
        '---------------------------------------
        'var per popolare i bookmark relativi 
        'alla sezione degli importi interessi
        Dim sDataAtto As String
        Dim sDataAtto1 As String

        Try
            ' COMPONGO IL PERCORSO DEL FILE DOT
            If Tributo = Utility.Costanti.TRIBUTO_TARSU Then
                sFilenameDOT = "BollettinoViolazioneTARSU_"
                sFilenameDOC = "BollettinoViolazioneTARSU"
            ElseIf Tributo = Utility.Costanti.TRIBUTO_OSAP Then
                sFilenameDOT = "BollettinoViolazioneOSAP_"
                sFilenameDOC = "BollettinoViolazioneOSAP"
            Else
                sFilenameDOT = "BollettinoViolazioneICI_"
                sFilenameDOC = "BollettinoViolazioneICI"
            End If
            sFilenameDOT += ConstSession.IdEnte + ".doc"

            objTestataDOT.Atto = "Template"
            objTestataDOT.Dominio = "Provvedimenti"
            objTestataDOT.Ente = ConstSession.DescrizioneEnte.Replace("Comune di ", "")
            objTestataDOT.Filename = sFilenameDOT

            Dim objHashTable As Hashtable = New Hashtable
            objHashTable.Add("CodENTE", ConstSession.IdEnte)
            objHashTable.Add("CODENTE", ConstSession.IdEnte)
            objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)

            Dim objCOM As IElaborazioneLiquidazioni = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneLiquidazioni), ConstSession.URLServiziLiquidazioni)

            For iCount = 0 To objDSstampa.Tables(0).Rows.Count - 1
                FileNameContrib = objDSstampa.Tables(0).Rows(iCount)("FileNameContrib")
                iCodContrib = objDSstampa.Tables(0).Rows(iCount)("COD_CONTRIBUENTE")

                oArrBookmark = New ArrayList

                IDPROVVEDIMENTO = objDSstampa.Tables(0).Rows(iCount)("ID_PROVVEDIMENTO")
                objHashTable.Add("COD_CONTRIBUENTE", iCodContrib)

                dsDatiProvv = objCOM.GetProvvedimentoPerStampaLiquidazione(ConstSession.StringConnection, objHashTable, IDPROVVEDIMENTO)

                '*****************************************
                'DATI ANAGRAFICI
                '*****************************************
                'cognome
                '*****************************************
                Dim sCognome, sNome As String
                sCognome = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("COGNOME"))
                sNome = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("NOME"))
                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_cognome"
                objBookmark.Valore = sCognome
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_cognome_1"
                objBookmark.Valore = sCognome
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_cognome_2"
                objBookmark.Valore = sCognome
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_cognome_3"
                objBookmark.Valore = sCognome
                oArrBookmark.Add(objBookmark)
                '*****************************************
                'nome
                '*****************************************
                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_nome"
                objBookmark.Valore = sNome
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_nome_1"
                objBookmark.Valore = sNome
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_nome_2"
                objBookmark.Valore = sNome
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_nome_3"
                objBookmark.Valore = sNome
                oArrBookmark.Add(objBookmark)

                Dim sCAP, sCitta, sProvincia, sVia, sCivico As String

                sCAP = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("CAP_RES"))
                sCitta = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("CITTA_RES"))
                sProvincia = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("PROVINCIA_RES"))
                If CStr(sProvincia).CompareTo("") <> 0 Then
                    sProvincia = "(" & sProvincia & ")"
                Else
                    sProvincia = ""
                End If
                sVia = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("VIA_RES"))
                sCivico = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("CIVICO_RES"))

                '*****************************************
                'cap res/CO
                '*****************************************
                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_cap_res"
                objBookmark.Valore = sCAP
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_cap_res_1"
                objBookmark.Valore = sCAP
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_cap_res_2"
                objBookmark.Valore = sCAP
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_cap_res_3"
                objBookmark.Valore = sCAP
                oArrBookmark.Add(objBookmark)
                '*****************************************
                'città res/CO
                '*****************************************
                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_citta_res"
                objBookmark.Valore = sCitta
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_citta_res_1"
                objBookmark.Valore = sCitta
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_citta_res_2"
                objBookmark.Valore = sCitta
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_citta_res_3"
                objBookmark.Valore = sCitta
                oArrBookmark.Add(objBookmark)
                '*****************************************
                'prov res/CO
                '*****************************************
                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_prov_res"
                objBookmark.Valore = sProvincia
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_prov_res_1"
                objBookmark.Valore = sProvincia
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_prov_res_2"
                objBookmark.Valore = sProvincia
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_prov_res_3"
                objBookmark.Valore = sProvincia
                oArrBookmark.Add(objBookmark)
                '*****************************************
                'via res/CO
                '*****************************************
                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_via_res"
                objBookmark.Valore = sVia
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_via_res_1"
                objBookmark.Valore = sVia
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_via_res_2"
                objBookmark.Valore = sVia
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_via_res_3"
                objBookmark.Valore = sVia
                oArrBookmark.Add(objBookmark)
                '*****************************************
                'civico res/CO
                '*****************************************
                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_civico_res"
                objBookmark.Valore = sCivico
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_civico_res_1"
                objBookmark.Valore = sCivico
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_civico_res_2"
                objBookmark.Valore = sCivico
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_civico_res_3"
                objBookmark.Valore = sCivico
                oArrBookmark.Add(objBookmark)
                '*****************************************
                'codice fiscale
                '*****************************************
                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_cod_fiscale"
                objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("CODICE_FISCALE"))
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_cod_fiscale_1"
                objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("CODICE_FISCALE"))
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_cod_fiscale_2"
                objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("CODICE_FISCALE"))
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_cod_fiscale_3"
                objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("CODICE_FISCALE"))
                oArrBookmark.Add(objBookmark)

                'objBookmark = New oggettiStampa
                'objBookmark.Descrizione = "partita_iva"
                'objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("PARTITA_IVA"))
                'oArrBookmark.Add(objBookmark)
                '*****************************************
                'numero provvedimento
                '*****************************************
                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_numero_atto"
                'objBookmark.Valore = dsDatiProvv.Tables(0).Rows(iCount)("NUMERO_AVVISO")
                objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("NUMERO_ATTO"))
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_numero_atto_1"
                'objBookmark.Valore = dsDatiProvv.Tables(0).Rows(iCount)("NUMERO_AVVISO")
                objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("NUMERO_ATTO"))
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_numero_atto_2"
                'objBookmark.Valore = dsDatiProvv.Tables(0).Rows(iCount)("NUMERO_AVVISO")
                objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("NUMERO_ATTO"))
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_numero_atto_3"
                'objBookmark.Valore = dsDatiProvv.Tables(0).Rows(iCount)("NUMERO_AVVISO")
                objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("NUMERO_ATTO"))
                oArrBookmark.Add(objBookmark)
                '*****************************************
                'data provvedimento
                '*****************************************
                sDataAtto = objUtility.GiraDataFromDB(dsDatiProvv.Tables(0).Rows(iCount)("DATA_ELABORAZIONE"))
                sDataAtto1 = objUtility.GiraDataFromDB(dsDatiProvv.Tables(0).Rows(iCount)("DATA_ELABORAZIONE")).Replace("/", "")
                sDataAtto1 = sDataAtto1.Substring(0, 4) + sDataAtto1.Substring(6, 2)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_data_atto"
                objBookmark.Valore = sDataAtto
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_data_atto_1"
                objBookmark.Valore = sDataAtto1
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_data_atto_2"
                objBookmark.Valore = sDataAtto
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_data_atto_3"
                objBookmark.Valore = sDataAtto1
                oArrBookmark.Add(objBookmark)
                '*****************************************
                'importo provvedimento
                '*****************************************
                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_tot_dovuto"
                objBookmark.Valore = FormatNumberToZero(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_TOTALE_RIDOTTO")).Replace(",", "").Replace(".", "")
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_tot_dovuto_1"
                objBookmark.Valore = FormatNumberToZero(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_TOTALE_RIDOTTO")).Replace(",", "").Replace(".", "")
                oArrBookmark.Add(objBookmark)
                '*****************************************
                'stampa dell'importo in lettere
                '*****************************************
                Dim sValPrint As String
                Dim sValDecimal As String
                Dim sValIntero As String
                Dim sVal As String = String.Empty

                sVal = EuroForGridView(dsDatiProvv.Tables(0).Rows(iCount)("IMPORTO_TOTALE_RIDOTTO").ToString())
                sValIntero = sVal.Substring(0, sVal.Length - 3)
                sValDecimal = sVal.Substring(sVal.Length - 2, 2)
                sValPrint = NumberToText(CInt(sValIntero)).ToUpper() + "/" + sValDecimal

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_imp_lett"
                objBookmark.Valore = sValPrint
                oArrBookmark.Add(objBookmark)

                objBookmark = New oggettiStampa
                objBookmark.Descrizione = "IV_imp_lett_1"
                objBookmark.Valore = sValPrint
                oArrBookmark.Add(objBookmark)


                ArrayBookMark = CType(oArrBookmark.ToArray(GetType(oggettiStampa)), oggettiStampa())

                objTestataDOC = New oggettoTestata
                objTestataDOC.Atto = "Documenti"
                objTestataDOC.Dominio = "Provvedimenti"
                objTestataDOC.Ente = ConstSession.DescrizioneEnte.ToUpper.Replace("Comune di ", "")
                objTestataDOC.Filename = FileNameContrib & "_" & dsDatiProvv.Tables(0).Rows(iCount)("NUMERO_ATTO").ToString.Replace("/", "") & "_" & sFilenameDOC


                objToPrint = New oggettoDaStampareCompleto
                objToPrint.TestataDOC = objTestataDOC
                objToPrint.TestataDOT = objTestataDOT
                objToPrint.Stampa = ArrayBookMark

                oArrListOggettiDaStampare.Add(objToPrint)
            Next

            'Dim GruppoDOC As New GruppoDocumenti
            'Dim GruppoDOCUMENTI As GruppoDocumenti()
            'Dim ArrListGruppoDOC As New ArrayList

            'Dim ArrOggCompleto As oggettoDaStampareCompleto()
            'Dim objTestataGruppo As New oggettoTestata

            'ArrOggCompleto = CType(oArrListOggettiDaStampare.ToArray(GetType(oggettoDaStampareCompleto)), oggettoDaStampareCompleto())

            'GruppoDOC.OggettiDaStampare = ArrOggCompleto
            'GruppoDOC.TestataGruppo = objTestataGruppo

            'ArrListGruppoDOC.Add(GruppoDOC)

            'GruppoDOCUMENTI = CType(ArrListGruppoDOC.ToArray(GetType(GruppoDocumenti)), GruppoDocumenti())

            'Dim oInterfaceStampaDocOggetti As IElaborazioneStampaDocOggetti
            'oInterfaceStampaDocOggetti = Activator.GetObject(GetType(IElaborazioneStampaDocOggetti), ConfigurationManager.AppSettings("ServizioStampe").ToString())

            'Dim retArray As GruppoURL

            'Dim objTestataComplessiva As New RIBESElaborazioneDocumentiInterface.Stampa.oggetti.oggettoTestata
            'objTestataComplessiva.Atto = ""
            'objTestataComplessiva.Dominio = ""
            'objTestataComplessiva.Ente = ""
            'objTestataComplessiva.Filename = ""

            Dim GruppoDOC As New GruppoDocumenti
            Dim GruppoDOCUMENTI As GruppoDocumenti()
            Dim ArrListGruppoDOC As New ArrayList

            Dim ArrOggCompleto As oggettoDaStampareCompleto()
            Dim objTestataGruppo As New oggettoTestata

            objTestataGruppo.Atto = objTestataDOC.Atto
            objTestataGruppo.Dominio = objTestataDOC.Dominio
            objTestataGruppo.Ente = objTestataDOC.Ente
            If Tributo = Utility.Costanti.TRIBUTO_TARSU Then
                objTestataGruppo.Filename = "BollettinoViolazione_TARSU_" & iCodContrib.ToString() & "_" & ConstSession.IdEnte
            ElseIf Tributo = Utility.Costanti.TRIBUTO_OSAP Then
                objTestataGruppo.Filename = "BollettinoViolazione_OSAP_" & iCodContrib.ToString() & "_" & ConstSession.IdEnte
            Else
                objTestataGruppo.Filename = "BollettinoViolazione_ICI_" & iCodContrib.ToString() & "_" & ConstSession.IdEnte
            End If


            ArrOggCompleto = CType(oArrListOggettiDaStampare.ToArray(GetType(oggettoDaStampareCompleto)), oggettoDaStampareCompleto())

            GruppoDOC.OggettiDaStampare = ArrOggCompleto
            GruppoDOC.TestataGruppo = objTestataGruppo

            ArrListGruppoDOC.Add(GruppoDOC)

            GruppoDOCUMENTI = CType(ArrListGruppoDOC.ToArray(GetType(GruppoDocumenti)), GruppoDocumenti())

            Dim oInterfaceStampaDocOggetti As IElaborazioneStampaDocOggetti
            oInterfaceStampaDocOggetti = Activator.GetObject(GetType(IElaborazioneStampaDocOggetti), ConfigurationManager.AppSettings("ServizioStampe").ToString())

            Dim retArray As GruppoURL

            Dim objTestataComplessiva As New RIBESElaborazioneDocumentiInterface.Stampa.oggetti.oggettoTestata
            objTestataComplessiva.Atto = objTestataDOC.Atto
            objTestataComplessiva.Dominio = objTestataDOC.Dominio
            objTestataComplessiva.Ente = objTestataDOC.Ente
            If Tributo = Utility.Costanti.TRIBUTO_TARSU Then
                objTestataComplessiva.Filename = "BollettiniViolazione_TARSU_" & iCodContrib.ToString() & "_" & ConstSession.IdEnte
            ElseIf Tributo = Utility.Costanti.TRIBUTO_OSAP Then
                objTestataComplessiva.Filename = "BollettiniViolazione_OSAP_" & iCodContrib.ToString() & "_" & ConstSession.IdEnte
            Else
                objTestataComplessiva.Filename = "BollettiniViolazione_ICI_" & iCodContrib.ToString() & "_" & ConstSession.IdEnte
            End If

            '************************************************************
            ' definisco anche il numero di documenti che voglio stampare.
            '************************************************************
            'retArray = oInterfaceStampaDocOggetti.StampaDocumenti(objTestataComplessiva, GruppoDOCUMENTI, True, bCreaPDF)
            'ReDim retArray1(0)
            'retArray1(0) = retArray
            'Session.Remove("ELENCO_BOLLETTINI_STAMPATI")
            'If Not retArray Is Nothing Then
            '    Session.Add("ELENCO_BOLLETTINI_STAMPATI", retArray1)
            '    dim sScript as string=""
            '    sscript+= "parent.frames.item('corpo').location.href='ViewBollettiniViolazioneStampati.aspx'" & vbCrLf
            '    RegisterScript(sScript , Me.GetType())
            'End If
            '*** 201511 - template documenti per ruolo ***
            'retArray = oInterfaceStampaDocOggetti.StampaDocumenti(objTestataComplessiva, GruppoDOCUMENTI)
            retArray = oInterfaceStampaDocOggetti.StampaDocumenti(ConstSession.PathStampe, ConstSession.PathVirtualStampe, objTestataComplessiva, GruppoDOCUMENTI, True, False)
            'retArray = oInterfaceStampaDocOggetti.StampaDocumenti(objTestataComplessiva, GruppoDOCUMENTI, True, False)
            Session.Remove("ELENCO_ACCERTAMENTI_STAMPATI")
            If Not retArray Is Nothing Then
                Session.Add("ELENCO_ACCERTAMENTI_STAMPATI", retArray)
                Dim sScript As String = ""
                sScript += "parent.corpo.location.href='../../GestioneAccertamenti/StampaAccertamenti/ViewAccertamentiStampati.aspx';"
                sScript += "parent.parent.document.getElementById('DivAttesa').style.display='none';"
                RegisterScript(sScript, Me.GetType())
            End If
            'If Not IsNothing(objSessione) Then
            '    objSessione.Kill()
            '    objSessione = Nothing
            'End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleL.STAMPA_BOLLETTINI_PRE_e_ACCERTAMENTO.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
            Throw New Exception("STAMPA_BOLLETTINI_PRE_e_ACCERTAMENTO::si è verificato il seguente errore::" & ex.Message)
        End Try
    End Sub
    'Private Sub STAMPA_BOLLETTINI_PRE_e_ACCERTAMENTO_RATEIZZATI(Tributo As String, ByVal strTipoDoc As String, ByVal bCreaPDF As Boolean)
    '    Try
    '        Dim sFilenameDOT, sFilenameDOC As String
    '        Dim objDSstampa As DataSet
    '        Dim objUtility As New MyUtility
    '        'contiene i dati anagrafici visualizzati nella grilgia della ricerca lettere
    '        objDSstampa = CType(Session("PROVVEDIMENTI_DA_STAMPARE"), DataSet)

    '        ' creo l'oggetto testata per l'oggetto da stampare
    '        'serve per indicare la posizione di salvataggio e il nome del file.
    '        Dim objTestataDOC As oggettoTestata
    '        Dim objTestataDOT As oggettoTestata
    '        Dim IDPROVVEDIMENTO As String
    '        Dim IDPROCEDIMENTO As String
    '        Dim strTIPODOCUMENTO As String
    '        Dim strANNO As String

    '        Dim oArrBookmark As ArrayList
    '        Dim iCount As Integer
    '        'Dim iImmobili As Integer
    '        'Dim iErrori As Integer
    '        Dim objBookmark As oggettiStampa
    '        Dim oArrListOggettiDaStampare As New ArrayList
    '        Dim objToPrint As oggettoDaStampareCompleto
    '        Dim ArrayBookMark As oggettiStampa()
    '        Dim FileNameContrib As String

    '        'Dim strRiga As String=""
    '        Dim strImmoTemp As String = String.Empty
    '        Dim strErroriTemp As String = String.Empty
    '        Dim strImmoTempTitolo As String = String.Empty
    '        Dim Anno As String = String.Empty

    '        Dim dsDatiProvv As New DataSet
    '        Dim dsDatiRate As New DataSet
    '        Dim dsImmobiliDichiarato As New DataSet
    '        Dim dsVersamenti As New DataSet
    '        Dim dsImmobiliCatasto As New DataSet
    '        Dim objDSTipiInteressi As New DataSet
    '        Dim objDSElencoSanzioni As New DataSet
    '        Dim objDSImportiInteressi As New DataSet
    '        '---------------------------------------
    '        'var per popolare i bookmark relativi 
    '        'alla sezione degli importi interessi
    '        'Dim strImportoGiorni As String
    '        'Dim strImportoSemestriACC As String
    '        'Dim strImportoSemestriSAL As String
    '        'Dim strNumSemestriACC As String
    '        'Dim strNumSemestriSAL As String
    '        'Dim iRetValImpInt As Boolean
    '        Dim sDataAtto As String
    '        Dim sDataAtto1 As String

    '        Dim iNumRate, iCountRate, numRata As Integer
    '        Dim sValoreRata, sValoreRataLettere As String
    '        Dim sNumRata As String
    '        Dim bRatePari As Boolean

    '        Dim sValPrint As String
    '        Dim sValDecimal As String
    '        Dim sValIntero As String
    '        Dim sVal As String = String.Empty

    '        '---------------------------------------
    '        'Dim DSperInsertTabStorico As DataSet = CreateDataSetStampa()
    '        'Dim dr As DataRow
    '        'Dim strConnectionStringOPENgovProvvedimenti As String
    '        'Dim objSessione As CreateSessione
    '        'Dim strWFErrore As String

    '        'strConnectionStringOPENgovProvvedimenti = objSessione.oSession.oAppDB.GetConnection.ConnectionString

    '        Dim objHashTable As Hashtable = New Hashtable
    '        objHashTable.Add("CodENTE", ConstSession.IdEnte)
    '        objHashTable.Add("CODENTE", ConstSession.IdEnte)
    '        objHashTable.Add("CONNECTIONSTRINGOPENGOVPROVVEDIMENTI", ConstSession.StringConnection)

    '        Dim objCOM As IElaborazioneLiquidazioni = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneLiquidazioni), ConstSession.URLServiziLiquidazioni)
    '        Dim objCOMTipoVoci As IGestioneConfigurazione = Activator.GetObject(GetType(ComPlusInterface.IGestioneConfigurazione), ConstSession.URLGestioneConfigurazione)
    '        Dim objCOMRicerca As IElaborazioneAtti = Activator.GetObject(GetType(ComPlusInterface.IElaborazioneAtti), ConstSession.URLServiziElaborazioneAtti)

    '        For iCount = 0 To objDSstampa.Tables(0).Rows.Count - 1
    '            FileNameContrib = objDSstampa.Tables(0).Rows(iCount)("FileNameContrib")

    '            IDPROVVEDIMENTO = objDSstampa.Tables(0).Rows(iCount)("ID_PROVVEDIMENTO")
    '            objHashTable.Add("COD_CONTRIBUENTE", objDSstampa.Tables(0).Rows(iCount)("COD_CONTRIBUENTE"))

    '            dsDatiProvv = objCOM.GetProvvedimentoPerStampaLiquidazione(objHashTable, IDPROVVEDIMENTO)

    '            'dsDatiRate = objCOMRicerca.getRateProvvedimenti(objHashTable, IDPROVVEDIMENTO)
    '            Dim FncPag As New clsPagamenti(ConstSession.StringConnection) 'clsPagamenti(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))
    '            '*** 20130125 - inserimento gestione pagato anche rispetto all'importo pieno ***
    '            dsDatiRate = FncPag.getRateProvvedimento(IDPROVVEDIMENTO, ConstSession.IdEnte, True)
    '            '*** ***

    '            iNumRate = dsDatiRate.Tables(0).Rows.Count
    '            If iNumRate Mod 2 = 0 Then
    '                bRatePari = True
    '            Else
    '                bRatePari = False
    '            End If

    '            IDPROCEDIMENTO = dsDatiProvv.Tables(0).Rows(iCount)("ID_PROCEDIMENTO")
    '            strANNO = dsDatiProvv.Tables(0).Rows(iCount)("ANNO")

    '            For iCountRate = 0 To iNumRate - 1 Step 2
    '                ' COMPONGO IL PERCORSO DEL FILE DOT
    '                If Tributo = Utility.Costanti.TRIBUTO_TARSU Then
    '                    If bRatePari Then
    '                        sFilenameDOT = "BollettinoViolazioneTARSU_" + ConstSession.IdEnte + "_2_rate.doc"
    '                        sFilenameDOC = "BollettinoViolazioneTARSU_" & FileNameContrib
    '                    Else
    '                        If iCountRate = iNumRate - 1 Then
    '                            sFilenameDOT = "BollettinoViolazioneTARSU_" + ConstSession.IdEnte + "_1_rata.doc"
    '                            sFilenameDOC = "BollettinoViolazioneTARSU_" & FileNameContrib
    '                        Else
    '                            sFilenameDOT = "BollettinoViolazioneTARSU_" + ConstSession.IdEnte + "_2_rate.doc"
    '                            sFilenameDOC = "BollettinoViolazioneTARSU_" & FileNameContrib
    '                        End If
    '                    End If
    '                ElseIf Tributo = Utility.Costanti.TRIBUTO_OSAP Then
    '                    If bRatePari Then
    '                        sFilenameDOT = "BollettinoViolazioneOSAP_" + ConstSession.IdEnte + "_2_rate.doc"
    '                        sFilenameDOC = "BollettinoViolazioneOSAP_" & FileNameContrib
    '                    Else
    '                        If iCountRate = iNumRate - 1 Then
    '                            sFilenameDOT = "BollettinoViolazioneOSAP_" + ConstSession.IdEnte + "_1_rata.doc"
    '                            sFilenameDOC = "BollettinoViolazioneOSAP_" & FileNameContrib
    '                        Else
    '                            sFilenameDOT = "BollettinoViolazioneOSAP_" + ConstSession.IdEnte + "_2_rate.doc"
    '                            sFilenameDOC = "BollettinoViolazioneOSAP_" & FileNameContrib
    '                        End If
    '                    End If
    '                Else
    '                    If bRatePari Then
    '                        sFilenameDOT = "BollettinoViolazioneICI_" + ConstSession.IdEnte + "_2_rate.doc"
    '                        sFilenameDOC = "BollettinoViolazioneICI_" & FileNameContrib
    '                    Else
    '                        If iCountRate = iNumRate - 1 Then
    '                            sFilenameDOT = "BollettinoViolazioneICI_" + ConstSession.IdEnte + "_1_rata.doc"
    '                            sFilenameDOC = "BollettinoViolazioneICI_" & FileNameContrib
    '                        Else
    '                            sFilenameDOT = "BollettinoViolazioneICI_" + ConstSession.IdEnte + "_2_rate.doc"
    '                            sFilenameDOC = "BollettinoViolazioneICI_" & FileNameContrib
    '                        End If
    '                    End If
    '                End If
    '                strTIPODOCUMENTO = CostantiProvv.COD_TIPO_DOC_BOLL_PRE_E_ACCERTAMENTO_RATEIZZATI

    '                objTestataDOT = New oggettoTestata
    '                objTestataDOT.Atto = "Template"
    '                objTestataDOT.Dominio = "Provvedimenti"
    '                objTestataDOT.Ente = ConstSession.DescrizioneEnte.ToUpper.Replace("Comune di ", "")
    '                objTestataDOT.Filename = sFilenameDOT

    '                '*********************************
    '                '****** Valorizzo bookMarks ******
    '                '*********************************
    '                oArrBookmark = New ArrayList

    '                '*****************************************
    '                'DATI ANAGRAFICI
    '                '*****************************************
    '                'cognome
    '                '*****************************************
    '                Dim sCognome, sNome As String
    '                sCognome = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("COGNOME"))
    '                sNome = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("NOME"))
    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_cognome"
    '                objBookmark.Valore = sCognome
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_cognome_1"
    '                objBookmark.Valore = sCognome
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_cognome_2"
    '                objBookmark.Valore = sCognome
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_cognome_3"
    '                objBookmark.Valore = sCognome
    '                oArrBookmark.Add(objBookmark)
    '                '*****************************************
    '                'nome
    '                '*****************************************
    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_nome"
    '                objBookmark.Valore = sNome
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_nome_1"
    '                objBookmark.Valore = sNome
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_nome_2"
    '                objBookmark.Valore = sNome
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_nome_3"
    '                objBookmark.Valore = sNome
    '                oArrBookmark.Add(objBookmark)

    '                Dim sCAP, sCitta, sProvincia, sVia, sCivico As String

    '                sCAP = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("CAP_RES"))
    '                sCitta = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("CITTA_RES"))
    '                sProvincia = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("PROVINCIA_RES"))
    '                If CStr(sProvincia).CompareTo("") <> 0 Then
    '                    sProvincia = "(" & sProvincia & ")"
    '                Else
    '                    sProvincia = ""
    '                End If
    '                sVia = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("VIA_RES"))
    '                sCivico = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("CIVICO_RES"))

    '                '*****************************************
    '                'cap res/CO
    '                '*****************************************
    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_cap_res"
    '                objBookmark.Valore = sCAP
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_cap_res_1"
    '                objBookmark.Valore = sCAP
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_cap_res_2"
    '                objBookmark.Valore = sCAP
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_cap_res_3"
    '                objBookmark.Valore = sCAP
    '                oArrBookmark.Add(objBookmark)
    '                '*****************************************
    '                'città res/CO
    '                '*****************************************
    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_citta_res"
    '                objBookmark.Valore = sCitta
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_citta_res_1"
    '                objBookmark.Valore = sCitta
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_citta_res_2"
    '                objBookmark.Valore = sCitta
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_citta_res_3"
    '                objBookmark.Valore = sCitta
    '                oArrBookmark.Add(objBookmark)
    '                '*****************************************
    '                'prov res/CO
    '                '*****************************************
    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_prov_res"
    '                objBookmark.Valore = sProvincia
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_prov_res_1"
    '                objBookmark.Valore = sProvincia
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_prov_res_2"
    '                objBookmark.Valore = sProvincia
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_prov_res_3"
    '                objBookmark.Valore = sProvincia
    '                oArrBookmark.Add(objBookmark)
    '                '*****************************************
    '                'via res/CO
    '                '*****************************************
    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_via_res"
    '                objBookmark.Valore = sVia
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_via_res_1"
    '                objBookmark.Valore = sVia
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_via_res_2"
    '                objBookmark.Valore = sVia
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_via_res_3"
    '                objBookmark.Valore = sVia
    '                oArrBookmark.Add(objBookmark)
    '                '*****************************************
    '                'civico res/CO
    '                '*****************************************
    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_civico_res"
    '                objBookmark.Valore = sCivico
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_civico_res_1"
    '                objBookmark.Valore = sCivico
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_civico_res_2"
    '                objBookmark.Valore = sCivico
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_civico_res_3"
    '                objBookmark.Valore = sCivico
    '                oArrBookmark.Add(objBookmark)
    '                '*****************************************
    '                'codice fiscale
    '                '*****************************************
    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_cod_fiscale"
    '                objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("CODICE_FISCALE"))
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_cod_fiscale_1"
    '                objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("CODICE_FISCALE"))
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_cod_fiscale_2"
    '                objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("CODICE_FISCALE"))
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_cod_fiscale_3"
    '                objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("CODICE_FISCALE"))
    '                oArrBookmark.Add(objBookmark)

    '                'objBookmark = New oggettiStampa
    '                'objBookmark.Descrizione = "partita_iva"
    '                'objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("PARTITA_IVA"))
    '                'oArrBookmark.Add(objBookmark)
    '                '*****************************************
    '                'numero provvedimento
    '                '*****************************************
    '                numRata = iCountRate + 1
    '                If numRata < 10 Then sNumRata = "0" & numRata.ToString() Else sNumRata = numRata.ToString()

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_numero_atto"
    '                'objBookmark.Valore = dsDatiProvv.Tables(0).Rows(iCount)("NUMERO_AVVISO")
    '                objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("NUMERO_ATTO") & " Rata:" & sNumRata)
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_numero_atto_1"
    '                'objBookmark.Valore = dsDatiProvv.Tables(0).Rows(iCount)("NUMERO_AVVISO")
    '                objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("NUMERO_ATTO"))
    '                oArrBookmark.Add(objBookmark)

    '                numRata += 1
    '                If numRata < 10 Then sNumRata = "0" & numRata.ToString() Else sNumRata = numRata.ToString()

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_numero_atto_2"
    '                'objBookmark.Valore = dsDatiProvv.Tables(0).Rows(iCount)("NUMERO_AVVISO")
    '                objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("NUMERO_ATTO") & " Rata:" & sNumRata)
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_numero_atto_3"
    '                'objBookmark.Valore = dsDatiProvv.Tables(0).Rows(iCount)("NUMERO_AVVISO")
    '                objBookmark.Valore = FormatStringToEmpty(dsDatiProvv.Tables(0).Rows(iCount)("NUMERO_ATTO"))
    '                oArrBookmark.Add(objBookmark)
    '                '*****************************************
    '                'data provvedimento
    '                '*****************************************
    '                sDataAtto = objUtility.GiraDataFromDB(dsDatiProvv.Tables(0).Rows(iCount)("DATA_ELABORAZIONE"))
    '                sDataAtto1 = objUtility.GiraDataFromDB(dsDatiProvv.Tables(0).Rows(iCount)("DATA_ELABORAZIONE")).Replace("/", "")
    '                sDataAtto1 = sDataAtto1.Substring(0, 4) + sDataAtto1.Substring(6, 2)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_data_atto"
    '                objBookmark.Valore = sDataAtto
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_data_atto_1"
    '                objBookmark.Valore = sDataAtto1
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_data_atto_2"
    '                objBookmark.Valore = sDataAtto
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_data_atto_3"
    '                objBookmark.Valore = sDataAtto1
    '                oArrBookmark.Add(objBookmark)
    '                '*****************************************
    '                'importo provvedimento
    '                '*****************************************

    '                numRata = iCountRate + 1

    '                Dim workTable As New DataTable("RATE_PROVVEDIMENTI")
    '                Dim rowsArray() As DataRow

    '                workTable = dsDatiRate.Tables(0)
    '                rowsArray = workTable.Select("NUMERO_RATA='" & numRata & "'")


    '                If Not IsDBNull(rowsArray(0)("IMPORTO_RATA")) Then
    '                    'sValoreRataLettere = rowsArray(0)("IMPORTO_RATA").ToString()
    '                    sValoreRata = FormatNumber(rowsArray(0)("IMPORTO_RATA"), 2)
    '                    sValoreRataLettere = sValoreRata.ToString
    '                    sValoreRata = sValoreRata.Replace(",", "").Replace(".", "")
    '                    'sValoreRata = Left(sValoreRata & "00", sValoreRata.Length + 1)
    '                Else
    '                    sValoreRata = "000"
    '                End If
    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_tot_dovuto"
    '                objBookmark.Valore = FormatNumberToZero(sValoreRata)
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_tot_dovuto_1"
    '                objBookmark.Valore = FormatNumberToZero(sValoreRata)
    '                oArrBookmark.Add(objBookmark)

    '                '*****************************************
    '                'stampa dell'importo in lettere
    '                '*****************************************
    '                sVal = EuroForGridView(sValoreRataLettere)
    '                sValIntero = sVal.Substring(0, sVal.Length - 3)
    '                sValDecimal = sVal.Substring(sVal.Length - 2, 2)
    '                sValPrint = NumberToText(CInt(sValIntero)).ToUpper() + "/" + sValDecimal

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_imp_lett"
    '                objBookmark.Valore = sValPrint
    '                oArrBookmark.Add(objBookmark)

    '                objBookmark = New oggettiStampa
    '                objBookmark.Descrizione = "IV_imp_lett_1"
    '                objBookmark.Valore = sValPrint
    '                oArrBookmark.Add(objBookmark)


    '                numRata += 1
    '                If numRata <= iNumRate - 1 Or bRatePari Then
    '                    'da fare solo se il numero di rate è pari e se si è all'ultimo documento

    '                    rowsArray = workTable.Select("NUMERO_RATA='" & numRata & "'")
    '                    If Not IsDBNull(rowsArray(0)("IMPORTO_RATA")) Then
    '                        sValoreRata = FormatNumber(rowsArray(0)("IMPORTO_RATA"), 2)
    '                        sValoreRataLettere = sValoreRata.ToString
    '                        sValoreRata = sValoreRata.Replace(",", "").Replace(".", "")
    '                        'sValoreRata = Left(sValoreRata & "00", sValoreRata.Length + 1)
    '                    Else
    '                        sValoreRata = "000"
    '                    End If

    '                    objBookmark = New oggettiStampa
    '                    objBookmark.Descrizione = "IV_tot_dovuto_2"
    '                    objBookmark.Valore = FormatNumberToZero(sValoreRata)
    '                    oArrBookmark.Add(objBookmark)

    '                    objBookmark = New oggettiStampa
    '                    objBookmark.Descrizione = "IV_tot_dovuto_3"
    '                    objBookmark.Valore = FormatNumberToZero(sValoreRata)
    '                    oArrBookmark.Add(objBookmark)

    '                    '*****************************************
    '                    'stampa dell'importo in lettere
    '                    '*****************************************
    '                    sVal = EuroForGridView(sValoreRataLettere)
    '                    sValIntero = sVal.Substring(0, sVal.Length - 3)
    '                    sValDecimal = sVal.Substring(sVal.Length - 2, 2)
    '                    sValPrint = NumberToText(CInt(sValIntero)).ToUpper() + "/" + sValDecimal


    '                    objBookmark = New oggettiStampa
    '                    objBookmark.Descrizione = "IV_imp_lett_2"
    '                    objBookmark.Valore = sValPrint
    '                    oArrBookmark.Add(objBookmark)

    '                    objBookmark = New oggettiStampa
    '                    objBookmark.Descrizione = "IV_imp_lett_3"
    '                    objBookmark.Valore = sValPrint
    '                    oArrBookmark.Add(objBookmark)
    '                End If

    '                '*********************************
    '                '****** Valorizzo bookMarks ******
    '                '*********************************

    '                ArrayBookMark = CType(oArrBookmark.ToArray(GetType(oggettiStampa)), oggettiStampa())
    '                objTestataDOC.oSetupDocumento.Orientamento = "O"
    '                objTestataDOT.oSetupDocumento.Orientamento = "O"

    '                objTestataDOC = New oggettoTestata
    '                objTestataDOC.Atto = "Documenti"
    '                objTestataDOC.Dominio = "Provvedimenti"
    '                objTestataDOC.Ente = ConstSession.DescrizioneEnte.ToUpper.Replace("Comune di ", "")
    '                objTestataDOC.Filename = FileNameContrib & "_" & dsDatiProvv.Tables(0).Rows(iCount)("NUMERO_ATTO").ToString.Replace("/", "") & "_" & sFilenameDOC

    '                objToPrint = New oggettoDaStampareCompleto
    '                objToPrint.TestataDOC = objTestataDOC
    '                objToPrint.TestataDOT = objTestataDOT
    '                objToPrint.Stampa = ArrayBookMark

    '                oArrListOggettiDaStampare.Add(objToPrint)
    '            Next
    '        Next

    '        Dim GruppoDOC As New GruppoDocumenti
    '        Dim GruppoDOCUMENTI As GruppoDocumenti()
    '        Dim ArrListGruppoDOC As New ArrayList

    '        Dim ArrOggCompleto As oggettoDaStampareCompleto()
    '        Dim objTestataGruppo As New oggettoTestata

    '        objTestataGruppo.Atto = objTestataDOC.Atto
    '        objTestataGruppo.Dominio = objTestataDOC.Dominio
    '        objTestataGruppo.Ente = objTestataDOC.Ente
    '        If Tributo = Utility.Costanti.TRIBUTO_TARSU Then
    '            objTestataGruppo.Filename = "BollettinoViolazione_TARSU_" & FileNameContrib
    '        ElseIf Tributo = Utility.Costanti.TRIBUTO_OSAP Then
    '            objTestataGruppo.Filename = "BollettinoViolazione_OSAP_" & FileNameContrib
    '        Else
    '            objTestataGruppo.Filename = "BollettinoViolazione_ICI_" & FileNameContrib
    '        End If
    '        ArrOggCompleto = CType(oArrListOggettiDaStampare.ToArray(GetType(oggettoDaStampareCompleto)), oggettoDaStampareCompleto())

    '        GruppoDOC.OggettiDaStampare = ArrOggCompleto
    '        GruppoDOC.TestataGruppo = objTestataGruppo

    '        ArrListGruppoDOC.Add(GruppoDOC)

    '        GruppoDOCUMENTI = CType(ArrListGruppoDOC.ToArray(GetType(GruppoDocumenti)), GruppoDocumenti())

    '        Dim oInterfaceStampaDocOggetti As IElaborazioneStampaDocOggetti
    '        oInterfaceStampaDocOggetti = Activator.GetObject(GetType(IElaborazioneStampaDocOggetti), ConfigurationManager.AppSettings("ServizioStampe").ToString())

    '        Dim retArray As GruppoURL
    '        Dim retArray1 As GruppoURL()

    '        Dim objTestataComplessiva As New oggettoTestata
    '        objTestataComplessiva.Atto = objTestataDOC.Atto
    '        objTestataComplessiva.Dominio = objTestataDOC.Dominio
    '        objTestataComplessiva.Ente = objTestataDOC.Ente
    '        If Tributo = Utility.Costanti.TRIBUTO_TARSU Then
    '            objTestataComplessiva.Filename = "BollettinoViolazione_TARSU_" & FileNameContrib
    '        ElseIf Tributo = Utility.Costanti.TRIBUTO_OSAP Then
    '            objTestataComplessiva.Filename = "BollettinoViolazione_OSAP_" & FileNameContrib
    '        Else
    '            objTestataComplessiva.Filename = "BollettinoViolazione_ICI_" & FileNameContrib
    '        End If

    '        '************************************************************
    '        ' definisco anche il numero di documenti che voglio stampare.
    '        '************************************************************
    '        '*** 201511 - template documenti per ruolo ***
    '        'retArray = oInterfaceStampaDocOggetti.StampaDocumenti(objTestataComplessiva, GruppoDOCUMENTI, True, bCreaPDF)
    '        retArray = oInterfaceStampaDocOggetti.StampaDocumenti(ConstSession.PathStampe, ConstSession.PathVirtualStampe, objTestataComplessiva, GruppoDOCUMENTI, True, bCreaPDF)
    '        'retArray = oInterfaceStampaDocOggetti.StampaDocumenti(objTestataComplessiva, GruppoDOCUMENTI, True, bCreaPDF)
    '        ReDim retArray1(0)
    '        retArray1(0) = retArray

    '        Session.Remove("ELENCO_BOLLETTINI_STAMPATI")

    '        If Not retArray Is Nothing Then

    '            Session.Add("ELENCO_BOLLETTINI_STAMPATI", retArray1)

    '            Dim sScript As String = ""
    '            sScript += "parent.corpo.location.href='ViewBollettiniViolazioneStampati.aspx';"
    '            sScript += "parent.parent.document.getElementById('DivAttesa').style.display='none';"
    '            RegisterScript(sScript, Me.GetType())
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleL.STAMPA_BOLLETTINI_PRE_e_ACCERTAMENTO_RATEIZZATI.errore: ", Err)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Private Function CreateDataSetStampa() As DataSet

        Dim dsTmp As New DataSet

        dsTmp.Tables.Add("DS_TAB_STORICO_DOCUMENTI")

        dsTmp.Tables("DS_TAB_STORICO_DOCUMENTI").Columns.Add("COD_CONTRIBUENTE").DataType = GetType(System.String)
        dsTmp.Tables("DS_TAB_STORICO_DOCUMENTI").Columns.Add("COD_ENTE").DataType = GetType(System.String)
        dsTmp.Tables("DS_TAB_STORICO_DOCUMENTI").Columns.Add("ANNO").DataType = GetType(System.String)
        dsTmp.Tables("DS_TAB_STORICO_DOCUMENTI").Columns.Add("TIPO_DOCUMENTO").DataType = GetType(System.String)
        dsTmp.Tables("DS_TAB_STORICO_DOCUMENTI").Columns.Add("COD_TRIBUTO").DataType = GetType(System.String)
        dsTmp.Tables("DS_TAB_STORICO_DOCUMENTI").Columns.Add("DATA_ELABORAZIONE").DataType = GetType(System.String)
        dsTmp.Tables("DS_TAB_STORICO_DOCUMENTI").Columns.Add("NOTE").DataType = GetType(System.String)
        dsTmp.Tables("DS_TAB_STORICO_DOCUMENTI").Columns.Add("URL_DOCUMENTO").DataType = GetType(System.String)

        CreateDataSetStampa = dsTmp

    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ds"></param>
    ''' <param name="blnConfigDichiarazione"></param>
    ''' <returns></returns>
    Private Function FillBookMarkVersamenti(ByVal ds As DataSet, ByVal blnConfigDichiarazione As Boolean) As String

        Dim strRiga As String = ""
        Dim strVersTemp As String = String.Empty
        Dim iVersamenti As Integer
        Dim objUtility As New MyUtility

        Try
            If ds.Tables(0).Rows.Count > 0 Then

                strRiga = ""
                strRiga = strRiga.PadLeft(148, "-")
                strVersTemp = strRiga & Microsoft.VisualBasic.Constants.vbCrLf


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
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleL.FillBookMarkVersamenti.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
        Return strVersTemp
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ds"></param>
    ''' <param name="ImportoGiorni"></param>
    ''' <param name="ImportoSemestriACC"></param>
    ''' <param name="ImportoSemestriSAL"></param>
    ''' <param name="NumSemestriACC"></param>
    ''' <param name="NumSemestriSAL"></param>
    ''' <returns></returns>
    Private Function FillBookMarkIMPORTIINTERESSI(ByVal ds As DataSet, ByRef ImportoGiorni As String, ByRef ImportoSemestriACC As String, ByRef ImportoSemestriSAL As String, ByRef NumSemestriACC As String, ByRef NumSemestriSAL As String) As Boolean
        Dim iInteressi As Integer
        Try
            If ds.Tables(0).Rows.Count > 0 Then
                'deve restituire sempre al max un record perchè la query
                'prevede un raggruppamento con somme
                For iInteressi = 0 To ds.Tables(0).Rows.Count - 1

                    ImportoGiorni = FormatImport(ds.Tables(0).Rows(iInteressi)("IMPORTO_TOTALE_GIORNI"))
                    ImportoSemestriACC = FormatImport(ds.Tables(0).Rows(iInteressi)("IMPORTO_ACC_SEMESTRI"))
                    ImportoSemestriSAL = FormatImport(ds.Tables(0).Rows(iInteressi)("IMPORTO_SALDO_SEMESTRI"))
                    NumSemestriACC = FormatNumberToZero(ds.Tables(0).Rows(iInteressi)("N_SEMESTRI_ACC"))
                    NumSemestriSAL = FormatNumberToZero(ds.Tables(0).Rows(iInteressi)("N_SEMESTRI_SALDO"))

                Next
            Else

                ImportoGiorni = "0"
                ImportoSemestriACC = "0"
                ImportoSemestriSAL = "0"
                NumSemestriACC = "0"
                NumSemestriSAL = "0"

            End If

            Return True
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleL.FillBookMarkIMPORTOINTERESSI.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ds"></param>
    ''' <returns></returns>
    Private Function FillBookMarkELENCOINTERESSI(ByVal ds As DataSet) As String

        'Dim strRiga As String=""
        Dim strIntTemp As String = String.Empty
        Dim iInteressi As Integer
        Dim objUtility As New MyUtility
        Try
            If ds.Tables(0).Rows.Count > 0 Then
                For iInteressi = 0 To ds.Tables(0).Rows.Count - 1

                    strIntTemp = strIntTemp & FormatStringToEmpty(ds.Tables(0).Rows(iInteressi)("DESCRIZIONE")) & Microsoft.VisualBasic.Constants.vbTab
                    strIntTemp = strIntTemp & "Dal: " & objUtility.GiraDataFromDB((ds.Tables(0).Rows(iInteressi)("DATA_INIZIO"))) & Microsoft.VisualBasic.Constants.vbTab
                    If Not IsDBNull(ds.Tables(0).Rows(iInteressi)("DATA_FINE")) Then
                        strIntTemp = strIntTemp & "Al: " & objUtility.GiraDataFromDB((ds.Tables(0).Rows(iInteressi)("DATA_FINE"))) & Microsoft.VisualBasic.Constants.vbTab
                    Else
                        strIntTemp = strIntTemp & "" & Microsoft.VisualBasic.Constants.vbTab
                    End If
                    strIntTemp = strIntTemp & ds.Tables(0).Rows(iInteressi)("TASSO") & "%" & Microsoft.VisualBasic.Constants.vbCrLf
                Next
            Else
                strIntTemp = "Nessuna Tipologia di Interessi Configurata." & Microsoft.VisualBasic.Constants.vbCrLf
            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleL.FillBookMarkELENCOINTERESSI.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
        Return strIntTemp
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ds"></param>
    ''' <returns></returns>
    Private Function FillBookMarkELENCOSANZIONI(ByVal ds As DataSet) As String

        'Dim strRiga As String=""
        Dim strSanzTemp As String = String.Empty
        Dim iSanzioni As Integer
        Try
            If ds.Tables(0).Rows.Count > 0 Then

                For iSanzioni = 0 To ds.Tables(0).Rows.Count - 1

                    strSanzTemp = strSanzTemp & FormatStringToEmpty(ds.Tables(0).Rows(iSanzioni)("DESCRIZIONE_VOCE_ATTRIBUITA")) & Microsoft.VisualBasic.Constants.vbTab
                    strSanzTemp = strSanzTemp & "€ " & FormatImport(ds.Tables(0).Rows(iSanzioni)("TOT_IMPORTO_SANZ")) & Microsoft.VisualBasic.Constants.vbCrLf

                Next

            Else
                strSanzTemp = "Nessuna Tipologia di Sanzione Applicata." & Microsoft.VisualBasic.Constants.vbCrLf
            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleL.FillBookMarkELENCOSANZIONI.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
        Return strSanzTemp
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ds"></param>
    ''' <param name="blnConfigDichiarazione"></param>
    ''' <returns></returns>
    Private Function FillBookMarkCATASTO(ByVal ds As DataSet, ByVal blnConfigDichiarazione As Boolean) As String

        Dim strRiga As String = ""
        Dim strImmoTemp As String = String.Empty
        Dim iImmobili As Integer

        Try
            If ds.Tables(0).Rows.Count > 0 Then

                strRiga = ""
                strRiga = strRiga.PadLeft(148, "-")
                strImmoTemp = strRiga


                For iImmobili = 0 To ds.Tables(0).Rows.Count - 1
                    strImmoTemp = strImmoTemp & "Tipologia Immobile: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("DescrTipoImmobile")) & Microsoft.VisualBasic.Constants.vbCrLf
                    strImmoTemp = strImmoTemp & "Ubicazione: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("Via")) & " " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("NumeroCivico")) & Microsoft.VisualBasic.Constants.vbCrLf
                    strImmoTemp = strImmoTemp & "Foglio: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("FOGLIO")) & Microsoft.VisualBasic.Constants.vbTab
                    strImmoTemp = strImmoTemp & "Numero: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("NUMERO")) & Microsoft.VisualBasic.Constants.vbTab
                    strImmoTemp = strImmoTemp & "Subalterno: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("SUBALTERNO")) & Microsoft.VisualBasic.Constants.vbTab
                    strImmoTemp = strImmoTemp & "Categoria: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("CODCATEGORIACATASTALE")) & Microsoft.VisualBasic.Constants.vbTab
                    strImmoTemp = strImmoTemp & "Classe: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("CODCLASSE")) & Microsoft.VisualBasic.Constants.vbCrLf
                    strImmoTemp = strImmoTemp & "Rendita/Valore: " & FormatImport(ds.Tables(0).Rows(iImmobili)("ValoreImmobile")) & Microsoft.VisualBasic.Constants.vbTab
                    strImmoTemp = strImmoTemp & "Importo Dovuto: " & FormatImport(ds.Tables(0).Rows(iImmobili)("IMPORTO_TOTALE_ICI_DOVUTO")) ' & Microsoft.VisualBasic.Constants.vbTab
                    strImmoTemp = strImmoTemp & Microsoft.VisualBasic.Constants.vbCrLf
                    strImmoTemp = strImmoTemp & strRiga

                Next

            Else
                strImmoTemp = strRiga
                strImmoTemp = "Nessun immobile abbinato a catasto." & Microsoft.VisualBasic.Constants.vbCrLf
                strImmoTemp = strImmoTemp & strRiga
            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleL.FillBookMarkCATASTO.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
        Return strImmoTemp
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="ds"></param>
    ''' <param name="blnConfigDichiarazione"></param>
    ''' <returns></returns>
    Private Function FillBookMarkDICHIARATO(ByVal ds As DataSet, ByVal blnConfigDichiarazione As Boolean) As String

        Dim strRiga As String = ""
        Dim strImmoTemp As String = String.Empty
        Dim iImmobili As Integer
        Dim objUtility As New MyUtility

        Try
            If ds.Tables(0).Rows.Count > 0 Then

                strRiga = ""
                strRiga = strRiga.PadLeft(148, "-")
                strImmoTemp = strRiga & Microsoft.VisualBasic.Constants.vbCrLf

                For iImmobili = 0 To ds.Tables(0).Rows.Count - 1

                    If blnConfigDichiarazione = False Then
                        strImmoTemp = strImmoTemp & "Dal: " & objUtility.GiraDataFromDB(ds.Tables(0).Rows(iImmobili)("DATAINIZIO")) & Microsoft.VisualBasic.Constants.vbTab
                        strImmoTemp = strImmoTemp & "Al: " & objUtility.GiraDataFromDB(ds.Tables(0).Rows(iImmobili)("DATAFINE")) & Microsoft.VisualBasic.Constants.vbCrLf
                    Else
                        strImmoTemp = strImmoTemp & "Anno Dich.: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("ANNODICHIARAZIONE")) & Microsoft.VisualBasic.Constants.vbCrLf
                    End If

                    strImmoTemp = strImmoTemp & "Tipologia Immobile: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("DescrTipoImmobile")) & Microsoft.VisualBasic.Constants.vbCrLf
                    strImmoTemp = strImmoTemp & "Ubicazione: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("Via")) & " " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("NumeroCivico")) & Microsoft.VisualBasic.Constants.vbCrLf
                    strImmoTemp = strImmoTemp & "Foglio: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("FOGLIO")) & Microsoft.VisualBasic.Constants.vbTab
                    strImmoTemp = strImmoTemp & "Numero: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("NUMERO")) & Microsoft.VisualBasic.Constants.vbTab
                    strImmoTemp = strImmoTemp & "Subalterno: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("SUBALTERNO")) & Microsoft.VisualBasic.Constants.vbTab
                    strImmoTemp = strImmoTemp & "Categoria: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("CODCATEGORIACATASTALE")) & Microsoft.VisualBasic.Constants.vbTab
                    strImmoTemp = strImmoTemp & "Classe: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("CODCLASSE")) & Microsoft.VisualBasic.Constants.vbCrLf
                    strImmoTemp = strImmoTemp & "Rendita/Valore: " & FormatImport(ds.Tables(0).Rows(iImmobili)("ValoreImmobile")) & Microsoft.VisualBasic.Constants.vbTab
                    strImmoTemp = strImmoTemp & "Perc. Possesso: " & FormatStringToEmpty(ds.Tables(0).Rows(iImmobili)("PERCPOSSESSO")).Replace(",", ".") & Microsoft.VisualBasic.Constants.vbTab
                    strImmoTemp = strImmoTemp & "Importo Dovuto: " & FormatImport(ds.Tables(0).Rows(iImmobili)("ICI_TOTALE_DOVUTA"))  ' & Microsoft.VisualBasic.Constants.vbTab

                    If blnConfigDichiarazione = True Then
                        strImmoTemp = strImmoTemp & Microsoft.VisualBasic.Constants.vbTab & "Possesso 31/12: " & BoolToStringForGridView(ds.Tables(0).Rows(iImmobili)("Possesso").ToString())
                    End If

                    strImmoTemp = strImmoTemp & Microsoft.VisualBasic.Constants.vbCrLf
                    strImmoTemp = strImmoTemp & strRiga

                Next

            Else
                strImmoTemp = strRiga
                strImmoTemp = "Nessun Immobile dichiarato." & Microsoft.VisualBasic.Constants.vbCrLf
                strImmoTemp = strImmoTemp & strRiga
            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleL.FillBookMarkDICHIARATO.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
        Return strImmoTemp
    End Function
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

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleL.BoolToStringForGridView.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
        Return ret
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="objInput"></param>
    ''' <returns></returns>
    Private Function FormatString(ByVal objInput As Object) As String

        Dim strOutput As String = String.Empty
        Try
            If (objInput Is Nothing) Then
                strOutput = ""
            Else
                strOutput = objInput.ToString()
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleL.FormatString.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
        Return strOutput
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
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleL.FormatStringToEmpty.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
        Return strOutput
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

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleL.FormatNumberToZero.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
        Return strOutput
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
                If CStr(objInput) = "" Or CStr(objInput) = "0" Or CStr(objInput) = "-1" Then
                    strOutput = 0
                Else
                    '#,##0.00
                    Dim dblImporto As Double
                    dblImporto = CDbl(objInput)
                    strOutput = Format(dblImporto, "#,##0.00")
                End If
            Else
                strOutput = 0
            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleL.FormatImport.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
        Return strOutput
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="iInput"></param>
    ''' <returns></returns>
    Private Function EuroForGridView(ByVal iInput As Double) As String
        Dim ret As String = String.Empty
        Try
            If ((iInput.ToString() = "-1") Or (iInput.ToString() = "-1,00")) Then
                ret = String.Empty
            Else
                ret = Convert.ToDecimal(iInput).ToString("N")
            End If

        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleL.EuroForGridView.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
        Return ret
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="n"></param>
    ''' <returns></returns>
    Private Function NumberToText(ByVal n As Integer) As String
        NumberToText = ""
        Try
            If (n < 0) Then
                Return "Meno " + NumberToText(-n)
            ElseIf (n = 0) Then
                Return "" ' settando quì la stringa zero l'aggiungerebbe per tutti i numeri multipli di dieci
            ElseIf (n <= 19) Then
                Return New String() {"Uno", "Due", "Tre", "Quattro", "Cinque", "Sei", "Sette", "Otto", "Nove", "Dieci", "Undici", "Dodici", "Tredici", "Quattordici", "Quindici", "Sedici", "Diciasette", "Diciotto", "Diciannove"}(n - 1)
            ElseIf (n <= 99) Then
                Dim strUnita As String = n.ToString().Substring(1, 1)
                If (strUnita = "1" Or strUnita = "8") Then
                    Return New String() {"Vent", "Trent", "Quarant", "Cinquant", "Sessant", "Settant", "Ottant", "Novant"}(Int(n / 10) - 2) + NumberToText(n Mod 10)
                Else
                    Return New String() {"Venti", "Trenta", "Quaranta", "Cinquanta", "Sessanta", "Settanta", "Ottanta", "Novanta"}(Int(n / 10) - 2) + NumberToText(n Mod 10)
                End If
            ElseIf (n <= 199) Then
                Return "Cento" + NumberToText(n Mod 100)
            ElseIf (n <= 999) Then
                Return NumberToText(Int(n / 100)) + "Cento" + NumberToText(n Mod 100)
            ElseIf (n <= 1999) Then
                Return "Mille" + NumberToText(n Mod 1000)
            ElseIf (n <= 999999) Then
                Return NumberToText(Int(n / 1000)) + "Mila" + NumberToText(n Mod 1000)
            ElseIf (n <= 1999999) Then
                Return "Un Milione" + NumberToText(n Mod 1000000)
            ElseIf (n <= 999999999) Then
                Return NumberToText(Int(n / 1000000)) + "Milioni" + NumberToText(n Mod 1000000)
            ElseIf (n <= 1999999999) Then
                Return "Unmiliardo" + NumberToText(n Mod 1000000000)
            Else
                Return NumberToText(Int(n / 1000000000)) + "Miliardi" + NumberToText(n Mod 1000000000)
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ristampaOriginaleL.NumberToText.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Function
End Class
