Imports log4net
Imports log4net.Config
Imports System.IO



Partial Class _Default
    Inherits BasePage
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(_Default))

    Dim pathfileinfo As String = ConfigurationManager.AppSettings("pathfileconflog4net").ToString()
    Dim fileconfiglog4net As FileInfo = New FileInfo(pathfileinfo)

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Config.XmlConfigurator.ConfigureAndWatch(fileconfiglog4net)

        'Session("codente") = ""
        'Session("CODAMBIENTE") = ""
        'Session("COMUNEENTE") = ""
        'Session("PERIODO") = ""
        'Session("CODCOMUNEENTE") = ""
        'Session("PERIODOID") = ""
        'Session("CODICEISTAT") = ""

        Session("FILE_INI") = ConfigurationManager.AppSettings("FILE_INI")
        Session("GENERAL_PATH") = ConfigurationManager.AppSettings("GENERAL_PATH")
        Session("WEB_PATH") = ConfigurationManager.AppSettings("WEB_PATH")
        Session("COD_TRIBUTO") = ConfigurationManager.AppSettings("COD_TRIBUTO")
        Session("PARAMETROENV") = ConfigurationManager.AppSettings("PARAMETROENV")
        'Session("PATH_APPLICAZIONE") = ConfigurationManager.AppSettings("PATH_APPLICAZIONE")
        Log.Debug("default:: letto path_applicazione::" & ConstSession.pathapplicazione)
        Session("IDENTIFICATIVOAPPLICAZIONE") = ConfigurationManager.AppSettings("IDENTIFICATIVOAPPLICAZIONE")
        Session("ANAGRAFICA") = ConfigurationManager.AppSettings("PARAMETRO_ANAGRAFICA")
        Session("StileStradario") = ConfigurationManager.AppSettings("StileStradario")
    End Sub

End Class
