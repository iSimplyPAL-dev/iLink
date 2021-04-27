Imports System.Web
Imports System.Web.SessionState
Imports System.Runtime.Remoting
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters
Imports System.Runtime.Remoting.Channels
Imports System.Runtime.Remoting.Channels.Http
Imports ComPlusInterface
Imports log4net, log4net.Config

Public Class [Global]
  Inherits System.Web.HttpApplication
  Private chan As HttpChannel

    Private Shared Log As ILog = LogManager.GetLogger(GetType([Global]))

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
        Application("nome_sito") = ConfigurationManager.AppSettings("NOME_SITO")
        Try
            Dim clientProvider As SoapClientFormatterSinkProvider = New SoapClientFormatterSinkProvider
            Dim serverProvider As SoapServerFormatterSinkProvider = New SoapServerFormatterSinkProvider
            serverProvider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full

            Dim props As IDictionary = New Hashtable
            props("port") = ConfigurationManager.AppSettings("PORTACLIENT")
            Dim s As String = System.Guid.NewGuid().ToString()
            props("name") = s
            props("typeFilterLevel") = TypeFilterLevel.Full

            chan = New HttpChannel(props, clientProvider, serverProvider)

            ChannelServices.RegisterChannel(chan)

            Dim pathfileinfo As String
            pathfileinfo = ConfigurationManager.AppSettings("pathfileconflog4net")
            Dim fileconfiglog4net As System.IO.FileInfo = New System.IO.FileInfo(pathfileinfo)
            XmlConfigurator.ConfigureAndWatch(fileconfiglog4net)
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Global.Application_Start.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
        End Try
    End Sub


  Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        Try
            ' Fires when the session is started
            Session.Clear()
            Session("IDENTIFICATIVOAPPLICAZIONE") = "OPENGOVP"
            Session("COD_TRIBUTO") = ConfigurationManager.AppSettings("Tributo")
            Session("PATH_APPLICAZIONE") = ConfigurationManager.AppSettings("PATH_APPLICAZIONE")
            Session("PATH_IMAGES") = ConfigurationManager.AppSettings("PATH_IMAGES")
            Session("PATH_FOLDER_IMPORT") = "Files/"
            Session("ID_APPLICAZIONE") = ""
            Session("objSessione") = ""
            Session("PATH_IMAGES") = ConfigurationManager.AppSettings("PATH_IMAGES")
            Session("PATH_PHOTO") = ConfigurationManager.AppSettings("PATH_PHOTO")
            Session("CODENTE") = ConstSession.IdEnte

            Session.Timeout = ConfigurationManager.AppSettings("SessionTimeout")
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovPROVVEDIMENTI.Global.Session_Start.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
        End Try
    End Sub

  Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
    ' Fires at the beginning of each request
  End Sub

  Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
    ' Fires upon attempting to authenticate the use
  End Sub

  Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
    ChannelServices.UnregisterChannel(chan)
  End Sub

  Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
    ' Fires when the session ends
  End Sub

  Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
    ChannelServices.UnregisterChannel(chan)
  End Sub

End Class
