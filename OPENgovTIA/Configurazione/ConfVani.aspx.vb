Imports log4net
''' <summary>
''' Pagina per la consultazione dei vani.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class ConfVani
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ConfVani))
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ConfVani_Init(sender As Object, e As EventArgs) Handles Me.Init
        'Put user code to initialize the page here
        lblTitolo.Text = Session("DESCRIZIONE_ENTE")
        Try
            If ConstSession.IsFromTARES = "1" Then
                info.Text = "TARES " + info.Text
            Else
                info.Text = "TARSU " + info.Text
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ConfVani.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdSearch_Click(sender As Object, e As EventArgs) Handles CmdSearch.Click
        Try
            Dim clsVani As New ClsVani
            Dim dsVani As New DataSet
            dsVani = clsVani.GetVani("")

            If dsVani.Tables.Item(0).Rows.Count > 0 Then
                ViewState.Add("dsVANI", dsVani)
                ViewState.Add("SortKey", "IDTIPOVANO")
                ViewState.Add("OrderBy", "ASC")

                Session("dsVANI") = dsVani
                GrdResult.DataSource = dsVani
                GrdResult.DataBind()
            Else
                LblResult.Text = "La ricerca non ha prodotto risultati."
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ConfVani.CmdSearch_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
#Region "Griglie"
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
            GrdResult.DataSource = CType(Session("dsVANI"), DataSet)
            If page.HasValue Then
                GrdResult.PageIndex = page.Value
            End If
            GrdResult.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.ConfVani.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
End Class