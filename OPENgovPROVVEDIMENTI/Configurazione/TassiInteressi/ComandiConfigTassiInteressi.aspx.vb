Imports log4net
''' <summary>
''' Pagina dei comandi per la gestione degli interessi.
''' Le possibili opzioni sono:
''' - Salva
''' - Elimina
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class ComandiConfigTassiInteressi
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(ComandiConfigTassiInteressi))

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
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CmdRibalta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdRibalta.Click
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(ConstSession.StringConnection)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure

            'costruisco la query
            cmdMyCommand.CommandText = "prc_RibaltaTassiInteresse"
            'valorizzo i parameters:
            cmdMyCommand.Parameters.Clear()
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTEORG", SqlDbType.NVarChar)).Value = hfEnteFrom.Value
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@IDENTENEW", SqlDbType.NVarChar)).Value = ConstSession.IdEnte
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@TRIBUTO", SqlDbType.NVarChar)).Value = hfTributo.Value
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            cmdMyCommand.ExecuteNonQuery()
        Catch Err As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.ComandiConfigTassiInteressi.CmdRibalta_Click.errore: ", Err)
            Response.Redirect("../../../PaginaErrore.aspx")
        Finally
            cmdMyCommand.Connection.Close()
            cmdMyCommand.Dispose()
        End Try
    End Sub
End Class
