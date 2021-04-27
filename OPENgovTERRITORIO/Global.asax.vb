Imports System.Web
Imports System.Web.SessionState
Imports System.Configuration
Imports log4net, log4net.Config

Public Class Global1
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
        'EO.Web.Runtime.AddLicense( _
        '    "HOC0cq63yeOxaqe9xty7aOPt9BDtrNzN9emMQ5ekscu7peDn9hnyntzC1Azv" + _
        '    "ounz/xCffuX2+g7udabw+g7kp+rp9umMQ5ekscu7muPwACK9RoGkscufWZek" + _
        '    "sefgndukBSTvnrSm3hDtrpmkBxDxrODz/+ihb6W0s8uud4SOscufWZekscu7" + _
        '    "mtvosR/4qdzBs/7rotvp3hDtrpmkBxDxrODz/+ihb6W0s8uud4SOscufWZek" + _
        '    "scu7mtvosR/4qdzBs//gm8r4AxTvW5f69h3youbyzs21Z6emsdq9RoGkscuf" + _
        '    "WZeksefgndukBSTvnrSm5R3kns3t9iKhWe3pAx7oqOXBs+GtaZmkwOmMQ5ek" + _
        '    "scufWZekzQzjnZf4ChvkdpnRBhfzosfl+BChWe3pAx7oqOXBs+GtaZmkwOmM" + _
        '    "Q5ekscufWZekzQzjnZf4ChvkdpnH8hfkp9vlA82fr9z2BBTup7Smx9mvW5ez" + _
        '    "z7iJWZekscufWZfA8g/jWev9ARC8W7rl/Rfhmtrvs8v1nun3+hrtdpm6v9uh" + _
        '    "WabCnrWfWZekscufWbPl9Q+frfD09uihesHF6QDvpebl9RDxW5f69h3youby" + _
        '    "zs21Z6emsdq9RoGkscufWZeksefgndukBSTvnrSm1RTgpebrs8v1nun3+hrt" + _
        '    "dpm6v9uhWabCnrWfWZekscufWbPl9Q+frfD09uihjOfw+h/znummsSHkq+rt" + _
        '    "ABm8W62ywc2faLWRm8ufWZekscufddjo9cvzsufpzs3Mmurv9g/EneD4s8v1" + _
        '    "nun3+hrtdpm6v9uhWabCnrWfWZekscufWbPl9Q+frfD09uihgOnt9c2fr9z2" + _
        '    "BBTup7Smx9mvW5ezz7iJWZekseeumuPwACK9RoGkscufdert+Bngrez29unu" + _
        '    "ssrNxQPVnsbUwNzrnOTY2/jlkOfFzui7aOrt+Bngrez29umMQ7Oz/RTinuX3" + _
        '    "9umMQ3Xj7fQQ7azcwp61n1mXpM0X6Jzc8gQQyJ21t8Q=")
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started
        Session("COD_TRIBUTO") = ConfigurationManager.AppSettings("COD_TRIBUTO")
        Session("PATH_APPLICAZIONE") = ConfigurationManager.AppSettings("PATH_APPLICAZIONE")
        Session("PATH_IMAGES") = ConfigurationManager.AppSettings("PATH_IMAGES")
        Session("PATH_PHOTO") = ConfigurationManager.AppSettings("PATH_PHOTO")
        Session("PARAMETROENV") = ConfigurationManager.AppSettings("PARAMETROENV")
        Session("IDENTIFICATIVOAPPLICAZIONE") = ConfigurationManager.AppSettings("IDENTIFICATIVOAPPLICAZIONE")
        Session("Anagrafica") = ConfigurationManager.AppSettings("ANAGRAFICA")
        If Not ConfigurationManager.AppSettings("ConnByWF") Is Nothing Then
            Session("IsConnectionByWorkFlow") = ConfigurationManager.AppSettings("ConnByWF")
        Else
            Session("IsConnectionByWorkFlow") = False
        End If
        '/* S.T. DEBUG */
        'Session("username") = "RIADM"
        'Session("COD_ENTE") = "050027"
        'Session("DESCRIZIONE_ENTE") = "POMARANCE"
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
    End Sub

End Class

