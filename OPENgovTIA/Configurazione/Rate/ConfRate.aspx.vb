Imports log4net
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
''' <summary>
''' Pagina per la visualizzazione/gestione delle rate.
''' Contiene i parametri di ricerca, le funzioni della comandiera e la griglia per la visualizzazione del risultato.
''' </summary>
Partial Class ConfRate
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ConfRate))
    Private oMyRuolo() As ObjRuolo
    Private FncRate As New GestRata
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
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="04/11/2013">
    ''' TARES
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="08/03/2013">
    ''' devo memorizzare il conto corrente altrimenti non sarà valorizzato nella codeline e nel codice a barre
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim oListRate() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata
        Dim oMyConfigRata As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata
        Dim nList As Integer
        Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
        Dim FncRuolo As New ClsGestRuolo
        Dim sScript As String = ""

        Try
            lblTitolo.Text = ConstSession.DescrizioneEnte
            If ConstSession.IsFromTARES = "1" Then
                info.InnerText = "TARI "
            Else
                info.InnerText = "TARSU "
            End If
            If ConstSession.IsFromVariabile(Request.Item("IsFromVariabile")) = "1" Then
                info.InnerText += "Variabile "
            End If
            info.InnerText += " - Avvisi - Elaborazioni - Configurazione Rate"

            oMyRuolo = Session("oRuoloTIA")
            If Not Page.IsPostBack Then
                If Not IsNothing(oMyRuolo) Then
                    FncRuolo.LoadTipoCalcolo(ConstSession.StringConnection, ConstSession.IdEnte, oMyRuolo(0).sAnno, LblTipoCalcolo, LblTipoMQ, ChkConferimenti, ChkMaggiorazione)
                    oListRate = FncRate.GetRateConfigurate(ConstSession.StringConnection, oMyRuolo(0).IdFlusso, Nothing)
                    If Not IsNothing(oListRate) Then
                        nList = oListRate.GetUpperBound(0) + 1
                        Select Case oListRate(0).sTipoBollettino
                            Case RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata.BOLLETTINO_F24
                                rbF24.Checked = True
                            Case RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata.BOLLETTINO_896
                                rbBollettinoTD896.Checked = True
                            Case Else
                                rbBollettinoTD123.Checked = True
                        End Select
                        TxtSogliaMinima.Text = oListRate(0).impSoglia
                    Else
                        nList = 0
                    End If
                    oMyConfigRata = New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata
                    oMyConfigRata.NumeroRata = ""
                    oMyConfigRata.DescrizioneRata = ""
                    oMyConfigRata.DataScadenza = Date.MinValue
                    oMyConfigRata.Percentuale = 100
                    oMyConfigRata.HasImposta = True
                    oMyConfigRata.HasMaggiorazione = False
                    ReDim Preserve oListRate(nList)
                    oListRate(nList) = oMyConfigRata
                    GrdRate.DataSource = oListRate
                    GrdRate.DataBind()
                    Session("oListRate") = oListRate
                Else
                    rbBollettinoTD123.Checked = True
                End If
            End If
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
            RegisterScript(sScript, Me.GetType)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ConfRate.Page_Load.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        Finally
            'WFSessione.Kill()
        End Try
    End Sub
    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    'Put user code to initialize the page here
    '    Dim oListRate() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata
    '    Dim oMyConfigRata As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata
    '    Dim nList As Integer
    '    Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
    '    Dim FncRuolo As New ClsGestRuolo

    '    Try
    '        lblTitolo.Text = ConstSession.DescrizioneEnte
    '        If ConstSession.IsFromTARES = "1" Then
    '            info.InnerText = "TARI "
    '        Else
    '            info.InnerText = "TARSU "
    '        End If
    '        If ConstSession.IsFromVariabile(Request.Item("IsFromVariabile")) = "1" Then
    '            info.InnerText += "Variabile "
    '        End If
    '        info.InnerText += " - Avvisi - Elaborazioni - Configurazione Rate"

    '        oMyRuolo = Session("oRuoloTIA")
    '        'WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        'If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '        '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        'End If
    '        If Not Page.IsPostBack Then
    '            If Not IsNothing(oMyRuolo) Then
    '                '*** 20131104 - TARES ***
    '                ''*** 20130308 - devo memorizzare il conto corrente altrimenti non sarà valorizzato nella codeline e nel codice a barre ***
    '                'oListRate = FncRate.GetRateConfigurate(oMyRuolo(0).IdFlusso, Nothing, WFSessione)
    '                ''*** ***
    '                FncRuolo.LoadTipoCalcolo(ConstSession.IdEnte, oMyRuolo(0).sAnno, LblTipoCalcolo, LblTipoMQ, ChkConferimenti, ChkMaggiorazione, cmdMyCommand)
    '                oListRate = FncRate.GetRateConfigurate(oMyRuolo(0).IdFlusso, Nothing, cmdMyCommand)
    '                If Not IsNothing(oListRate) Then
    '                    nList = oListRate.GetUpperBound(0) + 1
    '                    Select Case oListRate(0).sTipoBollettino
    '                        Case RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata.BOLLETTINO_F24
    '                            rbF24.Checked = True
    '                        Case RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata.BOLLETTINO_896
    '                            rbBollettinoTD896.Checked = True
    '                        Case Else
    '                            rbBollettinoTD123.Checked = True
    '                    End Select
    '                    TxtSogliaMinima.Text = oListRate(0).impSoglia
    '                Else
    '                    nList = 0
    '                End If
    '                oMyConfigRata = New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata
    '                oMyConfigRata.NumeroRata = ""
    '                oMyConfigRata.DescrizioneRata = ""
    '                oMyConfigRata.DataScadenza = Date.MinValue
    '                oMyConfigRata.Percentuale = 100
    '                oMyConfigRata.HasImposta = True
    '                oMyConfigRata.HasMaggiorazione = False
    '                ReDim Preserve oListRate(nList)
    '                oListRate(nList) = oMyConfigRata
    '                '*** ***
    '                GrdRate.DataSource = oListRate
    '                GrdRate.DataBind()
    '                Session("oListRate") = oListRate
    '            Else
    '                rbBollettinoTD123.Checked = True
    '            End If
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ConfRate.Page_Load.errore: ", Err)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    Finally
    '        'WFSessione.Kill()
    '    End Try
    'End Sub

    Private Sub CmdSalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdSalva.Click
        '*** 20131104 - TARES ***
        Dim sScript, sTipoBollettino As String
        Dim oListRate() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata = Nothing
        Dim oMyConfigRata As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata
        Dim x As Integer = 0

        Try
            'salvo la soglia minima
            If Not IsNumeric(TxtSogliaMinima.Text) Then
                sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario valorizzare la soglia minima!');"
                RegisterScript(sScript, Me.GetType)
                Exit Sub
            Else
                'aggiorno la data di creazione minuta sul db
                If New ClsGestRuolo().UpdateDateRuolo(oMyRuolo, 3, "c") = False Then
                    Response.Redirect("../../../PaginaErrore.aspx")
                End If
                If New ClsGestRuolo().UpdateDateRuolo(oMyRuolo, 4, "c") = False Then
                    Response.Redirect("../../../PaginaErrore.aspx")
                End If
                Session("oRuoloTIA") = oMyRuolo
            End If

            If rbF24.Checked Then
                sTipoBollettino = RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata.BOLLETTINO_F24
            ElseIf rbBollettinoTD896.Checked Then
                sTipoBollettino = RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata.BOLLETTINO_896
            Else
                sTipoBollettino = RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata.BOLLETTINO_123
            End If
            For Each myRow As GridViewRow In GrdRate.Rows
                If CType(myRow.FindControl("txtNRata"), TextBox).Text <> "" Then
                    'ciclo sulla griglia per popolarmi i dati da inserire
                    oMyConfigRata = New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata
                    oMyConfigRata.NumeroRata = CType(myRow.FindControl("txtNRata"), TextBox).Text.ToUpper
                    oMyConfigRata.DescrizioneRata = CType(myRow.FindControl("txtDescrRata"), TextBox).Text.ToUpper
                    oMyConfigRata.DataScadenza = CType(myRow.FindControl("txtDataScadenza"), TextBox).Text
                    oMyConfigRata.Percentuale = CType(myRow.FindControl("txtPercentuale"), TextBox).Text
                    oMyConfigRata.HasImposta = CType(myRow.FindControl("ChkImposta"), CheckBox).Checked
                    oMyConfigRata.HasMaggiorazione = CType(myRow.FindControl("ChkMaggiorazione"), CheckBox).Checked
                    oMyConfigRata.sTipoBollettino = sTipoBollettino
                    oMyConfigRata.impSoglia = TxtSogliaMinima.Text
                    'se sono unica soluzione viene forzato
                    If oMyConfigRata.NumeroRata = "U" Then
                        oMyConfigRata.Percentuale = 100
                        oMyConfigRata.HasImposta = True
                        If oMyRuolo(0).HasMaggiorazione Then 'se si prevede maggiorazione viene forzato il flag
                            oMyConfigRata.HasMaggiorazione = True
                        End If
                    End If
                    ReDim Preserve oListRate(x)
                    oListRate(x) = oMyConfigRata
                    x += 1
                End If
            Next
            'se il ruolo prevede la maggiorazione devo avere almeno una rata con il flag e la somma delle percentuali deve essere 100
            Dim CheckMagg As Boolean = False
            If oMyRuolo(0).HasMaggiorazione Then
                For x = 0 To oListRate.GetUpperBound(0)
                    If oListRate(x).HasMaggiorazione = True Then
                        CheckMagg = True
                    End If
                Next
            End If
            If CheckMagg = False And ChkMaggiorazione.Checked = True Then
                sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario valorizzare il flag maggiorazione almeno su una rata!');"
                RegisterScript(sScript, Me.GetType)
                Exit Sub
            End If
            Dim TotPercent As Integer = 0
            For x = 0 To oListRate.GetUpperBound(0)
                If oListRate(x).NumeroRata <> "U" Or oListRate.GetUpperBound(0) = 0 Then
                    TotPercent += oListRate(x).Percentuale
                End If
            Next
            If TotPercent <> 100 Then
                sScript = "GestAlert('a', 'warning', '', '', 'La somma delle percentuali deve essere 100!');"
                RegisterScript(sScript, Me.GetType)
                Exit Sub
            End If
            'elimino le rate precedentemente inserite
            If FncRate.SetRataConfigurata(ConstSession.StringConnection, Nothing, oMyRuolo(0).IdFlusso, Utility.Costanti.AZIONE_DELETE, ConstSession.UserName) < 0 Then
                sScript = "GestAlert('a', 'danger', '', '', 'Errore in inserimento rate!');"
                RegisterScript(sScript, Me.GetType)
                Exit Sub
            End If
            'inserisco le rate
            For x = 0 To oListRate.GetUpperBound(0)
                If FncRate.SetRataConfigurata(ConstSession.StringConnection, oListRate(x), oMyRuolo(0).IdFlusso, Utility.Costanti.AZIONE_NEW, ConstSession.UserName) < 1 Then
                    sScript = "GestAlert('a', 'danger', '', '', 'Errore in inserimento rate!');"
                    RegisterScript(sScript, Me.GetType)
                    Exit Sub
                End If
            Next
            Session("oListRate") = Nothing
            sScript = "GestAlert('a', 'success', '', '', 'Inserimento effettuato correttamente!');"
            sScript += "location.href='../../Avvisi/Calcolo/Calcolo.aspx?IsFromVariabile=" + ConstSession.IsFromVariabile(Request.Item("IsFromVariabile")) + "';"
            RegisterScript(sScript, Me.GetType)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ConfRate.CmdSalva_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
        '*** ***
    End Sub
    'Private Sub CmdSalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdSalva.Click
    '    '*** 20131104 - TARES ***
    '    Dim sScript, sTipoBollettino As String
    '    Dim oListRate() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata = Nothing
    '    Dim oMyConfigRata As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata
    '    Dim x As Integer = 0
    '    'Dim WFErrore As String=""
    '    'Dim WFSessione As OPENUtility.CreateSessione
    '    'Dim MyDBEngine As DBEngine = Nothing
    '    Dim cmdMyCommand As New SqlClient.SqlCommand

    '    Try
    '        'MyDBEngine = Utility.DBEngineFactory.GetDBEngine(ConstSession.StringConnection)
    '        'MyDBEngine.OpenConnection()
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0

    '        'WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        'If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '        '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        'End If

    '        'salvo la soglia minima
    '        If Not IsNumeric(TxtSogliaMinima.Text) Then
    '            sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario valorizzare la soglia minima!');"
    '            RegisterScript(sScript, Me.GetType)
    '            Exit Sub
    '        Else
    '            Dim MyFnc As New ClsGestRuolo
    '            oMyRuolo(0).tDataCartellazione = Date.MaxValue
    '            oMyRuolo(0).tDataElabDoc = Date.MaxValue
    '            oMyRuolo(0).tDataOKDoc = Date.MaxValue
    '            If MyFnc.SetRuolo(Utility.Costanti.AZIONE_UPDATE, oMyRuolo(0), ConstSession.StringConnection) <= 0 Then
    '                sScript = "GestAlert('a', 'danger', '', '', 'Errore in inserimento rate!');"
    '                RegisterScript(sScript, Me.GetType)
    '                Exit Sub
    '            End If
    '        End If

    '        If rbF24.Checked Then
    '            sTipoBollettino = RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata.BOLLETTINO_F24
    '        ElseIf rbBollettinoTD896.Checked Then
    '            sTipoBollettino = RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata.BOLLETTINO_896
    '        Else
    '            sTipoBollettino = RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata.BOLLETTINO_123
    '        End If
    '        For Each myRow As GridViewRow In GrdRate.Rows
    '            If CType(myRow.FindControl("txtNRata"), TextBox).Text <> "" Then
    '                'ciclo sulla griglia per popolarmi i dati da inserire
    '                oMyConfigRata = New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata
    '                oMyConfigRata.NumeroRata = CType(myRow.FindControl("txtNRata"), TextBox).Text.ToUpper
    '                oMyConfigRata.DescrizioneRata = CType(myRow.FindControl("txtDescrRata"), TextBox).Text.ToUpper
    '                oMyConfigRata.DataScadenza = CType(myRow.FindControl("txtDataScadenza"), TextBox).Text
    '                oMyConfigRata.Percentuale = CType(myRow.FindControl("txtPercentuale"), TextBox).Text
    '                oMyConfigRata.HasImposta = CType(myRow.FindControl("ChkImposta"), CheckBox).Checked
    '                oMyConfigRata.HasMaggiorazione = CType(myRow.FindControl("ChkMaggiorazione"), CheckBox).Checked
    '                oMyConfigRata.sTipoBollettino = sTipoBollettino
    '                oMyConfigRata.impSoglia = TxtSogliaMinima.Text
    '                'se sono unica soluzione viene forzato
    '                If oMyConfigRata.NumeroRata = "U" Then
    '                    oMyConfigRata.Percentuale = 100
    '                    oMyConfigRata.HasImposta = True
    '                    If oMyRuolo(0).HasMaggiorazione Then 'se si prevede maggiorazione viene forzato il flag
    '                        oMyConfigRata.HasMaggiorazione = True
    '                    End If
    '                End If
    '                ReDim Preserve oListRate(x)
    '                oListRate(x) = oMyConfigRata
    '                x += 1
    '            End If
    '        Next
    '        'For x = 0 To GrdRate.Items.Count - 1
    '        '    If CType(GrdRate.Items(x).Cells(0).FindControl("txtNRata"), TextBox).Text <> "" Then
    '        '        'ciclo sulla griglia per popolarmi i dati da inserire
    '        '        oMyConfigRata = New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata
    '        '        oMyConfigRata.NumeroRata = CType(GrdRate.Items(x).Cells(0).FindControl("txtNRata"), TextBox).Text.ToUpper
    '        '        oMyConfigRata.DescrizioneRata = CType(GrdRate.Items(x).Cells(1).FindControl("txtDescrRata"), TextBox).Text.ToUpper
    '        '        oMyConfigRata.DataScadenza = CType(GrdRate.Items(x).Cells(2).FindControl("txtDataScadenza"), TextBox).Text
    '        '        oMyConfigRata.Percentuale = CType(GrdRate.Items(x).Cells(3).FindControl("txtPercentuale"), TextBox).Text
    '        '        oMyConfigRata.HasImposta = CType(GrdRate.Items(x).Cells(4).FindControl("ChkImposta"), CheckBox).Checked
    '        '        oMyConfigRata.HasMaggiorazione = CType(GrdRate.Items(x).Cells(5).FindControl("ChkMaggiorazione"), CheckBox).Checked
    '        '        oMyConfigRata.sTipoBollettino = sTipoBollettino
    '        '        oMyConfigRata.impSoglia = TxtSogliaMinima.Text
    '        '        'se sono unica soluzione viene forzato
    '        '        If oMyConfigRata.NumeroRata = "U" Then
    '        '            oMyConfigRata.Percentuale = 100
    '        '            oMyConfigRata.HasImposta = True
    '        '            If oMyRuolo(0).HasMaggiorazione Then 'se si prevede maggiorazione viene forzato il flag
    '        '                oMyConfigRata.HasMaggiorazione = True
    '        '            End If
    '        '        End If
    '        '        ReDim Preserve oListRate(x)
    '        '        oListRate(x) = oMyConfigRata
    '        '    End If
    '        'Next
    '        'se il ruolo prevede la maggiorazione devo avere almeno una rata con il flag e la somma delle percentuali deve essere 100
    '        Dim CheckMagg As Boolean = False
    '        If oMyRuolo(0).HasMaggiorazione Then
    '            For x = 0 To oListRate.GetUpperBound(0)
    '                If oListRate(x).HasMaggiorazione = True Then
    '                    CheckMagg = True
    '                End If
    '            Next
    '        End If
    '        If CheckMagg = False And ChkMaggiorazione.Checked = True Then
    '            sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario valorizzare il flag maggiorazione almeno su una rata!');"
    '            RegisterScript(sScript, Me.GetType)
    '            Exit Sub
    '        End If
    '        Dim TotPercent As Integer = 0
    '        For x = 0 To oListRate.GetUpperBound(0)
    '            If oListRate(x).NumeroRata <> "U" Or oListRate.GetUpperBound(0) = 0 Then
    '                TotPercent += oListRate(x).Percentuale
    '            End If
    '        Next
    '        If TotPercent <> 100 Then
    '            sScript = "GestAlert('a', 'warning', '', '', 'La somma delle percentuali deve essere 100!');"
    '            RegisterScript(sScript, Me.GetType)
    '            Exit Sub
    '        End If
    '        'elimino le rate precedentemente inserite
    '        'If FncRate.SetRataConfigurata(Nothing, oMyRuolo(0).IdFlusso, Costanti.AZIONE_DELETE, Session("username"), WFSessione) = False Then
    '        If FncRate.SetRataConfigurata(Nothing, oMyRuolo(0).IdFlusso, Utility.Costanti.AZIONE_DELETE, ConstSession.UserName, cmdMyCommand) < 0 Then
    '            sScript = "GestAlert('a', 'danger', '', '', 'Errore in inserimento rate!');"
    '            RegisterScript(sScript, Me.GetType)
    '            Exit Sub
    '        End If
    '        'inserisco le rate
    '        For x = 0 To oListRate.GetUpperBound(0)
    '            'If FncRate.SetRataConfigurata(oListRate(x), oMyRuolo(0).IdFlusso, Costanti.AZIONE_NEW, Session("username"), WFSessione) = False Then
    '            If FncRate.SetRataConfigurata(oListRate(x), oMyRuolo(0).IdFlusso, Utility.Costanti.AZIONE_NEW, ConstSession.UserName, cmdMyCommand) < 1 Then
    '                sScript = "GestAlert('a', 'danger', '', '', 'Errore in inserimento rate!');"
    '                RegisterScript(sScript, Me.GetType)
    '                Exit Sub
    '            End If
    '        Next
    '        Session("oListRate") = Nothing
    '        sScript = "GestAlert('a', 'success', '', '', 'Inserimento effettuato correttamente!');"
    '        'sScript += "parent.Comandi.location.href='../../Avvisi/Calcolo/ComandiCalcolo.aspx';parent.Visualizza.location.href='../../Avvisi/Calcolo/Calcolo.aspx';"
    '        'sScript += "parent.Visualizza.location.href='../../Avvisi/Calcolo/Calcolo.aspx';"
    '        sScript += "location.href='../../Avvisi/Calcolo/Calcolo.aspx?IsFromVariabile=" + ConstSession.IsFromVariabile(Request.Item("IsFromVariabile")) + "';"
    '        RegisterScript(sScript, Me.GetType)
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTIA.ConfRate.CmdSalva_Click.errore: ", Err)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    Finally
    '        'WFSessione.Kill()
    '        'MyDBEngine.CloseConnection()
    '        cmdMyCommand.Dispose()
    '        cmdMyCommand.Connection.Close()
    '    End Try
    '    '*** ***
    'End Sub

#Region "Griglie"
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim IDRow As String = e.CommandArgument.ToString()
            If e.CommandName = "RowOpen" Then
                Dim nList As Integer
                Dim sScript As String
                Dim oListRate() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata
                Dim oMyConfigRata As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata

                Try
                    For Each myRow As GridViewRow In GrdRate.Rows
                        If CType(myRow.FindControl("txtNRata"), TextBox).Text = IDRow Then
                            If GrdRate.SelectedIndex <> -1 Then
                                oListRate = Session("oListRate")
                                If CType(myRow.FindControl("txtNRata"), TextBox).Text <> "" Or CType(myRow.FindControl("txtDescrRata"), TextBox).Text <> "" Or CType(myRow.FindControl("txtDataScadenza"), TextBox).Text <> "" Then
                                    If CType(myRow.FindControl("txtNRata"), TextBox).Text = "" Then
                                        sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario valorizzare il numero rata!');"
                                        RegisterScript(sScript, Me.GetType)
                                        Exit Sub
                                    End If
                                    If CType(myRow.FindControl("txtDescrRata"), TextBox).Text = "" Then
                                        sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario valorizzare la descrizione della rata!');"
                                        RegisterScript(sScript, Me.GetType)
                                        Exit Sub
                                    End If
                                    If CType(myRow.FindControl("txtDataScadenza"), TextBox).Text = "" Then
                                        sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario valorizzare la data di scadenza della rata!');"
                                        RegisterScript(sScript, Me.GetType)
                                        Exit Sub
                                    End If
                                    '*** 20131104 - TARES ***
                                    If CType(myRow.FindControl("txtPercentuale"), TextBox).Text = "" Or CType(myRow.FindControl("txtPercentuale"), TextBox).Text = "0" Then
                                        sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario valorizzare la precentuale della rata!');"
                                        RegisterScript(sScript, Me.GetType)
                                        Exit Sub
                                    End If
                                    If CType(myRow.FindControl("ChkImposta"), CheckBox).Checked = False And CType(myRow.FindControl("ChkMaggiorazione"), CheckBox).Checked = False Then
                                        sScript = "GestAlert('a', 'warning', '', '', 'E\' necessario valorizzare o il flag imposta e/o il flag maggiorazione della rata!');"
                                        RegisterScript(sScript, Me.GetType)
                                        Exit Sub
                                    End If
                                    '*** ***
                                    'ciclo sulla griglia per popolarmi i dati da inserire
                                    nList = oListRate.GetUpperBound(0) - 1
                                    oMyConfigRata = New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata
                                    oMyConfigRata.NumeroRata = CType(myRow.FindControl("txtNRata"), TextBox).Text
                                    oMyConfigRata.DescrizioneRata = CType(myRow.FindControl("txtDescrRata"), TextBox).Text
                                    oMyConfigRata.DataScadenza = CType(myRow.FindControl("txtDataScadenza"), TextBox).Text
                                    '*** 20131104 - TARES ***
                                    oMyConfigRata.Percentuale = CType(myRow.FindControl("txtPercentuale"), TextBox).Text
                                    oMyConfigRata.HasImposta = CType(myRow.FindControl("ChkImposta"), CheckBox).Checked
                                    oMyConfigRata.HasMaggiorazione = CType(myRow.FindControl("ChkMaggiorazione"), CheckBox).Checked
                                    '*** ***
                                    ReDim Preserve oListRate(nList)
                                    oListRate(nList) = oMyConfigRata

                                    oMyConfigRata = New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata
                                    oMyConfigRata.NumeroRata = ""
                                    oMyConfigRata.DescrizioneRata = ""
                                    oMyConfigRata.DataScadenza = Date.MinValue
                                    ReDim Preserve oListRate(nList + 1)
                                    oListRate(nList + 1) = oMyConfigRata

                                    Session("oListRate") = oListRate
                                End If
                            End If
                        End If
                    Next
                Catch Err As Exception
                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ConfRate.GrdRowCommand.errore: ", Err)
                End Try
            ElseIf e.CommandName = "RowNew" Then
                Dim oListRate() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata
                Dim oMyConfigRata As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata
                Dim nList As Integer

                Try
                    oListRate = Session("oListRate")
                    nList = oListRate.GetUpperBound(0) + 1

                    oMyConfigRata = New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata
                    oMyConfigRata.NumeroRata = ""
                    oMyConfigRata.DescrizioneRata = ""
                    oMyConfigRata.DataScadenza = Date.MinValue
                    '*** 20131104 - TARES ***
                    oMyConfigRata.Percentuale = 100
                    oMyConfigRata.HasImposta = True
                    oMyConfigRata.HasMaggiorazione = False
                    '*** ***
                    ReDim Preserve oListRate(nList)
                    oListRate(nList) = oMyConfigRata

                    GrdRate.DataSource = oListRate
                    GrdRate.DataBind()
                    Session("oListRate") = oListRate
                Catch Err As Exception
                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ConfRate.GrdRowCommand.errore: ", Err)
                End Try
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ConfRate.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub GrdRate_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GrdRate.SelectedIndexChanged
    '    Dim oListRate() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata
    '    Dim oMyConfigRata As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata
    '    Dim nList As Integer

    '    Try
    '        oListRate = Session("oListRate")
    '        nList = oListRate.GetUpperBound(0) + 1

    '        oMyConfigRata = New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata
    '        oMyConfigRata.NumeroRata = ""
    '        oMyConfigRata.DescrizioneRata = ""
    '        oMyConfigRata.DataScadenza = Date.MinValue
    '        '*** 20131104 - TARES ***
    '        oMyConfigRata.Percentuale = 100
    '        oMyConfigRata.HasImposta = True
    '        oMyConfigRata.HasMaggiorazione = False
    '        '*** ***
    '        ReDim Preserve oListRate(nList)
    '        oListRate(nList) = oMyConfigRata

    '        GrdRate.DataSource = oListRate
    '        GrdRate.DataBind()
    '        Session("oListRate") = oListRate
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ConfRate.GrdRate_SelectedIndexChanged.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Private Sub GrdRate_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GrdRate.ItemCommand
    '    Dim nList As Integer
    '    Dim sScript As String
    '    Dim oListRate() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata
    '    Dim oMyConfigRata As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata

    '    Try
    '        If GrdRate.SelectedIndex <> -1 Then
    '            oListRate = Session("oListRate")
    '            If CType(GrdRate.Items(GrdRate.SelectedIndex).Cells(0).FindControl("txtNRata"), TextBox).Text <> "" Or CType(GrdRate.Items(GrdRate.SelectedIndex).Cells(1).FindControl("txtDescrRata"), TextBox).Text <> "" Or CType(GrdRate.Items(GrdRate.SelectedIndex).Cells(2).FindControl("txtDataScadenza"), TextBox).Text <> "" Then
    '                If CType(GrdRate.Items(GrdRate.SelectedIndex).Cells(0).FindControl("txtNRata"), TextBox).Text = "" Then
    '                    sScript = "alert('E\' necessario valorizzare il numero rata!');"
    '                   RegisterScript( sScript,Me.GetType)
    '                    Exit Sub
    '                End If
    '                If CType(GrdRate.Items(GrdRate.SelectedIndex).Cells(1).FindControl("txtDescrRata"), TextBox).Text = "" Then
    '                    sScript = "alert('E\' necessario valorizzare la descrizione della rata!');"
    '                   RegisterScript( sScript,Me.GetType)
    '                    Exit Sub
    '                End If
    '                If CType(GrdRate.Items(GrdRate.SelectedIndex).Cells(2).FindControl("txtDataScadenza"), TextBox).Text = "" Then
    '                    sScript = "alert('E\' necessario valorizzare la data di scadenza della rata!');"
    '                   RegisterScript( sScript,Me.GetType)
    '                    Exit Sub
    '                End If
    '                '*** 20131104 - TARES ***
    '                If CType(GrdRate.Items(GrdRate.SelectedIndex).Cells(3).FindControl("txtPercentuale"), TextBox).Text = "" Or CType(GrdRate.Items(GrdRate.SelectedIndex).Cells(3).FindControl("txtPercentuale"), TextBox).Text = "0" Then
    '                    sScript = "alert('E\' necessario valorizzare la precentuale della rata!');"
    '                   RegisterScript( sScript,Me.GetType)
    '                    Exit Sub
    '                End If
    '                If CType(GrdRate.Items(GrdRate.SelectedIndex).Cells(4).FindControl("ChkImposta"), CheckBox).Checked = False And CType(GrdRate.Items(GrdRate.SelectedIndex).Cells(5).FindControl("ChkMaggiorazione"), CheckBox).Checked = False Then
    '                    sScript = "alert('E\' necessario valorizzare o il flag imposta e/o il flag maggiorazione della rata!');"
    '                   RegisterScript( sScript,Me.GetType)
    '                    Exit Sub
    '                End If
    '                '*** ***
    '                'ciclo sulla griglia per popolarmi i dati da inserire
    '                nList = oListRate.GetUpperBound(0) - 1
    '                oMyConfigRata = New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata
    '                oMyConfigRata.NumeroRata = CType(GrdRate.Items(GrdRate.SelectedIndex).Cells(0).FindControl("txtNRata"), TextBox).Text
    '                oMyConfigRata.DescrizioneRata = CType(GrdRate.Items(GrdRate.SelectedIndex).Cells(1).FindControl("txtDescrRata"), TextBox).Text
    '                oMyConfigRata.DataScadenza = CType(GrdRate.Items(GrdRate.SelectedIndex).Cells(2).FindControl("txtDataScadenza"), TextBox).Text
    '                '*** 20131104 - TARES ***
    '                oMyConfigRata.Percentuale = CType(GrdRate.Items(GrdRate.SelectedIndex).Cells(3).FindControl("txtPercentuale"), TextBox).Text
    '                oMyConfigRata.HasImposta = CType(GrdRate.Items(GrdRate.SelectedIndex).Cells(4).FindControl("ChkImposta"), CheckBox).Checked
    '                oMyConfigRata.HasMaggiorazione = CType(GrdRate.Items(GrdRate.SelectedIndex).Cells(5).FindControl("ChkMaggiorazione"), CheckBox).Checked
    '                '*** ***
    '                ReDim Preserve oListRate(nList)
    '                oListRate(nList) = oMyConfigRata

    '                oMyConfigRata = New RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata
    '                oMyConfigRata.NumeroRata = ""
    '                oMyConfigRata.DescrizioneRata = ""
    '                oMyConfigRata.DataScadenza = Date.MinValue
    '                ReDim Preserve oListRate(nList + 1)
    '                oListRate(nList + 1) = oMyConfigRata

    '                Session("oListRate") = oListRate
    '            End If
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ConfRate.GrdRate_ItemCommand.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    End Try
    'End Sub
#End Region

    'Public Function FormattaDataGrd(ByVal tDataGrd As DateTime) As String
    'Try
    '    If tDataGrd = Date.MinValue Then
    '        Return ""
    '    Else
    '        Return tDataGrd.ToShortDateString
    '    End If
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ConfRate.FormattaDataGrd.errore: ", ex)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    End Try
    'End Function
End Class
