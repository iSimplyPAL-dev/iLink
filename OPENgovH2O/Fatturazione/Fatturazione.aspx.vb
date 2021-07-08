Imports log4net
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti

Partial Class Fatturazione
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(Fatturazione))

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
    ''' Gestione della fatturazione
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            Dim FncGen As New ClsGenerale.Generale
            FncGen.GetPeriodoAttuale
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
            Dim paginacomandi As String = Request("paginacomandi")
            If Len(paginacomandi) = 0 Then
                paginacomandi = ConstSession.PathApplicazione & ConstSession.Path_H2O & "/Fatturazione/ComandiFatturazione.aspx"
            End If
            Dim parametri As String
            parametri = "?title=Acquedotto - Fatturazione&enteperiodo=" & " Ente: " & ConstSession.DescrizioneEnte & " - Periodo: " & ConstSession.DescrPeriodo

            Dim sScript As String = ""
            sScript += "document.getElementById('paginacomandi').value='" & paginacomandi & "';"
            sScript += "parent.Comandi.location.href='" & paginacomandi & parametri & "';"
            RegisterScript(sScript, Me.GetType())

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
            lblLinkFile290.Attributes.Add("onclick", "CmdScarica.click()")
            If Page.IsPostBack = False Then
                Dim FunctionRuoloH2O As New ClsRuoloH2O
                Dim oMyTotRuolo As New ObjTotRuoloFatture
                Dim oListTotRuolo() As ObjTotRuoloFatture
                Dim FunctionFatture As New ClsFatture
                Dim nContatoriNoLett As Integer

                'prelevo i dati da fatturare
                oMyTotRuolo = FunctionRuoloH2O.GetDatiDaFatturare(ConstSession.IdEnte, ConstSession.IdPeriodo, ConstSession.CodTributo, CStr(ConstSession.DescrPeriodo).Substring(0, 4), nContatoriNoLett)
                If Not oMyTotRuolo Is Nothing Then
                    LblNContribDaFatt.Text = oMyTotRuolo.nNContribuenti
                    LblNLettureDaFatt.Text = oMyTotRuolo.nNDocumenti
                    LblNotePrec.Text = oMyTotRuolo.sNote
                End If
                TxtNContatoriNoLettura.Text = nContatoriNoLett

                'prelevo il ruolo in corso
                oMyTotRuolo = FunctionRuoloH2O.GetRuoloH2OAtt(ConstSession.IdEnte, ConstSession.IdPeriodo)
                If Not oMyTotRuolo Is Nothing Then
                    ReDim Preserve oListTotRuolo(0)
                    oListTotRuolo(0) = oMyTotRuolo
                    VisualizzaRiepilogo(oMyTotRuolo, oListTotRuolo)
                End If
                Dim sAnno As String
                If TxtDataFattura.Text = "" Then
                    TxtDataFattura.Text = Now.ToShortDateString
                Else
                    TxtNIniziale.ReadOnly = True : TxtDataFattura.ReadOnly = True : TxtDataScadenza.ReadOnly = True
                End If
                sAnno = Year(TxtDataFattura.Text)
                TxtNIniziale.Text = FunctionFatture.GetFirstNFattura(ConstSession.IdEnte, sAnno)

                'prelevo i dati fatturati in precedenza
                oListTotRuolo = FunctionRuoloH2O.GetRuoloH2OPrec(ConstSession.IdEnte, ConstSession.IdPeriodo)
                If Not oListTotRuolo Is Nothing Then
                    'popolo la griglia
                    GrdFatturazioniPrec.DataSource = oListTotRuolo
                    GrdFatturazioniPrec.DataBind()
                    GrdFatturazioniPrec.SelectedIndex = -1
                End If
                'aggiorno la variabile di sessione
                Session("oRuoloH2O") = oMyTotRuolo
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Fatturazione.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' pulsante di calcolo documenti da emettere per il periodo attivo
    ''' il calcolo può essere rifatto fintantoché non si ha la minuta approvata
    ''' calcolo una fattura per ogni lettura
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CmdCalcola_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdCalcola.Click
        Dim oMyTotRuolo As New ObjTotRuoloFatture
        Dim sScript As String
        Dim FunctionRuolo As New ClsCreaFatture
        Dim oListTotRuolo() As ObjTotRuoloFatture

        Try
            If LblNContribDaFatt.Text <> 0 Then
                oMyTotRuolo = Session("oRuoloH2O")
                If Not oMyTotRuolo Is Nothing Then
                    'se il ruolo non è ancora stato chiuso
                    If oMyTotRuolo.tDataApprovazioneDOC = Date.MaxValue Then
                        'non deve essere approvata la minuta
                        If oMyTotRuolo.tDataOkMinuta.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                            sScript = "GestAlert('a', 'warning', '', '', 'Minuta già approvata!\nImpossibile rieseguire i calcoli!');"
                            RegisterScript(sScript, Me.GetType())
                            DivAttesa.Style.Add("display", "none")
                            Exit Sub
                        End If
                    Else
                        'svuoto la variabile di sessione del ruolo prima di elaborare il successivo per lo stesso periodo
                        oMyTotRuolo = New ObjTotRuoloFatture
                    End If
                Else
                    oMyTotRuolo = New ObjTotRuoloFatture
                End If
                Log.Debug("Fatturazione::CmdCalcola_Click::richiamo CreaRuoloFatture::Session(CODENTE)::" & ConstSession.IdEnte & "::Session(PERIODOID)::" & ConstSession.IdPeriodo & "::oMyTotRuolo.IdFlusso::" & oMyTotRuolo.IdFlusso & "::Session(COD_TRIBUTO)::" & Session("COD_TRIBUTO") & "::Session(username)::" & Session("username") & "::CStr(Session(PERIODO)).Substring(0, 4)::" & CStr(ConstSession.DescrPeriodo).Substring(0, 4))
                oMyTotRuolo = FunctionRuolo.CreaRuoloFatture(ConstSession.IdEnte, ConstSession.IdPeriodo, oMyTotRuolo.IdFlusso, Session("COD_TRIBUTO"), ConstSession.UserName, CStr(ConstSession.DescrPeriodo).Substring(0, 4))
                If Not oMyTotRuolo Is Nothing Then
                    ReDim Preserve oListTotRuolo(0)
                    oListTotRuolo(0) = oMyTotRuolo
                    VisualizzaRiepilogo(oMyTotRuolo, oListTotRuolo)

                    If LblNote.Text = "" Then
                        sScript = "GestAlert('a', 'success', '', '', 'Elaborazione terminata correttamente!');"
                    Else
                        sScript = "GestAlert('a', 'warning', '', '', 'L\'elaborazione e\' stata terminata a causa di un errore!');"
                    End If
                Else
                    sScript = "GestAlert('a', 'warning', '', '', 'L\'elaborazione e\' stata terminata a causa di un errore!');"
                End If
            Else
                oMyTotRuolo = Session("oRuoloH2O")
                sScript = "GestAlert('a', 'warning', '', '', 'Non sono presenti Letture da Fatturare!');"
                LblNote.Text = "Non sono presenti Letture da Fatturare!"
            End If
            RegisterScript(sScript, Me.GetType())
            DivAttesa.Style.Add("display", "none")
            'aggiorno la variabile di sessione
            Session("oRuoloH2O") = oMyTotRuolo
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Fatturazione.CmdCalcola_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' pulsante per la cancellazione del calcolo fatto
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CmdDeleteFatturazione_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdDeleteFatturazione.Click
        Dim sScript As String
        Dim FunctionRuolo As New ClsRuoloH2O
        Dim oMyTotRuolo As New ObjTotRuoloFatture
        Dim oListTotRuolo() As ObjTotRuoloFatture
        Dim FunctionFatture As New ClsFatture
        Dim nContatoriNoLett As Integer

        Try
            oMyTotRuolo = Session("oRuoloH2O")
            If oMyTotRuolo.tDataApprovazioneDOC.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                sScript = "GestAlert('a', 'warning', '', '', 'Documenti gia\' approvati!');"
                RegisterScript(sScript, Me.GetType())
            Else
                If FunctionRuolo.DeleteRuoloH2O(oMyTotRuolo.IdFlusso) <> 0 Then
                    'prelevo i dati da fatturare
                    oMyTotRuolo = FunctionRuolo.GetDatiDaFatturare(ConstSession.IdEnte, ConstSession.IdPeriodo, ConstSession.CodTributo, CStr(ConstSession.DescrPeriodo).Substring(0, 4), nContatoriNoLett)
                    If Not oMyTotRuolo Is Nothing Then
                        LblNContribDaFatt.Text = oMyTotRuolo.nNContribuenti
                        LblNLettureDaFatt.Text = oMyTotRuolo.nNDocumenti
                        LblNotePrec.Text = oMyTotRuolo.sNote
                    End If
                    TxtNContatoriNoLettura.Text = nContatoriNoLett

                    'prelevo il ruolo in corso
                    oMyTotRuolo = FunctionRuolo.GetRuoloH2OAtt(ConstSession.IdEnte, ConstSession.IdPeriodo)
                    If Not oMyTotRuolo Is Nothing Then
                        ReDim Preserve oListTotRuolo(0)
                        oListTotRuolo(0) = oMyTotRuolo
                        VisualizzaRiepilogo(oMyTotRuolo, oListTotRuolo)
                    End If
                    TxtNIniziale.Text = FunctionFatture.GetFirstNFattura(ConstSession.IdEnte, CStr(ConstSession.DescrPeriodo).Substring(0, 4))
                    If TxtDataFattura.Text = "" Then
                        TxtDataFattura.Text = Now.ToShortDateString
                    Else
                        TxtNIniziale.ReadOnly = True : TxtDataFattura.ReadOnly = True : TxtDataScadenza.ReadOnly = True
                    End If

                    Session("oRuoloH2O") = oMyTotRuolo
                    sScript = "GestAlert('a', 'success', '', '', 'Eliminazione Fatturazione effettuata con succcesso!');"
                    RegisterScript(sScript, Me.GetType())
                Else
                    sScript = "GestAlert('a', 'danger', '', '', 'Errore in eliminazione Fatturazione!');"
                    RegisterScript(sScript, Me.GetType())
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Fatturazione.CmdDeleteFatturazione.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    'Private Sub CmdStampaMinuta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdStampaMinuta.Click
    'Try
    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Dim WFErrore As String
    '    Dim sPathProspetti, sNameFile, Str As String
    '    Dim oMyTotRuolo As New ObjTotRuoloFatture
    '    Dim oListFatture() As ObjFattura
    '    Dim FunctionFatture As New ClsFatture
    '    Dim sScript As String
    '    Dim x As Integer
    '    Dim FunctionStampa As New ClsStampaXLS
    '    Dim DtDatiStampa As New DataTable
    '    Dim aListColonne As ArrayList
    '    Dim aMyHeaders As String()
    '    Dim oListTotRuolo() As ObjTotRuoloFatture

    '    Log.Debug("Entro in Fatturazione::CmdStampaMinuta_Click::" & Now.ToString)
    '    oMyTotRuolo = Session("oRuoloH2O")
    '    'devono essere stati fatti i calcoli
    '    If oMyTotRuolo.tDataCalcoli = Date.MinValue Then 'LblDataCalcolo.Text = "" Then
    '        sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario elaborare le fatture prima di stampare la minuta!')"
    '        RegisterScript(sScript , Me.GetType())
    '    Else
    '        'inizializzo la connessione
    '        WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If
    '        'prelevo i ruoli per i parametri inseriti
    '        oListFatture = FunctionFatture.GetFattura(ConstSession.IdEnte, CInt(LblIdFlusso.Text), -1, False, WFSessione)
    '        Log.Debug("Sono in Fatturazione::CmdStampaMinuta_Click:: devo valorizzare il datatable::" & Now.ToString)
    '        'valorizzo il nome del file
    '        sPathProspetti = ConstSession.PathProspetti
    '        sNameFile = ConstSession.DescrizioneEnte & "_" & ConstSession.IdEnte & "_MINUTA_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
    '        DtDatiStampa = FunctionStampa.PrintMinutaRuoloH2O(oListFatture, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, ConstSession.DescrPeriodo)
    '        Log.Debug("Sono in Fatturazione::CmdStampaMinuta_Click:: ho valorizzato il datatable::" & Now.ToString)
    '        'aggiorno la variabile di sessione
    '        oMyTotRuolo.tDataStampaMinuta = Now 'LblDataMinuta.Text = Now.ToShortDateString
    '        ReDim Preserve oListTotRuolo(0)
    '        oListTotRuolo(0) = oMyTotRuolo
    '        VisualizzaRiepilogo(oMyTotRuolo, oListTotRuolo)
    '        WFSessione.Kill()

    '        Session("oRuoloH2O") = oMyTotRuolo
    '        If Not DtDatiStampa Is Nothing Then
    '            'definisco le colonne
    '            Log.Debug("Sono in Fatturazione::CmdStampaMinuta_Click:: definisco le colonne::" & Now.ToString)
    '            aListColonne = New ArrayList
    '            For x = 0 To 36
    '                aListColonne.Add("")
    '            Next
    '            aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

    '            'definisco l'insieme delle colonne da esportare
    '            Dim MyCol() As Integer = New Integer() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36}
    '            'esporto i dati in excel
    '            Log.Debug("Sono in Fatturazione::CmdStampaMinuta_Click:: esporto i dati in excel::" & Now.ToString)
    '            Dim MyStampa As New RKLib.ExportData.Export("Web")
    '            MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameFile)
    '            Str = sPathProspetti & sNameFile
    '            Log.Debug("Esco da Fatturazione::CmdStampaMinuta_Click::" & Now.ToString)
    '        End If
    '    End If
    'Catch Err As Exception

    '     Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Fatturazione.CmdStampaMinuta_Click.errore: ", Err)
    '     Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub

    ''' <summary>
    ''' pulsante per la creazione della minuta in formato excel
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Esportazione completa dati</em>
    ''' </revision>
    ''' </revisionHistory>
    '''<revisionHistory><revision date="18/02/2020">bisogna esporre separatamento tutte le voci dei canoni</revision></revisionHistory>
    Private Sub CmdStampaMinuta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdStampaMinuta.Click
        Dim sNameXLS As String
        Dim oMyTotRuolo As New ObjTotRuoloFatture
        Dim oListFatture() As ObjFattura
        Dim FunctionFatture As New ClsFatture
        Dim sScript As String
        Dim x, nCol As Integer
        Dim FunctionStampa As New ClsStampaXLS
        Dim DtDatiStampa As New DataTable
        Dim aListColonne As ArrayList
        Dim aMyHeaders As String()
        Dim oListTotRuolo() As ObjTotRuoloFatture

        Log.Debug("Entro in Fatturazione::CmdStampaMinuta_Click::" & Now.ToString)
        nCol = 51
        oMyTotRuolo = Session("oRuoloH2O")
        'devono essere stati fatti i calcoli
        If oMyTotRuolo.tDataCalcoli = Date.MaxValue Then 'LblDataCalcolo.Text = "" Then
            sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario elaborare le fatture prima di stampare la minuta!');"
            RegisterScript(sScript, Me.GetType())
        Else
            'prelevo i ruoli per i parametri inseriti
            oListFatture = FunctionFatture.GetFattura(ConstSession.StringConnection, ConstSession.IdEnte, CInt(LblIdFlusso.Text), -1, False)
            Log.Debug("Sono in Fatturazione::CmdStampaMinuta_Click:: devo valorizzare il datatable::" & Now.ToString)
            'valorizzo il nome del file
            sNameXLS = ConstSession.IdEnte & "_MINUTA_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
            DtDatiStampa = FunctionStampa.PrintMinutaRuoloH2O(oListFatture, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, ConstSession.DescrPeriodo, hdMinutaAnagAllRow.Value, nCol)
            Log.Debug("Sono in Fatturazione::CmdStampaMinuta_Click:: ho valorizzato il datatable::" & Now.ToString)
            'aggiorno la variabile di sessione
            oMyTotRuolo.tDataStampaMinuta = Now
            ReDim Preserve oListTotRuolo(0)
            oListTotRuolo(0) = oMyTotRuolo
            VisualizzaRiepilogo(oMyTotRuolo, oListTotRuolo)

            Session("oRuoloH2O") = oMyTotRuolo
            If Not DtDatiStampa Is Nothing Then
                'definisco le colonne
                Log.Debug("Sono in Fatturazione::CmdStampaMinuta_Click:: definisco le colonne::" & Now.ToString)
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
                Log.Debug("Sono in Fatturazione::CmdStampaMinuta_Click:: esporto i dati in excel::" & Now.ToString)
                Dim MyStampa As New RKLib.ExportData.Export("Web")
                MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
                Log.Debug("Esco da Fatturazione::CmdStampaMinuta_Click::" & Now.ToString)
            End If
        End If
    End Sub
    'Private Sub CmdStampaMinuta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdStampaMinuta.Click
    '    Dim sNameXLS, Str As String
    '    Dim oMyTotRuolo As New ObjTotRuoloFatture
    '    Dim oListFatture() As ObjFattura
    '    Dim FunctionFatture As New ClsFatture
    '    Dim sScript As String
    '    Dim x, nCol As Integer
    '    Dim FunctionStampa As New ClsStampaXLS
    '    Dim DtDatiStampa As New DataTable
    '    Dim aListColonne As ArrayList
    '    Dim aMyHeaders As String()
    '    Dim oListTotRuolo() As ObjTotRuoloFatture

    '    Log.Debug("Entro in Fatturazione::CmdStampaMinuta_Click::" & Now.ToString)
    '    nCol = 45
    '    oMyTotRuolo = Session("oRuoloH2O")
    '    'devono essere stati fatti i calcoli
    '    If oMyTotRuolo.tDataCalcoli = Date.MaxValue Then 'LblDataCalcolo.Text = "" Then
    '        sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario elaborare le fatture prima di stampare la minuta!');"
    '        RegisterScript(sScript, Me.GetType())
    '    Else
    '        'prelevo i ruoli per i parametri inseriti
    '        oListFatture = FunctionFatture.GetFattura(ConstSession.StringConnection, ConstSession.IdEnte, CInt(LblIdFlusso.Text), -1, False)
    '        Log.Debug("Sono in Fatturazione::CmdStampaMinuta_Click:: devo valorizzare il datatable::" & Now.ToString)
    '        'valorizzo il nome del file
    '        sNameXLS = ConstSession.IdEnte & "_MINUTA_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
    '        DtDatiStampa = FunctionStampa.PrintMinutaRuoloH2O(oListFatture, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, ConstSession.DescrPeriodo, hdMinutaAnagAllRow.Value, nCol)
    '        Log.Debug("Sono in Fatturazione::CmdStampaMinuta_Click:: ho valorizzato il datatable::" & Now.ToString)
    '        'aggiorno la variabile di sessione
    '        oMyTotRuolo.tDataStampaMinuta = Now
    '        ReDim Preserve oListTotRuolo(0)
    '        oListTotRuolo(0) = oMyTotRuolo
    '        VisualizzaRiepilogo(oMyTotRuolo, oListTotRuolo)

    '        Session("oRuoloH2O") = oMyTotRuolo
    '        If Not DtDatiStampa Is Nothing Then
    '            'definisco le colonne
    '            Log.Debug("Sono in Fatturazione::CmdStampaMinuta_Click:: definisco le colonne::" & Now.ToString)
    '            aListColonne = New ArrayList
    '            For x = 0 To nCol
    '                aListColonne.Add("")
    '            Next
    '            aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

    '            'definisco l'insieme delle colonne da esportare
    '            Dim MyCol() As Integer = New Integer(nCol) {}
    '            For x = 0 To nCol
    '                MyCol(x) = x
    '            Next
    '            'esporto i dati in excel
    '            Log.Debug("Sono in Fatturazione::CmdStampaMinuta_Click:: esporto i dati in excel::" & Now.ToString)
    '            Dim MyStampa As New RKLib.ExportData.Export("Web")
    '            MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
    '            Str = sNameXLS
    '            Log.Debug("Esco da Fatturazione::CmdStampaMinuta_Click::" & Now.ToString)
    '        End If
    '    End If
    'End Sub
    'Private Sub CmdStampaMinuta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdStampaMinuta.Click
    '    Dim sPathProspetti, sNameXLS, Str As String
    '    Dim oMyTotRuolo As New ObjTotRuoloFatture
    '    Dim oListFatture() As ObjFattura
    '    Dim FunctionFatture As New ClsFatture
    '    Dim sScript As String
    '    Dim x, nCol As Integer
    '    Dim FunctionStampa As New ClsStampaXLS
    '    Dim DtDatiStampa As New DataTable
    '    Dim aListColonne As ArrayList
    '    Dim aMyHeaders As String()
    '    Dim oListTotRuolo() As ObjTotRuoloFatture

    '    Log.Debug("Entro in Fatturazione::CmdStampaMinuta_Click::" & Now.ToString)
    '    nCol = 45 '41
    '    oMyTotRuolo = Session("oRuoloH2O")
    '    'devono essere stati fatti i calcoli
    '    If oMyTotRuolo.tDataCalcoli = Date.MinValue Then 'LblDataCalcolo.Text = "" Then
    '        sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario elaborare le fatture prima di stampare la minuta!')"
    '        RegisterScript(sScript, Me.GetType())
    '    Else
    '        'prelevo i ruoli per i parametri inseriti
    '        oListFatture = FunctionFatture.GetFattura(ConstSession.IdEnte, CInt(LblIdFlusso.Text), -1, False)
    '        Log.Debug("Sono in Fatturazione::CmdStampaMinuta_Click:: devo valorizzare il datatable::" & Now.ToString)
    '        'valorizzo il nome del file
    '        'sPathProspetti = ConstSession.PathProspetti
    '        sNameXLS = ConstSession.IdEnte & "_MINUTA_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
    '        '*** 20141107 ***
    '        'DtDatiStampa = FunctionStampa.PrintMinutaRuoloH2O(oListFatture, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, ConstSession.DescrPeriodo, hdMinutaAnagAllRow.value)
    '        DtDatiStampa = FunctionStampa.PrintMinutaRuoloH2O(oListFatture, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, ConstSession.DescrPeriodo, hdMinutaAnagAllRow.Value, nCol)
    '        '*** ***
    '        Log.Debug("Sono in Fatturazione::CmdStampaMinuta_Click:: ho valorizzato il datatable::" & Now.ToString)
    '        'aggiorno la variabile di sessione
    '        oMyTotRuolo.tDataStampaMinuta = Now 'LblDataMinuta.Text = Now.ToShortDateString
    '        ReDim Preserve oListTotRuolo(0)
    '        oListTotRuolo(0) = oMyTotRuolo
    '        VisualizzaRiepilogo(oMyTotRuolo, oListTotRuolo)

    '        Session("oRuoloH2O") = oMyTotRuolo
    '        If Not DtDatiStampa Is Nothing Then
    '            'definisco le colonne
    '            Log.Debug("Sono in Fatturazione::CmdStampaMinuta_Click:: definisco le colonne::" & Now.ToString)
    '            aListColonne = New ArrayList
    '            For x = 0 To nCol
    '                aListColonne.Add("")
    '            Next
    '            aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

    '            'definisco l'insieme delle colonne da esportare
    '            Dim MyCol() As Integer = New Integer(nCol) {}
    '            For x = 0 To nCol
    '                MyCol(x) = x
    '            Next
    '            'esporto i dati in excel
    '            Log.Debug("Sono in Fatturazione::CmdStampaMinuta_Click:: esporto i dati in excel::" & Now.ToString)
    '            Dim MyStampa As New RKLib.ExportData.Export("Web")
    '            MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
    '            Str = sPathProspetti & sNameXLS
    '            Log.Debug("Esco da Fatturazione::CmdStampaMinuta_Click::" & Now.ToString)
    '        End If
    '    End If
    'End Sub
    ''' <summary>
    ''' pulsante per l'approvazione della minuta
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CmdApprovaMinuta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdApprovaMinuta.Click
        Dim sScript As String
        Dim FunctionRuolo As New ClsRuoloH2O
        Dim oMyTotRuolo As New ObjTotRuoloFatture
        Dim oListTotRuolo() As ObjTotRuoloFatture

        Try
            oMyTotRuolo = Session("oRuoloH2O")
            If oMyTotRuolo.tDataOkMinuta.ToShortDateString <> Date.MaxValue.ToShortDateString Then 'LblDataOkMinuta.Text <> "" Then
                sScript = "GestAlert('a', 'warning', '', '', 'Minuta già approvata!');"
                RegisterScript(sScript, Me.GetType())
            ElseIf oMyTotRuolo.tDataStampaMinuta = Date.MaxValue Then 'LblDataMinuta.Text = "" Then
                sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario produrre la Minuta prima di fare l\'approvazione!');"
                RegisterScript(sScript, Me.GetType())
            Else
                If FunctionRuolo.SetDateRuoliH2OGenerati(oMyTotRuolo.IdFlusso, 1) <> 0 Then
                    'aggiorno la variabile di sessione
                    oMyTotRuolo.tDataOkMinuta = Now 'LblDataOkMinuta.Text = Now.ToShortDateString
                    ReDim Preserve oListTotRuolo(0)
                    oListTotRuolo(0) = oMyTotRuolo
                    VisualizzaRiepilogo(oMyTotRuolo, oListTotRuolo)

                    Session("oRuoloH2O") = oMyTotRuolo
                    sScript = "GestAlert('a', 'warning', '', '', 'Approvazione effettuata con succcesso!');"
                    RegisterScript(sScript, Me.GetType())
                Else
                    sScript = "GestAlert('a', 'danger', '', '', 'Errore in approvazione Minuta!');"
                    RegisterScript(sScript, Me.GetType())
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Fatturazione.CmdApprovaMinuta_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' pulsante per l'eliminazione della minuta generata
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CmdCancellaMinuta_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdCancellaMinuta.Click
        Dim sScript As String
        Dim FunctionRuolo As New ClsRuoloH2O
        Dim oMyTotRuolo As New ObjTotRuoloFatture
        Dim oListTotRuolo() As ObjTotRuoloFatture

        Try
            oMyTotRuolo = Session("oRuoloH2O")
            If oMyTotRuolo.tDataNumerazione.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                sScript = "GestAlert('a', 'warning', '', '', 'Numerazione Documenti gia\' effettuata!');"
                RegisterScript(sScript, Me.GetType())
            ElseIf oMyTotRuolo.tDataOkMinuta = Date.MaxValue Then
                sScript = "GestAlert('a', 'warning', '', '', 'Minuta non approvata!');"
                RegisterScript(sScript, Me.GetType())
            ElseIf oMyTotRuolo.tDataStampaMinuta = Date.MaxValue Then
                sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario produrre la Minuta prima di togliere l\'approvazione!');"
                RegisterScript(sScript, Me.GetType())
            Else
                If FunctionRuolo.SetDateRuoliH2OGenerati(oMyTotRuolo.IdFlusso, 1, "C") <> 0 Then
                    'aggiorno la variabile di sessione
                    oMyTotRuolo.tDataOkMinuta = Date.MaxValue
                    ReDim Preserve oListTotRuolo(0)
                    oListTotRuolo(0) = oMyTotRuolo
                    VisualizzaRiepilogo(oMyTotRuolo, oListTotRuolo)

                    Session("oRuoloH2O") = oMyTotRuolo
                    sScript = "GestAlert('a', 'warning', '', '', 'Eliminazione approvazione effettuata con succcesso!');"
                    RegisterScript(sScript, Me.GetType())
                Else
                    sScript = "GestAlert('a', 'danger', '', '', 'Errore in eliminazione Approvazione Minuta!');"
                    RegisterScript(sScript, Me.GetType())
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Fatturazione.CmdCancellaMinuta_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' pulsante per numerazione e la generazione delle rate dei documenti
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CmdNumerazioneDocumenti_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdNumerazioneDocumenti.Click
        Dim sScript As String
        Dim FunctionRuolo As New ClsCreaFatture
        Dim oMyTotRuolo As New ObjTotRuoloFatture
        Dim oListTotRuolo() As ObjTotRuoloFatture

        Try
            oMyTotRuolo = Session("oRuoloH2O")
            If oMyTotRuolo.tDataNumerazione.ToShortDateString <> Date.MaxValue.ToShortDateString Then 'LblDataNumerazione.Text <> "" Then
                sScript = "GestAlert('a', 'warning', '', '', 'Numerazione Documenti già effettuata!');"
                RegisterScript(sScript, Me.GetType())
            ElseIf oMyTotRuolo.tDataOkMinuta = Date.MaxValue Then 'LblDataOkMinuta.Text = "" Then
                sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario approvare la Minuta prima di fare la numerazione!');"
                RegisterScript(sScript, Me.GetType())
            Else
                oMyTotRuolo.tDataEmissioneFattura = CDate(TxtDataFattura.Text)
                oMyTotRuolo.tDataScadenza = CDate(TxtDataScadenza.Text)
                If FunctionRuolo.NumeraDocumenti(oMyTotRuolo, TxtNIniziale.Text, TxtPrefissoFattura.Text, TxtSuffissoFattura.Text, ConstSession.CodTributo, ConstSession.UserName) = 0 Then
                    ReDim Preserve oListTotRuolo(0)
                    oListTotRuolo(0) = oMyTotRuolo
                    VisualizzaRiepilogo(oMyTotRuolo, oListTotRuolo)

                    sScript = "GestAlert('a', 'danger', '', '', 'Errore durante la Numerazione!');"
                    RegisterScript(sScript, Me.GetType())
                Else
                    'aggiorno la variabile di sessione
                    oMyTotRuolo.tDataNumerazione = Now 'LblDataNumerazione.Text = Now.ToShortDateString
                    ReDim Preserve oListTotRuolo(0)
                    oListTotRuolo(0) = oMyTotRuolo
                    VisualizzaRiepilogo(oMyTotRuolo, oListTotRuolo)

                    sScript = "GestAlert('a', 'success', '', '', 'Numerazione effettuata con succcesso!');"
                    RegisterScript(sScript, Me.GetType())
                End If
            End If
            Session("oRuoloH2O") = oMyTotRuolo
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Fatturazione.CmdNumerazioneDocumenti_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' pulsante per il richiamo della videata di generazione documenti
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CmdElaborazioniDocumenti_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdElaborazioniDocumenti.Click
        Dim sScript As String = ""
        Dim oMyTotRuolo As New ObjTotRuoloFatture

        Try
            oMyTotRuolo = Session("oRuoloH2O")
            If oMyTotRuolo.tDataApprovazioneDOC.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                sScript = "GestAlert('a', 'warning', '', '', 'Approvazione Documenti già effettuata!');"
                RegisterScript(sScript, Me.GetType())
            ElseIf oMyTotRuolo.tDataNumerazione = Date.MaxValue Then
                sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario Numerare i documenti prima di fare le stampe!');"
                RegisterScript(sScript, Me.GetType())
            Else
                sScript += "parent.parent.Comandi.location.href='../Documenti/ComandiDocumenti.aspx';"
                sScript += "parent.parent.Visualizza.location.href='../Documenti/RicercaDocumenti.aspx'"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.Fatturazione.CmdElaborazioneDocumenti_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    ''*** 201511 - template documenti per ruolo ***
    Protected Sub BtnUploadClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        Try
            Dim oMyTotRuolo As New ObjTotRuoloFatture
            oMyTotRuolo = Session("oRuoloH2O")

            If oMyTotRuolo.IdFlusso <= 0 Then
                Dim sScript As String = "GestAlert('a', 'warning', '', '', 'E\' necessario selezionare un Ruolo!');"
                RegisterScript(sScript, Me.GetType())
            Else
                If System.IO.Path.GetFileName(fileUpload.PostedFile.FileName) = "" Then
                    Dim sScript As String = "GestAlert('a', 'warning', '', '', 'E\' necessario selezionare un file!');"
                    RegisterScript(sScript, Me.GetType())
                Else
                    Dim myTemplateDoc As New ElaborazioneDatiStampeInterface.ObjTemplateDoc
                    myTemplateDoc.myStringConnection = ConstSession.StringConnectionOPENgov
                    myTemplateDoc.IdEnte = ConstSession.IdEnte
                    myTemplateDoc.IdTributo = ConstSession.CodTributo
                    myTemplateDoc.IdRuolo = oMyTotRuolo.IdFlusso
                    myTemplateDoc.FileMIMEType = fileUpload.PostedFile.ContentType
                    myTemplateDoc.PostedFile = fileUpload.FileBytes
                    myTemplateDoc.FileName = System.IO.Path.GetFileName(fileUpload.PostedFile.FileName)
                    myTemplateDoc.IdTemplateDoc = myTemplateDoc.Save()
                    If myTemplateDoc.IdTemplateDoc <= 0 Then
                        lblMessage.Text = "Si sono verificati errori durante il salvataggio del file."
                    Else
                        lblMessage.Text = "File caricato con successo."
                        lblMessage.CssClass = "Input_Label_bold"
                    End If

                    lblMessage.Visible = True
                    fileUpload = New FileUpload
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Fatturazione.btnUpload_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            DivAttesa.Style.Add("display", "none")
        End Try
    End Sub

    Protected Sub BtnDownloadClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDownload.Click
        Dim myTemplateDoc As New ElaborazioneDatiStampeInterface.ObjTemplateDoc
        Try
            Dim oMyTotRuolo As New ObjTotRuoloFatture
            oMyTotRuolo = Session("oRuoloH2O")
            If oMyTotRuolo.IdFlusso <= 0 Then
                Dim sScript As String = "GestAlert('a', 'warning', '', '', 'E\' necessario selezionare un Ruolo!');"
                RegisterScript(sScript, Me.GetType())
            Else
                myTemplateDoc.myStringConnection = ConstSession.StringConnectionOPENgov
                myTemplateDoc.IdEnte = ConstSession.IdEnte
                myTemplateDoc.IdTributo = ConstSession.CodTributo
                myTemplateDoc.IdRuolo = oMyTotRuolo.IdFlusso
                myTemplateDoc.Load()
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Fatturazione.btnDownload_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            DivAttesa.Style.Add("display", "none")
        End Try
        If Not myTemplateDoc.PostedFile Is Nothing Then
            Response.ContentType = myTemplateDoc.FileMIMEType
            Response.AddHeader("content-disposition", String.Format("attachment;filename=""{0}""", myTemplateDoc.FileName))
            Response.BinaryWrite(myTemplateDoc.PostedFile)
            Response.End()
        End If
    End Sub
    ''*** ***
#Region "290"
    'Private Sub CmdEstrazione290_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdEstrazione290.Click
    '    Dim sScript As String = ""
    '    Dim FunctionEstrazioni As New ClsCreaFatture
    '    Dim nReturnEstraz As Integer
    '    Dim sPathFile290, sNameFile290, sNomeErr As String
    '    Dim FncRate As New ClsRate
    '    Dim oListRate() As ObjConfiguraRata
    '    Dim oMyConfigRata As ObjConfiguraRata
    '    Dim oMyTotRuolo As New ObjTotRuoloFatture

    '    Try
    '        Session("nProgRuolo") = 0 'perchè non è previsto un numero di ruolo fisso
    '        'controllo che sia stato selezionato un ruolo
    '        If Not Session("oRuoloH2O") Is Nothing Then
    '            'controllo che siano stati configurati i codici enti
    '            If Session("idente_credben") <> "" And Session("idente_CNC") <> "" Then
    '                oMyTotRuolo = Session("oRuoloH2O")
    '                'controllo che il ruolo non sia stato elaborato come documenti
    '                If oMyTotRuolo.tDataNumerazione = Date.MinValue Then
    '                    sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario Numerare i documenti prima di fare le stampe!')"
    '                    RegisterScript(sScript , Me.GetType())
    '                ElseIf oMyTotRuolo.tDataApprovazioneDOC = Date.MinValue Then
    '                    'valorizzo il percorso e il nome del file
    '                    sPathFile290 = System.Configuration.ConfigurationManager.AppSettings("PATH_ESTRAZIONE290")
    '                    sNameFile290 = Session("idente_credben") & Format(Now, "yy") & ".001"
    '                    'prelevo le rate configurate
    '                    Log.Debug("ClsFatture::SetIdentificativiDoc::prelevo prelevo le rate configurate")
    '                    oListRate = FncRate.GetConfiguraRata(oMyTotRuolo.IdFlusso)
    '                    If oListRate Is Nothing Then
    '                        'vuol dire che non ho configurato le rate perchè ho solo l'unica soluzione quindi le configuro adesso
    '                        oMyConfigRata = New ObjConfiguraRata
    '                        oMyConfigRata.nIdRuolo = oMyTotRuolo.IdFlusso
    '                        oMyConfigRata.sNRata = "U"
    '                        oMyConfigRata.sIdEnte = ConstSession.IdEnte
    '                        oMyConfigRata.sDescrRata = "UNICA SOLUZIONE"
    '                        oMyConfigRata.tDataScadenza = CDate(TxtDataScadenza.Text)
    '                        ReDim Preserve oListRate(1)
    '                        oListRate(1) = oMyConfigRata
    '                    End If

    '                    If Not oListRate Is Nothing Then
    '                        'richiamo la funzione di estrazione
    '                        nReturnEstraz = FunctionEstrazioni.Crea290(oMyTotRuolo.IdFlusso, ConstSession.IdEnte, Session("idente_credben"), Session("idente_CNC"), Session("nProgRuolo"), oListRate.GetUpperBound(0), sPathFile290 + sNameFile290, sNomeErr)
    '                        If nReturnEstraz = -2 Then
    '                            sScript = "GestAlert('a', 'warning', '', '', 'Sono presenti delle anagrafiche con data nascita errata e/o senza l\'indicazione della natura giuridica!\nEstrazione non effettuata!\n" & sNomeErr & "');"
    '                            RegisterScript(sScript , Me.GetType())"msg", "" & sScript & "")
    '                        ElseIf nReturnEstraz < 0 Then
    '                            sScript = "GestAlert('a', 'warning', '', '', 'Estrazione non effettuata!');"
    '                            RegisterScript(sScript , Me.GetType())"msg", "" & sScript & "")
    '                        ElseIf nReturnEstraz = 0 Then
    '                            sScript = "GestAlert('a', 'warning', '', '', 'Si e\' verificato un errore!');"
    '                            RegisterScript(sScript , Me.GetType())"msg", "" & sScript & "")
    '                        Else
    '                            sScript = "GestAlert('a', 'warning', '', '', 'Estrazione 290 terminata con successo!');"
    '                            lblLinkFile290.Text = sNameFile290
    '                            RegisterScript(sScript , Me.GetType())"download", "" + sScript + "")
    '                        End If
    '                    Else
    '                        sScript = "GestAlert('a', 'warning', '', '', 'Non sono state configurate le rate!\nImpossibile proseguire!');"
    '                        RegisterScript(sScript , Me.GetType())"msg", "" & sScript & "")
    '                    End If
    '                Else
    '                    sScript = "GestAlert('a', 'warning', '', '', 'Approvazione Documenti già effettuata!\nImpossibile proseguire!');"
    '                    RegisterScript(sScript , Me.GetType())"msg", "" & sScript & "")
    '                End If
    '            Else
    '                sScript = "GestAlert('a', 'warning', '', '', 'Manca la configurazione del codice ente impositore.\nImpossibile proseguire!');"
    '                RegisterScript(sScript , Me.GetType())"msg", "" & sScript & "")
    '            End If
    '        Else
    '            sScript = "GestAlert('a', 'warning', '', '', 'Selezionare un Ruolo da Elaborare!');"
    '            RegisterScript(sScript , Me.GetType())"msg", "" & sScript & "")
    '        End If
    '        DivAttesa.Style.Add("display", "none")
    '    Catch ex As Exception
    '        Log.Debug("Si è verificato un errore in Fatturazione::CmdEstrazione290_Click::" & ex.Message)
    '    End Try
    'End Sub
    'Private Sub CmdScarica_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdScarica.Click
    '    Response.ContentType = "*/*"
    '    Response.AppendHeader("content-disposition", "attachment; filename=" + lblLinkFile290.Text)
    '    Response.WriteFile(ConfigurationManager.AppSettings("PATH_ESTRAZIONE290").ToString() + lblLinkFile290.Text)
    '    Response.End()
    'End Sub
#End Region
    '*** 20141107 ***
    'Private Sub GrdFatturazioniPrec_UpdateCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GrdFatturazioniPrec.UpdateCommand
    '    'uso questo comando per ristampare la minuta
    'Try
    '    Dim WFSessione As OPENUtility.CreateSessione
    '    Dim WFErrore As String
    '    Dim sPathProspetti, sNameFile, Str As String
    '    Dim oListFatture() As ObjFattura
    '    Dim FunctionFatture As New ClsFatture
    '    Dim sScript As String
    '    Dim x As Integer
    '    Dim FunctionStampa As New ClsStampaXLS
    '    Dim DtDatiStampa As New DataTable
    '    Dim aListColonne As ArrayList
    '    Dim aMyHeaders As String()

    '    Log.Debug("Entro in Fatturazione::GrdFatturazioniPrec_UpdateCommand::" & Now.ToString)
    '    'inizializzo la connessione
    '    WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '    If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '        Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '    End If
    '    'prelevo i ruoli per i parametri inseriti
    '    oListFatture = FunctionFatture.GetFattura(ConstSession.IdEnte, CInt(e.Item.Cells(10).Text), -1, False, WFSessione)
    '    Log.Debug("Sono in Fatturazione::GrdFatturazioniPrec_UpdateCommand:: devo valorizzare il datatable::" & Now.ToString)
    '    'valorizzo il nome del file
    '    sPathProspetti = ConstSession.PathProspetti
    '    sNameFile = ConstSession.DescrizioneEnte & "_" & ConstSession.IdEnte & "_MINUTA_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
    '    DtDatiStampa = FunctionStampa.PrintMinutaRuoloH2O(oListFatture, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, ConstSession.DescrPeriodo)
    '    Log.Debug("Sono in Fatturazione::GrdFatturazioniPrec_UpdateCommand:: ho valorizzato il datatable::" & Now.ToString)
    '    WFSessione.Kill()

    '    If Not DtDatiStampa Is Nothing Then
    '        'definisco le colonne
    '        Log.Debug("Sono in Fatturazione::GrdFatturazioniPrec_UpdateCommand:: definisco le colonne::" & Now.ToString)
    '        aListColonne = New ArrayList
    '        For x = 0 To 38
    '            aListColonne.Add("")
    '        Next
    '        aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

    '        'definisco l'insieme delle colonne da esportare
    '        Dim MyCol() As Integer = New Integer() {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38}
    '        'esporto i dati in excel
    '        Log.Debug("Sono in Fatturazione::GrdFatturazioniPrec_UpdateCommand:: esporto i dati in excel::" & Now.ToString)
    '        Dim MyStampa As New RKLib.ExportData.Export("Web")
    '        MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameFile)
    '        Str = sPathProspetti & sNameFile
    '        Log.Debug("Esco da Fatturazione::GrdFatturazioniPrec_UpdateCommand::" & Now.ToString)
    '    End If
    ' Catch Err As Exception

    '     Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Fatturazione.GrdFatturazioniPrec_UpdateCommand.errore: ", Err)
    '    Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub
#Region "Griglie"
    ''' <summary>
    ''' pulsante per la stampa in formato excel della minuta della riga di ruolo precedente selezionata
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Esportazione completa dati</em>
    ''' </revision>
    ''' </revisionHistory>
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Dim IDRow As String = e.CommandArgument.ToString()
        If e.CommandName = "RowUpdate" Then
            'uso questo comando per ristampare la minuta
            Dim sPathProspetti, sNameXLS As String
            Dim oListFatture() As ObjFattura
            Dim FunctionFatture As New ClsFatture
            Dim x, nCol As Integer
            Dim FunctionStampa As New ClsStampaXLS
            Dim DtDatiStampa As New DataTable
            Dim aListColonne As ArrayList
            Dim aMyHeaders As String()
            For Each myRow As GridViewRow In GrdFatturazioniPrec.Rows
                If IDRow = CType(myRow.FindControl("hfid"), HiddenField).Value Then
                    Log.Debug("Entro in Fatturazione::GrdFatturazioniPrec_UpdateCommand::" & Now.ToString)

                    '' BD 25/06/2021 erano 46
                    nCol = 51
                    '' BD 25/06/2021

                    'prelevo i ruoli per i parametri inseriti
                    oListFatture = FunctionFatture.GetFattura(ConstSession.StringConnection, ConstSession.IdEnte, CInt(CType(myRow.FindControl("hfid"), HiddenField).Value), -1, False)
                    Log.Debug("Sono in Fatturazione::GrdFatturazioniPrec_UpdateCommand:: devo valorizzare il datatable::" & Now.ToString)
                    'valorizzo il nome del file
                    sPathProspetti = ConstSession.PathProspetti
                    sNameXLS = ConstSession.IdEnte & "_MINUTA_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
                    DtDatiStampa = FunctionStampa.PrintMinutaRuoloH2O(oListFatture, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, ConstSession.DescrPeriodo, hdMinutaAnagAllRow.Value, nCol)
                    Log.Debug("Sono in Fatturazione::GrdFatturazioniPrec_UpdateCommand:: ho valorizzato il datatable::" & Now.ToString)
                    'definisco le colonne
                    Log.Debug("Sono in Fatturazione::GrdFatturazioniPrec_UpdateCommand:: definisco le colonne::" & Now.ToString)
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
                    Log.Debug("Sono in Fatturazione::GrdFatturazioniPrec_UpdateCommand:: esporto i dati in excel::" & Now.ToString)
                    Dim MyStampa As New RKLib.ExportData.Export("Web")
                    MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
                    Log.Debug("Esco da Fatturazione::GrdFatturazioniPrec_UpdateCommand::" & Now.ToString)
                End If
            Next
        End If
    End Sub
    'Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
    '    Dim IDRow As String = e.CommandArgument.ToString()
    '    If e.CommandName = "RowUpdate" Then
    '        'uso questo comando per ristampare la minuta
    '        Dim sPathProspetti, sNameXLS, Str As String
    '        Dim oListFatture() As ObjFattura
    '        Dim FunctionFatture As New ClsFatture
    '        Dim x, nCol As Integer
    '        Dim FunctionStampa As New ClsStampaXLS
    '        Dim DtDatiStampa As New DataTable
    '        Dim aListColonne As ArrayList
    '        Dim aMyHeaders As String()
    '        For Each myRow As GridViewRow In GrdFatturazioniPrec.Rows
    '            If IDRow = CType(myRow.FindControl("hfid"), HiddenField).Value Then
    '                Log.Debug("Entro in Fatturazione::GrdFatturazioniPrec_UpdateCommand::" & Now.ToString)
    '                nCol = 46
    '                'prelevo i ruoli per i parametri inseriti
    '                oListFatture = FunctionFatture.GetFattura(ConstSession.IdEnte, CInt(CType(myRow.FindControl("hfid"), HiddenField).Value), -1, False)
    '                Log.Debug("Sono in Fatturazione::GrdFatturazioniPrec_UpdateCommand:: devo valorizzare il datatable::" & Now.ToString)
    '                'valorizzo il nome del file
    '                sPathProspetti = ConstSession.PathProspetti
    '                sNameXLS = ConstSession.IdEnte & "_MINUTA_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
    '                DtDatiStampa = FunctionStampa.PrintMinutaRuoloH2O(oListFatture, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, ConstSession.DescrPeriodo, hdMinutaAnagAllRow.Value, nCol)
    '                Log.Debug("Sono in Fatturazione::GrdFatturazioniPrec_UpdateCommand:: ho valorizzato il datatable::" & Now.ToString)
    '                'definisco le colonne
    '                Log.Debug("Sono in Fatturazione::GrdFatturazioniPrec_UpdateCommand:: definisco le colonne::" & Now.ToString)
    '                aListColonne = New ArrayList
    '                For x = 0 To nCol
    '                    aListColonne.Add("")
    '                Next
    '                aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

    '                'definisco l'insieme delle colonne da esportare
    '                Dim MyCol() As Integer = New Integer(nCol) {}
    '                For x = 0 To nCol
    '                    MyCol(x) = x
    '                Next
    '                'esporto i dati in excel
    '                Log.Debug("Sono in Fatturazione::GrdFatturazioniPrec_UpdateCommand:: esporto i dati in excel::" & Now.ToString)
    '                Dim MyStampa As New RKLib.ExportData.Export("Web")
    '                MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
    '                Str = sPathProspetti & sNameXLS
    '                Log.Debug("Esco da Fatturazione::GrdFatturazioniPrec_UpdateCommand::" & Now.ToString)
    '            End If
    '        Next
    '    End If
    'End Sub
#End Region

    'Private Sub GrdFatturazioniPrec_UpdateCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GrdFatturazioniPrec.UpdateCommand
    '    'uso questo comando per ristampare la minuta
    '    Dim sPathProspetti, sNameFile, Str As String
    '    Dim oListFatture() As ObjFattura
    '    Dim FunctionFatture As New ClsFatture
    '    Dim sScript As String
    '    Dim x, nCol As Integer
    '    Dim FunctionStampa As New ClsStampaXLS
    '    Dim DtDatiStampa As New DataTable
    '    Dim aListColonne As ArrayList
    '    Dim aMyHeaders As String()
    'Try
    '    Log.Debug("Entro in Fatturazione::GrdFatturazioniPrec_UpdateCommand::" & Now.ToString)
    '    nCol = 41
    '    'prelevo i ruoli per i parametri inseriti
    '    oListFatture = FunctionFatture.GetFattura(ConstSession.IdEnte, CInt(e.Item.Cells(10).Text), -1, False)
    '    Log.Debug("Sono in Fatturazione::GrdFatturazioniPrec_UpdateCommand:: devo valorizzare il datatable::" & Now.ToString)
    '    'valorizzo il nome del file
    '    sPathProspetti = ConstSession.PathProspetti
    '    sNameFile = ConstSession.DescrizioneEnte & "_" & ConstSession.IdEnte & "_MINUTA_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
    '    DtDatiStampa = FunctionStampa.PrintMinutaRuoloH2O(oListFatture, ConstSession.IdEnte & "-" & ConstSession.DescrizioneEnte, ConstSession.DescrPeriodo, hdMinutaAnagAllRow.Value, ncol)
    '    Log.Debug("Sono in Fatturazione::GrdFatturazioniPrec_UpdateCommand:: ho valorizzato il datatable::" & Now.ToString)

    '    If Not DtDatiStampa Is Nothing Then
    '        'definisco le colonne
    '        Log.Debug("Sono in Fatturazione::GrdFatturazioniPrec_UpdateCommand:: definisco le colonne::" & Now.ToString)
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
    '        Log.Debug("Sono in Fatturazione::GrdFatturazioniPrec_UpdateCommand:: esporto i dati in excel::" & Now.ToString)
    '        Dim MyStampa As New RKLib.ExportData.Export("Web")
    '        MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameFile)
    '        Str = sPathProspetti & sNameFile
    '        Log.Debug("Esco da Fatturazione::GrdFatturazioniPrec_UpdateCommand::" & Now.ToString)
    '    End If
    'Catch ex As Exception

    '     Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Fatturazione.GrdFatturazioniPrec_UpdateCommand.errore: ", ex)
    '     Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub

    '*** ***
    ''' <summary>
    ''' popolamento delle label di riepilogo dati con i valori dell'oggetto passato
    ''' popolamento griglia con lista di oggetti passata
    ''' </summary>
    ''' <param name="oRuoloH2O">oggetto di tipo ObjTotRuoloFatture</param>
    ''' <param name="oListTotRuolo">lista di oggetti di tipo ObjTotRuoloFatture</param>
    ''' <remarks></remarks>
    Private Sub VisualizzaRiepilogo(ByVal oRuoloH2O As ObjTotRuoloFatture, ByVal oListTotRuolo() As ObjTotRuoloFatture)
        Try
            LblIdFlusso.Text = oRuoloH2O.IdFlusso
            LblImpNegativi.Text = FormatNumber(oRuoloH2O.impNegativi, 2)
            LblImpPositivi.Text = FormatNumber(oRuoloH2O.impPositivi, 2)
            LblNContrib.Text = FormatNumber(oRuoloH2O.nNContribuenti, 0)
            LblNDoc.Text = FormatNumber(oRuoloH2O.nNDocumenti, 0)
            If oRuoloH2O.tDataEmissioneFattura.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                TxtDataFattura.Text = oRuoloH2O.tDataEmissioneFattura.ToShortDateString
            End If
            If oRuoloH2O.tDataScadenza.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                TxtDataScadenza.Text = oRuoloH2O.tDataScadenza.ToShortDateString
            End If
            LblNote.Text = oRuoloH2O.sNote
            LblNote.Font.Bold = True

            'popolo la griglia
            GrdDateRuolo.DataSource = oListTotRuolo
            GrdDateRuolo.DataBind()
            GrdDateRuolo.SelectedIndex = -1
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.Fatturazione.VisualizzaRiepilogo.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Formattazione dell'oggetto passato a data
    ''' </summary>
    ''' <param name="tDataGrd"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FormattaDataGrd(ByVal tDataGrd As DateTime) As String
        If tDataGrd = Date.MinValue Or tDataGrd = Date.MaxValue Then
            Return ""
        Else
            Return tDataGrd.ToShortDateString
        End If
    End Function
End Class
