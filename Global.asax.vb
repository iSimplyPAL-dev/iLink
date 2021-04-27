Imports System.Web
Imports System.Web.SessionState
Imports System.Runtime.Remoting
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters
Imports System.Runtime.Remoting.Channels
Imports System.Runtime.Remoting.Channels.Http
Imports System.IO
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
        components = New System.ComponentModel.Container
    End Sub

#End Region

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        Try
            ' Fires when the application is started
            Application("nome_sito") = "OpenGov"
            Application("nome_sito") = ConfigurationManager.AppSettings("NOME_SITO")

            'Dim clientProvider As SoapClientFormatterSinkProvider = New SoapClientFormatterSinkProvider
            'Dim serverProvider As SoapServerFormatterSinkProvider = New SoapServerFormatterSinkProvider
            'serverProvider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full

            'Dim props As IDictionary = New Hashtable
            'props("port") = ConfigurationManager.AppSettings("PORTACLIENT")
            'Dim s As String = System.Guid.NewGuid().ToString()
            'props("name") = s
            'props("typeFilterLevel") = TypeFilterLevel.Full


            'chan = New HttpChannel(props, clientProvider, serverProvider)

            'ChannelServices.RegisterChannel(chan)

            Dim pathfileinfo As String
            pathfileinfo = ConfigurationManager.AppSettings("pathfileconflog4net")
            Dim fileconfiglog4net As FileInfo = New FileInfo(pathfileinfo)
            XmlConfigurator.ConfigureAndWatch(fileconfiglog4net)
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.Global.Application_Start.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
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
    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)

        ' Fires when the session is started
        Try
            Session.Clear()
            Session("COD_TRIBUTO") = ConfigurationManager.AppSettings("Tributo")
            Session("CODENTE") = "1"
            Session("PATH_APPLICAZIONE") = ConfigurationManager.AppSettings("PATH_APPLICAZIONE")
            Session("PATH_IMAGES") = ConfigurationManager.AppSettings("PATH_IMAGES")
            Session("PATH_FOLDER_IMPORT") = "Files/"
            Session("ID_APPLICAZIONE") = ""
            Session("objSessione") = ""
            Session("COD_ENTE") = ""
            Session("DESCRIZIONE_ENTE") = ""
            Session("PATH_IMAGES") = ConfigurationManager.AppSettings("PATH_IMAGES")
            Session("PATH_PHOTO") = ConfigurationManager.AppSettings("PATH_PHOTO")

            'Session.Timeout = ConfigurationManager.AppSettings("SessionTimeout")
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.Global.Session_Start.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
        End Try
    End Sub
    'Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)

    '    ' Fires when the session is started
    '    Try
    '        Session.Clear()
    '        Session("COD_TRIBUTO") = ConfigurationManager.AppSettings("Tributo")
    '        Session("CODENTE") = "1"
    '        Session("PATH_APPLICAZIONE") = ConfigurationManager.AppSettings("PATH_APPLICAZIONE")
    '        Session("PATH_IMAGES") = ConfigurationManager.AppSettings("PATH_IMAGES")
    '        Session("PARAMETROENV") = ConfigurationManager.AppSettings("PARAMETROENV")
    '        Session("IDENTIFICATIVOAPPLICAZIONE") = "OPENGOVG"
    '        Session("Anagrafica") = ConfigurationManager.AppSettings("OPENGOVA")
    '        Session("PATH_FOLDER_IMPORT") = "Files/"
    '        Session("ID_APPLICAZIONE") = ""
    '        Session("objSessione") = ""
    '        Session("COD_ENTE") = ""
    '        Session("DESCRIZIONE_ENTE") = ""
    '        Session("PATH_IMAGES") = ConfigurationManager.AppSettings("PATH_IMAGES")
    '        Session("PATH_PHOTO") = ConfigurationManager.AppSettings("PATH_PHOTO")

    '        'Session.Timeout = ConfigurationManager.AppSettings("SessionTimeout")
    '    Catch ex As Exception
    '        Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.Global.Session_Start.errore: ", ex)
    '        Response.Redirect("../PaginaErrore.aspx")
    '    End Try
    'End Sub
    ''' <summary>
    ''' Add security headers to help protection from injection attacks
    ''' </summary>
    Sub Application_PreSendRequestHeaders()
        Response.Headers.Remove("Server")
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when an error occurs
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session ends
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
        Try
            If New UtilityOPENgov().LogOff(COSTANTValue.ConstSession.UserName) Then
                Dim cookie1 As New HttpCookie(Web.Security.FormsAuthentication.FormsCookieName, "")
                cookie1.Expires = DateTime.Now.AddYears(-1)
                Response.Cookies.Add(cookie1)

                'clear session cookie (Not necessary for your current problem but i would recommend you do it anyway)
                Dim sessionStateSection As Web.Configuration.SessionStateSection = CType(Web.Configuration.WebConfigurationManager.GetSection("system.web/sessionState"), Web.Configuration.SessionStateSection)
                Dim cookie2 As New HttpCookie(sessionStateSection.CookieName, "")
                cookie2.Expires = DateTime.Now.AddYears(-1)
                Response.Cookies.Add(cookie2)

                HttpContext.Current.Session("username") = ""
            End If
        Catch ex As Exception
            Log.Debug("Application_End.LogOut.errore: ", ex)
            Response.Redirect("../../../PaginaErrore.aspx")
        End Try
    End Sub
End Class
