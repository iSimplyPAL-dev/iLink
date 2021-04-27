Imports System.Web
Imports System.Web.SessionState
Imports System.Web.Mail
Imports log4net, log4net.Config

Public Class [Global]
    Inherits System.Web.HttpApplication

#Region " Component Designer Generated Code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Component Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Component Designer
    'It can be modified using the Component Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        components = New System.ComponentModel.Container()
    End Sub

#End Region

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application is started
        Dim pathfileinfo As String
        pathfileinfo = ConfigurationManager.AppSettings("pathfileconflog4net")
        Dim fileconfiglog4net As System.IO.FileInfo = New System.IO.FileInfo(pathfileinfo)
        XmlConfigurator.ConfigureAndWatch(fileconfiglog4net)
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        '' Fires when the session is started
        Session("DESC_TIPO_PROC_SERV") = ""
        'Session("FILE_INI") = "C:\Inetpub\wwwroot\OpenUtenze\GenDocFTP\GenDocOpenUtenze.ini"
        'Session("GENERAL_PATH") = "C:\Inetpub\wwwroot\OpenUtenze\GenDocFTP\"
        'Session("WEB_PATH") = "/OpenUtenze/GenDocFTP/"
        'Session("COD_TRIBUTO") = "9000"
        'Session("PARAMETROENV") = "_OU"
        'ConstSession.pathapplicazione = "/OPENutenze"
        'Session("IDENTIFICATIVOAPPLICAZIONE") = "OU"
        'Session("CODCOMUNEFD") = "127"
        'Session("CODICECITTA") = "1"
        'Session("CODICEIMPIANTO") = ""
        '/* S.T. DEBUG */
        'Session("Ambiente") = "CMGC"
        'Session("username") = "RIADM"
        'Session("codente") = "7030" '"7015" '"7069" '"7025" '"7039" '"7062" '"7002" '"7026" '
        'Session("COD_ENTE") = Session("codente")
        'Session("CODICEISTAT") = Session("codente")
        'Session("descrizione_ente") = "gignod" '"chambave" '"valpelline" '"emarese" '"lamagdeleine" '"saint oyen" '"antey saint andrè" '"etroubles" '
        'Session("COD_BELFIORE") = "E029" '"C595" '"L643" '"D402" '"A308" '"" ' "D444" '
        'Session("VisualGIS") = True
        'Session("PERIODO") = "2014/01" '"2013/01" '"2014/01" '"2008/01" '
        'Session("PERIODOID") = "119" '"106" '"124" '"107" '"34" '"109" '"114" '
        Dim FncGen As New ClsGenerale.Generale
        FncGen.GetPeriodoAttuale
    End Sub

  Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
	' Fires at the beginning of each request
  End Sub

  Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
	' Fires upon attempting to authenticate the use
  End Sub


  Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
	' Fires when the session ends
  End Sub

  Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
	' Fires when the application ends
  End Sub

  Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)



  End Sub

End Class
