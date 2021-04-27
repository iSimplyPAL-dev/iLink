
Imports System
Imports System.IO
Imports System.Web
Imports System.Web.UI
Imports log4net
Imports log4net.Config

Namespace OpUtenzeGC

    Public Class BasePageOLD
        Inherits System.Web.UI.Page
        Private Shared Log As ILog = LogManager.GetLogger(GetType(BasePageOLD))
        Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object

            'If (Me.IsReRequest()) Then
            'Dim viewState As Object = Me.LoadPageStateFromCache()
            'If (Not IsNothing(viewState)) Then
            'Return viewState
            'End If
            'Else
            '// determine the file to access
            Try
                If (Not File.Exists(ViewStateFilePath)) Then
                    Return Nothing
                Else
                    '// open the file
                    Dim sr As StreamReader = File.OpenText(ViewStateFilePath)
                    Dim viewStateString As String = sr.ReadToEnd()
                    sr.Close()
                    Dim los As New LosFormatter
                    Return los.Deserialize(viewStateString)
                End If
                'End If
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.BasePageOLD.LoadPageStateFromPersistenceMedium.errore: ", ex)
            End Try
        End Function

        ''' <summary>
        ''' Calls the inherited implementation, then saves the ViewState tothe application cache as well.
        ''' </summary>
        Protected Overrides Sub SavePageStateToPersistenceMedium(ByVal ViewStateBag As Object)

            '// serialize the view state into a base-64 encoded string
            Dim los As New LosFormatter
            Dim writer As New StringWriter

            los.Serialize(writer, ViewStateBag)

            Dim sw As StreamWriter = File.CreateText(ViewStateFilePath)
            sw.Write(writer.ToString())
            sw.Close()

            'Me.SavePageStateToCache(viewState)

        End Sub

        Public Function ViewStateFilePath() As String

            Dim folderName As String = Path.Combine(Request.PhysicalApplicationPath, "PersistedViewState")
            Dim fileName As String = Session.SessionID + "-" + Path.GetFileNameWithoutExtension(Request.Path).Replace("/", "-") + ".vs"
            Return Path.Combine(folderName, fileName)

        End Function

        Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

            'Dim RequestedUsername As String
            'RequestedUsername = Session("REQUESTED_USERNAME")

            'gestione per accesso diretto al sito senza passare dalla login
            Try
                If HttpContext.Current.Session("SESSIONE_SCADUTA") Is Nothing Then
                    'And (HttpContext.Current.Session("Operatore") = "" Or HttpContext.Current.Session("Operatore") Is Nothing) Then
                    'Response.Redirect(ConfigurationManager.AppSettings("WebSiteAddress") & "/login/login.aspx")
                    Response.Redirect(ConfigurationManager.AppSettings("WebSiteAddress") & "/Errors/PageSessionExpired.aspx")
                End If

                'gestione per la sessione scaduta
                If HttpContext.Current.Session("SESSIONE_SCADUTA") Is Nothing Then
                    Response.Redirect(ConfigurationManager.AppSettings("WebSiteAddress") & "/Errors/PageSessionExpired.aspx")
                End If


                'controllo congruenza sessionID
                'Dim objUtility As New Utility
                'If objMyUtility.CheckSessionId(RequestedUsername) = False Then
                '
                'Response.Redirect(ConfigurationManager.AppSettings("WebSiteAddress") & "/login/login.aspx")
                'End If
                '---------------------------------------------------------------------


                Dim pathfileinfo As String = ConfigurationManager.AppSettings("pathfileconflog4net").ToString()
                Dim fileconfiglog4net As FileInfo = New FileInfo(pathfileinfo)
                XmlConfigurator.ConfigureAndWatch(fileconfiglog4net)
            Catch ex As Exception
                Log.Debug(ConstSession.IdEnte +"."+ ConstSession.UserName + " - OPENgovH2O.BasePageOLD.Page_Load.errore: ", ex)
            End Try
        End Sub



    End Class
End Namespace