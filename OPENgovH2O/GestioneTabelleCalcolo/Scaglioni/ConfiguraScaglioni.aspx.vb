Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports log4net

Partial Class ConfiguraScaglioni
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ConfiguraScaglioni))
    Public IdScaglione, IdTU, Da, A, Anno, Minimo, Iva, Tariffa, Operazione As String
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
        Try
            Dim clsGenerale As New ClsGenerale.Generale
            'Put user code to initialize the page here
            IdScaglione = Request.Item("IdScaglione")
            IdTU = Request.Item("IdTU")
            Da = Request.Item("Da")
            A = Request.Item("A")
            Tariffa = Request.Item("Tariffa")
            Iva = Request.Item("Iva")
            Anno = Request.Item("Anno")
            Minimo = Request.Item("Minimo")
            Operazione = Request.Item("Operazione")
            If Not Page.IsPostBack Then
                IdScaglione = Request.Item("IdScaglione")
                IdTU = Request.Item("IdTU")
                Da = Request.Item("Da")
                A = Request.Item("A")
                txtTariffa.Text = Request.Item("Tariffa")
                If Not Request.Item("Iva") Is Nothing Then
                    txtIva.Text = New ClsGenerale.FunctionGrd().euroforgridview(Request.Item("Iva"))
                End If
                txtAnno.Text = Request.Item("Anno")
                If Not Request.Item("Minimo") Is Nothing Then
                    txtMinimo.Text = New ClsGenerale.FunctionGrd().euroforgridview(Request.Item("Minimo"))
                End If
                txtDa.Text = Request.Item("Da")
                txtA.Text = Request.Item("A")

                If IsNothing(Operazione) = True Then
                    lblDescrizioneOperazione.Text = "Dati " + CType(ConstSession.DescrTipoProcServ, String).Replace("Tabella", "") + " - Inserimento " + CType(ConstSession.DescrTipoProcServ, String).Replace("Tabella", "")
                Else
                    lblDescrizioneOperazione.Text = "Dati " + CType(ConstSession.DescrTipoProcServ, String).Replace("Tabella", "") + " - Modifica " + CType(ConstSession.DescrTipoProcServ, String).Replace("Tabella", "")
                End If

                Dim ClsCanoni As New ClsCanoni
                'ClsCanoni.LoadComboUtenze(ddlTipoUtenza)
                If (IsNumeric(txtAnno.Text) And (txtAnno.Text.Length = 4)) Then
                    ClsCanoni.LoadComboUtenze(ddlTipoUtenza, txtAnno.Text)
                    ddlTipoUtenza.SelectedValue = IdTU
                End If

                If Operazione = "modifica" Then
                    'ddlTipoUtenza.Enabled = False
                End If
            End If

        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ConfiguraScaglioni.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub txtAnno_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAnno.TextChanged
        Dim clsCanoni As New ClsCanoni
        If (IsNumeric(txtAnno.Text) And (txtAnno.Text.Length = 4)) Then
            clsCanoni.LoadComboUtenze(ddlTipoUtenza, txtAnno.Text)
        End If
    End Sub

    Private Sub BtnSalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSalva.Click
        Dim ClsScaglioni As New ClsScaglioni
        Dim sScript As String
        Dim strErrore As String=""
        Dim objScaglione As New OggettoScaglione
        Dim objScaglioneOld As New OggettoScaglione
        Try
            If Operazione = "modifica" Then
                objScaglione.ID = IdScaglione
                objScaglione.idTipoUtenza = ddlTipoUtenza.SelectedValue
                objScaglione.dAliquota = txtIva.Text
                objScaglione.dTariffa = txtTariffa.Text
                If txtMinimo.Text <> "" Then
                    objScaglione.dMinimo = txtMinimo.Text
                End If
                objScaglione.sAnno = txtAnno.Text
                objScaglione.DA = txtDa.Text
                objScaglione.A = txtA.Text

                objScaglioneOld.sAnno = Anno
                objScaglioneOld.ID = IdScaglione
                If ClsScaglioni.UpdateScaglioniEnte(objScaglioneOld, objScaglione, strErrore) = True Then
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

                objScaglione.idTipoUtenza = ddlTipoUtenza.SelectedValue
                objScaglione.dAliquota = txtIva.Text
                objScaglione.dTariffa = txtTariffa.Text
                If txtMinimo.Text <> "" Then
                    objScaglione.dMinimo = txtMinimo.Text
                End If

                objScaglione.sAnno = txtAnno.Text
                objScaglione.DA = txtDa.Text
                objScaglione.A = txtA.Text
                If ClsScaglioni.SetScaglioniEnte(objScaglione, strErrore) = True Then
                    sScript = "GestAlert('a', 'success', '', '', 'Inserimento effettuato correttamente!'); parent.Search();"
                    RegisterScript(sScript, Me.GetType())
                    sScript = "UscitaDopoOperazione();"
                    RegisterScript(sScript, Me.GetType())
                Else
                    sScript = "GestAlert('a', 'warning', '', '', '" & strErrore & "');"
                    RegisterScript(sScript, Me.GetType())
                    sScript = "UscitaDopoOperazione();"
                    RegisterScript(sScript, Me.GetType())
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ConfiguraScaglioni.BtnSalva_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub BtnElimina_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnElimina.Click
        Dim ClsScaglioni As New ClsScaglioni
        Dim sScript As String
        Dim strErrore As String=""
        Dim objScaglione As New OggettoScaglione

        objScaglione.ID = IdScaglione
        Try
            If ClsScaglioni.DeleteScaglioniEnte(objScaglione, strErrore) = True Then
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
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ConfiguraScaglioni.BtnElimina_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
