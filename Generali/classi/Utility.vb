Imports System
Imports System.Data.SqlClient
Imports System.Security.Cryptography
Imports System.Web.Security
Imports log4net
Imports Utility
''' <summary>
''' Classe generale che eredita Page.
''' Viene inclusa in tutti i form perché contiene le funzioni basi che tutti i form devono avere.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class BasePage
    Inherits Page
    Private Shared Log As ILog = LogManager.GetLogger(GetType(BasePage))
    Public Shared authCookie As HttpCookie
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BasePage_Init(sender As Object, e As EventArgs) Handles Me.Init
        Session("COD_TRIBUTO") = String.Empty
        Try
            Dim isExpired As Boolean = False
            'get cookie
            If authCookie Is Nothing Then
                authCookie = HttpContext.Current.Request.Cookies.Get("aplckute")
            End If
            If Not authCookie Is Nothing Then
                Dim authTicket As FormsAuthenticationTicket = FormsAuthentication.Decrypt(authCookie.Value)
                If Not authTicket Is Nothing Then
                    If authTicket.Expired = False Then
                        Dim myData As String() = authTicket.UserData.Split("|")
                        If Not myData Is Nothing Then
                            If myData.GetUpperBound(0) <> 1 Then
                                Log.Debug("sessione scaduta perchè non ho userdata nel cookie")
                                isExpired = True
                            Else
                                'If myData(0) <> COSTANTValue.ConstSession.UserName Then
                                Log.Debug("utente cookie->" + myData(0) + "  utente sessione->" + COSTANTValue.ConstSession.UserName)
                                If COSTANTValue.ConstSession.UserName <> "" Then
                                    'Log.Debug("sessione scaduta perchè utente cookie diverso da utente sessione")
                                    'isExpired = True
                                Else
                                    Session("username") = myData(0)
                                End If
                                'End If
                            End If
                        Else
                            Log.Debug("sessione scaduta perchè userdata cookie null")
                            isExpired = True
                        End If
                    Else
                        Log.Debug("sessione scaduta perchè cookie scaduto")
                        isExpired = True
                    End If
                Else
                    Log.Debug("sessione scaduta perchè non ticket cookie")
                    isExpired = True
                End If
            Else
                Log.Debug("sessione scaduta perchè non cookie")
                isExpired = True
            End If
            If COSTANTValue.ConstSession.UserName = "" Then
                Log.Debug("sessione scaduta perchè sessione operatore vuoto")
                isExpired = True
            End If
            If isExpired = True Then
                HttpContext.Current.Session("username") = ""
                'RegisterScript("GestAlert('a', 'warning', '', '', 'Sessione scaduta rieffettuare LOGIN');parent.location.href = '" & Request.Url.GetLeftPart(UriPartial.Authority) & "/" & Request.ApplicationPath & "/Default.aspx';", Me.GetType)
                RegisterScript("GestAlert('a', 'warning', 'CmdLogOut', '', 'Sessione scaduta rieffettuare LOGIN');", Me.GetType)
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte + "." + COSTANTValue.ConstSession.UserName + " - OPENgov.Utility.BasePage_Init.errore: ", ex)
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="script"></param>
    ''' <param name="type"></param>
    Protected Sub RegisterScript(ByVal script As String, ByVal type As Type)
        Business.ConstWrapper.CountScript = (Business.ConstWrapper.CountScript + 1)
        Dim uniqueId As String = ("spc_" _
                    + (Business.ConstWrapper.CountScript.ToString _
                    + (DateTime.Now.ToString + ("." + DateTime.Now.Millisecond.ToString))))
        Dim sScript As String = "<script language='javascript'>"
        sScript = (sScript + script)
        sScript = (sScript + "</script>")
        ClientScript.RegisterStartupScript(type, uniqueId, sScript)
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="s"></param>
    ''' <param name="result"></param>
    ''' <returns></returns>
    Public Shared Function GuidTryParse(ByVal s As String, ByRef result As Guid) As Boolean
        If (s Is Nothing) Then
            Throw New ArgumentNullException("s")
        End If

        Dim format As Text.RegularExpressions.Regex = New Text.RegularExpressions.Regex(("^[A-Fa-f0-9]{32}$|" + ("^({|\()?[A-Fa-f0-9]{8}-([A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}(}|\))?$|" + "^({)?[0xA-Fa-f0-9]{3,10}(, {0,1}[0xA-Fa-f0-9]{3,6}){2}, {0,1}({)([0xA-Fa-f0-9]{3,4}, {0,1}){7}[0xA-Fa-f0-9]{3,4}(}})$")))
        Dim match As Text.RegularExpressions.Match = format.Match(s)
        If match.Success Then
            result = New Guid(s)
            Return True
        Else
            result = Guid.Empty
            Return False
        End If
    End Function
End Class

''' <summary>
''' Classe che incapsula tutti le utility necessarie
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class UtilityOPENgov
    Dim Constant As Utility.Costanti
    Private Shared Log As ILog = LogManager.GetLogger(GetType(UtilityOPENgov))

#Region "Gestione Credenziali di accesso"
    '*** 201805 - Expire&Strong Password ***
    Public Enum SignInStatus
        Success = 0
        LockedOut = 1
        RequiresVerification = 2
        FailurePwd = 3
        FailureUser = 4
    End Enum
    ''' <summary>
    ''' Funzione che verifica se è scaduto il tempo di validita della password
    ''' </summary>
    ''' <param name="sLoginName"></param>
    ''' <param name="ExpiredDays"></param>
    ''' <returns></returns>
    Public Function IsPasswordExpired(ByVal sLoginName As String, ByVal ExpiredDays As Integer) As Boolean
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dtMyDati As New DataTable()
        Dim cmdMyCommand As New SqlClient.SqlCommand
        Dim RetVal As Boolean = False

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(COSTANTValue.ConstSession.StringConnectionOPENgov)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_CheckPasswordExpired"
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@LOGINNAME", SqlDbType.NVarChar)).Value = sLoginName
            cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@EXPIREDDAYS", SqlDbType.Int)).Value = ExpiredDays
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(dtMyDati)
            For Each myRow As DataRow In dtMyDati.Rows
                If myRow("ISEXPIRED").ToString = "1" Then
                    RetVal = True
                End If
                HttpContext.Current.Session("LASTCHANGEPWD") = myRow("LASTCHANGEPWD")
            Next
        Catch ex As Exception
            Log.Debug("OPENgov.UtilityOPENgov.IsPasswordExpired.errore: ", ex)
            Throw New Exception
        Finally
            myAdapter.Dispose()
            dtMyDati.Dispose()
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Funzione che autentica ed autorizza l'operatore verificando se la password è scaduta e il numero di tentativi di accesso
    ''' </summary>
    ''' <param name="Usr">stringa username</param>
    ''' <param name="Pwd">stringa password</param>
    ''' <param name="IP">stringa IP</param>
    ''' <param name="lblMessage">ref label messaggio di errore</param>
    ''' <returns></returns>
    Public Function CheckLogin(ByVal Usr As String, ByVal Pwd As String, IP As String, ByRef lblMessage As Label) As String
        Dim sScript As String = "<script language='javascript'>"

        Try
            HttpContext.Current.Session("IDENTIFICATIVOAPPLICAZIONE") = "OPENGOVG"
            HttpContext.Current.Session("COD_OPERATORE") = ConfigurationManager.AppSettings("COD_OPERATORE")

            Dim Path As String = HttpContext.Current.Session("PATH_APPLICAZIONE").ToString 'Request.Item("Path")
            Dim RetVal As Integer
            RetVal = IsValidUser(Usr, Pwd)
            Select Case RetVal
                Case UtilityOPENgov.SignInStatus.Success
                    If CheckLockOut(IP, RetVal) Then
                        Log.Error("Utente non trovato USERNAME='" & Usr & "' PASSWORD='" & Pwd & "'")
                        sScript += "GestAlert('a', 'warning', '', '', 'Questo account è stato bloccato, riprovare più tardi.');"
                        lblMessage.Text = "Questo account è stato bloccato, riprovare più tardi."
                    Else
                        If MultipleIPLock(Usr, IP) Then
                            sScript += "GestAlert('a', 'warning', '', '', 'Accesso non valido! L\'utente è già loggato da un\'altra postazione.');"
                            lblMessage.Text = "Accesso non valido! L'utente è già loggato da un\'altra postazione."
                        Else
                            SetLogginIP(Usr, IP, Now)
                            If IsPasswordExpired(Usr, COSTANTValue.ConstSession.PasswordExpireDays) Then
                                sScript += "parent.location.href='../Generali/asp/aspFrameIniziale.aspx?utente=" + Usr + "';"
                            Else
                                Dim fncActionEvent As New Utility.DBUtility(COSTANTValue.ConstSession.DBType, COSTANTValue.ConstSession.StringConnectionOPENgov)
                                fncActionEvent.LogActionEvent(DateTime.Now, Usr, "Login", "", "accesso al sistema", IP, "", -1)
                                Log.Debug("Utente '" & Usr & "' autenticato")
                                HttpContext.Current.Session("path") = Path

                                Dim clSelectEnti As New SelectEnti
                                Dim dsEnti As New DataSet
                                dsEnti = clSelectEnti.GetEntiByUser(Usr)

                                sScript += CaricaPagina(Path, dsEnti)
                            End If
                        End If
                    End If
                Case UtilityOPENgov.SignInStatus.FailureUser
                    Log.Debug("Utente non trovato USERNAME='" & Usr)
                    sScript += "GestAlert('a', 'danger', '', '', 'Credenziali non valide!');"
                    lblMessage.Text = "Credenziali non valide!"
                Case UtilityOPENgov.SignInStatus.FailurePwd
                    If CheckLockOut(IP, RetVal) Then
                        Log.Error("Utente non trovato USERNAME='" & Usr & "' PASSWORD='" & Pwd & "'")
                        sScript += "GestAlert('a', 'warning', '', '', 'Questo account è stato bloccato, riprovare più tardi.');"
                        lblMessage.Text = "Questo account è stato bloccato, riprovare più tardi."
                    Else
                        Log.Debug("Password errata USERNAME='" & Usr & "' PASSWORD='" & Pwd & "'")
                        sScript += "GestAlert('a', 'danger', '', '', 'Credenziali non valide!');"
                        lblMessage.Text = "Credenziali non valide!"
                    End If
                Case UtilityOPENgov.SignInStatus.LockedOut
                    Log.Error("Account bloccato USERNAME='" & Usr & "' PASSWORD='" & Pwd & "'")
                    sScript += "GestAlert('a', 'warning', '', '', 'Questo account è stato bloccato, riprovare più tardi.');"
                    lblMessage.Text = "Questo account è stato bloccato, riprovare più tardi."
                Case UtilityOPENgov.SignInStatus.RequiresVerification
                    sScript += "GestAlert('a', 'warning', '', '', 'Problemi in autenticazione, riprovare più tardi.');"
                    lblMessage.Text = "Problemi in autenticazione, riprovare più tardi."
            End Select
            '*** ***
        Catch ex As Exception
            Log.Debug("OPENgov.Login.CheckLogin.errore: ", ex)
            sScript += "GestAlert('a', 'warning', '', '', 'Errore in autenticazione, riprovare più tardi.');"
            lblMessage.Text = "Errore in autenticazione, riprovare più tardi."
        End Try
        sScript += "</script>"
        Return sScript
    End Function
    ''' <summary>
    ''' Funzione che verifica se la password rispetta i criteri di formattazione
    ''' </summary>
    ''' <param name="myPwd">stringa testo da verificare</param>
    ''' <returns></returns>
    Public Function PasswordValidator(myPwd As String) As Boolean
        Dim RetVal As Boolean = True
        Dim minLength As Integer = 8
        Dim numSpecial As Integer = 1
        Try
            ' Replace [A-Z] with \p{Lu}, to allow for Unicode uppercase letters.
            Dim upper As New System.Text.RegularExpressions.Regex("[A-Z]")
            Dim lower As New System.Text.RegularExpressions.Regex("[a-z]")
            Dim number As New System.Text.RegularExpressions.Regex("[0-9]")
            ' Special is "none of the above".
            Dim special As New System.Text.RegularExpressions.Regex("[^a-zA-Z0-9]")

            ' Check the length.
            If Len(myPwd) < minLength Then RetVal = False
            ' Check for minimum number of occurrences.
            If upper.Matches(myPwd).Count < 1 Then RetVal = False
            If lower.Matches(myPwd).Count < 1 Then RetVal = False
            If number.Matches(myPwd).Count < 1 Then RetVal = False
            If special.Matches(myPwd).Count < numSpecial Then RetVal = False

            ' Passed all checks.
        Catch ex As Exception
            RetVal = False
        End Try
        Return RetVal
    End Function
    Public Function PasswordIsInHistory(myUser As String, myPwd As String) As Boolean
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Dim IsInHistory As Boolean = False

        Try
            'Valorizzo la connessione
            Dim oDbManagerRepository As New DBModel(COSTANTValue.ConstSession.DBType, COSTANTValue.ConstSession.StringConnectionOPENgov)
            Using ctx As DBModel = oDbManagerRepository
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GESTIONE_UTENTI_S", "LOGNAME")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("LOGNAME", myUser))
                If dvMyDati.Table.Rows.Count() = 1 Then
                    If Not IsDBNull(dvMyDati(0)("password")) Then
                        If VerifyHashedPassword(dvMyDati(0)("password"), myPwd) = False Then
                            IsInHistory = True
                        End If
                    End If
                End If
                ctx.Dispose()
            End Using
        Catch ex As Exception
            Log.Debug("OPENgov.UtilityOPENgov.PasswordIsInHistory.errore: ", ex)
            IsInHistory = True
        End Try
        Return IsInHistory
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Path"></param>
    ''' <param name="dsEnti"></param>
    ''' <returns></returns>
    Public Function CaricaPagina(ByVal Path As String, ByVal dsEnti As DataSet) As String
        Dim sHTML As String = ""
        Try
            If dsEnti.Tables(0).Rows.Count = 0 Then
                sHTML = "Utente non abilitato a nessun ente"
            ElseIf dsEnti.Tables(0).Rows.Count = 1 Then
                HttpContext.Current.Session("COD_ENTE") = dsEnti.Tables(0).Rows(0).Item("COD_ISTAT")
                HttpContext.Current.Session("CODENTE") = dsEnti.Tables(0).Rows(0).Item("COD_ISTAT")
                HttpContext.Current.Session("DESCRIZIONE_ENTE") = dsEnti.Tables(0).Rows(0).Item("DENOMINAZIONE")
                HttpContext.Current.Session("NOME_ENTE") = dsEnti.Tables(0).Rows(0).Item("DESCRIZIONE_ENTE")
                HttpContext.Current.Session("Ambiente") = dsEnti.Tables(0).Rows(0).Item("Ambiente")
                HttpContext.Current.Session("CONTO_CORRENTE") = dsEnti.Tables(0).Rows(0).Item("CONTO_CORRENTE")
                HttpContext.Current.Session("COMUNE_UBICAZIONE_IMMOBILE") = dsEnti.Tables(0).Rows(0).Item("DENOMINAZIONE")
                HttpContext.Current.Session("INTESTAZIONE_BOLLETTINO") = dsEnti.Tables(0).Rows(0).Item("INTESTAZIONE_BOLLETTINO")

                HttpContext.Current.Session("COD_BELFIORE") = dsEnti.Tables(0).Rows(0).Item("COD_BELFIORE")

                'variabili per il 290
                If Not IsDBNull(dsEnti.Tables(0).Rows(0).Item("idente_credben")) Then
                    HttpContext.Current.Session("idente_credben") = dsEnti.Tables(0).Rows(0).Item("idente_credben")
                Else
                    HttpContext.Current.Session("idente_credben") = ""
                End If
                If Not IsDBNull(dsEnti.Tables(0).Rows(0).Item("cod_ente_cnc")) Then
                    HttpContext.Current.Session("idente_CNC") = dsEnti.Tables(0).Rows(0).Item("cod_ente_cnc")
                Else
                    HttpContext.Current.Session("idente_CNC") = ""
                End If
                '*** 20130228 - gestione categoria Ateco per TARES ***
                If Not IsDBNull(dsEnti.Tables(0).Rows(0).Item("fk_IdTypeAteco")) Then
                    HttpContext.Current.Session("IdTypeAteco") = dsEnti.Tables(0).Rows(0).Item("fk_IdTypeAteco")
                Else
                    HttpContext.Current.Session("IdTypeAteco") = "2"
                End If
                '*** ***
                '*** 20140923 - GIS ***
                If Not IsDBNull(dsEnti.Tables(0).Rows(0).Item("HASGIS")) Then
                    HttpContext.Current.Session("VisualGIS") = CBool(dsEnti.Tables(0).Rows(0).Item("HASGIS"))
                End If
                '*** ***
                HttpContext.Current.Session("TributiBollettinoF24") = dsEnti.Tables(0).Rows(0).Item("TributiBollettinoF24")
                Log.Debug("CaricoPagina per::Session('Ambiente')::" & HttpContext.Current.Session("Ambiente").ToString & "::Session('COD_ENTE')::" & HttpContext.Current.Session("COD_ENTE").ToString & "::Session('DESCRIZIONE_ENTE')::" & HttpContext.Current.Session("DESCRIZIONE_ENTE").ToString & "::Session('NOME_ENTE')::" & HttpContext.Current.Session("NOME_ENTE").ToString & "::Session('COD_BELFIORE')::" & HttpContext.Current.Session("COD_BELFIORE").ToString & "::Session('VisualGIS')::" & HttpContext.Current.Session("VisualGIS").ToString)

                sHTML += "function apri(){"
                sHTML += "myheight = screen.height - 75;"
                sHTML += "mywidth = screen.width-10;"
                'shtml+= "var finestra=window.open(""" & Path & "/Generali/asp/aspFrameIniziale.aspx?CODENTE=" + Session("COD_ENTE") + """,""openGov"",""toolbar=no,status=yes,left=0,top=0,height =""+myheight+"",width=""+mywidth);"
                'shtml+= "finestra.focus();"
                '*** 201507 - FUNZIONI SOVRACOMUNALI ***
                If COSTANTValue.ConstSession.HasDummyDich Then
                    sHTML += "parent.location.href='../Generali/asp/aspFrameIniziale.aspx';"
                Else
                    sHTML += "var finestra=window.open(""" & Path & "/Generali/asp/aspFrameIniziale.aspx?CODENTE=" + COSTANTValue.ConstSession.IdEnte + """,""openGov"",""toolbar=no,status=yes,left=0,top=0,height =""+myheight+"",width=""+mywidth);"
                    sHTML += "finestra.focus();"
                End If
                '*** ***
                sHTML += "document.getElementById(""Form1"").style.display=""none"";"
                sHTML += "return false;"
                sHTML += "}"
                sHTML += "apri();"
            ElseIf dsEnti.Tables(0).Rows.Count > 1 Then
                'shtml+= "height = 320"
                'shtml+= "width = 500"
                'shtml+= "myleft = (screen.width - width) / 2"
                'shtml+= "mytop = (screen.height - height) / 2"
                '*** 201507 - FUNZIONI SOVRACOMUNALI ***
                If COSTANTValue.ConstSession.HasDummyDich Then
                    sHTML += "parent.location.href='../Generali/asp/aspFrameIniziale.aspx';"
                Else
                    sHTML += "location.href='../Generali/asp/aspVuotaHome.aspx';"
                    sHTML += "parent.document.getElementById('ifrmEnti').src='seleziona_ambiente.aspx';"
                End If
                '*** ***
                'shtml+= "window.moveTo(myleft, mytop)"
                'shtml+= "window.resizeTo(width, height)"
            Else
                sHTML = "Non gestito"
            End If

            'Dim sHTML As String
            ''sHTML = ""
            ''shtml+= "<script>"

            ''shtml+= "height = screen.height - 25"
            ''shtml+= "width = screen.width"
            ''shtml+= "location.href='" & Path & "/Generali/asp/aspFrameIniziale.aspx'"
            ''shtml+= "window.moveTo(0, 0)"
            ''shtml+= "window.resizeTo(width, height)"
            ''shtml+= "</script>"
            'sHTML = ""
            'shtml+= "<script type=""text/javascript"">"
            'shtml+= "function apri(){"
            'shtml+= "myheight = screen.height - 75;"
            'shtml+= "mywidth = screen.width-10;"
            'shtml+= "var finestra=window.open(""" & Path & "/Generali/asp/aspFrameIniziale.aspx"",""openGov"",""toolbar=no,status=yes,left=0,top=0,height =""+myheight+"",width=""+mywidth);"
            'shtml+= "finestra.focus();"
            'shtml+= "return false;"
            'shtml+= "}"
            'shtml+= "apri()"
            'shtml+= "</script>"

            'CaricaPagina = sHTML
        Catch ex As Exception
            Log.Debug("OPENgov.Login.CaricaPagina.errore: ", ex)
            sHTML = "parent.Visualizza.location.href='../PaginaErrore.aspx;'"
        End Try
        Return sHTML
    End Function
    ''' <summary>
    ''' Funzione che verifica il numero di tentativi di accesso errati per bloccare l'operatore
    ''' </summary>
    ''' <param name="myIP"></param>
    ''' <param name="LoginStatus"></param>
    ''' <returns></returns>
    Private Function CheckLockOut(myIP As String, LoginStatus As Integer) As Boolean
        Dim RetVal As Boolean = False
        Try
            Dim user As Generic.List(Of InvalidLogin) = Nothing
            If (HttpContext.Current.Application("Users") Is Nothing) Then
                user = New Generic.List(Of InvalidLogin)
            Else
                user = CType(HttpContext.Current.Application("Users"), Generic.List(Of InvalidLogin))
            End If
            'Remove IP Before 15 minutes(Give 15 Min Time Next Login)
            For Each myLogin As InvalidLogin In user
                If myLogin.AttemptTime < DateTime.Now.AddMinutes(-15) Then
                    user.Remove(myLogin)
                End If
            Next

            If LoginStatus = SignInStatus.Success Then
                user.RemoveAll(Function(y) y.IP = myIP)
            End If

            Dim checkLogged As New InvalidLogin
            checkLogged = user.Find(Function(x) x.IP = myIP)
            If (checkLogged Is Nothing) Then
                Dim myItem As New InvalidLogin()
                myItem.IP = myIP
                myItem.AttemptTime = DateTime.Now
                myItem.AttemptCount = 1
                user.Add(myItem)
                HttpContext.Current.Application("Users") = user
            ElseIf (checkLogged.AttemptCount < 4) Then
                checkLogged.AttemptTime = DateTime.Now
                checkLogged.AttemptCount = (checkLogged.AttemptCount + 1)
                HttpContext.Current.Application("Users") = user
            End If

            If (Not (checkLogged) Is Nothing) Then
                If (checkLogged.AttemptCount > 3) Then
                    RetVal = True
                End If
            End If
        Catch ex As Exception
            Log.Debug("OPENgov.Login.CheckLockOut.errore: ", ex)
        End Try
        Return RetVal
    End Function
    Public Function LogOff(ByVal myUser As String) As Boolean
        Dim bRet As Boolean = False
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(COSTANTValue.ConstSession.StringConnectionOPENgov)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_SetLogOff"
            cmdMyCommand.Parameters.AddWithValue("@LOGINUSER", myUser)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            cmdMyCommand.ExecuteNonQuery()
            bRet = True
        Catch ex As Exception
            Log.Debug("OPENgov.LogOff.errore: ", ex)
            bRet = False
        Finally
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
        Return bRet
    End Function
    ''' <summary>
    ''' Funzione che verifica l'accesso dello stesso utente da IP diversi
    ''' </summary>
    ''' <param name="myUser">string username</param>
    ''' <param name="myIP">string IP</param>
    ''' <returns>boolean True in caso di accesso da multipli ip altrimenti false</returns>
    Private Function MultipleIPLock(myUser As String, myIP As String) As Boolean
        Dim RetVal As Boolean = False
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dtMyDati As New DataTable()
        Dim dvMyDati As New DataView
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(COSTANTValue.ConstSession.StringConnectionOPENgov)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_MultipleIPLock"
            cmdMyCommand.Parameters.AddWithValue("@LOGINUSER", myUser)
            cmdMyCommand.Parameters.AddWithValue("@MAXMINUTE", HttpContext.Current.Session.Timeout)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.SelectCommand = cmdMyCommand
            myAdapter.Fill(dtMyDati)
            dvMyDati = dtMyDati.DefaultView
            For Each myRow As DataRow In dvMyDati.Table.Rows
                If Not IsDBNull(myRow("LOGINIP")) Then
                    If myIP <> myRow("LOGINIP").ToString Then
                        RetVal = True
                    End If
                End If
            Next
        Catch ex As Exception
            Log.Debug("OPENgov.UtilityOPENgov.MultipleIPLock.errore: ", ex)
            Throw New Exception
        Finally
            myAdapter.Dispose()
            dtMyDati.Dispose()
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
        Return RetVal
    End Function
    ''' <summary>
    ''' Funzione per la memorizzazione dell'IP di accesso
    ''' </summary>
    ''' <param name="myUser">string username</param>
    ''' <param name="myIP">string IP</param>
    ''' <param name="myTime">DateTime momento di accesso</param>
    Private Sub SetLogginIP(myUser As String, myIP As String, myTime As DateTime)
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(COSTANTValue.ConstSession.StringConnectionOPENgov)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_TBLLOGGINIP_IU"
            cmdMyCommand.Parameters.AddWithValue("@LOGINUSER", myUser)
            cmdMyCommand.Parameters.AddWithValue("@LOGINIP", myIP)
            cmdMyCommand.Parameters.AddWithValue("@LOGINTIME", myTime)
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            cmdMyCommand.ExecuteNonQuery()
        Catch ex As Exception
            Log.Debug("OPENgov.UtilityOPENgov.SetLogginIP.errore: ", ex)
            Throw New Exception
        Finally
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
    End Sub
    ''' <summary>
    ''' Funzione per il reset della password.
    ''' La funzione serve anche per l'inserimento di un nuovo account; se non presente inserisce sull'ente in lavorazione.
    ''' </summary>
    ''' <param name="sLogName">string</param>
    ''' <param name="sLogPwd">string</param>
    ''' <returns></returns>
    Public Function ResetPassword(ByVal sLogName As String, ByVal sLogPwd As String) As Boolean
        Dim RetVal As Boolean = False
        Dim myAdapter As New SqlClient.SqlDataAdapter
        Dim dtMyDati As New DataTable()
        Dim dvMyDati As New DataView
        Dim cmdMyCommand As New SqlClient.SqlCommand

        Try
            cmdMyCommand.Connection = New SqlClient.SqlConnection(COSTANTValue.ConstSession.StringConnectionOPENgov)
            cmdMyCommand.Connection.Open()
            cmdMyCommand.CommandTimeout = 0
            cmdMyCommand.CommandType = CommandType.StoredProcedure
            cmdMyCommand.CommandText = "prc_ResetPassword"
            cmdMyCommand.Parameters.AddWithValue("@LOGINNAME", sLogName)
            cmdMyCommand.Parameters.AddWithValue("@LOGINPWD", HashPassword(sLogPwd))
            cmdMyCommand.Parameters.AddWithValue("@IDENTE", COSTANTValue.ConstSession.IdEnte)
            myAdapter.SelectCommand = cmdMyCommand
            Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
            myAdapter.Fill(dtMyDati)
            dvMyDati = dtMyDati.DefaultView
            If dvMyDati.Table.Rows.Count() = 1 Then
                If Not IsDBNull(dvMyDati(0)("id")) Then
                    If CInt(dvMyDati(0)("id")) > 0 Then
                        RetVal = True
                    End If
                End If
            End If
        Catch ex As Exception
            Log.Debug("OPENgov.UtilityOPENgov.ResetPassword.errore: ", ex)
        Finally
            myAdapter.Dispose()
            dtMyDati.Dispose()
            cmdMyCommand.Dispose()
            cmdMyCommand.Connection.Close()
        End Try
        Return RetVal
    End Function
    'Public Function ResetPassword(ByVal sLogName As String, ByVal sLogPwd As String) As Boolean
    '    Dim RetVal As Boolean = False
    '    Dim myAdapter As New SqlClient.SqlDataAdapter
    '    Dim dtMyDati As New DataTable()
    '    Dim dvMyDati As New DataView
    '    Dim cmdMyCommand As New SqlClient.SqlCommand

    '    Try
    '        cmdMyCommand.Connection = New SqlClient.SqlConnection(COSTANTValue.ConstSession.StringConnectionOPENgov)
    '        cmdMyCommand.Connection.Open()
    '        cmdMyCommand.CommandTimeout = 0
    '        cmdMyCommand.CommandType = CommandType.StoredProcedure
    '        cmdMyCommand.CommandText = "prc_ResetPassword"
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@LOGINNAME", SqlDbType.NVarChar)).Value = sLogName
    '        cmdMyCommand.Parameters.Add(New SqlClient.SqlParameter("@LOGINPWD", SqlDbType.NVarChar)).Value = HashPassword(sLogPwd)
    '        myAdapter.SelectCommand = cmdMyCommand
    '        Log.Debug(Utility.Costanti.LogQuery(cmdMyCommand))
    '        myAdapter.Fill(dtMyDati)
    '        dvMyDati = dtMyDati.DefaultView
    '        If dvMyDati.Table.Rows.Count() = 1 Then
    '            If Not IsDBNull(dvMyDati(0)("id")) Then
    '                If CInt(dvMyDati(0)("id")) > 0 Then
    '                    RetVal = True
    '                End If
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Log.Debug("OPENgov.UtilityOPENgov.ResetPassword.errore: ", ex)
    '    Finally
    '        myAdapter.Dispose()
    '        dtMyDati.Dispose()
    '        cmdMyCommand.Dispose()
    '        cmdMyCommand.Connection.Close()
    '    End Try
    '    Return RetVal
    'End Function
    '*** ***
    ''' <summary>
    ''' Funzione che controlla che username e password siano validi
    ''' </summary>
    ''' <param name="sLogName">string username</param>
    ''' <param name="sLogPwd">string password</param>
    ''' <returns></returns>
    ''' <revisionHistory>
    ''' <revision date="11/2015">
    ''' Funzioni Sovracomunali
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="01/2018">
    ''' menù in base a utente
    ''' </revision>
    ''' </revisionHistory>
    ''' <revisionHistory>
    ''' <revision date="12/04/2019">
    ''' Modifiche da revisione manuale
    ''' </revision>
    ''' </revisionHistory>
    Public Function IsValidUser(ByVal sLogName As String, ByVal sLogPwd As String) As Integer
        Dim sSQL As String = ""
        Dim dvMyDati As New DataView
        Dim mySignInStatus As Integer = UtilityOPENgov.SignInStatus.RequiresVerification

        Try
            'Valorizzo la connessione
            Dim oDbManagerRepository As New DBModel(COSTANTValue.ConstSession.DBType, COSTANTValue.ConstSession.StringConnectionOPENgov)
            Using ctx As DBModel = oDbManagerRepository
                sSQL = ctx.GetSQL(DBModel.TypeQuery.StoredProcedure, "prc_GESTIONE_UTENTI_S", "LOGNAME")
                dvMyDati = ctx.GetDataView(sSQL, "TBL", ctx.GetParam("LOGNAME", sLogName))
                If dvMyDati.Table.Rows.Count() = 1 Then
                    If Not IsDBNull(dvMyDati(0)("password")) Then
                        If VerifyHashedPassword(dvMyDati(0)("password"), sLogPwd) = False Then
                            mySignInStatus = SignInStatus.FailurePwd
                        Else
                            mySignInStatus = SignInStatus.Success
                        End If
                    End If
                    HttpContext.Current.Session("dirittioperatore") = Trim(dvMyDati(0)("dirittioperatore").ToString)
                    HttpContext.Current.Session("SOLA_LETTURA") = "0"
                    If Not IsDBNull(dvMyDati(0)("profilo")) Then
                        If (Trim(dvMyDati(0)("profilo").ToString) = COSTANTValue.ConstSession.TipoProfilo.SolaLettura) Then
                            HttpContext.Current.Session("SOLA_LETTURA") = "1"
                        End If
                        HttpContext.Current.Session("profilo") = dvMyDati(0)("profilo")
                    End If
                    HttpContext.Current.Session("username") = sLogName
                    HttpContext.Current.Session("Ambiente") = dvMyDati(0)("Ambiente")
                    HttpContext.Current.Session("APPLICATIONS_ENABLED") = dvMyDati(0)("APPLICATIONS_ENABLED")
                End If
                ctx.Dispose()
            End Using
            Return mySignInStatus
        Catch ex As Exception
            Log.Debug("OPENgov.UtilityOPENgov.IsValidUser.errore: ", ex)
            Throw New Exception
        End Try
    End Function
    ''' <summary>
    ''' Creating a hash
    ''' The salt Is randomly generated Using the Function Rfc2898DeriveBytes which generates a hash And a salt. Inputs To Rfc2898DeriveBytes are the password, the size Of the salt To generate And the number Of hashing iterations To perform. https://msdn.microsoft.com/en-us/library/h83s4e12(v=vs.110).aspx
    ''' The salt And the hash are Then mashed together(salt first followed by the hash) And encoded As a String (so the salt Is encoded In the hash). This encoded hash (which contains the salt And hash) Is Then stored (typically) In the database against the user.
    ''' Checking a password against a hash
    ''' To check a password that a user inputs.
    ''' The salt Is extracted from the stored hashed password.
    ''' The salt Is used To hash the users input password Using an overload Of Rfc2898DeriveBytes which takes a salt instead Of generating one. https://msdn.microsoft.com/en-us/library/yx129kfs(v=vs.110).aspx
    ''' The stored hash And the test hash are Then compared.
    ''' The Hash
    ''' Under the covers the hash Is generated Using the SHA1 hash Function (https://en.wikipedia.org/wiki/SHA-1). This function Is iteratively called 1000 times (In the default Identity implementation)
    ''' Why Is this secure
    ''' Random salts means that an attacker can't use a pre-generated table of hashs to try and break passwords. They would need to generate a hash table for every salt. (Assuming here that the hacker has also compromised your salt)
    ''' If 2 passwords are identical they will have different hashes. (meaning attackers can't infer ‘common’ passwords)
    ''' Iteratively calling SHA1 1000 times means that the attacker also needs To Do this. The idea being that unless they have time On a supercomputer they won't have enough resource to brute force the password from the hash. It would massively slow down the time to generate a hash table for a given salt.
    ''' </summary>
    ''' <param name="password">string password da crifrare</param>
    ''' <returns></returns>
    Private Function HashPassword(ByVal password As String) As String
        Dim salt() As Byte
        Dim buffer2() As Byte
        Try
            If (password Is Nothing) Then
                Throw New ArgumentNullException("password")
            End If

            Dim bytes As Rfc2898DeriveBytes = New Rfc2898DeriveBytes(password, 16, 1000)
            salt = bytes.Salt
            buffer2 = bytes.GetBytes(32)
            Dim dst() As Byte = New Byte((49) - 1) {}
            Buffer.BlockCopy(salt, 0, dst, 1, 16)
            Buffer.BlockCopy(buffer2, 0, dst, 17, 32)
            Return Convert.ToBase64String(dst)
        Catch ex As Exception
            Log.Debug("OPENgov.Login.HashPassword.errore: ", ex)
            Return ""
        End Try
    End Function
    ''' <summary>
    ''' To verify the hash the output is split back to the salt and the rest, and the KDF is run again on the password with the specified salt. If the result matches to the rest of the initial output the hash is verified.
    ''' </summary>
    ''' <param name="hashedPassword">string Password salvata sul db</param>
    ''' <param name="password">string password da verificare</param>
    ''' <returns>boolean True quando la password è corretta altrimenti false</returns>
    Private Function VerifyHashedPassword(ByVal hashedPassword As String, ByVal password As String) As Boolean
        Dim buffer4() As Byte
        Try
            If (hashedPassword Is Nothing) Then
                Return False
            End If

            If (password Is Nothing) Then
                Throw New ArgumentNullException("password")
            End If

            Dim src() As Byte = Convert.FromBase64String(hashedPassword)
            If ((src.Length <> 49) Or (src(0) <> 0)) Then
                Return False
            End If

            Dim dst() As Byte = New Byte((16) - 1) {}
            Buffer.BlockCopy(src, 1, dst, 0, 16)
            Dim buffer3() As Byte = New Byte((32) - 1) {}
            Buffer.BlockCopy(src, 17, buffer3, 0, 32)
            Dim bytes As Rfc2898DeriveBytes = New Rfc2898DeriveBytes(password, dst, 1000)
            buffer4 = bytes.GetBytes(32)
            Return ByteArraysEqual(buffer3, buffer4)
        Catch ex As Exception
            Log.Debug("OPENgov.Login.VerifyHashedPassword.errore: ", ex)
            Return False
        End Try
    End Function
    Private Function ByteArraysEqual(bytes1() As Byte, bytes2() As Byte) As Boolean
        Try
            If (bytes1.Equals(bytes2)) Then
                Return True
            End If

            If ((Not bytes1 Is Nothing) And (Not bytes2 Is Nothing)) Then
                If (bytes1.Length <> bytes2.Length) Then
                    Return False
                End If

                For x As Integer = 0 To bytes1.GetUpperBound(0)
                    If bytes1(x) <> bytes2(x) Then
                        Return False
                    End If
                Next
                Return True
            End If
        Catch ex As Exception
            Log.Debug("OPENgov.Login.ByteArraysEqual.errore: ", ex)
            Return False
        End Try
    End Function
#End Region
#Region "DropDown"
    ''' <summary>
    ''' Il metodo SelectIndexDropDownList consente di selezionare l'id di un elemento di un controllo DropDownList salvato nel database
    ''' </summary>
    ''' <param name="cboTemp"></param>
    ''' <param name="strValue"></param>
    Private Sub SelectIndexDropDownList(ByVal cboTemp As DropDownList, ByVal strValue As String)

        Dim blnFindElement As Boolean = False
        Dim intCount As Integer = 1
        Dim intNumberElements As Integer = cboTemp.Items.Count
        Try
            Do While intCount < intNumberElements
                cboTemp.SelectedIndex = intCount
                If cboTemp.SelectedItem.Value = strValue Then
                    cboTemp.SelectedItem.Text = cboTemp.Items(intCount).Text
                    blnFindElement = True
                    Exit Do
                End If
                intCount = intCount + 1
            Loop

            If Not blnFindElement Then cboTemp.SelectedIndex = "-1"
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte + "." + COSTANTValue.ConstSession.UserName + " - OPENgov.UtilityOPENgov.SelectIndexDropDownList.errore: ", ex)
        End Try
    End Sub
    ''' <summary>
    ''' Il metodo FillDropDownSQL consente di caricre un controllo DropDownList da un data reader caricato dal database
    ''' </summary>
    ''' <param name="cboTemp"></param>
    ''' <param name="dr"></param>
    ''' <param name="lngSelectedID"></param>
    ''' <param name="strTesto"></param>
    Public Sub FillDropDownSQL(ByVal cboTemp As DropDownList, ByVal dr As SqlDataReader, ByVal lngSelectedID As Long, ByVal strTesto As String)
        Try

            Dim myListItem As ListItem
            myListItem = New ListItem


            myListItem.Text = strTesto
            myListItem.Value = "-1"
            cboTemp.Items.Add(myListItem)
            While dr.Read()
                myListItem = New ListItem

                myListItem.Text = dr.GetString(1)
                myListItem.Value = dr.GetInt32(0).ToString

                cboTemp.Items.Add(myListItem)
            End While

            If lngSelectedID <> Constant.INIT_VALUE_NUMBER Then
                SelectIndexDropDownList(cboTemp, CStr(lngSelectedID))
            End If
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte + "." + COSTANTValue.ConstSession.UserName + " - OPENgov.UtilityOPENgov.FillDropDownSQL.errore: ", Err)
            Throw New Exception("Class Utility::Sub FillDropDownSQL::" & Err.Message)
        Finally
            If Not dr Is Nothing Then
                dr.Close()
            End If
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="cboTemp"></param>
    ''' <param name="dr"></param>
    ''' <param name="strSelectedID"></param>
    ''' <param name="strTesto"></param>
    Public Sub FillDropDownSQLValueString(ByVal cboTemp As DropDownList, ByVal dr As SqlDataReader, ByVal strSelectedID As String, ByVal strTesto As String)
        Try

            Dim myListItem As ListItem
            myListItem = New ListItem

            myListItem.Text = strTesto
            myListItem.Value = "-1"
            cboTemp.Items.Add(myListItem)
            While dr.Read()
                myListItem = New ListItem

                myListItem.Text = dr.GetString(1)
                myListItem.Value = dr.GetString(0)

                cboTemp.Items.Add(myListItem)
            End While

            If strSelectedID <> CStr(Constant.INIT_VALUE_NUMBER) Then
                SelectIndexDropDownList(cboTemp, strSelectedID)
            End If
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte + "." + COSTANTValue.ConstSession.UserName + " - OPENgov.UtilityOPENgov.FillDropDownSQLValueString.errore: ", Err)
            Throw New Exception("Class Utility::Sub FillDropDownSQL::" & Err.Message)
        Finally
            If Not dr Is Nothing Then
                dr.Close()
            End If
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="cboTemp"></param>
    ''' <param name="ds"></param>
    ''' <param name="strSelectedID"></param>
    ''' <param name="strTesto"></param>
    Public Sub FillDropDownSQLValueString(ByVal cboTemp As DropDownList, ByVal ds As DataSet, ByVal strSelectedID As String, ByVal strTesto As String)
        Try
            Dim myListItem As ListItem
            myListItem = New ListItem
            Dim iCount As Integer

            myListItem.Text = strTesto
            myListItem.Value = "-1"
            cboTemp.Items.Add(myListItem)

            For iCount = 0 To ds.Tables(0).Rows.Count - 1
                myListItem = New ListItem

                myListItem.Text = ds.Tables(0).Rows(iCount).Item(1).ToString
                myListItem.Value = ds.Tables(0).Rows(iCount).Item(0).ToString

                cboTemp.Items.Add(myListItem)
            Next

            If strSelectedID <> CStr(Constant.INIT_VALUE_NUMBER) Then
                SelectIndexDropDownList(cboTemp, strSelectedID)
            End If
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte + "." + COSTANTValue.ConstSession.UserName + " - OPENgov.UtilityOPENgov.FillDropDownSQLValueString.errore: ", Err)
            Throw New Exception("Class Utility::Sub FillDropDownSQL::" & Err.Message)
        Finally
            If Not ds Is Nothing Then
                ds.Dispose()
            End If
        End Try
    End Sub
    ''' <summary>
    ''' '
    ''' </summary>
    ''' <param name="cboTemp"></param>
    ''' <param name="dr"></param>
    ''' <param name="strSelectedID"></param>
    ''' <param name="strTesto"></param>
    Public Sub FillDropDownSQLSingleString(ByVal cboTemp As DropDownList, ByVal dr As SqlDataReader, ByVal strSelectedID As String, ByVal strTesto As String)
        Try
            Dim myListItem As ListItem
            myListItem = New ListItem

            myListItem.Text = strTesto
            myListItem.Value = "-1"
            cboTemp.Items.Add(myListItem)
            While dr.Read()
                myListItem = New ListItem

                myListItem.Text = dr.GetString(0)
                myListItem.Value = dr.GetString(0)

                cboTemp.Items.Add(myListItem)
            End While

            If strSelectedID <> CStr(Constant.INIT_VALUE_NUMBER) Then
                SelectIndexDropDownList(cboTemp, strSelectedID)
            End If
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte + "." + COSTANTValue.ConstSession.UserName + " - OPENgov.UtilityOPENgov.FillDropDownSQLSingleString.errore: ", Err)
            Throw New Exception("Class Utility::Sub FillDropDownSQL::" & Err.Message)
        Finally
            If Not dr Is Nothing Then
                dr.Close()
            End If
        End Try
    End Sub
    ''' <summary>
    ''' '
    ''' </summary>
    ''' <param name="cboTemp"></param>
    ''' <param name="ds"></param>
    ''' <param name="strSelectedID"></param>
    ''' <param name="strTesto"></param>
    Public Sub FillDropDownSQLSingleString(ByVal cboTemp As DropDownList, ByVal ds As DataSet, ByVal strSelectedID As String, ByVal strTesto As String)
        Try
            Dim myListItem As ListItem
            myListItem = New ListItem
            Dim iCount As Integer

            myListItem.Text = strTesto
            myListItem.Value = "-1"
            cboTemp.Items.Add(myListItem)

            For iCount = 0 To ds.Tables(0).Rows.Count - 1
                myListItem = New ListItem
                myListItem.Text = ds.Tables(0).Rows(iCount).Item(0).ToString
                myListItem.Value = ds.Tables(0).Rows(iCount).Item(0).ToString

                cboTemp.Items.Add(myListItem)
            Next

            If strSelectedID <> CStr(Constant.INIT_VALUE_NUMBER) Then
                SelectIndexDropDownList(cboTemp, strSelectedID)
            End If
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte + "." + COSTANTValue.ConstSession.UserName + " - OPENgov.UtilityOPENgov.FillDropDownSQLSingleString.errore: ", Err)
            Throw New Exception("Class Utility::Sub FillDropDownSQLSingleString::" & Err.Message)
        Finally
            If Not ds Is Nothing Then
                ds.Dispose()
            End If
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="cboTemp"></param>
    ''' <param name="ds"></param>
    ''' <param name="lngSelectedID"></param>
    ''' <param name="strTesto"></param>
    Public Sub FillDropDownSQLValueStringCodDesc(ByVal cboTemp As DropDownList, ByVal ds As DataSet, ByVal lngSelectedID As String, ByVal strTesto As String)
        Try
            Dim myListItem As ListItem
            myListItem = New ListItem
            Dim iCount As Integer


            myListItem.Text = strTesto
            myListItem.Value = "-1"
            cboTemp.Items.Add(myListItem)
            For iCount = 0 To ds.Tables(0).Rows.Count - 1
                myListItem = New ListItem

                myListItem.Text = ds.Tables(0).Rows(iCount).Item(0).ToString & " - " & ds.Tables(0).Rows(iCount).Item(1).ToString
                myListItem.Value = ds.Tables(0).Rows(iCount).Item(0).ToString
                myListItem.Attributes.Add("title", ds.Tables(0).Rows(iCount).Item(0).ToString & " - " & ds.Tables(0).Rows(iCount).Item(1).ToString)

                cboTemp.Items.Add(myListItem)
            Next

            If lngSelectedID <> CStr(Constant.INIT_VALUE_NUMBER) Then
                SelectIndexDropDownList(cboTemp, CStr(lngSelectedID))
            End If
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte + "." + COSTANTValue.ConstSession.UserName + " - OPENgov.UtilityOPENgov.FillDropDownSQLValueStringCodDesc.errore: ", Err)
            Throw New Exception("Class Utility::Sub FillDropDownSQLValueStringCodDesc::" & Err.Message)
        Finally
            If Not ds Is Nothing Then
                ds.Dispose()
            End If
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="cboTemp"></param>
    ''' <param name="dr"></param>
    ''' <param name="lngSelectedID"></param>
    ''' <param name="strTesto"></param>
    Public Sub FillDropDownSQLValueStringCodDesc(ByVal cboTemp As DropDownList, ByVal dr As SqlDataReader, ByVal lngSelectedID As String, ByVal strTesto As String)
        Try
            Dim myListItem As ListItem
            myListItem = New ListItem

            myListItem.Text = strTesto
            myListItem.Value = "-1"
            cboTemp.Items.Add(myListItem)
            While dr.Read()
                myListItem = New ListItem

                myListItem.Text = dr.GetString(0) & " - " & dr.GetString(1)
                myListItem.Value = dr.GetString(0)
                myListItem.Attributes.Add("title", dr.GetString(0) & " - " & dr.GetString(1))

                cboTemp.Items.Add(myListItem)
            End While

            If lngSelectedID <> CStr(Constant.INIT_VALUE_NUMBER) Then
                SelectIndexDropDownList(cboTemp, CStr(lngSelectedID))
            End If
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte + "." + COSTANTValue.ConstSession.UserName + " - OPENgov.UtilityOPENgov.FillDropDownSQLValueStringCodDesc.errore: ", Err)
            Throw New Exception("Class Utility::Sub FillDropDownSQL::" & Err.Message)
        Finally
            If Not dr Is Nothing Then
                dr.Close()
            End If
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="cboTemp"></param>
    ''' <param name="dr"></param>
    ''' <param name="lngSelectedID"></param>
    ''' <param name="strTesto"></param>
    Public Sub FillDropDownSQLString(ByVal cboTemp As DropDownList, ByVal dr As SqlDataReader, ByVal lngSelectedID As Long, ByVal strTesto As String)
        Try

            Dim myListItem As ListItem
            myListItem = New ListItem

            myListItem.Text = strTesto
            myListItem.Value = "-1"
            cboTemp.Items.Add(myListItem)
            While dr.Read()
                myListItem = New ListItem

                myListItem.Text = dr.GetString(0)
                myListItem.Value = dr.GetString(0)
                cboTemp.Items.Add(myListItem)
            End While

            If lngSelectedID <> Constant.INIT_VALUE_NUMBER Then
                SelectIndexDropDownList(cboTemp, CStr(lngSelectedID))
            End If
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte + "." + COSTANTValue.ConstSession.UserName + " - OPENgov.UtilityOPENgov.FillDropDownSQLString.errore: ", Err)
            Throw New Exception("Class Utility::Sub FillDropDownSQL::" & Err.Message)
        Finally
            If Not dr Is Nothing Then
                dr.Close()
            End If
        End Try
    End Sub
#End Region
#Region "Funzioni di conversione"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="vInput"></param>
    ''' <param name="blnClearSpace"></param>
    ''' <returns></returns>
    Public Function CStrToDB(ByVal vInput As Object, Optional ByRef blnClearSpace As Boolean = False) As String
        Dim sTesto As String

        CStrToDB = "''"
        Try
            If Not IsDBNull(vInput) And Not IsNothing(vInput) Then

                sTesto = CStr(vInput)
                If blnClearSpace Then
                    sTesto = Trim(sTesto)
                End If
                If Trim(sTesto) <> "" Then
                    CStrToDB = "'" & Replace(sTesto, "'", "''") & "'"
                End If
            End If
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte + "." + COSTANTValue.ConstSession.UserName + " - OPENgov.UtilityOPENgov.CStrToDB.errore: ", Err)
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="vInput"></param>
    ''' <returns></returns>
    Public Function CStrToDBForIn(ByVal vInput As String) As String
        Dim sTesto As String = ""
        Dim i As Integer

        CStrToDBForIn = "''"
        Dim arrayIn() As String
        arrayIn = Split(vInput, ",")
        Try
            For i = -1 To UBound(arrayIn) - 1
                sTesto = sTesto & "'" & arrayIn(i + 1) & "',"
            Next

            sTesto = Left(sTesto, Len(sTesto) - 1)

            Return sTesto
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte + "." + COSTANTValue.ConstSession.UserName + " - OPENgov.UtilityOPENgov.CStrToDBForIn.errore: ", Err)
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strInput"></param>
    ''' <returns></returns>
    Public Function CToStr(ByRef strInput As Object) As String
        CToStr = ""
        Try
            If Not IsDBNull(strInput) And Not IsNothing(strInput) Then
                CToStr = CStr(strInput)
            End If

            Return CToStr
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte + "." + COSTANTValue.ConstSession.UserName + " - OPENgov.UtilityOPENgov.CToStr.errore: ", Err)
        End Try
    End Function
    ''' <summary>
    ''' leggo la data nel formato gg/mm/aaaa e la metto nel formato aaaammgg
    ''' </summary>
    ''' <param name="data"></param>
    ''' <param name="divisore"></param>
    ''' <returns></returns>
    Public Function GiraData(ByVal data As String, Optional ByVal divisore As String = "") As String
        Dim Giorno As String
        Dim Mese As String
        Dim Anno As String
        Try
            If data <> "" Then
                Giorno = Mid(data, 1, 2)
                Mese = Mid(data, 4, 2)
                Anno = Mid(data, 7, 4)
                GiraData = Anno & divisore & Mese & divisore & Giorno
            Else
                GiraData = ""
            End If
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte + "." + COSTANTValue.ConstSession.UserName + " - OPENgov.UtilityOPENgov.GiraData.errore: ", Err)
            Return ""
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="data"></param>
    ''' <param name="sTypeFormat"><list type=""><item>G=leggo la data nel formato aaaammgg  e la metto nel formato gg/mm/aaaa</item><item>A=leggo la data nel formato gg/mm/aaaa  e la metto nel formato aaaammgg</item></list></param>
    ''' <returns></returns>
    Public Function GiraDataFromDB(ByVal data As String, ByVal sTypeFormat As String) As String
        Dim Giorno As String
        Dim Mese As String
        Dim Anno As String
        Try
            If data <> "" Then
                Select Case sTypeFormat
                    Case "G"
                        Giorno = Mid(data, 7, 2)
                        Mese = Mid(data, 5, 2)
                        Anno = Mid(data, 1, 4)
                        GiraDataFromDB = Giorno & "/" & Mese & "/" & Anno
                    Case "A"
                        Giorno = Mid(data, 1, 2)
                        Mese = Mid(data, 4, 2)
                        Anno = Mid(data, 7, 4)
                        GiraDataFromDB = Anno & Mese & Giorno
                    Case Else
                        GiraDataFromDB = ""
                End Select
            Else
                GiraDataFromDB = ""
            End If
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte + "." + COSTANTValue.ConstSession.UserName + " - OPENgov.UtilityOPENgov.GiraDataFromDB.errore: ", Err)
            Return ""
        End Try
    End Function
    ''' <summary>
    ''' leggo la data nel formato aaaa/mm/gg  e la metto nel formato gg/mm/aaaa
    ''' </summary>
    ''' <param name="data"></param>
    ''' <returns></returns>
    Public Function GiraDataCompletaFromDB(ByVal data As String) As String
        Dim Giorno As String
        Dim Mese As String
        Dim Anno As String
        Try
            If data <> "" Then
                Giorno = Mid(data, 9, 2)
                Mese = Mid(data, 6, 2)
                Anno = Mid(data, 1, 4)
                GiraDataCompletaFromDB = Giorno & "/" & Mese & "/" & Anno
            Else
                GiraDataCompletaFromDB = ""
            End If
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte + "." + COSTANTValue.ConstSession.UserName + " - OPENgov.UtilityOPENgov.GiraDataCompletaFromDB.errore: ", Err)
            Return ""
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="objInput"></param>
    ''' <returns></returns>
    Public Function cToDbl(ByRef objInput As Object) As Double
        cToDbl = 0
        Try
            If Not IsDBNull(objInput) And Not IsNothing(objInput) Then
                If IsNumeric(objInput) Then
                    cToDbl = CDbl(objInput)
                End If
            End If
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte + "." + COSTANTValue.ConstSession.UserName + " - OPENgov.UtilityOPENgov.CToDbl.errore: ", Err)
        End Try
    End Function
#End Region
End Class
''' <summary>
''' Classe per la gestione delle funzioni di formattazione per le griglie
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class FunctionGrd
    Private Shared Log As ILog = LogManager.GetLogger(GetType(FunctionGrd))
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="tDataGrd"></param>
    ''' <returns></returns>
    Public Function FormattaDataGrd(ByVal tDataGrd As DateTime) As String
        Try
            If tDataGrd = Date.MinValue Or tDataGrd = Date.MaxValue Or tDataGrd.ToShortDateString = DateTime.MaxValue.ToShortDateString Or tDataGrd = DateTime.MaxValue.ToShortDateString Then
                Return ""
            Else
                Return tDataGrd.ToShortDateString
            End If
        Catch Err As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte + "." + COSTANTValue.ConstSession.UserName + " - OPENgov.FunctionGrd.FormattaDataGrd.errore: ", Err)
            Return ""
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nID"></param>
    ''' <returns></returns>
    Public Function FormattaCC(ByVal nID As Integer) As String
        Try
            If nID <= 0 Then
                Return "..\..\images\Bottoni\nuovoinserisci.png"
            Else
                Return "..\..\images\Bottoni\apri1.png"
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte + "." + COSTANTValue.ConstSession.UserName + " - OPENgov.FunctionGrd.FormattaCC.errore: ", ex)
            Return ""
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nIsSubContatore"></param>
    ''' <returns></returns>
    Public Function FormattaIsSubContatore(ByVal nIsSubContatore As Object) As String
        Try
            If Not IsDBNull(nIsSubContatore) Then
                If CInt(nIsSubContatore) > 0 Then
                    Return "..\..\images\Bottoni\visto.png"
                Else
                    Return "..\..\images\Bottoni\trasparente.png"
                End If
            Else
                Return "..\..\images\Bottoni\trasparente.png"
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte + "." + COSTANTValue.ConstSession.UserName + " - OPENgov.FunctionGrd.FormattalsSubContatore.errore: ", ex)
            Return "..\..\images\Bottoni\trasparente.png"
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sMatricolaPrincipale"></param>
    ''' <returns></returns>
    Public Function FormattaToolTipSubContatore(ByVal sMatricolaPrincipale As Object) As String
        Try
            If Not IsDBNull(sMatricolaPrincipale) Then
                If sMatricolaPrincipale.ToString <> "" Then
                    Return "Contatore Principale " & sMatricolaPrincipale.ToString
                Else
                    Return ""
                End If
            Else
                Return ""
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte + "." + COSTANTValue.ConstSession.UserName + " - OPENgov.FunctionGrd.FormattaToolTipSubContatore.errore: ", ex)
            Return ""
        End Try
    End Function
    ''' <summary>
    ''' Funzione che concatena cognome e nome
    ''' </summary>
    ''' <param name="Cognome"></param>
    ''' <param name="Nome"></param>
    ''' <returns>string risultante dalla concatenzazione</returns>
    Public Function FormattaNominativo(ByVal Cognome As Object, ByVal Nome As Object) As String
        Dim ret As String = ""
        Try
            If Not IsDBNull(Cognome) Then
                ret += Cognome.ToString
            End If
            If Not IsDBNull(Nome) Then
                ret += " " + Nome.ToString
            End If
        Catch ex As Exception
            Log.Debug("Anagrafica.FunctionGrd.FormattaNominativo.errore: ", ex)
            ret = ""
        End Try
        Return ret.Trim
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Via"></param>
    ''' <param name="Civico"></param>
    ''' <param name="Posizione"></param>
    ''' <param name="Esponente"></param>
    ''' <param name="Scala"></param>
    ''' <param name="Interno"></param>
    ''' <param name="Frazione"></param>
    ''' <returns></returns>
    Public Function FormattaVia(ByVal Via As Object, ByVal Civico As Object, ByVal Posizione As Object, ByVal Esponente As Object, ByVal Scala As Object, ByVal Interno As Object, ByVal Frazione As Object) As String
        Dim ret As String = String.Empty

        Try
            If Not IsDBNull(Via) Then
                ret += Via.ToString
            End If
            If Not IsDBNull(Civico) Then
                If CStr(Civico) <> "0" And CStr(Civico) <> "-1" Then
                    ret += " " + CStr(Civico)
                End If
            End If
            If Not IsDBNull(Posizione) Then
                If CStr(Posizione) <> "" Then
                    ret += " " + CStr(Posizione)
                End If
            End If
            If Not IsDBNull(Esponente) Then
                If CStr(Esponente) <> "" Then
                    ret += " " + CStr(Esponente)
                End If
            End If
            If Not IsDBNull(Scala) Then
                If CStr(Scala) <> "" Then
                    ret += " Sc." + CStr(Scala)
                End If
            End If
            If Not IsDBNull(Interno) Then
                If CStr(Interno) <> "" Then
                    ret += " Int." + CStr(Interno)
                End If
            End If
            If Not IsDBNull(Frazione) Then
                If CStr(Frazione) <> "" Then
                    ret += " Fraz." + Frazione.ToString
                End If
            End If
        Catch ex As Exception
            Log.Debug("Anagrafica.FunctionGrd.FormattaVia.errore: ", ex)
            ret = ""
        End Try
        Return ret.Trim
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="CAP"></param>
    ''' <param name="Comune"></param>
    ''' <param name="Prov"></param>
    ''' <returns></returns>
    Public Function FormattaComune(ByVal CAP As Object, ByVal Comune As Object, ByVal Prov As Object) As String
        Dim ret As String = ""
        Try
            If Not IsDBNull(CAP) Then
                If CStr(CAP) <> "" Then
                    ret += CAP.ToString
                End If
            End If
            If Not IsDBNull(Comune) Then
                If CStr(Comune) <> "" Then
                    ret += " " + Comune.ToString
                End If
            End If
            If Not IsDBNull(Prov) Then
                If CStr(Prov) <> "" Then
                    ret += " (" + Prov.ToString + ")"
                End If
            End If
        Catch ex As Exception
            Log.Debug("Anagrafica.FunctionGrd.FormattaComune.errore: ", ex)
            ret = ""
        End Try
        Return ret.Trim
    End Function
End Class
'*** 201805 - Expire&Strong Password ***
''' <summary>
''' Definizione oggetto validità login
''' </summary>
Public Class InvalidLogin
    Dim _IP As String
    Dim _AttemptTime As DateTime
    Dim _AttemptCount As Integer
    Public Property IP As String
        Get
            Return _IP
        End Get
        Set
            _IP = Value
        End Set
    End Property
    Public Property AttemptTime As DateTime
        Get
            Return _AttemptTime
        End Get
        Set
            _AttemptTime = Value
        End Set
    End Property
    Public Property AttemptCount As Integer
        Get
            Return _AttemptCount
        End Get
        Set
            _AttemptCount = Value
        End Set
    End Property
End Class
'*** ***