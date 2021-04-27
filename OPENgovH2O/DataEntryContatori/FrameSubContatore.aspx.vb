Imports log4net
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti
Imports AnagInterface

Partial Class FrameSubContatore
    Inherits BasePage
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(FrameSubContatore))
    Private myDvResult As DataView

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents btnEV As System.Web.UI.WebControls.Button
    Protected WithEvents Associato As System.Web.UI.WebControls.TextBox

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
        Try
            ContNascosto.Text = Request.Params("contID")
            If Page.IsPostBack Then
                If Not IsNothing(Session("myDvResult")) Then
                    myDvResult = CType(Session("myDvResult"), DataView)
                    GrdContatori.DataSource = myDvResult
                End If
            End If
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.FrameSubContatore.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim DEContatori As GestContatori = New GestContatori

        Try
            Session("myDvResult") = Nothing
            myDvResult = DEContatori.GetListaSubContatori(ConstSession.IdEnte, ContNascosto.Text, txtMatricola.Text, txtNumeroUtente.Text, TxtVia.Text, txtIntestatario.Text, txtUtente.Text)
            If Not IsNothing(myDvResult) Then
                If myDvResult.Count > 0 Then
                    GrdContatori.DataSource = myDvResult
                    GrdContatori.DataBind()
                    Session("myDvResult") = myDvResult
                Else
                    lblMessage.Text = "La ricerca non ha prodotto risultati."
                    lblMessage.Style.Add("display", "")
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.FrameSubContatore.btnSearch_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    'Private Sub btnAssocia_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAssocia.Click
    '    Dim sScript As String
    '    Dim x, nList As Integer
    '    Dim oListSubContatori() As ObjSubContatore
    '    Dim oSubContatore As ObjSubContatore

    '    Try
    '        'ribalto
    '        'controllo se ho già degli immobili inseriti
    '        If Not Session("oListSubContatori") Is Nothing Then
    '            oListSubContatori = Session("oListSubContatori")
    '            nList = oListSubContatori.GetUpperBound(0)
    '        Else
    '            nList = -1
    '        End If
    '        If GrdContatori.SelectedIndex > -1 Then
    '            oSubContatore = New ObjSubContatore
    '            oSubContatore.IdContatorePrincipale = ContNascosto.Text
    '            oSubContatore.IdSubContatore = GrdContatori.Rows(GrdContatori.SelectedIndex).Cells(4).Text
    '            oSubContatore.sMatricola = GrdContatori.Rows(GrdContatori.SelectedIndex).Cells(0).Text
    '            oSubContatore.sUbicazione = CType(GrdContatori.Rows(GrdContatori.SelectedIndex).FindControl("LblUbicazione"), Label).Text
    '            oSubContatore.sCognomeIntestatario = GrdContatori.Rows(GrdContatori.SelectedIndex).Cells(1).Text
    '            oSubContatore.sNomeIntestatario = GrdContatori.Rows(GrdContatori.SelectedIndex).Cells(2).Text
    '            nList += 1
    '            ReDim Preserve oListSubContatori(nList)
    '            oListSubContatori(nList) = oSubContatore

    '            Session("oListSubContatori") = oListSubContatori
    '            sScript = "opener.parent.frames('visualizza').document.getElementById('CmdRibaltaSubContatori').click();"
    '            sScript += "window.close();"
    '        Else
    '            sScript = "GestAlert('a', 'warning', '', '', 'Selezionare una posizione dalla griglia e premere il pulsante Associa.');"
    '        End If
    '        RegisterScript(sScript , Me.GetType())
    '    Catch Err As Exception

    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.FrameSubContatore.btnAssocia_Click.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
    '    Dim DEContatori As GestContatori = New GestContatori
    '    Dim WFSession As OPENUtility.CreateSessione
    '    Dim WFErrore As String

    '    Try
    '        'inizializzo la connessione
    '        WFSession = New OPENUtility.CreateSessione(ConstSession.ParametroEnv, ConstSession.UserName, ConstSession.IdentificativoApplicazione)
    '        If Not WFSession.CreaSessione(ConstSession.UserName, WFErrore) Then
    '            Throw New Exception("Errore durante l'apertura della sessione di WorkFlow")
    '        End If

    '        Session("myDvResult") = Nothing
    '        myDvResult = DEContatori.GetListaSubContatori(ConstSession.IdEnte, ContNascosto.Text, txtMatricola.Text, txtNumeroUtente.Text, TxtVia.Text, txtIntestatario.Text, txtUtente.Text, WFSession)
    '        If Not IsNothing(myDvResult) Then
    '            If myDvResult.Count > 0 Then
    '                GrdContatori.start_index = Convert.ToString(0)
    '                GrdContatori.Rows.Count = myDvResult.Count
    '                GrdContatori.DataSource = myDvResult
    '                GrdContatori.DataBind()
    '                Session("myDvResult") = myDvResult
    '            Else
    '                lblMessage.Text = "La ricerca non ha prodotto risultati."
    '                lblMessage.Style.Add("display", "")
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.FrameSubContatore.btnSearch_Click.errore: ", Err)
    '        Response.Redirect("../../PaginaErrore.aspx")
    '    Finally
    '        WFSession.Kill()
    '    End Try
    'End Sub
#Region "Griglie"
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim IDRow As String = e.CommandArgument.ToString()
            Dim sScript As String
            Dim nList As Integer
            Dim oListSubContatori() As ObjSubContatore
            Dim oSubContatore As ObjSubContatore

            If e.CommandName = "RowOpen" Then
                'controllo se ho già degli immobili inseriti
                If Not Session("oListSubContatori") Is Nothing Then
                    oListSubContatori = Session("oListSubContatori")
                    nList = oListSubContatori.GetUpperBound(0)
                Else
                    nList = -1
                End If
                For Each myRow As GridViewRow In GrdContatori.Rows
                    If IDRow = CType(myRow.FindControl("hfidcontatore"), HiddenField).Value Then
                        oSubContatore = New ObjSubContatore
                        oSubContatore.IdContatorePrincipale = ContNascosto.Text
                        oSubContatore.IdSubContatore = IDRow
                        oSubContatore.sMatricola = myRow.Cells(0).Text
                        oSubContatore.sUbicazione = CType(myRow.FindControl("LblUbicazione"), Label).Text
                        oSubContatore.sCognomeIntestatario = myRow.Cells(1).Text
                        oSubContatore.sNomeIntestatario = myRow.Cells(2).Text
                        oSubContatore.speriodo = myRow.Cells(3).Text
                        nList += 1
                        ReDim Preserve oListSubContatori(nList)
                        oListSubContatori(nList) = oSubContatore

                        Session("oListSubContatori") = oListSubContatori
                        sScript = "opener.parent.Visualizza.document.getElementById('CmdRibaltaSubContatori').click();"
                        sScript += "window.close();"
                        RegisterScript(sScript, Me.GetType())
                    End If
                Next
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.FrameSubContatore.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
#End Region
End Class
