Imports log4net

Partial Class FrameCessazione
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(FrameCessazione))
    Private iDB As New DBAccess.getDBobject
    Private FncDate As New ClsGenerale.Generale

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
        Dim dvMyDati As New DataView

        'prelevo l'ultima data lettura presente
        Try
            txtDataCess.Text = Utility.StringOperation.FormatString(Request.Params("data"))
            dvMyDati = New clsLetture().GetTopLetture(ConstSession.DBType, ConstSession.StringConnection, 1, Utility.StringOperation.FormatInt(Request.Params("IdContatore")), New ClsGenerale.Generale().FormattaData("A", "", DateTime.MaxValue.ToShortDateString, False), "<")
            For Each myRow As DataRowView In dvMyDati
                TxtDataLastLettura.Text = FncDate.GiraDataFromDB(myRow("datalettura"))
            Next
            dvMyDati.Dispose()
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte + "." + ConstSession.UserName + " - OPENgovH2O.FrameCessazione.Page_Load.errore: ", Err)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
