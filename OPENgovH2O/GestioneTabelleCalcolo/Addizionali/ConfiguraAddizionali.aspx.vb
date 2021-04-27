Imports log4net
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti

Partial Class ConfiguraAddizionali
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ConfiguraAddizionali))
    'Public IdAddizionale, Percentuale, IdCapitolo, Operazione, Anno As String
    Public IdAddizionaleEnte, IdAddizionale, Tariffa, Anno, Iva, Operazione As String
    Protected WithEvents Textbox1 As System.Web.UI.WebControls.TextBox

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
    ''' Pagina per l'inserimento/modifica/cancellazione dell'addizionale ente per anno
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            'Put user code to initialize the page here
            Dim clsGenerale As New ClsGenerale.Generale
            IdAddizionaleEnte = Request.Item("IdAddizionaleEnte")
            IdAddizionale = Request.Item("IdAddizionale")
            Tariffa = Request.Item("Tariffa")
            Iva = Request.Item("Iva")
            Anno = Request.Item("Anno")
            Operazione = Request.Item("Operazione")
            If Not Page.IsPostBack Then
                IdAddizionale = Request.Item("IdAddizionale")
                txtTariffa.Text = Request.Item("Tariffa")
                If Not Request.Item("Iva") Is Nothing Then
                    txtIva.Text = New ClsGenerale.FunctionGrd().EuroForGridView(Request.Item("Iva"))
                End If
                txtAnno.Text = Request.Item("Anno")

                If IsNothing(Operazione) = True Then
                    lblDescrizioneOperazione.Text = "Dati " + CType(ConstSession.DescrTipoProcServ, String).Replace("Tabella", "") + " - Inserimento " + CType(ConstSession.DescrTipoProcServ, String).Replace("Tabella", "")
                Else
                    lblDescrizioneOperazione.Text = "Dati " + CType(ConstSession.DescrTipoProcServ, String).Replace("Tabella", "") + " - Modifica " + CType(ConstSession.DescrTipoProcServ, String).Replace("Tabella", "")
                End If

                Dim ClsAddizionali As New ClsAddizionali
                ClsAddizionali.LoadComboAddizionali(ddlAddizionali)

                ddlAddizionali.SelectedValue = IdAddizionale
                If Operazione = "modifica" Then
                    ddlAddizionali.Enabled = False
                End If
            End If

            Dim sArray() As Object

            ReDim Preserve sArray(2)

            sArray(0) = "ddlAddizionali"
            sArray(1) = "txtPercentuale"
            sArray(2) = "txtAnno"

            'Dim ClsGenerale As New ClsGenerale.Generale

            'If Session("dirittioperatore") = "LETTURA" Then
            '    'str = "document.getElementById('ddlAddizionali').disabled=true;"
            '    str = ClsGenerale.PopolaJSdisabilita(sArray)
            '    RegisterScript(sScript , Me.GetType())"load", str)
            'End If

            Dim sScript As String
            sScript = "Setfocus(document.Form1.txtAnno);"
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ConfiguraAddizionali.Page_Load.errore: ", ex)
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
            Dim ClsAddizionali As New ClsAddizionali
            Dim sScript As String
            Dim strErrore As String = ""
            Dim objAdd As New OggettoAddizionaleEnte
            Dim objAddOld As New OggettoAddizionaleEnte

            If Operazione = "modifica" Then
                objAdd.ID = IdAddizionaleEnte
                objAdd.IDaddizionale = IdAddizionale
                objAdd.Aliquota = txtIva.Text
                objAdd.dImporto = txtTariffa.Text
                objAdd.sAnno = txtAnno.Text

                objAddOld.sAnno = Anno
                objAddOld.IDaddizionale = IdAddizionale
                If ClsAddizionali.SetAddizionaliEnte(objAdd, objAddOld, strErrore) = True Then
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
                objAdd.IDaddizionale = ddlAddizionali.SelectedValue
                objAdd.Aliquota = txtIva.Text
                objAdd.dImporto = txtTariffa.Text
                objAdd.sAnno = txtAnno.Text
                If ClsAddizionali.SetAddizionaliEnte(objAdd, Nothing, strErrore) = True Then
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
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.ConfiguraAddizionali.BtnSalva_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' pulsante per la cancellazione dell'addizionale selezionata
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnElimina_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnElimina.Click
        Dim ClsAddizionale As New ClsAddizionali
        Dim sScript As String
        Dim strErrore As String = ""
        Dim ObjAddizionale As New OggettoAddizionaleEnte

        ObjAddizionale.ID = IdAddizionaleEnte
        Try
            If ClsAddizionale.DeleteAddizionaleEnte(ObjAddizionale, strErrore) = True Then
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
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ConfiguraAddizionali.BtnElimina_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
