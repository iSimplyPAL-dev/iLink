Imports log4net

Partial Class ChartAnalisiEconomiche
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ChartAnalisiEconomiche))

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

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            Dim ds As DataSet = GetDataSet(Request.Item("ImpIncassato"), Request.Item("ImpInsoluto"))
            Dim view As DataView = ds.Tables(0).DefaultView
            Dim chart As New WebChart.PieChart
            Dim mylistcolor(2) As Color

            mylistcolor(0) = Color.SeaGreen
            mylistcolor(1) = Color.Crimson

            chart.Colors = mylistcolor
            chart.Line.Color = Color.Transparent
            chart.Explosion = 5
            chart.DataSource = view
            chart.DataXValueField = "MyDescription"
            chart.DataYValueField = "MyValue"

            chart.DataBind()

            PieChartAnalisi.Charts.Add(chart)
            PieChartAnalisi.Background.Color = Color.FromArgb(255, 203, 165)
            PieChartAnalisi.GridLines = WebChart.GridLines.None
            PieChartAnalisi.Legend.Position = WebChart.LegendPosition.Bottom
            PieChartAnalisi.Legend.Width = 40
            PieChartAnalisi.ChartTitle.Text = ""
            PieChartAnalisi.ChartTitle.ForeColor = Color.DarkBlue
            PieChartAnalisi.BorderStyle = BorderStyle.None
            PieChartAnalisi.HasChartLegend = True

            PieChartAnalisi.RedrawChart()
        Catch Err As Exception

            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.CConfiguraCanoni.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    Private Function GetDataSet(ByVal impIncassato As Double, ByVal impInsoluto As Double) As DataSet
        Dim ds As New DataSet
        Dim table As DataTable = ds.Tables.Add("Data")
        table.Columns.Add("MyDescription")
        table.Columns.Add("MyValue", GetType(Double))

        Dim row As DataRow = table.NewRow()
        row("MyDescription") = "Totale Incassato"
        row("MyValue") = impIncassato
        table.Rows.Add(row)
        row = table.NewRow
        row("MyDescription") = "Totale Insoluto"
        row("MyValue") = impInsoluto
        table.Rows.Add(row)

        Return ds
    End Function
End Class
