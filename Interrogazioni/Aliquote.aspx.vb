Imports log4net
''' <summary>
''' Pagina la consultazione da cruscotto delle aliquote.
''' Le possibili opzioni sono:
''' - Stampa
''' - Ricerca
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class Aliquote '*** 201511 - Funzioni Sovracomunali ***
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(Aliquote))
    Private myParamSearch As New ObjInterAliquoteSearch
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            lblTitolo.Text = COSTANTValue.ConstSession.DescrizioneEnte
            If Not Page.IsPostBack Then
                LoadCombo()
                GestOpt("")
                GrdResult.Visible = False
                Dim fncActionEvent As New Utility.DBUtility(COSTANTValue.ConstSession.DBType, COSTANTValue.ConstSession.StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, COSTANTValue.ConstSession.UserName, "Cruscotto", "Aliquote", Utility.Costanti.AZIONE_LETTURA.ToString, COSTANTValue.ConstSession.CodTributo, COSTANTValue.ConstSession.IdEnte, -1)
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.Aliquote.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub ddlTributo_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlTributo.SelectedIndexChanged
        Try
            GestOpt(ddlTributo.SelectedValue)
            GrdResult.Visible = False
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.Aliquote.ddlTributo_SelectedIndexChanged.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnRicerca_Click(sender As Object, e As System.EventArgs) Handles btnRicerca.Click
        Dim dsMyDati As New DataSet
        Dim FncInt As New clsInterrogazioni

        Try
            GrdResult.Columns.Clear()
            If ddlTributo.SelectedValue = "" Then
                Dim sScript As String = "GestAlert('a', 'warning', '', '', 'Selezionare un tributo!');"
                sScript += "document.getElementById('optTARSU').style.display='none';"
                sScript += "document.getElementById('optICI').style.display='none';"
                sScript += "document.getElementById('optH2O').style.display='none';"
                RegisterScript(sScript, Me.GetType())
            Else
                myParamSearch = New ObjInterAliquoteSearch
                myParamSearch = GetParamSearch()
                Session("mySearchInterGen") = myParamSearch
                dsMyDati = FncInt.GetInterAliquote(COSTANTValue.ConstSession.StringConnectionOPENgov, myParamSearch)
                If Not IsNothing(dsMyDati) Then
                    For Each myCol As DataColumn In dsMyDati.Tables(0).Columns
                        Dim myGrdCol As New BoundField
                        myGrdCol.HeaderText = myCol.ColumnName
                        myGrdCol.DataField = myCol.ColumnName
                        GrdResult.Columns.Add(myGrdCol)
                    Next
                    GrdResult.DataSource = dsMyDati.Tables(0)
                    GrdResult.DataBind()
                    If GrdResult.Rows.Count > 0 Then
                        LblResult.Style.Add("display", "none")
                        GrdResult.Visible = True
                    Else
                        LblResult.Text = "La ricerca non ha prodotto risultati."
                        LblResult.Style.Add("display", "")
                        GrdResult.Visible = False
                    End If
                End If
                GestOpt(ddlTributo.SelectedValue)
            End If
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.Aliquote.btnRicerca_Click.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            DivAttesa.Style.Add("display", "none")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnStampaExcel_Click(sender As Object, e As System.EventArgs) Handles btnStampaExcel.Click
        Dim sNameXLS, Str As String
        Dim DtDatiStampa As New DataTable
        Dim aMyHeaders As String()
        Dim aListColonne As ArrayList
        Dim FncStampa As New ClsStampaXLS
        Dim x As Integer
        Dim dsMyDati As New DataSet
        Dim FncInt As New clsInterrogazioni

        Try
            DivAttesa.Style.Add("display", "")
            If ddlTributo.SelectedValue = "" Then
                Dim sScript As String = "GestAlert('a', 'warning', '', '', 'Selezionare un tributo!');"
                sScript += "document.getElementById('optTARSU').style.display='none';"
                sScript += "document.getElementById('optICI').style.display='none';"
                sScript += "document.getElementById('optH2O').style.display='none';"
                RegisterScript(sScript, Me.GetType())
            Else
                myParamSearch = New ObjInterAliquoteSearch
                myParamSearch = getparamsearch
                Session("mySearchInterGen") = myParamSearch
                dsMyDati = FncInt.GetInterAliquote(COSTANTValue.ConstSession.StringConnectionOPENgov, myParamSearch)

                GestOpt(ddlTributo.SelectedValue)
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.Aliquote.btnStampaExcel_Click.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        Finally
            DivAttesa.Style.Add("display", "none")
            'WFSessione.Kill()
        End Try
        If Not IsNothing(dsMyDati) Then
            'valorizzo il nome del file
            'sPathProspetti = COSTANTValue.ConstSession.PathProspetti
            sNameXLS = COSTANTValue.ConstSession.IdEnte & "_ALIQUOTE_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
            Dim nCol As Integer = 0
            DtDatiStampa = FncStampa.PrintAliquote(dsMyDati, "Aliquote", nCol)
            If Not DtDatiStampa Is Nothing Then
                'definisco le colonne
                aListColonne = New ArrayList
                For x = 0 To nCol
                    aListColonne.Add("")
                Next
                aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())
                'definisco l'insieme delle colonne da esportare
                Dim MyCol() As Integer = New Integer(nCol) {} '{0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13}
                For x = 0 To nCol
                    MyCol(x) = x
                Next

                'esporto i dati in excel
                Dim MyStampa As New RKLib.ExportData.Export("Web")
                MyStampa.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
                Str = sNameXLS
            End If
        End If
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub LoadCombo()
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim myDataReader As SqlClient.SqlDataReader
        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(COSTANTValue.ConstSession.StringConnectionOPENgov)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_GetTributi"
            cmdMyCommand.Parameters.Clear()
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myDataReader = cmdMyCommand.ExecuteReader
            Try
                ddlTributo.Items.Clear()
                ddlTributo.Items.Add("...")
                ddlTributo.Items(0).Value = ""
                If Not myDataReader Is Nothing Then
                    Do While myDataReader.Read
                        If Not IsDBNull(myDataReader(0)) Then
                            ddlTributo.Items.Add(myDataReader(0).ToString)
                            ddlTributo.Items(ddlTributo.Items.Count - 1).Value = myDataReader(1).ToString
                        End If
                    Loop
                End If
            Catch ex As Exception
                Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.Aliquote.LoadCombo.errore: ", ex)
                Response.Redirect("../../PaginaErrore.aspx")
            Finally
                myDataReader.Close()
            End Try
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.Aliquote.LoadCombo.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw New Exception("Problemi nell'esecuzione di Aliquote::LoadCombo" + ex.Message)
        Finally
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="IdTributo"></param>
    Private Sub GestOpt(IdTributo As String)
        Try
            Dim sScript As String = ""
            Select Case IdTributo
                Case Utility.Costanti.TRIBUTO_TARSU
                    sScript += "document.getElementById('optTARSU').style.display='';"
                    sScript += "document.getElementById('optICI').style.display='none';"
                    sScript += "document.getElementById('optH2O').style.display='none';"
                    optTARSUTariffe.Checked = True : optICIIMU.Checked = False : optH2OAddizionali.Checked = False
                Case Utility.Costanti.TRIBUTO_ICI
                    sScript += "document.getElementById('optTARSU').style.display='none';"
                    sScript += "document.getElementById('optICI').style.display='none';"
                    sScript += "document.getElementById('optH2O').style.display='none';"
                    optTARSUTariffe.Checked = False : optICIIMU.Checked = True : optH2OAddizionali.Checked = False
                Case Utility.Costanti.TRIBUTO_TASI
                    sScript += "document.getElementById('optTARSU').style.display='none';"
                    sScript += "document.getElementById('optICI').style.display='none';"
                    sScript += "document.getElementById('optH2O').style.display='none';"
                    optTARSUTariffe.Checked = False : optICIIMU.Checked = False : optICITASI.Checked = True : optH2OAddizionali.Checked = False
                Case Utility.Costanti.TRIBUTO_H2O
                    sScript += "document.getElementById('optTARSU').style.display='none';"
                    sScript += "document.getElementById('optICI').style.display='none';"
                    sScript += "document.getElementById('optH2O').style.display='';"
                    optTARSUTariffe.Checked = False : optICIIMU.Checked = False : optH2OAddizionali.Checked = True
                Case Utility.Costanti.TRIBUTO_OSAP, Utility.Costanti.TRIBUTO_SCUOLE, ""
                    sScript += "document.getElementById('optTARSU').style.display='none';"
                    sScript += "document.getElementById('optICI').style.display='none';"
                    sScript += "document.getElementById('optH2O').style.display='none';"
                    optTARSUTariffe.Checked = False : optICIIMU.Checked = False : optH2OAddizionali.Checked = False
                Case Else
                    sScript += "GestAlert('a', 'warning', '', '', 'Selezionare un tributo!');"
                    sScript += "document.getElementById('optTARSU').style.display='none';"
                    sScript += "document.getElementById('optICI').style.display='none';"
                    sScript += "document.getElementById('optH2O').style.display='none';"
                    optTARSUTariffe.Checked = False : optICIIMU.Checked = False : optH2OAddizionali.Checked = False
            End Select
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.Aliquote.GestOpt.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
            Throw New Exception("Problemi nell'esecuzione di Aliquote::GestOpt" + ex.Message)
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Tipo"></param>
    Private Sub LoadOpt(Tipo As TipoInterAliquote)
        Try
            Select Case Tipo
                Case TipoInterAliquote.Tariffe
                    optTARSUTariffe.Checked = True
                Case TipoInterAliquote.Riduzioni
                    optTARSURiduzioni.Checked = True
                Case TipoInterAliquote.Esenzioni
                    optTARSUEsenzioni.Checked = True
                Case TipoInterAliquote.ICI
                    optICIIMU.Checked = True
                Case TipoInterAliquote.TASI
                    optICITASI.Checked = True
                Case TipoInterAliquote.Addizionali
                    optH2OAddizionali.Checked = True
                Case TipoInterAliquote.Canoni
                    optH2OCanoni.Checked = True
                Case TipoInterAliquote.Scaglioni
                    optH2OScaglioni.Checked = True
                Case TipoInterAliquote.Nolo
                    optH2ONolo.Checked = True
                Case TipoInterAliquote.QuotaFissa
                    optH2OQuotaFissa.Checked = True
            End Select
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.Aliquote.LoadOpt.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    Private Function GetParamSearch() As ObjInterAliquoteSearch
        Dim myParam As New ObjInterAliquoteSearch
        Try
            myParam = New ObjInterAliquoteSearch
            myParam.Ambiente = COSTANTValue.ConstSession.Ambiente
            If COSTANTValue.ConstSession.IdEnte <> "" Then
                myParam.IdEnte = COSTANTValue.ConstSession.IdEnte
            End If
            myParam.Anno = txtAnno.Text
            myParam.IdTributo = ddlTributo.SelectedValue
            If optTARSUTariffe.Checked Then
                myParam.Tipo = TipoInterAliquote.Tariffe
            ElseIf optTARSURiduzioni.Checked Then
                myParam.Tipo = TipoInterAliquote.Riduzioni
            ElseIf optTARSUEsenzioni.Checked Then
                myParam.Tipo = TipoInterAliquote.Esenzioni
            ElseIf optICIIMU.Checked Then
                myParam.Tipo = TipoInterAliquote.ICI
            ElseIf optICITASI.Checked Then
                myParam.Tipo = TipoInterAliquote.TASI
            ElseIf optH2OAddizionali.Checked Then
                myParam.Tipo = TipoInterAliquote.Addizionali
            ElseIf optH2OCanoni.Checked Then
                myParam.Tipo = TipoInterAliquote.Canoni
            ElseIf optH2OScaglioni.Checked Then
                myParam.Tipo = TipoInterAliquote.Scaglioni
            ElseIf optH2ONolo.Checked Then
                myParam.Tipo = TipoInterAliquote.Nolo
            ElseIf optH2OQuotaFissa.Checked Then
                myParam.Tipo = TipoInterAliquote.QuotaFissa
            Else
                If myParam.IdTributo = Utility.Costanti.TRIBUTO_OSAP Then
                    myParam.Tipo = TipoInterAliquote.TariffeOSAP
                Else
                    myParam.Tipo = TipoInterAliquote.TariffeScuola
                End If
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.Aliquote.GetParamSearch.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
        Return myParam
    End Function
End Class

'Public Class TemplateHandler
'    Implements ITemplate

'    Sub ITemplate_InstantiateIn(ByVal container As Control) Implements ITemplate.InstantiateIn
'        Dim myGrdCmdImg As New ImageButton
'        myGrdCmdImg.ID = "ImgGrafico"
'        myGrdCmdImg.CommandName = "Update"
'        myGrdCmdImg.ImageUrl = "../images/Bottoni/Bar-chart-icon.png"
'        myGrdCmdImg.ToolTip = "Grafico per ente"
'        'myGrdCmdImg.OnClientClick = "document.getElementById('btnGrafico').click();"
'        container.Controls.Add(myGrdCmdImg)
'    End Sub
'End Class