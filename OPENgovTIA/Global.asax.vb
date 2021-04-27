Imports System.Web
Imports System.Web.SessionState
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
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started
        '/* S.T. DEBUG */
        ''Session("PARAMETROENV") = "OG_GECGrandCombin_PRE_RIBES" '"OG_TRIBUTI_PRE_RIBES" '"OG_TRIBUTI_PRO_RIBES" '
        ''Session("IDENTIFICATIVOAPPLICAZIONE") = "OPENGOVTA" '"OPENGOVTIA" '
        'Session("username") = "RIADM"
        'Session("ambiente") = "" '"CMGC" '"CMMC" '
        'Session("COD_ENTE") = "050027" '"7026" '"7057" '"050019" '"096024" '"7015" '"7069" '"7016" '"7025" '"7016" '"7046" '"7024" '"7030" '"7002" '"7072" '"7010" '"7039" '"003115" '"7001" '"7062" '"7047" '"7064" '"006085" '
        'Session("DESCRIZIONE_ENTE") = "POMARANCE" '"ETROUBLES" '"roisan" '"montecatini val di cecina" '"donato" '"chambave" '"valpelline" '"chamois" '"emarese" '"CHAMOIS" '"ollomont" '"doues" '"gignod" '"antey" '"verrayes" '"bionaz" '"lamagdeleine" '"pella" '"allein" '"SaintOyen" '"oyace" '"saintrhemy" '"grondona" '
        'Session("COD_BELFIORE") = "G804" '"D444" '"H497" '"F458" '"D339" '"C595" '"L643" '"B491" '"D402" '"b491" '"G045" '"A205" '"E029" '"A305" '"L783" '"G421" '"A877" '"A308" '"D356" '"" '"G012" '"H675" '"E191" '
        'Session("IdTypeAteco") = "1" '"2" '
        'Session("VisualGIS") = True
        'Session("IsFromTARES") = "1"
        ''N.B. cambia parametri avvisi-riduzioni CMGC / RIBES
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
