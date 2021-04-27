Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports log4net
Imports Utility

Partial Class InserimentoPagamenti
    Inherits BasePage
    Private IdPagamento, DataFattura, NumeroFattura, ImportoPagamento, ImportoEmesso, DataPagamento, CodUtente, Operazione, DataAccredito As String
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
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ClsFatture))
    Private oReplace As New ClsGenerale.Generale
    Private ClsGenerale As New ClsGenerale.Generale

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Operazione = Request.Item("Operazione")
        DataFattura = Request.Item("DataFattura")
        NumeroFattura = Request.Item("NumeroFattura")
        ImportoPagamento = Request.Item("Importo")
        ImportoEmesso = Request.Item("ImportoEmesso")
        DataPagamento = Request.Item("DataPagamento")
        IdPagamento = Request.Item("Id")
        CodUtente = Request.Item("CodUtente")
        DataAccredito = Request.Item("DataAccredito")
        Try
            If Operazione <> "Inserimento" Then
                If IdPagamento = "" Or IdPagamento Is Nothing Or IdPagamento = "-1" Then
                    Operazione = ""
                End If
            End If
            If Not Page.IsPostBack Then
                txtDataFattura.Text = oReplace.GiraDataFromDB(DataFattura)
                txtNfattura.Text = NumeroFattura
                If StringOperation.FormatDouble(ImportoPagamento) = 0 Then
                    ImportoPagamento = ImportoEmesso
                End If
                txtImportoPagamento.Text = ImportoPagamento
                If StringOperation.FormatString(DataPagamento) <> "" Then
                    If StringOperation.FormatString(DataPagamento).Replace("/", "").Length >= 8 And StringOperation.FormatString(DataAccredito).Replace("/", "").Substring(0, 8) <> "31129999" Then
                        txtDataPagamento.Text = oReplace.GiraDataFromDB(StringOperation.FormatString(DataPagamento).Replace("/", "").Substring(0, 8))
                    End If
                End If
                If StringOperation.FormatString(DataAccredito) <> "" Then
                    If StringOperation.FormatString(DataAccredito).Replace("/", "").Length >= 8 And StringOperation.FormatString(DataAccredito).Replace("/", "").Substring(0, 8) <> "31129999" Then
                        txtDataAccredito.Text = oReplace.GiraDataFromDB(StringOperation.FormatString(DataAccredito).Replace("/", "").Substring(0, 8))
                    End If
                End If
            End If

            Dim sScript As String
            sScript = "Setfocus(document.getElementById('txtNfattura'));"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.InserimentoPagamenti.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    'Private Sub BtnSalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSalva.Click
    '    Dim FncPagamenti As New ClsPagamenti
    '    Dim str As String
    '    Dim strErrore As String=""
    '    Dim oMyPagamento As New OggettoPagamento
    '    Dim oMyPagamentoOLD As New OggettoPagamento
    '    Dim WFSessione As CreateSessione
    '    Dim WFErrore As String = ""
    'Try
    '    'apro la connessione al db
    '    WFSessione = New CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '    If Not WFSessione.CreaSessione(ConstSession.UserName, WFErrore) Then
    '        Throw New Exception("CmdImporta_Click::" & "Errore durante l'apertura della sessione di WorkFlow")
    '        Exit Sub
    '    End If

    '    If Operazione = "modifica" Then
    '        oMyPagamento.ID = IdPagamento
    '        oMyPagamento.IDEnte = ConstSession.IdEnte
    '        oMyPagamento.sDataFattura = txtDataFattura.Text
    '        oMyPagamento.ImportoPagamento = txtImportoPagamento.Text
    '        oMyPagamento.sDataPagamento = txtDataPagamento.Text
    '        oMyPagamento.sNumeroFattura = txtNfattura.Text
    '        oMyPagamento.sDataAccredito = txtDataAccredito.Text

    '        oMyPagamentoOLD.ID = IdPagamento
    '        If FncPagamenti.SetPagamento(oMyPagamento, 1, WFSessione) > 0 Then
    '            Dim omydettaglio As New ObjDettaglioPagamento
    '            omydettaglio.IDPagamento = oMyPagamentoOLD.ID
    '            'elimino il vecchio dettaglio
    '            If FncPagamenti.SetDettaglioPagamento(omydettaglio, 2, WFSessione) <= 0 Then
    '                strErrore = "Si è verificato un\'errore nella cancellazione del dettaglio!"
    '                str = "GestAlert('a', 'warning', '', '', '" & strErrore & "');"
    '                RegisterScript(sScript , Me.GetType())
    '            End If
    '            If FncPagamenti.SetPagamento(oMyPagamentoOLD, 4, WFSessione) <= 0 Then
    '                strErrore = "Si è verificato un\'errore nella cancellazione del dettaglio!"
    '                str = "GestAlert('a', 'warning', '', '', '" & strErrore & "');"
    '                RegisterScript(sScript , Me.GetType())
    '            End If
    '            'se il richiamo al dettaglio dei pagamenti attraverso la funzione DettagliaPagamenti(sEnteImport, sOperatore, WFSessione) è negativo:
    '            If FncPagamenti.DettagliaPagamenti(ConstSession.IdEnte, ConstSession.UserName, WFSessione) <= 0 Then
    '                strErrore = "Si è verificato un\'errore in inserimento dettaglio!"
    '                str = "GestAlert('a', 'warning', '', '', '" & strErrore & "');"
    '                RegisterScript(sScript , Me.GetType())
    '                str = "UscitaDopoOperazione();"
    '                RegisterScript(sScript , Me.GetType())
    '            Else
    '                str = "GestAlert('a', 'success', '', '', 'Modifica effettuata correttamente!'); parent.Search();"
    '                RegisterScript(sScript , Me.GetType())
    '                str = "ConfermaUscita();"
    '                RegisterScript(sScript , Me.GetType())
    '            End If
    '        Else
    '            strErrore = "Si è verificato un\'errore!"
    '            str = "GestAlert('a', 'warning', '', '', '" & strErrore & "');"
    '            RegisterScript(sScript , Me.GetType())
    '            str = "UscitaDopoOperazione();"
    '            RegisterScript(sScript , Me.GetType())
    '        End If
    '    Else
    '        oMyPagamento.IDEnte = ConstSession.IdEnte
    '        oMyPagamento.sProvenienza = "DE"
    '        oMyPagamento.sSegno = "+"
    '        oMyPagamento.sDataFattura = txtDataFattura.Text
    '        oMyPagamento.ImportoPagamento = txtImportoPagamento.Text
    '        oMyPagamento.sDataPagamento = txtDataPagamento.Text
    '        oMyPagamento.sNumeroFattura = txtNfattura.Text
    '        oMyPagamento.nCodUtente = CodUtente
    '        oMyPagamento.sDataAccredito = txtDataAccredito.Text

    '        If FncPagamenti.SetPagamento(oMyPagamento, 0, WFSessione) > 0 Then
    '            'se il richiamo al dettaglio dei pagamenti attraverso la funzione DettagliaPagamenti(sEnteImport, sOperatore, WFSessione) è negativo:
    '            If FncPagamenti.DettagliaPagamenti(ConstSession.IdEnte, ConstSession.UserName, WFSessione) <= 0 Then
    '                strErrore = "Si è verificato un\'errore in inserimento dettaglio!"
    '                str = "GestAlert('a', 'warning', '', '', '" & strErrore & "');"
    '                RegisterScript(sScript , Me.GetType())
    '                str = "UscitaDopoOperazione();"
    '                RegisterScript(sScript , Me.GetType())
    '            Else
    '                str = "GestAlert('a', 'success', '', '', 'Inserimento effettuato correttamente!'); parent.Search();"
    '                RegisterScript(sScript , Me.GetType())
    '                str = "ConfermaUscita();"
    '                RegisterScript(sScript , Me.GetType())
    '            End If
    '        Else
    '            strErrore = "Si è verificato un\'errore!"
    '            str = "GestAlert('a', 'warning', '', '', '" & strErrore & "');"
    '            RegisterScript(sScript , Me.GetType())
    '            str = "UscitaDopoOperazione();"
    '            RegisterScript(sScript , Me.GetType())
    '        End If
    '    End If
    '  Catch ex As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.InserimentoPagamenti.BtnSalva_Click.errore: ", ex)
    '       Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub
    Private Sub BtnSalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSalva.Click
        Dim FncPagamenti As New ClsPagamenti
        Dim sScript As String = ""
        Dim strErrore As String = ""
        Dim oMyPagamento As New OggettoPagamento
        Dim oMyPagamentoOLD As New OggettoPagamento
        Try
            If Operazione = "modifica" Then
                oMyPagamento.ID = IdPagamento
                oMyPagamento.IDEnte = ConstSession.IdEnte
                oMyPagamento.sDataFattura = txtDataFattura.Text
                oMyPagamento.ImportoPagamento = txtImportoPagamento.Text
                oMyPagamento.sDataPagamento = txtDataPagamento.Text
                oMyPagamento.sNumeroFattura = txtNfattura.Text
                oMyPagamento.sDataAccredito = txtDataAccredito.Text

                oMyPagamentoOLD.ID = IdPagamento
                If FncPagamenti.SetPagamento(oMyPagamento, 1) > 0 Then
                    Dim omydettaglio As New ObjDettaglioPagamento
                    omydettaglio.IDPagamento = oMyPagamentoOLD.ID
                    'elimino il vecchio dettaglio
                    If FncPagamenti.SetDettaglioPagamento(omydettaglio, 2) <= 0 Then
                        strErrore = "Si è verificato un\'errore nella cancellazione del dettaglio!"
                        sScript = "GestAlert('a', 'warning', '', '', '" & strErrore & "');"
                        RegisterScript(sScript, Me.GetType())
                    End If
                    If FncPagamenti.SetPagamento(oMyPagamentoOLD, 4) <= 0 Then
                        strErrore = "Si è verificato un\'errore nella cancellazione del dettaglio!"
                        sScript = "GestAlert('a', 'warning', '', '', '" & strErrore & "');"
                        RegisterScript(sScript, Me.GetType())
                    End If
                    'se il richiamo al dettaglio dei pagamenti attraverso la funzione DettagliaPagamenti(sEnteImport, sOperatore, WFSessione) è negativo:
                    If FncPagamenti.DettagliaPagamenti(ConstSession.IdEnte, ConstSession.UserName) <= 0 Then
                        strErrore = "Si è verificato un\'errore in inserimento dettaglio!"
                        sScript = "GestAlert('a', 'warning', '', '', '" & strErrore & "');"
                        RegisterScript(sScript, Me.GetType())
                        sScript = "UscitaDopoOperazione();"
                        RegisterScript(sScript, Me.GetType())
                    Else
                        sScript = "GestAlert('a', 'success', '', '', 'Modifica effettuata correttamente!'); parent.Search();"
                        RegisterScript(sScript, Me.GetType())
                        sScript = "ConfermaUscita();"
                        RegisterScript(sScript, Me.GetType())
                    End If
                Else
                    strErrore = "Si è verificato un\'errore!"
                    sScript = "GestAlert('a', 'warning', '', '', '" & strErrore & "');"
                    RegisterScript(sScript, Me.GetType())
                    sScript = "UscitaDopoOperazione();"
                    RegisterScript(sScript, Me.GetType())
                End If
            Else
                oMyPagamento.IDEnte = ConstSession.IdEnte
                oMyPagamento.sProvenienza = "DE"
                oMyPagamento.sSegno = "+"
                oMyPagamento.sDataFattura = txtDataFattura.Text
                oMyPagamento.ImportoPagamento = txtImportoPagamento.Text
                oMyPagamento.sDataPagamento = txtDataPagamento.Text
                oMyPagamento.sNumeroFattura = txtNfattura.Text
                oMyPagamento.nCodUtente = CodUtente
                oMyPagamento.sDataAccredito = txtDataAccredito.Text

                If FncPagamenti.SetPagamento(oMyPagamento, 0) > 0 Then
                    'se il richiamo al dettaglio dei pagamenti attraverso la funzione DettagliaPagamenti(sEnteImport, sOperatore, WFSessione) è negativo:
                    If FncPagamenti.DettagliaPagamenti(ConstSession.IdEnte, ConstSession.UserName) <= 0 Then
                        strErrore = "Si è verificato un\'errore in inserimento dettaglio!"
                        sScript = "GestAlert('a', 'warning', '', '', '" & strErrore & "');"
                        RegisterScript(sScript, Me.GetType())
                        sScript = "UscitaDopoOperazione();"
                        RegisterScript(sScript, Me.GetType())
                    Else
                        sScript = "GestAlert('a', 'success', '', '', 'Inserimento effettuato correttamente!'); parent.Search();"
                        RegisterScript(sScript, Me.GetType())
                        sScript = "ConfermaUscita();"
                        RegisterScript(sScript, Me.GetType())
                    End If
                Else
                    strErrore = "Si è verificato un\'errore!"
                    sScript = "GestAlert('a', 'warning', '', '', '" & strErrore & "');"
                    RegisterScript(sScript, Me.GetType())
                    sScript = "UscitaDopoOperazione();"
                    RegisterScript(sScript, Me.GetType())
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.InserimentoPagamenti.BtnSalva_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub BtnElimina_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnElimina.Click
        Dim ClsPagamenti As New ClsPagamenti
        Dim sScript As String = ""
        Dim strErrore As String =""
        Dim ObjPag As New OggettoPagamento
        Try
            ObjPag.ID = IdPagamento

            If ClsPagamenti.DeletePagamentiEnte(ObjPag, strErrore) = True Then
                sScript = "GestAlert('a', 'success', '', '', 'Cancellazione effettuata correttamente!'); parent.Search();"
                RegisterScript(sScript, Me.GetType())
                sScript = "UscitaDopoOperazione();"
                RegisterScript(sScript, Me.GetType())
            Else
                sScript = "GestAlert('a', 'warning', '', '', '" & strErrore & "');"
                RegisterScript(sScript, Me.GetType())
                sScript = "UscitaDopoOperazione();"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.InserimentoPagamenti.BtnElimina_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
