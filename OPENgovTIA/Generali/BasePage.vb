Imports System.Web.Security
Imports log4net
''' <summary>
''' Classe generale che eredita BasePage.
''' Viene inclusa in tutti i form perché contiene le funzioni basi che tutti i form devono avere.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class BaseEnte
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(BaseEnte))

    Private Sub BaseEnte_Init(sender As Object, e As EventArgs) Handles Me.Init
        Try
            If ConstSession.IdEnte = "" Then
                RegisterScript("GestAlert('a', 'warning', 'CmdLogOut', '', 'Sessione scaduta rieffettuare LOGIN');", Me.GetType)
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.BaseEnte.BasePage_Init.errore: ", ex)
        End Try
    End Sub
End Class
''' <summary>
''' Classe generale che eredita Page.
''' Viene inclusa in tutti i form perché contiene le funzioni basi che tutti i form devono avere.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Public Class BasePage
    Inherits Page
    Private Shared Log As ILog = LogManager.GetLogger(GetType(BasePage))
    Public bp_TipoRuolo As String = ""
    Public Shared authCookie As HttpCookie

    Private Sub BasePage_Init(sender As Object, e As EventArgs) Handles Me.Init
        Session("COD_TRIBUTO") = Utility.Costanti.TRIBUTO_TARSU
        Session("IsFromTARES") = "1"
        Try
            Dim isExpired As Boolean = False
            'get cookie
            authCookie = HttpContext.Current.Request.Cookies.Get("aplckute")
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
                                'If myData(0) <> ConstSession.UserName Then
                                Log.Debug("utente cookie->" + myData(0) + "  utente sessione->" + ConstSession.UserName)
                                If ConstSession.UserName <> "" Then
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
            If ConstSession.UserName = "" Then
                Log.Debug("sessione scaduta perchè sessione operatore vuoto")
                isExpired = True
            End If
            If isExpired = True Then
                HttpContext.Current.Session("username") = ""
                RegisterScript("GestAlert('a', 'warning', 'CmdLogOut', '', 'Sessione scaduta rieffettuare LOGIN');", Me.GetType)
            End If
            If ConstSession.Ambiente.ToUpper() = "CMGC" Then
                If ConstSession.IsFromVariabile("1") Then
                End If
            End If
        Catch ex As Exception
            Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovTIA.BasePage_Init.errore: ", ex)
        End Try
    End Sub

    Protected Sub RegisterScript(ByVal script As String, ByVal type As Type)
        ConstSession.CountScript = (ConstSession.CountScript + 1)
        Dim uniqueId As String = ("spc_" _
                    + (ConstSession.CountScript.ToString _
                    + (DateTime.Now.ToString + ("." + DateTime.Now.Millisecond.ToString))))
        Dim sScript As String = "<script language='javascript'>"
        sScript = (sScript + script)
        sScript = (sScript + "</script>")
        ClientScript.RegisterStartupScript(type, uniqueId, sScript)
    End Sub
End Class
