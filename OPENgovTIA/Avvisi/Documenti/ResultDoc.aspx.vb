Imports log4net
Imports RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti
''' <summary>
''' Pagina per la visualizzazione dei documenti da elaborare e per la selezione dei parametri di elaborazione.
''' E' possibile indicare:
''' - Tipo di elaborazione: Prova, Effettivo.
''' - Tutti i contribuenti.
''' - Preparare per l'invio con mail.
''' - Tipo di bollettino.
''' - Numero di documenti per gruppo.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class ResultDoc
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ResultDoc))
    Private oListAvvisiConv() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoCartella

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
    ''' Al caricamento della pagina vengono visualizzati i documenti che rispondono ai criteri impostati.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        'Dim WFErrore As String = ""
        'Dim WFSessione As OPENUtility.CreateSessione
        Dim oMyRuolo() As ObjRuolo
        Dim FncAvvisi As New GestAvviso
        Dim oListAvvisi() As ObjAvviso
        Dim FncDoc As New ClsGestDocumenti
        'Dim MyDBEngine As DAL.DBEngine = Nothing

        Try
            'WFSessione = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IDENTIFICATIVOAPPLICAZIONE)
            'If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
            '    Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
            'End If

            If Page.IsPostBack = False Then
                txtNumDoc.Text = ConstSession.nMaxDocPerFile.ToString()
                'prelevo il ruolo
                oMyRuolo = Session("oRuoloTIA")
                If IsNumeric(Request.Item("IdRuolo")) Then
                    oMyRuolo(0).IdFlusso = CInt(Request.Item("IdRuolo"))
                End If
                LoadTipoBollettino(oMyRuolo(0).IdFlusso)
                'oListAvvisi = FncAvvisi.GetAvviso(-1, ConstSession.IdEnte, oMyRuolo(0).IdFlusso, "", "", "", "", "", "", Request.Item("CodiceCartella"), Request.Item("NominativoDa"), Request.Item("NominativoA"), False, False, False, True, MyDBEngine)
                oListAvvisi = FncAvvisi.GetAvviso(ConstSession.StringConnection, -1, ConstSession.IdEnte, oMyRuolo(0).IdFlusso, "", "", "", "", "", "", Request.Item("CodiceCartella"), Request.Item("NominativoDa"), Request.Item("NominativoA"), False, False, False, True, "", -1, Nothing)
                oListAvvisiConv = FncDoc.ConvAvvisi(oListAvvisi)
                Session("ListCartelle") = oListAvvisiConv
                If Not IsNothing(oListAvvisiConv) Then
                    If oListAvvisi.Length > 0 Then
                        Session.Add("oListAvvisi", oListAvvisiConv)
                        GrdAvvisi.DataSource = oListAvvisiConv
                        GrdAvvisi.DataBind()
                    End If
                End If
                'If optProve.Checked = True Then
                '    Session("ElaboraDocumenti") = 1
                'Else
                '    Session("ElaboraDocumenti") = 2
                'End If

                If Session("nMaxDocPerFile") Is Nothing Then
                    If IsNumeric(txtNumDoc.Text) Then
                        Session("nMaxDocPerFile") = txtNumDoc.Text
                    End If
                End If
            Else
                ControllaCheckbox()
                'oListAvvisiConv = CType(Session("oListAvvisi"), RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoCartella())
                'GrdAvvisi.DataSource = oListAvvisiConv

                'If IsNumeric(txtNumDoc.Text) Then
                '    Session("nMaxDocPerFile") = txtNumDoc.Text
                'End If

                'If chkElaboraBollettini.Checked Then
                '    Session("ElaboraBollettini") = True
                'Else
                '    Session("ElaboraBollettini") = False
                'End If
            End If
            If optProve.Checked = True Then
                Session("ElaboraDocumenti") = 1
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultDoc.Page_Load.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        Finally
            'WFSessione.Kill()
        End Try
    End Sub
    ''' <summary>
    ''' Funzione che abilita/disabilita le opzioni in base alla selezione.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub optProve_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optProve.CheckedChanged
        Try
            If optProve.Checked = True Then
                Session("ElaboraDocumenti") = 1
                chkElaboraTutti.Checked = False
                chkElaboraTutti.Visible = False
                chkElaboraTutti_CheckedChanged(sender, e)
            Else
                Session("ElaboraDocumenti") = 2
                chkElaboraTutti.Visible = True
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultDoc.optProve_CheckedChanged.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Funzione che abilita/disabilita le opzioni in base alla selezione.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub optEffettivo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optEffettivo.CheckedChanged
        Try
            If optEffettivo.Checked = True Then
                Session("ElaboraDocumenti") = 2
                chkElaboraTutti.Visible = True
                '*** 20140509 - TASI ***
                chkSendMail.Visible = True : chkSendMail.Checked = True : Session("bSendByMail") = True
                '*** ***
            Else
                Session("ElaboraDocumenti") = 1
                chkElaboraTutti.Checked = False
                chkElaboraTutti.Visible = False
                chkElaboraTutti_CheckedChanged(sender, e)
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultDoc.optEffettivo_CheckedChanged.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Funzione che seleziona/deseleziona tutti i documenti in base alla selezione.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub chkElaboraTutti_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkElaboraTutti.CheckedChanged
        Dim itemGrid As GridViewRow
        Dim x As Integer
        Try
            If chkElaboraTutti.Checked = True Then
                '*** 20140509 - TASI ***
                chkSendMail.Visible = True : chkSendMail.Checked = True : Session("bSendByMail") = True
                '*** ***
                '*****************************************************
                'seleziono tutti gli elementi
                '*****************************************************
                oListAvvisiConv = CType(Session("oListAvvisi"), RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoCartella())
                If Not oListAvvisiConv Is Nothing Then
                    For x = 0 To oListAvvisiConv.Length - 1
                        oListAvvisiConv(x).Selezionato = True
                    Next
                    For Each itemGrid In GrdAvvisi.Rows
                        CType(itemGrid.FindControl("ChkSelezionato"), CheckBox).Checked = True
                    Next
                End If
                Session("ListCartelle") = oListAvvisiConv
            Else
                '*** 20140509 - TASI ***
                chkSendMail.Checked = False : chkSendMail.Visible = True : Session("bSendByMail") = False
                '*** ***
                oListAvvisiConv = CType(Session("oListAvvisi"), RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoCartella())
                If Not oListAvvisiConv Is Nothing Then
                    For x = 0 To oListAvvisiConv.Length - 1
                        oListAvvisiConv(x).Selezionato = False
                    Next
                    For Each itemGrid In GrdAvvisi.Rows
                        CType(itemGrid.FindControl("ChkSelezionato"), CheckBox).Checked = False
                    Next
                End If
                Session("ListCartelle") = oListAvvisiConv
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultDoc.chkElaboraTutti_CheckedChanged.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub

    '*** 20140509 - TASI ***
    ''' <summary>
    ''' Funzione che imposta se deve essere fatto l'invio tramite mail.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub chkSendMail_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSendMail.CheckedChanged
        Try
            If chkSendMail.Checked = True Then
                Session("bSendByMail") = True
            Else
                Session("bSendByMail") = False
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultDoc.chkSendMail_CheckedChanged.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** ***
    ''' <summary>
    ''' Funzione che aggiorna l'oggetto di sessione con i documenti da elaborare.
    ''' </summary>
    Private Sub ControllaCheckbox()
        Dim itemGrid As GridViewRow
        Dim oListAvvisi() As RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoCartella
        Dim x As Integer

        Try
            oListAvvisi = CType(Session("ListCartelle"), RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoCartella())
            For Each itemGrid In GrdAvvisi.Rows
                For x = 0 To oListAvvisi.GetUpperBound(0)
                    'confronto il codice cartella per individuare l'elemento all'interno della griglia
                    If oListAvvisi(x).CodiceCartella = CType(itemGrid.Cells(2).Text, Long) Then
                        oListAvvisi(x).Selezionato = CType(itemGrid.FindControl("ChkSelezionato"), CheckBox).Checked
                        Exit For
                    End If
                Next
            Next
            Session("ListCartelle") = oListAvvisi
            If IsNumeric(txtNumDoc.Text) Then
                Session("nMaxDocPerFile") = txtNumDoc.Text
            End If

            If chkElaboraBollettini.Checked Then
                Session("ElaboraBollettini") = True
            Else
                Session("ElaboraBollettini") = False
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultDoc.ControllaCheckbox.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Funzione che abilita/disabilita le tipologie di bollettini attivate.
    ''' </summary>
    ''' <param name="IdFlussoRuolo"></param>
    Private Sub LoadTipoBollettino(ByVal IdFlussoRuolo As Integer)
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myDataReader As SqlClient.SqlDataReader
        Dim dtMyDati As New DataTable()
        Dim dtMyRow As DataRow

        Try
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            'Valorizzo la connessione
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            cmdMyCommand.CommandTimeout = 0
            If cmdMyCommand.Connection.State = ConnectionState.Closed Then
                cmdMyCommand.Connection.Open()
            End If
            'valorizzo i parameters:
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDFLUSSORUOLO", SqlDbType.Int)).Value = IdFlussoRuolo
            cmdMyCommand.CommandText = "prc_GetTipoBollettinoRuolo"
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myDataReader = cmdMyCommand.ExecuteReader
            dtMyDati.Load(myDataReader)
            For Each dtMyRow In dtMyDati.Rows
                Select Case dtMyRow("tipobollettino").ToString
                    Case RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata.bollettino_896
                        optTD896.Checked = True
                    Case RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoDatiRata.bollettino_f24
                        optF24.Checked = True
                    Case Else
                        optTD123.Checked = True
                End Select
                Session("tipobollettino") = dtMyRow("tipobollettino").ToString
            Next
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultDoc.LoadTipoBollettino.errore: ", Err)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            Response.Redirect("../../../PaginaErrore.aspx")
        Finally
            dtMyDati.Dispose()
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Sub
#Region "Griglie"
    ''' <summary>
    ''' Gestione del cambio pagina della griglia.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdPageIndexChanging(ByVal sender As Object, e As GridViewPageEventArgs)
        ControllaCheckbox()
        LoadSearch(e.NewPageIndex)
        Try
            If IsNumeric(txtNumDoc.Text) Then
                Session("nMaxDocPerFile") = txtNumDoc.Text
            End If

            If chkElaboraBollettini.Checked Then
                Session("ElaboraBollettini") = True
            Else
                Session("ElaboraBollettini") = False
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultDoc.GrdPageIndexChanging.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdAvvisi.DataSource = CType(Session("oListAvvisi"), RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoCartella())
            If page.HasValue Then
                GrdAvvisi.PageIndex = page.Value
            End If
            GrdAvvisi.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ResultDoc.LoadSearch.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
End Class
