Imports System.Data.SqlClient
Imports log4net, log4net.Config
Imports System.IO
Imports System.Configuration

Partial Class _Default
    Inherits Page
    Dim Path As String
    Dim Applicazione As String
    Protected WithEvents Annulla As System.Web.UI.WebControls.Button
    'Private GestErrore As New VisualizzaErrore
    Dim Usr As String
    Dim Pwd As String
    Dim strvalidita As String


    

    ' PER LA LOG4NET
    Private Shared ReadOnly Log As ILog = LogManager.GetLogger(GetType(_Default))



#Region " Web Form Designer Generated Code "

  'This call is required by the Web Form Designer.
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
  Protected WithEvents Accedi As System.Web.UI.WebControls.Button

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
        'Dim strScript As String
        'Accedi.Attributes("onclick") = "Controlla()"
        'Accedi.Attributes("onclick") = "return false;"
        'strScript = "<script language='javascript'>" & vbCrLf
        'strScript = strScript & "document.getElementById ('Username').focus();" & vbCrLf
        'strScript = strScript & "</script>" & vbCrLf
        'ClientScript.RegisterStartupScript(Me.GetType(),"prova", strScript)

        Try


            'strvalidita = Request.Item("TestValidita")
            'If Not IsNothing(strvalidita) Then
            '    If strvalidita.CompareTo("") <> 0 Then
            '        Usr = Request.Item("username")
            '        Pwd = Request.Item("password")
            '        TestValidita.Text = "0"
            '        Username.Text = ""
            '        Password.Text = ""
            '        If Usr.CompareTo("") = 0 Or Pwd.CompareTo("") = 0 Then
            '            strScript = "<script language='javascript'>" & vbCrLf
            '            strScript = strScript & "alert ('Inserire username e password');" & vbCrLf
            '            strScript = strScript & "</script>" & vbCrLf
            '            ClientScript.RegisterStartupScript(Me.GetType(),"prova", strScript)
            '        Else
            '            Login()
            '        End If
            '    End If
            'End If

            Dim pathfileinfo As String
            pathfileinfo = ConfigurationManager.AppSettings("pathfileconflog4net")
            Dim fileconfiglog4net As FileInfo = New FileInfo(pathfileinfo)
            XmlConfigurator.ConfigureAndWatch(fileconfiglog4net)

            If Session("username") <> "" Then
                Dim fncActionEvent As New Utility.DBUtility(COSTANTValue.ConstSession.DBType, COSTANTValue.ConstSession.StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, Session("username"), "Login", "", "uscita dal sistema", "", "", -1)
            End If
            Session("COD_ENTE") = ""
            Session("DESCRIZIONE_ENTE") = ""
            Session("COD_TRIBUTO") = ""
            Session("username") = ""
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.Default.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub

    'Private Function Login()
    '    Dim strScript As String

    '    Dim objCreaSessione As GlobalSession
    '    Dim oSM As New RIBESFrameWork.SessionManager(Session("PARAMETROENV"))


    '    Path = Session("PATH_APPLICAZIONE") 'Request.Item("Path")
    '        Applicazione = Session("PARAMETROENV") ' Request.Item("Applicazione")

    '    'Usr = Username.Text
    '    'Pwd = Password.Text
    'Try
    '        If oSM.Initialize(Usr, Applicazione) Then
    '            Dim oSession = oSM.CreateSession(Session("IDENTIFICATIVOAPPLICAZIONE"))
    '            If oSession Is Nothing Then
    '                'Errore creazione Session
    '            Else
    '                If oSession.oErr.Number <> 0 Then
    '                    'Errore
    '                Else
    '                    Dim strConn As String
    '                    dim sSQL as string
    '                    Dim nRecord As Integer
    '                    Dim objConn As SqlConnection
    '                    objConn = oSession.oAppDB.GetConnection
    '                    If Not objConn Is Nothing Then

    '                        strConn = objConn.ConnectionString ' oSession.oAppDB.GetConnection.ConnectionString
    '                        'Session("Connessione") = strConn

    '                        sSQL = "select * from GESTIONE_UTENTI "
    '                        sSQL+=" where USERNAME='" & Usr & "'"
    '                        sSQL+=" and PASSWORD='" & Pwd & "'"

    '                        Dim adoRec = oSession.oAppDB.GetPrivateDataview(sSQL)
    '                        nRecord = adoRec.count
    '                        adoRec.dispose()

    '                        If nRecord <> 1 Then
    '                            'sono stati trovati troppi utenti
    '                        Else

    '                            Session("username") = Usr
    '                            Session("path") = Path
    '                            Session("Applicazione") = Applicazione

    '                            objCreaSessione = New GlobalSession("_OPENgov", Usr, Session("IDENTIFICATIVOAPPLICAZIONE"))

    '                            Session("objSessione") = objCreaSessione.objSession()
    '                            Username.Text = ""
    '                            Password.Text = ""
    '                            TestValidita.Text = ""


    '                            strScript = CaricaPagina(Path)
    '                            'Response.Write(str)
    '                            ClientScript.RegisterStartupScript(Me.GetType(),"apri", strScript)

    '                        End If

    '                    End If

    '                End If
    '            End If
    '        End If
    '        oSM = Nothing



    'Catch ex As Exception
    '    Response.Write(GestErrore.GetHTMLError(ex, "../Styles.css"))
    '    Response.End()

    'End Try
    ' Catch ex As Exception
    '   Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.Default.Login.errore: ", ex)
    '   Response.Redirect("../../PaginaErrore.aspx")
    'End Try
    'End Function

    'Private Sub Accedi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Accedi.Click
    'Try


    '    Dim Usr As String
    '    Dim Pwd As String
    '    Dim strScript As String

    '    Dim objCreaSessione As GlobalSession
    '    Dim oSM As New RIBESFrameWork.SessionManager(Session("PARAMETROENV"))

    '    If Username.Text = "" Or Password.Text = "" Then
    '        strScript = "<script language='javascript'>" & vbCrLf
    '        strScript = strScript & "alert ('Inserire username e password');" & vbCrLf
    '        strScript = strScript & "</script>" & vbCrLf
    '        ClientScript.RegisterStartupScript(Me.GetType(),"prova", strScript)
    '    Else


    '        Path = Session("PATH_APPLICAZIONE") 'Request.Item("Path")
    '        Applicazione = Session("PARAMETROENV") ' Request.Item("Applicazione")
    '        'If CInt(TestValidita.Text) = 1 Then
    '        Usr = Username.Text
    '        Pwd = Password.Text
    '        If oSM.Initialize(Usr, Applicazione) Then
    '            Dim oSession = oSM.CreateSession(Session("IDENTIFICATIVOAPPLICAZIONE"))
    '            If oSession Is Nothing Then
    '                'Errore creazione Session
    '            Else
    '                If oSession.oErr.Number <> 0 Then
    '                    'Errore
    '                Else
    '                    Dim strConn As String
    '                    dim sSQL as string
    '                    Dim nRecord As Integer
    '                    Dim objConn As SqlConnection
    '                    objConn = oSession.oAppDB.GetConnection
    '                    If Not objConn Is Nothing Then

    '                        strConn = objConn.ConnectionString ' oSession.oAppDB.GetConnection.ConnectionString
    '                        'Session("Connessione") = strConn

    '                        sSQL = "select * from GESTIONE_UTENTI "
    '                        sSQL+=" where USERNAME='" & Usr & "'"
    '                        sSQL+=" and PASSWORD='" & Pwd & "'"

    '                        Dim adoRec = oSession.oAppDB.GetPrivateDataview(sSQL)
    '                        nRecord = adoRec.count
    '                        adoRec.dispose()

    '                        If nRecord <> 1 Then
    '                            'sono stati trovati troppi utenti
    '                        Else

    '                            Session("username") = Usr
    '                            Session("path") = Path
    '                            Session("Applicazione") = Applicazione

    '                            objCreaSessione = New GlobalSession("_OPENgov", Usr, Session("IDENTIFICATIVOAPPLICAZIONE"))

    '                            Session("objSessione") = objCreaSessione.objSession()
    '                            Username.Text = ""
    '                            Password.Text = ""
    '                            TestValidita.Text = ""


    '                            strScript = CaricaPagina(Path)
    '                            'Response.Write(str)
    '                            ClientScript.RegisterStartupScript(Me.GetType(),"apri", strScript)

    '                        End If

    '                    End If

    '                End If
    '            End If
    '        End If
    '        oSM = Nothing
    '        'End If
    '    End If

    'Catch ex As Exception

    '    Response.Write(GestErrore.GetHTMLError(ex, "../Styles.css"))
    '    Response.End()
    '      Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.Default.Accedi_Click.errore: ", ex)
    '     Response.Redirect("../../PaginaErrore.aspx")


    'End Try
    'End Sub


    Private Function CaricaPagina(ByVal Path As String) As String

        Dim sHTML As String
    sHTML = ""
    sHTML = sHTML & vbCrLf & "<script>"

    sHTML = sHTML & vbCrLf & "myheight = screen.height - 75"
    sHTML = sHTML & vbCrLf & "mywidth = screen.width-10"
        'sHTML = sHTML & vbCrLf & "window.open('" & Path & "/Generali/asp/aspFrameIniziale.aspx','openGov','toolbar=no,status=yes,left=0,top=0,height ='+myheight+',width='+mywidth).focus();"
        sHTML = sHTML & vbCrLf & "window.open('" & Path & "/Generali/asp/aspFrameIniziale.aspx','openGov').focus();"
    'sHTML = sHTML & vbCrLf & "newwin.focus()"
    'sHTML = sHTML & vbCrLf & "document.location.href='default.aspx'"
    sHTML = sHTML & vbCrLf & "</script>"


    CaricaPagina = sHTML

  End Function

End Class
