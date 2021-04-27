Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
'Imports Utility
Imports log4net

Partial Class ConfiguraCanoni
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ConfiguraCanoni))
    Public IdCanoneEnte, IdTU, IdTC, Anno, PercentualeSuConsumo, Aliquota, Tariffa, Operazione As String

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
    ''' Pagina per l'inserimento/modifica/cancellazione della quota canone ente per anno
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            'Put user code to initialize the page here
            Dim clsGenerale As New ClsGenerale.Generale
            IdCanoneEnte = Request.Item("IdCanoneEnte")
            IdTU = Request.Item("IdTU")
            IdTC = Request.Item("IdTC")
            Tariffa = Request.Item("Tariffa")
            Aliquota = Request.Item("Aliquota")
            Anno = Request.Item("Anno")
            PercentualeSuConsumo = Request.Item("PercentualeSuConsumo")
            Operazione = Request.Item("Operazione")
            If Not Page.IsPostBack Then
                IdCanoneEnte = Request.Item("IdCanoneEnte")
                IdTU = Request.Item("IdTU")
                IdTC = Request.Item("IdTC")
                txtTariffa.Text = Request.Item("Tariffa")
                If Not Request.Item("Aliquota") Is Nothing Then
                    txtIva.Text = New ClsGenerale.FunctionGrd().euroforgridview(Request.Item("Aliquota"))
                End If
                txtAnno.Text = Request.Item("Anno")
                If Not Request.Item("PercentualeSuConsumo") Is Nothing Then
                    txtConsumo.Text = New ClsGenerale.FunctionGrd().euroforgridview(Request.Item("PercentualeSuConsumo"))
                End If
                If IsNothing(Operazione) = True Then
                    lblDescrizioneOperazione.Text = "Dati " + CType(ConstSession.DescrTipoProcServ, String).Replace("Tabella", "") + " - Inserimento " + CType(ConstSession.DescrTipoProcServ, String).Replace("Tabella", "")
                Else
                    lblDescrizioneOperazione.Text = "Dati " + CType(ConstSession.DescrTipoProcServ, String).Replace("Tabella", "") + " - Modifica " + CType(ConstSession.DescrTipoProcServ, String).Replace("Tabella", "")
                End If

                Dim ClsCanoni As New ClsCanoni
                ClsCanoni.LoadComboCanoni(ddlTipoCanone)
                ClsCanoni.LoadComboUtenze(ddlTipoUtenza, txtAnno.Text)

                ddlTipoCanone.SelectedValue = IdTC
                ddlTipoUtenza.SelectedValue = IdTU
                If Operazione = "modifica" Then
                    'ddlTipoCanone.Enabled = False
                    'ddlTipoUtenza.Enabled = False
                End If
            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ConfiguraCanoni.Page_Load.errore: ", ex)
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
        Try
            Dim ClsCanoni As New ClsCanoni
            Dim sScript As String
            Dim strErrore As String=""
            Dim objCanone As New OggettoCanone
            Dim objCanoneOld As New OggettoCanone

            If Operazione = "modifica" Then
                objCanone.sIdEnte = ConstSession.IdEnte
                objCanone.ID = IdCanoneEnte
                objCanone.idTipoCanone = ddlTipoCanone.SelectedValue
                objCanone.idTipoUtenza = ddlTipoUtenza.SelectedValue
                objCanone.dAliquota = txtIva.Text
                objCanone.dTariffa = txtTariffa.Text
                objCanone.dPercentualeSuConsumo = txtConsumo.Text
                objCanone.sAnno = txtAnno.Text

                objCanoneOld.sIdEnte = ConstSession.IdEnte
                objCanoneOld.sAnno = Anno
                objCanoneOld.ID = IdCanoneEnte
                If ClsCanoni.SetCanoniEnte(objCanone, objCanoneOld, strErrore) = True Then
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
                objCanone.sIdEnte = ConstSession.IdEnte
                objCanone.idTipoCanone = ddlTipoCanone.SelectedValue
                objCanone.idTipoUtenza = ddlTipoUtenza.SelectedValue
                objCanone.dAliquota = txtIva.Text
                objCanone.dTariffa = txtTariffa.Text
                objCanone.dPercentualeSuConsumo = txtConsumo.Text
                objCanone.sAnno = txtAnno.Text
                If ClsCanoni.SetCanoniEnte(objCanone, Nothing, strErrore) = True Then
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
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ConfiguraCanoni.BtnSalva_Click.errore: ", ex)
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
        Dim ClsCanoni As New ClsCanoni
        Dim sScript As String
        Dim strErrore As String=""
        Dim ObjCanone As New OggettoCanone

        ObjCanone.ID = IdCanoneEnte
        Try
            If ClsCanoni.DeleteCanoniEnte(ObjCanone, strErrore) = True Then
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

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ConfiguraCanoni.BtnElimina_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub txtAnno_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAnno.TextChanged
        Dim ClsCanoni As New ClsCanoni
        ClsCanoni.LoadComboUtenze(ddlTipoUtenza, txtAnno.Text)
    End Sub
End Class
