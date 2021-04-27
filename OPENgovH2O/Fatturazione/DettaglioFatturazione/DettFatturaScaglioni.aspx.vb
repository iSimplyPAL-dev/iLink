Imports log4net
Imports RemotingInterfaceMotoreH2O.MotoreH2o.Oggetti

Partial Class DettFatturaScaglioni
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(DettFatturaScaglioni))

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




#Region "Griglie"

    'aggiunta per test
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim IDRow As String = e.CommandArgument.ToString()
            If e.CommandName = "RowOpen" Then
                For Each myRow As GridViewRow In GrdScaglioni.Rows

                    If IDRow = CType(myRow.FindControl("hfId"), HiddenField).Value Then
                        'Dim sIdDocumento, sProvenienza As String
                        'sIdDocumento = CType(myRow.FindControl("hfnIdDocumento"), HiddenField).Value
                        'sProvenienza = CType(myRow.FindControl("hfsVariato"), HiddenField).Value
                        'Log.Debug("OPENutenze::Fatturazione::DettaglioFatturazione::ResultRicFatturazione.aspx.vb.GrdRowCommand::errore::________________________________________________________________________________")
                        'Log.Debug("OPENutenze::Fatturazione::DettaglioFatturazione::ResultRicFatturazione.aspx.vb.GrdRowCommand::errore::" + sIdDocumento)
                        'Log.Debug("OPENutenze::Fatturazione::DettaglioFatturazione::ResultRicFatturazione.aspx.vb.GrdRowCommand::errore::" + sProvenienza)
                        'If sProvenienza = "" Or sProvenienza = "&nbsp;" Or sProvenienza.ToUpper = "X" Then
                        '    sProvenienza = TxtProvenienza.Text
                        '    Log.Debug(".GrdRowCommand::errore::IF" + sProvenienza)
                        'Else
                        '    sProvenienza = "C"
                        '    Log.Debug(".GrdRowCommand::errore::ELSE" + sProvenienza)
                        'End If
                        'myRow.Attributes.Add("onClick", "DettaglioDocumento('" & sIdDocumento & "','" & sProvenienza & "')")
                    End If
                Next
            ElseIf e.CommandName = "RowDelete" Then
                'If CType(sender, Ribes.OPENgov.WebControls.RibesGridView).UniqueID = "Grd" Then
                '    For Each myRow As GridViewRow In GrdDocumenti.Rows
                '        If IDRow = CType(myRow.FindControl("hfid"), HiddenField).Value Then

                '        End If
                '    Next
                'End If
            End If
        Catch ex As Exception
            Log.Debug(".GrdRowCommand::errore::", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub




    Protected Sub GrdRowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            If (e.Row.RowType = DataControlRowType.DataRow) Then
                'Dim sIdDocumento, sProvenienza As String
                'sIdDocumento = CType(e.Row.FindControl("hfnIdDocumento"), HiddenField).Value
                'sProvenienza = CType(e.Row.FindControl("hfsVariato"), HiddenField).Value

            End If
        Catch ex As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.ResultRicFatturazione.GrdRowDataBound.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub


    ''' <summary>
    ''' Gestione del cambio pagina della griglia.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdPageIndexChanging(ByVal sender As Object, e As GridViewPageEventArgs)
        'LoadSearch(e.NewPageIndex)
    End Sub

#End Region







#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Dim oListDettFatturaScaglioni() As ObjTariffeScaglione
        Try
            If Page.IsPostBack = False Then
                If Not Session("oDettFatturaScaglioni") Is Nothing Then
                    oListDettFatturaScaglioni = CType(Session("oDettFatturaScaglioni"), ObjTariffeScaglione())

                    GrdScaglioni.DataSource = oListDettFatturaScaglioni
                    GrdScaglioni.DataBind()
                    GrdScaglioni.SelectedIndex = -1
                    LblResultScaglioni.Style.Add("display", "none")
                Else
                    LblResultScaglioni.Style.Add("display", "")
                End If
            Else
                oListDettFatturaScaglioni = CType(Session("oDettFatturaScaglioni"), ObjTariffeScaglione())
                GrdScaglioni.DataSource = oListDettFatturaScaglioni
                GrdScaglioni.DataBind()
            End If
        Catch Err As Exception


            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.DettFatturaScaglioni.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
