Imports log4net

Partial Class DettLetture
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

    Private dvLetture As DataView
    Private Shared Log As ILog = LogManager.GetLogger(GetType(DettLetture))
    Protected FncGrd As New ClsGenerale.FunctionGrd
    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    Dim dt As DataView
    '    Dim strCODENTE As String
    '    Dim strCOD_CONTRIBUENTE As String
    '    Dim strUsername As String
    '    Dim strIdentificativoApplicazione As String
    '    Dim BELetture As GestLetture = New GestLetture
    '    Dim WFSession As OPENUtility.CreateSessione
    '    Dim WFErrore As String

    '    Dim strscript As String = ""

    '    Try
    '        If (Not IsPostBack) Then

    '            ' Questa pagina può essere richiamata sia da OPENUtenze
    '            ' (dopo il login, e quindi con parametri di Session popolati)
    '            ' che da OPENGov nella pagina di riassunto situazione contribuente,
    '            ' pertanto i parametri vengono passati nella Request
    '            strCODENTE = ConstSession.IdEnte
    '            If strCODENTE Is Nothing Then
    '                strCODENTE = Request.Item("COD_ENTE").ToString
    '            End If
    '            strCOD_CONTRIBUENTE = Request.Item("COD_CONTRIBUENTE").ToString

    '            strUsername = Session("username")
    '            If strUsername Is Nothing Then
    '                strUsername = Request.Item("username").ToString
    '            End If

    '            strIdentificativoApplicazione = ConfigurationManager.AppSettings("IDENTIFICATIVOAPPLICAZIONE")

    '            'inizializzo la connessione
    '            WFSession = New OPENUtility.CreateSessione(ConfigurationManager.AppSettings("PARAMETROENV"), strUsername, strIdentificativoApplicazione)
    '            If Not WFSession.CreaSessione(strUsername, WFErrore) Then
    '                Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '            End If

    '            dvLetture = BELetture.GetInfoLettureContribuente(strCODENTE, strCOD_CONTRIBUENTE, WFSession)

    '            Session.Remove("DV_H2O_LETTURE")
    '            Session("DV_H2O_LETTURE") = dvLetture
    '            If Not dvLetture Is Nothing Then
    '                viewstate("SortKey") = "DATALETTURA"
    '                viewstate("OrderBy") = "DESC"
    '                dt = dvLetture
    '                dt.Sort = viewstate("SortKey") & " " & viewstate("OrderBy")
    '            End If

    '            If dt.Count <= 0 Then
    '                grdLetture.Visible = False
    '                lblMessage.Text = "La ricerca non ha prodotto risultati"
    '                lblMessage.Visible = True
    '                strscript = "parent.riduciFrame('ifrmContatoreH2O');" & vbCrLf
    '            Else
    '                grdLetture.Visible = True
    '                Session("vistaH2OLetture") = dt
    '                grdLetture.start_index = 0
    '                grdLetture.DataSource = dt
    '                grdLetture.DataBind()
    '                strscript = "parent.aumentaFrame('ifrmContatoreH2O',180);" & vbCrLf
    '            End If
    '            RegisterScript(sScript , Me.GetType())
    '        Else
    '            ' faccio il bind della ribesdatagrid per gestire la paginazione
    '            dvLetture = CType(Session("DV_H2O_LETTURE"), DataView)
    '            grdLetture.AllowCustomPaging = False
    '            grdLetture.start_index = Convert.ToString(grdLetture.CurrentPageIndex)

    '            dt = Session("vistaH2OLetture")

    '            If Not IsNothing(dt) Then
    '                grdLetture.Rows.Count = dt.Count
    '                grdLetture.DataSource = dt
    '            End If
    '        End If

    '    Catch ex As Exception
    'Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DettLetture.Page_Load.errore: ", ex)
    '    Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim dt As New DataView
        Dim strCODENTE As String
        Dim strCOD_CONTRIBUENTE As String
        Dim strUsername As String
        Dim strIdentificativoApplicazione As String
        Dim BELetture As GestLetture = New GestLetture
        Dim sScript As String = ""

        Try
            If (Not IsPostBack) Then

                ' Questa pagina può essere richiamata sia da OPENUtenze
                ' (dopo il login, e quindi con parametri di Session popolati)
                ' che da OPENGov nella pagina di riassunto situazione contribuente,
                ' pertanto i parametri vengono passati nella Request
                strCODENTE = ConstSession.IdEnte
                If strCODENTE Is Nothing Then
                    strCODENTE = Request.Item("COD_ENTE").ToString
                End If
                strCOD_CONTRIBUENTE = Request.Item("COD_CONTRIBUENTE").ToString

                strUsername = Session("username")
                If strUsername Is Nothing Then
                    strUsername = Request.Item("username").ToString
                End If

                strIdentificativoApplicazione = ConfigurationManager.AppSettings("IDENTIFICATIVOAPPLICAZIONE")

                dvLetture = BELetture.GetInfoLettureContribuente(strCODENTE, strCOD_CONTRIBUENTE)

                Session.Remove("DV_H2O_LETTURE")
                Session("DV_H2O_LETTURE") = dvLetture
                If Not dvLetture Is Nothing Then
                    ViewState("SortKey") = "DATALETTURA"
                    ViewState("OrderBy") = "DESC"
                    dt = dvLetture
                    dt.Sort = ViewState("SortKey") & " " & ViewState("OrderBy")
                End If

                If dt.Count <= 0 Then
                    GrdLetture.Visible = False
                    lblMessage.Text = "La ricerca non ha prodotto risultati"
                    lblMessage.Visible = True
                    sScript = "parent.riduciFrame('ifrmContatoreH2O');" & vbCrLf
                Else
                    GrdLetture.Visible = True
                    Session("vistaH2OLetture") = dt
                    GrdLetture.DataSource = dt
                    GrdLetture.DataBind()
                    sScript = "parent.aumentaFrame('ifrmContatoreH2O',180);" & vbCrLf
                End If
                RegisterScript(sScript , Me.GetType())
            Else
                '' faccio il bind della ribesdatagrid per gestire la paginazione
                'dvLetture = CType(Session("DV_H2O_LETTURE"), DataView)

                'dt = Session("vistaH2OLetture")

                'If Not IsNothing(dt) Then
                '    GrdLetture.DataSource = dt
                'End If
            End If

        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DettLetture.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub


    Protected Function StatusToInt(ByVal prdStatus As String) As String
        If prdStatus = "No" Then
            Return 0
        Else
            Return 1
        End If
    End Function

#Region "Griglie"
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim IDRow As String = e.CommandArgument.ToString()
            If e.CommandName = "RowOpen" Then
                Dim url As String
                Dim sScript As String
                Dim opengovpath As String
                For Each myRow As GridViewRow In GrdLetture.Rows
                    If IDRow = CType(myRow.FindControl("hfid"), HiddenField).Value Then
                        opengovpath = Request.Item("OPENgovPATH")

                        url = "/Letture/ModLettureHome.aspx?IDCONTATORE=" & GrdLetture.Rows(GrdLetture.SelectedIndex).Cells(7).Text
                        url += "&IDLETTURA=" & GrdLetture.Rows(GrdLetture.SelectedIndex).Cells(8).Text
                        url += "&IsFatturata=" & StatusToInt(GrdLetture.Rows(GrdLetture.SelectedIndex).Cells(6).Text)
                        url += "&PAG_PREC=1&sProvenienza=SC"
                        url += "&OPENgovPATH=" & opengovpath & "&CodContribuente=" & Request.Item("COD_CONTRIBUENTE")

                        sScript = "parent.parent.Visualizza.location.href='" & opengovpath & "/Login/RichiamaUtenze.aspx?provenienza_applicativo=OPENGOV"
                        sScript += "&RedirectPage=" & Server.UrlEncode(url) & "'"

                        RegisterScript(sScript , Me.GetType())
                    End If
                Next
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DettLetture.GrdRowCommand.errore: ", ex)

            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Gestione del cambio pagina della griglia.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdPageIndexChanging(ByVal sender As Object, e As GridViewPageEventArgs)
        LoadSearch(e.NewPageIndex)
    End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdLetture.DataSource = CType(Session("vistaH2OLetture"), DataView)
            If page.HasValue Then
                GrdLetture.PageIndex = page.Value
            End If
            GrdLetture.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DettLetture.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub

    'Private Sub grdLetture_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles grdLetture.SortCommand

    '    Dim strSortKey As String
    '    Dim dt As DataView

    '    Try
    '        If e.SortExpression.ToString() = viewstate("SortKey").ToString() Then
    '            Select Case viewstate("OrderBy").ToString()
    '                Case "ASC"
    '                    viewstate("OrderBy") = "DESC"

    '                Case "DESC"
    '                    viewstate("OrderBy") = "ASC"
    '            End Select
    '        Else
    '            viewstate("SortKey") = e.SortExpression
    '            viewstate("OrderBy") = "ASC"
    '        End If

    '        dt = dvLetture
    '        dt.Sort = viewstate("SortKey") & " " & viewstate("OrderBy")

    '        GrdLetture.DataSource = dt
    '        grdLetture.DataBind()

    '    Catch ex As Exception
    ' Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DettLetture.grdLetture_SortCommand.errore: ", ex)

    '   Response.Redirect("../../PaginaErrore.aspx")
    '    End Try

    'End Sub

    'Private Sub GrdLetture_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdLetture.SelectedIndexChanged
    '    Try
    '        Dim url As String
    '        Dim sScript As String
    '        Dim opengovpath As String

    '        opengovpath = Request.Item("OPENgovPATH")

    '        url = "/Letture/ModLettureHome.aspx?IDCONTATORE=" & grdLetture.Items(grdLetture.SelectedIndex).Cells(7).Text
    '        url += "&IDLETTURA=" & grdLetture.Items(grdLetture.SelectedIndex).Cells(8).Text
    '        url += "&IsFatturata=" & StatusToInt(grdLetture.Items(grdLetture.SelectedIndex).Cells(6).Text)
    '        url += "&PAG_PREC=1&sProvenienza=SC"
    '        url += "&OPENgovPATH=" & opengovpath & "&CodContribuente=" & Request.Item("COD_CONTRIBUENTE")

    '        sScript = "parent.parent.Visualizza.location.href='" & opengovpath & "/Login/RichiamaUtenze.aspx?provenienza_applicativo=OPENGOV"
    '        sScript += "&RedirectPage=" & Server.UrlEncode(url) & "'"

    '        RegisterScript(sScript , Me.GetType())
    '    Catch Err As Exception
    '       Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DettLetture.GrdLetture_SelectedIndexChanged.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try

    'End Sub

End Class
