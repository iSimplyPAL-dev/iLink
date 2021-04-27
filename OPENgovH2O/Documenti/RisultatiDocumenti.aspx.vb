Imports Anagrafica
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports Utility
Imports log4net

Partial Class RisultatiDocumenti
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(RisultatiDocumenti))
    Private clsElabDoc As New ClsElaborazioneDocumenti
    Protected FncGrd As New ClsGenerale.FunctionGrd

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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim sNumeroFattura As String
        Dim sNominativoDa As String
        Dim sNominativoA As String
        Dim DataFattura As String
        Dim IdFlusso As Integer
        Dim sScript As String
        '********************************************************
        'eseguo la ricerca e carico la griglia
        '********************************************************
        Dim ListFatture() As ObjAnagDocumenti
        Dim classeRuoli As New ClsRuoloH2O

        Try
            '********************************************************
            'leggo i parametri di ricerca
            '********************************************************
            sNumeroFattura = Request.Item("NumeroFattura")
            DataFattura = Request.Item("DataFattura")
            sNominativoDa = Request.Item("NominativoDa")
            sNominativoA = Request.Item("NominativoA")
            IdFlusso = Request.Item("IdRuolo")
            If Session("NumDocPerFile") Is Nothing Then
                If IsNumeric(txtNumDoc.Text) Then
                    Session("NumDocPerFile") = txtNumDoc.Text
                End If
            End If
            If chkElaboraBollettini.Checked Then
                Session("ElaboraBollettini") = True
            Else
                Session("ElaboraBollettini") = False
            End If
            If optTD123.Checked Then
                Session("tipobollettino") = "123"
            End If
            If optTD896.Checked Then
                Session("tipobollettino") = "896"
            End If
            If optF24.Checked Then
                Session("tipobollettino") = "F24"
            End If
            Session("OrdinamentoDoc") = 1

            If Page.IsPostBack = False Then

                ViewState("OrderBy") = TipoOrdinamento.Crescente
                '********************************************************
                'caricamento griglia con le cartella
                '********************************************************
                'dalla query ottengo un'array di oggetti di tipo ObjTotRuolo
                'che assegno direttamente al datasource della griglia
                '********************************************************
                ListFatture = classeRuoli.GetCartelleContribDaElaborare(IdFlusso, 0, sNumeroFattura, DataFattura, sNominativoDa, sNominativoA)
                '********************************************************
                ' ordino per anno 
                '********************************************************
                Dim objComparer As New Comparatore(New String() {"sCognome", "sNome"}, New Boolean() {ViewState("OrderBy"), ViewState("OrderBy")})
                If Not ListFatture Is Nothing Then
                    Array.Sort(ListFatture, objComparer)
                    '********************************************************
                    'carico in una variabile di sessione l'array di oggetti in modo
                    'da non dover rieseguire la query tutte le volte che viene ricaricata la pagina
                    '********************************************************
                    GrdFattura.DataSource = ListFatture
                    Session.Add("ListFatture", ListFatture)

                    GrdFattura.DataBind()
                End If

                '********************************************************
                'caricamento griglia con le i totali del dettaglio - senza ordinamento
                '********************************************************
                ''ListTotDettaglioFatture = classeRuoli.GetTotaliDettaglioCartelle(IdFlusso, sCodiceCartella, sNominativoDa, sNominativoA)
                ''GrdTotaliDettaglio.start_index = 0
                ''If Not ListTotDettaglioFatture Is Nothing Then
                ''    '********************************************************
                ''    'carico in una variabile di sessione l'array di oggetti in modo
                ''    'da non dover rieseguire la query tutte le volte che viene ricaricata la pagina
                ''    '********************************************************
                ''    GrdTotaliDettaglio.DataSource = ListTotDettaglioFatture
                ''    Session.Add("ListTotDettaglioFatture", ListTotDettaglioFatture)

                ''    GrdTotaliDettaglio.DataBind()
                ''End If

                If Session("ElaboraDocumenti") Is Nothing Then
                    If optProve.Checked = True Then
                        Session("ElaboraDocumenti") = 1
                    Else
                        Session("ElaboraDocumenti") = 2
                    End If
                Else
                    If Session("ElaboraDocumenti") = 1 Then
                        optProve.Checked = True
                        optEffettivo.Checked = False
                        chkElaboraTutti.Visible = False
                    Else
                        optProve.Checked = False
                        optEffettivo.Checked = True
                        chkElaboraTutti.Visible = True
                    End If
                End If

                If Not Session("OrdinamentoDoc") Is Nothing Then
                    If Session("OrdinamentoDoc") = 1 Then
                        optNominativo.Checked = True
                        optIndirizzo.Checked = False
                    Else
                        optNominativo.Checked = False
                        optIndirizzo.Checked = True
                    End If
                End If

                If Session("nMaxDocPerFile") Is Nothing Then
                    If IsNumeric(txtNumDoc.Text) Then
                        Session("nMaxDocPerFile") = txtNumDoc.Text
                    End If
                End If

                'If Not Session("chkElaboraTutti") Is Nothing Then
                '    chkElaboraTutti.Checked = CType(Session("chkElaboraTutti"), Boolean)
                'Else
                '    chkElaboraTutti.Checked = False
                'End If
                sScript = "parent.parent.Visualizza.DivAttesa.style.display='none';"
                RegisterScript(sScript, Me.GetType())
            Else
                ControllaCheckbox()
                '********************************************************
                'assegno al datasource la variabile di sessione precedentemente caricata
                '********************************************************
                'If Not Session("ListFatture") Is Nothing Then
                '    GrdFattura.DataSource = CType(Session("ListFatture"), ObjAnagDocumenti())
                '    GrdFattura.DataBind()
                'End If
                '********************************************************
                'assegno al datasource la variabile di sessione precedentemente caricata
                '********************************************************
                'If Not Session("ListTotDettaglioFatture") Is Nothing Then
                '    GrdTotaliDettaglio.DataSource = CType(Session("ListTotDettaglioFatture"), ListTotDettaglioFatture())
                '    GrdTotaliDettaglio.DataBind()
                'End If
            End If

            Select Case CInt(GrdFattura.Rows.Count)
                Case 0
                    GrdFattura.Visible = False
                    'lblMessage.Text = "Non sono state trovati ruoli da elaborare."
                Case Is > 0
                    GrdFattura.Visible = True
            End Select

            Select Case CInt(GrdTotaliDettaglio.Rows.Count)
                Case 0
                    GrdTotaliDettaglio.Visible = False
                    'lblMessage.Text = "Non sono state trovati ruoli da elaborare."
                Case Is > 0
                    GrdTotaliDettaglio.Visible = True
            End Select

        Catch err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RisultatiDocumenti.Page_Load.errore: ", err)
            Response.Redirect("../../PaginaErrore.aspx")
            'Response.Write(GestError.GetHTMLError(err, Server.MapPath("/" & Application("nome_sito") & "/ERRORIANAGRAFICA.css"), "history.back()"))
            'Response.End()
        End Try
    End Sub

    Private Sub ControllaCheckbox()
        Dim itemGrid As GridViewRow
        Dim oArrayOggettoFatture() As ObjAnagDocumenti
        Dim x, nSel As Integer

        Try
            nSel = 0
            oArrayOggettoFatture = CType(Session("ListFatture"), ObjAnagDocumenti())
            For Each itemGrid In GrdFattura.Rows
                'prendo l'ID da aggiornare
                For x = 0 To oArrayOggettoFatture.GetUpperBound(0)
                    'confronto il codice cartella per individuare l'elemento all'interno della griglia
                    If oArrayOggettoFatture(x).sNDocumento = CType(itemGrid.Cells(5).Text, String) Then
                        oArrayOggettoFatture(x).Selezionato = CType(itemGrid.FindControl("ChkSelezionato"), CheckBox).Checked
                        If oArrayOggettoFatture(x).Selezionato = True Then
                            nSel += 1
                        End If
                    End If
                Next
            Next
            Session("ListFatture") = oArrayOggettoFatture
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RisultatiDocumenti.ControllaCheckbox.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub optProve_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optProve.CheckedChanged
        Try
            If optProve.Checked = True Then
                Session("ElaboraDocumenti") = 1
                chkElaboraTutti.Checked = False : chkSendMail.Checked = False
                chkElaboraTutti.Visible = False : chkSendMail.Visible = False
                chkElaboraTutti_CheckedChanged(sender, e)
            Else
                Session("ElaboraDocumenti") = 2
                chkElaboraTutti.Visible = True : chkSendMail.Visible = True
            End If
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RisultatiDocumenti.optProve_CheckedChanged.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

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
                chkElaboraTutti.Checked = False : chkSendMail.Checked = False
                chkElaboraTutti.Visible = False : chkSendMail.Visible = False
                chkElaboraTutti_CheckedChanged(sender, e)
            End If
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RisultatiDocumenti.optEffettivo_CheckedChanged.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

#Region "Griglie"
    ''' <summary>
    ''' Gestione del cambio pagina della griglia.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdPageIndexChanging(ByVal sender As Object, e As GridViewPageEventArgs)
        LoadSearch(e.NewPageIndex)
        If IsNumeric(txtNumDoc.Text) Then
            Session("nMaxDocPerFile") = txtNumDoc.Text
        End If
    End Sub
    'Private Sub GrdFattura_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles GrdFattura.SortCommand
    '   Try
    '    Dim oArrayOggettoFattura() As ObjAnagDocumenti

    '    oArrayOggettoFattura = CType(Session("ListFatture"), ObjAnagDocumenti())

    '    If ViewState("OrderBy") = TipoOrdinamento.Crescente Then
    '        ViewState("OrderBy") = TipoOrdinamento.Decrescente
    '    Else
    '        ViewState("OrderBy") = TipoOrdinamento.Crescente
    '    End If

    '    Dim objComparer As New Comparatore(New String() {"Cognome", "Nome"}, New Boolean() {ViewState("OrderBy"), ViewState("OrderBy")})
    '    Array.Sort(oArrayOggettoFattura, objComparer)

    '    GrdFattura.DataSource = oArrayOggettoFattura
    '  Catch Err As Exception

    '   Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RisultatiDocumenti.GrdFattura_SortCommand.errore: ", Err)
    '  Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdFattura.DataSource = CType(Session("ListFatture"), ObjAnagDocumenti())
            If page.HasValue Then
                GrdFattura.PageIndex = page.Value
            End If
            GrdFattura.DataBind()
        Catch ex As Exception


            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RisultatiDocumenti.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub

    Private Sub optNominativo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optNominativo.CheckedChanged
        If optNominativo.Checked = True Then
            Session("OrdinamentoDoc") = 1
        Else
            Session("OrdinamentoDoc") = 0
        End If
    End Sub

    Private Sub optIndirizzo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optIndirizzo.CheckedChanged
        If optIndirizzo.Checked = True Then
            Session("OrdinamentoDoc") = 0
        Else
            Session("OrdinamentoDoc") = 1
        End If
    End Sub

    Private Sub chkElaboraTutti_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkElaboraTutti.CheckedChanged
        Dim itemGrid As GridViewRow
        Dim oArrayOggettoFattura() As ObjAnagDocumenti
        Dim x, nSel As Integer
        Try
            If chkElaboraTutti.Checked = True Then
                '*** 20140509 - TASI ***
                chkSendMail.Visible = True : chkSendMail.Checked = True : Session("bSendByMail") = True
                '*** ***
                '*****************************************************
                'seleziono tutti gli elementi
                '*****************************************************
                oArrayOggettoFattura = CType(Session("ListFatture"), ObjAnagDocumenti())
                If Not oArrayOggettoFattura Is Nothing Then
                    For x = 0 To oArrayOggettoFattura.Length - 1
                        oArrayOggettoFattura(x).Selezionato = True
                    Next
                    For Each itemGrid In GrdFattura.Rows
                        CType(itemGrid.FindControl("ChkSelezionato"), CheckBox).Checked = True
                    Next
                End If
                Session("ListFatture") = oArrayOggettoFattura
                'Session("chkElaboraTutti") = True
            Else
                nSel = 0
                oArrayOggettoFattura = CType(Session("ListFatture"), ObjAnagDocumenti())
                If Not oArrayOggettoFattura Is Nothing Then
                    For x = 0 To oArrayOggettoFattura.Length - 1
                        oArrayOggettoFattura(x).Selezionato = False
                    Next
                    For Each itemGrid In GrdFattura.Rows
                        CType(itemGrid.FindControl("ChkSelezionato"), CheckBox).Checked = False
                    Next
                End If
                Session("ListFatture") = oArrayOggettoFattura
            End If
        Catch Err As Exception


            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RisultatiDocumenti.chkElaboraTutti_CheckedChanged.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    '*** 20140509 - TASI ***
    Private Sub chkSendMail_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSendMail.CheckedChanged
        Try
            If chkSendMail.Checked = True Then
                Session("bSendByMail") = True
            Else
                Session("bSendByMail") = False
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RisultatiDocumenti.chkSendMail_CheckedChanged.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    '*** ***

    'Public Function FormattaDataGrd(ByVal tDataGrd As DateTime) As String
    '    Try
    '        If tDataGrd = Date.MinValue Or tDataGrd = Date.MaxValue Then
    '            Return ""
    '        Else
    '            Return tDataGrd.ToShortDateString
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RisultatiDocumenti.FormattaDataGrdi.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Function

    'Protected Function EuroForGridView(ByVal iInput As Object) As String
    '    Dim ret As String
    '    ret = String.Empty
    '    Try
    '        If (iInput.ToString() = "-1") Or (iInput.ToString() = "-1,00") Then

    '            ret = String.Empty
    '        Else

    '            ret = Convert.ToDecimal(iInput).ToString("N")
    '        End If
    '        Return ret
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.RisultatiDocumenti.EuroForGridView.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Function


    Private Sub txtNumDoc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNumDoc.TextChanged
        If IsNumeric(txtNumDoc.Text) Then
            Session("nMaxDocPerFile") = txtNumDoc.Text
        End If
    End Sub

    Private Sub optF24_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optF24.CheckedChanged
        Try
            If optTD123.Checked Then
                Session("tipobollettino") = "123"
            End If
            If optTD896.Checked Then
                Session("tipobollettino") = "896"
            End If
            If optF24.Checked Then
                Session("tipobollettino") = "F24"
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RisultatiDocumenti.optF24_CheckedChanged.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub optTD123_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optTD123.CheckedChanged
        Try
            If optTD123.Checked Then
                Session("tipobollettino") = "123"
            End If
            If optTD896.Checked Then
                Session("tipobollettino") = "896"
            End If
            If optF24.Checked Then
                Session("tipobollettino") = "F24"
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RisultatiDocumenti.optTD123_CheckedChanged.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub optTD896_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles optTD896.CheckedChanged
        Try
            If optTD123.Checked Then
                Session("tipobollettino") = "123"
            End If
            If optTD896.Checked Then
                Session("tipobollettino") = "896"
            End If
            If optF24.Checked Then
                Session("tipobollettino") = "F24"
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.RisultatiDocumenti.optTD896_CheckedChanged.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
