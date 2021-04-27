Imports log4net

Partial Class DettFatture
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

    Private dvFatture As DataView
    Private Shared Log As ILog = LogManager.GetLogger(GetType(DettFatture))
    'Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    '    Dim dt As DataView
    '    Dim strCODENTE As String
    '    Dim strCOD_CONTRIBUENTE As String
    '    Dim strUsername As String
    '    Dim strIdentificativoApplicazione As String
    '    Dim Fatture As ClsFatture = New ClsFatture
    '    Dim WFSession As OPENUtility.CreateSessione
    '    Dim WFErrore As String
    '    Dim myDvResult As DataView

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

    '            dvFatture = Fatture.GetInfoFattureContribuente(strCODENTE, strCOD_CONTRIBUENTE, WFSession)

    '            Session.Remove("DV_H2O_FATTURE")
    '            Session("DV_H2O_FATTURE") = dvFatture
    '            If Not dvFatture Is Nothing Then
    '                ViewState("SortKey") = "MATRICOLA"
    '                ViewState("OrderBy") = "DESC"
    '                dt = dvFatture
    '                dt.Sort = ViewState("SortKey") & " " & ViewState("OrderBy")
    '            End If

    '            If dt.Count <= 0 Then
    '                grdFatture.Visible = False
    '                lblMessage.Text = "La ricerca non ha prodotto risultati"
    '                lblMessage.Visible = True
    '                strscript = "parent.riduciFrame('ifrmFattureH2O');" & vbCrLf
    '            Else
    '                grdFatture.Visible = True
    '                Session("vistaH2OFatture") = dt
    '                grdFatture.start_index = 0
    '                grdFatture.DataSource = dt
    '                grdFatture.DataBind()
    '                strscript = "parent.aumentaFrame('ifrmFattureH2O',180);" & vbCrLf
    '            End If
    '            RegisterScript(sScript , Me.GetType())
    '        Else
    '            ' faccio il bind della ribesdatagrid per gestire la paginazione
    '            dvFatture = CType(Session("DV_H2O_FATTURE"), DataView)
    '            grdFatture.AllowCustomPaging = False
    '            grdFatture.start_index = Convert.ToString(grdFatture.CurrentPageIndex)

    '            dt = Session("vistaH2OFatture")

    '            If Not IsNothing(dt) Then
    '                grdFatture.Rows.Count = dt.Count
    '                grdFatture.DataSource = dt
    '            End If
    '        End If

    '    Catch ex As Exception

    '  Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DettFatture.Page_Load.errore: ", ex)
    '   Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim dt As New DataView
        Dim strCODENTE As String
        Dim strCOD_CONTRIBUENTE As String
        Dim strUsername As String
        Dim Fatture As New ClsFatture
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

                dvFatture = Fatture.GetInfoFattureContribuente(strCODENTE, strCOD_CONTRIBUENTE)

                Session.Remove("DV_H2O_FATTURE")
                Session("DV_H2O_FATTURE") = dvFatture
                If Not dvFatture Is Nothing Then
                    ViewState("SortKey") = "MATRICOLA"
                    ViewState("OrderBy") = "DESC"
                    dt = dvFatture
                    dt.Sort = ViewState("SortKey") & " " & ViewState("OrderBy")
                End If

                If dt.Count <= 0 Then
                    GrdFatture.Visible = False
                    lblMessage.Text = "La ricerca non ha prodotto risultati"
                    lblMessage.Visible = True
                    sScript = "parent.riduciFrame('ifrmFattureH2O');" & vbCrLf
                Else
                    GrdFatture.Visible = True
                    Session("vistaH2OFatture") = dt
                    GrdFatture.DataSource = dt
                    GrdFatture.DataBind()
                    sScript = "parent.aumentaFrame('ifrmFattureH2O',180);" & vbCrLf
                End If
                RegisterScript(sScript, Me.GetType())
            Else
                ' faccio il bind della ribesdatagrid per gestire la paginazione
                'dvFatture = CType(Session("DV_H2O_FATTURE"), DataView)

                'dt = Session("vistaH2OFatture")

                'If Not IsNothing(dt) Then
                '    GrdFatture.DataSource = dt
                'End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DettFatture.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Protected Function GiraData(ByVal prdStatus As Object) As String
        GiraData = MyUtility.GiraDataFromDB(utility.stringoperation.formatstring(prdStatus))
        Return GiraData
    End Function

#Region "Griglie"
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim IDRow As String = e.CommandArgument.ToString()
            If e.CommandName = "RowOpen" Then
                For Each myRow As GridViewRow In GrdFatture.Rows
                    If IDRow = CType(myRow.FindControl("hfid"), HiddenField).Value Then
                        Dim url As String
                        Dim sScript As String
                        Dim opengovpath As String
                        Dim urlChiamante As String

                        opengovpath = Request.Item("OPENgovPATH")

                        urlChiamante = opengovpath & "/SituazioneContribuente/SitContrib.aspx?COD_CONTRIBUENTE=" & Request.Item("COD_CONTRIBUENTE")

                        url = "/Fatturazione/DettaglioFatturazione/DettaglioFatturazione.aspx?IDDOCUMENTO=" & GrdFatture.Rows(GrdFatture.SelectedIndex).Cells(6).Text
                        url += "&PAGINACOMANDI=ComandiDettFatturazione.aspx&Provenienza=SC&ProvenienzaChiamante="
                        url += "&PaginaChiamante=" & Server.UrlEncode(urlChiamante)

                        sScript = "parent.parent.Visualizza.location.href='" & opengovpath & "/Login/RichiamaUtenze.aspx?provenienza_applicativo=OPENGOV"
                        sScript += "&RedirectPage=" & Server.UrlEncode(url) & "'"

                        RegisterScript(sScript , Me.GetType())
                    End If
                Next
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DettFatture.GrdRowCommand.errore: ", ex)
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
            GrdFatture.DataSource = CType(Session("vistaH2OFatture"), DataView)
            If page.HasValue Then
                GrdFatture.PageIndex = page.Value
            End If
            GrdFatture.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DettFatture.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub

    'Private Sub grdFatture_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles grdFatture.SortCommand

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

    '        dt = dvFatture
    '        dt.Sort = viewstate("SortKey") & " " & viewstate("OrderBy")

    '        GrdFatture.DataSource = dt
    '        grdFatture.DataBind()

    '    Catch ex As Exception
    '     Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DettFatture.grdFatture_SortCommand.errore: ", ex)
    '     Response.Redirect("../../PaginaErrore.aspx")
    '    End Try

    'End Sub

    'Private Sub GrdFatture_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdFatture.SelectedIndexChanged
    '    Try
    '        Dim url As String
    '        Dim sScript As String
    '        Dim opengovpath As String
    '        Dim urlChiamante As String

    '        opengovpath = Request.Item("OPENgovPATH")

    '        urlChiamante = opengovpath & "/SituazioneContribuente/SitContrib.aspx?COD_CONTRIBUENTE=" & Request.Item("COD_CONTRIBUENTE")

    '        url = "/Fatturazione/DettaglioFatturazione/DettaglioFatturazione.aspx?IDDOCUMENTO=" & GrdFatture.Rows(GrdFatture.SelectedIndex).Cells(6).Text
    '        url += "&PAGINACOMANDI=ComandiDettFatturazione.aspx&Provenienza=SC&ProvenienzaChiamante="
    '        url += "&PaginaChiamante=" & Server.UrlEncode(urlChiamante)

    '        sScript = "parent.parent.Visualizza.location.href='" & opengovpath & "/Login/RichiamaUtenze.aspx?provenienza_applicativo=OPENGOV"
    '        sScript += "&RedirectPage=" & Server.UrlEncode(url) & "'"

    '        RegisterScript(sScript , Me.GetType())
    '    Catch Err As Exception
    '     Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DettFatture.grdFatture_SelectedIndexChanged.errore: ", ex)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try

    'End Sub

End Class
