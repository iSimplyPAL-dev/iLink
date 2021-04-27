Imports System.Xml
Imports System.Data
Imports System.Text
Imports System.Data.SqlClient
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Reflection
Imports log4net
''' <summary>
''' Pagina per la gestione dello stradario.
''' Contiene i parametri di ricerca, le funzioni della comandiera e la griglia per la visualizzazione del risultato. 
''' </summary>
Partial Class RicercaStrade
    Inherits BaseEnte
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(RicercaStrade))
    Private WFErrore As String

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents txtNote As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtLarghezza As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtLunghezza As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label
    Protected WithEvents Label2 As System.Web.UI.WebControls.Label
    Protected WithEvents Label6 As System.Web.UI.WebControls.Label
    Protected WithEvents Label3 As System.Web.UI.WebControls.Label
    Protected WithEvents Label4 As System.Web.UI.WebControls.Label
    Protected WithEvents Label8 As System.Web.UI.WebControls.Label
    Protected WithEvents txtExDenominazione As System.Web.UI.WebControls.TextBox
    Protected WithEvents Label5 As System.Web.UI.WebControls.Label
    Protected WithEvents Label10 As System.Web.UI.WebControls.Label

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            Dim oLoadCombo As New ClsUtilities

            'sSQL = "SELECT DESCRIZIONE_ENTE, COD_ENTE FROM ENTI WHERE COD_ENTE LIKE '" & Session("COD_ENTE") & "' ORDER BY DESCRIZIONE_ENTE"
            'Dim adoRecEnte = WFSessione.oSession.oAppDB.GetPrivateDataview(CStr(sSQL))
            'adoRecEnte = Nothing
            If Page.IsPostBack = False Then
                Dim sScript As String
                sScript = "parent.Comandi.location.href='ComandiRicerca.aspx';"
                sScript = sScript & "document.getElementById('ddlToponimo').focus();"
                RegisterScript(sScript, Me.GetType())

                oLoadCombo.PopolaComboVieFrazioni(ConstSession.IdEnte, ddlToponimo, ddlFrazione, ConstSession.StringConnection)
                GrdStrade.Visible = False
            End If
        Catch ex As Exception
            Log.Error("Si è verificato un errore in RicercaStrade::Page_Load::" & ex.Message)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Sub Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Search.Click
        Dim dtMyDati As New DataTable()
        Dim FncDB As New ClsDB

        Try
            dtMyDati = FncDB.GetSQLRicercaStrade(ConstSession.StringConnection, ConstSession.IdEnte, -1, ddlToponimo.SelectedValue, ddlFrazione.SelectedValue, TxtDenominazione.Text, TxtCAP.Text)
            If Not dtMyDati Is Nothing Then
                GrdStrade.DataSource = dtMyDati
                GrdStrade.DataBind()
                GrdStrade.Visible = True
            Else
                GrdStrade.Visible = False
            End If
            Session("dtMyDati") = dtMyDati
        Catch ex As Exception
            Log.Error("Si è verificato un errore in RicercaStrade::Search_Click::" & ex.Message)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
#Region "Griglie"
    Protected Sub GrdRowCommand(ByVal sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim IDRow As Integer = CInt(e.CommandArgument.ToString())
            For Each myRow As GridViewRow In GrdStrade.Rows
                If IDRow = CType(myRow.FindControl("hfId"), HiddenField).Value Then
                    RegisterScript("LoadStrade(" & IDRow & ")", Me.GetType())
                End If
            Next
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTERRITORIO.RicercaStrade.GrdRowCommand.errore: ", ex)
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
    'Private Sub GrdStrade_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles GrdStrade.ItemDataBound
    '    If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
    '        e.Item.Attributes.Add("onClick", "LoadStrade(" & e.Item.Cells(7).Text & ")")
    '    End If
    'End Sub
#End Region
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdStrade.DataSource = CType(Session("dtMyDati"), DataTable)
            If page.HasValue Then
                GrdStrade.PageIndex = page.Value
            End If
            GrdStrade.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTERRITORIO.RicercaStrade.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
End Class
