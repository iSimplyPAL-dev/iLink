Imports System.Xml
Imports System.Xml.Xsl
Imports System.Xml.XPath
Imports System.IO
Imports log4net
''' <summary>
''' Pagina per la costruzione del menù.
''' </summary>
''' <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
Partial Class aspmenu
    Inherits BasePage
    Private Shared Log As ILog = LogManager.GetLogger(GetType(aspmenu))

    'Dim WFSessione As New CreateSession
    Dim objSession As Object
    'Dim clsFile As New GestioneFile
    'Private GestErrore As New VisualizzaErrore

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
        LoginInfo.InnerHtml = "&nbsp;<label>Benvenuto " + COSTANTValue.ConstSession.UserName + "</label>&nbsp;<label Class='barra'>|</label>&nbsp;<button onclick='Chiudi();' class='logOff'>Disconnetti</button><br/><br/>"
    End Sub

#End Region
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here
        Try
            Log.Debug("aspmenu::ho ente=" + COSTANTValue.ConstSession.IdEnte)
            AddEntiToXML()
            If Not Page.IsPostBack Then
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte  +"."+ COSTANTValue.ConstSession.UserName + " - OPENgov.aspmenu.Page_Load.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub AddEntiToXML()
        Try
            Dim doc As New XmlDocument()
            doc.Load(COSTANTValue.ConstSession.GetRepositoryMenu + "xmlMenuSovracomunale.xml")

            Dim foo As XmlElement = doc.CreateElement("Menu")
            foo.SetAttribute("id", "Enti")
            foo.SetAttribute("text", "Seleziona Enti")

            Dim clSelectEnti As New SelectEnti
            Dim dsEnti As New DataSet
            dsEnti = clSelectEnti.GetEntiByUser(COSTANTValue.ConstSession.UserName)
            For Each drItem As DataRow In dsEnti.Tables(0).Rows
                Dim bar As XmlElement = doc.CreateElement("SubMenu")
                bar.SetAttribute("id", "ENTE" & drItem("COD_ISTAT").ToString())
                bar.SetAttribute("text", drItem("descrizione_ente").ToString().Replace("'", "’").ToLower & " - " & drItem("COD_ISTAT").ToString())
                bar.SetAttribute("NomeEnte", drItem("descrizione_ente").ToString().Replace("'", "’").ToLower)

                foo.AppendChild(bar)
            Next

            Dim barAll As XmlElement = doc.CreateElement("SubMenu")
            barAll.SetAttribute("id", "ENTE")
            barAll.SetAttribute("text", "Tutti")
            barAll.SetAttribute("NomeEnte", "Tutti")
            foo.AppendChild(barAll)

            doc.DocumentElement.AppendChild(foo)
            '*** 201801 - carico solo le applicazioni abilitate all'utente ***
            If InStr(COSTANTValue.ConstSession.ApplicationsEnabled, "CruscottoGen") <= 0 Then
                Dim myRoot As XmlElement = doc.DocumentElement
                Dim ListElement As XmlNodeList = myRoot.GetElementsByTagName("Menu")
                For Each myElement As XmlNode In ListElement
                    If myElement.Attributes(0).Value.IndexOf("CruscottoGen") >= 0 Then
                        myElement.ParentNode.RemoveChild(myElement)
                        Exit For
                    End If
                Next
            End If
            doc.Save(COSTANTValue.ConstSession.GetRepositoryMenu + "xmlMenu.xml")
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte + "." + COSTANTValue.ConstSession.UserName + " - OPENgov.aspmenu.AddEntiToXML.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub LoadMenuEnte(sender As Object, e As EventArgs)
        Try
            Dim docA As New XmlDocument()
            Dim ListToDelete As New ArrayList
            docA.Load(COSTANTValue.ConstSession.GetRepositoryMenu + "xmlMenu.xml")

            If hfIdEnte.Value <> "" Then
                Dim fncActionEvent As New Utility.DBUtility(COSTANTValue.ConstSession.DBType, COSTANTValue.ConstSession.StringConnectionOPENgov)
                fncActionEvent.LogActionEvent(DateTime.Now, COSTANTValue.ConstSession.UserName, "Login", "aspMenu", "selezione ente", "", hfIdEnte.Value, -1)

                Dim FncLog As New SelectEnti
                FncLog.LoginEnte(hfIdEnte.Value, "")
                Dim docB As New XmlDocument()
                docB.Load(COSTANTValue.ConstSession.GetRepositoryMenu + "xmlMenuEnte.xml")
                For Each childEl As Object In docB.DocumentElement.ChildNodes
                    '*** 201801 - carico solo le applicazioni abilitate all'utente ***
                    If InStr(COSTANTValue.ConstSession.ApplicationsEnabled, DirectCast(childEl, XmlElement).Attributes(0).Value) > 0 Then
                        Dim newNode As XmlNode = docA.ImportNode(childEl, True)
                        newNode.InnerXml = newNode.InnerXml.Replace("myidente", hfIdEnte.Value).Replace("+", "&")
                        docA.DocumentElement.AppendChild(newNode)
                        If COSTANTValue.ConstSession.Profilo = COSTANTValue.ConstSession.TipoProfilo.SingolaVoceLettura Then
                            For Each InnerChild As Object In childEl
                                If InStr(COSTANTValue.ConstSession.ApplicationsEnabled, DirectCast(InnerChild, XmlElement).Attributes(0).Value) <= 0 Then
                                    ListToDelete.Add(DirectCast(InnerChild, XmlElement).Attributes(0).Value)
                                End If
                            Next
                        End If
                    Else
                        ListToDelete.Add(DirectCast(childEl, XmlElement).Attributes(0).Value)
                    End If
                Next
            End If
            If COSTANTValue.ConstSession.HasNotifiche = False Then
                ListToDelete.Add("Notifica")
                ListToDelete.Add("Coattivo")
            End If
            '*** 201801 - carico solo le voci relative ad applicazioni abilitate all'utente ***
            CheckAppEnabled(docA, ListToDelete)
            docA.Save(COSTANTValue.ConstSession.GetRepositoryMenu + "xmlMenu.xml")
            Session("COD_ENTE") = hfIdEnte.Value
            Session("DESCRIZIONE_ENTE") = hfDescrizioneEnte.Value
            Session("COD_TRIBUTO") = hfIdTributo.Value
            If hfIdEnte.Value = "" Then
                Session("FromSovracomunali") = "S"
            Else
                Session("FromSovracomunali") = "N"
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte + "." + COSTANTValue.ConstSession.UserName + " - OPENgov.aspmenu.LoadMenuEnte.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="docA"></param>
    ''' <param name="ListToDelete"></param>
    Private Sub CheckAppEnabled(docA As XmlDocument, ListToDelete As ArrayList)
        Try
            Dim myRoot As XmlElement = docA.DocumentElement
            Dim ListElement As XmlNodeList = myRoot.GetElementsByTagName("SubMenu")
            For Each myElement As XmlNode In ListElement
                For Each AppToDelete As String In ListToDelete
                    If myElement.Attributes(0).Value.IndexOf(AppToDelete) >= 0 Then
                        myElement.ParentNode.RemoveChild(myElement)
                        CheckAppEnabled(docA, ListToDelete)
                        Exit Sub
                    End If
                Next
            Next
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte + "." + COSTANTValue.ConstSession.UserName + " - OPENgov.aspmenu.CheckAppEnabled.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub LogOut(sender As Object, e As EventArgs)
        Try
            If New UtilityOPENgov().LogOff(COSTANTValue.ConstSession.UserName) Then
                Dim cookie1 As New HttpCookie(Web.Security.FormsAuthentication.FormsCookieName, "")
                cookie1.Expires = DateTime.Now.AddYears(-1)
                Response.Cookies.Add(cookie1)
                cookie1 = New HttpCookie("aplckute", "")
                cookie1.Expires = DateTime.Now.AddYears(-1)
                Response.Cookies.Add(cookie1)

                'clear session cookie
                Dim sessionStateSection As Web.Configuration.SessionStateSection = CType(Web.Configuration.WebConfigurationManager.GetSection("system.web/sessionState"), Web.Configuration.SessionStateSection)
                Dim cookie2 As New HttpCookie(sessionStateSection.CookieName, "")
                cookie2.Expires = DateTime.Now.AddYears(-1)
                Response.Cookies.Add(cookie2)

                HttpContext.Current.Session("username") = ""
                RegisterScript("parent.location.href = '" & Request.Url.GetLeftPart(UriPartial.Authority) & "/" & Request.ApplicationPath & "/Default.aspx';", Me.GetType)
            Else
                RegisterScript("GestAlert('a', 'warning', '', '', 'Errore in logout!');", Me.GetType())
            End If
        Catch ex As Exception
            Log.Debug(COSTANTValue.ConstSession.IdEnte + "." + COSTANTValue.ConstSession.UserName + " - OPENgov.aspmenu.LogOut.errore: ", ex)
            Response.Redirect("../../PaginaErrore.aspx")
        End Try
    End Sub
End Class

