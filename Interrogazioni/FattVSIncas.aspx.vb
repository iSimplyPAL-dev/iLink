Imports log4net
''' <summary>
''' Pagina per la consultazione da cruscotto della situazione fatturato/incassato IMU/TASI o TARI.
''' Le possibili opzioni sono:
''' - Stampa
''' - Ricerca
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class FattVSIncas '*** 201511 - Funzioni Sovracomunali ***
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(FattVSIncas))
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            lblTitolo.Text = COSTANTValue.ConstSession.DescrizioneEnte
            info.InnerText = "Analisi"
            Select Case Request.Item("Tributo")
                Case Utility.Costanti.TRIBUTO_TARSU
                    info.InnerText += " TARI"
                Case Utility.Costanti.TRIBUTO_ICI, Utility.Costanti.TRIBUTO_TASI
                    info.InnerText += " ICI/IMU/TASI"
                Case Utility.Costanti.TRIBUTO_H2O
                    info.InnerText += " Acquedotto"
            End Select
            If Not Page.IsPostBack Then
                LoadCombos()
            End If
            GestOpt(Request.Item("Tributo"))
            If COSTANTValue.ConstSession.IdEnte <> "" Then
                ddlEnti.SelectedValue = COSTANTValue.ConstSession.IdEnte
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FattVSIncas.Page_Load.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
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
        Dim myParamSearch As New ObjInterFattVSIncasSearch
        Dim nResult As Integer = 0
        Try
            Session("dsFattVSIncas") = Nothing
            myParamSearch = New ObjInterFattVSIncasSearch
            myParamSearch.IdTributo = Request.Item("Tributo")
            If optICITASI.Checked Then
                myParamSearch.IdTributo = Utility.Costanti.TRIBUTO_TASI
            End If
            myParamSearch.Ambiente = COSTANTValue.ConstSession.Ambiente
            myParamSearch.IdEnte = ddlEnti.SelectedValue
            myParamSearch.Anno = txtAnno.Text
            If Request.Item("Tributo") = Utility.Costanti.TRIBUTO_TARSU Then
                myParamSearch.Param3 = ddlParam3.SelectedValue
            Else
                myParamSearch.Param3 = txtParam3.Text
            End If
            If txtDal.Text <> "" Then
                myParamSearch.tDal = txtDal.Text
            End If
            If txtAl.Text <> "" Then
                myParamSearch.tAl = txtAl.Text
            End If
            If optAcconto.Checked Then
                myParamSearch.Rata = 1
            ElseIf optSaldo.Checked Then
                myParamSearch.Rata = 2
            End If
            dsMyDati = FncInt.GetInterFattVSIncas(COSTANTValue.ConstSession.StringConnectionOPENgov, myParamSearch)
            If Not IsNothing(dsMyDati) Then
                Session("dsFattVSIncas") = dsMyDati
                Dim GrdEmesso As Ribes.OPENgov.WebControls.RibesGridView
                Dim GrdNetto As Ribes.OPENgov.WebControls.RibesGridView
                Dim GrdPagato As Ribes.OPENgov.WebControls.RibesGridView
                Dim LblEmesso, LblNetto, LblPagato As Label

                Select Case Request.Item("Tributo")
                    Case Utility.Costanti.TRIBUTO_TARSU
                        GrdEmesso = GrdTARSUEmesso
                        GrdNetto = GrdTARSUNetto
                        GrdPagato = GrdTARSUIncassato
                        LblEmesso = LblTARSUEmesso
                        LblNetto = LblTARSUNetto
                        LblPagato = LblTARSUIncassato
                    Case Utility.Costanti.TRIBUTO_ICI, Utility.Costanti.TRIBUTO_TASI
                        GrdPagato = GrdICIIncassato
                        GrdEmesso = GrdICIEmesso
                        GrdNetto = Nothing
                        LblEmesso = LblICIEmesso
                        LblNetto = Nothing
                        LblPagato = LblICIIncassato
                    Case Utility.Costanti.TRIBUTO_H2O
                        GrdEmesso = GrdH2OEmesso
                        GrdPagato = GrdH2OIncassato
                        GrdNetto = Nothing
                        LblEmesso = LblH2OEmesso
                        LblNetto = Nothing
                        LblPagato = LblH2OIncassato
                End Select
                If Not GrdEmesso Is Nothing Then
                    GrdEmesso.DataSource = dsMyDati.Tables("EMESSO")
                    GrdEmesso.DataBind()
                    If txtAnno.Text <> "" And txtParam3.Text <> "" Then
                        GrdEmesso.Columns(2).Visible = True
                    End If
                    If GrdEmesso.Rows.Count > 0 Then
                        nResult += GrdEmesso.Rows.Count
                        Dim x As Integer = 5
                        If Request.Item("Tributo") = Utility.Costanti.TRIBUTO_ICI Then
                            GrdEmesso.Columns(x).HeaderText = "Abi. Princ. (3912)" : x += 1
                            GrdEmesso.Columns(x).HeaderText = "Altri Fab. (3918)" : x += 2
                            GrdEmesso.Columns(x).HeaderText = "Aree Fab. (3916)" : x += 4
                            GrdEmesso.Columns(x).HeaderText = "Fab.Rur. (3913)"
                        ElseIf Request.Item("Tributo") = Utility.Costanti.TRIBUTO_TASI Then
                            GrdEmesso.Columns(x).HeaderText = "Abi. Princ. (3958)" : x += 1
                            GrdEmesso.Columns(x).HeaderText = "Altri Fab. (3961)" : x += 2
                            GrdEmesso.Columns(x).HeaderText = "Aree Fab. (3960)" : x += 4
                            GrdEmesso.Columns(x).HeaderText = "Fab.Rur. (3959)"
                        End If
                        GrdEmesso.Visible = True
                            LblEmesso.Visible = True
                        Else
                            GrdEmesso.Visible = False
                        LblEmesso.Visible = False
                    End If
                End If
                If Not GrdNetto Is Nothing Then
                    GrdNetto.DataSource = dsMyDati.Tables("NETTO")
                    GrdNetto.DataBind()
                    If txtAnno.Text <> "" And txtParam3.Text <> "" Then
                        GrdNetto.Columns(2).Visible = True
                    End If
                    If GrdNetto.Rows.Count > 0 Then
                        nResult += GrdNetto.Rows.Count
                        GrdNetto.Visible = True
                        LblNetto.Visible = True
                    Else
                        GrdNetto.Visible = False
                        LblNetto.Visible = False
                    End If
                End If
                If Not GrdPagato Is Nothing Then
                    GrdPagato.DataSource = dsMyDati.Tables("PAGATO")
                    GrdPagato.DataBind()
                    If txtAnno.Text <> "" And txtParam3.Text <> "" Then
                        GrdPagato.Columns(2).Visible = True
                    End If
                    If GrdPagato.Rows.Count > 0 Then
                        nResult += GrdPagato.Rows.Count
                        Dim x As Integer = 3
                        If Request.Item("Tributo") = Utility.Costanti.TRIBUTO_ICI Then
                            GrdPagato.Columns(x).HeaderText = "Abi. Princ. (3912)" : x += 1
                            GrdPagato.Columns(x).HeaderText = "Altri Fab. (3918)" : x += 2
                            GrdPagato.Columns(x).HeaderText = "Aree Fab. (3916)" : x += 4
                            GrdPagato.Columns(x).HeaderText = "Fab.Rur. (3913)"
                        ElseIf Request.Item("Tributo") = Utility.Costanti.TRIBUTO_TASI Then
                            GrdPagato.Columns(x).HeaderText = "Abi. Princ. (3958)" : x += 1
                            GrdPagato.Columns(x).HeaderText = "Altri Fab. (3961)" : x += 2
                            GrdPagato.Columns(x).HeaderText = "Aree Fab. (3960)" : x += 4
                            GrdPagato.Columns(x).HeaderText = "Fab.Rur. (3959)"
                        End If
                        GrdPagato.Visible = True
                        LblPagato.Visible = True
                    Else
                        GrdPagato.Visible = False
                        LblPagato.Visible = False
                    End If
                End If
                If nResult = 0 Then
                    LblResult.Text = "La ricerca non ha prodotto risultati."
                    LblResult.Style.Add("display", "")
                Else
                    LblResult.Style.Add("display", "none")
                End If
            End If

            GestOpt(Request.Item("Tributo"))
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FattVSIncas.btnRicerca_Click.errore: ", Err)
            Response.Redirect("../PaginaErrore.aspx")
        Finally
            DivAttesa.Style.Add("display", "none")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Private Sub btnGrafico_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGrafico.Click
        Dim sScript As String = ""
        Dim ListSlices As New Generic.List(Of Generic.List(Of String))
        Try
            If Request.Item("tributo") = Utility.Costanti.TRIBUTO_TARSU Then
                For Each myRow As GridViewRow In GrdTARSUIncassato.Rows
                    sScript = ""
                    For Each mySlice As Generic.List(Of String) In GetSlicesTARSU(myRow, True)
                        ListSlices.Add(mySlice)
                    Next
                Next
                If ListSlices.Count > 0 Then
                    sScript += DrawColumnChart(ListSlices, "Percentuale insoluti")
                Else
                    sScript += "GestAlert('a', 'warning', '', '', 'I dati selezionati non permettono la generazione di un grafico.');"
                End If
                RegisterScript(sScript, Me.GetType())
                GestOpt(Request.Item("Tributo"))
                DivGrafico.Style.Add("display", "")
                DivAttesa.Style.Add("display", "none")
            ElseIf Request.Item("tributo") = Utility.Costanti.TRIBUTO_H2O Then
                For Each myRow As GridViewRow In GrdH2OIncassato.Rows
                    sScript = ""
                    For Each mySlice As Generic.List(Of String) In GetSlicesH2O(myRow, True)
                        ListSlices.Add(mySlice)
                    Next
                    If ListSlices.Count > 0 Then
                        sScript += DrawColumnChart(ListSlices, "Percentuale insoluti")
                    Else
                        sScript += "GestAlert('a', 'warning', '', '', 'I dati selezionati non permettono la generazione di un grafico.');"
                    End If
                    RegisterScript(sScript, Me.GetType())
                    GestOpt(Request.Item("Tributo"))
                    DivGrafico.Style.Add("display", "")
                    DivAttesa.Style.Add("display", "none")
                Next
            End If
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FattVSIncas.btnGrafico_Click.errore: ", Err)
            Response.Redirect("../PaginaErrore.aspx")
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
        Dim nCol As Integer = 0
        Dim x As Integer
        Dim FncStampa As New ClsStampaXLS

        Try
            If Not IsNothing(Session("dsFattVSIncas")) Then
                ds = New DataSet
                ds.Tables.Add("STAMPA")
                If Not IsNothing(CType(Session("dsFattVSIncas"), DataSet).Tables("EMESSO")) Then
                    nCol = CType(Session("dsFattVSIncas"), DataSet).Tables("EMESSO").Columns.Count
                End If
                If Not IsNothing(CType(Session("dsFattVSIncas"), DataSet).Tables("NETTO")) Then
                    If CType(Session("dsFattVSIncas"), DataSet).Tables("NETTO").Columns.Count > nCol Then
                        nCol = CType(Session("dsFattVSIncas"), DataSet).Tables("NETTO").Columns.Count
                    End If
                End If
                If Not IsNothing(CType(Session("dsFattVSIncas"), DataSet).Tables("PAGATO")) Then
                    If CType(Session("dsFattVSIncas"), DataSet).Tables("PAGATO").Columns.Count > nCol Then
                        nCol = CType(Session("dsFattVSIncas"), DataSet).Tables("PAGATO").Columns.Count
                    End If
                End If
                For x = 1 To nCol + 1
                    ds.Tables("STAMPA").Columns.Add("Col" & x.ToString.PadLeft(3, CChar("0")))
                Next
                DtDatiStampa = ds.Tables("STAMPA")

                ds = CType(Session("dsFattVSIncas"), DataSet)
                DtDatiStampa = FncStampa.PrintFattVSIncas(ds, "Analisi", nCol)
            End If
            GestOpt(Request.Item("Tributo"))
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FattVSIncas.btnStampaExcel_Click.errore: ", Err)
            Response.Redirect("../PaginaErrore.aspx")
        Finally
            DivAttesa.Style.Add("display", "none")
        End Try
        If Not IsNothing(DtDatiStampa) Then
            'valorizzo il nome del file
            sNameXLS = COSTANTValue.ConstSession.IdEnte & "_ANALISI_" & Format(Now, "yyyyMMdd_hhmmss") & ".xls"
            'definisco le colonne
            aListColonne = New ArrayList
            For x = 0 To nCol
                aListColonne.Add("")
            Next
            aMyHeaders = CType(aListColonne.ToArray(GetType(String)), String())

            Dim MyCol() As Integer = New Integer(nCol) {}
            For x = 0 To nCol
                MyCol(x) = x
            Next
            Dim objExport As New RKLib.ExportData.Export("Web")
            objExport.ExportDetails(DtDatiStampa, MyCol, aMyHeaders, RKLib.ExportData.Export.ExportFormat.Excel, sNameXLS)
        End If
    End Sub

#Region "Griglie"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
        Dim sScript As String = ""
        Dim ListSlices As New Generic.List(Of Generic.List(Of String))
        Try
            Dim IDRow As String = e.CommandArgument.ToString()
            If e.CommandName = "RowOpen" Then
                Select Case CType(sender, Ribes.OPENgov.WebControls.RibesGridView).UniqueID
                    Case "GrdICIIncassato"
                        For Each myRow As GridViewRow In GrdICIIncassato.Rows
                            If IDRow = myRow.Cells(0).Text Then
                                sScript = ""
                                ListSlices = GetSlicesICI(myRow)
                                If ListSlices.Count > 0 Then
                                    sScript += DrawPieChart(ListSlices, "")
                                Else
                                    'sScript += "GestAlert('a', 'warning', '', '', 'I dati selezionati non permettono la generazione di un grafico.');"
                                End If
                                RegisterScript(sScript, Me.GetType())
                                GestOpt(Request.Item("Tributo"))
                                DivGrafico.Style.Add("display", "")
                                DivAttesa.Style.Add("display", "none")
                            End If
                        Next
                    Case "GrdTARSUEmesso"
                        For Each myRow As GridViewRow In GrdTARSUEmesso.Rows
                            If IDRow = myRow.Cells(0).Text Then
                                sScript = ""
                                ListSlices = GetSlicesTARSU(myRow, False)
                                If ListSlices.Count > 0 Then
                                    sScript += DrawPieChart(ListSlices, "")
                                Else
                                    sScript += "GestAlert('a', 'warning', '', '', 'I dati selezionati non permettono la generazione di un grafico.');"
                                End If
                                RegisterScript(sScript, Me.GetType())
                                GestOpt(Request.Item("Tributo"))
                                DivGrafico.Style.Add("display", "")
                                DivAttesa.Style.Add("display", "none")
                            End If
                        Next
                    Case "GrdH2OEmesso"
                        For Each myRow As GridViewRow In GrdH2OEmesso.Rows
                            If IDRow = myRow.Cells(0).Text Then
                                sScript = ""
                                ListSlices = GetSlicesH2O(myRow, False)
                                If ListSlices.Count > 0 Then
                                    sScript += DrawPieChart(ListSlices, "")
                                Else
                                    sScript += "GestAlert('a', 'warning', '', '', 'I dati selezionati non permettono la generazione di un grafico.');"
                                End If
                                RegisterScript(sScript, Me.GetType())
                                GestOpt(Request.Item("Tributo"))
                                DivGrafico.Style.Add("display", "")
                                DivAttesa.Style.Add("display", "none")

                                If GetCharFatturatoIncassato(myRow.Cells(0).Text, "Fatture", "Note", "Incassato", CDbl(myRow.Cells(5).Text), CDbl(myRow.Cells(6).Text), CDbl(myRow.Cells(9).Text)) Then
                                    GestOpt(Request.Item("Tributo"))
                                    DivGrafico.Style.Add("display", "")
                                    DivAttesa.Style.Add("display", "none")
                                Else
                                    Dim strscript As String = "GestAlert('a', 'danger', '', '', 'Si è verificato un errore nella creazione del grafico!');"
                                    RegisterScript(strscript, Me.GetType())
                                End If
                            End If
                        Next
                End Select
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FattVSIncas.GrdRowCommand.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
        End Try
    End Sub
    'Protected Sub GrdRowCommand(sender As Object, e As GridViewCommandEventArgs)
    '    Try
    '        Dim IDRow As String = e.CommandArgument.ToString()
    '        If e.CommandName = "RowOpen" Then
    '            Select Case CType(sender, Ribes.OPENgov.WebControls.RibesGridView).UniqueID
    '                Case "GrdICIIncassato"
    '                    For Each myRow As GridViewRow In GrdICIIncassato.Rows
    '                        If IDRow = myRow.Cells(0).Text Then
    '                            Dim impAP, impAF, impAR, impTE, impFR, impUP, impAFS, impARS, impTES, impFRS, impUPS As Double
    '                            Dim sScript As String

    '                            impAP = 0 : impAF = 0 : impAR = 0 : impTE = 0 : impFR = 0 : impUP = 0 : impAFS = 0 : impARS = 0 : impTES = 0 : impFRS = 0 : impUPS = 0
    '                            If CDbl(myRow.Cells(2).Text) > 0 Then
    '                                impAP = CDbl(myRow.Cells(2).Text)
    '                            End If
    '                            If CDbl(myRow.Cells(3).Text) Then
    '                                impAF = CDbl(myRow.Cells(3).Text)
    '                            End If
    '                            If CDbl(myRow.Cells(4).Text) Then
    '                                impAFS = CDbl(myRow.Cells(4).Text)
    '                            End If
    '                            If CDbl(myRow.Cells(5).Text) Then
    '                                impAR = CDbl(myRow.Cells(5).Text)
    '                            End If
    '                            If CDbl(myRow.Cells(6).Text) Then
    '                                impARS = CDbl(myRow.Cells(6).Text)
    '                            End If
    '                            If CDbl(myRow.Cells(7).Text) Then
    '                                impTE = CDbl(myRow.Cells(7).Text)
    '                            End If
    '                            If CDbl(myRow.Cells(8).Text) Then
    '                                impTES = CDbl(myRow.Cells(8).Text)
    '                            End If
    '                            If CDbl(myRow.Cells(9).Text) Then
    '                                impFR = CDbl(myRow.Cells(9).Text)
    '                            End If
    '                            If CDbl(myRow.Cells(10).Text) Then
    '                                impFRS = CDbl(myRow.Cells(10).Text)
    '                            End If
    '                            If CDbl(myRow.Cells(11).Text) Then
    '                                impUP = CDbl(myRow.Cells(11).Text)
    '                            End If
    '                            If CDbl(myRow.Cells(12).Text) Then
    '                                impUPS = CDbl(myRow.Cells(12).Text)
    '                            End If

    '                            sScript = ""
    '                            If (impAP + impAF + impAR + impTE + impFR + impUP + impAFS + impARS + impTES + impFRS + impUPS) > 0 Then
    '                                sScript += "google.charts.load('current', { 'packages': ['corechart'] });"
    '                                '// Set a callback to run when the Google Visualization API is loaded."
    '                                sScript += "google.charts.setOnLoadCallback(drawChart);"
    '                                '// Callback that creates and populates a data table,"
    '                                '// instantiates the pie chart, passes in the data and"
    '                                '// draws it."
    '                                sScript += "function drawChart() {"
    '                                '// Create the data table."
    '                                sScript += "var data = new google.visualization.DataTable();"
    '                                sScript += "data.addColumn('string', 'Topping');"
    '                                sScript += "data.addColumn('number', 'Slices');"
    '                                sScript += "data.addRows(["
    '                                If impAP > 0 Then
    '                                    sScript += "['Abi. Princ.', " & impAP.ToString.Replace(",", ".") & "]"
    '                                End If
    '                                If impAF > 0 Then
    '                                    sScript += ",['Altri Fab.', " & impAF.ToString.Replace(",", ".") & "]"
    '                                End If
    '                                If impAFS > 0 Then
    '                                    sScript += ",['Altri Fab. Stato', " & impAFS.ToString.Replace(",", ".") & "]"
    '                                End If
    '                                If impAR > 0 Then
    '                                    sScript += ",['Aree Fab.', " & impAR.ToString.Replace(",", ".") & "]"
    '                                End If
    '                                If impARS > 0 Then
    '                                    sScript += ",['Aree Fab. Stato', " & impARS.ToString.Replace(",", ".") & "]"
    '                                End If
    '                                If impTE > 0 Then
    '                                    sScript += ",['Terreni', " & impTE.ToString.Replace(",", ".") & "]"
    '                                End If
    '                                If impTES > 0 Then
    '                                    sScript += ",['Terreni Stato', " & impTES.ToString.Replace(",", ".") & "]"
    '                                End If
    '                                If impFR > 0 Then
    '                                    sScript += ",['Fab.Rur.', " & impFR.ToString.Replace(",", ".") & "]"
    '                                End If
    '                                If impFRS > 0 Then
    '                                    sScript += ",['Fab.Rur. Stato', " & impFRS.ToString.Replace(",", ".") & "]"
    '                                End If
    '                                If impUP > 0 Then
    '                                    sScript += ",['Uso Prod.Cat.D', " & impUP.ToString.Replace(",", ".") & "]"
    '                                End If
    '                                If impUPS > 0 Then
    '                                    sScript += ",['Uso Prod.Cat.D Stato', " & impUPS.ToString.Replace(",", ".") & "]"
    '                                End If
    '                                sScript += "]);"

    '                                '// Set chart options"
    '                                sScript += "var options = {"
    '                                sScript += "'title': ''"
    '                                sScript += ", width: 600"
    '                                sScript += ", height: 450"
    '                                sScript += ", legend: {position:'labeled', width:200}"
    '                                sScript += ", sliceVisibilityThreshold: 0"
    '                                sScript += ", pieSliceText: 'none'"
    '                                sScript += ", is3D:true"
    '                                sScript += "};"

    '                                '// Instantiate and draw our chart, passing in some options."
    '                                sScript += "var chart = new google.visualization.PieChart(document.getElementById('chart_div'));"
    '                                sScript += "chart.draw(data, options);"
    '                                sScript += "}"
    '                            Else
    '                                sScript += "GestAlert('a', 'warning', '', '', 'I dati selezionati non permettono la generazione di un grafico.');"
    '                            End If
    '                            RegisterScript(sScript, Me.GetType())
    '                            GestOpt(Request.Item("Tributo"))
    '                            DivGrafico.Style.Add("display", "")
    '                            DivAttesa.Style.Add("display", "none")
    '                        End If
    '                    Next
    '                Case "GrdTARSUEmesso"
    '                    For Each myRow As GridViewRow In GrdTARSUEmesso.Rows
    '                        If IDRow = myRow.Cells(0).Text Then
    '                            If GetCharFatturatoIncassato(myRow.Cells(0).Text, "Emesso", "Netto Sgravi", "Incassato", CDbl(myRow.Cells(10).Text), CDbl(CType(myRow.FindControl("hfnettosgravi"), HiddenField).Value), CDbl(CType(myRow.FindControl("hfincassato"), HiddenField).Value)) Then
    '                                GestOpt(Request.Item("Tributo"))
    '                                DivGrafico.Style.Add("display", "")
    '                                DivAttesa.Style.Add("display", "none")
    '                            Else
    '                                Dim strscript As String = "GestAlert('a', 'danger', '', '', 'Si è verificato un errore nella creazione del grafico!');"
    '                                RegisterScript(strscript, Me.GetType())
    '                            End If
    '                        End If
    '                    Next
    '                Case "GrdH2OEmesso"
    '                    For Each myRow As GridViewRow In GrdH2OEmesso.Rows
    '                        If IDRow = myRow.Cells(0).Text Then
    '                            If GetCharFatturatoIncassato(myRow.Cells(0).Text, "Fatture", "Note", "Incassato", CDbl(myRow.Cells(5).Text), CDbl(myRow.Cells(6).Text), CDbl(myRow.Cells(9).Text)) Then
    '                                GestOpt(Request.Item("Tributo"))
    '                                DivGrafico.Style.Add("display", "")
    '                                DivAttesa.Style.Add("display", "none")
    '                            Else
    '                                Dim strscript As String = "GestAlert('a', 'danger', '', '', 'Si è verificato un errore nella creazione del grafico!');"
    '                                RegisterScript(strscript, Me.GetType())
    '                            End If
    '                        End If
    '                    Next
    '            End Select
    '        End If
    '    Catch ex As Exception
    '        Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FattVSIncas.GrdRowCommand.errore: ", ex)
    '        Response.Redirect("../PaginaErrore.aspx")
    '    End Try
    'End Sub
    'Private Sub GrdTARSUEmesso_UpdateCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GrdTARSUEmesso.UpdateCommand
    '    Try
    '        If GetCharFatturatoIncassato(e.Item.Cells(0).Text, "Emesso", "Netto Sgravi", "Incassato", CDbl(e.Item.Cells(10).Text), CDbl(e.Item.Cells(12).Text), CDbl(e.Item.Cells(13).Text)) Then
    '            GestOpt(Request.Item("Tributo"))
    '            DivGrafico.Style.Add("display", "")
    '            DivAttesa.Style.Add("display", "none")
    '        Else
    '            Dim strscript As String = "<script language='javascript' type='text/javascript'>"
    '            strscript += "alert('Si è verificato un errore nella creazione del grafico!');"
    '            strscript += "</script>"
    '            RegisterScript(Me.GetType(), "errchart", strscript)
    '        End If
    '    Catch Err As Exception
    '        Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FattVSIncas.GrdTARSUEmesso_UpdateCommand.errore: ", Err)
    '       Response.Redirect("../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Private Sub GrdH2OEmesso_UpdateCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GrdH2OEmesso.UpdateCommand
    '    Try
    '        If GetCharFatturatoIncassato(e.Item.Cells(0).Text, "Fatture", "Note", "Incassato", CDbl(e.Item.Cells(5).Text), CDbl(e.Item.Cells(6).Text), CDbl(e.Item.Cells(9).Text)) Then
    '            GestOpt(Request.Item("Tributo"))
    '            DivGrafico.Style.Add("display", "")
    '            DivAttesa.Style.Add("display", "none")
    '        Else
    '            Dim strscript As String = "<script language='javascript' type='text/javascript'>"
    '            strscript += "alert('Si è verificato un errore nella creazione del grafico!');"
    '            strscript += "</script>"
    '            RegisterScript(Me.GetType(), "errchart", strscript)
    '        End If
    '    Catch Err As Exception
    '         Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FattVSIncas.GrdH2OEmesso_UpdateCommand.errore: ", Err)
    '        Response.Redirect("../PaginaErrore.aspx")
    '    End Try
    'End Sub

    'Private Sub GrdICIIncassato_UpdateCommand(source As Object, e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles GrdICIIncassato.UpdateCommand
    '    Try
    '        Dim impAP, impAF, impAR, impTE, impFR, impUP, impAFS, impARS, impTES, impFRS, impUPS As Double
    '        Dim sScript As String

    '        impAP = 0 : impAF = 0 : impAR = 0 : impTE = 0 : impFR = 0 : impUP = 0 : impAFS = 0 : impARS = 0 : impTES = 0 : impFRS = 0 : impUPS = 0
    '        If CDbl(e.Item.Cells(2).Text) > 0 Then
    '            impAP = CDbl(e.Item.Cells(2).Text)
    '        End If
    '        If CDbl(e.Item.Cells(3).Text) Then
    '            impAF = CDbl(e.Item.Cells(3).Text)
    '        End If
    '        If CDbl(e.Item.Cells(4).Text) Then
    '            impAFS = CDbl(e.Item.Cells(4).Text)
    '        End If
    '        If CDbl(e.Item.Cells(5).Text) Then
    '            impAR = CDbl(e.Item.Cells(5).Text)
    '        End If
    '        If CDbl(e.Item.Cells(6).Text) Then
    '            impARS = CDbl(e.Item.Cells(6).Text)
    '        End If
    '        If CDbl(e.Item.Cells(7).Text) Then
    '            impTE = CDbl(e.Item.Cells(7).Text)
    '        End If
    '        If CDbl(e.Item.Cells(8).Text) Then
    '            impTES = CDbl(e.Item.Cells(8).Text)
    '        End If
    '        If CDbl(e.Item.Cells(9).Text) Then
    '            impFR = CDbl(e.Item.Cells(9).Text)
    '        End If
    '        If CDbl(e.Item.Cells(10).Text) Then
    '            impFRS = CDbl(e.Item.Cells(10).Text)
    '        End If
    '        If CDbl(e.Item.Cells(11).Text) Then
    '            impUP = CDbl(e.Item.Cells(11).Text)
    '        End If
    '        If CDbl(e.Item.Cells(12).Text) Then
    '            impUPS = CDbl(e.Item.Cells(12).Text)
    '        End If

    '        sScript = "<script language='javascript' type='text/javascript'>"
    '        If (impAP + impAF + impAR + impTE + impFR + impUP + impAFS + impARS + impTES + impFRS + impUPS) > 0 Then
    '            sScript += "google.charts.load('current', { 'packages': ['corechart'] });"
    '            '// Set a callback to run when the Google Visualization API is loaded."
    '            sScript += "google.charts.setOnLoadCallback(drawChart);"
    '            '// Callback that creates and populates a data table,"
    '            '// instantiates the pie chart, passes in the data and"
    '            '// draws it."
    '            sScript += "function drawChart() {"
    '            '// Create the data table."
    '            sScript += "var data = new google.visualization.DataTable();"
    '            sScript += "data.addColumn('string', 'Topping');"
    '            sScript += "data.addColumn('number', 'Slices');"
    '            sScript += "data.addRows(["
    '            If impAP > 0 Then
    '                sScript += "['Abi. Princ.', " & impAP.ToString.Replace(",", ".") & "]"
    '            End If
    '            If impAF > 0 Then
    '                sScript += ",['Altri Fab.', " & impAF.ToString.Replace(",", ".") & "]"
    '            End If
    '            If impAFS > 0 Then
    '                sScript += ",['Altri Fab. Stato', " & impAFS.ToString.Replace(",", ".") & "]"
    '            End If
    '            If impAR > 0 Then
    '                sScript += ",['Aree Fab.', " & impAR.ToString.Replace(",", ".") & "]"
    '            End If
    '            If impARS > 0 Then
    '                sScript += ",['Aree Fab. Stato', " & impARS.ToString.Replace(",", ".") & "]"
    '            End If
    '            If impTE > 0 Then
    '                sScript += ",['Terreni', " & impTE.ToString.Replace(",", ".") & "]"
    '            End If
    '            If impTES > 0 Then
    '                sScript += ",['Terreni Stato', " & impTES.ToString.Replace(",", ".") & "]"
    '            End If
    '            If impFR > 0 Then
    '                sScript += ",['Fab.Rur.', " & impFR.ToString.Replace(",", ".") & "]"
    '            End If
    '            If impFRS > 0 Then
    '                sScript += ",['Fab.Rur. Stato', " & impFRS.ToString.Replace(",", ".") & "]"
    '            End If
    '            If impUP > 0 Then
    '                sScript += ",['Uso Prod.Cat.D', " & impUP.ToString.Replace(",", ".") & "]"
    '            End If
    '            If impUPS > 0 Then
    '                sScript += ",['Uso Prod.Cat.D Stato', " & impUPS.ToString.Replace(",", ".") & "]"
    '            End If
    '            sScript += "]);"

    '            '// Set chart options"
    '            sScript += "var options = {"
    '            sScript += "'title': ''"
    '            sScript += ", width: 600"
    '            sScript += ", height: 450"
    '            sScript += ", legend: {position:'labeled', width:200}"
    '            sScript += ", sliceVisibilityThreshold: 0"
    '            sScript += ", pieSliceText: 'none'"
    '            sScript += ", is3D:true"
    '            'sScript += ", pieSliceTextStyle: {color:'transparent'}"
    '            'sScript += ", slices: {"
    '            'sScript += "0: { color: 'aqua' }"
    '            'sScript += ", 1: { color: 'red' }"
    '            'sScript += ", 2: { color: 'lime' }"
    '            'sScript += " }"
    '            sScript += "};"

    '            '// Instantiate and draw our chart, passing in some options."
    '            sScript += "var chart = new google.visualization.PieChart(document.getElementById('chart_div'));"
    '            sScript += "chart.draw(data, options);"
    '            sScript += "}"
    '        Else
    '            sScript += "alert('I dati selezionati non permettono la generazione di un grafico.');"
    '        End If
    '        sScript += "</script>"
    '        RegisterScript(Me.GetType(), "drawchart" & Now.ToLongDateString, sScript)
    '        'Dim myPieChart As New WebChart.PieChart
    '        'Dim mylistcolor(11) As Color

    '        'Dim ds As DataSet = New DataSet
    '        'Dim dtMyDati As DataTable = ds.Tables.Add("MyTable")
    '        'dtMyDati.Columns.Add(New DataColumn("myTitle"))
    '        'dtMyDati.Columns.Add(New DataColumn("myValue", GetType(System.Double)))


    '        'Dim row As DataRow = dtMyDati.NewRow
    '        'row("myTitle") = "Abi. Princ."
    '        'row("myValue") = e.Item.Cells(2).Text
    '        'dtMyDati.Rows.Add(row)

    '        'row = dtMyDati.NewRow
    '        'row("myTitle") = "Altri Fab."
    '        'row("myValue") = e.Item.Cells(3).Text
    '        'dtMyDati.Rows.Add(row)
    '        'row = dtMyDati.NewRow
    '        'row("myTitle") = "Altri Fab. Stato"
    '        'row("myValue") = e.Item.Cells(4).Text
    '        'dtMyDati.Rows.Add(row)

    '        'row = dtMyDati.NewRow
    '        'row("myTitle") = "Aree Fab."
    '        'row("myValue") = e.Item.Cells(5).Text
    '        'dtMyDati.Rows.Add(row)
    '        'row = dtMyDati.NewRow
    '        'row("myTitle") = "Aree Fab. Stato"
    '        'row("myValue") = e.Item.Cells(6).Text
    '        'dtMyDati.Rows.Add(row)

    '        'row = dtMyDati.NewRow
    '        'row("myTitle") = "Terreni"
    '        'row("myValue") = e.Item.Cells(7).Text
    '        'dtMyDati.Rows.Add(row)
    '        'row = dtMyDati.NewRow
    '        'row("myTitle") = "Terreni Stato"
    '        'row("myValue") = e.Item.Cells(8).Text
    '        'dtMyDati.Rows.Add(row)

    '        'row = dtMyDati.NewRow
    '        'row("myTitle") = "Fab.Rur."
    '        'row("myValue") = e.Item.Cells(9).Text
    '        'dtMyDati.Rows.Add(row)
    '        'row = dtMyDati.NewRow
    '        'row("myTitle") = "Fab.Rur. Stato"
    '        'row("myValue") = e.Item.Cells(10).Text
    '        'dtMyDati.Rows.Add(row)

    '        'row = dtMyDati.NewRow
    '        'row("myTitle") = "Uso Prod.Cat.D"
    '        'row("myValue") = e.Item.Cells(11).Text
    '        'dtMyDati.Rows.Add(row)
    '        'row = dtMyDati.NewRow
    '        'row("myTitle") = "Uso Prod.Cat.D Stato"
    '        'row("myValue") = e.Item.Cells(12).Text
    '        'dtMyDati.Rows.Add(row)

    '        'Dim x As Integer = 0
    '        'mylistcolor(x) = Color.Teal : x += 1
    '        'mylistcolor(x) = Color.SteelBlue : x += 1
    '        'mylistcolor(x) = Color.DodgerBlue : x += 1
    '        'mylistcolor(x) = Color.LightSeaGreen : x += 1
    '        'mylistcolor(x) = Color.MediumAquamarine : x += 1
    '        'mylistcolor(x) = Color.RoyalBlue : x += 1
    '        'mylistcolor(x) = Color.Blue : x += 1
    '        'mylistcolor(x) = Color.DeepSkyBlue : x += 1
    '        'mylistcolor(x) = Color.SkyBlue : x += 1
    '        'mylistcolor(x) = Color.Navy : x += 1
    '        'mylistcolor(x) = Color.DarkSlateBlue : x += 1

    '        'myPieChart.Colors = mylistcolor
    '        'myPieChart.Line.Color = Color.Transparent
    '        'myPieChart.Explosion = 15
    '        'myPieChart.DataSource = ds.Tables(0).DefaultView
    '        'myPieChart.Shadow.Visible = True
    '        'myPieChart.DataLabels.Visible = True
    '        'myPieChart.DataXValueField = "myTitle"
    '        'myPieChart.DataYValueField = "myValue"

    '        'myPieChart.DataBind()

    '        'ColChartAnalisi.Charts.Add(myPieChart)
    '        'ColChartAnalisi.Width = 700
    '        'ColChartAnalisi.Height = 500
    '        'ColChartAnalisi.Background.Color = Color.Transparent
    '        'ColChartAnalisi.GridLines = WebChart.GridLines.None
    '        'ColChartAnalisi.Legend.Position = WebChart.LegendPosition.Bottom
    '        'ColChartAnalisi.Legend.Width = 50
    '        'ColChartAnalisi.ChartTitle.Text = e.Item.Cells(0).Text
    '        'ColChartAnalisi.ChartTitle.ForeColor = Color.Black
    '        'ColChartAnalisi.ChartTitle.Font = New Font("Verdana", 8, FontStyle.Bold)
    '        'ColChartAnalisi.BorderStyle = BorderStyle.None
    '        'ColChartAnalisi.HasChartLegend = True

    '        'ColChartAnalisi.RedrawChart()

    '        GestOpt(Request.Item("Tributo"))
    '        DivGrafico.Style.Add("display", "")
    '        DivAttesa.Style.Add("display", "none")
    '    Catch Err As Exception
    '        Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FattVSIncas.GrdICIIncassato_UpdateCommand.errore: ", Err)
    '        Response.Redirect("../PaginaErrore.aspx")
    '    End Try
    'End Sub
#End Region
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
                Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FattVSIncas.LoadCombos.errore: ", ex)
                Response.Redirect("../PaginaErrore.aspx")
            Finally
                myDataReader.Close()
            End Try

            Dim oLoadCombo As New OPENgovTIA.generalClass.generalFunction
            Dim sSQL As String = "SELECT DISTINCT DESCRIZIONE, IDTIPORUOLO, ORDINAMENTO"
            sSQL += " FROM TBLTIPORUOLO"
            sSQL += " ORDER BY ORDINAMENTO"
            oLoadCombo.LoadComboGenerale(ddlParam3, sSQL, COSTANTValue.ConstSession.StringConnectionTARSU, True, OPENgovTIA.Costanti.TipoDefaultCmb.STRINGA)
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FattVSIncas.LoadCombos.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
            Throw New Exception("Problemi nell'esecuzione di FattVSIncas::LoadCombo::", ex)
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
            'If GrdTARSUEmesso.Rows.Count <= 0 Then LblTARSUEmesso.Visible = False
            'If GrdTARSUNetto.Rows.Count <= 0 Then LblTARSUNetto.Visible = False
            'If GrdTARSUIncassato.Rows.Count <= 0 Then LblTARSUIncassato.Visible = False
            'If GrdH2OEmesso.Rows.Count <= 0 Then LblH2OEmesso.Visible = False
            'If GrdH2OIncassato.Rows.Count <= 0 Then LblH2OIncassato.Visible = False

            Dim sScript As String = ""
            Select Case IdTributo
                Case Utility.Costanti.TRIBUTO_TARSU
                    sScript += "document.getElementById('txtParam3').style.display='none';"
                    sScript += "document.getElementById('optICI').style.display='none';"
                    sScript += "document.getElementById('optICIRata').style.display='none';"
                    sScript += "document.getElementById('divTARSU').style.display='';"
                    sScript += "document.getElementById('divICI').style.display='none';"
                    sScript += "document.getElementById('divH2O').style.display='none';"
                    sScript += "document.getElementById('Grafico').style.display='';"
                    lblParam3.Text = "Ruolo"
                Case Utility.Costanti.TRIBUTO_ICI, Utility.Costanti.TRIBUTO_TASI
                    sScript += "document.getElementById('ddlParam3').style.display='none';"
                    sScript += "document.getElementById('divTARSU').style.display='none';"
                    sScript += "document.getElementById('divICI').style.display='';"
                    sScript += "document.getElementById('divH2O').style.display='none';"
                    sScript += "document.getElementById('Grafico').style.display='none';"
                    lblParam3.Text = "Anno Confronto"
                Case Utility.Costanti.TRIBUTO_H2O
                    sScript += "document.getElementById('ddlParam3').style.display='none';"
                    sScript += "document.getElementById('optICI').style.display='none';"
                    sScript += "document.getElementById('optICIRata').style.display='none';"
                    sScript += "document.getElementById('divTARSU').style.display='none';"
                    sScript += "document.getElementById('divICI').style.display='none';"
                    sScript += "document.getElementById('divH2O').style.display='';"
                    sScript += "document.getElementById('Grafico').style.display='';"
                    lblParam3.Text = "Data Emissione"
                Case Else
                    Throw New Exception("Tributo mancante")
            End Select
            RegisterScript(sScript, Me.GetType())
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FattVSIncas.GestOpt.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
            Throw New Exception("Problemi nell'esecuzione di FattVSIncas::GestOpt" + ex.Message)
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sTitleChart"></param>
    ''' <param name="sDescrCol1"></param>
    ''' <param name="sDescrCol2"></param>
    ''' <param name="sDescrCol3"></param>
    ''' <param name="impCol1"></param>
    ''' <param name="impCol2"></param>
    ''' <param name="impCol3"></param>
    ''' <returns></returns>
    Private Function GetCharFatturatoIncassato(sTitleChart As String, sDescrCol1 As String, sDescrCol2 As String, sDescrCol3 As String, impCol1 As Double, impCol2 As Double, impCol3 As Double) As Boolean
        Try
            Dim nCharInterval As Double

            If impCol1 > impCol2 Then
                nCharInterval = impCol1 / 10
                ColChartAnalisi.YCustomEnd = impCol1
            Else
                nCharInterval = impCol2 / 10
                ColChartAnalisi.YCustomEnd = impCol2
            End If

            Dim myColChart As New WebChart.ColumnChart
            myColChart.MaxColumnWidth = 20
            myColChart.Fill.Color = Color.SteelBlue
            myColChart.Line.Width = 0
            myColChart.Shadow.Visible = True
            myColChart.Data.Add(New WebChart.ChartPoint(sDescrCol1, impCol1))
            myColChart.Data.Add(New WebChart.ChartPoint(sDescrCol2, 0))
            myColChart.Data.Add(New WebChart.ChartPoint(sDescrCol3, 0))
            ColChartAnalisi.Charts.Add(myColChart)

            myColChart = New WebChart.ColumnChart
            myColChart.MaxColumnWidth = 20
            myColChart.Fill.Color = Color.SpringGreen
            myColChart.Line.Width = 0
            myColChart.Shadow.Visible = True
            myColChart.Data.Add(New WebChart.ChartPoint("", 0))
            myColChart.Data.Add(New WebChart.ChartPoint("", impCol2))
            myColChart.Data.Add(New WebChart.ChartPoint("", 0))
            ColChartAnalisi.Charts.Add(myColChart)

            myColChart = New WebChart.ColumnChart
            myColChart.MaxColumnWidth = 20
            myColChart.Fill.Color = Color.Teal
            myColChart.Line.Width = 0
            myColChart.Shadow.Visible = True
            myColChart.Data.Add(New WebChart.ChartPoint("", 0))
            myColChart.Data.Add(New WebChart.ChartPoint("", 0))
            myColChart.Data.Add(New WebChart.ChartPoint("", impCol3))
            ColChartAnalisi.Charts.Add(myColChart)

            ColChartAnalisi.YAxisFont.ForeColor = Color.Black
            ColChartAnalisi.XAxisFont.ForeColor = Color.Black

            ColChartAnalisi.ChartTitle.Text = sTitleChart
            ColChartAnalisi.ChartTitle.Font = New Font("Verdana", 8, FontStyle.Bold)
            ColChartAnalisi.BorderStyle = BorderStyle.None
            ColChartAnalisi.YValuesInterval = nCharInterval
            ColChartAnalisi.ShowXValues = True
            ColChartAnalisi.HasChartLegend = False
            ColChartAnalisi.Width = 600

            ColChartAnalisi.RedrawChart()

            Return True
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FattVSIncas.GetCharFatturatoIncassato.errore: ", Err)
            Response.Redirect("../PaginaErrore.aspx")
            Return False
        End Try
    End Function
    ''' <summary>
    ''' Funzione che preleva i dati della griglia per generare il grafico
    ''' </summary>
    ''' <param name="myRow"></param>
    ''' <returns></returns>
    Private Function GetSlicesICI(myRow As GridViewRow) As Generic.List(Of Generic.List(Of String))
        Dim ListSlices As New Generic.List(Of Generic.List(Of String))
        Dim mySlice As New Generic.List(Of String)
        Dim myDouble As Double = 0
        Dim nCol As Integer = 2
        Try
            Double.TryParse(myRow.Cells(nCol).Text, myDouble)
            nCol += 1
            If myDouble > 0 Then
                mySlice = New Generic.List(Of String)
                mySlice.Add("Abi. Princ.")
                mySlice.Add(myDouble)
                ListSlices.Add(mySlice)
            End If
            Double.TryParse(myRow.Cells(nCol).Text, myDouble)
            nCol += 1
            If myDouble > 0 Then
                mySlice = New Generic.List(Of String)
                mySlice.Add("Altri Fab.")
                mySlice.Add(myDouble)
                ListSlices.Add(mySlice)
            End If
            Double.TryParse(myRow.Cells(nCol).Text, myDouble)
            nCol += 1
            If myDouble > 0 Then
                mySlice = New Generic.List(Of String)
                mySlice.Add("Altri Fab. Stato")
                mySlice.Add(myDouble)
                ListSlices.Add(mySlice)
            End If
            Double.TryParse(myRow.Cells(nCol).Text, myDouble)
            nCol += 1
            If myDouble > 0 Then
                mySlice = New Generic.List(Of String)
                mySlice.Add("Aree Fab.")
                mySlice.Add(myDouble)
                ListSlices.Add(mySlice)
            End If
            Double.TryParse(myRow.Cells(nCol).Text, myDouble)
            nCol += 1
            If myDouble > 0 Then
                mySlice = New Generic.List(Of String)
                mySlice.Add("Aree Fab. Stato")
                mySlice.Add(myDouble)
                ListSlices.Add(mySlice)
            End If
            Double.TryParse(myRow.Cells(nCol).Text, myDouble)
            nCol += 1
            If myDouble > 0 Then
                mySlice = New Generic.List(Of String)
                mySlice.Add("Terreni")
                mySlice.Add(myDouble)
                ListSlices.Add(mySlice)
            End If
            Double.TryParse(myRow.Cells(nCol).Text, myDouble)
            nCol += 1
            If myDouble > 0 Then
                mySlice = New Generic.List(Of String)
                mySlice.Add("Terreni Stato")
                mySlice.Add(myDouble)
                ListSlices.Add(mySlice)
            End If
            Double.TryParse(myRow.Cells(nCol).Text, myDouble)
            nCol += 1
            If myDouble > 0 Then
                mySlice = New Generic.List(Of String)
                mySlice.Add("Fab.Rur.")
                mySlice.Add(myDouble)
                ListSlices.Add(mySlice)
            End If
            Double.TryParse(myRow.Cells(nCol).Text, myDouble)
            nCol += 1
            If myDouble > 0 Then
                mySlice = New Generic.List(Of String)
                mySlice.Add("Fab.Rur. Stato")
                mySlice.Add(myDouble)
                ListSlices.Add(mySlice)
            End If
            Double.TryParse(myRow.Cells(nCol).Text, myDouble)
            nCol += 1
            If myDouble > 0 Then
                mySlice = New Generic.List(Of String)
                mySlice.Add("Uso Prod.Cat.D")
                mySlice.Add(myDouble)
                ListSlices.Add(mySlice)
            End If
            Double.TryParse(myRow.Cells(nCol).Text, myDouble)
            nCol += 1
            If myDouble > 0 Then
                mySlice = New Generic.List(Of String)
                mySlice.Add("Uso Prod.Cat.D Stato")
                mySlice.Add(myDouble)
                ListSlices.Add(mySlice)
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FattVSIncas.GetSlicesICI.errore: ", ex)
            ListSlices = New Generic.List(Of Generic.List(Of String))
        End Try
        Return ListSlices
    End Function
    ''' <summary>
    ''' Funzione che preleva i dati della griglia per generare il grafico
    ''' </summary>
    ''' <param name="myRow"></param>
    ''' <param name="ChartInsoluto"></param>
    ''' <returns></returns>
    Private Function GetSlicesTARSU(myRow As GridViewRow, ChartInsoluto As Boolean) As Generic.List(Of Generic.List(Of String))
        Dim ListSlices As New Generic.List(Of Generic.List(Of String))
        Dim mySlice As New Generic.List(Of String)
        Try
            If ChartInsoluto Then
                If CDbl(myRow.Cells(8).Text) > 0 Then
                    mySlice = New Generic.List(Of String)
                    mySlice.Add(myRow.Cells(0).Text)
                    mySlice.Add(CDbl(myRow.Cells(8).Text))
                    ListSlices.Add(mySlice)
                End If
            Else
                If CDbl(CType(myRow.FindControl("hfsgravi"), HiddenField).Value) > 0 Then
                    mySlice = New Generic.List(Of String)
                    mySlice.Add("Sgravi")
                    mySlice.Add(CDbl(CType(myRow.FindControl("hfsgravi"), HiddenField).Value))
                    ListSlices.Add(mySlice)
                End If
                If CDbl(CType(myRow.FindControl("hfincassato"), HiddenField).Value) > 0 Then
                    mySlice = New Generic.List(Of String)
                    mySlice.Add("Incassato")
                    mySlice.Add(CDbl(CType(myRow.FindControl("hfincassato"), HiddenField).Value))
                    ListSlices.Add(mySlice)
                End If
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FattVSIncas.GetSlicesTARSU.errore: ", ex)
            ListSlices = New Generic.List(Of Generic.List(Of String))
        End Try
        Return ListSlices
    End Function
    ''' <summary>
    ''' Funzione che preleva i dati della griglia per generare il grafico
    ''' </summary>
    ''' <param name="myRow"></param>
    ''' <param name="ChartInsoluto"></param>
    ''' <returns></returns>
    Private Function GetSlicesH2O(myRow As GridViewRow, ChartInsoluto As Boolean) As Generic.List(Of Generic.List(Of String))
        Dim ListSlices As New Generic.List(Of Generic.List(Of String))
        Dim mySlice As New Generic.List(Of String)
        Try
            If ChartInsoluto Then
                If CDbl(myRow.Cells(5).Text) > 0 Then
                    mySlice = New Generic.List(Of String)
                    mySlice.Add(myRow.Cells(0).Text)
                    mySlice.Add(CDbl(myRow.Cells(5).Text))
                    ListSlices.Add(mySlice)
                End If
            Else
                If CDbl(myRow.Cells(6).Text) > 0 Then
                    mySlice = New Generic.List(Of String)
                    mySlice.Add("Note")
                    mySlice.Add(CDbl(myRow.Cells(6).Text))
                    ListSlices.Add(mySlice)
                End If
                If CDbl(myRow.Cells(9).Text) > 0 Then
                    mySlice = New Generic.List(Of String)
                    mySlice.Add("Incassato")
                    mySlice.Add(CDbl(myRow.Cells(9).Text))
                    ListSlices.Add(mySlice)
                End If
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FattVSIncas.GetSlicesH2O.errore: ", ex)
            ListSlices = New Generic.List(Of Generic.List(Of String))
        End Try
        Return ListSlices
    End Function
    ''' <summary>
    ''' funzione che costruisce lo script da eseguire per la generazione del grafico
    ''' </summary>
    ''' <param name="ListValues"></param>
    ''' <param name="TitleChart"></param>
    ''' <returns></returns>
    Private Function DrawPieChart(ListValues As Generic.List(Of Generic.List(Of String)), TitleChart As String) As String
        Dim sScript As String = ""
        Dim sSlice As String = ""
        Try
            sScript += "google.charts.load('current', { 'packages': ['corechart'] });"
            '// Set a callback to run when the Google Visualization API is loaded."
            sScript += "google.charts.setOnLoadCallback(drawChart);"
            '// Callback that creates and populates a data table,"
            '// instantiates the pie chart, passes in the data and"
            '// draws it."
            sScript += "function drawChart() {"
            '// Create the data table."
            sScript += "var data = new google.visualization.DataTable();"
            sScript += "data.addColumn('string', 'Topping');"
            sScript += "data.addColumn('number', 'Slices');"
            sScript += "data.addRows(["
            For Each myList As Generic.List(Of String) In ListValues
                If Utility.StringOperation.FormatDouble(myList(1)) > 0 Then
                    If sSlice <> "" Then
                        sSlice += ","
                    End If
                    sSlice += "['" + myList(0) + "', " + Utility.StringOperation.FormatDouble(myList(1)).ToString.Replace(",", ".") + "]"
                End If
            Next
            sScript += sSlice + "]);"

            '// Set chart options"
            sScript += "var options = {"
            sScript += "'title': '" + TitleChart + "'"
            sScript += ", width: 600"
            sScript += ", height: 450"
            sScript += ", legend: {position:'labeled', width:200}"
            sScript += ", sliceVisibilityThreshold: 0"
            sScript += ", pieSliceText: 'none'"
            sScript += ", is3D:true"
            sScript += "};"

            '// Instantiate and draw our chart, passing in some options."
            sScript += "var chart = new google.visualization.PieChart(document.getElementById('chart_div'));"
            sScript += "chart.draw(data, options);"
            sScript += "}"
        Catch ex As Exception
            sScript += "GestAlert('a', 'warning', '', '', 'Errore nella generazione del grafico.');"
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FattVSIncas.DrawPieChart.errore: ", ex)
        End Try
        Return sScript
    End Function
    Private Function DrawColumnChart(ListValues As Generic.List(Of Generic.List(Of String)), TitleChart As String) As String
        Dim sScript As String = ""
        Dim sSlice As String = ""
        Try
            sScript += "google.charts.load('current', { 'packages': ['bar'] });"
            '// Set a callback to run when the Google Visualization API is loaded."
            sScript += "google.charts.setOnLoadCallback(drawChart);"
            '// Callback that creates and populates a data table,"
            '// instantiates the pie chart, passes in the data and"
            '// draws it."
            sScript += "function drawChart() {"
            '// Create the data table."
            sScript += "var data = new google.visualization.arrayToDataTable(["
            sSlice += "['Ente', '%']"
            For Each myList As Generic.List(Of String) In ListValues
                If Utility.StringOperation.FormatDouble(myList(1)) > 0 Then
                    If sSlice <> "" Then
                        sSlice += ","
                    End If
                    sSlice += "['" + myList(0) + "', " + Utility.StringOperation.FormatDouble(myList(1)).ToString.Replace(",", ".") + "]"
                End If
            Next
            sScript += sSlice + "]);"

            '// Set chart options"
            sScript += "var options = {"
            sScript += "chart: {title: '" + TitleChart + "',subtitle: '' }"
            sScript += ", axes: {x: {  0: { side: 'top', label: ''}}}"
            sScript += ", width: 600"
            sScript += ", height: 450"
            sScript += ", legend: { position: 'none' }"
            sScript += ", bar: { groupWidth: '90%' }"
            sScript += "};"

            '// Instantiate and draw our chart, passing in some options."
            sScript += "var chart = new google.charts.Bar(document.getElementById('chart_div'));"
            sScript += "chart.draw(data, google.charts.Bar.convertOptions(options));"
            sScript += "}"
        Catch ex As Exception
            sScript += "GestAlert('a', 'warning', '', '', 'Errore nella generazione del grafico.');"
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.FattVSIncas.DrawColumnChart.errore: ", ex)
        End Try
        Return sScript
    End Function
End Class

