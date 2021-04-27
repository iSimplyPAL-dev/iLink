Imports Anagrafica.DLL
Imports AnagInterface
Imports log4net
''' <summary>
''' Pagina per la gestione di accorpamento/rateizzazione.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class Dettaglio
    Inherits BasePage

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblInfoRate As System.Web.UI.WebControls.Label
    Protected WithEvents btnNuovoPagamento As System.Web.UI.WebControls.Button

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(Dettaglio))
   
    Private WFErrore As String
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim objDS As DataSet
        Dim sScript As String = ""
        Try
            If Not Page.IsPostBack Then
                If Not Request.Item("IdAccorpamento") Is Nothing Then
                    hfIdAccorpamento.Value = Request.Item("IdAccorpamento")
                End If
                If Not Request.Item("IdProvvedimento") Is Nothing Then
                    hfIdProvvedimento.Value = Request.Item("IdProvvedimento")
                End If
                If Not Request.Item("IdContribuente") Is Nothing Then
                    hdIdContribuente.Value = Request.Item("IdContribuente")
                End If
                CaricaDettaglio(ConstSession.IdEnte)
                '*** 201507 - GESTIONE INCROCIATA RIFIUTI/ICI & DIVERSA GESTIONE QUOTA VARIABILE***
                If ConstSession.HasPlainAnag Then
                    sScript += "document.getElementById('TRSpecAnag').style.display='none';"
                Else
                    sScript += "document.getElementById('TRPlainAnag').style.display='none';"
                End If
                RegisterScript(sScript, Me.GetType())
                '*** ***
            Else
                If Not IsNothing(Session("ProvvedimentiAccorpamento")) Then
                    objDS = CType(Session("ProvvedimentiAccorpamento"), DataSet)
                    GrdProvvedimenti.DataSource = objDS.Tables(0).DefaultView
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Dettaglio.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sIdEnte"></param>
    ''' <returns></returns>
    Function CaricaDettaglio(ByVal sIdEnte As String) As Boolean
        Dim bRet As Boolean = False
        Try
            'Visualizzazione anagrafica
            GetAnagrafica(hdIdContribuente.Value)
            'Visualizzazione provvedimenti da accorpamento
            If GetProvvedimentiAccorpamento(hfIdAccorpamento.Value, hfIdProvvedimento.Value) Then
                bRet = True
            End If
            'Visualizzazione rate accorpamento
            Dim wucRate As Provvedimenti.usercontrol.WUCRate
            wucRate = Page.FindControl("ElencoRate")
            'permette di andare in modifica pagamenti direttamente dalla griglia
            wucRate.InserisciPagamento = True
            wucRate.IdContribuente = hdIdContribuente.Value

            If hfIdProvvedimento.Value <> "0" Then
                wucRate.id_provvedimento = hfIdProvvedimento.Value
                wucRate.getRateProvvedimento(sIdEnte)
            End If
            If hfIdAccorpamento.Value <> "0" Then
                wucRate.id_accorpamento = hfIdAccorpamento.Value
                wucRate.getRateAccorpamento(sIdEnte)
            End If

            'nasconde il pulsante di cancellazione pagamento
            'wucRate.viewDelete = False

            Return bRet
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Dettaglio.CaricaDettaglio.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw New Exception(ex.Message, ex)
            Return False
        End Try
    End Function

    '
    ''' <summary>
    ''' Reperimento dati Provvedimenti
    ''' </summary>
    ''' <param name="id_accorpamento"></param>
    ''' <param name="id_provvedimento"></param>
    ''' <returns></returns>
    Function GetProvvedimentiAccorpamento(ByVal id_accorpamento As Integer, ByVal id_provvedimento As Integer) As Boolean
        Dim objPagamenti As clsPagamenti
        Dim objDS As DataSet
        Try
            objPagamenti = New clsPagamenti(ConstSession.StringConnection) 'clsPagamenti(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))
            objDS = objPagamenti.getProvvedimentiAccorpamento(id_accorpamento, id_provvedimento)
            Session("ProvvedimentiAccorpamento") = objDS
            If Not IsNothing(objDS) Then
                If objDS.Tables(0).Rows.Count > 0 Then
                    GrdProvvedimenti.SelectedIndex = -1
                    GrdProvvedimenti.DataSource = objDS.Tables(0).DefaultView
                    grdProvvedimenti.DataBind()
                    lblInfoProvv.Visible = False
                    grdProvvedimenti.Visible = True
                    Return True
                Else
                    lblInfoProvv.Text = "Nessun Provvedimento trovato"
                    lblInfoProvv.Visible = True
                    grdProvvedimenti.Visible = False
                    Return False
                End If
            Else
                lblInfoProvv.Text = "Nessun Provvedimento trovato"
                lblInfoProvv.Visible = True
                grdProvvedimenti.Visible = False
                Return False
            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Dettaglio.GetProvvedimentiAccorpamento.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw New Exception(ex.Message, ex)
            Return False
        Finally
            objPagamenti.kill()
            objPagamenti = Nothing
            objDS.Dispose()
        End Try
    End Function
    '
    ''' <summary>
    ''' Reperimento dati anagrafici
    ''' </summary>
    ''' <param name="cod_contribuente"></param>
    Private Sub GetAnagrafica(ByVal cod_contribuente As Integer)
        Dim oAnagrafica As New GestioneAnagrafica
        Dim oDettaglioAnagrafica As New DettaglioAnagrafica
        Try
            '*** 201504 - Nuova Gestione anagrafica con form unico ***
            If ConstSession.HasPlainAnag Then
                ifrmAnag.Attributes.Add("src", "../../Generali/asp/VisualAnag.aspx?IdContribuente=" + cod_contribuente.ToString() + "&Azione=" + Utility.Costanti.AZIONE_NEW.ToString())
            Else
                oDettaglioAnagrafica = oAnagrafica.GetAnagrafica(cod_contribuente, -1, String.Empty, ConstSession.DBType, ConstSession.StringConnectionAnagrafica, False)

            lblCognomeNome.Text = oDettaglioAnagrafica.Cognome + " " + oDettaglioAnagrafica.Nome
            lblCfPiva.Text = oDettaglioAnagrafica.CodiceFiscale + " " + oDettaglioAnagrafica.PartitaIva
            lblSesso.Text = oDettaglioAnagrafica.Sesso
            lblDataNascita.Text = oDettaglioAnagrafica.DataNascita
            lblComuneNascita.Text = oDettaglioAnagrafica.ComuneNascita
                lblResidenza.Text = oDettaglioAnagrafica.ViaResidenza + " " + oDettaglioAnagrafica.CivicoResidenza + " " + oDettaglioAnagrafica.ComuneResidenza + " (" + oDettaglioAnagrafica.ProvinciaResidenza + ")"
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Dettaglio.GetAnagrafica.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw New Exception(ex.Message, ex)
        Finally
            'WFSessione.Kill()
            oAnagrafica = Nothing
            oDettaglioAnagrafica = Nothing
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="objtemp"></param>
    ''' <returns></returns>
    Protected Function annoBarra(ByVal objtemp As Object) As String
        Dim clsGeneralFunction As New MyUtility
        Dim strTemp As String = ""
        Try
            If Not IsDBNull(objtemp) Then
                If CStr(objtemp).CompareTo("") <> 0 Then
                    strTemp = clsGeneralFunction.GiraDataFromDB(objtemp)
                Else
                    strTemp = ""
                End If
            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Dettaglio.annoBarra.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
        Return strTemp
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="cod_tributo"></param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="04/07/2012">
    ''' <strong>IMU</strong>
    ''' passaggio tributo da ICI a IMU
    ''' </revision>
    ''' </revisionHistory>
    Protected Function Tributo(ByVal cod_tributo As Object) As String
        '*** 20120704 - IMU ***
        Dim strTemp As String = ""
        Try
            If Not IsDBNull(cod_tributo) Then
                Select Case Utility.StringOperation.FormatString(cod_tributo)
                    Case Utility.Costanti.TRIBUTO_ICI : strTemp = "ICI/IMU"
                    Case Utility.Costanti.TRIBUTO_TARSU : strTemp = "TARSU/TARES"
                    Case Utility.Costanti.TRIBUTO_H2O : strTemp = "H2O"
                    Case Utility.Costanti.TRIBUTO_OSAP : strTemp = "TOSAP/COSAP"
                    Case "0634" : strTemp = "TIA"
                    Case Else : strTemp = ""
                End Select
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Dettaglio.Tributo.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
        Return strTemp
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="NumeroDaFormattareParam"></param>
    ''' <param name="numDec"></param>
    ''' <returns></returns>
    Protected Function FormattaNumero(ByVal NumeroDaFormattareParam As Object, ByVal numDec As Integer) As String
        FormattaNumero = ""
        Try
            If IsDBNull(NumeroDaFormattareParam) Then
                NumeroDaFormattareParam = ""
            ElseIf NumeroDaFormattareParam.ToString() = "" Or NumeroDaFormattareParam.ToString() = "-1" Or NumeroDaFormattareParam.ToString() = "-1,00" Then
                NumeroDaFormattareParam = ""
            Else
                FormattaNumero = FormatNumber(NumeroDaFormattareParam, numDec)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Dettaglio.FormattaNumero.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw New Exception(ex.Message, ex)
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnEliminaAccorpamento_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEliminaAccorpamento.Click
        Dim detthashtable As Hashtable
        Dim id_accorpamento, cod_contribuente, id_provvedimento As Integer
        Dim objPagamenti As clsPagamenti
        dim sScript as string=""
        Try
            objPagamenti = New clsPagamenti(ConstSession.StringConnection) 'clsPagamenti(Session("PARAMETROENV"), ConstSession.UserName, Session("IDENTIFICATIVOAPPLICAZIONE"))

            detthashtable = Session("detthashtable")
            id_accorpamento = detthashtable("id_accorpamento")
            cod_contribuente = detthashtable("cod_contribuente")
            id_provvedimento = detthashtable("id_provvedimento")

            If objPagamenti.haSpesePagate(id_accorpamento, New Double) Then
                'esistono già dei pagamenti non è possibile eliminare l'accorpamento
                If id_provvedimento <> 0 Then
                    sScript += "GestAlert('a', 'warning', '', '', 'Non è possibile eliminare il pagamento')"
                Else
                    sScript += "GestAlert('a', 'warning', '', '', 'Non è possibile eliminare l\'accorpamento')"
                End If
                RegisterScript(sScript, Me.GetType())
            Else
                If Not objPagamenti.deleteAccorpamento(id_accorpamento) Then
                    Throw New Exception("Errore cancellazione accorpamento")
                End If

                If Not CaricaDettaglio(ConstSession.IdEnte) Then
                    'nascondi pulsanti inserimento e cancellazione
                    sScript += "Abilita_btnDelete('none');Abilita_btnPagamento('none');"
                End If
                sScript += "GestAlert('a', 'success', '', '', 'Cancellazione effettuata correttamente');"
                RegisterScript(sScript , Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Dettaglio.btnEliminaAccorpamento_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw New Exception(ex.Message, ex)
        Finally
            objPagamenti.kill()
            objPagamenti = Nothing
        End Try
    End Sub
End Class
