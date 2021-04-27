Imports log4net
''' <summary>
''' Pagina la consultazione da cruscotto del riepilogo della situazione fatturato/incassato di tutti i tributi gestiti.
''' Le possibili opzioni sono:
''' - Stampa
''' - Ricerca
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Public Class Cruscotto
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(Cruscotto))
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sMyEnte As String = ""
        Try
            Log.Debug("entrata in cruscotto")
            If Not Page.IsPostBack Then
                LoadCombos()
                sMyEnte = COSTANTValue.ConstSession.IdEnte
                If Not Request.Item("Ente") Is Nothing Then
                    If Request.Item("Ente") <> "" Then
                        sMyEnte = Request.Item("Ente")
                    End If
                End If
                If sMyEnte <> "" Then
                    ddlEnti.SelectedValue = sMyEnte
                    Dim strscript As String = "document.getElementById ('lblEnti').style.display='none';"
                    strscript += "document.getElementById ('ddlEnti').style.display='none';"
                    RegisterScript(strscript, Me.GetType())
                End If
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.Cruscotto.Page_Load.errore: ", ex)
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
            GrdResult.DataSource = CType(Session("DsCruscotto"), DataSet)
            If page.HasValue Then
                GrdResult.PageIndex = page.Value
            End If
            GrdResult.DataBind()
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.Cruscotto.LoadSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnRicerca_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRicerca.Click
        Dim DsResult As New DataSet
        Dim FncRes As New clsInterrogazioni

        Try
            DsResult = FncRes.GetInterrogazioneCruscotto(COSTANTValue.ConstSession.StringConnectionOPENgov, COSTANTValue.ConstSession.Ambiente, ddlEnti.SelectedValue, txtAnno.Text, ddlTributo.SelectedValue)
            If Not IsNothing(DsResult) Then
                GrdResult.DataSource = DsResult.Tables(0)
                GrdResult.DataBind()
                Session("DsCruscotto") = DsResult
            End If
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.Cruscotto.btnRicerca_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            DivAttesa.Style.Add("display", "none")
            GrdResult.Style.Add("display", "")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnStampaExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStampaExcel.Click
        Dim sNameXLS As String
        Dim ds As New DataSet
        Dim DtDatiStampa As New DataTable
        Dim aListColonne As ArrayList
        Dim aMyHeaders As String()
        Dim nCol As Integer = 6
        Dim x As Integer
        Dim dvDati As DataView
        Dim FncStampa As New ClsStampaXLS

        Try
            ds = New DataSet
            ds.Tables.Add("STAMPA")
            For x = 1 To nCol + 1
                ds.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, CChar("0")))
            Next
            DtDatiStampa = ds.Tables("STAMPA")
            If Not IsNothing(Session("DsCruscotto")) Then
                ds = CType(Session("DsCruscotto"), DataSet)
                dvDati = ds.Tables(0).DefaultView

                DtDatiStampa = FncStampa.PrintCruscotto(DtDatiStampa, dvDati)
                If DtDatiStampa Is Nothing Then
                    Throw New Exception("Errore in stampa")
                End If
            Else
                DtDatiStampa = Nothing
                RegisterScript("GestAlert('a', 'warning', '', '', 'Si sono verificati dei problemi nello stradario. Contattare il servizio di assistenza!');", Me.GetType)
            End If
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.Cruscotto.btnStampaExcel_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            DivAttesa.Style.Add("display", "none")
        End Try
        If Not IsNothing(DtDatiStampa) Then
            'valorizzo il nome del file
            sNameXLS = COSTANTValue.ConstSession.IdEnte & "_CRUSCOTTO_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
            'definisco le colonne
            aListColonne = New ArrayList
            For x = 0 To nCol
                aListColonne.Add("")
            Next
            aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

            'Dim MyCol As Integer() = {0, 1, 2, 3, 4, 5, 6}
            Dim MyCol() As Integer = New Integer(nCol) {}
            For x = 0 To nCol
                MyCol(x) = x
            Next
            Dim objExport As New RKLib.ExportData.Export("Web")
            objExport.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
        End If
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub LoadCombos()
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myDataReader As SqlClient.SqlDataReader
        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(COSTANTValue.ConstSession.StringConnectionOPENgov)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "ENTI_S"
            cmdMyCommand.Parameters.Add("@AMBIENTE", SqlDbType.NVarChar).Value = COSTANTValue.ConstSession.Ambiente
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myDataReader = cmdMyCommand.ExecuteReader
            Try
                ddlEnti.Items.Clear()
                ddlEnti.Items.Add("...")
                ddlEnti.Items(0).Value = ""
                If Not myDataReader Is Nothing Then
                    Do While myDataReader.Read
                        If Not IsDBNull(myDataReader(0)) Then
                            ddlEnti.Items.Add(myDataReader(1).ToString)
                            ddlEnti.Items(ddlEnti.Items.Count - 1).Value = myDataReader(0).ToString
                        End If
                    Loop
                End If
            Catch ex As Exception
                Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.Cruscotto.LoadCombos.errore: ", ex)
                Response.Redirect("../../PaginaErrore.aspx")
            Finally
                myDataReader.Close()
            End Try

            cmdMyCommand.CommandText = "prc_GetTributi"
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.AddWithValue("@TIPORICERCA", "CRUSCOTTO")
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myDataReader = cmdMyCommand.ExecuteReader
            Try
                ddlTributo.Items.Clear()
                If Not myDataReader Is Nothing Then
                    Do While myDataReader.Read
                        If Not IsDBNull(myDataReader(0)) Then
                            ddlTributo.Items.Add(myDataReader(0).ToString)
                            ddlTributo.Items(ddlTributo.Items.Count - 1).Value = myDataReader(1).ToString
                        End If
                    Loop
                End If
            Catch ex As Exception
                Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.Cruscotto.LoadCombos.errore: ", ex)
                Response.Redirect("../../PaginaErrore.aspx")
            Finally
                myDataReader.Close()
            End Try
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.Cruscotto.LoadCombos.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
    End Sub
End Class