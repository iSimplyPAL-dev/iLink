Imports System.Data.SqlClient
Imports OPENUtility
'Imports ComPlusInterface
Imports Anater.Oggetti
Imports RemotingInterfaceAnater
Imports GestioneStradarioVaniDLL.ReadAnater
Imports log4net
Imports System.Web.Security
''' <summary>
''' Pagina per l'accesso al sistema
''' Contiene i parametri di accesso e le funzioni della comandiera.
''' Le possibili opzioni sono:
''' - Accedi
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
''' <revisionHistory>
''' <revision date="26/06/2018">
''' <strong>Adeguamento GDPR</strong>
''' Per adeguare OPENgov all’art. 13 del Regolamento (UE) n. 679/2016 (“GDPR”) bisogna introdurre le logiche di validazione formale della password, la password dovrà essere cambiata dopo un certo tempo e dopo un certo numero di tentativi errati si sarà bloccati per un certo tempo.
''' Dovrà inoltre essere bloccato il primo cambio password se non si accetta l'informativa sul trattamento dei dati personali.
''' 
''' <em>Password</em>
''' <c>Formattazione</c>
''' La password deve:
''' •	essere almeno lunga 8 caratteri
''' •	avere almeno una lettera
''' •	avere almeno un numero
''' •	avere almeno un carattere speciale
''' •	avere almeno una lettera minuscola
''' •	avere almeno una lettera maiuscola
''' <c>Validità</c>
''' La password dovrà essere cambiata ogni 90 giorni. Dopo l'autenticazione il sistema controllerà se è passato il periodo di validità; se si, l’utente sarà reindirizzato alla nuova pagina di cambio password. 
''' <c>Numero Tentativi</c>
''' Dopo 3 tentativi di accesso con password errata l'utente sarà bloccato per 15 minuti, trascorso questo tempo l’utente potrà riprovare a collegarsi.
''' </revision>
''' </revisionHistory>
Partial Class Login
    Inherits Page
    Private Shared Log As ILog = LogManager.GetLogger(GetType(Login))
    Dim Path As String
    Dim Applicazione As String
    Private GestErrore As New VisualizzaErrore
    Private IP As String

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
    ''' <summary>
    ''' Determino se autenticare l'operatore "normalmente" oppure autorizzarlo e profilarlo tramite ANATER se Request.Item("ANATER") è = 1 vuol dire che la chiamata arriva dall'applicativo del Grand Combin e quindi devo richiamare la procedura di autorizzazione tramite ANATER altrimenti vuol dire che carico la pagina "normalmente", faccio inserire le credenziali ed eseguo la login "classica".
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Accedi.Attributes("onclick") = "Controlla()"
        Dim sUsername As String = String.Empty
        Dim sPassword As String = String.Empty
        Dim sProfilo As String = String.Empty
        Dim sCod_ente As String = String.Empty
        Dim sLoginType As String = String.Empty
        Dim sErrore As String = String.Empty
        Dim bLoginTypeRiconosciuto As Boolean = False

        Dim FncLog As New SelectEnti
        FncLog.ClearVarEnte()
        Session("usernameAnater") = ""

        'usa anater oppure no
        'I dati LOGINTYPE, NOME_OPERATORE, PASSWORD, CODICE_ENTE, PROFILO arrivano dalla pagina default.aspx
        If Request.Item("LOGINTYPE") <> "" Then
            'username
            'If Not Request.Item("USERNAME") Is Nothing Or Request.Item("USERNAME") <> "" Then
            If Request.Item("NOME_OPERATORE") <> "" Then
                sUsername = Request.Item("NOME_OPERATORE")
            Else
                sErrore = sErrore & "Parametro NOME_OPERATORE non valorizzato. "
            End If
            'password
            'DIPE 09/01/2008 Lapassword non viene passata da anater
            'If Not Request.Item("PASSWORD") Is Nothing Or Request.Item("PASSWORD") <> "" Then
            sPassword = Request.Item("PASSWORD")
            'If Request.Item("PASSWORD") <> "" Then
            '    sPassword = Request.Item("PASSWORD")
            'Else
            '    sErrore = sErrore & "Parametro PASSWORD non valorizzato. "
            'End If

            'COD_ENTE
            If Request.Item("CODICE_ENTE") <> "" Then
                sCod_ente = Request.Item("CODICE_ENTE")
            Else
                sErrore = sErrore & "Parametro CODICE_ENTE non valorizzato. "
            End If
            'profilo
            'If Not Request.Item("PROFILO") Is Nothing Or Request.Item("PROFILO") <> "" Then
            If Request.Item("PROFILO") <> "" Then
                sProfilo = Request.Item("PROFILO")
                If sProfilo.ToUpper = COSTANTValue.ConstSession.TipoProfilo.SolaLettura Or sProfilo.ToUpper = COSTANTValue.ConstSession.TipoProfilo.SingolaVoceLettura Then
                    Session("SOLA_LETTURA") = "1"
                Else
                    Session("SOLA_LETTURA") = "0"
                End If
            Else
                sErrore = sErrore & "Parametro PROFILO non valorizzato. "
            End If

            If sErrore.Length > 0 Then
                sErrore = sErrore & "Impossibile Effettuare il Login."
                Response.Redirect("../PaginaErrore.aspx")
            End If

            If sLoginType = "A" Then 'ANATER
                Log.Debug("mi loggo da ANATER")
                AnaterLogin(sUsername, sPassword, sProfilo, sCod_ente)
            End If
            sLoginType = Request.Item("LOGINTYPE")
            bLoginTypeRiconosciuto = True
            If bLoginTypeRiconosciuto = False Then
                sErrore = "Parametro LOGINTYPE non riconosciuto dal sistema. Impossibile Effettuare il Login."
                Response.Redirect("../PaginaErrore.aspx")
            End If
        Else
            Session("SOLA_LETTURA") = "0"
            'carico la pagina "normalmente" e faccio inserire le credenziali
            'quindi a livello di codice non faccio nulla
        End If
        ip = Request.UserHostAddress.ToString
        IP = Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        'when user is behind proxy server
        If (IP Is Nothing) Then
            IP = Request.ServerVariables("REMOTE_ADDR")
            'Without proxy
        End If
        Username.Attributes.Add("onkeydown", "keyPress();")
        Password.Attributes.Add("onkeydown", "keyPress();")
        If Page.IsPostBack = False Then
            Dim fncActionEvent As New Utility.DBUtility(COSTANTValue.ConstSession.DBType, COSTANTValue.ConstSession.StringConnectionOPENgov)
            fncActionEvent.LogActionEvent(DateTime.Now, IP, "Login", "", Utility.Costanti.AZIONE_LETTURA.ToString, "", "", -1)
        End If
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Accedi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Accedi.Click
        Try
            Dim Usr As String
            Dim Pwd As String
            Dim str As String = ""

            Log.Debug("Indirizzo IP client : " & IP)

            Usr = Username.Text
            Pwd = Password.Text

            str = New UtilityOPENgov().CheckLogin(Usr, Pwd, IP, lblMessage)
            'set the forms auth cookie
            'FormsAuthentication.SetAuthCookie(Usr, True) ***works only in SSL
            Dim myTicket As New FormsAuthenticationTicket(1, FormsAuthentication.FormsCookieName, Now, DateAdd(DateInterval.Minute, HttpContext.Current.Session.Timeout, Now), True, Usr + "|" + IP)
            'Create an HttpOnly cookie.
            Dim myHttpOnlyCookie As New HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(myTicket))
            'Setting the HttpOnly value to true, makes this cookie accessible only to ASP.NET.
            myHttpOnlyCookie.HttpOnly = True
            Response.AppendCookie(myHttpOnlyCookie)
            'create a cookie
            Dim myCookieName As String = "aplckute"
            myHttpOnlyCookie = New HttpCookie(myCookieName, FormsAuthentication.Encrypt(New FormsAuthenticationTicket(1, myCookieName, Now, DateAdd(DateInterval.Minute, HttpContext.Current.Session.Timeout, Now), True, Usr + "|" + IP)))
            'Setting the HttpOnly value to true, makes this cookie accessible only to ASP.NET.
            myHttpOnlyCookie.HttpOnly = True
            Response.AppendCookie(myHttpOnlyCookie)
            BasePage.authCookie = HttpContext.Current.Request.Cookies.Get(FormsAuthentication.FormsCookieName)
            Username.Text = ""
            Password.Text = ""

            ClientScript.RegisterStartupScript(Me.GetType(), "apri", str)
        Catch ex As Exception
            Log.Debug("OPENgov.Login.Accedi_Click.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Profilo"></param>
    ''' <returns></returns>
    Private Function AutorizzaDaAnater(ByVal Profilo As String) As Boolean
        'Dim objCreaSessione As GlobalSession

        'Dim oSM As New RIBESFrameWork.SessionManager(Session("PARAMETROENV"))
        Try

            Log.Debug("mi autorizzo da ANATER::profilo::" & Profilo)

            Session("IDENTIFICATIVOAPPLICAZIONE") = "OPENGOVG"
            Session("COD_OPERATORE") = ConfigurationManager.AppSettings("COD_OPERATORE")

            Path = Session("PATH_APPLICAZIONE").ToString 'Request.Item("Path")
            If CInt(Request.Item("TestValidita")) = 0 Then
                Dim FncLog As New UtilityOPENgov
                If FncLog.IsValidUser(Profilo, String.Empty) Then 'If oSM.Initialize(Profilo, Applicazione) Then
                    'Dim oSession = oSM.CreateSession(Session("IDENTIFICATIVOAPPLICAZIONE"))
                    'If oSession Is Nothing Then
                    '    'Errore creazione Session
                    '    Log.Debug("errore in autorizzo da anater 1")
                    '    Return False
                    'Else
                    '    If oSession.oErr.Number <> 0 Then
                    '        Log.Debug("errore in autorizzo da anater 2")
                    '        Return False
                    '    End If
                    'End If
                Else
                    Log.Debug("errore in autorizzo da anater 3")
                    Return False
                End If
            End If

            Session("username") = Profilo
            Session("path") = Path

            'objCreaSessione = New GlobalSession(Applicazione, Profilo, "OPENGOVP")

            'Session("objSessione") = objCreaSessione.objSession()

            Return True
        Catch ex As Exception
            Log.Debug("OPENgov.Login.AutorizzaDaAnater.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
            Return False
        Finally
            'oSM.Terminate()
            'oSM = Nothing
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sUsername"></param>
    ''' <param name="sPassword"></param>
    ''' <param name="sProfilo"></param>
    ''' <param name="sCodEnte"></param>
    Private Sub AnaterLogin(sUsername As String, sPassword As String, sProfilo As String, sCodEnte As String)
        Dim sErrore As String = ""
        Dim bAuthorization As Boolean
        Dim oArrayEnti As String()
        Dim sScript As String = ""
        Dim clSelectEnti As New SelectEnti
        Dim dsEnti As New DataSet

        Try
            Session("usernameAnater") = sUsername
            'richiamo l'autorizzazione
            bAuthorization = AutorizzaDaAnater(sProfilo)
            If bAuthorization = False Then
                sErrore = "Utente non autorizzato all'accesso dell'applicativo. Impossibile Effettuare il Login."
                Response.Redirect("../PaginaErrore.aspx")
            End If

            'richiamo il metodo di autorizzazione di anater
            ' OGGETTO PER LA CONNESSIONE ALL'ANAGRAFICA DI ANATER
            Try
                Log.Debug("controllo validita operatore da ANATER::sUsername::" & sUsername & "::sCod_ente::" & sCodEnte)
                Dim remObject As IRemotingInterfaceLogin = Activator.GetObject(GetType(IRemotingInterfaceLogin), ConfigurationManager.AppSettings("URLanaterLogin"))
                oArrayEnti = remObject.GetValidateOperatore(sUsername, sPassword, sCodEnte)
                remObject = Nothing
            Catch ex As Exception
                Log.Debug("OPENgov.Login.Page_Load.controllo validita operatore da ANATER errore::", ex)
                sScript += "<script language='javascript'>"
                'strscript+="document.location.href=""../Generali/asp/aspVuotaHome.aspx"";")
                sScript += "document.getElementById(""Form1"").style.display=""none"";"
                sScript += "GestAlert('a', 'warning', '', '', 'Attenzione, si sono verificati dei problemi di connessione, contattare l\'assistenza');"
                sScript += "</script>"
                ClientScript.RegisterStartupScript(Me.GetType(), "strErrore", sScript.ToString)
                Exit Sub
            End Try

            If oArrayEnti.Length = 0 Then
                sErrore = sErrore & "Utente non autenticato in Anater. Impossibile Effettuare il Login."
                Response.Redirect("../PaginaErrore.aspx")
            End If

            dsEnti = clSelectEnti.GetEntiByUser(sProfilo, oArrayEnti)
            If dsEnti.Tables(0).Rows.Count = 0 Then
                sErrore = sErrore & "Utente non abilitato a nessun ente. Impossibile Effettuare il Login."
                Response.Redirect("../PaginaErrore.aspx")
            Else
                Session.Add("ELENCO_ENTI_ANATER", oArrayEnti)
            End If

            'DIPE 08/05/2008
            'Qui devo inserire il codice per controllare se ci sono delle strade
            'o dei vani da modificare rispetto al db di Anater.

            'Dim oArrayStradario As String()
            'Dim oArrayVani As String()

            'Dim oModificaStradario() As OggettoModificaStradario
            'Dim oModificaVani() As OggettoModificaVani
            'Dim remObjectSV As IRemotingInterfaceModificaStradarioVani = Activator.GetObject(GetType(IRemotingInterfaceModificaStradarioVani), ConfigurationManager.AppSettings("URLanaterStradarioVani"))
            'oModificaStradario = remObjectSV.GetStradario()
            'oModificaVani = remObjectSV.GetVani()

            Dim Str As String
            Dim iNumStrade, iNumVani As Integer
            Dim FncStradarioVani As New StradarioVani
            Dim objDT As DataTable
            Try

                'Dim objAnaStradario As New GestioneStradarioVaniDLL.ReadAnater("S")
                'objDT = objAnaStradario.selectStradarioVani(sCod_ente)
                objDT = FncStradarioVani.selectStradarioVani("S", sCodEnte)
                If Not objDT Is Nothing Then
                    iNumStrade = objDT.Rows.Count
                    If iNumStrade > 0 Then
                        Session("DTStradario") = objDT
                    End If
                End If

                'Dim objAnaVani As New GestioneStradarioVaniDLL.ReadAnater("V")
                'objDT = objAnaVani.selectStradarioVani(sCod_ente)
                objDT = FncStradarioVani.selectStradarioVani("V", sCodEnte)
                If Not objDT Is Nothing Then
                    iNumVani = objDT.Rows.Count
                    If iNumVani > 0 Then
                        Session("DTVani") = objDT
                    End If
                End If
            Catch ex As Exception
                Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.Login.Page_Load.errore: ", ex)
                sScript = "<script language='javascript'>"
                'strscript+="document.location.href=""../Generali/asp/aspVuotaHome.aspx"";")
                sScript += "document.getElementById(""Form1"").style.display=""none"";"
                sScript += "GestAlert('a', 'warning', '', '', 'Attenzione, si sono verificati dei problemi di connessione, contattare l\'assistenza');"
                sScript += "</script>"
                ClientScript.RegisterStartupScript(Me.GetType(), "strErrore", sScript.ToString)
                Log.Debug("devo controllare se ci sono delle strade o dei vani da modificare rispetto al db di Anater::si è verificato il seguente errore::" & ex.Message)
                'Exit Sub
            End Try

            'If (oModificaStradario.Length > 0 Or oModificaVani.Length > 0) Then
            If (iNumStrade > 0 Or iNumVani > 0) Then
                'Trovate strade o vani da modificare
                Session("oModificaStradario") = iNumStrade
                Session("oModificaVani") = iNumVani
                Session("dsEnti") = dsEnti
                Str = CaricaPaginaModStradarioVani()
                ClientScript.RegisterStartupScript(Me.GetType(), "apri", Str)
            Else
                'str = CaricaPagina(Path)
                Str = New UtilityOPENgov().CaricaPagina(Path, dsEnti)
                ClientScript.RegisterStartupScript(Me.GetType(), "apri", Str)
            End If
        Catch ex As Exception
            Log.Debug("OPENgov.Login.AnaterLogin.errore: ", ex)
            Response.Redirect("../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Private Function CaricaPaginaModStradarioVani() As String
        Dim sHTML As String = ""
        sHTML += "<script>"
        sHTML += "location.href='../Generali/asp/aspVuotaHome.aspx';"
        sHTML += "parent.document.getElementById('ifrmEnti').src='ModificaStradarioVani.aspx';"
        sHTML += "</script>"
        CaricaPaginaModStradarioVani = sHTML
    End Function
End Class

