Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports log4net

Partial Class ConfiguraNoloC
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger("ConfiguraNoloC")
    Private IdNoloEnte, IdTipoContatore, Iva, Tariffa, Anno, Operazione, IsUnaTantum As String

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
    ''' Pagina per l'inserimento/modifica/cancellazione del nolo per anno
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            Dim clsGenerale As New ClsGenerale.Generale
            IdNoloEnte = Request.Item("IdNoloEnte")
            IdTipoContatore = Request.Item("IdTipoContatore")
            Tariffa = Request.Item("Tariffa")
            Iva = Request.Item("Iva")
            Anno = Request.Item("Anno")
            Operazione = Request.Item("Operazione")
            If Not IsNothing(Request.Item("IsUnaTantum")) Then
                IsUnaTantum = Request.Item("IsUnaTantum")
            Else
                IsUnaTantum = "false"
            End If

            If Not Page.IsPostBack Then
                IdTipoContatore = Request.Item("IdTipoContatore")
                If Not Request.Item("Tariffa") Is Nothing Then
                    txtTariffa.Text = New ClsGenerale.FunctionGrd().euroforgridview(Request.Item("Tariffa"))
                End If
                If Not Request.Item("Iva") Is Nothing Then
                    txtIva.Text = New ClsGenerale.FunctionGrd().euroforgridview(Request.Item("Iva"))
                End If
                txtAnno.Text = Request.Item("Anno")
                ChkIsUnaTantum.Checked = CBool(IsUnaTantum)

                If IsNothing(Operazione) = True Then
                    lblDescrizioneOperazione.Text = "Dati " + CType(ConstSession.DescrTipoProcServ, String).Replace("Tabella", "") + " - Inserimento " + CType(ConstSession.DescrTipoProcServ, String).Replace("Tabella", "")
                Else
                    lblDescrizioneOperazione.Text = "Dati " + CType(ConstSession.DescrTipoProcServ, String).Replace("Tabella", "") + " - Modifica " + CType(ConstSession.DescrTipoProcServ, String).Replace("Tabella", "")
                End If

                Dim ClsNoliContatore As New ClsNoliContatore
                ClsNoliContatore.LoadComboTipoContatore(ddlTipoContatore)

                ddlTipoContatore.SelectedValue = IdTipoContatore
                If Operazione = "modifica" Then
                    'ddlTipoContatore.Enabled = False
                End If
            End If

            Dim sScript As String
            sScript = "Setfocus(document.getElementById('txtAnno'));"
            RegisterScript(sScript, Me.GetType())
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ConfiguraNoloC.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' pulsante per il salvataggio dei dati della videata
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnSalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSalva.Click
        Dim ClsNoliContatore As New ClsNoliContatore
        Dim sScript As String
        Dim strErrore As String=""
        Dim objNolo As New OggettoNoloContatore
        Dim objNoloOld As New OggettoNoloContatore

        Try
            If Operazione = "modifica" Then
                objNolo.ID = IdNoloEnte
                objNolo.idTipoContatore = ddlTipoContatore.SelectedValue
                objNolo.dAliquota = txtIva.Text
                objNolo.dImporto = txtTariffa.Text
                objNolo.sAnno = txtAnno.Text
                objNolo.bIsUnaTantum = ChkIsUnaTantum.Checked

                objNoloOld.sAnno = Anno
                objNoloOld.ID = IdNoloEnte
                If ClsNoliContatore.UpdateNoliContatoreEnte(objNoloOld, objNolo, strErrore) = True Then
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
                objNolo.idTipoContatore = ddlTipoContatore.SelectedValue
                objNolo.dAliquota = txtIva.Text
                objNolo.dImporto = txtTariffa.Text
                objNolo.sAnno = txtAnno.Text
                objNolo.bIsUnaTantum = ChkIsUnaTantum.Checked

                If ClsNoliContatore.SetNoliContatoreEnte(objNolo, strErrore) = True Then
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

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ConfiguraNoloC.BtnSalva_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' pulsante per la cancellazione del canone selezionato
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnElimina_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnElimina.Click
        Dim ClsNoliContatore As New ClsNoliContatore
        Dim sScript As String
        Dim strErrore As String=""
        Dim objNolo As New OggettoNoloContatore

        Try
            objNolo.ID = IdNoloEnte

            If ClsNoliContatore.DeleteNoliContatoreEnte(objNolo, strErrore) = True Then
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
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ConfiguraNoloC.BtnElimina_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
