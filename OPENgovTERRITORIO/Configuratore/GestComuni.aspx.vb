Imports log4net
Imports OggettiComuniStrade

Public Class GestComuni
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger("GestComuni")
    Dim sScript As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Page.IsPostBack = False Then
                Dim fncActionEvent As New Utility.DBUtility(ConstSession.DBType, ConstSession.StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, ConstSession.UserName, "Territorio", "Ricerca", Utility.Costanti.AZIONE_LETTURA.ToString(), "", "", -1)
            End If
            sScript = ""
            If hdIdComune.Value = "-1" Then
                sScript += "$('#ViewAdd').hide();$('#SearchItem').show();"
                TxtComune.Text = ""
                TxtPV.Text = ""
                TxtCAP.Text = ""
                TxtBelfiore.Text = ""
                TxtIstat.Text = ""
            Else
                sScript += "$('#ViewAdd').show();$('#SearchItem').hide();"
            End If
            RegisterScript(sScript, Me.GetType)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTERRITORIO.GestComuni.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
#Region "Bottoni"
    Private Sub CmdRicerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdRicerca.Click
        Dim mySearch As New OggettoEnte
        Dim ListComuni() As OggettoEnte
        Dim TypeOfRI As Type = GetType(RemotingInterfaceOpenGovStradario.IRemotingInterfaceOpenGovStradario)
        Dim RemStradario As RemotingInterfaceOpenGovStradario.IRemotingInterfaceOpenGovStradario
        RemStradario = Activator.GetObject(TypeOfRI, ConstSession.UrlServizioStradario)

        Try
            mySearch.Denominazione = TxtSearchComune.Text
            mySearch.Provincia = TxtSearchPV.Text
            mySearch.Cap = TxtSearchCAP.Text
            mySearch.CodBelfiore = TxtSearchBelfiore.Text
            mySearch.CodIstat = TxtSearchIstat.Text
            ListComuni = RemStradario.GetArrayEnti(ConstSession.DBType, ConstSession.StringConnectionOPENgovCOMUNISTRADE, mySearch)
            If Not IsNothing(ListComuni) Then
                If ListComuni.Length > 0 Then
                    Session.Add("mySearchComuni", ListComuni)
                    GrdResult.DataSource = ListComuni
                    GrdResult.DataBind()
                Else
                    LblResult.Text = "La ricerca non ha prodotto risultati."
                    LblResult.Style.Add("display", "")
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTERRITORIO.GestComuni.CmdRicerca_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            DivAttesa.Style.Add("display", "none")
        End Try
    End Sub
    Private Sub CmdSalva_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdSalva.Click
        Dim myItem As New OggettoEnte

        Try
            myItem.ID = hdIdComune.Value
            myItem.Denominazione = TxtComune.Text
            myItem.Provincia = TxtPV.Text
            myItem.Cap = TxtCAP.Text
            myItem.CodBelfiore = TxtBelfiore.Text
            myItem.CodIstat = TxtIstat.Text

            If New ClsDB().SetComuni(ConstSession.StringConnectionOPENgovCOMUNISTRADE, myItem) = False Then
                sScript = "GestAlert('a', 'danger', '', '', 'Errore in salvataggio!');"
                RegisterScript(sScript, Me.GetType)
            Else
                hdIdComune.Value = "-1"
                sScript = "GestAlert('a', 'success', '', '', 'Salvataggio effettuato con successo!');"
                sScript += "$('#ViewAdd').hide();$('#SearchItem').show();"
                RegisterScript(sScript, Me.GetType)
                CmdRicerca_Click(Nothing, Nothing)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTERRITORIO.GestComuni.CmdSalva_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            DivAttesa.Style.Add("display", "none")
        End Try
    End Sub
    Private Sub CmdStampa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CmdStampa.Click
        Dim sNameXLS As String
        Dim ListComuni() As OggettoEnte
        Dim FncStampa As New ClsStampaXLS
        Dim DtDatiStampa As New DataTable
        Dim aListColonne As ArrayList
        Dim x, nCol As Integer
        Dim aMyHeaders As String()

        Try
            nCol = 5
            If Not (Session("mySearchComuni")) Is Nothing Then
                ListComuni = Session("mySearchComuni")
                DtDatiStampa = FncStampa.PrintComuni(ListComuni, nCol)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTERRITORIO.GestComuni.CmdStampa_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try

        If Not DtDatiStampa Is Nothing Then
            sNameXLS = "ELENCO_COMUNI_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"

            'definisco le colonne
            aListColonne = New ArrayList
            For x = 0 To nCol
                aListColonne.Add("")
            Next
            aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

            'definisco l'insieme delle colonne da esportare
            Dim MyCol() As Integer = New Integer(nCol) {}
            For x = 0 To nCol
                MyCol(x) = x
            Next
            'esporto i dati in excel
            Dim MyStampa As New RKLib.ExportData.Export("Web")
            MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
        Else
            Dim sScript As String = "GestAlert('a', 'danger', '', '', 'Errore in estrazione!');"
            sScript += "parent.parent.Visualizza.DivAttesa.style.display='none';"
            RegisterScript(sScript, Me.GetType)
        End If
    End Sub
#End Region
#Region "Griglie"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Try
            Dim x As Integer = 0
            Dim IDRow As String = e.CommandArgument.ToString()
            If e.CommandName = "RowOpen" Then
                For Each myRow As GridViewRow In GrdResult.Rows
                    If IDRow = CType(myRow.FindControl("hfId"), HiddenField).Value Then
                        hdIdComune.Value = IDRow
                        x = 0
                        TxtComune.Text = myRow.Cells(x).Text.Replace("&nbsp;", "")
                        x += 1
                        TxtPV.Text = myRow.Cells(x).Text.Replace("&nbsp;", "")
                        x += 1
                        TxtCAP.Text = myRow.Cells(x).Text.Replace("&nbsp;", "")
                        x += 1
                        TxtBelfiore.Text = myRow.Cells(x).Text.Replace("&nbsp;", "")
                        x += 1
                        TxtIstat.Text = myRow.Cells(x).Text.Replace("&nbsp;", "")
                    End If
                Next
                sScript = "$('#ViewAdd').show(); $('#SearchItem').hide();"
                RegisterScript(sScript, Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTERRITORIO.GestComuni.GrdRowCommand.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Gestione del cambio pagina della griglia.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub GrdPageIndexChanging(ByVal sender As Object, e As GridViewPageEventArgs)
        Try
            LoadSearch(e.NewPageIndex)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTERRITORIO.GestComuni.GrdPageIndexChanging.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' Funzione per il popolamento della griglia con riposizionamento nella pagina selezionata.
    ''' </summary>
    ''' <param name="page"></param>
    Private Sub LoadSearch(Optional ByVal page As Integer? = 0)
        Try
            GrdResult.DataSource = CType(Session("mySearchComuni"), OggettoEnte())
            If page.HasValue Then
                GrdResult.PageIndex = page.Value
            End If
            GrdResult.DataBind()
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovTERRITORIO.GestComuni.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
#End Region
End Class