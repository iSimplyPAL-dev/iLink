Imports log4net
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
''' <summary>
''' Pagina per la selezione dei parametri di ricerca dei documenti da stampare e per la gestione dell'iter di stampa.
''' Contiene i parametri di ricerca, le funzioni della comandiera e iframe per la visualizzazione del risultato.''' 
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
''' <revisionHistory>
''' <revision date="05/09/2018">
''' <strong>Bollettazione Vigliano</strong>
''' A livello di Stampa Minuta Rate deve essere aggiunto il campo TIPOUTENZA (Domestica o Produttiva).
''' La stampa minuta rate popolerà la tabella dei documenti elaborati In modo da poter approvare e rendere definitivo il ruolo.
''' </revision>
''' </revisionHistory>
''' <revisionHistory>
''' <revision date="26/11/2018">
''' <strong>Insoluti e Coattivo</strong>
''' <list type = "bullet" >
''' <item>
''' <em>Stampa documenti</em>
''' Dovranno essere gestiti nuovi segnalibri sul modello per l'evidenza di spese di notifica e sanzioni e dovrà essere gestito un F24 di unica soluzione nel caso di pagamento oltre i termini.
''' </item>
'''   <item>
'''   <em>Approva documenti</em>
''' L'approvazione del ruolo dovrà settare la data variazione sull’avviso originale, come se si stesse facendo uno sgravio, e ripristinare i primi 3 caratteri del codice cartella.
'''   </item>
''' </list>
''' </revision>
''' </revisionHistory>
Partial Class RicercaDoc
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(RicercaDoc))
    Private nDocDaElab, nDocElab As Integer
    Private FncDoc As New ClsGestDocumenti
    Private FncRuolo As New ClsGestRuolo
    Private oMyRuolo() As ObjRuolo
    Private sScript As String

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
    ''' Al caricamento della pagina viene visualizzato il riepilogo dei documenti da produrre per il ruolo in elaborazione.
    ''' Viene inoltre controllato se c'è un'elaborazione in corso.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim ElaborazioneInCorso As Integer
        Dim fncRuolo As New ClsGestRuolo

        Try
            'prelevo il ruolo
            oMyRuolo = Session("oRuoloTIA")
            If Page.IsPostBack = False Then
                txtIdRuolo.Text = oMyRuolo(0).IdFlusso
                lblTipoRuolo.Text = oMyRuolo(0).sDescrTipoRuolo
                lblAnnoRuolo.Text = oMyRuolo(0).sAnno
                lblDataCartellazione.Text = oMyRuolo(0).tDataCartellazione

                fncRuolo.LoadTipoCalcolo(ConstSession.StringConnection, ConstSession.IdEnte, oMyRuolo(0).sAnno, LblTipoCalcolo, LblTipoMQ, ChkConferimenti, ChkMaggiorazione)
                'se non selezionati nascondo
                If ChkMaggiorazione.Checked = False Then
                    ChkMaggiorazione.Visible = False
                Else
                    ChkMaggiorazione.Visible = True
                End If
                If ChkConferimenti.Checked = False Then
                    ChkConferimenti.Visible = False
                Else
                    ChkConferimenti.Visible = True
                End If

                ElaborazioneInCorso = FncDoc.InElaborazione(ConstSession.DBType, ConstSession.StringConnectionOPENgov, Now.Year.ToString(), ConstSession.IdEnte, "0465", "", oMyRuolo(0).IdFlusso)
                If ElaborazioneInCorso = 1 Then
                    ' visualizzo le pagine di elaborazione in corso
                    sScript = "parent.Comandi.location.href='ComandiVuoto.aspx';"
                    sScript += "document.location.href='StampaInCorso.aspx';"
                    RegisterScript(sScript, Me.GetType)
                    Exit Sub
                ElseIf ElaborazioneInCorso = 0 And oMyRuolo(0).tDataElabDoc <> Date.MinValue Then
                    lblElaborazioniEffettuate.Text = "Elaborazione Terminata con successo."
                ElseIf ElaborazioneInCorso = 2 Then
                    lblElaborazioniEffettuate.Text = "Elaborazione Terminata con errori."
                End If

                FncDoc.GetNDoc(ConstSession.IdEnte, oMyRuolo(0).IdFlusso, nDocElab, nDocDaElab, Nothing)
                lblDocElaborati.Text = "DOCUMENTI GIA' ELABORATI:   " + nDocElab.ToString
                lblDocDaElaborare.Text = "DOCUMENTI DA ELABORARE:  " + nDocDaElab.ToString
                Session("nDocDaElaborare") = nDocDaElab
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.RicercaDoc.Page_Load.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Pulsante per la creazione dei documenti.
    ''' Prima della stampa viene eseguita una query di sicurezza per andare a registrare le eventuali riduzioni su articoli non registrate in articolivsriduzioni ma contemplate in calcolo perché ogni tanto non riesce a fare la dovuta query di insert in fase di creazione cartella ma senza dare errori.    ''' 
    ''' Richiama ClsGestDocumenti.ElaboraDocumenti.
    ''' Se ho chiamato l'elaborazione asincrona viene visualizzata l'elaborazione in corso, se ha dato errore messaggio di blocco; altrimenti reindirizza al download dei documenti prodotti.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdElaborazione_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdElaborazione.Click
        Dim sScript, sTypeOrd, sNameModello As String
        Dim nTipoElab As Integer = -1
        Dim FncDoc As New ClsGestDocumenti
        Dim nReturn, nMaxDocPerFile As Integer
        Dim oMyRuolo() As ObjRuolo
        Dim oListDocStampati() As RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL = Nothing
        Dim bCreaPDF As Boolean
        Dim cmdMyCommand As New SqlClient.SqlCommand

        sNameModello = ""
        Try
            'prelevo il ruolo
            oMyRuolo = Session("oRuoloTIA")

            '*** query di sicurezza per andare a registrare le eventuali riduzioni su articoli non registrate in articolivsriduzioni ma contemplate in calcolo ***
            '*** (ogni tanto non riesce a fare la dovuta query di insert in fase di creazione cartella ma senza dare errori) ***
            Dim sSQL As String
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            Try
                cmdMyCommand.CommandType = CommandType.Text
                If ConstSession.IsFromVariabile = "1" Then
                    sSQL = "INSERT INTO TBLARTICOLIRIDUZIONI(IDARTICOLO, IDCODICE)"
                    sSQL += " SELECT DISTINCT A.ID, D.IDCODICE"
                    sSQL += " FROM TBLARTICOLI A"
                    sSQL += " INNER JOIN TBLDETTAGLIOTESTATARIDUZIONI D ON A.IDDETTAGLIOTESTATA=D.IDDETTAGLIOTESTATA"
                    sSQL += " LEFT JOIN TBLARTICOLIRIDUZIONI AR ON A.ID=AR.IDARTICOLO"
                    sSQL += " WHERE 1=1"
                    sSQL += " AND A.DATA_VARIAZIONE IS NULL"
                    sSQL += " AND IMPORTO_RIDUZIONI>0"
                    sSQL += " AND AR.IDARTICOLO IS NULL"
                Else
                    sSQL = "INSERT INTO TBLRUOLORIDUZIONI(IDENTE,IDRUOLO,IDRIDUZIONE)"
                    sSQL += " SELECT DISTINCT A.IDENTE, A.ID, D.IDRIDUZIONE"
                    sSQL += " FROM TBLRUOLOTARSU A"
                    sSQL += " INNER JOIN TBLDETTAGLIOTESTATARIDUZIONI D ON A.IDDETTAGLIOTESTATA=D.IDDETTAGLIOTESTATA"
                    sSQL += " LEFT JOIN TBLRUOLORIDUZIONI AR ON A.ID=AR.IDRUOLO"
                    sSQL += " WHERE 1=1"
                    sSQL += " AND A.DATA_VARIAZIONE IS NULL"
                    sSQL += " AND IMPORTO_RIDUZIONI>0"
                    sSQL += " AND AR.IDRUOLO IS NULL"
                End If
                cmdMyCommand.CommandText = sSQL
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
                cmdMyCommand.ExecuteNonQuery()
            Catch ex As Exception
                Log.Debug("RicercaDoc::CmdElaborazione_Click::si è verificato il seguente errore::query di sicurezza::", ex)
                Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            End Try
            '*** ***

            sScript = "Search();"
            RegisterScript(sScript, Me.GetType)

            '********************************************************
            'testo se sto facendo delle prove oppure un elaborazione effettiva
            '********************************************************
            If Session("ElaboraDocumenti") = 1 Then
                nTipoElab = 1
            ElseIf Session("ElaboraDocumenti") = 2 Then
                nTipoElab = 0
                '****************************************************
                'stiamo effettuando l'elaborazione effettiva
                'controllo se ci sono ancora dei doc da elaborare
                '****************************************************
                If Session("nDocDaElaborare") = 0 Then
                    lblElaborazioniEffettuate.Text = "I documenti sono già stati tutti elaborati in effettivo!"
                    Exit Sub
                End If
                If FncRuolo.SetDateRuolo(oMyRuolo(0).IdFlusso, 4, "I") = 0 Then
                    Log.Debug("Si è verificato un errore in RicercaDoc::CmdElaborazione_Click::SetDateRuolo")
                    Response.Redirect("../../../PaginaErrore.aspx")
                    Exit Sub
                End If
            End If

            '********************************************************
            'elaborazione prove dei documenti
            '********************************************************
            'verifico gli elementi selezionati all'interno della griglia
            '********************************************************
            'se l'operatore non ha selezionato alcun elemento ----->do un messaggio
            '********************************************************
            If nTipoElab = 1 And IsNothing(Session("ListCartelle")) Then
                lblElaborazioniEffettuate.Text = "Non sono stati selezionati documenti da elaborare!"
                Exit Sub
            End If

            If nTipoElab <> -1 Then
                If Not Session("ListCartelle") Is Nothing Then
                    Session.Remove("ELENCO_DOCUMENTI_STAMPATI")
                    If Session("OrdinamentoDoc") = 0 Then
                        sTypeOrd = "Indirizzo"
                    Else
                        sTypeOrd = "Nominativo"
                    End If

                    Select Case oMyRuolo(0).sTipoRuolo
                        Case "O"
                            sNameModello = "TARSU_ORDINARIO"
                        Case "S"
                            sNameModello = "TARSU_ORDINARIO"
                    End Select

                    If Not Session("nMaxDocPerFile") Is Nothing Then
                        nMaxDocPerFile = Integer.Parse(Session("nMaxDocPerFile"))
                    Else
                        nMaxDocPerFile = ConstSession.nMaxDocPerFile
                    End If
                    '*** 201511 - template documenti per ruolo ***
                    'Try 'scarico template dalla tabella dei ruoli elaborati
                    '    Dim myTemplateDoc As New ElaborazioneDatiStampeInterface.ObjTemplateDoc
                    '    myTemplateDoc.IdEnte = ConstSession.IdEnte
                    '    myTemplateDoc.IdTributo = ConstSession.CodTributo
                    '    myTemplateDoc.IdRuolo = oMyRuolo(0).IdFlusso
                    '    myTemplateDoc.Load()
                    '    Dim PathFileTemplate As String = ConstSession.PathStampe
                    '    PathFileTemplate += ElaborazioneDatiStampeInterface.ObjTemplateDoc.ATTOTemplate + "\\"
                    '    PathFileTemplate += ElaborazioneDatiStampeInterface.ObjTemplateDoc.Dominio_TARSU + "\\"
                    '    PathFileTemplate += ConstSession.DescrizioneEnte + "\\"
                    '    PathFileTemplate += myTemplateDoc.FileName
                    '    System.IO.File.WriteAllBytes(PathFileTemplate, myTemplateDoc.PostedFile)
                    'Catch Err As Exception
                    '    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.RicercaDoc.CmdElaborazione_Click.errore: ", Err)
                    '    Response.Redirect("../../../PaginaErrore.aspx")
                    'End Try
                    '*** ***

                    Try
                        '*** 20131104 - TARES ***
                        'WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
                        'If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
                        '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
                        'End If
                        Dim nDecimal As Integer = 2
                        Dim TipoStampaBollettini As String = "BOLLETTINISTANDARD"
                        If Session("ElaboraBollettini") = False Then
                            TipoStampaBollettini = ""
                        End If
                        '*** 20140509 - TASI ***
                        Dim bSendByMail As Boolean = False
                        If Not Session("bSendByMail") Is Nothing Then
                            bSendByMail = Session("bSendByMail")
                        End If
                        'per ora si fa l'invio tramite mail solo se il bollettino è F24
                        If Session("tipobollettino") <> "F24" Then
                            bSendByMail = False
                        End If
                        'nReturn = FncDoc.ElaboraDocumenti(WFSessione, ConstSession.IdEnte, oMyRuolo(0).IdFlusso, oMyRuolo(0).sTipoRuolo, nDocDaElab, nDocElab, nTipoElab, sTypeOrd, sNameModello, nMaxDocPerFile, Session("ElaboraBollettini"), Session("ListCartelle"), oListDocStampati, bCreaPDF)
                        'nReturn = FncDoc.ElaboraDocumenti(ConstSession.IdEnte, oMyRuolo(0).IdFlusso, oMyRuolo(0).sTipoRuolo, oMyRuolo(0).sAnno, nDocDaElab, nDocElab, nTipoElab, sTypeOrd, sNameModello, nMaxDocPerFile, Session("ElaboraBollettini"), Session("ListCartelle"), oListDocStampati, bCreaPDF, nDecimal, LblTipoCalcolo.Text, TipoStampaBollettini, Session("tipobollettino"), MyDBEngine)
                        nReturn = FncDoc.ElaboraDocumenti(ConstSession.IdEnte, oMyRuolo(0).IdFlusso, oMyRuolo(0).sTipoRuolo, oMyRuolo(0).sAnno, nDocDaElab, nDocElab, nTipoElab, sTypeOrd, sNameModello, nMaxDocPerFile, Session("ElaboraBollettini"), Session("ListCartelle"), oListDocStampati, bCreaPDF, nDecimal, LblTipoCalcolo.Text, TipoStampaBollettini, Session("tipobollettino"), bSendByMail)
                        '*** ***
                        '*** ***
                    Catch Err As Exception
                        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.RicercaDoc.CmdElaborazione_Click.errore: ", Err)
                        Response.Redirect("../../../PaginaErrore.aspx")
                    Finally
                        'WFSessione.Kill()
                    End Try

                    If Not oListDocStampati Is Nothing Then
                        Session.Add("ELENCO_DOCUMENTI_STAMPATI", oListDocStampati)
                    End If

                    If nReturn = 0 Then
                        '********************************************************
                        'si è verificato uin errore
                        '********************************************************
                        Exit Sub
                    ElseIf nReturn = 2 Then
                        '********************************************************
                        ' ho chiamato l'elaborazione asincrona, sostituisco le pagine
                        ' dei comandi e quella principale
                        '********************************************************
                        sScript = "parent.Comandi.location.href='ComandiDocElaborati.aspx';"
                        sScript += "document.location.href='StampaInCorso.aspx';"
                        RegisterScript(sScript, Me.GetType)
                        Exit Sub
                    End If
                Else
                    lblElaborazioniEffettuate.Text = "Non ci sono documenti da elaborare!"
                    Exit Sub
                End If
                Session("ListCartelle") = Nothing

                'sScript = "parent.Visualizza.location.href='ViewDocElaborati.aspx';"
                'sScript += "parent.Comandi.location.href='ComandiDocElaborati.aspx';"
                sScript = "document.getElementById('DivAttesa').style.display = 'none';"
                sScript += "document.getElementById('divStampa').style.display = '';"
                sScript += "document.getElementById('divAvviso').style.display = 'none';"
                sScript += "document.getElementById('loadStampa').src = 'ViewDocElaborati.aspx?Provenienza=E';"
                RegisterScript(sScript, Me.GetType)
            Else
                lblElaborazioniEffettuate.Text = "Effettuare una ricerca!"
            End If

        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.RicercaDoc.CmdElaborazione_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        Finally
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
    End Sub
    ''' <summary>
    ''' Pulsante per l'estrazione in formato XLS delle rate relative ai documenti da stampare; questo flusso deve essere inviato allo stampatore.
    ''' L'estrazione del file corrisponde all'elaborazione in effettivo di tutti i documenti.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="09/2018">
    ''' <strong>Bollettazione Vigliano in OPENgov</strong>
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Esportazione completa dati</em>
    ''' </revision>
    ''' </revisionHistory>
    Private Sub CmdStampaMinutaRate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdStampaMinutaRate.Click
        Dim sNameXLS, sPathProspetti As String
        Dim oMyRuolo() As ObjRuolo = Nothing
        Dim x, nCol As Integer
        Dim FncStampa As New ClsStampaXLS
        Dim DtDatiStampa As New DataTable
        Dim aMyHeaders As String()
        Dim aListColonne As ArrayList
        Dim FncRuolo As New ClsGestRuolo

        sNameXLS = "" : sPathProspetti = ""
        Try
            nCol = 19
            'prelevo il ruolo
            oMyRuolo = Session("oRuoloTIA")
            'valorizzo il nome del file
            sNameXLS = ConstSession.IdEnte & "_MINUTARATE_" & oMyRuolo(0).sAnno & "_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
            Dim DvDati As New DataView
            DvDati = FncRuolo.GetMinutaRate(ConstSession.StringConnection, ConstSession.IdEnte, oMyRuolo(0).IdFlusso)
            If Not DvDati Is Nothing Then
                If DvDati.Count > 0 Then
                    DtDatiStampa = FncStampa.PrintMinutaRate(DvDati, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, oMyRuolo(0).sAnno, nCol)
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.Calcolo.CmdStampaMinutaRate_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        Finally
            CacheManager.RemoveAvanzamentoElaborazione()
        End Try
        If Not DtDatiStampa Is Nothing Then
            If FncRuolo.SetDocumentiElaborati(oMyRuolo(0)) = False Then
                Response.Redirect("../../../PaginaErrore.aspx")
            End If
            If FncRuolo.UpdateDateRuolo(oMyRuolo, 4, "I") = False Then
                Response.Redirect("../../../PaginaErrore.aspx")
            End If
            'definisco le colonne
            aListColonne = New ArrayList
            For x = 0 To nCol
                aListColonne.Add("")
            Next
            aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())
            'definisco l'insieme delle colonne da esportare
            Dim MyCol() As Integer = New Integer(nCol) {}
            For x = 0 To nCol
                MyCol(x) = x
            Next
            'esporto i dati in excel
            Dim MyStampa As New RKLib.ExportData.Export("Web")
            MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sPathProspetti & sNameXLS)
        End If
    End Sub
    'Private Sub CmdStampaMinutaRate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdStampaMinutaRate.Click
    '    Dim sNameXLS, sPathProspetti, Str As String
    '    Dim oMyRuolo() As ObjRuolo = Nothing
    '    Dim x, nCol As Integer
    '    Dim FncStampa As New ClsStampaXLS
    '    Dim DtDatiStampa As New DataTable
    '    Dim aMyHeaders As String()
    '    Dim aListColonne As ArrayList
    '    Dim FncRuolo As New ClsGestRuolo

    '    sNameXLS = "" : sPathProspetti = ""
    '    Try
    '        '*** 201809 Bollettazione Vigliano in OPENgov***
    '        nCol = 19 '18
    '        'prelevo il ruolo
    '        oMyRuolo = Session("oRuoloTIA")
    '        'valorizzo il nome del file
    '        sNameXLS = ConstSession.IdEnte & "_MINUTARATE_" & oMyRuolo(0).sAnno & "_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
    '        Dim DvDati As New DataView
    '        DvDati = FncRuolo.GetMinutaRate(oMyRuolo(0).IdFlusso)
    '        If Not DvDati Is Nothing Then
    '            If DvDati.Count > 0 Then
    '                DtDatiStampa = FncStampa.PrintMinutaRate(DvDati, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, oMyRuolo(0).sAnno, nCol)
    '            End If
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.Calcolo.CmdStampaMinutaRate_Click.errore: ", Err)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    Finally
    '        CacheManager.RemoveAvanzamentoElaborazione()
    '    End Try
    '    If Not DtDatiStampa Is Nothing Then
    '        If FncRuolo.SetDocumentiElaborati(oMyRuolo(0)) = False Then
    '            Response.Redirect("../../../PaginaErrore.aspx")
    '        End If
    '        If FncRuolo.UpdateDateRuolo(oMyRuolo, 4, "I") = False Then
    '            Response.Redirect("../../../PaginaErrore.aspx")
    '        End If
    '        'definisco le colonne
    '        aListColonne = New ArrayList
    '        For x = 0 To nCol
    '            aListColonne.Add("")
    '        Next
    '        aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())
    '        'definisco l'insieme delle colonne da esportare
    '        Dim MyCol() As Integer = New Integer(nCol) {}
    '        For x = 0 To nCol
    '            MyCol(x) = x
    '        Next
    '        'esporto i dati in excel
    '        Dim MyStampa As New RKLib.ExportData.Export("Web")
    '        MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sPathProspetti & sNameXLS)
    '        Str = sPathProspetti & sNameXLS
    '    End If
    'End Sub
    '**** 201809 - Cartelle Insoluti ***
    ''' <summary>
    ''' Pulsante per l'approvazione dei documenti del ruolo selezionato. Per l'approvazione devono essere stati stampati tutti i documenti; altrimenti messaggio di blocco.
    ''' L'approvazione comporta il popolamento delle tabelle "Storico" dei documenti e lo svuotamento delle tabelle dei documenti da elaborare.
    ''' Se il ruolo è di tipo "CartelleInsoluti" viene richiamata la funzione ClsGestDocumenti.ApprovaRuoloCartelleInsoluti.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdApprovaDoc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdApprovaDoc.Click
        Dim cmdMyCommand As SqlClient.SqlCommand = Nothing

        Try
            cmdMyCommand = New SqlClient.SqlCommand
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0

            '********************************************************
            'è necessario controllare se ci sono ancora delle cartelle per  le quali ci sono ancora dei documenti da elaorare
            '********************************************************
            If Session("nDocDaElaborare") > 0 Then
                lblElaborazioniEffettuate.Text = "L'elaborazione del ruolo selezionato non e' ancora terminata! Ci sono ancora " & Session("nDocDaElaborare") & " documenti da elaborare!"
            Else
                If oMyRuolo(0).sTipoRuolo = ObjRuolo.Ruolo.CartelleInsoluti Then
                    If FncDoc.ApprovaRuoloCartelleInsoluti(oMyRuolo(0).IdFlusso, cmdMyCommand) = False Then
                        Response.Redirect("../../../PaginaErrore.aspx")
                        Exit Sub
                    End If
                End If
                '********************************************************
                'se i documenti sono stati tutti elaborati chiedere conferma prima di procedere
                'se l'operatore conferma inserire la data di elaborazione doc nella tabella TBLRUOLI_GENERATI spostare i record da TBLGUIDA_COMUNICO a TBLGUIDA_COMUNICO_STORICO
                '********************************************************
                If FncDoc.SetTabGuidaComunicoStorico(ConstSession.IdEnte, oMyRuolo(0).IdFlusso, cmdMyCommand) = 0 Then
                    Log.Debug("Si è verificato un errore in RicercaDoc::CmdApprovaDoc_Click:: errore in SetTabGuidaComunicoStorico")
                    Response.Redirect("../../../PaginaErrore.aspx")
                    Exit Sub
                End If

                If FncDoc.SetTabFilesComunicoElabStorico(ConstSession.IdEnte, oMyRuolo(0).IdFlusso, cmdMyCommand) = 0 Then
                    Log.Debug("Si è verificato un errore in RicercaDoc::CmdApprovaDoc_Click:: errore in SetTabFilesComunicoElabStorico")
                    Response.Redirect("../../../PaginaErrore.aspx")
                    Exit Sub
                End If

                If FncRuolo.SetDateRuolo(oMyRuolo(0).IdFlusso, 5, "I") = 0 Then
                    ''si è verificato uin errore devo svuotare lo storico
                    'If FncDoc.DeleteTabGuidaComunico(ConstSession.IdEnte, "TBLGUIDA_COMUNICO_STORICO", oMyRuolo(0).IdFlusso, Utility.Costanti.TRIBUTO_TARSU) = 0 Then
                    '    Log.Debug("Si è verificato un errore in RicercaDoc::CmdApprovaDoc_Click::errore in DeleteTabGuidaComunico")
                    '    Response.Redirect("../../../PaginaErrore.aspx")
                    '    Exit Sub
                    'End If

                    'If FncDoc.DeleteTabGuidaComunico(ConstSession.IdEnte, "TBLDOCUMENTI_ELABORATI_STORICO", oMyRuolo(0).IdFlusso, Utility.Costanti.TRIBUTO_TARSU) = 0 Then
                    '    Log.Debug("Si è verificato un errore in RicercaDoc::CmdApprovaDoc_Click::errore in DeleteTabGuidaComunico")
                    '    Response.Redirect("../../../PaginaErrore.aspx")
                    '    Exit Sub
                    'End If
                    Log.Debug("Si è verificato un errore in RicercaDoc::CmdApprovaDoc_Click::errore in SetDateRuoliGenerati")
                    Response.Redirect("../../../PaginaErrore.aspx")
                    Exit Sub
                End If
                Page_Load(sender, e)
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.RicercaDoc.CmdApprovaDoc_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        Finally
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Sub
    ''' <summary>
    ''' Funzione che elimina l'elaborazione dei documenti del ruolo in elaborazione.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdEliminaDoc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdEliminaDoc.Click
        '********************************************************
        'un'elaborazione si può eliminare solo se non è stata data l'approvazione 
        '********************************************************
        Try
            If oMyRuolo(0).tDataOKDoc <> DateTime.MinValue And oMyRuolo(0).tDataOKDoc <> Date.MaxValue Then
                lblElaborazioniEffettuate.Text = "E' già presente la data di approvazione elaborazione documenti!Non si può tornare indietro!"
                Exit Sub
            End If
            If FncRuolo.SetDateRuolo(oMyRuolo(0).IdFlusso, 4, "C") = 0 Then
                Log.Debug("Si è verificato un errore in RicercaDoc::CmdEliminaDoc_Click::SetDateRuolo")
                Response.Redirect("../../../PaginaErrore.aspx")
                Exit Sub
            End If
            If FncDoc.DeleteTabGuidaComunico(ConstSession.IdEnte, "TBLGUIDA_COMUNICO", oMyRuolo(0).IdFlusso, Utility.Costanti.TRIBUTO_TARSU) = 0 Then
                Log.Debug("Si è verificato un errore in RicercaDoc::CmdEliminaDoc_Click::errore in DeleteTabGuidaComunico")
                Response.Redirect("../../../PaginaErrore.aspx")
                Exit Sub
            End If
            If FncDoc.DeleteTabGuidaComunico(ConstSession.IdEnte, "TBLDOCUMENTI_ELABORATI", oMyRuolo(0).IdFlusso, Utility.Costanti.TRIBUTO_TARSU) = 0 Then
                Log.Debug("Si è verificato un errore in RicercaDoc::CmdEliminaDoc_Click::errore in DeleteTabFilesComunicoElab")
                Response.Redirect("../../../PaginaErrore.aspx")
                Exit Sub
            End If

            Page_Load(sender, e)
            lblElaborazioniEffettuate.Text = "Elaborazione documenti eliminata correttamente!"
            sScript = "parent.Comandi.location.href = '../../../aspVuotaRemoveComandi.aspx';"
            sScript += "parent.Visualizza.location.href ='../Calcolo/Calcolo.aspx';"
            RegisterScript(sScript, Me.GetType)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.RicercaDoc.CmdEliminaDoc_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Funzione per il reindirizzamento alla pagina di download dei documenti prodotti.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdViewDocElab_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdViewDocElab.Click
        Dim oListDocStampati() As RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL
        Try
            oMyRuolo = Session("oRuoloTIA")
            oListDocStampati = New ClsGestDocumenti().GetDocElaborati(oMyRuolo(0).sEnte, ConstSession.CodTributo, oMyRuolo(0).IdFlusso)
            If Not oListDocStampati Is Nothing Then
                If oListDocStampati.GetLength(0) > 0 Then
                    Session.Add("ELENCO_DOCUMENTI_STAMPATI", oListDocStampati)
                    sScript = "document.getElementById('DivAttesa').style.display = 'none';"
                    sScript += "document.getElementById('divStampa').style.display = '';"
                    sScript += "document.getElementById('divAvviso').style.display = 'none';"
                    sScript += "document.getElementById('loadStampa').src = 'ViewDocElaborati.aspx?Provenienza=E';"
                    RegisterScript(sScript, Me.GetType)
                Else
                    lblElaborazioniEffettuate.Text = "Non ci sono documenti da scaricare!"
                End If
            Else
                lblElaborazioniEffettuate.Text = "Non ci sono documenti da elaborare!"
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.RicercaDoc.CmdViewDocElab_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
