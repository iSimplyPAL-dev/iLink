Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports log4net

Partial Class ConfiguraQuotaFissa
    Inherits BasePage

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
    Public IdQuotaFissa, IdTU, Da, A, Anno, Iva, TariffaH2O, TariffaDep, TariffaFog, Operazione As String
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ConfiguraQuotaFissa))

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim ClsGenerale As New ClsGenerale.Generale
        IdQuotaFissa = Request.Item("IdQuotaFissa")
        IdTU = Request.Item("IdTU")
        Da = Request.Item("Da")
        A = Request.Item("A")
        TariffaH2O = Request.Item("TariffaH2O")
        TariffaDep = Request.Item("TariffaDep")
        TariffaFog = Request.Item("TariffaFog")
        Iva = Request.Item("Iva")
        Anno = Request.Item("Anno")
        Operazione = Request.Item("Operazione")
        Try
            If Not Page.IsPostBack Then
                IdQuotaFissa = Request.Item("IdQuotaFissa")
                IdTU = Request.Item("IdTU")
                Da = Request.Item("Da")
                A = Request.Item("A")
                If Not Request.Item("TariffaH2O") Is Nothing Then
                    txtTariffaH2O.Text = New ClsGenerale.FunctionGrd().euroforgridview(Request.Item("TariffaH2O"))
                End If
                If Not Request.Item("TariffaDep") Is Nothing Then
                    txtTariffaDep.Text = New ClsGenerale.FunctionGrd().euroforgridview(Request.Item("TariffaDep"))
                End If
                If Not Request.Item("TariffaFog") Is Nothing Then
                    txtTariffaFog.Text = New ClsGenerale.FunctionGrd().euroforgridview(Request.Item("TariffaFog"))
                End If
                If Not Request.Item("Iva") Is Nothing Then
                    txtIva.Text = New ClsGenerale.FunctionGrd().euroforgridview(Request.Item("Iva"))
                End If
                txtAnno.Text = Request.Item("Anno")
                txtDa.Text = Request.Item("Da")
                txtA.Text = Request.Item("A")

                If IsNothing(Operazione) = True Then
                    lblDescrizioneOperazione.Text = "Dati " + CType(ConstSession.DescrTipoProcServ, String).Replace("Tabella", "") + " - Inserimento " + CType(ConstSession.DescrTipoProcServ, String).Replace("Tabella", "")
                Else
                    lblDescrizioneOperazione.Text = "Dati " + CType(ConstSession.DescrTipoProcServ, String).Replace("Tabella", "") + " - Modifica " + CType(ConstSession.DescrTipoProcServ, String).Replace("Tabella", "")
                End If

                Dim ClsCanoni As New ClsCanoni
                If Anno <> "" Then
                    ClsCanoni.LoadComboUtenze(ddlTipoUtenza, Anno)
                Else
                    ClsCanoni.LoadComboUtenze(ddlTipoUtenza, "")
                End If


                ddlTipoUtenza.SelectedValue = IdTU
                If Operazione = "modifica" Then
                    'ddlTipoUtenza.Enabled = False
                End If
            End If
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ConfiguraQuotaFissa.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub BtnSalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSalva.Click
        Dim ClsQuotaFissa As New ClsQuotaFissa
        Dim sScript As String
        Dim strErrore As String=""
        Dim objQuotaFissa As New OggettoQuotaFissa
        Dim objQuotaFissaOld As New OggettoQuotaFissa
        Try
            If Operazione = "modifica" Then
                objQuotaFissa.ID = IdQuotaFissa
                objQuotaFissa.idTipoUtenza = ddlTipoUtenza.SelectedValue
                objQuotaFissa.dAliquota = txtIva.Text
                If txtTariffaH2O.Text <> "" And IsNumeric(txtTariffaH2O.Text) Then
                    objQuotaFissa.dImportoH2O = txtTariffaH2O.Text
                Else
                    objQuotaFissa.dImportoH2O = 0
                End If
                If txtTariffaFog.Text <> "" And IsNumeric(txtTariffaFog.Text) Then
                    objQuotaFissa.dImportoFog = txtTariffaFog.Text
                Else
                    objQuotaFissa.dImportoFog = 0
                End If
                If txtTariffaDep.Text <> "" And IsNumeric(txtTariffaDep.Text) Then
                    objQuotaFissa.dImportoDep = txtTariffaDep.Text
                Else
                    objQuotaFissa.dImportoDep = 0
                End If
                objQuotaFissa.sAnno = txtAnno.Text
                objQuotaFissa.DA = txtDa.Text
                objQuotaFissa.A = txtA.Text

                objQuotaFissaOld.sAnno = Anno
                objQuotaFissaOld.ID = IdQuotaFissa
                If ClsQuotaFissa.UpdateQuotaFissaEnte(ConstSession.IdEnte, objQuotaFissaOld, objQuotaFissa, strErrore) = True Then
                    sScript = "GestAlert('a', 'success', '', '', 'Modifica effettuata correttamente!'); parent.Search();"
                    RegisterScript(sScript, Me.GetType())
                    sScript = "UscitaDopoOperazione();"
                    RegisterScript(sScript, Me.GetType())
                Else
                    sScript = "GestAlert('a', 'warning', '', '', '" & strErrore & "');"
                    RegisterScript(sScript, Me.GetType())
                    sScript = "UscitaDopoOperazione();"
                    RegisterScript(sScript, Me.GetType())
                End If
            Else
                objQuotaFissa.idTipoUtenza = ddlTipoUtenza.SelectedValue
                objQuotaFissa.dAliquota = txtIva.Text
                If txtTariffaH2O.Text <> "" And IsNumeric(txtTariffaH2O.Text) Then
                    objQuotaFissa.dImportoH2O = txtTariffaH2O.Text
                Else
                    objQuotaFissa.dImportoH2O = 0
                End If
                If txtTariffaFog.Text <> "" And IsNumeric(txtTariffaFog.Text) Then
                    objQuotaFissa.dImportoFog = txtTariffaFog.Text
                Else
                    objQuotaFissa.dImportoFog = 0
                End If
                If txtTariffaDep.Text <> "" And IsNumeric(txtTariffaDep.Text) Then
                    objQuotaFissa.dImportoDep = txtTariffaDep.Text
                Else
                    objQuotaFissa.dImportoDep = 0
                End If
                objQuotaFissa.sAnno = txtAnno.Text
                objQuotaFissa.DA = txtDa.Text
                objQuotaFissa.A = txtA.Text
                If ClsQuotaFissa.SetQuotaFissaEnte(objQuotaFissa, strErrore) = True Then
                    sScript = "GestAlert('a', 'success', '', '', 'Inserimento effettuato correttamente!'); parent.Search();"
                    RegisterScript(sScript, Me.GetType())
                    sScript = "UscitaDopoOperazione();"
                    RegisterScript(sScript, Me.GetType())
                Else
                    sScript = "GestAlert('a', 'warning', '', '', '" & strErrore & "');"
                    RegisterScript(sScript, Me.GetType())
                    sScript = "UscitaDopoOperazione();"
                    RegisterScript(sScript , Me.GetType())
                End If
            End If
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ConfiguraQuotaFissa.BtnSalva_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub BtnElimina_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnElimina.Click
        Dim ClsQuotaFissa As New ClsQuotaFissa
        Dim sScript As String
        Dim strErrore As String=""
        Dim objQuotaFissa As New OggettoQuotaFissa

        objQuotaFissa.ID = IdQuotaFissa
        Try
            If ClsQuotaFissa.DeleteQuotaFissaEnte(objQuotaFissa, strErrore) = True Then
                sScript = "GestAlert('a', 'success', '', '', 'Cancellazione effettuata correttamente!'); parent.Search();"
                RegisterScript(sScript, Me.GetType())
                sScript = "UscitaDopoOperazione();"
                RegisterScript(sScript, Me.GetType())
            Else
                sScript = "GestAlert('a', 'warning', '', '', '" & strErrore & "');"
                RegisterScript(sScript, Me.GetType())
                sScript = "UscitaDopoOperazione();"
                RegisterScript(sScript , Me.GetType())
            End If
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ConfiguraQuotaFissa.BtnElimina_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub txtAnno_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAnno.TextChanged
        Dim ClsCanoni As New ClsCanoni
        ClsCanoni.LoadComboUtenze(ddlTipoUtenza, txtAnno.Text)
    End Sub
End Class
