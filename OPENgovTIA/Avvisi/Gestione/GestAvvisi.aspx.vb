Imports log4net
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
''' <summary>
''' Pagina per la visualizzazione e gestione dell'avviso ordinario
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
''' <revisionHistory>
''' <revision date="12/04/2019">
''' <strong>Qualificazione AgID-analisi_rel01</strong>
''' <em>Analisi eventi</em>
''' </revision>
''' </revisionHistory>
Partial Class GestAvvisi
    Inherits BaseEnte
    Private Shared Log As ILog = LogManager.GetLogger(GetType(GestAvvisi))
    Private sScript As String
    Private oMyAvviso As New ObjAvviso
    Private FncAvviso As New GestAvviso
    Protected FncGrd As New Formatta.FunctionGrd

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Label10 As System.Web.UI.WebControls.Label
    Protected WithEvents LnkAnagTributi As System.Web.UI.WebControls.ImageButton
    Protected WithEvents LnkAnagAnater As System.Web.UI.WebControls.ImageButton
    Protected WithEvents LnkPulisciContr As System.Web.UI.WebControls.ImageButton
    Protected WithEvents lblCognome As System.Web.UI.WebControls.Label
    Protected WithEvents lblNome As System.Web.UI.WebControls.Label
    Protected WithEvents lblDataNascita As System.Web.UI.WebControls.Label
    Protected WithEvents lblIndirizzo As System.Web.UI.WebControls.Label
    Protected WithEvents lblComune As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
        Try
            If ConstSession.IsFromVariabile(Request.Item("IsFromVariabile")) = "1" Then
            End If
            lblTitolo.Text = ConstSession.DescrizioneEnte
            If ConstSession.IsFromTARES = "1" Then
                info.InnerText = "TARI"
            Else
                info.InnerText = "TARSU"
            End If
            If ConstSession.IsFromVariabile() = "1" Then
                info.InnerText += "Variabile"
            End If
            info.InnerText += " - Avvisi - Gestione"
            '*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI & DIVERSA GESTIONE QUOTA VARIABILE***
            If ConstSession.HasPlainAnag Then
                sScript += "document.getElementById('TRSpecAnag').style.display='none';"
            Else
                sScript += "document.getElementById('TRPlainAnag').style.display='none';"
            End If
            sScript += "parent.Comandi.location.href = '../../../aspVuotaRemoveComandi.aspx';"
            sScript += "parent.Basso.location.href = '../../../aspVuotaRemoveComandi.aspx';"
            sScript += "parent.Nascosto.location.href = '../../../aspVuotaRemoveComandi.aspx';"
            RegisterScript(sScript, Me.GetType)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAvvisi.Page_Init.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub

#End Region
    ''' <summary>
    ''' Al caricamento della pagina vengono caricati, leggendo da banca dati, i dati dell'avviso sul quale si è entrati.
    ''' I pulsanti vengono abilitati/disabilitati in base alle casistiche:
    ''' - Avviso già sgravato: pulsante elimina abilitato.
    ''' - Avviso su ruolo non ancora approvato: pulsante elimina abilitato.
    ''' - Avviso su ruolo caricato da flusso: pulsante elimina disabilitato, pulsante sgravio disabilitato, pulsante ristampa disabilitato.
    ''' </summary>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim oListAvvisi() As ObjAvviso
        Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
        Dim fncRuolo As New ClsGestRuolo

        Try
            DivTessere.Style.Add("display", "none")
            DivScaglioni.Style.Add("display", "none")
            If Not Page.IsPostBack Then
                Try
                    cmdMyCommand = New SqlClient.SqlCommand
                    cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
                    cmdMyCommand.Connection.Open()
                    cmdMyCommand.CommandTimeout = 0
                    'controllo se sono in visualizzazione
                    Log.Debug("GestAvvisi::Page_Load::devo caricare l'avviso")
                    If Request.Item("AzioneProv") = Utility.Costanti.AZIONE_UPDATE Then
                        oListAvvisi = FncAvviso.GetAvviso(ConstSession.StringConnection, Request.Item("IdUniqueAvviso"), ConstSession.IdEnte, Request.Item("IdRuolo"), "", "", "", "", "", "", "", "", "", True, False, False, False, "", -1, cmdMyCommand)
                        If Not IsNothing(oListAvvisi) Then
                            Log.Debug("GestAvvisi::Page_Load::avviso caricato")
                            oMyAvviso = oListAvvisi(0)
                            fncRuolo.LoadTipoCalcolo(ConstSession.StringConnection, ConstSession.IdEnte, oMyAvviso.sAnnoRiferimento, LblTipoCalcolo, LblTipoMQ, ChkConferimenti, ChkMaggiorazione)
                            '*** 20120917 - sgravi ***
                            'controllo se l'avviso è sgravato
                            Try
                                If FncAvviso.GetAvvisoSgravato(oMyAvviso.sCodiceCartella, cmdMyCommand) = 1 Then
                                    LblSgravio.Visible = True
                                    'sScript = "parent.Comandi.document.getElementById('Delete').style.display='';"
                                    sScript = "$('#Delete').removeClass('DisableBtn');"
                                Else
                                    LblSgravio.Visible = False
                                    If Not IsNothing(Session("oRuoloTIA")) Then
                                        If CType(Session("oRuoloTIA"), ObjRuolo())(0).tDataOKDoc.ToShortDateString <> DateTime.MaxValue.ToShortDateString And CType(Session("oRuoloTIA"), ObjRuolo())(0).tDataOKDoc.ToShortDateString <> DateTime.MinValue.ToShortDateString Then
                                            'sScript = "parent.Comandi.document.getElementById('Delete').style.display='none';"
                                            sScript = "$('#Delete').addClass('DisableBtn');"
                                        Else
                                            'sScript = "parent.Comandi.document.getElementById('Delete').style.display='';"
                                            sScript = "$('#Delete').removeClass('DisableBtn');"
                                        End If
                                    Else
                                        'sScript = "parent.Comandi.document.getElementById('Delete').style.display='none';"
                                        sScript = "$('#Delete').addClass('DisableBtn');"
                                    End If
                                End If
                                RegisterScript(sScript, Me.GetType)
                            Catch ex As Exception
                                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAvvisi.Page_Load.LoadAvvisoSgravato.errore :   ", ex)
                            End Try
                            '*** ***
                            '*** 201809 Bollettazione Vigliano in OPENgov***
                            Try
                                Dim oMyTotRuolo() As ObjRuolo
                                'oMyTotRuolo = fncRuolo.GetDatiPerRuolo(ConstSession.IdEnte, Request.Item("IdRuolo"), "", "", cmdMyCommand)
                                oMyTotRuolo = fncRuolo.GetRuolo(ConstSession.StringConnection, ConstSession.IdEnte, oMyAvviso.IdFlussoRuolo, "", "", 1, False, Nothing)
                                bp_TipoRuolo = oMyTotRuolo(0).sTipoRuolo
                                If oMyTotRuolo(0).TipoGenerazione = ObjRuolo.Generazione.DaFlusso Then
                                    LblSgravio.Visible = False
                                    'sScript = "parent.Comandi.document.getElementById('Delete').style.display='none';"
                                    'sScript += "parent.Comandi.document.getElementById('BloccaSgravio').style.display='none';"
                                    'sScript += "parent.Comandi.document.getElementById('ElaborazioneDocumenti').style.display='none';"
                                    sScript = "$('#Delete').addClass('DisableBtn');"
                                    sScript += "$('#BloccaSgravio').addClass('DisableBtn');"
                                    sScript += "$('#ElaborazioneDocumenti').addClass('DisableBtn');"
                                    RegisterScript(sScript, Me.GetType)
                                End If
                            Catch ex As Exception
                                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAvvisi.Page_Load.LoadRuoloCalcolato.errore :   ", ex)
                            End Try
                            Session.Add("oMyAvviso", oMyAvviso)
                            Session("oListTessere") = oMyAvviso.oTessere
                            Session("oListScaglioni") = oMyAvviso.oScaglioni
                            Session("oListArticoli") = oMyAvviso.oArticoli
                            Session("oListDetVoci") = oMyAvviso.oDetVoci
                            Session("oListRate") = oMyAvviso.oRate
                        Else
                            Log.Debug("GestAvvisi::Page_Load::avviso vuoto")
                        End If
                    Else
                        fncRuolo.LoadTipoCalcolo(ConstSession.StringConnection, ConstSession.IdEnte, Now.Year, LblTipoCalcolo, LblTipoMQ, ChkConferimenti, ChkMaggiorazione)
                        Log.Debug("GestAvvisi::Page_Load::nuovo inserimento")
                    End If
                    If LoadDati() = False Then
                        Response.Redirect("../../../PaginaErrore.aspx")
                    End If
                Catch ex As Exception
                    Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAvvisi.Page_Load.errore: ", ex)
                    Response.Redirect("../../../PaginaErrore.aspx")
                Finally
                    cmdMyCommand.Dispose()
                    cmdMyCommand.Connection.Close()
                End Try
                Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Sgravio, "Gestione", Utility.Costanti.AZIONE_LETTURA.ToString, ConstSession.CodTributo, ConstSession.IdEnte, oMyAvviso.ID)
            End If
            Label1.Text = "Dati Parte " & LblTipoCalcolo.Text
            '*** ***
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAvvisi.Page_Load.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub

    '*** 20120917 - sgravi ***
    ''' <summary>
    ''' Pulsante per l'attivazione/conferma di una modifica di avviso.
    ''' Se è presente la data di accertamento la posizione non può essere modificata; messaggio di blocco.
    ''' Se è la prima volta che si clicca il pulsante viene inserito il blocco della posizione.
    ''' Se il blocco della posizione è già attivo, viene ricalcolato l'avviso, ricaricata la pagina, tolto il blocco alla posizione.
    ''' </summary>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Analisi eventi</em>
    ''' </revision>
    ''' </revisionHistory>
    Private Sub CmdSgravi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdSgravi.Click
        Dim myIdentity As Integer
        Dim sMyErr As String = ""
        Dim cmdMyCommand As SqlClient.SqlCommand = Nothing

        Try
            If Not Session("BloccoSgravio") Is Nothing Then
                TxtIsBloccato.Text = CInt(Session("BloccoSgravio"))
            End If

            If Not Session("MyAvvisoRicalcolato") Is Nothing Then
                oMyAvviso = Session("MyAvvisoRicalcolato")
            Else
                oMyAvviso = Session("oMyAvviso")
            End If

            If oMyAvviso.tDataAccertamento.ToShortDateString <> DateTime.MaxValue.ToShortDateString Then
                sScript = "GestAlert('a', 'warning', '', '', 'Avviso con Atto di Ingiunzione dal " + New Formatta.FunctionGrd().FormattaDataGrd(oMyAvviso.tDataAccertamento) + "!\nImpossibile proseguire!');"
                RegisterScript(sScript, Me.GetType)
            Else
                Select Case TxtIsBloccato.Text
                    Case Costanti.BloccoSgravi.NO '-1: devo inserire il blocco
                        'inserisco il record di blocco
                        myIdentity = FncAvviso.SetLockSgravio(Costanti.BloccoSgravi.Bloccato, oMyAvviso.sCodiceCartella, oMyAvviso.IdContribuente, oMyAvviso.IdFlussoRuolo, ConstSession.UserName, cmdMyCommand)
                        If myIdentity > 0 Then
                            Session.Add("BloccoSgravio", 1)
                            sScript = "GestAlert('a', 'info', 'CmdEnableNewPartita', '', 'Posizione del Contribuente Bloccata. Procedere con la procedura di Sgravio!');"
                            RegisterScript(sScript, Me.GetType)
                            Exit Sub
                        Else
                            'devo dare messaggio di errore
                            sScript = "GestAlert('a', 'danger', '', '', 'Procedura di Sgravio terminata a causa di un errore!');"
                            RegisterScript(sScript, Me.GetType)
                            Exit Sub
                        End If
                    Case Costanti.BloccoSgravi.Fine '1: fine procedura di sgravio
                        Log.Debug("sono in fine procedura sgravio")
                        Try
                            cmdMyCommand = New SqlClient.SqlCommand
                            Dim myTrans As SqlClient.SqlTransaction
                            Dim myConnection As New SqlClient.SqlConnection(ConstSession.StringConnection)
                            myConnection.Open()
                            myTrans = myConnection.BeginTransaction
                            cmdMyCommand.Connection = myConnection
                            cmdMyCommand.CommandTimeout = 0
                            cmdMyCommand.Transaction = myTrans
                            'aggiorno i dati dello sgravio ed elimino il record di blocco
                            If Not oMyAvviso Is Nothing Then
                                Log.Debug("richiamo FncAvviso.CalcoloAvvisoSgravio(oMyAvviso, ConstSession.UserName, ConstSession.IsFromVariabile, sMyErr, MyDBEngine):: per::" & ConstSession.UserName & "::" & ConstSession.IsFromVariabile & "::" & sMyErr)
                                If FncAvviso.CalcoloAvvisoSgravio(ConstSession.DBType, ConstSession.StringConnection, oMyAvviso, ConstSession.UserName, ConstSession.IsFromVariabile, True, sMyErr, cmdMyCommand) = False Then
                                    'devo dare messaggio di errore
                                    sScript = "GestAlert('a', 'danger', 'CmdReloadPage', '', 'Procedura di Sgravio terminata a causa di un errore!" & sMyErr & "');"
                                    RegisterScript(sScript, Me.GetType)
                                    Exit Sub
                                End If
                            End If
                            'aggiorno la variabile di sessione
                            Session("oMyAvviso") = oMyAvviso
                            'solo se va a buon fine il salvataggio dei dati elimino il record di blocco o se non sono state fatte modifiche
                            Log.Debug("devo eliminare record di blocco")
                            myIdentity = FncAvviso.SetLockSgravio(Utility.Costanti.AZIONE_DELETE, oMyAvviso.sCodiceCartella, oMyAvviso.IdContribuente, oMyAvviso.IdFlussoRuolo, oMyAvviso.sOperatore, cmdMyCommand)
                            If myIdentity > 0 Then
                                Session("BloccoSgravio") = Nothing
                                TxtIsBloccato.Text = Costanti.BloccoSgravi.NO
                                Session("oListArticoliSgravi") = Nothing
                                myTrans.Commit()
                                'calcolo le rate
                                Dim oMyRuolo As New ObjRuolo
                                Dim FncCalcolo As New ClsElabRuolo
                                oMyRuolo.IdFlusso = oMyAvviso.IdFlussoRuolo
                                oMyRuolo = FncCalcolo.CalcoloRate(ConstSession.StringConnection, oMyRuolo, oMyAvviso.ID)
                                Session.Remove("MyAvvisoRicalcolato")
                                'aggiorno i dati a video
                                Dim oListAvvisi() As ObjAvviso = FncAvviso.GetAvviso(ConstSession.StringConnection, oMyAvviso.ID, ConstSession.IdEnte, oMyAvviso.IdFlussoRuolo, "", "", "", "", "", "", "", "", "", True, False, False, False, "", -1, Nothing)
                                If Not IsNothing(oListAvvisi) Then
                                    Session("oMyAvviso") = oListAvvisi(0)
                                    Session("oListTessere") = oListAvvisi(0).oTessere
                                    Session("oListScaglioni") = oListAvvisi(0).oScaglioni
                                    Session("oListArticoli") = oListAvvisi(0).oArticoli
                                    Session("oListDetVoci") = oListAvvisi(0).oDetVoci
                                    Session("oListRate") = oListAvvisi(0).oRate
                                End If
                                DivAttesa.Style.Add("display", "none")
                                'messagebox sgravio registrato correttamente
                                LblSgravio.Visible = False
                                sScript = "GestAlert('a', 'success', 'CmdReloadPage', '', 'Procedura di Sgravio terminata correttamente!')"
                                RegisterScript(sScript, Me.GetType)
                                Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                                fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Sgravio, "CmdSgravi", Utility.Costanti.AZIONE_NEW, ConstSession.CodTributo, ConstSession.IdEnte, oMyAvviso.ID)
                            Else
                                myTrans.Rollback()
                            End If
                        Catch ex As Exception
                            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAvvisi.CmdSgravi_Click.errore: ", ex)
                            Response.Redirect("../../../PaginaErrore.aspx")
                        Finally
                            cmdMyCommand.Dispose()
                            cmdMyCommand.Connection.Close()
                        End Try
                End Select
            End If
            Session("oListArticoliSgravi") = Nothing
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAvvisi.CmdSgravi_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub CmdSgravi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdSgravi.Click
    '    Dim myIdentity As Integer
    '    Dim sMyErr As String = ""
    '    Dim cmdMyCommand As SqlClient.SqlCommand = Nothing

    '    Try
    '        If Not Session("BloccoSgravio") Is Nothing Then
    '            TxtIsBloccato.Text = CInt(Session("BloccoSgravio"))
    '        End If

    '        If Not Session("MyAvvisoRicalcolato") Is Nothing Then
    '            oMyAvviso = Session("MyAvvisoRicalcolato")
    '        Else
    '            oMyAvviso = Session("oMyAvviso")
    '        End If

    '        If oMyAvviso.tDataAccertamento.ToShortDateString <> DateTime.MaxValue.ToShortDateString Then
    '            sScript = "GestAlert('a', 'warning', '', '', 'Avviso con Atto di Ingiunzione dal " + New Formatta.FunctionGrd().FormattaDataGrd(oMyAvviso.tDataAccertamento) + "!\nImpossibile proseguire!');"
    '            RegisterScript(sScript, Me.GetType)
    '        Else
    '            Select Case TxtIsBloccato.Text
    '                Case Costanti.BloccoSgravi.NO '-1: devo inserire il blocco
    '                    'inserisco il record di blocco
    '                    myIdentity = FncAvviso.SetLockSgravio(Costanti.BloccoSgravi.Bloccato, oMyAvviso.sCodiceCartella, oMyAvviso.IdContribuente, oMyAvviso.IdFlussoRuolo, ConstSession.UserName, cmdMyCommand)
    '                    If myIdentity > 0 Then
    '                        Session.Add("BloccoSgravio", 1)
    '                        'TxtIsBloccato.Text = Costanti.BloccoSgravi.Fine
    '                        'LblSgravio.Text = "(*) sgravio in corso"
    '                        'LblSgravio.Visible = True
    '                        sScript = "GestAlert('a', 'info', 'CmdEnableNewPartita', '', 'Posizione del Contribuente Bloccata. Procedere con la procedura di Sgravio!');"
    '                        'sScript += "document.getElementById('LnkNewPF').disabled=false;"
    '                        'sScript += "document.getElementById('LnkNewPC').disabled=false;"
    '                        RegisterScript(sScript, Me.GetType)
    '                        Exit Sub
    '                    Else
    '                        'devo dare messaggio di errore
    '                        sScript = "GestAlert('a', 'danger', '', '', 'Procedura di Sgravio terminata a causa di un errore!');"
    '                        RegisterScript(sScript, Me.GetType)
    '                        Exit Sub
    '                    End If
    '                Case Costanti.BloccoSgravi.Fine '1: fine procedura di sgravio
    '                    Log.Debug("sono in fine procedura sgravio")
    '                    Try
    '                        cmdMyCommand = New SqlClient.SqlCommand
    '                        Dim myTrans As SqlClient.SqlTransaction
    '                        Dim myConnection As New SqlClient.SqlConnection(ConstSession.StringConnection)
    '                        myConnection.Open()
    '                        myTrans = myConnection.BeginTransaction
    '                        cmdMyCommand.Connection = myConnection
    '                        cmdMyCommand.CommandTimeout = 0
    '                        cmdMyCommand.Transaction = myTrans
    '                        'aggiorno i dati dello sgravio ed elimino il record di blocco
    '                        'oListSgravi = CType(Session("oListArticoliSgravi"), ObjArticolo())
    '                        'oMyAvviso.oArticoli = oListSgravi
    '                        If Not oMyAvviso Is Nothing Then
    '                            Log.Debug("richiamo FncAvviso.CalcoloAvvisoSgravio(oMyAvviso, ConstSession.UserName, ConstSession.IsFromVariabile, sMyErr, MyDBEngine):: per::" & ConstSession.UserName & "::" & ConstSession.IsFromVariabile & "::" & sMyErr)
    '                            If FncAvviso.CalcoloAvvisoSgravio(ConstSession.DBType, ConstSession.StringConnection, oMyAvviso, ConstSession.UserName, ConstSession.IsFromVariabile, True, sMyErr, cmdMyCommand) = False Then
    '                                'devo dare messaggio di errore
    '                                sScript = "GestAlert('a', 'danger', 'CmdReloadPage', '', 'Procedura di Sgravio terminata a causa di un errore!" & sMyErr & "');"
    '                                RegisterScript(sScript, Me.GetType)
    '                                'If LoadDati() = False Then
    '                                '    Response.Redirect("../../../PaginaErrore.aspx")
    '                                'End If
    '                                Exit Sub
    '                            End If
    '                        End If
    '                        'aggiorno la variabile di sessione
    '                        Session("oMyAvviso") = oMyAvviso
    '                        'solo se va a buon fine il salvataggio dei dati elimino il record di blocco o se non sono state fatte modifiche
    '                        Log.Debug("devo eliminare record di blocco")
    '                        myIdentity = FncAvviso.SetLockSgravio(Utility.Costanti.AZIONE_DELETE, oMyAvviso.sCodiceCartella, oMyAvviso.IdContribuente, oMyAvviso.IdFlussoRuolo, oMyAvviso.sOperatore, cmdMyCommand)
    '                        If myIdentity > 0 Then
    '                            Session("BloccoSgravio") = Nothing
    '                            TxtIsBloccato.Text = Costanti.BloccoSgravi.NO
    '                            Session("oListArticoliSgravi") = Nothing
    '                            myTrans.Commit()
    '                            'calcolo le rate
    '                            Dim oMyRuolo As New ObjRuolo
    '                            Dim FncCalcolo As New ClsElabRuolo
    '                            oMyRuolo.IdFlusso = oMyAvviso.IdFlussoRuolo
    '                            oMyRuolo = FncCalcolo.CalcoloRate(oMyRuolo, oMyAvviso.ID)
    '                            Session.Remove("MyAvvisoRicalcolato")
    '                            'aggiorno i dati a video
    '                            'Dim oListAvvisi() As ObjAvviso = FncAvviso.GetAvviso(oMyAvviso.ID, ConstSession.IdEnte, oMyAvviso.IdFlussoRuolo, "", "", "", "", "", "", "", "", "", True, False, False, False, MyDBEngine)
    '                            Dim oListAvvisi() As ObjAvviso = FncAvviso.GetAvviso(ConstSession.StringConnection, oMyAvviso.ID, ConstSession.IdEnte, oMyAvviso.IdFlussoRuolo, "", "", "", "", "", "", "", "", "", True, False, False, False, "", -1, Nothing)
    '                            If Not IsNothing(oListAvvisi) Then
    '                                Session("oMyAvviso") = oListAvvisi(0)
    '                                Session("oListTessere") = oListAvvisi(0).oTessere
    '                                Session("oListScaglioni") = oListAvvisi(0).oScaglioni
    '                                Session("oListArticoli") = oListAvvisi(0).oArticoli
    '                                Session("oListDetVoci") = oListAvvisi(0).oDetVoci
    '                                Session("oListRate") = oListAvvisi(0).oRate
    '                            End If
    '                            'If LoadDati() = False Then
    '                            '    Response.Redirect("../../../PaginaErrore.aspx")
    '                            'End If
    '                            DivAttesa.Style.Add("display", "none")
    '                            'messagebox sgravio registrato correttamente
    '                            LblSgravio.Visible = False
    '                            sScript = "GestAlert('a', 'success', 'CmdReloadPage', '', 'Procedura di Sgravio terminata correttamente!')"
    '                            RegisterScript(sScript, Me.GetType)
    '                        Else
    '                            'WFSessione.oSession.oAppDB.RollbackTrans()
    '                            myTrans.Rollback()
    '                        End If
    '                    Catch ex As Exception
    '                        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAvvisi.CmdSgravi_Click.errore: ", ex)
    '                        Response.Redirect("../../../PaginaErrore.aspx")
    '                    Finally
    '                        'WFSessione.Kill()
    '                        cmdMyCommand.Dispose()
    '                        cmdMyCommand.Connection.Close()
    '                    End Try
    '            End Select
    '        End If
    '        Session("oListArticoliSgravi") = Nothing
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAvvisi.CmdSgravi_Click.errore: ", Err)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' Pulsante per la ristampa del documento.
    ''' </summary>
    Private Sub CmdStampaDoc_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdStampaDoc.Click
        Dim sScript, sTypeOrd, sNameModello, sTipoRuolo As String
        Dim nTipoElab As Integer = 1
        Dim FncDoc As New ClsGestDocumenti
        Dim nReturn, nMaxDocPerFile, nDocDaElab, nDocElab, x As Integer
        Dim oListDocStampati() As RIBESElaborazioneDocumentiInterface.Stampa.oggetti.GruppoURL = Nothing
        Dim bCreaPDF As Boolean = False
        Dim oListAvvisi(0) As ObjAvviso
        Dim oListAvvisiConv() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoCartella
        Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
        Dim nDecimal As Integer = 2

        Try
            Session.Remove("ELENCO_DOCUMENTI_STAMPATI")
            sTypeOrd = "Nominativo"
            sNameModello = "TARSU_ORDINARIO"
            sTipoRuolo = "O"
            nMaxDocPerFile = ConstSession.nMaxDocPerFile
            nDocDaElab = 1
            nDocElab = 1

            Try
                cmdMyCommand = New SqlClient.SqlCommand
                cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
                cmdMyCommand.Connection.Open()
                cmdMyCommand.CommandTimeout = 0
                'WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
                'If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
                '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
                'End If

                oListAvvisi(0) = Session("oMyAvviso")
                oListAvvisiConv = FncDoc.ConvAvvisi(oListAvvisi)
                For x = 0 To oListAvvisiConv.GetUpperBound(0)
                    oListAvvisiConv(x).Selezionato = True
                Next
                '*** 20140509 - TASI ***
                'in ristampa singolo viene sempre creato il documento per l'invio tramite mail
                Dim bSendByMail As Boolean = False
                'nReturn = FncDoc.ElaboraDocumenti(ConstSession.IdEnte, oMyAvviso.IdFlussoRuolo, sTipoRuolo, oListAvvisiConv(0).AnnoRiferimento, nDocDaElab, nDocElab, nTipoElab, sTypeOrd, sNameModello, nMaxDocPerFile, Session("ElaboraBollettini"), oListAvvisiConv, oListDocStampati, bCreaPDF, nDecimal, LblTipoCalcolo.Text, "BOLLETTINISTANDARD", "F24", MyDBEngine)
                nReturn = FncDoc.ElaboraDocumenti(ConstSession.IdEnte, oListAvvisiConv(0).IdFlussoRuolo, sTipoRuolo, oListAvvisiConv(0).AnnoRiferimento, nDocDaElab, nDocElab, nTipoElab, sTypeOrd, sNameModello, nMaxDocPerFile, Session("ElaboraBollettini"), oListAvvisiConv, oListDocStampati, bCreaPDF, nDecimal, LblTipoCalcolo.Text, "BOLLETTINISTANDARD", "F24", bSendByMail)
                '*** ***
            Catch Err As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAvvisi.CmdStampaDoc_Click.errore: ", Err)
                Response.Redirect("../../../PaginaErrore.aspx")
            Finally
                'WFSessione.Kill()
            End Try

            If Not oListDocStampati Is Nothing Then
                Session.Add("ELENCO_DOCUMENTI_STAMPATI", oListDocStampati)
            Else
                nReturn = 0
            End If
            If nReturn = 0 Then
                RegisterScript("GestAlert('a', 'danger', '', '', 'Si e\' verificato un\'errore in stampa!');", Me.GetType)
            Else
                'sScript = "parent.parent.Visualizza.location.href='../Documenti/ViewDocElaborati.aspx?IdFlussoRuolo=" & oMyAvviso.IdFlussoRuolo & "&IdContribuente=" & oMyAvviso.IdContribuente & "&Provenienza=1&ProvenienzaForm=SGRAVI';"
                'sScript += "parent.parent.Comandi.location.href='../Documenti/ComandiDocElaborati.aspx';"
                sScript = "document.getElementById('DivAttesa').style.display = 'none';"
                sScript += "document.getElementById('divStampa').style.display = '';"
                sScript += "document.getElementById('divAvviso').style.display = 'none';"
                sScript += "document.getElementById('loadStampa').src = '../Documenti/ViewDocElaborati.aspx?Provenienza=A&IdFlussoRuolo=" & oMyAvviso.IdFlussoRuolo & "&IdContribuente=" & oMyAvviso.IdContribuente & "&Provenienza=1&ProvenienzaForm=SGRAVI';"
                RegisterScript(sScript, Me.GetType)
            End If
            Session("ListCartelle") = Nothing
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAvvisi.CmdStampaDoc_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        Finally
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Sub
    ''' <summary>
    ''' Pulsante per la cancellazione dell'avviso.
    ''' Se è presente la data di accertamento la posizione non può essere modificata; messaggio di blocco.
    ''' </summary>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Analisi eventi</em>
    ''' </revision>
    ''' </revisionHistory>
    Private Sub CmdDeleteSgravi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdDeleteSgravi.Click
        Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
        Try
            oMyAvviso = Session("oMyAvviso")
            If oMyAvviso.tDataAccertamento.ToShortDateString <> DateTime.MaxValue.ToShortDateString Then
                sScript = "GestAlert('a', 'warning', '', '', 'Avviso con Atto di Ingiunzione dal " + New Formatta.FunctionGrd().FormattaDataGrd(oMyAvviso.tDataAccertamento) + "!\nImpossibile proseguire!');"
                RegisterScript(sScript, Me.GetType)
            Else
                'devo eliminare le operazioni fatte finora
                If FncAvviso.UndoSgravio(oMyAvviso, cmdMyCommand) = 0 Then
                    sScript = "GestAlert('a', 'danger', '', '', 'Errore in annullo sgravio');"
                    RegisterScript(sScript, Me.GetType)
                Else
                    Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                    fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, New Utility.Costanti.LogEventArgument().Sgravio, "CmdDeleteSgravi", Utility.Costanti.AZIONE_DELETE, ConstSession.CodTributo, ConstSession.IdEnte, oMyAvviso.ID)
                    sScript = "GestAlert('a', 'success', 'CmdGoBack', '', 'Sgravio annullato correttamente!');"
                    RegisterScript(sScript, Me.GetType)
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAvvisi.CmdDeleteSgravi_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub CmdDeleteSgravi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdDeleteSgravi.Click
    '    Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
    '    Try
    '        oMyAvviso = Session("oMyAvviso")
    '        If oMyAvviso.tDataAccertamento.ToShortDateString <> DateTime.MaxValue.ToShortDateString Then
    '            sScript = "GestAlert('a', 'warning', '', '', 'Avviso con Atto di Ingiunzione dal " + New Formatta.FunctionGrd().FormattaDataGrd(oMyAvviso.tDataAccertamento) + "!\nImpossibile proseguire!');"
    '            RegisterScript(sScript, Me.GetType)
    '        Else
    '            'devo eliminare le operazioni fatte finora
    '            If FncAvviso.UndoSgravio(oMyAvviso, cmdMyCommand) = 0 Then
    '                sScript = "GestAlert('a', 'danger', '', '', 'Errore in annullo sgravio');"
    '                RegisterScript(sScript, Me.GetType)
    '            Else
    '                sScript = "GestAlert('a', 'success', 'CmdGoBack', '', 'Sgravio annullato correttamente!');"
    '                'sScript += "parent.parent.Visualizza.location.href='RicAvvisi.aspx?IsFromVariabile=" & ConstSession.IsFromVariabile & "';"
    '                'sScript += "parent.parent.Comandi.location.href='ComandiRicAvvisi.aspx?IsFromVariabile=" & ConstSession.IsFromVariabile & "';"
    '                RegisterScript(sScript, Me.GetType)
    '            End If
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAvvisi.CmdDeleteSgravi_Click.errore: ", Err)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' Funzione per il caricamento dei dati da oggetto a videata
    ''' </summary>
    ''' <returns><c>False</c> in caso di errore altrimenti <c>True</c></returns>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' <strong>Qualificazione AgID-analisi_rel01</strong>
    ''' <em>Analisi eventi</em>
    ''' </revision>
    ''' </revisionHistory>
    Private Function LoadDati() As Boolean
        Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
        Try
            If Not Session("MyAvvisoRicalcolato") Is Nothing Then
                oMyAvviso = Session("MyAvvisoRicalcolato")
            Else
                oMyAvviso = Session("oMyAvviso")
            End If

            'carico i dati dell'avviso
            LblDatiAvviso.Text = "Avviso per l'anno " & oMyAvviso.sAnnoRiferimento & " N. " & oMyAvviso.sCodiceCartella
            If oMyAvviso.tDataEmissione <> Date.MinValue Then
                LblDatiAvviso.Text += " del " & oMyAvviso.tDataEmissione.ToShortDateString
            End If
            If ConstSession.HasNotifiche Then
                If oMyAvviso.tDataNotifica.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                    LblDatiAvviso.Text += vbTab + " Notificato il " + New Formatta.FunctionGrd().FormattaDataGrd(oMyAvviso.tDataNotifica)
                End If
                If oMyAvviso.tDataAccertamento.ToShortDateString <> Date.MaxValue.ToShortDateString Then
                    lblIntestAvviso.Text += vbTab + " in Atto di Ingiunzione dal " + New Formatta.FunctionGrd().FormattaDataGrd(oMyAvviso.tDataAccertamento)
                End If
            End If
            If oMyAvviso.sCodiceCartella = "" Then
                LblEmesso.Text = FormatNumber(oMyAvviso.impTotale, 2).ToString & " €"
            Else
                LblEmesso.Text = FormatNumber(oMyAvviso.impCarico, 2).ToString & " €"
            End If
            LblPagato.Text = FormatNumber(oMyAvviso.impPagato, 2).ToString & " €"
            LblSaldo.Text = FormatNumber(oMyAvviso.impSaldo, 2).ToString & " €"
            'controllo se devo caricare la griglia dei Pagamenti
            Log.Debug("GestAvvisi::Page_Load::devo caricare i Pagamenti")
            If Not oMyAvviso.oPagamenti Is Nothing Then
                GrdPagamenti.DataSource = oMyAvviso.oPagamenti
                GrdPagamenti.DataBind()
                GrdPagamenti.SelectedIndex = -1
                LblResultPagamenti.Style.Add("display", "none")
            Else
                LblResultPagamenti.Style.Add("display", "")
            End If
            '*** 201504 - Nuova Gestione anagrafica con form unico ***
            hdIdContribuente.Value = CInt(Request.Item("IdContribuente"))
            If ConstSession.HasPlainAnag Then
                ifrmAnag.Attributes.Add("src", "../../../Generali/asp/VisualAnag.aspx?IdContribuente=" & oMyAvviso.IdContribuente & "&Azione=" & Utility.Costanti.AZIONE_LETTURA)
            Else
                sScript += "LoadAnagrafica.location.href='../../Generali/DatiContribuente.aspx?IdContribuente=" & oMyAvviso.IdContribuente & "';"
                RegisterScript(sScript, Me.GetType)
            End If
            '*** ***
            'End If
            'controllo se devo caricare la griglia delle tessere
            Log.Debug("GestAvvisi::Page_Load::devo caricare le tessere")
            If Not Session("oListTessere") Is Nothing Then
                GrdTessere.DataSource = Session("oListTessere")
                GrdTessere.DataBind()
                GrdTessere.SelectedIndex = -1
                LblResultTessere.Style.Add("display", "none")
            Else
                LblResultTessere.Style.Add("display", "")
            End If
            'controllo se devo caricare la griglia degli scaglioni
            Log.Debug("GestAvvisi::Page_Load::devo caricare gli scaglioni")
            If Not Session("oListScaglioni") Is Nothing Then
                GrdScaglioni.DataSource = Session("oListScaglioni")
                GrdScaglioni.DataBind()
                GrdScaglioni.SelectedIndex = -1
                LblResultScaglioni.Style.Add("display", "none")
            Else
                LblResultScaglioni.Style.Add("display", "")
            End If
            'controllo se devo caricare la griglia degli articoli
            Log.Debug("GestAvvisi::Page_Load::devo caricare gli articoli")
            If Not oMyAvviso.oArticoli Is Nothing Then
                GrdArticoli.DataSource = oMyAvviso.oArticoli
                GrdArticoli.DataBind()
                GrdArticoli.SelectedIndex = -1
                LblResultArticoli.Style.Add("display", "none")
            Else
                LblResultArticoli.Style.Add("display", "")
            End If
            'controllo se devo caricare la griglia del dettaglio voci
            Log.Debug("GestAvvisi::Page_Load::devo caricare il dettaglio voci")
            If Not Session("oListDetVoci") Is Nothing Then
                GrdDettaglioVoci.DataSource = Session("oListDetVoci")
                GrdDettaglioVoci.DataBind()
                GrdDettaglioVoci.SelectedIndex = -1
                LblResultDettVoci.Style.Add("display", "none")
            Else
                LblResultDettVoci.Style.Add("display", "")
            End If
            'controllo se devo caricare la griglia delle rate
            Log.Debug("GestAvvisi::Page_Load::devo caricare le rate")
            If Not Session("oListRate") Is Nothing Then
                GrdRate.DataSource = Session("oListRate")
                GrdRate.DataBind()
                GrdRate.SelectedIndex = -1
                LblResultRate.Style.Add("display", "none")
            Else
                LblResultRate.Style.Add("display", "")
            End If

            '*** 20120917 - sgravi ***
            'controllo lo stato di lock a fronte di sgravio
            Dim nLockSgravio As Integer = FncAvviso.GetLockSgravio(oMyAvviso.sCodiceCartella, cmdMyCommand)
            Select Case nLockSgravio
                Case -1 'non presente procedura sgravio
                    LblSgravio.Visible = False
                    sScript = "$('#LnkNewPF').addClass('DisableBtn');"
                    sScript += "$('#LnkNewPC').addClass('DisableBtn');"
                    RegisterScript(sScript, Me.GetType)
                Case 0 'presente procedura sgravio
                    LblSgravio.Text = "(*) sgravio in corso"
                    sScript = "$('#LnkNewPF').removeClass('DisableBtn');"
                    sScript += "$('#LnkNewPC').removeClass('DisableBtn');"
                    RegisterScript(sScript, Me.GetType)
                    LblSgravio.Visible = True
                Case 1 'presente procedura sgravio altro utente
                    GrdArticoli.Enabled = False
                    sScript = "GestAlert('a', 'warning', 'CmdDisableNewPartita', '', 'Articolo in fase di Lavorazione da un\'altro Utente. Impossibile effettuare modifiche!')"
                    RegisterScript(sScript, Me.GetType)
                Case 2 'errore
                    Log.Debug("Si è verificato un errore in GestAvvisi::Page_Load::errore in recupero LockSgravio")
                    Response.Redirect("../../../PaginaErrore.aspx")
                Case 3
                    LblSgravio.Text = "Avviso in sola consultazione"
                    LblSgravio.Visible = True
                    sScript = "$('#LnkNewPF').addClass('DisableBtn');"
                    sScript += "$('#LnkNewPC').addClass('DisableBtn');"
                    sScript += "$('#BloccaSgravio').addClass('DisableBtn');"
                    sScript += "$('#ElaborazioneDocumenti').removeClass('DisableBtn');"
                    RegisterScript(sScript, Me.GetType)
            End Select
            '*** ***
            If ChkConferimenti.Checked = False Then
                LnkNewPC.Style.Add("display", "none")
            Else
                LnkNewPC.Style.Add("display", "")
            End If
            Return True
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAvvisi.LoadDati.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
            Return False
        End Try
    End Function
    'Private Function LoadDati() As Boolean
    '    Dim cmdMyCommand As SqlClient.SqlCommand = Nothing
    '    Try
    '        If Not Session("MyAvvisoRicalcolato") Is Nothing Then
    '            oMyAvviso = Session("MyAvvisoRicalcolato")
    '        Else
    '            oMyAvviso = Session("oMyAvviso")
    '        End If

    '        'carico i dati dell'avviso
    '        LblDatiAvviso.Text = "Avviso per l'anno " & oMyAvviso.sAnnoRiferimento & " N. " & oMyAvviso.sCodiceCartella
    '        If oMyAvviso.tDataEmissione <> Date.MinValue Then
    '            LblDatiAvviso.Text += " del " & oMyAvviso.tDataEmissione.ToShortDateString
    '        End If
    '        If ConstSession.HasNotifiche Then
    '            If oMyAvviso.tDataNotifica.ToShortDateString <> Date.MaxValue.ToShortDateString Then
    '                LblDatiAvviso.Text += vbTab + " Notificato il " + New Formatta.FunctionGrd().FormattaDataGrd(oMyAvviso.tDataNotifica)
    '            End If
    '            If oMyAvviso.tDataAccertamento.ToShortDateString <> Date.MaxValue.ToShortDateString Then
    '                lblIntestAvviso.Text += vbTab + " in Atto di Ingiunzione dal " + New Formatta.FunctionGrd().FormattaDataGrd(oMyAvviso.tDataAccertamento)
    '            End If
    '        End If
    '        If oMyAvviso.sCodiceCartella = "" Then
    '            LblEmesso.Text = FormatNumber(oMyAvviso.impTotale, 2).ToString & " €"
    '        Else
    '            LblEmesso.Text = FormatNumber(oMyAvviso.impCarico, 2).ToString & " €"
    '        End If
    '        LblPagato.Text = FormatNumber(oMyAvviso.impPagato, 2).ToString & " €"
    '        LblSaldo.Text = FormatNumber(oMyAvviso.impSaldo, 2).ToString & " €"
    '        'controllo se devo caricare la griglia dei Pagamenti
    '        Log.Debug("GestAvvisi::Page_Load::devo caricare i Pagamenti")
    '        If Not oMyAvviso.oPagamenti Is Nothing Then
    '            GrdPagamenti.DataSource = oMyAvviso.oPagamenti
    '            GrdPagamenti.DataBind()
    '            GrdPagamenti.SelectedIndex = -1
    '            LblResultPagamenti.Style.Add("display", "none")
    '        Else
    '            LblResultPagamenti.Style.Add("display", "")
    '        End If
    '        '*** 201504 - Nuova Gestione anagrafica con form unico ***
    '        hdIdContribuente.Value = CInt(Request.Item("IdContribuente"))
    '        If ConstSession.HasPlainAnag Then
    '            ifrmAnag.Attributes.Add("src", "../../../Generali/asp/VisualAnag.aspx?IdContribuente=" & oMyAvviso.IdContribuente & "&Azione=" & Utility.Costanti.AZIONE_LETTURA)
    '        Else
    '            sScript += "LoadAnagrafica.location.href='../../Generali/DatiContribuente.aspx?IdContribuente=" & oMyAvviso.IdContribuente & "';"
    '            RegisterScript(sScript, Me.GetType)
    '        End If
    '        '*** ***
    '        'End If
    '        'controllo se devo caricare la griglia delle tessere
    '        Log.Debug("GestAvvisi::Page_Load::devo caricare le tessere")
    '        If Not Session("oListTessere") Is Nothing Then
    '            GrdTessere.DataSource = Session("oListTessere")
    '            GrdTessere.DataBind()
    '            GrdTessere.SelectedIndex = -1
    '            LblResultTessere.Style.Add("display", "none")
    '        Else
    '            LblResultTessere.Style.Add("display", "")
    '        End If
    '        'controllo se devo caricare la griglia degli scaglioni
    '        Log.Debug("GestAvvisi::Page_Load::devo caricare gli scaglioni")
    '        If Not Session("oListScaglioni") Is Nothing Then
    '            GrdScaglioni.DataSource = Session("oListScaglioni")
    '            GrdScaglioni.DataBind()
    '            GrdScaglioni.SelectedIndex = -1
    '            LblResultScaglioni.Style.Add("display", "none")
    '        Else
    '            LblResultScaglioni.Style.Add("display", "")
    '        End If
    '        'controllo se devo caricare la griglia degli articoli
    '        Log.Debug("GestAvvisi::Page_Load::devo caricare gli articoli")
    '        If Not oMyAvviso.oArticoli Is Nothing Then
    '            GrdArticoli.DataSource = oMyAvviso.oArticoli
    '            GrdArticoli.DataBind()
    '            GrdArticoli.SelectedIndex = -1
    '            LblResultArticoli.Style.Add("display", "none")
    '        Else
    '            LblResultArticoli.Style.Add("display", "")
    '        End If
    '        'If Not Session("oListArticoli") Is Nothing Then
    '        '    If Not IsNothing(Session("oListArticoliSgravi")) Then
    '        '        Session("oListArticoli") = Session("oListArticoliSgravi")
    '        '    End If
    '        '    GrdArticoli.DataSource = Session("oListArticoli")
    '        '    GrdArticoli.start_index = GrdArticoli.CurrentPageIndex
    '        '    GrdArticoli.DataBind()
    '        '    GrdArticoli.SelectedIndex = -1
    '        '    LblResultArticoli.Style.Add("display", "none")
    '        'Else
    '        '    LblResultArticoli.Style.Add("display", "")
    '        'End If
    '        'controllo se devo caricare la griglia del dettaglio voci
    '        Log.Debug("GestAvvisi::Page_Load::devo caricare il dettaglio voci")
    '        If Not Session("oListDetVoci") Is Nothing Then
    '            GrdDettaglioVoci.DataSource = Session("oListDetVoci")
    '            GrdDettaglioVoci.DataBind()
    '            GrdDettaglioVoci.SelectedIndex = -1
    '            LblResultDettVoci.Style.Add("display", "none")
    '        Else
    '            LblResultDettVoci.Style.Add("display", "")
    '        End If
    '        'controllo se devo caricare la griglia delle rate
    '        Log.Debug("GestAvvisi::Page_Load::devo caricare le rate")
    '        If Not Session("oListRate") Is Nothing Then
    '            GrdRate.DataSource = Session("oListRate")
    '            GrdRate.DataBind()
    '            GrdRate.SelectedIndex = -1
    '            LblResultRate.Style.Add("display", "none")
    '        Else
    '            LblResultRate.Style.Add("display", "")
    '        End If

    '        '*** 20120917 - sgravi ***
    '        'controllo lo stato di lock a fronte di sgravio
    '        Dim nLockSgravio As Integer = FncAvviso.GetLockSgravio(oMyAvviso.sCodiceCartella, cmdMyCommand)
    '        Select Case nLockSgravio
    '            Case -1 'non presente procedura sgravio
    '                LblSgravio.Visible = False
    '                'sScript = "document.getElementById('LnkNewPF').disabled=true;"
    '                'sScript += "document.getElementById('LnkNewPC').disabled=true;"
    '                sScript = "$('#LnkNewPF').addClass('DisableBtn');"
    '                sScript += "$('#LnkNewPC').addClass('DisableBtn');"
    '                RegisterScript(sScript, Me.GetType)
    '            Case 0 'presente procedura sgravio
    '                LblSgravio.Text = "(*) sgravio in corso"
    '                'sScript = "document.getElementById('LnkNewPF').disabled=false;"
    '                'sScript += "document.getElementById('LnkNewPC').disabled=false;"
    '                sScript = "$('#LnkNewPF').removeClass('DisableBtn');"
    '                sScript += "$('#LnkNewPC').removeClass('DisableBtn');"
    '                RegisterScript(sScript, Me.GetType)
    '                LblSgravio.Visible = True
    '            Case 1 'presente procedura sgravio altro utente
    '                GrdArticoli.Enabled = False

    '                sScript = "GestAlert('a', 'warning', 'CmdDisableNewPartita', '', 'Articolo in fase di Lavorazione da un\'altro Utente. Impossibile effettuare modifiche!')"
    '                RegisterScript(sScript, Me.GetType)

    '                'sScript = "parent.parent.Comandi.document.getElementById('BloccaSgravio').disabled=true;"
    '                'sScript += "document.getElementById('LnkNewPF').disabled=true;"
    '                'sScript += "document.getElementById('LnkNewPC').disabled=true;"
    '                'RegisterScript(sScript, Me.GetType)
    '            Case 2 'errore
    '                Log.Debug("Si è verificato un errore in GestAvvisi::Page_Load::errore in recupero LockSgravio")
    '                Response.Redirect("../../../PaginaErrore.aspx")
    '        End Select
    '        '*** ***
    '        If ChkConferimenti.Checked = False Then
    '            LnkNewPC.Style.Add("display", "none")
    '        Else
    '            LnkNewPC.Style.Add("display", "")
    '        End If
    '        Return True
    '    Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAvvisi.LoadDati.errore: ", Err)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '        Return False
    '    End Try
    'End Function
    '*** ***
    ''' <summary>
    ''' Richiama la videata per l'inserimento di un nuovo Conferimento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub LnkNewPC_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles LnkNewPC.Click
        'RegisterScript(me.gettype(),"NewPC", "<script language='javascript'>alert('Funzionalita\' al momento non disponibile!')</script>")
        sScript = "LoadArticolo('-1','" & Utility.Costanti.AZIONE_NEW & "','PC','" & Request.Item("Provenienza") & "');"
        RegisterScript(sScript, Me.GetType)
    End Sub
    ''' <summary>
    ''' Richiama la videata per l'inserimento di un nuovo Immobile
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub LnkNewPF_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles LnkNewPF.Click
        'ShowPopUp("T", -1, Costanti.AZIONE_NEW)
        sScript = "LoadArticolo('-1','" & Utility.Costanti.AZIONE_NEW & "','PF','" & Request.Item("Provenienza") & "');"
        RegisterScript(sScript, Me.GetType)
    End Sub

#Region "Griglie"
    ''' <summary>
    ''' Gestione degli eventi sulla griglia.
    ''' Con il comando <c>RowOpen</c> si entra in visualizzazione di un immobile.
    ''' Se si sta tentando di entrare in una Parte Variabile messaggio di blocco. E' possibile entrare solo su Parte Fissa o Conferimenti.
    ''' </summary>
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim IDRow As String = e.CommandArgument.ToString()
            If e.CommandName = "RowOpen" Then 'articoli
                For Each myRow As GridViewRow In GrdArticoli.Rows
                    If CType(myRow.FindControl("hfid"), HiddenField).Value = IDRow Then
                        If CType(myRow.FindControl("Label13"), Label).Text <> ObjArticolo.PARTEPRECEMESSO_DESCR Then
                            If CType(myRow.FindControl("hftipopartita"), HiddenField).Value = ObjArticolo.PARTEFISSA Or CType(myRow.FindControl("hftipopartita"), HiddenField).Value = ObjArticolo.PARTECONFERIMENTI Then
                                sScript = "LoadArticolo('" & IDRow & "','" & Utility.Costanti.AZIONE_UPDATE & "','','" & Request.Item("Provenienza") & "')"
                            Else
                                sScript = "GestAlert('a', 'warning', '', '', 'Per modificare questa partita agire sugli immobili.')"
                            End If
                        Else
                            sScript = "GestAlert('a', 'warning', '', '', 'Partita non modificabile!')"
                        End If
                    End If
                Next
            End If
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAvvisi.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    'Private Sub GrdArticoli_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdArticoli.ItemDataBound
    'Try
    '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '        If CType(e.Item.FindControl("Label13"), Label).Text <> ObjArticolo.PARTEPRECEMESSO_DESCR Then
    '            If e.Item.Cells(10).Text = ObjArticolo.PARTEFISSA Or e.Item.Cells(10).Text = ObjArticolo.PARTECONFERIMENTI Then
    '                e.Item.Attributes.Add("onClick", "LoadArticolo('" & e.Item.Cells(9).Text & "','" & Utility.Costanti.AZIONE_UPDATE & "','','" & Request.Item("Provenienza") & "')")
    '            Else
    '                e.Item.Attributes.Add("onClick", "alert('Per modificare questa partita agire sugli immobili.')")
    '            End If
    '        Else
    '            e.Item.Attributes.Add("onClick", "alert('Partita non modificabile!')")
    '        End If
    '    End If
    ' Catch ex As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAvvisi.GrdArticoli_ItemDataBound.errore: ", ex)
    '       Response.Redirect("../../../PaginaErrore.aspx")
    'End Try
    'End Sub
#End Region

    'Private Sub ShowPopUp(ByVal sTipoPopUP As String, ByVal nIdAvviso As Integer, ByVal sAzione As String)
    '    Try
    'Dim oMyTestata As New ObjTestata

    ''carico i dati dal form
    'oMyTestata.Id = TxtIdDich.Text
    'oMyTestata.IdTestata = TxtIdDich.Text
    'oMyTestata.sEnte = Session("COD_ENTE")
    'oMyTestata.IdContribuente = TxtCodContribuente.Text
    'If TxtDataDichiarazione.Text.Trim <> "" Then
    '    oMyTestata.tDataDichiarazione = TxtDataDichiarazione.Text.Trim
    'End If
    'oMyTestata.sNDichiarazione = TxtNDichiarazione.Text.Trim
    'If TxtDataProtocollo.Text.Trim <> "" Then
    '    oMyTestata.tDataProtocollo = TxtDataProtocollo.Text.Trim
    'End If
    'oMyTestata.sNProtocollo = TxtNProtocollo.Text.Trim
    'oMyTestata.sIdProvenienza = DdlTipoDich.SelectedItem.Value
    'oMyTestata.sNoteDichiarazione = TxtNoteDich.Text
    'oMyTestata.tDataInserimento = Now
    'oMyTestata.tDataCessazione = Nothing
    'oMyTestata.sOperatore = ConstSession.UserName
    'oMyTestata.oTessere = Session("oListTessere")
    'oMyTestata.oTesUI = Session("oListTesUI")
    'oMyTestata.oFamiglia = Session("oDatiFamiglia")
    'oMyTestata.oAnagrafe = Session("oAnagrafe")
    ''memorizzo l'oggetto nella sessione
    'Session("oTestata") = oMyTestata

    'Dim x As Integer
    'If Not IsNothing(oMyTestata.oTessere) Then
    '    For x = 0 To oMyTestata.oTessere.GetUpperBound(0)
    '        If nIdAvviso = oMyTestata.oTessere(x).Id Then
    '            Session("oAvviso") = oMyTestata.oTessere(x)
    '            Session("oListImmobili") = oMyTestata.oTessere(x).oImmobili
    '        End If
    '    Next
    'End If
    ''apro il popup passandogli l'indice dell'array da visualizzare
    'Dim sScript, sProvenienza As String
    'sProvenienza = Costanti.FORMPROVENIENZA.DICHIARAZIONE
    'If sTipoPopUP = "F" Then
    '    sScript = "ShowInsertFamiglia('" & TxtIdDich.Text & "','" & sAzione & "')"
    '    RegisterScript(me.gettype(),"", "<script language='javascript'>" & sScript & "</script>")
    'ElseIf sTipoPopUP = "T" Then
    '    sScript = "ShowInsertAvviso('" & TxtCodContribuente.Text & "','" & TxtIdDich.Text & "','" & nIdAvviso & "','" & sAzione & "')"
    '    RegisterScript(me.gettype(),"", "<script language='javascript'>" & sScript & "</script>")
    'ElseIf sTipoPopUP = "I" Then
    '    sScript = "ShowInsertUI('" & TxtCodContribuente.Text & "','" & nIdAvviso & "','" & nIdUI & "','" & sAzione & "','" & sProvenienza & "')"
    '    RegisterScript(me.gettype(),"", "<script language='javascript'>" & sScript & "</script>")
    'ElseIf sTipoPopUP = "A" Then
    '    sScript = "ShowRicUIAnater()"
    '    RegisterScript(me.gettype(),"", "<script language='javascript'>" & sScript & "</script>")
    'End If
    'Catch Err As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAvvisi.ShowPopUp.errore: ", Err)
    '        Response.Redirect("../../../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="bTypeAbilita"></param>
    ''' <param name="IsSoloContrib"></param>
    Private Sub Abilita(ByVal bTypeAbilita As Boolean, ByVal IsSoloContrib As Integer)
        LnkAnagTributi.Visible = False : LnkAnagAnater.Visible = False
        LnkNewPV.Visible = False
        LnkNewPF.Visible = False
        Try
            ''se passo da modifica a variazione devo disabilitare il pulsante di modifica/cancellazione e abilitare il pulsante di salvataggio
            'If bTypeAbilita = True Then
            '    ReDim Preserve oListCmd(1)
            '    oListCmd(0) = "Modifica"
            '    oListCmd(1) = "Delete"
            '    For x = 0 To oListCmd.Length - 1
            '        sScript += "parent.Comandi.document.getElementById('" & oListCmd(x).ToString() & "').disabled=" & True.ToString.ToLower & ";"
            '    Next
            '    ReDim Preserve oListCmd(2)
            '    oListCmd(2) = "Salva"
            '    For x = 2 To oListCmd.Length - 1
            '        sScript += "parent.Comandi.document.getElementById('" & oListCmd(x).ToString() & "').disabled=" & False.ToString.ToLower & ";"
            '    Next
            '    RegisterScript(me.gettype(),"", "<script language='javascript'>" & sScript & "</script>")
            'End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAvvisi.Abilita.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Pulsante per l'uscita dalla videata.
    ''' In base alla provenienza si reindirizza sulla pagina chiamante corretta.
    ''' </summary>
    Private Sub CmdGoBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdGoBack.Click
        Try
            'aggiorno la pagina chiamante
            Dim sScript As String = ""

            If Request.Item("Provenienza") = "" Then
                sScript = "parent.Visualizza.location.href='RicAvvisi.aspx?IsFromVariabile=" & ConstSession.IsFromVariabile() & "';"
                sScript += "parent.Comandi.location.href='ComandiRicAvvisi.aspx?IsFromVariabile=" & ConstSession.IsFromVariabile() & "';"
                sScript += "parent.Basso.location.href='../../../aspVuota.aspx';"
                sScript += "parent.Nascosto.location.href='../../../aspVuota.aspx';"
                '*** 20150703 - INTERROGAZIONE GENERALE ***
            ElseIf Request.Item("Provenienza") = Costanti.FormProvenienza.InterGen Then
                sScript = "parent.Visualizza.location.href='../../../Interrogazioni/DichEmesso.aspx?Ente=" & ConstSession.IdEnte & "';"
                sScript += "parent.Comandi.location.href='../../../aspVuotaRemoveComandi.aspx';"
                sScript += "parent.Basso.location.href='../../../aspVuotaRemoveComandi.aspx';"
                sScript += "parent.Nascosto.location.href='../../../aspVuotaRemoveComandi.aspx';"
                '*** ***
                '**** 201809 - Cartelle Insoluti ***
            ElseIf Request.Item("Provenienza") = Costanti.FormProvenienza.Notifiche Then
                sScript = "parent.Visualizza.location.href='GestNotifica.aspx?IsFromVariabile=" & ConstSession.IsFromVariabile() & "';"
                sScript += "parent.Comandi.location.href='../../../aspVuotaRemoveComandi.aspx';"
                sScript += "parent.Basso.location.href='../../../aspVuotaRemoveComandi.aspx';"
                sScript += "parent.Nascosto.location.href='../../../aspVuotaRemoveComandi.aspx';"
                '*** ***
            End If
            RegisterScript(sScript, Me.GetType)
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.GestAvvisi.CmdGoBack.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
